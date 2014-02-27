using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Controls
{
	/// <summary>This control allows the user to set the overrides for the default media paths used by the database</summary>
	public class CTmaxPathOverrideCtrl : System.Windows.Forms.UserControl
	{
		#region Constants
		
		private const int ERROR_ON_LOAD_EX			= 0;
		private const int ERROR_ON_CLICK_BROWSE_EX	= 1;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>Local member bound to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member used to construct error messages</summary>
		protected FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();

		/// <summary>Local member bound to Folder property</summary>
		private FTI.Shared.Trialmax.TmaxCaseFolders m_eFolder = TmaxCaseFolders.Documents;

		/// <summary>Local member bound to Override property</summary>
		private string m_strOverride = "";

		/// <summary>Local member bound to CasePath property</summary>
		private string m_strCasePath = "";

		/// <summary>Static text control to display folder type</summary>
		private System.Windows.Forms.Label m_ctrlFolder;

		/// <summary>Edit control to allow user to provide override path</summary>
		private System.Windows.Forms.TextBox m_ctrlOverride;

		/// <summary>Tooltip for this control's children</summary>
		private System.Windows.Forms.ToolTip m_ctrlToolTips;

		/// <summary>Folder browse button</summary>
		private System.Windows.Forms.Button m_ctrlBrowse;

		/// <summary>Folder to clear out the override</summary>
		private System.Windows.Forms.Button m_ctrlReset;

		/// <summary>Image list for the button images</summary>
		private System.Windows.Forms.ImageList m_ctrlImages;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxPathOverrideCtrl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		
			//	Add the error builder's format strings
			SetErrorStrings();
		
		}// public CTmaxPathOverrideCtrl()

		#endregion Public Methods
		
		#region Protected Methods
		
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
		
		/// <summary>This method handles the control's Load event</summary>
		/// <param name="e">The event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			try
			{
				//	Initialize the child controls
				m_ctrlFolder.Text = m_eFolder.ToString();
				m_ctrlOverride.Text = m_strOverride;

				//	Add the tool tips
				if(m_strCasePath.Length > 0)
					m_ctrlToolTips.SetToolTip(m_ctrlFolder, m_strCasePath);
				m_ctrlToolTips.SetToolTip(m_ctrlBrowse, "Browse");
				m_ctrlToolTips.SetToolTip(m_ctrlReset, "Clear");
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnLoad", m_tmaxErrorBuilder.Message(ERROR_ON_LOAD_EX, Folder.ToString()), Ex);
			}
			
			//	Do the base class processing
			base.OnLoad(e);
			
		}// private void OnLoad(System.EventArgs e)
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>This method handles events fired when the user clicks on the reset button"</summary>
		/// <param name="sender">The reset button control</param>
		/// <param name="e">The system event arguments</param>
		private void OnClickReset(object sender, System.EventArgs e)
		{
			//	Clear the override path
			Override = "";
		}

		/// <summary>This method handles events fired when the user clicks on the Browse button"</summary>
		/// <param name="sender">The Browse button control</param>
		/// <param name="e">The system event arguments</param>
		private void OnClickBrowse(object sender, System.EventArgs e)
		{
			try
			{
				FTI.Shared.CBrowseForFolder bff = new CBrowseForFolder();
				
				bff.Prompt = "Select the new folder : ";
				bff.NoNewFolder = false;
				
				if(Override.Length > 0)
					bff.Folder = Override;

				if(bff.ShowDialog(this.Handle) == DialogResult.OK)
				{
					Override = bff.Folder.ToLower();
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnClickBrowse", m_tmaxErrorBuilder.Message(ERROR_ON_CLICK_BROWSE_EX, Folder.ToString()), Ex);
			}
		
		}// private void OnClickBrowse(object sender, System.EventArgs e)

		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CTmaxPathOverrideCtrl));
			this.m_ctrlFolder = new System.Windows.Forms.Label();
			this.m_ctrlOverride = new System.Windows.Forms.TextBox();
			this.m_ctrlBrowse = new System.Windows.Forms.Button();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlReset = new System.Windows.Forms.Button();
			this.m_ctrlToolTips = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// m_ctrlFolder
			// 
			this.m_ctrlFolder.Location = new System.Drawing.Point(0, 4);
			this.m_ctrlFolder.Name = "m_ctrlFolder";
			this.m_ctrlFolder.Size = new System.Drawing.Size(76, 20);
			this.m_ctrlFolder.TabIndex = 1;
			this.m_ctrlFolder.Text = "Folder";
			this.m_ctrlFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlOverride
			// 
			this.m_ctrlOverride.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOverride.Location = new System.Drawing.Point(76, 4);
			this.m_ctrlOverride.Name = "m_ctrlOverride";
			this.m_ctrlOverride.Size = new System.Drawing.Size(144, 20);
			this.m_ctrlOverride.TabIndex = 2;
			this.m_ctrlOverride.Text = "";
			// 
			// m_ctrlBrowse
			// 
			this.m_ctrlBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlBrowse.ImageIndex = 0;
			this.m_ctrlBrowse.ImageList = this.m_ctrlImages;
			this.m_ctrlBrowse.Location = new System.Drawing.Point(224, 4);
			this.m_ctrlBrowse.Name = "m_ctrlBrowse";
			this.m_ctrlBrowse.Size = new System.Drawing.Size(24, 20);
			this.m_ctrlBrowse.TabIndex = 3;
			this.m_ctrlBrowse.Click += new System.EventHandler(this.OnClickBrowse);
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.m_ctrlImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// m_ctrlReset
			// 
			this.m_ctrlReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlReset.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.m_ctrlReset.ImageIndex = 1;
			this.m_ctrlReset.ImageList = this.m_ctrlImages;
			this.m_ctrlReset.Location = new System.Drawing.Point(252, 4);
			this.m_ctrlReset.Name = "m_ctrlReset";
			this.m_ctrlReset.Size = new System.Drawing.Size(24, 20);
			this.m_ctrlReset.TabIndex = 4;
			this.m_ctrlReset.Click += new System.EventHandler(this.OnClickReset);
			// 
			// CTmaxPathOverrideCtrl
			// 
			this.Controls.Add(this.m_ctrlReset);
			this.Controls.Add(this.m_ctrlBrowse);
			this.Controls.Add(this.m_ctrlOverride);
			this.Controls.Add(this.m_ctrlFolder);
			this.Name = "CTmaxPathOverrideCtrl";
			this.Size = new System.Drawing.Size(280, 28);
			this.Tag = "";
			this.ResumeLayout(false);

		}// private void InitializeComponent()
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			Debug.Assert(m_tmaxErrorBuilder != null);
			Debug.Assert(m_tmaxErrorBuilder.FormatStrings != null);
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to initialize the path override editor: FolderType = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to browse for the override folder: FolderType = %1");
			
		}// protected void SetErrorStrings()

		#endregion Private Methods

		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get	{ return m_tmaxEventSource; }
		}		
		
		/// <summary>The default (no override) path assigned by the case</summary>
		public string CasePath
		{
			get 
			{
				return m_strCasePath; 
			}
			set 
			{ 
				m_strCasePath = value; 
			
				//	Set the tooltip for the folder control to display this text
				if((m_ctrlToolTips != null) && (m_ctrlFolder != null) && (m_ctrlFolder.IsDisposed == false))
				{
					try { m_ctrlToolTips.SetToolTip(m_ctrlFolder, m_strCasePath); }
					catch {}
				}
			
			}
		
		}
		
		/// <summary>The override path for the specified folder type</summary>
		public string Override
		{
			get 
			{ 
				//	Make sure we have the latest text
				if((m_ctrlOverride != null) && (m_ctrlOverride.IsDisposed == false))
					m_strOverride = m_ctrlOverride.Text;

				return m_strOverride; 
			}
			set 
			{ 
				m_strOverride = value; 
				
				//	Update the text
				if((m_ctrlOverride != null) && (m_ctrlOverride.IsDisposed == false))
					m_ctrlOverride.Text = m_strOverride;
			}
		
		}
		
		/// <summary>The enumerated media folder identifier</summary>
		public FTI.Shared.Trialmax.TmaxCaseFolders Folder
		{
			get 
			{ 
				return m_eFolder; 
			}
			set 
			{ 
				m_eFolder = value; 
				
				//	Update the text
				if((m_ctrlFolder != null) && (m_ctrlFolder.IsDisposed == false))
					m_ctrlFolder.Text = m_eFolder.ToString();
			}
		
		}
		
		#endregion Properties
		
	}// public class CTmaxPathOverrideCtrl : System.Windows.Forms.UserControl

}// namespace FTI.Trialmax.Controls
