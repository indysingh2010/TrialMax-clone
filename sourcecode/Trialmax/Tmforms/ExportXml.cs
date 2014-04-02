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
	public class CFExportXml : CFTmaxBaseForm
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
		
		/// <summary>Group box for file naming preferences</summary>
		private System.Windows.Forms.GroupBox m_ctrlFilesGroup;
		
		/// <summary>Check box to set AutoFilenames option</summary>
		private System.Windows.Forms.CheckBox m_ctrlAutoFilenames;
		
		/// <summary>Group box for file format options</summary>
		private System.Windows.Forms.GroupBox m_ctrlFormatGroup;
		
		/// <summary>Radio button to select video viewer format</summary>
		private System.Windows.Forms.RadioButton m_ctrlFormatVideoViewer;
		
		/// <summary>Radio button to select case manager format</summary>
		private System.Windows.Forms.RadioButton m_ctrlFormatManager;
		
		/// <summary>Check box to request inclusion of source transcripts</summary>
		private System.Windows.Forms.CheckBox m_ctrlIncludeDepositions;

		/// <summary>Group box for file content options</summary>
		private GroupBox m_ctrlContentGroup;

		/// <summary>Check box to request inclusion of objections</summary>
		private CheckBox m_ctrlIncludeObjections;
		
		/// <summary>Check box to set ConfirmOverwrite option</summary>
		private System.Windows.Forms.CheckBox m_ctrlConfirmOverwrite;
		
		#endregion Private Members
		
		#region Public Methods
		
		public CFExportXml()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
		}// public CFExportXml()

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
					m_tmaxExportOptions.IncludeDepositions = m_ctrlIncludeDepositions.Checked;
					m_tmaxExportOptions.IncludeObjections = m_ctrlIncludeObjections.Checked;
					
					if(m_ctrlFormatVideoViewer.Checked == true)
						m_tmaxExportOptions.XmlScriptFormat = TmaxXmlScriptFormats.VideoViewer;
					else
						m_tmaxExportOptions.XmlScriptFormat = TmaxXmlScriptFormats.Manager;
				
				}
				else
				{
					m_ctrlAutoFilenames.Checked = m_tmaxExportOptions.AutoFilenames;
					m_ctrlConfirmOverwrite.Checked = m_tmaxExportOptions.ConfirmOverwrite;
					m_ctrlIncludeDepositions.Checked = m_tmaxExportOptions.IncludeDepositions;
					m_ctrlIncludeObjections.Checked = m_tmaxExportOptions.IncludeObjections;
				
					m_ctrlFormatVideoViewer.Checked = (m_tmaxExportOptions.XmlScriptFormat == TmaxXmlScriptFormats.VideoViewer);
					m_ctrlFormatManager.Checked = (m_tmaxExportOptions.XmlScriptFormat != TmaxXmlScriptFormats.VideoViewer);
				
					OnClickFormat(m_ctrlFormatManager, System.EventArgs.Empty);
					
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CFExportXml));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlFilesGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlAutoFilenames = new System.Windows.Forms.CheckBox();
			this.m_ctrlConfirmOverwrite = new System.Windows.Forms.CheckBox();
			this.m_ctrlIncludeDepositions = new System.Windows.Forms.CheckBox();
			this.m_ctrlFormatGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlFormatVideoViewer = new System.Windows.Forms.RadioButton();
			this.m_ctrlFormatManager = new System.Windows.Forms.RadioButton();
			this.m_ctrlContentGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlIncludeObjections = new System.Windows.Forms.CheckBox();
			this.m_ctrlFilesGroup.SuspendLayout();
			this.m_ctrlFormatGroup.SuspendLayout();
			this.m_ctrlContentGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(189, 257);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlCancel.TabIndex = 3;
			this.m_ctrlCancel.Text = "  &Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(101, 257);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlOk.TabIndex = 2;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlFilesGroup
			// 
			this.m_ctrlFilesGroup.Controls.Add(this.m_ctrlAutoFilenames);
			this.m_ctrlFilesGroup.Controls.Add(this.m_ctrlConfirmOverwrite);
			this.m_ctrlFilesGroup.Location = new System.Drawing.Point(8, 174);
			this.m_ctrlFilesGroup.Name = "m_ctrlFilesGroup";
			this.m_ctrlFilesGroup.Size = new System.Drawing.Size(256, 74);
			this.m_ctrlFilesGroup.TabIndex = 2;
			this.m_ctrlFilesGroup.TabStop = false;
			this.m_ctrlFilesGroup.Text = "Files";
			// 
			// m_ctrlAutoFilenames
			// 
			this.m_ctrlAutoFilenames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlAutoFilenames.Location = new System.Drawing.Point(16, 19);
			this.m_ctrlAutoFilenames.Name = "m_ctrlAutoFilenames";
			this.m_ctrlAutoFilenames.Size = new System.Drawing.Size(232, 24);
			this.m_ctrlAutoFilenames.TabIndex = 0;
			this.m_ctrlAutoFilenames.Text = "Use default filenames if multiple outputs";
			// 
			// m_ctrlConfirmOverwrite
			// 
			this.m_ctrlConfirmOverwrite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlConfirmOverwrite.Location = new System.Drawing.Point(16, 40);
			this.m_ctrlConfirmOverwrite.Name = "m_ctrlConfirmOverwrite";
			this.m_ctrlConfirmOverwrite.Size = new System.Drawing.Size(228, 24);
			this.m_ctrlConfirmOverwrite.TabIndex = 1;
			this.m_ctrlConfirmOverwrite.Text = "Confirm before overwrite existing ouput";
			// 
			// m_ctrlIncludeDepositions
			// 
			this.m_ctrlIncludeDepositions.Location = new System.Drawing.Point(16, 19);
			this.m_ctrlIncludeDepositions.Name = "m_ctrlIncludeDepositions";
			this.m_ctrlIncludeDepositions.Size = new System.Drawing.Size(232, 24);
			this.m_ctrlIncludeDepositions.TabIndex = 0;
			this.m_ctrlIncludeDepositions.Text = "Include Source Transcripts";
			// 
			// m_ctrlFormatGroup
			// 
			this.m_ctrlFormatGroup.Controls.Add(this.m_ctrlFormatVideoViewer);
			this.m_ctrlFormatGroup.Controls.Add(this.m_ctrlFormatManager);
			this.m_ctrlFormatGroup.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlFormatGroup.Name = "m_ctrlFormatGroup";
			this.m_ctrlFormatGroup.Size = new System.Drawing.Size(256, 80);
			this.m_ctrlFormatGroup.TabIndex = 0;
			this.m_ctrlFormatGroup.TabStop = false;
			this.m_ctrlFormatGroup.Text = "File Format";
			// 
			// m_ctrlFormatVideoViewer
			// 
			this.m_ctrlFormatVideoViewer.Location = new System.Drawing.Point(16, 48);
			this.m_ctrlFormatVideoViewer.Name = "m_ctrlFormatVideoViewer";
			this.m_ctrlFormatVideoViewer.Size = new System.Drawing.Size(224, 24);
			this.m_ctrlFormatVideoViewer.TabIndex = 1;
			this.m_ctrlFormatVideoViewer.Text = "Video Viewer";
			this.m_ctrlFormatVideoViewer.Click += new System.EventHandler(this.OnClickFormat);
			// 
			// m_ctrlFormatManager
			// 
			this.m_ctrlFormatManager.Location = new System.Drawing.Point(16, 24);
			this.m_ctrlFormatManager.Name = "m_ctrlFormatManager";
			this.m_ctrlFormatManager.Size = new System.Drawing.Size(224, 24);
			this.m_ctrlFormatManager.TabIndex = 0;
			this.m_ctrlFormatManager.Text = "Case Manager";
			this.m_ctrlFormatManager.Click += new System.EventHandler(this.OnClickFormat);
			// 
			// m_ctrlContentGroup
			// 
			this.m_ctrlContentGroup.Controls.Add(this.m_ctrlIncludeObjections);
			this.m_ctrlContentGroup.Controls.Add(this.m_ctrlIncludeDepositions);
			this.m_ctrlContentGroup.Location = new System.Drawing.Point(8, 94);
			this.m_ctrlContentGroup.Name = "m_ctrlContentGroup";
			this.m_ctrlContentGroup.Size = new System.Drawing.Size(256, 74);
			this.m_ctrlContentGroup.TabIndex = 1;
			this.m_ctrlContentGroup.TabStop = false;
			this.m_ctrlContentGroup.Text = "Content";
			// 
			// m_ctrlIncludeObjections
			// 
			this.m_ctrlIncludeObjections.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlIncludeObjections.Location = new System.Drawing.Point(16, 40);
			this.m_ctrlIncludeObjections.Name = "m_ctrlIncludeObjections";
			this.m_ctrlIncludeObjections.Size = new System.Drawing.Size(228, 24);
			this.m_ctrlIncludeObjections.TabIndex = 1;
			this.m_ctrlIncludeObjections.Text = "Include Objections";
			// 
			// CFExportXml
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(272, 290);
			this.Controls.Add(this.m_ctrlContentGroup);
			this.Controls.Add(this.m_ctrlFormatGroup);
			this.Controls.Add(this.m_ctrlFilesGroup);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFExportXml";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Export XML Options";
			this.m_ctrlFilesGroup.ResumeLayout(false);
			this.m_ctrlFormatGroup.ResumeLayout(false);
			this.m_ctrlContentGroup.ResumeLayout(false);
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

		/// <summary>This method is called when the user clicks on one of the export format radio buttons</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickFormat(object sender, System.EventArgs e)
		{
			m_ctrlIncludeDepositions.Enabled = (m_ctrlFormatVideoViewer.Checked == false);
		
		}// private void OnClickFormat(object sender, System.EventArgs e)

		#endregion Private Methods

		#region Properties
		
		/// <summary>The user defined export options</summary>
		public CTmaxExportOptions ExportOptions
		{
			get { return m_tmaxExportOptions;  }
			set { m_tmaxExportOptions = value; }
		}
		
		#endregion Properties

	
	}// public class CFExportXml : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
