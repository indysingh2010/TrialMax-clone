using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;
using FTI.Trialmax.Database;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using FTI.Trialmax.Forms;
using FTI.Trialmax.Controls;

namespace FTI.Trialmax.Panes
{
	/// <summary>This pane displays the results of a search operation</summary>
	public class CResultsPane : FTI.Trialmax.Panes.CBasePane
	{
		#region Constants

		/// <summary>Local context menu command identifiers</summary>
		protected enum ResultsPaneCommands
		{
			Invalid = 0,
			CopyAll,
			CopySelection,
			SaveAll,
			SaveSelection,
            PreviewSelection,
		}

		/// <summary>Error message identifiers</summary>
		private const int ERROR_SEARCH_EX				= (ERROR_BASE_PANE_MAX + 1);
		private const int ERROR_ON_CMD_COPY_EX			= (ERROR_BASE_PANE_MAX + 2);
		private const int ERROR_ON_CMD_SAVE_EX			= (ERROR_BASE_PANE_MAX + 3);
		private const int ERROR_GET_SAVE_FILESPEC_EX	= (ERROR_BASE_PANE_MAX + 4);
		private const int ERROR_OPEN_FILE_STREAM_EX		= (ERROR_BASE_PANE_MAX + 5);

		#endregion Constants
		
		#region Private Members

        private CTranscriptPane transcriptPane = null;


		/// <summary>Component container required by form's designer</summary>
		private IContainer components;

		/// <summary>The pane's status bar</summary>
		private Infragistics.Win.UltraWinStatusBar.UltraStatusBar m_ctrlStatus;

		/// <summary>The pane's child grid</summary>
		private FTI.Trialmax.Controls.CTmaxResultsGridCtrl m_ctrlGrid;

		/// <summary>Infragistics toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager m_ctrlToolbarManager;

		/// <summary>Infragistics toolbar manager left docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CResultsPane_Toolbars_Dock_Area_Left;

		/// <summary>Infragistics toolbar manager right docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CResultsPane_Toolbars_Dock_Area_Right;

		/// <summary>Infragistics toolbar manager top docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CResultsPane_Toolbars_Dock_Area_Top;

		/// <summary>Infragistics toolbar manager bottom docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CResultsPane_Toolbars_Dock_Area_Bottom;

		/// <summary>Image list used for context menu</summary>
		private ImageList m_ctrlToolbarImages;

		/// <summary>Local member to keep track of the active results</summary>
		private CTmaxSearchResults m_tmaxSearchResults = null;

		/// <summary>Path to file used to save the results</summary>
		private string m_strSaveFileSpec = "_tmax_search_results";

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CResultsPane(CTranscriptPane transPane) : base()
		{
            this.transcriptPane = transPane;
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			//	Propagate events fired by the child grid
			m_tmaxEventSource.Name = "Search Results Pane";
			m_tmaxEventSource.Attach(m_ctrlGrid.EventSource);
					
		}// public CResultsPane()
		
		/// <summary>This function is called when the control window gets created</summary>
		/// <param name="e">Event arguments to be fired with Load event</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
		
		}// protected override void OnLoad(EventArgs e)

		/// <summary>This function is called when the value of the Database property changes</summary>
		protected override void OnDatabaseChanged()
		{
			if(m_tmaxSearchResults != null)
			{
				m_tmaxSearchResults.Clear();
				m_tmaxSearchResults = null;
			}
			if(m_tmaxAppOptions.SearchOptions.PreviousItems != null)
			{
				m_tmaxAppOptions.SearchOptions.PreviousItems.Clear();
			}
			if((m_ctrlStatus != null) && (m_ctrlStatus.IsDisposed == false))
			{
				m_ctrlStatus.Text = "";
			}
			if((m_ctrlGrid != null) && (m_ctrlGrid.IsDisposed == false))
			{
				m_ctrlGrid.Clear();
			}

		}// protected override void OnDatabaseChanged()
		
		/// <summary>This method handles all Hotkey notifications from the application</summary>
		/// <param name="eHotkey">The enumerated hotkey</param>
		/// <returns>true if processed by the pane</returns>
		public override bool OnHotkey(TmaxHotkeys eHotkey)
		{
			//	Which hotkey has been pressed?
			switch(eHotkey)
			{
				case TmaxHotkeys.FindNext:
				
					try { SelectNextResult(); }
					catch {};
					return true;
					
				default:
				
					return false;
			}
			
		}// public override bool OnHotkey(TmaxHotkeys eHotkey)
		
		/// <summary>This method is called by the application to initiate a search operation</summary>
		/// <param name="tmaxItem">The items to be searched</param>
		/// <param name="ePane">The pane making the request</param>
		/// <returns>true if successful</returns>
		public virtual bool Search(FTI.Shared.Trialmax.CTmaxItems tmaxItems, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
			bool bSuccessful = false;
			
			try
			{
				Debug.Assert(m_tmaxDatabase != null);
				Debug.Assert(m_tmaxAppOptions != null);
				if(m_tmaxDatabase == null) return false;
				if(m_tmaxAppOptions == null) return false;
				
				//	Create and initialize a new search form
				CFSearch cfSearch = new CFSearch();
				
				cfSearch.Database      = m_tmaxDatabase;
				cfSearch.Options       = m_tmaxAppOptions.SearchOptions;
				cfSearch.Selections    = tmaxItems;
				cfSearch.SearchResults = new CTmaxSearchResults();
				
				//	Connect to the form's events
				m_tmaxEventSource.Attach(cfSearch.EventSource);
				
				//	Did the user cancel the operation?
				if(cfSearch.ShowDialog() == DialogResult.OK)
				{
					//m_tmaxEventSource.InitElapsed();
					
					//	Clear the existing results
					if(m_tmaxSearchResults != null)
					{
						m_tmaxSearchResults.Clear();
						m_tmaxSearchResults = null;
					}
					
					//	Set the new results
                    if((m_tmaxSearchResults = cfSearch.SearchResults) != null)
					{
						m_ctrlGrid.Add(m_tmaxSearchResults, true);
						m_ctrlStatus.Text = (m_tmaxSearchResults.Count.ToString() + " search results");
					}
					else
					{
						//	Make sure the list is empty
						m_ctrlGrid.Clear();
						m_ctrlStatus.Text = "";
					}
					
					//m_tmaxEventSource.FireElapsed(this, "Search", "Time to load results: ");
				
					//	Select the first result
					if(m_tmaxSearchResults.Count > 0)
						m_ctrlGrid.SetSelection(0);
						
					bSuccessful = true;
				}
					
				cfSearch.Dispose();
				return bSuccessful;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Search", m_tmaxErrorBuilder.Message(ERROR_SEARCH_EX), Ex);
				return false;
			}
			
		}// public virtual bool Search(FTI.Shared.Trialmax.CTmaxItems tmaxItems, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		
		/// <summary>This method is called to determine if the FindNext command should be enabled</summary>
		/// <returns>true if the command should be enabled</returns>
		public bool CanFindNext()
		{
			return (GetNext() != null);

		}// public override bool CanFindNext()

		/// <summary>This method is called by the application to initialize the pane</summary>
		/// <returns>true if successful</returns>
		public override bool Initialize(CXmlIni xmlINI)
		{
			//	Do the base class initialization first
			if(base.Initialize(xmlINI) == false)
				return false;

			//	Initialize the grid
			m_ctrlGrid.Initialize(xmlINI);

			return true;

		}// public override bool Initialize()

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

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected override void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CResultsPane));
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CopyAll");
			Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CopySelection");
			Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SaveAll");
			Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SaveSelection");
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PreviewSelection");
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ContextMenu");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CopyAll");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SaveAll");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CopySelection");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SaveSelection");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PreviewSelection");

			
            this.m_ctrlStatus = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
			this.m_ctrlGrid = new FTI.Trialmax.Controls.CTmaxResultsGridCtrl();
			this.m_ctrlToolbarImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlToolbarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
			this._CResultsPane_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CResultsPane_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CResultsPane_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CResultsPane_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlToolbarManager)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlStatus
			// 
			appearance1.BackColor = System.Drawing.SystemColors.Control;
			appearance1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.m_ctrlStatus.Appearance = appearance1;
			this.m_ctrlStatus.BorderStyle = Infragistics.Win.UIElementBorderStyle.InsetSoft;
			this.m_ctrlStatus.Location = new System.Drawing.Point(0, 108);
			this.m_ctrlStatus.Name = "m_ctrlStatus";
			this.m_ctrlStatus.Size = new System.Drawing.Size(344, 16);
			this.m_ctrlStatus.TabIndex = 1;
			this.m_ctrlStatus.WrapText = false;
			// 
			// m_ctrlGrid
			// 
			this.m_ctrlToolbarManager.SetContextMenuUltra(this.m_ctrlGrid, "ContextMenu");
			this.m_ctrlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlGrid.Location = new System.Drawing.Point(0, 25);
			this.m_ctrlGrid.Name = "m_ctrlGrid";
			this.m_ctrlGrid.PaneId = 0;
			this.m_ctrlGrid.Size = new System.Drawing.Size(344, 83);
			this.m_ctrlGrid.TabIndex = 3;
			this.m_ctrlGrid.DblClick += new System.EventHandler(this.OnGridDblClick);
			this.m_ctrlGrid.SelectionChanged += new System.EventHandler(this.OnGridSelectionChanged);
			// 
			// m_ctrlToolbarImages
			// 
			this.m_ctrlToolbarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlToolbarImages.ImageStream")));
			this.m_ctrlToolbarImages.TransparentColor = System.Drawing.Color.Magenta;
			this.m_ctrlToolbarImages.Images.SetKeyName(0, "results_all_copy.bmp");
			this.m_ctrlToolbarImages.Images.SetKeyName(1, "results_all_print.bmp");
			this.m_ctrlToolbarImages.Images.SetKeyName(2, "results_all_save.bmp");
			this.m_ctrlToolbarImages.Images.SetKeyName(3, "results_selection_copy.bmp");
			this.m_ctrlToolbarImages.Images.SetKeyName(4, "results_selection_print.bmp");
            this.m_ctrlToolbarImages.Images.SetKeyName(5, "results_selection_save.bmp");
            this.m_ctrlToolbarImages.Images.SetKeyName(6, "preview_selection.bmp");
			// 
			// m_ctrlToolbarManager
			// 
			this.m_ctrlToolbarManager.DesignerFlags = 1;
			this.m_ctrlToolbarManager.DockWithinContainer = this;
			this.m_ctrlToolbarManager.ImageListSmall = this.m_ctrlToolbarImages;
			this.m_ctrlToolbarManager.ShowFullMenusDelay = 500;
			appearance12.Image = 0;
			buttonTool1.SharedProps.AppearancesSmall.Appearance = appearance12;
			buttonTool1.SharedProps.Caption = "Copy All";
			appearance13.Image = 3;
			buttonTool2.SharedProps.AppearancesSmall.Appearance = appearance13;
			buttonTool2.SharedProps.Caption = "Copy Selection(s)";
			appearance14.Image = 2;
			buttonTool3.SharedProps.AppearancesSmall.Appearance = appearance14;
			buttonTool3.SharedProps.Caption = "Save All As ...";
			appearance15.Image = 5;
			buttonTool4.SharedProps.AppearancesSmall.Appearance = appearance15;
			buttonTool4.SharedProps.Caption = "Save Selection(s) As ...";
            appearance16.Image = 6;
            buttonTool6.SharedProps.AppearancesSmall.Appearance = appearance16;
            buttonTool6.SharedProps.Caption = "Preview Selection";
            buttonTool6.SharedProps.Enabled = true;
            //ToolDisplayStyle.ImageAndText
            popupMenuTool1.SharedProps.Caption = "ContextMenu";
			buttonTool10.InstanceProps.IsFirstInGroup = true;
			popupMenuTool1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool5,
            buttonTool9,
            buttonTool10,
            buttonTool8,
            buttonTool7,
            });

            this.m_ctrlToolbarManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool2,
            buttonTool3,
            buttonTool4,
            buttonTool6,
            popupMenuTool1});
			this.m_ctrlToolbarManager.BeforeToolDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventHandler(this.OnUltraBeforeDropDown);
			this.m_ctrlToolbarManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.OnUltraToolClick);
			this.m_ctrlToolbarManager.AfterToolCloseup += new Infragistics.Win.UltraWinToolbars.ToolDropdownEventHandler(this.OnUltraAfterCloseup);
			this.m_ctrlToolbarManager.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.OnUltraBeforeToolbarListDropdown);
			// 
			// _CResultsPane_Toolbars_Dock_Area_Left
			// 
			this._CResultsPane_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CResultsPane_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
			this._CResultsPane_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
			this._CResultsPane_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CResultsPane_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 25);
			this._CResultsPane_Toolbars_Dock_Area_Left.Name = "_CResultsPane_Toolbars_Dock_Area_Left";
			this._CResultsPane_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 83);
			this._CResultsPane_Toolbars_Dock_Area_Left.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// _CResultsPane_Toolbars_Dock_Area_Right
			// 
			this._CResultsPane_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CResultsPane_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
			this._CResultsPane_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
			this._CResultsPane_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CResultsPane_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(344, 25);
			this._CResultsPane_Toolbars_Dock_Area_Right.Name = "_CResultsPane_Toolbars_Dock_Area_Right";
			this._CResultsPane_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 83);
			this._CResultsPane_Toolbars_Dock_Area_Right.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// _CResultsPane_Toolbars_Dock_Area_Top
			// 
			this._CResultsPane_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CResultsPane_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
			this._CResultsPane_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
			this._CResultsPane_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CResultsPane_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
			this._CResultsPane_Toolbars_Dock_Area_Top.Name = "_CResultsPane_Toolbars_Dock_Area_Top";
			this._CResultsPane_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(344, 25);
			this._CResultsPane_Toolbars_Dock_Area_Top.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// _CResultsPane_Toolbars_Dock_Area_Bottom
			// 
			this._CResultsPane_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CResultsPane_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
			this._CResultsPane_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
			this._CResultsPane_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CResultsPane_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 108);
			this._CResultsPane_Toolbars_Dock_Area_Bottom.Name = "_CResultsPane_Toolbars_Dock_Area_Bottom";
			this._CResultsPane_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(344, 0);
			this._CResultsPane_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.m_ctrlToolbarManager;
			// 
			// CResultsPane
			// 
			this.Controls.Add(this.m_ctrlGrid);
			this.Controls.Add(this._CResultsPane_Toolbars_Dock_Area_Left);
			this.Controls.Add(this._CResultsPane_Toolbars_Dock_Area_Right);
			this.Controls.Add(this._CResultsPane_Toolbars_Dock_Area_Top);
			this.Controls.Add(this._CResultsPane_Toolbars_Dock_Area_Bottom);
			this.Controls.Add(this.m_ctrlStatus);
			this.Name = "CResultsPane";
			this.PaneName = "Search Results";
			this.Size = new System.Drawing.Size(344, 124);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlToolbarManager)).EndInit();
			this.ResumeLayout(false);

		}
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			base.SetErrorStrings();
			
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to perform the search operation");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to copy the %1 to the clipboard");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to save the %1 to %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to get the path to the target file.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to open the file stream: filename = %1");

		}// protected override void SetErrorStrings()

		/// <summary>This method is called by the application to when one or more records get deleted</summary>
		/// <param name="tmaxItem">The item that has been deleted</param>
		public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			if(m_tmaxSearchResults == null) return;
			if(m_tmaxSearchResults.Count == 0) return;
			if(tmaxItems == null) return;
			
			//	Check each of the items in the collection
			foreach(CTmaxItem O in tmaxItems)
			{
				//	The records that have actually been deleted are in the SubItems collection
				foreach(CTmaxItem tmaxDeleted in O.SubItems)
				{
					if(tmaxDeleted.GetMediaRecord() != null)
					{
						switch(tmaxDeleted.GetMediaRecord().GetMediaType())
						{
							case TmaxMediaTypes.Script:
							case TmaxMediaTypes.Scene:
							case TmaxMediaTypes.Deposition:
							
								OnDeleted((CDxMediaRecord)(tmaxDeleted.GetMediaRecord()));
								break;
								
							default:
							
								break;
						
						}// switch(tmaxDeleted.GetMediaRecord().GetMediaType())
						
					}// if(tmaxDeleted.GetMediaRecord() != null)
					
				}// foreach(CTmaxItem tmaxDeleted in O.SubItems)
			}
			
		}// public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		
		/// <summary>This method is called by the application to when the item has been updated by the user</summary>
		/// <param name="tmaxItem">The item that has been updated</param>
		public override void OnEdited(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Trialmax.Database.CTmaxDatabaseResults tmaxResults)
		{
			CDxSecondary		dxScene = null;
			CDxSecondaries		dxAdded = null;
			CDxTertiary			dxDesignation = null;
			CTmaxSearchResults	tmaxReferences = null;
			long				lStartPL = 0;
			long				lStopPL = 0;
			
			if(m_tmaxSearchResults == null) return;
			if(m_tmaxSearchResults.Count == 0) return;
			
			//	Check to see if this is a scene that is referenced in this pane
			if(tmaxItem.MediaType == TmaxMediaTypes.Scene)
				dxScene = (CDxSecondary)(tmaxItem.GetMediaRecord());
			if((dxScene != null) && (dxScene.GetSource() != null) && (dxScene.GetSource().MediaType == TmaxMediaTypes.Designation))
				dxDesignation = (CDxTertiary)(dxScene.GetSource());
				
			if((dxDesignation != null) && (dxDesignation.GetExtent() != null))
			{
				dxAdded = new CDxSecondaries();
				
				//	Is this scene referenced in this results collection?
				if((tmaxReferences = GetReferences(dxScene)) != null)
				{
					//	Get the new extents
					lStartPL = dxDesignation.GetExtent().StartPL;
					lStopPL = dxDesignation.GetExtent().StopPL;

					foreach(CTmaxItem O in tmaxItem.SubItems)
					{
						if((O.MediaType == TmaxMediaTypes.Scene) && 
							(O.GetMediaRecord() != null) && 
							(((CDxSecondary)(O.GetMediaRecord())).GetSource() != null))
						{
							dxAdded.AddList((CDxSecondary)(O.GetMediaRecord()));
						}
					
					}// foreach(CTmaxItem O in tmaxItem.SubItems)
					
					foreach(CTmaxSearchResult O in tmaxReferences)
					{
						//	Is this result now outside the original designation?
						if((O.PL < lStartPL) || (O.PL > lStopPL))
						{
							OnEdited(O, dxAdded);
						}
					
					}// foreach(CTmaxSearchResult O in tmaxReferences)
					
					tmaxReferences.Clear();				
				
				}// if((tmaxReferences = GetReferences(dxScene)) != null)
			
			}// if((dxDesignation != null) && (dxDesignation.GetExtent() != null))
			
		}// public override void OnEdited(FTI.Shared.Trialmax.CTmaxItem tmaxItem)

		/// <summary>Overridden by derived classes to provide access to the pane's toolbar manager</summary>
		/// <returns>The pane's toolbar manager if available</returns>
		protected override Infragistics.Win.UltraWinToolbars.UltraToolbarsManager GetUltraToolbarManager()
		{
			return m_ctrlToolbarManager;
		}

		#endregion Protected Methods
		
		#region Private Members
		
		/// <summary>This method is called when a result is no longer valid because it's scene has been edited</summary>
		/// <param name="tmaxResult">The result that has been invalidated</param>
		///	<param name="dxAdded">Collection of scenes added as a result of splitting the original designation</param>
		/// <returns>True if updated in the list, false if removed</returns>
		private bool OnEdited(CTmaxSearchResult tmaxResult, CDxSecondaries dxAdded)
		{
			CDxTertiary dxDesignation = null;
			
			//	Check to see if it now falls within one of the new scenes
			foreach(CDxSecondary dxScene in dxAdded)
			{
				if((dxScene.GetSource() != null) && (dxScene.GetSource().MediaType == TmaxMediaTypes.Designation))
					dxDesignation = (CDxTertiary)(dxScene.GetSource());
				else
					dxDesignation = null;
					
				if((dxDesignation != null) && (dxDesignation.GetExtent() != null))
				{
					//	Does it fall within this designation?
					if((tmaxResult.PL >= dxDesignation.GetExtent().StartPL) &&
					   (tmaxResult.PL <= dxDesignation.GetExtent().StopPL))
					{
						//	Reassign the scene for this result
						tmaxResult.IScene = dxScene;
						m_ctrlGrid.Update(tmaxResult);
						return true;
					}
				
				}// if((dxDesignation != null) && (dxDesignation.GetExtent() != null))
				
			}// foreach(CDxSecondary dxScene in dxAdded)
			
			//	If we made it this far we have to remove it from the list
			m_ctrlGrid.Delete(tmaxResult);
			m_tmaxSearchResults.Remove(tmaxResult);
			return false;
			
		}// private bool OnEdited(CTmaxSearchResult tmaxResult, CDxSecondaries dxAdded)
		
		/// <summary>This method is called to get all results that reference the specified record</summary>
		/// <param name="dxRecord">The reference record</param>
		///	<returns>The collection of results that reference the record</returns>
		private CTmaxSearchResults GetReferences(CDxMediaRecord dxRecord)
		{
			CTmaxSearchResults tmaxResults = null;
			
			if(dxRecord == null) return null;
			if(m_tmaxSearchResults == null) return null;
			if(m_tmaxSearchResults.Count == 0) return null;
			
			try
			{
				tmaxResults = new CTmaxSearchResults();
				
				//	What type of record is this?
				switch(dxRecord.MediaType)
				{
					case TmaxMediaTypes.Scene:
					
						foreach(CTmaxSearchResult O in m_tmaxSearchResults)
						{
							if(ReferenceEquals(dxRecord, O.IScene) == true)
								tmaxResults.Add(O);
						}
						break;
						
					case TmaxMediaTypes.Deposition:
					
						foreach(CTmaxSearchResult O in m_tmaxSearchResults)
						{
							if(ReferenceEquals(dxRecord, O.IDeposition) == true)
								tmaxResults.Add(O);
						}
						break;
						
					case TmaxMediaTypes.Script:
					
						foreach(CTmaxSearchResult O in m_tmaxSearchResults)
						{
							if(O.IScene != null)
							{
								if(ReferenceEquals(dxRecord, O.IScene.GetParent()) == true)
									tmaxResults.Add(O);
							}
						}
						break;
						
				}// switch(dxRecord.MediaType)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetReferences", Ex);
			}
			
			if((tmaxResults != null) && (tmaxResults.Count > 0))
				return tmaxResults;
			else
				return null;
				
		}// private void OnDeleted(CDxMediaRecord dxRecord)
		
		/// <summary>This method is called by the application to when the item gets deleted</summary>
		/// <param name="tmaxItem">The item that has been deleted</param>
		private void OnDeleted(CDxMediaRecord dxRecord)
		{
			CTmaxSearchResults tmaxResults = null;
			
			if(dxRecord == null) return;
			
			//	Get the results that are bound to the deleted record
			if((tmaxResults = GetReferences(dxRecord)) != null)
			{
				//	Remove each from the list
				foreach(CTmaxSearchResult O in tmaxResults)
				{
					m_ctrlGrid.Delete(O);
					m_tmaxSearchResults.Remove(O);
				}	
			
			}
			
		}// private void OnDeleted(CDxMediaRecord dxRecord)
		
		/// <summary>This method will fire the SetSearchResult command event</summary>
		/// <param name="bOpen">true to open the transcript viewer</param>
		private void FireSetSearchResult(bool bOpen)
		{
			CTmaxItem			tmaxItem = null;
			CTmaxSearchResult	tmaxResult = null;
			CTmaxParameters		tmaxParameters = null;
			
			if(m_tmaxSearchResults == null) return;
			if(m_tmaxSearchResults.Count == 0) return;
			
			//	Get the current selection
			if((tmaxResult = m_ctrlGrid.GetSelection()) != null)
			{
				if(tmaxResult.IScene != null)
				{
					if(m_tmaxDatabase.IsValidRecord((CDxMediaRecord)(tmaxResult.IScene)) == true)
						tmaxItem = new CTmaxItem(tmaxResult.IScene);
				}
				else if(tmaxResult.IDeposition != null)
				{
					if(m_tmaxDatabase.IsValidRecord((CDxMediaRecord)(tmaxResult.IDeposition)) == true)
						tmaxItem = new CTmaxItem(tmaxResult.IDeposition);
				}
						
				if(tmaxItem != null)
				{
					tmaxItem.UserData1 = tmaxResult;
							
					if(bOpen == true)
					{
						//	Create the required parameters required to open the transcript viewer
						tmaxParameters = new CTmaxParameters();
						tmaxParameters.Add(TmaxCommandParameters.Viewer, true);
					}
							
					FireCommand(TmaxCommands.SetSearchResult, tmaxItem, tmaxParameters);
				}
						
			}// if((tmaxResult = (CTmaxSearchResult)(m_ctrlResults.GetSelected())) != null)
					
		}// private void FireSetSearchResult(bool bOpen)
		
		/// <summary>This method is called to get the next result in the list</summary>
		private CTmaxSearchResult GetNext()
		{
			CTmaxSearchResult	tmaxResult = null;
			int					iIndex = 0;
			
			if(m_tmaxSearchResults == null) return null;
			if(m_tmaxSearchResults.Count == 0) return null;
			
			//	Get the current selection
			if((tmaxResult = m_ctrlGrid.GetSelection()) != null)
			{
				//	Is this the last one in the list?
				if((iIndex = m_tmaxSearchResults.IndexOf(tmaxResult)) >= 0)
				{
					if(iIndex < (m_tmaxSearchResults.Count - 1))
						tmaxResult = m_tmaxSearchResults[iIndex + 1];
					else
						tmaxResult = null;
				}
				else
				{
					tmaxResult = null;
				}
			}
			else
			{
				if(m_tmaxSearchResults.Count > 0)
					tmaxResult = m_tmaxSearchResults[0];
											
			}// if((tmaxResult = (CTmaxSearchResult)(m_ctrlResults.GetSelected())) != null)
					
			return tmaxResult;
			
		}// private CTmaxSearchResult GetNext()
		
		/// <summary>Called when the user selects a new result</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnGridSelectionChanged(object sender, System.EventArgs e)
		{
			FireSetSearchResult(false);
								
		}// private void OnSelChanged(object sender, System.EventArgs e)
		
		/// <summary>Called when the user double clicks in the results list</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnGridDblClick(object sender, System.EventArgs e)
		{
			FireSetSearchResult(true);

		}// private void OnGridDblClick(object sender, System.EventArgs e)
		
		/// <summary>This method is called to select the next result in the list</summary>
		private void SelectNextResult()
		{
			CTmaxSearchResult tmaxResult = null;
			
			if((tmaxResult = GetNext()) != null)
				m_ctrlGrid.SetSelection(tmaxResult);
			
		}// private void SelectNextResult()

		/// <summary>This method processes the specified command</summary>
		/// <param name="eCommand">The enumerated command identifier</param>
		private void OnCommand(ResultsPaneCommands eCommand)
		{
			try
			{
				//	Direct to the appropriate handler
				switch(eCommand)
				{
					case ResultsPaneCommands.CopyAll:
					
						OnCmdCopy(false);
						break;
						
					case ResultsPaneCommands.SaveAll:

						OnCmdSave(false);
						break;

					case ResultsPaneCommands.CopySelection:

						OnCmdCopy(true);
						break;

					case ResultsPaneCommands.SaveSelection:

						OnCmdSave(true);
						break;

                    case ResultsPaneCommands.PreviewSelection:

                        OnCmdPreviewSelection();
                        break;

                    
                    default:

						break;

				}// switch(eCommand)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnCommand", Ex);
			}

        }// private void OnCommand(ResultsPaneCommands eCommand)

		/// <summary>Toolbar event handler for Copy commands</summary>
		private void OnCmdCopy(bool bSelections)
		{
			CTmaxSearchResults	tmaxResults = null;
			string				strClipboard = "";
			string				strMsg = "";

			try
			{
				//	Which results should we use?
				if(bSelections == true)
					tmaxResults = m_ctrlGrid.GetSelected();
				else
					tmaxResults = m_tmaxSearchResults;

				//	Is there anything to be saved?
				if((tmaxResults != null) && (tmaxResults.Count > 0))
				{
					foreach(CTmaxSearchResult O in tmaxResults)
					{
						if(strClipboard.Length > 0)
							strClipboard += "\r\n";
							
						strClipboard += O.ToClipboard();

					}// foreach(CTmaxSearchResult O in tmaxResults)

					Clipboard.SetText(strClipboard);

					strMsg = String.Format("{0} search results were copied to the clipboard.", tmaxResults.Count);
					MessageBox.Show(strMsg, "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					MessageBox.Show("No search results to be copied", "Copy", MessageBoxButtons.OK, MessageBoxIcon.Information);

				}// if((tmaxResults != null) && (tmaxResults.Count > 0))
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdCopy", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_COPY_EX, bSelections ? "selections" : "results"), Ex);
			}

		}// private void OnCmdCopy(bool bSelections)

		/// <summary>Toolbar event handler for Save commands</summary>
		private void OnCmdSave(bool bSelections)
		{
			CTmaxSearchResults		tmaxResults = null;
			System.IO.StreamWriter	streamWriter = null;
			string					strMsg = "";
			
			try
			{
				//	Which results should we use?
				if(bSelections == true)
					tmaxResults = m_ctrlGrid.GetSelected();
				else
					tmaxResults = m_tmaxSearchResults;
					
				//	Is there anything to be saved?
				if((tmaxResults != null) && (tmaxResults.Count > 0))
				{
					//	Get the path to the file where the results should be stored
					if(GetSaveFileSpec() == true)
					{
						//	Open the output file stream
						if((streamWriter = OpenFileStream(m_strSaveFileSpec)) != null)
						{
							foreach(CTmaxSearchResult O in tmaxResults)
							{
								streamWriter.WriteLine(O.ToFileString());
							}
							
							strMsg = String.Format("{0} search results written to {1}", tmaxResults.Count, m_strSaveFileSpec);
							MessageBox.Show(strMsg, "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

						}// if((streamWriter = OpenFileStream(m_strSaveFileSpec)) != null)
						
					}// if(GetSaveFileSpec() == true)
				
				}
				else
				{
					MessageBox.Show("No search results to be saved", "Save As", MessageBoxButtons.OK, MessageBoxIcon.Information);

				}// if((tmaxResults != null) && (tmaxResults.Count > 0))
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdSave", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_SAVE_EX, bSelections ? "selections" : "results", m_strSaveFileSpec), Ex);
			}
			finally
			{
				if(streamWriter != null)
					streamWriter.Close();
			}

		}// private void OnCmdSave(bool bSelections)

        /// <summary>Toolbar event handler for PreviewSelection command</summary>
        private bool OnCmdPreviewSelection()
        {
            CTmaxItem tmaxItem = null;
            CTmaxSearchResult tmaxResult = null;
            CTmaxParameters tmaxParameters = null;

            if (m_tmaxSearchResults == null) return false;
            if (m_tmaxSearchResults.Count == 0) return false;

            //	Get the current selection
            if ((tmaxResult = m_ctrlGrid.GetSelection()) != null)
            {
                if (tmaxResult.IScene != null)
                {
                    if (m_tmaxDatabase.IsValidRecord((CDxMediaRecord)(tmaxResult.IScene)) == true)
                        tmaxItem = new CTmaxItem(tmaxResult.IScene);
                }
                else if (tmaxResult.IDeposition != null)
                {
                    if (m_tmaxDatabase.IsValidRecord((CDxMediaRecord)(tmaxResult.IDeposition)) == true)
                        tmaxItem = new CTmaxItem(tmaxResult.IDeposition);
                }

                if (tmaxItem != null)
                {
                    tmaxItem.UserData1 = tmaxResult;

                    //	Create the required parameters required to open the transcript viewer
                    tmaxParameters = new CTmaxParameters();
                    tmaxParameters.Add(TmaxCommandParameters.Viewer, true);
                    FireCommand(TmaxCommands.SetSearchResult, tmaxItem, tmaxParameters);
                    transcriptPane.PreviewSelectionResultPane();
                }
            }

            return true;

        }// private void OnCmdPreviewSelection()

        
        /// <summary>This method is called to enable/disable the tools in the manager's collection</summary>
		/// <param name="bShortcuts">true to assign shortcuts</param>
		private void SetToolStates(bool bShortcuts)
		{
			ResultsPaneCommands eCommand = ResultsPaneCommands.Invalid;
			int					iSelections = 0;
			Shortcut			eShortcut = Shortcut.None;

			Debug.Assert(m_ctrlToolbarManager != null);
			Debug.Assert(m_ctrlToolbarManager.Tools != null);
			if((m_ctrlToolbarManager == null) || (m_ctrlToolbarManager.Tools == null)) return;

			//	Get the current number of row selections
			if((m_ctrlGrid != null) && (m_ctrlGrid.IsDisposed == false))
				iSelections = m_ctrlGrid.GetSelectedCount();

			//	Check each tool in the manager's collection
			foreach(ToolBase O in m_ctrlToolbarManager.Tools)
			{
				if(O.Key == null) continue;

				try
				{
					//	Do we have a valid command?
					eCommand = GetCommand(O.Key);

					//	Should the command be enabled?
					O.SharedProps.Enabled = GetCommandEnabled(eCommand, iSelections);

					//	Should we assign a shortcut to this command?
					if(bShortcuts == true)
					{
						if((eShortcut = GetCommandShortcut(eCommand)) != Shortcut.None)
							O.SharedProps.Shortcut = eShortcut;
					}

				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireDiagnostic(this, "SetToolStates", Ex);
				}

			}// foreach(ToolBase O in m_ctrlToolbarManager.Tools)

		}// private void SetToolStates()

		/// <summary>This method is called to convert the key to a command identifier</summary>
		/// <param name="strKey">The Infragistic key identifier</param>
		/// <returns>The associated pane command</returns>
		private ResultsPaneCommands GetCommand(string strKey)
		{
			try
			{
				Array aCommands = Enum.GetValues(typeof(ResultsPaneCommands));

				foreach(ResultsPaneCommands eCommand in aCommands)
				{
					if(eCommand.ToString() == strKey)
						return eCommand;
				}

			}
			catch
			{
			}

			return ResultsPaneCommands.Invalid;

		}// private ResultsPaneCommands GetCommand(string strKey)

		/// <summary>This method is called to determine if the specified command should be enabled</summary>
		/// <param name="eCommand">The pane command enumeration</param>
		/// <param name="iSelections">The number of rows currently selected</param>
		/// <returns>true if command should be enabled</returns>
		private bool GetCommandEnabled(ResultsPaneCommands eCommand, int iSelections)
		{
			//	We have to have an active database with codes
			if(m_tmaxDatabase == null) return false;
			if(m_tmaxDatabase.Primaries == null) return false;

			//	What is the command?
			switch(eCommand)
			{
				case ResultsPaneCommands.CopyAll:
				case ResultsPaneCommands.SaveAll:
					return ((m_tmaxSearchResults != null) && (m_tmaxSearchResults.Count > 0));

				case ResultsPaneCommands.CopySelection:
				case ResultsPaneCommands.SaveSelection:
					return ((m_tmaxSearchResults != null) && (iSelections > 0));

                case ResultsPaneCommands.PreviewSelection:
                    return ((m_tmaxSearchResults != null) && (iSelections > 0));

				default:

					break;

			}// switch(eCommand)	

			return false;

		}// private bool GetCommandEnabled(ResultsPaneCommands eCommand, int iSelections)

		/// <summary>This method is called to get the shortcut key for the specified command</summary>
		/// <param name="eCommand">The tree pane command enumeration</param>
		/// <returns>The shortcut key if there is one</returns>
		private Shortcut GetCommandShortcut(ResultsPaneCommands eCommand)
		{
			switch(eCommand)
			{
				default:

					return Shortcut.None;

			}// switch(eCommand)

		}// protected virtual Shortcut GetCommandShortcut(ResultsPaneCommands eCommand)

		/// <summary>This method handles events fired by the toolbar manager after displaying the popup menu</summary>
		/// <param name="sender">The toolbar manager firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnUltraAfterCloseup(object sender, Infragistics.Win.UltraWinToolbars.ToolDropdownEventArgs e)
		{
			ResultsPaneCommands eCommand = ResultsPaneCommands.Invalid;

			//	Check each tool in the manager's collection
			foreach(ToolBase O in m_ctrlToolbarManager.Tools)
			{
				//	Get the command for this tool
				if((eCommand = GetCommand(O.Key)) != ResultsPaneCommands.Invalid)
				{
					//	Uninstall the shortcut if required
					if(GetCommandShortcut(eCommand) != Shortcut.None)
						O.SharedProps.Shortcut = Shortcut.None;
				}

			}// foreach(ToolBase O in m_ctrlToolbarManager.Tools)

		}// private void OnUltraAfterCloseup(object sender, Infragistics.Win.UltraWinToolbars.ToolDropdownEventArgs e)

		/// <summary>This method handles events fired by the toolbar manager before displaying the popup menu</summary>
		/// <param name="sender">The toolbar manager firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnUltraBeforeDropDown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventArgs e)
		{
			SetToolStates(true);
		}
		
		/// <summary>This event is fired by the toolbar manager when it is about to display the customize menu</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The cancelable event arguments</param>
		private void OnUltraBeforeToolbarListDropdown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventArgs e)
		{
			// Prevent this menu from coming up
			e.Cancel = true;
		}

		/// <summary>Traps the ToolClick event fired by the toolbar manager</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event argument object</param>
		private void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		{
			ResultsPaneCommands eCommand = ResultsPaneCommands.Invalid;

			//	Get the command
			if(e.Tool != null && e.Tool.Key != null)
				eCommand = GetCommand(e.Tool.Key);

			if(eCommand != ResultsPaneCommands.Invalid)
				OnCommand(eCommand);

		}// OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)

		/// <summary>Called to prompt the user for the path to the file where results should be stored</summary>
		/// <returns>true if succcessful</returns>
		private bool GetSaveFileSpec()
		{
			SaveFileDialog	wndSaveAs = null;
			bool			bSuccessful = false;
			
			try
			{
				//	Initialize the file selection dialog
				wndSaveAs = new SaveFileDialog();
				wndSaveAs.AddExtension = true;
				wndSaveAs.CheckPathExists = true;
				wndSaveAs.OverwritePrompt = true;
				wndSaveAs.DefaultExt = "txt";
				wndSaveAs.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

				if(m_strSaveFileSpec.Length > 0)
				{
					wndSaveAs.FileName = System.IO.Path.GetFileName(m_strSaveFileSpec);
					wndSaveAs.InitialDirectory = System.IO.Path.GetDirectoryName(m_strSaveFileSpec);;
				}

				//	Open the dialog box
				if(wndSaveAs.ShowDialog() == DialogResult.OK)
				{
					m_strSaveFileSpec = wndSaveAs.FileName;
					bSuccessful = true;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSaveFileSpec", m_tmaxErrorBuilder.Message(ERROR_GET_SAVE_FILESPEC_EX), Ex);
			}
			
			return bSuccessful;

		}// private bool GetSaveFileSpec()

		/// <summary>Called to open the output file stream using the specified path</summary>
		/// <param name="strFileSpec">The fully qualified path to the output file stream</param>
		/// <returns>the new IO stream if successful</returns>
		private System.IO.StreamWriter OpenFileStream(string strFileSpec)
		{
			System.IO.FileStream	fs = null;
			System.IO.StreamWriter	streamWriter = null;
			string					strMsg = "";

			try
			{
				//	First try to delete the existing file
				if(System.IO.File.Exists(strFileSpec) == true)
				{
					try
					{
						System.IO.File.Delete(strFileSpec);
					}
					catch
					{
						//	Just use a message box to notify the user
						strMsg = String.Format("Unable to delete {0} to create the new results file.", strFileSpec);
						MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
						return null;
					}

				}// if(System.IO.File.Exists(strFileSpec) == true)
				
				try
				{
					//	Open the stream and create the writer
					if((fs = new FileStream(strFileSpec, FileMode.Create)) != null)
						streamWriter = new StreamWriter(fs, System.Text.Encoding.Default);
				}
				catch
				{
					//	Just use a message box to notify the user
					strMsg = String.Format("Unable to open {0} to save the search results", strFileSpec);
					MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OpenFileStream", m_tmaxErrorBuilder.Message(ERROR_OPEN_FILE_STREAM_EX, strFileSpec), Ex);
				streamWriter = null;
			}

			return streamWriter;

		}// private System.IO.StreamWriter OpenFileStream(string strFileSpec)

		#endregion Private Members

	}// public class CResultsPane : FTI.Trialmax.Panes.CBasePane

}// namespace FTI.Trialmax.Panes
