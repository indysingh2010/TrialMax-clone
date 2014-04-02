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
	public class CRFExportSummary : FTI.Trialmax.Forms.CFTmaxBaseForm
	{
		#region Private Members
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>The form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOK;
		
		/// <summary>Static text label for total exported text</summary>
		private System.Windows.Forms.Label m_ctrlTotalLabel;
		
		/// <summary>Static text to display total number exported</summary>
		private System.Windows.Forms.Label m_ctrlTotal;
		
		/// <summary>Static text to display the folder path</summary>
		private System.Windows.Forms.Label m_ctrlFolder;
		
		/// <summary>List box to display the list of exported files</summary>
		private System.Windows.Forms.ListBox m_ctrlFiles;
		
		/// <summary>Static text label for Files list box</summary>
		private System.Windows.Forms.Label m_ctrlFilesLabel;
		
		/// <summary>Static text label for target folder path</summary>
		private System.Windows.Forms.Label m_ctrlTargetLabel;
		
		/// <summary>Local member bound to Exported property</summary>
		private CTmaxSourceFolder m_tmaxExported = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CRFExportSummary()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			m_tmaxEventSource.Name = "Exported Summary";
			
		}// public CRFExportSummary()

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
			
			//	Do we have a folder of results?
			if(m_tmaxExported != null)
			{
				SetExported(m_tmaxExported);
			}
				
			FTI.Shared.Win32.User.MessageBeep(FTI.Shared.Win32.User.MB_ICONINFORMATION);
		
		}// protected override void OnLoad(EventArgs e)
		
		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CRFExportSummary));
			this.m_ctrlOK = new System.Windows.Forms.Button();
			this.m_ctrlTotalLabel = new System.Windows.Forms.Label();
			this.m_ctrlTargetLabel = new System.Windows.Forms.Label();
			this.m_ctrlTotal = new System.Windows.Forms.Label();
			this.m_ctrlFolder = new System.Windows.Forms.Label();
			this.m_ctrlFiles = new System.Windows.Forms.ListBox();
			this.m_ctrlFilesLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// m_ctrlOK
			// 
			this.m_ctrlOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.m_ctrlOK.Location = new System.Drawing.Point(272, 160);
			this.m_ctrlOK.Name = "m_ctrlOK";
			this.m_ctrlOK.TabIndex = 7;
			this.m_ctrlOK.Text = "&OK";
			// 
			// m_ctrlTotalLabel
			// 
			this.m_ctrlTotalLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlTotalLabel.Location = new System.Drawing.Point(8, 16);
			this.m_ctrlTotalLabel.Name = "m_ctrlTotalLabel";
			this.m_ctrlTotalLabel.Size = new System.Drawing.Size(100, 16);
			this.m_ctrlTotalLabel.TabIndex = 9;
			this.m_ctrlTotalLabel.Text = "Total Exported :";
			// 
			// m_ctrlTargetLabel
			// 
			this.m_ctrlTargetLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlTargetLabel.Location = new System.Drawing.Point(8, 40);
			this.m_ctrlTargetLabel.Name = "m_ctrlTargetLabel";
			this.m_ctrlTargetLabel.Size = new System.Drawing.Size(100, 16);
			this.m_ctrlTargetLabel.TabIndex = 10;
			this.m_ctrlTargetLabel.Text = "Target Folder :";
			// 
			// m_ctrlTotal
			// 
			this.m_ctrlTotal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlTotal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlTotal.Location = new System.Drawing.Point(112, 16);
			this.m_ctrlTotal.Name = "m_ctrlTotal";
			this.m_ctrlTotal.Size = new System.Drawing.Size(240, 16);
			this.m_ctrlTotal.TabIndex = 11;
			this.m_ctrlTotal.Text = "0";
			// 
			// m_ctrlFolder
			// 
			this.m_ctrlFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlFolder.Location = new System.Drawing.Point(112, 40);
			this.m_ctrlFolder.Name = "m_ctrlFolder";
			this.m_ctrlFolder.Size = new System.Drawing.Size(248, 16);
			this.m_ctrlFolder.TabIndex = 12;
			// 
			// m_ctrlFiles
			// 
			this.m_ctrlFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlFiles.Location = new System.Drawing.Point(8, 80);
			this.m_ctrlFiles.Name = "m_ctrlFiles";
			this.m_ctrlFiles.Size = new System.Drawing.Size(352, 69);
			this.m_ctrlFiles.TabIndex = 13;
			// 
			// m_ctrlFilesLabel
			// 
			this.m_ctrlFilesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlFilesLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlFilesLabel.Location = new System.Drawing.Point(8, 64);
			this.m_ctrlFilesLabel.Name = "m_ctrlFilesLabel";
			this.m_ctrlFilesLabel.Size = new System.Drawing.Size(312, 16);
			this.m_ctrlFilesLabel.TabIndex = 14;
			this.m_ctrlFilesLabel.Text = "Files :";
			// 
			// CRFExportSummary
			// 
			this.AcceptButton = this.m_ctrlOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(368, 189);
			this.Controls.Add(this.m_ctrlFilesLabel);
			this.Controls.Add(this.m_ctrlFiles);
			this.Controls.Add(this.m_ctrlFolder);
			this.Controls.Add(this.m_ctrlTotal);
			this.Controls.Add(this.m_ctrlTargetLabel);
			this.Controls.Add(this.m_ctrlTotalLabel);
			this.Controls.Add(this.m_ctrlOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "CRFExportSummary";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Export Results";
			this.ResumeLayout(false);

		}
		
		/// <summary>This method is called to set the results folder</summary>
		private void SetExported(CTmaxSourceFolder tmaxExported)
		{
			try
			{
				m_tmaxExported = tmaxExported;
				
				m_ctrlFiles.Items.Clear();
				
				if(m_tmaxExported != null)
				{
					if(m_tmaxExported.Files.Count == 0)
						m_ctrlTotal.Text = "None";
					else if(m_tmaxExported.Files.Count == 1)
						m_ctrlTotal.Text = "1 report";
					else
						m_ctrlTotal.Text = (m_tmaxExported.Files.Count.ToString() + " reports");
						
					m_ctrlFolder.Text = m_tmaxExported.Path;
					
					foreach(CTmaxSourceFile O in m_tmaxExported.Files)
						m_ctrlFiles.Items.Add(O.Name);
						
				}
				else
				{
					m_ctrlTotal.Text = "";
					m_ctrlTotal.Text = "";
					m_ctrlTotalLabel.Enabled = false;
					m_ctrlFolder.Text = "";
					m_ctrlFolder.Enabled = false;
					m_ctrlTargetLabel.Enabled = false;
					m_ctrlFilesLabel.Enabled = false;
					m_ctrlFiles.Enabled = false;
				}	
				
			}
			catch
			{
			}

		}// private void SetExported(CTmaxSourceFolder tmaxExported)
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>Folder containing the exported files</summary>
		public CTmaxSourceFolder Exported
		{
			get { return m_tmaxExported; }
			set { SetExported(value); }
		}
		
		#endregion Properties
		
	}// public class CRFExportSummary : System.Windows.Forms.Form
	
}// namespace FTI.Trialmax.Reports
