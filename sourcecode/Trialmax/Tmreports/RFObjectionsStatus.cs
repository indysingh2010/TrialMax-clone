using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Controls;

namespace FTI.Trialmax.Reports
{
	/// <summary>This form displays status information during an import operation</summary>
	public class CRFObjectionsStatus : System.Windows.Forms.Form
	{
		#region Private Members

		/// <summary>Component collection required by forms designer</summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>Button to abort the current operation</summary>
		private System.Windows.Forms.Button m_ctrlAbort;

		/// <summary>Local member bound to Aborted property</summary>
		private bool m_bAborted = false;

		/// <summary>Local member bound to Finished property</summary>
		private bool m_bFinished = false;

		/// <summary>Local member bound to Status property</summary>
		private string m_strStatus = "";

		/// <summary>Status text label for status message control</summary>
		private System.Windows.Forms.Label m_ctrlStatusLabel;

		/// <summary>Static text control to display the form's status message</summary>
		private System.Windows.Forms.Label m_ctrlStatusMessage;

		/// <summary>List view to display error messages</summary>
		private FTI.Trialmax.Controls.CTmaxListViewCtrl m_ctrlMessages;

		/// <summary>Static text label for the messages list</summary>
		private System.Windows.Forms.Label m_ctrlMessagesLabel;

		/// <summary>Pushbutton to save the contents of the errors list</summary>
		private System.Windows.Forms.Button m_ctrlSaveAs;
		private Infragistics.Win.UltraWinProgressBar.UltraProgressBar m_ctrlProgressBar;

		/// <summary>Collection of error messages displayed in the list view control</summary>
		private CTmaxExportMessages m_tmaxExportMessages = new CTmaxExportMessages();

		#endregion Private Members

		#region Public Methods

		/// <summary>Constructor</summary>
		public CRFObjectionsStatus()
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

		/// <summary>Called to add a new message to the errors list box</summary>
		/// <param name="strMessage">text associated with the message</param>
		/// <param name="strFilename">the file associated with the message</param>
		/// <param name="eType">enumerated error level</param>
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

		/// <summary>Called to step the progress bar</summary>
		public void StepProgressBar()
		{
			//	Step the progress bar
			try
			{
				if(m_ctrlProgressBar.Value + m_ctrlProgressBar.Step > m_ctrlProgressBar.Maximum)
					m_ctrlProgressBar.Value = m_ctrlProgressBar.Minimum;
				else
					m_ctrlProgressBar.PerformStep();

				m_ctrlProgressBar.Refresh();
				Application.DoEvents();
			}
			catch
			{
			}

		}// public void StepProgressBar()

		/// <summary>Called to set the current value of the progress bar</summary>
		/// <param name="iPercent">the percent to be used to position the progress bar</param>
		public void SetProgress(int iPercent)
		{
			//	Step the progress bar
			try
			{
				if(iPercent > m_ctrlProgressBar.Maximum)
					m_ctrlProgressBar.Value = m_ctrlProgressBar.Maximum;
				else if(iPercent < m_ctrlProgressBar.Minimum)
					m_ctrlProgressBar.Value = m_ctrlProgressBar.Minimum;
				else
					m_ctrlProgressBar.Value = iPercent;

				if(m_ctrlProgressBar.TextVisible == false)
					m_ctrlProgressBar.TextVisible = true;
					
				m_ctrlProgressBar.Refresh();
				Application.DoEvents();
			}
			catch
			{
			}

		}// public void SetProgress(int iPercent)

		#endregion Public Methods

		#region Protected Methods

		/// <summary>Overridden base class member called when the form gets loaded</summary>
		/// <param name="e">System event items</param>
		protected override void OnLoad(EventArgs e)
		{
			if(m_ctrlStatusMessage != null)
				m_ctrlStatusMessage.Text = m_strStatus;

			m_ctrlMessages.Initialize(new CTmaxExportMessage());

			base.OnLoad(e);
		}

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

		/// <summary>This method is called to save the errors to the specified file</summary>
		private void Save(string strFileSpec)
		{
			System.IO.StreamWriter fileStream = null;
			string strLine = "";

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
				strLine = "TrialMax Objections Report Operation:  ";
				strLine += DateTime.Now.ToString();
				fileStream.WriteLine(strLine);
				fileStream.WriteLine("----------------------------------------------------------------------------");

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
			this.m_ctrlMessages = new FTI.Trialmax.Controls.CTmaxListViewCtrl();
			this.m_ctrlMessagesLabel = new System.Windows.Forms.Label();
			this.m_ctrlSaveAs = new System.Windows.Forms.Button();
			this.m_ctrlProgressBar = new Infragistics.Win.UltraWinProgressBar.UltraProgressBar();
			this.SuspendLayout();
			// 
			// m_ctrlAbort
			// 
			this.m_ctrlAbort.Location = new System.Drawing.Point(374, 68);
			this.m_ctrlAbort.Name = "m_ctrlAbort";
			this.m_ctrlAbort.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlAbort.TabIndex = 2;
			this.m_ctrlAbort.Text = "&Abort";
			this.m_ctrlAbort.Click += new System.EventHandler(this.OnClickAbort);
			// 
			// m_ctrlStatusLabel
			// 
			this.m_ctrlStatusLabel.Location = new System.Drawing.Point(8, 12);
			this.m_ctrlStatusLabel.Name = "m_ctrlStatusLabel";
			this.m_ctrlStatusLabel.Size = new System.Drawing.Size(50, 24);
			this.m_ctrlStatusLabel.TabIndex = 3;
			this.m_ctrlStatusLabel.Text = "Status:";
			// 
			// m_ctrlStatusMessage
			// 
			this.m_ctrlStatusMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlStatusMessage.Location = new System.Drawing.Point(57, 12);
			this.m_ctrlStatusMessage.Name = "m_ctrlStatusMessage";
			this.m_ctrlStatusMessage.Size = new System.Drawing.Size(393, 24);
			this.m_ctrlStatusMessage.TabIndex = 4;
			// 
			// m_ctrlMessages
			// 
			this.m_ctrlMessages.AddTop = false;
			this.m_ctrlMessages.AutoResizeColumns = true;
			this.m_ctrlMessages.ClearOnDblClick = false;
			this.m_ctrlMessages.DisplayMode = 0;
			this.m_ctrlMessages.HideSelection = false;
			this.m_ctrlMessages.Location = new System.Drawing.Point(59, 70);
			this.m_ctrlMessages.MaxRows = 0;
			this.m_ctrlMessages.Name = "m_ctrlMessages";
			this.m_ctrlMessages.OwnerImages = null;
			this.m_ctrlMessages.PaneId = 0;
			this.m_ctrlMessages.SelectedIndex = -1;
			this.m_ctrlMessages.ShowHeaders = true;
			this.m_ctrlMessages.ShowImage = true;
			this.m_ctrlMessages.Size = new System.Drawing.Size(101, 23);
			this.m_ctrlMessages.TabIndex = 7;
			this.m_ctrlMessages.Visible = false;
			// 
			// m_ctrlMessagesLabel
			// 
			this.m_ctrlMessagesLabel.Location = new System.Drawing.Point(7, 70);
			this.m_ctrlMessagesLabel.Name = "m_ctrlMessagesLabel";
			this.m_ctrlMessagesLabel.Size = new System.Drawing.Size(46, 15);
			this.m_ctrlMessagesLabel.TabIndex = 10;
			this.m_ctrlMessagesLabel.Text = "Errors:";
			this.m_ctrlMessagesLabel.Visible = false;
			// 
			// m_ctrlSaveAs
			// 
			this.m_ctrlSaveAs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlSaveAs.Location = new System.Drawing.Point(166, 71);
			this.m_ctrlSaveAs.Name = "m_ctrlSaveAs";
			this.m_ctrlSaveAs.Size = new System.Drawing.Size(75, 23);
			this.m_ctrlSaveAs.TabIndex = 11;
			this.m_ctrlSaveAs.Text = "&Save As";
			this.m_ctrlSaveAs.Visible = false;
			this.m_ctrlSaveAs.Click += new System.EventHandler(this.OnClickSaveAs);
			// 
			// m_ctrlProgressBar
			// 
			this.m_ctrlProgressBar.Location = new System.Drawing.Point(12, 47);
			this.m_ctrlProgressBar.Name = "m_ctrlProgressBar";
			this.m_ctrlProgressBar.Size = new System.Drawing.Size(437, 15);
			this.m_ctrlProgressBar.Step = 5;
			this.m_ctrlProgressBar.TabIndex = 13;
			this.m_ctrlProgressBar.Text = "[Formatted]";
			this.m_ctrlProgressBar.TextVisible = false;
			// 
			// CRFObjectionsStatus
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(458, 100);
			this.ControlBox = false;
			this.Controls.Add(this.m_ctrlProgressBar);
			this.Controls.Add(this.m_ctrlSaveAs);
			this.Controls.Add(this.m_ctrlMessagesLabel);
			this.Controls.Add(this.m_ctrlMessages);
			this.Controls.Add(this.m_ctrlStatusMessage);
			this.Controls.Add(this.m_ctrlStatusLabel);
			this.Controls.Add(this.m_ctrlAbort);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "CRFObjectionsStatus";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Objections Report Status";
			this.ResumeLayout(false);

		}// private void InitializeComponent()

		/// <summary>This method is called when the user clicks on Abort</summary>
		/// <param name="sender">the object firing the event</param>
		/// <param name="e">system event arguments</param>
		private void OnClickAbort(object sender, System.EventArgs e)
		{
			//	Set the flag to indicate that the user has aborted the operation
			m_bAborted = true;
				
			//	Close the form
			this.DialogResult = DialogResult.Abort;
			this.Close();
			
		}// private void OnClickAbort(object sender, System.EventArgs e)

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

		/// <summary>True if the operation has been aborted by the user</summary>
		public bool Aborted
		{
			get { return m_bAborted; }
		}

		/// <summary>True if the operation has been finished normally</summary>
		public bool Finished
		{
			get { return m_bFinished; }
			set { m_bFinished = value; }
		}

		#endregion Properties

	}// public class CRFObjectionsStatus : System.Windows.Forms.Form 
	

}// namespace FTI.Trialmax.Reports
