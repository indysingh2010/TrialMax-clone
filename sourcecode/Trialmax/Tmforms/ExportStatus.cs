using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Forms
{
	/// <summary>This form displays status information during an import operation</summary>
	public class CFExportStatus : System.Windows.Forms.Form
	{
		#region Private Members

		/// <summary>Component collection required by forms designer</summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>Button to abort the current operation</summary>
		private System.Windows.Forms.Button m_ctrlAbort;
		
		/// <summary>Local member bound to Aborted property</summary>
		private bool m_bAborted = false;
		
		/// <summary>Local member bound to Summary property</summary>
		private string m_strSummary = "";
		
		/// <summary>Local member bound to Status property</summary>
		private string m_strStatus = "";
		
		/// <summary>Local member bound to Filename property</summary>
		private string m_strFilename = "";
		
		/// <summary>Status text label for status message control</summary>
		private System.Windows.Forms.Label m_ctrlStatusLabel;

		/// <summary>Static text control to display the form's status message</summary>
		private System.Windows.Forms.Label m_ctrlStatusMessage;
		
		/// <summary>Static text control to display the name of the active import file</summary>
		private System.Windows.Forms.Label m_ctrlFilename;
		
		/// <summary>Static text label for the Filename control</summary>
		private System.Windows.Forms.Label m_ctrlFilenameLabel;
		
		/// <summary>List view to display error messages</summary>
		private FTI.Trialmax.Controls.CTmaxListViewCtrl m_ctrlMessages;
		
		/// <summary>Local member bound to Finished property</summary>
		private bool m_bFinished = false;
		
		/// <summary>Static text to display operation summary</summary>
		private System.Windows.Forms.Label m_ctrlSummary;
		
		/// <summary>Static text label for the Summary control</summary>
		private System.Windows.Forms.Label m_ctrlSummaryLabel;
		
		/// <summary>Static text label for the messages list</summary>
		private System.Windows.Forms.Label m_ctrlMessagesLabel;
		private System.Windows.Forms.Button m_ctrlSaveAs;
		
		/// <summary>Collection of error messages displayed in the list view control</summary>
		private CTmaxExportMessages m_tmaxExportMessages = new CTmaxExportMessages();
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFExportStatus()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
		}
		
		/// <summary>Called to set the status message displayed in the form</summary>
		/// <param name="strStatus">the new status message</param>
		public void SetStatus(string strStatus)
		{
			try
			{
				if(strStatus != null)
					m_strStatus = strStatus;
				else
					m_strStatus = "";
					
				if((m_ctrlStatusMessage != null) && (m_bAborted == false))
					m_ctrlStatusMessage.Text = m_strStatus; 
			
				Application.DoEvents();

			}
			catch
			{
			
			}
			
		}// public void SetStatus(string strStatus)
		
		/// <summary>Called to set the summary message displayed in the form</summary>
		/// <param name="strStatus">the new summary message</param>
		public void SetSummary(string strSummary)
		{
			try
			{
				if(strSummary != null)
					m_strSummary = strSummary;
				else
					m_strSummary = "";
					
				if(m_strSummary.Length > 0)
					MessageBox.Show(this, m_strSummary, "Export Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
			
				if(m_ctrlSummary != null)
					m_ctrlSummary.Text = m_strSummary;
			}
			catch
			{
			
			}
			
		}// public void SetSummary(string strSummary)
		
		/// <summary>Called to set the filename displayed in the form</summary>
		/// <param name="strFilename">the new filename</param>
		public void SetFilename(string strFilename)
		{
			try
			{
				if(strFilename != null)
					m_strFilename = strFilename;
				else
					m_strFilename = "";
					
				if(m_ctrlFilename != null)
					m_ctrlFilename.Text = m_strFilename; 
			
			}
			catch
			{
			
			}
			
		}// public void SetFilename(string strFilename)
		
		/// <summary>Called to set the flag to indicate that the operation is finished</summary>
		/// <param name="bFinished">true if finished</param>
		public void SetFinished(bool bFinished)
		{
			try
			{
				if(m_bFinished != bFinished)
				{
					m_bFinished = bFinished;
					
					if(m_bFinished == true)
						SwitchToClose();
				}
			
			}
			catch
			{
			
			}
			
		}// public void SetFinished(bool bFinished)
		
		/// <summary>Called to add a new message to the errors list box</summary>
		/// <param name="strMessage">text associated with the message</param>
		/// <param name="strFilename">the file associated with the message</param>
		/// <param name="eType">message type identifier to indicate error level</param>
		public void AddMessage(string strMessage, string strFilename, TmaxMessageLevels eLevel)
		{
			try
			{
				//	Create a new export message
				CTmaxExportMessage tmaxMessage = new CTmaxExportMessage();
			
				tmaxMessage.Filename = strFilename;
				tmaxMessage.Message = strMessage;
				tmaxMessage.Level = eLevel;
				
				m_ctrlMessages.Add(tmaxMessage);
				m_tmaxExportMessages.Add(tmaxMessage);
			}
			catch
			{
			}
			
		}// public void AddError(string strFilename, int iLine, string strMessage)

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Overridden base class member called when the form gets loaded</summary>
		/// <param name="e">System event items</param>
		protected override void OnLoad(EventArgs e)
		{
			if(m_ctrlStatusMessage != null)
				m_ctrlStatusMessage.Text = m_strStatus;
			if(m_ctrlFilename != null)
				m_ctrlFilename.Text = m_strFilename;
				
			m_ctrlMessages.Initialize(new CTmaxExportMessage());
			
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
		
		/// <summary>This method is called to save the errors to the specified file</summary>
		private void Save(string strFileSpec)
		{
			System.IO.StreamWriter	fileStream = null;
			string					strLine = "";
			
			try
			{
				fileStream = System.IO.File.CreateText(strFileSpec);
			}
			catch(System.Exception Ex)
			{
				strLine = ("Unable to open " + strFileSpec + " \n\nException:\n\n");
				strLine += Ex.ToString();
				MessageBox.Show(strLine, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			
			try
			{
				//	Write a header line
				strLine = "TrialMax Export Operation:  ";
				strLine += DateTime.Now.ToString();
				fileStream.WriteLine(strLine);
				fileStream.WriteLine("----------------------------------------------------------------------------");
				
				//	Write the summary message
				strLine = ("Summary: " + m_strSummary);
				fileStream.WriteLine(strLine);
				
				strLine = "Errors: ";
				if(m_tmaxExportMessages.Count == 0)
					strLine += "None";
				else
					strLine += m_tmaxExportMessages.Count.ToString();
				fileStream.WriteLine(strLine);
				fileStream.WriteLine("");
					
				foreach(CTmaxExportMessage O in m_tmaxExportMessages)
				{
					switch(O.Level)
					{
						case TmaxMessageLevels.CriticalError:
						case TmaxMessageLevels.FatalError:
						
							strLine = "Critical: ";
							break;
							
						case TmaxMessageLevels.Warning:
						
							strLine = "Warning: ";
							break;
							
						default:
						
							strLine = "General: ";
							break;
							
					}
					
					strLine += O.Message;
					
					if(O.Filename.Length > 0)
						strLine += (" " + O.Filename);
						
					fileStream.WriteLine(strLine);
				
				}// foreach(CTmaxExportMessage O in m_tmaxExportMessages)
			
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

			}
			catch(System.Exception Ex)
			{
				fileStream.Close();
				strLine = ("Error writing to " + strFileSpec + "\n\n");
				strLine += "Exception:\n\n";
				strLine += Ex.ToString();
				MessageBox.Show(strLine);
			}
				
		}// private void Save(string strFileSpec)
		
		/// <summary>This method is called when the user clicks on Save As</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickSaveAs(object sender, System.EventArgs e)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			
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
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			this.m_ctrlAbort = new System.Windows.Forms.Button();
			this.m_ctrlStatusLabel = new System.Windows.Forms.Label();
			this.m_ctrlStatusMessage = new System.Windows.Forms.Label();
			this.m_ctrlFilename = new System.Windows.Forms.Label();
			this.m_ctrlFilenameLabel = new System.Windows.Forms.Label();
			this.m_ctrlMessages = new FTI.Trialmax.Controls.CTmaxListViewCtrl();
			this.m_ctrlSummary = new System.Windows.Forms.Label();
			this.m_ctrlSummaryLabel = new System.Windows.Forms.Label();
			this.m_ctrlMessagesLabel = new System.Windows.Forms.Label();
			this.m_ctrlSaveAs = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// m_ctrlAbort
			// 
			this.m_ctrlAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlAbort.Location = new System.Drawing.Point(358, 220);
			this.m_ctrlAbort.Name = "m_ctrlAbort";
			this.m_ctrlAbort.TabIndex = 2;
			this.m_ctrlAbort.Text = "&Abort";
			this.m_ctrlAbort.Click += new System.EventHandler(this.OnClickAbort);
			// 
			// m_ctrlStatusLabel
			// 
			this.m_ctrlStatusLabel.Location = new System.Drawing.Point(8, 36);
			this.m_ctrlStatusLabel.Name = "m_ctrlStatusLabel";
			this.m_ctrlStatusLabel.Size = new System.Drawing.Size(60, 16);
			this.m_ctrlStatusLabel.TabIndex = 3;
			this.m_ctrlStatusLabel.Text = "Status:";
			// 
			// m_ctrlStatusMessage
			// 
			this.m_ctrlStatusMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlStatusMessage.Location = new System.Drawing.Point(72, 36);
			this.m_ctrlStatusMessage.Name = "m_ctrlStatusMessage";
			this.m_ctrlStatusMessage.Size = new System.Drawing.Size(378, 24);
			this.m_ctrlStatusMessage.TabIndex = 4;
			// 
			// m_ctrlFilename
			// 
			this.m_ctrlFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlFilename.Location = new System.Drawing.Point(72, 12);
			this.m_ctrlFilename.Name = "m_ctrlFilename";
			this.m_ctrlFilename.Size = new System.Drawing.Size(378, 16);
			this.m_ctrlFilename.TabIndex = 6;
			// 
			// m_ctrlFilenameLabel
			// 
			this.m_ctrlFilenameLabel.Location = new System.Drawing.Point(8, 12);
			this.m_ctrlFilenameLabel.Name = "m_ctrlFilenameLabel";
			this.m_ctrlFilenameLabel.Size = new System.Drawing.Size(60, 16);
			this.m_ctrlFilenameLabel.TabIndex = 5;
			this.m_ctrlFilenameLabel.Text = "Filename:";
			// 
			// m_ctrlMessages
			// 
			this.m_ctrlMessages.AddTop = false;
			this.m_ctrlMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlMessages.AutoResizeColumns = true;
			this.m_ctrlMessages.ClearOnDblClick = false;
			this.m_ctrlMessages.DisplayMode = 0;
			this.m_ctrlMessages.HideSelection = false;
			this.m_ctrlMessages.Location = new System.Drawing.Point(8, 116);
			this.m_ctrlMessages.MaxRows = 0;
			this.m_ctrlMessages.Name = "m_ctrlMessages";
			this.m_ctrlMessages.OwnerImages = null;
			this.m_ctrlMessages.SelectedIndex = -1;
			this.m_ctrlMessages.ShowHeaders = true;
			this.m_ctrlMessages.ShowImage = true;
			this.m_ctrlMessages.Size = new System.Drawing.Size(442, 92);
			this.m_ctrlMessages.TabIndex = 7;
			// 
			// m_ctrlSummary
			// 
			this.m_ctrlSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlSummary.Location = new System.Drawing.Point(72, 68);
			this.m_ctrlSummary.Name = "m_ctrlSummary";
			this.m_ctrlSummary.Size = new System.Drawing.Size(378, 24);
			this.m_ctrlSummary.TabIndex = 9;
			// 
			// m_ctrlSummaryLabel
			// 
			this.m_ctrlSummaryLabel.Location = new System.Drawing.Point(8, 68);
			this.m_ctrlSummaryLabel.Name = "m_ctrlSummaryLabel";
			this.m_ctrlSummaryLabel.Size = new System.Drawing.Size(60, 16);
			this.m_ctrlSummaryLabel.TabIndex = 8;
			this.m_ctrlSummaryLabel.Text = "Summary:";
			// 
			// m_ctrlMessagesLabel
			// 
			this.m_ctrlMessagesLabel.Location = new System.Drawing.Point(8, 100);
			this.m_ctrlMessagesLabel.Name = "m_ctrlMessagesLabel";
			this.m_ctrlMessagesLabel.Size = new System.Drawing.Size(116, 16);
			this.m_ctrlMessagesLabel.TabIndex = 10;
			this.m_ctrlMessagesLabel.Text = "Errors:";
			// 
			// m_ctrlSaveAs
			// 
			this.m_ctrlSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlSaveAs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlSaveAs.Location = new System.Drawing.Point(268, 220);
			this.m_ctrlSaveAs.Name = "m_ctrlSaveAs";
			this.m_ctrlSaveAs.TabIndex = 11;
			this.m_ctrlSaveAs.Text = "&Save As";
			this.m_ctrlSaveAs.Click += new System.EventHandler(this.OnClickSaveAs);
			// 
			// CFExportStatus
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(458, 255);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlSaveAs);
			this.Controls.Add(this.m_ctrlMessagesLabel);
			this.Controls.Add(this.m_ctrlSummary);
			this.Controls.Add(this.m_ctrlSummaryLabel);
			this.Controls.Add(this.m_ctrlMessages);
			this.Controls.Add(this.m_ctrlFilename);
			this.Controls.Add(this.m_ctrlFilenameLabel);
			this.Controls.Add(this.m_ctrlStatusMessage);
			this.Controls.Add(this.m_ctrlStatusLabel);
			this.Controls.Add(this.m_ctrlAbort);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "CFExportStatus";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Export Status";
			this.ResumeLayout(false);

		}// private void InitializeComponent()
		
		/// <summary>This method is called when the user clicks on Abort</summary>
		/// <param name="sender">the object firing the event</param>
		/// <param name="e">system event arguments</param>
		private void OnClickAbort(object sender, System.EventArgs e)
		{
			//	Should we close the form?
			if((m_bAborted == true) || (m_bFinished == true))
			{
				DialogResult = (m_bFinished == true) ? DialogResult.OK : DialogResult.Abort;
				this.Close();
			}
			else
			{
				//	Update the status message
				this.Status = "Operation aborted";
				
				//	User has aborted the operation
				m_bAborted = true;
				
				//	Allow the user to close the form
				SwitchToClose();
			}
			
		}// private void OnClickAbort(object sender, System.EventArgs e)
		
		/// <summary>This method is called to reset the controls to allow the user to close the form</summary>
		private void SwitchToClose()
		{
			Application.DoEvents();

			//	Change the text on the button
			if((m_ctrlAbort != null) && (m_ctrlAbort.IsDisposed == false))
				m_ctrlAbort.Text = "&OK";
			
		}// private void SwitchToClose()
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>Title to be displayed in the form caption</summary>
		public string Title
		{
			get { return this.Text; }
			set { this.Text = value; }
		}
		
		/// <summary>The form's status message</summary>
		public string Status
		{
			get { return m_strStatus; }
			set { SetStatus(value); }
		}
		
		/// <summary>The form's summary message</summary>
		public string Summary
		{
			get { return m_strSummary; }
			set { SetSummary(value); }
		}
		
		/// <summary>The active file</summary>
		public string Filename
		{
			get { return m_strFilename; }
			set { SetFilename(value); }
		}
		
		/// <summary>True if the operation has been aborted by the user</summary>
		public bool Aborted
		{
			get { return m_bAborted; }
		}
		
		/// <summary>True if the operation has finished</summary>
		public bool Finished
		{
			get { return m_bFinished;  }
			set { SetFinished(value); }
		}
		
		#endregion Properties
		
	}// public class CFExportStatus : System.Windows.Forms.Form 
	
}// namespace FTI.Trialmax.Forms
