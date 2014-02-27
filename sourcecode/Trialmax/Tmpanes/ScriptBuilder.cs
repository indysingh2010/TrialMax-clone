using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Threading;

using FTI.Shared;
using FTI.Shared.Win32;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;
using FTI.Trialmax.Database;
using FTI.Trialmax.MSOffice.MSPowerPoint;
using FTI.Trialmax.ActiveX;

using Infragistics.Win;
using Infragistics.Shared;
using Infragistics.Win.UltraWinDock;
using Infragistics.Win.UltraWinToolbars;

namespace FTI.Trialmax.Panes
{
	/// <summary>This class creates the pane used to build TrialMax media scripts</summary>
	public class CScriptBuilder : FTI.Trialmax.Panes.CBasePane
	{
		#region Constants
		
		/// <summary>Local context menu command identifiers</summary>
		protected enum ScriptBuilderCommands
		{
			Invalid = 0,
			Delete,
			Properties,
			Preferences,
			Copy,
			Paste,
			PasteBefore,
			PasteAfter,
			New,
			Viewer,
			Cut,
			PinScript,
			Presentation,
			Tuner,
			Print,
			Find,
			ShowScrollText,
			HideScrollText,
			SetHighlighter,
			SetHighlighter1,
			SetHighlighter2,
			SetHighlighter3,
			SetHighlighter4,
			SetHighlighter5,
			SetHighlighter6,
			SetHighlighter7,
		}
		
		protected const int MINIMUM_SCENE_SEPARATOR_SIZE = 6;
		protected const int MINIMUM_SCENE_VISIBLE = 5;
		protected const int DEFAULT_SELECT_BORDER_SIZE = 4;
		
		protected const int ERROR_PRESENTATION_NOT_FOUND	= (ERROR_BASE_PANE_MAX + 1);
		protected const int ERROR_EXPORT_SLIDE_EX			= (ERROR_BASE_PANE_MAX + 2);
		protected const int ERROR_VIEW_EX					= (ERROR_BASE_PANE_MAX + 3);
		protected const int ERROR_GET_COMMAND_ITEM_EX		= (ERROR_BASE_PANE_MAX + 4);
		protected const int ERROR_ON_UPDATE_EX				= (ERROR_BASE_PANE_MAX + 5);
		protected const int ERROR_ON_ADDED_EX				= (ERROR_BASE_PANE_MAX + 6);
		protected const int ERROR_ON_REORDERED_EX			= (ERROR_BASE_PANE_MAX + 7);
		protected const int ERROR_ON_DELETED_EX				= (ERROR_BASE_PANE_MAX + 8);
		protected const int ERROR_ADD_SCENES_EX				= (ERROR_BASE_PANE_MAX + 9);
		protected const int ERROR_ADD_SELECTION_EX			= (ERROR_BASE_PANE_MAX + 10);
		protected const int ERROR_REMOVE_SELECTION_EX		= (ERROR_BASE_PANE_MAX + 11);
		protected const int ERROR_SET_SELECTION_EX			= (ERROR_BASE_PANE_MAX + 12);
		protected const int ERROR_CREATE_SCENE_EX			= (ERROR_BASE_PANE_MAX + 13);
		protected const int ERROR_FREE_SCENE_EX				= (ERROR_BASE_PANE_MAX + 14);
		protected const int ERROR_LOAD_SCENE_EX				= (ERROR_BASE_PANE_MAX + 15);
		protected const int ERROR_RECALC_LAYOUT_EX			= (ERROR_BASE_PANE_MAX + 16);
		protected const int ERROR_SET_CELLS_EX				= (ERROR_BASE_PANE_MAX + 17);
		protected const int ERROR_SET_FIRST_SCENE_EX		= (ERROR_BASE_PANE_MAX + 18);
		protected const int ERROR_SET_SCROLL_RANGE_EX		= (ERROR_BASE_PANE_MAX + 19);
		protected const int ERROR_SET_SCROLL_POSITION_EX	= (ERROR_BASE_PANE_MAX + 20);
		protected const int ERROR_SET_SCENE_TEXT_EX			= (ERROR_BASE_PANE_MAX + 21);
		protected const int ERROR_ON_CMD_SCROLL_TEXT_EX		= (ERROR_BASE_PANE_MAX + 22);
		
		protected const string KEY_COLUMNS					= "Columns";
		protected const string KEY_SCENE_TEXT_MODE			= "SceneTextMode";
		protected const string KEY_STATUS_TEXT_MODE			= "StatusTextMode";
		
		#endregion Constants

		#region Private Members

		/// <summary>Local member bound to Scenes property</summary>
		private CScriptScenes m_aScenes = new CScriptScenes();
		
		/// <summary>Local collection of rectangles defining scene positions</summary>
		private System.Collections.ArrayList m_aCells = new System.Collections.ArrayList();
		
		/// <summary>Local scene rendering object used to calculate cell positions</summary>
		private CScriptScene m_ctrlScene = new CScriptScene(null);
		
		/// <summary>Delegate to asynchronously load scene files</summary>
		private LoadScenesDelegate m_delLoadScenes = null;
		
		/// <summary>Local member bound to SceneTextMode property</summary>
		protected FTI.Shared.Trialmax.TmaxTextModes m_eSceneTextMode = TmaxTextModes.Barcode;

		/// <summary>Local member bound to StatusTextMode property</summary>
		protected FTI.Shared.Trialmax.TmaxTextModes m_eStatusTextMode = TmaxTextModes.MediaId;

		/// <summary>Primary exchange object for the script to be activated</summary>
		private FTI.Trialmax.Database.CDxPrimary m_dxPrimary = null;
		
		/// <summary>Secondary exchange object for the scene to be activated</summary>
		private FTI.Trialmax.Database.CDxSecondary m_dxActivate = null;
		
		/// <summary>Scene control acting as the anchor</summary>
		private CScriptScene m_Anchor = null;
		
		/// <summary>Local member bound to Columns property</summary>
		private int m_iColumns = 2;
		
		/// <summary>Total number of rows available</summary>
		private int m_iRows = 0;
		
		/// <summary>Total number of visible cells</summary>
		private int	m_iVisibleScenes = 0;
		
		/// <summary>Index of first visible scene</summary>
		private int	m_iFirstVisibleScene = 0;
		
		/// <summary>Index of last visible scene</summary>
		private int m_iLastVisibleScene	= 0;

		/// <summary>Index of the last scene to be ensured visible</summary>
		private int m_iLastEnsuredScene = 0;

		/// <summary>Local member bound to SceneSeparatorSize property</summary>
		private int m_iSceneSeparatorSize = 8;
		
		/// <summary>Width of each scene control in pixels</summary>
		private int m_iSceneWidth = 0;
		
		/// <summary>Height of each scene control in pixels</summary>
		private int m_iSceneHeight = 0;
		
		/// <summary>Height of each row</summary>
		private int m_iRowHeight = 0;
		
		/// <summary>True if the row at the bottom is only partially visible</summary>
		private bool m_bPartialRow = false;
		
		/// <summary>Flag to inhibit processing of activation requests from the application</summary>
		private bool m_bLocalActivation = false;
		
		/// <summary>Infragistics toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager m_ultraToolbarManager;
		
		/// <summary>Left-hand docking area used by the toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CScriptBuilder_Toolbars_Dock_Area_Left;
		
		/// <summary>Right-hand docking area used by the toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CScriptBuilder_Toolbars_Dock_Area_Right;
		
		/// <summary>Top docking area used by the toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CScriptBuilder_Toolbars_Dock_Area_Top;
		
		/// <summary>Bottom docking area used by the toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CScriptBuilder_Toolbars_Dock_Area_Bottom;
		
		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components;

		/// <summary>Rectangle used to define boundries for starting drag operations</summary>
		private System.Drawing.Rectangle m_rcDropIndicator = new System.Drawing.Rectangle(0, 0, 0, 0);
		
		/// <summary>Rectangle used to draw the drop target indicator</summary>
		private System.Drawing.Rectangle m_rcStartDrag = new System.Drawing.Rectangle(0, 0, 0, 0);
		
		/// <summary>Collection of scenes being dragged</summary>
		private CScriptScenes m_aDragSelections = new CScriptScenes();
		
		/// <summary>Index of the scene that is the current drop target</summary>
		private int m_iDropScene = -1;
		
		/// <summary>Index of the cell that is the current drop target</summary>
		private int m_iDropCell = -1;
		
		/// <summary>Flag to indicate if dropping before or after</summary>
		private bool m_bDropBefore = false;
		
		/// <summary>Flag to indicate if we should check the media when the pane gets activated</summary>
		private bool m_bCheckMedia = false;
		
		/// <summary>Flag to indicate if the current script is pinned</summary>
		private bool m_bScriptPinned = false;
		
		/// <summary>Flag to inhibit processing of application activation notifications</summary>
		private bool m_bIgnoreAppActivation = false;
		
		/// <summary>Check box to allow user to hide the scene</summary>
		private System.Windows.Forms.CheckBox m_ctrlHideScene = null;
		
		/// <summary>Label for hide scene check box</summary>
		private Infragistics.Win.UltraWinToolbars.LabelTool m_ctrlHideSceneLabel = null;

		/// <summary>Check box to allow user to enable auto transitioning</summary>
		private System.Windows.Forms.CheckBox m_ctrlAutoTransition;
		
		/// <summary>Label for hide scene check box</summary>
		private Infragistics.Win.UltraWinToolbars.LabelTool m_ctrlAutoTransitionLabel = null;

		/// <summary>Text box to enter auto transition period</summary>
		private Infragistics.Win.UltraWinToolbars.TextBoxTool m_ctrlAutoPeriod = null;

		/// <summary>Label for auto transition period</summary>
		private Infragistics.Win.UltraWinToolbars.LabelTool m_ctrlAutoPeriodLabel = null;

		/// <summary>Control that allows the user to pin the active script</summary>
		private Infragistics.Win.UltraWinToolbars.StateButtonTool m_ctrlPinScript = null;

		/// <summary>Label for Scroll Text check box</summary>
		private Infragistics.Win.UltraWinToolbars.LabelTool m_ctrlScrollTextLabel = null;

		/// <summary>Worker thread for loading the scenes in the script</summary>
		private System.Threading.Thread m_threadLoadScenes = null;

		/// <summary>Background panel for hosting scene controls</summary>
		private CScriptPanel m_ctrlPanel;
	
		/// <summary>Status bar to display active script information</summary>
		private Infragistics.Win.UltraWinStatusBar.UltraStatusBar m_ctrlStatusBar;

		/// <summary>Scroll bar to scroll the visible scenes</summary>
		private System.Windows.Forms.VScrollBar m_ctrlScrollbar;

		/// <summary>.NET workaround - see note in constructor</summary>
		private System.Windows.Forms.TextBox m_ctrlSelectable;

		/// <summary>Image list for context menu</summary>
		protected System.Windows.Forms.ImageList m_ctrlImages;
		
		#endregion Private Members
		
		#region Public Members
		
		/// <summary>This method is called by the application to activate the specified item</summary>
		/// <param name="tmaxItem">The item to be activated</param>
		/// <param name="ePane">The pane requesting activation</param>
		/// <returns>true if successful</returns>
		public override bool Activate(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
			//	Is this being called as the result of a local activation?
			if(m_bLocalActivation == true) return true;
			
			//	What type of media is being activated?
			switch(tmaxItem.MediaType)
			{
				case TmaxMediaTypes.Script:
				case TmaxMediaTypes.Scene:
				
					//	Is this the current script?
					if(ReferenceEquals(m_aScenes.Primary, tmaxItem.IPrimary) == true)
					{
						//	Update the local record references
						if(SetRecords(tmaxItem) == true)
						{
							//	Select this scene if the pane is active
							if(m_bPaneVisible == true)
								View();
						}
						
					}
					else
					{
						if(m_bScriptPinned == false)
							return Open(tmaxItem, ePane);
					}
					
					break;
					
				default:
				
					//	Nothing to do
					break;
			}
			
			return true;
			
		}// public override bool Activate(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		/// <summary>Default constructor</summary>
		public CScriptBuilder()
		{
			//	Initialize the child controls
			InitializeComponent();
			
			m_tmaxEventSource.Attach(m_ctrlPanel.EventSource);
			
			m_aScenes.SelectionChanging += new CScriptScenes.SelectionChangingHandler(this.OnSelectionChanging);
			m_aScenes.SelectionChanged += new CScriptScenes.SelectionChangedHandler(this.OnSelectionChanged);
			
			//	Force the control to BitBlt when painting
			//this.SetStyle(ControlStyles.DoubleBuffer, true);
			//this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			m_ctrlScene.Visible = false;
			
			//	For some reason we need to put a selectable control
			//	on the panel or else .NET gets confused some times and
			//	refuses to let us close the application
			m_ctrlSelectable.Width = 0;
			m_ctrlSelectable.Height = 0;
			
			//	Initilize the size of the rectangle used to determine if we should start dragging
			m_rcStartDrag.Width = System.Windows.Forms.SystemInformation.DragSize.Width;
			m_rcStartDrag.Height = System.Windows.Forms.SystemInformation.DragSize.Height;
			m_aDragSelections.KeepSorted = false;

			//	Allocate the delegate and thread used for asynchronous loading
			m_delLoadScenes = new LoadScenesDelegate(this.LoadScene);
				
			//	Get the Infragistics tools from the toolbar manager
			m_ctrlHideSceneLabel = (LabelTool)GetUltraTool("HideSceneLabel");
			m_ctrlAutoTransitionLabel = (LabelTool)GetUltraTool("AutoTransitionLabel");
			m_ctrlAutoPeriod = (TextBoxTool)GetUltraTool("AutoPeriod");
			m_ctrlAutoPeriodLabel = (LabelTool)GetUltraTool("AutoPeriodLabel");
			m_ctrlPinScript = (StateButtonTool)GetUltraTool("PinScript");
			m_ctrlScrollTextLabel = (LabelTool)GetUltraTool("ScrollTextLabel");
			
			//	Initialize the control states
			SetControlStates();
			
		}// public CScriptBuilder()

		/// <summary>This method is called by the application to initialize the pane</summary>
		/// <returns>true if successful</returns>
		public override bool Initialize(CXmlIni xmlINI)
		{
			//	Do the base class processing first
			if(base.Initialize(xmlINI) == false)
				return false;
				
			//	Make sure we are on the correct section
			if(xmlINI.SetSection(m_strPaneName) == false) return false;
				
			m_iColumns = xmlINI.ReadInteger(KEY_COLUMNS, m_iColumns);
			m_eStatusTextMode = (TmaxTextModes)xmlINI.ReadEnum(KEY_STATUS_TEXT_MODE, m_eStatusTextMode);
			m_eSceneTextMode = (TmaxTextModes)xmlINI.ReadEnum(KEY_SCENE_TEXT_MODE, m_eSceneTextMode);
			
			return true;
		}
		
		/// <summary>This function is called when the ApplicationOptions property changes</summary>
		public override void OnApplicationActivated()
		{
			//	Should we ignore this notification?
			if(m_bIgnoreAppActivation == true) return;
			
			//	Make sure the user hasn't returned to the application after
			//	modifying the current media
			if(PaneVisible == true)
			{
				//m_tmaxEventSource.FireDiagnostic(this, "ONApp", "app activated - active");
			
				CheckMedia();
			}
			else
			{
				//m_tmaxEventSource.FireDiagnostic(this, "ONApp", "app activated - not active");
				m_bCheckMedia = true;
			}
		}
		
		/// <summary>This method is called by the application to notify the panes to refresh their text</summary>
		public override void RefreshText()
		{
			try
			{
				SetStatusText();
				SetSceneText();
			}
			catch
			{
			}
		
		}// public override void RefreshText()
		
		/// <summary>This method handles all Hotkey notifications from the application</summary>
		/// <param name="eHotkey">The enumerated hotkey</param>
		/// <returns>true if processed by the pane</returns>
		public override bool OnHotkey(TmaxHotkeys eHotkey)
		{
			ScriptBuilderCommands	eCommand = ScriptBuilderCommands.Invalid;
			CScriptScenes			Selections = null;
			
			//	Get the current selections
			if(m_aScenes != null)
				Selections = m_aScenes.GetSelections();
			
			//	Which hotkey has been pressed?
			switch(eHotkey)
			{
				case TmaxHotkeys.Print:
				
					eCommand = ScriptBuilderCommands.Print;
					break;
					
				case TmaxHotkeys.Find:
				
					eCommand = ScriptBuilderCommands.Find;
					break;
					
				case TmaxHotkeys.Copy:
				
					eCommand = ScriptBuilderCommands.Copy;
					break;

				case TmaxHotkeys.Delete:

					eCommand = ScriptBuilderCommands.Delete;
					break;

				case TmaxHotkeys.Paste:
				
					//	If Paste is enabled use that, otherwise use
					//	Paste Before
					if(GetCommandEnabled(ScriptBuilderCommands.Paste, Selections) == true)
						eCommand = ScriptBuilderCommands.Paste;
					else if(GetCommandEnabled(ScriptBuilderCommands.PasteBefore, Selections) == true)
						eCommand = ScriptBuilderCommands.PasteBefore;
						
					break;
					
				case TmaxHotkeys.AddToBinder:
				case TmaxHotkeys.AddToScript:
				default:
				
					//MessageBox.Show(eHotkey.ToString() + " hotkey not yet implemented in " + PaneName);
					break;
			}
		
			//	Did this hotkey translate to a command?
			if(eCommand != ScriptBuilderCommands.Invalid)
			{
				//	Is this command enabled
				if(GetCommandEnabled(eCommand, Selections) == true)
				{
					//	Prompt for confirmation if attempting to delete records
					if((eCommand != ScriptBuilderCommands.Delete) || (GetDeleteConfirmation() == true))
						OnCommand(eCommand);
				}

			}// if(eCommand != ScriptBuilderCommands.Invalid)
			
			return (eCommand != ScriptBuilderCommands.Invalid);
			
		}// public override bool OnHotkey(TmaxHotkeys eHotkey)
		
		/// <summary>This method is called by the application when a new search result has been activated</summary>
		/// <param name="tmaxResult">The search result to be activated</param>
		public override void OnActivateResult(FTI.Shared.Trialmax.CTmaxSearchResult tmaxResult)
		{
			CTmaxItem tmaxItem = null;
			
			//	Don't bother if not visible
			if(PaneVisible == false) return;
			
			//	Don't bother if this is not a scene
			if(tmaxResult.IScene == null) return;
			
			try
			{
				//	Activate this record
				tmaxItem = new CTmaxItem(tmaxResult.IScene);
				Activate(tmaxItem, TmaxAppPanes.Results);
			}
			catch
			{
			}
		
		}// public override void OnActivateResult(FTI.Shared.Trialmax.CTmaxSearchResult tmaxResult)
		
		/// <summary>This method is called by the application when it adds new media to the database</summary>
		/// <param name="tmaxParent">TrialMax event item that identifies the parent record</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		{
			CScriptScene Scene = null;
			CScriptScene First = null;
			
			try
			{
				//	Is this a new primary record being added?
				if(tmaxParent.MediaLevel == TmaxMediaLevels.None)
				{
					//	Add a selection to the list box for each new script
					foreach(CTmaxItem O in tmaxChildren)
					{
						if((O.IPrimary != null) && (O.MediaType == TmaxMediaTypes.Script))
						{
							AddSelection((CDxPrimary)O.IPrimary);
						}
						
					}
					
				}
				else
				{
					if(m_aScenes == null) return;
					if(m_aScenes.Primary == null) return;
					if(tmaxParent.IPrimary == null) return;

					//	Update the scene text if the user has added to non-script media
					if(tmaxParent.MediaType != TmaxMediaTypes.Script)
					{
						SetSceneText();
						return;
					}
					
					//	Nothing to do if this is not the active script
					if(m_aScenes.Primary.AutoId != tmaxParent.IPrimary.GetAutoId()) return;

					//	Turn off sorting while we add the new scenes
					m_aScenes.KeepSorted = false;
						
					//	Add a scene for each event item
					foreach(CTmaxItem tmaxScene in tmaxChildren)
					{
						Debug.Assert(tmaxScene.MediaLevel == TmaxMediaLevels.Secondary);
						Debug.Assert(tmaxScene.MediaType == TmaxMediaTypes.Scene);
						Debug.Assert(tmaxScene.ISecondary != null);

						if((Scene = CreateScene((CDxSecondary)tmaxScene.ISecondary)) != null)
						{
							//	Add to the local collection
							m_aScenes.Add(Scene);
							
							//	Keep track of the first new scene
							if(First == null)
								First = Scene;
						
						}// if((Scene = CreateScene(dxScene)) != null)
								
					}// foreach(CTmaxItem tmaxScene in tmaxChildren) 
						
					//	Restore sorting
					m_aScenes.Sort(true);
					m_aScenes.KeepSorted = true;
					
					//	Make sure the scenes are displaying the correct text
					SetSceneText();
					
					//	Update the scrollbar
					SetScrollRange();
					SetScrollPosition();
					
//					//	Make the first new scene the first visible
//					if(First != null)
//					{
//						//	This makes sure the request to make visible gets processed
//						m_iFirstVisibleScene = -1;
//						m_iLastVisibleScene = -1; 
//							
//						//	Is this pane active
//						if(m_bActive == true)
//						{
//							EnsureVisible(m_aScenes.IndexOf(First), true);
//							
//							//	Activate this scene
//							Activate(First);
//						}
//						else
//						{
//							//	Set up for activation when the pane is made visible
//							m_dxActivate = First.Secondary;
//						}
//					
//					}
					
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnAdded", m_tmaxErrorBuilder.Message(ERROR_ON_ADDED_EX), Ex);
			}
				
		}// public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		
		/// <summary>This function is called when the value of the Database property changes</summary>
		protected override void OnDatabaseChanged()
		{
			//	Do the base class processing first
			base.OnDatabaseChanged();

			//	Unload the viewer whenever the database changes
			Unload();
			
			//	Reset the script selection list
			FillScripts();
						
		}// OnDatabaseChanged()
		
		/// <summary>This method is called by the application to when the item gets deleted</summary>
		/// <param name="tmaxItem">The items that have been deleted</param>
		public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			//	Iterate the collection of parent items
			foreach(CTmaxItem O in tmaxItems)
			{
				try
				{
					//	Don't bother if the user has deleted pick list items
					if(O.DataType == TmaxDataTypes.PickItem)
						break;
						
					OnDeleted(O);
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "OnDeleted", m_tmaxErrorBuilder.Message(ERROR_ON_DELETED_EX), Ex);
				}
				
			}

		}// public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		
		/// <summary>This method is called by the application to when the item has been updated by the user</summary>
		/// <param name="tmaxItem">The item that has been updated</param>
		public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{
			int iIndex = 0;
			
			try
			{
				switch(tmaxItem.MediaType)
				{
					case TmaxMediaTypes.Script:
					
						Debug.Assert(tmaxItem.IPrimary != null);
						if(tmaxItem.IPrimary != null)
						{
							//	Update the text in the list box
							SetScriptText((CDxPrimary)(tmaxItem.IPrimary));
							
							//	Is this the active script?
							if((m_aScenes.Primary != null) &&
							(ReferenceEquals(m_aScenes.Primary, tmaxItem.IPrimary) == true))
							{
								SetStatusText();
								SetSceneText();
							}
							
						}
						break;
						
					case TmaxMediaTypes.Scene:
					
						Debug.Assert(tmaxItem.ISecondary != null);
						
						if((m_aScenes != null) && (m_aScenes.Primary != null) && (tmaxItem.ISecondary != null))
						{
							//	Is this one of our current scenes?
							if((iIndex = m_aScenes.GetIndex((CDxSecondary)tmaxItem.ISecondary)) >= 0)
							{
								m_aScenes[iIndex].StatusText = GetSceneText((CDxSecondary)(tmaxItem.ISecondary));
							}
						}
						break;
						
					case TmaxMediaTypes.Unknown:
					
						break;
						
					default:
					
						//	Could be source for one of our active scenes
						SetSceneText();
						break;
						
				}// switch(tmaxItem.MediaType)
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnUpdated", m_tmaxErrorBuilder.Message(ERROR_ON_UPDATE_EX), Ex);
			}
					
		}// public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		/// <param name="tmaxItems">The collection of items that identify the deleted scenes</param>
		protected void OnScenesDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			int		iIndex = 0;
			bool	bReposition = false;
			
			if(m_aScenes == null) return;
			if(m_aScenes.Count == 0) return;
			if(m_aScenes.Primary == null) return;
			if(tmaxItems == null) return;
			if(tmaxItems.Count == 0) return;
			
			foreach(CTmaxItem tmaxItem in tmaxItems)
			{
				Debug.Assert(tmaxItem.MediaLevel == TmaxMediaLevels.Secondary);
				Debug.Assert(tmaxItem.ISecondary != null);
				if(tmaxItem.MediaLevel != TmaxMediaLevels.Secondary) continue;
				if(tmaxItem.ISecondary == null) continue;
									
				//	Get the index of the scene
				iIndex = m_aScenes.GetIndex((CDxSecondary)tmaxItem.ISecondary);
				if(iIndex < 0) continue;

				//	Is this scene currently visible
				if(m_aScenes[iIndex].Visible == true)
				{
					//	We are going to need to reposition the scenes
					bReposition = true;
					
					//	Are we deleting the first visible scene?
					if(iIndex == m_iFirstVisibleScene)
					{
						//	Adjust to keep same scenes visible when done
						if(m_iFirstVisibleScene > 0)
							m_iFirstVisibleScene--;
					}
					
				}
				else
				{
					//	Adjust the first visible scene to keep it on the same object
					if(iIndex < m_iFirstVisibleScene)
						m_iFirstVisibleScene--;
				}
				
				//	Remove the scene
				FreeScene(m_aScenes[iIndex]);
				
			}// foreach(CTmaxItem tmaxItem in tmaxItems)
			
			//	Do we need to reposition?
			if(bReposition == true)
			{
				if(m_iFirstVisibleScene >= 0)
				{
					SetFirstScene(m_iFirstVisibleScene);
				}
				else
				{
					SetFirstScene(0);
				}
				
			}
			
			//	Make sure the scenes are displaying the correct text
			SetSceneText();
			
			//	Update the scrollbar
			SetScrollRange();
			SetScrollPosition();
			
		}// public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		/// <summary>This method is called by the application to when the item's child collection has been reordered</summary>
		/// <param name="tmaxItem">The item that owns the child collection</param>
		public override void OnReordered(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{
			CScriptScene FirstVisible = null;
			
			if(m_aScenes == null) return;
			if(m_aScenes.Count == 0) return;
			if(m_aScenes.Primary == null) return;
			if(tmaxItem.IPrimary == null) return;
			if(tmaxItem.DataType == TmaxDataTypes.CaseCode) return;
			
			try
			{
				//	Is this the current script?
				if(m_aScenes.Primary.AutoId == tmaxItem.IPrimary.GetAutoId())
				{
					//	Get the first scene 
					if((m_iFirstVisibleScene >= 0) && (m_iFirstVisibleScene < m_aScenes.Count))
						FirstVisible = m_aScenes[m_iFirstVisibleScene];
						
					//	Sort the scenes
					m_aScenes.Sort();
					
					//	Make sure the scenes are displaying the correct text
					SetSceneText();
				
					//	Reposition the scenes
					if(m_aScenes.GetSelection() != null)
					{
						SetFirstScene(m_aScenes.IndexOf(m_aScenes.GetSelection()));
					}
					else if(FirstVisible != null)
					{
						SetFirstScene(m_aScenes.IndexOf(FirstVisible));
					}
					else
					{
						SetFirstScene(0);
					}
				}
				else
				{
					//	The user may have reordered source media
					SetSceneText();
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnReordered", m_tmaxErrorBuilder.Message(ERROR_ON_REORDERED_EX), Ex);
			}
			
		}// public override void OnReordered(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		/// <summary>This method is called by the application to open the specified item</summary>
		/// <param name="tmaxItem">The item to be opened</param>
		/// <param name="ePane">The pane making the request</param>
		/// <returns>true if successful</returns>
		public override bool Open(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
			//	Is this a script or scene?
			if((tmaxItem.MediaType == TmaxMediaTypes.Script) || 
			   (tmaxItem.MediaType == TmaxMediaTypes.Scene))
			{
				//	Make sure the local references are up to date
				SetRecords(tmaxItem);

				//	View the new scenes if the pane is active
				if(m_bPaneVisible == true)
				{
					return View();
				}
			}

			return true;
				
		}// public override bool Open(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		/// <summary>This method is called by the application when it is about to terminate</summary>
		public override void Terminate(CXmlIni xmlINI)
		{
			if(xmlINI.SetSection(m_strPaneName) == true)
			{
				xmlINI.Write(KEY_COLUMNS, m_iColumns);
				xmlINI.Write(KEY_STATUS_TEXT_MODE, m_eStatusTextMode);
				xmlINI.Write(KEY_SCENE_TEXT_MODE, m_eSceneTextMode);
			}
			
			//	Flush the existing scenes
			FreeScenes();
			
			//	Do the base class termination
			base.Terminate(xmlINI);
			
		}// public override void Terminate(CXmlIni xmlINI)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Clean up all resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			if(disposing == true)
			{
				if(m_aCells != null)
					m_aCells.Clear();
			}
			base.Dispose(disposing);
		
		}// protected override void Dispose(bool disposing)
		
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected override void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("MainToolbar");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("PinScript", "");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("ScriptsLabel");
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool1 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("Scripts");
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar2 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("ScenesBar");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool1 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("HideScene");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("HideSceneLabel");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool2 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("AutoTransition");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool3 = new Infragistics.Win.UltraWinToolbars.LabelTool("AutoTransitionLabel");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool1 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("AutoPeriod");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool4 = new Infragistics.Win.UltraWinToolbars.LabelTool("AutoPeriodLabel");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ContextMenu");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Preferences");
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PasteBefore");
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PasteAfter");
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ContextMenu");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("PinScript", "");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Properties");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Presentation");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Tuner");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Viewer");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Print");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Find");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Copy");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Cut");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Paste");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PasteBefore");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PasteAfter");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("New");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ShowScrollText");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("HideScrollText");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool3 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("SetHighlighter");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Preferences");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool5 = new Infragistics.Win.UltraWinToolbars.LabelTool("Spacer");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete");
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Copy");
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool22 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Properties");
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool23 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Paste");
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool24 = new Infragistics.Win.UltraWinToolbars.ButtonTool("New");
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool25 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Viewer");
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool2 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("Scripts");
            Infragistics.Win.ValueList valueList1 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool6 = new Infragistics.Win.UltraWinToolbars.LabelTool("AutoTransitionLabel");
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool3 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("AutoTransition");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool7 = new Infragistics.Win.UltraWinToolbars.LabelTool("ScriptsLabel");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool26 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Cut");
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool2 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("AutoPeriod");
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool4 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("HideScene");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool8 = new Infragistics.Win.UltraWinToolbars.LabelTool("HideSceneLabel");
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool3 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("PinScript", "");
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool9 = new Infragistics.Win.UltraWinToolbars.LabelTool("AutoPeriodLabel");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool27 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Presentation");
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool28 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Tuner");
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool29 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Print");
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool30 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Find");
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool31 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ShowScrollText");
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool32 = new Infragistics.Win.UltraWinToolbars.ButtonTool("HideScrollText");
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool4 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("SetHighlighter");
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool4 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter1", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool5 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter2", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool6 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter3", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool7 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter4", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool8 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter5", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool9 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter6", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool10 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter7", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool11 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter1", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool12 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter2", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool13 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter3", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool14 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter4", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool15 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter5", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool16 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter6", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool17 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter7", "");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CScriptBuilder));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel2 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.m_ctrlHideScene = new System.Windows.Forms.CheckBox();
            this.m_ctrlAutoTransition = new System.Windows.Forms.CheckBox();
            this.m_ultraToolbarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
            this._CScriptBuilder_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._CScriptBuilder_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._CScriptBuilder_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._CScriptBuilder_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.m_ctrlPanel = new FTI.Trialmax.Panes.CScriptPanel();
            this.m_ctrlSelectable = new System.Windows.Forms.TextBox();
            this.m_ctrlStatusBar = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.m_ctrlScrollbar = new System.Windows.Forms.VScrollBar();
            ((System.ComponentModel.ISupportInitialize)(this.m_ultraToolbarManager)).BeginInit();
            this.m_ctrlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_ctrlHideScene
            // 
            this.m_ctrlHideScene.BackColor = System.Drawing.Color.Transparent;
            this.m_ctrlHideScene.Location = new System.Drawing.Point(4, 28);
            this.m_ctrlHideScene.Name = "m_ctrlHideScene";
            this.m_ctrlHideScene.Size = new System.Drawing.Size(16, 16);
            this.m_ctrlHideScene.TabIndex = 9;
            this.m_ctrlHideScene.UseVisualStyleBackColor = false;
            // 
            // m_ctrlAutoTransition
            // 
            this.m_ctrlAutoTransition.BackColor = System.Drawing.Color.Transparent;
            this.m_ctrlAutoTransition.Location = new System.Drawing.Point(4, 44);
            this.m_ctrlAutoTransition.Name = "m_ctrlAutoTransition";
            this.m_ctrlAutoTransition.Size = new System.Drawing.Size(16, 16);
            this.m_ctrlAutoTransition.TabIndex = 7;
            this.m_ctrlAutoTransition.UseVisualStyleBackColor = false;
            this.m_ctrlAutoTransition.Click += new System.EventHandler(this.OnClickAutoTransition);
            // 
            // m_ultraToolbarManager
            // 
            this.m_ultraToolbarManager.DesignerFlags = 1;
            this.m_ultraToolbarManager.DockWithinContainer = this;
            this.m_ultraToolbarManager.ImageListSmall = this.m_ctrlImages;
            this.m_ultraToolbarManager.LockToolbars = true;
            this.m_ultraToolbarManager.ShowFullMenusDelay = 500;
            this.m_ultraToolbarManager.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.Office2003;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            labelTool1.InstanceProps.IsFirstInGroup = true;
            comboBoxTool1.InstanceProps.Width = 154;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool1,
            labelTool1,
            comboBoxTool1});
            ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.Settings.FillEntireRow = Infragistics.Win.DefaultableBoolean.True;
            ultraToolbar1.Settings.GrabHandleStyle = Infragistics.Win.UltraWinToolbars.GrabHandleStyle.None;
            ultraToolbar1.Settings.PaddingLeft = 7;
            ultraToolbar1.Settings.PaddingRight = 0;
            ultraToolbar1.Settings.ToolSpacing = 0;
            ultraToolbar1.ShowInToolbarList = false;
            ultraToolbar1.Text = "ScriptsBar";
            ultraToolbar2.DockedColumn = 0;
            ultraToolbar2.DockedRow = 1;
            controlContainerTool1.Control = this.m_ctrlHideScene;
            labelTool2.InstanceProps.Width = 38;
            controlContainerTool2.Control = this.m_ctrlAutoTransition;
            labelTool3.InstanceProps.Width = 89;
            textBoxTool1.InstanceProps.Width = 36;
            ultraToolbar2.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            controlContainerTool1,
            labelTool2,
            controlContainerTool2,
            labelTool3,
            textBoxTool1,
            labelTool4,
            popupMenuTool1});
            ultraToolbar2.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar2.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar2.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar2.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar2.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar2.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar2.Settings.FillEntireRow = Infragistics.Win.DefaultableBoolean.True;
            ultraToolbar2.Settings.GrabHandleStyle = Infragistics.Win.UltraWinToolbars.GrabHandleStyle.None;
            ultraToolbar2.Settings.PaddingLeft = 8;
            ultraToolbar2.ShowInToolbarList = false;
            ultraToolbar2.Text = "ScenesBar";
            this.m_ultraToolbarManager.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1,
            ultraToolbar2});
            this.m_ultraToolbarManager.ToolbarSettings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            this.m_ultraToolbarManager.ToolbarSettings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
            this.m_ultraToolbarManager.ToolbarSettings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
            this.m_ultraToolbarManager.ToolbarSettings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
            this.m_ultraToolbarManager.ToolbarSettings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
            this.m_ultraToolbarManager.ToolbarSettings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            this.m_ultraToolbarManager.ToolbarSettings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            this.m_ultraToolbarManager.ToolbarSettings.GrabHandleStyle = Infragistics.Win.UltraWinToolbars.GrabHandleStyle.None;
            appearance4.Image = 0;
            buttonTool1.SharedProps.AppearancesSmall.Appearance = appearance4;
            buttonTool1.SharedProps.Caption = "Preferences ...";
            appearance5.Image = 15;
            buttonTool2.SharedProps.AppearancesSmall.Appearance = appearance5;
            buttonTool2.SharedProps.Caption = "Paste Before";
            appearance6.Image = 14;
            buttonTool3.SharedProps.AppearancesSmall.Appearance = appearance6;
            buttonTool3.SharedProps.Caption = "Paste After";
            popupMenuTool2.SharedProps.Caption = "ContextMenu";
            popupMenuTool2.SharedProps.Visible = false;
            buttonTool4.InstanceProps.IsFirstInGroup = true;
            buttonTool8.InstanceProps.IsFirstInGroup = true;
            buttonTool11.InstanceProps.IsFirstInGroup = true;
            buttonTool16.InstanceProps.IsFirstInGroup = true;
            buttonTool17.InstanceProps.IsFirstInGroup = true;
            buttonTool19.InstanceProps.IsFirstInGroup = true;
            popupMenuTool2.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool2,
            buttonTool4,
            buttonTool5,
            buttonTool6,
            buttonTool7,
            buttonTool8,
            buttonTool9,
            buttonTool10,
            buttonTool11,
            buttonTool12,
            buttonTool13,
            buttonTool14,
            buttonTool15,
            buttonTool16,
            buttonTool17,
            buttonTool18,
            popupMenuTool3,
            buttonTool19});
            labelTool5.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            appearance7.Image = 2;
            buttonTool20.SharedProps.AppearancesSmall.Appearance = appearance7;
            buttonTool20.SharedProps.Caption = "Delete";
            appearance8.Image = 16;
            buttonTool21.SharedProps.AppearancesSmall.Appearance = appearance8;
            buttonTool21.SharedProps.Caption = "Copy";
            appearance9.Image = 3;
            buttonTool22.SharedProps.AppearancesSmall.Appearance = appearance9;
            buttonTool22.SharedProps.Caption = "Open in Properties";
            appearance10.Image = 13;
            buttonTool23.SharedProps.AppearancesSmall.Appearance = appearance10;
            buttonTool23.SharedProps.Caption = "Paste";
            appearance11.Image = 12;
            buttonTool24.SharedProps.AppearancesSmall.Appearance = appearance11;
            buttonTool24.SharedProps.Caption = "New Script ...";
            appearance12.Image = 1;
            buttonTool25.SharedProps.AppearancesSmall.Appearance = appearance12;
            buttonTool25.SharedProps.Caption = "Open in Viewer";
            comboBoxTool2.SharedProps.Caption = "Scripts";
            comboBoxTool2.SharedProps.Spring = true;
            comboBoxTool2.SharedProps.ToolTipText = "Choose A Script";
            comboBoxTool2.ValueList = valueList1;
            appearance13.TextHAlignAsString = "Left";
            labelTool6.SharedProps.AppearancesSmall.AppearanceOnToolbar = appearance13;
            labelTool6.SharedProps.Caption = "Auto Transition";
            labelTool6.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            labelTool6.SharedProps.ToolTipText = "Automatic Transition";
            controlContainerTool3.Control = this.m_ctrlAutoTransition;
            controlContainerTool3.SharedProps.Caption = "Automatic Transition";
            controlContainerTool3.SharedProps.ToolTipText = "Check for Auto Transition";
            labelTool7.SharedProps.Caption = "Scripts:";
            labelTool7.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            appearance14.Image = 17;
            buttonTool26.SharedProps.AppearancesSmall.Appearance = appearance14;
            buttonTool26.SharedProps.Caption = "Cut";
            appearance15.BackColorDisabled = System.Drawing.SystemColors.InactiveCaptionText;
            textBoxTool2.SharedProps.AppearancesSmall.AppearanceOnToolbar = appearance15;
            textBoxTool2.SharedProps.Caption = "Time to Next";
            controlContainerTool4.Control = this.m_ctrlHideScene;
            controlContainerTool4.SharedProps.Caption = "Hide Scene";
            appearance16.TextHAlignAsString = "Left";
            labelTool8.SharedProps.AppearancesSmall.AppearanceOnToolbar = appearance16;
            labelTool8.SharedProps.Caption = "Hide";
            labelTool8.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            labelTool8.SharedProps.MinWidth = 0;
            labelTool8.SharedProps.ToolTipText = "Hide Scene";
            labelTool8.SharedProps.Width = 5;
            appearance17.Image = 19;
            stateButtonTool3.SharedProps.AppearancesSmall.Appearance = appearance17;
            stateButtonTool3.SharedProps.Caption = "Pin Script";
            labelTool9.SharedProps.Caption = "sec";
            labelTool9.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            appearance18.Image = 20;
            buttonTool27.SharedProps.AppearancesSmall.Appearance = appearance18;
            buttonTool27.SharedProps.Caption = "Open in Presentation ...";
            appearance19.Image = 21;
            buttonTool28.SharedProps.AppearancesSmall.Appearance = appearance19;
            buttonTool28.SharedProps.Caption = "Open In Tuner";
            appearance20.Image = 22;
            buttonTool29.SharedProps.AppearancesSmall.Appearance = appearance20;
            buttonTool29.SharedProps.Caption = "Print ...";
            appearance21.Image = 23;
            buttonTool30.SharedProps.AppearancesSmall.Appearance = appearance21;
            buttonTool30.SharedProps.Caption = "Find ...";
            appearance22.Image = 24;
            buttonTool31.SharedProps.AppearancesSmall.Appearance = appearance22;
            buttonTool31.SharedProps.Caption = "Show Scrolling Text";
            appearance23.Image = 25;
            buttonTool32.SharedProps.AppearancesSmall.Appearance = appearance23;
            buttonTool32.SharedProps.Caption = "Hide Scrolling Text";
            appearance24.Image = 26;
            popupMenuTool4.SharedProps.AppearancesSmall.Appearance = appearance24;
            popupMenuTool4.SharedProps.Caption = "Set Highlighter";
            popupMenuTool4.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool4,
            stateButtonTool5,
            stateButtonTool6,
            stateButtonTool7,
            stateButtonTool8,
            stateButtonTool9,
            stateButtonTool10});
            stateButtonTool11.SharedProps.Caption = "SetHighlighter1";
            stateButtonTool12.SharedProps.Caption = "SetHighlighter2";
            stateButtonTool13.SharedProps.Caption = "SetHighlighter3";
            stateButtonTool14.SharedProps.Caption = "SetHighlighter4";
            stateButtonTool15.SharedProps.Caption = "SetHighlighter5";
            stateButtonTool16.SharedProps.Caption = "SetHighlighter6";
            stateButtonTool17.SharedProps.Caption = "SetHighlighter7";
            this.m_ultraToolbarManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool2,
            buttonTool3,
            popupMenuTool2,
            labelTool5,
            buttonTool20,
            buttonTool21,
            buttonTool22,
            buttonTool23,
            buttonTool24,
            buttonTool25,
            comboBoxTool2,
            labelTool6,
            controlContainerTool3,
            labelTool7,
            buttonTool26,
            textBoxTool2,
            controlContainerTool4,
            labelTool8,
            stateButtonTool3,
            labelTool9,
            buttonTool27,
            buttonTool28,
            buttonTool29,
            buttonTool30,
            buttonTool31,
            buttonTool32,
            popupMenuTool4,
            stateButtonTool11,
            stateButtonTool12,
            stateButtonTool13,
            stateButtonTool14,
            stateButtonTool15,
            stateButtonTool16,
            stateButtonTool17});
            this.m_ultraToolbarManager.ToolValueChanged += new Infragistics.Win.UltraWinToolbars.ToolEventHandler(this.OnUltraToolValueChanged);
            this.m_ultraToolbarManager.BeforeToolDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventHandler(this.OnUltraBeforeDropDown);
            this.m_ultraToolbarManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.OnUltraToolClick);
            this.m_ultraToolbarManager.AfterToolCloseup += new Infragistics.Win.UltraWinToolbars.ToolDropdownEventHandler(this.OnUltraAfterCloseup);
            this.m_ultraToolbarManager.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.OnUltraBeforeToolbarListDropdown);
            // 
            // m_ctrlImages
            // 
            this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
            this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
            this.m_ctrlImages.Images.SetKeyName(0, "");
            this.m_ctrlImages.Images.SetKeyName(1, "");
            this.m_ctrlImages.Images.SetKeyName(2, "");
            this.m_ctrlImages.Images.SetKeyName(3, "");
            this.m_ctrlImages.Images.SetKeyName(4, "");
            this.m_ctrlImages.Images.SetKeyName(5, "");
            this.m_ctrlImages.Images.SetKeyName(6, "");
            this.m_ctrlImages.Images.SetKeyName(7, "");
            this.m_ctrlImages.Images.SetKeyName(8, "");
            this.m_ctrlImages.Images.SetKeyName(9, "");
            this.m_ctrlImages.Images.SetKeyName(10, "");
            this.m_ctrlImages.Images.SetKeyName(11, "");
            this.m_ctrlImages.Images.SetKeyName(12, "");
            this.m_ctrlImages.Images.SetKeyName(13, "");
            this.m_ctrlImages.Images.SetKeyName(14, "");
            this.m_ctrlImages.Images.SetKeyName(15, "");
            this.m_ctrlImages.Images.SetKeyName(16, "");
            this.m_ctrlImages.Images.SetKeyName(17, "");
            this.m_ctrlImages.Images.SetKeyName(18, "");
            this.m_ctrlImages.Images.SetKeyName(19, "");
            this.m_ctrlImages.Images.SetKeyName(20, "");
            this.m_ctrlImages.Images.SetKeyName(21, "");
            this.m_ctrlImages.Images.SetKeyName(22, "");
            this.m_ctrlImages.Images.SetKeyName(23, "");
            this.m_ctrlImages.Images.SetKeyName(24, "");
            this.m_ctrlImages.Images.SetKeyName(25, "");
            this.m_ctrlImages.Images.SetKeyName(26, "");
            // 
            // _CScriptBuilder_Toolbars_Dock_Area_Left
            // 
            this._CScriptBuilder_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CScriptBuilder_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._CScriptBuilder_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._CScriptBuilder_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CScriptBuilder_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 52);
            this._CScriptBuilder_Toolbars_Dock_Area_Left.Name = "_CScriptBuilder_Toolbars_Dock_Area_Left";
            this._CScriptBuilder_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 232);
            this._CScriptBuilder_Toolbars_Dock_Area_Left.ToolbarsManager = this.m_ultraToolbarManager;
            // 
            // _CScriptBuilder_Toolbars_Dock_Area_Right
            // 
            this._CScriptBuilder_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CScriptBuilder_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._CScriptBuilder_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._CScriptBuilder_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CScriptBuilder_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(492, 52);
            this._CScriptBuilder_Toolbars_Dock_Area_Right.Name = "_CScriptBuilder_Toolbars_Dock_Area_Right";
            this._CScriptBuilder_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 232);
            this._CScriptBuilder_Toolbars_Dock_Area_Right.ToolbarsManager = this.m_ultraToolbarManager;
            // 
            // _CScriptBuilder_Toolbars_Dock_Area_Top
            // 
            this._CScriptBuilder_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CScriptBuilder_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._CScriptBuilder_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._CScriptBuilder_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CScriptBuilder_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._CScriptBuilder_Toolbars_Dock_Area_Top.Name = "_CScriptBuilder_Toolbars_Dock_Area_Top";
            this._CScriptBuilder_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(492, 52);
            this._CScriptBuilder_Toolbars_Dock_Area_Top.ToolbarsManager = this.m_ultraToolbarManager;
            // 
            // _CScriptBuilder_Toolbars_Dock_Area_Bottom
            // 
            this._CScriptBuilder_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CScriptBuilder_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._CScriptBuilder_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._CScriptBuilder_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CScriptBuilder_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 284);
            this._CScriptBuilder_Toolbars_Dock_Area_Bottom.Name = "_CScriptBuilder_Toolbars_Dock_Area_Bottom";
            this._CScriptBuilder_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(492, 0);
            this._CScriptBuilder_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.m_ultraToolbarManager;
            // 
            // m_ctrlPanel
            // 
            this.m_ctrlPanel.AllowDrop = true;
            this.m_ctrlPanel.BackColor = System.Drawing.SystemColors.Control;
            this.m_ctrlPanel.Controls.Add(this.m_ctrlHideScene);
            this.m_ctrlPanel.Controls.Add(this.m_ctrlSelectable);
            this.m_ctrlPanel.Controls.Add(this.m_ctrlAutoTransition);
            this.m_ctrlPanel.Controls.Add(this.m_ctrlStatusBar);
            this.m_ctrlPanel.Controls.Add(this.m_ctrlScrollbar);
            this.m_ctrlPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.m_ctrlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ctrlPanel.Location = new System.Drawing.Point(0, 52);
            this.m_ctrlPanel.Name = "m_ctrlPanel";
            this.m_ctrlPanel.Size = new System.Drawing.Size(492, 232);
            this.m_ctrlPanel.TabIndex = 2;
            this.m_ctrlPanel.DragOver += new System.Windows.Forms.DragEventHandler(this.OnDragOver);
            this.m_ctrlPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnPanelMouseDown);
            this.m_ctrlPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnPanelMouseMove);
            this.m_ctrlPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this.m_ctrlPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.m_ctrlPanel_Paint);
            this.m_ctrlPanel.MouseHover += new System.EventHandler(this.OnMouseHover);
            this.m_ctrlPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnPanelMouseUp);
            this.m_ctrlPanel.DragLeave += new System.EventHandler(this.OnDragLeave);
            // 
            // m_ctrlSelectable
            // 
            this.m_ctrlSelectable.Location = new System.Drawing.Point(4, 4);
            this.m_ctrlSelectable.Name = "m_ctrlSelectable";
            this.m_ctrlSelectable.Size = new System.Drawing.Size(28, 20);
            this.m_ctrlSelectable.TabIndex = 8;
            // 
            // m_ctrlStatusBar
            // 
            appearance1.TextHAlignAsString = "Center";
            this.m_ctrlStatusBar.Appearance = appearance1;
            this.m_ctrlStatusBar.BorderStyle = Infragistics.Win.UIElementBorderStyle.InsetSoft;
            this.m_ctrlStatusBar.ImageList = this.m_ctrlImages;
            this.m_ctrlStatusBar.Location = new System.Drawing.Point(0, 209);
            this.m_ctrlStatusBar.Name = "m_ctrlStatusBar";
            appearance2.Image = 18;
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance2.ImageVAlign = Infragistics.Win.VAlign.Middle;
            ultraStatusPanel1.Appearance = appearance2;
            ultraStatusPanel1.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            ultraStatusPanel1.Width = 20;
            appearance3.TextHAlignAsString = "Center";
            ultraStatusPanel2.Appearance = appearance3;
            ultraStatusPanel2.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            ultraStatusPanel2.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Spring;
            this.m_ctrlStatusBar.Panels.AddRange(new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel[] {
            ultraStatusPanel1,
            ultraStatusPanel2});
            this.m_ctrlStatusBar.Size = new System.Drawing.Size(492, 23);
            this.m_ctrlStatusBar.TabIndex = 1;
            this.m_ctrlStatusBar.WrapText = false;
            this.m_ctrlStatusBar.MouseHover += new System.EventHandler(this.OnMouseHover);
            // 
            // m_ctrlScrollbar
            // 
            this.m_ctrlScrollbar.Location = new System.Drawing.Point(444, 16);
            this.m_ctrlScrollbar.Name = "m_ctrlScrollbar";
            this.m_ctrlScrollbar.Size = new System.Drawing.Size(16, 112);
            this.m_ctrlScrollbar.TabIndex = 0;
            this.m_ctrlScrollbar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.OnScroll);
            // 
            // CScriptBuilder
            // 
            this.AllowDrop = true;
            this.m_ultraToolbarManager.SetContextMenuUltra(this, "ContextMenu");
            this.Controls.Add(this.m_ctrlPanel);
            this.Controls.Add(this._CScriptBuilder_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._CScriptBuilder_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._CScriptBuilder_Toolbars_Dock_Area_Top);
            this.Controls.Add(this._CScriptBuilder_Toolbars_Dock_Area_Bottom);
            this.Name = "CScriptBuilder";
            this.Size = new System.Drawing.Size(492, 284);
            ((System.ComponentModel.ISupportInitialize)(this.m_ultraToolbarManager)).EndInit();
            this.m_ctrlPanel.ResumeLayout(false);
            this.m_ctrlPanel.PerformLayout();
            this.ResumeLayout(false);

		}// protected override void InitializeComponent()
	
		/// <summary>This function is called when the PaneVisible property changes</summary>
		protected override void OnPaneVisibleChanged()
		{
			//	Is this pane being activated?
			if((PaneVisible == true) && (m_aScenes != null))
			{
				//	Has the script changed?
				if(ReferenceEquals(m_aScenes.Primary, m_dxPrimary) == false)
				{
					View();
				}
				
				//	Did one or more scenes get added while we were invisible?
				else if((m_iFirstVisibleScene < 0) && (m_dxActivate != null))
				{
					View();
					
					//	Make sure all media is up to date
					if(m_bCheckMedia == true)
					{
						//m_tmaxEventSource.FireDiagnostic(this, "OnActiveChanged", "one or more");
						CheckMedia();
					}
				}
				
				else
				{
					//	Make sure all media is up to date
					if(m_bCheckMedia == true)
					{
						//m_tmaxEventSource.FireDiagnostic(this, "OnActiveChanged", "check all");
						CheckMedia();
					}
				}
				
			}
			else
			{
				//	Stop all playback
				StopScenes();
			}

		}// protected override void OnPaneVisibleChanged()
		
		/// <summary>Overridden by derived classes to provide access to the pane's toolbar manager</summary>
		/// <returns>The pane's toolbar manager if available</returns>
		protected override Infragistics.Win.UltraWinToolbars.UltraToolbarsManager GetUltraToolbarManager()
		{
			return m_ultraToolbarManager;
		}

		/// <summary>This method handles the event fired when the user clicks on Copy from the context menu</summary>
		/// <param name="Selections">The scenes that are currently selected</param>
		private void OnCmdCopy(CScriptScenes aSelections)
		{
			if(FireCommand(TmaxCommands.Copy, aSelections, false) == true)
			{
			
			}

		}// OnCmdCopy(CScriptScenes aSelections)
		
		/// <summary>This method handles the event fired when the user clicks on Cut from the context menu</summary>
		/// <param name="Selections">The scenes that are currently selected</param>
		private void OnCmdCut(CScriptScenes aSelections)
		{
			//	Copy the source media to the application clipboard
			if(FireCommand(TmaxCommands.Copy, aSelections, true) == true)
			{
				//	Delete scenes from the current script
				OnCmdDelete(aSelections);
			}

		}// OnCmdCut(CScriptScenes aSelections)
		
		/// <summary>This method handles the event fired when the user clicks on Search from the context menu</summary>
		/// <param name="Selections">The scenes that are currently selected</param>
		private void OnCmdFind(CScriptScenes aSelections)
		{
			CTmaxItems tmaxItems = null;
			
			if((tmaxItems = GetCmdFindItems()) != null)
				FireCommand(TmaxCommands.Find, tmaxItems);
			else
				FireCommand(TmaxCommands.Find);
			
		}// private void OnCmdFind(CScriptScenes aSelections)
				
		/// <summary>This method is called by the application to get a list of items that represent the current selections for a Find command</summary>
		/// <returns>The items that represent the current selections</returns>
		public override CTmaxItems GetCmdFindItems()
		{
			CScriptScenes	aSelections = null;
			CTmaxItems		tmaxItems = null;
			
			//	Get the current selections
			if(m_aScenes != null)
				aSelections = m_aScenes.GetSelections();
				
			if((aSelections == null) || (aSelections.Count == 0))
			{
				//	Do we have a valid script?
				if((m_aScenes != null) && (m_aScenes.Primary != null))
				{
					tmaxItems = new CTmaxItems();
					tmaxItems.Add(new CTmaxItem(m_aScenes.Primary));
				}
			}
			else
			{
				tmaxItems = GetCommandItems(aSelections, false);
			}
			
			if((tmaxItems != null) && (tmaxItems.Count > 0))
				return tmaxItems;
			else
				return null;
				
		}// public virtual CTmaxItems GetCmdFindItems()
		
		/// <summary>This method handles the event fired when the user clicks on Delete from the context menu</summary>
		/// <param name="Selections">The scenes that are currently selected</param>
		private void OnCmdDelete(CScriptScenes aSelections)
		{	
			CTmaxItem	tmaxScript = null;
			CTmaxItem	tmaxScene = null;
				
			//	Make sure we have the required objects
			if(m_aScenes.Primary == null) return;
			if(aSelections == null) return;
			if(aSelections.Count == 0) return;
		
			//	Create an event item for the script
			tmaxScript = new CTmaxItem(m_aScenes.Primary);
			
			//	Add a subitem for each selection
			foreach(CScriptScene O in aSelections)
			{
				if(O.Secondary != null)
				{
					if((tmaxScene = new CTmaxItem(O.Secondary)) != null)
						tmaxScript.SubItems.Add(tmaxScene);
				}
			}
			
			if(FireCommand(TmaxCommands.Delete, tmaxScript) == true)
			{
			
			}

		}// OnCmdDelete(CScriptScenes aSelections)
		
		/// <summary>This method handles the event fired when the user clicks on Print from the context menu</summary>
		/// <param name="Selections">The scenes that are currently selected</param>
		private void OnCmdPrint(CScriptScenes aSelections)
		{
			CTmaxItems tmaxItems = null;
			
			if((tmaxItems = GetCmdPrintItems()) != null)
				FireCommand(TmaxCommands.Print, tmaxItems);
			else
				FireCommand(TmaxCommands.Print);

		}// private void OnCmdPrint(CScriptScenes aSelections)
		
		/// <summary>This method is called by the application to get a list of items that represent the current selections for a Print command</summary>
		/// <returns>The items that represent the current selections</returns>
		public override CTmaxItems GetCmdPrintItems()
		{
			CScriptScenes	aSelections = null;
			CTmaxItems		tmaxItems = null;
			
			//	Get the current selections
			if(m_aScenes != null)
				aSelections = m_aScenes.GetSelections();
				
			if((aSelections == null) || (aSelections.Count == 0))
			{
				//	Do we have a valid script?
				if((m_aScenes != null) && (m_aScenes.Primary != null))
				{
					tmaxItems = new CTmaxItems();
					tmaxItems.Add(new CTmaxItem(m_aScenes.Primary));
				}
			
			}
			else
			{
				tmaxItems = GetCommandItems(aSelections, false);
			}
			
			if((tmaxItems != null) && (tmaxItems.Count > 0))
				return tmaxItems;
			else
				return null;
				
		}// public override CTmaxItems GetCmdPrintItems()
		
		/// <summary>This method is called by the application to get a list of items that represent the current selections for a Scroll Text command</summary>
		/// <param name="bBreakOnFirst">true to stop when we find one record that meets the criteria</param>
		/// <returns>The items that represent the current selections</returns>
		public CTmaxItems GetCmdDesignationItems(bool bBreakOnFirst)
		{
			CScriptScenes	aScenes = null;
			CTmaxItems		tmaxItems = null;
			
			//	Do we have a valid script?
			if((m_aScenes != null) && (m_aScenes.Primary != null))
			{
				//	Get the current selections
				aScenes = m_aScenes.GetSelections();
				
				//	Use the entire script if no selections
				if((aScenes == null) || (aScenes.Count == 0))
					aScenes = m_aScenes;
					
				//	Do we have any scenes?
				if((aScenes != null) && (aScenes.Count > 0))
				{
					tmaxItems = new CTmaxItems();

					//	Search for all scenes that are bound to video designations
					foreach(CScriptScene O in aScenes)
					{
						if((O.Secondary != null) && (O.Secondary.GetIsVideoDesignation() == true))
						{
							if(O.Secondary.GetSource() != null)
							{
								tmaxItems.Add(new CTmaxItem(O.Secondary.GetSource()));
							
								if(bBreakOnFirst == true)
									break;
								
							}// if(O.Secondary.GetSource() != null)
					
						}// if((O.Secondary != null) && (O.Secondary.GetIsVideoDesignation() == true))
				
					}// foreach(CScriptScene O in aScenes)
				
				}// if((aScenes != null) && (aScenes.Count > 0))

			}// if((m_aScenes != null) && (m_aScenes.Primary != null))

			if((tmaxItems != null) && (tmaxItems.Count > 0))
				return tmaxItems;
			else
				return null;
				
		}// public override CTmaxItems GetCmdDesignationItems(bool bBreakOnFirst)
		
		/// <summary>This method handles the event fired when the user clicks on New Script from the context menu</summary>
		private void OnCmdNew()
		{		
			CTmaxParameters		tmaxParameters = null;
			CTmaxItem			tmaxItem = null;
			
			if(m_tmaxDatabase == null) return;
			
			//	Create an event item to request a new script
			tmaxItem = new CTmaxItem();
			tmaxItem.DataType = TmaxDataTypes.Media;
			tmaxItem.MediaType = TmaxMediaTypes.Script;
			
			//	Create the parameters required to activate and open the new item
			tmaxParameters = new CTmaxParameters();
			tmaxParameters.Add(TmaxCommandParameters.Activate, true);
			tmaxParameters.Add(TmaxCommandParameters.Builder, true);
				
			//	Add the new script
			FireCommand(TmaxCommands.Add, tmaxItem, tmaxParameters);
			
		}// OnCmdNew()
		
		/// <summary>This method handles the event fired when the user clicks on one of the Paste commands in the context menu</summary>
		/// <param name="Selection">The current scene selection</param>
		/// <param name="bBefore">true to insert before the selected scene</param>
		private void OnCmdPaste(CScriptScene Selection, bool bBefore)
		{
			CTmaxParameters	tmaxParameters = null;
			CTmaxItem		tmaxScript = null;
			
			//	Make sure we have the required objects
			if(m_aScenes.Primary == null) return;
			if(m_tmaxClipboard == null) return;
			if(m_tmaxClipboard.ContainsMedia(true) == false) return;
		
			//	Create an event item for the script
			tmaxScript = new CTmaxItem(m_aScenes.Primary);
			
			//	Assign the source items
			tmaxScript.SourceItems = new CTmaxItems();
			Pack(m_tmaxClipboard, true, tmaxScript.SourceItems);
			
			//	Are we inserting?
			if((Selection != null) && (Selection.Secondary != null))
			{
				Debug.Assert(ReferenceEquals(((CDxSecondary)(Selection.Secondary)).Primary, m_aScenes.Primary) == true);
				
				//	Create the required parameters for the event
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.Before, bBefore);
				
				//	Put the insertion point in the parent's subitem collection
				tmaxScript.SubItems.Add(new CTmaxItem(Selection.Secondary));
			}

			//	Fire the event
			FireCommand(TmaxCommands.Add, tmaxScript, tmaxParameters);
			
		}// OnCmdPaste(CScriptScene Selection, bool bBefore)
		
		/// <summary>This method handles the event fired when the user clicks on Properties from the context menu</summary>
		/// <param name="Selection">The current scene selection</param>
		private void OnCmdProperties(CScriptScene Selection)
		{
			CTmaxParameters tmaxParameters = null;
			
			//	Create the required parameters for the event
			tmaxParameters = new CTmaxParameters();
			tmaxParameters.Add(TmaxCommandParameters.Properties, true);
				
			if(FireCommand(TmaxCommands.Open, Selection, false, tmaxParameters) == true)
			{
			
			}
			
		}// OnCmdProperties(CScriptScene Selection)
		
		/// <summary>This method handles the event fired when the user clicks on Presentation from the context menu</summary>
		/// <param name="Selection">The current scene selection</param>
		private void OnCmdPresentation(CScriptScene Selection)
		{
			CTmaxParameters tmaxParameters = null;
			
			//	Create the required parameters for the event
			tmaxParameters = new CTmaxParameters();
			tmaxParameters.Add(TmaxCommandParameters.Presentation, true);
				
			if(FireCommand(TmaxCommands.Open, Selection, false, tmaxParameters) == true)
			{
			
			}
			
		}// OnCmdPresentation(CScriptScene Selection)
		
		/// <summary>This method handles the event fired when the user clicks on PinScript from the context menu</summary>
		private void OnCmdPinScript()
		{
			if(m_ctrlPinScript != null)
				m_bScriptPinned = m_ctrlPinScript.Checked;
				
			SetControlStates();
			
		}// OnCmdPinScript()
		
		/// <summary>This method handles the event fired when the user clicks on Viewer from the context menu</summary>
		/// <param name="Selection">The current scene selection</param>
		private void OnCmdViewer(CScriptScene Selection)
		{
			CTmaxParameters tmaxParameters = null;
			
			//	Create the required parameters for the event
			tmaxParameters = new CTmaxParameters();
			tmaxParameters.Add(TmaxCommandParameters.Viewer, true);
				
			if(FireCommand(TmaxCommands.Open, Selection, false, tmaxParameters) == true)
			{
			
			}
			
		}// OnCmdViewer(CScriptScene Selection)
		
		/// <summary>This method handles the event fired when the user clicks on Show / Hide Scrolling Text from the context menu</summary>
		/// <param name="bScroll">true to show scrolling text on all selected designations</param>
		protected virtual void OnCmdScrollText(bool bScroll)
		{
			CTmaxItems		tmaxItems = GetCmdDesignationItems(false);
			CDxTertiary		dxDesignation = null;
			CTmaxParameters	tmaxParameters = null;
			
			try
			{
				//	Do we have any items to be processed?
				if(tmaxItems.Count > 0)
				{
					//	Are we supposed to synchronize the XML files?
					if(m_tmaxCaseOptions.SyncXmlDesignations == true)
					{
						tmaxParameters = new CTmaxParameters();
						tmaxParameters.Add(TmaxCommandParameters.SyncXml, true);
					}
					
					//	Update each of the selected scenes
					foreach(CTmaxItem O in tmaxItems)
					{
						if((dxDesignation = (CDxTertiary)(O.GetMediaRecord())) != null)
						{
							if(dxDesignation.ScrollText != bScroll)
							{
								//	Update the designation
								dxDesignation.ScrollText = bScroll;
								FireCommand(TmaxCommands.Update, O, tmaxParameters);	
							}
						
						}// if((dxDesignation = (CDxTertiary)(O.GetMediaRecord())) != null)
						
					}// foreach(CTmaxItem O in tmaxItems)
					
				}
				else
				{
					MessageBox.Show("No video designations to be processed in the current selections", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
				
				}// if(tmaxItems.Count > 0)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdScrollText", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_SCROLL_TEXT_EX, bScroll ? "show" : "hide"), Ex);
			}
			
			
		}// protected virtual void OnCmdScrollText(CTmaxMediaTreeNodes tmaxNodes, bool bHide)
		
		/// <summary>This method handles the event fired when the user clicks on Set Highlighter from the context menu</summary>
		/// <param name="iCommand">The command identifier</param>
		protected virtual void OnCmdSetHighlighter(int iCommand)
		{
			CTmaxItems		tmaxItems = null;
			CDxTertiary		dxDesignation = null;
			CDxHighlighter	dxHighlighter = null;
			CTmaxParameters	tmaxParameters = null;
			int				iIndex = 0;
			
			//	Get the requested highlighter
			if((m_tmaxDatabase != null) && (m_tmaxDatabase.Highlighters != null))
			{
				iIndex = (iCommand - (int)ScriptBuilderCommands.SetHighlighter1);
			
				if((iIndex >= 0) && (iIndex < m_tmaxDatabase.Highlighters.Count))
					dxHighlighter = m_tmaxDatabase.Highlighters[iIndex];
			
			}// if((m_tmaxDatabase != null) && (m_tmaxDatabase.Highlighters != null))
			
			if(dxHighlighter != null)
			{
				//	Get the designations to be modified
				tmaxItems = GetCmdDesignationItems(false);
				if((tmaxItems != null) && (tmaxItems.Count > 0))
				{
					//	Are we supposed to synchronize the XML files?
					if(m_tmaxCaseOptions.SyncXmlDesignations == true)
					{
						tmaxParameters = new CTmaxParameters();
						tmaxParameters.Add(TmaxCommandParameters.SyncXml, true);
					}
					
					//	Update each of the selected scenes
					foreach(CTmaxItem O in tmaxItems)
					{
						if((dxDesignation = (CDxTertiary)(O.GetMediaRecord())) != null)
						{
							if(dxDesignation.GetExtent() != null)
							{
								//	Has the highlighter changed?
								if(dxDesignation.GetExtent().HighlighterId != dxHighlighter.AutoId)
								{
									//	Update the designation
									dxDesignation.GetExtent().Highlighter = dxHighlighter;
									dxDesignation.GetExtent().HighlighterId = dxHighlighter.AutoId;
									FireCommand(TmaxCommands.Update, new CTmaxItem(dxDesignation), tmaxParameters);	
								}
							
							}// if(dxDesignation.GetExtent() != null)
						
						}// if((dxDesignation = (CDxTertiary)(O.GetMediaRecord())) != null)
						
					}// foreach(CTmaxItem O in tmaxItems)
					
				}
				else
				{
					Warn("No designations were found among the current selections");
				}
				
			}
			else
			{
				Warn("Unable to retrieve the requested highlighter");
			}
			
		}// protected virtual void OnCmdSetHighlighter(int iCommand)
		
		/// <summary>This method handles the event fired when the user clicks on Tuner from the context menu</summary>
		/// <param name="Selection">The current scene selection</param>
		private void OnCmdTuner(CScriptScene Selection)
		{
			CTmaxParameters tmaxParameters = null;
			
			//	Create the required parameters for the event
			tmaxParameters = new CTmaxParameters();
			tmaxParameters.Add(TmaxCommandParameters.Tuner, true);
				
			if(FireCommand(TmaxCommands.Open, Selection, false, tmaxParameters) == true)
			{
			
			}
			
		}// OnCmdTuner(CScriptScene Selection)
		
		/// <summary>This method handles the event fired when the user clicks on Preferences from the context menu</summary>
		/// <param name="Scenes">The current scene selections</param>
		private void OnCmdPreferences(CScriptScenes Scenes)
		{
			CFScriptPreferences Preferences = null;
			
			try
			{
				Preferences = new CFScriptPreferences();
			
				//	Initialize the preferences dialog
				Preferences.Columns = Columns;
				Preferences.StatusTextMode = StatusTextMode;
				Preferences.SceneTextMode = SceneTextMode;
			
				//	Open the dialog
				if(Preferences.ShowDialog() == DialogResult.OK)
				{
					if(Preferences.StatusTextMode != StatusTextMode)
					{
						m_eStatusTextMode = Preferences.StatusTextMode;
						SetStatusText();
					}
					
					if(Preferences.SceneTextMode != SceneTextMode) 
					{
						m_eSceneTextMode = Preferences.SceneTextMode;
						SetSceneText();
					}
					
					if(Preferences.Columns != m_iColumns)
					{
						m_iColumns = Preferences.Columns;
						RecalcLayout();
					}
					
				}// if(Preferences.ShowDialog() == DialogResult.OK)
				
			}
			catch
			{
			}
			finally
			{
				if(Preferences != null)
					Preferences.Dispose();
			}
			
		}// OnCmdPreferences(CScriptScene Scene)
		
		/// <summary>This method traps all DragDrop events</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">System event arguements</param>
		protected override void OnDragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			//	Clear the insertion indicator
			//
			//	NOTE: We do this first in case the pane gets redrawn as a result
			//		  of the operation
			DrawDropIndicator(true);
			
			//	Notify the application that this is the drop pane
			FireCommand(TmaxCommands.AcceptDrop);
			
			//	If the user was dragging records we need to process the items
			if((m_eDragState == PaneDragStates.Records) && (e.Effect != DragDropEffects.None))
			{
				//	Notify the derived class
				OnDropRecords(e.Effect);
			}
			
			//	Clear the drop target members
			SetDropTarget(-1, -1);
			
			//	Perform the base class processing
			base.OnDragDrop(sender, e);
		
		}// protected override void OnDragDrop(object sender, System.Windows.Forms.DragEventArgs e)

		/// <summary>This method traps all DragLeave events</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">System event arguements</param>
		protected override void OnDragLeave(object sender, System.EventArgs e)
		{
			//	Clear the drop target
			SetDropTarget(-1,-1);
		}

		/// <summary>This method traps all DragOver events</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">System event arguements</param>
		protected override void OnDragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			System.Drawing.Point Pos;
			
			//	Initialize the effects
			e.Effect = DragDropEffects.None;
			
			//	Only allow dropping of registered media
			if(m_eDragState != PaneDragStates.Records) return;
			
			//	Must have a valid script
			if(m_aScenes.Primary == null) return;
		
			//	Must have something to drop
			if(m_tmaxDragData == null) return;
			if(m_tmaxDragData.SourceItems == null) return;
			if(m_tmaxDragData.SourceItems.ContainsMedia(true) == false) return;
				
			//	Make sure we have the background panel
			Debug.Assert(m_ctrlPanel != null);
			Debug.Assert(m_ctrlPanel.IsDisposed == false);
			if((m_ctrlPanel == null) || (m_ctrlPanel.IsDisposed == true)) return;
			
			//	Convert the current mouse position to client coordinates
			Pos = m_ctrlPanel.PointToClient(Control.MousePosition);
				
			//	Set the new drop target
			SetDropTarget(Pos.X, Pos.Y);
			
			//	Is the user even in the drop zone
			if((m_iDropCell < 0) || (m_iDropCell >= m_aCells.Count)) return;

//			//	Is there a valid drop scene?
//			if((m_iDropScene < 0) || (m_iDropScene >= m_aScenes.Count))
//			{
//				//	Unless this is an empty script we want to make them choose
//				//	a scene
//				if((m_aScenes != null) && (m_aScenes.Count > 0)) return;
//			}
			
			//	If the drag script is the same as the current script then the
			//	user must be reordering scenes
			if(ReferenceEquals(m_aScenes.Primary, m_tmaxDragData.GetMediaRecord()) == true)
				e.Effect = DragDropEffects.Move;
			else
				e.Effect = DragDropEffects.Link;
				
		}// protected override void OnDragOver(object sender, System.Windows.Forms.DragEventArgs e)

		/// <summary>This function is called to resize and reposition the panes child controls</summary>
		protected override void RecalcLayout()
		{
			//	Make sure we have the required objects
			if(m_ctrlPanel == null) return;
			if(m_ctrlPanel.IsDisposed == true) return;
			if(m_ctrlScene == null) return;
			if(m_ctrlStatusBar == null) return;
			if(m_aCells == null) return;
			
			try
			{
				//	Position the scroll bar to dock to the right side of the background panel
				m_ctrlScrollbar.SetBounds(m_ctrlPanel.Width  - m_ctrlScrollbar.Width,
										0, m_ctrlScrollbar.Width,
										m_ctrlPanel.Height - m_ctrlStatusBar.Height);

				//	Set the new scene cell sizes and positions
				SetCells();
				
				//	Move the scenes
				if(m_aScenes != null)
					SetFirstScene(m_iFirstVisibleScene);
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "RecalcLayout", m_tmaxErrorBuilder.Message(ERROR_RECALC_LAYOUT_EX), Ex);
			}

		}// RecalcLayout()

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			base.SetErrorStrings();
			
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate %1 to export thumbnail for slide id = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to export the slide in %1 to %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to view the scenes");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to create the event item for this scene: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to process the item update");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to process the new additions");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to handle the reorder notification");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to handle the delete notification");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to add new scenes to the database");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to add the selection for %1 to the script selection list");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to remove the selection for %1 from the script selection list");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to set the current script selection in the list box");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to create the scene rendering control for %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to free the scene rendering control for %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to load this scene: %1");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to recalculate the builder layout");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to set the scene rendering cells");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to set the first scene: index = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to set the scroll range: max = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to set the scroll position: position = %1");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to set the scene text for the rendering controls");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to %1 scrolling text for the selected designations.");

		}// protected override void SetErrorStrings()

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>This function is called to set the state of the pane's child controls</summary>
		private void SetControlStates()
		{
			if(m_ctrlPinScript != null)
			{
				if(m_bScriptPinned == true)
				{
					m_ctrlPinScript.SharedProps.AppearancesSmall.Appearance.Image = 18;
					m_ctrlPinScript.SharedProps.Caption = "Unpin Script";
					m_ctrlStatusBar.Panels[0].Appearance.Image = 18;
				}
				else
				{
					m_ctrlPinScript.SharedProps.AppearancesSmall.Appearance.Image = 19;
					m_ctrlPinScript.SharedProps.Caption = "Pin Script";
					m_ctrlStatusBar.Panels[0].Appearance.Image = -1;
				}
				
				m_ctrlPinScript.SharedProps.Enabled = (m_tmaxDatabase != null);

			}
			
			SetScriptSelection();
				
			SetSceneControls();
			
		}// SetControlStates()
		
		/// <summary>This function is called to set the selection in the scripts list</summary>
		private void SetScriptSelection()
		{
			Infragistics.Win.UltraWinToolbars.ComboBoxTool ctrlScripts = null;
			Infragistics.Win.ValueListItemsCollection vlItems = null;
			Infragistics.Win.ValueListItem vlItem = null;
			
			//	Prevent responding to events while we make the change
			//
			//	NOTE:	This may be getting called as a result of the user
			//			making a new selection in the scripts combobox
			if(m_bIgnoreUltraEvents == false)
				m_bIgnoreUltraEvents = true;
			else
				return;	//	Must be selecting locally
			
			try
			{
				//	Make sure the script selection list is showing the correct value
				if((ctrlScripts = (ComboBoxTool)GetUltraTool("Scripts")) != null)
				{
					if((ctrlScripts.ValueList != null) && (ctrlScripts.ValueList.ValueListItems != null))
					{
						vlItems = ctrlScripts.ValueList.ValueListItems;
						
						if(m_aScenes.Primary != null)
						{
							foreach(ValueListItem O in vlItems)
							{
								if(ReferenceEquals(O.DataValue, m_aScenes.Primary) == true)
								{
									vlItem = O;
									break;
								}
								
							}
							
							ctrlScripts.SelectedItem = vlItem;
						}
						else
						{
							ctrlScripts.SelectedItem = null;
						}
					
					}// if((ctrlScripts.ValueList != null) && (ctrlScripts.ValueList.ValueListItems != null))
				
					m_bIgnoreUltraEvents = false;
					
				}// if((ctrlScripts = (ComboBoxTool)GetUltraTool("Scripts")) != null)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetScriptSelection", m_tmaxErrorBuilder.Message(ERROR_SET_SELECTION_EX), Ex);
			}
			finally
			{
				m_bIgnoreUltraEvents = false;
			}		
			
		}// private void SetScriptSelection()
		
		/// <summary>This function is called to set the text for the specified script in the combobox</summary>
		private void SetScriptText(CDxPrimary dxScript)
		{
			Infragistics.Win.UltraWinToolbars.ComboBoxTool ctrlScripts = null;
			Infragistics.Win.ValueListItemsCollection vlItems = null;
			
			//	Make sure the script selection list is showing the correct value
			if((ctrlScripts = (ComboBoxTool)GetUltraTool("Scripts")) != null)
			{
				if((ctrlScripts.ValueList != null) && (ctrlScripts.ValueList.ValueListItems != null))
				{
					vlItems = ctrlScripts.ValueList.ValueListItems;
					
					foreach(ValueListItem O in vlItems)
					{
						if(ReferenceEquals(O.DataValue, dxScript) == true)
						{							
							O.DisplayText = GetDisplayText(dxScript);
							
							if(ReferenceEquals(O, ctrlScripts.SelectedItem) == true)
								ctrlScripts.Text = O.DisplayText;
								
							break;
						}
						
					}
				
				}// if((ctrlScripts.ValueList != null) && (ctrlScripts.ValueList.ValueListItems != null))
				
			}// if((ctrlScripts = (ComboBoxTool)GetUltraTool("Scripts")) != null)
		
			
		}// private void SetScriptText(CDxPrimary dxScript)
		
		/// <summary>This function is called to get the text assigned to the script in the combobox</summary>
		/// <param name="dxScript">The script to retrieve the display text for</param>
		/// <returns>The text to display in the scripts combobox</returns>
		private string GetDisplayText(CDxPrimary dxScript)
		{
			string strText = "";
			
			if(dxScript.Name.Length > 0)
				strText = (dxScript.Name + " - " + dxScript.MediaId);
			else
				strText = dxScript.MediaId;
				
			return strText;
			
		}// private string GetDisplayText(CDxPrimary dxScript)
		
		/// <summary>This method is called to set the text displayed in the status bar</summary>
		private void SetStatusText()
		{
			CScriptScenes	aSelections = null;
			string			strText = "";

			if(m_ctrlStatusBar == null) return;
			if(m_ctrlStatusBar.IsDisposed == true) return;
			
			//	Do we have an active script?
			if((m_aScenes != null) && (m_aScenes.Primary != null))
			{
				//	Get the current selections
				aSelections = m_aScenes.GetSelections();
				
				//	Do we have any selections?
				if((aSelections != null) && (aSelections.Count > 0))
				{
					strText = aSelections[0].StatusText;
					if(aSelections.Count > 1)
					{
						strText += " .. ";
						strText += aSelections[aSelections.Count - 1].StatusText;
					}
					
				}
				else
				{				
					strText = m_aScenes.Primary.GetText(m_eStatusTextMode);
				}
				
			}

			m_ctrlStatusBar.Panels[1].Text = strText;
		
		}// SetStatusText()
		
		/// <summary>This method is called to set the record interfaces using the specified event item</summary>
		/// <param name="tmaxItem">The TrialMax event item</param>
		/// <returns>true if the values have changed</returns>
		private bool SetRecords(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{
			CDxPrimary		dxPrimary = null;
			CDxSecondary	dxSecondary = null;

			//	Is this a script?
			if((tmaxItem.IPrimary != null) && (tmaxItem.IPrimary.GetMediaType() == TmaxMediaTypes.Script))
			{
				dxPrimary   = (CDxPrimary)tmaxItem.IPrimary;
				dxSecondary = (CDxSecondary)tmaxItem.ISecondary;
				
				//	Get the first scene in this script if available
				if(dxSecondary == null)
				{
					if((dxPrimary.Secondaries == null) || (dxPrimary.Secondaries.Count == 0))
						dxPrimary.Fill();
						
					if((dxPrimary.Secondaries != null) && (dxPrimary.Secondaries.Count > 0))
						dxSecondary = dxPrimary.Secondaries[0];
				}
				
			}
				
			//	Save the record interfaces if they've changed
			if((ReferenceEquals(dxPrimary, m_aScenes.Primary) == false) ||
			   (ReferenceEquals(dxSecondary, m_dxActivate) == false))
			{
				if((m_dxPrimary = dxPrimary) != null)
					m_dxActivate = dxSecondary;
				return true;
			}
			else
			{
				return false;
			}
			
		}// private bool SetRecords(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		/// <summary>This method is called to remove a script from the selection list</summary>
		/// <param name="dxScript">The exchange object associated with the script to be removed</param>
		private void RemoveSelection(CDxPrimary dxScript)
		{
			Infragistics.Win.UltraWinToolbars.ComboBoxTool ctrlScripts = null;
			Infragistics.Win.ValueListItemsCollection vlItems = null;
			Infragistics.Win.ValueListItem vlItem = null;
			
			try
			{
				if((ctrlScripts = (ComboBoxTool)GetUltraTool("Scripts")) != null)
				{
					if((ctrlScripts.ValueList != null) && (ctrlScripts.ValueList.ValueListItems != null))
					{
						vlItems = ctrlScripts.ValueList.ValueListItems;
						
						foreach(ValueListItem O in vlItems)
						{
							if(ReferenceEquals(O.DataValue, dxScript) == true)
							{
								vlItem = O;
								break;
							}
							
						}
					
						//	Did we locate the item being removed
						if(vlItem != null)
						{
							//	Are we removing the selected item?
							if(ReferenceEquals(ctrlScripts.SelectedItem, vlItem) == true)
							{
								m_bIgnoreUltraEvents = true;
								ctrlScripts.SelectedItem = null;
								m_bIgnoreUltraEvents = false;
							}
							
							//	Remove from the list
							vlItems.Remove(vlItem);
							
						}
							
					}// if((ctrlScripts.ValueList != null) && (ctrlScripts.ValueList.ValueListItems != null))
					
				}// if((ctrlScripts = (ComboBoxTool)GetUltraTool("Scripts")) != null)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "RemoveSelection", m_tmaxErrorBuilder.Message(ERROR_REMOVE_SELECTION_EX, dxScript.MediaId), Ex);
			}
		
		}// private void RemoveSelection(CDxPrimary dxScript)
		
		/// <summary>This method is called to add a script to the selection list</summary>
		/// <param name="dxScript">The exchange object associated with the script to be removed</param>
		private void AddSelection(CDxPrimary dxScript)
		{
			Infragistics.Win.UltraWinToolbars.ComboBoxTool ctrlScripts = null;
			
			try
			{
				if((ctrlScripts = (ComboBoxTool)GetUltraTool("Scripts")) != null)
				{
					if((ctrlScripts.ValueList != null) && (ctrlScripts.ValueList.ValueListItems != null))
					{
						ctrlScripts.ValueList.ValueListItems.Add(dxScript, GetDisplayText(dxScript));
						
					}// if((ctrlScripts.ValueList != null) && (ctrlScripts.ValueList.ValueListItems != null))
					
				}// if((ctrlScripts = (ComboBoxTool)GetUltraTool("Scripts")) != null)
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddSelection", m_tmaxErrorBuilder.Message(ERROR_ADD_SELECTION_EX, dxScript.MediaId), Ex);
			}
			
		}// private void AddSelection(CDxPrimary dxScript)
		
		/// <summary>This method is called internally to view the scenes in the script</summary>
		/// <returns>true if successful</returns>
		private bool View()
		{
			//	Has the script changed?
			if(ReferenceEquals(m_dxPrimary, m_aScenes.Primary) == false)
			{
				//	Make sure the existing scenes have been updated
				if((m_aScenes != null) && (m_aScenes.Count > 0))
					SetSceneProps();
				
				if(m_dxPrimary != null)
				{
					//	Create the new scenes collection
					CreateScenes(m_dxPrimary);
				
					//	Initialize the first scene
					SetFirstScene(0);
			
					//	Update the status bar
					SetStatusText();
					
					//	Make sure we're showing the correct script
					SetScriptSelection();
					
				}
				else
				{
					Unload();
				}
				
				//	Update the control states
				SetControlStates();
			}
			
			try
			{
				//	Make sure the requested scene is visible
				if((m_dxActivate != null) && (m_aScenes != null) && (m_aScenes.Count > 0))
				{
					EnsureVisible(m_aScenes.GetIndex(m_dxActivate), true);
				
					//	The scene has been activated
					m_dxActivate = null;
				
					//	Update the status bar and toolbar
					SetControlStates();
					
				}
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "View", m_tmaxErrorBuilder.Message(ERROR_VIEW_EX), Ex);
			}

			return true;
			
		}// private bool View()
		
		/// <summary>This method is called internally to unload the builder</summary>
		private void Unload()
		{
			//	Flush the existing scenes
			FreeScenes();
			
			m_dxPrimary = null;
			m_dxActivate = null;
						
			//	Update the status bar
			SetStatusText();
			
			//	Update the controls
			SetControlStates();
		
		}// protected void Unload()			
			
		/// <summary>This method will create the required number of scene rendering objects</summary>
		private void CreateScenes(CDxPrimary dxScript)
		{
			CScriptScene Scene = null;

			Debug.Assert(m_aScenes != null);
			if(m_aScenes == null) return;
			
			//	Display the wait cursor
			Cursor.Current = Cursors.WaitCursor;
			
			//	Clear the existing scenes
			FreeScenes();
		
			System.DateTime dtNow = System.DateTime.Now;			

			//	Did the caller provide a valid script?
			if((dxScript == null) || (dxScript.Secondaries == null)) 
			{
				Cursor.Current = Cursors.Default;
				return;
			}

			try
			{
				//	Assign the primary exchange object
				m_aScenes.Primary = dxScript;
				m_dxPrimary = dxScript;	//	FreeScenes() resets this member
			
				//	Do we need to fill the secondaries collection?
				if(dxScript.Secondaries.Count == 0)
					dxScript.Fill();
					
				//	Turn off sorting while we populate the new collection
				m_aScenes.KeepSorted = false;
				
				m_ctrlPanel.SuspendLayout();
				//	Add a scene for each secondary record
				foreach(CDxSecondary dxScene in dxScript.Secondaries)
				{
					if((Scene = CreateScene(dxScene)) != null)
					{
						//	Add to the local collection
						m_aScenes.Add(Scene);
						
						//SetFileSpec(Scene);
						
					}// if((Scene = CreateScene(dxScene)) != null)
						
				}// foreach(CDxSecondary dxScene in m_dxPrimary.Secondaries) 
				
				//	Restore sorting
				m_aScenes.Sort(true);
				m_aScenes.KeepSorted = true;
				m_ctrlPanel.ResumeLayout();
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "CreateScenes", Ex);
			}

			TimeSpan tsElapsed = System.DateTime.Now.Subtract(dtNow);
			//FireDiagnostic("CreateScenes", "Create " + m_aScenes.Count.ToString() + " scenes: " + tsElapsed.ToString());
				
			//	Set the scroll bar range since the number of scenes has changed
			SetScrollRange();
			
			//AsyncLoadScenes();
			
			Cursor.Current = Cursors.Default;
			
		}// private void CreateScenes(CDxPrimary dxScript)
		
		/// <summary>This method will create and initialize a new scene rendering object</summary> 
		/// <param name="dxScene">The secondary data exchange object to be rendered by the scene control</param>
		/// <returns>The new scene rendering object</returns>
		private CScriptScene CreateScene(CDxSecondary dxScene)
		{
			CScriptScene Scene = null;
			
			if(m_ctrlPanel == null) return null;
			if(m_ctrlPanel.IsDisposed == true) return null;

			try
			{
				Scene = new CScriptScene(dxScene);
				
				//	Set the control properties
				//Scene.Visible = false;
				Scene.SelectBorderSize = DEFAULT_SELECT_BORDER_SIZE;
				Scene.BackColor = System.Drawing.SystemColors.Control;
				Scene.Bounds = new System.Drawing.Rectangle(-m_iSceneWidth, -m_iSceneHeight, m_iSceneWidth, m_iSceneHeight);
				Scene.Name = m_tmaxDatabase.GetBarcode(dxScene, false);
				Scene.StatusText = GetSceneText(dxScene);
				Scene.Selected = false;

				//	Attach to the scene object's event source
				Scene.EventSource.Name = Scene.Name;
				m_tmaxEventSource.Attach(Scene.EventSource);
				Scene.EventSource.MouseDownEvent += new System.Windows.Forms.MouseEventHandler(this.OnSceneMouseDown);
				Scene.EventSource.MouseUpEvent += new System.Windows.Forms.MouseEventHandler(this.OnSceneMouseUp);
				Scene.EventSource.MouseMoveEvent += new System.Windows.Forms.MouseEventHandler(this.OnSceneMouseMove);
				Scene.EventSource.MouseDblClickEvent += new System.Windows.Forms.MouseEventHandler(this.OnSceneMouseDblClick);
				
				//	Add to this control's child collection
				m_ctrlPanel.Controls.Add(Scene);
				
				return Scene;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateScene", m_tmaxErrorBuilder.Message(ERROR_CREATE_SCENE_EX, m_tmaxDatabase.GetBarcode(dxScene, false)), Ex);
				return null;
			}

		}// protected CScriptScene CreateScene(CDxSecondary dxScene)
	
		/// <summary>This method will release the specified scene</summary>
		/// <param name="Scene">The scene object to be removed</param>
		private void FreeScene(CScriptScene Scene)
		{
			string strText = "";
			
			try
			{
				if((Scene != null) && (Scene.IsDisposed == false))
				{
					//	Just in case we need it for an exception
					strText = Scene.StatusText;
					
					//	Make sure we're not playing video
					if(Scene.Viewer != null)
						Scene.Viewer.Stop();
						
					Scene.Visible = false;
					
					//	Remove from the parent's control collection 
					if((m_ctrlPanel.Controls != null) && (m_ctrlPanel.Controls.Contains(Scene) == true))
						m_ctrlPanel.Controls.Remove(Scene);
				
					//	Dispose of the scene object
					Scene.Terminate();
					
					//	Should we remove from the local collection?
					m_aScenes.Remove(Scene);

					//Scene.Dispose();
				}
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Free", m_tmaxErrorBuilder.Message(ERROR_FREE_SCENE_EX, strText), Ex);
			}

		}// private void FreeScene(CScriptScene Scene, bool bAll)
					
		/// <summary>This method will clear the local scenes collection</summary>
		private void FreeScenes()
		{
			if((m_ctrlPanel != null) && (m_ctrlPanel.IsDisposed == false))
				m_ctrlPanel.Visible = false;
			
			m_iFirstVisibleScene = -1;
			m_iLastVisibleScene  = -1;
			m_iLastEnsuredScene  = -1;
			m_bCheckMedia = false;
						
			if(m_aScenes != null)
			{
				while(m_aScenes.Count > 0)
					FreeScene(m_aScenes[0]);
					
				m_aScenes.Primary = null;
				m_dxPrimary = null;
			}
			
			if((m_ctrlPanel != null) && (m_ctrlPanel.IsDisposed == false))
				m_ctrlPanel.Visible = true;

		}// private void FreeScenes()
					
		/// <summary>Delegate used to perform asynchronous scene loading</summary>
		protected delegate bool LoadScenesDelegate(CScriptScene O);
		
		/// <summary>This method kills the thread used to load the scenes</summary>
		private void KillLoadThread()
		{
			try
			{
				if((m_threadLoadScenes != null) && (m_threadLoadScenes.IsAlive == true))
				{
					//FireDiagnostic("KillLoadThread", "KLT->ABORT");
					m_threadLoadScenes.Abort();
				}
			}
			catch
			{
			
			}
			
			m_threadLoadScenes = null;

		}// private void KillLoadThread()
			
		/// <summary>This method starts a thread that loads each of the scenes asynchronously</summary>
		private void AsyncLoadScenes()
		{
			if(m_delLoadScenes != null)
			{
				try
				{
					//	Kill the current thread
					KillLoadThread();
					
					//	Start a thread to fire the command
					m_threadLoadScenes = new System.Threading.Thread(new ThreadStart(this.AsyncThreadProc));
					m_threadLoadScenes.Start();
					//FireDiagnostic("AsyncLoadScenes", "ALS->R");
					return;
				}
				catch
				{
				}
				
			}
			
			//	Load synchronously because there must have been a problem
			LoadScenes();
					
		}// private void AsyncLoadScenes()
					
		/// <summary>This method will set the fully qualified path to the file to be rendered by the viewer</summary>
		/// <param name="Scene">The scene to be rendered</param>
		/// <returns>true if successful</returns>
		private bool SetFileSpec(CScriptScene Scene)
		{
			string			strFileSpec = "";
			CDxSecondary	dxSegment = null;
			
			if(Scene == null) return false;
			if(Scene.Secondary == null) return false;
			if(Scene.Secondary.GetSource() == null) return false;
			if(m_tmaxDatabase == null) return false;
			
			//	What type of scene is this?
			switch(Scene.Secondary.GetSource().MediaType)
			{
				case TmaxMediaTypes.Clip:
				case TmaxMediaTypes.Designation:
				
					//	The file we want is actually the parent segment
					if((dxSegment = (CDxSecondary)Scene.Secondary.GetSource().GetParent()) != null)
					{
						strFileSpec = m_tmaxDatabase.GetFileSpec(dxSegment);
					}
					
					break;
					
				case TmaxMediaTypes.Treatment:
				
					//	Set the path to the source image
					Scene.ZapSourceFileSpec = m_tmaxDatabase.GetFileSpec(Scene.Secondary.GetSource().GetParent());
					
					strFileSpec = m_tmaxDatabase.GetFileSpec(Scene.Secondary.GetSource());
					break;
					
				default:
				
					strFileSpec = m_tmaxDatabase.GetFileSpec(Scene.Secondary.GetSource());
					break;
			}

			if((strFileSpec == null) || (strFileSpec.Length == 0)) 
			{
				return false;
			}
			else
			{
				return Scene.Initialize(strFileSpec);
			}
			
		}// private bool SetFileSpec(CScriptScene Scene)
					
		/// <summary>This method will load the viewer associated with the specified scene</summary>
		/// <param name="Scene">The scene to be loaded</param>
		/// <returns>true if successful</returns>
		private bool LoadScene(CScriptScene Scene)
		{
			double	dFrom = 0;
			double	dTo = 0;
			
			if(Scene == null) return false;
			if(Scene.Secondary == null) return false;
			if(Scene.Secondary.GetSource() == null) return false;
			if(m_tmaxDatabase == null) return false;
			
			if(SetFileSpec(Scene) == false)
				return false;
			
			try
			{
				//	Is this a PowerPoint slide?
				if(Scene.Secondary.GetSource().MediaType == TmaxMediaTypes.Slide)
				{
					//	Make sure the exported image is up to date
					if(m_tmaxDatabase.CheckSlide((CDxSecondary)(Scene.Secondary.GetSource())) == false)
						return false;
				}				
				
				switch(Scene.Secondary.GetSource().MediaType)
				{
					case TmaxMediaTypes.Page:
					case TmaxMediaTypes.Treatment:
					case TmaxMediaTypes.Slide:
							
						return Scene.View(Scene.FileSpec);
					
					case TmaxMediaTypes.Segment:
					
						if(((CDxSecondary)Scene.Secondary.GetSource()).GetExtent() != null)
						{
							dFrom = ((CDxSecondary)Scene.Secondary.GetSource()).GetExtent().Start;
							dTo = ((CDxSecondary)Scene.Secondary.GetSource()).GetExtent().Stop;
						}
							
						return Scene.Play(Scene.FileSpec, dFrom, dTo);

					case TmaxMediaTypes.Clip:
					case TmaxMediaTypes.Designation:
					
						if(((CDxTertiary)Scene.Secondary.GetSource()).GetExtent() != null)
						{
							dFrom = ((CDxTertiary)Scene.Secondary.GetSource()).GetExtent().Start;
							dTo = ((CDxTertiary)Scene.Secondary.GetSource()).GetExtent().Stop;
						}

						return Scene.Play(Scene.FileSpec, dFrom, dTo);

					default:
							
						return false;
						
				}// switch(Scene.Secondary.GetSource().MediaType)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "LoadScene", m_tmaxErrorBuilder.Message(ERROR_LOAD_SCENE_EX, Scene.StatusText), Ex);
				return false;
			}

		}// private bool LoadScene(CScriptScene Scene)
					
		/// <summary>This method will make sure playback is stopped on all scenes</summary>
		private void StopScenes()
		{
			if(m_aScenes != null)
			{
				foreach(CScriptScene O in m_aScenes)
				{
					if((O.Viewer != null) && (O.Viewer.State == TmxStates.Playing))
					{
						O.Viewer.Stop();
						O.Viewer.Cue(TmxCueModes.Start);
					}
					
				}
				
			}
		}// private void StopScenes()
					
		private void LoadScenes()
		{
		}
		
		/// <summary>This method is the entry point for the thread used to load the scenes</summary>
		private void AsyncThreadProc()
		{
			//FireDiagnostic("AsyncThreadProc", "ATP->E");
			
			if((m_aScenes != null) && (m_aScenes.Count > 0))
			{
				foreach(CScriptScene O in m_aScenes)
				{
					BeginInvoke(m_delLoadScenes, new object [] { O } );
					//LoadScene(O);
				}
				
			}
			
			//FireDiagnostic("AsyncThreadProc", "ATP->R");
			
		
		}// private void m_delLoadScenes()
					
		/// <summary>This method will make sure media loaded in all scenes is up to date</summary>
		private void CheckMedia()
		{
			if(m_aScenes == null) return;
			if(m_aScenes.Count == 0) return;
//m_tmaxEventSource.FireDiagnostic(this, "CheckMedia", "check all media");			
			
			//	Inhibit processing of application activation notifications
			//
			//	NOTE:	We do this because the application loses and regains
			//			focus (thus causing recursive activations) when we
			//			export PowerPoint slides
			m_bIgnoreAppActivation = true;
			
			foreach(CScriptScene O in m_aScenes)
			{
				try
				{
					CheckMedia(O);
				}
				catch
				{
				}
				
			}
			
			m_bCheckMedia = false;
			m_bIgnoreAppActivation = false;
			
		}// private void CheckMedia()
					
		/// <summary>This method will make sure media loaded in the specified scene is up to date</summary>
		///	<param name="Scene">The scene to be checked</param>
		private void CheckMedia(CScriptScene Scene)
		{
			CDxSecondary dxSlide = null;
			
			if(Scene == null) return;
			if(Scene.Secondary == null) return;
			if(Scene.Secondary.GetSource() == null) return;
			if(Scene.IsLoaded == false) return;
			
			//	Is a PowerPoint slide loaded in the scene?
			if(Scene.Secondary.GetSource().MediaType == TmaxMediaTypes.Slide)
			{
				dxSlide = (CDxSecondary)(Scene.Secondary.GetSource());
			//m_tmaxEventSource.FireDiagnostic(this, "CheckMedia", "checking: " + Scene.FileSpec);	
				if(m_tmaxDatabase.CheckSlide(dxSlide, false) == false)
				{
					//m_tmaxEventSource.FireDiagnostic(this, "CheckMedia", "exporting: " + Scene.FileSpec);	
					if(m_tmaxDatabase.ExportSlide(dxSlide, false) == true)
					{
						//m_tmaxEventSource.FireDiagnostic(this, "CheckMedia", "reloading: " + Scene.FileSpec);	
						//	Reload the viewer
						Scene.View(Scene.FileSpec);
					}
					
					
				}
			
			}// if(Scene.Secondary.GetSource().MediaType == TmaxMediaTypes.Slide)
		
		}// private void CheckMedia(CScriptScene Scene)
					
		/// <summary>This method is called to set the cells used to size and position the scene rendering controls</summary>
		private void SetCells()
		{
			int	iMaxWidth = 0;
			int iMaxHeight = 0;
			int	iColumns = 0;
			int	iSeparatorWidth = 0;
			int	iLeft = 0;
			int	iTop = 0;
			
			//	Make sure we have the required objects
			if(m_ctrlPanel == null) return;
			if(m_ctrlPanel.IsDisposed == true) return;
			if(m_ctrlScene == null) return;
			if(m_aCells == null) return;
			
			try
			{
				//	Make sure we have a valid scene separator size
				if(m_iSceneSeparatorSize < MINIMUM_SCENE_SEPARATOR_SIZE)
					m_iSceneSeparatorSize = MINIMUM_SCENE_SEPARATOR_SIZE;
					
				//	Make sure we have a valid number of columns
				if((iColumns = m_iColumns) <= 0) iColumns = 1;
					
				//	What is the maximum width we have available?
				iMaxWidth = m_ctrlPanel.Width;
				if(m_ctrlScrollbar != null && m_ctrlScrollbar.Visible == true)
				{
					iMaxWidth -= m_ctrlScrollbar.Width;
				}
				
				//	What is the maximum height we have available?
				iMaxHeight = m_ctrlPanel.Height;
				if(m_ctrlStatusBar != null && m_ctrlStatusBar.Visible == true)
				{
					iMaxHeight -= m_ctrlStatusBar.Height;
				}
				
				//	Subtract out the width we need for the separations between the scenes
				iSeparatorWidth = ((iColumns + 1) * m_iSceneSeparatorSize);
				if((iMaxWidth - iSeparatorWidth) > iColumns)
				{
					iMaxWidth -= iSeparatorWidth;
				}
				else if(iMaxWidth > (2 * m_iSceneSeparatorSize))
				{
					//	Force single column
					iColumns = 1;
					iMaxWidth -= (2 * m_iSceneSeparatorSize);
				}
				else
				{
					iMaxWidth = 0;
				}
				
				//	Now determine the maximum width for each scene
				if((m_iSceneWidth = iMaxWidth / iColumns) < 0) m_iSceneWidth = 0;
				
				//	Determine the number of rows we can fit in the client area
				if(m_iSceneWidth > 0)
				{
					//	What is the height required for each scene control?
					m_iSceneHeight = m_ctrlScene.GetPreferredHeight(m_iSceneWidth);
				
					//	Add in the separator to get the total row height
					m_iRowHeight = (m_iSceneHeight + m_iSceneSeparatorSize);
					
					//	Just in case...
					Debug.Assert(m_iRowHeight > 0);
					if(m_iRowHeight == 0) m_iRowHeight = 1;
					
					//	How many rows can we fit in?
					m_iRows = iMaxHeight / m_iRowHeight;
		
		//FireDiagnostic("SetCells", "MH: " + iMaxHeight.ToString() + " RH: " + m_iRowHeight.ToString() + " ROWS " + m_iRows.ToString());			
					
					//	Should we allow for a partial row?
					if((iMaxHeight - (m_iRows * m_iRowHeight)) > (m_iSceneSeparatorSize + m_ctrlScene.SelectBorderSize + MINIMUM_SCENE_VISIBLE))
					{
						m_bPartialRow = true;
						m_iRows += 1;
					}
					else
					{
						m_bPartialRow = false;
					}
				}
				else
				{
					m_iSceneHeight = 0;
					m_iRowHeight = 0;
					m_iRows = 0;
					m_bPartialRow = false;
				}
				
				//	Clear the existing cells
				m_aCells.Clear();
				
				//	Use row major layout to determine the cell coordinates
				for(int i = 0; i < m_iRows; i++)
				{
					iLeft = m_iSceneSeparatorSize;
						
					for(int j = 0; j < iColumns; j++)
					{
						//	Add a new rectangle
						m_aCells.Add(new System.Drawing.Rectangle(iLeft, iTop, m_iSceneWidth, m_iSceneHeight));
							
						iLeft += (m_iSceneWidth + m_iSceneSeparatorSize);
					}
						
					//	Move to the next row
					iTop += (m_iSceneHeight + m_iSceneSeparatorSize);
				}
				
				//	Store the number of visible cells
				m_iVisibleScenes = m_aCells.Count;
				if(m_bPartialRow == true)
					m_iVisibleScenes -= m_iColumns;
					
				//	Set the scroll bar range since the number of cells has changed
				SetScrollRange();
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetCells", m_tmaxErrorBuilder.Message(ERROR_SET_CELLS_EX), Ex);
			}

		}// private void SetCells()

		/// <summary>Sets the text for all scene rendering controls</summary>
		private void SetSceneText()
		{
			//	Update each of the scenes
			if((m_aScenes != null) && (m_aScenes.Count > 0))
			{
				try
				{
					foreach(CScriptScene O in m_aScenes)
					{
						if(O.Secondary != null)
						{
							O.StatusText = GetSceneText(O.Secondary);
						}
						
					}// foreach(CScriptScene O in m_aScenes)
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "SetSceneText", m_tmaxErrorBuilder.Message(ERROR_SET_SCENE_TEXT_EX), Ex);
				}
			}
			
		}// private void SetSceneText()
		
		/// <summary>This method is called to set the first scene to be displayed</summary>
		/// <param name="iScene">Index of the first scene to display</param>
		private void SetFirstScene(int iScene)
		{
			int	iCell = 0;
			
			if(m_aScenes == null) return;
			if(m_aScenes.Count == 0) return;
			
			try
			{
				//	Make sure the scene is in range
				if(iScene < 0)
					iScene = 0;
				else if(iScene >= m_aScenes.Count)
					iScene = m_aScenes.Count - 1;

				//	Make sure the caller has specified the first in a row
				if((iScene % m_iColumns) > 0)
					iScene -= (iScene % m_iColumns);
					
				//	Lock on first scene if all scenes are visible
				if(m_aScenes.Count <= m_iVisibleScenes)
					iScene = 0;
					
				//	Determine the visible scenes
				m_iFirstVisibleScene = iScene ;
				m_iLastVisibleScene	 = m_iFirstVisibleScene + m_iVisibleScenes - 1;

				//	Move last scene if we have hit the end of the script
				if(m_iLastVisibleScene >= m_aScenes.Count)
				{
					m_iLastVisibleScene = m_aScenes.Count - 1 ;
				}
			
				// Turn the appropriate scenes on
				for(int i = 0; i < m_aScenes.Count; i++)
				{
					if(i < m_iFirstVisibleScene)
					{
						m_aScenes[i].Visible = false;
					}
					else
					{
						//	Do we have a cell available for this scene
						if(iCell < m_aCells.Count)
						{
							m_aScenes[i].SetPos((Rectangle)m_aCells[iCell]);
							
							if((m_aScenes[i].IsLoaded == false) && (m_aScenes[i].Failed == false))
							{
								LoadScene(m_aScenes[i]);
							}
							
							m_aScenes[i].Visible = true;
							iCell++;
						}
						else
						{
							m_aScenes[i].Visible = false;
						}
						
					}
					
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetFirstScene", m_tmaxErrorBuilder.Message(ERROR_SET_FIRST_SCENE_EX, iScene.ToString()), Ex);
			}

			//	Update the scroll bar position
			SetScrollPosition();
			
			//	Clear the last ensured index. It will be reset if this method
			//	is being called by EnsureVisible()
			m_iLastEnsuredScene = 0;

		}// private void SetFirstScene(int iScene)

		/// <summary>Called to determine if the specified scene is visible</summary>
		/// <param name="iScene">The index of the specified scene</param>
		/// <param name="bIncludePartial">True to include partially visible scenes</param>
		/// <returns>true if visible</returns>
		private bool IsSceneVisible(int iScene, bool bIncludePartial)
		{
			int iLast = m_iLastVisibleScene;
			
			//	Are we including partially visible scenes?
			if((bIncludePartial == true) && (m_bPartialRow == true))
			{
				if((iLast = m_iLastVisibleScene + m_iColumns) >= m_aScenes.Count)
					iLast = m_aScenes.Count - 1;
			}

			return  ((iScene >= m_iFirstVisibleScene ) && ( iScene <= iLast));
	
		}// private bool IsSceneVisible(int iScene, bool bIncludePartial)

		/// <summary>Called to ensure that the desired scene is fully visible</summary>
		/// <param name="iScene">Index of the desired scene</param>
		/// <param name="bSelect">true to select the specified scene</param>
		private void EnsureVisible(int iScene, bool bSelect)
		{
			int iFirst = 0;
			
			if(m_aScenes == null) return;
			if(m_aScenes.Count == 0) return;
			
			//	Make sure the scene is in range
			if(iScene < 0)
				iScene = 0;
			else if(iScene >= m_aScenes.Count)
				iScene = m_aScenes.Count - 1;

			//	Is the scene already visible?
			if(!IsSceneVisible(iScene, false))
			{
				// Attempt to keep prev and new scene visible.

				// Is the new scene after the current one, and within one page of it?
				if((iScene > m_iLastEnsuredScene) && (iScene < (m_iLastEnsuredScene + m_iVisibleScenes)))
				{
					// Scroll down the required number of rows to expose the new scene
					iFirst = m_iFirstVisibleScene;
					while((iFirst + m_iVisibleScenes - 1) < iScene)
						iFirst += m_iColumns;
				}

				// Is the new scene before the current one, and within one page of it?
				else if ((iScene < m_iLastEnsuredScene) && (iScene > (m_iLastEnsuredScene - m_iVisibleScenes)))
				{
					// Scroll up the required amount to expose the new scene
					iFirst = m_iFirstVisibleScene;
					while(iFirst > iScene)
						iFirst -= m_iColumns;
				}

				// Otherwise put the requested scene in the first row
				else
				{
					// Round down scene number to multiple of columns
					iFirst = ((int)(iScene / m_iColumns)) * m_iColumns ;
				}
			
				//	Reposition the scenes
				SetFirstScene(iFirst);

			}// if(!IsSceneVisible(iScene))
			
			//	Keep track of the last scene to be ensured visible
			//
			//	NOTE: This must be done AFTER the call to SetFirstScene() because
			//		  it gets reset there
			m_iLastEnsuredScene = iScene;
			
			//	Should we select the specified scene?
			if(bSelect == true)
				m_aScenes.Select(iScene);

		}// private void EnsureVisible(int iScene)

		/// <summary>This method traps SceneMouseDown events fired by one of the scene rendering controls</summary>
		/// <param name="sender">The scene object firing the event</param>
		/// <param name="e">System mouse event arguments</param>
		private void OnSceneMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//	Is this a single click?
			if(e.Clicks == 1)
			{
				OnMouseDown((CScriptScene)sender);
				
				StopScenes();
			}
			
		}// private void OnSceneMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)

		/// <summary>This method traps SceneMouseMove events fired by one of the scene rendering controls</summary>
		/// <param name="sender">The scene object firing the event</param>
		/// <param name="e">System mouse event arguments</param>
		private void OnSceneMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//	Call the generic handler
			OnMouseMove((CScriptScene)sender);
		}

		/// <summary>This method traps SceneMouseUp events fired by one of the scene rendering controls</summary>
		/// <param name="sender">The scene object firing the event</param>
		/// <param name="e">System mouse event arguments</param>
		private void OnSceneMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//	Is this a single click?
			if(e.Clicks == 1)
			{
				OnMouseUp((CScriptScene)sender);
			}
			
		}// private void OnSceneMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)

		/// <summary>This method traps MouseDown events fired by the background panel</summary>
		/// <param name="sender">The background panel</param>
		/// <param name="e">System mouse event arguments</param>
		private void OnPanelMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//	Is this a single click?
			if(e.Clicks == 1)
				OnMouseDown(null);
		
		}// private void OnPanelMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)

		/// <summary>This method traps MouseMove events fired by the background panel</summary>
		/// <param name="sender">The background panel</param>
		/// <param name="e">System mouse event arguments</param>
		private void OnPanelMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//	Redirect to the generic handler
			OnMouseMove(null);
		}

		/// <summary>This method traps MouseUp events fired by the background panel</summary>
		/// <param name="sender">The background panel</param>
		/// <param name="e">System mouse event arguments</param>
		private void OnPanelMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Clicks == 1)
				OnMouseUp(null);
		
		}// private void OnPanelMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)

		/// <summary>This method traps and handles double click events from the viewer control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The mouse event arguments</param>
		protected void OnSceneMouseDblClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			CScriptScene Scene = null;
			
			try
			{
				if((Scene = (CScriptScene)sender) == null) return;
				
				Debug.Assert(Scene.Secondary != null);
				Debug.Assert(Scene.Secondary.GetSource() != null);
				if(Scene.Secondary == null) return;
				if(Scene.Secondary.GetSource() == null) return;
				
				//	What type of media is being viewed
				switch(Scene.Secondary.GetSource().MediaType)
				{
					case TmaxMediaTypes.Slide:
					
						FireCommand(TmaxCommands.Edit, Scene, true);
						break;
						
					case TmaxMediaTypes.Segment:
					case TmaxMediaTypes.Clip:
					case TmaxMediaTypes.Designation:
					
						//	Toggle the playback state of the viewer
						if(Scene.Viewer != null)
						{
							switch(Scene.Viewer.State)
							{
								case TmxStates.Playing:
									
									Scene.Viewer.Pause();
									break;
									
								case TmxStates.Stopped:
								
									Scene.Viewer.Cue(TmxCueModes.Start, 0, true);
									break;
										
								case TmxStates.Paused:
								case TmxStates.Loaded:
									
									Scene.Viewer.Resume();
									break;
										
							}// switch(m_ctrlViewer.State)
						}
						break;
						
					default:
					
						break;
						
				}
				
			}
			catch
			{
			
			}

		}// protected void OnMouseDblClick(object sender, System.Windows.Forms.MouseEventArgs e)

		/// <summary>This method handles all mouse down events fired when the use presses the left mouse button</summary>
		/// <param name="Scene">The scene under the mouse position</param>
		private void OnMouseDown(CScriptScene Scene)
		{
			//	Is the mouse over a scene?
			if(Scene != null)
			{
				//	Is the user pressing the control key?
				if((Control.ModifierKeys & Keys.Control) == Keys.Control)
				{
					//	Select this scene without deselecting the others
					m_aScenes.Select(Scene, false);
					m_Anchor = Scene;
				}
				
				//	Is the user pressing the Shift key
				else if((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
				{
					int iAnchor = -1;
					int	iSelection = -1;
					
					//	Get the index of the anchor
					if(m_Anchor != null)
						iAnchor = m_aScenes.IndexOf(m_Anchor);
						
					//	Has the anchor been set?
					if(iAnchor >= 0)
					{
						//	Get the index of the scene being clicked
						if((iSelection = m_aScenes.IndexOf(Scene)) >= 0)
						{
							//	Select the range of scenes
							if(iAnchor < iSelection)
							{
								m_aScenes.Select(iAnchor, iSelection, true);
							}
							else
							{
								m_aScenes.Select(iSelection, iAnchor, true);
							}
							
						}
					}
					else
					{
						//	Select this scene and set the anchor
						m_aScenes.Select(Scene, true);
						m_Anchor = Scene;
					}
				
				}
				else
				{	
					//	Select this and deselect others
					m_aScenes.Select(Scene, true);
					
					//	Set the anchor
					m_Anchor = Scene;
				}
				
				//	Activate this scene if it's the first in the selection collection
				if(ReferenceEquals(Scene, m_aScenes.GetSelection()) == true)
				{
					Activate(Scene);
				}
			
			}// if(Scene != null)
			else
			{
				//	Clear all the selections
				m_aScenes.Select(null, true);
				m_Anchor = null;
			}
			
			//	Is the left button pressed?
			if(Control.MouseButtons == MouseButtons.Left)
			{
				//	Get the mouse position in the background panel's client area
				System.Drawing.Point Pos = m_ctrlPanel.PointToClient(Control.MousePosition);
				
				//	Set up the new drag boundries
				m_rcStartDrag.X = Pos.X - (m_rcStartDrag.Width / 2);
				m_rcStartDrag.Y = Pos.Y - (m_rcStartDrag.Height / 2);
			}
			
		}// private void OnMouseDown(CScriptScene Scene)

		/// <summary>This method handles all mouse move events fired by any of the child controls</summary>
		/// <param name="Scene">The scene under the mouse position</param>
		private void OnMouseMove(CScriptScene Scene)
		{
			MouseButtons		 eButton = Control.MouseButtons;
			System.Drawing.Point Pos = m_ctrlPanel.PointToClient(Control.MousePosition);
			
			//	Only process the left button moves
			if(eButton != MouseButtons.Left) return;
			
			//	What is the current drag state?
			switch(m_eDragState)
			{
				case PaneDragStates.None:
				
					//	Are we still within the non-drag boundries?
					if(m_rcStartDrag.Contains(Pos.X, Pos.Y) == false)
					{
						//	Do we have anything to drag?
						if(m_aScenes.GetSelections(m_aDragSelections) > 0)
						{
							//	Start the operation
							DoDragDrop();
						}
						
					}
					break;
					
				case PaneDragStates.Records:
				
					//	Update the insertion point
					
					break;
					
				case PaneDragStates.Source:
				default:
				
					break;
					
			}// switch(m_eDragState)
			
		}// private void OnMouseMove(CScriptScene Scene) 
		
		/// <summary>This method handles all MouseUp events fired when the use releases the mouse button</summary>
		/// <param name="Scene">The scene under the mouse position</param>
		private void OnMouseUp(CScriptScene Scene)
		{
			//	Just playing it safe
			if(m_aDragSelections != null)
				m_aDragSelections.Clear();
				
		}// private void OnMouseUp(CScriptScene Scene)

		/// <summary>This method traps events fired when the user clicks on the scrollbar</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">the event arguments</param>
		private void OnScroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		{
			int iNewFirst = 0;
			
			//FireDiagnostic("OnScroll", "OS-> " + e.Type.ToString() + " " + e.NewValue.ToString());
//			switch(e.Type)
//			{
//				case ScrollEventType.SmallDecrement:
//				
//					FireDiagnostic("OnScroll", "OS-> -1");
//					break;
//					
//				case ScrollEventType.LargeDecrement:
//				
//					FireDiagnostic("OnScroll", "OS-> -11111");
//					break;
//					
//				case ScrollEventType.SmallIncrement:
//				
//					FireDiagnostic("OnScroll", "OS-> +1");
//					break;
//					
//				case ScrollEventType.LargeIncrement:
//				
//					FireDiagnostic("OnScroll", "OS-> +11111");
//					break;
//					
//				default:
//				
//					break;
//					
//			}
			
			//	We perform this test because a single click on the scroll bar
			//	can result in multiple messages with different values for e.Type
			if((iNewFirst = e.NewValue * m_iColumns) != m_iFirstVisibleScene)
			{
				//FireDiagnostic("OnScroll", "OS: " + e.NewValue.ToString() + " - " + e.Type.ToString());
				SetFirstScene(iNewFirst);
			}
		
		}// private void OnScroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		
		/// <summary>This method traps SelectionChanging events fired by the scenes collection</summary>
		/// <param name="sender">The collection firing the event</param>
		private void OnSelectionChanging(object sender)
		{
			//	Update the selected scenes
			SetSceneProps();
						
		}// private void OnSelectionChanging(object sender)

		/// <summary>This method traps SelectionChanged events fired by the scenes collection</summary>
		/// <param name="sender">The collection firing the event</param>
		private void OnSelectionChanged(object sender)
		{
			//	Update the scene toolbar
			SetSceneControls();
			
			//	Update the status bar
			SetStatusText();
			
		}// private void OnSelectionChanged(object sender)

		/// <summary>This method will set the range of the scroll bar</summary>
		private void SetScrollRange()
		{
			int iVisibleRows = 0;
			int iTotalRows = 0;
			int iScrollRows = 0;
			int iLargeChange = 0;
			
			//	Just in case...
			Debug.Assert(m_ctrlScrollbar != null);
			Debug.Assert(m_ctrlScrollbar.IsDisposed == false);
			Debug.Assert(m_aScenes != null);
			Debug.Assert(m_aCells != null);
			Debug.Assert(m_iColumns > 0);
			if(m_ctrlScrollbar == null) return;
			if(m_ctrlScrollbar.IsDisposed == true) return;
			if(m_aScenes == null) return;
			if(m_aCells == null) return;
			if(m_iColumns <= 0) return;			
			
			try
			{
				//	Should we disable the scrollbar?
				if((m_aScenes.Count == 0) ||
				   (m_aScenes.Count <= m_iVisibleScenes) || 
				   (m_iVisibleScenes <= 0) || 
				   (m_aCells.Count == 0))
				{
					m_ctrlScrollbar.Minimum = 0;
					m_ctrlScrollbar.Maximum = 0;
					m_ctrlScrollbar.Enabled = false;
				}
				else
				{
					//	How many visible rows are there
					iVisibleRows = m_iVisibleScenes / m_iColumns;
					
					//	How many total rows are required?
					iTotalRows = m_aScenes.Count / m_iColumns;
					if((iTotalRows * m_iColumns) < m_aScenes.Count)
						iTotalRows += 1;
						
					//	How many rows do we need to be able to scroll?
					iScrollRows = iTotalRows - iVisibleRows;
					
					//	How big do we want the page steps to be?
					if(iScrollRows > iVisibleRows)
						iLargeChange = iVisibleRows;
					else
						iLargeChange = iScrollRows;
						
					//	Set the scroll bar properties
					//
					//	NOTES:	
					//
					//	Do NOT use m_ctrlScrollbar.LargeChange to perform the 
					//	calculation because there is some kind of order dependency
					//	that prevents it from showing up right away
					//
					//	As per MSDN documentation for Maximum property, the maximum value
					//	the user can reach by scrolling is (Maximum - LargeChange + 1)
					m_ctrlScrollbar.Enabled = true;
					m_ctrlScrollbar.SmallChange = 1;
					m_ctrlScrollbar.LargeChange = iLargeChange;
					m_ctrlScrollbar.Minimum = 0;
					m_ctrlScrollbar.Maximum = iScrollRows + iLargeChange - 1; // See Notes
					m_ctrlScrollbar.Value = m_ctrlScrollbar.Minimum;
					
					//FireDiagnostic("SetRange", "TR: " + iTotalRows.ToString() + " VR: " + iVisibleRows.ToString() + " SR: " + iScrollRows.ToString() + " MIN: " + m_ctrlScrollbar.Minimum.ToString() + " MAX: " + m_ctrlScrollbar.Maximum.ToString() + " SC: " + m_ctrlScrollbar.SmallChange.ToString() + " LC: " + m_ctrlScrollbar.LargeChange.ToString());
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetScrollRange", m_tmaxErrorBuilder.Message(ERROR_SET_SCROLL_RANGE_EX, (iScrollRows + iLargeChange - 1).ToString()), Ex);
			}
		
		}// private void SetScrollRange()
		
		/// <summary>This method will set the position of the scrollbar's thumbwheel</summary>
		private void SetScrollPosition()
		{
			int iPos = 0;
			
			Debug.Assert(m_ctrlScrollbar != null);
			Debug.Assert(m_ctrlScrollbar.IsDisposed == false);
			if(m_ctrlScrollbar == null) return;
			if(m_ctrlScrollbar.IsDisposed == true) return;

			try
			{
				//	What is the top row?
				if((m_iColumns > 0) && (m_iFirstVisibleScene >= 0))
				{
					//	Get current row position
					iPos = m_iFirstVisibleScene / m_iColumns;
				}
				
				//	Set the thumbwheel position
				if(iPos <= m_ctrlScrollbar.Minimum)
					m_ctrlScrollbar.Value = m_ctrlScrollbar.Minimum;
				else if(iPos > m_ctrlScrollbar.Maximum)
					m_ctrlScrollbar.Value = m_ctrlScrollbar.Maximum;
				else
					m_ctrlScrollbar.Value = iPos;
					
				//FireDiagnostic("SetScrollPosition", "SSP: iPos = " +iPos.ToString() + " FVS = " + m_iFirstVisibleScene.ToString() + " VS = " + m_iVisibleScenes.ToString());
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetScrollPosition", m_tmaxErrorBuilder.Message(ERROR_SET_SCROLL_POSITION_EX, iPos), Ex);
			}

		}// private void SetScrollPosition()
		
		/// <summary>
		/// This method is called internally to convert the key passed in
		///	an Infragistics event to its associated command enumeration
		/// </summary>
		/// <param name="strKey">The Infragistic key identifier</param>
		/// <returns>The associated script builder command</returns>
		private ScriptBuilderCommands GetCommand(string strKey)
		{
			try
			{
				Array aCommands = Enum.GetValues(typeof(ScriptBuilderCommands));
				
				foreach(ScriptBuilderCommands eCommand in aCommands)
				{
					if(eCommand.ToString() == strKey)
						return eCommand;
				}
				
			}
			catch
			{
			}
			
			return ScriptBuilderCommands.Invalid;
		}
		
		/// <summary>This method is called to get the text to be displayed with the scene</summary>
		/// <param name="dxScene">The scene's secondary exchange record</param>
		/// <returns>The text to be displayed with the scene</returns>
		private string GetSceneText(CDxSecondary dxScene)
		{
			return dxScene.GetText(m_eSceneTextMode);
				
		}// private string GetSceneText(CDxSecondary dxScene)
		
		/// <summary>This function will retrieve the tool associated with the specified command</summary>
		/// <param name="eCommand">The enumerated command identifier</param>
		/// <returns>Infragistic base class tool object</returns>
		private ToolBase GetUltraTool(ScriptBuilderCommands eCommand)
		{
			//	The enumerated name is also the key
			return GetUltraTool(eCommand.ToString());
		}
				
		/// <summary>Processes the specified command</summary>
		private void OnCommand(ScriptBuilderCommands eCommand)
		{
			CScriptScenes aSelections = null;
			
			//	Get the current selections
			if(m_aScenes != null)
				aSelections = m_aScenes.GetSelections();
				
			try
			{
				//	Direct to the appropriate handler
				switch(eCommand)
				{
					case ScriptBuilderCommands.Delete:
					
						if((aSelections != null) && (aSelections.Count > 0))
							OnCmdDelete(aSelections);
						break;
						
					case ScriptBuilderCommands.Print:
					
						OnCmdPrint(aSelections);
						break;
						
					case ScriptBuilderCommands.Find:
					
						OnCmdFind(aSelections);
						break;
						
					case ScriptBuilderCommands.Properties:
					
						if((aSelections != null) && (aSelections.Count == 1))
							OnCmdProperties(aSelections[0]);
						break;
						
					case ScriptBuilderCommands.Presentation:
					
						if((aSelections != null) && (aSelections.Count == 1))
							OnCmdPresentation(aSelections[0]);
						break;
						
					case ScriptBuilderCommands.Viewer:
					
						if((aSelections != null) && (aSelections.Count == 1))
							OnCmdViewer(aSelections[0]);
						break;
						
					case ScriptBuilderCommands.Tuner:
					
						if((aSelections != null) && (aSelections.Count == 1))
							OnCmdTuner(aSelections[0]);
						break;
						
					case ScriptBuilderCommands.Copy:
					
						if((aSelections != null) && (aSelections.Count > 0))
							OnCmdCopy(aSelections);
						break;
						
					case ScriptBuilderCommands.Cut:
					
						if((aSelections != null) && (aSelections.Count > 0))
							OnCmdCut(aSelections);
						break;
						
					case ScriptBuilderCommands.Paste:
					
						OnCmdPaste(null, false);
						break;
						
					case ScriptBuilderCommands.PasteBefore:
					
						if((aSelections != null) && (aSelections.Count == 1))
							OnCmdPaste(aSelections[0], true);
						break;
						
					case ScriptBuilderCommands.PasteAfter:
					
						if((aSelections != null) && (aSelections.Count == 1))
							OnCmdPaste(aSelections[0], false);
						break;
						
					case ScriptBuilderCommands.Preferences:
					
						OnCmdPreferences(aSelections);
						break;
						
					case ScriptBuilderCommands.New:
					
						OnCmdNew();
						break;
						
					case ScriptBuilderCommands.PinScript:
					
						OnCmdPinScript();
						break;
						
					case ScriptBuilderCommands.ShowScrollText:
					case ScriptBuilderCommands.HideScrollText:
					
						OnCmdScrollText(eCommand == ScriptBuilderCommands.ShowScrollText);
						break;
						
					case ScriptBuilderCommands.SetHighlighter1:
					case ScriptBuilderCommands.SetHighlighter2:
					case ScriptBuilderCommands.SetHighlighter3:
					case ScriptBuilderCommands.SetHighlighter4:
					case ScriptBuilderCommands.SetHighlighter5:
					case ScriptBuilderCommands.SetHighlighter6:
					case ScriptBuilderCommands.SetHighlighter7:
					
						OnCmdSetHighlighter((int)eCommand);
						break;
						
					default:
					
						//	Don't assert here because we have the spacers we've
						//	inserted in the toolbar
						//FireDiagnostic("OnUltraToolClick", "Invalid Command");
						break;
				
				}// switch(eCommand)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnCommand", "Ex: " + Ex.ToString());
			}
		
		}// private void OnCommand(ScriptBuilderCommands eCommand)

		/// <summary>Traps the ToolClick event fired by the toolbar manager</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event argument object</param>
		private void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		{
			ScriptBuilderCommands eCommand = ScriptBuilderCommands.Invalid;
			
			//	Don't bother if ignoring events
			if(m_bIgnoreUltraEvents == true) return;
			
			//	Get the command
			if(e.Tool != null && e.Tool.Key != null)
				eCommand = GetCommand(e.Tool.Key);
				
			if(eCommand != ScriptBuilderCommands.Invalid)
				OnCommand(eCommand);
				
		}// private void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
			
		/// <summary>This method handles events fired by the toolbar manager before displaying the popup menu</summary>
		/// <param name="sender">The toolbar manager firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnUltraBeforeDropDown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventArgs e)
		{
			try
			{
				switch(e.Tool.Key)
				{
					case "ContextMenu":
					
						OnBeforeContextMenu();
						break;
						
					case "Scripts":
					
						break;
						
					case "SetHighlighter":
					
						OnBeforeSetHighlighterMenu((PopupMenuTool)e.Tool, null);
						break;
						
				}
			
			}
			catch
			{
			}
							
		}// OnUltraBeforeDropDown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventArgs e)

		/// <summary>This method handles events fired by the toolbar manager after displaying the popup menu</summary>
		/// <param name="sender">The toolbar manager firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnUltraAfterCloseup(object sender, Infragistics.Win.UltraWinToolbars.ToolDropdownEventArgs e)
		{
			ScriptBuilderCommands eCommand = ScriptBuilderCommands.Invalid;
			
			//	Check each tool in the manager's collection
			foreach(ToolBase O in m_ultraToolbarManager.Tools)
			{
				//	Get the command for this tool
				if((eCommand = GetCommand(O.Key)) != ScriptBuilderCommands.Invalid)
				{
					//	Uninstall the shortcut if required
					if(GetCommandShortcut(eCommand) != Shortcut.None)
						O.SharedProps.Shortcut = Shortcut.None;
				}

			}// foreach(ToolBase O in m_ultraToolbarManager.Tools)
				
		}

		/// <summary>This method is called when the context menu is about to be shown</summary>
		private void OnBeforeContextMenu()
		{
			ScriptBuilderCommands	eCommand;
			CScriptScenes			Selections = null;
			
			Debug.Assert(m_ultraToolbarManager != null);
			Debug.Assert(m_ultraToolbarManager.Tools != null);
			if((m_ultraToolbarManager == null) ||( m_ultraToolbarManager.Tools == null)) return;
			
			//	Get the current selections
			if(m_aScenes != null)
				Selections = m_aScenes.GetSelections();

			//	Check each tool in the manager's collection
			foreach(ToolBase ultraTool in m_ultraToolbarManager.Tools)
			{
				if((eCommand = GetCommand(ultraTool.Key)) != ScriptBuilderCommands.Invalid)
				{
					try
					{
						ultraTool.SharedProps.Enabled = GetCommandEnabled(eCommand, Selections);
						ultraTool.SharedProps.Shortcut = GetCommandShortcut(eCommand);
					}
					catch
					{
					}
					
				}

			}// foreach(ToolBase ultraTool in m_ultraToolbarManager.Tools)
				
		}// private void OnBeforeContextMenu()

		/// <summary>This method is called to fill the script selection list</summary>
		private void FillScripts()
		{
			Infragistics.Win.UltraWinToolbars.ComboBoxTool ctrlScripts = null;
			
			if((ctrlScripts = (ComboBoxTool)GetUltraTool("Scripts")) != null)
			{
				if((ctrlScripts.ValueList != null) && (ctrlScripts.ValueList.ValueListItems != null))
				{
					ctrlScripts.ValueList.ValueListItems.Clear();
					
					if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null))
					{
						foreach(CDxPrimary dxPrimary in m_tmaxDatabase.Primaries)
						{
							//	Is this a script?
							if(dxPrimary.MediaType == TmaxMediaTypes.Script)
							{
								ctrlScripts.ValueList.ValueListItems.Add(dxPrimary, GetDisplayText(dxPrimary));
							}
							
						}
						
					}// if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null))
				
				}// if((ctrlScripts.ValueList != null) && (ctrlScripts.ValueList.ValueListItems != null))
			
			}// if((ctrlScripts = (ComboBoxTool)GetUltraTool("Scripts")) != null)
		
		}// private void FillScripts()
		
		/// <summary>This method is called to determine if the specified command should be enabled</summary>
		/// <param name="eCommand">The tree pane command enumeration</param>
		/// <param name="Scene">The current scene selections</param>
		/// <returns>true if command should be enabled</returns>
		private bool GetCommandEnabled(ScriptBuilderCommands eCommand, CScriptScenes Selections)
		{
			CTmaxItems tmaxItems = null;
			
			//	What is the command?
			switch(eCommand)
			{
				case ScriptBuilderCommands.Preferences:
				
					return true;	//	Always enabled
					
				case ScriptBuilderCommands.New:
				case ScriptBuilderCommands.PinScript:
				
					return (m_tmaxDatabase != null);
					
				case ScriptBuilderCommands.Print:
				case ScriptBuilderCommands.Find:
					
					//	All we need is valid media
					return ((m_tmaxDatabase != null) && 
							(m_tmaxDatabase.Primaries != null) && 
							(m_tmaxDatabase.Primaries.Count > 0));
				
				case ScriptBuilderCommands.Properties:
				case ScriptBuilderCommands.Viewer:
				case ScriptBuilderCommands.Presentation:
				case ScriptBuilderCommands.Tuner:
	
					//	These commands require single selection
					return ((Selections != null) && (Selections.Count == 1));
					
				case ScriptBuilderCommands.Delete:
				case ScriptBuilderCommands.Copy:
				case ScriptBuilderCommands.Cut:
				
					return ((Selections != null) && (Selections.Count > 0));
				
				case ScriptBuilderCommands.PasteBefore:
				case ScriptBuilderCommands.PasteAfter:
				case ScriptBuilderCommands.Paste:
				
					//	Must be some stuff in the clipboard
					if((m_tmaxClipboard == null) || (m_tmaxClipboard.ContainsMedia(true) == false))
					{
						return false;
					}
					
					//	Are we pasting new media to the end of the script?
					if(eCommand == ScriptBuilderCommands.Paste)
					{
						//	Can't have any selections
						return ((m_aScenes != null) && (m_aScenes.Primary != null) &&
						        ((Selections == null) || (Selections.Count == 0)));
					}
					else
					{
						//	Have to have one selection
						return ((Selections != null) && (Selections.Count == 1));
					}
						
				case ScriptBuilderCommands.ShowScrollText:
				case ScriptBuilderCommands.HideScrollText:
				case ScriptBuilderCommands.SetHighlighter:
				
					tmaxItems = GetCmdDesignationItems(true);
					return ((tmaxItems != null) && (tmaxItems.Count > 0));
				
				case ScriptBuilderCommands.SetHighlighter1:
				case ScriptBuilderCommands.SetHighlighter2:
				case ScriptBuilderCommands.SetHighlighter3:
				case ScriptBuilderCommands.SetHighlighter4:
				case ScriptBuilderCommands.SetHighlighter5:
				case ScriptBuilderCommands.SetHighlighter6:
				case ScriptBuilderCommands.SetHighlighter7:
				
					return (m_tmaxDatabase.Highlighters != null);
						
				default:
				
					break;
			}	
			
			return false;
		
		}// GetCommandEnabled(ScriptBuilderCommands eCommand, CScriptScene Scene)
		
		/// <summary>This method is called to get the shortcut key for the specified command</summary>
		/// <param name="eCommand">The tree pane command enumeration</param>
		/// <returns>The shortcut key if there is one</returns>
		private Shortcut GetCommandShortcut(ScriptBuilderCommands eCommand)
		{
			switch(eCommand)
			{
				case ScriptBuilderCommands.Copy:
					
					return Shortcut.CtrlC;
					
				case ScriptBuilderCommands.Paste:
					
					return Shortcut.CtrlV;
					
				case ScriptBuilderCommands.Print:
				
					return Shortcut.CtrlP;
					
				case ScriptBuilderCommands.Find:
				
					return Shortcut.CtrlF;

				case ScriptBuilderCommands.Delete:

					return Shortcut.ShiftDel;

				case ScriptBuilderCommands.Presentation:
				
					return Shortcut.F5;
				
				default:
				
					return Shortcut.None;
			
			}// switch(eCommand)

		}// protected virtual Shortcut GetCommandShortcut(ScriptBuilderCommands eCommand)
		
		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="Scene">The scene being passed with the command arguments</param>
		/// <param name="bSource">true to create the item using the scene's source record</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		/// <returns>true if successful</returns>
		private bool FireCommand(TmaxCommands eCommand, CScriptScene Scene, bool bSource, CTmaxParameters tmaxParameters)
		{
			CTmaxItem	tmaxItem = null;
			CTmaxItems	tmaxItems = null;
			
			//	Did the caller provide a scene?
			if(Scene != null)
			{
				//	Create an event item
				if((tmaxItem = GetCommandItem(Scene, bSource)) == null)
				{
					return false;
				}
				else
				{
					//	Create a collection to fire with the event
					tmaxItems = new CTmaxItems();
					tmaxItems.Add(tmaxItem);
				}
			}
			
			//	Fire the command
			return FireCommand(eCommand, tmaxItems, tmaxParameters);
					
		}//	FireCommand(TmaxCommands eCommand, CScriptScene Scene, CTmaxParameters tmaxParameters)

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="Scene">The scene being passed with the command arguments</param>
		/// <param name="bSource">true to create the item using the scene's source record</param>
		/// <returns>true if successful</returns>
		private bool FireCommand(TmaxCommands eCommand, CScriptScene Scene, bool bSource)
		{
			return FireCommand(eCommand, Scene, bSource, null);
					
		}//	FireCommand(TmaxCommands eCommand, CScriptScene Scene, CTmaxParameters tmaxParameters)

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="aScenes">The scenes being passed with the command arguments</param>
		/// <param name="bSource">true to create the item using the scene's source record</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		/// <returns>true if successful</returns>
		private bool FireCommand(TmaxCommands eCommand, CScriptScenes aScenes, bool bSource, CTmaxParameters tmaxParameters)
		{
			CTmaxItems	tmaxItems = null;
			
			//	Did the caller provide a scene collection?
			if((aScenes != null) && (aScenes.Count > 0))
			{
				//	Create the event items
				if((tmaxItems = GetCommandItems(aScenes, bSource)) == null)
				{
					return false;
				}

			}// if((aScenes != null) && (aScenes.Count > 0))
			
			//	Fire the command
			return FireCommand(eCommand, tmaxItems, tmaxParameters);
					
		}//	FireCommand(TmaxCommands eCommand, CScriptScenes aScenes, bool bSource, CTmaxParameters tmaxParameters)

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="Scenes">The scenes being passed with the command arguments</param>
		/// <param name="bSource">true to create the item using the scene's source record</param>
		/// <returns>true if successful</returns>
		private bool FireCommand(TmaxCommands eCommand, CScriptScenes aScenes, bool bSource)
		{
			//	Fire the command
			return FireCommand(eCommand, aScenes, bSource, null);
					
		}//	FireCommand(TmaxCommands eCommand, CScriptScenes aScenes, bool bSource)

		/// <summary>This method is called to get a TrialMax command item based on the specified scene rendering control</summary>
		/// <param name="Scene">The scene being passed with the command arguments</param>
		/// <param name="bSource">true to create the item using the scene's source record</param>
		/// <returns>The equivalent command item</returns>
		private CTmaxItem GetCommandItem(CScriptScene Scene, bool bSource)
		{
			CTmaxItem tmaxItem = null;
			
			try
			{
				if(Scene.Secondary != null)
				{
					//	Are we supposed to use the source?
					if(bSource == true)
					{
						if(((CDxSecondary)Scene.Secondary).GetSource() != null)
						{
							tmaxItem = new CTmaxItem(((CDxSecondary)Scene.Secondary).GetSource());
						}
					}
					else
					{
						tmaxItem = new CTmaxItem(Scene.Secondary);
					}
					
				}

				return tmaxItem;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetCommandItem", m_tmaxErrorBuilder.Message(ERROR_GET_COMMAND_ITEM_EX, Scene.StatusText), Ex);
				return null;
			}
			
		}// CTmaxItem GetCommandItem(CScriptScene Scene, bool bSource)
					
		/// <summary>This method is called to fire the command to request activation of the specified scene</summary>
		/// <param name="Scene">The scene to be activated</param>
		protected void Activate(CScriptScene Scene)
		{
			Debug.Assert(Scene != null);
			Debug.Assert(Scene.Secondary != null);
			
			m_bLocalActivation = true;
			
			//	Fire the command required to activate the scene
			FireCommand(TmaxCommands.Activate, Scene, false);
			
			m_bLocalActivation = false;
						
		}// protected void Activate(CScriptScene Scene)
		
		/// <summary>This method is called to create and populate a TrialMax command items collection</summary>
		/// <param name="Scenes">The scenes being passed with the command arguments</param>
		/// <param name="bSource">true to create the item using the scene's source record</param>
		/// <returns>The new command items collection</returns>
		protected CTmaxItems GetCommandItems(CScriptScenes aScenes, bool bSource)
		{
			CTmaxItems tmaxItems = null;
			
			if((aScenes != null) && (aScenes.Count > 0))
			{
				tmaxItems = new CTmaxItems();
				
				foreach(CScriptScene O in aScenes)
				{
					tmaxItems.Add(GetCommandItem(O, bSource));
				}
				
			}// if((aScenes != null) && (aScenes.Count > 0))

			return tmaxItems;
			
		}// GetCommandItems(CScriptScenes aScenes)
			
		/// <summary>This method initiates a drag/drop operation</summary>
		private void DoDragDrop()
		{
			CTmaxItem tmaxScript = null;
			
			//	Make sure we have the required objects
			Debug.Assert(m_aScenes.Primary != null);
			Debug.Assert(m_aDragSelections != null);
			Debug.Assert(m_aDragSelections.Count > 0);
			if(m_aScenes.Primary == null) return;
			if(m_aDragSelections == null) return;
			if(m_aDragSelections.Count == 0) return;
						
			//	Create the command item used to define the parent script
			if((tmaxScript = new CTmaxItem(m_aScenes.Primary)) != null)
			{
				//	Assign the source items
				tmaxScript.SourceItems = GetCommandItems(m_aDragSelections, false);
				
				if(tmaxScript.SourceItems != null)
				{
					//	Fire the event
					FireCommand(TmaxCommands.StartDrag, tmaxScript);
					
					//	Start the operation
					DoDragDrop(m_aDragSelections, (DragDropEffects.Copy | DragDropEffects.Link | DragDropEffects.Move));

					//	Notify the application that the user has stopped dragging
					//
					//	NOTE:	We don't care about the return value because it's up
					//			to the drop target to process the drop action
					FireCommand(TmaxCommands.CompleteDrag);

				}// if(tmaxScript.SourceItems != null)
				
			}// if((tmaxScript = new CTmaxItem(m_aScenes.Primary)) != null)
			
			//	Clear the selections
			m_aDragSelections.Clear();
			
		}// private void DoDragDrop() 
		
		/// <summary>This method is called to get the index of the row at the specified Y position</summary>
		/// <param name="iY">The Y position</param>
		/// <returns>The zero-based row index</returns>
		private int GetRowIndex(int iY)
		{
			int	iTop = 0;
			int iBottom = m_iSceneHeight + (m_iSceneSeparatorSize / 2);
			
			Debug.Assert(m_iRows > 0);
			Debug.Assert(m_aCells != null);
			Debug.Assert(m_aCells.Count > 0);
			if(m_iRows <= 0) return -1;
			if(m_aCells == null) return -1;
			if(m_aCells.Count == 0) return -1;
			
			//	Initialize the first column boundries
			iTop = ((Rectangle)m_aCells[0]).Top - (m_iSceneSeparatorSize / 2);
			iBottom = ((Rectangle)m_aCells[0]).Bottom + (m_iSceneSeparatorSize / 2);
			
			//	Is the mouse too close to the top?
			if(iY < iTop) return -1;
			
			for(int i = 0; i < m_iRows; i++)
			{
				//	Is this the row we're looking for?
				if((iY >= iTop) && (iY < iBottom))
				{
					return i;
				}
				else
				{
					iTop = iBottom;
					iBottom = iTop + m_iSceneHeight + m_iSceneSeparatorSize;
				}
				
			}// for(int i = 0; i < m_iRows; i++)
			
			//	Must be too far to the bottom
			return -1;
					
		}// private int GetRowIndex(int iY)
		
		/// <summary>This method is called to get the index of the column at the specified Y position</summary>
		/// <param name="iX">The X position</param>
		/// <returns>The zero-based column index</returns>
		private int GetColumnIndex(int iX)
		{
			int	iLeft = 0;
			int iRight = 0;
			
			Debug.Assert(m_iColumns > 0);
			Debug.Assert(m_aCells != null);
			Debug.Assert(m_aCells.Count > 0);
			if(m_iColumns <= 0) return -1;
			if(m_aCells == null) return -1;
			if(m_aCells.Count == 0) return -1;
			
			//	Initialize the first column boundries
			iLeft = ((Rectangle)m_aCells[0]).Left - (m_iSceneSeparatorSize / 2);
			iRight = ((Rectangle)m_aCells[0]).Right + (m_iSceneSeparatorSize / 2);
			
			//	Is the mouse too far left?
			if(iX < iLeft) return -1;
			
			//	Locate the column
			for(int i = 0; i < m_iColumns; i++)
			{
				//	Is this the column we're looking for?
				if((iX >= iLeft) && (iX < iRight))
				{
					return i;
				}
				else
				{
					iLeft  = iRight;
					iRight = iLeft + m_iSceneWidth + m_iSceneSeparatorSize;
				}
			}
			
			//	Must be too far to the right
			return -1;
		
		}// private int GetColumnIndex(int iX)
		
		/// <summary>This method is called to get the index of the cell at the specified location</summary>
		/// <param name="iX">The X position</param>
		/// <param name="iY">The Y position</param>
		/// <returns>The zero-based cell index</returns>
		private int GetCellIndex(int iX, int iY)
		{
			int	iRow = 0;
			int iColumn = 0;
			int iCell = -1;
			
			if((m_aCells == null) || (m_aCells.Count == 0)) return -1;
			
			iRow = GetRowIndex(iY);
			iColumn = GetColumnIndex(iX);
			
			if((iRow >= 0) && (iColumn >= 0))
			{
				iCell = ((iRow * m_iColumns) + iColumn);
				
				Debug.Assert(iCell >= 0);
				Debug.Assert(iCell < m_aCells.Count);
				
				if(iCell < 0) iCell = 0;
				if(iCell >= m_aCells.Count) iCell = m_aCells.Count;
			}
			
			return iCell;
		
		}// private int GetCellIndex(int iX, int iY)
		
		/// <summary>This method is called to get the index of the scene located in the specified cell</summary>
		/// <param name="iCell">The cell index</param>
		/// <returns>The zero-based scene index</returns>
		private int GetSceneIndex(int iCell)
		{
			int	iScene = 0;
			
			if((m_aCells == null) || (m_aCells.Count == 0)) return -1;
			if((m_aScenes == null) || (m_aScenes.Count == 0)) return -1;
			if(m_iFirstVisibleScene < 0) return -1;
			
			if((iScene = (m_iFirstVisibleScene + iCell)) >= m_aScenes.Count)
				return (-1);
			else
				return iScene;

		}// private int private int GetSceneIndex(int iCell)
		
		/// <summary>This method is called to set the members that define the drop target</summary>
		/// <param name="iX">Mouse X position</param>
		/// <param name="iY">Mouse Y position</param>
		private void SetDropTarget(int iX, int iY)
		{
			Rectangle	rcCell;
			int			iTop = 0;
			int			iLeft  = 0;
			int			iScene = -1;
			bool		bBefore = false;
			
			//	Should we just reset the target?
			//
			//	NOTE:	We do not check for empty scenes collection here because
			//			we want user to be able to drop into empty collection
			if((iX < 0) || (iY < 0) || (m_aCells == null) || 
			   (m_aScenes == null) || (m_aCells.Count == 0))
			{
				m_iDropScene = -1;
				m_iDropCell  = -1;
				DrawDropIndicator(true); // Erase
				return;
			}
			
			//	Get the cell closest to the specified position
			if((m_iDropCell = GetCellIndex(iX, iY)) >= 0)
			{				
				Debug.Assert(m_iDropCell < m_aCells.Count);
				rcCell = (System.Drawing.Rectangle)(m_aCells[m_iDropCell]);

				//	Get the scene to use as the drop target
				if(m_aScenes.Count > 0)
				{
					iScene = GetSceneIndex(m_iDropCell);
					Debug.Assert(iScene < m_aScenes.Count);
				}
				
			}
			else
			{
				//	This prevents compiler errors
				rcCell = (System.Drawing.Rectangle)(m_aCells[0]);
			}
			
			//	Are we dropping before or after the scene?
			if(iScene >= 0)
			{
				// Vertical insertion indicator?
				if(m_iColumns > 1)
				{
					//	Are we to the left of the center?
					if(iX < (rcCell.Left + (rcCell.Width / 2)))
					{
						bBefore = true;
					}

				}
				else // if(m_iColumns > 1)
				{
					Debug.Assert(m_iColumns == 1);
					
					//	Are we above the center line?
					if(iY < (rcCell.Top + (rcCell.Height / 2)))
					{
						bBefore = true;
					}

				}
				
			}// if(iScene >= 0)
			else
			{
				//	Use the last scene in the collection if there is one
				if(m_aScenes.Count > 0)
				{
					iScene = m_aScenes.Count - 1;
					Debug.Assert(iScene >= 0);
					
					//	Change the drop cell to match this scene
					if((m_iDropCell = iScene - m_iFirstVisibleScene) >= 0)
					{
						rcCell = (System.Drawing.Rectangle)(m_aCells[m_iDropCell]);
					}
				
				}
				
			}
			
			//	Has the drop target changed?
			if((iScene != m_iDropScene) || (bBefore != m_bDropBefore))
			{
				//	Erase the existing indicator
				DrawDropIndicator(true);
				
				//	Save the new values
				m_iDropScene  = iScene;
				m_bDropBefore = bBefore;
				
				//	Do we have a valid drop target?
				if(m_iDropScene >= 0)
				{
					//	Calculate the new indicator coordinates
					if(m_iColumns > 1)
					{
						iTop = rcCell.Top;
						m_rcDropIndicator.Width = 0;
						m_rcDropIndicator.Height = rcCell.Height;
								
						//	Are we to the left of the center?
						if(m_bDropBefore == true)
						{
							iLeft = rcCell.Left - (m_iSceneSeparatorSize / 2);
						}
						else
						{
							iLeft = rcCell.Right + (m_iSceneSeparatorSize / 2);
						}
							
					}
					else
					{
						iLeft = rcCell.Left;
						m_rcDropIndicator.Width = rcCell.Width;
						m_rcDropIndicator.Height = 0;
								
						if(m_bDropBefore == true)
						{
							iTop = rcCell.Top - (m_iSceneSeparatorSize / 2);
						}
						else
						{
							iTop = rcCell.Bottom + (m_iSceneSeparatorSize / 2);
						}			
					
					}// if(m_iColumns > 1)
					
					//	Update the drop indicator
					m_rcDropIndicator.Location = new System.Drawing.Point(iLeft, iTop);
					DrawDropIndicator(false);
					
				}// if(iScene >= 0)
		
			}// if((iScene != m_iDropScene) || (bBefore != m_bDropBefore))
			
		}// private void SetDropTarget(int iCell, int iScene, int iX, int iY)
		
		/// <summary>This method will draw or erase the drop target indicator</summary>
		/// <param name="bErase">true to erase the indicator</param>
		private void DrawDropIndicator(bool bErase)
		{
			System.Drawing.Graphics gd = null;
			System.Drawing.Pen pen = null;
			
			//	Is there anything to draw?
			if((m_rcDropIndicator.Width == 0) && (m_rcDropIndicator.Height == 0)) return;
			
			//	Make sure the background panel is still valid
			if((m_ctrlPanel == null) || (m_ctrlPanel.IsDisposed == true)) return;
			
			try
			{
				//	Get a graphics device for the background panel
				gd = m_ctrlPanel.CreateGraphics();
				
				//	Create the pen used to draw the indicator
				if(bErase)
					pen = new System.Drawing.Pen(SystemColors.Control, 3);
				else
					pen = new System.Drawing.Pen(SystemColors.ControlText, 3);
					
				//	Horizontal ?
				if(m_rcDropIndicator.Height == 0)
				{
					gd.DrawLine(pen, m_rcDropIndicator.Left, 
									 m_rcDropIndicator.Top,
									 m_rcDropIndicator.Left + m_rcDropIndicator.Width,
									 m_rcDropIndicator.Top);
				}
				else
				{
					gd.DrawLine(pen, m_rcDropIndicator.Left, 
						m_rcDropIndicator.Top,
						m_rcDropIndicator.Left,
						m_rcDropIndicator.Top + m_rcDropIndicator.Height);
				}

				//	Reset if we just erased the indicator
				if(bErase == true)
				{
					m_rcDropIndicator.Width = 0;
					m_rcDropIndicator.Height = 0;
				}
				
			}
			catch
			{
			}
			finally
			{
				//	Clean up
				if(gd != null)
					gd.Dispose();
				if(pen != null)
					pen.Dispose();
			}
		
		}// private void DrawDropIndicator(bool bErase)
		
		/// <summary>This method is called to allow derived classes to process the acceptance of media dropped in the pane</summary>
		/// <param name="eEffects">The drag/drop effects calculated by this pane in the OnDragOver() method</param>
		private void OnDropRecords(DragDropEffects eEffects)
		{
			CDxSecondary dxScene = null;
			
			//	Check these various conditions before going on
			if(m_tmaxDragData == null) return;
			if(m_tmaxDragData.SourceItems == null) return;
			if(m_tmaxDragData.SourceItems.Count == 0) return;
			if(m_tmaxDragData.SourceItems[0].MediaLevel == TmaxMediaLevels.None) return;
			if(m_tmaxDragData.SourceItems[0].IPrimary == null) return;
			
			Debug.Assert((eEffects == DragDropEffects.Link) || (eEffects == DragDropEffects.Move));
		
			//	Are we reordering records?
			if(eEffects == DragDropEffects.Move)
			{
				//	Must have a drop target
				if((m_iDropScene >= 0) && (m_iDropScene < m_aScenes.Count))
				{
					Reorder(m_tmaxDragData.SourceItems, m_aScenes[m_iDropScene], m_bDropBefore);
				}
			}
			else
			{
				//	Do we have a drop target?
				if((m_iDropScene >= 0) && (m_iDropScene < m_aScenes.Count))
				{
					//	Make sure we're not trying to insert after the last scene
					if((m_iDropScene < (m_aScenes.Count - 1)) || (m_bDropBefore == true))
					{
						dxScene = m_aScenes[m_iDropScene].Secondary;
					}
				}
				
				//	Add the new scenes
				AddScenes(m_tmaxDragData.SourceItems, dxScene, m_bDropBefore);
				
			}
			
		}// private void OnDropRecords(DragDropEffects eEffects)
		
		/// <summary>This method will add new scenes to the script</summary>
		/// <param name="tmaxSource">The collection of source media for the new scenes</param>
		/// <param name="dxInsert">The scene that identifies the insertion point</param>
		/// <param name="bBefore">true to insert before the specified target scene</param>
		protected void AddScenes(CTmaxItems tmaxSource, CDxSecondary dxInsert, bool bBefore)
		{
			CTmaxParameters	tmaxParameters = null;
			CTmaxItem		tmaxItem = null;
			
			//	Make sure we have the required objects
			if(m_aScenes.Primary == null) return;
			if(tmaxSource == null) return;
			if(tmaxSource.ContainsMedia(true) == false) return;
			
			try
			{
				//	Create the command item used to define the parent script
				tmaxItem = new CTmaxItem(m_aScenes.Primary);
					
				//	Assign the source items
				tmaxItem.SourceItems = new CTmaxItems();
				Pack(tmaxSource, true, tmaxItem.SourceItems);
					
				//	Are we inserting into the script
				if(dxInsert != null)
				{
					Debug.Assert(dxInsert.Primary != null);
					Debug.Assert(ReferenceEquals(dxInsert.Primary, m_aScenes.Primary) == true);
					
					//	Create the required parameters for the event
					tmaxParameters = new CTmaxParameters();
					tmaxParameters.Add(TmaxCommandParameters.Before, bBefore);
					
					//	Put the insertion point in the subitem collection
					tmaxItem.SubItems.Add(new CTmaxItem(dxInsert));

					//	Fire the event
					FireCommand(TmaxCommands.Add, tmaxItem, tmaxParameters);
				}
				else
				{
					//	Fire the event
					FireCommand(TmaxCommands.Add, tmaxItem);
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddScenes", m_tmaxErrorBuilder.Message(ERROR_ADD_SCENES_EX), Ex);
			}
			
		}// protected void AddScenes(CTmaxItems tmaxSource, CDxSecondary dxInsert, bool bBefore)
		
		/// <summary>This method is called to move the specified children to the new location</summary>
		/// <param name="tmaxScenes">Scenes to be relocated</param>
		/// <param name="Location">New location at which to place the scenes</param>
		/// <param name="bBefore">true if new position is before the specified location</param>
		private void Reorder(CTmaxItems tmaxScenes, CScriptScene Location, bool bBefore)
		{
			CTmaxItem	tmaxScript = null;
			int			iInsert = -1;
			int			i;
			
			//	Check these various conditions before going on
			if(tmaxScenes == null) return;
			if(tmaxScenes.Count == 0) return;
			if(m_aScenes == null) return;
			if(m_aScenes.Primary == null) return;
			if(m_aScenes.Count == 0) return;
			
			//	Create an event item to represent the script
			tmaxScript = new CTmaxItem(m_aScenes.Primary);
			
			//	Get the index at which the insertion point is located
			if(Location != null)
			{
				if((iInsert = m_aScenes.IndexOf(Location)) >= 0)
				{
					//	Adjust the insertion point if inserting after
					if(bBefore == false)
						iInsert++;
				}
				
			}
			
			//	Start by adding all scenes up to the insertion point that don't
			//	appear in the selected collection
			for(i = 0; i < iInsert; i++)
			{
				//	Make sure this scene is not being moved
				if(tmaxScenes.Find(m_aScenes[i].Secondary) == null)
				{
					//	Add to the event item's collection
					tmaxScript.SubItems.Add(new CTmaxItem(m_aScenes[i].Secondary));
				}
			}
			
			//	Now put in all of the scenes being moved
			foreach(CTmaxItem tmaxScene in tmaxScenes)
			{
				tmaxScript.SubItems.Add(new CTmaxItem(tmaxScene));
			}
			
			//	Now put the rest of the scenes in the event collection
			for(; i < m_aScenes.Count; i++)
			{
				if(tmaxScenes.Find(m_aScenes[i].Secondary) == null)
				{
					//	Add to the event item's collection
					tmaxScript.SubItems.Add(new CTmaxItem(m_aScenes[i].Secondary));
				}
			}
			
			//	Ask the system to reorder based on the subitem collection's new order
			FireCommand(TmaxCommands.Reorder, tmaxScript);
			
		}// private void Reorder(CTmaxItems tmaxScenes, CScriptScene Location, bool bBefore)
		
		/// <summary>This method will update the properties of all selected scenes</summary>
		private void SetSceneProps()
		{
			CScriptScenes	aSelections = null;
			bool			bModified = false;
			short			sPeriod = 0;

			//	Get the current selections
			if((m_aScenes != null) && (m_aScenes.Primary != null))
				aSelections = m_aScenes.GetSelections();
				
			//	Do we have anything to process?
			if(aSelections == null) return;
			if(aSelections.Count == 0) return;
				
			//	Get the specified transition period
			if(m_ctrlAutoPeriod.Text.Length > 0)
			{
				try	{ sPeriod = System.Convert.ToInt16(m_ctrlAutoPeriod.Text); }
				catch {}
			}
			
			//	Check each selection to see if it's values have
			//	changed
			foreach(CScriptScene O in aSelections)
			{
				//	We should have a secondary record
				Debug.Assert(O.Secondary != null);
				if(O.Secondary == null) continue;
				
				//	Should we process the hide check box?
				if(m_ctrlHideScene.CheckState != CheckState.Indeterminate)
				{
					if(m_ctrlHideScene.CheckState == CheckState.Checked)
					{
						if(O.Secondary.Hidden == false)
						{
							O.Secondary.Hidden = true;
							bModified = true;
						}
					}
					else
					{
						if(O.Secondary.Hidden == true)
						{
							O.Secondary.Hidden = false;
							bModified = true;
						}
					}
					
				}// if(m_ctrlHideScene.CheckState != CheckState.Indeterminate)
				
				//	Should we process the auto-transition check box?
				if((m_ctrlAutoTransition.Enabled == true) &&
					(m_ctrlAutoTransition.CheckState != CheckState.Indeterminate))
				{
					if(m_ctrlAutoTransition.CheckState == CheckState.Checked)
					{
						if(O.Secondary.AutoTransition == false)
						{
							O.Secondary.AutoTransition = true;
							O.Secondary.TransitionTime = sPeriod;
							bModified = true;
						}
						else if(O.Secondary.TransitionTime != sPeriod)
						{
							O.Secondary.TransitionTime = sPeriod;
							bModified = true;
						}
						else
						{
						}
						
					}
					else
					{
						if(O.Secondary.AutoTransition == true)
						{
							O.Secondary.AutoTransition = false;
							bModified = true;
						}
					}
					
				}// if(m_ctrlAutoTransition.CheckState != CheckState.Indeterminate)
				
				// Do we need to update this record?
				if(bModified == true)
				{
					FireCommand(TmaxCommands.Update, O, false);
					bModified = false;
				}
			
			}// foreach(CScriptScene O in aSelections)
			
		}// private void SetSceneProps()

		/// <summary>This method will update the scenes toolbar</summary>
		private void SetSceneControls()
		{
			CScriptScenes	aSelections = null;
			short			sPeriod = -1;
			int				iSelections = 0;
			bool			bHidden = false;
			bool			bAutoTransition = false;
			
			//	Get the current selections
			if((m_aScenes != null) && (m_aScenes.Primary != null))
				aSelections = m_aScenes.GetSelections();
				
			//	How many selections do we have?
			if(aSelections != null) 
				iSelections = aSelections.Count;
				
			//	Enable/disable the controls
			m_ctrlHideScene.Enabled = (iSelections > 0);
			m_ctrlHideSceneLabel.SharedProps.Enabled = (iSelections > 0);
			
			//	Do we have any selections?
			if(iSelections > 0)
			{
				//	Initialize the controls with the first selection
				bHidden = aSelections[0].Secondary.Hidden;
				m_ctrlHideScene.CheckState = (bHidden == true) ? CheckState.Checked : CheckState.Unchecked;
					
				bAutoTransition = aSelections[0].Secondary.AutoTransition;
				m_ctrlAutoTransition.CheckState = (bAutoTransition == true) ? CheckState.Checked : CheckState.Unchecked;
					
				if((sPeriod = aSelections[0].Secondary.TransitionTime) < 0)
					sPeriod = 0;
					
				//	Now check the remaining scenes
				for(int i = 1; i < iSelections; i++)
				{
					//	Does this object have a different hidden state?
					if(aSelections[i].Secondary.Hidden != bHidden)
						m_ctrlHideScene.CheckState = CheckState.Indeterminate;
						
					//	Does this object have a different auto transition state state?
					if(aSelections[i].Secondary.AutoTransition != bAutoTransition)
						m_ctrlAutoTransition.CheckState = CheckState.Indeterminate;
						
				}// for(int i = 1; i < iSelections; i++)
				
				//	Is this a playlist?
				if(m_aScenes.Primary.Playlist == true)
				{
					m_ctrlAutoTransition.Enabled = false;
					m_ctrlAutoTransition.CheckState = CheckState.Unchecked;
					m_ctrlAutoTransitionLabel.SharedProps.Enabled = false;
					
					m_ctrlAutoPeriod.Text = "";
					m_ctrlAutoPeriod.SharedProps.Enabled = false;
					m_ctrlAutoPeriodLabel.SharedProps.Enabled = false;
				}
				else
				{
					m_ctrlAutoTransition.Enabled = true;
					m_ctrlAutoTransitionLabel.SharedProps.Enabled = true;
					
					if(m_ctrlAutoTransition.CheckState == CheckState.Indeterminate)
					{
						m_ctrlAutoPeriod.Text = "";
						m_ctrlAutoPeriod.SharedProps.Enabled = false;
						m_ctrlAutoPeriodLabel.SharedProps.Enabled = false;
					}
					else
					{
						m_ctrlAutoPeriod.Text = sPeriod.ToString();
						
						if(m_ctrlAutoTransition.CheckState == CheckState.Checked)
						{
							m_ctrlAutoPeriod.SharedProps.Enabled = true;
							m_ctrlAutoPeriodLabel.SharedProps.Enabled = true;
						}
						else
						{
							m_ctrlAutoPeriod.SharedProps.Enabled = false;
							m_ctrlAutoPeriodLabel.SharedProps.Enabled = false;
						}
					
					}
				
				}
			
			}
			else
			{
				m_ctrlHideScene.CheckState = CheckState.Unchecked;
				m_ctrlAutoTransition.CheckState = CheckState.Unchecked;
				m_ctrlAutoTransition.Enabled = false;
				m_ctrlAutoTransitionLabel.SharedProps.Enabled = false;
				m_ctrlAutoPeriod.Text = "";
				m_ctrlAutoPeriod.SharedProps.Enabled = false;
				m_ctrlAutoPeriodLabel.SharedProps.Enabled = false;
			}
				
		}// private void UpdateScenesToolbar()

		/// <summary>This method is called when the user has deleted children from the specified parent</summary>
		/// <param name="tmaxItem">The parent of the deleted records</param>
		protected void OnDeleted(CTmaxItem tmaxParent)
		{
			//	Have primary records been deleted?
			if(tmaxParent.MediaLevel == TmaxMediaLevels.None)
			{
				//	Check to see if the active script has been deleted
				foreach(CTmaxItem O in tmaxParent.SubItems)
				{
					//	Has a script been deleted?
					if(O.MediaType == TmaxMediaTypes.Script)
					{
						//	Is this the active script?
						if((m_aScenes.Primary != null) && 
							(ReferenceEquals(O, m_aScenes.Primary) == true))
						{
							//	Unload the active script
							Unload();
						}
						
						//	Remove this selection
						RemoveSelection((CDxPrimary)O.IPrimary);
						
					}// if(O.MediaType == TmaxMediaTypes.Script)
					
				}// foreach(CTmaxItem O in tmaxParent.SubItems)
				
			}
			
			//	Have secondary records been deleted?
			else if(tmaxParent.MediaLevel == TmaxMediaLevels.Primary)
			{
				//	Have they been deleted from the active script?
				if((m_aScenes.Primary != null) &&
					(ReferenceEquals(m_aScenes.Primary, tmaxParent.GetMediaRecord()) == true))
				{
					//	Delete each of the scenes
					OnScenesDeleted(tmaxParent.SubItems);

				}// if(ReferenceEquals(m_aScenes.Primary, tmaxParent.GetMediaRecord()) == true)
				
			}// else if(tmaxParent.MediaLevel == TmaxMediaLevels.Primary)
				
			//	Make sure all scene text is correct
			SetSceneText();

		}// protected void OnDeleted(CTmaxItem tmaxParent)
		
		/// <summary>This method handles events fired when the user clicks on the AutoTransition check box</summary>
		/// <param name="sender">The toolbar manager firing the event</param>
		/// <param name="e">The system event argument object</param>
		private void OnClickAutoTransition(object sender, System.EventArgs e)
		{
			if(m_ctrlAutoTransition.CheckState == CheckState.Checked)
			{
				m_ctrlAutoPeriod.SharedProps.Enabled = true;
				m_ctrlAutoPeriodLabel.SharedProps.Enabled = true;
			}
			else
			{
				m_ctrlAutoPeriod.SharedProps.Enabled = false;
				m_ctrlAutoPeriodLabel.SharedProps.Enabled = false;
			}
		
		}// private void OnClickAutoTransition(object sender, System.EventArgs e)
		
		#endregion Private Methods

		private void OnMouseHover(object sender, System.EventArgs e)
		{
			//FireDiagnostic("OnMouseHover", "MH");
		}

		/// <summary>This event is fired by the toolbar manager when it is about to display the customize menu</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The cancelable event arguments</param>
		private void OnUltraBeforeToolbarListDropdown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventArgs e)
		{
			// Prevent this menu from coming up
			e.Cancel = true;
		}

		private void OnUltraToolValueChanged(object sender, Infragistics.Win.UltraWinToolbars.ToolEventArgs e)
		{
			ValueListItem	vlItem;
			ComboBoxTool	ctrlScripts;
			CTmaxParameters tmaxParameters = null;
			CDxPrimary		dxScript = null;
			CTmaxItem		tmaxScript = null;			
			
			if(m_bIgnoreUltraEvents == true) return;
			
			//	Is this the scripts selection box?
			if(e.Tool.Key == "Scripts")
			{
				if((ctrlScripts = (ComboBoxTool)e.Tool) != null)
				{
					if((vlItem = (ValueListItem)ctrlScripts.SelectedItem) != null)
					{
						if((dxScript = (CDxPrimary)vlItem.DataValue) != null)
						{
							//	Create a new event item to identify the script
							tmaxScript = new CTmaxItem();
							tmaxScript.SetRecord(dxScript);
							
							//	Create the required parameters for the event
							tmaxParameters = new CTmaxParameters();
							tmaxParameters.Add(TmaxCommandParameters.Builder, true);
				
							FireCommand(TmaxCommands.Open, tmaxScript, tmaxParameters);
						}
					}
					
				}
			}
		}
		
		protected override void DefWndProc(ref Message m)
		{
			string strText = "";
			
			//	Is the user clicking on one of the grids?
			//
			//	NOTE:	We trap this because there is a discontinuity between the 
			//			property grid control and the parent pane. For some reason
			//			the pane does not get selected when the user clicks on
			//			the grid
			if(m.Msg == FTI.Shared.Win32.User.WM_MOUSEACTIVATE)
			{
				//	Make sure the parent is selected
				this.Parent.Select();
			}
		
			if(m.Msg == FTI.Shared.Win32.User.WM_PAINT)
			{
				strText = "WM_PAINT";
			}
			else if(m.Msg == FTI.Shared.Win32.User.WM_NCPAINT)
			{
				strText = "WM_NCPAINT";
				System.IntPtr result = new System.IntPtr(0L);
				m.Msg = 0;
				m.Result = result;
				return;
			}
			else if(m.Msg == 311)
			{
				System.IntPtr result = new System.IntPtr(0L);
				m.Result = result;
				return;
			}
			else if(m.Msg == FTI.Shared.Win32.User.WM_ERASEBKGND)
			{
				strText = "WM_ERASEBKGND";
			}
			else
			{
				strText = m.Msg.ToString();
			}

			//FireDiagnostic("DEF", "BUILDER DWP: " + strText);
			base.DefWndProc(ref m);
		}

		private void m_ctrlPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		#region Properties
		
		/// <summary>The active script's primary exchange interface</summary>
		public CDxPrimary Script
		{
			get
			{
				return m_aScenes.Primary;
			}
			
		}// Script property
		
		/// <summary>Text mode used to display text in each scene's status bar</summary>
		public FTI.Shared.Trialmax.TmaxTextModes SceneTextMode
		{
			get
			{
				return m_eSceneTextMode;
			}
			set
			{
				m_eSceneTextMode = value;
			
				SetSceneText();

			}
			
		}// SceneTextMode Property
		
		/// <summary>Text mode used to display text in the builder's status bar</summary>
		public FTI.Shared.Trialmax.TmaxTextModes StatusTextMode
		{
			get
			{
				return m_eStatusTextMode;
			}
			set
			{
				m_eStatusTextMode = value;
			
				//	Update the status bar
				SetStatusText();
			}
			
		}// StatusTextMode Property
		
		/// <summary>Size in pixels used to separate scene rendering controls</summary>
		public int SceneSeparatorSize
		{
			get
			{
				return m_iSceneSeparatorSize;
			}
			set
			{
				m_iSceneSeparatorSize = value;
				
				RecalcLayout();
			}
			
		}// SceneSeparatorSize property
		
		/// <summary>Number of columns of scene rendering controls</summary>
		public int Columns
		{
			get
			{
				return m_iColumns;
			}
			set
			{
				m_iColumns = value;
				
				RecalcLayout();
			}
			
		}// Columns property
		
		#endregion Properties

	}// public class CScriptBuilder : FTI.Trialmax.Panes.CBasePane

}// namespace FTI.Trialmax.Panes
