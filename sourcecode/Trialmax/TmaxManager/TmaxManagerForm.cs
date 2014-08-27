using System;
using System.Drawing;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Reflection;

using Infragistics.Win;
using Infragistics.Win.UltraWinDock;
using Infragistics.Win.UltraWinToolbars;

using FTI.Shared;
using FTI.Shared.Win32;
using FTI.Shared.Xml;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Panes;
using FTI.Trialmax.Forms;
using FTI.Trialmax.Controls;
using FTI.Trialmax.Database;
using FTI.Trialmax.ActiveX;
using FTI.Trialmax.Reports;
using FTI.Trialmax.MSOffice;
using FTI.Trialmax.MSOffice.MSPowerPoint;
using FTI.Trialmax.Encode;

namespace FTI.Trialmax.TmaxManager
{
	/// <summary>This is the TrialMax application's main form class</summary>
	public class CTmaxManagerForm : System.Windows.Forms.Form
	{
		#region Constants
		
		const string DEFAULT_APP_CONFIGURATION_FILE		= "TmaxManager.xml";
		const string DEFAULT_PRESENTATION_INI_FILENAME	= "fti.ini";
		const string DEFAULT_LAYOUT_FILENAME			= "tmaxpanes.tpd";
		const string DEFAULT_UPDATE_INSTALLER_FILENAME	= "TmaxInstaller.exe";
		const string DEFAULT_UPDATES_SUBFOLDER			= "_tmax_updates\\";
		const string DEFAULT_CONTACT_FTI_FILENAME		= "_tmax_contact_information.rtf";
		const string DEFAULT_VERSIONS_HISTORY_FILE		= "_tmax_versions.htm";
		
		const int ERROR_INITIALIZE_LAYOUT_EX			= 0;
		const int ERROR_INITIALIZE_PANES_EX				= 1;
		const int ERROR_INITIALIZE_TOOLBAR_EX			= 2;
		const int ERROR_LOAD_LAYOUT_EX					= 3;
		const int ERROR_SAVE_LAYOUT_EX					= 4;
		const int ERROR_SET_TOOL_STATE_EX				= 5;
		const int ERROR_SET_PANE_VISIBLE_EX				= 6;
		const int ERROR_LOCATE_PANE						= 7;
		const int ERROR_TOGGLE_PANE_EX					= 8;
		const int ERROR_ADD_PANE_EX						= 9;
		const int ERROR_EDIT_EX							= 10;
		const int ERROR_EDIT_FILE_NOT_FOUND				= 11;
		const int ERROR_ON_CMD_ACTIVATE_EX				= 12;
		const int ERROR_ON_CMD_SET_SEARCH_RESULT_EX		= 13;
		const int ERROR_ON_PRESENTATION_OPTIONS_EX		= 14;
		const int ERROR_ON_MANAGER_OPTIONS_EX			= 15;
		const int ERROR_INITIALIZE_VERSIONS_EX			= 16;
		const int ERROR_ON_PRESENTATION_TOOLBARS_EX		= 17;
		const int ERROR_LAUNCH_INSTALLER_EX				= 18;
		const int ERROR_ON_MM_CHECK_FOR_UPDATES_EX		= 19;
		const int ERROR_UPDATE_INSTALLER_EX				= 20;
		const int ERROR_CONTACT_FTI_EX					= 21;
		const int ERROR_IMPORT_LOAD_FILE_EX				= 22;
		const int ERROR_INITIALIZE_PRODUCT_MANAGER_EX	= 23;
		const int ERROR_INITIALIZE_SCREEN_CAPTURE_EX	= 24;
		const int ERROR_INITIALIZE_SCREEN_CAPTURE_FAIL	= 25;
		const int ERROR_ON_CMD_SET_DEPOSITION_EX		= 26;
		const int ERROR_ON_APP_TRIM_DATABASE_EX			= 27;
		const int ERROR_ON_APP_COMPACT_DATABASE_EX		= 28;
		
		const int WM_LOAD_BARCODE = (FTI.Shared.Win32.User.WM_USER + 0x05);
		
		/// <summary>Application menu command identifiers</summary>
		private enum AppCommands
		{
			Invalid = 0,
			NewCase,
			OpenCase,
			CloseCase,
			Exit, 
			LoadLayout,
			SaveLayout,
			Recent1,
			Recent2,
			Recent3,
			Recent4,
			Recent5,
			ManagerOptions,
			CaseOptions,
			PresentationOptions,
			PresentationToolbars,
			ToggleSourceExplorer,    
			ToggleMediaTree,    
			ToggleBinders,    
			ToggleMediaViewer,    
			ToggleProperties,   
			ToggleScriptBuilder,   
			ToggleTranscripts, 
			ToggleTuner,  
			ToggleCodes,
			ToggleObjections,
			ToggleObjectionProperties,
			ToggleScriptReview,
			ToggleSearchResults, 
			ToggleErrorMessages,    
			ToggleVersions,    
			ToggleDiagnostics,
			ToggleFilteredTree, 
			ShowAll,   
			ReloadCase,
			LockPanes,
			RefreshTreatments,
			ValidateDatabase,
			OpenPresentation,
			HelpContents,
			ContactFTI,
			UsersManual,
			AboutBox,   
			ActivateProduct, 
			OnlineSite,  
			CheckForUpdates,   
			Print,
			Find,
			FindNext,
			ImportAsciiBinder,
			ImportXmlBinder,
			ImportAsciiScript,
			ImportXmlScript,
			ImportBarcodeMap,
			ImportLoadFile,
			ImportCodes,
			ImportCodesDatabase,
			ImportXmlCaseCodes,
			ImportAsciiObjections,
			ExportBarcodeMap,
			ExportXmlCaseCodes,
			ExportAsciiObjections,
			SetFilter,
			FastFilter,
			ScreenCapture,
			BulkUpdate,
			TrimDatabase,
			CompactDatabase,
			Debug1,
			Debug2,
		}
		
		#endregion Constants

		#region Private Members
		
		/// <summary>Components collection required by forms designer</summary>
		private System.ComponentModel.IContainer components;
		
		/// <summary>Application splash screen</summary>
		private FTI.Trialmax.Forms.CFSplashScreen m_ctrlSplashScreen = null;

		/// <summary>Presentation options form</summary>
		FTI.Trialmax.Forms.CFPresentationOptions m_ctrlPresentationOptions = null;

		/// <summary>Image list used for toolbar buttons</summary>
		private System.Windows.Forms.ImageList m_ctrlSmallImages;

		/// <summary>Infragistics window docking manager control</summary>
		private UltraDockManager m_ctrlUltraDockManager;
		
		/// <summary>Infragistics toolbar/menu manager control</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager m_ctrlUltraToolbarManager;
		
		/// <summary>Trialmax database</summary>
		private CTmaxCaseDatabase m_tmaxDatabase = new CTmaxCaseDatabase();
		
		/// <summary>Source registration options</summary>
		private CTmaxRegOptions m_tmaxRegOptions = new CTmaxRegOptions();
		
		/// <summary>Application options</summary>
		private CTmaxManagerOptions m_tmaxAppOptions = new CTmaxManagerOptions();

		/// <summary>TmaxPresentation options</summary>
		private CTmaxPresentationOptions m_tmaxPresentationOptions = new CTmaxPresentationOptions();

		/// <summary>Application report manager</summary>
		private FTI.Trialmax.Reports.CTmaxReportManager m_tmaxReportManager = new CTmaxReportManager();
		
		/// <summary>Application clipboard</summary>
		private CTmaxItems m_tmaxClipboard = new CTmaxItems();
		
		/// <summary>Version descriptors displayed in the Versions pane</summary>
		private ArrayList m_aVersions = new ArrayList();
		
		/// <summary>Objects the support the ITmaxAppNotification interface</summary>
		private ArrayList m_aIAppNotifications = new ArrayList();
		
		/// <summary>Version descriptor used to display information for active database</summary>
		private FTI.Shared.CBaseVersion m_verDatabase = null;
		
		/// <summary>Application message filter used to trap keyboard messages</summary>
		private FTI.Shared.Trialmax.CTmaxKeyboard m_tmaxKeyboard = new CTmaxKeyboard();
		
		/// <summary>Application message filter used to trap custom windows messages</summary>
		private FTI.Shared.Trialmax.CTmaxAsyncFilter m_tmaxAsyncFilter = new CTmaxAsyncFilter();
		
		/// <summary>The application's print manager/form</summary>
		FTI.Trialmax.Panes.CFPrint m_tmaxPrintManager = null;

		/// <summary>Application's primary initialization file</summary>
		private CXmlIni m_xmlIni = new CXmlIni();
		
		/// <summary>Application's collection of valid media types</summary>
		private FTI.Shared.Trialmax.CTmaxMediaTypes	m_tmaxMediaTypes = new CTmaxMediaTypes();
		
		/// <summary>Application's collection of valid source types</summary>
		private FTI.Shared.Trialmax.CTmaxSourceTypes m_tmaxSourceTypes = new CTmaxSourceTypes();
		
		/// <summary>Application's error message builder</summary>
		private FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();
		
		/// <summary>Local member bound to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Local member to manage media encoder operations</summary>
		private FTI.Trialmax.Encode.CWMEncoder m_mediaEncoder = new CWMEncoder();

		/// <summary>Local member to store and retrieve registry information</summary>
		private FTI.Shared.Trialmax.CTmaxRegistry m_tmaxRegistry = new CTmaxRegistry();

		/// <summary>Local member to store product information</summary>
		private FTI.Shared.Trialmax.CTmaxProductManager m_tmaxProductManager = new CTmaxProductManager();

		/// <summary>Application's error log</summary>
		private FTI.Shared.Xml.CXmlFile m_xmlErrors = new CXmlFile();
		
		/// <summary>Application's debugging log</summary>
		private FTI.Shared.Xml.CXmlFile m_xmlDiagnostics = new CXmlFile();
		
		/// <summary>Local array of display panes</summary>
		private CBasePane[] m_aPanes = new CBasePane[(int)TmaxAppPanes.MaxPanes];
		
		/// <summary>Object used to simulate user operation</summary>
		private CTmaxManagerSimulator m_tmaxSimulator = null;
		
		/// <summary>Path of the application's configuration file</summary>
		private string m_strIniFilename = "";
		
		/// <summary>The folder the application is executing from</summary>
		private string m_strAppFolder = "";
		
		/// <summary>The fully qualified path to the update installer application</summary>
		private string m_strUpdateInstaller = "";
		
		/// <summary>The fully qualified path to the installer's XML update file</summary>
		private string m_strXmlUpdateFileSpec = "";
		
		/// <summary>Path to Presentation's configuration file</summary>
		private string m_strPresentationIniFileSpec = "";
		
		/// <summary>Flag to inhibit processing of toolbar click events</summary>
		private bool m_bIgnoreToolbarClicks = false;
		
		/// <summary>Local flag to indicate that the updates installer application has been updated</summary>
		private bool m_bInstallerUpdated = false;
		
		/// <summary>Flag to indicate that the panes should be notified when the app gets activated</summary>
		private bool m_bNotifyOnActivate = false;
		
		/// <summary>Flag to indicate that the application is terminating</summary>
		private bool m_bTerminating = false;
		
		/// <summary>TrialMax ActiveX movie control used by the database to retrieve video file extents</summary>
		private FTI.Trialmax.ActiveX.CTmxMovie m_tmxMovie;
		
		/// <summary>TrialMax ActiveX viewer control used by the database to split multipage tiff files</summary>
		private FTI.Trialmax.ActiveX.CTmxView m_tmxView;

		/// <summary>Pane control used to display control and assembly version information</summary>
		private FTI.Trialmax.Panes.CMessagePane m_paneVersions;
		
		/// <summary>Pane control used to display error messages</summary>
		private FTI.Trialmax.Panes.CMessagePane m_paneErrors;
		
		/// <summary>Pane control used to display diagnostic messages</summary>
		private FTI.Trialmax.Panes.CMessagePane m_paneDiagnostics;
		
		/// <summary>Pane control used to display the database media tree</summary>
		private FTI.Trialmax.Panes.CMediaTree m_paneMedia;
		
		/// <summary>Pane control used to display the database binders tree</summary>
		private FTI.Trialmax.Panes.CBinderTree m_paneBinders;
		
		/// <summary>Pane control used to display the video tuner controls</summary>
		private FTI.Trialmax.Panes.CTunePane m_paneTuner;
		
		/// <summary>Pane control used to display transcripts</summary>
		private FTI.Trialmax.Panes.CTranscriptPane m_paneTranscripts;
		
		/// <summary>Pane control used to display scripts</summary>
		private FTI.Trialmax.Panes.CScriptBuilder m_paneScripts;
		
		/// <summary>Pane control used to display media properties</summary>
		private FTI.Trialmax.Panes.CPropertiesPane m_paneProperties;
		
		/// <summary>Pane control used to display source for registration</summary>
		private FTI.Trialmax.Panes.CExplorerPane m_paneSource;
		
		/// <summary>Pane control used to view media files</summary>
		private FTI.Trialmax.Panes.CMediaViewer m_paneViewer;
		
		/// <summary>Pane control used to display search results</summary>
		private FTI.Trialmax.Panes.CResultsPane m_paneResults;
		
		/// <summary>Pane control used to display application help</summary>
		private FTI.Trialmax.Panes.CHelpPane m_paneHelp;
		
		/// <summary>Enumerated id of the pane that accepted the last drop operation</summary>
		private TmaxAppPanes m_eDropPane;
		
		/// <summary>Local member to store application command line</summary>
		private CTmaxCommandLine m_tmaxCommandLine = null;
		
		#endregion Private Members
		
		#region Infragistics Docking Internals
		
		/// <summary>Infragistics auto-hide control</summary>
		private AutoHideControl _CTmaxManagerFormAutoHideControl;

		/// <summary>Infragistics unpinned dock area component to manage left side</summary>
		private UnpinnedTabArea _CTmaxManagerFormUnpinnedTabAreaLeft;
		
		/// <summary>Infragistics unpinned dock area component to manage right side</summary>
		private UnpinnedTabArea _CTmaxManagerFormUnpinnedTabAreaRight;
		
		/// <summary>Infragistics unpinned dock area component to manage top</summary>
		private UnpinnedTabArea _CTmaxManagerFormUnpinnedTabAreaTop;
		
		/// <summary>Infragistics unpinned dock area component to manage bottom</summary>
		private UnpinnedTabArea _CTmaxManagerFormUnpinnedTabAreaBottom;
		
		/// <summary>Infragistics dock area component to manage top</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxManagerForm_Toolbars_Dock_Area_Top;

        void m_paneViewer_OnRequestPresentation(object sender, EventArgs e)
        {
            OnAppOpenPresentation();
        }
		
		/// <summary>Infragistics dock area component to manage bottom</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxManagerForm_Toolbars_Dock_Area_Bottom;
		
		/// <summary>Infragistics dock area component to manage left side</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxManagerForm_Toolbars_Dock_Area_Left;
		
		/// <summary>Infragistics dock area component to manage right side</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxManagerForm_Toolbars_Dock_Area_Right;
		
		private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea1;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow1;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow2;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow4;
		private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea2;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow5;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow6;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow7;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow8;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow9;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow11;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow12;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow3;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow10;
		private FTI.Trialmax.Panes.CCodesPane m_paneCodes;
		private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea4;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow14;
		private FTI.Trialmax.Panes.CFilteredTree m_paneFiltered;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow15;
		private System.Windows.Forms.Panel m_ctrlMainFillPanel;
		private System.Windows.Forms.Panel m_ctrlFillPanel;
		private AxTM_PRINT6Lib.AxTMPrint6 m_tmxPrint;
		private FTI.Trialmax.ActiveX.CTmxShare m_ctrlPresentation;
		private AxTM_GRAB6Lib.AxTMGrab6 m_ctrlScreenCapture;
		private CObjectionsPane m_paneObjections;
		private WindowDockingArea windowDockingArea6;
		private DockableWindow dockableWindow16;
		private CScriptReviewPane m_paneScriptReview;
		private DockableWindow dockableWindow17;
		private CObjectionPropertiesPane m_paneObjectionProperties;
		private DockableWindow dockableWindow18;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow13;
		
		#endregion Infragistics Docking Internals
		
		#region Public Methods
		
		/// <summary>The main entry point for the application</summary>
		[STAThread]
		static void Main(string[] args) 
		{
			FTI.Trialmax.Forms.CFSplashScreen splashScreen = null;

			try
			{
				//	Check to see if there's already an active instance
				if(CTmaxInstanceManager.GetPrevInstance(TmaxApplications.TmaxManager) == true)
				{
					//	Activate the previous instance
					CTmaxInstanceManager.ActivatePrevInstance(args, TmaxApplications.TmaxManager);
				}
				else
				{
					splashScreen = new CFSplashScreen();
					splashScreen.Start();
					splashScreen.SetMessage("Starting TmaxManager");


                    try
                    {
                        Application.Run(new CTmaxManagerForm(args, splashScreen));
                    }
                    catch (System.ObjectDisposedException e)
                    {
                    }
				}
			
			}
			catch(System.DllNotFoundException e)
			{
				MessageBox.Show(e.Message, "Dll Not Found Exception");
			}
			catch(System.IO.FileNotFoundException e)
			{
				if((e.FileName != null) && (e.FileName.Length > 0))
					MessageBox.Show(e.Message + ": " + e.FileName, "File Not Found Exception");
				else
					MessageBox.Show(e.ToString(), "File Not Found Exception");
			}
			catch(System.Exception  e)
			{
				MessageBox.Show(e.ToString(), "System Exception");
			}
			finally
			{
				if(splashScreen != null)
					splashScreen.Stop();
			}

		
		}// static void Main() 
		
		/// <summary>Constructor</summary>
		public CTmaxManagerForm(string[] args, FTI.Trialmax.Forms.CFSplashScreen splashScreen)
		{
			//	Start the splash screen thread
			m_ctrlSplashScreen = splashScreen;
			if(m_ctrlSplashScreen != null)
			{
				try
				{
					SetSplashMessage("Loading TmaxManager");
					Thread splashThread = new Thread(new ThreadStart(this.SplashThreadProc));
					splashThread.Start();
				}
				catch
				{
				}
				
			}// if(m_ctrlSplashScreen != null)
		
			//	Connect to the local event source
			m_tmaxEventSource.Name = "TrialMax Application";
			m_tmaxEventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);
			m_tmaxEventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.OnDiagnostic);
			
			//	Populate the error builder
			SetErrorStrings();
			
			// Required for Windows Form Designer support
			SetSplashMessage("Initializing main window");
			InitializeComponent();

			//	Initialize the application's files
			SetSplashMessage("Reading configuration files");
			InitializeFiles();
			
			//	Initilize the local class members
			SetSplashMessage("Initializing class members");
			InitializeMembers();
			
			//	Initialize the Presentation interfaces
			SetSplashMessage("Initializing TmaxPresentation interface");
			InitializePresentation();
			
			//	Initialize the panes using the INI file
			SetSplashMessage("Initializing pane windows");
			InitializePanes();

			//	Initialize the application's product manager
			SetSplashMessage("Initializing product manager");
			InitializeProductManager();
			
			//	Initialize the database
			SetSplashMessage("Initializing database engine");
			InitializeDatabase(m_tmaxDatabase);
			
			//	Initialize the versions information
			SetSplashMessage("Reading version descriptors");
			InitializeVersions();
			
			// Initialize docking capabilities
			SetSplashMessage("Initializing screen layout");
			InitializeLayout();
			
			//	Initialize the menu and toolbars
			SetSplashMessage("Initializing toolbar manager");
			InitializeToolbarManager();
			
			//	Make sure the proper panes are activated
			SetSplashMessage("Activating child panes");
			SetPaneStates();
			
			//	Enable screen capture
			SetSplashMessage("Initializing screen capture");
			InitializeScreenCapture();
			
			//	Initialize a command line object if provided by the caller
			if((args != null) && (args.Length > 0))
			{
				m_tmaxCommandLine = new CTmaxCommandLine();
				m_tmaxCommandLine.SetProperties(args);
			}
            CTmaxComponent tmaxComponent = null;

            //  Check Activation Key 
            CheckActivation(tmaxComponent);
            
		
		}//	CTmaxManagerForm::CTmaxManagerForm()

		#endregion Public Methods

        #region Private Method
        /// <summary>This method will check activation and expiration of the TrialMax
        private void CheckActivation(CTmaxComponent tmaxComponent)
        {
            DateTime datetimeCheck = new DateTime();
            bool isActivated = false;

            // var isExpired is used to flag for Expiration message
            bool isExpired = false;
            if ((tmaxComponent = m_tmaxProductManager.Components.Find(TmaxComponents.TrialMax)) != null)
            {
                if (m_tmaxProductManager.IsDefaultActivationCode(tmaxComponent.ActivationCode))
                {
                    if (m_tmaxProductManager.MasterKeyDefaultDuration == 0 || 
                        (DateTime.TryParse(tmaxComponent.ActivationDate, out datetimeCheck) && 
                        datetimeCheck.AddMonths(m_tmaxProductManager.MasterKeyDefaultDuration) >= DateTime.Now))
                    isActivated = true;
                }
                else
                {
                    if (tmaxComponent.ActivationDate != "")
                    {
                        if (DateTime.TryParse(tmaxComponent.ActivationDate, out datetimeCheck) && datetimeCheck.AddDays(m_tmaxProductManager.DaysAllowed) >= DateTime.Now)
                            isActivated = true;
                        else
                            isExpired = true;
                    }
                     
                }
            }
            if (!isActivated)
            {
                OnProductExpiry(isExpired);
            }
        }
        #endregion

        #region Private Methods

        private delegate void LoadBarcodeDelegate();
		
		/// <summary>This method will put the focus on the Go To barcode text box in the main menu</summary>
		private void SetFocusLoadBarcode()
		{
			if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null))
			{
				try
				{
					((TextBoxTool)(m_ctrlUltraToolbarManager.Toolbars["MainMenu"].Tools["LoadBarcode"])).IsActiveTool = true;
					((TextBoxTool)(m_ctrlUltraToolbarManager.Toolbars["MainMenu"].Tools["LoadBarcode"])).IsInEditMode = true;
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireDiagnostic(this, "ActivateGoTo", "Ex: " + Ex.ToString());
				}
						
			}

		}// private void ActivateGoTo()
		
		/// <summary>Thread procedure for updating splash box progress during initialization</summary>
		private void SplashThreadProc()
		{
			try
			{
				//	Loop as long as the splash screen is visible
				while(true)
				{
					if(m_ctrlSplashScreen == null) break;
					if(m_ctrlSplashScreen.IsDisposed == true) break;
					if(m_ctrlSplashScreen.Visible == false) break;

					//	Step the progress indicator
					m_ctrlSplashScreen.StepProgress();
					
					//	Delay
					System.Threading.Thread.Sleep(10);
					
				}// while(true)
			
			}
			catch
			{
			}
			
		}// private void SplashThreadProc()
		
		/// <summary>This method will set the text displayed in the splash box</summary>
		/// <param name="strMessage">The message to be displayed</param>
		private void SetSplashMessage(string strMessage)
		{
			if((m_ctrlSplashScreen != null) && (m_ctrlSplashScreen.IsDisposed == false))
			{
				try 
				{ 
					//	Set the text
					m_ctrlSplashScreen.SetMessage(strMessage); 
				
					//	Step the progress indicator
					m_ctrlSplashScreen.StepProgress();
				
				}
				catch 
				{
				}
			}
			
		}// private void SetSplashMessage(string strMessage)
		
		/// <summary>This method is to add the specified pane to the docking manager collection</summary>
		/// <param name="ePane">The enumerated pane identifier</param>
		///	<returns>true if successful</returns>
		private bool AddPane(TmaxAppPanes ePane, TmaxAppPanes eSibling)
		{
			DockableControlPane dcp = null;
			DockablePaneBase	dpb = null;
			DockAreaPane		dap = null;
			
			Debug.Assert(m_aPanes != null);
			Debug.Assert(m_aPanes[(int)ePane] != null);
			if((m_aPanes == null) || (m_aPanes[(int)ePane] == null)) return false;
			
			//	Need a docking pane to add the control pane to
			if(m_ctrlUltraDockManager.DockAreas == null) return false;
			if(m_ctrlUltraDockManager.DockAreas.Count == 0) return false;
			
			try
			{
				dcp = new DockableControlPane(GetUltraPaneKey(ePane), GetUltraTabText(ePane), m_aPanes[(int)ePane]);

				dcp.Text = GetUltraTitleText(ePane);
				dcp.TextTab = GetUltraTabText(ePane);
				dcp.Closed  = true;
	
				if((dpb = GetUltraPane(eSibling)) != null)
					dap = dpb.DockAreaPane;
					
				if(dap == null)
					dap = m_ctrlUltraDockManager.DockAreas[0];
					
				if((dap != null) && (dap.Panes != null))
					dap.Panes.Add(dcp);
					
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddPane", m_tmaxErrorBuilder.Message(ERROR_ADD_PANE_EX, ePane.ToString()), Ex);
			}
			
			return false;
			
		}// private bool AddPane(TmaxAppPanes ePane)
		
		/// <summary>This method is called to determine if the specified command should be enabled</summary>
		/// <param name="eCommand">The command being checked</param>
		/// <returns>true to enable the command</returns>
		private bool GetCommandEnabled(AppCommands eCommand)
		{
			string		strUsersManual = "";
			ArrayList	aPending = null;
			

			switch(eCommand)
			{
				case AppCommands.ImportXmlScript:
				case AppCommands.ImportAsciiScript:
				case AppCommands.ImportLoadFile:
				case AppCommands.ValidateDatabase:
				case AppCommands.CaseOptions:
				case AppCommands.ReloadCase:
				case AppCommands.CloseCase:
				case AppCommands.SetFilter:
				case AppCommands.FastFilter:
				case AppCommands.TrimDatabase:
				
					if(m_tmaxDatabase == null) return false;
					if(m_tmaxDatabase.Primaries == null) return false;
					break;
					
				case AppCommands.ImportBarcodeMap:
				
					if(m_tmaxDatabase == null) return false;
					if(m_tmaxDatabase.BarcodeMap == null) return false;
					break;

				case AppCommands.ImportAsciiObjections:
				case AppCommands.ExportAsciiObjections:

					if(m_tmaxDatabase == null) return false;
					if(m_tmaxDatabase.ObjectionsEnabled == false) return false;
					break;

				case AppCommands.ImportXmlCaseCodes:
				case AppCommands.ExportXmlCaseCodes:
				
					if(m_tmaxDatabase == null) return false;
					if(m_tmaxDatabase.CodesEnabled == false) return false;
					if(m_tmaxDatabase.CodesManager == null) return false;
					break;
					
				case AppCommands.ExportBarcodeMap:

					if(m_tmaxDatabase == null) return false;
					if(m_tmaxDatabase.BarcodeMap == null) return false;
					if(m_tmaxDatabase.BarcodeMap.Count == 0) return false;
					break;
					
				case AppCommands.Find:
				case AppCommands.Print:
				case AppCommands.BulkUpdate:
				case AppCommands.ImportCodes:
				case AppCommands.ImportCodesDatabase:
				case AppCommands.ImportAsciiBinder:
				case AppCommands.ImportXmlBinder:
				
					if(m_tmaxDatabase == null) return false;
					if(m_tmaxDatabase.Primaries == null) return false;
					if(m_tmaxDatabase.Primaries.Count == 0) return false;
					
					if((eCommand == AppCommands.ImportCodes) ||
					   (eCommand == AppCommands.ImportCodesDatabase) ||
					   (eCommand == AppCommands.BulkUpdate))
					{
						if(m_tmaxDatabase.CodesEnabled == false) return false;
					}
					
					break;
					
				case AppCommands.FindNext:

					if(m_paneResults == null) return false;
					if(m_paneResults.CanFindNext() == false) return false;
					break;
					
				case AppCommands.CheckForUpdates:

					if((m_tmaxProductManager != null) && (m_tmaxProductManager.Activated == false))
						return false;

					break;
					
				case AppCommands.OnlineSite:

					if((m_tmaxProductManager == null) || (m_tmaxProductManager.OnlineSite.Length == 0))
						return false;

					break;
					
				case AppCommands.UsersManual:

					//	Does the user's manual exist?
					strUsersManual = m_tmaxAppOptions.GetUsersManualFileSpec();
					if((strUsersManual.Length == 0) || (System.IO.File.Exists(strUsersManual) == false))
						return false;
						
					break;
					
				case AppCommands.RefreshTreatments:

					if(m_tmaxDatabase == null) return false;
					if(m_tmaxDatabase.Primaries == null) return false;

					//	Are there pending treatments?
					if((aPending = m_tmaxDatabase.GetZapFiles()) != null)
					{
						if(aPending.Count == 0) return false;
						aPending.Clear();
						aPending = null;
					}
					else
					{
						return false;
					}
					break;
					
				case AppCommands.OpenPresentation:

					if(m_ctrlPresentation == null) return false;
					if(m_ctrlPresentation.Initialized == false) return false;
						
					break;
					
				default:
				
					break;	//	Enabled by default
					
			}
		
			return true;
			
		}// private bool GetCommandEnabled(AppCommands eCommand)
		
		/// <summary>This method converts the specified text key to an application command enumeration</summary>
		/// <param name="strKey">The text identifier</param>
		/// <returns>The associated application command</returns>
		private AppCommands GetCommand(string strKey)
		{
			try
			{
				Array aCommands = Enum.GetValues(typeof(AppCommands));
				
				foreach(AppCommands O in aCommands)
				{
					if(O.ToString() == strKey)
						return O;
				}
				
			}
			catch
			{
			}
			
			return AppCommands.Invalid;
		
		}// private AppCommands GetCommand(string strKey)
		
		/// <summary>This function is called to prompt the user to select the case folder</summary>
		/// <param name="strFolder">Current case folder</param>
		/// <param name="bCreate">True if user is creating a new case</param>
		/// <returns>true if successful</returns>
		private bool GetCaseFolder(ref string strFolder, ref bool bCreate)
		{
			string							strMsg = "";
			FTI.Shared.CBrowseForFolder		bff = null;
			FTI.Trialmax.Forms.CFNewCase	cfNewCase = null;
			bool							bSuccessful = false;
			
			DisableTmaxKeyboard(true);

			while(true)
			{
				//	Is the user trying to open a case?
				if(bCreate == false)
				{
					if(bff == null)
					{
						bff = new CBrowseForFolder();
						if(strFolder != null)
							bff.Folder = strFolder;
					}

					if(bff.ShowDialog(this.Handle) == DialogResult.Cancel) 
						break;
					
					//	Does the database exist?
					if(m_tmaxDatabase.Exists(bff.Folder) == true)
					{
						strFolder = bff.Folder;
						bSuccessful = true;
						break;
					}
					else
					{
						//	Notify the user and try again
						strMsg = String.Format("{0} does not contain a TrialMax database.", bff.Folder);
						MessageBox.Show(strMsg, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						
					}// if(m_tmaxDatabase.Exists(bff.Folder) == true)
				
				}// if(bCreate == false)
				else
				{
					if(cfNewCase == null)
					{
						cfNewCase = new CFNewCase();
						cfNewCase.SetParentFromCase(strFolder);
					}
					
					if(cfNewCase.ShowDialog(this) == DialogResult.Cancel)
						break;
					
					//	Does the database exist?
					if(m_tmaxDatabase.Exists(cfNewCase.GetFolderSpec()) == true)
					{
						//	Prompt the user
						strMsg = String.Format("{0} already contains a TrialMax database. Do you want to open the existing case?", cfNewCase.GetFolderSpec());
						
						if(MessageBox.Show(strMsg, "Create?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
						{
							bCreate = false;
							strFolder = cfNewCase.GetFolderSpec();
							bSuccessful = true;
							break;
						}
						else
						{
							//	Loop
						}
						
					}
					else
					{
						strFolder = cfNewCase.GetFolderSpec();
						bSuccessful = true;
						break;
					}

				}// if(bCreate == false)
			
			}// while(true)
			
			DisableTmaxKeyboard(false);
			return bSuccessful;
		
		}// GetCaseFolder(string strFolder, bool bCreate)
		
		/// <summary>This function is called to get the version of TmaxInstaller</summary>
		/// <returns>The TrialMax version descriptor if available</returns>
		private CBaseVersion GetInstallerVersion()
		{
			CBaseVersion verInstaller = null;
			string strUpdateFileSpec = "";
			string strVerCurrent = "";
			string strVerUpdate = "";
			long lPackedCurrent = 0;
			long lPackedUpdate = 0;
			int iBuildCurrent = 0;
			int iBuildUpdate = 0;
			System.DateTime dtBuildCurrent = System.DateTime.Now;
			System.DateTime dtBuildUpdate = System.DateTime.Now;
			bool bReplace = false;
			int iAttempts = 0;

			Debug.Assert(m_strUpdateInstaller.Length != 0);
			if(m_strUpdateInstaller.Length == 0) return null;

			//	Get the version information for the current installer
			if(System.IO.File.Exists(m_strUpdateInstaller) == true)
			{
				strVerCurrent = CBaseVersion.GetVersion(m_strUpdateInstaller);
				lPackedCurrent = CBaseVersion.GetPackedVersion(strVerCurrent);
				iBuildCurrent = CBaseVersion.GetBuild(strVerCurrent);
				if(iBuildCurrent > 0)
					CBaseVersion.GetBuildAsDate(iBuildCurrent, ref dtBuildCurrent);
			}

			//	Get the version information for the update if it exists
			strUpdateFileSpec = m_strAppFolder;
			if((strUpdateFileSpec.Length > 0) && (strUpdateFileSpec.EndsWith("\\") == false))
				strUpdateFileSpec += "\\";
			strUpdateFileSpec += DEFAULT_UPDATES_SUBFOLDER;
			strUpdateFileSpec += DEFAULT_UPDATE_INSTALLER_FILENAME;
			if(System.IO.File.Exists(strUpdateFileSpec) == true)
			{
				strVerUpdate = CBaseVersion.GetVersion(strUpdateFileSpec);
				lPackedUpdate = CBaseVersion.GetPackedVersion(strVerUpdate);
				iBuildUpdate = CBaseVersion.GetBuild(strVerUpdate);
				if(iBuildUpdate > 0)
					CBaseVersion.GetBuildAsDate(iBuildUpdate, ref dtBuildUpdate);
			}

			//	Do we have a pending update?
			if(lPackedUpdate > 0)
			{
				//	Are the major.minor.update values the same?
				if(lPackedUpdate == lPackedCurrent)
				{
					if((iBuildCurrent > 0) && (iBuildUpdate > 0))
						bReplace = (dtBuildUpdate > dtBuildCurrent);
					else
						bReplace = (iBuildUpdate > iBuildCurrent);
				}
				else
				{
					bReplace = (lPackedUpdate > lPackedCurrent);
				}

			}// if(lPackedUpdate > 0)

			//	Do we have a newer update?
			if(bReplace == true)
			{
				//	Delete the current application
				while(System.IO.File.Exists(m_strUpdateInstaller) == true)
				{
					//	Attempt to delete the file
					try
					{
						System.IO.File.Delete(m_strUpdateInstaller);
					}
					catch
					{
						//	It may be that manager was launched by the installer and
						//	the installer has not yet had time to terminate.
						if(iAttempts < 5)
						{
							Thread.Sleep(200);
							iAttempts++;
						}
						else
						{
							//	Give up 
							break;
						}

					}

				}// while(System.IO.File.Exists(m_strUpdateInstaller) == true)

				//	Install the update
				if(System.IO.File.Exists(m_strUpdateInstaller) == false)
				{
					try
					{
						System.IO.File.Move(strUpdateFileSpec, m_strUpdateInstaller);

						lPackedCurrent = lPackedUpdate;
						strVerCurrent = strVerUpdate;

						m_bInstallerUpdated = true;
					}
					catch(System.Exception Ex)
					{
						m_tmaxEventSource.FireError(this, "GetInstallerVersion", m_tmaxErrorBuilder.Message(ERROR_UPDATE_INSTALLER_EX, m_strUpdateInstaller), Ex);
					}

				}// if(System.IO.File.Exists(m_strUpdateInstaller) == false)

			}// if((lPackedUpdate > 0) && (lPackedUpdate > lPackedCurrent))

			//	Do we have the installer?
			if(lPackedCurrent > 0)
			{
				verInstaller = new CBaseVersion();
				verInstaller.SetTmaxLocation(m_strUpdateInstaller);
				verInstaller.Description = "TrialMax Updates Installer";
			}

			return verInstaller;

		}// private CBaseVersion GetInstallerVersion()
		
		/// <summary>This function is called to retrieve the docking pane associated with the specified application command</summary>
		/// <param name="eCommand">Enumerated application command identifier</param>
		/// <returns>Infragistics dockable base pane owneer</returns>
		private DockablePaneBase GetUltraPane(AppCommands eCommand)
		{
			try
			{
				TmaxAppPanes ePane = GetAppPane(eCommand);
				
				if(ePane != TmaxAppPanes.MaxPanes)
					return GetUltraPane(ePane);
				else
					return null;
			}
			catch
			{
				return null;
			}
				
		}// private DockablePaneBase GetUltraPane(AppCommands eCommand)
		
		/// <summary>This function is called to retrieve the docking pane that owns the specified pane control</summary>
		/// <param name="eTmaxPane">Enumerated pane control identifier</param>
		/// <returns>Infragistics dockable base pane owneer</returns>
		private DockablePaneBase GetUltraPane(TmaxAppPanes eTmaxPane)
		{
			string strKey = GetUltraPaneKey(eTmaxPane);
		
			if((strKey != null) && (strKey.Length > 0))
				return GetUltraPane(strKey);
			else
				return null;
				
		}// private DockablePaneBase GetUltraPane(TmaxAppPanes eTmaxPane)
		
		/// <summary>This function is called to retrieve the docking pane with the specified key</summary>
		/// <param name="strKey">The value assigned to the pane's Key property</param>
		/// <returns>Infragistics dockable base pane</returns>
		private DockablePaneBase GetUltraPane(string strKey)
		{
			DockablePaneBase Pane = null;
			
			if((m_ctrlUltraDockManager != null) && (strKey != null) && (strKey.Length > 0))
			{
				//	NOTE:	We purposely do not trap exceptions here. It
				//			is up to the calling function to trap the exception
				
				//	Check our local dock manager first
				if((Pane = m_ctrlUltraDockManager.PaneFromKey(strKey)) == null)
				{
					
				}
			
			}// if((m_ctrlUltraDockManager != null) && (strKey.Length > 0))

			return Pane;
		
		}// private DockablePaneBase GetUltraPane(string strKey)
		
		/// <summary>This function is called to convert the specified pane control identifier to its associated alpha-numeric dockable pane key</summary>
		/// <param name="eTmaxPane">Enumerated Pane Identifier</param>
		/// <returns>Alpha-numeric key for the specified pane</returns>
		private string GetUltraPaneKey(TmaxAppPanes eTmaxPane)
		{
			string strKey = eTmaxPane.ToString();
			return strKey;
		
		}// GetPaneKey(TmaxAppPanes eTmaxPane)
		
		/// <summary>This function is get the tab text for the specified pane</summary>
		/// <param name="eTmaxPane">Enumerated Pane Identifier</param>
		/// <returns>Text to be displayed in the property page tab</returns>
		private string GetUltraTabText(TmaxAppPanes eTmaxPane)
		{
			switch(eTmaxPane)
			{
				case TmaxAppPanes.Errors:				return "Errors";
				case TmaxAppPanes.Diagnostics:			return "Diagnostics";
				case TmaxAppPanes.Media:				return "Media";
				case TmaxAppPanes.Source:				return "Source";
				case TmaxAppPanes.Binders:				return "Binders";
				case TmaxAppPanes.Viewer:				return "Viewer";
				case TmaxAppPanes.Properties:			return "Properties";
				case TmaxAppPanes.Scripts:				return "Scripts";
				case TmaxAppPanes.Results:				return "Results";
				case TmaxAppPanes.Transcripts:			return "Transcripts";
				case TmaxAppPanes.Tuner:				return "Tuner";
				case TmaxAppPanes.Help:					return "Help";
				case TmaxAppPanes.Versions:				return "Versions";
				case TmaxAppPanes.Codes:				return "Codes";
				case TmaxAppPanes.Objections:			return "Objections";
				case TmaxAppPanes.ObjectionProperties:	return "Objection Props";
				case TmaxAppPanes.ScriptReview:			return "Script Review";
				default:								return eTmaxPane.ToString();
			}
		
		}// private string GetUltraTabText(TmaxAppPanes eTmaxPane)
		
		/// <summary>This function is get the title bar text for the specified pane</summary>
		/// <param name="eTmaxPane">Enumerated Pane Identifier</param>
		/// <returns>Text to be displayed in the property page title bar</returns>
		private string GetUltraTitleText(TmaxAppPanes eTmaxPane)
		{
			switch(eTmaxPane)
			{
				case TmaxAppPanes.Errors:				return "Error Messages";
				case TmaxAppPanes.Diagnostics:			return "Diagnostic Messages";
				case TmaxAppPanes.Media:				return "Media Tree";
				case TmaxAppPanes.Source:				return "Source Explorer";
				case TmaxAppPanes.Binders:				return "Binder Tree";
				case TmaxAppPanes.Viewer:				return "Media Viewer";
				case TmaxAppPanes.Properties:			return "Properties";
				case TmaxAppPanes.Scripts:				return "Script Builder";
				case TmaxAppPanes.Results:				return "Search Results";
				case TmaxAppPanes.Transcripts:			return "Transcripts";
				case TmaxAppPanes.Tuner:				return "Tuner";
				case TmaxAppPanes.Help:					return "Help";
				case TmaxAppPanes.Versions:				return "Versions";
				case TmaxAppPanes.Codes:				return "Codes";
				case TmaxAppPanes.Objections:			return "Objections";
				case TmaxAppPanes.ObjectionProperties:	return "Objection Properties";
				case TmaxAppPanes.ScriptReview:			return "Script Review";
				default: return eTmaxPane.ToString();
			}
		
		}// private string GetUltraTitleText(TmaxAppPanes eTmaxPane)
		
		/// <summary>This method will retrieve the application pane identifier associated with the specified command</summary>
		/// <param name="eCommand">The enumerated application command</param>
		/// <returns>The pane enumerator if valid, MaxPanes if no associated pane</returns>
		private TmaxAppPanes GetAppPane(AppCommands eCommand)
		{
			TmaxAppPanes ePane = TmaxAppPanes.MaxPanes;
			
			//	Which command is associated with this tool?
			switch(eCommand)
			{
				case AppCommands.ToggleSourceExplorer: 
					
					ePane = TmaxAppPanes.Source;
					break;
						
				case AppCommands.ToggleMediaTree:    
					
					ePane = TmaxAppPanes.Media;
					break;
						
				case AppCommands.ToggleBinders:    
					
					ePane = TmaxAppPanes.Binders;
					break;
						
				case AppCommands.ToggleFilteredTree:    
					
					ePane = TmaxAppPanes.FilteredTree;
					break;
						
				case AppCommands.ToggleMediaViewer:    
					
					ePane = TmaxAppPanes.Viewer;
					break;
						
				case AppCommands.ToggleProperties:   
					
					ePane = TmaxAppPanes.Properties;
					break;
						
				case AppCommands.ToggleScriptBuilder:   
					
					ePane = TmaxAppPanes.Scripts;
					break;
						
				case AppCommands.ToggleTranscripts: 
					
					ePane = TmaxAppPanes.Transcripts;
					break;
						
				case AppCommands.ToggleTuner:  
					
					ePane = TmaxAppPanes.Tuner;
					break;
						
				case AppCommands.ToggleCodes:  
					
					ePane = TmaxAppPanes.Codes;
					break;
						
				case AppCommands.ToggleSearchResults: 
					
					ePane = TmaxAppPanes.Results;
					break;
						
				case AppCommands.ToggleErrorMessages:    
					
					ePane = TmaxAppPanes.Errors;
					break;
						
				case AppCommands.ToggleVersions:    
					
					ePane = TmaxAppPanes.Versions;
					break;
						
				case AppCommands.ToggleDiagnostics: 
					
					ePane = TmaxAppPanes.Diagnostics;
					break;

				case AppCommands.ToggleObjections:

					ePane = TmaxAppPanes.Objections;
					break;

				case AppCommands.ToggleObjectionProperties:

					ePane = TmaxAppPanes.ObjectionProperties;
					break;

				case AppCommands.ToggleScriptReview:

					ePane = TmaxAppPanes.ScriptReview;
					break;

				case AppCommands.HelpContents:
					
					ePane = TmaxAppPanes.Help;
					break;
						
				default:
				
					break;
					
			}// switch(GetCommand(ultraTool.Key))
		
			return ePane;
			
		}// private void SetUltraCheckedState(ToolBase ultraTool)
		
		/// <summary>This method is called to get the pane that currently has the keyboard focus</summary>
		/// <returns>The pane that currently has the keyboard focus</returns>
		private CBasePane GetActivePane()
		{
			CBasePane tmaxPane = null;
			
			if(m_ctrlUltraDockManager == null) return null;
			if(m_ctrlUltraDockManager.ActivePane == null) return null;
			if(m_ctrlUltraDockManager.ActivePane.Control == null) return null;
			
			try
			{
				//	Make sure the pane is visible
				//
				//	NOTE:	Just because the pane's active it may not be visible. If the 
				//			user selects the pane and then clicks on the close button, 
				//			Infragistics still makes it the active pane
				if(m_ctrlUltraDockManager.ActivePane.IsVisible == true)
				{
					tmaxPane = (CBasePane)(m_ctrlUltraDockManager.ActivePane.Control);
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetActivePane", "Ex: " + Ex.ToString());
			}
					
			return tmaxPane;
			
		}// private CBasePane GetActivePane()
		
		/// <summary>This function is called to retrieve the menu / toolbar tool with the specified key</summary>
		/// <param name="strKey">Alpha-numeric key identifier</param>
		/// <returns>Infragistics base class tool object</returns>
		/// <remarks>
		/// This function intentionally does not trap exceptions. It is up to the
		///	caller to trap all exceptions.
		///</remarks>
		private ToolBase GetUltraTool(string strKey)
		{
			ToolBase Tool = null;
			
			if(m_ctrlUltraToolbarManager != null)
			{
				Tool = m_ctrlUltraToolbarManager.Tools[strKey];
			}
	
			return Tool;
		
		}// GetUltraTool(string strKey)
		
		/// <summary>This function is called to get the tool associated with the specified command</summary>
		/// <param name="eCommand">The application command enumeration</param>
		/// <returns>Infragistics base class tool object</returns>
		private ToolBase GetUltraTool(AppCommands eCommand)
		{
			return GetUltraTool(eCommand.ToString());
		}
		
		/// <summary>This function is called to determine if the panes are locked</summary>
		/// <returns>True if the panes are locked</returns>
		private bool GetUltraLockedState()
		{
			if((m_ctrlUltraDockManager.ControlPanes != null) &&
				(m_ctrlUltraDockManager.ControlPanes.Count > 0))
			{
				//	Assume if one is locked they are all locked
				if(m_ctrlUltraDockManager.ControlPanes[0].Settings.AllowDragging == DefaultableBoolean.False)
					return true;
			}
			
			return false;
		
		}// private bool GetUltraLockedState()

		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane1 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedRight, new System.Guid("ca1f80c4-a2e3-430a-b556-dfbceb97651a"));
			Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane1 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("6780c3bc-0feb-49b3-acc7-3ec8ab9e0343"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("ca1f80c4-a2e3-430a-b556-dfbceb97651a"), -1);
			Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane2 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("e6924544-31ee-49df-8529-d92f683b0ee0"), new System.Guid("de049042-f47f-4c9f-b0ee-2eb5df5a37d8"), -1, new System.Guid("ca1f80c4-a2e3-430a-b556-dfbceb97651a"), -1);
			Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane3 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("7493530a-30a2-4b3a-97b5-e23484adabf8"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("ca1f80c4-a2e3-430a-b556-dfbceb97651a"), -1);
			Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane4 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("04b172cf-8686-4f0e-b62e-bbe4dd3b339c"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("ca1f80c4-a2e3-430a-b556-dfbceb97651a"), -1);
			Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane5 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("ce563e1b-fa30-4ed9-8fb6-df12b8ff5223"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("ca1f80c4-a2e3-430a-b556-dfbceb97651a"), -1);
			Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane6 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("cb62c32a-0b6c-4f29-93ad-76963a4e5d65"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("ca1f80c4-a2e3-430a-b556-dfbceb97651a"), -1);
			Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane7 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("ef7302e7-48cf-429c-a0a9-ddec0129b6b3"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("ca1f80c4-a2e3-430a-b556-dfbceb97651a"), -1);
			Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane2 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedLeft, new System.Guid("6edc4aa6-45ee-43db-89b0-056d7396a0f9"));
			Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane8 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("6edb9228-5163-4921-8ac9-06d20efcc96e"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("6edc4aa6-45ee-43db-89b0-056d7396a0f9"), -1);
			Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane9 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("7c893a6e-fbb0-4fbf-9492-6c6fcc4413ba"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("6edc4aa6-45ee-43db-89b0-056d7396a0f9"), -1);
			Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane10 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("a6c4c7d4-72e0-4395-84dd-e27e4e6ea07b"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("6edc4aa6-45ee-43db-89b0-056d7396a0f9"), -1);
			Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane11 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("975f580a-551b-4d47-bfd7-22e4b01e6897"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("6edc4aa6-45ee-43db-89b0-056d7396a0f9"), -1);
			Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane12 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("95101d18-0367-4a99-a97e-e415555de34b"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("6edc4aa6-45ee-43db-89b0-056d7396a0f9"), -1);
			Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane13 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("7fa9ffa3-0442-4ad8-b4fb-c9e87618146b"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("6edc4aa6-45ee-43db-89b0-056d7396a0f9"), -1);
			Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane3 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedTop, new System.Guid("86f34a60-f2d5-497f-bc73-c4091785de8e"));
			Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane14 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("d2d25e5a-6b66-4d54-ae86-baabf89bb9f4"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("86f34a60-f2d5-497f-bc73-c4091785de8e"), -1);
			Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane15 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("aa0ff010-f91f-4b80-91ba-81d00d60f58c"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("86f34a60-f2d5-497f-bc73-c4091785de8e"), -1);
			Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane16 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("9cdd6990-95fb-41b3-8a64-7aae1527fccc"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("86f34a60-f2d5-497f-bc73-c4091785de8e"), -1);
			Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane17 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("fdb49793-d741-4d70-bc2c-cc4bce3afb33"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("86f34a60-f2d5-497f-bc73-c4091785de8e"), -1);
			Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane18 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("8f886d34-b910-430e-8074-e449d4c21460"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("86f34a60-f2d5-497f-bc73-c4091785de8e"), -1);
			Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane4 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.Floating, new System.Guid("de049042-f47f-4c9f-b0ee-2eb5df5a37d8"));
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("MainMenu");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("FileMenu");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("EditMenu");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool3 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ViewMenu");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool4 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ToolsMenu");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool5 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("HelpMenu");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("FixedSpacer");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("LoadBarcodeLabel");
			Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool1 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("LoadBarcode");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool3 = new Infragistics.Win.UltraWinToolbars.LabelTool("FixedSpacer");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool6 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("FileMenu");
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("NewCase");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("OpenCase");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CloseCase");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Print");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SaveLayout");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("LoadLayout");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool7 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ImportMenu");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool8 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ExportMenu");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Recent1");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Recent2");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Recent3");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Recent4");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Recent5");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exit");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("NewCase");
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("OpenCase");
			Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CloseCase");
			Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exit");
			Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool9 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ViewMenu");
			Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleSourceExplorer", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleMediaTree", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool3 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleBinders", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool4 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleFilteredTree", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool5 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleProperties", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool6 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleMediaViewer", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool7 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleScriptBuilder", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool8 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleScriptReview", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool9 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleTuner", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool10 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleTranscripts", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool11 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleCodes", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool12 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleSearchResults", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool13 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleObjections", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool14 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleObjectionProperties", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool15 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleVersions", "");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool10 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ViewOthersMenu");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ShowAll");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("OpenPresentation");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ReloadCase");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool16 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("LockPanes", "");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ShowAll");
			Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool11 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("HelpMenu");
			Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool17 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("HelpContents", "");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("UsersManual");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool22 = new Infragistics.Win.UltraWinToolbars.ButtonTool("OnlineSite");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool23 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ContactFTI");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool24 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AboutBox");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool25 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CheckForUpdates");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool26 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ActivateProduct");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool27 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AboutBox");
			Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool18 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleSourceExplorer", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool19 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleMediaTree", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool20 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleBinders", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool21 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleMediaViewer", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool22 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleProperties", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool23 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleScriptBuilder", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool24 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleTranscripts", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool25 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleTuner", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool26 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleSearchResults", "");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool12 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ViewOthersMenu");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool27 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleErrorMessages", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool28 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleDiagnostics", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool29 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleErrorMessages", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool30 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleDiagnostics", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool31 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("HelpContents", "");
			Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool32 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleVersions", "");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool28 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Recent1");
			Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool29 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Recent2");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool30 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Recent3");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool31 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Recent4");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool32 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Recent5");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool33 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SaveLayout");
			Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool34 = new Infragistics.Win.UltraWinToolbars.ButtonTool("LoadLayout");
			Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool13 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ToolsMenu");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool35 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RefreshTreatments");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool36 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ValidateDatabase");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool37 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TrimDatabase");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool98 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CompactDatabase");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool38 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ManagerOptions");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool39 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PresentationOptions");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool40 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PresentationToolbars");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool41 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CaseOptions");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool42 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PresentationOptions");
			Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool2 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("LoadBarcode");
			Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool4 = new Infragistics.Win.UltraWinToolbars.LabelTool("FixedSpacer");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool5 = new Infragistics.Win.UltraWinToolbars.LabelTool("LoadBarcodeLabel");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool6 = new Infragistics.Win.UltraWinToolbars.LabelTool("SpringSpacer");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool43 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RefreshTreatments");
			Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool44 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ValidateDatabase");
			Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool45 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CaseOptions");
			Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool46 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ReloadCase");
			Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool47 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Print");
			Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool48 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Find");
			Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool49 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FindNext");
			Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool14 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ImportMenu");
			Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool50 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportAsciiBinder");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool51 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportXmlBinder");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool52 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportAsciiScript");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool53 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportXmlScript");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool54 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportXmlCaseCodes");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool55 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportCodes");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool56 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportCodesDatabase");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool57 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportAsciiObjections");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool58 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportLoadFile");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool59 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportBarcodeMap");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool15 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ExportMenu");
			Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool60 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportBarcodeMap");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool61 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportXmlCaseCodes");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool62 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportAsciiObjections");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool16 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("EditMenu");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool63 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Find");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool64 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FindNext");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool65 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FastFilter");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool66 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SetFilter");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool67 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BulkUpdate");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool68 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportBarcodeMap");
			Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool69 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportBarcodeMap");
			Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool70 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportAsciiBinder");
			Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool71 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportAsciiScript");
			Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool33 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("LockPanes", "");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool72 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ManagerOptions");
			Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool73 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportLoadFile");
			Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool74 = new Infragistics.Win.UltraWinToolbars.ButtonTool("OpenPresentation");
			Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool75 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PresentationToolbars");
			Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool76 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Debug1");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool77 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Debug2");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool17 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Debug");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool78 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Debug1");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool79 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Debug2");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool80 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ActivateProduct");
			Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool81 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CheckForUpdates");
			Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool82 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ContactFTI");
			Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool83 = new Infragistics.Win.UltraWinToolbars.ButtonTool("UsersManual");
			Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool34 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleCodes", "");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool84 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportCodes");
			Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool35 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleFilteredTree", "");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool85 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SetFilter");
			Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool86 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportXmlScript");
			Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool87 = new Infragistics.Win.UltraWinToolbars.ButtonTool("OnlineSite");
			Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool88 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportXmlCaseCodes");
			Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool89 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportXmlCaseCodes");
			Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool90 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FastFilter");
			Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool91 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportCodesDatabase");
			Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool92 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportXmlBinder");
			Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool93 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BulkUpdate");
			Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool94 = new Infragistics.Win.UltraWinToolbars.ButtonTool("TrimDatabase");
			Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool36 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleObjections", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool37 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleScriptReview", "");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool95 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportAsciiObjections");
			Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool96 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportAsciiObjections");
			Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool38 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleObjectionProperties", "");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool97 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CompactDatabase");
			Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CTmaxManagerForm));
			this.m_paneTuner = new FTI.Trialmax.Panes.CTunePane();
			this.m_paneCodes = new FTI.Trialmax.Panes.CCodesPane();
			this.m_paneHelp = new FTI.Trialmax.Panes.CHelpPane();
			this.m_paneVersions = new FTI.Trialmax.Panes.CMessagePane();
			this.m_paneProperties = new FTI.Trialmax.Panes.CPropertiesPane();
			this.m_paneErrors = new FTI.Trialmax.Panes.CMessagePane();
			this.m_paneDiagnostics = new FTI.Trialmax.Panes.CMessagePane();
			this.m_paneMedia = new FTI.Trialmax.Panes.CMediaTree();
			this.m_paneObjections = new FTI.Trialmax.Panes.CObjectionsPane();
			this.m_paneBinders = new FTI.Trialmax.Panes.CBinderTree();
			this.m_paneFiltered = new FTI.Trialmax.Panes.CFilteredTree();
			this.m_paneObjectionProperties = new FTI.Trialmax.Panes.CObjectionPropertiesPane();
			this.m_paneScriptReview = new FTI.Trialmax.Panes.CScriptReviewPane();
			this.m_paneTranscripts = new FTI.Trialmax.Panes.CTranscriptPane();
            this.m_paneResults = new FTI.Trialmax.Panes.CResultsPane(this.m_paneTranscripts);

			this.m_paneViewer = new FTI.Trialmax.Panes.CMediaViewer();
            this.m_paneViewer.OnRequestPresentation += new EventHandler(m_paneViewer_OnRequestPresentation);
			this.m_paneScripts = new FTI.Trialmax.Panes.CScriptBuilder();
			this.m_paneSource = new FTI.Trialmax.Panes.CExplorerPane();
			this.m_ctrlUltraDockManager = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
			this._CTmaxManagerFormUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
			this._CTmaxManagerFormUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
			this._CTmaxManagerFormUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
			this._CTmaxManagerFormUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
			this._CTmaxManagerFormAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
			this.m_ctrlSmallImages = new System.Windows.Forms.ImageList(this.components);
			this._CTmaxManagerForm_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this.m_ctrlUltraToolbarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
			this._CTmaxManagerForm_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTmaxManagerForm_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTmaxManagerForm_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this.windowDockingArea1 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
			this.dockableWindow12 = new Infragistics.Win.UltraWinDock.DockableWindow();
			this.dockableWindow11 = new Infragistics.Win.UltraWinDock.DockableWindow();
			this.dockableWindow10 = new Infragistics.Win.UltraWinDock.DockableWindow();
			this.dockableWindow4 = new Infragistics.Win.UltraWinDock.DockableWindow();
			this.dockableWindow5 = new Infragistics.Win.UltraWinDock.DockableWindow();
			this.dockableWindow6 = new Infragistics.Win.UltraWinDock.DockableWindow();
			this.dockableWindow7 = new Infragistics.Win.UltraWinDock.DockableWindow();
			this.dockableWindow9 = new Infragistics.Win.UltraWinDock.DockableWindow();
			this.dockableWindow8 = new Infragistics.Win.UltraWinDock.DockableWindow();
			this.dockableWindow2 = new Infragistics.Win.UltraWinDock.DockableWindow();
			this.dockableWindow1 = new Infragistics.Win.UltraWinDock.DockableWindow();
			this.dockableWindow3 = new Infragistics.Win.UltraWinDock.DockableWindow();
			this.dockableWindow13 = new Infragistics.Win.UltraWinDock.DockableWindow();
			this.dockableWindow14 = new Infragistics.Win.UltraWinDock.DockableWindow();
			this.windowDockingArea2 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
			this.dockableWindow18 = new Infragistics.Win.UltraWinDock.DockableWindow();
			this.dockableWindow16 = new Infragistics.Win.UltraWinDock.DockableWindow();
			this.dockableWindow15 = new Infragistics.Win.UltraWinDock.DockableWindow();
			this.windowDockingArea4 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
			this.dockableWindow17 = new Infragistics.Win.UltraWinDock.DockableWindow();
			this.m_ctrlMainFillPanel = new System.Windows.Forms.Panel();
			this.m_tmxMovie = new FTI.Trialmax.ActiveX.CTmxMovie();
			this.m_tmxView = new FTI.Trialmax.ActiveX.CTmxView();
			this.m_ctrlScreenCapture = new AxTM_GRAB6Lib.AxTMGrab6();
			this.m_ctrlPresentation = new FTI.Trialmax.ActiveX.CTmxShare();
			this.m_ctrlFillPanel = new System.Windows.Forms.Panel();
			this.m_tmxPrint = new AxTM_PRINT6Lib.AxTMPrint6();
			this.windowDockingArea6 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraDockManager)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraToolbarManager)).BeginInit();
			this.windowDockingArea1.SuspendLayout();
			this.dockableWindow12.SuspendLayout();
			this.dockableWindow11.SuspendLayout();
			this.dockableWindow10.SuspendLayout();
			this.dockableWindow4.SuspendLayout();
			this.dockableWindow5.SuspendLayout();
			this.dockableWindow6.SuspendLayout();
			this.dockableWindow7.SuspendLayout();
			this.dockableWindow9.SuspendLayout();
			this.dockableWindow8.SuspendLayout();
			this.dockableWindow2.SuspendLayout();
			this.dockableWindow1.SuspendLayout();
			this.dockableWindow3.SuspendLayout();
			this.dockableWindow13.SuspendLayout();
			this.dockableWindow14.SuspendLayout();
			this.windowDockingArea2.SuspendLayout();
			this.dockableWindow18.SuspendLayout();
			this.dockableWindow16.SuspendLayout();
			this.dockableWindow15.SuspendLayout();
			this.windowDockingArea4.SuspendLayout();
			this.dockableWindow17.SuspendLayout();
			this.m_ctrlMainFillPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlScreenCapture)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_tmxPrint)).BeginInit();
			this.SuspendLayout();
			// 
			// m_paneTuner
			// 
			this.m_paneTuner.ApplicationOptions = null;
			this.m_paneTuner.AsyncCommandArgs = null;
			this.m_paneTuner.CaseCodes = null;
			this.m_paneTuner.CaseOptions = null;
			this.m_paneTuner.Database = null;
			this.m_paneTuner.Filtered = null;
			this.m_paneTuner.Location = new System.Drawing.Point(2, 20);
			this.m_paneTuner.MediaTypes = null;
			this.m_paneTuner.Name = "m_paneTuner";
			this.m_paneTuner.PaneId = 0;
			this.m_paneTuner.PaneName = "Tuner Pane";
			this.m_paneTuner.PaneVisible = false;
			this.m_paneTuner.PresentationOptions = null;
			this.m_paneTuner.PresentationOptionsFilename = "";
			this.m_paneTuner.RegistrationOptions = null;
			this.m_paneTuner.ReportManager = null;
			this.m_paneTuner.Size = new System.Drawing.Size(330, 214);
			this.m_paneTuner.SourceTypes = null;
			this.m_paneTuner.StationOptions = null;
			this.m_paneTuner.TabIndex = 24;
			this.m_paneTuner.TmaxClipboard = null;
			this.m_paneTuner.TmaxProductManager = null;
			this.m_paneTuner.TmaxRegistry = null;
			// 
			// m_paneCodes
			// 
			this.m_paneCodes.ApplicationOptions = null;
			this.m_paneCodes.AsyncCommandArgs = null;
			this.m_paneCodes.CaseCodes = null;
			this.m_paneCodes.CaseOptions = null;
			this.m_paneCodes.Database = null;
			this.m_paneCodes.Filtered = null;
			this.m_paneCodes.Location = new System.Drawing.Point(2, 20);
			this.m_paneCodes.MediaTypes = null;
			this.m_paneCodes.Name = "m_paneCodes";
			this.m_paneCodes.PaneId = 0;
			this.m_paneCodes.PaneName = "Codes Pane";
			this.m_paneCodes.PaneVisible = false;
			this.m_paneCodes.PresentationOptions = null;
			this.m_paneCodes.PresentationOptionsFilename = "";
			this.m_paneCodes.RegistrationOptions = null;
			this.m_paneCodes.ReportManager = null;
			this.m_paneCodes.Size = new System.Drawing.Size(198, 224);
			this.m_paneCodes.SourceTypes = null;
			this.m_paneCodes.StationOptions = null;
			this.m_paneCodes.TabIndex = 31;
			this.m_paneCodes.TmaxClipboard = null;
			this.m_paneCodes.TmaxProductManager = null;
			this.m_paneCodes.TmaxRegistry = null;
			// 
			// m_paneHelp
			// 
			this.m_paneHelp.ApplicationOptions = null;
			this.m_paneHelp.AsyncCommandArgs = null;
			this.m_paneHelp.BackColor = System.Drawing.SystemColors.Control;
			this.m_paneHelp.CaseCodes = null;
			this.m_paneHelp.CaseOptions = null;
			this.m_paneHelp.Database = null;
			this.m_paneHelp.Filtered = null;
			this.m_paneHelp.Location = new System.Drawing.Point(2, 20);
			this.m_paneHelp.MediaTypes = null;
			this.m_paneHelp.Name = "m_paneHelp";
			this.m_paneHelp.PaneId = 0;
			this.m_paneHelp.PaneName = "Help Pane";
			this.m_paneHelp.PaneVisible = false;
			this.m_paneHelp.PresentationOptions = null;
			this.m_paneHelp.PresentationOptionsFilename = "";
			this.m_paneHelp.RegistrationOptions = null;
			this.m_paneHelp.ReportManager = null;
			this.m_paneHelp.Size = new System.Drawing.Size(329, 214);
			this.m_paneHelp.SourceTypes = null;
			this.m_paneHelp.StationOptions = null;
			this.m_paneHelp.TabIndex = 29;
			this.m_paneHelp.TmaxClipboard = null;
			this.m_paneHelp.TmaxProductManager = null;
			this.m_paneHelp.TmaxRegistry = null;
			// 
			// m_paneVersions
			// 
			this.m_paneVersions.AddTop = false;
			this.m_paneVersions.ApplicationOptions = null;
			this.m_paneVersions.AsyncCommandArgs = null;
			this.m_paneVersions.CaseCodes = null;
			this.m_paneVersions.CaseOptions = null;
			this.m_paneVersions.ClearOnDblClick = false;
			this.m_paneVersions.Database = null;
			this.m_paneVersions.Filtered = null;
			this.m_paneVersions.Format = FTI.Trialmax.Controls.TmaxMessageFormats.Version;
			this.m_paneVersions.Location = new System.Drawing.Point(2, 20);
			this.m_paneVersions.MaxRows = 64;
			this.m_paneVersions.MediaTypes = null;
			this.m_paneVersions.Name = "m_paneVersions";
			this.m_paneVersions.PaneId = 0;
			this.m_paneVersions.PaneName = "Versions Pane";
			this.m_paneVersions.PaneVisible = false;
			this.m_paneVersions.PresentationOptions = null;
			this.m_paneVersions.PresentationOptionsFilename = "";
			this.m_paneVersions.RegistrationOptions = null;
			this.m_paneVersions.ReportManager = null;
			this.m_paneVersions.Size = new System.Drawing.Size(330, 214);
			this.m_paneVersions.SourceTypes = null;
			this.m_paneVersions.StationOptions = null;
			this.m_paneVersions.TabIndex = 18;
			this.m_paneVersions.TmaxClipboard = null;
			this.m_paneVersions.TmaxProductManager = null;
			this.m_paneVersions.TmaxRegistry = null;
			// 
			// m_paneProperties
			// 
			this.m_paneProperties.ApplicationOptions = null;
			this.m_paneProperties.AsyncCommandArgs = null;
			this.m_paneProperties.CaseCodes = null;
			this.m_paneProperties.CaseOptions = null;
			this.m_paneProperties.Database = null;
			this.m_paneProperties.Filtered = null;
			this.m_paneProperties.Location = new System.Drawing.Point(2, 20);
			this.m_paneProperties.MediaTypes = null;
			this.m_paneProperties.Name = "m_paneProperties";
			this.m_paneProperties.PaneId = 0;
			this.m_paneProperties.PaneName = "Properties Pane";
			this.m_paneProperties.PaneVisible = false;
			this.m_paneProperties.PresentationOptions = null;
			this.m_paneProperties.PresentationOptionsFilename = "";
			this.m_paneProperties.RegistrationOptions = null;
			this.m_paneProperties.ReportManager = null;
			this.m_paneProperties.Size = new System.Drawing.Size(207, 214);
			this.m_paneProperties.SourceTypes = null;
			this.m_paneProperties.StationOptions = null;
			this.m_paneProperties.TabIndex = 20;
			this.m_paneProperties.TmaxClipboard = null;
			this.m_paneProperties.TmaxProductManager = null;
			this.m_paneProperties.TmaxRegistry = null;
			// 
			// m_paneErrors
			// 
			this.m_paneErrors.AddTop = true;
			this.m_paneErrors.ApplicationOptions = null;
			this.m_paneErrors.AsyncCommandArgs = null;
			this.m_paneErrors.CaseCodes = null;
			this.m_paneErrors.CaseOptions = null;
			this.m_paneErrors.ClearOnDblClick = true;
			this.m_paneErrors.Database = null;
			this.m_paneErrors.Filtered = null;
			this.m_paneErrors.Format = FTI.Trialmax.Controls.TmaxMessageFormats.ErrorArgs;
			this.m_paneErrors.Location = new System.Drawing.Point(2, 20);
			this.m_paneErrors.MaxRows = 32;
			this.m_paneErrors.MediaTypes = null;
			this.m_paneErrors.Name = "m_paneErrors";
			this.m_paneErrors.PaneId = 0;
			this.m_paneErrors.PaneName = "Errors Pane";
			this.m_paneErrors.PaneVisible = false;
			this.m_paneErrors.PresentationOptions = null;
			this.m_paneErrors.PresentationOptionsFilename = "";
			this.m_paneErrors.RegistrationOptions = null;
			this.m_paneErrors.ReportManager = null;
			this.m_paneErrors.Size = new System.Drawing.Size(208, 214);
			this.m_paneErrors.SourceTypes = null;
			this.m_paneErrors.StationOptions = null;
			this.m_paneErrors.TabIndex = 15;
			this.m_paneErrors.TmaxClipboard = null;
			this.m_paneErrors.TmaxProductManager = null;
			this.m_paneErrors.TmaxRegistry = null;
			// 
			// m_paneDiagnostics
			// 
			this.m_paneDiagnostics.AddTop = true;
			this.m_paneDiagnostics.ApplicationOptions = null;
			this.m_paneDiagnostics.AsyncCommandArgs = null;
			this.m_paneDiagnostics.CaseCodes = null;
			this.m_paneDiagnostics.CaseOptions = null;
			this.m_paneDiagnostics.ClearOnDblClick = true;
			this.m_paneDiagnostics.Database = null;
			this.m_paneDiagnostics.Filtered = null;
			this.m_paneDiagnostics.Format = FTI.Trialmax.Controls.TmaxMessageFormats.DiagnosticArgs;
			this.m_paneDiagnostics.Location = new System.Drawing.Point(2, 20);
			this.m_paneDiagnostics.MaxRows = 64;
			this.m_paneDiagnostics.MediaTypes = null;
			this.m_paneDiagnostics.Name = "m_paneDiagnostics";
			this.m_paneDiagnostics.PaneId = 0;
			this.m_paneDiagnostics.PaneName = "Diagnostics Pane";
			this.m_paneDiagnostics.PaneVisible = false;
			this.m_paneDiagnostics.PresentationOptions = null;
			this.m_paneDiagnostics.PresentationOptionsFilename = "";
			this.m_paneDiagnostics.RegistrationOptions = null;
			this.m_paneDiagnostics.ReportManager = null;
			this.m_paneDiagnostics.Size = new System.Drawing.Size(197, 214);
			this.m_paneDiagnostics.SourceTypes = null;
			this.m_paneDiagnostics.StationOptions = null;
			this.m_paneDiagnostics.TabIndex = 14;
			this.m_paneDiagnostics.TmaxClipboard = null;
			this.m_paneDiagnostics.TmaxProductManager = null;
			this.m_paneDiagnostics.TmaxRegistry = null;
			// 
			// m_paneMedia
			// 
			this.m_paneMedia.AllowDrop = true;
			this.m_paneMedia.ApplicationOptions = null;
			this.m_paneMedia.AsyncCommandArgs = null;
			this.m_paneMedia.BackColor = System.Drawing.SystemColors.Window;
			this.m_paneMedia.CaseCodes = null;
			this.m_paneMedia.CaseOptions = null;
			this.m_paneMedia.CheckBoxes = false;
			this.m_paneMedia.Database = null;
			this.m_paneMedia.EnableContextMenu = true;
			this.m_paneMedia.EnableDragDrop = true;
			this.m_paneMedia.Filtered = null;
			this.m_paneMedia.ForeColor = System.Drawing.SystemColors.WindowText;
			this.m_paneMedia.Location = new System.Drawing.Point(2, 20);
			this.m_paneMedia.MediaTypes = null;
			this.m_paneMedia.Name = "m_paneMedia";
			this.m_paneMedia.PaneId = 1;
			this.m_paneMedia.PaneName = "Media Pane";
			this.m_paneMedia.PaneVisible = false;
			this.m_paneMedia.PresentationOptions = null;
			this.m_paneMedia.PresentationOptionsFilename = "";
			this.m_paneMedia.PrimaryTextMode = FTI.Shared.Trialmax.TmaxTextModes.MediaId;
			this.m_paneMedia.QuaternaryTextMode = FTI.Shared.Trialmax.TmaxTextModes.Barcode;
			this.m_paneMedia.RegistrationOptions = null;
			this.m_paneMedia.ReportManager = null;
			this.m_paneMedia.SecondaryTextMode = FTI.Shared.Trialmax.TmaxTextModes.Name;
			this.m_paneMedia.Size = new System.Drawing.Size(200, 214);
			this.m_paneMedia.SourceTypes = null;
			this.m_paneMedia.StationOptions = null;
			this.m_paneMedia.SuperNodeSize = 0;
			this.m_paneMedia.TabIndex = 13;
			this.m_paneMedia.TertiaryTextMode = FTI.Shared.Trialmax.TmaxTextModes.Barcode;
			this.m_paneMedia.TmaxClipboard = null;
			this.m_paneMedia.TmaxProductManager = null;
			this.m_paneMedia.TmaxRegistry = null;
			// 
			// m_paneObjections
			// 
			this.m_paneObjections.ApplicationOptions = null;
			this.m_paneObjections.AsyncCommandArgs = null;
			this.m_paneObjections.CaseCodes = null;
			this.m_paneObjections.CaseOptions = null;
			this.m_paneObjections.Database = null;
			this.m_paneObjections.Filtered = null;
			this.m_paneObjections.Location = new System.Drawing.Point(2, 20);
			this.m_paneObjections.MediaTypes = null;
			this.m_paneObjections.Name = "m_paneObjections";
			this.m_paneObjections.PaneId = 0;
			this.m_paneObjections.PaneName = "Objections Pane";
			this.m_paneObjections.PaneVisible = false;
			this.m_paneObjections.PresentationOptions = null;
			this.m_paneObjections.PresentationOptionsFilename = "";
			this.m_paneObjections.RegistrationOptions = null;
			this.m_paneObjections.ReportManager = null;
			this.m_paneObjections.Size = new System.Drawing.Size(192, 269);
			this.m_paneObjections.SourceTypes = null;
			this.m_paneObjections.StationOptions = null;
			this.m_paneObjections.TabIndex = 34;
			this.m_paneObjections.TmaxClipboard = null;
			this.m_paneObjections.TmaxProductManager = null;
			this.m_paneObjections.TmaxRegistry = null;
			// 
			// m_paneBinders
			// 
			this.m_paneBinders.AllowDrop = true;
			this.m_paneBinders.ApplicationOptions = null;
			this.m_paneBinders.AsyncCommandArgs = null;
			this.m_paneBinders.BackColor = System.Drawing.SystemColors.Window;
			this.m_paneBinders.CaseCodes = null;
			this.m_paneBinders.CaseOptions = null;
			this.m_paneBinders.CheckBoxes = false;
			this.m_paneBinders.Database = null;
			this.m_paneBinders.EnableContextMenu = true;
			this.m_paneBinders.EnableDragDrop = true;
			this.m_paneBinders.Filtered = null;
			this.m_paneBinders.ForeColor = System.Drawing.SystemColors.WindowText;
			this.m_paneBinders.Location = new System.Drawing.Point(2, 20);
			this.m_paneBinders.MediaTypes = null;
			this.m_paneBinders.Name = "m_paneBinders";
			this.m_paneBinders.PaneId = 3;
			this.m_paneBinders.PaneName = "Binders Pane";
			this.m_paneBinders.PaneVisible = false;
			this.m_paneBinders.PresentationOptions = null;
			this.m_paneBinders.PresentationOptionsFilename = "";
			this.m_paneBinders.PrimaryTextMode = FTI.Shared.Trialmax.TmaxTextModes.MediaId;
			this.m_paneBinders.QuaternaryTextMode = FTI.Shared.Trialmax.TmaxTextModes.Barcode;
			this.m_paneBinders.RegistrationOptions = null;
			this.m_paneBinders.ReportManager = null;
			this.m_paneBinders.SecondaryTextMode = FTI.Shared.Trialmax.TmaxTextModes.Barcode;
			this.m_paneBinders.Size = new System.Drawing.Size(147, 269);
			this.m_paneBinders.SourceTypes = null;
			this.m_paneBinders.StationOptions = null;
			this.m_paneBinders.SuperNodeSize = 0;
			this.m_paneBinders.TabIndex = 12;
			this.m_paneBinders.TertiaryTextMode = FTI.Shared.Trialmax.TmaxTextModes.Barcode;
			this.m_paneBinders.TmaxClipboard = null;
			this.m_paneBinders.TmaxProductManager = null;
			this.m_paneBinders.TmaxRegistry = null;
			// 
			// m_paneFiltered
			// 
			this.m_paneFiltered.AllowDrop = true;
			this.m_paneFiltered.ApplicationOptions = null;
			this.m_paneFiltered.AsyncCommandArgs = null;
			this.m_paneFiltered.BackColor = System.Drawing.SystemColors.Control;
			this.m_paneFiltered.CaseCodes = null;
			this.m_paneFiltered.CaseOptions = null;
			this.m_paneFiltered.CheckBoxes = false;
			this.m_paneFiltered.Database = null;
			this.m_paneFiltered.EnableContextMenu = true;
			this.m_paneFiltered.EnableDragDrop = true;
			this.m_paneFiltered.Filtered = null;
			this.m_paneFiltered.Location = new System.Drawing.Point(2, 20);
			this.m_paneFiltered.MediaTypes = null;
			this.m_paneFiltered.Name = "m_paneFiltered";
			this.m_paneFiltered.PaneId = 0;
			this.m_paneFiltered.PaneName = "Filtered Pane";
			this.m_paneFiltered.PaneVisible = false;
			this.m_paneFiltered.PresentationOptions = null;
			this.m_paneFiltered.PresentationOptionsFilename = "";
			this.m_paneFiltered.PrimaryTextMode = FTI.Shared.Trialmax.TmaxTextModes.MediaId;
			this.m_paneFiltered.QuaternaryTextMode = ((FTI.Shared.Trialmax.TmaxTextModes)((FTI.Shared.Trialmax.TmaxTextModes.Barcode | FTI.Shared.Trialmax.TmaxTextModes.Name)));
			this.m_paneFiltered.RegistrationOptions = null;
			this.m_paneFiltered.ReportManager = null;
			this.m_paneFiltered.SecondaryTextMode = FTI.Shared.Trialmax.TmaxTextModes.Barcode;
			this.m_paneFiltered.Size = new System.Drawing.Size(159, 268);
			this.m_paneFiltered.SourceTypes = null;
			this.m_paneFiltered.StationOptions = null;
			this.m_paneFiltered.SuperNodeSize = 0;
			this.m_paneFiltered.TabIndex = 31;
			this.m_paneFiltered.TertiaryTextMode = FTI.Shared.Trialmax.TmaxTextModes.Barcode;
			this.m_paneFiltered.TmaxClipboard = null;
			this.m_paneFiltered.TmaxProductManager = null;
			this.m_paneFiltered.TmaxRegistry = null;
			// 
			// m_paneObjectionProperties
			// 
			this.m_paneObjectionProperties.ApplicationOptions = null;
			this.m_paneObjectionProperties.AsyncCommandArgs = null;
			this.m_paneObjectionProperties.CaseCodes = null;
			this.m_paneObjectionProperties.CaseOptions = null;
			this.m_paneObjectionProperties.Database = null;
			this.m_paneObjectionProperties.Filtered = null;
			this.m_paneObjectionProperties.Location = new System.Drawing.Point(2, 20);
			this.m_paneObjectionProperties.MediaTypes = null;
			this.m_paneObjectionProperties.Name = "m_paneObjectionProperties";
			this.m_paneObjectionProperties.PaneId = 0;
			this.m_paneObjectionProperties.PaneName = "Objection Properties";
			this.m_paneObjectionProperties.PaneVisible = false;
			this.m_paneObjectionProperties.PresentationOptions = null;
			this.m_paneObjectionProperties.PresentationOptionsFilename = "";
			this.m_paneObjectionProperties.RegistrationOptions = null;
			this.m_paneObjectionProperties.ReportManager = null;
			this.m_paneObjectionProperties.Size = new System.Drawing.Size(182, 224);
			this.m_paneObjectionProperties.SourceTypes = null;
			this.m_paneObjectionProperties.StationOptions = null;
			this.m_paneObjectionProperties.TabIndex = 34;
			this.m_paneObjectionProperties.TmaxClipboard = null;
			this.m_paneObjectionProperties.TmaxProductManager = null;
			this.m_paneObjectionProperties.TmaxRegistry = null;
			// 
			// m_paneResults
			// 
			this.m_paneResults.ApplicationOptions = null;
			this.m_paneResults.AsyncCommandArgs = null;
			this.m_paneResults.BackColor = System.Drawing.SystemColors.Window;
			this.m_paneResults.CaseCodes = null;
			this.m_paneResults.CaseOptions = null;
			this.m_paneResults.Database = null;
			this.m_paneResults.Filtered = null;
			this.m_paneResults.Location = new System.Drawing.Point(2, 20);
			this.m_paneResults.MediaTypes = null;
			this.m_paneResults.Name = "m_paneResults";
			this.m_paneResults.PaneId = 0;
			this.m_paneResults.PaneName = "Results Pane";
			this.m_paneResults.PaneVisible = false;
			this.m_paneResults.PresentationOptions = null;
			this.m_paneResults.PresentationOptionsFilename = "";
			this.m_paneResults.RegistrationOptions = null;
			this.m_paneResults.ReportManager = null;
			this.m_paneResults.Size = new System.Drawing.Size(165, 98);
			this.m_paneResults.SourceTypes = null;
			this.m_paneResults.StationOptions = null;
			this.m_paneResults.TabIndex = 28;
			this.m_paneResults.TmaxClipboard = null;
			this.m_paneResults.TmaxProductManager = null;
			this.m_paneResults.TmaxRegistry = null;
			// 
			// m_paneScriptReview
			// 
			this.m_paneScriptReview.ApplicationOptions = null;
			this.m_paneScriptReview.AsyncCommandArgs = null;
			this.m_paneScriptReview.CaseCodes = null;
			this.m_paneScriptReview.CaseOptions = null;
			this.m_paneScriptReview.Database = null;
			this.m_paneScriptReview.Filtered = null;
			this.m_paneScriptReview.Location = new System.Drawing.Point(2, 20);
			this.m_paneScriptReview.MediaTypes = null;
			this.m_paneScriptReview.Name = "m_paneScriptReview";
			this.m_paneScriptReview.PaneId = 0;
			this.m_paneScriptReview.PaneName = "ScriptReview Pane";
			this.m_paneScriptReview.PaneVisible = false;
			this.m_paneScriptReview.PresentationOptions = null;
			this.m_paneScriptReview.PresentationOptionsFilename = "";
			this.m_paneScriptReview.RegistrationOptions = null;
			this.m_paneScriptReview.ReportManager = null;
			this.m_paneScriptReview.Size = new System.Drawing.Size(293, 98);
			this.m_paneScriptReview.SourceTypes = null;
			this.m_paneScriptReview.StationOptions = null;
			this.m_paneScriptReview.TabIndex = 34;
			this.m_paneScriptReview.TmaxClipboard = null;
			this.m_paneScriptReview.TmaxProductManager = null;
			this.m_paneScriptReview.TmaxRegistry = null;
			// 
			// m_paneTranscripts
			// 
			this.m_paneTranscripts.ApplicationOptions = null;
			this.m_paneTranscripts.AsyncCommandArgs = null;
			this.m_paneTranscripts.CaseCodes = null;
			this.m_paneTranscripts.CaseOptions = null;
			this.m_paneTranscripts.Database = null;
			this.m_paneTranscripts.Filtered = null;
			this.m_paneTranscripts.Location = new System.Drawing.Point(2, 20);
			this.m_paneTranscripts.MediaTypes = null;
			this.m_paneTranscripts.Name = "m_paneTranscripts";
			this.m_paneTranscripts.PaneId = 0;
			this.m_paneTranscripts.PaneName = "Transcripts Pane";
			this.m_paneTranscripts.PaneVisible = false;
			this.m_paneTranscripts.PresentationOptions = null;
			this.m_paneTranscripts.PresentationOptionsFilename = "";
			this.m_paneTranscripts.RegistrationOptions = null;
			this.m_paneTranscripts.ReportManager = null;
			this.m_paneTranscripts.Size = new System.Drawing.Size(293, 98);
			this.m_paneTranscripts.SourceTypes = null;
			this.m_paneTranscripts.StationOptions = null;
			this.m_paneTranscripts.TabIndex = 23;
			this.m_paneTranscripts.TmaxClipboard = null;
			this.m_paneTranscripts.TmaxProductManager = null;
			this.m_paneTranscripts.TmaxRegistry = null;
			// 
			// m_paneViewer
			// 
			this.m_paneViewer.ApplicationOptions = null;
			this.m_paneViewer.AsyncCommandArgs = null;
			this.m_paneViewer.CaseCodes = null;
			this.m_paneViewer.CaseOptions = null;
			this.m_paneViewer.Database = null;
			this.m_paneViewer.Filtered = null;
			this.m_paneViewer.Location = new System.Drawing.Point(2, 20);
			this.m_paneViewer.MediaTypes = null;
			this.m_paneViewer.Name = "m_paneViewer";
			this.m_paneViewer.PaneId = 0;
			this.m_paneViewer.PaneName = "Viewer Pane";
			this.m_paneViewer.PaneVisible = false;
			this.m_paneViewer.PresentationOptions = null;
			this.m_paneViewer.PresentationOptionsFilename = "";
			this.m_paneViewer.RegistrationOptions = null;
			this.m_paneViewer.ReportManager = null;
			this.m_paneViewer.Size = new System.Drawing.Size(199, 214);
			this.m_paneViewer.SourceTypes = null;
			this.m_paneViewer.StationOptions = null;
			this.m_paneViewer.TabIndex = 21;
			this.m_paneViewer.TmaxClipboard = null;
			this.m_paneViewer.TmaxProductManager = null;
			this.m_paneViewer.TmaxRegistry = null;
			// 
			// m_paneScripts
			// 
			this.m_paneScripts.AllowDrop = true;
			this.m_paneScripts.ApplicationOptions = null;
			this.m_paneScripts.AsyncCommandArgs = null;
			this.m_paneScripts.CaseCodes = null;
			this.m_paneScripts.CaseOptions = null;
			this.m_paneScripts.Columns = 2;
			this.m_paneScripts.Database = null;
			this.m_paneScripts.Filtered = null;
			this.m_paneScripts.Location = new System.Drawing.Point(2, 20);
			this.m_paneScripts.MediaTypes = null;
			this.m_paneScripts.Name = "m_paneScripts";
			this.m_paneScripts.PaneId = 0;
			this.m_paneScripts.PaneName = "Scripts Pane";
			this.m_paneScripts.PaneVisible = false;
			this.m_paneScripts.PresentationOptions = null;
			this.m_paneScripts.PresentationOptionsFilename = "";
			this.m_paneScripts.RegistrationOptions = null;
			this.m_paneScripts.ReportManager = null;
			this.m_paneScripts.SceneSeparatorSize = 8;
			this.m_paneScripts.SceneTextMode = FTI.Shared.Trialmax.TmaxTextModes.Barcode;
			this.m_paneScripts.Size = new System.Drawing.Size(258, 98);
			this.m_paneScripts.SourceTypes = null;
			this.m_paneScripts.StationOptions = null;
			this.m_paneScripts.StatusTextMode = FTI.Shared.Trialmax.TmaxTextModes.Barcode;
			this.m_paneScripts.TabIndex = 22;
			this.m_paneScripts.TmaxClipboard = null;
			this.m_paneScripts.TmaxProductManager = null;
			this.m_paneScripts.TmaxRegistry = null;
			// 
			// m_paneSource
			// 
			this.m_paneSource.ApplicationOptions = null;
			this.m_paneSource.AsyncCommandArgs = null;
			this.m_paneSource.BackColor = System.Drawing.SystemColors.Control;
			this.m_paneSource.CaseCodes = null;
			this.m_paneSource.CaseOptions = null;
			this.m_paneSource.CtrlWidthRatio = 0.5F;
			this.m_paneSource.Database = null;
			this.m_paneSource.Filtered = null;
			this.m_paneSource.ForeColor = System.Drawing.SystemColors.WindowText;
			this.m_paneSource.Location = new System.Drawing.Point(2, 20);
			this.m_paneSource.MediaTypes = null;
			this.m_paneSource.Name = "m_paneSource";
			this.m_paneSource.PaneId = 2;
			this.m_paneSource.PaneName = "Source Explorer";
			this.m_paneSource.PaneVisible = false;
			this.m_paneSource.PresentationOptions = null;
			this.m_paneSource.PresentationOptionsFilename = "";
			this.m_paneSource.RegistrationOptions = null;
			this.m_paneSource.ReportManager = null;
			this.m_paneSource.ShowHidden = false;
			this.m_paneSource.ShowTree = true;
			this.m_paneSource.Size = new System.Drawing.Size(293, 98);
			this.m_paneSource.SourceType = FTI.Shared.Trialmax.RegSourceTypes.AllFiles;
			this.m_paneSource.SourceTypes = null;
			this.m_paneSource.StationOptions = null;
			this.m_paneSource.TabIndex = 0;
			this.m_paneSource.TmaxClipboard = null;
			this.m_paneSource.TmaxProductManager = null;
			this.m_paneSource.TmaxRegistry = null;
			// 
			// m_ctrlUltraDockManager
			// 
			this.m_ctrlUltraDockManager.AnimationEnabled = false;
			this.m_ctrlUltraDockManager.AutoHideDelay = 1000;
			this.m_ctrlUltraDockManager.DefaultGroupSettings.TabStyle = Infragistics.Win.UltraWinTabs.TabStyle.Excel;
			appearance1.FontData.BoldAsString = "True";
			this.m_ctrlUltraDockManager.DefaultPaneSettings.ActiveTabAppearance = appearance1;
			this.m_ctrlUltraDockManager.DefaultPaneSettings.AllowPin = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraDockManager.DefaultPaneSettings.BorderStylePane = Infragistics.Win.UIElementBorderStyle.Etched;
			appearance2.ForeColor = System.Drawing.SystemColors.ControlText;
			appearance2.ForeColorDisabled = System.Drawing.SystemColors.ControlText;
			this.m_ctrlUltraDockManager.DefaultPaneSettings.TabAppearance = appearance2;
			dockAreaPane1.ChildPaneStyle = Infragistics.Win.UltraWinDock.ChildPaneStyle.TabGroup;
			dockAreaPane1.DockedBefore = new System.Guid("6edc4aa6-45ee-43db-89b0-056d7396a0f9");
			dockableControlPane1.Control = this.m_paneTuner;
			dockableControlPane1.Key = "Tuner";
			dockableControlPane1.OriginalControlBounds = new System.Drawing.Rectangle(108, 10, 140, 44);
			dockableControlPane1.Size = new System.Drawing.Size(285, 240);
			dockableControlPane1.Text = "Tuner";
			dockableControlPane1.TextTab = "Tuner";
			dockableControlPane2.Control = this.m_paneCodes;
			dockableControlPane2.Key = "Codes";
			dockableControlPane2.OriginalControlBounds = new System.Drawing.Rectangle(128, 128, 316, 288);
			dockableControlPane2.Size = new System.Drawing.Size(285, 240);
			dockableControlPane2.Text = "Fielded Data";
			dockableControlPane2.TextTab = "Fielded";
			dockableControlPane2.ToolTipTab = "";
			dockableControlPane3.Control = this.m_paneHelp;
			dockableControlPane3.Key = "Help";
			dockableControlPane3.OriginalControlBounds = new System.Drawing.Rectangle(92, 76, 140, 44);
			dockableControlPane3.Size = new System.Drawing.Size(285, 240);
			dockableControlPane3.Text = "Help";
			dockableControlPane3.TextTab = "Help";
			dockableControlPane4.Control = this.m_paneVersions;
			dockableControlPane4.Key = "Versions";
			dockableControlPane4.OriginalControlBounds = new System.Drawing.Rectangle(8, 228, 140, 44);
			dockableControlPane4.Size = new System.Drawing.Size(285, 240);
			dockableControlPane4.Text = "Versions";
			dockableControlPane4.TextTab = "Versions";
			dockableControlPane5.Control = this.m_paneProperties;
			dockableControlPane5.Key = "Properties";
			dockableControlPane5.OriginalControlBounds = new System.Drawing.Rectangle(158, 8, 140, 44);
			dockableControlPane5.Size = new System.Drawing.Size(285, 240);
			dockableControlPane5.Text = "Properties";
			dockableControlPane5.TextTab = "Properties";
			dockableControlPane6.Control = this.m_paneErrors;
			dockableControlPane6.Key = "Errors";
			dockableControlPane6.OriginalControlBounds = new System.Drawing.Rectangle(8, 118, 140, 44);
			dockableControlPane6.Size = new System.Drawing.Size(285, 240);
			dockableControlPane6.Text = "Error Messages";
			dockableControlPane6.TextTab = "Errors";
			dockableControlPane7.Control = this.m_paneDiagnostics;
			dockableControlPane7.Key = "Diagnostics";
			dockableControlPane7.OriginalControlBounds = new System.Drawing.Rectangle(8, 173, 140, 44);
			dockableControlPane7.Size = new System.Drawing.Size(285, 240);
			dockableControlPane7.Text = "Diagnostic Messages";
			dockableControlPane7.TextTab = "Diagnostics";
			dockAreaPane1.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane1,
            dockableControlPane2,
            dockableControlPane3,
            dockableControlPane4,
            dockableControlPane5,
            dockableControlPane6,
            dockableControlPane7});
			dockAreaPane1.SelectedTabIndex = 1;
			dockAreaPane1.Size = new System.Drawing.Size(206, 272);
			dockAreaPane2.ChildPaneStyle = Infragistics.Win.UltraWinDock.ChildPaneStyle.TabGroup;
			dockAreaPane2.DockedBefore = new System.Guid("86f34a60-f2d5-497f-bc73-c4091785de8e");
			dockableControlPane8.Control = this.m_paneMedia;
			dockableControlPane8.Key = "Media";
			dockableControlPane8.OriginalControlBounds = new System.Drawing.Rectangle(8, 63, 140, 44);
			dockableControlPane8.Size = new System.Drawing.Size(163, 240);
			dockableControlPane8.Text = "Media Tree";
			dockableControlPane8.TextTab = "Media";
			dockableControlPane9.Control = this.m_paneObjections;
			dockableControlPane9.OriginalControlBounds = new System.Drawing.Rectangle(90, 12, 226, 230);
			dockableControlPane9.Size = new System.Drawing.Size(100, 100);
			dockableControlPane9.Text = "Objections";
			dockableControlPane10.Control = this.m_paneBinders;
			dockableControlPane10.Key = "Binders";
			dockableControlPane10.OriginalControlBounds = new System.Drawing.Rectangle(8, 8, 140, 44);
			dockableControlPane10.Size = new System.Drawing.Size(163, 240);
			dockableControlPane10.Text = "Binder Tree";
			dockableControlPane10.TextTab = "Binders";
			dockableControlPane11.Control = this.m_paneFiltered;
			dockableControlPane11.Key = "FilteredTree";
			dockableControlPane11.OriginalControlBounds = new System.Drawing.Rectangle(36, 64, 92, 48);
			dockableControlPane11.Size = new System.Drawing.Size(163, 240);
			dockableControlPane11.Text = "Filtered Tree";
			dockableControlPane11.TextTab = "Filtered";
			dockableControlPane12.Control = this.m_paneObjectionProperties;
			dockableControlPane12.OriginalControlBounds = new System.Drawing.Rectangle(216, 8, 79, 98);
			dockableControlPane12.Size = new System.Drawing.Size(100, 100);
			dockableControlPane12.Text = "Objection Props";
			dockableControlPane12.TextTab = "Objection Props";
			dockableControlPane12.ToolTipTab = "";
			dockableControlPane13.Control = this.m_paneResults;
			dockableControlPane13.Key = "Results";
			dockableControlPane13.OriginalControlBounds = new System.Drawing.Rectangle(132, 48, 32, 44);
			dockableControlPane13.Size = new System.Drawing.Size(163, 240);
			dockableControlPane13.Text = "Search Results";
			dockableControlPane13.TextTab = "Results";
			dockAreaPane2.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane8,
            dockableControlPane9,
            dockableControlPane10,
            dockableControlPane11,
            dockableControlPane12,
            dockableControlPane13});
			dockAreaPane2.SelectedTabIndex = 4;
			dockAreaPane2.Size = new System.Drawing.Size(190, 272);
			dockAreaPane3.ChildPaneStyle = Infragistics.Win.UltraWinDock.ChildPaneStyle.TabGroup;
			dockAreaPane3.DockedBefore = new System.Guid("de049042-f47f-4c9f-b0ee-2eb5df5a37d8");
			dockableControlPane14.Control = this.m_paneScriptReview;
			dockableControlPane14.OriginalControlBounds = new System.Drawing.Rectangle(-41, 28, 208, 90);
			dockableControlPane14.Size = new System.Drawing.Size(100, 100);
			dockableControlPane14.Text = "Script Review";
			dockableControlPane14.TextTab = "Script Review";
			dockableControlPane15.Control = this.m_paneTranscripts;
			dockableControlPane15.Key = "Transcripts";
			dockableControlPane15.OriginalControlBounds = new System.Drawing.Rectangle(32, 64, 140, 44);
			dockableControlPane15.Size = new System.Drawing.Size(142, 122);
			dockableControlPane15.Text = "Transcripts";
			dockableControlPane15.TextTab = "Transcripts";
			dockableControlPane16.Control = this.m_paneViewer;
			dockableControlPane16.Key = "Viewer";
			dockableControlPane16.OriginalControlBounds = new System.Drawing.Rectangle(184, 96, 140, 44);
			dockableControlPane16.Size = new System.Drawing.Size(142, 122);
			dockableControlPane16.Text = "Media Viewer";
			dockableControlPane16.TextTab = "Viewer";
			dockableControlPane17.Control = this.m_paneScripts;
			dockableControlPane17.Key = "Scripts";
			dockableControlPane17.OriginalControlBounds = new System.Drawing.Rectangle(156, 44, 140, 68);
			dockableControlPane17.Size = new System.Drawing.Size(142, 122);
			dockableControlPane17.Text = "Script Builder";
			dockableControlPane17.TextTab = "Scripts";
			dockableControlPane18.Control = this.m_paneSource;
			dockableControlPane18.Key = "Source";
			dockableControlPane18.OriginalControlBounds = new System.Drawing.Rectangle(160, 164, 140, 44);
			dockableControlPane18.Size = new System.Drawing.Size(142, 122);
			dockableControlPane18.Text = "Source Explorer";
			dockableControlPane18.TextTab = "Source";
			dockAreaPane3.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane14,
            dockableControlPane15,
            dockableControlPane16,
            dockableControlPane17,
            dockableControlPane18});
			dockAreaPane3.SelectedTabIndex = 3;
			dockAreaPane3.Size = new System.Drawing.Size(266, 146);
			dockAreaPane4.FloatingLocation = new System.Drawing.Point(485, 156);
			dockAreaPane4.Size = new System.Drawing.Size(100, 100);
			this.m_ctrlUltraDockManager.DockAreas.AddRange(new Infragistics.Win.UltraWinDock.DockAreaPane[] {
            dockAreaPane1,
            dockAreaPane2,
            dockAreaPane3,
            dockAreaPane4});
			this.m_ctrlUltraDockManager.DragWindowStyle = Infragistics.Win.UltraWinDock.DragWindowStyle.LayeredWindow;
			this.m_ctrlUltraDockManager.HostControl = this;
			this.m_ctrlUltraDockManager.ShowPinButton = false;
			this.m_ctrlUltraDockManager.PaneActivate += new Infragistics.Win.UltraWinDock.ControlPaneEventHandler(this.OnUltraPaneActivate);
			this.m_ctrlUltraDockManager.PaneDeactivate += new Infragistics.Win.UltraWinDock.ControlPaneEventHandler(this.OnUltraPaneDectivate);
			this.m_ctrlUltraDockManager.AfterPaneButtonClick += new Infragistics.Win.UltraWinDock.PaneButtonEventHandler(this.OnUltraPaneButtonClick);
			// 
			// _CTmaxManagerFormUnpinnedTabAreaLeft
			// 
			this._CTmaxManagerFormUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this._CTmaxManagerFormUnpinnedTabAreaLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._CTmaxManagerFormUnpinnedTabAreaLeft.Location = new System.Drawing.Point(0, 24);
			this._CTmaxManagerFormUnpinnedTabAreaLeft.Name = "_CTmaxManagerFormUnpinnedTabAreaLeft";
			this._CTmaxManagerFormUnpinnedTabAreaLeft.Owner = this.m_ctrlUltraDockManager;
			this._CTmaxManagerFormUnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 317);
			this._CTmaxManagerFormUnpinnedTabAreaLeft.TabIndex = 1;
			// 
			// _CTmaxManagerFormUnpinnedTabAreaRight
			// 
			this._CTmaxManagerFormUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
			this._CTmaxManagerFormUnpinnedTabAreaRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._CTmaxManagerFormUnpinnedTabAreaRight.Location = new System.Drawing.Point(672, 24);
			this._CTmaxManagerFormUnpinnedTabAreaRight.Name = "_CTmaxManagerFormUnpinnedTabAreaRight";
			this._CTmaxManagerFormUnpinnedTabAreaRight.Owner = this.m_ctrlUltraDockManager;
			this._CTmaxManagerFormUnpinnedTabAreaRight.Size = new System.Drawing.Size(0, 317);
			this._CTmaxManagerFormUnpinnedTabAreaRight.TabIndex = 2;
			// 
			// _CTmaxManagerFormUnpinnedTabAreaTop
			// 
			this._CTmaxManagerFormUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
			this._CTmaxManagerFormUnpinnedTabAreaTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._CTmaxManagerFormUnpinnedTabAreaTop.Location = new System.Drawing.Point(0, 24);
			this._CTmaxManagerFormUnpinnedTabAreaTop.Name = "_CTmaxManagerFormUnpinnedTabAreaTop";
			this._CTmaxManagerFormUnpinnedTabAreaTop.Owner = this.m_ctrlUltraDockManager;
			this._CTmaxManagerFormUnpinnedTabAreaTop.Size = new System.Drawing.Size(672, 0);
			this._CTmaxManagerFormUnpinnedTabAreaTop.TabIndex = 3;
			// 
			// _CTmaxManagerFormUnpinnedTabAreaBottom
			// 
			this._CTmaxManagerFormUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._CTmaxManagerFormUnpinnedTabAreaBottom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._CTmaxManagerFormUnpinnedTabAreaBottom.Location = new System.Drawing.Point(0, 341);
			this._CTmaxManagerFormUnpinnedTabAreaBottom.Name = "_CTmaxManagerFormUnpinnedTabAreaBottom";
			this._CTmaxManagerFormUnpinnedTabAreaBottom.Owner = this.m_ctrlUltraDockManager;
			this._CTmaxManagerFormUnpinnedTabAreaBottom.Size = new System.Drawing.Size(672, 0);
			this._CTmaxManagerFormUnpinnedTabAreaBottom.TabIndex = 4;
			// 
			// _CTmaxManagerFormAutoHideControl
			// 
			this._CTmaxManagerFormAutoHideControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._CTmaxManagerFormAutoHideControl.Location = new System.Drawing.Point(0, 0);
			this._CTmaxManagerFormAutoHideControl.Name = "_CTmaxManagerFormAutoHideControl";
			this._CTmaxManagerFormAutoHideControl.Owner = this.m_ctrlUltraDockManager;
			this._CTmaxManagerFormAutoHideControl.Size = new System.Drawing.Size(0, 0);
			this._CTmaxManagerFormAutoHideControl.TabIndex = 5;
			// 
			// m_ctrlSmallImages
			// 
			this.m_ctrlSmallImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlSmallImages.ImageStream")));
			this.m_ctrlSmallImages.TransparentColor = System.Drawing.Color.Magenta;
			this.m_ctrlSmallImages.Images.SetKeyName(0, "");
			this.m_ctrlSmallImages.Images.SetKeyName(1, "");
			this.m_ctrlSmallImages.Images.SetKeyName(2, "");
			this.m_ctrlSmallImages.Images.SetKeyName(3, "");
			this.m_ctrlSmallImages.Images.SetKeyName(4, "");
			this.m_ctrlSmallImages.Images.SetKeyName(5, "");
			this.m_ctrlSmallImages.Images.SetKeyName(6, "");
			this.m_ctrlSmallImages.Images.SetKeyName(7, "");
			this.m_ctrlSmallImages.Images.SetKeyName(8, "");
			this.m_ctrlSmallImages.Images.SetKeyName(9, "");
			this.m_ctrlSmallImages.Images.SetKeyName(10, "");
			this.m_ctrlSmallImages.Images.SetKeyName(11, "");
			this.m_ctrlSmallImages.Images.SetKeyName(12, "");
			this.m_ctrlSmallImages.Images.SetKeyName(13, "");
			this.m_ctrlSmallImages.Images.SetKeyName(14, "");
			this.m_ctrlSmallImages.Images.SetKeyName(15, "");
			this.m_ctrlSmallImages.Images.SetKeyName(16, "");
			this.m_ctrlSmallImages.Images.SetKeyName(17, "");
			this.m_ctrlSmallImages.Images.SetKeyName(18, "");
			this.m_ctrlSmallImages.Images.SetKeyName(19, "");
			this.m_ctrlSmallImages.Images.SetKeyName(20, "");
			this.m_ctrlSmallImages.Images.SetKeyName(21, "");
			this.m_ctrlSmallImages.Images.SetKeyName(22, "");
			this.m_ctrlSmallImages.Images.SetKeyName(23, "");
			this.m_ctrlSmallImages.Images.SetKeyName(24, "");
			this.m_ctrlSmallImages.Images.SetKeyName(25, "");
			this.m_ctrlSmallImages.Images.SetKeyName(26, "");
			this.m_ctrlSmallImages.Images.SetKeyName(27, "");
			this.m_ctrlSmallImages.Images.SetKeyName(28, "");
			this.m_ctrlSmallImages.Images.SetKeyName(29, "");
			this.m_ctrlSmallImages.Images.SetKeyName(30, "");
			this.m_ctrlSmallImages.Images.SetKeyName(31, "");
			this.m_ctrlSmallImages.Images.SetKeyName(32, "");
			this.m_ctrlSmallImages.Images.SetKeyName(33, "");
			this.m_ctrlSmallImages.Images.SetKeyName(34, "");
			this.m_ctrlSmallImages.Images.SetKeyName(35, "");
			this.m_ctrlSmallImages.Images.SetKeyName(36, "");
			this.m_ctrlSmallImages.Images.SetKeyName(37, "");
			this.m_ctrlSmallImages.Images.SetKeyName(38, "");
			this.m_ctrlSmallImages.Images.SetKeyName(39, "");
			this.m_ctrlSmallImages.Images.SetKeyName(40, "");
			this.m_ctrlSmallImages.Images.SetKeyName(41, "");
			this.m_ctrlSmallImages.Images.SetKeyName(42, "");
			this.m_ctrlSmallImages.Images.SetKeyName(43, "trim_database.bmp");
			this.m_ctrlSmallImages.Images.SetKeyName(44, "objection_import.bmp");
			this.m_ctrlSmallImages.Images.SetKeyName(45, "objection_export.bmp");
			this.m_ctrlSmallImages.Images.SetKeyName(46, "compact_database.bmp");
			// 
			// _CTmaxManagerForm_Toolbars_Dock_Area_Top
			// 
			this._CTmaxManagerForm_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmaxManagerForm_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
			this._CTmaxManagerForm_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
			this._CTmaxManagerForm_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxManagerForm_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
			this._CTmaxManagerForm_Toolbars_Dock_Area_Top.Name = "_CTmaxManagerForm_Toolbars_Dock_Area_Top";
			this._CTmaxManagerForm_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(672, 69);
			this._CTmaxManagerForm_Toolbars_Dock_Area_Top.ToolbarsManager = this.m_ctrlUltraToolbarManager;
			// 
			// m_ctrlUltraToolbarManager
			// 
			appearance3.AlphaLevel = ((short)(211));
			this.m_ctrlUltraToolbarManager.Appearance = appearance3;
			this.m_ctrlUltraToolbarManager.DesignerFlags = 1;
			this.m_ctrlUltraToolbarManager.DockWithinContainer = this;
			this.m_ctrlUltraToolbarManager.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
			this.m_ctrlUltraToolbarManager.ImageListSmall = this.m_ctrlSmallImages;
			this.m_ctrlUltraToolbarManager.ShowFullMenusDelay = 500;
			this.m_ctrlUltraToolbarManager.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.Office2003;
			ultraToolbar1.DockedColumn = 0;
			ultraToolbar1.DockedRow = 0;
			ultraToolbar1.IsMainMenuBar = true;
			labelTool1.InstanceProps.Width = 137;
			labelTool2.InstanceProps.Width = 47;
			textBoxTool1.InstanceProps.Width = 216;
			labelTool3.InstanceProps.Width = 24;
			ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool1,
            popupMenuTool2,
            popupMenuTool3,
            popupMenuTool4,
            popupMenuTool5,
            labelTool1,
            labelTool2,
            textBoxTool1,
            labelTool3});
			ultraToolbar1.Settings.FillEntireRow = Infragistics.Win.DefaultableBoolean.True;
			ultraToolbar1.Text = "Main Menu";
			this.m_ctrlUltraToolbarManager.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
			popupMenuTool6.Settings.IsSideStripVisible = Infragistics.Win.DefaultableBoolean.True;
			appearance4.BackColor = System.Drawing.Color.WhiteSmoke;
			appearance4.BackColor2 = System.Drawing.Color.Navy;
			appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			appearance4.FontData.ItalicAsString = "True";
			appearance4.FontData.Name = "Microsoft Sans Serif";
			appearance4.FontData.SizeInPoints = 16F;
			appearance4.TextVAlignAsString = "Middle";
			popupMenuTool6.Settings.SideStripAppearance = appearance4;
			popupMenuTool6.Settings.SideStripText = "Trialmax";
			popupMenuTool6.Settings.SideStripWidth = 24;
			appearance5.BackColor = System.Drawing.Color.Transparent;
			popupMenuTool6.Settings.ToolAppearance = appearance5;
			popupMenuTool6.SharedProps.Caption = "&File";
			popupMenuTool6.SharedProps.Category = "File";
			buttonTool4.InstanceProps.IsFirstInGroup = true;
			buttonTool5.InstanceProps.IsFirstInGroup = true;
			popupMenuTool7.InstanceProps.IsFirstInGroup = true;
			buttonTool7.InstanceProps.IsFirstInGroup = true;
			buttonTool12.InstanceProps.IsFirstInGroup = true;
			popupMenuTool6.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool2,
            buttonTool3,
            buttonTool4,
            buttonTool5,
            buttonTool6,
            popupMenuTool7,
            popupMenuTool8,
            buttonTool7,
            buttonTool8,
            buttonTool9,
            buttonTool10,
            buttonTool11,
            buttonTool12});
			appearance6.Image = 0;
			buttonTool13.SharedProps.AppearancesSmall.Appearance = appearance6;
			buttonTool13.SharedProps.Caption = "&New ...";
			buttonTool13.SharedProps.Category = "File";
			buttonTool13.SharedProps.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
			appearance7.Image = 1;
			buttonTool14.SharedProps.AppearancesSmall.Appearance = appearance7;
			buttonTool14.SharedProps.Caption = "&Open ...";
			buttonTool14.SharedProps.Category = "File";
			buttonTool14.SharedProps.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			appearance8.Image = 4;
			buttonTool15.SharedProps.AppearancesSmall.Appearance = appearance8;
			buttonTool15.SharedProps.Caption = "&Close ...";
			buttonTool15.SharedProps.Category = "File";
			appearance9.Image = 5;
			buttonTool16.SharedProps.AppearancesSmall.Appearance = appearance9;
			buttonTool16.SharedProps.Caption = "E&xit";
			buttonTool16.SharedProps.Category = "File";
			buttonTool16.SharedProps.Shortcut = System.Windows.Forms.Shortcut.AltF4;
			appearance10.BackColor = System.Drawing.Color.Transparent;
			popupMenuTool9.Settings.ToolAppearance = appearance10;
			popupMenuTool9.SharedProps.Caption = "&View";
			popupMenuTool9.SharedProps.Category = "View";
			stateButtonTool1.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool2.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool3.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool4.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool5.InstanceProps.IsFirstInGroup = true;
			stateButtonTool5.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool6.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool7.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool8.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool9.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool10.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool11.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool12.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool13.InstanceProps.IsFirstInGroup = true;
			stateButtonTool13.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool14.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool15.InstanceProps.IsFirstInGroup = true;
			stateButtonTool15.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			buttonTool18.InstanceProps.IsFirstInGroup = true;
			buttonTool19.InstanceProps.IsFirstInGroup = true;
			stateButtonTool16.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			popupMenuTool9.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool1,
            stateButtonTool2,
            stateButtonTool3,
            stateButtonTool4,
            stateButtonTool5,
            stateButtonTool6,
            stateButtonTool7,
            stateButtonTool8,
            stateButtonTool9,
            stateButtonTool10,
            stateButtonTool11,
            stateButtonTool12,
            stateButtonTool13,
            stateButtonTool14,
            stateButtonTool15,
            popupMenuTool10,
            buttonTool17,
            buttonTool18,
            buttonTool19,
            stateButtonTool16});
			appearance11.Image = 6;
			buttonTool20.SharedProps.AppearancesSmall.Appearance = appearance11;
			buttonTool20.SharedProps.Caption = "Show &All";
			buttonTool20.SharedProps.Category = "View";
			appearance12.BackColor = System.Drawing.Color.Transparent;
			popupMenuTool11.Settings.ToolAppearance = appearance12;
			popupMenuTool11.SharedProps.Caption = "&Help";
			popupMenuTool11.SharedProps.Category = "Help";
			stateButtonTool17.InstanceProps.IsFirstInGroup = true;
			buttonTool25.InstanceProps.IsFirstInGroup = true;
			popupMenuTool11.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool17,
            buttonTool21,
            buttonTool22,
            buttonTool23,
            buttonTool24,
            buttonTool25,
            buttonTool26});
			appearance13.Image = 3;
			buttonTool27.SharedProps.AppearancesSmall.Appearance = appearance13;
			buttonTool27.SharedProps.Caption = "&About ...";
			buttonTool27.SharedProps.Category = "Help";
			stateButtonTool18.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool18.SharedProps.Caption = "Source &Explorer";
			stateButtonTool18.SharedProps.Category = "View";
			stateButtonTool19.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool19.SharedProps.Caption = "&Media Tree";
			stateButtonTool19.SharedProps.Category = "View";
			stateButtonTool20.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool20.SharedProps.Caption = "&Binders";
			stateButtonTool20.SharedProps.Category = "View";
			stateButtonTool21.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool21.SharedProps.Caption = "Media &Viewer";
			stateButtonTool21.SharedProps.Category = "View";
			stateButtonTool21.SharedProps.Shortcut = System.Windows.Forms.Shortcut.F8;
			stateButtonTool22.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool22.SharedProps.Caption = "&Properties";
			stateButtonTool22.SharedProps.Category = "View";
			stateButtonTool22.SharedProps.Shortcut = System.Windows.Forms.Shortcut.F4;
			stateButtonTool23.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool23.SharedProps.Caption = "&Script Builder";
			stateButtonTool23.SharedProps.Category = "View";
			stateButtonTool23.SharedProps.Shortcut = System.Windows.Forms.Shortcut.F6;
			stateButtonTool24.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool24.SharedProps.Caption = "Tra&nscripts";
			stateButtonTool24.SharedProps.Category = "View";
			stateButtonTool25.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool25.SharedProps.Caption = "&Tuner";
			stateButtonTool25.SharedProps.Category = "View";
			stateButtonTool25.SharedProps.Shortcut = System.Windows.Forms.Shortcut.F7;
			stateButtonTool26.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool26.SharedProps.Caption = "Sear&ch Results";
			stateButtonTool26.SharedProps.Category = "View";
			popupMenuTool12.SharedProps.Caption = "Others";
			popupMenuTool12.SharedProps.Category = "View";
			stateButtonTool27.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool28.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			popupMenuTool12.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool27,
            stateButtonTool28});
			stateButtonTool29.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool29.SharedProps.Caption = "Error Messages";
			stateButtonTool29.SharedProps.Category = "View";
			stateButtonTool30.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool30.SharedProps.Caption = "Diagnostics";
			stateButtonTool30.SharedProps.Category = "View";
			appearance14.Image = 2;
			stateButtonTool31.SharedProps.AppearancesSmall.Appearance = appearance14;
			stateButtonTool31.SharedProps.Caption = "&Contents";
			stateButtonTool31.SharedProps.Category = "Help";
			stateButtonTool31.SharedProps.Shortcut = System.Windows.Forms.Shortcut.F1;
			stateButtonTool32.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool32.SharedProps.Caption = "Version &Information";
			stateButtonTool32.SharedProps.Category = "View";
			appearance15.Image = 7;
			buttonTool28.SharedProps.AppearancesSmall.Appearance = appearance15;
			buttonTool28.SharedProps.Caption = "Recent1";
			buttonTool28.SharedProps.Category = "File";
			buttonTool28.SharedProps.Shortcut = System.Windows.Forms.Shortcut.CtrlL;
			buttonTool28.SharedProps.Visible = false;
			buttonTool29.SharedProps.Caption = "Recent2";
			buttonTool29.SharedProps.Category = "File";
			buttonTool29.SharedProps.Visible = false;
			buttonTool30.SharedProps.Caption = "Recent3";
			buttonTool30.SharedProps.Category = "File";
			buttonTool30.SharedProps.Visible = false;
			buttonTool31.SharedProps.Caption = "Recent4";
			buttonTool31.SharedProps.Category = "File";
			buttonTool31.SharedProps.Visible = false;
			buttonTool32.SharedProps.Caption = "Recent5";
			buttonTool32.SharedProps.Category = "File";
			buttonTool32.SharedProps.Visible = false;
			appearance16.Image = 8;
			buttonTool33.SharedProps.AppearancesSmall.Appearance = appearance16;
			buttonTool33.SharedProps.Caption = "Save Screen Layout ...";
			buttonTool33.SharedProps.Category = "File";
			appearance17.Image = 9;
			buttonTool34.SharedProps.AppearancesSmall.Appearance = appearance17;
			buttonTool34.SharedProps.Caption = "Load Screen Layout ...";
			buttonTool34.SharedProps.Category = "File";
			popupMenuTool13.SharedProps.Caption = "&Tools";
			popupMenuTool13.SharedProps.Category = "Tools";
			buttonTool36.InstanceProps.IsFirstInGroup = true;
			buttonTool38.InstanceProps.IsFirstInGroup = true;
			buttonTool41.InstanceProps.IsFirstInGroup = true;
			popupMenuTool13.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool35,
            buttonTool36,
            buttonTool37,
            buttonTool98,
            buttonTool38,
            buttonTool39,
            buttonTool40,
            buttonTool41});
			appearance18.Image = 10;
			buttonTool42.SharedProps.AppearancesSmall.Appearance = appearance18;
			buttonTool42.SharedProps.Caption = "&Presentation Options ...";
			buttonTool42.SharedProps.Category = "Tools";
			appearance19.BackColorDisabled = System.Drawing.SystemColors.Menu;
			textBoxTool2.EditAppearance = appearance19;
			textBoxTool2.SharedProps.Caption = "GoTo";
			textBoxTool2.SharedProps.ShowInCustomizer = false;
			textBoxTool2.SharedProps.Spring = true;
			textBoxTool2.SharedProps.ToolTipText = "Enter Barcode";
			labelTool4.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
			labelTool5.SharedProps.Caption = "Go To:";
			labelTool5.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
			labelTool5.SharedProps.ShowInCustomizer = false;
			labelTool5.SharedProps.ToolTipText = "Go to record";
			labelTool6.SharedProps.Spring = true;
			appearance20.Image = 11;
			buttonTool43.SharedProps.AppearancesSmall.Appearance = appearance20;
			buttonTool43.SharedProps.Caption = "&Refresh Treatments";
			buttonTool43.SharedProps.Category = "Tools";
			buttonTool43.SharedProps.Shortcut = System.Windows.Forms.Shortcut.CtrlT;
			appearance21.Image = 12;
			buttonTool44.SharedProps.AppearancesSmall.Appearance = appearance21;
			buttonTool44.SharedProps.Caption = "&Validate Database ...";
			buttonTool44.SharedProps.Category = "Tools";
			appearance22.Image = 13;
			buttonTool45.SharedProps.AppearancesSmall.Appearance = appearance22;
			buttonTool45.SharedProps.Caption = "&Case Options ...";
			buttonTool45.SharedProps.Category = "Tools";
			appearance23.Image = 28;
			buttonTool46.SharedProps.AppearancesSmall.Appearance = appearance23;
			buttonTool46.SharedProps.Caption = "&Refresh All";
			buttonTool46.SharedProps.Shortcut = System.Windows.Forms.Shortcut.F9;
			appearance24.Image = 17;
			buttonTool47.SharedProps.AppearancesSmall.Appearance = appearance24;
			buttonTool47.SharedProps.Caption = "&Print ...";
			buttonTool47.SharedProps.Shortcut = System.Windows.Forms.Shortcut.CtrlP;
			appearance25.Image = 15;
			buttonTool48.SharedProps.AppearancesSmall.Appearance = appearance25;
			buttonTool48.SharedProps.Caption = "Find &Transcript Text ...";
			buttonTool48.SharedProps.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
			appearance26.Image = 16;
			buttonTool49.SharedProps.AppearancesSmall.Appearance = appearance26;
			buttonTool49.SharedProps.Caption = "Find &Next";
			buttonTool49.SharedProps.Shortcut = System.Windows.Forms.Shortcut.F3;
			appearance27.Image = 18;
			popupMenuTool14.SharedProps.AppearancesSmall.Appearance = appearance27;
			popupMenuTool14.SharedProps.Caption = "&Import";
			buttonTool52.InstanceProps.IsFirstInGroup = true;
			buttonTool54.InstanceProps.IsFirstInGroup = true;
			buttonTool57.InstanceProps.IsFirstInGroup = true;
			buttonTool58.InstanceProps.IsFirstInGroup = true;
			popupMenuTool14.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool50,
            buttonTool51,
            buttonTool52,
            buttonTool53,
            buttonTool54,
            buttonTool55,
            buttonTool56,
            buttonTool57,
            buttonTool58,
            buttonTool59});
			appearance28.Image = 19;
			popupMenuTool15.SharedProps.AppearancesSmall.Appearance = appearance28;
			popupMenuTool15.SharedProps.Caption = "&Export";
			popupMenuTool15.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool60,
            buttonTool61,
            buttonTool62});
			popupMenuTool16.SharedProps.Caption = "&Edit";
			buttonTool65.InstanceProps.IsFirstInGroup = true;
			buttonTool67.InstanceProps.IsFirstInGroup = true;
			popupMenuTool16.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool63,
            buttonTool64,
            buttonTool65,
            buttonTool66,
            buttonTool67});
			appearance29.Image = 20;
			buttonTool68.SharedProps.AppearancesSmall.Appearance = appearance29;
			buttonTool68.SharedProps.Caption = "Barcode &Map ...";
			appearance30.Image = 20;
			buttonTool69.SharedProps.AppearancesSmall.Appearance = appearance30;
			buttonTool69.SharedProps.Caption = "&Barcode Map To Text File ...";
			appearance31.Image = 40;
			buttonTool70.SharedProps.AppearancesSmall.Appearance = appearance31;
			buttonTool70.SharedProps.Caption = "&Binder(s) From Text File(s) ...";
			appearance32.Image = 33;
			buttonTool71.SharedProps.AppearancesSmall.Appearance = appearance32;
			buttonTool71.SharedProps.Caption = "&Script(s) From Text File(s) ...";
			stateButtonTool33.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool33.SharedProps.Caption = "Lock Panes In Place";
			appearance33.Image = 23;
			buttonTool72.SharedProps.AppearancesSmall.Appearance = appearance33;
			buttonTool72.SharedProps.Caption = "&Manager Options ...";
			appearance34.Image = 24;
			buttonTool73.SharedProps.AppearancesSmall.Appearance = appearance34;
			buttonTool73.SharedProps.Caption = "&Load File ...";
			appearance35.Image = 25;
			buttonTool74.SharedProps.AppearancesSmall.Appearance = appearance35;
			buttonTool74.SharedProps.Caption = "TrialMax Presentation ...";
			buttonTool74.SharedProps.Shortcut = System.Windows.Forms.Shortcut.CtrlF5;
			appearance36.Image = 26;
			buttonTool75.SharedProps.AppearancesSmall.Appearance = appearance36;
			buttonTool75.SharedProps.Caption = "Presentation &Toolbars ...";
			buttonTool76.SharedProps.Caption = "Debug 1";
			buttonTool77.SharedProps.Caption = "Debug 2";
			popupMenuTool17.SharedProps.Caption = "Debug";
			popupMenuTool17.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool78,
            buttonTool79});
			appearance37.Image = 27;
			buttonTool80.SharedProps.AppearancesSmall.Appearance = appearance37;
			buttonTool80.SharedProps.Caption = "Activate &TrialMax ...";
			appearance38.Image = 14;
			buttonTool81.SharedProps.AppearancesSmall.Appearance = appearance38;
			buttonTool81.SharedProps.Caption = "Check For &Updates ...";
			appearance39.Image = 29;
			buttonTool82.SharedProps.AppearancesSmall.Appearance = appearance39;
			buttonTool82.SharedProps.Caption = "Contact &FTI ...";
			appearance40.Image = 30;
			buttonTool83.SharedProps.AppearancesSmall.Appearance = appearance40;
			buttonTool83.SharedProps.Caption = "User\'s &Manual ...";
			stateButtonTool34.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool34.SharedProps.Caption = "Fielded &Data";
			stateButtonTool34.SharedProps.Category = "View";
			stateButtonTool34.SharedProps.Shortcut = System.Windows.Forms.Shortcut.F10;
			appearance41.Image = 31;
			buttonTool84.SharedProps.AppearancesSmall.Appearance = appearance41;
			buttonTool84.SharedProps.Caption = "Fielded Data From &Text File(s) ...";
			stateButtonTool35.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool35.SharedProps.Caption = "&Filtered Tree";
			appearance42.Image = 32;
			buttonTool85.SharedProps.AppearancesSmall.Appearance = appearance42;
			buttonTool85.SharedProps.Caption = "&Advanced Filter ...";
			buttonTool85.SharedProps.Shortcut = System.Windows.Forms.Shortcut.F12;
			appearance43.Image = 34;
			buttonTool86.SharedProps.AppearancesSmall.Appearance = appearance43;
			buttonTool86.SharedProps.Caption = "Script(s) From &XML File(s) ...";
			appearance44.Image = 35;
			buttonTool87.SharedProps.AppearancesSmall.Appearance = appearance44;
			buttonTool87.SharedProps.Caption = "TrialMax &Online ...";
			appearance45.Image = 36;
			buttonTool88.SharedProps.AppearancesSmall.Appearance = appearance45;
			buttonTool88.SharedProps.Caption = "&Fielded Data Definitions ...";
			appearance46.Image = 37;
			buttonTool89.SharedProps.AppearancesSmall.Appearance = appearance46;
			buttonTool89.SharedProps.Caption = "&Fielded Data Definitions ...";
			appearance47.Image = 38;
			buttonTool90.SharedProps.AppearancesSmall.Appearance = appearance47;
			buttonTool90.SharedProps.Caption = "&Fast Filter ...";
			buttonTool90.SharedProps.Shortcut = System.Windows.Forms.Shortcut.CtrlF12;
			appearance48.Image = 39;
			buttonTool91.SharedProps.AppearancesSmall.Appearance = appearance48;
			buttonTool91.SharedProps.Caption = "Fielded Data From &Database ...";
			appearance49.Image = 41;
			buttonTool92.SharedProps.AppearancesSmall.Appearance = appearance49;
			buttonTool92.SharedProps.Caption = "Bi&nder(s) From XML File(s) ...";
			appearance50.Image = 42;
			buttonTool93.SharedProps.AppearancesSmall.Appearance = appearance50;
			buttonTool93.SharedProps.Caption = "&Update Fielded Data ...";
			appearance51.Image = 43;
			buttonTool94.SharedProps.AppearancesSmall.Appearance = appearance51;
			buttonTool94.SharedProps.Caption = "Trim Database ...";
			stateButtonTool36.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool36.SharedProps.Caption = "Objections";
			stateButtonTool37.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool37.SharedProps.Caption = "Script Review";
			appearance52.Image = 44;
			buttonTool95.SharedProps.AppearancesSmall.Appearance = appearance52;
			buttonTool95.SharedProps.Caption = "&Objections  From Text File(s) ...";
			appearance53.Image = 45;
			buttonTool96.SharedProps.AppearancesSmall.Appearance = appearance53;
			buttonTool96.SharedProps.Caption = "&Objections  To Text File ...";
			stateButtonTool38.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool38.SharedProps.Caption = "Objection Properties";
			appearance64.Image = 46;
			buttonTool97.SharedProps.AppearancesSmall.Appearance = appearance64;
			buttonTool97.SharedProps.Caption = "Compact Database ...";
			this.m_ctrlUltraToolbarManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool6,
            buttonTool13,
            buttonTool14,
            buttonTool15,
            buttonTool16,
            popupMenuTool9,
            buttonTool20,
            popupMenuTool11,
            buttonTool27,
            stateButtonTool18,
            stateButtonTool19,
            stateButtonTool20,
            stateButtonTool21,
            stateButtonTool22,
            stateButtonTool23,
            stateButtonTool24,
            stateButtonTool25,
            stateButtonTool26,
            popupMenuTool12,
            stateButtonTool29,
            stateButtonTool30,
            stateButtonTool31,
            stateButtonTool32,
            buttonTool28,
            buttonTool29,
            buttonTool30,
            buttonTool31,
            buttonTool32,
            buttonTool33,
            buttonTool34,
            popupMenuTool13,
            buttonTool42,
            textBoxTool2,
            labelTool4,
            labelTool5,
            labelTool6,
            buttonTool43,
            buttonTool44,
            buttonTool45,
            buttonTool46,
            buttonTool47,
            buttonTool48,
            buttonTool49,
            popupMenuTool14,
            popupMenuTool15,
            popupMenuTool16,
            buttonTool68,
            buttonTool69,
            buttonTool70,
            buttonTool71,
            stateButtonTool33,
            buttonTool72,
            buttonTool73,
            buttonTool74,
            buttonTool75,
            buttonTool76,
            buttonTool77,
            popupMenuTool17,
            buttonTool80,
            buttonTool81,
            buttonTool82,
            buttonTool83,
            stateButtonTool34,
            buttonTool84,
            stateButtonTool35,
            buttonTool85,
            buttonTool86,
            buttonTool87,
            buttonTool88,
            buttonTool89,
            buttonTool90,
            buttonTool91,
            buttonTool92,
            buttonTool93,
            buttonTool94,
            stateButtonTool36,
            stateButtonTool37,
            buttonTool95,
            buttonTool96,
            stateButtonTool38,
            buttonTool97});
			this.m_ctrlUltraToolbarManager.BeforeToolDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventHandler(this.OnUltraToolPopup);
			this.m_ctrlUltraToolbarManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.OnUltraToolClick);
			this.m_ctrlUltraToolbarManager.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.OnUltraBeforeToolbarListDropdown);
			this.m_ctrlUltraToolbarManager.ToolKeyDown += new Infragistics.Win.UltraWinToolbars.ToolKeyEventHandler(this.OnUltraToolKeyDown);
			// 
			// _CTmaxManagerForm_Toolbars_Dock_Area_Bottom
			// 
			this._CTmaxManagerForm_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmaxManagerForm_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
			this._CTmaxManagerForm_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
			this._CTmaxManagerForm_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxManagerForm_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 341);
			this._CTmaxManagerForm_Toolbars_Dock_Area_Bottom.Name = "_CTmaxManagerForm_Toolbars_Dock_Area_Bottom";
			this._CTmaxManagerForm_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(672, 0);
			this._CTmaxManagerForm_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.m_ctrlUltraToolbarManager;
			// 
			// _CTmaxManagerForm_Toolbars_Dock_Area_Left
			// 
			this._CTmaxManagerForm_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmaxManagerForm_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
			this._CTmaxManagerForm_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
			this._CTmaxManagerForm_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxManagerForm_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 69);
			this._CTmaxManagerForm_Toolbars_Dock_Area_Left.Name = "_CTmaxManagerForm_Toolbars_Dock_Area_Left";
			this._CTmaxManagerForm_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 272);
			this._CTmaxManagerForm_Toolbars_Dock_Area_Left.ToolbarsManager = this.m_ctrlUltraToolbarManager;
			// 
			// _CTmaxManagerForm_Toolbars_Dock_Area_Right
			// 
			this._CTmaxManagerForm_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmaxManagerForm_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
			this._CTmaxManagerForm_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
			this._CTmaxManagerForm_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxManagerForm_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(672, 69);
			this._CTmaxManagerForm_Toolbars_Dock_Area_Right.Name = "_CTmaxManagerForm_Toolbars_Dock_Area_Right";
			this._CTmaxManagerForm_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 272);
			this._CTmaxManagerForm_Toolbars_Dock_Area_Right.ToolbarsManager = this.m_ctrlUltraToolbarManager;
			// 
			// windowDockingArea1
			// 
			this.windowDockingArea1.Controls.Add(this.dockableWindow12);
			this.windowDockingArea1.Controls.Add(this.dockableWindow11);
			this.windowDockingArea1.Controls.Add(this.dockableWindow10);
			this.windowDockingArea1.Controls.Add(this.dockableWindow4);
			this.windowDockingArea1.Controls.Add(this.dockableWindow5);
			this.windowDockingArea1.Controls.Add(this.dockableWindow6);
			this.windowDockingArea1.Controls.Add(this.dockableWindow7);
			this.windowDockingArea1.Dock = System.Windows.Forms.DockStyle.Right;
			this.windowDockingArea1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.windowDockingArea1.Location = new System.Drawing.Point(461, 69);
			this.windowDockingArea1.Name = "windowDockingArea1";
			this.windowDockingArea1.Owner = this.m_ctrlUltraDockManager;
			this.windowDockingArea1.Size = new System.Drawing.Size(211, 272);
			this.windowDockingArea1.TabIndex = 10;
			// 
			// dockableWindow12
			// 
			this.dockableWindow12.Controls.Add(this.m_paneTuner);
			this.dockableWindow12.Location = new System.Drawing.Point(-10000, 2);
			this.dockableWindow12.Name = "dockableWindow12";
			this.dockableWindow12.Owner = this.m_ctrlUltraDockManager;
			this.dockableWindow12.Size = new System.Drawing.Size(336, 238);
			this.dockableWindow12.TabIndex = 16;
			// 
			// dockableWindow11
			// 
			this.dockableWindow11.Controls.Add(this.m_paneCodes);
			this.dockableWindow11.Location = new System.Drawing.Point(7, 2);
			this.dockableWindow11.Name = "dockableWindow11";
			this.dockableWindow11.Owner = this.m_ctrlUltraDockManager;
			this.dockableWindow11.Size = new System.Drawing.Size(202, 248);
			this.dockableWindow11.TabIndex = 17;
			// 
			// dockableWindow10
			// 
			this.dockableWindow10.Controls.Add(this.m_paneHelp);
			this.dockableWindow10.Location = new System.Drawing.Point(-10000, 2);
			this.dockableWindow10.Name = "dockableWindow10";
			this.dockableWindow10.Owner = this.m_ctrlUltraDockManager;
			this.dockableWindow10.Size = new System.Drawing.Size(335, 238);
			this.dockableWindow10.TabIndex = 18;
			// 
			// dockableWindow4
			// 
			this.dockableWindow4.Controls.Add(this.m_paneVersions);
			this.dockableWindow4.Location = new System.Drawing.Point(-10000, 2);
			this.dockableWindow4.Name = "dockableWindow4";
			this.dockableWindow4.Owner = this.m_ctrlUltraDockManager;
			this.dockableWindow4.Size = new System.Drawing.Size(336, 238);
			this.dockableWindow4.TabIndex = 19;
			// 
			// dockableWindow5
			// 
			this.dockableWindow5.Controls.Add(this.m_paneProperties);
			this.dockableWindow5.Location = new System.Drawing.Point(-10000, 2);
			this.dockableWindow5.Name = "dockableWindow5";
			this.dockableWindow5.Owner = this.m_ctrlUltraDockManager;
			this.dockableWindow5.Size = new System.Drawing.Size(213, 238);
			this.dockableWindow5.TabIndex = 20;
			// 
			// dockableWindow6
			// 
			this.dockableWindow6.Controls.Add(this.m_paneErrors);
			this.dockableWindow6.Location = new System.Drawing.Point(-10000, 2);
			this.dockableWindow6.Name = "dockableWindow6";
			this.dockableWindow6.Owner = this.m_ctrlUltraDockManager;
			this.dockableWindow6.Size = new System.Drawing.Size(214, 238);
			this.dockableWindow6.TabIndex = 21;
			// 
			// dockableWindow7
			// 
			this.dockableWindow7.Controls.Add(this.m_paneDiagnostics);
			this.dockableWindow7.Location = new System.Drawing.Point(-10000, 2);
			this.dockableWindow7.Name = "dockableWindow7";
			this.dockableWindow7.Owner = this.m_ctrlUltraDockManager;
			this.dockableWindow7.Size = new System.Drawing.Size(203, 238);
			this.dockableWindow7.TabIndex = 22;
			// 
			// dockableWindow9
			// 
			this.dockableWindow9.Controls.Add(this.m_paneMedia);
			this.dockableWindow9.Location = new System.Drawing.Point(-10000, 2);
			this.dockableWindow9.Name = "dockableWindow9";
			this.dockableWindow9.Owner = this.m_ctrlUltraDockManager;
			this.dockableWindow9.Size = new System.Drawing.Size(206, 238);
			this.dockableWindow9.TabIndex = 23;
			// 
			// dockableWindow8
			// 
			this.dockableWindow8.Controls.Add(this.m_paneObjections);
			this.dockableWindow8.Location = new System.Drawing.Point(-10000, 2);
			this.dockableWindow8.Name = "dockableWindow8";
			this.dockableWindow8.Owner = this.m_ctrlUltraDockManager;
			this.dockableWindow8.Size = new System.Drawing.Size(196, 293);
			this.dockableWindow8.TabIndex = 24;
			// 
			// dockableWindow2
			// 
			this.dockableWindow2.Controls.Add(this.m_paneBinders);
			this.dockableWindow2.Location = new System.Drawing.Point(-10000, 2);
			this.dockableWindow2.Name = "dockableWindow2";
			this.dockableWindow2.Owner = this.m_ctrlUltraDockManager;
			this.dockableWindow2.Size = new System.Drawing.Size(151, 293);
			this.dockableWindow2.TabIndex = 25;
			// 
			// dockableWindow1
			// 
			this.dockableWindow1.Controls.Add(this.m_paneFiltered);
			this.dockableWindow1.Location = new System.Drawing.Point(-10000, 2);
			this.dockableWindow1.Name = "dockableWindow1";
			this.dockableWindow1.Owner = this.m_ctrlUltraDockManager;
			this.dockableWindow1.Size = new System.Drawing.Size(336, 238);
			this.dockableWindow1.TabIndex = 26;
			// 
			// dockableWindow3
			// 
			this.dockableWindow3.Controls.Add(this.m_paneObjectionProperties);
			this.dockableWindow3.Location = new System.Drawing.Point(2, 2);
			this.dockableWindow3.Name = "dockableWindow3";
			this.dockableWindow3.Owner = this.m_ctrlUltraDockManager;
			this.dockableWindow3.Size = new System.Drawing.Size(186, 248);
			this.dockableWindow3.TabIndex = 27;
			// 
			// dockableWindow13
			// 
			this.dockableWindow13.Controls.Add(this.m_paneResults);
			this.dockableWindow13.Location = new System.Drawing.Point(-10000, 2);
			this.dockableWindow13.Name = "dockableWindow13";
			this.dockableWindow13.Owner = this.m_ctrlUltraDockManager;
			this.dockableWindow13.Size = new System.Drawing.Size(297, 122);
			this.dockableWindow13.TabIndex = 28;
			// 
			// dockableWindow14
			// 
			this.dockableWindow14.Controls.Add(this.m_paneScriptReview);
			this.dockableWindow14.Location = new System.Drawing.Point(-10000, 2);
			this.dockableWindow14.Name = "dockableWindow14";
			this.dockableWindow14.Owner = this.m_ctrlUltraDockManager;
			this.dockableWindow14.Size = new System.Drawing.Size(297, 122);
			this.dockableWindow14.TabIndex = 29;
			// 
			// windowDockingArea2
			// 
			this.windowDockingArea2.Controls.Add(this.dockableWindow9);
			this.windowDockingArea2.Controls.Add(this.dockableWindow8);
			this.windowDockingArea2.Controls.Add(this.dockableWindow2);
			this.windowDockingArea2.Controls.Add(this.dockableWindow1);
			this.windowDockingArea2.Controls.Add(this.dockableWindow3);
			this.windowDockingArea2.Controls.Add(this.dockableWindow13);
			this.windowDockingArea2.Dock = System.Windows.Forms.DockStyle.Left;
			this.windowDockingArea2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.windowDockingArea2.Location = new System.Drawing.Point(0, 69);
			this.windowDockingArea2.Name = "windowDockingArea2";
			this.windowDockingArea2.Owner = this.m_ctrlUltraDockManager;
			this.windowDockingArea2.Size = new System.Drawing.Size(195, 272);
			this.windowDockingArea2.TabIndex = 11;
			// 
			// dockableWindow18
			// 
			this.dockableWindow18.Controls.Add(this.m_paneTranscripts);
			this.dockableWindow18.Location = new System.Drawing.Point(-10000, 2);
			this.dockableWindow18.Name = "dockableWindow18";
			this.dockableWindow18.Owner = this.m_ctrlUltraDockManager;
			this.dockableWindow18.Size = new System.Drawing.Size(186, 293);
			this.dockableWindow18.TabIndex = 30;
			// 
			// dockableWindow16
			// 
			this.dockableWindow16.Controls.Add(this.m_paneViewer);
			this.dockableWindow16.Location = new System.Drawing.Point(-10000, 2);
			this.dockableWindow16.Name = "dockableWindow16";
			this.dockableWindow16.Owner = this.m_ctrlUltraDockManager;
			this.dockableWindow16.Size = new System.Drawing.Size(196, 293);
			this.dockableWindow16.TabIndex = 31;
			// 
			// dockableWindow15
			// 
			this.dockableWindow15.Controls.Add(this.m_paneScripts);
			this.dockableWindow15.Location = new System.Drawing.Point(2, 2);
			this.dockableWindow15.Name = "dockableWindow15";
			this.dockableWindow15.Owner = this.m_ctrlUltraDockManager;
			this.dockableWindow15.Size = new System.Drawing.Size(262, 122);
			this.dockableWindow15.TabIndex = 32;
			// 
			// windowDockingArea4
			// 
			this.windowDockingArea4.Controls.Add(this.dockableWindow14);
			this.windowDockingArea4.Controls.Add(this.dockableWindow16);
			this.windowDockingArea4.Controls.Add(this.dockableWindow15);
			this.windowDockingArea4.Controls.Add(this.dockableWindow17);
			this.windowDockingArea4.Controls.Add(this.dockableWindow18);
			this.windowDockingArea4.Dock = System.Windows.Forms.DockStyle.Top;
			this.windowDockingArea4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.windowDockingArea4.Location = new System.Drawing.Point(195, 69);
			this.windowDockingArea4.Name = "windowDockingArea4";
			this.windowDockingArea4.Owner = this.m_ctrlUltraDockManager;
			this.windowDockingArea4.Size = new System.Drawing.Size(266, 151);
			this.windowDockingArea4.TabIndex = 0;
			// 
			// dockableWindow17
			// 
			this.dockableWindow17.Controls.Add(this.m_paneSource);
			this.dockableWindow17.Location = new System.Drawing.Point(-10000, 2);
			this.dockableWindow17.Name = "dockableWindow17";
			this.dockableWindow17.Owner = this.m_ctrlUltraDockManager;
			this.dockableWindow17.Size = new System.Drawing.Size(297, 122);
			this.dockableWindow17.TabIndex = 33;
			// 
			// m_ctrlMainFillPanel
			// 
			this.m_ctrlMainFillPanel.Controls.Add(this.m_tmxMovie);
			this.m_ctrlMainFillPanel.Controls.Add(this.m_tmxView);
			this.m_ctrlMainFillPanel.Controls.Add(this.m_ctrlScreenCapture);
			this.m_ctrlMainFillPanel.Controls.Add(this.m_ctrlPresentation);
			this.m_ctrlMainFillPanel.Controls.Add(this.m_ctrlFillPanel);
			this.m_ctrlMainFillPanel.Controls.Add(this.m_tmxPrint);
			this.m_ctrlMainFillPanel.Cursor = System.Windows.Forms.Cursors.Default;
			this.m_ctrlMainFillPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlMainFillPanel.Location = new System.Drawing.Point(195, 220);
			this.m_ctrlMainFillPanel.Name = "m_ctrlMainFillPanel";
			this.m_ctrlMainFillPanel.Size = new System.Drawing.Size(266, 121);
			this.m_ctrlMainFillPanel.TabIndex = 5;
			// 
			// m_tmxMovie
			// 
			this.m_tmxMovie.AxAutoSave = false;
			this.m_tmxMovie.AxError = ((short)(0));
			this.m_tmxMovie.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.m_tmxMovie.CueStep = 10;
			this.m_tmxMovie.EnableSimulation = false;
			this.m_tmxMovie.EnableToolbar = false;
			this.m_tmxMovie.IniFilename = "";
			this.m_tmxMovie.IniSection = "";
			this.m_tmxMovie.Location = new System.Drawing.Point(146, 8);
			this.m_tmxMovie.LockRange = false;
			this.m_tmxMovie.Name = "m_tmxMovie";
			this.m_tmxMovie.NavigatorPosition = -1;
			this.m_tmxMovie.NavigatorTotal = 0;
			this.m_tmxMovie.ShowToolbar = false;
			this.m_tmxMovie.SimulationText = "";
			this.m_tmxMovie.Size = new System.Drawing.Size(44, 36);
			this.m_tmxMovie.TabIndex = 33;
			this.m_tmxMovie.Visible = false;
			// 
			// m_tmxView
			// 
			this.m_tmxView.AxAutoSave = false;
			this.m_tmxView.AxError = ((short)(0));
			this.m_tmxView.BackColor = System.Drawing.Color.Silver;
			this.m_tmxView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.m_tmxView.EnableToolbar = false;
			this.m_tmxView.IniFilename = "";
			this.m_tmxView.IniSection = "";
			this.m_tmxView.Location = new System.Drawing.Point(92, 8);
			this.m_tmxView.Name = "m_tmxView";
			this.m_tmxView.NavigatorPosition = -1;
			this.m_tmxView.NavigatorTotal = 0;
			this.m_tmxView.ShowToolbar = false;
			this.m_tmxView.Size = new System.Drawing.Size(48, 40);
			this.m_tmxView.TabIndex = 32;
			this.m_tmxView.UseScreenRatio = false;
			this.m_tmxView.Visible = false;
			this.m_tmxView.ZapSourceFile = "";
			this.m_tmxView.TmxEvent += new FTI.Trialmax.ActiveX.TmxEventHandler(this.OnTmxViewEvent);
			// 
			// m_ctrlScreenCapture
			// 
			this.m_ctrlScreenCapture.Enabled = true;
			this.m_ctrlScreenCapture.Location = new System.Drawing.Point(8, 72);
			this.m_ctrlScreenCapture.Name = "m_ctrlScreenCapture";
			this.m_ctrlScreenCapture.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("m_ctrlScreenCapture.OcxState")));
			this.m_ctrlScreenCapture.Size = new System.Drawing.Size(192, 32);
			this.m_ctrlScreenCapture.TabIndex = 31;
			// 
			// m_ctrlPresentation
			// 
			this.m_ctrlPresentation.AppFolder = "";
			this.m_ctrlPresentation.AxAutoSave = false;
			this.m_ctrlPresentation.AxError = ((short)(0));
			this.m_ctrlPresentation.BackColor = System.Drawing.Color.Black;
			this.m_ctrlPresentation.Barcode = "";
			this.m_ctrlPresentation.BarcodeId = 0;
			this.m_ctrlPresentation.CaseFolder = "";
			this.m_ctrlPresentation.Command = FTI.Trialmax.ActiveX.TmxShareCommands.None;
			this.m_ctrlPresentation.DisplayOrder = 0;
			this.m_ctrlPresentation.EnableToolbar = true;
			this.m_ctrlPresentation.IniFilename = "";
			this.m_ctrlPresentation.IniSection = "";
			this.m_ctrlPresentation.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlPresentation.Name = "m_ctrlPresentation";
			this.m_ctrlPresentation.NavigatorPosition = -1;
			this.m_ctrlPresentation.NavigatorTotal = 0;
			this.m_ctrlPresentation.PrimaryId = 0;
			this.m_ctrlPresentation.QuaternaryId = 0;
			this.m_ctrlPresentation.SecondaryId = 0;
			this.m_ctrlPresentation.ShowToolbar = false;
			this.m_ctrlPresentation.Size = new System.Drawing.Size(36, 40);
			this.m_ctrlPresentation.SourceFileName = "";
			this.m_ctrlPresentation.SourceFilePath = "";
			this.m_ctrlPresentation.TabIndex = 0;
			this.m_ctrlPresentation.TertiaryId = 0;
			this.m_ctrlPresentation.Visible = false;
			this.m_ctrlPresentation.TmxShareRequestEvent += new FTI.Trialmax.ActiveX.TmxShareEventHandler(this.OnPresentationRequest);
			// 
			// m_ctrlFillPanel
			// 
			this.m_ctrlFillPanel.BackColor = System.Drawing.SystemColors.Window;
			this.m_ctrlFillPanel.Location = new System.Drawing.Point(48, 8);
			this.m_ctrlFillPanel.Name = "m_ctrlFillPanel";
			this.m_ctrlFillPanel.Size = new System.Drawing.Size(36, 40);
			this.m_ctrlFillPanel.TabIndex = 30;
			// 
			// m_tmxPrint
			// 
			this.m_tmxPrint.Enabled = true;
			this.m_tmxPrint.Location = new System.Drawing.Point(0, 112);
			this.m_tmxPrint.Name = "m_tmxPrint";
			this.m_tmxPrint.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("m_tmxPrint.OcxState")));
			this.m_tmxPrint.Size = new System.Drawing.Size(594, 397);
			this.m_tmxPrint.TabIndex = 29;
			this.m_tmxPrint.Visible = false;
			// 
			// windowDockingArea6
			// 
			this.windowDockingArea6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.windowDockingArea6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.windowDockingArea6.Location = new System.Drawing.Point(205, 24);
			this.windowDockingArea6.Name = "windowDockingArea6";
			this.windowDockingArea6.Owner = this.m_ctrlUltraDockManager;
			this.windowDockingArea6.Size = new System.Drawing.Size(100, 100);
			this.windowDockingArea6.TabIndex = 38;
			// 
			// CTmaxManagerForm
			// 
			this.AccessibleName = "";
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(672, 341);
			this.Controls.Add(this._CTmaxManagerFormAutoHideControl);
			this.Controls.Add(this.m_ctrlMainFillPanel);
			this.Controls.Add(this._CTmaxManagerFormUnpinnedTabAreaLeft);
			this.Controls.Add(this._CTmaxManagerFormUnpinnedTabAreaTop);
			this.Controls.Add(this._CTmaxManagerFormUnpinnedTabAreaBottom);
			this.Controls.Add(this._CTmaxManagerFormUnpinnedTabAreaRight);
			this.Controls.Add(this.windowDockingArea4);
			this.Controls.Add(this.windowDockingArea2);
			this.Controls.Add(this.windowDockingArea1);
			this.Controls.Add(this._CTmaxManagerForm_Toolbars_Dock_Area_Left);
			this.Controls.Add(this._CTmaxManagerForm_Toolbars_Dock_Area_Right);
			this.Controls.Add(this._CTmaxManagerForm_Toolbars_Dock_Area_Top);
			this.Controls.Add(this._CTmaxManagerForm_Toolbars_Dock_Area_Bottom);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "CTmaxManagerForm";
			this.Text = "FTI Consulting - TrialMax Manager";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraDockManager)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraToolbarManager)).EndInit();
			this.windowDockingArea1.ResumeLayout(false);
			this.dockableWindow12.ResumeLayout(false);
			this.dockableWindow11.ResumeLayout(false);
			this.dockableWindow10.ResumeLayout(false);
			this.dockableWindow4.ResumeLayout(false);
			this.dockableWindow5.ResumeLayout(false);
			this.dockableWindow6.ResumeLayout(false);
			this.dockableWindow7.ResumeLayout(false);
			this.dockableWindow9.ResumeLayout(false);
			this.dockableWindow8.ResumeLayout(false);
			this.dockableWindow2.ResumeLayout(false);
			this.dockableWindow1.ResumeLayout(false);
			this.dockableWindow3.ResumeLayout(false);
			this.dockableWindow13.ResumeLayout(false);
			this.dockableWindow14.ResumeLayout(false);
			this.windowDockingArea2.ResumeLayout(false);
			this.dockableWindow18.ResumeLayout(false);
			this.dockableWindow16.ResumeLayout(false);
			this.dockableWindow15.ResumeLayout(false);
			this.windowDockingArea4.ResumeLayout(false);
			this.dockableWindow17.ResumeLayout(false);
			this.m_ctrlMainFillPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlScreenCapture)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_tmxPrint)).EndInit();
			this.ResumeLayout(false);

		}
		
		/// <summary>This function is called to initialize the application's database object</summary>
		/// <param name="tmaxDatabase">The database to be initialized</param>
		private void InitializeDatabase(CTmaxCaseDatabase tmaxDatabase)
		{
			Debug.Assert(tmaxDatabase != null);
			
			tmaxDatabase.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);
			tmaxDatabase.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.OnDiagnostic);
			tmaxDatabase.InternalUpdateEvent += new CTmaxCaseDatabase.DatabaseEventHandler(this.OnDbInternalUpdate);
			tmaxDatabase.TmaxCommandEvent += new FTI.Shared.Trialmax.TmaxCommandHandler(this.OnTmaxCommand);
			tmaxDatabase.MediaTypes = m_tmaxMediaTypes;
			tmaxDatabase.SourceTypes = m_tmaxSourceTypes;
			tmaxDatabase.RegistrationOptions = m_tmaxRegOptions;
			tmaxDatabase.AppOptions = m_tmaxAppOptions;
			tmaxDatabase.TmaxRegistry = m_tmaxRegistry;
			tmaxDatabase.TmaxProductManager = m_tmaxProductManager;
			tmaxDatabase.TmxMovie = m_tmxMovie;
			tmaxDatabase.TmxView = m_tmxView;
			tmaxDatabase.WMEncoder = m_mediaEncoder;
			tmaxDatabase.PaneId = (int)(TmaxAppPanes.MaxPanes);

		}// private void InitializeDatabase(CTmaxCaseDatabase tmaxDatabase)
		
		/// <summary>This function is called to initialize the screen layout</summary>
		private void InitializeLayout()
		{ 
			string strFilename;
								
			try
			{
				//	Initialize the filler group
				m_ctrlFillPanel.Dock = System.Windows.Forms.DockStyle.Fill;
				//m_ctrlGroupPane.Add(GetUltraPaneKey(TmaxAppPanes.Results), "Results", m_paneResults);

				//	First attempt to load the information from the layout file
				strFilename = m_tmaxAppOptions.LastLayout;
				if(strFilename.Length > 0)
				{
					if(System.IO.File.Exists(strFilename) == false)
					{
						m_tmaxAppOptions.LastLayout = "";
						strFilename = "";
					}
				}
				//	Do we need to try the default?
				if(strFilename.Length == 0)
				{
					strFilename = m_strAppFolder + DEFAULT_LAYOUT_FILENAME;
				}
				if(System.IO.File.Exists(strFilename))
				{
					if(LoadPaneLayout(strFilename) == true)
					{
						m_tmaxAppOptions.LastLayout = strFilename;
						return;
					}
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "InitializeLayout", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_LAYOUT_EX), Ex);
			}
		
		}// InitializeLayout()
		
		/// <summary>This function is called to initialize the screen capture engine</summary>
		private void InitializeScreenCapture()
		{ 
			try
			{
				m_ctrlScreenCapture.Silent = true;
				m_ctrlScreenCapture.OneShot = true;
				m_ctrlScreenCapture.CancelKey = 27;
				m_ctrlScreenCapture.Hotkey = 122;	//	Default = F11
				m_ctrlScreenCapture.Area = 2;		//	User selected capture area
				
				if(m_ctrlScreenCapture.Initialize() != 0)
				{
					m_tmaxEventSource.FireDiagnostic(this, "InitializeScreenCapture", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_SCREEN_CAPTURE_FAIL));
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "InitializeScreenCapture", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_SCREEN_CAPTURE_EX), Ex);
			}
		
		}// private void InitializeScreenCapture()
		
		/// <summary>This function is called to initialize the application's initialization and event log files</summary>
		private void InitializeFiles()
		{
			CTmaxManagerVersion ver = new CTmaxManagerVersion();

			m_strAppFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
			m_strAppFolder += "\\";
			m_strIniFilename = m_strAppFolder + DEFAULT_APP_CONFIGURATION_FILE;
			m_strUpdateInstaller = m_strAppFolder + DEFAULT_UPDATE_INSTALLER_FILENAME;
			m_strPresentationIniFileSpec = m_strAppFolder + DEFAULT_PRESENTATION_INI_FILENAME;
			
			m_tmaxDatabase.AppFolder = m_strAppFolder;
			
			//	Open the INI file for use during initialization
			m_xmlIni.XMLComments.Add("TrialMax .NET XML Application Configuration File");
			m_xmlIni.XMLComments.Add("Copyright FTI Consulting");
			m_xmlIni.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);
			m_xmlIni.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.OnDiagnostic);
			m_xmlIni.Open(m_strIniFilename);
			
			//	Initialize the error log
			m_xmlErrors.Folder = m_strAppFolder;
			m_xmlErrors.Filename = "EL_";
			m_xmlErrors.AddDateToFilename = true;
			m_xmlErrors.Extension = ".xml";
			m_xmlErrors.Root = "Errors";
			m_xmlErrors.Comments.Add("TrialMax " + ver.Version + " Error Log");
			m_xmlErrors.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);
			
			//	Initialize the diagnostics log
			m_xmlDiagnostics.Folder = m_strAppFolder;
			m_xmlDiagnostics.Filename = "DL_";
			m_xmlDiagnostics.AddDateToFilename = true;
			m_xmlDiagnostics.Extension = ".xml";
			m_xmlDiagnostics.Root = "Diagnostics";
			m_xmlDiagnostics.Comments.Add("TrialMax " + ver.Version + " Diagnostics Log");
			m_xmlDiagnostics.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);
			
		}// InitializeFilenames()

		/// <summary>This method is called to initialize the collection of media types</summary>
		private void InitializeMembers()
		{
			string strLastCase = "";
			
			//	Initialize the application options
			SetSplashMessage("Loading application options");
			m_tmaxAppOptions.Load(m_xmlIni);
			m_tmaxAppOptions.ApplicationFolder = m_strAppFolder;

			//	Initialize the connection to the system registry
			SetSplashMessage("Initializing registry interface");
			m_tmaxRegistry.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.OnDiagnostic);
			m_tmaxRegistry.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);
			m_tmaxRegistry.Initialize();
			
			//	Check to see if a demo case is in the registry if no cases
			//	have been opened yet
			if(m_tmaxAppOptions.RecentlyUsed.Count == 0)
			{
				strLastCase = m_tmaxRegistry.GetLastCase();
				if((strLastCase != null) && (strLastCase.Length > 0))
				{
					//	Strip the filename added by the installer if it exists
					if(System.IO.Path.HasExtension(strLastCase) == true)
						strLastCase = System.IO.Path.GetDirectoryName(strLastCase);
					
					//	Add to recently used list if valid folder
					if(System.IO.Directory.Exists(strLastCase) == true)
						m_tmaxAppOptions.AddRecentlyUsed(strLastCase);
					
					//	Clear the registry
					m_tmaxRegistry.SetLastCase("");
					
				}// if((strLastCase != null) && (strLastCase.Length > 0))
				
			}// if(m_tmaxAppOptions.RecentlyUsed.Count == 0)
			
			//	Initialize the registration options
			SetSplashMessage("Loading registration options");
			m_tmaxRegOptions.Load(m_xmlIni);
			
			//	Initialize the presentation options
			SetSplashMessage("Loading presentation options");
			m_tmaxPresentationOptions.Load(m_strPresentationIniFileSpec);
			
			Debug.Assert(m_tmaxSourceTypes != null);
			if(m_tmaxSourceTypes == null) return;
			
			//	Retrieve the source filters from the configuration file
			SetSplashMessage("Loading source descriptors");
			m_tmaxSourceTypes.Load(m_xmlIni);
			
			//	Initialize the media encoder
			SetSplashMessage("Initializing media encoder");
			if(m_mediaEncoder.Initialize(null) == true)
				m_mediaEncoder.Load(m_xmlIni);
			
			//	Initialize the report manager
			if(m_tmaxReportManager != null)
			{
				SetSplashMessage("Initializing report manager");
				m_tmaxReportManager.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);
				m_tmaxReportManager.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.OnDiagnostic);
				m_tmaxReportManager.AppOptions = m_tmaxAppOptions;
				m_tmaxReportManager.ProductManager = m_tmaxProductManager;
				m_tmaxReportManager.Load(m_xmlIni);
			}
			
			//	Initialize the asynchronous message filter
			if(m_tmaxAsyncFilter != null)
			{
				m_tmaxAsyncFilter.TmaxAsyncMessage += new CTmaxAsyncFilter.TmaxAsyncHandler(this.OnTmaxAsyncMessage);
			}
				
			//	Initialize the keyboard hook
			if(m_tmaxKeyboard != null)
			{
				m_tmaxKeyboard.TmaxKeyDown += new CTmaxKeyboard.TmaxKeyDownHandler(this.OnTmaxKeyDown);
				m_tmaxKeyboard.TmaxHotkey += new CTmaxKeyboard.TmaxHotkeyHandler(this.OnTmaxHotkey);
				m_tmaxEventSource.Attach(m_tmaxKeyboard.EventSource);
			}
				
			//	Initialize the application's movie control
			//
			//	NOTE:	This is used by the database to retrieve file information
			//			during registration
			if(m_tmxMovie != null)
			{
				m_tmxMovie.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.OnDiagnostic);
				m_tmxMovie.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);
				if(m_tmxMovie.AxInitialize() == false)
					m_tmxMovie = null;
			}
			
			//	Initialize the application's viewer control
			//
			//	NOTE:	This is used by the database to split Multi-page tiffs
			if(m_tmxView != null)
			{
				m_tmxView.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.OnDiagnostic);
				m_tmxView.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);
				if(m_tmxView.AxInitialize() == false)
					m_tmxView = null;
			}
			
			//	Make sure the TMPrint control stays out of the way
			if(m_tmxPrint != null)
			{
				m_tmxPrint.Location = new Point(0,0);
				m_tmxPrint.Size = new Size(10,10);
				if(m_tmaxAppOptions.EnableDIBPrinting == true)
					m_tmxPrint.EnableDIBPrinting(1);
				else
					m_tmxPrint.EnableDIBPrinting(0);
			}
				
		}// private void InitializeMembers()
		
		/// <summary>This method is called to initialize each child pane</summary>
		private void InitializePanes()
		{
			string strPane = "";
			
			try
			{
				//	Set up the array of panes
				m_aPanes[(int)TmaxAppPanes.Errors] = m_paneErrors;
				m_aPanes[(int)TmaxAppPanes.Diagnostics] = m_paneDiagnostics;
				m_aPanes[(int)TmaxAppPanes.Media] = m_paneMedia;
				m_aPanes[(int)TmaxAppPanes.Source] = m_paneSource;
				m_aPanes[(int)TmaxAppPanes.Binders] = m_paneBinders;
				m_aPanes[(int)TmaxAppPanes.Viewer] = m_paneViewer;
				m_aPanes[(int)TmaxAppPanes.Properties] = m_paneProperties;
				m_aPanes[(int)TmaxAppPanes.Scripts] = m_paneScripts;
				m_aPanes[(int)TmaxAppPanes.Transcripts] = m_paneTranscripts;
				m_aPanes[(int)TmaxAppPanes.Tuner] = m_paneTuner;
				m_aPanes[(int)TmaxAppPanes.Results] = m_paneResults;
				m_aPanes[(int)TmaxAppPanes.Help] = m_paneHelp;
				m_aPanes[(int)TmaxAppPanes.Versions] = m_paneVersions;
				m_aPanes[(int)TmaxAppPanes.Codes] = m_paneCodes;
				m_aPanes[(int)TmaxAppPanes.FilteredTree] = m_paneFiltered;
				m_aPanes[(int)TmaxAppPanes.Objections] = m_paneObjections;
				m_aPanes[(int)TmaxAppPanes.ObjectionProperties] = m_paneObjectionProperties;
				m_aPanes[(int)TmaxAppPanes.ScriptReview] = m_paneScriptReview;
				
				//	Set the pane properties
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					if(m_aPanes[i] != null)
					{
						//	Just in case there's an error
						strPane = m_aPanes[i].PaneName;
						
						SetSplashMessage("Initializing " + strPane + " pane");

						//	Set the pane identifier
						m_aPanes[i].PaneId = i;
						
						//	Register the event handlers
						m_aPanes[i].TmaxCommandEvent += new FTI.Shared.Trialmax.TmaxCommandHandler(this.OnTmaxCommand);
						m_aPanes[i].EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);
						m_aPanes[i].EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.OnDiagnostic);
					
						//	Set references to application data objects
						m_aPanes[i].RegistrationOptions = m_tmaxRegOptions;
						m_aPanes[i].MediaTypes = m_tmaxMediaTypes;
						m_aPanes[i].SourceTypes = m_tmaxSourceTypes;
						m_aPanes[i].TmaxClipboard = m_tmaxClipboard;
						m_aPanes[i].TmaxRegistry = m_tmaxRegistry;
						m_aPanes[i].ReportManager = m_tmaxReportManager;
						m_aPanes[i].ApplicationOptions = m_tmaxAppOptions;
						m_aPanes[i].PresentationOptions = m_tmaxPresentationOptions;
						m_aPanes[i].PresentationOptionsFilename = m_strPresentationIniFileSpec;
						
						//	Perform runtime initialization
						m_aPanes[i].Initialize(m_xmlIni);
					}
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "InitializePanes", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_PANES_EX, strPane), Ex);
			}

		}// private void InitializePanes()
		
		/// <summary>This method is called to initialize the application product manager</summary>
		private void InitializeProductManager()
		{
			try
			{
				//	Initialize the product descriptor
				m_tmaxProductManager.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.OnDiagnostic);
				m_tmaxProductManager.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);
				m_tmaxProductManager.Registry = m_tmaxRegistry;
				m_tmaxProductManager.Load(m_xmlIni);
				m_tmaxProductManager.FillComponents();

				//	Set the pane properties
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					if(m_aPanes[i] != null)
						m_aPanes[i].TmaxProductManager = m_tmaxProductManager;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "InitializeProductManager", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_PRODUCT_MANAGER_EX), Ex);
			}

		}// private void InitializePanes()
		
		/// <summary>This method is called to initialize the interfaces to the Presentation application</summary>
		private void InitializePresentation()
		{
			//	Connect to the application sharing control events
			m_ctrlPresentation.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);
			m_ctrlPresentation.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.OnDiagnostic);
			m_ctrlPresentation.AppFolder = m_strAppFolder;
			m_ctrlPresentation.AxInitialize();
		
		}// private void InitializePresentation()
		
		/// <summary>This function is called to initialize the Infragistics toolbar/menu manager</summary>
		private void InitializeToolbarManager()
		{ 
			PopupMenuTool PopupMenu;

			//	Modify the File menu
			try
			{
				PopupMenu = (PopupMenuTool)m_ctrlUltraToolbarManager.Tools["FileMenu"];

				PopupMenu.Settings.IsSideStripVisible							= DefaultableBoolean.True;
				PopupMenu.Settings.SideStripText								= "TrialMax Manager 7";
				PopupMenu.Settings.SideStripAppearance.ForeColor				= Color.White;
				PopupMenu.Settings.SideStripAppearance.BackColor				= Color.WhiteSmoke;
				PopupMenu.Settings.SideStripAppearance.BackColor2				= Color.Navy;
				PopupMenu.Settings.SideStripAppearance.BackGradientStyle		= GradientStyle.Vertical;
				PopupMenu.Settings.SideStripAppearance.FontData.Bold			= DefaultableBoolean.True;
				PopupMenu.Settings.SideStripAppearance.FontData.Italic 			= DefaultableBoolean.True;
				PopupMenu.Settings.SideStripAppearance.FontData.SizeInPoints	= 10;
				PopupMenu.Settings.SideStripAppearance.FontData.Name			= "Arial";
				PopupMenu.Settings.SideStripAppearance.TextVAlign				= VAlign.Middle;
				
				//	Make sure the Recently Used selections are initialized just in case the
				//	user presses Control+L without having opened the File menu
				SetRecentlyUsed();
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "InitializeToolbarManager", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_TOOLBAR_EX), Ex);
			}
			
		}// private void InitializeToolbarManager()
		
		/// <summary>This method is called to initialize the versions pane with the information for all assemblies and ActiveX controls</summary>
		private void InitializeVersions()
		{
			CFPresentationOptions	presentationOptions = null;
			ArrayList				aAxVersions = null;
			CTmaxComponent			tmaxComponent = null;
			CBaseVersion			baseVersion = null;		
			
			Debug.Assert(m_paneVersions != null);
			if(m_paneVersions == null) return;
			Debug.Assert(m_paneVersions.IsDisposed == false);
			if(m_paneVersions.IsDisposed == true) return;
			Debug.Assert(m_aVersions != null);
			if(m_aVersions == null) return;
			
			try
			{
				//	Add version descriptors for each of the .NET assemblies
				m_aVersions.Add(new CTmaxManagerVersion());
				m_aVersions.Add(new CTmpanesVersion());
				m_aVersions.Add(new CTmreportsVersion());
				m_aVersions.Add(new CTmdataVersion());
				m_aVersions.Add(new CTmformsVersion());
				m_aVersions.Add(new CTmctrlsVersion());
				m_aVersions.Add(new CTmactxVersion());
				m_aVersions.Add(new CTmofficeVersion());
				m_aVersions.Add(new CTmencodeVersion());
				m_aVersions.Add(new CTmsharedVersion());
				
				if((baseVersion = GetInstallerVersion()) != null)
					m_aVersions.Add(baseVersion);
					
				//	Add the Presentation/ActiveX versions
				try
				{
					if((presentationOptions = new CFPresentationOptions()) != null)
					{
						if((aAxVersions = presentationOptions.GetAxVersions(true)) != null)
						{
							foreach(CBaseVersion O in aAxVersions)
								m_aVersions.Add(O);
						}
							
						presentationOptions.Dispose();					
					}
					
				}
				catch
				{
				}
				
				//	Add a blank line
				m_aVersions.Add(new CBaseVersion());
				
				//	Add PDF converter
				if((m_tmaxProductManager != null) && (m_tmaxProductManager.Components != null))
				{
					if((tmaxComponent = m_tmaxProductManager.Components.Find(TmaxComponents.FTIP2I)) != null)
					{
						if((baseVersion = tmaxComponent.GetBaseVersion()) != null)
							m_aVersions.Add(baseVersion);
					}
				}

				//	Add Objections Report Generator
				if((m_tmaxProductManager != null) && (m_tmaxProductManager.Components != null))
				{
					if((tmaxComponent = m_tmaxProductManager.Components.Find(TmaxComponents.FTIORE)) != null)
					{
						if((baseVersion = tmaxComponent.GetBaseVersion()) != null)
							m_aVersions.Add(baseVersion);
					}
				}

				//	Add Windows Media Encoder
				if((m_tmaxProductManager != null) && (m_tmaxProductManager.Components != null))
				{
					if((tmaxComponent = m_tmaxProductManager.Components.Find(TmaxComponents.WMEncoder)) != null)
					{
						if((baseVersion = tmaxComponent.GetBaseVersion()) != null)
							m_aVersions.Add(baseVersion);
					}
				}
				
				//	Add version information for .NET Framework
				baseVersion = new CBaseVersion();
				baseVersion.Title = "NET Framework";				
				baseVersion.Major = System.Environment.Version.Major;
				baseVersion.Minor = System.Environment.Version.Minor;
				baseVersion.QEF = System.Environment.Version.Build;
				baseVersion.SetVersionText(false);
				m_aVersions.Add(baseVersion);
				
				//	Add a blank line
				m_aVersions.Add(new CBaseVersion());
				
				//	Add version information for the database
				m_verDatabase = new CBaseVersion();
				m_aVersions.Add(m_verDatabase);
				
				//	Add the versions to the pane
				foreach(CBaseVersion O in m_aVersions)
					m_paneVersions.Add(O);
					
				//	Make sure the database information is current
				SetDatabaseVersion();

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "InitializeVersions", m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_VERSIONS_EX), Ex);
			}

		}// private void InitializeVersions()
		
		/// <summary>This function is called to load the pane docking layout from file</summary>
		/// <param name="strFilename">Name of layout file to be loaded</param>
		/// <returns>true if successful</returns>
		private bool LoadPaneLayout(string strFilename)
		{
			System.IO.FileStream fsLayout = null; 
			bool bReturn = false;

			//	Make sure the file exists
			if(System.IO.File.Exists(strFilename) == false) return false;
			
			try
			{
				//	Notify the panes that we are about to load a new layout
				foreach(CBasePane O in m_aPanes)
				{
					try	{ O.OnBeforeLoadLayout(); }
					catch {}
				}
					
				fsLayout = new System.IO.FileStream(strFilename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);

				if(fsLayout != null)
				{
					this.m_ctrlUltraDockManager.LoadFromXML(fsLayout);
					bReturn = true;
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "LoadPaneLayout", m_tmaxErrorBuilder.Message(ERROR_LOAD_LAYOUT_EX, strFilename), Ex);
			}
			finally
			{
				if(fsLayout != null)
					fsLayout.Close();
				
				//	Do all required post-processing
				OnLayoutLoaded();
					
			}// finally
			
			return bReturn;
		
		}// LoadPaneLayout()
		
		/// <summary>This method is called when the user wants to import a load file</summary>
		private void OnAppImportLoadFile()
		{
			FTI.Trialmax.Panes.CFImportWizard wizard = null;
			CTmaxSourceFolder tmaxSource = null;
			
			try
			{
				wizard = new FTI.Trialmax.Panes.CFImportWizard();
				
				//	Set the wizard's properties
				wizard.RegisterOptions = m_tmaxRegOptions;
				wizard.WizardOptions = m_tmaxAppOptions.ImportWizard;
				wizard.Database = m_tmaxDatabase;
				
				//	Connect to the wizard events
				wizard.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.OnDiagnostic);
				wizard.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);
				wizard.TmaxCommandEvent += new FTI.Shared.Trialmax.TmaxCommandHandler(this.OnTmaxCommand);
	
				DisableTmaxKeyboard(true);
				
				//	Show the form to the user
				if(wizard.ShowDialog() == DialogResult.OK)
				{
					if((tmaxSource = wizard.RegisterSource) != null)
					{
						//	Register the top-level (parent) item
						if(m_tmaxDatabase.Register(tmaxSource, null) == true)
						{
							//	Notify each pane
							for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
							{
								if(m_aPanes[i] != null)
								{
									m_aPanes[i].OnRegistered(tmaxSource, TmaxAppPanes.Media);
								}
					
							}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				
						}
					
					}// if(wizard.RegisterSource != null)
					
				}// if(wizard.ShowDialog() == DialogResult.OK)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnAppImportLoadFile", m_tmaxErrorBuilder.Message(ERROR_IMPORT_LOAD_FILE_EX), Ex);
			}
			finally
			{
				DisableTmaxKeyboard(false);
			}
			
		}// private void OnAppImportLoadFile()
		
		/// <summary>This method is called to handle pane events where CommandId == AcceptDrop</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdAcceptDrop(object objSender, CTmaxCommandArgs Args)
		{
			//	Update the drop target
			m_eDropPane = (TmaxAppPanes)Args.Source;

		}// OnCmdAcceptDrop(CPaneEventArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == Activate</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdActivate(object objSender, CTmaxCommandArgs Args)
		{
			CTmaxParameter	tmaxParameter;
			bool			bSyncMedia = false;
			bool			bSyncFiltered = false;
			bool			bEnsureVisible = true;
			bool			bObjection = false;
			bool			bOpenTranscript = false;

			Debug.Assert(Args != null);
			Debug.Assert(Args.Items != null);
			Debug.Assert(Args.Items.Count == 1);
			
			//	Get the optional parameters
			if((tmaxParameter = Args.GetParameter(TmaxCommandParameters.SyncMediaTree)) != null)
				bSyncMedia = tmaxParameter.AsBoolean();
			if((tmaxParameter = Args.GetParameter(TmaxCommandParameters.SyncFilterTree)) != null)
				bSyncFiltered = tmaxParameter.AsBoolean();
			if((tmaxParameter = Args.GetParameter(TmaxCommandParameters.Objections)) != null)
				bObjection = tmaxParameter.AsBoolean();

			if(bObjection == true)
			{
				if((tmaxParameter = Args.GetParameter(TmaxCommandParameters.Transcripts)) != null)
					bOpenTranscript = tmaxParameter.AsBoolean();
			}
			
			//	Is the simulator active?
			//
			//	We need to do this until we figure out why the tree blows
			//	up if driven by the simulator long enough
			if(m_tmaxSimulator != null)
				bEnsureVisible = (m_tmaxSimulator.Active == false);
					
			//	Are we supposed to sync the media tree
			if((bSyncMedia == true) && (m_paneMedia != null))
			{
				//	Set the selection in the tree
				m_paneMedia.Select(Args.Items[0], false, bEnsureVisible);
			
				//	Should we make the tree visible?
				if(IsVisible(m_paneMedia) == false)
				{
					//	The sister tree (filtered) must be visible for use to force this to the top
					if(IsVisible(m_paneFiltered) == true)
						SetUltraPaneVisible(TmaxAppPanes.Media, true, true);
				}
				
			}// if((bSyncMedia == true) && (m_paneMedia != null))
			
			if((bSyncFiltered == true) && (m_paneFiltered != null))
			{
				m_paneFiltered.Select(Args.Items[0], false, bEnsureVisible);
			
				//	Should we make the tree visible?
				if(IsVisible(m_paneFiltered) == false)
				{
					//	The sister tree (media) must be visible for use to force this to the top
					if(IsVisible(m_paneMedia) == true)
						SetUltraPaneVisible(TmaxAppPanes.FilteredTree, true, true);
				}
				
			}// if((bSyncFiltered == true) && (m_paneFiltered != null))

			//	Should we make the transcript pane visible?
			if((bObjection == true) && (bOpenTranscript == true) && (m_paneTranscripts != null))
			{
				if(IsVisible(m_paneTranscripts) == false)
				{
					SetUltraPaneVisible(TmaxAppPanes.Transcripts, true, true);
				}

			}// if((bObjection == true) && (m_paneTranscripts != null))

			//	Notify the panes
			if((Args.Items != null) && (Args.Items.Count > 0))
			{
				//	Notify each pane
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					if(m_aPanes[i] != null)
					{
						try
						{
							if(bObjection == true)
								m_aPanes[i].OnObjectionSelected(Args.Items[0], (TmaxAppPanes)Args.Source);
							else
								m_aPanes[i].Activate(Args.Items[0], (TmaxAppPanes)Args.Source);
						}
						catch(System.Exception Ex)
						{
							m_tmaxEventSource.FireError(this, "OnCmdActivate", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_ACTIVATE_EX, ((TmaxAppPanes)i).ToString()), Ex);
						}
					}
						
				}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
					
			}// if((Args.Items != null) && (Args.Items.Count > 0))

			//	Mark the item as processed
			Args.Items[0].State = TmaxItemStates.Processed;
			
			//	Acknowledge the request
			Args.Successful = true;
			
		}// OnCmdActivate(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == Rotate</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdRotate(object objSender, CTmaxCommandArgs Args)
		{
			CTmaxParameter	tmaxParameter;
			bool			bCCW = false;
			bool			bSuccessful = false;

			Debug.Assert(m_paneViewer != null);
			Debug.Assert(m_paneViewer.IsDisposed == false);
			if(m_paneViewer == null) return;
			if(m_paneViewer.IsDisposed == true) return;

			Debug.Assert(Args != null);
			Debug.Assert(Args.Items != null);
			Debug.Assert(Args.Items.Count == 1);
			
			//	Get the optional parameters
			if((tmaxParameter = Args.GetParameter(TmaxCommandParameters.CounterClockwise)) != null)
				bCCW = tmaxParameter.AsBoolean();

			//	Is the media viewer active?
			if(m_paneViewer.PaneVisible == false)
			{
				//	Make sure the viewer is opened before requesting rotation
				m_paneViewer.Open(Args.Items[0], (TmaxAppPanes)Args.Source);
				SetUltraPaneVisible(TmaxAppPanes.Viewer, true, false);
				
				//	Make sure the Active property for the panes is properly set
				SetPaneStates();
			}
				
			//	Ask the viewer pane to rotate this image
			bSuccessful = m_paneViewer.Rotate(Args.Items[0], bCCW);

			//	Mark the item as processed
			Args.Items[0].State = TmaxItemStates.Processed;
			Args.Successful = bSuccessful;
			
		}// OnCmdRotate(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == Add</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdAdd(object objSender, CTmaxCommandArgs Args)
		{
			CTmaxParameter			tmaxParameter;
			CTmaxDatabaseResults	tmaxResults = new CTmaxDatabaseResults();
			bool					bActivate = false;
			
			Debug.Assert(Args != null);
			Debug.Assert(Args.Items != null);
			Debug.Assert(Args.Items.Count == 1);
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);
			
			//	Just in case...
			if((Args == null) || (m_tmaxDatabase == null) || (m_tmaxDatabase.Primaries == null)) return;
			
			//	Get the optional parameters
			if((tmaxParameter = Args.GetParameter(TmaxCommandParameters.Activate)) != null)
				bActivate = tmaxParameter.AsBoolean();
				
			//	Process the request
			Args.Successful = m_tmaxDatabase.Add(Args.Items[0], Args.Parameters, tmaxResults);
			Args.Items[0].State = TmaxItemStates.Processed;

			//	Add to the physical tree if successful
			if(Args.Successful == true)
			{
				//	Notify each pane
				Notify(tmaxResults);

				//	Should we activate the new item?
				if((bActivate == true) && (m_paneMedia != null))
				{
					if((tmaxResults.Added.Count > 0) && (tmaxResults.Added[0].SubItems.Count > 0))
						m_paneMedia.Select(tmaxResults.Added[0].SubItems[0], true, true);
				}

			}// if(Args.Successful == true)
			
		}// OnCmdAdd(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == AddNotification</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdAddNotification(object objSender, CTmaxCommandArgs Args)
		{
			Debug.Assert(Args != null);
			Debug.Assert(Args.Items != null);
			Debug.Assert(Args.Items.Count == 1);
		
			//	Just in case...
			if((Args == null) || (Args.Items == null) || (Args.Items.Count == 0)) return;
			
			Args.Successful = false;
			
			try
			{
				if((m_aIAppNotifications != null) && (Args.Items[0].IAppNotification != null))
				{
					if(m_aIAppNotifications.Contains(Args.Items[0].IAppNotification) == false)
						m_aIAppNotifications.Add(Args.Items[0].IAppNotification);
					
					Args.Successful = true;
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnCmdAddNotification", Ex);
			}
			
		}// OnCmdAddNotification(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == AddNotification</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdEndNotification(object objSender, CTmaxCommandArgs Args)
		{
			Debug.Assert(Args != null);
			Debug.Assert(Args.Items != null);
			Debug.Assert(Args.Items.Count == 1);
		
			//	Just in case...
			if((Args == null) || (Args.Items == null) || (Args.Items.Count == 0)) return;
			
			Args.Successful = false;
			
			try
			{
				if((m_aIAppNotifications != null) && (Args.Items[0].IAppNotification != null))
				{
					if(m_aIAppNotifications.Contains(Args.Items[0].IAppNotification) == true)
						m_aIAppNotifications.Remove(Args.Items[0].IAppNotification);
					
					Args.Successful = true;
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnCmdEndNotification", Ex);
			}
			
		}// OnCmdEndNotification(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == Move</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdMove(object objSender, CTmaxCommandArgs Args)
		{
			Debug.Assert(Args != null);
			Debug.Assert(Args.Items != null);
			Debug.Assert(Args.Items.Count == 1);
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);
			
			//	Just in case...
			if((Args == null) || (m_tmaxDatabase == null) || (m_tmaxDatabase.Primaries == null)) return;
			
			//	Process the request
			Args.Successful = m_tmaxDatabase.Move(Args.Items[0], Args.Parameters);
			Args.Items[0].State = TmaxItemStates.Processed;

			//	Notify the panes if successful
			if(Args.Successful == true)
			{
				//	Notify each pane
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					if(m_aPanes[i] != null)
					{
						try { m_aPanes[i].OnMoved(Args.Items[0]); }
						catch {}
					}
					
				}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				
			}
			
		}// OnCmdMove(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == Merge</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdMerge(object objSender, CTmaxCommandArgs Args)
		{
			CTmaxDatabaseResults	tmaxResults = new CTmaxDatabaseResults();
			CTmaxParameter			tmaxParameter;
			bool					bActivate = false;
			
			Debug.Assert(Args != null);
			Debug.Assert(Args.Items != null);
			Debug.Assert(Args.Items.Count == 1);
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);
			
			//	Just in case...
			if((Args == null) || (m_tmaxDatabase == null) || (m_tmaxDatabase.Primaries == null)) return;
			
			//	Get the optional parameters
			if((tmaxParameter = Args.GetParameter(TmaxCommandParameters.Activate)) != null)
				bActivate = tmaxParameter.AsBoolean();
				
			//	Process the request
			Args.Successful = m_tmaxDatabase.Merge(Args.Items[0], tmaxResults);
			Args.Items[0].State = TmaxItemStates.Processed;

			if((Args.Successful == true) && (tmaxResults.Added.Count > 0))
			{
				//	Notify each pane
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					if(m_aPanes[i] != null)
					{
						m_aPanes[i].OnAdded(tmaxResults.Added[0], tmaxResults.Added[0].SubItems);
					}
					
				}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				
				//	Should we activate the new item?
				if((bActivate == true) && (m_paneMedia != null))
				{
					m_paneMedia.Select(tmaxResults.Added[0].SubItems[0], true, true);
				}

			}
			
		}// OnCmdMerge(object objSender, CTmaxCommandArgs Args)

		/// <summary>This method is called to handle pane events where CommandId == Navigate</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdNavigate(object objSender, CTmaxCommandArgs Args)
		{
			Debug.Assert(Args != null);

			Args.Successful = false;

			try
			{
				//	Notify each pane
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					if(m_aPanes[i] != null)
					{
						m_aPanes[i].OnNavigate((TmaxAppPanes)Args.Source, Args.Items, Args.Parameters);
					}

				}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)

				Args.Successful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnCmdNavigate", Ex);
			}

		}// private void OnCmdNavigate(object objSender, CTmaxCommandArgs Args)

		/// <summary>This method is called to handle pane events where CommandId == NavigatorChanged</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdNavigatorChanged(object objSender, CTmaxCommandArgs Args)
		{
			Debug.Assert(Args != null);

			Args.Successful = false;

			try
			{
				//	Notify each pane
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					if(m_aPanes[i] != null)
					{
						m_aPanes[i].OnNavigatorChanged((TmaxAppPanes)Args.Source, Args.Items, Args.Parameters);
					}

				}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)

				Args.Successful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnCmdNavigatorChanged", Ex);
			}

		}// private void OnCmdNavigatorChanged(object objSender, CTmaxCommandArgs Args)

		/// <summary>This method is called to handle pane events where CommandId == Import</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdImport(object objSender, CTmaxCommandArgs Args)
		{
			CTmaxDatabaseResults	tmaxResults = new CTmaxDatabaseResults();
			bool					bActivate = false;

			Debug.Assert(Args != null);
			Debug.Assert(Args.Items != null);
			Debug.Assert(Args.Items.Count == 1);
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);
			
			//	Just in case...
			if(Args == null) return;
			if(Args.Items == null) return;
			if(Args.Items.Count == 0) return;
			if(m_tmaxDatabase == null) return;
			if(m_tmaxDatabase.Primaries == null) return;

			try
			{
				DisableTmaxKeyboard(true);
				
				//	Process the request
				Args.Successful = m_tmaxDatabase.Import(Args.Parameters, Args.Items[0], tmaxResults);
				Args.Items[0].State = TmaxItemStates.Processed;
				OnAppReloadCase();
				if(Args.Successful == true)
				{
					//	Notify each pane
					Notify(tmaxResults);

					//	Should we activate the new item?
					if((bActivate == true) && (m_paneMedia != null))
					{
						if((tmaxResults.Added.Count > 0) && (tmaxResults.Added[0].SubItems.Count > 0))
							m_paneMedia.Select(tmaxResults.Added[0].SubItems[0], false, true);
					}
					
				}// if(Args.Successful == true)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnCmdImport", Ex);
			}
			finally
			{
				DisableTmaxKeyboard(false);
			}
			
		}// private void OnCmdImport(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == Export</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdExport(object objSender, CTmaxCommandArgs Args)
		{
			Debug.Assert(Args != null);
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);
			
			//	Just in case...
			if((Args == null) || (m_tmaxDatabase == null) || (m_tmaxDatabase.Primaries == null)) return;
			
			try
			{
				DisableTmaxKeyboard(true);
				
				//	Initialize the return
				Args.Successful = m_tmaxDatabase.Export(Args.Parameters, Args.Items);

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnCmdExport", Ex);
			}
			finally
			{
				DisableTmaxKeyboard(false);
			}
			
		}// private void OnCmdExport(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == Duplicate</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdDuplicate(object objSender, CTmaxCommandArgs Args)
		{
			CTmaxParameter	tmaxParameter;
			CTmaxItems		tmaxAdded = new CTmaxItems();
			bool			bActivate = false;
			
			Debug.Assert(Args != null);
			Debug.Assert(Args.Items != null);
			Debug.Assert(Args.Items.Count == 1);
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);
			
			//	Just in case...
			if((Args == null) || (m_tmaxDatabase == null) || (m_tmaxDatabase.Primaries == null)) return;
			
			//	Get the optional parameters
			if((tmaxParameter = Args.GetParameter(TmaxCommandParameters.Activate)) != null)
				bActivate = tmaxParameter.AsBoolean();
				
			//	Process the request
			Args.Successful = m_tmaxDatabase.Duplicate(Args.Items[0], tmaxAdded);
			Args.Items[0].State = TmaxItemStates.Processed;

			//	Add to the physical tree if successful
			if((Args.Successful == true) && (tmaxAdded.Count > 0))
			{
				//	Note:	The database returns the parent as the first item in the
				//			return collection. The new duplicate record is returned as
				//			the first subitem in the parent collection
				Debug.Assert(tmaxAdded.Count == 1);
				Debug.Assert(tmaxAdded[0].SubItems != null);
				Debug.Assert(tmaxAdded[0].SubItems.Count == 1);
				
				//	Notify each pane
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					if(m_aPanes[i] != null)
					{
						m_aPanes[i].OnAdded(tmaxAdded[0], tmaxAdded[0].SubItems);
					}
					
				}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				
				//	Should we activate the new item?
				if((bActivate == true) && (m_paneMedia != null))
				{
					m_paneMedia.Select(tmaxAdded[0].SubItems[0], false, true);
				}
				
			}
			
		}// private void OnCmdDuplicate(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle CompleteDrag command events</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdCompleteDrag(object objSender, CTmaxCommandArgs Args)
		{
			//	Notify the child panes
			for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
			{
				if(m_aPanes[i] != null)
				{
					try
					{
						if((TmaxAppPanes)Args.Source == TmaxAppPanes.Source)
						{
							m_aPanes[i].OnCompleteSourceDrag();
						}
						else
						{
							m_aPanes[i].OnCompleteDataDrag();
						}
						
					}
					catch
					{
					
					}
				
				}
					
			}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
			
		}// OnCmdCompleteDrag(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == Copy</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdCopy(object objSender, CTmaxCommandArgs Args)
		{
			string strText = "";
			
			Debug.Assert(Args != null);
			Debug.Assert(Args.Items != null);
			
			if((m_tmaxClipboard != null) && (Args.Items != null))
			{
				//	Clear the local clipboard
				m_tmaxClipboard.Clear();
				
				//	Put each of the items in the local clipboard
				foreach(CTmaxItem tmaxItem in Args.Items)
					m_tmaxClipboard.Add(tmaxItem);
					
				//	Copy the barcode of the first item to the real Window's clipboard
				if(Args.Items.Count > 0)
				{
					if(Args.Items[0].GetMediaRecord() != null)
						strText = Args.Items[0].GetMediaRecord().GetBarcode(false);
				}
				try { Clipboard.SetDataObject(strText, false); } 
				catch{}
				
				//	Notify each pane
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					if(m_aPanes[i] != null)
					{
						m_aPanes[i].OnClipboardUpdated();
					}
					
				}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				
			}
			
			//	Mark the item as processed
			Args.Items[0].State = TmaxItemStates.Processed;
			
			//	Acknowledge the request
			Args.Successful = true;
			
		}// OnCmdCopy(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == Delete</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdDelete(object objSender, CTmaxCommandArgs Args)
		{
			bool					bObjections = false;
			CTmaxDatabaseResults	tmaxResults = new CTmaxDatabaseResults();
			
			Debug.Assert(Args != null);
			Debug.Assert(Args.Items != null);
			Debug.Assert(Args.Items.Count == 1);
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);
			
			//	Just in case...
			if((Args == null) || (m_tmaxDatabase == null) || (m_tmaxDatabase.Primaries == null)) return;

			//	Are we updating objections
			bObjections = IsObjectionsCommand(Args.Parameters);
			
			//	Initialize the return
			Args.Successful = true;
			
			Cursor.Current = Cursors.WaitCursor;
			
			//	Delete the item
			if(m_tmaxDatabase.Delete(Args.Items[0], Args.Parameters, tmaxResults) == true)
			{
				Notify(tmaxResults);
				
			}// if(m_tmaxDatabase.Delete(tmaxItem) == true)
			
			Cursor.Current = Cursors.Default;
			
		}// OnCmdDelete(CPaneEventArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == Print</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdPrint(object objSender, CTmaxCommandArgs Args)
		{
			//	Print the specified items
			Print(Args.Items);
					
		}// private void OnCmdPrint(object objSender, CTmaxCommandArgs Args)
				
		/// <summary>This method is called to handle pane events where CommandId == Find</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdFind(object objSender, CTmaxCommandArgs Args)
		{
			Debug.Assert(Args != null);
			
			//	Pass the request to the search results pane
			if((m_paneResults != null) && (m_paneResults.IsDisposed == false))
			{
				if(Args.Source > 0)
					Args.Successful = Find(Args.Items, (TmaxAppPanes)Args.Source);
				else
					Args.Successful = Find(Args.Items, TmaxAppPanes.MaxPanes);
			}
			else
			{
				Args.Successful = false;
			}
			
			if(Args.Successful == true)
			{
				SetUltraPaneVisible(TmaxAppPanes.Results, true, false);
			}
			
		}// private void OnCmdFind(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == SetCodes</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdSetCodes(object objSender, CTmaxCommandArgs Args)
		{
			CTmaxParameter	tmaxParameter = null;
			CTmaxItems		tmaxModified = new CTmaxItems();
			TmaxCodeActions	eAction = TmaxCodeActions.Unknown;
			
			Debug.Assert(Args != null);
			Debug.Assert(Args.Items != null);
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);
			
			//	Just in case...
			if((Args == null) || (m_tmaxDatabase == null) || (m_tmaxDatabase.Primaries == null)) return;
			
			//	Initialize the return
			Args.Successful = true;
			
			//	Get the action parameter
			if((tmaxParameter = Args.GetParameter(TmaxCommandParameters.CodesAction)) != null)
				eAction = (TmaxCodeActions)(tmaxParameter.AsInteger());
				
			//	Perform the operation
			m_tmaxDatabase.SetCodes(Args.Items, tmaxModified, Args.Parameters);
			
			//	Notify the panes
			if(tmaxModified.Count > 0)
			{
				//	Notify each pane
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					if(m_aPanes[i] != null)
					{
						m_aPanes[i].OnSetCodes(tmaxModified, eAction);
					}
				
				}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
			}
			
		}// private void OnCmdSetCodes(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == SetTargetBinder</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdSetTargetBinder(object objSender, CTmaxCommandArgs Args)
		{
			CDxBinderEntry dxOldTarget = null;
			CDxBinderEntry dxNewTarget = null;
			
			Debug.Assert(Args != null);
			Debug.Assert(m_tmaxDatabase != null);
			
			if(m_tmaxDatabase != null)
			{
				//	Did the caller provide a new target?
				if((Args.Items != null) && (Args.Items.Count > 0))
					dxNewTarget = (CDxBinderEntry)(Args.Items[0].IBinderEntry);
					
				//	Store the current target so we can use it in the pane notifications
				dxOldTarget = m_tmaxDatabase.TargetBinder;
				
				//	Has the binder changed?
				if(ReferenceEquals(dxOldTarget, dxNewTarget) == false)
				{
					//	Notify the database
					m_tmaxDatabase.TargetBinder = dxNewTarget;
					
					//	Notify the child panes
					for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
					{
						if(m_aPanes[i] != null)
						{
							m_aPanes[i].OnTargetBinderChanged(dxOldTarget, dxNewTarget);
						}
				
					}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				
				}// if(ReferenceEquals(dxOldTarget, dxNewTarget) == false)
			
				//	Initialize the return
				Args.Successful = true;
			}
			
		}// private void OnCmdSetTargetBinder(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == BulkUpdate</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdBulkUpdate(object objSender, CTmaxCommandArgs Args)
		{
			CTmaxItems tmaxModified = new CTmaxItems();
			
			Debug.Assert(Args != null);
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);
			
			//	Just in case...
			if((Args == null) || (m_tmaxDatabase == null) || (m_tmaxDatabase.Primaries == null)) return;
			
			//	Perform the operation
			if(m_tmaxDatabase.BulkUpdate(Args.Items, tmaxModified, Args.Parameters) != false)
			{
				//	Notify the panes
				if(tmaxModified.Count > 0)
				{
					//	Notify each pane
					for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
					{
						if(m_aPanes[i] != null)
						{
							m_aPanes[i].OnBulkUpdate(tmaxModified);
						}
					
					}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				
				}// if(tmaxModified.Count > 0)
			
				Args.Successful = true;
			
			}// if(m_tmaxDatabase.BulkUpdate(Args.Items, tmaxModified, Args.Parameters) != false)
						
		}// private void OnCmdBulkUpdate(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == Export</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdSetFilter(object objSender, CTmaxCommandArgs Args)
		{
			Debug.Assert(Args != null);
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);
			
			//	Just in case...
			if((Args == null) || (m_tmaxDatabase == null) || (m_tmaxDatabase.Primaries == null)) return;
			
			//	Initialize the return
			Args.Successful = m_tmaxDatabase.SetFilter(Args.Items, Args.Parameters);
			
			//	Notify each of the child panes
			if(Args.Successful == true)
			{
				//	Notify each pane
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					if(m_aPanes[i] != null)
					{
						m_aPanes[i].Filtered = m_tmaxDatabase.Filtered;
					}
				
				}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
			
			}// if(Args.Successful == true)
			
		}// private void OnCmdSetFilter(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == Export</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdHideCaseCodes(object objSender, CTmaxCommandArgs Args)
		{
			Debug.Assert(Args != null);
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.CaseCodes != null);
			
			//	Just in case...
			if((Args == null) || (m_tmaxDatabase == null) || (m_tmaxDatabase.CaseCodes == null)) return;
			
			//	Initialize the return
			Args.Successful = m_tmaxDatabase.HideCaseCodes(Args.Items, Args.Parameters);
			
			//	Notify each of the child panes
			if(Args.Successful == true)
			{
				//	Notify each pane
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					if(m_aPanes[i] != null)
					{
						m_aPanes[i].OnHideCaseCodes(Args.Items);
					}
				
				}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
			
			}// if(Args.Successful == true)
			
		}// private void OnCmdHideCaseCodes(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == SetFilteredOrder</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdSetPrimariesOrder(object objSender, CTmaxCommandArgs Args)
		{
			CTmaxParameter	tmaxParameter = null;
			bool			bFiltered = false;
			
			Debug.Assert(Args != null);
		
			//	Get the filtered parameter
			if((tmaxParameter = Args.GetParameter(TmaxCommandParameters.Filtered)) != null)
				bFiltered = tmaxParameter.AsBoolean();
				
			//	Are we supposed to sync the tree
			//	Notify each pane
			for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
			{
				if(m_aPanes[i] != null)
				{
					try { m_aPanes[i].OnSetPrimariesOrder(bFiltered); }
					catch {}
				}
						
			}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)

			//	Mark the item as processed
			Args.Items[0].State = TmaxItemStates.Processed;
			
			//	Acknowledge the request
			Args.Successful = true;
			
		}// OnCmdSetFilteredOrder(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == SetSearchResult</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdSetSearchResult(object objSender, CTmaxCommandArgs Args)
		{
			CTmaxSearchResult	tmaxResult;
			CTmaxParameter		tmaxParameter;
			bool				bViewer = false;
			
			Debug.Assert(Args != null);
			Debug.Assert(Args.Items != null);
			Debug.Assert(Args.Items.Count == 1);
			Debug.Assert(Args.Items[0].UserData1 != null);
			
			//	Cast the item data to a search result
			try
			{
				if((tmaxResult = ((CTmaxSearchResult)(Args.Items[0].UserData1))) != null)
				{
					//	Should we open the transcript viewer
					if((tmaxParameter = Args.GetParameter(TmaxCommandParameters.Viewer)) != null)
						bViewer = tmaxParameter.AsBoolean();
				
					if(bViewer == true)
					{
						SetUltraPaneVisible(TmaxAppPanes.Transcripts, true, false);
						SetPaneStates();
					}
					
					//	Notify each pane
					for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
					{
						try
						{
							if(m_aPanes[i] != null)
							{
								m_aPanes[i].OnActivateResult(tmaxResult);
							}
						}
						catch(System.Exception Ex)
						{
							m_tmaxEventSource.FireError(this, "OnCmdSetSearchResult", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_SET_SEARCH_RESULT_EX, ((TmaxAppPanes)i).ToString()), Ex);
						}
						
					}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
					
				}
				
			}
			catch
			{
			}

			Args.Successful = true;
			
		}// private void OnCmdSetSearchResult(object objSender, CTmaxCommandArgs Args)

		/// <summary>This method is called to handle pane events where CommandId == SetDeposition</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdSetDeposition(object objSender, CTmaxCommandArgs Args)
		{
			Debug.Assert(Args != null);
			Debug.Assert(Args.Items != null);
			Debug.Assert(Args.Items.Count == 1);

			try
			{
				//	Notify each pane
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					try
					{
						if(m_aPanes[i] != null)
						{
							m_aPanes[i].OnSetDeposition(Args.Items[0], (TmaxAppPanes)Args.Source);
						}
					}
					catch(System.Exception Ex)
					{
						m_tmaxEventSource.FireError(this, "OnCmdSetDeposition", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_SET_DEPOSITION_EX, ((TmaxAppPanes)i).ToString()), Ex);
					}

				}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)

			}
			catch
			{
			}

			Args.Successful = true;

		}// private void OnCmdSetDeposition(object objSender, CTmaxCommandArgs Args)

		/// <summary>This method is called to handle pane events where CommandId == Edit</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdEdit(object objSender, CTmaxCommandArgs Args)
		{
			CDxMediaRecord		dxRecord = null;
			string			strFileSpec = "";
			int				iSlideId = -1;
			CMSPowerPoint	msPowerPoint = null;
			
			Debug.Assert(Args != null);
			Debug.Assert(Args.Items != null);
			Debug.Assert(Args.Items.Count == 1);
			
			//	Get the media record
			Debug.Assert(Args.Items[0].GetMediaRecord() != null);
			if((dxRecord = (CDxMediaRecord)Args.Items[0].GetMediaRecord()) == null) return;

			Cursor.Current = Cursors.WaitCursor;
			
			try
			{
				switch(dxRecord.MediaType)
				{
					case TmaxMediaTypes.Powerpoint:
					case TmaxMediaTypes.Slide:
					
						//	Connect to PowerPoint
						msPowerPoint = new FTI.Trialmax.MSOffice.MSPowerPoint.CMSPowerPoint();
						msPowerPoint.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.OnDiagnostic);
						if(msPowerPoint.Initialize() == false)
							break;

						//	Don't connect the error event until we're initialized
						//	just in case the machine does not have PowerPoint
						msPowerPoint.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);

						//	Is this the primary record?
						if(dxRecord.MediaType == TmaxMediaTypes.Powerpoint)
						{
							strFileSpec = m_tmaxDatabase.GetFileSpec((CDxPrimary)dxRecord);
						}
						else
						{
							Debug.Assert(((CDxSecondary)dxRecord).Primary != null);
							strFileSpec = m_tmaxDatabase.GetFileSpec(((CDxSecondary)dxRecord).Primary);
							iSlideId = (int)(((CDxSecondary)dxRecord).MultipageId);
						}
						
						//	Make sure the file exists
						if(System.IO.File.Exists(strFileSpec) == false)
						{
							m_tmaxEventSource.FireError(this, "OnCmdEdit", m_tmaxErrorBuilder.Message(ERROR_EDIT_FILE_NOT_FOUND, strFileSpec));
						}
						else
						{			
							//	Launch PowerPoint
							msPowerPoint.Execute(strFileSpec, iSlideId);
							//FTI.Shared.Win32.Shell.ShellExecute(this.Handle, "open", strFileSpec, "", "", FTI.Shared.Win32.User.SW_NORMAL);
						
							//	Mark the item as processed
							Args.Items[0].State = TmaxItemStates.Processed;
						
							//	Acknowledge the request
							Args.Successful = true;
							
							//	Notify the panes when we get focus back
							m_bNotifyOnActivate = true;
						}
						
						break;
						
					default:
					
						break;
						
				}
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdEdit", m_tmaxErrorBuilder.Message(ERROR_EDIT_EX, strFileSpec), Ex);
			}
			finally
			{
				if(msPowerPoint != null)
				{
					msPowerPoint.Terminate();
					msPowerPoint = null;
				}
				
				Cursor.Current = Cursors.Default;
				
			}
			
		}// OnCmdEdit(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == EditDesignation</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdEditDesignation(object objSender, CTmaxCommandArgs Args)
		{
			CTmaxDatabaseResults tmaxResults = new CTmaxDatabaseResults();
			
			Debug.Assert(Args != null);
			Debug.Assert(Args.Items != null);
			Debug.Assert(Args.Items.Count == 1);
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);
			
			//	Just in case...
			if((Args == null) || (m_tmaxDatabase == null) || (m_tmaxDatabase.Primaries == null)) return;
			
			//	Initialize the return
			Args.Successful = true;
			
			if(m_tmaxDatabase.EditDesignations(Args.Items, Args.Parameters, tmaxResults) == true)
			{
				Notify(tmaxResults);
			}
			
		}// private void OnCmdEditDesignation(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == Edit</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdSetCaseOptions(object objSender, CTmaxCommandArgs Args)
		{
			//	Make it look like the user clicked on the tools menu
			OnAppCaseOptions();
		}
		
		/// <summary>This message handles WM_INSTANCE_COMMAND_LINE messages sent to the window</summary>
		/// <param name="m">The message to be processed</param>
		private void OnWMInstanceCommandLine(ref Message m)
		{
			try
			{
				CTmaxCommandLine cmdLine = new CTmaxCommandLine();
				
				cmdLine.Folder = m_strAppFolder;
				
				if(cmdLine.Open(false) == true)
				{
					//	Process the arguments
					ProcessCommandLine(cmdLine);
				}
				
			}
			catch
			{
			}

			m.Result = IntPtr.Zero;

		}// private void OnWMInstanceCommandLine(ref Message m)

		/// <summary>This method is called to handle pane events where CommandId == Open</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdOpen(object objSender, CTmaxCommandArgs Args)
		{
			CTmaxParameter	tmaxParameter;
			bool			bViewer = false;
			bool			bBuilder = false;
			bool			bProperties = false;
			bool			bTuner = false;
			bool			bPresentation = false;
			bool			bCodes = false;
			long			lStartPL = 0;

			Debug.Assert(Args != null);
			Debug.Assert(Args.Items != null);
			Debug.Assert(Args.Items.Count == 1);
			
			if((Args.Items != null) && (Args.Items.Count > 0))
			{
				//	Get the parameters that define the target pane(s)
				if((tmaxParameter = Args.GetParameter(TmaxCommandParameters.Properties)) != null)
					bProperties = tmaxParameter.AsBoolean();
				if((tmaxParameter = Args.GetParameter(TmaxCommandParameters.Viewer)) != null)
					bViewer = tmaxParameter.AsBoolean();
				if((tmaxParameter = Args.GetParameter(TmaxCommandParameters.Builder)) != null)
					bBuilder = tmaxParameter.AsBoolean();
				if((tmaxParameter = Args.GetParameter(TmaxCommandParameters.Tuner)) != null)
					bTuner = tmaxParameter.AsBoolean();
				if((tmaxParameter = Args.GetParameter(TmaxCommandParameters.Presentation)) != null)
					bPresentation = tmaxParameter.AsBoolean();
				if((tmaxParameter = Args.GetParameter(TmaxCommandParameters.Codes)) != null)
					bCodes = tmaxParameter.AsBoolean();
				
				//	Should we open the properties pane?
				if((bProperties == true) && (m_paneProperties != null) && (m_paneProperties.IsDisposed == false))
				{
					m_paneProperties.Open(Args.Items[0], (TmaxAppPanes)Args.Source);
					SetUltraPaneVisible(TmaxAppPanes.Properties, true, false);
				}
	
				//	Should we open the fielded data pane?
				if((bCodes == true) && (m_paneCodes != null) && (m_paneCodes.IsDisposed == false))
				{
					m_paneCodes.Open(Args.Items[0], (TmaxAppPanes)Args.Source);
					SetUltraPaneVisible(TmaxAppPanes.Codes, true, false);
				}
	
				//	Should we open the viewer pane?
				if((bViewer == true) && (m_paneViewer != null) && (m_paneViewer.IsDisposed == false))
				{
					//	Is this a deposition?
					if(Args.Items[0].MediaType == TmaxMediaTypes.Deposition)
					{
						m_paneTranscripts.Open(Args.Items[0], (TmaxAppPanes)Args.Source);
						SetUltraPaneVisible(TmaxAppPanes.Transcripts, true, true);
					}
					else
					{
						m_paneViewer.Open(Args.Items[0], (TmaxAppPanes)Args.Source);
						SetUltraPaneVisible(TmaxAppPanes.Viewer, true, true);
					}
				
				}
	
				//	Should we open the builder?
				if((bBuilder == true) && (m_paneScripts != null) && (m_paneScripts.IsDisposed == false))
				{
					//m_tmaxEventSource.FireDiagnostic(this, "OnCmdOpen", "Opening script builder pane");
					m_paneScripts.Open(Args.Items[0], (TmaxAppPanes)Args.Source);
					SetUltraPaneVisible(TmaxAppPanes.Scripts, true, true);
				}
								
				//	Should we open the tuner?
				if((bTuner == true) && (m_paneTuner != null) && (m_paneTuner.IsDisposed == false))
				{
					m_paneTuner.Open(Args.Items[0], (TmaxAppPanes)Args.Source);
					SetUltraPaneVisible(TmaxAppPanes.Tuner, true, true);
				}
								
				//	Should we send to Presentation?
				if(bPresentation == true)
				{
					string strBarcode = m_tmaxDatabase.GetBarcode(Args.Items[0].GetMediaRecord(), true);
					if((strBarcode != null) && (strBarcode.Length > 0))
					{
						m_ctrlPresentation.CaseFolder = m_tmaxDatabase.Folder;
						m_ctrlPresentation.Barcode = strBarcode;
						m_ctrlPresentation.PageNumber = 0;
						m_ctrlPresentation.LineNumber = 0;

						//	Has a start PL been specified?
						if((tmaxParameter = Args.GetParameter(TmaxCommandParameters.StartPL)) != null)
						{
							if((lStartPL = tmaxParameter.AsLong()) > 0)
							{
								m_ctrlPresentation.PageNumber = (int)(CTmaxToolbox.PLToPage(lStartPL));
								m_ctrlPresentation.LineNumber = (short)(CTmaxToolbox.PLToLine(lStartPL));
							}

						}// if((tmaxParameter = Args.GetParameter(TmaxCommandParameters.StartPL)) != null)					
						
						m_ctrlPresentation.Open();
					}
				}
								
			}// if((Args.Items != null) && (Args.Items.Count > 0))
			
			//	Mark the item as processed
			Args.Items[0].State = TmaxItemStates.Processed;
			
			//	Acknowledge the request
			Args.Successful = true;
			
			//	Make sure the Active property for the panes is properly set
			SetPaneStates();
			
			//	Give focus back to the sender
			try
			{
				if(ReferenceEquals(objSender, m_paneMedia) == true)
					m_paneMedia.Select();
			}
			catch
			{
			}

		}// OnCmdOpen(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == Paste</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdPaste(object objSender, CTmaxCommandArgs Args)
		{
		}// OnCmdPaste(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == RefreshCodes</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdRefreshCodes(object objSender, CTmaxCommandArgs Args)
		{
			Debug.Assert(Args != null);
			Debug.Assert(m_tmaxDatabase != null);
			
			//	Just in case...
			if((Args == null) || (m_tmaxDatabase == null)) return;
			
			//	Ask the database to refresh the collection
			if((Args.Successful = m_tmaxDatabase.RefreshCaseCodes()) == true)
			{
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					try 
					{ 
						if(m_aPanes[i] != null) 
							m_aPanes[i].OnRefreshCodes(); 
					}
					catch {}
				}
			
			}// if((Args.Successful = m_tmaxDatabase.RefreshCodes()) == true)
			
		}// private void OnCmdRefreshCodes(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle RegisterSource command events</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdRegisterSource(object objSender, CTmaxCommandArgs Args)
		{
			Debug.Assert(Args.Source == (int)TmaxAppPanes.Source);
			
			if((Args.Items == null) || (Args.Items.Count == 0)) return;
			
			//	Register the top-level (parent) item
			if(m_tmaxDatabase.Register(Args.Items[0].SourceFolder, Args.Parameters) == true)
			{
				//	Notify each pane
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					if(m_aPanes[i] != null)
					{
						m_aPanes[i].OnRegistered(Args.Items[0].SourceFolder, m_eDropPane);
					}
					
				}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				
			}
			
			//	Reset the drop target
			m_eDropPane = TmaxAppPanes.Media;
			
		}// OnCmdRegisterSource(CPaneEventArgs Args)
			
		/// <summary>This method is called to handle pane events where CommandId == Reorder</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdReorder(object objSender, CTmaxCommandArgs Args)
		{
			Debug.Assert(Args != null);
			Debug.Assert(Args.Items != null);
			Debug.Assert(Args.Items.Count == 1);
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);
			
			//	Just in case...
			if((Args == null) || (m_tmaxDatabase == null) || (m_tmaxDatabase.Primaries == null)) return;
			
			//	Process the request
			Args.Successful = m_tmaxDatabase.Reorder(Args.Items[0]);
			Args.Items[0].State = TmaxItemStates.Processed;
			
			//	Notify each pane
			//
			//	NOTE:	We do this even if the database fails so that the pane
			//			that fired the event can put their objects back in order
			for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
			{
				if(m_aPanes[i] != null)
				{
					m_aPanes[i].OnReordered(Args.Items[0]);
				}
					
			}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)

			foreach(ITmaxAppNotification I in m_aIAppNotifications)
			{
				try { I.OnReordered(Args.Items[0]); }
				catch {};
					
			}// foreach(ITmaxAppNotification I in m_aIAppNotifications)
		
		}// OnCmdReorder(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle StartDrag command events</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdStartDrag(object objSender, CTmaxCommandArgs Args)
		{
			CTmaxParameter	tmaxParameter;
			RegSourceTypes	regType = RegSourceTypes.AllFiles;
			
			//	Is the user dragging new source files?
			if((TmaxAppPanes)Args.Source == TmaxAppPanes.Source)
			{
				//	Set the physical tree as the default drop target
				m_eDropPane = TmaxAppPanes.Media;
						
				//	Get the source type parameter
				if((tmaxParameter = Args.GetParameter(TmaxCommandParameters.RegSourceType)) != null)
				{
					regType = (RegSourceTypes)tmaxParameter.iValue;
				}
				
			}
			
			//	Notify the child panes
			for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
			{
				if(m_aPanes[i] != null)
				{
					try
					{
						if((TmaxAppPanes)Args.Source == TmaxAppPanes.Source)
						{
							m_aPanes[i].OnStartSourceDrag(regType);
						}
						else
						{
							//	Dragging media must define a parent for the items being dragged
							Debug.Assert(Args.Items != null);
							Debug.Assert(Args.Items.Count == 1);
							
							if((Args.Items != null) && (Args.Items.Count == 1))
							{
								m_aPanes[i].OnStartDragRecords(Args.Items[0]);
							}
							
						}
						
					}
					catch
					{
					
					}
				
				}
					
			}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)

		}// OnCmdStartDrag(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == Synchronize</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdSynchronize(object objSender, CTmaxCommandArgs Args)
		{
			CTmaxItems	tmaxModified = new CTmaxItems();
			
			Debug.Assert(Args != null);
			Debug.Assert(Args.Items != null);
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);
			
			//	Just in case...
			if((Args == null) || (m_tmaxDatabase == null) || (m_tmaxDatabase.Primaries == null)) return;
			
			//	Initialize the return
			Args.Successful = true;
			
			Cursor.Current = Cursors.WaitCursor;
			
			//	Perform the synchronization
			m_tmaxDatabase.Synchronize(Args.Items, tmaxModified);
			
			//	Notify the panes
			foreach(CTmaxItem O in tmaxModified)
			{
				//	Notify each pane
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					if(m_aPanes[i] != null)
					{
						m_aPanes[i].OnUpdated(O);
					}
				
				}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
			}
			
			Cursor.Current = Cursors.Default;
			
		}// private void OnCmdSynchronize(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == Update</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdUpdate(object objSender, CTmaxCommandArgs Args)
		{
			bool					bObjections = false;
			CTmaxDatabaseResults	tmaxResults = new CTmaxDatabaseResults();
			
			Debug.Assert(Args != null);
			Debug.Assert(Args.Items != null);
			Debug.Assert(Args.Items.Count == 1);
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);
		
			//	Just in case...
			if((Args == null) || (m_tmaxDatabase == null) || (m_tmaxDatabase.Primaries == null)) return;
			
			//	Are we updating objections
			bObjections = IsObjectionsCommand(Args.Parameters);
			
			//	Process the request
			Args.Successful = m_tmaxDatabase.Update(Args.Items[0], Args.Parameters, tmaxResults);
			Args.Items[0].State = TmaxItemStates.Processed;

			//	Notify the child panes
			if(Args.Successful == true)
				Notify(tmaxResults);
				
		}// OnCmdUpdate(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to handle pane events where CommandId == Help</summary>
		/// <param name="objSender">The object firing the event</param>
		/// <param name="Args">TrialMax command arguments</param>
		private void OnCmdHelp(object objSender, CTmaxCommandArgs Args)
		{
			Debug.Assert(Args != null);
			if(Args == null) return;
			
			//	Mark the request as processed
			Args.Successful = true;
			
			//	Which help request?
			if((Args.Parameters != null) && (Args.Parameters.Count > 0))
			{
				if(Args.Parameters.Find(TmaxCommandParameters.HelpContact) != null)
				{
					OnAppContactFTI();
				}
				else if(Args.Parameters.Find(TmaxCommandParameters.HelpAbout) != null)
				{
					OnAppAboutBox();
				}
				else
				{
					OnAppUsersManual();
				}
				
			}
			else
			{
				//	User's manual by default
				OnAppUsersManual();
			}
			
		}// private void OnCmdHelp(object objSender, CTmaxCommandArgs Args)
		
		/// <summary>This method is called to open the About box</summary>
		private void OnAppAboutBox()
		{
			string strFileSpec = "";
			
			//	Construct the path to the versions history file
			strFileSpec = m_strAppFolder;
			if((strFileSpec.Length > 0) && (strFileSpec.EndsWith("\\") == false))
				strFileSpec += "\\";
			strFileSpec += DEFAULT_VERSIONS_HISTORY_FILE;

			CAboutForm wndAbout = new CAboutForm();
			wndAbout.VersionsFileSpec = strFileSpec;
			
			wndAbout.ShowDialog();

		}// private void OnAppAboutBox()
		
		/// <summary>This method is called when the user wants to activate the installation</summary>
		private void OnAppActivateProduct()
		{
			FTI.Trialmax.Forms.CFActivateProduct activate = null;
            
			string	strMsg = "";
			
			//	Has the product already been activated?
            if (m_tmaxProductManager.Activated == true && m_tmaxProductManager.IsActivationExpired())
			{
				MessageBox.Show("TrialMax has already been activated.", "Activate", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			
			try
			{   
				activate = new FTI.Trialmax.Forms.CFActivateProduct();
				
				//	Set the form's properties
				activate.Registry = m_tmaxRegistry;
				activate.ManagerOptions = m_tmaxAppOptions;
				activate.Product = m_tmaxProductManager;
                //	Show the form to the user
				if(activate.ShowDialog() == DialogResult.OK)
				{
					//	Has the product been activated?
                    if (m_tmaxProductManager.Activated == true)
                    {
                        //	Prompt for jump to updates
                        strMsg = "Congratulations, TrialMax has been activated !\n\n";
                        strMsg += "It is recommended that you check for updates to ensure your product is up to date.";
                        strMsg += "Would you like to check for on-line updates now?";

                        if (MessageBox.Show(strMsg, "Update?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            OnAppCheckForUpdates();
                        }

                    }
                }
               
				
			}
			catch(System.Exception Ex)
			{
				MessageBox.Show(Ex.ToString());
			}
			
		}// private void OnAppActivateProduct()

        private void OnProductExpiry(bool isExpired)
        {
            FTI.Trialmax.Forms.CFActivateProduct activate = null;
            string strMsg = "";

            //	Has the product already been activated?
            if (m_tmaxProductManager.Activated == true && m_tmaxProductManager.IsActivationExpired())
            {
                MessageBox.Show("TrialMax has already been activated.", "Activate", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                activate = new FTI.Trialmax.Forms.CFActivateProduct();

                //	Set the form's properties
                activate.Registry = m_tmaxRegistry;
                activate.ManagerOptions = m_tmaxAppOptions;
                activate.Product = m_tmaxProductManager;
                activate.Controls["m_ctrlCancel"].Enabled = false;
                activate.Controls["lblExpirationMessage"].Visible = isExpired;
                
                //	Show the form to the user
                if (activate.ShowDialog() == DialogResult.OK)
                {
                    //	Has the product been activated?
                    if (m_tmaxProductManager.Activated == true)
                    {
                        //	Prompt for jump to updates
                        strMsg = "Congratulations, TrialMax has been activated !\n\n";
                        strMsg += "It is recommended that you check for updates to ensure your product is up to date.";
                        strMsg += "Would you like to check for on-line updates now?";

                        if (MessageBox.Show(strMsg, "Update?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            OnAppCheckForUpdates();
                        }

                    }



                }
                else
                {
                    this.Close();
                }

            }
            catch (System.Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }

        }// private void OnProductExpiry()

		/// <summary>This method is called when the user wants to go to the TrialMax online site</summary>
		private void OnAppOnlineSite()
		{
			try
			{
				if(m_tmaxProductManager.OnlineSite.Length > 0)
				{
					System.Diagnostics.Process.Start(m_tmaxProductManager.OnlineSite);
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnAppOnlineSite", Ex);
			}
			
		}// private void OnAppOnlineSite()
		
		/// <summary>This method is called when the user wants to edit the drive/server aliases</summary>
		private void OnAppCaseOptions()
		{
			bool bApplied = false;
			
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);
			
			if(m_tmaxDatabase == null) return;
			if(m_tmaxDatabase.Primaries == null) return;
			
			//	Notify each of the panes that the user is about to set the options
			for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
			{
				if(m_aPanes[i] != null)
				{
					try { m_aPanes[i].OnBeforeSetCaseOptions(); }
					catch {}
				}
				
			}

			//	Ask the database to invoke the editor
			bApplied = m_tmaxDatabase.SetCaseOptions();
	
			//	Notify each of the panes that the user is about to set the options
			for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
			{
				if(m_aPanes[i] != null)
				{
					try { m_aPanes[i].OnAfterSetCaseOptions(bApplied == false); }
					catch {}
				}
				
			}
			
			//	Make sure the correct text is displayed in the title bar
			if(bApplied == true)
				SetControlStates();

		}// private void OnAppCaseOptions()
		
		/// <summary>This method is called when the user wants to check for application updates</summary>
		private void OnAppCheckForUpdates()
		{
			FTI.Trialmax.Forms.CFUpdateWizard update = null;
			
			//	Clear the product update path
			m_strXmlUpdateFileSpec = "";
			
			//	Make sure the installer application exists
			if(System.IO.File.Exists(m_strUpdateInstaller) == false)
			{
				MessageBox.Show("Unable to locate " + m_strUpdateInstaller + " to perform on-line updates", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			
			try
			{
				update = new FTI.Trialmax.Forms.CFUpdateWizard();
				
				//	Connect the event source
				m_tmaxEventSource.Attach(update.EventSource);
				
				//	Set the form's properties
				update.Registry  = m_tmaxRegistry;
				update.AppFolder = m_tmaxAppOptions.ApplicationFolder;
				update.Product   = m_tmaxProductManager;
				
				DisableTmaxKeyboard(true);
				
				//	Show the form to the user
				if(update.ShowDialog() == DialogResult.OK)
				{
					//	There should be a valid installation file
					if((update.InstallFileSpec == null) || (update.InstallFileSpec.Length == 0))
					{
						MessageBox.Show("Unable to launch the installer. No product update has been specified", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
					else if(System.IO.File.Exists(update.InstallFileSpec) == false)
					{
						MessageBox.Show("Unable to locate " + update.InstallFileSpec + " to initialize the installer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
					else
					{
						//	Launch the installer
						m_strXmlUpdateFileSpec = update.InstallFileSpec;
						this.Close();
					}
						
				}// if(update.ShowDialog() == DialogResult.OK)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnAppCheckForUpdates", m_tmaxErrorBuilder.Message(ERROR_ON_MM_CHECK_FOR_UPDATES_EX), Ex);
			}
			finally
			{
				DisableTmaxKeyboard(false);
			}
			
		}// private void OnAppCheckForUpdates()
		
		/// <summary>This method is called when the user closes the current case</summary>
		/// <param name="bVerify">true to verify that it is OK to close</param>
		/// <returns>True if the database is closed</returns>
		private bool OnAppCloseCase(bool bVerify)
		{
			if(m_tmaxDatabase != null)
			{
				//	First verify that it is OK to close the database
				if(bVerify == true)
				{
					for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
					{
						if(m_aPanes[i] != null)
						{
							if(m_aPanes[i].CanCloseDatabase() == false)
								return false;
						}
					
					}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
					
				}//if(bVerify == true)
				
				//	Stop the simulator if active
				if((m_tmaxSimulator != null) && (m_tmaxSimulator.Database != null))
					m_tmaxSimulator.Database = null;
				
				//	Flush the clipboard
				if(m_tmaxClipboard != null)
					m_tmaxClipboard.Clear();
						
				//	Notify each of the panes
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					if(m_aPanes[i] != null)
					{
						try
						{
							m_aPanes[i].OnClipboardUpdated();
							m_aPanes[i].CaseOptions = null;
							m_aPanes[i].StationOptions = null;
							m_aPanes[i].CaseCodes = null;
							m_aPanes[i].Filtered = null;

							if(m_aPanes[i].Database != null)
								m_aPanes[i].Database = null;
						}
						catch
						{
						}
						
					}
					
				}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
					
				//	Notify the report manager
				if(m_tmaxReportManager != null)
				{
					try
					{
						m_tmaxReportManager.Database = null;
					}
					catch
					{
					}
				
				}
					
				try
				{
					//	Close the database
					m_tmaxDatabase.Close();
				}
				catch
				{
				}

			}// if(m_tmaxDatabase != null)
		
			//	Update the database dependent controls
			SetControlStates();
			SetDatabaseVersion();
			
			return true;
			
		}// private bool OnAppCloseCase()
		
		/// <summary>This method is called when the user clicks on Simulator</summary>
		private void OnAppSimulator()
		{
			try
			{
				//	Do we need to create the simulator?
				if(m_tmaxSimulator == null)
				{
					m_tmaxSimulator = new CTmaxManagerSimulator();
					
					//	Initialize the simulator
					m_tmaxSimulator.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.OnDiagnostic);
					m_tmaxSimulator.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);
					m_tmaxSimulator.Command += new FTI.Shared.Trialmax.TmaxCommandHandler(this.OnTmaxCommand);
					m_tmaxSimulator.Initialize(m_xmlIni);
					
				}// if(m_tmaxSimulator == null)
				
				//	Do we need to set the database?
				if(m_tmaxSimulator.Database == null)
					m_tmaxSimulator.Database = m_tmaxDatabase;
					
				//	Is the simulator active?
				if(m_tmaxSimulator.Active == true)
					m_tmaxSimulator.Stop();
				else
					m_tmaxSimulator.Start();
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnAppSimulator", "An exception occurred while attempting to activate/deactivate the simulator", Ex);
			}
			
		}// private void OnAppSimulator()
		
		/// <summary>This method handles all internal application command events</summary>
		/// <param name="eCommand">The application command identifier</param>
		private void OnAppCommand(AppCommands eCommand)
		{
			//	Call the handler for the specified command
			switch(eCommand)
			{
				case AppCommands.Exit: 
					
					OnAppExit();
					break;

				case AppCommands.NewCase:

					OnAppOpenCase("", true, false);
					break;

				case AppCommands.OpenCase:
					
					OnAppOpenCase("", false, false);
					break;

				case AppCommands.CloseCase:

					OnAppCloseCase(true);
					break;

				case AppCommands.Recent1:
				case AppCommands.Recent2:
				case AppCommands.Recent3:
				case AppCommands.Recent4:
				case AppCommands.Recent5:
					
					OnAppOpenRecent(eCommand);
					break;

				case AppCommands.LoadLayout:
					
					OnAppLoadLayout();
					break;

				case AppCommands.SaveLayout:
					
					OnAppSaveLayout();
					break;

				case AppCommands.ManagerOptions:
				
					OnAppManagerOptions();
					break;
					
				case AppCommands.CaseOptions:
				
					OnAppCaseOptions();
					break;
					
				case AppCommands.TrimDatabase:

					OnAppTrimDatabase();
					break;

				case AppCommands.CompactDatabase:

					OnAppCompactDatabase();
					break;

				case AppCommands.PresentationOptions:
				
					OnAppPresentationOptions();
					break;
					
				case AppCommands.PresentationToolbars:
				
					OnAppPresentationToolbars();
					break;
					
				case AppCommands.ToggleSourceExplorer: 
					
					OnAppTogglePane(TmaxAppPanes.Source);
					break;

				case AppCommands.ToggleMediaTree:    
					
					OnAppTogglePane(TmaxAppPanes.Media);
					break;

				case AppCommands.ToggleBinders:    
					
					OnAppTogglePane(TmaxAppPanes.Binders);
					break;

				case AppCommands.ToggleMediaViewer:    
					
					OnAppTogglePane(TmaxAppPanes.Viewer);
					break;

				case AppCommands.ToggleProperties:   
					
					OnAppTogglePane(TmaxAppPanes.Properties);
					break;

				case AppCommands.ToggleScriptBuilder:   
					
					OnAppTogglePane(TmaxAppPanes.Scripts);
					break;

				case AppCommands.ToggleTranscripts: 
					
					OnAppTogglePane(TmaxAppPanes.Transcripts);
					break;

				case AppCommands.ToggleTuner:  
					
					OnAppTogglePane(TmaxAppPanes.Tuner);
					break;

				case AppCommands.ToggleCodes:  
					
					OnAppTogglePane(TmaxAppPanes.Codes);
					break;

				case AppCommands.ToggleObjections:

					OnAppTogglePane(TmaxAppPanes.Objections);
					break;

				case AppCommands.ToggleObjectionProperties:

					OnAppTogglePane(TmaxAppPanes.ObjectionProperties);
					break;

				case AppCommands.ToggleScriptReview:

					OnAppTogglePane(TmaxAppPanes.ScriptReview);
					break;

				case AppCommands.ToggleSearchResults: 
					
					OnAppTogglePane(TmaxAppPanes.Results);
					break;

				case AppCommands.ToggleFilteredTree: 
					
					OnAppTogglePane(TmaxAppPanes.FilteredTree);
					break;

				case AppCommands.ToggleErrorMessages:    
					
					OnAppTogglePane(TmaxAppPanes.Errors);
					break;

				case AppCommands.ToggleVersions:    
					
					OnAppTogglePane(TmaxAppPanes.Versions);
					break;

				case AppCommands.ToggleDiagnostics: 
					
					OnAppTogglePane(TmaxAppPanes.Diagnostics);
					break;

				case AppCommands.ShowAll: 
					
					OnAppShowAll();
					break;

				case AppCommands.ReloadCase:
					
					OnAppReloadCase();
					break;

				case AppCommands.LockPanes:
					
					OnAppLockPanes();
					break;

				case AppCommands.RefreshTreatments:
				
					OnAppRefreshTreatments();
					break;
					
				case AppCommands.ValidateDatabase:
				
					OnAppValidateDatabase();
					break;
					
				case AppCommands.OpenPresentation:

					OnAppOpenPresentation();
					break;

				case AppCommands.HelpContents:
					
					OnAppTogglePane(TmaxAppPanes.Help);
					break;

				case AppCommands.ContactFTI:
					
					OnAppContactFTI();
					break;

				case AppCommands.UsersManual:
					
					OnAppUsersManual();
					break;

				case AppCommands.OnlineSite:
					
					OnAppOnlineSite();
					break;

				case AppCommands.AboutBox:   
					
					OnAppAboutBox();
					break;

				case AppCommands.Debug1:
				case AppCommands.Debug2:
				
					OnAppDebug(eCommand);
					break;
					
				case AppCommands.ActivateProduct:   
					
					OnAppActivateProduct();
					break;

				case AppCommands.CheckForUpdates:   
					
					OnAppCheckForUpdates();
					break;

				case AppCommands.Print:
					
					OnAppPrint();
					break;
					
				case AppCommands.Find:
				
					OnAppFind();
					break;

				case AppCommands.FindNext:
				
					//	Make it look like the hotkey was pressed
					if(m_paneResults != null)
						m_paneResults.OnHotkey(TmaxHotkeys.FindNext);
					break;

				case AppCommands.ImportAsciiBinder:
				case AppCommands.ImportXmlBinder:
					
					OnAppImportBinders(eCommand);
					break;
					
				case AppCommands.ImportAsciiScript:
				case AppCommands.ImportXmlScript:
					
					OnAppImportScripts(eCommand);
					break;
					
				case AppCommands.ImportBarcodeMap:
					
					OnAppImportBarcodeMap();
					break;
					
				case AppCommands.ImportCodes:
				case AppCommands.ImportCodesDatabase:
					
					OnAppImportCodes(eCommand);
					break;

				case AppCommands.ImportAsciiObjections:

					OnAppImportObjections(eCommand);
					break;

				case AppCommands.ImportLoadFile:
					
					OnAppImportLoadFile();
					break;
					
				case AppCommands.ImportXmlCaseCodes:
					
					OnAppImportXmlCaseCodes();
					break;
					
				case AppCommands.ExportXmlCaseCodes:
					
					OnAppExportXmlCaseCodes();
					break;
					
				case AppCommands.ExportBarcodeMap:
					
					OnAppExportBarcodeMap();
					break;

				case AppCommands.ExportAsciiObjections:

					OnAppExportObjections(eCommand);
					break;

				case AppCommands.SetFilter:
				case AppCommands.FastFilter:
					
					OnAppSetFilter(eCommand);
					break;
					
				case AppCommands.BulkUpdate:
					
					OnAppBulkUpdate();
					break;
					
				case AppCommands.ScreenCapture:
					
					OnAppScreenCapture();
					break;
					
				default:
				
					break;
			}
			
		}// private void OnAppCommand(AppCommands eCommand)
					
		/// <summary>This method is called when the user clicks on Contact FTI fro the Help menu</summary>
		private void OnAppContactFTI()
		{
			FTI.Trialmax.Forms.CFRichLabel	contactFTI = null;
			System.IO.Stream				ioStream = null;
			string							strFileSpec = "";
			string							strStream = "";
			string							strMsg = "";

			try
			{
				contactFTI = new FTI.Trialmax.Forms.CFRichLabel();
				
				//	Connect to the wizard events
				contactFTI.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.OnDiagnostic);
				contactFTI.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);
	
				//	Set the default property values
				contactFTI.Text = " Contact Information";
				contactFTI.Width = 650;
				contactFTI.Height = 525;
				contactFTI.StartPosition = FormStartPosition.CenterScreen;
				
				//	Construct the path to the external file containing the contact information
				strFileSpec = m_strAppFolder;
				if((strFileSpec.Length > 0) && (strFileSpec.EndsWith("\\") == false))
					strFileSpec += "\\";
				strFileSpec += DEFAULT_CONTACT_FTI_FILENAME;
				
				//	Does the external file exist?
				if(System.IO.File.Exists(strFileSpec) == true)
				{
					//	Make the form use the external file
					contactFTI.FileSpec = strFileSpec;
				}
				else
				{
					//	Build the path to the resource stream
					strStream = ("FTI.Trialmax.TmaxManager." + DEFAULT_CONTACT_FTI_FILENAME);
					
					//	Get the stream from the resource descriptor
					ioStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(strStream);
					
					//	Were we able to get the contact information?
					if(ioStream != null)
					{
						contactFTI.IOStream = ioStream;
					}
					else
					{
						//	Display an error message
						strMsg = "Unable to locate the contact information in " + strFileSpec;
						MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
				
				}// if(System.IO.File.Exists(strFileSpec) == true)
				
				//	Show the form to the user
				if((contactFTI.IOStream != null) || (contactFTI.FileSpec.Length > 0))
					contactFTI.ShowDialog();
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnAppContactFTI", m_tmaxErrorBuilder.Message(ERROR_CONTACT_FTI_EX), Ex);
			}
			
		}// private void OnAppContactFTI()
		
		/// <summary>This method is called when the user clicks on a selection in the debug submenu</summary>
		private void OnAppDebug(AppCommands eCommand)
		{
			switch(eCommand)
			{
				case AppCommands.Debug1:
				
				
					FTI.Trialmax.Controls.CFTestEditCtrl testEditor = new CFTestEditCtrl();
					testEditor.ShowDialog();
					break;
					
				case AppCommands.Debug2:
				
					break;
					
			}
			
		}// private void OnAppDebug(AppCommands eCommand)
		
		/// <summary>This method is called when the user wants to exit the application</summary>
		private void OnAppExit()
		{
			//	Close down and clean up
			Terminate();
			
			//	Kill the application
			Application.Exit();
			
		}// private void OnAppExit()
		
		/// <summary>This method is called when the user wants to export the barcode map table</summary>
		private void OnAppExportBarcodeMap()
		{
			if(m_tmaxDatabase == null) return;
			if(m_tmaxDatabase.Primaries == null) return;
			
			FireExportCommand(AppCommands.ExportBarcodeMap);

		}// private void OnAppExportBarcodeMap()
		
		/// <summary>This method is called when the user wants to export the active case codes</summary>
		private void OnAppExportXmlCaseCodes()
		{
			if(m_tmaxDatabase == null) return;
			if(m_tmaxDatabase.Primaries == null) return;
			
			FireExportCommand(AppCommands.ExportXmlCaseCodes);

		}// private void OnAppExportXmlCaseCodes()
		
		/// <summary>This method is called when the user wants to import a barcode map</summary>
		private void OnAppImportBarcodeMap()
		{
			if(m_tmaxDatabase == null) return;
			if(m_tmaxDatabase.Primaries == null) return;
			
			try
			{
				DisableTmaxKeyboard(true);
				
				if(m_tmaxDatabase.Import(TmaxImportFormats.BarcodeMap) == true)
				{
					//	Are we using foreign barcodes?
					if(m_tmaxAppOptions.ShowForeignBarcodes == true)
					{
						//	Notify each pane to refresh its text
						for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
						{
							if(m_aPanes[i] != null)
							{
								m_aPanes[i].RefreshText();
							}
								
						}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
					
					}// if(m_tmaxDatabase.CaseOptions.UseBarcodeMap == true)
					
				}// if(m_tmaxDatabase.Import(null, null, tmaxParameters) == true)
		
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnAppImportBarcodeMap", Ex);
			}
			finally
			{
				DisableTmaxKeyboard(false);
			}
			
		}// private void OnAppImportBarcodeMap()
		
		/// <summary>This method is called when the user wants to import metadata</summary>
		/// <param name="eCommand">The application command identifier</param>
		private void OnAppImportCodes(AppCommands eCommand)
		{
			CTmaxDatabaseResults tmaxResults = new CTmaxDatabaseResults();
			TmaxImportFormats eFormat = TmaxImportFormats.Codes;
			
			if(m_tmaxDatabase == null) return;
			if(m_tmaxDatabase.Primaries == null) return;
			if(m_tmaxDatabase.Primaries.Count == 0) return;

			if(eCommand == AppCommands.ImportCodesDatabase)
				eFormat = TmaxImportFormats.CodesDatabase;
			else
				eFormat = TmaxImportFormats.Codes;
				
			try
			{
				DisableTmaxKeyboard(true);
				
				if(m_tmaxDatabase.Import(eFormat, tmaxResults) == true)
				{
					//	Notify each pane
					Notify(tmaxResults);
					
				}// if(m_tmaxDatabase.Import(TmaxImportFormats.Codes) == true)
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnAppImportCodes", Ex);
			}
			finally
			{
				DisableTmaxKeyboard(false);
			}
		}// private void OnAppImportCodes(AppCommand eCommand)

		/// <summary>This method is called when the user wants to import objections</summary>
		/// <param name="eCommand">The application command identifier</param>
		private void OnAppImportObjections(AppCommands eCommand)
		{
			if(m_tmaxDatabase == null) return;
			if(m_tmaxDatabase.Objections == null) return;

			FireImportCommand(eCommand);
			
		}// private void OnAppImportObjections(AppCommands eCommand)

		/// <summary>This method is called when the user wants to export objections</summary>
		/// <param name="eCommand">The application command identifier</param>
		private void OnAppExportObjections(AppCommands eCommand)
		{
			if(GetCommandEnabled(eCommand) == true)
				FireExportCommand(eCommand);

		}// private void OnAppExportObjections(AppCommands eCommand)

		/// <summary>This method is called when the user wants to import binders</summary>
		private void OnAppImportBinders(AppCommands eCommand)
		{
			if(m_tmaxDatabase == null) return;
			if(m_tmaxDatabase.Primaries == null) return;
			
			FireImportCommand(eCommand);
			
		}// private void OnAppImportBinders()
		
		/// <summary>This method is called when the user wants to import case codes</summary>
		private void OnAppImportXmlCaseCodes()
		{
			if(m_tmaxDatabase == null) return;
			if(m_tmaxDatabase.Primaries == null) return;
			
			FireImportCommand(AppCommands.ImportXmlCaseCodes);
			
		}// private void OnAppImportXmlCaseCodes()
		
		/// <summary>This method is called when the user wants to import scripts</summary>
		/// <param name="eCommand">The import command requested by the user</param>
		private void OnAppImportScripts(AppCommands eCommand)
		{
			if(m_tmaxDatabase == null) return;
			if(m_tmaxDatabase.Primaries == null) return;
			
			FireImportCommand(eCommand);
			
		}// private void OnAppImportScripts(AppCommands eCommand)
		
		/// <summary>This method is called when the user wants to load a pane layout file</summary>
		private void OnAppLoadLayout()
		{
			try
			{
				OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
				
				//	Initialize the file selection dialog
				dlg.CheckFileExists = true;
				dlg.CheckPathExists = true;
				dlg.Multiselect = false;
				dlg.Title = "Open File";
				dlg.Filter = "Custom Layouts (*.tpl)|*.tpl|All Layouts (*.tpl,*.tpd)|*.tpl;*.tpd|All Files (*.*)|*.*";
				dlg.InitialDirectory = m_strAppFolder;
				dlg.FilterIndex = 2;
				
				DisableTmaxKeyboard(true);
				
				//	Open the dialog box
				if(dlg.ShowDialog() == DialogResult.OK) 
				{
					if(LoadPaneLayout(dlg.FileName) == true)
					{
						SetPaneStates();
						
						if(m_tmaxAppOptions != null)
							m_tmaxAppOptions.LastLayout = dlg.FileName;
					}
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnAppLoadLayout", Ex);
			}
			finally
			{
				DisableTmaxKeyboard(false);
			}
		
		}// private void OnAppLoadLayout()
		
		/// <summary>This method is called when the user clicks on the Lock Panes option in the menu</summary>
		private void OnAppLockPanes()
		{
			StateButtonTool Tool = null;
			
			try
			{
				if((Tool = (StateButtonTool)GetUltraTool(AppCommands.LockPanes)) != null)
				{
					SetPaneLocks(Tool.Checked == false);
				}
			
			}
			catch
			{
			}
			
		}// private void OnAppLockPanes()
		
		/// <summary>This method is called when the user wants to set the Manager Options</summary>
		private void OnAppManagerOptions()
		{
			bool			 bShowForeignBarcodes = false;
			bool			 bCancelled = false;
			CFManagerOptions managerOptions = null;	
			
			try
			{
				//	Notify each of the panes that the user is about
				//	to set the options
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					if(m_aPanes[i] != null)
						m_aPanes[i].OnBeforeSetApplicationOptions();
				}

				//	Save the current setting 
				bShowForeignBarcodes = m_tmaxAppOptions.ShowForeignBarcodes;
			
				managerOptions = new CFManagerOptions();
				
				managerOptions.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);
				managerOptions.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.OnDiagnostic);
				managerOptions.ManagerOptions = m_tmaxAppOptions;
				managerOptions.SourceTypes = m_tmaxSourceTypes;
				managerOptions.ProductManager = m_tmaxProductManager;
				managerOptions.WMEncoder = m_mediaEncoder;
				
				DisableTmaxKeyboard(true);
				
				bCancelled = (managerOptions.ShowDialog(this) == DialogResult.Cancel);

				//	Notify the panes when complete
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					if(m_aPanes[i] != null)
					{
						m_aPanes[i].OnAfterSetApplicationOptions(bCancelled);
						
						if(bShowForeignBarcodes != m_tmaxAppOptions.ShowForeignBarcodes)
							m_aPanes[i].RefreshText();
					}
				
				}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)

				managerOptions.Dispose();
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnAppManagerOptions", m_tmaxErrorBuilder.Message(ERROR_ON_MANAGER_OPTIONS_EX), Ex);
			}
			finally
			{
				DisableTmaxKeyboard(false);
			}
			
		}// private void OnAppManagerOptions()
		
		/// <summary>This method is called when the user wants to open a case</summary>
		/// <param name="strFolder">optional folder path to the case to be opened</param>
		/// <param name="bCreate">true if intending to create a new case</param>
		/// <param name="bReload">true if the active case is being reloaded</param>
		/// <returns>true if successful</returns>
		private bool OnAppOpenCase(string strFolder, bool bCreate, bool bReload)
		{
			bool	bSuccess = false;
			string	strMsg = "";
			
			Debug.Assert(strFolder != null);
			Debug.Assert(m_tmaxDatabase != null);
			
			if(strFolder == null) return false;
			if(m_tmaxDatabase == null) return false;

			//	Do we need to prompt the user for the folder?
			if(strFolder.Length == 0)
			{
				//	Initialize the folder using the last opened case
				if(m_tmaxAppOptions.RecentlyUsed.Count > 0)
					strFolder = m_tmaxAppOptions.RecentlyUsed[0].ToString();

				//	Prompt the user to select the folder
				if(GetCaseFolder(ref strFolder, ref bCreate) == false) 
					return false;
			
			}
			else
			{
				//	Check to see if the database exists
				if(m_tmaxDatabase.Exists(strFolder) == false)
				{
					//	Notify the user
					strMsg = String.Format("{0} does not contain a TrialMax database.", strFolder);
					MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					m_tmaxAppOptions.RemoveRecentlyUsed(strFolder);
					return false;
							
				}// if(m_tmaxDatabase.Exists(strFolder) == false)
					
			}
			
			try
			{
				//	Make sure the existing database is closed
				OnAppCloseCase(true);

				if(bCreate)
					bSuccess = m_tmaxDatabase.Create(strFolder, System.Environment.UserName);
				else
					bSuccess = m_tmaxDatabase.Open(strFolder, System.Environment.UserName);
			
				//	Notify the panes if successful
				if(bSuccess == true)
				{
					//	Set the pane properties
					for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
					{
						if(m_aPanes[i] != null)
						{
							m_aPanes[i].CaseOptions = m_tmaxDatabase.CaseOptions;
							m_aPanes[i].StationOptions = m_tmaxDatabase.StationOptions;
							m_aPanes[i].CaseCodes = m_tmaxDatabase.CaseCodes;
							m_aPanes[i].Database = m_tmaxDatabase;
							m_aPanes[i].Filtered = m_tmaxDatabase.Filtered;
						}
						
					}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
					
					m_tmaxAppOptions.AddRecentlyUsed(m_tmaxDatabase.Folder);
					
					//	Notify the report manager that there is now an active database
					if(m_tmaxReportManager != null)
						m_tmaxReportManager.Database = m_tmaxDatabase;
						
					//	Set the main title
					SetControlStates();
					SetDatabaseVersion();
					
					//	Make sure the media tree is visible
					if(bReload == false)
						SetUltraPaneVisible(TmaxAppPanes.Media, true, true);
				
				}// if(bSuccess == true)
				else
				{
					m_tmaxAppOptions.RemoveRecentlyUsed(strFolder);
				}
				
			}
			catch(System.Exception Ex)
			{
				MessageBox.Show(Ex.ToString(), "Exception");
			}
			finally
			{
				//	Make sure the keyboard hook is enabled
				DisableTmaxKeyboard(false);
			}
			
			return bSuccess;
		
		}// private bool OnAppOpenCase(string strFolder, bool bCreate)
		
		/// <summary>This method is called when the user wants to launch Presentation from the View menu</summary>
		private void OnAppOpenPresentation()
		{
			Debug.Assert(m_ctrlPresentation != null);
			Debug.Assert(m_ctrlPresentation.Initialized == true);
			
			if(m_ctrlPresentation == null) return;
			if(m_ctrlPresentation.Initialized == false) return;
			
			//	Open Presentation
			if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null))
				m_ctrlPresentation.CaseFolder = m_tmaxDatabase.Folder;
			else
				m_ctrlPresentation.CaseFolder = "";
			m_ctrlPresentation.Barcode = "";
			m_ctrlPresentation.Open();
			
		}// private void OnAppOpenPresentation()
		
		/// <summary>This method is called when the user selects one of the Recent cases from the application's File menu</summary>
		/// <param name="eCommand">The command identifier of the recent case selected by the user</param>
		private void OnAppOpenRecent(AppCommands eCommand)
		{
			ToolBase tool = null;
			
			//	Get the menu tool selected by the user
			if((tool = GetUltraTool(eCommand.ToString())) != null)
				OnAppOpenCase(tool.SharedProps.Caption, false, false);
		
		}// private void OnAppOpenRecent(AppCommands eCommand)
		
		/// <summary>This method is called when the user wants to set the Presentation Options</summary>
		private void OnAppPresentationOptions()
		{
			CFPresentationOptions	presentationOptions = null;	
			bool					bCancelled = false;		
			
			try
			{
				//	Notify each of the panes that the user is about
				//	to set the options
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					if(m_aPanes[i] != null)
						m_aPanes[i].OnBeforeSetPresentationOptions();
				}

				presentationOptions = new CFPresentationOptions();
				
				presentationOptions.FileSpec = m_strPresentationIniFileSpec;
				
				DisableTmaxKeyboard(true);
				
				bCancelled = (presentationOptions.ShowDialog(this) == DialogResult.Cancel);
			
				//	Refresh the options collection
				if((m_tmaxPresentationOptions != null) && (bCancelled == false))
					m_tmaxPresentationOptions.Load(m_strPresentationIniFileSpec);
				
				//	Notify the panes when complete
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					if(m_aPanes[i] != null)
						m_aPanes[i].OnAfterSetPresentationOptions(bCancelled);
				}

				presentationOptions.Dispose();
				this.Activate();
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnAppPresentationOptions", m_tmaxErrorBuilder.Message(ERROR_ON_PRESENTATION_OPTIONS_EX), Ex);
			}
			finally
			{
				DisableTmaxKeyboard(false);
			}
			
		}// private void OnAppPresentationOptions()
		
		/// <summary>This method is called when the user wants to set the Presentation Toolbars</summary>
		private void OnAppPresentationToolbars()
		{
			CFPresentationToolbars	presentationToolbars = null;	
			
			try
			{
				//	Allocate and initialize the form
				presentationToolbars = new CFPresentationToolbars();
				presentationToolbars.FileSpec = m_strPresentationIniFileSpec;
				
				DisableTmaxKeyboard(true);
				
				//	Show the form
				presentationToolbars.ShowDialog(this);
			
				//	Destroy the form
				presentationToolbars.Dispose();
				this.Activate();
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnAppPresentationToolbars", m_tmaxErrorBuilder.Message(ERROR_ON_PRESENTATION_TOOLBARS_EX), Ex);
			}
			finally
			{
				DisableTmaxKeyboard(false);
			}
			
		}// private void OnAppPresentationToolbars()
		
		/// <summary>This method is called to start a print job with the specified items</summary>
		private void OnAppPrint()
		{
			CBasePane	tmaxPane = GetActivePane();
			CTmaxItems	tmaxItems = null;
			
			//	See if the active pane wants to specify the print items
			if(tmaxPane != null)
				tmaxItems = tmaxPane.GetCmdPrintItems();
			
			//	Process the command here
			Print(tmaxItems);
			
		}// private void OnAppPrint()
				
		/// <summary>This method is called to start a screen capture session</summary>
		private void OnAppScreenCapture()
		{
			string	strMsg = "";
			
			try
			{
				//	Prompt the user for confirmation
				strMsg  = "To capture a selection to the Windows clipboard click on OK and press F11. ";
				strMsg += "Then select the region to be captured.";
					
				if(MessageBox.Show(strMsg, "Capture", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
				{
					m_ctrlScreenCapture.Capture();
				}

			}
			catch(System.Exception Ex)
			{
				try { m_ctrlScreenCapture.Stop(); } catch {};
				
				m_tmaxEventSource.FireDiagnostic(this, "OnAppScreenCapture", Ex);
			}
		
		}// private void OnAppScreenCapture()
		
		/// <summary>This method is called to search for transcript text</summary>
		private void OnAppFind()
		{
			CBasePane		tmaxPane = GetActivePane();
			CTmaxItems		tmaxItems = null;
			TmaxAppPanes	eSource = TmaxAppPanes.MaxPanes;
			
			//	See if the active pane wants to process the request
			if(tmaxPane != null)
			{
				tmaxItems = tmaxPane.GetCmdFindItems();
				eSource = (TmaxAppPanes)(tmaxPane.PaneId);
			}
				
			//	Type the media pane if no items specified
			if((tmaxItems == null) || (tmaxItems.Count == 0))
			{
				tmaxItems = m_paneMedia.GetCmdFindItems();
				eSource = TmaxAppPanes.Media;
			}
			
			//	Execute the operation
			if(Find(tmaxItems, eSource) == true)
				SetUltraPaneVisible(TmaxAppPanes.Results, true, false);
			
		}// private void OnAppFind()
				
		/// <summary>This method is called when the user wants to register treatments created with Presentation</summary>
		private void OnAppRefreshTreatments()
		{
			CTmaxDatabaseResults	tmaxResults = new CTmaxDatabaseResults();
			long					lTotal = 0;
			
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);
			
			if(m_tmaxDatabase == null) return;
			if(m_tmaxDatabase.Primaries == null) return;
			
			//	Get the database to create the new treatments
			m_tmaxDatabase.AddZaps(tmaxResults);
			
			//	Did we add any new treatments?
			if(tmaxResults.Added.Count > 0)
			{
				//	The return collection is a collection of parent records
				foreach(CTmaxItem tmaxParent in tmaxResults.Added)
				{
					lTotal += tmaxParent.SubItems.Count;
					
					//	Notify each pane
					for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
					{
						if(m_aPanes[i] != null)
						{
							m_aPanes[i].OnAdded(tmaxParent, tmaxParent.SubItems);
						}
						
					}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				}
				
				tmaxResults.Clear();
				
				MessageBox.Show(lTotal.ToString() + " treatments have been added.", "", 
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				MessageBox.Show("No pending treatments were located", "", 
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			
		}// private void OnAppRefreshTreatments()
		
		/// <summary>This method is called when the user wants to refresh the views by reloading the database</summary>
		private void OnAppReloadCase()
		{
			bool		bFilterOnOpen = false;
			CTmaxFilter	tmaxFilter = null;
			
			if(m_tmaxDatabase == null) return;
			if(m_tmaxDatabase.Primaries == null) return;
			
			//	Make a copy of the active filter
			if(m_tmaxDatabase.Filter != null)
			{
				tmaxFilter = new CTmaxFilter();
				tmaxFilter.Copy(m_tmaxDatabase.Filter);
			}

			//	Make sure there is no attempt load the filter until we're ready
			bFilterOnOpen = m_tmaxAppOptions.FilterOnOpen;
			m_tmaxAppOptions.FilterOnOpen = false;
			
			//	Reopen the existing case
			OnAppOpenCase(m_tmaxDatabase.Folder, false, true);
			
			m_tmaxAppOptions.FilterOnOpen = bFilterOnOpen;
			
			//	Reset the filter if there is one
			try
			{
				if(tmaxFilter != null)
				{
					m_tmaxDatabase.SetFiltered(tmaxFilter);

					for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
					{
						if(m_aPanes[i] != null)
						{
							m_aPanes[i].Filtered = m_tmaxDatabase.Filtered;
						}
				
					}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				}
			}
			catch
			{
			}

		}// private void OnAppReloadCase()
		
		/// <summary>This method is called when the user wants to save the current screen layout</summary>
		private void OnAppSaveLayout()
		{
			try
			{
				SaveFileDialog dlg = new SaveFileDialog();
				
				//	Initialize the file selection dialog
				dlg.AddExtension = true;
				dlg.DefaultExt = "xml";
				dlg.CheckPathExists = true;
				dlg.OverwritePrompt = true;
				dlg.Filter = "Custom Layouts (*.tpl)|*.tpl|All Layouts (*.tpl,*.tpd)|*.tpl;*.tpd|All Files (*.*)|*.*";
				dlg.InitialDirectory = m_strAppFolder;
				
				DisableTmaxKeyboard(true);
				
				//	Open the dialog box
				if(dlg.ShowDialog() == DialogResult.OK) 
				{
					if(SavePaneLayout(dlg.FileName) == true)
					{
						if(m_tmaxAppOptions != null)
							m_tmaxAppOptions.LastLayout = dlg.FileName;
					}
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnAppSaveLayout", Ex);
			}
			finally
			{
				DisableTmaxKeyboard(false);
			}
		
		}// private void OnAppSaveLayout()
		
		/// <summary>This method is called to set the filter for the filter tree</summary>
		/// <param name="eCommand">The command used to trigger the event</param>
		private void OnAppSetFilter(AppCommands eCommand)
		{
			if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null))
			{
				//	Notify the filter tree
				if(m_paneFiltered != null)
				{
					SetUltraPaneVisible(TmaxAppPanes.FilteredTree, true, true);
				}
				
				//	Make it look like the command was fired by a child pane
				FireFilterCommand(eCommand);	
			}			
			
		}// private void OnAppSetFilter(AppCommands eCommand)
		
		/// <summary>This method is called to perform a bulk update of fielded data records</summary>
		private void OnAppBulkUpdate()
		{
			CTmaxCommandArgs tmaxArgs = null;
			
			//	Make it look this the command was fired by a child pane
			if((tmaxArgs = new CTmaxCommandArgs(TmaxCommands.BulkUpdate, 0, new CTmaxItems(), new CTmaxParameters())) != null)
			{
				OnCmdBulkUpdate(this, tmaxArgs);
			}

		}// private void OnAppBulkUpdate(AppCommands eCommand)
		
		/// <summary>This function makes all primary child panes visible</summary>
		private void OnAppShowAll()
		{
			try
			{
				SetUltraPaneVisible(TmaxAppPanes.Media, true, false);
				SetUltraPaneVisible(TmaxAppPanes.Source, true, false);
				SetUltraPaneVisible(TmaxAppPanes.Binders, true, false);
				SetUltraPaneVisible(TmaxAppPanes.FilteredTree, true, false);
				SetUltraPaneVisible(TmaxAppPanes.Viewer, true, false);
				SetUltraPaneVisible(TmaxAppPanes.Properties, true, false);
				SetUltraPaneVisible(TmaxAppPanes.Codes, true, false);
				SetUltraPaneVisible(TmaxAppPanes.Scripts, true, false);
				SetUltraPaneVisible(TmaxAppPanes.Transcripts, true, false);
				SetUltraPaneVisible(TmaxAppPanes.Tuner, true, false);
				SetUltraPaneVisible(TmaxAppPanes.Results, true, false);
				SetUltraPaneVisible(TmaxAppPanes.Versions, true, false);
				SetUltraPaneVisible(TmaxAppPanes.Objections, true, false);
				SetUltraPaneVisible(TmaxAppPanes.ObjectionProperties, true, false);
				SetUltraPaneVisible(TmaxAppPanes.ScriptReview, true, false);
					
				SetPaneStates();
						
			}
			catch
			{
			}
			
		}// private void OnAppShowAll()
		
		/// <summary>This function toggles the visibility of docking pane that owns the specified pane control</summary>
		/// <param name="eTmaxPane">Enumerated Pane Identifier</param>
		private void OnAppTogglePane(TmaxAppPanes eTmaxPane)
		{
			DockablePaneBase Pane = null;
			
			try
			{
				//m_tmaxEventSource.FireDiagnostic(this, "OnAppTogglePane", "Toggle application pane: " + eTmaxPane.ToString());

				//	Get the specified pane object
				if((Pane = GetUltraPane(eTmaxPane)) != null)
				{
					if(Pane.Closed == false)
					{
						SetUltraPaneVisible(eTmaxPane, false, false);
					}
					else
					{
						SetUltraPaneVisible(eTmaxPane, true, true);
					}
					
					SetPaneStates();
				}
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnAppTogglePane", m_tmaxErrorBuilder.Message(ERROR_TOGGLE_PANE_EX, eTmaxPane), Ex);
			}
		
		}// private void OnAppTogglePane(TmaxAppPanes eTmaxPane)

		/// <summary>This method is called when the user wants to Trim the database contents</summary>
		private void OnAppTrimDatabase()
		{
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);

			if (m_tmaxDatabase == null) return;
			if (m_tmaxDatabase.Primaries == null) return;

			//	Ask the database to perform the operation
			try
			{
				if(GetTrimOptions() == true)
					m_tmaxDatabase.Trim(null);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnAppTrimDatabase", m_tmaxErrorBuilder.Message(ERROR_ON_APP_TRIM_DATABASE_EX), Ex);
			}

		}// private void OnAppTrimDatabase()

		/// <summary>This method is called when the user wants to Compact a database</summary>
		private void OnAppCompactDatabase()
		{
			CTmaxCaseDatabase	tmaxCompactor = null;
			string				strMsg = "";
			string				strCaseFolder = "";
			string				strFileSpec = "";
			
			try
			{
				//	Should we use the active database?
				if((m_tmaxDatabase != null) && (m_tmaxDatabase.FileSpec.Length > 0))
				{
					//	Confirm before closing the database
					strMsg = String.Format("You must close the {0} database before performing the compact operation. Do you want to close the database now?", m_tmaxDatabase.CaseName);
					
					if(MessageBox.Show(strMsg, "Compact", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					{
						//	Get the path to the case folder and database file
						strFileSpec = m_tmaxDatabase.FileSpec;
						strCaseFolder = m_tmaxDatabase.Folder;

						//	Close the active database
						if(OnAppCloseCase(true) == true)
						{						
							//	Compact the database
							m_tmaxDatabase.Compact(strFileSpec);
							
							//	Reopen the database
							OnAppOpenCase(strCaseFolder, false, false);
							
						}// if(OnAppCloseCase(true) == true)

					}// if(MessageBox.Show(strMsg, "Compact", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					
				}
				else
				{
					//	Initialize the file selection dialog
					OpenFileDialog wndOpenFile = new System.Windows.Forms.OpenFileDialog(); ;
					wndOpenFile.CheckFileExists = true;
					wndOpenFile.CheckPathExists = true;
					wndOpenFile.Multiselect = false;
					wndOpenFile.Title = "Select Database";
					wndOpenFile.Filter = "Access Files (*.mdb)|*.mdb|All Files (*.*)|*.*";

					//	Open the dialog box
					if(wndOpenFile.ShowDialog() == DialogResult.OK)
					{
						//	Do we need to allocate a database to perform the operation?
						if((tmaxCompactor = m_tmaxDatabase) == null)
						{
							tmaxCompactor = new CTmaxCaseDatabase();
							InitializeDatabase(tmaxCompactor);
						}
						
						//	Perform the compact operation
						tmaxCompactor.Compact(wndOpenFile.FileName);

					}// if(openFile.ShowDialog() == DialogResult.OK)

				}// if((m_tmaxDatabase != null) && (m_tmaxDatabase.FileSpec.Length > 0))
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnAppCompactDatabase", m_tmaxErrorBuilder.Message(ERROR_ON_APP_COMPACT_DATABASE_EX), Ex);
			}

		}// private void OnAppCompactDatabase()

		/// <summary>This method is called when the user wants to view the user's manual</summary>
		private void OnAppUsersManual()
		{
			string strFileSpec = "";
			
			strFileSpec = m_tmaxAppOptions.GetUsersManualFileSpec();
			
			//	Does the file exist?
			if(System.IO.File.Exists(strFileSpec) == true)
			{
				System.Diagnostics.Process.Start(strFileSpec);
			}
			else
			{
				MessageBox.Show("Unable to locate " + strFileSpec, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			
		}// private void OnAppUsersManual()
		
		/// <summary>This method is called when the user wants to validate the database contents</summary>
		private void OnAppValidateDatabase()
		{
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Primaries != null);
			
			if(m_tmaxDatabase == null) return;
			if(m_tmaxDatabase.Primaries == null) return;
			
			//	Ask the database to edit the aliases
			try
			{
				m_tmaxDatabase.Validate();
			}
			catch
			{
			}
			
		}// private void OnAppValidateDatabase()
		
		/// <summary>This method handles all command events received by the application</summary>
		/// <param name="objSender">The object sending the event</param>
		/// <param name="Args">Command event arguments</param>
		private void OnTmaxCommand(object objSender, CTmaxCommandArgs Args)
		{
			switch(Args.Command)
			{
				case TmaxCommands.Activate:
					
					OnCmdActivate(objSender, Args);
					break;
			
				case TmaxCommands.StartDrag:
					
					OnCmdStartDrag(objSender, Args);
					break;
						
				case TmaxCommands.CompleteDrag:
					
					OnCmdCompleteDrag(objSender, Args);
					break;
						
				case TmaxCommands.AcceptDrop:
					
					OnCmdAcceptDrop(objSender, Args);
					break;
						
				case TmaxCommands.RegisterSource:
					
					OnCmdRegisterSource(objSender, Args);
					break;
			
				case TmaxCommands.Open:

					OnCmdOpen(objSender, Args);
					break;
			
				case TmaxCommands.Print:
					
					OnCmdPrint(objSender, Args);
					break;
			
				case TmaxCommands.Find:

					OnCmdFind(objSender, Args);
					break;
			
				case TmaxCommands.SetSearchResult:

					OnCmdSetSearchResult(objSender, Args);
					break;

				case TmaxCommands.SetDeposition:

					OnCmdSetDeposition(objSender, Args);
					break;

				case TmaxCommands.SetCodes:

					OnCmdSetCodes(objSender, Args);
					break;
			
				case TmaxCommands.Copy:
					
					OnCmdCopy(objSender, Args);
					break;
			
				case TmaxCommands.Update:
					
					OnCmdUpdate(objSender, Args);
					break;

				case TmaxCommands.Navigate:

					OnCmdNavigate(objSender, Args);
					break;

				case TmaxCommands.NavigatorChanged:

					OnCmdNavigatorChanged(objSender, Args);
					break;

				case TmaxCommands.Delete:
					
					OnCmdDelete(objSender, Args);
					break;
			
				case TmaxCommands.Duplicate:
					
					OnCmdDuplicate(objSender, Args);
					break;
			
				case TmaxCommands.Synchronize:
					
					OnCmdSynchronize(objSender, Args);
					break;
			
				case TmaxCommands.Reorder:
					
					OnCmdReorder(objSender, Args);
					break;
			
				case TmaxCommands.Add:
					
					OnCmdAdd(objSender, Args);
					break;
			
				case TmaxCommands.Move:
					
					OnCmdMove(objSender, Args);
					break;
			
				case TmaxCommands.Merge:
					
					OnCmdMerge(objSender, Args);
					break;
			
				case TmaxCommands.Edit:
					
					OnCmdEdit(objSender, Args);
					break;
			
				case TmaxCommands.SetCaseOptions:
					
					OnCmdSetCaseOptions(objSender, Args);
					break;
			
				case TmaxCommands.Import:
					
					OnCmdImport(objSender, Args);
					break;
			
				case TmaxCommands.Export:
					
					OnCmdExport(objSender, Args);
					break;
			
				case TmaxCommands.Rotate:
					
					OnCmdRotate(objSender, Args);
					break;
			
				case TmaxCommands.Help:
					
					OnCmdHelp(objSender, Args);
					break;
			
				case TmaxCommands.SetFilter:
					
					OnCmdSetFilter(objSender, Args);
					break;
			
				case TmaxCommands.SetPrimariesOrder:
					
					OnCmdSetPrimariesOrder(objSender, Args);
					break;
			
				case TmaxCommands.HideCaseCodes:
					
					OnCmdHideCaseCodes(objSender, Args);
					break;
			
				case TmaxCommands.EditDesignation:
					
					OnCmdEditDesignation(objSender, Args);
					break;
			
				case TmaxCommands.RefreshCodes:
					
					OnCmdRefreshCodes(objSender, Args);
					break;
			
				case TmaxCommands.AddNotification:
					
					OnCmdAddNotification(objSender, Args);
					break;
			
				case TmaxCommands.EndNotification:
					
					OnCmdEndNotification(objSender, Args);
					break;
			
				case TmaxCommands.BulkUpdate:
					
					OnCmdBulkUpdate(objSender, Args);
					break;
			
				case TmaxCommands.SetTargetBinder:
					
					OnCmdSetTargetBinder(objSender, Args);
					break;
			
			}// switch(Args.Command)
			
		}
		
		/// <summary>This method handles all asynchronous window messages received by the application</summary>
		/// <param name="objSender">The object sending the event</param>
		/// <param name="eAppMessage">The application message identifier</param>
		/// <param name="rMsg">The custom window message</param>
		/// <returns>true if handled</returns>
		private bool OnTmaxAsyncMessage(object objSender, TmaxWindowMessages eWndMessage, ref Message rMsg)
		{
			switch(eWndMessage)
			{
				case TmaxWindowMessages.Command:
					
					try
					{
						OnTmaxAsyncCommand((TmaxCommands)((int)(rMsg.WParam)), (int)(rMsg.LParam));
						return true;
					}
					catch
					{
						return false;
					}
					
				default:
				
					return false;
			
			}// switch(eWndMessage)
			
		}// private bool OnTmaxAsyncMessage(object objSender, TmaxWindowMessages eWndMessage, ref Message rMsg)
		
		/// <summary>This method handles all custom window messages received by the application</summary>
		/// <param name="objSender">The object sending the event</param>
		/// <param name="eAppMessage">The application message identifier</param>
		/// <param name="rMsg">The custom window message</param>
		/// <returns>true if handled</returns>
		private bool OnTmaxAsyncCommand(TmaxCommands eCommand, int iPane)
		{
			CTmaxCommandArgs Args = null;
			
			switch(eCommand)
			{
				case TmaxCommands.Activate:
					
					//	Get the command arguments
					if((iPane >= 0) && (iPane < (int)(TmaxAppPanes.MaxPanes)))
						Args = m_aPanes[iPane].AsyncCommandArgs;
					
					if(Args != null)
					{
						OnCmdActivate(m_aPanes[iPane], Args);
						m_aPanes[iPane].AcknowledgeAsyncCommand(Args, true);
						
						//	Is the sending pane visible?
						if(IsVisible(iPane) == true)
						{
							//	This is done to try to make sure the pane maintains the focus
							if(GetUltraPane((TmaxAppPanes)iPane).IsActive == false)
							{
								GetUltraPane((TmaxAppPanes)iPane).Activate();
							}
						
						}// if(IsVisible(iPane) == true)
					
					}
					return true;
					
				default:
				
					return false;
			
			}// switch(eAppMessage)
			
		}// private bool OnTmaxAsyncCommand(TmaxCommands eCommand, TmaxAppPanes ePane)
		
		/// <summary>This method handles InternalUpdate events fired by the database</summary>
		/// <param name="objSender">The database object firing the event</param>
		/// <param name="tmaxItems">The event items that identify the records being updated</param>
		private void OnDbInternalUpdate(object objSender, CTmaxItems tmaxItems)
		{
			//	Were any records updated?
			if((tmaxItems != null) && (tmaxItems.Count > 0))
			{
				foreach(CTmaxItem O in tmaxItems)
				{
					try
					{
						//	Notify the child panes
						for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
						{
							if(m_aPanes[i] != null)
							{
								m_aPanes[i].OnUpdated(O);
							}
					
						}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
					}
					catch
					{
					}
					
				}// foreach(CTmaxItem O in tmaxItems)
				
			}// if((tmaxItems != null) && (tmaxItems.Count > 0))

		}// private void OnDbInternalUpdate(object objSender, CTmaxItems tmaxItems)
		
		/// <summary>This method handles all KeyDown events fired by the keyboard filter</summary>
		/// <param name="objSender">The object sending the event</param>
		/// <param name="eKey">The key being pressed</param>
		///	<param name="eModifiers">The current control/shift/alt key states</param>
		/// <param name="bProcessed">Set to true if the keystroke is processed</param>
		private void OnTmaxKeyDown(object objSender, Keys eKey, Keys eModifiers, ref bool bProcessed)
		{
			CBasePane tmaxPane = null;

			//	Initialize the return value
			bProcessed = false;
				
			try
			{
				//	Don't bother if the hook is disabled
				if((m_tmaxAppOptions != null) && (m_tmaxAppOptions.DisableTmaxKeyboard == false))
				{
					if((tmaxPane = GetActivePane()) != null)
						bProcessed = tmaxPane.OnKeyDown(eKey, eModifiers);
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnKFKeyDown", "Ex: " + Ex.ToString());
			}

		}// private void OnTmaxKeyDown(object objSender, Keys eKey, Keys eModifiers, ref bool bProcessed)
		
		/// <summary>This method handles all Hotkey events fired by the keyboard filter</summary>
		/// <param name="objSender">The object sending the event</param>
		/// <param name="eHotkey">The enumerated hotkey</param>
		/// <param name="bProcessed">Set to true if the keystroke is processed</param>
		private void OnTmaxHotkey(object objSender, TmaxHotkeys eHotkey, ref bool bProcessed)
		{
			CBasePane tmaxPane = GetActivePane();

			//	Don't bother if the hook is disabled
			if((m_tmaxAppOptions != null) && (m_tmaxAppOptions.DisableTmaxKeyboard == true))
				return;
				
			//	Initialize the return value. Assume all application hotkeys
			//	get processed
			bProcessed = true;
			
			//	Which hotkey has been pressed?
			switch(eHotkey)
			{
				case TmaxHotkeys.FileNew:
					
					OnAppOpenCase("", true, false);
					break;

				case TmaxHotkeys.FileOpen:
					
					OnAppOpenCase("", false, false);
					break;

				case TmaxHotkeys.ReloadCase:
					
					OnAppReloadCase();
					break;

				case TmaxHotkeys.CaseOptions:
				
					if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null))
						OnAppCaseOptions();
					break;
					
				case TmaxHotkeys.RefreshTreatments:
					
					if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null))
					{
						OnAppRefreshTreatments();
					}
					
					break;

				case TmaxHotkeys.OpenLast:

					//	Don't bother if case is already open
					if((m_tmaxDatabase == null) || (m_tmaxDatabase.Primaries == null))
					{
						if(m_tmaxAppOptions.RecentlyUsed.Count > 0)
						{
							OnAppOpenCase(m_tmaxAppOptions.RecentlyUsed[0].ToString(), false, false);
						}
						
					}
					break;
					
				case TmaxHotkeys.OpenHelp:
				
					SetUltraPaneVisible(TmaxAppPanes.Help, true, true);
					break;
					
				case TmaxHotkeys.ViewProperties:
				
					SetUltraPaneVisible(TmaxAppPanes.Properties, true, true);
					break;
					
				case TmaxHotkeys.ViewBuilder:
				
					//m_tmaxEventSource.FireDiagnostic(this, "OnTmaxHotkey", "Handling hotkey to open script builder");
					SetUltraPaneVisible(TmaxAppPanes.Scripts, true, true);
					break;
					
				case TmaxHotkeys.ViewTuner:
				
					SetUltraPaneVisible(TmaxAppPanes.Tuner, true, true);
					break;
					
				case TmaxHotkeys.ViewMediaViewer:
				
					SetUltraPaneVisible(TmaxAppPanes.Viewer, true, true);
					break;

				case TmaxHotkeys.ViewCodes:
				
					SetUltraPaneVisible(TmaxAppPanes.Codes, true, true);
					break;

				case TmaxHotkeys.ScreenCapture:
				
					OnAppScreenCapture();
					break;

				case TmaxHotkeys.OpenPresentation:

					//	Let the active pane have first crack
					if(tmaxPane != null)
					{
						bProcessed = tmaxPane.OnHotkey(eHotkey);
					}

					//	Use the default pane if the active pane ignored the request
					if((bProcessed == false) || (tmaxPane == null))
					{
						//	The MediaViewer is the default pane for sending to presentation
						if((m_aPanes != null) && (m_aPanes[(int)TmaxAppPanes.Viewer] != null))
						{
							m_aPanes[(int)TmaxAppPanes.Viewer].OnHotkey(eHotkey);
						}
					
					}
					break;
					
				case TmaxHotkeys.BlankPresentation:
				
					OnAppOpenPresentation();
					break;
					
				case TmaxHotkeys.Print:
				
					OnAppPrint();
					break;
					
				case TmaxHotkeys.Copy:
				case TmaxHotkeys.Paste:
				
					//	Let the active pane process this request
					if(tmaxPane != null)
					{
						bProcessed = tmaxPane.OnHotkey(eHotkey);
					}
					else
					{
						bProcessed = false;
					}
					break;
					
				case TmaxHotkeys.GoToBarcode:
				
					SetFocusLoadBarcode();
					break;
					
				case TmaxHotkeys.Find:
				
					OnAppFind();
					break;
					
				case TmaxHotkeys.FindNext:
				
					//	Only the results pane can process this hotkey
					if(m_paneResults != null)
						m_paneResults.OnHotkey(eHotkey);
					break;
					
				case TmaxHotkeys.SetFilter:
				
					OnAppSetFilter(AppCommands.SetFilter);
					break;
					
				case TmaxHotkeys.FastFilter:
				
					OnAppSetFilter(AppCommands.FastFilter);
					break;
					
				case TmaxHotkeys.AddToBinder:
				case TmaxHotkeys.AddToScript:
				case TmaxHotkeys.GoTo:
				case TmaxHotkeys.Save:
				case TmaxHotkeys.Delete:
				
					//	Let the active pane process this request
					if(tmaxPane != null)
					{
						tmaxPane.OnHotkey(eHotkey);
					}
					break;

				case TmaxHotkeys.AddObjection:
				case TmaxHotkeys.RepeatObjection:

					//	Let the active pane have first crack
					if(tmaxPane != null)
					{
						bProcessed = tmaxPane.OnHotkey(eHotkey);
					}

					//	Use the default pane if the active pane ignored the request
					if((bProcessed == false) || (tmaxPane == null))
					{
						//	The Objections pane is the default pane for adding objections
						if((m_aPanes != null) && (m_aPanes[(int)TmaxAppPanes.Objections] != null))
						{
							m_aPanes[(int)TmaxAppPanes.Objections].OnHotkey(eHotkey);
						}

					}
					break;

				default:
				
					//MessageBox.Show(eHotkey.ToString() + " hotkey not yet implemented");
					break;
			}
		
		}// private void OnTmaxHotkey(object objSender, TmaxHotkeys eHotkey)
		
		/// <summary>This method is called to disable / enable the keyboard hook</summary>
		/// <param name="bDisable">True to disable the keyboard hook</param>
		private void DisableTmaxKeyboard(bool bDisable)
		{
			if(m_tmaxAppOptions != null)
				m_tmaxAppOptions.DisableTmaxKeyboard = bDisable;
		
		}// private void DisableTmaxKeyboard(bool bDisable)
		
		/// <summary>This method is called to fire the command required to import binders or scripts</summary>
		/// <param name="eCommand">The application command identifier for the import operation</param>
		private void FireImportCommand(AppCommands eCommand)
		{
			CTmaxCommandArgs	tmaxArgs = null;
			CTmaxItems			tmaxItems = null;
			CTmaxItem			tmaxItem = null;
			CTmaxParameters		tmaxParameters = null;
			
			try
			{
				//	Create the event items collection
				tmaxItems = new CTmaxItems();
				tmaxItem = new CTmaxItem();
				tmaxItems.Add(tmaxItem);
				
				//	Initialize the command parameters
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.MergeImported, false);
				
				//	Are we importing objections?
				if(eCommand == AppCommands.ImportAsciiObjections)
				{
					tmaxItem = null; // Don't need if for objections
					tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.Objections, true));
				}
				else if(eCommand == AppCommands.ImportAsciiBinder)
				{
					tmaxItem.DataType  = TmaxDataTypes.Binder;
					tmaxItem.MediaType = TmaxMediaTypes.Unknown;
					tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.ImportFormat, (int)TmaxImportFormats.AsciiBinder));
				}
				else if(eCommand == AppCommands.ImportXmlBinder)
				{
					tmaxItem.DataType  = TmaxDataTypes.Binder;
					tmaxItem.MediaType = TmaxMediaTypes.Unknown;
					tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.ImportFormat, (int)TmaxImportFormats.XmlBinder));
				}
				else if(eCommand == AppCommands.ImportXmlCaseCodes)
				{
					tmaxItem.DataType  = TmaxDataTypes.CaseCode;
					tmaxItem.MediaType = TmaxMediaTypes.Unknown;
					tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.ImportFormat, (int)TmaxImportFormats.XmlCaseCodes));
				}
				else
				{
					//	Must be importing a script
					tmaxItem.DataType = TmaxDataTypes.Media;
					tmaxItem.MediaType = TmaxMediaTypes.Script;
					
					tmaxParameters.Add(TmaxCommandParameters.Activate, true);
					if(eCommand == AppCommands.ImportXmlScript)
						tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.ImportFormat, (int)TmaxImportFormats.XmlScript));
					else
						tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.ImportFormat, (int)TmaxImportFormats.AsciiMedia));
				}
			
				// Get the command arguments
				if((tmaxArgs = new CTmaxCommandArgs(TmaxCommands.Import, 0, tmaxItems, tmaxParameters)) != null)
				{
					//	Make it look this the command was fired and trapped
					OnCmdImport(this, tmaxArgs);
				}
			
			}
			catch
			{
			}
			
		}// private void FireImportCommand(AppCommands eCommand)
		
		/// <summary>This method is called to fire the command required to preform the export operation</summary>
		/// <param name="eCommand">The application command identifier for the export operation</param>
		private void FireExportCommand(AppCommands eCommand)
		{
			CTmaxCommandArgs	tmaxArgs = null;
			CTmaxItems			tmaxItems = null;
			CTmaxItem			tmaxItem = null;
			CTmaxParameters		tmaxParameters = null;
			
			try
			{
				//	Create the event items collection
				tmaxItems = new CTmaxItems();
				tmaxItem = new CTmaxItem();
				tmaxItems.Add(tmaxItem);

				//	Initialize the command parameters
				tmaxParameters = new CTmaxParameters();
				
				switch(eCommand)
				{
					case AppCommands.ExportAsciiObjections:

						tmaxItems = null;
						tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.Objections, true));
						break;

					case AppCommands.ExportBarcodeMap:
					
						tmaxItem.DataType  = TmaxDataTypes.Unknown;
						tmaxItem.MediaType = TmaxMediaTypes.Unknown;
						tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.ExportFormat, (int)TmaxExportFormats.BarcodeMap));
						break;
						
					case AppCommands.ExportXmlCaseCodes:
					
						tmaxItem.DataType  = TmaxDataTypes.CaseCode;
						tmaxItem.MediaType = TmaxMediaTypes.Unknown;
						tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.ExportFormat, (int)TmaxExportFormats.XmlCaseCodes));
						break;
						
					default:
					
						Debug.Assert(false, "Unknown export command : " + eCommand.ToString());
						tmaxParameters = null; // Cancel the operation
						break;
				}
				
				// Get the command arguments
				if(tmaxParameters != null)
				{
					if((tmaxArgs = new CTmaxCommandArgs(TmaxCommands.Export, 0, tmaxItems, tmaxParameters)) != null)
					{
						//	Make it look this the command was fired and trapped
						OnCmdExport(this, tmaxArgs);
					}
				
				}
			
			}
			catch
			{
			}
			
		}// private void FireExportCommand(AppCommands eCommand)
		
		/// <summary>This method is called to fire the command required to set the primary media filter</summary>
		/// <param name="eCommand">The application command identifier for the export operation</param>
		private void FireFilterCommand(AppCommands eCommand)
		{
			CTmaxCommandArgs	tmaxArgs = null;
			CTmaxParameters		tmaxParameters = null;
			long				lFlags = 0;
			
			try
			{
				//	Initialize the command parameters
				tmaxParameters = new CTmaxParameters();
				
				switch(eCommand)
				{
					case AppCommands.FastFilter:
					
						lFlags = (long)(TmaxSetFilterFlags.PromptUser);
						break;
						
					case AppCommands.SetFilter:
					
						lFlags = (long)(TmaxSetFilterFlags.PromptUser | TmaxSetFilterFlags.Advanced);
						break;
						
					default:
					
						Debug.Assert(false, "Unknown set filter command : " + eCommand.ToString());
						tmaxParameters = null; // Cancel the operation
						break;
				}
				
				// Get the command arguments
				if(tmaxParameters != null)
				{
					//	Add the flags to the parameters collection
					tmaxParameters.Add(TmaxCommandParameters.SetFilterFlags, lFlags);
					
					if((tmaxArgs = new CTmaxCommandArgs(TmaxCommands.SetFilter, 0, null, tmaxParameters)) != null)
					{
						//	Make it look this the command was fired and trapped
						OnCmdSetFilter(this, tmaxArgs);
					}
				
				}// if(tmaxParameters != null)
			
			}
			catch
			{
			}
			
		}// private void FireFilterCommand(AppCommands eCommand)
		
		/// <summary>This method will process the command line specified by the caller</summary>
		/// <param name="tmaxCommandLine">The command line arguments</param>
		private void ProcessCommandLine(CTmaxCommandLine tmaxCommandLine)
		{	
			Debug.Assert(tmaxCommandLine != null);
			if(tmaxCommandLine == null) return;
			
			try
			{
				//	Did the user specify a database?
				if((tmaxCommandLine.CaseFolder != null) && (tmaxCommandLine.CaseFolder.Length > 0))
				{
					//	Do we have an active database?
					if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null))
					{
						//	Did the user specify a different case?
						if(String.Compare(m_tmaxDatabase.Folder, tmaxCommandLine.CaseFolder, true) != 0)
						{
							//	Open the requested case
							OnAppOpenCase(tmaxCommandLine.CaseFolder, false, false);
						}
					
					}
					else
					{
						//	Open the requested case
						OnAppOpenCase(tmaxCommandLine.CaseFolder, false, false);
					}
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "ProcessCommandLine", "Ex: " + Ex.ToString());
			}
		
		}// private void ProcessCommandLine(CTmaxCommandLine tmaxCommandLine)
		
		/// <summary>This method handles all Diagnostic events received by the application</summary>
		/// <param name="objSender">The object sending the event</param>
		/// <param name="Args">Diagnostic event arguments</param>
		private void OnDiagnostic(object objSender, CTmaxDiagnosticArgs Args)
		{
			if(m_tmaxAppOptions.EnableDiagnostics == true)
			{
				//	Add to the diagnostic pane
				if(m_paneDiagnostics != null)
					m_paneDiagnostics.Add(Args);
					
				//	Add to the log file
				if(m_xmlDiagnostics != null && (m_tmaxAppOptions.LogDiagnostics == true))
					m_xmlDiagnostics.Write(Args);
			}
		}
		
		/// <summary>This method handles all Error events received by the application</summary>
		/// <param name="objSender">The object sending the event</param>
		/// <param name="Args">Error event arguments</param>
		private void OnError(object objSender, CTmaxErrorArgs Args)
		{
			bool bSimulator = false;
			
			FTI.Trialmax.Forms.CFErrorMessage cfErrorMessage = null;
			
			//	Is the simulator active?
			if(m_tmaxSimulator != null)
				bSimulator = m_tmaxSimulator.Active;
				
			//	Should we display the popup?
			if((bSimulator == false) && (m_bTerminating == false) && (Args.Show == true) && (m_tmaxAppOptions.ShowErrorMessages == true))
			{
				User.MessageBeep(User.MB_ICONEXCLAMATION);
				cfErrorMessage = new FTI.Trialmax.Forms.CFErrorMessage();
				cfErrorMessage.SetControls(Args);
				cfErrorMessage.ShowDialog(this);
			}
			
			//	Add to the errors pane as long as the error pane is not the source
			//	of the error
			if((m_bTerminating == false) && (m_paneErrors != null) && (object.ReferenceEquals(m_paneErrors, objSender) == false))
				m_paneErrors.Add(Args);
				
			//	Add to the error log as long as the log did not generate the error
			if((m_xmlErrors != null) && (object.ReferenceEquals(m_xmlErrors, objSender) == false))
				m_xmlErrors.Write(Args);
		}
		
		/// <summary>This method is called when the user wants to go to the barcode entered in the main toolbar</summary>
		/// <returns>true if successful</returns>
		private void OnAppLoadBarcode()
		{
			CDxMediaRecord	dxRecord = null;
			TextBoxTool		tbBarcode = null;
			
			if(m_tmaxDatabase == null) return;
			if(m_tmaxDatabase.Primaries == null) return;
			
			try
			{
				//	Get the barcode specified by the user
				if((tbBarcode = (TextBoxTool)GetUltraTool("LoadBarcode")) == null)
					return;
				if(tbBarcode.Text.Length == 0)
					return;
					
				//	Get the record identified by this barcode
				dxRecord = m_tmaxDatabase.GetRecordFromBarcode(tbBarcode.Text, true, false);
				
				if(dxRecord != null)
				{
					//	Select this record in the media tree
					m_paneMedia.Select(new CTmaxItem(dxRecord), true, true);
					m_paneMedia.Focus();// Take focus away from text box
					
					//	Clear the barcode text box
					tbBarcode.Text = "";
					
					//	Make sure the appropriate pane is visible
					SetActivePane(dxRecord.MediaType);

				}
				else
				{
					MessageBox.Show(tbBarcode.Text + " is not a valid barcode!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					SetFocusLoadBarcode();
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnAppLoadBarcode", "Ex: " + Ex.ToString());
			}
			
		}// private bool OnAppLoadBarcode()
		
		/// <summary>This function handles events fired by the PowerPoint interface control</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Event parameters</param>
		private void OnPowerPoint(object sender, FTI.Trialmax.MSOffice.MSPowerPoint.CMSPowerPointArgs e)
		{
			switch(e.EventId)
			{
				case MSPowerPointEvents.OpenPresentation:
				
					//MessageBox.Show(e.FileSpec, "OPEN");
					break;
					
				case MSPowerPointEvents.ClosePresentation:
				
					//MessageBox.Show(e.FileSpec, "CLOSE");
					break;
					
				case MSPowerPointEvents.SavePresentation:
				
					//MessageBox.Show(e.FileSpec, "SAVE");
					break;
					
			}
		}
		
		/// <summary>This function handles events fired by the Presentation interface control</summary>
		/// <param name="sender">The object firing the event</param>
		private void OnPresentationRequest(object objSender)
		{
			//	What is the command?
			switch(m_ctrlPresentation.Command)
			{
				case TmxShareCommands.AddTreatment:
				
					OnPresentationAddTreatment();
					break;
					
				case TmxShareCommands.AddToBinder:
				
					OnPresentationAddToBinder();
					break;

				case TmxShareCommands.UpdateTreatment:

					OnPresentationUpdateTreatment();
					break;

			}
		
		}// private void OnPresentationRequest(object objSender)

		/// <summary>This method is called Presentation fires an AddTreatment command request</summary>
		private void OnPresentationAddTreatment()
		{
			CTmaxDatabaseResults	tmaxResults = new CTmaxDatabaseResults();
			CTmaxItem				tmaxAdded = null;
			bool					bAddDelay = false;
			
			if(m_tmaxDatabase == null) return;
			if(m_tmaxDatabase.Primaries == null) return;
			
			//	Ask the database to add the treatment
			m_tmaxDatabase.AddZap(m_ctrlPresentation.SourceFilePath, tmaxResults);
			
			//	Did the database add a new record (or records)?
			for(int iAdded = 0; iAdded < tmaxResults.Added.Count; iAdded++)
			{
				if((tmaxAdded = tmaxResults.Added[iAdded]) != null)
				{
					if((tmaxAdded.SubItems != null) && (tmaxAdded.SubItems.Count > 0))
					{
						//	Notify each child pane
						for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
						{
							if(m_aPanes[i] != null)
							{
								m_aPanes[i].OnAdded(tmaxAdded, tmaxAdded.SubItems);
							}

						}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
						
						//	Notify TmaxPresentation application
						//
						//	NOTE:	We have to iterate the SubItems collection because it's possible
						//			that the source page is the same event though it's a split screen zap
						foreach(CTmaxItem O in tmaxAdded.SubItems)
						{
							//	If we have to send more than one response we have
							//	to introduce a delay to give TmaxPresentation time to read
							//	the shared buffer. We should probably replace this with some
							//	type of response acknowlege but for now we'll take the easy
							//	way out
							//
							//	NOTE:	This should only occur when adding split screen
							//			treatments
							if(bAddDelay == true)
								System.Threading.Thread.Sleep(750);

							m_ctrlPresentation.Command = TmxShareCommands.AddTreatment;
							m_ctrlPresentation.CaseFolder		= m_tmaxDatabase.Folder;
							m_ctrlPresentation.PrimaryId		= (int)O.IPrimary.GetAutoId();
							m_ctrlPresentation.SecondaryId		= (int)O.ISecondary.GetAutoId();
							m_ctrlPresentation.TertiaryId		= (int)O.ITertiary.GetAutoId();
							m_ctrlPresentation.DisplayOrder		= (int)O.ITertiary.GetDisplayOrder();
							m_ctrlPresentation.BarcodeId		= (int)O.ITertiary.GetBarcodeId();
							m_ctrlPresentation.Barcode			= O.ITertiary.GetBarcode(true);
							m_ctrlPresentation.SourceFilePath	= O.ITertiary.GetFileSpec();
							m_ctrlPresentation.SourceFileName	= O.ITertiary.GetFileName();
							m_ctrlPresentation.Respond();
							
							//	Make sure we add a delay if we have to send
							//	another response
							bAddDelay = true;
							
						}

					}// if((tmaxAdded.SubItems != null) && (tmaxAdded.SubItems.Count > 0))

				}// if((tmaxAdded = tmaxResults.Added[iAdded]) != null)

			}// if(tmaxResults.Added.Count > 0)			
				
		}// private void OnPresentationAddTreatment()

		/// <summary>This method is called Presentation fires an AddTreatment command request</summary>
		private void OnPresentationAddToBinder()
		{
			CTmaxCommandArgs	Args = null;
			CTmaxItems			tmaxItems = null;
			CTmaxItem			tmaxItem = null;
			CTmaxItem			tmaxSource = null;
			CDxMediaRecord		dxRecord = null;
			CTmaxParameters		tmaxParameters = null;
			
			try
			{
				Debug.Assert(m_tmaxDatabase != null);
				if(m_tmaxDatabase == null) return;
				
				//	Notify the binder tree
				if(m_paneBinders != null)
					m_paneBinders.AddFromPresentation = true;
					
				//	Make sure we can get the specified record
				if((dxRecord = m_tmaxDatabase.GetRecordFromBarcode(m_ctrlPresentation.Barcode, true, false)) != null)
				{
					//	Do we need to add a default binder to put this barcode in?
					if(m_tmaxDatabase.TargetBinder == null)
					{
						//	Make it look like we're adding a binder using a filename
						tmaxSource = new CTmaxItem();
						tmaxSource.SourceFile = new CTmaxSourceFile("TmaxPresentation");
						
						tmaxItem = new CTmaxItem();
						tmaxItem.DataType = TmaxDataTypes.Binder;
						if(tmaxItem.SourceItems == null)
							tmaxItem.SourceItems = new CTmaxItems();
						tmaxItem.SourceItems.Add(tmaxSource);
							
						//	Build the command arguments
						tmaxItems = new CTmaxItems();
						tmaxItems.Add(tmaxItem);
						
						Args = new CTmaxCommandArgs(TmaxCommands.Add, (int)(TmaxAppPanes.Binders), tmaxItems); 
						OnCmdAdd(this, Args);
					}
					
					//	We should have a target binder now
					if(m_tmaxDatabase.TargetBinder != null)
					{
						//	Add this record to the target
						tmaxSource = new CTmaxItem(dxRecord);
						
						tmaxItem = new CTmaxItem(m_tmaxDatabase.TargetBinder);
						if(tmaxItem.SourceItems == null)
							tmaxItem.SourceItems = new CTmaxItems();
						tmaxItem.SourceItems.Add(tmaxSource);
							
						//	Build the command arguments
						tmaxItems = new CTmaxItems();
						tmaxItems.Add(tmaxItem);
						
						//	Add a parameter to prevent duplicate entries
						tmaxParameters = new CTmaxParameters();
						tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.NoDuplicates, true));
						
						Args = new CTmaxCommandArgs(TmaxCommands.Add, (int)(TmaxAppPanes.Binders), tmaxItems, tmaxParameters); 
						OnCmdAdd(this, Args);
					}
				
				}// if((dxRecord = m_tmaxDatabase.GetRecordFromBarcode(m_ctrlPresentation.Barcode, false)) != null)
			
			}
			catch(System.Exception Ex)
			{
				MessageBox.Show(Ex.ToString());
			}
			finally
			{
				//	Clear the binder tree flag
				if(m_paneBinders != null)
					m_paneBinders.AddFromPresentation = false;			
			}
			
		}// private void OnPresentationAddTreatment()

		/// <summary>This method is called Presentation fires an UpdateTreatment command request</summary>
		private void OnPresentationUpdateTreatment()
		{
			CTmaxItem			tmaxUpdated = null;
			CDxMediaRecord		dxRecord = null;
			string				strUniqueId = "";
			
			if(m_tmaxDatabase == null) return;
			if(m_tmaxDatabase.Primaries == null) return;

			//	Format the unique identifier for the treatment
			strUniqueId = String.Format("{0}.{1}.{2}", m_ctrlPresentation.PrimaryId,
													   m_ctrlPresentation.SecondaryId,
													   m_ctrlPresentation.TertiaryId);
			//	Get the database record
			if((dxRecord = m_tmaxDatabase.GetRecordFromId(strUniqueId, true)) != null)
			{
				//	Create an event item to represent the record
				tmaxUpdated = new CTmaxItem(dxRecord);

				//	Process the request
				if(m_tmaxDatabase.Update(tmaxUpdated, null, null) == true)
				{
					//	Force the viewer to reload the file
					tmaxUpdated.Reload = true;
					
					//	Notify each pane
					for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
					{
						if(m_aPanes[i] != null)
						{
							m_aPanes[i].OnUpdated(tmaxUpdated);
						}

					}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)

				}// if(m_tmaxDatabase.Update(tmaxItem, null, null) == true)

			}
			else
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnPresentationUpdateTreatment", "Invalid unique id: " + strUniqueId);
			}

		}// private void OnPresentationUpdateTreatment()

		/// <summary>This function handles events fired by the docking manager when it is about to activate a pane</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event parameters</param>
		private void OnUltraPaneActivate(object sender, Infragistics.Win.UltraWinDock.ControlPaneEventArgs e)
		{
			//m_tmaxEventSource.FireDiagnostic(this, "OnUltraPaneActivate", "activating -> " + e.Pane.Text);

			//	Notify the pane being deactivated
			if((e.Pane != null) && (e.Pane.Control != null))
			{
				try { ((CBasePane)(e.Pane.Control)).OnPaneActivate(); }
				catch { }
			}

			//	Update all pane states
			SetPaneStates();

		}// private void OnUltraPaneActivate(object sender, Infragistics.Win.UltraWinDock.ControlPaneEventArgs e)

		/// <summary>This function handles events fired by the docking manager when it is about to deactivate a pane</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event parameters</param>
		private void OnUltraPaneDectivate(object sender, Infragistics.Win.UltraWinDock.ControlPaneEventArgs e)
		{
			//m_tmaxEventSource.FireDiagnostic(this, "OnUltraPaneDectivate", "deactivating -> " + e.Pane.Text);

			//	Notify the pane being deactivated
			if((e.Pane != null) && (e.Pane.Control != null))
			{
				try { ((CBasePane)(e.Pane.Control)).OnPaneDeactivate(); }
				catch { }
			}

			//	Update all pane states
			SetPaneStates();

		}// private void OnUltraPaneDectivate(object sender, Infragistics.Win.UltraWinDock.ControlPaneEventArgs e)

		/// <summary>This function handles events fired by the docking manager when the user clicks the close button on a pane</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event parameters</param>
		private void OnUltraPaneButtonClick(object sender, Infragistics.Win.UltraWinDock.PaneButtonEventArgs e)
		{
			if(e.Pane == m_ctrlUltraDockManager.ActivePane)
			{
				e.Pane.DockAreaPane.Activate();
			}
			SetPaneStates();
		
		}// private void OnUltraPaneButtonClick(object sender, Infragistics.Win.UltraWinDock.PaneButtonEventArgs e)

		/// <summary>This function handles events fired by the toolbar manager when the user clicks on a toolbar button or menu item</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">Infragistics event parameters</param>
		private void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		{
			//	Is processing of this event disabled?
			if(m_bIgnoreToolbarClicks == true) return;
			
			AppCommands eCommand = AppCommands.Invalid;
			
			if((eCommand = GetCommand(e.Tool.Key)) != AppCommands.Invalid)
				OnAppCommand(eCommand);
		
		}// OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)

		/// <summary>This function handles events fired by the toolbar manager when the user releases a key in one of the tools</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event parameters</param>
		private void OnUltraToolKeyDown(object sender, Infragistics.Win.UltraWinToolbars.ToolKeyEventArgs e)
		{
			if((e.Tool.Key == "LoadBarcode") && (e.KeyCode == Keys.Enter))
			{
				//	Mark the event as handled
				e.Handled = true;
				
				OnAppLoadBarcode();
			}
		
		}// private void OnUltraToolKeyDown(object sender, Infragistics.Win.UltraWinToolbars.ToolKeyEventArgs e)

		/// <summary>This event is fired by the toolbar manager when it is about to display the customize menu</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The cancelable event arguments</param>
		private void OnUltraBeforeToolbarListDropdown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventArgs e)
		{
			// Prevent this menu from coming up
			e.Cancel = true;
		}

		/// <summary>This function handles events fired by the toolbar manager when it is about to display a popup menu</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event parameters</param>
		private void OnUltraToolPopup(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventArgs e)
		{
			PopupMenuTool	popupMenu = null;
			AppCommands		eCommand = AppCommands.Invalid;
			
			switch(e.Tool.Key)
			{
				case "FileMenu":
				case "EditMenu":
				case "ImportMenu":
				case "ExportMenu":
				case "ToolsMenu":
				case "ViewMenu":
				case "HelpMenu":
				
					try
					{
						popupMenu = (PopupMenuTool)(e.Tool);
						
						//	Iterate the tool collection for this submenu
						foreach(ToolBase O in popupMenu.Tools)
						{
							//	Get the command associated with this tool
							if((eCommand = GetCommand(O.Key)) != AppCommands.Invalid)
							{
								//	Should this tool be enabled?
								O.SharedProps.Enabled = GetCommandEnabled(eCommand);
								
								//	Set the check state of the tool
								m_bIgnoreToolbarClicks = true;
								SetUltraCheckedState(O, eCommand);
								m_bIgnoreToolbarClicks = false;
								
							}
						
						}// foreach(ToolBase O in popupMenu.Tools)
						
					}
					catch
					{
					
					}
					
					//	Have to do some additional work for the File menu
					if(e.Tool.Key == "FileMenu")
						SetRecentlyUsed();
						
					break;
					
				default:
				
					break;
						
			}// switch(e.Tool.Key)
		
		}// OnUltraToolPopup(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventArgs e)

		/// <summary>This function is called to save the pane docking layout to file</summary>
		///	<param name="strFilename">Name of file used to save the layout</param>
		///	<returns>true if successful</returns>
		private bool SavePaneLayout(string strFilename)
		{
			System.IO.FileStream fsLayout = null; 
			bool bSuccessful = true;

			try
			{
				fsLayout = new System.IO.FileStream(strFilename, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
				this.m_ctrlUltraDockManager.SaveAsXML(fsLayout);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SavePaneLayout", m_tmaxErrorBuilder.Message(ERROR_SAVE_LAYOUT_EX, strFilename), Ex);
				bSuccessful = false;
			}
			finally
			{
				if(fsLayout != null)
					fsLayout.Close();
			}
			
			return bSuccessful;
			
		}// SavePaneLayout()

		/// <summary>This method is called to update the current state of the child panes</summary>
		private void SetPaneStates()
		{
			CBasePane basePane = null;
			
			//	Don't bother if terminating
			if(m_bTerminating == true) return;
			
			if(m_ctrlUltraDockManager == null) return;
			if(m_ctrlUltraDockManager.ControlPanes == null) return;

			//	Iterate the collection of dockable panes
			foreach(DockableControlPane dcp in m_ctrlUltraDockManager.ControlPanes)
			{
				
				try
				{
					if((basePane = (CBasePane)dcp.Control) != null)
					{
						if((dcp.IsSelectedTab == true) || (dcp.DockedState == DockedState.Floating))
							basePane.PaneVisible = true;
						else
							basePane.PaneVisible = IsVisible(dcp.Control);
					}
					
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireDiagnostic(this, "SetPaneStates", Ex.ToString());
				}
				
			}// foreach(DockableControlPane dcp in m_ctrlUltraDockManager.ControlPanes)
		
		}// private void SetPaneStates()
		
		/// <summary>This method is called to activate the pane appropriate for the specified media type</summary>
		private void SetActivePane(TmaxMediaTypes eMediaType)
		{
			try
			{
				switch(eMediaType)
				{
					case TmaxMediaTypes.Document:
					case TmaxMediaTypes.Powerpoint:
					case TmaxMediaTypes.Recording:
					case TmaxMediaTypes.Page:
					case TmaxMediaTypes.Segment:
					case TmaxMediaTypes.Slide:
					case TmaxMediaTypes.Treatment:
					case TmaxMediaTypes.Clip:

						if(m_paneViewer.PaneVisible == false)
						{
							SetUltraPaneVisible(TmaxAppPanes.Viewer, true, false);
						}
						break;
						
					case TmaxMediaTypes.Script:
					case TmaxMediaTypes.Scene:
					case TmaxMediaTypes.Designation:
					
						//	Only activate a pane if the script builder is not active
						//	and the viewer is not active
						if((m_paneScripts.PaneVisible == false) &&
							(m_paneViewer.PaneVisible == false) &&
							(m_paneTuner.PaneVisible == false))
						{
							//m_tmaxEventSource.FireDiagnostic(this, "SetActivePane", "Activating script builder for media type: -> " + eMediaType.ToString());
							SetUltraPaneVisible(TmaxAppPanes.Scripts, true, false);
						}
						break;
						
					case TmaxMediaTypes.Link:

						if((m_paneViewer.PaneVisible == false) &&
							(m_paneTuner.PaneVisible == false))
						{
							SetUltraPaneVisible(TmaxAppPanes.Tuner, true, false);
						}
						break;
						
					case TmaxMediaTypes.Deposition:

						if(m_paneTranscripts.PaneVisible == false)
						{
							SetUltraPaneVisible(TmaxAppPanes.Transcripts, true, false);
						}
						break;
						
					default:
					
						break;
						
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetActivePane", "Ex: " + Ex.ToString());
			}
			finally
			{
				//	Make sure the pane states are correct
				SetPaneStates();
			}
		
		}// private void SetActivePane(TmaxMediaTypes eMediaType)
		
		/// <summary>This method is called to update the database dependent controls</summary>
		private void SetControlStates()
		{
			ToolBase ultraTool = null;
			
			if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null))
			{
				if(m_tmaxDatabase.CaseName.Length > 0)
					this.Text = String.Format(" FTI TrialMax 7 - [{0}] - {1}", m_tmaxDatabase.CaseName, m_tmaxDatabase.Folder);
				else
					this.Text = String.Format(" FTI TrialMax 7 - {0}", m_tmaxDatabase.Folder);
				
				if((ultraTool = GetUltraTool("LoadBarcode")) != null)
					ultraTool.SharedProps.Enabled = true;
				if((ultraTool = GetUltraTool("LoadBarcodeLabel")) != null)
					ultraTool.SharedProps.Enabled = true;
			}
			else
			{
				this.Text = " FTI TrialMax 7";
				
				if((ultraTool = GetUltraTool("LoadBarcode")) != null)
					ultraTool.SharedProps.Enabled = false;
				if((ultraTool = GetUltraTool("LoadBarcodeLabel")) != null)
					ultraTool.SharedProps.Enabled = false;
			}
			
		}// protected void SetControlStates()
		
		/// <summary>This method will populate the local error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			ArrayList aStrings = null;
			
			if(m_tmaxErrorBuilder != null)
				aStrings = m_tmaxErrorBuilder.FormatStrings;
		
			if(aStrings == null) return;
				
			//	The format strings must be added in the order in which they are defined
			aStrings.Add("An exception was raised while initializing the screen layout");
			aStrings.Add("An exception was raised while initializing the %1 pane");
			aStrings.Add("An exception was raised while initializing the toolbar manager");
			aStrings.Add("An exception was raised while loading the child pane layout stored in %1");
			aStrings.Add("An exception was raised while saving the child pane layout to %1");
			
			aStrings.Add("An exception was raised while setting the menu tool's check state: TmaxAppPanes = %1 Key = %2");
			aStrings.Add("An exception was raised while setting the pane's visibility: TmaxAppPanes = %1");
			aStrings.Add("Unable to locate the specified pane: TmaxAppPanes = %1");
			aStrings.Add("An exception was raised while toggling the pane's visibility: TmaxAppPanes = %1");
			aStrings.Add("An exception was raised while attempting to add the %1 pane");
			
			aStrings.Add("An exception was raised while attempting to edit %1");
			aStrings.Add("Unable to edit %1. The file could not be found");
			aStrings.Add("An exception was raised while attempting to activate the requested record: pane = %1");
			aStrings.Add("An exception was raised while attempting to set the active search result: pane = %1");
			aStrings.Add("An exception was raised while attempting to set the TmaxPresentation options");
			
			aStrings.Add("An exception was raised while attempting to set the TmaxManager options");
			aStrings.Add("An exception was raised while attempting to initialize the Versions pane");
			aStrings.Add("An exception was raised while attempting to set the TmaxPresentation toolbars");
			aStrings.Add("An exception was raised while attempting to launch the updates installer application");
			aStrings.Add("An exception was raised while attempting to check for product updates");
			
			aStrings.Add("An exception was raised while attempting to update the TrialMax updates installer application: %1");
			aStrings.Add("An exception was raised while attempting to display the FTI contact information");
			aStrings.Add("An exception was raised while attempting to import a load file.");
			aStrings.Add("An exception was raised while initializing the application's product manager.");
			aStrings.Add("An exception was raised while initializing the application's screen capture manager.");
			
			aStrings.Add("The attempt to initialize the TrialMax screen capture manager failed.");
			aStrings.Add("An exception was raised while attempting to set the active deposition: pane = %1");
			aStrings.Add("An exception was raised while attempting to trim the database.");
			aStrings.Add("An exception was raised while attempting to compact a database.");

		}// private void SetErrorStrings()
		
		/// <summary>This method sets the checked state of the specified tool</summary>
		/// <param name="ultraTool">The tool to be set</param>
		/// <param name="eCommand">The command associated with the tool</param>
		private void SetUltraCheckedState(ToolBase ultraTool, AppCommands eCommand)
		{
			TmaxAppPanes Pane = TmaxAppPanes.MaxPanes;
			
			Debug.Assert(ultraTool != null);
			if(ultraTool == null) return;
			
			try
			{
				//	Do we need to get the command identifier?
				if(eCommand == AppCommands.Invalid)
					eCommand = GetCommand(ultraTool.Key);

				//	Which command is associated with this tool?
				switch(eCommand)
				{
					case AppCommands.ToggleSourceExplorer:    
					case AppCommands.ToggleMediaTree:    
					case AppCommands.ToggleBinders:    
					case AppCommands.ToggleFilteredTree:    
					case AppCommands.ToggleMediaViewer:    
					case AppCommands.ToggleProperties:
					case AppCommands.ToggleObjections:
					case AppCommands.ToggleObjectionProperties:
					case AppCommands.ToggleScriptBuilder: 
					case AppCommands.ToggleScriptReview:  
					case AppCommands.ToggleTranscripts: 
					case AppCommands.ToggleTuner:  
					case AppCommands.ToggleCodes:  
					case AppCommands.ToggleSearchResults: 
					case AppCommands.ToggleErrorMessages:    
					case AppCommands.ToggleVersions:    
					case AppCommands.ToggleDiagnostics: 
					case AppCommands.HelpContents:
					
						//	Get the pane associated with this command
						if((Pane = GetAppPane(eCommand)) != TmaxAppPanes.MaxPanes)
						{
							SetUltraCheckedState((StateButtonTool)ultraTool, Pane);
						}
						break;
						
					case AppCommands.LockPanes:
					
						((StateButtonTool)ultraTool).Checked = GetUltraLockedState();
						break;
						
				}// switch(GetCommand(ultraTool.Key))
					
			}
			catch
			{
			}
		
		}// private void SetUltraCheckedState(ToolBase ultraTool)
		
		/// <summary>This function is called to set the check state of a menu item</summary>
		/// <param name="Tool">Infragistics state button tool</param>
		/// <param name="eTmaxPane">Enumerated pane identifier used to determine the check state</param>
		private void SetUltraCheckedState(StateButtonTool Tool, TmaxAppPanes eTmaxPane)
		{
			DockablePaneBase Pane = null;
			
			Debug.Assert(Tool != null);
			
			try
			{
				//	Get the specified pane
				Pane = GetUltraPane(eTmaxPane);
				
				//	Do we have a valid pane object?
				if(Pane != null)
				{
					Tool.Checked = (Pane.Closed == false);
					Tool.SharedProps.Enabled = true;
				}
				else
				{
					Tool.SharedProps.Enabled = false;
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetMenuCheckState", m_tmaxErrorBuilder.Message(ERROR_SET_TOOL_STATE_EX, eTmaxPane, Tool.Key), Ex);
			}
					
		}// SetMenuCheckState(string strButtonKey, TmaxAppPanes eTmaxPane)
		
		/// <summary>Sets the visibility of the docking pane that owns the specified pane control</summary>
		/// <param name="eTmaxPane">Enumerated Pane Identifier</param>
		/// <param name="bVisible">true if visible</param>
		/// <param name="bActivate">true to activate the pane after making it visible</param>
		private void SetUltraPaneVisible(TmaxAppPanes eTmaxPane, bool bVisible, bool bActivate)
		{
			DockablePaneBase Pane = null;
			
			try
			{
				//	Get the specified pane object
				if((Pane = GetUltraPane(eTmaxPane)) != null)
				{
					if(bVisible == false)
					{
						Pane.Close(true);
					}
					else
					{
						Pane.Show();
			
						//	Should we activate the pane?
						if(bActivate)
						{
							Pane.Activate();
						}
						//else
						//{
			
						Pane.IsSelectedTab = true;
						//}
					}
				}
				else
				{
					m_tmaxEventSource.FireError(this, "SetUltraPaneVisible", m_tmaxErrorBuilder.Message(ERROR_LOCATE_PANE, eTmaxPane));
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetUltraPaneVisible", m_tmaxErrorBuilder.Message(ERROR_SET_PANE_VISIBLE_EX, eTmaxPane), Ex);
			}
		
		}// SetUltraPaneVisible(TmaxAppPanes eTmaxPane, bool bVisible)

		/// <summary>This function is called to set the recently used selections in the file menu</summary>
		private void SetRecentlyUsed()
		{
			ToolBase	Tool = null;
			string		strKey = "";
	
			for(int i = 0; i < m_tmaxAppOptions.MaxRecentlyUsed; i++)
			{
				//	Construct the key to the menu tool
				strKey = ("Recent" + ((i + 1).ToString()));
				
				//	Get the menu tool
				if((Tool = GetUltraTool(strKey)) != null)
				{
					//	Do we have a valid folder path?
					if((i < m_tmaxAppOptions.RecentlyUsed.Count) && 
						(m_tmaxAppOptions.RecentlyUsed[i].ToString().Length > 0))
					{
						Tool.SharedProps.Caption = m_tmaxAppOptions.RecentlyUsed[i].ToString();
						Tool.SharedProps.Visible = true;
					}
					else
					{
						Tool.SharedProps.Caption = "Not available";
						Tool.SharedProps.Visible = false;
					}
				}
				else
				{
					//	Out of menu tools
					break;
				}
				
			}
			
		}// private void OnShowFileMenu()
		
		/// <summary>This method is called to terminate the application and clean up all resources</summary>
		private void Terminate()
		{
			m_bTerminating = true;
			
			try
			{
				//	Shut down the application ActiveX interfaces
				if(m_tmxMovie != null)
				{
					m_tmxMovie.AxTerminate();
					m_tmxMovie = null;
				}
			
				m_ctrlPresentation.AxTerminate();

			}
			catch
			{
			}
			
			try
			{
				//	Stop the simulator if active
				if((m_tmaxSimulator != null) && (m_tmaxSimulator.Database != null))
					m_tmaxSimulator.Terminate(m_xmlIni);
				
				//	Kill the print manager
				if(m_tmaxPrintManager != null)
				{
					m_tmaxPrintManager.Dispose();
					m_tmaxPrintManager = null;
				}
			
				//	Terminate the connection to the registry
				if(m_tmaxRegistry != null)
				{
					m_tmaxRegistry.Terminate();
					m_tmaxRegistry = null;
				}
			
			}
			catch
			{
			}
			
			//	Make sure the current case is closed
			try { OnAppCloseCase(false); }
			catch {}

			//	Notify the panes
			for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
			{
				if(m_aPanes[i] != null)
				{
					try
					{
						Cursor.Current = Cursors.WaitCursor;
						
						try { m_aPanes[i].Terminate(m_xmlIni); }
						catch {}
						
						Cursor.Current = Cursors.Default;
					}
					catch
					{
					}

				}
			
			}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
			
			//	Save the configuration information to file
			try { m_tmaxRegOptions.Save(m_xmlIni); } 
			catch {}
			try { m_tmaxAppOptions.Save(m_xmlIni); } 
			catch {}
			try { m_tmaxSourceTypes.Save(m_xmlIni); } 
			catch {}
			try { m_tmaxReportManager.Save(m_xmlIni); } 
			catch {}
			try { m_tmaxProductManager.Save(m_xmlIni); } 
			catch {}
			try { m_mediaEncoder.Save(m_xmlIni); } 
			catch {}
			try { m_xmlIni.Save(); } 
			catch {}
		
		}// private void Terminate()
		
		/// <summary>This method is called to enable/disable the ability to drag panes</summary>
		/// <param name="bLock">True to enable dragging</param>
		private void SetPaneLocks(bool bLock)
		{
			foreach(DockablePaneBase O in m_ctrlUltraDockManager.ControlPanes)
			{
				try
				{
					O.Settings.AllowDragging = (bLock == true) ? DefaultableBoolean.True : DefaultableBoolean.False;
					O.Settings.AllowFloating = (bLock == true) ? DefaultableBoolean.True : DefaultableBoolean.False;
					O.Settings.DoubleClickAction = (bLock == true) ? PaneDoubleClickAction.Default : PaneDoubleClickAction.None;
				}
				catch
				{
				}

			}
			
		}// private void SetPaneLocks(bool bLock)
			
		/// <summary>This method performs validations and notifications required after loading a new layout</summary>
		private void OnLayoutLoaded()
		{
			DockableControlPane dcp = null;
			
			//	Make sure all controls have been put in the docking manager's collection
			for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
			{
				if(m_aPanes[i] != null)
				{
					try
					{
						if((dcp = m_ctrlUltraDockManager.PaneFromControl(m_aPanes[i])) == null)
						{
							switch((TmaxAppPanes)i)
							{
								case TmaxAppPanes.FilteredTree:
								
									AddPane((TmaxAppPanes)i, TmaxAppPanes.Media);
									break;

								case TmaxAppPanes.ScriptReview:

									AddPane((TmaxAppPanes)i, TmaxAppPanes.Scripts);
									break;

								case TmaxAppPanes.Codes:
								case TmaxAppPanes.Objections:
								case TmaxAppPanes.ObjectionProperties:
								default:
								
									AddPane((TmaxAppPanes)i, TmaxAppPanes.Properties);
									break;
							}
							
						}
						else
						{
							//	Is this the codes (filtered data) pane?
							if((TmaxAppPanes)i == TmaxAppPanes.Codes)
							{
								dcp.TextTab = "Fielded";
								dcp.Text = "Fielded Data";
							}
						
						}
					}
					catch
					{
					}
				}
							
			}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				
			//	Notify each of the child panes
			foreach(CBasePane O in m_aPanes)
			{
				try	  { O.OnAfterLoadLayout(); }
				catch { }
			}

		}// private void OnLayoutLoaded()
			
		/// <summary>This method will update the database version information</summary>
		private void SetDatabaseVersion()
		{
			Debug.Assert(m_verDatabase != null);
			if(m_verDatabase == null) return;
			Debug.Assert(m_paneVersions != null);
			if(m_paneVersions == null) return;
			Debug.Assert(m_paneVersions.IsDisposed == false);
			if(m_paneVersions.IsDisposed == true) return;
			
			m_verDatabase.Title = "Database";

			//	Do we have an active database?
			if((m_tmaxDatabase != null) && (m_tmaxDatabase.Detail != null) &&
				(m_tmaxDatabase.Primaries != null))
			{
				m_verDatabase.Location = m_tmaxDatabase.FileSpec.ToLower();
				m_verDatabase.ShortVersion = m_tmaxDatabase.Detail.Version;
				m_verDatabase.Description = ("Created: " + m_tmaxDatabase.Detail.CreatedOn.ToString());
			
			}
			else
			{
				m_verDatabase.BuildDate = "";
				m_verDatabase.Description = "";
				m_verDatabase.Location = "";
				m_verDatabase.ShortVersion = "";
			}
			
			//	Update the versions pane
			m_paneVersions.Update(m_verDatabase);
			
		}// private void SetDatabaseVersion()
		
		/// <summary>This method is called to launch the updates installer when the application exits</summary>
		private bool LaunchUpdatesInstaller()
		{
			System.Diagnostics.Process	installer = null;
			bool						bSuccessful = false;

			Debug.Assert(m_strXmlUpdateFileSpec.Length > 0);
			if(m_strXmlUpdateFileSpec.Length == 0) return false;
			if(System.IO.File.Exists(m_strUpdateInstaller) == false) return false;
			
			try
			{
				//	Create the process for launching the converter
				installer = new Process();
				
				//	Initialize the startup information
				installer.StartInfo.FileName = m_strUpdateInstaller;
				installer.StartInfo.Arguments = (" \"" + m_strXmlUpdateFileSpec + "\"");
				installer.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

				//	Start the process
				bSuccessful = installer.Start();

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "LaunchInstaller", m_tmaxErrorBuilder.Message(ERROR_LAUNCH_INSTALLER_EX), Ex);
			}
			
			return bSuccessful;
			
		}// private void private void LaunchUpdatesInstaller()
		
		/// <summary>This method is called to determine if the pane can be seen by the user</summary>
		/// <param name="iPane">The pane identifier (index)</param>
		/// <returns>true if the pane is visible on the screen</returns>
		private bool IsVisible(int iPane)
		{
			bool bVisible = false;
			
			try
			{
				if((iPane >= 0) && (iPane < (int)TmaxAppPanes.MaxPanes))
					bVisible = IsVisible(m_aPanes[iPane]);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "IsVisible", Ex);
			}
			
			return bVisible;
		
		}// private bool IsVisible(int iPane)
		
		/// <summary>This method is called to determine if the pane can be seen by the user</summary>
		/// <param name="ePane">The enumerated pane identifier</param>
		/// <returns>true if the pane is visible on the screen</returns>
		private bool IsVisible(TmaxAppPanes ePane)
		{
			bool bVisible = false;
			
			try
			{
				if(ePane != TmaxAppPanes.MaxPanes)
					bVisible = IsVisible((int)ePane);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "IsVisible", Ex);
			}
			
			return bVisible;
		
		}// private bool IsVisible(TmaxAppPanes ePane)
		
		/// <summary>This method is called to determine if the control can be seen by the user</summary>
		/// <param name="control">The control to be checked</param>
		/// <returns>true if the control is visible on the screen</returns>
		/// <remarks>This method is provided by Infragistics tech support</remarks>
		private bool IsVisible(Control control)
		{
			//m_tmaxEventSource.FireDiagnostic(this, "IsVisible", ((CBasePane)(control)).PaneName + " - " + control.Visible.ToString());
	
			if(control == null) return false;
			if(control.IsHandleCreated == false) return false;
			if(control.Visible == false) return false;
			
			try
			{
				//m_tmaxEventSource.FireDiagnostic(this, "IsVisible", ((CBasePane)(control)).PaneName + " - ");
				Rectangle rcScreen = control.RectangleToScreen(control.ClientRectangle);
				return IsVisible(control, rcScreen);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "IsVisible", Ex);
				
				//	Assume visible
				return true;
			}
		
		}// private bool IsVisible(Control control)
		
		/// <summary>This method is called to determine if the rectangle owned by the specified control is visible within it's parent's client rectangle</summary>
		/// <param name="control">The control associated with the specified rectangle</param>
		/// <param name="rcScreen">The screen rectangle to be checked</param>
		/// <returns>true if the complete or partial rectangle is visbile</returns>
		/// <remarks>This method is provided by Infragistics tech support</remarks>
		private bool IsVisible(Control control, Rectangle rcScreen)
		{
			Control parent = null;
			
			if(control == null) return false;
			if(control.IsHandleCreated == false) return false;
			if(control.Visible == false) return false;
			
			//	Have we walked to the top of the parent chain?
			if((parent = control.Parent) == null) return true;
			
			//	The parent must be visible for the control to be visible
			if(parent.IsHandleCreated == false) return false;
			if(parent.Visible == false) return false;
			
			//	Get the screen coordinates for the parent's rectangle
			Rectangle rcParent = parent.RectangleToScreen(parent.ClientRectangle);
			
			//	Eliminate any portion of the specified rectangle that is outside
			//	the parent's screen rectangle
			rcScreen.Intersect(rcParent);
			
			//	Not visible to end user if nothing visible inside the parent
			if(rcScreen.Size.IsEmpty == true) return false;
			
			//	Make sure not hidden by a sibling control
			for(int i = 0; i < parent.Controls.Count; i++)
			{
				//	The collection is maintained based on Z order with 0 on top
				Control sibling = parent.Controls[i];
				
				if(ReferenceEquals(sibling, control) == true)
					break;
					
				if(sibling.IsHandleCreated == false) continue;
				if(sibling.Visible == false) continue;
					
				Rectangle rcSibling = sibling.RectangleToScreen(sibling.ClientRectangle);
				
				//	Adjust the caller's rectangle if the sibling clips it
				if(rcSibling.IntersectsWith(rcScreen) == true)
					rcScreen.Intersect(rcSibling);
					
				if(rcScreen.Size.IsEmpty == true)
					return false;
			
			}// for(int i = 0; i < control.Controls.Count; i++)
			
			//	Walk the parent chain
			return IsVisible(parent, rcScreen);

		}// private bool IsVisible(Control control, Rectangle rcScreen)
		
		/// <summary>This method is called to execute a search operation</summary>
		/// <param name="tmaxItems">The items used to initialize the search</param>
		/// <returns>True if successful</returns>
		private bool Find(CTmaxItems tmaxItems, TmaxAppPanes eSource)
		{
			bool bSuccessful = false;
			
			//	Pass the request to the search results pane
			if((m_paneResults != null) && (m_paneResults.IsDisposed == false))
			{
				bSuccessful = m_paneResults.Search(tmaxItems, eSource);
			}
			
			return bSuccessful;
			
		}// private bool Find(CTmaxItems tmaxItems, TmaxAppPanes eSource)
		
		/// <summary>This method is called to notify the child panes when a database operation is complete</summary>
		/// <param name="tmaxResults">The results of the database operation</param>
		private void Notify(CTmaxDatabaseResults tmaxResults)
		{
			//	We're any records added?
			if((tmaxResults.Added != null) && (tmaxResults.Added.Count > 0))
			{
				foreach(CTmaxItem O in tmaxResults.Added)
				{
					//	Has anything been added?
					if((O.SubItems != null) && (O.SubItems.Count > 0))
					{
						for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
						{
							try { m_aPanes[i].OnAdded(O, O.SubItems); }
							catch {}
						}
					
						foreach(ITmaxAppNotification I in m_aIAppNotifications)
						{
							try  { I.OnAdded(O, O.SubItems);  }
							catch {}
						}

					}// if((O.SubItems != null) && (O.SubItems.Count > 0))
					
				}// foreach(CTmaxItem O in tmaxResults.Added)
					
			}// if((tmaxResults.Added != null) && (tmaxResults.Added.Count > 0))
				
			//	We're any records edited?
			if((tmaxResults.Edited != null) && (tmaxResults.Edited.GetMediaRecord() != null))
			{
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					try { if(m_aPanes[i] != null) m_aPanes[i].OnEdited(tmaxResults.Edited, tmaxResults); }
					catch {}
				}
					
			}// if((tmaxResults.Edited != null) && (tmaxResults.Edited.GetMediaRecord() != null))
				
			//	We're any records updated?
			if((tmaxResults.Updated != null) && (tmaxResults.Updated.Count > 0))
			{
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					try { m_aPanes[i].OnUpdated(tmaxResults.Updated); }
					catch{}
				}
									
				foreach(ITmaxAppNotification I in m_aIAppNotifications)
				{
					try { I.OnUpdated(tmaxResults.Updated); }
					catch {};
				}
				
			}// if((tmaxResults.Updated != null) && (tmaxResults.Updated.Count > 0))
				
			//	Are we supposed to set the selection?
			if((tmaxResults.Selection != null) && (tmaxResults.Selection.GetMediaRecord() != null))
			{
				if(m_paneMedia != null)
					m_paneMedia.Select(tmaxResults.Selection, false, true);
			}
				
			//	We're any records deleted?
			if((tmaxResults.Deleted != null) && (tmaxResults.Deleted.Count > 0))
			{
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					try 
					{	m_aPanes[i].OnDeleted(tmaxResults.Deleted);
						m_aPanes[i].OnClipboardUpdated();
					}
					catch {}
				}
					
				foreach(ITmaxAppNotification I in m_aIAppNotifications)
				{
					try { I.OnDeleted(tmaxResults.Deleted); }
					catch {}
				}
				
			}// if((tmaxResults.Deleted != null) && (tmaxResults.Deleted.Count > 0))

			//	We're any objection records added?
			if((tmaxResults.ObjectionsAdded != null) && (tmaxResults.ObjectionsAdded.Count > 0))
			{
				foreach(CTmaxItem O in tmaxResults.ObjectionsAdded)
				{
					for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
					{
						try	{ m_aPanes[i].OnObjectionsAdded(O, O.SubItems); }
						catch {}
					}

					foreach(ITmaxAppNotification I in m_aIAppNotifications)
					{
						try { I.OnObjectionsAdded(O, O.SubItems); }
						catch {}
					}

				}// foreach(CTmaxItem O in tmaxResults.ObjectionsAdded)

			}// if((tmaxResults.ObjectionsAdded != null) && (tmaxResults.ObjectionsAdded.Count > 0))

			//	We're any objection records updated?
			if((tmaxResults.ObjectionsUpdated != null) && (tmaxResults.ObjectionsUpdated.Count > 0))
			{
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					try { m_aPanes[i].OnObjectionsUpdated(tmaxResults.ObjectionsUpdated); }
					catch { }
				}

				foreach(ITmaxAppNotification I in m_aIAppNotifications)
				{
					try { I.OnObjectionsUpdated(tmaxResults.ObjectionsUpdated); }
					catch { };
				}

			}// if((tmaxResults.ObjectionsUpdated != null) && (tmaxResults.ObjectionsUpdated.Count > 0))

			//	We're any objections deleted?
			if((tmaxResults.ObjectionsDeleted != null) && (tmaxResults.ObjectionsDeleted.Count > 0))
			{
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					try { m_aPanes[i].OnObjectionsDeleted(tmaxResults.ObjectionsDeleted); }
					catch { }
				}

				foreach(ITmaxAppNotification I in m_aIAppNotifications)
				{
					try { I.OnObjectionsDeleted(tmaxResults.ObjectionsDeleted); }
					catch { }
				}

			}// if((tmaxResults.ObjectionsDeleted != null) && (tmaxResults.ObjectionsDeleted.Count > 0))

		}// private void Notify(CTmaxDatabaseResults tmaxResults)

		/// <summary>Called to determine if the parameter for declaring objections has been set</summary>
		/// <param name="tmaxParameters">The parameters passed with the command event arguments</param>
		/// <returns>true if the Objections parameter has been set</returns>
		public bool IsObjectionsCommand(CTmaxParameters tmaxParameters)
		{
			CTmaxParameter tmaxParameter = null;
			bool bObjections = false;

			if(tmaxParameters != null)
			{
				if((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.Objections)) != null)
					bObjections = tmaxParameter.AsBoolean();
			}

			return bObjections;

		}// public bool IsObjectionsCommand(CTmaxParameters tmaxParameters)

		/// <summary>This method is called to start a print job with the specified items</summary>
		/// <param name="tmaxItems">The event items that represent the records to be printed</param>
		private void Print(CTmaxItems tmaxItems)
		{
			try
			{
				//	Do we need to allocate the print manager form?
				if(m_tmaxPrintManager == null)
				{
					m_tmaxPrintManager = new CFPrint();
					
					m_tmaxPrintManager.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.OnDiagnostic);
					m_tmaxPrintManager.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);
					
					m_tmaxPrintManager.TmxPrint = m_tmxPrint;
					m_tmaxPrintManager.PrintOptions = m_tmaxAppOptions.PrintOptions;
					m_tmaxPrintManager.PresentationOptions = m_tmaxPresentationOptions;
					m_tmaxPrintManager.TemplateFilename = m_strPresentationIniFileSpec;
				
					if(m_tmaxPrintManager.Initialize() == false)
					{
						m_tmaxPrintManager.Dispose();
						m_tmaxPrintManager = null;
						return;
					}
					
				}
				
				//	Update the database and item list before showing the form
				m_tmaxPrintManager.Database = m_tmaxDatabase;
				m_tmaxPrintManager.Items = tmaxItems;
				
				//	Show the dialog
				DisableTmaxKeyboard(true);
				m_tmaxPrintManager.ShowDialog();
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Print", Ex);
			}
			finally
			{
				DisableTmaxKeyboard(false);
			}
			
		}// private void Print(CTmaxItems tmaxItems)
				
		/// <summary>Handles TmxEvent events fired by the TmxView control</summary>
		/// <param name="objSender">The child TmxView object</param>
		/// <param name="Args">The event arguments</param>
		private void OnTmxViewEvent(object objSender, FTI.Trialmax.ActiveX.CTmxEventArgs Args)
		{
			try
			{
				//	Pass the event to the active database
				if(m_tmaxDatabase != null)
					m_tmaxDatabase.OnTmxViewEvent(Args);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnTmxViewEvent", Ex);
			}
		
		}// private void OnTmxViewEvent(object objSender, FTI.Trialmax.ActiveX.CTmxEventArgs Args)

		/// <summary>This method is called to prompt the user for the options used to trim the database</summary>
		/// <returns>true to continue, false to cancel</returns>
		private bool GetTrimOptions()
		{
			FTI.Trialmax.Panes.CFTrimOptions wndOptions = null;
			bool bContinue = false;

			try
			{
				wndOptions = new FTI.Trialmax.Panes.CFTrimOptions();

				m_tmaxEventSource.Attach(wndOptions.EventSource);
				wndOptions.Options = m_tmaxDatabase.StationOptions.TrimOptions;
				wndOptions.SourceFolder = m_tmaxDatabase.Folder;
				wndOptions.Database = m_tmaxDatabase;

				if (wndOptions.ShowDialog() == DialogResult.OK)
				{
					bContinue = true;
				}

			}
			catch
			{
			}

			return bContinue;

		}// private bool GetTrimOptions()

		#endregion Private Methods
		
		#region Protected Methods
		
		/// <summary>This method traps events fired when the form is loaded</summary>
		/// <param name="e">Form closing event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Set the instance key to prevent multiple instances of this application
			CTmaxInstanceManager.SetInstanceKey((int)Handle, TmaxApplications.TmaxManager);
			
			m_ctrlSplashScreen.SetMessage("Initializing control states");
			SetControlStates();
			
			//	Install the asynchronous message filter
			if(m_tmaxAsyncFilter != null)
			{
				m_ctrlSplashScreen.SetMessage("Initializing asynchronous messages");
				Application.AddMessageFilter(m_tmaxAsyncFilter);
			}
			
			//	Install the keyboard filter to trap messages
			if(m_tmaxKeyboard != null)
			{
				m_ctrlSplashScreen.SetMessage("Initializing hotkeys");
				Application.AddMessageFilter(m_tmaxKeyboard);
			}
			
			//	Destroy the splash screen
			if(m_ctrlSplashScreen != null)
			{
				m_ctrlSplashScreen.Stop();
				m_ctrlSplashScreen.Dispose();
				m_ctrlSplashScreen = null;
			}
			
			//	Do the base class processing
			base.OnLoad(e);
			
			//	Set the handle to the main window
			m_tmaxAppOptions.AppMainForm = this;
			m_tmaxAppOptions.AppMainWnd  = this.Handle;
			
			//	Notify the user if the updates installer has been updated
			if(m_bInstallerUpdated == true)
			{
				MessageBox.Show("TrialMax detected a new version of the updates installer application. It has been installed on your system.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			
			//	Do we have a command line?
			if(m_tmaxCommandLine != null)
			{
				ProcessCommandLine(m_tmaxCommandLine);
					
			}// if(m_tmaxCommandLine != null)

			//	Should we load the last case?
			//
			//	NOTE:	Check for an open database just in case one
			//			was specified on the command line
			if((m_tmaxDatabase.Primaries == null) && (m_tmaxAppOptions.LoadLastCase == true))
			{
				if(m_tmaxAppOptions.RecentlyUsed.Count > 0)
				{
					OnAppOpenCase(m_tmaxAppOptions.RecentlyUsed[0].ToString(), false, false);
				}
				
			}

            /// <summary>This function called after TmaxManagerForm is created to solve in issue caused by DPI Change</summary>
            /// <summary>in which Media Viewer Pane does not resize until it gets focus or the window is resized</summary>
            this.m_paneViewer.Focus();

		}// protected void OnLoad(System.EventArgs e)
		
		/// <summary>This method traps events fired when the form is closing</summary>
		/// <param name="e">Form closing event arguments</param>
		protected override void OnClosing(CancelEventArgs e)
		{
			//			if(MessageBox.Show("Are You Sure?", "Close", MessageBoxButtons.YesNo) == DialogResult.No)
			//			{
			//				e.Cancel = true;
			//			}
			//			else
			//			{
			Terminate();
			//			}

			//	Should we launch the installer?
			if(m_strXmlUpdateFileSpec.Length > 0)
				LaunchUpdatesInstaller();
		
		}// protected override void OnClosing(CancelEventArgs e)
		
		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
				
				if((m_ctrlPresentationOptions != null) && (m_ctrlPresentationOptions.IsDisposed == false))
					m_ctrlPresentationOptions.Dispose();
					
				//	Make sure the database is closed
				if(m_tmaxDatabase != null)
					m_tmaxDatabase.Close();
					
				//	Flush the clipboard
				if(m_tmaxClipboard != null)
					m_tmaxClipboard.Clear();
					
			}
			base.Dispose( disposing );
		}
		
		/// <summary>This method is called when the application gets activated</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated (e);
			
			//	Are we supposed to notify the panes?
			if(m_bNotifyOnActivate == true)
			{
				for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				{
					if(m_aPanes[i] != null)
					{
						try
						{
							m_aPanes[i].OnApplicationActivated();
						}
						catch
						{
						}
					}
							
				}// for(int i = 0; i < (int)TmaxAppPanes.MaxPanes; i++)
				
				m_bNotifyOnActivate = false;
			}

		}// protected override void OnActivated(EventArgs e)

		/// <summary>Overridden default window procedure</summary>
		/// <param name="m">The message to be processed</param>
		protected override void DefWndProc(ref Message m)
		{
			switch(m.Msg)
			{
				case ((int)(TmaxWindowMessages.InstanceCommandLine)):
				
					OnWMInstanceCommandLine(ref m);
					break;
					
				default:
				
					base.DefWndProc (ref m);
					break;
			}

		}// protected override void DefWndProc(ref Message m)

		#endregion Protected Methods

	}// public class CTmaxManagerForm : System.Windows.Forms.Form

} // namespace FTI.Trialmax.App
