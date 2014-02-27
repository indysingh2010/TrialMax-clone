using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Timers;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form is used to display status during a database compact operation</summary>
	public class CFCompactorStatus : System.Windows.Forms.Form
	{
		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.Container components = null;

		/// <summary>Progress bar used to indicate activity</summary>
		private System.Windows.Forms.ProgressBar m_ctrlProgressBar;

		/// <summary>Path to the file being encoded</summary>
		private System.Windows.Forms.Label m_ctrlFileSpec;

		/// <summary>Local member bound to FileSpec property</summary>
		private string m_strFileSpec = "";

		/// <summary>Static text label for file path control</summary>
		private System.Windows.Forms.Label m_ctrlFileSpecLabel;

		/// <summary>Local member to store the internal timer</summary>
		private System.Timers.Timer m_Timer = null;

		#endregion Private Members

		#region Public Methods

		/// <summary>Constructor</summary>
		public CFCompactorStatus()
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

		#endregion Public Methods

		#region Protected Methods

		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				//	Just in case...
				KillTimer();

				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>Called when form window gets created</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Do the base class processing
			base.OnLoad(e);
			
			//	Start the progress timer
			SetTimer(250);

		}// protected override void OnLoad(EventArgs e)

		#endregion Protected Methods

		#region Private Methods

		/// <summary>Required by form designer to initialize child controls</summary>
		private void InitializeComponent()
		{
			this.m_ctrlProgressBar = new System.Windows.Forms.ProgressBar();
			this.m_ctrlFileSpec = new System.Windows.Forms.Label();
			this.m_ctrlFileSpecLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlProgressBar
			// 
			this.m_ctrlProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlProgressBar.Location = new System.Drawing.Point(11, 38);
			this.m_ctrlProgressBar.Maximum = 10;
			this.m_ctrlProgressBar.Name = "m_ctrlProgressBar";
			this.m_ctrlProgressBar.Size = new System.Drawing.Size(428, 16);
			this.m_ctrlProgressBar.Step = 1;
			this.m_ctrlProgressBar.TabIndex = 7;
			// 
			// m_ctrlFileSpec
			// 
			this.m_ctrlFileSpec.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlFileSpec.Location = new System.Drawing.Point(68, 9);
			this.m_ctrlFileSpec.Name = "m_ctrlFileSpec";
			this.m_ctrlFileSpec.Size = new System.Drawing.Size(372, 16);
			this.m_ctrlFileSpec.TabIndex = 8;
			// 
			// m_ctrlFileSpecLabel
			// 
			this.m_ctrlFileSpecLabel.Location = new System.Drawing.Point(8, 9);
			this.m_ctrlFileSpecLabel.Name = "m_ctrlFileSpecLabel";
			this.m_ctrlFileSpecLabel.Size = new System.Drawing.Size(56, 16);
			this.m_ctrlFileSpecLabel.TabIndex = 10;
			this.m_ctrlFileSpecLabel.Text = "Filename:";
			// 
			// CFCompactorStatus
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(448, 66);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlFileSpecLabel);
			this.Controls.Add(this.m_ctrlFileSpec);
			this.Controls.Add(this.m_ctrlProgressBar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "CFCompactorStatus";
			this.ShowInTaskbar = false;
			this.Text = "Compacting Database";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.OnClosing);
			this.ResumeLayout(false);

		}

		/// <summary>This method is called to step the progress bar</summary>
		private void StepProgress()
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

		/// <summary>Handles events fired when the form is closing</summary>
		/// <param name="sender">the object firing the event</param>
		/// <param name="e">the system event arguments</param>
		private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			KillTimer();
			
		}// private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)

		/// <summary>Called when the timer period expires</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">System event arguments</param>
		private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			StepProgress();

		}// private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)

		/// <summary>Called to set the update timer</summary>
		/// <param name="dInterval">The interval used to fire timer events</param>
		/// <returns>true if successful</returns>
		private bool SetTimer(double dInterval)
		{
			try
			{
				m_Timer = new System.Timers.Timer();
				m_Timer.Interval = dInterval;
				m_Timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimerElapsed);
				
				m_Timer.Start();
			}
			catch
			{
				m_Timer = null;
			}
			
			return (m_Timer != null);

		}// private bool SetTimer(double dInterval)		
		
		/// <summary>Called to kill the update timer</summary>
		private void KillTimer()
		{
			try
			{
				if(m_Timer != null)
					m_Timer.Stop();
			}
			catch
			{
			}
			finally
			{
				m_Timer = null;
			}

		}// private void KillTimer()
		
		#endregion Private Methods

		#region Properties

		/// <summary>Fully qualified path to the output file</summary>
		public string FileSpec
		{
			get { return m_strFileSpec; }
			set { SetFileSpec(value); }
		}

		#endregion Properties

	}// public class CFCompactorStatus : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Forms
