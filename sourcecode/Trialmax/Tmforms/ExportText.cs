using System;
using System.Drawing;
using System.Drawing.Text;
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
	public class CFExportText : CFTmaxBaseForm
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_EXCHANGE_EX = ERROR_TMAX_FORM_MAX + 1;
		
		#endregion Constants

		#region Private Members
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>The form's cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>The form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		
		/// <summary>Local member bound to ExportOptions property</summary>
		private CTmaxExportOptions m_tmaxExportOptions = null;
		
		/// <summary>Group box for general preferences</summary>
		private System.Windows.Forms.GroupBox m_ctrlGeneralGroup;
		
		/// <summary>Check box to set AutoFilenames option</summary>
		private System.Windows.Forms.CheckBox m_ctrlAutoFilenames;
		
		/// <summary>Check box to set ConfirmOverwrite option</summary>
		private System.Windows.Forms.CheckBox m_ctrlConfirmOverwrite;
		
		#endregion Private Members
		
		#region Public Methods
		
		public CFExportText()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
		}// public CFExportText()

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to exchange the form's properties: SetMembers = %1");
		
		}// protected override void SetErrorStrings()

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
			//	Initialize all the child controls
			m_ctrlOk.Enabled = Exchange(false);
			
			base.OnLoad(e);
			
		}// protected override void OnLoad(EventArgs e)
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>This method is called to manage the exchange of data between the class members and child controls</summary>
		/// <param name="bSetMembers">true to set the class members</param>
		/// <returns>true if successful</returns>
		private bool Exchange(bool bSetMembers)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Are we setting the class members?
				if(bSetMembers == true)
				{
					m_tmaxExportOptions.AutoFilenames = m_ctrlAutoFilenames.Checked;
					m_tmaxExportOptions.ConfirmOverwrite = m_ctrlConfirmOverwrite.Checked;
				
				}
				else
				{
					m_ctrlAutoFilenames.Checked = m_tmaxExportOptions.AutoFilenames;
					m_ctrlConfirmOverwrite.Checked = m_tmaxExportOptions.ConfirmOverwrite;
					
				}// if(bSetMembers == true)
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Exchange", m_tmaxErrorBuilder.Message(ERROR_EXCHANGE_EX, bSetMembers), Ex);
			}
			
			return bSuccessful; 
			
		}// private bool Exchange(bool bSetMembers)
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFExportText));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlGeneralGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlAutoFilenames = new System.Windows.Forms.CheckBox();
			this.m_ctrlConfirmOverwrite = new System.Windows.Forms.CheckBox();
			this.m_ctrlGeneralGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(184, 112);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 1;
			this.m_ctrlCancel.Text = "  &Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(96, 112);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 0;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlGeneralGroup
			// 
			this.m_ctrlGeneralGroup.Controls.Add(this.m_ctrlAutoFilenames);
			this.m_ctrlGeneralGroup.Controls.Add(this.m_ctrlConfirmOverwrite);
			this.m_ctrlGeneralGroup.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlGeneralGroup.Name = "m_ctrlGeneralGroup";
			this.m_ctrlGeneralGroup.Size = new System.Drawing.Size(256, 92);
			this.m_ctrlGeneralGroup.TabIndex = 1;
			this.m_ctrlGeneralGroup.TabStop = false;
			this.m_ctrlGeneralGroup.Text = "Preferences";
			// 
			// m_ctrlAutoFilenames
			// 
			this.m_ctrlAutoFilenames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlAutoFilenames.Location = new System.Drawing.Point(12, 24);
			this.m_ctrlAutoFilenames.Name = "m_ctrlAutoFilenames";
			this.m_ctrlAutoFilenames.Size = new System.Drawing.Size(232, 24);
			this.m_ctrlAutoFilenames.TabIndex = 0;
			this.m_ctrlAutoFilenames.Text = "Use default filenames if multiple outputs";
			// 
			// m_ctrlConfirmOverwrite
			// 
			this.m_ctrlConfirmOverwrite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlConfirmOverwrite.Location = new System.Drawing.Point(12, 52);
			this.m_ctrlConfirmOverwrite.Name = "m_ctrlConfirmOverwrite";
			this.m_ctrlConfirmOverwrite.Size = new System.Drawing.Size(228, 24);
			this.m_ctrlConfirmOverwrite.TabIndex = 1;
			this.m_ctrlConfirmOverwrite.Text = "Confirm before overwrite existing ouput";
			// 
			// CFExportText
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(272, 149);
			this.Controls.Add(this.m_ctrlGeneralGroup);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFExportText";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Export Text Options";
			this.m_ctrlGeneralGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOk(object sender, System.EventArgs e)
		{
			//	Get the user settings
			Exchange(true);
			
			//	Close the form
			DialogResult = DialogResult.OK;
			this.Close();
		
		}// private void OnClickOk(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on Cancel</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickCancel(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		
		}// private void OnClickCancel(object sender, System.EventArgs e)

		#endregion Private Methods

		#region Properties
		
		/// <summary>The user defined export options</summary>
		public CTmaxExportOptions ExportOptions
		{
			get { return m_tmaxExportOptions;  }
			set { m_tmaxExportOptions = value; }
		}
		
		#endregion Properties
	
	}// public class CFExportText : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
