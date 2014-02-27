using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinTree;
using Infragistics.Win.UltraWinToolbars;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;
using FTI.Trialmax.Controls;

namespace FTI.Trialmax.TMVV.Tmvideo
{
	/// <summary>This class creates the view used to display the deposition transcript</summary>
	public class CTmaxVideoTree : FTI.Trialmax.TMVV.Tmvideo.CTmaxVideoView
	{
		#region Constants
		
		/// <summary>View specific command identifiers</summary>
		private enum TmaxVideoTreeCommands
		{
			Invalid = 0,
			New,
			InsertBefore,
			InsertAfter,
			Delete,
			MoveUp,
			MoveDown,
			Sort,
			SetProperties,
			Find,
			ExportDesignations,
			ExportTranscript,
			ExportVideo,
			
		}// private enum TmaxVideoTreeCommands
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_SET_SCRIPT_EX			= ERROR_BASE_VIEW_MAX + 1;
		private const int ERROR_ON_APP_OPENED_EX		= ERROR_BASE_VIEW_MAX + 2;
		private const int ERROR_GET_COMMAND_ITEM_EX		= ERROR_BASE_VIEW_MAX + 3;
		private const int ERROR_CREATE_DESIGNATION_EX	= ERROR_BASE_VIEW_MAX + 4;
		private const int ERROR_ADD_DESIGNATION_EX		= ERROR_BASE_VIEW_MAX + 5;
		private const int ERROR_ON_COMMAND_EX			= ERROR_BASE_VIEW_MAX + 6;
		private const int ERROR_ON_CMD_NEW_EX			= ERROR_BASE_VIEW_MAX + 7;
		private const int ERROR_ON_CMD_DELETE_EX		= ERROR_BASE_VIEW_MAX + 8;
		private const int ERROR_ON_CMD_MOVE_EX			= ERROR_BASE_VIEW_MAX + 9;
		private const int ERROR_ON_CMD_SORT_EX			= ERROR_BASE_VIEW_MAX + 10;
		private const int ERROR_ON_CMD_SET_PROPS_EX		= ERROR_BASE_VIEW_MAX + 11;
		private const int ERROR_SET_SELECTION_EX		= ERROR_BASE_VIEW_MAX + 12;
		private const int ERROR_ON_CMD_FIND_EX			= ERROR_BASE_VIEW_MAX + 13;
		private const int ERROR_ON_CMD_EXPORT_EX		= ERROR_BASE_VIEW_MAX + 13;
			
		#endregion Constants
		
		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components;

		/// <summary>Child tree control used to render the designations</summary>
		private FTI.Trialmax.Controls.CTmaxMediaTreeCtrl m_ctrlTmaxTree;

		/// <summary>Image list bound to the child tree control</summary>
		private System.Windows.Forms.ImageList m_ctrlTreeImages;

		/// <summary>Collection used to sort the designations</summary>
		private CTmaxMediaTreeNodes m_tmaxDisplaySorter = new CTmaxMediaTreeNodes();
		
		/// <summary>Tree control draw filter</summary>
		protected FTI.Trialmax.Controls.CTmaxBaseTreeFilter m_tmaxTreeFilter = new CTmaxBaseTreeFilter();
		
		/// <summary>Default docking area created by toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxVideoTree_Toolbars_Dock_Area_Left;
		
		/// <summary>Default docking area created by toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxVideoTree_Toolbars_Dock_Area_Right;
		
		/// <summary>Default docking area created by toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxVideoTree_Toolbars_Dock_Area_Top;
		
		/// <summary>Default docking area created by toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmaxVideoTree_Toolbars_Dock_Area_Bottom;
		
		/// <summary>Image list bound to the toolbar manager</summary>
		private System.Windows.Forms.ImageList m_ctrlToolbarImages;
		
		/// <summary>The control's toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager m_ctrlToolbarManager;
		
		/// <summary>The time required to display the Paused icons for designations</summary>
		private double m_dPauseThreshold = 0;
		
		/// <summary>Local flag to inhibit processing of toolbar manager events</summary>
		private bool m_bIgnoreEvents = false;
		
		/// <summary>Local collection to store nodes being dragged by the user</summary>
		private CTmaxMediaTreeNodes m_tmaxDragNodes = null;
		
		/// <summary>Local member to keep track of the active drop node</summary>
		private CTmaxMediaTreeNode m_tmaxDropNode = null;
		
		/// <summary>Local member to keep track of the drop position</summary>
		private TmaxTreePositions m_eDropPosition = TmaxTreePositions.None;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxVideoTree() : base()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			m_tmaxDisplaySorter.KeepSorted = false;
			m_tmaxDisplaySorter.Sorter = new CTmaxVideoTreeSorter();
			
			if(m_tmaxTreeFilter != null)
			{
				m_tmaxTreeFilter.QueryStateAllowed += new CTmaxBaseTreeFilter.QueryStateAllowedHandler(this.OnQueryStateAllowed);
			}
			
		}// public CTmaxVideoTree() : base()

		/// <summary>This function is called to clear the tree</summary>
		public void Clear()
		{
			try
			{
				if((m_ctrlTmaxTree != null) && (m_ctrlTmaxTree.Nodes != null))
					m_ctrlTmaxTree.Nodes.Clear();
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Clear", Ex);
			}
			
		}// public void Clear()
		
		/// <summary>This method is called by the application when it is about to terminate</summary>
		public override void Terminate(CXmlIni xmlINI)
		{
			//	Clear the grid
			Clear();
			
			//	Do the base class cleanup
			base.Terminate(xmlINI);
			
		}// public override void Terminate(CXmlIni xmlINI)
		
		/// <summary>This function is called when the application options object changes</summary>
		public override void OnAppOptionsChanged()
		{
			if(this.AppOptions != null)
				SetPauseThreshold(this.AppOptions.PauseThreshold);

		}// public override void OnAppOptionsChanged()
		
		/// <summary>This function is called by the application when the user is is finished setting the preferences</summary>
		/// <param name="bApplied">true if changes have been applied</param>
		public override void OnAppAfterSetPreferences(bool bApplied)
		{
			//	Has the user changed the values?
			if((bApplied == true) && (this.AppOptions != null))
			{
				SetPauseThreshold(this.AppOptions.PauseThreshold);

			}// if((bApplied == true) && (this.AppOptions != null))
		
		}// public override void OnAppAfterSetPreferences(bool bApplied)
		
		/// <summary>This method handles all Hotkey notifications from the application</summary>
		/// <param name="eHotkey">The enumerated hotkey</param>
		/// <returns>true if processed by the view</returns>
		public override bool OnAppHotkey(TmaxHotkeys eHotkey)
		{
			CTmaxMediaTreeNodes			tmaxNodes = null;
			TmaxVideoTreeCommands	eCommand = TmaxVideoTreeCommands.Invalid;

			//	Get the current selections
			tmaxNodes = GetSelections(true);
				
			//	Which hotkey has been pressed?
			switch(eHotkey)
			{
				case TmaxHotkeys.Find:
				
					eCommand = TmaxVideoTreeCommands.Find;
					break;
					
				default:
				
					break;
			}
		
			//	Did this hotkey translate to a command?
			if(eCommand != TmaxVideoTreeCommands.Invalid)
			{
				//	Is this command visible?
				if(GetCommandVisible(eCommand) == true)
				{
					//	Is this command enabled
					if(GetCommandEnabled(eCommand, tmaxNodes) == true)
					{
						OnCommand(eCommand);
					}
					
				}

			}// if(eCommand != TreePaneCommands.Invalid)
			
			return (eCommand != TmaxVideoTreeCommands.Invalid);
			
		}// public override bool OnAppHotkey(TmaxHotkeys eHotkey)
		
		/// <summary>This function is called by the application when the user opens a new XML script</summary>
		/// <param name="xmlScript">The new XML script</param>
		/// <returns>true if successful</returns>
		public override bool OnAppOpened(CXmlScript xmlScript)
		{
			bool bSuccessful = false;
			
			try
			{
				if(xmlScript != null)
				{
					bSuccessful = SetScript(xmlScript);
				}
				else
				{
					Clear();
					
					bSuccessful = true;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnAppOpened", m_tmaxErrorBuilder.Message(ERROR_ON_APP_OPENED_EX), Ex);
			}
			
			return bSuccessful;
			
		}// public override bool OnAppOpened(CXmlScript xmlScript)
		
		/// <summary>This method selects the specified designation in the tree</summary>
		/// <param name="xmlDesignation">The designation to be selected</param>
		///	<param name="bActivate">true to notify the application to activate the designation</param>
		/// <returns>true if successful</returns>
		public bool SetSelection(CXmlDesignation xmlDesignation, bool bActivate)
		{
			CTmaxMediaTreeNode	tmaxNode = null;
			bool			bSuccessful = false;
			
			try
			{
				//	Get the node bound to the specified designation
				if((tmaxNode = GetNode(xmlDesignation)) == null) 
					return false;
			
				//	Prevent event processing while we set the selection
				m_bIgnoreEvents = true;
				
				//	Select the node in the tree
				m_ctrlTmaxTree.SetSelection(tmaxNode);
				m_ctrlTmaxTree.EnsureVisible(tmaxNode);
					
				//	Notify the application?
				if(bActivate == true)
					FireCommand(TmaxVideoCommands.Activate, tmaxNode, false);
				
				bSuccessful = true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetSelection", m_tmaxErrorBuilder.Message(ERROR_SET_SELECTION_EX, xmlDesignation != null ? xmlDesignation.Name : "NULL"), Ex);
			}
			finally
			{
				m_bIgnoreEvents = false;
			}
			
			return bSuccessful;
			
		}// public bool SetSelection(CXmlDesignation xmlDesignation, bool bActivate)
		
		/// <summary>This method allows the application to activate a designation</summary>
		/// <param name="tmaxItem">The item to be activated</param>
		///	<param name="bActivate">true to notify the application to activate the designation</param>
		/// <returns>true if successful</returns>
		public bool SetSelection(CTmaxItem tmaxItem, bool bActivate)
		{
			//	There should be a designation to be activated
			if((tmaxItem != null) && (tmaxItem.XmlDesignation != null))
				return SetSelection(tmaxItem.XmlDesignation, bActivate);
			else
				return false;	
			
		}// public bool SetSelection(CTmaxItem tmaxItem, bool bActivate)
		
		/// <summary>This method is called by the application when it processes an Update command event</summary>
		/// <param name="tmaxItem">The item that represents the parent element</param>
		/// <param name="eView">The view that deleted the new elements</param>
		/// <returns>true if successful</returns>
		public override bool OnTmaxVideoLoadResult(CTmaxVideoResult tmaxResult)
		{
			//	Is a designation bound to the result?
			if(tmaxResult.XmlDesignation != null)
			{
				//	Activate this designation
				SetSelection(tmaxResult.XmlDesignation, false);
			}
			
			return true;			
		
		}// public override bool OnTmaxVideoLoadResult(CTmaxVideoResult tmaxResult)
		
		/// <summary>This method is called by the application when it processes a Select command event</summary>
		/// <param name="tmaxItem">The item that represents the parent element</param>
		/// <param name="eView">The view that added the new elements</param>
		/// <returns>true if successful</returns>
		public override bool OnTmaxVideoAdd(CTmaxItem tmaxItem, TmaxVideoViews eView)
		{
			//	New designations are stored in the SubItems collection
			if((tmaxItem.SubItems != null) && (tmaxItem.SubItems.Count > 0))
			{
				foreach(CTmaxItem O in tmaxItem.SubItems)
				{
					if(O.XmlDesignation != null)
						Add(O.XmlDesignation);
				}
				
				//	Make sure the nodes are in the correct order
				Sort(m_ctrlTmaxTree.Nodes);
			
			}
			
			return true;
			
		}// public virtual bool OnTmaxVideoAdd(CTmaxItem tmaxItem, int iId)
		
		/// <summary>This method is called by the application when it processes a Delete command event</summary>
		/// <param name="tmaxItem">The item that represents the parent element</param>
		/// <param name="eView">The view that deleted the new elements</param>
		/// <returns>true if successful</returns>
		public override bool OnTmaxVideoDelete(CTmaxItem tmaxItem, TmaxVideoViews eView)
		{
			CTmaxMediaTreeNode tmaxNode = null;
			
			//	Designations are stored in the SubItems collection
			if((tmaxItem.SubItems != null) && (tmaxItem.SubItems.Count > 0))
			{
				foreach(CTmaxItem O in tmaxItem.SubItems)
				{
					if(O.XmlDesignation != null)
					{
						if((tmaxNode = GetNode(O.XmlDesignation)) != null)
							OnDeleted(tmaxNode);
					}
					
				}// foreach(CTmaxItem O in tmaxItem.SubItems)
			
			}
			
			return true;
		}
		
		/// <summary>This method is called by the application when it processes a Reorder command event</summary>
		/// <param name="tmaxItem">The item that represents the parent element</param>
		/// <param name="eView">The view that deleted the new elements</param>
		/// <returns>true if successful</returns>
		public override bool OnTmaxVideoReorder(CTmaxItem tmaxItem, TmaxVideoViews eView)
		{
			//	Sort the designations
			if((m_ctrlTmaxTree != null) && (m_ctrlTmaxTree.Nodes != null))
				Sort(m_ctrlTmaxTree.Nodes);

			return true;
		}
		
		/// <summary>This method is called by the application when it processes an Update command event</summary>
		/// <param name="tmaxItem">The item that represents the parent element</param>
		/// <param name="eView">The view that deleted the new elements</param>
		/// <returns>true if successful</returns>
		public override bool OnTmaxVideoUpdate(CTmaxItem tmaxItem, TmaxVideoViews eView)
		{
			CTmaxMediaTreeNode	tmaxNode = null;
			int				iImage = 0;
			
			if((tmaxItem != null) && (tmaxItem.XmlDesignation != null))
			{
				if((tmaxNode = GetNode(tmaxItem.XmlDesignation)) != null)
				{
					tmaxNode.Text = GetText(tmaxItem.XmlDesignation);
					iImage = GetImageIndex(tmaxNode);
					if(iImage != (int)(tmaxNode.Override.NodeAppearance.Image))
						tmaxNode.Override.NodeAppearance.Image = iImage;
				}
				
			}// if((tmaxItem != null) && (tmaxItem.XmlDesignation != null))
			
			return true;
		
		}// public override bool OnTmaxVideoUpdate(CTmaxItem tmaxItem, int iId)
		
		/// <summary>This method is called by the application to get a list of items that represent the current selections for a Find command</summary>
		/// <returns>The items that represent the current selections</returns>
		public override CTmaxItems GetSearchItems()
		{
			CTmaxMediaTreeNodes tmaxNodes = null;
			
			//	Get the current selections
			if((tmaxNodes = GetSelections(true)) != null)
				return GetCommandItems(tmaxNodes, false);
			else
				return null;
		
		}// public virtual CTmaxItems GetSearchItems()
		
		/// <summary>This method is called by the application to get a text representation of the total time required for the script</summary>
		/// <returns>The text representation of playback time</returns>
		public string GetDurationAsString()
		{
			string	strText = "";
			double	dSeconds = 0;

			if(m_xmlScript != null)
			{
				if((dSeconds = m_xmlScript.GetDuration()) > 0)
				{
					strText = SecondsToString(dSeconds);
				}
				
			}
			
			return strText;
			
		}// public string GetDuration()
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called the when the window gets created</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Perform the base class processing
			base.OnLoad (e);
		
			// Attach the draw filter to the tree
			m_ctrlTmaxTree.DrawFilter = m_tmaxTreeFilter;
			
		}// protected override void OnLoad(EventArgs e)

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			//	Let the base class add its strings first
			base.SetErrorStrings();
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the active script.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised when the application opened a new script.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to get a command item for this node: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while creating a new node for the XML designation object.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add the designation to the tree.");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to process the view command: command = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add new designations to the script.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to delete designations in the script.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to move the selected designation.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to sort the designations in the script.");
		
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the script's properties.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the current selection: designation = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to search the selected designations.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to export the selected designations.");

		}// protected override void SetErrorStrings()

		/// <summary>Used by form designer to lay out child controls</summary> 
		protected override void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Infragistics.Win.UltraWinTree.UltraTreeColumnSet ultraTreeColumnSet1 = new Infragistics.Win.UltraWinTree.UltraTreeColumnSet();
			Infragistics.Win.UltraWinTree.Override _override1 = new Infragistics.Win.UltraWinTree.Override();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CTmaxVideoTree));
			Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("MainToolbar");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ContextMenu");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ContextMenu");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SetProperties");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Find");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("New");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertBefore");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertAfter");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MoveUp");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MoveDown");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Sort");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool3 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ExportMenu");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("New");
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertBefore");
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertAfter");
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MoveUp");
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MoveDown");
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Sort");
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete");
			Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SetProperties");
			Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Find");
			Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool4 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ExportMenu");
			Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportDesignations");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportTranscript");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportVideo");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool22 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportDesignations");
			Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool23 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportTranscript");
			Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool24 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportVideo");
			Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
			this.m_ctrlTmaxTree = new FTI.Trialmax.Controls.CTmaxMediaTreeCtrl();
			this.m_ctrlTreeImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlToolbarImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlToolbarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
			this._CTmaxVideoTree_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTmaxVideoTree_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTmaxVideoTree_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTmaxVideoTree_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlTmaxTree)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlToolbarManager)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlTmaxTree
			// 
			this.m_ctrlTmaxTree.AllowDrop = true;
			this.m_ctrlTmaxTree.ColumnSettings.RootColumnSet = ultraTreeColumnSet1;
			this.m_ctrlToolbarManager.SetContextMenuUltra(this.m_ctrlTmaxTree, "ContextMenu");
			this.m_ctrlTmaxTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlTmaxTree.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.m_ctrlTmaxTree.FullRowSelect = true;
			this.m_ctrlTmaxTree.HideSelection = false;
			this.m_ctrlTmaxTree.ImageList = this.m_ctrlTreeImages;
			this.m_ctrlTmaxTree.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_ctrlTmaxTree.Location = new System.Drawing.Point(0, 25);
			this.m_ctrlTmaxTree.Name = "m_ctrlTmaxTree";
			_override1.SelectionType = Infragistics.Win.UltraWinTree.SelectType.ExtendedAutoDrag;
			this.m_ctrlTmaxTree.Override = _override1;
			this.m_ctrlTmaxTree.Size = new System.Drawing.Size(150, 125);
			this.m_ctrlTmaxTree.TabIndex = 0;
			this.m_ctrlTmaxTree.DragOver += new System.Windows.Forms.DragEventHandler(this.OnDragOver);
			this.m_ctrlTmaxTree.SelectionDragStart += new System.EventHandler(this.OnDragStart);
			this.m_ctrlTmaxTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
			this.m_ctrlTmaxTree.AfterSelect += new Infragistics.Win.UltraWinTree.AfterNodeSelectEventHandler(this.OnUltraAfterSelect);
			this.m_ctrlTmaxTree.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.OnQueryContinueDrag);
			this.m_ctrlTmaxTree.DragLeave += new System.EventHandler(this.OnDragLeave);
			this.m_ctrlTmaxTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
			// 
			// m_ctrlTreeImages
			// 
			this.m_ctrlTreeImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlTreeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlTreeImages.ImageStream")));
			this.m_ctrlTreeImages.TransparentColor = System.Drawing.Color.Magenta;
			// 
			// m_ctrlToolbarImages
			// 
			this.m_ctrlToolbarImages.ImageSize = new System.Drawing.Size(16, 16);
			this.m_ctrlToolbarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlToolbarImages.ImageStream")));
			this.m_ctrlToolbarImages.TransparentColor = System.Drawing.Color.Magenta;
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
			buttonTool3.InstanceProps.IsFirstInGroup = true;
			buttonTool6.InstanceProps.IsFirstInGroup = true;
			buttonTool7.InstanceProps.IsFirstInGroup = true;
			buttonTool9.InstanceProps.IsFirstInGroup = true;
			popupMenuTool3.InstanceProps.IsFirstInGroup = true;
			popupMenuTool2.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																							   buttonTool1,
																							   buttonTool2,
																							   buttonTool3,
																							   buttonTool4,
																							   buttonTool5,
																							   buttonTool6,
																							   buttonTool7,
																							   buttonTool8,
																							   buttonTool9,
																							   popupMenuTool3});
			appearance1.Image = 0;
			buttonTool10.SharedProps.AppearancesSmall.Appearance = appearance1;
			buttonTool10.SharedProps.Caption = "New ...";
			appearance2.Image = 2;
			buttonTool11.SharedProps.AppearancesSmall.Appearance = appearance2;
			buttonTool11.SharedProps.Caption = "Insert Before ...";
			appearance3.Image = 1;
			buttonTool12.SharedProps.AppearancesSmall.Appearance = appearance3;
			buttonTool12.SharedProps.Caption = "Insert After ...";
			appearance4.Image = 3;
			buttonTool13.SharedProps.AppearancesSmall.Appearance = appearance4;
			buttonTool13.SharedProps.Caption = "Move Up";
			appearance5.Image = 4;
			buttonTool14.SharedProps.AppearancesSmall.Appearance = appearance5;
			buttonTool14.SharedProps.Caption = "Move Down";
			appearance6.Image = 5;
			buttonTool15.SharedProps.AppearancesSmall.Appearance = appearance6;
			buttonTool15.SharedProps.Caption = "Sort Designations";
			appearance7.Image = 6;
			buttonTool16.SharedProps.AppearancesSmall.Appearance = appearance7;
			buttonTool16.SharedProps.Caption = "Delete";
			appearance8.Image = 7;
			buttonTool17.SharedProps.AppearancesSmall.Appearance = appearance8;
			buttonTool17.SharedProps.Caption = "Set Properties ...";
			appearance9.Image = 8;
			buttonTool18.SharedProps.AppearancesSmall.Appearance = appearance9;
			buttonTool18.SharedProps.Caption = "Find ...";
			appearance10.Image = 10;
			popupMenuTool4.SharedProps.AppearancesSmall.Appearance = appearance10;
			popupMenuTool4.SharedProps.Caption = "Export";
			popupMenuTool4.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																							   buttonTool19,
																							   buttonTool20,
																							   buttonTool21});
			appearance11.Image = 9;
			buttonTool22.SharedProps.AppearancesSmall.Appearance = appearance11;
			buttonTool22.SharedProps.Caption = "Designations ...";
			appearance12.Image = 11;
			buttonTool23.SharedProps.AppearancesSmall.Appearance = appearance12;
			buttonTool23.SharedProps.Caption = "Script Text ...";
			appearance13.Image = 12;
			buttonTool24.SharedProps.AppearancesSmall.Appearance = appearance13;
			buttonTool24.SharedProps.Caption = "Video ...";
			this.m_ctrlToolbarManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																										  popupMenuTool2,
																										  buttonTool10,
																										  buttonTool11,
																										  buttonTool12,
																										  buttonTool13,
																										  buttonTool14,
																										  buttonTool15,
																										  buttonTool16,
																										  buttonTool17,
																										  buttonTool18,
																										  popupMenuTool4,
																										  buttonTool22,
																										  buttonTool23,
																										  buttonTool24});
			this.m_ctrlToolbarManager.AfterToolCloseup += new Infragistics.Win.UltraWinToolbars.ToolDropdownEventHandler(this.OnUltraAfterPopup);
			this.m_ctrlToolbarManager.BeforeToolDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventHandler(this.OnUltraBeforeDropDown);
			this.m_ctrlToolbarManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.OnUltraToolClick);
			// 
			// _CTmaxVideoTree_Toolbars_Dock_Area_Left
			// 
			this._CTmaxVideoTree_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmaxVideoTree_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
			this._CTmaxVideoTree_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
			this._CTmaxVideoTree_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxVideoTree_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 25);
			this._CTmaxVideoTree_Toolbars_Dock_Area_Left.Name = "_CTmaxVideoTree_Toolbars_Dock_Area_Left";
			this._CTmaxVideoTree_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 125);
			this._CTmaxVideoTree_Toolbars_Dock_Area_Left.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// _CTmaxVideoTree_Toolbars_Dock_Area_Right
			// 
			this._CTmaxVideoTree_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmaxVideoTree_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
			this._CTmaxVideoTree_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
			this._CTmaxVideoTree_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxVideoTree_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(150, 25);
			this._CTmaxVideoTree_Toolbars_Dock_Area_Right.Name = "_CTmaxVideoTree_Toolbars_Dock_Area_Right";
			this._CTmaxVideoTree_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 125);
			this._CTmaxVideoTree_Toolbars_Dock_Area_Right.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// _CTmaxVideoTree_Toolbars_Dock_Area_Top
			// 
			this._CTmaxVideoTree_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmaxVideoTree_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
			this._CTmaxVideoTree_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
			this._CTmaxVideoTree_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxVideoTree_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
			this._CTmaxVideoTree_Toolbars_Dock_Area_Top.Name = "_CTmaxVideoTree_Toolbars_Dock_Area_Top";
			this._CTmaxVideoTree_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(150, 25);
			this._CTmaxVideoTree_Toolbars_Dock_Area_Top.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// _CTmaxVideoTree_Toolbars_Dock_Area_Bottom
			// 
			this._CTmaxVideoTree_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmaxVideoTree_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
			this._CTmaxVideoTree_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
			this._CTmaxVideoTree_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmaxVideoTree_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 150);
			this._CTmaxVideoTree_Toolbars_Dock_Area_Bottom.Name = "_CTmaxVideoTree_Toolbars_Dock_Area_Bottom";
			this._CTmaxVideoTree_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(150, 0);
			this._CTmaxVideoTree_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// CTmaxVideoTree
			// 
			this.Controls.Add(this.m_ctrlTmaxTree);
			this.Controls.Add(this._CTmaxVideoTree_Toolbars_Dock_Area_Left);
			this.Controls.Add(this._CTmaxVideoTree_Toolbars_Dock_Area_Right);
			this.Controls.Add(this._CTmaxVideoTree_Toolbars_Dock_Area_Top);
			this.Controls.Add(this._CTmaxVideoTree_Toolbars_Dock_Area_Bottom);
			this.Name = "CTmaxVideoTree";
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlTmaxTree)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlToolbarManager)).EndInit();
			this.ResumeLayout(false);

		}// protected void InitializeComponent()
		
		/// <summary>Clean up</summary>
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
		
		/// <summary>This method traps the MouseDown event</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">System mouse event arguments</param>
		protected virtual void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			CTmaxMediaTreeNode tmaxNode = null;
			
			//	Don't do anything if we are not over a node
			if((tmaxNode = m_ctrlTmaxTree.GetNode(e.X, e.Y)) == null) return;
				
			//	Is this the right button?
			if(e.Button == MouseButtons.Right)
			{
				//	Don't do anything if the user is pressing the Control key
				if((FTI.Shared.Win32.User.GetKeyState(FTI.Shared.Win32.User.VK_CONTROL) & 0x8000) != 0) return;
				
				//	Don't do anything if pressing the Shift key
				if((FTI.Shared.Win32.User.GetKeyState(FTI.Shared.Win32.User.VK_SHIFT) & 0x8000) != 0) return;
				
				//	Make sure the node under the mouse is selected
				if(tmaxNode.Selected == false)
				{
					m_ctrlTmaxTree.SetSelection(tmaxNode);
				}

			}
		
		}// OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)

		#endregion Protected Methods
		
		#region Private Methods
	
		/// <summary>This method is called to add the specified designation to the tree</summary>
		/// <param name="xmlDesignation">The designation to be added</param>
		/// <returns>The new node if successful</returns>
		private CTmaxMediaTreeNode Add(CXmlDesignation xmlDesignation)
		{
			CTmaxMediaTreeNode tmaxNode = null;
			
			try
			{
				if((tmaxNode = CreateNode(xmlDesignation)) != null)
				{
					//	Add to the root collection
					m_ctrlTmaxTree.Nodes.Add(tmaxNode);
					
					//	Set the property values AFTER adding to the tree
					tmaxNode.SetProperties(GetImageIndex(tmaxNode));
				
				}// if((tmaxNode = CreateNode(xmlDesignation)) != null)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_DESIGNATION_EX), Ex);
			}
			
			return tmaxNode;
			
		}// private CTmaxMediaTreeNode Add(CXmlDesignation xmlDesignation)
		
		/// <summary>This method is called to create a new node for the specified designation</summary>
		/// <param name="xmlDesignation">The designation to be bound to the node</param>
		/// <returns>The new node if successful</returns>
		private CTmaxMediaTreeNode CreateNode(CXmlDesignation xmlDesignation)
		{
			CTmaxMediaTreeNode tmaxNode = null;
			
			try
			{
				tmaxNode = new CTmaxMediaTreeNode(GetText(xmlDesignation));
				
				tmaxNode.MediaType  = TmaxMediaTypes.Designation;
				tmaxNode.MediaLevel = TmaxMediaLevels.Tertiary;
				tmaxNode.XmlObject  = xmlDesignation;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateNode", m_tmaxErrorBuilder.Message(ERROR_CREATE_DESIGNATION_EX), Ex);
			}
			
			return tmaxNode;
			
		}// private CTmaxMediaTreeNode CreateNode(CXmlDesignation xmlDesignation)
		
		/// <summary>This method is called to get the text for the node bound to the specified designation</summary>
		/// <param name="xmlDesignation">The designation bound to the tree node</param>
		/// <returns>The text used to represent the node</returns>
		private string GetText(CXmlDesignation xmlDesignation)
		{
			string strText = "";
			string strRange = "";
			
			try
			{
				strRange = String.Format("{0} - {1}",
					CTmaxToolbox.PLToString(xmlDesignation.FirstPL),
					CTmaxToolbox.PLToString(xmlDesignation.LastPL));

				strText = String.Format("{0,-15} [{1}]", strRange,
										SecondsToString(xmlDesignation.Stop - xmlDesignation.Start));
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetText", Ex);
			}
			
			return strText;
			
		}// private string GetText(CXmlDesignation xmlDesignation)
		
		/// <summary>This method is called to convert the decimal seconds to a string</summary>
		/// <param name="dSeconds">The decimal seconds</param>
		/// <returns>The string representation</returns>
		private string SecondsToString(double dSeconds)
		{
			try
			{
				System.TimeSpan Pos = System.TimeSpan.FromSeconds(dSeconds);

				//	Round up if necessary
				if(Pos.Milliseconds >= 995)
				{
					dSeconds = (double)(System.Convert.ToInt32(dSeconds));
					dSeconds += 1;
					Pos = System.TimeSpan.FromSeconds(dSeconds);
				}
				
				if(dSeconds >= 3600)
					return String.Format("{0}:{1:00}:{2:00}.{3}", Pos.Hours, Pos.Minutes, Pos.Seconds, (int)(Pos.Milliseconds / 100.0));
				else
					return String.Format("{0:00}:{1:00}.{2}", Pos.Minutes, Pos.Seconds, (int)(Pos.Milliseconds / 100.0));
			}
			catch
			{
				return dSeconds.ToString();
			}
			
		}// private string SecondsToString(double dSeconds)
		
		/// <summary>This method is called to set the active script</summary>
		/// <param name="xmlScript">The script to be displayed</param>
		/// <returns>true if successful</returns>
		private bool SetScript(CXmlScript xmlScript)
		{
			bool bSuccessful = false;
			
			//	Clear the existing script
			Clear();
			
			//	Is the caller attempting to clear the view?
			if(xmlScript == null) return true;
			
			try
			{
				//	Update the class reference
				m_xmlScript = xmlScript;
				
				//	Add each of the designations
				if((m_xmlScript.XmlDesignations != null) && (m_xmlScript.XmlDesignations.Count > 0))
				{
					foreach(CXmlDesignation O in m_xmlScript.XmlDesignations)
						Add(O);
				}
				
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetScript", m_tmaxErrorBuilder.Message(ERROR_SET_SCRIPT_EX), Ex);
			}
			
			return bSuccessful;
			
		}// private bool SetScript(CXmlScript xmlScript)
		
		/// <summary>This method is called to get the index of the image appropriate for the specified node</summary>
		/// <param name="tmaxNode">The node associated with the desired image</param>
		/// <returns>The index of the associated image</returns>
		private int GetImageIndex(CTmaxMediaTreeNode tmaxNode)
		{
			CXmlDesignation	xmlDesignation = null;
			CXmlLink		xmlLink = null;
			int				iIndex = 0;
			
			try
			{
				//	Get the appropriate XML object
				if(tmaxNode.XmlObject != null)
				{
					switch(tmaxNode.MediaType)
					{
						case TmaxMediaTypes.Designation:
							xmlDesignation = (CXmlDesignation)(tmaxNode.XmlObject);
							break;
							
						case TmaxMediaTypes.Link:
							xmlLink = (CXmlLink)(tmaxNode.XmlObject);
							break;
							
						default:
							m_tmaxEventSource.FireDiagnostic(this, "GetImageIndex", "Invalid XML object");
							break;
							
					}// switch(tmaxNode.MediaType)
					
				}// if(tmaxNode.XmlObject != null)
				
				//	Is this node bound to an XML designation?
				if(xmlDesignation != null)
				{
					//	Should we use one of the Pause images?
					if((m_dPauseThreshold > 0) && (xmlDesignation.GetMaxLineTime() > m_dPauseThreshold))
					{
						if(xmlDesignation.StartTuned == true)
						{
							if(xmlDesignation.StopTuned == true)
								iIndex = 6;
							else
								iIndex = 8;
						}
						else
						{
							if(xmlDesignation.StopTuned == true)
								iIndex = 7;
							else
								iIndex = 5;
						}
						
					}
					else
					{
						if(xmlDesignation.StartTuned == true)
						{
							if(xmlDesignation.StopTuned == true)
								iIndex = 2;
							else
								iIndex = 4;
						}
						else
						{
							if(xmlDesignation.StopTuned == true)
								iIndex = 3;
							else
								iIndex = 1;
						}
						
					}				
				
				}
				else if(xmlLink != null)
				{
					if(xmlLink.StartTuned == true)
						iIndex = 10;
					else
						iIndex = 9;
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetImageIndex", Ex);
			}
			
			return iIndex;
			
		}// private int GetImageIndex(CTmaxMediaTreeNode tmaxNode)
		
		/// <summary>This method handles events fired by the tree control when it has selected a new node(s)</summary>
		/// <param name="sender">The tree firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnUltraAfterSelect(object sender, Infragistics.Win.UltraWinTree.SelectEventArgs e)
		{
			/*
			if(e.NewSelections != null && e.NewSelections.Count > 0)
				FireDiagnostic("OnUltraAfterSelect", "After Select: " + e.NewSelections[0].Text + " of " + e.NewSelections.Count.ToString());
			else
				FireDiagnostic("OnUltraAfterSelect", "After Select: null");
			*/

			if(m_bIgnoreEvents == false)
			{
				if((e.NewSelections != null) && (e.NewSelections.Count > 0))
				{
					if(e.NewSelections.Count == 1)
					{
						FireCommand(TmaxVideoCommands.Activate, (CTmaxMediaTreeNode)e.NewSelections[0], false);
					}
					
				}
			
			}

		}// private void OnUltraAfterSelect(object sender, Infragistics.Win.UltraWinTree.SelectEventArgs e)

		/// <summary>This method is called to get a TrialMax event item based on the specified tree node</summary>
		/// <param name="tmaxNode">The node being passed with the event arguments</param>
		/// <param name="bChildren">true to include child nodes</param>
		/// <returns>The fully initialized event item</returns>
		private CTmaxItem GetCommandItem(CTmaxMediaTreeNode tmaxNode, bool bChildren)
		{
			CTmaxItem			tmaxItem = null;
			CTmaxItem			tmaxSubItem = null;
			TreeNodesCollection		aChildren = null;
			
			try
			{
				//	Allocate a new item
				tmaxItem = new CTmaxItem();
			
				if(tmaxNode != null)
				{
					//	Set the item properties
					tmaxItem.MediaType = tmaxNode.MediaType;
					
					if(tmaxNode.MediaType == TmaxMediaTypes.Link)
					{
						tmaxItem.XmlLink = (CXmlLink)(tmaxNode.XmlObject);
						Debug.Assert(tmaxItem.XmlLink != null);
						
						//	Set the parent designation
						Debug.Assert(tmaxNode.Parent != null);
						if(tmaxNode.Parent != null)
							tmaxItem.XmlDesignation = (CXmlDesignation)(((CTmaxMediaTreeNode)(tmaxNode.Parent)).XmlObject);
						Debug.Assert(tmaxItem.XmlDesignation != null);
					}
					else
					{
						tmaxItem.XmlDesignation = (CXmlDesignation)(tmaxNode.XmlObject);
						Debug.Assert(tmaxItem.XmlDesignation != null);
					}
					
					//	Only one script
					tmaxItem.XmlScript = m_xmlScript;
					
					//	Set the parent item
					//
					//	NOTE:	This allows us to provide the full tree path to the
					//			item being acted on
					if(tmaxNode.Parent != null)
					{
						try { tmaxItem.ParentItem = GetCommandItem((CTmaxMediaTreeNode)(tmaxNode.Parent), false); }
						catch { }
					}
				
				}// if(tmaxNode != null)

				//	Are we supposed to add items for the children?
				if(bChildren == true)
				{
					//	Do we need to use the root collection?
					if(tmaxNode == null)
						aChildren = m_ctrlTmaxTree.Nodes;
					else
						aChildren = tmaxNode.Nodes;
						
					if((aChildren != null) && (aChildren.Count > 0))
					{
						//	Add a sub item for each child node
						foreach(CTmaxMediaTreeNode O in aChildren)
						{
							if((tmaxSubItem = GetCommandItem(O, true)) != null)
							{
								tmaxItem.SubItems.Add(tmaxSubItem);
							}
						}
					
					}// if((aChildren != null) && (aChildren.Count > 0))
				
				}// if(bChildren == true)
			
				return tmaxItem;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetCommandItem", m_tmaxErrorBuilder.Message(ERROR_GET_COMMAND_ITEM_EX, (tmaxNode != null ? tmaxNode.FullPath : "NULL")), Ex);
				return null;
			}
			
		}// private CTmaxItem GetCommandItem(CTmaxMediaTreeNode tmaxNode, bool bChildren)
					
		/// <summary>This method is called to create and populate a TrialMax event items collection</summary>
		/// <param name="tmaxNodes">The nodes being passed with the event arguments</param>
		/// <param name="bChildren">true to include child nodes</param>
		/// <returns>The new event items collection</returns>
		private CTmaxItems GetCommandItems(CTmaxMediaTreeNodes tmaxNodes, bool bChildren)
		{
			CTmaxItems tmaxItems = null;
			
			if((tmaxNodes != null) && (tmaxNodes.Count > 0))
			{
				try
				{
					tmaxItems = new CTmaxItems();
					
					foreach(CTmaxMediaTreeNode tmaxNode in tmaxNodes)
					{
						tmaxItems.Add(GetCommandItem(tmaxNode, bChildren));
					}
					
				}
				catch
				{
				
				}
				
			}

			return tmaxItems;
			
		}// private CTmaxItems GetCommandItems(CTmaxMediaTreeNodes tmaxNodes, bool bChildren)
			
		/// <summary>This method is called to locate the node bound to the specified designation</summary>
		/// <param name="xmlDesignation">The designation bound to the node</param>
		/// <returns>The node bound to the designation</returns>
		private CTmaxMediaTreeNode GetNode(CXmlDesignation xmlDesignation)
		{
			if((m_ctrlTmaxTree != null) && (m_ctrlTmaxTree.Nodes != null))
			{
				foreach(CTmaxMediaTreeNode O in m_ctrlTmaxTree.Nodes)
				{
					if(ReferenceEquals(O.XmlObject, xmlDesignation) == true)
						return O;
				}
				
			}
			
			//	Must not have found the desired node
			return null;
			
		}// private CTmaxMediaTreeNode GetNode(CXmlDesignation xmlDesignation)
		
		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxNode">The node being passed with the command arguments</param>
		/// <param name="bChildren">true to include the node's children</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		/// <returns>The command argument object</returns>
		private CTmaxVideoArgs FireCommand(TmaxVideoCommands eCommand, CTmaxMediaTreeNode tmaxNode, bool bChildren, CTmaxParameters tmaxParameters)
		{
			CTmaxItem	tmaxItem = null;
			
			//	Create an event item
			if((tmaxItem = GetCommandItem(tmaxNode, bChildren)) == null)
			{
				return null;
			}
			else
			{
				return FireCommand(eCommand, tmaxItem, tmaxParameters);
			}
					
		}//	FireCommand(TmaxVideoCommands eCommand, CTmaxMediaTreeNode tmaxNode, CTmaxParameters tmaxParameters)

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxNode">The node being passed with the command arguments</param>
		/// <param name="bChildren">True to include the node's children</param>
		/// <returns>The command argument object</returns>
		private CTmaxVideoArgs FireCommand(TmaxVideoCommands eCommand, CTmaxMediaTreeNode tmaxNode, bool bChildren)
		{
			return FireCommand(eCommand, tmaxNode, bChildren, null);
		}

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxNodes">The nodes being passed with the command arguments</param>
		/// <param name="bChildren">True to include the node's children</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		/// <returns>The command argument object</returns>
		private CTmaxVideoArgs FireCommand(TmaxVideoCommands eCommand, CTmaxMediaTreeNodes tmaxNodes, bool bChildren, CTmaxParameters tmaxParameters)
		{
			CTmaxItems	tmaxItems = null;
			
			//	Did the caller provide a node collection?
			if((tmaxNodes != null) && (tmaxNodes.Count > 0))
			{
				//	Create the event items
				if((tmaxItems = GetCommandItems(tmaxNodes, bChildren)) == null)
				{
					return null;
				}

			}// if((tmaxNodes != null) && (tmaxNodes.Count > 0))
			
			//	Fire the command
			return FireCommand(eCommand, tmaxItems, tmaxParameters);
					
		}//	FireCommand(TmaxVideoCommands eCommand, CTmaxMediaTreeNodes tmaxNodes, CTmaxParameters tmaxParameters)

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxNodes">The nodes being passed with the command arguments</param>
		/// <param name="bChildren">True to include the node's children</param>
		/// <returns>The command argument object</returns>
		private CTmaxVideoArgs FireCommand(TmaxVideoCommands eCommand, CTmaxMediaTreeNodes tmaxNodes, bool bChildren)
		{
			return FireCommand(eCommand, tmaxNodes, bChildren, null);
		}

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
		private ToolBase GetUltraTool(TmaxVideoTreeCommands eCommand)
		{
			return GetUltraTool(eCommand.ToString());
		}
		
		/// <summary>This method is called to get the current selection</summary>
		/// <returns>The node that is currently selected in the tree</returns>
		///	<remarks>This method returns null if more than one node is selected</remarks>
		private CTmaxMediaTreeNode GetSelection()
		{
			if(m_ctrlTmaxTree != null)
				return m_ctrlTmaxTree.GetSelection();
			else
				return null;
		}
		
		/// <summary>This method is called to get the current selections</summary>
		/// <returns>The nodes that are currently selected in the tree</returns>
		private CTmaxMediaTreeNodes GetSelections(bool bSortByPosition)
		{
			if(m_ctrlTmaxTree != null)
				return m_ctrlTmaxTree.GetSelections(bSortByPosition);
			else
				return null;
		}
		
		/// <summary>This method will process the specified command</summary>
		/// <param name="eCommand">The command to be processed</param>
		private void OnCommand(TmaxVideoTreeCommands eCommand)
		{
			try
			{	
				//	Direct to the appropriate handler
				switch(eCommand)
				{
					case TmaxVideoTreeCommands.New:
					
						OnCmdNew(GetSelection(), false, false);
						break;
						
					case TmaxVideoTreeCommands.InsertBefore:
					
						OnCmdNew(GetSelection(), true, true);
						break;
						
					case TmaxVideoTreeCommands.InsertAfter:
					
						OnCmdNew(GetSelection(), true, false);
						break;
						
					case TmaxVideoTreeCommands.Delete:
					
						OnCmdDelete(GetSelections(true));
						break;
						
					case TmaxVideoTreeCommands.MoveUp:
					
						OnCmdMove(GetSelection(), true);
						break;
						
					case TmaxVideoTreeCommands.MoveDown:
					
						OnCmdMove(GetSelection(), false);
						break;
						
					case TmaxVideoTreeCommands.Sort:
					
						OnCmdSort();
						break;
						
					case TmaxVideoTreeCommands.SetProperties:
					
						OnCmdSetProperties();
						break;
						
					case TmaxVideoTreeCommands.Find:
					
						OnCmdFind();
						break;
						
					case TmaxVideoTreeCommands.ExportDesignations:
					
						OnCmdExport(GetSelections(true), TmaxExportFormats.AsciiMedia);
						break;
						
					case TmaxVideoTreeCommands.ExportTranscript:
					
						OnCmdExport(GetSelections(true), TmaxExportFormats.Transcript);
						break;
						
					case TmaxVideoTreeCommands.ExportVideo:
					
						OnCmdExport(GetSelections(true), TmaxExportFormats.Video);
						break;
						
					default:
					
						Debug.Assert(false);
						break;
				
				}// switch(eCommand)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCommand", m_tmaxErrorBuilder.Message(ERROR_ON_COMMAND_EX, eCommand), Ex);
			}
		
		}// private void OnCommand(TmaxVideoTreeCommands eCommand)

		/// <summary>This method handles the event fired when the user clicks on Delete from the context menu</summary>
		/// <param name="tmaxNodes">The nodes selected by the user</param>
		private void OnCmdDelete(CTmaxMediaTreeNodes tmaxNodes)
		{
			CTmaxItem	tmaxScript = null;
			
			try
			{
				//	Create an event item to represent the active script
				tmaxScript = new CTmaxItem(m_xmlScript);
				
				//	Add subitems for each selected designation
				foreach(CTmaxMediaTreeNode O in tmaxNodes)
					tmaxScript.SubItems.Add(GetCommandItem(O, false));
					
				//	Fire the application command
				FireCommand(TmaxVideoCommands.Delete, tmaxScript);
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdDelete", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_DELETE_EX), Ex);
			}
						
		}// private void OnCmdDelete(CTmaxMediaTreeNode tmaxNodes)
		
		/// <summary>This method handles the event fired when the user clicks on Find from the context menu</summary>
		private void OnCmdFind()
		{
			try
			{
				//	Fire the application command
				FireCommand(TmaxVideoCommands.Find, GetSearchItems());
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdFind", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_FIND_EX), Ex);
			}
						
		}// private void OnCmdFind(CTmaxMediaTreeNode tmaxNodes)
		
		/// <summary>This method handles the event fired when the user clicks on New/Insert from the context menu</summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		/// <param name="bInsert">true if inserting a new node</param>
		/// <param name="bBefore">true to insert before the selected node</param>
		private void OnCmdNew(CTmaxMediaTreeNode tmaxNode, bool bInsert, bool bBefore)
		{
			CFTmaxVideoDesignations addDesignations = null;
			
			try
			{
				addDesignations = new CFTmaxVideoDesignations();
				m_tmaxEventSource.Attach(addDesignations.EventSource);

				addDesignations.View = m_eAppId;
				addDesignations.VideoOptions = this.AppOptions;
				addDesignations.TmaxVideoCommandEvent += new FTI.Trialmax.TMVV.Tmvideo.TmaxVideoHandler(this.OnTmaxVideoCommand);
				
				//	Initialize the highlighter selection
				if((this.AppOptions != null) && (this.AppOptions.Highlighters != null))
				{
					if(this.AppOptions.LastHighlighter > 0)
						addDesignations.Highlighter = this.AppOptions.Highlighters.Find(this.AppOptions.LastHighlighter);
				}
			
				if((tmaxNode != null) && (bInsert == true))
				{
					addDesignations.XmlScript = m_xmlScript;
					addDesignations.XmlDesignation = GetXmlDesignation(tmaxNode);
					addDesignations.InsertBefore = bBefore;
				}
				else
				{
					addDesignations.XmlScript = m_xmlScript;
					addDesignations.XmlDesignation = null;
					addDesignations.InsertBefore = false;
				}
			
				addDesignations.ShowDialog();
				
				//	Update the last highlighter information
				if(addDesignations.Highlighter != null)
					m_tmaxAppOptions.LastHighlighter = addDesignations.Highlighter.Id;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdNew", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_NEW_EX), Ex);
			}
						
		}// OnCmdNew(CTmaxMediaTreeNode tmaxNode, bool bInsert, bool bBefore)
		
		/// <summary>This method handles the event fired when the user clicks on Move Up/Down from the context menu</summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		/// <param name="bUp">true to move the node up in the collection</param>
		private void OnCmdMove(CTmaxMediaTreeNode tmaxNode, bool bUp)
		{
			CTmaxItem	tmaxScript = null;
			CTmaxItem	tmaxMove = null;
			int				iIndex = -1;
			
			Debug.Assert(m_xmlScript != null);
			if(m_xmlScript == null) return;
			Debug.Assert(m_xmlScript.XmlDesignations != null);
			if(m_xmlScript.XmlDesignations == null) return;
			Debug.Assert(m_xmlScript.XmlDesignations.Count > 1);
			if(m_xmlScript.XmlDesignations.Count <= 1) return;

			Debug.Assert(tmaxNode != null);
			if(tmaxNode == null) return;
			Debug.Assert(tmaxNode.MediaType == TmaxMediaTypes.Designation);
			if(tmaxNode.MediaType != TmaxMediaTypes.Designation) return;
			
			try
			{
				//	Where does the current selection appear in the designations collection?
				if((iIndex = m_xmlScript.XmlDesignations.IndexOf(tmaxNode.XmlObject)) < 0)
				{
					Debug.Assert(false, "designation not found");
					return;
				}
				
				//	Create an event item to represent the script
				tmaxScript = new CTmaxItem(m_xmlScript);
				
				//	Add all but the current selection to the SubItems collection
				foreach(CTmaxMediaTreeNode O in m_ctrlTmaxTree.Nodes)
				{
					if(ReferenceEquals(O, tmaxNode) != true)
					{
						tmaxScript.SubItems.Add(new CTmaxItem(m_xmlScript, ((CXmlDesignation)(O.XmlObject))));
					}
					
				}// foreach(CTmaxMediaTreeNode O in m_ctrlTmaxTree.Nodes)	
			
				Debug.Assert(tmaxScript.SubItems.Count > 0);
				if(tmaxScript.SubItems.Count == 0) return;
				
				tmaxMove = new CTmaxItem(m_xmlScript, (CXmlDesignation)(tmaxNode.XmlObject));
				
				//	Are we moving up?
				if(bUp == true)
				{
					if(iIndex >= 1)
						tmaxScript.SubItems.Insert(tmaxMove, iIndex - 1);
				}
				else
				{
					if(iIndex < tmaxScript.SubItems.Count)
						tmaxScript.SubItems.Insert(tmaxMove, iIndex + 1);
				}
				
				//	Collection sizes must match to process this event
				if(tmaxScript.SubItems.Count == m_xmlScript.XmlDesignations.Count)
					FireCommand(TmaxVideoCommands.Reorder, tmaxScript);
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdMove", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_MOVE_EX), Ex);
			}
						
		}// private void OnCmdMove(CTmaxMediaTreeNode tmaxNode, bool bUp)
		
		/// <summary>This method handles the event fired when the user clicks on one of the options in the Export submenu</summary>
		/// <param name="tmaxNodes">The nodes selected by the user</param>
		/// <param name="eFormat">The format to use for the export operation</param>
		private void OnCmdExport(CTmaxMediaTreeNodes tmaxNodes, TmaxExportFormats eFormat)
		{
			CTmaxItem		tmaxScript = null;
			CTmaxParameters	tmaxParameters = null;
			
			try
			{
				//	Create an event item to represent the active script
				tmaxScript = new CTmaxItem(m_xmlScript);
				
				//	Add subitems for each selected designation
				if((tmaxNodes != null) && (tmaxNodes.Count > 0))
				{
					foreach(CTmaxMediaTreeNode O in tmaxNodes)
						tmaxScript.SubItems.Add(GetCommandItem(O, false));
				}
				else
				{
					foreach(CTmaxMediaTreeNode O in m_ctrlTmaxTree.Nodes)
						tmaxScript.SubItems.Add(GetCommandItem(O, false));
				}
					
				//	Add the parameter for the export format
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.ExportFormat, (int)eFormat);
				
				//	Fire the application command
				FireCommand(TmaxVideoCommands.Export, tmaxScript, tmaxParameters);
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdExport", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_EXPORT_EX), Ex);
			}
						
		}// private void OnCmdExport(TmaxExportFormats eFormat)
		
		/// <summary>This method handles the event fired when the user clicks on Set Properties from the context menu</summary>
		private void OnCmdSetProperties()
		{
			CFTmaxVideoSetProps setProps = null;
			CTmaxItem		tmaxScript = null;
			
			Debug.Assert(m_xmlScript != null);
			if(m_xmlScript == null) return;
			
			try
			{
				setProps = new CFTmaxVideoSetProps();
				m_tmaxEventSource.Attach(setProps.EventSource);
				
				setProps.XmlScript = m_xmlScript;
				setProps.SetSource = false;
				
				if(setProps.ShowDialog() == DialogResult.OK)
				{
					//	Fire the update command
					tmaxScript = new CTmaxItem(m_xmlScript);
					FireCommand(TmaxVideoCommands.Update, tmaxScript);
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdSetProperties", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_SET_PROPS_EX), Ex);
			}
						
		}// private void OnCmdMove(CTmaxMediaTreeNode tmaxNode, bool bUp)
		
		/// <summary>This method handles the event fired when the user clicks on Sort Designations from the context menu</summary>
		private void OnCmdSort()
		{
			CTmaxItem		tmaxScript = null;
			CXmlDesignations	xmlSorted = null;
			
			Debug.Assert(m_xmlScript != null);
			if(m_xmlScript == null) return;
			Debug.Assert(m_xmlScript.XmlDesignations != null);
			if(m_xmlScript.XmlDesignations == null) return;

			if(m_xmlScript.XmlDesignations.Count <= 1) return;

			try
			{
				//	Create the collection to sort the designations
				xmlSorted = new CXmlDesignations();
				xmlSorted.Comparer = new CXmlBaseSorter(CXmlDesignation.XML_DESIGNATION_SORT_POSITION);
				xmlSorted.KeepSorted = false;
				
				//	Transfer all the designations to the collection for sorting
				foreach(CXmlDesignation O in m_xmlScript.XmlDesignations)
					xmlSorted.Add(O);
					
				//	Now sort the designations
				if(xmlSorted.Count < 2) return;
				else xmlSorted.Sort();
				
				//	Create an event item to represent the script
				tmaxScript = new CTmaxItem(m_xmlScript);
				
				//	Now populate the subitems collection in the new sort order
				foreach(CXmlDesignation O in xmlSorted)
				{
					tmaxScript.SubItems.Add(new CTmaxItem(m_xmlScript, O));
					
				}// foreach(CXmlDesignation O in xmlSorted)

				//	Collection sizes must match to process this event
				if(tmaxScript.SubItems.Count == m_xmlScript.XmlDesignations.Count)
					FireCommand(TmaxVideoCommands.Reorder, tmaxScript);
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdSort", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_SORT_EX), Ex);
			}
						
		}// private void OnCmdSort(CTmaxMediaTreeNode tmaxNode)
		
		/// <summary>Traps the ToolClick event fired by the toolbar manager</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event argument object</param>
		private void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		{
			if(m_bIgnoreEvents == false)
			{
				TmaxVideoTreeCommands eCommand = TmaxVideoTreeCommands.Invalid;
				
				if((eCommand = GetCommand(e.Tool.Key)) != TmaxVideoTreeCommands.Invalid)
					OnCommand(eCommand);
				
			}
		
		}// private void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		
		/// <summary>This function handles events fired by the toolbar manager when it is about to display a menu</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event parameters</param>
		private void OnUltraBeforeDropDown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventArgs e)
		{
			PopupMenuTool popupMenu = null;
			
			try
			{
				if((popupMenu = (PopupMenuTool)(e.Tool)) != null)
				{
					if(popupMenu.Tools != null)
						SetToolStates(popupMenu.Tools);
				}
						
			}
			catch
			{
			}
					
		}// private void OnUltraBeforeDropDown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventArgs e)

		/// <summary>This method handles events fired by the toolbar manager after displaying the popup menu</summary>
		/// <param name="sender">The toolbar manager firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnUltraAfterPopup(object sender, Infragistics.Win.UltraWinToolbars.ToolDropdownEventArgs e)
		{
			TmaxVideoTreeCommands	eCommand = TmaxVideoTreeCommands.Invalid;
			PopupMenuTool			popupMenu = null;
			
			try { popupMenu = (PopupMenuTool)(e.Tool); }
			catch { return; }
			
			//	Check each tool in the menu's collection
			foreach(ToolBase O in popupMenu.Tools)
			{
				//	Get the command for this tool
				if((eCommand = GetCommand(O.Key)) != TmaxVideoTreeCommands.Invalid)
				{
					//	Uninstall the shortcut if required
					if(GetCommandShortcut(eCommand) != Shortcut.None)
						O.SharedProps.Shortcut = Shortcut.None;
				}

			}// foreach(ToolBase O in m_ultraToolbarManager.Tools)
				
		}// private void OnUltraAfterPopup(object sender, Infragistics.Win.UltraWinToolbars.ToolDropdownEventArgs e)

		/// <summary>Handles all TmaxVideoCommand events trapped by the form</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Event argument object</param>
		private void OnTmaxVideoCommand(object sender, CTmaxVideoArgs Args)
		{
			//	Propagate the event
			FireCommand(Args);
		}
		
		/// <summary>This method is called to determine if the specified command should be enabled</summary>
		/// <param name="eCommand">The tree pane command enumeration</param>
		/// <param name="tmaxNodes">The current node selections</param>
		/// <returns>true if command should be enabled</returns>
		private bool GetCommandEnabled(TmaxVideoTreeCommands eCommand, CTmaxMediaTreeNodes tmaxNodes)
		{
			//	Do we have a valid tree
			if(m_ctrlTmaxTree == null) return false;
			if(m_ctrlTmaxTree.Nodes == null) return false;
			
			//	We have to have a script
			if(m_xmlScript == null) return false;
				
			//	What is the command?
			switch(eCommand)
			{
				case TmaxVideoTreeCommands.SetProperties:
				case TmaxVideoTreeCommands.Find:
				
					//	All we need is a script
					return true;
					
				case TmaxVideoTreeCommands.New:
	
					//	Should not be any nodes selected
					if(tmaxNodes == null) return true;
					if(tmaxNodes.Count == 0) return true;
					
					return false;
					
				case TmaxVideoTreeCommands.InsertAfter:
				case TmaxVideoTreeCommands.InsertBefore:
	
					//	These commands require single media selections
					return (tmaxNodes.Count == 1);
										
				case TmaxVideoTreeCommands.Delete:
				
					//	Must have at least one selection
					if(tmaxNodes.Count == 0) return false;
					
					//	All nodes must be designations
					foreach(CTmaxMediaTreeNode O in tmaxNodes)
					{
						if(O.MediaType != TmaxMediaTypes.Designation)
							return false;
					}
					
					return true; // All nodes check out						
				
				case TmaxVideoTreeCommands.MoveUp:
				case TmaxVideoTreeCommands.MoveDown:

					//	Must be only one node selected
					if(tmaxNodes.Count != 1) return false;
				
					//	Can not reorder anything other than designations
					if(tmaxNodes[0].MediaType != TmaxMediaTypes.Designation) return false;
					
					//	Is there room to move up ?
					if((eCommand == TmaxVideoTreeCommands.MoveUp) &&
						(tmaxNodes[0].HasSibling(NodePosition.Previous) == false)) return false;
					
					//	Is there room to move down ?
					if((eCommand == TmaxVideoTreeCommands.MoveDown) &&
						(tmaxNodes[0].HasSibling(NodePosition.Next) == false)) return false;
					
					return true;
				
				case TmaxVideoTreeCommands.Sort:

					//	Must be more than one node in the script
					if(m_xmlScript == null) return false;
					if(m_xmlScript.XmlDesignations == null) return false;
					if(m_xmlScript.XmlDesignations.Count < 2) return false;
				
					return true;
				
				case TmaxVideoTreeCommands.ExportDesignations:
				case TmaxVideoTreeCommands.ExportTranscript:
				case TmaxVideoTreeCommands.ExportVideo:

					//	Must be at least one node in the script
					if(m_xmlScript == null) return false;
					if(m_xmlScript.XmlDesignations == null) return false;
					if(m_xmlScript.XmlDesignations.Count == 0) return false;
				
					//	Can not export anything other than designations
					if((tmaxNodes != null) && (tmaxNodes.Count > 0))
					{
						if(tmaxNodes[0].MediaType != TmaxMediaTypes.Designation) return false;
					}
					
					return true;
				
				default:
				
					break;
			}	
			
			return false;
		
		}// private bool GetCommandEnabled(TmaxVideoTreeCommands eCommand, CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method is called to determine if the specified command should be visible</summary>
		/// <param name="eCommand">The tree pane command enumeration</param>
		/// <returns>true if command should be visible</returns>
		private bool GetCommandVisible(TmaxVideoTreeCommands eCommand)
		{
			//	We changed our mind on this one
			if(eCommand == TmaxVideoTreeCommands.SetProperties)
				return false;
			else
				return true;
		}
		
		/// <summary>This method is called to convert the specified key to its associated command enumeration</summary>
		/// <returns>The associated tree command</returns>
		private TmaxVideoTreeCommands GetCommand(string strKey)
		{
			try
			{
				Array aCommands = Enum.GetValues(typeof(TmaxVideoTreeCommands));
				
				foreach(TmaxVideoTreeCommands eCommand in aCommands)
				{
					if(eCommand.ToString() == strKey)
						return eCommand;
				}
				
			}
			catch
			{
			}
			
			return TmaxVideoTreeCommands.Invalid;
		
		}// private TmaxVideoTreeCommands GetCommand(string strKey)
		
		/// <summary>This method is called to get the shortcut key for the specified command</summary>
		/// <param name="eCommand">The tree pane command enumeration</param>
		/// <returns>The shortcut key if there is one</returns>
		private Shortcut GetCommandShortcut(TmaxVideoTreeCommands eCommand)
		{
			switch(eCommand)
			{
				case TmaxVideoTreeCommands.Find:
				
					return Shortcut.CtrlF;
				
				default:
				
					return Shortcut.None;
			
			}// switch(eCommand)

		}// private Shortcut GetCommandShortcut(TmaxVideoTreeCommands eCommand)
		
		/// <summary>This function enables / disables the command tools</summary>
		private void SetToolStates(Infragistics.Win.UltraWinToolbars.ToolsCollectionBase ultraTools)
		{
			TmaxVideoTreeCommands	eCommand = TmaxVideoTreeCommands.Invalid;
			Shortcut				eShortcut = Shortcut.None;
			
			try
			{
				//	Should we use the root collection?
				if(ultraTools == null)
					ultraTools = m_ctrlToolbarManager.Tools;
					
				//	Iterate the master tools collection
				foreach(ToolBase O in ultraTools)
				{
					//	Get the command associated with this tool
					if((eCommand = GetCommand(O.Key)) != TmaxVideoTreeCommands.Invalid)
					{
						//	Should this tool be visible and enabled?
						O.SharedProps.Visible = GetCommandVisible(eCommand);
						O.SharedProps.Enabled = GetCommandEnabled(eCommand, GetSelections(true));

						//	Set shortcuts for commands on this menu
						if(O.SharedProps.Visible == true)
						{
							if((eShortcut = GetCommandShortcut(eCommand)) != Shortcut.None)
								O.SharedProps.Shortcut = eShortcut;
						}
														
					}
						
				}// foreach(ToolBase O in ultraTools)
						
			}
			catch
			{
					
			}
					
		}// private void SetToolStates()

		/// <summary>This method is called to get the XML designation bound to the specified node</summary>
		/// <param name="xmlDesignation">The node that references the designation</param>
		/// <returns>The designation bound to the tree node</returns>
		private CXmlDesignation GetXmlDesignation(CTmaxMediaTreeNode tmaxNode)
		{
			CXmlDesignation xmlDesignation = null;
			
			try
			{
				switch(tmaxNode.MediaType)
				{
					case TmaxMediaTypes.Designation:
					
						xmlDesignation = (CXmlDesignation)(tmaxNode.XmlObject);
						break;
						
					case TmaxMediaTypes.Link:
					
						if(tmaxNode.Parent != null)
							xmlDesignation= GetXmlDesignation((CTmaxMediaTreeNode)(tmaxNode.Parent));
						break;
						
				}// switch(tmaxNode.MediaType)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetXmlDesignation", Ex);
			}
			
			return xmlDesignation;
			
		}// private CXmlDesignation GetXmlDesignation(CTmaxMediaTreeNode tmaxNode)
		
		/// <summary>This method is called to sort the node collection based on display order</summary>
		/// <param name="aNodes">The  collection to be sorted</param>
		private void Sort(TreeNodesCollection aNodes)
		{
			bool bSelected = false;
			
			Debug.Assert(aNodes != null);
			Debug.Assert(m_tmaxDisplaySorter != null);
			if(aNodes == null) return;
			if(m_tmaxDisplaySorter == null) return;
			
			//	Display sorter collection should be empty
			Debug.Assert(m_tmaxDisplaySorter.Count == 0);
			if(m_tmaxDisplaySorter.Count > 0)
				m_tmaxDisplaySorter.Clear();
			
			//	Is there anything to sort?
			if(aNodes.Count < 2)
				return;
		
			//	Put each of the children into the sorter
			foreach(CTmaxMediaTreeNode O in aNodes)
				m_tmaxDisplaySorter.Add(O);
				
			//	Sort the nodes
			m_tmaxDisplaySorter.Sort(true);

			for(int i = 0; i < aNodes.Count; i++)
			{
				//	Is this object out of position?
				if(ReferenceEquals(m_tmaxDisplaySorter[i], aNodes[i]) == false)
				{
					try
					{
						bSelected = m_tmaxDisplaySorter[i].Selected;
						
						m_tmaxDisplaySorter[i].Reposition(aNodes[i], NodePosition.Previous);
					
						if(bSelected)
							m_tmaxDisplaySorter[i].Selected = true;
					}
					catch
					{
					}
					
				}
				
			}
				
			//	Empty out the sorter
			m_tmaxDisplaySorter.Clear();
			
		}// private void Sort(TreeNodesCollection aNodes)
		
		/// <summary>This method is called when a node has been deleted</summary>
		/// <param name="tmaxItem">The node that has been deleted</param>
		private void OnDeleted(CTmaxMediaTreeNode tmaxNode)
		{
			CTmaxMediaTreeNode		tmaxParent = null;
			CTmaxMediaTreeNode		tmaxSelect = null;
			
			//	Is this node the current selection?
			if(tmaxNode.Selected == true)
			{
				if((tmaxSelect = (CTmaxMediaTreeNode)tmaxNode.GetSibling(NodePosition.Next)) == null)
					tmaxSelect = (CTmaxMediaTreeNode)tmaxNode.GetSibling(NodePosition.Previous);
				
				//	Make the parent the next selection if no siblings
				if(tmaxSelect == null)
					tmaxSelect = (CTmaxMediaTreeNode)tmaxNode.Parent;
					
				tmaxNode.Selected = false;				
			}
			
			//	Make sure it is not the active node
			if(m_ctrlTmaxTree.ActiveNode != null)
			{
				if(ReferenceEquals(m_ctrlTmaxTree.ActiveNode, tmaxNode) == true)
					m_ctrlTmaxTree.ActiveNode = null;
			}
				
			//	Make sure this node is no longer selected
			if(tmaxNode.Selected == true)
				tmaxNode.Selected = false;
					
			//	Get the parent node if it exists
			tmaxParent = (CTmaxMediaTreeNode)tmaxNode.Parent;

			//	Remove this node from the tree
			if((tmaxParent != null) && (tmaxParent.Nodes != null))
				tmaxParent.Nodes.Remove(tmaxNode);
			else
				m_ctrlTmaxTree.Nodes.Remove(tmaxNode);
					
			//	Should we select a new node
			if(tmaxSelect != null)
				m_ctrlTmaxTree.SetSelection(tmaxSelect);

		}// private void OnDeleted(CTmaxMediaTreeNode tmaxNode)
			
		/// <summary>This method is called to refresh the text assigned to each node</summary>
		private void RefreshText()
		{
			foreach(CTmaxMediaTreeNode O in m_ctrlTmaxTree.Nodes)
				O.Text = GetText((CXmlDesignation)(O.XmlObject));
		}
		
		/// <summary>This method handles events fired by the draw filter when it wants to know what kind of drop actions we want to permit on a node</summary>
		/// <param name="sender">The draw filter object sending the event</param>
		/// <param name="e">The event arguements object</param>
		private void OnQueryStateAllowed(object sender, CTmaxBaseTreeFilter.CQueryStateAllowedArgs e)
		{
			e.m_eStatesAllowed = (TmaxTreePositions.AboveNode | TmaxTreePositions.BelowNode);
			m_tmaxTreeFilter.EdgeSensitivity = e.m_objNode.Bounds.Height / 3;
		}
		
		/// <summary>This method traps all QueryContinueDrag events</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">System event arguements</param>
		private void OnQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		{
			//	Did the user press escape? 
			if(e.EscapePressed)
			{
				//	Cancel the Drag
				e.Action = DragAction.Cancel;
			}
		
		}// private void OnQueryContinueDrag(object sender, QueryContinueDragEventArgs qcdevent)

		/// <summary>Handles events fired by the tree when the user starts dragging nodes</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">System event arguments</param>
		private void OnDragStart(object sender, System.EventArgs e)
		{
			//	Reset the local members
			m_tmaxDropNode = null;
			m_eDropPosition = TmaxTreePositions.None;
			if(m_tmaxDragNodes != null)
			{
				m_tmaxDragNodes.Clear();
				m_tmaxDragNodes = null;
			}
			
			try
			{
				while(true)
				{
					//	Get the current selections
					if((m_tmaxDragNodes = GetSelections(true)) == null)
						break;
						
					//	Must have at least one selection
					if(m_tmaxDragNodes.Count == 0)
						break;
						
					//	Must not have selected all the nodes
					if(m_tmaxDragNodes[0].Parent != null)
					{
						if(m_tmaxDragNodes.Count >= m_tmaxDragNodes[0].Parent.Nodes.Count)
							break;
					}
					else
					{
						if(m_tmaxDragNodes.Count >= m_ctrlTmaxTree.Nodes.Count)
							break;
					}
				
					//	Only allowed to drag designations
					if(m_tmaxDragNodes[0].MediaType != TmaxMediaTypes.Designation)
						break;

					//	Start the operation
					m_ctrlTmaxTree.DoDragDrop(m_tmaxDragNodes, (DragDropEffects.Copy | DragDropEffects.Link | DragDropEffects.Move));
				
					//	Is there a valid drop node?
					if(m_tmaxDropNode != null)
					{
						Reorder(m_tmaxDragNodes, m_tmaxDropNode, m_eDropPosition == TmaxTreePositions.AboveNode);
					}
					
					break; // We're done
					
				}// while(true)
			
			}
			catch
			{
			}
			finally
			{
				//	Clean up
				m_tmaxDropNode = null;
				m_eDropPosition = TmaxTreePositions.None;
				if(m_tmaxDragNodes != null)
				{
					m_tmaxDragNodes.Clear();
					m_tmaxDragNodes = null;
				}
			
			}
				
		}// private void OnDragStart(object sender, System.EventArgs e)
	
		/// <summary>This method handles events fired when the user drags across the client area</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">Drag/drop event arguments object</param>
		private void OnDragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			Point	objPoint;
			bool	bInvalidate;
			
			//	Only support dropping from within the tree
			if(m_tmaxDragNodes != null)
			{
				//	Get the position of the mouse in the tree's client coordinates
				objPoint = m_ctrlTmaxTree.PointToClient(new Point(e.X, e.Y));

				//	Get the node the mouse is over
				m_tmaxDropNode = m_ctrlTmaxTree.GetNode(objPoint);
				
				//	Set the draw filter's highlight node
				//
				//	NOTE:	If the drop node or line position has changed, the
				//			filter is going to fire a QueryStatesAllowed event
				//
				//			See: CTreePane::OnDrawFilterQueryStateAllowed()
				bInvalidate = m_tmaxTreeFilter.SetDropHighlightNode(m_tmaxDropNode, objPoint);
				
				//	Set the drag cursor
				e.Effect = (m_tmaxDropNode != null) ? DragDropEffects.Move : DragDropEffects.None;

				//	Do we need to redraw the drop filter?
				if(bInvalidate == true)
					m_ctrlTmaxTree.Invalidate();
				
				//	Store the current drop position
				m_eDropPosition = m_tmaxTreeFilter.DropLinePosition;
				
			}// if(m_tmaxDragNodes != null)
			
		}// private void OnDragOver(object sender, System.Windows.Forms.DragEventArgs e)

		/// <summary>This method traps all DragLeave events</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">Drag/drop event arguments object</param>
		private void OnDragLeave(object sender, System.EventArgs e)
		{
			if(m_tmaxTreeFilter != null)
				m_tmaxTreeFilter.ClearDropHighlight();
			m_tmaxDropNode = null;
			
		}// private void OnDragLeave(object sender, System.EventArgs e)

		/// <summary>This method handles events fired when the user drops on this pane</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">Drag/drop event arguments object</param>
		private void OnDragDrop(object sender, DragEventArgs drgevent)
		{
			if(m_tmaxTreeFilter != null)
				m_tmaxTreeFilter.ClearDropHighlight();
		
		}// private void OnDragDrop(object sender, DragEventArgs drgevent)

		/// <summary>This method is called to move the specified children to the new location</summary>
		/// <param name="tmaxSelections">Nodes selected to be moved</param>
		/// <param name="tmaxLocation">New location at which to place the children</param>
		/// <param name="bBefore">true if new position is before the specified location</param>
		private void Reorder(CTmaxMediaTreeNodes tmaxSelections, CTmaxMediaTreeNode tmaxLocation, bool bBefore)
		{
			CTmaxItem			tmaxScript = null;
			TreeNodesCollection	treeCollection = null;
			CTmaxMediaTreeNodes		tmaxReordered  = new CTmaxMediaTreeNodes();
			int					iIndex = -1;
			int					i;
			
			//	Check these various conditions before going on
			if(tmaxLocation == null) return;
			if(tmaxSelections == null) return;
			if(tmaxSelections.Count == 0) return;
			if(tmaxSelections[0].MediaType != TmaxMediaTypes.Designation) return;

			//	Get the collection being modified
			if(tmaxLocation.Parent != null)
				treeCollection = tmaxLocation.Parent.Nodes;
			else
				treeCollection = m_ctrlTmaxTree.Nodes;
			Debug.Assert(treeCollection != null);
			
			//	Transfer all those not being moved into our reordered collection
			tmaxReordered = new CTmaxMediaTreeNodes();
			foreach(CTmaxMediaTreeNode O in treeCollection)
			{
				if(tmaxSelections.Contains(O) == false)
					tmaxReordered.Add(O);
			}
			
			//	Should always be at least one in the collection
			if(tmaxReordered.Count == 0) return;
			
			//	Get the index of the node that identifies the location where
			//	we should insert the nodes being moved
			iIndex = treeCollection.IndexOf(tmaxLocation);
			Debug.Assert(iIndex >= 0);
			
			//	Is the location node in the list of nodes being moved?
			if(tmaxSelections.Contains(tmaxLocation) == true)
			{
				tmaxLocation = null;
				bBefore = true; // We're going to change the location node
				
				//	Locate the first node that appears after the specified
				//	location that is not also being moved
				for(i = iIndex + 1; i < treeCollection.Count; i++)
				{
					if(tmaxSelections.Contains(treeCollection[i]) == false)
					{
						tmaxLocation = (CTmaxMediaTreeNode)(treeCollection[i]);
					}
					
				}// for(i = iIndex + 1; i < treeCollection.Count; i++)
			
			}// if(tmaxSelections.Contains(tmaxLocation) == true)
			
			//	Do we have a location to insert into the collection?
			if(tmaxLocation != null)
			{
				//	Where does the insertion point appear in the reordered collection
				iIndex = tmaxReordered.IndexOf(tmaxLocation);
				Debug.Assert(iIndex >= 0);
				if(iIndex < 0) return;
				
				//	Are we inserting after the specified location?
				if(bBefore == false)
				{
					//	Is this the last in the collection?
					if(iIndex == tmaxReordered.Count - 1)
					{
						//	Just add all to the end
						foreach(CTmaxMediaTreeNode O in tmaxSelections)
							tmaxReordered.Add(O);
						iIndex = -1; // Prevents further processing
					}
					else
					{
						//	Change location to the next node so the we are
						//	ALWAYS inserting BEFORE
						iIndex = iIndex + 1;
						tmaxLocation = tmaxReordered[iIndex];
						bBefore = true;
					}
					
				}// if(bBefore == false)
				
				//	Insert the selections into the appropriate location
				if(iIndex >= 0)
					tmaxReordered.InsertRange(iIndex, tmaxSelections);
			}
			else
			{
				//	Add the nodes being moved to the end of the list
				foreach(CTmaxMediaTreeNode O in tmaxSelections)
					tmaxReordered.Add(O);
			}
			
			//	Create an event item to represent the parent script
			tmaxScript = new CTmaxItem(m_xmlScript);
			
			//	Add subitems for the new order
			foreach(CTmaxMediaTreeNode O in tmaxReordered)
			{
				if(O.XmlObject != null)
					tmaxScript.SubItems.Add(new CTmaxItem(m_xmlScript, (CXmlDesignation)(O.XmlObject)));
			}

			FireCommand(TmaxVideoCommands.Reorder, tmaxScript);
			
		}// private void Reorder(CTmaxMediaTreeNodes tmaxMove, CTmaxMediaTreeNode tmaxLocation, bool bBefore)
		
		/// <summary>This function is called to set the pause threshold</summary>
		/// <param name="dThreshold">The new pause threshold</param>
		private void SetPauseThreshold(double dThreshold)
		{
			int iImage = 0;
			
			//	Has the Pause Threshold changed?
			if(dThreshold != m_dPauseThreshold)
			{
				m_dPauseThreshold = dThreshold;
					
				//	Update each of the nodes
				if(m_ctrlTmaxTree.Nodes != null)
				{
					foreach(CTmaxMediaTreeNode O in m_ctrlTmaxTree.Nodes)
					{
						iImage = GetImageIndex(O);
						if(iImage != (int)(O.Override.NodeAppearance.Image))
							O.Override.NodeAppearance.Image = iImage;
					}
				
				}// if(m_ctrlTmaxTree.Nodes != null)

			}// if(dThreshold != m_dPauseThreshold)
		
		}// private void SetPauseThreshold(double dThreshold)
		
		#endregion Private Methods

	}//  public class CTmaxVideoTree : FTI.Trialmax.TMVV.Tmvideo.CTmaxVideoView

	/// <summary>Objects of this class are used to sort nodes in the tree</summary>
	public class CTmaxVideoTreeSorter : CTmaxBaseTreeSorter
	{
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxVideoTreeSorter() : base()
		{
		}
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This function is called to compare two nodes in the tree</summary>
		/// <param name="tmaxNode1">First node to be compared</param>
		/// <param name="tmaxNode2">Second node to be compared</param>
		/// <returns>-1 if tmaxNode1 less than tmaxNode2, 0 if equal, 1 if greater than</returns>
		protected override int Compare(CTmaxBaseTreeNode tmaxNode1, CTmaxBaseTreeNode tmaxNode2)
		{
			return Compare((CTmaxMediaTreeNode)tmaxNode1, (CTmaxMediaTreeNode)tmaxNode2);
		}
		
		/// <summary>This function is called to compare two nodes in the tree</summary>
		/// <param name="tmaxNode1">First node to be compared</param>
		/// <param name="tmaxNode2">Second node to be compared</param>
		/// <returns>-1 if tmaxNode1 less than tmaxNode2, 0 if equal, 1 if greater than</returns>
		protected int Compare(CTmaxMediaTreeNode tmaxNode1, CTmaxMediaTreeNode tmaxNode2)
		{
			CXmlDesignation	xmlDesignation1 = null;
			CXmlDesignation	xmlDesignation2 = null;
			int				iReturn = -1;
			
			try
			{
				//	Check for equality first
				//
				//	NOTE:	.NET raises and exception if we don't return 0 for
				//			equal objects
				if(ReferenceEquals(tmaxNode1, tmaxNode2) == true)
				{
					iReturn = 0;
				}
				else
				{
					if(tmaxNode1.MediaType == TmaxMediaTypes.Designation)
						xmlDesignation1 = (CXmlDesignation)(tmaxNode1.XmlObject);
					if(tmaxNode2.MediaType == TmaxMediaTypes.Designation)
						xmlDesignation2 = (CXmlDesignation)(tmaxNode2.XmlObject);
						
					if((xmlDesignation1 != null) && (xmlDesignation2 != null))
					{
						if(xmlDesignation1.XmlSortOrder == xmlDesignation2.XmlSortOrder)
							iReturn = 0;
						else if(xmlDesignation1.XmlSortOrder > xmlDesignation2.XmlSortOrder)
							iReturn = 1;
					}
					
				}
		
			}
			catch
			{
			}
			
			return iReturn;
		
		}//protected int Compare(CTmaxMediaTreeNode tmaxNode1, CTmaxMediaTreeNode tmaxNode2)
		
		#endregion Protected Methods
		
	}// class CTmaxVideoTreeSorter
	
}//  namespace FTI.Trialmax.TMVV.Tmvideo

/*
//			CTmaxItem	tmaxParent  = null;
//			CTmaxItem	tmaxInsert  = null;
//			CTmaxItem	tmaxRemove  = null;
			CTmaxMediaTreeNodes	tmaxMoving  = new CTmaxMediaTreeNodes();
//			CTmaxItems	tmaxHolding = new CTmaxItems();
			int			iInsert = -1;
			int			i;
			
			//	Check these various conditions before going on
			if(tmaxLocation == null) return;
			if(tmaxSelections == null) return;
			if(tmaxSelections.Count == 0) return;
			if(tmaxSelections[0].MediaType != TmaxMediaTypes.Designation) return;

			//	Transfer the children to a local collection we can manipulate
			for(i = 0; i < tmaxSelections.Count; i++)
			{
				//	If the first node being moved is also the drop node
				//	do not copy it to the collection of nodes being moved
				if((i == 0) && (ReferenceEquals(tmaxSelections[0], tmaxLocation) == true))
				{
					bBefore = false; //	Always after in this case
				}
				else
				{
					//	Add to the collection of those being moved
					tmaxMoving.Add(tmaxSelections[i]);
				}
				
			}
			
			//	Do we have anything to move?
			if(tmaxMoving.Count == 0) return;
//			
//			//	Create a command item for the parent node and request
//			//	that items be created for each child as they appear in the
//			//	tree right now
//			tmaxParent = GetCommandItem((CTmaxMediaTreeNode)tmaxLocation.Parent, true);
//			if(tmaxParent == null) return;
//			
			//	Where is the target location in the parent collection?
			if(tmaxLocation.Parent != null)
				iInsert = tmaxLocation.Parent.Nodes.IndexOf(tmaxLocation);
			else
				iInsert = m_ctrlTmaxTree.Nodes.IndexOf(tmaxLocation);
			Debug.Assert(iInsert >= 0);
			if(iInsert < 0) return;
MessageBox.Show("got it");			
//			//	Adjust the target location if the specified location is among
//			//	those nodes being moved
//			if(tmaxMoving.IndexOf(tmaxLocation.GetTmaxRecord(true)) >= 0)
//			{
//				//	Try moving down the collection in search of the first node not among
//				//	those being moved
//				for(i = iInsert + 1; i < tmaxParent.SubItems.Count; i++)
//				{
//					if(tmaxMoving.IndexOf(tmaxParent.SubItems[i].GetMediaRecord()) < 0)
//					{
//						iInsert = i;
//						bBefore = true;
//						break;
//					}
//				
//				}
//			
//				//	Were we unable to find a node?
//				if(i >= tmaxParent.SubItems.Count)
//				{
//					//	Try going backwards up the tree
//					for(i = iInsert - 1; i >= 0; i--)
//					{
//						if(tmaxMoving.IndexOf(tmaxParent.SubItems[i].GetMediaRecord()) < 0)
//						{
//							iInsert = i;
//							bBefore = false;
//							break;
//						}
//					}
//					
//				}
//				
//				//	Are all nodes being moved?
//				if(i < 0)
//					iInsert = -1;			
			
//			}
//			
//			//	Get the item that represents the actual insertion point
//			//
//			//	NOTE:	The index will go bad as soon as we start reordering
//			if((iInsert >= 0) && (iInsert < tmaxParent.SubItems.Count))
//				tmaxInsert = tmaxParent.SubItems[iInsert];
//				
//			//	Remove each of the children being moved from the parent collection
//			foreach(CTmaxItem tmaxMove in tmaxMoving)
//			{
//				if((tmaxRemove = tmaxParent.SubItems.Find(tmaxMove.GetMediaRecord())) != null)
//				{
//					//	Put in holding collection and remove from parent
//					tmaxHolding.Add(tmaxRemove);
//					tmaxParent.SubItems.Remove(tmaxRemove);
//				}
//				else
//				{
//					//	Should be in there
//					Debug.Assert(tmaxRemove != null);
//				}	
//			
//			}// foreach(CTmaxItem tmaxMove in tmaxMoving)
//			
//			//	If no insertion point is defined then just transfer from the holding
//			//	collection to the parent's collection
//			if(tmaxInsert == null)
//			{
//				foreach(CTmaxItem O in tmaxHolding)
//					tmaxParent.SubItems.Add(O);
//			}
//			else
//			{
//				//	Get the location of the insertion point
//				iInsert = tmaxParent.SubItems.IndexOf(tmaxInsert);
//				Debug.Assert(iInsert >= 0);
//				
//				//	Are we actually adding to the end?
//				if((bBefore == false) && (iInsert == (tmaxParent.SubItems.Count - 1)))
//				{
//					foreach(CTmaxItem O in tmaxHolding)
//						tmaxParent.SubItems.Add(O);
//				}
//				else
//				{
//					//	Adjust the insertion point so that we are always
//					//	inserting before the point
//					if(bBefore == false)
//						iInsert++;
//						
//					//	Now put everything back in
//					foreach(CTmaxItem O in tmaxHolding)
//					{
//						tmaxParent.SubItems.Insert(O, iInsert);
//						iInsert++;
//					}
//						
//				}
//				
//			}
//			tmaxHolding.Clear();
//			
//			//	Ask the system to reorder based on the subitem collection's new order
//			FireCommand(TmaxCommands.Reorder, tmaxParent);
			

*/