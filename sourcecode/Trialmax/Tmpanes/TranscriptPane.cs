using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;
using System.Xml.XPath;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;
using FTI.Trialmax.Database;
using FTI.Trialmax.Forms;
using FTI.Trialmax.Controls;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;

namespace FTI.Trialmax.Panes
{
	/// <summary>
	/// This class implements a Trialmax pane that can be used to display
	///	transcripts and create clips based on selected text in the pane
	/// </summary>
	public class CTranscriptPane : FTI.Trialmax.Panes.CBasePane
	{
		#region Constants
		
		/// <summary>Local context menu command identifiers</summary>
		protected enum TranscriptPaneCommands
		{
			Invalid = 0,
			PreviewSelection,
			AddSelection,
			InsertSelectionBefore,
			InsertSelectionAfter,
			SetHighlighter,
			SetHighlighter1,
			SetHighlighter2,
			SetHighlighter3,
			SetHighlighter4,
			SetHighlighter5,
			SetHighlighter6,
			SetHighlighter7,
			AssignHighlighter,
			PinTranscript,
			Find,
			GoPageLine,
			Update,
			EditExtents,
			SplitBefore,
			SplitAfter,
			Exclude,
			AddObjection,
			RepeatObjection,
			AddLink,
			CopySelection,
			PrintSelection,
			SaveSelection,
			OpenProperties,
			OpenCodes,
			OpenPresentation,
		}
		
		private const string XMLKEY_DOUBLE_SPIT_BEFORE		= "DoubleSplitBefore";
		
		private const int ERROR_LOAD_DEPOSITION_EX			= (ERROR_BASE_PANE_MAX + 1);
		private const int ERROR_FILE_NOT_FOUND				= (ERROR_BASE_PANE_MAX + 2);
		private const int ERROR_ON_GRID_DBL_CLICK_EX		= (ERROR_BASE_PANE_MAX + 3);
		private const int ERROR_FILL_TRANSCRIPTS_EX			= (ERROR_BASE_PANE_MAX + 4);
		private const int ERROR_FILL_HIGHLIGHTERS_EX		= (ERROR_BASE_PANE_MAX + 5);
		private const int ERROR_TEXT_NOT_FOUND				= (ERROR_BASE_PANE_MAX + 6);
		private const int ERROR_TRANSCRIPT_NOT_FOUND		= (ERROR_BASE_PANE_MAX + 7);
		private const int ERROR_INVALID_FILESPEC			= (ERROR_BASE_PANE_MAX + 8);
		private const int ERROR_SET_TRANSCRIPT_SEL_EX		= (ERROR_BASE_PANE_MAX + 9);
		private const int ERROR_SET_HIGHLIGHTER_SEL_EX		= (ERROR_BASE_PANE_MAX + 10);
		private const int ERROR_ADD_TRANSCRIPTS_SEL_EX		= (ERROR_BASE_PANE_MAX + 11);
		private const int ERROR_SEGMENT_NOT_FOUND			= (ERROR_BASE_PANE_MAX + 12);
		private const int ERROR_CREATE_DESIGNATION_EX		= (ERROR_BASE_PANE_MAX + 13);
		private const int ERROR_ADD_DESIGNATION_EX			= (ERROR_BASE_PANE_MAX + 14);
		private const int ERROR_ADD_SELECTION_EX			= (ERROR_BASE_PANE_MAX + 15);
		private const int ERROR_ADD_SCENES_EX				= (ERROR_BASE_PANE_MAX + 16);
		private const int ERROR_PREVIEW_SELECTION_EX		= (ERROR_BASE_PANE_MAX + 17);
		private const int ERROR_ACTIVATE_EX					= (ERROR_BASE_PANE_MAX + 18);
		private const int ERROR_GO_PAGE_LINE_EX				= (ERROR_BASE_PANE_MAX + 19);
		private const int ERROR_FILL_GRID_EX				= (ERROR_BASE_PANE_MAX + 20);
		private const int ERROR_CMD_EDIT_EX					= (ERROR_BASE_PANE_MAX + 21);
		private const int ERROR_FIRE_EDIT_COMMAND_EX		= (ERROR_BASE_PANE_MAX + 22);
		private const int ERROR_ON_OBJECTIONS_ADDED_EX		= (ERROR_BASE_PANE_MAX + 23);
		private const int ERROR_ON_OBJECTIONS_DELETED_EX	= (ERROR_BASE_PANE_MAX + 24);
		private const int ERROR_ON_OBJECTIONS_UPDATED_EX	= (ERROR_BASE_PANE_MAX + 25);
		private const int ERROR_FIRE_SET_DEPOSITION_EX		= (ERROR_BASE_PANE_MAX + 26);
		private const int ERROR_ADD_LINK_EX					= (ERROR_BASE_PANE_MAX + 27);
		private const int ERROR_CREATE_XML_LINK_EX			= (ERROR_BASE_PANE_MAX + 28);
		private const int ERROR_ON_ADD_LINK_VALIDATE_EX		= (ERROR_BASE_PANE_MAX + 29);
		private const int ERROR_ON_CMD_OPEN_EX				= (ERROR_BASE_PANE_MAX + 30);

		#endregion Constants
		
		#region Private Members
		
		/// <summary>Member required by forms designer</summary>
		private System.ComponentModel.IContainer components;

		/// <summary>Background fill panel used by the toolbar manager</summary>
		private System.Windows.Forms.Panel m_ctrlFillPanel;
		
		/// <summary>Infragistics toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager m_ultraToolbarManager;
		
		/// <summary>Infragistics toolbar manager left docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTranscriptPane_Toolbars_Dock_Area_Left;
		
		/// <summary>Infragistics toolbar manager right docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTranscriptPane_Toolbars_Dock_Area_Right;
		
		/// <summary>Infragistics toolbar manager top docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTranscriptPane_Toolbars_Dock_Area_Top;
		
		/// <summary>Infragistics state button tool for pinning the transcript</summary>
		private Infragistics.Win.UltraWinToolbars.StateButtonTool m_ctrlPinTranscript = null;
		
		/// <summary>Image list used for toolbar and context menu</summary>
		private System.Windows.Forms.ImageList m_ctrlToolbarImages;
		
		/// <summary>The pane's status bar</summary>
		private Infragistics.Win.UltraWinStatusBar.UltraStatusBar m_ctrlStatusBar;
		
		/// <summary>Infragistics toolbar manager bottom docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTranscriptPane_Toolbars_Dock_Area_Bottom;

		/// <summary>TrialMax custom transcript viewer grid</summary>
		private CTmaxTransGridCtrl m_transGrid;

		/// <summary>Local flag to inhibit processing grid events</summary>
		private bool m_bIgnoreGridEvents = false;
		
		/// <summary>Local flag to inhibit processing of application transcript activations</summary>
		private bool m_bPinTranscript = false;
		
		/// <summary>Local flag to control processing of double-click events</summary>
		private bool m_bDoubleSplitBefore = true;
		
		/// <summary>Record exchange object for transcript to be activated</summary>
		private FTI.Trialmax.Database.CDxMediaRecord m_dxActivate = null;
		
		/// <summary>Primary exchange object for current deposition</summary>
		private FTI.Trialmax.Database.CDxPrimary m_dxDeposition = null;
		
		/// <summary>XML file containing the transcript text</summary>
		private FTI.Shared.Xml.CXmlDeposition m_xmlDeposition = null;
		
		/// <summary>Scene designations currently highlighted in the transcript</summary>
		private FTI.Trialmax.Database.CDxSecondaries m_dxDesignationScenes = new CDxSecondaries();
		
		/// <summary>Primary exchange object for the active script</summary>
		private FTI.Trialmax.Database.CDxPrimary m_dxScript = null;
		
		/// <summary>Secondary exchange object for the active script scene</summary>
		private FTI.Trialmax.Database.CDxSecondary m_dxScene = null;
		
		/// <summary>Secondary exchange object for newly added scene</summary>
		private FTI.Trialmax.Database.CDxSecondary m_dxAdded = null;
		
		/// <summary>Active highlighter used for creating designations</summary>
		private FTI.Trialmax.Database.CDxHighlighter m_dxHighlighter = null;

		/// <summary>Record exchange object for dragging new links</summary>
		private FTI.Trialmax.Database.CDxMediaRecord m_dxDropSource = null;

		/// <summary>Secondary exchange object for the target drop scene</summary>
		private FTI.Trialmax.Database.CDxSecondary m_dxDropScene = null;

		/// <summary>Tertiary exchange object for the target drop designation</summary>
		private FTI.Trialmax.Database.CDxTertiary m_dxDropDesignation = null;

		/// <summary>XML transcript that identifies the target drop line</summary>
		private FTI.Shared.Xml.CXmlTranscript m_xmlDropLine = null;
		
		/// <summary>Array of selected rows being previewed</summary>
		private Array m_aPreviewSelections = null;
		
		/// <summary>Designation being previewed</summary>
		private CXmlDesignation m_xmlPreviewDesignation = null;
		
		/// <summary>Current transcript line being previewed</summary>
		private int m_iPreviewRow = -1;
		
		/// <summary>The threshold (sec) used to control the display of Pause indicators</summary>
		private double m_dPauseThreshold = 0;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTranscriptPane()
		{
			//	Initialize the child controls
			InitializeComponent();

			m_tmaxEventSource.Name = "Transcript Pane";
			m_tmaxEventSource.Attach(m_transGrid.EventSource);
			
			m_ctrlPinTranscript = (StateButtonTool)GetUltraTool("PinTranscript");

			//=====================================================
			//	PLAY DEPOSITION : USE FORM DESIGNER TO PUT THE 
			//					  "OPEN" SUBMENU ON TOP IN THE
			//					  FORM'S CONTEXT MENU
			//=====================================================
		}

		/// <summary>This method is called by the application to initialize the pane</summary>
		/// <returns>true if successful</returns>
		public override bool Initialize(CXmlIni xmlINI)
		{
			//	Do the base class initialization first
			if(base.Initialize(xmlINI) == false)
				return false;

			//	Make sure we are on the correct section
			xmlINI.SetSection(m_strPaneName);

			m_bDoubleSplitBefore = xmlINI.ReadBool(XMLKEY_DOUBLE_SPIT_BEFORE, true);

			return true;

		}// public override bool Initialize()

		/// <summary>This method is called by the application when it is about to terminate</summary>
		public override void Terminate(CXmlIni xmlINI)
		{
			//	Make sure we are on the correct section
			xmlINI.SetSection(m_strPaneName);

			xmlINI.Write(XMLKEY_DOUBLE_SPIT_BEFORE, m_bDoubleSplitBefore);

			//	Do the base class processing
			base.Terminate(xmlINI);
		}

		/// <summary>This method is called by the application to get a list of items that represent the current selections for a Find command</summary>
		/// <returns>The items that represent the current selections</returns>
		public override CTmaxItems GetCmdFindItems()
		{
			CTmaxItems tmaxItems = null;
			
			if(m_dxDeposition != null)
			{
				tmaxItems = new CTmaxItems();
				tmaxItems.Add(new CTmaxItem(m_dxDeposition));
			}
			
			return tmaxItems;
		
		}// public override CTmaxItems GetCmdFindItems()
		
		/// <summary>This method is called by the application to activate the specified item</summary>
		/// <param name="tmaxItem">The item to be activated</param>
		/// <param name="ePane">The pane requesting activation</param>
		/// <returns>true if successful</returns>
		public override bool Activate(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
			CDxMediaRecord	dxRecord = null;
			CDxPrimary		dxDeposition = null;
			
			//	Don't bother if not valid media
			if(tmaxItem.GetMediaRecord() == null) return false;
			
			//	Always keep track of the last scene/script selection
			//	so that we know where to insert new designations
			if(tmaxItem.MediaType == TmaxMediaTypes.Script)
			{  
				m_dxScript = (CDxPrimary)tmaxItem.GetMediaRecord();
				m_dxScene  = null;
			}
			else if(tmaxItem.MediaType == TmaxMediaTypes.Scene)
			{
				m_dxScene  = (CDxSecondary)tmaxItem.GetMediaRecord();
				m_dxScript = m_dxScene.Primary;
				Debug.Assert(m_dxScript != null);
			}
			else if(tmaxItem.MediaType == TmaxMediaTypes.Link)
			{
				Debug.Assert(tmaxItem.ParentItem != null);
				Debug.Assert(tmaxItem.ParentItem.GetMediaRecord() != null);
				Debug.Assert(tmaxItem.ParentItem.MediaType == TmaxMediaTypes.Scene);
				
				if((tmaxItem.ParentItem != null) && (tmaxItem.ParentItem.GetMediaRecord() != null))
				{
					m_dxScene = (CDxSecondary)(tmaxItem.ParentItem.GetMediaRecord());
					Debug.Assert(m_dxScene != null);
					if(m_dxScene != null)
						m_dxScript = m_dxScene.Primary;
					Debug.Assert(m_dxScript != null);
				}
			}

			//	Go ahead and activate this record if the pane is active
			if(PaneVisible == true)
			{
				try
				{
					SetRecord((CDxMediaRecord)tmaxItem.GetMediaRecord(), false);
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "Activate", m_tmaxErrorBuilder.Message(ERROR_ACTIVATE_EX, tmaxItem.GetMediaRecord().GetText(TmaxTextModes.Barcode)), Ex);
				}
			}
			else
			{
				dxRecord = (CDxMediaRecord)tmaxItem.GetMediaRecord();
				
				//	Switch to the parent record if this is a link
				if(dxRecord.MediaType == TmaxMediaTypes.Link)
					dxRecord = dxRecord.GetParent();
				Debug.Assert(dxRecord != null);
				if(dxRecord == null) return false;
			
				//	Does this item have an associated deposition?
				if((dxDeposition = GetDeposition(dxRecord)) != null)
				{
					m_dxActivate = dxRecord;

					//	Is the transcript pinned?
					if(m_bPinTranscript == false)
					{
						//	Even though we are not visible we still have to 
						//	notify the system because the objections pane is
						//	slaved to this pane
						FireSetDeposition(dxDeposition);
					}
					
				}
				else
				{
					m_dxActivate = null;
				}

			}// if(PaneVisible == true)
			
			return true;
						
		}// public override bool Activate(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		/// <summary>This method handles all Hotkey notifications from the application</summary>
		/// <param name="eHotkey">The enumerated hotkey</param>
		/// <returns>true if processed by the pane</returns>
		public override bool OnHotkey(TmaxHotkeys eHotkey)
		{
			TranscriptPaneCommands	eCommand = TranscriptPaneCommands.Invalid;
			long					lSelections = 0;
			long					lStartPL = 0;
			long					lStopPL = 0;
			
			//	Get the current number of row selections
			lSelections = m_transGrid.GetSelectionRange(ref lStartPL, ref lStopPL);

			//	Which hotkey has been pressed?
			switch(eHotkey)
			{
				case TmaxHotkeys.Find:
				
					eCommand = TranscriptPaneCommands.Find;
					break;
					
				case TmaxHotkeys.GoTo:

					eCommand = TranscriptPaneCommands.GoPageLine;
					break;
					
				case TmaxHotkeys.AddToScript:

					eCommand = TranscriptPaneCommands.AddSelection;
					break;

				case TmaxHotkeys.AddObjection:

					eCommand = TranscriptPaneCommands.AddObjection;
					break;

				case TmaxHotkeys.RepeatObjection:

					eCommand = TranscriptPaneCommands.RepeatObjection;
					break;

				case TmaxHotkeys.OpenPresentation:

					eCommand = TranscriptPaneCommands.OpenPresentation;
					break;

				default:
				
					break;
			}
		
			//	Did this hotkey translate to a command?
			if(eCommand != TranscriptPaneCommands.Invalid)
			{
				//	Is this command enabled
				if(GetCommandEnabled(eCommand, lSelections, lStartPL, lStopPL) == true)
				{
					OnCommand(eCommand);
				}

			}// if(eCommand != TranscriptPaneCommands.Invalid)
			
			return (eCommand != TranscriptPaneCommands.Invalid);
			
		}// public override bool OnHotkey(TmaxHotkeys eHotkey)
		
		/// <summary>This method is called by the application when a new search result has been activated</summary>
		/// <param name="tmaxResult">The search result to be activated</param>
		public override void OnActivateResult(FTI.Shared.Trialmax.CTmaxSearchResult tmaxResult)
		{
			CTmaxItem tmaxItem = null;
			
			//	Don't bother if not active
			if(PaneVisible == false) return;
			
			//	Is this result associated with a record?
			if(tmaxResult.IScene != null)
				tmaxItem = new CTmaxItem(tmaxResult.IScene);
			else if(tmaxResult.IDeposition != null)
				tmaxItem = new CTmaxItem(tmaxResult.IDeposition);
				
			//	Should we activate the record
			if(tmaxItem != null)
			{
				if(Activate(tmaxItem, TmaxAppPanes.Transcripts) == true)
				{
					m_transGrid.SetSelection(tmaxResult.PL, true, true);
				}
				
			}
		
		}// public override void OnActivateResult(FTI.Shared.Trialmax.CTmaxSearchResult tmaxResult)
		
		/// <summary>This method is called by the application when it adds new media to the database</summary>
		/// <param name="tmaxParent">TrialMax event item that identifies the parent record</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		{
			Debug.Assert(tmaxParent != null);
			Debug.Assert(tmaxChildren != null);
			
			//	All new media should be in the child collection
			Debug.Assert(tmaxChildren.Count > 0);
			if(tmaxChildren.Count == 0) return;
		
			//	Are we adding new primary media?
			if(tmaxParent.MediaLevel == TmaxMediaLevels.None)
			{ 
				foreach(CTmaxItem tmaxChild in tmaxChildren)
				{
					if(tmaxChild.MediaType == TmaxMediaTypes.Deposition)
					{
						//	Add a selection to the transcripts list box
						if(tmaxChild.GetMediaRecord() != null)
							AddTranscriptsSelection((CDxPrimary)tmaxChild.GetMediaRecord());
					}
				
				}

			}
			//	New scenes?
			else if(tmaxParent.MediaType == TmaxMediaTypes.Script)
			{
				//	Are we adding scenes to the active script?
				if((m_dxScript != null) &&
				   (ReferenceEquals(m_dxScript, tmaxParent.GetMediaRecord()) == true))
				{
					//	Check each of the new children
					foreach(CTmaxItem tmaxChild in tmaxChildren)
					{
						if(tmaxChild.GetMediaRecord() != null)
						{
							Highlight((CDxSecondary)tmaxChild.GetMediaRecord());
				
							//	We will use this member locally to activate the
							//	first scene added to the script
							if(m_dxAdded == null) 
								m_dxAdded = (CDxSecondary)tmaxChild.GetMediaRecord();
						}
							
					}// foreach(CTmaxItem tmaxChild in tmaxChildren)
				
				}
				
			}// else if(tmaxParent.MediaType == TmaxMediaTypes.Script)
			
		}// public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		
		/// <summary>This method is called by the application to when the item has been updated by the user</summary>
		/// <param name="tmaxItem">The item that has been updated</param>
		public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{
			Debug.Assert(tmaxItem != null);
			if(tmaxItem == null) return;
			if(tmaxItem.GetMediaRecord() == null) return;
				
			//	What type of media has been updated?
			switch(tmaxItem.GetMediaRecord().GetMediaType())
			{
				case TmaxMediaTypes.Designation:
				
					//	Are we currently highlighting this designation?
					if((m_dxDesignationScenes != null) && (m_dxDesignationScenes.Count > 0))
					{
						foreach(CDxSecondary O in m_dxDesignationScenes)
						{
							if(O.GetSource() != null)
							{
								if(ReferenceEquals(O.GetSource(), tmaxItem.GetMediaRecord()) == true)
								{
									Erase(true);
									Highlight(m_dxScript);
									
									//	Is this the active scene
									if(ReferenceEquals(O, m_dxScene) == true)
									{
										//	Note:	We used to ensure that the designation was visible
										//			when we did this but the users complained because
										//			it would scroll to the top of the designation whenever they
										//			made a change even if they had scrolled to the bottom of the designation
										SetSelectedIndicators(false);
									}
										
									break;
								}
							}
							
						}// foreach(CDxSecondary O in m_dxDesignationScenes)
					
					}// if((m_dxDesignationScenes != null) && (m_dxDesignationScenes.Count > 0))
					
					break;
					
				default:
				
					break;
					
			}
					
		}// public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		/// <summary>This method is called by the application when new media gets registered</summary>
		/// <param name="tmaxFolder">The source folder containing the new media</param>
		/// <param name="ePane">The pane that accepted the registration request</param>
		public override void OnRegistered(CTmaxSourceFolder tmaxSource, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
			//	Make sure we have the required objects
			if(tmaxSource == null) return;

			//	Is this a registered primary media object?
			if(tmaxSource.IPrimary != null)
			{
				//	Is this a deposition?
				if(tmaxSource.IPrimary.GetMediaType() == TmaxMediaTypes.Deposition)
					AddTranscriptsSelection((CDxPrimary)tmaxSource.IPrimary);
					
			}// if(tmaxSource.IPrimary != null)
			
			//	Now add each of this folder's subfolders
			if((tmaxSource.SubFolders != null) && (tmaxSource.SubFolders.Count > 0))
			{
				foreach(CTmaxSourceFolder tmaxFolder in tmaxSource.SubFolders)
				{
					OnRegistered(tmaxFolder, ePane);
				}
			
			}// if((tmaxSource.SubFolders != null) && (tmaxSource.SubFolders.Count > 0))
		
		}// OnRegistered(CTmaxSourceFolder tmaxSource, int iPane)

		/// <summary>This function is called when the the user has closed the Case Options form</summary>
		/// <param name="bCancelled">true if the user cancelled the operation</param>
		public override void OnAfterSetCaseOptions(bool bCancelled)
		{
			//	Don't bother if cancelled by the user
			if(bCancelled == false)
			{
				try
				{
					//	Did the user change the Pause threshold?
					if((double)m_tmaxStationOptions.PauseThreshold != m_dPauseThreshold)
					{
						//	Update the threshold and indicators
						SetPauseThreshold();
					}

				}
				catch
				{
				}

			}// if(bCancelled == false)

		}// public override void OnAfterSetCaseOptions(bool bCancelled)

		/// <summary>This method is called by the application to when the item gets deleted</summary>
		/// <param name="tmaxItem">The items that have been deleted</param>
		public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			foreach(CTmaxItem O in tmaxItems)
			{
				try
				{
					//	Don't bother if the user has deleted pick list items
					if(O.DataType == TmaxDataTypes.PickItem)
						break;
						
					OnDeleted(O);
				}
				catch
				{
				}
				
			}

		}// public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)

		/// <summary>This method is called by the application when it adds new media to the database</summary>
		/// <param name="tmaxParent">TrialMax event item that identifies the parent record</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		public override void OnObjectionsAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		{
			try
			{
				if(m_dxDeposition != null)
				{
					//	New records are stored in the child collection
					if((tmaxChildren != null) && (tmaxChildren.Count > 0))
					{
						foreach(CTmaxItem O in tmaxChildren)
						{
							if((O.Objection != null) && (ReferenceEquals(O.Objection.ICaseDeposition, m_dxDeposition) == true))
							{
								m_transGrid.AddObjection(O.Objection);
							}

						}// foreach(CTmaxItem O in tmaxChildren)

					}// if((tmaxChildren != null) && (tmaxChildren.Count > 0))

				}// if(m_dxDeposition != null)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnObjectionsAdded", m_tmaxErrorBuilder.Message(ERROR_ON_OBJECTIONS_ADDED_EX), Ex);
			}

		}// public override void OnObjectionsAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)

		/// <summary>This method is called by the application when objections have been deleted</summary>
		/// <param name="tmaxItems">The objections that have been deleted</param>
		public override void OnObjectionsDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			try
			{
				if(m_dxDeposition != null)
				{
					foreach(CTmaxItem O in tmaxItems)
					{
						if((O.Objection != null) && (String.Compare(O.Objection.Deposition, m_dxDeposition.MediaId, true) == 0))
						{
							m_transGrid.RemoveObjection(O.Objection);
						}

					}// foreach(CTmaxItem O in tmaxItems)

				}// if(m_dxDeposition != null)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnObjectionsDeleted", m_tmaxErrorBuilder.Message(ERROR_ON_OBJECTIONS_DELETED_EX), Ex);
			}

		}// public override void OnObjectionsDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)

		/// <summary>This method is called by the application when multiple records have been updated in an operation</summary>
		/// <param name="tmaxItems">The items that have been updated</param>
		public override void OnObjectionsUpdated(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			try
			{
				if(m_dxDeposition != null)
				{
					foreach(CTmaxItem O in tmaxItems)
					{
						if((O.Objection != null) && (String.Compare(O.Objection.Deposition, m_dxDeposition.MediaId, true) == 0))
						{
							m_transGrid.UpdateObjection(O.Objection);
						}

					}// foreach(CTmaxItem O in tmaxItems)

				}// if(m_dxDeposition != null)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnObjectionsUpdated", m_tmaxErrorBuilder.Message(ERROR_ON_OBJECTIONS_UPDATED_EX), Ex);
			}

		}// public override void OnObjectionsUpdated(FTI.Shared.Trialmax.CTmaxItems tmaxItems)

		/// <summary>This method is called by the application when objections has been selected by the user</summary>
		/// <param name="tmaxItem">The objection that has been selected</param>
		/// <param name="ePane">The pane that fired the selection event</param>
		public override void OnObjectionSelected(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
			if(tmaxItem.Objection != null)
			{
				m_transGrid.SelectObjection(tmaxItem.Objection);
			
			}

		}// public override void OnObjectionSelected(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)

		#endregion Public Methods

		#region Private Methods
		
		/// <summary>This method is called when children of the specified item have been deleted</summary>
		/// <param name="tmaxItem">The parent item</param>
		private void OnDeleted(CTmaxItem tmaxParent)
		{
			//	Are we deleting primary records?
			if(tmaxParent.IPrimary == null)
			{
				foreach(CTmaxItem O in tmaxParent.SubItems)
				{
					//	Deposition?
					if(O.MediaType == TmaxMediaTypes.Deposition)
					{
						//	Remove this deposition from the list box
						RemoveTranscriptsSelection((CDxPrimary)O.IPrimary);
							
						//	Is this our active Deposition?
						if(ReferenceEquals(O.IPrimary, m_dxDeposition) == true)
						{
							Unload();
						}
					}
					
					//	Script?
					else if(O.MediaType == TmaxMediaTypes.Script)
					{
						//	Is this our active Script?
						if(ReferenceEquals(O.IPrimary, m_dxScript) == true)
						{
							//	Erase all the highlights
							Erase(true);
							
							//	No longer have an active script
							m_dxScript = null;
							m_dxScene = null;
						}
					
					}
					
				}// foreach(CTmaxItem O in tmaxParent.SubItems)
				
			}
			//	Are we deleting scenes?
			else if(tmaxParent.MediaType == TmaxMediaTypes.Script)
			{
				//	Are we deleting scenes in the active script
				if(ReferenceEquals(m_dxScript, tmaxParent.GetMediaRecord()) == true)
				{
					Erase(true);
					Highlight(m_dxScript);

					//	Has the active scene been deleted?
					if((m_dxScene != null) && (m_tmaxDatabase.IsValidRecord(m_dxScene) == false))
					{
						m_dxScene = null;
						SetSelectedIndicators(false);
					}
						
				}
			}
				
		}// public override void OnDeleted(CTmaxItem tmaxParent)
		
		/// <summary>Toolbar event handler for PreviewSelection command</summary>
		private void OnCmdPreviewSelection()
		{
			PreviewSelection();
			
		}// private void OnCmdPreviewSelection()
		
		/// <summary>Toolbar event handler for EditExtents command</summary>
		private void OnCmdEditDesignation(TranscriptPaneCommands eCommand)
		{
			TmaxDesignationEditMethods	eMethod = TmaxDesignationEditMethods.Unknown;
			CTmaxParameters				tmaxParameters = null;
			CTmaxItem					tmaxItem = null;
			long						lStartPL = 0;
			long						lStopPL = 0;
			
			//	Get the appropriate edit method
			switch(eCommand)
			{
				case TranscriptPaneCommands.EditExtents:	eMethod = TmaxDesignationEditMethods.Extents;
															break;
				case TranscriptPaneCommands.Exclude:		eMethod = TmaxDesignationEditMethods.Exclude;
															break;
				case TranscriptPaneCommands.SplitBefore:	eMethod = TmaxDesignationEditMethods.SplitBefore;
															break;
				case TranscriptPaneCommands.SplitAfter:		eMethod = TmaxDesignationEditMethods.SplitAfter;
															break;
				default:									Debug.Assert(false, "Invalid Edit Command: " + eCommand.ToString());
															return;
		
			}// switch(eCommand)
			
			//	Get the active designation
			//
			//	NOTE:	We use this call to verify that the active scene is bound
			//			to a designation built from the acive deposition
			if(GetDesignation() == null)
			{
				MessageBox.Show("You must select a valid designation", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			
			//	Get the new range
			if(m_transGrid.GetSelectionRange(ref lStartPL, ref lStopPL) == 0)
			{
				MessageBox.Show("You must select some text within the active designation", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			
			try
			{
				//	Allocate and initialize the event item
				//
				//	NOTE:	The database requires the active scene (instead of the designation)
				tmaxItem = new CTmaxItem(m_dxScene);
				
				//	Populate the parameters collection
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.EditMethod, (int)eMethod);
				tmaxParameters.Add(TmaxCommandParameters.StartPL, lStartPL);
				tmaxParameters.Add(TmaxCommandParameters.StopPL, lStopPL);
				
				//	Fire the command event
				if(FireCommand(TmaxCommands.EditDesignation, tmaxItem, tmaxParameters) == true)
					m_transGrid.SetSelections(null);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdEditDesignation", m_tmaxErrorBuilder.Message(ERROR_CMD_EDIT_EX, eMethod.ToString()), Ex);
			}
			
		}// private void OnCmdEditDesignation(TmaxDesignationEditMethods eMethod)
		
		/// <summary>Toolbar event handler for SaveSelection command</summary>
		/// <param name="bInsert">true to insert the current selection</param>
		/// <param name="bBefore">true to save before the current scene</param>
		private void OnCmdAddSelection(bool bInsert, bool bBefore)
		{
			Debug.Assert(m_dxScript != null);
		
			AddSelection(bInsert, bBefore);
			
			//	Make sure this pane retains the focus
			this.Select();
			
		}// private void OnCmdAddSelection(bool bInsert, bool bBefore)

		/// <summary>Toolbar event handler for AddObjection command</summary>
		/// <param name="bRepeat">true to repeat the last used argument</param>
		private void OnCmdAddObjection(bool bRepeat)
		{
			CTmaxObjection	tmaxObjection = null;
			long			lFirstPL = 0;
			long			lLastPL = 0;
			
			//	Must have an active deposition
			if(m_dxDeposition != null)
			{
				tmaxObjection = new CTmaxObjection();
				tmaxObjection.ICaseDeposition = m_dxDeposition;

				//	Get the current selections
				if(m_transGrid.GetSelectionRange(ref lFirstPL, ref lLastPL) > 0)
				{
					tmaxObjection.FirstPL = lFirstPL;
					tmaxObjection.LastPL = lLastPL;
				}

				if(AddObjection(tmaxObjection, (lFirstPL == 0), bRepeat) != null)
				{
					//	Clear the current selection
					m_transGrid.SetSelection(null, true);
				}				
			
			}// if(m_dxDeposition != null)

		}// private void OnCmdAddObjection()

		/// <summary>Toolbar event handler for AddLink command</summary>
		private void OnCmdAddLink()
		{
			CFAddLink		addLink = null;
			CDxTertiary		dxDesignation = null;
			CXmlTranscript	xmlLine = null;
			CXmlLink		xmlLink = null;

			Debug.Assert(m_tmaxDatabase != null);

			try
			{
				//	Get the parent designation and line of text
				if((dxDesignation = GetDesignation()) != null)
				{
					if((xmlLine = GetAddLinkLine(dxDesignation)) != null)
					{
						//	Create the new link to be configured by the user
						xmlLink = CreateXmlLink(xmlLine, dxDesignation, null);

					}// if((xmlLine = GetAddLinkLine(dxDesignation)) != null)

				}// if((dxDesignation = GetDesignation()) != null)
					
				//	Prompt the user for the link options
				if(xmlLink != null)
				{
					addLink = new CFAddLink();
					m_tmaxEventSource.Attach(addLink.EventSource);
					
					addLink.XmlTranscript = xmlLine;
					addLink.XmlLink = xmlLink;
					
					if(this.PresentationOptions != null)
						addLink.ClassicLinks = this.PresentationOptions.ClassicLinks;
					else
						addLink.ClassicLinks = false;

					addLink.ValidateBarcode += new CFAddLink.ValidateBarcodeHandler(this.OnAddLinkValidate);

					//	Show the form and allow the user to set the options
					if(addLink.ShowDialog() == DialogResult.OK)
					{
						//	Add the link to the database
						AddLink(dxDesignation, xmlLink);
					}

				}//	if(xmlLink != null)

			}
			catch
			{
			}

		}// private void OnCmdAddLink()

		/// <summary>This method handles the event fired when the user clicks on one of the selections in the Open submenu</summary>
		/// <param name="eCommand">The command that identifies the target destination</param>
		private void OnCmdOpen(TranscriptPaneCommands eCommand)
		{
			CTmaxParameters tmaxParameters = null;
			CTmaxItem		tmaxItem = null;
			CXmlTranscripts xmlSelections = null;

			try
			{
				if((tmaxItem = GetCommandItem()) != null)
				{
					//	Create the required parameters for the event
					tmaxParameters = new CTmaxParameters();

					//	Add the parameter to identify the target destination
					switch(eCommand)
					{
						case TranscriptPaneCommands.OpenCodes:
						
							tmaxParameters.Add(TmaxCommandParameters.Codes, true);
							break;
							
						case TranscriptPaneCommands.OpenProperties:
						
							tmaxParameters.Add(TmaxCommandParameters.Properties, true);
							break;
							
						case TranscriptPaneCommands.OpenPresentation:
						
							tmaxParameters.Add(TmaxCommandParameters.Presentation, true);
							
							//	Get the selected lines
							if((xmlSelections = m_transGrid.GetSelections()) != null)
							{
								//	Search for the first line of synchronized text
								foreach(CXmlTranscript O in xmlSelections)
								{
									if(O.Synchronized == true)
									{
										tmaxParameters.Add(TmaxCommandParameters.StartPL, O.PL);
										break;
									}

								}// foreach(CXmlTranscript O in xmlSelections)

							}// if((xmlSelections = m_transGrid.GetSelections()) != null)
							
							break;
							
						default:
						
							Debug.Assert(false, "Unhandled command: " + eCommand.ToString());
							break;
							
					}// switch(eCommand)
					
					if(tmaxParameters.Count > 0)
						FireCommand(TmaxCommands.Open, tmaxItem, tmaxParameters);

				}// if((tmaxItem = GetCommandItem()) != null)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdOpen", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_OPEN_EX, eCommand), Ex);
			}

		}// private void OnCmdOpen(TmaxCommandParameters eDestination)

		/// <summary>Toolbar event handler for CopySelection command</summary>
		private void OnCmdCopySelection()
		{
			Debug.Assert(m_tmaxDatabase != null);

			try
			{
				m_transGrid.CopySelections();
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "CopySelection", Ex);
			}

		}// private void OnCmdCopySelection()

		/// <summary>Toolbar event handler for PrintSelection command</summary>
		private void OnCmdPrintSelection()
		{
			Debug.Assert(m_tmaxDatabase != null);

			try
			{
				m_transGrid.PrintSelections();
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "PrintSelection", Ex);
			}

		}// private void OnCmdPrintSelection()

		/// <summary>Toolbar event handler for SaveSelection command</summary>
		private void OnCmdSaveSelection()
		{
			Debug.Assert(m_tmaxDatabase != null);

			try
			{
				m_transGrid.SaveSelections("");
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SaveSelection", Ex);
			}

		}// private void OnCmdSaveSelection()

		/// <summary>Toolbar event handler for PinTranscript command</summary>
		private void OnCmdPinTranscript()
		{
			//	Toggle the state of the option
			m_bPinTranscript = !m_bPinTranscript;
			
			//	Update the status bar
			SetStatusText();
		}
		
		/// <summary>Toolbar event handler for Update command</summary>
		private void OnCmdUpdate()
		{
			CTmaxItem		tmaxItem = null;
			CTmaxParameters	tmaxParameters = null;
			
			if(m_dxDeposition != null)
			{
				tmaxItem = new CTmaxItem(m_dxDeposition);
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.UpdateFromFile, true);
				
				if(FireCommand(TmaxCommands.Update, tmaxItem, tmaxParameters) == true)
				{
					if(tmaxItem.ReturnItem != null)
					{
						//	Force a refresh of the grid
						m_dxDeposition = null;
						Activate(tmaxItem.ReturnItem, TmaxAppPanes.Transcripts);						
					}
					
				}
			
			}
			
		}// private void OnCmdUpdate()
		
		/// <summary>Toolbar event handler for GoPageLine command</summary>
		private void OnCmdGoPageLine()
		{
			CFGoTranscript goTranscript = null;
			
			//	Command should be disabled if no deposition available
			Debug.Assert(m_dxDeposition != null);
			if(m_dxDeposition == null) return;
			
			try
			{
				//	Allocate a new form
				goTranscript = new CFGoTranscript();
				
				//	Set the form properties
				goTranscript.TranscriptName = m_dxDeposition.Name;
				goTranscript.FirstPL = m_dxDeposition.Transcript.FirstPL;
				goTranscript.LastPL = m_dxDeposition.Transcript.LastPL;
				goTranscript.LinesPerPage = m_dxDeposition.Transcript.LinesPerPage;
				
				//	Show the form
				if(goTranscript.ShowDialog() == DialogResult.OK)
				{
					//	Select the user specified page/line
					m_transGrid.SetSelection(goTranscript.GoPL, true, false);
				}
				
				goTranscript.Dispose();
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdGoPageLine", m_tmaxErrorBuilder.Message(ERROR_GO_PAGE_LINE_EX), Ex);
			}
			
		}// private void OnCmdGoPageLine()
		
		/// <summary>Toolbar event handler for Find command</summary>
		private void OnCmdFind()
		{
			CTmaxItems tmaxItems = null;
			
			try
			{
				if((tmaxItems = GetCmdFindItems()) != null)
					FireCommand(TmaxCommands.Find, tmaxItems);
				else
					FireCommand(TmaxCommands.Find);
			}
			catch
			{
			}
			
		}// private void OnCmdFind()
		
		/// <summary>Toolbar event handler for SetHighlighter command</summary>
		/// <param name="iCommand">The command identifier</param>
		private void OnCmdSetHighlighter(int iCommand)
		{
			int iIndex = 0;
			
			if(m_tmaxDatabase == null) return;
			if(m_tmaxDatabase.Highlighters == null) return;
			
			iIndex = (iCommand - (int)TranscriptPaneCommands.SetHighlighter1);
			
			if(iIndex < 0) return;
			if(iIndex >= m_tmaxDatabase.Highlighters.Count) return;
			
			if(m_tmaxDatabase.Highlighters[iIndex] != null)
			{
				m_dxHighlighter = m_tmaxDatabase.Highlighters[iIndex];
				SetHighlightersSelection(m_dxHighlighter);
			}		
		}
		
		/// <summary>Toolbar event handler for AssignHighlighters command</summary>
		private void OnCmdAssignHighlighter()
		{
			CFSetHighlighters setHighlighters = null;
			
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxDatabase.Highlighters != null);
			
			try
			{
				setHighlighters = new CFSetHighlighters();
				
				setHighlighters.Database = m_tmaxDatabase;
				
				if(setHighlighters.ShowDialog() == DialogResult.OK)
				{
					//	Refresh the highlighters combobox
					FillHighlighters();
						
					//	Redraw the highlights if we have an active script
					if(m_dxScript != null)
					{
						Erase(false);
						Highlight(m_dxScript);
					}
					
					//	Make sure the grid has the correct objection colors
					m_transGrid.DefendantColor = m_tmaxDatabase.GetObjectionsColor(TmaxHighlighterGroups.Defendant);
					m_transGrid.PlaintiffColor = m_tmaxDatabase.GetObjectionsColor(TmaxHighlighterGroups.Plaintiff);

				}
			}
			catch
			{
			}
					
		}// private void OnCmdAssignHighlighter()
		
		/// <summary>This method is called to handle a TrialMax video playback (TmxVideoEvent) event</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">event arguments</param>
		private void OnTmaxVideoCtrlEvent(object sender, CTmaxVideoCtrlEventArgs e)
		{
			//	Which event?
			switch(e.EventId)
			{
				case TmaxVideoCtrlEvents.PlayerTranscriptChanged:
				
					OnPreviewTranscriptChanged(e.XmlDesignation, e.TranscriptIndex);
					break;

				case TmaxVideoCtrlEvents.PlayerPositionChanged:
				case TmaxVideoCtrlEvents.PlayerStateChanged:
				case TmaxVideoCtrlEvents.SetMode:
				case TmaxVideoCtrlEvents.SetPreviewPeriod:
				case TmaxVideoCtrlEvents.Apply:
				case TmaxVideoCtrlEvents.QueryPlayerPosition:
				default:
				
					break;
					
			}
			
		}// private virtual void OnTmaxVideoCtrlEvent(object sender, CTmaxVideoCtrlEventArgs e)
	
		/// <summary>This method handles events fired by the preview form when it changes the transcript text</summary>
		/// <param name="xmlDesignation">The designation being previewed</param>
		///	<param name="iTranscript">The index of the active transcript</param>
		private void OnPreviewTranscriptChanged(CXmlDesignation xmlDesignation, int iTranscript)
		{
			CTmaxTransGridRow tgRow = null;
			
			Debug.Assert(xmlDesignation != null);
			Debug.Assert(ReferenceEquals(m_xmlPreviewDesignation,xmlDesignation) == true);
			Debug.Assert(m_aPreviewSelections != null);
			Debug.Assert(m_xmlPreviewDesignation.Transcripts != null);
			
			if(xmlDesignation == null) return;
			if(ReferenceEquals(m_xmlPreviewDesignation,xmlDesignation) == false) return;
			if(m_aPreviewSelections == null) return;
			if(m_xmlPreviewDesignation.Transcripts == null) return;
			
			//	Clear the current row if necessary
			if(m_iPreviewRow >= 0)
			{
				m_transGrid.SetSelection(null, true);
				m_iPreviewRow = -1;
			}
			
			//	Stop here if transcript is out of range
			if((iTranscript < 0) || (iTranscript > m_aPreviewSelections.GetUpperBound(0)))
				return;
				
			//	Get the row object associated with the specified transcript
			if((tgRow = (CTmaxTransGridRow)m_aPreviewSelections.GetValue(iTranscript)) == null)
				return;
				
			//	Select this row
			if(tgRow.GetGridIndex() >= 0)
			{
				m_iPreviewRow = tgRow.GetGridIndex();
				m_transGrid.SetSelection(tgRow, true);
			}
		
		}// private void OnPreviewTranscriptChanged(object sender, CXmlDesignation xmlDesignation, int iTranscript)
		
		/// <summary>This method is called to unload the viewer</summary>
		private void Unload()
		{
			try
			{
				if((m_transGrid != null) && (m_transGrid.IsDisposed == false))
				{
					m_transGrid.Visible = false;
					m_transGrid.Clear();
				}
				
			}
			catch
			{
			}
			finally
			{				
				//	Reset the local members
				m_dxDeposition = null;
				m_dxDesignationScenes.Clear();
				m_dxDesignationScenes.Primary = null;
				m_bPinTranscript = false;
				
				if(m_xmlDeposition != null)
				{
					m_xmlDeposition.Clear();
					m_xmlDeposition = null;
				}
				
				if(m_ctrlPinTranscript != null)
					m_ctrlPinTranscript.Checked = false;

				//	Make sure the application is in sync
				FireSetDeposition(m_dxDeposition);
			}
			
		}// private void Unload()

		/// <summary>
		/// This method is called internally to convert the key passed in
		///	an Infragistics event to its associated command enumeration
		/// </summary>
		/// <param name="strKey">The Infragistic key identifier</param>
		/// <returns>The associated transcript pane command</returns>
		private TranscriptPaneCommands GetCommand(string strKey)
		{
			try
			{
				Array aCommands = Enum.GetValues(typeof(TranscriptPaneCommands));
				
				foreach(TranscriptPaneCommands eCommand in aCommands)
				{
					if(eCommand.ToString() == strKey)
						return eCommand;
				}
				
			}
			catch
			{
			}
			
			return TranscriptPaneCommands.Invalid;
		}
		
		/// <summary>This method is called to determine if the specified command should be enabled</summary>
		/// <param name="eCommand">The transcript pane command enumeration</param>
		/// <param name="lSelections">The number of lines in the current selection</param>
		/// <param name="lSelectedStart">The composite PL value of the first selected line</param>
		/// <param name="lSelectedStop">The composite PL value of the last selected line</param>
		/// <returns>true if command should be enabled</returns>
		private bool GetCommandEnabled(TranscriptPaneCommands eCommand, long lSelections, long lSelectedStart, long lSelectedStop)
		{
			//	Do we have an active database?
			if((m_tmaxDatabase == null) || (m_tmaxDatabase.Primaries == null))
				return false;
				
			//	What is the command?
			switch(eCommand)
			{
				case TranscriptPaneCommands.PreviewSelection:
				case TranscriptPaneCommands.CopySelection:
				case TranscriptPaneCommands.SaveSelection:
				case TranscriptPaneCommands.PrintSelection:
				
					return (lSelections > 0);
				
				case TranscriptPaneCommands.AddSelection:
				
					if(lSelections <= 0) return false;
					if(m_dxScript == null) return false;
					
					//	All conditions have been met
					return true;

				case TranscriptPaneCommands.InsertSelectionBefore:
				case TranscriptPaneCommands.InsertSelectionAfter:
				
					if(lSelections <= 0) return false;
					if(m_dxScript == null) return false;
					if(m_dxScene == null) return false;
					
					//	All conditions have been met
					return true;
					
				case TranscriptPaneCommands.Exclude:
				case TranscriptPaneCommands.SplitBefore:
				case TranscriptPaneCommands.SplitAfter:
				case TranscriptPaneCommands.EditExtents:
				
					return GetEditCommandEnabled(eCommand, lSelections, lSelectedStart, lSelectedStop);
					
				case TranscriptPaneCommands.SetHighlighter:
				case TranscriptPaneCommands.SetHighlighter1:
				case TranscriptPaneCommands.SetHighlighter2:
				case TranscriptPaneCommands.SetHighlighter3:
				case TranscriptPaneCommands.SetHighlighter4:
				case TranscriptPaneCommands.SetHighlighter5:
				case TranscriptPaneCommands.SetHighlighter6:
				case TranscriptPaneCommands.SetHighlighter7:
				case TranscriptPaneCommands.AssignHighlighter:
				
					return (m_tmaxDatabase.Highlighters != null);
						
				case TranscriptPaneCommands.PinTranscript:
				case TranscriptPaneCommands.GoPageLine:
				case TranscriptPaneCommands.Update:
				case TranscriptPaneCommands.AddObjection:
				case TranscriptPaneCommands.RepeatObjection:
				
					return (m_dxDeposition != null);

				case TranscriptPaneCommands.OpenProperties:
				case TranscriptPaneCommands.OpenCodes:
				case TranscriptPaneCommands.OpenPresentation:

				//=====================================================
				//	PLAY DEPOSITION : RESTORE THIS CODE
				//=====================================================
				// return (m_dxDeposition != null);
				return false;
				
				case TranscriptPaneCommands.AddLink:

					if(m_dxDeposition == null) return false;
					if(GetAddLinkLine(null) == null) return false;
					
					return true;

				case TranscriptPaneCommands.Find:
				
					//	All we need is an open database
					return true;
					
				default:
				
					break;
			}	
			
			return false;
		
		}// private virtual bool GetCommandEnabled(TranscriptPaneCommands eCommand, int lSelections)
		
		/// <summary>This method is called to get the shortcut key for the specified command</summary>
		/// <param name="eCommand">The tree pane command enumeration</param>
		/// <returns>The shortcut key if there is one</returns>
		private Shortcut GetCommandShortcut(TranscriptPaneCommands eCommand)
		{
			switch(eCommand)
			{
				case TranscriptPaneCommands.Find:
				
					return Shortcut.CtrlF;

				case TranscriptPaneCommands.AddObjection:

					return Shortcut.CtrlJ;

				case TranscriptPaneCommands.OpenPresentation:

					return Shortcut.F5;

				default:
				
					return Shortcut.None;
			
			}// switch(eCommand)

		}// protected virtual Shortcut GetCommandShortcut(TranscriptPaneCommands eCommand)

		/// <summary>Called to get an event item to represent the active transcript</summary>
		private CTmaxItem GetCommandItem()
		{
			CTmaxItem tmaxItem = null;

			try
			{
				if(m_dxDeposition != null)
				{
					tmaxItem = new CTmaxItem(m_dxDeposition);

				}// if(m_dxDeposition != null)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetCommandItem", Ex);
				tmaxItem = null;
			}
			
			return tmaxItem;

		}// private CTmaxItem GetCommandItem()

		/// <summary>This method processes the specified command</summary>
		/// <param name="eCommand">The enumerated command identifier</param>
		private void OnCommand(TranscriptPaneCommands eCommand)
		{
			long lSelections = 0;
			
			//	Get the current number of row selections
			if(m_transGrid != null)
			{
				lSelections = m_transGrid.GetSelectedCount();
			}
			
			try
			{
				//	Direct to the appropriate handler
				switch(eCommand)
				{
					case TranscriptPaneCommands.PreviewSelection:

						if(lSelections > 0)
							OnCmdPreviewSelection();
						break;

					case TranscriptPaneCommands.CopySelection:

						if(lSelections > 0)
							OnCmdCopySelection();
						break;

					case TranscriptPaneCommands.PrintSelection:

						if(lSelections > 0)
							OnCmdPrintSelection();
						break;

					case TranscriptPaneCommands.SaveSelection:

						if(lSelections > 0)
							OnCmdSaveSelection();
						break;

					case TranscriptPaneCommands.AddSelection:

						if(lSelections > 0)
							OnCmdAddSelection(false, false);
						break;

					case TranscriptPaneCommands.AddObjection:

						OnCmdAddObjection(false);
						break;

					case TranscriptPaneCommands.RepeatObjection:

						OnCmdAddObjection(true);
						break;

					case TranscriptPaneCommands.AddLink:

						OnCmdAddLink();
						break;

					case TranscriptPaneCommands.InsertSelectionBefore:

						if(lSelections > 0)
							OnCmdAddSelection(true, true);
						break;
						
					case TranscriptPaneCommands.InsertSelectionAfter:

						if(lSelections > 0)
							OnCmdAddSelection(true, false);
						break;
						
					case TranscriptPaneCommands.EditExtents:
					case TranscriptPaneCommands.Exclude:
					case TranscriptPaneCommands.SplitBefore:
					case TranscriptPaneCommands.SplitAfter:

						if(lSelections > 0)
							OnCmdEditDesignation(eCommand);
						break;
						
					case TranscriptPaneCommands.AssignHighlighter:
					
						OnCmdAssignHighlighter();
						break;
						
					case TranscriptPaneCommands.Find:
					
						OnCmdFind();
						break;
						
					case TranscriptPaneCommands.SetHighlighter1:
					case TranscriptPaneCommands.SetHighlighter2:
					case TranscriptPaneCommands.SetHighlighter3:
					case TranscriptPaneCommands.SetHighlighter4:
					case TranscriptPaneCommands.SetHighlighter5:
					case TranscriptPaneCommands.SetHighlighter6:
					case TranscriptPaneCommands.SetHighlighter7:
					
						OnCmdSetHighlighter((int)eCommand);
						break;
						
					case TranscriptPaneCommands.PinTranscript:
					
						OnCmdPinTranscript();
						break;
						
					case TranscriptPaneCommands.GoPageLine:
					
						OnCmdGoPageLine();
						break;
						
					case TranscriptPaneCommands.Update:
					
						OnCmdUpdate();
						break;

					case TranscriptPaneCommands.OpenProperties:
					case TranscriptPaneCommands.OpenCodes:
					case TranscriptPaneCommands.OpenPresentation:

						OnCmdOpen(eCommand);
						break;

					default:
					
						break;
				
				}// switch(eCommand)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnCommand", Ex);
			}
		
		}// private void OnCommand(TranscriptPaneCommands eCommand)

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
			TranscriptPaneCommands eCommand = TranscriptPaneCommands.Invalid;
			
			//	Don't bother if ignoring events
			if(m_bIgnoreUltraEvents == true) return;
			
			//	Get the command
			if(e.Tool != null && e.Tool.Key != null)
				eCommand = GetCommand(e.Tool.Key);
				
			if(eCommand != TranscriptPaneCommands.Invalid)
				OnCommand(eCommand);
		
		}// OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)

		/// <summary>This method handles events fired by the toolbar manager after displaying the popup menu</summary>
		/// <param name="sender">The toolbar manager firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnUltraAfterCloseup(object sender, Infragistics.Win.UltraWinToolbars.ToolDropdownEventArgs e)
		{
			TranscriptPaneCommands eCommand = TranscriptPaneCommands.Invalid;
			
			//	Don't bother if this is not the context menu
			if(e.Tool.Key != "ContextMenu") return;
			
			//	Check each tool in the manager's collection
			foreach(ToolBase O in m_ultraToolbarManager.Tools)
			{
				//	Get the command for this tool
				if((eCommand = GetCommand(O.Key)) != TranscriptPaneCommands.Invalid)
				{
					//	Uninstall the shortcut if required
					if(GetCommandShortcut(eCommand) != Shortcut.None)
						O.SharedProps.Shortcut = Shortcut.None;
				}

			}// foreach(ToolBase O in m_ultraToolbarManager.Tools)
				
		}

		/// <summary>Traps the ToolValueChanged event fired by the toolbar manager</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event argument object</param>
		private void OnUltraToolValueChanged(object sender, Infragistics.Win.UltraWinToolbars.ToolEventArgs e)
		{
			if(e.Tool.Key == "Transcripts")
			{
				OnTranscriptsSelChanged();
			}
			else if(e.Tool.Key == "Highlighters")
			{
				OnHighlightersSelChanged();
			}
		
		}// private void OnUltraToolValueChanged(object sender, Infragistics.Win.UltraWinToolbars.ToolEventArgs e)

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
					
						try { OnContextMenu((PopupMenuTool)(e.Tool)); }
						catch {}
						break;
						
					case "SetHighlighter":
					
						try { OnSetHighlighterMenu((PopupMenuTool)(e.Tool)); }
						catch {}
						break;
						
					case "Transcripts":
					case "Highlighters":
					
						break;
						
				}
			
			}
			catch
			{
			}
							
		}// OnUltraBeforeDropDown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventArgs e)

		/// <summary>This method handles events fired by the grid when the user changes the row selections</summary>
		/// <param name="sender">The toolbar manager firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnGridSelChanged(object sender, System.EventArgs e)
		{
			//	Are grid events inhibited?
			if(m_bIgnoreGridEvents == true) return;
			
			//	Update the commands
			OnContextMenu(null);
		}

		/// <summary>This method handles events fired by the grid when the user double clicks</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The event arguments</param>
		private void OnGridDblClick(object sender, System.EventArgs e)
		{
			CDxTertiary dxDesignation = null;
			CXmlTranscript xmlTranscript = null;
			bool bSuccessful = false;

			if(m_dxDeposition == null) return;

			try
			{
				Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

				while(true)
				{
					//	There MUST be an active designation
					if((dxDesignation = GetDesignation()) == null)
						break;

					//	Get the XML transcript at the click location
					if((xmlTranscript = m_transGrid.GetCursorTranscript()) == null)
						break;

					//	Are we out of range for the current designation?
					if(m_bDoubleSplitBefore == true)
					{
						if((xmlTranscript.PL <= dxDesignation.StartPL) || (xmlTranscript.PL > dxDesignation.StopPL))
							break;
					}
					else
					{
						//	Split after...
						if((xmlTranscript.PL < dxDesignation.StartPL) || (xmlTranscript.PL >= dxDesignation.StopPL))
							break;
					}


					//	Ignore this line if not synchronized
					if(xmlTranscript.Synchronized == false)
						break;

					//	Ignore this line if it does not exceed the Pause period
					if((xmlTranscript.Stop - xmlTranscript.Start) < m_tmaxStationOptions.PauseThreshold)
						break;

					//	Fire the command to edit the designation
					if(m_bDoubleSplitBefore == true)
						bSuccessful = FireEditCommand(TmaxDesignationEditMethods.SplitBefore, xmlTranscript.PL, xmlTranscript.PL);
					else
						bSuccessful = FireEditCommand(TmaxDesignationEditMethods.SplitAfter, xmlTranscript.PL, xmlTranscript.PL);

					if(bSuccessful == true)
						m_transGrid.SetSelections(null);

					//	We're done
					break;

				}// while(true)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnGridDblClick", m_tmaxErrorBuilder.Message(ERROR_ON_GRID_DBL_CLICK_EX), Ex);
			}
			finally
			{
				Cursor.Current = System.Windows.Forms.Cursors.Default;
			}

		}// private void OnGridDblClick(object sender, System.EventArgs e)

		/// <summary>This method is retrieve the requested combobox</summary>
		private Infragistics.Win.UltraWinToolbars.ComboBoxTool GetUltraComboBox(string strKey)
		{
			return ((ComboBoxTool)GetUltraTool(strKey));
	
		}// private Infragistics.Win.UltraWinToolbars.ComboBoxTool GetUltraComboBox(string strKey)
		
		/// <summary>This method is retrieve the transcripts combobox</summary>
		private Infragistics.Win.UltraWinToolbars.ComboBoxTool GetTranscriptsBox()
		{
			return ((ComboBoxTool)GetUltraTool("Transcripts"));
	
		}// private Infragistics.Win.UltraWinToolbars.ComboBoxTool GetTranscriptsBox()
		
		/// <summary>This method is retrieve the highlighters combobox</summary>
		private Infragistics.Win.UltraWinToolbars.ComboBoxTool GetHighlightersBox()
		{
			return ((ComboBoxTool)GetUltraTool("Highlighters"));
	
		}// private Infragistics.Win.UltraWinToolbars.ComboBoxTool GetHighlightersBox()
		
		/// <summary>This method is called to fill the transcripts combobox</summary>
		private void FillTranscripts()
		{
			ComboBoxTool ctrlTranscripts = GetTranscriptsBox();
						
			if((ctrlTranscripts != null) &&
			   (ctrlTranscripts.ValueList != null) &&
			   (ctrlTranscripts.ValueList.ValueListItems != null))
			{
				//	Clear the existing transcripts
				ctrlTranscripts.ValueList.ValueListItems.Clear();
				
				if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null))
				{
					foreach(CDxPrimary dxPrimary in m_tmaxDatabase.Primaries)
					{
						//	Is this a deposition?
						if(dxPrimary.MediaType == TmaxMediaTypes.Deposition)
						{
							try
							{
								if(dxPrimary.Name != null && (dxPrimary.Name.Length > 0))
									ctrlTranscripts.ValueList.ValueListItems.Add(dxPrimary, dxPrimary.Name);
								else
									ctrlTranscripts.ValueList.ValueListItems.Add(dxPrimary, dxPrimary.MediaId);
							}
							catch(System.Exception Ex)
							{
								m_tmaxEventSource.FireError(this, "FillTranscripts", m_tmaxErrorBuilder.Message(ERROR_FILL_TRANSCRIPTS_EX), Ex);
							}
							
						}
							
					}
						
				}// if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null))
				
			}
		
		}// private void FillTranscripts()
		
		/// <summary>This method is called to add an entry to the transcripts list box</summary>
		private void AddTranscriptsSelection(CDxPrimary dxPrimary)
		{
			ComboBoxTool ctrlTranscripts = GetTranscriptsBox();
						
			if((ctrlTranscripts != null) &&
				(ctrlTranscripts.ValueList != null) &&
				(ctrlTranscripts.ValueList.ValueListItems != null))
			{
				try
				{
					if(dxPrimary.Name != null && (dxPrimary.Name.Length > 0))
						ctrlTranscripts.ValueList.ValueListItems.Add(dxPrimary, dxPrimary.Name);
					else
						ctrlTranscripts.ValueList.ValueListItems.Add(dxPrimary, dxPrimary.MediaId);
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "AddTranscriptsSelection", m_tmaxErrorBuilder.Message(ERROR_ADD_TRANSCRIPTS_SEL_EX), Ex);
				}
				
			}
		
		}// private void AddTranscriptsSelection(CDxPrimary dxPrimary)

		/// <summary>Called to add a link to the specified designation using the specified source record and transcript line</summary>
		/// <param name="dxDesignation">The parent designation</param>
		/// <param name="xmlLink">The XML link descriptor</param>
		/// <returns>true if successful</returns>
		private bool AddLink(CDxTertiary dxDesignation, CXmlLink xmlLink)
		{
			CTmaxItem		tmaxItem = null;
			CXmlDesignation	xmlDesignation = null;
			bool			bSuccessful = false;

			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(dxDesignation != null);
			Debug.Assert(xmlLink != null);

			try
			{
				//	We need the XML designation to complete the initialization
				//	of the link and add the record
				if((xmlDesignation = m_tmaxDatabase.GetXmlDesignation(dxDesignation, true, true, true)) == null)
					return false;
					
				//	Set the time for the link object if it hasn't already been set
				if(xmlLink.Start <= 0)
				{
					xmlLink.Start = xmlDesignation.GetPosition(xmlLink.PL);
					xmlLink.StartTuned = false;
				}
				
				//	Construct the item needed to add the link as a child of the designation	
				tmaxItem = new CTmaxItem(dxDesignation);
				tmaxItem.XmlDesignation = xmlDesignation;
				
				if(tmaxItem.SourceItems == null)
					tmaxItem.SourceItems = new CTmaxItems();

				tmaxItem.SourceItems.Add(new CTmaxItem());
				tmaxItem.SourceItems[0].XmlLink = xmlLink;

				bSuccessful = FireCommand(TmaxCommands.Add, tmaxItem);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddLink", m_tmaxErrorBuilder.Message(ERROR_ADD_LINK_EX), Ex);
			}
			
			return bSuccessful;

		}// private bool AddLink(CDxTertiary dxDesignation, CXmlLink xmlLink)

		/// <summary>Called to create an XML link using the specified values</summary>
		/// <param name="xmlTranscript">The line of transcript text being linked</param>
		/// <param name="dxDesignation">The parent designation</param>
		/// <param name="dxSource">The source record</param>
		/// <returns>The new link</returns>
		private CXmlLink CreateXmlLink(CXmlTranscript xmlTranscript, CDxTertiary dxDesignation, CDxMediaRecord dxSource)
		{
			CXmlLink xmlLink = null;

			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(xmlTranscript != null);

			try
			{
				//	Create the XML link required to add the record
				xmlLink = new CXmlLink();

				if(xmlTranscript != null)
				{
					xmlLink.PL = xmlTranscript.PL;
					xmlLink.Start = xmlTranscript.Start;
					xmlLink.StartTuned = false;
				}

				if(dxSource != null)
				{
					xmlLink.SourceDbId = dxSource.GetUniqueId();
					xmlLink.SourceMediaId = dxSource.GetBarcode(true);
				}
				
				xmlLink.Hide = false;
				xmlLink.HideVideo = false;
				if(dxDesignation != null)
					xmlLink.HideText = (dxDesignation.ScrollText == false);
				else
					xmlLink.HideText = false;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateXmlLink", m_tmaxErrorBuilder.Message(ERROR_CREATE_XML_LINK_EX), Ex);
				xmlLink = null;
			}

			return xmlLink;

		}// private CXmlLink CreateXmlLink(CXmlTranscript xmlTranscript, CDxTertiary dxDesignation, CDxMediaRecord dxSource)

		/// <summary>This function is called to set the selection in the scripts list</summary>
		/// <param name="dxPrimary">The primary deposition that owns the transcript</param>
		private void SetTranscriptsSelection(CDxPrimary dxPrimary)
		{
			ComboBoxTool	ctrlTranscripts = null;
			ValueListItem	vlItem = null;
		
			//	Prevent responding to events while we make the change
			//
			//	NOTE:	This may be getting called as a result of the user
			//			making a new selection in the transcripts combobox
			if(m_bIgnoreUltraEvents == false)
				m_bIgnoreUltraEvents = true;
			else
				return;	//	Must be selecting locally
			

			//	Make sure we have the required objects
			if((ctrlTranscripts = GetTranscriptsBox()) == null) return;
			if(ctrlTranscripts.ValueList == null) return;
			if(ctrlTranscripts.ValueList.ValueListItems == null) return;
			
			try
			{
				//	Clear the current selection
				ctrlTranscripts.SelectedItem = null;
				
				if(dxPrimary != null)
				{	
					foreach(Infragistics.Win.ValueListItem O in ctrlTranscripts.ValueList.ValueListItems)
					{
						if(ReferenceEquals(O.DataValue, dxPrimary) == true)
						{
							ctrlTranscripts.SelectedItem = O;
							vlItem = O;
							break;
						}
						
					}
					
					//	Move to the top of the list
					if(vlItem != null)
					{
						ctrlTranscripts.ValueList.ValueListItems.Remove(vlItem);
						ctrlTranscripts.ValueList.ValueListItems.Insert(0, vlItem);
					}
				
				}// if(dxPrimary != null)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetTranscriptSelection", m_tmaxErrorBuilder.Message(ERROR_SET_TRANSCRIPT_SEL_EX), Ex);
			}
			finally
			{
				m_bIgnoreUltraEvents = false;
			}		
			
		}// private void SetTranscriptSelection(CDxPrimary dxPrimary)
		
		/// <summary>This function is called to remove the specified transcript from the list</summary>
		/// <param name="dxPrimary">The primary deposition that owns the transcript</param>
		private void RemoveTranscriptsSelection(CDxPrimary dxPrimary)
		{
			ComboBoxTool ctrlTranscripts = null;
		
			//	Prevent responding to events while we make the change
			m_bIgnoreUltraEvents = true;

			//	Make sure we have the required objects
			if((ctrlTranscripts = GetTranscriptsBox()) == null) return;
			if(ctrlTranscripts.ValueList == null) return;
			if(ctrlTranscripts.ValueList.ValueListItems == null) return;
			
			try
			{
				foreach(Infragistics.Win.ValueListItem O in ctrlTranscripts.ValueList.ValueListItems)
				{
					if(ReferenceEquals(O.DataValue, dxPrimary) == true)
					{
						//	Is this the selected item?
						if(ctrlTranscripts.SelectedItem == O)
							ctrlTranscripts.SelectedItem = null;
							
						ctrlTranscripts.ValueList.ValueListItems.Remove(O);
						break;
					}
					
				}
				
			}
			catch
			{
			}

			m_bIgnoreUltraEvents = false;
			
		}// private void RemoveTranscriptsSelection(CDxPrimary dxPrimary)
		
		/// <summary>This method is called when the user selects a new transcript from the drop down list</summary>
		private void OnTranscriptsSelChanged()
		{
			ComboBoxTool	ctrlTranscripts = null;
			ValueListItem	vlItem = null;
			
			if(m_bIgnoreUltraEvents == true) return;
			
			//	Get the combo box
			if((ctrlTranscripts = GetTranscriptsBox()) == null) return;
			
			//	Is there a selection?
			if((vlItem = (ValueListItem)ctrlTranscripts.SelectedItem) != null)
			{
				if(vlItem.DataValue != null)
				{
					//	Load this transcript
					SetRecord((CDxPrimary)(vlItem.DataValue), true);
				
					//	Move this item to the top of the list
					m_bIgnoreUltraEvents = true;
					ctrlTranscripts.ValueList.ValueListItems.Remove(vlItem);
					ctrlTranscripts.ValueList.ValueListItems.Insert(0, vlItem);
					ctrlTranscripts.SelectedItem = vlItem;
					m_bIgnoreUltraEvents = false;
				}
				
			}
			else
			{
				//	Unload the pane
				Unload();
			}

		}

		/// <summary>This method is called to fill the highlighters combobox</summary>
		private void FillHighlighters()
		{
			ComboBoxTool ctrlHighlighters = GetHighlightersBox();
						
			//	Clear the active highlighter
			m_dxHighlighter = null;
			
			if((ctrlHighlighters != null) &&
			   (ctrlHighlighters.ValueList != null) &&
			   (ctrlHighlighters.ValueList.ValueListItems != null))
			{
				//	Clear the existing transcripts
				ctrlHighlighters.ValueList.ValueListItems.Clear();
				
				if((m_tmaxDatabase != null) && (m_tmaxDatabase.Highlighters != null))
				{
					foreach(CDxHighlighter O in m_tmaxDatabase.Highlighters)
					{
						if(O.Name != null && O.Name.Length > 0)
						{
							try
							{
								ctrlHighlighters.ValueList.ValueListItems.Add(O, O.Name);
							}
							catch(System.Exception Ex)
							{
								m_tmaxEventSource.FireError(this, "FillHighlighters", m_tmaxErrorBuilder.Message(ERROR_FILL_HIGHLIGHTERS_EX), Ex);
							}
						}
							
							
					}
						
				}// if((m_tmaxDatabase != null) && (m_tmaxDatabase.Highlighters != null))
				
			}
			
			//	Set the active highlighter
			if((m_tmaxDatabase != null) && (m_tmaxDatabase.Highlighters != null))
			{
				if(m_tmaxDatabase.Highlighters.Count > 0)
				{
					if((m_tmaxStationOptions != null) && (m_tmaxStationOptions.LastHighlighter > 0))
						m_dxHighlighter = m_tmaxDatabase.Highlighters.Find(m_tmaxStationOptions.LastHighlighter);
						
					if((m_dxHighlighter == null) || (m_dxHighlighter.Name.Length == 0))
					{
						//	Locate the first valid highlighter
						foreach(CDxHighlighter O in m_tmaxDatabase.Highlighters)
						{
							if(O.Name != null && O.Name.Length > 0)
							{
								m_dxHighlighter = O;
								break;
							}
							
						}
					
					}// if(m_dxHighlighter == null)
						
					SetHighlightersSelection(m_dxHighlighter);
				
				}
				
			}
		
		}// private void FillHighlighters()
		
		/// <summary>This function is called to set the selection in the scripts list</summary>
		/// <param name="dxHighlighter">The highlighter to be selected</param>
		private void SetHighlightersSelection(CDxHighlighter dxHighlighter)
		{
			ComboBoxTool	ctrlHighlighters = null;
			ValueListItem	vlItem = null;
		
			//	Prevent responding to events while we make the change
			//
			//	NOTE:	This may be getting called as a result of the user
			//			making a new selection in the transcripts combobox
			if(m_bIgnoreUltraEvents == false)
				m_bIgnoreUltraEvents = true;
			else
				return;	//	Must be selecting locally
			
			//	Update the case options
			if((m_tmaxCaseOptions != null) && (m_tmaxCaseOptions.Machine != null))
			{
				if(dxHighlighter != null)
					m_tmaxStationOptions.LastHighlighter = dxHighlighter.AutoId;
				else
					m_tmaxStationOptions.LastHighlighter = -1;
			}
			
			//	Make sure we have the required objects
			if((ctrlHighlighters = GetHighlightersBox()) == null) return;
			if(ctrlHighlighters.ValueList == null) return;
			if(ctrlHighlighters.ValueList.ValueListItems == null) return;
			
			try
			{
				//	Clear the current selection
				ctrlHighlighters.SelectedItem = null;
				
				if(dxHighlighter != null)
				{	
					foreach(Infragistics.Win.ValueListItem O in ctrlHighlighters.ValueList.ValueListItems)
					{
						if(ReferenceEquals(O.DataValue, dxHighlighter) == true)
						{
							ctrlHighlighters.SelectedItem = O;
							vlItem = O;
							break;
						}
						
					}
				
				}// if(dxHighlighter != null)
			
				//	Move to the top of the list
				if(vlItem != null)
				{
					ctrlHighlighters.ValueList.ValueListItems.Remove(vlItem);
					ctrlHighlighters.ValueList.ValueListItems.Insert(0, vlItem);
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetHighlighterSelection", m_tmaxErrorBuilder.Message(ERROR_SET_HIGHLIGHTER_SEL_EX), Ex);
			}
			finally
			{
				m_bIgnoreUltraEvents = false;
			}		
			
		}// private void SetHighlighterSelection(CDxHighlighter dxHighlighter)
		
		/// <summary>This method is called when the user selects a new highlighter from the drop down list</summary>
		private void OnHighlightersSelChanged()
		{
			ComboBoxTool	ctrlHighlighters = null;
			ValueListItem	vlItem = null;
			
			if(m_bIgnoreUltraEvents == true) return;
			
			//	Get the combo box
			if((ctrlHighlighters = GetHighlightersBox()) == null) return;
			
			//	Is there a selection?
			if((vlItem = (ValueListItem)ctrlHighlighters.SelectedItem) != null)
			{
				m_dxHighlighter = (CDxHighlighter)(vlItem.DataValue);
				
				//	Move this item to the top of the list
				m_bIgnoreUltraEvents = true;
				ctrlHighlighters.ValueList.ValueListItems.Remove(vlItem);
				ctrlHighlighters.ValueList.ValueListItems.Insert(0, vlItem);
				ctrlHighlighters.SelectedItem = vlItem;
				m_bIgnoreUltraEvents = false;
			}
			else
			{
				m_dxHighlighter = null;
			}

			//	Update the case options
			if((m_tmaxCaseOptions != null) && (m_tmaxCaseOptions.Machine != null))
			{
				if(m_dxHighlighter != null)
					m_tmaxStationOptions.LastHighlighter = m_dxHighlighter.AutoId;
				else
					m_tmaxStationOptions.LastHighlighter = -1;

			}
			
		}// private void OnHighlightersSelChanged()

		/// <summary>This method is called to load the transcript associated with the specified primary deposition</summary>
		/// <param name="dxDeposition">The primary record exchange object for the deposition</param>
		/// <returns>true if successful</returns>
		private bool LoadDeposition(CDxPrimary dxDeposition)
		{
			string			strFileSpec = "";
			CXmlDeposition	xmlDeposition = null;
			
			//	Make sure we have the segments
			if((dxDeposition.Secondaries == null) || (dxDeposition.Secondaries.Count == 0))
			{	
				if(dxDeposition.Fill() == false)
					return false;
			}
				
			//	Get the path to the XML transcript
			if((strFileSpec = GetTranscriptFileSpec(dxDeposition)) == null)
				return false;
				
			//	Create and load a new XML deposition 
			xmlDeposition = new CXmlDeposition();
			xmlDeposition.FastFill(strFileSpec, true, true, false);
				
			if((xmlDeposition.Transcripts == null) || (xmlDeposition.Transcripts.Count == 0))
			{
				m_tmaxEventSource.FireError(this, "LoadDeposition", m_tmaxErrorBuilder.Message(ERROR_TEXT_NOT_FOUND, strFileSpec));
				return false;
			}

			//	Update the local references
			m_dxDeposition  = dxDeposition;
			m_xmlDeposition = xmlDeposition;
			m_xmlDeposition.MediaId = m_dxDeposition.GetMediaId();

			//	Load the grid with the new transcript text
			return FillGrid();
			
		}// private bool LoadDeposition(CDxPrimary dxDeposition)
		
		/// <summary>This method is called to get the path to the transcript file associated with the specified deposition</summary>
		/// <param name="dxPrimary">The deposition's record exchange object</param>
		/// <returns>true if successful</returns>
		private string GetTranscriptFileSpec(CDxPrimary dxDeposition)
		{
			CDxTranscript	dxTranscript = null;
			string			strFileSpec = "";
			
			while(true)
			{
				//	Get the transcript information for this deposition
				if((dxTranscript = dxDeposition.GetTranscript()) == null)
				{
					m_tmaxEventSource.FireError(this, "GetTranscriptFileSpec", m_tmaxErrorBuilder.Message(ERROR_TRANSCRIPT_NOT_FOUND, dxDeposition.MediaId));
					break;
				}
			
				//	Get the path to the transcript file from the database
				strFileSpec = m_tmaxDatabase.GetFileSpec(dxTranscript);
				
				if((strFileSpec == null) || (strFileSpec.Length == 0))
				{
					m_tmaxEventSource.FireError(this, "GetTranscriptFileSpec", m_tmaxErrorBuilder.Message(ERROR_INVALID_FILESPEC, dxDeposition.MediaId));
					break;
				}
				
				//	Make sure the file exists
				if(System.IO.File.Exists(strFileSpec) == false)
				{
					m_tmaxEventSource.FireError(this, "GetTranscriptFileSpec", m_tmaxErrorBuilder.Message(ERROR_FILE_NOT_FOUND, strFileSpec));
					strFileSpec = "";	//	Prevents attempts to process the file
					break;
				}
			
				//	We're done
				break;
				
			}// while(true)
			
			if((strFileSpec != null) && (strFileSpec.Length > 0))
				return strFileSpec;
			else
				return null;

		}// private string GetTranscriptFileSpec(CDxPrimary dxDeposition)
		
		/// <summary>Called to set the source record used for dragging new links</summary>
		/// <param name="dxSource">The exchange interface to the source record</param>
		/// <returns>The record assigned as the new drag link</returns>
		private CDxMediaRecord SetDropSource(CDxMediaRecord dxSource)
		{
			//	Make sure we have a valid media type
			if(dxSource != null)
			{
				//	What type of record is being dragged?
				switch(dxSource.MediaType)
				{
					case TmaxMediaTypes.Page:
					case TmaxMediaTypes.Treatment:
					case TmaxMediaTypes.Slide:

						//	These are valid media types
						m_dxDropSource = dxSource;
						break;

					case TmaxMediaTypes.Scene:

						//	Use the source record
						m_dxDropSource = SetDropSource(((CDxSecondary)dxSource).GetSource()); 
						break;

					case TmaxMediaTypes.Link:

						//	Use the source record
						m_dxDropSource = SetDropSource(((CDxQuaternary)dxSource).GetSource());
						break;

					default:

						m_dxDropSource = null; // Can't link to this media
						break;

				}// switch(dxSource.MediaType)

			}
			else
			{
				m_dxDropSource = null;
				
			}// if(dxSource != null)
			
			return m_dxDropSource;

		}// private CDxMediaRecord SetDropSource(CDxMediaRecord dxSource)

		/// <summary>Called to set the line where the new link will be dropped</summary>
		/// <returns>The value assigned to the DropLine class member</returns>
		private CXmlTranscript SetDropLine()
		{
			try
			{
				//	Must have an active drop designation
				if(m_dxDropDesignation != null)
				{
					//	Query the grid control for the line under the cursor
					if((m_xmlDropLine = m_transGrid.GetCursorTranscript()) != null)
					{
						//	The line must be within the range defined for the designation
						if((m_xmlDropLine.PL < m_dxDropDesignation.StartPL) || (m_xmlDropLine.PL > m_dxDropDesignation.StopPL))
							m_xmlDropLine = null;
					}

				}
				else
				{
					m_xmlDropLine = null;
				}

			}
			catch(System.Exception Ex)
			{
				m_xmlDropLine = null;
				m_tmaxEventSource.FireDiagnostic(this, "SetDropLine", Ex);
			}
			
			return m_xmlDropLine;

		}// private CXmlTranscript SetDropLine()

		/// <summary>Called to get the line where the link will be added</summary>
		/// <param name="dxDesignation">The designation that will parent the link</param>
		/// <returns>The line where the link will be added</returns>
		private CXmlTranscript GetAddLinkLine(CDxTertiary dxDesignation)
		{
			CXmlTranscript	xmlLine = null;
			CXmlTranscripts	xmlSelections = null;

			try
			{
				if(dxDesignation == null)
					dxDesignation = GetDesignation();

				//	MUST have a designation to define the page/line range
				if(dxDesignation != null)
				{
					xmlSelections = new CXmlTranscripts();
					
					//	Must only be one line selected
					if(m_transGrid.GetSelections(xmlSelections) == 1)
					{
						if((xmlLine = xmlSelections[0]) != null)
						{
							//	The line must be within the range defined for the designation
							if((xmlLine.PL < dxDesignation.StartPL) || (xmlLine.PL > dxDesignation.StopPL))
								xmlLine = null;

						}// if((xmlLine = xmlSelections[0]) != null)
						
						xmlSelections.Clear();

					}// if(m_transGrid.GetSelections(xmlSelections) == 1)

				}// if(dxDesignation != null)

			}
			catch(System.Exception Ex)
			{
				xmlLine = null;
				m_tmaxEventSource.FireDiagnostic(this, "GetAddLinkLine", Ex);
			}

			return xmlLine;

		}// private CXmlTranscript GetAddLinkLine(CDxTertiary dxDesignation)

		/// <summary>This method is called to get the drag/drop effects for the current position</summary>
		/// <param name="iX">The current X position of the cursor</param>
		/// <param name="iY">The current Y position of the cursor</param>
		/// <returns>The appropriate drag/drop effects</returns>
		private System.Windows.Forms.DragDropEffects GetDropEffects(int iX, int iY)
		{
			DragDropEffects e = DragDropEffects.None;
			
			while(true)
			{
				//	Must have a valid drag link
				if(m_dxDropSource == null)
					break;
					
				//	Must have a valid transcript
				if(m_dxDeposition == null)
					break;

				//	Must have an active script
				if(m_dxScript == null)
					break;

				//	Must have an active designation
				if(m_dxDropScene == null)
					break;
				if(m_dxDropDesignation == null)
					break;

				//	Must have a target line
				if(m_xmlDropLine == null)
					break;

				//	All is well
				e = DragDropEffects.Link;
				
				//	All done
				break;
				
			}
			
			return e;

		}// private System.Windows.Forms.DragDropEffects GetDropEffects(int iX, int iY)

		/// <summary>This method is called to fill the grid with the active transcript</summary>
		/// <returns>true if successful</returns>
		private bool FillGrid()
		{
			bool			bSuccessful = false;
			CTmaxObjections	tmaxObjections = null;

			Debug.Assert(m_xmlDeposition != null, "NULL XML Deposition");
			if(m_xmlDeposition == null) return false;
			Debug.Assert(m_xmlDeposition.Transcripts != null, "NULL XML Transcripts Collection");
			if(m_xmlDeposition.Transcripts == null) return false;
			Debug.Assert(m_xmlDeposition.Transcripts.Count > 0, "Empty XML Transcripts Collection");
			if(m_xmlDeposition.Transcripts.Count == 0) return false;
			
			//	Display the wait cursor
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				//	Make sure we are using the correct Pause threshold when we load a transcript
				SetPauseThreshold();

				//	Make sure we have the correct objection colors
				m_transGrid.DefendantColor = m_tmaxDatabase.GetObjectionsColor(TmaxHighlighterGroups.Defendant);
				m_transGrid.PlaintiffColor = m_tmaxDatabase.GetObjectionsColor(TmaxHighlighterGroups.Plaintiff);
				
				//m_tmaxEventSource.InitElapsed();

				//	Get the collection of objections associated with this deposition
				tmaxObjections = m_tmaxDatabase.Objections.FindAll(m_dxDeposition);

				//if(tmaxObjections != null)
					//m_tmaxEventSource.FireElapsed(this, "FillGrid", "Time to find " + tmaxObjections.Count.ToString() + " objections: ");
				//else
					//m_tmaxEventSource.FireElapsed(this, "FillGrid", "Time to find 0 objections: ");

				//m_tmaxEventSource.InitElapsed();

				//	Assign the deposition to the grid
				m_transGrid.SetDeposition(m_xmlDeposition, tmaxObjections);

				//m_tmaxEventSource.FireElapsed(this, "FillGrid", "Time to set deposition: ");
				
				//	Make sure the grid is visible
				if(m_transGrid.Visible == false)
					m_transGrid.Visible = true;
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				Unload();
				m_tmaxEventSource.FireError(this, "FillGrid", m_tmaxErrorBuilder.Message(ERROR_FILL_GRID_EX, m_xmlDeposition.FileSpec), Ex);
			}
			finally
			{
				//	Restore the cursor
				Cursor.Current = Cursors.Default;
			}

			return bSuccessful;

		}// private bool FillGrid()
		
		/// <summary>This method is called to set the active record</summary>
		/// <param name="dxRecord">The record to be activated</param>
		/// <param name="bDropDown">true if selected from the drop down list</param>
		/// <returns>true if the specified record gets activated</returns>
		private bool SetRecord(CDxMediaRecord dxRecord, bool bDropDown)
		{
			CDxPrimary		dxDeposition = null;
			CDxTertiary		dxDesignation = null;
			CDxSecondary	dxScene = null;
			CDxTertiary		dxSelect = null;
			bool			bDepositionChanged = false;
			long			lPageLine = -1;
			
			Debug.Assert(dxRecord != null);
			if(dxRecord == null) return false;
			
			//	Clear the record waiting for activation
			m_dxActivate = null;
			
			//	Switch to the parent designation if this is a link
			if(dxRecord.MediaType == TmaxMediaTypes.Link)
				dxRecord = dxRecord.GetParent();
			Debug.Assert(dxRecord != null);
			if(dxRecord == null) return false;
			
			//	Get the deposition associated with this record
			dxDeposition = GetDeposition(dxRecord);
		
			//	Do we need to load a new transcript?
			if((dxDeposition != null) && (ReferenceEquals(m_dxDeposition, dxDeposition) == false))
			{
				//	Is it OK to change the transcript?
				if((m_bPinTranscript == false) || (bDropDown == true))
				{
					//	Load the new deposition and its transcript
					if(LoadDeposition(dxDeposition) == false) 
						return false;

					//	Clear the old highlights
					m_dxDesignationScenes.Clear();
					m_dxDesignationScenes.Primary = null;
				
					if(bDropDown == false)
						SetTranscriptsSelection(m_dxDeposition);
						
					//	Update the case options
					if(m_tmaxStationOptions != null)
						m_tmaxStationOptions.LastDeposition = m_dxDeposition.AutoId;
						
					//	The deposition has changed
					bDepositionChanged = true;

				}// if((m_bPinTranscript == false) || (bDropDown == true))
				
			}// if((m_dxDeposition == null) || (ReferenceEquals(m_dxDeposition, dxDeposition) == false))
			
			//	Don't bother going any further if we have no active transcript
			if(m_dxDeposition == null)
			{
				OnContextMenu(null);
				SetStatusText();
				return true;
			}
			
			//	Has the deposition changed?
			if(bDepositionChanged == true)
			{
				//	Highlight all designations in the active script that have
				//	been created from this transcript
				if(m_dxScript != null)
					Highlight(m_dxScript);
			}
			else
			{
				//	Has the active script changed?
				if(ReferenceEquals(m_dxDesignationScenes.Primary, m_dxScript) == false)
				{
					//	Erase the existing highlights
					Erase(false);
					
					//	Add the highlights for this script
					Highlight(m_dxScript);
				}
				
			}

			//	Get the desired position
			switch(dxRecord.MediaType)
			{
				case TmaxMediaTypes.Segment:
				
					if(ReferenceEquals(dxRecord.GetParent(), m_dxDeposition) == true)
					{
						if(((CDxSecondary)dxRecord).GetExtent() != null)
						{
							lPageLine = ((CDxSecondary)dxRecord).GetExtent().StartPL;
						}
					}
					break;
					
				case TmaxMediaTypes.Designation:
				
					dxDesignation = (CDxTertiary)dxRecord;
					
					if(dxDesignation.Secondary != null)
					{
						if(ReferenceEquals(dxDesignation.Secondary.Primary, m_dxDeposition) == true)
						{
							if(dxDesignation.GetExtent() != null)
							{
								lPageLine = dxDesignation.GetExtent().StartPL;
								dxSelect = dxDesignation;
							}
						}
					}
					break;
					
				case TmaxMediaTypes.Scene:
				
					dxScene = (CDxSecondary)dxRecord;
					
					if(dxScene.SourceType == TmaxMediaTypes.Designation)
					{
						//	Make sure we have the source record
						if(dxScene.GetSource() != null) 
							dxDesignation = (CDxTertiary)dxScene.GetSource();
					
						if((dxDesignation != null) && (dxDesignation.Secondary != null))
						{
							if(ReferenceEquals(dxDesignation.Secondary.Primary, m_dxDeposition) == true)
							{
								if(dxDesignation.GetExtent() != null)
								{
									lPageLine = dxDesignation.GetExtent().StartPL;
									dxSelect = dxDesignation;
								}
							}
						}
					}
					break;
					
			}// switch(dxRecord.MediaType)
					
			//	Do we need to make a specific row visible?
			if(lPageLine > 0)
			{
				m_transGrid.EnsureVisible(lPageLine, false);
			}
			
			//	Set the selected designation
			SetSelectedIndicators(false);
			
			//	Enable/disable commands
			OnContextMenu(null);
						
			//	Update the status bar
			SetStatusText();

			//	Make sure the application is in sync
			FireSetDeposition(m_dxDeposition);
			
			return true;
		
		}// private bool SetRecord(CDxMediaRecord dxRecord)
			
		/// <summary>This method is called when the user brings up the context menu</summary>
		private void OnContextMenu(PopupMenuTool popupMenu)
		{
			TranscriptPaneCommands	eCommand;
			long					lSelections = 0;
			long					lStartPL = -1;
			long					lStopPL = -1;
			Shortcut				eShortcut = Shortcut.None;
						
			Debug.Assert(m_ultraToolbarManager != null);
			Debug.Assert(m_ultraToolbarManager.Tools != null);
			if((m_ultraToolbarManager == null) ||( m_ultraToolbarManager.Tools == null)) return;
			
			//	Get the current number of row selections
			lSelections = m_transGrid.GetSelectionRange(ref lStartPL, ref lStopPL);
			
			//	Check each tool in the manager's collection
			foreach(ToolBase ultraTool in m_ultraToolbarManager.Tools)
			{
				if(ultraTool.Key != null)
				{
					try
					{
						if((eCommand = GetCommand(ultraTool.Key)) != TranscriptPaneCommands.Invalid)
						{
							//	Should the command be enabled?
							ultraTool.SharedProps.Enabled = GetCommandEnabled(eCommand, lSelections, lStartPL, lStopPL);
							
							if(popupMenu != null)
							{
								if((eShortcut = GetCommandShortcut(eCommand)) != Shortcut.None)
									ultraTool.SharedProps.Shortcut = eShortcut;
							}
							
						}
						
					}
					catch
					{
					}
					
				}// if(ultraTool.Tag != null)

			}// foreach(ToolBase ultraTool in m_ultraToolbarManager.Tools)
				
			//	Set the check state of the Pin Transcript command
			if(m_ctrlPinTranscript != null)
				m_ctrlPinTranscript.Checked = m_bPinTranscript;
			
		}// private void OnContextMenu()

		/// <summary>This method is called when the user brings up the Set Highlighter menu</summary>
		private void OnSetHighlighterMenu(PopupMenuTool popupMenu)
		{
			string			strKey = "";
			StateButtonTool	toolHighlighter = null;
			CDxHighlighter	dxHighlighter = null;
			
			if(m_tmaxDatabase == null) return;
			if(m_tmaxDatabase.Highlighters == null) return;
			
			for(int i = 0; i < m_tmaxDatabase.Highlighters.Count; i++)
			{
				// Format the key for the menu tool
				strKey = String.Format("SetHighlighter{0}", i + 1);
				
				if((toolHighlighter = (StateButtonTool)GetUltraTool(strKey)) != null)
				{
					dxHighlighter = m_tmaxDatabase.Highlighters[i];
					
					if((dxHighlighter != null) && (dxHighlighter.Name.Length > 0))
					{
						toolHighlighter.SharedProps.Caption = dxHighlighter.Name;
						
						if(ReferenceEquals(dxHighlighter, m_dxHighlighter) == true)
							toolHighlighter.Checked = true;
						else
							toolHighlighter.Checked = false;
						
						toolHighlighter.SharedProps.Visible = true;
					}
					else
					{
						toolHighlighter.SharedProps.Visible = false;
					}
				}
			}
			
			
		}// private void OnSetHighlighterMenu(PopupMenuTool popupMenu)

		/// <summary>This method is called to set the status bar text</summary>
		private void SetStatusText()
		{
			string strText = "";
			
			if((m_ctrlStatusBar != null) && (m_ctrlStatusBar.IsDisposed == false))
			{
				//	Do we have a valid deposition?
				if(m_dxDeposition != null)
				{
					//	Has this record been assigned a name?
					if((m_dxDeposition.Name != null) && (m_dxDeposition.Name.Length > 0))
					{
						strText = ("[ " + m_dxDeposition.MediaId + " ]  --  ");
						strText += m_dxDeposition.Name;						
					}
					else
					{
						strText = m_dxDeposition.MediaId;
					}
					
				}
				
				//	Update the status bar
				if((m_ctrlStatusBar.Panels != null) && (m_ctrlStatusBar.Panels.Count > 1))
				{
					m_ctrlStatusBar.Panels[1].Text = strText;
					
					if(m_bPinTranscript)
						m_ctrlStatusBar.Panels[0].Appearance.Image = 3;
					else
						m_ctrlStatusBar.Panels[0].Appearance.Image = -1;
						
				}		
			}
			
			//	Update the pin transcript image
			if(m_ctrlPinTranscript != null)
			{
				m_ctrlPinTranscript.SharedProps.AppearancesSmall.Appearance.Image = m_bPinTranscript ? 3 : 7;
			}
		
		}// private void SetStatusText()
		
		
		/// <summary>This method will retrieve the deposition associate with the specified media record</summary>
		/// <param name="dxRecord">The media record associated with the deposition</param>
		/// <returns>The associated deposition record if available</returns>
		private CDxPrimary GetDeposition(CDxMediaRecord dxRecord)
		{
			CDxPrimary dxDeposition = null;
			CDxPrimary dxScript = null;
			CDxSecondary dxScene = null;
			
			//	What type of media is this?
			switch(dxRecord.MediaType)
			{
				case TmaxMediaTypes.Deposition:
				
					dxDeposition = (CDxPrimary)dxRecord;
					break;
					
				case TmaxMediaTypes.Segment:
				
					if((dxDeposition = (CDxPrimary)dxRecord.GetParent()) != null)
					{
						//	Segments may also belong to recordings
						if(dxDeposition.MediaType != TmaxMediaTypes.Deposition)
							dxDeposition = null;
					}
					break;
					
				case TmaxMediaTypes.Designation:
				
					if(dxRecord.GetParent() != null)
					{
						dxDeposition = (CDxPrimary)((dxRecord.GetParent()).GetParent());
					}
					break;
					
				case TmaxMediaTypes.Script:
				
					dxScript = (CDxPrimary)dxRecord;
					
					//	Does this script have any scenes?
					if(dxScript.ChildCount > 0)
					{
						//	Make sure we have the scene records
						if((dxScript.Secondaries == null) ||
						   (dxScript.Secondaries.Count == 0))
						{
							dxScript.Fill();
						}
						
						//	Get the trascript associated with the first scene
						if((dxScript.Secondaries != null) &&
						   (dxScript.Secondaries.Count > 0))
						{
							dxScene = dxScript.Secondaries[0];
							
							//	Get the deposition associated with the source
							if(dxScene.GetSource() != null)
								dxDeposition = GetDeposition(dxScene.GetSource());
								
						}
						
					}// if(dxScript.ChildCount > 0)
					
					break;
						
				case TmaxMediaTypes.Scene:
				
					dxScene = (CDxSecondary)dxRecord;
					
					//	Get the deposition associated with the source
					if(dxScene.GetSource() != null)
						dxDeposition = GetDeposition(dxScene.GetSource());
					
					break;
						
				default:
				
					break;
					
			}
			
			return dxDeposition;
		
		}// private CDxPrimary GetDeposition(CDxMediaRecord dxRecord)
		
		/// <summary>This method is called to highlight all designations contained in the specified script</summary>
		/// <param name="dxScript">The script that owns the designations</param>
		private void Highlight(CDxPrimary dxScript)
		{
			Debug.Assert(dxScript != null);
			Debug.Assert(dxScript.Secondaries != null);
			if(dxScript == null) return;
			if(dxScript.Secondaries == null) return;
			
			//	Make sure our local collection is empty
			m_dxDesignationScenes.Clear();
			m_dxDesignationScenes.Primary = null;
			
			//	Don't bother if we don't have any highlighters
			if((m_tmaxDatabase.Highlighters == null) ||
			   (m_tmaxDatabase.Highlighters.Count == 0)) return;
			
			//	Store the reference to the parent script
			m_dxDesignationScenes.Primary = dxScript;
			
			//	Highlight each scene that is a designation created from
			//	the active transcript
			foreach(CDxSecondary dxScene in dxScript.Secondaries)
			{
				Highlight(dxScene);
			}
			
		}// private void Highlight(CDxPrimary dxScript)
		
		/// <summary>This method is called to highlight the scene if it is an active designation</summary>
		/// <param name="dxScene">The scene to be highlighted</param>
		private void Highlight(CDxSecondary dxScene)
		{
			CDxTertiary		dxDesignation = null;
			CDxHighlighter	dxHighlighter = null;
			
			Debug.Assert(dxScene != null);
			Debug.Assert(dxScene.Primary != null);
			if(dxScene == null) return;
			if(dxScene.Primary == null) return;
			
			//	Don't bother if we don't have any highlighters
			if((m_tmaxDatabase.Highlighters == null) ||
				(m_tmaxDatabase.Highlighters.Count == 0)) return;

			//	Is this a designation?
			if(dxScene.SourceType != TmaxMediaTypes.Designation) return;
			
			//	Do we have the source?
			if((dxDesignation = (CDxTertiary)dxScene.GetSource()) == null) return;
			
			//	Make sure this designation belongs to the current transcript?
			if((dxDesignation.Secondary == null) || (dxDesignation.Secondary.Primary == null) ||
				(ReferenceEquals(m_dxDeposition, dxDesignation.Secondary.Primary) == false)) return;
			
			//	Does this designation have a highlighter?
			if((dxDesignation.GetExtent() == null) ||
			   (dxDesignation.GetExtent().HighlighterId <= 0)) return;

			if((dxHighlighter = m_tmaxDatabase.Highlighters.Find(dxDesignation.GetExtent().HighlighterId)) == null) return;
			
			//	Highlight this designation
			SetForeColor(dxDesignation, (int)dxHighlighter.Color);

			//	Add this to our collection
			m_dxDesignationScenes.AddList(dxScene);
			
		}// private void Highlight(CDxPrimary dxScript)
		
		/// <summary>This method is called to set the forground color for all rows associated with the specified designation</summary>
		/// <param name="dxDesignation">The designation to be highlighted</param>
		/// <param name="iColor">The argb color value</param>
		/// <returns>true if successful</returns>
		private bool SetForeColor(CDxTertiary dxDesignation, int iColor)
		{
			try
			{
				System.Drawing.Color foreColor = System.Drawing.Color.FromArgb(iColor);
				return SetForeColor(dxDesignation, foreColor);
			}
			catch
			{
				return false;
			}
			
		}// private bool SetForeColor(CDxTertiary dxDesignation, int iColor)
		
		/// <summary>This method is called to set the forground color for all rows associated with the specified designation</summary>
		/// <param name="dxDesignation">The designation to be highlighted</param>
		/// <param name="foreColor">The system drawing color</param>
		/// <returns>true if successful</returns>
		private bool SetForeColor(CDxTertiary dxDesignation, System.Drawing.Color foreColor)
		{
			long	lStartPL = 0;
			long	lStopPL = 0;
			bool	bSuccessful = false;
			
			Debug.Assert(m_transGrid != null);
			Debug.Assert(m_transGrid.IsDisposed == false);
			if(m_transGrid == null) return false;
			if(m_transGrid.IsDisposed == true) return false;
			
			try
			{
				//	Get the extents for this designation
				if(dxDesignation.GetExtent() == null) return false;
				lStartPL = dxDesignation.GetExtent().StartPL;
				lStopPL = dxDesignation.GetExtent().StopPL;

				bSuccessful = m_transGrid.SetTextColor(lStartPL, lStopPL, foreColor);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetForeColor", Ex);
			}
			
			return bSuccessful;

		}// private bool SetForeColor(CDxTertiary dxDesignation, System.Drawing.Color foreColor)
		
		/// <summary>This method is called to get the record interface for the active designation</summary>
		/// <returns>the exchange interface for the active designation if it exists</returns>
		private CDxTertiary GetDesignation()
		{
			CDxTertiary dxDesignation = null;
			
			if(m_dxDeposition == null) return null;
			if(m_dxScene == null) return null;
			if(m_dxScene.GetSource() == null) return null;
			if(m_dxScene.GetSource().MediaType != TmaxMediaTypes.Designation) return null;
			
			try
			{
				//	Make sure this designation is built from the active transcript?
				dxDesignation = (CDxTertiary)m_dxScene.GetSource();
				if(ReferenceEquals(dxDesignation.Secondary.Primary, m_dxDeposition) == true)
					return dxDesignation;
			}
			catch
			{
			}
			
			return null;
			
		}// private CDxTertiary GetDesignation()
		
		/// <summary>This method is called to set the selected indicators for the specified designation</summary>
		/// <returns>true if successful</returns>
		private bool SetSelectedIndicators(bool bEnsureVisible)
		{
			CDxTertiary	dxDesignation = null;
			long		lStartPL = -1;
			long		lStopPL = -1;
			bool		bSuccessful = false;

			Debug.Assert(m_transGrid != null);
			Debug.Assert(m_transGrid.IsDisposed == false);
			if(m_transGrid == null) return false;
			if(m_transGrid.IsDisposed == true) return false;
			
			try
			{
				//	Get the extents for the active designation
				if((dxDesignation = GetDesignation()) == null) return false;
				if(dxDesignation.GetExtent() == null) return false;

				lStartPL = dxDesignation.GetExtent().StartPL;
				lStopPL = dxDesignation.GetExtent().StopPL;
			
				bSuccessful = m_transGrid.SetActive(lStartPL, lStopPL, bEnsureVisible);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetSelectedIndicators", Ex);
			}
			
			return bSuccessful;

		}// private bool SetSelectedIndicators(bool bEnsureVisible)
		
		/// <summary>This method is called to erase all active highlights</summary>
		private void Erase(bool bIgnoreScenes)
		{
			if(bIgnoreScenes == true)
			{
				m_transGrid.Erase();
			}
			else
			{
				if(m_dxDesignationScenes != null)	
				{
					foreach(CDxSecondary dxScene in m_dxDesignationScenes)
					{
						if(dxScene.GetSource() != null)
							SetForeColor((CDxTertiary)dxScene.GetSource(), System.Drawing.Color.Black);
					
					}
				}
			}
					
			//	Clear the local collection
			m_dxDesignationScenes.Clear();
			m_dxDesignationScenes.Primary = null;
				
		}// private void Erase()
		
		/// <summary>This method is called to create a collection of XML designations using the specified rows</summary>
		/// <param name="aRows">The rows to be converted to designations</param>
		/// <returns>The resultant designations collection</returns>
		private CXmlDesignations CreateDesignations(Array aRows)
		{
			int					iIndex = 0;
			int					iSegment = 0;
			CDxSecondary		dxSegment = null;
			CXmlDesignations	xmlDesignations = new CXmlDesignations();
			CXmlDesignation		xmlDesignation = null;

			while(iIndex <= aRows.GetUpperBound(0))
			{
				//	What is the new segment?
				if((dxSegment = GetSegment((CTmaxTransGridRow)(aRows.GetValue(iIndex)))) != null)
				{
					if((xmlDesignation = CreateDesignation(dxSegment, aRows, ref iIndex)) != null)
					{
						//	Add to the return collection
						xmlDesignations.Add(xmlDesignation);
					}
				
				}
				else
				{
					iSegment = ((CTmaxTransGridRow)aRows.GetValue(iIndex)).GetSegment();
					m_tmaxEventSource.FireError(this, "CreateDesignations", m_tmaxErrorBuilder.Message(ERROR_SEGMENT_NOT_FOUND, iSegment.ToString()));
					break;
				}
				
			}// while(iIndex <= aRows.GetUpperBound(0))
			
			if(xmlDesignations.Count > 0)
				return xmlDesignations;
			else
				return null;
		
		}// private CXmlDesignations CreateDesignations(Array aRows)
		
		/// <summary>This method is called to create an XML designation object</summary>
		/// <param name="dxSegment">The parent segment</param>
		/// <param name="aRows">The rows used to create the designation</param>
		/// <param name="rIndex">The index at which to start iterating rows</param>
		/// <returns>The resultant designations collection</returns>
		private CXmlDesignation CreateDesignation(CDxSecondary dxSegment, Array aRows, ref int rIndex)
		{
			CXmlDesignation		xmlDesignation = null;
			CXmlTranscript		xmlTranscript = null;
			CDxExtent			dxExtent = null;
			CTmaxTransGridRow	tgRow = null;
			
			//	Make sure we have the extents for the segment
			if((dxExtent = dxSegment.GetExtent()) == null)
				return null;
				
			try
			{
				//	Allocate the new designation
				xmlDesignation = new CXmlDesignation();
				m_tmaxEventSource.Attach(xmlDesignation.EventSource);
				xmlDesignation.UserData = dxSegment;

				//	Iterate the rows specified by the caller up to the next segment
				for(; rIndex <= aRows.GetUpperBound(0); rIndex++)
				{
					tgRow = (CTmaxTransGridRow)aRows.GetValue(rIndex);
					
					//	Has the segment changed?
					if(tgRow.GetSegment() != dxExtent.XmlSegmentId)
						break;
						
					//	Allocate a new transcript line
					xmlTranscript = new CXmlTranscript();
					m_tmaxEventSource.Attach(xmlTranscript.EventSource);
					
					//	Initialize the transcript
					xmlTranscript.Segment = tgRow.GetSegment().ToString();
					xmlTranscript.PL = tgRow.GetPL();
					xmlTranscript.Page = tgRow.GetPage();
					xmlTranscript.Line = tgRow.LN;
					xmlTranscript.QA = tgRow.QA;
					xmlTranscript.Start = tgRow.GetStart();
					xmlTranscript.Stop = tgRow.GetStop();
					xmlTranscript.Text = tgRow.Text;
					xmlTranscript.Synchronized = tgRow.GetSynchronized();
					
					//	Add the row to the designation transcripts
					xmlDesignation.Transcripts.Add(xmlTranscript);
					
				}// for(iIndex; iIndex <= aRows.GetUpperBound(); iIndex++)
				
				//	Do we have any text for this designation?
				if(xmlDesignation.Transcripts.Count > 0)
				{
					//	Use the transcripts collection to set the extents
					xmlDesignation.SetExtents();
					
					xmlDesignation.HasText = true;
					xmlDesignation.ScrollText = true;
					
					if((m_dxHighlighter != null) && (m_dxHighlighter.Name.Length > 0))
						xmlDesignation.Highlighter = (int)m_dxHighlighter.AutoId;
					else
						xmlDesignation.Highlighter = 0;
						
					xmlDesignation.SetNameFromExtents(m_dxDeposition.Name);					
				}
				else
				{
					xmlDesignation = null;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateDesignation", m_tmaxErrorBuilder.Message(ERROR_CREATE_DESIGNATION_EX), Ex);
				xmlDesignation = null;
			}
			
			return xmlDesignation;
		}
		
		/// <summary>This method is called to add a designation to the database</summary>
		/// <param name="xmlDesignation">The xml designation to be added</param>
		/// <returns>The new designation record exchange object</returns>
		private CDxTertiary AddDesignation(CXmlDesignation xmlDesignation)
		{
			CTmaxItem	tmaxParent = null;
			CTmaxItem	tmaxDesignation = null;
			
			Debug.Assert(xmlDesignation != null);
			Debug.Assert(xmlDesignation.UserData != null);
			if(xmlDesignation == null) return null;
			if(xmlDesignation.UserData == null) return null;
				
			try
			{
				//	Create a parent item for the segment
				tmaxParent = new CTmaxItem((CDxMediaRecord)xmlDesignation.UserData);
				Debug.Assert(tmaxParent.MediaType == TmaxMediaTypes.Segment);
				
				//	Create an item to represent the designation and add it
				//	to the source items collection
				tmaxDesignation = new CTmaxItem();
				tmaxDesignation.XmlDesignation = xmlDesignation;
				
				if(tmaxParent.SourceItems == null)
					tmaxParent.SourceItems = new CTmaxItems();
				tmaxParent.SourceItems.Add(tmaxDesignation);
				
				//	Fire the command to add the designation
				FireCommand(TmaxCommands.Add, tmaxParent);
				
				//	The database should have set the record interface
				if((tmaxDesignation.GetMediaRecord() != null) &&
				   (tmaxDesignation.MediaType == TmaxMediaTypes.Designation))
				{
					return (CDxTertiary)tmaxDesignation.GetMediaRecord();
				}
				else
				{
					return null;
				}				

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddDesignation", m_tmaxErrorBuilder.Message(ERROR_ADD_DESIGNATION_EX, xmlDesignation.Name), Ex);
				return null;
			}
			
		}
		
		/// <summary>This method is called to save the current selection as a designation(s)</summary>
		/// <param name="bInsert">true to insert designations into the current scene</param>
		/// <param name="bBefore">true to insert designations before the current scene</param>
		private bool AddSelection(bool bInsert, bool bBefore)
		{
			Array				aSelections = null;
			CXmlDesignations	xmlDesignations = null;
			string				strMsg = "";
			CTmaxItems			tmaxAdded = null;
			CDxTertiary			dxDesignation = null;
			bool				bSuccessful = false;
			
			Debug.Assert(m_dxScript != null);
			if(m_dxScript == null) return false;

			try
			{
				//	Get the current selections
				if((aSelections = m_transGrid.GetSelectedRows()) == null) return false;

				//	Get the designations that will result from the selection
				if((xmlDesignations = CreateDesignations(aSelections)) == null) return false;
				if(xmlDesignations.Count == 0) return false;

				//	Did the selections break across segments?
				if(xmlDesignations.Count > 1)
				{
					strMsg = String.Format("The current selection will result in {0} designations\n\n", xmlDesignations.Count);
					
					foreach(CXmlDesignation O in xmlDesignations)
						strMsg += (O.Name + "\n");
						
					strMsg += "\nDo you want to continue?";
					
					//	Prompt the user for confirmation before continuing
					if(MessageBox.Show(strMsg, "Please Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
						return false;
				}
					
				//	Allocate a temporary collection to hold event items that
				//	represent the new designations
				tmaxAdded = new CTmaxItems();
				
				//	Add each of the designations
				foreach(CXmlDesignation O in xmlDesignations)
				{
					if((dxDesignation = AddDesignation(O)) != null)
					{
						tmaxAdded.Add(new CTmaxItem(dxDesignation));
						
						//	Close the designation file
						O.Close(true);
					}
					
				}
				
				//	Did we add any designations?
				if(tmaxAdded.Count > 0)
				{
					//	Add new scenes to the script
					AddScenes(tmaxAdded, bInsert, bBefore);
				}
				
				//	Clear the current selection
				m_transGrid.SetSelection(null, true);
				
				bSuccessful = true;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddScenes", m_tmaxErrorBuilder.Message(ERROR_ADD_SCENES_EX), Ex);
			}				
			
			//	Clean up
			if(tmaxAdded != null)
			{
				tmaxAdded.Clear();
				tmaxAdded = null;
			}
			
			return bSuccessful;
			
		}// private void AddSelection(bool bInsert, bool bBefore)
		
		/// <summary>This method is called to add new scenes to the active script</summary>
		/// <param name="tmaxDesignations">the collection of designations to be added as new scenes</param>
		/// <param name="bInsert">true to insert, false to add</param>
		/// <param name="bBefore">true to insert designations before the current scene</param>
		/// <returns>true if successful</returns>
		private bool AddScenes(CTmaxItems tmaxDesignations, bool bInsert, bool bBefore)
		{
			CTmaxItem			tmaxScript = null;
			CTmaxParameters		tmaxParameters = null;
			
			Debug.Assert(tmaxDesignations != null);
			Debug.Assert(tmaxDesignations.Count > 0);
			Debug.Assert(m_dxScript != null);
			if(tmaxDesignations == null) return false;
			if(tmaxDesignations.Count == 0) return false;
			if(m_dxScript == null) return false;
			
			try
			{
				//	Reset this record so that we can determine the first new
				//	scene to be added
				m_dxAdded = null;
				
				//	Create an event item for the parent script
				tmaxScript = new CTmaxItem(m_dxScript);
					
				//	Assign the source items
				tmaxScript.SourceItems = tmaxDesignations;
					
				//	Create the required parameters for the event
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.Activate, true);
					
				//	Are we inserting into the script
				if((bInsert == true) && (m_dxScene != null))
				{
					Debug.Assert(ReferenceEquals(m_dxScene.Primary, m_dxScript) == true);
					
					//	Create the required parameters for the event
					tmaxParameters.Add(TmaxCommandParameters.Before, bBefore);
					
					//	Put the insertion point in the subitem collection
					tmaxScript.SubItems.Add(new CTmaxItem(m_dxScene));
				}

				//	Fire the event
				FireCommand(TmaxCommands.Add, tmaxScript, tmaxParameters);
				
				return true;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddScenes", m_tmaxErrorBuilder.Message(ERROR_ADD_SCENES_EX), Ex);
				return false;
			}				

		}// private void AddSelection(bool bBefore)
		
		/// <summary>This method is called to preview the current selection as a designation(s)</summary>
		public bool PreviewSelection()
		{
			Array				aSelections = null;
			CXmlDesignations	xmlDesignations = null;
			string				strMsg = "";
			string				strFileSpec = "";
			bool				bSuccessful = false;
			CXmlDesignation		xmlDesignation = null;
			CDxSecondary		dxSegment = null;
			
			try
			{
				//	Get the current selections
				if((aSelections = m_transGrid.GetSelectedRows()) == null) return false;

				//	Get the designations that will result from the selection
				if((xmlDesignations = CreateDesignations(aSelections)) == null) return false;
				if(xmlDesignations.Count == 0) return false;
				
				//	Did the selections break across segments?
				if(xmlDesignations.Count > 1)
				{
					strMsg = String.Format("The current selection will result in {0} designations. Preview will only show the first designation\n\n", xmlDesignations.Count);
					
					foreach(CXmlDesignation O in xmlDesignations)
						strMsg += (O.Name + "\n");
						
					strMsg += "\nDo you want to continue?";
					
					//	Prompt the user for confirmation before continuing
					if(MessageBox.Show(strMsg, "Please Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
						return false;
				}
					
				//	Get the preview objects
				xmlDesignation = xmlDesignations[0];
				dxSegment = (CDxSecondary)(xmlDesignations[0].UserData);
				Debug.Assert(xmlDesignation != null);
				Debug.Assert(dxSegment != null);
				if((xmlDesignation == null) || (dxSegment == null)) return false;
				
				//	Make sure the designation contains synchronized text
				if(xmlDesignation.GetSynchronized(false) == false)
				{
					strMsg = String.Format("Unable to start the preview. The selection does not contain synchronized text.");
					MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return false;
				}

				//	Get the path for the video from the database
				strFileSpec = m_tmaxDatabase.GetFileSpec(dxSegment);
				if((strFileSpec == null) || (strFileSpec.Length == 0))
					return false;
					
				//	Make sure the file exists
				if(System.IO.File.Exists(strFileSpec) == false)
				{
					strMsg = String.Format("Unable to start the preview. {0} could not be found", strFileSpec);
					MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return false;
				}

				//	Store these members locally so that we can process events from the preview form
				m_xmlPreviewDesignation = xmlDesignation;
				m_aPreviewSelections = aSelections;
				m_iPreviewRow = -1;
				
				//	Inhibit grid events until we're done
				m_bIgnoreGridEvents = true;
				
				//	Clear the current selections
				m_transGrid.SetSelections(null);
				
				//	Create a new preview form
				CFPreview preview = new CFPreview();
				m_tmaxEventSource.Attach(preview.EventSource);
				preview.Player.TmaxVideoCtrlEvent += new FTI.Trialmax.Controls.TmaxVideoCtrlHandler(this.OnTmaxVideoCtrlEvent);
				
				preview.SetProperties(strFileSpec, xmlDesignation);
				
				preview.ShowDialog();
				
				//	Restore the selections
				m_transGrid.SetSelections(m_aPreviewSelections);
					
				return true;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "PreviewSelection", m_tmaxErrorBuilder.Message(ERROR_PREVIEW_SELECTION_EX), Ex);
			}
			finally
			{
				m_xmlPreviewDesignation = null;
				m_aPreviewSelections = null;
				m_iPreviewRow = -1;
				m_bIgnoreGridEvents = false;
			}				
			
			return bSuccessful;
			
		}// private void AddSelection(bool bInsert, bool bBefore)

        /// <summary>This method is called to preview the current selection and 4 lines before and after selection as a designation(s)</summary>
        public bool PreviewSelectionResultPane()
        {
            Array aSelections = null;
            CXmlDesignations xmlDesignations = null;
            string strMsg = "";
            string strFileSpec = "";
            bool bSuccessful = false;
            CXmlDesignation xmlDesignation = null;
            CDxSecondary dxSegment = null;

            try
            {

                //	Get the current selections
                if ((aSelections = m_transGrid.GetSelectedRowsForResultPane()) == null) return false;

                //	Get the designations that will result from the selection
                if ((xmlDesignations = CreateDesignations(aSelections)) == null) return false;
                if (xmlDesignations.Count == 0) return false;

                //	Did the selections break across segments?
                if (xmlDesignations.Count > 1)
                {
                    strMsg = String.Format("The current selection will result in {0} designations. Preview will only show the first designation\n\n", xmlDesignations.Count);

                    foreach (CXmlDesignation O in xmlDesignations)
                        strMsg += (O.Name + "\n");

                    strMsg += "\nDo you want to continue?";

                    //	Prompt the user for confirmation before continuing
                    if (MessageBox.Show(strMsg, "Please Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return false;
                }

                //	Get the preview objects
                xmlDesignation = xmlDesignations[0];
                dxSegment = (CDxSecondary)(xmlDesignations[0].UserData);
                Debug.Assert(xmlDesignation != null);
                Debug.Assert(dxSegment != null);
                if ((xmlDesignation == null) || (dxSegment == null)) return false;

                //	Make sure the designation contains synchronized text
                if (xmlDesignation.GetSynchronized(false) == false)
                {
                    strMsg = String.Format("Unable to start the preview. The selection does not contain synchronized text.");
                    MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                //	Get the path for the video from the database
                strFileSpec = m_tmaxDatabase.GetFileSpec(dxSegment);
                if ((strFileSpec == null) || (strFileSpec.Length == 0))
                    return false;

                //	Make sure the file exists
                if (System.IO.File.Exists(strFileSpec) == false)
                {
                    strMsg = String.Format("Unable to start the preview. {0} could not be found", strFileSpec);
                    MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                //	Store these members locally so that we can process events from the preview form
                m_xmlPreviewDesignation = xmlDesignation;
                m_aPreviewSelections = aSelections;
                m_iPreviewRow = -1;

                //	Inhibit grid events until we're done
                m_bIgnoreGridEvents = true;

                //	Clear the current selections
                m_transGrid.SetSelections(null);

                //	Create a new preview form
                CFPreview preview = new CFPreview();
                m_tmaxEventSource.Attach(preview.EventSource);
                preview.Player.TmaxVideoCtrlEvent += new FTI.Trialmax.Controls.TmaxVideoCtrlHandler(this.OnTmaxVideoCtrlEvent);

                preview.SetProperties(strFileSpec, xmlDesignation);

                preview.ShowDialog();

                //	Restore the selections
                m_transGrid.SetSelections(m_aPreviewSelections);


                return true;

            }
            catch (System.Exception Ex)
            {
                m_tmaxEventSource.FireError(this, "PreviewSelection", m_tmaxErrorBuilder.Message(ERROR_PREVIEW_SELECTION_EX), Ex);
            }
            finally
            {
                m_xmlPreviewDesignation = null;
                m_aPreviewSelections = null;
                m_iPreviewRow = -1;
                m_bIgnoreGridEvents = false;
            }

            return bSuccessful;

        }// private void AddSelection(bool bInsert, bool bBefore)

		/// <summary>Called to set the treshold used to show Pause indicators</summary>
		private void SetPauseThreshold()
		{
			m_dPauseThreshold = 0;
			
			if(m_tmaxStationOptions != null)
			{
				try { m_dPauseThreshold = (double)(m_tmaxStationOptions.PauseThreshold); }
				catch {};
			}
			
			m_transGrid.PauseThreshold = m_dPauseThreshold;
			
		}// private void SetPauseThreshold()
		
		/// <summary>This method is called to get the segment record for the specified row</summary>
		/// <param name="row">The row used to identify the segment</param>
		/// <returns>the segment record if found</returns>
		private CDxSecondary GetSegment(CTmaxTransGridRow row)
		{
			CDxSecondary dxSegment = null;

			if(m_dxDeposition == null) return null;
			if(m_dxDeposition.Secondaries == null) return null;
			if(m_dxDeposition.Secondaries.Count == 0) return null;
			
			//	Locate the appropriate segment
			foreach(CDxSecondary O in m_dxDeposition.Secondaries)
			{
				if((O.GetExtent() != null) && (O.GetExtent().XmlSegmentId == row.GetSegment()))
				{
					dxSegment = O;
					break;
				}
							
			}

			return dxSegment;

		}// private CDxSecondary GetSegment(CTmaxTransGridRow row)
		
		/// <summary>This method is called to determine if the specified edit command should be enabled</summary>
		/// <param name="eCommand">The transcript pane command enumeration</param>
		/// <param name="lSelections">The number of lines in the current selection</param>
		/// <param name="lSelectedStart">The composite PL value of the first selected line</param>
		/// <param name="lSelectedStop">The composite PL value of the last selected line</param>
		/// <returns>true if command should be enabled</returns>
		private bool GetEditCommandEnabled(TranscriptPaneCommands eCommand, long lSelections, long lSelectedStart, long lSelectedStop)
		{
			CDxTertiary dxDesignation = null;
			long		lStartPL = 0;
			long		lStopPL = 0;
			
			//	All edit mode commands require at least one selected line
			if(lSelections <= 0) return false;
				
			//	There must be an active designation
			if((dxDesignation = GetDesignation()) == null) return false;
			
			//	What is the range defined by the designation
			lStartPL = dxDesignation.GetExtent().StartPL;
			lStopPL = dxDesignation.GetExtent().StopPL;
			
			//	What is the command?
			switch(eCommand)
			{
				case TranscriptPaneCommands.EditExtents:
				
					return true;
					
				case TranscriptPaneCommands.Exclude:
				
					//	Must be a portion of the designation selected
					if(lSelectedStart <= lStartPL)
					{
						return ((lSelectedStop >= lStartPL) && (lSelectedStop < lStopPL));
					}
					else
					{
						return (lSelectedStart <= lStopPL);
					}
						
				case TranscriptPaneCommands.SplitBefore:
				
					//	Must be a portion selected after the start of the designation
					return ((lSelectedStart > lStartPL) && (lSelectedStart <= lStopPL));
						
				case TranscriptPaneCommands.SplitAfter:
				
					//	Must be a portion selected before the end of the designation
					return ((lSelectedStop < lStopPL) && (lSelectedStop >= lStartPL));
						
				default:
				
					break;
			}	
			
			return false;
		
		}// private bool GetEditCommandEnabled(TranscriptPaneCommands eCommand, long lSelections, long lSelectedStart, long lSelectedStop)
		
		/// <summary>This method fires the requested EditDesignation command</summary>
		/// <param name="eMethod">The enumerated edit method to be passed with the event</param>
		/// <param name="lStartPL">The composite Page/Line start position</param>
		/// <param name="lStopPL">The composite Page/Line stop position</param>
		///	<returns>True if successful</returns>
		private bool FireEditCommand(TmaxDesignationEditMethods eMethod, long lStartPL, long lStopPL)
		{
			CTmaxParameters	tmaxParameters = null;
			CTmaxItem		tmaxItem = null;
			
			if(m_dxScene == null) return false;
			
			try
			{
				//	Allocate and initialize the event item
				//
				//	NOTE:	The database requires the active scene (instead of the designation)
				tmaxItem = new CTmaxItem(m_dxScene);
				
				//	Populate the parameters collection
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.EditMethod, (int)eMethod);
				tmaxParameters.Add(TmaxCommandParameters.StartPL, lStartPL);
				tmaxParameters.Add(TmaxCommandParameters.StopPL, lStopPL);
				
				//	Fire the command event
				return FireCommand(TmaxCommands.EditDesignation, tmaxItem, tmaxParameters);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FireEditCommand", m_tmaxErrorBuilder.Message(ERROR_FIRE_EDIT_COMMAND_EX, eMethod.ToString(), lStartPL, lStopPL), Ex);
				return false;
			}
			
		}// private bool FireEditCommand(TmaxDesignationEditMethods eMethod, long lStartPL, long lStopPL)

		/// <summary>Called to fire the command event to indicate that the user has selected a new deposition</summary>
		/// <returns>true if successful</returns>
		private bool FireSetDeposition(CDxPrimary dxDeposition)
		{
			bool		bSuccessful = false;
			CTmaxItem	tmaxItem = null;

			try
			{
				//	Allocate an event item to represent the deposition
				tmaxItem = new CTmaxItem();
				if(dxDeposition != null)
					tmaxItem.SetRecord(dxDeposition);

				bSuccessful = FireCommand(TmaxCommands.SetDeposition, tmaxItem);
			}
			catch(System.Exception Ex)
			{
				if(dxDeposition != null)
					m_tmaxEventSource.FireError(this, "FireSetDeposition", m_tmaxErrorBuilder.Message(ERROR_FIRE_SET_DEPOSITION_EX, dxDeposition.GetBarcode(false)), Ex);
				else
					m_tmaxEventSource.FireError(this, "FireSetDeposition", m_tmaxErrorBuilder.Message(ERROR_FIRE_SET_DEPOSITION_EX, "NULL"), Ex);
			}

			return bSuccessful;

		}// private bool FireSetDeposition(CDxPrimary dxDeposition)

		/// <summary>Handles ValidateBarcode events fired by the AddLink form</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="xmlLink">The link being configured by the user</param>
		/// <returns>true if the barcode is linked to valid media</returns>
		private bool OnAddLinkValidate(object sender, CXmlLink xmlLink)
		{
			bool			bSuccessful = true;
			CDxMediaRecord	dxSource = null;
			
			try
			{
				//	Do we need to check the barcode?
				if((xmlLink!= null) && (xmlLink.Hide == false) && (xmlLink.SourceMediaId.Length > 0))
				{
					//	Attempt the get the associated record
					if((dxSource = m_tmaxDatabase.GetRecordFromBarcode(xmlLink.SourceMediaId, true, false)) != null)
					{
						switch(dxSource.MediaType)
						{
							case TmaxMediaTypes.Page:
							case TmaxMediaTypes.Slide:
							
								//	Assign the database identifier
								xmlLink.SourceDbId = dxSource.GetUniqueId();
								break;

							case TmaxMediaTypes.Treatment:
	
								//	Can't link to a split screen treatment
								if(((CDxTertiary)dxSource).SplitScreen == true)
								{
									Warn("Links to split screen treatments are not permitted.");
									bSuccessful = false;
								}
								else
								{
									xmlLink.SourceDbId = dxSource.GetUniqueId();
								}
								break;

							default:
							
								//	All other types are invalid
								Warn("Only pages, treatments, and slides can be linked to video");
								bSuccessful = false;
								break;

						}//	switch(dxSource.MediaType)
					
					}
					else
					{
						Warn(xmlLink.SourceMediaId + " is not a valid barcode");
						bSuccessful = false;
						
					}// if((dxSource = m_tmaxDatabase.GetRecordFromBarcode(xmlLink.SourceMediaId, true, false)) != null)

				}// if((xmlLink!= null) && (xmlLink.Hide == false) && (xmlLink.SourceMediaId.Length > 0))

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnAddLinkValidate", m_tmaxErrorBuilder.Message(ERROR_ON_ADD_LINK_VALIDATE_EX), Ex);
				bSuccessful = false;
			}
			
			return bSuccessful;

		}// private bool OnAddLinkValidate(object sender, CXmlLink xmlLink)

		#endregion Private Methods
		
		#region Protected Methods
		
		/// <summary>Overridden base class member to perform custom message processing</summary>
		/// <param name="m">the message being sent to the window</param>
		protected override void DefWndProc(ref Message m)
		{
			//	Perform the base class processing
			base.DefWndProc (ref m);
			
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
		
		}// protected override void DefWndProc(ref Message m)

		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			base.SetErrorStrings();
			
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to load the deposition's transcript: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to load the transcript. %1 could not be found.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to process the double click notification");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the transcripts list box.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the highlighters list box.");
			
			m_tmaxErrorBuilder.FormatStrings.Add("The transcript file does not contain any text. filename = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to retrieve the transcript record associated with the primary deposition: MediaId = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to retrieve a valid transcript file specification for the primary deposition: MediaId = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the transcripts list selection");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the highlighters list selection");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a selection to the transcripts list.");
			m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the specified segment: XML segment key = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create a new designation.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a new designation: Name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add designations for the current selection");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add script scenes for the new designations");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to preview the current transcript selections");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to activate the specified record: %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to go to the specified page/line");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to fill the grid with the new transcript: %1");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to edit the designation: method = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fire an EditDesignation command: method = %1 StartPL = %2 StopPL = %3");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised when objections were added to the database.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised when objections were deleted.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised when objections were updated.");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while firing the event to set the active deposition: barcode = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while firing the event to add the link to the database");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to create an XML link");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to validate the link settings");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to open the active transcript: designation = %1");

		}// private override void SetErrorStrings()

		/// <summary>This function is called when the value of the Database property changes</summary>
		protected override void OnDatabaseChanged()
		{
			CDxPrimary dxDeposition = null;
			
			//	Unload the viewer whenever the database changes
			Unload();
			
			//	Records are no longer valid
			m_dxScript = null;
			m_dxScene = null;
			m_dxActivate = null;
			m_dxHighlighter = null;
			m_dxDeposition = null;
			m_dxDesignationScenes.Clear();
			m_xmlPreviewDesignation = null;
			m_dxAdded = null;
			
			//	Reset the comboboxes
			FillTranscripts();
			FillHighlighters();
			
			//	Enable/disable commands
			OnContextMenu(null);
			
			//	Update the status bar
			SetStatusText();
			
			//	Reload the last deposition
			if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null))
			{
				if((m_tmaxStationOptions != null) && (m_tmaxStationOptions.LastDeposition > 0))
				{
					if((dxDeposition = m_tmaxDatabase.Primaries.Find(m_tmaxStationOptions.LastDeposition)) != null)
					{
						Activate(new CTmaxItem(dxDeposition), TmaxAppPanes.Transcripts);
					}
					else
					{
						m_tmaxStationOptions.LastDeposition = -1;
					}
				}
				
			}
						
		}// OnDatabaseChanged()
		
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected override void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("MainToolbar");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("PinTranscript", "");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("TranscriptsLabel");
			Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool1 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("Transcripts");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PreviewSelection");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Find");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("GoPageLine");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AddSelection");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertSelectionBefore");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertSelectionAfter");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("HighlightersLabel");
			Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool2 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("Highlighters");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PreviewSelection");
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AddSelection");
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AssignHighlighter");
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ContextMenu");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("PinTranscript", "");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AddObjection");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RepeatObjection");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AddSelection");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertSelectionBefore");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertSelectionAfter");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool36 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AddLink");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PreviewSelection");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool40 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CopySelection");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool41 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PrintSelection");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool42 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SaveSelection");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SplitBefore");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SplitAfter");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exclude");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("EditExtents");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("SetHighlighter");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AssignHighlighter");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Find");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool22 = new Infragistics.Win.UltraWinToolbars.ButtonTool("GoPageLine");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool23 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Update");
			Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool3 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("Highlighters");
			Infragistics.Win.ValueList valueList1 = new Infragistics.Win.ValueList(0);
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool3 = new Infragistics.Win.UltraWinToolbars.LabelTool("HighlightersLabel");
			Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool4 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("Transcripts");
			Infragistics.Win.ValueList valueList2 = new Infragistics.Win.ValueList(0);
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool4 = new Infragistics.Win.UltraWinToolbars.LabelTool("TranscriptsLabel");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool3 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("PinTranscript", "");
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool5 = new Infragistics.Win.UltraWinToolbars.LabelTool("Spacer");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool24 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertSelectionBefore");
			Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool25 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertSelectionAfter");
			Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool3 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("SetHighlighter");
			Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
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
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool26 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Find");
			Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool27 = new Infragistics.Win.UltraWinToolbars.ButtonTool("GoPageLine");
			Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool28 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Update");
			Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool29 = new Infragistics.Win.UltraWinToolbars.ButtonTool("EditExtents");
			Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool30 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SplitBefore");
			Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool31 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SplitAfter");
			Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool32 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exclude");
			Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool33 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AddObjection");
			Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool34 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RepeatObjection");
			Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool35 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AddLink");
			Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool37 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CopySelection");
			Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool38 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SaveSelection");
			Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool39 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PrintSelection");
			Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool43 = new Infragistics.Win.UltraWinToolbars.ButtonTool("OpenProperties");
			Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool44 = new Infragistics.Win.UltraWinToolbars.ButtonTool("OpenPresentation");
			Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool45 = new Infragistics.Win.UltraWinToolbars.ButtonTool("OpenCodes");
			Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool4 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Open");
			Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool46 = new Infragistics.Win.UltraWinToolbars.ButtonTool("OpenProperties");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool47 = new Infragistics.Win.UltraWinToolbars.ButtonTool("OpenPresentation");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool48 = new Infragistics.Win.UltraWinToolbars.ButtonTool("OpenCodes");
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CTranscriptPane));
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinStatusBar.UltraStatusPanel ultraStatusPanel2 = new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel();
			this.m_ultraToolbarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
			this.m_ctrlToolbarImages = new System.Windows.Forms.ImageList(this.components);
			this.m_ctrlFillPanel = new System.Windows.Forms.Panel();
			this.m_transGrid = new FTI.Trialmax.Controls.CTmaxTransGridCtrl();
			this.m_ctrlStatusBar = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
			this._CTranscriptPane_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTranscriptPane_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTranscriptPane_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTranscriptPane_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			((System.ComponentModel.ISupportInitialize)(this.m_ultraToolbarManager)).BeginInit();
			this.m_ctrlFillPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_ultraToolbarManager
			// 
			this.m_ultraToolbarManager.DesignerFlags = 1;
			this.m_ultraToolbarManager.DockWithinContainer = this;
			this.m_ultraToolbarManager.ImageListSmall = this.m_ctrlToolbarImages;
			this.m_ultraToolbarManager.ShowFullMenusDelay = 500;
			this.m_ultraToolbarManager.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.Office2003;
			ultraToolbar1.DockedColumn = 0;
			ultraToolbar1.DockedRow = 0;
			stateButtonTool1.InstanceProps.IsFirstInGroup = true;
			labelTool1.InstanceProps.Width = 67;
			comboBoxTool1.InstanceProps.Width = 128;
			buttonTool4.InstanceProps.IsFirstInGroup = true;
			labelTool2.InstanceProps.Width = 72;
			comboBoxTool2.InstanceProps.Width = 134;
			ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool1,
            labelTool1,
            comboBoxTool1,
            buttonTool1,
            buttonTool2,
            buttonTool3,
            buttonTool4,
            buttonTool5,
            buttonTool6,
            labelTool2,
            comboBoxTool2});
			ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.FillEntireRow = Infragistics.Win.DefaultableBoolean.True;
			ultraToolbar1.Settings.GrabHandleStyle = Infragistics.Win.UltraWinToolbars.GrabHandleStyle.None;
			ultraToolbar1.Settings.ToolSpacing = -2;
			ultraToolbar1.ShowInToolbarList = false;
			ultraToolbar1.Text = "MainToolbar";
			this.m_ultraToolbarManager.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
			appearance3.Image = 0;
			buttonTool7.SharedProps.AppearancesSmall.Appearance = appearance3;
			buttonTool7.SharedProps.Caption = "Preview Selection ...";
			appearance4.Image = 4;
			buttonTool8.SharedProps.AppearancesSmall.Appearance = appearance4;
			buttonTool8.SharedProps.Caption = "Add Designation";
			buttonTool8.SharedProps.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			buttonTool8.SharedProps.ToolTipText = "Add Selection (Shortcut = Ctrl+S)";
			appearance5.Image = 2;
			buttonTool9.SharedProps.AppearancesSmall.Appearance = appearance5;
			buttonTool9.SharedProps.Caption = "Assign Highlighters ...";
			popupMenuTool1.SharedProps.Caption = "ContextMenu";
			stateButtonTool2.InstanceProps.IsFirstInGroup = true;
			buttonTool10.InstanceProps.IsFirstInGroup = true;
			buttonTool12.InstanceProps.IsFirstInGroup = true;
			buttonTool36.InstanceProps.IsFirstInGroup = true;
			buttonTool15.InstanceProps.IsFirstInGroup = true;
			buttonTool16.InstanceProps.IsFirstInGroup = true;
			popupMenuTool2.InstanceProps.IsFirstInGroup = true;
			buttonTool21.InstanceProps.IsFirstInGroup = true;
			buttonTool23.InstanceProps.IsFirstInGroup = true;
			popupMenuTool1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool2,
            buttonTool10,
            buttonTool11,
            buttonTool12,
            buttonTool13,
            buttonTool14,
            buttonTool36,
            buttonTool15,
            buttonTool40,
            buttonTool41,
            buttonTool42,
            buttonTool16,
            buttonTool17,
            buttonTool18,
            buttonTool19,
            popupMenuTool2,
            buttonTool20,
            buttonTool21,
            buttonTool22,
            buttonTool23});
			comboBoxTool3.SharedProps.Caption = "Highlighters";
			comboBoxTool3.ValueList = valueList1;
			labelTool3.SharedProps.Caption = "Highlighter: ";
			labelTool3.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
			comboBoxTool4.SharedProps.Caption = "Transcripts";
			comboBoxTool4.ValueList = valueList2;
			labelTool4.SharedProps.Caption = "Transcript:";
			labelTool4.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
			labelTool4.SharedProps.ShowInCustomizer = false;
			appearance6.Image = 3;
			stateButtonTool3.SharedProps.AppearancesSmall.Appearance = appearance6;
			stateButtonTool3.SharedProps.Caption = "Pin Transcript";
			appearance7.Image = 5;
			buttonTool24.SharedProps.AppearancesSmall.Appearance = appearance7;
			buttonTool24.SharedProps.Caption = "Insert Designation Before";
			appearance8.Image = 6;
			buttonTool25.SharedProps.AppearancesSmall.Appearance = appearance8;
			buttonTool25.SharedProps.Caption = "Insert Designation After";
			appearance9.Image = 1;
			popupMenuTool3.SharedProps.AppearancesSmall.Appearance = appearance9;
			popupMenuTool3.SharedProps.Caption = "Select Active Highlighter";
			stateButtonTool4.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool5.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool6.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool7.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool8.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool9.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool10.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			popupMenuTool3.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool4,
            stateButtonTool5,
            stateButtonTool6,
            stateButtonTool7,
            stateButtonTool8,
            stateButtonTool9,
            stateButtonTool10});
			stateButtonTool11.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool11.SharedProps.Caption = "SetHighlighter1";
			stateButtonTool12.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool12.SharedProps.Caption = "SetHighlighter2";
			stateButtonTool13.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool13.SharedProps.Caption = "SetHighlighter3";
			stateButtonTool14.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool14.SharedProps.Caption = "SetHighlighter4";
			stateButtonTool15.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool15.SharedProps.Caption = "SetHighlighter5";
			stateButtonTool16.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool16.SharedProps.Caption = "SetHighlighter6";
			stateButtonTool17.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool17.SharedProps.Caption = "SetHighlighter7";
			appearance10.Image = 8;
			buttonTool26.SharedProps.AppearancesSmall.Appearance = appearance10;
			buttonTool26.SharedProps.Caption = "Find ...";
			buttonTool26.SharedProps.ToolTipText = "Find (Shortcut = Ctrl+F)";
			appearance11.Image = 9;
			buttonTool27.SharedProps.AppearancesSmall.Appearance = appearance11;
			buttonTool27.SharedProps.Caption = "Go To ...";
			buttonTool27.SharedProps.Shortcut = System.Windows.Forms.Shortcut.CtrlG;
			buttonTool27.SharedProps.ToolTipText = "Go To (Shortcut = Ctrl+G)";
			appearance12.Image = 10;
			buttonTool28.SharedProps.AppearancesSmall.Appearance = appearance12;
			buttonTool28.SharedProps.Caption = "Update Transcript From Log";
			appearance13.Image = 11;
			buttonTool29.SharedProps.AppearancesSmall.Appearance = appearance13;
			buttonTool29.SharedProps.Caption = "&Edit Extents";
			appearance14.Image = 14;
			buttonTool30.SharedProps.AppearancesSmall.Appearance = appearance14;
			buttonTool30.SharedProps.Caption = "Split &Before";
			appearance15.Image = 13;
			buttonTool31.SharedProps.AppearancesSmall.Appearance = appearance15;
			buttonTool31.SharedProps.Caption = "Split &After";
			appearance16.Image = 12;
			buttonTool32.SharedProps.AppearancesSmall.Appearance = appearance16;
			buttonTool32.SharedProps.Caption = "E&xclude";
			appearance17.Image = 15;
			buttonTool33.SharedProps.AppearancesSmall.Appearance = appearance17;
			buttonTool33.SharedProps.Caption = "Add Objection ...";
			appearance18.Image = 16;
			buttonTool34.SharedProps.AppearancesSmall.Appearance = appearance18;
			buttonTool34.SharedProps.Caption = "Repeat Objection ...";
			appearance29.Image = 17;
			buttonTool35.SharedProps.AppearancesSmall.Appearance = appearance29;
			buttonTool35.SharedProps.Caption = "Add Link ...";
			appearance30.Image = 20;
			buttonTool37.SharedProps.AppearancesSmall.Appearance = appearance30;
			buttonTool37.SharedProps.Caption = "Copy Selection ...";
			appearance31.Image = 18;
			buttonTool38.SharedProps.AppearancesSmall.Appearance = appearance31;
			buttonTool38.SharedProps.Caption = "Save Selection As...";
			appearance32.Image = 19;
			buttonTool39.SharedProps.AppearancesSmall.Appearance = appearance32;
			buttonTool39.SharedProps.Caption = "Print Selection ...";
			appearance33.Image = 22;
			buttonTool43.SharedProps.AppearancesSmall.Appearance = appearance33;
			buttonTool43.SharedProps.Caption = "Open in Properties";
			appearance34.Image = 21;
			buttonTool44.SharedProps.AppearancesSmall.Appearance = appearance34;
			buttonTool44.SharedProps.Caption = "Open in Presentation";
			appearance35.Image = 23;
			buttonTool45.SharedProps.AppearancesSmall.Appearance = appearance35;
			buttonTool45.SharedProps.Caption = "Open in Fielded Data";
			appearance36.Image = 24;
			popupMenuTool4.SharedProps.AppearancesSmall.Appearance = appearance36;
			popupMenuTool4.SharedProps.Caption = "Open";
			popupMenuTool4.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool46,
            buttonTool47,
            buttonTool48});
			this.m_ultraToolbarManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool7,
            buttonTool8,
            buttonTool9,
            popupMenuTool1,
            comboBoxTool3,
            labelTool3,
            comboBoxTool4,
            labelTool4,
            stateButtonTool3,
            labelTool5,
            buttonTool24,
            buttonTool25,
            popupMenuTool3,
            stateButtonTool11,
            stateButtonTool12,
            stateButtonTool13,
            stateButtonTool14,
            stateButtonTool15,
            stateButtonTool16,
            stateButtonTool17,
            buttonTool26,
            buttonTool27,
            buttonTool28,
            buttonTool29,
            buttonTool30,
            buttonTool31,
            buttonTool32,
            buttonTool33,
            buttonTool34,
            buttonTool35,
            buttonTool37,
            buttonTool38,
            buttonTool39,
            buttonTool43,
            buttonTool44,
            buttonTool45,
            popupMenuTool4});
			this.m_ultraToolbarManager.ToolValueChanged += new Infragistics.Win.UltraWinToolbars.ToolEventHandler(this.OnUltraToolValueChanged);
			this.m_ultraToolbarManager.BeforeToolDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventHandler(this.OnUltraBeforeDropDown);
			this.m_ultraToolbarManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.OnUltraToolClick);
			this.m_ultraToolbarManager.AfterToolCloseup += new Infragistics.Win.UltraWinToolbars.ToolDropdownEventHandler(this.OnUltraAfterCloseup);
			this.m_ultraToolbarManager.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.OnUltraBeforeToolbarListDropdown);
			// 
			// m_ctrlToolbarImages
			// 
			this.m_ctrlToolbarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlToolbarImages.ImageStream")));
			this.m_ctrlToolbarImages.TransparentColor = System.Drawing.Color.Magenta;
			this.m_ctrlToolbarImages.Images.SetKeyName(0, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(1, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(2, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(3, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(4, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(5, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(6, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(7, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(8, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(9, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(10, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(11, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(12, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(13, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(14, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(15, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(16, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(17, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(18, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(19, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(20, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(21, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(22, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(23, "");
			this.m_ctrlToolbarImages.Images.SetKeyName(24, "open_menu.bmp");
			// 
			// m_ctrlFillPanel
			// 
			this.m_ctrlFillPanel.Controls.Add(this.m_transGrid);
			this.m_ctrlFillPanel.Controls.Add(this.m_ctrlStatusBar);
			this.m_ctrlFillPanel.Cursor = System.Windows.Forms.Cursors.Default;
			this.m_ctrlFillPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlFillPanel.Location = new System.Drawing.Point(0, 73);
			this.m_ctrlFillPanel.Name = "m_ctrlFillPanel";
			this.m_ctrlFillPanel.Size = new System.Drawing.Size(528, 243);
			this.m_ctrlFillPanel.TabIndex = 0;
			// 
			// m_transGrid
			// 
			this.m_transGrid.BackColor = System.Drawing.Color.White;
			this.m_transGrid.DefendantColor = System.Drawing.Color.Blue;
			this.m_transGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_transGrid.Location = new System.Drawing.Point(0, 0);
			this.m_transGrid.Name = "m_transGrid";
			this.m_transGrid.PaneId = 0;
			this.m_transGrid.PauseThreshold = 0;
			this.m_transGrid.PlaintiffColor = System.Drawing.Color.Red;
			this.m_transGrid.Size = new System.Drawing.Size(528, 220);
			this.m_transGrid.TabIndex = 2;
			this.m_transGrid.XmlDeposition = null;
			this.m_transGrid.DblClick += new System.EventHandler(this.OnGridDblClick);
			this.m_transGrid.DragOver += new System.Windows.Forms.DragEventHandler(this.OnDragOver);
			this.m_transGrid.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
			this.m_transGrid.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
			this.m_transGrid.SelChanged += new System.EventHandler(this.OnGridSelChanged);
			this.m_transGrid.DragLeave += new System.EventHandler(this.OnDragLeave);
			// 
			// m_ctrlStatusBar
			// 
			appearance1.TextHAlignAsString = "Center";
			this.m_ctrlStatusBar.Appearance = appearance1;
			this.m_ctrlStatusBar.BorderStyle = Infragistics.Win.UIElementBorderStyle.InsetSoft;
			this.m_ctrlStatusBar.ImageList = this.m_ctrlToolbarImages;
			this.m_ctrlStatusBar.Location = new System.Drawing.Point(0, 220);
			this.m_ctrlStatusBar.Name = "m_ctrlStatusBar";
			appearance2.Image = 3;
			appearance2.ImageHAlign = Infragistics.Win.HAlign.Center;
			appearance2.ImageVAlign = Infragistics.Win.VAlign.Middle;
			ultraStatusPanel1.Appearance = appearance2;
			ultraStatusPanel1.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
			ultraStatusPanel1.Width = 20;
			ultraStatusPanel2.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
			ultraStatusPanel2.SizingMode = Infragistics.Win.UltraWinStatusBar.PanelSizingMode.Spring;
			this.m_ctrlStatusBar.Panels.AddRange(new Infragistics.Win.UltraWinStatusBar.UltraStatusPanel[] {
            ultraStatusPanel1,
            ultraStatusPanel2});
			this.m_ctrlStatusBar.Size = new System.Drawing.Size(528, 23);
			this.m_ctrlStatusBar.TabIndex = 0;
			this.m_ctrlStatusBar.Text = "Transcript Status Bar";
			// 
			// _CTranscriptPane_Toolbars_Dock_Area_Left
			// 
			this._CTranscriptPane_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTranscriptPane_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
			this._CTranscriptPane_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
			this._CTranscriptPane_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTranscriptPane_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 73);
			this._CTranscriptPane_Toolbars_Dock_Area_Left.Name = "_CTranscriptPane_Toolbars_Dock_Area_Left";
			this._CTranscriptPane_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 243);
			this._CTranscriptPane_Toolbars_Dock_Area_Left.ToolbarsManager = this.m_ultraToolbarManager;
			// 
			// _CTranscriptPane_Toolbars_Dock_Area_Right
			// 
			this._CTranscriptPane_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTranscriptPane_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
			this._CTranscriptPane_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
			this._CTranscriptPane_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTranscriptPane_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(528, 73);
			this._CTranscriptPane_Toolbars_Dock_Area_Right.Name = "_CTranscriptPane_Toolbars_Dock_Area_Right";
			this._CTranscriptPane_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 243);
			this._CTranscriptPane_Toolbars_Dock_Area_Right.ToolbarsManager = this.m_ultraToolbarManager;
			// 
			// _CTranscriptPane_Toolbars_Dock_Area_Top
			// 
			this._CTranscriptPane_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTranscriptPane_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
			this._CTranscriptPane_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
			this._CTranscriptPane_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTranscriptPane_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
			this._CTranscriptPane_Toolbars_Dock_Area_Top.Name = "_CTranscriptPane_Toolbars_Dock_Area_Top";
			this._CTranscriptPane_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(528, 73);
			this._CTranscriptPane_Toolbars_Dock_Area_Top.ToolbarsManager = this.m_ultraToolbarManager;
			// 
			// _CTranscriptPane_Toolbars_Dock_Area_Bottom
			// 
			this._CTranscriptPane_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTranscriptPane_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
			this._CTranscriptPane_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
			this._CTranscriptPane_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTranscriptPane_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 316);
			this._CTranscriptPane_Toolbars_Dock_Area_Bottom.Name = "_CTranscriptPane_Toolbars_Dock_Area_Bottom";
			this._CTranscriptPane_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(528, 0);
			this._CTranscriptPane_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.m_ultraToolbarManager;
			// 
			// CTranscriptPane
			// 
			this.m_ultraToolbarManager.SetContextMenuUltra(this, "ContextMenu");
			this.Controls.Add(this.m_ctrlFillPanel);
			this.Controls.Add(this._CTranscriptPane_Toolbars_Dock_Area_Left);
			this.Controls.Add(this._CTranscriptPane_Toolbars_Dock_Area_Right);
			this.Controls.Add(this._CTranscriptPane_Toolbars_Dock_Area_Top);
			this.Controls.Add(this._CTranscriptPane_Toolbars_Dock_Area_Bottom);
			this.Name = "CTranscriptPane";
			this.Size = new System.Drawing.Size(528, 316);
			((System.ComponentModel.ISupportInitialize)(this.m_ultraToolbarManager)).EndInit();
			this.m_ctrlFillPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		
		/// <summary>This function is called when the PaneVisible property changes</summary>
		protected override void OnPaneVisibleChanged()
		{
			//	Is this pane being activated by the user?
			if(m_bPaneVisible == true)
			{
				//	Is there a record waiting for activation?
				if(m_dxActivate != null)
				{
					SetRecord(m_dxActivate, false);
				}
				else if(ReferenceEquals(m_dxScript, m_dxDesignationScenes.Primary) == false)
				{
					//	Erase any existing highlights
					Erase(false);

					//	Add highlights for the new script
					if(m_dxScript != null)
						Highlight(m_dxScript);

					//	Enable/disable commands
					OnContextMenu(null);

					//	Update the status bar
					SetStatusText();

				}// if(m_dxActivate != null)

			}// if(m_bPaneVisible == true)

		}// protected override void OnPaneVisibleChanged()

		/// <summary>Overridden by derived classes to provide access to the pane's toolbar manager</summary>
		/// <returns>The pane's toolbar manager if available</returns>
		protected override Infragistics.Win.UltraWinToolbars.UltraToolbarsManager GetUltraToolbarManager()
		{
			return m_ultraToolbarManager;
		}

		/// <summary>This method traps all DragDrop events</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">System event arguements</param>
		protected override void OnDragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			CXmlLink xmlLink = null;
			
			try
			{
				//	Notify the application that this is the drop pane
				FireCommand(TmaxCommands.AcceptDrop);

				//	Do we have a the values we need to create the link?
				if((m_dxDropSource != null) && (m_dxDropDesignation != null) && (m_xmlDropLine != null))
				{
					//	Create the XML link required to add the record
					if((xmlLink = CreateXmlLink(m_xmlDropLine, m_dxDropDesignation, m_dxDropSource)) != null)
						AddLink(m_dxDropDesignation, xmlLink);
				}

//				m_tmaxEventSource.FireDiagnostic(this, "OnDragDrop", "Transcript drag drop");

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnDragDrop", Ex);
			}
			finally
			{
				//	Clear the drop target members
				m_dxDropDesignation = null;
				m_dxDropScene = null;
				m_dxDropSource = null;
				m_xmlDropLine = null;
			}

			//	Perform the base class processing
			base.OnDragDrop(sender, e);

		}// protected override void OnDragDrop(object sender, System.Windows.Forms.DragEventArgs e)

		/// <summary>This method traps all DragEnter events</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">System event arguements</param>
		protected override void OnDragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			CDxMediaRecord dxSource = null;
		
			try
			{
				//	Is the user dragging media records?
				if((m_eDragState == PaneDragStates.Records) && (m_tmaxDragData != null))
				{
					//	Only permit single record dragging
					if((m_tmaxDragData.SourceItems != null) && (m_tmaxDragData.SourceItems.Count == 1))
					{
						//	Get the media record being dragged by the user
						dxSource = (CDxMediaRecord)(m_tmaxDragData.SourceItems[0].GetMediaRecord());
					
					}// if((m_tmaxDragData.SourceItems != null) && (m_tmaxDragData.SourceItems.Count == 1))

				}// if((m_eDragState == PaneDragStates.Records) && (m_tmaxDragData != null))
				
				//	Assign the drag link
				SetDropSource(dxSource);
				
				//	Assign the drop target
				if((m_dxDropScene = m_dxScene) != null)
					m_dxDropDesignation = GetDesignation();
					
				//	Assign the current drop line
				//
				//	NOTE: This must be called AFTER setting the drop designation
				SetDropLine();
					
				//	Assign the drop effects
				e.Effect = GetDropEffects(e.X, e.Y);
							
//				m_tmaxEventSource.FireDiagnostic(this, "OnDragEnter", "Transcript drag enter");

			}
			catch(System.Exception Ex)
			{
				m_dxDropSource = null;
				e.Effect = DragDropEffects.None;

				m_tmaxEventSource.FireDiagnostic(this, "OnDragEnter", Ex);
			}

			//	Perform the base class processing
			base.OnDragEnter(sender, e);
			
		}// protected override void OnDragEnter(object sender, System.Windows.Forms.DragEventArgs e)

		/// <summary>This method traps all DragLeave events</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">System event arguements</param>
		protected override void OnDragLeave(object sender, System.EventArgs e)
		{
			try
			{
				//	Clear the drop target members
				m_dxDropDesignation = null;
				m_dxDropScene = null;
				m_dxDropSource = null;
				m_xmlDropLine = null;

//				m_tmaxEventSource.FireDiagnostic(this, "OnDragLeave", "Transcript drag leave");

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnDragLeave", Ex);
			}

			//	Perform the base class processing
			base.OnDragLeave(sender, e);

		}// protected override void OnDragLeave(object sender, System.EventArgs e)

		/// <summary>This method traps all DragOver events</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">System event arguements</param>
		protected override void OnDragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			try
			{
				//	Assign the current drop line
				SetDropLine();

				//	Assign the drop effects
				e.Effect = GetDropEffects(e.X, e.Y);

//				m_tmaxEventSource.FireDiagnostic(this, "OnDragOver", "Transcript drag enter");

			}
			catch(System.Exception Ex)
			{
				e.Effect = DragDropEffects.None;
				m_tmaxEventSource.FireDiagnostic(this, "OnDragOver", Ex);
			}

			//	Perform the base class processing
			base.OnDragOver(sender, e);

		}// protected override void OnDragOver(object sender, System.Windows.Forms.DragEventArgs e)

		#endregion Protected Methods

	}// public class CTranscriptPane : FTI.Trialmax.Panes.CBasePane

}// namespace FTI.Trialmax.Panes
