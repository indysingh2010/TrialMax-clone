using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Controls;

namespace FTI.Trialmax.Forms
{
	/// <summary>Generic form used to reorder an arraylist of objects</summary>
	public class CFFastFilter : CFTmaxBaseForm
	{
		#region Private Members

		/// <summary>Form designer component container</summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>Form's Cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;

		/// <summary>Form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;

		/// <summary>Static text label for code name</summary>
		private System.Windows.Forms.Label m_ctrlCodeNameLabel;

		/// <summary>Static text control to display name of CaseCode</summary>
		private System.Windows.Forms.Label m_ctrlCodeName;

		/// <summary>Static text label for search text edit box</summary>
		private System.Windows.Forms.Label m_ctrlTmaxEditorLabel;

		/// <summary>Local member bound to CaseCode property</summary>
		private FTI.Shared.Trialmax.CTmaxCaseCode m_tmaxCaseCode = null;
		
		/// <summary>Local member bound to PickList property</summary>
		private FTI.Shared.Trialmax.CTmaxPickItem m_tmaxPickList = null;
		
		/// <summary>Local member bound to SearchText property</summary>
		private string m_strSearchText = "";
		
		/// <summary>Pushbutton to invoke the Advanced Filter Builder</summary>
		private System.Windows.Forms.Button m_ctrlAdvanced;
		
		/// <summary>Custom TrialMax editor control</summary>
		private FTI.Trialmax.Controls.CTmaxEditorCtrl m_ctrlTmaxEditor;
		
		/// <summary>Control to allow the user to set the search fields</summary>
		private System.Windows.Forms.ComboBox m_ctrlSearchFields;
		
		/// <summary>Local member bound to GoAdvanced property</summary>
		private bool m_bGoAdvanced = false;
		
		/// <summary>Search field option when no case code specified</summary>
		private TmaxFastFilterFields m_eSearchFields = TmaxFastFilterFields.All;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFFastFilter() : base()
		{
			// Required for Windows Form Designer support
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
		}

		/// <summary>Called when the form gets displayed</summary>
		/// <param name="e">Load event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			Exchange(false);
			
			base.OnLoad(e);
		}

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFFastFilter));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlCodeNameLabel = new System.Windows.Forms.Label();
			this.m_ctrlCodeName = new System.Windows.Forms.Label();
			this.m_ctrlTmaxEditorLabel = new System.Windows.Forms.Label();
			this.m_ctrlAdvanced = new System.Windows.Forms.Button();
			this.m_ctrlTmaxEditor = new FTI.Trialmax.Controls.CTmaxEditorCtrl();
			this.m_ctrlSearchFields = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(288, 72);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 4;
			this.m_ctrlCancel.Text = "  &Cancel";
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(208, 72);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 3;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOK);
			// 
			// m_ctrlCodeNameLabel
			// 
			this.m_ctrlCodeNameLabel.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlCodeNameLabel.Name = "m_ctrlCodeNameLabel";
			this.m_ctrlCodeNameLabel.Size = new System.Drawing.Size(80, 20);
			this.m_ctrlCodeNameLabel.TabIndex = 5;
			this.m_ctrlCodeNameLabel.Text = "Field Name:";
			this.m_ctrlCodeNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlCodeName
			// 
			this.m_ctrlCodeName.Location = new System.Drawing.Point(96, 8);
			this.m_ctrlCodeName.Name = "m_ctrlCodeName";
			this.m_ctrlCodeName.Size = new System.Drawing.Size(264, 20);
			this.m_ctrlCodeName.TabIndex = 6;
			this.m_ctrlCodeName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlTmaxEditorLabel
			// 
			this.m_ctrlTmaxEditorLabel.Location = new System.Drawing.Point(8, 40);
			this.m_ctrlTmaxEditorLabel.Name = "m_ctrlTmaxEditorLabel";
			this.m_ctrlTmaxEditorLabel.Size = new System.Drawing.Size(80, 20);
			this.m_ctrlTmaxEditorLabel.TabIndex = 7;
			this.m_ctrlTmaxEditorLabel.Text = "Search For:";
			this.m_ctrlTmaxEditorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlAdvanced
			// 
			this.m_ctrlAdvanced.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlAdvanced.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlAdvanced.Location = new System.Drawing.Point(96, 72);
			this.m_ctrlAdvanced.Name = "m_ctrlAdvanced";
			this.m_ctrlAdvanced.TabIndex = 2;
			this.m_ctrlAdvanced.Text = "&Advanced";
			this.m_ctrlAdvanced.Click += new System.EventHandler(this.OnClickAdvanced);
			// 
			// m_ctrlTmaxEditor
			// 
			this.m_ctrlTmaxEditor.DropListValues = null;
			this.m_ctrlTmaxEditor.FalseText = "False";
			this.m_ctrlTmaxEditor.Location = new System.Drawing.Point(96, 40);
			this.m_ctrlTmaxEditor.MaxTextLength = 255;
			this.m_ctrlTmaxEditor.MemoAsText = true;
			this.m_ctrlTmaxEditor.MultiLevel = null;
			this.m_ctrlTmaxEditor.MultiLevelSelection = null;
			this.m_ctrlTmaxEditor.Name = "m_ctrlTmaxEditor";
			this.m_ctrlTmaxEditor.PaneId = 0;
			this.m_ctrlTmaxEditor.Size = new System.Drawing.Size(264, 24);
			this.m_ctrlTmaxEditor.TabIndex = 0;
			this.m_ctrlTmaxEditor.TrueText = "True";
			this.m_ctrlTmaxEditor.Type = FTI.Trialmax.Controls.TmaxEditorCtrlTypes.Text;
			this.m_ctrlTmaxEditor.UserAdditions = false;
			this.m_ctrlTmaxEditor.Value = "";
			// 
			// m_ctrlSearchFields
			// 
			this.m_ctrlSearchFields.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_ctrlSearchFields.Location = new System.Drawing.Point(8, 72);
			this.m_ctrlSearchFields.Name = "m_ctrlSearchFields";
			this.m_ctrlSearchFields.Size = new System.Drawing.Size(72, 21);
			this.m_ctrlSearchFields.TabIndex = 1;
			// 
			// CFFastFilter
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(370, 101);
			this.Controls.Add(this.m_ctrlSearchFields);
			this.Controls.Add(this.m_ctrlTmaxEditor);
			this.Controls.Add(this.m_ctrlAdvanced);
			this.Controls.Add(this.m_ctrlTmaxEditorLabel);
			this.Controls.Add(this.m_ctrlCodeName);
			this.Controls.Add(this.m_ctrlCodeNameLabel);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFFastFilter";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Fast Filter";
			this.ResumeLayout(false);

		}
		
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOK(object sender, System.EventArgs e)
		{
			if(Exchange(true) == true)
			{
				if(m_strSearchText.Length > 0)
				{
					DialogResult = DialogResult.OK;
					this.Close();
				}
				else
				{
					Warn("You must supply text for this operation", m_ctrlTmaxEditor);
				}

			}
			
		}// private void OnClickOK(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on Advanced</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickAdvanced(object sender, System.EventArgs e)
		{
			//	Set the flag to indicate that the user wants to switch to the advanced form
			m_bGoAdvanced = true;
			DialogResult = DialogResult.OK;
			this.Close();
			
		}// private void OnClickAdvanced(object sender, System.EventArgs e)

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
					m_strSearchText = m_ctrlTmaxEditor.Value;
					
					//	Is this a multi-level case code?
					if((m_tmaxCaseCode != null) && (m_tmaxCaseCode.IsMultiLevel == true))
					{
						if(m_ctrlTmaxEditor.MultiLevelSelection != null)
						{
							this.PickList = m_ctrlTmaxEditor.MultiLevelSelection.Parent;
							m_strSearchText = m_ctrlTmaxEditor.MultiLevelSelection.Name; // just in case...
						}
						
					}// if((m_tmaxCaseCode != null) && (m_tmaxCaseCode.IsMultiLevel == true))

					//	Are we using the search fields list?
					if((m_ctrlSearchFields.Visible == true) && (m_ctrlSearchFields.SelectedItem != null))
					{
						m_eSearchFields = (TmaxFastFilterFields)(m_ctrlSearchFields.SelectedItem);
					}
				}
				else
				{
					if(m_tmaxCaseCode != null)
					{
						m_ctrlSearchFields.Visible = false;
						
						m_ctrlCodeName.Visible = true;
						m_ctrlCodeName.Text = m_tmaxCaseCode.Name;
						
						m_ctrlTmaxEditor.SetType(m_tmaxCaseCode);
					
						if(m_tmaxCaseCode.Type == TmaxCodeTypes.PickList)
						{
							if(m_tmaxCaseCode.IsMultiLevel == true)
							{
								m_ctrlTmaxEditor.MultiLevel = m_tmaxCaseCode.PickList;
								if((this.PickList != null) && (this.SearchText.Length > 0))
									m_ctrlTmaxEditor.MultiLevelSelection = this.PickList.FindChild(this.SearchText);
							}
							else
							{
								if(m_tmaxCaseCode.PickList != null)
									m_ctrlTmaxEditor.SetDropListValues(m_tmaxCaseCode.PickList.Children);
							}
						
						}// if(m_tmaxCaseCode.Type == TmaxCodeTypes.PickList)
						
					}
					else
					{
						m_ctrlSearchFields.Visible = true;
						m_ctrlSearchFields.Location = m_ctrlCodeName.Location;
						m_ctrlSearchFields.Size = m_ctrlCodeName.Size;
						FillSearchFields();
						
						m_ctrlCodeName.Visible = false;
						m_ctrlCodeName.Text = "";
						m_ctrlCodeNameLabel.Text = "Search Fields:";

						m_ctrlTmaxEditor.Type = TmaxEditorCtrlTypes.Text;
					
					}// if(m_tmaxCaseCode != null)
					
					m_ctrlTmaxEditor.Value = m_strSearchText;
					m_ctrlTmaxEditor.Focus();

				}// if(bSetMembers == true)
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Exchange", Ex);
			}
			
			return bSuccessful; 
			
		}// private bool Exchange(bool bSetMembers)
		
		/// <summary>This method is called to fill the search fields combobox</summary>
		private void FillSearchFields()
		{
			try
			{
				int			iIndex = -1;
				Array		aValues = Enum.GetValues(typeof(TmaxFastFilterFields));
				
				foreach(object O in aValues)
				{
					m_ctrlSearchFields.Items.Add(O);
				}

				//	Set the initial selection
				iIndex = m_ctrlSearchFields.FindStringExact(m_eSearchFields.ToString());
					
				if(iIndex >= 0)
					m_ctrlSearchFields.SelectedIndex = iIndex;
				else
					m_ctrlSearchFields.SelectedIndex = 0;
				
			}
			catch
			{
			}

		}// private void FillSearchFields()
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>The case code used to define the filter</summary>
		public CTmaxCaseCode CaseCode
		{
			get { return m_tmaxCaseCode; }
			set { m_tmaxCaseCode = value; }
		}
		
		/// <summary>The pick list used to define the filter</summary>
		public CTmaxPickItem PickList
		{
			get { return m_tmaxPickList; }
			set { m_tmaxPickList = value; }
		}
		
		/// <summary>Text used to run the filter query</summary>
		public string SearchText
		{
			get { return m_strSearchText; }
			set { m_strSearchText = value; }
		}
		
		/// <summary>Fields used for the operation if no case code specified</summary>
		public TmaxFastFilterFields SearchFields
		{
			get { return m_eSearchFields; }
			set { m_eSearchFields = value; }
		}
		
		/// <summary>True if the user wants to switch to Advanced filtering</summary>
		public bool GoAdvanced
		{
			get { return m_bGoAdvanced; }
		}
		
		#endregion Properties
		
	}// public class CFFastFilter : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
