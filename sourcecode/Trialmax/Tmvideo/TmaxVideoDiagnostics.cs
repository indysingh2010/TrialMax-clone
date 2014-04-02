using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.TMVV.Tmvideo
{
	/// <summary>This form allows users to view the system diagnostics</summary>
	public class CFTmaxVideoDiagnostics : CFTmaxVideoForm
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_EXCHANGE_EX = (ERROR_TMAX_VIDEO_FORM_MAX + 1);
		
		#endregion Constants

		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>Tab control to manage the property pages</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabControl m_ctrlUltraTabs;
		
		/// <summary>Property page shared by all tabs</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage m_ctrlUltraSharedPage;
		
		/// <summary>The form's Accept button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		
		/// <summary>Private member bound to VideoOptions property</summary>
		private CTmaxVideoOptions m_tmaxVideoOptions = null;
		
		/// <summary>Private member bound to VideoDiagnostics property</summary>
		private CTmaxVideoDiagnostics m_tmaxVideoDiagnostics = null;
		
		/// <summary>Check box to allow user to request logging of diagnostic messages</summary>
		private System.Windows.Forms.CheckBox m_ctrlLogDiagnostics;
		
		/// <summary>Check box to allow user to request display of diagnostic messages</summary>
		private System.Windows.Forms.CheckBox m_ctrlEnableDiagnostics;
		
		/// <summary>Property page to display system diagnostics</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlDiagnosticsPage;
		
		/// <summary>Property page to display component versions</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlVersionsPage;
		
		/// <summary>Property page to display system error messages</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlErrorsPage;
		
		/// <summary>List view to display error messages</summary>
		private FTI.Trialmax.Controls.CTmaxListViewCtrl m_ctrlErrors;
		
		/// <summary>List view to display diagnostic messages</summary>
		private FTI.Trialmax.Controls.CTmaxListViewCtrl m_ctrlDiagnostics;
		private FTI.Trialmax.Controls.CTmaxMessageCtrl m_ctrlVersions;
		
		/// <summary>Private member bound to InitialTabIndex property</summary>
		private int m_iInitialTabIndex = 0;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFTmaxVideoDiagnostics()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
			m_tmaxEventSource.Name = "TmaxVideo Options Form";
		}

		/// <summary>This method is called to add an error message</summary>
		/// <param name="Args">The error event arguments</param>
		/// <returns>true if successful</returns>
		public bool Add(CTmaxErrorArgs Args)
		{
			bool bSuccessful = true;
			
			try
			{
				if(m_tmaxVideoDiagnostics != null)
				{
					if((bSuccessful = m_tmaxVideoDiagnostics.Add(Args)) == true)
					{
						if((m_ctrlErrors != null) && (m_ctrlErrors.IsDisposed == false))
						{
							m_ctrlErrors.Add(Args);
						}
					}
					
				}// if(m_tmaxVideoDiagnostics != null)
				
			}
			catch
			{
				bSuccessful = false;
			}
			
			return bSuccessful;
			
		}// public bool Add(CTmaxErrorArgs Args)
		
		/// <summary>This method is called to add a diagnostic message</summary>
		/// <param name="Args">The error event arguments</param>
		/// <returns>true if successful</returns>
		public bool Add(CTmaxDiagnosticArgs Args)
		{
			bool bSuccessful = true;
			
			try
			{
				if(m_tmaxVideoDiagnostics != null)
				{
					if((bSuccessful = m_tmaxVideoDiagnostics.Add(Args)) == true)
					{
						if((m_ctrlDiagnostics != null) && (m_ctrlDiagnostics.IsDisposed == false))
						{
							m_ctrlDiagnostics.Add(Args);
						}
					}
					
				}// if(m_tmaxVideoDiagnostics != null)
				
			}
			catch
			{
				bSuccessful = false;
			}
			
			return bSuccessful;
			
		}// public bool Add(CTmaxDiagnosticArgs Args)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Do the base class processing
			base.SetErrorStrings();
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while exchanging data: SetControls = %1");

		}// protected override void SetErrorStrings()

		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		
		}// protected override void Dispose( bool disposing )
		
		/// <summary>Overloaded base class member to do custom initialization when the form window gets created</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			Debug.Assert(m_tmaxVideoOptions != null);

			//	Initialize the errors list
			FillErrors();
			
			//	Initialize the diagnostics list
			FillDiagnostics();
			
			//	Initialize the versions list
			FillVersions();
			
			//	Initialize the controls
			Exchange(true);

			//	Set the initial tab selection
			if((m_iInitialTabIndex > 0) && (m_iInitialTabIndex < m_ctrlUltraTabs.Tabs.Count))
			{
				try
				{
					if(m_ctrlUltraTabs.Tabs[m_iInitialTabIndex] != null)
						m_ctrlUltraTabs.SelectedTab = m_ctrlUltraTabs.Tabs[m_iInitialTabIndex];			
				}
				catch
				{
				}
			
			}// if((m_iInitialTabIndex > 0) && (m_iInitialTabIndex < m_ctrlUltraTabs.Tabs.Count))
			
			//	Perform the base class processing
			base.OnLoad(e);
		
		}// protected override void OnLoad(EventArgs e)

		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>This method is called to populate the error messages control</summary>
		/// <returns>true if successful</returns>
		public bool FillErrors()
		{
			bool bSuccessful = true;
			
			try
			{
				if((m_tmaxVideoDiagnostics != null) && (m_ctrlErrors != null) && (m_ctrlErrors.IsDisposed == false))
				{
					if((m_ctrlErrors != null) && (m_ctrlErrors.IsDisposed == false))
					{
						//	Set the control up to display errors
						m_ctrlErrors.Initialize(new CTmaxErrorArgs());
						
						//	Set the maximum number of e
						m_ctrlErrors.MaxRows = m_tmaxVideoDiagnostics.MaxErrors;

						//	Add all errors to the list view
						if(m_tmaxVideoDiagnostics.Errors.Count > 0)
							m_ctrlErrors.Add(m_tmaxVideoDiagnostics.Errors, true);
					}
				
				}
				
			}
			catch
			{
				bSuccessful = false;
			}
			
			return bSuccessful;
			
		}// public bool FillErrors()
		
		/// <summary>This method is called to populate the error messages control</summary>
		/// <returns>true if successful</returns>
		public bool FillDiagnostics()
		{
			bool bSuccessful = true;
			
			try
			{
				if((m_tmaxVideoDiagnostics != null) && (m_ctrlDiagnostics != null) && (m_ctrlDiagnostics.IsDisposed == false))
				{
					if((m_ctrlDiagnostics != null) && (m_ctrlDiagnostics.IsDisposed == false))
					{
						//	Set the control up to display errors
						m_ctrlDiagnostics.Initialize(new CTmaxDiagnosticArgs());
						
						//	Set the maximum number of e
						m_ctrlDiagnostics.MaxRows = m_tmaxVideoDiagnostics.MaxDiagnostics;

						//	Add all errors to the list view
						if(m_tmaxVideoDiagnostics.Diagnostics.Count > 0)
							m_ctrlDiagnostics.Add(m_tmaxVideoDiagnostics.Diagnostics, true);
					}
				
				}
				
			}
			catch
			{
				bSuccessful = false;
			}
			
			return bSuccessful;
			
		}// public bool FillDiagnostics()
		
		/// <summary>This method is called to populate the versions list</summary>
		/// <returns>true if successful</returns>
		public bool FillVersions()
		{
			bool bSuccessful = true;
			
			try
			{
				if((m_tmaxVideoDiagnostics != null) && (m_ctrlVersions != null) && (m_ctrlVersions.IsDisposed == false))
				{
					foreach(CBaseVersion O in m_tmaxVideoDiagnostics.Versions)
						m_ctrlVersions.Add(O);
				}
				
			}
			catch
			{
				bSuccessful = false;
			}
			
			return bSuccessful;
			
		}// public bool FillVersions()
		
		/// <summary>This method is called to exchange values between the form control and the local options object</summary>
		/// <param name="bSetControls">true to set the control values</param>
		/// <returns>true if successful</returns>
		private bool Exchange(bool bSetControls)
		{
			try
			{
				if(bSetControls == true)
				{
					m_ctrlEnableDiagnostics.Checked = m_tmaxVideoOptions.EnableDiagnostics;
					m_ctrlLogDiagnostics.Checked = m_tmaxVideoOptions.LogDiagnostics;
					
					// Enable/disable the LogDiagnostics check box
					m_ctrlLogDiagnostics.Enabled = m_ctrlEnableDiagnostics.Checked;
		
				}
				else
				{
					m_tmaxVideoOptions.EnableDiagnostics = m_ctrlEnableDiagnostics.Checked;
					m_tmaxVideoOptions.LogDiagnostics = m_ctrlLogDiagnostics.Checked;
				}
				
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Exchange", m_tmaxErrorBuilder.Message(ERROR_EXCHANGE_EX, bSetControls), Ex);
				return false;
			}
						
		}// private bool Exchange(bool bSetControls)
		
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOk(object sender, System.EventArgs e)
		{
			if(Exchange(false) == true)
			{			
				DialogResult = DialogResult.OK;
				this.Close();
			}
		
		}// private void OnClickOk(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on the EnableDiagnostics check box</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickEnableDiagnostics(object sender, System.EventArgs e)
		{
			// Enable/disable the LogDiagnostics check box
			m_ctrlLogDiagnostics.Enabled = m_ctrlEnableDiagnostics.Checked;
			Exchange(false);
		}

		/// <summary>This method is called when the user clicks on the EnableDiagnostics check box</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickLogDiagnostics(object sender, System.EventArgs e)
		{
			Exchange(false);
		}

		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFTmaxVideoDiagnostics));
			this.m_ctrlErrorsPage = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.m_ctrlErrors = new FTI.Trialmax.Controls.CTmaxListViewCtrl();
			this.m_ctrlVersionsPage = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.m_ctrlDiagnosticsPage = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.m_ctrlDiagnostics = new FTI.Trialmax.Controls.CTmaxListViewCtrl();
			this.m_ctrlLogDiagnostics = new System.Windows.Forms.CheckBox();
			this.m_ctrlEnableDiagnostics = new System.Windows.Forms.CheckBox();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlUltraSharedPage = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
			this.m_ctrlUltraTabs = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
			this.m_ctrlVersions = new FTI.Trialmax.Controls.CTmaxMessageCtrl();
			this.m_ctrlErrorsPage.SuspendLayout();
			this.m_ctrlVersionsPage.SuspendLayout();
			this.m_ctrlDiagnosticsPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraTabs)).BeginInit();
			this.m_ctrlUltraTabs.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlErrorsPage
			// 
			this.m_ctrlErrorsPage.Controls.Add(this.m_ctrlErrors);
			this.m_ctrlErrorsPage.Location = new System.Drawing.Point(-10000, -10000);
			this.m_ctrlErrorsPage.Name = "m_ctrlErrorsPage";
			this.m_ctrlErrorsPage.Size = new System.Drawing.Size(528, 198);
			// 
			// m_ctrlErrors
			// 
			this.m_ctrlErrors.AddTop = true;
			this.m_ctrlErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlErrors.AutoResizeColumns = true;
			this.m_ctrlErrors.ClearOnDblClick = false;
			this.m_ctrlErrors.DisplayMode = 0;
			this.m_ctrlErrors.HideSelection = false;
			this.m_ctrlErrors.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlErrors.MaxRows = 0;
			this.m_ctrlErrors.Name = "m_ctrlErrors";
			this.m_ctrlErrors.OwnerImages = null;
			this.m_ctrlErrors.SelectedIndex = -1;
			this.m_ctrlErrors.ShowHeaders = true;
			this.m_ctrlErrors.ShowImage = false;
			this.m_ctrlErrors.Size = new System.Drawing.Size(512, 184);
			this.m_ctrlErrors.TabIndex = 0;
			// 
			// m_ctrlVersionsPage
			// 
			this.m_ctrlVersionsPage.Controls.Add(this.m_ctrlVersions);
			this.m_ctrlVersionsPage.Location = new System.Drawing.Point(2, 24);
			this.m_ctrlVersionsPage.Name = "m_ctrlVersionsPage";
			this.m_ctrlVersionsPage.Size = new System.Drawing.Size(528, 198);
			// 
			// m_ctrlDiagnosticsPage
			// 
			this.m_ctrlDiagnosticsPage.Controls.Add(this.m_ctrlDiagnostics);
			this.m_ctrlDiagnosticsPage.Controls.Add(this.m_ctrlLogDiagnostics);
			this.m_ctrlDiagnosticsPage.Controls.Add(this.m_ctrlEnableDiagnostics);
			this.m_ctrlDiagnosticsPage.Location = new System.Drawing.Point(-10000, -10000);
			this.m_ctrlDiagnosticsPage.Name = "m_ctrlDiagnosticsPage";
			this.m_ctrlDiagnosticsPage.Size = new System.Drawing.Size(528, 198);
			// 
			// m_ctrlDiagnostics
			// 
			this.m_ctrlDiagnostics.AddTop = true;
			this.m_ctrlDiagnostics.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlDiagnostics.AutoResizeColumns = true;
			this.m_ctrlDiagnostics.ClearOnDblClick = false;
			this.m_ctrlDiagnostics.DisplayMode = 0;
			this.m_ctrlDiagnostics.HideSelection = false;
			this.m_ctrlDiagnostics.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlDiagnostics.MaxRows = 0;
			this.m_ctrlDiagnostics.Name = "m_ctrlDiagnostics";
			this.m_ctrlDiagnostics.OwnerImages = null;
			this.m_ctrlDiagnostics.SelectedIndex = -1;
			this.m_ctrlDiagnostics.ShowHeaders = true;
			this.m_ctrlDiagnostics.ShowImage = false;
			this.m_ctrlDiagnostics.Size = new System.Drawing.Size(512, 160);
			this.m_ctrlDiagnostics.TabIndex = 6;
			// 
			// m_ctrlLogDiagnostics
			// 
			this.m_ctrlLogDiagnostics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlLogDiagnostics.Location = new System.Drawing.Point(192, 176);
			this.m_ctrlLogDiagnostics.Name = "m_ctrlLogDiagnostics";
			this.m_ctrlLogDiagnostics.Size = new System.Drawing.Size(180, 16);
			this.m_ctrlLogDiagnostics.TabIndex = 5;
			this.m_ctrlLogDiagnostics.Text = "Log diagnostic messages";
			this.m_ctrlLogDiagnostics.Click += new System.EventHandler(this.OnClickLogDiagnostics);
			// 
			// m_ctrlEnableDiagnostics
			// 
			this.m_ctrlEnableDiagnostics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlEnableDiagnostics.Location = new System.Drawing.Point(8, 176);
			this.m_ctrlEnableDiagnostics.Name = "m_ctrlEnableDiagnostics";
			this.m_ctrlEnableDiagnostics.Size = new System.Drawing.Size(180, 16);
			this.m_ctrlEnableDiagnostics.TabIndex = 4;
			this.m_ctrlEnableDiagnostics.Text = "Enable diagnostic messages";
			this.m_ctrlEnableDiagnostics.Click += new System.EventHandler(this.OnClickEnableDiagnostics);
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(456, 240);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 11;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlUltraSharedPage
			// 
			this.m_ctrlUltraSharedPage.Location = new System.Drawing.Point(-10000, -10000);
			this.m_ctrlUltraSharedPage.Name = "m_ctrlUltraSharedPage";
			this.m_ctrlUltraSharedPage.Size = new System.Drawing.Size(528, 198);
			// 
			// m_ctrlUltraTabs
			// 
			this.m_ctrlUltraTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlUltraTabs.Controls.Add(this.m_ctrlUltraSharedPage);
			this.m_ctrlUltraTabs.Controls.Add(this.m_ctrlDiagnosticsPage);
			this.m_ctrlUltraTabs.Controls.Add(this.m_ctrlVersionsPage);
			this.m_ctrlUltraTabs.Controls.Add(this.m_ctrlErrorsPage);
			this.m_ctrlUltraTabs.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlUltraTabs.Name = "m_ctrlUltraTabs";
			this.m_ctrlUltraTabs.SharedControlsPage = this.m_ctrlUltraSharedPage;
			this.m_ctrlUltraTabs.Size = new System.Drawing.Size(532, 224);
			this.m_ctrlUltraTabs.TabIndex = 0;
			ultraTab1.TabPage = this.m_ctrlErrorsPage;
			ultraTab1.Text = "Errors";
			ultraTab1.ToolTipText = "System Error Messages";
			ultraTab2.TabPage = this.m_ctrlVersionsPage;
			ultraTab2.Text = "Versions";
			ultraTab2.ToolTipText = "Component Versions";
			ultraTab3.TabPage = this.m_ctrlDiagnosticsPage;
			ultraTab3.Text = "Diagnostics";
			ultraTab3.ToolTipText = "System Diagnostic Messages";
			this.m_ctrlUltraTabs.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
																									  ultraTab1,
																									  ultraTab2,
																									  ultraTab3});
			// 
			// m_ctrlVersions
			// 
			this.m_ctrlVersions.AddTop = false;
			this.m_ctrlVersions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlVersions.ClearOnDblClick = false;
			this.m_ctrlVersions.Format = FTI.Trialmax.Controls.TmaxMessageFormats.Version;
			this.m_ctrlVersions.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlVersions.MaxRows = 0;
			this.m_ctrlVersions.Name = "m_ctrlVersions";
			this.m_ctrlVersions.SelectedIndex = -1;
			this.m_ctrlVersions.ShowHeaders = true;
			this.m_ctrlVersions.ShowImage = false;
			this.m_ctrlVersions.Size = new System.Drawing.Size(512, 184);
			this.m_ctrlVersions.TabIndex = 0;
			// 
			// CFTmaxVideoDiagnostics
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(546, 271);
			this.Controls.Add(this.m_ctrlUltraTabs);
			this.Controls.Add(this.m_ctrlOk);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFTmaxVideoDiagnostics";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "System Information";
			this.TopMost = true;
			this.m_ctrlErrorsPage.ResumeLayout(false);
			this.m_ctrlVersionsPage.ResumeLayout(false);
			this.m_ctrlDiagnosticsPage.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraTabs)).EndInit();
			this.m_ctrlUltraTabs.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>Global TmaxVideo application options</summary>
		public CTmaxVideoOptions VideoOptions
		{
			get { return m_tmaxVideoOptions; }
			set { m_tmaxVideoOptions = value; }
		}
		
		/// <summary>Global TmaxVideo diagnostics object</summary>
		public CTmaxVideoDiagnostics VideoDiagnostics
		{
			get { return m_tmaxVideoDiagnostics; }
			set { m_tmaxVideoDiagnostics = value; }
		}
		
		/// <summary>Tab to be visible when the form opens</summary>
		public int InitialTabIndex
		{
			get { return m_iInitialTabIndex; }
			set { m_iInitialTabIndex = value; }
		}
		
		#endregion Properties
	
	}// public class CFTmaxVideoDiagnostics : System.Windows.Forms.Form

	/// <summary>This class stores all the system diagnostics information</summary>
	public class CTmaxVideoDiagnostics
	{
		#region Constants
		
		private const int DEFAULT_MAX_ERRORS		= 64;
		private const int DEFAULT_MAX_DIAGNOSTICS	= 64;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to Errors property</summary>
		private ArrayList m_aErrors = new ArrayList();
		
		/// <summary>Local member bound to Diagnostics property</summary>
		private ArrayList m_aDiagnostics = new ArrayList();
		
		/// <summary>Local member bound to Versions property</summary>
		private ArrayList m_aVersions = new ArrayList();
		
		/// <summary>Local member bound to MaxErrors property</summary>
		private int m_iMaxErrors = DEFAULT_MAX_ERRORS;
		
		/// <summary>Local member bound to MaxDiagnostics property</summary>
		private int m_iMaxDiagnostics = DEFAULT_MAX_DIAGNOSTICS;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxVideoDiagnostics()
		{
		
		}// CTmaxVideoDiagnostics()
		
		/// <summary>This method is called to add an error message</summary>
		/// <param name="Args">The error event arguments</param>
		/// <returns>true if successful</returns>
		public bool Add(CTmaxErrorArgs Args)
		{
			bool bSuccessful = true;
			
			try
			{
				//	Add to the collection
				m_aErrors.Add(Args);
					
				//	Do we have too many?
				while((this.MaxErrors > 0) && (m_aErrors.Count > this.MaxErrors))
					m_aErrors.RemoveAt(0);
			}
			catch
			{
				bSuccessful = false;
			}
			
			return bSuccessful;
			
		}// public bool Add(CTmaxErrorArgs Args)
		
		/// <summary>This method is called to add a diagnostic message</summary>
		/// <param name="Args">The error event arguments</param>
		/// <returns>true if successful</returns>
		public bool Add(CTmaxDiagnosticArgs Args)
		{
			bool bSuccessful = true;
			
			try
			{
				//	Add to the collection
				m_aDiagnostics.Add(Args);
					
				//	Do we have too many?
				while((this.MaxDiagnostics > 0) && (m_aDiagnostics.Count > this.MaxDiagnostics))
					m_aDiagnostics.RemoveAt(0);
			}
			catch
			{
				bSuccessful = false;
			}
			
			return bSuccessful;
			
		}// public bool Add(CTmaxDiagnosticArgs Args)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Error Messages</summary>
		public ArrayList Errors
		{
			get { return m_aErrors; }
		}
		
		/// <summary>Diagnostic Messages</summary>
		public ArrayList Diagnostics
		{
			get { return m_aDiagnostics; }
		}
		
		/// <summary>Version Descriptors</summary>
		public ArrayList Versions
		{
			get { return m_aVersions; }
		}
		
		/// <summary>Maximum number of error messages to store</summary>
		public int MaxErrors
		{
			get { return m_iMaxErrors; }
			set { m_iMaxErrors = value; }
		}
		
		/// <summary>Maximum number of diagnostic messages to store</summary>
		public int MaxDiagnostics
		{
			get { return m_iMaxDiagnostics; }
			set { m_iMaxDiagnostics = value; }
		}
		
		#endregion Properties

	}//	public class CTmaxVideoDiagnostics

}// namespace FTI.Trialmax.Forms
