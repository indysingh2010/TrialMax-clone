using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Controls
{
	/// <summary>This control allows the user to set the overrides for the default media paths used by the database</summary>
	public class CTmaxCaseCodesViewerCtrl : FTI.Trialmax.Controls.CTmaxBaseCtrl
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_CLICK_EDIT_EX	=			ERROR_TMAX_BASE_CONTROL_MAX + 1;
		private const int ERROR_CLICK_INSERT_AFTER_EX =		ERROR_TMAX_BASE_CONTROL_MAX + 2;
		private const int ERROR_APPLY_EX =					ERROR_TMAX_BASE_CONTROL_MAX + 3;
		private const int ERROR_FILL_CASE_CODES_EX =		ERROR_TMAX_BASE_CONTROL_MAX + 4;
		private const int ERROR_ADD_EX =					ERROR_TMAX_BASE_CONTROL_MAX + 5;
		private const int ERROR_ON_ADDED_EX =				ERROR_TMAX_BASE_CONTROL_MAX + 6;
		private const int ERROR_ON_UPDATED_EX =				ERROR_TMAX_BASE_CONTROL_MAX + 7;
		private const int ERROR_ON_DELETED_EX =				ERROR_TMAX_BASE_CONTROL_MAX + 8;
		private const int ERROR_CLICK_DELETE_EX =			ERROR_TMAX_BASE_CONTROL_MAX + 9;
		private const int ERROR_CLICK_MOVE_UP_EX =			ERROR_TMAX_BASE_CONTROL_MAX + 10;
		private const int ERROR_CLICK_MOVE_DOWN_EX =		ERROR_TMAX_BASE_CONTROL_MAX + 11;
		private const int ERROR_ON_REORDERED_EX =			ERROR_TMAX_BASE_CONTROL_MAX + 12;
		private const int ERROR_CLICK_INSERT_BEFORE_EX =	ERROR_TMAX_BASE_CONTROL_MAX + 13;
	
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>Button to request editing of existing map</summary>
		private System.Windows.Forms.Button m_ctrlEdit;

		/// <summary>Group box for the list of case codes</summary>
		private System.Windows.Forms.GroupBox m_ctrlCodesGroup = null;

		/// <summary>Static text label for data type control</summary>
		private System.Windows.Forms.Label m_ctrlTypeLabel;

		/// <summary>Static text control to display the data type</summary>
		private System.Windows.Forms.Label m_ctrlType;

		/// <summary>Static text control to display if the code can be edited</summary>
		private System.Windows.Forms.Label m_ctrlAllowEdit;

		/// <summary>Static text label for Allow Edit control</summary>
		private System.Windows.Forms.Label m_ctrlAllowEditLabel;

		/// <summary>Static text control to display whether the code supports multiple entries</summary>
		private System.Windows.Forms.Label m_ctrlAllowMultiple;

		/// <summary>Static text label for Allow Multiple control</summary>
		private System.Windows.Forms.Label m_ctrlAllowMultipleLabel;

		/// <summary>List box containing the names of all the case codes</summary>
		private System.Windows.Forms.ListBox m_ctrlCodes;

		/// <summary>Group box for case code properties</summary>
		private System.Windows.Forms.GroupBox m_ctrlPropertiesGroup;

		/// <summary>The form's tool tip control</summary>
		private System.Windows.Forms.ToolTip m_ctrlToolTips;

		/// <summary>Static text label for Pick List control</summary>
		private System.Windows.Forms.Label m_ctrlPickList;

		/// <summary>Group box for case code properties</summary>
		private System.Windows.Forms.Label m_ctrlPickListLabel;

		/// <summary>Local member bound to CaseCodes property</summary>
		private FTI.Shared.Trialmax.CTmaxCaseCodes m_tmaxCaseCodes = null;

		/// <summary>Local member bound to PickLists property</summary>
		private FTI.Shared.Trialmax.CTmaxPickItem m_tmaxPickLists = null;

		/// <summary>Pushbutton to delete the current selection</summary>
		private System.Windows.Forms.Button m_ctrlDelete;

		/// <summary>Pushbutton to move the current selection up in the list</summary>
		private System.Windows.Forms.Button m_ctrlMoveUp;

		/// <summary>Pushbutton to move the current selection down in the list</summary>
		private System.Windows.Forms.Button m_ctrlMoveDown;

		/// <summary>Pushbutton to add a new code after the current selection</summary>
		private System.Windows.Forms.Button m_ctrlInsertAfter;

		/// <summary>Pushbutton to add a new code before the current selection</summary>
		private System.Windows.Forms.Button m_ctrlInsertBefore;

		/// <summary>Local member bound to Modified property</summary>
		private bool m_bModified = false;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Fired by the control when the user clicks on the Edit button</summary>
		public event System.EventHandler ClickEdit;	
	
		/// <summary>Fired by the control when the user clicks on the Insert After button</summary>
		public event System.EventHandler ClickInsertAfter;	
	
		/// <summary>Fired by the control when the user clicks on the Insert After button</summary>
		public event System.EventHandler ClickInsertBefore;	
	
		/// <summary>Fired by the control when the user clicks on the Delete button</summary>
		public event System.EventHandler ClickDelete;	
	
		/// <summary>Fired by the control when the user clicks on the MoveUp button</summary>
		public event System.EventHandler ClickMoveUp;	
	
		/// <summary>Fired by the control when the user clicks on the MoveDown button</summary>
		public event System.EventHandler ClickMoveDown;	
	
		/// <summary>Constructor</summary>
		public CTmaxCaseCodesViewerCtrl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		
			m_tmaxEventSource.Name = "Case Codes Viewer Control";
			
			//	Add the error builder's format strings
			SetErrorStrings();
			
		}// public CTmaxCaseCodesViewerCtrl()

		/// <summary>This method is called to apply the user defined values</summary>
		/// <returns>true if changes have been made</returns>
		public bool Apply()
		{
			try
			{
				return true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Apply", m_tmaxErrorBuilder.Message(ERROR_APPLY_EX), Ex);
				return true; // Don't prevent the form from closing
			}
		
		}// public bool Apply()
		
		/// <summary>This method is called by the application when it adds new media to the database</summary>
		/// <param name="tmaxParent">TrialMax event item that identifies the parent record</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		{
			CTmaxCaseCode	tmaxSelection = null;
			int				iIndex = -1;
			
			try
			{
				//	Add each new child case code to the list
				foreach(CTmaxItem O in tmaxChildren)
				{
					if((O.DataType == TmaxDataTypes.CaseCode) && (O.CaseCode != null))
					{
						if(Add(O.CaseCode) == true)
						{
							//	We want to select the first new code
							if(tmaxSelection == null)
								tmaxSelection = O.CaseCode;
						}
						
					}// if((O.DataType == TmaxDataTypes.CaseCode) && (O.CaseCode != null))
					
				}// foreach(CTmaxItem O in tmaxChildren)
				
				//	Should we select a code?
				if(tmaxSelection != null)
					if((iIndex = m_ctrlCodes.Items.IndexOf(tmaxSelection)) >= 0)
						m_ctrlCodes.SelectedIndex = iIndex;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnAdded", m_tmaxErrorBuilder.Message(ERROR_ON_ADDED_EX), Ex);
			}
		
		}// public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		
		/// <summary>This method is called by the application when multiple records have been updated in an operation</summary>
		/// <param name="tmaxItems">The items that have been updated</param>
		public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			CTmaxCaseCode	tmaxSelection = null;
			bool			bSelected = false;
			
			try
			{
				//	Get the current selection
				tmaxSelection = GetSelection();
				
				foreach(CTmaxItem O in tmaxItems)
				{
					//	Is this a case code?
					if(O.CaseCode != null)
					{
						for(int i = 0; i < m_ctrlCodes.Items.Count; i++)
						{
							if(ReferenceEquals(O.CaseCode, m_ctrlCodes.Items[i]) == true)
							{
								//	Is this the selected item?
								if(i == m_ctrlCodes.SelectedIndex)
									bSelected = true;
									
								//	We have to remove and replace to get the text to redraw
								m_ctrlCodes.Items.RemoveAt(i);
								if(i < m_ctrlCodes.Items.Count)
									m_ctrlCodes.Items.Insert(i, O.CaseCode);
								else
									m_ctrlCodes.Items.Add(O.CaseCode);
									
								if(bSelected == true)
									m_ctrlCodes.SelectedIndex = i;
									
							}
								
						}// for(int i = 0; i < m_ctrlCodes.Items.Count; i++)
					
					}// if(O.CaseCode != null)
					
					//	Is this a pick item?
					if(O.PickItem != null)
					{
						//	Update the current selection if it has a pick list
						if((tmaxSelection != null) && (tmaxSelection.PickListId > 0))
							UpdateSelection();
					}
				
				}// foreach(CTmaxItem O in tmaxItems)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnUpdated", m_tmaxErrorBuilder.Message(ERROR_ON_UPDATED_EX), Ex);
			}
			
		}// public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		
		/// <summary>This method is called by the application to when the item's child collection has been reordered</summary>
		/// <param name="tmaxItem">The item that owns the child collection</param>
		public override void OnReordered(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{
			CTmaxCaseCode tmaxSelection = null;
			
			try
			{
				//	Have the case codes been reordered?
				if((tmaxItem != null) && (tmaxItem.DataType == TmaxDataTypes.CaseCode))
				{
					if(this.CaseCodes != null)
					{
						//	Get the current selection
						tmaxSelection = GetSelection();
						
						//	Rebuild the list
						FillCodes();
						
						if(tmaxSelection != null)
							this.SetSelection(tmaxSelection);
							
						SetControlStates();
							
					}// if(this.CaseCodes != null)
				
				}// if((tmaxItem != null) && (tmaxItem.DataType == TmaxDataTypes.CaseCode))
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnReordered", m_tmaxErrorBuilder.Message(ERROR_ON_REORDERED_EX), Ex);
			}
			
		}// public override void OnReordered(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		/// <summary>This method is called by the application to when the item gets deleted</summary>
		/// <param name="tmaxItem">The item that has been deleted</param>
		public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			int		iIndex = 0;
			bool	bSelected = false;
			
			try
			{
				foreach(CTmaxItem tmaxParent in tmaxItems)
				{
					foreach(CTmaxItem tmaxDeleted in tmaxParent.SubItems)
					{
						if((tmaxDeleted.DataType == TmaxDataTypes.CaseCode) && (tmaxDeleted.CaseCode != null))
						{
							if((iIndex = m_ctrlCodes.Items.IndexOf(tmaxDeleted.CaseCode)) >= 0)
							{
								//	Is this the selected code?
								if(iIndex == m_ctrlCodes.SelectedIndex)
									bSelected = true;
									
								m_ctrlCodes.Items.RemoveAt(iIndex);
								
								//	Reset the current selection if we can
								if((bSelected == true) && (m_ctrlCodes.Items.Count > 0))
								{
									if(iIndex >= m_ctrlCodes.Items.Count)
										iIndex = m_ctrlCodes.Items.Count - 1;
									m_ctrlCodes.SelectedIndex = iIndex;
								}
								
								
							}
							
						}// if((tmaxDeleted.DataType == TmaxDataTypes.CaseCode) && (tmaxDeleted.CaseCode != null))
						
					}// foreach(CTmaxItem tmaxDeleted in tmaxParent.SubItems)
					
				}// foreach(CTmaxItem tmaxParent in tmaxItems)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnDeleted", m_tmaxErrorBuilder.Message(ERROR_ON_DELETED_EX), Ex);
			}
			
			SetControlStates();

		}// public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		
		/// <summary>Called to get the current selection</summary>
		/// <returns>The current selection</returns>
		public CTmaxCaseCode GetSelection()
		{
			CTmaxCaseCode tmaxCode = null;
			
			try { tmaxCode = (CTmaxCaseCode)(m_ctrlCodes.SelectedItem); }
			catch {}
			
			return tmaxCode;
			
		}// public CTmaxCaseCode GetSelection()
		
		/// <summary>This method will add a code to the list</summary>
		/// <param name="tmaxCode">The code to be added</param>
		/// <returns>true if successful</returns>
		private bool Add(CTmaxCaseCode tmaxCode)
		{
			bool	bSuccessful = false;
			int		iIndex = 0;
			
			try
			{
				Debug.Assert(m_ctrlCodes != null);
				if(m_ctrlCodes == null) return false;
				Debug.Assert(m_ctrlCodes.IsDisposed == false);
				if(m_ctrlCodes.IsDisposed == true) return false;
				
				if((this.CaseCodes != null) && (m_ctrlCodes.Items != null))
				{
					//	Get the location of the new code
					iIndex = this.CaseCodes.IndexOf(tmaxCode);

					if(iIndex < m_ctrlCodes.Items.Count)
					{
						m_ctrlCodes.Items.Insert(iIndex, tmaxCode);
						bSuccessful = true;
					}
					else
					{
						bSuccessful = (m_ctrlCodes.Items.Add(tmaxCode) >= 0);					
					}
					
				}// if(this.CaseCodes != null)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_EX, tmaxCode.Name), Ex);
			}
			
			return bSuccessful;
			
		}// private bool Add(CTmaxCaseCode tmaxCode)
		
		/// <summary>Called to request an update of the current selection</summary>
		/// <returns>The current selection</returns>
		public void UpdateSelection()
		{
			SetControlStates();
		}
		
		/// <summary>This method refreshes the list and sets the selection to the specified code</summary>
		/// <param name="lSelection">The id of the code to be selected</param>
		///	<returns>True if successful</returns>
		public bool Reload(long lSelection)
		{
			if(m_tmaxCaseCodes == null) return false;
			
			//	Do we want to use the caller's selection or our current selection?
			if((lSelection <= 0) && (GetSelection() != null))
				lSelection = GetSelection().UniqueId;
				
			//	Rebuild the list of case codes
			FillCodes();
				
			//	Set the selection
			SetSelection(lSelection);
			
			return (GetSelection() != null);
			
		}// public bool Reload(long lSelection)
		
		/// <summary>This method is called by the parent form when the user cancels</summary>
		/// <returns>true if ok to close the form</returns>
		public bool Cancel()
		{
			return true;

		}// private bool Cancel()

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Overloaded member called when the window gets created</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Must have a valid collection of maps to do anything
			if(m_tmaxCaseCodes != null)
			{
				//	Populate the list box
				FillCodes();
			
				//	Set the initial selection
				if(m_ctrlCodes.Items.Count > 0)
				{
					m_ctrlCodes.SelectedIndex = 0;
				}
			
				m_ctrlToolTips.SetToolTip(m_ctrlInsertAfter, "Insert field after current selection");
				m_ctrlToolTips.SetToolTip(m_ctrlInsertAfter, "Insert field before current selection");
				m_ctrlToolTips.SetToolTip(m_ctrlEdit, "Edit the selected field");
				m_ctrlToolTips.SetToolTip(m_ctrlAllowEdit, "Allow user to edit the field properties");
				m_ctrlToolTips.SetToolTip(m_ctrlAllowEditLabel, "Allow user to edit the field properties");
				m_ctrlToolTips.SetToolTip(m_ctrlAllowMultiple, "Allow multiple field values per record");
				m_ctrlToolTips.SetToolTip(m_ctrlAllowMultipleLabel, "Allow multiple field values per record");
				m_ctrlToolTips.SetToolTip(m_ctrlMoveUp, "Move the current selection up");
				m_ctrlToolTips.SetToolTip(m_ctrlMoveDown, "Move the current selection down");
			
			}// if(m_tmaxCaseCodes != null)
			
			//	Set the child controls
			SetControlStates();

			//	Do the base class processing
			base.OnLoad (e);
		
		}// protected override void OnLoad(EventArgs e)

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
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		override protected void SetErrorStrings()
		{
			Debug.Assert(m_tmaxErrorBuilder != null);
			Debug.Assert(m_tmaxErrorBuilder.FormatStrings != null);
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;

			//	Let the base class add it's strings first
			base.SetErrorStrings();
			
			//	Now add the strings for this control
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised on firing the ClickEdit event");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised on firing the ClickInsertAfter event");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while applying the changes");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while filling the case codes list");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add the case code: Name = %1");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while handling the application notification that user has added new records");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while handling the application notification that user has updated records");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while handling the application notification that user has deleted records");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised on firing the ClickDelete event");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised on firing the ClickMoveUp event");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised on firing the ClickMoveDown event");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while handling the application notification that user has reordered records");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised on firing the ClickInsertBefore event");

		}// override protected void SetErrorStrings()

		/// <summary>Required method for Designer support</summary>
		protected new void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.m_ctrlInsertAfter = new System.Windows.Forms.Button();
			this.m_ctrlEdit = new System.Windows.Forms.Button();
			this.m_ctrlCodesGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlCodes = new System.Windows.Forms.ListBox();
			this.m_ctrlTypeLabel = new System.Windows.Forms.Label();
			this.m_ctrlType = new System.Windows.Forms.Label();
			this.m_ctrlAllowEdit = new System.Windows.Forms.Label();
			this.m_ctrlAllowEditLabel = new System.Windows.Forms.Label();
			this.m_ctrlAllowMultiple = new System.Windows.Forms.Label();
			this.m_ctrlAllowMultipleLabel = new System.Windows.Forms.Label();
			this.m_ctrlPropertiesGroup = new System.Windows.Forms.GroupBox();
			this.m_ctrlPickList = new System.Windows.Forms.Label();
			this.m_ctrlPickListLabel = new System.Windows.Forms.Label();
			this.m_ctrlToolTips = new System.Windows.Forms.ToolTip(this.components);
			this.m_ctrlDelete = new System.Windows.Forms.Button();
			this.m_ctrlMoveUp = new System.Windows.Forms.Button();
			this.m_ctrlMoveDown = new System.Windows.Forms.Button();
			this.m_ctrlInsertBefore = new System.Windows.Forms.Button();
			this.m_ctrlPropertiesGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlInsertAfter
			// 
			this.m_ctrlInsertAfter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlInsertAfter.Location = new System.Drawing.Point(288, 144);
			this.m_ctrlInsertAfter.Name = "m_ctrlInsertAfter";
			this.m_ctrlInsertAfter.Size = new System.Drawing.Size(92, 23);
			this.m_ctrlInsertAfter.TabIndex = 2;
			this.m_ctrlInsertAfter.Text = "Insert &After";
			this.m_ctrlInsertAfter.Click += new System.EventHandler(this.OnClickInsertAfter);
			// 
			// m_ctrlEdit
			// 
			this.m_ctrlEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlEdit.Location = new System.Drawing.Point(188, 172);
			this.m_ctrlEdit.Name = "m_ctrlEdit";
			this.m_ctrlEdit.Size = new System.Drawing.Size(92, 23);
			this.m_ctrlEdit.TabIndex = 3;
			this.m_ctrlEdit.Text = "&Edit";
			this.m_ctrlEdit.Click += new System.EventHandler(this.OnClickEdit);
			// 
			// m_ctrlCodesGroup
			// 
			this.m_ctrlCodesGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCodesGroup.Location = new System.Drawing.Point(4, 4);
			this.m_ctrlCodesGroup.Name = "m_ctrlCodesGroup";
			this.m_ctrlCodesGroup.Size = new System.Drawing.Size(172, 220);
			this.m_ctrlCodesGroup.TabIndex = 4;
			this.m_ctrlCodesGroup.TabStop = false;
			this.m_ctrlCodesGroup.Text = "Fields";
			// 
			// m_ctrlCodes
			// 
			this.m_ctrlCodes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCodes.IntegralHeight = false;
			this.m_ctrlCodes.Location = new System.Drawing.Point(8, 24);
			this.m_ctrlCodes.Name = "m_ctrlCodes";
			this.m_ctrlCodes.Size = new System.Drawing.Size(160, 192);
			this.m_ctrlCodes.TabIndex = 0;
			this.m_ctrlCodes.DoubleClick += new System.EventHandler(this.OnDblClickCode);
			this.m_ctrlCodes.SelectedIndexChanged += new System.EventHandler(this.OnCodeIndexChanged);
			// 
			// m_ctrlTypeLabel
			// 
			this.m_ctrlTypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlTypeLabel.Location = new System.Drawing.Point(8, 24);
			this.m_ctrlTypeLabel.Name = "m_ctrlTypeLabel";
			this.m_ctrlTypeLabel.Size = new System.Drawing.Size(84, 20);
			this.m_ctrlTypeLabel.TabIndex = 6;
			this.m_ctrlTypeLabel.Text = "Type:";
			this.m_ctrlTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlType
			// 
			this.m_ctrlType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlType.Location = new System.Drawing.Point(96, 24);
			this.m_ctrlType.Name = "m_ctrlType";
			this.m_ctrlType.Size = new System.Drawing.Size(104, 20);
			this.m_ctrlType.TabIndex = 7;
			this.m_ctrlType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlAllowEdit
			// 
			this.m_ctrlAllowEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlAllowEdit.Location = new System.Drawing.Point(96, 52);
			this.m_ctrlAllowEdit.Name = "m_ctrlAllowEdit";
			this.m_ctrlAllowEdit.Size = new System.Drawing.Size(104, 20);
			this.m_ctrlAllowEdit.TabIndex = 9;
			this.m_ctrlAllowEdit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlAllowEditLabel
			// 
			this.m_ctrlAllowEditLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlAllowEditLabel.Location = new System.Drawing.Point(8, 52);
			this.m_ctrlAllowEditLabel.Name = "m_ctrlAllowEditLabel";
			this.m_ctrlAllowEditLabel.Size = new System.Drawing.Size(84, 20);
			this.m_ctrlAllowEditLabel.TabIndex = 8;
			this.m_ctrlAllowEditLabel.Text = "Allow Edit:";
			this.m_ctrlAllowEditLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlAllowMultiple
			// 
			this.m_ctrlAllowMultiple.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlAllowMultiple.Location = new System.Drawing.Point(96, 80);
			this.m_ctrlAllowMultiple.Name = "m_ctrlAllowMultiple";
			this.m_ctrlAllowMultiple.Size = new System.Drawing.Size(104, 20);
			this.m_ctrlAllowMultiple.TabIndex = 11;
			this.m_ctrlAllowMultiple.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlAllowMultipleLabel
			// 
			this.m_ctrlAllowMultipleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlAllowMultipleLabel.Location = new System.Drawing.Point(8, 80);
			this.m_ctrlAllowMultipleLabel.Name = "m_ctrlAllowMultipleLabel";
			this.m_ctrlAllowMultipleLabel.Size = new System.Drawing.Size(84, 20);
			this.m_ctrlAllowMultipleLabel.TabIndex = 10;
			this.m_ctrlAllowMultipleLabel.Text = "Allow Multiple:";
			this.m_ctrlAllowMultipleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlPropertiesGroup
			// 
			this.m_ctrlPropertiesGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlPropertiesGroup.Controls.Add(this.m_ctrlAllowEdit);
			this.m_ctrlPropertiesGroup.Controls.Add(this.m_ctrlAllowEditLabel);
			this.m_ctrlPropertiesGroup.Controls.Add(this.m_ctrlAllowMultiple);
			this.m_ctrlPropertiesGroup.Controls.Add(this.m_ctrlAllowMultipleLabel);
			this.m_ctrlPropertiesGroup.Controls.Add(this.m_ctrlTypeLabel);
			this.m_ctrlPropertiesGroup.Controls.Add(this.m_ctrlType);
			this.m_ctrlPropertiesGroup.Controls.Add(this.m_ctrlPickList);
			this.m_ctrlPropertiesGroup.Controls.Add(this.m_ctrlPickListLabel);
			this.m_ctrlPropertiesGroup.Location = new System.Drawing.Point(180, 4);
			this.m_ctrlPropertiesGroup.Name = "m_ctrlPropertiesGroup";
			this.m_ctrlPropertiesGroup.Size = new System.Drawing.Size(204, 136);
			this.m_ctrlPropertiesGroup.TabIndex = 12;
			this.m_ctrlPropertiesGroup.TabStop = false;
			this.m_ctrlPropertiesGroup.Text = "Properties";
			// 
			// m_ctrlPickList
			// 
			this.m_ctrlPickList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlPickList.Location = new System.Drawing.Point(96, 108);
			this.m_ctrlPickList.Name = "m_ctrlPickList";
			this.m_ctrlPickList.Size = new System.Drawing.Size(104, 20);
			this.m_ctrlPickList.TabIndex = 14;
			this.m_ctrlPickList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlPickListLabel
			// 
			this.m_ctrlPickListLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlPickListLabel.Location = new System.Drawing.Point(8, 108);
			this.m_ctrlPickListLabel.Name = "m_ctrlPickListLabel";
			this.m_ctrlPickListLabel.Size = new System.Drawing.Size(84, 20);
			this.m_ctrlPickListLabel.TabIndex = 13;
			this.m_ctrlPickListLabel.Text = "Pick List:";
			this.m_ctrlPickListLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// m_ctrlDelete
			// 
			this.m_ctrlDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlDelete.Location = new System.Drawing.Point(288, 172);
			this.m_ctrlDelete.Name = "m_ctrlDelete";
			this.m_ctrlDelete.Size = new System.Drawing.Size(92, 23);
			this.m_ctrlDelete.TabIndex = 4;
			this.m_ctrlDelete.Text = "&Delete";
			this.m_ctrlDelete.Click += new System.EventHandler(this.OnClickDelete);
			// 
			// m_ctrlMoveUp
			// 
			this.m_ctrlMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlMoveUp.Location = new System.Drawing.Point(188, 200);
			this.m_ctrlMoveUp.Name = "m_ctrlMoveUp";
			this.m_ctrlMoveUp.Size = new System.Drawing.Size(92, 23);
			this.m_ctrlMoveUp.TabIndex = 5;
			this.m_ctrlMoveUp.Text = "Move &Up";
			this.m_ctrlMoveUp.Click += new System.EventHandler(this.OnClickMoveUp);
			// 
			// m_ctrlMoveDown
			// 
			this.m_ctrlMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlMoveDown.Location = new System.Drawing.Point(288, 200);
			this.m_ctrlMoveDown.Name = "m_ctrlMoveDown";
			this.m_ctrlMoveDown.Size = new System.Drawing.Size(92, 23);
			this.m_ctrlMoveDown.TabIndex = 6;
			this.m_ctrlMoveDown.Text = "&Move Down";
			this.m_ctrlMoveDown.Click += new System.EventHandler(this.OnClickMoveDown);
			// 
			// m_ctrlInsertBefore
			// 
			this.m_ctrlInsertBefore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlInsertBefore.Location = new System.Drawing.Point(188, 144);
			this.m_ctrlInsertBefore.Name = "m_ctrlInsertBefore";
			this.m_ctrlInsertBefore.Size = new System.Drawing.Size(92, 23);
			this.m_ctrlInsertBefore.TabIndex = 1;
			this.m_ctrlInsertBefore.Text = "Insert &Before";
			this.m_ctrlInsertBefore.Click += new System.EventHandler(this.OnClickInsertBefore);
			// 
			// CTmaxCaseCodesViewerCtrl
			// 
			this.Controls.Add(this.m_ctrlInsertBefore);
			this.Controls.Add(this.m_ctrlMoveDown);
			this.Controls.Add(this.m_ctrlMoveUp);
			this.Controls.Add(this.m_ctrlDelete);
			this.Controls.Add(this.m_ctrlPropertiesGroup);
			this.Controls.Add(this.m_ctrlCodes);
			this.Controls.Add(this.m_ctrlCodesGroup);
			this.Controls.Add(this.m_ctrlEdit);
			this.Controls.Add(this.m_ctrlInsertAfter);
			this.Name = "CTmaxCaseCodesViewerCtrl";
			this.Size = new System.Drawing.Size(388, 232);
			this.m_ctrlPropertiesGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}// private void InitializeComponent()
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>Called when the user clicks on the Delete button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickDelete(object sender, System.EventArgs e)
		{
			//	Must have an editable code selected
			if(IsEditable(GetSelection()) == false)
			{
				FTI.Shared.Win32.User.MessageBeep(0);
				return;
			}
			
			//	Propagate the event
			if(ClickDelete != null)
			{
				try
				{
					//	Fire the event to open the editor
					ClickDelete(sender, e);
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "OnClickDelete", m_tmaxErrorBuilder.Message(ERROR_CLICK_DELETE_EX), Ex);
				}
				
			}// if(ClickDelete != null)
			
		}

		/// <summary>Called when the user clicks on the Edit button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickEdit(object sender, System.EventArgs e)
		{
			//	Must have an editable code selected
			if(IsEditable(GetSelection()) == false)
			{
				FTI.Shared.Win32.User.MessageBeep(0);
				return;
			}
			
			//	Propagate the event
			if(ClickEdit != null)
			{
				try
				{
					//	Fire the event to open the editor
					ClickEdit(sender, e);
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "OnClickEdit", m_tmaxErrorBuilder.Message(ERROR_CLICK_EDIT_EX), Ex);
				}
				
			}// if(ClickEdit != null)
			
		}// private void OnClickEdit(object sender, System.EventArgs e)
		
		/// <summary>Called when the user clicks on the InsertAfter button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickInsertAfter(object sender, System.EventArgs e)
		{
			//	Propagate the event
			if(ClickInsertAfter != null)
			{
				try
				{
					//	Fire the event to open the editor
					ClickInsertAfter(sender, e);
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "OnClickInsertAfter", m_tmaxErrorBuilder.Message(ERROR_CLICK_INSERT_AFTER_EX), Ex);
				}
				
			}// if(ClickInsertAfter != null)
		
		}// private void OnClickInsertAfter(object sender, System.EventArgs e)
		
		/// <summary>Called when the user clicks on the InsertAfter button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickInsertBefore(object sender, System.EventArgs e)
		{
			//	Propagate the event
			if(ClickInsertBefore != null)
			{
				try
				{
					//	Fire the event to open the editor
					ClickInsertBefore(sender, e);
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "OnClickInsertBefore", m_tmaxErrorBuilder.Message(ERROR_CLICK_INSERT_BEFORE_EX), Ex);
				}
				
			}// if(ClickInsertBefore != null)
		
		}// private void OnClickInsertBefore(object sender, System.EventArgs e)
		
		/// <summary>Called when the user clicks on the MoveUp button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickMoveUp(object sender, System.EventArgs e)
		{
			//	Propagate the event
			if(ClickMoveUp != null)
			{
				try
				{
					//	Fire the event to open the editor
					ClickMoveUp(sender, e);
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "OnClickMoveUp", m_tmaxErrorBuilder.Message(ERROR_CLICK_MOVE_UP_EX), Ex);
				}
				
			}// if(ClickMoveUp != null)
		
		}// private void OnClickMoveUp(object sender, System.EventArgs e)
		
		/// <summary>Called when the user clicks on the MoveDown button</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnClickMoveDown(object sender, System.EventArgs e)
		{
			//	Propagate the event
			if(ClickMoveDown != null)
			{
				try
				{
					//	Fire the event to open the editor
					ClickMoveDown(sender, e);
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "OnClickMoveDown", m_tmaxErrorBuilder.Message(ERROR_CLICK_MOVE_DOWN_EX), Ex);
				}
				
			}// if(ClickMoveDown != null)
		
		}// private void OnClickMoveDown(object sender, System.EventArgs e)
		
		/// <summary>This method will populate the list of case codes (data fields)</summary>
		private void FillCodes()
		{
			try
			{
				Debug.Assert(m_ctrlCodes != null);
				if(m_ctrlCodes == null) return;
				Debug.Assert(m_ctrlCodes.IsDisposed == false);
				if(m_ctrlCodes.IsDisposed == true) return;
				
				m_ctrlCodes.SuspendLayout();
				
				//	Clear the existing list
				m_ctrlCodes.Items.Clear();
				
				//	Add all codes in the collection
				if(m_tmaxCaseCodes != null)
				{
					foreach(CTmaxCaseCode O in m_tmaxCaseCodes)
						m_ctrlCodes.Items.Add(O);
						
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FillCodes", m_tmaxErrorBuilder.Message(ERROR_FILL_CASE_CODES_EX), Ex);
			}
			
			m_ctrlCodes.ResumeLayout();
			
		}// private void FillCodes()
		
		/// <summary>This method will select the specified code in the list box</summary>
		/// <param name="tmaxCaseCode">The code to be selected</param>
		/// <returns>True if the desired code was selected</returns>
		private bool SetSelection(CTmaxCaseCode tmaxCaseCode)
		{
			return SetSelection(tmaxCaseCode.UniqueId);			
		}

		/// <summary>This member handles events fired when the user selects a new case code from the list</summary>
		/// <param name="sender">The path maps list box</param>
		/// <param name="e">System event arguments</param>
		private void OnCodeIndexChanged(object sender, System.EventArgs e)
		{
			//	Update the current selection
			SetControlStates();
		}

		/// <summary>This member handles events fired when the user double clicks a path map</summary>
		/// <param name="sender">The path maps list box</param>
		/// <param name="e">System event arguments</param>
		private void OnDblClickCode(object sender, System.EventArgs e)
		{
			if(GetSelection() != null)
				OnClickEdit(m_ctrlEdit, System.EventArgs.Empty);
		
		}// private void OnDblClickCode(object sender, System.EventArgs e)

		/// <summary>This method will select the specified code in the list box</summary>
		/// <param name="lId">The id of the code to be selected</param>
		/// <returns>True if the desired code was selected</returns>
		private bool SetSelection(long lId)
		{
			if(m_tmaxCaseCodes == null) return false;
			if(m_ctrlCodes == null) return false;
			if(m_ctrlCodes.IsDisposed == true) return false;
			
			foreach(CTmaxCaseCode O in m_ctrlCodes.Items)
			{
				//	Is this the desired code?
				if(O.UniqueId == lId)
				{
					m_ctrlCodes.SelectedItem = O;
					return true;
				}
				
			}
			
			//	Must not have found the requested code
			return false;
			
		}// private bool SetSelection(int iId)
		
		/// <summary>This member updates the child controls based on the current selection</summary>
		private void SetControlStates()
		{
			CTmaxCaseCode tmaxCode = null;
			
			//	Get the current selection
			tmaxCode = GetSelection();
			
			if((tmaxCode = GetSelection()) != null)
			{
				m_ctrlType.Text = tmaxCode.Type.ToString();
				m_ctrlAllowEdit.Text = IsEditable(tmaxCode) ? "Yes" : "No";
				m_ctrlAllowMultiple.Text = tmaxCode.AllowMultiple ? "Yes" : "No";

				//	Has this code been assigned a pick list?
				if(tmaxCode.PickListId > 0)
				{
					if((tmaxCode.PickList == null) && (this.PickLists != null))
						tmaxCode.PickList = this.PickLists.FindList(tmaxCode.PickListId);
					
					if(tmaxCode.PickList != null)
						m_ctrlPickList.Text = tmaxCode.PickList.Name;
					else
						m_ctrlPickList.Text = (tmaxCode.PickListId.ToString() + " - NOT FOUND");
				}
				else
				{
					m_ctrlPickList.Text = "";
				}
				
				m_ctrlToolTips.SetToolTip(m_ctrlType, CTmaxCaseCodes.GetTypeDescription(tmaxCode.Type));
				m_ctrlToolTips.SetToolTip(m_ctrlTypeLabel, CTmaxCaseCodes.GetTypeDescription(tmaxCode.Type));
			}
			else
			{
				m_ctrlType.Text = "";
				m_ctrlAllowEdit.Text = "";
				m_ctrlAllowMultiple.Text = "";
				m_ctrlPickList.Text = "";
			}
			
			m_ctrlTypeLabel.Enabled = (tmaxCode != null);
			m_ctrlType.Enabled = (tmaxCode != null);
			m_ctrlAllowEditLabel.Enabled = (tmaxCode != null);
			m_ctrlAllowEdit.Enabled = (tmaxCode != null);
			m_ctrlAllowMultipleLabel.Enabled = (tmaxCode != null);
			m_ctrlAllowMultiple.Enabled = (tmaxCode != null);
			m_ctrlPickListLabel.Enabled = (tmaxCode != null);
			m_ctrlPickList.Enabled = (tmaxCode != null);
			m_ctrlInsertAfter.Enabled = (m_tmaxCaseCodes != null);
			m_ctrlInsertBefore.Enabled = (tmaxCode != null);
			m_ctrlEdit.Enabled = IsEditable(tmaxCode);
			m_ctrlDelete.Enabled = IsEditable(tmaxCode);
			m_ctrlMoveUp.Enabled = ((tmaxCode != null) && (m_tmaxCaseCodes.IndexOf(tmaxCode) != 0));
			m_ctrlMoveDown.Enabled = ((tmaxCode != null) && (m_tmaxCaseCodes.IndexOf(tmaxCode) < m_tmaxCaseCodes.Count - 1));
			
		}// private void SetControlStates()

		/// <summary>Call to determine if the specified code is editable</summary>
		/// <param name="tmaxCode">The code to be checked</param>
		/// <returns>true if the code can be edited</returns>
		private bool IsEditable(CTmaxCaseCode tmaxCode)
		{
			if(tmaxCode == null) return false;
			if(tmaxCode.IsCodedProperty == true) return false;
			
			return true;
			
		}// private bool IsEditable(CTmaxCaseCode tmaxCode)
		
		#endregion Private Methods

		#region Properties
		
		/// <summary>Case options containing the case paths to be configured</summary>
		public FTI.Shared.Trialmax.CTmaxCaseCodes CaseCodes
		{
			get	{ return m_tmaxCaseCodes; }
			set	{ m_tmaxCaseCodes = value; }
		}		
		
		/// <summary>True if values have been modified</summary>
		public bool Modified
		{
			get	{ return m_bModified; }
		}
				
		/// <summary>The current selection</summary>
		public FTI.Shared.Trialmax.CTmaxCaseCode CaseCode
		{
			get	{ return GetSelection(); }
		}		
		
		/// <summary>This is the application's collection of pick lists</summary>
		public FTI.Shared.Trialmax.CTmaxPickItem PickLists
		{
			get { return m_tmaxPickLists; }
			set { m_tmaxPickLists = value; }
		}
		
		#endregion Properties
		
	}// public class CTmaxCaseCodesViewerCtrl : CTmaxBaseCtrl

}// namespace FTI.Trialmax.Controls

