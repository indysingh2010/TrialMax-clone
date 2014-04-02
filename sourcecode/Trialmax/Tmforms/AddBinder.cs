using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form prompts the user for information needed to create a new binder</summary>
	public class CFAddBinder : System.Windows.Forms.Form
	{
		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.Container components = null;

		/// <summary>Button to cancel the operation</summary>
		private System.Windows.Forms.Button m_ctrlCancel;

		/// <summary>Button to accept the operation</summary>
		private System.Windows.Forms.Button m_ctrlOk;

		/// <summary>Local member bound to Name property</summary>
		private string m_strBinderName = "";

		/// <summary>Label for media name edit box</summary>
		private System.Windows.Forms.Label m_ctrlNameLabel;

		/// <summary>Edit box allowing user to specify the media name</summary>
		private System.Windows.Forms.TextBox m_ctrlName;

		#endregion Private Members

		#region Public Methods
		
		public CFAddBinder()
		{
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
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		protected void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFAddBinder));
			this.m_ctrlNameLabel = new System.Windows.Forms.Label();
			this.m_ctrlName = new System.Windows.Forms.TextBox();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// m_ctrlNameLabel
			// 
			this.m_ctrlNameLabel.Location = new System.Drawing.Point(8, 12);
			this.m_ctrlNameLabel.Name = "m_ctrlNameLabel";
			this.m_ctrlNameLabel.Size = new System.Drawing.Size(44, 20);
			this.m_ctrlNameLabel.TabIndex = 19;
			this.m_ctrlNameLabel.Text = "Name : ";
			this.m_ctrlNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlName
			// 
			this.m_ctrlName.Location = new System.Drawing.Point(52, 12);
			this.m_ctrlName.Name = "m_ctrlName";
			this.m_ctrlName.Size = new System.Drawing.Size(268, 20);
			this.m_ctrlName.TabIndex = 16;
			this.m_ctrlName.Text = "";
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(179, 48);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 18;
			this.m_ctrlCancel.Text = "  &Cancel";
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(75, 48);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 17;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// CFAddBinder
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(328, 81);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.m_ctrlNameLabel,
																		  this.m_ctrlName,
																		  this.m_ctrlCancel,
																		  this.m_ctrlOk});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "CFAddBinder";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Add Binder";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

		}
		
		/// <summary>Traps the event fired when the form gets loaded for the first time</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		protected void OnLoad(object sender, System.EventArgs e)
		{
			if(m_ctrlName != null)
				m_ctrlName.Text = m_strBinderName;
		}
		
		/// <summary>Traps the event fired when the user clicks on the Ok button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		protected void OnClickOk(object sender, System.EventArgs e)
		{
			if(m_ctrlName.Text.Length == 0)
			{
				FTI.Shared.Win32.User.MessageBeep(FTI.Shared.Win32.User.MB_ICONEXCLAMATION);
				MessageBox.Show("You must supply a name for the new binder", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				
			}
			else
			{
				//	Update the property values
				m_strBinderName = m_ctrlName.Text;

				// Close the form		
				DialogResult = DialogResult.OK;
				Close();
			}
			
		}
		
		#endregion Protected Methods

		#region Properties
		
		/// <summary>Name to be assigned to the binder</summary>
		public string BinderName
		{
			get	{	return m_strBinderName;	}
			set	{	m_strBinderName = value;	}
		}
		
		#endregion Properties
	
	}// public class CFAddBinder : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Forms
