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
	/// <summary>This form allows the user to edit the properties of a data field</summary>
	public class CFCaseCodeEditor : CFTmaxBaseForm
	{
		#region Private Members
		
		/// <summary>Component collection used by form designer</summary>
		private System.ComponentModel.IContainer components;

		/// <summary>Local member bound to PickLists property</summary>
		private CTmaxPickItem m_tmaxPickLists = null;
		
		/// <summary>Local member bound to CaseCodes property</summary>
		private CTmaxCaseCodes m_tmaxCaseCodes = null;
		
		/// <summary>Local member bound to CaseCode property</summary>
		private CTmaxCaseCode m_tmaxCaseCode = null;
		
		/// <summary>Local member bound to PickList property</summary>
		private CTmaxPickItem m_tmaxPickList = null;
		
		/// <summary>Local member bound to Modified property</summary>
		private bool m_bModified = true;
		
		/// <summary>Local member bound to EditMode property</summary>
		private bool m_bEditMode = true;
		
		/// <summary>Local member bound to PickListsEnabled property</summary>
		private bool m_bPickListsEnabled = true;
		
		/// <summary>The form's cancel button</summary>
		private System.Windows.Forms.Button m_ctrlCancel;
		
		/// <summary>The form's OK button</summary>
		private System.Windows.Forms.Button m_ctrlOk;
		
		/// <summary>The edit box used to enter the field name</summary>
		private System.Windows.Forms.TextBox m_ctrlName;

		/// <summary>The label for the Name edit box</summary>
		private System.Windows.Forms.Label m_ctrlNameLabel;
		
		/// <summary>Local member used to update the object</summary>
		private string m_strName = "";
		
		/// <summary>List box of data field types</summary>
		private System.Windows.Forms.ComboBox m_ctrlTypes;
		
		/// <summary>Label for data types list box</summary>
		private System.Windows.Forms.Label m_ctrlTypeLabel;
		
		/// <summary>Check box to allow multiple instances per record</summary>
		private System.Windows.Forms.CheckBox m_ctrlAllowMultiple;
		
		/// <summary>Static text description of selected code type</summary>
		private System.Windows.Forms.Label m_ctrlTypeDescription;
		
		/// <summary>The form's tool tip manager</summary>
		private System.Windows.Forms.ToolTip m_ctrlToolTips;
		
		/// <summary>Image list to be assigned to the pick lists tree</summary>
		private System.Windows.Forms.ImageList m_ctrlTreeImages;
		
		/// <summary>The form's pick lists tree</summary>
		private FTI.Trialmax.Controls.CTmaxPickListTreeCtrl m_ctrlPickListsTree;
		
		/// <summary>Static text label for the form's pick lists tree</summary>
		private System.Windows.Forms.Label m_ctrlPickListsLabel;
		
		/// <summary>Local member used to update the object</summary>
		private TmaxCodeTypes m_eType = TmaxCodeTypes.Unknown;
		
		#endregion Private Members
		
		#region Public Methods
		
		public CFCaseCodeEditor()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
			
		}// public CFCaseCodeEditor()

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
			if((m_tmaxCaseCodes == null) || (m_tmaxCaseCode == null))
			{
				m_ctrlOk.Enabled = false;
				m_ctrlNameLabel.Enabled = false;
				m_ctrlName.Enabled = false;
				m_ctrlTypeLabel.Enabled = false;
				m_ctrlTypes.Enabled = false;
				m_ctrlAllowMultiple.Enabled = false;
			}
			else
			{
				//	Initialize the name
				m_ctrlName.Text = m_tmaxCaseCode.Name;
				
				//	Fill the data types list box
				FillTypes();
				
				m_ctrlAllowMultiple.Checked = m_tmaxCaseCode.AllowMultiple;
				
				//	Don't let the user change the type if in edit mode
				if(m_bEditMode == true)
				{
					m_ctrlTypeLabel.Enabled = false;
					m_ctrlTypes.Enabled = false;
					SetPickListSelection();
					m_ctrlPickListsLabel.Enabled = false;
					m_ctrlPickListsLabel.Visible = false;
					m_ctrlPickListsTree.Enabled = true;
					m_ctrlPickListsTree.HideSelection = true;
					m_ctrlPickListsTree.Override.SelectionType = Infragistics.Win.UltraWinTree.SelectType.None;
				}
				
				m_ctrlToolTips.SetToolTip(m_ctrlName, "Name used to identify the field");
				m_ctrlToolTips.SetToolTip(m_ctrlNameLabel, m_ctrlToolTips.GetToolTip(m_ctrlName));
				m_ctrlToolTips.SetToolTip(m_ctrlTypes, "Type of data assigned to the field");
				m_ctrlToolTips.SetToolTip(m_ctrlTypeLabel, m_ctrlToolTips.GetToolTip(m_ctrlTypes));
				m_ctrlToolTips.SetToolTip(m_ctrlAllowMultiple, "Check to allow multiple values of this field per record");
				m_ctrlToolTips.SetToolTip(m_ctrlPickListsTree, "Pick list assigned to the field");
				m_ctrlToolTips.SetToolTip(m_ctrlPickListsLabel, m_ctrlToolTips.GetToolTip(m_ctrlPickListsTree));
			
			}// if((m_tmaxCaseCodes == null) || (m_tmaxCaseCode == null) || (m_tmaxCaseCode.CasePaths == null))
		
			base.OnLoad(e);
			
		}// private void OnLoad(object sender, System.EventArgs e)
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>Required method for Designer support</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTree.UltraTreeColumnSet ultraTreeColumnSet1 = new Infragistics.Win.UltraWinTree.UltraTreeColumnSet();
			Infragistics.Win.UltraWinTree.Override _override1 = new Infragistics.Win.UltraWinTree.Override();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CFCaseCodeEditor));
			this.m_ctrlCancel = new System.Windows.Forms.Button();
			this.m_ctrlOk = new System.Windows.Forms.Button();
			this.m_ctrlNameLabel = new System.Windows.Forms.Label();
			this.m_ctrlName = new System.Windows.Forms.TextBox();
			this.m_ctrlTypes = new System.Windows.Forms.ComboBox();
			this.m_ctrlTypeLabel = new System.Windows.Forms.Label();
			this.m_ctrlAllowMultiple = new System.Windows.Forms.CheckBox();
			this.m_ctrlTypeDescription = new System.Windows.Forms.Label();
			this.m_ctrlToolTips = new System.Windows.Forms.ToolTip(this.components);
			this.m_ctrlPickListsTree = new FTI.Trialmax.Controls.CTmaxPickListTreeCtrl();
			this.m_ctrlTreeImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlPickListsLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlPickListsTree)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlCancel
			// 
			this.m_ctrlCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.m_ctrlCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlCancel.Location = new System.Drawing.Point(260, 212);
			this.m_ctrlCancel.Name = "m_ctrlCancel";
			this.m_ctrlCancel.TabIndex = 4;
			this.m_ctrlCancel.Text = "  &Cancel";
			this.m_ctrlCancel.Click += new System.EventHandler(this.OnClickCancel);
			// 
			// m_ctrlOk
			// 
			this.m_ctrlOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.m_ctrlOk.Location = new System.Drawing.Point(172, 212);
			this.m_ctrlOk.Name = "m_ctrlOk";
			this.m_ctrlOk.TabIndex = 3;
			this.m_ctrlOk.Text = "&OK";
			this.m_ctrlOk.Click += new System.EventHandler(this.OnClickOk);
			// 
			// m_ctrlNameLabel
			// 
			this.m_ctrlNameLabel.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlNameLabel.Name = "m_ctrlNameLabel";
			this.m_ctrlNameLabel.Size = new System.Drawing.Size(112, 20);
			this.m_ctrlNameLabel.TabIndex = 4;
			this.m_ctrlNameLabel.Text = "Field Name:";
			this.m_ctrlNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlName
			// 
			this.m_ctrlName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlName.Location = new System.Drawing.Point(124, 8);
			this.m_ctrlName.Name = "m_ctrlName";
			this.m_ctrlName.Size = new System.Drawing.Size(218, 20);
			this.m_ctrlName.TabIndex = 0;
			this.m_ctrlName.Text = "";
			// 
			// m_ctrlTypes
			// 
			this.m_ctrlTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_ctrlTypes.Location = new System.Drawing.Point(124, 36);
			this.m_ctrlTypes.Name = "m_ctrlTypes";
			this.m_ctrlTypes.Size = new System.Drawing.Size(218, 21);
			this.m_ctrlTypes.TabIndex = 1;
			this.m_ctrlTypes.SelectedIndexChanged += new System.EventHandler(this.OnTypeSelChanged);
			// 
			// m_ctrlTypeLabel
			// 
			this.m_ctrlTypeLabel.Location = new System.Drawing.Point(8, 36);
			this.m_ctrlTypeLabel.Name = "m_ctrlTypeLabel";
			this.m_ctrlTypeLabel.Size = new System.Drawing.Size(112, 21);
			this.m_ctrlTypeLabel.TabIndex = 5;
			this.m_ctrlTypeLabel.Text = "Data Type:";
			this.m_ctrlTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlAllowMultiple
			// 
			this.m_ctrlAllowMultiple.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.m_ctrlAllowMultiple.Location = new System.Drawing.Point(124, 176);
			this.m_ctrlAllowMultiple.Name = "m_ctrlAllowMultiple";
			this.m_ctrlAllowMultiple.Size = new System.Drawing.Size(136, 24);
			this.m_ctrlAllowMultiple.TabIndex = 2;
			this.m_ctrlAllowMultiple.Text = "Allow multiple entries";
			// 
			// m_ctrlTypeDescription
			// 
			this.m_ctrlTypeDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlTypeDescription.Location = new System.Drawing.Point(124, 64);
			this.m_ctrlTypeDescription.Name = "m_ctrlTypeDescription";
			this.m_ctrlTypeDescription.Size = new System.Drawing.Size(218, 16);
			this.m_ctrlTypeDescription.TabIndex = 6;
			// 
			// m_ctrlPickListsTree
			// 
			this.m_ctrlPickListsTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			appearance1.BackColorDisabled = System.Drawing.SystemColors.Control;
			appearance1.ForeColorDisabled = System.Drawing.SystemColors.GrayText;
			this.m_ctrlPickListsTree.Appearance = appearance1;
			this.m_ctrlPickListsTree.ClearLeftClick = false;
			this.m_ctrlPickListsTree.ClearRightClick = false;
			this.m_ctrlPickListsTree.ColumnSettings.RootColumnSet = ultraTreeColumnSet1;
			this.m_ctrlPickListsTree.FullRowSelect = true;
			this.m_ctrlPickListsTree.HideSelection = false;
			this.m_ctrlPickListsTree.ImageList = this.m_ctrlTreeImages;
			this.m_ctrlPickListsTree.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_ctrlPickListsTree.Location = new System.Drawing.Point(124, 84);
			this.m_ctrlPickListsTree.Name = "m_ctrlPickListsTree";
			_override1.SelectionType = Infragistics.Win.UltraWinTree.SelectType.Single;
			_override1.Sort = Infragistics.Win.UltraWinTree.SortType.Ascending;
			this.m_ctrlPickListsTree.Override = _override1;
			this.m_ctrlPickListsTree.PickLists = null;
			this.m_ctrlPickListsTree.RightClickSelect = false;
			this.m_ctrlPickListsTree.Size = new System.Drawing.Size(220, 84);
			this.m_ctrlPickListsTree.TabIndex = 0;

			// 
			// m_ctrlTreeImages
			// 
			this.m_ctrlTreeImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlTreeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlTreeImages.ImageStream")));
			this.m_ctrlTreeImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// m_ctrlPickListsLabel
			// 
			this.m_ctrlPickListsLabel.Location = new System.Drawing.Point(8, 84);
			this.m_ctrlPickListsLabel.Name = "m_ctrlPickListsLabel";
			this.m_ctrlPickListsLabel.Size = new System.Drawing.Size(112, 21);
			this.m_ctrlPickListsLabel.TabIndex = 7;
			this.m_ctrlPickListsLabel.Text = "Pick Lists:";
			this.m_ctrlPickListsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// CFCaseCodeEditor
			// 
			this.AcceptButton = this.m_ctrlOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.m_ctrlCancel;
			this.ClientSize = new System.Drawing.Size(348, 245);
			this.Controls.Add(this.m_ctrlPickListsLabel);
			this.Controls.Add(this.m_ctrlPickListsTree);
			this.Controls.Add(this.m_ctrlTypeDescription);
			this.Controls.Add(this.m_ctrlAllowMultiple);
			this.Controls.Add(this.m_ctrlTypeLabel);
			this.Controls.Add(this.m_ctrlTypes);
			this.Controls.Add(this.m_ctrlName);
			this.Controls.Add(this.m_ctrlNameLabel);
			this.Controls.Add(this.m_ctrlCancel);
			this.Controls.Add(this.m_ctrlOk);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CFCaseCodeEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Data Field Editor";
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlPickListsTree)).EndInit();
			this.ResumeLayout(false);

		}
	
		/// <summary>This method is called when the user clicks on OK</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickOk(object sender, System.EventArgs e)
		{
			Debug.Assert(m_tmaxCaseCodes != null);
			Debug.Assert(m_tmaxCaseCode != null);
			if((m_tmaxCaseCodes == null) || (m_tmaxCaseCode == null))
			{
				DialogResult = DialogResult.Cancel;
				this.Close();
				return;
			}

			//	Check the values set by the user
			if(CheckUserSettings() == false) return;			

			//	Set the values
			if(m_tmaxCaseCode.Name != m_strName)
			{
				m_tmaxCaseCode.Name = m_strName;
				this.Modified = true;
			}
			if(m_tmaxCaseCode.AllowMultiple != m_ctrlAllowMultiple.Checked)
			{
				m_tmaxCaseCode.AllowMultiple = m_ctrlAllowMultiple.Checked;
				this.Modified = true;
			}
			if(m_bEditMode == false)
			{
				if(m_tmaxCaseCode.Type != m_eType)
				{
					m_tmaxCaseCode.Type = m_eType;
					this.Modified = true;
				}
				
				//	Assign the pick list
				if(this.PickListsEnabled == true)
				{
					if(m_tmaxPickList != null)
					{
						if(m_tmaxCaseCode.PickListId != m_tmaxPickList.UniqueId)
						{
							m_tmaxCaseCode.PickListId = m_tmaxPickList.UniqueId;
							this.Modified = true;
						}
					}
					else
					{
						if(m_tmaxCaseCode.PickListId != 0)
						{
							m_tmaxCaseCode.PickListId = 0;
							this.Modified = true;
						}
						
					}// if(m_tmaxPickList != null)
				
				}//	if(this.PickListsEnabled == true)
				
			}// if(m_bEditMode == false)
				
			//	Close the form
			DialogResult = (this.Modified == true) ? DialogResult.OK : DialogResult.Cancel;
			this.Close();
		
		}// private void OnClickOk(object sender, System.EventArgs e)

		/// <summary>This method is called when the user clicks on Cancel</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickCancel(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		
		}// private void OnClickCancel(object sender, System.EventArgs e)

		/// <summary>This method is called to fill the data types list box</summary>
		private void FillTypes()
		{
			try
			{
				int			iIndex = -1;
				string[]	aNames = Enum.GetNames(typeof(TmaxCodeTypes));
				
				foreach(string O in aNames)
				{
					if(O != TmaxCodeTypes.Unknown.ToString())
						m_ctrlTypes.Items.Add(O);
				}
				
				//	Set the initial selection
				if(m_tmaxCaseCode != null)
					iIndex = m_ctrlTypes.FindStringExact(m_tmaxCaseCode.Type.ToString());
				if(iIndex < 0)
					iIndex = m_ctrlTypes.FindStringExact(TmaxCodeTypes.Text.ToString());
					
				if(iIndex >= 0)
					m_ctrlTypes.SelectedIndex = iIndex;
				
			}
			catch
			{
			}
			
			OnTypeSelChanged(m_ctrlTypes, System.EventArgs.Empty);
		
		}// private void FillTypes()
		
		/// <summary>This method is called to set the selection in the pick list tree</summary>
		private void SetPickListSelection()
		{
			try
			{
				CTmaxPickItem	tmaxPickItem = null;
				CTmaxPickItem	tmaxPickLists = null;
				
				//	Don't bother if not using pick lists
				if(m_tmaxPickLists == null) return;
				if(this.PickListsEnabled == false) return;
				
				//	Does the case code have a pick list assigned?
				if((m_tmaxCaseCode != null) && (m_tmaxCaseCode.PickListId > 0))
				{
					//	Find the pick item to be selected
					if((tmaxPickItem = m_tmaxPickLists.FindList(m_tmaxCaseCode.PickListId)) != null)
					{
						//	Create a container so that only this list appears in the tree
						tmaxPickLists = new CTmaxPickItem();
						tmaxPickLists.Type = TmaxPickItemTypes.MultiLevel;
						tmaxPickLists.UniqueId = 0;
						tmaxPickLists.Children.Add(tmaxPickItem);

						m_ctrlPickListsTree.PickLists = tmaxPickLists;	
						m_ctrlPickListsTree.Expand(tmaxPickItem);
						
					}
					
				}
				
			}
			catch
			{
			}
		
		}// private void SetPickListSelection()
		
		/// <summary>This method is called to make sure the values set by the user are valid</summary>
		private bool CheckUserSettings()
		{
			CTmaxCaseCode	tmaxCaseCode = null;
			string			strMsg = "";
			
			//	Must have an active data field
			Debug.Assert(m_tmaxCaseCode != null);
			if(m_tmaxCaseCode == null) return false;
			
			//	Make sure the user supplied a name
			m_strName = m_ctrlName.Text.Trim();
			if(m_strName.Length == 0)
			{
				Warn("You must supply a valid name for the data field");
				m_ctrlName.Focus();
				m_ctrlName.SelectAll();
				return false;
			}
			
			//	Has the name changed?
			if(m_tmaxCaseCode.Name != m_strName)
			{
				//	Make sure this is a valid case code name
				if(CTmaxCaseCode.IsValidName(m_strName, true) == false)
				{
					return false;
				}
				
				//	No duplicate names allowed
				if(m_tmaxCaseCodes != null)
				{
					if((tmaxCaseCode = m_tmaxCaseCodes.Find(m_strName)) != null)
					{
						//	Better be the same object
						if(tmaxCaseCode.UniqueId != m_tmaxCaseCode.UniqueId)
						{
							//	Warn the user
							strMsg = "This case already has a data field named ";
							strMsg += m_strName;
							strMsg += " You must supply a unique name for the field.";
							Warn(strMsg);
							
							m_ctrlName.Focus();
							m_ctrlName.SelectAll();
							return false;
						}
						
					}
				
				}// if(m_tmaxCaseCodes != null)
				
			}// if(tmaxCaseCode.Name != m_strName)
			
			//	If adding a new field make sure the user has selected a type
			if(m_bEditMode == false)
			{
				//	Get the requested data type
				m_eType = GetTypeSelection();
				
				//	User must select a valid type
				if(m_eType == TmaxCodeTypes.Unknown)
				{
					Warn("You must select a valid data type for the new data field");
					return false;
				}
				
				//	Has the user selected a pick list type?
				else if(m_eType == TmaxCodeTypes.PickList)
				{
					if(this.PickListsEnabled ==  true)
					{
						//	Get the pick list to use for this code
						if((m_tmaxPickList = GetPickListsSelection()) == null)
							return false;
					}
					else
					{
						Warn("Pick list data types are not supported for this case");
						return false;
					}
				
				}// else if(m_eType == TmaxCodeTypes.PickList)
				
			}// if(m_bEditMode == false)
			
			return true;
		
		}// private bool CheckUserSettings()

		/// <summary>This method is called to get the current type selection</summary>
		private TmaxCodeTypes GetTypeSelection()
		{
			TmaxCodeTypes eType = TmaxCodeTypes.Unknown;
			
			if(m_ctrlTypes.SelectedIndex >= 0)
			{
				//	NOTE:	Add 1 to the selected index because we don't add
				//			Unknown to the list box
				try { eType = (TmaxCodeTypes)(m_ctrlTypes.SelectedIndex + 1); }
				catch {}
			}
		
			return eType;
			
		}// private TmaxCodeTypes GetTypeSelection()

		/// <summary>This method is called when the user selects a new data type</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnTypeSelChanged(object sender, System.EventArgs e)
		{
			TmaxCodeTypes eType = GetTypeSelection();

			if(eType != TmaxCodeTypes.Unknown)
			{
				m_ctrlTypeDescription.Text = CTmaxCaseCodes.GetTypeDescription(eType);
				
				if(this.PickListsEnabled == true)
				{
					m_ctrlPickListsTree.Enabled = (eType == TmaxCodeTypes.PickList);
					m_ctrlPickListsLabel.Enabled = (eType == TmaxCodeTypes.PickList);
				}
				else
				{
					if(eType == TmaxCodeTypes.PickList)
						Warn("Pick lists are not supported by this case");
						
					m_ctrlPickListsTree.Enabled = false;
					m_ctrlPickListsLabel.Enabled = false;
				}

			}
			else
			{
				m_ctrlTypeDescription.Text = "";
				m_ctrlPickListsTree.Enabled = false;
				m_ctrlPickListsLabel.Enabled = false;
			}
		
		}// private void OnTypeSelChanged(object sender, System.EventArgs e)

		/// <summary>Called to assign the collection of pick lists</summary>
		/// <param name="tmaxPickLists">The pick lists collection</param>
		private void SetPickLists(CTmaxPickItem tmaxPickLists)
		{
			m_tmaxPickLists = tmaxPickLists;

			if(m_ctrlPickListsTree != null)
				m_ctrlPickListsTree.PickLists = m_tmaxPickLists; 
		
		}// private void SetPickLists(CTmaxPickItem tmaxPickLists)
		
		/// <summary>Called to enable/disable pick lists</summary>
		/// <param name="bEnabled">true to enable pick lists</param>
		private void SetPickListsEnabled(bool bEnabled)
		{
			m_bPickListsEnabled = bEnabled;

			if(m_bPickListsEnabled == false)
			{
				m_ctrlPickListsTree.Enabled = false;
				m_ctrlPickListsLabel.Enabled = false;
			}
			else
			{
				//	Let the type change handler deal with it
				OnTypeSelChanged(m_ctrlTypes, System.EventArgs.Empty);
			}
		
		}// private void SetPickLists(CTmaxPickItem tmaxPickLists)
		
		/// <summary>This method is called to get the selection in the pick lists tree</summary>
		private CTmaxPickItem GetPickListsSelection()
		{
			CTmaxPickItem	tmaxPickList = null;
			string			strMsg = "";
			
			//	Get the selection in the tree
			if((tmaxPickList = m_ctrlPickListsTree.GetSelection()) != null)
			{
				//	What type of list is selected?
				switch(tmaxPickList.Type)
				{
					case TmaxPickItemTypes.Unknown:
					
						Warn("You must select a list containing values for the data field assignment.");
						tmaxPickList = null;
						break;
						
					case TmaxPickItemTypes.Value:
					
						//	This item should have a value list parent
						if(tmaxPickList.Parent != null)
						{
							strMsg  = "Data fields require value lists, not individual values. ";
							strMsg += "Do you want to assign the ";
							strMsg += tmaxPickList.Parent.Name;
							strMsg += " list?";
							
							if(MessageBox.Show(strMsg, "Pick List?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
								tmaxPickList = null;
							else
								tmaxPickList = tmaxPickList.Parent;
						}
						else
						{
							Debug.Assert(false, "Values should always have a parent");
							Warn("You must select a list containing values for the data field assignment.");
							tmaxPickList = null;
						}
						break;
						
					case TmaxPickItemTypes.StringList:
					case TmaxPickItemTypes.MultiLevel:
					default:
					
						break;	//	Allow assignment of value lists
				}
				
			}// if((tmaxPickList = m_ctrlPickListsTree.GetSelection()) != null)
			else
			{
				Warn("You must select the pick list to be assigned to this data field");
			}
			
			return tmaxPickList;
			
		}// private CTmaxPickItem GetPickListsSelection()

		#endregion Private Methods

		#region Properties
		
		/// <summary>The application's collection of case codes</summary>
		public CTmaxCaseCodes CaseCodes
		{
			get { return m_tmaxCaseCodes;  }
			set { m_tmaxCaseCodes = value; }
		}
		
		/// <summary>The application's collection of pick lists</summary>
		public CTmaxPickItem PickLists
		{
			get { return m_tmaxPickLists;  }
			set { SetPickLists(value); }
		}
		
		/// <summary>The case code being edited</summary>
		public CTmaxCaseCode CaseCode
		{
			get { return m_tmaxCaseCode;  }
			set { m_tmaxCaseCode = value; }
		}
		
		/// <summary>The pick list assigned to the case code</summary>
		public CTmaxPickItem PickList
		{
			get { return m_tmaxPickList;  }
		}
		
		/// <summary>True to indicate that the user is editing an existing field</summary>
		public bool EditMode
		{
			get { return m_bEditMode;  }
			set { m_bEditMode = value; }
		}
		
		/// <summary>True if the user has modified the case code properties</summary>
		public bool Modified
		{
			get { return m_bModified;  }
			set { m_bModified = value; }
		}
		
		/// <summary>True if the use of pick lists is enabled</summary>
		public bool PickListsEnabled
		{
			get { return m_bPickListsEnabled;  }
			set { SetPickListsEnabled(value); }
		}
		
		#endregion Properties
	
	}// public class CFCaseCodeEditor : CFTmaxBaseForm

}// namespace FTI.Trialmax.Forms
