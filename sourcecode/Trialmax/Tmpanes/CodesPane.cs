using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;
using FTI.Trialmax.Controls;
using FTI.Trialmax.Database;
using FTI.Trialmax.Forms;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;

namespace FTI.Trialmax.Panes
{
	/// <summary>This pane displays the codes assigned to a media record</summary>
	public class CCodesPane : FTI.Trialmax.Panes.CBasePane
	{
		#region Constants
		
		/// <summary>Local context menu command identifiers</summary>
		private enum CodesPaneCommands
		{
			Invalid = 0,
			ShowUnassigned,
			Add,
			AddAnother,
			Delete,
			FilterSelected,
			FilterExclude,
			FilterText,
			HideCaseCodes,
			First,
			Previous,
			Next,
			Last,
			GoTo,
			ToggleUseFiltered,
			SetFilter,
			RefreshFiltered,
		}
		
		private string XML_KEY_SHOW_UNASSIGNED = "ShowUnassigned";
		private string XML_KEY_DROP_LIST_BOOLEAN = "DropListBoolean";
		
		private const int ERROR_ON_CMD_DELETE_EX		= (ERROR_BASE_PANE_MAX + 1);
		private const int ERROR_ON_CMD_ADD_EX			= (ERROR_BASE_PANE_MAX + 2);
		private const int ERROR_ON_CMD_SHOW_ASSIGNED_EX	= (ERROR_BASE_PANE_MAX + 3);
		private const int ERROR_FILL_EX					= (ERROR_BASE_PANE_MAX + 4);
		private const int ERROR_FIRE_SET_CODES_EX		= (ERROR_BASE_PANE_MAX + 5);
		private const int ERROR_ON_BEFORE_UPDATE_EX		= (ERROR_BASE_PANE_MAX + 6);
		private const int ERROR_ADD_PICK_VALUE_EX		= (ERROR_BASE_PANE_MAX + 7);
		private const int ERROR_ON_CMD_FILTER_EX		= (ERROR_BASE_PANE_MAX + 8);
		private const int ERROR_ON_CMD_ADD_ANOTHER_EX	= (ERROR_BASE_PANE_MAX + 9);
		private const int ERROR_ON_CMD_NAVIGATE_EX		= (ERROR_BASE_PANE_MAX + 10);
		
		#endregion Constants
		
		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>Image list bound to the pane's toolbar</summary>
		private System.Windows.Forms.ImageList m_ctrlToolbarImages;
		
		/// <summary>Infragistics toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager m_ultraToolbarManager;
		
		/// <summary>Infragistics toolbar manager left docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CCodesPane_Toolbars_Dock_Area_Left;
		
		/// <summary>Infragistics toolbar manager right docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CCodesPane_Toolbars_Dock_Area_Right;
		
		/// <summary>Infragistics toolbar manager top docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CCodesPane_Toolbars_Dock_Area_Top;
		
		/// <summary>Infragistics toolbar manager bottom docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CCodesPane_Toolbars_Dock_Area_Bottom;
		
		/// <summary>Local member to reference the codes belonging to the active owner</summary>
		private CDxCodes m_dxCodes = null;
		
		/// <summary>Local member to store the collection of codes displayed in the grid</summary>
		private CDxCodes m_dxGridCodes = new CDxCodes();
		
		/// <summary>Interface to the record that owns the active codes collection</summary>
		private CDxMediaRecord m_dxOwner = null;
		
		/// <summary>Property grid control used to display and edit codes</summary>
		private FTI.Trialmax.Controls.CTmaxPropGridCtrl m_ctrlPropGrid;
		
		/// <summary>Interface to the record that is to be activated when the pane gets activated</summary>
		private CDxMediaRecord m_dxActivate = null;
		
		/// <summary>Local member to store a flag to indicate if unassigned codes should be shown</summary>
		private bool m_bShowUnassigned = true;
		
		/// <summary>Local member to store a flag to indicate if navigation is being done against the filtered tree</summary>
		private bool m_bUseFiltered = false;
		
		/// <summary>Local member to store a flag to inhibit handling of update notifications from the application</summary>
		private bool m_bIgnoreUpdates = false;
		
		/// <summary>Local member to store a flag to enable drop-down list boolean editing</summary>
		private bool m_bDropListBoolean = true;
		
		/// <summary>Local member to store a flag to request refilling of the property grid</summary>
		private bool m_bRefill = false;
		
		/// <summary>Local member to to keep track of all text in the property being edited</summary>
		private string m_strEditorText = "";
		
		/// <summary>Local member to to keep track of the code being edited</summary>
		private CDxCode m_dxEditorCode = null;
		
		/// <summary>Local member to to keep track of text selected in the property being edited</summary>
		private string m_strSelectedText = "";
		
		/// <summary>Local member to to keep track of start index of the selected text</summary>
		private long m_lSelectedIndex = 0;
		
		/// <summary>Local member to to keep track of the index of the current record in the navigation collection</summary>
		private int m_iNavigatorIndex = -1;
		
		/// <summary>Local member to to keep track of total records in the navigation collection</summary>
		private int m_iNavigatorTotal = -1;
		
		/// <summary>Local member to store AfterUpdate arguments</summary>
		private FTI.Trialmax.Controls.CTmaxPropGridCtrl.CTmaxPropGridArgs m_tmaxAfterUpdateArgs = null;
		private Infragistics.Win.UltraWinStatusBar.UltraStatusBar m_ctrlStatusBar;
		
		/// <summary>Delegate used to break event chain for AfterUpdate events</summary>
		private delegate void AfterUpdateDelegate();
		private AfterUpdateDelegate m_delAfterUpdate = null;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CCodesPane() : base()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			//m_delAfterUpdate = new AfterUpdateDelegate(this.OnAfterUpdate);

			//	We are going to want to keep the codes in sorted order
			if(m_dxGridCodes.Comparer == null)
			{
				m_dxGridCodes.Comparer = new CTmaxSorter(CTmaxCaseCodes.SORT_ON_ORDER);
				m_dxGridCodes.KeepSorted = false;
			}
			
		}// public CCodesPane()
		
		/// <summary>This method is called by the application to activate the specified item</summary>
		/// <param name="tmaxItem">The item to be activated</param>
		/// <param name="ePane">The pane requesting activation</param>
		/// <returns>true if successful</returns>
		public override bool Activate(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
			//	Don't bother if codes are not enabled
			if(GetCodesEnabled() == false) return false;
			
			//	Get the record to be activated
			//
			//	NOTE:	We only allow coding of primary records right now
			m_dxActivate = (CDxMediaRecord)(tmaxItem.IPrimary);
			//m_dxActivate = (CDxMediaRecord)(tmaxItem.GetMediaRecord());

			if((m_bPaneVisible == true) && (ReferenceEquals(m_dxOwner, m_dxActivate) == false))
			{
				SetOwner(m_dxActivate);
			}

			return true;
						
		}// public override bool Activate(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		
		/// <summary>This method is called by the application to open the specified item</summary>
		/// <param name="tmaxItem">The item to be opened</param>
		///	<param name="ePane">The pane making the request</param>
		/// <returns>true if successful</returns>
		public override bool Open(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
			return Activate(tmaxItem, ePane);
						
		}// public override bool Open(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		/// <summary>This method is called by the application to when the user updates multiple records</summary>
		/// <param name="tmaxItem">The items that have been updated</param>
		public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			//	Does this collection contain a reference to the owner record?
			if(ContainsOwner(tmaxItems, true) == true)
			{
				//	Refresh the codes
				this.OnUpdated();
			}

		}

		/// <summary>This method is called by the application to when the item has been updated by the user</summary>
		/// <param name="tmaxItem">The item that has been updated</param>
		public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{
			//	Is this a case code notification?
			if(tmaxItem.DataType == TmaxDataTypes.CaseCode)
			{
				if(tmaxItem.CaseCode != null)
					OnUpdated(tmaxItem.CaseCode);				
			}
			
			//	Is it a pick list?
			else if(tmaxItem.DataType == TmaxDataTypes.PickItem)
			{
				if(tmaxItem.PickItem != null)
					OnUpdated(tmaxItem.PickItem);				
			}
			else
			{
				//	Does this item reference the existing owner?
				if(IsOwner(tmaxItem, true) == true)
				{
					//	Refresh the codes
					this.OnUpdated();
				}
			
			}

		}// public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItem tmaxItem)

		/// <summary>This method is called by the application when it adds new media to the database</summary>
		/// <param name="tmaxParent">TrialMax event item that identifies the parent record</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		{
			Debug.Assert(tmaxParent != null);
			Debug.Assert(tmaxChildren != null);
			Debug.Assert(tmaxChildren.Count > 0);
			if(tmaxChildren.Count == 0) return;

			//	What type of records are being added?
			switch(tmaxParent.DataType)
			{
				case TmaxDataTypes.CaseCode:
				case TmaxDataTypes.PickItem:
				
					m_bRefill = true;
					break;
					
				default:
				
					break;
					
			}// switch(tmaxParent.DataType)
			
		}// public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		
		/// <summary>This method is called by the application when the records in the filtered collection have been reordered</summary>
		/// <param name="bFiltered">True if the filtered collection has been reordered</param>
		public override void OnSetPrimariesOrder(bool bFiltered)
		{
			//	Is the pane visible
			if(this.PaneVisible == true)
			{
				if(bFiltered == true)
				{
					if(m_bUseFiltered == true)
						SetToolStates(false);
				}
				else
				{
					if(m_bUseFiltered == false)
						SetToolStates(false);
				}
				
			}// if(this.Active == true)
			
		}// public virtual void OnSetPrimariesOrder(bool bFiltered)
		
		/// <summary>This method is called by the application to when the item gets deleted</summary>
		/// <param name="tmaxItem">The item that has been deleted</param>
		public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			//	Has the item to be activated been deleted?
			if((m_dxActivate != null) && (m_tmaxDatabase.IsValidRecord(m_dxActivate) == false))
				m_dxActivate = null;
			
			//	Nothing else to do if not displaying anything
			if(m_dxOwner == null) return;
			
			//	Has the active record been deleted?
			if(m_tmaxDatabase.IsValidRecord(m_dxOwner) == false)
			{
				SetOwner(null);
			}
			
			//	The collection specified by the caller is a collection of parent records
			foreach(CTmaxItem tmaxParent in tmaxItems)
			{
				//	What type of records are being added?
				switch(tmaxParent.DataType)
				{
					case TmaxDataTypes.CaseCode:
					case TmaxDataTypes.PickItem:
				
						m_bRefill = true;
						break;
					
					default:
				
						break;
					
				}// switch(tmaxParent.DataType)
				
				if(m_bRefill == true)
					break;
				
			}// foreach(CTmaxItem tmaxParent in tmaxItems)

			SetToolStates(false);
			
		}// public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		
		/// <summary>This method is called by the application when a bulk update operation has been performed</summary>
		/// <param name="tmaxItems">The primary owners of the codes modified by the operation</param>
		public override void OnBulkUpdate(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			//	This will force a refresh of the active record
			OnCaseCodesModified();
		}
		
		/// <summary>This method is called by the application when the codes assigned to a record have changed</summary>
		/// <param name="tmaxItem">The item that identifies the modified record</param>
		/// <param name="eAction">The action taken on the record's codes collection</param>
		public override void OnSetCodes(FTI.Shared.Trialmax.CTmaxItem tmaxItem, TmaxCodeActions eAction)
		{
			CDxMediaRecord	dxOwner = null;
			CDxCode		dxCode = null;

			//	Get the owner record from the event item
			if((dxOwner = (CDxMediaRecord)(tmaxItem.GetMediaRecord())) != null)
			{
				//	Don't bother if this is not the active owner
				if(ReferenceEquals(dxOwner, m_dxOwner) == true)
				{
					//	Iterate the subitems to get all the code records
					foreach(CTmaxItem O in tmaxItem.SubItems)
					{
						//	Get the code record interface
						if((dxCode = (CDxCode)(O.ICode)) == null) continue;
						
						//	What is the action
						switch(eAction)
						{
							case TmaxCodeActions.Add:
							
								OnAdded(dxCode);
								break;
								
							case TmaxCodeActions.Delete:
							
								OnDeleted(dxCode);
								break;
								
							case TmaxCodeActions.Update:
							
								OnUpdated(dxCode);
								break;
								
						}// switch(eAction)
					
					}// foreach(CTmaxItem O in tmaxItem.SubItems)
					
				}// if(ReferenceEquals(dxOwner, m_dxOwner) == true)
				
			}// if((dxOwner = (CDxMediaRecord)(tmaxItem.GetMediaRecord())) != null)
			
		}// public override void OnSetCodes(FTI.Shared.Trialmax.CTmaxItem tmaxItem, TmaxCodeActions eAction)
		
		/// <summary>This method is called by the application to initialize the pane</summary>
		/// <returns>true if successful</returns>
		public override bool Initialize(CXmlIni xmlINI)
		{
			//	Do the base class initialization first
			if(base.Initialize(xmlINI) == false)
				return false;
				
			//	Make sure we are on the correct section
			xmlINI.SetSection(m_strPaneName);
				
			m_bShowUnassigned = xmlINI.ReadBool(XML_KEY_SHOW_UNASSIGNED, m_bShowUnassigned);
			m_bDropListBoolean = xmlINI.ReadBool(XML_KEY_DROP_LIST_BOOLEAN, m_bDropListBoolean);
			
			return true;
			
		}// public override bool Initialize()

		/// <summary>This method is called by the application prior to closing the active database</summary>
		/// <returns>True if OK to close the database</returns>
		public override bool CanCloseDatabase()
		{
			try
			{
				//	Make sure all changes have been committed
				if(m_dxOwner != null)
					m_ctrlPropGrid.EndUserUpdate(false);
			}
			catch
			{
			}

			return true;

		}// public override bool CanCloseDatabase()

		/// <summary>This method is called by the application when it is about to terminate</summary>
		/// <remarks>Derived classes should override for custom shutdown</remarks>
		public override void Terminate(CXmlIni xmlINI)
		{
			//	Make sure we are on the correct section
			xmlINI.SetSection(m_strPaneName);
				
			//	Store the pane options
			xmlINI.Write(XML_KEY_SHOW_UNASSIGNED, m_bShowUnassigned);
			xmlINI.Write(XML_KEY_DROP_LIST_BOOLEAN, m_bDropListBoolean);
		
			//	Clear the property grid control
			m_ctrlPropGrid.EndUserUpdate(true);
			m_ctrlPropGrid.Clear(true);
			
			//	Do the base class processing
			base.Terminate(xmlINI);
			
		}// public override void Terminate(CXmlIni xmlINI)
		
		/// <summary>This method is called by the application to when the item's child collection has been reordered</summary>
		/// <param name="tmaxItem">The item that owns the child collection</param>
		public override void OnReordered(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{
			Debug.Assert(tmaxItem != null);
			if(tmaxItem == null) return;
			
			//	Has the user reordered the case codes?
			if(tmaxItem.DataType == TmaxDataTypes.CaseCode)
			{
				//	Force a refill when the user closes the case options form
				m_bRefill = true;
			
			}// if(tmaxItem.DataType == TmaxDataTypes.CaseCode)
					
		}// public override void OnReordered(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		
		/// <summary>This function is called when the the user has closed the Case Options form</summary>
		/// <param name="bCancelled">true if the user cancelled the operation</param>
		public override void OnAfterSetCaseOptions(bool bCancelled)
		{
			//	Were any changes made that require a refill?
			if(m_bRefill == true)
			{
				//	Is the pane visible
				if(m_bPaneVisible == true)
				{
					if(m_dxOwner != null)
					{
						//	Force a refill
						m_dxActivate = m_dxOwner;
						m_dxOwner = null;
						SetOwner(m_dxActivate);
					}
					else
					{
						m_bRefill = false;
					}

				}
				else
				{
					//	Force an update when the pane goes active
					if(ReferenceEquals(m_dxOwner, m_dxActivate) == true)
					{
						m_dxOwner = null;
					}
					
				}// if(m_bActive == true)
			
			}// if(m_bRefill == true)
			
		}// public override void OnAfterSetCaseOptions(bool bCancelled)

		/// <summary>This function is called when the the application collection of case codes and pick lists is refreshed</summary>
		public override void OnRefreshCodes()
		{
			OnCaseCodesModified();
		}
		
		/// <summary>This method is called by the application when case codes Hidden property has been changed by the user</summary>
		/// <param name="tmaxItems">The items that identify the case codes (all codes if tmaxItems == null)</param>
		public override void OnHideCaseCodes(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			OnCaseCodesModified();
		}

		/// <summary>This method handles all Hotkey notifications from the application</summary>
		/// <param name="eHotkey">The enumerated hotkey</param>
		/// <returns>true if processed by the pane</returns>
		public override bool OnHotkey(TmaxHotkeys eHotkey)
		{
			CodesPaneCommands	eCommand = CodesPaneCommands.Invalid;
			int					iSelections = 0;

			//	Get the current number of row selections
			if((m_ctrlPropGrid != null) && (m_ctrlPropGrid.IsDisposed == false))
				iSelections = m_ctrlPropGrid.GetSelectedCount();
			
			//	Which hotkey has been pressed?
			switch(eHotkey)
			{
				case TmaxHotkeys.Delete:

					eCommand = CodesPaneCommands.Delete;
					break;

				default:

					break;

			}// switch(eHotkey)

			//	Did this hotkey translate to a command?
			if(eCommand != CodesPaneCommands.Invalid)
			{
				//	Is this command enabled
				if(GetCommandEnabled(eCommand, iSelections) == true)
				{
					//	Prompt for confirmation if attempting to delete records
					if((eCommand != CodesPaneCommands.Delete) || (GetDeleteConfirmation() == true))
						OnCommand(eCommand);
				}

			}// if(eCommand != CodesPaneCommands.Invalid)

			return (eCommand != CodesPaneCommands.Invalid);

		}// public override bool OnHotkey(TmaxHotkeys eHotkey)

		/// <summary>This method will fire the SetCodes command</summary>
		/// <param name="dxCode">The code being modified</param>
		/// <param name="eAction">The action identifier</param>
		public void FireSetCodes(CDxCode dxCode, TmaxCodeActions eAction)
		{
			CTmaxItems		tmaxItems = null;
			CTmaxItem		tmaxItem = null;
			CTmaxParameters	tmaxParameters = null;
			
			//	Don't bother if codes are not enabled
			Debug.Assert(GetCodesEnabled(), "Code operations have not been enabled");
			if(GetCodesEnabled() == false) return;
			
			Debug.Assert(m_dxCodes != null);
			if(m_dxCodes == null) return ;
			
			try
			{
				//	Create the items collection required to fire the event
				tmaxItems = new CTmaxItems();
				
				//	Create an item to identify the owner record and add it to the collection
				tmaxItem = new CTmaxItem(m_dxOwner);
				tmaxItems.Add(tmaxItem);
				
				//	Add an item for the code to the appropriate sub-collection
				if(eAction == TmaxCodeActions.Add)
				{
					if(tmaxItem.SourceItems == null)
						tmaxItem.SourceItems = new CTmaxItems();
						
					tmaxItem.SourceItems.Add(new CTmaxItem(dxCode));
				}
				else
				{
					tmaxItem.SubItems.Add(new CTmaxItem(dxCode));
				}
				
				//	Create the parameters required for the event
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.CodesAction, (int)eAction);
				
				//	Fire the command to delete the record
				FireCommand(TmaxCommands.SetCodes, tmaxItems, tmaxParameters);
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FireSetCodes", m_tmaxErrorBuilder.Message(ERROR_FIRE_SET_CODES_EX, eAction), Ex);
			}
			
		}// public void FireSetCodes(CDxCode dxCode, TmaxCodeActions eAction)
		
		/// <summary>This method is called to delete the specified code</summary>
		/// <param name="dxCode">The code being deleted</param>
		public void Delete(CDxCode dxCode)
		{
			Debug.Assert(m_dxCodes != null);
			if(m_dxCodes == null) return ;
			
			//	Does this code exist in the active owner's collection
			if(m_dxCodes.Contains(dxCode) == true)
			{
				FireSetCodes(dxCode, TmaxCodeActions.Delete);
			}

		}// public void Delete(CDxCode dxCode)
		
		/// <summary>This method is called to update the specified code</summary>
		/// <param name="dxCode">The code being updated</param>
		public void Update(CDxCode dxCode)
		{
			Debug.Assert(m_dxCodes != null);
			if(m_dxCodes == null) return ;
			
			//	Does this code exist in the active owner's collection
			if(m_dxCodes.Contains(dxCode) == true)
			{
				//	Inhibit handling of notifications from the application
				//	when we are causing the update
				m_bIgnoreUpdates = true;
				
				FireSetCodes(dxCode, TmaxCodeActions.Update);
				
				m_bIgnoreUpdates = false;
			}
			
		}// public void Update(CDxCode dxCode)
		
		/// <summary>This method is called to add the specified code</summary>
		/// <param name="dxCode">The code being updated</param>
		public void Add(CDxCode dxCode)
		{
			Debug.Assert(m_dxCodes != null);
			if(m_dxCodes == null) return ;
			
			//	Inhibit handling of notifications from the application
			//	when we are causing the update
			m_bIgnoreUpdates = true;
		
			FireSetCodes(dxCode, TmaxCodeActions.Add);
			
			m_bIgnoreUpdates = false;
			
		}// public void Add(CDxCode dxCode)
		
		/// <summary>This method is called to add the specified value to the pick list</summary>
		/// <param name="tmaxPickList">The parent pick list</param>
		/// <param name="strValue">The value to be added to the list</param>
		/// <returns>The new pick list value</returns>
		public CTmaxPickItem Add(CTmaxPickItem tmaxPickList, string strValue)
		{
			bool				bSuccessful = false;
			CTmaxItem			tmaxParent = null;
			CTmaxPickItem		tmaxValue = null;
			
			Debug.Assert(tmaxPickList != null);
			if(tmaxPickList == null) return null;
			
			try
			{
				//	Create a new pick item value 
				tmaxValue = new CTmaxPickItem();
				tmaxValue.ParentId = tmaxPickList.UniqueId;
				tmaxValue.Type = TmaxPickItemTypes.Value;
				tmaxValue.Name = strValue;
				
				//	Create an event item to identify the parent list
				tmaxParent = new CTmaxItem(tmaxPickList);
				
				//	Add an event item to identify the new value
				tmaxParent.SourceItems.Add(new CTmaxItem(tmaxValue));
				
				if((bSuccessful = FireCommand(TmaxCommands.Add, tmaxParent)) == true)
				{
					//	The user may have add a value to a parent of Unknown type
					if(tmaxPickList.Type == TmaxPickItemTypes.Unknown)
					{
						//	Update the type to indicate that it is now a value list
						tmaxPickList.Type = TmaxPickItemTypes.StringList;
						FireCommand(TmaxCommands.Update, new CTmaxItem(tmaxPickList));
					}
					
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_PICK_VALUE_EX, tmaxPickList.Name, strValue), Ex);
			}
			
			if(bSuccessful == true)
				return tmaxValue;
			else
				return null;
			
		}// public CTmaxPickItem Add(CTmaxPickItem tmaxPickList, string strValue)

		#endregion Public Methods
		
		#region Protected Methods

		/// <summary>This function is called when the value of the Database property changes</summary>
		protected override void OnDatabaseChanged()
		{
			//	Do the base class processing first
			base.OnDatabaseChanged();

			//	Clear out the existing information
			m_dxActivate = null;
			SetOwner(null);
			if(m_dxGridCodes != null)
				m_dxGridCodes.Database = m_tmaxDatabase;
				
			//	Has the user opened a new database?
			if(m_tmaxDatabase != null)
				SetUseFiltered(false); // Use media tree to start
				
		}// protected override void OnDatabaseChanged()
		
		/// <summary>This function is called when the value of the Filtered property changes</summary>
		protected override void OnFilteredChanged()
		{
			//	Has the user executed a new filter?
			if(m_dxFiltered != null)
				SetUseFiltered(true); // Switch to filtered view
				
		}// protected override void OnFilteredChanged()

		/// <summary>This function is called when the PaneVisible property changes</summary>
		protected override void OnPaneVisibleChanged()
		{
			//	Is the pane being made active?
			if(m_bPaneVisible == true)
			{
				if(ReferenceEquals(m_dxOwner, m_dxActivate) == false)
				{
					SetOwner(m_dxActivate);
				}
				
			}

		}// protected override void OnPaneVisibleChanged()
		
		/// <summary>Clean up any resources being used</summary>
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
		
		}// protected override void Dispose( bool disposing )

		/// <summary>Required method for Designer support</summary>
		protected override void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("MainToolbar");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("First");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Previous");
			Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool1 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("GoTo");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Next");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Last");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("Total");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleUseFiltered", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ShowUnassigned", "");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Add");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Add");
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ContextMenu");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Add");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AddAnother");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FilterSelected");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FilterExclude");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FilterText");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SetFilter");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RefreshFiltered");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("First");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Previous");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Next");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Last");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool3 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ShowUnassigned", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool4 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleUseFiltered", "");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("HideCaseCodes");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("Spacer");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete");
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool5 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ShowUnassigned", "");
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool22 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FilterSelected");
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool23 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FilterExclude");
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool24 = new Infragistics.Win.UltraWinToolbars.ButtonTool("FilterText");
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool25 = new Infragistics.Win.UltraWinToolbars.ButtonTool("HideCaseCodes");
			Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool26 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AddAnother");
			Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool27 = new Infragistics.Win.UltraWinToolbars.ButtonTool("First");
			Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool28 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Next");
			Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            // TP addition, Add shortcut for Next 
            buttonTool28.SharedProps.Shortcut = Shortcut.AltRightArrow;
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool29 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Previous");
			Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            // TP addition, Add shortcut for Previous 
            buttonTool29.SharedProps.Shortcut = Shortcut.AltLeftArrow;
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool30 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Last");
			Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool2 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("GoTo");
			Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool3 = new Infragistics.Win.UltraWinToolbars.LabelTool("Total");
			Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool6 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("ToggleUseFiltered", "");
			Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool31 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SetFilter");
			Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool32 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RefreshFiltered");
			Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CCodesPane));
			Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
			this.m_ctrlPropGrid = new FTI.Trialmax.Controls.CTmaxPropGridCtrl();
			this.m_ultraToolbarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
			this.m_ctrlToolbarImages = new System.Windows.Forms.ImageList(this.components);
			this._CCodesPane_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CCodesPane_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CCodesPane_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CCodesPane_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this.m_ctrlStatusBar = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
			((System.ComponentModel.ISupportInitialize)(this.m_ultraToolbarManager)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlPropGrid
			// 
			this.m_ctrlPropGrid.BackColor = System.Drawing.Color.White;
			this.m_ctrlPropGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlPropGrid.DropListBoolean = true;
			this.m_ctrlPropGrid.Location = new System.Drawing.Point(0, 73);
			this.m_ctrlPropGrid.Name = "m_ctrlPropGrid";
			this.m_ctrlPropGrid.PaneId = 0;
			this.m_ctrlPropGrid.Size = new System.Drawing.Size(316, 192);
			this.m_ctrlPropGrid.SortOn = ((long)(0));
			this.m_ctrlPropGrid.TabIndex = 0;
			this.m_ctrlPropGrid.GridMouseDown += new System.Windows.Forms.MouseEventHandler(this.OnGridMouseDown);
			this.m_ctrlPropGrid.AfterUpdate += new FTI.Trialmax.Controls.CTmaxPropGridCtrl.TmaxPropGridHandler(this.OnGridAfterUpdate);
			this.m_ctrlPropGrid.AddKeyPress += new FTI.Trialmax.Controls.CTmaxPropGridCtrl.TmaxPropGridHandler(this.OnGridAddKeyPress);
			this.m_ctrlPropGrid.BeforeUpdate += new FTI.Trialmax.Controls.CTmaxPropGridCtrl.TmaxPropGridHandler(this.OnGridBeforeUpdate);
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
			buttonTool1.InstanceProps.IsFirstInGroup = true;
			textBoxTool1.InstanceProps.Width = 46;
			labelTool1.InstanceProps.Width = 25;
			stateButtonTool1.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool2.InstanceProps.IsFirstInGroup = true;
			stateButtonTool2.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			buttonTool5.InstanceProps.IsFirstInGroup = true;
			ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool2,
            textBoxTool1,
            buttonTool3,
            buttonTool4,
            labelTool1,
            stateButtonTool1,
            stateButtonTool2,
            buttonTool5,
            buttonTool6});
			ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.FillEntireRow = Infragistics.Win.DefaultableBoolean.True;
			ultraToolbar1.Settings.GrabHandleStyle = Infragistics.Win.UltraWinToolbars.GrabHandleStyle.None;
			ultraToolbar1.Settings.ToolSpacing = -3;
			ultraToolbar1.ShowInToolbarList = false;
			ultraToolbar1.Text = "MainToolbar";
			this.m_ultraToolbarManager.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
			appearance1.Image = 2;
			buttonTool7.SharedProps.AppearancesSmall.Appearance = appearance1;
			buttonTool7.SharedProps.Caption = "&Add ...";
			buttonTool7.SharedProps.ToolTipText = "Add Selection (Shortcut = Ctrl+S)";
			popupMenuTool1.SharedProps.Caption = "ContextMenu";
			buttonTool8.InstanceProps.IsFirstInGroup = true;
			buttonTool11.InstanceProps.IsFirstInGroup = true;
			buttonTool14.InstanceProps.IsFirstInGroup = true;
			buttonTool16.InstanceProps.IsFirstInGroup = true;
			stateButtonTool3.InstanceProps.IsFirstInGroup = true;
			stateButtonTool3.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			stateButtonTool4.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			popupMenuTool1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
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
            buttonTool19,
            stateButtonTool3,
            stateButtonTool4,
            buttonTool20});
			appearance2.Image = 3;
			buttonTool21.SharedProps.AppearancesSmall.Appearance = appearance2;
			buttonTool21.SharedProps.Caption = "&Delete";
			stateButtonTool5.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			appearance3.Image = 0;
			stateButtonTool5.SharedProps.AppearancesSmall.Appearance = appearance3;
			stateButtonTool5.SharedProps.Caption = "Show &Unassigned";
			stateButtonTool5.SharedProps.ToolTipText = "Show Unassigned";
			appearance4.Image = 14;
			buttonTool22.SharedProps.AppearancesSmall.Appearance = appearance4;
			buttonTool22.SharedProps.Caption = "Filter On &Selection";
			appearance5.Image = 13;
			buttonTool23.SharedProps.AppearancesSmall.Appearance = appearance5;
			buttonTool23.SharedProps.Caption = "Filter &Exclude Selection";
			appearance6.Image = 15;
			buttonTool24.SharedProps.AppearancesSmall.Appearance = appearance6;
			buttonTool24.SharedProps.Caption = "&Filter On Text ...";
			appearance7.Image = 4;
			buttonTool25.SharedProps.AppearancesSmall.Appearance = appearance7;
			buttonTool25.SharedProps.Caption = "Show / Hide Data Fields ...";
			appearance8.Image = 16;
			buttonTool26.SharedProps.AppearancesSmall.Appearance = appearance8;
			buttonTool26.SharedProps.Caption = "Add Another";
			appearance9.Image = 8;
			buttonTool27.SharedProps.AppearancesSmall.Appearance = appearance9;
			buttonTool27.SharedProps.Caption = "First";
			appearance10.Image = 6;
			buttonTool28.SharedProps.AppearancesSmall.Appearance = appearance10;
			buttonTool28.SharedProps.Caption = "Next";
			appearance11.Image = 5;
			buttonTool29.SharedProps.AppearancesSmall.Appearance = appearance11;
			buttonTool29.SharedProps.Caption = "Previous";
			appearance12.Image = 7;
			buttonTool30.SharedProps.AppearancesSmall.Appearance = appearance12;
			buttonTool30.SharedProps.Caption = "Last";
			appearance13.TextHAlignAsString = "Right";
			textBoxTool2.EditAppearance = appearance13;
			appearance14.BackColorDisabled = System.Drawing.SystemColors.Control;
			textBoxTool2.SharedProps.AppearancesSmall.AppearanceOnToolbar = appearance14;
			textBoxTool2.SharedProps.Caption = "Go To";
			appearance15.TextHAlignAsString = "Left";
			labelTool3.SharedProps.AppearancesSmall.Appearance = appearance15;
			labelTool3.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
			labelTool3.SharedProps.ToolTipText = "Total Records";
			stateButtonTool6.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			appearance16.Image = 9;
			stateButtonTool6.SharedProps.AppearancesSmall.Appearance = appearance16;
			stateButtonTool6.SharedProps.Caption = "Use Filtered Tree";
			stateButtonTool6.SharedProps.ToolTipText = "Toggle Filtered Tree";
			appearance17.Image = 11;
			buttonTool31.SharedProps.AppearancesSmall.Appearance = appearance17;
			buttonTool31.SharedProps.Caption = "Set Filter ...";
			appearance18.Image = 12;
			buttonTool32.SharedProps.AppearancesSmall.Appearance = appearance18;
			buttonTool32.SharedProps.Caption = "Refresh Filtered";
			this.m_ultraToolbarManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool7,
            popupMenuTool1,
            labelTool2,
            buttonTool21,
            stateButtonTool5,
            buttonTool22,
            buttonTool23,
            buttonTool24,
            buttonTool25,
            buttonTool26,
            buttonTool27,
            buttonTool28,
            buttonTool29,
            buttonTool30,
            textBoxTool2,
            labelTool3,
            stateButtonTool6,
            buttonTool31,
            buttonTool32});
			this.m_ultraToolbarManager.BeforeToolDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventHandler(this.OnUltraBeforeDropDown);
			this.m_ultraToolbarManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.OnUltraToolClick);
			this.m_ultraToolbarManager.AfterToolCloseup += new Infragistics.Win.UltraWinToolbars.ToolDropdownEventHandler(this.OnUltraAfterCloseup);
			this.m_ultraToolbarManager.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.OnUltraBeforeToolbarListDropdown);
			this.m_ultraToolbarManager.ToolKeyDown += new Infragistics.Win.UltraWinToolbars.ToolKeyEventHandler(this.OnUltraToolKeyDown);
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
			// 
			// _CCodesPane_Toolbars_Dock_Area_Top
			// 
			this._CCodesPane_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CCodesPane_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
			this._CCodesPane_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
			this._CCodesPane_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CCodesPane_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
			this._CCodesPane_Toolbars_Dock_Area_Top.Name = "_CCodesPane_Toolbars_Dock_Area_Top";
			this._CCodesPane_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(316, 73);
			this._CCodesPane_Toolbars_Dock_Area_Top.ToolbarsManager = this.m_ultraToolbarManager;
			// 
			// _CCodesPane_Toolbars_Dock_Area_Bottom
			// 
			this._CCodesPane_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CCodesPane_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
			this._CCodesPane_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
			this._CCodesPane_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CCodesPane_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 265);
			this._CCodesPane_Toolbars_Dock_Area_Bottom.Name = "_CCodesPane_Toolbars_Dock_Area_Bottom";
			this._CCodesPane_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(316, 0);
			this._CCodesPane_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.m_ultraToolbarManager;
			// 
			// _CCodesPane_Toolbars_Dock_Area_Left
			// 
			this._CCodesPane_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CCodesPane_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
			this._CCodesPane_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
			this._CCodesPane_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CCodesPane_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 73);
			this._CCodesPane_Toolbars_Dock_Area_Left.Name = "_CCodesPane_Toolbars_Dock_Area_Left";
			this._CCodesPane_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 192);
			this._CCodesPane_Toolbars_Dock_Area_Left.ToolbarsManager = this.m_ultraToolbarManager;
			// 
			// _CCodesPane_Toolbars_Dock_Area_Right
			// 
			this._CCodesPane_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CCodesPane_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
			this._CCodesPane_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
			this._CCodesPane_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CCodesPane_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(316, 73);
			this._CCodesPane_Toolbars_Dock_Area_Right.Name = "_CCodesPane_Toolbars_Dock_Area_Right";
			this._CCodesPane_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 192);
			this._CCodesPane_Toolbars_Dock_Area_Right.ToolbarsManager = this.m_ultraToolbarManager;
			// 
			// m_ctrlStatusBar
			// 
			appearance19.TextHAlignAsString = "Center";
			this.m_ctrlStatusBar.Appearance = appearance19;
			this.m_ctrlStatusBar.BorderStyle = Infragistics.Win.UIElementBorderStyle.InsetSoft;
			this.m_ctrlStatusBar.Location = new System.Drawing.Point(0, 265);
			this.m_ctrlStatusBar.Name = "m_ctrlStatusBar";
			this.m_ctrlStatusBar.Size = new System.Drawing.Size(316, 23);
			this.m_ctrlStatusBar.TabIndex = 5;
			this.m_ctrlStatusBar.WrapText = false;
			// 
			// CCodesPane
			// 
			this.m_ultraToolbarManager.SetContextMenuUltra(this, "ContextMenu");
			this.Controls.Add(this.m_ctrlPropGrid);
			this.Controls.Add(this._CCodesPane_Toolbars_Dock_Area_Left);
			this.Controls.Add(this._CCodesPane_Toolbars_Dock_Area_Right);
			this.Controls.Add(this._CCodesPane_Toolbars_Dock_Area_Top);
			this.Controls.Add(this._CCodesPane_Toolbars_Dock_Area_Bottom);
			this.Controls.Add(this.m_ctrlStatusBar);
			this.Name = "CCodesPane";
			this.Size = new System.Drawing.Size(316, 288);
			((System.ComponentModel.ISupportInitialize)(this.m_ultraToolbarManager)).EndInit();
			this.ResumeLayout(false);

		}// protected override void InitializeComponent()
		
		/// <summary>Overloaded method called when the window is loaded the first time</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Initialize the viewer
			m_tmaxEventSource.Attach(m_ctrlPropGrid.EventSource);
			m_ctrlPropGrid.DropListBoolean = m_bDropListBoolean;
			m_ctrlPropGrid.SortOn = CTmaxCaseCodes.SORT_ON_ORDER;
			m_ctrlPropGrid.Initialize(null);
			
			//	Set the initial states of the toolbar buttons
			SetToolStates(false);
			
			//	Do the base class processing
			base.OnLoad (e);
		
		}// protected override void OnLoad(EventArgs e)

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			base.SetErrorStrings();
			
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to delete the user selections");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while trying to add the new code");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while changing the ShowAssigned option");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while populating the codes grid");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while changing the ShowAssigned option");
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while firing the SetCodes command: action = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to update the data field: name = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add a value to the pick list: list = %1 value = %2");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while processing the filter command: command = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add another code.");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to process a navigation command: %1");

		}// protected override void SetErrorStrings()

		/// <summary>Overridden by derived classes to provide access to the pane's toolbar manager</summary>
		/// <returns>The pane's toolbar manager if available</returns>
		protected override Infragistics.Win.UltraWinToolbars.UltraToolbarsManager GetUltraToolbarManager()
		{
			return m_ultraToolbarManager;
		}

		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>This method is called when the user wants to go to the barcode entered in the main toolbar</summary>
		/// <returns>true if successful</returns>
		private void OnCmdGoTo()
		{
			int			iSequence = -1;
			TextBoxTool	goTo = null;
			
			try
			{
				//	Get the sequence specified by the user
				if((goTo = (TextBoxTool)GetUltraTool(CodesPaneCommands.GoTo.ToString())) == null)
					return;
				if(goTo.Text.Length == 0)
					return;
					
				//	Convert to a sequence number
				try { iSequence = System.Convert.ToInt32(goTo.Text); }
				catch { iSequence = -1; }
				
				//	Did the caller provided a valid sequence identifier?
				if(iSequence <= 0)
				{
					Warn("You must provide a valid sequence number");
					SetFocusGoTo();
				}
				
					//	Is the sequence within range?
				else if(iSequence > m_iNavigatorTotal)
				{
					Warn("You must provide a sequence number between 1 and " + m_iNavigatorTotal.ToString());
					SetFocusGoTo();
				}
				else
				{
					//	Fire the event to activate this record
					FireNavigate(iSequence - 1);
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnCmdGoTo", Ex);
			}
			
		}// private void OnCmdGoTo()
		
		/// <summary>This method is called to fire the command to navigate to the requested record</summary>
		/// <param name="iIndex">The index of the record to be activated</param>
		/// <returns>true if successful</returns>
		private bool FireNavigate(int iIndex)
		{
			CDxPrimary	dxActivate = null;
			bool		bSuccessful = false;
			
			try
			{
				if(iIndex < 0) return false; // Just in case...
				
				//	Which collection are we using?
				if(m_bUseFiltered == true)
				{
					if((m_dxFiltered != null) && (m_dxFiltered.Count > iIndex))
						dxActivate = m_dxFiltered[iIndex];
				}
				else
				{
					if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null) && (m_tmaxDatabase.Primaries.Count > iIndex))
						dxActivate = m_tmaxDatabase.Primaries[iIndex];
				}
				
				//	Do we have a record to activate?
				if(dxActivate != null)
					bSuccessful = FireActivate(dxActivate, true);
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "FireNavigate", Ex);
			}
			
			return bSuccessful;
			
		}// private bool FireNavigate(int iIndex)
		
		/// <summary>This method is called to fire the command to activate requested record</summary>
		/// <param name="dxActivate">The record to be activated</param>
		/// <param name="bSynchronize">True to synchronize the active tree</param>
		/// <returns>true if successful</returns>
		private bool FireActivate(CDxMediaRecord dxActivate, bool bSynchronize)
		{
			CTmaxItem		tmaxItem = null;
			CTmaxParameters	tmaxParameters = null;
			bool			bSuccessful = false;
			
			try
			{
				//	Create the event item for this record
				tmaxItem = new CTmaxItem(dxActivate);
					
				//	Should we synchronize the tree?
				if(bSynchronize == true)
				{
					//	Create the parameters required for the event
					tmaxParameters = new CTmaxParameters();
					if(m_bUseFiltered == true)
						tmaxParameters.Add(TmaxCommandParameters.SyncFilterTree, true);
					else
						tmaxParameters.Add(TmaxCommandParameters.SyncMediaTree, true);
				
				}// if(bSynchronize == true)
				
				//	Fire the command to activate the record
				bSuccessful = FireCommand(TmaxCommands.Activate, tmaxItem, tmaxParameters);
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "FireActivate", Ex);
			}
			
			return bSuccessful;
			
		}// private bool FireActivate(CDxMediaRecord dxRecord, bool bSynchronize)
		
		/// <summary>This method handles events fired by the grid before it updates a code's value</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The property grid event arguments</param>
		private void OnGridBeforeUpdate(object sender, FTI.Trialmax.Controls.CTmaxPropGridCtrl.CTmaxPropGridArgs e)
		{
			CDxCode dxCode = null;
			
			try
			{
				if((dxCode = (CDxCode)(e.IProperty)) != null)
				{
					//	Is the user deleting this code?
					if((e.Value == null) || (e.Value.Length == 0))
					{
						//	Allow the user to delete the code
						e.Cancel = false;
					}
					else
					{	
						e.Cancel = OnBeforeUpdate(dxCode, e.Value, e.PickList, e.SupressConfirmations);

					}// if((e.Value == null) || (e.Value.Length == 0))

				}
				else
				{
					m_tmaxEventSource.FireDiagnostic(this, "OnGridBeforeUpdate", "NULL TAG");
				}
				
			}
			catch
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnGridBeforeUpdate", "Exception");
			}
			
		}// private void OnGridBeforeUpdate(object sender, FTI.Trialmax.Controls.CTmaxPropGridCtrl.CTmaxPropGridArgs e)
		
		/// <summary>This method handles events fired by the grid when it has updated a code's value</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The property grid event arguments</param>
		private void OnGridAfterUpdate(object sender, FTI.Trialmax.Controls.CTmaxPropGridCtrl.CTmaxPropGridArgs e)
		{
			try
			{
				//	Store the arguments
				m_tmaxAfterUpdateArgs = e;
				
				//	Should we process asynchronously?
				if(m_delAfterUpdate != null)
					BeginInvoke(m_delAfterUpdate);
				else
					OnAfterUpdate();
			
			}
			catch
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnGridAfterUpdate", "Exception");
			}
			
		}// private void OnGridAfterUpdate(object sender, FTI.Trialmax.Controls.CTmaxPropGridCtrl.CTmaxPropGridArgs e)
		
		/// <summary>This method handles events fired by the grid when it has updated a code's value</summary>
		private void OnAfterUpdate()
		{
			CDxCode dxCode = null;
			
			if(m_tmaxAfterUpdateArgs == null) return;
			
			try
			{
				//	Get the code associated with the update
				dxCode = (CDxCode)(m_tmaxAfterUpdateArgs.IProperty);
				m_tmaxAfterUpdateArgs = null;

				if(dxCode == null)
				{
					m_tmaxEventSource.FireDiagnostic(this, "OnAfterUpdate", "NULL TAG REFERENCE");
					return;
				}
			
				//	Has the user deleted this code?
				if(dxCode.IsNull() == true)
				{
					//	Delete this code
					Delete(dxCode);
				}
				else
				{
					//	Is this an existing code?
					if(m_dxCodes.Contains(dxCode) == true)
						Update(dxCode);
					else
						Add(dxCode);
				}
			
			}
			catch
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnAfterUpdate", "Exception");
			}
			
		}// private void OnAfterUpdate()
		
		/// <summary>This method is called before the specified value gets assigned to the specified code</summary>
		/// <param name="dxCode">The code being updated</param>
		/// <param name="strValue">The new value to be assigned</param>
		/// <param name="tmaxPickList">The pick list required for multi-level data codes</param>
		/// <param name="bSuppressConfirmations">True to suppress user confirmation dialogs</param>
		/// <returns>True if the operation should be cancelled</returns>
		private bool OnBeforeUpdate(CDxCode dxCode, string strValue, CTmaxPickItem tmaxPickList, bool bSuppressConfirmations)
		{
			CTmaxPickItem	tmaxPickValue = null;
			CTmaxPickItem	tmaxValueList = null;
			bool			bCancel = false;
			
			try
			{
				//	Is this a pick list code?
				if(dxCode.Type == TmaxCodeTypes.PickList)
				{
					//	Do we have access to the pick list bound to this code
					if((dxCode.CaseCode == null) || (dxCode.CaseCode.PickList == null))
					{
						Warn("Unable to retrieve the pick list for the data code.");
						return true; // Cancel the operation
					}
					
					//	Is this a multilevel code?
					if(dxCode.IsMultiLevel == true)
					{
						//	Use the list already bound to the data code if not specified by the user
						if((tmaxValueList = tmaxPickList) == null)
							tmaxValueList = dxCode.PickList;
							
						if(tmaxValueList == null)
						{
							Warn("Unable to retrieve the parent pick list for the data code.");
							return true; // Cancel the operation
						}
						
					}
					else
					{
						//	Use the pick list bound to the case code
						tmaxValueList = dxCode.CaseCode.PickList;
					}
						
					//	Get the pick list value with the specified name
					if((tmaxPickValue = dxCode.GetPickItem(strValue, tmaxValueList)) == null)
					{
						//	Is the user allowed to add to this pick list?
						if(tmaxValueList.UserAdditions == true)
						{
							//	Should we prompt for confirmation?
							if(bSuppressConfirmations == false)
							{
								if(MessageBox.Show(strValue + " does not appear in the list of available values. Do you want to add it now? ", "Add Value?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
								{
									bCancel = true;
								}
							}
							
							if(bCancel == false)
								bCancel = (Add(tmaxValueList, strValue) == null);
						}
						else
						{
							Warn(strValue + " does not appear in the list of available values. This case does not permit on-the-fly entry of additional pick list values. Pick list entries may only be performed via Tools : Case Options : Pick Lists.");
							bCancel = true;
						}
									
					}// if((tmaxPickValue = dxCode.GetPickItem(strValue)) == null)
					else
					{
						bCancel = false; // Ok to keep going
					}
								
				}
				else
				{
					//	Is this a valid value for this code?
					if(CTmaxCaseCode.IsValid(dxCode.Type, strValue) == true)
					{
						bCancel = false;
					}
					else
					{
						MessageBox.Show(strValue + " is not a valid value for " + dxCode.Name, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						bCancel = true;
					}
							
				}// if(dxCode.Type == TmaxCodeTypes.PickList)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnBeforeUpdate", m_tmaxErrorBuilder.Message(ERROR_ON_BEFORE_UPDATE_EX, dxCode.Name), Ex);
			}
			
			return bCancel;
			
		}// private bool OnBeforeUpdate(CDxCode dxCode, string strValue)
		
		/// <summary>This method will populate the property grid with the active codes collection</summary>
		private void Fill()
		{
			CDxCode dxCode = null;
			
			try
			{
				//	Flush the existing collection of grid codes
				Debug.Assert(m_dxGridCodes != null);
				m_dxGridCodes.Clear();
				
				//	Make sure the active database is assigned to the
				//	collection so that pick list assignments can be resolved
				m_dxGridCodes.Database = m_tmaxDatabase;
				
				//	Does the active owner support codes?
				if((m_dxCodes != null) && (GetCodesEnabled() == true))
				{
					//	Transfer the owner codes to the grid codes collection
					foreach(CDxCode O in m_dxCodes)
					{
						if((O.CaseCode != null) && (O.CaseCode.Hidden == false))
							m_dxGridCodes.AddList(O);
					}
						
					//	Do we need to add unassigned codes?
					if((m_bShowUnassigned == true) && (this.CaseCodes != null))
					{
						//	Make sure at least one code of each type exists in the collection
						foreach(CTmaxCaseCode O in this.CaseCodes)
						{
							if((O.Hidden == false) && (m_dxGridCodes.Find(O) == null))
							{
								//	Create a new placeholder and add to the list
								//
								//	NOTE:	We must assign the collection for pick list assignments to be resolved
								dxCode = new CDxCode(O);
								dxCode.Collection = m_dxGridCodes;

								m_dxGridCodes.AddList(dxCode);

							}
							
						}// foreach(CTmaxCaseCode O in this.CaseCodes)
							
					}// if((m_bShowUnassigned == true) && (this.TmaxMetaFields != null))
				
				}// if(m_dxCodes != null)
				
				//	Make sure the codes are in sorted order
				m_dxGridCodes.Sort(true);
				
				//	Notify the property grid control
				if((m_ctrlPropGrid != null) && (m_ctrlPropGrid.IsDisposed == false))
				{
					try { m_ctrlPropGrid.Add(m_dxGridCodes, true); }
					catch {}
				}
				
				//	Clear this flag after the grid is filled
				m_bRefill = false;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Fill", m_tmaxErrorBuilder.Message(ERROR_FILL_EX), Ex);
			}

		}// private void Fill()
		
		/// <summary>This method is called to set the owner of the codes displayed in this pane</summary>
		/// <param name="dxOwner">The exchange interface of the owner record</param>
		private void SetOwner(CDxMediaRecord dxOwner)
		{
			//	Don't bother if codes are not enabled
			if(GetCodesEnabled() == false) return;
			
			//	Has the record changed?
			if(ReferenceEquals(dxOwner, m_dxOwner) == false)
			{
				//	Update the local member
				m_dxOwner = dxOwner;
				
				//	Update the codes collection
				if(m_dxOwner != null)
					m_dxCodes = m_dxOwner.GetCodes(true);
				else
					m_dxCodes = null;
					
				//	Populate the property grid
				Fill();
				
				//	Update the toolbar buttons
				SetToolStates(false);
		
			}// if(ReferenceEquals(dxOwner, m_dxOwner) == false)
			
		}// private void SetOwner(CDxMediaRecord dxOwner)
		
		/// <summary>This method is called to determine if the event item references the current owner record</summary>
		/// <param name="tmaxItem">The event item to be checked</param>
		/// <param name="bSubItems">True to recurse into the SubItems collection in search of the owner</param>
		/// <returns>true if it references the owner record</returns>
		private bool IsOwner(CTmaxItem tmaxItem, bool bSubItems)
		{
			CDxMediaRecord dxRecord = null;
			
			//	Do we have an owner record?
			if(m_dxOwner == null) return false;
			
			//	Did the caller provide a valid item?
			if(tmaxItem == null) return false;
			
			//	Get the record associated with this item
			dxRecord = (CDxMediaRecord)(tmaxItem.GetMediaRecord());
			
			if(dxRecord != null)
			{
				//	Is this the owner record?
				if(ReferenceEquals(dxRecord, m_dxOwner) == true)
					return true;
			}
			
			//	Should we check the subitems?
			if((bSubItems == true) && (tmaxItem.SubItems != null))
			{
				if(ContainsOwner(tmaxItem.SubItems, true) == true)
					return true;				
			}
			
			//	Must not be the owner
			return false;
			
		}// private bool IsOwner(CTmaxItem tmaxItem)
		
		/// <summary>This method is called to determine if the event item collection contains one or more items that reference the current owner record</summary>
		/// <param name="tmaxItems">The event items to be checked</param>
		/// <param name="bSubItems">True to recurse into the SubItems collection in search of the owner</param>
		/// <returns>true if one or more items reference the owner record</returns>
		private bool ContainsOwner(CTmaxItems tmaxItems, bool bSubItems)
		{
			if((tmaxItems != null) && (tmaxItems.Count > 0))
			{
				foreach(CTmaxItem O in tmaxItems)
				{
					if(IsOwner(O, bSubItems) == true)
						return true;
				}
				
			}
			
			//	Must not reference the owner
			return false;
			
		}// private bool ContainsOwner(CTmaxItems tmaxItems)
		
		/// <summary>This method is called when the owner record has been updated</summary>
		private void OnUpdated()
		{
			//	Don't bother if the update is because of the user working in this pane
			if(m_bIgnoreUpdates == false)
			{
				//	Is the pane visible
				if(m_bPaneVisible == true)
				{
					//	Update the property grid
					Fill();
				}
				else
				{
					//	Force an update when the pane goes active
					if(ReferenceEquals(m_dxOwner, m_dxActivate) == true)
					{
						m_dxOwner = null;
					}
					
				}// if(m_bActive == true)
			
			}// if(m_bIgnoreUpdates == false)
		
		}// private void OnUpdated()
		
		/// <summary>This function is called when a code has been deleted</summary>
		/// <param name="dxCode">The code that has been deleted</param>
		private void OnDeleted(CDxCode dxCode)
		{
			long	lId = 0;
			CDxCode	dxGrid = null;
			
			//	To be deleted the code must have been assigned an ID
			//
			//	NOTE:	We use the ID wherever possible because the codes
			//			collection may have been refreshed in which case
			//			the object references have changed since the grid 
			//			got populated
			if((lId = dxCode.AutoId) <= 0)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnDeleted", "NO ID ASSIGNED TO CODE TO BE DELETED");
			}

			//	Nothing to do if not in the grid's collection
			if((dxGrid = m_dxGridCodes.Find(lId)) == null) return;
			
			dxGrid.AutoId = 0;
			dxGrid.SetValue("");
			m_ctrlPropGrid.Update(dxGrid);
			
//			//	Are we showing unassigned codes?
//			if(m_bShowUnassigned == true)
//			{
//			}
//			else
//			{
//				//	Delete the row in the grid and the record in the grid collection
//				m_ctrlPropGrid.Delete(lId);
//				m_dxGridCodes.Remove(dxGrid);
//			}
				
		}// private void OnDeleted(CDxCode dxCode)
		
		/// <summary>This method handles MouseDown events fired by the child property grid</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">system event arguments</param>
		private void OnGridMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			m_dxEditorCode = null;
			m_strEditorText = "";
			m_strSelectedText = "";
			m_lSelectedIndex = -1;

			//	Is this a right click?
			if(e.Button == MouseButtons.Right)
			{
				//	Get the code being edited
				m_dxEditorCode = (CDxCode)(m_ctrlPropGrid.GetEditorProps(ref m_strEditorText, ref m_strSelectedText, ref m_lSelectedIndex));
				
				//if(m_dxEditorCode != null)
				//    m_tmaxEventSource.FireDiagnostic(this, "ONGRIDMOUSEDOWN", "code = " + m_dxEditorCode.Name + "  all = " + m_strEditorText + "  -->  selected = " + m_strSelectedText );			
				//else
				//    m_tmaxEventSource.FireDiagnostic(this, "ONGRIDMOUSEDOWN", "code = NULL");			
				
			}// if(e.Button == MouseButtons.Right)

		}// private void OnGridMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		
		/// <summary>This function is called when a code has been added</summary>
		/// <param name="dxCode">The code that has been added</param>
		private void OnAdded(CDxCode dxCode)
		{
			//	Does this object already exist in the grid?
			//
			//	NOTE:	It may be getting added because the user supplied
			//			a value via the grid editor
			if(m_dxGridCodes.Contains(dxCode) == true)
			{
				//	This will update the ID field in the grid
				m_ctrlPropGrid.Update(dxCode);
			}
			else
			{
				m_dxGridCodes.AddList(dxCode);
				m_dxGridCodes.Sort(true);
				
				//	Add it to the grid
				m_ctrlPropGrid.Add(dxCode);
				m_ctrlPropGrid.Sort();
			}
				
		}// private void OnAdded(CDxCode dxCode)
		
		/// <summary>This function is called when a code has been updated</summary>
		/// <param name="dxCode">The code that has been updated</param>
		private void OnUpdated(CDxCode dxCode)
		{
			long lId = 0;
			
			//	If this is an update the code should be assigned an ID
			//
			//	NOTE:	We use the ID wherever possible because the codes
			//			collection may have been refreshed in which case
			//			the object references have changed since the grid 
			//			got populated
			if((lId = dxCode.AutoId) > 0)
			{
				//	Is this code in the grid's collection?
				if(m_dxGridCodes.Find(lId) != null)
					m_ctrlPropGrid.Update(dxCode, lId);
			}
			else
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnUpdated", "NO ID ASSIGNED TO CODE TO BE UPDATED");
			}
				
		}// private void OnUpdated(CDxCode dxCode)
		
		/// <summary>This method is called to convert the key to a command identifier</summary>
		/// <param name="strKey">The Infragistic key identifier</param>
		/// <returns>The associated pane command</returns>
		private CodesPaneCommands GetCommand(string strKey)
		{
			try
			{
				Array aCommands = Enum.GetValues(typeof(CodesPaneCommands));
				
				foreach(CodesPaneCommands eCommand in aCommands)
				{
					if(eCommand.ToString() == strKey)
						return eCommand;
				}
				
			}
			catch
			{
			}
			
			return CodesPaneCommands.Invalid;
		
		}// private CodesPaneCommands GetCommand(string strKey)
		
		/// <summary>This method is called to determine if the specified command should be enabled</summary>
		/// <param name="eCommand">The transcript pane command enumeration</param>
		/// <param name="iSelections">The number of rows currently selected</param>
		/// <returns>true if command should be enabled</returns>
		private bool GetCommandEnabled(CodesPaneCommands eCommand, int iSelections)
		{
			CDxCode dxCode = null;
			string	strText = "";
			
			//	We have to have an active database with codes
			if(m_tmaxDatabase == null) return false;
			if(m_tmaxDatabase.Primaries == null) return false;
			if(GetCodesEnabled() == false) return false;
				
			//	What is the command?
			switch(eCommand)
			{
				case CodesPaneCommands.ShowUnassigned:
				case CodesPaneCommands.ToggleUseFiltered:
				
					return true;
					
				case CodesPaneCommands.SetFilter:
				case CodesPaneCommands.RefreshFiltered:
				
					return (m_tmaxDatabase.Primaries.Count > 0);
					
				case CodesPaneCommands.Add:
				
					if(m_dxCodes == null) return false;
					if(this.CaseCodes == null) return false;
					if(this.CaseCodes.Count == 0) return false;
					
					//	It's all good
					return true;

				case CodesPaneCommands.Delete:
				
					if(m_dxGridCodes == null) return false;
					if(m_dxGridCodes.Count == 0) return false;
					
					return true;
					//return (iSelections > 0);
				
				case CodesPaneCommands.FilterText:
				
					return (GetCmdFilterCode(ref strText) != null);
					
				case CodesPaneCommands.FilterSelected:
				case CodesPaneCommands.FilterExclude:
				
					dxCode = GetCmdFilterCode(ref strText);
					
					if(dxCode == null) return false;
					if(strText.Length == 0) return false;
					
					return true;
					
				case CodesPaneCommands.HideCaseCodes:
				
					if(m_tmaxDatabase == null) return false;
					if(this.CaseCodes == null) return false;
					if(this.CaseCodes.Count == 0) return false;
					return true;
					
				case CodesPaneCommands.AddAnother:
				
					return (GetCmdAddAnotherCode(true) != null);
					
				case CodesPaneCommands.GoTo:
				
					return (m_iNavigatorTotal > 0);
					
				case CodesPaneCommands.First:
				case CodesPaneCommands.Previous:
				
					if(m_iNavigatorTotal == 0) return false;
					
					// NOTE:	We have to use the index instead of m_dxOwner because the
					//			user may have selected a record that's not in the active navigation collection
					if(m_iNavigatorIndex >= 0)
						return (m_iNavigatorIndex > 0);
					else
						return (eCommand == CodesPaneCommands.First);
					
				case CodesPaneCommands.Last:
				case CodesPaneCommands.Next:
				
					if(m_iNavigatorTotal == 0) return false;
					
					if(m_iNavigatorIndex >= 0)
						return (m_iNavigatorIndex < m_iNavigatorTotal - 1);
					else
						return (eCommand == CodesPaneCommands.Last);
					
				default:
				
					break;
			
			}// switch(eCommand)	
			
			return false;
		
		}// private bool GetCommandEnabled(CodesPaneCommands eCommand, int iSelections)
		
		/// <summary>This method processes the specified command</summary>
		/// <param name="eCommand">The enumerated command identifier</param>
		private void OnCommand(CodesPaneCommands eCommand)
		{
			int	iSelections = 0;
			
			//	Get the current number of row selections
			if((m_ctrlPropGrid != null) && (m_ctrlPropGrid.IsDisposed == false))
				iSelections = m_ctrlPropGrid.GetSelectedCount();
			
			try
			{
				//	Direct to the appropriate handler
				switch(eCommand)
				{
					case CodesPaneCommands.First:
					case CodesPaneCommands.Previous:
					case CodesPaneCommands.Next:
					case CodesPaneCommands.Last:
					
						OnCmdNavigate(eCommand);
						break;

					case CodesPaneCommands.ShowUnassigned:
					
						OnCmdShowUnassigned();
						break;
						
					case CodesPaneCommands.ToggleUseFiltered:
					
						OnCmdToggleUseFiltered();
						break;
						
					case CodesPaneCommands.HideCaseCodes:
					
						OnCmdHideCaseCodes();
						break;
						
					case CodesPaneCommands.Add:
					
						OnCmdAdd();
						break;
						
					case CodesPaneCommands.AddAnother:
					
						OnCmdAddAnother(true);
						break;
						
					case CodesPaneCommands.Delete:
					
						OnCmdDelete();
						break;

					case CodesPaneCommands.FilterExclude:
					case CodesPaneCommands.FilterSelected:
					case CodesPaneCommands.FilterText:
					
						OnCmdFilter(eCommand);
						break;

					case CodesPaneCommands.SetFilter:
					
						OnCmdSetFilter();
						break;

					case CodesPaneCommands.RefreshFiltered:
					
						OnCmdRefreshFiltered();
						break;

					default:
					
						break;
				
				}// switch(eCommand)

			}
			catch
			{
				//MessageBox.Show(Ex.ToString());
				//Debug.Assert(false);
			}
		
		}// private void OnCommand(CodesPaneCommands eCommand)

		/// <summary>Event handler for the ShowUnassigned command</summary>
		private void OnCmdShowUnassigned()
		{
			CTmaxItem tmaxOwner = null;
			
			try
			{
				//	Make sure existing changes have been processed
				m_ctrlPropGrid.EndUserUpdate(false);
				
				//	Toggle the state of the unassigned codes
				m_bShowUnassigned = !m_bShowUnassigned;
				
				//	Force a repopulation of the grid if we have an active owner
				if(m_dxOwner != null)
				{
					tmaxOwner = new CTmaxItem(m_dxOwner);
					m_dxOwner = null;
					
					Activate(tmaxOwner, TmaxAppPanes.Codes);
				}
				else
				{
					//	Make sure the toolbar button has the right image
					SetToolStates(false);
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdShowAssigned", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_SHOW_ASSIGNED_EX), Ex);
			}
			
		}// private void OnCmdShowUnassigned()
		
		/// <summary>Event handler for the HideCaseCodes command</summary>
		private void OnCmdHideCaseCodes()
		{
			
			try
			{
				FireCommand(TmaxCommands.HideCaseCodes);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnCmdHideCaseCodes", Ex);
			}
			
		}// private void OnCmdHideCaseCodes()
		
		/// <summary>Event handler for the Add command</summary>
		private void OnCmdAdd()
		{
			CDxCode			dxCode = null;
			CTmaxPickItem	tmaxValue = null;
			CTmaxPickItem	tmaxPickList = null;
			
			FTI.Trialmax.Forms.CFAddCode addCode = null;
			
			if(m_dxOwner == null) return;
			if(m_dxCodes == null) return;
			if(this.CaseCodes == null) return;
			if(this.CaseCodes.Count == 0) return;
			
			try
			{
				//	Make sure existing changes have been processed
				m_ctrlPropGrid.EndUserUpdate(false);
				
				//	Create the form for adding a new code
				addCode = new FTI.Trialmax.Forms.CFAddCode();
				
				//	Initialize the form
				m_tmaxEventSource.Attach(addCode.EventSource);
				
				//	Assign a collection for the form
				addCode.CaseCodes = new CTmaxCaseCodes();
				
				//	Populate the collection
				foreach(CTmaxCaseCode O in this.CaseCodes)
				{
					//	Should we add to the collection?
					if(O.Hidden == false)
					{
						if((O.AllowMultiple == true) || (m_dxCodes.Find(O) == null))
							addCode.CaseCodes.Add(O);
					}
				
				}
				addCode.CaseCodes.Sort(true);
					
				//	Open the form
				if(addCode.ShowDialog() == DialogResult.OK)
				{
					Debug.Assert(addCode.CaseCode != null);
					Debug.Assert(addCode.Value != null);
					Debug.Assert(addCode.Value.Length > 0);
					
					//	Do we need to add a pick list value?
					if((addCode.CaseCode.Type == TmaxCodeTypes.PickList) && (addCode.AddValue == true))
					{
						//	Which pick list are we adding to?
						if(addCode.CaseCode.IsMultiLevel == true)
							tmaxPickList = addCode.MultiLevelSelection.Parent;
						else
							tmaxPickList = addCode.CaseCode.PickList;

						//	Add the new value to the pick list
						if((tmaxValue = Add(tmaxPickList, addCode.Value)) == null)
						{
							return;
						}
							
					}
						
					//	See if we have an unassigned code of the same type available in the grid
					foreach(CDxCode O in m_dxGridCodes)
					{
						if(O.CaseCodeId == addCode.CaseCode.UniqueId)
						{
							if(O.AutoId <= 0)
							{
								dxCode = O;
								break;
							}
						}
							
					}// foreach(CDxCode O in m_dxGridCodes)
						
					//	Do we need to allocate a new code?
					if(dxCode == null)
						dxCode = new CDxCode(addCode.CaseCode, m_dxOwner);
					
					//	Set the value
					if(addCode.MultiLevelSelection != null)
					{
						//	Make sure the code has been assigned the correct pick list
						dxCode.PickList = addCode.MultiLevelSelection.Parent;
						
						if(tmaxValue != null)
							dxCode.SetValue(tmaxValue.UniqueId.ToString());
						else
							dxCode.SetValue(addCode.Value);
					}
					else
					{
						if(tmaxValue != null)
							dxCode.SetValue(tmaxValue.UniqueId.ToString());
						else
							dxCode.SetValue(addCode.Value);
					}						
					//	Add the code
					FireSetCodes(dxCode, TmaxCodeActions.Add);
					
				}// if(addCode.ShowDialog() == DialogResult.OK)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdAdd", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_ADD_EX), Ex);
			}
			
		}// private void OnCmdAdd()
		
		/// <summary>Event handler for the AddAnother command</summary>
		/// <param name="bSilent">True to suppress MessageBeep if no code available</param>
		private void OnCmdAddAnother(bool bSilent)
		{
			CDxCode dxCode = null;

			if(m_dxOwner == null) return;
			if(m_dxCodes == null) return;
			if(this.CaseCodes == null) return;
			if(this.CaseCodes.Count == 0) return;
			
			try
			{
				if((dxCode = GetCmdAddAnotherCode(bSilent)) != null)
				{
					if(m_ctrlPropGrid.Add(dxCode) == true)
					{
						m_dxGridCodes.AddList(dxCode);
						
						m_ctrlPropGrid.Sort();
						m_ctrlPropGrid.SetSelection(dxCode, true);
					
					}// if(m_ctrlPropGrid.Add(dxCode) == true)
					
				}// if((dxCode = GetCmdAddAnotherCode()) != null)
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdAddAnother", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_ADD_ANOTHER_EX), Ex);
			}
			
		}// private void OnCmdAddAnother()
		
		/// <summary>Event handler for the Delete command</summary>
		private void OnCmdDelete()
		{
			ArrayList		aSelected = null;
			CTmaxItem		tmaxOwner = null;
			CDxCode			dxDelete = null;
			CTmaxParameters	tmaxParameters = null;
			
			if(m_dxOwner == null) return;
			if(m_dxCodes == null) return;
			if(m_dxCodes.Count == 0) return;
			
			try
			{
				//	Get the codes that are selected in the grid
				aSelected = m_ctrlPropGrid.GetSelected();
				
				//	Has the user selected any rows?
				if((aSelected != null) && (aSelected.Count > 0))
				{
					//	Allocate an event item to represent the owner
					tmaxOwner = new CTmaxItem(m_dxOwner);
					
					foreach(ITmaxPropGridCtrl O in aSelected)
					{
						//	Should have a valid database id to delete
						if(O.GetId() > 0)
						{
							//	Look for the object in the master list
							if((dxDelete = m_dxCodes.Find(O.GetId())) != null)
							{
								//	Add this to the SubItems collection
								tmaxOwner.SubItems.Add(new CTmaxItem(dxDelete));
									
							}// if((dxDelete = m_dxCodes.Find(O.GetId())) != null)
							
						}// if(O.GetId() > 0)
						
					}// foreach(ITmaxPropGridCtrl O in aSelected)
						
				}// if((aSelected != null) && (aSelected.Count > 0))
				
				//	Do we have any codes to delete?
				if((tmaxOwner != null) && (tmaxOwner.SubItems.Count > 0))
				{
					//	Create the parameters required for the event
					tmaxParameters = new CTmaxParameters();
					tmaxParameters.Add(TmaxCommandParameters.CodesAction, (int)(TmaxCodeActions.Delete));
					
					//	Fire the command event
					FireCommand(TmaxCommands.SetCodes, tmaxOwner, tmaxParameters);
				}
				else
				{
					MessageBox.Show("You must highlight one or more assigned codes to be deleted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
				
				}// if((dxCodes == null) || (dxCodes.Count == 0))
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdDeleted", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_DELETE_EX), Ex);
			}
			
		}// private void OnCmdDelete()
		
		/// <summary>Event handler for the Filter commands command</summary>
		/// <param name="eCommand">The filter command being processed</param>
		private void OnCmdFilter(CodesPaneCommands eCommand)
		{
			CTmaxItem		tmaxCode = null;
			CDxCode			dxCode = null;
			CTmaxParameters	tmaxParameters = null;
			long			lFlags = 0;
			string			strText = "";
			
			if(m_dxGridCodes == null) return;
			if(m_dxGridCodes.Count == 0) return;
			
			try
			{
				//	Get the selected code
				if((dxCode = GetCmdFilterCode(ref strText)) != null)
				{
					tmaxCode = new CTmaxItem(dxCode.CaseCode);
					tmaxParameters = new CTmaxParameters();
					
					//	Are we supposed to prompt the user for the text?
					if(eCommand == CodesPaneCommands.FilterText)
					{
						lFlags = (long)(TmaxSetFilterFlags.PromptUser);

						if(strText.Length > 0)
						{
							if((dxCode.Type == TmaxCodeTypes.Boolean) || (dxCode.Type == TmaxCodeTypes.PickList))
								tmaxParameters.Add(TmaxCommandParameters.SetFilterText, strText);
						}

					}
					else
					{
						//	We have to have text for these commands
						if(strText.Length > 0)
						{
							//	Add the text parameter
							tmaxParameters.Add(TmaxCommandParameters.SetFilterText, strText);
								
							if(eCommand == CodesPaneCommands.FilterExclude)
								lFlags = (long)(TmaxSetFilterFlags.Exclude);
						}
						else
						{
							MessageBox.Show("No text is available for this command");
							tmaxCode = null; // Cancel the operation
						}
					
					}
						
				}// if((dxCode = (CDxCode)(m_ctrlPropGrid.GetSelection())) != null)
				else
				{
					m_tmaxEventSource.FireDiagnostic(this, "OnCmdFilter", eCommand.ToString() + " - no code");
				}
				
				//	Should we fire the command?
				if(tmaxCode != null)
				{
					//	Do we need to add an item for the actual pick list selection?
					if(tmaxCode.CaseCode.IsMultiLevel == true)
					{
						if(dxCode.PickList != null)
							tmaxCode.SubItems.Add(new CTmaxItem(dxCode.PickList));
					}
					
					tmaxParameters.Add(TmaxCommandParameters.SetFilterFlags, lFlags);
					
					//	Fire the command event
					FireCommand(TmaxCommands.SetFilter, tmaxCode, tmaxParameters);
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdFilter", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_FILTER_EX, eCommand), Ex);
			}
			
		}// private void OnCmdFilter(bool bSelection, bool bExclude)
		
		/// <summary>This method handles the event fired when the user clicks on Refresh Filter from the context menu</summary>
		private void OnCmdRefreshFiltered()
		{
			FireSetFilterCommand(true);
			
		}// private void OnCmdRefreshFiltered()
		
		/// <summary>This method handles the event fired when the user clicks on Set Filter from the context menu</summary>
		private void OnCmdSetFilter()
		{
/*
			CFPropGrid wndGrid = new CFPropGrid();
			
			wndGrid.Database = m_tmaxDatabase;
			m_tmaxEventSource.Attach(wndGrid.EventSource);
			
			if(m_dxGridCodes != null)
				wndGrid.Fill(m_dxGridCodes);
				
			wndGrid.ShowDialog();
*/			
			FireSetFilterCommand(false);

		}// private void OnCmdSetFilter()
		
		/// <summary>Event handler for the Navigate commands</summary>
		/// <param name="eCommand">The navigation command being processed</param>
		private void OnCmdNavigate(CodesPaneCommands eCommand)
		{
			int iIndex = -1;
			
			try
			{
				//	Process any pending changes before moving to a new record
				m_ctrlPropGrid.EndUserUpdate(false);
				
				//	Make sure the navigation information is up to date
				GetNavigatorIndex();
				
				//	Calculate the new index
				if(m_iNavigatorTotal > 0)
				{
					switch(eCommand)
					{
						case CodesPaneCommands.First:
						
							iIndex = 0;
							break;
							
						case CodesPaneCommands.Last:
						
							iIndex = m_iNavigatorTotal - 1;
							break;
							
						case CodesPaneCommands.Next:
						
							iIndex = m_iNavigatorIndex + 1;
							break;

						case CodesPaneCommands.Previous:
						
							iIndex = m_iNavigatorIndex - 1;
							break;
					
					}// switch(eCommand)
					
					if((iIndex >= 0) && (iIndex < m_iNavigatorTotal))
						FireNavigate(iIndex);
				
				}// if(m_iNavigatorTotal > 0)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdNavigate", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_NAVIGATE_EX, eCommand), Ex);
			}
			
		}// private void OnCmdNavigate(CodesPaneCommands eCommand)
		
		/// <summary>Event handler for the ToggleUseFilter commands</summary>
		private void OnCmdToggleUseFiltered()
		{
			try
			{
				//	Toggle the current flag
				SetUseFiltered(!m_bUseFiltered);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnCmdToggleUseFiltered", Ex);
			}
			
		}// private void OnCmdToggleUseFiltered()
		
		/// <summary>Called to get another code of the selected type</summary>
		/// <param name="bSilent">true to suppress beep if no code available</param>
		/// <returns>A new code that matches the selected type</returns>
		private CDxCode GetCmdAddAnotherCode(bool bSilent)
		{
			CDxCode dxCode = null;
			CDxCode dxSelection = null;
			
			if(m_dxOwner == null) return null;
			if(m_dxCodes == null) return null;
			if(this.CaseCodes == null) return null;
			if(this.CaseCodes.Count == 0) return null;
			if(m_dxGridCodes == null) return null;
			if(m_dxGridCodes.Count == 0) return null;
			
			try
			{
				//	Get the current selection
				dxSelection = (CDxCode)(m_ctrlPropGrid.GetSelection(true));

				if((dxSelection != null) && (dxSelection.CaseCode.AllowMultiple == true))
				{
					dxCode = new CDxCode(dxSelection.CaseCode);
					dxCode.Collection = m_dxGridCodes;
					dxCode.SetValue("");
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetCmdAddAnotherCode", Ex);
			}
			
			//	Should we sound an alert?
			if((dxCode == null) && (bSilent == false))
				FTI.Shared.Win32.User.MessageBeep(0);
				
			return dxCode;
			
		}// private CDxCode GetCmdAddAnotherCode()

		/// <summary>This method handles events fired by the toolbar manager after displaying the popup menu</summary>
		/// <param name="sender">The toolbar manager firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnUltraAfterCloseup(object sender, Infragistics.Win.UltraWinToolbars.ToolDropdownEventArgs e)
		{
			CodesPaneCommands eCommand = CodesPaneCommands.Invalid;

			//	Check each tool in the manager's collection
			foreach(ToolBase O in m_ultraToolbarManager.Tools)
			{
				//	Get the command for this tool
				if((eCommand = GetCommand(O.Key)) != CodesPaneCommands.Invalid)
				{
					//	Uninstall the shortcut if required
					if(GetCommandShortcut(eCommand) != Shortcut.None)
						O.SharedProps.Shortcut = Shortcut.None;
				}

			}// foreach(ToolBase O in m_ultraToolbarManager.Tools)

		}// private void OnUltraAfterCloseup(object sender, Infragistics.Win.UltraWinToolbars.ToolDropdownEventArgs e)

		/// <summary>This method handles events fired by the toolbar manager before displaying the popup menu</summary>
		/// <param name="sender">The toolbar manager firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnUltraBeforeDropDown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventArgs e)
		{
			SetToolStates(true);
		}
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
			CodesPaneCommands eCommand = CodesPaneCommands.Invalid;
			
			//	Don't bother if ignoring events
			if(m_bIgnoreUltraEvents == true) return;
			
			//	Get the command
			if(e.Tool != null && e.Tool.Key != null)
				eCommand = GetCommand(e.Tool.Key);
				
			if(eCommand != CodesPaneCommands.Invalid)
				OnCommand(eCommand);
		
		}// OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)

		/// <summary>Called to set the UseFiltered flag</summary>
		/// <param name="bUseFiltered">True to use the filtered tree for navigation</param>
		private void SetUseFiltered(bool bUseFiltered)
		{
			m_bUseFiltered = bUseFiltered;
			
			//	Do we have an active selection?
			if(m_dxOwner != null)
			{
				FireActivate(m_dxOwner, true);
			}
			
			//	Make sure the navigation bar is up to date
			SetToolStates(false);
		
		}// private void SetUseFiltered(bool bUseFiltered)
		
		/// <summary>This method is called to enable/disable the tools in the manager's collection</summary>
		/// <param name="bShortcuts">true to assign shortcuts</param>
		private void SetToolStates(bool bShortcuts)
		{
			CodesPaneCommands	eCommand;
			int					iSelections = 0;
			Shortcut			eShortcut = Shortcut.None;
			CDxCode				dxAddAnother = null;
			
			Debug.Assert(m_ultraToolbarManager != null);
			Debug.Assert(m_ultraToolbarManager.Tools != null);
			if((m_ultraToolbarManager == null) ||( m_ultraToolbarManager.Tools == null)) return;
			
			//	Get the current number of row selections
			if((m_ctrlPropGrid != null) && (m_ctrlPropGrid.IsDisposed == false))
				iSelections = m_ctrlPropGrid.GetSelectedCount();
			
			//	Make sure we have the correct navigator information
			GetNavigatorIndex();
			
			//	Check each tool in the manager's collection
			foreach(ToolBase O in m_ultraToolbarManager.Tools)
			{
				if(O.Key == null) continue;
				
				try
				{
					if((eCommand = GetCommand(O.Key)) == CodesPaneCommands.Invalid)
					{
						//	This could be the total records label
						if(O.Key == "Total")
						{
							if(m_iNavigatorTotal > 0)
								O.SharedProps.Caption = ("of " + m_iNavigatorTotal.ToString());
							else
								O.SharedProps.Caption = "";
						}
						
						//	Get the next tool
						continue;
					
					}// if((eCommand = GetCommand(O.Key)) == CodesPaneCommands.Invalid)
					
					//	Should the command be enabled?
					O.SharedProps.Enabled = GetCommandEnabled(eCommand, iSelections);

					//	Should we assign a shortcut to this command?
					if(bShortcuts == true)
					{
						if((eShortcut = GetCommandShortcut(eCommand)) != Shortcut.None)
							O.SharedProps.Shortcut = eShortcut;
					}

					switch(eCommand)
					{
						case CodesPaneCommands.AddAnother:
								
							if((dxAddAnother = GetCmdAddAnotherCode(true)) != null)
								O.SharedProps.Caption = "Add Another " + dxAddAnother.Name;
							else
								O.SharedProps.Caption = "Add Another";
							break;
									
						case CodesPaneCommands.ToggleUseFiltered:
								
							m_bIgnoreUltraEvents = true;
							((StateButtonTool)O).Checked = m_bUseFiltered;
							m_bIgnoreUltraEvents = false;
				
							if(m_bUseFiltered == true)
								O.SharedProps.AppearancesSmall.Appearance.Image = 9;
							else
								O.SharedProps.AppearancesSmall.Appearance.Image = 10;
							break;
									
						case CodesPaneCommands.ShowUnassigned:
								
							m_bIgnoreUltraEvents = true;
							((StateButtonTool)O).Checked = m_bShowUnassigned;
							m_bIgnoreUltraEvents = false;
				
							if(m_bShowUnassigned == true)
								O.SharedProps.AppearancesSmall.Appearance.Image = 0;
							else
								O.SharedProps.AppearancesSmall.Appearance.Image = 1;
							break;
									
						case CodesPaneCommands.GoTo:
						
							if(m_iNavigatorIndex >= 0)
								((TextBoxTool)O).Text = (m_iNavigatorIndex + 1).ToString();
							else
								((TextBoxTool)O).Text = "";
							break;
							
						default:
						
							break;
							
					}// switch(eCommand)
						
				}
				catch
				{
				}

			}// foreach(ToolBase ultraTool in m_ultraToolbarManager.Tools)
				
			//	Set the status bar text
			SetStatusText();
			
		}// private void SetToolStates()

		/// <summary>This function is called to set the status bar text</summary>
		private void SetStatusText()
		{
			string strText = "";
			
			if(m_ctrlStatusBar == null) return;
			if(m_ctrlStatusBar.IsDisposed == true) return;
			
			//	Do we have media loaded?
			if(m_dxOwner != null)
			{
				strText = m_dxOwner.GetText(TmaxTextModes.Barcode);
			}
			
			m_ctrlStatusBar.Text = strText;
			
		}// private void SetStatusText()

		/// <summary>This method will put the focus on the Go To barcode text box in the main menu</summary>
		private void SetFocusGoTo()
		{
			try
			{
				((TextBoxTool)(m_ultraToolbarManager.Toolbars["MainToolbar"].Tools[CodesPaneCommands.GoTo.ToString()])).IsActiveTool = true;
				((TextBoxTool)(m_ultraToolbarManager.Toolbars["MainToolbar"].Tools[CodesPaneCommands.GoTo.ToString()])).IsInEditMode = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "ActivateGoTo", "Ex: " + Ex.ToString());
			}

		}// private void SetFocusGoTo()

		/// <summary>This method is called to get the shortcut key for the specified command</summary>
		/// <param name="eCommand">The tree pane command enumeration</param>
		/// <returns>The shortcut key if there is one</returns>
		private Shortcut GetCommandShortcut(CodesPaneCommands eCommand)
		{
			switch(eCommand)
			{
				case CodesPaneCommands.AddAnother:

					return Shortcut.CtrlA;

				case CodesPaneCommands.Delete:

					return Shortcut.ShiftDel;

				default:

					return Shortcut.None;

			}// switch(eCommand)

		}// protected virtual Shortcut GetCommandShortcut(CodesPaneCommands eCommand)

		/// <summary>This method is called to get the index of the current record in the navigator collection</summary>
		private void GetNavigatorIndex()
		{
			//	Reset the current values
			m_iNavigatorTotal = 0;
			m_iNavigatorIndex = -1;
			
			//	Are we using the filtered collection?
			if(m_bUseFiltered == true)
			{
				//	Do we have a collection to use?
				if((m_dxFiltered != null) && (m_dxFiltered.Count > 0))
				{
					m_iNavigatorTotal = m_dxFiltered.Count;
					
					//	Do we have an active record?
					if(m_dxOwner != null)
					{
						m_iNavigatorIndex = m_dxFiltered.IndexOf(m_dxOwner);
					}
				
				}
				
			}
			else
			{
				//	Do we have a collection to use?
				if((m_tmaxDatabase != null) && (m_tmaxDatabase.Primaries != null) && (m_tmaxDatabase.Primaries.Count > 0))
				{
					m_iNavigatorTotal = m_tmaxDatabase.Primaries.Count;
					
					//	Do we have an active record?
					if(m_dxOwner != null)
					{
						m_iNavigatorIndex = m_tmaxDatabase.Primaries.IndexOf(m_dxOwner);
					}
				
				}
				
			}			
			
		}// private void GetNavigatorIndex()

		/// <summary>This function handles events fired by the toolbar manager when the user releases a key in one of the tools</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event parameters</param>
		private void OnUltraToolKeyDown(object sender, Infragistics.Win.UltraWinToolbars.ToolKeyEventArgs e)
		{
			if((e.Tool.Key == CodesPaneCommands.GoTo.ToString()) && (e.KeyCode == Keys.Enter))
			{
				//	Mark the event as handled
				e.Handled = true;

				OnCmdGoTo();
			}
		
		}// private void OnUltraToolKeyDown(object sender, Infragistics.Win.UltraWinToolbars.ToolKeyEventArgs e)

		/// <summary>This method is called when a case code gets updated</summary>
		/// <param name="tmaxCaseCode">The case code that has been updated</param>
		private void OnUpdated(CTmaxCaseCode tmaxCaseCode)
		{
			// Just in case ...
			if(tmaxCaseCode == null) return;
			
			//	Force a refill when the user closes the case options form
			m_bRefill = true;
			
		}// private void OnUpdated(CTmaxCaseCode tmaxCaseCode)

		/// <summary>This method is called when a pick list item gets updated</summary>
		/// <param name="tmaxPickItem">The item that has been updated</param>
		private void OnUpdated(CTmaxPickItem tmaxPickItem)
		{
			// Just in case ...
			if(tmaxPickItem == null) return;
			
			//	Force a refill when the user closes the case options form
			m_bRefill = true;
			
		}// private void OnUpdated(CTmaxPickItem tmaxPickItem)

		/// <summary>This method is called to get the parameters required for the filter commands</summary>
		/// <param name="strText">The text to be provided with the command</param>
		/// <returns>The code to be provided with the command</returns>
		private CDxCode GetCmdFilterCode(ref string strText)
		{
			CDxCode			dxCode = null;
			CTmaxPickItem	pickList = null;
			
			//	Get the selected code and its current value?
			if((dxCode = (CDxCode)(m_ctrlPropGrid.GetSelection(ref strText))) != null)
			{
				//	Must be a text-based code to use the selection
				if((dxCode.Type == TmaxCodeTypes.Text) || (dxCode.Type == TmaxCodeTypes.Memo))
				{
					//	Was the user editing this code before the context menu got popped up?
					if(ReferenceEquals(dxCode, m_dxEditorCode) == true)
					{
						//	Was any text selected?
						if(m_strSelectedText.Length > 0)
							strText = m_strSelectedText;
					}
				
				}
				else if(dxCode.Type == TmaxCodeTypes.PickList)
				{
					//	Make sure we can find the text in the pick list
					if((dxCode.CaseCode != null) && (dxCode.CaseCode.PickList != null))
					{
						if(strText.Length > 0)
						{
							if(dxCode.IsMultiLevel == true)
							{
								//	Use the pick list attached to the code
								pickList = dxCode.PickList;
							}
							else
							{
								//	Use the pick list bound to the case code
								pickList = dxCode.CaseCode.PickList;
							}

							//	Cancel the operation if unable to find the selected text
							if((pickList == null) || (pickList.FindChild(strText) == null))
								dxCode = null;
						
						}// if(strText.Length > 0)
						
					}
					else
					{
						dxCode = null;
					}
				}
				else if(dxCode.Type == TmaxCodeTypes.Boolean)
				{
					//	Special case
					if(String.Compare(strText, "unassigned", true) == 0)
						strText = "";
						
					if(strText.Length > 0)
					{
						if(dxCode.IsValid(strText) == false)
							dxCode = null;
					}
				
				}
				else
				{
					dxCode = null; // Can't be filtered
				}
			
			}// if((dxCode = m_ctrlPropGrid.GetSelection(ref strText)) != null)
			
			return dxCode;
		
		}// private CDxCode GetCmdFilterCode(ref string strText)
		
		/// <summary>This function is called when the case codes have been modified in some way</summary>
		private void OnCaseCodesModified()
		{
			//	Is the pane visible
			if(m_bPaneVisible == true)
			{
				if(m_dxOwner != null)
				{
					//	Force a refill
					m_dxActivate = m_dxOwner;
					m_dxOwner = null;
					SetOwner(m_dxActivate);
				}

			}
			else
			{
				//	Force an update when the pane goes active
				if(ReferenceEquals(m_dxOwner, m_dxActivate) == true)
				{
					m_dxOwner = null;
				}
					
			}// if(m_bActive == true)

		}// private void OnCaseCodesModified()

		/// <summary>Handles AddKeyPress events fired by the child grid control</summmary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnGridAddKeyPress(object sender, CTmaxPropGridCtrl.CTmaxPropGridArgs e)
		{
			try
			{
				OnCmdAddAnother(false);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnGridAddKeyPress", Ex);
			}

		}// private void OnGridAddKeyPress(object sender, CTmaxPropGridCtrl.CTmaxPropGridArgs e)

		#endregion Private Methods
		
	}// public class CCodesPane : FTI.Trialmax.Panes.CBasePane

}// namespace FTI.Trialmax.Panes
