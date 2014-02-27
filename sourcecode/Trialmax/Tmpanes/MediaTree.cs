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
	/// This class creates and manages the tree that gives the user a physical
	/// view of the database contents 
	/// </summary>
	public class CMediaTree : FTI.Trialmax.Panes.CTreePane
	{
		#region Constants
		
		protected const string KEY_PRIMARY_SORT_FIELD			= "PrimarySortField";
        protected const string KEY_SORT_FIELD_DATA              = "SortFieldData";
		protected const string KEY_PRIMARY_SORT_ASCENDING		= "PrimarySortAscending";
		protected const string KEY_PRIMARY_SORT_CASE_SENSITIVE	= "PrimarySortCaseSensitive";
		protected const string KEY_SUPER_NODE_SIZE				= "SuperNodeSize";
		
		#endregion Constants
		
		#region Protected Members
		
		/// <summary>Local collection used to store root media nodes</summary>
		protected CMediaNodes m_aMediaNodes = new CMediaNodes();

		/// <summary>Class member bound to the Filtered property</summary>
		protected bool m_bShowFiltered = false;
		
		#endregion Protected Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CMediaTree() : base()
		{
			if(m_tmaxTreeCtrl != null)
				m_tmaxTreeCtrl.EventSource.Name = "Media TreeCtrl";

			//	Allocate the sorter interface for primary media nodes
			m_tmaxPrimarySorter = new FTI.Trialmax.Controls.CTmaxBaseTreeSorter();
			m_tmaxPrimarySorter.EventSource.Name = "PT Primary Sorter";
			m_tmaxEventSource.Attach(m_tmaxPrimarySorter.EventSource);
		
			//	Register events from the media node collection
			m_tmaxEventSource.Attach(m_aMediaNodes.EventSource);
		}

		/// <summary>Called by the application to synchronize the selection in the tree</summary>
		/// <param name="tmaxItem">The item to be selected</param>
		/// <param name="bActivate">True to fire the Activate event for the item</param>
		/// <param name="bEnsureVisible">True to ensure the node is visible</param>
		/// <returns>True if successful</returns>
		public virtual bool Select(CTmaxItem tmaxItem, bool bActivate, bool bEnsureVisible)
		{
			CTmaxMediaTreeNode	tmaxNode = null;
			bool				bSuccessful = false;

			//	Get the requested node
			if((tmaxNode = GetNode(tmaxItem, true)) == null)
			{
				m_tmaxTreeCtrl.SetSelection(null);
				return false;
			}
			
			try
			{
				//	Inhibit processing of the selection
				m_bIgnoreSelection = true;
					
				//	Select this node
				m_tmaxTreeCtrl.SetSelection(tmaxNode);
					
				//	Should we make sure this node is visible?
				if(bEnsureVisible == true)
					m_tmaxTreeCtrl.EnsureVisible(tmaxNode);
					
				//	Should we activate this node?
				if(bActivate == true)
					FireAsyncCommand(TmaxCommands.Activate, tmaxNode);
						
				bSuccessful = true;
			}
			catch
			{
			}
			finally
			{
				m_bIgnoreSelection = false;
			}
			
			return bSuccessful;		

		}// public virtual bool Select(CTmaxItem tmaxItem, bool bActivate, bool bEnsureVisible)
		
		/// <summary>This method is called by the application when new media gets registered</summary>
		/// <param name="tmaxFolder">The source folder containing the new media</param>
		/// <param name="ePane">The pane that accepted the registration request</param>
		public override void OnRegistered(CTmaxSourceFolder tmaxSource, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
			//	Don't respond if displaying a filtered collection
			if(this.ShowFiltered == true) return;
			
			//	Make sure we have the required objects
			if(tmaxSource == null) return;
			if(m_tmaxTreeCtrl == null) return;
			if(m_aMediaNodes == null) return;
			
			//	Disable Paint events
			m_tmaxTreeCtrl.BeginUpdate();
		
			//	Show the wait cursor
			Cursor.Current = Cursors.WaitCursor;
			
			//	Add the new media to the tree
			AddRegistered(tmaxSource);

			//	Sort the primaries to match the tree
			SortPrimaries();
			
			//	Restore the system cursor
			Cursor.Current = Cursors.Default;
			
			//	Reenable Paint events
			m_tmaxTreeCtrl.EndUpdate();
		
		}// public override void OnRegistered(CTmaxSourceFolder tmaxSource, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		
		/// <summary>This method is called by the application to when the item's child collection has been reordered</summary>
		/// <param name="tmaxItem">The item that owns the child collection</param>
		public override void OnReordered(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{
			CTmaxMediaTreeNodes	tmaxScenes = null;
			CTmaxMediaTreeNode	tmaxNode = null;

			//	Ignore case codes
			if(tmaxItem.DataType == TmaxDataTypes.CaseCode) return;
			
			//	Is this item a designation or clip?
			if((tmaxItem.MediaType == TmaxMediaTypes.Designation) ||
			   (tmaxItem.MediaType == TmaxMediaTypes.Clip))
			{
				//	We need to locate all scenes that reference this item
				tmaxScenes = new CTmaxMediaTreeNodes();
				GetScenesFromSource(tmaxScenes, (CDxMediaRecord)tmaxItem.GetMediaRecord());

				//	Now refresh each scene since the children appear under the scene
				//	node instead of the designation/clip node
				foreach(CTmaxMediaTreeNode O in tmaxScenes)
					Refresh(O);
					
				tmaxScenes.Clear();
			}
			else
			{
				//	Locate the node whose children have been reordered
				if((tmaxNode = GetNode(tmaxItem, false)) != null)
				{
					if(tmaxNode.MediaLevel != TmaxMediaLevels.None)
					{
						//	Refresh the node
						Refresh(tmaxNode);
					
						//	Refresh all scenes because the user may have just
						//	modified a record that is a source for a scene
						if(tmaxNode.MediaType != TmaxMediaTypes.Script)
							RefreshScenes();		
					}
				}
			}

		}// public override void OnReordered(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		/// <summary>This method is called by the application when it adds new media to the database</summary>
		/// <param name="tmaxParent">TrialMax event item that identifies the parent record</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		{
			CTmaxMediaTreeNode	tmaxNode = null;
			CMediaNode			mediaNode = null;
			
			Debug.Assert(tmaxParent != null, "NULL Parent");
			Debug.Assert(tmaxChildren != null, "NULL Children");
			
			//	All new media should be in the child collection
			Debug.Assert(tmaxChildren.Count > 0, "NO Children");
			if(tmaxChildren.Count == 0) return;
		
			//	Are we adding a new primary node?
			if(tmaxParent.MediaLevel == TmaxMediaLevels.None)
			{
				//	Only add nodes for new primary records if not displaying a filtered
				//	collection
				if(this.ShowFiltered == false)
				{
					foreach(CTmaxItem tmaxChild in tmaxChildren)
					{
						//	Get the media node associated with this type
						if((mediaNode = m_aMediaNodes.Find(tmaxChild.MediaType)) != null)
						{
							if(tmaxChild.IPrimary != null)
							{
								Add(mediaNode, (CDxPrimary)tmaxChild.IPrimary, true);

								//	Now make sure the collection is sorted
								SortPrimaries();

							}
						}
					}
				
				}// if(this.ShowFiltered == false)

			}
			
			//	Are we adding new links?
			//
			//	NOTE:	We have to treat links specially because they do not appear
			//			in the tree under their true parent
			else if((tmaxParent.MediaType == TmaxMediaTypes.Designation) ||
					(tmaxParent.MediaType == TmaxMediaTypes.Clip))
			{
				Debug.Assert(tmaxParent.ITertiary != null);
				
				if(tmaxParent.ITertiary != null)
					AddLinks((CDxTertiary)tmaxParent.ITertiary, tmaxChildren);
			}
			
			else
			{
				//	Get the node associated with the parent
				if((tmaxNode = GetNode(tmaxParent, true)) == null) 
					return;

				//	Don't add designations and clips
				if(tmaxNode.GetMediaRecord().GetMediaType() == TmaxMediaTypes.Segment)
					return;
					
				//	Should we add the child nodes now or wait for the 
				//	the user to expand the node?
				if((tmaxNode.Nodes != null) && 
				   ((tmaxNode.Nodes.Count > 0) || (tmaxNode.Expanded == true)))
				{
					//	Add the new children to the collection
					foreach(CTmaxItem tmaxChild in tmaxChildren)
					{
						if(tmaxChild.GetMediaRecord() != null)
						{
							if(Find(tmaxNode, tmaxChild.GetMediaRecord()) == null)
								Add(tmaxNode, tmaxChild.GetMediaRecord());
						}
					}
						
				}
				else
				{
					//	Make sure the user can expand the node
					if(tmaxNode.HasExpansionIndicator == false)
					{
						tmaxNode.Override.ShowExpansionIndicator = ShowExpansionIndicator.Always;
					}
				}
						
				//	Make sure all text and sorting is in order
				Refresh(tmaxNode);
				
				//	Refresh all scenes because the user may have just
				//	modified a record that is a source for a scene
				if(tmaxNode.MediaType != TmaxMediaTypes.Script)
					RefreshScenes();		
			
			}
			
		}// public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		
		/// <summary>This method is called by the application when records have been deleted</summary>
		/// <param name="tmaxItems">A collection of items that represent the parents of the records that have been deleted</param>
		public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			//	The collection specified by the caller is a collection of parent records
			foreach(CTmaxItem tmaxParent in tmaxItems)
			{
				//	Iterate all the children that have been deleted
				foreach(CTmaxItem tmaxChild in tmaxParent.SubItems)
				{
					OnDeleted(tmaxChild);
				}
				
			}

		}// public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)

		/// <summary>This method is called by the application to initialize the pane</summary>
		/// <returns>true if successful</returns>
		/// <remarks>Derived classes should override for custom runtime initialization</remarks>
		public override bool Initialize(CXmlIni xmlINI)
		{
			//	Do the base class processing first
			if(base.Initialize(xmlINI) == false)
				return false;
				
			//	Make sure we are on the correct section
			if(xmlINI.SetSection(m_strPaneName) == false) return false;
				
			m_iSuperNodeSize = xmlINI.ReadInteger(KEY_SUPER_NODE_SIZE, m_iSuperNodeSize);

			//	Save the sorting preferences to the ini file
			if(ReferenceEquals(m_tmaxPrimarySorter, null) == false)
			{
				m_tmaxPrimarySorter.SortBy = (TmaxTreeSortFields)xmlINI.ReadEnum(KEY_PRIMARY_SORT_FIELD, m_tmaxPrimarySorter.SortBy);
				m_tmaxPrimarySorter.Ascending = xmlINI.ReadBool(KEY_PRIMARY_SORT_ASCENDING, m_tmaxPrimarySorter.Ascending);
                m_tmaxPrimarySorter.CaseCodeId = xmlINI.ReadLong(KEY_SORT_FIELD_DATA, m_tmaxPrimarySorter.CaseCodeId); 
				m_tmaxPrimarySorter.CaseSensitive = xmlINI.ReadBool(KEY_PRIMARY_SORT_CASE_SENSITIVE, m_tmaxPrimarySorter.CaseSensitive);
                //if (m_tmaxPrimarySorter.CaseCodeId > 0)
                
			}
			
			return true;
		}

        protected override void OnCaseCodesChanged()
        {
            if (this.CaseCodes != null)
                m_tmaxPrimarySorter.CaseCode = this.CaseCodes.Find(m_tmaxPrimarySorter.CaseCodeId);
        }

	    /// <summary>This method is called by the application when it is about to terminate</summary>
		/// <remarks>Derived classes should override for custom shutdown</remarks>
		public override void Terminate(CXmlIni xmlINI)
		{
			//	Save the sorting preferences to the ini file
            if (m_tmaxPrimarySorter != null)
            {
                if (xmlINI.SetSection(m_strPaneName) == true)
                {
                    xmlINI.Write(KEY_SUPER_NODE_SIZE, m_iSuperNodeSize);
                    xmlINI.Write(KEY_PRIMARY_SORT_FIELD, m_tmaxPrimarySorter.SortBy);
                    if (m_tmaxPrimarySorter.CaseCode != null)
                        xmlINI.Write(KEY_SORT_FIELD_DATA, m_tmaxPrimarySorter.CaseCode.UniqueId);
                    else
                        xmlINI.Write(KEY_SORT_FIELD_DATA, 0);
                    xmlINI.Write(KEY_PRIMARY_SORT_ASCENDING, m_tmaxPrimarySorter.Ascending);
                    xmlINI.Write(KEY_PRIMARY_SORT_CASE_SENSITIVE, m_tmaxPrimarySorter.CaseSensitive);
                }
            }

		    //	Do the base class termination
			base.Terminate(xmlINI);
			
		}// public override void Terminate(CXmlIni xmlINI)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		///	<summary>This method will set the text for the specified node</summary>
		/// <param name="tmaxNode">The node who's text is to be set</param>
		/// <param name="bChildren">true to set the text for child nodes</param>
		protected override void SetText(CTmaxMediaTreeNode tmaxNode, bool bChildren)
		{
			//	Is this a super node?
			if(tmaxNode.MediaLevel == TmaxMediaLevels.None)
			{
				if(m_aMediaNodes != null)
					m_aMediaNodes.SetText(tmaxNode);
			}
			else
			{
				//	Do the base class processing
				base.SetText(tmaxNode, bChildren);
			}
			
		}// protected override void SetText(CTmaxMediaTreeNode tmaxNode)
			
		/// <summary>This method is called when the draw filter wants to know which drop positions are allowed
		/// <returns>The allowed drop positions for source files</returns>
		protected override TmaxTreePositions GetDropSourcePositionsAllowed()
		{	
			if(m_dropTarget.eAction != TreeDropActions.None)
				return TmaxTreePositions.OnNode;
			else
				return TmaxTreePositions.None;
		
		}// protected override TmaxTreePositions GetSourceDropPositions()
		
		/// <summary>This method will retrieve the appropriate drop action when dragging source files</summary>
		/// <returns>The appropriate drop action</returns>
		protected override TreeDropActions GetDropSourceAction()
		{
			//	Is the user over a node?
			if((m_dropTarget.node != null) && (m_dropTarget.node.MediaType != m_eSourceMediaType))
			{
				return TreeDropActions.None;
			}
			else
			{
				return TreeDropActions.Add;
			}
			
		}// protected override TreeDropActions GetDropSourceAction()
		
		/// <summary>This method is called to give the derived class an opprotunity to modify the drop node before highlighting is performed</summary>
		/// <param name="CursorPosition">Current cursor position in client coordinates</param>
		protected override void AdjustDropNode(ref Point CursorPosition)
		{
			//	Is there anything to adjust?
			if((m_eDragState == PaneDragStates.Source) && (m_dropTarget.node != null))
			{
				//	Is this a valid drop node?
				if(m_dropTarget.node.MediaType == m_eSourceMediaType)
				{
					//	Reset the drop node to be the root media node
					m_dropTarget.node = m_aMediaNodes.FindTreeNode(m_eSourceMediaType);
					
					//	Make it look like the cursor is on the node
					//
					//	NOTE:	This tricks the draw filter into thinking that the cursor is on the
					//			highlight node
					CursorPosition.X = m_dropTarget.node.Bounds.Left + (m_dropTarget.node.Bounds.Width / 2);
					CursorPosition.Y = m_dropTarget.node.Bounds.Top + (m_dropTarget.node.Bounds.Height / 2);
				}
				
			}
			
		}// protected override void AdjustDropNode(ref Point CursorPosition)
		
		/// <summary>This function is called when the value of the Database property changes</summary>
		protected override void OnDatabaseChanged()
		{
			//	Do the base class processing first
			base.OnDatabaseChanged();
			
			if((m_tmaxTreeCtrl != null) && (m_tmaxTreeCtrl.Nodes != null))
			{
				//	Are we using a filtered collection?
				if(this.ShowFiltered == true)
				{
					//	Wait for the OnFilteredChanged() notification
					Clear();
				}
				else
				{
					//	Do we have a valid database?
					if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null))
					{
						Populate(m_tmaxDatabase.Primaries);					
					}
					else
					{
						Clear();
					}
					
				}// if(this.ShowFiltered == true)
				
			}// if((m_tmaxTreeCtrl != null) && (m_tmaxTreeCtrl.Nodes != null))
			
		}// protected override void OnDatabaseChanged()
		
		/// <summary>This function is called when the value of the Filtered property changes</summary>
		protected override void OnFilteredChanged()
		{
			//	Ignore this notification if not showing a filtered collection
			if(this.ShowFiltered == true)
			{
				if((m_dxFiltered != null) && (m_dxFiltered.Count > 0))
				{
					Populate(m_dxFiltered);
					
					//	Expand the media nodes
					foreach(CMediaNode O in m_aMediaNodes)
					{
						if(O.Node != null)
							O.Node.Expanded = true;
					}
					
				}
				else
				{
					Clear();
				}
			}
		
		}// protected override void OnFilteredChanged()
		
		/// <summary>
		/// This method is called to notify the derived class when the value of the PrimaryTextMode property changes
		/// </summary>
		protected override void OnPrimaryTextModeChanged()
		{
			if((m_aMediaNodes != null) && (m_aMediaNodes.Count > 0))
				m_aMediaNodes.SetPrimaryTextMode(m_ePrimaryTextMode);
		}
		
		/// <summary>This method is called when the user has dropped data records in the tree</summary>
		/// <param name="eAction">The action to be taken</param>
		protected override void OnDroppedRecords(TreeDropActions eAction)
		{
			//	Check these various conditions before going on
			if(m_dropTarget.node == null) return;
			if(m_dropTarget.node.IPrimary == null) return;
			if(m_tmaxDragData == null) return;
			if(m_tmaxDragData.SourceItems == null) return;
			if(m_tmaxDragData.SourceItems.Count == 0) return;
			if(m_tmaxDragData.SourceItems.ContainsMedia(true) == false) return;

			//	Are we reordering records?
			if(eAction == TreeDropActions.Reorder)
			{
				//	The drop node must be secondary or lower
				if(m_dropTarget.node.ISecondary == null) return;
				
				Reorder(m_tmaxDragData.SourceItems, m_dropTarget.node, (m_dropTarget.ePosition == TmaxTreePositions.AboveNode));
			}
			else
			{
				//	For media to be added the drop target must be a script
				//	or a scene
				if(m_dropTarget.node.MediaType == TmaxMediaTypes.Script)
				{
					//	Add drag media to the end of the script
					Add(m_dropTarget.node, m_tmaxDragData.SourceItems, false, false);
				}
				else if(m_dropTarget.node.MediaType == TmaxMediaTypes.Scene)
				{
					//	Insert before?
					if(m_dropTarget.ePosition == TmaxTreePositions.AboveNode)
						Add(m_dropTarget.node, m_tmaxDragData.SourceItems, true, true);
					else
						Add(m_dropTarget.node, m_tmaxDragData.SourceItems, true, false);
				}
				
			}
			
		}// protected override void OnDroppedRecords(TreeDropActions eAction)
		
		/// <summary>
		/// This method is called by the application to notify the panes to refresh their text
		/// </summary>
		public override void RefreshText()
		{
			//	Set text for all media levels
			try { SetText(TmaxMediaLevels.Secondary); }	catch{}
			try { SetText(TmaxMediaLevels.Tertiary); } catch{}
			try { SetText(TmaxMediaLevels.Quaternary); } catch{}

			if((m_aMediaNodes != null) && (m_aMediaNodes.Count > 0))
			{
				m_aMediaNodes.SetPrimaryTextMode(m_ePrimaryTextMode);
				
				if((m_tmaxPrimarySorter != null) && (m_tmaxPrimarySorter.SortBy == TmaxTreeSortFields.Text))
				{
					m_aMediaNodes.Sort();
				}

			}
					
		}// public override void RefreshText()
		
		/// <summary>This method is called when the user sets the preferences</summary>
		/// <param name="tmaxPreferences">The new set of preferences</param>
		protected override void OnPreferencesChanged(CFTreePreferences tmaxPreferences)
		{
			bool bSort = false;

			//	Display the wait cursor
			Cursor.Current = Cursors.WaitCursor;
			
			//	Suspend painting until we finish making changes
			m_tmaxTreeCtrl.BeginUpdate();
			
			try
			{
				if(tmaxPreferences.QuaternaryTextMode != QuaternaryTextMode)
					QuaternaryTextMode = tmaxPreferences.QuaternaryTextMode;
				if(tmaxPreferences.TertiaryTextMode != TertiaryTextMode)
					TertiaryTextMode = tmaxPreferences.TertiaryTextMode;
				if(tmaxPreferences.SecondaryTextMode != SecondaryTextMode)
					SecondaryTextMode = tmaxPreferences.SecondaryTextMode;
			
				if(tmaxPreferences.PrimaryTextMode != PrimaryTextMode)
				{
					//	Update the local bounded member
					m_ePrimaryTextMode = tmaxPreferences.PrimaryTextMode;
					
					if((m_aMediaNodes != null) && (m_aMediaNodes.Count > 0))
					{
						m_aMediaNodes.SetPrimaryTextMode(m_ePrimaryTextMode);
					}
					
					if((tmaxPreferences.PrimarySorter != null) && (tmaxPreferences.PrimarySorter.SortBy == TmaxTreeSortFields.Text))
					{
						bSort = true;
					}
					else
					{
						//	Make sure the super nodes have the appropriate text
						if((m_aMediaNodes != null) && (m_aMediaNodes.Count > 0))
						{
							m_aMediaNodes.ResetSuperNodeText();
						}
						
					}
					
				}// if(tmaxPreferences.PrimaryTextMode != PrimaryTextMode)
				
				if((m_tmaxPrimarySorter != null) && (tmaxPreferences.PrimarySorter != null))
				{
						//	Update the sort options
						m_tmaxPrimarySorter.Copy(tmaxPreferences.PrimarySorter);
						bSort = true;	
				}
				
				if((m_aMediaNodes != null) && (m_aMediaNodes.Count > 0))
				{
					//	Do we need to rebuild?
					if(tmaxPreferences.SuperNodeSize != m_iSuperNodeSize)
					{
						m_iSuperNodeSize = tmaxPreferences.SuperNodeSize;
						m_aMediaNodes.SetSuperNodeSize(m_iSuperNodeSize);
						
						m_aMediaNodes.Rebuild(bSort);
					}
					else if(bSort == true)
					{
						m_aMediaNodes.Sort();
					}
					
					//	Keep the primary collection sorted
					if(bSort == true)
						SortPrimaries();
				
				}// if((m_aMediaNodes != null) && (m_aMediaNodes.Count > 0))
			}
			catch(System.Exception Ex)
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
		
		/// <summary>This method is called to get the list of items used to fire the print command</summary>
		/// <param name="tmaxNodes">The collection of selected nodes</param>
		protected override CTmaxItems GetCmdPrintItems(CTmaxMediaTreeNodes tmaxNodes)
		{
			CMediaNode		mediaNode = null;
			CTmaxItems		tmaxItems = null;
			
			if(tmaxNodes == null) return null;
			if(tmaxNodes.Count == 0) return null;
			
			//	Has the user selected media nodes?
			if(tmaxNodes[0].IPrimary != null)
			{
				//	Let the base class process the request
				return base.GetCmdPrintItems(tmaxNodes);
			}
			else
			{
				tmaxItems = new CTmaxItems();
				
				//	These must be media nodes or super nodes
				foreach(CTmaxMediaTreeNode O in tmaxNodes)
				{
					//	Don't allow printing of depositions
					if((O.MediaType != TmaxMediaTypes.Unknown) && 
					   (O.MediaType != TmaxMediaTypes.Deposition))
					{
						//	Get the media node for this type
						if((mediaNode = m_aMediaNodes.Find(O.MediaType)) != null)
						{
							//	Is this the actual media node that is selected?
							if(ReferenceEquals(O, mediaNode.Node) == true)
							{
								//	Add an item for each primary record
								if(mediaNode.Primaries != null)
								{
									foreach(CTmaxMediaTreeNode tmaxPrimary in mediaNode.Primaries)
									{
										tmaxItems.Add(GetCommandItem(tmaxPrimary, false));
									}
									
								}// if(mediaNode.Primaries != null)
										
							}
							else
							{
								//	This must be a super node so we add the primaries associated with the super node
								//
								//	NOTE:	We use the Children collection because the nodes may not have
								//			actually been added to the tree yet
								if(O.Children != null)
								{
									foreach(CTmaxMediaTreeNode tmaxPrimary in O.Children)
									{
										tmaxItems.Add(GetCommandItem(tmaxPrimary, false));
									}
									
								}
							
							}// if(ReferenceEquals(O, mediaNode.Node) == true)
						
						}// if((mediaNode = m_aMediaNodes.Find(O.MediaType)) != null)
						
					}// if((O.MediaType != TmaxMediaTypes.Unknown) && (O.MediaType != TmaxMediaTypes.Deposition))
					
				}// foreach(CTmaxMediaTreeNode O in tmaxNodes)
				
				return tmaxItems;
			}
			
		}// protected override CTmaxItems GetCmdPrintItems(CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method is called to get the list of items that identify the records to be included in the report</summary>
		///	<param name="eReport">The enumerated report identifier</param>
		/// <param name="tmaxNodes">The collection of selected nodes</param>
		/// <returns>The event items that represent the records to be included in the report</returns>
		protected override CTmaxItems GetCmdReportItems(TmaxReports eReport, CTmaxMediaTreeNodes tmaxNodes)
		{
			CMediaNode		mediaNode = null;
			CTmaxItems		tmaxItems = null;
			
			if(tmaxNodes == null) return null;
			if(tmaxNodes.Count == 0) return null;
			
			//	Has the user selected media nodes?
			if(tmaxNodes[0].IPrimary != null)
			{
				//	Let the base class process the request
				return base.GetCmdReportItems(eReport, tmaxNodes);
			}
			else
			{
				tmaxItems = new CTmaxItems();
				
				//	These must be media nodes or super nodes
				foreach(CTmaxMediaTreeNode O in tmaxNodes)
				{
					//	Only process scripts if this is a scripts report
					if(eReport == TmaxReports.Scripts)
					{
						if(O.MediaType != TmaxMediaTypes.Script) continue;
					}
					
					//	Only process scripts and depositions if this is a transcript or objections report
					else if((eReport == TmaxReports.Transcript) ||
							(eReport == TmaxReports.Objections))
					{
						if((O.MediaType != TmaxMediaTypes.Script) && 
						   (O.MediaType != TmaxMediaTypes.Deposition)) continue;
					}
					
					//	Get the media node for this type
					if((mediaNode = m_aMediaNodes.Find(O.MediaType)) != null)
					{
						//	Is this the actual media node that is selected?
						if(ReferenceEquals(O, mediaNode.Node) == true)
						{
							//	Add an item for each primary record
							if(mediaNode.Primaries != null)
							{
								foreach(CTmaxMediaTreeNode tmaxPrimary in mediaNode.Primaries)
								{
									tmaxItems.Add(GetCommandItem(tmaxPrimary, false));
								}
								
							}// if(mediaNode.Primaries != null)
									
						}
						else
						{
							//	This must be a super node so we add the primaries associated with the super node
							//
							//	NOTE:	We use the Children collection because the nodes may not have
							//			actually been added to the tree yet
							if(O.Children != null)
							{
								foreach(CTmaxMediaTreeNode tmaxPrimary in O.Children)
								{
									tmaxItems.Add(GetCommandItem(tmaxPrimary, false));
								}
								
							}
						
						}// if(ReferenceEquals(O, mediaNode.Node) == true)
					
					}// if((mediaNode = m_aMediaNodes.Find(O.MediaType)) != null)
					
				}// foreach(CTmaxMediaTreeNode O in tmaxNodes)
				
				return tmaxItems;
			
			}// if(tmaxNodes[0].IPrimary != null)
			
		}// protected override CTmaxItems GetCmdReportItems(CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method is called to get the list of items used to fire the print command</summary>
		/// <param name="tmaxNodes">The collection of selected nodes</param>
		protected override CTmaxItems GetCmdFindItems(CTmaxMediaTreeNodes tmaxNodes)
		{
			CMediaNode		mediaNode = null;
			CTmaxItems		tmaxItems = null;
			CTmaxItem		tmaxItem = null;
			
			if(tmaxNodes == null) return null;
			if(tmaxNodes.Count == 0) return null;
			
			//	Has the user selected media nodes?
			if(tmaxNodes[0].IPrimary != null)
			{
				//	Let the base class process the request
				return base.GetCmdFindItems(tmaxNodes);
			}
			else
			{
				tmaxItems = new CTmaxItems();
				
				//	These must be media nodes or super nodes
				foreach(CTmaxMediaTreeNode O in tmaxNodes)
				{
					//	Only allow searching of scripts and depositions
					if((O.MediaType == TmaxMediaTypes.Script) || (O.MediaType == TmaxMediaTypes.Deposition))
					{
						//	Get the media node for this type
						if((mediaNode = m_aMediaNodes.Find(O.MediaType)) != null)
						{
							//	Is this the actual media node that is selected?
							if(ReferenceEquals(O, mediaNode.Node) == true)
							{
								//	Add an item that identifies only the media type
								tmaxItem = new CTmaxItem();
								tmaxItem.MediaType = O.MediaType;
								tmaxItems.Add(tmaxItem);
										
							}
							else
							{
								//	This must be a super node so we add the primaries associated with the super node
								//
								//	NOTE:	We use the Children collection because the nodes may not have
								//			actually been added to the tree yet
								if(O.Children != null)
								{
									foreach(CTmaxMediaTreeNode tmaxPrimary in O.Children)
									{
										tmaxItems.Add(GetCommandItem(tmaxPrimary, false));
									}
									
								}
							
							}// if(ReferenceEquals(O, mediaNode.Node) == true)
						
						}// if((mediaNode = m_aMediaNodes.Find(O.MediaType)) != null)
						
					}// if((O.MediaType == TmaxMediaTypes.Script) || (O.MediaType == TmaxMediaTypes.Deposition))
					
				}// foreach(CTmaxMediaTreeNode O in tmaxNodes)
				
				return tmaxItems;
			}
			
		}// protected override CTmaxItems GetCmdFindItems(CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method is called to get the list of items used to fire the Clean command event</summary>
		/// <param name="tmaxNodes">The collection of selected nodes</param>
		/// <returns>The collection of items to be fired with the event</returns>
		protected override CTmaxItems GetCmdCleanItems(CTmaxMediaTreeNodes tmaxNodes)
		{
			CMediaNode		mediaNode = null;
			CTmaxItems		tmaxItems = null;
			
			if(tmaxNodes == null) return null;
			if(tmaxNodes.Count == 0) return null;
			
			//	Let the base class process the request if the user has selected media nodes
			if(tmaxNodes[0].IPrimary != null)
				return base.GetCmdCleanItems(tmaxNodes);

			tmaxItems = new CTmaxItems();
			
			//	The user must have selected either media nodes or super nodes
			foreach(CTmaxMediaTreeNode O in tmaxNodes)
			{
				//	Can only clean documents and scripts
				if((O.MediaType != TmaxMediaTypes.Document) &&
				   (O.MediaType != TmaxMediaTypes.Script)) continue;

				//	Get the media node for this type
				if((mediaNode = m_aMediaNodes.Find(O.MediaType)) == null) continue;

				//	Is this the actual media node that is selected?
				if(ReferenceEquals(O, mediaNode.Node) == true)
				{
					//	Add each primary record of this type
					foreach(CDxPrimary dxPrimary in m_tmaxDatabase.Primaries)
					{
						if(dxPrimary.MediaType == O.MediaType)
							GetCmdCleanItems(dxPrimary, tmaxItems);
					}
							
				}
				else
				{
					//	This must be a super node so we add the primaries associated with the super node
					//
					//	NOTE:	We use the Children collection because the nodes may not have
					//			actually been added to the tree yet
					if(O.Children != null)
					{
						foreach(CTmaxMediaTreeNode tmaxPrimary in O.Children)
						{
							if(tmaxPrimary.IPrimary != null)
								GetCmdCleanItems(((CDxMediaRecord)(tmaxPrimary.IPrimary)), tmaxItems);
						}
					
					}// if(O.Children != null)
				
				}// if(ReferenceEquals(O, mediaNode.Node) == true)
				
			}// foreach(CTmaxMediaTreeNode O in tmaxNodes)
			
			//	Do not return an empty collection
			if((tmaxItems != null) && (tmaxItems.Count == 0))
				tmaxItems = null;
				
			return tmaxItems;
			
		}// protected override CTmaxItems GetCmdCleanItems(CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method is called to get the list of items used to fire the Export command</summary>
		/// <param name="tmaxNodes">The collection of selected nodes</param>
		/// <param name="eFormat">The TrialMax export format applied to the operation</param>
		/// <param name="bObjections">true if objections are being exported</param>
		/// <param name="bBreakOnFirst">True to break on first item that satisfies the conditions</param>
		/// <returns>The collection of items to be passed with the command event</returns>
		protected override CTmaxItems GetCmdExportItems(CTmaxMediaTreeNodes tmaxNodes, TmaxExportFormats eFormat, bool bObjections, bool bBreakOnFirst)
		{
			CMediaNode		mediaNode = null;
			CTmaxItems		tmaxItems = null;
			CTmaxItem		tmaxItem = null;
			
			if(tmaxNodes == null) return null;
			if(tmaxNodes.Count == 0) return null;
			
			//	Exporting XML binders not supported by media tree (should be hidden)
			if(eFormat == TmaxExportFormats.XmlBinder) return null;
			
			//	Has the user selected media nodes?
			if(tmaxNodes[0].IPrimary != null)
			{
				//	Let the base class process the request
				return base.GetCmdExportItems(tmaxNodes, eFormat, bObjections, bBreakOnFirst);
			}
			else
			{
				tmaxItems = new CTmaxItems();
				
				//	These must be media nodes or super nodes
				foreach(CTmaxMediaTreeNode O in tmaxNodes)
				{
					//	Should we ignore this node?
					switch(eFormat)
					{
						case TmaxExportFormats.LoadFile:
						
							if(O.MediaType != TmaxMediaTypes.Script &&
							   O.MediaType != TmaxMediaTypes.Document)
							{
								continue;	//	Only allow export scripts and documents to load file
							}
							else
							{
								break;	//	Process this node
							}
							
						case TmaxExportFormats.Video:
						case TmaxExportFormats.AsciiMedia:
						case TmaxExportFormats.XmlScript:
						
							if(O.MediaType == TmaxMediaTypes.Script)
								break;	// Process all scripts
							else if((bObjections == true) && (O.MediaType == TmaxMediaTypes.Deposition))
								break;	//	Depositions are allowed when exporting objections
							else
								continue;
						
						case TmaxExportFormats.Codes:
						case TmaxExportFormats.CodesDatabase:
						
							break;	//	Codes valid for all media types
					}
					
					//	Get the media node for this type
					if((mediaNode = m_aMediaNodes.Find(O.MediaType)) != null)
					{
						//	Is this the actual media node that is selected?
						if(ReferenceEquals(O, mediaNode.Node) == true)
						{
							//	Add an item for each primary record
							if(mediaNode.Primaries != null)
							{
								foreach(CTmaxMediaTreeNode tmaxPrimary in mediaNode.Primaries)
								{
									if((tmaxItem = GetCommandItem(tmaxPrimary, false)) != null)
									{
										tmaxItems.Add(tmaxItem);
									}
								}
								
							}// if(mediaNode.Primaries != null)
						}
						else
						{
							//	This must be a super node so we add the primaries associated with the super node
							//
							//	NOTE:	We use the Children collection because the nodes may not have
							//			actually been added to the tree yet
							if(O.Children != null)
							{
								foreach(CTmaxMediaTreeNode tmaxPrimary in O.Children)
								{
									if((tmaxItem = GetCommandItem(tmaxPrimary, false)) != null)
									{
										tmaxItems.Add(tmaxItem);
									}
								}
									
							}
							
						}// if(ReferenceEquals(O, mediaNode.Node) == true)
						
					}// if((mediaNode = m_aMediaNodes.Find(O.MediaType)) != null)
					
					//	Should we break on the first valid item?
					if((bBreakOnFirst == true) && (tmaxItems.Count > 0))
						break;
					
				}// foreach(CTmaxMediaTreeNode O in tmaxNodes)
				
				return tmaxItems;
			}
			
		}// protected override CTmaxItems GetCmdExportItems(CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>This method is called to get the list of items used to fire the Export command</summary>
		/// <param name="tmaxNodes">The collection of selected nodes</param>
		/// <param name="bBreakOnFirst">True to break on first item that satisfies the conditions</param>
		/// <returns>The collection of items to be passed with the command event</returns>
		protected override CTmaxItems GetCmdBulkUpdateItems(CTmaxMediaTreeNodes tmaxNodes, bool bBreakOnFirst)
		{
			CMediaNode		mediaNode = null;
			CTmaxItems		tmaxItems = null;
			CTmaxItem		tmaxItem = null;
			
			if(tmaxNodes == null) return null;
			if(tmaxNodes.Count == 0) return null;
			
			//	Has the user selected media nodes?
			if(tmaxNodes[0].IPrimary != null)
			{
				//	Let the base class process the request
				return base.GetCmdBulkUpdateItems(tmaxNodes, bBreakOnFirst);
			}
			else
			{
				tmaxItems = new CTmaxItems();
				
				//	These must be media nodes or super nodes
				foreach(CTmaxMediaTreeNode O in tmaxNodes)
				{
					//	Get the media node for this type
					if((mediaNode = m_aMediaNodes.Find(O.MediaType)) != null)
					{
						//	Is this the actual media node that is selected?
						if(ReferenceEquals(O, mediaNode.Node) == true)
						{
							//	Add an item for each primary record
							if(mediaNode.Primaries != null)
							{
								foreach(CTmaxMediaTreeNode tmaxPrimary in mediaNode.Primaries)
								{
									if((tmaxItem = GetCommandItem(tmaxPrimary, false)) != null)
									{
										tmaxItems.Add(tmaxItem);
									}
								}
								
							}// if(mediaNode.Primaries != null)
						}
						else
						{
							//	This must be a super node so we add the primaries associated with the super node
							//
							//	NOTE:	We use the Children collection because the nodes may not have
							//			actually been added to the tree yet
							if(O.Children != null)
							{
								foreach(CTmaxMediaTreeNode tmaxPrimary in O.Children)
								{
									if((tmaxItem = GetCommandItem(tmaxPrimary, false)) != null)
									{
										tmaxItems.Add(tmaxItem);
									}
								}
									
							}
							
						}// if(ReferenceEquals(O, mediaNode.Node) == true)
						
					}// if((mediaNode = m_aMediaNodes.Find(O.MediaType)) != null)
					
					//	Should we break on the first valid item?
					if((bBreakOnFirst == true) && (tmaxItems.Count > 0))
						break;
					
				}// foreach(CTmaxMediaTreeNode O in tmaxNodes)
				
				return tmaxItems;
			}
			
		}// protected override CTmaxItems GetCmdBulkUpdateItems(CTmaxMediaTreeNodes tmaxNodes, bool bBreakOnFirst)
		
		/// <summary>This method is called to populate the child collection of the specified node</summary>
		/// <param name="tmaxNode">The node to be filled</param>
		/// <returns>true if successful</returns>
		protected override bool Fill(CTmaxMediaTreeNode tmaxNode)
		{
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
			
			//	Do we have children waiting to be added to the tree?
			//
			//	NOTE:	This is used on super nodes to speed up loading of 
			//			the database
			if(tmaxNode.Children.Count > 0)
			{
				foreach(CTmaxMediaTreeNode tmaxChild in tmaxNode.Children)
				{
					//	Has this node already been put in the tree once?
					if(tmaxChild.Added == true)
					{
						try
						{
							if(tmaxChild.Control != null)
								tmaxChild.Reposition(tmaxNode.Nodes);
							else
								tmaxNode.Nodes.Add(tmaxChild);
						}
						catch
						{
						}
					}
					else
					{
						tmaxNode.Nodes.Add(tmaxChild);
						tmaxChild.Added = true;	//	Mark as having been added to the tree
						
						//	Make sure the correct properties are in use now that it's been added to the tree
						if((tmaxChild.MediaLevel != TmaxMediaLevels.None) && (tmaxChild.MediaLevel != TmaxMediaLevels.Quaternary))
							tmaxChild.SetProperties(GetImageIndex(tmaxChild));
				
					}
					
				}// foreach(CTmaxMediaTreeNode tmaxChild in tmaxNode.Children)
			
				return true;
			}
			else
			{
				//	Let the base class handle it
				return base.Fill(tmaxNode);
				
			}// if(tmaxNode.Children.Count > 0)
		
		}// protected bool Fill(CTmaxMediaTreeNode tmaxNode)

		/// <summary>This method handles the event fired when the user clicks on Refresh Super Nodes from the context menu</summary>
		/// <param name="tmaxNode">The node under the current mouse position</param>
		protected override void OnCmdRefreshSuperNodes(CTmaxMediaTreeNode tmaxNode)
		{
			CMediaNode mediaNode = null;
			
			//	Reorder the media nodes
			if((m_aMediaNodes != null) && (m_aMediaNodes.Count > 0))
			{
				//	Is there a selection?
				if(tmaxNode != null)
				{
					//	Is the selection a media node?
					if((mediaNode = m_aMediaNodes.Find(tmaxNode)) != null)
					{
						//	Rebuild only the selected media node
						mediaNode.Rebuild(true);
						return;
					}
					
				}
				
				//	Rebuild all the nodes
				m_aMediaNodes.Rebuild(true);
				
			}// if((m_aMediaNodes != null) && (m_aMediaNodes.Count > 0))
				
		}// OnCmdRefreshSuperNodes(CTmaxMediaTreeNode tmaxNode)
		
		/// <summary>
		/// This method is called to determine if the specified command should be visible
		/// </summary>
		/// <param name="eCommand">The tree pane command enumeration</param>
		/// <param name="tmaxNodes">The current node selections</param>
		/// <returns>true if command should be visible</returns>
		protected override bool GetCommandVisible(TreePaneCommands eCommand, CTmaxMediaTreeNodes tmaxNodes)
		{
			switch(eCommand)
			{
				//	These submenus/commands only used by the virtual tree
				case TreePaneCommands.NewMenu:
				case TreePaneCommands.NewBinder:
				case TreePaneCommands.NewDocument:
				case TreePaneCommands.NewRecording:
				case TreePaneCommands.NewScript:
				
				case TreePaneCommands.InsertBeforeMenu:
				case TreePaneCommands.InsertBinderBefore:
				case TreePaneCommands.InsertDocumentBefore:
				case TreePaneCommands.InsertRecordingBefore:
				case TreePaneCommands.InsertScriptBefore:
				
				case TreePaneCommands.InsertAfterMenu:
				case TreePaneCommands.InsertBinderAfter:
				case TreePaneCommands.InsertDocumentAfter:
				case TreePaneCommands.InsertRecordingAfter:
				case TreePaneCommands.InsertScriptAfter:
				
				case TreePaneCommands.ImportAsciiBinders:
				case TreePaneCommands.ImportXmlBinders:
				case TreePaneCommands.ExportXmlBinder:
					
					return false;
				
				//	These selections allow the user to add media to a script
				case TreePaneCommands.ScriptNewMenu:
				case TreePaneCommands.ScriptNewDesignations:
				case TreePaneCommands.ScriptNewClips:
				case TreePaneCommands.ScriptNewBarcodes:
	
				case TreePaneCommands.ScriptBeforeMenu:
				case TreePaneCommands.ScriptBeforeDesignations:
				case TreePaneCommands.ScriptBeforeClips:
				case TreePaneCommands.ScriptBeforeBarcodes:

				case TreePaneCommands.ScriptAfterMenu:
				case TreePaneCommands.ScriptAfterDesignations:
				case TreePaneCommands.ScriptAfterClips:
				case TreePaneCommands.ScriptAfterBarcodes:

					//	Must be only one node selected
					if(tmaxNodes == null) return false;
					if(tmaxNodes.Count != 1) return false;
					
					//	Must be a script selected
					if(tmaxNodes[0].IPrimary == null) return false;
					if(tmaxNodes[0].IPrimary.GetMediaType() != TmaxMediaTypes.Script) return false;
					
					return true;
					
				case TreePaneCommands.New:
				case TreePaneCommands.InsertBefore:
				case TreePaneCommands.InsertAfter:
				
					//	If ScriptsNew popup is visible then New... is not
					return !GetCommandVisible(TreePaneCommands.ScriptNewMenu, tmaxNodes);
					
				case TreePaneCommands.ExpandAll:
				case TreePaneCommands.SetFilter:
				case TreePaneCommands.RefreshFiltered:
				
					return this.ShowFiltered;
					
				default:
			
					return base.GetCommandVisible(eCommand, tmaxNodes);
			}
			
		}// GetCommandVisible(TreePaneCommands eCommand, CTmaxMediaTreeNodes tmaxNodes)
		
		/// <summary>
		/// This method is called to determine if the specified command should be enabled
		/// </summary>
		/// <param name="eCommand">The tree pane command enumeration</param>
		/// <param name="tmaxNode">The current node selections</param>
		/// <returns>true if command should be enabled</returns>
		protected override bool GetCommandEnabled(TreePaneCommands eCommand, CTmaxMediaTreeNodes tmaxNodes)
		{
			switch(eCommand)
			{
				case TreePaneCommands.ImportMenu:

					return (GetCommandEnabled(TreePaneCommands.ImportAsciiScripts, tmaxNodes) ||
							GetCommandEnabled(TreePaneCommands.ImportXmlScripts, tmaxNodes));
							
				case TreePaneCommands.ImportXmlScripts:
				case TreePaneCommands.ImportAsciiScripts:
				
					if(this.ShowFiltered == true) return false;
					if(tmaxNodes == null) return true;
					if(tmaxNodes.Count == 0) return true;
					if(tmaxNodes.Count == 1)
						return (((tmaxNodes[0]).MediaType == TmaxMediaTypes.Script) && ((tmaxNodes[0]).IPrimary == null));
					
					return false;
					
				case TreePaneCommands.RefreshSuperNodes:
				
					return ((m_aMediaNodes != null) && (m_aMediaNodes.Count > 0));
				
				case TreePaneCommands.SetFilter:
				case TreePaneCommands.RefreshFiltered:
				
					//	Only used in the filtered tree
					if(this.ShowFiltered == false) return false;
					
					//	Do we have an active database?
					if(m_tmaxDatabase == null) return false;
					if(m_tmaxDatabase.Primaries == null) return false;
					if(m_tmaxDatabase.Primaries.Count == 0) return false;
					if(m_tmaxTreeCtrl == null) return false;
					
					//	Database must support codes (fielded data)
					if(GetCodesEnabled() == false) return false;
					
					//	It's all good...
					return true;
					
				default:
				
					return base.GetCommandEnabled(eCommand, tmaxNodes);
			}
			
		}// protected override bool GetCommandEnabled(TreePaneCommands eCommand, CTmaxMediaTreeNodes tmaxNodes)
			
		/// <summary>This method is called when the item gets deleted</summary>
		/// <param name="tmaxItem">The item that has been deleted</param>
		protected virtual void OnDeleted(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{
			CTmaxMediaTreeNode	tmaxNode = null;
			CTmaxMediaTreeNodes	tmaxScenes = null;
			
			//	Don't bother if this is not a media node
			if(tmaxItem.MediaType == TmaxMediaTypes.Unknown) return;
			
			//	Is this a link that's being deleted?
			if(tmaxItem.MediaType == TmaxMediaTypes.Link)
			{
				Debug.Assert(tmaxItem.IQuaternary != null);	// Link record
				Debug.Assert(tmaxItem.ITertiary != null);	// Parent designation/clip
				if(tmaxItem.IQuaternary == null) return;
				if(tmaxItem.ITertiary == null) return;
				
				//	Locate any scene that parents the link being deleted
				tmaxScenes = new CTmaxMediaTreeNodes();
				GetScenesFromSource(tmaxScenes, (CDxMediaRecord)tmaxItem.ITertiary);

				foreach(CTmaxMediaTreeNode tmaxScene in tmaxScenes)
				{
					//	Does this link appear in the child collection?
					if((tmaxScene.Nodes != null) && (tmaxScene.Nodes.Count > 0))
					{
						foreach(CTmaxMediaTreeNode O in tmaxScene.Nodes)
						{
							if(ReferenceEquals(O.GetMediaRecord(), tmaxItem.IQuaternary) == true)
							{
								tmaxNode = O;
								break;	
							}
							
						}// foreach(CTmaxMediaTreeNode O in tmaxScene.Nodes)
					
					}// if((tmaxScene.Nodes != null) && (tmaxScene.Nodes.Count > 0))
					
					//	Did we find the node?
					if(tmaxNode != null)
					{
						OnDeleted(tmaxNode);
						Refresh(tmaxScene);
						tmaxNode = null;
					}
					
				}// foreach(CTmaxMediaTreeNode tmaxScene in tmaxScenes)
				
			}
			else
			{
				//	Locate the node associated with this item
				if((tmaxNode = GetNode(tmaxItem, false)) != null)
					OnDeleted(tmaxNode);
			}

		}// private void OnDeleted(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		/// <summary>This method is called when a node has been deleted</summary>
		/// <param name="tmaxItem">The node that has been deleted</param>
		protected virtual void OnDeleted(CTmaxMediaTreeNode tmaxNode)
		{
			CTmaxMediaTreeNode	tmaxParent = null;
			CTmaxMediaTreeNode	tmaxSelect = null;
			CMediaNode		mediaNode  = null;
			
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
				
			//	Are we deleting a primary media node?
			if(tmaxNode.MediaLevel == TmaxMediaLevels.Primary)
			{
				//	Get the media node that owns this primary
				if((mediaNode = m_aMediaNodes.Find(tmaxNode.MediaType)) != null)
				{
					mediaNode.Remove(tmaxNode, false);
				}
			}
			else
			{
				//	Not supposed to be able to delete media nodes or
				//	super nodes
				Debug.Assert(tmaxNode.MediaLevel != TmaxMediaLevels.None);
				if(tmaxNode.MediaLevel == TmaxMediaLevels.None) return;
				
				//	Get the parent node
				tmaxParent = (CTmaxMediaTreeNode)tmaxNode.Parent;
				Debug.Assert(tmaxParent != null);
				Debug.Assert(tmaxParent.Nodes != null);
				
				//	Is this node currently selected?
				if(tmaxNode.Selected == true)
				{
					//	Set up the replacement
					tmaxNode.Selected = false;
				}
					
				//	Remove this node from the tree
				tmaxParent.Nodes.Remove(tmaxNode);
					
				//	Refresh the parent
				Refresh(tmaxParent);
				
				//	Refresh all scenes because the user may have just
				//	modified a record that is a source for a scene
				if((tmaxParent.MediaType != TmaxMediaTypes.Script) && 
					(tmaxParent.MediaType != TmaxMediaTypes.Link))
					RefreshScenes();		
			}
			
			//	Should we select a new node
			if(tmaxSelect != null)
			{
				m_tmaxTreeCtrl.SetSelection(tmaxSelect);
			}

		}// private void OnDeleted(CTmaxMediaTreeNode tmaxNode)
			
		///	<summary>This method will retrieve the collection of scenes that reference the specified source record</summary>
		/// <param name="tmaxSceneNodes">The collection in which to place the scene nodes</param>
		/// <param name="dxSource">The scene's source record interface</param>
		/// <returns>The total number of scenes placed in the collection</returns>
		protected virtual long GetScenesFromSource(CTmaxMediaTreeNodes tmaxSceneNodes, CDxMediaRecord dxSource)
		{
			CMediaNode		tmaxMedia = null;
			CDxSecondary	dxScene = null;
			
			//	Get the media node that contains the scripts
			if(m_aMediaNodes != null)
				tmaxMedia = m_aMediaNodes.Find(TmaxMediaTypes.Script);
			if(tmaxMedia == null) return 0;
			
			//	Iterate the collection of scripts
			if((tmaxMedia.Primaries != null) && (tmaxMedia.Primaries.Count > 0))
			{
				foreach(CTmaxMediaTreeNode Script in tmaxMedia.Primaries)
				{
					//	Has this node been added to the tree yet?
					if((Script.Added == true) && (Script.Nodes != null))
					{
						//	Check each of the scenes
						foreach(CTmaxMediaTreeNode Scene in Script.Nodes)
						{
							//	Get the record associated with this scene
							if((dxScene = (CDxSecondary)Scene.ISecondary) != null)
							{
								//	Does this scene reference the specified record
								if(ReferenceEquals(dxScene.GetSource(), dxSource) == true)
								{
									//	Add to the caller's collection
									tmaxSceneNodes.Add(Scene);
								}
							}
							
						}// foreach(CTmaxMediaTreeNode Scene in Script.Nodes)
					
					}// if((Script.Added == true) && (Script.Nodes != null))
				
				}// foreach(CTmaxMediaTreeNode Script in tmaxMedia.Primaries)
							
			}// if((tmaxMedia.Primaries != null) && (tmaxMedia.Primaries.Count > 0))	
			
			return tmaxSceneNodes.Count;
			
		}// private long GetScenesFromSource(CTmaxMediaTreeNodes tmaxSceneNodes, CDxMediaRecord dxSource)
			
		/// <summary>This function is called to sort the primary collection to match the tree</summary>
		/// <returns>true if successful</returns>
		protected virtual bool SortPrimaries()
		{
			ArrayList	dxHolding = null;
			CDxPrimaries	dxPrimaries = null;
			CTmaxParameters	tmaxParameters = null;

			if(m_tmaxTreeCtrl == null) return false;
			if(m_tmaxTreeCtrl.Nodes == null) return false;

			//	Are we using a filtered collection?
			if(this.ShowFiltered == true)
			{
				dxPrimaries = m_dxFiltered;
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.Filtered, true);
			}
			else
			{
				dxPrimaries = m_tmaxDatabase != null ? m_tmaxDatabase.Primaries : null;
			}
				
			if(dxPrimaries == null) return false;
			if(dxPrimaries.Count < 2) return true; // nothing to sort

			//	Transfer all the primaries to a temporary holding collection
			dxHolding = new ArrayList(dxPrimaries.Count);
			foreach(CDxPrimary O in dxPrimaries)
			{
				dxHolding.Add(O);
				O.Holding = true;
			}
			dxPrimaries.Clear();
			
			//	Now rebuild the filtered collection using the node order
			foreach(CMediaNode mediaNode in m_aMediaNodes)
			{
				if(mediaNode.Primaries != null)
				{
					foreach(CTmaxMediaTreeNode O in mediaNode.Primaries)
					{
						if(O.GetMediaRecord() != null)
						{
							//	We have to do this test because we may be in
							//	the middle of a composite database operation and
							//	this record may have actually been deleted but
							//	the pane hasn't been notified yet
							if(((CDxMediaRecord)(O.GetMediaRecord())).Holding == true)
							{
								dxPrimaries.AddList((CBaseRecord)(O.GetMediaRecord()));
								((CDxMediaRecord)(O.GetMediaRecord())).Holding = false;
							}
						}
					
					}// foreach(CTmaxMediaTreeNode O in mediaNode.Primaries)
					
				}// if(mediaNode.Primaries != null)
				
			}// foreach(CMediaNode mediaNode in m_aMediaNodes)
			
			//	If all were properly transferred, the counts should be the same
			if(dxHolding.Count != dxPrimaries.Count)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SortPrimaries", "Count mismatch: Holding = " + dxHolding.Count.ToString() + " Primaries = " + dxPrimaries.Count.ToString());
				
				if(dxHolding.Count > dxPrimaries.Count)
				{
					foreach(CDxPrimary O in dxHolding)
					{
						if(O.Holding == true)
						{
							dxPrimaries.AddList(O);
							O.Holding = false;
						}
						
					}
					
				}

			}// if(dxHolding.Count != dxPrimaries.Count)
			
			dxHolding.Clear();

			//	Notify the other panes
			FireCommand(TmaxCommands.SetPrimariesOrder, (CTmaxItem)null, tmaxParameters);
		
			return true;
			
		}// protected virtual SortPrimaries()
		
		///	<summary>This method will refresh all nodes associated with script scenes</summary>
		protected virtual void RefreshScenes()
		{
			CMediaNode tmaxMedia = null;
			
			//	Get the media node that contains the scripts
			if(m_aMediaNodes != null)
				tmaxMedia = m_aMediaNodes.Find(TmaxMediaTypes.Script);
			if(tmaxMedia == null) return;
			
			//	Iterate the collection of scripts
			if((tmaxMedia.Primaries != null) && (tmaxMedia.Primaries.Count > 0))
			{
				foreach(CTmaxMediaTreeNode Script in tmaxMedia.Primaries)
				{
					//	Has this node been added to the tree yet?
					if((Script.Added == true) && (Script.Nodes != null))
					{
						//	Refresh each of the scenes
						foreach(CTmaxMediaTreeNode Scene in Script.Nodes)
						{
							Refresh(Scene);
						}
					}
				}
							
			}	
			
		}// protected virtual void RefreshScenes()
			
		/// <summary>This method is called when the PauseThreshold value changes</summary>
		protected override void OnPauseThresholdChanged()
		{
			CMediaNode		tmaxMedia = null;
			int				iImage = 0;
			CDxSecondary	dxScene = null;
			
			//	Get the media node that contains the scripts
			if(m_aMediaNodes != null)
				tmaxMedia = m_aMediaNodes.Find(TmaxMediaTypes.Script);
			if(tmaxMedia == null) return;
			
			//	Do we have any scripts in the tree
			if(tmaxMedia.Primaries == null) return;
			if(tmaxMedia.Primaries.Count == 0) return;
			
			//	Check each script in the collection
			foreach(CTmaxMediaTreeNode Script in tmaxMedia.Primaries)
			{
				//	Make sure this script has been added to the tree
				if(Script.Added == false) continue;
				if(Script.Nodes == null) continue;
					
				//	Refresh each of the designations
				foreach(CTmaxMediaTreeNode O in Script.Nodes)
				{
					//	Is this a designation?
					if((dxScene = (CDxSecondary)(O.GetMediaRecord())) != null)
					{
						if((dxScene.GetSource() != null) && (dxScene.GetSource().MediaType == TmaxMediaTypes.Designation))
						{
							iImage = GetImageIndex(O);
							if(iImage != (int)(O.Override.NodeAppearance.Image))
								O.Override.NodeAppearance.Image = iImage;
						}
						
					}// if((dxScene = (CDxSecondary)(O.GetMediaRecord())) != null)
			
				}// foreach(CTmaxMediaTreeNode O in Script.Nodes)
				
			}// foreach(CTmaxMediaTreeNode Script in tmaxMedia.Primaries)
			
		}// protected override void OnPauseThresholdChanged()
			
		/// <summary>This method will locate the child node associated with the specified record</summary>
		/// <param name="tmaxParent">The parent node who's child collection is to be searched</param>
		/// <param name="IChild">The child node's record interface</param>
		/// <returns>The child node if found</returns>
		protected virtual CTmaxMediaTreeNode Find(CTmaxMediaTreeNode tmaxParent, ITmaxMediaRecord IChild)
		{	
			ITmaxMediaRecord tmaxRecord;
			
			if((tmaxParent == null) || (tmaxParent.Nodes == null) || (tmaxParent.Nodes.Count == 0)) return null;
			
			foreach(CTmaxMediaTreeNode tmaxChild in tmaxParent.Nodes)
			{
				if((tmaxRecord = tmaxChild.GetTmaxRecord(true)) != null)
				{
					if(IChild.GetAutoId() == tmaxRecord.GetAutoId())
						return tmaxChild;
				}
			}
			
			return null;
			
		}// private CTmaxMediaTreeNode Find(CTmaxMediaTreeNode tmaxParent, ITmaxMediaRecord IChild)
		
		/// <summary>This method is called to retrieve the node associated with the specified item</summary>
		/// <param name="tmaxItem">TrialMax event item that identifies the node</param>
		/// <param name="bFill">True to fill child collections if required</param>
		/// <returns>The associated node</returns>
		protected virtual CTmaxMediaTreeNode GetNode(CTmaxItem tmaxItem, bool bFill)
		{
			CMediaNode		mediaNode = null;
			CTmaxMediaTreeNode	tmaxPrimary = null;
			CTmaxMediaTreeNode	tmaxSecondary = null;
			CTmaxMediaTreeNode	tmaxTertiary = null;
			CTmaxMediaTreeNode	tmaxSuper = null;
			
			//	Must at least have a valid primary interface
			if(tmaxItem.IPrimary == null) return null;
						
			//	Get the media node associated with this item
			if((mediaNode = m_aMediaNodes.Find(tmaxItem.IPrimary.GetMediaType())) == null) return null;
			
			//	Get the primary node
			if((tmaxPrimary = mediaNode.Primaries.Find(tmaxItem.IPrimary.GetAutoId())) == null) return null;
			
			//	Is this primary node owned by a super node?
			if((tmaxSuper = mediaNode.GetSuperNode(tmaxPrimary)) != null)
			{
				//	Should we populate the super node child collection?
				//
				//	NOTE:	If the user has not yet expanded the super node
				//			then the primary does not actually appear in the tree
				if((tmaxSuper.Nodes != null) && (tmaxSuper.Nodes.Count == 0))
				{
					if((bFill == true) && (tmaxSuper.Children.Count > 0))
					{
						try { Fill(tmaxSuper); }
						catch {};
					}
					
				}
				
			}// if((tmaxSuper = mediaNode.GetSuperNode(tmaxPrimary)) != null)
			
			//	Is the caller looking for the primary node?
			if(tmaxItem.ISecondary == null) return tmaxPrimary;
			
			//	Do we need to populate the primary node's child collection?
			if((tmaxPrimary.Nodes == null) || (tmaxPrimary.Nodes.Count == 0))
			{
				if(bFill == true)
					Fill(tmaxPrimary);
				else
					return null;
			}
				
			//	Get the secondary node
			if((tmaxSecondary = Find(tmaxPrimary, tmaxItem.ISecondary)) == null) return null;
			
			//	Is the caller looking for the secondary node?
			if(tmaxItem.ITertiary == null) return tmaxSecondary;
			
			//	Do we need to populate the secondary node's child collection?
			if((tmaxSecondary.Nodes == null) || (tmaxSecondary.Nodes.Count == 0))
			{
				if(bFill == true)
					Fill(tmaxSecondary);
				else
					return null;
			}
		
			//	Get the tertiary node
			if((tmaxTertiary = Find(tmaxSecondary, tmaxItem.ITertiary)) == null) return null;
		
			//	Is the caller looking for the tertiary node?
			if(tmaxItem.IQuaternary == null) return tmaxTertiary;
		
			//	Do we need to populate the tertiary node's child collection?
			if((tmaxTertiary.Nodes == null) || (tmaxTertiary.Nodes.Count == 0))
			{
				if(bFill == true)
					Fill(tmaxTertiary);
				else
					return null;
			}

			return Find(tmaxTertiary, tmaxItem.IQuaternary);
		
		}// protected virtual CTmaxMediaTreeNode GetNode(CTmaxItem tmaxItem, bool bFill)
		
		/// <summary>This method is called to add the newly registered records</summary>
		/// <param name="tmaxFolder">The source folder containing the new media</param>
		protected virtual void AddRegistered(CTmaxSourceFolder tmaxSource)
		{
			CTmaxMediaTreeNode	tmaxNode = null;
			CMediaNode			mediaNode = null;
			bool				bReorder = false;
			
			//	Should we update the super nodes?
			if((m_tmaxRegOptions != null) && (m_tmaxRegOptions.GetFlag(RegFlags.UpdateSuperNodes) == true))
			{
				if(m_iSuperNodeSize > 0)
					bReorder = true;
			}						
			
			//	Is this a registered primary media object?
			if(tmaxSource.IPrimary != null)
			{
				//	Get the root media node
				if((mediaNode = m_aMediaNodes.Find(tmaxSource.IPrimary.GetMediaType())) != null)
				{
					//	Create a new node
					if((tmaxNode = CreateNode(tmaxSource)) != null)
					{
						try
						{
							//	Add to the root collection
							if(mediaNode.Add(tmaxNode, bReorder) == true)
							{							
								//	Now add each file in this folder
								foreach(CTmaxSourceFile tmaxFile in tmaxSource.Files)
								{
									Add(tmaxNode, tmaxFile);
								}
								
							}
							
						}
						catch(System.Exception Ex)
						{
							m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_PHYSICAL_ADD_SOURCE_FOLDER_EX, tmaxSource.Path), Ex);
						}
					
					}// if((tmaxNode = CreateNode(tmaxSource)) != null)
				
				}// if((tmaxRoot = GetRootNode(tmaxSource.IPrimary.GetMediaType())) != null)
			
			}// if(tmaxSource.IPrimary != null)
			
			//	Now add each of this folder's subfolders
			if((tmaxSource.SubFolders != null) && (tmaxSource.SubFolders.Count > 0))
			{
				foreach(CTmaxSourceFolder tmaxFolder in tmaxSource.SubFolders)
				{
					AddRegistered(tmaxFolder);
				}
			
			}// if((tmaxSource.SubFolders != null) && (tmaxSource.SubFolders.Count > 0))
		
		}// protected virtual void AddRegistered(CTmaxSourceFolder tmaxSource)
		
		/// <summary>This method will add new links to the tree</summary>
		/// <param name="dxParent">The tertiary parent of the links</param>
		/// <param name="tmaxLinks">The collection of event items that represent the new links</param>
		protected virtual void AddLinks(CDxTertiary dxParent, CTmaxItems tmaxLinks)
		{
			CTmaxMediaTreeNodes	tmaxScenes = new CTmaxMediaTreeNodes();
			
			//	Get all scenes that reference this parent
			if(GetScenesFromSource(tmaxScenes, dxParent) == 0) return;
			
			//	Add the links to each scene in the collection
			foreach(CTmaxMediaTreeNode tmaxScene in tmaxScenes)
			{
				if(tmaxScene.Nodes != null)
				{
					//	Should we add the links now?
					if((tmaxScene.Nodes.Count > 0) || (tmaxScene.Expanded == true))
					{
						AddLinks(tmaxScene, tmaxLinks);
						Refresh(tmaxScene);
					}
					else
					{
						//	Make sure the user can expand the node
						if(tmaxScene.HasExpansionIndicator == false)
						{
							tmaxScene.Override.ShowExpansionIndicator = ShowExpansionIndicator.Always;
						}
					}
					
				}// if(tmaxScene.Nodes != null)
			
			}// foreach(CTmaxMediaTreeNode tmaxScene in tmaxScenes)
			
		}// protected virtual void AddLinks(CDxTertiary dxParent, CTmaxItems tmaxLinks)
		
		/// <summary>This method will add new links to the specified scene</summary>
		/// <param name="dxParent">The scene whose source record is the parent of the links</param>
		/// <param name="tmaxLinks">The collection of event items that represent the new links</param>
		protected virtual void AddLinks(CTmaxMediaTreeNode tmaxScene, CTmaxItems tmaxLinks)
		{
			//	Add the new link to the scene
			foreach(CTmaxItem tmaxLink in tmaxLinks)
			{
				if(tmaxLink.GetMediaRecord() != null)
				{
					if(Find(tmaxScene, tmaxLink.GetMediaRecord()) == null)
						Add(tmaxScene, tmaxLink.GetMediaRecord());
				}
			}
		
		}// protected virtual void AddLinks(CTmaxMediaTreeNode tmaxScene, CTmaxItems tmaxLinks)
		
		/// <summary>This method is called to add root nodes for each primary media type</summary>
		/// <param name="tmaxMediaTypes">The collection of media types</param>
		protected virtual void Add(CTmaxMediaTypes tmaxMediaTypes)
		{
			CTmaxMediaTreeNode tmaxNode = null;
			CMediaNode mediaNode = null;
			
			//	Make sure we have the required objects
			if(tmaxMediaTypes == null) return;
			if(m_tmaxTreeCtrl == null) return;
			if(tmaxMediaTypes.Count == 0) return;
			
			//	Add a node for each type
			foreach(CTmaxMediaType tmaxMediaType in tmaxMediaTypes)
			{
				if((tmaxNode = CreateNode(tmaxMediaType)) != null)
				{
					//	Add to the root of the tree
					m_tmaxTreeCtrl.Nodes.Add(tmaxNode as UltraTreeNode);
					
					//	Set the property values AFTER adding to the tree
					tmaxNode.SetProperties(GetImageIndex(tmaxNode));
					tmaxNode.Override.ShowExpansionIndicator = Infragistics.Win.UltraWinTree.ShowExpansionIndicator.CheckOnDisplay;

					//	Add a media node to our local collection
					mediaNode = new CMediaNode(this, tmaxMediaType.PrimaryType);
					m_tmaxEventSource.Attach(mediaNode.EventSource);
					mediaNode.Node = tmaxNode;
					mediaNode.SuperNodeSize = m_iSuperNodeSize;
					mediaNode.Primaries.Sorter = m_tmaxPrimarySorter;
					m_aMediaNodes.Add(mediaNode);
					
				}
			
			}// foreach(CTmaxMediaType tmaxMediaType in tmaxMediaTypes)

		}// protected virtual void Add(CTmaxMediaTypes tmaxMediaTypes)
				
		/// <summary>This method is called to create a new node for the specified media type</summary>
		/// <param name="tmaxMediaType">The primary media type to be displayed</param>
		/// <returns>The new node if successful</returns>
		protected virtual CTmaxMediaTreeNode CreateNode(CTmaxMediaType tmaxMediaType)
		{
			CTmaxMediaTreeNode tmaxNode = null;
			
			try
			{
				tmaxNode = new CTmaxMediaTreeNode(tmaxMediaType.Name);
				
				tmaxNode.MediaType = tmaxMediaType.PrimaryType;

				if(m_bCheckBoxes == true)
					tmaxNode.Override.NodeStyle = Infragistics.Win.UltraWinTree.NodeStyle.CheckBox;
						
				return tmaxNode;
			}
			catch
			{
				return tmaxNode;
			}
			
		}// protected virtual CTmaxMediaTreeNode CreateNode(CTmaxMediaType tmaxMediaType)
		
		/// <summary>This method is called to add a primary exchange record to the specified parent node</summary>
		/// <param name="mediaNode">The media node that owns the primary</param>
		/// <param name="dxPrimary">The primary exchange record</param>
		/// <param name="bReorder">true to reorder primary nodes inside the super nodes</param>
		/// <returns>The newly added node</returns>
		protected virtual CTmaxMediaTreeNode Add(CMediaNode mediaNode, CDxPrimary dxPrimary, bool bReorder)
		{
			CTmaxMediaTreeNode	tmaxNode = null;
			
			//	Make sure we have the required objects
			Debug.Assert(mediaNode != null);
			Debug.Assert(mediaNode.Node != null);
			Debug.Assert(dxPrimary != null);
			if(mediaNode == null) return null;
			if(mediaNode.Node == null) return null;
			if(dxPrimary == null) return null;

			//	Create a new node
			if((tmaxNode = CreateNode(dxPrimary)) != null)
			{
				try
				{
					//	Insert into the media node's collection
					if(mediaNode.Add(tmaxNode, bReorder) == true)
					{			
						//	Now add each secondary record
						foreach(CDxSecondary dxSecondary in dxPrimary.Secondaries)
						{
							Add(tmaxNode, dxSecondary);
						}
					
						return tmaxNode;
						
					}
					
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_PHYSICAL_ADD_PRIMARY_EX, dxPrimary.AutoId), Ex);
				}
				
			}// if((tmaxNode = CreateNode(tmaxSource)) != null)
			
			//	An error must have occurred
			return null;	
		
		}// protected virtual CTmaxMediaTreeNode Add(CMediaNode mediaNode, CDxPrimary dxPrimary, bool bReorder)
		
		/// <summary>This function is called to populate the tree</summary>
		/// <param name="dxPrimaries">The collection of primary records used to populate the tree</param>
		/// <returns>true if successful</returns>
		protected virtual bool Populate(CDxPrimaries dxPrimaries)
		{
			CMediaNode			tmaxMedia = null;
			CTmaxMediaTreeNode	tmaxPrimary = null;

			if(m_tmaxTreeCtrl == null) return false;
			if(m_tmaxTreeCtrl.Nodes == null) return false;
			
			//	Clear the existing tree
			Clear();
			
			//	We have to have the media types collection
			if(m_tmaxMediaTypes == null) return false;

			//	Prevent processing of Paint events until we are done
			m_tmaxTreeCtrl.BeginUpdate();
		
			//	Display the wait cursor
			Cursor.Current = Cursors.WaitCursor;
			
			//	Add nodes for each primary media type to the root
			Add(m_tmaxMediaTypes);

			//	Prepare the nodes for loading
			m_aMediaNodes.PreLoad();
					
			//	Do we have any records to add
			if((dxPrimaries != null) && (dxPrimaries.Count > 0))
			{
				//	Create the nodes to be added to tree
				foreach(CDxPrimary dxPrimary in dxPrimaries)
				{
					//	Do we need to get a different media node?
					if((tmaxMedia == null) || (tmaxMedia.MediaType != dxPrimary.MediaType))
						tmaxMedia = m_aMediaNodes.Find(dxPrimary.MediaType);
						
					if((tmaxMedia != null) && (tmaxMedia.Primaries != null))
					{
						if((tmaxPrimary = CreateNode(dxPrimary)) != null)
						{
							tmaxMedia.Primaries.Add(tmaxPrimary);
							
							//	NOTE: We do not set the properties until after the
							//		  node is actually added to the tree. This is
							//		  done in the call to Load()
						}
						
					}
				
				}// foreach(CDxPrimary dxPrimary in m_tmaxDatabase.Primaries)
				
			}// if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null))
				
			//	Now load the records
			m_aMediaNodes.Load();
			
			//	Now make sure the collection is sorted
			SortPrimaries();
			//#warning sort primaries disabled
			
			//	Reenable Paint events
			m_tmaxTreeCtrl.EndUpdate();
			
			//	Restore the cursor
			Cursor.Current = Cursors.Default;
			
			return true;
			
		}// protected virtual bool Populate(CDxPrimaries dxPrimaries)
		
		/// <summary>This function is called to clear the tree</summary>
		protected virtual void Clear()
		{
			try
			{
				if((m_tmaxTreeCtrl != null) && (m_tmaxTreeCtrl.Nodes != null))
					m_tmaxTreeCtrl.Nodes.Clear();
					
				if(m_aMediaNodes != null)
					m_aMediaNodes.Clear();
			
			}
			catch
			{
			}
			
		}// private void Clear()
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>True if this control displays a filtered record set</summary>
		public bool ShowFiltered
		{
			get { return m_bShowFiltered; }
		}
		
		#endregion Properties
		
	}// public class CMediaTree : FTI.Trialmax.Panes.CTreePane

}// namespace FTI.Trialmax.Panes
