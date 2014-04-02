using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace FTI.Trialmax.Database
{
	/// <summary>Form to display status during a repair operation</summary>
	public class CFRepairStatus : System.Windows.Forms.Form
	{
		#region Private Members
		
		/// <summary>Static text control to display status message</summary>
		private System.Windows.Forms.Label m_ctrlMessage;
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;

		/// <summary>Local member bound to Message property</summary>
		private string m_strMessage = "";

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFRepairStatus()
		{
			InitializeComponent();
		}

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Overridden base class member called when the window gets created</summary>
		/// <param name="e">System event arguements</param>
		protected override void OnLoad(EventArgs e)
		{
			m_ctrlMessage.Text = m_strMessage;
			base.OnLoad (e);
		}

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
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.m_ctrlMessage = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlMessage
			// 
			this.m_ctrlMessage.Location = new System.Drawing.Point(8, 12);
			this.m_ctrlMessage.Name = "m_ctrlMessage";
			this.m_ctrlMessage.Size = new System.Drawing.Size(460, 28);
			this.m_ctrlMessage.TabIndex = 0;
			// 
			// CFRepairStatus
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(476, 45);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlMessage);
			this.Name = "CFRepairStatus";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Repair Status";
			this.ResumeLayout(false);

		}
		
		#endregion Private Methods
		
		#region Properties
		
		public string Message
		{
			get
			{
				return m_strMessage;
			}
			set
			{
				m_strMessage = value;
				
				if((m_ctrlMessage != null) && (m_ctrlMessage.IsDisposed == false))
				{
					m_ctrlMessage.Text = m_strMessage;
					m_ctrlMessage.Refresh();
				}
				
			}
			
		}
		
		#endregion Properties
	
	}// public class CFRepairStatus : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Database
