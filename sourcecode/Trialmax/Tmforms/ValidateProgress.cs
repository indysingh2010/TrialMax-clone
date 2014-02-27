using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace FTI.Trialmax.Forms
{
	/// <summary>Form used to display progress during database validation</summary>
	public class CFValidateProgress : System.Windows.Forms.Form
	{
		#region Private Members

		/// <summary>Component container required by form designer</summary>
		private System.ComponentModel.IContainer components;
		
		/// <summary>The form's cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>Status text</summary>
		private System.Windows.Forms.Label m_ctrlStatus;
		
		/// <summary>Errors messages list view</summary>
		private System.Windows.Forms.ListView m_ctrlErrors;
		
		/// <summary>Error messages image column</summary>
		private System.Windows.Forms.ColumnHeader I;
		
		/// <summary>Error messages barcode column</summary>
		private System.Windows.Forms.ColumnHeader BARCODE;
		
		/// <summary>Error messages description column</summary>
		private System.Windows.Forms.ColumnHeader DESCRIPTION;
		
		/// <summary>Error messages list label</summary>
		private System.Windows.Forms.Label m_ctrlErrorsLabel;
		
		/// <summary>Local member bound to Cancelled property</summary>
		private bool m_bCancelled = false;

		/// <summary>Local member bound to Finished property</summary>
		private bool m_bFinished = false;

		/// <summary>Save As pushbutton</summary>
		private System.Windows.Forms.Button m_ctrlSaveAs;

		/// <summary>Image list for errors list view</summary>
		private System.Windows.Forms.ImageList m_ctrlImages;
		
		/// <summary>Local member bound to Status property</summary>
		private string m_strStatus = "";
		
		/// <summary>Local member bound to CaseFolder property</summary>
		private string m_strCaseFolder = "";
		
		/// <summary>Local member to store time at start</summary>
		private System.DateTime m_dtValidate = System.DateTime.Now;
		
		/// <summary>Local member bound to Documents property</summary>
		private long m_lDocuments = 0;
		
		/// <summary>Local member bound to Pages property</summary>
		private long m_lPages = 0;
		
		/// <summary>Local member bound to PowerPoints property</summary>
		private long m_lPowerPoints = 0;
		
		/// <summary>Local member bound to Recordings property</summary>
		private long m_lRecordings = 0;
		
		/// <summary>Local member bound to Depositions property</summary>
		private long m_lDepositions = 0;
		
		/// <summary>Local member bound to Videos property</summary>
		private long m_lVideos = 0;
		
		/// <summary>Local member to keep track of number of critical errors</summary>
		private long m_lErrors = 0;
		
		/// <summary>Local member to keep track of number of warnings</summary>
		private long m_lWarnings = 0;
		
		#endregion Private Methods
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFValidateProgress()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
		
		}// public CFValidateProgress()
	
		/// <summary>This method is called to set the status text</summary>
		/// <param name="strStatus">The new status text</param>
		public void SetStatus(string strStatus)
		{
			if(strStatus != null)
				m_strStatus = strStatus;
			else
				m_strStatus = "";
				
			if((m_ctrlStatus != null) && (m_ctrlStatus.IsDisposed == false))
				m_ctrlStatus.Text = m_strStatus;
		
		}// public void SetStatus(string strStatus)
		
		/// <summary>This method is called to add an error to the list box</summary>
		/// <param name="strBarcode">The barcode of the associated record</param>
		/// <param name="strMessage">The error message</param>
		/// <param name="bCritical">True if this is a critical error</param>
		public void AddError(string strBarcode, string strMessage, bool bCritical)
		{
			ListViewItem lvItem = null;
			
			if(m_ctrlErrors == null) return;
			if(m_ctrlErrors.IsDisposed == true) return;
			
			try
			{
				if(bCritical == true)
					m_lErrors++;
				else
					m_lWarnings++;
					
				lvItem = new ListViewItem();
							
				lvItem.ImageIndex = bCritical ? 0 : 1;
				lvItem.SubItems.Add(strBarcode);
				lvItem.SubItems.Add(strMessage);
							
				m_ctrlErrors.Items.Add(lvItem);

				//	Automatically resize the columns to fit the text
				SuspendLayout();
				m_ctrlErrors.Columns[0].Width = -2;
				m_ctrlErrors.Columns[1].Width = -2;
				m_ctrlErrors.Columns[2].Width = -2;
				ResumeLayout();

			}
			catch
			{
			}

		}// AddError(string strBarcode, string strMessage, bool bCritical)
		
		/// <summary>This method is called to add an error to the list box</summary>
		/// <param name="strBarcode">The barcode of the associated record</param>
		/// <param name="strMessage">The error message</param>
		public void AddError(string strBarcode, string strMessage)
		{
			AddError(strBarcode, strMessage, true);
			
		}// AddError(string strBarcode, string strMessage)
		
		/// <summary>This method is called to add an error warning to the list box</summary>
		/// <param name="strBarcode">The barcode of the associated record</param>
		/// <param name="strMessage">The error message</param>
		public void AddWarning(string strBarcode, string strMessage)
		{
			AddError(strBarcode, strMessage, false);
			
		}// AddWarning(string strBarcode, string strMessage)
		
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
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFValidateProgress));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlSaveAs = new System.Windows.Forms.Button();
			this.m_ctrlStatus = new System.Windows.Forms.Label();
			this.m_ctrlErrors = new System.Windows.Forms.ListView();
			this.I = new System.Windows.Forms.ColumnHeader();
			this.BARCODE = new System.Windows.Forms.ColumnHeader();
			this.DESCRIPTION = new System.Windows.Forms.ColumnHeader();
			this.m_ctrlErrorsLabel = new System.Windows.Forms.Label();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(292, 204);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 10;
			this.m_ctrlCancel.Text = "  &Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlSaveAs
			// 
			this.m_ctrlSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlSaveAs.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.m_ctrlSaveAs.Enabled = false;
			this.m_ctrlSaveAs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlSaveAs.Location = new System.Drawing.Point(204, 204);
			this.m_ctrlSaveAs.Name = "m_ctrlSaveAs";
			this.m_ctrlSaveAs.TabIndex = 9;
			this.m_ctrlSaveAs.Text = "&Save As";
			this.m_ctrlSaveAs.Click += new System.EventHandler(this.OnClickSaveAs);
			// 
			// m_ctrlStatus
			// 
			this.m_ctrlStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlStatus.Location = new System.Drawing.Point(8, 12);
			this.m_ctrlStatus.Name = "m_ctrlStatus";
			this.m_ctrlStatus.Size = new System.Drawing.Size(368, 23);
			this.m_ctrlStatus.TabIndex = 11;
			this.m_ctrlStatus.Text = "status";
			// 
			// m_ctrlErrors
			// 
			this.m_ctrlErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						   this.I,
																						   this.BARCODE,
																						   this.DESCRIPTION});
			this.m_ctrlErrors.FullRowSelect = true;
			this.m_ctrlErrors.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.m_ctrlErrors.HideSelection = false;
			this.m_ctrlErrors.LabelWrap = false;
			this.m_ctrlErrors.Location = new System.Drawing.Point(8, 60);
			this.m_ctrlErrors.MultiSelect = false;
			this.m_ctrlErrors.Name = "m_ctrlErrors";
			this.m_ctrlErrors.Size = new System.Drawing.Size(368, 136);
			this.m_ctrlErrors.SmallImageList = this.m_ctrlImages;
			this.m_ctrlErrors.TabIndex = 12;
			this.m_ctrlErrors.View = System.Windows.Forms.View.Details;
			// 
			// I
			// 
			this.I.Text = "";
			this.I.Width = 16;
			// 
			// BARCODE
			// 
			this.BARCODE.Text = "Barcode";
			this.BARCODE.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// DESCRIPTION
			// 
			this.DESCRIPTION.Text = "Description";
			// 
			// m_ctrlErrorsLabel
			// 
			this.m_ctrlErrorsLabel.Location = new System.Drawing.Point(8, 44);
			this.m_ctrlErrorsLabel.Name = "m_ctrlErrorsLabel";
			this.m_ctrlErrorsLabel.Size = new System.Drawing.Size(100, 16);
			this.m_ctrlErrorsLabel.TabIndex = 13;
			this.m_ctrlErrorsLabel.Text = "Errors:";
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// CFValidateProgress
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(384, 233);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlErrorsLabel);
			this.Controls.Add(this.m_ctrlErrors);
			this.Controls.Add(this.m_ctrlStatus);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlSaveAs);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFValidateProgress";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Validating ...";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);

		}
		
		/// <summary>This method is called when the user clicks on Save As</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickSaveAs(object sender, System.EventArgs e)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			
			if(m_ctrlErrors == null) return;
			if(m_ctrlErrors.IsDisposed == true) return;
			if(m_ctrlErrors.Items == null) return;
			if(m_ctrlErrors.Items.Count == 0) return;
			
			//	Initialize the file selection dialog
			dlg.AddExtension = true;
			dlg.DefaultExt = "txt";
			dlg.CheckPathExists = true;
			dlg.OverwritePrompt = true;
			dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
			
			//	Open the dialog box
			if(dlg.ShowDialog() == DialogResult.OK) 
			{
				Save(dlg.FileName);
			}
			
		}// private void OnClickSaveAs(object sender, System.EventArgs e)
		
		/// <summary>This method is called when the user clicks on Cancel</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickCancel(object sender, System.EventArgs e)
		{
			//	Has the operation been cancelled yet?
			if((m_bCancelled == true) || (m_bFinished == true))
			{
				//	Close the form
				DialogResult = DialogResult.Cancel;
				this.Close();
			}
			else
			{
				//	Cancel the operation
				m_bCancelled = true;
				
				//	Change the text on the cancelled button
				OnEndValidation();
				
				//	Show the summary
				ShowSummary();
			}
			
		}// private void OnClickCancel(object sender, System.EventArgs e)

		/// <summary>This method is called when the form is being loaded</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnLoad(object sender, System.EventArgs e)
		{
			m_dtValidate = DateTime.Now;
			
			//	Initialize the status text control
			SetStatus(m_strStatus);
			
			this.Text = ("Validating " + m_strCaseFolder);
			
			//	Automatically resize the columns to fit the column text
			SuspendLayout();
			m_ctrlErrors.Columns[1].Width = -2;
			m_ctrlErrors.Columns[2].Width = -2;
			ResumeLayout();
		}

		/// <summary>This method is called to save the errors to the specified file</summary>
		private void Save(string strFileSpec)
		{
			System.IO.StreamWriter	fileStream = null;
			string					strText = "";
			
			try
			{
				fileStream = System.IO.File.CreateText(strFileSpec);
			}
			catch(System.Exception Ex)
			{
				strText = ("Unable to open " + strFileSpec + " \n\nException:\n\n");
				strText += Ex.ToString();
				MessageBox.Show(strText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			
			//	Write a header line
			strText = "Validation results for ";
			strText += m_strCaseFolder;
			strText += "   ";
			strText += m_dtValidate.ToString();
			fileStream.WriteLine(strText);
			fileStream.WriteLine("------------------------------------------------------------------------------------------------------------------------------");
			
			//	Write the media summary
			strText = GetSummaryMessage();
			if(strText.Length > 0)
			{
				fileStream.WriteLine("");
				fileStream.WriteLine(strText);
				fileStream.WriteLine("");
				fileStream.WriteLine("");
			}
				
			foreach(ListViewItem O in m_ctrlErrors.Items)
			{
				strText = O.SubItems[1].Text;
				strText += "\t\t";
				strText += O.SubItems[2].Text;
				
				try
				{
					fileStream.WriteLine(strText);
				}
				catch(System.Exception Ex)
				{
					strText = ("Error writing to " + strFileSpec + "\n\n");
					strText += "Exception:\n\n";
					strText += Ex.ToString();
					break;
				}
			
			}// foreach(ListViewItem O in m_ctrlErrors.Items)
			
			fileStream.Close();

			try
			{
				FTI.Shared.Win32.Shell.ShellExecute(this.Handle, 
					"open",
					strFileSpec,
					"",
					"",
					FTI.Shared.Win32.User.SW_SHOWDEFAULT);
			}
			catch
			{
			}
				
		}// private void Save(string strFileSpec)
		
		/// <summary>This method is called when the operation stops</summary>
		private void OnEndValidation()
		{
			//	Change the text on the cancel button
			if((m_ctrlCancel != null) && (m_ctrlCancel.IsDisposed == false))
			{
				m_ctrlCancel.Text = "&Close";
			}
				
			//	Should we enable the Save As button?
			if((m_ctrlSaveAs != null) && (m_ctrlSaveAs.IsDisposed == false))
			{
				if((m_ctrlErrors != null) && (m_ctrlErrors.IsDisposed == false))
				{
					if(m_ctrlErrors.Items.Count > 0)
						m_ctrlSaveAs.Enabled = true;
				}

			}
			
			//	Display the summary information
			ShowSummary();
			
		}// private void OnEndValidation()
		
		/// <summary>Called to get the summary message</summary>
		/// <returns>The summary message to be displayed when the operation terminates</returns>
		private string GetSummaryMessage()
		{
			string strMsg = "";
			
			// Docments / pages
			if(m_lDocuments > 0)
			{
				if(m_lPages > 0)
					strMsg = String.Format("{0} pages in {1} documents", m_lPages, m_lDocuments);
				else
					strMsg = String.Format("{0} documents", m_lDocuments);
			}
			else if(m_lPages > 0)
			{
				strMsg = String.Format("{0} pages", m_lPages);
			}
			
			//	PowerPoint presentations
			if(m_lPowerPoints > 0)
			{
				if(strMsg.Length > 0)
					strMsg += "\n";
				strMsg += String.Format("{0} PowerPoint presentations", m_lPowerPoints);
			}
			
			//	Recordings
			if(m_lRecordings > 0)
			{
				if(strMsg.Length > 0)
					strMsg += "\n";
				strMsg += String.Format("{0} recordings", m_lRecordings);
			}
			
			// Depositions / video files
			if(m_lDepositions > 0)
			{
				if(strMsg.Length > 0)
					strMsg += "\n";

				if(m_lVideos > 0)
					strMsg += String.Format("{0} video files in {1} depositions", m_lVideos, m_lDepositions);
				else
					strMsg += String.Format("{0} depositions", m_lDepositions);
			}
			else if(m_lVideos > 0)
			{
				if(strMsg.Length > 0)
					strMsg += "\n";
				strMsg += String.Format("{0} video files", m_lVideos);
			}
			
			if(strMsg.Length > 0)
				strMsg += "\n";
			strMsg += String.Format("\n{0} critical errors", m_lErrors);
			strMsg += String.Format("\n{0} warnings", m_lWarnings);
			
			return strMsg;
			
		}// private string GetSummaryMessage()
		
		/// <summary>This method is called to show the summary information</summary>
		private void ShowSummary()
		{
			string	strSummary = "";
			string	strMsg = "";
			
			strSummary = GetSummaryMessage();
			
			if((strSummary != null) && (strSummary.Length > 0))
			{
				strMsg = "Validation results: ";
				strMsg += "\n---------------------------------\n\n";
				strMsg += strSummary;
				
				MessageBox.Show(strMsg, "Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
				
		}// private void ShowSummary()
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>Status text</summary>
		public string Status
		{
			get { return m_strStatus; }
			set { SetStatus(value); }
		}
		
		/// <summary>Case folder</summary>
		public string CaseFolder
		{
			get { return m_strCaseFolder; }
			set { m_strCaseFolder = value; }
		}
		
		/// <summary>True if user has cancelled the operation</summary>
		public bool Cancelled
		{
			get { return m_bCancelled; }
		}
		
		/// <summary>True if validation is complete</summary>
		public bool Finished
		{
			get 
			{ 
				return m_bFinished; 
			}
			set
			{
				m_bFinished = value;
				
				//	Change the text on the cancel button
				OnEndValidation();				
			}
			
		}
		
		/// <summary>Number of documents validated</summary>
		public long Documents
		{
			get { return m_lDocuments; }
			set { m_lDocuments = value; }
		}
		
		/// <summary>Number of pages validated</summary>
		public long Pages
		{
			get { return m_lPages; }
			set { m_lPages = value; }
		}
		
		/// <summary>Number of PowerPoints validated</summary>
		public long PowerPoints
		{
			get { return m_lPowerPoints; }
			set { m_lPowerPoints = value; }
		}
		
		/// <summary>Number of Recordings validated</summary>
		public long Recordings
		{
			get { return m_lRecordings; }
			set { m_lRecordings = value; }
		}
		
		/// <summary>Number of Depositions validated</summary>
		public long Depositions
		{
			get { return m_lDepositions; }
			set { m_lDepositions = value; }
		}
		
		/// <summary>Number of Video Files validated</summary>
		public long Videos
		{
			get { return m_lVideos; }
			set { m_lVideos = value; }
		}
		
		#endregion Properties

	}// public class CFValidateProgress : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Forms
