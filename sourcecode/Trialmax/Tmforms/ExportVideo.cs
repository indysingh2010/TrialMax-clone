using System;
using System.Drawing;
using System.Drawing.Text;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Controls;
using FTI.Trialmax.Encode;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form allows the user to edit the properties of a meta field</summary>
	public class CFExportVideo : CFTmaxBaseForm
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_EXCHANGE_EX		= ERROR_TMAX_FORM_MAX + 1;
		protected const int ERROR_FILL_PROFILES_EX	= ERROR_TMAX_FORM_MAX + 2;
		protected const int ERROR_SET_PROFILE_EX	= ERROR_TMAX_FORM_MAX + 3;
		protected const int ERROR_SET_SAMI_FONT_EX	= ERROR_TMAX_FORM_MAX + 4;
		
		#endregion Constants

		#region Private Members
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>The form's cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>The form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		
		/// <summary>Check box for VideoEDL option</summary>
		private System.Windows.Forms.CheckBox m_ctrlVideoEDL;
		
		/// <summary>Check box for VideoSAMI option</summary>
		private System.Windows.Forms.CheckBox m_ctrlVideoSAMI;
		
		/// <summary>Check box for VideoWMV option</summary>
		private System.Windows.Forms.CheckBox m_ctrlVideoWMV;
		
		/// <summary>Local member bound to ExportOptions property</summary>
		private CTmaxExportOptions m_tmaxExportOptions = null;
		
		/// <summary>List box to display WMV encoder profiles</summary>
		private System.Windows.Forms.ListBox m_ctrlProfiles;
		
		/// <summary>Static text label for profiles list box</summary>
		private System.Windows.Forms.Label m_ctrlProfilesLabel;
		
		/// <summary>Group box for file format options</summary>
		private System.Windows.Forms.GroupBox m_ctrlFormatsGroup;
		
		/// <summary>Group box for SAMI options</summary>
		private System.Windows.Forms.GroupBox m_ctrlSAMIGroup;
		
		/// <summary>Group box for WMV options</summary>
		private System.Windows.Forms.GroupBox m_ctrlWMVGroup;
		
		/// <summary>List box of fonts for SAMI files</summary>
		private System.Windows.Forms.ComboBox m_ctrlSAMIFonts;
		
		/// <summary>Color selection button for SAMI text</summary>
		private System.Windows.Forms.Button m_ctrlSAMIColor;
		
		/// <summary>Color picker dialog for SAMI text color</summary>
		private System.Windows.Forms.ColorDialog m_ctrlColorPicker;
		
		/// <summary>Static text label for SAMI font name</summary>
		private System.Windows.Forms.Label m_ctrlSAMIFontLabel;
		
		/// <summary>Static text label for SAMI text color</summary>
		private System.Windows.Forms.Label m_ctrlSAMIColorLabel;
		
		/// <summary>Group box for WMV options</summary>
		private System.Windows.Forms.Label m_ctrlSAMISizeLabel;
		
		/// <summary>Text box to allow user to set the SAMI text point size</summary>
		private System.Windows.Forms.TextBox m_ctrlSAMISize;
		
		/// <summary>Text box to allow user to set the SAMI lines of text</summary>
		private System.Windows.Forms.TextBox m_ctrlSAMILines;
		
		/// <summary>Static text label for SAMI lines of text</summary>
		private System.Windows.Forms.Label m_ctrlSAMILinesLabel;
		
		/// <summary>Text box to request use of highlighter colors for SAMI filess</summary>
		private System.Windows.Forms.CheckBox m_ctrlSAMIHighlighter;
		
		/// <summary>Group box for general preferences</summary>
		private System.Windows.Forms.GroupBox m_ctrlGeneralGroup;
		
		/// <summary>Check box to set AutoFilenames option</summary>
		private System.Windows.Forms.CheckBox m_ctrlAutoFilenames;
		
		/// <summary>Check box to set ConfirmOverwrite option</summary>
		private System.Windows.Forms.CheckBox m_ctrlConfirmOverwrite;
		
		/// <summary>Pushbutton to allow user to display list of registered codecs</summary>
		private System.Windows.Forms.Button m_ctrlViewCodecs;
		private System.Windows.Forms.CheckBox m_ctrlShowPreferred;
		private System.Windows.Forms.CheckBox m_ctrlSAMIPageNumbers;
		
		/// <summary>Collection of Windows Media Encoder Profiles</summary>
		private FTI.Trialmax.Encode.CWMEncoder m_wmEncoder = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		public CFExportVideo()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
		}// public CFExportVideo()

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to exchange the form's properties: SetMembers = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the list of WMV encoder profiles.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the profile selection: Name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the SAMI font selection: Name = %1");
		
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
		
		/// <summary>This method handles the form's Load event</summary>
		/// <param name="e">The event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Initialize all the child controls
			m_ctrlOk.Enabled = Exchange(false);
			
			base.OnLoad(e);
			
		}// protected override void OnLoad(EventArgs e)
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>Called to populate the list of Fonts for SAMI text</summary>
		/// <returns>true if successful</returns>
		private bool FillSAMIFonts()
		{
			InstalledFontCollection	aInstalled = null;
			FontFamily[]			aFamilies = null;
			bool					bSuccessful = false;

			try
			{
				//	Get the collection of installed fonts
				aInstalled = new InstalledFontCollection();

				//	Get the array of FontFamily objects.
				aFamilies = aInstalled.Families;
				
				for(int i = 0; i <= aFamilies.GetUpperBound(0); i++)
				{
					m_ctrlSAMIFonts.Items.Add(aFamilies[i].Name);
				}
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "FillSAMIFonts", "Ex:", Ex);
			}
			
			return bSuccessful;
		
		}// private bool FillSAMIFonts()
				
		/// <summary>Called to populate the list of encoder profiles</summary>
		/// <returns>true if successful</returns>
		private bool FillProfiles()
		{
			bool bSuccessful = false;
			bool bPreferred = false;
			
			try
			{
				//	Clear the existing profiles
				m_ctrlProfiles.Items.Clear();
				
				//	Caller should have provided the encoder object
				if((m_wmEncoder == null) || (m_wmEncoder.Profiles == null))
					return false;
					
				//	Are we showing only preferred profiles?
				if((bPreferred = m_ctrlShowPreferred.Checked) == true)
				{
					//	Make sure the Preferred are properly flagged
					if(m_wmEncoder.PreferredProfiles != null)
						m_wmEncoder.Profiles.SetPreferred(m_wmEncoder.PreferredProfiles);
				}
				
				foreach(CWMProfile O in m_wmEncoder.Profiles)
				{
					if((bPreferred == false) || (O.Preferred == true))
						m_ctrlProfiles.Items.Add(O);
				}
						
				bSuccessful = (m_ctrlProfiles.Items.Count > 0);

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillProfiles", m_tmaxErrorBuilder.Message(ERROR_FILL_PROFILES_EX), Ex);
			}
			
			return bSuccessful;
			
		}// private bool FillProfiles()
		
		/// <summary>Called to set the profile selection</summary>
		/// <param name="strName">The name to be selected</param>
		private void SetProfileSelection(string strName)
		{
			int iIndex = -1;
			try
			{
				//	Locate the specified profile name
				if((strName != null) && (strName.Length > 0))
				{
					iIndex = m_ctrlProfiles.FindStringExact(strName);
				}
				
				//	Did we locate the requested profile?
				if(iIndex < 0)
					iIndex = 0;
					
				//	Make the selection
				if(m_ctrlProfiles.Items.Count > 0)
					m_ctrlProfiles.SetSelected(iIndex, true);
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProfileSelection", m_tmaxErrorBuilder.Message(ERROR_SET_PROFILE_EX, strName), Ex);
			}
			
		}// private void SetProfileSelection(string strName)
		
		/// <summary>Called to set the font selection</summary>
		/// <param name="strName">The name to be selected</param>
		private void SetSAMIFontSelection(string strName)
		{
			int iIndex = -1;
			try
			{
				//	Locate the specified font name
				if((strName != null) && (strName.Length > 0))
				{
					iIndex = m_ctrlSAMIFonts.FindStringExact(strName);
				}
				if(iIndex < 0)
				{
					iIndex = m_ctrlSAMIFonts.FindStringExact("Arial");
				}
				
				//	Did we locate the requested font?
				if(iIndex < 0)
					iIndex = 0;
					
				//	Make the selection
				if(m_ctrlSAMIFonts.Items.Count > 0)
					m_ctrlSAMIFonts.SelectedIndex = iIndex;
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetSAMIFontSelection", m_tmaxErrorBuilder.Message(ERROR_SET_SAMI_FONT_EX, strName), Ex);
			}
			
		}// private void SetProfileSelection(string strName)
		
		/// <summary>This method is called to manage the exchange of data between the class members and child controls</summary>
		/// <param name="bSetMembers">true to set the class members</param>
		/// <returns>true if successful</returns>
		private bool Exchange(bool bSetMembers)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Are we setting the class members?
				if(bSetMembers == true)
				{
					m_tmaxExportOptions.VideoEDL  = m_ctrlVideoEDL.Checked;
					m_tmaxExportOptions.VideoSAMI = m_ctrlVideoSAMI.Checked;
					m_tmaxExportOptions.VideoWMV  = m_ctrlVideoWMV.Checked;
					m_tmaxExportOptions.AutoFilenames = m_ctrlAutoFilenames.Checked;
					m_tmaxExportOptions.ConfirmOverwrite = m_ctrlConfirmOverwrite.Checked;
					
					m_wmEncoder.UsePreferred = m_ctrlShowPreferred.Checked;
					
					//	Do we have any encoder profiles?
					if(m_ctrlProfiles.Items.Count > 0)
					{
						if(m_ctrlProfiles.SelectedItem != null)
						{
							m_wmEncoder.LastProfile = ((CWMProfile)m_ctrlProfiles.SelectedItem).Name;
							
						}
						else
						{
							m_wmEncoder.LastProfile = "";
						}

					}
				
					//	Do we have any SAMI fonts
					m_tmaxExportOptions.SAMIFontFamily = "Arial";
					if(m_ctrlSAMIFonts.Items.Count > 0)
					{
						if(m_ctrlSAMIFonts.SelectedItem != null)
						{
							m_tmaxExportOptions.SAMIFontFamily = m_ctrlSAMIFonts.SelectedItem.ToString();
						}

					}
					m_tmaxExportOptions.SAMIFontColor = m_ctrlSAMIColor.BackColor;
					m_tmaxExportOptions.SAMIFontHighlighter = m_ctrlSAMIHighlighter.Checked;
					m_tmaxExportOptions.SAMIPageNumbers = m_ctrlSAMIPageNumbers.Checked;
						
					try { m_tmaxExportOptions.SAMIFontSize = System.Convert.ToInt32(m_ctrlSAMISize.Text); }
					catch { m_tmaxExportOptions.SAMIFontSize = 12; }
					
					try { m_tmaxExportOptions.SAMILines = System.Convert.ToInt32(m_ctrlSAMILines.Text); }
					catch { m_tmaxExportOptions.SAMILines = 3; }
				
				}
				else
				{
					m_ctrlAutoFilenames.Checked = m_tmaxExportOptions.AutoFilenames;
					m_ctrlConfirmOverwrite.Checked = m_tmaxExportOptions.ConfirmOverwrite;
					m_ctrlVideoEDL.Checked = m_tmaxExportOptions.VideoEDL;
					m_ctrlVideoSAMI.Checked = m_tmaxExportOptions.VideoSAMI;
					m_ctrlShowPreferred.Checked = m_wmEncoder.UsePreferred;
					
					//	Do we have any encoder profiles?
					FillProfiles();
					if(m_ctrlProfiles.Items.Count > 0)
					{
						m_ctrlVideoWMV.Checked = m_tmaxExportOptions.VideoWMV;
					
						SetProfileSelection(m_wmEncoder.LastProfile);
					}
					
					FillSAMIFonts();
					if(m_ctrlSAMIFonts.Items.Count > 0)
						SetSAMIFontSelection(m_tmaxExportOptions.SAMIFontFamily);
					m_ctrlSAMIColor.BackColor = m_tmaxExportOptions.SAMIFontColor;
					m_ctrlSAMISize.Text = m_tmaxExportOptions.SAMIFontSize.ToString();
					m_ctrlSAMILines.Text = m_tmaxExportOptions.SAMILines.ToString();
					m_ctrlSAMIHighlighter.Checked = m_tmaxExportOptions.SAMIFontHighlighter;
					m_ctrlSAMIPageNumbers.Checked = m_tmaxExportOptions.SAMIPageNumbers;
					
					SetControlStates();
					
				}// if(bSetMembers == true)
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Exchange", m_tmaxErrorBuilder.Message(ERROR_EXCHANGE_EX, bSetMembers), Ex);
			}
			
			return bSuccessful; 
			
		}// private bool Exchange(bool bSetMembers)
		
		/// <summary>This method is called to enable / disable child controls based on current settings</summary>
		private void SetControlStates()
		{
			try
			{
				//	Do we have any encoder profiles?
				if((m_wmEncoder != null) && (m_wmEncoder.Profiles != null))
				{
                    m_ctrlVideoWMV.Enabled = true;// (m_ctrlProfiles.Items.Count > 0);
					m_ctrlViewCodecs.Enabled = true;
					m_ctrlWMVGroup.Enabled = (m_wmEncoder.Profiles.Count > 0);
					m_ctrlShowPreferred.Enabled = (m_wmEncoder.Profiles.Count > 0);
					m_ctrlProfilesLabel.Enabled = (m_ctrlProfiles.Items.Count > 0);
					m_ctrlProfiles.Enabled = (m_ctrlProfiles.Items.Count > 0);
				}
				else
				{
                    m_ctrlVideoWMV.Enabled = true;//false;
					m_ctrlWMVGroup.Enabled = false;
					m_ctrlProfilesLabel.Enabled = false;
					m_ctrlProfiles.Enabled = false;
					//m_ctrlVideoWMV.Checked = false;
					m_ctrlViewCodecs.Enabled = false;
					m_ctrlShowPreferred.Checked = false;
					m_ctrlShowPreferred.Enabled = false;
				}
				
				m_ctrlSAMIGroup.Enabled = m_ctrlVideoSAMI.Checked;
			
			}
			catch
			{
			}
			
		}// private void SetControlStates()
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFExportVideo));
            this.m_ctrlCancel = new System.Windows.Forms.Button();
            this.m_ctrlOk = new System.Windows.Forms.Button();
            this.m_ctrlVideoEDL = new System.Windows.Forms.CheckBox();
            this.m_ctrlVideoSAMI = new System.Windows.Forms.CheckBox();
            this.m_ctrlVideoWMV = new System.Windows.Forms.CheckBox();
            this.m_ctrlProfiles = new System.Windows.Forms.ListBox();
            this.m_ctrlProfilesLabel = new System.Windows.Forms.Label();
            this.m_ctrlFormatsGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlSAMIGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlSAMIPageNumbers = new System.Windows.Forms.CheckBox();
            this.m_ctrlSAMIHighlighter = new System.Windows.Forms.CheckBox();
            this.m_ctrlSAMISize = new System.Windows.Forms.TextBox();
            this.m_ctrlSAMISizeLabel = new System.Windows.Forms.Label();
            this.m_ctrlSAMIColorLabel = new System.Windows.Forms.Label();
            this.m_ctrlSAMIFontLabel = new System.Windows.Forms.Label();
            this.m_ctrlSAMIColor = new System.Windows.Forms.Button();
            this.m_ctrlSAMIFonts = new System.Windows.Forms.ComboBox();
            this.m_ctrlSAMILines = new System.Windows.Forms.TextBox();
            this.m_ctrlSAMILinesLabel = new System.Windows.Forms.Label();
            this.m_ctrlWMVGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlShowPreferred = new System.Windows.Forms.CheckBox();
            this.m_ctrlColorPicker = new System.Windows.Forms.ColorDialog();
            this.m_ctrlGeneralGroup = new System.Windows.Forms.GroupBox();
            this.m_ctrlAutoFilenames = new System.Windows.Forms.CheckBox();
            this.m_ctrlConfirmOverwrite = new System.Windows.Forms.CheckBox();
            this.m_ctrlViewCodecs = new System.Windows.Forms.Button();
            this.m_ctrlFormatsGroup.SuspendLayout();
            this.m_ctrlSAMIGroup.SuspendLayout();
            this.m_ctrlWMVGroup.SuspendLayout();
            this.m_ctrlGeneralGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_ctrlCancel
            // 
            this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_ctrlCancel.Location = new System.Drawing.Point(500, 320);
            this.m_ctrlCancel.Name = "m_ctrlCancel";
            this.m_ctrlCancel.Size = new System.Drawing.Size(75, 23);
            this.m_ctrlCancel.TabIndex = 1;
            this.m_ctrlCancel.Text = "  &Cancel";
            this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
            // 
            // m_ctrlOk
            // 
            this.m_ctrlOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_ctrlOk.Location = new System.Drawing.Point(412, 320);
            this.m_ctrlOk.Name = "m_ctrlOk";
            this.m_ctrlOk.Size = new System.Drawing.Size(75, 23);
            this.m_ctrlOk.TabIndex = 0;
            this.m_ctrlOk.Text = "&OK";
            this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
            // 
            // m_ctrlVideoEDL
            // 
            this.m_ctrlVideoEDL.Location = new System.Drawing.Point(12, 80);
            this.m_ctrlVideoEDL.Name = "m_ctrlVideoEDL";
            this.m_ctrlVideoEDL.Size = new System.Drawing.Size(204, 24);
            this.m_ctrlVideoEDL.TabIndex = 2;
            this.m_ctrlVideoEDL.Text = "Create EDL File";
            // 
            // m_ctrlVideoSAMI
            // 
            this.m_ctrlVideoSAMI.Location = new System.Drawing.Point(12, 52);
            this.m_ctrlVideoSAMI.Name = "m_ctrlVideoSAMI";
            this.m_ctrlVideoSAMI.Size = new System.Drawing.Size(204, 24);
            this.m_ctrlVideoSAMI.TabIndex = 1;
            this.m_ctrlVideoSAMI.Text = "Create SAMI File";
            this.m_ctrlVideoSAMI.Click += new System.EventHandler(this.OnClickSAMI);
            // 
            // m_ctrlVideoWMV
            // 
            this.m_ctrlVideoWMV.Location = new System.Drawing.Point(12, 24);
            this.m_ctrlVideoWMV.Name = "m_ctrlVideoWMV";
            this.m_ctrlVideoWMV.Size = new System.Drawing.Size(204, 24);
            this.m_ctrlVideoWMV.TabIndex = 0;
            this.m_ctrlVideoWMV.Text = "Create Video File";
            this.m_ctrlVideoWMV.Click += new System.EventHandler(this.OnClickWMV);
            // 
            // m_ctrlProfiles
            // 
            this.m_ctrlProfiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlProfiles.HorizontalScrollbar = true;
            this.m_ctrlProfiles.IntegralHeight = false;
            this.m_ctrlProfiles.Location = new System.Drawing.Point(8, 44);
            this.m_ctrlProfiles.Name = "m_ctrlProfiles";
            this.m_ctrlProfiles.Size = new System.Drawing.Size(296, 108);
            this.m_ctrlProfiles.TabIndex = 0;
            // 
            // m_ctrlProfilesLabel
            // 
            this.m_ctrlProfilesLabel.Location = new System.Drawing.Point(8, 24);
            this.m_ctrlProfilesLabel.Name = "m_ctrlProfilesLabel";
            this.m_ctrlProfilesLabel.Size = new System.Drawing.Size(168, 16);
            this.m_ctrlProfilesLabel.TabIndex = 15;
            this.m_ctrlProfilesLabel.Text = "Encoder Profiles:";
            // 
            // m_ctrlFormatsGroup
            // 
            this.m_ctrlFormatsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlFormatsGroup.Controls.Add(this.m_ctrlVideoWMV);
            this.m_ctrlFormatsGroup.Controls.Add(this.m_ctrlVideoSAMI);
            this.m_ctrlFormatsGroup.Controls.Add(this.m_ctrlVideoEDL);
            this.m_ctrlFormatsGroup.Location = new System.Drawing.Point(8, 8);
            this.m_ctrlFormatsGroup.Name = "m_ctrlFormatsGroup";
            this.m_ctrlFormatsGroup.Size = new System.Drawing.Size(312, 132);
            this.m_ctrlFormatsGroup.TabIndex = 0;
            this.m_ctrlFormatsGroup.TabStop = false;
            this.m_ctrlFormatsGroup.Text = "Output File Formats";
            // 
            // m_ctrlSAMIGroup
            // 
            this.m_ctrlSAMIGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlSAMIGroup.Controls.Add(this.m_ctrlSAMIPageNumbers);
            this.m_ctrlSAMIGroup.Controls.Add(this.m_ctrlSAMIHighlighter);
            this.m_ctrlSAMIGroup.Controls.Add(this.m_ctrlSAMISize);
            this.m_ctrlSAMIGroup.Controls.Add(this.m_ctrlSAMISizeLabel);
            this.m_ctrlSAMIGroup.Controls.Add(this.m_ctrlSAMIColorLabel);
            this.m_ctrlSAMIGroup.Controls.Add(this.m_ctrlSAMIFontLabel);
            this.m_ctrlSAMIGroup.Controls.Add(this.m_ctrlSAMIColor);
            this.m_ctrlSAMIGroup.Controls.Add(this.m_ctrlSAMIFonts);
            this.m_ctrlSAMIGroup.Controls.Add(this.m_ctrlSAMILines);
            this.m_ctrlSAMIGroup.Controls.Add(this.m_ctrlSAMILinesLabel);
            this.m_ctrlSAMIGroup.Location = new System.Drawing.Point(328, 128);
            this.m_ctrlSAMIGroup.Name = "m_ctrlSAMIGroup";
            this.m_ctrlSAMIGroup.Size = new System.Drawing.Size(256, 180);
            this.m_ctrlSAMIGroup.TabIndex = 3;
            this.m_ctrlSAMIGroup.TabStop = false;
            this.m_ctrlSAMIGroup.Text = "SAMI Files";
            // 
            // m_ctrlSAMIPageNumbers
            // 
            this.m_ctrlSAMIPageNumbers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlSAMIPageNumbers.Location = new System.Drawing.Point(12, 152);
            this.m_ctrlSAMIPageNumbers.Name = "m_ctrlSAMIPageNumbers";
            this.m_ctrlSAMIPageNumbers.Size = new System.Drawing.Size(196, 16);
            this.m_ctrlSAMIPageNumbers.TabIndex = 5;
            this.m_ctrlSAMIPageNumbers.Text = "Include page and line numbers";
            // 
            // m_ctrlSAMIHighlighter
            // 
            this.m_ctrlSAMIHighlighter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlSAMIHighlighter.Location = new System.Drawing.Point(12, 128);
            this.m_ctrlSAMIHighlighter.Name = "m_ctrlSAMIHighlighter";
            this.m_ctrlSAMIHighlighter.Size = new System.Drawing.Size(172, 16);
            this.m_ctrlSAMIHighlighter.TabIndex = 4;
            this.m_ctrlSAMIHighlighter.Text = "Use Highlighter Colors";
            // 
            // m_ctrlSAMISize
            // 
            this.m_ctrlSAMISize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlSAMISize.Location = new System.Drawing.Point(92, 72);
            this.m_ctrlSAMISize.Name = "m_ctrlSAMISize";
            this.m_ctrlSAMISize.Size = new System.Drawing.Size(68, 20);
            this.m_ctrlSAMISize.TabIndex = 2;
            // 
            // m_ctrlSAMISizeLabel
            // 
            this.m_ctrlSAMISizeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlSAMISizeLabel.Location = new System.Drawing.Point(12, 76);
            this.m_ctrlSAMISizeLabel.Name = "m_ctrlSAMISizeLabel";
            this.m_ctrlSAMISizeLabel.Size = new System.Drawing.Size(76, 16);
            this.m_ctrlSAMISizeLabel.TabIndex = 5;
            this.m_ctrlSAMISizeLabel.Text = "Size :";
            // 
            // m_ctrlSAMIColorLabel
            // 
            this.m_ctrlSAMIColorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlSAMIColorLabel.Location = new System.Drawing.Point(12, 100);
            this.m_ctrlSAMIColorLabel.Name = "m_ctrlSAMIColorLabel";
            this.m_ctrlSAMIColorLabel.Size = new System.Drawing.Size(76, 16);
            this.m_ctrlSAMIColorLabel.TabIndex = 4;
            this.m_ctrlSAMIColorLabel.Text = "Color :";
            // 
            // m_ctrlSAMIFontLabel
            // 
            this.m_ctrlSAMIFontLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlSAMIFontLabel.Location = new System.Drawing.Point(12, 48);
            this.m_ctrlSAMIFontLabel.Name = "m_ctrlSAMIFontLabel";
            this.m_ctrlSAMIFontLabel.Size = new System.Drawing.Size(76, 16);
            this.m_ctrlSAMIFontLabel.TabIndex = 3;
            this.m_ctrlSAMIFontLabel.Text = "Font :";
            // 
            // m_ctrlSAMIColor
            // 
            this.m_ctrlSAMIColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlSAMIColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.m_ctrlSAMIColor.Location = new System.Drawing.Point(92, 100);
            this.m_ctrlSAMIColor.Name = "m_ctrlSAMIColor";
            this.m_ctrlSAMIColor.Size = new System.Drawing.Size(68, 16);
            this.m_ctrlSAMIColor.TabIndex = 3;
            this.m_ctrlSAMIColor.Click += new System.EventHandler(this.OnClickSAMIColor);
            // 
            // m_ctrlSAMIFonts
            // 
            this.m_ctrlSAMIFonts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlSAMIFonts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_ctrlSAMIFonts.IntegralHeight = false;
            this.m_ctrlSAMIFonts.Location = new System.Drawing.Point(92, 44);
            this.m_ctrlSAMIFonts.Name = "m_ctrlSAMIFonts";
            this.m_ctrlSAMIFonts.Size = new System.Drawing.Size(156, 21);
            this.m_ctrlSAMIFonts.TabIndex = 1;
            // 
            // m_ctrlSAMILines
            // 
            this.m_ctrlSAMILines.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlSAMILines.Location = new System.Drawing.Point(92, 20);
            this.m_ctrlSAMILines.Name = "m_ctrlSAMILines";
            this.m_ctrlSAMILines.Size = new System.Drawing.Size(68, 20);
            this.m_ctrlSAMILines.TabIndex = 0;
            // 
            // m_ctrlSAMILinesLabel
            // 
            this.m_ctrlSAMILinesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlSAMILinesLabel.Location = new System.Drawing.Point(12, 24);
            this.m_ctrlSAMILinesLabel.Name = "m_ctrlSAMILinesLabel";
            this.m_ctrlSAMILinesLabel.Size = new System.Drawing.Size(76, 16);
            this.m_ctrlSAMILinesLabel.TabIndex = 7;
            this.m_ctrlSAMILinesLabel.Text = "Lines (1-4) :";
            // 
            // m_ctrlWMVGroup
            // 
            this.m_ctrlWMVGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlWMVGroup.Controls.Add(this.m_ctrlShowPreferred);
            this.m_ctrlWMVGroup.Controls.Add(this.m_ctrlProfilesLabel);
            this.m_ctrlWMVGroup.Controls.Add(this.m_ctrlProfiles);
            this.m_ctrlWMVGroup.Location = new System.Drawing.Point(8, 128);
            this.m_ctrlWMVGroup.Name = "m_ctrlWMVGroup";
            this.m_ctrlWMVGroup.Size = new System.Drawing.Size(312, 180);
            this.m_ctrlWMVGroup.TabIndex = 2;
            this.m_ctrlWMVGroup.TabStop = false;
            this.m_ctrlWMVGroup.Text = "WMV Files";
            this.m_ctrlWMVGroup.Visible = false;
            // 
            // m_ctrlShowPreferred
            // 
            this.m_ctrlShowPreferred.Location = new System.Drawing.Point(12, 156);
            this.m_ctrlShowPreferred.Name = "m_ctrlShowPreferred";
            this.m_ctrlShowPreferred.Size = new System.Drawing.Size(216, 20);
            this.m_ctrlShowPreferred.TabIndex = 16;
            this.m_ctrlShowPreferred.Text = "Show Preferred Only";
            this.m_ctrlShowPreferred.Click += new System.EventHandler(this.OnClickShowPreferred);
            // 
            // m_ctrlGeneralGroup
            // 
            this.m_ctrlGeneralGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlGeneralGroup.Controls.Add(this.m_ctrlAutoFilenames);
            this.m_ctrlGeneralGroup.Controls.Add(this.m_ctrlConfirmOverwrite);
            this.m_ctrlGeneralGroup.Location = new System.Drawing.Point(328, 8);
            this.m_ctrlGeneralGroup.Name = "m_ctrlGeneralGroup";
            this.m_ctrlGeneralGroup.Size = new System.Drawing.Size(256, 112);
            this.m_ctrlGeneralGroup.TabIndex = 1;
            this.m_ctrlGeneralGroup.TabStop = false;
            this.m_ctrlGeneralGroup.Text = "Preferences";
            // 
            // m_ctrlAutoFilenames
            // 
            this.m_ctrlAutoFilenames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlAutoFilenames.Location = new System.Drawing.Point(12, 52);
            this.m_ctrlAutoFilenames.Name = "m_ctrlAutoFilenames";
            this.m_ctrlAutoFilenames.Size = new System.Drawing.Size(232, 24);
            this.m_ctrlAutoFilenames.TabIndex = 1;
            this.m_ctrlAutoFilenames.Text = "Use default filenames if multiple outputs";
            // 
            // m_ctrlConfirmOverwrite
            // 
            this.m_ctrlConfirmOverwrite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlConfirmOverwrite.Location = new System.Drawing.Point(12, 24);
            this.m_ctrlConfirmOverwrite.Name = "m_ctrlConfirmOverwrite";
            this.m_ctrlConfirmOverwrite.Size = new System.Drawing.Size(228, 24);
            this.m_ctrlConfirmOverwrite.TabIndex = 0;
            this.m_ctrlConfirmOverwrite.Text = "Confirm before overwrite existing ouput";
            // 
            // m_ctrlViewCodecs
            // 
            this.m_ctrlViewCodecs.Location = new System.Drawing.Point(16, 324);
            this.m_ctrlViewCodecs.Name = "m_ctrlViewCodecs";
            this.m_ctrlViewCodecs.Size = new System.Drawing.Size(84, 23);
            this.m_ctrlViewCodecs.TabIndex = 2;
            this.m_ctrlViewCodecs.Text = "View Codecs";
            this.m_ctrlViewCodecs.Visible = false;
            this.m_ctrlViewCodecs.Click += new System.EventHandler(this.OnClickViewCodecs);
            // 
            // CFExportVideo
            // 
            this.AcceptButton = this.m_ctrlOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.m_ctrlCancel;
            this.ClientSize = new System.Drawing.Size(592, 353);
            this.Controls.Add(this.m_ctrlViewCodecs);
            this.Controls.Add(this.m_ctrlGeneralGroup);
            this.Controls.Add(this.m_ctrlWMVGroup);
            this.Controls.Add(this.m_ctrlSAMIGroup);
            this.Controls.Add(this.m_ctrlFormatsGroup);
            this.Controls.Add(this.m_ctrlCancel);
            this.Controls.Add(this.m_ctrlOk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CFExportVideo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Export Video Options";
            this.m_ctrlFormatsGroup.ResumeLayout(false);
            this.m_ctrlSAMIGroup.ResumeLayout(false);
            this.m_ctrlSAMIGroup.PerformLayout();
            this.m_ctrlWMVGroup.ResumeLayout(false);
            this.m_ctrlGeneralGroup.ResumeLayout(false);
            this.ResumeLayout(false);

		}
	
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOk(object sender, System.EventArgs e)
		{
			//	Get the user settings
			Exchange(true);
			
			//	Must have selected at least on output format
			if((m_tmaxExportOptions.VideoEDL == false) &&
			   (m_tmaxExportOptions.VideoWMV == false) &&
			   (m_tmaxExportOptions.VideoSAMI == false))
			{
				MessageBox.Show("You must select at least one output format for the operation", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			else
			{
				//	Close the form
				DialogResult = DialogResult.OK;
				this.Close();
			}
		
		}// private void OnClickOk(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on Cancel</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickCancel(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		
		}// private void OnClickCancel(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on Create WMV</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickWMV(object sender, System.EventArgs e)
		{
			SetControlStates();
		}

		/// <summary>This method is called when the user clicks on Create SAMI</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickSAMI(object sender, System.EventArgs e)
		{
			SetControlStates();
		}

		/// <summary>This method is called when the user clicks on SAMI Font Color</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickSAMIColor(object sender, System.EventArgs e)
		{
			try
			{
				m_ctrlColorPicker.Color = m_ctrlSAMIColor.BackColor;
				if(m_ctrlColorPicker.ShowDialog() == DialogResult.OK)
					m_ctrlSAMIColor.BackColor = m_ctrlColorPicker.Color;
			}
			catch
			{
			}
		
		}

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

		/// <summary>This method is called when the user clicks on Show Preferred profiles</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickShowPreferred(object sender, System.EventArgs e)
		{
			string strProfile = "";
			
			//	Get the name of the current profile selection
			if(m_ctrlProfiles.SelectedItem != null)
				strProfile = ((CWMProfile)m_ctrlProfiles.SelectedItem).Name;
				
			//	Rebuild the profiles list
			FillProfiles();
			
			//	Update the profile selection
			SetProfileSelection(strProfile);
			
			SetControlStates();
		
		}// private void OnClickShowPreferred(object sender, System.EventArgs e)

		#endregion Private Methods

		#region Properties
		
		/// <summary>The user defined export options</summary>
		public CTmaxExportOptions ExportOptions
		{
			get { return m_tmaxExportOptions;  }
			set { m_tmaxExportOptions = value; }
		}
		
		/// <summary>TrialMax media encoder wrapper</summary>
		public FTI.Trialmax.Encode.CWMEncoder WMEncoder
		{
			get { return m_wmEncoder; }
			set { m_wmEncoder = value; }
		}
		
		#endregion Properties
	
	}// public class CFExportVideo : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
