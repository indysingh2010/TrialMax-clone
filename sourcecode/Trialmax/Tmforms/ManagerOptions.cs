using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Encode;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form allows users to set the Manager application options</summary>
	public class CFManagerOptions : CFTmaxBaseForm
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_EXCHANGE_EX = ERROR_TMAX_FORM_MAX + 1;
		
		#endregion Constants

		#region Private Members
		
		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>The form's Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>The form's Accept button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		
		/// <summary>The form's tab control to manage the individual pages</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabControl m_ctrlUltraTabs;
		
		/// <summary>Default shared property page assigned by Infragistics library</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage m_ctrlUltraSharedPage;
		
		/// <summary>Property page containing general application options</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlGeneralPage;
		
		/// <summary>Check box to enable/disable the ShowErrorMessages option</summary>
		private System.Windows.Forms.CheckBox m_ctrlShowErrorMessages;
		
		/// <summary>Check box to enable/disable diagnostic messages</summary>
		private System.Windows.Forms.CheckBox m_ctrlEnableDiagnostics;
		
		/// <summary>Check box to enable/disable logging of diagnostic messages</summary>
		private System.Windows.Forms.CheckBox m_ctrlLogDiagnostics;
		
		/// <summary>Property page for configuring file filters</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlFiltersPage;
		
		/// <summary>Check box to request automatic reloading of the last case</summary>
		private System.Windows.Forms.CheckBox m_ctrlLoadLastCase;
		
		/// <summary>List box of source media types</summary>
		private System.Windows.Forms.ListBox m_ctrlSourceTypes;
		
		/// <summary>Files filter for current source media type selection</summary>
		private System.Windows.Forms.TextBox m_ctrlSourceFilter;
		
		/// <summary>Label for source types list box</summary>
		private System.Windows.Forms.Label m_ctrlSourceTypesLabel;
		
		/// <summary>Label for source media file filter edit box</summary>
		private System.Windows.Forms.Label m_ctrlSourceFilterLabel;
		
		/// <summary>Filter page user prompt line 3</summary>
		private System.Windows.Forms.Label m_ctrlFiltersPrompt3;
		
		/// <summary>Filter page user prompt line 2</summary>
		private System.Windows.Forms.Label m_ctrlFiltersPrompt2;
		
		/// <summary>Filter page user prompt line 1</summary>
		private System.Windows.Forms.Label m_ctrlFiltersPrompt1;
		
		/// <summary>Private member bound to ManagerOptions property</summary>
		private FTI.Shared.Trialmax.CTmaxManagerOptions m_tmaxManagerOptions = null;
		
		/// <summary>Private member bound to SourceTypes property</summary>
		private FTI.Shared.Trialmax.CTmaxSourceTypes m_tmaxSourceTypes = null;
		
		/// <summary>Local member bound to the ProductManager property</summary>
		private FTI.Shared.Trialmax.CTmaxProductManager m_tmaxProductManager = null;
		
		/// <summary>Check box to enable foreign barcode operation</summary>
		private System.Windows.Forms.CheckBox m_ctrlShowForeignBarcodes;
		
		/// <summary>Tab page used to display on-line updates options</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlUpdatesPage;
		
		/// <summary>Static text label for Group Id control on Updates page</summary>
		private System.Windows.Forms.Label m_ctrlUpdatesGroupIdLabel;
		
		/// <summary>Edit box for entering group id on the Updates page</summary>
		private System.Windows.Forms.TextBox m_ctrlUpdatesGroupId;
		
		/// <summary>Edit box for entering primary site address on the Updates page</summary>
		private System.Windows.Forms.TextBox m_ctrlUpdatesPrimarySite;
		
		/// <summary>Static text label for Primary Site control on Updates page</summary>
		private System.Windows.Forms.Label m_ctrlUpdatesPrimarySiteLabel;
	
		/// <summary>Edit box for entering alternate site address on the Updates page</summary>
		private System.Windows.Forms.TextBox m_ctrlUpdatesAlternateSite;
		
		/// <summary>Static text label for Alternate Site control on Updates page</summary>
		private System.Windows.Forms.Label m_ctrlUpdatesAlternateSiteLabel;
		
		/// <summary>Check box to enable warning messages for duplicate binders</summary>
		private System.Windows.Forms.CheckBox m_ctrlWarnBinderDuplicates;
		
		/// <summary>Check box to request population of the filter tree on Open</summary>
		private System.Windows.Forms.CheckBox m_ctrlFilterOnOpen;
		
		/// <summary>Private member to keep track of active source type</summary>
		private FTI.Shared.Trialmax.CTmaxSourceType m_tmaxSourceType = null;
		
		/// <summary>Private member bound to WMEncoder property</summary>
		private FTI.Trialmax.Encode.CWMEncoder m_wmEncoder = null;
		
		/// <summary>Property page to display encoder profiles</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlProfilesPage;
		
		/// <summary>List box of available profiles</summary>
		private System.Windows.Forms.ListBox m_ctrlProfiles;
		
		/// <summary>Private member to keep track of the active profile</summary>
		private FTI.Trialmax.Encode.CWMProfile m_wmProfile = null;
		
		/// <summary>Static text label for profiles list box</summary>
		private System.Windows.Forms.Label m_ctrlProfilesLabel;
		
		/// <summary>Group box for profile encoders</summary>
		private System.Windows.Forms.GroupBox m_ctrlProfileCodecsGroup;
		
		/// <summary>Video encoder for selected profile</summary>
		private System.Windows.Forms.Label m_ctrlProfileVideo;
		
		/// <summary>Audio encoder for selected profile</summary>
		private System.Windows.Forms.Label m_ctrlProfileAudio;
		private System.Windows.Forms.Button m_ctrlViewCodecs;
        private System.Windows.Forms.CheckBox m_ctrlShowAudioWaveform;
		
		/// <summary>Check box to set Profile Preferred flag</summary>
		private System.Windows.Forms.CheckBox m_ctrlPreferred;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFManagerOptions()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

		}

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to exchange the application options.");

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
			Debug.Assert(m_tmaxManagerOptions != null);
			
			//	Initialize the controls
			Exchange(true);
			
			//	Perform the base class processing
			base.OnLoad(e);
		
		}// protected override void OnLoad(EventArgs e)

		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>This method is called to exchange values between the form control and the local options object</summary>
		/// <param name="bSetControls">true to set the control values</param>
		/// <returns>true if successful</returns>
		private bool Exchange(bool bSetControls)
		{
			try
			{
				if(bSetControls == true)
				{
					m_ctrlLoadLastCase.Checked = m_tmaxManagerOptions.LoadLastCase;
					m_ctrlShowErrorMessages.Checked = m_tmaxManagerOptions.ShowErrorMessages;
					m_ctrlShowForeignBarcodes.Checked = m_tmaxManagerOptions.ShowForeignBarcodes;
					m_ctrlEnableDiagnostics.Checked = m_tmaxManagerOptions.EnableDiagnostics;
					m_ctrlLogDiagnostics.Checked = m_tmaxManagerOptions.LogDiagnostics;
					m_ctrlWarnBinderDuplicates.Checked = m_tmaxManagerOptions.WarnBinderDuplicates;
					m_ctrlFilterOnOpen.Checked = m_tmaxManagerOptions.FilterOnOpen;
                    m_ctrlShowAudioWaveform.Checked = m_tmaxManagerOptions.ShowAudioWaveform;
					
					//	Fill the list of source types
					if((m_tmaxSourceTypes != null) && (m_tmaxSourceTypes.Count > 0))
					{
						foreach(CTmaxSourceType O in m_tmaxSourceTypes)
						{
							if(O.RegSourceType != RegSourceTypes.NoSource)
								m_ctrlSourceTypes.Items.Add(O.RegSourceType);
						}
						
					}
					
					//	Select the initial source type
					if(m_ctrlSourceTypes.Items.Count > 0)
					{
						m_ctrlSourceTypes.SelectedIndex = 0;
						SetSourceFilter(false, true);
					}
					
					// Enable/disable the LogDiagnostics check box
					m_ctrlLogDiagnostics.Enabled = m_ctrlEnableDiagnostics.Checked;
		
					if(m_tmaxProductManager != null)
					{
						m_ctrlUpdatesPrimarySite.Text = m_tmaxProductManager.UpdateSite;
						m_ctrlUpdatesAlternateSite.Text = m_tmaxProductManager.UpdateAlternateSite;
						m_ctrlUpdatesGroupId.Text = m_tmaxProductManager.UpdateGroupId;
					}
					else
					{
						m_ctrlUpdatesPrimarySite.Text = "";
						m_ctrlUpdatesAlternateSite.Text = "";
						m_ctrlUpdatesGroupId.Text = "";
						m_ctrlUpdatesGroupId.Enabled = false;
						m_ctrlUpdatesPrimarySite.Enabled = false;
						m_ctrlUpdatesAlternateSite.Enabled = false;
					}
					
					//	Fill the list of media encoders
					FillProfiles();					
				}
				else
				{
					m_tmaxManagerOptions.ShowErrorMessages = m_ctrlShowErrorMessages.Checked;
					m_tmaxManagerOptions.ShowForeignBarcodes = m_ctrlShowForeignBarcodes.Checked;
					m_tmaxManagerOptions.EnableDiagnostics = m_ctrlEnableDiagnostics.Checked;
					m_tmaxManagerOptions.LogDiagnostics = m_ctrlLogDiagnostics.Checked;
					m_tmaxManagerOptions.LoadLastCase = m_ctrlLoadLastCase.Checked;
					m_tmaxManagerOptions.WarnBinderDuplicates = m_ctrlWarnBinderDuplicates.Checked;
					m_tmaxManagerOptions.FilterOnOpen = m_ctrlFilterOnOpen.Checked;
                    m_tmaxManagerOptions.ShowAudioWaveform = m_ctrlShowAudioWaveform.Checked;

					//	Make sure the current source type is up to date
					SetSourceFilter(true, false);

					if(m_tmaxProductManager != null)
					{
						m_tmaxProductManager.UpdateSite = m_ctrlUpdatesPrimarySite.Text;
						m_tmaxProductManager.UpdateAlternateSite = m_ctrlUpdatesAlternateSite.Text;
						m_tmaxProductManager.UpdateGroupId = m_ctrlUpdatesGroupId.Text;
					}
					
					//	Are we working with the encoder profiles?
					if(m_ctrlProfiles.Items.Count > 0)
					{
						//	Make sure the current selection is up to date
						if(m_wmProfile != null)
							m_wmProfile.Preferred = m_ctrlPreferred.Checked;
							
						//	Update the list of preferred profiles
						if(m_wmEncoder.PreferredProfiles != null)
							m_wmEncoder.Profiles.GetPreferred(m_wmEncoder.PreferredProfiles);
						
					}// if(m_ctrlProfiles.Items.Count > 0)

				}
				
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Exchange", m_tmaxErrorBuilder.Message(ERROR_EXCHANGE_EX), Ex);
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

		/// <summary>This method is called when the user clicks on View Codecs</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickViewCodecs(object sender, System.EventArgs e)
		{
			try
			{
				CFEnumCodecs enumCodecs = new CFEnumCodecs();

				//	Do we have a profile selection?
				if(m_ctrlProfiles.SelectedItem != null)
					enumCodecs.Profile = ((CWMProfile)m_ctrlProfiles.SelectedItem).Name;

				enumCodecs.ShowDialog();
			}
			catch
			{
			}
		
		}// private void OnClickViewCodecs(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on the EnableDiagnostics check box</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickEnableDiagnostics(object sender, System.EventArgs e)
		{
			// Enable/disable the LogDiagnostics check box
			m_ctrlLogDiagnostics.Enabled = m_ctrlEnableDiagnostics.Checked;
		
		}// private void OnClickEnableDiagnostics(object sender, System.EventArgs e)

		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFManagerOptions));
            this.m_ctrlGeneralPage = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.m_ctrlFilterOnOpen = new System.Windows.Forms.CheckBox();
            this.m_ctrlWarnBinderDuplicates = new System.Windows.Forms.CheckBox();
            this.m_ctrlShowForeignBarcodes = new System.Windows.Forms.CheckBox();
            this.m_ctrlLoadLastCase = new System.Windows.Forms.CheckBox();
            this.m_ctrlLogDiagnostics = new System.Windows.Forms.CheckBox();
            this.m_ctrlEnableDiagnostics = new System.Windows.Forms.CheckBox();
            this.m_ctrlShowErrorMessages = new System.Windows.Forms.CheckBox();
            this.m_ctrlFiltersPage = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.m_ctrlFiltersPrompt3 = new System.Windows.Forms.Label();
            this.m_ctrlFiltersPrompt2 = new System.Windows.Forms.Label();
            this.m_ctrlFiltersPrompt1 = new System.Windows.Forms.Label();
            this.m_ctrlSourceFilterLabel = new System.Windows.Forms.Label();
            this.m_ctrlSourceTypesLabel = new System.Windows.Forms.Label();
            this.m_ctrlSourceFilter = new System.Windows.Forms.TextBox();
            this.m_ctrlSourceTypes = new System.Windows.Forms.ListBox();
            this.m_ctrlProfilesPage = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.m_ctrlViewCodecs = new System.Windows.Forms.Button();
            this.m_ctrlProfileCodecsGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlProfileAudio = new System.Windows.Forms.Label();
            this.m_ctrlProfileVideo = new System.Windows.Forms.Label();
            this.m_ctrlProfilesLabel = new System.Windows.Forms.Label();
            this.m_ctrlPreferred = new System.Windows.Forms.CheckBox();
            this.m_ctrlProfiles = new System.Windows.Forms.ListBox();
            this.m_ctrlUpdatesPage = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.m_ctrlUpdatesAlternateSite = new System.Windows.Forms.TextBox();
            this.m_ctrlUpdatesAlternateSiteLabel = new System.Windows.Forms.Label();
            this.m_ctrlUpdatesPrimarySite = new System.Windows.Forms.TextBox();
            this.m_ctrlUpdatesPrimarySiteLabel = new System.Windows.Forms.Label();
            this.m_ctrlUpdatesGroupId = new System.Windows.Forms.TextBox();
            this.m_ctrlUpdatesGroupIdLabel = new System.Windows.Forms.Label();
            this.m_ctrlCancel = new System.Windows.Forms.Button();
            this.m_ctrlOk = new System.Windows.Forms.Button();
            this.m_ctrlUltraTabs = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.m_ctrlUltraSharedPage = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.m_ctrlShowAudioWaveform = new System.Windows.Forms.CheckBox();
            this.m_ctrlGeneralPage.SuspendLayout();
            this.m_ctrlFiltersPage.SuspendLayout();
            this.m_ctrlProfilesPage.SuspendLayout();
            this.m_ctrlProfileCodecsGroup.SuspendLayout();
            this.m_ctrlUpdatesPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraTabs)).BeginInit();
            this.m_ctrlUltraTabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_ctrlGeneralPage
            // 
            this.m_ctrlGeneralPage.Controls.Add(this.m_ctrlShowAudioWaveform);
            this.m_ctrlGeneralPage.Controls.Add(this.m_ctrlFilterOnOpen);
            this.m_ctrlGeneralPage.Controls.Add(this.m_ctrlWarnBinderDuplicates);
            this.m_ctrlGeneralPage.Controls.Add(this.m_ctrlShowForeignBarcodes);
            this.m_ctrlGeneralPage.Controls.Add(this.m_ctrlLoadLastCase);
            this.m_ctrlGeneralPage.Controls.Add(this.m_ctrlLogDiagnostics);
            this.m_ctrlGeneralPage.Controls.Add(this.m_ctrlEnableDiagnostics);
            this.m_ctrlGeneralPage.Controls.Add(this.m_ctrlShowErrorMessages);
            this.m_ctrlGeneralPage.Location = new System.Drawing.Point(1, 23);
            this.m_ctrlGeneralPage.Name = "m_ctrlGeneralPage";
            this.m_ctrlGeneralPage.Size = new System.Drawing.Size(528, 198);
            // 
            // m_ctrlFilterOnOpen
            // 
            this.m_ctrlFilterOnOpen.Location = new System.Drawing.Point(12, 112);
            this.m_ctrlFilterOnOpen.Name = "m_ctrlFilterOnOpen";
            this.m_ctrlFilterOnOpen.Size = new System.Drawing.Size(180, 16);
            this.m_ctrlFilterOnOpen.TabIndex = 4;
            this.m_ctrlFilterOnOpen.Text = "Load filtered on case open";
            // 
            // m_ctrlWarnBinderDuplicates
            // 
            this.m_ctrlWarnBinderDuplicates.Location = new System.Drawing.Point(12, 88);
            this.m_ctrlWarnBinderDuplicates.Name = "m_ctrlWarnBinderDuplicates";
            this.m_ctrlWarnBinderDuplicates.Size = new System.Drawing.Size(180, 16);
            this.m_ctrlWarnBinderDuplicates.TabIndex = 3;
            this.m_ctrlWarnBinderDuplicates.Text = "Warn duplicate binder entries";
            // 
            // m_ctrlShowForeignBarcodes
            // 
            this.m_ctrlShowForeignBarcodes.Location = new System.Drawing.Point(12, 40);
            this.m_ctrlShowForeignBarcodes.Name = "m_ctrlShowForeignBarcodes";
            this.m_ctrlShowForeignBarcodes.Size = new System.Drawing.Size(180, 16);
            this.m_ctrlShowForeignBarcodes.TabIndex = 1;
            this.m_ctrlShowForeignBarcodes.Text = "Show foreign barcodes";
            // 
            // m_ctrlLoadLastCase
            // 
            this.m_ctrlLoadLastCase.Location = new System.Drawing.Point(12, 16);
            this.m_ctrlLoadLastCase.Name = "m_ctrlLoadLastCase";
            this.m_ctrlLoadLastCase.Size = new System.Drawing.Size(180, 16);
            this.m_ctrlLoadLastCase.TabIndex = 0;
            this.m_ctrlLoadLastCase.Text = "Load last case on startup";
            // 
            // m_ctrlLogDiagnostics
            // 
            this.m_ctrlLogDiagnostics.Location = new System.Drawing.Point(272, 40);
            this.m_ctrlLogDiagnostics.Name = "m_ctrlLogDiagnostics";
            this.m_ctrlLogDiagnostics.Size = new System.Drawing.Size(180, 16);
            this.m_ctrlLogDiagnostics.TabIndex = 6;
            this.m_ctrlLogDiagnostics.Text = "Log diagnostic messages";
            // 
            // m_ctrlEnableDiagnostics
            // 
            this.m_ctrlEnableDiagnostics.Location = new System.Drawing.Point(272, 16);
            this.m_ctrlEnableDiagnostics.Name = "m_ctrlEnableDiagnostics";
            this.m_ctrlEnableDiagnostics.Size = new System.Drawing.Size(180, 16);
            this.m_ctrlEnableDiagnostics.TabIndex = 5;
            this.m_ctrlEnableDiagnostics.Text = "Enable diagnostic messages";
            this.m_ctrlEnableDiagnostics.Click += new System.EventHandler(this.OnClickEnableDiagnostics);
            // 
            // m_ctrlShowErrorMessages
            // 
            this.m_ctrlShowErrorMessages.Location = new System.Drawing.Point(12, 64);
            this.m_ctrlShowErrorMessages.Name = "m_ctrlShowErrorMessages";
            this.m_ctrlShowErrorMessages.Size = new System.Drawing.Size(180, 16);
            this.m_ctrlShowErrorMessages.TabIndex = 2;
            this.m_ctrlShowErrorMessages.Text = "Show error messages";
            // 
            // m_ctrlFiltersPage
            // 
            this.m_ctrlFiltersPage.Controls.Add(this.m_ctrlFiltersPrompt3);
            this.m_ctrlFiltersPage.Controls.Add(this.m_ctrlFiltersPrompt2);
            this.m_ctrlFiltersPage.Controls.Add(this.m_ctrlFiltersPrompt1);
            this.m_ctrlFiltersPage.Controls.Add(this.m_ctrlSourceFilterLabel);
            this.m_ctrlFiltersPage.Controls.Add(this.m_ctrlSourceTypesLabel);
            this.m_ctrlFiltersPage.Controls.Add(this.m_ctrlSourceFilter);
            this.m_ctrlFiltersPage.Controls.Add(this.m_ctrlSourceTypes);
            this.m_ctrlFiltersPage.Location = new System.Drawing.Point(-10000, -10000);
            this.m_ctrlFiltersPage.Name = "m_ctrlFiltersPage";
            this.m_ctrlFiltersPage.Size = new System.Drawing.Size(528, 198);
            // 
            // m_ctrlFiltersPrompt3
            // 
            this.m_ctrlFiltersPrompt3.Location = new System.Drawing.Point(272, 100);
            this.m_ctrlFiltersPrompt3.Name = "m_ctrlFiltersPrompt3";
            this.m_ctrlFiltersPrompt3.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlFiltersPrompt3.TabIndex = 6;
            this.m_ctrlFiltersPrompt3.Text = "filter box (separated by spaces)";
            // 
            // m_ctrlFiltersPrompt2
            // 
            this.m_ctrlFiltersPrompt2.Location = new System.Drawing.Point(272, 84);
            this.m_ctrlFiltersPrompt2.Name = "m_ctrlFiltersPrompt2";
            this.m_ctrlFiltersPrompt2.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlFiltersPrompt2.TabIndex = 5;
            this.m_ctrlFiltersPrompt2.Text = "enter valid file extensions in the";
            // 
            // m_ctrlFiltersPrompt1
            // 
            this.m_ctrlFiltersPrompt1.Location = new System.Drawing.Point(272, 68);
            this.m_ctrlFiltersPrompt1.Name = "m_ctrlFiltersPrompt1";
            this.m_ctrlFiltersPrompt1.Size = new System.Drawing.Size(168, 16);
            this.m_ctrlFiltersPrompt1.TabIndex = 4;
            this.m_ctrlFiltersPrompt1.Text = "Select source type from list and";
            // 
            // m_ctrlSourceFilterLabel
            // 
            this.m_ctrlSourceFilterLabel.Location = new System.Drawing.Point(272, 12);
            this.m_ctrlSourceFilterLabel.Name = "m_ctrlSourceFilterLabel";
            this.m_ctrlSourceFilterLabel.Size = new System.Drawing.Size(152, 16);
            this.m_ctrlSourceFilterLabel.TabIndex = 3;
            this.m_ctrlSourceFilterLabel.Text = "Source Filter";
            // 
            // m_ctrlSourceTypesLabel
            // 
            this.m_ctrlSourceTypesLabel.Location = new System.Drawing.Point(8, 12);
            this.m_ctrlSourceTypesLabel.Name = "m_ctrlSourceTypesLabel";
            this.m_ctrlSourceTypesLabel.Size = new System.Drawing.Size(152, 16);
            this.m_ctrlSourceTypesLabel.TabIndex = 2;
            this.m_ctrlSourceTypesLabel.Text = "Source Types";
            // 
            // m_ctrlSourceFilter
            // 
            this.m_ctrlSourceFilter.Location = new System.Drawing.Point(268, 28);
            this.m_ctrlSourceFilter.Name = "m_ctrlSourceFilter";
            this.m_ctrlSourceFilter.Size = new System.Drawing.Size(248, 20);
            this.m_ctrlSourceFilter.TabIndex = 1;
            // 
            // m_ctrlSourceTypes
            // 
            this.m_ctrlSourceTypes.Location = new System.Drawing.Point(8, 28);
            this.m_ctrlSourceTypes.Name = "m_ctrlSourceTypes";
            this.m_ctrlSourceTypes.Size = new System.Drawing.Size(248, 160);
            this.m_ctrlSourceTypes.TabIndex = 0;
            this.m_ctrlSourceTypes.SelectedIndexChanged += new System.EventHandler(this.OnSourceTypeChanged);
            // 
            // m_ctrlProfilesPage
            // 
            this.m_ctrlProfilesPage.Controls.Add(this.m_ctrlViewCodecs);
            this.m_ctrlProfilesPage.Controls.Add(this.m_ctrlProfileCodecsGroup);
            this.m_ctrlProfilesPage.Controls.Add(this.m_ctrlProfilesLabel);
            this.m_ctrlProfilesPage.Controls.Add(this.m_ctrlPreferred);
            this.m_ctrlProfilesPage.Controls.Add(this.m_ctrlProfiles);
            this.m_ctrlProfilesPage.Location = new System.Drawing.Point(-10000, -10000);
            this.m_ctrlProfilesPage.Name = "m_ctrlProfilesPage";
            this.m_ctrlProfilesPage.Size = new System.Drawing.Size(528, 198);
            // 
            // m_ctrlViewCodecs
            // 
            this.m_ctrlViewCodecs.Location = new System.Drawing.Point(340, 168);
            this.m_ctrlViewCodecs.Name = "m_ctrlViewCodecs";
            this.m_ctrlViewCodecs.Size = new System.Drawing.Size(180, 23);
            this.m_ctrlViewCodecs.TabIndex = 7;
            this.m_ctrlViewCodecs.Text = "View Codecs";
            this.m_ctrlViewCodecs.Click += new System.EventHandler(this.OnClickViewCodecs);
            // 
            // m_ctrlProfileCodecsGroup
            // 
            this.m_ctrlProfileCodecsGroup.Controls.Add(this.m_ctrlProfileAudio);
            this.m_ctrlProfileCodecsGroup.Controls.Add(this.m_ctrlProfileVideo);
            this.m_ctrlProfileCodecsGroup.Location = new System.Drawing.Point(340, 24);
            this.m_ctrlProfileCodecsGroup.Name = "m_ctrlProfileCodecsGroup";
            this.m_ctrlProfileCodecsGroup.Size = new System.Drawing.Size(180, 96);
            this.m_ctrlProfileCodecsGroup.TabIndex = 3;
            this.m_ctrlProfileCodecsGroup.TabStop = false;
            this.m_ctrlProfileCodecsGroup.Text = "Encoders";
            // 
            // m_ctrlProfileAudio
            // 
            this.m_ctrlProfileAudio.Location = new System.Drawing.Point(8, 64);
            this.m_ctrlProfileAudio.Name = "m_ctrlProfileAudio";
            this.m_ctrlProfileAudio.Size = new System.Drawing.Size(164, 28);
            this.m_ctrlProfileAudio.TabIndex = 1;
            // 
            // m_ctrlProfileVideo
            // 
            this.m_ctrlProfileVideo.Location = new System.Drawing.Point(8, 24);
            this.m_ctrlProfileVideo.Name = "m_ctrlProfileVideo";
            this.m_ctrlProfileVideo.Size = new System.Drawing.Size(164, 28);
            this.m_ctrlProfileVideo.TabIndex = 0;
            // 
            // m_ctrlProfilesLabel
            // 
            this.m_ctrlProfilesLabel.Location = new System.Drawing.Point(8, 8);
            this.m_ctrlProfilesLabel.Name = "m_ctrlProfilesLabel";
            this.m_ctrlProfilesLabel.Size = new System.Drawing.Size(152, 12);
            this.m_ctrlProfilesLabel.TabIndex = 2;
            this.m_ctrlProfilesLabel.Text = "WMV Encoder Profiles";
            // 
            // m_ctrlPreferred
            // 
            this.m_ctrlPreferred.Location = new System.Drawing.Point(344, 132);
            this.m_ctrlPreferred.Name = "m_ctrlPreferred";
            this.m_ctrlPreferred.Size = new System.Drawing.Size(148, 24);
            this.m_ctrlPreferred.TabIndex = 1;
            this.m_ctrlPreferred.Text = "Preferred";
            // 
            // m_ctrlProfiles
            // 
            this.m_ctrlProfiles.HorizontalScrollbar = true;
            this.m_ctrlProfiles.IntegralHeight = false;
            this.m_ctrlProfiles.Location = new System.Drawing.Point(8, 24);
            this.m_ctrlProfiles.Name = "m_ctrlProfiles";
            this.m_ctrlProfiles.Size = new System.Drawing.Size(324, 168);
            this.m_ctrlProfiles.TabIndex = 0;
            this.m_ctrlProfiles.SelectedIndexChanged += new System.EventHandler(this.OnProfileSelChanged);
            // 
            // m_ctrlUpdatesPage
            // 
            this.m_ctrlUpdatesPage.Controls.Add(this.m_ctrlUpdatesAlternateSite);
            this.m_ctrlUpdatesPage.Controls.Add(this.m_ctrlUpdatesAlternateSiteLabel);
            this.m_ctrlUpdatesPage.Controls.Add(this.m_ctrlUpdatesPrimarySite);
            this.m_ctrlUpdatesPage.Controls.Add(this.m_ctrlUpdatesPrimarySiteLabel);
            this.m_ctrlUpdatesPage.Controls.Add(this.m_ctrlUpdatesGroupId);
            this.m_ctrlUpdatesPage.Controls.Add(this.m_ctrlUpdatesGroupIdLabel);
            this.m_ctrlUpdatesPage.Location = new System.Drawing.Point(-10000, -10000);
            this.m_ctrlUpdatesPage.Name = "m_ctrlUpdatesPage";
            this.m_ctrlUpdatesPage.Size = new System.Drawing.Size(528, 198);
            // 
            // m_ctrlUpdatesAlternateSite
            // 
            this.m_ctrlUpdatesAlternateSite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlUpdatesAlternateSite.Location = new System.Drawing.Point(88, 40);
            this.m_ctrlUpdatesAlternateSite.Name = "m_ctrlUpdatesAlternateSite";
            this.m_ctrlUpdatesAlternateSite.Size = new System.Drawing.Size(432, 20);
            this.m_ctrlUpdatesAlternateSite.TabIndex = 1;
            // 
            // m_ctrlUpdatesAlternateSiteLabel
            // 
            this.m_ctrlUpdatesAlternateSiteLabel.Location = new System.Drawing.Point(8, 40);
            this.m_ctrlUpdatesAlternateSiteLabel.Name = "m_ctrlUpdatesAlternateSiteLabel";
            this.m_ctrlUpdatesAlternateSiteLabel.Size = new System.Drawing.Size(80, 20);
            this.m_ctrlUpdatesAlternateSiteLabel.TabIndex = 4;
            this.m_ctrlUpdatesAlternateSiteLabel.Text = "Alternate Site:";
            this.m_ctrlUpdatesAlternateSiteLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_ctrlUpdatesPrimarySite
            // 
            this.m_ctrlUpdatesPrimarySite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlUpdatesPrimarySite.Location = new System.Drawing.Point(88, 12);
            this.m_ctrlUpdatesPrimarySite.Name = "m_ctrlUpdatesPrimarySite";
            this.m_ctrlUpdatesPrimarySite.Size = new System.Drawing.Size(432, 20);
            this.m_ctrlUpdatesPrimarySite.TabIndex = 0;
            // 
            // m_ctrlUpdatesPrimarySiteLabel
            // 
            this.m_ctrlUpdatesPrimarySiteLabel.Location = new System.Drawing.Point(8, 12);
            this.m_ctrlUpdatesPrimarySiteLabel.Name = "m_ctrlUpdatesPrimarySiteLabel";
            this.m_ctrlUpdatesPrimarySiteLabel.Size = new System.Drawing.Size(80, 20);
            this.m_ctrlUpdatesPrimarySiteLabel.TabIndex = 2;
            this.m_ctrlUpdatesPrimarySiteLabel.Text = "Primary Site:";
            this.m_ctrlUpdatesPrimarySiteLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_ctrlUpdatesGroupId
            // 
            this.m_ctrlUpdatesGroupId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlUpdatesGroupId.Location = new System.Drawing.Point(88, 68);
            this.m_ctrlUpdatesGroupId.Name = "m_ctrlUpdatesGroupId";
            this.m_ctrlUpdatesGroupId.Size = new System.Drawing.Size(432, 20);
            this.m_ctrlUpdatesGroupId.TabIndex = 2;
            // 
            // m_ctrlUpdatesGroupIdLabel
            // 
            this.m_ctrlUpdatesGroupIdLabel.Location = new System.Drawing.Point(8, 68);
            this.m_ctrlUpdatesGroupIdLabel.Name = "m_ctrlUpdatesGroupIdLabel";
            this.m_ctrlUpdatesGroupIdLabel.Size = new System.Drawing.Size(80, 20);
            this.m_ctrlUpdatesGroupIdLabel.TabIndex = 0;
            this.m_ctrlUpdatesGroupIdLabel.Text = "Group Id:";
            this.m_ctrlUpdatesGroupIdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_ctrlCancel
            // 
            this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_ctrlCancel.Location = new System.Drawing.Point(458, 242);
            this.m_ctrlCancel.Name = "m_ctrlCancel";
            this.m_ctrlCancel.Size = new System.Drawing.Size(75, 23);
            this.m_ctrlCancel.TabIndex = 12;
            this.m_ctrlCancel.Text = "  &Cancel";
            // 
            // m_ctrlOk
            // 
            this.m_ctrlOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_ctrlOk.Location = new System.Drawing.Point(374, 242);
            this.m_ctrlOk.Name = "m_ctrlOk";
            this.m_ctrlOk.Size = new System.Drawing.Size(75, 23);
            this.m_ctrlOk.TabIndex = 11;
            this.m_ctrlOk.Text = "&OK";
            this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
            // 
            // m_ctrlUltraTabs
            // 
            this.m_ctrlUltraTabs.Controls.Add(this.m_ctrlUltraSharedPage);
            this.m_ctrlUltraTabs.Controls.Add(this.m_ctrlGeneralPage);
            this.m_ctrlUltraTabs.Controls.Add(this.m_ctrlFiltersPage);
            this.m_ctrlUltraTabs.Controls.Add(this.m_ctrlUpdatesPage);
            this.m_ctrlUltraTabs.Controls.Add(this.m_ctrlProfilesPage);
            this.m_ctrlUltraTabs.Location = new System.Drawing.Point(8, 8);
            this.m_ctrlUltraTabs.Name = "m_ctrlUltraTabs";
            this.m_ctrlUltraTabs.SharedControlsPage = this.m_ctrlUltraSharedPage;
            this.m_ctrlUltraTabs.Size = new System.Drawing.Size(532, 224);
            this.m_ctrlUltraTabs.TabIndex = 1;
            ultraTab1.TabPage = this.m_ctrlGeneralPage;
            ultraTab1.Text = "General";
            ultraTab1.ToolTipText = "General Application Options";
            ultraTab2.TabPage = this.m_ctrlFiltersPage;
            ultraTab2.Text = "Filters";
            ultraTab2.ToolTipText = "Registration Source Filters";
            ultraTab3.TabPage = this.m_ctrlProfilesPage;
            ultraTab3.Text = "Profiles";
            ultraTab3.ToolTipText = "Set Preferred Media Encoder Profiles";
            ultraTab4.TabPage = this.m_ctrlUpdatesPage;
            ultraTab4.Text = "Updates";
            ultraTab4.ToolTipText = "On-Line Updates Setup";
            this.m_ctrlUltraTabs.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2,
            ultraTab3,
            ultraTab4});
            // 
            // m_ctrlUltraSharedPage
            // 
            this.m_ctrlUltraSharedPage.Location = new System.Drawing.Point(-10000, -10000);
            this.m_ctrlUltraSharedPage.Name = "m_ctrlUltraSharedPage";
            this.m_ctrlUltraSharedPage.Size = new System.Drawing.Size(528, 198);
            // 
            // m_ctrlShowAudioWaveform
            // 
            this.m_ctrlShowAudioWaveform.AutoSize = true;
            this.m_ctrlShowAudioWaveform.Location = new System.Drawing.Point(272, 64);
            this.m_ctrlShowAudioWaveform.Name = "m_ctrlShowAudioWaveform";
            this.m_ctrlShowAudioWaveform.Size = new System.Drawing.Size(199, 17);
            this.m_ctrlShowAudioWaveform.TabIndex = 7;
            this.m_ctrlShowAudioWaveform.Text = "Show audio waveform in tuner pane";
            this.m_ctrlShowAudioWaveform.UseVisualStyleBackColor = true;
            this.m_ctrlShowAudioWaveform.CheckedChanged += new System.EventHandler(this.onClickShowAudioWaveform);
            // 
            // CFManagerOptions
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(546, 271);
            this.Controls.Add(this.m_ctrlUltraTabs);
            this.Controls.Add(this.m_ctrlCancel);
            this.Controls.Add(this.m_ctrlOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CFManagerOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " Manager Options";
            this.m_ctrlGeneralPage.ResumeLayout(false);
            this.m_ctrlGeneralPage.PerformLayout();
            this.m_ctrlFiltersPage.ResumeLayout(false);
            this.m_ctrlFiltersPage.PerformLayout();
            this.m_ctrlProfilesPage.ResumeLayout(false);
            this.m_ctrlProfileCodecsGroup.ResumeLayout(false);
            this.m_ctrlUpdatesPage.ResumeLayout(false);
            this.m_ctrlUpdatesPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraTabs)).EndInit();
            this.m_ctrlUltraTabs.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		
		/// <summary>This method sets the source filter text based on the current selection in the list box</summary>
		/// <param name="bChangeSelection">True to update the filter for the current selection</param>
		/// <param name="bUpdateCurrent">True to change the current selection</param>
		void SetSourceFilter(bool bUpdateCurrent, bool bChangeSelection)
		{
			//	Do we have an active filter to be updated?
			if((bUpdateCurrent == true) && (m_tmaxSourceType != null))
			{
				if(m_ctrlSourceFilter.Text.Length > 0)
					m_tmaxSourceType.SetExtensions(m_ctrlSourceFilter.Text);
			}
			
			//	Had the selection changed?
			if(bChangeSelection == false) return;
			
			//	Do we have a current selection?
			if(m_ctrlSourceTypes.SelectedItem != null)
			{
				m_tmaxSourceType = m_tmaxSourceTypes.Find((RegSourceTypes)m_ctrlSourceTypes.SelectedItem);
			}
			else
			{
				m_tmaxSourceType = null;
			}
			
			//	Do we have an active source type?
			if(m_tmaxSourceType != null)
				m_ctrlSourceFilter.Text = m_tmaxSourceType.GetFileFilterString(" ", false);
			else
				m_ctrlSourceFilter.Text = "";
				
		}// void SetSourceFilter()
		
		/// <summary>This method handles events fired when the user selects a new source media type</summary>
		/// <param name="sender">The source media types list box</param>
		/// <param name="e">System parameters</param>
		private void OnSourceTypeChanged(object sender, System.EventArgs e)
		{
			//	Update the filter information
			SetSourceFilter(true, true);
		}
		
		/// <summary>This method fills the list of encoder profiles</summary>
		/// <returns>true if successful</returns>
		private bool FillProfiles()
		{
			//	Do we have the required objects?
			if(m_wmEncoder == null) return false;
			if(m_wmEncoder.Profiles == null) return false;
			if(m_wmEncoder.Profiles.Count == 0) return false;
				
			//	Clear the existing list
			m_wmProfile = null;
			m_ctrlProfiles.Items.Clear();
			
			//	Make sure the profile objects are up to date
			m_wmEncoder.Profiles.SetPreferred(m_wmEncoder.PreferredProfiles);
			
			//	Now populate the list box
			foreach(CWMProfile O in m_wmEncoder.Profiles)
				m_ctrlProfiles.Items.Add(O);
				
			//	Should we set the initial selection?
			if(m_ctrlProfiles.Items.Count > 0)
			{
				m_ctrlProfiles.SelectedIndex = 0;
				OnProfileSelChanged(m_ctrlProfiles, System.EventArgs.Empty);
			}
			else
			{
			}
			
			return (m_ctrlProfiles.Items.Count > 0);
			
		}// private bool FillProfiles()

		/// <summary>Called when the user changes the profile selection</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Event argument object</param>
		private void OnProfileSelChanged(object sender, System.EventArgs e)
		{
			try
			{
				//	Update the existing profile
				if(m_wmProfile != null)
					m_wmProfile.Preferred = m_ctrlPreferred.Checked;
			
				//	Get the new profile
				m_wmProfile = (CWMProfile)(m_ctrlProfiles.SelectedItem);
			
				if(m_wmProfile != null)
				{
					m_ctrlPreferred.Enabled = true;
					m_ctrlPreferred.Checked = m_wmProfile.Preferred;
					m_ctrlProfileVideo.Text = m_wmProfile.GetVideoCodec();
					m_ctrlProfileAudio.Text = m_wmProfile.GetAudioCodec();
					m_ctrlProfileCodecsGroup.Enabled = true;
				}
				else
				{
					m_ctrlPreferred.Enabled = false;
					m_ctrlPreferred.Checked = false;
					m_ctrlProfileVideo.Text = "";
					m_ctrlProfileAudio.Text = "";
					m_ctrlProfileCodecsGroup.Enabled = false;
				}
				 
			}
			catch
			{
			}
			
		}// private void OnProfileSelChanged(object sender, System.EventArgs e)
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Global TmaxManager application options</summary>
		public CTmaxManagerOptions ManagerOptions
		{
			get { return m_tmaxManagerOptions; }
			set { m_tmaxManagerOptions = value; }
		}
		
		/// <summary>The application's product descriptor</summary>
		public FTI.Shared.Trialmax.CTmaxProductManager ProductManager
		{
			get { return m_tmaxProductManager; }
			set { m_tmaxProductManager = value; }
		}
		
		/// <summary>Global TmaxManager source types</summary>
		public CTmaxSourceTypes SourceTypes
		{
			get { return m_tmaxSourceTypes; }
			set { m_tmaxSourceTypes = value; }
		}
		
		/// <summary>Global TmaxManager Windows Media Encoder wrapper</summary>
		public FTI.Trialmax.Encode.CWMEncoder WMEncoder
		{
			get { return m_wmEncoder; }
			set { m_wmEncoder = value; }
		}
		
		#endregion Properties

        /// <summary>This method is called when the user clicks on the ShowAudioWaveform check box</summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The event arguments</param>
        private void onClickShowAudioWaveform(object sender, EventArgs e)
        {
            // Enable/disable the ShowAudioWaveform check box
            //m_ctrlShowAudioWaveform.Enabled = m_ctrlShowAudioWaveform.Checked;
        }
	
	}// public class CFManagerOptions : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Forms
