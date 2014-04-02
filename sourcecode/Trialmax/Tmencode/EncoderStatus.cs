using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Encode
{
	/// <summary>This form is used to display status during an encoder operation</summary>
	public class CFEncoderStatus : System.Windows.Forms.Form
	{
		#region Private Members
		
		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>Progress bar used to indicate activity</summary>
		private System.Windows.Forms.ProgressBar m_ctrlProgressBar;
		
		/// <summary>Status message displayed during the operation</summary>
		private System.Windows.Forms.Label m_ctrlStatus;
		
		/// <summary>Pushbutton to allow the user to cancel the operation</summary>
		private System.Windows.Forms.Button m_ctrlCancel;

		/// <summary>Path to the file being encoded</summary>
		private System.Windows.Forms.Label m_ctrlFileSpec;
		
		/// <summary>Local member bound to FileSpec property</summary>
		private string m_strFileSpec = "";
		
		/// <summary>Local member bound to Cancelled property</summary>
		private bool m_bCancelled = false;
		
		/// <summary>Local member bound to Failed property</summary>
		private bool m_bFailed = false;
		
		/// <summary>Local member bound to ShowCancel property</summary>
		private bool m_bShowCancel = true;
		
		/// <summary>Static text label for file path control</summary>
		private System.Windows.Forms.Label m_ctrlFileSpecLabel;
		
		/// <summary>Static text label for status message control</summary>
		private System.Windows.Forms.Label m_ctrlStatusLabel;
		
		/// <summary>Local member bound to Status property</summary>
		private string m_strStatus = "";
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFEncoderStatus()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

		}

		/// <summary>Called to set the value of the FileSpec property</summary>
		/// <param name="strFileSpec">The path to the output file</param>
		public void SetFileSpec(string strFileSpec)
		{
			try
			{
				if(strFileSpec != null)
				{
					m_strFileSpec = strFileSpec.ToLower();
					m_ctrlFileSpec.Text = CTmaxToolbox.FitPathToWidth(m_strFileSpec, m_ctrlFileSpec);
				}
				else
				{
					m_strFileSpec = "";
					m_ctrlFileSpec.Text = "";
				}
			
			}
			catch
			{
			}
			
		}// public void SetFileSpec(string strFileSpec)
		
		/// <summary>Called to set the value of the Status property</summary>
		/// <param name="strStatus">The path to the output file</param>
		public void SetStatus(string strStatus)
		{
			try
			{
				if(strStatus != null)
				{
					m_strStatus = strStatus.ToLower();
					m_ctrlStatus.Text = CTmaxToolbox.FitPathToWidth(m_strStatus, m_ctrlStatus);
				}
				else
				{
					m_strStatus = "";
					m_ctrlStatus.Text = "";
			
				}
			
			}
			catch
			{
			}
			
		}// public void SetStatus(string strStatus)
		
		/// <summary>This method is called to step the progress bar</summary>
		public void StepProgress()
		{
			//	Step the progress bar
			try
			{
				if(m_ctrlProgressBar.Value + m_ctrlProgressBar.Step > m_ctrlProgressBar.Maximum)
					m_ctrlProgressBar.Value = m_ctrlProgressBar.Minimum;
				else
					m_ctrlProgressBar.PerformStep();
						
				m_ctrlProgressBar.Refresh();
				Application.DoEvents();
			}
			catch
			{
			}
		
		}// public void StepProgress()
		
		/// <summary>This method is called to set the progress bar value</summary>
		/// <param name="iPercent">The percent complete (0 - 100)</param>
		public void SetProgress(int iPercent)
		{
			//	Set the progress bar
			try
			{
				if(iPercent > m_ctrlProgressBar.Maximum)
					iPercent = m_ctrlProgressBar.Maximum;
				else if(iPercent < m_ctrlProgressBar.Minimum)
					iPercent = m_ctrlProgressBar.Minimum;
					
				m_ctrlProgressBar.Value = iPercent;
				m_ctrlProgressBar.Refresh();
				Application.DoEvents();
			}
			catch
			{
			}
		
		}// public void StepProgress()
		
		/// <summary>Called to set the value of the ShowCancel property</summary>
		/// <param name="bShowCancel">true to show the cancel button</param>
		public void SetShowCancel(bool bShowCancel)
		{
			try
			{
				m_bShowCancel = bShowCancel;
				
				if(m_ctrlCancel != null)
					m_ctrlCancel.Visible = bShowCancel;
			}
			catch
			{
			}
			
		}// public void SetShowCancel(bool bShowCancel)
		
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
		}
		
		/// <summary>Called when form window gets created</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Show/hide the cancel button
			SetShowCancel(m_bShowCancel);
			
			//	Do the base class processing
			base.OnLoad (e);
		}

		#endregion Protected Methods
		
		#region Private Methods

		/// <summary>Required by form designer to initialize child controls</summary>
		private void InitializeComponent()
		{
			this.m_ctrlProgressBar = new System.Windows.Forms.ProgressBar();
			this.m_ctrlStatus = new System.Windows.Forms.Label();
			this.m_ctrlFileSpec = new System.Windows.Forms.Label();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlFileSpecLabel = new System.Windows.Forms.Label();
			this.m_ctrlStatusLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlProgressBar
			// 
			this.m_ctrlProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlProgressBar.Location = new System.Drawing.Point(64, 56);
			this.m_ctrlProgressBar.Name = "m_ctrlProgressBar";
			this.m_ctrlProgressBar.Size = new System.Drawing.Size(376, 16);
			this.m_ctrlProgressBar.Step = 5;
			this.m_ctrlProgressBar.TabIndex = 7;
			// 
			// m_ctrlStatus
			// 
			this.m_ctrlStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlStatus.Location = new System.Drawing.Point(68, 32);
			this.m_ctrlStatus.Name = "m_ctrlStatus";
			this.m_ctrlStatus.Size = new System.Drawing.Size(372, 16);
			this.m_ctrlStatus.TabIndex = 6;
			// 
			// m_ctrlFileSpec
			// 
			this.m_ctrlFileSpec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlFileSpec.Location = new System.Drawing.Point(68, 8);
			this.m_ctrlFileSpec.Name = "m_ctrlFileSpec";
			this.m_ctrlFileSpec.Size = new System.Drawing.Size(372, 16);
			this.m_ctrlFileSpec.TabIndex = 8;
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.Location = new System.Drawing.Point(364, 78);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 9;
			this.m_ctrlCancel.Text = "&Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlFileSpecLabel
			// 
			this.m_ctrlFileSpecLabel.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlFileSpecLabel.Name = "m_ctrlFileSpecLabel";
			this.m_ctrlFileSpecLabel.Size = new System.Drawing.Size(56, 16);
			this.m_ctrlFileSpecLabel.TabIndex = 10;
			this.m_ctrlFileSpecLabel.Text = "Filename:";
			// 
			// m_ctrlStatusLabel
			// 
			this.m_ctrlStatusLabel.Location = new System.Drawing.Point(8, 32);
			this.m_ctrlStatusLabel.Name = "m_ctrlStatusLabel";
			this.m_ctrlStatusLabel.Size = new System.Drawing.Size(56, 16);
			this.m_ctrlStatusLabel.TabIndex = 11;
			this.m_ctrlStatusLabel.Text = "Status:";
			// 
			// CFEncoderStatus
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(448, 107);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlStatusLabel);
			this.Controls.Add(this.m_ctrlFileSpecLabel);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlFileSpec);
			this.Controls.Add(this.m_ctrlProgressBar);
			this.Controls.Add(this.m_ctrlStatus);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "CFEncoderStatus";
			this.ShowInTaskbar = false;
			this.Text = "Encoder Status";
			this.TopMost = true;
			this.ResumeLayout(false);

		}
		
		/// <summary>Called when the user clicks on the Cancel button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">System event arguments</param>
		private void OnClickCancel(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			m_bCancelled = true;
			this.Close();
		}
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>Fully qualified path to the output file</summary>
		public string FileSpec
		{
			get { return m_strFileSpec; }
			set { SetFileSpec(value); }
		}
		
		/// <summary>Fully qualified path to the output file</summary>
		public string Status
		{
			get { return m_strStatus; }
			set { SetStatus(value); }
		}
		
		/// <summary>True if operation cancelled by the user</summary>
		public bool Cancelled
		{
			get { return m_bCancelled; }
			set { m_bCancelled = value; }
		}
		
		/// <summary>True to show the Cancel button</summary>
		public bool ShowCancel
		{
			get { return m_bShowCancel; }
			set { SetShowCancel(value); }
		}
		
		/// <summary>True if operation failed</summary>
		public bool Failed
		{
			get { return m_bFailed; }
			set { m_bFailed = value; }
		}
		
		#endregion Properties
		
	}// public class CFEncoderStatus : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Encode
