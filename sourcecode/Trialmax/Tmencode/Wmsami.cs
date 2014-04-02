using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

using WMEncoderLib;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Trialmax.Encode
{
	/// <summary>This class contains information associated with a system message</summary>
	public class CWMSAMI
	{
		#region Constants
			
		/// <summary>Error message identifiers</summary>
		private const int ERROR_INITIALIZE_EX				= 0;
		private const int ERROR_SAVE_EX						= 1;
		private const int ERROR_SAVE_NO_FILESPECS			= 2;
		private const int ERROR_SAVE_NO_SOURCES				= 3;
		private const int ERROR_OPEN_STREAM_FAILED			= 4;
		private const int ERROR_WRITE_LINE_EX				= 5;
		private const int ERROR_WRITE_SYNC_EX				= 6;
		private const int ERROR_WRITE_HEADER_EX				= 7;
		private const int ERROR_WRITE_FOOTER_EX				= 8;
		private const int ERROR_WRITE_BODY_EX				= 9;
		private const int ERROR_GET_SOURCE_LINES_EX			= 10;
		
		public const int SAMI_MINIMUM_VISIBLE_LINES = 1;
		public const int SAMI_MAXIMUM_VISIBLE_LINES = 4;
		public const int SAMI_DEFAULT_VISIBLE_LINES = 3;
			
		public const string SAMI_DEFAULT_EXTENSION = ".smi";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bounded to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member used to construct error messages</summary>
		private FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();
		
		/// <summary>Local member bound to VisibleLines property</summary>
		private int m_iVisibleLines = SAMI_DEFAULT_VISIBLE_LINES;
		
		/// <summary>Index of the source descriptor being processed</summary>
		private int m_iSourceIndex = 0;
		
		/// <summary>Time used to create the SNYC block</summary>
		private long m_lSyncTime = 0;
		
		/// <summary>Local member bound to Sources property</summary>
		private CWMSAMISources m_aSources = new CWMSAMISources();
		
		/// <summary>Local member bound to FileSpec property</summary>
		private string m_strFileSpec = "";
		
		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "";
		
		/// <summary>File stream used to create the output file</summary>
		private System.IO.StreamWriter m_fileStream = null;
		
		/// <summary>Local member bound to SAMIFontFamily property</summary>
		private string m_strFontFamily = "Arial";
		
		/// <summary>Local member bound to SAMIFontHighlighter property</summary>
		private bool m_bFontHighlighter = true;
		
		/// <summary>Local member bound to PageNumbers property</summary>
		private bool m_bPageNumbers = false;
		
		/// <summary>Local member bound to SAMIFontSize property</summary>
		private int m_iFontSize = 12;
		
		/// <summary>Local member bound to SAMIFontColor property</summary>
		private System.Drawing.Color m_crFontColor = System.Drawing.Color.White;
		
		/// <summary>Local member to store the lines to be written to file</summary>
		private CWMSAMILines m_aSynchronize = new CWMSAMILines();
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CWMSAMI()
		{
			m_aSynchronize.KeepSorted = false;
			
			m_tmaxEventSource.Name = "WMSAMI Events";
			m_tmaxEventSource.Attach(m_aSources.EventSource);
			m_tmaxEventSource.Attach(m_aSynchronize.EventSource);
			
			SetErrorStrings();		
		}
	
		/// <summary>Called to prepare the object to create a new SAMI file</summary>
		/// <param name="strName">The name of the SAMI title</param>
		/// <param name="strFileSpec">The path to the SAMI file</param>
		/// <returns>True if successful</returns>
		public bool Initialize(string strName, string strFileSpec)
		{
			bool bSuccessful = false;
			
			try
			{
				m_strName = strName;
				m_strFileSpec = strFileSpec;
				
				bSuccessful = true;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_EX), Ex);
			}
			
			return bSuccessful;
			
		}// public bool Initialize(string strFileSpec)

		/// <summary>Called to clear the current values and reset the defaults</summary>
		public void Clear()
		{
			try
			{
				CloseStream();
				
				//	Clear the collections
				Debug.Assert(m_aSources != null);
				m_aSources.Clear();
				Debug.Assert(m_aSynchronize != null);
				m_aSynchronize.Clear();
					
				m_strFileSpec = "";	
				m_iSourceIndex = 0;
				m_lSyncTime = 0;
			}
			catch
			{
			}
			
		}// public void Clear()

		/// <summary>This method allows the caller to add a new source file</summary>
		/// <param name="strFileSpec">Fully qualified path to the video file</param>
		/// <param name="dStart">Start time of source relative to beingging of it's parent video</param>
		/// <param name="dStop">Stop time of source relative to beingging of it's parent video</param>
		/// <param name="highlighterColor">Time in seconds of start position</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CWMSAMISource AddSource(string strFileSpec, double dStart, double dStop, System.Drawing.Color highlighterColor)
		{
			if(m_aSources != null)
				return m_aSources.Add(strFileSpec, dStart, dStop, highlighterColor);
			else
				return null;
			
		}// public CWMSAMISource AddSource(string strFileSpec, System.Drawing.Color highlighterColor)

		/// <summary>This method allows the caller to add a new source file</summary>
		/// <param name="xmlDesignation">The XML designation to use for the source object</param>
		/// <param name="highlighterColor">Time in seconds of start position</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CWMSAMISource AddSource(CXmlDesignation xmlDesignation, System.Drawing.Color highlighterColor)
		{
			if(m_aSources != null)
				return m_aSources.Add(xmlDesignation, highlighterColor);
			else
				return null;
			
		}// public CWMSAMISource AddSource(string strFileSpec, System.Drawing.Color highlighterColor)

		/// <summary>Called to create the SAMI file using the current property values</summary>
		/// <returns>True if successful</returns>
		public bool Save()
		{
			bool bSuccessful = false;
			
			//	Do we have an output file specification
			if(m_strFileSpec.Length == 0) 
			{
				m_tmaxEventSource.FireError(this, "Save", m_tmaxErrorBuilder.Message(ERROR_SAVE_NO_FILESPECS));
				return false;
			}
				
			//	Do we have at least one source?
			if((m_aSources == null) || (m_aSources.Count == 0))
			{
				m_tmaxEventSource.FireError(this, "Save", m_tmaxErrorBuilder.Message(ERROR_SAVE_NO_SOURCES, m_strFileSpec));
				return false;
			}
				
			//	Open the file stream
			if(OpenStream(m_strFileSpec) == false)
			{
				m_tmaxEventSource.FireError(this, "Save", m_tmaxErrorBuilder.Message(ERROR_OPEN_STREAM_FAILED, m_strFileSpec));
				return false;
			}
			
			//	Make sure the visible line count is within range
			SetVisibleLines(m_iVisibleLines);
			
			try
			{
				while(bSuccessful == false)
				{
					//	Write the header
					if(WriteHeader() == false)
						break;
						
					//	Write the body
					if(WriteBody() == false)
						break;
						
					//	Write the footer
					if(WriteFooter() == false)
						break;
						
					//	We're done
					bSuccessful = true;
				
				}// while(bSuccessful == false)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Save", m_tmaxErrorBuilder.Message(ERROR_SAVE_EX, m_strFileSpec), Ex);
			}
			finally
			{
				//	Clean up
				CloseStream();
			}
			
			return bSuccessful;
			
		}// public bool Save()

		/// <summary>Called to set the number of visible lines</summary>
		/// <param name="iVisibleLines">The number of visible lines</param>
		public void SetVisibleLines(int iVisibleLines)
		{
			m_iVisibleLines = iVisibleLines;
			
			if(m_iVisibleLines < SAMI_MINIMUM_VISIBLE_LINES)
				m_iVisibleLines = SAMI_MINIMUM_VISIBLE_LINES;
			else if(m_iVisibleLines > SAMI_MAXIMUM_VISIBLE_LINES)
				m_iVisibleLines = SAMI_MAXIMUM_VISIBLE_LINES;
		
		}// public void SetVisibleLines(int iVisibleLines)

		/// <summary>Called to get the most recent error message</summary>
		/// <param name="bClear">true to clear the value</param>
		/// <returns>the last error message</returns>
		public string GetLastErrorMessage(bool bClear)
		{
			string strMessage = "";
			
			if((m_tmaxEventSource != null) && (m_tmaxEventSource.LastErrorArgs != null))
			{
				strMessage = m_tmaxEventSource.LastErrorArgs.Message;
				
				if(bClear == true)
					m_tmaxEventSource.LastErrorArgs = null;
			}
			
			return strMessage;
			
		}// public string GetLastErrorMessage(bool bClear)
		
		/// <summary>Called to get the default file extension for a SAMI file</summary>
		/// <returns>The default SAMI extension</returns>
		static public string GetDefaultExtension()
		{
			return SAMI_DEFAULT_EXTENSION;
		}
		
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>Called to get enough lines to write one or more SYNC blocks</summary>
		/// <param name="iBlanks">The number of blank lines to add when the last source is processed</param>
		/// <returns>true if able to retrieve new lines</returns>
		private bool GetSourceLines(int iBlanks)
		{
			CXmlDesignation	xmlDesignation = null;
			bool			bSuccessful = false;
			CWMSAMILine		blankLine = null;
			
			Debug.Assert(m_aSources != null, "NULL SOURCE COLLECTION");
			Debug.Assert(m_aSources.Count > 0, "EMPTY SOURCE COLLECTION");
			
			//	Have we processed all the source descriptors?
			if(m_iSourceIndex >= m_aSources.Count) return false;
			
			try
			{
				while(m_iSourceIndex < m_aSources.Count)
				{
					//	Do we need to assign the designation?
					if(m_aSources[m_iSourceIndex].XmlDesignation == null)
					{
						xmlDesignation = new CXmlDesignation();
					
						//	Read the file containing the designation's transcript text
						if(m_aSources[m_iSourceIndex].FileSpec.Length > 0)
							xmlDesignation.FastFill(m_aSources[m_iSourceIndex].FileSpec, false, true);

						//	This will assign the designation and fill the SAMI lines collection
						m_aSources[m_iSourceIndex].XmlDesignation = xmlDesignation;
					}
					
					//	Do we have some lines?
					if(m_aSources[m_iSourceIndex].Lines.Count > 0)
					{
						//	Add each to the syncronize collection
						foreach(CWMSAMILine O in m_aSources[m_iSourceIndex].Lines)
						{
							m_aSynchronize.Add(O);
						}
					
					}
					else
					{
						//	Add a blank line
						blankLine = new CWMSAMILine();
						m_aSynchronize.Add(blankLine);
					}
					
					//	Consider it a success if we process at least one file
					bSuccessful = true;
					
					//	Is this the last source in the collection?
					if(m_iSourceIndex == (m_aSources.Count - 1))
					{
						for(int i = 0; i < iBlanks; i++)
						{
							//	Set an arbitrary line length
							blankLine = new CWMSAMILine();
							blankLine.Duration  = 50;
							m_aSynchronize.Add(blankLine);
						}
						
					}// if(m_iSourceIndex == (m_aSources.Count - 1))
					
					//	Line up on the next source object
					m_iSourceIndex++;
				
					//	Stop here if we have enough lines to write a SYNC section
					if(m_aSynchronize.Count >= m_iVisibleLines)
						break;
						
				}// while(m_iSourceIndex < m_aSources.Count)
				
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSourceLines", m_tmaxErrorBuilder.Message(ERROR_GET_SOURCE_LINES_EX, m_strFileSpec), Ex);
			}
			
			return bSuccessful;
		
		}// private bool GetSourceLines()
		
		/// <summary>Called to write the header to file</summary>
		/// <returns>true if successful</returns>
		private bool WriteHeader()
		{
			bool	bSuccessful = false;
			string	strTitle = "";
			
			try
			{
				if(m_strName.Length > 0)
					strTitle = m_strName;
				else
					strTitle = System.IO.Path.GetFileNameWithoutExtension(m_strFileSpec);
					
				m_fileStream.WriteLine("<SAMI>");
				m_fileStream.WriteLine("<HEAD>");
				m_fileStream.WriteLine("<TITLE>" + strTitle + "</TITLE>");
				m_fileStream.WriteLine("<STYLE TYPE='text/css'>");
				
				m_fileStream.WriteLine("<!--");
				m_fileStream.WriteLine("P { margin-left: 4pt; margin-right: 4pt;");
				m_fileStream.WriteLine("margin-bottom: 4pt; margin-top: 4pt;");
				m_fileStream.WriteLine("text-align: left; color: #C0C0C0;");
				m_fileStream.WriteLine("font-weight: bold; font-style:normal");
				
				m_fileStream.WriteLine("font-size: " + m_iFontSize.ToString() + "pt;  font-family: " + m_strFontFamily + "; }");
				m_fileStream.WriteLine(".ENUSCC {Name:English Captions; lang: en-US; SAMIType:CC;}");

				m_fileStream.WriteLine("-->");
				m_fileStream.WriteLine("</STYLE>");
				m_fileStream.WriteLine("</HEAD>");
				m_fileStream.WriteLine("<BODY>");
    
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "WriteHeader", m_tmaxErrorBuilder.Message(ERROR_WRITE_HEADER_EX, m_strFileSpec), Ex);
			}
			
			return bSuccessful;
			
		}// private bool WriteHeader()
		
		/// <summary>Called to write the body of the file</summary>
		/// <returns>true if successful</returns>
		private bool WriteBody()
		{
			bool	bSuccessful = false;
			int		iSyncIndex = -1;
			int		iBlanks = 0;
			
			//	Nothing to write if no source descriptors
			if(m_aSources == null) return false;
			if(m_aSources.Count == 0) return false;
			
			Debug.Assert(m_aSynchronize != null);
			if(m_aSynchronize == null) 
				m_aSynchronize = new CWMSAMILines();
			
			try
			{
				//	This should already be done but just in case...
				m_aSynchronize.Clear();
				m_lSyncTime = 0;
								
				//	Get the index of the "current" line within the SYNC block. The
				//	current line determines the amount of time for the block. It is
				//	based on the number of lines being displayed
				iSyncIndex = (m_iVisibleLines > 2) ? 1 : 0; 
				
				//	Do we need to insert any blank lines into front of the text?
				for(int i = 0; i < iSyncIndex; i++)
					m_aSynchronize.Add(new CWMSAMILine());
					
				//	How many blank lines will be needed at the end of the text?
				iBlanks = (m_iVisibleLines - (iSyncIndex + 1));
				
				//	Reset the source index
				m_iSourceIndex = 0;
				
				//	Process all the source descriptors
				while(m_iSourceIndex < m_aSources.Count)
				{
					//	Get lines for another sync section
					if(GetSourceLines(iBlanks) == false)
						break;
					
					//	Write as many sync blocks as we can
					while(m_aSynchronize.Count >= m_iVisibleLines)
					{
						//	Write the sync block
						WriteSync(m_lSyncTime, m_aSynchronize);
						
						//	Adjust the sync time (convert line duration to ms)
						m_lSyncTime += (long)(m_aSynchronize[iSyncIndex].Duration * 1000.0);
						
						//	Scroll
						m_aSynchronize.RemoveAt(0);
					
					}// while(m_aSynchronize.Count >= m_iVisibleLines)
						
				}// while(m_iSourceIndex < m_aSources.Count)
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "WriteBody", m_tmaxErrorBuilder.Message(ERROR_WRITE_BODY_EX, m_strFileSpec), Ex);
			}
			
			return bSuccessful;
			
		}// private bool WriteBody()
		
		/// <summary>Called to write the footer to file</summary>
		/// <returns>true if successful</returns>
		private bool WriteFooter()
		{
			bool	bSuccessful = false;
			string	strLine = "";
			
			try
			{
				//	Write the last SYNC block to clear the text
				strLine = String.Format("<SYNC START={0}><P CLASS=ENUSCC>&nbsp;<BR>", (m_lSyncTime - 100));
				m_fileStream.WriteLine(strLine);
				
				m_fileStream.WriteLine("</BODY>");
				m_fileStream.WriteLine("</SAMI>");
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "WriteFooter", m_tmaxErrorBuilder.Message(ERROR_WRITE_FOOTER_EX, m_strFileSpec), Ex);
			}
			
			return bSuccessful;
			
		}// private bool WriteFooter()
		
		/// <summary>This method is called to write a new SYNC block to the SAMI file</summary>
		/// <param name="lMilliSeconds">The sync time in milliseconds</param>
		/// <param name="samiLines">The lines to be written</param>
		/// <returns>true if successful</returns>
		private bool WriteSync(long lMilliSeconds, CWMSAMILines samilLines)
		{
			bool	bSuccessful = false;
			string	strLine = "";
			int		iLines = 0;
			int		iBlanks = 0;
			int		i;
			
			Debug.Assert(m_fileStream != null);
			
			try
			{
				//	Format the SYNC line in accordance with SAMI specification
				strLine = String.Format("<SYNC START={0}><P CLASS=ENUSCC>", lMilliSeconds);
				
				//	Write the SYNC line
				m_fileStream.WriteLine(strLine);
				
				//	Do we have enough lines?
				if(samilLines != null)
				{
					if(samilLines.Count >= m_iVisibleLines)
						iLines = m_iVisibleLines;
					else
						iLines = samilLines.Count;
				
				}// if(samilLines != null)
					
				//	How many blank lines do we have to write?
				iBlanks = m_iVisibleLines - iLines;
				
				//	Write the lines
				for(i = 0; i < iLines; i++)
				{
					if(WriteLine(samilLines[i]) == false)
						return false;
				}
				
				//	Write the blank lines
				for(i = 0; i < iBlanks; i++)
				{
					if(WriteBlank() == false)
						return false;
				}
				
				bSuccessful = true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "WriteSync", m_tmaxErrorBuilder.Message(ERROR_WRITE_SYNC_EX, m_strFileSpec), Ex);
			}
			
			return bSuccessful;
			
		}// private bool WriteSync()
		
		/// <summary>This method is called to write the specified line to the file</summary>
		/// <param name="samiLine">The line to be written to the file</param>
		/// <returns>true if successful</returns>
		private bool WriteLine(CWMSAMILine samiLine)
		{
			bool					bSuccessful = false;
			string					strLine = "";
			System.Drawing.Color	crColor = m_crFontColor;
			
			Debug.Assert(m_fileStream != null);
			
			try
			{
				//	Use the highlighter color if requested
				if(m_bFontHighlighter == true)
					crColor = samiLine.GetColor(m_crFontColor);

				//	Get the text to be written
				strLine = samiLine.GetSyncText(m_bPageNumbers, crColor);
			 
				//	Write the line to file
				if(strLine.Length > 0)
					m_fileStream.WriteLine(strLine);
				
				bSuccessful = true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "WriteLine", m_tmaxErrorBuilder.Message(ERROR_WRITE_LINE_EX, m_strFileSpec), Ex);
			}
			
			return bSuccessful;
			
		}// private bool WriteLine(CWMSAMILine samiLine)
		
		/// <summary>This method is called to write a blank line to the SYNC block</summary>
		/// <returns>true if successful</returns>
		private bool WriteBlank()
		{
			return WriteLine(new CWMSAMILine());
		}
		
		/// <summary>This method  called to get a transcript with the specified text</summary>
		/// <param name="strText">The text to be in the transcript</param>
		/// <returns>A transcript line with the specified text</returns>
		private CXmlTranscript GetTranscript(string strText)
		{
			CXmlTranscript xmlTranscript = new CXmlTranscript();
			if(strText != null)
				xmlTranscript.Text = strText;
			else
				xmlTranscript.Text = "";
			xmlTranscript.QA = "";
			return xmlTranscript;			
		}	
		
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
				return false;
			}
				
		}// private bool OpenStream(string strFileSpec)
					
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
					
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while initializing the operation.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while saving the SAMI file: filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to save the SAMI file. No output filename was provided.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to save the SAMI file. Not source inputs were provided. filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to open the file stream to save the SAMI file. filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while writing a line of text to the file. filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while writing a SYNC block to the file. filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while writing the header to the SAMI file. filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while writing the footer to the SAMI file. filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while writing the body to the SAMI file. filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while retrieving source lines for the SAMI file. filename = %1");

		}// private void SetErrorStrings()

		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}
		
		/// <summary>The collection of files containing the SAMI text</summary>
		public CWMSAMISources Sources
		{
			get { return m_aSources; }
		}
		
		/// <summary>The fully qualified path to the SAMI file</summary>
		public string FileSpec
		{
			get { return m_strFileSpec; }
		}
		
		/// <summary>The Name to be inserted in the SAMI file</summary>
		public string Name
		{
			get { return m_strName; }
		}
		
		/// <summary>The number of lines visible during playback</summary>
		public int VisibleLines
		{
			get { return m_iVisibleLines; }
			set { SetVisibleLines(value); }
		}
		
		/// <summary>True to include page/line numbers</summary>
		public bool PageNumbers
		{
			get { return m_bPageNumbers; }
			set { m_bPageNumbers = value; }
		}
		
		/// <summary>The font to use for  text</summary>
		public string FontFamily
		{
			get { return m_strFontFamily; }
			set { m_strFontFamily = value; }
		}
		
		/// <summary>The color to use for  text</summary>
		public System.Drawing.Color FontColor
		{
			get { return m_crFontColor; }
			set { m_crFontColor = value; }
		}
		
		/// <summary>True to use highlighter color for text color</summary>
		public bool FontHighlighter
		{
			get { return m_bFontHighlighter; }
			set { m_bFontHighlighter = value; }
		}
		
		/// <summary>Point size for  text</summary>
		public int FontSize
		{
			get { return m_iFontSize; }
			set { m_iFontSize = value; }
		}
		
		#endregion Properties
		
	}// public class CWMSAMI
		
}// namespace FTI.Trialmax.Encode
