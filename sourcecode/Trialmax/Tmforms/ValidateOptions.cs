using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Controls;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form allows the user to set the options for validating the database contents</summary>
	public class CFValidateOptions : System.Windows.Forms.Form
	{
		#region Private Members
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>Form's Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>Form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		private System.Windows.Forms.CheckBox m_ctrlDocumentFiles;
		private System.Windows.Forms.CheckBox m_ctrlPowerPointFiles;
		private System.Windows.Forms.CheckBox m_ctrlRecordingFiles;
		private System.Windows.Forms.CheckBox m_ctrlTranscriptFiles;
		private System.Windows.Forms.CheckBox m_ctrlVideoFiles;
		private System.Windows.Forms.GroupBox m_ctrlFilesGroup;
		private System.Windows.Forms.CheckBox m_ctrlTransferCodes;
		private System.Windows.Forms.CheckBox m_ctrlBarcodeMap;
		private System.Windows.Forms.CheckBox m_ctrlBinders;
		private System.Windows.Forms.CheckBox m_ctrlScripts;
		private System.Windows.Forms.GroupBox m_ctrlReferencesGroup;

		/// <summary>Local member bound to ValidateOptions property</summary>
		private FTI.Shared.Trialmax.CTmaxValidateOptions m_tmaxValidateOptions;
		private System.Windows.Forms.CheckBox m_ctrlCreateDesignations;
		
		/// <summary>Local member bound to ShowTransferCodes property</summary>
		private bool m_bShowTransferCodes = false;
		
		#endregion Private Members
		
		#region Public Methods
		
		public CFValidateOptions()
		{
			InitializeComponent();
		}

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFValidateOptions));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlFilesGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlTranscriptFiles = new System.Windows.Forms.CheckBox();
			this.m_ctrlPowerPointFiles = new System.Windows.Forms.CheckBox();
			this.m_ctrlRecordingFiles = new System.Windows.Forms.CheckBox();
			this.m_ctrlVideoFiles = new System.Windows.Forms.CheckBox();
			this.m_ctrlDocumentFiles = new System.Windows.Forms.CheckBox();
			this.m_ctrlTransferCodes = new System.Windows.Forms.CheckBox();
			this.m_ctrlBarcodeMap = new System.Windows.Forms.CheckBox();
			this.m_ctrlBinders = new System.Windows.Forms.CheckBox();
			this.m_ctrlScripts = new System.Windows.Forms.CheckBox();
			this.m_ctrlReferencesGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlCreateDesignations = new System.Windows.Forms.CheckBox();
			this.m_ctrlFilesGroup.SuspendLayout();
			this.m_ctrlReferencesGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(304, 184);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 2;
			this.m_ctrlCancel.Text = "  &Cancel";
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(212, 184);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 1;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOK);
			// 
			// m_ctrlFilesGroup
			// 
			this.m_ctrlFilesGroup.Controls.Add(this.m_ctrlTranscriptFiles);
			this.m_ctrlFilesGroup.Controls.Add(this.m_ctrlPowerPointFiles);
			this.m_ctrlFilesGroup.Controls.Add(this.m_ctrlRecordingFiles);
			this.m_ctrlFilesGroup.Controls.Add(this.m_ctrlVideoFiles);
			this.m_ctrlFilesGroup.Controls.Add(this.m_ctrlDocumentFiles);
			this.m_ctrlFilesGroup.Location = new System.Drawing.Point(8, 12);
			this.m_ctrlFilesGroup.Name = "m_ctrlFilesGroup";
			this.m_ctrlFilesGroup.Size = new System.Drawing.Size(188, 168);
			this.m_ctrlFilesGroup.TabIndex = 0;
			this.m_ctrlFilesGroup.TabStop = false;
			this.m_ctrlFilesGroup.Text = "Media File Locations";
			// 
			// m_ctrlTranscriptFiles
			// 
			this.m_ctrlTranscriptFiles.Location = new System.Drawing.Point(12, 108);
			this.m_ctrlTranscriptFiles.Name = "m_ctrlTranscriptFiles";
			this.m_ctrlTranscriptFiles.Size = new System.Drawing.Size(148, 24);
			this.m_ctrlTranscriptFiles.TabIndex = 3;
			this.m_ctrlTranscriptFiles.Text = "Deposition Transcripts";
			// 
			// m_ctrlPowerPointFiles
			// 
			this.m_ctrlPowerPointFiles.Location = new System.Drawing.Point(12, 52);
			this.m_ctrlPowerPointFiles.Name = "m_ctrlPowerPointFiles";
			this.m_ctrlPowerPointFiles.Size = new System.Drawing.Size(148, 24);
			this.m_ctrlPowerPointFiles.TabIndex = 1;
			this.m_ctrlPowerPointFiles.Text = "PowerPoints";
			// 
			// m_ctrlRecordingFiles
			// 
			this.m_ctrlRecordingFiles.Location = new System.Drawing.Point(12, 80);
			this.m_ctrlRecordingFiles.Name = "m_ctrlRecordingFiles";
			this.m_ctrlRecordingFiles.Size = new System.Drawing.Size(148, 24);
			this.m_ctrlRecordingFiles.TabIndex = 2;
			this.m_ctrlRecordingFiles.Text = "Recordings";
			// 
			// m_ctrlVideoFiles
			// 
			this.m_ctrlVideoFiles.Location = new System.Drawing.Point(12, 136);
			this.m_ctrlVideoFiles.Name = "m_ctrlVideoFiles";
			this.m_ctrlVideoFiles.Size = new System.Drawing.Size(148, 24);
			this.m_ctrlVideoFiles.TabIndex = 4;
			this.m_ctrlVideoFiles.Text = "Deposition Video";
			// 
			// m_ctrlDocumentFiles
			// 
			this.m_ctrlDocumentFiles.Location = new System.Drawing.Point(12, 24);
			this.m_ctrlDocumentFiles.Name = "m_ctrlDocumentFiles";
			this.m_ctrlDocumentFiles.Size = new System.Drawing.Size(148, 24);
			this.m_ctrlDocumentFiles.TabIndex = 0;
			this.m_ctrlDocumentFiles.Text = "Documents";
			// 
			// m_ctrlTransferCodes
			// 
			this.m_ctrlTransferCodes.Location = new System.Drawing.Point(12, 184);
			this.m_ctrlTransferCodes.Name = "m_ctrlTransferCodes";
			this.m_ctrlTransferCodes.Size = new System.Drawing.Size(180, 28);
			this.m_ctrlTransferCodes.TabIndex = 0;
			this.m_ctrlTransferCodes.Text = "Update Fielded Data From ver 7.0.0 Table";
			// 
			// m_ctrlBarcodeMap
			// 
			this.m_ctrlBarcodeMap.Location = new System.Drawing.Point(12, 80);
			this.m_ctrlBarcodeMap.Name = "m_ctrlBarcodeMap";
			this.m_ctrlBarcodeMap.Size = new System.Drawing.Size(168, 24);
			this.m_ctrlBarcodeMap.TabIndex = 2;
			this.m_ctrlBarcodeMap.Text = "Barcode Map";
			// 
			// m_ctrlBinders
			// 
			this.m_ctrlBinders.Location = new System.Drawing.Point(12, 52);
			this.m_ctrlBinders.Name = "m_ctrlBinders";
			this.m_ctrlBinders.Size = new System.Drawing.Size(168, 24);
			this.m_ctrlBinders.TabIndex = 1;
			this.m_ctrlBinders.Text = "Binders";
			// 
			// m_ctrlScripts
			// 
			this.m_ctrlScripts.Location = new System.Drawing.Point(12, 24);
			this.m_ctrlScripts.Name = "m_ctrlScripts";
			this.m_ctrlScripts.Size = new System.Drawing.Size(168, 24);
			this.m_ctrlScripts.TabIndex = 0;
			this.m_ctrlScripts.Text = "Scripts";
			// 
			// m_ctrlReferencesGroup
			// 
			this.m_ctrlReferencesGroup.Controls.Add(this.m_ctrlCreateDesignations);
			this.m_ctrlReferencesGroup.Controls.Add(this.m_ctrlScripts);
			this.m_ctrlReferencesGroup.Controls.Add(this.m_ctrlBinders);
			this.m_ctrlReferencesGroup.Controls.Add(this.m_ctrlBarcodeMap);
			this.m_ctrlReferencesGroup.Location = new System.Drawing.Point(204, 12);
			this.m_ctrlReferencesGroup.Name = "m_ctrlReferencesGroup";
			this.m_ctrlReferencesGroup.Size = new System.Drawing.Size(188, 168);
			this.m_ctrlReferencesGroup.TabIndex = 1;
			this.m_ctrlReferencesGroup.TabStop = false;
			this.m_ctrlReferencesGroup.Text = "Media References";
			// 
			// m_ctrlCreateDesignations
			// 
			this.m_ctrlCreateDesignations.Location = new System.Drawing.Point(12, 108);
			this.m_ctrlCreateDesignations.Name = "m_ctrlCreateDesignations";
			this.m_ctrlCreateDesignations.Size = new System.Drawing.Size(168, 24);
			this.m_ctrlCreateDesignations.TabIndex = 3;
			this.m_ctrlCreateDesignations.Text = "Create Missing Designations";
			// 
			// CFValidateOptions
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(398, 215);
			this.Controls.Add(this.m_ctrlTransferCodes);
			this.Controls.Add(this.m_ctrlFilesGroup);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.Controls.Add(this.m_ctrlReferencesGroup);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFValidateOptions";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Validate Options";
			this.Load += new System.EventHandler(this.OnLoad);
			this.m_ctrlFilesGroup.ResumeLayout(false);
			this.m_ctrlReferencesGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		
		/// <summary>This method handles the form's Load event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnLoad(object sender, System.EventArgs e)
		{
			if(m_tmaxValidateOptions != null)
			{
				m_ctrlDocumentFiles.Checked = m_tmaxValidateOptions.Documents;
				m_ctrlPowerPointFiles.Checked = m_tmaxValidateOptions.PowerPoints;
				m_ctrlRecordingFiles.Checked = m_tmaxValidateOptions.Recordings;
				m_ctrlTranscriptFiles.Checked = m_tmaxValidateOptions.Transcripts;
				m_ctrlVideoFiles.Checked = m_tmaxValidateOptions.VideoFiles;
				
				m_ctrlScripts.Checked = m_tmaxValidateOptions.Scripts;
				m_ctrlBarcodeMap.Checked = m_tmaxValidateOptions.BarcodeMap;
				m_ctrlBinders.Checked = m_tmaxValidateOptions.Binders;
				m_ctrlCreateDesignations.Checked = m_tmaxValidateOptions.CreateDesignations;
				
				if(m_bShowTransferCodes == true)
				{
					m_ctrlTransferCodes.Checked = m_tmaxValidateOptions.TransferCodes;
				}
				else
				{			
					m_ctrlTransferCodes.Checked = false;
					m_ctrlTransferCodes.Visible = false;
				}
			
			}
			else
			{
				m_ctrlDocumentFiles.Enabled = false;
				m_ctrlPowerPointFiles.Enabled = false;
				m_ctrlRecordingFiles.Enabled = false;
				m_ctrlTranscriptFiles.Enabled = false;
				m_ctrlVideoFiles.Enabled = false;
				m_ctrlScripts.Enabled = false;
				m_ctrlBarcodeMap.Enabled = false;
				m_ctrlBinders.Enabled = false;
				m_ctrlCreateDesignations.Enabled = false;
				m_ctrlTransferCodes.Enabled = false;
				m_ctrlTransferCodes.Visible = false;
			}
		
		}// private void OnLoad(object sender, System.EventArgs e)

		/// <summary>This method will use the selections in the list box to set the Selected property of all registration options</summary>

		/// <summary>This method handles the OK button's Click event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOK(object sender, System.EventArgs e)
		{
			m_tmaxValidateOptions.Documents   = m_ctrlDocumentFiles.Checked;
			m_tmaxValidateOptions.PowerPoints = m_ctrlPowerPointFiles.Checked;
			m_tmaxValidateOptions.Recordings  = m_ctrlRecordingFiles.Checked;
			m_tmaxValidateOptions.Transcripts = m_ctrlTranscriptFiles.Checked;
			m_tmaxValidateOptions.VideoFiles  = m_ctrlVideoFiles.Checked;
				
			m_tmaxValidateOptions.Scripts   = m_ctrlScripts.Checked;
			m_tmaxValidateOptions.BarcodeMap = m_ctrlBarcodeMap.Checked;
			m_tmaxValidateOptions.Binders  = m_ctrlBinders.Checked;
			m_tmaxValidateOptions.CreateDesignations  = m_ctrlCreateDesignations.Checked;

			m_tmaxValidateOptions.TransferCodes = m_ctrlTransferCodes.Checked;
				
			//	Close the dialog
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Database validation options</summary>
		public CTmaxValidateOptions ValidateOptions
		{
			get { return m_tmaxValidateOptions; }
			set { m_tmaxValidateOptions = value; }
		}
		
		/// <summary>True to show the Transfer Codes options</summary>
		public bool ShowTransferCodes
		{
			get { return m_bShowTransferCodes; }
			set { m_bShowTransferCodes = value; }
		}
		
		#endregion Properties

	}// public class CFValidateOptions : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Forms
