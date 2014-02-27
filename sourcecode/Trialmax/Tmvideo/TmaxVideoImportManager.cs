using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.Text.RegularExpressions;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;
using FTI.Shared.Win32;
using FTI.Trialmax.Forms;

namespace FTI.Trialmax.TMVV.Tmvideo
{
	/// <summary>This class manages a form that allows the user to add new video designations</summary>
	public class CTmaxVideoImportManager
	{
		#region Constants
		
		private const int ERROR_INITIALIZE_EX				= 0;
		private const int ERROR_FILE_NOT_FOUND				= 1;
		private const int ERROR_FILE_OPEN_FAILED			= 2;
		private const int ERROR_IMPORT_EX					= 3;
		private const int ERROR_CREATE_STATUS_FORM_EX		= 4;
		private const int ERROR_GET_SOURCE_FILES_EX			= 5;
		private const int ERROR_IMPORT_STREAM_EX			= 6;
		private const int ERROR_IMPORT_SCRIPT_EX			= 7;
		private const int ERROR_INVALID_DESIGNATION_LINE	= 8;
		private const int ERROR_CREATE_XML_DESIGNATIONS		= 9;
		private const int ERROR_START_PL_OUT_OF_RANGE		= 10;
		private const int ERROR_STOP_PL_OUT_OF_RANGE		= 11;
		private const int ERROR_START_STOP_REVERSED			= 12;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bounded to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member used to construct error messages</summary>
		private FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();
		
		/// <summary>Local member bound to AppOptions property</summary>
		protected CTmaxVideoOptions m_tmaxAppOptions = null;
		
		/// <summary>Local member bound to Format property</summary>
		private TmaxImportFormats m_eFormat = TmaxImportFormats.Unknown;
		
		/// <summary>Local member bound to StatusForm property</summary>
		private CFImportStatus m_wndStatusForm = null;
		
		/// <summary>Local member bound to Stream property</summary>
		private System.IO.StreamReader m_fileStream = null;
		
		/// <summary>Local class member bound to SourceFiles property</summary>
		private string [] m_aSourceFiles = null;
		
		/// <summary>Local class member bound to SourceFolder property</summary>
		private string m_strSourceFolder = "";
		
		/// <summary>The active line</summary>
		private string m_strLine = "";
		
		/// <summary>Local class member to store a reference to the active XML script</summary>
		private FTI.Shared.Xml.CXmlScript m_xmlScript = null;
		
		/// <summary>Local class member to store a reference to the active highlighter</summary>
		private CTmaxHighlighter m_tmaxHighlighter = null;
		
		/// <summary>Local class member to store a reference to the active XML deposition</summary>
		private FTI.Shared.Xml.CXmlDeposition m_xmlDeposition = null;
		
		/// <summary>Local class member used as temporary holding collection during an import operation</summary>
		private CXmlDesignations m_xmlPending = new CXmlDesignations();
		
		/// <summary>Collection to store references to designations added to the active script</summary>
		private CXmlDesignations m_xmlAdded = new CXmlDesignations();
		
		/// <summary>XML designation to define the desired insertion point</summary>
		private CXmlDesignation m_xmlInsert = null;
		
		/// <summary>Local class member to keep track of the last designation read in from the file</summary>
		private CTmaxImportDesignation m_lastDesignation = null;
		
		/// <summary>Local flag to allow multiple file selections</summary>
		private bool m_bAllowMultiple = false;
		
		/// <summary>Local class member bound to InsertBefore property</summary>
		private bool m_bInsertBefore = false;
		
		/// <summary>Local class member bound to Cancelled property</summary>
		private bool m_bCancelled = false;
		
		/// <summary>Local class member bound to Merge property</summary>
		private bool m_bMergeSource = true;
		
		/// <summary>Local member bound to FileSpec property</summary>
		private string m_strFileSpec = "";
		
		/// <summary>Text used for file selection and status forms</summary>
		private string m_strDisplayType = "";
		
		/// <summary>Local member bound to Line property</summary>
		private int m_iLineNumber = 0;
		
		/// <summary>Local member to store the index of the active file</summary>
		private int m_iSourceIndex = -1;
		
		/// <summary>Delimiter(s) used to parse lines in the import file</summary>
		private string m_strDelimiters = "\t";
		
		/// <summary>Character(s) used to identify a comment line in the import file</summary>
		private string m_strCommentCharacters = "";
		
		#endregion Protected Members
		
		#region Public Methods
		
		public CTmaxVideoImportManager()
		{
			//	Populate the error builder's format strings
			m_tmaxEventSource.Name = "TmaxVideo Import Manager";
			SetErrorStrings();
		}

		/// <summary>This method is called to add a message to the status form list</summary>
		/// <param name="strMessage">The message to be added</param>
		/// <param name="eType">Enumerated message type to define error level</param>
		public void AddMessage(string strMessage, TmaxMessageLevels eType)
		{
			string strFilename = "";
			
			try
			{
				if((m_wndStatusForm != null) && (m_wndStatusForm.IsDisposed == false))
				{
					if(m_strFileSpec.Length > 0)
						strFilename = System.IO.Path.GetFileName(m_strFileSpec);
					
					m_wndStatusForm.AddError(strFilename, m_iLineNumber, strMessage, eType);
				}
					
			}
			catch
			{
			}
		
		}// public void AddMessage(string strMessage, TmaxMessageLevels eType)
		
		/// <summary>This method is called to execute the import operation</summary>
		/// <param name="tmaxTarget">The target location for designations imported from file</param>
		/// <param name="tmaxParameters">The collection of parameters</param>
		/// <returns>true if successful</returns>
		public bool Import(CTmaxItem tmaxTarget, CTmaxParameters tmaxParameters)
		{
			System.Threading.Thread importThread = null;
			int						iAttempts = 0;
			
			//	Initialize the import operation
			if(Initialize(tmaxTarget, tmaxParameters) == false)
				return false;
				
			//	Prompt the user for options
			if(m_eFormat == TmaxImportFormats.AsciiMedia)
			{
				if(GetScriptsOptions() == false)
					return false;
			}
		
			//	Create the status form for the operation
			if(this.CreateStatusForm() == false) return false;
			
			try
			{
				//	Start the operation
				importThread = new Thread(new ThreadStart(this.ImportThreadProc));
				importThread.Start();
					
				//	Block the caller until operation is complete or the user cancels
				if(m_wndStatusForm.ShowDialog() == DialogResult.Cancel)
				{
					m_bCancelled = m_wndStatusForm.Aborted;
					
				}// if(StatusForm.ShowDialog() == DialogResult.Cancel)
			
				//	Wait for import thread to be terminated
				while(importThread.ThreadState == System.Threading.ThreadState.Running)
				{
					Thread.Sleep(500);
					
					//	Crude test for timeout
					if(iAttempts < 20)
						iAttempts++;
					else
						break;
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Import", m_tmaxErrorBuilder.Message(ERROR_IMPORT_EX), Ex);
			}
			
			//	Do we need to undo the operation?
			if((m_xmlAdded.Count > 0) && (m_bCancelled == true))
			{
				foreach(CXmlDesignation O in m_xmlAdded)
					m_xmlScript.XmlDesignations.Remove(O);
					
				m_xmlAdded.Clear();
			}
			
			return (m_xmlAdded.Count > 0);
				
		}// public bool Import()
		
		#endregion Public Methods
		
		#region Protected Methods
		
		#endregion Protected Methods
		
		#region Properties
		
		#region Private Methods
		
		/// <summary>This method will close the active file stream</summary>
		private void CloseStream()
		{
			if(m_fileStream != null)
			{
				try	{ m_fileStream.Close(); }
				catch {}
				
				m_fileStream = null;
			}
			
			SetFileSpec("");
			SetLineNumber(0);
			m_strLine = "";

		}// private void CloseStream()
					
		/// <summary>This method will open the file stream for the specified file</summary>
		/// <returns>true if successful</returns>
		private bool OpenStream(string strFileSpec)
		{
			//	Make sure the active stream is closed
			CloseStream();

			//	Make sure the specified file exists
			if(System.IO.File.Exists(strFileSpec) == false)
			{
				AddMessage(ERROR_FILE_NOT_FOUND, strFileSpec, TmaxMessageLevels.FatalError);
				return false;
			}
			
			try
			{
				//	Open the file stream				
				m_fileStream = System.IO.File.OpenText(strFileSpec);
				
				//	Update the filename
				SetFileSpec(strFileSpec);
				m_strLine = "";
				SetLineNumber(0);
				
				return true;
			}
			catch
			{
				AddMessage(ERROR_FILE_OPEN_FAILED, strFileSpec, TmaxMessageLevels.FatalError);
				return false;
			}
				
		}// private bool OpenStream()
					
		/// <summary>This method will reset the local members to their default values</summary>
		/// <param name="bTotal">True to do a reset of all members, False for new source file</param>
		private void Reset(bool bTotal)
		{
			//	Reset the source file specific members
			m_lastDesignation	= null;
			m_tmaxHighlighter	= null;
			
			//	Close the import file
			CloseStream();
			
			//	Reset all members if requested
			if(bTotal == true)
			{
				m_aSourceFiles		= null;
				m_bInsertBefore		= false;
				m_bCancelled		= false;
				m_bMergeSource		= true;
				m_eFormat			= TmaxImportFormats.Unknown;
				m_iSourceIndex		= -1;
				m_xmlInsert			= null;
				
				//	Flush the collections
				m_xmlPending.Clear();
				m_xmlAdded.Clear();
				
				//	Destroy the status form
				if(m_wndStatusForm != null)
				{
					if(m_wndStatusForm.IsDisposed == false)
						m_wndStatusForm.Dispose();
					m_wndStatusForm = null;
				}
			
			}// if(bTotal == true)
			
			//	Never reset the source folder because we want to keep
			//	track of the folder used on the last operation
			
		}// private void Reset(bool bTotal)
		
		/// <summary>Called to set the fully qualified path to the active source file</summary>
		/// <param name="strFileSpec">The path to the active file</param>
		private void SetFileSpec(string strFileSpec)
		{
			m_strFileSpec = strFileSpec; 
				
			if((m_wndStatusForm != null) && (m_wndStatusForm.IsDisposed == false))
			{
				m_wndStatusForm.Filename = System.IO.Path.GetFileName(m_strFileSpec);
				m_wndStatusForm.Refresh();
			}
		
		}// private void SetFileSpec(string strFileSpec)
		
		/// <summary>Called to set the current line number</summary>
		/// <param name="iLineNumber">The new line number</param>
		private void SetLineNumber(int iLineNumber)
		{
			m_iLineNumber = iLineNumber; 
				
			if((m_wndStatusForm != null) && (m_wndStatusForm.IsDisposed == false))
			{
				m_wndStatusForm.Line = m_iLineNumber;
				m_wndStatusForm.Refresh();
			}
		
		}// private void SetLineNumber(int iLineNumber)
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to initialize the import operation");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate %1 to perform the import operation");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to open %1 to perform the import operation");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to execute the import operation");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create the status form for the import operation");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to get the source files for the import operation");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to import from %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to import the designations from %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Invalid designation line: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to create XML designations using the specified range: %1");
			
			m_tmaxErrorBuilder.FormatStrings.Add("The designation's start page/line is out of range");
			m_tmaxErrorBuilder.FormatStrings.Add("The designation's stop page/line is out of range");
			m_tmaxErrorBuilder.FormatStrings.Add("The designation's start and stop page/line values are reversed");

		}// private void SetErrorStrings()

		/// <summary>This method is called to add a warning message to the status form list</summary>
		/// <param name="strMessage">The message to be added</param>
		private void AddMessage(string strMessage)
		{
			AddMessage(strMessage, TmaxMessageLevels.Warning);
		}
		
		/// <summary>This method is called to add a message to the status form list</summary>
		/// <param name="iErrorId">Error identifier</param>
		/// <param name="param1">Parameter 1 to construct the message</param>
		/// <param name="param2">Parameter 2 to construct the message</param>
		/// <param name="param3">Parameter 3 to construct the message</param>
		/// <param name="eType">Enumerated message type identifier</param>
		private void AddMessage(int iErrorId, object param1, object param2, object param3, TmaxMessageLevels eType)
		{
			AddMessage(m_tmaxErrorBuilder.Message(iErrorId, param1, param2, param3), eType);
		}
		
		/// <summary>This method is called to add a message to the status form list</summary>
		/// <param name="iErrorId">Error identifier</param>
		/// <param name="param1">Parameter 1 to construct the message</param>
		/// <param name="param2">Parameter 2 to construct the message</param>
		/// <param name="eType">Enumerated message type identifier</param>
		private void AddMessage(int iErrorId, object param1, object param2, TmaxMessageLevels eType)
		{
			AddMessage(m_tmaxErrorBuilder.Message(iErrorId, param1, param2), eType);
		}
		
		/// <summary>This method is called to add a message to the status form list</summary>
		/// <param name="iErrorId">Error identifier</param>
		/// <param name="param1">Parameter 1 to construct the message</param>
		/// <param name="eType">Enumerated message type identifier</param>
		private void AddMessage(int iErrorId, object param1, TmaxMessageLevels eType)
		{
			AddMessage(m_tmaxErrorBuilder.Message(iErrorId, param1), eType);
		}
		
		/// <summary>This method is called to add a message to the status form list</summary>
		/// <param name="iErrorId">Error identifier</param>
		/// <param name="eType">Enumerated message type identifier</param>
		private void AddMessage(int iErrorId, TmaxMessageLevels eType)
		{
			AddMessage(m_tmaxErrorBuilder.Message(iErrorId), eType);
		}
		
		/// <summary>This method is called to add a warning message to the status form list</summary>
		/// <param name="iErrorId">Error identifier</param>
		/// <param name="param1">Parameter 1 to construct the message</param>
		/// <param name="param2">Parameter 2 to construct the message</param>
		/// <param name="param3">Parameter 3 to construct the message</param>
		private void AddMessage(int iErrorId, object param1, object param2, object param3)
		{
			AddMessage(iErrorId, param1, param2, param3, TmaxMessageLevels.Warning);
		}
		
		/// <summary>This method is called to add a warning message to the status form list</summary>
		/// <param name="iErrorId">Error identifier</param>
		/// <param name="param1">Parameter 1 to construct the message</param>
		/// <param name="param2">Parameter 2 to construct the message</param>
		private void AddMessage(int iErrorId, object param1, object param2)
		{
			AddMessage(iErrorId, param1, param2, TmaxMessageLevels.Warning);
		}
		
		/// <summary>This method is called to add a warning message to the status form list</summary>
		/// <param name="iErrorId">Error identifier</param>
		/// <param name="param1">Parameter 1 to construct the message</param>
		private void AddMessage(int iErrorId, object param1)
		{
			AddMessage(iErrorId, param1, TmaxMessageLevels.Warning);
		}
		
		/// <summary>This method is called to add a warning message to the status form list</summary>
		/// <param name="iErrorId">Error identifier</param>
		private void AddMessage(int iErrorId)
		{
			AddMessage(iErrorId, TmaxMessageLevels.Warning);
		}
		
		/// <summary>This method is called to prompt the user for the options used to import scripts</summary>
		/// <returns>true to continue, false to cancel</returns>
		private bool GetScriptsOptions()
		{
			FTI.Trialmax.Forms.CFImportScripts importOptions = null;
			bool bContinue = false;
			
			try
			{
				importOptions = new CFImportScripts();
				
				m_tmaxEventSource.Attach(importOptions.EventSource);
				importOptions.ImportOptions = m_tmaxAppOptions.ImportOptions;
				importOptions.HideRecords = true;
				
				if(importOptions.ShowDialog() == DialogResult.OK)
				{
					m_strCommentCharacters = m_tmaxAppOptions.ImportOptions.CommentCharacters;
					m_strDelimiters = m_tmaxAppOptions.ImportOptions.GetDelimiter();
					
					bContinue = true;
				}
			
			}
			catch
			{
			
			}
			
			return bContinue;
			
		}// private bool GetScriptsOptions()
			
		/// <summary>This method is called to warn the user when an invalid range value is encountered</summary>
		/// <param name="strMsg">The warning message</param>
		private bool Warn(string strMsg)
		{
			MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK,
							MessageBoxIcon.Exclamation);
			
			return false; // allows for cleaner code						
		
		}// private bool Warn(string strMsg)
		
		
		/// <summary>This method uses the specified parameters to set the associated properties</summary>
		/// <param name="tmaxTarget">The target location for designations imported from file</param>
		/// <param name="tmaxParameters">The collection of parameters</param>
		/// <returns>True if successful</returns>
		private bool Initialize(CTmaxItem tmaxTarget, CTmaxParameters tmaxParameters)
		{
			CTmaxParameter	tmaxParameter = null;
			bool			bSuccessful = false;

			//	Reset the current values to their defaults
			Reset(true);
			
			try
			{
				//	Get the import format
				if((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.ImportFormat)) != null)
				{
					try	  { m_eFormat = (TmaxImportFormats)(tmaxParameter.AsInteger()); }
					catch {}
				}

				if((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.Before)) != null)
					m_bInsertBefore = tmaxParameter.AsBoolean();
				
				if((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.MergeImported)) != null)
					m_bMergeSource = tmaxParameter.AsBoolean();
				
				//	Set the format specific defaults
				switch(m_eFormat)
				{
					case TmaxImportFormats.AsciiMedia:
					default:
						
						m_strCommentCharacters = "#";
						m_strDelimiters = "\t";					
						m_strDisplayType = "Script";
						m_bAllowMultiple = false;
						
						//	The caller's event item should identify the active script
						if((m_xmlScript = tmaxTarget.XmlScript) == null)
							break;
							
						//	There should be a source deposition
						if((m_xmlDeposition = m_xmlScript.XmlDeposition) == null)
							break;
							
						//	Is there an insertion designation?
						if((tmaxTarget.SubItems != null) && (tmaxTarget.SubItems.Count > 0))
						{
							if(tmaxTarget.SubItems[0].XmlDesignation != null)
								m_xmlInsert = tmaxTarget.SubItems[0].XmlDesignation;
						}
						
						bSuccessful = GetSourceFiles();
						break;
							
				}// switch(m_eFormat)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_EX), Ex);
			}
			
			return bSuccessful;
		
		}// private bool Initialize(CTmaxItem tmaxTarget, CTmaxParameters tmaxParameters)
		
		/// <summary>This method will populate the source files array with the files selected by the user</summary>
		/// <returns>true if the user selects one or more files</returns>
		private bool GetSourceFiles()
		{
			OpenFileDialog openFile = null;
			
			//	Clear the existing array
			m_aSourceFiles = null;
			m_iSourceIndex = -1;
			
			try
			{
				//	Initialize the file selection dialog
				openFile = new System.Windows.Forms.OpenFileDialog();;
				openFile.CheckFileExists = true;
				openFile.CheckPathExists = true;
				openFile.Multiselect = m_bAllowMultiple;
				openFile.Filter = "Text files (*.txt)|*.txt|All Files (*.*)|*.*";
				
				if(m_bAllowMultiple == true)
					openFile.Title = ("Select " + m_strDisplayType + " File(s)");
				else 
					openFile.Title = ("Select " + m_strDisplayType + " File");
					
				//	Use the folder of the last file imported to initialize the form
				if(m_strSourceFolder.Length > 0)
					openFile.InitialDirectory = m_strSourceFolder;

				//	Open the dialog box
				if(openFile.ShowDialog() == DialogResult.Cancel) return false; 
			
				if((openFile.FileNames != null) && (openFile.FileNames.GetUpperBound(0) >= 0))
				{
					m_aSourceFiles = openFile.FileNames;
					
					try	{ m_strSourceFolder = System.IO.Path.GetDirectoryName(m_aSourceFiles[0]); }
					catch { }
					
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSourceFiles", m_tmaxErrorBuilder.Message(ERROR_GET_SOURCE_FILES_EX), Ex);
			}
			
			return (m_aSourceFiles != null);
				
		}// private bool GetSourceFiles()
					
		/// <summary>This method is called to create the status form for the operation</summary>
		/// <returns>true if successful</returns>
		private bool CreateStatusForm()
		{
			//	Clear the cancellation flag
			m_bCancelled = false;
			
			try
			{
				//	Make sure the previous instance is disposed
				if(m_wndStatusForm != null) 
				{
					if(m_wndStatusForm.IsDisposed == false)
						m_wndStatusForm.Dispose();
					m_wndStatusForm = null;
				}
				
				//	Create a new instance
				m_wndStatusForm = new FTI.Trialmax.Forms.CFImportStatus();
			
				if(m_aSourceFiles.GetUpperBound(0) > 0)
					m_wndStatusForm.Title = "Importing " + m_strDisplayType + " Files";
				else
					m_wndStatusForm.Title = "Importing " + m_strDisplayType + " File";
				
				//	Set the initial status message
				SetStatus("Initializing import operation ...");
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateStatusForm", m_tmaxErrorBuilder.Message(ERROR_CREATE_STATUS_FORM_EX), Ex);
				m_wndStatusForm = null;
			}
			
			return (m_wndStatusForm != null);
		
		}// private bool CreateStatusForm()
		
		/// <summary>This method is called to update the status text on the status form</summary>
		/// <param name="strStatus">The new status message</param>
		private void SetStatus(string strStatus)
		{
			try
			{
				if((m_wndStatusForm != null) && (m_wndStatusForm.IsDisposed == false))
				{
					m_wndStatusForm.Status = strStatus;
					m_wndStatusForm.Refresh();
				}
				
			}
			catch
			{
			}
			
		}// private void SetStatus(string strStatus)
		
		/// <summary>Called to determine if the operation has been cancelled by the user</summary>
		/// <returns>true if cancelled</returns>
		private bool GetCancelled()
		{
			if((m_wndStatusForm != null) && (m_wndStatusForm.IsDisposed == false))
				m_bCancelled = m_wndStatusForm.Aborted;

			return m_bCancelled; 
		
		}// private bool GetCancelled()
		
		/// <summary>This method runs in its own thread to perform the import operation</summary>
		private void ImportThreadProc()
		{
			Debug.Assert(m_aSourceFiles != null);
			Debug.Assert(m_aSourceFiles.GetUpperBound(0) >= 0);
						
			//	Iterate the collection of source files to be imported
			for(m_iSourceIndex = 0; m_iSourceIndex <= m_aSourceFiles.GetUpperBound(0); m_iSourceIndex++)
			{
				//	Has the user cancelled?
				if(GetCancelled() == true) break;
					
				//	Open the stream for this file
				if(OpenStream(m_aSourceFiles[m_iSourceIndex]) == true)
				{
					if(GetCancelled() == true) break;
					
					//	Process the input stream
					ImportStream();
				
				}// if(OpenStream(SourceFiles[i]) == true)
					
				//	Reset the source file specific values
				Reset(false);
			
			}// for(int i = 0; i <= m_aSourceFiles.GetUpperBound(0); i++)
			
			//	Notify the status form
			if((m_wndStatusForm != null) && (m_wndStatusForm.IsDisposed == false))
			{
				m_wndStatusForm.OnThreadFinished("");
			}
		
		}// private void ImportThreadProc()
		
		/// <summary>This method is called to process the active file stream</summary>
		/// <returns>true if successful</returns>
		private bool ImportStream()
		{
			try
			{
				SetStatus("importing " + System.IO.Path.GetFileName(m_strFileSpec));
				
				//	What are we importing?
				switch(m_eFormat)
				{
					case TmaxImportFormats.AsciiMedia:
					
						return ImportScript();
					
					default:
					
						Debug.Assert(false, "Unable to import " + m_eFormat.ToString() + " streams");
						return false;
				
				}// switch(this.Format)
			
			}
			catch
			{
				AddMessage(ERROR_IMPORT_STREAM_EX, m_strFileSpec, TmaxMessageLevels.CriticalError);
				return false;
			}

		}// private bool ImportStream()
		
		/// <summary>This method is called to import the active stream into the active XML script</summary>
		/// <returns>true if successful</returns>
		private bool ImportScript()
		{
			string []	aFields = null;
			bool		bSuccessful = false;
			
			try
			{
				//	Process all lines in the file
				while((aFields = Parse()) != null)
				{
					Debug.Assert(aFields.GetUpperBound(0) >= 0);
					if(aFields.GetUpperBound(0) < 0) continue;
				
					if(GetCancelled() == true) break;
				
					//	Process this line
					if(ImportFields(aFields, m_xmlPending) == false)
					{
						//	An error has occurred
						bSuccessful = false;
					}
				
				}// while((aFields = Parse()) != null)
			
				//	Do we have a designation waiting to be added to the database?
				if(m_lastDesignation != null)
				{
					ImportDesignation(m_lastDesignation, m_xmlPending);
					m_lastDesignation = null;
				
				}// if(m_lastDesignation != null)
			
				//	Do we have any designations to add?
				if(m_xmlPending.Count > 0) 
				{
					//	Are we inserting?
					if(m_xmlInsert != null)
						bSuccessful = m_xmlScript.Insert(m_xmlPending, m_xmlInsert, m_bInsertBefore);
					else
						bSuccessful = m_xmlScript.Add(m_xmlPending);
						
					//	Transfer to the results collection
					if(bSuccessful == true)
						m_xmlAdded.AddRange(m_xmlPending);
						
				}// if(m_xmlPending.Count > 0) 			
		
			}
			catch
			{
				AddMessage(ERROR_IMPORT_SCRIPT_EX, m_strFileSpec, TmaxMessageLevels.CriticalError);
			}
			
			return bSuccessful;

		}// private bool ImportScript()
		
		/// <summary>This method uses the fields collection to add one or more designations</summary>
		/// <param name="aFields">The fields parsed from the line in the import file</param>
		/// <param name="xmlImported">Collection where imported designations should be stored</param>
		/// <returns>true if successful</returns>
		private bool ImportFields(string [] aFields, CXmlDesignations xmlImported)
		{
			CTmaxImportDesignation	tmaxDesignation = null;
			long					lInitial = 0;
			
			Debug.Assert(aFields != null);
			if(aFields == null) return false;
			Debug.Assert(aFields.GetUpperBound(0) >= 0);
			if(aFields.GetUpperBound(0) < 0) return false;
			
			//	Create our own collection if one not provided by the caller
			if(xmlImported == null)
				xmlImported = new CXmlDesignations();
			lInitial = xmlImported.Count;
			
			//	Allocate and initialize a designation builder
			tmaxDesignation = new CTmaxImportDesignation();
					
			while(true)
			{
				//	Set the import designation properties
				if(tmaxDesignation.SetProperties(aFields) == false)
				{
					if(tmaxDesignation.ErrorMessage.Length > 0)
						AddMessage("Bad designation - " + tmaxDesignation.ErrorMessage + ": " + m_strLine);
					else
						AddMessage(ERROR_INVALID_DESIGNATION_LINE, m_strLine);
					break;
				}
							
				//	Assign the deposition
				tmaxDesignation.DepositionId = m_xmlDeposition.Name;
				tmaxDesignation.XmlDeposition = m_xmlDeposition;
							
				//	Assign the highlighter
				SetHighlighter(tmaxDesignation);
				
				//	Make sure the start and stop positions are valid
				if(CheckRange(tmaxDesignation) == false)
					break;
							
				//	First try to add this designation to the end of the previous designation
				if(Merge(tmaxDesignation, xmlImported) == true)
					break;	//	Nothing more to do if merged

				//	Process the pending designation if it exists
				if(m_lastDesignation != null)
				{
					ImportDesignation(m_lastDesignation, xmlImported);
					m_lastDesignation = null;
				}
						
				//	Can this designation be merged with the next one in the file?
				if(CanMerge(tmaxDesignation) == true)
				{
					//	Store this until we read the next designation
					m_lastDesignation = tmaxDesignation;
				}
				else
				{
					//	Add it to the script since we can't merge the next one in the file
					ImportDesignation(tmaxDesignation, xmlImported);
				}
						
				//	We're done
				break;
					
			}// while(true)
		
			return ((xmlImported.Count - lInitial) > 0);
			
		}// private bool ImportFields(string [] aFields, CXmlDesignations xmlPending)
		
		/// <summary>This method imports the designation(s) defined by the specified descriptor</summary>
		/// <param name="tmaxDesignation">The import designation descriptor</param>
		///	<param name="xmlImported">The collection in which to store the new designations</param>
		///	<returns>The number of designations imported</returns>
		private int ImportDesignation(CTmaxImportDesignation tmaxDesignation, CXmlDesignations xmlImported)
		{
			CXmlDesignations	xmlDesignations = null;
			int					iImported = 0;

			Debug.Assert(tmaxDesignation != null);
			if(tmaxDesignation == null) return 0;
			Debug.Assert(m_xmlDeposition != null);
			if(m_xmlDeposition == null) return 0;
			
			try
			{
				//	Use the XML deposition to create XML designations
				xmlDesignations = new CXmlDesignations();
				if(tmaxDesignation.XmlDeposition.CreateDesignations(xmlDesignations, tmaxDesignation.StartPL, tmaxDesignation.StopPL, (int)(tmaxDesignation.HighlighterId)) == false)
				{
					xmlDesignations.Clear();
					AddMessage(ERROR_CREATE_XML_DESIGNATIONS, CTmaxToolbox.PLToString(tmaxDesignation.StartPL) + " -> " + CTmaxToolbox.PLToString(tmaxDesignation.StopPL));
					return 0;
				}
			
				//	Set the tuning information
				if(tmaxDesignation.StartTime >= 0)
				{
					xmlDesignations[0].Start = tmaxDesignation.StartTime;
					xmlDesignations[0].StartTuned = true;
				}
				if(tmaxDesignation.StopTime >= 0)
				{
					xmlDesignations[xmlDesignations.Count - 1].Stop = tmaxDesignation.StopTime;
					xmlDesignations[xmlDesignations.Count - 1].StopTuned = true;
				}
			
				//	Check to see if the user cancelled before we add the records
				if(GetCancelled()) return 0;
				
				//	Add each designation to the caller's collection
				foreach(CXmlDesignation O in xmlDesignations)
				{
					xmlImported.Add(O);
					iImported++;
			
				}// foreach(CXmlDesignation O in xmlDesignations)
			
			}
			catch
			{
			}
		
			return iImported;
			
		}// private int ImportDesignation(CTmaxImportDesignation tmaxDesignation, CXmlDesignations xmlImported)
		
		/// <summary>This method is called to merge the specified designation with the last designation read in from the file</summary>
		/// <param name="tmaxDesignation">the designation to be merged</param>
		///	<param name="xmlImported">The collection in which to store any new designations</param>
		///	<returns>True if the caller's designation is processed</returns>
		private bool Merge(CTmaxImportDesignation tmaxDesignation, CXmlDesignations xmlImported)
		{
			//	Has the user turned off merging?
			if(m_tmaxAppOptions.ImportOptions == null) return false;
			if(m_tmaxAppOptions.ImportOptions.MergeDesignations == TmaxDesignationMergeMethods.None) return false;
				
			//	Can't merge if there is nothing to merge with
			if(m_lastDesignation == null) return false;
			
			//	Are they created from the same designation?
			if(m_lastDesignation.DepositionId != tmaxDesignation.DepositionId)
				return false;
				
			//	Have they been assigned the same highlighter?
			if(m_lastDesignation.HighlighterId > 0)
			{
				if(tmaxDesignation.HighlighterId > 0)
				{
					if(m_lastDesignation.HighlighterId != tmaxDesignation.HighlighterId)
						return false;
				}
				else
				{
					tmaxDesignation.HighlighterId = m_lastDesignation.HighlighterId;
				}
			
			}
			else
			{
				m_lastDesignation.HighlighterId = tmaxDesignation.HighlighterId;
			}

			//	If the start position has been tuned we can't merge this designation
			if(tmaxDesignation.StartTime >= 0) return false;
			
			//	Are we restricted to merging only on page breaks?
			if(m_tmaxAppOptions.ImportOptions.MergeDesignations == TmaxDesignationMergeMethods.AdjacentPages)
			{
				//	This designation must start on line 1
				if(tmaxDesignation.StartLine != 1)
					return false;
			}
			
			//	Does this designation start on the next line?
			if(tmaxDesignation.StartPL != m_lastDesignation.GetNextPL())
				return false;
				
			//	All criteria has been met so add the caller's designation to the end of the
			//	last designation
			m_lastDesignation.StopPage	= tmaxDesignation.StopPage;
			m_lastDesignation.StopLine	= tmaxDesignation.StopLine;
			m_lastDesignation.StopPL	= tmaxDesignation.StopPL;
			m_lastDesignation.StopTime	= tmaxDesignation.StopTime;
			
			//	If the resultant designation can no longer be merged with the next
			//	designation then add it to the database now
			//
			//	NOTE:	This could happen if the designation we just merged had a tuned stop position
			if(CanMerge(m_lastDesignation) == false)
			{
				ImportDesignation(m_lastDesignation, xmlImported);
				m_lastDesignation = null;
			}
			
			return true;
			
		}// private bool Merge(CTmaxImportDesignation tmaxDesignation, CXmlDesignations xmlImported)
		
		/// <summary>This method is called to set the highlighter to use for creating a new designation</summary>
		/// <param name="tmaxDesignation">The designation being imported</param>
		///	<returns>true if successful</returns>
		private bool SetHighlighter(CTmaxImportDesignation tmaxDesignation)
		{
			CTmaxHighlighter tmaxHighlighter = null;
			
			//	Get the highlighter to use for these designations
			if((tmaxHighlighter = m_tmaxAppOptions.Highlighters.Find(tmaxDesignation.HighlighterId)) != null)
				m_tmaxHighlighter = tmaxHighlighter;
			else if((m_tmaxHighlighter == null) && (m_tmaxAppOptions.Highlighters.Count > 0))
				m_tmaxHighlighter = m_tmaxAppOptions.Highlighters[0];
				
			//	Set the highlighter Id so that we can create the designations
			if(m_tmaxHighlighter != null)
				tmaxDesignation.HighlighterId = (int)(m_tmaxHighlighter.Id);
			
			return (m_tmaxHighlighter != null);
		
		}// private bool SetHighlighter(CTmaxImportDesignation tmaxDesignation)
		
		/// <summary>This method is called to determine if the specified designation can be merged with the next one in the file</summary>
		/// <param name="tmaxDesignation">The designation being imported</param>
		/// <returns>true if it can be merged</returns>
		private bool CanMerge(CTmaxImportDesignation tmaxDesignation)
		{
			Debug.Assert(tmaxDesignation != null);
			if(tmaxDesignation == null) return false;
			
			//	Can't merge if the user has turned merging off
			if(m_tmaxAppOptions.ImportOptions == null) return false;
			if(m_tmaxAppOptions.ImportOptions.MergeDesignations == TmaxDesignationMergeMethods.None) return false;

			//	Are we only merging on page boundries?
			if(m_tmaxAppOptions.ImportOptions.MergeDesignations == TmaxDesignationMergeMethods.AdjacentPages)
			{
				//	Make sure this designation ends on the last line of the page
				if(tmaxDesignation.StopLine != tmaxDesignation.GetLinesPerPage())
					return false;
			}
			
			//	Cant merge this designation with the next one if the end point has been tuned
			if(tmaxDesignation.StopTime >= 0) return false;
			
			//	Can't merge if we're already on the last line if the transcript
			if(tmaxDesignation.StopPL == m_xmlDeposition.GetLastPL())
				return false;

			return true; //	It's all good
			
		}// private bool CanMerge(CTmaxImportDesignation tmaxDesignation)

		/// <summary>This method parses the next line in the stream</summary>
		///	<param name="strCommentCharacters">Characters to identify comment lines</param>
		/// <param name="strDelimiters">The delimiters used to separate the fields</param>
		/// <param name="bExpression">True to treat strDelimiters as a regular expression</param>
		/// <returns>The array of fields parsed from the line of text</returns>
		private string[] Parse(string strCommentCharacters, string strDelimiters, bool bExpression)
		{
			string		strLine = "";
			string[]	aFields = null;
			
			while((strLine = ReadLine()) != null)
			{
				//	Is this a blank line?
				strLine.Trim();
				if(strLine.Length == 0) continue;

				//	Is this a comment?
				if((strCommentCharacters != null) && (strCommentCharacters.Length > 0))
				{
					if(strCommentCharacters.IndexOf(strLine[0]) >= 0)
						continue;
				}

				//	Are we supposed to treat the delimiter string as a regular expression?
				if(bExpression == true)
				{
					try
					{
						Regex regEx = new Regex(strDelimiters);
						aFields = regEx.Split(strLine);
					}
					catch
					{
						//	Don't bother going any further
						return null;
					}
					
				}
				else
				{
					try { aFields = strLine.Split(strDelimiters.ToCharArray()); }
					catch {}
				}
				
				if((aFields != null) && (aFields.GetUpperBound(0) >= 0))
				{
					//	Trim all whitespace
					for(int i = 0; i <= aFields.GetUpperBound(0); i++)
					{
						try { aFields[i] = aFields[i].Trim(); }
						catch {}
					}
						
					//	For this to be a good line the first field must be assigned a value
					if(aFields[0].Length > 0)
						break;
				}
				
			}// while((strLine = ReadLine()) != null)
			
			return aFields;
		
		}// private string[] Parse()
		
		/// <summary>This method parses the next line in the stream using the local option settings</summary>
		/// <param name="bExpression">True to treat the local Delimiters as a regular expression</param>
		/// <returns>The array of fields parsed from the line of text</returns>
		private string[] Parse(bool bExpression)
		{
			return Parse(m_strCommentCharacters, m_strDelimiters, bExpression);
					
		}// private string[] Parse()
		
		/// <summary>This method parses the next line in the stream using the local option settings</summary>
		/// <returns>The array of fields parsed from the line of text</returns>
		private string[] Parse()
		{
			return Parse(m_strCommentCharacters, m_strDelimiters, false);
					
		}// private string[] Parse()
		
		/// <summary>This method is called to read the next line from the file stream</summary>
		/// <returns>The next line of text</returns>
		private string ReadLine()
		{
			string strLine = null;
			
			try
			{
				if(m_fileStream != null)
				{
					strLine = m_fileStream.ReadLine();
				}
			}
			catch
			{
				strLine = null;
			}
			
			//	Increment the line counter
			if(strLine != null)
			{
				SetLineNumber(m_iLineNumber + 1);
				m_strLine = strLine;
			}
						
			return strLine;
			
		}// private string ReadLine()
		
		/// <summary>This method is called to verify that the specified designation is within the range defined by it's source deposition</summary>
		/// <param name="tmaxDesignation">The designation being imported</param>
		/// <returns>true if within range</returns>
		private bool CheckRange(CTmaxImportDesignation tmaxDesignation)
		{
			Debug.Assert(tmaxDesignation != null);
			if(tmaxDesignation == null) return false;
			
			//	Check the range specified by the caller against the transcript extents
			if(tmaxDesignation.StartPL < m_xmlDeposition.GetFirstPL())
			{
				AddMessage(ERROR_START_PL_OUT_OF_RANGE);
				return false;
			}
			if((m_xmlDeposition.GetLastPL() > 0) && (tmaxDesignation.StopPL > m_xmlDeposition.GetLastPL()))
			{
				AddMessage(ERROR_STOP_PL_OUT_OF_RANGE);
				return false;
			}
			if(tmaxDesignation.StopPL < tmaxDesignation.StartPL)
			{
				AddMessage(ERROR_START_STOP_REVERSED);
				return false;
			}
			
			//	It's all good
			return true;
					
		}// private bool CheckRange(CTmaxImportDesignation tmaxDesignation)

		#endregion Private Methods
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}

		/// <summary>Collection of options used by the application</summary>
		public CTmaxVideoOptions AppOptions
		{
			get { return m_tmaxAppOptions; }
			set { m_tmaxAppOptions = value; }
		}
		
		/// <summary>Collection of designations added to the script</summary>
		public CXmlDesignations XmlAdded
		{
			get { return m_xmlAdded; }
		}
		
		#endregion Properties
		
	}// public class CTmaxVideoImportManager : System.Windows.Forms.Form
	
}// namespace FTI.Trialmax.TMVV.Tmvideo
