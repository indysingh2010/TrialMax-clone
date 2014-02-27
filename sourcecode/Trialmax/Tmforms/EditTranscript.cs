using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace FTI.Trialmax.Forms
{
	/// <summary>Form class used to edit transcript text</summary>
	public class CFEditTranscript : System.Windows.Forms.Form
	{
		#region Private Members
		
		/// <summary>OK button</summary>
		private System.Windows.Forms.Button m_ctrlOK;
		
		/// <summary>Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		private System.ComponentModel.IContainer components;
		
		/// <summary>Grid control used to edit lines</summary>
		private Infragistics.Win.UltraWinGrid.UltraGrid m_ctrlLines;
		
		/// <summary>Local member bound to Transcripts property</summary>
		private CXmlTranscripts m_xmlTranscripts = null;
		
		/// <summary>Local collection to store editable lines</summary>
		private ArrayList m_aLines = new ArrayList();
		private System.Windows.Forms.ImageList m_ctrlImages;
		
		/// <summary>Local member bound to Modified property</summary>
		private bool m_bModified = false;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CFEditTranscript()
		{
			//	Initialize the child controls
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
				
				if(m_aLines != null)
				{
					m_aLines.Clear();
					m_aLines = null;
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFEditTranscript));
			this.m_ctrlOK = new System.Windows.Forms.Button();
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlLines = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlLines)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlOK
			// 
			this.m_ctrlOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOK.Location = new System.Drawing.Point(260, 260);
			this.m_ctrlOK.Name = "m_ctrlOK";
			this.m_ctrlOK.TabIndex = 1;
			this.m_ctrlOK.Text = "&OK";
			this.m_ctrlOK.Click += new System.EventHandler(this.OnOK);
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.Location = new System.Drawing.Point(344, 260);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 2;
			this.m_ctrlCancel.Text = "&Cancel";
			// 
			// m_ctrlLines
			// 
			this.m_ctrlLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			appearance1.BackColor = System.Drawing.Color.White;
			this.m_ctrlLines.DisplayLayout.Appearance = appearance1;
			ultraGridBand1.ColHeadersVisible = false;
			ultraGridBand1.GroupHeadersVisible = false;
			ultraGridBand1.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
			ultraGridBand1.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
			ultraGridBand1.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.None;
			ultraGridBand1.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
			ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
			ultraGridBand1.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
			ultraGridBand1.Override.AllowGroupMoving = Infragistics.Win.UltraWinGrid.AllowGroupMoving.NotAllowed;
			ultraGridBand1.Override.AllowGroupSwapping = Infragistics.Win.UltraWinGrid.AllowGroupSwapping.NotAllowed;
			ultraGridBand1.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
			ultraGridBand1.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None;
			ultraGridBand1.Override.CellPadding = 0;
			ultraGridBand1.Override.CellSpacing = 0;
			ultraGridBand1.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlLines.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
			this.m_ctrlLines.DisplayLayout.InterBandSpacing = 0;
			this.m_ctrlLines.DisplayLayout.MaxColScrollRegions = 1;
			this.m_ctrlLines.DisplayLayout.MaxRowScrollRegions = 1;
			this.m_ctrlLines.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None;
			this.m_ctrlLines.DisplayLayout.Override.BorderStyleSummaryValue = Infragistics.Win.UIElementBorderStyle.None;
			this.m_ctrlLines.DisplayLayout.Override.NullText = "";
			this.m_ctrlLines.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.m_ctrlLines.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
			this.m_ctrlLines.ImageList = this.m_ctrlImages;
			this.m_ctrlLines.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlLines.Name = "m_ctrlLines";
			this.m_ctrlLines.Size = new System.Drawing.Size(420, 244);
			this.m_ctrlLines.TabIndex = 0;
			this.m_ctrlLines.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnUpdate;
			// 
			// m_ctrlImages
			// 
			this.m_ctrlImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.m_ctrlImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
			this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// CFEditTranscript
			// 
			this.AcceptButton = this.m_ctrlOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(436, 289);
			this.Controls.Add(this.m_ctrlLines);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFEditTranscript";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Transcript Editor";
			this.Load += new System.EventHandler(this.OnLoad);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlLines)).EndInit();
			this.ResumeLayout(false);

		}
		
		/// <summary>This method will transfer text from the grid to the transcript elements</summary>
		protected void TransferText()
		{
			string strText = "";
			
			//	Check each row in the grid
			for(int i = 0; i < m_ctrlLines.Rows.Count; i++)
			{
				//	Has the text been modified?
				if(m_ctrlLines.Rows[i].Cells["Text"].DataChanged == true)
				{
					strText = m_ctrlLines.Rows[i].Cells["Text"].Text;
					
					//	This is just in case the user changes it and then puts it back
					if(String.Compare(m_xmlTranscripts[i].Text, strText, false) != 0)
					{
						//	Update the transcript text and mark it as having been edited
						m_xmlTranscripts[i].Text = strText;
						m_xmlTranscripts[i].Edited = true;
						
						//	Set the local flag so that the caller knows something has been
						//	modified
						m_bModified = true;
					}

				}// if(m_ctrlLines.Rows[i].Cells["Text"].DataChanged == true)
				
			}// for(int i = 0; i < m_ctrlLines.Rows.Count; i++)
			
		}// protected void TransferText()
		
		/// <summary>This method handles the form's Load event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		protected void OnLoad(object sender, System.EventArgs e)
		{
			CLine	line;
			long	lPrevPage = -1;
			long	lPage = 0;
			int		iLine = 0;
			
			//	Fill the list box
			if((m_xmlTranscripts != null) && (m_xmlTranscripts.Count > 0))
			{
				foreach(CXmlTranscript O in m_xmlTranscripts)
				{
					line = new CLine();
					
					lPage = CTmaxToolbox.PLToPage(O.PL);
					iLine = CTmaxToolbox.PLToLine(O.PL);
					
					if(lPage != lPrevPage)
					{
						lPrevPage = lPage;
						line.SetPage(lPage.ToString());
					}
					else
					{
						line.SetPage("");
					}
					
					line.SetLine(iLine.ToString());
					line.Text = O.Text;
					
					m_aLines.Add(line);
				}
				
				m_ctrlLines.DataSource = m_aLines;
				
				PerformGridLayout();
				
				//	Set the edited indicators
				for(int i = 0; i < m_ctrlLines.Rows.Count; i++)
				{
					if(m_xmlTranscripts[i].Edited == true)
						m_ctrlLines.Rows[i].Cells["E"].Appearance.Image = 0;
					//else
						//m_ctrlLines.Rows[i].Cells[0].Appearance.Image = -1;
				}
				
			}
		
		}// protected void OnLoad(object sender, System.EventArgs e)
		
		/// <summary>This method is perform the grid layout after it's been populated</summary>
		protected void PerformGridLayout()
		{
			try
			{
				UltraGridColumn colEdited = m_ctrlLines.DisplayLayout.Bands[0].Columns["E"];
				UltraGridColumn colPage = m_ctrlLines.DisplayLayout.Bands[0].Columns["P"];
				UltraGridColumn colLine = m_ctrlLines.DisplayLayout.Bands[0].Columns["L"];
				UltraGridColumn colText = m_ctrlLines.DisplayLayout.Bands[0].Columns["Text"];
		
				//	Set the column order
				colEdited.Header.VisiblePosition = 0;
				colPage.Header.VisiblePosition = 1;
				colLine.Header.VisiblePosition = 2;
				colText.Header.VisiblePosition = 3;

				colText.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
				
				colEdited.TabStop = false;
				colPage.TabStop = false;
				colLine.TabStop = false;
				
				colEdited.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
				colPage.CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
				colLine.CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;

				colEdited.CellAppearance.ForeColorDisabled = System.Drawing.Color.Black;
				colPage.CellAppearance.ForeColorDisabled = System.Drawing.Color.Black;
				colLine.CellAppearance.ForeColorDisabled = System.Drawing.Color.Black;
			
				//	Size the variable columns to match the text they contain
				colEdited.Width = 16;
				colPage.PerformAutoResize();
				colLine.PerformAutoResize();
				colText.PerformAutoResize();

			}
			catch
			{
			}
			
		}// protected void PerformGridLayout()
		
		/// <summary>This method handles the event fired when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		protected void OnOK(object sender, System.EventArgs e)
		{
			//	Update the caller's text with the grid text
			TransferText();
			
			DialogResult = DialogResult.OK;
			this.Close();
		
		}// protected void OnOK(object sender, System.EventArgs e)

		#endregion Protected Methods

		#region Properties
		
		public CXmlTranscripts Transcripts
		{
			get { return m_xmlTranscripts; }
			set { m_xmlTranscripts = value; }
		}
		
		public bool Modified
		{
			get { return m_bModified; }
		}
		
		#endregion Properties
		
		protected class CLine
		{
			#region Private Members
			
			private string m_strLine = "";
			
			private string m_strPage = "";
			
			private string m_strText = "";
			
			private string m_strEdited = "";
			
			#endregion Private Members
			
			#region Public Methods
			
			public CLine()
			{
			}
			
			public void SetPage(string strPage) { m_strPage = strPage; }
			
			public void SetLine(string strLine) { m_strLine = strLine; }
			

			#endregion Public Methods
			
			#region Properties
			
			public string P
			{
				get { return m_strPage; }
			}
			
			public string L
			{
				get { return m_strLine; }
			}
			
			public string E
			{
				get { return m_strEdited; }
			}
			
			public string Text
			{ 
				get { return m_strText; }
				set { m_strText = value; }
				
			}
			
			#endregion Properties
			
		}// protected class CLine
		
	}// public class CFEditTranscript : System.Windows.Forms.Form
	
}// namespace FTI.Trialmax.Forms
