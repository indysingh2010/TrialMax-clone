using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.IO;
using Microsoft.Win32;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinTabControl;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form allows the user to activate the installation</summary>
	public class CFUpdateWizard : System.Windows.Forms.Form
	{
		#region Constants
		
		private const int ERROR_ON_LOAD_PAGE_EX				= 0;
		private const int ERROR_ON_LEAVE_PAGE_EX			= 1;
		private const int ERROR_ON_CLICK_NEXT_EX			= 2;
		private const int ERROR_ON_CLICK_BACK_EX			= 3;
		private const int ERROR_NO_UPDATE_ADDRESS			= 4;
		private const int ERROR_NO_UPDATES_FOLDER			= 5;
		private const int ERROR_CONNECT_UPDATES_SERVER		= 6;
		private const int ERROR_DOWNLOAD_PRODUCT_UPDATE		= 7;
		private const int ERROR_TRANSFER_PROXY_SETTINGS_EX	= 8;
		private const int ERROR_CREATE_LOCAL_LIST_STREAM	= 9;
		private const int ERROR_CREATE_REMOTE_LIST_STREAM	= 10;
		private const int ERROR_TRANSFER_UPDATES_LIST		= 11;
		private const int ERROR_OPEN_PRODUCT_UPDATE_FAILED	= 12;
		private const int ERROR_OPEN_XML_UPDATES_EX			= 13;
		private const int ERROR_NO_LOCAL_FILESPEC			= 14;
		private const int ERROR_NO_REMOTE_URL				= 15;
		private const int ERROR_INVALID_REMOTE_URL			= 16;
		private const int ERROR_CONNECT_DOWNLOAD_SERVER		= 17;
		private const int ERROR_CONNECT_DOWNLOAD_FILE		= 18;
		private const int ERROR_CREATE_DOWNLOAD_STREAM		= 19;
		private const int ERROR_CREATE_LOCAL_STREAM			= 20;		
		private const int ERROR_CHECK_UPDATE_VERSION_EX		= 21;		
		private const int ERROR_SET_CREDENTIAL_EX			= 22;		
		private const int ERROR_CONNECTION_CLOSED			= 23;		
		private const int ERROR_DOWNLOAD_INSTALLATION		= 24;		
		private const int ERROR_SET_PROXY_EX				= 25;
		private const int ERROR_NO_DOWNLOADS				= 26;
		private const int ERROR_NO_DOWNLOADS_SELECTED		= 27;
		private const int ERROR_CHECK_GROUP_ID_EX			= 28;
		private const int ERROR_CHECK_PRODUCT_VERSION_EX	= 29;
		private const int ERROR_CHECK_REG_INSTALLED_EX		= 30;		
	
		private const int WRAP_PROGRESS_MINIMUM = -100;
		private const int WRAP_PROGRESS_STEP = 10;
		
		private const string DEFAULT_UPDATE_FILENAME = "_tmax_updates.xml";
		
		/// <summary>Page identifiers</summary>
		private enum Pages
		{
			Setup = 0,
			Select,
			Download,
		}
		
		#endregion
		
		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>Tab control to manage the wizard pages</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabControl m_ctrlUltraTab;

		/// <summary>Tab control to manage the wizard pages</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage m_ctrlSharedPage;

		/// <summary>Pushbutton to cancel the operation</summary>
		private System.Windows.Forms.Button m_ctrlCancel;

		/// <summary>Pushbutton to go to the next page</summary>
		private System.Windows.Forms.Button m_ctrlNext;
		
		/// <summary>Pushbutton to go back one page</summary>
		private System.Windows.Forms.Button m_ctrlBack;
		
		/// <summary>Shared static text control to display status text for all pages</summary>
		private System.Windows.Forms.Label m_sharedStatus;
		
		/// <summary>Local member bounded to EventSource property</summary>
		protected FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member used to construct error messages</summary>
		protected FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();
		
		/// <summary>Local member bound to Registry property</summary>
		private FTI.Shared.Trialmax.CTmaxRegistry m_tmaxRegistry = null;

		/// <summary>Local member bound to the Product property</summary>
		private FTI.Shared.Trialmax.CTmaxProductManager m_tmaxProductManager = null;

		/// <summary>Local member bound to AppFolder property</summary>
		private string m_strAppFolder = "";

		/// <summary>Property page to set up the operation</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlSetupPage;

		/// <summary>Property page to select updates for installation</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlSelectPage;

		/// <summary>Property page to monitor the download operations</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlDownloadPage;
		
		/// <summary>Static text control to display the introduction text</summary>
		private System.Windows.Forms.Label m_setupIntroduction;

		/// <summary>Proxy server setting group box</summary>
		private System.Windows.Forms.GroupBox m_setupProxyGroup;

		/// <summary>Proxy server password</summary>
		private System.Windows.Forms.TextBox m_setupProxyPassword;

		/// <summary>Property server user name</summary>
		private System.Windows.Forms.TextBox m_setupProxyUser;

		/// <summary>Static text label for proxy server password control</summary>
		private System.Windows.Forms.Label m_setupProxyPasswordLabel;

		/// <summary>Static text label for proxy server user name control</summary>
		private System.Windows.Forms.Label m_setupProxyUserLabel;

		/// <summary>Static text label for proxy server port control</summary>
		private System.Windows.Forms.Label m_setupProxyPortLabel;

		/// <summary>Static text label for proxy server name control</summary>
		private System.Windows.Forms.Label m_setupProxyServerLabel;

		/// <summary>Proxy server port identifier</summary>
		private System.Windows.Forms.TextBox m_setupProxyPort;

		/// <summary>Proxy server name</summary>
		private System.Windows.Forms.TextBox m_setupProxyServer;

		/// <summary>Label for list box of available updates on Select page</summary>
		private System.Windows.Forms.Label m_selectUpdatesLabel;

		/// <summary>List box of available updates on Select page</summary>
		private System.Windows.Forms.ListView m_selectUpdates;

		/// <summary>List box of downloaded updates</summary>
		private System.Windows.Forms.ListView m_downloadUpdates;

		/// <summary>Label for list box of downloaded updates</summary>
		private System.Windows.Forms.Label m_downloadUpdatesLabel;

		/// <summary>Column descriptor for list box of downloaded updates</summary>
		private System.Windows.Forms.ColumnHeader m_downloadColumnDescription;

		/// <summary>Check box to request use of proxy settings for downloads</summary>
		private System.Windows.Forms.CheckBox m_setupProxyIESettings;

		/// <summary>Description column in list box of available updates on Select page</summary>
		private System.Windows.Forms.ColumnHeader m_selectColumnDescription;

		/// <summary>Progress bar on Shared property page</summary>
		private Infragistics.Win.UltraWinProgressBar.UltraProgressBar m_sharedProgress;

		/// <summary>Local member to keep track of the active page</summary>
		private Pages m_eActivePage = Pages.Setup;
		
		/// <summary>Local member to store the active XML product update file</summary>
		private FTI.Shared.Xml.CXmlProductUpdate m_xmlUpdate = null;
		
		/// <summary>Local member to store the path to the folder where downloaded files are stored</summary>
		private string m_strLocalFolder = "";
		
		/// <summary>Local member to store the path to the downloaded product update file</summary>
		private string m_strLocalFileSpec = "";
		
		/// <summary>Local member to store the address of the remote product update file</summary>
		private string m_strRemoteFileSpec = "";
		
		/// <summary>Local member to store the address of the remote folder containing the product update file</summary>
		private string m_strRemoteFolderSpec = "";
		
		/// <summary>Local member to store the address of the server where the product update is stored</summary>
		private string m_strRemoteServer = "";
		
		/// <summary>Local member to store the name of the product update file</summary>
		private string m_strUpdateFilename = "";
		
		/// <summary>Local member bound to InstallFileSpec property</summary>
		private string m_strInstallFileSpec = "";
		
		/// <summary>Local member to store the address of the primary update site</summary>
		private string m_strPrimaryAddress = "";
		
		/// <summary>Local member to store the path to the updates folder</summary>
		private string m_strAlternateAddress = "";

		/// <summary>Local member to store the default group identifier</summary>
		private string m_strDefaultGroupId = "";

		/// <summary>Local member to store the active web request</summary>
		private System.Net.WebRequest m_webRequest = null;
		
		/// <summary>Local member to store the active web response</summary>
		private System.Net.WebResponse m_webResponse = null;
		
		/// <summary>Local flag to indicate if the request has been cancelled</summary>
		private bool m_bCancelled = false;
		
		/// <summary>Local member to keep track of Minimum progress value</summary>
		private long m_lMinProgress = 0;
		
		/// <summary>Local member to keep track of Maximum progress value</summary>
		private long m_lMaxProgress = 0;
		
		/// <summary>Local member to keep track of Current progress value</summary>
		private long m_lProgress = 0;
		
		/// <summary>Local member to keep track of elapsed time during a download</summary>
		private System.DateTime m_dtLastTime = System.DateTime.Now;
		
		/// <summary>Local member to keep track of time out period in milliseconds</summary>
		private long m_lTimeOut = 0;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFUpdateWizard()
		{
			//	Initialize the child controls
			InitializeComponent();
			
			//	Populate the error builder
			SetErrorStrings();
		
		}// public CFUpdateWizard()

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(components != null)
				{
					components.Dispose();
				}
				
				//	Make sure the XML is closed
				CloseXmlUpdate();
				
				//	Make sure we are disconnected
				Disconnect();
				
			}
			base.Dispose(disposing);
		
		}// protected override void Dispose(bool disposing)

		/// <summary>Overloaded member called when form window is created</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			bool bSuccessful = false;
			
			if((m_tmaxRegistry != null) && (m_tmaxProductManager != null))
			{
				//	Set the default group identifier
				SetDefaultGroupId();
				
				//	Set the static proxy information
				TransferProxySettings(true);

				//	Set up the operation
				while(bSuccessful == false)
				{
					//	Set the site addresses
					if(SetServerAddresses() == false)
						break;
								
					//	Set the local updates folder
					if(SetLocalFolder() == false)
						break;
								
					bSuccessful = true;
				
				}// while(bSuccessful == false)
						
			}
			
			if(bSuccessful == true)
			{	
				//	Set the static introduction
				m_setupIntroduction.Text  = "TrialMax will use the Internet to search for updates ";
				m_setupIntroduction.Text += "to your installed applications and components. ";
				m_setupIntroduction.Text += "You must have an active Internet connection.";
			
				//	Initialize the Setup page
				OnLoadPage(Pages.Setup);
			}
			else
			{
				//	Set the static introduction
				m_setupIntroduction.Text  = "Unable to initialize the TrialMax updates operation.";
			
				//	Only allow the user to cancel the operation
				m_ctrlNext.Enabled = false;
				m_ctrlBack.Enabled = false;
			}
			
			base.OnLoad (e);
		
		}// protected override void OnLoad(EventArgs e)

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFUpdateWizard));
			this.m_ctrlSetupPage = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.m_setupProxyGroup = new System.Windows.Forms.GroupBox();
			this.m_setupProxyIESettings = new System.Windows.Forms.CheckBox();
			this.m_setupProxyPassword = new System.Windows.Forms.TextBox();
			this.m_setupProxyUser = new System.Windows.Forms.TextBox();
			this.m_setupProxyPasswordLabel = new System.Windows.Forms.Label();
			this.m_setupProxyUserLabel = new System.Windows.Forms.Label();
			this.m_setupProxyPortLabel = new System.Windows.Forms.Label();
			this.m_setupProxyServerLabel = new System.Windows.Forms.Label();
			this.m_setupProxyPort = new System.Windows.Forms.TextBox();
			this.m_setupProxyServer = new System.Windows.Forms.TextBox();
			this.m_setupIntroduction = new System.Windows.Forms.Label();
			this.m_sharedProgress = new Infragistics.Win.UltraWinProgressBar.UltraProgressBar();
			this.m_sharedStatus = new System.Windows.Forms.Label();
			this.m_ctrlBack = new System.Windows.Forms.Button();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlNext = new System.Windows.Forms.Button();
			this.m_ctrlSelectPage = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.m_selectUpdates = new System.Windows.Forms.ListView();
			this.m_selectColumnDescription = new System.Windows.Forms.ColumnHeader();
			this.m_selectUpdatesLabel = new System.Windows.Forms.Label();
			this.m_ctrlDownloadPage = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.m_downloadUpdates = new System.Windows.Forms.ListView();
			this.m_downloadColumnDescription = new System.Windows.Forms.ColumnHeader();
			this.m_downloadUpdatesLabel = new System.Windows.Forms.Label();
			this.m_ctrlUltraTab = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
			this.m_ctrlSharedPage = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
			this.m_ctrlSetupPage.SuspendLayout();
			this.m_setupProxyGroup.SuspendLayout();
			this.m_ctrlSelectPage.SuspendLayout();
			this.m_ctrlDownloadPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraTab)).BeginInit();
			this.m_ctrlUltraTab.SuspendLayout();
			this.m_ctrlSharedPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlSetupPage
			// 
			this.m_ctrlSetupPage.Controls.Add(this.m_setupProxyGroup);
			this.m_ctrlSetupPage.Controls.Add(this.m_setupIntroduction);
			this.m_ctrlSetupPage.Controls.Add(this.m_sharedProgress);
			this.m_ctrlSetupPage.Controls.Add(this.m_sharedStatus);
			this.m_ctrlSetupPage.Controls.Add(this.m_ctrlBack);
			this.m_ctrlSetupPage.Controls.Add(this.m_ctrlCancel);
			this.m_ctrlSetupPage.Controls.Add(this.m_ctrlNext);
			this.m_ctrlSetupPage.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlSetupPage.Name = "m_ctrlSetupPage";
			this.m_ctrlSetupPage.Size = new System.Drawing.Size(484, 277);
			// 
			// m_setupProxyGroup
			// 
			this.m_setupProxyGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_setupProxyGroup.Controls.Add(this.m_setupProxyIESettings);
			this.m_setupProxyGroup.Controls.Add(this.m_setupProxyPassword);
			this.m_setupProxyGroup.Controls.Add(this.m_setupProxyUser);
			this.m_setupProxyGroup.Controls.Add(this.m_setupProxyPasswordLabel);
			this.m_setupProxyGroup.Controls.Add(this.m_setupProxyUserLabel);
			this.m_setupProxyGroup.Controls.Add(this.m_setupProxyPortLabel);
			this.m_setupProxyGroup.Controls.Add(this.m_setupProxyServerLabel);
			this.m_setupProxyGroup.Controls.Add(this.m_setupProxyPort);
			this.m_setupProxyGroup.Controls.Add(this.m_setupProxyServer);
			this.m_setupProxyGroup.Location = new System.Drawing.Point(8, 60);
			this.m_setupProxyGroup.Name = "m_setupProxyGroup";
			this.m_setupProxyGroup.Size = new System.Drawing.Size(468, 132);
			this.m_setupProxyGroup.TabIndex = 10;
			this.m_setupProxyGroup.TabStop = false;
			this.m_setupProxyGroup.Text = "Proxy Settings";
			// 
			// m_setupProxyIESettings
			// 
			this.m_setupProxyIESettings.Location = new System.Drawing.Point(8, 104);
			this.m_setupProxyIESettings.Name = "m_setupProxyIESettings";
			this.m_setupProxyIESettings.Size = new System.Drawing.Size(312, 20);
			this.m_setupProxyIESettings.TabIndex = 4;
			this.m_setupProxyIESettings.Text = "Use default browser settings";
			this.m_setupProxyIESettings.CheckedChanged += new System.EventHandler(this.OnSetupIESettingsChanged);
			// 
			// m_setupProxyPassword
			// 
			this.m_setupProxyPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_setupProxyPassword.Location = new System.Drawing.Point(72, 80);
			this.m_setupProxyPassword.Name = "m_setupProxyPassword";
			this.m_setupProxyPassword.Size = new System.Drawing.Size(388, 20);
			this.m_setupProxyPassword.TabIndex = 3;
			this.m_setupProxyPassword.Text = "";
			this.m_setupProxyPassword.WordWrap = false;
			// 
			// m_setupProxyUser
			// 
			this.m_setupProxyUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_setupProxyUser.Location = new System.Drawing.Point(72, 52);
			this.m_setupProxyUser.Name = "m_setupProxyUser";
			this.m_setupProxyUser.Size = new System.Drawing.Size(388, 20);
			this.m_setupProxyUser.TabIndex = 2;
			this.m_setupProxyUser.Text = "";
			// 
			// m_setupProxyPasswordLabel
			// 
			this.m_setupProxyPasswordLabel.Location = new System.Drawing.Point(8, 80);
			this.m_setupProxyPasswordLabel.Name = "m_setupProxyPasswordLabel";
			this.m_setupProxyPasswordLabel.Size = new System.Drawing.Size(64, 20);
			this.m_setupProxyPasswordLabel.TabIndex = 23;
			this.m_setupProxyPasswordLabel.Text = "Password:";
			this.m_setupProxyPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_setupProxyUserLabel
			// 
			this.m_setupProxyUserLabel.Location = new System.Drawing.Point(8, 52);
			this.m_setupProxyUserLabel.Name = "m_setupProxyUserLabel";
			this.m_setupProxyUserLabel.Size = new System.Drawing.Size(64, 20);
			this.m_setupProxyUserLabel.TabIndex = 22;
			this.m_setupProxyUserLabel.Text = "User Name:";
			this.m_setupProxyUserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_setupProxyPortLabel
			// 
			this.m_setupProxyPortLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_setupProxyPortLabel.Location = new System.Drawing.Point(380, 24);
			this.m_setupProxyPortLabel.Name = "m_setupProxyPortLabel";
			this.m_setupProxyPortLabel.Size = new System.Drawing.Size(36, 20);
			this.m_setupProxyPortLabel.TabIndex = 1;
			this.m_setupProxyPortLabel.Text = "Port:";
			this.m_setupProxyPortLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_setupProxyServerLabel
			// 
			this.m_setupProxyServerLabel.Location = new System.Drawing.Point(8, 24);
			this.m_setupProxyServerLabel.Name = "m_setupProxyServerLabel";
			this.m_setupProxyServerLabel.Size = new System.Drawing.Size(64, 20);
			this.m_setupProxyServerLabel.TabIndex = 20;
			this.m_setupProxyServerLabel.Text = "Server:";
			this.m_setupProxyServerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_setupProxyPort
			// 
			this.m_setupProxyPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_setupProxyPort.Location = new System.Drawing.Point(420, 24);
			this.m_setupProxyPort.Name = "m_setupProxyPort";
			this.m_setupProxyPort.Size = new System.Drawing.Size(40, 20);
			this.m_setupProxyPort.TabIndex = 19;
			this.m_setupProxyPort.Text = "";
			// 
			// m_setupProxyServer
			// 
			this.m_setupProxyServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_setupProxyServer.Location = new System.Drawing.Point(72, 24);
			this.m_setupProxyServer.Name = "m_setupProxyServer";
			this.m_setupProxyServer.Size = new System.Drawing.Size(304, 20);
			this.m_setupProxyServer.TabIndex = 0;
			this.m_setupProxyServer.Text = "";
			// 
			// m_setupIntroduction
			// 
			this.m_setupIntroduction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_setupIntroduction.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_setupIntroduction.Location = new System.Drawing.Point(8, 12);
			this.m_setupIntroduction.Name = "m_setupIntroduction";
			this.m_setupIntroduction.Size = new System.Drawing.Size(468, 44);
			this.m_setupIntroduction.TabIndex = 9;
			this.m_setupIntroduction.Text = "Introduction";
			// 
			// m_sharedProgress
			// 
			this.m_sharedProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_sharedProgress.BorderStyle = Infragistics.Win.UIElementBorderStyle.InsetSoft;
			appearance1.BackColor = System.Drawing.SystemColors.Window;
			appearance1.BackColor2 = System.Drawing.SystemColors.Highlight;
			appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
			this.m_sharedProgress.FillAppearance = appearance1;
			this.m_sharedProgress.Location = new System.Drawing.Point(8, 196);
			this.m_sharedProgress.Name = "m_sharedProgress";
			this.m_sharedProgress.Size = new System.Drawing.Size(468, 16);
			this.m_sharedProgress.TabIndex = 9;
			this.m_sharedProgress.Text = "[Formatted]";
			this.m_sharedProgress.TextVisible = false;
			// 
			// m_sharedStatus
			// 
			this.m_sharedStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_sharedStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_sharedStatus.Location = new System.Drawing.Point(8, 216);
			this.m_sharedStatus.Name = "m_sharedStatus";
			this.m_sharedStatus.Size = new System.Drawing.Size(468, 24);
			this.m_sharedStatus.TabIndex = 8;
			this.m_sharedStatus.Text = "Status Text Goes Here";
			this.m_sharedStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// m_ctrlBack
			// 
			this.m_ctrlBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlBack.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.m_ctrlBack.ImageIndex = 1;
			this.m_ctrlBack.Location = new System.Drawing.Point(220, 247);
			this.m_ctrlBack.Name = "m_ctrlBack";
			this.m_ctrlBack.Size = new System.Drawing.Size(80, 23);
			this.m_ctrlBack.TabIndex = 7;
			this.m_ctrlBack.Text = "< &Back";
			this.m_ctrlBack.Click += new System.EventHandler(this.OnClickBack);
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(396, 247);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.Size = new System.Drawing.Size(80, 23);
			this.m_ctrlCancel.TabIndex = 1;
			this.m_ctrlCancel.Text = "  &Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlNext
			// 
			this.m_ctrlNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlNext.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.m_ctrlNext.ImageIndex = 1;
			this.m_ctrlNext.Location = new System.Drawing.Point(300, 247);
			this.m_ctrlNext.Name = "m_ctrlNext";
			this.m_ctrlNext.Size = new System.Drawing.Size(80, 23);
			this.m_ctrlNext.TabIndex = 0;
			this.m_ctrlNext.Text = "&Next >";
			this.m_ctrlNext.Click += new System.EventHandler(this.OnClickNext);
			// 
			// m_ctrlSelectPage
			// 
			this.m_ctrlSelectPage.Controls.Add(this.m_selectUpdates);
			this.m_ctrlSelectPage.Controls.Add(this.m_selectUpdatesLabel);
			this.m_ctrlSelectPage.Location = new System.Drawing.Point(-10000, -10000);
			this.m_ctrlSelectPage.Name = "m_ctrlSelectPage";
			this.m_ctrlSelectPage.Size = new System.Drawing.Size(484, 277);
			// 
			// m_selectUpdates
			// 
			this.m_selectUpdates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_selectUpdates.CheckBoxes = true;
			this.m_selectUpdates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							  this.m_selectColumnDescription});
			this.m_selectUpdates.FullRowSelect = true;
			this.m_selectUpdates.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.m_selectUpdates.Location = new System.Drawing.Point(8, 28);
			this.m_selectUpdates.Name = "m_selectUpdates";
			this.m_selectUpdates.Size = new System.Drawing.Size(468, 164);
			this.m_selectUpdates.TabIndex = 11;
			this.m_selectUpdates.View = System.Windows.Forms.View.Details;
			this.m_selectUpdates.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.OnUpdateItemCheck);
			// 
			// m_selectColumnDescription
			// 
			this.m_selectColumnDescription.Text = "Description";
			this.m_selectColumnDescription.Width = 65;
			// 
			// m_selectUpdatesLabel
			// 
			this.m_selectUpdatesLabel.Location = new System.Drawing.Point(8, 12);
			this.m_selectUpdatesLabel.Name = "m_selectUpdatesLabel";
			this.m_selectUpdatesLabel.Size = new System.Drawing.Size(152, 16);
			this.m_selectUpdatesLabel.TabIndex = 10;
			this.m_selectUpdatesLabel.Text = "Available Updates";
			this.m_selectUpdatesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlDownloadPage
			// 
			this.m_ctrlDownloadPage.Controls.Add(this.m_downloadUpdates);
			this.m_ctrlDownloadPage.Controls.Add(this.m_downloadUpdatesLabel);
			this.m_ctrlDownloadPage.Location = new System.Drawing.Point(-10000, -10000);
			this.m_ctrlDownloadPage.Name = "m_ctrlDownloadPage";
			this.m_ctrlDownloadPage.Size = new System.Drawing.Size(484, 277);
			// 
			// m_downloadUpdates
			// 
			this.m_downloadUpdates.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_downloadUpdates.BackColor = System.Drawing.SystemColors.Window;
			this.m_downloadUpdates.CheckBoxes = true;
			this.m_downloadUpdates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																								this.m_downloadColumnDescription});
			this.m_downloadUpdates.FullRowSelect = true;
			this.m_downloadUpdates.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.m_downloadUpdates.Location = new System.Drawing.Point(8, 28);
			this.m_downloadUpdates.Name = "m_downloadUpdates";
			this.m_downloadUpdates.Size = new System.Drawing.Size(468, 164);
			this.m_downloadUpdates.TabIndex = 13;
			this.m_downloadUpdates.View = System.Windows.Forms.View.Details;
			this.m_downloadUpdates.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.OnUpdateItemCheck);
			// 
			// m_downloadColumnDescription
			// 
			this.m_downloadColumnDescription.Text = "Description";
			this.m_downloadColumnDescription.Width = 272;
			// 
			// m_downloadUpdatesLabel
			// 
			this.m_downloadUpdatesLabel.Location = new System.Drawing.Point(8, 12);
			this.m_downloadUpdatesLabel.Name = "m_downloadUpdatesLabel";
			this.m_downloadUpdatesLabel.Size = new System.Drawing.Size(152, 16);
			this.m_downloadUpdatesLabel.TabIndex = 12;
			this.m_downloadUpdatesLabel.Text = "Available Updates";
			this.m_downloadUpdatesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlUltraTab
			// 
			this.m_ctrlUltraTab.Controls.Add(this.m_ctrlSharedPage);
			this.m_ctrlUltraTab.Controls.Add(this.m_ctrlSelectPage);
			this.m_ctrlUltraTab.Controls.Add(this.m_ctrlSetupPage);
			this.m_ctrlUltraTab.Controls.Add(this.m_ctrlDownloadPage);
			this.m_ctrlUltraTab.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlUltraTab.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlUltraTab.Name = "m_ctrlUltraTab";
			this.m_ctrlUltraTab.SharedControls.AddRange(new System.Windows.Forms.Control[] {
																							   this.m_sharedProgress,
																							   this.m_sharedStatus,
																							   this.m_ctrlBack,
																							   this.m_ctrlCancel,
																							   this.m_ctrlNext});
			this.m_ctrlUltraTab.SharedControlsPage = this.m_ctrlSharedPage;
			this.m_ctrlUltraTab.Size = new System.Drawing.Size(484, 277);
			this.m_ctrlUltraTab.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Wizard;
			this.m_ctrlUltraTab.TabIndex = 0;
			ultraTab1.TabPage = this.m_ctrlSetupPage;
			ultraTab1.Text = "Setup";
			ultraTab2.TabPage = this.m_ctrlSelectPage;
			ultraTab2.Text = "Select";
			ultraTab3.TabPage = this.m_ctrlDownloadPage;
			ultraTab3.Text = "Download";
			this.m_ctrlUltraTab.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
																									 ultraTab1,
																									 ultraTab2,
																									 ultraTab3});
			// 
			// m_ctrlSharedPage
			// 
			this.m_ctrlSharedPage.Controls.Add(this.m_sharedProgress);
			this.m_ctrlSharedPage.Controls.Add(this.m_sharedStatus);
			this.m_ctrlSharedPage.Controls.Add(this.m_ctrlBack);
			this.m_ctrlSharedPage.Controls.Add(this.m_ctrlCancel);
			this.m_ctrlSharedPage.Controls.Add(this.m_ctrlNext);
			this.m_ctrlSharedPage.Location = new System.Drawing.Point(-10000, -10000);
			this.m_ctrlSharedPage.Name = "m_ctrlSharedPage";
			this.m_ctrlSharedPage.Size = new System.Drawing.Size(484, 277);
			// 
			// CFUpdateWizard
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(484, 277);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlUltraTab);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFUpdateWizard";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "TrialMax Update Wizard";
			this.m_ctrlSetupPage.ResumeLayout(false);
			this.m_setupProxyGroup.ResumeLayout(false);
			this.m_ctrlSelectPage.ResumeLayout(false);
			this.m_ctrlDownloadPage.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraTab)).EndInit();
			this.m_ctrlUltraTab.ResumeLayout(false);
			this.m_ctrlSharedPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}// private void InitializeComponent()
		
		/// <summary>This method handles the Next button's Click event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickNext(object sender, System.EventArgs e)
		{
			//	Make sure it's OK to leave the active page
			if(OnLeavePage(m_eActivePage) == false) return;
			
			try
			{
				//	Which page is active
				switch(m_eActivePage)
				{
					case Pages.Setup:

						m_ctrlUltraTab.PerformAction(UltraTabControlAction.SelectNextTab);
						m_eActivePage = Pages.Select;
			
						//	Make sure the child controls are initialized
						OnLoadPage(m_eActivePage);
						break;
						
					case Pages.Select:
					
						m_ctrlUltraTab.PerformAction(UltraTabControlAction.SelectNextTab);
						m_eActivePage = Pages.Download;
			
						//	Make sure the child controls are initialized
						OnLoadPage(m_eActivePage);
						break;
						
					case Pages.Download:
					
						//	Close the form
						this.DialogResult = DialogResult.OK;
						this.Close();
						
						break;
						
				}// switch(m_eActivePage)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnClickNext", m_tmaxErrorBuilder.Message(ERROR_ON_CLICK_NEXT_EX, m_eActivePage), Ex);
			}
		
		}// private void OnClickNext(object sender, System.EventArgs e)
		
		/// <summary>This method handles the Back button's Click event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickBack(object sender, System.EventArgs e)
		{
			try
			{
				//	Whick page is active
				switch(m_eActivePage)
				{
					case Pages.Setup:
					
						break;
						
					case Pages.Select:
					
						m_ctrlUltraTab.PerformAction(UltraTabControlAction.SelectPreviousTab);
						m_eActivePage = Pages.Setup;
						break;
						
					case Pages.Download:
					
						m_ctrlUltraTab.PerformAction(UltraTabControlAction.SelectPreviousTab);
						m_eActivePage = Pages.Select;
						break;
						
				}// switch(m_eActivePage)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnClickBack", m_tmaxErrorBuilder.Message(ERROR_ON_CLICK_BACK_EX, m_eActivePage), Ex);
			}
			
			//	Notify the new page
			OnLoadPage(m_eActivePage);
		
		}// private void OnClickBack(object sender, System.EventArgs e)
		
		/// <summary>This method is called when the specified page is being loaded</summary>
		/// <param name="ePage">The page being loaded</param>
		private void OnLoadPage(Pages ePage)
		{
			try
			{
				//	Whick page is being loaded?
				switch(ePage)
				{
					case Pages.Setup:
					
						m_ctrlBack.Enabled = false;
						m_ctrlNext.Enabled = true;
						m_sharedProgress.Visible = false;
						SetSharedStatus("Click Next to start checking for updates");
						break;
						
					case Pages.Select:
					
						SetSharedStatus("Select updates and click Next to download");
						m_ctrlNext.Enabled = (GetCheckedCount(m_selectUpdates) > 0);
						m_ctrlBack.Enabled = true;
						m_sharedProgress.Visible = false;
						break;
						
					case Pages.Download:
					
						SetSharedStatus("Downloading selected updates");
						m_ctrlNext.Enabled = false;
						m_ctrlBack.Enabled = false;
						
						//	Clear the list of downloaded updates
						m_downloadUpdates.Items.Clear();
						
						//	Start downloading the selections
						DownloadSelections();
						
						break;
						
				}// switch(ePage)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnLoadPage", m_tmaxErrorBuilder.Message(ERROR_ON_LOAD_PAGE_EX, ePage), Ex);
			}
		
		}// private void OnLoadPage(Pages ePage)
		
		/// <summary>This method is called when leaving the specified page</summary>
		/// <param name="ePage">The page being deactivated</param>
		/// <returns>true if ok to leave the specified page</returns>
		private bool OnLeavePage(Pages ePage)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Whick page is being deactivated?
				switch(ePage)
				{
					case Pages.Setup:
					
						while(bSuccessful == false)
						{
							//	Get the proxy settings from the user
							TransferProxySettings(false);
							
							//	Download the list of available updates
							if(DownloadXmlUpdate() == false)
								break;

							//	Open the XML list of updates
							if(OpenXmlUpdate() == false)
								break;
								
							//	Populate the list of available updates
							if(FillAvailableUpdates() == false)
								break;
								
							//	All's good.....
							bSuccessful = true;
							
						}// while(bSuccessful == false)
						
						//	Make sure the Next button is enabled to allow the user to try again
						m_ctrlNext.Enabled = true;
						
						if(bSuccessful == false)
							m_sharedProgress.Visible = false;
						break;
						
					case Pages.Select:

						while(bSuccessful == false)
						{
							//	Get the user selections
							if(GetSelectedUpdates(m_selectUpdates) <= 0)
								break;

							//	All's good.....
							bSuccessful = true;
							
						}// while(bSuccessful == false)
						
						break;
						
					case Pages.Download:
					
						bSuccessful = Install();
						break;
						
				}// switch(ePage)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnLeavePage", m_tmaxErrorBuilder.Message(ERROR_ON_LEAVE_PAGE_EX, ePage), Ex);
			}
			
			return bSuccessful;
		
		}// private void OnLeavePage(Pages ePage)
		
		/// <summary>This method is called to determine how many items in the list view are checked</summary>
		/// <param name="listView">The parent list view control</param>
		/// <returns>the number of checked items</returns>
		private int GetCheckedCount(System.Windows.Forms.ListView listView)
		{
			int iChecked = 0;
			
			try
			{
				if((listView != null) && (listView.IsDisposed == false))
				{
					if((listView.Items != null) && (listView.Items.Count > 0))
					{
						//	Get the selected updates
						foreach(ListViewItem O in listView.Items)
						{
							try
							{
								if(O.Checked == true) 
									iChecked++;
							}
							catch
							{
							}
						
						}// foreach(ListViewItem O in m_selectUpdates.Items)
					
					}// if((listView.Items != null) && (listView.Items.Count > 0))
					
				}// if((listView != null) && (listView.IsDisposed == false))
			}
			catch
			{
			}
			
			return iChecked;
		
		}// private int GetCheckedCount(System.Windows.Forms.ListView listView)
		
		/// <summary>This method is called to set the local folder used to perform the updates</summary>
		/// <returns>True if successful</returns>
		private bool SetLocalFolder()
		{
			//	Use the executing assembly if no value has been assigned to the AppFolder property
			if(m_strAppFolder.Length == 0)
				m_strAppFolder = CTmaxToolbox.GetApplicationFolder();
			
			m_strLocalFolder = "";
			m_strLocalFileSpec = "";
			
			//	Construct the path to the local folder where the files will be downloaded
			m_strLocalFolder = m_strAppFolder;
			if(m_strLocalFolder.EndsWith("\\") == false)
				m_strLocalFolder += "\\";
			m_strLocalFolder += "_tmax_updates";
			
			//	Make sure the folder exists
			if(System.IO.Directory.Exists(m_strLocalFolder) == false)
			{
				try
				{
					System.IO.Directory.CreateDirectory(m_strLocalFolder);
				}
				catch
				{
					OnLocalError(m_tmaxErrorBuilder.Message(ERROR_NO_UPDATES_FOLDER, m_strLocalFolder));
					return false;
				}
			
			}// if(System.IO.Directory.Exists(m_strLocalFolder) == false)
			
			m_strLocalFileSpec = (m_strLocalFolder + "\\" + DEFAULT_UPDATE_FILENAME);
			return true;

		}// private bool SetLocalFolder()

		/// <summary>This method is called to set the primary and alternate server addresses</summary>
		/// <returns>True if successful</returns>
		private bool SetServerAddresses()
		{
			if(m_tmaxProductManager == null) return false;
			
			m_strPrimaryAddress = "";
			m_strAlternateAddress = "";
			m_strUpdateFilename = "";
			
			//	Do we have a primary address in the manager options?
			if(m_tmaxProductManager.UpdateSite.Length > 0)
			{
				m_strPrimaryAddress = m_tmaxProductManager.UpdateSite;
				m_strAlternateAddress = m_tmaxProductManager.UpdateAlternateSite;
			}
			else
			{
				m_strPrimaryAddress = m_tmaxProductManager.UpdateAlternateSite;
				m_strAlternateAddress = "";
			}
			
			//	We must have a primary address
			if(m_strPrimaryAddress.Length == 0)
			{
				OnLocalError(m_tmaxErrorBuilder.Message(ERROR_NO_UPDATE_ADDRESS));
				return false;
			}
			else
			{
				//	Get the name of the product update file
				if(m_tmaxProductManager.UpdateFilename.Length == 0)
					m_strUpdateFilename = DEFAULT_UPDATE_FILENAME;
				else
					m_strUpdateFilename = m_tmaxProductManager.UpdateFilename;
					
				return true;
			}

		}// private bool SetServerAddresses()

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to load the %1 page.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to leave the %1 page.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to go to the next page: active = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to go to the previous page: active = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the address for the TrialMax update site in the application configuration file.");
			
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to create the folder for the TrialMax updates: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to connect to the TrialMax updates site: %1\n\nMake sure you have an active Internet connection and try again.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to retrieve the updates list from the TrialMax updates site: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to transfer the proxy settings %1 the application object.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to create the local file stream for the updates list: %1");
			
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to create the remote file stream for the updates list: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An error occurred while transferring the updates list to %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to open the XML product update file: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to process the XML product update file: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to construct the local file specification for the update: %1");
			
			m_tmaxErrorBuilder.FormatStrings.Add("No URL specified for %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An invalid URL specified for %1: url = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to connect to %1 to download %2");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate %1 to download %2");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to create remote stream for %1 to download %2");
			
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to create the local file to download %1 : filename = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while checking the version information for the update: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while assigning the request authentication: url = %1  username = %2  password = %3");
			m_tmaxErrorBuilder.FormatStrings.Add("The connection to %1 was closed before completing the download. %2 bytes of %3 were downloaded.");
			m_tmaxErrorBuilder.FormatStrings.Add("An error was encountered while downloading %1 from %2");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while assigning the proxy settings: url = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("No updates have been downloaded for installation");
			m_tmaxErrorBuilder.FormatStrings.Add("You must select one or more of the downloaded updates to install");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while checking the group identifier for the update: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while checking the minimum product version for the update: %1");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while checking the registry installation key for the update: %1");

		}// private void SetErrorStrings()

		/// <summary>This method is called to connect to the product update server</summary>
		/// <returns>true if successful</returns>
		private bool ConnectXmlServer()
		{
			bool	bSuccessful = false;
			int		iIndex = 0;
			
			Debug.Assert(m_strPrimaryAddress.Length > 0);
			if(m_strPrimaryAddress.Length == 0) return false;
			
			//	Start the operation
			m_strRemoteServer = "";
			m_strRemoteFolderSpec = "";
			m_strRemoteFileSpec = "";
			
			SetProgressUpdates("Locating product update server ...", -1);
			
			//	Attempt to connect to the primary site
			if(Connect(m_strPrimaryAddress, m_tmaxProductManager.UpdateUserName, m_tmaxProductManager.UpdatePassword) == false)
			{
				//	Try the alternate
				if(m_strAlternateAddress.Length > 0)
				{
					if(Connect(m_strAlternateAddress, m_tmaxProductManager.UpdateUserName, m_tmaxProductManager.UpdatePassword) == true)
						m_strRemoteServer = m_strAlternateAddress;
				}
				
			}
			else
			{
				//	Use the primary address
				m_strRemoteServer = m_strPrimaryAddress;
			}
			
			//	Did we fail to establish a connection to the remote server?
			if(m_strRemoteServer.Length == 0)
			{
				OnLocalError(m_tmaxErrorBuilder.Message(ERROR_CONNECT_UPDATES_SERVER, m_strPrimaryAddress));
				StopProgressUpdates("Failed to locate the update server", false);
			}
			else
			{
				if((m_strRemoteServer.EndsWith("/") == false) && (m_strRemoteServer.EndsWith("\\") == false))
					m_strRemoteServer += "/";
					
				//	Construct the address to the product update file
				m_strRemoteFileSpec = m_strRemoteServer;
				if((m_strUpdateFilename.StartsWith("/") == false) && (m_strUpdateFilename.StartsWith("\\") == false))
					m_strRemoteFileSpec = (m_strRemoteServer + m_strUpdateFilename);
				else
					m_strRemoteFileSpec += (m_strRemoteServer + m_strUpdateFilename.Substring(1, m_strUpdateFilename.Length -1));
				
				//	Get the address to the remote folder containing the product update
				//
				//	NOTE:	This is done AFTER constructing the address to the file because the server address
				//			could include a relative path or the update filename could include a relative path
				m_strRemoteFolderSpec = m_strRemoteFileSpec;
				if((iIndex = m_strRemoteFolderSpec.IndexOf("?")) > 0) // Strip the parameters first
					m_strRemoteFolderSpec = m_strRemoteFolderSpec.Substring(0, iIndex);
				if((iIndex = m_strRemoteFolderSpec.LastIndexOfAny("\\/".ToCharArray())) > 0)
					m_strRemoteFolderSpec = m_strRemoteFolderSpec.Substring(0, iIndex + 1);
					
				bSuccessful = true;
			}
			
			//	Clean up
			Disconnect();
			
			return bSuccessful;
			
		}// private bool ConnectXmlServer()

		/// <summary>This method is called to download the XML product update file from the server</summary>
		/// <returns>true if successful</returns>
		private bool DownloadXmlUpdate()
		{
			Stream	streamLocal = null;
			Stream	streamRemote = null;
			byte []	aTransferBytes = new byte[1024];
			bool	bSuccessful = false;
			bool	bContinue = true;
			int		iBytesRead = 0;
			long	lTotalBytes = 0;
			string	strLocalError = "";
			string	strStatusMsg = "";
			
			//	Make sure we have a valid filename for the product update
			Debug.Assert(m_tmaxProductManager.UpdateFilename.Length > 0);
			if(m_tmaxProductManager.UpdateFilename.Length == 0)
				m_tmaxProductManager.UpdateFilename = DEFAULT_UPDATE_FILENAME;
			
			//	Disable the Back and Next buttons while we download
			//
			//	NOTE:	The caller will reset when this method returns
			this.m_ctrlNext.Enabled = false;
			this.m_ctrlBack.Enabled = false;
			
			//	Initialize the progress bar
			StartProgressUpdates("Connecting to updates site", -1);
			
			//	Verify the ability to connect to the server
			if(ConnectXmlServer() == false) return false;
			
			Debug.Assert(m_strRemoteFileSpec.Length > 0);
			if(m_strRemoteFileSpec.Length == 0) return false;
			
			//	Download the updates list
			while(bContinue == true)
			{
				//	Send the request to download the XML product update file
				if(GetCancelled("Requesting product update list") == true) 
					break;
				
				if(Connect(m_strRemoteFileSpec, m_tmaxProductManager.UpdateUserName, m_tmaxProductManager.UpdatePassword) == false)
				{
					strLocalError = m_tmaxErrorBuilder.Message(ERROR_DOWNLOAD_PRODUCT_UPDATE, m_strRemoteFileSpec);
					strStatusMsg = "Product update list not available";
					break;
				}

				//	Reset the progress bar to display byte percentage
				if(m_webResponse.ContentLength > 0)
					StartProgressUpdates(null, m_webResponse.ContentLength);
					
				if(GetCancelled("creating product update file") == true) 
					break;
				
				//	Create the local file stream
				try
				{
					streamLocal = System.IO.File.Create(m_strLocalFileSpec);
				}
				catch
				{
					strLocalError = m_tmaxErrorBuilder.Message(ERROR_CREATE_LOCAL_LIST_STREAM, m_strLocalFileSpec);
					strStatusMsg = "Unable to create product update file";
					break;
				}
				
				try
				{
					if(GetCancelled("downloading updates list") == true) break;

					m_tmaxEventSource.FireDiagnostic(this, "DownloadXmlUpdate", "downloading " + m_webResponse.ContentLength.ToString() + " bytes from " + m_strRemoteFileSpec);
				
					//	Get the remote file stream to download the file
					if((streamRemote = m_webResponse.GetResponseStream()) != null)
					{
						//	Set the time out period (ms)
						InitTimedOut(10000);
						
						//	Read the remote stream until no more data
						while(true)
						{
							//	Read data from the remote stream
							iBytesRead = streamRemote.Read(aTransferBytes, 0, aTransferBytes.Length);

							//	Did we get any new data?
							if(iBytesRead > 0)
							{
								streamLocal.Write(aTransferBytes, 0, iBytesRead);
								lTotalBytes += iBytesRead;
								
								//	Reset the time out start time
								SetLastTime();
								
								//	Update the progress bar
								SetProgressUpdates(null, lTotalBytes);

							}
							else
							{
								//	Have we transferred all the bytes?
								if(lTotalBytes >= m_webResponse.ContentLength)
								{
									//	Finished
									StopProgressUpdates("download complete", true);
									bSuccessful = true;
									break;
								}
								else
								{
									//	If we don't know how may bytes to download, we have to 
									//	assume we're done
									if(m_webResponse.ContentLength <= 0)
									{
										StopProgressUpdates("download complete", true);
										bSuccessful = true;
									}
									else
									{
										strLocalError = m_tmaxErrorBuilder.Message(ERROR_CONNECTION_CLOSED, m_strRemoteFileSpec, lTotalBytes, m_webResponse.ContentLength);
										strStatusMsg = ("connection closed - download incomplete");
										break;
									}
									break;
								
								}
							
							}// if(iBytesRead > 0)
							
							//	Has the user cancelled
							if(GetCancelled(null) == true) break;
							
						}// while(true)
					
					}
					else
					{
						strLocalError = m_tmaxErrorBuilder.Message(ERROR_CREATE_REMOTE_LIST_STREAM, m_strRemoteFileSpec);
						strStatusMsg = "Unable to create remote stream";
					
					}// if((streamRemote = m_webResponse.GetResponseStream()) != null)
					
				}
				catch
				{
					strLocalError = m_tmaxErrorBuilder.Message(ERROR_TRANSFER_UPDATES_LIST, m_strLocalFileSpec);
					strStatusMsg = "Error transferring updates list";
					break;
				}
				
				//	Only try once
				bContinue = false;

			}// while(bContinue == true)
			
			//	Should we display the error messages?
			if((bSuccessful == false) && (GetCancelled(null) == false))
			{
				StopProgressUpdates(strStatusMsg, false);
				OnLocalError(strLocalError);
			}
			
			//	Clean up
			if(streamRemote != null) 
			{
				try { streamRemote.Close(); }
				catch{}
				streamRemote = null;
			}
			if(streamLocal != null) 
			{
				try { streamLocal.Close(); }
				catch{}
				streamLocal = null;
			}
			aTransferBytes = null;
			Disconnect();
			
			return bSuccessful;
			
		}// private void DownloadXmlUpdate()

		/// <summary>This method is called to display an error message that we don't want to treat as a system error</summary>
		private void OnLocalError(string strMessage)
		{
			//	Display an error message
			MessageBox.Show(strMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			
		}// private void OnLocalError(string strMessage)

		/// <summary>This method is called to connect to the specified site</summary>
		/// <param name="strAddress">The address of the site to be connected</param>
		/// <param name="strUser">The desired user name</param>
		/// <param name="strPassword">The desired password</param>
		/// <returns>true if successful</returns>
		private bool Connect(string strAddress, string strUser, string strPassword)
		{
			string strMsg = "";
			
			//	Make sure we've cleaned up from the last operation
			Disconnect();
			
			//	Clear the cancelled flag
			m_bCancelled = false;
			
			//	Create the web request
			try 
			{ 
				m_webRequest = WebRequest.Create(strAddress); 
			}
			catch(System.Exception Ex) 
			{ 
				strMsg  = ("WebRequest.Create(" + strAddress + ") FAILED\r\n");
				strMsg += ("Address: " + strAddress + "\r\n");
				strMsg += ("User: " + strUser + "\r\n");
				strMsg += ("Password: " + strPassword + "\r\n");
				strMsg += ("Exception:\r\n" + Ex.ToString());
				
				m_tmaxEventSource.FireDiagnostic(this, "Connect", strMsg); 
			}
			if(m_webRequest == null)
				return false;

			//	Set the authentication information for this request
			if(SetCredential(m_webRequest, strUser, strPassword) == true)
			{
				//	Set the proxy information
				SetProxy(m_webRequest);
		
				//	Send the request to the server and retrieve the response
				try 
				{ 
					m_webResponse = m_webRequest.GetResponse(); 
				}
				catch(System.Exception Ex) 
				{ 
					strMsg  = ("WebRequest.GetResponse() FAILED\r\n");
					strMsg += ("Address: " + strAddress + "\r\n");
					strMsg += ("User: " + strUser + "\r\n");
					strMsg += ("Password: " + strPassword + "\r\n");
					strMsg += ("Exception:\r\n" + Ex.ToString());
				
					m_tmaxEventSource.FireDiagnostic(this, "Connect", strMsg); 
				}

			}// if(SetCredential(m_webRequest, strUser, strPassword) == true)

			if(m_webResponse == null)
				Disconnect();
	
			return (m_webResponse != null);
			
		}// private bool Connect(string strAddress)
		
		/// <summary>This method is to set the authentication credentials for the specified request</summary>
		/// <param name="webRequest">The web request object to be authenticated</param>
		/// <param name="strUser">The desired user name</param>
		/// <param name="strPassword">The desired password</param>
		/// <returns>true if successful</returns>
		private bool SetCredential(WebRequest webRequest, string strUser, string strPassword)
		{
			System.Net.NetworkCredential	netCredential = null;
			bool							bSuccessful = false;
			
			//	Don't bother if no user name
			if((strUser == null) || (strUser.Length == 0))
				return true; // Not an error - just means no authentication required

			try
			{
				//	Create the network credential and assign it to the request
				webRequest.PreAuthenticate = true;
				netCredential = new NetworkCredential();
				netCredential.UserName = strUser;
				if((strPassword != null) && (strPassword.Length > 0))
					netCredential.Password = strPassword;
				webRequest.Credentials = netCredential;
						
				bSuccessful = true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetCredential", m_tmaxErrorBuilder.Message(ERROR_SET_CREDENTIAL_EX, webRequest.RequestUri.AbsoluteUri, strUser, strPassword != null ? strPassword : ""), Ex);
			}
			
			return bSuccessful;
			
		}// private bool SetCredential(WebRequest webRequest, string strUser, string strPassword)

		/// <summary>This method is called to set the proxy information for the specified request</summary>
		/// <param name="webRequest">The web request object to be assigned the proxy</param>
		/// <returns>true if successful</returns>
		private bool SetProxy(WebRequest webRequest)
		{
			IWebProxy			webProxy = null;
			NetworkCredential	netCredential = null;
			bool				bSuccessful = false;
			
			//	NOTE:	We assume the proxy settings have been transferred to the product manager
			Debug.Assert(m_tmaxProductManager != null);
			
			try
			{
				//	Are we using the default browser settings?
				if(m_tmaxProductManager.UseIEProxy == true)
				{
                    //  .NET 2.0 Modification
					//webProxy = System.Net.WebProxy.GetDefaultProxy();
                    webProxy = WebRequest.DefaultWebProxy;
                }
				else if(m_tmaxProductManager.ProxyServerName.Length > 0)
				{
					if(m_tmaxProductManager.ProxyPort <= 0)
						m_tmaxProductManager.ProxyPort = m_tmaxProductManager.GetDefaultProxyPort();
						
					webProxy = new WebProxy(m_tmaxProductManager.ProxyServerName, m_tmaxProductManager.ProxyPort);
					
					//	Should we assign proxy server credentials?
					if(m_tmaxProductManager.ProxyUserName.Length > 0)
					{
						netCredential = new NetworkCredential();
						netCredential.UserName = m_tmaxProductManager.ProxyUserName;
						if(m_tmaxProductManager.ProxyPassword.Length > 0)
							netCredential.Password = m_tmaxProductManager.ProxyPassword;
						
						webProxy.Credentials = netCredential;
					}
					
				}// if(m_tmaxProductManager.UseIEProxy == true)
				
				//	Do we have a proxy to assign?
				if(webProxy != null)
					webRequest.Proxy = webProxy;
				
				bSuccessful = true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProxy", m_tmaxErrorBuilder.Message(ERROR_SET_PROXY_EX, webRequest.RequestUri.AbsoluteUri), Ex);
			}
			
			return bSuccessful;
			
		}// private bool SetProxy(WebRequest webRequest)

		/// <summary>This method cleans up after connecting to a site</summary>
		private void Disconnect()
		{
			if(m_webResponse != null)
			{
				try { m_webResponse.Close(); }
				catch {};
			}
			
			m_webResponse = null;
			m_webRequest = null;
			
		}// private void Disconnect()
		
		/// <summary>This method initializes the progress updates for a download operation</summary>
		/// <param name="strStatus">Status message to be displayed</param>
		/// <param name="lMaximum">Maximum number of bytes to be downloaded</param>
		private void StartProgressUpdates(string strStatus, long lMaximum)
		{
			//	Set the shared status text
			if(strStatus != null)
				SetSharedStatus(strStatus);
			
			if((m_sharedProgress != null) && (m_sharedProgress.IsDisposed == false))
			{
				try
				{
					//	Is this going to be a percent complete indicator?
					if(lMaximum > 0)
					{
						m_lProgress = 0;
						m_lMinProgress = 0;
						m_lMaxProgress = lMaximum;
						
						m_sharedProgress.Minimum = 0;
						m_sharedProgress.Maximum = 100;
						m_sharedProgress.Value = 0;
						m_sharedProgress.TextVisible = true;
					}
					else
					{
						m_lMinProgress = WRAP_PROGRESS_MINIMUM;
						m_lMaxProgress = 0;
						m_lProgress = WRAP_PROGRESS_MINIMUM;
						
						//	Set up for wrap around progress updates
						m_sharedProgress.Minimum = WRAP_PROGRESS_MINIMUM;
						m_sharedProgress.Maximum = 0;
						m_sharedProgress.Value   = WRAP_PROGRESS_MINIMUM;
						m_sharedProgress.TextVisible = false;
					}
					
					//	Make sure the progress bar is visible
					m_sharedProgress.Visible = true;
					m_sharedProgress.Update();
				
				}
				catch(System.Exception Ex)
				{
					string strMsg;
					strMsg  = "Minimum: " + m_sharedProgress.Minimum.ToString() + "\n";
					strMsg += "Maximum: " + m_sharedProgress.Maximum.ToString() + "\n";
					strMsg += "Value: " + m_sharedProgress.Value.ToString() + "\n";
					MessageBox.Show(strMsg);
					MessageBox.Show(Ex.ToString());
				}
			
			}// if((m_sharedProgress != null) && (m_sharedProgress.IsDisposed == false))
			
		}// private void StartProgressUpdates(string strStatus, long lMaximum)
		
		/// <summary>This method stops the download progress bar updates</summary>
		/// <param name="strStatus">Status message to be displayed</param>
		/// <param name="bFinished">true if the operation finished normally</param>
		private void StopProgressUpdates(string strStatus, bool bFinished)
		{
			//	Set the shared status text if a message has been provided
			if(strStatus != null)
				SetSharedStatus(strStatus);
			
			if((m_sharedProgress != null) && (m_sharedProgress.IsDisposed == false))
			{
				try
				{
					//	Did the operation finish normally?
					if(bFinished == true)
						m_sharedProgress.Value = m_sharedProgress.Maximum;
					
					m_sharedProgress.Update();
				}
				catch
				{
				}
			
			}// if((m_sharedProgress != null) && (m_sharedProgress.IsDisposed == false))
			
		}// private void StopProgressUpdates(string strStatus, bool bFinished)
		
		/// <summary>This method sets the current value of the progress bar</summary>
		/// <param name="strStatus">Status message to be displayed</param>
		/// <param name="lValue">The current value of the progress bar</param>
		private void SetProgressUpdates(string strStatus, long lValue)
		{
			//	Set the shared status text if provided
			if(strStatus != null)
				SetSharedStatus(strStatus);
			
			if((m_sharedProgress != null) && (m_sharedProgress.IsDisposed == false))
			{
				try
				{
					//	Is this a wrap-around progress bar?
					if(m_sharedProgress.Maximum <= 0)
					{
						if((m_sharedProgress.Value + WRAP_PROGRESS_STEP) > m_sharedProgress.Maximum)
							m_sharedProgress.Value = m_sharedProgress.Minimum;
						else
							m_sharedProgress.Value = (m_sharedProgress.Value + WRAP_PROGRESS_STEP);
					
						m_lProgress = m_sharedProgress.Value;
					}
					else
					{
						
						//	Compute the percent complete
						try
						{
							m_lProgress = lValue;
							
							int iComplete = 0;
							if(m_lProgress <= m_lMinProgress)
								iComplete = 0;
							else if(m_lProgress >= m_lMaxProgress)
								iComplete = 100;
							else
								iComplete = ((int)(((double)m_lProgress / (double)(m_lMaxProgress - m_lMinProgress)) * 100.0));
							
							//	Set the current value
							m_sharedProgress.Value = iComplete;
							
						}
						catch
						{
						}

					}// if(m_sharedProgress.Maximum <= 0)
					
					m_sharedProgress.Update();
				
				}
				catch
				{
				}
			
			}// if((m_sharedProgress != null) && (m_sharedProgress.IsDisposed == false))
			
		}// private void SetProgressUpdates(string strStatus, int iValue)
		
		/// <summary>This method sets the status text displayed on the shared property page</summary>
		/// <param name="strStatus">Status message to be displayed</param>
		private void SetSharedStatus(string strMessage)
		{
			if((m_sharedStatus != null) && (m_sharedStatus.IsDisposed == false))
			{
				try
				{
					if(strMessage != null)
						m_sharedStatus.Text = strMessage;
					else
						m_sharedStatus.Text = "";
				}
				catch
				{
				}
				
			}// if((m_sharedStatus != null) && (m_sharedStatus.IsDisposed == false))
			
		}// private void StartProgressUpdates(string strStatus, long lMaximum)
		
		/// <summary>This method opens the XML file containing the list of updates returned by the server</summary>
		/// <returns>true if successful</returns>
		private bool OpenXmlUpdate()
		{
			bool bSuccessful = false;
			
			//	Make sure the active update is closed
			CloseXmlUpdate();
			
			try
			{
				//	Allocate a new XML product update
				m_xmlUpdate = new CXmlProductUpdate();
			
				//	Open the product update file
				if(m_xmlUpdate.FastFill(m_strLocalFileSpec, true) == false)
				{
					OnLocalError(m_tmaxErrorBuilder.Message(ERROR_OPEN_PRODUCT_UPDATE_FAILED, m_strLocalFileSpec));
					return false;
				}

				//	Set the user name and time stamp
				m_xmlUpdate.Source = m_strRemoteFileSpec;
				m_xmlUpdate.RetrievedBy = System.Environment.UserName;
				m_xmlUpdate.RetrievedOn = System.DateTime.Now.ToString();
				m_xmlUpdate.AppFilename = System.IO.Path.GetFileName(Application.ExecutablePath);
				
				//	Do we have any updates to process?
				if(m_xmlUpdate.Updates.Count > 0)
				{
					//	Make sure the updates are in sorted order
					m_xmlUpdate.Updates.Sort(true);
					
					//	Locate all updates that can be installed
					foreach(CXmlUpdate O in m_xmlUpdate.Updates)
					{
						if(CheckAvailability(O) == true)
							bSuccessful = true;

					}// foreach(CXmlUpdate O in m_xmlUpdate.Updates)
					
					//	Have all updates been installed
					if(bSuccessful == false)
					{
						MessageBox.Show("Your system is up to date. All updates have been installed.", "TrialMax Updates", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				
				}// if(m_xmlUpdate.Updates.Count > 0)
				else
				{
					MessageBox.Show("No updates reported by the Trialmax updates server.", "TrialMax Updates", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OpenXmlUpdate", m_tmaxErrorBuilder.Message(ERROR_OPEN_XML_UPDATES_EX, m_strLocalFileSpec), Ex);
			}
			
			//	Save the current values
			SaveXmlUpdate();
			
			return bSuccessful;
		
		}// private bool OpenXmlUpdate()

		/// <summary>This method saves the XML product update</summary>
		/// <returns>True if successful</returns>
		private bool SaveXmlUpdate()
		{
			if(m_xmlUpdate != null)
			{
				try 
				{ 
					if(m_xmlUpdate.Save() == true)
						return true;
				} 
				catch(System.Exception Ex) 
				{
					m_tmaxEventSource.FireDiagnostic(this, "SaveXmlUpdate", "Exception: " + Ex.ToString());
				}
				
			}
			
			//	If we've reached this point there must have been a problem
			return false;
		
		}// private bool SaveXmlUpdate()

		/// <summary>This method closes the XML file containing the updates list</summary>
		private void CloseXmlUpdate()
		{
			if(m_xmlUpdate != null)
			{
				//	Make sure the file has been saved
				SaveXmlUpdate();
				
				//	Close the file
				m_xmlUpdate.Close(true);
				m_xmlUpdate = null;
			}
		
		}// private void CloseXmlUpdate()

		/// <summary>This method is called to determine if the specified update should be installed</summary>
		/// <param name="xmlUpdate">The XML update object to check</param>
		/// <returns>true if it should be added to the available list</returns>
		private bool CheckAvailability(CXmlUpdate xmlUpdate)
		{
			CTmaxComponent	tmaxComponent = null;
			long			lComponent = 0;
			long			lUpdate = 0;
			int				iUpdateBuild = 0;
			int				iComponentBuild = 0;
			System.DateTime dtComponent = System.DateTime.Now;
			System.DateTime dtUpdate = System.DateTime.Now;
			
			Debug.Assert(xmlUpdate != null);
			Debug.Assert(m_tmaxProductManager != null);
			Debug.Assert(m_tmaxProductManager.Components != null);
			
			try
			{
				//	Initialize the update
				xmlUpdate.Available = false;
				
				//	Confirm the update's group identifier and required version
				if((CheckGroupId(xmlUpdate) == false) ||
				   (CheckProductVersion(xmlUpdate) == false) ||
				   (CheckRegInstalled(xmlUpdate) == false))
				{
					xmlUpdate.Selected = false;
					return false;
				}
				
				//	Get the product component that matches this update
				if((m_tmaxProductManager != null) && (m_tmaxProductManager.Components != null))
					tmaxComponent = m_tmaxProductManager.Components.Find(xmlUpdate.Component);

				//	Do we need to compare version information?
				if((tmaxComponent != null) && (xmlUpdate.IgnoreVersion == false))
				{
					lComponent = CBaseVersion.GetPackedVersion(tmaxComponent.Version);
					lUpdate = CBaseVersion.GetPackedVersion(xmlUpdate.Version);
					
					//	Use the build identifiers if the packed versions are equal
					if(lComponent == lUpdate)
					{
						iComponentBuild = CBaseVersion.GetBuild(tmaxComponent.Version);
						iUpdateBuild = CBaseVersion.GetBuild(xmlUpdate.Version);

						//	Do both have valid build identifiers?
						if((iComponentBuild > 0) && (iUpdateBuild > 0))
						{
							//	Convert the build identifiers to dates
							if((CBaseVersion.GetBuildAsDate(iComponentBuild, ref dtComponent) == true) &&
							   (CBaseVersion.GetBuildAsDate(iUpdateBuild, ref dtUpdate) == true))
							{
								//	Compare the actual dates instead of the numeric values
								xmlUpdate.Available = (dtUpdate > dtComponent);
							}
							else
							{
								xmlUpdate.Available = (iUpdateBuild > iComponentBuild);
							}

						}// if((iComponentBuild > 0) && (iUpdateBuild > 0))
						
					}
					else
					{
						//	Is this a newer version than the active component?
						xmlUpdate.Available = (lUpdate > lComponent);
					}

				}
				else
				{			
					//	NOTE:	We assume the update can be installed if we don't have
					//			a matching component
					xmlUpdate.Available = true;
				}
				
				//	NOTE:	We decided to leave it up to the person setting up the
				//			product update file to determine what should be pre-selected
				//xmlUpdate.Selected = xmlUpdate.Available;
				
				//	Can't be selected if not available
				if(xmlUpdate.Available == false)
					xmlUpdate.Selected = false;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CheckAvailability", m_tmaxErrorBuilder.Message(ERROR_CHECK_UPDATE_VERSION_EX, xmlUpdate.Description), Ex);
			}
			
			return xmlUpdate.Available;
		
		}// private bool CheckAvailability(CXmlUpdate xmlUpdate)

		/// <summary>Called to check the group id of the specified update to see if it should be available</summary>
		/// <param name="xmlUpdate">The XML update object to check</param>
		/// <returns>true if it the update should be allowed</returns>
		private bool CheckGroupId(CXmlUpdate xmlUpdate)
		{
			bool bAllowed = true;
			
			Debug.Assert(xmlUpdate != null);
			Debug.Assert(m_tmaxProductManager != null);

			try
			{
				//	Is this update meant for a specific group?
				if(xmlUpdate.GroupId.Length > 0)
				{
					//	Check the default group identifier
					if(String.Compare(xmlUpdate.GroupId, m_strDefaultGroupId, true) != 0)
					{
						//	Is this installation in the specified group?
						if(String.Compare(xmlUpdate.GroupId, m_tmaxProductManager.UpdateGroupId, true) != 0)
						{
							bAllowed = false;
						}

					}// if(String.Compare(xmlUpdate.GroupId, m_strDefaultGroupId, true) != 0)

				}// if(xmlUpdate.GroupId.Length > 0)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CheckGroupId", m_tmaxErrorBuilder.Message(ERROR_CHECK_GROUP_ID_EX, xmlUpdate.Description), Ex);
			}

			return bAllowed;

		}// private bool CheckGroupId(CXmlUpdate xmlUpdate)

		/// <summary>Called to check the update to confirm we have an appropriate product version</summary>
		/// <param name="xmlUpdate">The XML update object to check</param>
		/// <returns>true if it the update should be allowed</returns>
		private bool CheckProductVersion(CXmlUpdate xmlUpdate)
		{
			bool	bAllowed = true;
			long	lProductVer = 0;
			long	lUpdateVer = 0;

			Debug.Assert(xmlUpdate != null);
			Debug.Assert(m_tmaxProductManager != null);

			try
			{
				//	Has a product version for this update been requested?
				if(xmlUpdate.RequiredProductVer.Length > 0)
				{
					if(CTmaxToolbox.IsDigit(xmlUpdate.RequiredProductVer[0]) == true)
						lUpdateVer = CBaseVersion.GetPackedVersion(xmlUpdate.RequiredProductVer);
					else
						lUpdateVer = CBaseVersion.GetPackedVersion(xmlUpdate.RequiredProductVer.Substring(1));
						
					lProductVer = CBaseVersion.GetPackedVersion(m_tmaxProductManager.ShortVersion);

				}// if(xmlUpdate.RequiredProductVer.Length > 0)					
					
				//	Has a product version for this update been requested?
				if((lProductVer > 0) && (lUpdateVer > 0))
				{
					switch(xmlUpdate.RequiredProductVer[0])
					{
						case '-':
						case '<':
						
							bAllowed = (lProductVer <= lUpdateVer);
							break;

						case '+':
						case '>':

							bAllowed = (lProductVer >= lUpdateVer);
							break;

						case '=':
						default:
						
							bAllowed = (lProductVer == lUpdateVer);
							break;

					}// switch(xmlUpdate.RequiredProductVer[0])

				}// if((lProductVer > 0) && (lUpdateVer > 0))

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CheckProductVersion", m_tmaxErrorBuilder.Message(ERROR_CHECK_PRODUCT_VERSION_EX, xmlUpdate.Description), Ex);
			}

			return bAllowed;

		}// private bool CheckProductVersion(CXmlUpdate xmlUpdate)

		/// <summary>Called to check the registry to see if the component is already installed</summary>
		/// <param name="xmlUpdate">The XML update to be checked</param>
		/// <returns>true if it the update should be allowed</returns>
		private bool CheckRegInstalled(CXmlUpdate xmlUpdate)
		{
			bool		bInstalled = false;
			RegistryKey	rkInstalled = null;
			object		regValue = null;

			Debug.Assert(xmlUpdate != null);
			Debug.Assert(m_tmaxRegistry != null);

			try
			{
				//	Has an installation key been specified
				if(xmlUpdate.InstalledKeyPath.Length > 0)
				{
					//	Open the key at the specified path
					if((rkInstalled = m_tmaxRegistry.OpenSubKey(xmlUpdate.InstalledKeyPath, false, true)) != null)
					{
						//	Do we have to look for a value entry in this key
						if(xmlUpdate.InstalledValueName.Length > 0)
						{
							//	Get the value stored in the registry with this name
							if((regValue = rkInstalled.GetValue(xmlUpdate.InstalledValueName)) != null)
							{
								//	Do we need to check for a specific value?
								if(xmlUpdate.InstalledValue.Length > 0)
								{
									bInstalled = (String.Compare(xmlUpdate.InstalledValue, regValue.ToString(), true) == 0);
								}
								else
								{
									bInstalled = true; // Component has already been installed
								}

							}// if((regValue = rkInstalled.GetValue(xmlUpdate.InstalledValueName)) != null)
							
						}
						
						//	Do we need to check for a default value?
						else if(xmlUpdate.InstalledValue.Length > 0)
						{
							//	Get the value stored at the default location
							if((regValue = rkInstalled.GetValue("")) != null)
							{
								bInstalled = (String.Compare(xmlUpdate.InstalledValue, regValue.ToString(), true) == 0);
							}

						}
						
						//	No need to look for a value
						else
						{
							bInstalled = true; // Component is already installed

						}// if(xmlUpdate.InstalledValueName.Length > 0)

					}// if((rkInstalled = m_tmaxRegistry.OpenSubKey(xmlUpdate.InstalledKeyPath, false, true)) != null)
					
				}// if(xmlUpdate.InstalledKeyPath.Length > 0)	

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CheckRegInstalled", m_tmaxErrorBuilder.Message(ERROR_CHECK_REG_INSTALLED_EX, xmlUpdate.Description), Ex);
			}
			finally
			{
				if(rkInstalled != null)
					rkInstalled.Close();			
			}

			//	Enable the update only if the component is not installed
			return (bInstalled == false);

		}// private bool CheckRegInstalled(CXmlUpdate xmlUpdate)

		/// <summary>This method is called to populate the list of available updates</summary>
		/// <returns>true if there are available updates</returns>
		private bool FillAvailableUpdates()
		{
			ListViewItem lvItem = null;
			
			//	Clear the existing items
			m_selectUpdates.Items.Clear();
			
			if((m_xmlUpdate != null) && (m_xmlUpdate.Updates != null) && (m_xmlUpdate.Updates.Count > 0))
			{
				foreach(CXmlUpdate O in m_xmlUpdate.Updates)
				{
					//	Should this update be made available for installation?
					if(O.Available == true)
					{					
						//	Add a row to the list box
						lvItem = new ListViewItem();
						lvItem.Text = O.Description;
						lvItem.Tag = O;
						lvItem.Checked = O.Selected;
								
						m_selectUpdates.Items.Add(lvItem);
					
					}// if(O.Available == true)

				}// foreach(CXmlUpdate O in m_xmlAvailableUpdates)
			
				//	Automatically resize the columns to fit the text
				SuspendLayout();
				m_selectUpdates.Columns[0].Width = -2;
				ResumeLayout();

				return true;
			}
			else
			{
				return false;
			}
		
		}// private bool FillAvailableUpdates()

		/// <summary>This method is called to add an entry to the list of downloaded updates</summary>
		/// <returns>true if successful</returns>
		private bool AddDownloaded(CXmlUpdate xmlUpdate)
		{
			ListViewItem	lvItem = null;
			bool			bSuccessful = false;
			
			if(m_downloadUpdates == null) return false;
			if(m_downloadUpdates.IsDisposed == true) return false;
			
			try
			{
				//	Initialize a new item to add to the collection
				lvItem = new ListViewItem();
				lvItem.Text = xmlUpdate.Description;
				lvItem.Tag  = xmlUpdate;
				lvItem.Checked = true;
								
				//	Add the new item
				m_downloadUpdates.Items.Add(lvItem);

				//	Automatically resize the columns to fit the text
				SuspendLayout();
				m_downloadUpdates.Columns[0].Width = -2;
				ResumeLayout();
				
				// We're done...
				bSuccessful = true;

			}
			catch
			{
			}
			
			return bSuccessful;

		}// private bool AddDownloaded(CXmlUpdate xmlUpdate)

		/// <summary>This method is called when the user checks or unchecks one of the available updates on the Select page</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">the event argument object</param>
		private void OnUpdateItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			ListView lvUpdates = null;
			
			//	The sending object should be a list of updates
			try { lvUpdates = (ListView)sender; }
			catch { return; }
			
			// Is the item being checked!
			//
			//	NOTE: We have to do it this way because the control doesn't actually change
			//		  the item's check state until after this event gets fired
			if(e.NewValue == CheckState.Checked)
			{
				m_ctrlNext.Enabled = true;
			}
			else
			{
				//	We have to have more than one item checked
				if(GetCheckedCount(lvUpdates) > 1)
				{
					m_ctrlNext.Enabled = true;
				}
				else
				{
					m_ctrlNext.Enabled = false;
				}
			
			}// if(e.NewValue == CheckState.Checked)
			
		}// private void OnUpdateItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)

		/// <summary>This method is called to get the updates that have been selected for installation</summary>
		/// <returns>the total number of selections</returns>
		private int GetSelectedUpdates(ListView lvUpdates)
		{
			int iSelected = 0;
			
			if(lvUpdates == null) return 0;
			if(lvUpdates.IsDisposed == true) return 0;
			if(lvUpdates.Items == null) return 0;
			if(m_xmlUpdate == null) return 0;
			
			//	Check the list of available updates and see which ones are selected
			foreach(ListViewItem O in lvUpdates.Items)
			{
				try
				{
					((CXmlUpdate)O.Tag).Selected = O.Checked;
					if(O.Checked == true) iSelected++;
				}
				catch
				{
				}
						
			}// foreach(ListViewItem O in m_selectUpdates.Items)
			
			//	Save the XML using the new values
			SaveXmlUpdate();
			
			return iSelected;
		
		}// private int GetSelectedUpdates()
		
		/// <summary>This method is called to download all updates selected by the user</summary>
		private void DownloadSelections()
		{
			if(m_xmlUpdate == null) return;
			if(m_xmlUpdate.Updates == null) return;
			if(m_xmlUpdate.Updates.Count == 0) return;
			
			//	Clear the cancelled flag
			m_bCancelled = false;
			
			//	Try to download all selected updates
			foreach(CXmlUpdate O in m_xmlUpdate.Updates)
			{
				//	Don't bother if not available
				if(O.Available == false) continue;
				
				//	Don't bother if not selected for installation
				if(O.Selected == false) continue;
				
				//	Stop here if cancelled
				if(GetCancelled(null) == true) break;
				
				if(DownloadSelection(O) == true)
				{
					//	Add this update to the list
					AddDownloaded(O);
					
					//	Mark it as having been downloaded and save the file
					O.Downloaded = true;
					SaveXmlUpdate();
					
				}// if(DownloadSelection(O) == true)					
			
			}// foreach(CXmlUpdate O in m_xmlUpdate.Updates)
			
			//	Download operation is complete
			OnSelectionsDownloaded();
			
		}// private bool DownloadSelections()
		
		/// <summary>This method is called to download the specified update</summary>
		/// <returns>True if successful</returns>
		private bool DownloadSelection(CXmlUpdate xmlUpdate)
		{
			string	strLocal = "";
			string	strServer = "";
			string	strUrl = "";
			bool	bServerPassed = false;
			
			if(xmlUpdate == null) return false;
			
			//	Hide the progress bar and set the status text
			m_sharedProgress.Visible = false;
			SetSharedStatus("retrieving " + xmlUpdate.Description);
			
			//	Build the path to the local filename
			strLocal = xmlUpdate.GetLocalFileSpec(m_strLocalFolder);
			if(strLocal.Length == 0)
			{
				OnLocalError(m_tmaxErrorBuilder.Message(ERROR_NO_LOCAL_FILESPEC, xmlUpdate.Description));
				return false;
			}
			
			//	Does the file already exist?
			if(System.IO.File.Exists(strLocal) == true)
			{
				//	Is this file marked as having been downloaded?
				if(xmlUpdate.Downloaded == true)
				{
					return true;
				}
				else
				{
					//	Until we put version checking in the update installations
					//	we have to assume this is an old file
					try { System.IO.File.Delete(strLocal); }
					catch {};
				}
				
			}// if(System.IO.File.Exists(strLocal) == true)
			
			//	Get the URL to the file on the server
			strUrl = xmlUpdate.GetServerFileSpec(m_strRemoteFolderSpec);

			//	Parse out the server so we can test the connection first
			if(strUrl.Length > 0)
			{
				try
				{
					System.Uri uri = new System.Uri(strUrl);
					if(uri.HostNameType != UriHostNameType.Unknown)
						strServer = uri.GetLeftPart(UriPartial.Authority);
				}
				catch(System.Exception Ex) 
				{
					m_tmaxEventSource.FireDiagnostic(this, "DownloadSelection", "Invalid URI Exception: " + Ex.ToString());
				}
			
				//	Check to see if the user has cancelled the operation
				if(GetCancelled(null) == true) return false;
			
				if(strServer.Length == 0)
				{
					OnLocalError(m_tmaxErrorBuilder.Message(ERROR_INVALID_REMOTE_URL, xmlUpdate.Description, strUrl));
					return false;
				}

			}// if(strUrl.Length > 0)
			else
			{
				OnLocalError(m_tmaxErrorBuilder.Message(ERROR_NO_REMOTE_URL, xmlUpdate.Description));
				return false;
			}

			//	Try connecting to the server
			SetSharedStatus("connecting to " + strServer);
			bServerPassed = Connect(strServer, xmlUpdate.UserName, xmlUpdate.Password);
			Disconnect();
			
			//	Were we able to connect to the server?
			if(bServerPassed == true)
			{
				if(GetCancelled(null) == true) return false;
				
				//	Download the file
				if(DownloadInstallation(strUrl, strLocal, xmlUpdate) == false)
				{
					try { System.IO.File.Delete(strLocal); }
					catch {};
					return false;
				}
				else
				{
					return true;
				}
				
			}
			else
			{
				OnLocalError(m_tmaxErrorBuilder.Message(ERROR_CONNECT_DOWNLOAD_SERVER, strServer, xmlUpdate.Description));
				return false;
			}
			
		}// private bool DownloadSelection(CXmlUpdate xmlUpdate)
		
		/// <summary>This method is called to determine if the operation has been cancelled</summary>
		/// <param name="strStatusMsg">Text to be displayed in status if not cancelled</param>
		/// <returns>True if cancelled</returns>
		private bool GetCancelled(string strStatusMsg)
		{
			//	Force the application to process pending requests?
			Application.DoEvents();
			
			if(m_bCancelled == true)
			{
				StopProgressUpdates("operation cancelled", false);
			}
			else
			{
				if((strStatusMsg != null) && (strStatusMsg.Length > 0))
					SetSharedStatus(strStatusMsg);
			}
			
			return m_bCancelled; 
			
		}// private bool GetCancelled()
		
		/// <summary>Downloads the installation for the specified update</summary>
		/// <param name="strUrl">The URL to the installation file</param>
		/// <param name="strFileSpec">The local file specification</param>
		/// <param name="xmlUpdate">The update being downloaded</param>
		/// <returns>True if successful</returns>
		private bool DownloadInstallation(string strUrl, string strFileSpec, CXmlUpdate xmlUpdate)
		{
			bool			bSuccessful = false;
			bool			bContinue = true;
			Stream			localStream = null;
			Stream			remoteStream = null;
			byte []			aTransferred = new byte[2048];
			int				iBytesRead = 0;
			int				iTotalTransferred = 0;
			string			strLocalError = "";
			string			strStatusMsg = "";

			//	Initialize the progress bar
			StartProgressUpdates("downloading " + xmlUpdate.Description, -1);
			
			while(bContinue == true)
			{
				if(GetCancelled("connecting to update server") == true) break;
				
				//	Send the request to the server
				if(Connect(strUrl, xmlUpdate.UserName, xmlUpdate.Password) == false)
				{
					strLocalError = m_tmaxErrorBuilder.Message(ERROR_CONNECT_DOWNLOAD_FILE, strUrl, xmlUpdate.Description);
					strStatusMsg = ("Failed to locate " + strUrl);
					break;
				}

				if(GetCancelled("opening local file stream") == true) break;
				
				//	Create the local file stream
				try { localStream = System.IO.File.Create(strFileSpec); }
				catch
				{
					strLocalError = m_tmaxErrorBuilder.Message(ERROR_CREATE_LOCAL_STREAM, xmlUpdate.Description, strFileSpec);
					strStatusMsg = ("Unable to create " + strFileSpec.ToLower());
					break;
				}
				
				//	Did the user cancel?
				if(GetCancelled(null) == true) break;
				StartProgressUpdates("downloading " + xmlUpdate.Description, m_webResponse.ContentLength);

				try
				{	
					m_tmaxEventSource.FireDiagnostic(this, "DownloadInstallation", "downloading " + m_webResponse.ContentLength.ToString() + " bytes from " + strUrl);
				
					//	Get the remote file stream to download the file
					if((remoteStream = m_webResponse.GetResponseStream()) != null)
					{
						//	Set the time out period (ms)
						InitTimedOut(10000);
						
						//	Read the remote stream until no more data
						while(true)
						{
							//	Read data from the stream
							iBytesRead = remoteStream.Read(aTransferred, 0, aTransferred.Length);
							m_tmaxEventSource.FireDiagnostic(this, "DownloadInstallation", iBytesRead.ToString() + " of " + m_webResponse.ContentLength.ToString() + " downloaded");
							
							//	Did we receive any data
							if(iBytesRead > 0)
							{
								//	Copy the data to the local stream
								localStream.Write(aTransferred, 0, iBytesRead);
								iTotalTransferred += iBytesRead;
								
								//	Reset the time out start time
								SetLastTime();
								
								//	Update the progress bar
								SetProgressUpdates(null, iTotalTransferred);
							}
							else
							{
								//	Have we transferred all the bytes?
								if((m_webResponse.ContentLength <= 0) || (iTotalTransferred >= m_webResponse.ContentLength))
								{
									//	Finished
									StopProgressUpdates("download complete", true);
									bSuccessful = true;
									break;
								}
								else
								{
									strLocalError = m_tmaxErrorBuilder.Message(ERROR_CONNECTION_CLOSED, strUrl, iTotalTransferred, m_webResponse.ContentLength);
									strStatusMsg = ("connection closed - download incomplete");
									break;
								}
								
							}// if(iBytesRead > 0)
							
							//	Did the user cancel?
							if(GetCancelled(null) == true) break;
				
						} // while(true);
					
					}// if((streamRemote = m_webResponse.GetResponseStream()) != null)
					else
					{
						strLocalError = m_tmaxErrorBuilder.Message(ERROR_CREATE_DOWNLOAD_STREAM, strUrl, xmlUpdate.Description);
						strStatusMsg = ("Unable to create remote stream");
						break;
						
					}// if((streamRemote = m_webResponse.GetResponseStream()) != null)
					
				}
				catch(System.Exception Ex)
				{
					strLocalError = m_tmaxErrorBuilder.Message(ERROR_DOWNLOAD_INSTALLATION, xmlUpdate.Description, strUrl);
					strLocalError += "\n\nException:\n\n";
					strLocalError += Ex.ToString();
					strStatusMsg = ("download error");
					break;
				}
				
				//	Only try once
				bContinue = false;

			}// while(bContinue == true)
			
			//	Should we display the error messages?
			if((bSuccessful == false) && (GetCancelled(null) == false))
			{
				StopProgressUpdates(strStatusMsg, false);
				OnLocalError(strLocalError);
			}
			
			//	Clean up
			if(remoteStream != null) 
			{
				try { remoteStream.Close(); }
				catch{}
				remoteStream = null;
			}
			if(localStream != null) 
			{
				try { localStream.Close(); }
				catch{}
				localStream = null;
			}
			aTransferred = null;
			Disconnect();
			
			return bSuccessful;
			
		}// private bool DownloadInstallation(string strUrl, string strFileSpec, CXmlUpdate xmlUpdate)
		
		/// <summary>This is a debugging aide to display the parts of a URI</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void ShowUri(string strUri)
		{
			try
			{
				System.Uri uri = new System.Uri(strUri);
				ShowUri(uri);
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "ShowUri", Ex);
			}
		
		}// private void ShowUri(string strUri)

		/// <summary>Called to set the default group identifier used to locate available updates</summary>
		private void SetDefaultGroupId()
		{
			int iVerMajor = 0;
			int iVerMinor = 0;
			
			try
			{
				//	Get the product's major and minor version identifiers
				if((m_tmaxProductManager != null) && (m_tmaxProductManager.Version.Length > 0))
				{
					if(CBaseVersion.SplitVersion(m_tmaxProductManager.Version, ref iVerMajor, ref iVerMinor) == false)
						iVerMajor = 0;
				}
				
				if(iVerMajor > 0)
					m_strDefaultGroupId = String.Format("TM{0}{1}", iVerMajor, iVerMinor);
				else
					m_strDefaultGroupId = "";
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetDefaultGroupId", Ex);
				m_strDefaultGroupId = "";
			}

		}// private void SetDefaultGroupId()

		/// <summary>This is a debugging aide to display the parts of a URI</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void ShowUri(System.Uri uri)
		{
			string strMsg = "";

			try
			{
				strMsg += ("AbsolutePath: " + uri.AbsolutePath);
				strMsg += ("\nAbsoluteUri: " + uri.AbsoluteUri);
				strMsg += ("\nFragment: " + uri.Fragment);
				strMsg += ("\nHost: " + uri.Host);
				strMsg += ("\nHostNameType: " + uri.HostNameType.ToString());
				strMsg += ("\nLocalPath: " + uri.LocalPath);
				strMsg += ("\nAbsolutePath: " + uri.PathAndQuery);
				strMsg += ("\nPartial Authority: " + uri.GetLeftPart(UriPartial.Authority));
				strMsg += ("\nPartial Path: " + uri.GetLeftPart(UriPartial.Path));
				strMsg += ("\nPartial Scheme: " + uri.GetLeftPart(UriPartial.Scheme));
				strMsg += ("\nPort: " + uri.Port.ToString());
				
				MessageBox.Show(strMsg);
							
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "ShowUri", Ex);
			}
		
		}// private void ShowUri(string strUri)
		
		/// <summary>This method is called when selections have been downloaded</summary>
		private void OnSelectionsDownloaded()
		{
			int iDownloaded = 0;
			
			if(m_xmlUpdate == null) return;
			if(m_xmlUpdate.Updates == null) return;
			
			//	How many selections have been downloaded?
			foreach(CXmlUpdate O in m_xmlUpdate.Updates)
			{
				if(O.Downloaded == true)
					iDownloaded++;
			
			}// foreach(CXmlUpdate O in m_xmlUpdate.Updates)
			
			//	Download operation is complete
			m_sharedProgress.Visible = false;
			m_ctrlBack.Enabled = true;
			if(iDownloaded > 0)
			{
				m_ctrlNext.Enabled = true;
				SetSharedStatus("Select updates to install and click Next");
			}
			else
			{
				m_ctrlNext.Enabled = false;
				SetSharedStatus("No updates were downloaded");
			}
			
		}// private bool DownloadSelections()
		
		/// <summary>This method handles the Cancel button's Click event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickCancel(object sender, System.EventArgs e)
		{
			//	Do we have an active web request?
			if(m_webRequest != null)
			{
				m_bCancelled = true;
				try { m_webRequest.Abort(); } 
				catch {}
			}
			else
			{
				m_strInstallFileSpec = "";
				this.DialogResult = DialogResult.Cancel;
				this.Close();
			}
			
		}// private void OnClickCancel(object sender, System.EventArgs e)

		/// <summary>Called to initialize the timed out status</summary>
		/// <param name="lMilliseconds">The number of milliseconds before operation times out</param>
		public void InitTimedOut(long lMilliseconds)
		{
			m_lTimeOut = lMilliseconds;
			SetLastTime();
		
		}// public void InitTimedOut(long lMilliseconds)
		
		/// <summary>Called to update the last time information used to determine timed out status</summary>
		public void SetLastTime()
		{
			//	Set the last time to NOW
			m_dtLastTime = System.DateTime.Now;
		
		}// public void SetLastTime()
		
		/// <summary>Called to determine if the operation has timed out</summary>
		/// <returns>True if timed out</returns>
		public bool GetTimedOut()
		{
			//	Do we have a valid time out period?
			if(m_lTimeOut > 0)
			{
				System.TimeSpan tsElapsed = System.DateTime.Now.Subtract(m_dtLastTime);
				
				if(tsElapsed.Milliseconds > m_lTimeOut)
					return true;
					
			}
		
			return false;
			
		}// public bool GetTimedOut()
		
		/// <summary>Handles events fired when the Use IE proxy settings check box changes state</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The system event arguments</param>
		private void OnSetupIESettingsChanged(object sender, System.EventArgs e)
		{
			try
			{
				bool bUseIE = m_setupProxyIESettings.Checked;
				
				m_setupProxyServer.Enabled = (bUseIE == false);
				m_setupProxyPort.Enabled = (bUseIE == false);
				m_setupProxyUser.Enabled = (bUseIE == false);
				m_setupProxyPassword.Enabled = (bUseIE == false);
			}
			catch
			{
			}
			
		}// private void OnSetupIESettingsChanged(object sender, System.EventArgs e)

		/// <summary>This method transfers values between the proxy settings controls and the local application settings</summary>
		/// <param name="bSetControls">true if controls are being initialized</param>
		private void TransferProxySettings(bool bSetControls)
		{
			if(m_tmaxProductManager != null)
			{
				try
				{
					//	Are we setting the control values?
					if(bSetControls == true)
					{
						m_setupProxyServer.Text = m_tmaxProductManager.ProxyServerName;
						m_setupProxyPort.Text = m_tmaxProductManager.ProxyPort.ToString();
						m_setupProxyUser.Text = m_tmaxProductManager.ProxyUserName;
						m_setupProxyPassword.Text = m_tmaxProductManager.ProxyPassword;
						m_setupProxyIESettings.Checked = m_tmaxProductManager.UseIEProxy;
					
						OnSetupIESettingsChanged(m_setupProxyIESettings, System.EventArgs.Empty);
					}
					else
					{
						m_tmaxProductManager.ProxyServerName = m_setupProxyServer.Text;
						m_tmaxProductManager.ProxyUserName = m_setupProxyUser.Text;
						m_tmaxProductManager.ProxyPassword = m_setupProxyPassword.Text;
						m_tmaxProductManager.UseIEProxy = m_setupProxyIESettings.Checked;
						if(m_setupProxyPort.Text.Length > 0)
						{
							try { m_tmaxProductManager.ProxyPort = System.Convert.ToInt32(m_setupProxyPort.Text); }
							catch
							{
								m_tmaxProductManager.ProxyPort = -1;
							}
						
						}
						
						//	Assign the default port if we have an invalid value
						if(m_tmaxProductManager.ProxyPort <= 0)
						{
							m_tmaxProductManager.ProxyPort = m_tmaxProductManager.GetDefaultProxyPort();
							m_setupProxyPort.Text = m_tmaxProductManager.ProxyPort.ToString();
						}
						
					}// if(bSetControls == true)
				
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "TransferProxySettings", m_tmaxErrorBuilder.Message(ERROR_TRANSFER_PROXY_SETTINGS_EX, bSetControls ? "from" : "to"), Ex);
				}
			
			}// if(m_tmaxProductManager != null)
			
		}// private void TransferProxySettings(bool bSetControls)
		
		/// <summary>This method is called to install the selected updates</summary>
		/// <returns>True if successful</returns>
		private bool Install()
		{
			bool bSuccessful = false;
			
			while(bSuccessful == false)
			{
				//	Do we have any downloaded updates?
				if(m_downloadUpdates.Items.Count == 0)
				{
					OnLocalError(m_tmaxErrorBuilder.Message(ERROR_NO_DOWNLOADS));
					m_ctrlNext.Enabled = false;
					break;
				}
				
				//	Do we have any selected downloads?
				if(GetCheckedCount(m_downloadUpdates) == 0)
				{
					OnLocalError(m_tmaxErrorBuilder.Message(ERROR_NO_DOWNLOADS_SELECTED));
					m_ctrlNext.Enabled = false;
					break;
				}
				
				//	Get the selected updates
				GetSelectedUpdates(m_downloadUpdates);
					
				//	Set the path to the file used by the installer
				m_strInstallFileSpec = m_xmlUpdate.FileSpec;
				
				bSuccessful = true;
				
			}// while(bSuccessful == false)
			
			return bSuccessful;
			
		}// private bool Install()
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}
		
		/// <summary>The application's connection to the system registry</summary>
		public FTI.Shared.Trialmax.CTmaxRegistry Registry
		{
			get { return m_tmaxRegistry; }
			set { m_tmaxRegistry = value; }
		}
		
		/// <summary>The folder containing the host application</summary>
		public string AppFolder
		{
			get { return m_strAppFolder; }
			set { m_strAppFolder = value; }
		}
		
		/// <summary>The application's product descriptor</summary>
		public FTI.Shared.Trialmax.CTmaxProductManager Product
		{
			get { return m_tmaxProductManager; }
			set { m_tmaxProductManager = value; }
		}
		
		/// <summary>The processed product update file</summary>
		public string InstallFileSpec
		{
			get { return m_strInstallFileSpec; }
		}
		
		#endregion Properties

	}// public class CFUpdateWizard : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Forms
