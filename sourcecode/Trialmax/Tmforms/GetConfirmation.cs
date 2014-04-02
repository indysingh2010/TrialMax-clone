using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Controls;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form allows the user to edit the properties of a meta field</summary>
	public class CFGetConfirmation : CFTmaxBaseForm
	{
		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private IContainer components = null;

		/// <summary>The form's No button</summary>
		private System.Windows.Forms.Button m_ctrlNo;

		/// <summary>The form's Yes button</summary>
		private System.Windows.Forms.Button m_ctrlYes;

		/// <summary>Container for the question mark icon</summary>
		private System.Windows.Forms.PictureBox m_ctrlIconHolder;

		/// <summary>Displays the specified Message text</summary>
		private System.Windows.Forms.Label m_ctrlMessage;

		/// <summary>Check box to select the Apply To All option</summary>
		private System.Windows.Forms.CheckBox m_ctrlApplyAll;

		/// <summary>Local member bound to Message property</summary>
		private string m_strMessage = "Confirm";

		/// <summary>Local member bound to ApplyAllLabel property</summary>
		private string m_strApplyAllLabel = "Apply To All";

		/// <summary>Local member bound to ApplyAll property</summary>
		private bool m_bApplyAll = false;
		
		#endregion Private Members
		
		#region Public Methods
		
		public CFGetConfirmation()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
		}// public CFGetConfirmation()

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
		
		/// <summary>This method handles the form's Load event</summary>
		/// <param name="e">The event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			if(m_strMessage.Length > 0)
				m_ctrlMessage.Text = m_strMessage;
			if(m_strApplyAllLabel.Length > 0)
				m_ctrlApplyAll.Text = m_strApplyAllLabel;
			m_ctrlApplyAll.Checked = m_bApplyAll;
			
			base.OnLoad(e);
			
		}// private void OnLoad(object sender, System.EventArgs e)
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFGetConfirmation));
			this.m_ctrlNo = new System.Windows.Forms.Button();
			this.m_ctrlYes = new System.Windows.Forms.Button();
			this.m_ctrlIconHolder = new System.Windows.Forms.PictureBox();
			this.m_ctrlMessage = new System.Windows.Forms.Label();
			this.m_ctrlApplyAll = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlIconHolder)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlNo
			// 
			this.m_ctrlNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlNo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlNo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlNo.Location = new System.Drawing.Point(278, 77);
			this.m_ctrlNo.Name = "m_ctrlNo";
			this.m_ctrlNo.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlNo.TabIndex = 2;
			this.m_ctrlNo.Text = "&No";
			this.m_ctrlNo.Click += new System.EventHandler(this.OnClickNo);
			// 
			// m_ctrlYes
			// 
			this.m_ctrlYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlYes.Location = new System.Drawing.Point(195, 77);
			this.m_ctrlYes.Name = "m_ctrlYes";
			this.m_ctrlYes.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlYes.TabIndex = 1;
			this.m_ctrlYes.Text = "&Yes";
			this.m_ctrlYes.Click += new System.EventHandler(this.OnClickYes);
			// 
			// m_ctrlIconHolder
			// 
			this.m_ctrlIconHolder.Image = ((System.Drawing.Image)(resources.GetObject("m_ctrlIconHolder.Image")));
			this.m_ctrlIconHolder.Location = new System.Drawing.Point(9, 5);
			this.m_ctrlIconHolder.Name = "m_ctrlIconHolder";
			this.m_ctrlIconHolder.Size = new System.Drawing.Size(32, 32);
			this.m_ctrlIconHolder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.m_ctrlIconHolder.TabIndex = 20;
			this.m_ctrlIconHolder.TabStop = false;
			// 
			// m_ctrlMessage
			// 
			this.m_ctrlMessage.Location = new System.Drawing.Point(55, 10);
			this.m_ctrlMessage.Name = "m_ctrlMessage";
			this.m_ctrlMessage.Size = new System.Drawing.Size(298, 27);
			this.m_ctrlMessage.TabIndex = 21;
			this.m_ctrlMessage.Text = "Message Appears Here";
			// 
			// m_ctrlApplyAll
			// 
			this.m_ctrlApplyAll.Location = new System.Drawing.Point(58, 45);
			this.m_ctrlApplyAll.Name = "m_ctrlApplyAll";
			this.m_ctrlApplyAll.Size = new System.Drawing.Size(295, 24);
			this.m_ctrlApplyAll.TabIndex = 0;
			this.m_ctrlApplyAll.Text = "Apply To All";
			// 
			// CFGetConfirmation
			// 
			this.AcceptButton = this.m_ctrlYes;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlNo;
			this.ClientSize = new System.Drawing.Size(365, 107);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlApplyAll);
			this.Controls.Add(this.m_ctrlMessage);
			this.Controls.Add(this.m_ctrlIconHolder);
			this.Controls.Add(this.m_ctrlYes);
			this.Controls.Add(this.m_ctrlNo);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFGetConfirmation";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " ";
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlIconHolder)).EndInit();
			this.ResumeLayout(false);

		}
	
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickNo(object sender, System.EventArgs e)
		{
			//	Close the form
			m_bApplyAll = m_ctrlApplyAll.Checked;
			DialogResult = DialogResult.No;
			this.Close();
		
		}// private void OnClickNo(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on Yes</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickYes(object sender, System.EventArgs e)
		{
			//	Close the form
			m_bApplyAll = m_ctrlApplyAll.Checked;
			DialogResult = DialogResult.Yes;
			this.Close();
		
		}// private void OnClickYes(object sender, System.EventArgs e)

		#endregion Private Methods

		#region Properties
		
		/// <summary>Message used to prompt the user</summary>
		public string Message
		{
			get { return m_strMessage;  }
			set { m_strMessage = value; }
		}

		/// <summary>True if user selects Apply To All option</summary>
		public bool ApplyAll
		{
			get { return m_bApplyAll; }
			set { m_bApplyAll = value; }
		}

		/// <summary>Text label for the Apply All check box</summary>
		public string ApplyAllLabel
		{
			get { return m_strApplyAllLabel; }
			set { m_strApplyAllLabel = value; }
		}

		#endregion Properties
	
	}// public class CFGetConfirmation : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
