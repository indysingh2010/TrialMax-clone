using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace FTI.Trialmax.Controls
{
	/// <summary>This form allows the user to edit memo fields</summary>
	public class CTmaxMemoEditor : System.Windows.Forms.Form
	{
		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.Container components = null;

		/// <summary>Local member bound to Memo property</summary>
		private string m_strMemo = "";

		/// <summary>Ok button</summary>
		private System.Windows.Forms.Button m_ctrlOk;

		/// <summary>Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;

		/// <summary>Text editor control</summary>
		private System.Windows.Forms.RichTextBox m_ctrlEditor;

		#endregion Private Members
		
		#region Public Methods
		
		public CTmaxMemoEditor()
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
		}

		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		protected void OnClickOK(object sender, System.EventArgs e)
		{
			//	Update the memo text
			if(m_ctrlEditor != null)
				Memo = m_ctrlEditor.Text;
				
			//	Close the dialog
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		
		/// <summary>This method is called when the form is loaded the first time</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnLoad(object sender, System.EventArgs e)
		{
			if(m_ctrlEditor != null)
				m_ctrlEditor.Text = Memo;
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CTmaxMemoEditor));
			this.m_ctrlEditor = new System.Windows.Forms.RichTextBox();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// m_ctrlEditor
			// 
			this.m_ctrlEditor.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlEditor.Name = "m_ctrlEditor";
			this.m_ctrlEditor.Size = new System.Drawing.Size(404, 168);
			this.m_ctrlEditor.TabIndex = 0;
			this.m_ctrlEditor.Text = "";
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(336, 188);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 7;
			this.m_ctrlCancel.Text = "  &Cancel";
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(252, 188);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 6;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOK);
			// 
			// CTmaxMemoEditor
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(420, 221);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.m_ctrlCancel,
																		  this.m_ctrlOk,
																		  this.m_ctrlEditor});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "CTmaxMemoEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Memo Editor";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

		}
		
		#endregion Protected Methods

		#region Properties
		
		/// <summary>Memo text</summary>
		public string Memo
		{
			get 
			{
				return m_strMemo;
			}
			set
			{
				m_strMemo = value;
			}
			
		}// Memo property
			
		#endregion Properties

	}// public class CTmaxMemoEditor : System.Windows.Forms.Form
	
}// namespace FTI.Trialmax.Controls
