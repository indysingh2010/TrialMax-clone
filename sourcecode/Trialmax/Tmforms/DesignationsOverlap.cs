using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FTI.Trialmax.Forms
{
    public partial class CFDesignationsOverlap : Form
    {
        #region Public Methods

        /// <summary>Constructor</summary>
        public CFDesignationsOverlap()
        {
            InitializeComponent();
        }// public CFDesignationsOverlap()

        /// <summary>Clears all the items from the ListView</summary>
        public void ClearDesignationsOverlapList()
        {
            m_ctrlOverlap.Clear();
        }// public void ClearDesignationsOverlapList()

        /// <summary>
        /// Appends the provided values into the overlap list view at the end of the list as a single row
        /// </summary>
        /// <param name="value"></param>
        public void AppendItemToList(string[] value)
        {
            value[0] = (m_ctrlOverlap.Items.Count + 1).ToString();
            m_ctrlOverlap.Items.Add(new ListViewItem(value));
        }// public void AppendItemToList(string[] value)

        /// <summary>
        /// Appends the provided list of values into the overlap list view at the end of the list as a rows
        /// </summary>
        /// <param name="values"></param>
        public void AppendItemsToList(List<string[]> values)
        {
            foreach (var value in values)
            {
                value[0] = (m_ctrlOverlap.Items.Count + 1).ToString();
                m_ctrlOverlap.Items.Add(new ListViewItem(value));
            }
        }// public void AppendItemsToList(List<string[]> values)

        #endregion

        #region Private Methods

        /// <summary>Return the number of rows that have been added to the overlap list view</summary>
        /// <returns>Total number of items in the overlap list view</returns>
        private int TotalItems()
        {
            return m_ctrlOverlap.Items.Count;
        }// private int TotalItems()

        /// <summary>This method is called when the user clicks on Close</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">The event arguments</param>
        private void m_ctrlClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }// private void m_ctrlClose_Click(object sender, EventArgs e)

        /// <summary>This method is called when the user clicks on Save</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">The event arguments</param>
        private void m_ctrlSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Normal text file|*.txt";
            saveFileDialog1.Title = "Save Designation Overlap Report";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                StreamWriter overlappingStatus = new StreamWriter(saveFileDialog1.FileName, false);
                for (int i = 0; i < TotalItems(); i++)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(m_ctrlOverlap.Items[i].SubItems[0].Text);
                    sb.Append("\tBarcode=\"" + m_ctrlOverlap.Items[i].SubItems[1].Text + "\"");
                    sb.Append("\tBarcode=\"" + m_ctrlOverlap.Items[i].SubItems[2].Text + "\"");
                    sb.Append("\tSegment=\"" + m_ctrlOverlap.Items[i].SubItems[3].Text + "\"");
                    sb.Append("\t\tPL=\"" + m_ctrlOverlap.Items[i].SubItems[4].Text + "\"");
                    sb.Append("\t\tPage=\"" + m_ctrlOverlap.Items[i].SubItems[5].Text + "\"");
                    sb.Append("\t\tLine=\"" + m_ctrlOverlap.Items[i].SubItems[6].Text + "\"");
                    sb.Append("\t\tQA=\"" + m_ctrlOverlap.Items[i].SubItems[7].Text + "\"");
                    sb.Append("\t\tStart=\"" + m_ctrlOverlap.Items[i].SubItems[8].Text + "\"");
                    sb.Append("\t\tStop=\"" + m_ctrlOverlap.Items[i].SubItems[9].Text + "\"");
                    sb.Append("\t\tText=\"" + m_ctrlOverlap.Items[i].SubItems[10].Text + "\"");
                    sb.AppendLine();
                    overlappingStatus.WriteLine(sb.ToString());
                }
                overlappingStatus.Close();
            }
            m_ctrlClose.PerformClick();
        }// private void m_ctrlSave_Click(object sender, EventArgs e)

        #endregion

    }
}
