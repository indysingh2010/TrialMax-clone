using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared.Trialmax;
using FTI.Shared.Xml;
using FTI.Trialmax.Controls;
using FTI.Trialmax.Forms;
using FTI.Trialmax.Database;

using Infragistics.Win.UltraWinTree;
using Infragistics.Win.UltraWinDock;

namespace FTI.Trialmax.Panes
{
	/// <summary>
	/// This class maintains the relationship between a media type node, it's node in the tree,
	/// and it's collection of primary nodes in the tree
	/// </summary>
	public class CMediaNode
	{
		#region Private Members
		
		/// <summary>Local member bound to Tree property</summary>
		protected CMediaTree m_ctrlTree = null;
	
		/// <summary>Local member bound to MediaType property</summary>
		protected FTI.Shared.Trialmax.TmaxMediaTypes m_eMediaType = TmaxMediaTypes.Unknown;
	
		/// <summary>Local member bound to Primaries property</summary>
		protected FTI.Trialmax.Controls.CTmaxMediaTreeNodes m_tmaxPrimaries = new CTmaxMediaTreeNodes();
		
		/// <summary>Local member bound to SuperNodes property</summary>
		protected FTI.Trialmax.Controls.CTmaxMediaTreeNodes m_tmaxSuperNodes = new CTmaxMediaTreeNodes();
		
		/// <summary>Local member bound to Node property</summary>
		protected FTI.Trialmax.Controls.CTmaxMediaTreeNode m_tmaxNode = null;

		/// <summary>Local member bound to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new FTI.Shared.Trialmax.CTmaxEventSource();

		/// <summary>Local member bound to SuperNodeSize property</summary>
		protected int m_iSuperNodeSize = 0;

		#endregion Private Members
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CMediaNode()
		{
			Initialize();
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="ctrlTree">The physical tree pane that contains the node</param>
		///	<param name="eType">The node's primary media type identifier</param>
		public CMediaNode(TmaxMediaTypes eType)
		{
			MediaType = eType;
			Initialize();
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="ctrlTree">The physical tree pane that contains the node</param>
		///	<param name="eType">The node's primary media type identifier</param>
		public CMediaNode(CMediaTree ctrlTree, TmaxMediaTypes eType)
		{
			Tree = ctrlTree;
			MediaType = eType;
			Initialize();
		}
		
		/// <summary>Overloaded constructor</summary>
		/// <param name="ctrlTree">The physical tree pane that contains the node</param>
		public CMediaNode(CMediaTree ctrlTree)
		{
			Tree = ctrlTree;
			Initialize();
		}
		
		/// <summary>This method is called to prepare the node for loading a batch of primary records</summary>
		/// <returns>true if successful</returns>
		public bool PreLoad()
		{
			//	Clear the existing nodes
			Clear();
			
			//	Turn off sorting until we've loaded the records
			m_tmaxPrimaries.KeepSorted = false;
			
			return true;				
		}
		
		/// <summary>This method is called load the entire collection of primary media records</summary>
		/// <returns>true if successful</returns>
		/// <remarks>It is assumed the parent tree pane has populated the primary collection</remarks>
		public bool Load()
		{
			int	iSuper = -1;

			//	Sort the records
			//m_tmaxEventSource.InitElapsed();

			m_tmaxPrimaries.Sorter.Comparisons = 0;

			m_tmaxPrimaries.Sort();
			m_tmaxPrimaries.KeepSorted = true;

			//if(this.MediaType == TmaxMediaTypes.Document)
				//m_tmaxEventSource.FireElapsed(this, "Load", "Time to sort " + m_tmaxPrimaries.Count.ToString() + " " + m_tmaxNode.MediaType.ToString() + " nodes [" + m_tmaxPrimaries.Sorter.Comparisons.ToString() + " comparisons] : ");

			//	Create the appropriate number of super nodes
			SetSuperNodes();
			
			//m_tmaxEventSource.InitElapsed();
				
			//	Line up on the first super node
			if((m_tmaxSuperNodes != null) && (m_tmaxSuperNodes.Count > 0))
				iSuper = 0;
			
			//	Now add each node to the tree
			foreach(CTmaxMediaTreeNode tmaxNode in m_tmaxPrimaries)
			{
				//	Are we using super nodes?
				if(iSuper >= 0)
				{
					//	Do we need to switch super nodes?
					if((m_tmaxSuperNodes[iSuper].Children.Count >= m_iSuperNodeSize) &&
						(iSuper < m_tmaxSuperNodes.Count - 1))
					{
						SetSuperNodeText(m_tmaxSuperNodes[iSuper]);
						iSuper++;
					}

					//	Add to the super node child collection
					//
					//	NOTE:	We don't add to the tree until the user
					//			expands the super node. This cuts down
					//			significantly on the load time
					m_tmaxSuperNodes[iSuper].Children.Add(tmaxNode);

					//	Can't set the properties until added to the tree
					//	when the user expands the node
				}
				else
				{
					//	Just add to the root collection
					Add(m_tmaxNode, tmaxNode);
				}

			}

			//	Make sure the last super node has the appropriate text
			if(m_tmaxSuperNodes.GetLast() != null)
				SetSuperNodeText(m_tmaxSuperNodes.GetLast());
			
			//m_tmaxEventSource.FireElapsed(this, "Load", "Time to load " + m_tmaxPrimaries.Count.ToString() + " " + m_tmaxNode.MediaType.ToString() + " nodes: ");
			
			return true;
		}
		
		/// <summary>This method is called to add a new node to the collection</summary>
		/// <param name="tmaxPrimary">The primary node to be added</param>
		/// <param name="bReorder">true to reorder items in the supernode after adding the new item</param>
		/// <returns>true if successful</returns>
		public bool Add(CTmaxMediaTreeNode tmaxPrimary, bool bReorder)
		{
			int iPrimaryIndex = 0;
			
			Debug.Assert(m_tmaxPrimaries != null);
			Debug.Assert(m_tmaxNode != null);
			Debug.Assert(m_tmaxNode.Nodes != null);
			if(m_tmaxPrimaries == null) return false;
			if(m_tmaxNode == null) return false;
			if(m_tmaxNode.Nodes == null) return false;

			//	Insert into the primaries collection
			if((iPrimaryIndex = m_tmaxPrimaries.Add(tmaxPrimary)) < 0)
			{
				return false;
			}

			//	Insert into root collection if not using super nodes
			if(m_iSuperNodeSize == 0)
			{
				return Insert(m_tmaxNode, tmaxPrimary, iPrimaryIndex);
			}
			
			//	Is everything currently stored in the parent collection?
			else if(m_tmaxSuperNodes.Count == 0)
			{
				Insert(m_tmaxNode, tmaxPrimary, iPrimaryIndex);
									
				//	Do we need to create super nodes ?
				if((bReorder == true) && (m_tmaxNode.Nodes.Count > m_iSuperNodeSize))
				{
					//	Construct the super nodes
					//
					//	NOTE:	The primaries are already sorted
					Rebuild(false);
				}
				return true;
			}
			
			//	Need to insert into the appropriate super node
			else
			{			
				return Insert(iPrimaryIndex, bReorder);
			}

		}// public bool Add(CTmaxMediaTreeNode tmaxNode)
		
		/// <summary>This method is called to remove a primary node</summary>
		/// <param name="tmaxPrimary">The primary node to be removed</param>
		/// <param name="bReorder">true to reorder items in the supernode after removing the node</param>
		public void Remove(CTmaxMediaTreeNode tmaxPrimary, bool bReorder)
		{
			CTmaxMediaTreeNode tmaxSuper = null;
			
			Debug.Assert(m_tmaxPrimaries != null);
			Debug.Assert(m_tmaxNode != null);
			Debug.Assert(m_tmaxNode.Nodes != null);
			if(m_tmaxPrimaries == null) return;
			if(m_tmaxNode == null) return;
			if(m_tmaxNode.Nodes == null) return;
			
			//	Remove from the primaries collection
			m_tmaxPrimaries.Remove(tmaxPrimary);

			//	Are we using super nodes
			if((m_tmaxSuperNodes != null) && (m_tmaxSuperNodes.Count > 0))
			{
				//	Locate the super node that owns this primary
				foreach(CTmaxMediaTreeNode O in m_tmaxSuperNodes)
				{
					if(O.Children != null)
					{
						if(O.Children.Contains(tmaxPrimary) == true)
						{
							//	Remove from the super node
							O.Remove(tmaxPrimary, true, true);
							tmaxSuper = O;
							break;
							
						}// if(O.Children.Contains(tmaxPrimary) == true)
						
					}// if(O.Children != null)
					
				}// foreach(CTmaxMediaTreeNode O in m_tmaxSuperNodes)
				
				//	Did we locate the super node?
				if(tmaxSuper != null)
				{
					//	Do we need to reorder?
					if(bReorder == true)
					{
						//	Compress the nodes
						Pack();
					}
					else
					{
						//	Make sure the text is correct
						SetSuperNodeText(tmaxSuper);
					}
					
				}// if(tmaxSuper != null)	
			
			}
			else
			{
				//	Remove from the root collection
				m_tmaxNode.Nodes.Remove(tmaxPrimary);
			}
			
		}// Remove(CTmaxMediaTreeNode tmaxPrimary, bool bReorder)
		
		/// <summary>This method is called to get the super node that owns the specified primary node</summary>
		/// <param name="tmaxPrimary">The primary node that belongs to the super node</param>
		/// <returns>The parent super node if found</returns>
		public CTmaxMediaTreeNode GetSuperNode(CTmaxMediaTreeNode tmaxPrimary)
		{
			Debug.Assert(m_tmaxPrimaries != null);
			Debug.Assert(m_tmaxNode != null);
			Debug.Assert(m_tmaxNode.Nodes != null);
			if(m_tmaxPrimaries == null) return null;
			if(m_tmaxNode == null) return null;
			if(m_tmaxNode.Nodes == null) return null;
			
			//	Are we using super nodes
			if(m_tmaxSuperNodes == null) return null;
			if(m_tmaxSuperNodes.Count == 0) return null;
			
			//	Locate the super node that owns this primary
			foreach(CTmaxMediaTreeNode O in m_tmaxSuperNodes)
			{
				if(O.Children != null)
				{
					if(O.Children.Contains(tmaxPrimary) == true)
					{
						return O;
						
					}// if(O.Children.Contains(tmaxPrimary) == true)
					
				}// if(O.Children != null)
				
			}// foreach(CTmaxMediaTreeNode O in m_tmaxSuperNodes)
			
			//	Must not have found the super node
			return null;
			
		}// public CTmaxMediaTreeNode GetSuperNode(CTmaxMediaTreeNode tmaxPrimary)
		
		///	<summary>This method will set the text for the specified node</summary>
		/// <param name="tmaxNode">The node who's text is to be set</param>
		public void SetText(CTmaxMediaTreeNode tmaxNode)
		{
			//	Is this a super node?
			if((m_tmaxSuperNodes != null) && (m_tmaxSuperNodes.Contains(tmaxNode) == true))
			{
				SetSuperNodeText(tmaxNode);
			}
			else
			{
				//	Is this one of our primary nodes?
				if((m_tmaxPrimaries != null) && (m_tmaxPrimaries.Contains(tmaxNode) == true))
				{
					tmaxNode.Text = tmaxNode.GetMediaRecord().GetText(Tree.PrimaryTextMode);
				}
				
			}
			
		}// public void SetText(CTmaxMediaTreeNode tmaxNode)
			
		/// <summary>Clears the list of primaries without resetting the parent node</summary>
		public void Clear()
		{
			try
			{
				if((m_tmaxNode != null) && (m_tmaxNode.Nodes != null))
				{
					m_tmaxNode.Clear();
				}
					
				if(m_tmaxPrimaries != null)
				{
					m_tmaxPrimaries.Clear();
				}
				if(m_tmaxSuperNodes != null)
				{
					foreach(CTmaxMediaTreeNode tmaxSuper in m_tmaxSuperNodes)
					{
						tmaxSuper.Clear();
					}
					m_tmaxSuperNodes.Clear();
				}
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Clear", "Exception Raised", Ex);
			}
			
		}// public void Clear()
		
		/// <summary>Resets the text for all super nodes</summary>
		public void ResetSuperNodeText()
		{
			try
			{
				if(m_tmaxSuperNodes != null)
				{
					foreach(CTmaxMediaTreeNode tmaxSuper in m_tmaxSuperNodes)
					{
						SetSuperNodeText(tmaxSuper);
					}
				}
			}
			catch
			{
			}
			
		}// public void ResetSuperNodeText()
		
		/// <summary>Resets the text mode for all primary nodes</summary>
		public void SetPrimaryTextMode(TmaxTextModes eMode)
		{
			try
			{
				//	Set the text for each primary node
				if(m_tmaxPrimaries != null && (m_tmaxPrimaries.Count > 0))
				{
					foreach(CTmaxMediaTreeNode tmaxPrimary in m_tmaxPrimaries)
					{
						tmaxPrimary.Text = tmaxPrimary.IPrimary.GetText(eMode);
					}
					
					//	Update the super nodes
					ResetSuperNodeText();
					
				}// if(m_tmaxPrimaries != null && (m_tmaxPrimaries.Count > 0)
				
			}
			catch
			{
			}
			
		}// public void SetPrimaryTextMode(TmaxTextModes eMode)
		
		/// <summary>Rebuilds the branch using the current SuperNodeSize value</summary>
		/// <param name="bSort">true to also sort the primary records</param>
		public void Rebuild(bool bSort)
		{
			Debug.Assert(m_tmaxPrimaries != null);
			Debug.Assert(m_tmaxNode != null);
			Debug.Assert(m_tmaxNode.Nodes != null);
			if(m_tmaxPrimaries == null) return;
			if(m_tmaxNode == null) return;
			if(m_tmaxNode.Nodes == null) return;
			
			//	Make sure we have the correct number of super nodes
			SetSuperNodes();
			
			if(m_tmaxPrimaries.Count > 0)
			{
				if(bSort)
				{
					//m_tmaxEventSource.FireDiagnostic(this, "Rebuild", "REBUILD - SORT");
					Sort();
				}
				else
				{
					//m_tmaxEventSource.FireDiagnostic(this, "Rebuild", "REBUILD - REORDER");
					Reorder();
				}
			}
			
		}// public void Rebuild()
		
		/// <summary>Resets the node to its initial condition</summary>
		public void Reset()
		{
			//	Clear all child nodes
			Clear();
			
			//	Reset the parent node
			m_tmaxNode = null;
		
		}// public void Reset()
		
		/// <summary>This method is called to sort the primary nodes</summary>
		public void Sort()
		{
			if((m_tmaxPrimaries == null) || (m_tmaxPrimaries.Count == 0)) return;
			
			try
			{
				//	Sort the primaries without regard for the IsSorted flag
				m_tmaxPrimaries.Sort(true);
				
				//	Make sure primaries are in appropriate parent
				Reorder();
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Sort", "Exception Raised", Ex);
			}
			
		}// public void Sort()
	
		#endregion Public Members
		
		#region Protected Members
		
		/// <summary>This method is called to add a node to the specified parent collection</summary>
		/// <param name="tmaxParent">The parent node</param>
		/// <param name="tmaxPrimary">The node to be added</param>
		/// <returns>true if successful</returns>
		protected bool Add(CTmaxMediaTreeNode tmaxParent, CTmaxMediaTreeNode tmaxPrimary)
		{
			try
			{
				//	Should we reposition instead of add?
				if(tmaxPrimary.Added == true)
				{
					if(tmaxPrimary.Control != null)
						tmaxPrimary.Reposition(tmaxParent.Nodes);
					else
						tmaxParent.Nodes.Add(tmaxPrimary);
				}
				else
				{					
					//	Add to the parent collection
					tmaxParent.Nodes.Add(tmaxPrimary);
					
					//	Mark the node as having been put in the tree
					tmaxPrimary.Added = true;
					
					//	Set the property values AFTER adding to the tree
					tmaxPrimary.SetProperties(m_ctrlTree.GetImageIndex(tmaxPrimary));
							
				}
				
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Add", Ex);
				return false;
			}
				
		}// public bool Add(CTmaxMediaTreeNode tmaxParent, CTmaxMediaTreeNode tmaxNode)
		
		/// <summary>This method is called to add a node to the specified parent collection</summary>
		/// <param name="tmaxParent">The parent node</param>
		/// <param name="tmaxNode">The primary node to be added</param>
		/// <param name="iIndex">Index at which to insert the node</param>
		/// <returns>true if successful</returns>
		protected bool Insert(CTmaxMediaTreeNode tmaxParent, CTmaxMediaTreeNode tmaxPrimary, int iIndex)
		{
			try
			{
				//	Should we reposition instead of add?
				if(tmaxPrimary.Added == true)
				{
					if(tmaxPrimary.Control != null)
						tmaxPrimary.Reposition(tmaxParent.Nodes, iIndex);
					else
						tmaxParent.Nodes.Insert(iIndex, tmaxPrimary);
				}
				else
				{					
					//	Add to the parent collection
					tmaxParent.Nodes.Insert(iIndex, tmaxPrimary);
					
					//	Mark the node as having been put in the tree
					tmaxPrimary.Added = true;
					
					//	Set the property values AFTER adding to the tree
					tmaxPrimary.SetProperties(m_ctrlTree.GetImageIndex(tmaxPrimary));
				}
				
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Insert", Ex);
				return false;
			}
				
		}// public bool Add(CTmaxMediaTreeNode tmaxParent, CTmaxMediaTreeNode tmaxNode)
		
		/// <summary>This method is called to add a new super node to the collection</summary>
		/// <param name="strName">The name of the new node</param>
		/// <returns>The new node if successful</returns>
		protected CTmaxMediaTreeNode AddSuperNode(string strName)
		{
			CTmaxMediaTreeNode tmaxSuper = null;
			
			try
			{
				tmaxSuper = new CTmaxMediaTreeNode(strName);
				tmaxSuper.MediaType = MediaType;
						
				//	Add to parent's collection
				m_tmaxNode.Nodes.Add(tmaxSuper);
							
				//	Set the property values AFTER adding to the tree
				tmaxSuper.SetProperties(2);
				tmaxSuper.Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.CheckOnExpand;
				tmaxSuper.Override.AllowAutoDragExpand = Infragistics.Win.UltraWinTree.AllowAutoDragExpand.ExpandOnDragHoverWhenExpansionIndicatorVisible;			
				
				//	Add to the local supernode collection
				m_tmaxSuperNodes.Add(tmaxSuper);
				
				return tmaxSuper;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddSuperNode", "Exception Raised: ", Ex);
				return null;
			}
			
		}// protected CTmaxMediaTreeNode AddSuperNode(string strName)
		
		/// <summary>This method will initialize the local members</summary>
		protected void Initialize()
		{
			Debug.Assert(m_tmaxPrimaries != null);
			if(m_tmaxPrimaries != null)
			{
				m_tmaxPrimaries.KeepSorted = true;
			}
			
			Debug.Assert(m_tmaxSuperNodes != null);
			if(m_tmaxSuperNodes != null)
			{
				m_tmaxSuperNodes.KeepSorted = false;
			}
			
		}// protected void Initialize()
		
		/// <summary>Populates the parent collection with the required number of super nodes</summary>
		protected void SetSuperNodes()
		{
			int	iSuperNodes = 0;
			
			Debug.Assert(m_tmaxPrimaries != null);
			Debug.Assert(m_tmaxNode != null);
			Debug.Assert(m_tmaxNode.Nodes != null);
			if(m_tmaxPrimaries == null) return;
			if(m_tmaxNode == null) return;
			if(m_tmaxNode.Nodes == null) return;
			
			//m_tmaxEventSource.InitElapsed();
			
			//	Remove the primaries from their existing collections
			if(m_tmaxSuperNodes.Count > 0)
			{
				//	Clear each existing super node
				foreach(CTmaxMediaTreeNode O in m_tmaxSuperNodes)
					O.Clear();
			}
			else
			{
				//	Clear the root collection
				m_tmaxNode.Clear();
			}
			
			//	How many super nodes do we need?
			if(m_iSuperNodeSize > 0 && m_tmaxPrimaries.Count > m_iSuperNodeSize)
			{
				iSuperNodes = m_tmaxPrimaries.Count / m_iSuperNodeSize;
				
				if(m_tmaxPrimaries.Count % m_iSuperNodeSize > 0)
					iSuperNodes += 1;
			}
				
			//	Has the super node count changed?
			if(iSuperNodes != m_tmaxSuperNodes.Count)
			{
				//	Should we clear all super nodes?
				if(iSuperNodes == 0)
				{
					//	Clear the existing nodes in root
					m_tmaxNode.Clear();
					m_tmaxSuperNodes.Clear();
				}
				else
				{
					//	Do we need to add super nodes?
					if(iSuperNodes > m_tmaxSuperNodes.Count)
					{
						for(int i = m_tmaxSuperNodes.Count + 1; i <= iSuperNodes; i++)
							AddSuperNode("SN" + i.ToString());
					}
					else
					{
						//	Remove the unneccessary super nodes
						while(m_tmaxSuperNodes.Count > iSuperNodes)
						{
							m_tmaxNode.Nodes.Remove(m_tmaxSuperNodes.GetLast());
							m_tmaxSuperNodes.Remove(m_tmaxSuperNodes.GetLast());
						}
					
					}// if(iSuperNodes > m_tmaxSuperNodes.Count)
					
				}
				
			}// if(iSuperNodes != m_tmaxSuperNodes.Count)
					
			//m_tmaxEventSource.FireElapsed(this, "SetSuperNodes", "Time to set super nodes: ");

		}// protected void SetSuperNodes()
		
		/// <summary>This method is called to set the text for the specified super node</summary>
		/// <param name="tmaxSuper">The desired super node</param>
		protected void SetSuperNodeText(CTmaxMediaTreeNode tmaxSuper)
		{
			string strText = "";
			
			if(tmaxSuper.Children.Count == 0)
			{
				strText = "Empty";
			}
			else
			{
				strText = tmaxSuper.Children[0].Text + " - " + tmaxSuper.Children[tmaxSuper.Children.Count - 1].Text;
			}

			tmaxSuper.Text = strText;
		
		}// protected void SetSuperNodeText(CTmaxMediaTreeNode tmaxSuper)
		
		/// <summary>Reorders the primary nodes within the supernodes</summary>
		protected void Reorder()
		{
			int i = 0;
			
			Debug.Assert(m_tmaxPrimaries != null);
			Debug.Assert(m_tmaxNode != null);
			Debug.Assert(m_tmaxNode.Nodes != null);
			if(m_tmaxPrimaries == null) return;
			if(m_tmaxNode == null) return;
			if(m_tmaxNode.Nodes == null) return;
		
			try
			{
				//	Use the parent collection if not using supernodes
				if((m_tmaxSuperNodes == null) || (m_tmaxSuperNodes.Count == 0))
				{
					foreach(CTmaxMediaTreeNode tmaxNode in m_tmaxPrimaries)
					{
						Add(m_tmaxNode, tmaxNode);
					}
				}
				else
				{
					//	Clear out the first super node
					m_tmaxSuperNodes[0].Clear();
					
					//	Start adding primaries
					foreach(CTmaxMediaTreeNode tmaxPrimary in m_tmaxPrimaries)
					{
						//	Put in the super node's child collection
						m_tmaxSuperNodes[i].Children.Add(tmaxPrimary);
						
						//	Do we need to get the next supernode?
						if(m_tmaxSuperNodes[i].Children.Count == m_iSuperNodeSize)
						{
							if((i < m_tmaxSuperNodes.Count - 1) && (m_tmaxSuperNodes[i + 1] != null))
							{
								//	Set supernode text here
								SetSuperNodeText(m_tmaxSuperNodes[i]);
								
								//	Move to the next super node
								i++;
								
								//	Clear out the new super node
								m_tmaxSuperNodes[i].Clear();
							}
							
						}// if(m_tmaxSuperNodes[i].Nodes.Count == m_iSuperNodeSize)
						
					}// foreach(CTmaxMediaTreeNode tmaxPrimary in m_tmaxPrimaries)
					
					//	Set the text for the last super node
					SetSuperNodeText(m_tmaxSuperNodes[m_tmaxSuperNodes.Count - 1]);
				}
					
				//	Make sure the expansion indicators are properly initialized
				if((m_ctrlTree != null) && (m_ctrlTree.TreeCtrl != null))
				{
					if((m_tmaxNode != null) && (m_tmaxNode.Nodes != null))
					{
						try { m_ctrlTree.TreeCtrl.ResetExpansionIndicator(m_tmaxNode.Nodes, false); }
						catch {};
					}
					
				}// if((m_ctrlTree != null) && (m_ctrlTree.TreeCtrl != null))
				
				//m_tmaxEventSource.FireElapsed(this, "Reorder", "Time to reorder " + m_tmaxPrimaries.Count.ToString() + " nodes");
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Reorder", "Exception Raised", Ex);
			}
			
		}// protected void Reorder()
		
		/// <summary>This method rearranges the super node children so that each node if filled</summary>
		protected void Pack()
		{
			Debug.Assert(m_tmaxSuperNodes != null);
			Debug.Assert(m_tmaxSuperNodes.Count != 0);
			if(m_tmaxSuperNodes == null) return;
			if(m_tmaxSuperNodes.Count == 0) return;
			
			//m_tmaxEventSource.InitElapsed();	

			for(int i = 0; i < m_tmaxSuperNodes.Count; i++)
			{
				//	Is this node over the limit?
				if(m_tmaxSuperNodes[i].Children.Count > m_iSuperNodeSize)
				{
					//	Push excess nodes down
					Push(i);
				}
				
				//	Does this node need some more children?
				else if(m_tmaxSuperNodes[i].Children.Count < m_iSuperNodeSize)
				{
					if(i == m_tmaxSuperNodes.Count - 1)
					{
						//	Remove this node if doesn't have any children
						if(m_tmaxSuperNodes[i].Children.Count == 0)
						{
							m_tmaxNode.Nodes.Remove(m_tmaxSuperNodes[i]);
							m_tmaxSuperNodes.Remove(i);
						}
						
					}
					else
					{
						//	Use children in lower super nodes to fill this one
						Pull(i);
					}
					
				}
				
			}// for(int i = 0; i < m_tmaxSuperNodes.Count; i++)
			
			//	Remove all empty nodes from the bottom up
			while((m_tmaxSuperNodes.Count > 0) && (m_tmaxSuperNodes.GetLast().Children.Count == 0))
			{
				m_tmaxNode.Nodes.Remove(m_tmaxSuperNodes.GetLast());
				m_tmaxSuperNodes.Remove(m_tmaxSuperNodes.GetLast());
			}

			//m_tmaxEventSource.FireElapsed(this, "Pack", "Time to pack " + m_tmaxPrimaries.Count.ToString() + " nodes");
		
		}// protected void Pack()
		
		/// <summary>This method pushes excess children in the specified super node down into the next super node</summary>
		/// <param name="iSuper">The index of the super node to be pushed</param>
		protected void Push(int iSuper)
		{
			CTmaxMediaTreeNode	tmaxSuper = m_tmaxSuperNodes[iSuper];
			CTmaxMediaTreeNode	tmaxLast = null;
			
			//	Are we on the last super node?
			if(iSuper == m_tmaxSuperNodes.Count - 1)
			{
				//	Add a new super node so that we have some place to
				//	put everything
				if(AddSuperNode("SN") == null)
					return;
			}

			//	Move all the excess nodes down into the next node
			while(tmaxSuper.Children.Count > m_iSuperNodeSize)
			{
				//	Get the last node in the super node's collection
				tmaxLast = tmaxSuper.Children[tmaxSuper.Children.Count - 1];
				
				//	Remove the last node from the child and tree collections
				tmaxSuper.Remove(tmaxLast, true, true);
			
				//	Put in the child collection of the next node but not the tree collection
				m_tmaxSuperNodes[iSuper + 1].Children.Insert(0, tmaxLast);
				
				//	Insert into the tree collection if this super node has been expanded
				if(m_tmaxSuperNodes[iSuper + 1].Nodes.Count > 0)
				{
					Insert(m_tmaxSuperNodes[iSuper + 1], tmaxLast, 0);
				}

			}// while(tmaxSuper.Children.Count > m_iSuperNodeSize)

			//	Make sure the text is correct
			SetSuperNodeText(tmaxSuper);
			SetSuperNodeText(m_tmaxSuperNodes[iSuper + 1]);
		
		}// protected void Push(int iSuper)
		
		/// <summary>This method pulls children from lower super nodes and uses them to fill the specified super node</summary>
		/// <param name="iSuper">The index of the super node to be filled</param>
		protected void Pull(int iSuper)
		{
			CTmaxMediaTreeNode	tmaxSuper = m_tmaxSuperNodes[iSuper];
			int				iNext = iSuper + 1;
			
			Debug.Assert(iNext < m_tmaxSuperNodes.Count);
			
			while((iNext < m_tmaxSuperNodes.Count) && (tmaxSuper.Children.Count < m_iSuperNodeSize))
			{
				//	Move children from the lower super node up
				while(m_tmaxSuperNodes[iNext].Children.Count > 0)
				{
					//	Add to the child collection of the target super node
					tmaxSuper.Children.Add(m_tmaxSuperNodes[iNext].Children[0]);
					
					//	Add to the tree collection if the super node is expanded
					if(tmaxSuper.Nodes.Count > 0)
						Add(tmaxSuper, m_tmaxSuperNodes[iNext].Children[0]);
						
					//	Remove from the child collection of this node
					m_tmaxSuperNodes[iNext].Remove(m_tmaxSuperNodes[iNext].Children[0], true, true);
					
					//	Is the target node full?
					if(tmaxSuper.Children.Count == m_iSuperNodeSize)
						break;
						
				}// while(m_tmaxSuperNodes[iNext].Nodes.Count > 0)
				
				SetSuperNodeText(m_tmaxSuperNodes[iNext]);
				iNext++;
			}
			
			//	Make sure the text is correct
			SetSuperNodeText(tmaxSuper);
									
			//	Did we reach the end of the collection?
			if(iNext == m_tmaxSuperNodes.Count - 1)
			{
				//	Remove all empty nodes from the bottom up
				while((m_tmaxSuperNodes.Count > 0) && (m_tmaxSuperNodes.GetLast().Children.Count == 0))
				{
					m_tmaxNode.Nodes.Remove(m_tmaxSuperNodes.GetLast());
					m_tmaxSuperNodes.Remove(m_tmaxSuperNodes.GetLast());
				}
			}
		
		}// protected void Pull(int iSuper)
		
		/// <summary>This method is called to insert the primary node at the specified index</summary>
		/// <param name="iPrimaryIndex">The index of the node to be inserted</param>
		/// <param name="bPack">true to reorder items in the supernode after inserting the new item</param>
		/// <returns>true if successful</returns>
		protected bool Insert(int iPrimaryIndex, bool bPack)
		{
			int	iSuperNode = 0;
			int	iSuperIndex = 0;
			int	iNodes = 0;
			
			Debug.Assert(m_tmaxPrimaries != null);
			Debug.Assert(m_tmaxNode != null);
			Debug.Assert(m_tmaxNode.Nodes != null);
			Debug.Assert(m_tmaxPrimaries[iPrimaryIndex] != null);
			Debug.Assert(m_tmaxSuperNodes != null);
			Debug.Assert(m_tmaxSuperNodes.Count > 0);
			if(m_tmaxPrimaries == null) return false;
			if(m_tmaxNode == null) return false;
			if(m_tmaxNode.Nodes == null) return false;
			if(m_tmaxPrimaries[iPrimaryIndex] == null) return false;
			
			//m_tmaxEventSource.InitElapsed();
		
			//	Is this the first primary node?
			if(iPrimaryIndex == 0)
			{
				iSuperNode = 0;
				iSuperIndex = 0;
			}
			
			//	Is it the last primary node?
			else if(iPrimaryIndex == m_tmaxPrimaries.Count - 1)
			{
				iSuperNode = m_tmaxSuperNodes.Count - 1;
				iSuperIndex = m_tmaxSuperNodes[iSuperNode].Children.Count;
			}
			
			else
			{
				//	Locate the super node where this object should be inserted
				//
				//	NOTE:	Because the primaries are in sorted order and the nodes within
				//			each super node are in sorted order all we have to do is count
				//			each super node child until we get to the right super node
				for(iSuperNode = 0; iSuperNode < m_tmaxSuperNodes.Count; iSuperNode++)
				{
					//	Have we found the appropriate super node?
					if((iNodes + m_tmaxSuperNodes[iSuperNode].Children.Count - 1) >= iPrimaryIndex)
					{
						iSuperIndex = iPrimaryIndex - iNodes;
						break;
					}
					else
					{
						iNodes += m_tmaxSuperNodes[iSuperNode].Children.Count;
					}
					
				}
				
				//	Do we need to add a new super node?
				Debug.Assert(iSuperNode <= m_tmaxSuperNodes.Count);
				if(iSuperNode == m_tmaxSuperNodes.Count)
				{
					//	Always add to the last node if not reordering
					if(bPack == false)
					{
						iSuperNode  = m_tmaxSuperNodes.Count - 1;
						iSuperIndex = m_tmaxSuperNodes[iSuperNode].Children.Count;
					}
					else
					{
						//	Add a new super node
						if(AddSuperNode("SN") != null)
						{
							iSuperIndex = 0;
						}
							
					}
					
				}// if(iSuperNode >= m_tmaxSuperNodes.Count)
				
			}
		
			//	Put the primary node in the appropriate super node
			if(iSuperIndex < m_tmaxSuperNodes[iSuperNode].Children.Count)
			{
				m_tmaxSuperNodes[iSuperNode].Children.Insert(iSuperIndex, m_tmaxPrimaries[iPrimaryIndex]);
				
				//	Has this super node been expanded already?
				if(m_tmaxSuperNodes[iSuperNode].Nodes.Count > 0)
					Insert(m_tmaxSuperNodes[iSuperNode], m_tmaxPrimaries[iPrimaryIndex], iSuperIndex);
			}
			else
			{
				m_tmaxSuperNodes[iSuperNode].Children.Add(m_tmaxPrimaries[iPrimaryIndex]);
				
				//	Has this super node been expanded already?
				if(m_tmaxSuperNodes[iSuperNode].Nodes.Count > 0)
					Add(m_tmaxSuperNodes[iSuperNode], m_tmaxPrimaries[iPrimaryIndex]);
			}
				
			//m_tmaxEventSource.FireElapsed(this, "Insert");
			
			//	Do we need to reorder?
			if((bPack == true) && (m_tmaxSuperNodes[iSuperNode].Children.Count > m_iSuperNodeSize))
			{
				Pack();			
			}
			else
			{
				//	Make sure the supernode text is correct
				SetSuperNodeText(m_tmaxSuperNodes[iSuperNode]);
			}
			
			return true;

		}// public bool Insert(int iPrimaryIndex, bool bReorder)
		
		#endregion Protected Members
		
		#region Properties
	
		/// <summary>MediaType associated with this node</summary>
		public FTI.Shared.Trialmax.TmaxMediaTypes MediaType
		{
			get
			{
				return m_eMediaType;
			}
			set
			{
				m_eMediaType = value;
			}
		
		}//	MediaType property
		
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
			}
		
		}//	SuperNodeSize property
		
		/// <summary>Tree pane that contains the node</summary>
		public CMediaTree Tree
		{
			get
			{
				return m_ctrlTree;
			}
			set
			{
				m_ctrlTree = value;
			}
		
		}//	Tree property
		
		/// <summary>Tree node used to represent this media type</summary>
		public FTI.Trialmax.Controls.CTmaxMediaTreeNode Node
		{
			get
			{
				return m_tmaxNode;
			}
			set
			{
				m_tmaxNode = value;
				
				if(m_tmaxNode != null)
					m_tmaxEventSource.Name = (m_tmaxNode.Text + " Media Node");
			}
		
		}//	Node property
		
		/// <summary>Collection of primary tree nodes belonging to this media node</summary>
		public FTI.Trialmax.Controls.CTmaxMediaTreeNodes Primaries
		{
			get
			{
				return m_tmaxPrimaries;
			}
		
		}//	Primaries property
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get
			{
				return m_tmaxEventSource;
			}
		
		}// EventSource property
		
		/// <summary>Collection of super nodes belonging to this media node</summary>
		public FTI.Trialmax.Controls.CTmaxMediaTreeNodes SuperNodes
		{
			get
			{
				return m_tmaxSuperNodes;
			}
		
		}//	SuperNodes property
		
		#endregion Properties
		
	}// protected class CMediaNode
	
	/// <summary>
	/// This class maintains a dynamic array list of CMediaNode objects
	/// </summary>
	public class CMediaNodes : CollectionBase
	{
		#region Private Members
		
		/// <summary>Local member bound to EventSource property</summary>
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new FTI.Shared.Trialmax.CTmaxEventSource();

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CMediaNodes()
		{
			m_tmaxEventSource.Name = "Media Nodes Collection";
		}
		
		/// <summary>Called to retrieve the object at the specified index</summary>
		/// <param name="iIndex">The desired index</param>
		/// <returns>The object at the specified index</returns>
		public CMediaNode GetAt(int iIndex)
		{
			if((base.List != null) && (iIndex >= 0) && (iIndex < Count))
			{
				return (base.List[iIndex] as CMediaNode);
			}
			else
			{
				return null;
			}

		}// public CMediaNode GetAt(int iIndex)
	
		/// <summary>Called to set the object at the specified index</summary>
		/// <param name="iIndex">The desired index</param>
		/// <param name="O">The new object</param>
		public void SetAt(int iIndex, CMediaNode mediaNode)
		{
			if((base.List != null) && (iIndex >= 0) && (iIndex < Count))
			{
				base.List[iIndex] = mediaNode;
			}
	
		}// public void SetAt(int iIndex, CMediaNode mediaNode)

		/// <summary>Overloaded array operator</summary>
		public CMediaNode this[int iIndex]
		{
			get
			{
				return GetAt(iIndex);
			}
			set
			{
				SetAt(iIndex, value);
			}
		
		}// public CMediaNode this[int iIndex]

		/// <summary>Clears all objects from the list</summary>
		public new void Clear()
		{
			if(base.List != null)
			{
				//	Clear each node
				foreach(CMediaNode medNode in  base.List)
				{
					medNode.Clear();
				}
				
				//	Now clear the list
				base.List.Clear();
			}
		
		}// public void Clear()
		
		/// <summary>Resets each node in the list</summary>
		public void Reset()
		{
			if(base.List != null)
			{
				foreach(CMediaNode medNode in base.List)
				{
					medNode.Reset();
				}
			}
		
		}// public void Reset()
		
		/// <summary>Resets the text for each super node</summary>
		public void ResetSuperNodeText()
		{
			if(base.List != null)
			{
				foreach(CMediaNode medNode in base.List)
				{
					medNode.ResetSuperNodeText();
				}
			}
		
		}// public void ResetSuperNodeText()
		
		/// <summary>Resets the text mode for all primary nodes</summary>
		public void SetPrimaryTextMode(TmaxTextModes eMode)
		{
			if(base.List != null)
			{
				foreach(CMediaNode medNode in base.List)
				{
					medNode.SetPrimaryTextMode(eMode);
				}
			}
		
		}// public void SetPrimaryTextMode(TmaxTextModes eMode)
			
		/// <summary>Sets the text for the specified node</summary>
		public void SetText(CTmaxMediaTreeNode tmaxNode)
		{
			if(base.List != null)
			{
				foreach(CMediaNode medNode in base.List)
				{
					medNode.SetText(tmaxNode);
				}
			}
		
		}// public void SetText(CTmaxMediaTreeNode tmaxNode)
			
		/// <summary>Instructs each node to load its primary records</summary>
		public void Load()
		{
			if(base.List != null)
			{
				foreach(CMediaNode medNode in base.List)
				{
					medNode.Load();
				}
			}
		
		}// public void Load()
		
		/// <summary>Instructs each node to prepare for loading</summary>
		public void PreLoad()
		{
			if(base.List != null)
			{
				foreach(CMediaNode medNode in base.List)
				{
					medNode.PreLoad();
				}
			}
		
		}// public void PreLoad()
		
		/// <summary>Sorts each node in the list</summary>
		public void Sort()
		{
			if(base.List != null)
			{
				foreach(CMediaNode medNode in base.List)
				{
					medNode.Sort();
				}
			}
		
		}// public void Sort()
		
		/// <summary>Adds a new node to the collection</summary>
		/// <param name="medNode">The new media node to be added</param>
		///	<returns>true if successful, false otherwise</returns>	
		public bool Add(CMediaNode medNode)
		{
			if(base.List != null)
			{
				try
				{
					base.List.Add(medNode as object);
					return true;
				}
				catch
				{
				}
			}
			
			//	Must have been a problem
			return false;
		
		}// public bool Add(CMediaNode medNode)
		
		/// <summary>Called to find the media node object with the specified MediaType identifier</summary>
		/// <param name="eType">The desired media type</param>
		///	<returns>The associated media node object if found</returns></returns>	
		public CMediaNode Find(FTI.Shared.Trialmax.TmaxMediaTypes eMediaType)
		{
			if(base.List != null)
			{
				foreach(CMediaNode medNode in base.List)
				{
					if(medNode.MediaType == eMediaType)
						return medNode;
				}
			}
			
			//	Must not have been found
			return null;
		}
		
		/// <summary>Called to find the media node object with the specified tree node</summary>
		/// <param name="tmaxNode">The desired tree node</param>
		///	<returns>The associated media node object if found</returns></returns>	
		public CMediaNode Find(FTI.Trialmax.Controls.CTmaxMediaTreeNode tmaxNode)
		{
			if(base.List != null)
			{
				foreach(CMediaNode medNode in base.List)
				{
					if(medNode.Node == tmaxNode)
						return medNode;
				}
			}
			
			//	Must not have been found
			return null;
		}
		
		/// <summary>Called to find the media node with the specified MediaType identifier</summary>
		/// <param name="eType">The desired media type</param>
		///	<returns>The associated media node if found</returns></returns>	
		public CTmaxMediaTreeNode FindTreeNode(FTI.Shared.Trialmax.TmaxMediaTypes eMediaType)
		{
			if(base.List != null)
			{
				foreach(CMediaNode medNode in base.List)
				{
					if(medNode.MediaType == eMediaType)
						return medNode.Node;
				}
			}
			
			//	Must not have been found
			return null;
		}
		
		/// <summary>Called to set the SuperNodeSize value of each object in the collection</summary>
		/// <param name="iSize">The new size</param>
		public void SetSuperNodeSize(int iSize)
		{
			if(base.List != null)
			{
				foreach(CMediaNode medNode in base.List)
				{
					medNode.SuperNodeSize = iSize;
				}
			}
		}
		
		/// <summary>Called to rebuild all media nodes in the collection</summary>
		/// <param name="bSort">true to sort the records also</param>
		public void Rebuild(bool bSort)
		{
			if(base.List != null)
			{
				foreach(CMediaNode medNode in base.List)
				{
					medNode.Rebuild(bSort);
				}
			}
		
		}// public void Rebuild(bool bSort)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get
			{
				return m_tmaxEventSource;
			}
		
		}// EventSource property

		#endregion Properties
		
	}// protected class CMediaNodes

}// namespace FTI.Trialmax.Panes
