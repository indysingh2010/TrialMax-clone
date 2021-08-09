using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Messaging;
using System.IO;
using System.Xml;

using FTI.Shared.Trialmax;
using FTI.Trialmax.Controls;
using FTI.Trialmax.Forms;
using FTI.Trialmax.Database;
using FTI.Shared.Xml;
using FTI.Shared.Win32;
using FTI.Trialmax.Reports;

using Infragistics.Win;
using Infragistics.Shared;
using Infragistics.Win.UltraWinTree;
using Infragistics.Win.UltraWinDock;
using Infragistics.Win.UltraWinToolbars;

using SHOpenFolderAndSelectItems;

namespace FTI.Trialmax.Panes
{
	/// <summary>Summary description for Treepane</summary>
	public class CTreePane : FTI.Trialmax.Panes.CBasePane
	{
		/// <summary>Delegate used to fire asynchronous command events</summary>
		protected delegate void AsyncCommandDelegate();

        protected bool folderaccess = true;
		/// <summary>This structure groups the members required to manage a drop target</summary>
		protected struct SDropTarget
		{
			/// <summary>The drop position relative to the drop node</summary>
			public FTI.Trialmax.Controls.TmaxTreePositions ePosition;
		
			/// <summary>The current drop action</summary>
			public TreeDropActions eAction;
		
			/// <summary>The active drop node</summary>
			public CTmaxMediaTreeNode node;
		
			/// <summary>Flag to indicate if the user is dragging one or more media records</summary>
			public bool bDraggingMedia;
		
			/// <summary>Flag to indicate that all drag records are binder entries</summary>
			public bool bAllBinders;
		
		}// protected struct SDropTarget
		
		#region Enumerations
		
		/// <summary>Context menu command identifiers</summary>
		protected enum TreePaneCommands
		{
			Invalid = 0,
			OpenMenu,
			Viewer,
            Explorer,
			Builder,
			Properties,
			Tuner,
			Presentation,
			Codes,
			Delete,
			MoveUp,
			MoveDown,
			InsertBefore,
			InsertAfter,
			ExportMenu,
			ExportText,
			ExportXmlScript,
			ExportXmlBinder,
			ExportVideo,
			ExportCodes,
			ExportCodesDatabase,
			ExportLoadFile,
			ExportAsciiObjections,
			Preferences,
			RefreshSuperNodes,
			New,
			Copy,
			Paste,
			PasteBefore,
			PasteAfter,
			ExpandAll,
			CollapseAll,
			Synchronize,
			NewMenu,
			NewBinder,
			NewDocument,
			NewRecording,
			NewScript,
			NewBarcodes,
			InsertBeforeMenu,
			InsertBinderBefore,
			InsertDocumentBefore,
			InsertRecordingBefore,
			InsertScriptBefore,
			InsertBarcodesBefore,
			InsertAfterMenu,
			InsertBinderAfter,
			InsertDocumentAfter,
			InsertRecordingAfter,
			InsertScriptAfter,
			InsertBarcodesAfter,
			ScriptNewMenu,
			ScriptNewDesignations,
			ScriptNewClips,
			ScriptNewBarcodes,
			ScriptBeforeMenu,
			ScriptBeforeDesignations,
			ScriptBeforeClips,
			ScriptBeforeBarcodes,
			ScriptAfterMenu,
			ScriptAfterDesignations,
			ScriptAfterClips,
			ScriptAfterBarcodes,
			SortDesignations,
			MergeScripts,
			Duplicate,
			Print,
			Find,
			ReportsMenu,
			ReportScripts,
			ReportExhibits,
			ReportTranscript,
			ReportObjections,
			Clean,
			RotateCW,
			RotateCCW,
			ImportMenu,
			ImportAsciiScripts,
			ImportAsciiBinders,
			ImportXmlScripts,
			ImportXmlBinders,
			SetFilter,
			RefreshFiltered,
			BulkUpdate,
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
			SetTargetBinder,
			AddObjection,
			RepeatObjection,
            PresentationRecording,
            AddAudioWaveform
		}
		
		/// <summary>Actions to be taken when user drops in the tree</summary>
		protected enum TreeDropActions
		{
			None = 0,
			Reorder,
			Add,
			InsertBefore,
			InsertAfter,
			MoveInto,
			MoveBefore,
			MoveAfter,
		}
			
		#endregion Enumerations
		
		#region Error Identifiers
		
		protected const int ERROR_BASE_ADD_SOURCE_FILE_EX			= (ERROR_BASE_PANE_MAX + 1);
		protected const int ERROR_BASE_CREATE_SOURCE_FOLDER_EX		= (ERROR_BASE_PANE_MAX + 2);
		protected const int ERROR_BASE_CREATE_RECORD_EX				= (ERROR_BASE_PANE_MAX + 3);
		protected const int ERROR_PHYSICAL_ADD_SOURCE_FOLDER_EX		= (ERROR_BASE_PANE_MAX + 4);
		protected const int ERROR_PHYSICAL_GET_PRIMARY_ROOT			= (ERROR_BASE_PANE_MAX + 5);
		protected const int ERROR_ADD_RECORD_EX						= (ERROR_BASE_PANE_MAX + 6);
		protected const int ERROR_VIRTUAL_ADD_SOURCE_FOLDER_EX		= (ERROR_BASE_PANE_MAX + 7);
		protected const int ERROR_ON_COMMAND_EX						= (ERROR_BASE_PANE_MAX + 8);
		protected const int ERROR_GET_COMMAND_ITEM_EX				= (ERROR_BASE_PANE_MAX + 9);
		protected const int ERROR_INSERT_NO_INTERFACE				= (ERROR_BASE_PANE_MAX + 10);
		protected const int ERROR_INSERT_NO_MEDIA_TYPE				= (ERROR_BASE_PANE_MAX + 11);
		protected const int ERROR_FIRE_ASYNC_COMMAND_EX				= (ERROR_BASE_PANE_MAX + 12);
		protected const int ERROR_ASYNC_THREAD_PROC_EX				= (ERROR_BASE_PANE_MAX + 13);
		protected const int ERROR_CHANGE_PREFERENCES_EX				= (ERROR_BASE_PANE_MAX + 14);
		protected const int ERROR_PHYSICAL_ADD_PRIMARY_EX			= (ERROR_BASE_PANE_MAX + 15);
		protected const int ERROR_ON_CMD_REPORT_EX					= (ERROR_BASE_PANE_MAX + 16);
		protected const int ERROR_ON_CMD_MERGE_SCRIPTS_EX			= (ERROR_BASE_PANE_MAX + 17);
		protected const int ERROR_ON_CMD_CLEAN_SCANNED_EX			= (ERROR_BASE_PANE_MAX + 18);
		protected const int ERROR_ON_CMD_SCROLL_TEXT_EX				= (ERROR_BASE_PANE_MAX + 19);
		protected const int ERROR_ON_CMD_SET_HIGHLIGHTER_EX			= (ERROR_BASE_PANE_MAX + 20);
			
		protected const string KEY_PRIMARY_TEXT_MODE				= "PrimaryTextMode";
		protected const string KEY_SECONDARY_TEXT_MODE				= "SecondaryTextMode";
		protected const string KEY_TERTIARY_TEXT_MODE				= "TertiaryTextMode";
		protected const string KEY_QUATERNARY_TEXT_MODE				= "QuaternaryTextMode";
		
		#endregion Error Identifiers
		
		#region Protected Members
		
		/// <summary>Control container required by forms designer</summary>
		private System.ComponentModel.IContainer components;

		/// <summary>The drop target descriptor</summary>
		protected SDropTarget m_dropTarget;
		
		/// <summary>Child tree control</summary>
		public FTI.Trialmax.Controls.CTmaxMediaTreeCtrl m_tmaxTreeCtrl = null;
		
		/// <summary>Tree control draw filter</summary>
		protected FTI.Trialmax.Controls.CTmaxBaseTreeFilter m_tmaxTreeFilter = new CTmaxBaseTreeFilter();
		
		/// <summary>Local member bounded to PrimarySorter property</summary>
		/// <remarks>It is up to the derived class to allocate this object if required</remarks>
		protected FTI.Trialmax.Controls.CTmaxBaseTreeSorter m_tmaxPrimarySorter = null;
		
		/// <summary>Collection used to sort nodes based on display order</summary>
		protected FTI.Trialmax.Controls.CTmaxMediaTreeNodes m_tmaxDisplaySorter = new CTmaxMediaTreeNodes();
		
		/// <summary>Local member to keep track of node when user clicks the mouse</summary>
		protected CTmaxMediaTreeNode m_tmaxClickNode = null;
		
		/// <summary>Local member bound to PrimaryTextMode property</summary>
		protected FTI.Shared.Trialmax.TmaxTextModes m_ePrimaryTextMode = TmaxTextModes.MediaId;

		/// <summary>Local member bound to SecondaryTextMode property</summary>
		protected FTI.Shared.Trialmax.TmaxTextModes m_eSecondaryTextMode = TmaxTextModes.Barcode;

		/// <summary>Infragistics toolbar/menu manager</summary>
		protected Infragistics.Win.UltraWinToolbars.UltraToolbarsManager m_ultraToolbarManager;
		
		/// <summary>Infragistics library toolbar/menu manager left-side docking zone</summary>
		protected Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTreePane_Toolbars_Dock_Area_Left;
		
		/// <summary>Infragistics library toolbar/menu manager right-side docking zone</summary>
		protected Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTreePane_Toolbars_Dock_Area_Right;
		
		/// <summary>Infragistics library toolbar/menu manager top docking zone</summary>
		protected Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTreePane_Toolbars_Dock_Area_Top;
		
		/// <summary>Infragistics library toolbar/menu manager bottom docking zone</summary>
		protected Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTreePane_Toolbars_Dock_Area_Bottom;

		/// <summary>Local member bound to TertiaryTextMode property</summary>
		protected FTI.Shared.Trialmax.TmaxTextModes m_eTertiaryTextMode = TmaxTextModes.Barcode;

		/// <summary>Local member bound to QuaternaryTextMode property</summary>
		protected FTI.Shared.Trialmax.TmaxTextModes m_eQuaternaryTextMode = (TmaxTextModes.Name | TmaxTextModes.Barcode);

		/// <summary>Image list used for popup menu</summary>
		protected System.Windows.Forms.ImageList m_ctrlMenuImages;

		/// <summary>Image list used for nodes in the tree</summary>
		protected System.Windows.Forms.ImageList m_ctrlNodeImages;

		/// <summary>Local member bound to SuperNodeSize property</summary>
		protected int m_iSuperNodeSize = 0;

		/// <summary>Local member keep track of the pause threshold (sec) for displaying designations</summary>
		protected double m_dPauseThreshold = 0;

		/// <summary>Local member to inhibit processing of AfterSelect events</summary>
		protected bool m_bIgnoreSelection = false;

		/// <summary>Local member to inhibit processing of AfterChecked events</summary>
		protected bool m_bIgnoreChecked = false;

		/// <summary>Local member bound to CheckBoxes property</summary>
		protected bool m_bCheckBoxes = false;
		
		/// <summary>Local member bound to EnableDragDrop property</summary>
		protected bool m_bEnableDragDrop = true;
		
		/// <summary>Local member bound to EnableContextMenu property</summary>
		protected bool m_bEnableContextMenu = true;

        /// <summary>Local member for opening File dialog for selecting Xmlt file for audio waveform generation</summary>
        private System.Windows.Forms.OpenFileDialog SelectXmltFileForGeneratingAudioWaveform = new OpenFileDialog();
		
		#endregion Protected Members
		
		#region Protected Methods
		
		/// <summary>This function is called when the value of the Database property changes</summary>
		protected override void OnDatabaseChanged()
		{
			//	Do the base class processing first
			base.OnDatabaseChanged();
			
			//	Make sure we're using the latest pause threshold
			if(m_tmaxStationOptions != null)
				m_dPauseThreshold = (double)(m_tmaxStationOptions.PauseThreshold);	
			else
				m_dPauseThreshold = 0; // disable
					
		}// protected override void OnDatabaseChanged()
		
		/// <summary>This method is called when user user left double clicks on a node</summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		protected virtual void OnDoubleClick(CTmaxMediaTreeNode tmaxNode)
		{
//			if(tmaxNode != null)
//			{
//				OnCmdViewer(tmaxNode);
//			}
			
		}// protected virtual void OnDoubleClick(CTmaxMediaTreeNode tmaxNode)
		
		/// <summary> This method is called to give the derived class an opprotunity to modify the drop node before highlighting is performed</summary>
		/// <param name="CursorPosition">Current cursor position in client coordinates</param>
		protected virtual void AdjustDropNode(ref Point CursorPosition)
		{
		}
		
		/// <summary>This method is called to notify the derived class when the value of the PrimaryTextMode property changes</summary>
		protected virtual void OnPrimaryTextModeChanged()
		{
			SetText(TmaxMediaLevels.Primary);
		}
		
		/// <summary>This method is called to notify the derived class when the value of the SecondaryTextMode property changes</summary>
		protected virtual void OnSecondaryTextModeChanged()
		{
			SetText(TmaxMediaLevels.Secondary);
		}
		
		/// <summary>This method is called to notify the derived class when the value of the TertiaryTextMode property changes
		/// 
		protected virtual void OnTertiaryTextModeChanged()
		{
			SetText(TmaxMediaLevels.Tertiary);
		}
		
		/// <summary>This method is called to notify the derived class when the value of the QuaternaryTextMode property changes</summary>
		protected virtual void OnQuaternaryTextModeChanged()
		{
			SetText(TmaxMediaLevels.Quaternary);
		}
		
		/// <summary>This method is called when the PauseThreshold value changes</summary>
		protected virtual void OnPauseThresholdChanged()
		{	
		}
		
		/// <summary>
		/// This method is called to notify the derived class when the value of the SuperNodeSize property changes
		/// </summary>
		protected virtual void OnSuperNodeSizeChanged()
		{
		}
		
		/// <summary>
		/// This method is called to allow the derived class to set the preferences
		/// </summary>
		/// <param name="tmaxPreferences">The new set of preferences</param>
		protected virtual void OnPreferencesChanged(CFTreePreferences tmaxPreferences)
		{
			m_tmaxTreeCtrl.BeginUpdate();
			
			//	Do the default processing
			if(tmaxPreferences.QuaternaryTextMode != QuaternaryTextMode)
				QuaternaryTextMode = tmaxPreferences.QuaternaryTextMode;
			if(tmaxPreferences.TertiaryTextMode != TertiaryTextMode)
				TertiaryTextMode = tmaxPreferences.TertiaryTextMode;
			if(tmaxPreferences.SecondaryTextMode != SecondaryTextMode)
				SecondaryTextMode = tmaxPreferences.SecondaryTextMode;
			if(tmaxPreferences.PrimaryTextMode != PrimaryTextMode)
				PrimaryTextMode = tmaxPreferences.PrimaryTextMode;
				
			m_tmaxTreeCtrl.EndUpdate();
		}
		
		/// <summary>This method is called reset the text for all nodes that occur as the specified media level</summary>
		/// <param name="eLevel">The media level at which the node must reside</param>
		protected virtual void SetText(TmaxMediaLevels eLevel)
		{
			//	Check each node in the tree
			if((m_tmaxTreeCtrl != null) && (m_tmaxTreeCtrl.Nodes != null))
			{
				foreach(CTmaxMediaTreeNode tmaxNode in m_tmaxTreeCtrl.Nodes)
				{
					SetText(tmaxNode, eLevel, true);
				}
			}
		}
		
		/// <summary>This method is called reset the text for all nodes in the specified branch that occur at the specified media level</summary>
		/// <param name="tmaxTop">Top node in the branch</param>
		/// <param name="eLevel">Media level at which to set the node's text</param>
		///	<param name="bChildren">True to drill down into child nodes</param>
		protected virtual void SetText(CTmaxMediaTreeNode tmaxTop, TmaxMediaLevels eLevel, bool bChildren)
		{
			//	Is this node at the desired level?
			if(tmaxTop.MediaLevel == eLevel)
			{
				switch(eLevel)
				{
					case TmaxMediaLevels.Primary:
										
						if(tmaxTop.IPrimary != null)
							tmaxTop.Text = tmaxTop.IPrimary.GetText(m_ePrimaryTextMode);
						break;
						
					case TmaxMediaLevels.Secondary:
					
						if(tmaxTop.ISecondary != null)
							tmaxTop.Text = tmaxTop.ISecondary.GetText(m_eSecondaryTextMode);
						break;
						
					case TmaxMediaLevels.Tertiary:
					
						if(tmaxTop.ITertiary != null)
							tmaxTop.Text = tmaxTop.ITertiary.GetText(m_eTertiaryTextMode);
						break;
						
					case TmaxMediaLevels.Quaternary:
					
						if(tmaxTop.IQuaternary != null)
							tmaxTop.Text = tmaxTop.IQuaternary.GetText(m_eQuaternaryTextMode);
						break;
						
					default:
					
						break;
				
				}// switch(eLevel)
			
			}// if(tmaxNode.MediaLevel == eLevel)
			
			//	Drill down into the subnodes?
			if(bChildren == true)
			{
				if((tmaxTop.Nodes != null) && (tmaxTop.Nodes.Count > 0))
				{
					foreach(CTmaxMediaTreeNode tmaxChild in tmaxTop.Nodes)
					{
						SetText(tmaxChild, eLevel, bChildren);
					}
				}
			}
			
		}//	SetText(CTmaxMediaTreeNode tmaxTop, TmaxMediaLevels eLevel, bool bChildren)
		
		///	<summary>This method will set the text for the specified node</summary>
		/// <param name="tmaxNode">The node who's text is to be set</param>
		/// <param name="bChildren">true to set the text for all child nodes</param>
		protected virtual void SetText(CTmaxMediaTreeNode tmaxNode, bool bChildren)
		{
			//	At what level does this node reside?
			switch(tmaxNode.MediaLevel)
			{
				case TmaxMediaLevels.Primary:
									
					if(tmaxNode.IPrimary != null)
						tmaxNode.Text = tmaxNode.IPrimary.GetText(m_ePrimaryTextMode);
					break;
					
				case TmaxMediaLevels.Secondary:
				
					if(tmaxNode.ISecondary != null)
						tmaxNode.Text = tmaxNode.ISecondary.GetText(m_eSecondaryTextMode);
					break;
					
				case TmaxMediaLevels.Tertiary:
				
					if(tmaxNode.ITertiary != null)
						tmaxNode.Text = tmaxNode.ITertiary.GetText(m_eTertiaryTextMode);
					break;
					
				case TmaxMediaLevels.Quaternary:
				
					if(tmaxNode.IQuaternary != null)
						tmaxNode.Text = tmaxNode.IQuaternary.GetText(m_eQuaternaryTextMode);
					break;
					
				default:
				
					break;
			
			}// switch(eLevel)
			
			//	Should we set the text for child nodes?
			if((bChildren == true) && (tmaxNode.Nodes != null))
			{
				foreach(CTmaxMediaTreeNode O in tmaxNode.Nodes)
					SetText(O, true);
			}
			
		}//	protected virtual void SetText(CTmaxMediaTreeNode tmaxNode, bool bChildren)
		
		/// <summary>This method is called to refresh the sort order and display text for the specified node and it's children</summary>
		/// <param name="tmaxNode">The node to be refreshed</param>
		protected virtual void Refresh(CTmaxMediaTreeNode tmaxNode)
		{
			int iImage = 0;
			
			Debug.Assert(tmaxNode != null);
			
			//	Update the text for this node
			SetText(tmaxNode, false);
			
			//	Has the image for this node changed?
			switch(tmaxNode.MediaType)
			{
				case TmaxMediaTypes.Designation:
				case TmaxMediaTypes.Clip:
				case TmaxMediaTypes.Scene:
				case TmaxMediaTypes.Link:
				
					iImage = GetImageIndex(tmaxNode);
					if(iImage != (int)tmaxNode.Override.NodeAppearance.Image)
						tmaxNode.Override.NodeAppearance.Image = iImage;
						
					break;
			}
			
			//	Make sure the parent text is correct
			if(tmaxNode.Parent != null)
				SetText((CTmaxMediaTreeNode)tmaxNode.Parent, false);
				
			//	Does this node have any children?
			if((tmaxNode.Nodes != null) && (tmaxNode.Nodes.Count > 0))
			{
				//	Should we sort the children?
                // Do not sort, if its binder tree.
				if((tmaxNode.GetMediaRecord() != null) || (tmaxNode.IBinder != null))
					Sort(tmaxNode,true);
					
				//	Update the text for each child
				foreach(CTmaxMediaTreeNode tmaxChild in tmaxNode.Nodes)
				{
					SetText(tmaxChild, true);
					
					//	Make sure we have the correct image if this is a link
					if(tmaxChild.MediaType == TmaxMediaTypes.Link)
					{
						iImage = GetImageIndex(tmaxChild);
						if(iImage != (int)tmaxChild.Override.NodeAppearance.Image)
							tmaxChild.Override.NodeAppearance.Image = iImage;
					}
					
				}// foreach(CTmaxMediaTreeNode tmaxChild in tmaxNode.Nodes)
			
			}
			
		}// protected virtual void Refresh(CTmaxMediaTreeNode tmaxNode)
		
		/// <summary>This method is called to sort the children of the specified node based on display order</summary>
		/// <param name="tmaxParent">The parent that owns the collection to be sorted</param>
        protected virtual void Sort(CTmaxMediaTreeNode tmaxParent, bool isDefualtSorter)
		{
			Debug.Assert(tmaxParent != null);
			if(tmaxParent == null) return;

			if((tmaxParent.Nodes != null) && (tmaxParent.Nodes.Count > 1))
                Sort(tmaxParent.Nodes, isDefualtSorter);
			
		}// protected virtual void Sort(CTmaxMediaTreeNode tmaxParent)
		
		/// <summary>This method is called to sort the node collection based on display order</summary>
		/// <param name="aNodes">The  collection to be sorted</param>
		protected virtual void Sort(TreeNodesCollection aNodes,bool isDefualtSorter)
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
            foreach (CTmaxMediaTreeNode tmaxChild in aNodes)
            {
                //if(tmaxChild is
                m_tmaxDisplaySorter.Add(tmaxChild);
            }

           

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
			
		}// protected virtual void Sort(TreeNodesCollection aNodes)
		
		/// <summary>This method is called to sort the designations contained in the specified script</summary>
		/// <param name="tmaxScript">The node associated with the script to be sorted</param>
		///	<returns>true if successful</returns>
		protected virtual bool SortDesignations(CTmaxMediaTreeNode tmaxScript)
		{
			ArrayList		aDepositions = null;
			CDxPrimary		dxScript = null;
			CDxMediaRecords		dxReordered = null;
			CDxMediaRecords		dxDesignations = null;
			CDxMediaRecords		dxRemaining = null;
			CTmaxItem		tmaxParent = null;
			bool			bReordered = false;
			
			//	Must be a valid script
			if(tmaxScript == null) return false;
			if(tmaxScript.GetMediaRecord() == null) return false;
			if(tmaxScript.MediaType != TmaxMediaTypes.Script) return false;
			
			//	Make sure the child collection is populated
			dxScript = (CDxPrimary)tmaxScript.GetMediaRecord();
			if((dxScript.Secondaries == null) || dxScript.Secondaries.Count == 0)
				dxScript.Fill();
				
			//	No point sorting if not at least two designations
			if(dxScript.Secondaries.Count <= 1) return false;
			
			//	First we need to group all designations by their owner deposition
			aDepositions = GroupByDeposition(dxScript);
			if((aDepositions == null) || (aDepositions.Count == 0)) return false;
			
			//	Do we need to prompt the user to order the depositions
			if(aDepositions.Count > 1)
			{
				CFReorder cfDepositions = new CFReorder();
				
				cfDepositions.Collection = aDepositions;
				cfDepositions.Prompt = "The designations were created from the depositions shown in this list. Please put them in the desired sort order.";
				cfDepositions.Title = " Set Deposition Order";
				cfDepositions.ReorderCollection = true;
				
				if(cfDepositions.ShowDialog() == DialogResult.Cancel)
				{
					aDepositions.Clear();
					return false;
				}
			
			}
			
			//	Create the temporary collections needed to reorder the scenes
			dxReordered    = new CDxMediaRecords();
			dxDesignations = new CDxMediaRecords();
			
			//	Transfer all scenes to our reordering collection
			foreach(CDxSecondary dxScene in dxScript.Secondaries)
				dxReordered.AddList(dxScene);
				
			//	Iterate the groups and build the composite designation collection
			foreach(CDxMediaRecords dxGroup in aDepositions)
			{
				foreach(CDxSecondary O in dxGroup)
				{
					//	Verify this is in the script collection
					if(dxReordered.Contains(O) == true)
					{
						//	Add to the designations collection
						dxDesignations.AddList(O);
						
						//	Remove from the reordering collection
						dxReordered.Remove(O);
					}
					else
					{
						Debug.Assert(false);
					}
					
				}// foreach(CDxSecondary O in dxGroup)
			
			}// foreach(CDxMediaRecords dxGroup in aDepositions)
			
			//	Do we have anything remaining in the reordered list?
			if(dxReordered.Count > 0)
			{
				dxRemaining = new CDxMediaRecords();
				
				//	Transfer to the temporary collection
				foreach(CDxSecondary O in dxReordered)
					dxRemaining.AddList(O);
					
				dxReordered.Clear();
			}
			
			//	Now add the sorted designations to the start of the script
			foreach(CDxSecondary O in dxDesignations)
				dxReordered.AddList(O);
			dxDesignations.Clear();
			
			//	Put anything that was left over at the end of the script
			if(dxRemaining != null)
			{
				foreach(CDxSecondary O in dxRemaining)
					dxReordered.AddList(O);
				
				dxRemaining.Clear();
			}
			
			//	Have we really changed anything 
			for(int i = 0; i < dxReordered.Count; i++)
			{
				if(dxReordered[i].DisplayOrder != (i + 1))
				{
					bReordered = true;
					break;
				}
				
			}
			
			//	Do we need to reorder the script?
			if(bReordered == true)
			{			
				//	Construct the event item we need to request reordering of the script
				tmaxParent = new CTmaxItem(dxScript);
				
				//	Now add items to indicate the new order
				foreach(CDxSecondary O in dxReordered)
					tmaxParent.SubItems.Add(new CTmaxItem(O));
			
				//	Fire the event
				FireCommand(TmaxCommands.Reorder, tmaxParent);
			}
			
			return bReordered;
			
		}// protected virtual void SortDesignations(CTmaxMediaTreeNode tmaxScript)
		
		/// <summary>This method is called to move the specified children to the new location</summary>
		/// <param name="tmaxChildren">Children to be moved</param>
		/// <param name="tmaxLocation">New location at which to place the children</param>
		/// <param name="bBefore">true if new position is before the specified location</param>
		protected virtual void Reorder(CTmaxItems tmaxChildren, CTmaxMediaTreeNode tmaxLocation, bool bBefore)
		{
			CTmaxItem	tmaxParent  = null;
			CTmaxItem	tmaxInsert  = null;
			CTmaxItem	tmaxRemove  = null;
			CTmaxItems	tmaxMoving  = new CTmaxItems();
			CTmaxItems	tmaxHolding = new CTmaxItems();
			int			iInsert = -1;
			int			i;
			
			//	Check these various conditions before going on
			if(tmaxLocation == null) return;
			if(tmaxLocation.IPrimary == null) return;
			if(tmaxChildren == null) return;
			if(tmaxChildren.Count == 0) return;
			if(tmaxChildren[0].MediaLevel == TmaxMediaLevels.None) return;
			if(tmaxChildren[0].IPrimary == null) return;
			
			//	The location must be secondary or lower
			if(tmaxLocation.ISecondary == null) return;
			
			//	Transfer the children to a local collection we can manipulate
			for(i = 0; i < tmaxChildren.Count; i++)
			{
				//	If the first node being moved is also the drop node
				//	do not copy it to the collection of nodes being moved
				if((i == 0) && (ReferenceEquals(tmaxChildren[0].GetMediaRecord(), tmaxLocation.GetTmaxRecord(true)) == true))
				{
					bBefore = false; //	Always after in this case
				}
				else
				{
					//	Add to the collection of those being moved
					tmaxMoving.Add(tmaxChildren[i]);
				}
				
			}
			
			//	Do we have anything to move?
			if(tmaxMoving.Count == 0) return;
			
			//	Create a command item for the parent node and request
			//	that items be created for each child as they appear in the
			//	tree right now
			tmaxParent = GetCommandItem((CTmaxMediaTreeNode)tmaxLocation.Parent, true);
			if(tmaxParent == null) return;
			
			//	Where is the target location in the tree?
			iInsert = tmaxParent.SubItems.IndexOf(tmaxLocation.GetTmaxRecord(true));
			Debug.Assert(iInsert >= 0);
			if(iInsert < 0) return;
			
			//	Adjust the target location if the specified location is among
			//	those nodes being moved
			if(tmaxMoving.IndexOf(tmaxLocation.GetTmaxRecord(true)) >= 0)
			{
				//	Try moving down the collection in search of the first node not among
				//	those being moved
				for(i = iInsert + 1; i < tmaxParent.SubItems.Count; i++)
				{
					if(tmaxMoving.IndexOf(tmaxParent.SubItems[i].GetMediaRecord()) < 0)
					{
						iInsert = i;
						bBefore = true;
						break;
					}
				
				}
			
				//	Were we unable to find a node?
				if(i >= tmaxParent.SubItems.Count)
				{
					//	Try going backwards up the tree
					for(i = iInsert - 1; i >= 0; i--)
					{
						if(tmaxMoving.IndexOf(tmaxParent.SubItems[i].GetMediaRecord()) < 0)
						{
							iInsert = i;
							bBefore = false;
							break;
						}
					}
					
				}
				
				//	Are all nodes being moved?
				if(i < 0)
					iInsert = -1;			
			}
			
			//	Get the item that represents the actual insertion point
			//
			//	NOTE:	The index will go bad as soon as we start reordering
			if((iInsert >= 0) && (iInsert < tmaxParent.SubItems.Count))
				tmaxInsert = tmaxParent.SubItems[iInsert];
				
			//	Remove each of the children being moved from the parent collection
			foreach(CTmaxItem tmaxMove in tmaxMoving)
			{
				if((tmaxRemove = tmaxParent.SubItems.Find(tmaxMove.GetMediaRecord())) != null)
				{
					//	Put in holding collection and remove from parent
					tmaxHolding.Add(tmaxRemove);
					tmaxParent.SubItems.Remove(tmaxRemove);
				}
				else
				{
					//	Should be in there
					Debug.Assert(tmaxRemove != null);
				}	
			
			}// foreach(CTmaxItem tmaxMove in tmaxMoving)
			
			//	If no insertion point is defined then just transfer from the holding
			//	collection to the parent's collection
			if(tmaxInsert == null)
			{
				foreach(CTmaxItem O in tmaxHolding)
					tmaxParent.SubItems.Add(O);
			}
			else
			{
				//	Get the location of the insertion point
				iInsert = tmaxParent.SubItems.IndexOf(tmaxInsert);
				Debug.Assert(iInsert >= 0);
				
				//	Are we actually adding to the end?
				if((bBefore == false) && (iInsert == (tmaxParent.SubItems.Count - 1)))
				{
					foreach(CTmaxItem O in tmaxHolding)
						tmaxParent.SubItems.Add(O);
				}
				else
				{
					//	Adjust the insertion point so that we are always
					//	inserting before the point
					if(bBefore == false)
						iInsert++;
						
					//	Now put everything back in
					foreach(CTmaxItem O in tmaxHolding)
					{
						tmaxParent.SubItems.Insert(O, iInsert);
						iInsert++;
					}
						
				}
				
			}
			tmaxHolding.Clear();
			
			//	Ask the system to reorder based on the subitem collection's new order
			FireCommand(TmaxCommands.Reorder, tmaxParent);
			
		}// protected virtual void Reorder(CTmaxItems tmaxChildren, CTmaxMediaTreeNode tmaxLocation, bool bBefore)
		
		/// <summary>This method is called to group all designations in the specified script by deposition</summary>
		/// <param name="dxScript">The script that owns the designations</param>
		///	<returns>true if successful</returns>
		protected virtual ArrayList GroupByDeposition(CDxPrimary dxScript)
		{
			CDxPrimary			dxDeposition = null;
			CDxMediaRecords			dxGroup = null;
			CDxTertiary			dxDesignation = null;
			ArrayList			aDepositions = new ArrayList();
			CTmaxRecordSorter	tmaxSorter  = null;
			
			Debug.Assert(dxScript != null);
			
			//	Make sure the child collection is populated
			if((dxScript.Secondaries == null) || dxScript.Secondaries.Count == 0)
				dxScript.Fill();
				
			//	Group all designation scenes by their owner deposition
			foreach(CDxSecondary dxScene in dxScript.Secondaries)
			{
				//	Is this scene a designation?
				if((dxScene.GetSource() != null) &&
					(dxScene.GetSource().MediaType == TmaxMediaTypes.Designation))
				{
					dxDeposition = null;
					
					//	Get the exchange interface for the deposition that owns this designation
					dxDesignation = (CDxTertiary)dxScene.GetSource();
					Debug.Assert(dxDesignation.Secondary != null);
					if(dxDesignation.Secondary != null)
					{
						Debug.Assert(dxDesignation.Secondary.Primary != null);
						if(dxDesignation.Secondary.Primary != null)
							dxDeposition = dxDesignation.Secondary.Primary;
					}
					
					//	Were we able to get the source deposition?
					if(dxDeposition != null)
					{
						//	Do we need to search for the deposition group?
						if((dxGroup == null) || 
						   (ReferenceEquals(dxGroup.OwnerAssigned, dxDeposition) == false))
						{
							//	We need to search for the group
							dxGroup = null;
							
							//	Is this group in our list already?
							foreach(CDxMediaRecords dxr in aDepositions)
							{
								if(ReferenceEquals(dxr.OwnerAssigned, dxDeposition) == true)
								{
									dxGroup = dxr;
									break;
								}
								
							}// foreach(CDxMediaRecords dxr in aDepositions)
							
							//	Do we need to create a new group?
							if(dxGroup == null)
							{
								//	Create a new group
								tmaxSorter = new CTmaxRecordSorter();
								tmaxSorter.UseSource = true;
								
								dxGroup = new CDxMediaRecords();
								dxGroup.OwnerAssigned = dxDeposition;
								dxGroup.DisplayText = dxDeposition.GetText();
								dxGroup.Comparer = tmaxSorter;
								dxGroup.KeepSorted = false;
								
								//	Add to the list
								aDepositions.Add(dxGroup);
							}
							
						}// if((dxGroup == null) || (ReferenceEquals(dxGroup.OwnerAssigned, dxDeposition) == false))
							
						//	Add the host SCENE (not designation) to the list
						Debug.Assert(dxGroup != null);
						if(dxGroup != null)
							dxGroup.AddList(dxScene);
							
					}// if(dxDeposition != null)
				
				}// if((dxScene.GetSource() != null) && (dxScene.GetSource().MediaType == TmaxMediaTypes.Designation)
				
			}// foreach(CDxSecondary dxScene in dxScript.Secondaries)
			
			//	Make sure all the designations are sorted
			foreach(CDxMediaRecords O in aDepositions)
				O.Sort();
				
			return aDepositions;
			
		}// GroupByDeposition(CDxPrimary dxScript)
		
		/// <summary>This method is called to let the derived class determine if the specified nodes can be dragged</summary>
		/// <param name="tmaxNodes">The nodes to be dragged</param>
		/// <returns>true if the nodes can be dragged</returns>
		protected virtual bool CanDrag(CTmaxMediaTreeNodes tmaxNodes)
		{
			if(tmaxNodes == null) return false;
			if(tmaxNodes.Count == 0) return false;
			
			if(m_bEnableDragDrop == false)
				return false;
				
			//	Check each node
			foreach(CTmaxMediaTreeNode O in tmaxNodes)
			{
				if(CanDrag(O) == false)
					return false;
			}
			
			return true;
		}
		
		/// <summary>This method is called to let the derived class determine if the specified node can be dragged</summary>
		/// <param name="tmaxNode">The node to be dragged</param>
		/// <returns>true if the node can be dragged</returns>
		protected virtual bool CanDrag(CTmaxMediaTreeNode tmaxNode)
		{
			if(m_bEnableDragDrop == false) return false;
			if(tmaxNode == null) return false;
			if(tmaxNode.IPrimary == null) return false;
			
			//	Can't be related to a deposition's child
			if((tmaxNode.IPrimary.GetMediaType() == TmaxMediaTypes.Deposition) &&
			   (tmaxNode.ISecondary != null)) 
				return false;
				
			//	Can't drag links
			if(tmaxNode.MediaType == TmaxMediaTypes.Link) return false;
			
			return true;
		}
		
		/// <summary>Called to get the action to be performed if the user drops source files at the current location</summary>
		/// <returns>The action to be carried out when the user drops</returns>
		protected virtual TreeDropActions GetDropSourceAction()
		{
			//	Update the current line position
			m_dropTarget.ePosition = m_tmaxTreeFilter.DropLinePosition;
			
			return TreeDropActions.None;
		}
		
		/// <summary>Called to get the action to be performed if the user drops data records at the current location</summary>
		/// <returns>The action to be carried out when the user drops</returns>
		protected virtual TreeDropActions GetDropRecordsAction()
		{
			TreeDropActions	eAction = TreeDropActions.None;
			CTmaxMediaTreeNode	tmaxParent = null;
			bool			bDraggingBinder = false;
			bool			bDraggingPrimary = false;
			
			//	Update the current line position
			m_dropTarget.ePosition = m_tmaxTreeFilter.DropLinePosition;
			
			//	Check these various conditions before going on
			if(m_dropTarget.node == null) return TreeDropActions.None;
			if(m_dropTarget.node.IPrimary == null) return TreeDropActions.None;
			if(m_tmaxDragData == null) return TreeDropActions.None;
			if(m_tmaxDragData.SourceItems == null) return TreeDropActions.None;
			if(m_tmaxDragData.SourceItems.ContainsMedia(true) == false) return TreeDropActions.None;
		
			//	Is the user dragging a deposition or one of it's descendants?
			//
			//	NOTE:	We check the entire collection because the user may be dragging
			//			a group selected in the binder tree
			foreach(CTmaxItem O in m_tmaxDragData.SourceItems)
			{
				if((O.IPrimary != null) && (O.IPrimary.GetMediaType() == TmaxMediaTypes.Deposition))
				{
					//	Can't drop depositions or segments in base trees
					switch(O.GetMediaRecord().GetMediaType())
					{
						case TmaxMediaTypes.Deposition:
						case TmaxMediaTypes.Segment:
						
							return TreeDropActions.None;
					}
					
				}// if((O.IPrimary != null) && (O.IPrimary.GetMediaType() == TmaxMediaTypes.Deposition))
			
			}// foreach(CTmaxItem O in m_tmaxDragData.SourceItems)

			//	Is the user dragging a binder?
			if((m_tmaxDragData.DataType == TmaxDataTypes.Binder) ||
			   ((m_tmaxDragData.IBinderEntry != null) && (m_tmaxDragData.GetMediaRecord() == null)))
			{
				bDraggingBinder = true;
			}
			//	Is the user dragging primary media
			else if(m_tmaxDragData.SourceItems[0].MediaLevel == TmaxMediaLevels.Primary)
			{
				bDraggingPrimary = true;
			}
			
			if((bDraggingBinder == true) || (bDraggingPrimary == true))
			{
				//	The drop node must be a script or scene
				if(m_dropTarget.node.MediaType == TmaxMediaTypes.Script)
				{
					if(m_dropTarget.ePosition == TmaxTreePositions.OnNode)
						eAction = TreeDropActions.Add; 
				}
				else if(m_dropTarget.node.MediaType == TmaxMediaTypes.Scene)
				{
					if(m_dropTarget.ePosition == TmaxTreePositions.AboveNode)
						eAction = TreeDropActions.InsertBefore;
					else if(m_dropTarget.ePosition == TmaxTreePositions.BelowNode)
						eAction = TreeDropActions.InsertAfter;
				}

			}
			else
			{
				//	Is the user dropping on a primary media node?
				if(m_dropTarget.node.MediaLevel == TmaxMediaLevels.Primary)
				{
					//	The drop node must be a script
					if(m_dropTarget.node.MediaType == TmaxMediaTypes.Script)
					{
						if(m_dropTarget.ePosition == TmaxTreePositions.OnNode)
							eAction = TreeDropActions.Add;
					}
				}
				else
				{
					//	Get the drop target's parent
					Debug.Assert(m_dropTarget.node.Parent != null);
					tmaxParent = (CTmaxMediaTreeNode)m_dropTarget.node.Parent;
					
					//	Are we reordering children within the same parent?
					if(ReferenceEquals(tmaxParent.GetMediaRecord(), m_tmaxDragData.GetMediaRecord()) == true)
					{
						//	Can't reorder composite media
						if((CTmaxMediaTypes.IsCompositeMedia(tmaxParent.MediaType) == false) &&
							(tmaxParent.MediaType != TmaxMediaTypes.Scene))
						{
							eAction = TreeDropActions.Reorder;
						}
						
					}
					else
					{
						//	The drop node must be a script or scene
						if(m_dropTarget.node.MediaType == TmaxMediaTypes.Script)
						{
							if(m_dropTarget.ePosition == TmaxTreePositions.OnNode)
								eAction = TreeDropActions.Add; 
						}
						else if(m_dropTarget.node.MediaType == TmaxMediaTypes.Scene)
						{
							if(m_dropTarget.ePosition == TmaxTreePositions.AboveNode)
								eAction = TreeDropActions.InsertBefore;
							else if(m_dropTarget.ePosition == TmaxTreePositions.BelowNode)
								eAction = TreeDropActions.InsertAfter;
						}

					}// if(ReferenceEquals(tmaxParent.GetTmaxRecord(), m_tmaxDragData.GetMediaRecord()) == true)
					
				}// if(m_dropTarget.node.MediaLevel == TmaxMediaLevels.Primary)
				
			}// if(m_tmaxDragData.SourceItems[0].MediaLevel == TmaxMediaLevels.Primary)
			
			return eAction;
		
		}// protected virtual TreeDropActions GetDropRecordsAction()
		
		/// <summary>This method will retrieve the appropriate drag/drop effects when dragging source files</summary>
		/// <returns>The appropriate drag drop effects</returns>
		protected virtual DragDropEffects GetDropSourceEffects()
		{
			if(m_dropTarget.eAction != TreeDropActions.None)
				return GetFileTransferEffects();
			else
				return DragDropEffects.None;

		}// protected virtual DragDropEffects GetDropSourceEffects()
		
		/// <summary>This method will retrieve the appropriate drag/drop effects when dragging database records</summary>
		/// <returns>The appropriate drag drop effects</returns>
		protected virtual DragDropEffects GetDropRecordsEffects()
		{
			switch(m_dropTarget.eAction)
			{
				case TreeDropActions.Add:
				case TreeDropActions.InsertAfter:
				case TreeDropActions.InsertBefore:
				
					return DragDropEffects.Link;
					
				case TreeDropActions.Reorder:
				case TreeDropActions.MoveInto:
				case TreeDropActions.MoveBefore:
				case TreeDropActions.MoveAfter:
				
					return DragDropEffects.Move;
					
				case TreeDropActions.None:
				default:
				
					return DragDropEffects.None;
			}
			
		}// protected virtual DragDropEffects GetDropRecordsEffects()
		
		/// <summary>This method is called to determine which drop line positions are allowed if the user is dropping source files</summary>
		/// <returns>The allowed drop positions</returns>
		protected virtual TmaxTreePositions GetDropSourcePositionsAllowed()
		{	
			switch(m_dropTarget.eAction)
			{
				case TreeDropActions.Add:
				
					return TmaxTreePositions.OnNode;
					
				case TreeDropActions.InsertAfter:
				
					return (TmaxTreePositions.BelowNode | TmaxTreePositions.OnNode);
					
				case TreeDropActions.InsertBefore:
				
					return (TmaxTreePositions.AboveNode | TmaxTreePositions.OnNode);
					
				case TreeDropActions.Reorder:
				
					return (TmaxTreePositions.AboveNode | TmaxTreePositions.BelowNode);

				case TreeDropActions.MoveInto:	//	Not valid for dropping source
				case TreeDropActions.MoveBefore:
				case TreeDropActions.MoveAfter:
				case TreeDropActions.None:
				default:
				
					return TmaxTreePositions.None;
			}
		
		}// protected virtual TmaxTreePositions GetDropSourcePositionsAllowed()
		
		/// <summary>This method is called to determine which drop line positions are allowed if the user is dropping data</summary>
		/// <returns>The allowed drop positions</returns>
		protected virtual TmaxTreePositions GetDropRecordsPositionsAllowed()
		{	
			switch(m_dropTarget.eAction)
			{
				case TreeDropActions.Add:
				case TreeDropActions.MoveInto:
				
					return TmaxTreePositions.OnNode;
					
				case TreeDropActions.InsertAfter:
				case TreeDropActions.MoveAfter:
				
					return (TmaxTreePositions.BelowNode | TmaxTreePositions.OnNode);
					
				case TreeDropActions.InsertBefore:
				case TreeDropActions.MoveBefore:
				
					return (TmaxTreePositions.AboveNode | TmaxTreePositions.OnNode);
					
				case TreeDropActions.Reorder:
				
					return (TmaxTreePositions.AboveNode | TmaxTreePositions.BelowNode);

				case TreeDropActions.None:
				default:
				
					return TmaxTreePositions.None;
			}
		
		}// protected virtual TmaxTreePositions GetDropRecordsPositionsAllowed()
		
		/// <summary>This method is called to determine if the specified command should be enabled</summary>
		/// <param name="eCommand">The tree pane command enumeration</param>
		/// <param name="tmaxNode">The current node selections</param>
		/// <returns>true if command should be enabled</returns>
		protected virtual bool GetCommandEnabled(TreePaneCommands eCommand, CTmaxMediaTreeNodes tmaxNodes)
        {
			CTmaxItems tmaxItems = null;
			
			//	Some commands are always enabled
			if(eCommand == TreePaneCommands.Preferences) return true;
			
			//	Do we have a valid tree
			if(m_tmaxTreeCtrl == null) return false;
			if(m_tmaxTreeCtrl.Nodes == null) return false;
			if(m_tmaxTreeCtrl.Nodes.Count == 0) return false;
			
			//	All we need is a valid tree for this command
			if(eCommand == TreePaneCommands.CollapseAll) return true;
			
			//	Do we have an active database?
			if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null) && (m_tmaxDatabase.Primaries.Count > 0))
			{
				//	An active database is all we need for these commands
				if(eCommand == TreePaneCommands.Print) return true;
				if(eCommand == TreePaneCommands.Find) return true;
				if(eCommand == TreePaneCommands.OpenMenu) return true;
				if(eCommand == TreePaneCommands.ReportsMenu) return true;
			}
			else
			{
				return false;
			}
				
			//	No selections?
			if((tmaxNodes == null) || (tmaxNodes.Count == 0))
			{
				//	All other commands require at least one selection
				return false;
			}
			
			//	What is the command?
			switch(eCommand)
			{
				//	These commands require valid media objects
				case TreePaneCommands.Viewer:
                case TreePaneCommands.Explorer:
                    //	These commands require single media selections
                    if (tmaxNodes.Count != 1) return false;
                    if (tmaxNodes[0].IPrimary == null) return false;

                    //	Top level must be a script
                    return (tmaxNodes[0].MediaType != TmaxMediaTypes.Script);
				case TreePaneCommands.Presentation:
                //case TreePaneCommands.PresentationRecording:
				case TreePaneCommands.Properties:
				case TreePaneCommands.Codes:
	
					//	These commands require single media selections
					if(tmaxNodes.Count != 1) return false;
					if(tmaxNodes[0].IPrimary == null) return false;
					
					if(eCommand == TreePaneCommands.Codes)
					{
						if(GetCodesEnabled() == false) return false;
					}

					//=====================================================
					//	PLAY DEPOSITION : REMOVE THIS BLOCK
					//=====================================================
					//	Is this a deposition or one of it's descendants?
					if(tmaxNodes[0].IPrimary.GetMediaType() == TmaxMediaTypes.Deposition)
					{
						//	Presentation can't view depositions directly
						return (eCommand != TreePaneCommands.Presentation);
					}
					//=====================================================

					//	No metadata for quaternary records
					if(tmaxNodes[0].IQuaternary != null)
					{
						return (eCommand != TreePaneCommands.Codes);
					}
					else
					{
						return true;
					}

                case TreePaneCommands.AddAudioWaveform:
                    if (FTI.Shared.Trialmax.Config.Configuration.ShowAudioWaveform == false) return false;
                 // if(tmaxNodes.Count != 1) return false;
                 // if(tmaxNodes[0].IPrimary == null) return false;
                 // if (tmaxNodes[0].IPrimary.GetMediaType() == TmaxMediaTypes.Deposition) return true;
                 // return false;
                    if (tmaxNodes.Count != 1 )
                    {
                        foreach (CTmaxMediaTreeNode tmaxNode in tmaxNodes)
                        {
                            if (tmaxNode.GetMediaRecord() is CDxSecondary || tmaxNode.GetMediaRecord() is CDxTertiary)
                                return false;
                        }
                    }
                    foreach (CTmaxMediaTreeNode tmaxNode in tmaxNodes)
                    {
                        if (tmaxNode.IPrimary == null) return false;
                        if (tmaxNode.GetMediaRecord() is CDxSecondary || tmaxNode.GetMediaRecord() is CDxTertiary) return false;
                        if (tmaxNode.IPrimary.GetMediaType() != TmaxMediaTypes.Deposition) return false;
                       
                    }
                    return true;

                case TreePaneCommands.PresentationRecording:
                //	These commands require single media selections
                    if (tmaxNodes.Count != 1) return false;
                    if (tmaxNodes[0].IPrimary == null) return false;

                    if (eCommand == TreePaneCommands.Codes)
                    {
                        if (GetCodesEnabled() == false) return false;
                    }

                    //	Is this a document,PPT or one of it's descendants?
                    if (tmaxNodes[0].IPrimary.GetMediaType() == TmaxMediaTypes.Document)
                    {
                        //	Presentation can't view Documents directly
                        return (eCommand != TreePaneCommands.PresentationRecording);
                    }

                    if (tmaxNodes[0].IPrimary.GetMediaType() == TmaxMediaTypes.Powerpoint)
                    {
                        //	Presentation can't view Powerpoints directly
                        return (eCommand != TreePaneCommands.PresentationRecording);
                    }
                    
                    if (tmaxNodes[0].IPrimary.GetMediaType() == TmaxMediaTypes.Recording)
                    {
                        //	Presentation can't view Recordings directly
                        return (eCommand != TreePaneCommands.PresentationRecording);
                    }
                    //	No metadata for quaternary records
                    if (tmaxNodes[0].IQuaternary != null)
                    {
                        return (eCommand != TreePaneCommands.Codes);
                    }
                    else
                    {
                        return true;
                    }

				case TreePaneCommands.Builder:
				case TreePaneCommands.Tuner:
	
					//	These commands require single media selections
					if(tmaxNodes.Count != 1) return false;
					if(tmaxNodes[0].IPrimary == null) return false;
					
					//	Top level must be a script
					return (tmaxNodes[0].IPrimary.GetMediaType() == TmaxMediaTypes.Script);
					
				case TreePaneCommands.New:
	
					//	Must be only one node selected
					if(tmaxNodes.Count != 1) return false;
				
					//	Add new primary media?
					if(tmaxNodes[0].MediaLevel == TmaxMediaLevels.None)
					{
						//	OK to add documents, recordings and scripts
						if(tmaxNodes[0].MediaType == TmaxMediaTypes.Document) return true;
						if(tmaxNodes[0].MediaType == TmaxMediaTypes.Recording) return true;
						if(tmaxNodes[0].MediaType == TmaxMediaTypes.Script) return true;
					}
					//	Add secondary media?
					else if (tmaxNodes[0].MediaLevel == TmaxMediaLevels.Primary)
					{
						//	OK to add pages and recording segments
						if(tmaxNodes[0].MediaType == TmaxMediaTypes.Document) return true;
						if(tmaxNodes[0].MediaType == TmaxMediaTypes.Recording) return true;
					}

					//	Can't add children to any other nodes
					return false;
					
				case TreePaneCommands.InsertBefore:
				case TreePaneCommands.InsertAfter:
				
					//	Must be one node selected
					if(tmaxNodes.Count != 1) return false;
					
					//	Must be secondary media
					if(tmaxNodes[0].MediaLevel != TmaxMediaLevels.Secondary) return false;
					
					//	Must be secondary page or recording segment
					if(tmaxNodes[0].MediaType == TmaxMediaTypes.Page) return true;
					
					if((tmaxNodes[0].MediaType == TmaxMediaTypes.Segment) &&
						(tmaxNodes[0].IPrimary.GetMediaType() == TmaxMediaTypes.Recording)) return true;
					
					return false; // Not page or recording segment
					
				case TreePaneCommands.ScriptNewMenu:
				case TreePaneCommands.ScriptNewDesignations:
				case TreePaneCommands.ScriptNewClips:
				case TreePaneCommands.ScriptNewBarcodes:
	
					//	Must be only one node selected
					if(tmaxNodes.Count != 1) return false;
					
					//	Must be a script selected
					if(tmaxNodes[0].IPrimary == null) return false;
					if(tmaxNodes[0].MediaType != TmaxMediaTypes.Script) return false;
					
					return true;
					
				case TreePaneCommands.ScriptBeforeMenu:
				case TreePaneCommands.ScriptBeforeDesignations:
				case TreePaneCommands.ScriptBeforeClips:
				case TreePaneCommands.ScriptBeforeBarcodes:
				case TreePaneCommands.ScriptAfterMenu:
				case TreePaneCommands.ScriptAfterDesignations:
				case TreePaneCommands.ScriptAfterClips:
				case TreePaneCommands.ScriptAfterBarcodes:
	
					//	Must be only one node selected
					if(tmaxNodes.Count != 1) return false;
					
					//	Must be a scene selected
					if(tmaxNodes[0].ISecondary == null) return false;
					if(tmaxNodes[0].MediaType != TmaxMediaTypes.Scene) return false;
					
					//	Can not be a scene that's been placed in a binder
					if(tmaxNodes[0].IBinder != null) return false;
					
					return true;
					
				case TreePaneCommands.Copy:
				
					//	Check each selection
					foreach(CTmaxMediaTreeNode tmaxNode in tmaxNodes)
					{
						//	Must be valid media and can't be related to a deposition
						if(tmaxNode.IPrimary == null) return false;
						if(tmaxNode.IPrimary.GetMediaType() == TmaxMediaTypes.Deposition) return false;
					}
					
					return true;	//	All nodes check out OK					
				
				case TreePaneCommands.Paste:
				case TreePaneCommands.PasteBefore:
				case TreePaneCommands.PasteAfter:
	
					//	Must be single media node selected
					if(tmaxNodes.Count != 1) return false;
					if(tmaxNodes[0].IPrimary == null) return false;
					
					//	Must be some stuff in the clipboard
					if(m_tmaxClipboard == null) return false;
					if(m_tmaxClipboard.ContainsMedia(true) == false) return false;
					
					if(eCommand == TreePaneCommands.Paste)
					{
						//	Can only paste into a script
						return (tmaxNodes[0].MediaType == TmaxMediaTypes.Script);
					}
					else
					{
						//	Can only paste before/after into a scene
						return (tmaxNodes[0].MediaType == TmaxMediaTypes.Scene);
					}
				
				case TreePaneCommands.MoveUp:
				case TreePaneCommands.MoveDown:

					//	Must be only one node selected
					if(tmaxNodes.Count != 1) return false;
				
					//	Can not reorder primary media
					if(tmaxNodes[0].MediaLevel == TmaxMediaLevels.None) return false;
					if(tmaxNodes[0].MediaLevel == TmaxMediaLevels.Primary) return false;
					
					//	Is there room to move up ?
					if((eCommand == TreePaneCommands.MoveUp) &&
					   (tmaxNodes[0].HasSibling(NodePosition.Previous) == false)) return false;
					
					//	Is there room to move down ?
					if((eCommand == TreePaneCommands.MoveDown) &&
						(tmaxNodes[0].HasSibling(NodePosition.Next) == false)) return false;
					
					//	Must be one of these types
					if(tmaxNodes[0].MediaType == TmaxMediaTypes.Page) return true;
					if(tmaxNodes[0].MediaType == TmaxMediaTypes.Scene) return true;
					if((tmaxNodes[0].MediaType == TmaxMediaTypes.Segment) &&
					   (tmaxNodes[0].IPrimary.GetMediaType() == TmaxMediaTypes.Recording)) return true;
					   
					return false; // Can't reorder any other type
				
				case TreePaneCommands.Delete:
				
					//	Check each node
					foreach(CTmaxMediaTreeNode tmaxNode in tmaxNodes)
					{
						//	Must be valid media
						if(tmaxNode.MediaLevel == TmaxMediaLevels.None) return false;
						
						//	Can't delete slides
						if(tmaxNode.MediaType == TmaxMediaTypes.Slide) return false;
						
						//	Can't be a deposition segment
						if((tmaxNode.MediaType == TmaxMediaTypes.Segment) &&
						   (tmaxNode.IPrimary.GetMediaType() == TmaxMediaTypes.Deposition))
								return false;
					}
					
					return true; // All nodes check out						
				
				case TreePaneCommands.Duplicate:
	
					//	Must be only one node selected
					if(tmaxNodes.Count != 1) return false;
					
					//	Must be a script selected
					if(tmaxNodes[0].IPrimary == null) return false;
					if(tmaxNodes[0].MediaType != TmaxMediaTypes.Script) return false;
					
					return true;
					
				case TreePaneCommands.RotateCW:
				case TreePaneCommands.RotateCCW:
	
					//	Must be only one node selected
					if(tmaxNodes.Count != 1) return false;
					
					//	Must be valid media
					if(tmaxNodes[0].GetTmaxRecord(true) != null)
					{
						return CanRotate((CDxMediaRecord)(tmaxNodes[0].GetTmaxRecord(true)));
					}
					else
					{
						return false;
					}
					
				case TreePaneCommands.SortDesignations:
	
					//	Each selection must be a valid script
					foreach(CTmaxMediaTreeNode tmaxNode in tmaxNodes)
					{
						if(tmaxNode.IPrimary == null) return false;
						if(tmaxNode.MediaType != TmaxMediaTypes.Script) return false;
					}
					
					return true;
					
				case TreePaneCommands.MergeScripts:
	
					//	Must be more than one node selected
					if(tmaxNodes.Count <= 1) return false;
					
					//	Each selection must be a valid script
					foreach(CTmaxMediaTreeNode tmaxNode in tmaxNodes)
					{
						if(tmaxNode.IPrimary == null) return false;
						if(tmaxNode.MediaType != TmaxMediaTypes.Script) return false;
					}
					
					return true;
					
				case TreePaneCommands.Clean:
	
					//	See if we have any nodes that qualify
					foreach(CTmaxMediaTreeNode O in tmaxNodes)
					{
						if(O.GetMediaRecord() != null)
						{ 
							if(O.GetMediaRecord().GetCanClean(false) == true)
								return true;
						}
						else if(O.IBinder != null)
						{
							//	Can this binder be cleaned?
							if(O.IBinder.GetCanClean(false) == true)
								return true;
						}
						else
						{
							//	Could be a media node or super node
							if((O.MediaType == TmaxMediaTypes.Document) ||
							   (O.MediaType == TmaxMediaTypes.Script))
								return true;
						}
					
					}// foreach(CTmaxMediaTreeNode O in tmaxNodes)
					
					return false;
					
				case TreePaneCommands.Synchronize:
	
					//	Can one or more of the current selections be renumbered?
					foreach(CTmaxMediaTreeNode O in tmaxNodes)
					{
						if(O.IPrimary == null) continue;
					
						//	These are the only primary types that have children that can be reordered
						if(O.MediaType == TmaxMediaTypes.Document) return true;
						if(O.MediaType == TmaxMediaTypes.Recording) return true; 
						if(O.MediaType == TmaxMediaTypes.Script) return true; 
					
						if(O.MediaType == TmaxMediaTypes.Page)
						{
							//	Does this page have any treatments?
							if(((CDxSecondary)O.ISecondary).GetChildCount() > 0)
								return true;
						}
					
					}// foreach(CTmaxMediaTreeNode O in tmaxNodes)
					
					//	None of the selections can be renumbered
					return false;
					
				case TreePaneCommands.ReportExhibits:
	
					//	All we need are valid node selections
					return true;

				case TreePaneCommands.ReportScripts:
					
					//	Must be at least one script or binder selected
					foreach(CTmaxMediaTreeNode O in tmaxNodes)
					{
						//	Is this a media node?
						if(O.IPrimary != null)
						{
							//	Is this a script
							if(O.IPrimary.GetMediaType() == TmaxMediaTypes.Script)
								return true;
						}
							
						//	Is this a binder?
						else if(O.IBinder != null)
							return true;	//	Assume the binder contains a script
								
						else if(O.MediaType == TmaxMediaTypes.Script)
							return true;
						
					}// foreach(CTmaxMediaTreeNode O in tmaxNodes)
						
					//	Must not be any scripts available
					return false;
				
				case TreePaneCommands.ReportTranscript:
				
					//	Must be at least one script, deposition, or binder selected
					foreach(CTmaxMediaTreeNode O in tmaxNodes)
					{
						//	Is this a media node?
						if(O.IPrimary != null)
						{
							//	Is this a script
							if(O.IPrimary.GetMediaType() == TmaxMediaTypes.Script)
								return true;
							else if(O.IPrimary.GetMediaType() == TmaxMediaTypes.Deposition)
								return true;
						}
							
						//	Is this a binder?
						else if(O.IBinder != null)
							return true;	//	Assume the binder contains a script
								
						else if(O.MediaType == TmaxMediaTypes.Script)
							return true;
						else if(O.MediaType == TmaxMediaTypes.Deposition)
							return true;
						
					}// foreach(CTmaxMediaTreeNode O in tmaxNodes)
						
					//	Must not be any records available
					return false;

				case TreePaneCommands.ReportObjections:

					//	Must have an objections collection
					if(m_tmaxDatabase.Objections == null) return false;
					if(m_tmaxDatabase.Objections.Count == 0) return false;
					
					//	Must be at least one script, deposition or binder selected
					foreach(CTmaxMediaTreeNode O in tmaxNodes)
					{
						//	Is this a media node?
						if(O.IPrimary != null)
						{
							//	Is this a script or deposition?
							if(O.IPrimary.GetMediaType() == TmaxMediaTypes.Script)
								return true;
							else if(O.IPrimary.GetMediaType() == TmaxMediaTypes.Deposition)
								return true;
						}

						//	Is this a binder?
						else if(O.IBinder != null)
							return true;	//	Assume the binder contains a script

						else if(O.MediaType == TmaxMediaTypes.Script)
							return true;
						else if(O.MediaType == TmaxMediaTypes.Deposition)
							return true;

					}// foreach(CTmaxMediaTreeNode O in tmaxNodes)

					//	Must not be any scripts or depositions available
					return false;

				case TreePaneCommands.ExportMenu:
				
					return (GetCommandEnabled(TreePaneCommands.ExportText, tmaxNodes) ||
						    GetCommandEnabled(TreePaneCommands.ExportVideo, tmaxNodes) ||
							GetCommandEnabled(TreePaneCommands.ExportLoadFile, tmaxNodes) ||
							GetCommandEnabled(TreePaneCommands.ExportXmlScript, tmaxNodes) ||
							GetCommandEnabled(TreePaneCommands.ExportXmlBinder, tmaxNodes) ||
							GetCommandEnabled(TreePaneCommands.ExportCodes, tmaxNodes) ||
							GetCommandEnabled(TreePaneCommands.ExportAsciiObjections, tmaxNodes));
							
				case TreePaneCommands.ExportText:
				case TreePaneCommands.ExportXmlScript:
				case TreePaneCommands.ExportXmlBinder:
				case TreePaneCommands.ExportVideo:
				case TreePaneCommands.ExportLoadFile:
				case TreePaneCommands.ExportCodes:
				case TreePaneCommands.ExportCodesDatabase:
				case TreePaneCommands.ExportAsciiObjections:
				
					tmaxItems = GetCmdExportItems(tmaxNodes, GetExportFormat(eCommand), (eCommand == TreePaneCommands.ExportAsciiObjections), true);
					return ((tmaxItems != null) && (tmaxItems.Count > 0));
				
				case TreePaneCommands.BulkUpdate:
				
					tmaxItems = GetCmdBulkUpdateItems(tmaxNodes, true);
					return ((tmaxItems != null) && (tmaxItems.Count > 0));
				
				case TreePaneCommands.ShowScrollText:
				case TreePaneCommands.HideScrollText:
				case TreePaneCommands.SetHighlighter:
				
					tmaxItems = GetCmdDesignationItems(tmaxNodes, true);
					return ((tmaxItems != null) && (tmaxItems.Count > 0));
				
				case TreePaneCommands.SetHighlighter1:
				case TreePaneCommands.SetHighlighter2:
				case TreePaneCommands.SetHighlighter3:
				case TreePaneCommands.SetHighlighter4:
				case TreePaneCommands.SetHighlighter5:
				case TreePaneCommands.SetHighlighter6:
				case TreePaneCommands.SetHighlighter7:
				
					return (m_tmaxDatabase.Highlighters != null);

				case TreePaneCommands.AddObjection:
				case TreePaneCommands.RepeatObjection:

					return (m_tmaxDatabase.ObjectionsEnabled == true);

				default:
				
					break;
			}	
			
			return false;
		
		}// GetCommandEnabled(TreePaneCommands eCommand, CTmaxMediaTreeNode tmaxNode)
		
		/// <summary>This method is called to determine if the specified command should be visible</summary>
		/// <param name="eCommand">The tree pane command enumeration</param>
		/// <param name="tmaxNodes">The current node selections</param>
		/// <returns>true if command should be visible</returns>
		protected virtual bool GetCommandVisible(TreePaneCommands eCommand, CTmaxMediaTreeNodes tmaxNodes)
		{
			switch(eCommand)
			{
				case TreePaneCommands.RotateCCW:
				case TreePaneCommands.RotateCW:
				case TreePaneCommands.SetFilter:
				case TreePaneCommands.RefreshFiltered:
				case TreePaneCommands.SetTargetBinder:
					return false;
			
				default:
				
					return true;
			
			}// switch(eCommand)
			
		}// protected virtual bool GetCommandVisible(TreePaneCommands eCommand, CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method is called to get the shortcut key for the specified command</summary>
		/// <param name="eCommand">The tree pane command enumeration</param>
		/// <returns>The shortcut key if there is one</returns>
		protected virtual Shortcut GetCommandShortcut(TreePaneCommands eCommand)
		{
			switch(eCommand)
			{
				case TreePaneCommands.Copy:
					
					return Shortcut.CtrlC;
					
				case TreePaneCommands.Paste:
					
					return Shortcut.CtrlV;
					
				case TreePaneCommands.Print:
				
					return Shortcut.CtrlP;
					
				case TreePaneCommands.Find:
				
					return Shortcut.CtrlF;
				
				case TreePaneCommands.Presentation:
				
					return Shortcut.F5;

//                case TreePaneCommands.PresentationRecording:

  //                  return Shortcut.F6;

				case TreePaneCommands.AddObjection:

					return Shortcut.CtrlJ;

				case TreePaneCommands.Delete:

					return Shortcut.ShiftDel;

				default:
				
					return Shortcut.None;
			
			}// switch(eCommand)

		}// protected virtual Shortcut GetCommandShortcut(TreePaneCommands eCommand)
		
		/// <summary>This method will retrieve the appropriate drag/drop effects based on the current file transfer selection</summary>
		/// <returns>The appropriate drag drop effects</returns>
		protected virtual DragDropEffects GetFileTransferEffects()
		{
			if(m_tmaxRegOptions != null)
			{
				//	What file transfer method is being used?
				switch(m_tmaxRegOptions.FileTransfer)
				{
					case RegFileTransfers.Link:
						
						return DragDropEffects.Link;
							
					case RegFileTransfers.Move:
						
						return DragDropEffects.Move;
							
					case RegFileTransfers.Copy:
					default:
						
						return DragDropEffects.Copy;
				}
			
			}
			else
			{
				return DragDropEffects.Copy;
			}
			
		}// GetFileTransferEffects()
					
		/// <summary>This method handles events fired when the user starts to drag a node in the tree</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">System event argument object</param>
		protected virtual void OnStartTreeDrag(object sender, System.EventArgs e)
		{
			CTmaxMediaTreeNodes	aSelections = null;
			CTmaxItem		tmaxParent  = null;
			
			//	Just in case...
			Debug.Assert(m_tmaxTreeCtrl != null);
			Debug.Assert(m_tmaxTreeCtrl.IsDisposed == false);
			if((m_tmaxTreeCtrl == null) || (m_tmaxTreeCtrl.IsDisposed == true)) return;
			
			//	Get the current selections
			if((aSelections = m_tmaxTreeCtrl.GetSelections(true)) != null)
			{
				//	Are we allowed to drag these nodes?
				if(CanDrag(aSelections) == true)
				{
					//	Create the command item used to define the parent item
					if((tmaxParent = GetDragItem(aSelections)) != null)
					{
						//	Notify the system
						FireCommand(TmaxCommands.StartDrag, tmaxParent);
						
						//	Start the operation
						m_tmaxTreeCtrl.DoDragDrop(aSelections, (DragDropEffects.Copy | DragDropEffects.Link | DragDropEffects.Move));

						//	Notify the application that the user has stopped dragging
						//
						//	NOTE:	We don't care about the return value because it's up
						//			to the drop target to process the drop action
						FireCommand(TmaxCommands.CompleteDrag);
					
					}// if((tmaxParent = GetDragItem(aSelections)) != null)
			
				}
				else
				{
					//FTI.Shared.Win32.User.MessageBeep(FTI.Shared.Win32.User.MB_ICONEXCLAMATION);
				}
				
			}
			
		}// protected virtual void OnStartTreeDrag(object sender, System.EventArgs e)

		/// <summary>This method builds the event item used for a drag operation from the list of selected nodes</summary>
		/// <param name="tmaxNodes">The collection containing the nodes to be dragged</param>
		/// <returns>The event item required to fire the drag event</returns>
		protected virtual CTmaxItem GetDragItem(CTmaxMediaTreeNodes tmaxNodes)
		{
			CTmaxItem tmaxParent = null;
			
			//	Create the command item used to define the parent item
			if(tmaxNodes[0].Parent != null)
			{
				tmaxParent = GetCommandItem(((CTmaxMediaTreeNode)(tmaxNodes[0].Parent)), false);
			}
			else
			{
				//	Create a null item to contain the source items
				tmaxParent = new CTmaxItem();
				tmaxParent.DataType  = TmaxDataTypes.Media;
				tmaxParent.MediaType = tmaxNodes[0].MediaType;
			}
			
			//	Assign the source items
			tmaxParent.SourceItems = GetCommandItems(tmaxNodes, false);
				
			return tmaxParent;
			
		}// protected virtual CTmaxItem GetDragItem(CTmaxMediaTreeNodes tmaxNodes)

		/// <summary>
		/// This method handles events fired by the draw filter when it 
		///	wants to know what kind of drop actions we want to permit on a node
		/// </summary>
		/// <param name="sender">The draw filter object sending the event</param>
		/// <param name="e">The event arguements object</param>
		protected virtual void OnDrawFilterQueryStateAllowed(object sender, CTmaxBaseTreeFilter.CQueryStateAllowedArgs e)
		{
			//	Let the derived class determine the allowable drop positions
			e.m_eStatesAllowed = GetDropPositionsAllowed();
			m_tmaxTreeFilter.EdgeSensitivity = e.m_objNode.Bounds.Height / 3;
		}
		
		/// <summary>This method is called to get the action to be performed if the user drops at the current position</summary>
		/// <returns>The drop action to be performed</returns>
		protected virtual TreeDropActions GetDropAction()
		{	
			//	Call the appropriate drop action calculator
			switch(m_eDragState)
			{
				case PaneDragStates.Source:
				
					return GetDropSourceAction();
					
				case PaneDragStates.Records:
					
					return GetDropRecordsAction();
					
				default:
				
					return TreeDropActions.None;
			}
		
		}// protected virtual TreeDropActions GetDropAction()
		
		/// <summary>This method is called when the draw filter wants to know which drop positions are allowed</summary>
		/// <returns>The allowed drop positions</returns>
		protected virtual TmaxTreePositions GetDropPositionsAllowed()
		{	
			//	Get the appropriate set of drop positions
			switch(m_eDragState)
			{
				case PaneDragStates.Source:
				
					return GetDropSourcePositionsAllowed();
					
				case PaneDragStates.Records:
					
					return GetDropRecordsPositionsAllowed();
					
				default:
				
					return TmaxTreePositions.None;
			}
		
		}// protected override TmaxTreePositions GetDropPositionsAllowed()
		
		/// <summary>This method is called to get the drag/drop effects for the current position</summary>
		/// <returns>The appropriate drag/drop effects</returns>
		protected virtual System.Windows.Forms.DragDropEffects GetDropEffects()
		{	
			//	Get the appropriate set of drop positions
			switch(m_eDragState)
			{
				case PaneDragStates.Source:
				
					return GetDropSourceEffects();
					
				case PaneDragStates.Records:
					
					return GetDropRecordsEffects();
					
				default:
				
					return DragDropEffects.None;
			}
		
		}// protected virtual System.Windows.Forms.DragDropEffects GetDropEffects()
		
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
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
		
		}// Dispose()
		
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected override void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinTree.Override _override1 = new Infragistics.Win.UltraWinTree.Override();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("Main");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ContextMenu");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("NewMenu");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool3 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("InsertBeforeMenu");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool4 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("InsertAfterMenu");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool5 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ScriptNewMenu");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool6 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ScriptBeforeMenu");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool7 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ScriptAfterMenu");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool8 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ContextMenu");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SetTargetBinder");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool9 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("OpenMenu");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CollapseAll");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExpandAll");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Print");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Find");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool10 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ReportsMenu");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool11 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ImportMenu");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool12 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ExportMenu");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Copy");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Paste");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PasteBefore");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PasteAfter");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool13 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ScriptNewMenu");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool14 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ScriptBeforeMenu");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool15 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ScriptAfterMenu");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("New");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertBefore");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertAfter");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool16 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("NewMenu");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool17 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("InsertBeforeMenu");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool18 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("InsertAfterMenu");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AddObjection");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MoveUp");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MoveDown");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RotateCW");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RotateCCW");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Synchronize");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Clean");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BulkUpdate");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool22 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Duplicate");


            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool23 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MergeScripts");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool197 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AddAudioWaveform");



            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool24 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SortDesignations");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool25 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ShowScrollText");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool26 = new Infragistics.Win.UltraWinToolbars.ButtonTool("HideScrollText");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool19 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("SetHighlighter");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool27 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RefreshSuperNodes");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool28 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SetFilter");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool29 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RefreshFiltered");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool30 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Preferences");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool31 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Preferences");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool32 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertBefore");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool33 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool34 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Viewer");
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool35 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertAfter");
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool36 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Properties");
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool37 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MoveUp");
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool38 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MoveDown");
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool39 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RefreshSuperNodes");
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool40 = new Infragistics.Win.UltraWinToolbars.ButtonTool("New");
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool41 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Copy");
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool42 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Paste");
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool43 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PasteBefore");
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool44 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PasteAfter");
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool45 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Builder");
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool46 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CollapseAll");
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool47 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExpandAll");
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool48 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Synchronize");
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool20 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("NewMenu");
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool49 = new Infragistics.Win.UltraWinToolbars.ButtonTool("NewBinder");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool50 = new Infragistics.Win.UltraWinToolbars.ButtonTool("NewDocument");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool51 = new Infragistics.Win.UltraWinToolbars.ButtonTool("NewRecording");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool52 = new Infragistics.Win.UltraWinToolbars.ButtonTool("NewScript");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool53 = new Infragistics.Win.UltraWinToolbars.ButtonTool("NewBarcodes");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool54 = new Infragistics.Win.UltraWinToolbars.ButtonTool("NewBinder");
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool55 = new Infragistics.Win.UltraWinToolbars.ButtonTool("NewDocument");
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool56 = new Infragistics.Win.UltraWinToolbars.ButtonTool("NewRecording");
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool57 = new Infragistics.Win.UltraWinToolbars.ButtonTool("NewScript");
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool21 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("InsertBeforeMenu");
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool58 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertBinderBefore");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool59 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertDocumentBefore");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool60 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertRecordingBefore");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool61 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertScriptBefore");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool62 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertBarcodesBefore");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool22 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("InsertAfterMenu");
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool63 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertBinderAfter");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool64 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertDocumentAfter");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool65 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertRecordingAfter");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool66 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertScriptAfter");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool67 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertBarcodesAfter");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool68 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertBinderBefore");
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool69 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertDocumentBefore");
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool70 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertRecordingBefore");
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool71 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertScriptBefore");
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool72 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertBinderAfter");
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool73 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertDocumentAfter");
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool74 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertRecordingAfter");
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool75 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertScriptAfter");
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool23 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ScriptNewMenu");
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool76 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ScriptNewBarcodes");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool77 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ScriptNewDesignations");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool78 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ScriptNewClips");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool79 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ScriptNewDesignations");
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool80 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ScriptNewClips");
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool24 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ScriptBeforeMenu");
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool81 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ScriptBeforeBarcodes");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool82 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ScriptBeforeDesignations");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool83 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ScriptBeforeClips");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool25 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ScriptAfterMenu");
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool84 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ScriptAfterBarcodes");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool85 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ScriptAfterDesignations");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool86 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ScriptAfterClips");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool87 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ScriptBeforeDesignations");
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool88 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ScriptBeforeClips");
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool89 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ScriptAfterDesignations");
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool90 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ScriptAfterClips");
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool91 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Tuner");
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool92 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Presentation");
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool161 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PresentationRecording");

            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool93 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SortDesignations");
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool94 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Duplicate");
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool95 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Print");
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool26 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("OpenMenu");
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool96 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Properties");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool154 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Explorer");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool97 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Presentation");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool160 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PresentationRecording");

            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool98 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Builder");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool99 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Tuner");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool100 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Viewer");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool101 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Codes");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool102 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MergeScripts");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool198 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AddAudioWaveform");




            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool27 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ReportsMenu");
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool103 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ReportExhibits");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool104 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ReportScripts");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool105 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ReportTranscript");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool106 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ReportObjections");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool107 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ReportScripts");
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool108 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ReportTranscript");
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool109 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ReportExhibits");
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool110 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Find");
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool111 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportText");
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool112 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportVideo");
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool28 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ExportMenu");
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool113 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportText");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool114 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportXmlBinder");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool115 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportXmlScript");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool116 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportCodes");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool117 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportCodesDatabase");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool118 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportLoadFile");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool119 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportVideo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool120 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportAsciiObjections");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool121 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Clean");
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool122 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ScriptNewBarcodes");
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool123 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ScriptBeforeBarcodes");
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool124 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ScriptAfterBarcodes");
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool125 = new Infragistics.Win.UltraWinToolbars.ButtonTool("NewBarcodes");
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool126 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertBarcodesAfter");
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool127 = new Infragistics.Win.UltraWinToolbars.ButtonTool("InsertBarcodesBefore");
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool128 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RotateCW");
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool129 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RotateCCW");
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool29 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ImportMenu");
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool130 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportAsciiBinders");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool131 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportXmlBinders");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool132 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportAsciiScripts");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool133 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportXmlScripts");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool134 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportAsciiScripts");
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool135 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportAsciiBinders");
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool136 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Codes");
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool137 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportCodes");
            Infragistics.Win.Appearance appearance71 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool138 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SetFilter");
            Infragistics.Win.Appearance appearance72 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool139 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RefreshFiltered");
            Infragistics.Win.Appearance appearance73 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool140 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportLoadFile");
            Infragistics.Win.Appearance appearance74 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool141 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportXmlScripts");
            Infragistics.Win.Appearance appearance75 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool142 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportXmlScript");
            Infragistics.Win.Appearance appearance76 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool143 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportCodesDatabase");
            Infragistics.Win.Appearance appearance77 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool144 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportXmlBinder");
            Infragistics.Win.Appearance appearance78 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool145 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ImportXmlBinders");
            Infragistics.Win.Appearance appearance79 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool146 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BulkUpdate");
            Infragistics.Win.Appearance appearance80 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool147 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ShowScrollText");
            Infragistics.Win.Appearance appearance81 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool148 = new Infragistics.Win.UltraWinToolbars.ButtonTool("HideScrollText");
            Infragistics.Win.Appearance appearance82 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool30 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("SetHighlighter");
            Infragistics.Win.Appearance appearance83 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter1", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter2", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool3 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter3", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool4 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter4", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool5 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter5", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool6 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter6", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool7 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter7", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool8 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter1", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool9 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter2", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool10 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter3", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool11 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter4", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool12 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter5", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool13 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter6", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool14 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("SetHighlighter7", "");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool149 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SetTargetBinder");
            Infragistics.Win.Appearance appearance84 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool150 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AddObjection");
            Infragistics.Win.Appearance appearance85 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool151 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ExportAsciiObjections");
            Infragistics.Win.Appearance appearance86 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool152 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ReportObjections");
            Infragistics.Win.Appearance appearance87 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool153 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Explorer");
            Infragistics.Win.Appearance appearance98 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CTreePane));
            this.m_tmaxTreeCtrl = new FTI.Trialmax.Controls.CTmaxMediaTreeCtrl();
            this.m_ctrlNodeImages = new System.Windows.Forms.ImageList(this.components);
            this.m_ultraToolbarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this.m_ctrlMenuImages = new System.Windows.Forms.ImageList(this.components);
            this._CTreePane_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._CTreePane_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._CTreePane_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._CTreePane_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.m_tmaxTreeCtrl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_ultraToolbarManager)).BeginInit();
            this.SuspendLayout();
            // 
            // m_tmaxTreeCtrl
            // 
            this.m_tmaxTreeCtrl.AllowDrop = true;
            this.m_tmaxTreeCtrl.ClearLeftClick = false;
            this.m_tmaxTreeCtrl.ClearRightClick = false;
            this.m_ultraToolbarManager.SetContextMenuUltra(this.m_tmaxTreeCtrl, "ContextMenu");
            this.m_tmaxTreeCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tmaxTreeCtrl.HideSelection = false;
            this.m_tmaxTreeCtrl.ImageList = this.m_ctrlNodeImages;
            this.m_tmaxTreeCtrl.Location = new System.Drawing.Point(0, 25);
            this.m_tmaxTreeCtrl.Name = "m_tmaxTreeCtrl";
            _override1.Sort = Infragistics.Win.UltraWinTree.SortType.None;
            this.m_tmaxTreeCtrl.Override = _override1;
            this.m_tmaxTreeCtrl.RightClickSelect = false;
            this.m_tmaxTreeCtrl.Size = new System.Drawing.Size(316, 263);
            this.m_tmaxTreeCtrl.TabIndex = 0;
            this.m_tmaxTreeCtrl.AfterCheck += new Infragistics.Win.UltraWinTree.AfterNodeChangedEventHandler(this.OnUltraAfterCheck);
            this.m_tmaxTreeCtrl.AfterCollapse += new Infragistics.Win.UltraWinTree.AfterNodeChangedEventHandler(this.OnUltraAfterCollapse);
            this.m_tmaxTreeCtrl.AfterExpand += new Infragistics.Win.UltraWinTree.AfterNodeChangedEventHandler(this.OnUltraAfterExpand);
            this.m_tmaxTreeCtrl.AfterSelect += new Infragistics.Win.UltraWinTree.AfterNodeSelectEventHandler(this.OnUltraAfterSelect);
            this.m_tmaxTreeCtrl.BeforeCollapse += new Infragistics.Win.UltraWinTree.BeforeNodeChangedEventHandler(this.OnUltraBeforeCollapse);
            this.m_tmaxTreeCtrl.BeforeExpand += new Infragistics.Win.UltraWinTree.BeforeNodeChangedEventHandler(this.OnUltraBeforeExpand);
            this.m_tmaxTreeCtrl.BeforeSelect += new Infragistics.Win.UltraWinTree.BeforeNodeSelectEventHandler(this.OnUltraBeforeSelect);
            this.m_tmaxTreeCtrl.SelectionDragStart += new System.EventHandler(this.OnStartTreeDrag);
            this.m_tmaxTreeCtrl.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this.m_tmaxTreeCtrl.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
            this.m_tmaxTreeCtrl.DragOver += new System.Windows.Forms.DragEventHandler(this.OnDragOver);
            this.m_tmaxTreeCtrl.DragLeave += new System.EventHandler(this.OnDragLeave);
            this.m_tmaxTreeCtrl.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.OnQueryContinueDrag);
            this.m_tmaxTreeCtrl.DoubleClick += new System.EventHandler(this.OnDoubleClick);
            this.m_tmaxTreeCtrl.Enter += new System.EventHandler(this.OnEnterTree);
            this.m_tmaxTreeCtrl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            this.m_tmaxTreeCtrl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // m_ctrlNodeImages
            // 
            this.m_ctrlNodeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlNodeImages.ImageStream")));
            this.m_ctrlNodeImages.TransparentColor = System.Drawing.Color.Magenta;
            this.m_ctrlNodeImages.Images.SetKeyName(0, "");
            this.m_ctrlNodeImages.Images.SetKeyName(1, "");
            this.m_ctrlNodeImages.Images.SetKeyName(2, "");
            this.m_ctrlNodeImages.Images.SetKeyName(3, "");
            this.m_ctrlNodeImages.Images.SetKeyName(4, "");
            this.m_ctrlNodeImages.Images.SetKeyName(5, "");
            this.m_ctrlNodeImages.Images.SetKeyName(6, "treatment.bmp");
            this.m_ctrlNodeImages.Images.SetKeyName(7, "");
            this.m_ctrlNodeImages.Images.SetKeyName(8, "");
            this.m_ctrlNodeImages.Images.SetKeyName(9, "");
            this.m_ctrlNodeImages.Images.SetKeyName(10, "");
            this.m_ctrlNodeImages.Images.SetKeyName(11, "");
            this.m_ctrlNodeImages.Images.SetKeyName(12, "");
            this.m_ctrlNodeImages.Images.SetKeyName(13, "");
            this.m_ctrlNodeImages.Images.SetKeyName(14, "");
            this.m_ctrlNodeImages.Images.SetKeyName(15, "");
            this.m_ctrlNodeImages.Images.SetKeyName(16, "");
            this.m_ctrlNodeImages.Images.SetKeyName(17, "");
            this.m_ctrlNodeImages.Images.SetKeyName(18, "");
            this.m_ctrlNodeImages.Images.SetKeyName(19, "");
            this.m_ctrlNodeImages.Images.SetKeyName(20, "");
            this.m_ctrlNodeImages.Images.SetKeyName(21, "");
            this.m_ctrlNodeImages.Images.SetKeyName(22, "");
            this.m_ctrlNodeImages.Images.SetKeyName(23, "");
            this.m_ctrlNodeImages.Images.SetKeyName(24, "");
            this.m_ctrlNodeImages.Images.SetKeyName(25, "");
            this.m_ctrlNodeImages.Images.SetKeyName(26, "");
            this.m_ctrlNodeImages.Images.SetKeyName(27, "");
            this.m_ctrlNodeImages.Images.SetKeyName(28, "");
            this.m_ctrlNodeImages.Images.SetKeyName(29, "");
            this.m_ctrlNodeImages.Images.SetKeyName(30, "");
            this.m_ctrlNodeImages.Images.SetKeyName(31, "");
            this.m_ctrlNodeImages.Images.SetKeyName(32, "");
            this.m_ctrlNodeImages.Images.SetKeyName(33, "");
            this.m_ctrlNodeImages.Images.SetKeyName(34, "");
            this.m_ctrlNodeImages.Images.SetKeyName(35, "");
            this.m_ctrlNodeImages.Images.SetKeyName(36, "");
            this.m_ctrlNodeImages.Images.SetKeyName(37, "");
            this.m_ctrlNodeImages.Images.SetKeyName(38, "");
            this.m_ctrlNodeImages.Images.SetKeyName(39, "");
            this.m_ctrlNodeImages.Images.SetKeyName(40, "");
            this.m_ctrlNodeImages.Images.SetKeyName(41, "");
            this.m_ctrlNodeImages.Images.SetKeyName(42, "");
            this.m_ctrlNodeImages.Images.SetKeyName(43, "");
            this.m_ctrlNodeImages.Images.SetKeyName(44, "");
            this.m_ctrlNodeImages.Images.SetKeyName(45, "");
            this.m_ctrlNodeImages.Images.SetKeyName(46, "link_hidden_no_text_tuned.bmp");
            this.m_ctrlNodeImages.Images.SetKeyName(47, "link_hidden_no_text_untuned.bmp");
            this.m_ctrlNodeImages.Images.SetKeyName(48, "link_hidden_no_video_tuned.bmp");
            this.m_ctrlNodeImages.Images.SetKeyName(49, "link_hidden_no_video_untuned.bmp");
            this.m_ctrlNodeImages.Images.SetKeyName(50, "link_hidden_blank_tuned.bmp");
            this.m_ctrlNodeImages.Images.SetKeyName(51, "link_hidden_blank_untuned.bmp");
            this.m_ctrlNodeImages.Images.SetKeyName(52, "link_hidden_tuned.bmp");
            this.m_ctrlNodeImages.Images.SetKeyName(53, "link_hidden_untuned.bmp");
            this.m_ctrlNodeImages.Images.SetKeyName(54, "treatment_vertical.bmp");
            this.m_ctrlNodeImages.Images.SetKeyName(55, "treatment_horizontal.bmp");
            // 
            // m_ultraToolbarManager
            // 
            this.m_ultraToolbarManager.AlwaysShowMenusExpanded = Infragistics.Win.DefaultableBoolean.True;
            this.m_ultraToolbarManager.DesignerFlags = 1;
            this.m_ultraToolbarManager.DockWithinContainer = this;
            this.m_ultraToolbarManager.ImageListSmall = this.m_ctrlMenuImages;
            this.m_ultraToolbarManager.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_ultraToolbarManager.ShowFullMenusDelay = 500;
            this.m_ultraToolbarManager.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.Office2003;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            popupMenuTool1.InstanceProps.Visible = Infragistics.Win.DefaultableBoolean.False;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool1,
            popupMenuTool2,
            popupMenuTool3,
            popupMenuTool4,
            popupMenuTool5,
            popupMenuTool6,
            popupMenuTool7});
            ultraToolbar1.ShowInToolbarList = false;
            ultraToolbar1.Text = "Main";
            ultraToolbar1.Visible = false;
            this.m_ultraToolbarManager.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            popupMenuTool8.InstanceProps.Visible = Infragistics.Win.DefaultableBoolean.False;
            popupMenuTool8.SharedProps.Caption = "ContextMenu";
            buttonTool2.InstanceProps.IsFirstInGroup = true;
            buttonTool4.InstanceProps.IsFirstInGroup = true;
            popupMenuTool11.InstanceProps.IsFirstInGroup = true;
            buttonTool6.InstanceProps.IsFirstInGroup = true;
            buttonTool7.InstanceProps.IsFirstInGroup = true;
            popupMenuTool13.InstanceProps.IsFirstInGroup = true;
            buttonTool11.InstanceProps.IsFirstInGroup = true;
            popupMenuTool16.InstanceProps.IsFirstInGroup = true;
            buttonTool14.InstanceProps.IsFirstInGroup = true;
            buttonTool15.InstanceProps.IsFirstInGroup = true;
            buttonTool19.InstanceProps.IsFirstInGroup = true;
            buttonTool22.InstanceProps.IsFirstInGroup = true;
            buttonTool27.InstanceProps.IsFirstInGroup = true;
            buttonTool30.InstanceProps.IsFirstInGroup = true;
            popupMenuTool8.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            popupMenuTool9,
            buttonTool2,
            buttonTool3,
            buttonTool4,
            buttonTool5,
            popupMenuTool10,
            popupMenuTool11,
            popupMenuTool12,
            buttonTool6,
            buttonTool7,
            buttonTool8,
            buttonTool9,
            buttonTool10,
            popupMenuTool13,
            popupMenuTool14,
            popupMenuTool15,
            buttonTool11,
            buttonTool12,
            buttonTool13,
            popupMenuTool16,
            popupMenuTool17,
            popupMenuTool18,
            buttonTool14,
            buttonTool15,
            buttonTool16,
            buttonTool17,
            buttonTool18,
            buttonTool19,
            buttonTool20,
            buttonTool21,
            buttonTool22,
            buttonTool23,
            buttonTool197,
            buttonTool24,
            buttonTool25,
            buttonTool26,
            popupMenuTool19,
            buttonTool27,
            buttonTool28,
            buttonTool29,
            buttonTool30});
            appearance1.Image = 0;
            buttonTool31.SharedProps.AppearancesSmall.Appearance = appearance1;
            buttonTool31.SharedProps.Caption = "Preferences ...";
            appearance2.Image = 5;
            buttonTool32.SharedProps.AppearancesSmall.Appearance = appearance2;
            buttonTool32.SharedProps.Caption = "Insert Before ...";
            appearance3.Image = 2;
            buttonTool33.SharedProps.AppearancesSmall.Appearance = appearance3;
            buttonTool33.SharedProps.Caption = "Delete";
            appearance4.Image = 1;
            buttonTool34.SharedProps.AppearancesSmall.Appearance = appearance4;
            buttonTool34.SharedProps.Caption = "Open in Viewer";
            appearance5.Image = 6;
            buttonTool35.SharedProps.AppearancesSmall.Appearance = appearance5;
            buttonTool35.SharedProps.Caption = "Insert After ...";
            appearance6.Image = 3;
            buttonTool36.SharedProps.AppearancesSmall.Appearance = appearance6;
            buttonTool36.SharedProps.Caption = "Open in Properties";
            appearance7.Image = 9;
            buttonTool37.SharedProps.AppearancesSmall.Appearance = appearance7;
            buttonTool37.SharedProps.Caption = "MoveUp";
            appearance8.Image = 10;
            buttonTool38.SharedProps.AppearancesSmall.Appearance = appearance8;
            buttonTool38.SharedProps.Caption = "MoveDown";
            appearance9.Image = 11;
            buttonTool39.SharedProps.AppearancesSmall.Appearance = appearance9;
            buttonTool39.SharedProps.Caption = "Refresh Super Nodes";
            appearance10.Image = 12;
            buttonTool40.SharedProps.AppearancesSmall.Appearance = appearance10;
            buttonTool40.SharedProps.Caption = "New ...";
            appearance11.Image = 16;
            buttonTool41.SharedProps.AppearancesSmall.Appearance = appearance11;
            buttonTool41.SharedProps.Caption = "Copy";
            appearance12.Image = 13;
            buttonTool42.SharedProps.AppearancesSmall.Appearance = appearance12;
            buttonTool42.SharedProps.Caption = "Paste";
            appearance13.Image = 15;
            buttonTool43.SharedProps.AppearancesSmall.Appearance = appearance13;
            buttonTool43.SharedProps.Caption = "Paste Before";
            appearance14.Image = 14;
            buttonTool44.SharedProps.AppearancesSmall.Appearance = appearance14;
            buttonTool44.SharedProps.Caption = "Paste After";
            appearance15.Image = 31;
            buttonTool45.SharedProps.AppearancesSmall.Appearance = appearance15;
            buttonTool45.SharedProps.Caption = "Open in Builder";
            appearance16.Image = 19;
            buttonTool46.SharedProps.AppearancesSmall.Appearance = appearance16;
            buttonTool46.SharedProps.Caption = "Collapse All";
            appearance17.Image = 18;
            buttonTool47.SharedProps.AppearancesSmall.Appearance = appearance17;
            buttonTool47.SharedProps.Caption = "Expand All";
            appearance18.Image = 20;
            buttonTool48.SharedProps.AppearancesSmall.Appearance = appearance18;
            buttonTool48.SharedProps.Caption = "Renumber Barcodes";
            appearance19.Image = 12;
            popupMenuTool20.SharedProps.AppearancesSmall.Appearance = appearance19;
            popupMenuTool20.SharedProps.Caption = "New";
            popupMenuTool20.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool49,
            buttonTool50,
            buttonTool51,
            buttonTool52,
            buttonTool53});
            appearance20.Image = 21;
            buttonTool54.SharedProps.AppearancesSmall.Appearance = appearance20;
            buttonTool54.SharedProps.Caption = "Binder ...";
            appearance21.Image = 22;
            buttonTool55.SharedProps.AppearancesSmall.Appearance = appearance21;
            buttonTool55.SharedProps.Caption = "Document ...";
            appearance22.Image = 26;
            buttonTool56.SharedProps.AppearancesSmall.Appearance = appearance22;
            buttonTool56.SharedProps.Caption = "Recording ...";
            appearance23.Image = 28;
            buttonTool57.SharedProps.AppearancesSmall.Appearance = appearance23;
            buttonTool57.SharedProps.Caption = "Script ...";
            appearance24.Image = 5;
            popupMenuTool21.SharedProps.AppearancesSmall.Appearance = appearance24;
            popupMenuTool21.SharedProps.Caption = "Insert Before";
            popupMenuTool21.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool58,
            buttonTool59,
            buttonTool60,
            buttonTool61,
            buttonTool62});
            appearance25.Image = 6;
            popupMenuTool22.SharedProps.AppearancesSmall.Appearance = appearance25;
            popupMenuTool22.SharedProps.Caption = "Insert After";
            popupMenuTool22.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool63,
            buttonTool64,
            buttonTool65,
            buttonTool66,
            buttonTool67});
            appearance26.Image = 21;
            buttonTool68.SharedProps.AppearancesSmall.Appearance = appearance26;
            buttonTool68.SharedProps.Caption = "Binder ...";
            appearance27.Image = 22;
            buttonTool69.SharedProps.AppearancesSmall.Appearance = appearance27;
            buttonTool69.SharedProps.Caption = "Document ...";
            appearance28.Image = 26;
            buttonTool70.SharedProps.AppearancesSmall.Appearance = appearance28;
            buttonTool70.SharedProps.Caption = "Recording ...";
            appearance29.Image = 28;
            buttonTool71.SharedProps.AppearancesSmall.Appearance = appearance29;
            buttonTool71.SharedProps.Caption = "Script ...";
            appearance30.Image = 21;
            buttonTool72.SharedProps.AppearancesSmall.Appearance = appearance30;
            buttonTool72.SharedProps.Caption = "Binder ...";
            appearance31.Image = 22;
            buttonTool73.SharedProps.AppearancesSmall.Appearance = appearance31;
            buttonTool73.SharedProps.Caption = "Document ...";
            appearance32.Image = 26;
            buttonTool74.SharedProps.AppearancesSmall.Appearance = appearance32;
            buttonTool74.SharedProps.Caption = "Recording ...";
            appearance33.Image = 28;
            buttonTool75.SharedProps.AppearancesSmall.Appearance = appearance33;
            buttonTool75.SharedProps.Caption = "Script ...";
            appearance34.Image = 12;
            popupMenuTool23.SharedProps.AppearancesSmall.Appearance = appearance34;
            popupMenuTool23.SharedProps.Caption = "New";
            popupMenuTool23.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool76,
            buttonTool77,
            buttonTool78});
            appearance35.Image = 29;
            buttonTool79.SharedProps.AppearancesSmall.Appearance = appearance35;
            buttonTool79.SharedProps.Caption = "Designations ...";
            appearance36.Image = 30;
            buttonTool80.SharedProps.AppearancesSmall.Appearance = appearance36;
            buttonTool80.SharedProps.Caption = "Clips ...";
            appearance37.Image = 5;
            popupMenuTool24.SharedProps.AppearancesSmall.Appearance = appearance37;
            popupMenuTool24.SharedProps.Caption = "Insert Before";
            popupMenuTool24.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool81,
            buttonTool82,
            buttonTool83});
            appearance38.Image = 6;
            popupMenuTool25.SharedProps.AppearancesSmall.Appearance = appearance38;
            popupMenuTool25.SharedProps.Caption = "Insert After";
            popupMenuTool25.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool84,
            buttonTool85,
            buttonTool86});
            appearance39.Image = 29;
            buttonTool87.SharedProps.AppearancesSmall.Appearance = appearance39;
            buttonTool87.SharedProps.Caption = "Designations ...";
            appearance40.Image = 30;
            buttonTool88.SharedProps.AppearancesSmall.Appearance = appearance40;
            buttonTool88.SharedProps.Caption = "Clips ...";
            appearance41.Image = 29;
            buttonTool89.SharedProps.AppearancesSmall.Appearance = appearance41;
            buttonTool89.SharedProps.Caption = "Designations ...";
            appearance42.Image = 30;
            buttonTool90.SharedProps.AppearancesSmall.Appearance = appearance42;
            buttonTool90.SharedProps.Caption = "Clips ...";
            appearance43.Image = 33;
            buttonTool91.SharedProps.AppearancesSmall.Appearance = appearance43;
            buttonTool91.SharedProps.Caption = "Open in Tuner";
            appearance44.Image = 32;
            buttonTool92.SharedProps.AppearancesSmall.Appearance = appearance44;
            buttonTool92.SharedProps.Caption = "Open in Presentation";
            appearance44.Image = 32;
            buttonTool161.SharedProps.AppearancesSmall.Appearance = appearance44;
            buttonTool161.SharedProps.Caption = "Open in Presentation with Recording";
            appearance45.Image = 34;
            buttonTool93.SharedProps.AppearancesSmall.Appearance = appearance45;
            buttonTool93.SharedProps.Caption = "Sort Designations";
            appearance46.Image = 35;
            buttonTool94.SharedProps.AppearancesSmall.Appearance = appearance46;
            buttonTool94.SharedProps.Caption = "Duplicate Script ...";
            appearance47.Image = 36;
            buttonTool95.SharedProps.AppearancesSmall.Appearance = appearance47;
            buttonTool95.SharedProps.Caption = "Print ...";
            appearance48.Image = 44;
            popupMenuTool26.SharedProps.AppearancesSmall.Appearance = appearance48;
            popupMenuTool26.SharedProps.Caption = "Open";
            popupMenuTool26.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool96,
            buttonTool154,
            buttonTool97,
            buttonTool160,
            buttonTool98,
            buttonTool99,
            buttonTool100,
            buttonTool101});
            appearance49.Image = 37;
            buttonTool102.SharedProps.AppearancesSmall.Appearance = appearance49;
            buttonTool102.SharedProps.Caption = "Merge Scripts ...";
            buttonTool198.SharedProps.AppearancesSmall.Appearance = appearance10;
            buttonTool198.SharedProps.Caption = "Add Audio Waveform";
            appearance50.Image = 38;
            popupMenuTool27.SharedProps.AppearancesSmall.Appearance = appearance50;
            popupMenuTool27.SharedProps.Caption = "Reports";
            popupMenuTool27.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool103,
            buttonTool104,
            buttonTool105,
            buttonTool106});
            appearance51.Image = 39;
            buttonTool107.SharedProps.AppearancesSmall.Appearance = appearance51;
            buttonTool107.SharedProps.Caption = "Scripts ...";
            appearance52.Image = 41;
            buttonTool108.SharedProps.AppearancesSmall.Appearance = appearance52;
            buttonTool108.SharedProps.Caption = "Transcript ...";
            appearance53.Image = 42;
            buttonTool109.SharedProps.AppearancesSmall.Appearance = appearance53;
            buttonTool109.SharedProps.Caption = "Exhibits ...";
            buttonTool109.SharedProps.Visible = false;
            appearance54.Image = 45;
            buttonTool110.SharedProps.AppearancesSmall.Appearance = appearance54;
            buttonTool110.SharedProps.Caption = "Find ...";
            appearance55.Image = 57;
            buttonTool111.SharedProps.AppearancesSmall.Appearance = appearance55;
            buttonTool111.SharedProps.Caption = "To Barcode Text File ...";
            appearance56.Image = 46;
            buttonTool112.SharedProps.AppearancesSmall.Appearance = appearance56;
            buttonTool112.SharedProps.Caption = "To Video ...";
            appearance57.Image = 8;
            popupMenuTool28.SharedProps.AppearancesSmall.Appearance = appearance57;
            popupMenuTool28.SharedProps.Caption = "Export";
            buttonTool120.InstanceProps.IsFirstInGroup = true;
            popupMenuTool28.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool113,
            buttonTool114,
            buttonTool115,
            buttonTool116,
            buttonTool117,
            buttonTool118,
            buttonTool119,
            buttonTool120});
            appearance58.Image = 48;
            buttonTool121.SharedProps.AppearancesSmall.Appearance = appearance58;
            buttonTool121.SharedProps.Caption = "Image Clean-up ...";
            appearance59.Image = 49;
            buttonTool122.SharedProps.AppearancesSmall.Appearance = appearance59;
            buttonTool122.SharedProps.Caption = "Barcodes ...";
            appearance60.Image = 49;
            buttonTool123.SharedProps.AppearancesSmall.Appearance = appearance60;
            buttonTool123.SharedProps.Caption = "Barcodes ...";
            appearance61.Image = 49;
            buttonTool124.SharedProps.AppearancesSmall.Appearance = appearance61;
            buttonTool124.SharedProps.Caption = "Barcodes ...";
            appearance62.Image = 49;
            buttonTool125.SharedProps.AppearancesSmall.Appearance = appearance62;
            buttonTool125.SharedProps.Caption = "Barcodes ...";
            appearance63.Image = 49;
            buttonTool126.SharedProps.AppearancesSmall.Appearance = appearance63;
            buttonTool126.SharedProps.Caption = "Barcodes ...";
            appearance64.Image = 49;
            buttonTool127.SharedProps.AppearancesSmall.Appearance = appearance64;
            buttonTool127.SharedProps.Caption = "Barcodes ...";
            appearance65.Image = 50;
            buttonTool128.SharedProps.AppearancesSmall.Appearance = appearance65;
            buttonTool128.SharedProps.Caption = "Rotate Clockwise";
            appearance66.Image = 51;
            buttonTool129.SharedProps.AppearancesSmall.Appearance = appearance66;
            buttonTool129.SharedProps.Caption = "Rotate Counter-Clockwise";
            appearance67.Image = 7;
            popupMenuTool29.SharedProps.AppearancesSmall.Appearance = appearance67;
            popupMenuTool29.SharedProps.Caption = "Import";
            buttonTool132.InstanceProps.IsFirstInGroup = true;
            popupMenuTool29.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool130,
            buttonTool131,
            buttonTool132,
            buttonTool133});
            appearance68.Image = 59;
            buttonTool134.SharedProps.AppearancesSmall.Appearance = appearance68;
            buttonTool134.SharedProps.Caption = "Script(s) from Text File(s) ...";
            appearance69.Image = 63;
            buttonTool135.SharedProps.AppearancesSmall.Appearance = appearance69;
            buttonTool135.SharedProps.Caption = "Binder(s) from Text File(s) ...";
            appearance70.Image = 52;
            buttonTool136.SharedProps.AppearancesSmall.Appearance = appearance70;
            buttonTool136.SharedProps.Caption = "Open in Fielded Data";
            buttonTool136.SharedProps.Enabled = false;
            appearance71.Image = 53;
            buttonTool137.SharedProps.AppearancesSmall.Appearance = appearance71;
            buttonTool137.SharedProps.Caption = "To Fielded Data Text File ...";
            appearance72.Image = 54;
            buttonTool138.SharedProps.AppearancesSmall.Appearance = appearance72;
            buttonTool138.SharedProps.Caption = "Set Filter ...";
            appearance73.Image = 55;
            buttonTool139.SharedProps.AppearancesSmall.Appearance = appearance73;
            buttonTool139.SharedProps.Caption = "Refresh Filtered";
            appearance74.Image = 56;
            buttonTool140.SharedProps.AppearancesSmall.Appearance = appearance74;
            buttonTool140.SharedProps.Caption = "To Load File ...";
            appearance75.Image = 60;
            buttonTool141.SharedProps.AppearancesSmall.Appearance = appearance75;
            buttonTool141.SharedProps.Caption = "Script(s) from XML File ...";
            appearance76.Image = 58;
            buttonTool142.SharedProps.AppearancesSmall.Appearance = appearance76;
            buttonTool142.SharedProps.Caption = "To Script XML File ...";
            appearance77.Image = 61;
            buttonTool143.SharedProps.AppearancesSmall.Appearance = appearance77;
            buttonTool143.SharedProps.Caption = "To Fielded Data Database ...";
            appearance78.Image = 62;
            buttonTool144.SharedProps.AppearancesSmall.Appearance = appearance78;
            buttonTool144.SharedProps.Caption = "To Binder XML File ...";
            appearance79.Image = 64;
            buttonTool145.SharedProps.AppearancesSmall.Appearance = appearance79;
            buttonTool145.SharedProps.Caption = "Binder(s) from XML File ...";
            appearance80.Image = 65;
            buttonTool146.SharedProps.AppearancesSmall.Appearance = appearance80;
            buttonTool146.SharedProps.Caption = "Update Fielded Data ...";
            appearance81.Image = 66;
            buttonTool147.SharedProps.AppearancesSmall.Appearance = appearance81;
            buttonTool147.SharedProps.Caption = "Show Scrolling Text";
            appearance82.Image = 67;
            buttonTool148.SharedProps.AppearancesSmall.Appearance = appearance82;
            buttonTool148.SharedProps.Caption = "Hide Scrolling Text";
            appearance83.Image = 68;
            popupMenuTool30.SharedProps.AppearancesSmall.Appearance = appearance83;
            popupMenuTool30.SharedProps.Caption = "Set Highlighter";
            popupMenuTool30.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool1,
            stateButtonTool2,
            stateButtonTool3,
            stateButtonTool4,
            stateButtonTool5,
            stateButtonTool6,
            stateButtonTool7});
            stateButtonTool8.SharedProps.Caption = "SetHighlighter1";
            stateButtonTool9.SharedProps.Caption = "SetHighlighter2";
            stateButtonTool10.SharedProps.Caption = "SetHighlighter3";
            stateButtonTool11.SharedProps.Caption = "SetHighlighter4";
            stateButtonTool12.SharedProps.Caption = "SetHighlighter5";
            stateButtonTool13.SharedProps.Caption = "SetHighlighter6";
            stateButtonTool14.SharedProps.Caption = "SetHighlighter7";
            appearance84.Image = 69;
            buttonTool149.SharedProps.AppearancesSmall.Appearance = appearance84;
            buttonTool149.SharedProps.Caption = "Set Target Binder";
            appearance85.Image = 70;
            buttonTool150.SharedProps.AppearancesSmall.Appearance = appearance85;
            buttonTool150.SharedProps.Caption = "Add Objection(s) ...";
            appearance86.Image = 71;
            buttonTool151.SharedProps.AppearancesSmall.Appearance = appearance86;
            buttonTool151.SharedProps.Caption = "To Objections Text File ...";
            appearance87.Image = 72;
            buttonTool152.SharedProps.AppearancesSmall.Appearance = appearance87;
            buttonTool152.SharedProps.Caption = "Objections ...";
            appearance98.Image = 11;
            buttonTool153.SharedProps.AppearancesSmall.Appearance = appearance98;
            buttonTool153.SharedProps.Caption = "Open in Windows Explorer";
            this.m_ultraToolbarManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool8,
            buttonTool31,
            buttonTool32,
            buttonTool33,
            buttonTool34,
            buttonTool35,
            buttonTool36,
            buttonTool37,
            buttonTool38,
            buttonTool39,
            buttonTool40,
            buttonTool41,
            buttonTool42,
            buttonTool43,
            buttonTool44,
            buttonTool45,
            buttonTool46,
            buttonTool47,
            buttonTool48,
            popupMenuTool20,
            buttonTool54,
            buttonTool55,
            buttonTool56,
            buttonTool57,
            popupMenuTool21,
            popupMenuTool22,
            buttonTool68,
            buttonTool69,
            buttonTool70,
            buttonTool71,
            buttonTool72,
            buttonTool73,
            buttonTool74,
            buttonTool75,
            popupMenuTool23,
            buttonTool79,
            buttonTool80,
            popupMenuTool24,
            popupMenuTool25,
            buttonTool87,
            buttonTool88,
            buttonTool89,
            buttonTool90,
            buttonTool91,
            buttonTool92,
            buttonTool161,
            buttonTool93,
            buttonTool94,
            buttonTool95,
            popupMenuTool26,
            buttonTool102,
            buttonTool198,
            popupMenuTool27,
            buttonTool107,
            buttonTool108,
            buttonTool109,
            buttonTool110,
            buttonTool111,
            buttonTool112,
            popupMenuTool28,
            buttonTool121,
            buttonTool122,
            buttonTool123,
            buttonTool124,
            buttonTool125,
            buttonTool126,
            buttonTool127,
            buttonTool128,
            buttonTool129,
            popupMenuTool29,
            buttonTool134,
            buttonTool135,
            buttonTool136,
            buttonTool137,
            buttonTool138,
            buttonTool139,
            buttonTool140,
            buttonTool141,
            buttonTool142,
            buttonTool143,
            buttonTool144,
            buttonTool145,
            buttonTool146,
            buttonTool147,
            buttonTool148,
            popupMenuTool30,
            stateButtonTool8,
            stateButtonTool9,
            stateButtonTool10,
            stateButtonTool11,
            stateButtonTool12,
            stateButtonTool13,
            stateButtonTool14,
            buttonTool149,
            buttonTool150,
            buttonTool151,
            buttonTool152,
            buttonTool153});
            this.m_ultraToolbarManager.AfterToolCloseup += new Infragistics.Win.UltraWinToolbars.ToolDropdownEventHandler(this.OnUltraAfterCloseup);
            this.m_ultraToolbarManager.BeforeToolDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventHandler(this.OnUltraBeforeDropDown);
            this.m_ultraToolbarManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.OnUltraToolClick);
            // 
            // m_ctrlMenuImages
            // 
            this.m_ctrlMenuImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlMenuImages.ImageStream")));
            this.m_ctrlMenuImages.TransparentColor = System.Drawing.Color.Magenta;
            this.m_ctrlMenuImages.Images.SetKeyName(0, "");
            this.m_ctrlMenuImages.Images.SetKeyName(1, "");
            this.m_ctrlMenuImages.Images.SetKeyName(2, "");
            this.m_ctrlMenuImages.Images.SetKeyName(3, "");
            this.m_ctrlMenuImages.Images.SetKeyName(4, "");
            this.m_ctrlMenuImages.Images.SetKeyName(5, "");
            this.m_ctrlMenuImages.Images.SetKeyName(6, "");
            this.m_ctrlMenuImages.Images.SetKeyName(7, "");
            this.m_ctrlMenuImages.Images.SetKeyName(8, "");
            this.m_ctrlMenuImages.Images.SetKeyName(9, "");
            this.m_ctrlMenuImages.Images.SetKeyName(10, "");
            this.m_ctrlMenuImages.Images.SetKeyName(11, "");
            this.m_ctrlMenuImages.Images.SetKeyName(12, "");
            this.m_ctrlMenuImages.Images.SetKeyName(13, "");
            this.m_ctrlMenuImages.Images.SetKeyName(14, "");
            this.m_ctrlMenuImages.Images.SetKeyName(15, "");
            this.m_ctrlMenuImages.Images.SetKeyName(16, "");
            this.m_ctrlMenuImages.Images.SetKeyName(17, "");
            this.m_ctrlMenuImages.Images.SetKeyName(18, "");
            this.m_ctrlMenuImages.Images.SetKeyName(19, "");
            this.m_ctrlMenuImages.Images.SetKeyName(20, "");
            this.m_ctrlMenuImages.Images.SetKeyName(21, "");
            this.m_ctrlMenuImages.Images.SetKeyName(22, "");
            this.m_ctrlMenuImages.Images.SetKeyName(23, "");
            this.m_ctrlMenuImages.Images.SetKeyName(24, "");
            this.m_ctrlMenuImages.Images.SetKeyName(25, "");
            this.m_ctrlMenuImages.Images.SetKeyName(26, "");
            this.m_ctrlMenuImages.Images.SetKeyName(27, "");
            this.m_ctrlMenuImages.Images.SetKeyName(28, "");
            this.m_ctrlMenuImages.Images.SetKeyName(29, "");
            this.m_ctrlMenuImages.Images.SetKeyName(30, "");
            this.m_ctrlMenuImages.Images.SetKeyName(31, "");
            this.m_ctrlMenuImages.Images.SetKeyName(32, "");
            this.m_ctrlMenuImages.Images.SetKeyName(33, "");
            this.m_ctrlMenuImages.Images.SetKeyName(34, "");
            this.m_ctrlMenuImages.Images.SetKeyName(35, "");
            this.m_ctrlMenuImages.Images.SetKeyName(36, "");
            this.m_ctrlMenuImages.Images.SetKeyName(37, "");
            this.m_ctrlMenuImages.Images.SetKeyName(38, "");
            this.m_ctrlMenuImages.Images.SetKeyName(39, "");
            this.m_ctrlMenuImages.Images.SetKeyName(40, "");
            this.m_ctrlMenuImages.Images.SetKeyName(41, "");
            this.m_ctrlMenuImages.Images.SetKeyName(42, "");
            this.m_ctrlMenuImages.Images.SetKeyName(43, "");
            this.m_ctrlMenuImages.Images.SetKeyName(44, "open_menu.bmp");
            this.m_ctrlMenuImages.Images.SetKeyName(45, "");
            this.m_ctrlMenuImages.Images.SetKeyName(46, "");
            this.m_ctrlMenuImages.Images.SetKeyName(47, "");
            this.m_ctrlMenuImages.Images.SetKeyName(48, "");
            this.m_ctrlMenuImages.Images.SetKeyName(49, "");
            this.m_ctrlMenuImages.Images.SetKeyName(50, "");
            this.m_ctrlMenuImages.Images.SetKeyName(51, "");
            this.m_ctrlMenuImages.Images.SetKeyName(52, "");
            this.m_ctrlMenuImages.Images.SetKeyName(53, "");
            this.m_ctrlMenuImages.Images.SetKeyName(54, "");
            this.m_ctrlMenuImages.Images.SetKeyName(55, "");
            this.m_ctrlMenuImages.Images.SetKeyName(56, "");
            this.m_ctrlMenuImages.Images.SetKeyName(57, "");
            this.m_ctrlMenuImages.Images.SetKeyName(58, "");
            this.m_ctrlMenuImages.Images.SetKeyName(59, "");
            this.m_ctrlMenuImages.Images.SetKeyName(60, "");
            this.m_ctrlMenuImages.Images.SetKeyName(61, "");
            this.m_ctrlMenuImages.Images.SetKeyName(62, "");
            this.m_ctrlMenuImages.Images.SetKeyName(63, "");
            this.m_ctrlMenuImages.Images.SetKeyName(64, "");
            this.m_ctrlMenuImages.Images.SetKeyName(65, "");
            this.m_ctrlMenuImages.Images.SetKeyName(66, "");
            this.m_ctrlMenuImages.Images.SetKeyName(67, "");
            this.m_ctrlMenuImages.Images.SetKeyName(68, "");
            this.m_ctrlMenuImages.Images.SetKeyName(69, "");
            this.m_ctrlMenuImages.Images.SetKeyName(70, "objection_add.bmp");
            this.m_ctrlMenuImages.Images.SetKeyName(71, "objection_export.bmp");
            this.m_ctrlMenuImages.Images.SetKeyName(72, "objection_report.bmp");
            // 
            // _CTreePane_Toolbars_Dock_Area_Left
            // 
            this._CTreePane_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CTreePane_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._CTreePane_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._CTreePane_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CTreePane_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 25);
            this._CTreePane_Toolbars_Dock_Area_Left.Name = "_CTreePane_Toolbars_Dock_Area_Left";
            this._CTreePane_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 263);
            this._CTreePane_Toolbars_Dock_Area_Left.ToolbarsManager = this.m_ultraToolbarManager;
            // 
            // _CTreePane_Toolbars_Dock_Area_Right
            // 
            this._CTreePane_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CTreePane_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._CTreePane_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._CTreePane_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CTreePane_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(316, 25);
            this._CTreePane_Toolbars_Dock_Area_Right.Name = "_CTreePane_Toolbars_Dock_Area_Right";
            this._CTreePane_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 263);
            this._CTreePane_Toolbars_Dock_Area_Right.ToolbarsManager = this.m_ultraToolbarManager;
            // 
            // _CTreePane_Toolbars_Dock_Area_Top
            // 
            this._CTreePane_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CTreePane_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._CTreePane_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._CTreePane_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CTreePane_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._CTreePane_Toolbars_Dock_Area_Top.Name = "_CTreePane_Toolbars_Dock_Area_Top";
            this._CTreePane_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(316, 25);
            this._CTreePane_Toolbars_Dock_Area_Top.ToolbarsManager = this.m_ultraToolbarManager;
            // 
            // _CTreePane_Toolbars_Dock_Area_Bottom
            // 
            this._CTreePane_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CTreePane_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._CTreePane_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._CTreePane_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CTreePane_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 288);
            this._CTreePane_Toolbars_Dock_Area_Bottom.Name = "_CTreePane_Toolbars_Dock_Area_Bottom";
            this._CTreePane_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(316, 0);
            this._CTreePane_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.m_ultraToolbarManager;
            // 
            // CTreePane
            // 
            this.AllowDrop = true;
            this.Controls.Add(this.m_tmaxTreeCtrl);
            this.Controls.Add(this._CTreePane_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._CTreePane_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._CTreePane_Toolbars_Dock_Area_Top);
            this.Controls.Add(this._CTreePane_Toolbars_Dock_Area_Bottom);
            this.Name = "CTreePane";
            this.Size = new System.Drawing.Size(316, 288);
            ((System.ComponentModel.ISupportInitialize)(this.m_tmaxTreeCtrl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_ultraToolbarManager)).EndInit();
            this.ResumeLayout(false);

		}// InitializeComponent()
		
		/// <summary>This method handles events fired when the user drops on this pane</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">Drag/drop event arguments object</param>
		protected override void OnDragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			m_tmaxTreeFilter.ClearDropHighlight();
		
			//	Notify the application that this is the drop pane
			FireCommand(TmaxCommands.AcceptDrop);
			
			//	If the user was dragging records we need to process the drop
			if(m_eDragState == PaneDragStates.Records)
			{
				//	Notify the derived class
				if(m_dropTarget.eAction != TreeDropActions.None)
					OnDroppedRecords(m_dropTarget.eAction);
			}
			
			//	Perform the base class processing
			base.OnDragDrop(sender, e);
		}

		/// <summary>This method traps all DragLeave events</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">Drag/drop event arguments object</param>
		protected override void OnDragLeave(object sender, System.EventArgs e)
		{
			m_tmaxTreeFilter.ClearDropHighlight();
			m_dropTarget.node = null;
			m_dropTarget.ePosition = TmaxTreePositions.None;
			
			//	Perform the base class processing
			base.OnDragLeave(sender, e);
		}

		/// <summary>This method handles events fired when the user drags across the client area</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">Drag/drop event arguments object</param>
		protected override void OnDragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			Point	objPoint;
			bool	bInvalidate;

			//	Get the position of the mouse in the tree's client coordinates
			objPoint = m_tmaxTreeCtrl.PointToClient(new Point(e.X, e.Y));

			//	Get the node the mouse is over
			m_dropTarget.node = m_tmaxTreeCtrl.GetNode(objPoint);

			//	Give the derived class an opprotunity to adjust the drop node
			AdjustDropNode(ref objPoint);
		
			//	Set the draw filter's highlight node
			//
			//	NOTE:	If the drop node or line position has changed, the
			//			filter is going to fire a QueryStatesAllowed event
			//
			//			See: CTreePane::OnDrawFilterQueryStateAllowed()
			bInvalidate = m_tmaxTreeFilter.SetDropHighlightNode(m_dropTarget.node, objPoint);
				
			//	Update the current drop action
			m_dropTarget.eAction = GetDropAction();

			//	Let the derived class determine what effects should be allowed?
			e.Effect = GetDropEffects();
			
			//	Do we need to redraw the drop filter?
			if(bInvalidate == true)
				m_tmaxTreeCtrl.Invalidate();
			
		}// OnDragOver()

		/// <summary>This method is called when the user has dropped data records in the tree</summary>
		/// <param name="eAction">The action to be taken</param>
		protected virtual void OnDroppedRecords(TreeDropActions eAction)
		{
		}
		
		/// <summary>This function handles all Load events</summary>
		/// <param name="e">System event parameters - no data</param>
		protected override void OnLoad(System.EventArgs e)
		{
			//	Perform the base class processing first
			base.OnLoad(e);
			
			// Attach the draw filter to the tree
			m_tmaxTreeCtrl.DrawFilter = m_tmaxTreeFilter;
			
			//	Multi/single select
			m_tmaxTreeCtrl.Override.SelectionType = Infragistics.Win.UltraWinTree.SelectType.ExtendedAutoDrag;
			//m_tmaxTreeCtrl.Override.SelectionType = Infragistics.Win.UltraWinTree.SelectType.SingleAutoDrag;
			//m_tmaxTreeCtrl.Override.LabelEdit = DefaultableBoolean.False;

			//m_ctrlToolTip.SetToolTip(m_tmaxTreeCtrl, "help");
						
			// Assign the appropriate command enumerator to each tool
			foreach(ToolBase ultraTool in m_ultraToolbarManager.Tools)
			{
				ultraTool.Tag = GetCommand(ultraTool.Key);
			}
		
		}// OnLoadPane()
		
		/// <summary>
		/// This method is called to add a node for the specified source file to the tree
		/// </summary>
		/// <param name="tmaxParent">The parent node to which the source file is being added</param>
		/// <param name="tmaxSource">The source file to be added</param>
		/// <returns>The new node if successful</returns>
		protected virtual CTmaxMediaTreeNode Add(CTmaxMediaTreeNode tmaxParent, CTmaxSourceFile tmaxSource)
		{
			CTmaxMediaTreeNode tmaxNode = null;
		
			//	Make sure we have the required objects
			Debug.Assert(tmaxParent != null);
			Debug.Assert(tmaxParent.Nodes != null);
			Debug.Assert(tmaxSource != null);
			if((tmaxParent == null) || (tmaxParent.Nodes == null) || (tmaxSource == null)) return null;
			
			try
			{
				//	Create the new node
				if((tmaxNode = CreateNode(tmaxSource)) == null) return null;

				//	Add to the parent's child collection
				tmaxParent.Nodes.Add(tmaxNode);
					
				//	Set the property values AFTER adding to the tree
				tmaxNode.SetProperties(GetImageIndex(tmaxNode));
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_BASE_ADD_SOURCE_FILE_EX, tmaxSource.Path), Ex);
			}
			
			return tmaxNode;	
		}

		/// <summary>This method is called to add a a node for the specified record to the tree</summary>
		/// <param name="tmaxParent">The parent node</param>
		/// <param name="tmaxRecord">The record exchange interface</param>
		/// <returns>The newly added node</returns>
		protected virtual CTmaxMediaTreeNode Add(CTmaxMediaTreeNode tmaxParent, ITmaxMediaRecord tmaxRecord)
		{
			CTmaxMediaTreeNode tmaxNode = null;
			
			//	Make sure we have the required objects
			Debug.Assert(tmaxParent != null);
			Debug.Assert(tmaxRecord != null);
			if(tmaxParent == null) return null;
			if(tmaxRecord == null) return null;
			
			//	Create a new node for the record
			if((tmaxNode = CreateNode(tmaxRecord)) == null) 
				return null;

			try
			{
				//	Add to the parent collection
				tmaxParent.Nodes.Add(tmaxNode);
				//	Set the property values AFTER adding to the tree
				tmaxNode.SetProperties(GetImageIndex(tmaxNode));
					
				//	Now add each child record
				switch(tmaxRecord.GetMediaLevel())
				{
					case TmaxMediaLevels.Primary:
					
						if(((CDxPrimary)tmaxRecord).Secondaries != null)
						{
							foreach(CDxSecondary dxChild in ((CDxPrimary)tmaxRecord).Secondaries)
							{
								Add(tmaxNode, dxChild);
							}
							
						}
						break;
						
					case TmaxMediaLevels.Secondary:
					
						if(((CDxSecondary)tmaxRecord).Tertiaries != null)
						{
							foreach(CDxTertiary dxChild in ((CDxSecondary)tmaxRecord).Tertiaries)
							{
								Add(tmaxNode, dxChild);
							}
							
						}
						break;
						
					case TmaxMediaLevels.Tertiary:
					
						if(((CDxTertiary)tmaxRecord).Quaternaries != null)
						{
							foreach(CDxQuaternary dxChild in ((CDxTertiary)tmaxRecord).Quaternaries)
							{
								Add(tmaxNode, dxChild);
							}
							
						}
						break;
						
					case TmaxMediaLevels.Quaternary:
					default:
					
						break;
				
				}// switch(tmaxRecord.GetMediaLevel())
				
				return tmaxNode;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_RECORD_EX, tmaxRecord.GetMediaType(), tmaxRecord.GetAutoId()), Ex);
				return null;	
			}
		
		}// Add(CTmaxMediaTreeNode tmaxParent, ITmaxMediaRecord tmaxRecord)
		
		/// <summary>This method is called to update all nodes associated with the specifid record</summary>
		/// <param name="tmaxNode">The node to be checked and updated if necessary</param>
		/// <param name="tmaxRecord">The record that has been modified</param>
		protected virtual void Update(CTmaxMediaTreeNode tmaxNode, ITmaxMediaRecord tmaxRecord)
		{
			CDxMediaRecord				dxRecord = null;
			CDxSecondary			dxScene = null;
			CDxQuaternary			dxLink = null;
			TmaxMediaRelationships	tmaxRelationship = TmaxMediaRelationships.None;
			
			Debug.Assert(tmaxNode != null);
			Debug.Assert(tmaxRecord != null);
			
			//	Does this node have a valid record?
			if((dxRecord = (CDxMediaRecord)tmaxNode.GetMediaRecord()) != null)
			{
				//	Are they a match?
				if(ReferenceEquals(dxRecord, (CDxMediaRecord)tmaxRecord) == true)
				{
					//	Refresh this node
					Refresh(tmaxNode);
				}
				
				//	Are they related?
				else if((tmaxRelationship = tmaxRecord.GetRelationship(dxRecord)) != TmaxMediaRelationships.None)
				{
					//	Child or grandchild?
					if((tmaxRelationship == TmaxMediaRelationships.Child) ||
					   (tmaxRelationship == TmaxMediaRelationships.Grandchild))
					{
						//	Is this a secondary or tertiary record inside a binder?
						if((tmaxNode.Parent == null) || (((CTmaxMediaTreeNode)(tmaxNode.Parent)).GetMediaRecord() == null))
						{
							Refresh(tmaxNode);
						}
					
					}
				
				}
				
				//	Is this node a script scene?
				else if(dxRecord.MediaType == TmaxMediaTypes.Scene)
				{
					dxScene = (CDxSecondary)dxRecord;
					
					if(dxScene.GetSource() != null)
					{
						//	Is the specified record related to the scene's source record
						if(tmaxRecord.GetRelationship(dxScene.GetSource()) != TmaxMediaRelationships.None)
							Refresh(tmaxNode);
					}
				
				}
				
				//	Is this node a link?
				else if(dxRecord.MediaType == TmaxMediaTypes.Link)
				{
					dxLink = (CDxQuaternary)dxRecord;
					
					if(dxLink.GetSource() != null)
					{
						//	Is the specified record related to the link's source record?
						if(tmaxRecord.GetRelationship(dxLink.GetSource()) != TmaxMediaRelationships.None)
							Refresh(tmaxNode);
					}
				
				}

			}// if(tmaxNode.GetTmaxRecord() != null)
			
			//	Drill down into this node's children
			if((tmaxNode.Nodes != null) && (tmaxNode.Nodes.Count > 0))
			{
				foreach(CTmaxMediaTreeNode O in tmaxNode.Nodes)
				{
					Update(O, tmaxRecord);
				}
				
			}
			
		}// protected virtual void Update(CTmaxMediaTreeNode tmaxNode, ITmaxMediaRecord tmaxRecord)

		/// <summary>This method traps all QueryContinueDrag events</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">System event arguements</param>
		protected override void OnQueryContinueDrag(object sender, System.Windows.Forms.QueryContinueDragEventArgs e)
		{
			//	We have to derive this method to keep the designer from complaining
			base.OnQueryContinueDrag(sender, e);
		}

		/// <summary>This method traps all DragEnter events</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">System event arguements</param>
		protected override void OnDragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			//	We have to derive this method to keep the designer from complaining
			base.OnDragEnter(sender, e);
		}

		/// <summary>
		/// This method is called to create a new node for the specified source folder
		/// </summary>
		/// <param name="tmaxFolder">The source folder to be displayed</param>
		/// <returns>The new node if successful</returns>
		protected virtual CTmaxMediaTreeNode CreateNode(CTmaxSourceFolder tmaxFolder)
		{
			CTmaxMediaTreeNode tmaxNode = null;
			
			//	Is this a registered primary media record?
			if(tmaxFolder.IPrimary != null)
			{
				return CreateNode(tmaxFolder.IPrimary);
			}
			else
			{
				try
				{
					tmaxNode = new CTmaxMediaTreeNode(tmaxFolder.Path);
					tmaxNode.MediaType = TmaxMediaTypes.Unknown;
				
					return tmaxNode;
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "CreateNode", m_tmaxErrorBuilder.Message(ERROR_BASE_CREATE_SOURCE_FOLDER_EX, tmaxFolder.Path), Ex);
					return null;
				}
			
			}
			
		}// CreateNode(CTmaxSourceFolder tmaxFolder)
		
		/// <summary>
		/// This method is called to create a new node for the specified source folder
		/// </summary>
		/// <param name="tmaxFolder">The source folder to be displayed</param>
		/// <returns>The new node if successful</returns>
		protected virtual CTmaxMediaTreeNode CreateNode(ITmaxMediaRecord tmaxRecord)
		{
			CTmaxMediaTreeNode	tmaxNode = null;
			TmaxTextModes	eMode = GetTextMode(tmaxRecord);
			
			try
			{
				//	Create the new node
				tmaxNode = new CTmaxMediaTreeNode(tmaxRecord.GetText(eMode));
				
				//	Set the properties
				tmaxNode.MediaType  = tmaxRecord.GetMediaType();
				tmaxNode.MediaLevel = tmaxRecord.GetMediaLevel();
				
				//	Set the record interfaces
				switch(tmaxNode.MediaLevel)
				{
					case TmaxMediaLevels.Primary:
					
						tmaxNode.IPrimary = tmaxRecord;
						tmaxNode.ISecondary = null;
						tmaxNode.ITertiary = null;
					
						break;
						
					case TmaxMediaLevels.Secondary:
					
						tmaxNode.ISecondary = tmaxRecord;
						tmaxNode.IPrimary = ((CDxSecondary)tmaxRecord).Primary;
						tmaxNode.ITertiary = null;

						break;
						
					case TmaxMediaLevels.Tertiary:
					
						tmaxNode.ITertiary = tmaxRecord;
						tmaxNode.ISecondary = ((CDxTertiary)tmaxRecord).Secondary;;
						if(tmaxNode.ISecondary != null)
							tmaxNode.IPrimary = ((CDxSecondary)(tmaxNode.ISecondary)).Primary;
						
						break;
						
					case TmaxMediaLevels.Quaternary:
					
						tmaxNode.IQuaternary = tmaxRecord;
						tmaxNode.ITertiary = ((CDxQuaternary)tmaxRecord).Tertiary;
						if(tmaxNode.ITertiary != null)
							tmaxNode.ISecondary = ((CDxTertiary)tmaxNode.ITertiary).Secondary;;
						if(tmaxNode.ISecondary != null)
							tmaxNode.IPrimary = ((CDxSecondary)(tmaxNode.ISecondary)).Primary;
						
						break;
						
					default:
					
						Debug.Assert(false);
						break;
						
				}

				return tmaxNode;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateNode", m_tmaxErrorBuilder.Message(ERROR_BASE_CREATE_RECORD_EX, tmaxRecord.GetAutoId(), tmaxRecord.GetMediaType()), Ex);
				return null;
			}
			
		}// CreateNode(CTmaxSourceFolder tmaxFolder)
		
		/// <summary>
		/// This method is called to create a new node for the specified source file
		/// </summary>
		/// <param name="tmaxFile">The source file to be displayed</param>
		/// <returns>The new node if successful</returns>
		protected virtual CTmaxMediaTreeNode CreateNode(CTmaxSourceFile tmaxFile)
		{
			//	Don't bother if the file did not get registered in the database
			if(tmaxFile.ISecondary != null)
			{
				return CreateNode(tmaxFile.ISecondary);
			}
			else
			{
				return null;
			}
		
		}// CreateNode(CTmaxSourceFile tmaxFile)
			
		/// <summary>This method is called to populate the child collection of the specified node</summary>
		/// <param name="tmaxNode">The node to be filled</param>
		/// <returns>true if successful</returns>
		protected virtual bool Fill(CTmaxMediaTreeNode tmaxNode)
		{
			CDxSecondary	dxSecondary = null;
			CDxPrimary		dxPrimary = null;
			CDxTertiary		dxTertiary = null;
			
			Debug.Assert(tmaxNode != null);
			if(tmaxNode == null) return true;
			if(tmaxNode.Nodes == null) return true;
			
			//	Flush the existing nodes
			//
			//	NOTE:	It's left up to the caller to determine if this
			//			method should be called when the child collection
			//			already has nodes
			if(tmaxNode.Nodes.Count > 0) 
				tmaxNode.Nodes.Clear();
			
			//	Is this a quaternary object?
			if(tmaxNode.IQuaternary != null)
			{
				//	Quaternary records have no children
				return false;
			}
				
			//	Is this a tertiary record?
			else if(tmaxNode.ITertiary != null)
			{
				dxTertiary = (CDxTertiary)tmaxNode.ITertiary;
				
				//	Do we need to fill the quaternaries collection?
				if((dxTertiary.Quaternaries == null) || (dxTertiary.Quaternaries.Count == 0))
				{
					dxTertiary.Fill();
				}
				
				//	Add a node for each quaternary
				if(dxTertiary.Quaternaries != null)
				{
					foreach(CDxQuaternary dxq in dxTertiary.Quaternaries)
					{
						Add(tmaxNode, dxq);
					}
				}
			
			}
				
			//	Is this a secondary object?
			else if(tmaxNode.ISecondary != null)
			{
				dxSecondary = (CDxSecondary)tmaxNode.ISecondary;
					
				//	What type of secondary?
				switch(tmaxNode.ISecondary.GetMediaType())
				{
					case TmaxMediaTypes.Page:
					
						//	Do we need to fill the tertiaries collection?
						if((dxSecondary.Tertiaries == null) || (dxSecondary.Tertiaries.Count == 0))
						{
							dxSecondary.Fill();
						}
					
						//	Add a node for each tertiary
						if(dxSecondary.Tertiaries != null)
						{
							foreach(CDxTertiary dxt in dxSecondary.Tertiaries)
							{
								Add(tmaxNode, dxt);
							}
						}
						break;
						
					case TmaxMediaTypes.Scene:
					
						//	Does this scene reference a designation or clip?
						if(dxSecondary.GetSource() != null)
						{
							if((dxSecondary.GetSource().MediaType == TmaxMediaTypes.Designation) ||
							   (dxSecondary.GetSource().MediaType == TmaxMediaTypes.Clip))
							{
								dxTertiary = (CDxTertiary)dxSecondary.GetSource();
								
								//	Make the children of the source appear as children of the scene
								//	Do we need to fill the quaternaries collection?
								if((dxTertiary.Quaternaries == null) || (dxTertiary.Quaternaries.Count == 0))
								{
									dxTertiary.Fill();
								}
				
								//	Add a node for each quaternary
								if(dxTertiary.Quaternaries != null)
								{
									foreach(CDxQuaternary dxq in dxTertiary.Quaternaries)
									{
										Add(tmaxNode, dxq);
									}
								}
							}
							
						}
						break;
					
					case TmaxMediaTypes.Slide:
					case TmaxMediaTypes.Segment:
					
						//	Slide don't have children an we don't show the
						//	children of a segment
						break;
						
					default:
					
						Debug.Assert(false);
						break;
						
				}
				
			}
			else if(tmaxNode.IPrimary != null)
			{
				dxPrimary = (CDxPrimary)tmaxNode.IPrimary;
				
				//	Do we need to fill the secondaries colllection?
				if((dxPrimary.Secondaries == null) || (dxPrimary.Secondaries.Count == 0))
				{
					dxPrimary.Fill();
				}
				
				//	Now add a node for each secondary record
				if(dxPrimary.Secondaries != null)
				{
					foreach(CDxSecondary dxs in dxPrimary.Secondaries)
					{
						Add(tmaxNode, dxs);
					}
				}
			
			}// else if(tmaxNode.IPrimary != null)
			else
			{
				//	Not a valid media node
				return false;	
			}

			//	Make sure the expansion indicator is turned on
			if(tmaxNode.Nodes.Count > 0)
			{
				if(tmaxNode.HasExpansionIndicator == false)
					tmaxNode.Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.Always;
				
				//	Make sure the check state matches the parent if using check boxes
				if(m_bCheckBoxes == true)
				{
					foreach(CTmaxMediaTreeNode O in tmaxNode.Nodes)
						O.CheckedState = tmaxNode.CheckedState;
				}
			
			}
			return true;
		
		}// protected bool Fill(CTmaxMediaTreeNode tmaxNode)

		/// <summary>
		/// This method is called to get the current selection
		/// </summary>
		/// <returns>The node that is currently selected in the tree</returns>
		///	<remarks>This method returns null if more than one node is selected</remarks>
		protected CTmaxMediaTreeNode GetSelection()
		{
			if(m_tmaxTreeCtrl != null)
				return m_tmaxTreeCtrl.GetSelection();
			else
				return null;
		}
		
		/// <summary>
		/// This method is called to get the current selections
		/// </summary>
		/// <returns>The nodes that are currently selected in the tree</returns>
		protected CTmaxMediaTreeNodes GetSelections(bool bSortByPosition)
		{
			if(m_tmaxTreeCtrl != null)
				return m_tmaxTreeCtrl.GetSelections(bSortByPosition);
			else
				return null;
		}
		
		/// <summary>This method will populate a TrialMax node collection using the specified selected node collection</summary>
		/// <param name="aCollection">The Infragistics tree node collection</param>
		/// <returns>The TrialMax node collection</returns>
		protected CTmaxMediaTreeNodes GetTmaxNodes(Infragistics.Win.UltraWinTree.SelectedNodesCollection aCollection)
		{
			CTmaxMediaTreeNodes tmaxNodes = new CTmaxMediaTreeNodes();
			
			if(aCollection != null)
			{
				//	Make sure the nodes are in the same order as they appear in the tree
				aCollection.SortByPosition();
				
				foreach(CTmaxMediaTreeNode tmaxNode in aCollection)
				{
					tmaxNodes.Add(tmaxNode);
				}
				
			}
			
			return tmaxNodes;
			
		}
		
		/// <summary>This method notifies the derived class when a node is being expanded</summary>
		/// <param name="tmaxNode">The node in the tree being expanded</param>
		/// <returns>true to cancel the operation</returns>
		protected virtual bool OnBeforeExpand(CTmaxMediaTreeNode tmaxNode)
		{
			Debug.Assert(tmaxNode != null);
			if(tmaxNode == null) return true;
			if(tmaxNode.Nodes == null) return true;
			
			//	Don't permit expansion of these types
			if(tmaxNode.MediaType == TmaxMediaTypes.Designation) return true;
			if(tmaxNode.MediaType == TmaxMediaTypes.Segment) return true;
			if(tmaxNode.MediaType == TmaxMediaTypes.Clip) return true;

			//	Has the child collection already been populated?
			if(tmaxNode.Nodes.Count > 0) return false; // Ok to continue
			
			//	Display the wait cursor
			System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
			
			//	Fill the child collection if this is a media node
			Fill(tmaxNode);

			System.Windows.Forms.Cursor.Current = Cursors.Default;
			
			//	Don't cancel the operation
			return false;
		
		}
		
		/// <summary>This method notifies the derived class when a node is being collapsed</summary>
		/// <param name="tmaxNode">The node in the tree being collapsed</param>
		/// <returns>true to cancel the operation</returns>
		protected virtual bool OnBeforeCollapse(CTmaxMediaTreeNode tmaxNode)
		{
			Debug.Assert(tmaxNode != null);
			return false;
		}
		
		/// <summary>This method notifies the derived class when a node is being selected</summary>
		/// <param name="tmaxNode">The node in the tree being selected</param>
		/// <returns>true to cancel the operation</returns>
		protected virtual bool OnBeforeSelect(CTmaxMediaTreeNode tmaxNode)
		{
			//	NOTE: tmaxNode could be null if no selection
			
			return false;
		}
		
		/// <summary>This method notifies the derived class when multiple nodes are being selected</summary>
		/// <param name="tmaxNode">The nodes in the tree being selected</param>
		/// <returns>true to cancel the operation</returns>
		protected virtual bool OnBeforeSelect(CTmaxMediaTreeNodes tmaxNodes)
		{
			Debug.Assert(tmaxNodes != null);
			Debug.Assert(tmaxNodes.Count > 0);
			return false;
		}
		
		/// <summary>This method notifies the derived class when a node has been expanded</summary>
		/// <param name="tmaxNode">The node in the tree that was expanded</param>
		/// <returns>true to cancel the operation</returns>
		protected virtual void OnAfterExpand(CTmaxMediaTreeNode tmaxNode)
		{
			//	Make sure all children are visible
			if(tmaxNode != null)
			{
				//	This check is required or Infragistics will cause a stack fault
				//	under some conditions
				if((tmaxNode.Nodes != null) && (tmaxNode.Nodes.Count > 0))
					tmaxNode.BringIntoView(true);
			}
				
//			CTmaxMediaTreeNode tmaxLastChild = null;
//			
//			Debug.Assert(tmaxNode != null);
//			if((tmaxNode != null) && (tmaxNode.Nodes != null) && (tmaxNode.Nodes.Count > 0))
//			{
//				tmaxLastChild = (CTmaxMediaTreeNode)tmaxNode.Nodes[tmaxNode.Nodes.Count - 1];
//				
//				try
//				{
//					//	Prevent processing of Paint messages
//					m_tmaxTreeCtrl.BeginUpdate();
//
//					//	Loop until the last child is visible or the parent
//					//	is all the way to the top of the tree
//					while((tmaxLastChild.IsInView == false) && 
//						  (m_tmaxTreeCtrl.TopNode != null) &&
//						  (m_tmaxTreeCtrl.TopNode != tmaxNode) &&
//						  (m_tmaxTreeCtrl.TopNode.NextVisibleNode != null))
//					{
//						   m_tmaxTreeCtrl.TopNode = m_tmaxTreeCtrl.TopNode.NextVisibleNode;
//					}
//					
//					//	Reenable paint processing
//					m_tmaxTreeCtrl.EndUpdate();
//				}
//				catch
//				{
//				}
//				
//			}
//		
		}
		
		/// <summary>This method notifies the derived class when a node has been collapseed</summary>
		/// <param name="tmaxNode">The node in the tree that was collapsed</param>
		protected virtual void OnAfterCollapse(CTmaxMediaTreeNode tmaxNode)
		{
			Debug.Assert(tmaxNode != null);
		}
		
		/// <summary>This method notifies the derived class when a node has been selected</summary>
		/// <param name="tmaxNode">The node in the tree that was selected</param>
		protected virtual void OnAfterSelect(CTmaxMediaTreeNode tmaxNode)
		{
			//	NOTE: tmaxNode could be null if no selection
			if(tmaxNode != null) 
			{
				if(m_bIgnoreSelection == false)
					FireAsyncCommand(TmaxCommands.Activate, tmaxNode);
				m_tmaxTreeCtrl.Update();
			}
		
		}
		
		/// <summary>This method notifies the derived class when multiple nodes have been selected</summary>
		/// <param name="tmaxNode">The nodes in the tree that were selected</param>
		protected virtual void OnAfterSelect(CTmaxMediaTreeNodes tmaxNodes)
		{
			Debug.Assert(tmaxNodes != null);
			Debug.Assert(tmaxNodes.Count > 0);
		}
		
		/// <summary>
		/// This method is called to populate the error builder's format string collection
		/// </summary>
		/// <remarks>The strings should be added to the collection in the same order in which they are enumerated</remarks>
		protected override void SetErrorStrings()
		{
			//	Do the base class first
			base.SetErrorStrings();
			
			if((m_tmaxErrorBuilder != null) && (m_tmaxErrorBuilder.FormatStrings != null))
			{
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to add the source file to the tree: %1");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to create the source folder node: %1");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to create the database record node: AutoId = %1 MediaType = %2");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to add the source folder to the physical tree: Path = %1");
				m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate root media node to add primary record to the physical tree: MediaType = %1");
				
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to add the specified record to the tree: MediaType = %1 AutoId = %2");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to add the source folder to the virtual tree: Path = %1");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while while processing the tool click notification: Key = %1");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while while converting the tree node to an event item: Node = %1");
				m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate the primary and/or secondary exchange interface to insert a new secondary media object: Tree Node = %1");
				
				m_tmaxErrorBuilder.FormatStrings.Add("Unable to locate media type information to insert a new secondary media object: Tree Node = %1");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to fire the command asynchronously: Command = %1");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to invoke the asynchronous command thread: Command = %1");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to update the preferences");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to add the primary record to the physical tree: AutoId = %1");
				
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to execute the %1 report.");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to merge the scripts.");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to clean the scanned images.");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to %1 scrolling text for the selected designations.");
				m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the highlighter for the selected designations.");
			}
			
		}// SetErrorStrings()

		/// <summary>
		/// This method handles the event fired when the user clicks on Open in Viewer from the context menu
		/// </summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		protected virtual void OnCmdViewer(CTmaxMediaTreeNode tmaxNode)
		{
			CTmaxParameters tmaxParameters = null;

			//	Create the required parameters for the event
			tmaxParameters = new CTmaxParameters();
			tmaxParameters.Add(TmaxCommandParameters.Viewer, true);
				
			if(FireCommand(TmaxCommands.Open, tmaxNode, false, tmaxParameters) == true)
			{
			
			}
			
		}// OnCmdViewer(CTmaxMediaTreeNode tmaxNode)

        /// <summary>
        /// This method handles the event fired when the user clicks on Open in Viewer from the context menu
        /// </summary>
        /// <param name="tmaxNode">The node under the current mouse position</param>
        protected virtual void OnCmdExplorer(CTmaxMediaTreeNode tmaxNode)
        {
            string folderPath = tmaxNode.GetMediaRecord().GetFileSpec();

            if (   tmaxNode.MediaType == TmaxMediaTypes.Document 
                || tmaxNode.MediaType == TmaxMediaTypes.Powerpoint
                || tmaxNode.MediaType == TmaxMediaTypes.Deposition
                || tmaxNode.MediaType == TmaxMediaTypes.Recording
                || tmaxNode.MediaType == TmaxMediaTypes.Script) // Entire document folder selected and not a single file
            {
                System.IO.FileAttributes attr = System.IO.File.GetAttributes(folderPath);
                folderPath = System.IO.Directory.GetParent(folderPath).ToString();
            }
            ShowSelectedInExplorer.FilesOrFolders(folderPath);     
        }// OnCmdViewer(CTmaxMediaTreeNode tmaxNode)

		/// <summary>
		/// This method handles the event fired when the user clicks on Collapse All from the context menu
		/// </summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		protected virtual void OnCmdExpandAll(CTmaxMediaTreeNode tmaxNode)
		{
			if(m_tmaxTreeCtrl != null)
				m_tmaxTreeCtrl.ExpandAll(ExpandAllType.Always);			
		}
				
		/// <summary>
		/// This method handles the event fired when the user clicks on Collapse All from the context menu
		/// </summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		protected virtual void OnCmdCollapseAll(CTmaxMediaTreeNode tmaxNode)
		{
			if(m_tmaxTreeCtrl != null)
				m_tmaxTreeCtrl.CollapseAll();			
		}
				
		/// <summary>This method handles the event fired when the user clicks on Open in Viewer from the context menu</summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		protected virtual void OnCmdBuilder(CTmaxMediaTreeNode tmaxNode)
		{
			CTmaxParameters tmaxParameters = null;
			
			//	Create the required parameters for the event
			tmaxParameters = new CTmaxParameters();
			tmaxParameters.Add(TmaxCommandParameters.Builder, true);
				
			m_tmaxEventSource.FireDiagnostic(this, "OnCmdBuilder", "Firing command to open in builder");
			
			if(FireCommand(TmaxCommands.Open, tmaxNode, false, tmaxParameters) == true)
			{
			
			}
			
		}// OnCmdBuilder(CTmaxMediaTreeNode tmaxNode)
				
		/// <summary>This method handles the event fired when the user clicks on Duplicate from the context menu</summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		///	<returns>An event item that represents the return result</returns>
		protected virtual CTmaxItem OnCmdDuplicate(CTmaxMediaTreeNode tmaxNode)
		{
			CTmaxParameters tmaxParameters = null;
			CTmaxItem		tmaxItem = null;
			
			//	We can only duplicate scripts
			if((tmaxNode != null) && (tmaxNode.IPrimary != null) && (tmaxNode.MediaType == TmaxMediaTypes.Script))
			{
				//	Create an event item to represent the script
				tmaxItem = new CTmaxItem(tmaxNode.IPrimary);
				
				//	Create the required parameters for the event
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.Activate, true);

				FireCommand(TmaxCommands.Duplicate, tmaxItem, tmaxParameters);
			}
			
			if(tmaxItem != null)
				return tmaxItem.ReturnItem;
			else
				return null;
			
		}// OnCmdDuplicate(CTmaxMediaTreeNode tmaxNode)
				
		/// <summary>This method handles the event fired when the user clicks on Print from the context menu</summary>
		/// <param name="tmaxNodes">The collection of selected nodes</param>
		protected virtual void OnCmdPrint(CTmaxMediaTreeNodes tmaxNodes)
		{
			CTmaxItems tmaxItems = null;
			
			//	Get the items that indicate the records for printing
			if((tmaxItems = GetCmdPrintItems(tmaxNodes)) != null)			
			{
				FireCommand(TmaxCommands.Print, tmaxItems);
			}
			else
			{
				FireCommand(TmaxCommands.Print);
			}
			
		}// OnCmdPrint(CTmaxMediaTreeNodes tmaxNodes)
				
		/// <summary>This method handles the event fired when the user clicks on Find from the context menu</summary>
		/// <param name="tmaxNodes">The collection of selected nodes</param>
		protected virtual void OnCmdFind(CTmaxMediaTreeNodes tmaxNodes)
		{
			CTmaxItems tmaxItems = null;

			//	Get the items that indicate the records for searching
			if((tmaxItems = GetCmdFindItems(tmaxNodes)) != null)			
			{
				FireCommand(TmaxCommands.Find, tmaxItems);
			}
			else
			{
				FireCommand(TmaxCommands.Find);
			}
			
		}// OnCmdFind(CTmaxMediaTreeNodes tmaxNodes)
				
		/// <summary>This method handles the event fired when the user clicks on BulkUpdate from the context menu</summary>
		/// <param name="tmaxNodes">The collection of selected nodes</param>
		protected virtual void OnCmdBulkUpdate(CTmaxMediaTreeNodes tmaxNodes)
		{
			CTmaxItems tmaxItems = null;

			//	Get the items that indicate the records for searching
			if((tmaxItems = GetCmdBulkUpdateItems(tmaxNodes, false)) != null)			
			{
				FireCommand(TmaxCommands.BulkUpdate, tmaxItems);
			}
			
		}// protected virtual void OnCmdBulkUpdate(CTmaxMediaTreeNodes tmaxNodes)
				
		/// <summary>This method handles the event fired when the user clicks on Properties from the context menu</summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		protected virtual void OnCmdProperties(CTmaxMediaTreeNode tmaxNode)
		{
			CTmaxParameters tmaxParameters = null;
			
			//	Create the required parameters for the event
			tmaxParameters = new CTmaxParameters();
			tmaxParameters.Add(TmaxCommandParameters.Properties, true);
			
			if(FireCommand(TmaxCommands.Open, tmaxNode, false, tmaxParameters) == true)
			{
			
			}
			
		}// OnCmdProperties(CTmaxMediaTreeNode tmaxNode)
		
		/// <summary>This method handles the event fired when the user clicks on Codes from the context menu</summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		protected virtual void OnCmdCodes(CTmaxMediaTreeNode tmaxNode)
		{
			CTmaxParameters tmaxParameters = null;
			
			//	Create the required parameters for the event
			tmaxParameters = new CTmaxParameters();
			tmaxParameters.Add(TmaxCommandParameters.Codes, true);
			
			if(FireCommand(TmaxCommands.Open, tmaxNode, false, tmaxParameters) == true)
			{
			
			}
			
		}// OnCmdCodes(CTmaxMediaTreeNode tmaxNode)
		
		/// <summary>This method handles the event fired when the user clicks on Set Target Binder from the context menu</summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		protected virtual void OnCmdSetTargetBinder(CTmaxMediaTreeNode tmaxNode)
		{
			//	Implemented in CBinderTree class
		}
		
		/// <summary>This method handles the event fired when the user clicks on Open in Tuner from the context menu</summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		protected virtual void OnCmdTuner(CTmaxMediaTreeNode tmaxNode)
		{
			CTmaxParameters tmaxParameters = null;
			
			//	Create the required parameters for the event
			tmaxParameters = new CTmaxParameters();
			tmaxParameters.Add(TmaxCommandParameters.Tuner, true);
			
			if(FireCommand(TmaxCommands.Open, tmaxNode, false, tmaxParameters) == true)
			{
			
			}
			
		}// OnCmdTuner(CTmaxMediaTreeNode tmaxNode)
		
		/// <summary>This method handles the event fired when the user clicks on Open in Presentation from the context menu</summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		protected virtual void OnCmdPresentation(CTmaxMediaTreeNode tmaxNode, bool? recording=false)
		{
			CTmaxParameters tmaxParameters = null;

			//	Create the required parameters for the event
			tmaxParameters = new CTmaxParameters();
			tmaxParameters.Add(TmaxCommandParameters.Presentation, true);

            string fileName = "recording.ini";
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + fileName;
            if (File.Exists(filePath))
                File.Delete(filePath);

            if (recording.Value) {
                using (TextWriter tw = new StreamWriter(filePath))
                {
               //   tw.WriteLine("[RECORDING]");
                    tw.WriteLine(tmaxNode.FullPath.ToLower().Replace("scripts\\",String.Empty)+ ".wmv");
                }
            }
			if(FireCommand(TmaxCommands.Open, tmaxNode, false, tmaxParameters) == true)
			{
			    
			}
			
		}// OnCmdPresentation(CTmaxMediaTreeNode tmaxNode)

		/// <summary>This method handles the event fired when the user clicks on Refresh Filter from the context menu</summary>
		protected virtual void OnCmdRefreshFiltered()
		{
			FireSetFilterCommand(true);
			
		}// protected virtual void OnCmdRefreshFiltered()
		
		/// <summary>This method handles the event fired when the user clicks on Set Filter from the context menu</summary>
		protected virtual void OnCmdSetFilter()
		{
			FireSetFilterCommand(false);

		}// protected virtual void OnCmdSetFilter()
		
		/// <summary>This method handles the event fired when the user clicks on Sort Designations from the context menu</summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		protected virtual void OnCmdSortDesignations(CTmaxMediaTreeNodes tmaxNodes)
		{
			//	Sort the designations for each selected playlist
			foreach(CTmaxMediaTreeNode O in tmaxNodes)
			{
				if(O.MediaType == TmaxMediaTypes.Script)
				{
					SortDesignations(O);
				}
			}
			
		}// OnCmdSortDesignations(CTmaxMediaTreeNode tmaxNode)
		
		/// <summary>This method handles the event fired when the user clicks on Show / Hide Scrolling Text from the context menu</summary>
		/// <param name="tmaxNodes">The nodes selected by the user</param>
		/// <param name="bScroll">true to show scrolling text on all selected designations</param>
		protected virtual void OnCmdScrollText(CTmaxMediaTreeNodes tmaxNodes, bool bScroll)
		{
			CTmaxItems		tmaxItems = GetCmdDesignationItems(tmaxNodes, false);
			CDxSecondary	dxScene = null;
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
						if((dxScene = (CDxSecondary)(O.GetMediaRecord())) != null)
						{
							if((dxScene.GetSource() != null) && (dxScene.GetSource().MediaType == TmaxMediaTypes.Designation))
							{
								if(((CDxTertiary)(dxScene.GetSource())).ScrollText != bScroll)
								{
									//	Update the designation
									((CDxTertiary)(dxScene.GetSource())).ScrollText = bScroll;
									FireCommand(TmaxCommands.Update, new CTmaxItem(dxScene.GetSource()), tmaxParameters);	
								}
								
							}// if((dxScene.GetSource() != null) && (dxScene.GetSource().MediaType == TmaxMediaTypes.Designation))
						
						}// if((dxScene = (CDxSecondary)(O.GetMediaRecord())) != null)
						
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
		/// <param name="tmaxNodes">The nodes selected by the user</param>
		/// <param name="iCommand">The command identifier</param>
		protected virtual void OnCmdSetHighlighter(CTmaxMediaTreeNodes tmaxNodes, int iCommand)
		{
			CTmaxItems		tmaxItems = null;
			CDxSecondary	dxScene = null;
			CDxTertiary		dxDesignation = null;
			CDxHighlighter	dxHighlighter = null;
			CTmaxParameters	tmaxParameters = null;
			int				iIndex = 0;
			
			//	Get the requested highlighter
			if((m_tmaxDatabase != null) && (m_tmaxDatabase.Highlighters != null))
			{
				iIndex = (iCommand - (int)TreePaneCommands.SetHighlighter1);
			
				if((iIndex >= 0) && (iIndex < m_tmaxDatabase.Highlighters.Count))
					dxHighlighter = m_tmaxDatabase.Highlighters[iIndex];
			
			}// if((m_tmaxDatabase != null) && (m_tmaxDatabase.Highlighters != null))
			
			if(dxHighlighter != null)
			{
				//	Get the designations to be modified
				tmaxItems = GetCmdDesignationItems(tmaxNodes, false);
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
						if((dxScene = (CDxSecondary)(O.GetMediaRecord())) != null)
						{
							if((dxScene.GetSource() != null) && (dxScene.GetSource().MediaType == TmaxMediaTypes.Designation))
							{
								dxDesignation = (CDxTertiary)(dxScene.GetSource());
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
								
							}// if((dxScene.GetSource() != null) && (dxScene.GetSource().MediaType == TmaxMediaTypes.Designation))
						
						}// if((dxScene = (CDxSecondary)(O.GetMediaRecord())) != null)
						
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
			
		}// protected virtual void OnCmdSetHighlighter(CTmaxMediaTreeNodes tmaxNodes, int iCommand)
		
		/// <summary>This method handles the event fired when the user clicks on Merge Scripts from the context menu</summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		protected virtual CTmaxItem OnCmdMergeScripts(CTmaxMediaTreeNodes tmaxNodes)
		{
			CTmaxItem tmaxItem = null;
			
			try
			{
				//	Create an item to identify the media type
				tmaxItem = new CTmaxItem();
				tmaxItem.MediaType = TmaxMediaTypes.Script;
				
				//	Now add subitems for each script being merged
				foreach(CTmaxMediaTreeNode O in tmaxNodes)
				{
					if((O.GetMediaRecord() != null) && (O.GetMediaRecord().GetMediaType() == TmaxMediaTypes.Script))
					{
						tmaxItem.SubItems.Add(new CTmaxItem(O.GetMediaRecord()));
					}
					
				}// foreach(CTmaxMediaTreeNode O in tmaxNodes)
				
				//	Do we have anything to merge?
				if(tmaxItem.SubItems.Count > 1)
				{
					FireCommand(TmaxCommands.Merge, tmaxItem);
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdMergeScripts", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_MERGE_SCRIPTS_EX), Ex);
			}
			
			if(tmaxItem != null)
				return tmaxItem.ReturnItem;
			else
				return null;
				
		}// OnCmdMergeScripts(CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>
		/// This method handles the event fired when the user clicks on Move Up/Down from the context menu
		/// </summary>
		/// <param name="tmaxNodes">The current node selection</param>
		protected virtual void OnCmdMove(CTmaxMediaTreeNode tmaxNode, bool bUp)
		{
			CTmaxMediaTreeNode	tmaxRelative = null;
			NodePosition	ePosition = NodePosition.Previous;
			
			//	NOTE: We don't check for valid parent node here because
			//		  binders can be reordered at the root level. It's up to
			//		  the derived class to make sure this method only gets 
			//		  called where appropriate
			//if(tmaxNode.Parent == null) return;
			
			try
			{
				if(bUp)
				{
					tmaxRelative = (CTmaxMediaTreeNode)tmaxNode.GetSibling(NodePosition.Previous);
					ePosition = NodePosition.Previous;
				}
				else
				{
					tmaxRelative = (CTmaxMediaTreeNode)tmaxNode.GetSibling(NodePosition.Next);
					ePosition = NodePosition.Next;
				}
			
				if(tmaxRelative != null)
				{
					//	Reposition the node
					tmaxNode.Reposition(tmaxRelative, ePosition);
				
					//	Make sure the node is still selected
					tmaxNode.Selected = true;
			
					//	Fire the command requesting reordering
					FireCommand(TmaxCommands.Reorder, (CTmaxMediaTreeNode)tmaxNode.Parent, true);
				}
			
			}
			catch
			{
			}
			
		}// OnCmdMove(CTmaxMediaTreeNodes tmaxNodes, bool bUp)
		
		/// <summary>
		/// This method handles the event fired when the user clicks on Rotate CW/CCW from the context menu
		/// </summary>
		/// <param name="tmaxNodes">The current node selection</param>
		protected virtual void OnCmdRotate(CTmaxMediaTreeNode tmaxNode, bool bCounterClockwise)
		{
			CTmaxParameters tmaxParameters = null;
			
			try
			{
				//	Create the required parameters for the event
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.CounterClockwise, bCounterClockwise);
			
				if(FireCommand(TmaxCommands.Rotate, tmaxNode, false, tmaxParameters) == true)
				{
			
				}
			
			}
			catch
			{
			}
			
		}// OnCmdMove(CTmaxMediaTreeNodes tmaxNodes, bool bUp)
		
		/// <summary>This method handles the event fired when the user clicks on New/Insert from the context menu</summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		/// <param name="bInsert">true if inserting a new node</param>
		/// <param name="bBefore">true to insert before the selected node</param>
		protected virtual void OnCmdNew(CTmaxMediaTreeNode tmaxNode, bool bInsert, bool bBefore)
		{
			CTmaxParameters	tmaxParameters = null;
			CTmaxItem		tmaxItem = null;
			
			//	Don't bother if not a valid node
			if(tmaxNode == null) return;
			
			//	Create the required parameters for the event
			tmaxParameters = new CTmaxParameters();
			tmaxParameters.Add(TmaxCommandParameters.Activate, true);

			//	Are we inserting a new object?
			if(bInsert == true)
			{
				if(tmaxNode.Parent == null) return;
				if(tmaxNode.MediaLevel != TmaxMediaLevels.Secondary) return;
				
				//	Create the required parameters for the event
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.Before, bBefore);
				
				//	Create the command item used to define the parent node
				tmaxItem = GetCommandItem((CTmaxMediaTreeNode)tmaxNode.Parent, false);
				
				//	Put the insertion point in the subitem collection
				tmaxItem.SubItems.Add(GetCommandItem(tmaxNode, false));

				//	Fire the event
				FireCommand(TmaxCommands.Add, tmaxItem, tmaxParameters);
			}
			else
			{
				//	Create the command item but do not include the children
				//
				//	NOTE:	If we include the child nodes the database will interpret these
				//			as insertion points
				FireCommand(TmaxCommands.Add, tmaxNode, false, tmaxParameters);
			}
			
		}// OnCmdNew(CTmaxMediaTreeNode tmaxNode, bool bInsert, bool bBefore)
		
		/// <summary>This method handles the event fired when the user clicks on a selection in the New popup or Insert popup menus</summary>
		/// <param name="tmaxMediaType">Type of media to be added to the tree</param>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		/// <param name="bInsert">true if inserting a new node</param>
		/// <param name="bBefore">true to insert before the selected node</param>
		protected virtual void OnCmdNew(TmaxMediaTypes tmaxMediaType, CTmaxMediaTreeNode tmaxNode, bool bInsert, bool bBefore)
		{
			CTmaxParameters	tmaxParameters = null;
			CTmaxItem		tmaxItem = null;
		
			//	Don't bother if not a valid node
			if(tmaxNode == null) return;
			
			//	Create the required parameters for the event
			tmaxParameters = new CTmaxParameters();
			tmaxParameters.Add(TmaxCommandParameters.Activate, true);

			//	Are we inserting a new object?
			if(bInsert == true)
			{
				if(tmaxNode.Parent == null) return;
				if(tmaxNode.MediaLevel != TmaxMediaLevels.Secondary) return;
				
				//	Create the required parameters for the event
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.Before, bBefore);
				
				//	Create the command item used to define the parent node
				tmaxItem = GetCommandItem((CTmaxMediaTreeNode)tmaxNode.Parent, false);
				
				//	Put the insertion point in the subitem collection
				tmaxItem.SubItems.Add(GetCommandItem(tmaxNode, false));

				//	Fire the event
				FireCommand(TmaxCommands.Add, tmaxItem, tmaxParameters);
			}
			else
			{
				//	Create the command item but do not include the children
				//
				//	NOTE:	If we include the child nodes the database will interpret these
				//			as insertion points
				FireCommand(TmaxCommands.Add, tmaxNode, false, tmaxParameters);
			}
			
		}// OnCmdNew(TmaxMediaTypes tmaxMediaType,CTmaxMediaTreeNode tmaxNode, bool bInsert, bool bBefore)
		
		/// <summary>This method handles the event fired when the user clicks on Delete from the context menu</summary>
		/// <param name="tmaxNodes">The nodes that are currently selected</param>
		protected virtual void OnCmdDelete(CTmaxMediaTreeNodes tmaxNodes)
		{
			CTmaxItem	tmaxParent = null;
			CTmaxItem	tmaxChild = null;
			
			Debug.Assert(tmaxNodes != null);
			Debug.Assert(tmaxNodes.Count > 0);
			
			//	Make sure we have something to delete
			if((tmaxNodes == null) || (tmaxNodes.Count == 0)) return;
			
			//	Can only delete media nodes and binders
			if(tmaxNodes[0].GetTmaxRecord(false) == null) return;
			
			//	Get the command item that will represent the parent
			if(tmaxNodes[0].IBinder != null)
			{
				tmaxParent = GetCommandItem((CTmaxMediaTreeNode)tmaxNodes[0].Parent, false);
			}
			else
			{
				//	Can't delete secondary children of composite media types
				if(tmaxNodes[0].MediaLevel == TmaxMediaLevels.Secondary)
				{
					if(CTmaxMediaTypes.IsCompositeMedia(tmaxNodes[0].IPrimary.GetMediaType()) == true)
						return;
				}
			
				if(tmaxNodes[0].MediaLevel == TmaxMediaLevels.Primary)
				{
					tmaxParent = new CTmaxItem();
					tmaxParent.MediaType = tmaxNodes[0].MediaType;
				}
				else
				{
					Debug.Assert(tmaxNodes[0].Parent != null);
					
					//	Get the parent item
					//
					//	NOTE: We use the ACTUAL parent record to create the parent item
					//		  instead of the parent node because this may be a link and
					//		  the parent node is not the same as the parent record
					if(tmaxNodes[0].GetMediaRecord().GetParent() != null)
						tmaxParent = new CTmaxItem(tmaxNodes[0].GetMediaRecord().GetParent());
				}
			}
			
			if(tmaxParent == null) return;
			
			//	Now add subitems for each child node
			foreach(CTmaxMediaTreeNode O in tmaxNodes)
			{
				if((tmaxChild = GetCommandItem(O, false)) != null)
					tmaxParent.SubItems.Add(tmaxChild);
			}
				
			if(tmaxParent.SubItems.Count > 0)
				FireCommand(TmaxCommands.Delete, tmaxParent);
			
		}// OnCmdDelete(CTmaxMediaTreeNodes tmaxNodes)

		/// <summary>This method handles the event fired when the user clicks on Add Objection from the context menu</summary>
		/// <param name="tmaxNodes">The nodes that are currently selected</param>
		/// <param name="bRepeat">true to repeat last used argument</param>
		protected virtual void OnCmdAddObjection(CTmaxMediaTreeNodes tmaxNodes, bool bRepeat)
		{
			ITmaxMediaRecord ITmaxRecord = null;

			//	Has the user selected a deposition?
			if((tmaxNodes != null) || (tmaxNodes.Count > 0))
			{
				ITmaxRecord = tmaxNodes[0].GetMediaRecord();
				
			}// if((tmaxNodes != null) || (tmaxNodes.Count > 0))

			//	Use the base class to add the objection
			AddObjection((CDxMediaRecord)ITmaxRecord, true, bRepeat);

		}// protected virtual void OnCmdAddObjection(CTmaxMediaTreeNodes tmaxNodes)

		/// <summary>This method handles the event fired when the user clicks on New Designations from the Script New Menu</summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		/// <param name="bInsert">true if inserting a new node</param>
		/// <param name="bBefore">true to insert before the selected node</param>
		protected virtual void OnCmdNewDesignations(CTmaxMediaTreeNode tmaxNode, bool bInsert, bool bBefore)
		{
			CFAddDesignations Add = new CFAddDesignations();
			
			Add.PaneId = PaneId;
			m_tmaxEventSource.Attach(Add.EventSource);
			Add.TmaxCommandEvent += new FTI.Shared.Trialmax.TmaxCommandHandler(this.OnTmaxCommand);
			Add.Database = m_tmaxDatabase;
			
			if((m_tmaxStationOptions != null) && (m_tmaxStationOptions.LastDeposition > 0))
			{
				if((m_tmaxDatabase.Primaries != null) && (m_tmaxStationOptions.LastDeposition > 0))
					Add.Deposition = m_tmaxDatabase.Primaries.Find(m_tmaxStationOptions.LastDeposition);
				if((m_tmaxDatabase.Highlighters != null) && (m_tmaxStationOptions.LastHighlighter > 0))
					Add.Highlighter = m_tmaxDatabase.Highlighters.Find(m_tmaxStationOptions.LastHighlighter);
			}
			
			if(bInsert == true)
			{
				Add.Scene = (CDxSecondary)tmaxNode.GetTmaxRecord(true);
				Add.Script = Add.Scene.Primary;
				Add.InsertBefore = bBefore;
			}
			else
			{
				Add.Script = (CDxPrimary)tmaxNode.GetTmaxRecord(true);
				Add.Scene = null;
				Add.InsertBefore = false;
			}
			
			Add.ShowDialog();
		}
				
		/// <summary>
		/// This method handles the event fired when the user clicks on New Clips from the Script New Menu
		/// </summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		/// <param name="bInsert">true if inserting a new node</param>
		/// <param name="bBefore">true to insert before the selected node</param>
		protected virtual void OnCmdNewClips(CTmaxMediaTreeNode tmaxNode, bool bInsert, bool bBefore)
		{
			CFAddClips Add = new CFAddClips();
			
			Add.PaneId = PaneId;
			m_tmaxEventSource.Attach(Add.EventSource);
			Add.TmaxCommandEvent += new FTI.Shared.Trialmax.TmaxCommandHandler(this.OnTmaxCommand);
			Add.Database = m_tmaxDatabase;
			
			if(bInsert == true)
			{
				Add.Scene = (CDxSecondary)tmaxNode.GetTmaxRecord(true);
				Add.Script = Add.Scene.Primary;
				Add.InsertBefore = bBefore;
			}
			else
			{
				Add.Script = (CDxPrimary)tmaxNode.GetTmaxRecord(true);
				Add.Scene = null;
				Add.InsertBefore = false;
			}
			
			Add.ShowDialog();
		}
				
		/// <summary>
		/// This method handles the event fired when the user clicks on New Barcodes from the Script New Menu
		/// </summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		/// <param name="bInsert">true if inserting a new node</param>
		/// <param name="bBefore">true to insert before the selected node</param>
		protected virtual void OnCmdNewBarcodes(CTmaxMediaTreeNode tmaxNode, bool bInsert, bool bBefore)
		{
			CFAddBarcodes		Add = new CFAddBarcodes();
			CDxMediaRecord		dxParent = null;
			CTmaxMediaTreeNode	tmaxParent = null;
			
			Add.PaneId = PaneId;
			m_tmaxEventSource.Attach(Add.EventSource);
			Add.TmaxCommandEvent += new FTI.Shared.Trialmax.TmaxCommandHandler(this.OnTmaxCommand);
			Add.Database = m_tmaxDatabase;
			
			if(bInsert == true)
			{
				//	Get the parent of the current selection
				if((tmaxParent = (CTmaxMediaTreeNode)(tmaxNode.Parent)) != null)
				{
					//	Does the parent reference a media record?
					if(tmaxParent.GetMediaRecord() != null)
					{
						//	Only allow the user to insert barcodes into scripts
						if(tmaxParent.GetMediaRecord().GetMediaType() == TmaxMediaTypes.Script)
							dxParent = (CDxMediaRecord)(tmaxParent.GetMediaRecord());
					}
					else
					{
						//	Parent must be a binder
						dxParent = (CDxMediaRecord)tmaxParent.IBinder;
					}
					
				}// if((tmaxParent = (CTmaxMediaTreeNode)(tmaxNode.Parent)) != null)
				
				//	If we are inserting barcodes (media) we must have a parent
				if(dxParent != null)
				{
					Add.Target = dxParent;
					Add.InsertBefore = bBefore;
					
					//	If the parent is a binder the child must also be a binder entry
					if(dxParent.GetDataType() == TmaxDataTypes.Binder)
						Add.InsertAt = (CDxMediaRecord)(tmaxNode.IBinder);
					else
						Add.InsertAt = (CDxMediaRecord)(tmaxNode.GetMediaRecord());
						
				}// if(dxParent != null)
				
			}
			else
			{
				//	Does this node reference a media record?
				if(tmaxNode.GetMediaRecord() != null)
				{
					//	Only allowed to new scenes to the end of a script
					if(tmaxNode.MediaType == TmaxMediaTypes.Script)
						Add.Target = (CDxMediaRecord)(tmaxNode.GetMediaRecord());
				}
				else
				{
					//	Must be a binder node if not media
					if(tmaxNode.IBinder != null)
						Add.Target = (CDxMediaRecord)tmaxNode.IBinder;
				}

				Add.InsertAt = null;
				Add.InsertBefore = false;
			}
			
			//	Were we able to assign a target?
			if(Add.Target != null)
			{
				Add.ShowDialog();
			}
			else
			{
				Debug.Assert(false);
				m_tmaxEventSource.FireDiagnostic(this, "OnCmdNewBarcodes", "Invalid node selection");
			}
			
		}// protected virtual void OnCmdNewBarcodes(CTmaxMediaTreeNode tmaxNode, bool bInsert, bool bBefore)
				
		/// <summary>
		/// This method handles the event fired when the user clicks on Synchronize from the context menu
		/// </summary>
		/// <param name="tmaxNodes">The nodes that are currently selected</param>
		protected virtual void OnCmdSynchronize(CTmaxMediaTreeNodes tmaxNodes)
		{
			CTmaxItems tmaxItems = null;
			
			//	Prompt for confirmation before continuing
			if(MessageBox.Show("This action will change the barcodes for selected media. Existing references to those barcodes will be invalidated. Do you want to continue?",
							   "Barcodes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
			{
				//	Build the event items list for this command
				tmaxItems = new CTmaxItems();
				
				foreach(CTmaxMediaTreeNode O in tmaxNodes)
				{
					//	Is this a valid media record?
					if(O.GetMediaRecord() != null)
					{
						//	What type of media?
						switch(O.MediaType)
						{
							case TmaxMediaTypes.Document:
							case TmaxMediaTypes.Recording:
							case TmaxMediaTypes.Script:
							
								tmaxItems.Add(new CTmaxItem(O.GetMediaRecord()));
								break;
								
							case TmaxMediaTypes.Page:
							
								//	Does this page have any treatments?
								if(((CDxSecondary)O.ISecondary).GetChildCount() > 0)
								{
									tmaxItems.Add(new CTmaxItem(O.GetMediaRecord()));
								}
								break;
								
						}// switch(O.MediaType)
						
					}// if(O.GetMediaRecord() != null)
					
				}// foreach(CTmaxMediaTreeNode O in tmaxNodes)
				
				//	Do we have any items to be synchronized?
				if(tmaxItems.Count > 0)
					FireCommand(TmaxCommands.Synchronize, tmaxItems);
			
			}
			
		}// OnCmdSynchronize(CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>
		/// This method handles the event fired when the user clicks on Copy from the context menu
		/// </summary>
		/// <param name="tmaxNodes">The nodes that are currently selected</param>
		protected virtual void OnCmdCopy(CTmaxMediaTreeNodes tmaxNodes)
		{
			if(FireCommand(TmaxCommands.Copy, tmaxNodes, false) == true)
			{
			
			}
			
		}// OnCmdCopy(CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method handles the event fired when the user clicks on Cleanup from the context menu</summary>
		/// <param name="tmaxNodes">The nodes that are currently selected</param>
		protected virtual void OnCmdClean(CTmaxMediaTreeNodes tmaxNodes)
		{
			CTmaxItems		tmaxItems = null;
			CFCleanScanned	cfClean = null;

			//	Get the items that indicate the records for printing
			if((tmaxItems = GetCmdCleanItems(tmaxNodes)) == null)
			{
				MessageBox.Show("No records suitable for clean up operation have been selected.", "",
								MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}			
			
			try
			{
				cfClean = new FTI.Trialmax.Panes.CFCleanScanned();
				
				m_tmaxEventSource.Attach(cfClean.EventSource);
				cfClean.TmaxCommandEvent += new FTI.Shared.Trialmax.TmaxCommandHandler(this.OnTmaxCommand);
				
				cfClean.Database = m_tmaxDatabase;
				cfClean.Items = tmaxItems;
				cfClean.CleanOptions = m_tmaxAppOptions.CleanOptions;
				
				cfClean.ShowDialog();
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdClean", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_CLEAN_SCANNED_EX), Ex);
			}
			
		}// OnCmdClean(CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method handles the event fired when the user clicks on one of the Paste commands in the context menu</summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		/// <param name="bInsert">true if inserting a new node</param>
		/// <param name="bBefore">true to insert before the selected node</param>
		protected virtual void OnCmdPaste(CTmaxMediaTreeNode tmaxNode, bool bInsert, bool bBefore)
		{
			//	Make sure we have the required objects
			if(tmaxNode == null) return;
			if(m_tmaxClipboard == null) return;
			
			Add(tmaxNode, m_tmaxClipboard, bInsert, bBefore);
			
		}// OnCmdPaste(CTmaxMediaTreeNode tmaxNode, bool bInsert, bool bBefore)
		
		/// <summary>
		/// This method handles the event fired when the user clicks on Preferences from the context menu
		/// </summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		protected virtual void OnCmdPreferences(CTmaxMediaTreeNode tmaxNode)
		{
			CFTreePreferences Preferences = null;
			
			try
			{
				Preferences = new CFTreePreferences();
		
				//	Initialize the preferences dialog
				Preferences.Initialize(this);
			
				//	Open the dialog
				if(Preferences.ShowDialog() == DialogResult.OK)
				{
					//	Give the derived class a chance to do custom processing
					OnPreferencesChanged(Preferences);
				}
				
			}
			catch
			{
			}
			finally
			{
				if(Preferences != null)
					Preferences.Dispose();
			}

		}// OnCmdPreferences(CTmaxMediaTreeNode tmaxNode)
		
		/// <summary>
		/// This method handles the event fired when the user clicks on Refresh Super Nodes from the context menu
		/// </summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		protected virtual void OnCmdRefreshSuperNodes(CTmaxMediaTreeNode tmaxNode)
		{
			
		}// OnCmdRefreshSuperNodes(CTmaxMediaTreeNode tmaxNode)
		
		/// <summary>This method handles the event fired when the user clicks on Export from the context menu</summary>
		/// <param name="tmaxNodes">The current node selections</param>
		/// <param name="eFormat">The TrialMax export format enumerations</param>
		/// <param name="bObjections">true if exporting objections</param>
		protected virtual void OnCmdExport(CTmaxMediaTreeNodes tmaxNodes, TmaxExportFormats eFormat, bool bObjections)
		{
			CTmaxItems		tmaxItems = null;
			CTmaxParameters	tmaxParameters = null;
			
			//	Get the items to be passed with the event
			if((tmaxItems = GetCmdExportItems(tmaxNodes, eFormat, bObjections, false)) != null)			
			{
				//	Add the format parameter
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.ExportFormat, (int)eFormat));
				
				if(bObjections == true)
					tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.Objections, true));
				
				FireCommand(TmaxCommands.Export, tmaxItems, tmaxParameters);
			}
			
		}// protected virtual void OnCmdExport(CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method handles the event fired when the user clicks on one of the selections in the Import submenu</summary>
		/// <param name="tmaxNodes">The current node selections</param>
		/// <param name="eCommand">The import command selected by the user</param>
		///	<returns>The return item provided by the database</returns>
		protected virtual CTmaxItem OnCmdImport(CTmaxMediaTreeNodes tmaxNodes, TreePaneCommands eCommand)
		{
			CTmaxItem		tmaxItem = null;
			CTmaxParameters	tmaxParameters = null;
			CDxMediaRecord	dxTarget = null;
			bool			bBinders = false;

			try
			{
				//	Are we importing binders?
				switch(eCommand)
				{
					case TreePaneCommands.ImportAsciiBinders:
					case TreePaneCommands.ImportXmlBinders:
					
						bBinders = true;
						break;
						
					default:
					
						bBinders = false;
						break;
				
				}// switch(eCommand)
				
				//	Get the target node if importing binders
				if((bBinders == true) && (tmaxNodes != null) && (tmaxNodes.Count == 1))
				{
					//	The target node for binders has to be a binder
					if(tmaxNodes[0].IBinder != null)
						dxTarget = (CDxMediaRecord)(tmaxNodes[0].IBinder);
				}
				
				//	Allocate an item for the event
				tmaxItem = new CTmaxItem(dxTarget);
				
				//	Initialize the event parameters
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.Activate, true));
				tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.MergeImported, false));

				//	Are we importing binders?
				if(bBinders == true)
				{
					tmaxItem.DataType  = TmaxDataTypes.Binder;
					tmaxItem.MediaType = TmaxMediaTypes.Unknown;
					if(eCommand == TreePaneCommands.ImportXmlBinders)
						tmaxParameters.Add(TmaxCommandParameters.ImportFormat, (int)TmaxImportFormats.XmlBinder);
					else
						tmaxParameters.Add(TmaxCommandParameters.ImportFormat, (int)TmaxImportFormats.AsciiBinder);
				}
				else
				{
					tmaxItem.DataType  = TmaxDataTypes.Media;
					tmaxItem.MediaType = TmaxMediaTypes.Script;
					if(eCommand == TreePaneCommands.ImportXmlScripts)
						tmaxParameters.Add(TmaxCommandParameters.ImportFormat, (int)TmaxImportFormats.XmlScript);
					else
						tmaxParameters.Add(TmaxCommandParameters.ImportFormat, (int)TmaxImportFormats.AsciiMedia);
				}

				FireCommand(TmaxCommands.Import, tmaxItem, tmaxParameters);
			}
			catch
			{
			}
			
			if(tmaxItem != null)
				return tmaxItem.ReturnItem;
			else
				return null;
			
		}// protected virtual CTmaxItem OnCmdImport(CTmaxMediaTreeNodes tmaxNodes, bool bScripts)
		
		/// <summary>This method handles the event fired when the user clicks on Playlist Summary from the Reports submenu</summary>
		/// <param name="tmaxNode">The current node selections</param>
		/// <summary>This method handles the event fired when the user clicks on a selection in the Reports subment</summary>
		/// <param name="eReport">The enumerated report identifier</param>
		/// <param name="tmaxNodes">The current node selections</param>
		protected virtual void OnCmdReport(TmaxReports eReport, CTmaxMediaTreeNodes tmaxNodes)
		{
			CTmaxItems	tmaxItems = null;
			
			Debug.Assert(m_tmaxDatabase != null);
			Debug.Assert(m_tmaxReportManager != null);
			if(m_tmaxDatabase == null) return;
			if(m_tmaxReportManager == null) return;
			
			//	Get the items to be included in the report
			tmaxItems = GetCmdReportItems(eReport, tmaxNodes);
			
			//	Let the report manager execute the report
			try
			{
				m_tmaxReportManager.Execute(eReport, tmaxItems);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdReport", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_REPORT_EX, eReport), Ex);
			}
		
		}// protected virtual void OnCmdReport(TmaxReports eReport, CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method is called to get the list of items used to fire the print command</summary>
		/// <param name="tmaxNodes">The collection of selected nodes</param>
		protected virtual CTmaxItems GetCmdPrintItems(CTmaxMediaTreeNodes tmaxNodes)
		{
			CTmaxMediaTreeNodes tmaxPrint = null;
			
			if(tmaxNodes == null) return null;
			if(tmaxNodes.Count == 0) return null;
			
			//	Is the user attempting to print media records?
			if(tmaxNodes[0].IPrimary != null)
			{
				//	Can print anything except depositions
				if(tmaxNodes[0].IPrimary.GetMediaType() != TmaxMediaTypes.Deposition)
				{
					tmaxPrint = tmaxNodes;
				}
				
			}// if(tmaxNodes[0].IPrimary != null)
			
			//	Are we printing binders?
			else if(tmaxNodes[0].IBinder != null)
			{
				tmaxPrint = tmaxNodes;
			}
			
			//	Convert the nodes to event items
			if(tmaxPrint != null)
				return GetCommandItems(tmaxPrint, false);
			else
				return null;
		
		}// protected virtual CTmaxItems GetCmdPrintItems(CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method is called by the application to get a list of items that represent the current selections for a Print command</summary>
		/// <returns>The items that represent the current selections</returns>
		public override CTmaxItems GetCmdPrintItems()
		{
			return GetCmdPrintItems(GetSelections(true));
		
		}// public override CTmaxItems GetCmdPrintItems()
		
		/// <summary>This method is called to get the list of items used to fire the Find command</summary>
		/// <param name="tmaxNodes">The collection of selected nodes</param>
		protected virtual CTmaxItems GetCmdFindItems(CTmaxMediaTreeNodes tmaxNodes)
		{
			CTmaxItems	tmaxItems = null;
			CDxPrimary	dxPrimary = null;
			
			if(tmaxNodes == null) return null;
			if(tmaxNodes.Count == 0) return null;
			
			//	Is the user attempting to sarch media records?
			if((dxPrimary = ((CDxPrimary)(tmaxNodes[0].IPrimary))) != null)
			{
				//	What type of media has been selected?
				switch(tmaxNodes[0].MediaType)
				{
					case TmaxMediaTypes.Deposition:
					case TmaxMediaTypes.Script:
					
						tmaxItems = GetCommandItems(tmaxNodes, false);
						break;
						
					case TmaxMediaTypes.Segment:
					
						//	Do these segments belong to a deposition?
						if(dxPrimary.MediaType == TmaxMediaTypes.Deposition)
						{
							//	Add an item for the owner deposition
							tmaxItems = new CTmaxItems();
							tmaxItems.Add(new CTmaxItem(dxPrimary));
						}
						break;
						
				}
				
			}// if(tmaxNodes[0].IPrimary != null)
			
				//	Are we searching binders?
			else if(tmaxNodes[0].IBinder != null)
			{
				tmaxItems = GetCommandItems(tmaxNodes, false);
			}
			
			return tmaxItems;
		
		}// protected virtual CTmaxItems GetCmdFindItems(CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method is called by the application to get a list of items that represent the current selections for a Find command</summary>
		/// <returns>The items that represent the current selections</returns>
		public override CTmaxItems GetCmdFindItems()
		{
			return GetCmdFindItems(GetSelections(true));
		
		}// public override CTmaxItems GetCmdFindItems()
		
		/// <summary>This method is called to get the list of items used to fire the BulkUpdate command</summary>
		/// <param name="tmaxNodes">The collection of selected nodes</param>
		///	<param name="bBreakOnFirst">Break on first occurance of an item that satisfies the condition</param>
		/// <returns>The collection of items to be passed with the command event</returns>
		protected virtual CTmaxItems GetCmdBulkUpdateItems(CTmaxMediaTreeNodes tmaxNodes, bool bBreakOnFirst)
		{
			CTmaxItems tmaxItems = null;
			
			if(tmaxNodes == null) return null;
			if(tmaxNodes.Count == 0) return null;
			
			tmaxItems = new CTmaxItems();
			
			foreach(CTmaxMediaTreeNode O in tmaxNodes)
			{
				GetCmdBulkUpdateItems(O, tmaxItems);
				
				if((bBreakOnFirst == true) && (tmaxItems.Count > 0))
					break;
			
			}// foreach(CTmaxMediaTreeNode O in tmaxNodes)
			
			if((tmaxItems != null) && (tmaxItems.Count > 0))
				return tmaxItems;
			else
				return null;
		
		}// protected virtual CTmaxItems GetCmdBulkUpdateItems(CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method is called to get the list of items used to fire the BulkUpdate command</summary>
		/// <param name="tmaxNodes">The collection of selected nodes</param>
		/// <param name="tmaxItems">The collection in which to store the items</param>
		/// <returns>The number of items added to the collection</returns>
		protected virtual int GetCmdBulkUpdateItems(CTmaxMediaTreeNode tmaxNode, CTmaxItems tmaxItems)
		{
			int iExisting = 0;
			
			Debug.Assert(tmaxNode != null);
			Debug.Assert(tmaxItems != null);
			
			//	Is this a media node?
			if(tmaxNode.GetMediaRecord() != null)
			{
				//	Only primary records are used for bulk updates
				if(tmaxNode.GetMediaRecord().GetMediaLevel() == TmaxMediaLevels.Primary)
				{
					tmaxItems.Add(GetCommandItem(tmaxNode, false));
				}
				
			}
			
			//	Is this node a binder?
			else if(tmaxNode.IBinder != null)
			{
				GetCmdBulkUpdateItems((CDxBinderEntry)(tmaxNode.IBinder), tmaxItems);
			}
			
			return (tmaxItems.Count - iExisting);
		
		}// protected virtual int GetCmdBulkUpdateItems(CTmaxMediaTreeNode tmaxNode, CTmaxItems tmaxItems)
		
		/// <summary>This method is called to get the list of items used to fire the BulkUpdate command</summary>
		/// <param name="dxBinder">The source binder</param>
		/// <param name="tmaxItems">The collection in which to store the items</param>
		/// <returns>The number of items added to the collection</returns>
		protected virtual int GetCmdBulkUpdateItems(CDxBinderEntry dxBinder, CTmaxItems tmaxItems)
		{
			int		iExisting = 0;
			bool	bFilled = false;
			
			Debug.Assert(dxBinder != null);
			Debug.Assert(tmaxItems != null);
			
			//	How many items currently exist in the collection
			iExisting = tmaxItems.Count;
			
			//	Is this binder bound to a media record?
			if(dxBinder.IsMedia() == true)
			{
				if(dxBinder.Source != null)
				{
					//	Only allow primary media used in bulk updates
					if(dxBinder.Source.GetMediaLevel() == TmaxMediaLevels.Primary)
						tmaxItems.Add(new CTmaxItem(dxBinder.Source));
				}
				
			}
			else
			{
				//	Does this binder have any children?
				if(dxBinder.ChildCount > 0)
				{
					//	Make sure the child contents have been populated
					if(dxBinder.Contents.Count == 0)
					{
						dxBinder.Fill();
						bFilled = true;
					}
					
					foreach(CDxBinderEntry O in dxBinder.Contents)
						GetCmdBulkUpdateItems(O, tmaxItems);
						
					//	Should we clear the child collection?
					if(bFilled == true)
						dxBinder.Contents.Clear();
						
				}// if(dxBinder.ChildCount > 0)
			
			}// if(dxBinder.IsMedia == true)
			
			return (tmaxItems.Count - iExisting);
		
		}// protected virtual int GetCmdBulkUpdateItems(CDxBinder dxBinder, CTmaxItems tmaxItems)
		
		/// <summary>This method is called to get the list of items used to fire commands that act only on video designations</summary>
		/// <param name="tmaxNodes">The collection of selected nodes</param>
		///	<param name="bBreakOnFirst">Break on first occurance of an item that satisfies the condition</param>
		/// <returns>The collection of items to be passed with the command event</returns>
		protected virtual CTmaxItems GetCmdDesignationItems(CTmaxMediaTreeNodes tmaxNodes, bool bBreakOnFirst)
		{
			CTmaxItems tmaxItems = null;
			
			if(tmaxNodes == null) return null;
			if(tmaxNodes.Count == 0) return null;
			
			tmaxItems = new CTmaxItems();
			
			foreach(CTmaxMediaTreeNode O in tmaxNodes)
			{
				//	Is this a media node?
				if(O.GetMediaRecord() != null)
				{
					GetCmdDesignationItems((CDxMediaRecord)(O.GetMediaRecord()), tmaxItems, bBreakOnFirst);
				}
			
				//	Is this node a binder?
				else if(O.IBinder != null)
				{
					GetCmdDesignationItems((CDxBinderEntry)(O.IBinder), tmaxItems, bBreakOnFirst);
				}
				
				if((bBreakOnFirst == true) && (tmaxItems.Count > 0))
					break;
			
			}// foreach(CTmaxMediaTreeNode O in tmaxNodes)
			
			if((tmaxItems != null) && (tmaxItems.Count > 0))
				return tmaxItems;
			else
				return null;
		
		}// protected virtual CTmaxItems GetCmdDesignationItems(CTmaxMediaTreeNodes tmaxNodes, bool bBreakOnFirst)
		
		/// <summary>This method is called to get the list of items for commands that operate only on video designations</summary>
		/// <param name="dxRecord">The media record to be added if appropriate</param>
		/// <param name="tmaxItems">The collection in which to store the items</param>
		///	<param name="bBreakOnFirst">Break on first occurance of an item that satisfies the condition</param>
		/// <returns>The number of items added to the collection</returns>
		protected virtual int GetCmdDesignationItems(CDxMediaRecord dxRecord, CTmaxItems tmaxItems, bool bBreakOnFirst)
		{
			int				iExisting = 0;
			CDxMediaRecord	dxSource = null;
			CDxPrimary		dxScript = null;
			
			Debug.Assert(dxRecord != null);
			Debug.Assert(tmaxItems != null);
			
			//	Is this a script?
			if(dxRecord.MediaType == TmaxMediaTypes.Script)
			{
				//	Assume the script has one or more designations
				//	if being called to enable/disable the command
				if(bBreakOnFirst == true)
				{
					tmaxItems.Add(new CTmaxItem(dxRecord));
				}
				else
				{
					dxScript = (CDxPrimary)dxRecord;
					
					//	Make sure the child collection has been populated
					if(dxScript.Secondaries.Count == 0)
						dxScript.Fill();
						
					//	Iterate the scenes and add each one bound to a video designation
					foreach(CDxSecondary O in dxScript.Secondaries)
						GetCmdDesignationItems(O, tmaxItems, bBreakOnFirst);
				}
				
			}
			
			//	Is it a scene?
			else if(dxRecord.MediaType == TmaxMediaTypes.Scene)
			{
				//	Get the source media record
				if((dxSource = ((CDxSecondary)dxRecord).GetSource()) != null)
				{
					//	Is the source media a video designation?
					if(dxSource.MediaType == TmaxMediaTypes.Designation)
					{
						if(((CDxTertiary)dxSource).IsVideoDesignation == true)
						{
							//	Add this scene to the collection
							tmaxItems.Add(new CTmaxItem(dxRecord));
						}
						
					}// if(dxSource.MediaType == TmaxMediaTypes.Designation)
					
				}// if((dxSource = ((CDxSecondary)dxRecord).GetSource()) != null)
				
			}// else if(dxRecord.MediaType == TmaxMediaTypes.Scene)
			
			return (tmaxItems.Count - iExisting);
		
		}// protected virtual int GetCmdDesignationItems(CTmaxMediaTreeNode tmaxNode, CTmaxItems tmaxItems)
		
		/// <summary>This method is called to get the list of items for commands that act only on video designations</summary>
		/// <param name="dxBinder">The source binder</param>
		/// <param name="tmaxItems">The collection in which to store the items</param>
		///	<param name="bBreakOnFirst">Break on first occurance of an item that satisfies the condition</param>
		/// <returns>The number of items added to the collection</returns>
		protected virtual int GetCmdDesignationItems(CDxBinderEntry dxBinder, CTmaxItems tmaxItems, bool bBreakOnFirst)
		{
			int		iExisting = 0;
			bool	bFilled = false;
			
			Debug.Assert(dxBinder != null);
			Debug.Assert(tmaxItems != null);
			
			//	How many items currently exist in the collection
			iExisting = tmaxItems.Count;
			
			//	Is this binder bound to a media record?
			if(dxBinder.IsMedia() == true)
			{
				if(dxBinder.Source != null)
					GetCmdDesignationItems(dxBinder.Source, tmaxItems, bBreakOnFirst);
			}
			else
			{
				//	Does this binder have any children?
				if(dxBinder.ChildCount > 0)
				{
					//	Make sure the child contents have been populated
					if(dxBinder.Contents.Count == 0)
					{
						dxBinder.Fill();
						bFilled = true;
					}
					
					foreach(CDxBinderEntry O in dxBinder.Contents)
						GetCmdDesignationItems(O, tmaxItems, bBreakOnFirst);
						
					//	Should we clear the child collection?
					if(bFilled == true)
						dxBinder.Contents.Clear();
						
				}// if(dxBinder.ChildCount > 0)
			
			}// if(dxBinder.IsMedia == true)
			
			return (tmaxItems.Count - iExisting);
		
		}// protected virtual int GetCmdDesignationItems(CDxBinder dxBinder, CTmaxItems tmaxItems)
		
		/// <summary>This method is called to get the list of items used to fire the Export command</summary>
		/// <param name="tmaxNodes">The collection of selected nodes</param>
		/// <param name="eFormat">The TrialMax export format applied to the operation</param>
		/// <param name="bObjections">true if objections are being exported</param>
		///	<param name="bBreakOnFirst">Break on first occurance of an item that satisfies the condition</param>
		/// <returns>The collection of items to be passed with the command event</returns>
		protected virtual CTmaxItems GetCmdExportItems(CTmaxMediaTreeNodes tmaxNodes, TmaxExportFormats eFormat, bool bObjections, bool bBreakOnFirst)
		{
			CTmaxItems	tmaxItems = new CTmaxItems();
			CTmaxItem	tmaxItem = null;
			
			if(tmaxNodes == null) return null;
			if(tmaxNodes.Count == 0) return null;
			
			//	Allow the user to export scripts and binders
			foreach(CTmaxMediaTreeNode O in tmaxNodes)
			{
				//	Is this a media node?
				if(O.GetMediaRecord() != null)
				{
					switch(eFormat)
					{
						case TmaxExportFormats.Codes:
						case TmaxExportFormats.CodesDatabase:
						
							tmaxItem = GetCommandItem(O, false);
							tmaxItems.Add(tmaxItem);
							break;
							
						case TmaxExportFormats.AsciiMedia:
						case TmaxExportFormats.XmlScript:
						case TmaxExportFormats.Video:
						
							//	Is this a script?
							if(O.MediaType == TmaxMediaTypes.Script)
							{
								tmaxItem = GetCommandItem(O, false);
								tmaxItems.Add(tmaxItem);
							}
							//	Are we exporting objections?
							else if(bObjections == true)
							{
								if(O.IPrimary.GetMediaType() == TmaxMediaTypes.Deposition)
								{
									tmaxItem = GetCommandItem(O, false);
									tmaxItems.Add(tmaxItem);
								}
								
							}
							break;
							
						case TmaxExportFormats.LoadFile:
						
							//	Must be a document or page or script
							switch(O.MediaType)
							{
								//	Must be able to export as a page
								case TmaxMediaTypes.Document:
								case TmaxMediaTypes.Page:
								case TmaxMediaTypes.Script:

									tmaxItem = GetCommandItem(O, false);
									tmaxItems.Add(tmaxItem);
									break;
									
								case TmaxMediaTypes.Scene:
								
									if(((CDxSecondary)(O.GetMediaRecord())).SourceType == TmaxMediaTypes.Page)
									{
										tmaxItem = GetCommandItem(O, false);
										tmaxItems.Add(tmaxItem);
									}
									break;
							}
							break;
							
						default:
						
							break;
							
					}// switch(eFormat)
				
				}
				//	Is this a binder?
				else if(O.IBinder != null)
				{
					//	Is this format valid for a binder?
					switch(eFormat)
					{
						case TmaxExportFormats.AsciiMedia:
						case TmaxExportFormats.Codes:
						case TmaxExportFormats.CodesDatabase:
						case TmaxExportFormats.LoadFile:
						case TmaxExportFormats.XmlBinder:
						
							tmaxItem = GetCommandItem(O, false);
							tmaxItems.Add(tmaxItem);
							break;
					}
					
				}
				
				//	Are we supposed to break on the first item put in the collection?
				if((bBreakOnFirst == true) && (tmaxItems.Count > 0))
					break;
				
			}// foreach(CTmaxMediaTreeNode O in tmaxNodes)
			
			if(tmaxItems.Count > 0)
				return tmaxItems;
			else
				return null;
		
		}// protected virtual CTmaxItems GetCmdExportItems(CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method is called to get the list of items that identify the records to be included in the report</summary>
		///	<param name="eReport">The enumerated report identifier</param>
		/// <param name="tmaxNodes">The collection of selected nodes</param>
		/// <returns>The event items that represent the records to be included in the report</returns>
		protected virtual CTmaxItems GetCmdReportItems(TmaxReports eReport, CTmaxMediaTreeNodes tmaxNodes)
		{
			CTmaxItems tmaxItems = null;
			
			if(tmaxNodes == null) return null;
			if(tmaxNodes.Count == 0) return null;
			
			switch(eReport)
			{
				case TmaxReports.Scripts:
				case TmaxReports.Transcript:
				case TmaxReports.Objections:
					
					tmaxItems = new CTmaxItems();
					
					//	Add all scripts and binders to the item collection
					foreach(CTmaxMediaTreeNode O in tmaxNodes)
					{
						//	Is this a media node?
						if(O.IPrimary != null)
						{
							//	Is this a script
							if(O.IPrimary.GetMediaType() == TmaxMediaTypes.Script)
							{
								tmaxItems.Add(new CTmaxItem(O.IPrimary));
							}
							else if(tmaxNodes[0].IPrimary.GetMediaType() == TmaxMediaTypes.Deposition)
							{
								//	Deposition nodes are OK for transcript or objections reports
								if(eReport != TmaxReports.Scripts)
									tmaxItems = GetCommandItems(tmaxNodes, false);
							}
							
						}
						else if(O.IBinder != null)
						{
							tmaxItems.Add(GetCommandItem(O, false));
						}
						
					}// foreach(CTmaxMediaTreeNode O in tmaxNodes)
						
					break;
					
				case TmaxReports.Exhibits:
				
					//	Has the user selected media records or binders?
					if((tmaxNodes[0].IPrimary != null) || (tmaxNodes[0].IBinder != null))
					{
						tmaxItems = GetCommandItems(tmaxNodes, false);
					}
					break;
					
			}// switch(eReport)
			
			if((tmaxItems != null) && (tmaxItems.Count > 0))
				return tmaxItems;
			else
				return null;
		
		}// protected virtual CTmaxItems GetCmdReportItems(TmaxReports eReport, CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method is called to get the list of items used to fire the Clean command event</summary>
		/// <param name="tmaxNodes">The collection of selected nodes</param>
		/// <returns>The collection of items to be fired with the event</returns>
		protected virtual CTmaxItems GetCmdCleanItems(CTmaxMediaTreeNodes tmaxNodes)
		{
			CTmaxItems tmaxItems = null;
			
			if(tmaxNodes == null) return null;
			if(tmaxNodes.Count == 0) return null;
			
			//	Allocate the collection
			tmaxItems = new CTmaxItems();
			
			foreach(CTmaxMediaTreeNode O in tmaxNodes)
			{
				//	Is this a media node?
				if(O.GetMediaRecord() != null)
				{
					GetCmdCleanItems(((CDxMediaRecord)O.GetMediaRecord()), tmaxItems);
				}
				
				//	Is it a binder?
				else if(O.IBinder != null)
				{
					GetCmdCleanItems(((CDxMediaRecord)O.IBinder), tmaxItems);
				}
						
			}// foreach(CTmaxMediaTreeNode O in tmaxNodes)
			
			//	Don't return an empty collection
			if((tmaxItems != null) && (tmaxItems.Count == 0))
				tmaxItems = null;
				
			return tmaxItems;
		
		}// protected virtual CTmaxItems GetCmdCleanItems(CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method determines if the specified record should be added to the collection used to fire a Clean command</summary>
		/// <param name="dxRecord">The record to be added if applicable</param>
		/// <param name="tmaxItems">The collection of items to be fired with the event</param>
		///	<returns>The number of items added to the collection</returns>
		protected virtual long GetCmdCleanItems(CDxMediaRecord dxRecord, CTmaxItems tmaxItems)
		{
			long			lBefore = 0;
			CDxPrimary		dxScript = null;
			CDxSecondary	dxScene = null;
			
			if(dxRecord == null) return 0;
			if(tmaxItems == null) return 0;
			
			lBefore = tmaxItems.Count;
			
			//	Is this a media record?
			if(dxRecord.GetDataType() == TmaxDataTypes.Media)
			{
				//	What type of media?
				switch(dxRecord.MediaType)
				{
					case TmaxMediaTypes.Document:
					case TmaxMediaTypes.Page:
					case TmaxMediaTypes.Treatment:
							
						tmaxItems.Add(new CTmaxItem(dxRecord));
						break;
						
					case TmaxMediaTypes.Scene:
					
						dxScene = ((CDxSecondary)dxRecord);
						
						if(dxScene.GetCanClean(false) == true)
							GetCmdCleanItems(dxScene.GetSource(), tmaxItems);
						break;	
				
					case TmaxMediaTypes.Script:
					
						dxScript = ((CDxPrimary)dxRecord);
						
						//	Does this script have anything that can be cleaned?
						//
						//	NOTE:	This will also fill the scenes collection if necessary
						if(dxScript.GetCanClean(true) == true)
						{
							foreach(CDxSecondary O in dxScript.Secondaries)
							{
								if(O.GetCanClean(false) == true)
									GetCmdCleanItems(O.GetSource(), tmaxItems);
							}
						
						}
						break;	
				
				}// switch(dxRecord.MediaType)
						
			}// if(dxRecord.GetDataType() == TmaxDataTypes.Media)
			
			//	Is it a binder?
			else if(dxRecord.GetDataType() == TmaxDataTypes.Binder)
			{
				//	Does this binder have anything that can be cleaned?
				//
				//	NOTE:	This will also fill the contents collection if necessary
				if(dxRecord.GetCanClean(true) == true)
				{
					foreach(CDxBinderEntry O in ((CDxBinderEntry)dxRecord).Contents)
					{
						//	Don't drill down into sub-binders
						if((O.IsMedia() == true) && (O.GetSource(true) != null))
						{
							if(O.GetSource(true).GetCanClean(true) == true)
								GetCmdCleanItems(O.GetSource(true), tmaxItems);
						}
						
					}// foreach(CDxBinderEntry O in ((CDxBinderEntry)dxRecord).Contents)
						
				}// if(dxRecord.GetCanClean(true) == true)
			
			}// else if(dxRecord.GetDataType() == TmaxDataTypes.Binder)
			
			return (tmaxItems.Count - lBefore);
		
		}// protected virtual long GetCmdCleanItems(CDxMediaRecord dxRecord, CTmaxItems tmaxItems)
		
		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxNode">The node being passed with the command arguments</param>
		/// <param name="bChildren">true to include the node's children</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		/// <returns>true if successful</returns>
		protected bool FireCommand(TmaxCommands eCommand, CTmaxMediaTreeNode tmaxNode, bool bChildren, CTmaxParameters tmaxParameters)
		{
			CTmaxItem	tmaxItem = null;
			CTmaxItems	tmaxItems = null;
			
			//	Create an event item
			if((tmaxItem = GetCommandItem(tmaxNode, bChildren)) == null)
			{
				return false;
			}
			else
			{
				//	Create a collection to fire with the event
				tmaxItems = new CTmaxItems();
				tmaxItems.Add(tmaxItem);
			}
			
			//	Fire the command
			return FireCommand(eCommand, tmaxItems, tmaxParameters);
					
		}//	FireCommand(TmaxCommands eCommand, CTmaxMediaTreeNode tmaxNode, CTmaxParameters tmaxParameters)

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxNode">The node being passed with the command arguments</param>
		/// <param name="bChildren">True to include the node's children</param>
		/// <returns>true if successful</returns>
		protected bool FireCommand(TmaxCommands eCommand, CTmaxMediaTreeNode tmaxNode, bool bChildren)
		{
			return FireCommand(eCommand, tmaxNode, bChildren, null);
					
		}//	FireCommand(TmaxCommands eCommand, CTmaxMediaTreeNode tmaxNode, CTmaxParameters tmaxParameters)

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxNodes">The nodes being passed with the command arguments</param>
		/// <param name="bChildren">True to include the node's children</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		/// <returns>true if successful</returns>
		protected bool FireCommand(TmaxCommands eCommand, CTmaxMediaTreeNodes tmaxNodes, bool bChildren, CTmaxParameters tmaxParameters)
		{
			CTmaxItems	tmaxItems = null;
			
			//	Did the caller provide a node collection?
			if((tmaxNodes != null) && (tmaxNodes.Count > 0))
			{
				//	Create the event items
				if((tmaxItems = GetCommandItems(tmaxNodes, bChildren)) == null)
				{
					return false;
				}

			}// if((tmaxNodes != null) && (tmaxNodes.Count > 0))
			
			//	Fire the command
			return FireCommand(eCommand, tmaxItems, tmaxParameters);
					
		}//	FireCommand(TmaxCommands eCommand, CTmaxMediaTreeNodes tmaxNodes, CTmaxParameters tmaxParameters)

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxNodes">The nodes being passed with the command arguments</param>
		/// <param name="bChildren">True to include the node's children</param>
		/// <returns>true if successful</returns>
		protected bool FireCommand(TmaxCommands eCommand, CTmaxMediaTreeNodes tmaxNodes, bool bChildren)
		{
			//	Fire the command
			return FireCommand(eCommand, tmaxNodes, bChildren, null);
					
		}//	FireCommand(TmaxCommands eCommand, CTmaxMediaTreeNodes tmaxNodes)

		/// <summary>This method is called to fire an asynchronouse command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxNode">The node being passed with the command arguments</param>
		/// <returns>true if successful</returns>
		protected bool FireAsyncCommand(TmaxCommands eCommand, CTmaxMediaTreeNode tmaxNode)
		{
			CTmaxItem	tmaxItem = null;
			bool		bSuccessful = false;
			
			try
			{
				if((tmaxItem = GetCommandItem(tmaxNode, false)) != null)
				{
					bSuccessful = FireAsyncCommand(eCommand, tmaxItem);
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FireAsyncCommand", m_tmaxErrorBuilder.Message(ERROR_FIRE_ASYNC_COMMAND_EX, eCommand), Ex);
			}

			return bSuccessful;
							
		}//	protected bool FireAsyncCommand(TmaxCommands eCommand, CTmaxMediaTreeNode tmaxNode)

		/// <summary>This method is called to create and populate a TrialMax command items collection</summary>
		/// <param name="tmaxNodes">The nodes being passed with the command arguments</param>
		/// <param name="bChildren">true to include child nodes</param>
		/// <returns>The new command items collection</returns>
		protected CTmaxItems GetCommandItems(CTmaxMediaTreeNodes tmaxNodes, bool bChildren)
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
			
		}// GetCommandItems(CTmaxMediaTreeNodes tmaxNodes)
			
		/// <summary>This method is called to get a TrialMax command item based on the specified tree node</summary>
		/// <param name="tmaxNode">The node being passed with the command arguments</param>
		/// <param name="bChildren">true to include child nodes</param>
		/// <returns>The equivalent command item</returns>
		protected CTmaxItem GetCommandItem(CTmaxMediaTreeNode tmaxNode, bool bChildren)
		{
			CTmaxItem			tmaxItem = null;
			CTmaxItem			tmaxSubItem = null;
			TreeNodesCollection	aChildren = null;
			
			try
			{
				//	Allocate a new item
				tmaxItem = new CTmaxItem();
			
				//	NOTE: NULL parents are valid for binders
				if(tmaxNode != null)
				{
					//	Set the item properties
					//
					//	NOTE:	The TrialMax item uses the interfaces to determine
					//			the media level and type
					tmaxItem.IBinderEntry = tmaxNode.IBinder;
					tmaxItem.IPrimary     = tmaxNode.IPrimary;
					tmaxItem.ISecondary   = tmaxNode.ISecondary;
					tmaxItem.ITertiary    = tmaxNode.ITertiary;
					tmaxItem.IQuaternary  = tmaxNode.IQuaternary;
					
					//	Set the media type just in case this node is not associated
					//	with a record
					tmaxItem.MediaType = tmaxNode.MediaType;
					
					//	Set the parent item
					//
					//	NOTE:	This allows us to provide the full tree path to the
					//			item being acted on
					if(tmaxNode.Parent != null)
					{
						try
						{
							tmaxItem.ParentItem = GetCommandItem((CTmaxMediaTreeNode)(tmaxNode.Parent), false);
						}
						catch
						{
						}
					
					}
				
				}// if(tmaxNode != null)

				//	Make sure the data type is set
				if(tmaxItem.MediaType != TmaxMediaTypes.Unknown)
					tmaxItem.DataType = TmaxDataTypes.Media;
				else
					tmaxItem.DataType = TmaxDataTypes.Binder;
					
				//	Are we supposed to add items for the children?
				if(bChildren == true)
				{
					//	Do we need to use the root collection?
					if(tmaxNode == null)
						aChildren = m_tmaxTreeCtrl.Nodes;
					else
						aChildren = tmaxNode.Nodes;
						
					if((aChildren != null) && (aChildren.Count > 0))
					{
						//	Add a sub item for each child node
						foreach(CTmaxMediaTreeNode tmaxChild in aChildren)
						{
							if((tmaxSubItem = GetCommandItem(tmaxChild, true)) != null)
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
				m_tmaxEventSource.FireError(this, "GetCommandItem", m_tmaxErrorBuilder.Message(ERROR_GET_COMMAND_ITEM_EX, tmaxNode.FullPath), Ex);
				return null;
			}
			
		}// GetCommandItem(CTmaxMediaTreeNode tmaxNode)
					
		/// <summary>This method handles is called to add new items to the specified media record</summary>
		/// <param name="tmaxTarget">The node under the current mouse position</param>
		/// <param name="tmaxSource">The collection of items that represent the records to add</param>
		/// <param name="bInsert">true if inserting records relative to the target, false if adding to the target</param>
		/// <param name="bBefore">true to insert before the specified target node</param>
		/// <returns>True if the command event gets fired</returns>
		protected virtual bool Add(CTmaxMediaTreeNode tmaxTarget, CTmaxItems tmaxSource, bool bInsert, bool bBefore)
		{
			CTmaxParameters	tmaxParameters = null;
			CTmaxItem		tmaxItem = null;
			
			//	Make sure we have the required objects
			if(tmaxTarget == null) return false;
			if(tmaxSource == null) return false;
			if(tmaxSource.ContainsMedia(true) == false) return false;
			
			//	Create the required parameters for the event
			tmaxParameters = new CTmaxParameters();
			tmaxParameters.Add(TmaxCommandParameters.Activate, true);
				
			//	Are we inserting a new object?
			if(bInsert == true)
			{
				if(tmaxTarget.Parent == null) return false;
				if(tmaxTarget.MediaType != TmaxMediaTypes.Scene) return false;
				
				//	Create the required parameters for the event
				tmaxParameters.Add(TmaxCommandParameters.Before, bBefore);
				
				//	Create the command item used to define the parent node
				tmaxItem = GetCommandItem((CTmaxMediaTreeNode)tmaxTarget.Parent, false);
				
				//	Put the insertion point in the subitem collection
				tmaxItem.SubItems.Add(GetCommandItem(tmaxTarget, false));

				//	Assign the source items
				tmaxItem.SourceItems = new CTmaxItems();
				Pack(tmaxSource, true, tmaxItem.SourceItems);
				
				//	Fire the event
				FireCommand(TmaxCommands.Add, tmaxItem, tmaxParameters);
			}
			else
			{
				if(tmaxTarget.MediaType != TmaxMediaTypes.Script) return false;
				
				//	The selected node is the parent when adding instead of inserting
				tmaxItem = GetCommandItem(tmaxTarget, false);
				
				//	Assign the source items
				tmaxItem.SourceItems = new CTmaxItems();
				Pack(tmaxSource, true, tmaxItem.SourceItems);
				
				//	Fire the event
				FireCommand(TmaxCommands.Add, tmaxItem, tmaxParameters);
			}
			
			return true;
			
		}// protected virtual bool Add(CTmaxMediaTreeNode tmaxTarget, CTmaxItems tmaxSource, bool bInsert, bool bBefore)
		
		/// <param name="tmaxTarget">The node under the current mouse position</param>
		/// <param name="tmaxSource">The collection of items that represent the nodes to be moved</param>
		/// <param name="bInto">true if moving records into the target, false if moving relative to the target</param>
		/// <param name="bBefore">true to move the nodes into position before the specified target node</param>
		/// <returns>True if the command event gets fired</returns>
		protected virtual bool MoveTo(CTmaxMediaTreeNode tmaxTarget, CTmaxItems tmaxSource, bool bInto, bool bBefore)
		{
			return false;
		}

		/// <summary>This method traps the MouseDown event</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">System mouse event arguments</param>
		protected virtual void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			CTmaxMediaTreeNode tmaxNode = null;
			
			//	Don't do anything if we are not over a node
			if((tmaxNode = m_tmaxTreeCtrl.GetNode(e.X, e.Y)) == null) return;
				
			//	Keep track of the node being clicked on if it is already selected
			//
			//	NOTE:	We do this to be able to activate this node if the user is
			//			bouncing back and forth between panes without actually
			//			changing the selection in the pane
			if((e.Clicks == 1) && (tmaxNode.Selected == true))
				m_tmaxClickNode = tmaxNode;
			else
				m_tmaxClickNode = null;
				
			//	Is this the right button?
			if(e.Button == MouseButtons.Right)
			{
				//	Don't do anything if the user is pressing the Control key
				if((User.GetKeyState(User.VK_CONTROL) & 0x8000) != 0) return;
				
				//	Don't do anything if pressing the Shift key
				if((User.GetKeyState(User.VK_SHIFT) & 0x8000) != 0) return;
				
				//	Make sure the node under the mouse is selected
				if(tmaxNode.Selected == false)
				{
					m_tmaxTreeCtrl.SetSelection(tmaxNode);
				}

			}
		
		}// OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)

		/// <summary>This method traps the MouseUp event</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">System mouse event arguments</param>
		protected virtual void OnMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//	Make sure the local click node is cleared
			m_tmaxClickNode = null;
					
		}// OnMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)

		/// <summary>Overridden by derived classes to provide access to the pane's toolbar manager</summary>
		/// <returns>The pane's toolbar manager if available</returns>
		protected override Infragistics.Win.UltraWinToolbars.UltraToolbarsManager GetUltraToolbarManager()
		{
			return m_ultraToolbarManager;
		}

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>This method will process the specified command</summary>
		/// <param name="eCommand">The command to be processed</param>
		private void OnCommand(TreePaneCommands eCommand)
		{
			try
			{	
				//	Direct to the appropriate handler
				switch(eCommand)
				{
					case TreePaneCommands.SetTargetBinder:
					
						OnCmdSetTargetBinder(GetSelection());
						break;
						
					case TreePaneCommands.Viewer:
					
						OnCmdViewer(GetSelection());
						break;
					
                    case TreePaneCommands.Explorer:

                        OnCmdExplorer(GetSelection());
                        break;
	
					case TreePaneCommands.Presentation:
					
						OnCmdPresentation(GetSelection());
						break;
					
                    case TreePaneCommands.PresentationRecording:

                        folderaccess = CheckFolderAccess();
                        if (folderaccess)
                        {
                            OnCmdPresentation(GetSelection(), true);
                        }
                        break;

					case TreePaneCommands.Builder:
					
						OnCmdBuilder(GetSelection());
						break;
						
					case TreePaneCommands.Tuner:
					
						OnCmdTuner(GetSelection());
						break;
						
					case TreePaneCommands.Properties:
					
						OnCmdProperties(GetSelection());
						break;
						
					case TreePaneCommands.Codes:
					
						OnCmdCodes(GetSelection());
						break;
						
					case TreePaneCommands.Delete:
					
						//	Delete supports multiple selections
						OnCmdDelete(GetSelections(true));
						break;
						
					case TreePaneCommands.Print:
					
						OnCmdPrint(GetSelections(true));
						break;
						
					case TreePaneCommands.Find:
					
						OnCmdFind(GetSelections(true));
						break;
						
					case TreePaneCommands.Synchronize:
					
						//	Synchronize supports multiple selections
						OnCmdSynchronize(GetSelections(true));
						break;
						
					case TreePaneCommands.SortDesignations:
					
						OnCmdSortDesignations(GetSelections(true));
						break;
						
					case TreePaneCommands.MergeScripts:
					
						OnCmdMergeScripts(GetSelections(true));
						break;
						
					case TreePaneCommands.Clean:
					
						OnCmdClean(GetSelections(true));
						break;
						
					case TreePaneCommands.CollapseAll:
					
						OnCmdCollapseAll(GetSelection());
						break;
						
					case TreePaneCommands.ExpandAll:
						
						OnCmdExpandAll(GetSelection());
						break;
						
					case TreePaneCommands.Copy:
					
						OnCmdCopy(GetSelections(true));
						break;
						
					case TreePaneCommands.Paste:
					
						OnCmdPaste(GetSelection(), false, false);
						break;
						
					case TreePaneCommands.PasteBefore:
					
						OnCmdPaste(GetSelection(), true, true);
						break;
						
					case TreePaneCommands.PasteAfter:
					
						OnCmdPaste(GetSelection(), true, false);
						break;
						
					case TreePaneCommands.New:
					
						OnCmdNew(GetSelection(), false, false);
						break;
						
					case TreePaneCommands.NewBinder:
					
						OnCmdNew(TmaxMediaTypes.Unknown, GetSelection(), false, false);
						break;
						
					case TreePaneCommands.NewDocument:
					
						OnCmdNew(TmaxMediaTypes.Document, GetSelection(), false, false);
						break;
						
					case TreePaneCommands.NewRecording:
					
						OnCmdNew(TmaxMediaTypes.Recording, GetSelection(), false, false);
						break;
						
					case TreePaneCommands.NewScript:
					
						OnCmdNew(TmaxMediaTypes.Script, GetSelection(), false, false);
						break;
						
					case TreePaneCommands.NewBarcodes:
					
						OnCmdNewBarcodes(GetSelection(), false, false);
						break;
						
					case TreePaneCommands.ScriptNewDesignations:
					
						OnCmdNewDesignations(GetSelection(), false, false);
						break;
						
					case TreePaneCommands.ScriptBeforeDesignations:
					
						OnCmdNewDesignations(GetSelection(), true, true);
						break;
						
					case TreePaneCommands.ScriptAfterDesignations:
					
						OnCmdNewDesignations(GetSelection(), true, false);
						break;
						
					case TreePaneCommands.ScriptNewClips:
					
						OnCmdNewClips(GetSelection(), false, false);
						break;
						
					case TreePaneCommands.ScriptNewBarcodes:
					
						OnCmdNewBarcodes(GetSelection(), false, false);
						break;
						
					case TreePaneCommands.ScriptBeforeClips:
					
						OnCmdNewClips(GetSelection(), true, true);
						break;
						
					case TreePaneCommands.ScriptBeforeBarcodes:
					
						OnCmdNewBarcodes(GetSelection(), true, true);
						break;
						
					case TreePaneCommands.ScriptAfterClips:
					
						OnCmdNewClips(GetSelection(), true, false);
						break;
						
					case TreePaneCommands.ScriptAfterBarcodes:
					
						OnCmdNewBarcodes(GetSelection(), true, false);
						break;
						
					case TreePaneCommands.Duplicate:
					
						OnCmdDuplicate(GetSelection());
						break;
						
					case TreePaneCommands.MoveUp:
					
						OnCmdMove(GetSelection(), true);
						break;
						
					case TreePaneCommands.MoveDown:
					
						OnCmdMove(GetSelection(), false);
						break;
						
					case TreePaneCommands.RotateCW:
					
						OnCmdRotate(GetSelection(), false);
						break;
						
					case TreePaneCommands.RotateCCW:
					
						OnCmdRotate(GetSelection(), true);
						break;
						
					case TreePaneCommands.InsertBefore:
					
						OnCmdNew(GetSelection(), true, true);
						break;
						
					case TreePaneCommands.InsertBinderBefore:
					
						OnCmdNew(TmaxMediaTypes.Unknown, GetSelection(), true, true);
						break;
						
					case TreePaneCommands.InsertDocumentBefore:
					
						OnCmdNew(TmaxMediaTypes.Document, GetSelection(), true, true);
						break;
						
					case TreePaneCommands.InsertRecordingBefore:
					
						OnCmdNew(TmaxMediaTypes.Recording, GetSelection(), true, true);
						break;
						
					case TreePaneCommands.InsertScriptBefore:
					
						OnCmdNew(TmaxMediaTypes.Script, GetSelection(), true, true);
						break;
						
					case TreePaneCommands.InsertBarcodesBefore:
						
						OnCmdNewBarcodes(GetSelection(), true, true);
						break;
						
					case TreePaneCommands.InsertAfter:
					
						OnCmdNew(GetSelection(), true, false);
						break;
						
					case TreePaneCommands.InsertBinderAfter:
					
						OnCmdNew(TmaxMediaTypes.Unknown, GetSelection(), true, false);
						break;
						
					case TreePaneCommands.InsertDocumentAfter:
					
						OnCmdNew(TmaxMediaTypes.Document, GetSelection(), true, false);
						break;
						
					case TreePaneCommands.InsertRecordingAfter:
					
						OnCmdNew(TmaxMediaTypes.Recording, GetSelection(), true, false);
						break;
						
					case TreePaneCommands.InsertScriptAfter:
					
						OnCmdNew(TmaxMediaTypes.Script, GetSelection(), true, false);
						break;
						
					case TreePaneCommands.InsertBarcodesAfter:
						
						OnCmdNewBarcodes(GetSelection(), true, false);
						break;
						
					case TreePaneCommands.ExportText:
					case TreePaneCommands.ExportXmlScript:
					case TreePaneCommands.ExportXmlBinder:
					case TreePaneCommands.ExportVideo:
					case TreePaneCommands.ExportCodes:
					case TreePaneCommands.ExportCodesDatabase:
					case TreePaneCommands.ExportLoadFile:
					
						OnCmdExport(GetSelections(true), GetExportFormat(eCommand), false);
						break;

					case TreePaneCommands.ExportAsciiObjections:

						OnCmdExport(GetSelections(true), GetExportFormat(eCommand), true);
						break;

					case TreePaneCommands.ImportAsciiBinders:
					case TreePaneCommands.ImportXmlBinders:
					case TreePaneCommands.ImportAsciiScripts:
					case TreePaneCommands.ImportXmlScripts:
					
						OnCmdImport(GetSelections(true), eCommand);
						break;
						
					case TreePaneCommands.Preferences:
					
						OnCmdPreferences(GetSelection());
						break;
						
					case TreePaneCommands.RefreshSuperNodes:
					
						OnCmdRefreshSuperNodes(GetSelection());
						break;
						
					case TreePaneCommands.ReportScripts:
					
						OnCmdReport(TmaxReports.Scripts, GetSelections(true));
						break;
						
					case TreePaneCommands.ReportExhibits:
					
						OnCmdReport(TmaxReports.Exhibits, GetSelections(true));
						break;
						
					case TreePaneCommands.ReportTranscript:
					
						OnCmdReport(TmaxReports.Transcript, GetSelections(true));
						break;

					case TreePaneCommands.ReportObjections:

						OnCmdReport(TmaxReports.Objections, GetSelections(true));
						break;

					case TreePaneCommands.SetFilter:
					
						OnCmdSetFilter();
						break;
						
					case TreePaneCommands.RefreshFiltered:
					
						OnCmdRefreshFiltered();
						break;
						
					case TreePaneCommands.BulkUpdate:
					
						OnCmdBulkUpdate(GetSelections(true));
						break;
						
					case TreePaneCommands.ShowScrollText:
					case TreePaneCommands.HideScrollText:
					
						OnCmdScrollText(GetSelections(true), eCommand == TreePaneCommands.ShowScrollText);
						break;
						
					case TreePaneCommands.SetHighlighter1:
					case TreePaneCommands.SetHighlighter2:
					case TreePaneCommands.SetHighlighter3:
					case TreePaneCommands.SetHighlighter4:
					case TreePaneCommands.SetHighlighter5:
					case TreePaneCommands.SetHighlighter6:
					case TreePaneCommands.SetHighlighter7:
					
						OnCmdSetHighlighter(GetSelections(true), (int)eCommand);
						break;

					case TreePaneCommands.AddObjection:

						OnCmdAddObjection(GetSelections(true), false);
						break;

					case TreePaneCommands.RepeatObjection:

						OnCmdAddObjection(GetSelections(true), true);
						break;



                    case TreePaneCommands.AddAudioWaveform:
                        OnCmdAddAudioWaveform(GetSelections(true));
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
		
		} // private void OnCommand(TreePaneCommands eCommand)

        protected virtual void OnCmdAddAudioWaveform(CTmaxMediaTreeNodes tmaxNodes)
        {
            string missingFileInfo = "";
            string audioWaveFormFileName = "";
            DialogResult messageBoxResult = DialogResult.Yes;
            CDxPrimary depositionPrimaryRecord = null;
            string treatmentFilePath = "";

            foreach (CTmaxMediaTreeNode selectedDeposition in tmaxNodes)
            {
                var tmaxRecord = selectedDeposition.GetMediaRecord();
                if (tmaxRecord is CDxPrimary)
                {
                    depositionPrimaryRecord = (CDxPrimary)tmaxRecord;
                    audioWaveFormFileName = depositionPrimaryRecord.Name;
                                       
                    treatmentFilePath = Path.Combine(depositionPrimaryRecord.Database.Folder, "_tmax_transcripts", depositionPrimaryRecord.AutoId.ToString());

                    if (!Directory.Exists(Path.Combine(depositionPrimaryRecord.Database.Folder, "_tmax_transcripts", depositionPrimaryRecord.AutoId.ToString())))
                    {
                        missingFileInfo += string.Format("Waveform file {0} not found for {1} under _tmax_transcripts folder", audioWaveFormFileName, depositionPrimaryRecord.MediaId) + System.Environment.NewLine;
                    }
                    else
                    {
                        string[] xmltfiles = Directory.GetFiles(treatmentFilePath, "*.xmlt");
                        if (xmltfiles == null || xmltfiles.Length == 0)
                        {
                            missingFileInfo += string.Format("Waveform file {0} not found for {1} at {2}", audioWaveFormFileName, depositionPrimaryRecord.MediaId, treatmentFilePath) + System.Environment.NewLine;
                        }
                    }
                }               
            }

            if(!string.IsNullOrEmpty(missingFileInfo))
            {
                messageBoxResult = MessageBox.Show(missingFileInfo + "Do you still want to proceed?", "Missing Audio Waveform Files", MessageBoxButtons.YesNo);
            }

            if (messageBoxResult == DialogResult.Yes)
            {
                try
                {
                    missingFileInfo = "";
                    foreach (CTmaxMediaTreeNode selectedDeposition in tmaxNodes)
                    {
                        String MediaName = "";
                        string File = "";
                        if (selectedDeposition.GetMediaRecord() is CDxPrimary)
                        {
                            MediaName = ((CDxPrimary)selectedDeposition.GetMediaRecord()).Name;
                            treatmentFilePath = Path.Combine(((CDxPrimary)selectedDeposition.GetMediaRecord()).Database.Folder, "_tmax_transcripts", ((CDxPrimary)selectedDeposition.GetMediaRecord()).AutoId.ToString());
                            string[] xmltfiles = Directory.GetFiles(treatmentFilePath, "*.xmlt");
                          
                            if (xmltfiles != null && xmltfiles.Length > 0)
                            {
                                File = Path.Combine(treatmentFilePath, xmltfiles[0]); //assuming there would be only 1 xmlt file always
                                if (System.IO.File.Exists(File))
                                {
                                    XmlDocument xmlDoc = new XmlDocument();

                                    xmlDoc.Load(File);

                                    XmlNodeList DepositionInfo = xmlDoc.GetElementsByTagName("deposition");
                                    XmlNodeList SegmentInfo = xmlDoc.GetElementsByTagName("segment");

                                    if (MediaName != DepositionInfo[0].Attributes["name"].Value)
                                    {
                                        missingFileInfo += string.Format("The xmlt file {0} does not belong to this Deposition {1}", File, MediaName) + System.Environment.NewLine;
                                        // throw new Exception("The xmlt file does not belong to this Deposition");
                                    }
                                    else
                                    {
                                        string error = "";
                                        m_tmaxDatabase.AddAudioWaveform(SegmentInfo, out error);
                                        missingFileInfo += error;
                                    }
                                }
                            }
                        }                       
                    }

                    messageBoxResult = MessageBox.Show("Add Audio Waveform process finished." + System.Environment.NewLine + missingFileInfo, "Add Audio Waveform", MessageBoxButtons.OK);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Incorrect File");
                }

            }
        }// protected virtual void OnCmdAddAudioWaveform(CTmaxMediaTreeNodes tmaxNodes)

		/// <summary>Traps the ToolClick event fired by the toolbar manager</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event argument object</param>
		private void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		{
			TreePaneCommands eCommand = TreePaneCommands.Invalid;
			
			//	Don't bother if ignoring events
			if(m_bIgnoreUltraEvents == true) return;
			
			if((eCommand = GetCommand(e.Tool.Key)) != TreePaneCommands.Invalid)
				OnCommand(eCommand);
		
		}// private void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		
		/// <summary>This method handles events fired by the tree control when it is about to expand a node</summary>
		/// <param name="sender">The tree firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnUltraBeforeExpand(object sender, Infragistics.Win.UltraWinTree.CancelableNodeEventArgs e)
		{
			if(e.TreeNode != null)
			{
				//m_tmaxEventSource.FireDiagnostic(this, "OnUltraBeforeExpand", "Before Expand: " + e.TreeNode.Text);
				e.Cancel = OnBeforeExpand((CTmaxMediaTreeNode)e.TreeNode);
			}
		}

		/// <summary>This method handles events fired by the tree control when it is about to collapse a node</summary>
		/// <param name="sender">The tree firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnUltraBeforeCollapse(object sender, Infragistics.Win.UltraWinTree.CancelableNodeEventArgs e)
		{
			if(e.TreeNode != null)
			{
				//FireDiagnostic("OnUltraBeforeCollapse", "Before Collapse: " + e.TreeNode.Text);
				e.Cancel = OnBeforeCollapse((CTmaxMediaTreeNode)e.TreeNode);
			}
		}

		/// <summary>This method handles events fired by the tree control when it is about to select a node</summary>
		/// <param name="sender">The tree firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnUltraBeforeSelect(object sender, Infragistics.Win.UltraWinTree.BeforeSelectEventArgs e)
		{
			/*
			if(e.NewSelections != null && e.NewSelections.Count > 0)
				FireDiagnostic("OnUltraBeforeSelect", "Before Select: Cancel = " + e.Cancel.ToString() + " " + e.NewSelections[0].Text + " of " + e.NewSelections.Count.ToString());
			else
				FireDiagnostic("OnUltraBeforeSelect", "Before Select: null");
			*/
			
			if((e.NewSelections != null) && (e.NewSelections.Count > 0))
			{
				if(e.NewSelections.Count == 1)
				{
					e.Cancel = OnBeforeSelect((CTmaxMediaTreeNode)e.NewSelections[0]);
				}
				else
				{
					e.Cancel = OnBeforeSelect(m_tmaxTreeCtrl.GetTmaxNodes(e.NewSelections, true));
				}
				
			}
			else
			{
				e.Cancel = OnBeforeSelect((CTmaxMediaTreeNode)null);
			}

		}// private void OnUltraBeforeSelect(object sender, Infragistics.Win.UltraWinTree.BeforeSelectEventArgs e)

		/// <summary>This method handles events fired by the toolbar manager before displaying the popup menu</summary>
		/// <param name="sender">The toolbar manager firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnUltraBeforeDropDown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventArgs e)
		{
			Debug.Assert(m_ultraToolbarManager != null);
			Debug.Assert(m_ultraToolbarManager.Tools != null);
			if((m_ultraToolbarManager == null) ||( m_ultraToolbarManager.Tools == null)) return;
			
			try
			{
				if(m_bEnableContextMenu == true)
				{
					switch(e.Tool.Key)
					{
						case "SetHighlighter":
						
							try { OnBeforeSetHighlighterMenu((PopupMenuTool)(e.Tool), null); }
							catch {}
							break;
							
						default:
						
							OnBeforePopupMenu((PopupMenuTool)(e.Tool), GetSelections(true));
							break;
							
					}// switch(e.Tool.Key)
					
				}
				else
				{
					e.Cancel = true;
				}
			
			}
            catch (Exception ee)
            {
                MessageBox.Show("{0} Exception caught.", ee.ToString());
            }
							
		}// OnUltraBeforeDropDown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventArgs e)

		/// <summary>This method handles events fired by the toolbar manager after displaying the popup menu</summary>
		/// <param name="sender">The toolbar manager firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnUltraAfterCloseup(object sender, Infragistics.Win.UltraWinToolbars.ToolDropdownEventArgs e)
		{
			TreePaneCommands	eCommand = TreePaneCommands.Invalid;
			PopupMenuTool		popupMenu = null;
			
			//	Process all popup menus
			try { popupMenu = (PopupMenuTool)(e.Tool); }
			catch { return; }
			
			//	Check each tool in the menu's collection
			foreach(ToolBase O in popupMenu.Tools)
			{
				//	Get the command for this tool
				if((eCommand = GetCommand(O.Key)) != TreePaneCommands.Invalid)
				{
					//	Uninstall the shortcut if required
					if(GetCommandShortcut(eCommand) != Shortcut.None)
						O.SharedProps.Shortcut = Shortcut.None;
				}

			}// foreach(ToolBase O in m_ultraToolbarManager.Tools)
				
		}// private void OnUltraAfterCloseup(object sender, Infragistics.Win.UltraWinToolbars.ToolDropdownEventArgs e)

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

			if((e.NewSelections != null) && (e.NewSelections.Count > 0))
			{
				if(e.NewSelections.Count == 1)
				{
					OnAfterSelect((CTmaxMediaTreeNode)e.NewSelections[0]);
				}
				else
				{
					OnAfterSelect(m_tmaxTreeCtrl.GetTmaxNodes(e.NewSelections));
				}
				
			}
			else
			{
				OnAfterSelect((CTmaxMediaTreeNode)null);
			}

		}// private void OnUltraAfterSelect(object sender, Infragistics.Win.UltraWinTree.SelectEventArgs e)

		/// <summary>This method handles events fired by the tree control when it has collapsed a node</summary>
		/// <param name="sender">The tree firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnUltraAfterCollapse(object sender, Infragistics.Win.UltraWinTree.NodeEventArgs e)
		{
			if(e.TreeNode != null)
			{
				//FireDiagnostic("OnUltraAfterCollapse", "After Collapse: " + e.TreeNode.Text);
				OnAfterCollapse((CTmaxMediaTreeNode)e.TreeNode);
			}
		
		}// private void OnUltraAfterCollapse(object sender, Infragistics.Win.UltraWinTree.NodeEventArgs e)

		/// <summary>This method handles events fired by the tree control when it has expanded a node</summary>
		/// <param name="sender">The tree firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnUltraAfterExpand(object sender, Infragistics.Win.UltraWinTree.NodeEventArgs e)
		{
			if(e.TreeNode != null)
			{
				//FireDiagnostic("OnUltraAfterExpand", "After Expand: " + e.TreeNode.Text);
				OnAfterExpand((CTmaxMediaTreeNode)e.TreeNode);
			}
		
		}// private void OnUltraAfterExpand(object sender, Infragistics.Win.UltraWinTree.NodeEventArgs e)

		/// <summary>This method is called to set the state of tools in the specified submenu</summary>
		/// <param name="ultraMenu">The pane's context menu tool</param>
		private void OnBeforePopupMenu(PopupMenuTool ultraMenu, CTmaxMediaTreeNodes	tmaxNodes)
		{
			TreePaneCommands	eCommand;
			Shortcut			eShortcut = Shortcut.None;
			
			Debug.Assert(m_ultraToolbarManager != null);
			Debug.Assert(m_ultraToolbarManager.Tools != null);
			if((m_ultraToolbarManager == null) ||( m_ultraToolbarManager.Tools == null)) return;
			
			//	Check each tool in the popup collection
			foreach(ToolBase ultraTool in ultraMenu.Tools)
			{
				if((eCommand = GetCommand(ultraTool.Key)) != TreePaneCommands.Invalid)
				{
					//	Should the command be visible?
					if(GetCommandVisible(eCommand, tmaxNodes) == true)
					{
						ultraTool.SharedProps.Visible = true;
						ultraTool.SharedProps.Enabled = GetCommandEnabled(eCommand, tmaxNodes);

						//	Set shortcuts for commands on this menu
						if((eShortcut = GetCommandShortcut(eCommand)) != Shortcut.None)
							ultraTool.SharedProps.Shortcut = eShortcut;
					}
					else
					{
						ultraTool.SharedProps.Visible = false;
					}
							
				}

			}// foreach(ToolBase ultraTool in ultraMenu.Tools)
				
		}// private void OnBeforePopupMenu(PopupMenuTool ultraMenu, CTmaxMediaTreeNodes	tmaxNodes)

		/// <summary>This method handles events fired by the tree control when the check state of a node changes</summary>
		/// <param name="sender">The tree firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnUltraAfterCheck(object sender, Infragistics.Win.UltraWinTree.NodeEventArgs e)
		{
			UltraTreeNode parent = null;
			
			try
			{
				if((m_bCheckBoxes == true) && (m_bIgnoreChecked == false))
				{
					m_bIgnoreChecked = true;
					
					//	Make sure all parent nodes are selected if this
					//	node is selected
					if(e.TreeNode.CheckedState == CheckState.Checked)
					{
						parent = e.TreeNode.Parent;
						while(parent != null)
						{
							if(parent.CheckedState != CheckState.Checked)
								parent.CheckedState = CheckState.Checked;
							parent = parent.Parent;
						}
						
					}// if(e.TreeNode.CheckedState == CheckState.Checked)
					
					//	Make sure the check state of all children match
					if(e.TreeNode.Nodes != null)
						SetCheckState(e.TreeNode.Nodes, e.TreeNode.CheckedState);
					
					m_bIgnoreChecked = false;
					
				}// if((m_bCheckBoxes == true) && (m_bIgnoreChecked == false))
		
			}
			catch
			{
			}
			
		}// private void OnUltraAfterCheck(object sender, Infragistics.Win.UltraWinTree.NodeEventArgs e)

		/// <summary>Called to set the check state of the specified nodes and their children</summary>
		/// <param name="treeNodes">The nodes to be set</param>
		/// <param name="checkState">The desired check state</param>
		private void SetCheckState(TreeNodesCollection treeNodes, CheckState checkState)
		{
			try
			{
				//	Make sure the check state of all children match
				if((treeNodes != null) && (treeNodes.Count > 0))
				{
					foreach(UltraTreeNode O in treeNodes)
					{
						O.CheckedState = checkState;
						
						if((O.Nodes != null) && (O.Nodes.Count > 0))
							SetCheckState(O.Nodes, checkState);
					}
							
				}// if((treeNodes != null) && (treeNodes.Count > 0))
		
			}
			catch
			{
			}
			
		}// private void OnUltraAfterCheck(object sender, Infragistics.Win.UltraWinTree.NodeEventArgs e)

		/// <summary>
		/// This method traps the DoubleClick event
		/// </summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">System mouse event arguments</param>
		private void OnDoubleClick(object sender, System.EventArgs e)
		{
			CTmaxMediaTreeNode tmaxNode = null;
			
			//	Are we over a node?
			if((tmaxNode = m_tmaxTreeCtrl.GetNode()) != null)
			{
				//	Notify the derived class
				OnDoubleClick(tmaxNode);
			}

		}

		/// <summary>This method is called to get the node at the current cursor position</summary>
		/// <returns>The node under the current cursor position</returns>
		private CTmaxMediaTreeNode GetNodeAtCursor()
		{
			try
			{
				if(m_tmaxTreeCtrl != null)
					return m_tmaxTreeCtrl.GetNode();
			}
			catch
			{
			}
			
			//	Can't get the node
			return null;
			
		}// GetNodeAtCursor()
		
		/// <summary>
		/// This method is called internally to convert the key passed in
		///	an Infragistics event to its associated command enumeration
		/// </summary>
		/// <param name="strKey">The Infragistic key identifier</param>
		/// <returns>The associated tree command</returns>
		private TreePaneCommands GetCommand(string strKey)
		{
			try
			{
				Array aCommands = Enum.GetValues(typeof(TreePaneCommands));
				
				foreach(TreePaneCommands eCommand in aCommands)
				{
					if(eCommand.ToString() == strKey)
						return eCommand;
				}
				
			}
			catch
			{
			}
			
			return TreePaneCommands.Invalid;
		}
		
		/// <summary>
		/// This method is called internally to get the text mode associated with the specified interface
		/// </summary>
		/// <param name="tmaxRecord">The record interface object</param>
		/// <returns>The associated text mode</returns>
		private FTI.Shared.Trialmax.TmaxTextModes GetTextMode(ITmaxMediaRecord tmaxRecord)
		{
			Debug.Assert(tmaxRecord != null);
			
			//	What is the level of this media?
			switch(tmaxRecord.GetMediaLevel())
			{
				case FTI.Shared.Trialmax.TmaxMediaLevels.Primary:
					
					return m_ePrimaryTextMode;
					
				case FTI.Shared.Trialmax.TmaxMediaLevels.Secondary:
					
					return m_eSecondaryTextMode;
					
				case FTI.Shared.Trialmax.TmaxMediaLevels.Tertiary:
					
					return m_eTertiaryTextMode;
					
				case FTI.Shared.Trialmax.TmaxMediaLevels.Quaternary:
					
					return m_eQuaternaryTextMode;
					
				default:
				
					Debug.Assert(false);
					return m_ePrimaryTextMode;
			}
			
		}
		
		/// <summary>This method traps the Enter event fired by the tree control</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">System mouse event arguments</param>
		private void OnEnterTree(object sender, System.EventArgs e)
		{
			//	Is the user clicking on an already selected node when the tree gets focus
			if(m_tmaxClickNode != null)
			{
				//ISSUE256 FireAsyncCommand(TmaxCommands.Activate, m_tmaxClickNode);
				m_tmaxClickNode = null;
			}
		
		}// private void OnEnterTree(object sender, System.EventArgs e)
		
		/// <summary>This function will retrieve the tool associated with the specified command</summary>
		/// <param name="eCommand">The enumerated command identifier</param>
		/// <returns>Infragistic base class tool object</returns>
		private ToolBase GetUltraTool(TreePaneCommands eCommand)
		{
			//	The enumerated name is also the key
			return GetUltraTool(eCommand.ToString());
		}
				
		/// <summary>This method gets the export format associated with the specified command identifier</summary>
		/// <param name="strKey">The Infragistic key identifier</param>
		/// <returns>The associated tree command</returns>
		private FTI.Shared.Trialmax.TmaxExportFormats GetExportFormat(TreePaneCommands eCommand)
		{
			switch(eCommand)
			{
				case TreePaneCommands.ExportText:
				case TreePaneCommands.ExportAsciiObjections:
				
					return TmaxExportFormats.AsciiMedia;
					
				case TreePaneCommands.ExportXmlScript:
				
					return TmaxExportFormats.XmlScript;
					
				case TreePaneCommands.ExportXmlBinder:
				
					return TmaxExportFormats.XmlBinder;
					
				case TreePaneCommands.ExportVideo:
				
					return TmaxExportFormats.Video;
					
				case TreePaneCommands.ExportCodes:
				
					return TmaxExportFormats.Codes;
					
				case TreePaneCommands.ExportCodesDatabase:
				
					return TmaxExportFormats.CodesDatabase;
					
				case TreePaneCommands.ExportLoadFile:
				
					return TmaxExportFormats.LoadFile;
					
				default:
				
					return TmaxExportFormats.Unknown;
			}
			
		}// private FTI.Shared.Trialmax.TmaxExportFormats GetExportFormat(TreePaneCommand eCommand)

		#endregion Private Methods
		
		#region Public Methods
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public CTreePane() : base()
		{
			// This call is required to initialize the child controls
			InitializeComponent();
			
			if(m_tmaxTreeCtrl != null)
			{
				m_tmaxEventSource.Attach(m_tmaxTreeCtrl.EventSource);
			}
			
			if(m_tmaxTreeFilter != null)
			{
				m_tmaxTreeFilter.QueryStateAllowed += new CTmaxBaseTreeFilter.QueryStateAllowedHandler(this.OnDrawFilterQueryStateAllowed);
			}
			
			if(m_tmaxDisplaySorter != null)
			{
				m_tmaxDisplaySorter.KeepSorted = false;
				m_tmaxDisplaySorter.Sorter = new FTI.Trialmax.Controls.CTmaxBaseTreeSorter();
				m_tmaxDisplaySorter.Sorter.EventSource.Name = "Display Sorter";
				m_tmaxDisplaySorter.Sorter.Ascending = true;
				m_tmaxDisplaySorter.Sorter.SortBy = TmaxTreeSortFields.DisplayOrder;
				m_tmaxEventSource.Attach(m_tmaxDisplaySorter.Sorter.EventSource);
			}
			
			//	Initialize the drop target descriptor
			m_dropTarget.bDraggingMedia = true;
			m_dropTarget.bAllBinders	= false;
			m_dropTarget.eAction		= TreeDropActions.None;
			m_dropTarget.ePosition		= TmaxTreePositions.None;
			m_dropTarget.node			= null;
			
		}// CTreePane()
		
		/// <summary>This method is called to get the index of the image appropriate for the specified node</summary>
		/// <param name="tmaxNode">The node associated with the desired image</param>
		/// <returns>The index of the associated image</returns>
		public virtual int GetImageIndex(CTmaxMediaTreeNode tmaxNode)
		{
			CDxMediaRecord	dxRecord   = (CDxMediaRecord)tmaxNode.GetTmaxRecord(true);
			TmaxMediaTypes	eMediaType = TmaxMediaTypes.Unknown;
			bool			bPauseIndicator = false;
			CDxTertiary		dxDesignation = null;
			CDxQuaternary	dxLink = null;
			
			//	Is this a script scene?
			if((dxRecord != null) && (dxRecord.MediaType == TmaxMediaTypes.Scene))
			{
				//	Switch to using the source record
				if(((CDxSecondary)dxRecord).GetSource() != null)
					dxRecord = ((CDxSecondary)dxRecord).GetSource();
				else
					return 0;
			}

			//	Get the media type for this node
			if(dxRecord != null)
				eMediaType = dxRecord.MediaType;
			else
				eMediaType = tmaxNode.MediaType;
				
			switch(eMediaType)
			{
				case TmaxMediaTypes.Document:	return 3;
				case TmaxMediaTypes.Powerpoint:	return 13;
				case TmaxMediaTypes.Recording:	return 7;
				case TmaxMediaTypes.Script:		return 17;
				case TmaxMediaTypes.Deposition:	return 18;
				case TmaxMediaTypes.Page:
				
					//	Is this a high resolution page?
					if(dxRecord != null)
					{
						if((dxRecord.GetAttributes() & (uint)TmaxSecondaryAttributes.HighResPage) != 0)
						{
							return 4;
						}
					}

					//	Normal graphic	
					return 5;
					
				case TmaxMediaTypes.Segment:
				
					if((dxRecord != null) && (dxRecord.GetParent() != null))
					{
						if(dxRecord.GetParent().MediaType == TmaxMediaTypes.Deposition)
							return 30;
						else
							return 31;
					}
					else
					{
						return 31;
					}
					
				case TmaxMediaTypes.Slide:			return 16;
				case TmaxMediaTypes.Scene:			return 12;

				case TmaxMediaTypes.Treatment:

					//	Is this a split screen treatment?
					if((dxRecord != null) && (((CDxTertiary)dxRecord).SplitScreen == true))
					{
						if(((CDxTertiary)dxRecord).SplitHorizontal == true)
						{
							return 55;
						}
						else 
						{
							return 54;
						}

					}
					
					//	Default value
					return 6;

				case TmaxMediaTypes.Clip:
				
					//	Is the clip tuned?
					if(dxRecord != null)
					{
						if(((CDxTertiary)dxRecord).StartTuned == true)
						{
							if(((CDxTertiary)dxRecord).StopTuned == true)
								return 29;
							else
								return 27;
						}
						else if(((CDxTertiary)dxRecord).StopTuned == true)
						{
							return 28;
						}
						
					}
					
					//	Untuned by default
					return 26;
					
				case TmaxMediaTypes.Designation:
				
					if(dxRecord != null)
					{
						//	Should we show the Pause indicator?
						if((m_dPauseThreshold > 0) && (((CDxTertiary)dxRecord).GetExtent() != null) && 
						   (((CDxTertiary)dxRecord).GetExtent().MaxLineTime > m_dPauseThreshold))
							bPauseIndicator = true;
							
						if(((CDxTertiary)dxRecord).StartTuned == true)
						{
							if(((CDxTertiary)dxRecord).StopTuned == true)
								return (bPauseIndicator ? 35 : 25);
							else
								return (bPauseIndicator ? 33 : 23);
						}
						else if(((CDxTertiary)dxRecord).StopTuned == true)
						{
								return (bPauseIndicator ? 34 : 24);
						}
						else
						{
							return (bPauseIndicator ? 32 : 22);
						}
						
					}
					
					//	Untuned by default
					return 22;
					
				case TmaxMediaTypes.Link:			
				
					//	Is the link tuned?
					if((dxLink = (CDxQuaternary)dxRecord) == null)
						return 20; // default to untuned link (no text or video)
						
					//	Make sure it's in range
					if((dxDesignation = (CDxTertiary)(dxLink.GetParent())) != null)
					{
						if((dxLink.Start < dxDesignation.Start) || (dxLink.Start > dxDesignation.Stop))
							return 37;
					}
					
					//	Is this a Hide link?
					if(dxLink.HideLink == true)
					{
						//	Is the scrolling text visible?
						if(dxLink.HideText == false)
						{
							//	Is the video playback visible?
							if(dxLink.HideVideo == false)
								return ((dxLink.StartTuned == true) ? 52 : 53);
							else
								return ((dxLink.StartTuned == true) ? 48 : 49);
						}
						else
						{
							//	Is the video playback visible?
							if(dxLink.HideVideo == false)
								return ((dxLink.StartTuned == true) ? 46 : 47);
							else
								return ((dxLink.StartTuned == true) ? 50 : 51);

						}// if(dxLink.HideText == false)

					}
					else
					{
						//	Is the scrolling text visible?
						if(dxLink.HideText == false)
						{
							//	Is the video playback visible?
							if(dxLink.HideVideo == false)
								return ((dxLink.StartTuned == true) ? 42 : 43);
							else
								return ((dxLink.StartTuned == true) ? 44 : 45);
						}
						else
						{
							//	Is the video playback visible?
							if(dxLink.HideVideo == false)
								return ((dxLink.StartTuned == true) ? 40 : 41);
							else
								return ((dxLink.StartTuned == true) ? 21 : 20);

						}// if(dxLink.HideText == false)
						
					}// if(dxLink.HideLink == true)
					
				case TmaxMediaTypes.Unknown:
				
					//	Is this a binder entry?
					if(tmaxNode.IBinder != null)
					{
						//	Is this supposed to be a media reference?
						if(((CDxBinderEntry)(tmaxNode.IBinder)).IsMedia() == true)
						{
							//	Assume invalid media record
							return 0;
						}
						else
						{
							if((m_tmaxDatabase != null) && (ReferenceEquals(m_tmaxDatabase.TargetBinder, tmaxNode.IBinder) == true))
								return 36;
							else
								return 1;
						}
						
					}
					else
					{
						return 1;
					}
					
				default:
				
					return 1;
			
			}// switch(eMediaType)
			
		}
		
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
					if((double)(m_tmaxStationOptions.PauseThreshold) != m_dPauseThreshold)
					{
						//	Update the value and notify the derived classes
						m_dPauseThreshold = (double)(m_tmaxStationOptions.PauseThreshold);
						OnPauseThresholdChanged();
					}
						
				}
				catch
				{
				}
					
			}// if(bCancelled == false)
					
		}// public override void OnAfterSetCaseOptions(bool bCancelled)

		/// <summary>This method is called by the application to when the item has been updated by the user</summary>
		/// <param name="tmaxItem">The item that has been updated</param>
		public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{
			ITmaxMediaRecord tmaxRecord = null;
			
			if(tmaxItem != null)
				tmaxRecord = tmaxItem.GetMediaRecord();
			if(tmaxRecord == null)
				return;
				
			//	Check all nodes in the tree
			if((m_tmaxTreeCtrl != null) && (m_tmaxTreeCtrl.Nodes != null))
			{
				foreach(CTmaxMediaTreeNode O in m_tmaxTreeCtrl.Nodes)
					Update(O, tmaxRecord);
			}
			
		}// public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		/// <summary>
		/// This method is called by the application to initialize the pane
		/// </summary>
		/// <returns>true if successful</returns>
		/// <remarks>Derived classes should override for custom runtime initialization</remarks>
		public override bool Initialize(CXmlIni xmlINI)
		{
			//	Do the base class processing first
			if(base.Initialize(xmlINI) == false)
				return false;
				
			//	Get the preferences from the ini file
			if(xmlINI.SetSection(m_strPaneName) == true)
			{
				m_ePrimaryTextMode = (TmaxTextModes)xmlINI.ReadEnum(KEY_PRIMARY_TEXT_MODE, m_ePrimaryTextMode);
				m_eSecondaryTextMode = (TmaxTextModes)xmlINI.ReadEnum(KEY_SECONDARY_TEXT_MODE, m_eSecondaryTextMode);
				m_eTertiaryTextMode = (TmaxTextModes)xmlINI.ReadEnum(KEY_TERTIARY_TEXT_MODE, m_eTertiaryTextMode);
				//m_eQuaternaryTextMode = (TmaxTextModes)xmlINI.ReadEnum(KEY_QUATERNARY_TEXT_MODE, m_eQuaternaryTextMode);
			}
			
			return true;
		}
		
		/// <summary>
		/// This method is called by the application when it is about to terminate
		/// </summary>
		/// <remarks>Derived classes should override for custom shutdown</remarks>
		public override void Terminate(CXmlIni xmlINI)
		{
			//	Save the preferences to the ini file
			if(xmlINI.SetSection(m_strPaneName) == true)
			{
				xmlINI.Write(KEY_PRIMARY_TEXT_MODE, m_ePrimaryTextMode);
				xmlINI.Write(KEY_SECONDARY_TEXT_MODE, m_eSecondaryTextMode);
				xmlINI.Write(KEY_TERTIARY_TEXT_MODE, m_eTertiaryTextMode);
				xmlINI.Write(KEY_QUATERNARY_TEXT_MODE, m_eQuaternaryTextMode);
			}

		}
		
		/// <summary>This method is called by the application to notify the panes to refresh their text</summary>
		public override void RefreshText()
		{
			//	Set text for all media levels
			try { SetText(TmaxMediaLevels.Primary); } catch{}
			try { SetText(TmaxMediaLevels.Secondary); } catch{}
			try { SetText(TmaxMediaLevels.Tertiary); } catch{}
			try { SetText(TmaxMediaLevels.Quaternary); } catch{}
			
		}// public override void RefreshText()
		
		/// <summary>This method handles all KeyDown notifications from the application</summary>
		/// <param name="eKey">The key being pressed</param>
		///	<param name="eModifiers">The current control/shift/alt key states</param>
		/// <returns>true if processed</returns>
		public override bool OnKeyDown(Keys eKey, Keys eModifiers)
		{
			TreePaneCommands eCommand = TreePaneCommands.Invalid;
			
			//	Translate the keystroke to a media bar command
			switch(CTmaxMediaBar.GetCommand(eKey, eModifiers))
			{
				case TmaxMediaBarCommands.RotateCCW:	
					
					eCommand = TreePaneCommands.RotateCCW;
					break;
				
				case TmaxMediaBarCommands.RotateCW:	
				
					eCommand = TreePaneCommands.RotateCW;
					break;
			}
			
			//	Did this key translate to a local command?
			if(eCommand != TreePaneCommands.Invalid)
			{
				if(GetCommandEnabled(eCommand, GetSelections(false)) == true)
				{
					OnCommand(eCommand);
					return true; // Command has been processed
				}
			
			}
			
			return false;
		
		}// public override bool OnKeyDown(Keys eKey, Keys eModifiers)
		
		/// <summary>This method handles all Hotkey notifications from the application</summary>
		/// <param name="eHotkey">The enumerated hotkey</param>
		/// <returns>true if processed by the pane</returns>
		public override bool OnHotkey(TmaxHotkeys eHotkey)
		{
			CTmaxMediaTreeNodes	tmaxNodes = null;
			TreePaneCommands	eCommand = TreePaneCommands.Invalid;
			bool				bProcessed = false;

			//	Get the current selections
			tmaxNodes = GetSelections(true);
				
			//	Which hotkey has been pressed?
			switch(eHotkey)
			{
				case TmaxHotkeys.Copy:
				
					eCommand = TreePaneCommands.Copy;
					break;

				case TmaxHotkeys.Delete:

					eCommand = TreePaneCommands.Delete;
					break;

				case TmaxHotkeys.Paste:
				
					//	If Paste is enabled use that, otherwise use
					//	Paste Before
					if(GetCommandEnabled(TreePaneCommands.Paste, tmaxNodes) == true)
						eCommand = TreePaneCommands.Paste;
					else if(GetCommandEnabled(TreePaneCommands.PasteBefore, tmaxNodes) == true)
						eCommand = TreePaneCommands.PasteBefore;
						
					break;
					
				case TmaxHotkeys.Print:
				
					eCommand = TreePaneCommands.Print;
					break;
					
				case TmaxHotkeys.Find:
				
					eCommand = TreePaneCommands.Find;
					break;
					
				case TmaxHotkeys.SetFilter:
				
					eCommand = TreePaneCommands.SetFilter;
					break;

				case TmaxHotkeys.AddObjection:

					eCommand = TreePaneCommands.AddObjection;
					break;

				case TmaxHotkeys.RepeatObjection:

					eCommand = TreePaneCommands.RepeatObjection;
					break;

				case TmaxHotkeys.AddToBinder:
				case TmaxHotkeys.AddToScript:
				default:
				
					//MessageBox.Show(eHotkey.ToString() + " hotkey not yet implemented in " + PaneName);
					break;
			}
		
			//	Did this hotkey translate to a command?
			if(eCommand != TreePaneCommands.Invalid)
			{
				//	Is this command visible?
				if(GetCommandVisible(eCommand, tmaxNodes) == true)
				{
					//	Is this command enabled
					if(GetCommandEnabled(eCommand, tmaxNodes) == true)
					{
						//	Prompt for confirmation if attempting to delete records
						if((eCommand != TreePaneCommands.Delete) || (GetDeleteConfirmation() == true))
							OnCommand(eCommand);
							
						bProcessed = true;

					}// if(GetCommandEnabled(eCommand, tmaxNodes) == true)

				}// if(GetCommandVisible(eCommand, tmaxNodes) == true)

			}// if(eCommand != TreePaneCommands.Invalid)
			
			return bProcessed;
			
		}// public override bool OnHotkey(TmaxHotkeys eHotkey)
		
		/// <summary>This method is called by the application to notify the pane that the user is dragging new source media</summary>
		/// <param name="eSourceType">Enumerated source type identifier</param>
		public override void OnStartSourceDrag(RegSourceTypes eSourceType)
		{
			//	Do the base class processing
			base.OnStartSourceDrag(eSourceType);
			
			//	Clear the current drop node
			m_dropTarget.node = null;
			m_dropTarget.eAction = TreeDropActions.None;
			
			//	Update the current drop action
			m_dropTarget.eAction = GetDropAction();

		}// OnStartSourceDrag(RegSourceTypes eSourceType)

		/// <summary>This method is called by the application to notify the pane that the user is dragging database records</summary>
		/// <param name="tmaxItems">TrialMax event items being dragged</param>
		public override void OnStartDragRecords(CTmaxItem tmaxItem)
		{
			//	Do the base class processing
			base.OnStartDragRecords(tmaxItem);
			
			//	Clear the current drop node
			m_dropTarget.node = null;
			m_dropTarget.eAction = TreeDropActions.None;
			
		}// public virtual void OnStartDragRecords(CTmaxItems tmaxItems)

		/// <summary>
		/// This method is called to notify the pane that the user has stopped dragging source files
		/// </summary>
		/// <param name="eSourceType">Enumerated source type identifier</param>
		public override void OnCompleteSourceDrag()
		{
			m_tmaxTreeFilter.ClearDropHighlight();
			
			//	Do the base class processing
			base.OnCompleteSourceDrag();
			
			//	NOTE:	We do not clear the drop node because the owner may 
			//			want to know what node we were on for the operation
		}

        public bool CheckFolderAccess()
        {
            bool successful = true;
            string folder = null;
            string folderLoc = null;
            string fileName = "Fti.ini";
            string filePath = AppDomain.CurrentDomain.BaseDirectory + fileName;

            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // Read the stream to a string, and write the string to the console.
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains("FilePath"))
                        {
                            string[] tokens = line.Split('=');
                            folder = tokens[1];
                        }
                    }

                    folderLoc = folder + "checkaccess.txt";

                    File.Create(folderLoc).Close(); //Will show exception if access is not granted
                    if (File.Exists(folderLoc))
                    {
                        File.Delete(folderLoc);
                    }
                }
            }
            catch (Exception Ex)
            {
                m_tmaxEventSource.FireError(this, "Presentation Recording", "TrialMax does not have access to save files in the current Video Export Path! Please correct Video Export Path found in Presentation Options", Ex);
                successful = false;
            }

            return successful;
        }

		#endregion Public Methods

		#region Properties
		
		/// <summary>Trialmax tree control owned by the pane</summary>
		public CTmaxMediaTreeCtrl TreeCtrl
		{
			get
			{
				return m_tmaxTreeCtrl;
			}
		}
		
		/// <summary>Trialmax tree node that is the current drop target</summary>
		public CTmaxMediaTreeNode DropNode
		{
			get
			{
				return m_dropTarget.node;
			}
		}
		
		/// <summary>Maximum number of child nodes in a super node</summary>
		public int SuperNodeSize
		{
			get
			{
				return m_iSuperNodeSize;
			}
			set
			{
				m_iSuperNodeSize = value;
			
				//	Notify the derived class
				OnSuperNodeSizeChanged();
			}
			
		}//	SuperNodeSize property
			
		/// <summary>Text mode used to display Primary nodes</summary>
		public FTI.Shared.Trialmax.TmaxTextModes PrimaryTextMode
		{
			get
			{
				return m_ePrimaryTextMode;
			}
			set
			{
				m_ePrimaryTextMode = value;
			
				//	Notify the derived class
				OnPrimaryTextModeChanged();
			}
			
		}
		
		/// <summary>Text mode used to display Secondary nodes</summary>
		public FTI.Shared.Trialmax.TmaxTextModes SecondaryTextMode
		{
			get
			{
				return m_eSecondaryTextMode;
			}
			set
			{
				m_eSecondaryTextMode = value;
			
				//	Notify the derived class
				OnSecondaryTextModeChanged();
			}
			
		}
		
		/// <summary>Text mode used to display Tertiary nodes</summary>
		public FTI.Shared.Trialmax.TmaxTextModes TertiaryTextMode
		{
			get
			{
				return m_eTertiaryTextMode;
			}
			set
			{
				m_eTertiaryTextMode = value;
			
				//	Notify the derived class
				OnTertiaryTextModeChanged();
			}
			
		}
		
		/// <summary>Text mode used to display Quaternary nodes</summary>
		public FTI.Shared.Trialmax.TmaxTextModes QuaternaryTextMode
		{
			get
			{
				return m_eQuaternaryTextMode;
			}
			set
			{
				m_eQuaternaryTextMode = value;
			
				//	Notify the derived class
				OnQuaternaryTextModeChanged();
			}
			
		}
		
		/// <summary>Sorter used for primary media nodes</summary>
		///
		///	<remarks>The derived class allocates this object if required</remarks>
		public FTI.Trialmax.Controls.CTmaxBaseTreeSorter PrimarySorter
		{
			get
			{
				return m_tmaxPrimarySorter;
			}
		}

	    public bool IsBinder;
		
		/// <summary>True to show check boxes on each node</summary>
		public bool CheckBoxes
		{
			get { return m_bCheckBoxes; }
			set { m_bCheckBoxes = value; }
		}
		
		/// <summary>True to enable drag/drop operations</summary>
		public bool EnableDragDrop
		{
			get { return m_bEnableDragDrop; }
			set { m_bEnableDragDrop = value; }
		}
		
		/// <summary>True to enable the pane's context menu</summary>
		public bool EnableContextMenu
		{
			get { return m_bEnableContextMenu; }
			set { m_bEnableContextMenu = value; }
		}
		
		#endregion

	}// public class CTreePane : FTI.Trialmax.Panes.CBasePane

}// namespace FTI.Trialmax.Panes
