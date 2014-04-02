using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using WMEncoderLib;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Encode
{
	/// <summary>Generic form used to enumerate all audio and video codecs</summary>
	public class CFEnumCodecs : System.Windows.Forms.Form
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_ON_LOAD_EX				= 0;
		protected const int ERROR_ON_VIDEO_MODE_CHANGED_EX	= 1;
		protected const int ERROR_ON_AUDIO_MODE_CHANGED_EX	= 2;
		protected const int ERROR_ADD_VIDEO_EX				= 3;
		protected const int ERROR_ADD_AUDIO_EX				= 4;
		protected const int ERROR_FILL_PROFILES_EX			= 5;
		protected const int ERROR_GET_PROFILES_EX			= 6;
		protected const int ERROR_INITIALIZE_ENCODER_FAILED = 7;
		protected const int ERROR_PROFILE_NOT_FOUND			= 8;
		protected const int ERROR_SET_PROFILE_EX		    = 9;
		protected const int ERROR_SET_CODEC_PROFILE_EX		= 10;
		protected const int ERROR_FILL_CODECS_EX		    = 11;
	
		#endregion Constants
		
		#region Private Members

		/// <summary>Form designer component container</summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>Form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;

		/// <summary>Local member bounded to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member used to construct error messages</summary>
		private FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();
		
		/// <summary>Group box for video codec controls</summary>
		private System.Windows.Forms.GroupBox m_ctrlVideoGroup;
		
		/// <summary>Combo box of supported video bit rate (VBR) modes</summary>
		private System.Windows.Forms.ComboBox m_ctrlVideoModes;
		
		/// <summary>Static text label for video VBR mode</summary>
		private System.Windows.Forms.Label m_ctrlVideoModeLabel;
		
		/// <summary>Group box for audio codec controls</summary>
		private System.Windows.Forms.GroupBox m_ctrlAudioGroup;
		
		/// <summary>Static text label for audio VBR mode</summary>
		private System.Windows.Forms.Label m_ctrlAudioModeLabel;
		
		/// <summary>Combo box of supported audio bit rate (VBR) modes</summary>
		private System.Windows.Forms.ComboBox m_ctrlAudioModes;
		
		/// <summary>List box of registered audio codecs</summary>
		private System.Windows.Forms.ListBox m_ctrlAudioCodecs;
		
		/// <summary>List box of registered video codecs</summary>
		private System.Windows.Forms.ListBox m_ctrlVideoCodecs;

		/// <summary>Label for the combobox containing the available profiles</summary>
		private System.Windows.Forms.Label m_ctrlProfilesLabel;
		
		/// <summary>List box of available profiles</summary>
		private System.Windows.Forms.ListBox m_ctrlProfiles;
		
		/// <summary>Media encoder engine</summary>
		private CWMEncoder m_wmEncoder = null;
		
		/// <summary>Local member bound to Profile property</summary>
		private string m_strProfile = "";

		/// <summary>Label for video codecs list box</summary>
		private System.Windows.Forms.Label m_ctrlVideoCodecsLabel;

		/// <summary>Label for audio codecs list box</summary>
		private System.Windows.Forms.Label m_ctrlAudioCodecsLabel;

		/// <summary>Static text control to show name of video codec bound to the active profile</summary>
		private System.Windows.Forms.Label m_ctrlProfileVideoCodec;

		/// <summary>Static text label for video codec name</summary>
		private System.Windows.Forms.Label m_ctrlProfileVideoCodecLabel;

		/// <summary>Static text control to show name of audio codec bound to the active profile</summary>
		private System.Windows.Forms.Label m_ctrlProfileAudioCodec;
		private System.Windows.Forms.GroupBox m_ctrlProfileGroup;
		private System.Windows.Forms.Label m_ctrlProfileAudioCodecLabel;
		private System.Windows.Forms.RadioButton m_ctrlAllEncoders;
		private System.Windows.Forms.RadioButton m_ctrlProfileEncoders;
		private System.Windows.Forms.CheckBox m_ctrlProfileFilterAudio;
		private System.Windows.Forms.CheckBox m_ctrlProfileFilterVideo;
		
		/// <summary>Profile used to exhibit the audio and video codecs</summary>
		WMEncoderLib.WMEncProfile2 m_codecProfile = null;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFEnumCodecs() : base()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			//	Populate the error builder collection
			SetErrorStrings();
		}

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Called when the form window gets created</summary>
		/// <param name="e">System event arguments for OnLoad event</param>
		protected override void OnLoad(EventArgs e)
		{
			bool bSuccessful = false;
			
			try
			{
				while(bSuccessful == false)
				{
					//	Initialize the codec modes
					m_ctrlVideoModes.SelectedIndex = 0;
					m_ctrlAudioModes.SelectedIndex = 0;
					
					//	Get the list of registered profiles
					if(GetProfiles() == false) break;

					//	Fill the list of profiles
					if(FillProfiles() == false) break;
					
					// Fill the codec lists if viewing all codecs
					if(m_ctrlProfileEncoders.Checked == false)
						FillCodecs();
					
					//	All done
					bSuccessful = true;
					
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnLoad", m_tmaxErrorBuilder.Message(ERROR_ON_LOAD_EX), Ex);
			}
			
			base.OnLoad (e);
		
		}// protected override void OnLoad(EventArgs e)

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
		}

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>Called to get the collection of available profiles</summary>
		/// <returns>True if successful</returns>
		private bool GetProfiles()
		{
			try
			{
				//	Allocate and initialize the encoder engine
				m_wmEncoder = new CWMEncoder();
				
				//	We want to see all profiles
				m_wmEncoder.FilterProfiles = false;
				
				//	Initializing the encoder fills its profiles collection
				if(m_wmEncoder.Initialize("") == false)
				{
					MessageBox.Show(m_tmaxErrorBuilder.Message(ERROR_INITIALIZE_ENCODER_FAILED), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					m_wmEncoder = null;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetProfiles", m_tmaxErrorBuilder.Message(ERROR_GET_PROFILES_EX), Ex);
				m_wmEncoder = null;
			}
			
			return ((this.Profiles != null) && (this.Profiles.Count > 0));
			
		}// private bool GetProfiles()
		
		/// <summary>Called to populate the profiles combo box</summary>
		/// <returns>True if successful</returns>
		private bool FillProfiles()
		{
			bool	bFilterVideo = false;
			bool	bFilterAudio = false;
			int		iIndex = 0;
			
			try
			{
				//	Get the filter options
				bFilterVideo = m_ctrlProfileFilterVideo.Checked;
				bFilterAudio = m_ctrlProfileFilterAudio.Checked;
				
				//	Clear the existing profiles
				m_ctrlProfiles.Items.Clear();
				
				if((this.Profiles != null) && (this.Profiles.Count > 0))
				{
					foreach(CWMProfile O in this.Profiles)
					{
						if((bFilterVideo == false) || (O.GetVideoCapable() == true))
						{
							if((bFilterAudio == false) || (O.GetAudioCapable() == true))
								m_ctrlProfiles.Items.Add(O);
						}
					
					}// foreach(CWMProfile O in this.Profiles)
				
				}//if((this.Profiles != null) && (this.Profiles.Count > 0))
				
				//	Set the initial selection
				if(m_ctrlProfiles.Items.Count > 0)
				{
					if(m_strProfile.Length > 0)
						iIndex = m_ctrlProfiles.FindStringExact(m_strProfile, -1);
					if(iIndex < 0)
						iIndex = 0;
						
					m_ctrlProfiles.SelectedIndex = iIndex;
					OnProfileChanged(m_ctrlProfiles, System.EventArgs.Empty);
				}
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillProfiles", m_tmaxErrorBuilder.Message(ERROR_FILL_PROFILES_EX), Ex);
			}
			
			m_ctrlProfilesLabel.Text = String.Format("{0} Profiles:", m_ctrlProfiles.Items.Count);
			
			return (m_ctrlProfiles.Items.Count > 0);
			
		}// private bool FillProfiles()
		
		/// <summary>Called to populate the codec list boxes</summary>
		/// <returns>True if successful</returns>
		private bool FillCodecs()
		{
			bool bSuccessful = false;
			
			try
			{
				//	Get the profile to be used for the codecs
				if(SetCodecProfile() == false)
					return false;
					
				//	Fill the video codecs
				OnVideoModeChanged(m_ctrlVideoModes, System.EventArgs.Empty);
				
				//	Fill the audio codecs
				OnAudioModeChanged(m_ctrlAudioModes, System.EventArgs.Empty);
				
				bSuccessful = true;
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillCodecs", m_tmaxErrorBuilder.Message(ERROR_FILL_CODECS_EX), Ex);
			}
			
			return bSuccessful;
			
		}// private bool FillCodecs()
		
		/// <summary>Called to set the profile to be used for codec enumeration</summary>
		/// <returns>True if successful</returns>
		private bool SetCodecProfile()
		{
			CWMProfile wmProfile = null;
			
			try
			{
				//	Clear the existing profile
				m_codecProfile = null;
				
				//	Are we viewing codecs for the active profile?
				if(m_ctrlProfileEncoders.Checked == true)
				{
					//	Do we have an active selection?
					if((Profiles != null) && (m_strProfile.Length > 0))
					{
						//	Get the active profile
						if((wmProfile = Profiles.Find(m_strProfile, -1)) != null)
							m_codecProfile = wmProfile.GetI2();	
					}
					
				}
				else
				{
					//	Allocate a system profile interface
					m_codecProfile = new WMEncoderLib.WMEncProfile2Class();
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetCodecProfile", m_tmaxErrorBuilder.Message(ERROR_SET_CODEC_PROFILE_EX), Ex);
			}
			
			return (m_codecProfile != null);
			
		}// private bool SetCodecProfile()
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to load the codecs viewer form.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while processing the new video mode.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while processing the new audio mode.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while initializing the video mode. Mode = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while initializing the audio mode. Mode = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while populating the list of available profiles.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to retrieve the collection of available profiles.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to initialize a connection to the Windows Media Encoder. The WME package may not be properly installed.");

			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate %1 in the list of available profiles.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the profile selection: Name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the profile for codec enumeration.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the lists of available codecs.");
		
		}// private void SetErrorStrings()

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFEnumCodecs));
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlVideoGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlVideoCodecsLabel = new System.Windows.Forms.Label();
			this.m_ctrlVideoModeLabel = new System.Windows.Forms.Label();
			this.m_ctrlVideoModes = new System.Windows.Forms.ComboBox();
			this.m_ctrlVideoCodecs = new System.Windows.Forms.ListBox();
			this.m_ctrlAudioGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlAudioModeLabel = new System.Windows.Forms.Label();
			this.m_ctrlAudioModes = new System.Windows.Forms.ComboBox();
			this.m_ctrlAudioCodecs = new System.Windows.Forms.ListBox();
			this.m_ctrlAudioCodecsLabel = new System.Windows.Forms.Label();
			this.m_ctrlProfilesLabel = new System.Windows.Forms.Label();
			this.m_ctrlProfiles = new System.Windows.Forms.ListBox();
			this.m_ctrlProfileVideoCodec = new System.Windows.Forms.Label();
			this.m_ctrlProfileVideoCodecLabel = new System.Windows.Forms.Label();
			this.m_ctrlProfileAudioCodec = new System.Windows.Forms.Label();
			this.m_ctrlProfileGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlProfileAudioCodecLabel = new System.Windows.Forms.Label();
			this.m_ctrlAllEncoders = new System.Windows.Forms.RadioButton();
			this.m_ctrlProfileEncoders = new System.Windows.Forms.RadioButton();
			this.m_ctrlProfileFilterAudio = new System.Windows.Forms.CheckBox();
			this.m_ctrlProfileFilterVideo = new System.Windows.Forms.CheckBox();
			this.m_ctrlVideoGroup.SuspendLayout();
			this.m_ctrlAudioGroup.SuspendLayout();
			this.m_ctrlProfileGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(548, 404);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 6;
			this.m_ctrlOk.Text = "&OK";
			// 
			// m_ctrlVideoGroup
			// 
			this.m_ctrlVideoGroup.Controls.Add(this.m_ctrlVideoCodecsLabel);
			this.m_ctrlVideoGroup.Controls.Add(this.m_ctrlVideoModeLabel);
			this.m_ctrlVideoGroup.Controls.Add(this.m_ctrlVideoModes);
			this.m_ctrlVideoGroup.Controls.Add(this.m_ctrlVideoCodecs);
			this.m_ctrlVideoGroup.Location = new System.Drawing.Point(4, 12);
			this.m_ctrlVideoGroup.Name = "m_ctrlVideoGroup";
			this.m_ctrlVideoGroup.Size = new System.Drawing.Size(308, 204);
			this.m_ctrlVideoGroup.TabIndex = 0;
			this.m_ctrlVideoGroup.TabStop = false;
			this.m_ctrlVideoGroup.Text = "Video Codecs";
			// 
			// m_ctrlVideoCodecsLabel
			// 
			this.m_ctrlVideoCodecsLabel.Location = new System.Drawing.Point(8, 68);
			this.m_ctrlVideoCodecsLabel.Name = "m_ctrlVideoCodecsLabel";
			this.m_ctrlVideoCodecsLabel.Size = new System.Drawing.Size(244, 16);
			this.m_ctrlVideoCodecsLabel.TabIndex = 3;
			this.m_ctrlVideoCodecsLabel.Text = "Video Codecs";
			// 
			// m_ctrlVideoModeLabel
			// 
			this.m_ctrlVideoModeLabel.Location = new System.Drawing.Point(8, 24);
			this.m_ctrlVideoModeLabel.Name = "m_ctrlVideoModeLabel";
			this.m_ctrlVideoModeLabel.Size = new System.Drawing.Size(176, 16);
			this.m_ctrlVideoModeLabel.TabIndex = 2;
			this.m_ctrlVideoModeLabel.Text = "Variable Bit Rate (VBR) Mode";
			// 
			// m_ctrlVideoModes
			// 
			this.m_ctrlVideoModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_ctrlVideoModes.IntegralHeight = false;
			this.m_ctrlVideoModes.Items.AddRange(new object[] {
																  "All",
																  "Constant (CBR)",
																  "Peak",
																  "Quality",
																  "Bit Rate"});
			this.m_ctrlVideoModes.Location = new System.Drawing.Point(8, 40);
			this.m_ctrlVideoModes.Name = "m_ctrlVideoModes";
			this.m_ctrlVideoModes.Size = new System.Drawing.Size(248, 21);
			this.m_ctrlVideoModes.TabIndex = 0;
			this.m_ctrlVideoModes.SelectedIndexChanged += new System.EventHandler(this.OnVideoModeChanged);
			// 
			// m_ctrlVideoCodecs
			// 
			this.m_ctrlVideoCodecs.Location = new System.Drawing.Point(8, 88);
			this.m_ctrlVideoCodecs.Name = "m_ctrlVideoCodecs";
			this.m_ctrlVideoCodecs.Size = new System.Drawing.Size(248, 95);
			this.m_ctrlVideoCodecs.Sorted = true;
			this.m_ctrlVideoCodecs.TabIndex = 1;
			// 
			// m_ctrlAudioGroup
			// 
			this.m_ctrlAudioGroup.Controls.Add(this.m_ctrlAudioModeLabel);
			this.m_ctrlAudioGroup.Controls.Add(this.m_ctrlAudioModes);
			this.m_ctrlAudioGroup.Controls.Add(this.m_ctrlAudioCodecs);
			this.m_ctrlAudioGroup.Controls.Add(this.m_ctrlAudioCodecsLabel);
			this.m_ctrlAudioGroup.Location = new System.Drawing.Point(320, 12);
			this.m_ctrlAudioGroup.Name = "m_ctrlAudioGroup";
			this.m_ctrlAudioGroup.Size = new System.Drawing.Size(308, 204);
			this.m_ctrlAudioGroup.TabIndex = 0;
			this.m_ctrlAudioGroup.TabStop = false;
			this.m_ctrlAudioGroup.Text = "Audio Codecs";
			// 
			// m_ctrlAudioModeLabel
			// 
			this.m_ctrlAudioModeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlAudioModeLabel.Location = new System.Drawing.Point(8, 24);
			this.m_ctrlAudioModeLabel.Name = "m_ctrlAudioModeLabel";
			this.m_ctrlAudioModeLabel.Size = new System.Drawing.Size(256, 16);
			this.m_ctrlAudioModeLabel.TabIndex = 2;
			this.m_ctrlAudioModeLabel.Text = "Variable Bit Rate (VBR) Mode";
			// 
			// m_ctrlAudioModes
			// 
			this.m_ctrlAudioModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_ctrlAudioModes.IntegralHeight = false;
			this.m_ctrlAudioModes.Items.AddRange(new object[] {
																  "All",
																  "Constant (CBR)",
																  "Peak",
																  "Quality",
																  "Bit Rate"});
			this.m_ctrlAudioModes.Location = new System.Drawing.Point(8, 40);
			this.m_ctrlAudioModes.Name = "m_ctrlAudioModes";
			this.m_ctrlAudioModes.Size = new System.Drawing.Size(248, 21);
			this.m_ctrlAudioModes.TabIndex = 0;
			this.m_ctrlAudioModes.SelectedIndexChanged += new System.EventHandler(this.OnAudioModeChanged);
			// 
			// m_ctrlAudioCodecs
			// 
			this.m_ctrlAudioCodecs.Location = new System.Drawing.Point(8, 92);
			this.m_ctrlAudioCodecs.Name = "m_ctrlAudioCodecs";
			this.m_ctrlAudioCodecs.Size = new System.Drawing.Size(248, 95);
			this.m_ctrlAudioCodecs.Sorted = true;
			this.m_ctrlAudioCodecs.TabIndex = 1;
			// 
			// m_ctrlAudioCodecsLabel
			// 
			this.m_ctrlAudioCodecsLabel.Location = new System.Drawing.Point(8, 72);
			this.m_ctrlAudioCodecsLabel.Name = "m_ctrlAudioCodecsLabel";
			this.m_ctrlAudioCodecsLabel.Size = new System.Drawing.Size(244, 16);
			this.m_ctrlAudioCodecsLabel.TabIndex = 7;
			this.m_ctrlAudioCodecsLabel.Text = "Audio Codecs";
			// 
			// m_ctrlProfilesLabel
			// 
			this.m_ctrlProfilesLabel.Location = new System.Drawing.Point(8, 224);
			this.m_ctrlProfilesLabel.Name = "m_ctrlProfilesLabel";
			this.m_ctrlProfilesLabel.Size = new System.Drawing.Size(176, 16);
			this.m_ctrlProfilesLabel.TabIndex = 4;
			this.m_ctrlProfilesLabel.Text = "Profiles";
			// 
			// m_ctrlProfiles
			// 
			this.m_ctrlProfiles.HorizontalScrollbar = true;
			this.m_ctrlProfiles.Location = new System.Drawing.Point(8, 240);
			this.m_ctrlProfiles.Name = "m_ctrlProfiles";
			this.m_ctrlProfiles.Size = new System.Drawing.Size(324, 186);
			this.m_ctrlProfiles.TabIndex = 3;
			this.m_ctrlProfiles.SelectedIndexChanged += new System.EventHandler(this.OnProfileChanged);
			// 
			// m_ctrlProfileVideoCodec
			// 
			this.m_ctrlProfileVideoCodec.Location = new System.Drawing.Point(112, 24);
			this.m_ctrlProfileVideoCodec.Name = "m_ctrlProfileVideoCodec";
			this.m_ctrlProfileVideoCodec.Size = new System.Drawing.Size(168, 16);
			this.m_ctrlProfileVideoCodec.TabIndex = 7;
			// 
			// m_ctrlProfileVideoCodecLabel
			// 
			this.m_ctrlProfileVideoCodecLabel.Location = new System.Drawing.Point(8, 24);
			this.m_ctrlProfileVideoCodecLabel.Name = "m_ctrlProfileVideoCodecLabel";
			this.m_ctrlProfileVideoCodecLabel.Size = new System.Drawing.Size(100, 16);
			this.m_ctrlProfileVideoCodecLabel.TabIndex = 8;
			this.m_ctrlProfileVideoCodecLabel.Text = "Video Encoder:";
			// 
			// m_ctrlProfileAudioCodec
			// 
			this.m_ctrlProfileAudioCodec.Location = new System.Drawing.Point(112, 44);
			this.m_ctrlProfileAudioCodec.Name = "m_ctrlProfileAudioCodec";
			this.m_ctrlProfileAudioCodec.Size = new System.Drawing.Size(168, 16);
			this.m_ctrlProfileAudioCodec.TabIndex = 9;
			// 
			// m_ctrlProfileGroup
			// 
			this.m_ctrlProfileGroup.Controls.Add(this.m_ctrlProfileVideoCodec);
			this.m_ctrlProfileGroup.Controls.Add(this.m_ctrlProfileAudioCodecLabel);
			this.m_ctrlProfileGroup.Controls.Add(this.m_ctrlProfileVideoCodecLabel);
			this.m_ctrlProfileGroup.Controls.Add(this.m_ctrlProfileAudioCodec);
			this.m_ctrlProfileGroup.Location = new System.Drawing.Point(340, 236);
			this.m_ctrlProfileGroup.Name = "m_ctrlProfileGroup";
			this.m_ctrlProfileGroup.Size = new System.Drawing.Size(288, 72);
			this.m_ctrlProfileGroup.TabIndex = 11;
			this.m_ctrlProfileGroup.TabStop = false;
			this.m_ctrlProfileGroup.Text = "Profile Encoders";
			// 
			// m_ctrlProfileAudioCodecLabel
			// 
			this.m_ctrlProfileAudioCodecLabel.Location = new System.Drawing.Point(8, 44);
			this.m_ctrlProfileAudioCodecLabel.Name = "m_ctrlProfileAudioCodecLabel";
			this.m_ctrlProfileAudioCodecLabel.Size = new System.Drawing.Size(100, 16);
			this.m_ctrlProfileAudioCodecLabel.TabIndex = 12;
			this.m_ctrlProfileAudioCodecLabel.Text = "Audio Encoder:";
			// 
			// m_ctrlAllEncoders
			// 
			this.m_ctrlAllEncoders.Location = new System.Drawing.Point(348, 340);
			this.m_ctrlAllEncoders.Name = "m_ctrlAllEncoders";
			this.m_ctrlAllEncoders.Size = new System.Drawing.Size(216, 24);
			this.m_ctrlAllEncoders.TabIndex = 13;
			this.m_ctrlAllEncoders.Text = "Show All Encoders";
			// 
			// m_ctrlProfileEncoders
			// 
			this.m_ctrlProfileEncoders.Checked = true;
			this.m_ctrlProfileEncoders.Location = new System.Drawing.Point(348, 320);
			this.m_ctrlProfileEncoders.Name = "m_ctrlProfileEncoders";
			this.m_ctrlProfileEncoders.Size = new System.Drawing.Size(220, 24);
			this.m_ctrlProfileEncoders.TabIndex = 12;
			this.m_ctrlProfileEncoders.TabStop = true;
			this.m_ctrlProfileEncoders.Text = "Show Encoders for Selected Profile";
			// 
			// m_ctrlProfileFilterAudio
			// 
			this.m_ctrlProfileFilterAudio.Checked = true;
			this.m_ctrlProfileFilterAudio.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_ctrlProfileFilterAudio.Location = new System.Drawing.Point(348, 376);
			this.m_ctrlProfileFilterAudio.Name = "m_ctrlProfileFilterAudio";
			this.m_ctrlProfileFilterAudio.Size = new System.Drawing.Size(196, 24);
			this.m_ctrlProfileFilterAudio.TabIndex = 14;
			this.m_ctrlProfileFilterAudio.Text = "Require Audio Encoder";
			// 
			// m_ctrlProfileFilterVideo
			// 
			this.m_ctrlProfileFilterVideo.Checked = true;
			this.m_ctrlProfileFilterVideo.CheckState = System.Windows.Forms.CheckState.Checked;
			this.m_ctrlProfileFilterVideo.Location = new System.Drawing.Point(348, 396);
			this.m_ctrlProfileFilterVideo.Name = "m_ctrlProfileFilterVideo";
			this.m_ctrlProfileFilterVideo.Size = new System.Drawing.Size(196, 24);
			this.m_ctrlProfileFilterVideo.TabIndex = 15;
			this.m_ctrlProfileFilterVideo.Text = "Require Video Encoder";
			// 
			// CFEnumCodecs
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(634, 435);
			this.Controls.Add(this.m_ctrlAllEncoders);
			this.Controls.Add(this.m_ctrlProfileEncoders);
			this.Controls.Add(this.m_ctrlProfileFilterAudio);
			this.Controls.Add(this.m_ctrlProfileFilterVideo);
			this.Controls.Add(this.m_ctrlProfileGroup);
			this.Controls.Add(this.m_ctrlProfiles);
			this.Controls.Add(this.m_ctrlProfilesLabel);
			this.Controls.Add(this.m_ctrlAudioGroup);
			this.Controls.Add(this.m_ctrlVideoGroup);
			this.Controls.Add(this.m_ctrlOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "CFEnumCodecs";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Registered Codecs";
			this.m_ctrlVideoGroup.ResumeLayout(false);
			this.m_ctrlAudioGroup.ResumeLayout(false);
			this.m_ctrlProfileGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		
		/// <summary>Called when the user makes a new selection in the Video Modes list</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The system event arguments</param>
		private void OnVideoModeChanged(object sender, System.EventArgs e)
		{
			try
			{
				//	Clear the existing contents 
				m_ctrlVideoCodecs.Items.Clear();
				
				//	Get the selected index
				switch(m_ctrlVideoModes.SelectedIndex)
				{
					//	Constant bit rate?
					case 1:
					
						AddVideo(WMENC_PROFILE_VBR_MODE.WMENC_PVM_NONE);
						break;
						
					//	Quality bit rate?
					case 2:
					
						AddVideo(WMENC_PROFILE_VBR_MODE.WMENC_PVM_PEAK);
						break;
						
					//	Quality bit rate?
					case 3:
					
						AddVideo(WMENC_PROFILE_VBR_MODE.WMENC_PVM_UNCONSTRAINED);
						break;
						
					//	Normal bit rate?
					case 4:
					
						AddVideo(WMENC_PROFILE_VBR_MODE.WMENC_PVM_BITRATE_BASED);
						break;
						
					//	All bit rates
					case 0:
					default:
					
						AddVideo(WMENC_PROFILE_VBR_MODE.WMENC_PVM_NONE);
						AddVideo(WMENC_PROFILE_VBR_MODE.WMENC_PVM_PEAK);
						AddVideo(WMENC_PROFILE_VBR_MODE.WMENC_PVM_BITRATE_BASED);
						AddVideo(WMENC_PROFILE_VBR_MODE.WMENC_PVM_BITRATE_BASED);
						break;
						
				}// switch(m_ctrlVideoModes.SelectedIndex)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnVideoModeChanged", m_tmaxErrorBuilder.Message(ERROR_ON_VIDEO_MODE_CHANGED_EX), Ex);
			}
			
			m_ctrlVideoCodecsLabel.Text = String.Format("{0} Video Codecs", m_ctrlVideoCodecs.Items.Count);
			
		}// private void OnVideoModeChanged(object sender, System.EventArgs e)

		/// <summary>Called when the user makes a new selection in the Audio Modes list</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The system event arguments</param>
		private void OnAudioModeChanged(object sender, System.EventArgs e)
		{
			try
			{
				//	Clear the existing contents 
				m_ctrlAudioCodecs.Items.Clear();
				
				//	Get the selected index
				switch(m_ctrlAudioModes.SelectedIndex)
				{
						//	Constant bit rate?
					case 1:
					
						AddAudio(WMENC_PROFILE_VBR_MODE.WMENC_PVM_NONE);
						break;
						
						//	Quality bit rate?
					case 2:
					
						AddAudio(WMENC_PROFILE_VBR_MODE.WMENC_PVM_PEAK);
						break;
						
						//	Quality bit rate?
					case 3:
					
						AddAudio(WMENC_PROFILE_VBR_MODE.WMENC_PVM_UNCONSTRAINED);
						break;
						
						//	Normal bit rate?
					case 4:
					
						AddAudio(WMENC_PROFILE_VBR_MODE.WMENC_PVM_BITRATE_BASED);
						break;
						
						//	All bit rates
					case 0:
					default:
					
						AddAudio(WMENC_PROFILE_VBR_MODE.WMENC_PVM_NONE);
						AddAudio(WMENC_PROFILE_VBR_MODE.WMENC_PVM_PEAK);
						AddAudio(WMENC_PROFILE_VBR_MODE.WMENC_PVM_BITRATE_BASED);
						AddAudio(WMENC_PROFILE_VBR_MODE.WMENC_PVM_BITRATE_BASED);
						break;
						
				}// switch(m_ctrlAudioModes.SelectedIndex)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnAudioModeChanged", m_tmaxErrorBuilder.Message(ERROR_ON_AUDIO_MODE_CHANGED_EX), Ex);
			}
			
		}// private void OnAudioModeChanged(object sender, System.EventArgs e)

		/// <summary>This method adds codecs of the specified VBR mode to the audio list</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The system event arguments</param>
		private void AddAudio(WMENC_PROFILE_VBR_MODE eMode)
		{
			object	codecName = null;
			int		iFourCC = 0;

			try
			{
				if(m_codecProfile != null)
				{
					//	Set the profile's VBR mode
					m_codecProfile.set_VBRMode(WMENC_SOURCE_TYPE.WMENC_AUDIO, 0, eMode);
					
					for(int i = 0; i < m_codecProfile.AudioCodecCount; i++)
					{
						iFourCC = m_codecProfile.EnumAudioCodec(i, out codecName);
						m_ctrlAudioCodecs.Items.Add(codecName.ToString());
					}
				
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddAudio", m_tmaxErrorBuilder.Message(ERROR_ADD_AUDIO_EX, eMode), Ex);
			}
			
			m_ctrlAudioCodecsLabel.Text = String.Format("{0} Audio Codecs", m_ctrlAudioCodecs.Items.Count);

		}// private void AddAudio(WMENC_PROFILE_VBR_MODE eMode)

		/// <summary>This method adds codecs of the specified VBR mode to the video list</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The system event arguments</param>
		private void AddVideo(WMENC_PROFILE_VBR_MODE eMode)
		{
			object	codecName = null;
			int		iFourCC = 0;
			try
			{
				if(m_codecProfile != null)
				{
					//	Set the profile's VBR mode
					m_codecProfile.set_VBRMode(WMENC_SOURCE_TYPE.WMENC_VIDEO, 0, eMode);
					
					for(int i = 0; i < m_codecProfile.VideoCodecCount; i++)
					{
						iFourCC = m_codecProfile.EnumVideoCodec(i, out codecName);
						m_ctrlVideoCodecs.Items.Add(codecName.ToString());
					}
				
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddVideo", m_tmaxErrorBuilder.Message(ERROR_ADD_VIDEO_EX, eMode), Ex);
			}

		}// private void AddVideo(WMENC_PROFILE_VBR_MODE eMode)

		/// <summary>This method handles events fired when the user changes one of the profile filter options</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">System event arguments</param>
		private void OnProfileFilterChange(object sender, System.EventArgs e)
		{
			//	Refill the profiles list box
			FillProfiles();
		}

		/// <summary>This method handles events fired when the user changes one of the codec filter options</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">System event arguments</param>
		private void OnCodecFilterChange(object sender, System.EventArgs e)
		{
			//	Refill the codecs list boxes
			FillCodecs();
		}

		/// <summary>This method handles events fired when the user selects a new profile in the list</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">System event arguments</param>
		private void OnProfileChanged(object sender, System.EventArgs e)
		{
			SetProfile((CWMProfile)(m_ctrlProfiles.SelectedItem));
		}

		/// <summary>This method is called to set the active profile</summary>
		/// <param name="wmProfile">The profile object to be activated</param>
		private void SetProfile(CWMProfile wmProfile)
		{
			try
			{
				if(wmProfile != null)
				{
					m_strProfile = wmProfile.Name;
					
					m_ctrlProfileVideoCodec.Text = wmProfile.GetVideoCodec();
					m_ctrlProfileAudioCodec.Text = wmProfile.GetAudioCodec();
				}
				else
				{
					m_strProfile = "";
					
					m_ctrlProfileVideoCodec.Text = "";
					m_ctrlProfileAudioCodec.Text = "";
				}
				
				//	Rebuild the codec lists if viewing those for the active profile
				if(m_ctrlProfileEncoders.Checked == true)
					FillCodecs();
				
			}
			catch(System.Exception Ex)
			{
				if(wmProfile != null)
					m_tmaxEventSource.FireError(this, "SetProfile", m_tmaxErrorBuilder.Message(ERROR_SET_PROFILE_EX, wmProfile.Name), Ex);
				else
					m_tmaxEventSource.FireError(this, "SetProfile", m_tmaxErrorBuilder.Message(ERROR_SET_PROFILE_EX, "NULL OBJECT"), Ex);
			}
				
		}// private void SetProfile(CWMProfile wmProfile)

		/// <summary>This method is called to set the active profile</summary>
		/// <param name="strProfile">The name of the profile object to be activated</param>
		private void SetProfile(string strProfile)
		{
			int	iIndex = 0;
			
			try
			{
				//	Should we make this the active selection?
				if(m_ctrlProfiles.Items.Count > 0)
				{
					if((strProfile != null) && (strProfile.Length > 0))
					{
						if((iIndex = m_ctrlProfiles.FindStringExact(strProfile, -1)) >= 0)
						{
							m_ctrlProfiles.SelectedIndex = iIndex;
							OnProfileChanged(m_ctrlProfiles, System.EventArgs.Empty);
						}
						else
						{
							MessageBox.Show(m_tmaxErrorBuilder.Message(ERROR_PROFILE_NOT_FOUND, strProfile), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						}
					
					}
					else
					{
						//	Clear the selection
						m_ctrlProfiles.SelectedIndex = -1;
						OnProfileChanged(m_ctrlProfiles, System.EventArgs.Empty);
					}
				
				}
				else
				{
					m_strProfile = strProfile;
				
				}// if(this.Profiles != null)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProfile", m_tmaxErrorBuilder.Message(ERROR_SET_PROFILE_EX, strProfile), Ex);
			}
				
		}// private void SetProfile(string strProfile)

		#endregion Private Methods

		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }

		}// EventSource property
		
		/// <summary>Windows Media Encoder interface</summary>
		public CWMEncoder Encoder
		{
			get { return m_wmEncoder; }
		}
		
		/// <summary>Collection of all known profiles</summary>
		public CWMProfiles Profiles
		{
			get { return ((m_wmEncoder != null) ? m_wmEncoder.Profiles : null); }
		}
		
		/// <summary>The name of the form's active profile selection</summary>
		public string Profile
		{
			get { return m_strProfile; }
			set { SetProfile(value); }
		}
		
		#endregion Properties
		
	}// public class CFEnumCodecs : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
