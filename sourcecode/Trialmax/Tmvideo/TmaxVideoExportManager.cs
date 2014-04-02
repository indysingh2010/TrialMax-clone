using System;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Forms;
using FTI.Trialmax.Encode;

namespace FTI.Trialmax.TMVV.Tmvideo
{
	/// <summary>This class manages the export operations for the database</summary>
	public class CTmaxVideoExportManager
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_INITIALIZE_EX				= 0;
		private const int ERROR_GET_EXPORT_STREAM_EX		= 1;
		private const int ERROR_EXPORT_EX					= 2;
		private const int ERROR_CREATE_STATUS_FORM_EX		= 3;
		private const int ERROR_FILE_OPEN_FAILED			= 4;
		private const int ERROR_EXPORT_AS_TEXT_EX			= 5;
		private const int ERROR_GET_VIDEO_OPTIONS_EX		= 6;
		private const int ERROR_GET_EXPORT_FILESPEC_EX		= 7;
		private const int ERROR_EXPORT_AS_WMV_EX			= 8;
		private const int ERROR_INITIALIZE_WMV_ENCODER		= 9;
		private const int ERROR_SET_WMV_PROFILE				= 10;
		private const int ERROR_ADD_WMV_SOURCE				= 11;
		private const int ERROR_EXECUTE_WMV_ENCODER			= 12;
		private const int ERROR_EXPORT_AS_SAMI_EX			= 13;
		private const int ERROR_EXPORT_AS_SAMI_FAILED		= 14;
		private const int ERROR_VIDEO_FILE_NOT_FOUND		= 15;
		private const int ERROR_EXPORT_THREAD_PROC_EX		= 16;
		private const int ERROR_EXPORT_WMV_INCOMPLETE		= 17;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bounded to EventSource property</summary>
		protected FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member used to construct error messages</summary>
		protected FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();
		
		/// <summary>Local member bound to AppOptions property</summary>
		protected CTmaxVideoOptions m_tmaxAppOptions = null;
		
		/// <summary>Local member bound to Format property</summary>
		private TmaxExportFormats m_eFormat = TmaxExportFormats.Unknown;
		
		/// <summary>Local member to reference the owner script</summary>
		private FTI.Shared.Xml.CXmlScript m_xmlScript = null;
		
		/// <summary>Local member to store the designations being exported</summary>
		private FTI.Shared.Xml.CXmlDesignations m_xmlDesignations = new CXmlDesignations();		
		
		/// <summary>Local member bound to StatusForm property</summary>
		private CFExportStatus m_wndStatus = null;
		
		/// <summary>Local member bound to Stream property</summary>
		private System.IO.StreamWriter m_fileStream = null;
		
		/// <summary>Local member bound to FileSpec property</summary>
		private string m_strFileSpec = "";
		
		/// <summary>Filter string used in file selection dialog</summary>
		private string m_strFileFilter = "";
		
		/// <summary>Default file extension in file selection dialog</summary>
		private string m_strExtension = "";
		
		/// <summary>Total number of designations that have been exported</summary>
		private long m_lExported = 0;
		
		/// <summary>Flag to indicate if the operation should be terminated</summary>
		private bool m_bTerminate = false;
		
		/// <summary>Flag to indicate if the operation was aborted by the user</summary>
		private bool m_bAborted = false;
		
		/// <summary>Flag to indicate if the user should be prompted before deleting output file</summary>
		private bool m_bConfirmOverwrite = true;
		
		/// <summary>Fully qualified path to the ouput folder</summary>
		private string m_strExportFolder = "";
		
		/// <summary>Private member bound to WMEncoder property</summary>
		private FTI.Trialmax.Encode.CWMEncoder m_tmaxEncoder = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxVideoExportManager()
		{
			//	Initialize the event source and error builder
			SetErrorStrings();
			m_tmaxEventSource.Name = "Export Manager";
			
		}// public CTmaxVideoExportManager()
		
		/// <summary>This method uses the specified parameters to set the associated properties</summary>
		/// <param name="tmaxSource">Item used to identify the source designations</param>
		/// <param name="tmaxParameters">The collection of parameters</param>
		/// <returns>true if successful</returns>
		public bool Initialize(CTmaxItem tmaxSource, CTmaxParameters tmaxParameters)
		{
			CTmaxParameter	tmaxParameter = null;

			//	Reset the current values to their defaults
			Reset();
			m_bAborted = false;
			
			try
			{
				//	Get the export format
				if((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.ExportFormat)) != null)
				{
					try	  { m_eFormat = (TmaxExportFormats)(tmaxParameter.AsInteger()); }
					catch {}
				}

				//	Set the format specific defaults
				switch(m_eFormat)
				{
					case TmaxExportFormats.Video:
					
						//						m_strExtension = "edl";
						//						m_strFileFilter = "EDL Files (*.edl)|*.edl|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
						m_strExtension = "";
						m_strFileFilter = "";
						break;
						
					case TmaxExportFormats.AsciiMedia:
					case TmaxExportFormats.Transcript:
					default:
					
						m_strExtension = "txt";
						m_strFileFilter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
						break;
						
				}// switch(this.Format)
				
				//	Set the source designations
				return SetSource(tmaxSource);		
			}
			catch
			{
				OnError(m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_EX));
				return false;
			}
			
		}// public bool Initialize(CTmaxItem tmaxSource, CTmaxParameters tmaxParameters)
		
		/// <summary>This method is called to execute the export operation</summary>
		/// <param name="tmaxSource">Item used to identify the source designations</param>
		/// <param name="tmaxParameters">The collection of parameters</param>
		/// <returns>true if successful</returns>
		public bool Export(CTmaxItem tmaxSource, CTmaxParameters tmaxParameters)
		{
			//	Initialize the operation
			if(Initialize(tmaxSource, tmaxParameters) == false)
				return false;

			//	Prompt for the user defined options
			if(m_eFormat == TmaxExportFormats.Video)
			{
				if(GetVideoOptions() == false)
					return false;
			}
		
			//	Create the status form for the operation
			if(this.CreateStatusForm() == false) return false;
			
			try
			{
				ExportThreadProc();
				/*
				//	Start the operation
				exportThread = new Thread(new ThreadStart(this.ExportThreadProc));
				exportThread.Start();
					
				//	Block the caller until operation is complete or the user cancels
				StatusForm.ShowDialog();
			
				//	Wait for thread to be terminated
				while(exportThread.ThreadState == System.Threading.ThreadState.Running)
				{
					Thread.Sleep(500);
					
					//	Crude test for timeout
					if(iAttempts < 20)
						iAttempts++;
					else
						break;
				}
*/			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Export", m_tmaxErrorBuilder.Message(ERROR_EXPORT_EX), Ex);
				AddMessage(m_tmaxErrorBuilder.Message(ERROR_EXPORT_THREAD_PROC_EX), TmaxMessageLevels.FatalError);
			}
			
			return (m_lExported > 0);
			
		}// public bool Export(CTmaxItem tmaxSource, CTmaxParameters tmaxParameters)
		
		/// <summary>This method is called to add a message to the status form list</summary>
		/// <param name="strMessage">The message to be added</param>
		/// <param name="eType">Enumerated message type to define error level</param>
		public void AddMessage(string strMessage, TmaxMessageLevels eType)
		{
			try
			{
				if((m_wndStatus != null) && (m_wndStatus.IsDisposed == false))
				{
					m_wndStatus.AddMessage(strMessage, m_wndStatus.Filename, eType);
				}
					
			}
			catch
			{
			}
		
		}// public void AddMessage(string strMessage, TmaxMessageLevels eType)
		
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>This method is called to execute the export thread</summary>
		public void ExportThreadProc()
		{
			try
			{
				//	What are we exporting?
				switch(m_eFormat)
				{
					case TmaxExportFormats.AsciiMedia:
					case TmaxExportFormats.Transcript:
				
						ExportAsText();
						break;
					
					case TmaxExportFormats.Video:
				
						ExportVideo();
						break;
					
				
					default:
				
						Debug.Assert(false, m_eFormat.ToString() + " is not a valid format for the export operation");
						break;
					
				}// switch(m_eFormat)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "ExportThreadProc", m_tmaxErrorBuilder.Message(ERROR_EXPORT_THREAD_PROC_EX), Ex);
				AddMessage(m_tmaxErrorBuilder.Message(ERROR_EXPORT_THREAD_PROC_EX), TmaxMessageLevels.FatalError);
			}
			
			//	Notify the status form
			if(m_wndStatus != null)
			{
				FTI.Shared.Win32.User.MessageBeep(0);
			
				if(m_wndStatus.Aborted == false)
				{
					m_wndStatus.Finished = true;
					SetStatus("Export operation complete");
				}
			
			}// if(this.StatusForm != null)
		
		}// public void Export()
		
		/// <summary>This method is called to export the source records to text</summary>
		/// <returns>True if successful</returns>
		private bool ExportAsText()
		{
			bool	bSuccessful = true;
			bool	bAddLine = false;
			
			if(m_xmlDesignations == null) return false;
			if(m_xmlDesignations.Count == 0) return false;
			
			//	Make the status form visible
			SetStatusVisible(true, "Exporting " + m_xmlDesignations.Count.ToString() + " designations ...");
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				while(true)
				{
					if(CheckAborted() == true)
						break;
						
					//	Open the file stream for this operation
					if(GetExportStream(m_xmlScript) == false)
						break;
						
					//	Export each of the source designations
					foreach(CXmlDesignation O in m_xmlDesignations)
					{
						if(m_eFormat == TmaxExportFormats.AsciiMedia)
						{
							if(ExportDesignation(O) == false)
							{
								bSuccessful = false;
								break;
							}
						
						}
						else if(m_eFormat == TmaxExportFormats.Transcript)
						{
							//	Do we need to add a blank line?
							if(bAddLine == true)
								m_fileStream.WriteLine("");

							if(ExportTranscript(O) == false)
							{
								bSuccessful = false;
								break;
							}
							else
							{
								//	Separate designations with a blank line
								bAddLine = true;
							}
						
						}
						else
						{
							Debug.Assert(false, "Unknown export format -> " + m_eFormat.ToString());
						}
				
					}// foreach(CTmaxItem O in m_tmaxSource)
				
					//	We're done
					break;
					
				}// while(true)
			
			}
			catch
			{
				OnError(m_tmaxErrorBuilder.Message(ERROR_EXPORT_AS_TEXT_EX, m_eFormat));
			}
			finally
			{
				Cursor.Current = Cursors.Default;

				//	Close the file stream
				CloseStream();
			}
				
			if(bSuccessful == true)
				SetSummary(String.Format("Exported {0} designations to {1}", m_lExported, System.IO.Path.GetFileName(m_strFileSpec)));
			
			return bSuccessful;
			
		}// private bool ExportAsText()
		
		/// <summary>This method is called to export the specified script to video</summary>
		/// <returns>True if operation should continue</returns>
		private bool ExportVideo()
		{
			bool			bSuccessful = false;
			string			strFileSpec = "";
			
			Debug.Assert(m_xmlScript != null);
			Debug.Assert(m_xmlDesignations != null);
			Debug.Assert(m_xmlDesignations.Count > 0);
			
			//	Make the status form visible
			SetStatusVisible(true, "Exporting " + m_xmlDesignations.Count.ToString() + " designations to video ...");
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				while(true)
				{
					//	Get the path to the export file(s)
					strFileSpec = GetExportFileSpec(m_xmlScript);
					if((strFileSpec == null) || (strFileSpec.Length == 0))
						break;
						
					//	Are we exporting to WMV?
					if((CheckAborted() == false) && (m_tmaxAppOptions.ExportOptions.VideoWMV == true))
					{
						if(ExportAsWMV(strFileSpec) == true)
						{
							bSuccessful = true;
						}
						
					}
					
					//	Are we exporting to EDL?
					if((CheckAborted() == false) && (m_bTerminate == false) && (m_tmaxAppOptions.ExportOptions.VideoEDL == true))
					{
						if(ExportAsEDL(strFileSpec) == true)
						{
							bSuccessful = true;
						}
					
					}
					
					//	Are we exporting to SAMI?
					if((CheckAborted() == false) && (m_bTerminate == false) && (m_tmaxAppOptions.ExportOptions.VideoSAMI == true))
					{
						if(ExportAsSAMI(strFileSpec) == true)
						{
							bSuccessful = true;
						}
						
					}
					
					if(bSuccessful == true)
						m_lExported += m_xmlDesignations.Count;
						
					//	We're done
					break;
				}
				
			}
			catch
			{
			}	
			finally
			{
			}
			
			//	Turn off the wait cursor
			Cursor.Current = Cursors.Default;
				
			if(bSuccessful == true)
				SetSummary(String.Format("Exported {0} designations", m_lExported));

			return bSuccessful;
			
		}// private bool ExportVideo()
		
		/// <summary>This method is called to export the specified script to a WMV file</summary>
		/// <param name="dxScript">The script to be exported</param>
		/// <param name="dxScenes">The collection of scenes to be exported</param>
		/// <param name="strFileSpec">The path to the export file</param>
		/// <returns>True if successful</returns>
		private bool ExportAsWMV(string strFileSpec)
		{
			CWMEncoder	wmEncoder = null;
			bool		bSuccessful = false;
			bool		bEncode = true;
			string		strVideo = "";
			int			iId = 1;
			
			Debug.Assert(m_xmlScript != null);
			Debug.Assert(m_xmlDesignations != null);
			Debug.Assert(m_xmlDesignations.Count > 0);
			Debug.Assert(strFileSpec != null);
			Debug.Assert(strFileSpec.Length > 0);
			
			//	Make the status form visible
			SetStatus("Exporting " + m_xmlDesignations.Count.ToString() + " designations to WMV ...");

			try
			{
				//	Substitute the appropriate extension for the WMV file
				strFileSpec = System.IO.Path.ChangeExtension(strFileSpec, ".wmv");
				
				//	Delete the existing file
				if(System.IO.File.Exists(strFileSpec) == true)
				{
					try { System.IO.File.Delete(strFileSpec); }
					catch {}
				}
				
				SetStatusFilename(System.IO.Path.GetFileName(strFileSpec));
				
				//	Use our Windows Media Encoder wrapper to perform the operation
				wmEncoder = new CWMEncoder();
				m_tmaxEventSource.Attach(wmEncoder.EventSource);
				wmEncoder.EncoderStatusUpdate += new FTI.Trialmax.Encode.CWMEncoder.EncoderStatusHandler(this.OnEncoderStatus);
				wmEncoder.ShowCancel = false;
				
				while(bSuccessful == false)
				{
					//	Initialize for this operation
					if(wmEncoder.Initialize(strFileSpec) == false)
					{
						OnError(m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_WMV_ENCODER, strFileSpec), false);
						break;
					}
				
					//	Set the encoder profile
					if(wmEncoder.SetProfile(m_tmaxEncoder.LastProfile) == false)
					{
						OnError(m_tmaxErrorBuilder.Message(ERROR_SET_WMV_PROFILE, m_tmaxEncoder.LastProfile), false);
						break;
					}
					
					//	Add a source descriptor for each designation
					foreach(CXmlDesignation O in m_xmlDesignations)
					{
						//	Get the path to the video source
						strVideo = m_tmaxAppOptions.GetVideoFileSpec(m_xmlScript, O, false, true);
								
						//	Add a source
						if(strVideo.Length > 0)
						{
							//	Make sure the video exists
							if(System.IO.File.Exists(strVideo) == true)
							{
								if(wmEncoder.AddSource("D" + iId.ToString(), strVideo, O.Start, O.Stop) == false)
								{
									OnError(m_tmaxErrorBuilder.Message(ERROR_ADD_WMV_SOURCE, O.GetDisplayString()), false);
									bEncode = false;
									break;
								}
								else
								{
									iId++;
								}
							
							}
							else
							{
								OnError(m_tmaxErrorBuilder.Message(ERROR_VIDEO_FILE_NOT_FOUND, O.GetDisplayString(), strVideo), false);
								bEncode = false;
								break;
							
							}// if(System.IO.File.Exists(strVideo) == true)
							
						}// if((strVideo.Length > 0) && (dxTertiary.GetExtent() != null))

					}// foreach(CDxSecondary O in dxScenes)
					
					//	Should we skip encoding
					if(bEncode == false) break;
					
					if(wmEncoder.Encode() == false)
					{
						if(wmEncoder.Cancelled == false)
						{
							if(wmEncoder.Completed < wmEncoder.SourceGroups.Count)
								OnError(m_tmaxErrorBuilder.Message(ERROR_EXPORT_WMV_INCOMPLETE, System.IO.Path.GetFileNameWithoutExtension(m_xmlScript.FileSpec), wmEncoder.Completed, wmEncoder.SourceGroups.Count), false);
							else
								OnError(m_tmaxErrorBuilder.Message(ERROR_EXECUTE_WMV_ENCODER, System.IO.Path.GetFileNameWithoutExtension(m_xmlScript.FileSpec)), false);
						}
						break;
					}
					
					//	We're done
					bSuccessful = true;
					
				}//	while(bSuccessful = false)					
				
			}
			catch
			{
				OnError(m_tmaxErrorBuilder.Message(ERROR_EXPORT_AS_WMV_EX, m_xmlScript.Name), false);
			}
			finally
			{
				//	Clean up
				if(wmEncoder != null)
				{
					wmEncoder.Clear();
					wmEncoder = null;
				}

			}	
			
			return bSuccessful;
			
		}// private bool ExportAsWMV(string strFileSpec)
		
		/// <summary>This method is called to export the specified script to an EDL clip descriptor file</summary>
		/// <param name="dxScript">The script to be exported</param>
		/// <param name="dxScenes">The collection of scenes to be exported</param>
		/// <param name="strFileSpec">The path to the export file</param>
		/// <returns>True if successful</returns>
		private bool ExportAsEDL(string strFileSpec)
		{
			bool			bSuccessful = false;
			CXmlSegments	xmlSegments = null;
			CXmlSegment		xmlSegment = null;
			string			strLine = "";
			string			strFileTag = "";

			Debug.Assert(m_xmlScript != null);
			Debug.Assert(m_xmlDesignations != null);
			Debug.Assert(m_xmlDesignations.Count > 0);
			Debug.Assert(strFileSpec != null);
			Debug.Assert(strFileSpec.Length > 0);
			
			//	Make the status form visible
			SetStatus("Exporting " + m_xmlScript.Name + " to EDL ...");

			try
			{
				//	Substitute the appropriate extension for the WMV file
				strFileSpec = System.IO.Path.ChangeExtension(strFileSpec, ".edl");
				
				//	Delete the existing file
				if(System.IO.File.Exists(strFileSpec) == true)
				{
					try { System.IO.File.Delete(strFileSpec); }
					catch {}
				}
				
				SetStatusFilename(System.IO.Path.GetFileName(strFileSpec));
				
				//	Allocate the working collections
				xmlSegments = new CXmlSegments();
						
				//	Build a list of all required segments
				foreach(CXmlDesignation O in m_xmlDesignations)
				{
					//	Get the segment associated with this designation
					if((xmlSegment = m_xmlScript.GetSegment(O.Segment)) != null)
					{
						if(xmlSegments.Contains(xmlSegment) == false)
							xmlSegments.Add(xmlSegment);
					}
						
				}// foreach(CXmlDesignation O in m_xmlDesignations)
				
				//	Make sure the user has not aborted the operation
				if(CheckAborted() == true) return false;
				
				//	Open the new stream
				if(OpenStream(strFileSpec) == false) 
					return false;

				//	Write the EDL header
				m_fileStream.WriteLine("# TYPE=EDL");
				m_fileStream.WriteLine("# VERSION=3.0.0");
				m_fileStream.WriteLine("");
						
				//	Write the video file segments
				m_fileStream.WriteLine("# VIDEO FILE TAGS");
				m_fileStream.WriteLine("");
				foreach(CXmlSegment O in xmlSegments)
				{
					if(m_xmlScript.XmlDeposition.Name.Length > 0)
						strFileTag = String.Format("{0}.{1}", m_xmlScript.XmlDeposition.Name, O.Key);
					else
						strFileTag = String.Format("{0}.{1}", "S", O.Key);
					strFileTag = strFileTag.Replace(' ', '_');
								
					m_fileStream.WriteLine("# " + strFileTag + " = " + m_tmaxAppOptions.GetVideoFileSpec(m_xmlScript, O, false, true));
				
					//	Store temporarily to prevent having to regenerate
					O.UserData = String.Format("{0}", strFileTag);
				}
				m_fileStream.WriteLine("");
				m_fileStream.WriteLine("");
							
				//	Write the designations to file
				foreach(CXmlDesignation O in m_xmlDesignations)
				{
					if((xmlSegment = xmlSegments.Locate(O.Segment)) != null)
					{
						strLine = String.Format("{0} VA C {1:.#}s {2:.#}s",
							xmlSegment.UserData,
							O.Start,
							O.Stop);
								
						m_fileStream.WriteLine("# (" + O.GetDisplayString() + ")");
						m_fileStream.WriteLine(strLine);
						m_fileStream.WriteLine("");
					
					}
								
				}// foreach(CXmlDesignation O in m_xmlDesignations)
						
				bSuccessful = true;
				
			}
			catch
			{
			}	
			finally
			{
				CloseStream();
			}
			
			return bSuccessful;
			
		}// private bool ExportEDL(string strFileSpec)
		
		/// <summary>This method is called to export the source designations to an SAMI clip descriptor file</summary>
		/// <param name="strFileSpec">The path to the export file</param>
		/// <returns>True if successful</returns>
		private bool ExportAsSAMI(string strFileSpec)
		{
			CTmaxHighlighter		tmaxHighlighter = null;
			CWMSAMI					wmSAMI = null;
			bool					bSuccessful = false;
			bool					bSceneError = false;
			System.Drawing.Color	highlighterColor = System.Drawing.Color.Yellow;
			string					strMessage = "";
			string					strName = "";
			
			Debug.Assert(m_xmlScript != null);
			Debug.Assert(m_xmlDesignations != null);
			Debug.Assert(m_xmlDesignations.Count > 0);
			Debug.Assert(strFileSpec != null);
			Debug.Assert(strFileSpec.Length > 0);
			
			//	Make the status form visible
			SetStatus("Exporting " + m_xmlDesignations.Count.ToString() + " designations to SAMI ...");
			
			try
			{
				//	Substitute the appropriate extension for the SAMI file
				strFileSpec = System.IO.Path.ChangeExtension(strFileSpec, CWMSAMI.SAMI_DEFAULT_EXTENSION);
				
				//	Delete the existing file
				if(System.IO.File.Exists(strFileSpec) == true)
				{
					try { System.IO.File.Delete(strFileSpec); }
					catch {}
				}
				
				SetStatusFilename(System.IO.Path.GetFileName(strFileSpec));
				
				//	What name do we want to use for the conversion?
				if(m_xmlScript.Name.Length > 0)
					strName = m_xmlScript.Name;
				else if(m_xmlScript.MediaId.Length > 0)
					strName = m_xmlScript.MediaId;
				else
					strName = System.IO.Path.GetFileNameWithoutExtension(strFileSpec);
					
				//	Use our custom encoder wrapper to perform the operation
				wmSAMI = new CWMSAMI();
				m_tmaxEventSource.Attach(wmSAMI.EventSource);
			
				//	Set the user defined options
				wmSAMI.FontFamily = m_tmaxAppOptions.ExportOptions.SAMIFontFamily;
				wmSAMI.FontColor = m_tmaxAppOptions.ExportOptions.SAMIFontColor;
				wmSAMI.FontSize = m_tmaxAppOptions.ExportOptions.SAMIFontSize;
				wmSAMI.FontHighlighter = m_tmaxAppOptions.ExportOptions.SAMIFontHighlighter;
				wmSAMI.VisibleLines = m_tmaxAppOptions.ExportOptions.SAMILines;
				wmSAMI.PageNumbers = m_tmaxAppOptions.ExportOptions.SAMIPageNumbers;
				
				while(bSuccessful == false)
				{
					//	Make sure the user has not aborted the operation
					if(CheckAborted() == true) return false;
				
					//	Initialize for this operation
					if(wmSAMI.Initialize(strName, strFileSpec) == false)
					{
						strMessage = wmSAMI.GetLastErrorMessage(false);
						if(strMessage.Length == 0)
							strMessage = "The attempt to initialize the SAMI converter object failed";
						break;
					}
				
					//	Add a source descriptor for each designation
					foreach(CXmlDesignation O in m_xmlDesignations)
					{
						//	Get the highlighter color
						try
						{
							if((m_tmaxAppOptions.Highlighters != null) && (m_tmaxAppOptions.Highlighters.Count > 0))
							{
								if((tmaxHighlighter = m_tmaxAppOptions.Highlighters.Find(O.Highlighter)) == null)
									tmaxHighlighter = m_tmaxAppOptions.Highlighters[0];	
							}
							
							if(tmaxHighlighter != null)
								highlighterColor = tmaxHighlighter.Color;
							else
								highlighterColor = System.Drawing.Color.Yellow;
						}
						catch
						{
						}
							
						//	Add a source
						if(wmSAMI.AddSource(O, highlighterColor) == null)
						{
							bSceneError = true;
							strMessage = wmSAMI.GetLastErrorMessage(false);
							break;
						}

					}// foreach(CXmlDesignation O in m_xmlDesignations)
					
					//	Should we skip encoding
					if(bSceneError == true) break;
					
					if(wmSAMI.Save() == false)
					{
						strMessage = wmSAMI.GetLastErrorMessage(false);
						break;
					}
					
					//	We're done
					bSuccessful = true;
					
				}//	while(bSuccessful = false)		
				
				if(bSuccessful == false)
				{
					OnError(m_tmaxErrorBuilder.Message(ERROR_EXPORT_AS_SAMI_FAILED, strName, strMessage), false);
				}			
				
			}
			catch
			{
				OnError(m_tmaxErrorBuilder.Message(ERROR_EXPORT_AS_SAMI_EX, strName), false);
			}
			finally
			{
				//	Clean up
				if(wmSAMI != null)
				{
					wmSAMI.Clear();
					wmSAMI = null;
				}
				
			}	
			
			return bSuccessful;
			
		}// private bool ExportAsSAMI(string strFileSpec)
		
		/// <summary>This method is called to export the specified designation</summary>
		/// <param name="xmlDesignation">The designation to be exported</param>
		/// <returns>True if successful</returns>
		private bool ExportDesignation(CXmlDesignation xmlDesignation)
		{
			bool		bSuccessful = false;
			string		strLine = "";

			Debug.Assert(m_fileStream != null);
			
			//	Make the status form visible
			SetStatus("Exporting " + xmlDesignation.GetDisplayString());

			try
			{
				//	Format the line
				strLine = String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
					CTmaxToolbox.PLToPage(xmlDesignation.FirstPL),
					CTmaxToolbox.PLToLine(xmlDesignation.FirstPL),
					CTmaxToolbox.PLToPage(xmlDesignation.LastPL),
					CTmaxToolbox.PLToLine(xmlDesignation.LastPL),
					m_xmlScript.XmlDeposition.Name,
					xmlDesignation.Highlighter);
						
				//	Add the tuning information
				if(xmlDesignation.StartTuned == true)
					strLine += ("\t" + xmlDesignation.Start.ToString());
				else
					strLine += ("\t-1.0");
				if(xmlDesignation.StopTuned == true)
					strLine += ("\t" + xmlDesignation.Stop.ToString());
				else
					strLine += ("\t-1.0");
							
				m_fileStream.WriteLine(strLine);
				m_lExported++;
				
				bSuccessful = true;
			}
			catch
			{
			}	
			finally
			{
			}
			
			return bSuccessful;
			
		}// private bool ExportDesignation(CXmlDesignation xmlDesignation)
		
		/// <summary>This method is called to export the transcript text bound to the specified designation</summary>
		/// <param name="xmlDesignation">The designation that owns the desired text</param>
		/// <returns>True if successful</returns>
		private bool ExportTranscript(CXmlDesignation xmlDesignation)
		{
			bool		bSuccessful = false;
			string		strLine = "";

			Debug.Assert(m_fileStream != null);
			
			//	Make the status form visible
			SetStatus("Exporting " + xmlDesignation.GetDisplayString());

			try
			{
				if(xmlDesignation.Transcripts == null) return false;
				
				foreach(CXmlTranscript O in xmlDesignation.Transcripts)
				{
					//	Format the line
					strLine = String.Format("{0}:{1:00}\t{2,3} {3}",
											O.Page,
											O.Line,
											O.QA,
											O.Text);
								
					m_fileStream.WriteLine(strLine);
					
				}// foreach(CXmlTranscript O in xmlDesignation.Transcripts)
				
				m_lExported++;
				bSuccessful = true;
			}
			catch
			{
			}	
			finally
			{
			}
			
			return bSuccessful;
			
		}// private bool ExportTranscript(CXmlDesignation xmlDesignation)
		
		/// <summary>This method sets the source collection for the operation</summary>
		/// <param name="tmaxSource">Collection of event items that identify the source records</param>
		/// <returns>true if successful</returns>
		private bool SetSource(CTmaxItem tmaxSource)
		{
			if((m_xmlScript = tmaxSource.XmlScript) == null)
				return false;
				
			//	Did the caller specify particular designations?
			if((tmaxSource.SubItems != null) && (tmaxSource.SubItems.Count > 0))
			{
				foreach(CTmaxItem O in tmaxSource.SubItems)
				{
					if(O.XmlDesignation != null)
						m_xmlDesignations.Add(O.XmlDesignation);
				}
				
			}
			else
			{
				//	Add all designations owned by the script
				foreach(CXmlDesignation O in m_xmlScript.XmlDesignations)
				{
					m_xmlDesignations.Add(O);
				}
				
			}
			
			//	Warn the user if nothing to export
			if(m_xmlDesignations.Count == 0)
				MessageBox.Show("No designations to be exported", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
			
			return (m_xmlDesignations.Count > 0);
			
		}// private bool SetSource(CTmaxItem tmaxSource)
		
		/// <summary>This method is called to create the status form for the operation</summary>
		/// <returns>true if successful</returns>
		private bool CreateStatusForm()
		{
			try
			{
				//	Clear the Aborted flag
				m_bAborted = false;
				
				//	Make sure the previous instance is disposed
				if(m_wndStatus != null) 
				{
					if(m_wndStatus.IsDisposed == false)
						m_wndStatus.Dispose();
					m_wndStatus = null;
				}
				
				//	Create a new instance
				m_wndStatus = new FTI.Trialmax.Forms.CFExportStatus();
				m_wndStatus.Title = "Export Status";
				
				//	Set the initial status message
				SetStatus("Initializing export operation ...");
			}
			catch
			{
				OnError(m_tmaxErrorBuilder.Message(ERROR_CREATE_STATUS_FORM_EX));
				m_wndStatus = null;
			}
			
			return (m_wndStatus != null);
		
		}// private bool CreateStatusForm()
		
		/// <summary>This method is called to update the status text on the status form</summary>
		/// <param name="strStatus">The new status message</param>
		private void SetStatus(string strStatus)
		{
			try
			{
				if((m_wndStatus != null) && (m_wndStatus.IsDisposed == false))
				{
					m_wndStatus.Status = strStatus;
					m_wndStatus.Refresh();
				}
				
			}
			catch
			{
			}
			
		}// private void SetStatus(string strStatus)
		
		/// <summary>This method is called to show/hide the status form</summary>
		/// <param name="bVisible">true if visible</param>
		/// <param name="strStatus">optional status message </param>
		private void SetStatusVisible(bool bVisible, string strStatus)
		{
			try
			{
				if((m_wndStatus != null) && (m_wndStatus.IsDisposed == false))
				{
					if(strStatus != null)
						m_wndStatus.Status = strStatus;
						
					if(bVisible == true)
					{
						m_wndStatus.Show();
						m_wndStatus.Refresh();
					}
					else
					{
						m_wndStatus.Hide();
					}
					
				}
				
			}
			catch
			{
			}
			
		}// private void SetStatusVisible(bool bVisible, string strStatus)
		
		/// <summary>This method is called to show/hide the status form</summary>
		/// <param name="bVisible">true if visible</param>
		private void SetStatusVisible(bool bVisible)
		{
			SetStatusVisible(bVisible, null);
		}
			
		/// <summary>This method is called to set the operation summary message</summary>
		/// <param name="strSummary">The new summary message</param>
		private void SetSummary(string strSummary)
		{
			try
			{
				if((m_wndStatus != null) && (m_wndStatus.IsDisposed == false))
				{
					m_wndStatus.Summary = strSummary;
				}
				
			}
			catch
			{
			}
			
		}// private void SetSummary(string strSummary)
		
		/// <summary>This method is called to set the filename displayed in the status form</summary>
		/// <param name="strFilename">The new filename</param>
		private void SetStatusFilename(string strFilename)
		{
			try
			{
				if((m_wndStatus != null) && (m_wndStatus.IsDisposed == false))
				{
					m_wndStatus.Filename = strFilename;
				}
				
			}
			catch
			{
			}
			
		}// private void SetStatusFilename(string strFilename)
		
		/// <summary>This method is called to determine if the user has aborted the operation</summary>
		/// <return>True if aborted</return>
		private bool CheckAborted()
		{
			try
			{
				if(m_bAborted == false)
				{
					if(m_wndStatus != null)
					{
						Application.DoEvents();
						m_bAborted = m_wndStatus.Aborted;
					}
				
				}
			}
			catch
			{
			}
			
			return m_bAborted;
			
		}// private bool CheckAborted()
		
		/// <summary>This method will close the active file stream</summary>
		private void CloseStream()
		{
			if(m_fileStream != null)
			{
				try	{ m_fileStream.Close(); }
				catch {}
				
				m_fileStream = null;
			}
			
		}// private void CloseStream()
					
		/// <summary>This method will open the file stream for the specified file</summary>
		/// <param name="strFileSpec">Fully qualified path to the desired file</param>
		/// <returns>true if successful</returns>
		private bool OpenStream(string strFileSpec)
		{
			//	Make sure the active stream is closed
			CloseStream();

			try
			{
				
				m_fileStream = System.IO.File.CreateText(strFileSpec);
				return true;
			}
			catch
			{
				OnError(m_tmaxErrorBuilder.Message(ERROR_FILE_OPEN_FAILED, strFileSpec));
				return false;
			}
				
		}// private bool OpenStream(string strFileSpec)
					
		/// <summary>This method will reset the local members to their default values</summary>
		private void Reset()
		{
			//	Close the file
			CloseStream();
			
			m_lExported			= 0;
			m_eFormat			= TmaxExportFormats.Unknown;
			m_bTerminate		= false;
			m_bConfirmOverwrite	= true;
			m_strExportFolder	= "";
			m_xmlScript			= null;
			
			m_xmlDesignations.Clear();
				
			//	Destroy the status form
			if(m_wndStatus != null)
			{
				if(m_wndStatus.IsDisposed == false)
					m_wndStatus.Dispose();
				m_wndStatus = null;
			}
			
			//	Never reset the file specification because we use it to 
			//	initialize the file selection dialog
			
		}// private void Reset()
		
		/// <summary>This method will get the path to the export file</summary>
		///	<param name="xmlScript">The script being exported</param>
		/// <returns>The fully qualified path to the export file</returns>
		private string GetExportFileSpec(CXmlScript xmlScript)
		{
			SaveFileDialog	saveFile = null;
			string			strFilename = "";
			
			try
			{
				//	Get the defaults for initializing the dialog box
				strFilename = GetDefaultFilename(xmlScript);
				
				//	Make sure no illegal characters are in the filename
				strFilename = CTmaxToolbox.CleanFilename(strFilename, false);
				
				//	Use the last output file to initialize the output folder
				if((m_strExportFolder.Length == 0) && (m_strFileSpec.Length > 0) && (System.IO.Path.IsPathRooted(m_strFileSpec) == true))			
					m_strExportFolder = System.IO.Path.GetDirectoryName(m_strFileSpec);
				
				while(true)
				{
					//	Initialize the file selection dialog
					saveFile = new SaveFileDialog();
					saveFile.AddExtension = true;
					saveFile.CheckPathExists = true;
					saveFile.OverwritePrompt = false;
					saveFile.FileName = System.IO.Path.GetFileName(strFilename);
					saveFile.DefaultExt = m_strExtension;
					saveFile.Filter = m_strFileFilter;
					saveFile.InitialDirectory = m_strExportFolder;

					//	Open the dialog box
					if(saveFile.ShowDialog() == DialogResult.Cancel) 
					{
						strFilename = "";
					}
					else
					{
						strFilename = saveFile.FileName;
						m_strExportFolder = System.IO.Path.GetDirectoryName(strFilename);
					}
					
					//	Clean up
					try { saveFile.Dispose(); }
					catch {}
					saveFile = null;
					Application.DoEvents();
					
					//	Stop here if cancelled by the user
					if(strFilename.Length == 0) return "";

					//	Confirm overwrite if necessary
					if(ConfirmOverwrite(strFilename) == false)
						continue;
					
					break;
				
				}// while(true)
				
				return strFilename;
			
			}
			catch
			{
				OnError(m_tmaxErrorBuilder.Message(ERROR_GET_EXPORT_FILESPEC_EX));
				return "";
			}
				
		}// private bool GetExportFileSpec(CDxRecord dxExport)
					
		/// <summary>This method will get the path to the export file</summary>
		///	<param name="xmlScript">The script being exported</param>
		/// <returns>true if the user selects a file for output</returns>
		private bool GetExportStream(CXmlScript xmlScript)
		{
			string strFileSpec = "";

			try
			{
				//	Get the path to the file
				strFileSpec = GetExportFileSpec(xmlScript);
				if((strFileSpec == null) || (strFileSpec.Length == 0)) 
					return false;
					
				//	Open the new stream
				if(OpenStream(strFileSpec) == false) 
					return false;
				
				//	Save the path to the file selected by the user
				SetFileSpec(strFileSpec);
				
				return true;
			
			}
			catch
			{
				OnError(m_tmaxErrorBuilder.Message(ERROR_GET_EXPORT_STREAM_EX));
				return false;
			}
				
		}// private bool GetExportStream(CDxRecord dxExport)
					
		/// <summary>Called to confirm if OK to overwrite existing file</summary>
		/// <param name="strFilespec">The fully qualified path to the export file</param>
		/// <returns>true if successful</returns>
		private bool ConfirmOverwrite(string strFilespec)
		{
			bool	bSuccessful = false;
			string	strMsg = "";
			
			try
			{
				//	Does this file already exist?
				if(System.IO.File.Exists(strFilespec) == false)
					return true; // Nothing to overwrite
					
				//	Build the prompt for the user
				if(m_bConfirmOverwrite == true)
					strMsg = String.Format("{0} already exists. Do you want to overwrite this file?", strFilespec);
				
				//	Do we need to prompt?
				if(strMsg.Length > 0)
				{
					//	Prompt for confirmation
					if(MessageBox.Show(strMsg, "Overwrite?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
						return false; // User does not want to overwrite
				}
				
				//	Attempt to delete the file
				try
				{
					System.IO.File.Delete(strFilespec);
				}
				catch
				{
					strMsg = String.Format("Unable to delete {0} for overwrite", strFilespec);
					MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return false; // Pick a new filename
				}
				
				//	We're done
				bSuccessful = true;

			}
			catch
			{
			}
			
			return bSuccessful;
			
		}// private bool ConfirmOverwrite(string strFilespec)
		
		/// <summary>This method will get the default filename to use for the specified record</summary>
		///	<param name="xmlScript">The script being exported</param>
		/// <returns>the default filename</returns>
		private string GetDefaultFilename(CXmlScript xmlScript)
		{
			string strFilename = "";
			
			//	Get the filename to use for initializing the dialog box
			try
			{
				if((xmlScript != null) && (xmlScript.Saved == true))
				{
					strFilename = System.IO.Path.GetFileNameWithoutExtension(xmlScript.FileSpec);
				}
			
			}
			catch
			{
			}
				
			return strFilename;
				
		}// private string GetDefaultFilename(CDxRecord dxRecord)
					
		/// <summary>This method is called to prompt the user for the options used to export metadata</summary>
		/// <returns>true to continue, false to cancel</returns>
		private bool GetVideoOptions()
		{
			FTI.Trialmax.Forms.CFExportVideo exportOptions = null;
			bool bContinue = false;
			
			try
			{
				exportOptions = new CFExportVideo();
				
				m_tmaxEventSource.Attach(exportOptions.EventSource);
				exportOptions.ExportOptions = m_tmaxAppOptions.ExportOptions;
				exportOptions.WMEncoder = this.TmaxEncoder;
				
				if(exportOptions.ShowDialog() == DialogResult.OK)
				{
					//	Set up default file filters and extension for prompting the user
					if(m_tmaxAppOptions.ExportOptions.VideoWMV == true)
					{
						m_strExtension = "wmv";
						m_strFileFilter = "WMV Files (*.wmv)|*.wmv|";
					}
					if(m_tmaxAppOptions.ExportOptions.VideoEDL == true)
					{
						if(m_strExtension.Length == 0) m_strExtension = "edl";
						m_strFileFilter += "EDL Files (*.edl)|*.edl|";
					}
					if(m_tmaxAppOptions.ExportOptions.VideoSAMI == true)
					{
						if(m_strExtension.Length == 0) m_strExtension = "smi";
						m_strFileFilter += "SMI Files (*.smi)|*.smi|";
					}

					m_strFileFilter += "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

					m_bConfirmOverwrite = m_tmaxAppOptions.ExportOptions.ConfirmOverwrite;
					
					bContinue = true;
				
				}// if(exportOptions.ShowDialog() == DialogResult.OK)
			
			}
			catch
			{
				OnError(m_tmaxErrorBuilder.Message(ERROR_GET_VIDEO_OPTIONS_EX));
				return false;
			}
			
			return bContinue;
			
		}// private bool GetVideoOptions()
			
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to initialize the export operation");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to open the export file");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to launch the export operation.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create the status form for the export operation");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to open %1 to perform the export operation");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to export the designations as text: format = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the options for the export video operation");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while retrieving the path to the export file.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to export the script as a WMV. MediaID = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to export to WMV. The encoder could not be initialized. File = %1");
			
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to export to WMV. The encoder profile could not be set: Profile = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("The attempt to add a WMV encoder source for the scene failed: Scene = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("The attempt to execute the WMV encoder failed: MediaID = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to export the script to SAMI. MediaID = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("The attempt to export the script to SAMI failed. MediaID = %1\n\n%2");
			
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the video file: Media ID: %1  Path: %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while performing the export operation.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to export %1 to WMV. Only %2 of %3 designations were encoded.");
		
		}// private void SetErrorStrings()

		/// <summary>This method is called to handle an error</summary>
		/// <param name="strMessage">The error message to be displayed</param>
		/// <param name="bFatal">True if the error is fatal</param>
		/// <returns>true to terminate the operation</returns>
		private bool OnError(string strMessage, bool bFatal)
		{
			//	Add this message to the status form
			AddMessage(strMessage, bFatal ? TmaxMessageLevels.FatalError : TmaxMessageLevels.Warning);
			
			//	Is this a fatal error?
			if(bFatal == true)
			{
				strMessage += "\n\nThe operation will be cancelled";
				
				MessageBox.Show(m_wndStatus, strMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				m_bTerminate = true;
			}
			else
			{
				strMessage += "\n\nDo you want to continue?";
				
				if(MessageBox.Show(m_wndStatus, strMessage, "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
					m_bTerminate = true;
			}
			
			return m_bTerminate;
			
		}// private bool OnError(string strMessage, bool bFatal)
		
		/// <summary>This method is called to handle a fatal error</summary>
		/// <param name="strMessage">The error message to be displayed</param>
		/// <returns>true to continue the operation</returns>
		private bool OnError(string strMessage)
		{
			return OnError(strMessage, true);
		}
		
		/// <summary>This method is called to handle a fatal error</summary>
		/// <param name="strMessage">The error message to be displayed</param>
		/// <returns>true to continue the operation</returns>
		private bool OnEncoderStatus(object sender, string strMessage)
		{
			if((strMessage != null) && (strMessage.Length > 0))
				SetStatus(strMessage);
				
			//	Should we continue?
			return (CheckAborted() == false);
		
		}// private bool OnEncoderStatus(object sender, string strMessage)
		
		/// <summary>Called to set the fully qualified path to the output file</summary>
		/// <param name="strFileSpec">The path to the file</param>
		private void SetFileSpec(string strFileSpec)
		{
			m_strFileSpec = strFileSpec; 
			SetStatusFilename(System.IO.Path.GetFileName(m_strFileSpec));
		}
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}
		
		/// <summary>The application options</summary>
		public CTmaxVideoOptions AppOptions
		{
			get { return m_tmaxAppOptions; }
			set { m_tmaxAppOptions = value; }
		}

		/// <summary>TrialMax application Windows Media Encoder wrapper</summary>
		public FTI.Trialmax.Encode.CWMEncoder TmaxEncoder
		{
			get { return m_tmaxEncoder; }
			set	{ m_tmaxEncoder = value; }
		}
		
		/// <summary>True if aborted by the user</summary>
		public bool Aborted
		{
			get { return m_bAborted; }
		}

		#endregion Properties	
	
	}// class CTmaxVideoExportManager
	
}// namespace namespace FTI.Trialmax.TMVV.Tmvideo

