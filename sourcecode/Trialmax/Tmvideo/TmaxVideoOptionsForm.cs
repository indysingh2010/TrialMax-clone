using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Encode;

namespace FTI.Trialmax.TMVV.Tmvideo
{
	/// <summary>This form allows users to set the Manager application options</summary>
	public class CFTmaxVideoOptions : CFTmaxVideoForm
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_EXCHANGE_EX = (ERROR_TMAX_VIDEO_FORM_MAX + 1);
		
		#endregion Constants

		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components;
		
		/// <summary>Tab control to manage the property pages</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabControl m_ctrlUltraTabs;
		
		/// <summary>Property page shared by all tabs</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage m_ctrlUltraSharedPage;
		
		/// <summary>Property page for General application options</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlGeneralPage;
		
		/// <summary>Property page for remote update options</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlUpdatesPage;

		/// <summary>Property page for video encoder profiles</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlProfilesPage;
		
		/// <summary>Property page for highlighter setup</summary>
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl m_ctrlHighlighterPage;

		/// <summary>The form's Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>The form's Accept button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		
		/// <summary>Private member bound to VideoOptions property</summary>
		private CTmaxVideoOptions m_tmaxVideoOptions = null;
		
		/// <summary>Private member bound to SourceTypes property</summary>
		private FTI.Shared.Trialmax.CTmaxSourceTypes m_tmaxSourceTypes = null;
		
		/// <summary>Local member bound to the ProductManager property</summary>
		private FTI.Shared.Trialmax.CTmaxProductManager m_tmaxProductManager = null;
		
		/// <summary>Private member bound to WMEncoder property</summary>
		private FTI.Trialmax.Encode.CWMEncoder m_tmaxEncoder = null;
		
		/// <summary>Check box to allow user to request loading of the last used file</summary>
		private System.Windows.Forms.CheckBox m_ctrlLoadLast;
		
		/// <summary>Check box to allow user to request logging of diagnostic messages</summary>
		private System.Windows.Forms.CheckBox m_ctrlLogDiagnostics;
		
		/// <summary>Check box to allow user to request display of diagnostic messages</summary>
		private System.Windows.Forms.CheckBox m_ctrlEnableDiagnostics;
		
		/// <summary>Check box to allow user to request display of error messages</summary>
		private System.Windows.Forms.CheckBox m_ctrlShowErrorMessages;
		
		/// <summary>Text box to allow specification of alternate updates site</summary>
		private System.Windows.Forms.TextBox m_ctrlUpdatesAlternateSite;
		
		/// <summary>Label for alternate update site edit box</summary>
		private System.Windows.Forms.Label m_ctrlUpdatesAlternateSiteLabel;
		
		/// <summary>Text box to allow specification of primary updates site</summary>
		private System.Windows.Forms.TextBox m_ctrlUpdatesPrimarySite;
		
		/// <summary>Label for primary update site edit box</summary>
		private System.Windows.Forms.Label m_ctrlUpdatesPrimarySiteLabel;
		
		/// <summary>Text box to specify updates group identifier</summary>
		private System.Windows.Forms.TextBox m_ctrlUpdatesGroupId;
		
		/// <summary>Label for update group id edit box</summary>
		private System.Windows.Forms.Label m_ctrlUpdatesGroupIdLabel;
		
		/// <summary>Pushbutton to request viewing of available codecs</summary>
		private System.Windows.Forms.Button m_ctrlViewCodecs;
		
		/// <summary>Group box for profile codec controls</summary>
		private System.Windows.Forms.GroupBox m_ctrlProfileCodecsGroup;
		
		/// <summary>Static text to display profile's audio codec</summary>
		private System.Windows.Forms.Label m_ctrlProfileAudio;
		
		/// <summary>Static text to display profile's video codec</summary>
		private System.Windows.Forms.Label m_ctrlProfileVideo;
		
		/// <summary>List box to display encoder profiles</summary>
		private System.Windows.Forms.ListBox m_ctrlProfiles;

		/// <summary>Label for encoder profiles list box</summary>
		private System.Windows.Forms.Label m_ctrlProfilesLabel;

		/// <summary>Check box to show only preferred profiles</summary>
		private System.Windows.Forms.CheckBox m_ctrlPreferred;

		/// <summary>Text box to enter the threshold for video pause indicators</summary>
		private System.Windows.Forms.TextBox m_ctrlPauseThreshold;

		/// <summary>Static text label for Pause Threshold edit box</summary>
		private System.Windows.Forms.Label m_ctrlPauseThresholdLabel;

		/// <summary>Pushbutton to set color for highlighter 7</summary>
		private System.Windows.Forms.Button m_ctrlHighlighterColor7;

		/// <summary>Pushbutton to set color for highlighter 6</summary>
		private System.Windows.Forms.Button m_ctrlHighlighterColor6;

		/// <summary>Pushbutton to set color for highlighter 5</summary>
		private System.Windows.Forms.Button m_ctrlHighlighterColor5;

		/// <summary>Pushbutton to set color for highlighter 4</summary>
		private System.Windows.Forms.Button m_ctrlHighlighterColor4;

		/// <summary>Pushbutton to set color for highlighter 3</summary>
		private System.Windows.Forms.Button m_ctrlHighlighterColor3;

		/// <summary>Pushbutton to set color for highlighter 2</summary>
		private System.Windows.Forms.Button m_ctrlHighlighterColor2;

		/// <summary>Pushbutton to set color for highlighter 1</summary>
		private System.Windows.Forms.Button m_ctrlHighlighterColor1;

		/// <summary>Text box to assign label for highlighter 1</summary>
		private System.Windows.Forms.TextBox m_ctrlHighlighterLabel1;

		/// <summary>Text box to assign label for highlighter 2</summary>
		private System.Windows.Forms.TextBox m_ctrlHighlighterLabel2;

		/// <summary>Text box to assign label for highlighter 4</summary>
		private System.Windows.Forms.TextBox m_ctrlHighlighterLabel4;

		/// <summary>Text box to assign label for highlighter 3</summary>
		private System.Windows.Forms.TextBox m_ctrlHighlighterLabel3;

		/// <summary>Text box to assign label for highlighter 7</summary>
		private System.Windows.Forms.TextBox m_ctrlHighlighterLabel7;

		/// <summary>Text box to assign label for highlighter 6</summary>
		private System.Windows.Forms.TextBox m_ctrlHighlighterLabel6;

		/// <summary>Text box to assign label for highlighter 5</summary>
		private System.Windows.Forms.TextBox m_ctrlHighlighterLabel5;

		/// <summary>Color picker dialog box for selection of highlighter colors</summary>
		private System.Windows.Forms.ColorDialog m_ctrlGetColor;
		
		/// <summary>Pushbutton to browse for video folder</summary>
		private System.Windows.Forms.Button m_ctrlVideoBrowse;
		
		/// <summary>Label for video folder edit control</summary>
		private System.Windows.Forms.Label m_ctrlVideoFolderLabel;
		
		/// <summary>Image list for form button images</summary>
		private System.Windows.Forms.ImageList m_ctrlImages;
		
		/// <summary>Edit box for video folder path</summary>
		private System.Windows.Forms.TextBox m_ctrlVideoFolder;
		
		/// <summary>Private member to keep track of the active profile</summary>
		private FTI.Trialmax.Encode.CWMProfile m_wmProfile = null;
		
		/// <summary>Private member bound to InitialTabIndex property</summary>
		private int m_iInitialTabIndex = 0;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFTmaxVideoOptions()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
			m_tmaxEventSource.Name = "TmaxVideo Options Form";
		}

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
		
		/// <summary>This method is called to get the color button associated with the specified highlighter</summary>
		/// <param name="lId">The id of the desired highlighter</param>
		/// <returns>The associated button</returns>
		private System.Windows.Forms.Button GetHighlighterColorButton(long lId)
		{
			switch(lId)
			{
				case 1:	return m_ctrlHighlighterColor1;
				case 2:	return m_ctrlHighlighterColor2;
				case 3:	return m_ctrlHighlighterColor3;
				case 4:	return m_ctrlHighlighterColor4;
				case 5:	return m_ctrlHighlighterColor5;
				case 6:	return m_ctrlHighlighterColor6;
				case 7:	return m_ctrlHighlighterColor7;
				default:	return null;
			}
			
		}// private System.Windows.Forms.Button GetHighlighterColorButton(long lId)
		
		/// <summary>This method is called to get the label text box associated with the specified highlighter</summary>
		/// <param name="lId">The id of the desired highlighter</param>
		/// <returns>The associated text box</returns>
		private System.Windows.Forms.TextBox GetHighlighterLabelBox(long lId)
		{
			switch(lId)
			{
				case 1:		return m_ctrlHighlighterLabel1;
				case 2:		return m_ctrlHighlighterLabel2;
				case 3:		return m_ctrlHighlighterLabel3;
				case 4:		return m_ctrlHighlighterLabel4;
				case 5:		return m_ctrlHighlighterLabel5;
				case 6:		return m_ctrlHighlighterLabel6;
				case 7:		return m_ctrlHighlighterLabel7;
				default:	return null;
			}
			
		}// private System.Windows.Forms.TextBox GetHighlighterLabelBox(long lId)
		
		/// <summary>This method is called to exchange values between the form control and the local options object</summary>
		/// <param name="bSetControls">true to set the control values</param>
		/// <returns>true if successful</returns>
		private bool Exchange(bool bSetControls)
		{
			try
			{
				if(bSetControls == true)
				{
					m_ctrlLoadLast.Checked = m_tmaxVideoOptions.LoadLast;
					m_ctrlShowErrorMessages.Checked = m_tmaxVideoOptions.ShowErrorMessages;
					m_ctrlEnableDiagnostics.Checked = m_tmaxVideoOptions.EnableDiagnostics;
					m_ctrlLogDiagnostics.Checked = m_tmaxVideoOptions.LogDiagnostics;
					m_ctrlPauseThreshold.Text = m_tmaxVideoOptions.PauseThreshold.ToString();
					m_ctrlVideoFolder.Text = m_tmaxVideoOptions.VideoFolder;
					
					// Enable/disable the LogDiagnostics check box
					m_ctrlLogDiagnostics.Enabled = m_ctrlEnableDiagnostics.Checked;
		
					//	Initialize the highlighter controls
					foreach(CTmaxHighlighter O in m_tmaxVideoOptions.Highlighters)
					{
						if(GetHighlighterColorButton(O.Id) != null)
							GetHighlighterColorButton(O.Id).BackColor = O.Color;
						if(GetHighlighterLabelBox(O.Id) != null)
							GetHighlighterLabelBox(O.Id).Text = O.Label;
					}
					
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
					m_tmaxVideoOptions.ShowErrorMessages = m_ctrlShowErrorMessages.Checked;
					m_tmaxVideoOptions.EnableDiagnostics = m_ctrlEnableDiagnostics.Checked;
					m_tmaxVideoOptions.LogDiagnostics = m_ctrlLogDiagnostics.Checked;
					m_tmaxVideoOptions.LoadLast = m_ctrlLoadLast.Checked;
					m_tmaxVideoOptions.VideoFolder = m_ctrlVideoFolder.Text;
					
					try { m_tmaxVideoOptions.PauseThreshold = System.Convert.ToDouble(m_ctrlPauseThreshold.Text); }
					catch { m_tmaxVideoOptions.PauseThreshold = 0; }
					
					if(m_tmaxProductManager != null)
					{
						m_tmaxProductManager.UpdateSite = m_ctrlUpdatesPrimarySite.Text;
						m_tmaxProductManager.UpdateAlternateSite = m_ctrlUpdatesAlternateSite.Text;
						m_tmaxProductManager.UpdateGroupId = m_ctrlUpdatesGroupId.Text;
					}
					
					//	Read the highlighter controls
					foreach(CTmaxHighlighter O in m_tmaxVideoOptions.Highlighters)
					{
						if(GetHighlighterColorButton(O.Id) != null)
							O.Color = GetHighlighterColorButton(O.Id).BackColor;
						if(GetHighlighterLabelBox(O.Id) != null)
							O.Label = GetHighlighterLabelBox(O.Id).Text;
					}
					
					//	Are we working with the encoder profiles?
					if(m_ctrlProfiles.Items.Count > 0)
					{
						//	Make sure the current selection is up to date
						if(m_wmProfile != null)
							m_wmProfile.Preferred = m_ctrlPreferred.Checked;
							
						//	Update the list of preferred profiles
						if(m_tmaxEncoder.PreferredProfiles != null)
							m_tmaxEncoder.Profiles.GetPreferred(m_tmaxEncoder.PreferredProfiles);
						
					}// if(m_ctrlProfiles.Items.Count > 0)

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

		/// <summary>This method is called when the user clicks on Video Browse button</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickVideoBrowse(object sender, System.EventArgs e)
		{
			try
			{
				FTI.Shared.CBrowseForFolder bff = new CBrowseForFolder();
				
				bff.Prompt = "Select the new folder : ";
				bff.NoNewFolder = false;
				
				if(m_ctrlVideoFolder.Text.Length > 0)
					bff.Folder = m_ctrlVideoFolder.Text;

				if(bff.ShowDialog(this.Handle) == DialogResult.OK)
				{
					m_ctrlVideoFolder.Text = bff.Folder.ToLower();
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnClickVideoBrowse", Ex);
			}
		
		}// private void OnClickVideoBrowse(object sender, System.EventArgs e)

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

		/// <summary>This method is called when the user clicks on one of the color buttons</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickHighlighterColor(object sender, System.EventArgs e)
		{
			Button colorButton = (Button)sender;
			
			m_ctrlGetColor.Color = colorButton.BackColor;
			if(m_ctrlGetColor.ShowDialog() == DialogResult.OK)
				colorButton.BackColor = m_ctrlGetColor.Color;
		}

		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFTmaxVideoOptions));
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			this.m_ctrlGeneralPage = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.m_ctrlVideoBrowse = new System.Windows.Forms.Button();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlVideoFolder = new System.Windows.Forms.TextBox();
			this.m_ctrlVideoFolderLabel = new System.Windows.Forms.Label();
			this.m_ctrlPauseThresholdLabel = new System.Windows.Forms.Label();
			this.m_ctrlPauseThreshold = new System.Windows.Forms.TextBox();
			this.m_ctrlLoadLast = new System.Windows.Forms.CheckBox();
			this.m_ctrlLogDiagnostics = new System.Windows.Forms.CheckBox();
			this.m_ctrlEnableDiagnostics = new System.Windows.Forms.CheckBox();
			this.m_ctrlShowErrorMessages = new System.Windows.Forms.CheckBox();
			this.m_ctrlHighlighterPage = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.m_ctrlHighlighterLabel7 = new System.Windows.Forms.TextBox();
			this.m_ctrlHighlighterLabel6 = new System.Windows.Forms.TextBox();
			this.m_ctrlHighlighterLabel5 = new System.Windows.Forms.TextBox();
			this.m_ctrlHighlighterLabel4 = new System.Windows.Forms.TextBox();
			this.m_ctrlHighlighterLabel3 = new System.Windows.Forms.TextBox();
			this.m_ctrlHighlighterLabel2 = new System.Windows.Forms.TextBox();
			this.m_ctrlHighlighterLabel1 = new System.Windows.Forms.TextBox();
			this.m_ctrlHighlighterColor7 = new System.Windows.Forms.Button();
			this.m_ctrlHighlighterColor6 = new System.Windows.Forms.Button();
			this.m_ctrlHighlighterColor5 = new System.Windows.Forms.Button();
			this.m_ctrlHighlighterColor4 = new System.Windows.Forms.Button();
			this.m_ctrlHighlighterColor3 = new System.Windows.Forms.Button();
			this.m_ctrlHighlighterColor2 = new System.Windows.Forms.Button();
			this.m_ctrlHighlighterColor1 = new System.Windows.Forms.Button();
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
			this.m_ctrlUltraSharedPage = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
			this.m_ctrlUltraTabs = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
			this.m_ctrlGetColor = new System.Windows.Forms.ColorDialog();
			this.m_ctrlGeneralPage.SuspendLayout();
			this.m_ctrlHighlighterPage.SuspendLayout();
			this.m_ctrlProfilesPage.SuspendLayout();
			this.m_ctrlProfileCodecsGroup.SuspendLayout();
			this.m_ctrlUpdatesPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraTabs)).BeginInit();
			this.m_ctrlUltraTabs.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlGeneralPage
			// 
			this.m_ctrlGeneralPage.Controls.Add(this.m_ctrlVideoBrowse);
			this.m_ctrlGeneralPage.Controls.Add(this.m_ctrlVideoFolder);
			this.m_ctrlGeneralPage.Controls.Add(this.m_ctrlVideoFolderLabel);
			this.m_ctrlGeneralPage.Controls.Add(this.m_ctrlPauseThresholdLabel);
			this.m_ctrlGeneralPage.Controls.Add(this.m_ctrlPauseThreshold);
			this.m_ctrlGeneralPage.Controls.Add(this.m_ctrlLoadLast);
			this.m_ctrlGeneralPage.Controls.Add(this.m_ctrlLogDiagnostics);
			this.m_ctrlGeneralPage.Controls.Add(this.m_ctrlEnableDiagnostics);
			this.m_ctrlGeneralPage.Controls.Add(this.m_ctrlShowErrorMessages);
			this.m_ctrlGeneralPage.Location = new System.Drawing.Point(-10000, -10000);
			this.m_ctrlGeneralPage.Name = "m_ctrlGeneralPage";
			this.m_ctrlGeneralPage.Size = new System.Drawing.Size(528, 198);
			// 
			// m_ctrlVideoBrowse
			// 
			this.m_ctrlVideoBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlVideoBrowse.ImageIndex = 0;
			this.m_ctrlVideoBrowse.ImageList = this.m_ctrlImages;
			this.m_ctrlVideoBrowse.Location = new System.Drawing.Point(240, 72);
			this.m_ctrlVideoBrowse.Name = "m_ctrlVideoBrowse";
			this.m_ctrlVideoBrowse.Size = new System.Drawing.Size(24, 20);
			this.m_ctrlVideoBrowse.TabIndex = 11;
			this.m_ctrlVideoBrowse.Click += new System.EventHandler(this.OnClickVideoBrowse);
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.m_ctrlImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// m_ctrlVideoFolder
			// 
			this.m_ctrlVideoFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlVideoFolder.Location = new System.Drawing.Point(88, 72);
			this.m_ctrlVideoFolder.Name = "m_ctrlVideoFolder";
			this.m_ctrlVideoFolder.Size = new System.Drawing.Size(144, 20);
			this.m_ctrlVideoFolder.TabIndex = 2;
			this.m_ctrlVideoFolder.Text = "";
			// 
			// m_ctrlVideoFolderLabel
			// 
			this.m_ctrlVideoFolderLabel.Location = new System.Drawing.Point(16, 72);
			this.m_ctrlVideoFolderLabel.Name = "m_ctrlVideoFolderLabel";
			this.m_ctrlVideoFolderLabel.Size = new System.Drawing.Size(76, 20);
			this.m_ctrlVideoFolderLabel.TabIndex = 9;
			this.m_ctrlVideoFolderLabel.Text = "Video Folder:";
			this.m_ctrlVideoFolderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlPauseThresholdLabel
			// 
			this.m_ctrlPauseThresholdLabel.Location = new System.Drawing.Point(64, 40);
			this.m_ctrlPauseThresholdLabel.Name = "m_ctrlPauseThresholdLabel";
			this.m_ctrlPauseThresholdLabel.Size = new System.Drawing.Size(176, 20);
			this.m_ctrlPauseThresholdLabel.TabIndex = 8;
			this.m_ctrlPauseThresholdLabel.Text = "Pause Indicators Threshold (sec)";
			this.m_ctrlPauseThresholdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlPauseThreshold
			// 
			this.m_ctrlPauseThreshold.Location = new System.Drawing.Point(12, 40);
			this.m_ctrlPauseThreshold.Name = "m_ctrlPauseThreshold";
			this.m_ctrlPauseThreshold.Size = new System.Drawing.Size(48, 20);
			this.m_ctrlPauseThreshold.TabIndex = 1;
			this.m_ctrlPauseThreshold.Text = "";
			// 
			// m_ctrlLoadLast
			// 
			this.m_ctrlLoadLast.Location = new System.Drawing.Point(12, 16);
			this.m_ctrlLoadLast.Name = "m_ctrlLoadLast";
			this.m_ctrlLoadLast.Size = new System.Drawing.Size(180, 16);
			this.m_ctrlLoadLast.TabIndex = 0;
			this.m_ctrlLoadLast.Text = "Load last file on startup";
			// 
			// m_ctrlLogDiagnostics
			// 
			this.m_ctrlLogDiagnostics.Location = new System.Drawing.Point(288, 64);
			this.m_ctrlLogDiagnostics.Name = "m_ctrlLogDiagnostics";
			this.m_ctrlLogDiagnostics.Size = new System.Drawing.Size(180, 16);
			this.m_ctrlLogDiagnostics.TabIndex = 5;
			this.m_ctrlLogDiagnostics.Text = "Log diagnostic messages";
			// 
			// m_ctrlEnableDiagnostics
			// 
			this.m_ctrlEnableDiagnostics.Location = new System.Drawing.Point(288, 40);
			this.m_ctrlEnableDiagnostics.Name = "m_ctrlEnableDiagnostics";
			this.m_ctrlEnableDiagnostics.Size = new System.Drawing.Size(180, 16);
			this.m_ctrlEnableDiagnostics.TabIndex = 4;
			this.m_ctrlEnableDiagnostics.Text = "Enable diagnostic messages";
			this.m_ctrlEnableDiagnostics.Click += new System.EventHandler(this.OnClickEnableDiagnostics);
			// 
			// m_ctrlShowErrorMessages
			// 
			this.m_ctrlShowErrorMessages.Location = new System.Drawing.Point(288, 16);
			this.m_ctrlShowErrorMessages.Name = "m_ctrlShowErrorMessages";
			this.m_ctrlShowErrorMessages.Size = new System.Drawing.Size(180, 16);
			this.m_ctrlShowErrorMessages.TabIndex = 3;
			this.m_ctrlShowErrorMessages.Text = "Show error messages";
			// 
			// m_ctrlHighlighterPage
			// 
			this.m_ctrlHighlighterPage.Controls.Add(this.m_ctrlHighlighterLabel7);
			this.m_ctrlHighlighterPage.Controls.Add(this.m_ctrlHighlighterLabel6);
			this.m_ctrlHighlighterPage.Controls.Add(this.m_ctrlHighlighterLabel5);
			this.m_ctrlHighlighterPage.Controls.Add(this.m_ctrlHighlighterLabel4);
			this.m_ctrlHighlighterPage.Controls.Add(this.m_ctrlHighlighterLabel3);
			this.m_ctrlHighlighterPage.Controls.Add(this.m_ctrlHighlighterLabel2);
			this.m_ctrlHighlighterPage.Controls.Add(this.m_ctrlHighlighterLabel1);
			this.m_ctrlHighlighterPage.Controls.Add(this.m_ctrlHighlighterColor7);
			this.m_ctrlHighlighterPage.Controls.Add(this.m_ctrlHighlighterColor6);
			this.m_ctrlHighlighterPage.Controls.Add(this.m_ctrlHighlighterColor5);
			this.m_ctrlHighlighterPage.Controls.Add(this.m_ctrlHighlighterColor4);
			this.m_ctrlHighlighterPage.Controls.Add(this.m_ctrlHighlighterColor3);
			this.m_ctrlHighlighterPage.Controls.Add(this.m_ctrlHighlighterColor2);
			this.m_ctrlHighlighterPage.Controls.Add(this.m_ctrlHighlighterColor1);
			this.m_ctrlHighlighterPage.Location = new System.Drawing.Point(-10000, -10000);
			this.m_ctrlHighlighterPage.Name = "m_ctrlHighlighterPage";
			this.m_ctrlHighlighterPage.Size = new System.Drawing.Size(528, 198);
			// 
			// m_ctrlHighlighterLabel7
			// 
			this.m_ctrlHighlighterLabel7.Location = new System.Drawing.Point(56, 164);
			this.m_ctrlHighlighterLabel7.Name = "m_ctrlHighlighterLabel7";
			this.m_ctrlHighlighterLabel7.Size = new System.Drawing.Size(456, 20);
			this.m_ctrlHighlighterLabel7.TabIndex = 13;
			this.m_ctrlHighlighterLabel7.Text = "";
			// 
			// m_ctrlHighlighterLabel6
			// 
			this.m_ctrlHighlighterLabel6.Location = new System.Drawing.Point(56, 138);
			this.m_ctrlHighlighterLabel6.Name = "m_ctrlHighlighterLabel6";
			this.m_ctrlHighlighterLabel6.Size = new System.Drawing.Size(456, 20);
			this.m_ctrlHighlighterLabel6.TabIndex = 11;
			this.m_ctrlHighlighterLabel6.Text = "";
			// 
			// m_ctrlHighlighterLabel5
			// 
			this.m_ctrlHighlighterLabel5.Location = new System.Drawing.Point(56, 112);
			this.m_ctrlHighlighterLabel5.Name = "m_ctrlHighlighterLabel5";
			this.m_ctrlHighlighterLabel5.Size = new System.Drawing.Size(456, 20);
			this.m_ctrlHighlighterLabel5.TabIndex = 9;
			this.m_ctrlHighlighterLabel5.Text = "";
			// 
			// m_ctrlHighlighterLabel4
			// 
			this.m_ctrlHighlighterLabel4.Location = new System.Drawing.Point(56, 86);
			this.m_ctrlHighlighterLabel4.Name = "m_ctrlHighlighterLabel4";
			this.m_ctrlHighlighterLabel4.Size = new System.Drawing.Size(456, 20);
			this.m_ctrlHighlighterLabel4.TabIndex = 7;
			this.m_ctrlHighlighterLabel4.Text = "";
			// 
			// m_ctrlHighlighterLabel3
			// 
			this.m_ctrlHighlighterLabel3.Location = new System.Drawing.Point(56, 60);
			this.m_ctrlHighlighterLabel3.Name = "m_ctrlHighlighterLabel3";
			this.m_ctrlHighlighterLabel3.Size = new System.Drawing.Size(456, 20);
			this.m_ctrlHighlighterLabel3.TabIndex = 5;
			this.m_ctrlHighlighterLabel3.Text = "";
			// 
			// m_ctrlHighlighterLabel2
			// 
			this.m_ctrlHighlighterLabel2.Location = new System.Drawing.Point(56, 34);
			this.m_ctrlHighlighterLabel2.Name = "m_ctrlHighlighterLabel2";
			this.m_ctrlHighlighterLabel2.Size = new System.Drawing.Size(456, 20);
			this.m_ctrlHighlighterLabel2.TabIndex = 3;
			this.m_ctrlHighlighterLabel2.Text = "";
			// 
			// m_ctrlHighlighterLabel1
			// 
			this.m_ctrlHighlighterLabel1.Location = new System.Drawing.Point(56, 8);
			this.m_ctrlHighlighterLabel1.Name = "m_ctrlHighlighterLabel1";
			this.m_ctrlHighlighterLabel1.Size = new System.Drawing.Size(456, 20);
			this.m_ctrlHighlighterLabel1.TabIndex = 1;
			this.m_ctrlHighlighterLabel1.Text = "";
			// 
			// m_ctrlHighlighterColor7
			// 
			this.m_ctrlHighlighterColor7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_ctrlHighlighterColor7.Location = new System.Drawing.Point(16, 164);
			this.m_ctrlHighlighterColor7.Name = "m_ctrlHighlighterColor7";
			this.m_ctrlHighlighterColor7.Size = new System.Drawing.Size(32, 21);
			this.m_ctrlHighlighterColor7.TabIndex = 12;
			this.m_ctrlHighlighterColor7.Click += new System.EventHandler(this.OnClickHighlighterColor);
			// 
			// m_ctrlHighlighterColor6
			// 
			this.m_ctrlHighlighterColor6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_ctrlHighlighterColor6.Location = new System.Drawing.Point(16, 138);
			this.m_ctrlHighlighterColor6.Name = "m_ctrlHighlighterColor6";
			this.m_ctrlHighlighterColor6.Size = new System.Drawing.Size(32, 21);
			this.m_ctrlHighlighterColor6.TabIndex = 10;
			this.m_ctrlHighlighterColor6.Click += new System.EventHandler(this.OnClickHighlighterColor);
			// 
			// m_ctrlHighlighterColor5
			// 
			this.m_ctrlHighlighterColor5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_ctrlHighlighterColor5.Location = new System.Drawing.Point(16, 112);
			this.m_ctrlHighlighterColor5.Name = "m_ctrlHighlighterColor5";
			this.m_ctrlHighlighterColor5.Size = new System.Drawing.Size(32, 21);
			this.m_ctrlHighlighterColor5.TabIndex = 8;
			this.m_ctrlHighlighterColor5.Click += new System.EventHandler(this.OnClickHighlighterColor);
			// 
			// m_ctrlHighlighterColor4
			// 
			this.m_ctrlHighlighterColor4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_ctrlHighlighterColor4.Location = new System.Drawing.Point(16, 86);
			this.m_ctrlHighlighterColor4.Name = "m_ctrlHighlighterColor4";
			this.m_ctrlHighlighterColor4.Size = new System.Drawing.Size(32, 21);
			this.m_ctrlHighlighterColor4.TabIndex = 6;
			this.m_ctrlHighlighterColor4.Click += new System.EventHandler(this.OnClickHighlighterColor);
			// 
			// m_ctrlHighlighterColor3
			// 
			this.m_ctrlHighlighterColor3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_ctrlHighlighterColor3.Location = new System.Drawing.Point(16, 60);
			this.m_ctrlHighlighterColor3.Name = "m_ctrlHighlighterColor3";
			this.m_ctrlHighlighterColor3.Size = new System.Drawing.Size(32, 21);
			this.m_ctrlHighlighterColor3.TabIndex = 4;
			this.m_ctrlHighlighterColor3.Click += new System.EventHandler(this.OnClickHighlighterColor);
			// 
			// m_ctrlHighlighterColor2
			// 
			this.m_ctrlHighlighterColor2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_ctrlHighlighterColor2.Location = new System.Drawing.Point(16, 34);
			this.m_ctrlHighlighterColor2.Name = "m_ctrlHighlighterColor2";
			this.m_ctrlHighlighterColor2.Size = new System.Drawing.Size(32, 21);
			this.m_ctrlHighlighterColor2.TabIndex = 2;
			this.m_ctrlHighlighterColor2.Click += new System.EventHandler(this.OnClickHighlighterColor);
			// 
			// m_ctrlHighlighterColor1
			// 
			this.m_ctrlHighlighterColor1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.m_ctrlHighlighterColor1.Location = new System.Drawing.Point(16, 8);
			this.m_ctrlHighlighterColor1.Name = "m_ctrlHighlighterColor1";
			this.m_ctrlHighlighterColor1.Size = new System.Drawing.Size(32, 21);
			this.m_ctrlHighlighterColor1.TabIndex = 0;
			this.m_ctrlHighlighterColor1.Click += new System.EventHandler(this.OnClickHighlighterColor);
			// 
			// m_ctrlProfilesPage
			// 
			this.m_ctrlProfilesPage.Controls.Add(this.m_ctrlViewCodecs);
			this.m_ctrlProfilesPage.Controls.Add(this.m_ctrlProfileCodecsGroup);
			this.m_ctrlProfilesPage.Controls.Add(this.m_ctrlProfilesLabel);
			this.m_ctrlProfilesPage.Controls.Add(this.m_ctrlPreferred);
			this.m_ctrlProfilesPage.Controls.Add(this.m_ctrlProfiles);
			this.m_ctrlProfilesPage.Location = new System.Drawing.Point(2, 24);
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
			this.m_ctrlUpdatesAlternateSite.Text = "";
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
			this.m_ctrlUpdatesPrimarySite.Text = "";
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
			this.m_ctrlUpdatesGroupId.Text = "";
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
			this.m_ctrlCancel.TabIndex = 12;
			this.m_ctrlCancel.Text = "  &Cancel";
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(374, 242);
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
			this.m_ctrlUltraTabs.Controls.Add(this.m_ctrlUltraSharedPage);
			this.m_ctrlUltraTabs.Controls.Add(this.m_ctrlGeneralPage);
			this.m_ctrlUltraTabs.Controls.Add(this.m_ctrlUpdatesPage);
			this.m_ctrlUltraTabs.Controls.Add(this.m_ctrlProfilesPage);
			this.m_ctrlUltraTabs.Controls.Add(this.m_ctrlHighlighterPage);
			this.m_ctrlUltraTabs.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlUltraTabs.Name = "m_ctrlUltraTabs";
			this.m_ctrlUltraTabs.SharedControlsPage = this.m_ctrlUltraSharedPage;
			this.m_ctrlUltraTabs.Size = new System.Drawing.Size(532, 224);
			this.m_ctrlUltraTabs.TabIndex = 0;
			ultraTab1.TabPage = this.m_ctrlGeneralPage;
			ultraTab1.Text = "General";
			ultraTab1.ToolTipText = "General Application Options";
			ultraTab2.TabPage = this.m_ctrlHighlighterPage;
			ultraTab2.Text = "Highlighters";
			ultraTab3.TabPage = this.m_ctrlUpdatesPage;
			ultraTab3.Text = "Updates";
			ultraTab3.ToolTipText = "On-Line Updates Setup";
			ultraTab4.TabPage = this.m_ctrlProfilesPage;
			ultraTab4.Text = "Profiles";
			ultraTab4.ToolTipText = "Set Preferred Media Encoder Profiles";
			this.m_ctrlUltraTabs.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
																									  ultraTab1,
																									  ultraTab2,
																									  ultraTab3,
																									  ultraTab4});
			// 
			// CFTmaxVideoOptions
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
			this.Name = "CFTmaxVideoOptions";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Video Viewer Options";
			this.m_ctrlGeneralPage.ResumeLayout(false);
			this.m_ctrlHighlighterPage.ResumeLayout(false);
			this.m_ctrlProfilesPage.ResumeLayout(false);
			this.m_ctrlProfileCodecsGroup.ResumeLayout(false);
			this.m_ctrlUpdatesPage.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraTabs)).EndInit();
			this.m_ctrlUltraTabs.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		
		/// <summary>This method fills the list of encoder profiles</summary>
		/// <returns>true if successful</returns>
		private bool FillProfiles()
		{
			//	Do we have the required objects?
			if(m_tmaxEncoder == null) return false;
			if(m_tmaxEncoder.Profiles == null) return false;
			if(m_tmaxEncoder.Profiles.Count == 0) return false;
				
			//	Clear the existing list
			m_wmProfile = null;
			m_ctrlProfiles.Items.Clear();
			
			//	Make sure the profile objects are up to date
			m_tmaxEncoder.Profiles.SetPreferred(m_tmaxEncoder.PreferredProfiles);
			
			//	Now populate the list box
			foreach(CWMProfile O in m_tmaxEncoder.Profiles)
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
		
		/// <summary>Global TmaxVideo application options</summary>
		public CTmaxVideoOptions VideoOptions
		{
			get { return m_tmaxVideoOptions; }
			set { m_tmaxVideoOptions = value; }
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
		
		/// <summary>The application's Windows Media Encoder wrapper</summary>
		public FTI.Trialmax.Encode.CWMEncoder TmaxEncoder
		{
			get { return m_tmaxEncoder; }
			set { m_tmaxEncoder = value; }
		}
		
		/// <summary>Tab to be visible when the form opens</summary>
		public int InitialTabIndex
		{
			get { return m_iInitialTabIndex; }
			set { m_iInitialTabIndex = value; }
		}
		
		#endregion Properties
	
	}// public class CFTmaxVideoOptions : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Forms
