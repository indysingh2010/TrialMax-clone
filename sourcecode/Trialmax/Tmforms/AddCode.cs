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
	/// <summary>This form allows the user to add a new code to a media record</summary>
	public class CFAddCode : CFTmaxBaseForm
	{
		#region Private Members
		
		/// <summary>Required designer variable</summary>
		private System.ComponentModel.Container components = null;
		
		/// <summary>Local member bound to CaseCodes property</summary>
		private CTmaxCaseCodes m_tmaxCaseCodes = null;
		
		/// <summary>Local member bound to CaseCode property</summary>
		private CTmaxCaseCode m_tmaxCaseCode = null;
		
		/// <summary>Local member bound to MultiLevelSelection property</summary>
		private CTmaxPickItem m_tmaxMultiLevelSelection = null;
		
		/// <summary>Local member bound to Value property</summary>
		private string m_strValue = "";
		
		/// <summary>Local member bound to AddValue property</summary>
		private bool m_bAddValue = false;
		
		/// <summary>The form's cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>The form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;

		/// <summary>The label for the Name edit box</summary>
		private System.Windows.Forms.Label m_ctrlValueLabel;

		/// <summary>The list view control used to display the case codes</summary>
		private FTI.Trialmax.Controls.CTmaxListViewCtrl m_ctrlCaseCodes;

		/// <summary>Static text label for case codes list box</summary>
		private System.Windows.Forms.Label m_ctrlCaseCodesLabel;

		/// <summary>Custom TrialMax editor for supported data types</summary>
		private FTI.Trialmax.Controls.CTmaxEditorCtrl m_ctrlValue;
		
		#endregion Private Members
		
		#region Public Methods
		
		public CFAddCode()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
			m_tmaxEventSource.Name = "Add Code";
			m_tmaxEventSource.Attach(m_ctrlValue.EventSource);
			
		}// public CFAddCode()

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
		
		/// <summary>This method handles the form's Load event</summary>
		/// <param name="e">The event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			if((m_tmaxCaseCodes == null) || (m_tmaxCaseCodes.Count == 0))
			{
				m_ctrlOk.Enabled = false;
				m_ctrlValueLabel.Enabled = false;
				m_ctrlValue.Enabled = false;
			}
			else
			{
				m_ctrlCaseCodes.Initialize(new CTmaxCaseCode());
				m_ctrlCaseCodes.Add(this.CaseCodes, true);
				
				//	Set the initial selection in the case codes list
				if(m_tmaxCaseCode != null)
					m_ctrlCaseCodes.SetSelected(m_tmaxCaseCode, true);
				else
					m_ctrlCaseCodes.SetSelectedIndex(0, true);
				OnCaseCodeChanged(m_ctrlCaseCodes, System.EventArgs.Empty);
				
				//	Initialize the value
				m_ctrlValue.Value = m_strValue;
				m_ctrlValue.MaxTextLength = CTmaxCaseCodes.CASE_CODES_MAX_TEXT_LENGTH;
				
			}// if((m_tmaxCaseCodes == null) || (m_tmaxCaseCode == null) || (m_tmaxCaseCode.CasePaths == null))
		
			base.OnLoad(e);
			
		}// protected void OnLoad(object sender, System.EventArgs e)
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>This method handles events fired when the user selects a new case code</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnCaseCodeChanged(object sender, System.EventArgs e)
		{
			try { m_tmaxCaseCode = (CTmaxCaseCode)(m_ctrlCaseCodes.GetSelected()); }
			catch { m_tmaxCaseCode = null; }

			m_ctrlOk.Enabled = (m_tmaxCaseCode != null);
			m_ctrlValueLabel.Enabled = (m_tmaxCaseCode != null);
			m_ctrlValue.Enabled = (m_tmaxCaseCode != null);
			SetValueType();
			
		}// private void OnCaseCodeChanged(object sender, System.EventArgs e)

		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFAddCode));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlValueLabel = new System.Windows.Forms.Label();
			this.m_ctrlCaseCodes = new FTI.Trialmax.Controls.CTmaxListViewCtrl();
			this.m_ctrlCaseCodesLabel = new System.Windows.Forms.Label();
			this.m_ctrlValue = new FTI.Trialmax.Controls.CTmaxEditorCtrl();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(196, 240);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 3;
			this.m_ctrlCancel.Text = "  &Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(112, 240);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 2;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlValueLabel
			// 
			this.m_ctrlValueLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlValueLabel.Location = new System.Drawing.Point(8, 152);
			this.m_ctrlValueLabel.Name = "m_ctrlValueLabel";
			this.m_ctrlValueLabel.Size = new System.Drawing.Size(48, 20);
			this.m_ctrlValueLabel.TabIndex = 4;
			this.m_ctrlValueLabel.Text = "Value :";
			this.m_ctrlValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlCaseCodes
			// 
			this.m_ctrlCaseCodes.AddTop = false;
			this.m_ctrlCaseCodes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCaseCodes.AutoResizeColumns = true;
			this.m_ctrlCaseCodes.ClearOnDblClick = false;
			this.m_ctrlCaseCodes.DisplayMode = 0;
			this.m_ctrlCaseCodes.HideSelection = false;
			this.m_ctrlCaseCodes.Location = new System.Drawing.Point(8, 24);
			this.m_ctrlCaseCodes.MaxRows = 0;
			this.m_ctrlCaseCodes.Name = "m_ctrlCaseCodes";
			this.m_ctrlCaseCodes.OwnerImages = null;
			this.m_ctrlCaseCodes.PaneId = 0;
			this.m_ctrlCaseCodes.SelectedIndex = -1;
			this.m_ctrlCaseCodes.ShowHeaders = true;
			this.m_ctrlCaseCodes.ShowImage = false;
			this.m_ctrlCaseCodes.Size = new System.Drawing.Size(268, 120);
			this.m_ctrlCaseCodes.TabIndex = 0;
			this.m_ctrlCaseCodes.SelectedIndexChanged += new System.EventHandler(this.OnCaseCodeChanged);
			// 
			// m_ctrlCaseCodesLabel
			// 
			this.m_ctrlCaseCodesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCaseCodesLabel.Location = new System.Drawing.Point(12, 8);
			this.m_ctrlCaseCodesLabel.Name = "m_ctrlCaseCodesLabel";
			this.m_ctrlCaseCodesLabel.Size = new System.Drawing.Size(256, 16);
			this.m_ctrlCaseCodesLabel.TabIndex = 6;
			this.m_ctrlCaseCodesLabel.Text = "Select A Case Code :";
			// 
			// m_ctrlValue
			// 
			this.m_ctrlValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlValue.DropListValues = null;
			this.m_ctrlValue.FalseText = "False";
			this.m_ctrlValue.Location = new System.Drawing.Point(60, 152);
			this.m_ctrlValue.MaxTextLength = 255;
			this.m_ctrlValue.MemoAsText = false;
			this.m_ctrlValue.MultiLevel = null;
			this.m_ctrlValue.MultiLevelSelection = null;
			this.m_ctrlValue.Name = "m_ctrlValue";
			this.m_ctrlValue.PaneId = 0;
			this.m_ctrlValue.Size = new System.Drawing.Size(216, 84);
			this.m_ctrlValue.TabIndex = 7;
			this.m_ctrlValue.TrueText = "True";
			this.m_ctrlValue.Type = FTI.Trialmax.Controls.TmaxEditorCtrlTypes.Text;
			this.m_ctrlValue.UserAdditions = false;
			this.m_ctrlValue.Value = "";
			// 
			// CFAddCode
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(284, 269);
			this.Controls.Add(this.m_ctrlValue);
			this.Controls.Add(this.m_ctrlCaseCodesLabel);
			this.Controls.Add(this.m_ctrlCaseCodes);
			this.Controls.Add(this.m_ctrlValueLabel);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFAddCode";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Add Code";
			this.ResumeLayout(false);

		}
	
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOk(object sender, System.EventArgs e)
		{
			CTmaxPickItem	tmaxValue = null;
			CTmaxPickItem	tmaxPickList = null;
			bool			bSuccessful = false;
			
			Debug.Assert(m_tmaxCaseCodes != null);

			if((m_tmaxCaseCodes == null) || (m_tmaxCaseCodes.Count == 0))
			{
				DialogResult = DialogResult.Cancel;
				this.Close();
				return;
			}

			//	Clear this flag before resolving the user values
			m_bAddValue = false;
			
			while(bSuccessful == false)
			{
				//	Did the user select a case code?
				m_tmaxCaseCode = (CTmaxCaseCode)(m_ctrlCaseCodes.GetSelected());
				if(m_tmaxCaseCode == null)
				{
					Warn("You must select one of the available case codes");
					m_ctrlCaseCodes.Focus();
					break;
				}
			
				//	Get the value specified by the user
				if(m_tmaxCaseCode.IsMultiLevel == true)
				{
					if((m_tmaxMultiLevelSelection = m_ctrlValue.MultiLevelSelection) != null)
						m_strValue = m_tmaxMultiLevelSelection.Name;
					else
						m_strValue = "";
				}
				else
				{
					m_tmaxMultiLevelSelection = null;
					m_strValue = m_ctrlValue.Value;
				}
				
				//	We must have a value
				if(m_strValue.Length == 0)
				{
					Warn("You must supply a value for the new code");
					m_ctrlValue.Focus();
					break;
				}
				
				//	Is this case code bound to a pick list?
				if(m_tmaxCaseCode.Type == TmaxCodeTypes.PickList)
				{
					//	Is the pick list bound to this code?
					if(m_tmaxCaseCode.PickList != null)
					{
						//	Which pick list should we be using to check for the specified value
						if(m_tmaxMultiLevelSelection != null)
							tmaxPickList = m_tmaxMultiLevelSelection.Parent;
						else
							tmaxPickList = m_tmaxCaseCode.PickList;
							
						//	Does this value exist in the collection?
						if((tmaxValue = tmaxPickList.FindChild(m_strValue)) != null)
						{
							//	Switch to the unique id
							m_strValue = tmaxValue.UniqueId.ToString();
						}
						else
						{
							//	Are user additions allowed for this list?
							if(tmaxPickList.UserAdditions == true)
							{
								//	Prompt for confirmation if this is not a multilevel
								if(m_tmaxMultiLevelSelection == null)
								{
									if(MessageBox.Show(m_strValue + " is not an existing value in the pick list. Do you want to add it now?", "Add Value?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
									{
										break;
									}
									
								}
								
								//	Set the flag to indicate that this is a new value
								m_bAddValue = true;
								
							}
							else
							{
								Warn(m_strValue + " does not appear in the pick list values.");
								break;
							}
							
						}// if((tmaxValue = m_tmaxCaseCode.PickList.FindChild(m_strValue)) != null)
					
					}
					else
					{
						Warn("Unable to obtain the pick list for the selected case code");
						break;
					
					}// if(m_tmaxCaseCode.PickList != null)
					
				}
				else
				{
					//	Is the user supplied value valid for the code?
					if(m_tmaxCaseCode.IsValid(m_strValue) == false)
					{
						Warn(m_strValue + " is not a valid value for the selected case code");
						m_ctrlValue.Focus();
						break;
					}
				
				}// if(m_tmaxCaseCode.Type == TmaxCodeTypes.PickList)
				
				//	All is good
				bSuccessful = true;
			
			}// while(bSuccessful == false)
					
			//	Close the form if everything is OK
			if(bSuccessful == true)
			{
				DialogResult = DialogResult.OK;
				this.Close();
			}
		
		}// private void OnClickOk(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on Cancel</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickCancel(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		
		}// private void OnClickCancel(object sender, System.EventArgs e)

		/// <summary>This method is called to set the appropriate editor for the active case code</summary>
		/// <returns>The enumerated type identifier of the appropriate editor</returns>
		private TmaxEditorCtrlTypes SetValueType()
		{
			TmaxEditorCtrlTypes eType = TmaxEditorCtrlTypes.Text;
			
			//	Do we have an active case code
			if(m_tmaxCaseCode != null)
			{
				switch(m_tmaxCaseCode.Type)
				{
					case TmaxCodeTypes.Memo:
						
						eType = TmaxEditorCtrlTypes.Memo;
						break;
							
					case TmaxCodeTypes.Boolean:
						
						eType = TmaxEditorCtrlTypes.Boolean;
						break;
							
					case TmaxCodeTypes.Integer:
						
						eType = TmaxEditorCtrlTypes.Integer;
						break;
							
					case TmaxCodeTypes.Decimal:
						
						eType = TmaxEditorCtrlTypes.Decimal;
						break;
							
					case TmaxCodeTypes.Date:
						
						eType = TmaxEditorCtrlTypes.Date;
						break;
							
					case TmaxCodeTypes.PickList:
						
						if(m_tmaxCaseCode.PickList != null)
						{
							//	Is this a multi-level value?
							if(m_tmaxCaseCode.IsMultiLevel == true)
							{
								eType = TmaxEditorCtrlTypes.MultiLevel;
								m_ctrlValue.MultiLevel = m_tmaxCaseCode.PickList;
								m_ctrlValue.MultiLevelSelection = null;
								m_ctrlValue.UserAdditions = true; // Use the pick list flag
							}
							else
							{
								eType = TmaxEditorCtrlTypes.DropList;
								m_ctrlValue.DropListValues = m_tmaxCaseCode.PickList.Children;
								m_ctrlValue.UserAdditions  = m_tmaxCaseCode.PickList.UserAdditions;
								m_ctrlValue.MultiLevel = null;
								m_ctrlValue.MultiLevelSelection = null;
							}
							
						}
						else	
						{
							m_tmaxEventSource.FireDiagnostic(this, "SetValueType", "NULL Case Code Pick List");
							eType = TmaxEditorCtrlTypes.DropList;
							m_ctrlValue.UserAdditions = false;
						}
						break;
							
					case TmaxCodeTypes.Text:
					default:
						
						eType = TmaxEditorCtrlTypes.Text;
						break;
						
				}// switch(m_tmaxCaseCode.Type)
					
			}// if(m_tmaxCaseCode != null)
			
			if(m_ctrlValue.Type != eType)
				m_ctrlValue.Type = eType;
				
			return eType;
		
		}// private TmaxEditBoxTypes SetValueType()
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>The collection of case codes</summary>
		public CTmaxCaseCodes CaseCodes
		{
			get { return m_tmaxCaseCodes;  }
			set { m_tmaxCaseCodes = value; }
		}
		
		/// <summary>The selected case code</summary>
		public CTmaxCaseCode CaseCode
		{
			get { return m_tmaxCaseCode;  }
			set { m_tmaxCaseCode = value; }
		}
		
		/// <summary>The selected pick list value when case code bound to multilevel pick list</summary>
		public CTmaxPickItem MultiLevelSelection
		{
			get { return m_tmaxMultiLevelSelection;  }
			set { m_tmaxMultiLevelSelection = value; }
		}
		
		/// <summary>The value to be assigned to the new code</summary>
		public string Value
		{
			get { return m_strValue;  }
			set { m_strValue = value; }
		}
		
		/// <summary>True if the value needs to be added to the pick list</summary>
		public bool AddValue
		{
			get { return m_bAddValue;  }
		}
		
		#endregion Properties
	
	}// public class CFAddCode : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
