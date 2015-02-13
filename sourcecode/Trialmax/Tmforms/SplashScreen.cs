using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

namespace FTI.Trialmax.Forms
{
	/// <summary>TrialMax Manager application splash screen</summary>
	public class CFSplashScreen : System.Windows.Forms.Form
	{
		#region Private Members
		
		/// <summary>Control used to display splash screen bitmap</summary>
		private System.Windows.Forms.PictureBox m_ctrlPictureBox;
		
		/// <summary>Control used to display initialization progress</summary>
		private Infragistics.Win.UltraWinProgressBar.UltraProgressBar m_ctrlProgress;
		
		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.Container components = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFSplashScreen()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
		}

		/// <summary>Called to start the initialization process</summary>
		public void Start()
		{
			try
			{
				//	Show the window
				this.Show();
				Application.DoEvents();
			}
			catch
			{
			}
			
		}// public void Start()

		/// <summary>Called to stop the initialization process</summary>
		public void Stop()
		{
			try
			{
				//	Hide this window
				this.Hide();
			}
			catch
			{
			}
			
		}// public void Stop()

		/// <summary>Called to set the progress message</summary>
		public void SetMessage(string strMessage)
		{
			if((m_ctrlProgress != null) && (m_ctrlProgress.IsDisposed == false))
			{
				try
				{
					if(strMessage != null)
						m_ctrlProgress.Text = strMessage;
					else
						m_ctrlProgress.Text = "";
						
					//	Force a repaint
					m_ctrlProgress.Refresh();
					Application.DoEvents();
						
				}
				catch
				{
				}
				
			}
		
		}// public void SetMessage(string strMessage)

		/// <summary>Called to step the progress bar to indicate activity</summary>
		public void StepProgress()
		{
			if((m_ctrlProgress != null) && (m_ctrlProgress.IsDisposed == false))
			{
				try
				{
					if(m_ctrlProgress.Value == m_ctrlProgress.Maximum)
						m_ctrlProgress.Value = m_ctrlProgress.Minimum;

					m_ctrlProgress.PerformStep();
					
					//	Force a repaint
					m_ctrlProgress.Refresh();
					Application.DoEvents();
				
				}
				catch
				{
				}
				
			}// if((m_ctrlProgress != null) && (m_ctrlProgress.IsDisposed == false))
		
		}// private void StepProgress()
	
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
			}
			base.Dispose(disposing);
		
		}// protected override void Dispose( bool disposing )

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFSplashScreen));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.m_ctrlPictureBox = new System.Windows.Forms.PictureBox();
            this.m_ctrlProgress = new Infragistics.Win.UltraWinProgressBar.UltraProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.m_ctrlPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // m_ctrlPictureBox
            // 
            this.m_ctrlPictureBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_ctrlPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("m_ctrlPictureBox.Image")));
            this.m_ctrlPictureBox.Location = new System.Drawing.Point(0, 0);
            this.m_ctrlPictureBox.Name = "m_ctrlPictureBox";
            this.m_ctrlPictureBox.Size = new System.Drawing.Size(320, 240);
            this.m_ctrlPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_ctrlPictureBox.TabIndex = 1;
            this.m_ctrlPictureBox.TabStop = false;
            // 
            // m_ctrlProgress
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            appearance1.BackColor2 = System.Drawing.Color.Black;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.TextHAlignAsString = "Center";
            this.m_ctrlProgress.Appearance = appearance1;
            this.m_ctrlProgress.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.m_ctrlProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance2.BackColor2 = System.Drawing.Color.Gray;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.HorizontalBump;
            appearance2.ForeColor = System.Drawing.Color.White;
            appearance2.TextHAlignAsString = "Left";
            this.m_ctrlProgress.FillAppearance = appearance2;
            this.m_ctrlProgress.Location = new System.Drawing.Point(0, 240);
            this.m_ctrlProgress.Name = "m_ctrlProgress";
            this.m_ctrlProgress.PercentFormat = "";
            this.m_ctrlProgress.Size = new System.Drawing.Size(320, 20);
            this.m_ctrlProgress.Step = 2;
            this.m_ctrlProgress.TabIndex = 2;
            this.m_ctrlProgress.Text = "[Formatted]";
            this.m_ctrlProgress.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // CFSplashScreen
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(320, 260);
            this.ControlBox = false;
            this.Controls.Add(this.m_ctrlProgress);
            this.Controls.Add(this.m_ctrlPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CFSplashScreen";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SplashScreen";
            ((System.ComponentModel.ISupportInitialize)(this.m_ctrlPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}// private void InitializeComponent()
		
		#endregion Private Methods

		#region Properties

		public System.Windows.Forms.PictureBox PictureBoxCtrl
		{
			get { return m_ctrlPictureBox; }
		}

		#endregion Properties

	}// public class CFSplashScreen : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Forms
