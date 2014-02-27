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

namespace FTI.Trialmax.Controls
{
	/// <summary>This class creates and manages a TrialMax tree of media records</summary>
	public class CTmaxMediaTreeCtrl : CTmaxBaseTreeCtrl
	{
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxMediaTreeCtrl() : base()
		{
			//	Set the default event source name
			m_tmaxEventSource.Name = "Tmax Media Tree Control";
		}
	
		/// <summary>This method is called to get the current selection</summary>
		/// <returns>The node that is currently selected</returns>
		new public CTmaxMediaTreeNode GetSelection()
		{
			return (CTmaxMediaTreeNode)(base.GetSelection());
		}

		/// <summary>This method is called to get the current selections in the tree</summary>
		/// <returns>The nodes that are currently selected sorted by position</returns>
		new public CTmaxMediaTreeNodes GetSelections()
		{
			return GetTmaxNodes(SelectedNodes, true);
		}

		/// <summary>This method is called to get the current selections in the tree</summary>
		/// <param name="bSortByPosition">true if the nodes should be sorted based on their position in the tree</param>
		/// <returns>The nodes that are currently selected sorted by position</returns>
		new public CTmaxMediaTreeNodes GetSelections(bool bSortByPosition)
		{
			return GetTmaxNodes(SelectedNodes, bSortByPosition);
		}

		/// <summary>This method will populate a TrialMax node collection using the specified selected node collection</summary>
		/// <param name="aSelected">The Infragistics tree node collection</param>
		/// <param name="bSortByPosition">True to sort the nodes by their position in the tree</param>
		/// <returns>The populated TrialMax node collection</returns>
		new public CTmaxMediaTreeNodes GetTmaxNodes(Infragistics.Win.UltraWinTree.SelectedNodesCollection aSelected, bool bSortByPosition)
		{
			return (CTmaxMediaTreeNodes)(base.GetTmaxNodes(new CTmaxMediaTreeNodes(), aSelected, bSortByPosition));
		}
		
		/// <summary>This method will populate a TrialMax node collection using the specified selected node collection</summary>
		/// <param name="aSelected">The Infragistics tree node collection</param>
		/// <returns>The populated TrialMax node collection</returns>
		new public CTmaxMediaTreeNodes GetTmaxNodes(Infragistics.Win.UltraWinTree.SelectedNodesCollection aSelected)
		{
			return GetTmaxNodes(aSelected, true);
		}
		
		/// <summary>This method is called to get the node located at the specified point</summary>
		/// <param name="objPoint">The point within the tree's client area</param>
		/// <returns>The node object if it exists</returns>
		new public CTmaxMediaTreeNode GetNode(Point objPoint)
		{
			return (CTmaxMediaTreeNode)(base.GetNode(objPoint));
		}

		/// <summary>This method is called to get the node located at the specified coordinates</summary>
		/// <param name="iX">The x coordinate within the tree's client area</param>
		/// <param name="iY">The y coordinate within the tree's client area</param>
		/// <returns>The node object if it exists</returns>
		new public CTmaxMediaTreeNode GetNode(int iX, int iY)
		{
			return (CTmaxMediaTreeNode)(base.GetNode(iX, iY));
		}
		
		/// <summary>This method is called to get the node at the current cursor position</summary>
		/// <return>The node under the current cursor position</return>
		new public CTmaxMediaTreeNode GetNode()
		{
			return (CTmaxMediaTreeNode)(base.GetNode());
		}
			
		#endregion Public Methods
		
	}// class CTmaxMediaTreeCtrl

	/// <summary>Implements node object used to add nodes to a TrialMax media tree control</summary>
	public class CTmaxMediaTreeNode : CTmaxBaseTreeNode
	{
		#region Private Members
		
		/// <summary>Local member bound to Children property</summary>
		private CTmaxMediaTreeNodes m_tmaxChildren = new CTmaxMediaTreeNodes();
		
		/// <summary>Local member bound to Reposition property</summary>
		private bool m_bAdded = false;
		
		/// <summary>Local member bound to MediaLevel property</summary>
		private TmaxMediaLevels m_eMediaLevel = TmaxMediaLevels.None;
		
		/// <summary>Local member bound to MediaType property</summary>
		private TmaxMediaTypes m_eMediaType = TmaxMediaTypes.Unknown;
		
		/// <summary>Local member bound to IBinder property</summary>
		private ITmaxMediaRecord m_IBinder = null;
		
		/// <summary>Local member bound to IPrimary property</summary>
		private ITmaxMediaRecord m_IPrimary = null;
		
		/// <summary>Local member bound to ISecondary property</summary>
		private ITmaxMediaRecord m_ISecondary = null;
		
		/// <summary>Local member bound to ITertiary property</summary>
		private ITmaxMediaRecord m_ITertiary = null;

		/// <summary>Local member bound to IQuaternary property</summary>
		private ITmaxMediaRecord m_IQuaternary = null;
		
		/// <summary>Local member bound to XmlObject property</summary>
		private CXmlBase m_xmlObject = null;
		
		/// <summary>Local member bound to UserDefined property</summary>
		private object m_userDefined = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxMediaTreeNode() : base()
		{
		
		}// public CTmaxMediaTreeNode() : base()
		
		/// <summary>Constructor</summary>
		/// <param name="strText">Text displayed on the node</param>
		///
		/// <remarks>
		/// We intentionally supply an empty string for the key because we don't care
		/// if the value is unique and it signficantly speeds up the processing
		/// </remarks>
		public CTmaxMediaTreeNode(string strText) : base(strText)
		{
		}
		
		/// <summary>Clears this node's child collections</summary>
		public override void Clear()
		{
			base.Clear();
			
			if(m_tmaxChildren != null)
				m_tmaxChildren.Clear();
				
		}// public virtual void Clear()
		
		/// <summary>This function is called to compare this node to the specified node</summary>
		/// <param name="tmaxNode">The node to be compared</param>
		/// <param name="eField">The field to use for the comparison</param>
		/// <param name="bCaseSensitive">True if the sort should be case sensitive</param>
		/// <returns>-1 if this less than tmaxNode, 0 if equal, 1 if greater than</returns>
		public override int Compare(CTmaxBaseTreeNode tmaxNode, TmaxTreeSortFields eField,CTmaxCaseCode caseCode, bool bCaseSensitive)
		{
            
			ITmaxMediaRecord	tmaxRecord1 = null;
			ITmaxMediaRecord	tmaxRecord2 = null;
			int					iReturn = -1;
		    string strValue1;
            string strValue2;
			
			//	Check for equality first
			//
			//	NOTE:	.NET raises and exception if we don't return 0 for
			//			equal objects
			if(ReferenceEquals(this, tmaxNode) == true)
				return 0;

            tmaxRecord1 = this.GetTmaxRecord(false);
            tmaxRecord2 = ((CTmaxMediaTreeNode)tmaxNode).GetTmaxRecord(false);

			//	Field are we sorting on?
			switch(eField)
			{
				case TmaxTreeSortFields.TreePosition:
				case TmaxTreeSortFields.Text:
				
					//	Let the base class sort these
					return base.Compare(tmaxNode, eField,caseCode, bCaseSensitive);
					
				case TmaxTreeSortFields.Modified:
						
					tmaxRecord1 = this.GetTmaxRecord(false);
					tmaxRecord2 = ((CTmaxMediaTreeNode)tmaxNode).GetTmaxRecord(false);
							
					if((tmaxRecord1 != null) && (tmaxRecord2 != null))
					{
						if(tmaxRecord1.GetModifiedOn() == tmaxRecord2.GetModifiedOn())
						{
							iReturn = 0;
						}
						else if(tmaxRecord1.GetModifiedOn() > tmaxRecord2.GetModifiedOn())
						{
							iReturn = 1;
						}
					}
					break;
							
				case TmaxTreeSortFields.DisplayOrder:
						
					tmaxRecord1 = this.GetTmaxRecord(false);
					tmaxRecord2 = ((CTmaxMediaTreeNode)tmaxNode).GetTmaxRecord(false);
							
					if((tmaxRecord1 != null) && (tmaxRecord2 != null))
					{
						if(tmaxRecord1.GetDisplayOrder() == tmaxRecord2.GetDisplayOrder())
						{
							iReturn = 0;
						}
						else if(tmaxRecord1.GetDisplayOrder() > tmaxRecord2.GetDisplayOrder())
						{
							iReturn = 1;
						}
					}
					break;
                case TmaxTreeSortFields.MediaId:
			        
                    //	Use the database id by default
                    if ((tmaxRecord1 != null) && (tmaxRecord2 != null))
                    {
                        strValue1 = tmaxRecord1.GetMediaId();
                        strValue2 = tmaxRecord2.GetMediaId();
                        if (strValue1.CompareTo(strValue2) == 0)
                        {
                            iReturn = 0;
                        }
                        else if (strValue1.CompareTo(strValue2) > 0)
                        {
                            iReturn = 1;
                        }
                    }
                    break;
                case TmaxTreeSortFields.MediaName:
                    
                    //	Use the database id by default
                    if ((tmaxRecord1 != null) && (tmaxRecord2 != null))
                    {
                        strValue1 = tmaxRecord1.GetName();
                        strValue2 = tmaxRecord2.GetName();
                        if (strValue1.CompareTo(strValue2) == 0)
                        {
                            iReturn = 0;
                        }
                        else if (strValue1.CompareTo(strValue2) > 0)
                        {
                            iReturn = 1;
                        }
                    }
                    break;
                case TmaxTreeSortFields.FiledData:
                    if (caseCode==null) break;

                    if ((tmaxRecord1 != null) && (tmaxRecord2 != null))
                    {
                        iReturn = CompareCodesValue(caseCode, tmaxRecord1, tmaxRecord2, iReturn);
                    }
			        break;
				case TmaxTreeSortFields.DatabaseId:
				case TmaxTreeSortFields.Created:
				default:
						
					tmaxRecord1 = this.GetTmaxRecord(false);
					tmaxRecord2 = ((CTmaxMediaTreeNode)tmaxNode).GetTmaxRecord(false);
							
					//	Use the database id by default
					if((tmaxRecord1 != null) && (tmaxRecord2 != null))
					{
						if(tmaxRecord1.GetAutoId() == tmaxRecord2.GetAutoId())
						{
							iReturn = 0;
						}
						else if(tmaxRecord1.GetAutoId() > tmaxRecord2.GetAutoId())
						{
							iReturn = 1;
						}
					}
					break;
							
			}// switch(eField)

			return iReturn;
		
		}

	    private int CompareCodesValue(CTmaxCaseCode caseCode, ITmaxMediaRecord tmaxRecord1, ITmaxMediaRecord tmaxRecord2, int iReturn)
	    {
	        switch (caseCode.Type)
	        {
	            case TmaxCodeTypes.Date:
	                iReturn = CompareDate(caseCode, tmaxRecord1, tmaxRecord2);
	                break;
	            case TmaxCodeTypes.Boolean:
	                iReturn = CompareBoolean(caseCode, tmaxRecord1, tmaxRecord2);
	                break;
	            case TmaxCodeTypes.Text:
                case TmaxCodeTypes.Memo:
	                iReturn = CompareString(caseCode, tmaxRecord1, tmaxRecord2);
	                break;
	            case TmaxCodeTypes.Integer:
	                iReturn = CompareInteger(caseCode, tmaxRecord1, tmaxRecord2);
	                break;
                case TmaxCodeTypes.Decimal:
                    iReturn = CompareDecimal(caseCode, tmaxRecord1, tmaxRecord2);
	                break;
                case TmaxCodeTypes.PickList:
                    iReturn = ComparePickList(caseCode, tmaxRecord1, tmaxRecord2);
	                break;

                
                    
	        }
	        return iReturn;
	    }

        private int ComparePickList(CTmaxCaseCode caseCode, ITmaxMediaRecord tmaxRecord1, ITmaxMediaRecord tmaxRecord2)
        {
            int iReturn = -1;
            string str1 = tmaxRecord1.GetCodePickItemValue(caseCode);
            string str2 = tmaxRecord2.GetCodePickItemValue(caseCode);
            if (str1 == null && str2 == null)
                iReturn = 0;
            else if (str1 != null)
                iReturn = str1.CompareTo(str2);
            return iReturn;
        }

	    private int CompareDate(CTmaxCaseCode caseCode, ITmaxMediaRecord tmaxRecord1, ITmaxMediaRecord tmaxRecord2)
	    {
            int iReturn = -1;
	        DateTime? date1 = tmaxRecord1.GetCodeDateValue(caseCode);
	        DateTime? date2 = tmaxRecord2.GetCodeDateValue(caseCode);
            if (!date1.HasValue && !date2.HasValue)
                iReturn = 0;
            else if (date1.HasValue && date2.HasValue)
                iReturn = date1.Value.CompareTo(date2.Value);
            else if (date1.HasValue && date2.HasValue == false)
                iReturn = 1;
	        return iReturn;
	    }

	    private int CompareDecimal(CTmaxCaseCode caseCode, ITmaxMediaRecord tmaxRecord1, ITmaxMediaRecord tmaxRecord2)
	    {
            int iReturn = -1;
	        double? decimalValue1 = tmaxRecord1.GetCodeDecimalValue(caseCode);
	        double? decimalValue2 = tmaxRecord2.GetCodeDecimalValue(caseCode);
            if (!decimalValue1.HasValue && !decimalValue2.HasValue)
                iReturn = 0;
            else if (decimalValue1.HasValue && decimalValue2.HasValue)
                iReturn = decimalValue1.Value.CompareTo(decimalValue2.Value);
            else if (decimalValue1.HasValue && decimalValue2.HasValue==false)
                iReturn = 1;
	        return iReturn;
	    }

	    private int CompareInteger(CTmaxCaseCode caseCode, ITmaxMediaRecord tmaxRecord1, ITmaxMediaRecord tmaxRecord2)
	    {
            int iReturn = -1;
	        int? val1 = tmaxRecord1.GetCodeIntValue(caseCode);
	        int? val2 = tmaxRecord2.GetCodeIntValue(caseCode);
            if (!val1.HasValue && !val2.HasValue)
                iReturn = 0;
            else if (val1.HasValue && val2.HasValue)
                iReturn = val1.Value.CompareTo(val2.Value);
            else if (val1.HasValue && val2.HasValue == false)
                iReturn = 1;
	        return iReturn;
	    }

	    private int CompareString(CTmaxCaseCode caseCode, ITmaxMediaRecord tmaxRecord1, ITmaxMediaRecord tmaxRecord2)
	    {
	        int iReturn=-1;
	        string str1 = tmaxRecord1.GetCodeText(caseCode);
	        string str2 = tmaxRecord2.GetCodeText(caseCode);
	        if(str1==null && str2==null)
	            iReturn=0;
	        else if(str1!=null)
	            iReturn = str1.CompareTo(str2);
	        return iReturn;
	    }

	    private int CompareBoolean(CTmaxCaseCode caseCode, ITmaxMediaRecord tmaxRecord1, ITmaxMediaRecord tmaxRecord2)
	    {
            int iReturn = -1;
	        bool? value1 = tmaxRecord1.GetCodeBoolValue(caseCode);
	        bool? value2 = tmaxRecord2.GetCodeBoolValue(caseCode);
            if (!value1.HasValue && !value2.HasValue)
                iReturn = 0;
            else if (value1.HasValue && value2.HasValue)
                iReturn = value1.Value.CompareTo(value2.Value);
            else if (value1.HasValue && value2.HasValue == false)
                iReturn = 1;
	        return iReturn;
	    }

	   

//	public override int Compare(CTmaxBaseTreeNode tmaxNode, TmaxTreeSortFields eField, bool bCaseSensitive)

		/// <summary>Adds the child node to the specified child collections</summary>
		/// <param name="tmaxChild">the child node to be added to the collection(s)</param>
		/// <param name="bTree">true to add the node to the tree's child collection</param>
		/// <param name="bLocal">true to add the node to the local child collection</param>
		/// <returns>true if successful</returns>
		public virtual bool Add(CTmaxMediaTreeNode tmaxChild, bool bTree, bool bLocal)
		{
			try
			{
				if((bTree == true) && (Nodes != null))
				{
					Nodes.Add(tmaxChild);
				}
				if((bLocal == true) && (m_tmaxChildren != null))
				{
					m_tmaxChildren.Add(tmaxChild);
				}
				
				return true;
			}
			catch
			{
				return false;
			}
			
		}// public virtual void Add(CTmaxMediaTreeNode tmaxChild, bool bTree, bool bLocal)
		
		/// <summary>Adds the child node to the tree and the local child collection if requested</summary>
		/// <param name="tmaxChild">the child node to be added to the collection(s)</param>
		/// <param name="bLocal">true to add the node to the local child collection</param>
		/// <returns>true if successful</returns>
		public virtual bool Add(CTmaxMediaTreeNode tmaxChild, bool bLocal)
		{
			return Add(tmaxChild, true, bLocal);
		}
		
		/// <summary>Adds the child node to the tree without adding it to the local child collection</summary>
		/// <param name="tmaxChild">the child node to be added to the tree</param>
		/// <returns>true if successful</returns>
		public virtual bool Add(CTmaxMediaTreeNode tmaxChild)
		{
			return Add(tmaxChild, true, false);
		}
		
		/// <summary>Inserts the child node into the specified child collections at the specified index</summary>
		/// <param name="tmaxChild">the child node to be inserted into the collection(s)</param>
		/// <param name="iIndex">the index of the location where the node is to be inserted</param>
		/// <param name="bTree">true to add the node to the tree's child collection</param>
		/// <param name="bLocal">true to add the node to the local child collection</param>
		/// <returns>true if successful</returns>
		public virtual bool Insert(CTmaxMediaTreeNode tmaxChild, int iIndex, bool bTree, bool bLocal)
		{
			try
			{
				if((bTree == true) && (Nodes != null))
				{
					Nodes.Insert(iIndex, tmaxChild);
				}
				if((bLocal == true) && (m_tmaxChildren != null))
				{
					m_tmaxChildren.Insert(iIndex, tmaxChild);
				}
				
				return true;
			}
			catch
			{
				return false;
			}
			
		}// public virtual bool Insert(CTmaxMediaTreeNode tmaxChild, int iIndex, bool bTree, bool bLocal)
		
		/// <summary>Inserts the child node into the tree and local collection if requested at the specified index</summary>
		/// <param name="tmaxChild">the child node to be inserted into the collection(s)</param>
		/// <param name="iIndex">the index of the location where the node is to be inserted</param>
		/// <param name="bLocal">true to add the node to the local child collection</param>
		/// <returns>true if successful</returns>
		public virtual bool Insert(CTmaxMediaTreeNode tmaxChild, int iIndex, bool bLocal)
		{
			return Insert(tmaxChild, iIndex, true, bLocal);
			
		}// public virtual bool Insert(CTmaxMediaTreeNode tmaxChild, int iIndex, bool bLocal)
		
		/// <summary>Inserts the child node into the tree at the specified index</summary>
		/// <param name="tmaxChild">the child node to be inserted into the collection</param>
		/// <param name="iIndex">the index of the location where the node is to be inserted</param>
		/// <returns>true if successful</returns>
		public virtual bool Insert(CTmaxMediaTreeNode tmaxChild, int iIndex)
		{
			return Insert(tmaxChild, iIndex, true, false);
			
		}// public virtual bool Insert(CTmaxMediaTreeNode tmaxChild, int iIndex)
		
		/// <summary>Removes the child node from the child collections</summary>
		/// <param name="tmaxChild">the child node to be removed</param>
		/// <param name="bTree">true to remove the node from the tree</param>
		/// <param name="bLocal">true to remove the node from the local collection</param>
		public virtual void Remove(CTmaxMediaTreeNode tmaxChild, bool bTree, bool bLocal)
		{
			try
			{
				if((bTree == true) && (Nodes != null))
				{
					if(Nodes.Contains(tmaxChild) == true)
						Nodes.Remove(tmaxChild);
				}
				if((bLocal == true) && (m_tmaxChildren != null))
				{
					if(m_tmaxChildren.Contains(tmaxChild) == true)
						m_tmaxChildren.Remove(tmaxChild);
				}
				
			}
			catch
			{
			}
			
		}// public virtual void Remove(CTmaxMediaTreeNode tmaxChild, bool bTree, bool bLocal)
		
		/// <summary>Removes the child node from the tree and the local collection if requested</summary>
		/// <param name="tmaxChild">the child node to be removed</param>
		/// <param name="bLocal">true to remove the node from the local collection</param>
		public virtual void Remove(CTmaxMediaTreeNode tmaxChild, bool bLocal)
		{
			Remove(tmaxChild, true, bLocal);
			
		}// public virtual void Remove(CTmaxMediaTreeNode tmaxChild, bool bLocal)
		
		/// <summary>Removes the child node from the tree and local collection</summary>
		/// <param name="tmaxChild">the child node to be removed</param>
		public virtual void Remove(CTmaxMediaTreeNode tmaxChild)
		{
			Remove(tmaxChild, true, true);
			
		}// public virtual void Remove(CTmaxMediaTreeNode tmaxChild)
		
		/// <summary>
		/// This method is called to set the default property values
		/// </summary>
		/// <param name="iImage">Index of the image to be applied to the node</param>
		/// <remarks>
		///	Because of the way Infragistics maintains the parent-child hiearchary
		///	this method should NOT be called until AFTER the node is added to the tree
		///	</remarks>
		public virtual void SetProperties(int iImage)
		{
			if(Override != null)
			{
				Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.CheckOnExpand;
				Override.NodeAppearance.Image = iImage;
				
				//	Set the record interfaces
				switch(MediaLevel)
				{
					case TmaxMediaLevels.Primary:
						
						//	Does this primary have any children?
						if((IPrimary != null) && (IPrimary.GetChildCount() > 0))
						{
							Override.AllowAutoDragExpand = Infragistics.Win.UltraWinTree.AllowAutoDragExpand.ExpandOnDragHoverWhenExpansionIndicatorVisible;
							Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.CheckOnExpand;
							//Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.Always;
						}
						else
						{
							Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.CheckOnDisplay;
						}
						break;
							
					case TmaxMediaLevels.Secondary:
						
						//	Does this secondary have any children?
						if((ISecondary != null) && (ISecondary.GetChildCount() > 0))
							Override.AllowAutoDragExpand = Infragistics.Win.UltraWinTree.AllowAutoDragExpand.ExpandOnDragHoverWhenExpansionIndicatorVisible;
						
						if((MediaType == TmaxMediaTypes.Slide) || (MediaType == TmaxMediaTypes.Segment))
						{
							Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.Never;
						}
						else
						{
							if((ISecondary != null) && (ISecondary.GetChildCount() > 0))
								//Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.Always;
								Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.CheckOnExpand;
							else
								Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.CheckOnDisplay;
						
						}
						break;
							
					case TmaxMediaLevels.Tertiary:
					case TmaxMediaLevels.Quaternary:
						
						//	Never show the expansion indicator
						Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.Never;
							
						break;
							
					default:
						
						Override.AllowAutoDragExpand = Infragistics.Win.UltraWinTree.AllowAutoDragExpand.ExpandOnDragHoverWhenExpansionIndicatorVisible;
						Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.CheckOnDisplay;
						break;
							
				}// switch(MediaLevel)
				
			}// if(Override != null)
				
		}// public virtual void SetProperties()
		
		/// <summary>This method is called to set the media record bound to the node</summary>
		/// <param name="tmaxRecord">The record to bind to the node</param>
		public virtual void SetMediaRecord(ITmaxMediaRecord tmaxRecord)
		{
			m_IPrimary		= null;
			m_ISecondary	= null;
			m_ITertiary		= null;
			m_IQuaternary	= null;
			
			if(tmaxRecord != null)
			{
				m_eMediaType = tmaxRecord.GetMediaType();
				m_eMediaLevel = tmaxRecord.GetMediaLevel();
			}
			else
			{
				m_eMediaType = TmaxMediaTypes.Unknown;
				m_eMediaLevel = TmaxMediaLevels.None;
			}
			
			//	Set the record interfaces
			switch(m_eMediaLevel)
			{
				case TmaxMediaLevels.Primary:
					
					m_IPrimary = tmaxRecord;
					break;
						
				case TmaxMediaLevels.Secondary:
					
					m_ISecondary = tmaxRecord;
					m_IPrimary = m_ISecondary.GetParent();
					break;
						
				case TmaxMediaLevels.Tertiary:
					
					m_ITertiary = tmaxRecord;
					m_ISecondary = m_ITertiary.GetParent();
					if(m_ISecondary != null)
						m_IPrimary = m_ISecondary.GetParent();
					break;
						
				case TmaxMediaLevels.Quaternary:
					
					m_IQuaternary = tmaxRecord;
					m_ITertiary = m_IQuaternary.GetParent();
					if(m_ITertiary != null)
						m_ISecondary = m_ITertiary.GetParent();
					if(m_ISecondary != null)
						m_IPrimary = m_ISecondary.GetParent();
					break;
						
				default:
					
					break;
						
			}// switch(m_eMediaLevel)

		}// public virtual void SetMediaRecord(ITmaxMediaRecord tmaxRecord)
		
		/// <summary>This method is called to retrieve the interface to the node's media record</summary>
		/// <returns>The associated media record interface</returns>
		public ITmaxMediaRecord GetMediaRecord()
		{
			return GetTmaxRecord(true);
		}
			
		/// <summary>
		/// This method is called to retrieve the appropriate ITmaxMediaRecord interface object for the node
		/// </summary>
		/// <param name="bIgnoreBinder">true to ignore the binder interface</param>
		/// <returns>The active data exchange interface</returns>
		public ITmaxMediaRecord GetTmaxRecord(bool bIgnoreBinder)
		{
			if((bIgnoreBinder == false) && (m_IBinder != null))
			{
				return m_IBinder;
			}
			else
			{				
				switch(m_eMediaLevel)
				{
					case TmaxMediaLevels.Primary:
					
						Debug.Assert(m_IPrimary != null);
						return m_IPrimary;
						
					case TmaxMediaLevels.Secondary:
					
						Debug.Assert(m_IPrimary != null);
						Debug.Assert(m_ISecondary != null);
						return m_ISecondary;
						
					case TmaxMediaLevels.Tertiary:
					
						Debug.Assert(m_IPrimary != null);
						Debug.Assert(m_ISecondary != null);
						Debug.Assert(m_ITertiary != null);
						return m_ITertiary;
						
					case TmaxMediaLevels.Quaternary:
					
						Debug.Assert(m_IPrimary != null);
						Debug.Assert(m_ISecondary != null);
						Debug.Assert(m_ITertiary != null);
						Debug.Assert(m_IQuaternary != null);
						return m_IQuaternary;
				
					default:
					
						return null;
						
				}// switch(m_eMediaLevel)
			}
		
		}// public ITmaxMediaRecord GetTmaxRecord()
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Level in the heirarchy where this node resides</summary>
		public TmaxMediaLevels MediaLevel
		{
			get { return m_eMediaLevel; }
			set { m_eMediaLevel = value; }
		}
		
		/// <summary>TrialMax media type associated with this node</summary>
		public FTI.Shared.Trialmax.TmaxMediaTypes MediaType
		{
			get { return m_eMediaType; }
			set { m_eMediaType = value; }
		}

        public CTmaxBaseTreeSorter Sorter
        {
            get; set;

        }// Sorter property

		/// <summary>Collection of child nodes belonging to this node</summary>
		public CTmaxMediaTreeNodes Children
		{
			get { return m_tmaxChildren; }
		}
		
		/// <summary>Flag to indicate node has been put in tree once and should be repositioned if moved</summary>
		public bool Added
		{
			get { return m_bAdded; }
			set { m_bAdded = value; }
		}
		
		/// <summary>Interface to binder entry exchange object that owns this node</summary>
		public ITmaxMediaRecord IBinder
		{
			get { return m_IBinder; }
			set { m_IBinder = value; }
		}
		
		/// <summary>Interface to primary data exchange object that owns this node</summary>
		public ITmaxMediaRecord IPrimary
		{
			get { return m_IPrimary; }
			set { m_IPrimary = value; }
		}
		
		/// <summary>Interface to secondary data exchange object associated with this node</summary>
		public ITmaxMediaRecord ISecondary
		{
			get { return m_ISecondary; }
			set { m_ISecondary = value; }
		}
		
		/// <summary>Interface to tertiary data exchange object associated with this node</summary>
		public ITmaxMediaRecord ITertiary
		{
			get { return m_ITertiary; }
			set { m_ITertiary = value; }
 		}
		
		/// <summary>Interface to quaternary data exchange object that owns this node</summary>
		public ITmaxMediaRecord IQuaternary
		{
			get { return m_IQuaternary; }
			set { m_IQuaternary = value; }
		}
		
		/// <summary>XML object to which this node is bound</summary>
		public FTI.Shared.Xml.CXmlBase XmlObject
		{
			get { return m_xmlObject; }
			set { m_xmlObject = value; }
		}
		
		/// <summary>User defined object to which this node is bound</summary>
		public object UserDefined
		{
			get { return m_userDefined; }
			set { m_userDefined = value; }
		}
		
		#endregion Properties
		
	}// class CTmaxMediaTreeNode

	/// <summary>
	/// Objects of this class are used to manage a dynamic array of CTmaxMediaTreeNodes objects
	/// </summary>
	public class CTmaxMediaTreeNodes : CTmaxBaseTreeNodes
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CTmaxMediaTreeNodes() : base()
		{
		}

		/// <summary>Overloaded constructor</summary>
		public CTmaxMediaTreeNodes(CTmaxBaseTreeSorter tmaxSorter) : base(tmaxSorter)
		{
		}

		/// <summary>This method is called to remove the requested filter from the collection</summary>
		/// <param name="tmaxNode">The filter object to be removed</param>
		public void Remove(CTmaxMediaTreeNode tmaxNode)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(tmaxNode as CTmaxBaseTreeNode);
			}
			catch
			{
			}
		
		}// public void Remove(CTmaxMediaTreeNode tmaxNode)

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="tmaxNode">The object to be checked</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CTmaxMediaTreeNode tmaxNode)
		{
			// Use base class to process actual collection operation
			return base.Contains(tmaxNode as CTmaxBaseTreeNode);
		
		}// public bool Contains(CTmaxMediaTreeNode tmaxNode)

		/// <summary>Gets the object at the specified index</summary>
		/// <returns>The object at the requested index</returns>
		public new CTmaxMediaTreeNode GetAt(int index)
		{
			return (base.GetAt(index) as CTmaxMediaTreeNode);
		}

		/// <summary>Gets the object that appears after the specified reference object</summary>
		/// <param name="O">The reference object</param>
		/// <returns>The next object</returns>
		public new CTmaxMediaTreeNode GetNext(object O)
		{
			return (base.GetNext(O) as CTmaxMediaTreeNode);
		}

		/// <summary>Gets the object that appears before the specified reference object</summary>
		/// <param name="O">The reference object</param>
		/// <returns>The previous object</returns>
		public new CTmaxMediaTreeNode GetPrevious(object O)
		{
			return (base.GetPrevious(O) as CTmaxMediaTreeNode);
		}

		/// <summary>Gets the first object in the collection</summary>
		/// <returns>The first object</returns>
		public new CTmaxMediaTreeNode GetFirst()
		{
			return (base.GetFirst() as CTmaxMediaTreeNode);
		}

		/// <summary>Gets the last object in the collection</summary>
		/// <returns>The last object</returns>
		public new CTmaxMediaTreeNode GetLast()
		{
			return (base.GetLast() as CTmaxMediaTreeNode);
		}

		/// <summary>Gets the node with the specified AutoId</summary>
		/// <param name="lAutoId">The desired AutoId</param>
		/// <returns>The node with the specified AutoId if found</returns>
		public CTmaxMediaTreeNode Find(long lAutoId)
		{
			ITmaxMediaRecord tmaxRecord = null;
			
			foreach(CTmaxMediaTreeNode tmaxNode in this)
			{
				if((tmaxRecord = tmaxNode.GetTmaxRecord(true)) != null)
				{
					if(tmaxRecord.GetAutoId() == lAutoId)
						return tmaxNode;
				}
				
			}
			
			//	Not found...
			return null;
		
		}// public CTmaxMediaTreeNode Find(long lAutoId)
		
		/// <summary>Overloaded version of [] operator to return the filter object at the desired index</summary>
		/// <returns>Filter object at the specified index</returns>
		public new CTmaxMediaTreeNode this[int index]
		{
			get { return GetAt(index); }
		}

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CTmaxMediaTreeNode value)
		{
			// Find the 0 based index of the requested entry
			return base.IndexOf(value);
		}

		#endregion Public Methods
		
	}//	public class CTmaxMediaTreeNodes
		
}// namespace FTI.Trialmax.Controls
