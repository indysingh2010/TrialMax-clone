using System;
using System.Collections;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Configuration;
using System.Text;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class manages the global options used by the TmaxManager application</summary>
	public class CTmaxManagerOptions
	{
		#region Constants
		
		private const string XMLINI_SECTION_NAME				  = "ApplicationOptions";
		private const string XMLINI_SHOW_ERROR_MESSAGES_KEY		  = "ShowErrorMessages";
		private const string XMLINI_ENABLE_DIAGNOSTICS_KEY		  = "EnableDiagnostics";
		private const string XMLINI_LOG_DIAGNOSTICS_KEY			  = "LogDiagnostics";
		private const string XMLINI_LOAD_LAST_CASE_KEY			  = "LoadLastCase";
		private const string XMLINI_SHOW_FOREIGN_BARCODES_KEY	  = "ShowForeignBarcodes";
		private const string XMLINI_LASTLAYOUT_KEY				  = "LastLayout";
		private const string XMLINI_PRIMARY_TEXT_MODE_KEY		  = "PrimaryTextMode";
		private const string XMLINI_SECONDARY_TEXT_MODE_KEY		  = "SecondaryTextMode";
		private const string XMLINI_TERTIARY_TEXT_MODE_KEY		  = "TertiaryTextMode";
		private const string XMLINI_QUATERNARY_TEXT_MODE_KEY	  = "QuaternaryTextMode";
		private const string XMLINI_MAX_PRIMARIES_KEY			  = "MaxPrimaries";
        //private const string XMLINI_DATE_INSTALLED                = "DateInstalled";
        //private const string XMLINI_DAYS_ALLOWED                  = "DaysAllowed";
		private const string XMLINI_USERS_MANUAL_KEY			  = "UsersManual";
		private const string XMLINI_WARN_BINDER_DUPLICATES_KEY	  = "WarnBinderDuplicates";
		private const string XMLINI_CONFIRM_DELETE_REFERENCES_KEY = "ConfirmDeleteReferences";
		private const string XMLINI_FILTER_ON_OPEN_KEY            = "FilterOnOpen";
		private const string XMLINI_ENABLE_DIB_PRINTING_KEY		  = "DIBPrinting";
        private const string XMLINI_SHOW_AUDIO_WAVEFORM_KEY       = "ShowAudioWaveform";
		
		#endregion Constants
		
		#region Private Members

		/// <summary>Local member bound to AppMainForm property</summary>
		private System.Windows.Forms.Form m_appMainForm = null;
		
		/// <summary>Local member bound to AppMainWnd property</summary>
		private System.IntPtr m_hAppMainWnd = System.IntPtr.Zero;
		
		/// <summary>Local member bound to ShowErrorMessages property</summary>
		private bool m_bShowErrorMessages = true; 
		
		/// <summary>Local member bound to ShowForeignBarcodes property</summary>
		private bool m_bShowForeignBarcodes = false; 
		
		/// <summary>Local member bound to EnableDiagnostics property</summary>
		private bool m_bEnableDiagnostics = false; 
		
		/// <summary>Local member bound to DisableTmaxKeyboard property</summary>
		private bool m_bDisableTmaxKeyboard = false; 
		
		/// <summary>Local member bound to LogDiagnostics property</summary>
		private bool m_bLogDiagnostics = false; 
		
		/// <summary>Local member bound to LoadLastCase property</summary>
		private bool m_bLoadLastCase = false; 
		
		/// <summary>Local member bound to FilterOnOpen property</summary>
		private bool m_bFilterOnOpen = false; 
		
		/// <summary>Local member bound to WarnBinderDuplicates property</summary>
		private bool m_bWarnBinderDuplicates = true;

		/// <summary>Local member bound to ConfirmDeleteReferences property</summary>
		private bool m_bConfirmDeleteReferences = false;

		/// <summary>Local member bound to EnableDIBPrinting property</summary>
		private bool m_bEnableDIBPrinting = true;

        /// <summary>Local member bound to ShowAudioWaveform property</summary>
        private bool m_bshowAudioWaveform = false;

		/// <summary>Local member bound to RecentlyUsed property</summary>
		private ArrayList m_aRecentlyUsed = new ArrayList();
		
		/// <summary>Local member bound to MaxRecentlyUsed property</summary>
		private int m_iMaxRecentlyUsed = 5;
		
		/// <summary>Local member bound to MaxPrimaries property</summary>
		private long m_lMaxPrimaries = 50;

        ///// <summary>Local member bound to DaysAllowed property</summary>
        //private int m_intDaysAllowed = 90;
		
		/// <summary>Local member bound to LastLayout property</summary>
		private string m_strLastLayout = "";
		
		/// <summary>Local member bound to UsersManual property</summary>
		private string m_strUsersManual = "TrialMax Manual.pdf";
		
		/// <summary>Local member bound to ApplicationFolder property</summary>
		private string m_strApplicationFolder = "";
		
		/// <summary>Local member bound to PrimaryTextMode property</summary>
		protected FTI.Shared.Trialmax.TmaxTextModes m_ePrimaryTextMode = TmaxTextModes.MediaId;

		/// <summary>Local member bound to SecondaryTextMode property</summary>
		protected FTI.Shared.Trialmax.TmaxTextModes m_eSecondaryTextMode = TmaxTextModes.Barcode;

		/// <summary>Local member bound to TertiaryTextMode property</summary>
		protected FTI.Shared.Trialmax.TmaxTextModes m_eTertiaryTextMode = TmaxTextModes.Barcode;

		/// <summary>Local member bound to QuaternaryTextMode property</summary>
		protected FTI.Shared.Trialmax.TmaxTextModes m_eQuaternaryTextMode = TmaxTextModes.Barcode;

		/// <summary>Local member bound to PrintOptions property</summary>
		protected FTI.Shared.Trialmax.CTmaxPrintOptions m_tmaxPrintOptions = new CTmaxPrintOptions();
		
		/// <summary>Local member bound to SearchOptions property</summary>
		protected FTI.Shared.Trialmax.CTmaxSearchOptions m_tmaxSearchOptions = new CTmaxSearchOptions();
		
		/// <summary>Local member bound to CleanOptions property</summary>
		protected FTI.Shared.Trialmax.CTmaxCleanOptions m_tmaxCleanOptions = new CTmaxCleanOptions();
		
		/// <summary>Local member bound to ImportWizard property</summary>
		protected FTI.Shared.Trialmax.CTmaxImportWizard m_tmaxImportWizard = new CTmaxImportWizard();
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxManagerOptions()
		{
		
		}// CTmaxManagerOptions()
		
		/// <summary>This method is called to add a case folder to the collection of recently used folders</summary>
		/// <param name="strFolder">The folder to be added to the local collection</param>
		/// <returns>true if successful</returns>
		public bool AddRecentlyUsed(string strFolder)
		{
			string strRecent = null;
			
			try
			{
				//	Trim the trailing backslash
				if(strFolder.EndsWith("\\"))
					strFolder = strFolder.Substring(0, strFolder.Length - 1);
					
				//	See if this folder already exists in the collection
				for(int i = 0; i < m_aRecentlyUsed.Count; i++)
				{
					if(String.Compare(m_aRecentlyUsed[i].ToString(), strFolder, true) == 0)
					{
						//	Is it already at the top of the list?
						if(i == 0)
						{
							//	Nothing to do
							return true;
						}
						else
						{
							//	Remove from the collection 
							m_aRecentlyUsed.RemoveAt(i);
						}
					}
					
				}
				
				//	Do we need to make room?
				while(m_aRecentlyUsed.Count > m_iMaxRecentlyUsed)
					m_aRecentlyUsed.RemoveAt(m_aRecentlyUsed.Count - 1);
					
				strRecent = new string(strFolder.ToCharArray());
				
				//	Insert at the top of the collection
				if(m_aRecentlyUsed.Count > 0)
					m_aRecentlyUsed.Insert(0, strRecent);
				else
					m_aRecentlyUsed.Add(strRecent);
					
				return true;
			
			}
			catch
			{
				return false;
			}
		
		}// public bool AddRecentlyUsed(string strFolder)
		
		/// <summary>This method is called to remove a case folder from the collection of recently used folders</summary>
		/// <param name="strFolder">The folder to be removed from the local collection</param>
		public void RemoveRecentlyUsed(string strFolder)
		{
			try
			{
				//	Trim the trailing backslash
				if(strFolder.EndsWith("\\"))
					strFolder = strFolder.Substring(0, strFolder.Length - 1);
					
				//	See if this folder exists in the collection
				for(int i = 0; i < m_aRecentlyUsed.Count; i++)
				{
					if(String.Compare(m_aRecentlyUsed[i].ToString(), strFolder, true) == 0)
					{
						//	Remove from the collection
						m_aRecentlyUsed.RemoveAt(i);
						break;
					}
					
				}
				
			}
			catch
			{
			}
		
		}// public void RemoveRecentlyUsed(string strFolder)
		
		/// <summary>This method is called to load the application options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the application option values</param>
        public void Load(CXmlIni xmlIni)
		{
			string strKey;
			string strRecent;
            //string strdateInstalledKey="";
			
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;
			
			m_strLastLayout = xmlIni.Read(XMLINI_LASTLAYOUT_KEY);
			m_strUsersManual = xmlIni.Read(XMLINI_USERS_MANUAL_KEY, m_strUsersManual);
			m_bShowErrorMessages = xmlIni.ReadBool(XMLINI_SHOW_ERROR_MESSAGES_KEY, m_bShowErrorMessages);
			m_bShowForeignBarcodes = xmlIni.ReadBool(XMLINI_SHOW_FOREIGN_BARCODES_KEY, m_bShowForeignBarcodes);
			m_bEnableDiagnostics = xmlIni.ReadBool(XMLINI_ENABLE_DIAGNOSTICS_KEY, m_bEnableDiagnostics);
			m_bLogDiagnostics = xmlIni.ReadBool(XMLINI_LOG_DIAGNOSTICS_KEY, m_bLogDiagnostics);
			m_bLoadLastCase = xmlIni.ReadBool(XMLINI_LOAD_LAST_CASE_KEY, m_bLoadLastCase);
			m_bFilterOnOpen = xmlIni.ReadBool(XMLINI_FILTER_ON_OPEN_KEY, m_bFilterOnOpen);
			m_bWarnBinderDuplicates = xmlIni.ReadBool(XMLINI_WARN_BINDER_DUPLICATES_KEY, m_bWarnBinderDuplicates);
			m_bConfirmDeleteReferences = xmlIni.ReadBool(XMLINI_CONFIRM_DELETE_REFERENCES_KEY, m_bConfirmDeleteReferences);
			m_bEnableDIBPrinting = xmlIni.ReadBool(XMLINI_ENABLE_DIB_PRINTING_KEY, m_bEnableDIBPrinting);
			m_ePrimaryTextMode = (TmaxTextModes)xmlIni.ReadEnum(XMLINI_PRIMARY_TEXT_MODE_KEY, m_ePrimaryTextMode);
			m_eSecondaryTextMode = (TmaxTextModes)xmlIni.ReadEnum(XMLINI_SECONDARY_TEXT_MODE_KEY, m_eSecondaryTextMode);
			m_eTertiaryTextMode = (TmaxTextModes)xmlIni.ReadEnum(XMLINI_TERTIARY_TEXT_MODE_KEY, m_eTertiaryTextMode);
			m_eQuaternaryTextMode = (TmaxTextModes)xmlIni.ReadEnum(XMLINI_QUATERNARY_TEXT_MODE_KEY, m_eQuaternaryTextMode);
			m_lMaxPrimaries = xmlIni.ReadLong(XMLINI_MAX_PRIMARIES_KEY, m_lMaxPrimaries);
            //strdateInstalledKey = xmlIni.Read(XMLINI_DATE_INSTALLED, strdateInstalledKey);
            //m_intDaysAllowed = xmlIni.ReadInteger(XMLINI_DAYS_ALLOWED, m_intDaysAllowed);
            //DateTime.TryParse(Decrypt(strdateInstalledKey, true), out m_dateInstalled);
            m_bshowAudioWaveform = xmlIni.ReadBool(XMLINI_SHOW_AUDIO_WAVEFORM_KEY, m_bshowAudioWaveform);
            

            //if (DateTime.Compare(m_dateInstalled, Convert.ToDateTime("01/01/1900 12:00:00")) == 0)
            //{
            //    m_dateInstalled = DateTime.Now;
            //}

			
			//	Retrieve the recently used case folders
			for(int i = 1; i <= m_iMaxRecentlyUsed; i++)
			{
				strKey = ("RecentlyUsed" + i.ToString());
				strRecent = xmlIni.Read(strKey);
				
				if(strRecent != null && (strRecent.Length > 0))
				{
					//	Make sure the folder still exists
					if(System.IO.Directory.Exists(strRecent))
						m_aRecentlyUsed.Add(new string(strRecent.ToCharArray()));
				}
			}
			
			//	Load the nested options
			m_tmaxPrintOptions.Load(xmlIni);
			m_tmaxSearchOptions.Load(xmlIni);
			m_tmaxCleanOptions.Load(xmlIni);
			m_tmaxImportWizard.Load(xmlIni);
			
		}// public void Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the application options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file to store the application option values</param>
		public void Save(CXmlIni xmlIni)
		{
			string strKey = "";
			
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;

			xmlIni.Write(XMLINI_LASTLAYOUT_KEY, m_strLastLayout);
			xmlIni.Write(XMLINI_USERS_MANUAL_KEY, m_strUsersManual);
			xmlIni.Write(XMLINI_SHOW_ERROR_MESSAGES_KEY, m_bShowErrorMessages);
			xmlIni.Write(XMLINI_SHOW_FOREIGN_BARCODES_KEY, m_bShowForeignBarcodes);
			xmlIni.Write(XMLINI_ENABLE_DIAGNOSTICS_KEY, m_bEnableDiagnostics);
			xmlIni.Write(XMLINI_LOG_DIAGNOSTICS_KEY, m_bLogDiagnostics);
			xmlIni.Write(XMLINI_LOAD_LAST_CASE_KEY, m_bLoadLastCase);
			xmlIni.Write(XMLINI_FILTER_ON_OPEN_KEY, m_bFilterOnOpen);
			xmlIni.Write(XMLINI_WARN_BINDER_DUPLICATES_KEY, m_bWarnBinderDuplicates);
			xmlIni.Write(XMLINI_CONFIRM_DELETE_REFERENCES_KEY, m_bConfirmDeleteReferences);
			xmlIni.Write(XMLINI_ENABLE_DIB_PRINTING_KEY, m_bEnableDIBPrinting);
			xmlIni.Write(XMLINI_PRIMARY_TEXT_MODE_KEY, m_ePrimaryTextMode);
			xmlIni.Write(XMLINI_SECONDARY_TEXT_MODE_KEY, m_eSecondaryTextMode);
			xmlIni.Write(XMLINI_TERTIARY_TEXT_MODE_KEY, m_eTertiaryTextMode);
			xmlIni.Write(XMLINI_QUATERNARY_TEXT_MODE_KEY, m_eQuaternaryTextMode);
            //xmlIni.Write(XMLINI_DATE_INSTALLED, Encrypt(m_dateInstalled.ToString(),true));
            //xmlIni.Write(XMLINI_DAYS_ALLOWED, m_intDaysAllowed);
            xmlIni.Write(XMLINI_SHOW_AUDIO_WAVEFORM_KEY, m_bshowAudioWaveform);
            

            

			//	Write the recently used case folders
			for(int i = 0; i < m_iMaxRecentlyUsed; i++)
			{
				strKey = ("RecentlyUsed" + (i + 1).ToString());
				
				if(i < m_aRecentlyUsed.Count)
					xmlIni.Write(strKey, m_aRecentlyUsed[i].ToString());
				else
					xmlIni.Write(strKey, "");
			}
		
			//	Save the nested options
			m_tmaxPrintOptions.Save(xmlIni);
			m_tmaxSearchOptions.Save(xmlIni);
			m_tmaxCleanOptions.Save(xmlIni);
			m_tmaxImportWizard.Save(xmlIni);
			
		}// public void Save(CXmlIni xmlIni)
		
		/// <summary>This method is called to get the fully qualified path to the application's users manual</summary>
		/// <returns>The path to the user's manual</returns>
		public string GetUsersManualFileSpec()
		{
			string strFileSpec = "";
			
			if(m_strUsersManual.Length > 0)
			{
				//	Is this a fully qualified path?
				if(strFileSpec.IndexOfAny("\\:/".ToCharArray()) >= 0)
				{
					strFileSpec = m_strUsersManual;
				}
				else
				{
					strFileSpec = this.ApplicationFolder;
					if(strFileSpec.EndsWith("\\") == false)
						strFileSpec += "\\";
					strFileSpec += m_strUsersManual;
				}
				
			}// if(m_strUsersManual.Length > 0)
			
			return strFileSpec;
			
		}// public string GetUsersManualFileSpec()
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Collection of options used for printing</summary>
		public CTmaxPrintOptions PrintOptions
		{
			get { return m_tmaxPrintOptions; }
		}
		
		/// <summary>Collection of options used for searching</summary>
		public CTmaxSearchOptions SearchOptions
		{
			get { return m_tmaxSearchOptions; }
		}
		
		/// <summary>Collection of options used for cleaning scanned documents</summary>
		public CTmaxCleanOptions CleanOptions
		{
			get { return m_tmaxCleanOptions; }
		}
		
		/// <summary>Import Wizard options</summary>
		public CTmaxImportWizard ImportWizard
		{
			get { return m_tmaxImportWizard; }
		}

		/// <summary>List of most recently used case folders</summary>
		public ArrayList RecentlyUsed
		{
			get { return m_aRecentlyUsed; }
		
		}

        ///// <summary>Date when application was Installed</summary>
        //public DateTime DateInstalled
        //{
        //    get { return m_dateInstalled; }

        //}

        ///// <summary>Days Allowed after trial installed</summary>
        //public int DaysAllowed
        //{
        //    get { return m_intDaysAllowed; }

        //}
		
		/// <summary>Path to the folder containing the application executable</summary>
		public string ApplicationFolder
		{
			get { return m_strApplicationFolder; }
			set { m_strApplicationFolder = value; }
		}
		
		/// <summary>Handle to application's main window</summary>
		public System.IntPtr AppMainWnd
		{
			get { return m_hAppMainWnd; }
			set { m_hAppMainWnd = value; }
		}

		/// <summary>Reference to the application's main form</summary>
		public System.Windows.Forms.Form AppMainForm
		{
			get { return m_appMainForm; }
			set { m_appMainForm = value; }
		}

		/// <summary>Flag to disable the application's keyboard hook</summary>
		public bool DisableTmaxKeyboard
		{
			get { return m_bDisableTmaxKeyboard; }
			set { m_bDisableTmaxKeyboard = value; }
		}

		/// <summary>User's manual filename</summary>
		public string UsersManual
		{
			get { return m_strUsersManual; }
			set { m_strUsersManual = value; }
		}
		
		/// <summary>Name of the last used screen layout file</summary>
		public string LastLayout
		{
			get { return m_strLastLayout; }
			set { m_strLastLayout = value; }
		}
		
		/// <summary>Maximum number of recently used selections</summary>
		public int MaxRecentlyUsed
		{
			get 
			{ 
				return m_iMaxRecentlyUsed; 
			}
			set 
			{ 
				m_iMaxRecentlyUsed = value; 
				
				//	Do we need to remove folders from the collection
				if(m_aRecentlyUsed != null)
				{
					while(m_aRecentlyUsed.Count > m_iMaxRecentlyUsed)
						m_aRecentlyUsed.RemoveAt(m_aRecentlyUsed.Count - 1);
				}
				
			}
			
		}// MaxRecentlyUsed Property
		
		/// <summary>Text display mode for primary records</summary>
		public TmaxTextModes PrimaryTextMode
		{
			get { return m_ePrimaryTextMode; }
			set { m_ePrimaryTextMode = value; }
		}
		
		/// <summary>Text display mode for secondary records</summary>
		public TmaxTextModes SecondaryTextMode
		{
			get { return m_eSecondaryTextMode; }
			set { m_eSecondaryTextMode = value; }
		}
		
		/// <summary>Text display mode for tertiary records</summary>
		public TmaxTextModes TertiaryTextMode
		{
			get { return m_eTertiaryTextMode; }
			set { m_eTertiaryTextMode = value; }
		}
		
		/// <summary>Text display mode for quaternary records</summary>
		public TmaxTextModes QuaternaryTextMode
		{
			get { return m_eQuaternaryTextMode; }
			set { m_eQuaternaryTextMode = value; }
		}
		
		/// <summary>True to load last case on startup</summary>
		public bool LoadLastCase
		{
			get { return m_bLoadLastCase; }
			set { m_bLoadLastCase = value; }
		}

		/// <summary>True to show error messages</summary>
		public bool ShowErrorMessages
		{
			get { return m_bShowErrorMessages; }
			set { m_bShowErrorMessages = value; }
		}

        /// <summary>True to show Audio Waveform</summary>
        public bool ShowAudioWaveform
        {
            get { return m_bshowAudioWaveform; }
            set { m_bshowAudioWaveform = value; }
        }

		/// <summary>True to display foreign barcodes</summary>
		public bool ShowForeignBarcodes
		{
			get { return m_bShowForeignBarcodes; }
			set { m_bShowForeignBarcodes = value; }
		}

		/// <summary>True to enable diagnostic messages</summary>
		public bool EnableDiagnostics
		{
			get { return m_bEnableDiagnostics; }
			set { m_bEnableDiagnostics = value; }
		}

		/// <summary>True to log diagnostic messages</summary>
		public bool LogDiagnostics
		{
			get { return m_bLogDiagnostics; }
			set { m_bLogDiagnostics = value; }
		}

		/// <summary>True to display warning for duplicate binder entries</summary>
		public bool WarnBinderDuplicates
		{
			get { return m_bWarnBinderDuplicates; }
			set { m_bWarnBinderDuplicates = value; }
		}

		/// <summary>True to prompt for confirmation before deleting media referenced by scripts and/or binders</summary>
		public bool ConfirmDeleteReferences
		{
			get { return m_bConfirmDeleteReferences; }
			set { m_bConfirmDeleteReferences = value; }
		}

		/// <summary>True to load filtered tree on case open</summary>
		public bool FilterOnOpen
		{
			get { return m_bFilterOnOpen; }
			set { m_bFilterOnOpen = value; }
		}

		/// <summary>True to enable DIB scaling for print jobs (preferred)</summary>
		public bool EnableDIBPrinting
		{
			get { return m_bEnableDIBPrinting; }
			set { m_bEnableDIBPrinting = value; }
		}

		/// <summary>Maximum number of primaries allowed if the product is not activated</summary>
		public long MaxPrimaries
		{
			get { return m_lMaxPrimaries; }
			set { m_lMaxPrimaries = value; }
		}

		#endregion Properties
 

	}//	CTmaxManagerOptions
   

}// namespace FTI.Shared.Trialmax
