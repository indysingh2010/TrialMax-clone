using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Database;
using FTI.Trialmax.Controls;

namespace FTI.Trialmax.Panes
{
	/// <summary>This pane displays the results of a search operation</summary>
	public class CFilteredTree : FTI.Trialmax.Panes.CMediaTree
	{
		#region Constants
		
		
		#endregion Constants
		
		#region Private Members

		/// <summary>Private member to keep track of record to be activated</summary>
		CDxMediaRecord m_dxActivate = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CFilteredTree() : base()
		{
			//	Set the flag to indicate that this is a filtered media tree
			m_bShowFiltered = true;
			
		}// public CFilteredTree()
		
		/// <summary>This method is called by the application to activate the specified item</summary>
		/// <param name="tmaxItem">The item to be activated</param>
		/// <param name="ePane">The pane requesting activation</param>
		/// <returns>true if successful</returns>
		public override bool Activate(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
			//	Is this pane visible?
			if(this.PaneVisible == true)
			{
				if(ePane != TmaxAppPanes.FilteredTree)
					return base.Select(tmaxItem, false, true);
				else
					return true;
			}
			else
			{
				m_dxActivate = (CDxMediaRecord)(tmaxItem.GetMediaRecord());
				return true;
			}
						
		}// public override bool Activate(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		#endregion Public Methods

		#region Protected Methods

		/// <summary>This function is called when the Active property changes</summary>
		protected override void OnPaneVisibleChanged()
		{
			if((m_bPaneVisible == true) && (m_dxActivate != null))
			{
				try
				{
					//	Make sure this is still a valid record
					//
					//	NOTE:	This allows us to ignore reopening the database and deleting records
					if((m_tmaxDatabase != null) && (m_tmaxDatabase.IsValidRecord(m_dxActivate) == true))
					{
						//	Make this the current selection
						Select(new CTmaxItem(m_dxActivate), false, true);
					}
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireDiagnostic(this, "OnPaneVisibleChanged", Ex);
				}
				
				m_dxActivate = null;

			}// if((m_bPaneVisible == true) && (m_dxActivate != null))

		}// protected override void OnPaneVisibleChanged()
		
		/// <summary>This method is called to determine if the specified command should be enabled</summary>
		/// <param name="eCommand">The tree pane command enumeration</param>
		/// <param name="tmaxNode">The current node selections</param>
		/// <returns>true if command should be enabled</returns>
		protected override bool GetCommandEnabled(TreePaneCommands eCommand, CTmaxMediaTreeNodes tmaxNodes)
		{
			//	What is the command?
			switch(eCommand)
			{
				case TreePaneCommands.ExpandAll:
				
					if(m_dxFiltered == null) return false;
					if(m_dxFiltered.Count == 0) return false;
					
					return true;
					
				case TreePaneCommands.New:
	
					//	Must be only one non-primary node selected
					if(tmaxNodes == null) return false;
					if(tmaxNodes.Count != 1) return false;
				
					//	Don't add new primary media in filtered tree
					if(tmaxNodes[0].MediaLevel == TmaxMediaLevels.None)
						return false;
						
					return base.GetCommandEnabled(eCommand, tmaxNodes);
					
				default:
				
					return base.GetCommandEnabled(eCommand, tmaxNodes);
			}	
			
		
		}// GetCommandEnabled(TreePaneCommands eCommand, CTmaxMediaTreeNode tmaxNode)
		
		/// <summary>This method notifies the derived class when a node has been expanded</summary>
		/// <param name="tmaxNode">The node in the tree that was expanded</param>
		/// <returns>true to cancel the operation</returns>
		protected override bool OnBeforeExpand(CTmaxMediaTreeNode tmaxNode)
		{
			//	Perform the base class processing
			if(base.OnBeforeExpand(tmaxNode) == true)
				return true;
			
			//	Are we only supposed to show treated pages?
			if(GetIfTreated() == true)
			{
				//	Is this a document being expanded?
				if((tmaxNode.GetMediaRecord() != null) && (tmaxNode.MediaType == TmaxMediaTypes.Document))
				{
					//	Check each of the child nodes
					foreach(CTmaxMediaTreeNode O in tmaxNode.Nodes)
					{
						if((O.GetMediaRecord() != null) && (O.GetMediaRecord().GetChildCount() > 0))
							O.Visible = true;
						else
							O.Visible = false;
					}
						
				}
			
			}// protected virtual bool GetIfTreated()
			
			//	OK to continue
			return false;
				
		}// protected virtual void OnAfterExpand(CTmaxMediaTreeNode tmaxNode)
		
		/// <summary>This method is called by the application when it adds new media to the database</summary>
		/// <param name="tmaxParent">TrialMax event item that identifies the parent record</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		{
			CTmaxMediaTreeNode tmaxNode = null;
			
			Debug.Assert(tmaxParent != null);
			Debug.Assert(tmaxChildren != null);
			
			//	All new media should be in the child collection
			Debug.Assert(tmaxChildren.Count > 0);
			if(tmaxChildren.Count == 0) return;
		
			//	Do the base class processing first
			base.OnAdded(tmaxParent, tmaxChildren);
			
			//	Do we need to perform special processing?
			if(GetIfTreated() == false) return;
			if(tmaxParent.GetMediaRecord() == null) return;
			
			//	What type of parent media?
			switch(tmaxParent.GetMediaRecord().GetMediaType())
			{
				case TmaxMediaTypes.Document:
				
					//	If the child nodes were added to the tree we have to set their visibility
					foreach(CTmaxItem O in tmaxChildren)
					{
						//	Get the node that's bound to this page
						if((tmaxNode = GetNode(O, false)) != null)
						{
							if((tmaxNode.GetMediaRecord() != null) && (tmaxNode.GetMediaRecord().GetChildCount() == 0))
								tmaxNode.Visible = false;
							else
								tmaxNode.Visible = true;
						}
						
					}// foreach(CTmaxItem O in tmaxChildren)
					break;
					
				case TmaxMediaTypes.Page:
				
					//	Get the node to the parent page
					if((tmaxNode = GetNode(tmaxParent, false)) != null)
					{
						//	Since we've added children we can make this page visible
						if(tmaxNode.Visible == false)
							tmaxNode.Visible = true;
					}
					break;
					
				default:
				
					break;
					
			}// switch(tmaxParent.GetMediaRecord().GetMediaType())
			
		}// public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		
		/// <summary>This method is called when the item gets deleted</summary>
		/// <param name="tmaxItem">The item that has been deleted</param>
		protected override void OnDeleted(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{
			CTmaxMediaTreeNode tmaxNode = null;
			
			//	Do the base class processing first
			base.OnDeleted(tmaxItem);
			
			//	Do we need to perform special processing?
			if(GetIfTreated() == false) return;
			if(tmaxItem.GetMediaRecord() == null) return;
			if(tmaxItem.GetMediaRecord().GetMediaType() != TmaxMediaTypes.Treatment) return;
			
			//	Get the node for the parent page if we've deleted the last of its children
			if((tmaxItem.ISecondary != null) && (tmaxItem.ISecondary.GetChildCount() == 0))
			{
				if((tmaxNode = GetNode(new CTmaxItem(tmaxItem.ISecondary), false)) != null)
				{
					//	Make sure it's no longer visible
					if(tmaxNode.Visible == true)
						tmaxNode.Visible = false;	
				}
				
			}

		}// private void OnDeleted(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		/// <summary>This method is called to add root nodes for each primary media type</summary>
		/// <param name="tmaxMediaTypes">The collection of media types</param>
		protected override void Add(CTmaxMediaTypes tmaxMediaTypes)
		{
			//	Do the base class processing first
			base.Add(tmaxMediaTypes);
			
			try
			{
				//	Are we only showing treated documents?
				if((GetIfTreated() == true) && (m_aMediaNodes != null))
				{
					//	Hide the non-document media nodes
					foreach(CMediaNode O in m_aMediaNodes)
					{
						if((O.Node != null) && (O.MediaType != TmaxMediaTypes.Document))
							O.Node.Visible = false;
					}
					
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Add", Ex);
			}

		}// protected override void Add(CTmaxMediaTypes tmaxMediaTypes)
				
		#endregion Protected Methods

		#region Properties
		
				
		#endregion Properties
		
	}// public class CFilteredTree : FTI.Trialmax.Panes.CBasePane

}// namespace FTI.Trialmax.Panes
