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
	/// <summary>Form use to specify page/line position for Transcript page</summary>
	public class CFGoTranscript : System.Windows.Forms.Form
	{
		#region Private Members
		
		/// <summary>The form's Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>The form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOK;
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>Edit box used to enter the desired page</summary>
		private System.Windows.Forms.TextBox m_ctrlPage;
		
		/// <summary>Label for page entry edit box</summary>
		private System.Windows.Forms.Label m_ctrlPageLabel;
		
		/// <summary>Label for line entry edit box</summary>
		private System.Windows.Forms.Label m_ctrlLineLabel;
		
		/// <summary>Edit box used to enter the desired line</summary>
		private System.Windows.Forms.TextBox m_ctrlLine;

		/// <summary>Private member bound to FirstPL property</summary>
		private long m_lFirstPL = 0;

		/// <summary>Private member bound to LastPL property</summary>
		private long m_lLastPL = 0;

		/// <summary>Private member bound to LinesPerPage property</summary>
		private int m_iLinesPerPage = 0;

		/// <summary>Group box for deposition descriptor controls</summary>
		private System.Windows.Forms.GroupBox m_ctrlNameGroup;

		/// <summary>Static text control to display lines per page</summary>
		private System.Windows.Forms.Label m_ctrlLinesPerPage;

		/// <summary>Label for lines/page control</summary>
		private System.Windows.Forms.Label m_ctrlLinesPerPageLabel;

		/// <summary>Static text control to display page/line range</summary>
		private System.Windows.Forms.Label m_ctrlRange;

		/// <summary>Label for page/line range control</summary>
		private System.Windows.Forms.Label m_ctrlRangeLabel;

		/// <summary>Static text control to display deposition name</summary>
		private System.Windows.Forms.Label m_ctrlName;

		/// <summary>Label for deposition name control</summary>
		private System.Windows.Forms.Label m_ctrlNameLabel;
		
		/// <summary>Local member bound to GoPL property</summary>
		private long m_lGoPL = 0;
		
		/// <summary>Local member bound to Name property</summary>
		private string m_strTranscriptName = "";
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFGoTranscript()
		{
			//	Initialize the child controls
			InitializeComponent();
		}

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
		
		}// protected override void Dispose( bool disposing )
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFGoTranscript));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOK = new System.Windows.Forms.Button();
			this.m_ctrlPage = new System.Windows.Forms.TextBox();
			this.m_ctrlPageLabel = new System.Windows.Forms.Label();
			this.m_ctrlLineLabel = new System.Windows.Forms.Label();
			this.m_ctrlLine = new System.Windows.Forms.TextBox();
			this.m_ctrlNameGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlLinesPerPage = new System.Windows.Forms.Label();
			this.m_ctrlLinesPerPageLabel = new System.Windows.Forms.Label();
			this.m_ctrlRange = new System.Windows.Forms.Label();
			this.m_ctrlRangeLabel = new System.Windows.Forms.Label();
			this.m_ctrlName = new System.Windows.Forms.Label();
			this.m_ctrlNameLabel = new System.Windows.Forms.Label();
			this.m_ctrlNameGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.Location = new System.Drawing.Point(228, 144);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 3;
			this.m_ctrlCancel.Text = "&Cancel";
			// 
			// m_ctrlOK
			// 
			this.m_ctrlOK.Location = new System.Drawing.Point(144, 144);
			this.m_ctrlOK.Name = "m_ctrlOK";
			this.m_ctrlOK.TabIndex = 2;
			this.m_ctrlOK.Text = "&OK";
			this.m_ctrlOK.Click += new System.EventHandler(this.OnClickOK);
			// 
			// m_ctrlPage
			// 
			this.m_ctrlPage.Location = new System.Drawing.Point(104, 104);
			this.m_ctrlPage.Name = "m_ctrlPage";
			this.m_ctrlPage.Size = new System.Drawing.Size(68, 20);
			this.m_ctrlPage.TabIndex = 0;
			this.m_ctrlPage.Text = "";
			// 
			// m_ctrlPageLabel
			// 
			this.m_ctrlPageLabel.Location = new System.Drawing.Point(16, 108);
			this.m_ctrlPageLabel.Name = "m_ctrlPageLabel";
			this.m_ctrlPageLabel.Size = new System.Drawing.Size(84, 16);
			this.m_ctrlPageLabel.TabIndex = 11;
			this.m_ctrlPageLabel.Text = "Go To -> Page:";
			// 
			// m_ctrlLineLabel
			// 
			this.m_ctrlLineLabel.Location = new System.Drawing.Point(184, 108);
			this.m_ctrlLineLabel.Name = "m_ctrlLineLabel";
			this.m_ctrlLineLabel.Size = new System.Drawing.Size(40, 16);
			this.m_ctrlLineLabel.TabIndex = 13;
			this.m_ctrlLineLabel.Text = "Line:";
			// 
			// m_ctrlLine
			// 
			this.m_ctrlLine.Location = new System.Drawing.Point(232, 104);
			this.m_ctrlLine.Name = "m_ctrlLine";
			this.m_ctrlLine.Size = new System.Drawing.Size(68, 20);
			this.m_ctrlLine.TabIndex = 1;
			this.m_ctrlLine.Text = "";
			// 
			// m_ctrlNameGroup
			// 
			this.m_ctrlNameGroup.Controls.Add(this.m_ctrlLinesPerPage);
			this.m_ctrlNameGroup.Controls.Add(this.m_ctrlLinesPerPageLabel);
			this.m_ctrlNameGroup.Controls.Add(this.m_ctrlRange);
			this.m_ctrlNameGroup.Controls.Add(this.m_ctrlRangeLabel);
			this.m_ctrlNameGroup.Controls.Add(this.m_ctrlName);
			this.m_ctrlNameGroup.Controls.Add(this.m_ctrlNameLabel);
			this.m_ctrlNameGroup.Location = new System.Drawing.Point(8, 4);
			this.m_ctrlNameGroup.Name = "m_ctrlNameGroup";
			this.m_ctrlNameGroup.Size = new System.Drawing.Size(300, 92);
			this.m_ctrlNameGroup.TabIndex = 14;
			this.m_ctrlNameGroup.TabStop = false;
			this.m_ctrlNameGroup.Text = "Deposition";
			// 
			// m_ctrlLinesPerPage
			// 
			this.m_ctrlLinesPerPage.Location = new System.Drawing.Point(96, 64);
			this.m_ctrlLinesPerPage.Name = "m_ctrlLinesPerPage";
			this.m_ctrlLinesPerPage.Size = new System.Drawing.Size(196, 16);
			this.m_ctrlLinesPerPage.TabIndex = 25;
			// 
			// m_ctrlLinesPerPageLabel
			// 
			this.m_ctrlLinesPerPageLabel.Location = new System.Drawing.Point(8, 64);
			this.m_ctrlLinesPerPageLabel.Name = "m_ctrlLinesPerPageLabel";
			this.m_ctrlLinesPerPageLabel.Size = new System.Drawing.Size(84, 16);
			this.m_ctrlLinesPerPageLabel.TabIndex = 24;
			this.m_ctrlLinesPerPageLabel.Text = "Lines per Page:";
			// 
			// m_ctrlRange
			// 
			this.m_ctrlRange.Location = new System.Drawing.Point(96, 44);
			this.m_ctrlRange.Name = "m_ctrlRange";
			this.m_ctrlRange.Size = new System.Drawing.Size(196, 16);
			this.m_ctrlRange.TabIndex = 23;
			// 
			// m_ctrlRangeLabel
			// 
			this.m_ctrlRangeLabel.Location = new System.Drawing.Point(8, 44);
			this.m_ctrlRangeLabel.Name = "m_ctrlRangeLabel";
			this.m_ctrlRangeLabel.Size = new System.Drawing.Size(84, 16);
			this.m_ctrlRangeLabel.TabIndex = 22;
			this.m_ctrlRangeLabel.Text = "Range:";
			// 
			// m_ctrlName
			// 
			this.m_ctrlName.Location = new System.Drawing.Point(96, 24);
			this.m_ctrlName.Name = "m_ctrlName";
			this.m_ctrlName.Size = new System.Drawing.Size(196, 16);
			this.m_ctrlName.TabIndex = 21;
			// 
			// m_ctrlNameLabel
			// 
			this.m_ctrlNameLabel.Location = new System.Drawing.Point(8, 24);
			this.m_ctrlNameLabel.Name = "m_ctrlNameLabel";
			this.m_ctrlNameLabel.Size = new System.Drawing.Size(84, 16);
			this.m_ctrlNameLabel.TabIndex = 20;
			this.m_ctrlNameLabel.Text = "Name:";
			// 
			// CFGoTranscript
			// 
			this.AcceptButton = this.m_ctrlOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(314, 175);
			this.Controls.Add(this.m_ctrlNameGroup);
			this.Controls.Add(this.m_ctrlLineLabel);
			this.Controls.Add(this.m_ctrlLine);
			this.Controls.Add(this.m_ctrlPageLabel);
			this.Controls.Add(this.m_ctrlPage);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFGoTranscript";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Go To";
			this.m_ctrlNameGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		
		/// <summary>This method is called when the form is being loaded</summary>
		/// <param name="e">The event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			m_ctrlName.Text = m_strTranscriptName;
			m_ctrlRange.Text = CTmaxToolbox.PLToString(m_lFirstPL) + " - " + CTmaxToolbox.PLToString(m_lLastPL);
			m_ctrlLinesPerPage.Text = m_iLinesPerPage.ToString();
			
			base.OnLoad(e);
		
		}// protected override void OnLoad(EventArgs e)

		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOK(object sender, System.EventArgs e)
		{
			long	lPage = 0;
			int		iLine = 0;
			
			//	Get the desired page
			if(m_ctrlPage.Text.Length > 0)
			{
				try {	lPage = System.Convert.ToInt64(m_ctrlPage.Text); }
				catch { lPage = 0; }
			}
			
			//	Did the caller provide a valid page number
			if(lPage <= 0)
			{
				MessageBox.Show("You must supply a valid page number", "Error",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				m_ctrlPage.Focus();
				return;
			}
			
			//	Get the desired line
			if(m_ctrlLine.Text.Length > 0)
			{
				try {	iLine = System.Convert.ToInt32(m_ctrlLine.Text); }
				catch { iLine = 0; }
			}
			else
			{
				//	Default to line 1 if no value supplied
				m_ctrlLine.Text = "1";
				iLine = 1;
			}
			
			//	Did the caller provide a valid line number
			if(iLine <= 0)
			{
				MessageBox.Show("You must supply a valid line number", "Error",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				m_ctrlLine.Focus();
				return;
			}
			
			//	Is the line number within the allowable range
			if((m_iLinesPerPage > 0) && (iLine > m_iLinesPerPage))
			{
				MessageBox.Show("The specified line number is outside the range allowed for this transcript", "Error",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				m_ctrlLine.Focus();
				return;
			}
			
			//	Get the composite page/line value
			m_lGoPL = CTmaxToolbox.GetPL(lPage, iLine);
			
			if((m_lGoPL < m_lFirstPL) || (m_lGoPL > m_lLastPL))
			{
				MessageBox.Show(CTmaxToolbox.PLToString(m_lGoPL) + " is not within the range allowed for this transcript", "Error",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				m_ctrlPage.Focus();
				return;
			}
			
			this.DialogResult = DialogResult.OK;
			this.Close();
			
		}// private void OnClickOK(object sender, System.EventArgs e)
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>The first line in the transcript</summary>
		public long FirstPL
		{
			get { return m_lFirstPL;}
			set { m_lFirstPL = value; }
		}
		
		/// <summary>The last line in the transcript</summary>
		public long LastPL
		{
			get { return m_lLastPL;}
			set { m_lLastPL = value; }
		}
		
		/// <summary>The composite PL value that represents the desired position</summary>
		public long GoPL
		{
			get { return m_lGoPL;}
			set { m_lGoPL = value; }
		}
		
		/// <summary>The number of lines per page in the transcript</summary>
		public int LinesPerPage
		{
			get { return m_iLinesPerPage;}
			set { m_iLinesPerPage = value; }
		}
		
		/// <summary>The name assigned to the deposition</summary>
		public string TranscriptName
		{
			get { return m_strTranscriptName;}
			set { m_strTranscriptName = value; }
		}
		
		#endregion Properties
	
	}// public class CFGoTranscript : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Forms
