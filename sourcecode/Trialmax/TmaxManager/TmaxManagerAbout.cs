using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace FTI.Trialmax.TmaxManager
{
	/// <summary>This form implements the TmaxManager About box</summary>
	public class CAboutForm : System.Windows.Forms.Form
	{
		#region Private Members

		/// <summary>Control used to display the TrialMax bitmap</summary>
		private System.Windows.Forms.PictureBox m_ctrlPicture;

		/// <summary>Component container require by form designer</summary>
		private System.ComponentModel.Container components = null;

		/// <summary>Status bar control to display current version</summary>
		private Infragistics.Win.UltraWinStatusBar.UltraStatusBar m_ctrlStatusBar;

		/// <summary>Pushbutton to display version history file</summary>
		private Button m_ctrlShowHistory;

		/// <summary>Private member bound to VersionsFileSpec property</summary>
		private string m_strVersionsFileSpec = "";
		
		#endregion Private Members
		
		#region Public Methods
		
		public CAboutForm()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
		}
		
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

		/// <summary>Called when the form window gets loaded</summary>
		/// <param name="e">Load event arguments</param>
		protected override void OnLoad(EventArgs e)
		{

			CTmaxManagerVersion ver = new CTmaxManagerVersion();
			m_ctrlStatusBar.Text = "TrialMax Case Manager ver. " + ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.QEF.ToString();

			if((m_strVersionsFileSpec.Length == 0) || (System.IO.File.Exists(m_strVersionsFileSpec) == false))
			{
				m_ctrlShowHistory.Visible = false;
				
				m_ctrlStatusBar.Width = m_ctrlPicture.Width;
				m_ctrlStatusBar.Appearance.TextHAlign = Infragistics.Win.HAlign.Center;
			
			}
			
			//	Do the base class processing
			base.OnLoad(e);

		}// protected override void OnLoad(EventArgs e)

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CAboutForm));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.m_ctrlPicture = new System.Windows.Forms.PictureBox();
            this.m_ctrlStatusBar = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.m_ctrlShowHistory = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.m_ctrlPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // m_ctrlPicture
            // 
            this.m_ctrlPicture.BackColor = System.Drawing.SystemColors.Desktop;
            this.m_ctrlPicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ctrlPicture.Image = ((System.Drawing.Image)(resources.GetObject("m_ctrlPicture.Image")));
            this.m_ctrlPicture.Location = new System.Drawing.Point(0, 0);
            this.m_ctrlPicture.Name = "m_ctrlPicture";
            this.m_ctrlPicture.Size = new System.Drawing.Size(320, 240);
            this.m_ctrlPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_ctrlPicture.TabIndex = 1;
            this.m_ctrlPicture.TabStop = false;
            // 
            // m_ctrlStatusBar
            // 
            this.m_ctrlStatusBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            appearance1.BackColor2 = System.Drawing.Color.Black;
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.TextHAlignAsString = "Left";
            this.m_ctrlStatusBar.Appearance = appearance1;
            this.m_ctrlStatusBar.Dock = System.Windows.Forms.DockStyle.None;
            this.m_ctrlStatusBar.Location = new System.Drawing.Point(0, 217);
            this.m_ctrlStatusBar.Name = "m_ctrlStatusBar";
            this.m_ctrlStatusBar.Size = new System.Drawing.Size(220, 23);
            this.m_ctrlStatusBar.TabIndex = 2;
            this.m_ctrlStatusBar.Text = "TmaxManager ver. 1.1.1";
            // 
            // m_ctrlShowHistory
            // 
            this.m_ctrlShowHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ctrlShowHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.m_ctrlShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.m_ctrlShowHistory.ForeColor = System.Drawing.Color.White;
            this.m_ctrlShowHistory.Location = new System.Drawing.Point(220, 217);
            this.m_ctrlShowHistory.Name = "m_ctrlShowHistory";
            this.m_ctrlShowHistory.Size = new System.Drawing.Size(100, 23);
            this.m_ctrlShowHistory.TabIndex = 3;
            this.m_ctrlShowHistory.TabStop = false;
            this.m_ctrlShowHistory.Text = "&Show History";
            this.m_ctrlShowHistory.UseVisualStyleBackColor = false;
            this.m_ctrlShowHistory.Click += new System.EventHandler(this.OnClickShowHistory);
            // 
            // CAboutForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(320, 240);
            this.Controls.Add(this.m_ctrlShowHistory);
            this.Controls.Add(this.m_ctrlStatusBar);
            this.Controls.Add(this.m_ctrlPicture);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CAboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.m_ctrlPicture)).EndInit();
            this.ResumeLayout(false);

		}

		/// <summary>Called when the user clicks on the Show History button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The system event arguments</param>
		private void OnClickShowHistory(object sender, EventArgs e)
		{
			try
			{
				//	Does the file exist?
				if(System.IO.File.Exists(m_strVersionsFileSpec) == true)
				{
					System.Diagnostics.Process.Start(m_strVersionsFileSpec);
				}
				else
				{
					MessageBox.Show("Unable to locate " + m_strVersionsFileSpec, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			
			}
			catch
			{
			
			}
						
		}
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>The fully qualified path to the versions history file</summary>
		public string VersionsFileSpec
		{
			get { return m_strVersionsFileSpec; }
			set { m_strVersionsFileSpec = value; }
		}
		
		#endregion Properties


	}// public class CAboutForm : System.Windows.Forms.Form

}// namespace FTI.Trialmax.TmaxManager

