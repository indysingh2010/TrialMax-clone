using System;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using System.IO;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Forms;
using FTI.Trialmax.Encode;

namespace FTI.Trialmax.Database
{
	/// <summary>This class manages the export operations for the database</summary>
	public class COExportManager
	{
		#region Constants

		/// <summary>Error message identifiers</summary>
		private const int ERROR_INITIALIZE_EX			= 0;
		private const int ERROR_GET_EXPORT_STREAM_EX	= 1;
		private const int ERROR_EXPORT_EX				= 2;
		private const int ERROR_CREATE_STATUS_FORM_EX	= 3;
		private const int ERROR_FILE_OPEN_FAILED		= 4;
		private const int ERROR_EXPORT_ASCII_EX			= 5;
		private const int ERROR_GET_EXPORT_FILESPEC_EX	= 6;
		private const int ERROR_EXPORT_THREAD_PROC_EX	= 7;
		private const int ERROR_GET_TEXT_OPTIONS_EX		= 8;
		private const int ERROR_ADD_MEDIA_SOURCE_EX		= 9;
		private const int ERROR_ADD_BINDER_SOURCE_EX	= 10;
		
		#endregion Constants

		#region Private Members

		/// <summary>Local member bounded to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Local member used to construct error messages</summary>
		private FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();

		/// <summary>Local member bound to ExportOptions property</summary>
		private CTmaxExportObjections m_tmaxExportOptions = null;

		/// <summary>Local member bound to CaseDatabase property</summary>
		private CTmaxCaseDatabase m_tmaxCaseDatabase = null;

		/// <summary>Local member bound to ObjectionsDatabase property</summary>
		private CObjectionsDatabase m_tmaxObjectionsDatabase = null;

		/// <summary>Items collection specified at initialization</summary>
		private CTmaxItems m_tmaxItems = null;

		/// <summary>Collection of objections to be exported</summary>
		private CTmaxObjections m_tmaxSource = null;

		/// <summary>Local member bound to Format property</summary>
		private TmaxExportFormats m_eFormat = TmaxExportFormats.Unknown;

		/// <summary>Local member bound to StatusForm property</summary>
		private CFExportStatus m_wndStatus = null;

		/// <summary>Local member bound to Stream property</summary>
		private System.IO.StreamWriter m_streamWriter = null;

		/// <summary>Local member bound to FileSpec property</summary>
		private string m_strFileSpec = "";

		/// <summary>Filter string used in file selection dialog</summary>
		private string m_strFileFilter = "";

		/// <summary>Default file extension in file selection dialog</summary>
		private string m_strExtension = "";

		/// <summary>Total number of records that have been exported</summary>
		private long m_lExported = 0;

		/// <summary>Flag to indicate if the operation should be terminated</summary>
		private bool m_bTerminate = false;

		/// <summary>Flag to indicate if the operation was aborted by the user</summary>
		private bool m_bAborted = false;

		/// <summary>Flag to indicate if the user should be prompted before deleting output file</summary>
		private bool m_bConfirmOverwrite = true;

		/// <summary>Fully qualified path to the ouput folder</summary>
		private string m_strExportFolder = "";

		#endregion Private Members

		#region Public Methods

		/// <summary>Constructor</summary>
		public COExportManager()
		{
			//	Initialize the event source and error builder
			SetErrorStrings();
			m_tmaxEventSource.Name = "Export Objections";

		}// public COExportManager()

		/// <summary>This method uses the specified parameters to set the associated properties</summary>
		/// <param name="tmaxParameters">The collection of parameters</param>
		/// <param name="tmaxSource">Collection of event items that identify the source records</param>
		/// <returns>true if successful</returns>
		public bool Initialize(CTmaxParameters tmaxParameters, CTmaxItems tmaxSource)
		{
			CTmaxParameter	tmaxParameter = null;
			bool			bSuccessful = true;

			//	Reset the current values to their defaults
			Reset();
			m_bAborted = false;

			//	Warn the user if no objections available for the operation
			if((m_tmaxObjectionsDatabase == null) || (m_tmaxObjectionsDatabase.Objections == null) || (m_tmaxObjectionsDatabase.Objections.Count == 0))
			{
				MessageBox.Show("No objections available for the operation", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
			}

			try
			{
				if(tmaxParameters != null)
				{
					//	Get the export format
					if((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.ExportFormat)) != null)
					{
						try { this.Format = (TmaxExportFormats)(tmaxParameter.AsInteger()); }
						catch { }
					}

				}
				else
				{
					this.Format = TmaxExportFormats.Unknown;
					return false;	//	Cancel the operation
				}

				//	Set the format specific defaults
				switch(this.Format)
				{
					//	Only support text output at this time
					default:

						m_strExtension = "txt";
						m_strFileFilter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
						break;

				}// switch(this.Format)

				//	Save the items so that we can used them to fill the source collection
				m_tmaxItems = tmaxSource;
			}
			catch
			{
				OnError(m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_EX));
				bSuccessful = false;
			}
			
			return bSuccessful;

		}// public bool Initialize(CTmaxParameters tmaxParameters, CTmaxItems tmaxSource)

		/// <summary>This method is called to execute the export operation</summary>
		/// <returns>true if successful</returns>
		public bool Export()
		{
			//	Get the user defined options
			switch(this.Format)
			{
				case TmaxExportFormats.AsciiMedia:
				default:

					if(GetAsciiOptions() == false)
						return false;
					break;

			}// switch(this.Format)

			//	Fill the source collection
			if(SetSource(m_tmaxItems) == false) return false;
			
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

		}// public bool Export()

		/// <summary>This method is called to add a message to the status form list</summary>
		/// <param name="strMessage">The message to be added</param>
		/// <param name="eType">Enumerated message type to define error level</param>
		public void AddMessage(string strMessage, TmaxMessageLevels eLevel)
		{
			try
			{
				if((m_wndStatus != null) && (m_wndStatus.IsDisposed == false))
				{
					m_wndStatus.AddMessage(strMessage, m_wndStatus.Filename, eLevel);
				}

			}
			catch
			{
			}

		}// public void AddMessage(string strMessage, TmaxMessageLevels eLevel)

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
					default:				

						ExportAscii();
						break;

				}// switch(m_eFormat)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "ExportThreadProc", m_tmaxErrorBuilder.Message(ERROR_EXPORT_THREAD_PROC_EX), Ex);
				AddMessage(m_tmaxErrorBuilder.Message(ERROR_EXPORT_THREAD_PROC_EX), TmaxMessageLevels.FatalError);
			}

			//	Notify the status form
			if(this.StatusForm != null)
			{
				FTI.Shared.Win32.User.MessageBeep(0);

				if(this.StatusForm.Aborted == false)
				{
					this.StatusForm.Finished = true;
					SetStatus("Export operation complete");
				}

			}// if(this.StatusForm != null)

		}// public void Export()

		/// <summary>This method is called to export the source objections to text</summary>
		/// <returns>True if successful</returns>
		private bool ExportAscii()
		{
			bool		bSuccessful = false;
			string		strLine = "";
			CBaseRecord	dxSource = null;

			Debug.Assert(m_tmaxSource != null);

			//	If only one source record we want to use it to construct the
			//	default filename
			if((m_tmaxItems != null) && (m_tmaxItems.Count == 1))
				dxSource = (CBaseRecord)(m_tmaxItems[0].GetRecord());
				
			//	Open the file stream
			if(GetExportStream(dxSource) == false) return false;

			//	Make the status form visible
			SetStatusVisible(true, "Exporting objections ...");
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				//	Write the header line
				strLine = CTmaxObjection.GetAsciiHeaders(m_tmaxExportOptions);
				m_streamWriter.WriteLine(strLine);
				
				//	Export each of the objections in the source collection
				foreach(CTmaxObjection O in m_tmaxSource)
				{
					strLine = O.GetAsciiExport(m_tmaxExportOptions);
					m_streamWriter.WriteLine(strLine);
					m_lExported++;

					//	Make sure the user has not aborted the operation
					if(CheckAborted() == true) break;

				}// foreach(CTmaxObjection O in m_tmaxSource)

				if(CheckAborted() == false)
					bSuccessful = true;
			}
			catch
			{
				OnError(m_tmaxErrorBuilder.Message(ERROR_EXPORT_ASCII_EX, m_strFileSpec));
			}
			finally
			{
				CloseStream();

				Cursor.Current = Cursors.Default;

				if(bSuccessful == true)
				{
					SetSummary(m_lExported.ToString() + " objections exported to " + m_strFileSpec);
				}

			}

			return bSuccessful;

		}// private bool ExportAscii()

		/// <summary>This method sets the source collection for the operation</summary>
		/// <param name="tmaxItems">Collection of event items that identify the source records</param>
		/// <returns>true if successful</returns>
		private bool SetSource(CTmaxItems tmaxItems)
		{
			bool			bSuccessful = true;
			CDxMediaRecord	dxMediaRecord = null;
			CTmaxObjections	tmaxAvailable = null;
			
			//	These should have been checked in the call to Initialize()
			Debug.Assert(m_tmaxObjectionsDatabase != null, "NULL Objections Database");
			if(m_tmaxObjectionsDatabase == null) return false;
			Debug.Assert(m_tmaxObjectionsDatabase.Objections != null, "NULL Objections Collection");
			if(m_tmaxObjectionsDatabase.Objections == null) return false;
			Debug.Assert(m_tmaxObjectionsDatabase.Objections.Count > 0, "Empty Objections Collection");
			if(m_tmaxObjectionsDatabase.Objections.Count == 0) return false;
			
			//	Did the caller provide a valid source collection?
			if((tmaxItems != null) && (tmaxItems.Count > 0))
			{
				//	Allocate the collection for the source objections
				m_tmaxSource = new CTmaxObjections();
				
				//	Allocate and fill a temporary collection of available objections
				tmaxAvailable = new CTmaxObjections();
				foreach(CTmaxObjection O in m_tmaxObjectionsDatabase.Objections)
					tmaxAvailable.Add(O);
					
				//	Check each of the source items
				foreach(CTmaxItem O in m_tmaxItems)
				{
					//	Is this a media record?
					if((dxMediaRecord = ((CDxMediaRecord)(O.GetMediaRecord()))) != null)
					{
						AddSource(dxMediaRecord, tmaxAvailable);
					}
					
					else if(O.IBinderEntry != null)
					{
						AddSource((CDxBinderEntry)(O.IBinderEntry), tmaxAvailable);
					}
					
					else if(O.IObjection != null)
					{
						//	Add to the source collection if not already added
						if(tmaxAvailable.Contains((CTmaxObjection)(O.IObjection)) == true)
						{
							m_tmaxSource.Add((CTmaxObjection)(O.IObjection));
							tmaxAvailable.Remove((CTmaxObjection)(O.IObjection));
						}

					}
					
					//	Have we used up all the available objections?
					if(tmaxAvailable.Count == 0)
						break;

				}// foreach(CTmaxItem O in m_tmaxItems)
					
			}
			else
			{
				//	Use the entire collection
				m_tmaxSource = m_tmaxObjectionsDatabase.Objections;

			}// if((tmaxSource != null) && (tmaxSource.Count > 0))

			//	Warn the user if nothing to export
			if((m_tmaxSource == null) || (m_tmaxSource.Count == 0))
			{
				MessageBox.Show("No objections found for the export operation", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
				bSuccessful = false;
			}
			
			return bSuccessful;

		}// private bool SetSource(CTmaxItems tmaxSource)

		/// <summary>This method adds objections related to the specified record to the source collection</summary>
		/// <param name="tmaxItems">Collection of available objections</param>
		/// <returns>true if records are added to the source collection</returns>
		private bool AddSource(CDxMediaRecord dxMediaRecord, CTmaxObjections tmaxAvailable)
		{
			bool			bAdded = false;
			string			strMediaId = "";
			long			lFirstPL = 0;
			long			lLastPL = 0;
			CTmaxObjections	tmaxMatches = null;
			
			try
			{
				//	Don't bother if nothing left to add
				if(tmaxAvailable.Count == 0) return false;
				
				//	What type of media record do we have?
				switch(dxMediaRecord.MediaType)
				{
					case TmaxMediaTypes.Deposition:
					
						strMediaId = ((CDxPrimary)(dxMediaRecord)).MediaId;
						break;

					case TmaxMediaTypes.Segment:

						if(dxMediaRecord.GetParent() != null)
						{
							//	Is this a deposition segment?
							if(dxMediaRecord.GetParent().MediaType == TmaxMediaTypes.Deposition)
								strMediaId = ((CDxPrimary)(dxMediaRecord.GetParent())).MediaId;
						}
						break;

					case TmaxMediaTypes.Designation:
						
						if(dxMediaRecord.GetParent().GetParent() != null)
						{
							strMediaId = ((CDxPrimary)(dxMediaRecord.GetParent().GetParent())).MediaId;
							lFirstPL = ((CDxTertiary)(dxMediaRecord)).StartPL;
							lLastPL = ((CDxTertiary)(dxMediaRecord)).StopPL;
						}
						break;

					case TmaxMediaTypes.Scene:

						if(((CDxSecondary)(dxMediaRecord)).GetSource() != null)
							bAdded = AddSource(((CDxSecondary)(dxMediaRecord)).GetSource(), tmaxAvailable);
						break;

					case TmaxMediaTypes.Script:
						
						if(((CDxPrimary)(dxMediaRecord)).Secondaries.Count == 0)
							((CDxPrimary)(dxMediaRecord)).Fill();
							
						//	Process each of the scenes
						foreach(CDxSecondary O in ((CDxPrimary)(dxMediaRecord)).Secondaries)	
						{
							if(AddSource(O, tmaxAvailable) == true)
								bAdded = true;
						}
						break;
						
				}// switch(dxMediaRecord.MediaType)
				
				//	Should we search for objections?
				if(strMediaId.Length > 0)
				{
					//	Find all objections in the range defined by the source record
					if((tmaxMatches = tmaxAvailable.FindAll(strMediaId, lFirstPL, lLastPL)) != null)
					{
						bAdded = (tmaxMatches.Count > 0);

						foreach(CTmaxObjection O in tmaxMatches)
						{
							//	Add to the source collection and remove from the available collection
							m_tmaxSource.Add(O);
							tmaxAvailable.Remove(O);
						}

						tmaxMatches.Clear();
						tmaxMatches = null;

					}// if((tmaxMatches = tmaxAvailable.FindAll(strMediaId, lFirstPL, lLastPL)) != null)
	
				}// if(strMediaId.Length > 0)
							
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddSource", m_tmaxErrorBuilder.Message(ERROR_ADD_MEDIA_SOURCE_EX, dxMediaRecord.GetBarcode(true)), Ex);
				AddMessage(m_tmaxErrorBuilder.Message(ERROR_ADD_MEDIA_SOURCE_EX, dxMediaRecord.GetBarcode(true)), TmaxMessageLevels.FatalError);
			}

			return bAdded;

		}// private bool AddSource(CDxMediaRecord dxMediaRecord, CTmaxObjections tmaxAvailable)

		/// <summary>This method adds objections related to the specified binder to the source collection</summary>
		/// <param name="dxBinder">the binder record to be processed</param>
		/// <param name="tmaxItems">Collection of available objections</param>
		/// <returns>true if records are added to the source collection</returns>
		private bool AddSource(CDxBinderEntry dxBinder, CTmaxObjections tmaxAvailable)
		{
			bool bAdded = false;

			try
			{
				//	Don't bother if nothing left to add
				if(tmaxAvailable.Count == 0) return false;
				
				//	Do we need to populate the child collection?
				if(dxBinder.Contents.Count == 0)
					dxBinder.Fill();
					
				//	Process the contents of the binder
				foreach(CDxBinderEntry O in dxBinder.Contents)
				{
					//	Is this a media entry
					if(O.GetSource(true) != null)
					{
						if(AddSource(O.GetSource(true), tmaxAvailable) == true)
							bAdded = true;
					
					}
					else if(m_tmaxExportOptions.SubBinders == true)
					{
						if(AddSource(O, tmaxAvailable) == true)
							bAdded = true;
					}

				}// foreach(CDxBinderEntry O in dxBinder.Contents)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddSource", m_tmaxErrorBuilder.Message(ERROR_ADD_MEDIA_SOURCE_EX, dxBinder.Name), Ex);
				AddMessage(m_tmaxErrorBuilder.Message(ERROR_ADD_BINDER_SOURCE_EX, dxBinder.Name), TmaxMessageLevels.FatalError);
			}

			return bAdded;

		}// private bool AddSource(CDxBinderEntry dxBinder, CTmaxObjections tmaxAvailable)

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
			if(m_streamWriter != null)
			{
				try { m_streamWriter.Close(); }
				catch { }

				m_streamWriter = null;
			}

		}// private void CloseStream()

		/// <summary>This method will open the file stream for the specified file</summary>
		/// <param name="strFileSpec">Fully qualified path to the desired file</param>
		/// <returns>true if successful</returns>
		private bool OpenStream(string strFileSpec)
		{
			System.IO.FileStream fs = null;
			
			//	Make sure the active stream is closed
			CloseStream();

			try
			{
				fs = new FileStream(strFileSpec, FileMode.Create);

				if(fs != null)
				{
					//	Open the file stream				
					m_streamWriter = new StreamWriter(fs, System.Text.Encoding.Default);
					return true;
				}

			}
			catch
			{
				OnError(m_tmaxErrorBuilder.Message(ERROR_FILE_OPEN_FAILED, strFileSpec));
			}
			
			return false;

		}// private bool OpenStream(string strFileSpec)

		/// <summary>This method will reset the local members to their default values</summary>
		private void Reset()
		{
			//	Close the file
			CloseStream();

			m_tmaxSource = null;
			m_lExported = 0;
			m_eFormat = TmaxExportFormats.Unknown;
			m_bTerminate = false;
			m_bConfirmOverwrite = true;
			m_strExportFolder = "";

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
		///	<param name="dxExport">Exchange interface to the record used to create the file</param>
		/// <returns>The fully qualified path to the export file</returns>
		private string GetExportFileSpec(CBaseRecord dxExport)
		{
			SaveFileDialog saveFile = null;
			string strFilename = "";

			try
			{
				//	Get the defaults for initializing the dialog box
				strFilename = GetDefaultFilename(dxExport);

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
					catch { }
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

		}// private bool GetExportFileSpec(CBaseRecord dxExport)

		/// <summary>This method will get the path to the export file</summary>
		///	<param name="dxExport">Exchange interface to the record used to create the file</param>
		/// <returns>true if the user selects a file for output</returns>
		private bool GetExportStream(CBaseRecord dxExport)
		{
			string strFileSpec = "";
			bool bSuccessful = false;

			try
			{
				while(bSuccessful == false)
				{
					//	Get the path to the file
					strFileSpec = GetExportFileSpec(dxExport);
					if((strFileSpec == null) || (strFileSpec.Length == 0))
						break;

					if(OpenStream(strFileSpec) == false)
						break;

					//	Save the path to the file selected by the user
					this.FileSpec = strFileSpec;

					bSuccessful = true;

				}// while(bSuccessful == false)

			}
			catch
			{
				OnError(m_tmaxErrorBuilder.Message(ERROR_GET_EXPORT_STREAM_EX));
			}

			return bSuccessful;

		}// private bool GetExportStream(CBaseRecord dxExport)

		/// <summary>Called to confirm if OK to overwrite existing file</summary>
		/// <param name="strFilespec">The fully qualified path to the export file</param>
		/// <returns>true if successful</returns>
		private bool ConfirmOverwrite(string strFilespec)
		{
			bool bSuccessful = false;
			string strMsg = "";

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
		///	<param name="dxRecord">the record used to create the filename</param>
		/// <returns>the default filename</returns>
		private string GetDefaultFilename(CBaseRecord dxRecord)
		{
			string strFilename = "";

			//	Get the filename to use for initializing the dialog box
			try
			{
				if(dxRecord != null)
				{
					//	Is this a media record?
					if(dxRecord.GetDataType() == TmaxDataTypes.Media)
					{
						//	Build the default filename
						strFilename = ((CDxMediaRecord)dxRecord).GetBarcode(false);
						if(((CDxMediaRecord)dxRecord).GetName().Length > 0)
							strFilename += (" " + ((CDxMediaRecord)dxRecord).GetName());
					}
					else if(dxRecord.GetDataType() == TmaxDataTypes.Binder)
					{
						strFilename = ((CDxMediaRecord)dxRecord).GetName();
					}

				}// if(dxRecord != null)
				else
				{
					strFilename = m_tmaxCaseDatabase.GetShortCaseName();
				}
				
				if(strFilename.Length > 0)
				{
					if(strFilename.ToLower().EndsWith("objections") == false)
						strFilename += "_objections";
				}
				else
				{
					strFilename = "tmax_objections";
				}

			}
			catch
			{
			}

			return strFilename;

		}// private string GetDefaultFilename(CBaseRecord dxRecord)

		/// <summary>This method is called to prompt the user for the options used to export to text</summary>
		/// <returns>true to continue, false to cancel</returns>
		private bool GetAsciiOptions()
		{
			FTI.Trialmax.Forms.CFExportObjections wndOptions = null;
			bool bContinue = false;

			try
			{
				wndOptions = new CFExportObjections();

				m_tmaxEventSource.Attach(wndOptions.EventSource);
				wndOptions.ExportOptions = m_tmaxExportOptions;

				if(wndOptions.ShowDialog() == DialogResult.OK)
				{
					bContinue = true;

				}// if(exportOptions.ShowDialog() == DialogResult.OK)

			}
			catch
			{
				OnError(m_tmaxErrorBuilder.Message(ERROR_GET_TEXT_OPTIONS_EX));
				return false;
			}

			return bContinue;

		}// private bool GetAsciiOptions()

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
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while exporting the objections to %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while retrieving the path to the export file.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while performing the export operation.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to prompt for the export to text options");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding objections for a media record: barcode = %1");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while adding objections for a binder: name = %1");

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

		#endregion Private Methods

		#region Properties

		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}

		/// <summary>The active objections database</summary>
		public CObjectionsDatabase ObjectionsDatabase
		{
			get { return m_tmaxObjectionsDatabase; }
			set { m_tmaxObjectionsDatabase = value; }
		}

		/// <summary>The active case database</summary>
		public CTmaxCaseDatabase CaseDatabase
		{
			get { return m_tmaxCaseDatabase; }
			set { m_tmaxCaseDatabase = value; }
		}

		/// <summary>The user defined set of export options</summary>
		public CTmaxExportObjections ExportOptions
		{
			get { return m_tmaxExportOptions; }
			set { m_tmaxExportOptions = value; }
		}

		/// <summary>Enumerated export format identifier</summary>
		public FTI.Shared.Trialmax.TmaxExportFormats Format
		{
			get { return m_eFormat; }
			set { m_eFormat = value; }
		}

		/// <summary>Form displayed during the export operation</summary>
		public FTI.Trialmax.Forms.CFExportStatus StatusForm
		{
			get { return m_wndStatus; }
		}

		/// <summary>Fully qualified path to the active file stream</summary>
		public string FileSpec
		{
			get { return m_strFileSpec; }
			set
			{
				m_strFileSpec = value;
				SetStatusFilename(System.IO.Path.GetFileName(m_strFileSpec));
			}

		}

		/// <summary>The collection of objections to be exported</summary>
		public CTmaxObjections Source
		{
			get { return m_tmaxSource; }
		}

		/// <summary>True if aborted by the user</summary>
		public bool Aborted
		{
			get { return m_bAborted; }
		}

		#endregion Properties

	}// public class COExportManager

}// namespace FTI.Trialmax.Database

