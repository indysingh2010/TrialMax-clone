using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace FTI.Trialmax.Controls
{
	/// <summary>Form used to prompt user when changes have been made to one or more video properties</summary>
	public class CTmaxVideoPrompt : System.Windows.Forms.Form
	{
		#region Private Members
		
		/// <summary>Yes button</summary>
		private System.Windows.Forms.Button m_ctrlYes;
		
		/// <summary>No button</summary>
		private System.Windows.Forms.Button m_ctrlNo;
		
		/// <summary>Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>User prompt</summary>
		private System.Windows.Forms.Label m_ctrlPrompt;
		
		/// <summary>User question</summary>
		private System.Windows.Forms.Label m_ctrlQuestion;
		
		/// <summary>Request automatic confirmation</summary>
		private System.Windows.Forms.CheckBox m_ctrlAutomatic;
		
		/// <summary>Local member bound to Automatic property</summary>
		private bool m_bAutomatic = false;
		
		/// <summary>Local member bound to Modifications property</summary>
		private ArrayList m_aModifications = null;
		private System.Windows.Forms.ListBox m_ctrlModifications;
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;

		#endregion Private Members
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxVideoPrompt()
		{
			//	Initialize the child controls
			InitializeComponent();
		}

		#endregion Public Members
		
		#region Protected Members
		
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
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CTmaxVideoPrompt));
			this.m_ctrlYes = new System.Windows.Forms.Button();
			this.m_ctrlNo = new System.Windows.Forms.Button();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlPrompt = new System.Windows.Forms.Label();
			this.m_ctrlQuestion = new System.Windows.Forms.Label();
			this.m_ctrlAutomatic = new System.Windows.Forms.CheckBox();
			this.m_ctrlModifications = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// m_ctrlYes
			// 
			this.m_ctrlYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.m_ctrlYes.Location = new System.Drawing.Point(20, 164);
			this.m_ctrlYes.Name = "m_ctrlYes";
			this.m_ctrlYes.TabIndex = 0;
			this.m_ctrlYes.Text = "&Yes";
			this.m_ctrlYes.Click += new System.EventHandler(this.OnYes);
			// 
			// m_ctrlNo
			// 
			this.m_ctrlNo.DialogResult = System.Windows.Forms.DialogResult.No;
			this.m_ctrlNo.Location = new System.Drawing.Point(120, 164);
			this.m_ctrlNo.Name = "m_ctrlNo";
			this.m_ctrlNo.TabIndex = 1;
			this.m_ctrlNo.Text = "&No";
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.Location = new System.Drawing.Point(220, 164);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 2;
			this.m_ctrlCancel.Text = "&Cancel";
			// 
			// m_ctrlPrompt
			// 
			this.m_ctrlPrompt.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlPrompt.Name = "m_ctrlPrompt";
			this.m_ctrlPrompt.Size = new System.Drawing.Size(272, 16);
			this.m_ctrlPrompt.TabIndex = 3;
			this.m_ctrlPrompt.Text = "Unsaved changes have been detected:";
			this.m_ctrlPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlQuestion
			// 
			this.m_ctrlQuestion.Location = new System.Drawing.Point(8, 140);
			this.m_ctrlQuestion.Name = "m_ctrlQuestion";
			this.m_ctrlQuestion.Size = new System.Drawing.Size(300, 16);
			this.m_ctrlQuestion.TabIndex = 4;
			this.m_ctrlQuestion.Text = "Do you want to save these changes?";
			this.m_ctrlQuestion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// m_ctrlAutomatic
			// 
			this.m_ctrlAutomatic.Location = new System.Drawing.Point(8, 108);
			this.m_ctrlAutomatic.Name = "m_ctrlAutomatic";
			this.m_ctrlAutomatic.Size = new System.Drawing.Size(264, 24);
			this.m_ctrlAutomatic.TabIndex = 5;
			this.m_ctrlAutomatic.Text = "Automatically save future changes";
			// 
			// m_ctrlModifications
			// 
			this.m_ctrlModifications.HorizontalScrollbar = true;
			this.m_ctrlModifications.IntegralHeight = false;
			this.m_ctrlModifications.Location = new System.Drawing.Point(8, 28);
			this.m_ctrlModifications.Name = "m_ctrlModifications";
			this.m_ctrlModifications.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.m_ctrlModifications.Size = new System.Drawing.Size(300, 72);
			this.m_ctrlModifications.TabIndex = 6;
			this.m_ctrlModifications.TabStop = false;
			// 
			// CTmaxVideoPrompt
			// 
			this.AcceptButton = this.m_ctrlYes;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(314, 191);
			this.Controls.Add(this.m_ctrlModifications);
			this.Controls.Add(this.m_ctrlAutomatic);
			this.Controls.Add(this.m_ctrlQuestion);
			this.Controls.Add(this.m_ctrlPrompt);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlNo);
			this.Controls.Add(this.m_ctrlYes);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CTmaxVideoPrompt";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = " Apply Changes";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

		}

		/// <summary>Traps events fired when the user clicks on Yes</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		protected void OnYes(object sender, System.EventArgs e)
		{
			m_bAutomatic = m_ctrlAutomatic.Checked;
			DialogResult = DialogResult.Yes;
			Close();
		}
	
		/// <summary>Traps events fired when the user clicks on No</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		protected void OnNo(object sender, System.EventArgs e)
		{
			m_bAutomatic = m_ctrlAutomatic.Checked;
			DialogResult = DialogResult.No;
			Close();
		}
	
		/// <summary>Traps events fired when the user clicks on Cancel</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		protected void OnCancel(object sender, System.EventArgs e)
		{
			m_bAutomatic = m_ctrlAutomatic.Checked;
			DialogResult = DialogResult.Cancel;
			Close();
		}

		/// <summary>Traps the form's Load event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnLoad(object sender, System.EventArgs e)
		{
			FTI.Shared.Win32.User.MessageBeep(FTI.Shared.Win32.User.MB_ICONEXCLAMATION);
			
			if((Modifications != null) && (Modifications.Count > 0))
			{
				m_ctrlModifications.DataSource = Modifications;
			}
			
		}// private void OnLoad(object sender, System.EventArgs e)
	
		#endregion Protected Members
		
		#region Properties
		
		/// <summary>Text descriptions for all modifications</summary>
		public ArrayList Modifications
		{
			get{ return m_aModifications; }
			set{ m_aModifications = value; }
		}
		
		/// <summary>True to request Automatic updates</summary>
		public bool ApplyAutomatic
		{
			get{ return m_bAutomatic; }
		}
		
		#endregion Properties
		
	}// public class CTmaxVideoPrompt : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Controls

