using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

using FTI.Shared;
using FTI.Shared.Trialmax;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinTree;

namespace FTI.Trialmax.Controls
{
	/// <summary>This class creates a custom tree for displaying TrialMax pick lists</summary>
	public class CTmaxPickListTreeCtrl : CTmaxBaseTreeCtrl
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		private const int ERROR_SET_PICK_LISTS_EX	= ERROR_TMAX_BASE_TREE_MAX + 1;
		private const int ERROR_CREATE_NODE_EX		= ERROR_TMAX_BASE_TREE_MAX + 2;
		private const int ERROR_ADD_EX				= ERROR_TMAX_BASE_TREE_MAX + 3;
		private const int ERROR_UPDATE_EX			= ERROR_TMAX_BASE_TREE_MAX + 4;
		private const int ERROR_SET_TYPE_EX			= ERROR_TMAX_BASE_TREE_MAX + 5;
		private const int ERROR_FILL_EX				= ERROR_TMAX_BASE_TREE_MAX + 6;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to PickLists property</summary>
		private FTI.Shared.Trialmax.CTmaxPickItem m_tmaxPickLists = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>This is the delegate used to handle events fired by this control</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">Event arguments</param>
		/// <returns>True to cancel the operation</returns>
		public delegate bool TmaxPickListTreeHandler(object sender, CTmaxPickListTreeNode tmaxNode);
		
		/// <summary>This event is fired before a node gets expanded</summary>
		new public event TmaxPickListTreeHandler BeforeExpand;
		
		/// <summary>This event is fired after a node gets expanded</summary>
		new public event TmaxPickListTreeHandler AfterExpand;
		
		/// <summary>This event is fired before a node gets selected</summary>
		new public event TmaxPickListTreeHandler BeforeSelect;
		
		/// <summary>This event is fired after a node gets selected</summary>
		new public event TmaxPickListTreeHandler AfterSelect;
		
		/// <summary>Constructor</summary>
		public CTmaxPickListTreeCtrl() : base()
		{
			//	Set the default event source name
			m_tmaxEventSource.Name = "Pick Lists Tree";			
		}
		
		/// <summary>This method is called to get the current selection</summary>
		/// <returns>The node that is currently selected</returns>
		new public CTmaxPickItem GetSelection()
		{
			CTmaxPickListTreeNode tmaxNode = null;
			
			if((tmaxNode = (CTmaxPickListTreeNode)(base.GetSelection())) != null)
				return tmaxNode.PickItem;
			else
				return null;
		}

		/// <summary>This method is called to get the current selections in the tree</summary>
		/// <returns>The items that are currently selected sorted by position</returns>
		new public CTmaxPickItems GetSelections()
		{
			return GetSelections(true);
		}

		/// <summary>This method is called to get the current selections in the tree</summary>
		/// <param name="bSortByPosition">true if the nodes should be sorted based on their position in the tree</param>
		/// <returns>The items that are currently selected sorted by position</returns>
		new public CTmaxPickItems GetSelections(bool bSortByPosition)
		{
			CTmaxPickListTreeNodes	tmaxNodes = null;
			CTmaxPickItems			tmaxSelections = null;
			
			if((tmaxNodes = GetTmaxNodes(this.SelectedNodes, bSortByPosition)) != null)
			{
				tmaxSelections = new CTmaxPickItems();
				foreach(CTmaxPickListTreeNode O in tmaxNodes)
					tmaxSelections.Add(O.PickItem);
			}
			
			return tmaxSelections;
		
		}// new public CTmaxPickItems GetSelections(bool bSortByPosition)

		/// <summary>This method will populate a TrialMax node collection using the specified selected node collection</summary>
		/// <param name="aSelected">The Infragistics tree node collection</param>
		/// <param name="bSortByPosition">True to sort the nodes by their position in the tree</param>
		/// <returns>The populated TrialMax node collection</returns>
		new public CTmaxPickListTreeNodes GetTmaxNodes(Infragistics.Win.UltraWinTree.SelectedNodesCollection aSelected, bool bSortByPosition)
		{
			return (CTmaxPickListTreeNodes)(this.GetTmaxNodes(new CTmaxPickListTreeNodes(), aSelected, bSortByPosition));
		}
		
		/// <summary>This method will populate a TrialMax node collection using the specified selected node collection</summary>
		/// <param name="aSelected">The Infragistics tree node collection</param>
		/// <returns>The populated TrialMax node collection</returns>
		new public CTmaxPickListTreeNodes GetTmaxNodes(Infragistics.Win.UltraWinTree.SelectedNodesCollection aSelected)
		{
			return GetTmaxNodes(aSelected, true);
		}
		
		/// <summary>This method is called to get the node located at the specified point</summary>
		/// <param name="objPoint">The point within the tree's client area</param>
		/// <returns>The node object if it exists</returns>
		new public CTmaxPickListTreeNode GetNode(Point objPoint)
		{
			return (CTmaxPickListTreeNode)(base.GetNode(objPoint));
		}

		/// <summary>This method is called to get the node located at the specified coordinates</summary>
		/// <param name="iX">The x coordinate within the tree's client area</param>
		/// <param name="iY">The y coordinate within the tree's client area</param>
		/// <returns>The node object if it exists</returns>
		new public CTmaxPickListTreeNode GetNode(int iX, int iY)
		{
			return (CTmaxPickListTreeNode)(base.GetNode(iX, iY));
		}
		
		/// <summary>This method is called to get the node at the current cursor position</summary>
		/// <return>The node under the current cursor position</return>
		new public CTmaxPickListTreeNode GetNode()
		{
			return (CTmaxPickListTreeNode)(base.GetNode());
		}
			
		/// <summary>This method is called to set the pick list collection</summary>
		/// <param name="tmaxPickLists">The new pick list collection</param>
		/// <returns>true if successful</returns>
		public bool SetPickLists(CTmaxPickItem tmaxPickLists)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Clear the tree
				this.Clear();
				
				//	Assign the new collection
				m_tmaxPickLists = tmaxPickLists;
				
				//	Do we have a collection of new lists?
				//
				//	NOTE:	The root pick list item is just a placeholder. The top-level
				//			lists are contained in its child collection
				if((m_tmaxPickLists != null) && (m_tmaxPickLists.Children != null))
				{
					Fill(m_tmaxPickLists, null);
				}
				
				bSuccessful = true;
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetPickLists", m_tmaxErrorBuilder.Message(ERROR_SET_PICK_LISTS_EX), Ex);
			}
			
			return bSuccessful;
			
		}// public bool SetPickLists(CTmaxPickItems tmaxPickItems)
		
		/// <summary>This method is called to add a a node for the specified item to the tree</summary>
		/// <param name="tmaxPickItem">The pick item to be added</param>
		/// <param name="bChildren">True to add the child nodes</param>
		/// <param name="bEnsureVisible">True to make sure the node is visible</param>
		/// <returns>The newly added node</returns>
		public CTmaxPickListTreeNode Add(CTmaxPickItem tmaxPickItem, bool bChildren, bool bEnsureVisible)
		{
			CTmaxPickListTreeNode	tmaxParent = null;
			CTmaxPickListTreeNode	tmaxAdded = null;
			
			Debug.Assert(tmaxPickItem != null);
			if(tmaxPickItem == null) return null;
			
			//	Does this item have a parent?
			if((tmaxPickItem.Parent != null) && (tmaxPickItem.Parent.UniqueId > 0))
			{
				if((tmaxParent = GetNode(tmaxPickItem.Parent, true)) != null)
				{
					//	Has the parent been populated and/or expanded?
					if(((tmaxParent.Nodes != null) && (tmaxParent.Nodes.Count > 0)) || 
					   (tmaxParent.Expanded == true))
					{
						tmaxAdded = Add(tmaxPickItem, tmaxParent, bChildren);
					}
					else
					{
						//	Don't do anything, wait for the user to expand the node
						//
						//	Make sure the user can expand the node
						if(tmaxParent.HasExpansionIndicator == false)
						{
							tmaxParent.Override.ShowExpansionIndicator = ShowExpansionIndicator.Always;
						}
					}
					
				}// if((tmaxParent = GetNode(tmaxPickItem.Parent, true)) != null)
				
			}// if((tmaxPickItem.Parent != null) && (tmaxPickItem.Parent.UniqueId > 0))
			else
			{
				tmaxAdded = Add(tmaxPickItem, null, bChildren);
			}
			
			if(bEnsureVisible == true)
				EnsureVisible(tmaxPickItem);
				
			return tmaxAdded;
		
			//	the user to expand the node?
//			if((tmaxNode.Nodes != null) && 
//			{
//				//	Add the new children to the collection
//				foreach(CTmaxItem tmaxChild in tmaxChildren)
//				{
//					if(tmaxChild.GetMediaRecord() != null)
//					{
//						if(Find(tmaxNode, tmaxChild.GetMediaRecord()) == null)
//							Add(tmaxNode, tmaxChild.GetMediaRecord());
//					}
//				}
//						
//			}
//			else
//			{
//			}
		}// public CTmaxPickListTreeNode Add(CTmaxPickItem tmaxPickItem, bool bChildren, bool bEnsureVisible)
		
		/// <summary>This method will delete the node for the specified item</summary>
		/// <param name="tmaxPickItem">The pick list item to be deleted</param>
		/// <returns>True if successful</returns>
		public bool Delete(CTmaxPickItem tmaxPickItem)
		{
			CTmaxPickListTreeNode tmaxNode = null;
			CTmaxPickListTreeNode tmaxSelect = null;
			
			//	Get the node for this item
			if((tmaxNode = GetNode(tmaxPickItem, false)) == null)
				return true; //	Nothing to delete
				
			//	Is this node the current selection?
			if(tmaxNode.Selected == true)
			{
				if((tmaxSelect = (CTmaxPickListTreeNode)tmaxNode.GetSibling(NodePosition.Next)) == null)
					tmaxSelect = (CTmaxPickListTreeNode)tmaxNode.GetSibling(NodePosition.Previous);
				
				//	Make the parent the next selection if no siblings
				if(tmaxSelect == null)
					tmaxSelect = (CTmaxPickListTreeNode)tmaxNode.Parent;
					
				tmaxNode.Selected = false;				
			}

			//	Make sure it is not the active node
			if(this.ActiveNode != null)
			{
				if(ReferenceEquals(this.ActiveNode, tmaxNode) == true)
					this.ActiveNode = null;
			}
				
			//	Remove from the parent collection
			if(tmaxNode.ParentNodesCollection != null)
				tmaxNode.ParentNodesCollection.Remove(tmaxNode);
				
			if(tmaxSelect != null)
			{
				this.SetSelection(tmaxSelect);
			}
			
			return true;

		}// public bool Delete(CTmaxPickItem tmaxPickItem)
		
		/// <summary>This method will update the node for the specified item</summary>
		/// <param name="tmaxPickItem">The pick list item to be updated</param>
		/// <returns>True if updated</returns>
		public bool Update(CTmaxPickItem tmaxPickItem)
		{
			CTmaxPickListTreeNode tmaxNode = null;
			
			//	Get the node for this item
			if((tmaxNode = GetNode(tmaxPickItem, false)) != null)
				Update(tmaxNode);
				
			return (tmaxNode != null);

		}// public bool Update(CTmaxPickItem tmaxItem)
		
		/// <summary>This method will select the node for the specified item</summary>
		/// <param name="tmaxPickItem">The pick list item to be selected</param>
		/// <returns>True if successful</returns>
		public bool SetSelection(CTmaxPickItem tmaxPickItem)
		{
			CTmaxPickListTreeNode tmaxNode = null;
			
			//	Get the node for this item
			if((tmaxNode = GetNode(tmaxPickItem, true)) != null)
				return SetSelection(tmaxNode);
			else
				return false;

		}// public bool SetSelection(CTmaxPickItem tmaxPickItem)
		
		/// <summary>This method will expand the node for the specified item</summary>
		/// <param name="tmaxPickItem">The pick list item to be expanded</param>
		/// <returns>True if successful</returns>
		public bool Expand(CTmaxPickItem tmaxPickItem)
		{
			CTmaxPickListTreeNode tmaxNode = null;
			
			//	Get the node for this item
			if((tmaxNode = GetNode(tmaxPickItem, true)) != null)
				tmaxNode.Expanded = true;
			
			return (tmaxNode != null);

		}// public bool Expand(CTmaxPickItem tmaxPickItem)
		
		/// <summary>This method will make sure the node for the specified item is visible</summary>
		/// <param name="tmaxPickItem">The pick list item to be made visible</param>
		/// <returns>True if successful</returns>
		public bool EnsureVisible(CTmaxPickItem tmaxPickItem)
		{
			CTmaxPickListTreeNode tmaxNode = null;
			
			//	Get the node for this item
			if((tmaxNode = GetNode(tmaxPickItem, true)) != null)
				EnsureVisible(tmaxNode);
			
			return (tmaxNode != null);

		}// public bool EnsureVisible(CTmaxPickItem tmaxPickItem)
		
		/// <summary>Called to get the collection that contains the node for the specified item
		/// <param name="tmaxPickItem">The item to search for</param>
		/// <param name="bFill">True to fill the collection if necessary</param>
		/// <returns>The collection containing the node</returns>
		public CTmaxPickListTreeNode GetNode(CTmaxPickItem tmaxPickItem, bool bFill)
		{
			CTmaxPickListTreeNode tmaxParent = null;
			
			//	Does this item have a non-root parent?
			if((tmaxPickItem.Parent != null) && (tmaxPickItem.Parent.UniqueId > 0))
			{
				//	Get the node for the parent
				if((tmaxParent = GetNode(tmaxPickItem.Parent, bFill)) != null)
				{
					//	Should we fill the parent collection?
					if((tmaxParent.Nodes == null) || (tmaxParent.Nodes.Count == 0))
					{
						if(bFill == true)
							Fill(tmaxPickItem.Parent, tmaxParent);
					}
					
					//	Search for the requested node
					foreach(CTmaxPickListTreeNode O in tmaxParent.Nodes)
					{
						if(ReferenceEquals(O.PickItem, tmaxPickItem) == true)
							return O;
					}
				
				}// if((tmaxParent = GetNode(tmaxPickItem.Parent, bFill)) != null)
			
			}
			else
			{
				//	Since there is no parent this item should be in the root collection
				//
				//	NOTE:	We make no attempt to fill the root collection because it always
				//			gets filled when the owner sets the PickLists property
				if(this.Nodes != null)
				{
					foreach(CTmaxPickListTreeNode O in this.Nodes)
					{
						if(ReferenceEquals(O.PickItem, tmaxPickItem) == true)
							return O;
					}
				
				}// if(this.Nodes != null)
			
			}// if((tmaxPickItem.Parent != null) && (tmaxPickItem.Parent.UniqueId > 0))
		
			return null; // Not found
			
		}// public CTmaxPickListTreeNode GetNode(CTmaxPickItem tmaxPickItem, bool bFill)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called before a node is expanded</summary>
		/// <param name="e">The event arguments</param>
		protected override void OnBeforeExpand(Infragistics.Win.UltraWinTree.CancelableNodeEventArgs e)
		{
			CTmaxPickListTreeNode tmaxNode = null;
			
			try
			{
				while(true)
				{
					//	Get the node being expanded
					if((tmaxNode = (CTmaxPickListTreeNode)(e.TreeNode)) == null)
						break;
						
					//	Fire the event if the owner is attached.
					//
					//	NOTE:	This gives the owner the chance to fill the child collection
					if(BeforeExpand != null)
					{
						if((e.Cancel = BeforeExpand(this, tmaxNode)) == true)
							break;
					}
					
					//	Nothing to do if child collection is already populated
					if(tmaxNode.Nodes == null) break;
					if(tmaxNode.Nodes.Count > 0) break;
					
					//	Do we have any child items for this node?
					if(tmaxNode.PickItem == null) break;
					if(tmaxNode.PickItem.Children == null) break;

					//	Add a node for each child
					foreach(CTmaxPickItem O in tmaxNode.PickItem.Children)
					{
						Add(O, tmaxNode, false);
					}

					//	We're done
					break;
					
				}// while(true)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnBeforeExpand", Ex);
			}
			
			//	Do the base class processing
			base.OnBeforeExpand(e);
		
		}// protected override void OnBeforeExpand(Infragistics.Win.UltraWinTree.CancelableNodeEventArgs e)

		/// <summary>This method handles events fired by the tree control when it has expanded a node</summary>
		/// <param name="sender">The tree firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		protected override void OnAfterExpand(Infragistics.Win.UltraWinTree.NodeEventArgs e)
		{
			if((e.TreeNode != null) && (AfterExpand != null))
			{
				AfterExpand(this, (CTmaxPickListTreeNode)(e.TreeNode));
			}
			
			base.OnAfterExpand(e);
		
		}// protected override void OnAfterExpand(Infragistics.Win.UltraWinTree.NodeEventArgs e)

		/// <summary>This method handles events fired by the tree control when it is about to select a node</summary>
		/// <param name="e">The Infragistics event argument object</param>
		protected override void OnBeforeSelect(Infragistics.Win.UltraWinTree.BeforeSelectEventArgs e)
		{
			CTmaxPickListTreeNode tmaxNode = null;
			
			if((e.NewSelections != null) && (e.NewSelections.Count > 0))
				tmaxNode = ((CTmaxPickListTreeNode)e.NewSelections[0]);

			//	Fire the event
			if(BeforeSelect != null)
				e.Cancel = BeforeSelect(this, tmaxNode);
				
			//	Do the base class processing
			base.OnBeforeSelect(e);

		}// protected override void OnBeforeSelect(Infragistics.Win.UltraWinTree.BeforeSelectEventArgs e)
		
		/// <summary>This method handles events fired by the tree control when it has selected a new node(s)</summary>
		/// <param name="e">The Infragistics event argument object</param>
		protected override void OnAfterSelect(Infragistics.Win.UltraWinTree.SelectEventArgs e)
		{
			CTmaxPickListTreeNode tmaxNode = null;
			
			if((e.NewSelections != null) && (e.NewSelections.Count > 0))
				tmaxNode = ((CTmaxPickListTreeNode)e.NewSelections[0]);

			//	Fire the event
			if(AfterSelect != null)
				AfterSelect(this, tmaxNode);
				
			//	Do the base class processing
			base.OnAfterSelect(e);

		}// protected override void OnAfterSelect(Infragistics.Win.UltraWinTree.SelectEventArgs e)

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
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a new node: Name = %1  Type = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to update the pick list item: Name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the parent item type: Name = %1");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the parent node: Name = %1");

		}// protected override void SetErrorStrings()

		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>This method is called to add a a node for the specified item to the tree</summary>
		/// <param name="tmaxPickItem">The pick item to be added</param>
		/// <param name="tmaxParent">The parent node</param>
		/// <param name="bChildren">True to add the child nodes</param>
		/// <returns>The newly added node</returns>
		private CTmaxPickListTreeNode Add(CTmaxPickItem tmaxPickItem, CTmaxPickListTreeNode tmaxParent, bool bChildren)
		{
			CTmaxPickListTreeNode tmaxNode = null;
			
			Debug.Assert(tmaxPickItem != null);
			if(tmaxPickItem == null) return null;
			
			//	Create a new node for the record
			if((tmaxNode = CreateNode(tmaxPickItem)) == null) 
				return null;

			try
			{
				//	Add to the parent collection
				if(tmaxParent != null)
					tmaxParent.Nodes.Add(tmaxNode);
				else
					this.Nodes.Add(tmaxNode); // Add to the root
				
				//	Set the property values AFTER adding to the tree
				tmaxNode.SetProperties(GetImageIndex(tmaxPickItem));

				//	Now add each child if requested
				if(bChildren == true)
					Fill(tmaxPickItem, tmaxNode);
				
				return tmaxNode;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_EX, tmaxPickItem.Name, tmaxPickItem.Type), Ex);
				return null;	
			}
		
		}// private CTmaxPickListTreeNode Add(CTmaxPickItem tmaxPickItem, CTmaxPickListTreeNode tmaxParent, bool bChildren)
		
		/// <summary>This method is called to get the index of the image used to display the specified item</summary>
		/// <param name="tmaxPickItem">The item to be displayed in the tree</param>
		/// <returns>The index into the image list bound to the tree</returns>
		private int GetImageIndex(CTmaxPickItem tmaxPickItem)
		{
			//	Use the item type to select the appropriate image
			switch(tmaxPickItem.Type)
			{
				case TmaxPickItemTypes.Value:		return 3;
				case TmaxPickItemTypes.StringList:	return 2;
				case TmaxPickItemTypes.MultiLevel:	return 1;
				case TmaxPickItemTypes.Unknown:
				default:							return 0;
				
			}
			
		}// private int GetImageIndex(CTmaxPickItem tmaxPickItem)
		
		/// <summary>This method is called to create a node to represent the specified item</summary>
		/// <param name="tmaxPickItem">The pick item to be bound to the node</param>
		/// <returns>The new node if successful</returns>
		private CTmaxPickListTreeNode CreateNode(CTmaxPickItem tmaxPickItem)
		{
			CTmaxPickListTreeNode tmaxNode = null;
			
			try
			{
				//	Create the new node
				tmaxNode = new CTmaxPickListTreeNode(tmaxPickItem.Name);
				
				//	Set the properties
				tmaxNode.PickItem = tmaxPickItem;
				
				return tmaxNode;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateNode", m_tmaxErrorBuilder.Message(ERROR_CREATE_NODE_EX, tmaxPickItem.Name, tmaxPickItem.Type), Ex);
				return null;
			}
			
		}// private CTmaxPickListTreeNode CreateNode(CTmaxPickItem tmaxPickItem)
		
		/// <summary>This method will update the specified node</summary>
		/// <param name="tmaxNode">The node to be updated</param>
		private void Update(CTmaxPickListTreeNode tmaxNode)
		{
			int iImage = 0;
			
			try
			{
				if(tmaxNode.PickItem != null)
				{
					//	Has the text changed?
					if(String.Compare(tmaxNode.Text, tmaxNode.PickItem.Name) != 0)
						tmaxNode.Text = tmaxNode.PickItem.Name;
						
					//	Get the image index
					iImage = GetImageIndex(tmaxNode.PickItem);
					
					if(((int)(tmaxNode.Override.NodeAppearance.Image)) != iImage)
						tmaxNode.Override.NodeAppearance.Image = iImage;
						
				}// if(tmaxNode.PickItem != null)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Update", Ex);
			}
		
		}// private void Update(CTmaxPickTreeNode tmaxNode)
		
		/// <summary>This method is called to use the specified items to fill the specified parent's child collection</summary>
		/// <param name="tmaxParentItem">The pick item whose children are to be added</param>
		/// <param name="tmaxParentNode">The parent node</param>
		/// <returns>true if successful</returns>
		private bool Fill(CTmaxPickItem tmaxParentItem, CTmaxPickListTreeNode tmaxParentNode)
		{
			bool bSuccessful = false;
			
			Debug.Assert(tmaxParentItem != null);
			if(tmaxParentItem == null) return false;
			
			try
			{
				//	Does this item have a child collection?
				if(tmaxParentItem.Children != null)
				{
					foreach(CTmaxPickItem O in tmaxParentItem.Children)
						Add(O, tmaxParentNode, false);
						
				}// if(tmaxParentItem.Children != null)
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Fill", m_tmaxErrorBuilder.Message(ERROR_FILL_EX, tmaxParentItem.Name), Ex);
			}
			
			return bSuccessful;
		
		}// private bool Fill(CTmaxPickItem tmaxParentItem, CTmaxPickListTreeNode tmaxParentNode)
		
		#endregion Private Methods
		
		#region Properties

		/// <summary>This is the application's collection of pick lists</summary>
		public FTI.Shared.Trialmax.CTmaxPickItem PickLists
		{
			get { return m_tmaxPickLists; }
			set { SetPickLists(value); }
		}
		
		#endregion Properties
		
	}// public class CTmaxPickListTreeCtrl : CTmaxBaseCtrl
			
	/// <summary>This class represents a node displayed in a TrialMax Pick Lists tree control</summary>
	public class CTmaxPickListTreeNode : CTmaxBaseTreeNode
	{
		#region Private Members
		
		private FTI.Shared.Trialmax.CTmaxPickItem m_tmaxPickItem = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxPickListTreeNode() : base()
		{
		}
		
		/// <summary>Constructor</summary>
		/// <param name="strText">Text displayed on the node</param>
		public CTmaxPickListTreeNode(string strText) : base(strText)
		{
		}
		
		/// <summary>This method is called to set the default property values</summary>
		/// <param name="iImage">Index of the image to be applied to the node</param>
		/// <remarks>
		///	Because of the way Infragistics maintains the parent-child hiearchary
		///	this method should NOT be called until AFTER the node is added to the tree
		///	</remarks>
		public void SetProperties(int iImage)
		{
			if(Override != null)
			{
				Override.NodeAppearance.Image = iImage;
				Override.AllowAutoDragExpand = Infragistics.Win.UltraWinTree.AllowAutoDragExpand.ExpandOnDragHoverWhenExpansionIndicatorVisible;
				
				if(this.PickItem != null)
				{
					if(this.PickItem.Type == TmaxPickItemTypes.Value)
						Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.Never;
					else
						Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.CheckOnExpand;
				}
				else
				{
					Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.CheckOnExpand;
				}
				
			}// if(Override != null)
				
		}// public void SetProperties(int iImage)
		
		/// <summary>This function is called to compare this node to the specified node</summary>
		/// <param name="tmaxNode">The node to be compared</param>
		/// <param name="eField">The field to use for the comparison</param>
		/// <param name="bCaseSensitive">True if the sort should be case sensitive</param>
		/// <returns>-1 if this less than tmaxNode, 0 if equal, 1 if greater than</returns>
		public override int Compare(CTmaxBaseTreeNode tmaxNode, TmaxTreeSortFields eField,CTmaxCaseCode caseCode, bool bCaseSensitive)
		{
			CTmaxPickItem tmaxPickItem = null;
			
			//	Check for equality first
			//
			//	NOTE:	.NET raises and exception if we don't return 0 for
			//			equal objects
			if(ReferenceEquals(this, tmaxNode) == true)
				return 0;

			//	Are we sorting on user defined sort order?
			if(eField == TmaxTreeSortFields.DisplayOrder)
			{
				tmaxPickItem = ((CTmaxPickListTreeNode)tmaxNode).PickItem;
				
				if((this.PickItem != null) && (tmaxPickItem != null))
				{
					if(this.PickItem.SortOrder == tmaxPickItem.SortOrder)
						return 0;
					else if(this.PickItem.SortOrder > tmaxPickItem.SortOrder)
						return 1;
					else
						return -1;
				}
				else
				{
					return -1;
				}
				
			}
			else
			{
				//	Let the base class handle the request
				return base.Compare(tmaxNode, eField,caseCode, bCaseSensitive);
			}
		
		}//	public virtual int Compare(CTmaxBaseTreeNode tmaxNode, TmaxTreeSortFields eField, bool bCaseSensitive)

		#endregion Public Methods
		
		#region Properties
		
		/// <summary>The application pick list item bound to this node</summary>
		public FTI.Shared.Trialmax.CTmaxPickItem PickItem
		{
			get { return m_tmaxPickItem; }
			set { m_tmaxPickItem = value; }
		}
		
		/// <summary>The type of Pick Item bound to this node</summary>
		public FTI.Shared.Trialmax.TmaxPickItemTypes Type
		{
			get { return (m_tmaxPickItem != null ? m_tmaxPickItem.Type : TmaxPickItemTypes.Unknown); }
		}
		
		#endregion Properties
		
	}// public class CTmaxPickListTreeNode : CTmaxBaseTreeNode

	
	/// <summary> Objects of this class are used to manage a dynamic array of CTmaxBaseTreeNodes objects</summary>
	public class CTmaxPickListTreeNodes : CTmaxBaseTreeNodes
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxPickListTreeNodes() : base()
		{
		}

		/// <summary>Overloaded constructor</summary>
		public CTmaxPickListTreeNodes(CTmaxBaseTreeSorter tmaxSorter) : base(tmaxSorter)
		{
		}

		/// <summary>This method is called to remove the requested filter from the collection</summary>
		/// <param name="tmaxNode">The filter object to be removed</param>
		public void Remove(CTmaxPickListTreeNode tmaxNode)
		{
			// Use base class to process actual collection operation
			Remove(tmaxNode as CTmaxBaseTreeNode);
		}

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxNode">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxPickListTreeNode tmaxNode)
		{
			// Use base class to process actual collection operation
			return Contains(tmaxNode as CTmaxBaseTreeNode);
		}

		/// <summary>Gets the object at the specified index</summary>
		/// <returns>The object at the requested index</returns>
		public new CTmaxPickListTreeNode GetAt(int index)
		{
			return (base.GetAt(index) as CTmaxPickListTreeNode);
		}

		/// <summary>Gets the object that appears after the specified reference object</summary>
		/// <param name="O">The reference object</param>
		/// <returns>The next object</returns>
		public new CTmaxPickListTreeNode GetNext(object O)
		{
			return (base.GetNext(O) as CTmaxPickListTreeNode);
		}

		/// <summary>Gets the object that appears before the specified reference object</summary>
		/// <param name="O">The reference object</param>
		/// <returns>The previous object</returns>
		public new CTmaxPickListTreeNode GetPrevious(object O)
		{
			return (base.GetPrevious(O) as CTmaxPickListTreeNode);
		}

		/// <summary>Gets the first object in the collection</summary>
		/// <returns>The first object</returns>
		public new CTmaxPickListTreeNode GetFirst()
		{
			return (base.GetFirst() as CTmaxPickListTreeNode);
		}

		/// <summary>Gets the last object in the collection</summary>
		/// <returns>The last object</returns>
		public new CTmaxPickListTreeNode GetLast()
		{
			return (base.GetLast() as CTmaxPickListTreeNode);
		}

		/// <summary>Overloaded version of [] operator to return the filter object at the desired index</summary>
		/// <returns>Filter object at the specified index</returns>
		public new CTmaxPickListTreeNode this[int index]
		{
			get { return GetAt(index); }
		}

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CTmaxPickListTreeNode value)
		{
			// Find the 0 based index of the requested entry
			return base.IndexOf(value);
		}

		/// <summary>Called to locate the node bound to the specified pick list item</summary>
		/// <param name="tmaxItem">The item bound to the desired node</param>
		/// <returns>The node bound to the specified item if found</returns>
		public CTmaxPickListTreeNode Find(CTmaxPickItem tmaxItem)
		{
			foreach(CTmaxPickListTreeNode O in this)
			{
				if(ReferenceEquals(tmaxItem, O.PickItem) == true)
					return O;
			}
			
			//	Not found ...
			return null;
			
		}// public CTmaxPickListNode Find(CTmaxPickItem tmaxItem)
		
		#endregion Public Methods
		
	}//	public class CTmaxPickListTreeNodes : CTmaxBaseTreeNodes
		
}// namespace FTI.Trialmax.Controls

