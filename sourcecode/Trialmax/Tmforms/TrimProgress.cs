using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Forms
{
    /// <summary>Form used to display progress during database validation</summary>
    public partial class CFTrimProgress : System.Windows.Forms.Form
    {
        #region Private Members

        /// <summary>Local member bound to Cancelled property</summary>
        private bool m_bCancelled = false;

		/// <summary>Local member to make sure summary only gets displayed once</summary>
		private bool m_bShowSummary = true;

		/// <summary>Local member bound to Finished property</summary>
        private bool m_bFinished = false;

        /// <summary>Local member bound to Status property</summary>
        private string m_strStatus = "";

        /// <summary>Local member bound to CaseFolder property</summary>
        private string m_strCaseFolder = "";

        /// <summary>Local member to keep track of number of critical errors</summary>
        private long m_lErrors = 0;

        /// <summary>Local member to keep track of number of warnings</summary>
        private long m_lWarnings = 0;

        #endregion Private Methods

        #region Public Methods

        /// <summary>Constructor</summary>
        public CFTrimProgress()
        {
            // Required for Windows Form Designer support
            InitializeComponent();

        }// public CFTrimProgress()

        /// <summary>This method is called to set the status text</summary>
        /// <param name="strStatus">The new status text</param>
        public void SetStatus(string strStatus)
        {
            if (strStatus != null)
                m_strStatus = strStatus;
            else
                m_strStatus = "";

            if ((m_ctrlStatus != null) && (m_ctrlStatus.IsDisposed == false))
                m_ctrlStatus.Text = CTmaxToolbox.FitPathToWidth(m_strStatus, m_ctrlStatus);

        }// public void SetStatus(string strStatus)

        /// <summary>This method is called to add an error to the list box</summary>
        /// <param name="strBarcode">The barcode of the associated record</param>
        /// <param name="strMessage">The error message</param>
        /// <param name="bCritical">True if this is a critical error</param>
        public void AddError(string strBarcode, string strMessage, bool bCritical)
        {
            ListViewItem lvItem = null;

            if (m_ctrlErrors == null) return;
            if (m_ctrlErrors.IsDisposed == true) return;

            try
            {
                if (bCritical == true)
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

        #region Private Methods

        /// <summary>This method is called when the user clicks on Save As</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">The event arguments</param>
        private void OnClickSaveAs(object sender, System.EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            if (m_ctrlErrors == null) return;
            if (m_ctrlErrors.IsDisposed == true) return;
            if (m_ctrlErrors.Items == null) return;
            if (m_ctrlErrors.Items.Count == 0) return;

            //	Initialize the file selection dialog
            dlg.AddExtension = true;
            dlg.DefaultExt = "txt";
            dlg.CheckPathExists = true;
            dlg.OverwritePrompt = true;
            dlg.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            //	Open the dialog box
            if (dlg.ShowDialog() == DialogResult.OK)
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
            if ((m_bCancelled == true) || (m_bFinished == true))
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
                OnEndOperation();

                //	Show the summary
                ShowSummary();
            }

        }// private void OnClickCancel(object sender, System.EventArgs e)

        /// <summary>This method is called when the form is being loaded</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">The event arguments</param>
        private void OnLoad(object sender, System.EventArgs e)
        {
            //	Initialize the status text control
            SetStatus(m_strStatus);

            this.Text = "Trimming ";
            if(m_strCaseFolder.Length > 0)
				this.Text += System.IO.Path.GetFileName(m_strCaseFolder);

            //	Automatically resize the columns to fit the column text
            SuspendLayout();
            m_ctrlErrors.Columns[1].Width = -2;
            m_ctrlErrors.Columns[2].Width = -2;
            ResumeLayout();
        }

        /// <summary>This method is called to save the errors to the specified file</summary>
        private void Save(string strFileSpec)
        {
            System.IO.StreamWriter fileStream = null;
            string strText = "";

            try
            {
                fileStream = System.IO.File.CreateText(strFileSpec);
            }
            catch (System.Exception Ex)
            {
                strText = ("Unable to open " + strFileSpec + " \n\nException:\n\n");
                strText += Ex.ToString();
                MessageBox.Show(strText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //	Write a header line
            strText = "Trim summary for ";
            strText += m_strCaseFolder;
            fileStream.WriteLine(strText);
            fileStream.WriteLine("------------------------------------------------------------------------------------------------------------------------------");

            foreach (ListViewItem O in m_ctrlErrors.Items)
            {
                strText = O.SubItems[1].Text;
                strText += "\t\t";
                strText += O.SubItems[2].Text;

                try
                {
                    fileStream.WriteLine(strText);
                }
                catch (System.Exception Ex)
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
        private void OnEndOperation()
        {
            //	Change the text on the cancel button
            if ((m_ctrlCancel != null) && (m_ctrlCancel.IsDisposed == false))
            {
                m_ctrlCancel.Text = "&Close";
            }

            //	Should we enable the Save As button?
            if ((m_ctrlSaveAs != null) && (m_ctrlSaveAs.IsDisposed == false))
            {
                if ((m_ctrlErrors != null) && (m_ctrlErrors.IsDisposed == false))
                {
                    if (m_ctrlErrors.Items.Count > 0)
                        m_ctrlSaveAs.Enabled = true;
                }

            }

            //	Display the summary information
            ShowSummary();

        }// private void OnEndOperation()

        /// <summary>Called to get the summary message</summary>
        /// <returns>The summary message to be displayed when the operation terminates</returns>
        private string GetSummaryMessage()
        {
            string strMsg = "";

            strMsg += String.Format("{0} critical errors\n", m_lErrors);
            strMsg += String.Format("{0} warnings", m_lWarnings);

            return strMsg;

        }// private string GetSummaryMessage()

        /// <summary>This method is called to show the summary information</summary>
        private void ShowSummary()
        {
            string strSummary = "";
            string strMsg = "";

            //	Don't bother if we've already shown the summary
            if(m_bShowSummary == true)
            {
				m_bShowSummary = false;
				
				strSummary = GetSummaryMessage();

				if((strSummary != null) && (strSummary.Length > 0))
				{
					strMsg = "Trim results: ";
					strMsg += "\n---------------------------------\n\n";
					strMsg += strSummary;

					MessageBox.Show(strMsg, "Summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}

			}// if(m_bShowSummary == true)

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
                OnEndOperation();
            }

        }

        #endregion Properties

    }// public class CFTrimProgress : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Forms
