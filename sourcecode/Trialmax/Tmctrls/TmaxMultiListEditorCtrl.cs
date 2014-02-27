using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinToolbars;

namespace FTI.Trialmax.Controls
{
	/// <summary>This class implements a custom editor for pick list data types</summary>
	public class CTmaxMultiLevelEditorCtrl : CTmaxBaseCtrl
	{
		#region Constants
		
		//	Toolbar command enumerations
		private enum TmaxMultiLevelCommands
		{
			Invalid = 0,
			UpOne,
			AddValue,
		}
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_SET_PARENT_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 1;
		private const int ERROR_FILL_EX				= ERROR_TMAX_BASE_CONTROL_MAX + 2;
		private const int ERROR_INVALID_CODE_ITEM	= ERROR_TMAX_BASE_CONTROL_MAX + 3;
		private const int ERROR_FIRE_FINISHED_EX	= ERROR_TMAX_BASE_CONTROL_MAX + 4;
		private const int ERROR_SET_VALUE_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 5;
		private const int ERROR_ON_CMD_ADD_VALUE_EX	= ERROR_TMAX_BASE_CONTROL_MAX + 6;
		
		private const string ULTRA_LIST_NAME_KEY	= "ListName";
		
		#endregion Constants
		
		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components;
		
		/// <summary>Image list bound to the control's main toolbar</summary>
		private System.Windows.Forms.ImageList m_ctrlToolbarImages;

		/// <summary>Manager object for the control's toolbar</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager m_ctrlToolbarManager;

		/// <summary>Background fill pane inserted by the toolbar manager</summary>
		private System.Windows.Forms.Panel CTmaxMultiLevelEditorCtrl_Fill_Panel;

		/// <summary>Docking region inserted by the toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Left;

		/// <summary>Docking region inserted by the toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Right;

		/// <summary>Docking region inserted by the toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Top;

		/// <summary>Docking region inserted by the toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Bottom;

		/// <summary>TrialMax list view control for displaying children of the active parent list</summary>
		private FTI.Trialmax.Controls.CTmaxListViewCtrl m_ctrlPickItems;

		/// <summary>Image list bound to the items list</summary>
		private System.Windows.Forms.ImageList m_ctrlListImages;

		/// <summary>Interface to the object being edited in the property grid</summary>
		FTI.Shared.Trialmax.ITmaxPropGridCtrl m_IPropGridCtrl = null;

		/// <summary>Local member bound to MultiLevel property</summary>
		FTI.Shared.Trialmax.CTmaxPickItem m_tmaxMultiLevel = null;

		/// <summary>Local member to keep track of the active parent list</summary>
		FTI.Shared.Trialmax.CTmaxPickItem m_tmaxParent = null;

		/// <summary>Local member bound to Value property</summary>
		FTI.Shared.Trialmax.CTmaxPickItem m_tmaxValue = null;

		/// <summary>Local member bound to Cancelled property</summary>
		bool m_bCancelled = true;

		/// <summary>Local member to disable processing of toolbar events</summary>
		bool m_bIgnoreEvents = false;

		/// <summary>Local member bound to UserAdditions property</summary>
		private bool m_bUserAdditions = false;
		
		#endregion Private Members

		#region Public Methods
		
		/// <summary>Fired by the control when the user completes the operation</summary>
		public event System.EventHandler Finished;	
	
		/// <summary>Constructor</summary>
		public CTmaxMultiLevelEditorCtrl() : base()
		{
			//	This call is required to initialize the child controls
			InitializeComponent();
			
			//	Set the default event source name
			m_tmaxEventSource.Name = "MultiLevel Editor";
			
			//	Initialize the child items list
			m_ctrlPickItems.Initialize(new CTmaxPickItem());
		
		}// public CTmaxMultiLevelEditorCtrl() : base()
		
		/// <summary>Called to alert the control that it's about to be displayed</summary>
		public void OnBeforeDropDown()
		{
			CTmaxPickItem tmaxParent = null;
			
			Debug.Assert(m_tmaxMultiLevel != null);
			
			if(m_tmaxMultiLevel != null)
			{
				//	Did the user specify an initial value?
				if(m_tmaxValue != null)
				{
					if(m_tmaxValue.Type == TmaxPickItemTypes.Value)
						tmaxParent = m_tmaxValue.Parent;
					else
						tmaxParent = m_tmaxValue;
				}
				
				//	Should we use the case code's list as the initial parent?
				if(tmaxParent == null)
				{
					switch(m_tmaxMultiLevel.Type)
					{
						case TmaxPickItemTypes.MultiLevel:
						case TmaxPickItemTypes.StringList:
						
							tmaxParent = m_tmaxMultiLevel;
							break;
							
						case TmaxPickItemTypes.Value:
						case TmaxPickItemTypes.Unknown:
						default:
						
							//	This should not happen
							m_tmaxEventSource.FireError(this, "OnBeforeDropDown", m_tmaxErrorBuilder.Message(ERROR_INVALID_CODE_ITEM, m_tmaxMultiLevel.Name));
							break;
					}
					
				}// if(tmaxParent == null)
				
			}// if((m_tmaxCaseCode != null) && (m_tmaxCaseCode.PickList != null))
			
			//	Set the initial parent
			SetParent(tmaxParent);
			
			//	Should we set the initial selection?
			if((m_tmaxValue != null) && (m_tmaxValue.Type == TmaxPickItemTypes.Value))
			{
				m_ctrlPickItems.SetSelected(m_tmaxValue, false);
			}
			
			m_bCancelled = true;
			m_ctrlPickItems.Focus();
			
		}// public void OnBeforeDropDown()
		
		#endregion Public Methods
		
		#region Protected Methods

		/// <summary>This function overrides the default implementation</summary>
		/// <param name="e">System event parameters - no data</param>
		protected override void OnLoad(System.EventArgs e)
		{
			//	Perform the base class processing first
			try
			{
				base.OnLoad(e);
			}
			catch
			{
				//	Need to put this in a try/catch block because of a bug
				//	in the Infragistics toolbar manager that raises an exception
				//	if the user control is not parented by a Windows form
			}

		}// protected override void OnLoad(System.EventArgs e)

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			Debug.Assert(m_tmaxErrorBuilder != null);
			Debug.Assert(m_tmaxErrorBuilder.FormatStrings != null);
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;

			//	Do the base class processing first
			base.SetErrorStrings();
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the parent list.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the pick items list.");
			m_tmaxErrorBuilder.FormatStrings.Add("The case code has been assigned an invalid pick list: code = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fire the control's Finished event.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the new value.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a new value.");
			
		}// protected void SetErrorStrings()

		/// <summary>Required by form designer</summary>
		override protected void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CTmaxMultiLevelEditorCtrl));
			Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("MainToolbar");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("ListName");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("UpOne");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AddValue");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("UpOne");
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AddValue");
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("ListName");
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			this.m_ctrlToolbarImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlToolbarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
			this.CTmaxMultiLevelEditorCtrl_Fill_Panel = new System.Windows.Forms.Panel();
			this.m_ctrlPickItems = new FTI.Trialmax.Controls.CTmaxListViewCtrl();
			this.m_ctrlListImages = new System.Windows.Forms.ImageList(this.components);
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlToolbarManager)).BeginInit();
			this.CTmaxMultiLevelEditorCtrl_Fill_Panel.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlToolbarImages
			// 
			this.m_ctrlToolbarImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.m_ctrlToolbarImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlToolbarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlToolbarImages.ImageStream")));
			this.m_ctrlToolbarImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// m_ctrlToolbarManager
			// 
			this.m_ctrlToolbarManager.DesignerFlags = 1;
			this.m_ctrlToolbarManager.DockWithinContainer = this;
			this.m_ctrlToolbarManager.ImageListSmall = this.m_ctrlToolbarImages;
			this.m_ctrlToolbarManager.ShowFullMenusDelay = 500;
			this.m_ctrlToolbarManager.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.Office2003;
			ultraToolbar1.DockedColumn = 0;
			ultraToolbar1.DockedRow = 0;
			ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.FillEntireRow = Infragistics.Win.DefaultableBoolean.True;
			ultraToolbar1.Settings.GrabHandleStyle = Infragistics.Win.UltraWinToolbars.GrabHandleStyle.None;
			ultraToolbar1.ShowInToolbarList = false;
			ultraToolbar1.Text = "MainToolbar";
			buttonTool1.InstanceProps.IsFirstInGroup = true;
			ultraToolbar1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																							  labelTool1,
																							  buttonTool1,
																							  buttonTool2});
			this.m_ctrlToolbarManager.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
																												 ultraToolbar1});
			appearance1.Image = 0;
			buttonTool3.SharedProps.AppearancesSmall.Appearance = appearance1;
			buttonTool3.SharedProps.Caption = "UpOne";
			appearance2.Image = 1;
			buttonTool4.SharedProps.AppearancesSmall.Appearance = appearance2;
			buttonTool4.SharedProps.Caption = "AddValue";
			appearance3.TextHAlign = Infragistics.Win.HAlign.Left;
			labelTool2.SharedProps.AppearancesSmall.AppearanceOnToolbar = appearance3;
			labelTool2.SharedProps.Caption = "ListName";
			labelTool2.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
			labelTool2.SharedProps.Spring = true;
			labelTool2.SharedProps.ToolTipText = "Pick List";
			this.m_ctrlToolbarManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																										  buttonTool3,
																										  buttonTool4,
																										  labelTool2});
			this.m_ctrlToolbarManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.OnUltraToolClick);
			this.m_ctrlToolbarManager.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.OnUltraBeforeToolbarListDropdown);
			// 
			// CTmaxMultiLevelEditorCtrl_Fill_Panel
			// 
			this.CTmaxMultiLevelEditorCtrl_Fill_Panel.Controls.Add(this.m_ctrlPickItems);
			this.CTmaxMultiLevelEditorCtrl_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
			this.CTmaxMultiLevelEditorCtrl_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CTmaxMultiLevelEditorCtrl_Fill_Panel.Location = new System.Drawing.Point(0, 27);
			this.CTmaxMultiLevelEditorCtrl_Fill_Panel.Name = "CTmaxMultiLevelEditorCtrl_Fill_Panel";
			this.CTmaxMultiLevelEditorCtrl_Fill_Panel.Size = new System.Drawing.Size(150, 117);
			this.CTmaxMultiLevelEditorCtrl_Fill_Panel.TabIndex = 0;
			// 
			// m_ctrlPickItems
			// 
			this.m_ctrlPickItems.AddTop = false;
			this.m_ctrlPickItems.AutoResizeColumns = false;
			this.m_ctrlPickItems.ClearOnDblClick = false;
			this.m_ctrlPickItems.DisplayMode = 0;
			this.m_ctrlPickItems.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlPickItems.HideSelection = false;
			this.m_ctrlPickItems.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlPickItems.MaxRows = 0;
			this.m_ctrlPickItems.Name = "m_ctrlPickItems";
			this.m_ctrlPickItems.OwnerImages = this.m_ctrlListImages;
			this.m_ctrlPickItems.PaneId = 0;
			this.m_ctrlPickItems.SelectedIndex = -1;
			this.m_ctrlPickItems.ShowHeaders = false;
			this.m_ctrlPickItems.ShowImage = true;
			this.m_ctrlPickItems.Size = new System.Drawing.Size(150, 117);
			this.m_ctrlPickItems.TabIndex = 0;
			this.m_ctrlPickItems.DblClick += new System.EventHandler(this.OnDblClickItem);
			this.m_ctrlPickItems.TmaxListKeyDown += new System.Windows.Forms.KeyEventHandler(this.OnTmaxListKeyDown);
			// 
			// m_ctrlListImages
			// 
			this.m_ctrlListImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.m_ctrlListImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlListImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlListImages.ImageStream")));
			this.m_ctrlListImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// _CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Left
			// 
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Left.Name = "_CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Left";
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 117);
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Left.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// _CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Right
			// 
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(150, 27);
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Right.Name = "_CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Right";
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 117);
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Right.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// _CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Top
			// 
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Top.Name = "_CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Top";
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(150, 27);
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Top.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// _CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Bottom
			// 
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 144);
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Bottom.Name = "_CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Bottom";
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(150, 0);
			this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// CTmaxMultiLevelEditorCtrl
			// 
			this.Controls.Add(this.CTmaxMultiLevelEditorCtrl_Fill_Panel);
			this.Controls.Add(this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Left);
			this.Controls.Add(this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Right);
			this.Controls.Add(this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Top);
			this.Controls.Add(this._CTmaxMultiLevelEditorCtrl_Toolbars_Dock_Area_Bottom);
			this.Name = "CTmaxMultiLevelEditorCtrl";
			this.Size = new System.Drawing.Size(150, 144);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlToolbarManager)).EndInit();
			this.CTmaxMultiLevelEditorCtrl_Fill_Panel.ResumeLayout(false);
			this.ResumeLayout(false);

		}// protected void InitializeComponent()
		
		/// <summary>Clean up all resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		
		}// protected override void Dispose(bool disposing)
		
		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>This method is called to set the option to allow the user to add values to the active pick list if supported by the list object</summary>
		/// <param name="bAllowed">True to allow user additions</param>
		public void SetUserAdditions(bool bAllowed)
		{
			m_bUserAdditions = bAllowed;
				
			SetToolStates();
		
		}// public bool SetUserAdditions(bool bAllowed)
		
		/// <summary>Called to set the value before closing the editor</summary>
		/// <param name="tmaxValue">The value selected by the user</param>
		/// <returns>true if successful</returns>
		private bool SetValue(CTmaxPickItem tmaxValue)
		{
			bool bSuccessful = true;
			
			try
			{
				m_tmaxValue = tmaxValue;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetParent", m_tmaxErrorBuilder.Message(ERROR_SET_PARENT_EX), Ex);
				bSuccessful = false;
			}
			
			return bSuccessful;
			
		}// private bool SetValue(CTmaxPickItem tmaxValue)
		
		/// <summary>Called to set the active parent list</summary>
		/// <param name="tmaxParent">The list to be activated</param>
		private void SetParent(CTmaxPickItem tmaxParent)
		{
			try
			{
				//	Save the new parent
				m_tmaxParent = tmaxParent;
				
				//	Populate the list box
				Fill(tmaxParent);
				
				//	Update the toolbar
				SetToolStates();	
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetParent", m_tmaxErrorBuilder.Message(ERROR_SET_PARENT_EX), Ex);
			}
				
		}// private void SetParent(CTmaxPickItem tmaxParent)
		
		private void Fill(CTmaxPickItem tmaxParent)
		{
			try
			{
				m_ctrlPickItems.Add(tmaxParent.Children, true);				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Fill", m_tmaxErrorBuilder.Message(ERROR_FILL_EX), Ex);
			}
				
		}// private void Fill()
		
		/// <summary>Called when the user double clicks on an item in the list</summary>
		/// <param name="sender">the object firing the event</param>
		/// <param name="e">the event arguments</param>
		private void OnDblClickItem(object sender, System.EventArgs e)
		{
			CTmaxPickItem tmaxSelection = null;

			//	Get the current selection from the list view
			if((tmaxSelection = (CTmaxPickItem)(m_ctrlPickItems.GetSelected())) != null)
			{
				//	Is the current selection a pick list value assignment?
				if(tmaxSelection.Type == TmaxPickItemTypes.Value)
				{
					this.Value = tmaxSelection;
					
					FireFinished(false);
				}
				else
				{
					//	Make the selection the new parent
					SetParent(tmaxSelection);
				}
			
			}
			else
			{
				//	This should not happen
				m_tmaxEventSource.FireDiagnostic(this, "OnDblClickItem", "ListView returns NULL item");
			}
		
		}// private void OnDblClickItem(object sender, System.EventArgs e)
		
		/// <summary>Called to notify the owner object when the user finishes the operation</summary>
		/// <param name="bCancelled">true if the user cancels</param>
		private void FireFinished(bool bCancelled)
		{
			try
			{
				m_bCancelled = bCancelled;
				
				if(Finished != null)
					Finished(this, System.EventArgs.Empty);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FireFinished", m_tmaxErrorBuilder.Message(ERROR_FIRE_FINISHED_EX), Ex);
			}
		
		}// private void FireFinished(bool bCancelled)
		
		/// <summary>Called internally to convert the specified key to its associated command enumeration</summary>
		/// <param name="strKey">The Infragistic key identifier</param>
		/// <returns>The associated command</returns>
		private TmaxMultiLevelCommands GetCommand(string strKey)
		{
			try
			{
				Array aCommands = Enum.GetValues(typeof(TmaxMultiLevelCommands));
				
				foreach(TmaxMultiLevelCommands eCommand in aCommands)
				{
					if(eCommand.ToString() == strKey)
						return eCommand;
				}
				
			}
			catch
			{
			}
			
			return TmaxMultiLevelCommands.Invalid;
		
		}// private TmaxMultiLevelCommands GetCommand(string strKey)
		
		/// <summary>This function will retrieve the tool with the specified key from the toolbar manager</summary>
		/// <param name="strKey">Alpha-numeric tool key identifier</param>
		/// <returns>Infragistic base class tool object</returns>
		private ToolBase GetUltraTool(string strKey)
		{
			ToolBase Tool = null;
					
			try
			{
				if(m_ctrlToolbarManager != null)
				{
					Tool = m_ctrlToolbarManager.Tools[strKey];
				}
			
			}
			catch
			{
			}
			
			return Tool;
		
		}// private ToolBase GetUltraTool(string strKey)
				
		/// <summary>This method is called to determine if the specified command should be enabled</summary>
		/// <param name="eCommand">The command enumeration</param>
		/// <returns>true if command should be enabled</returns>
		private bool GetCommandEnabled(TmaxMultiLevelCommands eCommand)
		{
			//	Do we have the required objects?
			if(m_tmaxMultiLevel == null) return false;
			if(m_tmaxParent == null) return false;
	
			//	What is the command?
			switch(eCommand)
			{
				case TmaxMultiLevelCommands.UpOne:
				
					//	Can we back up one level?
					if(m_tmaxParent.Parent == null) 
						return false;
					
					//	Can't go back beyond the multi-level item
					if(ReferenceEquals(m_tmaxParent, m_tmaxMultiLevel) == true)
						return false;
						
					//	All is good...
					return true;
					
				case TmaxMultiLevelCommands.AddValue:
				
					if(this.UserAdditions == false)
						return false;
					if(m_tmaxParent.UserAdditions == false)
						return false;
					if((m_tmaxParent.Type != TmaxPickItemTypes.StringList) &&
					   (m_tmaxParent.Type != TmaxPickItemTypes.Unknown))
						return false;
						
					//	All is good...
					return true;
					
				default:
				
					return false;
			}	
		
		}// private bool GetCommandEnabled(TmaxMultiLevelCommands eCommand, int iSelections)
		
		/// <summary>Traps the ToolClick event fired by the toolbar manager</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event argument object</param>
		private void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		{
			TmaxMultiLevelCommands eCommand = TmaxMultiLevelCommands.Invalid;
			
			//	Don't bother if ignoring events
			if(m_bIgnoreEvents == true) return;
			
			//	Get the command
			if(e.Tool != null && e.Tool.Key != null)
				eCommand = GetCommand(e.Tool.Key);
				
			if(eCommand != TmaxMultiLevelCommands.Invalid)
				OnCommand(eCommand);
		
		}// OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)

		/// <summary>This event is fired by the toolbar manager when it is about to display the customize menu</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The cancelable event arguments</param>
		protected void OnUltraBeforeToolbarListDropdown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventArgs e)
		{
			// Prevent this menu from coming up
			e.Cancel = true;
		}

		/// <summary>This method processes the specified command</summary>
		/// <param name="eCommand">The enumerated command identifier</param>
		private void OnCommand(TmaxMultiLevelCommands eCommand)
		{
			try
			{
				//	Direct to the appropriate handler
				switch(eCommand)
				{
					case TmaxMultiLevelCommands.UpOne:
					
						OnCmdUpOne();
						break;

					case TmaxMultiLevelCommands.AddValue:
					
						OnCmdAddValue();
						break;
						
					default:
					
						break;
				
				}// switch(eCommand)

			}
			catch
			{
				//MessageBox.Show(Ex.ToString());
				//Debug.Assert(false);
			}
		
		}// private void OnCommand(TmaxMultiLevelCommands eCommand)

		/// <summary>This method handles the event fired when the user clicks on UpOne</summary>
		private void OnCmdUpOne()
		{
			if((m_tmaxParent != null) && (m_tmaxParent.Parent != null))
			{
				//	Don't go beyond the multilevel
				if(ReferenceEquals(m_tmaxMultiLevel, m_tmaxParent) == false)
				{
					SetParent(m_tmaxParent.Parent);
				}
				
			}

		}// private void OnCmdUpOne()
		
		/// <summary>This method handles the event fired when the user clicks on AddValue</summary>
		private void OnCmdAddValue()
		{
			CFAddPickValue addValue = null;
			
			try
			{
				addValue = new CFAddPickValue();
				
				//	Initialize the form
				m_tmaxEventSource.Attach(addValue.EventSource);
				addValue.PickList = this.ParentList;
				addValue.PaneId = this.PaneId;

				if(addValue.ShowDialog(this) == DialogResult.OK)
				{
					this.Value = addValue.Added;
					FireFinished(false);
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdAddValue", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_ADD_VALUE_EX), Ex);
			}

		}// private void OnCmdAddValue()
		
		/// <summary>This method is called to enable/disable the tools in the manager's collection</summary>
		private void SetToolStates()
		{
			TmaxMultiLevelCommands eCommand;
			
			Debug.Assert(m_ctrlToolbarManager != null);
			Debug.Assert(m_ctrlToolbarManager.Tools != null);
			if((m_ctrlToolbarManager == null) ||( m_ctrlToolbarManager.Tools == null)) return;
			
			//	Check each tool in the manager's collection
			foreach(ToolBase O in m_ctrlToolbarManager.Tools)
			{
				if(O.Key == null) continue;
				
				try
				{
					if((eCommand = GetCommand(O.Key)) != TmaxMultiLevelCommands.Invalid)
					{
						//	Should the command be enabled?
						O.SharedProps.Enabled = GetCommandEnabled(eCommand);
					}
					else
					{
						//	Is this the parent list name tool?
						if(O.Key == ULTRA_LIST_NAME_KEY)
						{
							if(m_tmaxParent != null)
								O.SharedProps.Caption = m_tmaxParent.Name;
							else
								O.SharedProps.Caption = "";
						}
						
						//	Get the next tool
						continue;
					
					}// if((eCommand = GetCommand(O.Key)) == TmaxMultiLevelCommands.Invalid)
							
						
				}
				catch
				{
				}

			}// foreach(ToolBase ultraTool in m_ctrlUltraToolbarManager.Tools)
			
		}// private void SetToolStates()

		/// <summary>Handles events fired by the child list when the user presses a key</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">the event arguments</param>
		private void OnTmaxListKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Escape)
			{
				//	Cancel the operation
				FireFinished(true);
				e.Handled = true;
			}
			else if(e.KeyCode == Keys.Enter)
			{
				//	Is there a selection in the list?
				if(m_ctrlPickItems.GetSelected() != null)
				{
					//	Make is appear as though the user has double-clicked
					OnDblClickItem(m_ctrlPickItems, System.EventArgs.Empty);
				}
				else
				{
					FTI.Shared.Win32.User.MessageBeep(0);
				}

				e.Handled = true;
			}
			
		}// private void OnTmaxListKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)

		#endregion Private Methods

		#region Properties

		/// <summary>Interface to the object being edited in the property grid</summary>
		public FTI.Shared.Trialmax.ITmaxPropGridCtrl IPropGridCtrl
		{
			get { return m_IPropGridCtrl; }
			set { m_IPropGridCtrl = value; }
		}
		
		/// <summary>Top level pick list from which the selection must be made</summary>
		public FTI.Shared.Trialmax.CTmaxPickItem MultiLevel
		{
			get { return m_tmaxMultiLevel; }
			set { m_tmaxMultiLevel = value; }
		}
		
		/// <summary>Pick list value assigned to the data code</summary>
		public FTI.Shared.Trialmax.CTmaxPickItem Value
		{
			get { return m_tmaxValue; }
			set { m_tmaxValue = value; }
		}
		
		/// <summary>The parent pick list selection</summary>
		public FTI.Shared.Trialmax.CTmaxPickItem ParentList
		{
			get { return m_tmaxParent; }
		}
		
		/// <summary>True if the operation has been cancelled</summary>
		public bool Cancelled
		{
			get { return m_bCancelled; }
		}
		
		/// <summary>True if the user is allowed to add values to the value list</summary>
		public bool UserAdditions
		{
			get { return m_bUserAdditions; }
			set { SetUserAdditions(value); }
		}
		
		#endregion Properties
		
	}// class CTmaxMultiLevelEditorCtrl
			
}// namespace FTI.Trialmax.Controls

