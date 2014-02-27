using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace FTI.Trialmax.TMVV.TmaxVideo
{
	/// <summary>
	/// Summary description for About.
	/// </summary>
	public class CTmaxVideoAbout : System.Windows.Forms.Form
	{
		private System.Windows.Forms.StatusBar m_ctrlStatusBar;
		private System.Windows.Forms.PictureBox m_ctrlPicture;
		private System.Windows.Forms.StatusBarPanel version;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CTmaxVideoAbout()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			CTmaxVideoVersion ver = new CTmaxVideoVersion();
			m_ctrlStatusBar.Panels[0].Text = "TrialMax Video Viewer ver. " + ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.QEF.ToString();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CTmaxVideoAbout));
			this.m_ctrlStatusBar = new System.Windows.Forms.StatusBar();
			this.version = new System.Windows.Forms.StatusBarPanel();
			this.m_ctrlPicture = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.version)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlStatusBar
			// 
			this.m_ctrlStatusBar.Location = new System.Drawing.Point(0, 243);
			this.m_ctrlStatusBar.Name = "m_ctrlStatusBar";
			this.m_ctrlStatusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																							   this.version});
			this.m_ctrlStatusBar.ShowPanels = true;
			this.m_ctrlStatusBar.Size = new System.Drawing.Size(322, 20);
			this.m_ctrlStatusBar.SizingGrip = false;
			this.m_ctrlStatusBar.TabIndex = 0;
			// 
			// version
			// 
			this.version.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			this.version.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.version.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
			this.version.Text = "Trialmax Version Goes Here";
			this.version.Width = 322;
			// 
			// m_ctrlPicture
			// 
			this.m_ctrlPicture.BackColor = System.Drawing.SystemColors.Control;
			this.m_ctrlPicture.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlPicture.Image = ((System.Drawing.Image)(resources.GetObject("m_ctrlPicture.Image")));
			this.m_ctrlPicture.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlPicture.Name = "m_ctrlPicture";
			this.m_ctrlPicture.Size = new System.Drawing.Size(322, 243);
			this.m_ctrlPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.m_ctrlPicture.TabIndex = 1;
			this.m_ctrlPicture.TabStop = false;
			// 
			// CTmaxVideoAbout
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(322, 263);
			this.Controls.Add(this.m_ctrlPicture);
			this.Controls.Add(this.m_ctrlStatusBar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CTmaxVideoAbout";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About";
			((System.ComponentModel.ISupportInitialize)(this.version)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
	}
}
