using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form prompts the user for information needed to create a new binder</summary>
	public class CFVersionWarnings : System.Windows.Forms.Form
	{
		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components;

		/// <summary>Button to accept the operation</summary>
		private System.Windows.Forms.Button m_ctrlOk;

		/// <summary>Local member bound to Warnings property</summary>
		private ArrayList m_aWarnings = null;
		
		/// <summary>Local member bound to DatabaseVersion property</summary>
		private string m_strDatabaseVersion = "";
		
		/// <summary>Static text control to display a general warning message</summary>
		private System.Windows.Forms.Label m_ctrlMessage;
		private FTI.Trialmax.Controls.CTmaxListViewCtrl m_ctrlWarnings;
		private System.Windows.Forms.ImageList m_ctrlImages;

		/// <summary>Local member bound to AssemblyVersion property</summary>
		private string m_strAssemblyVersion = "";

		#endregion Private Members

		#region Public Methods
		
		public CFVersionWarnings()
		{
			InitializeComponent();
		}

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Traps the event fired when the form gets loaded for the first time</summary>
		/// <param name="e">The event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			string strMsg = "";
			
			strMsg = String.Format("The database you've just opened was created with TrialMax (ver. {0}). You are using (ver. {1}). You should update your installation as soon as possible.", m_strDatabaseVersion, m_strAssemblyVersion);
			m_ctrlMessage.Text = strMsg;
			
			if((m_aWarnings != null) && (m_aWarnings.Count > 0))
			{
				m_ctrlWarnings.OwnerImages = m_ctrlImages;
				m_ctrlWarnings.Initialize((ITmaxListViewCtrl)(m_aWarnings[0]));
				m_ctrlWarnings.Add(m_aWarnings, true);
			}
			else
			{
				m_ctrlOk.Location = new Point(m_ctrlOk.Left, m_ctrlWarnings.Top);
				m_ctrlWarnings.Visible = false;
				this.Size = new Size(this.Width, this.Height - m_ctrlWarnings.Height - 4);
			}
			
			//	Do the base class processing
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

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		protected void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFVersionWarnings));
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlMessage = new System.Windows.Forms.Label();
			this.m_ctrlWarnings = new FTI.Trialmax.Controls.CTmaxListViewCtrl();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(320, 164);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 17;
			this.m_ctrlOk.Text = "&OK";
			// 
			// m_ctrlMessage
			// 
			this.m_ctrlMessage.BackColor = System.Drawing.SystemColors.Control;
			this.m_ctrlMessage.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlMessage.Name = "m_ctrlMessage";
			this.m_ctrlMessage.Size = new System.Drawing.Size(388, 48);
			this.m_ctrlMessage.TabIndex = 18;
			// 
			// m_ctrlWarnings
			// 
			this.m_ctrlWarnings.AddTop = false;
			this.m_ctrlWarnings.AutoResizeColumns = true;
			this.m_ctrlWarnings.ClearOnDblClick = false;
			this.m_ctrlWarnings.HideSelection = false;
			this.m_ctrlWarnings.Location = new System.Drawing.Point(8, 64);
			this.m_ctrlWarnings.MaxRows = 0;
			this.m_ctrlWarnings.Name = "m_ctrlWarnings";
			this.m_ctrlWarnings.OwnerImages = null;
			this.m_ctrlWarnings.SelectedIndex = -1;
			this.m_ctrlWarnings.ShowHeaders = true;
			this.m_ctrlWarnings.ShowImage = true;
			this.m_ctrlWarnings.Size = new System.Drawing.Size(388, 92);
			this.m_ctrlWarnings.TabIndex = 19;
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// CFVersionWarnings
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(406, 195);
			this.Controls.Add(this.m_ctrlWarnings);
			this.Controls.Add(this.m_ctrlMessage);
			this.Controls.Add(this.m_ctrlOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFVersionWarnings";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Version Warnings";
			this.ResumeLayout(false);

		}
		
		#endregion Protected Methods

		#region Properties
		
		/// <summary>The array of warning messages</summary>
		public ArrayList Warnings
		{
			get	{	return m_aWarnings;	}
			set	{	m_aWarnings = value;	}
		}
		
		/// <summary>Database version string</summary>
		public string DatabaseVersion
		{
			get	{	return m_strDatabaseVersion;	}
			set	{	m_strDatabaseVersion = value;	}
		}
		
		/// <summary>Assembly version string</summary>
		public string AssemblyVersion
		{
			get	{	return m_strAssemblyVersion;	}
			set	{	m_strAssemblyVersion = value;	}
		}
		
		#endregion Properties
	
	}// public class CFVersionWarnings : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Forms
