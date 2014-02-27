using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections;

using Infragistics.Win;
using Infragistics.Win.UltraWinTree;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;
using FTI.Shared.Win32;

namespace FTI.Trialmax.Controls
{
	/// <summary>This is the base class for all TrialMax tree controls</summary>
	public class CTmaxBaseTreeCtrl : Infragistics.Win.UltraWinTree.UltraTree
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_TMAX_BASE_TREE_RESERVED_0	= 0;
		protected const int ERROR_TMAX_BASE_TREE_RESERVED_1	= 1;
		protected const int ERROR_TMAX_BASE_TREE_RESERVED_2	= 2;
		protected const int ERROR_TMAX_BASE_TREE_RESERVED_3	= 3;
		protected const int ERROR_TMAX_BASE_TREE_RESERVED_4	= 4;
		protected const int ERROR_TMAX_BASE_TREE_RESERVED_5	= 5;
		protected const int ERROR_TMAX_BASE_TREE_RESERVED_6	= 6;
		protected const int ERROR_TMAX_BASE_TREE_RESERVED_7	= 7;
		
		// Derived classes should start their error numbering with this value
		protected const int ERROR_TMAX_BASE_TREE_MAX		= 7;
		
		protected const int TREE_NODE_IMAGE_WIDTH  = 16;
		protected const int TREE_NODE_IMAGE_HEIGHT = 16;
		protected const int TREE_SCROLL_BAR_WIDTH  = 12;
		protected const int TREE_SCROLL_BAR_HEIGHT = 12;
			
		#endregion Constants
		
		#region Protected Members
		
		/// <summary>Local member bound to EventSource property</summary>
		protected FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Local member used to construct error messages</summary>
		protected FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();

		/// <summary>Local member bound to ClearRightClick property</summary>
		protected bool m_bClearRightClick = false;
		
		/// <summary>Local member bound to ClearLeftClick property</summary>
		protected bool m_bClearLeftClick = false;
		
		/// <summary>Local member bound to RightClickSelect property</summary>
		protected bool m_bRightClickSelect = false;
		
		#endregion Protected Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxBaseTreeCtrl()
		{
			//	Set the default event source name
			m_tmaxEventSource.Name = "Tmax Base Tree Control";
			
			//	Populate the error builder's format string collection
			SetErrorStrings();
		
		}// public CTmaxBaseTreeCtrl()
	
		/// <summary>This function is called to clear the tree</summary>
		public virtual void Clear()
		{
			try
			{
				if(this.Nodes != null)
					this.Nodes.Clear();
			}
			catch
			{
			}
			
		}// public virtual void Clear()
		
		/// <summary>This method is called to get the number of nodes that are selected</summary>
		/// <returns>The number of selected nodes</returns>
		public virtual int GetSelectionCount()
		{
			int iCount = 0;
			
			if(SelectedNodes != null)
				iCount = this.SelectedNodes.Count;
				
			return iCount;
		
		}//	public virtual int GetSelectionCount()

		/// <summary>This method is called to get the current selection</summary>
		/// <returns>The node that is currently selected</returns>
		/// <remarks>If more than one node is selected this method returns null</remarks>
		public virtual CTmaxBaseTreeNode GetSelection()
		{
			if((SelectedNodes != null) && (SelectedNodes.Count == 1))
			{
				return (CTmaxBaseTreeNode)(SelectedNodes[0]);
			}
			else
			{
				return null;
			}
		
		}//	public virtual CTmaxBaseTreeNode GetSelection()

		/// <summary>This method is called to get the current selections in the tree</summary>
		/// <returns>The nodes that are currently selected sorted by position</returns>
		public virtual CTmaxBaseTreeNodes GetSelections()
		{
			return GetTmaxNodes(SelectedNodes, true);

		}//	public virtual CTmaxBaseTreeNodes GetSelections()

		/// <summary>This method is called to get the current selections in the tree</summary>
		/// <param name="bSortByPosition">true if the nodes should be sorted based on their position in the tree</param>
		/// <returns>The nodes that are currently selected</returns>
		public virtual CTmaxBaseTreeNodes GetSelections(bool bSortByPosition)
		{
			return GetTmaxNodes(SelectedNodes, bSortByPosition);

		}//	public virtual CTmaxBaseTreeNodes GetSelections(bool bSortByPosition)

		/// <summary>This method will populate a TrialMax node collection using the specified selected node collection</summary>
		/// <param name="tmaxNodes">The collection in which to store the TrialMax nodes</param>
		/// <param name="aSelected">The Infragistics tree node collection</param>
		/// <param name="bSortByPosition">True to sort the nodes by their position in the tree</param>
		/// <returns>The populated TrialMax node collection</returns>
		public virtual CTmaxBaseTreeNodes GetTmaxNodes(CTmaxBaseTreeNodes tmaxNodes, Infragistics.Win.UltraWinTree.SelectedNodesCollection aSelected, bool bSortByPosition)
		{
			CTmaxBaseTreeSorter tmaxSorter = null;
			
			try
			{
				if((tmaxNodes != null) && (aSelected != null))
				{
					//	Are we supposed to sort ?
					if(bSortByPosition == true)
					{
						tmaxSorter = new CTmaxBaseTreeSorter();
						tmaxSorter.SortBy = TmaxTreeSortFields.TreePosition;
						tmaxSorter.Ascending = true;
						tmaxNodes.Sorter = tmaxSorter;
						tmaxNodes.KeepSorted = true;
					}
					
					foreach(CTmaxBaseTreeNode tmaxNode in aSelected)
					{
						tmaxNodes.Add(tmaxNode);
					}
					
				}// if((tmaxNodes != null) && (aSelected != null))
			
			}
			catch
			{
			}
			
			return tmaxNodes;
			
		}// public virtual CTmaxBaseTreeNodes GetTmaxNodes(CTmaxBaseTreeNodes tmaxNodes, Infragistics.Win.UltraWinTree.SelectedNodesCollection aSelected, bool bSortByPosition)
		
		/// <summary>This method will populate a TrialMax node collection using the specified selected node collection</summary>
		/// <param name="aSelected">The Infragistics tree node collection</param>
		/// <param name="bSortByPosition">True to sort the nodes by their position in the tree</param>
		/// <returns>The populated TrialMax node collection</returns>
		public virtual CTmaxBaseTreeNodes GetTmaxNodes(Infragistics.Win.UltraWinTree.SelectedNodesCollection aSelected, bool bSortByPosition)
		{
			return GetTmaxNodes(new CTmaxBaseTreeNodes(), aSelected, bSortByPosition);
		}
		
		/// <summary>This method will populate a TrialMax node collection using the specified selected node collection</summary>
		/// <param name="aSelected">The Infragistics tree node collection</param>
		/// <param name="bSortByPosition">true if the nodes should be sorted based on their position in the tree</param>
		/// <returns>The TrialMax node collection sorted by position in the tree</returns>
		public virtual CTmaxBaseTreeNodes GetTmaxNodes(Infragistics.Win.UltraWinTree.SelectedNodesCollection aSelected)
		{
			return GetTmaxNodes(aSelected, true);
		}
		
		/// <summary>
		/// This method is called to get the node located at the specified point
		/// </summary>
		/// <param name="objPoint">The point within the tree's client area</param>
		/// <returns>The node object if it exists</returns>
		public virtual CTmaxBaseTreeNode GetNode(Point objPoint)
		{
			return (CTmaxBaseTreeNode)base.GetNodeFromPoint(objPoint);
		}

		/// <summary>
		/// This method is called to get the node located at the specified coordinates
		/// </summary>
		/// <param name="iX">The x coordinate within the tree's client area</param>
		/// <param name="iY">The y coordinate within the tree's client area</param>
		/// <returns>The node object if it exists</returns>
		public virtual CTmaxBaseTreeNode GetNode(int iX, int iY)
		{
			return (CTmaxBaseTreeNode)base.GetNodeFromPoint(iX, iY);
		}
		
		/// <summary>This method is called to get the node at the current cursor position</summary>
		/// <return>The node under the current cursor position</return>
		public virtual CTmaxBaseTreeNode GetNode()
		{
			Point Pos;

			//	Get the current cursor position in client coordinates
			Pos = PointToClient(Cursor.Position);
				
			return (CTmaxBaseTreeNode)base.GetNodeFromPoint(Pos);
		}
			
		/// <summary>Selects the specified node</summary>
		/// <param name="tmaxNode">the  node to be selected)</param>
		/// <returns>true if successful</returns>
		public virtual bool SetSelection(CTmaxBaseTreeNode tmaxNode)
		{
			try
			{
				//	Clear existing selections
				if((SelectedNodes != null) && (SelectedNodes.Count > 0))
				{
					SelectedNodes.Clear();
				}
				
				if(tmaxNode != null)
					tmaxNode.Selected = true;
				
				ActiveNode = tmaxNode;
								
				return true;
			}
			catch
			{
				return false;
			}
			
		}// public virtual bool SetSelection(CTmaxBaseTreeNode tmaxNode)
		
		/// <summary>This method is called to make sure that at least part of the node is visible in the tree</summary>
		/// <param name="tmaxNode">The node that should be partially visible</param>
		///	<remarks>To make sure the whole node is visible use BringIntoView()</remarks>
		public virtual void EnsureVisible(CTmaxBaseTreeNode tmaxNode)
		{
			UltraTreeNode	node = null;
			bool			bBringIntoView = false;
			
			if(tmaxNode == null) return;
			Rectangle bounds = tmaxNode.Bounds;

			try
			{
				//	Make sure the entire parent chain has been expanded
				node = tmaxNode.Parent;
				while(node != null)
				{
					if(node.Expanded == false)
					{
						//m_tmaxEventSource.FireDiagnostic(this, "EnsureVisible", node.Text + " - expand parent chain");
						tmaxNode.BringIntoView(false);
						return;
					}
					else
					{
						node = node.Parent;
					}
					
				}

				//	Is the node over the right edge of the control?
				if(bounds.Left > (this.ClientRectangle.Right - TREE_NODE_IMAGE_WIDTH - TREE_SCROLL_BAR_WIDTH))
				{
					//m_tmaxEventSource.FireDiagnostic(this, "EnsureVisible", tmaxNode.Text + " - off to the right");
					bBringIntoView = true;
				}
				
				//	Is it over the left edge?
				else if(bounds.Right < (this.ClientRectangle.Left + TREE_NODE_IMAGE_WIDTH))
				{
					//m_tmaxEventSource.FireDiagnostic(this, "EnsureVisible", tmaxNode.Text + " - off to the left");
					bBringIntoView = true;
				}
				
					//	Is it off the top?
				else if(bounds.Bottom < (this.ClientRectangle.Top + TREE_NODE_IMAGE_HEIGHT))
				{
					//m_tmaxEventSource.FireDiagnostic(this, "EnsureVisible", tmaxNode.Text + " - off the top");
					bBringIntoView = true;
				}
				
					//	Is it off the bottom?
				else if(bounds.Top > (this.ClientRectangle.Bottom - TREE_NODE_IMAGE_HEIGHT - TREE_SCROLL_BAR_HEIGHT))
				{
					//m_tmaxEventSource.FireDiagnostic(this, "EnsureVisible", tmaxNode.Text + " - off the bottom");
					bBringIntoView = true;
				}
				
				if(bBringIntoView == true)
					tmaxNode.BringIntoView(false);
				
			}
			catch
			{
			}

		}// public virtual void EnsureVisible(CTmaxBaseTreeNode tmaxNode)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Called when the user clicks within the client area of the control</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			bool				bClearSelections = false;
			CTmaxBaseTreeNode	tmaxNode = null;
			
			//	Get the node under the current location
			tmaxNode = GetNode(e.X, e.Y);
			
			if(tmaxNode != null)
			{
				if((e.Button == MouseButtons.Right) && (m_bRightClickSelect == true))
				{
					//	Don't do anything if the user is pressing the Control key
					if((User.GetKeyState(User.VK_CONTROL) & 0x8000) != 0) return;
				
					//	Don't do anything if pressing the Shift key
					if((User.GetKeyState(User.VK_SHIFT) & 0x8000) != 0) return;
				
					//	Make sure the node under the mouse is selected
					if(tmaxNode.Selected == false)
					{
						SetSelection(tmaxNode);
					}

				}
			}
			else
			{
				//	Should we clear the current selection(s)?
				if(e.Button == MouseButtons.Right)
					bClearSelections = m_bClearRightClick;
				else
					bClearSelections = m_bClearLeftClick;
	
				//	Should we clear the current selections?
				if(bClearSelections == true)
				{
					PerformAction(UltraTreeAction.ToggleSelection, false, false);
					if((SelectedNodes != null) && (SelectedNodes.Count > 0))
					{
						SelectedNodes.Clear();
								
						//							if((tmaxActive = (CTmaxBaseTreeNode)ActiveNode) != null)
						//							{
						//								tmaxActive.
						//							Refresh();
					}
					
				}// if(bClearSelections == true)
				
			}
			
			//	Do the base class processing
			base.OnMouseDown(e);
		
		}// protected override void OnMouseDown(MouseEventArgs e)

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected virtual void SetErrorStrings()
		{
			Debug.Assert(m_tmaxErrorBuilder != null);
			Debug.Assert(m_tmaxErrorBuilder.FormatStrings != null);
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;

			//	Add placeholders for the reserved strings
			m_tmaxErrorBuilder.FormatStrings.Add("CTmaxBaseTreeCtrl reserved base class error #0");
			m_tmaxErrorBuilder.FormatStrings.Add("CTmaxBaseTreeCtrl reserved base class error #1");
			m_tmaxErrorBuilder.FormatStrings.Add("CTmaxBaseTreeCtrl reserved base class error #2");
			m_tmaxErrorBuilder.FormatStrings.Add("CTmaxBaseTreeCtrl reserved base class error #3");
			m_tmaxErrorBuilder.FormatStrings.Add("CTmaxBaseTreeCtrl reserved base class error #4");
			m_tmaxErrorBuilder.FormatStrings.Add("CTmaxBaseTreeCtrl reserved base class error #5");
			m_tmaxErrorBuilder.FormatStrings.Add("CTmaxBaseTreeCtrl reserved base class error #6");
			m_tmaxErrorBuilder.FormatStrings.Add("CTmaxBaseTreeCtrl reserved base class error #7");
			
		}// protected void SetErrorStrings()
		
		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}
		
		/// <summary>True to clear the selection when the user right clicks on whitespace</summary>
		public bool ClearRightClick
		{
			get { return m_bClearRightClick; }
			set { m_bClearRightClick = value; }
		}
		
		/// <summary>True to clear the selection when the user left clicks on whitespace</summary>
		public bool ClearLeftClick
		{
			get { return m_bClearLeftClick; }
			set { m_bClearLeftClick = value; }
		}
		
		/// <summary>True to select the node with a right click</summary>
		public bool RightClickSelect
		{
			get { return m_bRightClickSelect; }
			set { m_bRightClickSelect = value; }
		}
		
		#endregion Properties

	}// protected virtual void SetErrorStrings()

	/// <summary>Implements node object used to add nodes to a TrialMax tree control</summary>
	public class CTmaxBaseTreeNode : Infragistics.Win.UltraWinTree.UltraTreeNode
	{
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxBaseTreeNode() : base()
		{
		
		}// public CTmaxBaseTreeNode() : base()
		
		/// <summary>Default constructor</summary>
		/// <param name="strText">Text displayed on the node</param>
		///
		/// <remarks>
		/// We intentionally supply an empty string for the key because we don't care
		/// if the value is unique and it signficantly speeds up the processing
		/// </remarks>
		public CTmaxBaseTreeNode(string strText) : base(string.Empty, strText)
		{
		}
		
		/// <summary>Clears this node's child collections</summary>
		public virtual void Clear()
		{
			if(Nodes != null)
				Nodes.Clear();
				
			if(Expanded == true)
				Expanded = false;
				
		}// public virtual void Clear()
		
		/// <summary>This function is called to compare this node to the specified node</summary>
		/// <param name="tmaxNode">The node to be compared</param>
		/// <param name="eField">The field to use for the comparison</param>
		/// <param name="bCaseSensitive">True if the sort should be case sensitive</param>
		/// <returns>-1 if this less than tmaxNode, 0 if equal, 1 if greater than</returns>
		public virtual int Compare(CTmaxBaseTreeNode tmaxNode, TmaxTreeSortFields eField,CTmaxCaseCode caseCode, bool bCaseSensitive)
		{
			//	Check for equality first
			//
			//	NOTE:	.NET raises and exception if we don't return 0 for
			//			equal objects
			if(ReferenceEquals(this, tmaxNode) == true)
				return 0;

			//	Field are we sorting on?
			switch(eField)
			{
				case TmaxTreeSortFields.TreePosition:
						
					if(this.Index == tmaxNode.Index)
						return 0;
					else if(this.Index > tmaxNode.Index)
						return 1;
					else
						return -1;
							
				case TmaxTreeSortFields.Text:
				default:

					return CTmaxToolbox.Compare(this.Text, tmaxNode.Text, !bCaseSensitive);
							
			}// switch(eField)
		
		}//	public virtual int Compare(CTmaxBaseTreeNode tmaxNode, TmaxTreeSortFields eField, bool bCaseSensitive)

		#endregion Public Methods
		
	}// class CTmaxBaseTreeNode

	/// <summary> Objects of this class are used to manage a dynamic array of CTmaxBaseTreeNodes objects</summary>
	public class CTmaxBaseTreeNodes : FTI.Shared.Trialmax.CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxBaseTreeNodes() : base()
		{
		}

		/// <summary>Overloaded constructor</summary>
		public CTmaxBaseTreeNodes(CTmaxBaseTreeSorter tmaxSorter) : base(tmaxSorter as IComparer)
		{
		}

		/// <summary>This method is called to remove the requested filter from the collection</summary>
		/// <param name="tmaxNode">The filter object to be removed</param>
		public void Remove(CTmaxBaseTreeNode tmaxNode)
		{
			try
			{
				// Use base class to process actual collection operation
				Remove(tmaxNode as object);
			}
			catch
			{
			}
		
		}// public void Remove(CTmaxBaseTreeNode tmaxNode)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxNode">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxBaseTreeNode tmaxNode)
		{
			// Use base class to process actual collection operation
			return Contains(tmaxNode as object);
		
		}// public bool Contains(CTmaxBaseTreeNode tmaxNode)

		/// <summary>Gets the object at the specified index</summary>
		/// <returns>The object at the requested index</returns>
		public new CTmaxBaseTreeNode GetAt(int index)
		{
			return (base.GetAt(index) as CTmaxBaseTreeNode);
		}

		/// <summary>Gets the object that appears after the specified reference object</summary>
		/// <param name="O">The reference object</param>
		/// <returns>The next object</returns>
		public new CTmaxBaseTreeNode GetNext(object O)
		{
			return (base.GetNext(O) as CTmaxBaseTreeNode);
		}

		/// <summary>Gets the object that appears before the specified reference object</summary>
		/// <param name="O">The reference object</param>
		/// <returns>The previous object</returns>
		public new CTmaxBaseTreeNode GetPrevious(object O)
		{
			return (base.GetPrevious(O) as CTmaxBaseTreeNode);
		}

		/// <summary>Gets the first object in the collection</summary>
		/// <returns>The first object</returns>
		public new CTmaxBaseTreeNode GetFirst()
		{
			return (base.GetFirst() as CTmaxBaseTreeNode);
		}

		/// <summary>Gets the last object in the collection</summary>
		/// <returns>The last object</returns>
		public new CTmaxBaseTreeNode GetLast()
		{
			return (base.GetLast() as CTmaxBaseTreeNode);
		}

		/// <summary>Overloaded version of [] operator to return the filter object at the desired index</summary>
		/// <returns>Filter object at the specified index</returns>
		public new CTmaxBaseTreeNode this[int index]
		{
			get { return GetAt(index); }
		}

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CTmaxBaseTreeNode value)
		{
			// Find the 0 based index of the requested entry
			return base.IndexOf(value);
		}

		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Sorter used to sort the nodes in the collection</summary>
		///
		///	<remarks>This property allows the owner to reference the base class Comparer property as a tree sorter object</remarks>
		public CTmaxBaseTreeSorter Sorter
		{
			get
			{
				return (Comparer as CTmaxBaseTreeSorter);
			}
			set
			{
				Comparer = value;
			}
		
		}// Sorter property
		
		#endregion Properties
		
	}//	public class CTmaxBaseTreeNodes
		
	/// <summary>
	/// Objects of this class are used to draw the drag/drop highlights in a CTmaxTree object
	/// </summary>
	public class CTmaxBaseTreeFilter : Infragistics.Win.IUIElementDrawFilter
	{
		#region Private Members
		
		/// <summary>Local member bound to DropHighlightBackColor property</summary>
		private System.Drawing.Color m_DropHighlightBackColor; 

		/// <summary>Local member bound to DropHighlightForeColor property</summary>
		private System.Drawing.Color m_DropHighlightForeColor; 

		/// <summary>Local member bound to DropLineColor property</summary>
		private System.Drawing.Color  m_DropLineColor;

		/// <summary>Local member bound to DropHighlightNode property</summary>
		private CTmaxBaseTreeNode m_DropHighlightNode;
		
		/// <summary>Local member bound to DropLinePosition property</summary>
		private TmaxTreePositions m_eDropLinePosition;

		/// <summary>Local member bound to DropLineWidth property</summary>
		private int m_iDropLineWidth;

		/// <summary>Local member bound to EdgeSensitivity property</summary>
		private int m_iEdgeSensitivity;
		
		#endregion Private Members
		
		#region Private Methods
		
		/// <summary>This method initializes the properties to their default values</summary>
		private void Initialize()
		{
			m_DropHighlightNode = null;
			m_eDropLinePosition = TmaxTreePositions.None;
			m_DropHighlightBackColor = System.Drawing.SystemColors.Highlight;
			m_DropHighlightForeColor = System.Drawing.SystemColors.HighlightText;
			m_DropLineColor = System.Drawing.SystemColors.ControlText;
			m_iEdgeSensitivity = 0;
			m_iDropLineWidth = 2;
		
		}//	Initialize()
		
		/// <summary>
		/// This method is called by the Infragistics library to determine when it should call
		/// the DrawElement() method during the drawing of an element in the tree
		/// </summary>
		/// <returns></returns>
		Infragistics.Win.DrawPhase Infragistics.Win.IUIElementDrawFilter.GetPhasesToFilter(ref Infragistics.Win.UIElementDrawParams drawParams) 
		{
			//	We only want to be called right before and right after drawing the node
			return Infragistics.Win.DrawPhase.AfterDrawElement | Infragistics.Win.DrawPhase.BeforeDrawElement;
		}

		/// <summary>
		/// This method is called by Infragistics before it starts drawing a node and after it
		///	finishes drawing a node
		/// </summary>
		/// <param name="drawPhase">Infragistics DrawPhase enumeration to specify where we are in the drawing process</param>
		/// <param name="drawParams">Parameters required to draw the element</param>
		/// <returns>true to inhibit default drawing of the specified phase</returns>
		bool Infragistics.Win.IUIElementDrawFilter.DrawElement(Infragistics.Win.DrawPhase drawPhase, ref Infragistics.Win.UIElementDrawParams drawParams)
		{
			//	Perform the default processing if we don't have a highlight node or if the
			//	owner has not registered for the QueryStateAllowed event
			if((this.QueryStateAllowed == null) || (m_DropHighlightNode == null) || (m_eDropLinePosition == TmaxTreePositions.None))
			{
				return false;
			}

			// Create and initialize a new CQueryStateAllowedArgs object
			CQueryStateAllowedArgs eArgs = new CQueryStateAllowedArgs();
			eArgs.m_objNode = m_DropHighlightNode;
			eArgs.m_eDropLinePosition = this.m_eDropLinePosition;
			eArgs.m_eStatesAllowed = TmaxTreePositions.All; //	Default is to allow all states

			//	Fire the event
			this.QueryStateAllowed(this, eArgs);

			//	Stop here if the owner does not allow drawing in the current position
			if((eArgs.m_eStatesAllowed & m_eDropLinePosition) != m_eDropLinePosition)
			{
				return false;
			}

			//	Determine which drawing phase we are in. 
			switch(drawPhase)
			{
				case Infragistics.Win.DrawPhase.BeforeDrawElement:
				{
					OnBeforeDrawElement(ref drawParams);
					break;
				}
				case Infragistics.Win.DrawPhase.AfterDrawElement:
				{
					OnAfterDrawElement(ref drawParams);
					break;
				}				
			}
			
			//	Allow default drawing to occur
			return false;
		}

		/// <summary>
		/// This method is called to update the highlight node and/or position
		/// </summary>
		/// <param name="objNode">Node object to be highlighted</param>
		/// <param name="ePosition">Position of the cursor relative to the node</param>
		/// <returns>True if the node and/or position has changed</returns>
		private bool Update(CTmaxBaseTreeNode objNode , TmaxTreePositions ePosition)
		{
			bool bChanged = true;

			if(objNode != null)
			{
				//	Has the drop node stayed the same?
				if((m_DropHighlightNode != null) ||
					(ReferenceEquals(m_DropHighlightNode, objNode) == true))
				{
					bChanged = (ePosition == m_eDropLinePosition);
				}
			
			}
			else
			{
				if(m_DropHighlightNode == null)
					bChanged = (ePosition == m_eDropLinePosition);
				
			}// if(objNode != null)
			
			//	Update the properties
			m_DropHighlightNode = objNode;
			m_eDropLinePosition = ePosition;
			
			return bChanged;
		
		}// Update(CTmaxBaseTreeNode objNode , DropLinePositions ePosition)

		/// <summary>
		/// This method handles notifications recieved when the control is about to draw a node
		/// </summary>
		/// <param name="drawParams">Parameters required to draw the element</param>
		private void OnBeforeDrawElement(ref Infragistics.Win.UIElementDrawParams drawParams)
		{
			UIElement		objUIElement;
			UltraTreeNode	objNode;

			//	We have nothing to do unless we are actually on the node
			if((m_eDropLinePosition & TmaxTreePositions.OnNode) != TmaxTreePositions.OnNode) return;
		
			//	Get the element being drawn
			if((objUIElement = drawParams.Element) == null) return;

			//	Are we drawing text?
			if(objUIElement.GetType() == typeof(Infragistics.Win.UltraWinTree.NodeTextUIElement))
			{
				//	Get the node attached to this user interface element
				objNode = (UltraTreeNode)objUIElement.GetContext(typeof(UltraTreeNode));

				//	Is this the drop highlight?
				if(ReferenceEquals(objNode, m_DropHighlightNode))
				{
					//	Set the ForeColor and Backcolor of the node 
					//	
					//	NOTE:	AppearanceData only affects the node for this one paint. It will not
					//			change any properties of the node
					drawParams.AppearanceData.BackColor = m_DropHighlightBackColor;
					drawParams.AppearanceData.ForeColor = m_DropHighlightForeColor;
				}
			}

		}// OnBeforeDrawElement()

		/// <summary>
		/// This method handles notifications recieved when the control after it draws a node
		/// </summary>
		/// <param name="drawParams">Parameters required to draw the element</param>
		private void OnAfterDrawElement(ref Infragistics.Win.UIElementDrawParams drawParams)
		{
			NodeSelectableAreaUIElement	objArea;
			UIElement					objUIElement;
			UltraTree					objTree;
			Pen							objPen;
			Graphics					objGraphics;
			int							iLeft;
			int							iRight;
			int							iY;
			bool						bAbove;
			
			//	Do we have a valid drop node?
			if(m_DropHighlightNode == null) return;
			
			//	Are we above the node?
			if((m_eDropLinePosition & TmaxTreePositions.AboveNode) == TmaxTreePositions.AboveNode)
			{
				bAbove = true;
			}
				//	Are we below the node?
			else if((m_eDropLinePosition & TmaxTreePositions.BelowNode) == TmaxTreePositions.BelowNode)
			{
				bAbove = false;
			}
			else
			{
				//	We only care if we are above or below
				return;
			}
			
			//	Get the element being drawn
			if((objUIElement = drawParams.Element) == null) return;

			//	Are we drawing a tree element?
			if(objUIElement.GetType() != typeof(UltraTreeUIElement)) return;
			
			//	Create a pen to draw the drop line
			objPen = new System.Drawing.Pen(m_DropLineColor, m_iDropLineWidth);

			//	Get a reference to the graphics object we are drawing in
			objGraphics = drawParams.Graphics;

			if((objPen == null) || (objGraphics == null)) return;
			
			//	Get the NodeSelectableAreaUIElement for the current DropNode. We will use 
			//	this for positioning and sizing the DropLine
			if((objArea = (NodeSelectableAreaUIElement)drawParams.Element.GetDescendant(typeof(NodeSelectableAreaUIElement), (UltraTreeNode)m_DropHighlightNode)) == null) return;

			//	Get the control reference
			if((objTree = (UltraTree)objArea.GetContext(typeof(UltraTree))) == null) return;
			
			//	Get the left and right coordinates
			iLeft  = objArea.Rect.Left - 4;
			iRight = objTree.DisplayRectangle.Right - 4;

			//	Are we above the node?
			if(bAbove == true)
			{
				//	Draw line above node
				iY = m_DropHighlightNode.Bounds.Top;
				objGraphics.DrawLine(objPen, iLeft, iY, iRight, iY);
				objPen.Width = 1;
				objGraphics.DrawLine(objPen, iLeft, iY - 3, iLeft, iY + 2);
				objGraphics.DrawLine(objPen, iLeft + 1, iY - 2, iLeft + 1, iY + 1);
				objGraphics.DrawLine(objPen, iRight, iY - 3, iRight, iY + 2);
				objGraphics.DrawLine(objPen, iRight - 1, iY - 2, iRight - 1, iY + 1);
			}
			else
			{
				//	Draw Line below node
				iY = m_DropHighlightNode.Bounds.Bottom;
				objGraphics.DrawLine(objPen, iLeft, iY, iRight, iY);
				objPen.Width = 1;
				objGraphics.DrawLine(objPen, iLeft, iY - 3, iLeft, iY + 2);
				objGraphics.DrawLine(objPen, iLeft + 1, iY - 2, iLeft + 1, iY + 1);
				objGraphics.DrawLine(objPen, iRight, iY - 3, iRight, iY + 2);
				objGraphics.DrawLine(objPen, iRight - 1, iY - 2, iRight - 1, iY + 1);
			}

		}//	OnAfterDrawElement()

		#endregion Private Methods
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxBaseTreeFilter()
		{
			//	Initialize the properties to the defaults
			Initialize();
		
		}//	CTmaxBaseTreeFilter()
		
		/// <summary>Perform cleanup</summary>
		public void Dispose()
		{
			m_DropHighlightNode = null;
		
		}// Dispose()

		/// <summary>This method is called to clear the current drop highlight</summary>
		public void ClearDropHighlight()
		{
			Update(null, TmaxTreePositions.None);
		
		}// ClearDropHighlight()

		/// <summary>
		/// This method is called to set the current drop highlight node
		/// </summary>
		/// <param name="objNode">Node object to be highlighted</param>
		/// <param name="objPoint">Current mouse position in tree control coordinates</param>
		///	<returns>True if the node and/or line position has changed</returns>
		/// <remarks>This method should be called from the form's DragOver event</remarks>
		///	<remarks>The cursor position MUST be specified in the tree control's client coordinates</remarks>
		public bool SetDropHighlightNode(CTmaxBaseTreeNode objNode, System.Drawing.Point objPoint)
		{
			int					iDistanceFromEdge; 
			TmaxTreePositions	eNewPosition = TmaxTreePositions.None;

			if(objNode != null)
			{
				//	How far do we have to be from the edge?
				if(m_iEdgeSensitivity > 0)
				{
					iDistanceFromEdge = m_iEdgeSensitivity;
				}
				else
				{
					//	Use the default value
					iDistanceFromEdge = objNode.Bounds.Height / 3;
				}

				//	Determine which part of the node the point is in
				if(objPoint.Y < (objNode.Bounds.Top + iDistanceFromEdge))
				{
					//	Point is in the top of the node
					eNewPosition = TmaxTreePositions.AboveNode;
				}
				else
				{
					if(objPoint.Y > (objNode.Bounds.Bottom - iDistanceFromEdge))
					{
						//	Point is in the bottom of the node
						eNewPosition = TmaxTreePositions.BelowNode;
					}
					else
					{
						//	Point is in the middle of the node
						eNewPosition = TmaxTreePositions.OnNode;
					}
				}
			}

			//	Process the new position
			try
			{
				return Update(objNode, eNewPosition);
			}
			catch
			{
				return true;
			}
		
		}// public bool SetDropHighlightNode(CTmaxBaseTreeNode objNode, System.Drawing.Point objPoint)

		#endregion Public Methods
		
		#region Properties
		
		/// <summary>This property exposes the node the mouse is currently position on</summary>
		public CTmaxBaseTreeNode DropHighlightNode
		{
			get
			{
				return m_DropHighlightNode;
			}
			set
			{
				//	Has the node changed?
				if(ReferenceEquals(m_DropHighlightNode, value) == false)
				{	
					m_DropHighlightNode = value;
				}
			}
			
		}// DropHighlightNode property

		/// <summary>The background color of the drop highlight node</summary>
		/// <remarks>This only affects the node when it is being dropped On, not above or below</remarks> 
		public System.Drawing.Color DropHighlightBackColor
		{
			get
			{
				return m_DropHighlightBackColor;
			}
			set
			{
				m_DropHighlightBackColor = value;
			}
		}
		
		/// <summary>The foreground color of the drop highlight node</summary>
		/// <remarks>This only affects the node when it is being dropped On, not above or below</remarks> 
		public System.Drawing.Color DropHighlightForeColor
		{
			get
			{
				return m_DropHighlightForeColor;
			}
			set
			{
				m_DropHighlightForeColor = value;
			}
		}
		
		/// <summary>The color of the drop insertion indicator line</summary>
		public System.Drawing.Color DropLineColor
		{
			get
			{
				return m_DropLineColor;
			}
			set
			{
				m_DropLineColor = value;
			}
		}

		/// <summary>Position at which to draw the drop line indicator</summary>
		public TmaxTreePositions DropLinePosition
		{
			get
			{
				return m_eDropLinePosition;
			}
			set
			{
				//	Has the position changed?
				if(m_eDropLinePosition != value)
				{
					m_eDropLinePosition = value;
				}
			}
		}

		/// <summary>Width in pixels of the drop line indicator</summary>
		public int DropLineWidth
		{
			get
			{
				return m_iDropLineWidth;
			}
			set
			{
				m_iDropLineWidth = value;
			}
		}

		/// <summary>
		///	Determines how close to the top or bottom edge of a node
		///	the mouse must be to be consider dropping Above or Below
		///	respectively. 		
		/// </summary>
		/// <remarks>
		///	By default the top 1/3 of the node is Above, the bottom 1/3
		///	is Below, and the middle is On. 
		/// </remarks>
		public int EdgeSensitivity
		{
			get
			{
				return m_iEdgeSensitivity;
			}
			set
			{
				m_iEdgeSensitivity = value;
			}
		}

		#endregion Properties
		
		#region Delegates / Events
		
		/// <summary>This is the delegate for QueryStateAllowed events</summary>
		public event QueryStateAllowedHandler QueryStateAllowed;
		
		/// <summary>This event is fired to determine the drawing states allowed for the node</summary>
		public delegate void QueryStateAllowedHandler(object sender, CQueryStateAllowedArgs e);

		/// <summary>
		/// This class is used to pass arguements in QueryStateAllowed events
		/// </summary>
		public class CQueryStateAllowedArgs : System.EventArgs
		{
			public CTmaxBaseTreeNode m_objNode;
			public TmaxTreePositions m_eDropLinePosition;
			public TmaxTreePositions m_eStatesAllowed;
		}

		#endregion Delegates / Events
	
	}// class CTmaxBaseTreeFilter
	
	/// <summary>
	/// Objects of this class are used to sort nodes in the tree
	/// </summary>
	public class CTmaxBaseTreeSorter : IComparer
	{
		#region Private Members
		
		/// <summary>Local member bound to SortBy property</summary>
		private TmaxTreeSortFields m_eSortBy = TmaxTreeSortFields.Text;

        /// <summary>Local member bound to SortBy property</summary>
        private CTmaxCaseCode m_CaseCode;

		/// <summary>Local member bound to CaseSensitive property</summary>
		private bool m_bCaseSensitive = false;

		/// <summary>Local member bound to Ascending property</summary>
		private bool m_bAscending = true;

		/// <summary>Local member bound to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Local member bound to Comparisons property</summary>
		private long m_lComparisons = 0;

		#endregion Private Members
		
		#region Private Methods
		

		#endregion Private Methods
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxBaseTreeSorter()
		{
			//	Set the default event source name
			m_tmaxEventSource.Name = "Tree Sorter";
		}
		
		/// <summary>Copy constructor</summary>
		public CTmaxBaseTreeSorter(CTmaxBaseTreeSorter tmaxSorter)
		{
			Copy(tmaxSorter);
		}
		
		/// <summary>
		/// This method is called to compare determine if the specified object equals this object
		/// </summary>
		/// <param name="O">The object to be compared</param>
		/// <returns>true if both objects are equivalent</returns>
		public override bool Equals(object O)
		{
			CTmaxBaseTreeSorter tmaxSorter = null;
			
			//	Are the object types a match?
			if((O == null) || (GetType() != O.GetType())) 
				return false;
			else
				tmaxSorter = (CTmaxBaseTreeSorter)O;
				
			if(m_eSortBy != tmaxSorter.SortBy)
				return false;
			else if(m_bAscending != tmaxSorter.Ascending)
				return false;
			else if(m_bCaseSensitive != tmaxSorter.CaseSensitive)
				return false;
			else
				return true;	//	All values are equal
			
		}// public bool Equals(object obj)
		
		/// <summary>
		/// Overloaded equality operator
		/// </summary>
		/// <param name="tmaxSorter1">First object being compared</param>
		/// <param name="tmaxSorter2">Second object being compared</param>
		/// <returns>true if both objects are equal</returns>
		public static bool operator == (CTmaxBaseTreeSorter tmaxSorter1, CTmaxBaseTreeSorter tmaxSorter2)
		{
			return tmaxSorter1.Equals(tmaxSorter2);
		}
		
		/// <summary>
		/// Overloaded inequality operator
		/// </summary>
		/// <param name="tmaxSorter1">First object being compared</param>
		/// <param name="tmaxSorter2">Second object being compared</param>
		/// <returns>true if both objects are not equal</returns>
		public static bool operator != (CTmaxBaseTreeSorter tmaxSorter1, CTmaxBaseTreeSorter tmaxSorter2)
		{
			return !tmaxSorter1.Equals(tmaxSorter2);
		}
		
		/// <summary>
		/// This method is called to compare the local property values to the values assigned to the specified sorter object
		/// </summary>
		/// <param name="tmaxSorter">The object to be compared</param>
		/// <returns>true if both objects are equivalent</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
			
			
		/// <summary>
		/// This method is called to copy the properties of the specified sorter
		/// </summary>
		/// <param name="tmaxSorter">The source object to be copied</param>
		public void Copy(CTmaxBaseTreeSorter tmaxSorter)
		{
			m_bAscending = tmaxSorter.Ascending;
			m_bCaseSensitive = tmaxSorter.CaseSensitive;
			m_eSortBy = tmaxSorter.SortBy;
            m_CaseCode = tmaxSorter.m_CaseCode;
		    CaseCodeId = tmaxSorter.CaseCodeId;
		}
		
		/// <summary>
		/// This function is called to compare two nodes in the tree
		/// </summary>
		/// <param name="x">First node to be compared</param>
		/// <param name="y">Second node to be compared</param>
		/// <returns>-1 if x less than y, 0 if equal, 1 if x greater than y</returns>
		int IComparer.Compare(object x, object y) 
		{
			return Compare((CTmaxBaseTreeNode)x, (CTmaxBaseTreeNode)y);
		}



		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This function is called to compare two nodes in the tree</summary>
		/// <param name="tmaxNode1">First node to be compared</param>
		/// <param name="tmaxNode2">Second node to be compared</param>
		/// <returns>-1 if tmaxNode1 less than tmaxNode2, 0 if equal, 1 if greater than</returns>
		protected virtual int Compare(CTmaxBaseTreeNode tmaxNode1, CTmaxBaseTreeNode tmaxNode2)
		{
			int iReturn = -1;
			
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
					iReturn = tmaxNode1.Compare(tmaxNode2, m_eSortBy,m_CaseCode, m_bCaseSensitive);
				}
		
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Compare", "Exception:", Ex);
			}
			
			//	Do we need to reverse the meaning?
			if(m_bAscending == false)
				iReturn *= -1;

			// Bump the counter
			try { m_lComparisons++; }
			catch { m_lComparisons = 0; }
				
			return iReturn;
		
		}//protected virtual int Compare(CTmaxBaseTreeNode tmaxNode1, CTmaxBaseTreeNode tmaxNode2)
		
		#endregion Protected Methods
		
		#region Properties

		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}

		/// <summary>Field on which to sort the nodes</summary>
		public TmaxTreeSortFields SortBy
		{
			get { return m_eSortBy; }
			set { m_eSortBy = value; }
		}

        /// <summary>Field store CaseCodeId on which sort action performed </summary>
        public long CaseCodeId
        {
            get; set;
        }

	    public CTmaxCaseCode CaseCode
	    {
            get { return m_CaseCode; }
            set { m_CaseCode = value; }
	    }

        /// <summary>
        /// CaseCode if SortBy Fileded Data
        /// </summary>
        //public CTmaxCaseCode CaseCode
        //{
	        
        //}

		/// <summary>Case sensitive text comparisons</summary>
		public bool CaseSensitive
		{
			get { return m_bCaseSensitive; }
			set { m_bCaseSensitive = value; }
		}

		/// <summary>Sort in ascending order</summary>
		/// <remarks>
		///	We don't actually use this flag to process the comparison. It is
		///	assumed the owner of the tree has set the Overide.SortType property
		/// </remarks>
		public bool Ascending
		{
			get { return m_bAscending; }
			set { m_bAscending = value; }
		}

		/// <summary>Used for testing to monitor the number of comparisons</summary>
		public long Comparisons
		{
			get { return m_lComparisons; }
			set { m_lComparisons = value; }
		}

		#endregion Properties
		
	}// class CTmaxBaseTreeSorter
	
	/// <summary>Bitfield enumerations to define the drop line position</summary>
	[System.Flags] public enum TmaxTreePositions
	{
		None = 0,
		OnNode = 1,
		AboveNode = 2,
		BelowNode = 4,
		All = OnNode | AboveNode | BelowNode
	}
	
	/// <summary>These enumerations control the fields that nodes in the tree can be sorted on</summary>
	public enum TmaxTreeSortFields
	{
		DatabaseId = 0,
		Text,
		Created,
		Modified,
		DisplayOrder,
		TreePosition,
        MediaId,
        MediaName,
        FiledData
	}
		
}// namespace FTI.Trialmax.Controls
