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
	/// <summary>This class creates a custom tree for displaying TrialMax pick lists</summary>
	public class CTmaxPickListViewerCtrl : CTmaxBaseCtrl
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_SET_PICK_LISTS_EX	= ERROR_TMAX_BASE_CONTROL_MAX + 1;
		private const int ERROR_CREATE_NODE_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 2;
		private const int ERROR_OPEN_EDITOR_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 3;
		private const int ERROR_UPDATE_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 4;
		private const int ERROR_SET_TYPE_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 5;
		private const int ERROR_ON_ADDED_EX			= ERROR_TMAX_BASE_CONTROL_MAX + 6;
		private const int ERROR_ON_DELETED_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 7;
		private const int ERROR_ON_UPDATED_EX		= ERROR_TMAX_BASE_CONTROL_MAX + 8;
		private const int ERROR_SET_PARENT_TYPE_EX	= ERROR_TMAX_BASE_CONTROL_MAX + 9;
		
		/// <summary>Local context menu command identifiers</summary>
		private enum EditorCommands
		{
			Invalid = 0,
			Delete,
			ImportTextValues,
			ImportTextLists,
			ExportText,
		}
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>Tree control to display the pick list collection</summary>
		private FTI.Trialmax.Controls.CTmaxPickListTreeCtrl m_ctrlTree;
		
		/// <summary>Pushbutton to add new pick list value to the collection</summary>
		private System.Windows.Forms.Button m_ctrlAddValue;
		
		/// <summary>Pushbutton to add new pick list to the collection</summary>
		private System.Windows.Forms.Button m_ctrlAddList;
		
		/// <summary>Pushbutton to update the current selection in the tree</summary>
		private System.Windows.Forms.Button m_ctrlUpdate;
		
		/// <summary>Pushbutton to delete the current selection in the tree</summary>
		private System.Windows.Forms.Button m_ctrlDelete;
		
		/// <summary>Image list bound to the pick list tree</summary>
		private System.Windows.Forms.ImageList m_ctrlTreeImages;

		/// <summary>Local member bound to PickLists property</summary>
		private FTI.Shared.Trialmax.CTmaxPickItem m_tmaxPickLists = null;

		/// <summary>Infragistics toolbar manager to create and manage the context menu</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager m_ctrlToolbarManager;

		/// <summary>Default docking area set up for toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Left;

		/// <summary>Default docking area set up for toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Right;

		/// <summary>Default docking area set up for toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Top;

		/// <summary>Default docking area set up for toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Bottom;
		
		/// <summary>Local member to keep track of the current selection in the tree</summary>
		private FTI.Shared.Trialmax.CTmaxPickItem m_tmaxSelection = null;
		
		/// <summary>Image list bound to the toolbar manager</summary>
		private System.Windows.Forms.ImageList m_ctrlToolbarImages;
		
		/// <summary>Static text control to display the name of the current selection</summary>
		private System.Windows.Forms.Label m_ctrlName;
		
		/// <summary>Static text label for the selection name</summary>
		private System.Windows.Forms.Label m_ctrlNameLabel;
		
		/// <summary>Group box for property controls</summary>
		private System.Windows.Forms.GroupBox m_ctrlProperties;
		
		/// <summary>Static text control to display the CaseSensitive flag</summary>
		private System.Windows.Forms.Label m_ctrlCaseSensitive;
		
		/// <summary>Static text label for the CaseSensitive flag</summary>
		private System.Windows.Forms.Label m_ctrlCaseSensitiveLabel;
		
		/// <summary>Static text control to display the UserAdditions flag</summary>
		private System.Windows.Forms.Label m_ctrlUserAdditions;
		
		/// <summary>Static text label for the UserAdditions flag</summary>
		private System.Windows.Forms.Label m_ctrlUserAdditionsLabel;
		
		/// <summary>Flag to inhibit processing of toolbar and tree events</summary>
		private bool m_bIgnoreEvents = false;
		
		#endregion Private Members

		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxPickListViewerCtrl() : base()
		{
			// This call is required to initialize the child controls
			InitializeComponent();
			
			m_tmaxEventSource.Name = "Pick List Editor";
			m_tmaxEventSource.Attach(m_ctrlTree.EventSource);
		
			m_ctrlTree.Override.SortComparer = new CTmaxBaseTreeSorter();
			
		}// public CTmaxPickListViewerCtrl() : base()
		
		/// <summary>This method is called to set the pick list collection</summary>
		/// <param name="tmaxPickLists">The new pick list collection</param>
		/// <returns>true if successful</returns>
		public bool SetPickLists(CTmaxPickItem tmaxPickLists)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Assign the new collection
				m_tmaxPickLists = tmaxPickLists;
				
				//	Update the tree
				m_ctrlTree.PickLists = m_tmaxPickLists;
				
				//	Make sure the control states are correct
				SetControlStates();
				
				//	Assign the new pick list
				bSuccessful = true;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetPickLists", m_tmaxErrorBuilder.Message(ERROR_SET_PICK_LISTS_EX), Ex);
			}
			
			return bSuccessful;
			
		}// public bool SetPickLists(CTmaxPickItems tmaxPickItems)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Required by form designer</summary>
		override protected void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CTmaxPickListViewerCtrl));
			Infragistics.Win.UltraWinTree.UltraTreeColumnSet ultraTreeColumnSet1 = new Infragistics.Win.UltraWinTree.UltraTreeColumnSet();
			Infragistics.Win.UltraWinTree.Override _override1 = new Infragistics.Win.UltraWinTree.Override();
			Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("MainToolbar");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ContextMenu");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ContextMenu");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportTextValues");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportTextLists");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportText");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete");
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportTextValues");
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportText");
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportTextLists");
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			this.m_ctrlTreeImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlTree = new FTI.Trialmax.Controls.CTmaxPickListTreeCtrl();
			this.m_ctrlAddValue = new System.Windows.Forms.Button();
			this.m_ctrlAddList = new System.Windows.Forms.Button();
			this.m_ctrlUpdate = new System.Windows.Forms.Button();
			this.m_ctrlDelete = new System.Windows.Forms.Button();
			this.m_ctrlToolbarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
			this.m_ctrlToolbarImages = new System.Windows.Forms.ImageList(this.components);
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this.m_ctrlProperties = new System.Windows.Forms.GroupBox();
			this.m_ctrlUserAdditions = new System.Windows.Forms.Label();
			this.m_ctrlUserAdditionsLabel = new System.Windows.Forms.Label();
			this.m_ctrlCaseSensitive = new System.Windows.Forms.Label();
			this.m_ctrlCaseSensitiveLabel = new System.Windows.Forms.Label();
			this.m_ctrlName = new System.Windows.Forms.Label();
			this.m_ctrlNameLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlTree)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlToolbarManager)).BeginInit();
			this.m_ctrlProperties.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ctrlTreeImages
			// 
			this.m_ctrlTreeImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlTreeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlTreeImages.ImageStream")));
			this.m_ctrlTreeImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// m_ctrlTree
			// 
			this.m_ctrlTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlTree.ClearLeftClick = true;
			this.m_ctrlTree.ClearRightClick = false;
			this.m_ctrlTree.ColumnSettings.RootColumnSet = ultraTreeColumnSet1;
			this.m_ctrlToolbarManager.SetContextMenuUltra(this.m_ctrlTree, "ContextMenu");
			this.m_ctrlTree.FullRowSelect = true;
			this.m_ctrlTree.HideSelection = false;
			this.m_ctrlTree.ImageList = this.m_ctrlTreeImages;
			this.m_ctrlTree.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_ctrlTree.Location = new System.Drawing.Point(8, 8);
			this.m_ctrlTree.Name = "m_ctrlTree";
			_override1.SelectionType = Infragistics.Win.UltraWinTree.SelectType.ExtendedAutoDrag;
			_override1.Sort = Infragistics.Win.UltraWinTree.SortType.Ascending;
			this.m_ctrlTree.Override = _override1;
			this.m_ctrlTree.PickLists = null;
			this.m_ctrlTree.RightClickSelect = true;
			this.m_ctrlTree.Size = new System.Drawing.Size(128, 200);
			this.m_ctrlTree.TabIndex = 0;
			this.m_ctrlTree.AfterSelect += new FTI.Trialmax.Controls.CTmaxPickListTreeCtrl.TmaxPickListTreeHandler(this.OnAfterSelect);
			// 
			// m_ctrlAddValue
			// 
			this.m_ctrlAddValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlAddValue.Location = new System.Drawing.Point(264, 144);
			this.m_ctrlAddValue.Name = "m_ctrlAddValue";
			this.m_ctrlAddValue.Size = new System.Drawing.Size(92, 23);
			this.m_ctrlAddValue.TabIndex = 5;
			this.m_ctrlAddValue.Text = "Add &Value";
			this.m_ctrlAddValue.Click += new System.EventHandler(this.OnClickAddValue);
			// 
			// m_ctrlAddList
			// 
			this.m_ctrlAddList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlAddList.Location = new System.Drawing.Point(152, 144);
			this.m_ctrlAddList.Name = "m_ctrlAddList";
			this.m_ctrlAddList.Size = new System.Drawing.Size(92, 23);
			this.m_ctrlAddList.TabIndex = 4;
			this.m_ctrlAddList.Text = "Add &List";
			this.m_ctrlAddList.Click += new System.EventHandler(this.OnClickAddList);
			// 
			// m_ctrlUpdate
			// 
			this.m_ctrlUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlUpdate.Location = new System.Drawing.Point(264, 176);
			this.m_ctrlUpdate.Name = "m_ctrlUpdate";
			this.m_ctrlUpdate.Size = new System.Drawing.Size(92, 23);
			this.m_ctrlUpdate.TabIndex = 7;
			this.m_ctrlUpdate.Text = "&Edit";
			this.m_ctrlUpdate.Click += new System.EventHandler(this.OnClickUpdate);
			// 
			// m_ctrlDelete
			// 
			this.m_ctrlDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlDelete.Location = new System.Drawing.Point(152, 176);
			this.m_ctrlDelete.Name = "m_ctrlDelete";
			this.m_ctrlDelete.Size = new System.Drawing.Size(92, 23);
			this.m_ctrlDelete.TabIndex = 6;
			this.m_ctrlDelete.Text = "&Delete";
			this.m_ctrlDelete.Click += new System.EventHandler(this.OnClickDelete);
			// 
			// m_ctrlToolbarManager
			// 
			this.m_ctrlToolbarManager.DesignerFlags = 1;
			this.m_ctrlToolbarManager.DockWithinContainer = this;
			this.m_ctrlToolbarManager.ImageListSmall = this.m_ctrlToolbarImages;
			this.m_ctrlToolbarManager.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_ctrlToolbarManager.ShowFullMenusDelay = 500;
			ultraToolbar1.DockedColumn = 0;
			ultraToolbar1.DockedRow = 0;
			ultraToolbar1.Text = "MainToolbar";
			ultraToolbar1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																							  popupMenuTool1});
			ultraToolbar1.Visible = false;
			this.m_ctrlToolbarManager.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
																												 ultraToolbar1});
			popupMenuTool2.SharedProps.Caption = "ContextMenu";
			buttonTool2.InstanceProps.IsFirstInGroup = true;
			popupMenuTool2.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																							   buttonTool1,
																							   buttonTool2,
																							   buttonTool3,
																							   buttonTool4});
			appearance1.Image = 0;
			buttonTool5.SharedProps.AppearancesSmall.Appearance = appearance1;
			buttonTool5.SharedProps.Caption = "&Delete";
			appearance2.Image = 1;
			buttonTool6.SharedProps.AppearancesSmall.Appearance = appearance2;
			buttonTool6.SharedProps.Caption = "&Import Values from Text ...";
			appearance3.Image = 2;
			buttonTool7.SharedProps.AppearancesSmall.Appearance = appearance3;
			buttonTool7.SharedProps.Caption = "&Export Values to Text ...";
			appearance4.Image = 1;
			buttonTool8.SharedProps.AppearancesSmall.Appearance = appearance4;
			buttonTool8.SharedProps.Caption = "&Import List(s) from Text ...";
			this.m_ctrlToolbarManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																										  popupMenuTool2,
																										  buttonTool5,
																										  buttonTool6,
																										  buttonTool7,
																										  buttonTool8});
			this.m_ctrlToolbarManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.OnUltraToolClick);
			this.m_ctrlToolbarManager.AfterToolCloseup += new Infragistics.Win.UltraWinToolbars.ToolDropdownEventHandler(this.OnUltraAfterPopup);
			this.m_ctrlToolbarManager.BeforeToolDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventHandler(this.OnUltraBeforeDropDown);
			// 
			// m_ctrlToolbarImages
			// 
			this.m_ctrlToolbarImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlToolbarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlToolbarImages.ImageStream")));
			this.m_ctrlToolbarImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// _CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Left
			// 
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 0);
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Left.Name = "_CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Left";
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 216);
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Left.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// _CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Right
			// 
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(376, 0);
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Right.Name = "_CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Right";
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 216);
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Right.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// _CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Top
			// 
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Top.Name = "_CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Top";
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(376, 0);
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Top.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// _CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Bottom
			// 
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 216);
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Bottom.Name = "_CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Bottom";
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(376, 0);
			this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// m_ctrlProperties
			// 
			this.m_ctrlProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlProperties.Controls.Add(this.m_ctrlUserAdditions);
			this.m_ctrlProperties.Controls.Add(this.m_ctrlUserAdditionsLabel);
			this.m_ctrlProperties.Controls.Add(this.m_ctrlCaseSensitive);
			this.m_ctrlProperties.Controls.Add(this.m_ctrlCaseSensitiveLabel);
			this.m_ctrlProperties.Controls.Add(this.m_ctrlName);
			this.m_ctrlProperties.Controls.Add(this.m_ctrlNameLabel);
			this.m_ctrlProperties.Location = new System.Drawing.Point(144, 8);
			this.m_ctrlProperties.Name = "m_ctrlProperties";
			this.m_ctrlProperties.Size = new System.Drawing.Size(224, 128);
			this.m_ctrlProperties.TabIndex = 18;
			this.m_ctrlProperties.TabStop = false;
			this.m_ctrlProperties.Text = "Properties";
			// 
			// m_ctrlUserAdditions
			// 
			this.m_ctrlUserAdditions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlUserAdditions.Location = new System.Drawing.Point(104, 96);
			this.m_ctrlUserAdditions.Name = "m_ctrlUserAdditions";
			this.m_ctrlUserAdditions.Size = new System.Drawing.Size(88, 16);
			this.m_ctrlUserAdditions.TabIndex = 23;
			// 
			// m_ctrlUserAdditionsLabel
			// 
			this.m_ctrlUserAdditionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlUserAdditionsLabel.Location = new System.Drawing.Point(8, 96);
			this.m_ctrlUserAdditionsLabel.Name = "m_ctrlUserAdditionsLabel";
			this.m_ctrlUserAdditionsLabel.Size = new System.Drawing.Size(88, 16);
			this.m_ctrlUserAdditionsLabel.TabIndex = 22;
			this.m_ctrlUserAdditionsLabel.Text = "User Additions:";
			// 
			// m_ctrlCaseSensitive
			// 
			this.m_ctrlCaseSensitive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCaseSensitive.Location = new System.Drawing.Point(104, 60);
			this.m_ctrlCaseSensitive.Name = "m_ctrlCaseSensitive";
			this.m_ctrlCaseSensitive.Size = new System.Drawing.Size(88, 16);
			this.m_ctrlCaseSensitive.TabIndex = 21;
			// 
			// m_ctrlCaseSensitiveLabel
			// 
			this.m_ctrlCaseSensitiveLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlCaseSensitiveLabel.Location = new System.Drawing.Point(8, 60);
			this.m_ctrlCaseSensitiveLabel.Name = "m_ctrlCaseSensitiveLabel";
			this.m_ctrlCaseSensitiveLabel.Size = new System.Drawing.Size(88, 16);
			this.m_ctrlCaseSensitiveLabel.TabIndex = 20;
			this.m_ctrlCaseSensitiveLabel.Text = "Case Sensitive:";
			// 
			// m_ctrlName
			// 
			this.m_ctrlName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlName.Location = new System.Drawing.Point(64, 32);
			this.m_ctrlName.Name = "m_ctrlName";
			this.m_ctrlName.Size = new System.Drawing.Size(152, 16);
			this.m_ctrlName.TabIndex = 19;
			// 
			// m_ctrlNameLabel
			// 
			this.m_ctrlNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.m_ctrlNameLabel.Location = new System.Drawing.Point(8, 32);
			this.m_ctrlNameLabel.Name = "m_ctrlNameLabel";
			this.m_ctrlNameLabel.Size = new System.Drawing.Size(48, 16);
			this.m_ctrlNameLabel.TabIndex = 18;
			this.m_ctrlNameLabel.Text = "Name:";
			// 
			// CTmaxPickListViewerCtrl
			// 
			this.Controls.Add(this.m_ctrlProperties);
			this.Controls.Add(this.m_ctrlAddValue);
			this.Controls.Add(this.m_ctrlAddList);
			this.Controls.Add(this.m_ctrlUpdate);
			this.Controls.Add(this.m_ctrlDelete);
			this.Controls.Add(this.m_ctrlTree);
			this.Controls.Add(this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Left);
			this.Controls.Add(this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Right);
			this.Controls.Add(this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Top);
			this.Controls.Add(this._CTmaxPickListViewerCtrl_Toolbars_Dock_Area_Bottom);
			this.Name = "CTmaxPickListViewerCtrl";
			this.Size = new System.Drawing.Size(376, 216);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlTree)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlToolbarManager)).EndInit();
			this.m_ctrlProperties.ResumeLayout(false);
			this.ResumeLayout(false);

		}// protected void InitializeComponent()
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			Debug.Assert(m_tmaxErrorBuilder != null);
			Debug.Assert(m_tmaxErrorBuilder.FormatStrings != null);
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;

			//	Add placeholders for the reserved strings
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the pick lists collection.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create a new node: Name = %1  Type = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add/edit an item: Add = %1  Value = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to update the pick list item: Name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the parent item type: Name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while handling an application notification that records have been added.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while handling an application notification that records have been deleted.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while handling an application notification that records have been updated.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to update the Type property of the parent pick list item: Child = %1");
			
		}// protected override void SetErrorStrings()

		/// <summary>This method is called by the application when it adds new media to the database</summary>
		/// <param name="tmaxParent">TrialMax event item that identifies the parent record</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		{
			CTmaxPickItem tmaxEnsureVisible = null;
			
			try
			{
				//	Add each new child pick list item to the tree
				foreach(CTmaxItem O in tmaxChildren)
				{
					if((O.DataType == TmaxDataTypes.PickItem) && (O.PickItem != null))
					{
						m_ctrlTree.Add(O.PickItem, false, false);
						
						//	We want to make sure the first new node is made visible
						//
						//	NOTE:	We have to postpone this until all nodes have been
						//			added otherwise we wind up with double nodes because
						//			the parent node gets expanded when we add the first child
						if(tmaxEnsureVisible == null)
							tmaxEnsureVisible = O.PickItem;
							
						//	Make sure the parent Type value is correct
						if(O.PickItem.Parent != null)
							SetParentType(O.PickItem);
						
					}// if((O.DataType == TmaxDataTypes.PickItem) && (O.PickItem != null))
					
				}// foreach(CTmaxItem O in tmaxChildren)
			
				if(tmaxEnsureVisible != null)
					m_ctrlTree.EnsureVisible(tmaxEnsureVisible);
						
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnAdded", m_tmaxErrorBuilder.Message(ERROR_ON_ADDED_EX), Ex);
			}
			
			SetControlStates();
		
		}// public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		
		/// <summary>This method is called by the application to when the item gets deleted</summary>
		/// <param name="tmaxItem">The item that has been deleted</param>
		public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			try
			{
				//	Do process selection changes
				m_bIgnoreEvents = true;
				
				foreach(CTmaxItem tmaxParent in tmaxItems)
				{
					foreach(CTmaxItem tmaxDeleted in tmaxParent.SubItems)
					{
						if((tmaxDeleted.DataType == TmaxDataTypes.PickItem) && (tmaxDeleted.PickItem != null))
						{
							m_ctrlTree.Delete(tmaxDeleted.PickItem);
						}
						
					}// foreach(CTmaxItem tmaxDeleted in tmaxParent.SubItems)
					
					//	Update the parent if it exists
					if(tmaxParent.PickItem != null)
						Update(tmaxParent.PickItem);
						
				}// foreach(CTmaxItem tmaxParent in tmaxItems)

				SetControlStates();	
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnDeleted", m_tmaxErrorBuilder.Message(ERROR_ON_DELETED_EX), Ex);
			}
			finally
			{
				m_bIgnoreEvents = false;
			}
			
			SetControlStates();

		}// public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		
		/// <summary>This method is called by the application when multiple records have been updated in an operation</summary>
		/// <param name="tmaxItems">The items that have been updated</param>
		public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			try
			{
				foreach(CTmaxItem O in tmaxItems)
				{
					if(O.PickItem != null)
						m_ctrlTree.Update(O.PickItem);
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnUpdated", m_tmaxErrorBuilder.Message(ERROR_ON_DELETED_EX), Ex);
			}
			
		}// public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		
		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>This method is called to set the states of the child controls</summary>
		///	<param name="bSetName">true to set the text in the Name edit box</param>
		private void SetControlStates()
		{
			try
			{
				//	Update the current selection
				GetSelection();
				
				m_ctrlDelete.Enabled = GetCommandEnabled(EditorCommands.Delete, m_ctrlTree.GetSelections());
				
				if(m_tmaxSelection != null)
				{
					m_ctrlUpdate.Enabled = true;
					
					m_ctrlName.Text = m_tmaxSelection.Name;
					
					//	What type of item is this?
					switch(m_tmaxSelection.Type)
					{
						case TmaxPickItemTypes.MultiLevel:
						
							m_ctrlAddList.Enabled = true;
							m_ctrlAddValue.Enabled = false;
							m_ctrlCaseSensitive.Text = m_tmaxSelection.CaseSensitive == true ? "Yes" : "No";
							m_ctrlCaseSensitive.Visible = true;
							m_ctrlUserAdditions.Visible = false;
							break;
							
						case TmaxPickItemTypes.StringList:
						
							m_ctrlAddList.Enabled = false;
							m_ctrlAddValue.Enabled = true;
							m_ctrlCaseSensitive.Text = m_tmaxSelection.CaseSensitive == true ? "Yes" : "No";
							m_ctrlUserAdditions.Text = m_tmaxSelection.UserAdditions == true ? "Yes" : "No";
							m_ctrlCaseSensitive.Visible = true;
							m_ctrlUserAdditions.Visible = true;
							break;
							
						case TmaxPickItemTypes.Value:
						
							m_ctrlAddList.Enabled = false;
							m_ctrlAddValue.Enabled = false;
							m_ctrlCaseSensitive.Visible = false;
							m_ctrlUserAdditions.Visible = false;
							break;
							
						case TmaxPickItemTypes.Unknown:
						
							m_ctrlAddList.Enabled = true;
							m_ctrlAddValue.Enabled = true;
							m_ctrlCaseSensitive.Text = m_tmaxSelection.CaseSensitive == true ? "Yes" : "No";
							m_ctrlUserAdditions.Text = m_tmaxSelection.UserAdditions == true ? "Yes" : "No";
							m_ctrlCaseSensitive.Visible = true;
							m_ctrlUserAdditions.Visible = true;
							break;
							
						default:
						
							Debug.Assert(false, "Unhandled pick item type: " + m_tmaxSelection.Type.ToString());
							break;
							
					}// switch(m_tmaxSelection.Type)
					
				}
				else
				{
					m_ctrlUpdate.Enabled = false;
					m_ctrlAddList.Enabled = (m_ctrlTree.GetSelectionCount() == 0); // Could be multiple selections
					m_ctrlAddValue.Enabled = false;
					m_ctrlCaseSensitive.Visible = false;
					m_ctrlUserAdditions.Visible = false;
					m_ctrlName.Text = "";
					
				}// if(tmaxSelection != null)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetControlStates", Ex);
			}
		
		}// private void SetControlStates()
		
		/// <summary>This handles events fired by the tree when a node has been selected</summary>
		/// <param name="sender">The tree sending the event</param>
		/// <param name="tmaxNode">The node being selected</param>
		/// <returns>true if the operation should be cancelled</returns>
		private bool OnAfterSelect(object sender, FTI.Trialmax.Controls.CTmaxPickListTreeNode tmaxNode)
		{
			if(m_bIgnoreEvents == false)
			{
				//	Update the control states
				SetControlStates();
			}
			
			return false;
		
		}// private bool OnAfterSelect(object sender, FTI.Trialmax.Controls.CTmaxPickListTreeNode tmaxNode)

		/// <summary>Called to open the pick list item editor</summary>
		/// <param name="bAdd">True to add a new item</param>
		/// <param name="bValue">True if the item being added is a value item</param>
		/// <returns>True if successful</returns>
		private bool OpenEditor(bool bAdd, bool bValue)
		{
			CFPickListEditor		editor = null;
			CTmaxPickItem			tmaxSelection = null;
			CTmaxPickItem			tmaxEditor = null;
			CTmaxItem				tmaxParent = null;
			bool					bSuccessful = false;
			CTmaxCommandArgs		Args = null;
			
			Debug.Assert(m_tmaxPickLists != null);
			Debug.Assert(m_tmaxPickLists.Children != null);
			
			try
			{
				//	Get the current selection
				tmaxSelection = m_ctrlTree.GetSelection();
				
				//	Are we adding a new item?
				if(bAdd == true)
				{
					tmaxEditor = new CTmaxPickItem();
					tmaxEditor.Type = bValue == true ? TmaxPickItemTypes.Value : TmaxPickItemTypes.Unknown;
					tmaxEditor.Parent = tmaxSelection != null ? tmaxSelection : m_tmaxPickLists;
					
					if((tmaxEditor.Parent != null) && (tmaxEditor.Parent.Children != null))
						tmaxEditor.SortOrder = tmaxEditor.Parent.Children.GetNextSortOrder();

				}
				else
				{
					//	Must be a selection
					if((tmaxEditor = tmaxSelection) == null)
					{
						return Warn("You must select an item for editing");
					}
					
				}// if(bAdd == true)

				//	Open the editor
				editor = new CFPickListEditor();
				editor.PickItem = tmaxEditor;
				
				if(editor.ShowDialog() == DialogResult.OK)
				{
					if(bAdd == true)
					{
						//	Create an event item for the request
						tmaxParent = new CTmaxItem();
						tmaxParent.DataType = TmaxDataTypes.PickItem;
						tmaxParent.PickItem = editor.PickItem.Parent;
						
						//	Add a subitem for the new item
						if(tmaxParent.SourceItems == null)
							tmaxParent.SourceItems = new CTmaxItems();
						tmaxParent.SourceItems.Add(new CTmaxItem(editor.PickItem));
						
						//	Fire the command to request the new item
						if((Args = FireCommand(TmaxCommands.Add, tmaxParent)) != null)
							bSuccessful = Args.Successful;
							
					}
					else
					{
						bSuccessful = Update(editor.PickItem);
					}
					
				}// if(editor.ShowDialog() == DialogResult.OK)
						
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OpenEditor", m_tmaxErrorBuilder.Message(ERROR_OPEN_EDITOR_EX, bAdd, bValue), Ex);
			}
			
			return bSuccessful;
		
		}// private bool OpenEditor(bool bAdd, bool bValue)
		
		/// <summary>Called to get the item that is selected in the tree</summary>
		private void GetSelection()
		{
			m_tmaxSelection = null;
			
			try
			{
				if((m_ctrlTree.IsDisposed == false) && (m_tmaxPickLists != null))
				{
					if(m_ctrlTree.GetSelectionCount() == 1)
						m_tmaxSelection = m_ctrlTree.GetSelection();
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetSelection", Ex);
			}
		
		}// private void GetSelection()
		
		/// <summary>This method is called when the user clicks on the AddList button</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnClickAddList(object sender, System.EventArgs e)
		{
			//	Add a new pick list
			OpenEditor(true, false);
			
			SetControlStates();
		}

		/// <summary>This method is called when the user clicks on the AddValue button</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnClickAddValue(object sender, System.EventArgs e)
		{
			//	Add a new pick value
			OpenEditor(true, true);
			
			SetControlStates();
		}

		/// <summary>This method is called when the user clicks on the Update button</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnClickUpdate(object sender, System.EventArgs e)
		{
			OpenEditor(false, false);
			
			SetControlStates();

		}// private void OnClickUpdate(object sender, System.EventArgs e)

		/// <summary>This method will update the pick item specified by the caller</summary>
		/// <param name="tmaxPickItem">The item to be updated</param>
		/// <returns>True if successful</returns>
		private bool Update(CTmaxPickItem tmaxPickItem)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Fire the application command to update the item
				FireCommand(TmaxCommands.Update, new CTmaxItem(tmaxPickItem));
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Update", m_tmaxErrorBuilder.Message(ERROR_UPDATE_EX, tmaxPickItem.Name), Ex);
			}
			
			return bSuccessful;
		
		}// private bool Update(CTmaxPickItem tmaxPickItem)
		
		/// <summary>This method will get the type that should be assigned to the parent of the specified item</summary>
		///	<param name="tmaxChild">The child item</param>
		/// <returns>True if successful</returns>
		private TmaxPickItemTypes GetParentType(CTmaxPickItem tmaxChild)
		{
			switch(tmaxChild.Type)
			{
				case TmaxPickItemTypes.Unknown:
				case TmaxPickItemTypes.MultiLevel:
				case TmaxPickItemTypes.StringList:
				
					return TmaxPickItemTypes.MultiLevel;
					
				case TmaxPickItemTypes.Value:
				
					return TmaxPickItemTypes.StringList;
					
				default:
				
					return TmaxPickItemTypes.Unknown;
					
			}// switch(tmaxChild.Type)
		
		}// private TmaxPickItemTypes GetParentType(CTmaxPickItem tmaxChild)
		
		/// <summary>This method will set the Type assigned to the parent of the specified pick list item</summary>
		///	<param name="tmaxChild">The item whose parent is to be updated</param>
		/// <returns>True if successful</returns>
		private bool SetParentType(CTmaxPickItem tmaxChild)
		{
			bool				bSuccessful = false;
			TmaxPickItemTypes	eType = TmaxPickItemTypes.Unknown;
			
			try
			{
				//	Don't bother if this child does not have a valid parent
				if(tmaxChild.Parent == null) return false;
				if(tmaxChild.Parent.UniqueId <= 0) return false;
				
				//	Get the type that should be assigned to the parent
				eType = GetParentType(tmaxChild);
				
				//	Has the type changed?
				if(eType != tmaxChild.Parent.Type)
				{
					//	Update the parent
					tmaxChild.Parent.Type = eType;
					
					Update(tmaxChild.Parent);
				}
				
				bSuccessful = true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetParentType", m_tmaxErrorBuilder.Message(ERROR_SET_PARENT_TYPE_EX, tmaxChild.Name), Ex);
			}
			
			return bSuccessful;
		
		}// private void Update(CTmaxPickItem tmaxPickItem)
		
		/// <summary>This method is called when the user clicks on the Delete button</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnClickDelete(object sender, System.EventArgs e)
		{
			OnCmdDelete();
			
		}// private void OnClickDelete(object sender, System.EventArgs e)

		/// <summary>Traps the ToolClick event fired by the toolbar manager</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event argument object</param>
		private void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		{
			EditorCommands eCommand = EditorCommands.Invalid;
			
			if(m_bIgnoreEvents == false)
			{
				if((eCommand = GetCommand(e.Tool.Key)) != EditorCommands.Invalid)
					OnCommand(eCommand);
			}
		
		}// private void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		
		/// <summary>This method handles events fired by the toolbar manager after displaying the popup menu</summary>
		/// <param name="sender">The toolbar manager firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnUltraAfterPopup(object sender, Infragistics.Win.UltraWinToolbars.ToolDropdownEventArgs e)
		{
			EditorCommands	eCommand = EditorCommands.Invalid;
			PopupMenuTool	popupMenu = null;
			
			try { popupMenu = (PopupMenuTool)(e.Tool); }
			catch { return; }
			
			//	Check each tool in the menu's collection
			foreach(ToolBase O in popupMenu.Tools)
			{
				//	Get the command for this tool
				if((eCommand = GetCommand(O.Key)) != EditorCommands.Invalid)
				{
					//	Uninstall the shortcut if required
					if(GetCommandShortcut(eCommand) != Shortcut.None)
						O.SharedProps.Shortcut = Shortcut.None;
				}

			}// foreach(ToolBase O in m_ultraToolbarManager.Tools)
				
		}// private void OnUltraAfterPopup(object sender, Infragistics.Win.UltraWinToolbars.ToolDropdownEventArgs e)

		/// <summary>This function handles events fired by the toolbar manager when it is about to display a menu</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event parameters</param>
		private void OnUltraBeforeDropDown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventArgs e)
		{
			PopupMenuTool popupMenu = null;
			
			try
			{
				//	Enable/disable the commands in this submenu
				if((popupMenu = (PopupMenuTool)(e.Tool)) != null)
				{
					if(popupMenu.Tools != null)
						SetToolStates(popupMenu.Tools, true);
				}

			}
			catch
			{
			}
					
		}// private void OnUltraBeforeDropDown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventArgs e)

		/// <summary>This function is called to retrieve the menu / toolbar tool with the specified key</summary>
		/// <param name="strKey">Alpha-numeric key identifier</param>
		/// <returns>Infragistics base class tool object</returns>
		private ToolBase GetUltraTool(string strKey)
		{
			ToolBase Tool = null;
			
			if(m_ctrlToolbarManager != null)
				Tool = m_ctrlToolbarManager.Tools[strKey];

			return Tool;
		
		}// private ToolBase GetUltraTool(string strKey)
		
		/// <summary>This function is called to get the tool associated with the specified command</summary>
		/// <param name="eCommand">The application command enumeration</param>
		/// <returns>Infragistics base class tool object</returns>
		private ToolBase GetUltraTool(EditorCommands eCommand)
		{
			return GetUltraTool(eCommand.ToString());
		}
		
		/// <summary>This method is called to get the shortcut key for the specified command</summary>
		/// <param name="eCommand">The tree pane command enumeration</param>
		/// <returns>The shortcut key if there is one</returns>
		private Shortcut GetCommandShortcut(EditorCommands eCommand)
		{
			switch(eCommand)
			{
				case EditorCommands.Delete:
				case EditorCommands.ImportTextValues:
				case EditorCommands.ImportTextLists:
				case EditorCommands.ExportText:
				default:
				
					return Shortcut.None;
			
			}// switch(eCommand)

		}// private Shortcut GetCommandShortcut(EditorCommands eCommand)
		
		/// <summary>This method is called to determine if the specified command should be visible</summary>
		/// <param name="eCommand">The transcript view command enumeration</param>
		/// <param name="tmaxSelections">The current selections</param>
		/// <returns>true if command should be visible</returns>
		private bool GetCommandVisible(EditorCommands eCommand, CTmaxPickItems tmaxSelections)
		{
			switch(eCommand)
			{
				case EditorCommands.ImportTextValues:
				case EditorCommands.ImportTextLists:
				
					//	Hide the command if not enabled
					return GetCommandEnabled(eCommand, tmaxSelections);
					
				case EditorCommands.ExportText:
				case EditorCommands.Delete:
				default:
				
					return true;
			
			}// switch(eCommand)
			
		}// private bool GetCommandVisible(TreePaneCommands eCommand, CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method is called to determine if the specified command should be enabled</summary>
		/// <param name="eCommand">The transcript view command enumeration</param>
		/// <param name="tmaxSelections">The current selections</param>
		/// <returns>true if command should be enabled</returns>
		private bool GetCommandEnabled(EditorCommands eCommand, CTmaxPickItems tmaxSelections)
		{
			//	Do we have a valid collection
			if(m_tmaxPickLists == null) return false;
			
			//	What is the command?
			switch(eCommand)
			{
				case EditorCommands.Delete:
				
					if(tmaxSelections == null) return false;
					if(tmaxSelections.Count == 0) return false;
					
					CTmaxItem tmaxCmdItem = GetCmdDeleteItems(false);
					return ((tmaxCmdItem != null) && (tmaxCmdItem.SubItems.Count > 0));
					
				case EditorCommands.ImportTextValues:
					
					//	Must have a single selection for target list
					if(tmaxSelections == null) return false;
					if(tmaxSelections.Count != 1) return false;
					
					switch(tmaxSelections[0].Type)
					{
						case TmaxPickItemTypes.MultiLevel:
							return false;

						case TmaxPickItemTypes.Unknown:
						case TmaxPickItemTypes.StringList:
						case TmaxPickItemTypes.Value:
						default:
							return true;
					
					}// switch(tmaxSelections[0].Type)
					
				case EditorCommands.ImportTextLists:
					
					//	Must be no more than one node selected
					if((tmaxSelections != null) && (tmaxSelections.Count > 1))
						return false;
						
					//	Has the user selected a node?
					if((tmaxSelections != null) && (tmaxSelections.Count == 1))
					{
						switch(tmaxSelections[0].Type)
						{
							case TmaxPickItemTypes.Unknown:
							case TmaxPickItemTypes.StringList:
							case TmaxPickItemTypes.Value:
							default:
								return false;
					
							case TmaxPickItemTypes.MultiLevel:
								break;

						}// switch(tmaxSelections[0].Type)
					
					}// if((tmaxSelections != null) && (tmaxSelections.Count == 1))
					
					//	All conditions have been met
					return true;
					
				case EditorCommands.ExportText:
					
					if(tmaxSelections == null) return false;
					if(tmaxSelections.Count == 0) return false;
					
					foreach(CTmaxPickItem O in tmaxSelections)
					{
						if(O.Type != TmaxPickItemTypes.StringList)
							return false;
					}
					
					//	All conditions have been met
					return true;
						
				default:
				
					break;
			}	
			
			return false;
		
		}// private bool GetCommandEnabled(EditorCommands eCommand, CTmaxTreeNodes tmaxNodes)
		
		/// <summary>This method is called to convert the specified key to its associated command enumeration</summary>
		/// <returns>The associated tree command</returns>
		private EditorCommands GetCommand(string strKey)
		{
			try
			{
				Array aCommands = Enum.GetValues(typeof(EditorCommands));
				
				foreach(EditorCommands eCommand in aCommands)
				{
					if(eCommand.ToString() == strKey)
						return eCommand;
				}
				
			}
			catch
			{
			}
			
			return EditorCommands.Invalid;
		
		}// private EditorCommands GetCommand(string strKey)
		
		/// <summary>This function enables / disables the command tools</summary>
		/// <param name="ultraTools">The collection of tools to be enabled/disabled</param>
		/// <param name="bShortcuts">true to apply shortcuts to the tools in the collection</param>
		private void SetToolStates(Infragistics.Win.UltraWinToolbars.ToolsCollectionBase ultraTools, bool bShortcuts)
		{
			EditorCommands			eCommand = EditorCommands.Invalid;
			CTmaxPickItems			tmaxSelections = null;
			Shortcut				eShortcut = Shortcut.None;
						
			//	Get the current selections
			Debug.Assert(m_ctrlTree != null);
			Debug.Assert(m_ctrlTree.IsDisposed == false);
			if((m_ctrlTree != null) && (m_ctrlTree.IsDisposed == false))
				tmaxSelections = m_ctrlTree.GetSelections();
			
			try
			{
				//	Should we use the root collection?
				if(ultraTools == null)
					ultraTools = m_ctrlToolbarManager.Tools;
					
				//	Iterate the master tools collection
				foreach(ToolBase O in ultraTools)
				{
					//	Get the command associated with this tool
					if((eCommand = GetCommand(O.Key)) != EditorCommands.Invalid)
					{
						//	Should this tool be visible and enabled?
						O.SharedProps.Visible = GetCommandVisible(eCommand, tmaxSelections);
						O.SharedProps.Enabled = GetCommandEnabled(eCommand, tmaxSelections);

						//	Set shortcuts for commands on this menu
						if((bShortcuts == true) && (O.SharedProps.Visible == true))
						{
							if((eShortcut = GetCommandShortcut(eCommand)) != Shortcut.None)
								O.SharedProps.Shortcut = eShortcut;
						}
					
					}// if((eCommand = GetCommand(O.Key)) != EditorCommands.Invalid)
						
				}
						
			}
			catch
			{
					
			}
					
		}// private void SetToolStates()

		/// <summary>This method will process the specified command</summary>
		/// <param name="eCommand">The command to be processed</param>
		private void OnCommand(EditorCommands eCommand)
		{
			try
			{	
				//	Direct to the appropriate handler
				switch(eCommand)
				{
					case EditorCommands.Delete:
					
						OnCmdDelete();
						break;
						
					case EditorCommands.ImportTextValues:
					case EditorCommands.ImportTextLists:
					
						OnCmdImportText();
						break;
						
					case EditorCommands.ExportText:
					
						OnCmdExportText();
						break;
						
					default:
					
						Debug.Assert(false, "Unknown command identifier: " + eCommand.ToString());
						break;
				
				}// switch(eCommand)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnCommand", Ex);
			}
		
		}// private void OnCommand(EditorCommands eCommand)

		/// <summary>Toolbar event handler for Delete command</summary>
		private void OnCmdDelete()
		{
			CTmaxPickItems	tmaxSelections = null;
			CTmaxItem		tmaxParent = null;
			CTmaxPickItems	tmaxSiblings = null;
			
			//	Get the nodes selected in the tree
			tmaxSelections = m_ctrlTree.GetSelections();
			
			//	Must have at least one selection
			if((tmaxSelections == null) || (tmaxSelections.Count == 0))
			{
				Warn("You must select one or more nodes in the tree.", null);
				return;
			}
			
			//	Create an event item to represent the parent 
			tmaxParent = new CTmaxItem();
			tmaxParent.DataType = TmaxDataTypes.PickItem;
			if((tmaxSelections[0].Parent != null) && (tmaxSelections[0].Parent.UniqueId >= 0))
			{
				tmaxParent.PickItem = tmaxSelections[0].Parent;
				tmaxSiblings = tmaxParent.PickItem.Children;
			}
			else
			{
				tmaxSiblings = m_tmaxPickLists.Children;
			}

			//	Add subitems for each of the nodes being deleted
			foreach(CTmaxPickItem O in tmaxSelections)
				tmaxParent.SubItems.Add(new CTmaxItem(O));
			
			//	Fire the command to delete the associated records
			FireCommand(TmaxCommands.Delete, tmaxParent);
			
		}// private void OnCmdDelete()
		
		/// <summary>Toolbar event handler for ImportText command</summary>
		private void OnCmdImportText()
		{
			CTmaxPickItems	tmaxSelections = null;
			CTmaxItem		tmaxParent = null;
			CTmaxParameters	tmaxParameters = null;
			
			//	Get the nodes selected in the tree
			tmaxSelections = m_ctrlTree.GetSelections();
			
			//	Can not have more than one node selected
			if((tmaxSelections != null) && (tmaxSelections.Count > 1))
			{
				Warn("You must select just one node in the tree.", null);
				return;
			}
			
			//	Create an event item to represent the parent 
			tmaxParent = new CTmaxItem();
			tmaxParent.DataType = TmaxDataTypes.PickItem;
			
			//	Have any nodes been selected?
			if((tmaxSelections != null) && (tmaxSelections.Count == 1))
			{
				//	Is this a value node?
				if(tmaxSelections[0].Type == TmaxPickItemTypes.Value)
				{
					//	The parent node is the target parent
					Debug.Assert(tmaxSelections[0].Parent != null);
					if(tmaxSelections[0].Parent == null) return;
					
					tmaxParent.PickItem = tmaxSelections[0].Parent;
				}
				else
				{
					//	The selected node is the target parent
					tmaxParent.PickItem = tmaxSelections[0];
				}
				
			}// if((tmaxSelections != null) && (tmaxSelections.Count == 1))
			
			//	Add the parameter to define the import format
			tmaxParameters = new CTmaxParameters();
			tmaxParameters.Add(TmaxCommandParameters.ImportFormat, (int)(TmaxImportFormats.AsciiPickList));
			
			//	Fire the command to perform the operation
			FireCommand(TmaxCommands.Import, tmaxParent, tmaxParameters);
			
		}// private void OnCmdImportText()
		
		/// <summary>Toolbar event handler for ExportText command</summary>
		private void OnCmdExportText()
		{
			CTmaxPickItems	tmaxSelections = null;
			CTmaxItems		tmaxSource = null;
			CTmaxParameters	tmaxParameters = null;
			
			//	Get the nodes selected in the tree
			tmaxSelections = m_ctrlTree.GetSelections();
			
			//	Must have at least one node selected
			if((tmaxSelections == null) || (tmaxSelections.Count == 0))
			{
				Warn("You must select at least one pick list in the tree.", null);
				return;
			}
			
			//	Add each of the pick lists to the source collection
			tmaxSource = new CTmaxItems();
			foreach(CTmaxPickItem O in tmaxSelections)
			{
				switch(O.Type)
				{
					case TmaxPickItemTypes.Unknown:
					case TmaxPickItemTypes.MultiLevel:
					case TmaxPickItemTypes.Value:
						break;
						
					case TmaxPickItemTypes.StringList:
					default:
					
						tmaxSource.Add(new CTmaxItem(O));
						break;
						
				}// switch(O.Type)
				
			}// foreach(CTmaxPickItem O in tmaxSelections)
						
			//	Have any value lists been selected?
			if(tmaxSource.Count > 0)
			{
				//	Add the parameter to define the import format
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.ExportFormat, (int)(TmaxExportFormats.AsciiPickList));
			
				//	Fire the command to perform the operation
				FireCommand(TmaxCommands.Export, tmaxSource, tmaxParameters);
			
			}
			else
			{
				Warn("No value lists have been selected to export");
			}
			
		}// private void OnCmdExportText()
		
		/// <summary>Called to get the items required to fire the delete command</summary>
		private CTmaxItem GetCmdDeleteItems(bool bWarn)
		{
			CTmaxPickItems	tmaxSelections = null;
			CTmaxItem		tmaxParent = null;
			
			//	Get the nodes selected in the tree
			tmaxSelections = m_ctrlTree.GetSelections();
			
			//	Must have at least one selection
			if((tmaxSelections == null) || (tmaxSelections.Count == 0))
			{
				if(bWarn == true)
					Warn("You must select one or more nodes in the tree.", null);
				return null;
			}
			
			//	Create an event item to represent the parent 
			tmaxParent = new CTmaxItem();
			tmaxParent.DataType = TmaxDataTypes.PickItem;
			if((tmaxSelections[0].Parent != null) && (tmaxSelections[0].Parent.UniqueId >= 0))
				tmaxParent.PickItem = tmaxSelections[0].Parent;

			//	Add subitems for each of the nodes being deleted
			foreach(CTmaxPickItem O in tmaxSelections)
			{
				//	What type of item is this?
				switch(O.Type)
				{
					case TmaxPickItemTypes.MultiLevel:
					
						//	Must be empty
						if((O.Children != null) && (O.Children.Count > 0))
						{
							if(bWarn == true)
								Warn("Multilevel nodes must be empty before they can be deleted.", null);
							return null;
						}
						break;
						
					case TmaxPickItemTypes.Unknown:
					case TmaxPickItemTypes.Value:
					case TmaxPickItemTypes.StringList:
					default:
						
						break;
						
				}// switch(O.Type)
				
				tmaxParent.SubItems.Add(new CTmaxItem(O));
			
			}// foreach(CTmaxPickItem O in tmaxSelections)
			
			if((tmaxParent != null) && (tmaxParent.SubItems.Count > 0))
				return tmaxParent;
			else
				return null;
			
		}// private CTmaxItem GetCmdDeleteItems(bool bWarn)
		
		#endregion Private Methods

		#region Properties

		/// <summary>This is the application's collection of pick lists</summary>
		public FTI.Shared.Trialmax.CTmaxPickItem PickLists
		{
			get { return m_tmaxPickLists; }
			set { SetPickLists(value); }
		}
		
		#endregion Properties
		
	}// public class CTmaxPickListViewerCtrl : CTmaxBaseCtrl

}// namespace FTI.Trialmax.Controls

