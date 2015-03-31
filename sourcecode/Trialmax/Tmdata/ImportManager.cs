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
using FTI.Shared.Text;



namespace FTI.Trialmax.Database
{
	/// <summary>This class manages the import operations for the database</summary>
	public class CTmaxImportManager
	{
		#region Constants
		
		private const int CASE_CODE_BARCODE_ID			= -0xFF;
		private const string CASE_CODE_BARCODE_NAME	= "Barcode";
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_INITIALIZE_EX					= 0;
		private const int ERROR_SET_TARGET_EX					= 1;
		private const int ERROR_GET_SOURCE_FILES_EX				= 2;
		private const int ERROR_IMPORT_EX						= 3;
		private const int ERROR_CREATE_STATUS_FORM_EX			= 4;
		private const int ERROR_FILE_NOT_FOUND					= 5;
		private const int ERROR_FILE_OPEN_FAILED				= 6;
		private const int ERROR_IMPORT_STREAM_EX				= 7;
		private const int ERROR_INVALID_DESIGNATION_LINE		= 8;
		private const int ERROR_INVALID_BARCODE					= 9;
		private const int ERROR_INVALID_DESIGNATION_EXTENTS		= 10;
		private const int ERROR_INVALID_DEPOSITION_MEDIA_ID		= 11;
		private const int ERROR_NO_DEPOSITION_SPECIFIED			= 12;
		private const int ERROR_INVALID_HIGHLIGHTER				= 13;
		private const int ERROR_START_PL_OUT_OF_RANGE			= 14;
		private const int ERROR_STOP_PL_OUT_OF_RANGE			= 15;
		private const int ERROR_START_STOP_REVERSED				= 16;
		private const int ERROR_CREATE_XML_DESIGNATIONS			= 17;
		private const int ERROR_INVALID_SEGMENT					= 18;
		private const int ERROR_GET_CASE_CODES_EX				= 19;
		private const int ERROR_CASE_CODE_NOT_FOUND				= 20;
		private const int ERROR_IMPORT_PROPERTY_NOT_FOUND		= 21;
		private const int ERROR_CODES_NO_BARCODE_COLUMN			= 22;
		private const int ERROR_CODES_INVALID_BARCODE			= 23;
		private const int ERROR_NO_CODES_COLLECTION				= 24;
		private const int ERROR_SET_CODE_EX						= 25;
		private const int ERROR_SET_CODE_INVALID				= 26;
		private const int ERROR_CODES_EXPRESSION_EMPTY			= 27;
		private const int ERROR_CODES_EXPRESSION_INVALID		= 28;
		private const int ERROR_CODE_LENGTH_EXCEEDED			= 29;
		private const int ERROR_LOW_FIELD_COUNT					= 30;
		private const int ERROR_SPLIT_FILESPEC_EX				= 31;
		private const int ERROR_UPDATE_EX						= 32;
		private const int ERROR_CREATE_BACKUP_EX				= 33;
		private const int ERROR_CREATE_BACKUP_FAILED			= 34;
		private const int ERROR_GET_BINDER_ENTRIES_EX			= 35;
		private const int ERROR_UPDATE_BINDER_ENTRIES_EX		= 36;
		private const int ERROR_CREATE_XML_SCENES_FAILED		= 37;
		private const int ERROR_NO_XML_SCENES					= 38;
		private const int ERROR_IMPORT_XML_SCENE_EX				= 39;
		private const int ERROR_XML_NO_SOURCE_ID				= 40;
		private const int ERROR_XML_SOURCE_NOT_FOUND			= 41;
		private const int ERROR_GET_XML_PRIMARY_EX				= 42;
		private const int ERROR_GET_XML_SECONDARY_EX			= 43;
		private const int ERROR_XML_DESIGNATION_NOT_FOUND		= 44;
		private const int ERROR_XML_PRIMARY_NOT_FOUND			= 45;
		private const int ERROR_XML_SECONDARY_NOT_FOUND			= 46;
		private const int ERROR_ADD_XML_DEPOSITION_EX			= 47;
		private const int ERROR_XML_SAVE_DEPOSITION_FAILED		= 48;
		private const int ERROR_REPLACE_EX						= 49;
		private const int ERROR_XML_SCENE_NOT_FOUND				= 50;
		private const int ERROR_DUPLICATE_PICK_ITEM				= 51;
		private const int ERROR_IMPORT_PICK_LIST_EX				= 52;
		private const int ERROR_IMPORT_XML_CASE_CODES_EX		= 53;
		private const int ERROR_ADD_PICK_ITEM_EX				= 54;
		private const int ERROR_ADD_PICK_ITEM_FAILED			= 55;
		private const int ERROR_SET_CODE_NO_ADDITIONS			= 56;
		private const int ERROR_OPEN_CODES_DATABASE_EX			= 57;
		private const int ERROR_OPEN_CODES_DATABASE_FAILED		= 58;
		private const int ERROR_CODES_DATABASE_EMPTY			= 59;
		private const int ERROR_NO_XML_BINDERS					= 60;
		private const int ERROR_NO_MULTILEVEL_ADDITIONS			= 61;
		private const int ERROR_CLIP_START_INVALID				= 62;
		private const int ERROR_CLIP_STOP_INVALID				= 63;
		private const int ERROR_CLIP_START_STOP_REVERSED		= 64;
		private const int ERROR_NO_BARCODE						= 65;
		private const int ERROR_NO_CONCATENATED_CODES			= 66;
		private const int ERROR_IMPORT_XML_OBJECTIONS_EX		= 67;
		private const int ERROR_NO_OBJECTIONS_DATABASE			= 68;
		private const int ERROR_NO_OBJECTIONS_DEPOSITION		= 69;
			
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bounded to EventSource property</summary>
		protected FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member used to construct error messages</summary>
		protected FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();
		
		/// <summary>Local member bound to Database property</summary>
		private CTmaxCaseDatabase m_tmaxDatabase = null;
		
		/// <summary>Local member bound to ImportOptions property</summary>
		protected CTmaxImportOptions m_tmaxImportOptions = null;
		
		/// <summary>Local member bound to Format property</summary>
		private TmaxImportFormats m_eFormat = TmaxImportFormats.Unknown;
		
		/// <summary>Local member bound to StatusForm property</summary>
		private CFImportStatus m_wndStatusForm = null;
		
		/// <summary>Local member bound to Stream property</summary>
		private System.IO.StreamReader m_streamReader = null;
		
		/// <summary>Local class member bound to SourceFiles property</summary>
		private string [] m_aSourceFiles = null;
		
		/// <summary>Local class member bound to SourceFolder property</summary>
		private string m_strSourceFolder = "";
		
		/// <summary>The active line</summary>
		private string m_strLine = "";
		
		/// <summary>Local class member bound to Results property</summary>
		private CTmaxDatabaseResults m_tmaxResults = null;
		
		/// <summary>Local class member bound to XmlDeposition property</summary>
		private FTI.Shared.Xml.CXmlDeposition m_xmlDeposition = null;
		
		/// <summary>Local class member to store reference to active</summary>
		private FTI.Shared.Xml.CXmlScript m_xmlScript = null;
		
		/// <summary>Local class member to store reference to active XML binder</summary>
		private FTI.Shared.Xml.CXmlBinder m_xmlBinder = null;
		
		/// <summary>Local class member to load case codes from an XML file</summary>
		private FTI.Shared.Trialmax.CTmaxCodesManager m_xmlCodesManager = null;
		
		/// <summary>Local class member used as temporary holding collection during an import operation</summary>
		private CTmaxItems m_tmaxPending = new CTmaxItems();

		/// <summary>Local class member to keep track of original record when importing XML</summary>
		private CDxPrimary m_dxXmlOriginal = null;
		
		/// <summary>Local class member bound to Target property</summary>
		private CBaseRecord m_dxTarget = null;
		
		/// <summary>Local class member bound to InsertAt property</summary>
		private CBaseRecord m_dxInsertAt = null;
		
		/// <summary>Local class member bound to Deposition property</summary>
		private CDxPrimary m_dxDeposition = null;
		
		/// <summary>Local class member bound to Highlighter property</summary>
		private CDxHighlighter m_dxHighlighter = null;
		
		/// <summary>Local class member to keep track of the last designation read in from the file</summary>
		private CTmaxImportDesignation m_lastDesignation = null;
		
		/// <summary>Local flag to allow multiple file selections</summary>
		private bool m_bAllowMultiple = false;
		
		/// <summary>Local class member bound to InsertBefore property</summary>
		private bool m_bInsertBefore = false;
		
		/// <summary>Local class member bound to Cancelled property</summary>
		private bool m_bCancelled = false;
		
		/// <summary>Local class member bound to Merge property</summary>
		private bool m_bMergeSource = false;
		
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
		
		/// <summary>Prefix used to identify a column header as a property rather than a meta field</summary>
		private string m_strPropertyPrefix = "#";
		
		/// <summary>Private member for exporting codes (fielded data) to database</summary>
		private CTmaxCodesDatabase m_tmaxCodesDatabase = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxImportManager()
		{
			//	Initialize the event source and error builder
			SetErrorStrings();
			m_tmaxEventSource.Name = "Import Manager";
			
		}// public CTmaxImportManager()
		
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
				openFile = new System.Windows.Forms.OpenFileDialog();;
				openFile.CheckFileExists = true;
				openFile.CheckPathExists = true;
				openFile.Multiselect = m_bAllowMultiple;
				
				if(m_eFormat == TmaxImportFormats.XmlScript)
					openFile.Filter = "XML Scripts (*.xmls,*.xmlv)|*.xmls;*.xmlv|All Files (*.*)|*.*";
				else if(m_eFormat == TmaxImportFormats.XmlBinder)
					openFile.Filter = CXmlBinder.GetFilter(true);
				else if(m_eFormat == TmaxImportFormats.XmlCaseCodes)
					//Added .txt format in the option on 28June2012
                    openFile.Filter = "XML Files (*.xml)|*.xml|Text files (*.txt)|*.txt|All Files (*.*)|*.*";
				else if(m_eFormat == TmaxImportFormats.CodesDatabase)
					openFile.Filter = "Database Files (*.mdb)|*.mdb|All Files (*.*)|*.*";
				else
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
				
		}// public bool GetSourceFiles()
					
		/// <summary>This method uses the specified parameters to set the associated properties</summary>
		/// <param name="tmaxParameters">The collection of parameters</param>
		/// <param name="tmaxResults">The results of the operation are stored</param>
		public void Initialize(CTmaxParameters tmaxParameters, CTmaxDatabaseResults tmaxResults)
		{
			CTmaxParameter tmaxParameter = null;

			//	Reset the current values to their defaults
			Reset(true);
			
			try
			{
				m_tmaxResults = tmaxResults;
				
				if(tmaxParameters != null)
				{				
					//	Get the import format
					if((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.ImportFormat)) != null)
					{
						try	  { this.Format = (TmaxImportFormats)(tmaxParameter.AsInteger()); }
						catch {}
					}

					if((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.Before)) != null)
						this.InsertBefore = tmaxParameter.AsBoolean();
					
					if((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.MergeImported)) != null)
						this.MergeSource = tmaxParameter.AsBoolean();
				}
				else
				{
					this.Format = TmaxImportFormats.Unknown; // Disable the operation
				
				}// if(tmaxParameters != null)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Initialize", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_EX), Ex);
			}
			
			//	Set the format specific defaults
			switch(this.Format)
			{
				case TmaxImportFormats.BarcodeMap:
					
					m_strCommentCharacters = "";
					m_strDelimiters = "\t";					
					m_strDisplayType = "Barcode Map";
					m_bAllowMultiple = false;
					break;
						
				case TmaxImportFormats.XmlCaseCodes:
					
					m_strCommentCharacters = "";
					m_strDelimiters = "\t";					
					m_strDisplayType = "Data Fields";
					m_bAllowMultiple = false;
					break;
						
				case TmaxImportFormats.AsciiBinder:
					
					m_strCommentCharacters = "#";
					m_strDelimiters = "\t";					
					m_strDisplayType = "Binder";
					m_bAllowMultiple = true;
					break;
						
				case TmaxImportFormats.AsciiMedia:
					
					m_strCommentCharacters = "#";
					m_strDelimiters = "\t";					
					m_strDisplayType = "Script";
					m_bAllowMultiple = true;
					break;
						
				case TmaxImportFormats.XmlScript:
					
					m_strCommentCharacters = "!";
					m_strDelimiters = "\t";					
					m_strDisplayType = "XML Script";
					m_bAllowMultiple = true;
					break;
						
				case TmaxImportFormats.XmlBinder:
					
					m_strCommentCharacters = "!";
					m_strDelimiters = "\t";					
					m_strDisplayType = "XML Binder";
					m_bAllowMultiple = true;
					break;
						
				case TmaxImportFormats.Codes:
					
					m_strCommentCharacters = "";
					m_strDelimiters = "\t";					
					m_strDisplayType = "Codes";
					m_bAllowMultiple = true;
					break;
						
				case TmaxImportFormats.CodesDatabase:
					
					m_strCommentCharacters = "";
					m_strDelimiters = "\t";					
					m_strDisplayType = "Database";
					m_bAllowMultiple = false;
					break;
						
				case TmaxImportFormats.AsciiPickList:
					
					m_strCommentCharacters = "";
					m_strDelimiters = "\t";					
					m_strDisplayType = "Pick List";
					m_bAllowMultiple = true;	//	May change this in SetTarget()
					break;
						
				default:
					
					m_strCommentCharacters = "";
					m_strDelimiters = "\t";					
					m_strDisplayType = "";
					m_bAllowMultiple = false;
					break;
						
			}// switch(this.Format)
		
		}// public void Initialize(CTmaxParameters tmaxParameters)
		
		/// <summary>This method uses the caller's event item to set the target and insertion point</summary>
		/// <param name="tmaxItem">The event item used to set the target</param>
		public void SetTarget(CTmaxItem tmaxItem)
		{
			//	Set defaults if no target item
			if(tmaxItem == null)
			{
				this.Target = null;
				this.InsertAt = null;
				this.InsertBefore = false;
			}
			else
			{
				//	What is being imported?
				switch(this.Format)
				{
					case TmaxImportFormats.AsciiPickList:
					
						if(m_tmaxDatabase.PickListsEnabled)
						{
							if(tmaxItem.PickItem != null)
								this.Target = (CBaseRecord)(tmaxItem.PickItem.DxRecord);
							
							//	Should ALWAYS be a target parent for pick lists
							if(this.Target == null)
								this.Target = (CBaseRecord)(m_tmaxDatabase.PickLists.DxRecord);
						}
					
						if(this.Target != null)
						{
							//	Allow multiple if importing lists instead of values
							switch(((CDxPickItem)(this.Target)).Type)
							{
								case TmaxPickItemTypes.MultiLevel:
									m_bAllowMultiple = true;
									break;
								default:
									m_bAllowMultiple = false;
									break;
							}
					
						}
						else
						{
							this.Format = TmaxImportFormats.Unknown; // Disable the operation
						}
						break;
						
					case TmaxImportFormats.AsciiBinder:
					case TmaxImportFormats.XmlBinder:
					
						this.Target = (CBaseRecord)(tmaxItem.IBinderEntry);
						break;
						
					case TmaxImportFormats.XmlCaseCodes:
					case TmaxImportFormats.BarcodeMap:
					case TmaxImportFormats.CodesDatabase:
					
						//	No target required for these formats
						this.Target = null;
						this.InsertAt = null;
						this.InsertBefore = false;
						break;
						
					default:
					
						this.Target = (CBaseRecord)(tmaxItem.IPrimary);
						Debug.Assert(tmaxItem.MediaType == TmaxMediaTypes.Script, "MediaType != Script -> Import Format = " + this.Format.ToString());
						break;
						
				}// switch(this.Format)
					
				//	Did the caller specify a target?
				if(this.Target != null)
				{
					//	Did the caller specify an insertion point?
					if((tmaxItem.SubItems != null) && (tmaxItem.SubItems.Count > 0))
					{
						if((this.Format == TmaxImportFormats.AsciiBinder) || (this.Format == TmaxImportFormats.XmlBinder))
							this.InsertAt = (CBaseRecord)(tmaxItem.SubItems[0].IBinderEntry);
						else
							this.InsertAt = (CBaseRecord)(tmaxItem.SubItems[0].GetMediaRecord());
					}
				
				}// if(this.Target != null)
			
			}// if(tmaxItem == null)
			
		}// public void SetTarget(CTmaxItem tmaxItem)
		
		/// <summary>This method is called to execute the import operation</summary>
		/// <returns>true if successful</returns>
		public bool Import()
		{
			System.Threading.Thread importThread = null;
			int						iAttempts = 0;
			bool					bSuccessful = false;
			
			//	There should be source files available
			if((SourceFiles == null) || (SourceFiles.GetUpperBound(0) < 0))
				return false;
				
			//	Prompt the user for options if importing codes or scripts
			switch(this.Format)
			{
				case TmaxImportFormats.Codes:
				case TmaxImportFormats.CodesDatabase:
				
					if(GetCodesOptions() == false)
						return false;
					break;
					
				case TmaxImportFormats.XmlCaseCodes:
				
					if(GetXmlCaseCodesOptions() == false)
						return false;
					break;
					
				case TmaxImportFormats.AsciiMedia:
				
					if(GetScriptsOptions() == false)
						return false;
					break;
					
				case TmaxImportFormats.AsciiBinder:
				
					if(GetBindersOptions() == false)
						return false;
					break;
					
				case TmaxImportFormats.XmlScript:
				
					if(GetXmlScriptsOptions() == false)
						return false;
					break;
					
			}// switch(this.Format)
		
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
				m_tmaxEventSource.FireError(this, "Import", m_tmaxErrorBuilder.Message(ERROR_IMPORT_EX), Ex);
			}
			
			//	Did we import/update any records?
			return bSuccessful;
				
		}// public bool Import()
		
		/// <summary>This method is called to add a message to the status form list</summary>
		/// <param name="strMessage">The message to be added</param>
		/// <param name="eLevel">Enumerated message type to define error level</param>
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
				
				//	Should the results list be visible?
				if(this.Format == TmaxImportFormats.XmlScript)
				{
					m_wndStatusForm.ResultsMode = TmaxImportMessageModes.XmlScripts;
					m_wndStatusForm.ShowResults = true;
				}
				else
				{
					m_wndStatusForm.ResultsMode = TmaxImportMessageModes.AsciiMedia;
					m_wndStatusForm.ShowResults = false;
				}
					
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
                    WindowStatusRefresh();
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
				try	{ m_streamReader.Close(); }
				catch {}
				
				m_streamReader = null;
			}
			
			if(m_xmlScript != null)
			{
				m_xmlScript.Clear();
				m_xmlScript = null;
			}
			
			if(m_xmlBinder != null)
			{
				m_xmlBinder.Clear();
				m_xmlBinder = null;
			}
			
			if(m_xmlCodesManager != null)
			{
				m_xmlCodesManager.Clear();
				m_xmlCodesManager = null;
			}
			
			if(m_tmaxCodesDatabase != null)
			{
				m_tmaxCodesDatabase.Close();
				m_tmaxCodesDatabase = null;
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
			System.IO.FileStream	fsSource = null;
			bool					bSuccessful = true;
			
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
				//	What type of file is being imported?
				switch(this.Format)
				{
					case TmaxImportFormats.XmlScript:
					
						m_xmlScript = new CXmlScript();
						
						//	Open the script and fill its collections
						bSuccessful = m_xmlScript.FastFill(strFileSpec, true);
						break;
						
					case TmaxImportFormats.XmlBinder:
					
						m_xmlBinder = new CXmlBinder();
						
						//	Open the binder and fill its collections
						bSuccessful = m_xmlBinder.Load(strFileSpec, false);
						break;
						
					case TmaxImportFormats.CodesDatabase:
					
						bSuccessful = OpenCodesDatabase(strFileSpec);
						break;
						
					case TmaxImportFormats.XmlCaseCodes:
					
						m_xmlCodesManager = new CTmaxCodesManager();
						m_tmaxEventSource.Attach(m_xmlCodesManager.EventSource);
						
						//	Open the file and fill its collections
						bSuccessful = m_xmlCodesManager.Load(strFileSpec);
						break;
						
					case TmaxImportFormats.AsciiMedia:
					//Added case for importing fielded data definition
					//from text file format
                    case TmaxImportFormats.TextCaseCodes:
				    default:

						//	Open the file stream				
						fsSource = new FileStream(strFileSpec, FileMode.Open, FileAccess.Read);

						if(fsSource != null)
						{
							m_streamReader = new StreamReader(fsSource, System.Text.Encoding.Default);
						}
						else
						{
							bSuccessful = false;
						}
						break;
						
				}// switch(this.Format)
				
			}
			catch
			{
				bSuccessful = false;
			}
				
			//	Were we unable to open the file?
			if(bSuccessful == false)
				AddMessage(ERROR_FILE_OPEN_FAILED, strFileSpec, TmaxMessageLevels.FatalError);
			else
				FileSpec = strFileSpec;
				
			return bSuccessful;

		}// private bool OpenStream(string strFileSpec)
					
		/// <summary>This method will open the database for exporting fielded data (codes)</summary>
		/// <param name="strFileSpec">Fully qualified path to the desired file</param>
		/// <returns>true if successful</returns>
		private bool OpenCodesDatabase(string strFileSpec)
		{
			bool bSuccessful = false;
			
			//	Make sure the active database is closed
			CloseStream();

			try
			{
				while(bSuccessful == false)
				{
					//	Create the database
					m_tmaxCodesDatabase = new CTmaxCodesDatabase();
					m_tmaxEventSource.Attach(m_tmaxCodesDatabase.EventSource);
					m_tmaxCodesDatabase.CaseDatabase = m_tmaxDatabase;
					
					//	Open the database
					if(m_tmaxCodesDatabase.Open(strFileSpec) == false)
					{
						AddMessage(ERROR_OPEN_CODES_DATABASE_FAILED, strFileSpec, TmaxMessageLevels.FatalError);
						break;
					}
				
					//	Does this database contain concatenated codes?
					if((m_tmaxCodesDatabase.Info != null) && (m_tmaxCodesDatabase.Info.Concatenated == true))
					{
						AddMessage(ERROR_NO_CONCATENATED_CODES, TmaxMessageLevels.FatalError);
						break;
					}
					
					//	We're done
					bSuccessful = true;
					
				}// while(bSuccessful == false)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OpenCodesDatabase", Ex);
				AddMessage(ERROR_OPEN_CODES_DATABASE_EX, strFileSpec, TmaxMessageLevels.FatalError);
			}
			
			return bSuccessful;
				
		}// private bool OpenCodesDatabase(string strFileSpec)
					
		/// <summary>This method will reset the local members to their default values</summary>
		/// <param name="bTotal">True to do a reset of all members, False for new source file</param>
		private void Reset(bool bTotal)
		{
			//	Reset the source file specific members
			m_dxDeposition		= null;
			m_dxHighlighter		= null;
			m_lastDesignation	= null;
			m_dxXmlOriginal		= null;
			
			//	Close the import file
			CloseStream();
			
			//	Close the active deposition
			if(m_xmlDeposition != null)
			{
				m_xmlDeposition.Clear();
				m_xmlDeposition = null;
			}
			
			//	Reset all members if requested
			if(bTotal == true)
			{
				m_tmaxResults	= null;
				m_aSourceFiles  = null;
				m_dxTarget      = null;
				m_dxInsertAt    = null;
				m_bInsertBefore = false;
				m_bCancelled    = false;
				m_bMergeSource	= false;
				m_eFormat		= TmaxImportFormats.Unknown;
				m_iSourceIndex  = -1;
				
				//	Flush the pending collection
				m_tmaxPending.Clear();
				
				//	Destroy the status form
				if(m_wndStatusForm != null)
				{
					if(m_wndStatusForm.IsDisposed == false)
						m_wndStatusForm.Dispose();
					m_wndStatusForm = null;
				}
			
			}// if(bTotal == true)
			
			//	Never reset the source folder because we want to keep
			//	track of the folder used on the last job
			
		}// private void Reset()
		
		/// <summary>This method is called to read the next line from the file stream</summary>
		/// <returns>The next line of text</returns>
		private string ReadLine()
		{
			string strLine = null;
			
			try
			{
				if(m_streamReader != null)
				{
					strLine = m_streamReader.ReadLine();
				}
			}
			catch
			{
				strLine = null;
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
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(SourceFiles != null);
			Debug.Assert(SourceFiles.GetUpperBound(0) >= 0);
						
			//	Iterate the collection of source files to be imported
			for(m_iSourceIndex = 0; m_iSourceIndex <= SourceFiles.GetUpperBound(0); m_iSourceIndex++)
			{
				//	Has the user cancelled?
				if(this.Cancelled == true) break;
				//Added condition to change the this.Format if user has
				//selected .txt option	
                if (this.Format == TmaxImportFormats.XmlCaseCodes && Path.GetExtension(SourceFiles[m_iSourceIndex]) == ".txt")
                    this.Format = TmaxImportFormats.TextCaseCodes;
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
			if(this.StatusForm != null)
			{
				this.StatusForm.OnThreadFinished("");
			
			}// if(this.StatusForm != null)
		
		}// private void ImportThreadProc()
		
		/// <summary>This method is called to process the active file stream</summary>
		/// <returns>true if successful</returns>
		private bool ImportFile()
		{
			try
			{
				SetStatus("importing " + System.IO.Path.GetFileName(this.FileSpec));
				
				//	What are we importing?
				switch(this.Format)
				{
					case TmaxImportFormats.AsciiMedia:
					case TmaxImportFormats.AsciiBinder:
					
						return ImportMedia();
					
					case TmaxImportFormats.XmlScript:
					
						return ImportXmlScript();
					
					case TmaxImportFormats.XmlBinder:
					
						return ImportXmlBinder();
					
					case TmaxImportFormats.Codes:
					case TmaxImportFormats.CodesDatabase:
					
						return ImportCodes();
					
					case TmaxImportFormats.AsciiPickList:
					
						return ImportPickList();
					
					case TmaxImportFormats.XmlCaseCodes:
					
						return ImportXmlCaseCodes();
					//Added new case to import fielded data 
					//definition from text file format
                    case TmaxImportFormats.TextCaseCodes:

				        return ImportTextCaseCodes();
					
					case TmaxImportFormats.BarcodeMap:
					
						return ImportBarcodeMap();
					
					default:
					
						Debug.Assert(false, "Unable to import " + this.Format.ToString() + " streams");
						return false;
				
				}// switch(this.Format)
			
			}
			catch
			{
				AddMessage(ERROR_IMPORT_STREAM_EX, this.FileSpec, TmaxMessageLevels.CriticalError);
				return false;
			}

		}// private bool ImportFile()
		
		/// <summary>This method is called to import the active stream as a barcode map</summary>
		/// <returns>true if successful</returns>
		private bool ImportBarcodeMap()
		{
			string []		aFields = null;
			string			strForeign = "";
			string			strBarcode = "";
			CDxMediaRecord	dxRecord = null;
			bool			bSuccessful = true;

			//	Process all lines in the file
			while((aFields = Parse()) != null)
			{
				Debug.Assert(aFields.GetUpperBound(0) >= 0);
				if(aFields.GetUpperBound(0) < 0) continue;
				
				//	Get the barcode 
				strBarcode = CTmaxToolbox.StripQuotes(aFields[0], true);
				
				//	Get the foreign barcode
				if(aFields.GetUpperBound(0) > 0)
					strForeign = CTmaxToolbox.StripQuotes(aFields[1], true);
				else
					strForeign = "";	//	Clear the existing FBC

				//	Must have a valid barcode
				if(strBarcode.Length == 0) 
				{
					//	Add an error if attempting to assign a foreign barcode
					if(strForeign.Length > 0)
						AddMessage(ERROR_NO_BARCODE);
					
					continue;
					
				}// if(strBarcode.Length == 0) 
				
				//	Get the specified record
				if((dxRecord = m_tmaxDatabase.GetRecordFromBarcode(strBarcode, true, true)) != null)
				{
					//	Make sure we've loaded the latest value
					dxRecord.GetForeignBarcode();
					
					//	Assign the foreign barcode
					if(dxRecord.SetForeignBarcode(strForeign, true, false) == false)
					{
						bSuccessful = false;
					}
					else
					{
						if(m_tmaxResults != null)
							m_tmaxResults.Added.Add(new CTmaxItem(dxRecord));
					}
					
				}
				else
				{
					AddMessage(ERROR_INVALID_BARCODE, strBarcode);
					bSuccessful = false;
				}
				
			}// while((aFields = Parse()) != null)
			
			return bSuccessful;
		
		}// private bool ImportBarcodeMap()
		
		/// <summary>This method is called to import the active stream as media records</summary>
		/// <returns>true if successful</returns>
		private bool ImportMedia()
		{
			string []	aFields = null;
			bool		bSuccessful = true;

			//	Process all lines in the file
			while((aFields = Parse()) != null)
			{
				Debug.Assert(aFields.GetUpperBound(0) >= 0);
				if(aFields.GetUpperBound(0) < 0) continue;
				
				if(this.Cancelled == true) break;
				
				//	Process this line
				if(ImportMedia(aFields, m_tmaxPending) == false)
				{
					//	An error has occurred
					bSuccessful = false;
				}
				
			}// while((aFields = Parse()) != null)
			
			//	Do we have a designation waiting to be added to the database?
			if(m_lastDesignation != null)
            {
				ImportDesignation(m_lastDesignation, m_tmaxPending);
				m_lastDesignation = null;
				
			}// if(m_lastDesignation != null)
			
			//	Nothing to do if no records were imported
			if(m_tmaxPending.Count == 0) return bSuccessful;
			
			//	Should we transfer the pending records to their parent now?
			if((m_bMergeSource == false) || (m_iSourceIndex == m_aSourceFiles.GetUpperBound(0)))
			{
				if(SetPendingParent() == false)
					bSuccessful = false;
				
				//	Clear the pending records
				m_tmaxPending.Clear();
				
			}
				
			return bSuccessful;
		
		}// private bool ImportMedia()
		
		/// <summary>This method is called to import the active stream as a pick list</summary>
		/// <returns>true if successful</returns>
		private bool ImportPickList()
		{
			CDxPickItem				dxTarget = null;
			CTmaxPickItem			piTarget = null;
			CTmaxPickItem			piParent = null;
			CTmaxPickItem			piSource = null;
			CTmaxPickItems			piSourceItems = null;
			CTmaxItem				tmaxAdd = null;
			string []				aFields = null;
			bool					bSuccessful = false;
			bool					bDuplicate = false;

			//	There MUST be a target record and pick list item
			if((dxTarget = (CDxPickItem)(this.Target)) == null)
				return false;
			if((piTarget = dxTarget.TmaxPickItem) == null)
				return false;
				
			try
			{
				//	Are we importing a whole new list?
				if(piTarget.Type == TmaxPickItemTypes.MultiLevel)
				{
					//	Create a new parent application object
					piParent = new CTmaxPickItem();
					piParent.Type = TmaxPickItemTypes.StringList;
					piParent.ParentId = piTarget.UniqueId;
					piParent.Name = System.IO.Path.GetFileNameWithoutExtension(this.FileSpec);
				}
				else
				{
					//	The target IS the parent
					piParent = piTarget;
					
					//	This should already be set
					if(piTarget.DxRecord == null)
						piTarget.DxRecord = dxTarget;
				}
				
				//	Allocate a temporary collection for the new pick items
				piSourceItems = new CTmaxPickItems();
				
				//	Process all lines in the file
				while((aFields = Parse()) != null)
				{
					Debug.Assert(aFields.GetUpperBound(0) >= 0);
					if(aFields.GetUpperBound(0) < 0) continue;
					if(aFields[0].Length == 0) continue;
					
					if(this.Cancelled == true) break;
					
					//	Make sure we don't already have a value of the same name
					if(piSourceItems.Find(aFields[0], !piParent.CaseSensitive) != null)
						bDuplicate = true;
					else if(piParent.FindChild(aFields[0]) != null)
						bDuplicate = true;
						
					//	Did we find a duplicate value?
					if(bDuplicate == true)
					{
						AddMessage(ERROR_DUPLICATE_PICK_ITEM, aFields[0]);
						bDuplicate = false;
						continue;
					}
					
					//	Create a new pick item and add to the collection
					piSource = new CTmaxPickItem();
					piSource.Name = aFields[0];
					piSource.ParentId = piParent.UniqueId;
					piSource.Type = TmaxPickItemTypes.Value;
					piSourceItems.Add(piSource);
					
					//	Now add an event item to the pending collection
					m_tmaxPending.Add(new CTmaxItem(piSource));
					
				}// while((aFields = Parse()) != null)
				
				//	Nothing to do if no records were imported
				if(m_tmaxPending.Count == 0) return bSuccessful;
				
				//	Do we need to add the parent record first?
				if(piParent.DxRecord == null)
				{
					//	Create an event item to add the new parent list to the target list
					tmaxAdd = new CTmaxItem(piTarget);
					tmaxAdd.SourceItems.Add(new CTmaxItem(piParent));
					
					//	Add the new list
					if(m_tmaxDatabase.Add(tmaxAdd, null, m_tmaxResults) == true)
					{
						//	The new record becomes the new parent
						if(tmaxAdd.ReturnItem == null) return false;
						if(tmaxAdd.ReturnItem.SourceItems == null) return false;
						if(tmaxAdd.ReturnItem.SourceItems.Count == 0) return false;
						if(tmaxAdd.ReturnItem.SourceItems[0].PickItem == null) return false;
						
						//	Reset the parent
						piParent = tmaxAdd.ReturnItem.SourceItems[0].PickItem;

					}
					else
					{
						//	Can't add the parent list
						return false;
					}
					
				}// if(piParent.DxRecord == null)
					
				//	Now add the pending items to the parent
				if(tmaxAdd == null)
					tmaxAdd = new CTmaxItem(piParent);
				else
					tmaxAdd.Initialize(piParent);
				tmaxAdd.SourceItems = m_tmaxPending;

				bSuccessful = m_tmaxDatabase.Add(tmaxAdd, null, m_tmaxResults);
				
				//	Clear the pending records
				m_tmaxPending.Clear();
				piSourceItems.Clear(false);
			
			}
			catch
			{
				AddMessage(ERROR_IMPORT_PICK_LIST_EX, TmaxMessageLevels.FatalError);
			}
				
			return bSuccessful;
		
		}// private bool ImportPickList()
		
		/// <summary>This method is called to import the case codes and pick lists stored in an XML file</summary>
		/// <returns>true if successful</returns>
		private bool ImportXmlCaseCodes()
		{
			bool bSuccessful = false;
			
			//	There should be a codes manager that's been populated
			if(m_xmlCodesManager == null) return false;
			if(m_xmlCodesManager.CaseCodes == null) return false;
				
			try
			{
				//	Notify the database to rebuild the tables
				if(m_tmaxDatabase.SetCaseCodes(m_xmlCodesManager) == true)
				{
					//	Force a refresh so that the application gets notified
					m_tmaxDatabase.FireCommand(TmaxCommands.RefreshCodes);
					
					bSuccessful = true;
				}
				
			
			}
			catch
			{
				AddMessage(ERROR_IMPORT_XML_CASE_CODES_EX, TmaxMessageLevels.FatalError);
			}
				
			return bSuccessful;
		
		}// private bool ImportXmlCaseCodes()


        /// <summary>This method is called to import the case codes stored in a text file</summary>
        /// <returns>true if successful</returns>
        private bool ImportTextCaseCodes()
        {
            bool bSuccessful = false;

            try
            {
                CTextIni cTextIni = new CTextIni();
                cTextIni.fileName = m_strFileSpec;
                
                CTmaxCaseCodes cTmaxCaseCodes = cTextIni.GetCaseCodes();
                if(m_tmaxDatabase.SetCaseCodesForTextFile())
                {
                    m_tmaxDatabase.AddCaseCodes(cTmaxCaseCodes, null, false, true, null);

                    //	Force a refresh so that the application gets notified
                    m_tmaxDatabase.FireCommand(TmaxCommands.RefreshCodes);
                }


            }
            catch(Exception e)
            {
                AddMessage(ERROR_IMPORT_XML_CASE_CODES_EX, TmaxMessageLevels.FatalError);
            }

            return bSuccessful;

        }// private bool ImportTextCaseCodes()
		
		/// <summary>This method is called to import the active XML script</summary>
		/// <returns>true if successful</returns>
		private bool ImportXmlScript()
		{
			bool bSuccessful = true;
			
			Debug.Assert(m_xmlScript != null);
			if(m_xmlScript == null) return false;
			
			//	Do we need to populate the scenes collection?
			if((m_xmlScript.XmlScenes == null) || (m_xmlScript.XmlScenes.Count == 0))
			{
				//	This may be a video viewer XML file which doesn't have scenes
				if((m_xmlScript.XmlDesignations != null) && (m_xmlScript.XmlDesignations.Count > 0))
				{
					//	Create the scenes for the designations
					if(m_xmlScript.CreateXmlScenes() == false)
					{
						AddMessage(ERROR_CREATE_XML_SCENES_FAILED);
						return false;
					}
					else
					{
						//	Should we merge the designations?
						if(m_tmaxImportOptions.MergeDesignations != TmaxDesignationMergeMethods.None)
							m_xmlScript.ApplyMergeMethod(m_tmaxImportOptions);
					}
					
				}
				else
				{
					//	No scenes to be processed
					AddMessage(ERROR_NO_XML_SCENES);
					return false;
				}
				
			}// if((m_xmlScript.XmlScenes == null) || (m_xmlScript.XmlScenes.Count == 0))
            else
            {
                //	Should we merge the designations?
                if (m_tmaxImportOptions.MergeDesignations != TmaxDesignationMergeMethods.None)
                    m_xmlScript.ApplyMergeMethod(m_tmaxImportOptions);
            }

			//	Process all scenes in the active XML script
			foreach(CXmlScene O in m_xmlScript.XmlScenes)
			{
				if(this.Cancelled == true) break;
				
				if(ImportXmlScene(O) == false)
					bSuccessful = false;
			
			}// foreach(CXmlScene O in m_xmlScript.XmlScenes)
			
			//	Import the objections stored in the script
			//
			//	NOTE:	The XML scenes MUST be added first to ensure that the
			//			depositions have been added to the database
			ImportXmlObjections(m_xmlScript);
			
			//	Nothing to do if no records were imported
			if(m_tmaxPending.Count == 0) return bSuccessful;
			
			//	Should we transfer the pending records to their parent now?
			if((m_bMergeSource == false) || (m_iSourceIndex == m_aSourceFiles.GetUpperBound(0)))
			{
				if(SetPendingParent() == false)
					bSuccessful = false;
				
				//	Clear the pending records
				m_tmaxPending.Clear();
			}
				
			return bSuccessful;
		
		}// private bool ImportXmlScript()

		/// <summary>This method is called to import the objections contained in the XML script</summary>
		/// <param name="xmlScript">The XML script containing the objections</param>
		/// <returns>true if successful</returns>
		private bool ImportXmlObjections(CXmlScript xmlScript)
		{
			bool			bSuccessful = true;
			COImportManager objectionsManager = null;

			Debug.Assert(xmlScript != null);
			if(xmlScript == null) return false;

			try
			{
				//	Preprocessing checks
				if(xmlScript.XmlDepositions == null) return true; // Nothing to add
				if(xmlScript.XmlDepositions.Count == 0) return true;
				if(ImportOptions.ObjectionsMethod == TmaxImportObjectionMethods.IgnoreAll) return true;
				
				//	Do we have an active objections database?
				if(m_tmaxDatabase.ObjectionsEnabled == true)
				{
					//	Allocate and initialize the manager
					objectionsManager = new COImportManager();		
					m_tmaxEventSource.Attach(objectionsManager.EventSource);
					
					objectionsManager.CaseDatabase = this.Database;
					objectionsManager.ObjectionsDatabase = this.Database.ObjectionsDatabase;
					objectionsManager.Options = this.ImportOptions;
					objectionsManager.StatusForm = this.StatusForm;
					objectionsManager.Results = m_tmaxResults;
					
					//	Import objections stored in the specified script
					bSuccessful = objectionsManager.ImportXmlScript(m_xmlScript);
				}
				else
				{
					AddMessage(ERROR_NO_OBJECTIONS_DATABASE);
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "ImportXmlObjections", m_tmaxErrorBuilder.Message(ERROR_IMPORT_XML_OBJECTIONS_EX, xmlScript.FileSpec), Ex);
			}
	
			return bSuccessful;

		}// private bool ImportXmlObjections(CXmlScript xmlScript)

		/// <summary>This method is called to import the active XML script</summary>
		/// <returns>true if successful</returns>
		private bool ImportXmlBinder()
		{
			CTmaxItem		tmaxParent = null;
			CTmaxItem		tmaxBinder = null;
			CTmaxItem		tmaxInsert = null;
			CTmaxParameters	tmaxParameters = null;
			bool			bSuccessful = true;
			
			Debug.Assert(m_xmlBinder != null);
			if(m_xmlBinder == null) return false;
			
			//	Make sure there were binder descriptors contained in the XML
			if((m_xmlBinder.Binders == null) || (m_xmlBinder.Binders.Count == 0))
			{
				AddMessage(ERROR_NO_XML_BINDERS);
				return false;
			}
			
			//	Create an event item to represent the parent binder
			tmaxParent = new CTmaxItem();
			tmaxParent.DataType = TmaxDataTypes.Binder;
			tmaxParent.IBinderEntry = (CDxBinderEntry)(this.Target);
			if(tmaxParent.SourceItems == null)
				tmaxParent.SourceItems = new CTmaxItems();
				
			//	Add source items for each binder in the XML file
			foreach(CTmaxBinderItem O in m_xmlBinder.Binders)
			{
				if((tmaxBinder = GetCmdItem(O)) != null)
					tmaxParent.SourceItems.Add(tmaxBinder);	
			}
			
			if(this.InsertAt != null)
			{
				tmaxInsert = new CTmaxItem();
				tmaxInsert.IBinderEntry = ((CDxMediaRecord)(this.InsertAt));
				tmaxParent.SubItems.Add(tmaxInsert);
				
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.Before, this.InsertBefore);
				
			}
			
			//	Add the records to the database
			bSuccessful = m_tmaxDatabase.Add(tmaxParent, tmaxParameters, m_tmaxResults);

			return bSuccessful;
		
		}// private bool ImportXmlBinder()
		
		/// <summary>This method is called to import the specified XML scene</summary>
		/// <param name="xmlScene">The scene to be imported</param>
		/// <returns>true if successful</returns>
		private bool ImportXmlScene(CXmlScene xmlScene)
		{
			CDxPrimary		dxPrimary = null;
			CDxSecondary	dxSegment = null;
			CDxMediaRecord	dxSource = null;
			CXmlDesignation	xmlDesignation = null;
			CTmaxItem		tmaxPending = null;
			bool			bSuccessful = false;
			bool			bFilled = false;
			int				iScene = 0;
			
			Debug.Assert(m_xmlScript != null);
			Debug.Assert(xmlScene != null);
			if(m_xmlScript == null) return false;
			if(xmlScene == null) return false;
			
			try
			{
				//	Get the scene number in case we have to add an error message
				iScene = (m_xmlScript.XmlScenes.IndexOf(xmlScene) + 1);
				
				//	What type of record are we importing?
				switch(xmlScene.SourceType)
				{
					case TmaxMediaTypes.Designation:
					case TmaxMediaTypes.Clip:
					
						//	Get the source designation from the script
						if((xmlDesignation = m_xmlScript.GetSourceDesignation(xmlScene)) == null)
						{
							AddMessage(ERROR_XML_DESIGNATION_NOT_FOUND, iScene, xmlScene.SourceId);
							break;
						}

						//	Get the primary parent of the source record
						if((dxPrimary = GetXmlPrimary(xmlScene, xmlDesignation)) == null)
						{
							AddMessage(ERROR_XML_PRIMARY_NOT_FOUND, iScene, xmlDesignation.PrimaryId);
							break;
						}
							
						//	Make sure the secondary collection has been populated
						if((dxPrimary.Secondaries == null) || (dxPrimary.Secondaries.Count == 0))
						{
							dxPrimary.Fill();
							bFilled = true;
						}
						
						//	Get the secondary parent of the source record
						if((dxSegment = GetXmlSecondary(dxPrimary, xmlDesignation)) == null)
						{
							AddMessage(ERROR_XML_SECONDARY_NOT_FOUND, iScene, xmlDesignation.Segment);
							break;
						}
						
						//	Make sure the designation has the correct name
						//
						//	We have to do this because it may have been merged and the
						//	source deposition may not be in the XML script
						if(dxPrimary.MediaType == TmaxMediaTypes.Deposition)
							xmlDesignation.SetNameFromExtents(dxPrimary.Name);
			
						//	Add the designation to the database
						if((dxSource = m_tmaxDatabase.AddDesignation(dxSegment, xmlDesignation)) != null)
						{
							//	Add any links that appear in the designation
							if((xmlDesignation.Links != null) && (xmlDesignation.Links.Count > 0))
							{
								foreach(CXmlLink O in xmlDesignation.Links)
								{
									//	Force the database to use MediaId
									O.SourceDbId = "";
									
									m_tmaxDatabase.AddLink((CDxTertiary)dxSource, xmlDesignation, O);
									
								}// foreach(CXmlLink O in xmlDesignation.Links)
							
							}// if((xmlDesignation.Links != null) && (xmlDesignation.Links.Count > 0))
							
						}// if((dxSource = m_tmaxDatabase.AddDesignation(dxSegment, xmlDesignation)) != null)
						
						break;
						
					default:
					
						//	Must have a valid source identifier
						if(xmlScene.SourceId.Length == 0)
							AddMessage(ERROR_XML_NO_SOURCE_ID, iScene);
						else if((dxSource = m_tmaxDatabase.GetRecordFromBarcode(xmlScene.SourceId, true, true)) == null)
							AddMessage(ERROR_XML_SOURCE_NOT_FOUND, iScene, xmlScene.SourceId);
						break;
						
				}// switch(xmlScene.SourceType)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "ImportXmlScene", Ex);
				AddMessage(ERROR_IMPORT_XML_SCENE_EX, (m_xmlScript.XmlScenes.IndexOf(xmlScene) + 1), TmaxMessageLevels.FatalError);
			}
			
			//	Did we get the source record?
			if(dxSource != null)
			{
				//	Add to the pending collection
				tmaxPending = new CTmaxItem(dxSource);
				tmaxPending.XmlScene = xmlScene;
				m_tmaxPending.Add(tmaxPending);
				
				bSuccessful = true;				
			
			}// if(dxSource != null)
			
			//	Clean up if necessary
			if((dxPrimary != null) && (bFilled == true))
				dxPrimary.Secondaries.Clear();
				
			return bSuccessful;
		
		}// private bool ImportXmlScene(CXmlScene xmlScene)
		
		/// <summary>This method is called to get the primary parent of the XML scene's source record</summary>
		/// <param name="xmlScene">The scene that references the source record</param>
		/// <param name="xmlDesignation">The scene's source designation</param>
		/// <returns>The exchange interface to the source's primary parent</returns>
		private CDxPrimary GetXmlPrimary(CXmlScene xmlScene, CXmlDesignation xmlDesignation)
		{
			CDxPrimary		dxPrimary = null;
			CXmlDeposition	xmlDeposition = null;
			
			Debug.Assert(m_xmlScript != null);
			Debug.Assert(xmlScene != null);
			if(m_xmlScript == null) return null;
			if(xmlScene == null) return null;
			
			try
			{
				//	Is this a video designation
				if(xmlScene.SourceType == TmaxMediaTypes.Designation)
				{
					//	Check to see if the source deposition is already registered
					if(xmlDesignation.PrimaryId.Length > 0)
						dxPrimary = m_tmaxDatabase.Primaries.Find(xmlDesignation.PrimaryId);
						
					//	Do we need to get the XML source?
					if(dxPrimary == null)
					{
						if((xmlDeposition = m_xmlScript.GetSourceDeposition(xmlDesignation)) != null)
						{
							dxPrimary = AddXmlDeposition(xmlDeposition);
						}
						
					}// if(dxPrimary == null)
					
				}
				else if(xmlScene.SourceType == TmaxMediaTypes.Clip)
				{
					//	Must have a media id
					if(xmlDesignation.PrimaryId.Length > 0)
						dxPrimary = m_tmaxDatabase.Primaries.Find(xmlDesignation.PrimaryId);
				}
			
			}
			catch
			{
				AddMessage(ERROR_GET_XML_PRIMARY_EX, (m_xmlScript.XmlScenes.IndexOf(xmlScene) + 1));
			}
			
			return dxPrimary;
		
		}// private CDxPrimary GetXmlPrimary(CXmlScene xmlScene)
		
		/// <summary>This method is called to get the secondary parent of the XML scene's source record</summary>
		/// <param name="dxPrimary">The parent primary record</param>
		/// <param name="xmlDesignation">The designation that references the source record</param>
		/// <returns>The exchange interface to the source's secondary parent</returns>
		private CDxSecondary GetXmlSecondary(CDxPrimary dxPrimary, CXmlDesignation xmlDesignation)
		{
			CDxSecondary	dxSecondary = null;
			long			lBarcode = 0;
			
			Debug.Assert(m_xmlScript != null);
			Debug.Assert(dxPrimary != null);
			if(m_xmlScript == null) return null;
			if(dxPrimary == null) return null;
			
			//	NOTE:	We assume the Secondaries collection has been
			//			populated when this method gets called
			
			try
			{
				switch(dxPrimary.MediaType)
				{
					case TmaxMediaTypes.Deposition:
					
						//	Locate using the XML segment identifier
						dxSecondary = dxPrimary.Secondaries.Find(xmlDesignation.Segment);
						break;
						
					case TmaxMediaTypes.Recording:
					
						//	Locate using barcode identifier
						try { lBarcode = System.Convert.ToInt64(xmlDesignation.Segment); }
						catch { lBarcode = 0; }
						
						if(lBarcode > 0)
							dxSecondary = dxPrimary.Secondaries.Find(lBarcode, true);
						break;
						
				}
				
			}
			catch
			{
				AddMessage(ERROR_GET_XML_SECONDARY_EX, xmlDesignation.Name);
			}
			
			return dxSecondary;
		
		}// private CDxSecondary GetXmlSecondary(CDxPrimary dxPrimary, CXmlDesignation xmlDesignation)
		
		/// <summary>This method is called to add a new value to the specified parent pick list</summary>
		/// <param name="tmaxPickList">The pick list that will contain the new value</param>
		/// <param name="strValue">The value to be added</param>
		/// <returns>The new pick item</returns>
		private CTmaxPickItem AddPickItem(CTmaxPickItem tmaxPickList, string strValue)
		{
			CTmaxPickItem	tmaxValue = null;
			CTmaxItem		tmaxParent = null;
			
			try
			{
				//	Create an event item to represent the parent list
				tmaxParent = new CTmaxItem(tmaxPickList);
				
				//	Create a pick item to represent this new value
				tmaxValue = new CTmaxPickItem();
				tmaxValue.Type = TmaxPickItemTypes.Value;
				tmaxValue.Name = strValue;
				tmaxValue.Parent = tmaxPickList;
				tmaxValue.SortOrder = tmaxPickList.Children.GetNextSortOrder();
				
				//	Add to the event item's source collection
				tmaxParent.SourceItems.Add(new CTmaxItem(tmaxValue));

				//	Add to the database
				if(m_tmaxDatabase.Add(tmaxParent, null, m_tmaxResults) == false)
				{
					AddMessage(ERROR_ADD_PICK_ITEM_FAILED, tmaxPickList.Name, strValue);
					tmaxValue = null;
				}
						
			}
			catch
			{
				AddMessage(ERROR_ADD_PICK_ITEM_EX, tmaxPickList.Name, strValue);
				tmaxValue = null;
			}
			
			return tmaxValue;
					
		}// private CTmaxPickItem AddPickItem(CTmaxPickItem tmaxPickList, string strValue)
		
		/// <summary>This method is called to add the XML deposition to the database</summary>
		/// <param name="xmlDeposition">The deposition to be added</param>
		/// <returns>The exchange interface to the deposition record</returns>
		private CDxPrimary AddXmlDeposition(CXmlDeposition xmlDeposition)
		{
			CDxPrimary			dxPrimary = null;
			string				strMediaId = "";
			string				strFileSpec = "";
			CTmaxSourceFolder	tmaxFolder = null;
			CTmaxItem			tmaxAdded = null;
			
			try
			{
				//	Assign a default media id if necessary
				if(xmlDeposition.MediaId.Length == 0)
				{
					if(m_xmlScript.SourceFileSpec.Length > 0)
						strMediaId = System.IO.Path.GetFileNameWithoutExtension(m_xmlScript.SourceFileSpec);
					else
						strMediaId = xmlDeposition.Name;
				}
				else
				{	
					strMediaId = xmlDeposition.MediaId;
				}
							
				//	Do we need to add this deposition?
				if((dxPrimary = m_tmaxDatabase.Primaries.Find(strMediaId)) == null)
				{
					//	Get the system's temporary folder
					tmaxFolder = new CTmaxSourceFolder(System.IO.Path.GetTempPath());
					tmaxFolder.MediaType = TmaxMediaTypes.Deposition;
					tmaxFolder.SourceType = RegSourceTypes.Deposition;
					
					//	Build a path to store the XML deposition for registration
					strFileSpec = tmaxFolder.Path;
					if(strFileSpec.EndsWith("\\") == false)
						strFileSpec += "\\";
					strFileSpec += strMediaId;
					strFileSpec += ("." + CXmlDeposition.GetExtension());
					
					//	Add a file to the source folder
					tmaxFolder.Files.Add(new CTmaxSourceFile(strFileSpec));
					
					//	Delete the file if it already exists
					if(System.IO.File.Exists(strFileSpec) == true)
					{
						try { System.IO.File.Delete(strFileSpec); }
						catch {}
					}
					
					//	Save the deposition to the desired location
					if(xmlDeposition.Save(strFileSpec) == true)
					{
						//	Add to the database
						if(m_tmaxDatabase.AddSource(tmaxFolder) == true)
						{
							if((dxPrimary = (CDxPrimary)(tmaxFolder.IPrimary)) != null)
							{
								if(m_tmaxResults != null)
								{
									tmaxAdded = new CTmaxItem();
									tmaxAdded.MediaType = TmaxMediaTypes.Deposition;
									tmaxAdded.DataType = TmaxDataTypes.Media;
									tmaxAdded.SubItems.Add(new CTmaxItem(dxPrimary));
									tmaxAdded.SubItems[0].ParentItem = tmaxAdded;
									
									m_tmaxResults.Added.Add(tmaxAdded);
								}
								
								//	Add an entry to the status form
								AddStatusResult(dxPrimary, TmaxImportResults.Added);
								
							}// if((dxPrimary = (CDxPrimary)(tmaxFolder.IPrimary)) != null)
							
						}// if(m_tmaxDatabase.AddSource(tmaxFolder) == true)
							
						//	Clean up
						try { System.IO.File.Delete(strFileSpec); }
						catch {}
					}
					else
					{
						AddMessage(ERROR_XML_SAVE_DEPOSITION_FAILED, strFileSpec);
					
					}// if(xmlDeposition.Save(strFileSpec) == true)
				
				}// if((dxPrimary = m_tmaxDatabase.Primaries.Find(strMediaId)) == null)
						
			}
			catch
			{
				AddMessage(ERROR_ADD_XML_DEPOSITION_EX, strMediaId);
			}
			
			return dxPrimary;
		
		}// private CDxPrimary AddXmlDeposition(CXmlDeposition xmlDeposition)
		
		/// <summary>This method is called to import coded (fielded) data</summary>
		/// <returns>true if successful</returns>
		private bool ImportCodes()
		{
			bool bSuccessful = false;
			
			Debug.Assert(m_tmaxDatabase != null);
			if(m_tmaxDatabase == null) return false;
			if(m_tmaxDatabase.CodesEnabled == false) return false;
			if(m_tmaxDatabase.CaseCodes == null) return false;
			if(m_tmaxDatabase.CaseCodes.Count == 0) return false;
			
			//	Make sure the case codes are properly initialized for the operation
			foreach(CTmaxCaseCode O in m_tmaxDatabase.CaseCodes)
			{
				if(O.Imported != null)
					O.Imported.Clear();
				else
					O.Imported = new ArrayList();
					
				//	Should this code have a pick list?
				if((O.PickList == null) && (O.PickListId > 0))
				{
					if(m_tmaxDatabase.PickLists != null)
						O.PickList = m_tmaxDatabase.PickLists.FindList(O.PickListId);
				}
				
			}// foreach(CTmaxCaseCode O in m_tmaxDatabase.CaseCodes)
		
			//	Are we importing from text?
			if(this.Format == TmaxImportFormats.CodesDatabase)
				bSuccessful = ImportCodesFromDatabase();
			else
				bSuccessful = ImportCodesFromText();
			
			//	Clean up
			foreach(CTmaxCaseCode O in m_tmaxDatabase.CaseCodes)
			{
				if(O.Imported != null)
				{
					O.Imported.Clear();
					O.Imported = null;
				}
				
			}// foreach(CTmaxCaseCode O in m_tmaxDatabase.CaseCodes)
			
			return bSuccessful;
		
		}// private bool ImportCodes()
		
		/// <summary>This method is called to import the active text stream as coded (fielded) data</summary>
		/// <returns>true if successful</returns>
		private bool ImportCodesFromText()
		{
			string[]			aFields = null;
			CTmaxCaseCode[]		aCaseCodes = null;
			int					iBarcodeIndex = -1;
			bool				bExpression = false;
			bool				bSuccessful = true;

			Debug.Assert(m_tmaxDatabase != null);
			if(m_tmaxDatabase == null) return false;
			
			//	Set the local members for the operation
			if(m_tmaxImportOptions.Delimiter == TmaxImportDelimiters.Expression)
			{
				//	We have to have a valid regular expression
				if(m_tmaxImportOptions.Expression.Length == 0)
				{
					AddMessage(ERROR_CODES_EXPRESSION_EMPTY, TmaxMessageLevels.FatalError);
					return false;
				}
				
				try
				{
					System.Text.RegularExpressions.Regex regEx = new Regex(m_tmaxImportOptions.Expression);	
				}
				catch
				{
					AddMessage(ERROR_CODES_EXPRESSION_INVALID, m_tmaxImportOptions.Expression, TmaxMessageLevels.FatalError);
					return false;
				}
				
				m_strDelimiters = m_tmaxImportOptions.Expression;
				bExpression = true;
			
			}
			else
			{
				//	We use expressions for some of the predefined delimiters
				switch(m_tmaxImportOptions.Delimiter)
				{
					case TmaxImportDelimiters.Comma:
					case TmaxImportDelimiters.Pipe:
					
						m_strDelimiters = m_tmaxImportOptions.GetExpression(m_tmaxImportOptions.Delimiter);
						bExpression = true;
						break;
						
					case TmaxImportDelimiters.Tab:
					default:
					
						m_strDelimiters = m_tmaxImportOptions.GetDelimiter();
						bExpression = false;
						break;
						
				}// switch(m_tmaxImportOptions.Delimiter)
			}
			m_strCommentCharacters = m_tmaxImportOptions.CommentCharacters;
			
			//	The first line in the file should be the column headers
			if((aFields = Parse(bExpression)) == null) return false;
			
			//	Convert the headers to case codes
			if((aCaseCodes = GetCaseCodes(aFields)) == null) return false;
			
			//	Get the index of the column containing the barcode
			if((iBarcodeIndex = GetBarcodeIndex(aCaseCodes)) < 0)
				return false;
			
			//	Process the remaining lines in the file
			while((aFields = Parse(bExpression, false)) != null)
			{
				if(this.Cancelled == true) break;
				
				//	Do we have a sufficient number of fields?
				if(aFields.GetUpperBound(0) < aCaseCodes.GetUpperBound(0))
				{
					AddMessage(ERROR_LOW_FIELD_COUNT, aFields.GetUpperBound(0) + 1);
					bSuccessful = false;
					continue;
				}
				
				//	Set the codes using these fields
				if(SetCodes(aCaseCodes, aFields, iBarcodeIndex) == false)
					bSuccessful = false;
				
			}// while((aFields = Parse()) != null)
			
			return bSuccessful;
		
		}// private bool ImportCodesFromText()
		
		/// <summary>This method is called to import fielded data stored in an exported TrialMax fielded data (codes) database</summary>
		/// <returns>true if successful</returns>
		private bool ImportCodesFromDatabase()
		{
			string[]		aColumns = null;
			string[]		aValues = null;
			CTmaxCaseCode[]	aCaseCodes = null;
			int				iBarcodeIndex = -1;
			int				iRows = 0;
			bool			bSuccessful = true;

			Debug.Assert(m_tmaxDatabase != null);
			if(m_tmaxDatabase == null) return false;
			
			Debug.Assert(m_tmaxCodesDatabase != null);
			if(m_tmaxCodesDatabase == null) return false;
			
			//	Get the list of columns from the database
			if((aColumns = m_tmaxCodesDatabase.GetColumns()) == null)
				return false;
						
			//	Get the collection of case codes that match these columns
			if((aCaseCodes = this.GetCaseCodes(aColumns)) == null)
				return false;

			//	Get the index of the column containing the barcode
			if((iBarcodeIndex = GetBarcodeIndex(aCaseCodes)) < 0)
				return false;
			
			//	Get the total number of rows in the database
			if((iRows = m_tmaxCodesDatabase.GetRowCount()) == 0)
			{
				AddMessage(ERROR_CODES_DATABASE_EMPTY);
				return false;
			}
						
			//	Make sure the user didn't cancel before we blow away the data
			if(this.Cancelled == false)
				m_tmaxDatabase.FlushCodes();
			
			//	Read the values for each row
			for(int i = 0; i < iRows; i++)
			{
				if(this.Cancelled == true) break;
				
				if((aValues = m_tmaxCodesDatabase.GetValues(i)) != null)
				{
					//	Set the codes using these values
					if(SetCodes(aCaseCodes, aValues, iBarcodeIndex) == false)
						bSuccessful = false;
				
				}// if((aValues = m_tmaxCodesDatabase.GetValues(i)) != null)
				
			}// for(int i = 0; i < iRows; i++)
			
			return bSuccessful;
		
		}// private bool ImportCodesFromDatabase()
		
		/// <summary>This method sets the codes for the owner record specified by the caller</summary>
		/// <param name="dxOwner">The record that owns the codes</param>
		/// <param name="tmaxCaseCodes">The collection of code descriptors</param>
		/// <param name="aValues">The text fields parsed from the import file stream</param>
		/// <param name="iBarcodeIndex">The index of the code containing the barcode identifier</param>
		/// <returns>true if no errors occur</returns>
		private bool SetCodes(CDxMediaRecord dxOwner, CTmaxCaseCode[] tmaxCaseCodes, string[] aValues, int iBarcodeIndex)
		{
			string	strClean = "";
			bool	bSuccessful = true;
			bool	bModified = false;
			
			for(int i = 0; i <= tmaxCaseCodes.GetUpperBound(0); i++)
			{
				//	This could happen if there were empty fields at the end of the line
				//	The call to Parse() would have stripped them from the collection
				if(i > aValues.GetUpperBound(0))
					break; // No more values to process
					
				//	Do we have a case code descriptor?
				if(tmaxCaseCodes[i] == null) continue;
				
				//	Is this the barcode 
				if(i == iBarcodeIndex) continue;
				
				//	Clean the text assigned to this field if not retrieved from a database
				if(this.Format == TmaxImportFormats.Codes)
					strClean = CleanField(tmaxCaseCodes[i], aValues[i]);
				else
					strClean = aValues[i];
				
				//	Did the user provide a value for this column?
				if(strClean.Length == 0) continue;

				//	Add the code for this field
				if(SetCode(dxOwner, (CTmaxCaseCode)(tmaxCaseCodes[i]), strClean) == false)
					bSuccessful = false;
				else
					bModified = true;
								
			}// for(int i = 0; i < tmaxCaseCodes.GetUpperBound(0); i++)

			if(bModified == true)
			{
				//	Update the record
				if(dxOwner.Collection != null)
					dxOwner.Collection.Update(dxOwner);
			}
			
			return bSuccessful;
			
		}// private bool SetCodes(CDxMediaRecord dxOwner, CTmaxCaseCode[] tmaxCaseCodes, string[] aFields)
		
		/// <summary>This method sets the codes for the owner record identified by the barcode value</summary>
		/// <param name="tmaxCaseCodes">The collection of code descriptors</param>
		/// <param name="aValues">The text values used to set the codes</param>
		/// <param name="iBarcodeIndex">The index of the code containing the barcode identifier</param>
		/// <returns>true if no errors occur</returns>
		private bool SetCodes(CTmaxCaseCode[] aCaseCodes, string[] aValues, int iBarcodeIndex)
		{
			string			strBarcode = "";
			bool			bSuccessful = false;
			CDxMediaRecord	dxOwner = null;
			int				iStrip = 0;

			//	Get the barcode of the owner record
			strBarcode = aValues[iBarcodeIndex];
			if(strBarcode.Length == 0) return false;
			
			//	Should we apply the text options?
			if(this.Format == TmaxImportFormats.Codes)
			{
				//	Adjust based on user defined settings
				strBarcode = AdjustBarcode(strBarcode);
				
				//	Strip any .Secondary.Teritiary Id since we only apply
				//	codes to primary media
				if((iStrip = strBarcode.IndexOf(".")) >= 0)
					strBarcode = strBarcode.Substring(0, iStrip);

			}// if(this.Format == TmaxImportFormats.Codes)

			//	Get the record that owns the codes
			if((dxOwner = m_tmaxDatabase.GetRecordFromBarcode(strBarcode, true, false)) != null)
			{
				//	We only assign codes to the primary media record
				while(dxOwner.GetParent() != null)
					dxOwner = dxOwner.GetParent();
						
				//	Make sure this owner has a valid codes collection
				if(dxOwner.Codes != null)
				{
					//	Set the data
					bSuccessful = SetCodes(dxOwner, aCaseCodes, aValues, iBarcodeIndex);
				}
				else
				{
					AddMessage(ERROR_NO_CODES_COLLECTION, dxOwner.GetBarcode(true));
				}
					
			}
			else
			{
				AddMessage(ERROR_CODES_INVALID_BARCODE, strBarcode);
			}
			
			return bSuccessful;
			
		}// private bool SetCodes(CTmaxCaseCode[] tmaxCaseCodes, string[] aFields)
		
		/// <summary>This method sets a code for the owner record specified by the caller</summary>
		/// <param name="dxOwner">The record that owns the code</param>
		/// <param name="tmaxCaseCode">The data field descriptor</param>
		/// <param name="strValue">The text value of the code</param>
		/// <returns>true if successful</returns>
		private bool SetCode(CDxMediaRecord dxOwner, CTmaxCaseCode tmaxCaseCode, string strValue)
		{
			CDxCode					dxCode = null;
			CDxCodes				dxCodes = null;
			bool					bSuccessful = false;
			string []				aValues = null;
			TmaxImportProperties	tmaxProperty = TmaxImportProperties.Invalid;
			
			if(dxOwner == null) return false;
			if(tmaxCaseCode == null) return false;
			if(strValue == null) return false;
			if(strValue.Length == 0) return false;

			try
			{
				//	Is this a valid case code?
				if(tmaxCaseCode.Type != TmaxCodeTypes.Unknown)
				{
					//	The owner must have a valid codes collection
					if(dxOwner.Codes == null) return false;
				
					//	Is this a coded property?
					if(tmaxCaseCode.IsCodedProperty == true)
					{
						//	Make sure text fields are properly formatted
						if(tmaxCaseCode.Type == TmaxCodeTypes.Text)
							strValue = CheckMaxLength(tmaxCaseCode, strValue);
						else if(tmaxCaseCode.Type == TmaxCodeTypes.Memo)
							strValue = Expand(strValue);
						else if(tmaxCaseCode.Type == TmaxCodeTypes.Boolean)
							strValue = CTmaxToolbox.StringToBool(strValue).ToString();
									
						bSuccessful = dxOwner.SetCodedPropValue(tmaxCaseCode.CodedProperty, strValue, false);
					}
					else
					{
						//	Should we attempt to split this value into multiple fields?
						if((this.Format == TmaxImportFormats.Codes) && (m_tmaxImportOptions.SplitConcatenated == true))
						{
							aValues = strValue.Split(m_tmaxImportOptions.GetConcatenator().ToCharArray());
						
						}
						if(aValues == null)
						{
							aValues = new string[1];
							aValues[0] = strValue;
						}
							
						//	Process each value
						bSuccessful = true;
						for(int i = 0; i <= aValues.GetUpperBound(0); i++)
						{
							strValue = aValues[i].Trim();

							//	Should we check for an existing code of this type?
							if(this.Format == TmaxImportFormats.Codes)
							{
								if((m_tmaxImportOptions.OverwriteCodes == true) || (tmaxCaseCode.AllowMultiple == false))
								{
									//	Get all codes of this type
									dxCodes = dxOwner.Codes.FindAll(tmaxCaseCode);
									if((dxCodes != null) && (dxCodes.Count > 0))
									{
										if(tmaxCaseCode.AllowMultiple == false)
										{
											//	Should only be one
											dxCode = dxCodes[0];
										}
										else
										{
											//	Search for the first instance that has not already been imported in this operation
											foreach(CDxCode O in dxCodes)
											{
												Debug.Assert(tmaxCaseCode.Imported != null);
												if(tmaxCaseCode.Imported.Contains(O) == false)
												{
													dxCode = O;
													break;
												}
												
											}// foreach(CDxCode O in dxCodes)
										
										}// if(tmaxCaseCode.AllowMultiple == false)	
										
									}// if((dxCodes = dxOwner.Codes.FindAll(tmaxCaseCode)) != null)
														
								}// if((m_tmaxImportOptions.OverwriteCodes == true) || (tmaxCaseCode.AllowMultiple == false))
							
							}// if(this.Format == TmaxImportFormats.Codes)
							
							//	Create the new code record if necessary
							if(dxCode == null)
							{
								dxCode = new CDxCode(tmaxCaseCode, dxOwner);
								dxCode.Type = tmaxCaseCode.Type;
							}
							
							//	Set this codes value
							if(SetCode(dxOwner, dxCode, strValue) == true)
							{
								tmaxCaseCode.Imported.Add(dxCode);
							}
							else
							{	
								bSuccessful = false; // at least one failure
							}

							//	Reset for next value
							dxCode = null;
							
						}// for(int i = 0; i <= aValues.GetUpperBound(0); i++)
					
					}// if(tmaxCaseCode.IsCodedProperty == true)
				
				}
				else
				{
					//	Get the property id
					try { tmaxProperty = (TmaxImportProperties)(tmaxCaseCode.UniqueId); }
					catch {}
					
					//	Which property are we setting?
					switch(tmaxProperty)
					{
						case TmaxImportProperties.Name:
						
							//	Make sure maximum length has not been exceeded
							strValue = CheckMaxLength(tmaxCaseCode, strValue);
							
							dxOwner.Name = strValue;
							bSuccessful = true;
							break;
							
						case TmaxImportProperties.Barcode:
						default:
						
							break;
							
					}
			
				}// if(tmaxCaseCode.Type != TmaxCodeTypes.Unknown)
			
			}
			catch
			{
				AddMessage(ERROR_SET_CODE_EX, tmaxCaseCode.Name, strValue);
			}
			
			//	Add the owner record to the results
			if((bSuccessful == true) && (m_tmaxResults != null))
			{
				if(m_tmaxResults.Updated.Find(dxOwner) == null)
					m_tmaxResults.Updated.Add(new CTmaxItem(dxOwner));
			}			
			return bSuccessful;
			
		}// private bool SetCode(CDxMediaRecord dxOwner, CTmaxCaseCode tmaxCaseCode, string strValue)
		
		/// <summary>This method sets a code for the owner record specified by the caller</summary>
		/// <param name="dxOwner">The record that owns the code</param>
		/// <param name="dxCode">The exchange object for the code being set</param>
		/// <param name="strValue">The text value of the code</param>
		/// <returns>true if successful</returns>
		private bool SetCode(CDxMediaRecord dxOwner, CDxCode dxCode, string strValue)
		{
			bool			bSuccessful = false;
			CTmaxPickItem	tmaxPickItem = null;
			string			strOldValue = "";
			
			try
			{
				if(dxOwner == null) return false;
				if(dxCode == null) return false;
				if(dxCode.CaseCode == null) return false;
				if(strValue == null) return false;
				if(strValue.Length == 0) return false;
				
				//	Make sure fields are properly formatted
				if(dxCode.Type == TmaxCodeTypes.Text)
					strValue = CheckMaxLength(dxCode.CaseCode, strValue);
				else if(dxCode.Type == TmaxCodeTypes.Memo)
					strValue = Expand(strValue);
				else if(dxCode.Type == TmaxCodeTypes.Boolean)
					strValue = CTmaxToolbox.StringToBool(strValue).ToString();
									
				//	Is this code bound to a pick list?
				if(dxCode.CaseCode.PickList != null)
				{
					//	Does this value exist in the pick list?
					if((tmaxPickItem = dxCode.CaseCode.PickList.FindValue(strValue)) == null)
					{
						//	Can't add to multilevel case codes
						if(dxCode.CaseCode.IsMultiLevel == false)
						{
							//	Does this pick list allow user additions?
							if(dxCode.CaseCode.PickList.UserAdditions == true)
								tmaxPickItem = AddPickItem(dxCode.CaseCode.PickList, strValue);
							else
								AddMessage(ERROR_SET_CODE_NO_ADDITIONS, strValue, dxCode.CaseCode.PickList.Name);
						}
						else
						{
							AddMessage(ERROR_NO_MULTILEVEL_ADDITIONS, dxCode.CaseCode.Name, strValue);
						}
						
					}// if((tmaxPickItem = dxCode.CaseCode.PickList.FindChild(strValue)) == null)
					
					//	Change the value to use the pick list item id
					if(tmaxPickItem != null)
						strValue = tmaxPickItem.UniqueId.ToString();
					else
						return false;
							
				}// if(dxCode.CaseCode.PickList != null)
						
				//	Set the value of the code
				strOldValue = dxCode.GetValue(false);
				if(dxCode.SetValue(strValue) == true)
				{
					if(dxCode.AutoId == 0)
					{
						dxOwner.Codes.Add(dxCode);
					}
					else
					{
						//	Only update if the value has changed
						if(String.Compare(dxCode.GetValue(false), strOldValue, false) != 0)
							dxOwner.Codes.Update(dxCode);
					}
								
					bSuccessful = true;
				}
				else
				{
					AddMessage(ERROR_SET_CODE_INVALID, dxCode.Name, strValue);
				}
			
			}
			catch
			{
				AddMessage(ERROR_SET_CODE_EX, dxCode.Name, strValue);
			}
			
			return bSuccessful;
			
		}// private bool SetCode(CDxMediaRecord dxOwner, CDxCode dxCode, string strValue)
		
		/// <summary>This method checks the specified value to make sure it doesn't exceed maximum length</summary>
		/// <param name="tmaxCaseCode">The data field descriptor</param>
		/// <param name="strValue">The text value of the code</param>
		/// <returns>the adjusted (if necessary) value</returns>
		private string CheckMaxLength(CTmaxCaseCode tmaxCaseCode, string strValue)
		{
			try
			{
				if(strValue == null) return null;
			
				if(strValue.Length > CTmaxCaseCodes.CASE_CODES_MAX_TEXT_LENGTH)
				{
					//	Truncate the value
					strValue = strValue.Substring(0, CTmaxCaseCodes.CASE_CODES_MAX_TEXT_LENGTH);
					
					//	Add a warning so the user knows what's been done
					AddMessage(ERROR_CODE_LENGTH_EXCEEDED, CTmaxCaseCodes.CASE_CODES_MAX_TEXT_LENGTH, tmaxCaseCode.Name);
				}	
			
			}
			catch
			{
			}
			
			return strValue;
			
		}// private string CheckMaxLength(CTmaxCaseCode tmaxCaseCode, string strValue)

		/// <summary>This method expands CR/LF in the string specified by the caller</summary>
		/// <param name="strValue">The value to be expanded</param>
		/// <returns>the adjusted (if necessary) value</returns>
		private string Expand(string strValue)
		{
			string strSubstitute = "";
			
			try
			{
				if((strValue != null) && (strValue.Length > 0) && (m_tmaxImportOptions != null))
				{
					//	Get the substitution characters
					if(m_tmaxImportOptions.CRLFSubstitution != TmaxImportCRLF.None)
						strSubstitute = m_tmaxImportOptions.GetCRLFSubstitution();
						
					if(strSubstitute.Length > 0)
					{
						strValue = strValue.Replace(strSubstitute, "\r\n");	
						strValue = strValue.Replace(strSubstitute.ToLower(), "\r\n");	
						strValue = strValue.Replace(strSubstitute.ToUpper(), "\r\n");	
					}
				
				}
				
			}
			catch
			{
			}
			
			return strValue;
			
		}// private string Expand(string strValue)

		/// <summary>This method is called to build an array of data fields using the specified array of names</summary>
		/// <param name="aNames">The names used to identify the codes being imported</param>
		/// <returns>The collection of data fields used to add the coded (fielded) data</returns>
		private CTmaxCaseCode[] GetCaseCodes(string[] aNames)
		{
			CTmaxCaseCode[]			aCaseCodes = null;
			TmaxImportProperties	eImportProperty = TmaxImportProperties.Invalid;
			string					strName = "";
			bool					bInvalid = false;
			
			Debug.Assert(aNames != null);
			if(aNames == null) return null;
			Debug.Assert(aNames.GetUpperBound(0) >= 0);
			if(aNames.GetUpperBound(0) < 0) return null;

			try
			{
				//	Allocate the array of case codes
				aCaseCodes = new CTmaxCaseCode[aNames.GetUpperBound(0) + 1];
				
				for(int i = 0; i <= aNames.GetUpperBound(0); i++)
				{
					//	Get the name of the header
					strName = aNames[i].Trim();
					strName = CTmaxToolbox.StripQuotes(strName, true);
					
					//	Don't process the SortOrder column that we automatically add to the file
					if(strName == "SortOrder")
					{
						aCaseCodes[i] = null;
					}
					
					//	Is this column supposed to be a property?
					else if((m_strPropertyPrefix.Length > 0) && (strName.StartsWith(m_strPropertyPrefix) == true))
					{
						strName = strName.Substring(m_strPropertyPrefix.Length);
						strName = CTmaxToolbox.StripQuotes(strName, true);
						
						//	Is this a valid import property
						if((eImportProperty = CTmaxToolbox.GetImportProperty(strName)) != TmaxImportProperties.Invalid)
						{
							aCaseCodes[i] = CreateCaseCode(eImportProperty);
						}
						
						//	It could be one of our coded properties
						else if(CTmaxToolbox.GetCodedProperty(strName) != TmaxCodedProperties.Invalid)
						{
							//	Coded properties should already be in the case codes collection
							aCaseCodes[i] = m_tmaxDatabase.CaseCodes.Find(strName);
						}
						
						//	Were we unable to map this name to a property?
						if(aCaseCodes[i] == null)
						{
							AddMessage(ERROR_IMPORT_PROPERTY_NOT_FOUND, i + 1, strName, TmaxMessageLevels.FatalError);
							bInvalid = true;
						}
						
					}
					else
					{
						//	Is this a valid import property
						//
						//	NOTE:	We don't REQUIRE the user to provide the property
						//			prefix so we have to check here also
						if((eImportProperty = CTmaxToolbox.GetImportProperty(strName)) != TmaxImportProperties.Invalid)
						{
							aCaseCodes[i] = CreateCaseCode(eImportProperty);
						}
						else
						{
							//	Look for it in the case codes collection
							aCaseCodes[i] = m_tmaxDatabase.CaseCodes.Find(strName);
						}
						
						//	Did we find the code
						if(aCaseCodes[i] == null)
						{
							AddMessage(ERROR_CASE_CODE_NOT_FOUND, i + 1, strName, TmaxMessageLevels.FatalError);
							bInvalid = true;
						}
					
					}// if((m_strPropertyPrefix.Length > 0) && (strName.StartsWith(m_strPropertyPrefix) == true))
					
				}// for(int i = 0; i <= aFields.GetUpperBound(0); i++)
				
			}
			catch
			{
				AddMessage(ERROR_GET_CASE_CODES_EX, TmaxMessageLevels.FatalError);
				bInvalid = true;
			}
			
			if(bInvalid == false)
				return aCaseCodes;
			else
				return null;
		
		}// private CTmaxCaseCode[] GetCaseCodes(string[] aNames)
		
		/// <summary>This method is called to locate the index of the column containing the barcode</summary>
		/// <param name="aCaseCodes">The array of case codes</param>
		/// <returns>The index of the barcode column</returns>
		private int GetBarcodeIndex(CTmaxCaseCode [] aCaseCodes)
		{
			int iIndex = -1;
			
			//	Get the index of the column containing the barcode
			for(int i = 0; i <= aCaseCodes.GetUpperBound(0); i++)
			{
				if(aCaseCodes[i] == null) continue;
				
				//	Is this code created for one of our enumerated import properties?
				if(aCaseCodes[i].Type == TmaxCodeTypes.Unknown)
				{
					//	Is this the barcode property?
					if((TmaxImportProperties)(aCaseCodes[i].UniqueId) == TmaxImportProperties.Barcode)
					{
						iIndex = i;
						break;
					}
					
				}
				
			}// for(int i = 0; i <= aCaseCodes.GetUpperBound(0); i++)
			
			//	We have to have a column of barcodes
			if(iIndex < 0)
				AddMessage(ERROR_CODES_NO_BARCODE_COLUMN, TmaxMessageLevels.FatalError);
			
			return iIndex;
		
		}// private int GetBarcodeIndex(CTmaxCaseCode [] aCaseCodes)
		
		/// <summary>This method is called to create a case code to represent the specified import property</summary>
		/// <returns>The case code to identify the property</returns>
		private CTmaxCaseCode CreateCaseCode(TmaxImportProperties e)
		{
			CTmaxCaseCode tmaxCode = null;
			
			try
			{
				if(e != TmaxImportProperties.Invalid)
				{
					//	Allocate and initialize the new code
					tmaxCode = new CTmaxCaseCode();
					tmaxCode.Name = e.ToString();
					tmaxCode.UniqueId   = (int)e;
					tmaxCode.Type = TmaxCodeTypes.Unknown;
				}
					
			}
			catch
			{
			}
			
			return tmaxCode;
		
		}// private CTmaxCaseCode CreateCaseCode(TmaxImportProperties e)
		
		/// <summary>This method parses the next line in the stream</summary>
		///	<param name="strCommentCharacters">Characters to identify comment lines</param>
		/// <param name="strDelimiters">The delimiters used to separate the fields</param>
		/// <param name="bExpression">True to treat strDelimiters as a regular expression</param>
		/// <param name="bStripEmpty">True to strip empty fields at the end of the line</param>
		/// <returns>The array of fields parsed from the line of text</returns>
		private string[] Parse(string strCommentCharacters, string strDelimiters, bool bExpression, bool bStripEmpty)
		{
            string      strLine = "";
            string[]    aFields = null;
            string[]    aCleaned = null;
            int         iRemove = -1;

            while ((strLine = ReadLine()) != null)
            {
                //	Is this a blank line?
                strLine.Trim();
                if (strLine.Length == 0) continue;

                //	Is this a comment?
                if ((strCommentCharacters != null) && (strCommentCharacters.Length > 0))
                {
                    if (strCommentCharacters.IndexOf(strLine[0]) >= 0)
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
                    catch { }
                }

                if ((aFields != null) && (aFields.GetUpperBound(0) >= 0))
                {
                    //	Reset
                    iRemove = -1;

                    //	Trim all whitespace
                    for (int i = 0; i <= aFields.GetUpperBound(0); i++)
                    {
                        try { aFields[i] = aFields[i].Trim(); }
                        catch { }

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
                            for(int i = 0; i < iRemove; i++)
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

		}// private string[] Parse(string strCommentCharacters, string strDelimiters, bool bExpression, bool bStripEmpty)
		
		/// <summary>This method parses the next line in the stream using the local option settings</summary>
		/// <param name="bExpression">True to treat the local Delimiters as a regular expression</param>
		/// <param name="bStripEmpty">True to strip empty fields at the end of the line</param>
		/// <returns>The array of fields parsed from the line of text</returns>
		private string[] Parse(bool bExpression, bool bStripEmpty)
		{
			return Parse(m_strCommentCharacters, m_strDelimiters, bExpression, bStripEmpty);
		}

		/// <summary>This method parses the next line in the stream using the local option settings</summary>
		/// <param name="bExpression">True to treat the local Delimiters as a regular expression</param>
		/// <returns>The array of fields parsed from the line of text</returns>
		private string[] Parse(bool bExpression)
		{
			return Parse(m_strCommentCharacters, m_strDelimiters, bExpression, true);
		}

		/// <summary>This method parses the next line in the stream using the local option settings</summary>
		/// <returns>The array of fields parsed from the line of text</returns>
		private string[] Parse()
		{
			return Parse(m_strCommentCharacters, m_strDelimiters, false, true);
		}
		
		/// <summary>This method is called to prompt the user for the options used to import coded (fielded) data</summary>
		/// <returns>true to continue, false to cancel</returns>
		private bool GetCodesOptions()
		{
			bool bContinue = false;
			
			try
			{
				//	Are we importing from database?
				if(this.Format == TmaxImportFormats.CodesDatabase)
				{
					FTI.Trialmax.Forms.CFImportCodesDatabase databaseOptions = new CFImportCodesDatabase();
					
					m_tmaxEventSource.Attach(databaseOptions.EventSource);
					databaseOptions.ImportOptions = m_tmaxImportOptions;
					
					if(databaseOptions.ShowDialog() == DialogResult.OK)
						bContinue = true;
					
				}
				else
				{
					FTI.Trialmax.Forms.CFImportCodes importOptions = new CFImportCodes();
					
					m_tmaxEventSource.Attach(importOptions.EventSource);
					importOptions.ImportOptions = m_tmaxImportOptions;
					
					if(importOptions.ShowDialog() == DialogResult.OK)
						bContinue = true;
					
				}// if(this.Format == TmaxImportFormats.CodesDatabase)
			
			}
			catch
			{
			
			}
			
			return bContinue;
			
		}// private bool GetCodesOptions()
			
		/// <summary>This method is called to prompt the user for the options used to import coded (fielded) data</summary>
		/// <returns>true to continue, false to cancel</returns>
		private bool GetXmlCaseCodesOptions()
		{
			FTI.Trialmax.Forms.CFImportXmlCaseCodes importOptions = null;
			bool bContinue = false;
			
			try
			{
				importOptions = new CFImportXmlCaseCodes();
				
				m_tmaxEventSource.Attach(importOptions.EventSource);
				importOptions.ImportOptions = m_tmaxImportOptions;
				
				if(importOptions.ShowDialog() == DialogResult.OK)
					bContinue = true;
			
			}
			catch
			{
			
			}
			
			return bContinue;
			
		}// private bool GetXmlCaseCodesOptions()
			
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
				importOptions.ImportOptions = m_tmaxImportOptions;
				
				if(importOptions.ShowDialog() == DialogResult.OK)
				{
					m_strCommentCharacters = m_tmaxImportOptions.CommentCharacters;
					m_strDelimiters = m_tmaxImportOptions.GetDelimiter();
					
					bContinue = true;
				}
			
			}
			catch
			{
			
			}
			
			return bContinue;
			
		}// private bool GetScriptsOptions()
			
		/// <summary>This method is called to prompt the user for the options used to import scripts</summary>
		/// <returns>true to continue, false to cancel</returns>
		private bool GetBindersOptions()
		{
			FTI.Trialmax.Forms.CFImportBinders importOptions = null;
			bool bContinue = false;
			
			try
			{
				importOptions = new CFImportBinders();
				
				m_tmaxEventSource.Attach(importOptions.EventSource);
				importOptions.ImportOptions = m_tmaxImportOptions;
				
				if(importOptions.ShowDialog() == DialogResult.OK)
				{
					m_strCommentCharacters = m_tmaxImportOptions.CommentCharacters;
					m_strDelimiters = m_tmaxImportOptions.GetDelimiter();
					
					bContinue = true;
				}
			
			}
			catch
			{
			
			}
			
			return bContinue;
			
		}// private bool GetBindersOptions()
			
		/// <summary>This method is called to prompt the user for the options used to import XML scripts</summary>
		/// <returns>true to continue, false to cancel</returns>
		private bool GetXmlScriptsOptions()
		{
			FTI.Trialmax.Forms.CFImportXmlScripts xmlImport = null;
			bool bContinue = false;
			
			try
			{
				xmlImport = new CFImportXmlScripts();
				
				m_tmaxEventSource.Attach(xmlImport.EventSource);
				xmlImport.ImportOptions = m_tmaxImportOptions;
				
				if(xmlImport.ShowDialog() == DialogResult.OK)
				{
					bContinue = true;
				}
			
			}
			catch
			{
			}
			
			return bContinue;
			
		}// private bool GetXmlScriptsOptions()
			
		/// <summary>This method is called to get an event item used to add the binder entry to the database</summary>
		///	<param name="tmaxBinder">The binder entry to be added to the database</param>
		/// <returns>The event item if successful</returns>
		private CTmaxItem GetCmdItem(CTmaxBinderItem tmaxBinder)
		{
			CTmaxItem		tmaxCmdItem = null;
			CTmaxItem		tmaxChild = null;
			CDxMediaRecord	dxRecord = null;
			
			try
			{
				//	Is this a media entry?
				if(tmaxBinder.IsMedia == true)
				{
					if((dxRecord = m_tmaxDatabase.GetRecordFromBarcode(tmaxBinder.Barcode, true, false)) != null)
					{
						tmaxCmdItem = new CTmaxItem(dxRecord);
					}
					else
					{
						AddMessage(ERROR_INVALID_BARCODE, tmaxBinder.Barcode);
					}
						
				}
				else
				{
					//	Make it appear as though this is being created from a folder
					tmaxCmdItem = new CTmaxItem();
					tmaxCmdItem.SourceFolder = new CTmaxSourceFolder();
					tmaxCmdItem.SourceFolder.Name = tmaxBinder.Name;
					
					//	Does this binder have any children?
					if((tmaxBinder.Children != null) && (tmaxBinder.Children.Count > 0))
					{
						if(tmaxCmdItem.SourceItems == null)
							tmaxCmdItem.SourceItems = new CTmaxItems();
							
						foreach(CTmaxBinderItem O in tmaxBinder.Children)
						{
							if((tmaxChild = GetCmdItem(O)) != null)
								tmaxCmdItem.SourceItems.Add(tmaxChild);
						}
						
					}// if((tmaxBinder.Children != null) && (tmaxBinder.Children.Count > 0))
					
				}// if(tmaxBinder.IsMedia == true)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetCmdItem", Ex);
				tmaxCmdItem = null;
			}
			
			return tmaxCmdItem;
			
		}// private CTmaxItem GetCmdItem(CTmaxBinderItem tmaxBinder)
			
		/// <summary>This method cleans the field text before processing</summary>
		/// <param name="tmaxCode">The case code associated with the field</param>
		/// <param name="strField">The field string being processed</param>
		/// <returns>The cleaned up string</returns>
		private string CleanField(CTmaxCaseCode tmaxCode, string strField)
		{
			TmaxImportProperties	tmaxProp = TmaxImportProperties.Invalid;
			string					strClean = strField;
			
			if(strField.Length == 0) return strField;
			
			//	Trim leading and trailing whitespace
			strField = strField.Trim();
			
			//	What type of data is this?
			switch(tmaxCode.Type)
			{
				case TmaxCodeTypes.Unknown:
				
					//	Is this an enumerated column?
					try { tmaxProp = (TmaxImportProperties)(tmaxCode.UniqueId); }
					catch {}
					
					if(tmaxProp != TmaxImportProperties.Invalid)
						strField = CTmaxToolbox.StripQuotes(strField, true);
						
					break;
					
				case TmaxCodeTypes.Date:
				case TmaxCodeTypes.Memo:
				case TmaxCodeTypes.Text:
				case TmaxCodeTypes.Boolean:
				case TmaxCodeTypes.Decimal:
				case TmaxCodeTypes.Integer:
				default:
				
					//	Strip bounding quotes
					strField = CTmaxToolbox.StripQuotes(strField, true);
					break;
			}
			
			return strField;
			
		}// private string CleanField(CTmaxCaseCode tmaxCode, string strField)
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to initialize the import operation");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the target for the import operation");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to get the source files for the import operation");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to execute the import operation");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create the status form for the import operation");
			
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate %1 to perform the import operation");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to open %1 to perform the import operation");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to import from %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Invalid designation line: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Invalid barcode identifier: barcode = %1");
			
			m_tmaxErrorBuilder.FormatStrings.Add("Invalid designation page/line values");
			m_tmaxErrorBuilder.FormatStrings.Add("Invalid deposition MediaID: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("No deposition specified for the designation");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the highlighter for the designation: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("The designation's start page/line is out of range");
			
			m_tmaxErrorBuilder.FormatStrings.Add("The designation's stop page/line is out of range");
			m_tmaxErrorBuilder.FormatStrings.Add("The designation's start and stop page/line values are reversed");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to create XML designations using the specified range: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Invalid video segment identifier: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Exception raised while processing the column headers for fielded data");
			
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the data field descriptor from the header in column %1: header = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the property with the name defined in column %1: name = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the column containing the barcode of the owner record");
			m_tmaxErrorBuilder.FormatStrings.Add("Invalid barcode identifier: barcode = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("The owner record does not have a valid fielded data collection: barcode = %1");
			
			m_tmaxErrorBuilder.FormatStrings.Add("Exception raised while attempting to set code: code = %1 value = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("Invalid value for code: code = %1 value = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to perform the operation. No regular expression has been provided to parse the input file.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to perform the operation. %1 is not a valid regular expression.");
			m_tmaxErrorBuilder.FormatStrings.Add("Maximum text length (%1) exceeded for %2. The value has been truncated.");
			
			m_tmaxErrorBuilder.FormatStrings.Add("Invalid import line - insufficient field count: fields = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to split the file path: path = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to update the original record: MediaId = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to back up the original record: MediaId = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to assign new MediaID to the backup record: OriginalId = %1 NewId = %2");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to locate the binder references for the active script: MediaId = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to update the binder entries for the imported script: MediaId = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to create XML scenes for the import operation");
			m_tmaxErrorBuilder.FormatStrings.Add("The XML file does not contain any script scenes");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to import scene #%1");

			m_tmaxErrorBuilder.FormatStrings.Add("Unable to import scene #%1. No Source Id defined.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to import scene #%1. No source record with Id = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to get the primary source record for scene #%1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to get the secondary source record for the designation named: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate source designation for scene #%1: SourceId = %2");

			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate primary source for scene #%1: PrimaryId = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate secondary source for scene #%1: SegmentId = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to locate / add the specified XML deposition: MediaId = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to save %1 to add the source deposition");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to replace the existing script: MediaId = %1");
			
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate %1 in imported script. Entry removed from binder named %2");
			m_tmaxErrorBuilder.FormatStrings.Add("A value named %1 already appears in the pick list.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to import the pick list.");
			//Change the word xml to file to cator both xml and file formats error.
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to import the data fields from the file.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a new value to the pick list: Name = %1 Value = %2");
			
			m_tmaxErrorBuilder.FormatStrings.Add("The attempt to add a new value to the pick list failed: Name = %1 Value = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to add %1 to %2. User additions to this pick list not allowed.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to open the database for the operation: filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to open the database for fielded data: filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("The database does not contain any fielded data information.");
		
			m_tmaxErrorBuilder.FormatStrings.Add("There we no binder descriptors found in the XML file.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to add values to multi-level case codes: case code: %1 value: %2");
			m_tmaxErrorBuilder.FormatStrings.Add("Start position for the recording clip is not valid");
			m_tmaxErrorBuilder.FormatStrings.Add("Stop position for the recording clip is not valid");
			m_tmaxErrorBuilder.FormatStrings.Add("Start and stop positions for the recording clip are reversed");

			m_tmaxErrorBuilder.FormatStrings.Add("No barcode identifier");
			m_tmaxErrorBuilder.FormatStrings.Add("The database does not contain concatenated codes.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to import objections for the XML script: filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to add objections contained in the XML script. No objections database available: filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to add objections contained in the XML deposition. Unable to retrieve the deposition record: MediaId = %1");
			
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
		/// <param name="eLevel">Enumerated message error level identifier</param>
		private void AddMessage(int iErrorId, object param1, object param2, object param3, TmaxMessageLevels eLevel)
		{
			AddMessage(m_tmaxErrorBuilder.Message(iErrorId, param1, param2, param3), eLevel);
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

		/// <summary>Called to add the record to the results list on the status form</summary>
		/// <param name="dxRecord">The record that has been processed</param>
		/// <param name="eResult">Predefined result type identifier</param>
		/// <returns>True if successful</returns>
		private bool AddStatusResult(CDxMediaRecord dxRecord, TmaxImportResults eResult)
		{
			string	strMessage = "";
			string	strFilename = "";
			bool	bSuccessful = false;

			//	Only supported when importing XMLS
			if(this.Format != TmaxImportFormats.XmlScript)
				return false;
				
			try
			{
				//	Format the descriptor
				strMessage = String.Format("{0}", dxRecord.GetBarcode(false));

				if((m_wndStatusForm != null) && (m_wndStatusForm.IsDisposed == false))
				{
					if(FileSpec.Length > 0)
						strFilename = System.IO.Path.GetFileName(FileSpec);

					m_wndStatusForm.AddResult(strFilename, LineNumber, eResult, strMessage, dxRecord);
				
					bSuccessful = true;
				}

			}
			catch
			{
			}
			
			return bSuccessful;

		}// private void AddStatusResult(CDxBaseRecord dxRecord, TmaxImportResults eResult)

		/// <summary>This method uses the fields collection to retrieve/add one or more records</summary>
		/// <param name="aFields">The fields parsed from the line in the import file</param>
		/// <param name="tmaxImported">Collection where event items representing imported records should be stored</param>
		/// <returns>true if successful</returns>
		private bool ImportMedia(string [] aFields, CTmaxItems tmaxImported)
		{
			CDxMediaRecord	dxRecord = null;
			CDxSecondary	dxSegment = null;
			long			lRecords = 0;
			string			strBarcode = "";
			
			Debug.Assert(m_tmaxDatabase != null);
			if(m_tmaxDatabase == null) return false;
			
			Debug.Assert(aFields != null);
			if(aFields == null) return false;
			Debug.Assert(aFields.GetUpperBound(0) >= 0);
			if(aFields.GetUpperBound(0) < 0) return false;
			
			//	Create our own collection if one not provided by the caller
			if(tmaxImported == null)
				tmaxImported = new CTmaxItems();
			lRecords = tmaxImported.Count;
			
			//	Are we importing a binder?
			if(this.Format == TmaxImportFormats.AsciiBinder)
			{
				if(aFields[0].Length > 0)
				{
					//	Adjust based on user defined settings
					strBarcode = AdjustBarcode(aFields[0]);

					if((dxRecord = m_tmaxDatabase.GetRecordFromBarcode(strBarcode,true,true)) != null)
						tmaxImported.Add(new CTmaxItem(dxRecord));
					else
						AddMessage(ERROR_INVALID_BARCODE,strBarcode);
				}
				else
				{
					AddMessage(ERROR_NO_BARCODE);
				}

			}// if(this.Format == TmaxImportFormats.AsciiBinder)
			
			//	Are we importing a script?
			else if(this.Format == TmaxImportFormats.AsciiMedia)
			{
				//	Is this line a simple barcode id?
				if(aFields.GetUpperBound(0) == 0)
				{
					if(aFields[0].Length > 0)
					{
						//	Is there a pending designation?
						if(m_lastDesignation != null)
						{
							//	Add it to the database
							ImportDesignation(m_lastDesignation,tmaxImported);
							m_lastDesignation = null;
						}

						//	This should be a barcode
						strBarcode = AdjustBarcode(aFields[0]);

						//	Get the source record
						if((dxRecord = m_tmaxDatabase.GetRecordFromBarcode(strBarcode,true,true)) != null)
						{
							tmaxImported.Add(new CTmaxItem(dxRecord));
						}
						else
						{
							AddMessage(ERROR_INVALID_BARCODE,strBarcode);
						}

					}
					else
					{
						AddMessage(ERROR_NO_BARCODE);

					}// if(aFields[0].Length > 0)

				}
				else if((dxSegment = IsAsciiClip(aFields)) != null)
				{
					//	Is there a pending designation?
					if(m_lastDesignation != null)
					{
						//	Add it to the database
						ImportDesignation(m_lastDesignation, tmaxImported);
						m_lastDesignation = null;
					}
					
					//	Now import the clip
					ImportClip(dxSegment, aFields, tmaxImported);					
				}
				else
				{
					ImportDesignation(aFields, tmaxImported);
				}

			}
			else
			{
				//	Don't know how to import other formats here
				Debug.Assert(false, "Invalid import format");
			}
		
			return ((tmaxImported.Count - lRecords) > 0);
			
		}// private bool ImportMedia(string [] aFields, CTmaxItems tmaxImported)
		
		/// <summary>This method uses the fields collection to import a video designation</summary>
		/// <param name="aFields">The fields parsed from the line in the import file</param>
		/// <param name="tmaxImported">Collection where event items representing imported records should be stored</param>
		/// <returns>true if successful</returns>
		private bool ImportDesignation(string [] aFields, CTmaxItems tmaxImported)
		{
			CTmaxImportDesignation	tmaxDesignation = null;
			long					lRecords = 0;
			
			Debug.Assert(m_tmaxDatabase != null);
			if(m_tmaxDatabase == null) return false;
			
			Debug.Assert(aFields != null);
			if(aFields == null) return false;
			Debug.Assert(aFields.GetUpperBound(0) >= 0);
			if(aFields.GetUpperBound(0) < 0) return false;
			
			//	Create our own collection if one not provided by the caller
			if(tmaxImported == null)
				tmaxImported = new CTmaxItems();
			lRecords = tmaxImported.Count;
			
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
				if(SetDeposition(tmaxDesignation) == false)
					break;
							
				//	Assign the highlighter
				if(SetHighlighter(tmaxDesignation) == false)
					break;
							
				//	Make sure the start and stop positions are valid
				if(CheckRange(tmaxDesignation) == false)
					break;
							
				//	First try to add this designation to the end of the previous designation
				if(Merge(tmaxDesignation, tmaxImported) == true)
					break;	//	Nothing more to do if merged

				//	Process the pending designation if it exists
				if(m_lastDesignation != null)
				{
					ImportDesignation(m_lastDesignation, tmaxImported);
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
					//	Add it to the database since we can't merge the next one in the file
					ImportDesignation(tmaxDesignation, tmaxImported);
				}
						
				//	We're done
				break;
					
			}// while(true)
					
			return ((tmaxImported.Count - lRecords) > 0);
			
		}// private bool ImportMedia(string [] aFields, CTmaxItems tmaxImported)
		
		/// <summary>This method imports the designation(s) defined by the specified descriptor</summary>
		/// <param name="tmaxDesignation">The import designation descriptor</param>
		///	<param name="tmaxAdded">The collection in which to store the new designations</param>
		///	<returns>The number of designations added to the database</returns>
		private int ImportDesignation(CTmaxImportDesignation tmaxDesignation, CTmaxItems tmaxAdded)
		{
			CXmlDesignations	xmlDesignations = null;
			CXmlDesignation		xmlDesignation = null;
			long				lSegment = 0;
			int					iAdded = 0;
			CDxSecondary		dxSegment = null;
			CDxTertiary			dxDesignation = null;
			CDxPrimary			dxDeposition = null;

			Debug.Assert(tmaxDesignation != null);
			if(tmaxDesignation == null) return 0;
			Debug.Assert(tmaxDesignation.XmlDeposition != null);
			if(tmaxDesignation.XmlDeposition == null) return 0;
			
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
			
				//	Do we have any lines that should be removed from the transcript text?
				if(tmaxDesignation.HasIgnoreLines == true)
				{
					foreach(CXmlTranscript O in tmaxDesignation.XmlIgnoreLines)
					{
						//	Locate the owner designation
						if((xmlDesignation = xmlDesignations.FindFromPL(O.PL)) != null)
						{
							xmlDesignation.RemoveLine(O.PL);
						}

					}// foreach(CXmlTranscript O in tmaxDesignation.XmlIgnoreLines)

				}// if(tmaxDesignation.HasIgnoreLines == true)
				
				//	The parent deposition should be assigned to the import descriptor
				if((dxDeposition = (CDxPrimary)(tmaxDesignation.DxDeposition)) == null)
					dxDeposition = m_dxDeposition; // This shouldn't happen....
				if(dxDeposition == null) return 0;	
				
				//	Make sure we've populated the segments collection
				if(dxDeposition.Secondaries.Count == 0)
					dxDeposition.Fill();
				
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
				if(this.Cancelled) return 0;
				
				//	Add each designation to the database
				foreach(CXmlDesignation O in xmlDesignations)
				{
					//	Get the secondary segment for this designation
					try	
					{ 
						dxSegment = null;
						lSegment = System.Convert.ToInt64(O.Segment); 

						foreach(CDxSecondary S in dxDeposition.Secondaries)
						{
							if((S.GetExtent() != null) && (S.GetExtent().XmlSegmentId == lSegment))
							{
								dxSegment = S;	
								break;
							}

						}// foreach(CDxSecondary S in m_tmaxImportManager.Deposition.Secondaries)
					
					}
					catch
					{
					}
			
					//	Did we find the appropriate segment
					if(dxSegment != null)
					{
						if((dxDesignation = m_tmaxDatabase.AddDesignation(dxSegment, O)) != null)
						{
							iAdded++;
							if(tmaxAdded != null)
								tmaxAdded.Add(new CTmaxItem(dxDesignation));
						}
					}
					else
					{
						AddMessage(ERROR_INVALID_SEGMENT, O.Segment);
						continue;
					}
			
				}// foreach(CXmlDesignation O in xmlDesignations)
			
			}
			catch
			{
			}
		
			return iAdded;
			
		}// private int ImportDesignation(CTmaxImportDesignation tmaxDesignation, CTmaxItems tmaxAdded)
		
		/// <summary>This method uses the fields collection to import a recording clip</summary>
		/// <param name="dxSegment">The parent recording segment</param>
		/// <param name="aFields">The fields parsed from the line in the import file</param>
		/// <param name="tmaxImported">Collection where event items representing imported records should be stored</param>
		/// <returns>true if successful</returns>
		private bool ImportClip(CDxSecondary dxSegment, string [] aFields, CTmaxItems tmaxImported)
		{
			CXmlDesignation	xmlDesignation = null;
			CDxTertiary		dxClip = null;
			long			lRecords = 0;
			double			dStart = -1.0;
			double			dStop = -1.0;
			
			Debug.Assert(m_tmaxDatabase != null);
			if(m_tmaxDatabase == null) return false;
			
			Debug.Assert(dxSegment != null);
			if(dxSegment == null) return false;
			Debug.Assert(dxSegment.MediaType == TmaxMediaTypes.Segment);
			if(dxSegment.MediaType != TmaxMediaTypes.Segment) return false;
			
			Debug.Assert(aFields != null);
			if(aFields == null) return false;
			Debug.Assert(aFields.GetUpperBound(0) >= 3);
			if(aFields.GetUpperBound(0) < 3) return false;
			
			//	Create our own collection if one not provided by the caller
			if(tmaxImported == null)
				tmaxImported = new CTmaxItems();
			lRecords = tmaxImported.Count;
			
			//	Get the start and stop positions
			try 
			{ 
				dStart = System.Convert.ToDouble(aFields[0]); 
				dStop = System.Convert.ToDouble(aFields[1]); 
			}
			catch
			{
			}
			
			//	Are the start and stop positions valid?
			if((dStart < 0) || ((dxSegment.GetExtent().Start > 0) && (dStart < dxSegment.GetExtent().Start)))
			{
				AddMessage(ERROR_CLIP_START_INVALID);
				return false;
			}
			if((dStop < 0) || ((dxSegment.GetExtent().Stop > 0) && (dStop > dxSegment.GetExtent().Stop)))
			{
				AddMessage(ERROR_CLIP_STOP_INVALID);
				return false;
			}
			
			//	Did the user reverse the values?
			if(dStop < dStart)
			{
				AddMessage(ERROR_CLIP_START_STOP_REVERSED);
				return false;
			}
			
			//	Create an XML designation to use for adding the clip to the database
			xmlDesignation = new CXmlDesignation();
			
			//	Set the properties of the designation
			xmlDesignation.Start = dStart;
			xmlDesignation.Stop = dStop;
			xmlDesignation.HasText = false;
			xmlDesignation.ScrollText = false;
			if(dxSegment.Name.Length > 0)
				xmlDesignation.SetNameFromExtents(dxSegment.Name);
			else
				xmlDesignation.SetNameFromExtents(dxSegment.Filename);

			//	Get the tune flags
			try { xmlDesignation.StartTuned = (System.Convert.ToDouble(aFields[2]) > 0); }
			catch{}
			try { xmlDesignation.StopTuned = (System.Convert.ToDouble(aFields[3]) > 0); }
			catch{}
			
			if((dxClip = m_tmaxDatabase.AddDesignation(dxSegment, xmlDesignation)) != null)
				tmaxImported.Add(new CTmaxItem(dxClip));
					
			return ((tmaxImported.Count - lRecords) > 0);
			
		}// private bool ImportClip(CDxSecondary dxSegment, string [] aFields, CTmaxItems tmaxImported)



        
        /// <summary>This method uses the media record and traverses it</summary>
		/// <param name="mediaRecord">The node on which traversal is to be performed</param>
		/// <param name="aFields">The fields parsed from the line in the import file</param>
                
        /// <summary>This method assigns the pending records to the appropriate parent record</summary>
		/// <returns>True if successful</returns>
		private bool SetPendingParent()
		{
			CTmaxItem				tmaxAdd = null;
			CTmaxDatabaseResults	tmaxResults = null;
			CTmaxParameters			tmaxParameters = null;
			CDxMediaRecord			dxParent = null;
			CDxMediaRecord			dxInsert = null;
			bool					bAddedParent = true;          


			Debug.Assert(m_tmaxPending != null);
			if(m_tmaxPending == null) return false;
			Debug.Assert(m_tmaxPending.Count > 0);

            if (this.Cancelled == true) return false;

            //	Are we merging all files into one?
            if (m_bMergeSource == true)
            {
                //	Did the caller specify a target?
                if (this.Target != null)
                {
                    //	The target is the parent
                    dxParent = ((CDxMediaRecord)(this.Target));
                    dxInsert = ((CDxMediaRecord)(this.InsertAt));
                    bAddedParent = false;
                }
                else
                {
                    //	Create a new parent using the name of the first source file
                    dxParent = AddParent(m_aSourceFiles[0]);
                    dxInsert = null; // No insertion point if no target
                }

            }
            else
            {
                //	Create a new parent using the name of the active source file
                dxParent = AddParent(this.FileSpec);
                dxInsert = null;
            }

			//	Must have been a problem if we don't have a parent record
			if(dxParent == null) return false;
			
			//	Now add the pending children to the parent
			tmaxAdd = new CTmaxItem(dxParent);
			tmaxResults = new CTmaxDatabaseResults();
			
			if(dxInsert != null)
			{
				tmaxAdd.SubItems.Add(new CTmaxItem(dxInsert));
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.Before, this.InsertBefore);
				
			}
			
			if(tmaxAdd.SourceItems == null)
				tmaxAdd.SourceItems = new CTmaxItems();
			foreach(CTmaxItem O in m_tmaxPending)
				tmaxAdd.SourceItems.Add(O);

            if (m_tmaxDatabase.Add(tmaxAdd, tmaxParameters, tmaxResults) == true)
            {
                //	Add entries for each of the children to the results
                //	collection if we did not add their parent
                if ((m_tmaxResults != null) && (bAddedParent == false))
                {
                    foreach (CTmaxItem O in tmaxResults.Added)
                    {
                        m_tmaxResults.Added.Add(O);
                    }

                }// if(bAddedParent == false)

                //	Have we imported a new XML script?
                if ((bAddedParent == true) && (this.Format == TmaxImportFormats.XmlScript))
                {
                    //	Are we replacing the existing script?
                    if ((m_tmaxImportOptions.UpdateScripts == true) && (m_dxXmlOriginal != null))
                    {
                        //	Replace the original with the new script
                        Replace(m_dxXmlOriginal, (CDxPrimary)dxParent);

                        AddStatusResult(dxParent, TmaxImportResults.Updated);
                    }
                    else
                    {
                        AddStatusResult(dxParent, TmaxImportResults.Added);
                    }

                }

            }// if(m_tmaxDatabase.Add(tmaxAdd, tmaxAdded, tmaxParameters) == true)

			return true;
		
		}// bool SetPendingParent()
		
		/// <summary>This method will add a parent record to the database</summary>
		/// <param name="strFileSpec">Name of file used to initialize the MediaID or binder name</param>
		/// <returns>The exchange interface of the new parent record</returns>
		private CDxMediaRecord AddParent(string strFileSpec)
		{
			CDxMediaRecord		dxParent = null;
			CDxPrimary			dxOriginal = null;
			CTmaxItem			tmaxParent = null;
			CTmaxItem			tmaxAdded = null;
			string				strMediaId = "";
			string				strName = "";
			
			//	Should we create a new binder?
			if(this.Format == TmaxImportFormats.AsciiBinder)
			{
				tmaxParent = new CTmaxItem();
				tmaxParent.DataType = TmaxDataTypes.Binder;
						
				//	Use the filename to initialize the binder name
				tmaxParent.SourceFile = new CTmaxSourceFile(strFileSpec);
					
				//	Add a new binder to the database
                dxParent = m_tmaxDatabase.AddBinderEntry((CDxBinderEntry)this.Target, tmaxParent, (CDxBinderEntry)(this.InsertAt), this.InsertBefore);
            }
			
			//	Should we create a parent script?
			else if((this.Format == TmaxImportFormats.AsciiMedia) ||
					(this.Format == TmaxImportFormats.XmlScript))
			{
				//	Split the file path into MediaID and Name
				if(SplitFileSpec(strFileSpec, ref strMediaId, ref strName) == false)
					return null;
					
				//	Get the reference to the original record if performing an update
				if((m_tmaxImportOptions != null) && (m_tmaxImportOptions.UpdateScripts == true))
				{
					//	Locate the record with the specified MediaID
					try { dxOriginal = (CDxPrimary)(m_tmaxDatabase.GetRecordFromBarcode(strMediaId, true, false)); }
					catch {}
					
					//	The original MUST be a script
					if((dxOriginal != null) && (dxOriginal.MediaType != TmaxMediaTypes.Script))
						dxOriginal = null;
				}
				
				//	Add a new record if not preforming an update
				if(dxOriginal == null)
				{
					dxParent = m_tmaxDatabase.AddPrimary(TmaxMediaTypes.Script, strMediaId, strName, ("Imported from " + strFileSpec));
					m_dxXmlOriginal = null;
				}
				else
				{
					//	Are we importing from XML?
					if(this.Format == TmaxImportFormats.XmlScript)
					{
						//	Create the new parent with a temporary media id
						dxParent = m_tmaxDatabase.AddPrimary(TmaxMediaTypes.Script, System.Guid.NewGuid().ToString(), strName, ("Imported from " + strFileSpec));
						
						//	Store the original 
						if(dxParent != null)
							m_dxXmlOriginal = dxOriginal;
						else
							m_dxXmlOriginal = null; // Prevent removal
					}
					else
					{
						dxParent = Update(dxOriginal, strName);
					}
				
				}// if(dxOriginal == null)

			}
			else
			{
				Debug.Assert(false, "Unable to create parent records during " + this.Format.ToString() + " import operation");
			}
			
			//	Add the new parent to the results collection
			if((m_tmaxResults != null) && (dxParent != null))
			{
				tmaxAdded = new CTmaxItem(dxParent.GetParent());
				tmaxAdded.DataType = dxParent.GetDataType();
				
				//	This allows anybody processing the return results to 
				//	determine if the record was added as a result of an update
				tmaxAdded.UserData1 = dxOriginal;
				
				tmaxAdded.SubItems.Add(new CTmaxItem(dxParent));
				m_tmaxResults.Added.Add(tmaxAdded);
			}
				
			return dxParent;
		
		}// CDxMediaRecord AddParent(string strFileSpec)
		
		/// <summary>This method is called to set the XML deposition associated with the specified designation</summary>
		/// <param name="tmaxDesignation">The designation being imported</param>
		///	<returns>true if successful</returns>
		private bool SetDeposition(CTmaxImportDesignation tmaxDesignation)
		{
			//	Has a deposition been assigned to the designation?
			if(tmaxDesignation.DepositionId.Length > 0)
			{
				//	Is this the active deposition?
				if((this.Deposition != null) && (String.Compare(this.Deposition.MediaId, tmaxDesignation.DepositionId, true) == 0))
				{
					//	Make sure we have the XML
					if(this.XmlDeposition == null)
						this.XmlDeposition = m_tmaxDatabase.GetXmlDeposition(this.Deposition, true, true, false);
				}
				else
				{
					//	Locate this deposition
					if((this.Deposition = m_tmaxDatabase.Primaries.Find(tmaxDesignation.DepositionId)) == null)
					{
						AddMessage(ERROR_INVALID_DEPOSITION_MEDIA_ID, tmaxDesignation.DepositionId);
						this.XmlDeposition = null;
					}
					else
					{
						//	Get the XML
						this.XmlDeposition = m_tmaxDatabase.GetXmlDeposition(this.Deposition, true, true, false);
					}
				
				}// if((m_tmaxImportManager.Deposition != null) && (String.Compare(m_tmaxImportManager.Deposition.MediaId, strDeposition, true) == 0))
				
			}
			else
			{
				//	Do we have a deposition to use?
				if(this.XmlDeposition == null)
					AddMessage(ERROR_NO_DEPOSITION_SPECIFIED);
			}
			
			//	Assign the values to the caller's object
			if(this.Deposition != null)
			{
				if(tmaxDesignation.DepositionId.Length == 0)
					tmaxDesignation.DepositionId = this.Deposition.MediaId;
				
				tmaxDesignation.DxDeposition = this.Deposition;
				tmaxDesignation.XmlDeposition = this.XmlDeposition;
				
			}// if(this.Deposition != null)
			
			return (this.XmlDeposition != null);
		
		}// private bool SetDeposition(CTmaxImportDesignation tmaxDesignation)
		
		/// <summary>This method is called to set the highlighter to use for creating a new designation</summary>
		/// <param name="tmaxDesignation">The designation being imported</param>
		///	<returns>true if successful</returns>
		private bool SetHighlighter(CTmaxImportDesignation tmaxDesignation)
		{
			CDxHighlighter dxHighlighter = null;
			
			//	Get the highlighter to use for these designations
			if((dxHighlighter = m_tmaxDatabase.Highlighters.Find(tmaxDesignation.HighlighterId)) != null)
				this.Highlighter = dxHighlighter;
			else if((this.Highlighter == null) && (m_tmaxDatabase.Highlighters.Count > 0))
				this.Highlighter = m_tmaxDatabase.Highlighters[0];
				
			if(this.Highlighter == null)
				AddMessage(ERROR_INVALID_HIGHLIGHTER, tmaxDesignation.HighlighterId);
			
			//	Set the highlighter Id so that we can create the designations
			if(this.Highlighter != null)
				tmaxDesignation.HighlighterId = (int)(this.Highlighter.AutoId);
			
			return (this.Highlighter != null);
		
		}// private bool SetHighlighter(CTmaxImportDesignation tmaxDesignation)
		
		/// <summary>This method is called to verify that the specified designation is within the range defined by it's source deposition</summary>
		/// <param name="tmaxDesignation">The designation being imported</param>
		/// <returns>true if within range</returns>
		private bool CheckRange(CTmaxImportDesignation tmaxDesignation)
		{
			CDxTranscript dxTranscript = null;

			
            Debug.Assert(tmaxDesignation != null);
			if(tmaxDesignation == null) return false;
			
			//	The owner deposition should have been assigned before calling this
			//	method
			if(m_dxDeposition == null)
			{
				Debug.Assert(tmaxDesignation != null, "Deposition not assigned");
				return false;
			}
			
			//	Get the transcript associated with this deposition
			if((dxTranscript = m_dxDeposition.GetTranscript()) == null)
			{
				Debug.Assert(m_dxDeposition.GetTranscript() != null, "Transcript not available");
				return false;
			}


            if ((dxTranscript.LinesPerPage > 0) && (tmaxDesignation.StartLine > dxTranscript.LinesPerPage))
            {
                //Warn(m_tmaxErrorBuilder.Message(ERROR_START_LINE_OUT_OF_RANGE, m_dxTranscript.LinesPerPage), m_ctrlStartLine);
                AddMessage(string.Format("The specified start line is out of range. The maximum lines per page is {0}",dxTranscript.LinesPerPage));
                return false;
            }

            if ((dxTranscript.LinesPerPage > 0) && (tmaxDesignation.StopLine > dxTranscript.LinesPerPage))
            {
                //Warn(m_tmaxErrorBuilder.Message(ERROR_STOP_LINE_OUT_OF_RANGE, m_dxTranscript.LinesPerPage), tmaxDesignation.StopLine);
                AddMessage(string.Format("The specified stop line is out of range. The maximum lines per page is {0}",dxTranscript.LinesPerPage));
                return false;
            }

			
			//	Check the range specified by the caller against the transcript extents
			if(tmaxDesignation.StartPL < dxTranscript.FirstPL)
			{
				AddMessage(ERROR_START_PL_OUT_OF_RANGE);
				return false;
			}
			if((dxTranscript.LastPL > 0) && (tmaxDesignation.StopPL > dxTranscript.LastPL))
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



		/// <summary>This method is called to merge the specified designation with the last designation read in from the file</summary>
		/// <param name="tmaxDesignation">the designation to be merged</param>
		///	<param name="tmaxRecords">The collection in which to store any new records added to the database</param>
		///	<returns>True if the caller's designation is processed</returns>
		private bool Merge(CTmaxImportDesignation tmaxDesignation, CTmaxItems tmaxRecords)
		{
			bool bIgnoreLine1 = false;
			
			//	Has the user turned off merging?
			if(m_tmaxImportOptions == null) return false;
			if(m_tmaxImportOptions.MergeDesignations == TmaxDesignationMergeMethods.None) return false;
				
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
			if(m_tmaxImportOptions.MergeDesignations == TmaxDesignationMergeMethods.AdjacentPages)
			{
				//	This designation must start on line 1
				if(tmaxDesignation.StartLine != 1)
				{
					//	Are we ignoring the first line?
					if((tmaxDesignation.StartLine == 2) && (m_tmaxImportOptions.IgnoreFirstLine == true))
					{
						bIgnoreLine1 = true;
					}
					else
					{
						//	Can't merge this designation
						return false;
					}

				}// if(tmaxDesignation.StartLine != 1)

			}// if(m_tmaxImportOptions.MergeDesignations == TmaxDesignationMergeMethods.AdjacentPages)
			
			//	Does this designation start on the next line?
			if(bIgnoreLine1 == true)
			{
				//	Make it look like the designation starts on line one
				if((tmaxDesignation.StartPL - 1) != m_lastDesignation.GetNextPL())
				{
					return false;
				}
				else
				{
					m_lastDesignation.IgnoreLine(tmaxDesignation.StartPage, 1);
				}

			}
			else
			{
				if(tmaxDesignation.StartPL != m_lastDesignation.GetNextPL())
				{
					return false;
				}

			}// if(bIgnoreLine1 == true)
			
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
				ImportDesignation(m_lastDesignation, tmaxRecords);
				m_lastDesignation = null;
			}
			
			return true;
			
		}// private bool Merge(CTmaxImportDesignation tmaxDesignation, CTmaxItems tmaxRecords)
		
		/// <summary>This method is called to determine if the specified designation can be merged with the next one in the file</summary>
		/// <param name="tmaxDesignation">The designation being imported</param>
		/// <returns>true if it can be merged</returns>
		private bool CanMerge(CTmaxImportDesignation tmaxDesignation)
		{
			CDxTranscript dxTranscript = null;
			
			Debug.Assert(tmaxDesignation != null);
			if(tmaxDesignation == null) return false;
			
			//	Can't merge if the user has turned merging off
			if(m_tmaxImportOptions == null) return false;
			if(m_tmaxImportOptions.MergeDesignations == TmaxDesignationMergeMethods.None) return false;

			//	Are we only merging on page boundries?
			if(m_tmaxImportOptions.MergeDesignations == TmaxDesignationMergeMethods.AdjacentPages)
			{
				//	Make sure this designation ends on the last line of the page
				if(tmaxDesignation.StopLine != tmaxDesignation.GetLinesPerPage())
					return false;
			}
			
			//	Cant merge this designation with the next one if the end point has been tuned
			if(tmaxDesignation.StopTime >= 0) return false;
			
			//	Can't merge unless we have the transcript
			if(m_dxDeposition != null)
				dxTranscript = m_dxDeposition.GetTranscript();
			if(dxTranscript == null) return false;
			
			//	Can't merge if we're already on the last line if the transcript
			if(tmaxDesignation.StopPL == dxTranscript.LastPL)
				return false;

			return true; //	It's all good
			
		}// private bool CanMerge(CTmaxImportDesignation tmaxDesignation)

		/// <summary>This method will split the specified file path into a MediaID / Name pair</summary>
		/// <param name="strFileSpec">The file path used to create the MediaID and Name</param>
		/// <param name="rMediaId">The string in which to store the MediaId</param>
		/// <param name="rName">The string in which to store the Name</param>
		/// <returns>true if successful</returns>
		private bool SplitFileSpec(string strFileSpec, ref string rMediaId, ref string rName)
		{
			string	strFilename = "";
			int		iIndex = 0;
			bool	bSuccessful = false;
			
			try
			{
				//	Extract the filename from the path
				strFilename = System.IO.Path.GetFileNameWithoutExtension(strFileSpec);
						
				//	Split the filename if it contains a space
				iIndex = strFilename.IndexOf(' ');
				if((iIndex > 0) && (iIndex < (strFilename.Length - 1)))
				{
					rName = strFilename.Substring(iIndex + 1);
						
					//	Strip leading white space and hyphens
					while((rName[0] == ' ') || (rName[0] == '-'))
					{
						rName = rName.Substring(1);
						if(rName.Length == 0)
							break;
					}
						
					rMediaId = strFilename.Substring(0, iIndex);
				}
				else
				{
					rName = strFilename;
					rMediaId = strFilename;
				}
				
				//	Should we apply registration options?
				if(m_tmaxImportOptions.UseRegistrationOptions == true)
					rMediaId = m_tmaxDatabase.AdjustForMediaId(rMediaId, TmaxMediaTypes.Unknown);
				else
					rMediaId = m_tmaxDatabase.FormatMediaId(rMediaId, TmaxMediaTypes.Script);
					
				bSuccessful = true;
			
			}
			catch
			{
				AddMessage(ERROR_SPLIT_FILESPEC_EX, strFileSpec);
			}
						
			return bSuccessful;
			
		}// private bool SplitFileSpec(string strFileSpec, ref string rMediaId, ref string rName)
		
		/// <summary>This method will add a parent record to the database</summary>
		/// <param name="dxExisting">The matching record that exists in the database</param>
		/// <param name="strName">The name extracted from the import filename</param>
		/// <returns>The exchange interface of the new parent record</returns>
		private CDxPrimary Update(CDxPrimary dxExisting, string strName)
		{
			CDxTertiaries		dxDesignations = null;
			CDxTertiaries		dxImported = null;
			CTmaxItems			tmaxEntries = null;
			CDxPrimary			dxUpdated = null;
			string				strMediaId = "";
			
			try
			{
				//	What is the MediaId of the existing record?
				strMediaId = dxExisting.MediaId;
				
				//	Get the collection of designations in the existing script
				if(dxExisting.Secondaries.Count == 0)
					dxExisting.Fill();
				dxDesignations = GetDesignations(dxExisting.Secondaries);
					
				//	Get the collection of pending designations
				if((m_tmaxPending != null) && (m_tmaxPending.Count > 0))
					dxImported = GetDesignations(m_tmaxPending);
					
				//	Do we have any designations to be updated?
				if((dxDesignations != null) && (dxImported != null))
				{
					//	Update each of the imported designations
					foreach(CDxTertiary O in dxImported)
					{
						Update(O, dxDesignations);
					}
				
				}
					
				//	Get the binder entries that reference this script
				tmaxEntries = GetBinderEntries(dxExisting);
				
				//	Add the new script using a temporary media id
				dxUpdated = m_tmaxDatabase.AddPrimary(TmaxMediaTypes.Script, System.Guid.NewGuid().ToString(), strName, dxExisting.Description);
					
				//	Add binder references that point to the new script
				//
				//	NOTE:	It's critical that we do this before deleting the
				//			existing script so that we have the approriate insertion
				//			point identifiers for the new binder entries
				if((tmaxEntries != null) && (tmaxEntries.Count > 0))
					Update(tmaxEntries, dxUpdated);
			
				//	Rename or delete the existing to make room for the new record
				if(m_tmaxImportOptions.CreateBackup == true)
					CreateBackup(dxExisting);
				else
					Delete(dxExisting);

				//	Not set the appropriate media id
				dxUpdated.MediaId = strMediaId;
				m_tmaxDatabase.Primaries.UpdateMediaId(dxUpdated);
					
			}
			catch
			{
				AddMessage(ERROR_UPDATE_EX, dxExisting.GetBarcode(false));
			}
			finally
			{
				//	Clean up
				if(dxDesignations != null)
				{
					dxDesignations.Clear();
					dxDesignations = null;
				}
				if(dxImported != null)
				{
					dxImported.Clear();
					dxImported = null;
				}
				if(dxDesignations != null)
				{
					dxDesignations.Clear();
					dxDesignations = null;
				}
				if(tmaxEntries != null)
				{
					tmaxEntries.Clear();
					tmaxEntries = null;
				}
			
			}

			return dxUpdated;
		
		}// private CDxPrimary Update(CDxPrimary dxExisting, string strName)
		
		/// <summary>This method is called to update the imported designation with matching designation in the original collection</summary>
		/// <param name="dxImported">The designation imported from file</param>
		/// <param name="dxExisting">The collection of designations already stored in the database</param>
		/// <returns>true if successful</returns>
		private bool Update(CDxTertiary dxImported, CDxTertiaries dxExisting)
		{
			CXmlDesignation	xmlImported = null;
			CXmlDesignation	xmlMatch = null;
			CXmlTranscripts	xmlTranscripts = null;
			CXmlLink		xmlLink = null;
			CDxTertiary		dxMatch = null;
			CTmaxItem		tmaxItem = null;
			bool			bSuccessful = false;
			bool			bEdited = false;
			long			lStartPL = 0;
			long			lStopPL = 0;
			int				iIndex = 0;
						
			try
			{
				//	Anything to update ?
				if(dxImported == null) return false;
				if(dxExisting == null) return false;
				if(dxExisting.Count == 0) return false;
				
				//	Get the extents of the imported designation
				lStartPL = dxImported.StartPL;
				lStopPL  = dxImported.StopPL;
				
				//	Look for a designation with matching extents in the existing collection
				foreach(CDxTertiary O in dxExisting)
				{
					//	Do the starts match?
					if(lStartPL == O.StartPL)
					{
						//	Do the stop positions match?
						if(lStopPL == O.StopPL)
						{
							dxMatch = O;
							break;
						}
						else
						{
							//	Start position takes precidence over stop position
							if((dxMatch == null) || (dxMatch.StartPL != lStartPL))
								dxMatch = O;
						}
						
					}
					else if(lStopPL == O.StopPL)
					{
						//	Use this as the match until something better is found
						if(dxMatch == null)
							dxMatch = O;
					}
					
				}// foreach(CDxTertiary O in dxExisting)
				
				//	Nothing to do if no matching designation is found
				if(dxMatch == null) 
					return false;
				
				//	Get the XML designation for the import
				if((xmlImported = dxImported.GetXmlDesignation(true)) == null)
					return false;
					
				//	Update the matching positions
				if(dxMatch.StartPL == lStartPL)
				{
					dxImported.Start = dxMatch.Start;
					dxImported.StartTuned = dxMatch.StartTuned;
				}
				else
				{
					//	Clear tuning information that may have been read in from the file
					if(dxImported.StartTuned == true)
					{
						dxImported.StartTuned = false;
						if(xmlImported.Transcripts.Count > 0)
							dxImported.Start = xmlImported.Transcripts[0].Start;
					}
					
				}
				
				if(dxMatch.StopPL == lStopPL)
				{
					dxImported.Stop = dxMatch.Stop;
					dxImported.StopTuned = dxMatch.StopTuned;
				}
				else
				{
					//	Clear tuning information that may have been read in from the file
					if(dxImported.StopTuned == true)
					{
						dxImported.StopTuned = false;
						if(xmlImported.Transcripts.Count > 0)
							dxImported.Stop = xmlImported.Transcripts[xmlImported.Transcripts.Count - 1].Stop;
					}
					
				}
				
				//	Remove the match from the existing collection to speed up future searches
				//dxExisting.Remove(dxMatch);
				
				//	Update the XML extents and save the changes
				m_tmaxDatabase.Update(new CTmaxItem(dxImported), null, null);
				dxImported.SetAttributes(xmlImported);
				xmlImported.Save();
				
				//	Open the XML designation for the matching designation
				if((xmlMatch = dxMatch.GetXmlDesignation(false)) != null)
				{
					//	Create a temporary collection to speed up searching
					xmlTranscripts = new CXmlTranscripts();
					xmlTranscripts.KeepSorted = false;
					foreach(CXmlTranscript O in xmlImported.Transcripts)
						xmlTranscripts.Add(O);
					xmlTranscripts.Sort(true);
						
					//	Search for any text in the matching designation that's been edited
					foreach(CXmlTranscript O in xmlMatch.Transcripts)
					{
						if(O.Edited == true)
						{
							if((O.PL >= lStartPL) && (O.PL <= lStopPL))
							{
								//	Get the line in the imported collection
								if((iIndex = xmlTranscripts.Locate(O.PL)) >= 0)
								{
									xmlTranscripts[iIndex].Edited = true;
									xmlTranscripts[iIndex].Text = O.Text;
									xmlTranscripts.RemoveAt(iIndex);
									bEdited = true;
								}
								
							}// if((O.PL >= lStartPL) && (O.PL <= lStopPL))
						
						}// if(O.Edited == true)
						
					}// foreach(CXmlTranscript O in xmlMatch.Transcripts)
					
					//	Was there any edited text?
					if(bEdited == true)
						xmlImported.Save();
					
				}// if((xmlMatch = dxMatch.GetXmlDesignation(false)) == true)
				
				//	Transfer links to the imported designation
				if(dxMatch.ChildCount > 0)
				{
					if(dxMatch.Quaternaries.Count == 0)
						dxMatch.Fill();
						
					foreach(CDxQuaternary O in dxMatch.Quaternaries)
					{
						//	Make sure this link is within range
						if((O.StartPL < lStartPL) || (O.StartPL > lStopPL))
							continue; // Out of range
							
						//	Create an XML link
						xmlLink = new CXmlLink();
						O.SetAttributes(xmlLink);
						xmlLink.DatabaseId = "";
						
						//	Set up an event item to add the link to the imported designation
						tmaxItem = new CTmaxItem(dxImported);
						tmaxItem.XmlDesignation = xmlImported;
						if(tmaxItem.SourceItems == null) tmaxItem.SourceItems = new CTmaxItems();
						tmaxItem.SourceItems.Add(new CTmaxItem());
						tmaxItem.SourceItems[0].XmlLink = xmlLink;
						
						//	Add to the database
						if(m_tmaxDatabase.Add(tmaxItem, null, null) == true)
							xmlImported.Links.Add(xmlLink);
						
					}// foreach(CDxQuaternary O in dxMatch.Quaternaries)
					
					if(xmlImported.Links.Count > 0)
						xmlImported.Save();
					
				}// if(dxMatch.ChildCount > 0)
				
				bSuccessful = true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Update (designations)", Ex);
			}
						
			return bSuccessful;
			
		}// private bool Update(CDxTertiary dxImported, CDxTertiaries dxExisting)
		
		/// <summary>This method is called to make the specified record a backup</summary>
		/// <param name="dxOriginal">The record being backed up</param>
		/// <returns>true if successful</returns>
		private bool CreateBackup(CDxPrimary dxOriginal)
		{
			CDxPrimary	dxExisting = null;
			string		strMediaId = "";
			string		strOldId = "";
			bool		bSuccessful = false;
			
			try
			{
				//	Create the backup MediaId
				strOldId = dxOriginal.GetBarcode(false);
				strMediaId = (strOldId + "_IMPORT_BACKUP");
				
				//	Delete the old backup if it exists
				if((dxExisting = m_tmaxDatabase.Primaries.Find(strMediaId)) != null)
				{
					Delete(dxExisting);
				}
				
				//	Change the MediaID of the original record
				dxOriginal.MediaId = strMediaId;
				if(m_tmaxDatabase.Primaries.UpdateMediaId(dxOriginal) == 0)
				{
					if((m_tmaxResults != null) && (m_tmaxResults.Updated != null))
					{
						m_tmaxResults.Updated.Add(new CTmaxItem(dxOriginal));
					}
					
					bSuccessful = true;
				}
				else
				{
					AddMessage(ERROR_CREATE_BACKUP_FAILED, strOldId, strMediaId);
				}
			
			}
			catch
			{
				AddMessage(ERROR_CREATE_BACKUP_EX, strMediaId);
			}
						
			return bSuccessful;
			
		}// private bool CreateBackup(CDxPrimary dxOriginal)

		/// <summary>This method is called to update binder entries that are bound tothe script being deleted</summary>
		/// <param name="dxImported">The updated script that has been added to the database</param>
		/// <returns>true if successful</returns>
		private bool Update(CTmaxItems tmaxEntries, CDxPrimary dxImported)
		{
			bool					bSuccessful = false;
			CTmaxDatabaseResults	tmaxResults = null;
			CTmaxParameters			tmaxParameters = null;
			
			try
			{
				if(tmaxEntries == null) return true;
				if(tmaxEntries.Count == 0) return true;

				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.Before, true);
				
				tmaxResults = new CTmaxDatabaseResults();
				
				foreach(CTmaxItem O in tmaxEntries)
				{
					tmaxResults.Clear();
					
					if(O.SourceItems == null)
						O.SourceItems = new CTmaxItems();
					O.SourceItems.Add(new CTmaxItem(dxImported));
					
					//	Add a new binder entry to the database
					if(m_tmaxDatabase.Add(O, tmaxParameters, tmaxResults) == true)
					{
						if((m_tmaxResults != null) && (tmaxResults.Added.Count > 0))
						{
							foreach(CTmaxItem tmaxAdded in tmaxResults.Added)
							{
								m_tmaxResults.Added.Add(tmaxAdded);
							}
						}
						
					}// if(m_tmaxDatabase.Add(O, tmaxParameters, tmaxResults) == true)
					
				}// foreach(CTmaxItem O in tmaxEntries)
				
				bSuccessful = true;
			}
			catch
			{
				AddMessage(ERROR_UPDATE_BINDER_ENTRIES_EX, dxImported.MediaId);
			}
						
			return bSuccessful;
			
		}// private bool Update(CTmaxItems tmaxEntries, CDxPrimary dxImported)
		
		/// <summary>This method searchs the specified source collection for all designation records</summary>
		/// <param name="tmaxSource">The collection of source records</param>
		/// <returns>The collection of associated designations</returns>
		private CDxTertiaries GetDesignations(CTmaxItems tmaxSource)
		{
			CDxTertiaries	dxDesignations = null;
			CDxSecondary	dxScene = null;
			CDxMediaRecord		dxSource = null;
			
			try
			{
				dxDesignations = new CDxTertiaries(m_tmaxDatabase);
				
				foreach(CTmaxItem O in tmaxSource)
				{
					if((dxSource = (CDxMediaRecord)(O.GetMediaRecord())) != null)
					{
						//	What type of record?
						switch(dxSource.MediaType)
						{
							case TmaxMediaTypes.Designation:
							
								//	Add to the collection
								dxDesignations.AddList((CDxTertiary)dxSource);
								break;
								
							case TmaxMediaTypes.Scene:
							
								dxScene = (CDxSecondary)dxSource;
								
								//	Is this a designation scene?
								if((dxScene.GetSource() != null) && (dxScene.GetSource().MediaType == TmaxMediaTypes.Designation))
									dxDesignations.AddList((CDxTertiary)(dxScene.GetSource()));
									
								break;
						
						}// switch(O.MediaType)
					
					}// if((dxSource = O.GetMediaRecord()) != null)
				
				}// foreach(CTmaxItem O in tmaxSource)
				
			}
			catch
			{
			
			}
			
			if((dxDesignations != null) && (dxDesignations.Count > 0))
				return dxDesignations;
			else
				return null;
				
		}// private CDxTertiaries GetDesignations(CDxMediaRecords dxSource)
		
		/// <summary>This method searchs the specified source collection for all designation records</summary>
		/// <param name="dxSource">The collection of source records</param>
		/// <returns>The collection of associated designations</returns>
		private CDxTertiaries GetDesignations(CDxMediaRecords dxSource)
		{
			CDxTertiaries	dxDesignations = null;
			CDxSecondary	dxScene = null;
			
			try
			{
				dxDesignations = new CDxTertiaries(m_tmaxDatabase);
				
				foreach(CDxMediaRecord O in dxSource)
				{
					//	What type of record?
					switch(O.MediaType)
					{
						case TmaxMediaTypes.Designation:
							
							//	Add to the collection
							dxDesignations.AddList((CDxTertiary)O);
							break;
								
						case TmaxMediaTypes.Scene:
							
							dxScene = (CDxSecondary)O;
								
							//	Is this a designation scene?
							if((dxScene.GetSource() != null) && (dxScene.GetSource().MediaType == TmaxMediaTypes.Designation))
								dxDesignations.AddList((CDxTertiary)(dxScene.GetSource()));
									
							break;
						
					}// switch(O.MediaType)
				
				}// foreach(CDxMediaRecord O in dxSource)
				
			}
			catch
			{
			
			}
			
			if((dxDesignations != null) && (dxDesignations.Count > 0))
				return dxDesignations;
			else
				return null;
				
		}// private CDxTertiaries GetDesignations(CDxMediaRecords dxSource)
		
		/// <summary>This method is called to get a collection of items used to replace the binder entries that reference the specified script</summary>
		/// <param name="dxScript">The script to be referenced by the binder entries</param>
		/// <returns>A collection of event items that reference the specified script</returns>
		private CTmaxItems GetBinderEntries(CDxPrimary dxScript)
		{
			CDxBinderEntries	dxEntries = null;
			CDxBinderEntry		dxEntry = null;
			CDxBinderEntry		dxInsertAt = null;
			CDxBinderEntries	dxSiblings = null;
			CTmaxItems			tmaxEntries = null;
			CTmaxItem			tmaxEntry = null;
			CTmaxItem			tmaxInsertAt = null;
			
			try
			{
				tmaxEntries = new CTmaxItems();
				
				dxEntries = m_tmaxDatabase.GetBinderEntries(dxScript);
				
				if(dxEntries == null) return null;
				if(dxEntries.Count == 0) return null;

				foreach(CDxBinderEntry O in dxEntries)
				{
					//	Is this a script?
					//
					//	NOTE:	We ignore scenes because they will either
					//			get backed up or deleted
					if(O.GetSource(true) == null) continue;
					if(O.GetSource(true).MediaType != TmaxMediaTypes.Script) continue;
					
					//	Get the actual application exchange interface for the binder
					if((dxEntry = m_tmaxDatabase.GetBinderFromPath(O.ParentPathId + "." + O.AutoId.ToString())) == null) continue;
					
					tmaxEntry = new CTmaxItem();
					tmaxEntry.DataType = TmaxDataTypes.Binder;
					tmaxEntry.IBinderEntry = dxEntry.Parent;
					tmaxEntries.Add(tmaxEntry);

					if((dxSiblings = (CDxBinderEntries)(dxEntry.Collection)) != null)
					{
						if((dxEntry.DisplayOrder >= 1) && (dxEntry.DisplayOrder < dxSiblings.Count))
						{
							dxInsertAt = dxSiblings[(int)(dxEntry.DisplayOrder - 1)];
						}
					}
					
					if(dxInsertAt != null)
					{
						tmaxInsertAt = new CTmaxItem();
						tmaxInsertAt.DataType = TmaxDataTypes.Binder;
						tmaxInsertAt.IBinderEntry = dxInsertAt;
						tmaxEntry.SubItems.Add(tmaxInsertAt);
						dxInsertAt = null;
					}
					
				}// foreach(CDxBinderEntry O in dxEntries)
				
			}
			catch
			{
				AddMessage(ERROR_GET_BINDER_ENTRIES_EX, dxScript.MediaId);
			}
						
			if((tmaxEntries != null) && (tmaxEntries.Count > 0))
				return tmaxEntries;
			else
				return null;
			
		}// private CTmaxItems GetBinderEntries(CDxPrimary dxScript)
		
		/// <summary>This method is called to delete the specified record</summary>
		/// <param name="dxOriginal">The record being backed up</param>
		/// <returns>true if successful</returns>
		private bool Delete(CDxMediaRecord dxRecord)
		{
			CTmaxDatabaseResults	tmaxResults = null;
			CTmaxItem				tmaxItem = null;
			bool					bSuccessful = false;
			
			try
			{
				if(m_tmaxDatabase == null) return false;
				if(dxRecord == null) return false;
				
				//	Create an event item to make the request
				tmaxItem = new CTmaxItem(dxRecord.GetParent());
				tmaxItem.SubItems.Add(new CTmaxItem(dxRecord));
				
				tmaxResults = new CTmaxDatabaseResults();
				
				if(m_tmaxDatabase.Delete(tmaxItem, null, tmaxResults) == true)
				{
					bSuccessful = true;
					
					//	Copy the results to our master collection
					if(m_tmaxResults != null)
					{
						foreach(CTmaxItem O in tmaxResults.Deleted)
							m_tmaxResults.Deleted.Add(O);
					}
					
					tmaxResults.Clear();
					tmaxResults = null;
					
				}
			
			}
			catch(System.Exception)
			{
			//MessageBox.Show(Ex.ToString());
			}
						
			return bSuccessful;
			
		}// private bool Delete(CDxMediaRecord dxRecord)

		/// <summary>This method will replace the existing record with a new imported parent</summary>
		/// <param name="dxExisting">The matching record that exists in the database</param>
		/// <param name="dxImported">The name extracted from the import filename</param>
		/// <returns>True if successful</returns>
		private bool Replace(CDxPrimary dxExisting, CDxPrimary dxImported)
		{
			CDxBinderEntries	dxEntries = null;
			CDxBinderEntry		dxBinder = null;
			CDxSecondary		dxScene = null;
			CDxMediaRecord		dxSource = null;
			bool				bSuccessful = true;
			string				strMediaId = "";

			try
			{
				//	What is the MediaId of the existing record?
				strMediaId = dxExisting.MediaId;
				
				//	Get the binder entries that reference this script
				dxEntries = m_tmaxDatabase.GetBinderEntries(dxExisting);
				
				if((dxEntries != null) && (dxEntries.Count > 0))
				{
					foreach(CDxBinderEntry O in dxEntries)
					{
						//	Get the source record for this binder entry
						if((dxSource = O.GetSource(true)) == null) continue;
						
						//	Does this entry point to the primary script?
						if(dxSource.MediaType == TmaxMediaTypes.Script)
						{
							//	All we have to do is update it to point to the new script
							if((dxBinder = m_tmaxDatabase.SetBinderSource(O, dxImported)) != null)
							{
								if(m_tmaxResults != null)
									m_tmaxResults.Updated.Add(new CTmaxItem(dxBinder));
							}
							
						}
						//	It must point to one of the child scenes
						else if(dxSource.MediaType == TmaxMediaTypes.Scene)
						{
							//	Make sure we have the scenes for the imported record
							if(dxImported.Secondaries.Count == 0)
								dxImported.Fill();
								
							//	Find the imported scene with a matching barcode identifier
							if((dxScene = dxImported.Secondaries.Find(dxSource.BarcodeId, true)) != null)
							{
								//	Update this entry to point to the imported scene
								if((dxBinder = m_tmaxDatabase.SetBinderSource(O, dxScene)) != null)
								{
									if(m_tmaxResults != null)
										m_tmaxResults.Updated.Add(new CTmaxItem(dxBinder));
								}
							
							}
							else
							{
								//	Have to delete this scene from the binder
								
								AddMessage(ERROR_XML_SCENE_NOT_FOUND, dxSource.GetBarcode(true), m_tmaxDatabase.ExpandBinderPath(O.ParentPathId), TmaxMessageLevels.Information);
							}
						
						}//	else if(O.GetSource(true).MediaType == TmaxMediaTypes.Scene)
					
					}// foreach(CDxBinderEntry O in dxEntries)
					
				}// if((dxEntries != null) && (dxEntries.Count > 0))
				
				//	Rename or delete the existing to make room for the new record
				if(m_tmaxImportOptions.CreateBackup == true)
					CreateBackup(dxExisting);
				else
					Delete(dxExisting);
					
				//	Not set the appropriate media id
				dxImported.MediaId = strMediaId;
				m_tmaxDatabase.Primaries.UpdateMediaId(dxImported);
					
			}
			catch
			{
				AddMessage(ERROR_REPLACE_EX, dxExisting.GetBarcode(false));
			}
				
			return bSuccessful;
		
		}// private CDxPrimary Replace(CDxPrimary dxExisting, string strName)
		
		/// <summary>Called to adjust the barcode retrieved from file if required</summary>
		/// <param name="strBarcode">The barcode retrieved from the file</param>
		/// <returns>The adjusted barcode</returns>
		private string AdjustBarcode(string strBarcode)
		{
			string	strAdjusted = "";
			string	strMediaId = "";
			string	strSTQ = "";
			int		iStrip = -1;
			
			//	Strip bounding quotes if they exist
			strAdjusted = CTmaxToolbox.StripQuotes(strBarcode, true);
				
			//	Should we apply the registration options to the barcode in the file?
			if(m_tmaxImportOptions.UseRegistrationOptions == true)
			{
				//	Separate the MediaID from the STQ portion of the barcode
				if((iStrip = strAdjusted.IndexOf(".")) >= 0)
				{
					strMediaId = strAdjusted.Substring(0, iStrip);
					strSTQ = strAdjusted.Substring(iStrip);
				}
				else
				{
					//	No SQT identifiers attached
					strMediaId = strAdjusted;
				}

				//	Apply the registration options
				strAdjusted = m_tmaxDatabase.AdjustForMediaId(strMediaId, TmaxMediaTypes.Unknown);
				
				//	Put the Secondary.Tertiary.Quaternary identifiers
				if(strSTQ.Length > 0)
					strAdjusted += strSTQ;
			
			}// if(m_tmaxImportOptions.UseRegistrationOptions == true)
			
			return strAdjusted;
			
		}// private string AdjustBarcode(string strBarcode)
		
		/// <summary>Called to determine if the specified fields are to be used to import a recording clip</summary>
		/// <param name="aFields">The fields retrieved from the input file</param>
		/// <returns>The parent recording segment record</returns>
		private CDxSecondary IsAsciiClip(string [] aFields)
		{
			string			strBarcode = "";
			CDxMediaRecord	dxRecord = null;

            //	Should be 5 (more if user put unused stuff) fields
            if(aFields.GetUpperBound(0) < 4) return null;
            if(aFields[4].Length == 0) return null;

            //	The fifth field should be the barcode for the parent recording
            strBarcode = AdjustBarcode(aFields[4]);
            if(strBarcode.Length == 0) return null;
            
            //	Get the record identified by this barcode
			if((dxRecord = m_tmaxDatabase.GetRecordFromBarcode(strBarcode, true, true)) != null)
			{
				//	Is this record a segment?
				if(dxRecord.MediaType == TmaxMediaTypes.Segment)
				{
					//	Is the parent a recording?
					if((dxRecord.GetParent() != null) && (dxRecord.GetParent().MediaType == TmaxMediaTypes.Recording))
						return (CDxSecondary)dxRecord;
				}
				//	Is it a recording?
				else if(dxRecord.MediaType == TmaxMediaTypes.Recording)
				{
					//	Fill the secondaries collection
					if(((CDxPrimary)dxRecord).Secondaries.Count == 0)
						((CDxPrimary)dxRecord).Fill();
						
					if(((CDxPrimary)dxRecord).Secondaries.Count > 0)
						return ((CDxPrimary)dxRecord).Secondaries[0];					

				}// else if(dxRecord.MediaType == TmaxMediaTypes.Recording)
			
			}// if((dxRecord = m_tmaxDatabase.GetRecordFromBarcode(strBarcode, true, true)) != null)
					
			return null;
		
		}// private CDxSecondary IsAsciiClip(string strField)
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}
		
		/// <summary>The active database</summary>
		public CTmaxCaseDatabase Database
		{
			get { return m_tmaxDatabase; }
			set { m_tmaxDatabase = value; }
		}

		/// <summary>The active set of import options</summary>
		public CTmaxImportOptions ImportOptions
		{
			get { return m_tmaxImportOptions; }
			set { m_tmaxImportOptions = value; }
		}

		/// <summary>Enumerated import format identifier</summary>
		public FTI.Shared.Trialmax.TmaxImportFormats Format
		{
			get { return m_eFormat; }
			set { m_eFormat = value; }
		}

		/// <summary>Form displayed during the import operation</summary>
		public FTI.Trialmax.Forms.CFImportStatus StatusForm
		{
			get { return m_wndStatusForm; }
		}

		/// <summary>Array of source files to be imported</summary>
		public string [] SourceFiles
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
                    WindowStatusRefresh();
				}
			}
		}

        delegate void WindowStatusRefreshCallback();
        private void WindowStatusRefresh()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.m_wndStatusForm.InvokeRequired)
            {
                WindowStatusRefreshCallback d = new WindowStatusRefreshCallback(WindowStatusRefresh);
                this.m_wndStatusForm.Invoke(d, new object[] { });
                
            }
            else
            {
                m_wndStatusForm.Refresh();
            }
        }


		/// <summary>The record exchange interface for the active deposition</summary>
		public CDxPrimary Deposition
		{
			get { return m_dxDeposition; }
			set { m_dxDeposition = value; }
		}

		/// <summary>The record exchange interface for the active highlighter</summary>
		public CDxHighlighter Highlighter
		{
			get { return m_dxHighlighter; }
			set { m_dxHighlighter = value; }
		}

		/// <summary>The active XML deposition</summary>
		public FTI.Shared.Xml.CXmlDeposition XmlDeposition
		{
			get { return m_xmlDeposition; }
			set { m_xmlDeposition = value; }
		}

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
                    WindowStatusRefresh();
				}
			}
		
		}

		/// <summary>Record exchange interface to owner of the records being imported</summary>
		public CBaseRecord Target
		{
			get { return m_dxTarget; }
			set { m_dxTarget = value; }
		}

		/// <summary>Record exchange interface to insertion point for new records</summary>
		public CBaseRecord InsertAt
		{
			get { return m_dxInsertAt; }
			set { m_dxInsertAt = value; }
		}

		/// <summary>True to merge all source files into one target</summary>
		public bool MergeSource
		{
			get { return m_bMergeSource; }
			set { m_bMergeSource = value; }
		}

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

		/// <summary>True to request insertion of new records before the specified insertion point</summary>
		public bool InsertBefore
		{
			get { return m_bInsertBefore; }
			set { m_bInsertBefore = value; }
		}

		#endregion Properties	
	
	}// class CTmaxImportManager
	
}// namespace FTI.Trialmax.Database

