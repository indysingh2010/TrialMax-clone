using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared.Trialmax;
using FTI.Trialmax.Controls;
using FTI.Trialmax.Forms;
using FTI.Trialmax.Database;

using Infragistics.Win.UltraWinTree;
using FTI.Shared.Xml;

namespace FTI.Trialmax.Panes
{
	/// <summary>
	/// This class will create and manage the application's virtual tree. This
	///	is the tree that gives the user a configurable view of the database contents.
	/// </summary>
	public class CBinderTree : FTI.Trialmax.Panes.CTreePane
	{
        #region Constants

        protected const string KEY_PRIMARY_SORT_FIELD = "PrimarySortField";
        protected const string KEY_SORTBY_DISPLAYORDER = "SortByDisplayOrder";
        protected const string KEY_SORT_FIELD_DATA = "SortFieldData";
        protected const string KEY_PRIMARY_SORT_ASCENDING = "PrimarySortAscending";
        protected const string KEY_PRIMARY_SORT_CASE_SENSITIVE = "PrimarySortCaseSensitive";
        protected const string KEY_SUPER_NODE_SIZE = "SuperNodeSize";

        #endregion Constants

		#region Private Members
		
		/// <summary>Class member bound to AddFromPresentation property</summary>
		bool m_bAddFromPresentation = false;
        
        /// <summary> flag for rearrangment of media in binder tree(Add/Move) </summary>
        private bool SortByDisplayOrder;

		#endregion Private Members

		/// <summary>This structure groups the members required to manage the drop target for new registrations</summary>
		/// <remarks>Since there is no parameterized constructor the members must be initialized if statically allocated</remarks>
		protected struct SRegistrationTarget
		{
			/// <summary>The drop node pointed to by the user</summary>
			public CTmaxMediaTreeNode node;
		
			/// <summary>The drop node parent</summary>
			public CTmaxMediaTreeNode parent;
		
			/// <summary>True if inserting new entries into the parent</summary>
			public bool bInsert;
		
			/// <summary>True if inserting before the drop node</summary>
			public bool bBefore;
		
			/// <summary>True if the user has drop registered into the root</summary>
			public bool bInRoot;
		
		}// protected struct SRegistrationTarget
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CBinderTree() : base()
		{
			if(m_tmaxTreeCtrl != null)
				m_tmaxTreeCtrl.EventSource.Name = "Binder TreeCtrl";
            //	Allocate the sorter interface for primary media nodes
            m_tmaxPrimarySorter = new FTI.Trialmax.Controls.CTmaxBaseTreeSorter();
            m_tmaxPrimarySorter.EventSource.Name = "PT Primary Sorter";
            m_tmaxEventSource.Attach(m_tmaxPrimarySorter.EventSource);
            this.IsBinder = true;
            //	Register events from the media node collection
            //m_tmaxEventSource.Attach(m_aMediaNodes.EventSource);
		}

		/// <summary>This method is called by the application to notify the pane that the user is dragging database records</summary>
		/// <param name="tmaxItems">TrialMax event items being dragged</param>
		public override void OnStartDragRecords(CTmaxItem tmaxItem)
		{

			//	Do the base class processing
			base.OnStartDragRecords(tmaxItem);
			
			m_dropTarget.bDraggingMedia = true;
			m_dropTarget.bAllBinders = false;
			
			//	We need to determine if the user is dragging binders or media
			if((m_tmaxDragData != null) && (m_tmaxDragData.GetMediaRecord() == null))
			{
				//	Check each of the top level source items because the
				//	user could be dragging one or more media nodes from
				//	a binder
				m_dropTarget.bDraggingMedia = false;
				m_dropTarget.bAllBinders = true;
				foreach(CTmaxItem O in m_tmaxDragData.SourceItems)
				{
					if(O.GetMediaRecord() != null)
					{
						m_dropTarget.bDraggingMedia = true;
						if(m_dropTarget.bAllBinders == false)
							break;
					}
					if(O.IBinderEntry == null)
					{
						m_dropTarget.bAllBinders = false;
						if(m_dropTarget.bDraggingMedia == true)
							break;
					}
					
				}// foreach(CTmaxItem O in m_tmaxDragData.SourceItems)
			
			}// if((m_tmaxDragData != null) && (m_tmaxDragData.GetMediaRecord() == null))
			
		}// public virtual void OnStartDragRecords(CTmaxItems tmaxItems)

        public bool AddToBinderFromHotKey()
        {
            CTmaxMediaTreeNode tmaxTarget = GetSelection(); // get selected Binder where the Media will be added. Use GetSelections() to get multiple selected binder
            CMediaTree mediaTree = (CMediaTree)this.Parent.Parent.Controls[1].Controls[0]; // get access to Media Tree
            CTmaxItems tmaxSource = mediaTree.GetCmdPrintItems(); // get the selected items in the Media Tree (Documents, Scripts, Powerpoints, Recordings)
            if (tmaxSource == null)
                tmaxSource = mediaTree.GetCmdFindItems(); // get the selected items in the Media Tree (Depositions)
            if (tmaxTarget == null || tmaxSource == null) return false;
            Add(tmaxTarget, tmaxSource, false, false); // Add the selected items in the Media Tree to the selected Binder
            return true;
        }

		/// <summary>This method is called when the user has dropped media records in the tree</summary>
		/// <param name="eAction">The action to be taken</param>
		protected override void OnDroppedRecords(TreeDropActions eAction)
		{
           

			//	Must be dragging something
			if(m_tmaxDragData == null) return;
			if(m_tmaxDragData.SourceItems == null) return;
			if(m_tmaxDragData.SourceItems.Count == 0) return;
			if(m_dropTarget.node == null) return;
			
			switch(eAction)
			{
				case TreeDropActions.Add:
				
					Add(m_dropTarget.node, m_tmaxDragData.SourceItems, false, false);
					break;
					
				case TreeDropActions.InsertAfter:
				
					Add(m_dropTarget.node, m_tmaxDragData.SourceItems, true, false);
                    
					break;
					
				case TreeDropActions.InsertBefore:
				
					Add(m_dropTarget.node, m_tmaxDragData.SourceItems, true, true);
					break;
					
				case TreeDropActions.MoveInto:
				
					MoveTo(m_dropTarget.node, m_tmaxDragData.SourceItems, true, false);
					break;
					
				case TreeDropActions.MoveAfter:
				
					MoveTo(m_dropTarget.node, m_tmaxDragData.SourceItems, false, false);
					break;
					
				case TreeDropActions.MoveBefore:
				
					MoveTo(m_dropTarget.node, m_tmaxDragData.SourceItems, false, true);
					break;
					
				case TreeDropActions.Reorder:
				
					Reorder(m_tmaxDragData.SourceItems, m_dropTarget.node, (m_dropTarget.ePosition == TmaxTreePositions.AboveNode));
					break;
					
			}

            // On Addition of new binder, it sorts by displayorder
		    SortByDisplayOrder = true;
           // m_tmaxPrimarySorter.SortBy = TmaxTreeSortFields.DisplayOrder;
			
		}// protected override void OnDroppedRecords(TreeDropActions eAction)

	   

		/// <summary>This method is called by the application when new media gets registered</summary>
		/// <param name="tmaxFolder">The source folder containing the new media</param>
		/// <param name="ePane">The pane that accepted the registration request</param>
		public override void OnRegistered(CTmaxSourceFolder tmaxSource, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
			SRegistrationTarget	regTarget = new SRegistrationTarget();
			CTmaxItems			tmaxAdditions = null;
			
			//	Make sure we have the required objects
			if(tmaxSource == null) return;
			if(m_tmaxTreeCtrl == null) return;

			//	Don't bother if the user did not drop into this pane
			if(ePane != TmaxAppPanes.Binders) return;
			
			//	Get the descriptor for the drop location
			if(GetRegistrationTarget(ref regTarget) == false) return;
			
			//	Get the collection of items that represent the binder entries to be added
			if((tmaxAdditions = GetRegisteredAdditions(tmaxSource)) == null) return;
			
			//	Did the user drop register into the root?
			if(regTarget.bInRoot == true)
			{
				//	We are not allowed to drop records directly into the root so,
				//	if any one of our top level additions is a media record, then
				//	we have to add a parent binder
				tmaxAdditions = AddRegistrationParent(tmaxAdditions);
			}

			//	Add the new entries to the tree
			Add(regTarget.node, tmaxAdditions, regTarget.bInsert, regTarget.bBefore);

		}// public override void OnRegistered(CTmaxSourceFolder tmaxSource, int iPane)
				
		/// <summary>This method is called by the application when it adds new media to the database</summary>
		/// <param name="tmaxParent">TrialMax event item that identifies the parent record</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		{
			CTmaxMediaTreeNode	tmaxParentNode = null;
			CTmaxMediaTreeNode	tmaxSelect = null;
			
			Debug.Assert(tmaxParent != null);
			Debug.Assert(tmaxChildren != null);

			//	All new media should be in the child collection
			Debug.Assert(tmaxChildren.Count > 0);
			if(tmaxChildren.Count == 0) return;
		
			//	Is the parent a media node?
			if(tmaxParent.GetMediaRecord() != null)
			{
				OnAddMedia((CDxMediaRecord)tmaxParent.GetMediaRecord(), tmaxChildren);
			}
			else
			{
				//	May or may not have a parent - could be adding to the root
				if(tmaxParent.IBinderEntry != null)
				{
					if((tmaxParentNode = GetBinderNode(null, (CDxBinderEntry)tmaxParent.IBinderEntry, false)) == null)
					{
						//	User must not have expanded the parent's parent node yet
						//
						//	NOTE:	This could be an entry being added to the target binder
						//			from TmaxPresentation
						return;
					}
					
				}// if(tmaxParent.IBinderEntry != null)

				//	Add the new nodes to the tree
				OnAddEntries(tmaxParentNode, tmaxChildren);
								
				//	Make sure the node's collection is properly sorted
				if(tmaxParentNode != null)
					Sort(tmaxParentNode,true);
				else
					Sort(m_tmaxTreeCtrl.Nodes,true);
				
				//	Select the first new binder element node
				foreach(CTmaxItem O in tmaxChildren)
				{
					if(O.IBinderEntry != null)
					{
						if((tmaxSelect = GetBinderNode(tmaxParentNode, (CDxBinderEntry)O.IBinderEntry, false)) != null)
							break;
					}

				}// foreach(CTmaxItem O in tmaxChildren)
				
				if((tmaxSelect != null) && (m_bAddFromPresentation == false))
				{
					m_tmaxTreeCtrl.SetSelection(tmaxSelect);
					tmaxSelect.BringIntoView(false);
				}

                // On Addition of new binder, it sorts by displayorder
//			    m_tmaxPrimarySorter.SortBy = TmaxTreeSortFields.DisplayOrder;
			    SortByDisplayOrder = true;

			}// if(tmaxParent.IPrimary != null)
			
		}// public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		
		/// <summary>This method is called by the application after the database processes a Move command</summary>
		/// <param name="tmaxMoved">Event item that identifies the records that have been moved</param>
		public override void OnMoved(CTmaxItem tmaxMoved)
		{
			CTmaxMediaTreeNode tmaxChildNode = null;
			
			Debug.Assert(tmaxMoved != null);
			if(tmaxMoved == null) return;
			
			//	Has anything been moved?
			//
			//	NOTE:	When this pane fires the Move event it puts the node items in
			//			the SourceItems collection. When the database makes the move,
			//			it transfers the items to the SubItems collection
			if((tmaxMoved.SubItems != null) && (tmaxMoved.SubItems.Count > 0))
			{
				//	Delete the existing nodes
				foreach(CTmaxItem O in tmaxMoved.SubItems)
				{
					//	This should be a binder entry
					if(O.IBinderEntry == null) continue;
				
					//	Get the child node for this entry
					if((tmaxChildNode = GetBinderNode(null, ((CDxBinderEntry)(O.IBinderEntry)), false)) != null)
					{
						OnNodeDeleted(tmaxChildNode, false);
					
					}// if((tmaxChildNode = GetBinderNode(null, ((CDxBinderEntry)(O.IBinderEntry)), false)) != null)
			
				}// foreach(CTmaxItem O in tmaxMoved.SubItems)			
			
				//	Add the nodes in their new location
				OnAdded(tmaxMoved, tmaxMoved.SubItems);

			}// if((tmaxMoved.SubItems != null) && (tmaxMoved.SubItems.Count > 0))
			
		}// public override void OnMoved(CTmaxItem tmaxMoved)
		
		/// <summary>This method is called by the application when records have been deleted</summary>
		/// <param name="tmaxItems">A collection of items that represent the parents of the records that have been deleted</param>
		public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			//	The collection specified by the caller is a collection of parent records
			foreach(CTmaxItem tmaxParent in tmaxItems)
			{
				if((tmaxParent.SubItems != null) && (tmaxParent.SubItems.Count > 0))
				{
					try
					{
						//	Has the user deleted binders?
						if(tmaxParent.DataType == TmaxDataTypes.Binder)
							OnBindersDeleted((CDxBinderEntry)tmaxParent.IBinderEntry, tmaxParent.SubItems);
						else if(tmaxParent.DataType == TmaxDataTypes.Media)
							OnMediaDeleted(tmaxParent, tmaxParent.SubItems);
					}
					catch
					{
					}
					
				}// if((tmaxParent.SubItems != null) && (tmaxParent.SubItems.Count > 0))
				
			}// foreach(CTmaxItem tmaxParent in tmaxItems)
            
            // On Addition of new binder, it sorts by displayorder
		    SortByDisplayOrder = true;
           // m_tmaxPrimarySorter.SortBy = TmaxTreeSortFields.DisplayOrder;

		}// public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)

		/// <summary>This method is called by the application to when the item's child collection has been reordered</summary>
		/// <param name="tmaxItem">The item that owns the child collection</param>
		public override void OnReordered(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{
			CTmaxMediaTreeNodes	tmaxNodes = null;
			CTmaxMediaTreeNode	tmaxNode = null;
			
			//	Ignore case codes
			if(tmaxItem.DataType == TmaxDataTypes.CaseCode) return;
			
			//	Is this a binder?
			if(tmaxItem.GetMediaRecord() == null)
			{
				if(tmaxItem.IBinderEntry != null)
				{
					if((tmaxNode = GetBinderNode(null, (CDxBinderEntry)tmaxItem.IBinderEntry, false)) != null)
						Refresh(tmaxNode);
				}
				else
				{
					//	The user must have reordered root nodes
					Sort(m_tmaxTreeCtrl.Nodes,true);
					
					foreach(CTmaxMediaTreeNode O in m_tmaxTreeCtrl.Nodes)
						SetText(O, false);
				}
				
			}
			
			//	Is this item a designation or clip?
			else if((tmaxItem.MediaType == TmaxMediaTypes.Designation) ||
			        (tmaxItem.MediaType == TmaxMediaTypes.Clip))
			{
				//	We need to locate all scenes that reference this item
				tmaxNodes = new CTmaxMediaTreeNodes();
				GetScenesFromSource(null, (CDxMediaRecord)tmaxItem.GetMediaRecord(), tmaxNodes);

				//	Now refresh each scene since the children appear under the scene
				//	node instead of the designation/clip node
				foreach(CTmaxMediaTreeNode O in tmaxNodes)
					Refresh(O);
					
				tmaxNodes.Clear();
			}
			else
			{
				//	We need to locate all nodes that reference this item
				tmaxNodes = new CTmaxMediaTreeNodes();
				GetNodesFromRecord(null, tmaxItem.GetMediaRecord(), tmaxNodes);

				//	Refresh all the nodes
				foreach(CTmaxMediaTreeNode O in tmaxNodes)
					Refresh(O);

				//	We're done with the nodes
				tmaxNodes.Clear();
				
				//	Refresh all scenes because the user may have just
				//	modified a record that is a source for a scene
				if(tmaxItem.MediaType != TmaxMediaTypes.Script)
				{
					//	NOTE:	We don't specify the source record here because
					//			the scene could be referencing the parent or
					//			one of its children that has been reordered
					RefreshScenes(null, null);
				}

			}

		}// public override void OnReordered(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		/// <summary>This method is called by the application to when the item has been updated by the user</summary>
		/// <param name="tmaxItem">The item that has been updated</param>
		public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{
			CTmaxMediaTreeNode tmaxNode = null;
			
			Debug.Assert(tmaxItem != null);
			if(tmaxItem == null) return;
			
			//	Is this a media record being updated?
			if(tmaxItem.GetMediaRecord() != null)
			{
				//	Let the base class handle it
				base.OnUpdated(tmaxItem);
			}

			//	Is this a binder?
			if(tmaxItem.IBinderEntry != null)
			{
				if((tmaxNode = GetBinderNode(null, (CDxBinderEntry)tmaxItem.IBinderEntry, false)) != null)
				{
					//	Has the source record changed?
					//
					//	Updating on Import could reassign the source
					if(ReferenceEquals(tmaxNode.GetMediaRecord(), ((CDxBinderEntry)tmaxItem.IBinderEntry).GetSource(true)) == false)
						tmaxNode.SetMediaRecord(((CDxBinderEntry)tmaxItem.IBinderEntry).GetSource(true));
						
					Refresh(tmaxNode);
				}
			
			}// if(tmaxItem.IBinderEntry != null)
			
		}// public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItem tmaxItem)

        /// <summary>This method is called by the application when it is about to terminate</summary>
        /// <remarks>Derived classes should override for custom shutdown</remarks>
        public override void Terminate(CXmlIni xmlINI)
        {
           

        }

        private void SaveBinderSortProperty()
        {
            foreach (CTmaxMediaTreeNode tmaxChild in m_tmaxTreeCtrl.Nodes)
            {
                //ITmaxMediaRecord record = tmaxChild.GetTmaxRecord(true);
                //SetSortProprety(tmaxChild);
            }
        }

        private void SetSortProprety(CTmaxMediaTreeNode node)
        {
            if (node == null) return;
            CDxBinderEntry binder = (CDxBinderEntry)node.IBinder;
            if (binder != null)
            {
                binder.SpareNumber = (int)node.Sorter.SortBy;
                binder.SpareText = node.Sorter.CaseCodeId.ToString();
                m_tmaxDatabase.ResetSortProperty((CDxBinderEntry)node.IBinder);
            }
        }
	    #endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to let the derived class determine if the specified node can be dragged</summary>
		/// <param name="tmaxNode">The node to be dragged</param>
		/// <returns>true if the node can be dragged</returns>
		protected override bool CanDrag(CTmaxMediaTreeNode tmaxNode)
		{
			//	Is this a valid media node
			if(m_bEnableDragDrop == false)
				return false;
			else if(tmaxNode.GetMediaRecord() != null)
				return base.CanDrag(tmaxNode);
			else if(tmaxNode.IBinder != null)
				return true;
			else
				return false;
		
		}// protected override bool CanDrag(CTmaxMediaTreeNode tmaxNode)
		
		/// <summary>This method handles the event fired when the user clicks on Copy from the context menu</summary>
		/// <param name="tmaxNodes">The nodes that are currently selected</param>
		protected override void OnCmdCopy(CTmaxMediaTreeNodes tmaxNodes)
		{
			CTmaxItems tmaxItems = null;
			
			if(tmaxNodes == null) return;
			if(tmaxNodes.Count == 0) return;
			
			//	Get the collection of event items
			if((tmaxItems = GetBinderItems(tmaxNodes)) != null)
			{
				if(tmaxItems.Count > 0)
				{
					//	Notify the application
					FireCommand(TmaxCommands.Copy, tmaxItems);
				}
			
			}
			
		}// protected override void OnCmdCopy(CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method handles the event fired when the user clicks on Set Target Binder from the context menu</summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		protected override void OnCmdSetTargetBinder(CTmaxMediaTreeNode tmaxNode)
		{
			if((tmaxNode != null) && (tmaxNode.IPrimary == null) && (tmaxNode.IBinder != null))
			{
				FireCommand(TmaxCommands.SetTargetBinder, tmaxNode, false, null);
			}

		}// protected override void OnCmdSetTargetBinder(CTmaxMediaTreeNode tmaxNode)
		
		/// <summary>This method handles the event fired when the user clicks on a selection in the New popup or Insert popup menus</summary>
		/// <param name="tmaxMediaType">Type of media to be added to the tree</param>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		/// <param name="bInsert">true if inserting a new node</param>
		/// <param name="bBefore">true to insert before the selected node</param>
		protected override void OnCmdNew(TmaxMediaTypes tmaxMediaType, CTmaxMediaTreeNode tmaxNode, bool bInsert, bool bBefore)
		{
			CTmaxParameters tmaxParameters = null;
			CTmaxItem		tmaxAdd = null;
			CTmaxItem		tmaxParent = null;
			CTmaxItem		tmaxInsert = null;
			
			//	Are we inserting?
			if(bInsert == true)
			{
				Debug.Assert(tmaxNode != null);
				if(tmaxNode == null) return;
				
				//	Get an item to represent the parent
				if(tmaxNode.Parent != null)
				{
					tmaxParent = GetCommandItem((CTmaxMediaTreeNode)tmaxNode.Parent, false);
				}
				else
				{
					tmaxParent = new CTmaxItem();
					tmaxParent.DataType  = TmaxDataTypes.Binder;
					tmaxParent.MediaType = TmaxMediaTypes.Unknown;
				}
				
				//	Store the insertion node
				tmaxInsert = GetCommandItem(tmaxNode, false);	
			}
			else
			{
				if(tmaxNode!= null)
				{
					tmaxParent = GetCommandItem(tmaxNode, false);
				}
				else
				{
					tmaxParent = new CTmaxItem();
					tmaxParent.DataType = TmaxDataTypes.Binder;
					tmaxParent.MediaType = TmaxMediaTypes.Unknown;
				}
			}
			
			Debug.Assert(tmaxParent != null);
			
			//	Create the required parameters for the event
			tmaxParameters = new CTmaxParameters();
			tmaxParameters.Add(TmaxCommandParameters.Before, bBefore);
				
			//	Create an item to identify the new entry
			tmaxAdd = new CTmaxItem();
			tmaxAdd.MediaType = tmaxMediaType;
			tmaxAdd.DataType  = TmaxDataTypes.Binder;
			
			//	Construct the event item
			if(tmaxParent.SourceItems == null)
				tmaxParent.SourceItems = new CTmaxItems();
			tmaxParent.SourceItems.Add(tmaxAdd);
			
			if(tmaxInsert != null)
			{
				tmaxParent.SubItems.Add(tmaxInsert);
			}
				
			//	Fire the command to request the addition
			FireCommand(TmaxCommands.Add, tmaxParent, tmaxParameters);				
			
		}// protected override void OnCmdNew(TmaxMediaTypes tmaxMediaType, CTmaxMediaTreeNode tmaxNode, bool bInsert, bool bBefore)
		
		/// <summary>This method handles the event fired when the user clicks on one of the selections in the Import submenu</summary>
		/// <param name="tmaxNodes">The current node selections</param>
		/// <param name="eCommand">The command selected by the user</param>
		///	<returns>The return item provided by the database</returns>
		protected override CTmaxItem OnCmdImport(CTmaxMediaTreeNodes tmaxNodes, TreePaneCommands eCommand)
		{
			CTmaxItem		tmaxReturn = null;
			CTmaxItems		tmaxScripts = null;
			CDxBinderEntry	dxBinder = null;
			
			//	Let the base class add scripts or binders
			if((tmaxReturn = base.OnCmdImport(tmaxNodes, eCommand)) == null)
				return null;
				
			//	Nothing more to do if importing binders
			if(eCommand == TreePaneCommands.ImportAsciiBinders) return tmaxReturn;
			if(eCommand == TreePaneCommands.ImportXmlBinders) return tmaxReturn;

			//	Nothing more unless a single node is selected
			if(tmaxNodes == null) return tmaxReturn;
			if(tmaxNodes.Count != 1) return tmaxReturn;
			
			//	This should be a binder node
			if((dxBinder = (CDxBinderEntry)(tmaxNodes[0].IBinder)) == null)
				return tmaxReturn;
				
			//	Create a collection to add new scripts to this binder
			tmaxScripts = new CTmaxItems();
						
			//	Were any records added to the database
			if((tmaxReturn.SubItems != null) && (tmaxReturn.SubItems.Count > 0))
			{
				//	Get the scripts that have been added
				foreach(CTmaxItem O in tmaxReturn.SubItems)
				{
					//	Is this a media record that has been added?
					if(O.GetMediaRecord() != null)
					{
						if(O.MediaType == TmaxMediaTypes.Script)
							tmaxScripts.Add(new CTmaxItem(O.GetMediaRecord()));
					}
					//	This could be a root level parent item 
					else
					{
						//	Check each of the subitems in the parent item collection
						foreach(CTmaxItem S in O.SubItems)
						{
							if((S.GetMediaRecord() != null) && (S.MediaType == TmaxMediaTypes.Script))
								tmaxScripts.Add(new CTmaxItem(S.GetMediaRecord()));
						}
								
					}// if(O.GetMediaRecord() != null)
						
				}// foreach(CTmaxItem O in tmaxReturn.SubItems)
					
			}// if((tmaxReturn.SubItems != null) && (tmaxReturn.SubItems.Count > 0))
			
			//	Were any existing scripts updated by the operation?
			if((tmaxReturn.SourceItems != null) && (tmaxReturn.SourceItems.Count > 0))
			{
				//	We need to search because we don't want to duplicate 
				if(dxBinder.Contents.Count == 0)
					dxBinder.Fill();
					
				//	Get the scripts that have been updated
				foreach(CTmaxItem O in tmaxReturn.SourceItems)
				{
					//	Is this item bound to a script?
					if(O.GetMediaRecord() != null)
					{
						if(O.MediaType == TmaxMediaTypes.Script)
						{
							if(CheckContents(dxBinder, (CDxMediaRecord)(O.GetMediaRecord()), false) == false)
								tmaxScripts.Add(new CTmaxItem(O.GetMediaRecord()));
						}
					}
					else
					{
						foreach(CTmaxItem S in O.SubItems)
						{
							if((S.GetMediaRecord() != null) && (S.MediaType == TmaxMediaTypes.Script))
							{
								if(CheckContents(dxBinder, (CDxMediaRecord)(S.GetMediaRecord()), false) == false)
									tmaxScripts.Add(new CTmaxItem(S.GetMediaRecord()));
							}
						}
								
					}// if(O.GetMediaRecord() != null)
						
				}// foreach(CTmaxItem O in tmaxReturn.SourceItems)
					
			}// if((tmaxReturn.SourceItems != null) && (tmaxReturn.SourceItems.Count > 0))
			
			if((tmaxScripts != null) && (tmaxScripts.Count > 0))
			{
				Add(tmaxNodes[0], tmaxScripts, false, false);
				tmaxScripts.Clear();
			}

			return tmaxReturn;
			

		}// protected override CTmaxItem OnCmdImport(CTmaxMediaTreeNodes tmaxNodes, TreePaneCommands eCommand))
		
		/// <summary>This method handles the event fired when the user clicks on Duplicate from the context menu</summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		///	<returns>An event item that represents the return result</returns>
		protected override CTmaxItem OnCmdDuplicate(CTmaxMediaTreeNode tmaxNode)
		{
			CTmaxItem	tmaxScript = null;
			CTmaxItems	tmaxAdd = null;
			
			//	Let the base class add the script
			if((tmaxScript = base.OnCmdDuplicate(tmaxNode)) != null)
			{
				if(tmaxScript.IPrimary != null)
				{
					//	Add a binder entry to reference the duplicate
					tmaxAdd = new CTmaxItems();
					tmaxAdd.Add(tmaxScript);
					Add(tmaxNode, tmaxAdd, true, false);
				}
				
			}// if((tmaxScript = base.OnCmdDuplicate(tmaxNode)) != null)
			
			return tmaxScript;
			
		}// protected override CTmaxItem OnCmdDuplicate(CTmaxMediaTreeNode tmaxNode)
				
		/// <summary>This method handles the event fired when the user clicks on Merge Scripts from the context menu</summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		protected override CTmaxItem OnCmdMergeScripts(CTmaxMediaTreeNodes tmaxNodes)
		{
			CTmaxItem	tmaxScript = null;
			CTmaxItems	tmaxAdd = null;
			
			//	Let the base class add the script
			if((tmaxScript = base.OnCmdMergeScripts(tmaxNodes)) != null)
			{
				if(tmaxScript.IPrimary != null)
				{
					//	Add a binder entry to reference the new script
					tmaxAdd = new CTmaxItems();
					tmaxAdd.Add(tmaxScript);
					Add(tmaxNodes[tmaxNodes.Count - 1], tmaxAdd, true, false);
				}
				
			}// if((tmaxScript = base.OnCmdDuplicate(tmaxNode)) != null)
			
			return tmaxScript;
			
		}// OnCmdMergeScripts(CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This function is called when the value of the Database property changes</summary>
		protected override void OnDatabaseChanged()
		{
			//	Do the base class processing first
			base.OnDatabaseChanged();
			
			if((m_tmaxTreeCtrl != null) && (m_tmaxTreeCtrl.Nodes != null))
			{
				//	Prevent processing of Paint events until we are done
				m_tmaxTreeCtrl.BeginUpdate();
				
				//	Display the wait cursor
				Cursor.Current = Cursors.WaitCursor;
				
				//	Clear the existing tree
				m_tmaxTreeCtrl.Nodes.Clear();
				
				//	Do we have a valid database
				if((m_tmaxDatabase != null) && (m_tmaxDatabase.Binders != null))
				{
					//	Create the nodes to be added to tree
					foreach(CDxBinderEntry O in m_tmaxDatabase.Binders)
					{
						Add(m_tmaxTreeCtrl.Nodes, O);
						
					}// foreach(CDxPrimary dxPrimary in m_tmaxDatabase.Primaries)
					
				}// if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null))
				
				//	Reenable Paint events
				m_tmaxTreeCtrl.EndUpdate();
				
				//	Restore the cursor
				Cursor.Current = Cursors.Default;
				
			}// if((m_tmaxTreeCtrl != null) && (m_tmaxTreeCtrl.Nodes != null))
			
			//	This shouldn't be necessary but we'll do it just in case...
			m_bAddFromPresentation = false;
            foreach (CTmaxMediaTreeNode tmaxChild in m_tmaxTreeCtrl.Nodes)
            {
                PopulateChildNodes(tmaxChild);
                //Sort(tmaxChild.Nodes, false);
                SortCompleteTree(tmaxChild.Nodes);
            }

		}

        private void SortCompleteTree(TreeNodesCollection aNodes)
        {
            try
            {
                CTmaxMediaTreeNodes m_tmaxSorter = null;
                    m_tmaxSorter = m_tmaxDisplaySorter;
               
                bool bSelected = false;

                Debug.Assert(aNodes != null);
                Debug.Assert(m_tmaxSorter != null);
                if (aNodes == null) return;
                if (m_tmaxSorter == null) return;

                //	Put each of the children into the sorter
                foreach (CTmaxMediaTreeNode tmaxChild in aNodes)
                {
                    ITmaxMediaRecord record = tmaxChild.GetTmaxRecord(true);
                    if (record != null)
                    {
                        m_tmaxSorter.Add(tmaxChild);
                    }
                    else
                    {
                        SortCompleteTree(tmaxChild.Nodes);
                    }

                }

                if (m_tmaxSorter.Count == 0) return;
                //	Sort the nodes
                m_tmaxSorter.Sort(true);



                for (int i = 0; i < m_tmaxSorter.Count; i++)
                {
                    //	Is this object out of position?
                    if (ReferenceEquals(m_tmaxSorter[i], aNodes[i]) == false)
                    {
                        try
                        {
                            bSelected = m_tmaxSorter[i].Selected;

                            m_tmaxSorter[i].Reposition(aNodes[i], NodePosition.Previous);



                            if (bSelected)
                                m_tmaxSorter[i].Selected = true;
                        }
                        catch
                        {
                        }

                    }

                }

                //	Empty out the sorter
                m_tmaxSorter.Clear();

            }
            catch (Exception ex)
            {

            }
        }

        private void PopulateChildNodes(CTmaxMediaTreeNode aNode)
        {
            if (m_tmaxTreeCtrl.Nodes != null)
            {
                if (aNode.IBinder != null && aNode.IPrimary==null)
                    Fill(aNode);
                foreach (CTmaxMediaTreeNode tmaxChild in aNode.Nodes)
                {
                    if (tmaxChild.IBinder != null && tmaxChild.IPrimary == null)
                        PopulateChildNodes(tmaxChild);
                }
            }
        }
		
		/// <summary>This method is called by the application when the target binder changes</summary>
		/// <param name="dxOldTarget">The previous target binder</param>
		/// <param name="dxNewTarget">The new target binder</param>
		public override void OnTargetBinderChanged(CDxBinderEntry dxOldTarget, CDxBinderEntry dxNewTarget)
		{
			CTmaxMediaTreeNode tmaxNode = null;

			//	Refresh the node images
			if(dxOldTarget != null)
			{
				if((tmaxNode = GetBinderNode(null, dxOldTarget, false)) != null)
				{
					tmaxNode.Override.NodeAppearance.Image = GetImageIndex(tmaxNode);
				}
			}
			
			if(dxNewTarget != null)
			{
				if((tmaxNode = GetBinderNode(null, dxNewTarget, false)) != null)
				{
					tmaxNode.Override.NodeAppearance.Image = GetImageIndex(tmaxNode);
				}
			}

		}// public override void OnTargetBinderChanged(CDxBinderEntry dxOldTarget, CDxBinderEntry dxNewTarget)
		
		/// <summary>This method is called to populate the child collection of the specified node</summary>
		/// <param name="tmaxNode">The node to be filled</param>
		/// <returns>true if successful</returns>
		protected override bool Fill(CTmaxMediaTreeNode tmaxNode)
		{
			CDxBinderEntry dxBinder = null;
			
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

			//	Is this a pure binder?
			if((tmaxNode.IBinder != null) && (tmaxNode.IPrimary == null))
			{
				dxBinder = (CDxBinderEntry)tmaxNode.IBinder;
				
				//	Do we need to fill the child collection?
				if((dxBinder.Contents == null) || (dxBinder.Contents.Count == 0))
				{
					dxBinder.Fill();
				}
				
				//	Add a node for each child
				if(dxBinder.Contents != null)
				{
					foreach(CDxBinderEntry O in dxBinder.Contents)
					{
						Add(tmaxNode.Nodes, O);
					}
				}
				
				//	Make sure the check state matches the parent if using check boxes
				if(m_bCheckBoxes == true)
				{
					foreach(CTmaxMediaTreeNode O in tmaxNode.Nodes)
						O.CheckedState = tmaxNode.CheckedState;
				}
				
				return true;	
			}
			else
			{
				//	Let the base class do the work
				return base.Fill(tmaxNode);
			}
			
		}// protected override bool Fill(CTmaxMediaTreeNode tmaxNode)
			
		/// <summary>This method is called to determine if the specified command should be visible</summary>
		/// <param name="eCommand">The tree pane command enumeration</param>
		/// <param name="tmaxNodes">The current node selections</param>
		/// <returns>true if command should be visible</returns>
		protected override bool GetCommandVisible(TreePaneCommands eCommand, CTmaxMediaTreeNodes tmaxNodes)
		{
			switch(eCommand)
			{
				case TreePaneCommands.ExpandAll:

					//	We'll put this back when we know how it's supposed to behave
					return false;
					
				//	These are available only in the media tree
				case TreePaneCommands.RefreshSuperNodes:
				
					return false;
					
				//	These are available only in the binder tree
				case TreePaneCommands.ExportXmlBinder:
				case TreePaneCommands.ImportXmlBinders:
				case TreePaneCommands.ImportAsciiBinders:
				case TreePaneCommands.SetTargetBinder:
				
					return true;
					
				case TreePaneCommands.New:
				
					//	User must have a single  media node selected
					if(tmaxNodes == null) return false;
					if(tmaxNodes.Count != 1) return false;
					if(tmaxNodes[0].GetMediaRecord() == null) return false;
					
					//	Display this command if the script command is not visible
					return (GetCommandVisible(TreePaneCommands.ScriptNewMenu, tmaxNodes) == false);
				
				case TreePaneCommands.ScriptNewMenu:
				case TreePaneCommands.ScriptNewDesignations:
				case TreePaneCommands.ScriptNewClips:
				case TreePaneCommands.ScriptNewBarcodes:
	
					//	User must have a single node selected
					if(tmaxNodes == null) return false;
					if(tmaxNodes.Count != 1) return false;
					if(tmaxNodes[0].GetMediaRecord() == null) return false;
					
					//	Can only add to a script
					if(tmaxNodes[0].GetMediaRecord().GetMediaType() == TmaxMediaTypes.Script) return true;
					
					//	It's going to wind up being disabled but this is the menu
					//	we want displayed if the user is on a scene that is within
					//	a script
					if((tmaxNodes[0].GetMediaRecord().GetMediaType() == TmaxMediaTypes.Scene) ||
					   (tmaxNodes[0].GetMediaRecord().GetMediaType() == TmaxMediaTypes.Link))
					{
						if(tmaxNodes[0].IBinder == null) return true;
					}
					
					return false;
				
				case TreePaneCommands.NewMenu:
				
					//	Allow user to add a node to the root
					if(tmaxNodes == null) return true;
					if(tmaxNodes.Count == 0) return true;
					
					//	If there is a selection it can only be
					//	one node
					if(tmaxNodes.Count != 1) return false;
					
					//	Must not be a media node
					if(tmaxNodes[0].GetMediaRecord() != null) return false;
					
					return true;
				
				case TreePaneCommands.InsertBefore:
				case TreePaneCommands.InsertAfter:
				
					//	User must have a single media child node selected
					if(tmaxNodes == null) return false;
					if(tmaxNodes.Count == 0) return false;
					if(tmaxNodes[0].GetMediaRecord() == null) return false;
					if(tmaxNodes[0].GetMediaRecord().GetParent() == null) return false;

					//	This can not be a secondary record stored in a binder
					if(tmaxNodes[0].IBinder != null) return false;
					
					//	Parent must be a document or recording
					if((tmaxNodes[0].GetMediaRecord().GetParent().GetMediaType() != TmaxMediaTypes.Document) &&
					   (tmaxNodes[0].GetMediaRecord().GetParent().GetMediaType() != TmaxMediaTypes.Recording)) 
						return false;
					   
					return true;
					
				case TreePaneCommands.ScriptBeforeMenu:
				case TreePaneCommands.ScriptBeforeDesignations:
				case TreePaneCommands.ScriptBeforeClips:
				case TreePaneCommands.ScriptBeforeBarcodes:

				case TreePaneCommands.ScriptAfterMenu:
				case TreePaneCommands.ScriptAfterDesignations:
				case TreePaneCommands.ScriptAfterClips:
				case TreePaneCommands.ScriptAfterBarcodes:

					//	User must have a single scene node selected
					if(tmaxNodes.Count != 1) return false;
					if(tmaxNodes[0].GetMediaRecord() == null) return false;
					if(tmaxNodes[0].GetMediaRecord().GetMediaType() != TmaxMediaTypes.Scene) return false;
					
					//	It can not be a scene that's been placed in a binder
					if(tmaxNodes[0].IBinder != null) return false;
					
					return true;
				
				case TreePaneCommands.InsertBeforeMenu:
				case TreePaneCommands.InsertBinderBefore:
				case TreePaneCommands.InsertDocumentBefore:
				case TreePaneCommands.InsertRecordingBefore:
				case TreePaneCommands.InsertScriptBefore:
				
					//	One or the other must be visible
					return ((GetCommandVisible(TreePaneCommands.InsertBefore, tmaxNodes) == false) &&
					        (GetCommandVisible(TreePaneCommands.ScriptBeforeMenu, tmaxNodes) == false));
				
				case TreePaneCommands.InsertAfterMenu:
				case TreePaneCommands.InsertBinderAfter:
				case TreePaneCommands.InsertDocumentAfter:
				case TreePaneCommands.InsertRecordingAfter:
				case TreePaneCommands.InsertScriptAfter:
					
					//	One or the other must be visible
					return ((GetCommandVisible(TreePaneCommands.InsertAfter, tmaxNodes) == false) &&
						    (GetCommandVisible(TreePaneCommands.ScriptAfterMenu, tmaxNodes) == false));
				
				default:
			
					return base.GetCommandVisible(eCommand, tmaxNodes);
			}
			
		}// protected override bool GetCommandVisible(TreePaneCommands eCommand, CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method is called to determine if the specified command should be enabled</summary>
		/// <param name="eCommand">The tree pane command enumeration</param>
		/// <param name="tmaxNodes">The current node selections</param>
		/// <returns>true if command should be visible</returns>
		protected override bool GetCommandEnabled(TreePaneCommands eCommand, CTmaxMediaTreeNodes tmaxNodes)
		{
			//	Only thing we can do without an active database is set the preferences
			if(eCommand == TreePaneCommands.Preferences) return true;
			
			//	The database MUST be open for all other commands
			if(m_tmaxDatabase == null) return false;
			if(m_tmaxDatabase.Primaries == null) return false;
					
			//	Selections don't matter for these commands
			if(eCommand == TreePaneCommands.NewBinder) return true;
			if(eCommand == TreePaneCommands.NewMenu) return true;
			
			switch(eCommand)
			{
				case TreePaneCommands.SetTargetBinder:
				
					//	Must be a single binder selected
					if(tmaxNodes.Count != 1) return false;
					if(tmaxNodes[0].IPrimary != null) return false;
					if(tmaxNodes[0].IBinder == null) return false;
					if(m_tmaxDatabase == null) return false;
					if(ReferenceEquals(m_tmaxDatabase.TargetBinder, tmaxNodes[0].IBinder) == true) return false;
					
					return true;
					
				case TreePaneCommands.Properties:
				
					return (tmaxNodes.Count == 1);
					
				case TreePaneCommands.New:
				
					if(tmaxNodes.Count == 1)
					{
						//	Must be document or recording
						if(tmaxNodes[0].MediaType == TmaxMediaTypes.Document) return true;
						if(tmaxNodes[0].MediaType == TmaxMediaTypes.Recording) return true;
					}
					
					return false;
				
				case TreePaneCommands.Delete:
				
					return (tmaxNodes.Count > 0);
					
				case TreePaneCommands.NewDocument:
				case TreePaneCommands.NewRecording:
				case TreePaneCommands.NewScript:
				case TreePaneCommands.NewBarcodes:
				
					//	Must be a single binder selected
					if(tmaxNodes.Count != 1) return false;
					if(tmaxNodes[0].IBinder == null) return false;
					if(tmaxNodes[0].IPrimary != null) return false;
					
					//	OK to create new media in the selected binder
					return true;
				
				case TreePaneCommands.InsertBeforeMenu:
				case TreePaneCommands.InsertBinderBefore:
				case TreePaneCommands.InsertDocumentBefore:
				case TreePaneCommands.InsertRecordingBefore:
				case TreePaneCommands.InsertScriptBefore:
				case TreePaneCommands.InsertBarcodesBefore:
				
				case TreePaneCommands.InsertAfterMenu:
				case TreePaneCommands.InsertBinderAfter:
				case TreePaneCommands.InsertDocumentAfter:
				case TreePaneCommands.InsertRecordingAfter:
				case TreePaneCommands.InsertScriptAfter:
				case TreePaneCommands.InsertBarcodesAfter:
				
					//	Must be one node selected
					if(tmaxNodes.Count != 1) return false;
					
					//	Is the current selection a root node?
					if(tmaxNodes[0].Parent == null)
					{
						//	Can only create binders in the root
						if(eCommand == TreePaneCommands.InsertBeforeMenu) return true;
						if(eCommand == TreePaneCommands.InsertBinderBefore) return true;
						if(eCommand == TreePaneCommands.InsertAfterMenu) return true;
						if(eCommand == TreePaneCommands.InsertBinderAfter) return true;
						
						return false;
					}
					
					//	The parent node must be a binder
					if(((CTmaxMediaTreeNode)tmaxNodes[0].Parent).MediaType != TmaxMediaTypes.Unknown) return false;
					if(((CTmaxMediaTreeNode)tmaxNodes[0].Parent).IBinder == null) return false;
					
					//	Enable the commands
					return true;
					
				case TreePaneCommands.Copy:
				
					//	Must be something selected
					if((tmaxNodes == null) || (tmaxNodes.Count == 0)) return false;
					
					//	Allow copying of binders to the clipboard
					foreach(CTmaxMediaTreeNode tmaxNode in tmaxNodes)
					{
						if(tmaxNode.GetTmaxRecord(false) == null)
							return false;
					}
					
					//	All nodes OK
					return true;						
				
				case TreePaneCommands.Paste:
	
					//	Can't deal with multiple selections
					if(tmaxNodes.Count != 1) return false;
					
					//	Must be something in the clipboard
					if(m_tmaxClipboard == null) return false;
					if(m_tmaxClipboard.Count == 0) return false;
					
					//	Is the selected node a media node?
					if(tmaxNodes[0].GetMediaRecord() != null)
					{
						//	Let the base class handle the request
						return base.GetCommandEnabled(eCommand, tmaxNodes);
					}
					else
					{
						//	OK to paste anything we want into a binder
						return true;
					}
					
				case TreePaneCommands.PasteBefore:
				case TreePaneCommands.PasteAfter:
	
					//	Can't deal with multiple selections
					if(tmaxNodes.Count != 1) return false;
					
					//	Must be something in the clipboard
					if(m_tmaxClipboard == null) return false;
					if(m_tmaxClipboard.Count == 0) return false;
					
					//	Is this a root node?
					if(tmaxNodes[0].Parent == null)
					{
						//	We only allow binders at the root level
						foreach(CTmaxItem O in m_tmaxClipboard)
						{
							if(O.GetMediaRecord() != null)
								return false;
						}
						
						//	All top-level items must be binders
						return true;
					}
					else
					{
						//	Is the parent node a binder?
						if(((CTmaxMediaTreeNode)tmaxNodes[0].Parent).GetMediaRecord() == null)
						{
							//	Paste anything we want into a binder
							return true;
						}
						else
						{
							//	Leave it up to the base class since the parent is
							//	a media node
							return base.GetCommandEnabled(eCommand, tmaxNodes);
						}
						
					}// if(tmaxNodes[0].Parent == null)
					
				case TreePaneCommands.MoveUp:
				case TreePaneCommands.MoveDown:

					//	Must be only one node selected
					if((tmaxNodes == null) || (tmaxNodes.Count != 1)) 
					{
						return false;
					}
				
					if(eCommand == TreePaneCommands.MoveUp)
					{
						if(tmaxNodes[0].HasSibling(NodePosition.Previous) == false)
							return false;
					}
					else
					{
						if(tmaxNodes[0].HasSibling(NodePosition.Next) == false)
							return false;
					}
				
					//	Is the parent a binder?
					if((tmaxNodes[0].Parent == null) ||
					   (((CTmaxMediaTreeNode)tmaxNodes[0].Parent).IPrimary == null))
					{
						return true;
					}
					else
					{
						//	Must be secondary or lower media
						if(tmaxNodes[0].MediaLevel == TmaxMediaLevels.None) 
							return false;
						if(tmaxNodes[0].MediaLevel == TmaxMediaLevels.Primary) 
							return false;
						
						//	Must not be composite media
						if(CTmaxMediaTypes.IsCompositeMedia(tmaxNodes[0].MediaType) == true) 
							return false;
							
						return true;
					}
					
				case TreePaneCommands.ImportMenu:

					return (GetCommandEnabled(TreePaneCommands.ImportAsciiBinders, tmaxNodes) ||
						    GetCommandEnabled(TreePaneCommands.ImportXmlBinders, tmaxNodes) ||
							GetCommandEnabled(TreePaneCommands.ImportAsciiScripts, tmaxNodes) ||
							GetCommandEnabled(TreePaneCommands.ImportXmlScripts, tmaxNodes));

					
				case TreePaneCommands.ImportXmlScripts:
				case TreePaneCommands.ImportAsciiScripts:
				case TreePaneCommands.ImportAsciiBinders:
				case TreePaneCommands.ImportXmlBinders:
				
					//	Only binders can be added to the root
					if((tmaxNodes == null) || (tmaxNodes.Count == 0))
					{ 
						if(eCommand == TreePaneCommands.ImportAsciiBinders) return true;
						if(eCommand == TreePaneCommands.ImportXmlBinders) return true;
						else return false;
					}
					
					//	Target must be a single binder
					if(tmaxNodes.Count > 1) return false;
					if((tmaxNodes[0]).GetMediaRecord() != null) return false;
					
					return true;
				
				default:
			
					return base.GetCommandEnabled(eCommand, tmaxNodes);
			}
			
		}// protected override bool GetCommandEnabled(TreePaneCommands eCommand, CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method handles is called to add new items to the specified media record</summary>
		/// <param name="tmaxTarget">The node under the current mouse position</param>
		/// <param name="tmaxSource">The collection of items that represent the records to add</param>
		/// <param name="bInsert">true if inserting records relative to the target, false if adding to the target</param>
		/// <param name="bBefore">true to insert before the specified target node</param>
		/// <returns>True if the command event gets fired</returns>
		protected override bool Add(CTmaxMediaTreeNode tmaxTarget, CTmaxItems tmaxSource, bool bInsert, bool bBefore)
		{
			CTmaxParameters	tmaxParameters = null;
			CTmaxItem		tmaxParent = null;
			
			//	Make sure we have the required objects
			Debug.Assert(tmaxSource != null, "Add() invalid param: tmaxSource");
			if(tmaxSource == null) return false;
			
			//	Get the parent node for the operation
			if((tmaxParent = GetParentFromTarget(tmaxTarget, (bInsert == false))) == null)
			{
				//	Let the base class handle the request
				//
				//	NOTE:	GetParentFromTarget() returns NULL if the parent node turns
				//			out to be a valid media node
				return base.Add(tmaxTarget, tmaxSource, bInsert, bBefore);
			}
			
			//	Parent should be a binder
			Debug.Assert(tmaxParent.DataType == TmaxDataTypes.Binder);
			Debug.Assert(tmaxParent.MediaType == TmaxMediaTypes.Unknown);
				
			//	Create the required parameters for the event
			tmaxParameters = new CTmaxParameters();
			tmaxParameters.Add(TmaxCommandParameters.Activate, true);
				
			//	Build the collection of source items
			FillSourceCollection(tmaxParent, tmaxSource, tmaxParent.IBinderEntry != null);
			
			//	Add the insertion point
			if((bInsert == true) && (tmaxTarget != null))
			{
				tmaxParent.SubItems.Add(GetCommandItem(tmaxTarget, false));
				tmaxParameters.Add(TmaxCommandParameters.Before, bBefore);
			}
				
			//	Do we have anything to add?
			if((tmaxParent.SourceItems != null) && (tmaxParent.SourceItems.Count > 0))
			{
				//	Fire the command to request the addition
				FireCommand(TmaxCommands.Add, tmaxParent, tmaxParameters);
				return true;	
			}
			else
			{
				return false;
			}			
				
		}// protected override bool Add(CTmaxMediaTreeNode tmaxTarget, CTmaxItems tmaxSource, bool bInsert, bool bBefore)
		
		/// <param name="tmaxTarget">The node under the current mouse position</param>
		/// <param name="tmaxSource">The collection of items that represent the nodes to be moved</param>
		/// <param name="bInto">true if moving records into the target, false if moving relative to the target</param>
		/// <param name="bBefore">true to move the nodes into position before the specified target node</param>
		/// <returns>True if the command event gets fired</returns>
		protected override bool MoveTo(CTmaxMediaTreeNode tmaxTarget, CTmaxItems tmaxSource, bool bInto, bool bBefore)
		{
			CTmaxParameters	tmaxParameters = null;
			CTmaxItem		tmaxParent = null;
			
			//	Make sure we have the required objects
			Debug.Assert(tmaxSource != null, "Reposition() invalid param: tmaxSource");
			if(tmaxSource == null) return false;
			
			//	Get the parent node for the operation
			if((tmaxParent = GetParentFromTarget(tmaxTarget, bInto)) == null)
			{
				//	Let the base class handle the request
				//
				//	NOTE:	GetParentFromTarget() returns NULL if the parent node turns
				//			out to be a valid media node
				return base.MoveTo(tmaxTarget, tmaxSource, bInto, bBefore);
			}
			
			//	Parent should be a binder
			Debug.Assert(tmaxParent.DataType == TmaxDataTypes.Binder);
			Debug.Assert(tmaxParent.MediaType == TmaxMediaTypes.Unknown);
				
			//	Create the required parameters for the event
			tmaxParameters = new CTmaxParameters();
			tmaxParameters.Add(TmaxCommandParameters.Activate, true);
				
			//	Build the collection of source items
			FillSourceCollection(tmaxParent, tmaxSource, tmaxParent.IBinderEntry != null);
			
			//	Add the reference point
			if((bInto == false) && (tmaxTarget != null))
			{
				tmaxParent.SubItems.Add(GetCommandItem(tmaxTarget, false));
				tmaxParameters.Add(TmaxCommandParameters.Before, bBefore);
			}
				
			//	Do we have anything to move?
			if((tmaxParent.SourceItems != null) && (tmaxParent.SourceItems.Count > 0))
			{
				//	Fire the command to request repositioning of the binders
				FireCommand(TmaxCommands.Move, tmaxParent, tmaxParameters);	
				return true;	
			}
			else
			{
				return false;
			}			
				
		}// protected override bool MoveTo(CTmaxMediaTreeNode tmaxTarget, CTmaxItems tmaxSource, bool bInto, bool bBefore)
			
		/// <summary>This method builds the event item used for a drag operation from the list of selected nodes</summary>
		/// <param name="tmaxNodes">The collection containing the nodes to be dragged</param>
		/// <returns>The event item required to fire the drag event</returns>
		protected override CTmaxItem GetDragItem(CTmaxMediaTreeNodes tmaxNodes)
		{
			CTmaxItem tmaxParent = null;
			
			//	Create the command item used to define the parent item
			if(tmaxNodes[0].Parent != null)
			{
				//	Is the parent a media node?
				if(((CTmaxMediaTreeNode)tmaxNodes[0].Parent).GetMediaRecord() != null)
				{
					//	Let the base class do it
					return base.GetDragItem(tmaxNodes);
				}
				else
				{
					tmaxParent = GetBinderItem(((CTmaxMediaTreeNode)(tmaxNodes[0].Parent)));
				}
			}
			else
			{
				//	Create a null item to contain the source items
				tmaxParent = new CTmaxItem();
				tmaxParent.DataType  = TmaxDataTypes.Binder;
				tmaxParent.MediaType = TmaxMediaTypes.Unknown;
			}
			
			//	Assign the source items
			tmaxParent.SourceItems = GetBinderItems(tmaxNodes);
				
			return tmaxParent;
			
		}// protected override CTmaxItem GetDragItem(CTmaxMediaTreeNodes tmaxNodes)

		/// <summary>Called to get the possible actions if the user drops at the current mouse position</summary>
		/// <returns>The appropriate drop action</returns>
		protected override TreeDropActions GetDropRecordsAction()
		{
			ITmaxMediaRecord	tmaxIDropParent = null;
			TreeDropActions		eAction = TreeDropActions.None;
						
			//	Must be dragging something
			if(m_tmaxDragData == null) return TreeDropActions.None;
			if(m_tmaxDragData.SourceItems == null) return TreeDropActions.None;
			if(m_tmaxDragData.SourceItems.Count == 0) return TreeDropActions.None;
			if(m_dropTarget.node == null) return TreeDropActions.None;
			
			//	Make sure we have the current drop position
			m_dropTarget.ePosition = m_tmaxTreeFilter.DropLinePosition;
			
			//	Get the interfaces to the drag/drop parent records
			if(m_dropTarget.node.Parent != null)
			{
				//	Try to get the media record first
				if((tmaxIDropParent = ((CTmaxMediaTreeNode)m_dropTarget.node.Parent).GetMediaRecord()) == null)
					tmaxIDropParent = ((CTmaxMediaTreeNode)m_dropTarget.node.Parent).IBinder;
			}
			
			//	Is the user attempting to drop in the root?
			if(tmaxIDropParent == null)
			{
				eAction = GetDropRecordsInRootAction();	
			}
			
			//	Is the user attempting to drop within a binder?
			else if(tmaxIDropParent.GetMediaType() == TmaxMediaTypes.Unknown)
			{
				eAction = GetDropRecordsInBinderAction();
			}

			//	Must be dropping within a media record
			else
			{
				//	Let the base class handle it
				eAction = base.GetDropRecordsAction();
			}

			return eAction;
			
		}// protected override TreeDropActions GetDropRecordsAction()
		
		/// <summary>This method will retrieve the appropriate drag/drop effects when dragging database records</summary>
		/// <returns>The appropriate drag drop effects</returns>
		protected override DragDropEffects GetDropRecordsEffects()
		{
			switch(m_dropTarget.eAction)
			{
				case TreeDropActions.Add:
				case TreeDropActions.InsertAfter:
				case TreeDropActions.InsertBefore:
				
					return DragDropEffects.Copy;
					
				case TreeDropActions.Reorder:
				case TreeDropActions.MoveInto:
				case TreeDropActions.MoveBefore:
				case TreeDropActions.MoveAfter:
				
					return DragDropEffects.Move;
					
				case TreeDropActions.None:
				default:
				
					return DragDropEffects.None;
			}
			
		}// protected override DragDropEffects GetDropRecordsEffects()
		
		/// <summary>Called to get the action to be performed if the user drops source files at the current location</summary>
		/// <returns>The action to be carried out when the user drops</returns>
		protected override TreeDropActions GetDropSourceAction()
		{
			//	Update the current line position
			m_dropTarget.ePosition = m_tmaxTreeFilter.DropLinePosition;
			
			//	Is the user dropping in the root?
			if(m_dropTarget.node == null)
			{
				return TreeDropActions.Add;
			}
			
			//	Is the user on top of a binder?
			//
			//	NOTE:	We don't have to check the parent type because the
			//			parent of a binder is always another binder or the root
			else if(m_dropTarget.node.GetMediaRecord() == null)
			{
				switch(m_dropTarget.ePosition)
				{
					case TmaxTreePositions.AboveNode:	return TreeDropActions.InsertBefore;
					case TmaxTreePositions.BelowNode:	return TreeDropActions.InsertAfter;
					case TmaxTreePositions.OnNode:		return TreeDropActions.Add;
					default:							return TreeDropActions.None;
				}
				
			}
			
			//	The drop node must be a media record
			else
			{
				//	Is this a media record?
				if(m_dropTarget.node.Parent != null)
				{
					//	Is the parent a binder?
					if(((CTmaxMediaTreeNode)m_dropTarget.node.Parent).GetMediaRecord() == null)
					{
						switch(m_dropTarget.ePosition)
						{
							case TmaxTreePositions.AboveNode:	return TreeDropActions.InsertBefore;
							case TmaxTreePositions.BelowNode:	return TreeDropActions.InsertAfter;
							case TmaxTreePositions.OnNode:		// Can't add source to a media node
							default:							return TreeDropActions.None;
						}
					
					}
					else
					{
						return TreeDropActions.None;
					}
					
				}
				else
				{
					return TreeDropActions.None;
				}
						
			}// if((m_dropTarget.node == null) || (m_dropTarget.node.GetMediaRecord() == null))
					
		}// protected override TreeDropActions GetDropSourceAction()
		
		///	<summary>This method will set the text for the specified node</summary>
		/// <param name="tmaxNode">The node who's text is to be set</param>
		/// <param name="bChildren">true to set the text of child nodes</param>
		protected override void SetText(CTmaxMediaTreeNode tmaxNode, bool bChildren)
		{
			//	Let the base class do the work if this is a media node
			if(tmaxNode.GetMediaRecord() != null)
			{
				base.SetText(tmaxNode, bChildren);
			}
			else if(tmaxNode.IBinder != null)
			{
				tmaxNode.Text = ((CDxBinderEntry)tmaxNode.IBinder).Name;
			
				if((bChildren == true) && (tmaxNode.Nodes != null))
				{
					foreach(CTmaxMediaTreeNode O in tmaxNode.Nodes)
						SetText(O, true);
				}
		
			}
			
		}//	protected override void SetText(CTmaxMediaTreeNode tmaxNode)
		
		/// <summary>This method is called to move the specified children to the new location</summary>
		/// <param name="tmaxChildren">Children to be moved</param>
		/// <param name="tmaxLocation">New location at which to place the children</param>
		/// <param name="bBefore">true if new position is before the specified location</param>
		protected override void Reorder(CTmaxItems tmaxChildren, CTmaxMediaTreeNode tmaxLocation, bool bBefore)
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
			if(tmaxChildren == null) return;
			if(tmaxChildren.Count == 0) return;
			
			//	If the parent is a media record let the base class handle
			//	the request
			if((tmaxLocation.Parent != null) && 
			   (((CTmaxMediaTreeNode)tmaxLocation.Parent).GetMediaRecord() != null))
			{
				base.Reorder(tmaxChildren, tmaxLocation, bBefore);
				return;
			}
			else
			{
				//	Create a command item for the parent node and request
				//	that items be created for each child as they appear in the
				//	tree right now
				tmaxParent = GetCommandItem((CTmaxMediaTreeNode)tmaxLocation.Parent, true);
			}
			
			//	Transfer the children to a local collection we can manipulate
			for(i = 0; i < tmaxChildren.Count; i++)
			{
				//	If the first node being moved is also the drop node
				//	do not copy it to the collection of nodes being moved
				if((i == 0) && (ReferenceEquals(tmaxChildren[0].IBinderEntry, tmaxLocation.IBinder) == true))
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
			
			//	Where is the target location in the tree?
			iInsert = tmaxParent.SubItems.IndexOfBinderEntry(tmaxLocation.IBinder);
			Debug.Assert(iInsert >= 0);
			if(iInsert < 0) return;
			
			//	Adjust the target location if the specified location is among
			//	those nodes being moved
			if(tmaxMoving.IndexOfBinderEntry(tmaxLocation.IBinder) >= 0)
			{
				//	Try moving down the collection in search of the first node not among
				//	those being moved
				for(i = iInsert + 1; i < tmaxParent.SubItems.Count; i++)
				{
					if(tmaxMoving.IndexOfBinderEntry(tmaxParent.SubItems[i].IBinderEntry) < 0)
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
						if(tmaxMoving.IndexOfBinderEntry(tmaxParent.SubItems[i].IBinderEntry) < 0)
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
				if((tmaxRemove = tmaxParent.SubItems.FindBinderEntry(tmaxMove.IBinderEntry)) != null)
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

		}// protected override void Reorder(CTmaxItems tmaxChildren, CTmaxMediaTreeNode tmaxLocation, bool bBefore)
		
		/// <summary>This method is called when the PauseThreshold value changes</summary>
		protected override void OnPauseThresholdChanged()
		{
			if((m_tmaxTreeCtrl != null) && (m_tmaxTreeCtrl.Nodes != null))
			{
				foreach(CTmaxMediaTreeNode O in m_tmaxTreeCtrl.Nodes)
					OnPauseThresholdChanged(O);
			}
				
		}// protected override void OnPauseThresholdChanged()
			
		/// <summary>This method is called when the user sets the preferences</summary>
		/// <param name="tmaxPreferences">The new set of preferences</param>
		protected override void OnPreferencesChanged(CFTreePreferences tmaxPreferences)
		{
			//	Display the wait cursor
			Cursor.Current = Cursors.WaitCursor;
			
			//	Suspend painting until we finish making changes
			m_tmaxTreeCtrl.BeginUpdate();

            try
            {
                if (tmaxPreferences.PrimaryTextMode != PrimaryTextMode)
                {
                    PrimaryTextMode = tmaxPreferences.PrimaryTextMode;
                    SetText(TmaxMediaLevels.Primary);
                }

                if (tmaxPreferences.SecondaryTextMode != SecondaryTextMode)
                {
                    SecondaryTextMode = tmaxPreferences.SecondaryTextMode;
                    SetText(TmaxMediaLevels.Secondary);
                }

                if (tmaxPreferences.TertiaryTextMode != TertiaryTextMode)
                {
                    TertiaryTextMode = tmaxPreferences.TertiaryTextMode;
                    SetText(TmaxMediaLevels.Tertiary);
                }

                if ((m_tmaxPrimarySorter != null) && !(ReferenceEquals(tmaxPreferences.PrimarySorter , null)))
                {
                    //	Update the sort options
                    m_tmaxPrimarySorter.Copy(tmaxPreferences.PrimarySorter);

                }

                // DisplayOrder sorting disturbed
                SortByDisplayOrder = false;
                if (m_tmaxTreeCtrl.Nodes != null && m_tmaxTreeCtrl.Nodes.Count > 0)
                {
                    foreach (CTmaxMediaTreeNode tmaxChild in m_tmaxTreeCtrl.SelectedNodes)
                    {
                        tmaxChild.Sorter = new CTmaxBaseTreeSorter();
                        tmaxChild.Sorter.Copy(m_tmaxPrimarySorter);
                        Sort(tmaxChild.Nodes, false);
                        SetDisplayOrder(tmaxChild);
                        SetSortProprety(tmaxChild);
                    }
                }

            }
            catch (System.Exception Ex)
            {
                m_tmaxEventSource.FireError(this, "OnPreferencesChanged", m_tmaxErrorBuilder.Message(ERROR_CHANGE_PREFERENCES_EX), Ex);
            }
			finally
			{		
				//	Restore the system cursor
				Cursor.Current = Cursors.Default;
				
				//	Resume painting
				m_tmaxTreeCtrl.EndUpdate();
			}
			
		}// protected override void OnPreferencesChanged(CFTreePreferences tmaxPreferences)

        protected override void OnCmdPreferences(CTmaxMediaTreeNode tmaxNode)
        {
            CFTreePreferences Preferences = null;

            try
            {
                Preferences = new CFTreePreferences();
                if (!ReferenceEquals(tmaxNode, null) && tmaxNode.IPrimary == null)
                    Preferences.BinderSorter = tmaxNode.Sorter;
                else
                   Preferences.IsSortEnable = false;
                //	Initialize the preferences dialog
                Preferences.Initialize(this);

                //	Open the dialog
                if (Preferences.ShowDialog() == DialogResult.OK)
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
                if (Preferences != null)
                    Preferences.Dispose();
            }

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
        }

        protected override void Sort(TreeNodesCollection aNodes, bool isDefualtSorter)
        {
            // isDefualtSorter is false when sort is calling from Tree Preference, rather than adding/moving of media
            if (isDefualtSorter)
                base.Sort(aNodes, isDefualtSorter);
            else
                SortBinder(aNodes, isDefualtSorter);
        }

	    private void SortBinder(TreeNodesCollection aNodes,bool isDefualtSorter)
	    {
            try
            {
                CTmaxMediaTreeNodes m_tmaxSorter = null;
                if (SortByDisplayOrder)
                    m_tmaxSorter = m_tmaxDisplaySorter;
                else
                {
                    m_tmaxSorter = new CTmaxMediaTreeNodes();
                    m_tmaxSorter.Sorter = m_tmaxPrimarySorter;
                    m_tmaxSorter.Sorter.CaseCode = m_tmaxCaseCodes.Find(m_tmaxPrimarySorter.CaseCodeId);
                }

                bool bSelected = false;

                Debug.Assert(aNodes != null);
                Debug.Assert(m_tmaxSorter != null);
                if (aNodes == null) return;
                if (m_tmaxSorter == null) return;

                //	Put each of the children into the sorter
                foreach (CTmaxMediaTreeNode tmaxChild in aNodes)
                {
                    ITmaxMediaRecord record = tmaxChild.GetTmaxRecord(true);
                    if (record != null)
                    {
                        m_tmaxSorter.Add(tmaxChild);
                    }
                    
                }

                if (m_tmaxSorter.Count == 0) return;
                //	Sort the nodes
                m_tmaxSorter.Sort(true);

                

                for (int i = 0; i < m_tmaxSorter.Count; i++)
                {
                   //	Is this object out of position?
                    if (ReferenceEquals(m_tmaxSorter[i], aNodes[i]) == false)
                    {
                        try
                        {
                            bSelected = m_tmaxSorter[i].Selected;

                            m_tmaxSorter[i].Reposition(aNodes[i], NodePosition.Previous);



                            if (bSelected)
                                m_tmaxSorter[i].Selected = true;
                        }
                        catch
                        {
                        }

                    }

                }

                //	Empty out the sorter
                m_tmaxSorter.Clear();
               
            }
            catch (Exception ex)
            {

            }
	    }

        private void SetDisplayOrder(CTmaxMediaTreeNode node)
        {
            int count = 0;
            foreach (CTmaxMediaTreeNode treeNode in node.Nodes)
            {
                
                long index = treeNode.IBinder.GetDisplayOrder();
               // if (index != count+1)
                {
                    ITmaxMediaRecord record = treeNode.GetTmaxRecord(true);
                    if (record != null)
                    {
                        count++;
                        CDxBinderEntry binder = (CDxBinderEntry) treeNode.IBinder;
                        binder.DisplayOrder = count;
                    }
                    else
                    {
                        count++;
                        CDxBinderEntry binder = (CDxBinderEntry)treeNode.IBinder;
                        binder.DisplayOrder = count;
                        SetDisplayOrder(treeNode);
                    }

                }
            }
            m_tmaxDatabase.ResetDisplayOrder((CDxBinderEntry)node.IBinder);

        }



		/// <summary>This method traps the MouseDown event</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">System mouse event arguments</param>
		protected override void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//	Is the user clicking in the whitespace?
			if(m_tmaxTreeCtrl.GetNode(e.X, e.Y) == null) 
			{
				//	Is this the right button?
				if(e.Button == MouseButtons.Right)
				{
					if((m_tmaxTreeCtrl.SelectedNodes != null) && (m_tmaxTreeCtrl.SelectedNodes.Count > 0))
					{
						m_tmaxTreeCtrl.SelectedNodes.Clear();
						m_tmaxTreeCtrl.ActiveNode = null;
					}
				
				}
				
			}// if((tmaxNode = m_tmaxTreeCtrl.GetNode(e.X, e.Y)) == null) 
				
			//	Pass it on to the base class
			base.OnMouseDown(sender, e);
			
		}// OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>This method is called to get an event item that represents the parent for the command event</summary>
		/// <param name="tmaxReferenceNode">The node that identifies the reference point for the operation</param>
		/// <param name="bIsParent">true if the reference node is the parent, false if it is a sibling</param>
		///	<returns>An event item to represent the parent for the operation</returns>
		private CTmaxItem GetParentFromTarget(CTmaxMediaTreeNode tmaxReferenceNode, bool bIsParent)
		{
			CTmaxItem tmaxParent = null;
			
			//	Adjust the reference node if it is supposed to be a sibling
			if(bIsParent == false)
			{
				if(tmaxReferenceNode != null)
					tmaxReferenceNode = (CTmaxMediaTreeNode)(tmaxReferenceNode.Parent);
				else
					return null; // This shouldn't happen
			}
			
			//	Only create the parent if the reference node is not a media record
			if((tmaxReferenceNode == null) || (tmaxReferenceNode.GetMediaRecord() == null))
			{
				if(tmaxReferenceNode != null)
				{
					tmaxParent = GetCommandItem(tmaxReferenceNode, false);
				}
				else
				{
					tmaxParent = new CTmaxItem();
					tmaxParent.DataType  = TmaxDataTypes.Binder;
					tmaxParent.MediaType = TmaxMediaTypes.Unknown;
				}
			
			}// if((tmaxReferenceNode == null) || (tmaxReferenceNode.GetMediaRecord() == null))

			return tmaxParent;	
				
		}// private CTmaxItem GetParentFromTarget(CTmaxMediaTreeNode tmaxReferenceNode, bool bIsParent)
			
		/// <summary>Fills the SourceItems collection of the specified target item</summary>
		/// <param name="tmaxOwner">The item that will owns the source collection to be filled</param>
		/// <param name="tmaxSource">The collection of items that represent the source nodes/records</param>
		/// <param name="bAllowMedia">true to allow media nodes to be added</param>
		/// <returns>True if successful</returns>
		private bool FillSourceCollection(CTmaxItem tmaxOwner, CTmaxItems tmaxSource, bool bAllowMedia)
		{
			CTmaxItem tmaxTransfer = null;
			
			//	Make sure we have the required objects
			Debug.Assert(tmaxOwner != null, "FillSourceCollection() invalid param: tmaxOwner");
			if(tmaxOwner == null) return false;
			Debug.Assert(tmaxSource != null, "FillSourceCollection() invalid param: tmaxSource");
			if(tmaxSource == null) return false;
			
			//	Make sure the owner has a valid source items collection
			if(tmaxOwner.SourceItems != null)
				tmaxOwner.SourceItems.Clear();
			else
				tmaxOwner.SourceItems = new CTmaxItems();
								
			//	Add each of the items in the source collection specified by the caller
			foreach(CTmaxItem O in tmaxSource)
			{
				//	Is this item using the subitems collection?
				if((O.SubItems != null) && (O.SubItems.Count > 0))
				{
					//	Transfer subitems to source items collection
					if((tmaxTransfer = O.SubToSource()) != null)
					{
						if((bAllowMedia == true) || (tmaxTransfer.GetMediaRecord() == null))
							tmaxOwner.SourceItems.Add(tmaxTransfer);
					
					}// if((tmaxTransfer = O.SubToSource()) != null)
				
				}
				else
				{
					if((bAllowMedia == true) || (O.GetMediaRecord() == null))
						tmaxOwner.SourceItems.Add(O);
				}
			
			}// foreach(CTmaxItem O in tmaxSource)
				
			return true;
							
		}// private bool FillSourceCollection(CTmaxItem tmaxOwner, CTmaxItems tmaxSource, bool bAllowMedia)
			
		///	<summary>This method will refresh all scene nodes that reference the specified source node</summary>
		/// <param name="tmaxParent">The parent node at which to statt looking for scenes</param>
		/// <param name="dxSource">The scene's source record interface</param>
		private void RefreshScenes(CTmaxMediaTreeNode tmaxParent, CDxMediaRecord dxSource)
		{
			CTmaxMediaTreeNodes tmaxScenes = new CTmaxMediaTreeNodes();
			
			//	Locate all scenes that match the source criteria
			GetScenesFromSource(tmaxParent, dxSource, tmaxScenes);
			
			//	Refresh each scene
			foreach(CTmaxMediaTreeNode O in tmaxScenes)
			{
				try
				{
					Refresh(O);
				}
				catch
				{
				}
				
			}
			
			tmaxScenes.Clear();
			
		}// private void RefreshScenes(CTmaxMediaTreeNode tmaxParent, CDxMediaRecord dxSource)
			
		/// <summary>This method builds a collection of binder items using the specified collection of nodes</summary>
		/// <param name="tmaxNodes">The nodes to be expanded</param>
		/// <returns>The collection of associated binder event items</returns>
		private CTmaxItems GetBinderItems(CTmaxMediaTreeNodes tmaxNodes)
		{
			CTmaxItems	tmaxItems = null;
			CTmaxItem	tmaxItem = null;
			
			try
			{
				tmaxItems = new CTmaxItems();
				
				foreach(CTmaxMediaTreeNode O in tmaxNodes)
				{
					if((tmaxItem = GetBinderItem(O)) != null)
						tmaxItems.Add(tmaxItem);
				}
				
			}
			catch
			{
				Debug.Assert(false);
			}
			
			return tmaxItems;
		
		}// private CTmaxItems GetBinderItems(CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method builds an event item by expanding all non-media binder entries</summary>
		/// <param name="tmaxNode">The node to be associated with the item</param>
		/// <returns>The expanded event item</returns>
		private CTmaxItem GetBinderItem(CTmaxMediaTreeNode tmaxNode)
		{
			if(tmaxNode.IBinder == null)
				return GetCommandItem(tmaxNode, false);
			else
				return GetBinderItem((CDxBinderEntry)tmaxNode.IBinder);		
		
		}// private CTmaxItem GetBinderItem(CTmaxMediaTreeNode tmaxNode)
		
		/// <summary>This method builds an event item by expanding all non-media binder entries</summary>
		/// <param name="dxBinder">The binder to be associated with the item</param>
		/// <returns>The expanded event item</returns>
		private CTmaxItem GetBinderItem(CDxBinderEntry dxBinder)
		{
			CTmaxItem	tmaxItem = null;
			CTmaxItem	tmaxChild = null;
			
			try
			{
				//	Create an item for this binder entry
				tmaxItem = new CTmaxItem(dxBinder.Source);
				tmaxItem.IBinderEntry = dxBinder;
				
				//	Drill down into the children if this is not a media object
				if(dxBinder.IsMedia() == false)
				{
					//	Does this binder's child collection need to be populated
					if((dxBinder.Contents == null) || (dxBinder.Contents.Count == 0))
					{
						dxBinder.Fill();
					}
					
					//	Now get the items for each child
					if((dxBinder.Contents != null) && (dxBinder.Contents.Count > 0))
					{
						foreach(CDxBinderEntry O in dxBinder.Contents)
						{
							if((tmaxChild = GetBinderItem(O)) != null)
								tmaxItem.SubItems.Add(tmaxChild);
						}
					}
				
				}
				
			}
			catch
			{
				Debug.Assert(false);
			}
		
			return tmaxItem;
		
		}// private CTmaxItem GetBinderItem(CDxBinderEntry dxBinder)
		
		/// <summary>This method is called to locate all nodes that reference the specified media record</summary>
		/// <param name="tmaxParent">The parent node to be checked</param>
		/// <param name="tmaxRecord">The desired media record</param>
		/// <param name="tmaxNodes">The collection in which to store the nodes</param>
		private void GetNodesFromRecord(CTmaxMediaTreeNode tmaxParent, ITmaxMediaRecord tmaxRecord, CTmaxMediaTreeNodes tmaxNodes)
		{
			TreeNodesCollection	aNodes = null;
			ITmaxMediaRecord	dxParent = null;
			TmaxMediaTypes		eParentType = TmaxMediaTypes.Unknown;
			
			//	Get the media record bound to the parent
			if(tmaxParent != null)
			{
				if((dxParent = tmaxParent.GetMediaRecord()) != null)
					eParentType = dxParent.GetMediaType();
			}

			//	Is the parent node a match?
			if((dxParent != null) && (ReferenceEquals(dxParent, tmaxRecord) == true))
			{
				tmaxNodes.Add(tmaxParent);
			}
			
			//	Is the source record a match?
			else if((eParentType == TmaxMediaTypes.Scene) && (ReferenceEquals(((CDxSecondary)dxParent).GetSource(), tmaxRecord) == true))
			{
				tmaxNodes.Add(tmaxParent);
			}
			else
			{
				//	What child collection should we use
				if(tmaxParent != null)
					aNodes = tmaxParent.Nodes;
				else
					aNodes = m_tmaxTreeCtrl.Nodes;
					
				Debug.Assert(aNodes != null);
				if(aNodes == null) return;
				
				foreach(CTmaxMediaTreeNode O in aNodes)
				{
					GetNodesFromRecord(O, tmaxRecord, tmaxNodes);
				}
				
			}

		}// private void GetNodesFromRecord(CTmaxMediaTreeNode tmaxParent, ITmaxMediaRecord tmaxRecord, CTmaxMediaTreeNodes tmaxNodes)
		
		///	<summary>This method will add all scene nodes to the specified collection</summary>
		/// <param name="tmaxParent">The top-level parent node</param>
		/// <param name="tmaxScenes">The collection in which to place the scene nodes</param>
		/// <returns>The total number of scenes placed in the collection</returns>
		private long GetScenes(CTmaxMediaTreeNode tmaxParent, CTmaxMediaTreeNodes tmaxScenes)
		{
			return GetScenesFromSource(tmaxParent, null, tmaxScenes);
		}
		
		///	<summary>This method will retrieve the collection of scenes that reference the specified source record</summary>
		/// <param name="tmaxParent">The top-level parent node</param>
		/// <param name="dxSource">The scene's source record interface</param>
		/// <param name="tmaxScenes">The collection in which to place the scene nodes</param>
		/// <returns>The total number of scenes placed in the collection</returns>
		private long GetScenesFromSource(CTmaxMediaTreeNode tmaxParent, CDxMediaRecord dxSource, CTmaxMediaTreeNodes tmaxScenes)
		{
			TreeNodesCollection aNodes = null;
			CDxSecondary dxScene = null;
			
			//	Is the parent a script?
			if((tmaxParent != null) && (tmaxParent.GetMediaRecord() != null) && 
			   (tmaxParent.MediaType == TmaxMediaTypes.Script))
			{
				//	Iterate each of the scenes
				foreach(CTmaxMediaTreeNode O in tmaxParent.Nodes)
				{
					//	Did the caller provide a record to compare?
					if(dxSource != null)
					{
						//	Get the record associated with this scene
						if((dxScene = (CDxSecondary)O.ISecondary) != null)
						{
							//	Does this scene reference the specified record
							if(ReferenceEquals(dxScene.GetSource(), dxSource) == true)
							{
								//	Add to the caller's collection
								tmaxScenes.Add(O);
							}
						}
					}
					else
					{
						//	Add the scene to the collection
						tmaxScenes.Add(O);
					
					}// if(dxSource != null)
				
				}// foreach(CTmaxMediaTreeNode O in tmaxParent.Nodes)
			
			}
			
			//	Drill down into the child collection if the parent is a binder
			else if((tmaxParent == null) || (tmaxParent.GetMediaRecord() == null))
			{
				//	What child collection should we use
				if(tmaxParent != null)
					aNodes = tmaxParent.Nodes;
				else
					aNodes = m_tmaxTreeCtrl.Nodes;
					
				Debug.Assert(aNodes != null);
				
				if(aNodes != null)
				{
					//	Drill down into the child collection to locate scenes that
					//	use the specified record
					foreach(CTmaxMediaTreeNode O in aNodes)
					{
						//	Is this node a scene?
						if((O.GetMediaRecord() != null) && (O.GetMediaRecord().GetMediaType() == TmaxMediaTypes.Scene))
						{
							//	Did the caller provide a record to compare?
							if(dxSource != null)
							{
								//	Get the record associated with this scene
								if((dxScene = ((CDxSecondary)(O.GetMediaRecord()))) != null)
								{
									//	Does this scene reference the specified record
									if(ReferenceEquals(dxScene.GetSource(), dxSource) == true)
									{
										//	Add to the caller's collection
										tmaxScenes.Add(O);
									}
								}
							}
							else
							{
								//	Add the scene to the collection
								tmaxScenes.Add(O);
					
							}// if(dxSource != null)
						
						}
						else
						{
							//	Recurse
							GetScenesFromSource(O, dxSource, tmaxScenes);
						}
					}
				
				}
			
			}
			
			return tmaxScenes.Count;
			
		}// private long GetScenesFromSource(CTmaxMediaTreeNode tmaxParent, CDxMediaRecord dxSource, CTmaxMediaTreeNodes tmaxScenes)
			
		/// <summary>This method is called to locate the node associated with the specified binder</summary>
		/// <param name="aNodes">The collection of nodes to be checked</param>
		/// <param name="dxBinder">The binder to be located</param>
		/// <param name="bFill">true to fill child collections as we go</param>
		/// <returns>the associated node if found</returns>
		private CTmaxMediaTreeNode GetBinderNode(CTmaxMediaTreeNode tmaxParent, CDxBinderEntry dxBinder, bool bFill)
		{
			CTmaxMediaTreeNode		tmaxBinder = null;
			TreeNodesCollection	aNodes = null;
			
			//	Do we need to fill this collection?
			if((tmaxParent != null) && (tmaxParent.Nodes != null) &&
			   (tmaxParent.Nodes.Count == 0) && (bFill == true))
			{
				Fill(tmaxParent);
			}
			
			//	What node collection should we use
			if(tmaxParent != null)
				aNodes = tmaxParent.Nodes;
			else
				aNodes = m_tmaxTreeCtrl.Nodes;
				
			Debug.Assert(aNodes != null);
			if(aNodes == null) return null;
			
			foreach(CTmaxMediaTreeNode O in aNodes)
			{
				if(O.IBinder != null)
				{
					if(ReferenceEquals(dxBinder, O.IBinder) == true)
					{
						tmaxBinder = O;
						break;
					}
					
				}
				
				//	Check the children
				if(O.Nodes != null)
				{
					if((tmaxBinder = GetBinderNode(O, dxBinder, bFill)) != null)
					{
						break;
					}
				}
				
			}
			
			return tmaxBinder;
		
		}// private CTmaxMediaTreeNode GetBinderNode(CTmaxMediaTreeNode tmaxParent, CDxBinderEntry dxBinder, bool bFill)
		
		/// <summary>This method is called to locate the node associated with the specified media record</summary>
		/// <param name="tmaxParent">The parent node</param>
		/// <param name="dxChild">The child to be located</param>
		/// <param name="bFill">true to fill child child collection if necessary</param>
		/// <returns>the associated node if found</returns>
		private CTmaxMediaTreeNode GetMediaNode(CTmaxMediaTreeNode tmaxParent, CDxMediaRecord dxChild, bool bFill)
		{
			CTmaxMediaTreeNode tmaxMedia = null;
			
			//	Can't have media nodes in the root
			Debug.Assert(tmaxParent != null);
			if(tmaxParent == null) return null;
			
			//	Do we need to fill this collection?
			if((tmaxParent.Nodes.Count == 0) && (bFill == true))
			{
				Fill(tmaxParent);
			}
			
			foreach(CTmaxMediaTreeNode O in tmaxParent.Nodes)
			{
				if(O.GetMediaRecord() != null)
				{
					if(ReferenceEquals(dxChild, O.GetMediaRecord()) == true)
					{
						tmaxMedia = O;
						break;
					}
					
				}
				
			}
			
			return tmaxMedia;
		
		}// private CTmaxMediaTreeNode GetMediaNode(CTmaxMediaTreeNode tmaxParent, CDxMediaRecord dxChild, bool bFill)
		
		/// <summary>This method is called to add a new binder node to the specified collection</summary>
		///	<param name="aNodes">The collection where the new node should be stored</param>
		/// <param name="dxBinder">The binder entry to be added</param>
		/// <returns>The new node if successful</returns>
		private CTmaxMediaTreeNode Add(TreeNodesCollection aNodes, CDxBinderEntry dxBinder)
		{
			CTmaxMediaTreeNode	tmaxNode = null;
			long			lChildren = 0;
			
			//	Create a node for this entry
			if((tmaxNode = CreateNode(dxBinder)) != null)
			{
                if (!dxBinder.IsMedia())
                {
                    tmaxNode.Sorter = new CTmaxBaseTreeSorter();
                    tmaxNode.Sorter.SortBy =dxBinder.SpareNumber==null || dxBinder.SpareNumber == 0 ? TmaxTreeSortFields.Text : (TmaxTreeSortFields)dxBinder.SpareNumber;
                    if(!string.IsNullOrEmpty(dxBinder.SpareText))
                    {
                        tmaxNode.Sorter.CaseCodeId = Convert.ToInt32(dxBinder.SpareText);
                    }
                }
                //	Add to the caller's collection if provided
				if(aNodes != null)
					aNodes.Add(tmaxNode);
				else
					m_tmaxTreeCtrl.Nodes.Add(tmaxNode);
					
				//	Set the property values AFTER adding to the tree
				tmaxNode.SetProperties(GetImageIndex(tmaxNode));
				
				//	How many children does this node have
				if(tmaxNode.GetTmaxRecord(true) != null)
					lChildren = tmaxNode.GetTmaxRecord(true).GetChildCount();
				else
					lChildren = dxBinder.ChildCount;
					
				if(lChildren > 0)
					tmaxNode.Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.CheckOnExpand;
					//tmaxNode.Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.Always;
				else
					tmaxNode.Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.CheckOnDisplay;

				//	Don't expand media nodes while dragging
				if(tmaxNode.GetMediaRecord() == null)
				{
					tmaxNode.Override.AllowAutoDragExpand = AllowAutoDragExpand.ExpandOnDragHoverWhenExpansionIndicatorVisible;
				}
				else
				{
					if(tmaxNode.GetMediaRecord().GetMediaType() == TmaxMediaTypes.Script)
						tmaxNode.Override.AllowAutoDragExpand = AllowAutoDragExpand.ExpandOnDragHoverWhenExpansionIndicatorVisible;
					else
						tmaxNode.Override.AllowAutoDragExpand = AllowAutoDragExpand.Never;
				}
			
			}
			
			return tmaxNode;
			
		}// private CTmaxMediaTreeNode Add(TreeNodesCollection aNodes, CDxBinderEntry dxBinder)
		
		/// <summary>This method is called to add a node for the specified source folder to the tree</summary>
		/// <param name="tmaxParent">The parent node to which the source folder is being added</param>
		/// <param name="tmaxSource">The source folder to be added</param>
		/// <returns>The new node if successful</returns>
		private CTmaxMediaTreeNode Add(CTmaxMediaTreeNode tmaxParent, CTmaxSourceFolder tmaxSource)
		{
			CTmaxMediaTreeNode tmaxNode = null;
			
			//	Make sure we have the required objects
			if(tmaxSource == null) return null;
			
			//	Was this folder processed by the database?
			if(tmaxSource.Registered == false) return null;
			
			try
			{
				//	Create the new node
				if((tmaxNode = CreateNode(tmaxSource)) == null) return null;

				//	Add to the tree collection if no parent is specified
				if((tmaxParent != null) && (tmaxParent.Nodes != null))
					tmaxParent.Nodes.Add(tmaxNode);
				else
					m_tmaxTreeCtrl.Nodes.Add(tmaxNode);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_VIRTUAL_ADD_SOURCE_FOLDER_EX, tmaxSource.Path), Ex);
				return null;
			}
				
			//	Add a node for each subfolder
			if((tmaxSource.SubFolders != null) && (tmaxSource.SubFolders.Count > 0))
			{
				foreach(CTmaxSourceFolder tmaxFolder in tmaxSource.SubFolders)
				{
					Add(tmaxNode, tmaxFolder);
				}
				
			}// if(tmaxSource.SubFolders != null)
			
			//	Add a node for each file
			if((tmaxSource.Files != null) && (tmaxSource.Files.Count > 0))
			{
				foreach(CTmaxSourceFile tmaxFile in tmaxSource.Files)
				{
					Add(tmaxNode, tmaxFile);
				}
				
			}// if(tmaxSource.SubFolders != null)
			
			return tmaxNode;
			
		}// private CTmaxMediaTreeNode Add(CTmaxMediaTreeNode tmaxParent, CTmaxSourceFolder tmaxSource)
		
		/// <summary>This method is called to add a parent binder for the entries being added to the tree</summary>
		/// <param name="tmaxAdditions">The existing collection of items representing entries to be added to the tree</param>
		/// <returns>The new collection of items representing entries to be added to the tree</returns>
		private CTmaxItems AddRegistrationParent(CTmaxItems tmaxAdditions)
		{
			CTmaxItems	tmaxParented = null;
			CTmaxItem	tmaxParent = null;
			CTmaxItem	tmaxMedia = null;
			string		strPath = "";
			try
			{
				//	We are not allowed to drop records directly into the root so,
				//	if any one of the top level additions is a media record, then
				//	we have to add a parent binder
				foreach(CTmaxItem O in tmaxAdditions)
				{
					if(O.IPrimary != null)
					{
						tmaxMedia = O;
						break;
					}
					
				}// foreach(CTmaxItem O in tmaxAdditions)
				
				//	No need to add a parent if no media appears at the top level
				if(tmaxMedia == null) return tmaxAdditions;
				
				//	Get the path the media was registered from so that we can use
				//	it to initialize the new parent binder
				//
				//	NOTE:	Use the registered path first in case the database has messed with
				//			the folder path. If registered path not available, use the source
				//			folder path. If neither is available, substitute a default.
				strPath = ((CDxPrimary)tmaxMedia.IPrimary).RegisterPath;
				if(strPath.Length == 0)
				{
					if(tmaxMedia.SourceFolder != null)
						strPath = tmaxMedia.SourceFolder.Path;
				}
				
				//	Adjust the path based on the primary media type
				switch(tmaxMedia.IPrimary.GetMediaType())
				{
					case TmaxMediaTypes.Document:
					case TmaxMediaTypes.Recording:
					
						//	We need to use the parent folder instead of the actual
						//	source folder because document's and records are groups
						//	of files contained in a folder
						if(strPath.EndsWith("\\") == true)
							strPath = strPath.Substring(0, strPath.Length - 1);
	
						int iIndex = strPath.LastIndexOf("\\");
						if(iIndex > 0)
							strPath = strPath.Substring(0, iIndex);						
						break;
						
					case TmaxMediaTypes.Powerpoint:
					case TmaxMediaTypes.Deposition:
					default:
					
						//	Nothing to do because these are individual files
						break;
						
				}
				
				// Just in case ........
				if(strPath.Length == 0) 
					strPath = "c:\\unknown_path";
				
				//	Create a new item to represent the parent binder
				tmaxParent = new CTmaxItem();
				tmaxParent.DataType = TmaxDataTypes.Binder;
				if(tmaxParent.SourceFolder == null)
					tmaxParent.SourceFolder = new CTmaxSourceFolder();
				tmaxParent.SourceFolder.Initialize(strPath);
				
				//	Make the collection of new entries children of this parent
				tmaxParent.SourceItems = tmaxAdditions;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "AddRegistrationParent", Ex);
			}
			
			//	Did we create a parent item?
			if(tmaxParent != null)
			{
				//	Create the outer collection
				tmaxParented = new CTmaxItems();
				tmaxParented.Add(tmaxParent);
			}
			
			if(tmaxParented != null)
				return tmaxParented;
			else
				return tmaxAdditions;	
			
		}// private CTmaxItems AddRegistrationParent(CTmaxItems tmaxAdditions)
		
		/// <summary>This method is called when new children have been added to a binder</summary>
		/// <param name="dxParent">The parent node</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		private void OnAddEntries(CTmaxMediaTreeNode tmaxParent, CTmaxItems tmaxChildren)
		{
			CTmaxMediaTreeNode tmaxChild = null;
			
			//	Should we just go ahead and fill the whole parent collection?
			if((tmaxParent != null) && 
			   (tmaxParent.Nodes != null) && 
			   (tmaxParent.Nodes.Count == 0))
			{
				Fill(tmaxParent);
	
				//	No need to drill down because this node is not expanded yet
			}
			else
			{
				foreach(CTmaxItem O in tmaxChildren)
				{
					//	Does this new item belong in this tree?
					if(O.IBinderEntry != null)
					{
						//	Add to the appropriate collection
						if(tmaxParent != null)
						{
							if((tmaxChild = GetBinderNode(tmaxParent, (CDxBinderEntry)O.IBinderEntry, false)) == null)
								tmaxChild = Add(tmaxParent.Nodes, (CDxBinderEntry)O.IBinderEntry);
						}
						else
						{
							if((tmaxChild = GetBinderNode(null, (CDxBinderEntry)O.IBinderEntry, false)) == null)
								tmaxChild = Add(m_tmaxTreeCtrl.Nodes, (CDxBinderEntry)O.IBinderEntry);
						}
					
					}// if(O.IBinderEntry != null)
						
				}// foreach(CTmaxItem O in tmaxChildren)
			
			}
		
		}// private void OnAddEntries(CTmaxMediaTreeNode tmaxParent, CTmaxItems tmaxChildren)
			
		/// <summary>This method is called when new children have been added to a media object</summary>
		/// <param name="dxParent">The parent media record</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		private void OnAddMedia(CDxMediaRecord dxParent, CTmaxItems tmaxChildren)
		{
			CTmaxMediaTreeNodes tmaxMatches = null;
			
			Debug.Assert(dxParent != null);
			Debug.Assert(tmaxChildren != null);
			Debug.Assert(dxParent.MediaType != TmaxMediaTypes.Unknown);
			Debug.Assert(dxParent.GetMediaLevel() != TmaxMediaLevels.None);
			
			//	All new media should be in the child collection
			Debug.Assert(tmaxChildren.Count > 0);
			if(tmaxChildren.Count == 0) return;
		
			//	Locate all nodes in the tree that reference the parent
			tmaxMatches = new CTmaxMediaTreeNodes();
			if((dxParent.MediaType == TmaxMediaTypes.Designation) ||
			   (dxParent.MediaType == TmaxMediaTypes.Clip))
			{
				GetScenesFromSource(null, dxParent, tmaxMatches);
			}
			else
			{
				GetNodesFromRecord(null, dxParent, tmaxMatches);
			}
			
			//	Nothing to do if parent doesn't appear in the tree
			if(tmaxMatches.Count == 0) return;

			foreach(CTmaxMediaTreeNode O in tmaxMatches)
			{
				//	Should we add the child nodes now or wait for the 
				//	the user to expand the node?
				if((O.Expanded == true) || ((O.Nodes != null) && (O.Nodes.Count > 0)))
				{
					//	Add the new children to the collection
					foreach(CTmaxItem tmaxChild in tmaxChildren)
					{
						if(tmaxChild.GetMediaRecord() != null)
						{
							Add(O, tmaxChild.GetMediaRecord());
						}
					}
					
				}
				else
				{
					//	Make sure the user can expand the node
					if(O.HasExpansionIndicator == false)
					{
						O.Override.ShowExpansionIndicator = ShowExpansionIndicator.Always;
					}
				}
					
				//	Make sure all text and sorting is in order
				Refresh(O);
			}
			
		}// private void OnAddMedia(CDxMediaRecord dxParent, CTmaxItems tmaxChildren)
		
		/// <summary>This method is called to create a new node for the specified binder entry</summary>
		/// <param name="dxBinder">The binder entry to be displayed</param>
		/// <returns>The new node if successful</returns>
		private CTmaxMediaTreeNode CreateNode(CDxBinderEntry dxBinder)
		{
			CTmaxMediaTreeNode tmaxNode = null;
			
			//	Is this a media entry?
			if(dxBinder.Source != null)
			{
				//	Create a media node
				if((tmaxNode = CreateNode(dxBinder.Source)) != null)
				{
					tmaxNode.IBinder = dxBinder;

					//	NO CHECK BOXES FOR MEDIA NODES
					//if(m_bCheckBoxes == true)
						//tmaxNode.Override.NodeStyle = Infragistics.Win.UltraWinTree.NodeStyle.CheckBox;
				}
				
				return tmaxNode;
			}
			else
			{
				try
				{
					tmaxNode = new CTmaxMediaTreeNode(dxBinder.Name);
					tmaxNode.MediaType = TmaxMediaTypes.Unknown;
					tmaxNode.IBinder = dxBinder;

					if(m_bCheckBoxes == true)
						tmaxNode.Override.NodeStyle = Infragistics.Win.UltraWinTree.NodeStyle.CheckBox;
						
					return tmaxNode;
				}
				catch
				{
					return null;
				}
				
			}
			
		}// private CTmaxMediaTreeNode CreateNode(CDxBinderEntry dxBinder)
		
		/// <summary>This method is called to get the actions allowed when the user is dragging records in the root</summary>
		/// <returns>The appropriate action if the user were to drop into the root</returns>
		private TreeDropActions GetDropRecordsInRootAction()
		{
			ITmaxMediaRecord	tmaxIDragParent = null;
			TreeDropActions		eAction = TreeDropActions.None;
						
			//	Get the drag parent interface
			if((tmaxIDragParent = m_tmaxDragData.GetMediaRecord()) == null)
				tmaxIDragParent = m_tmaxDragData.IBinderEntry;
			
			//	Is the user dragging media records?
			if(m_dropTarget.bDraggingMedia == true)
			{
				//	Not allowed to have media records in the root so the
				//	user must be dropping the records into a root binder
				if(m_dropTarget.ePosition == TmaxTreePositions.OnNode)
				{
					if((GetControlPressed() == true) || (m_dropTarget.bAllBinders == false))
					{
						eAction = TreeDropActions.Add;
					}
					else
					{
						eAction = TreeDropActions.MoveInto;
					}
				
				}// if(m_dropTarget.ePosition == TmaxTreePositions.OnNode)
				
			}
			else
			{
				//	Is the user dragging root binders?
				if(tmaxIDragParent == null)
				{
					//	Is the user above/below the drop node?
					if(m_dropTarget.ePosition != TmaxTreePositions.OnNode)
					{ 
						eAction = TreeDropActions.Reorder;
					}
					else
					{
						//	Is the user pressing the control key?
						if(GetControlPressed() == true)
							eAction = TreeDropActions.Add;
						else
							eAction = TreeDropActions.MoveInto;
								
						//	Make sure the drop node is not in the drag collection
						//
						//	NOTE:	I'm too lazy to figure out what it means to drop the node
						//			in itself so instead I just prevent that from happening
						foreach(CTmaxItem O in m_tmaxDragData.SourceItems)
						{
							if(ReferenceEquals(O.IBinderEntry, m_dropTarget.node.IBinder) == true)
							{
								eAction = TreeDropActions.None;
								break;
							}
									
						}
								
					}// if(m_dropTarget.ePosition != TmaxTreePositions.OnNode)
							
				}
				else
				{
					//	Is the user dragging lower level binders into a root binder?
					if(m_dropTarget.ePosition == TmaxTreePositions.OnNode)
					{
						//	Is the user pressing the control key?
						if(GetControlPressed() == true)
							eAction = TreeDropActions.Add;
						else
							eAction = TreeDropActions.MoveInto;
					}
					else
					{
						//	User must be attempting to insert into the root branch
						if(m_dropTarget.ePosition == TmaxTreePositions.AboveNode)
						{
							if(GetControlPressed() == true)
								eAction = TreeDropActions.InsertBefore;
							else
								eAction = TreeDropActions.MoveBefore;
						}
						else if(m_dropTarget.ePosition == TmaxTreePositions.BelowNode)
						{
							if(GetControlPressed() == true)
								eAction = TreeDropActions.InsertAfter;
							else
								eAction = TreeDropActions.MoveAfter;
						}
						
					}
							
				}// if(tmaxIDragParent == null)

			}// if(m_dropTarget.bDraggingMedia == true)

			//	If moving, make sure we don't attempt to move a parent into one of
			//	it's child nodes
			//;adlkjfa; // force compiler error
			
			return eAction;
			
		}// private TreeDropActions GetDropRecordsInRootAction()
		
		/// <summary>Called to get the appropriate drop action is attempting to drop within a binder</summary>
		/// <returns>The appropriate drop action</returns>
		private TreeDropActions GetDropRecordsInBinderAction()
		{
			ITmaxMediaRecord	tmaxIDragParent = null;
			ITmaxMediaRecord	tmaxIDropParent = null;
			TreeDropActions		eAction = TreeDropActions.None;
			bool				bCheckDescendant = false;
						
			Debug.Assert(m_dropTarget.node != null);
			Debug.Assert(m_dropTarget.node.Parent != null);
			if(m_dropTarget.node == null) return TreeDropActions.None;
			if(m_dropTarget.node.Parent == null) return TreeDropActions.None;
						
			//	Get the interfaces to the drag/drop parent records
			if((tmaxIDropParent = ((CTmaxMediaTreeNode)m_dropTarget.node.Parent).GetMediaRecord()) == null)
				tmaxIDropParent = ((CTmaxMediaTreeNode)m_dropTarget.node.Parent).IBinder;
			if((tmaxIDragParent = m_tmaxDragData.GetMediaRecord()) == null)
				tmaxIDragParent = m_tmaxDragData.IBinderEntry;
			
			//	Is the user moving around within the same parent node?
			if((tmaxIDragParent != null) && 
			   (ReferenceEquals(tmaxIDragParent, tmaxIDropParent) == true))
			{
				if(m_dropTarget.ePosition == TmaxTreePositions.OnNode)
				{
					//	Is the drop node a media node?
					if(m_dropTarget.node.GetMediaRecord() != null)
					{
						//	Can only drop into a script
						if(m_dropTarget.node.GetMediaRecord().GetMediaType() == TmaxMediaTypes.Script)
						{
							//	Assume it's OK to add to the script
							eAction = TreeDropActions.Add;

							//	Check media types of the drag nodes
							foreach(CTmaxItem O in m_tmaxDragData.SourceItems)
							{
								if((O.IPrimary != null) && (O.IPrimary.GetMediaType() == TmaxMediaTypes.Deposition))
								{
									//	Can't drop depositions or video segments
									switch(O.GetMediaRecord().GetMediaType())
									{
										case TmaxMediaTypes.Deposition:
										case TmaxMediaTypes.Segment:
										
											eAction = TreeDropActions.None;
											break;
											
									}// switch(O.GetMediaRecord().GetMediaType())
									
								}// if((O.IPrimary != null) && (O.IPrimary.GetMediaType() == TmaxMediaTypes.Deposition))
									
							}// foreach(CTmaxItem O in m_tmaxDragData.SourceItems)
										
						}// if(m_dropTarget.node.GetMediaRecord().GetMediaType() == TmaxMediaTypes.Script)
						
					}
					else
					{
						if(GetControlPressed() == true)
						{
							eAction = TreeDropActions.Add;
						}
						else
						{
							//	Since the drop node is a binder and the drag
							//	parent is the same as the drop parent, we can
							//	trust that the user is dragging binder entries
							eAction = TreeDropActions.MoveInto;
						}
					
					}// if(m_dropTarget.node.GetMediaRecord() != null)
				
					//	Make sure the drop node is not in the drag collection
					foreach(CTmaxItem O in m_tmaxDragData.SourceItems)
					{
						if(ReferenceEquals(O.IBinderEntry, m_dropTarget.node.IBinder) == true)
						{
							eAction = TreeDropActions.None;
							break;
						}
									
					}
								
				}
				else
				{
					eAction = TreeDropActions.Reorder;
				
				}// if(m_dropTarget.ePosition == TmaxTreePositions.OnNode)
			
			}
			else
			{
				//	What is the current position
				switch(m_dropTarget.ePosition)
				{
					case TmaxTreePositions.OnNode:
					
						//	Is the drop node a media node?
						if(m_dropTarget.node.GetMediaRecord() != null)
						{
							//	The only media we can drop into is a script
							if(m_dropTarget.node.GetMediaRecord().GetMediaType() == TmaxMediaTypes.Script)
							{
								//	Assume it's OK to add to the script
								eAction = TreeDropActions.Add;

								//	Check media types of the drag nodes
								foreach(CTmaxItem O in m_tmaxDragData.SourceItems)
								{
									if((O.IPrimary != null) && (O.IPrimary.GetMediaType() == TmaxMediaTypes.Deposition))
									{
										//	Can't drop depositions or video segments
										switch(O.GetMediaRecord().GetMediaType())
										{
											case TmaxMediaTypes.Deposition:
											case TmaxMediaTypes.Segment:
										
												eAction = TreeDropActions.None;
												break;
											
										}// switch(O.GetMediaRecord().GetMediaType())
									
									}// if((O.IPrimary != null) && (O.IPrimary.GetMediaType() == TmaxMediaTypes.Deposition))
									
								}// foreach(CTmaxItem O in m_tmaxDragData.SourceItems)
										
							}// if(m_dropTarget.node.GetMediaRecord().GetMediaType() == TmaxMediaTypes.Script)
						}
						else
						{
							if((GetControlPressed() == true) || (m_dropTarget.bAllBinders == false))
							{
								eAction = TreeDropActions.Add;
							}
							else
							{
								eAction = TreeDropActions.MoveInto;
								bCheckDescendant = true;
					
							}// if(GetControlPressed() == true)
						
						}
						break;
						
					case TmaxTreePositions.AboveNode:
					
						if((GetControlPressed() == true) || (m_dropTarget.bAllBinders == false))
						{
							eAction = TreeDropActions.InsertBefore;
						}
						else
						{
							eAction = TreeDropActions.MoveBefore;
							bCheckDescendant = true;
						}						
						break;
						
					case TmaxTreePositions.BelowNode:
					
						if((GetControlPressed() == true) || (m_dropTarget.bAllBinders == false))
						{
							eAction = TreeDropActions.InsertAfter;
						}
						else
						{
							eAction = TreeDropActions.MoveAfter;
							bCheckDescendant = true;
						}						
						break;
						
				}// switch(m_dropTarget.ePosition)
				
			}

			//	Are we supposed to make sure the drop node is not a descendant of one of the drag nodes?
			if((bCheckDescendant == true) && (m_dropTarget.node != null) && (m_dropTarget.node.IBinder != null))
			{
				//	Make sure the drop node is not a descendant of a drag item
				foreach(CTmaxItem O in m_tmaxDragData.SourceItems)
				{
					if(O.IBinderEntry == null)
						eAction = TreeDropActions.None;
					else if(((CDxMediaRecord)(O.IBinderEntry)).IsDescendant((CDxMediaRecord)(m_dropTarget.node.IBinder)) == true)
						eAction = TreeDropActions.None;
									
					if(eAction == TreeDropActions.None)
						break;
				}
			}
			
			return eAction;
			
		}// private TreeDropActions GetDropRecordsInBinderAction()
		
		/// <summary>This method is called when the binder records associated with the specified child items get deleted</summary>
		/// <param name="dxParent">The parent of the records that have been deleted</param>
		/// <param name="tmaxChildren">The collection that contains the children that have been deleted</param>
		private void OnBindersDeleted(CDxBinderEntry dxParent, CTmaxItems tmaxChildren)
		{
			CTmaxMediaTreeNode	nodeParent = null;
			CTmaxMediaTreeNode	nodeChild = null;
			
			Debug.Assert(tmaxChildren != null);
			Debug.Assert(tmaxChildren.Count > 0);
			
			//	Is the user deleting a node below the root?
			if(dxParent != null)
			{
				//	Get the node associated with the parent if it exists in the tree
				if((nodeParent = GetBinderNode(null, dxParent, false)) == null) return;
			}
			
			//	Remove all the child nodes that match the deleted records
			foreach(CTmaxItem O in tmaxChildren)
			{
				//	Should be a binder
				Debug.Assert(O.IBinderEntry != null);
				if(O.IBinderEntry == null) continue;
				
				//	Get the child node
				if((nodeChild = GetBinderNode(nodeParent, (CDxBinderEntry)O.IBinderEntry, false)) != null)
				{
					//	Remove the node from the tree
					OnNodeDeleted(nodeChild);
				}
				
			}// foreach(CTmaxItem O in tmaxChildren)
					
			//	Refresh the parent node
			//
			//	NOTE:	This really shouldn't be necessary but just in case something
			//			gets out of order this will keep it straight
			if(nodeParent != null)
			{
				Refresh(nodeParent);
			}
			else
			{
				//	The user must have reordered root nodes
				Sort(m_tmaxTreeCtrl.Nodes,true);
					
				foreach(CTmaxMediaTreeNode O in m_tmaxTreeCtrl.Nodes)
					SetText(O, false);
			}
			
			//	Just in case we deleted the target node
			OnTargetBinderChanged(null, m_tmaxDatabase.TargetBinder);
			
		}// private void OnBindersDeleted(CDxBinderEntry dxParent, CTmaxItems tmaxChildren)
		
		/// <summary>This method is called when the media records associated with the specified child items get deleted</summary>
		/// <param name="tmaxParent">The parent of the records that have been deleted</param>
		/// <param name="tmaxChildren">The collection that contains the children that have been deleted</param>
		private void OnMediaDeleted(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		{
			CTmaxMediaTreeNodes	tmaxNodes = new CTmaxMediaTreeNodes();
			CTmaxMediaTreeNode	tmaxDeleted = null;
			CTmaxMediaTreeNode	tmaxRefresh = null;
			
			Debug.Assert(tmaxParent != null);
			Debug.Assert(tmaxChildren != null);
			Debug.Assert(tmaxChildren.Count > 0);
			
			//	Is the user deleting primary media?
			if(tmaxParent.GetMediaRecord() == null)
			{
				//	Iterate the collection of primary records that have been deleted
				foreach(CTmaxItem O in tmaxChildren)
				{
					Debug.Assert(O.GetMediaRecord() != null);
					if(O.GetMediaRecord() == null) continue;
					
					//	Locate all nodes that reference this record
					GetNodesFromRecord(null, O.GetMediaRecord(), tmaxNodes);
					
					//	Delete each of these nodes
					foreach(CTmaxMediaTreeNode tmaxNode in tmaxNodes)
					{
						//	We want to refresh the parent after processing the removal
						tmaxRefresh = (CTmaxMediaTreeNode)(tmaxNode.Parent);
						
						//	Disconnect the node
						OnNodeDeleted(tmaxNode);
						
						//	Refresh its parent
						if(tmaxRefresh != null)
						{
							Refresh(tmaxRefresh);
						}
						else
						{
							//	This shouldn't happen because we can't have
							//	media nodes in the root
							Debug.Assert(tmaxNode.Parent != null);
						}
						
					}// foreach(CTmaxMediaTreeNode tmaxNode in tmaxNodes)
					
					tmaxNodes.Clear();
				
				}// foreach(CTmaxItem O in tmaxChildren)
				
			}
			else
			{
				//	Locate all nodes that reference the parent
				GetNodesFromRecord(null, tmaxParent.GetMediaRecord(), tmaxNodes);

				//	Iterate the collection of parent nodes
				foreach(CTmaxMediaTreeNode O in tmaxNodes)
				{
					//	Don't have anything to do if this node hasn't been
					//	populated yet
					if((O.Nodes == null) || (O.Nodes.Count == 0)) continue;
					
					//	Remove the child nodes
					foreach(CTmaxItem tmaxChild in tmaxChildren)
					{
						if(tmaxChild.GetMediaRecord() != null)
						{
							//	Get the node that matches this child
							if((tmaxDeleted = GetMediaNode(O, (CDxMediaRecord)(tmaxChild.GetMediaRecord()), false)) != null)
							{
								OnNodeDeleted(tmaxDeleted);
							}
						
						}
						
					}// foreach(CTmaxItem tmaxChild in tmaxChildren)
					
					//	Refresh the parent
					Refresh(O);
						
				}// foreach(CTmaxMediaTreeNode tmaxNode in tmaxNodes)
					
				tmaxNodes.Clear();
				
			}
			
		}// private void OnMediaDeleted(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		
		/// <summary>This method is called when a node has been deleted</summary>
		/// <param name="tmaxNode">The node that has been deleted</param>
		/// <param name="bChangeSelection">True if OK to change the current selection</param>
		private void OnNodeDeleted(CTmaxMediaTreeNode tmaxNode, bool bChangeSelection)
		{
			CTmaxMediaTreeNode tmaxSelect = null;
			
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
			if(m_tmaxTreeCtrl.ActiveNode != null)
			{
				if(ReferenceEquals(m_tmaxTreeCtrl.ActiveNode, tmaxNode) == true)
					m_tmaxTreeCtrl.ActiveNode = null;
			}
				
			//	Remove the node
			if(tmaxNode.Parent != null)
				tmaxNode.Parent.Nodes.Remove(tmaxNode);
			else
				m_tmaxTreeCtrl.Nodes.Remove(tmaxNode);
				
			//	Should we select a new node
			if((bChangeSelection == true) && (tmaxSelect != null))
			{
				m_tmaxTreeCtrl.SetSelection(tmaxSelect);
			}

		}// private void OnNodeDeleted(CTmaxMediaTreeNode tmaxNode, bool bChangeSelection)
			
		/// <summary>This method is called when a node has been deleted</summary>
		/// <param name="tmaxNode">The node that has been deleted</param>
		private void OnNodeDeleted(CTmaxMediaTreeNode tmaxNode)
		{
			//	Automatically update the current selection by default
			OnNodeDeleted(tmaxNode, true);

		}// private void OnNodeDeleted(CTmaxMediaTreeNode tmaxNode)
			
		/// <summary>This method is called to get a drop target descriptor for adding newly registered media to the tree</summary>
		/// <param name="regTarget">The registration target descriptor to be initialized</param>
		///	<returns>True if successful</returns>
		private bool GetRegistrationTarget(ref SRegistrationTarget regTarget)
		{
			//	Set the default values
			regTarget.node    = null;
			regTarget.parent  = null;
			regTarget.bInsert = false;
			regTarget.bBefore = false;
			regTarget.bInRoot = false;
			
			//	Is there a drop target?
			if(m_dropTarget.node != null)
			{
				regTarget.node = m_dropTarget.node;
				regTarget.parent = (CTmaxMediaTreeNode)(regTarget.node.Parent);
				
				//	What is the drop position?
				switch(m_dropTarget.ePosition)
				{
					case TmaxTreePositions.AboveNode:
						
						regTarget.bBefore = true;
						regTarget.bInsert = true;
						break;
							
					case TmaxTreePositions.BelowNode:
						
						regTarget.bBefore = false;
						regTarget.bInsert = true;
						break;
							
					default:
						
						break;
				
				}// switch(m_eDropPosition)
			
			}// if((m_dropTarget.node != null) && (m_dropTarget.node.Nodes != null))
			else
			{
			
			}
			
			//	Are we inserting the new media?
			if(regTarget.bInsert == true)
			{
				//	The drop node parent must be null or must be a binder node
				if((regTarget.parent == null) || (regTarget.parent.GetTmaxRecord(true) == null))
				{
					// OK to drop the new media
					regTarget.bInRoot = (regTarget.parent == null);
				}
				else
				{
					//	If a parent node exists, it MUST be a binder in order to insert on registration
					Debug.Assert(false, "Can't drop register into a media record");
					return false;
				}
			
			}
			else
			{
				//	The drop node must be null or must be a binder node
				if((regTarget.node == null) || (regTarget.node.GetTmaxRecord(true) == null))
				{
					// OK to drop the new media
					regTarget.bInRoot = (regTarget.node == null);
				}
				else
				{
					//	Can't add source to a media record
					Debug.Assert(false, "Can't drop register into a media record");
					return false;
				}
			
			}// if(regTarget.bInsert == true)
			
			return true;
			
		}// private bool GetRegistrationTarget(ref SRegistrationTarget regTarget)
			
		/// <summary>This method creates and event item collection that requests the additions required to represent the new source registrations</summary>
		/// <param name="tmaxSource">The registered source folder</param>
		/// <returns>The event items needed to add the new nodes to the tree</returns>
		private CTmaxItems GetRegisteredAdditions(CTmaxSourceFolder tmaxSource)
		{
			CTmaxItems tmaxAdditions = null;
			
			try
			{
				//	Allocate the collection to hold event items for the entries to be added to the tree
				tmaxAdditions = new CTmaxItems();
				
				//	Should the top-level folder be included in the additions?
				if((tmaxSource.Anchor == true) || (tmaxSource.IPrimary != null))
				{
					//	Get entries for the top-level folder and all it's subfolders
					GetRegisteredAdditions(tmaxSource, tmaxAdditions);
				}
				else
				{
					//	Get entries for each of the subfolders
					foreach(CTmaxSourceFolder O in tmaxSource.SubFolders)
						GetRegisteredAdditions(O, tmaxAdditions);
				}
				
			}
			catch
			{
				Debug.Assert(false, "GetRegisteredAdditions() exception raised");
			}
		
			//	Do we have anything to add?
			if((tmaxAdditions != null) && (tmaxAdditions.Count > 0))
				return tmaxAdditions;
			else
				return null;
		
		}

		/// <summary>This method creates and event item collection that requests the additions required to represent the new source registrations</summary>
		/// <param name="tmaxSource">The registered source folder</param>
		/// <param name="tmaxAdditions">The collection in which to store items for the entries to be added to the tree</param>
		/// <returns>The total number of items added to the collection</returns>
		private long GetRegisteredAdditions(CTmaxSourceFolder tmaxSource, CTmaxItems tmaxAdditions)
		{
			CTmaxItem	tmaxPrimary = null;
			CTmaxItem	tmaxBinder = null;
			long		lPrevious = 0;
			
			Debug.Assert(tmaxSource != null, "Invalid Parameter: tmaxSource");
			Debug.Assert(tmaxSource != null, "Invalid Parameter: tmaxAdditions");
			
			try
			{
				//	How many items have already been added to the collection?
				lPrevious = tmaxAdditions.Count;
				
				//	Add an item to request and entry for the folder's primary record
				if(tmaxSource.IPrimary != null)
				{
					//	Create an item to request a new media entry
					tmaxPrimary = new CTmaxItem(tmaxSource.IPrimary);
					tmaxPrimary.SourceFolder = tmaxSource;
				
					tmaxAdditions.Add(tmaxPrimary);
				}

				//	Does this folder have any subfolders?
				if((tmaxSource.SubFolders != null) && (tmaxSource.SubFolders.Count > 0))
				{
					//	Create an item to request a new binder
					tmaxBinder = new CTmaxItem();
					tmaxBinder.DataType = TmaxDataTypes.Binder;
					tmaxBinder.SourceFolder = tmaxSource;
				
					//	Make sure there is a valid collection in which to store the subfolders
					if(tmaxBinder.SourceItems == null)
						tmaxBinder.SourceItems = new CTmaxItems();
						
					foreach(CTmaxSourceFolder O in tmaxSource.SubFolders)
					{
						//	Get the additions required for each subfolder
						GetRegisteredAdditions(O, tmaxBinder.SourceItems);
					
					}// foreach(CTmaxSourceFolder O in tmaxSource.SubFolders)
				
					//	Add the binder as long as it contains something
					if(tmaxBinder.SourceItems.Count > 0)
						tmaxAdditions.Add(tmaxBinder);
				
				}// if((tmaxSource.SubFolders != null) && (tmaxSource.SubFolders.Count > 0))
				
			}
			catch
			{
				Debug.Assert(false, "GetRegisteredAdditions() exception raised");
			}
		
			return (tmaxAdditions.Count - lPrevious);
		
		}// private long GetRegisteredAdditions(CTmaxSourceFolder tmaxSource, CTmaxItems tmaxAdditions)
		
		/// <summary>This method is called when the PauseThreshold value changes</summary>
		private void OnPauseThresholdChanged(CTmaxMediaTreeNode tmaxNode)
		{
			CDxMediaRecord dxRecord = null;
			int       iImage = 0;
			
			try
			{
				//	Is this a media node?
				if((dxRecord = (CDxMediaRecord)(tmaxNode.GetMediaRecord())) != null)
				{
					//	What type of media is this?
					switch(dxRecord.MediaType)
					{
						case TmaxMediaTypes.Designation:
						case TmaxMediaTypes.Scene:
						
							//	Update the image for this node
							iImage = GetImageIndex(tmaxNode);
							if(iImage != (int)(tmaxNode.Override.NodeAppearance.Image))
								tmaxNode.Override.NodeAppearance.Image = iImage;
							break;
							
						case TmaxMediaTypes.Script:
						
							//	Drill down into child nodes
							if((tmaxNode.Nodes != null) && (tmaxNode.Nodes.Count > 0))
							{
								foreach(CTmaxMediaTreeNode O in tmaxNode.Nodes)
									OnPauseThresholdChanged(O);
							}
							break;
							
						default:
						
							//	Nothing to do for these types
							break;
							
					}// switch(dxRecord.MediaType)
				
				}
				else
				{
					//	Drill down into binders
					if((tmaxNode.Nodes != null) && (tmaxNode.Nodes.Count > 0))
					{
						foreach(CTmaxMediaTreeNode O in tmaxNode.Nodes)
							OnPauseThresholdChanged(O);
					}
					
				}// if((dxRecord = (CDxMediaRecord)(tmaxNode.GetMediaRecord())) != null)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnPauseThresholdChanged", Ex);
			}
			
		}// private override void OnPauseThresholdChanged()
			
		/// <summary>Called to check the contents of the specified binder to see if the media record is referenced</summary>
		/// <param name="dxBinder">The binder to be searched</param>
		/// <param name="dxRecord">The record to be located</param>
		/// <param name="bFill">true if OK to fill the Contents collection</param>
		/// <returns>true if the record is in the binder</returns>
		private bool CheckContents(CDxBinderEntry dxBinder, CDxMediaRecord dxRecord, bool bFill)
		{
			//	Should we fill the contents?
			if((dxBinder.Contents.Count == 0) && (bFill == true))
				dxBinder.Fill();
				
			//	Search the contents for a reference to the media record
			foreach(CDxBinderEntry O in dxBinder.Contents)
			{
				if((O.IsMedia() == true) && (O.GetSource(true) != null))
				{
					if(ReferenceEquals(O.GetSource(true), dxRecord) == true)
						return true;
				}
				
			}// foreach(CDxBinderEntry O in dxBinder.Contents)
			
			return false; // not found
			
		}// private bool CheckContents(CDxBinderEntry dxBinder, CDxMediaRecord dxRecord, bool bFill)
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Flag to indicate that entries are being added from TmaxPresentation</summary>
		public bool AddFromPresentation
		{
			get { return m_bAddFromPresentation; }
			set { m_bAddFromPresentation = value; }
		}
		
		#endregion Properties
		
	}// public class CBinderTree : FTI.Trialmax.Panes.CTreePane

}// namespace FTI.Trialmax.Panes
