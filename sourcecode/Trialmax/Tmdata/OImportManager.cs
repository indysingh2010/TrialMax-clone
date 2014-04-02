using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Forms;

namespace FTI.Trialmax.Database
{
	/// <summary>This class manages the import operations for the objections database</summary>
	public class COImportManager
	{
		#region Constants

		private const int IMPORT_COLUMN_UNIQUE_ID		= 0;
		private const int IMPORT_COLUMN_DEPOSITION		= 1;
		private const int IMPORT_COLUMN_PLAINTIFF		= 2;
		private const int IMPORT_COLUMN_STATE			= 3;
		private const int IMPORT_COLUMN_RULING			= 4;
		private const int IMPORT_COLUMN_FIRST_PAGE		= 5;
		private const int IMPORT_COLUMN_FIRST_LINE		= 6;
		private const int IMPORT_COLUMN_LAST_PAGE		= 7;
		private const int IMPORT_COLUMN_LAST_LINE		= 8;
		private const int IMPORT_COLUMN_ARGUMENT		= 9;
		private const int IMPORT_COLUMN_RULING_TEXT		= 10;
		private const int IMPORT_COLUMN_RESPONSE_1		= 11;
		private const int IMPORT_COLUMN_RESPONSE_2		= 12;
		private const int IMPORT_COLUMN_RESPONSE_3		= 13;
		private const int IMPORT_COLUMN_WORK_PRODUCT	= 14;
		private const int IMPORT_COLUMN_COMMENTS		= 15;
		private const int IMPORT_COLUMN_CASE_NAME		= 16;
		private const int IMPORT_COLUMN_CASE_ID			= 17;
		private const int MAX_IMPORT_COLUMNS			= 18;
		
		/// <summary>Result identifiers</summary>
		private const int IMPORT_RESULT_ADDED			= 1;
		private const int IMPORT_RESULT_UPDATED			= 2;
		private const int IMPORT_RESULT_IGNORED			= 3;
		private const int IMPORT_RESULT_CONFLICT		= 4;
		private const int IMPORT_RESULT_ADD_FAILED		= 5;
		private const int IMPORT_RESULT_UPDATE_FAILED	= 6;
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_INITIALIZE_EX = 0;
		private const int ERROR_GET_SOURCE_FILES_EX = 1;
		private const int ERROR_IMPORT_EX = 2;
		private const int ERROR_CREATE_STATUS_FORM_EX = 3;
		private const int ERROR_FILE_NOT_FOUND = 4;
		private const int ERROR_FILE_OPEN_FAILED = 5;
		private const int ERROR_IMPORT_STREAM_EX = 6;
		private const int ERROR_INVALID_COLUMN = 7;
		private const int ERROR_NO_DEPOSITION_COLUMN = 8;
		private const int ERROR_INVALID_UNIQUE_ID = 9;
		private const int ERROR_NO_FIRST_PAGE_COLUMN = 10;
		private const int ERROR_NO_FIRST_LINE_COLUMN = 11;
		private const int ERROR_NO_LAST_PAGE_COLUMN = 12;
		private const int ERROR_NO_LAST_LINE_COLUMN = 13;
		private const int ERROR_NO_PLAINTIFF_COLUMN = 14;
		private const int ERROR_IMPORT_FROM_TEXT_EX = 15;
		private const int ERROR_SET_COLUMNS_EX = 16;
		private const int ERROR_ADD_EX = 17;
		private const int ERROR_UPDATE_EX = 18;
		private const int ERROR_SET_DEPOSITION_EX = 19;
		private const int ERROR_DEPOSITION_NOT_FOUND = 20;
		private const int ERROR_NO_DEPOSITION = 21;
		private const int ERROR_SET_STATE_EX = 22;
		private const int ERROR_STATE_NOT_FOUND = 23;
		private const int ERROR_NO_STATE = 24;
		private const int ERROR_NO_FIRST_PL = 25;
		private const int ERROR_INVALID_FIRST_PAGE = 26;
		private const int ERROR_INVALID_FIRST_LINE = 27;
		private const int ERROR_INVALID_LAST_PAGE = 28;
		private const int ERROR_INVALID_LAST_LINE = 29;
		private const int ERROR_INVALID_PL_RANGE = 30;
		private const int ERROR_FIRST_OUT_OF_BOUNDS = 31;
		private const int ERROR_LAST_OUT_OF_BOUNDS = 32;
		private const int ERROR_SET_PLAINTIFF_EX = 33;
		private const int ERROR_SET_RULING_EX = 34;
		private const int ERROR_RULING_NOT_FOUND = 35;
		private const int ERROR_NO_RULING = 36;
		private const int ERROR_CREATE_OBJECTION_EX = 37;
		private const int ERROR_SET_CASE_EX = 38;
		private const int ERROR_NO_CASE = 39;
		private const int ERROR_IMPORT_XML_SCRIPT_EX = 40;
		private const int ERROR_IMPORT_XML_DEPOSITION_EX = 41;
		private const int ERROR_IMPORT_OBJECTION_EX = 42;
		private const int ERROR_IMPORT_XML_OBJECTIONS_EX = 43;
		private const int ERROR_IMPORT_XML_OBJECTION_EX = 44;
		private const int ERROR_LOW_FIELD_COUNT = 45;
		
		#endregion Constants

		#region Private Members

		/// <summary>Local member bounded to EventSource property</summary>
		protected FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Local member used to construct error messages</summary>
		protected FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();

		/// <summary>Local member bound to CaseDatabase property</summary>
		private CTmaxCaseDatabase m_tmaxCaseDatabase = null;

		/// <summary>Local member bound to ObjectionsDatabase property</summary>
		private CObjectionsDatabase m_tmaxObjectionsDatabase = null;

		/// <summary>Local member bound to ImportOptions property</summary>
		protected CTmaxImportOptions m_tmaxOptions = null;

		/// <summary>Local member bound to StatusForm property</summary>
		private CFImportStatus m_wndStatusForm = null;

		/// <summary>Local member bound to Stream property</summary>
		private System.IO.StreamReader m_streamReader = null;

		/// <summary>Local class member bound to SourceFiles property</summary>
		private string[] m_aSourceFiles = null;

		/// <summary>Local class member bound to SourceFolder property</summary>
		private string m_strSourceFolder = "";

		/// <summary>The active line</summary>
		private string m_strLine = "";

		/// <summary>Local class member bound to Cancelled property</summary>
		private bool m_bCancelled = false;

		/// <summary>Local member bound to FileSpec property</summary>
		private string m_strFileSpec = "";

		/// <summary>Text used for file selection and status forms</summary>
		private string m_strDisplayType = "";

		/// <summary>Local member bound to Line property</summary>
		private int m_iLineNumber = 0;

		/// <summary>Local member to keep track of the active case record in the objections database</summary>
		private COxCase m_oxCase = null;

		/// <summary>Local member to keep track of the active state record in the objections database</summary>
		private COxState m_oxState = null;

		/// <summary>Local member to keep track of the active ruling record in the objections database</summary>
		private COxRuling m_oxRuling = null;

		/// <summary>Local member to keep track of the last Plantiff value</summary>
		private bool m_bPlaintiff = true;

		/// <summary>Local member to keep track of the active deposition record in the objections database</summary>
		private COxDeposition m_oxDeposition = null;

		/// <summary>Local member to keep track of the active deposition record in the case database</summary>
		private CDxPrimary m_dxDeposition = null;

		/// <summary>Local member to store the index of the active file</summary>
		private int m_iSourceIndex = -1;

		/// <summary>Delimiter(s) used to parse lines in the import file</summary>
		private string m_strDelimiters = "\t";

		/// <summary>Character(s) used to identify a comment line in the import file</summary>
		private string m_strCommentCharacters = "";

		/// <summary>Local flag to allow multiple file selections</summary>
		private bool m_bAllowMultiple = false;

		/// <summary>Local class member bound to Results property</summary>
		private CTmaxDatabaseResults m_tmaxResults = null;

		/// <summary>Local class member bound to store the column indexes</summary>
		private int[] m_aColumns = new int[MAX_IMPORT_COLUMNS];

		/// <summary>Local member to keep track of total number of files processed</summary>
		private long m_lFiles = 0;

		/// <summary>Local member to keep track of total number added to the database</summary>
		private long m_lAdded = 0;

		/// <summary>Local member to keep track of total number of records that were updated</summary>
		private long m_lUpdated = 0;

		/// <summary>Local member to keep track of total number of columns in the file</summary>
		private int m_iFileColumns = 0;

		#endregion Private Members

		#region Public Methods

		/// <summary>Constructor</summary>
		public COImportManager()
		{
			//	Initialize the event source and error builder
			SetErrorStrings();
			m_tmaxEventSource.Name = "Import Objections Manager";

		}// public COImportManager()

		/// <summary>This method will populate the source files array with the files selected by the user</summary>
		/// <returns>true if the user selects one or more files</returns>
		public bool GetSourceFiles()
		{
			OpenFileDialog openFile = null;

			//	Clear the existing array
			m_aSourceFiles = null;
			m_iSourceIndex = -1;

			try
			{
				//	Initialize the file selection dialog
				openFile = new System.Windows.Forms.OpenFileDialog(); ;
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

					try { m_strSourceFolder = System.IO.Path.GetDirectoryName(m_aSourceFiles[0]); }
					catch { }

				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this,"GetSourceFiles",m_tmaxErrorBuilder.Message(ERROR_GET_SOURCE_FILES_EX),Ex);
			}

			return (m_aSourceFiles != null);

		}// public bool GetSourceFiles()

		/// <summary>This method uses the specified parameters to set the associated properties</summary>
		/// <param name="tmaxParameters">The collection of parameters</param>
		/// <param name="tmaxResults">The results of the operation are stored</param>
		public void Initialize(CTmaxParameters tmaxParameters, CTmaxDatabaseResults tmaxResults)
		{
			//	Reset the current values to their defaults
			Reset(true);

			try
			{
				m_tmaxResults = tmaxResults;

				m_strCommentCharacters = "#";
				m_strDelimiters = "\t";
				m_strDisplayType = "Objections";
				m_bAllowMultiple = true;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this,"Initialize",m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_EX),Ex);
			}

		}// public void Initialize(CTmaxParameters tmaxParameters)

		/// <summary>This method is called to execute the import operation</summary>
		/// <returns>true if successful</returns>
		public bool Import()
		{
			System.Threading.Thread importThread = null;
			int iAttempts = 0;
			bool bSuccessful = false;

			//	There should be source files available
			if((SourceFiles == null) || (SourceFiles.GetUpperBound(0) < 0))
				return false;

			//	Prompt the user for options 
			if(GetOptions() == false)
				return false;

			//	Create the status form for the operation
			if(this.CreateStatusForm() == false) return false;

			try
			{
				//	Start the operation
				importThread = new Thread(new ThreadStart(this.ImportThreadProc));
				importThread.Start();

				//	Block the caller until operation is complete or the user cancels
				if(StatusForm.ShowDialog() == DialogResult.Cancel)
				{
					this.Cancelled = StatusForm.Aborted;

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
				m_tmaxEventSource.FireError(this,"Import",m_tmaxErrorBuilder.Message(ERROR_IMPORT_EX),Ex);
			}

			//	Did we import/update any records?
			if(m_tmaxResults != null)
			{
				bSuccessful = ((m_tmaxResults.ObjectionsAdded.Count > 0) || (m_tmaxResults.ObjectionsUpdated.Count > 0));
			}
			else
			{
				bSuccessful = true;
			}

			return bSuccessful;

		}// public bool Import()

		/// <summary>This method is called to import the specified XML script</summary>
		/// <param name="xmlScript">The script containing the objections to be imported</param>
		/// <returns>true if successful</returns>
		public bool ImportXmlScript(CXmlScript xmlScript)
		{
			bool bSuccessful = true;

			try
			{
				//	Update the status form
				if(xmlScript.MediaId.Length > 0)
					SetStatus("importing " + xmlScript.MediaId + " objections");
				else
					SetStatus("importing " + xmlScript.Name + " objections");
				
				FileSpec = xmlScript.FileSpec;

				//	Reset each of the column indexes to enable all import fields
				for(int i = 0; i <= m_aColumns.GetUpperBound(0); i++)
					m_aColumns[i] = 0;

				//	Import each of the depositions stored in the script
				if((xmlScript.XmlDepositions != null) && (xmlScript.XmlDepositions.Count > 0))
				{
					foreach(CXmlDeposition O in xmlScript.XmlDepositions)
					{
						if(ImportXmlDeposition(O) == false)
							bSuccessful = false;
					}

				}// if((xmlScript.XmlDepositions != null) && (xmlScript.XmlDepositions.Count > 0))
				
			}
			catch
			{
				AddMessage(ERROR_IMPORT_XML_SCRIPT_EX, xmlScript.MediaId.Length > 0 ? xmlScript.MediaId : xmlScript.Name, TmaxMessageLevels.CriticalError);
				bSuccessful = false;
			}

			return bSuccessful;

		}// public bool ImportXmlScript(CXmlScript xmlScript)
		
		/// <summary>This method will reset the local members to their default values</summary>
		public void Reset()
		{
			Reset(true);
		}
		
		/// <summary>This method is called to add a message to the status form list</summary>
		/// <param name="strMessage">The message to be added</param>
		/// <param name="eLevel">Enumerated error level</param>
		public void AddMessage(string strMessage, TmaxMessageLevels eLevel)
		{
			string strFilename = "";

			try
			{
				if((m_wndStatusForm != null) && (m_wndStatusForm.IsDisposed == false))
				{
					if(FileSpec.Length > 0)
						strFilename = System.IO.Path.GetFileName(FileSpec);

					m_wndStatusForm.AddError(strFilename, LineNumber, strMessage, eLevel);
				}

			}
			catch
			{
			}

		}// public void AddMessage(string strMessage, TmaxMessageLevels eLevel)

		#endregion Public Methods

		#region Private Methods

		/// <summary>This method will reset the local members to their default values</summary>
		/// <param name="bTotal">True to do a reset of all members, False for new source file</param>
		private void Reset(bool bTotal)
		{
			//	Close the import file
			CloseStream();
			m_oxState = null;
			m_oxRuling = null;
			m_oxCase = null;
			m_oxDeposition = null;
			m_bPlaintiff = false;

			//	Reset all members if requested
			if(bTotal == true)
			{
				m_tmaxResults = null;
				m_aSourceFiles = null;
				m_bCancelled = false;
				m_iSourceIndex = -1;
				m_oxDeposition = null;
				m_lAdded = 0;
				m_lFiles = 0;
				m_lUpdated = 0;

				//	Destroy the status form
				if(m_wndStatusForm != null)
				{
					if(m_wndStatusForm.IsDisposed == false)
						m_wndStatusForm.Dispose();
					m_wndStatusForm = null;
				}

				//	Reset each of the column indexes
				for(int i = 0;i <= m_aColumns.GetUpperBound(0);i++)
					m_aColumns[i] = -1;

			}// if(bTotal == true)

			//	Never reset the source folder because we want to keep
			//	track of the folder used on the last job

		}// private void Reset(bool bTotal)

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

				m_tmaxEventSource.Attach(m_wndStatusForm.EventSource);
				m_wndStatusForm.ResultsMode = TmaxImportMessageModes.AsciiObjections;
				m_wndStatusForm.ShowResults = true;
				
				if(m_aSourceFiles.GetUpperBound(0) > 0)
					m_wndStatusForm.Title = "Importing " + m_strDisplayType + " Files";
				else
					m_wndStatusForm.Title = "Importing " + m_strDisplayType + " File";

				//	Set the initial status message
				SetStatus("Initializing import operation ...");

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this,"CreateStatusForm",m_tmaxErrorBuilder.Message(ERROR_CREATE_STATUS_FORM_EX),Ex);
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

		/// <summary>This method will close the active file stream</summary>
		private void CloseStream()
		{
			if(m_streamReader != null)
			{
				try { m_streamReader.Close(); }
				catch { }

				m_streamReader = null;
			}

			FileSpec = "";
			LineNumber = 0;
			m_strLine = "";

		}// private void CloseStream()

		/// <summary>This method will open the file stream for the specified file</summary>
		/// <param name="strFileSpec">The fully qualified path to the desired file</param>
		/// <returns>true if successful</returns>
		private bool OpenStream(string strFileSpec)
		{
			bool					bSuccessful = true;
			System.IO.FileStream	fsSource = null;

			//	Make sure the active stream is closed
			CloseStream();

			//	Make sure the specified file exists
			if(System.IO.File.Exists(strFileSpec) == false)
			{
				AddMessage(ERROR_FILE_NOT_FOUND,strFileSpec,TmaxMessageLevels.FatalError);
				return false;
			}

			try
			{
				//	Open the file stream				
				fsSource = new FileStream(strFileSpec, FileMode.Open, FileAccess.Read);

				if(fsSource != null)
				{
					//	Open the file stream				
					m_streamReader = new StreamReader(fsSource, System.Text.Encoding.Default);
				}

			}
			catch
			{
				bSuccessful = false;
			}

			//	Were we unable to open the file?
			if(bSuccessful == false)
				AddMessage(ERROR_FILE_OPEN_FAILED,strFileSpec,TmaxMessageLevels.FatalError);
			else
				FileSpec = strFileSpec;

			return bSuccessful;

		}// private bool OpenStream(string strFileSpec)

		/// <summary>This method is called to read the next line from the file stream</summary>
		/// <returns>The next line of text</returns>
		private string ReadLine()
		{
			string strLine = null;

			try
			{
				Debug.Assert(m_streamReader != null);

				if(m_streamReader != null)
				{
					strLine = m_streamReader.ReadLine();
						
					//if((strLine = m_streamReader.ReadLine()) != null)
					//    strLine = CTmaxToolbox.StripMSChars(strLine, true);
				}
				
			}
			catch(System.Exception Ex)
			{
				strLine = null;
				m_tmaxEventSource.FireDiagnostic(this, "ReadLine", Ex);
			}

			//	Increment the line counter
			if(strLine != null)
			{
				LineNumber = LineNumber + 1;
				m_strLine = strLine;
			}

			return strLine;

		}// private string ReadLine()

		/// <summary>This method runs in its own thread to perform the import operation</summary>
		private void ImportThreadProc()
		{
			string strMsg = "";
			
			Debug.Assert(m_tmaxCaseDatabase != null);
			Debug.Assert(SourceFiles != null);
			Debug.Assert(SourceFiles.GetUpperBound(0) >= 0);

			//	Iterate the collection of source files to be imported
			for(m_iSourceIndex = 0; m_iSourceIndex <= SourceFiles.GetUpperBound(0); m_iSourceIndex++)
			{
				//	Has the user cancelled?
				if(this.Cancelled == true) break;

				//	Open the stream for this file
				if(OpenStream(SourceFiles[m_iSourceIndex]) == true)
				{
					if(this.Cancelled == true) break;

					//	Process the input stream
					ImportFile();

				}// if(OpenStream(SourceFiles[i]) == true)

				//	Reset the source file specific values
				Reset(false);

			}// for(int i = 0; i <= m_tmaxImportManager.SourceFiles.GetUpperBound(0); i++)

			//	Notify the status form
			if((this.StatusForm != null) && (this.StatusForm.IsDisposed == false))
			{
				//	Construct a summary message
				strMsg = String.Format("{0} files processed\n{1} objections added\n", m_lFiles, m_lAdded);
				strMsg += (m_lUpdated.ToString() + " objections updated");
				
				//	Notify the status form
				this.StatusForm.OnThreadFinished(strMsg);

			}// if(this.StatusForm != null)

		}// private void ImportThreadProc()

		/// <summary>This method is called to process the active file stream</summary>
		/// <returns>true if successful</returns>
		private bool ImportFile()
		{
			bool bSuccessful = false;
			
			try
			{
				SetStatus("importing " + System.IO.Path.GetFileName(this.FileSpec));
				
				//	Only support text files right now
				if((bSuccessful = ImportFromText()) == true)
					m_lFiles += 1;
			}
			catch
			{
				AddMessage(ERROR_IMPORT_STREAM_EX,this.FileSpec,TmaxMessageLevels.CriticalError);
			}
			
			return bSuccessful;

		}// private bool ImportFile()

		/// <summary>This method is called to import the active text stream as coded (fielded) data</summary>
		/// <returns>true if successful</returns>
		private bool ImportFromText()
		{
			string[]		aFields = null;
			bool			bSuccessful = false;
			CTmaxObjection	tmaxObjection = null;

			Debug.Assert(m_tmaxCaseDatabase != null);
			if(m_tmaxCaseDatabase == null) return false;

			try
			{
				m_strDelimiters = m_tmaxOptions.GetDelimiter();
				m_strCommentCharacters = m_tmaxOptions.CommentCharacters;

				//	The first line in the file should be the column headers
				if((aFields = Parse(true)) == null) return false;
				if((SetColumns(aFields)) == false) return false;
				
				//	Process the remaining lines in the file
				while((aFields = Parse(false)) != null)
				{
                   
					if(this.Cancelled == true) break;
                   //	Do we have a sufficient number of fields?
					if(aFields.GetUpperBound(0) < (m_iFileColumns - 1))
                    {
                        
						AddMessage(ERROR_LOW_FIELD_COUNT, m_iFileColumns, aFields.GetUpperBound(0) + 1);
						bSuccessful = false;
						continue;
					}
                    if (aFields.GetUpperBound(0) < (m_iFileColumns - 1))
                    {
                        AddMessage(ERROR_LOW_FIELD_COUNT, m_iFileColumns, aFields.GetUpperBound(0) + 1);
                        bSuccessful = false;
                        continue;
                    }

					//	Create and initialize an objection using the import values
					if((tmaxObjection = CreateObjection(aFields)) != null)
                    {
						//	Import this objection
						ImportObjection(tmaxObjection);
					}
					else
                    {
                        
						m_tmaxEventSource.FireDiagnostic(this, "ImportFromText", "Invalid Line: " + this.FileSpec.ToLower() + " - #" + this.LineNumber.ToString());

					}// // if((tmaxObjection = CreateObjection(aFields)) != null)
					
				}// while((aFields = Parse()) != null)
				
				bSuccessful = true;
				
			}
			catch(System.Exception Ex)
			{
				AddMessage(ERROR_IMPORT_FROM_TEXT_EX, TmaxMessageLevels.FatalError);
				m_tmaxEventSource.FireDiagnostic(this, "ImportFromText", m_tmaxErrorBuilder.Message(ERROR_IMPORT_FROM_TEXT_EX), Ex);
			}

			return bSuccessful;

		}// private bool ImportFromText()

		/// <summary>This method is called to import the specified objection</summary>
		/// <returns>true if successful</returns>
		private bool ImportObjection(CTmaxObjection tmaxObjection)
		{
		   
		    
			bool			bSuccessful = false;
			COxObjection	oxObjection = null;
			COxObjection	oxDuplicate = null;

			try
			{
				//	Should we search for a duplicate record?
				if((Options.ObjectionsMethod != TmaxImportObjectionMethods.AddAll) || (Options.DiscardObjectionsId == false))
					oxDuplicate = GetDuplicate(tmaxObjection);

				//	Did we find a duplicate objection?
				if(oxDuplicate != null)
				{
					//	Which method are we using?
					if(Options.ObjectionsMethod == TmaxImportObjectionMethods.AddAll)
					{
						AddResult(oxDuplicate.TmaxObjection, TmaxImportResults.Conflict);
					}
					else if(Options.ObjectionsMethod == TmaxImportObjectionMethods.IgnoreExisting)
					{
						AddResult(oxDuplicate.TmaxObjection, TmaxImportResults.Ignored);
					}
					else
					{
						//	Update the record
						if(Update(oxDuplicate, tmaxObjection) == true)
							AddResult(oxDuplicate.TmaxObjection, TmaxImportResults.Updated);
						else
							AddResult(oxDuplicate.TmaxObjection, TmaxImportResults.UpdateFailed);
					}

				}
				else
                {
					//	Make sure to strip the existing Id if requested
					if((Options.ObjectionsMethod == TmaxImportObjectionMethods.AddAll) && (Options.DiscardObjectionsId == true))
						tmaxObjection.UniqueId = "";

					//	Add the new objection to the database
					if((oxObjection = Add(tmaxObjection)) != null)
						AddResult(oxObjection.TmaxObjection, TmaxImportResults.Added);
					else
						AddResult(tmaxObjection, TmaxImportResults.AddFailed);

				
				}// if(oxDuplicate != null)

				bSuccessful = true;

			}
			catch(System.Exception Ex)
			{
				AddMessage(ERROR_IMPORT_OBJECTION_EX);
				m_tmaxEventSource.FireDiagnostic(this, "ImportObjection", m_tmaxErrorBuilder.Message(ERROR_IMPORT_OBJECTION_EX), Ex);
			}

			return bSuccessful;

		}// private bool ImportObjection(CTmaxObjection tmaxObjection)

		/// <summary>This method is called to import the specified XML deposition</summary>
		/// <param name="xmlDeposition">The deposition containing the objections to be imported</param>
		/// <returns>true if successful</returns>
		private bool ImportXmlDeposition(CXmlDeposition xmlDeposition)
		{
			bool bSuccessful = true;

			try
			{
				//	Update the status form
				if(xmlDeposition.MediaId.Length > 0)
					SetStatus("importing " + xmlDeposition.MediaId + " objections");
				else
					SetStatus("importing " + xmlDeposition.Name + " objections");

				//	Set the active deposition for the import operation
				if(SetOxDeposition(xmlDeposition.MediaId) == false)
					return false;
				
				//	Import each of the objections stored in the deposition
				if((xmlDeposition.Objections != null) && (xmlDeposition.Objections.Count > 0))
				{
					foreach(CXmlObjections O in xmlDeposition.Objections)
					{
						if(ImportXmlObjections(O) == false)
							bSuccessful = false;

					}// foreach(CXmlObjections O in xmlDeposition.Objections)

				}// if((xmlDeposition.Objections != null) && (xmlDeposition.Objections.Count > 0))

			}
			catch
			{
				AddMessage(ERROR_IMPORT_XML_DEPOSITION_EX, xmlDeposition.MediaId.Length > 0 ? xmlDeposition.MediaId : xmlDeposition.Name);
				bSuccessful = false;
			}

			return bSuccessful;

		}// private bool ImportXmlDeposition(CXmlDeposition xmlDeposition)

		/// <summary>This method is called to import the specified collection of XML objections</summary>
		/// <param name="xmlObjections">The collection of XML objections to be imported</param>
		/// <returns>true if successful</returns>
		private bool ImportXmlObjections(CXmlObjections xmlObjections)
		{
			bool bSuccessful = true;

			try
			{
				//	Import each of the objections stored in the specified collection
				if((xmlObjections != null) && (xmlObjections.Count > 0))
				{
					//	Set the active case for the import operation
					if(SetOxCase(xmlObjections.CaseId, xmlObjections.CaseName, xmlObjections.CaseVersion) == false)
						return false;

					foreach(CXmlObjection O in xmlObjections)
					{
						//	Make sure the objection is assigned the correct case
						O.TmaxObjection.Case = m_oxCase.TmaxCase;
						
						if(ImportXmlObjection(O) == false)
							bSuccessful = false;

					}// foreach(CXmlObjections O in xmlDeposition.Objections)

				}// if((xmlDeposition.Objections != null) && (xmlDeposition.Objections.Count > 0))

			}
			catch
			{
				AddMessage(ERROR_IMPORT_XML_OBJECTIONS_EX);
				bSuccessful = false;
			}

			return bSuccessful;

		}// private bool ImportXmlObjections(CXmlObjections xmlObjections)

		/// <summary>This method is called to import the specified XML objection</summary>
		/// <param name="xmlObjection">The XML objection to be imported</param>
		/// <returns>true if successful</returns>
		private bool ImportXmlObjection(CXmlObjection xmlObjection)
		{
			bool bSuccessful = false;

			try
            {
                
				Debug.Assert(xmlObjection != null);
				
				if((xmlObjection != null) && (xmlObjection.TmaxObjection != null))
				{
					//	Make sure the correct records have been assigned
					xmlObjection.TmaxObjection.IOxDeposition = m_oxDeposition;
					xmlObjection.TmaxObjection.Case = m_oxCase.TmaxCase;

					if(SetOxState(xmlObjection.TmaxObjection) == false)
						return false;

					if(SetOxRuling(xmlObjection.TmaxObjection) == false)
						return false;

					bSuccessful = ImportObjection(xmlObjection.TmaxObjection);

				}// if((xmlObjection != null) && (xmlObjection.TmaxObjection != null))
	
			}
			catch
			{
				AddMessage(ERROR_IMPORT_XML_OBJECTION_EX);
			}

			return bSuccessful;

		}// private bool ImportXmlObjection(CXmlObjections xmlObjection)

		/// <summary>Called to perform an update of the specified objection</summary>
		/// <param name="oxObjection">The objection to be updated</param>
		/// <param name="tmaxSource">The objection to be used as the update source</param>
		/// <returns>true if successful</returns>
		private bool Update(COxObjection oxObjection, CTmaxObjection tmaxSource)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Copy the source objection properties
				if(IsValid(IMPORT_COLUMN_DEPOSITION) == true)
					oxObjection.TmaxObjection.IOxDeposition = tmaxSource.IOxDeposition;
				
				if(IsValid(IMPORT_COLUMN_RULING) == true)
					oxObjection.TmaxObjection.IOxRuling = tmaxSource.IOxRuling;
				
				if(IsValid(IMPORT_COLUMN_STATE) == true)
					oxObjection.TmaxObjection.IOxState = tmaxSource.IOxState;
				
				if((IsValid(IMPORT_COLUMN_CASE_NAME) == true) ||
				   (IsValid(IMPORT_COLUMN_CASE_ID) == true))
					oxObjection.TmaxObjection.Case = tmaxSource.Case;

				if(IsValid(IMPORT_COLUMN_PLAINTIFF) == true)
					oxObjection.Plaintiff = tmaxSource.Plaintiff;

				if(IsValid(IMPORT_COLUMN_FIRST_PAGE) == true)
					oxObjection.FirstPL = tmaxSource.FirstPL;

				if(IsValid(IMPORT_COLUMN_LAST_PAGE) == true)
					oxObjection.LastPL = tmaxSource.LastPL;

				if(IsValid(IMPORT_COLUMN_ARGUMENT) == true)
					oxObjection.Argument = tmaxSource.Argument;

				if(IsValid(IMPORT_COLUMN_RULING_TEXT) == true)
					oxObjection.RulingText = tmaxSource.RulingText;

				if(IsValid(IMPORT_COLUMN_RESPONSE_1) == true)
					oxObjection.Response1 = tmaxSource.Response1;

				if(IsValid(IMPORT_COLUMN_RESPONSE_2) == true)
					oxObjection.Response2 = tmaxSource.Response2;

				if(IsValid(IMPORT_COLUMN_RESPONSE_3) == true)
					oxObjection.Response3 = tmaxSource.Response3;

				if(IsValid(IMPORT_COLUMN_WORK_PRODUCT) == true)
					oxObjection.WorkProduct = tmaxSource.WorkProduct;

				if(IsValid(IMPORT_COLUMN_COMMENTS) == true)
					oxObjection.Comments = tmaxSource.Comments;

				//	Update the record
				bSuccessful = m_tmaxObjectionsDatabase.OxObjections.Update(oxObjection);
			}
			catch(System.Exception Ex)
			{
				AddMessage(ERROR_UPDATE_EX);
				m_tmaxEventSource.FireDiagnostic(this, "Update", m_tmaxErrorBuilder.Message(ERROR_UPDATE_EX), Ex);
			}
			
			return bSuccessful;

		}// private bool Update(COxObjection oxObjection, CTmaxObjection tmaxSource)

		/// <summary>Called to add the specified application objection to the database</summary>
		/// <param name="tmaxObjection">The application objection object</param>
		/// <returns>The exchange interface for the new record if successful</returns>
		private COxObjection Add(CTmaxObjection tmaxObjection)
		{
			COxObjection oxObjection = null;
			System.Guid	 sysGuid;

			try
			{
				//	Make sure the caller's UniqueId is valid
				if(tmaxObjection.UniqueId.Length > 0)
				{
					try { sysGuid = new Guid(tmaxObjection.UniqueId); }
					catch
					{
						AddMessage(ERROR_INVALID_UNIQUE_ID, tmaxObjection.UniqueId);
						return null;
					}

				}// if(strUniqueId.Length > 0)
				
				//	Add to the database
				oxObjection = m_tmaxObjectionsDatabase.AddObjection(tmaxObjection);				
			}
			catch(System.Exception Ex)
			{
				AddMessage(ERROR_ADD_EX);
				m_tmaxEventSource.FireDiagnostic(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_EX), Ex);
			
				oxObjection = null;
			}

			return oxObjection;

		}// private COxObjection Add(CTmaxObjection tmaxObjection)

		/// <summary>Called to locate a matching objection in the database</summary>
		/// <param name="tmaxObjection">The source objection to search for</param>
		/// <returns>The record for the match if found</returns>
		private COxObjection GetDuplicate(CTmaxObjection tmaxObjection)
		{
			try
			{
				foreach(COxObjection O in m_tmaxObjectionsDatabase.OxObjections)
				{
					if(O.TmaxObjection != null)
					{
						if(O.TmaxObjection.IsDuplicate(tmaxObjection, false) == true)
						{
							return O;
						}
					}

				}// foreach(COxObjection O in m_tmaxObjectionsDatabase.OxObjections)
				
			}
			catch
			{
			}
			
			return null;

		}// private COxObjection GetDuplicate(CTmaxObjection tmaxObjection)
		
		/// <summary>Called to add the record to the results collection</summary>
		/// <param name="tmaxObjection">The objection that has been processed</param>
		/// <param name="eResult">Predefined result type identifier</param>
		private void AddResult(CTmaxObjection tmaxObjection, TmaxImportResults eResult)
		{
           

			string	strObjection = "";
			string	strFilename = "";
			
			//	Format the objection descriptor
			strObjection = String.Format("{0} {1} - {2}", tmaxObjection.Deposition, CTmaxToolbox.PLToString(tmaxObjection.FirstPL), CTmaxToolbox.PLToString(tmaxObjection.LastPL));

			//	What type of result is being reported?
			switch(eResult)
			{ 
				case TmaxImportResults.Added:
					if((m_tmaxResults != null) && (tmaxObjection.IOxObjection != null))
						m_tmaxResults.OnAdded((COxObjection)(tmaxObjection.IOxObjection));
					m_lAdded += 1;
					break;

				case TmaxImportResults.Updated:
					if((m_tmaxResults != null) && (tmaxObjection.IOxObjection != null))
						m_tmaxResults.OnUpdated((COxObjection)(tmaxObjection.IOxObjection));
					m_lUpdated += 1;
					break;

				case TmaxImportResults.Ignored:
				case TmaxImportResults.Conflict:
				case TmaxImportResults.AddFailed:
				case TmaxImportResults.UpdateFailed:
					break;
					
				default:
				
					Debug.Assert(false, "Unknown result identifier: " + eResult.ToString());
					break;
					
			}// switch(iResultId)

			try
			{
				if((m_wndStatusForm != null) && (m_wndStatusForm.IsDisposed == false))
				{
					if(FileSpec.Length > 0)
						strFilename = System.IO.Path.GetFileName(FileSpec);
                    
					m_wndStatusForm.AddResult(strFilename, LineNumber, eResult, strObjection, tmaxObjection);
				}

			}
			catch
			{
			}

		}// private void AddResult(CTmaxObjection tmaxObjection, TmaxImportResults eResult)
		
		/// <summary>Called to set the column indexes using the line parsed from the import file</
		/// <param name="aFields">The line parsed from the import file</param>
		/// <returns>true if all required columns are located</returns>
		private bool SetColumns(string[] aFields)
		{
			bool	bSuccessful = true;
			int		i = 0;

			try
			{
				//	Reset each of the column indexes
				for(i = 0; i <= m_aColumns.GetUpperBound(0); i++)
					m_aColumns[i] = -1;
					
				m_iFileColumns = 0;

				//	Iterate the fields and locate the associated column for each field
				for(i = 0; i <= aFields.GetUpperBound(0); i++)
				{
					if((aFields[i] != null) && (aFields[i].Length > 0))
					{
						switch(aFields[i].ToLower())
						{
							case "uniqueid":
							case "unique id":
							case "id":
							
								m_aColumns[IMPORT_COLUMN_UNIQUE_ID] = i;
								break;
								
							case "deposition":
							case "depositions":
							case "depo":
							
								m_aColumns[IMPORT_COLUMN_DEPOSITION] = i;
								break;

							case "plaintiff":
							case "party":

								m_aColumns[IMPORT_COLUMN_PLAINTIFF] = i;
								break;

							case "state":
							case "status":
							
								m_aColumns[IMPORT_COLUMN_STATE] = i;
								break;

							case "ruling":

								m_aColumns[IMPORT_COLUMN_RULING] = i;
								break;

							case "firstpage":
							case "first page":
							
								m_aColumns[IMPORT_COLUMN_FIRST_PAGE] = i;
								break;
								
							case "firstline":
							case "first line":
							
								m_aColumns[IMPORT_COLUMN_FIRST_LINE] = i;
								break;
								
							case "lastpage":
							case "last page":
							
								m_aColumns[IMPORT_COLUMN_LAST_PAGE] = i;
								break;
								
							case "lastline":
							case "last line":
							
								m_aColumns[IMPORT_COLUMN_LAST_LINE] = i;
								break;
								
							case "objection":
							case "argument":
							case "complaint":
							case "complaints":

								m_aColumns[IMPORT_COLUMN_ARGUMENT] = i;
								break;

							case "rulingtext":
							case "ruling text":

								m_aColumns[IMPORT_COLUMN_RULING_TEXT] = i;
								break;

							case "response":
							case "response1":
							case "response 1":

								m_aColumns[IMPORT_COLUMN_RESPONSE_1] = i;
								break;

							case "response2":
							case "response 2":

								m_aColumns[IMPORT_COLUMN_RESPONSE_2] = i;
								break;

							case "response3":
							case "response 3":

								m_aColumns[IMPORT_COLUMN_RESPONSE_3] = i;
								break;

							case "workproduct":
							case "work product":

								m_aColumns[IMPORT_COLUMN_WORK_PRODUCT] = i;
								break;

							case "comment":
							case "comments":
							
								m_aColumns[IMPORT_COLUMN_COMMENTS] = i;
								break;

							case "case":
							case "casename":

								m_aColumns[IMPORT_COLUMN_CASE_NAME] = i;
								break;

							case "caseid":
							case "case id":

								m_aColumns[IMPORT_COLUMN_CASE_ID] = i;
								break;

							default:
								AddMessage(ERROR_INVALID_COLUMN, aFields[i]);
								break;							
								
						}// switch(aFields[i].ToLower())
						
					}// if((aFields[i] != null) && (aFields[i].Length > 0))

				}// for(i = 0; i <= aFields.GetUpperBound(0); i++)
				
				//	Don't need any specific columns if performing updates
				if(m_aColumns[IMPORT_COLUMN_UNIQUE_ID] < 0)
				{
					//	Must have a deposition
					if((m_oxDeposition == null) && (m_aColumns[IMPORT_COLUMN_DEPOSITION] < 0))
					{
						AddMessage(ERROR_NO_DEPOSITION_COLUMN, TmaxMessageLevels.FatalError);
						bSuccessful = false;
					}

					//	Need the party identifier
					if(m_aColumns[IMPORT_COLUMN_PLAINTIFF] < 0)
					{
						AddMessage(ERROR_NO_PLAINTIFF_COLUMN, TmaxMessageLevels.FatalError);
						bSuccessful = false;
					}
					
					//	Need the page/line extents
					if(m_aColumns[IMPORT_COLUMN_FIRST_PAGE] < 0)
					{
						AddMessage(ERROR_NO_FIRST_PAGE_COLUMN, TmaxMessageLevels.FatalError);
						bSuccessful = false;
					}
					if(m_aColumns[IMPORT_COLUMN_FIRST_LINE] < 0)
					{
						AddMessage(ERROR_NO_FIRST_LINE_COLUMN, TmaxMessageLevels.FatalError);
						bSuccessful = false;
					}
					if(m_aColumns[IMPORT_COLUMN_LAST_PAGE] < 0)
					{
						AddMessage(ERROR_NO_LAST_PAGE_COLUMN, TmaxMessageLevels.FatalError);
						bSuccessful = false;
					}
					if(m_aColumns[IMPORT_COLUMN_LAST_LINE] < 0)
					{
						AddMessage(ERROR_NO_LAST_LINE_COLUMN, TmaxMessageLevels.FatalError);
						bSuccessful = false;
					}
					
				}// if(m_aColumns[IMPORT_COLUMN_UNIQUE_ID] < 0)

			}
			catch(System.Exception Ex)
			{
				AddMessage(ERROR_SET_COLUMNS_EX, TmaxMessageLevels.FatalError);
				m_tmaxEventSource.FireDiagnostic(this, "Add", m_tmaxErrorBuilder.Message(ERROR_SET_COLUMNS_EX), Ex);
			}
			
			//	How many active columns are in the import file?
			if(bSuccessful == true)
			{
				for(i = 0; i <= m_aColumns.GetUpperBound(0); i++)
				{
					if(m_aColumns[i] >= 0)
						m_iFileColumns += 1;
				}
			}
			
			return bSuccessful;

		}// private bool SetColumns(string[] aFields)

		/// <summary>Called to set the active objection case</summary>
		/// <param name="tmaxObjection">The objection to be assigned</param>
		/// <returns>true if successful</returns>
		private bool SetOxCase(CTmaxObjection tmaxObjection)
		{
			bool bSuccessful = false;
			
			if(tmaxObjection != null)
			{
				bSuccessful = SetOxCase(tmaxObjection.CaseId, tmaxObjection.CaseName, "");
			
				if(bSuccessful == true)
					tmaxObjection.Case = m_oxCase.TmaxCase;
			}
			
			return bSuccessful;

		}// private bool SetOxCase(CTmaxObjection tmaxObjection)

		/// <summary>Called to set the active objection case</summary>
		/// <param name="strId">The unique case identifier</param>
		/// <param name="strName">The name of the case</param>
		/// <param name="strVersion">The case version descriptor</param>
		/// <returns>true if successful</returns>
		private bool SetOxCase(string strId, string strName, string strVersion)
		{
			try
			{
				//	Should we ignore case information associated with the import object?
				if((Options.ObjectionsMethod == TmaxImportObjectionMethods.AddAll) && (Options.DiscardObjectionsId == true))
				{
					//	Force use of the active case
					strName = "";
					strId = "";
					m_oxCase = null;
				}

				//	Are we searching by Name?
				if(strName.Length > 0)
				{
					//	Do we have to locate the case?
					if((m_oxCase == null) || (String.Compare(m_oxCase.Name, strName, true) != 0))
					{
						//	Does it exist in the objections database?
						if((m_oxCase = m_tmaxObjectionsDatabase.OxCases.Find(strName, true)) == null)
						{
							m_oxCase = m_tmaxObjectionsDatabase.AddCase(strId, strName, strVersion);
						}

					}// if((m_oxCase == null) || (String.Compare(m_oxCase.MediaId, strMediaId, true) != 0))

				}
				else if(strId.Length > 0)
				{
					if((m_oxCase == null) || (String.Compare(m_oxCase.UniqueId, strId, true) != 0))
					{
						//	Does it exist in the objections database?
						if((m_oxCase = m_tmaxObjectionsDatabase.OxCases.Find(strId, false)) == null)
							m_oxCase = m_tmaxObjectionsDatabase.AddCase(strId, strName, strVersion);

					}// if((m_oxCase == null) || (String.Compare(m_oxCase.MediaId, strMediaId, true) != 0))
				
				}
				else
				{
					//	Assign the active case if necessary
					if(m_oxCase == null)
						m_oxCase = m_tmaxObjectionsDatabase.OxCase;

					if(m_oxCase == null)
						AddMessage(ERROR_NO_CASE);

				}// if(strCaseId.Length > 0)

			}
			catch(System.Exception Ex)
			{
				AddMessage(ERROR_SET_CASE_EX);
				m_tmaxEventSource.FireDiagnostic(this, "SetOxCase", m_tmaxErrorBuilder.Message(ERROR_SET_CASE_EX), Ex);

				m_oxCase = null;
			}

			return (m_oxCase != null);

		}// private bool SetOxCase(string strCaseId, string strName, string strVersion)

		/// <summary>Called to set the exchange interface to the source deposition record</summary>
		/// <param name="tmaxObjection">The objection to be assigned a deposition</param>
		/// <returns>true if successful</returns>
		private bool SetOxDeposition(CTmaxObjection tmaxObjection)
		{
			bool bSuccessful = false;

			if(tmaxObjection != null)
			{
				bSuccessful = SetOxDeposition(tmaxObjection.Deposition);

				if(bSuccessful == true)
					tmaxObjection.IOxDeposition = m_oxDeposition;
			}

			return bSuccessful;

		}// private bool SetOxDeposition(CTmaxObjection tmaxObjection)

		/// <summary>Called to set the exchange interface to the source deposition record</summary>
		/// <param name="strMediaId">The MediaId assigned to the desired deposition</param>
		/// <returns>true if successful</returns>
		private bool SetOxDeposition(string strMediaId)
		{
			Debug.Assert(m_tmaxCaseDatabase != null);
			Debug.Assert(m_tmaxObjectionsDatabase != null);

			try
			{
				//	Do we have a valid media Id?
				if(strMediaId.Length > 0)
				{
					//	Do we have to locate the deposition?
					if((m_oxDeposition == null) || (String.Compare(m_oxDeposition.MediaId, strMediaId, true) != 0))
					{
						//	Find the deposition record in the case database
						m_dxDeposition = m_tmaxCaseDatabase.Primaries.Find(strMediaId);

						//	Does it exist in the objections database?
						if((m_oxDeposition = m_tmaxObjectionsDatabase.OxDepositions.Find(strMediaId, true)) == null)
						{
							//	We have to add a deposition to the database
							if((m_dxDeposition != null) && (m_dxDeposition.Transcript != null))
							{
								m_oxDeposition = m_tmaxObjectionsDatabase.AddDeposition(m_dxDeposition);
							}
							else
							{
								AddMessage(ERROR_DEPOSITION_NOT_FOUND, strMediaId);
							}

						}// if((m_oxDeposition = m_tmaxObjectionsDatabase.Depositions.Find(strMediaId, true)) == null)

					}// if((m_oxDeposition == null) || (String.Compare(m_oxDeposition.MediaId, strMediaId, true) != 0))

				}// if(strMediaId.Length > 0)
				else
				{
					//	Warn the user if no deposition is available
					if(m_oxDeposition == null)
						AddMessage(ERROR_NO_DEPOSITION);
				}

			}
			catch(System.Exception Ex)
			{
				AddMessage(ERROR_SET_DEPOSITION_EX);
				m_tmaxEventSource.FireError(this, "SetOxDeposition", m_tmaxErrorBuilder.Message(ERROR_SET_DEPOSITION_EX), Ex);

				m_oxDeposition = null;
				m_dxDeposition = null;
			}

			return (m_oxDeposition != null);

		}// private bool SetOxDeposition(string strMediaID)

		/// <summary>Called to set the active objection state</summary>
		/// <param name="tmaxObjection">The objection to be assigned the state record</param>
		/// <returns>true if successful</returns>
		private bool SetOxState(CTmaxObjection tmaxObjection)
		{
			string	strLabel = tmaxObjection.State;

			try
			{
				//	Do we have a valid state label?
				if(strLabel.Length > 0)
				{
					//	Do we have to locate the state?
					if((m_oxState == null) || (String.Compare(m_oxState.Label, strLabel, true) != 0))
					{
						//	Does it exist in the objections database?
						if((m_oxState = m_tmaxObjectionsDatabase.OxStates.Find(strLabel, true)) == null)
						{
							//	We have to add a state to the database
							if(m_tmaxOptions.AddObjectionStates == true)
							{
								m_oxState = m_tmaxObjectionsDatabase.AddState(strLabel);
							}
							else
							{
								AddMessage(ERROR_STATE_NOT_FOUND, strLabel);
							}

						}// if((m_oxState = m_tmaxObjectionsDatabase.States.Find(strLabel)) == null)

					}// if((m_oxState == null) || (String.Compare(m_oxState.MediaId, strMediaId, true) != 0))

				}// if(strLabel.Length > 0)
				else
				{
					//	Force assignment of default if this column does not exist
					if(m_aColumns[IMPORT_COLUMN_STATE] < 0)
						m_oxState = null;

					//	Should we assign the default?
					if((m_oxState == null) && (m_tmaxObjectionsDatabase.OxStates.Count > 0))
						m_oxState = m_tmaxObjectionsDatabase.OxStates[0];

					if(m_oxState == null)
						AddMessage(ERROR_NO_STATE);

				}// if(strLabel.Length > 0)

			}
			catch(System.Exception Ex)
			{
				AddMessage(ERROR_SET_STATE_EX);
				m_tmaxEventSource.FireDiagnostic(this, "SetState", m_tmaxErrorBuilder.Message(ERROR_SET_STATE_EX), Ex);

				m_oxState = null;
			}
			
			tmaxObjection.IOxState = m_oxState;
			
			return (m_oxState != null);

		}// private bool SetOxState(CTmaxObjection tmaxObjection)

		/// <summary>Called to set the active objection ruling</summary>
		/// <param name="tmaxObjection">The objection being assigned a ruling</param>
		/// <returns>true if successful</returns>
		private bool SetOxRuling(CTmaxObjection tmaxObjection)
		{
			string strLabel = tmaxObjection.Ruling;

			try
			{
				//	Do we have a valid ruling label?
				if(strLabel.Length > 0)
				{
					//	Do we have to locate the ruling?
					if((m_oxRuling == null) || (String.Compare(m_oxRuling.Label, strLabel, true) != 0))
					{
						//	Does it exist in the objections database?
						if((m_oxRuling = m_tmaxObjectionsDatabase.OxRulings.Find(strLabel, true)) == null)
						{
							//	We have to add a ruling to the database
							if(m_tmaxOptions.AddObjectionRulings == true)
							{
								m_oxRuling = m_tmaxObjectionsDatabase.AddRuling(strLabel);
							}
							else
							{
								AddMessage(ERROR_RULING_NOT_FOUND, strLabel);
							}

						}// if((m_oxRuling = m_tmaxObjectionsDatabase.Rulings.Find(strLabel)) == null)

					}// if((m_oxRuling == null) || (String.Compare(m_oxRuling.MediaId, strMediaId, true) != 0))

				}// if(strLabel.Length > 0)
				else
				{
					//	Force assignment of default if this column does not exist
					if(m_aColumns[IMPORT_COLUMN_RULING] < 0)
						m_oxRuling = null;

					//	Should we assign the default?
					if((m_oxRuling == null) && (m_tmaxObjectionsDatabase.OxRulings.Count > 0))
						m_oxRuling = m_tmaxObjectionsDatabase.OxRulings[0];

					if(m_oxRuling == null)
					{
						AddMessage(ERROR_NO_RULING);
					}

				}// if(strLabel.Length > 0)

			}
			catch(System.Exception Ex)
			{
				AddMessage(ERROR_SET_RULING_EX);
				m_tmaxEventSource.FireDiagnostic(this, "SetRuling", m_tmaxErrorBuilder.Message(ERROR_SET_RULING_EX), Ex);

				m_oxRuling = null;
			}

			tmaxObjection.IOxRuling = m_oxRuling;
			
			return (m_oxRuling != null);

		}// private bool SetOxRuling(CTmaxObjection tmaxObjection)

		/// <summary>Called to set the active Plaintiff flag</summary>
		/// <param name="tmaxObjection">The objection being assigned</param>
		/// <param name="aFields">The line parsed from the import file</param>
		/// <returns>The Plaintiff value</returns>
		private bool SetPlaintiff(CTmaxObjection tmaxObjection, string[] aFields)
		{
			string strValue = "";

			try
			{
				//	Get the value from the fields provided by the caller
				strValue = GetColumnValue(IMPORT_COLUMN_PLAINTIFF, aFields);

				//	Do we have a valid Plaintiff value?
				if(strValue.Length > 0)
				{
					m_bPlaintiff = CTmaxToolbox.StringToBool(strValue);
				}

			}
			catch(System.Exception Ex)
			{
				AddMessage(ERROR_SET_PLAINTIFF_EX);
				m_tmaxEventSource.FireDiagnostic(this, "SetPlaintiff", m_tmaxErrorBuilder.Message(ERROR_SET_PLAINTIFF_EX), Ex);
			}

			tmaxObjection.Plaintiff = m_bPlaintiff;
			
			return m_bPlaintiff;

		}// private bool SetPlaintiff(CTmaxObjection tmaxObjection, string[] aFields)

		/// <summary>Called to set the page line extents using the values supplied by the caller</summary>
		/// <param name="tmaxObjection">The objection being modified</param>
		/// <param name="aFields">The line parsed from the import file</param>
		/// <returns>true if successful</returns>
		private bool SetRange(CTmaxObjection tmaxObjection, string[] aFields)
		{
			string			strFirstPage = "";
			string			strFirstLine = "";
			string			strLastPage = "";
			string			strLastLine = "";
			long			lFirstPage = 0;
			long			lLastPage = 0;
			long			lFirstPL = 0;
			long			lLastPL = 0;
			int				iFirstLine = 0;
			int				iLastLine = 0;
			bool			bSuccessful = false;
			COxDeposition	oxDeposition = null;

			Debug.Assert(tmaxObjection != null);
			if(tmaxObjection == null) return false;
			Debug.Assert(aFields != null);
			if(aFields == null) return false;

			//	Get the strings used to set the page/line extents
			strFirstPage = GetColumnValue(IMPORT_COLUMN_FIRST_PAGE, aFields);
			strFirstLine = GetColumnValue(IMPORT_COLUMN_FIRST_LINE, aFields);
			strLastPage  = GetColumnValue(IMPORT_COLUMN_LAST_PAGE, aFields);
			strLastLine  = GetColumnValue(IMPORT_COLUMN_LAST_LINE, aFields);

			while(bSuccessful == false)
			{
				//	At the very least we need a start page / line
				if((strFirstPage.Length == 0) || (strFirstLine.Length == 0))
				{
					AddMessage(ERROR_NO_FIRST_PL);
					break;
				}
			
				//	Get the first page
				try { lFirstPage = System.Convert.ToInt64(strFirstPage); }
				catch { }
				if(lFirstPage <= 0)
				{
					AddMessage(ERROR_INVALID_FIRST_PAGE);
					break;
				}

				//	Get the first line
				try { iFirstLine = System.Convert.ToInt32(strFirstLine); }
				catch { }
				if(iFirstLine <= 0) 
				{
					AddMessage(ERROR_INVALID_FIRST_LINE);
					break;
				}

				//	Get the last page
				if(strLastPage.Length > 0)
				{
					try { lLastPage = System.Convert.ToInt64(strLastPage); }
					catch { }
					if(lLastPage <= 0)
					{
						AddMessage(ERROR_INVALID_LAST_PAGE);
						break;
					}

				}
				else
				{
					lLastPage = lFirstPage;
				}

				//	Get the last line
				if(strLastLine.Length > 0)
				{
					try { iLastLine = System.Convert.ToInt32(strLastLine); }
					catch { }
					if(iLastLine <= 0)
					{
						AddMessage(ERROR_INVALID_LAST_LINE);
						break;
					}

				}
				else
				{
					iLastLine = iFirstLine;
				}

                // Check Line number exists in Objection
                if (iFirstLine > m_dxDeposition.Transcript.LinesPerPage)
                {
                    AddMessage(ERROR_FIRST_OUT_OF_BOUNDS);
                    break;
                }
                if(iLastLine>m_dxDeposition.Transcript.LinesPerPage)
                {
                    AddMessage(ERROR_LAST_OUT_OF_BOUNDS);
                    break;
                }

				//	Convert to PL values
				lFirstPL = CTmaxToolbox.GetPL(lFirstPage, iFirstLine);
				lLastPL = CTmaxToolbox.GetPL(lLastPage, iLastLine);

				if(lFirstPL > lLastPL)
				{
					AddMessage(ERROR_INVALID_PL_RANGE);
					break;
				}
				
				//	Are the values within the range defined by the deposition?
				if((oxDeposition = (COxDeposition)(tmaxObjection.IOxDeposition)) != null)
				{
				    
					if((oxDeposition.FirstPL > 0) && (lFirstPL < oxDeposition.FirstPL))
					{
						AddMessage(ERROR_FIRST_OUT_OF_BOUNDS);
						break;
					}
					if((oxDeposition.LastPL > 0) && (lLastPL > oxDeposition.LastPL))
					{
						AddMessage(ERROR_LAST_OUT_OF_BOUNDS);
						break;
					}

				}// if((oxDeposition = (COxDeposition)(tmaxObjection.IOxDeposition)) != null)
				
				//	It's all good
				bSuccessful = true;

			}// while(bSuccessful == false)

			//	Should we update the caller's record?
			if(bSuccessful == true)
			{
				tmaxObjection.FirstPL = lFirstPL;
				tmaxObjection.LastPL = lLastPL;
			}
			
			return bSuccessful;

		}// private bool SetRange(CTmaxObjection tmaxObjection, string[] aFields)

		/// <summary>Called to create and initialize an objection using the fields parsed from the import file</summary>
		/// <param name="aFields">The line parsed from the import file</param>
		/// <returns>The initialized objection object</returns>
		private CTmaxObjection CreateObjection(string[] aFields)
		{
			CTmaxObjection	tmaxObjection = null;
			bool			bSuccessful = false;
			
			try
			{
				//	Allocate the new application objection
				tmaxObjection = new CTmaxObjection();
				
				//	Use the fields from the import file to set the property values
				tmaxObjection.UniqueId = GetColumnValue(IMPORT_COLUMN_UNIQUE_ID, aFields);
				tmaxObjection.CaseId = GetColumnValue(IMPORT_COLUMN_CASE_ID, aFields);
				tmaxObjection.CaseName = GetColumnValue(IMPORT_COLUMN_CASE_NAME, aFields);
				tmaxObjection.Deposition = GetColumnValue(IMPORT_COLUMN_DEPOSITION, aFields);
				tmaxObjection.State = GetColumnValue(IMPORT_COLUMN_STATE, aFields);
				tmaxObjection.Ruling = GetColumnValue(IMPORT_COLUMN_RULING, aFields);
				tmaxObjection.Argument = GetColumnValue(IMPORT_COLUMN_ARGUMENT, aFields);
				tmaxObjection.RulingText = GetColumnValue(IMPORT_COLUMN_RULING_TEXT, aFields);
				tmaxObjection.Response1 = GetColumnValue(IMPORT_COLUMN_RESPONSE_1, aFields);
				tmaxObjection.Response2 = GetColumnValue(IMPORT_COLUMN_RESPONSE_2, aFields);
				tmaxObjection.Response3 = GetColumnValue(IMPORT_COLUMN_RESPONSE_3, aFields);
				tmaxObjection.WorkProduct = GetColumnValue(IMPORT_COLUMN_WORK_PRODUCT, aFields);
				tmaxObjection.Comments = GetColumnValue(IMPORT_COLUMN_COMMENTS, aFields);

				//	Complete the initialization
				while(bSuccessful == false)
				{
					//	Set the case record
					if(SetOxCase(tmaxObjection) == false)
						break;

					//	Set the deposition record
					if(SetOxDeposition(tmaxObjection) == false)
						break;

					//	Set the party identifier
					SetPlaintiff(tmaxObjection, aFields);

					//	Set the active objection state
					if(SetOxState(tmaxObjection) == false)
						break;

					//	Set the active objection ruling
					if(SetOxRuling(tmaxObjection) == false) 
						break;

					//	Set the page / line range
					if(SetRange(tmaxObjection, aFields) == false)
						break;
				
					bSuccessful = true; // We're done

				}// while(bSuccessful == false)

			}
			catch(System.Exception Ex)
			{
				AddMessage(ERROR_CREATE_OBJECTION_EX);
				m_tmaxEventSource.FireError(this, "CreateObjection", m_tmaxErrorBuilder.Message(ERROR_CREATE_OBJECTION_EX), Ex);
				tmaxObjection = null;
			}

			return ((bSuccessful == true) ?  tmaxObjection : null);

		}// private CTmaxObjection CreateObjection(string[] aFields)

		/// <summary>This method is called to get the value for the specified column</summary>
		/// <param name="iColumn">The column index</param>
		/// <param name="aFields">The collection of fields parsed from the import line</param>
		/// <returns>The value for the specified column if it exists</returns>
		private string GetColumnValue(int iColumn, string[] aFields)
		{
			string	strValue = "";
			int		iIndex = -1;

			// I'm not perfect ......
			Debug.Assert(iColumn >= 0);
			if(iColumn < 0) return "";
			Debug.Assert(iColumn < MAX_IMPORT_COLUMNS);
			if(iColumn >= MAX_IMPORT_COLUMNS) return "";
			Debug.Assert(aFields != null);
			if(aFields == null) return "";

			try
			{
				if((iIndex = m_aColumns[iColumn]) >= 0)
				{
					if(iIndex <= aFields.GetUpperBound(0))
					{
						strValue = aFields[iIndex];
						
						//	Is this one of our multi-line fields?
						switch(iColumn)
						{
							case IMPORT_COLUMN_ARGUMENT:
							case IMPORT_COLUMN_RULING_TEXT:
							case IMPORT_COLUMN_RESPONSE_1:
							case IMPORT_COLUMN_RESPONSE_2:
							case IMPORT_COLUMN_RESPONSE_3:
							case IMPORT_COLUMN_WORK_PRODUCT:
							case IMPORT_COLUMN_COMMENTS:
							
								//	Restore CR/LF
								strValue = Expand(strValue);
								break;
								
							default:
							
								break;

						}// switch(iColumn)

					}// if(iIndex <= aFields.GetUpperBound(0))
						
					

				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetColumnValue", Ex);
			}

			return strValue;

		}// private string GetColumnValue(int iColumn, string aFields)

		/// <summary>This method is called to determine if the specified column is valid</summary>
		/// <param name="iColumn">The column index</param>
		/// <returns>true if valid</returns>
		private bool IsValid(int iColumn)
		{
			bool bValid = false;

			try
			{
				if((iColumn >= 0) && (iColumn < MAX_IMPORT_COLUMNS))
				{
					bValid = (m_aColumns[iColumn] >= 0);

				}// if((iColumn >= 0) && (iColumn < MAX_IMPORT_COLUMNS))

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "IsValid", Ex);
			}

			return bValid;

		}// private bool IsValid(int iColumn)

		/// <summary>This method expands CR/LF in the string specified by the caller</summary>
		/// <param name="strValue">The value to be expanded</param>
		/// <returns>the adjusted (if necessary) value</returns>
		private string Expand(string strValue)
		{
			string strSubstitute = "";

			try
			{
				if((strValue != null) && (strValue.Length > 0) && (m_tmaxOptions != null))
				{
					//	Get the substitution characters
					if(m_tmaxOptions.CRLFSubstitution != TmaxImportCRLF.None)
						strSubstitute = m_tmaxOptions.GetCRLFSubstitution();

					if(strSubstitute.Length > 0)
					{
						strValue = strValue.Replace(strSubstitute,"\r\n");
						strValue = strValue.Replace(strSubstitute.ToLower(),"\r\n");
						strValue = strValue.Replace(strSubstitute.ToUpper(),"\r\n");
					}

				}

			}
			catch
			{
			}

			return strValue;

		}// private string Expand(string strValue)

		/// <summary>This method parses the next line in the stream</summary>
		///	<param name="strCommentCharacters">Characters to identify comment lines</param>
		/// <param name="strDelimiters">The delimiters used to separate the fields</param>
		/// <param name="bStripEmpty">True to strip empty fields at the end of the line</param>
		/// <returns>The array of fields parsed from the line of text</returns>
		private string[] Parse(string strCommentCharacters, string strDelimiters, bool bStripEmpty)
		{
			string strLine = "";
			string[] aFields = null;
			string[] aCleaned = null;
			int iRemove = -1;

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

				try { aFields = strLine.Split(strDelimiters.ToCharArray()); }
				catch { }

				if((aFields != null) && (aFields.GetUpperBound(0) >= 0))
				{
					//	Reset
					iRemove = -1;

					//	Trim all whitespace
					for(int i = 0;i <= aFields.GetUpperBound(0);i++)
					{
						try { aFields[i] = aFields[i].Trim(); }
						catch { }

						aFields[i] = CTmaxToolbox.StripQuotes(aFields[i], true);
						
						//	Keep track of empty strings that appear at the end of the collection
						if(aFields[i].Length == 0)
						{
							if(iRemove < 0)
								iRemove = i;
						}
						else
						{
							iRemove = -1;
						}

					}// for(int i = 0; i <= aFields.GetUpperBound(0); i++)

					//	For this to be a good we must have at least one valid field
					if(iRemove != 0)
					{
						//	Strip all trailing empty fields
						//
						//	NOTE:	We have to leave embedded empties in place because they
						//			may be placeholders for optional values
						if((iRemove > 0) && (bStripEmpty == true))
						{
							aCleaned = new string[iRemove];
							for(int i = 0;i < iRemove;i++)
								aCleaned[i] = aFields[i];
						}
						else
						{
							aCleaned = aFields;
						}

						//	We're done	
						break;

					}// if(iRemove != 0)

				}// if((aFields != null) && (aFields.GetUpperBound(0) >= 0))

			}// while((strLine = ReadLine()) != null)

			return aCleaned;

		}// private string[] Parse(string strCommentCharacters, string strDelimiters, bool bStripEmpty)

		/// <summary>This method parses the next line in the stream using the local option settings</summary>
		/// <param name="bStripEmpty">True to strip empty fields at the end of the line</param>
		/// <returns>The array of fields parsed from the line of text</returns>
		private string[] Parse(bool bStripEmpty)
		{
			return Parse(m_strCommentCharacters, m_strDelimiters, bStripEmpty);
		}

		/// <summary>This method parses the next line in the stream using the local option settings</summary>
		/// <returns>The array of fields parsed from the line of text</returns>
		private string[] Parse()
		{
			return Parse(m_strCommentCharacters, m_strDelimiters, true);
		}

		/// <summary>This method is called to prompt the user for the options used to import the objections</summary>
		/// <returns>true to continue, false to cancel</returns>
		private bool GetOptions()
		{
			bool bContinue = false;

			try
			{
				FTI.Trialmax.Forms.CFImportObjections wndOptions = new CFImportObjections();

				m_tmaxEventSource.Attach(wndOptions.EventSource);
				wndOptions.ImportOptions = m_tmaxOptions;

				if(wndOptions.ShowDialog() == DialogResult.OK)
					bContinue = true;

			}
			catch
			{

			}

			return bContinue;

		}// private bool GetOptions()

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to initialize the import operation");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to get the source files for the import operation");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to execute the import operation");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create the status form for the import operation");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate %1 to perform the import operation");

			m_tmaxErrorBuilder.FormatStrings.Add("Unable to open %1 to perform the import operation");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to import from %1");
			m_tmaxErrorBuilder.FormatStrings.Add("%1 is not a valid column name");
			m_tmaxErrorBuilder.FormatStrings.Add("Deposition column not found");
			m_tmaxErrorBuilder.FormatStrings.Add("Invalid UniqueId: %1");

			m_tmaxErrorBuilder.FormatStrings.Add("First Page column not found");
			m_tmaxErrorBuilder.FormatStrings.Add("First Line column not found");
			m_tmaxErrorBuilder.FormatStrings.Add("Last Page column not found");
			m_tmaxErrorBuilder.FormatStrings.Add("Last Line column not found");
			m_tmaxErrorBuilder.FormatStrings.Add("Plaintiff column not found");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to import the text file");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the import columns");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add an objection");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to update an objection");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the import deposition");

			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the deposition in the case database: MediaId = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("No deposition specified for the new record");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the objection state");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the objection state in the database: Label = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("No objection state specified for the new record");

			m_tmaxErrorBuilder.FormatStrings.Add("No first page/line specification");
			m_tmaxErrorBuilder.FormatStrings.Add("Invalid first page");
			m_tmaxErrorBuilder.FormatStrings.Add("Invalid first line");
			m_tmaxErrorBuilder.FormatStrings.Add("Invalid last page");
			m_tmaxErrorBuilder.FormatStrings.Add("Invalid last line");

			m_tmaxErrorBuilder.FormatStrings.Add("Invalid first/last page lines");
			m_tmaxErrorBuilder.FormatStrings.Add("First page/line out of bounds");
			m_tmaxErrorBuilder.FormatStrings.Add("Last page/line out of bounds");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the Plaintiff flag");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the objection ruling");
			
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the objection ruling in the database: Label = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("No objection ruling specified for the new record");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create an objection");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the case identifier");
			m_tmaxErrorBuilder.FormatStrings.Add("No case identifier specified for the new objection record");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to import an XML script: MediaID = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to import an XML deposition: MediaID = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to import an objection");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to import a collection of XML objections");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to import an XML objection");

			m_tmaxErrorBuilder.FormatStrings.Add("Invalid import line - insufficient field count: expected = %1 fields = %2");

		}// private void SetErrorStrings()

		/// <summary>This method is called to add a warning message to the status form list</summary>
		/// <param name="strMessage">The message to be added</param>
		private void AddMessage(string strMessage)
		{
			AddMessage(strMessage,TmaxMessageLevels.Warning);
		}

		/// <summary>This method is called to add a message to the status form list</summary>
		/// <param name="iErrorId">Error identifier</param>
		/// <param name="param1">Parameter 1 to construct the message</param>
		/// <param name="param2">Parameter 2 to construct the message</param>
		/// <param name="param3">Parameter 3 to construct the message</param>
		/// <param name="eType">Enumerated message error level identifier</param>
		private void AddMessage(int iErrorId, object param1, object param2, object param3, TmaxMessageLevels eLevel)
		{
			AddMessage(m_tmaxErrorBuilder.Message(iErrorId, param1, param2, param3), eLevel);
		}

		/// <summary>This method is called to add a message to the status form list</summary>
		/// <param name="iErrorId">Error identifier</param>
		/// <param name="param1">Parameter 1 to construct the message</param>
		/// <param name="param2">Parameter 2 to construct the message</param>
		/// <param name="eType">Enumerated message error level identifier</param>
		private void AddMessage(int iErrorId, object param1, object param2, TmaxMessageLevels eLevel)
		{
			AddMessage(m_tmaxErrorBuilder.Message(iErrorId, param1, param2), eLevel);
		}

		/// <summary>This method is called to add a message to the status form list</summary>
		/// <param name="iErrorId">Error identifier</param>
		/// <param name="param1">Parameter 1 to construct the message</param>
		/// <param name="eType">Enumerated message error level identifier</param>
		private void AddMessage(int iErrorId, object param1, TmaxMessageLevels eLevel)
		{
			AddMessage(m_tmaxErrorBuilder.Message(iErrorId, param1), eLevel);
		}

		/// <summary>This method is called to add a message to the status form list</summary>
		/// <param name="iErrorId">Error identifier</param>
		/// <param name="eType">Enumerated message error level identifier</param>
		private void AddMessage(int iErrorId, TmaxMessageLevels eLevel)
		{
			AddMessage(m_tmaxErrorBuilder.Message(iErrorId), eLevel);
		}

		/// <summary>This method is called to add a warning message to the status form list</summary>
		/// <param name="iErrorId">Error identifier</param>
		/// <param name="param1">Parameter 1 to construct the message</param>
		/// <param name="param2">Parameter 2 to construct the message</param>
		/// <param name="param3">Parameter 3 to construct the message</param>
		private void AddMessage(int iErrorId,object param1,object param2,object param3)
		{
			AddMessage(iErrorId,param1,param2,param3,TmaxMessageLevels.Warning);
		}

		/// <summary>This method is called to add a warning message to the status form list</summary>
		/// <param name="iErrorId">Error identifier</param>
		/// <param name="param1">Parameter 1 to construct the message</param>
		/// <param name="param2">Parameter 2 to construct the message</param>
		private void AddMessage(int iErrorId,object param1,object param2)
		{
			AddMessage(iErrorId,param1,param2,TmaxMessageLevels.Warning);
		}

		/// <summary>This method is called to add a warning message to the status form list</summary>
		/// <param name="iErrorId">Error identifier</param>
		/// <param name="param1">Parameter 1 to construct the message</param>
		private void AddMessage(int iErrorId,object param1)
		{
			AddMessage(iErrorId,param1,TmaxMessageLevels.Warning);
		}

		/// <summary>This method is called to add a warning message to the status form list</summary>
		/// <param name="iErrorId">Error identifier</param>
		private void AddMessage(int iErrorId)
		{
			AddMessage(iErrorId,TmaxMessageLevels.Warning);
		}

		#endregion Private Methods

		#region Properties

		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}

		/// <summary>The active case database</summary>
		public CTmaxCaseDatabase CaseDatabase
		{
			get { return m_tmaxCaseDatabase; }
			set { m_tmaxCaseDatabase = value; }
		}

		/// <summary>The active objections database</summary>
		public CObjectionsDatabase ObjectionsDatabase
		{
			get { return m_tmaxObjectionsDatabase; }
			set { m_tmaxObjectionsDatabase = value; }
		}

		/// <summary>The active set of import options</summary>
		public CTmaxImportOptions Options
		{
			get { return m_tmaxOptions; }
			set { m_tmaxOptions = value; }
		}

		/// <summary>The object used to report the results of the operation</summary>
		public CTmaxDatabaseResults Results
		{
			get { return m_tmaxResults; }
			set { m_tmaxResults = value; }
		}

		/// <summary>Form displayed during the import operation</summary>
		public FTI.Trialmax.Forms.CFImportStatus StatusForm
		{
			get { return m_wndStatusForm; }
			set { m_wndStatusForm = value; }
		}

		/// <summary>Array of source files to be imported</summary>
		public string[] SourceFiles
		{
			get { return m_aSourceFiles; }
			set { m_aSourceFiles = value; }
		}

		/// <summary>Folder containing the source files to be imported</summary>
		public string SourceFolder
		{
			get { return m_strSourceFolder; }
			set { m_strSourceFolder = value; }
		}

		/// <summary>Fully qualified path to the active file stream</summary>
		public string FileSpec
		{
			get { return m_strFileSpec; }
			set
			{
				m_strFileSpec = value;

				if((m_wndStatusForm != null) && (m_wndStatusForm.IsDisposed == false))
				{
					m_wndStatusForm.Filename = System.IO.Path.GetFileName(m_strFileSpec);
					m_wndStatusForm.Refresh();
				}
			}

		}// public string FileSpec

		/// <summary>Line number currently being processed in the file stream</summary>
		public int LineNumber
		{
			get { return m_iLineNumber; }
			set
			{
				m_iLineNumber = value;

				if((m_wndStatusForm != null) && (m_wndStatusForm.IsDisposed == false))
				{
					m_wndStatusForm.Line = m_iLineNumber;
					m_wndStatusForm.Refresh();
				}
			}

		}// public int LineNumber

		/// <summary>True if the user has cancelled the operation</summary>
		public bool Cancelled
		{
			get
			{
				if((m_wndStatusForm != null) && (m_wndStatusForm.IsDisposed == false))
					return m_wndStatusForm.Aborted;
				else
					return m_bCancelled;
			}
			set { m_bCancelled = value; }
		}

		#endregion Properties

	}// public class COImportManager

}// namespace FTI.Trialmax.Database

