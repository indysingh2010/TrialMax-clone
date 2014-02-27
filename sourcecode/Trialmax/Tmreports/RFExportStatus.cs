using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Reports
{
	/// <summary>This form is used to preview a report</summary>
	public class CRFExportStatus : FTI.Trialmax.Forms.CFTmaxBaseForm
	{
		#region Private Members
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>Control to display the status message</summary>
		private System.Windows.Forms.Label m_ctrlMessage;
		
		/// <summary>Local member bound to Message property</summary>
		private string m_strMessage = "";
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CRFExportStatus()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			m_tmaxEventSource.Name = "Export Status";
			
		}// public CRFExportStatus()

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
		
		/// <summary>Overridden base class member called when the form gets created</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Perform the base class processing first
			base.OnLoad(e);
			
			SetMessage(m_strMessage);
		
		}// protected override void OnLoad(EventArgs e)
		
		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CRFExportStatus));
			this.m_ctrlMessage = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlMessage
			// 
			this.m_ctrlMessage.Location = new System.Drawing.Point(16, 8);
			this.m_ctrlMessage.Name = "m_ctrlMessage";
			this.m_ctrlMessage.Size = new System.Drawing.Size(432, 40);
			this.m_ctrlMessage.TabIndex = 0;
			this.m_ctrlMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// CRFExportStatus
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(458, 55);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlMessage);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "CRFExportStatus";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Export Status";
			this.ResumeLayout(false);

		}
		
		/// <summary>This method is called to set the status message</summary>
		/// <param name="strMessage">The message to be displayed</param>
		private void SetMessage(string strMessage)
		{
			try
			{
				m_strMessage = strMessage;
				
				m_ctrlMessage.Text = CTmaxToolbox.FitPathToWidth(strMessage, m_ctrlMessage);
				
				m_ctrlMessage.Refresh();
			}
			catch
			{
			}

		}// private void SetMessage(string strMessage)
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>The text to be displayed</summary>
		public string Message
		{
			get { return m_strMessage; }
			set { SetMessage(value); }
		}
		
		#endregion Properties
		
	}// public class CRFExportStatus : System.Windows.Forms.Form
	
}// namespace FTI.Trialmax.Reports
