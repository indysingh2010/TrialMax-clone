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
using FTI.Shared.Xml;
using FTI.Trialmax.Forms;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinToolbars;

/// <summary>Namespace containing all Trialmax pane classes</summary>
namespace FTI.Trialmax.Panes
{
	/// <summary>Base class used by all panes in the Trialmax application</summary>
	public class CBasePane : System.Windows.Forms.UserControl
	{
		#region Constants
		
		/// <summary>Drag / drop state identifiers</summary>
		protected enum PaneDragStates
		{
			None = 0,
			Records,	//	User is dragging media records
			Source,		//	User is dragging source for registration
		}
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_BASE_PANE_GET_ULTRA_TOOL_EX	= 0;
		protected const int ERROR_BASE_PANE_ADD_OBJECTION_EX	= 1;
		protected const int ERROR_BASE_PANE_RESERVED_2			= 2;
		protected const int ERROR_BASE_PANE_RESERVED_3			= 3;
		protected const int ERROR_BASE_PANE_RESERVED_4			= 4;
		protected const int ERROR_BASE_PANE_RESERVED_5			= 5;
		protected const int ERROR_BASE_PANE_RESERVED_6			= 6;
		protected const int ERROR_BASE_PANE_RESERVED_7			= 7;
		protected const int ERROR_BASE_PANE_RESERVED_8			= 8;
		protected const int ERROR_BASE_PANE_RESERVED_9			= 9;
		protected const int ERROR_BASE_PANE_RESERVED_10			= 10;
		protected const int ERROR_BASE_PANE_RESERVED_11			= 11;
		protected const int ERROR_BASE_PANE_RESERVED_12			= 12;
		protected const int ERROR_BASE_PANE_RESERVED_13			= 13;
		protected const int ERROR_BASE_PANE_RESERVED_14			= 14;
		protected const int ERROR_BASE_PANE_RESERVED_15			= 15;
		
		// Derived classes should start their error numbering with this value
		protected const int ERROR_BASE_PANE_MAX					= 15;
		
		#endregion Constants
		
		#region Protected Members
		
		/// <summary>Local member used to construct error messages</summary>
		protected FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();
		
		/// <summary>Local member bounded to EventSource property</summary>
		protected FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member associated with the PaneId property</summary>
		protected int m_iPaneId = 0;
		
		/// <summary>Local member associated with the PaneVisible property</summary>
		protected bool m_bPaneVisible = false;

		/// <summary>Local member to store a flag to indicate that attempts to delete media have been confirmed</summary>
		protected bool m_bDeleteConfirmed = false;

		/// <summary>Current drag state for the pane</summary>
		protected PaneDragStates m_eDragState = PaneDragStates.None;

		/// <summary>Source files type identifier when the user is dragging new files into the database</summary>
		protected RegSourceTypes m_eSourceRegisterType = RegSourceTypes.AllFiles;

		/// <summary>Media type identifier when the user is dragging new files and/or media</summary>
		protected FTI.Shared.Trialmax.TmaxMediaTypes m_eSourceMediaType = TmaxMediaTypes.Unknown;

		/// <summary>Media type identifier when the user is dragging new files and/or media</summary>
		protected FTI.Shared.Trialmax.CTmaxItem m_tmaxDragData = null;

		/// <summary>Private member bound to AsyncCommandArgs property</summary>
		protected FTI.Shared.Trialmax.CTmaxCommandArgs m_asyncCommandArgs = null;

		/// <summary>Local member associated with the PaneName property</summary>
		protected string m_strPaneName = "";
		
		/// <summary>Local member associated with the PresentationOptionsFilename property</summary>
		protected string m_strPresentationOptionsFilename = "";
		
		/// <summary>Local member associated with the Clipboard property</summary>
		protected CTmaxItems m_tmaxClipboard = null;
		
		/// <summary>Local member associated with the ReportManager property</summary>
		protected FTI.Trialmax.Reports.CTmaxReportManager m_tmaxReportManager = null;
		
		/// <summary>Local member associated with the RegOptions property</summary>
		protected CTmaxRegOptions m_tmaxRegOptions = null;
		
		/// <summary>Local member associated with the AppOptions property</summary>
		protected CTmaxManagerOptions m_tmaxAppOptions = null;

		/// <summary>Local member associated with the PresentationOptions property</summary>
		protected CTmaxPresentationOptions m_tmaxPresentationOptions = null;

		/// <summary>Local member associated with the TmaxProductManager property</summary>
		protected FTI.Shared.Trialmax.CTmaxProductManager m_tmaxProductManager = null;
		
		/// <summary>Local member associated with the TmaxRegistry property</summary>
		protected FTI.Shared.Trialmax.CTmaxRegistry m_tmaxRegistry = null;
		
		/// <summary>Local member associated with the TmaxCaseCodes property</summary>
		protected FTI.Shared.Trialmax.CTmaxCaseCodes m_tmaxCaseCodes = null;
		
		/// <summary>Local member associated with the CaseOptions property</summary>
		protected CTmaxCaseOptions m_tmaxCaseOptions = null;
		
		/// <summary>Local member associated with the StationOptions property</summary>
		protected CTmaxStationOptions m_tmaxStationOptions = null;
		
		/// <summary>Local member bound to MediaTypes property</summary>
		protected CTmaxMediaTypes m_tmaxMediaTypes = null;
		
		/// <summary>Local member bound to SourceTypes property</summary>
		protected CTmaxSourceTypes m_tmaxSourceTypes = null;
		
		///	<summary>Local member bound to Database property</summary>
		protected CTmaxCaseDatabase m_tmaxDatabase = null;
		
		///	<summary>Local member bound to Filtered property</summary>
		protected FTI.Trialmax.Database.CDxPrimaries m_dxFiltered = null;
		
		///	<summary>Flag to inhibit processing of toolbar manager events</summary>
		protected bool m_bIgnoreUltraEvents = false;
		
		#endregion Protected Members

		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CBasePane()
		{
			m_tmaxEventSource.Name = m_strPaneName;
			
			//	Populate the error builder's format string collection
			SetErrorStrings();
			
			//	NOTE:	It is up to the derived class to call InitializeComponent()
			//			in it's constructor. This ensures that the derived constructor
			//			gets called before InitializeComponent()
		}
		
		/// <summary>This method is called by the application to initialize the pane</summary>
		/// <returns>true if successful</returns>
		/// <remarks>Derived classes should override for custom runtime initialization</remarks>
		public virtual bool Initialize(CXmlIni xmlINI)
		{
			return true;
		}
		
		/// <summary>This method is called by the application when it is about to terminate</summary>
		/// <remarks>Derived classes should override for custom shutdown</remarks>
		public virtual void Terminate(CXmlIni xmlINI)
		{
		}
		
		/// <summary>This method is called by the application to open the specified item</summary>
		/// <param name="tmaxItem">The item to be opened</param>
		/// <param name="ePane">The pane making the request</param>
		/// <returns>true if successful</returns>
		public virtual bool Open(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
			return true;
		}
		
		/// <summary>This method is called by the application to notify the panes to refresh their text</summary>
		public virtual void RefreshText()
		{
		}
		
		/// <summary>This method is called by the application to activate the specified item</summary>
		/// <param name="tmaxItem">The item to be activated</param>
		/// <param name="ePane">The pane requesting activation</param>
		/// <returns>true if successful</returns>
		public virtual bool Activate(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
			return true;
		}

		/// <summary>This method is called by the application when this pane is activated</summary>
		public virtual void OnPaneActivate()
		{
		}

		/// <summary>This method is called by the application when this pane is deactivated</summary>
		public virtual void OnPaneDeactivate()
		{
		}

		/// <summary>This method handles all Hotkey notifications from the application</summary>
		/// <param name="eHotkey">The enumerated hotkey</param>
		/// <returns>true if processed by the pane</returns>
		public virtual bool OnHotkey(TmaxHotkeys eHotkey)
		{
			return false;
		}
		
		/// <summary>This method handles notifications from the application when it is about to load a new screen layout</summary>
		public virtual void OnBeforeLoadLayout()
		{
		
		}// public virtual void OnBeforeLoadLayout()
		
		/// <summary>This method handles notifications from the application when it has finished loading a new screen layout</summary>
		public virtual void OnAfterLoadLayout()
		{
		
		}// public virtual void OnAfterLoadLayout()
		
		/// <summary>This method handles all KeyDown notifications from the application</summary>
		/// <param name="eKey">The key being pressed</param>
		///	<param name="eModifiers">The current control/shift/alt key states</param>
		/// <returns>true if processed</returns>
		public virtual bool OnKeyDown( Keys eKey, Keys eModifiers)
		{
			return false;
		}
		
		/// <summary>This method is called by the application to when the item gets deleted</summary>
		/// <param name="tmaxItem">The item that has been deleted</param>
		public virtual void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
		}
		
		/// <summary>
		/// This method is called by the application when it modifies the contents of the clipboard
		/// </summary>
		public virtual void OnClipboardUpdated()
		{
		}
		
		/// <summary>This method is called by the application to when the item's child collection has been reordered</summary>
		/// <param name="tmaxItem">The item that owns the child collection</param>
		public virtual void OnReordered(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{
		}
		
		/// <summary>This method is called by the application when new media gets registered</summary>
		/// <param name="tmaxFolder">The source folder containing the new media</param>
		/// <param name="ePane">The pane that accepted the registration request</param>
		public virtual void OnRegistered(CTmaxSourceFolder tmaxFolder, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
			//	This method should be overridded by derived classes
		}
		
		/// <summary>This method is called by the application when it adds new media to the database</summary>
		/// <param name="tmaxParent">TrialMax event item that identifies the parent record</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		public virtual void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		{
		}
		
		/// <summary>This method is called by the application after the database processes a Move command</summary>
		/// <param name="tmaxRepositioned">Event item that identifies the records that have been moved</param>
		public virtual void OnMoved(CTmaxItem tmaxMoved)
		{
		}
		
		/// <summary>This function is called when the ApplicationOptions property changes</summary>
		public virtual void OnApplicationActivated()
		{
		}
		
		/// <summary>This method is called by the application to when the item has been updated by the user</summary>
		/// <param name="tmaxItem">The item that has been updated</param>
		public virtual void OnUpdated(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{
		}
		
		/// <summary>This method is called by the application when the target binder changes</summary>
		/// <param name="dxOldTarget">The previous target binder</param>
		/// <param name="dxNewTarget">The new target binder</param>
		public virtual void OnTargetBinderChanged(CDxBinderEntry dxOldTarget, CDxBinderEntry dxNewTarget)
		{
		}
		
		/// <summary>This method is called by the application when multiple records have been updated in an operation</summary>
		/// <param name="tmaxItems">The items that have been updated</param>
		public virtual void OnUpdated(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			if(tmaxItems != null)
			{
				foreach(CTmaxItem O in tmaxItems)
					OnUpdated(O);
			}
		
		}
		
		/// <summary>This method is called by the application to when the record has been split into two or more records</summary>
		/// <param name="tmaxItem">The item that has been split</param>
		public virtual void OnEdited(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Trialmax.Database.CTmaxDatabaseResults tmaxResults)
		{
		}
		
		/// <summary>This method is called by the application when multiple records have been split in an operation</summary>
		/// <param name="tmaxItems">The items that have been split</param>
		public virtual void OnEdited(FTI.Shared.Trialmax.CTmaxItems tmaxItems, FTI.Trialmax.Database.CTmaxDatabaseResults tmaxResults)
		{
			if(tmaxItems != null)
			{
				foreach(CTmaxItem O in tmaxItems)
					OnEdited(O, tmaxResults);
			}
		
		}
		
		/// <summary>This method is called by the application when the codes assigned to a record have changed</summary>
		/// <param name="tmaxItems">The item that identifies the modified record</param>
		/// <param name="eAction">The action taken on the record's codes collection</param>
		public virtual void OnSetCodes(FTI.Shared.Trialmax.CTmaxItem tmaxItem, TmaxCodeActions eAction)
		{
		}
		
		/// <summary>This method is called by the application when the codes assigned to multiple records have changed</summary>
		/// <param name="tmaxItems">The items that have been modified</param>
		/// <param name="eAction">The action taken on the record's codes collection</param>
		public virtual void OnSetCodes(FTI.Shared.Trialmax.CTmaxItems tmaxItems, TmaxCodeActions eAction)
		{
			if(tmaxItems != null)
			{
				foreach(CTmaxItem O in tmaxItems)
					OnSetCodes(O, eAction);
			}
		
		}
		
		/// <summary>This method is called by the application when a bulk update operation has been performed</summary>
		/// <param name="tmaxItems">The primary owners of the codes modified by the operation</param>
		/// <param name="eAction">The action taken on the record's codes collection</param>
		public virtual void OnBulkUpdate(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
		}
		
		/// <summary>This method is called by the application when the records in the filtered collection have been reordered</summary>
		/// <param name="bFiltered">True if the filtered collection has been reordered</param>
		public virtual void OnSetPrimariesOrder(bool bFiltered)
		{
		}

		/// <summary>This method is called by the application when the user sets the active deponent</summary>
		/// <param name="tmaxItem">The item that identifies the deponent</param>
		/// <param name="ePane">The pane requesting activation</param>
		public virtual void OnSetDeposition(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
		}

		/// <summary>This method is called by the application to get a list of items that represent the current selections for a Print command</summary>
		/// <returns>The items that represent the current selections</returns>
		public virtual CTmaxItems GetCmdPrintItems()
		{
			return null;
		
		}// public virtual CTmaxItems GetCmdPrintItems()
		
		/// <summary>This method is called by the application to get a list of items that represent the current selections for a Find command</summary>
		/// <returns>The items that represent the current selections</returns>
		public virtual CTmaxItems GetCmdFindItems()
		{
			return null;
		
		}// public virtual CTmaxItems GetCmdFindItems()
		
		/// <summary>This method is called by the application when a new search result has been activated</summary>
		/// <param name="tmaxResult">The search result to be activated</param>
		public virtual void OnActivateResult(FTI.Shared.Trialmax.CTmaxSearchResult tmaxResult)
		{
		}
		
		/// <summary>This function is called when the the user is opening the Presentation Options form</summary>
		public virtual void OnBeforeSetPresentationOptions()
		{
		}
		
		/// <summary>This function is called when the the user has closed the Presentation Options form</summary>
		/// <param name="bCancelled">true if the user cancelled the operation</param>
		public virtual void OnAfterSetPresentationOptions(bool bCancelled)
		{
		}

		/// <summary>This function is called when the the user is opening the Application Options form</summary>
		public virtual void OnBeforeSetApplicationOptions()
		{
		}
		
		/// <summary>This function is called when the the user has closed the Application Options form</summary>
		/// <param name="bCancelled">true if the user cancelled the operation</param>
		public virtual void OnAfterSetApplicationOptions(bool bCancelled)
		{
		}

		/// <summary>This function is called when the the user is opening the Case Options form</summary>
		public virtual void OnBeforeSetCaseOptions()
		{
		}
		
		/// <summary>This function is called when the the user has closed the Case Options form</summary>
		/// <param name="bCancelled">true if the user cancelled the operation</param>
		public virtual void OnAfterSetCaseOptions(bool bCancelled)
		{
		}

		/// <summary>This method is called by the application when case codes Hidden property has been changed by the user</summary>
		/// <param name="tmaxItems">The items that identify the case codes (all codes if tmaxItems == null)</param>
		public virtual void OnHideCaseCodes(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
		}
		
		/// <summary>This function is called when the the application collection of case codes and pick lists is refreshed</summary>
		public virtual void OnRefreshCodes()
		{
		}

		/// <summary>This method is called by the application when objections are added to the database</summary>
		/// <param name="tmaxParent">TrialMax event item that identifies the parent record</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		public virtual void OnObjectionsAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		{
		}

		/// <summary>This method is called by the application when objections have been updated in an operation</summary>
		/// <param name="tmaxItems">The objections that have been updated</param>
		public virtual void OnObjectionsUpdated(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
		}

		/// <summary>This method is called by the application when objections have been deleted</summary>
		/// <param name="tmaxItems">The objections that have been deleted</param>
		public virtual void OnObjectionsDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
		}

		/// <summary>This method is called by the application when objections has been selected by the user</summary>
		/// <param name="tmaxItem">The objection that has been selected</param>
		/// <param name="ePane">The pane that fired the selection event</param>
		public virtual void OnObjectionSelected(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
		}

		/// <summary>This function is called when the value of the objections filter has changed</summary>
		protected virtual void OnObjectionsFilterChanged()
		{
		}

		/// <summary>This method is called by the application when a pane wants to move a record collection navigator</summary>
		/// <param name="ePane">The pane requesting the move</param>
		/// <param name="tmaxItems">The items passed with the event</param>
		/// <param name="tmaxParameters">The parameters passed with the event</param>
		public virtual void OnNavigate(TmaxAppPanes ePane, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)
		{
		}

		/// <summary>This method is called by the application when a pane wants to update a navigator's position</summary>
		/// <param name="ePane">The pane firing the notification</param>
		/// <param name="tmaxItems">The items passed with the event</param>
		/// <param name="tmaxParameters">The parameters passed with the event</param>
		public virtual void OnNavigatorChanged(TmaxAppPanes ePane, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)
		{
		}

		/// <summary>This method is called by the application prior to closing the active database</summary>
		/// <returns>True if OK to close the database</returns>
		public virtual bool CanCloseDatabase()
		{
			return true;
		}

		/// <summary>This method is called by the application to notify the pane that the user is dragging new source media</summary>
		/// <param name="eSourceType">Enumerated source type identifier</param>
		public virtual void OnStartSourceDrag(RegSourceTypes eSourceType)
		{
			CTmaxMediaType	tmaxMediaType = null;
			CTmaxSourceType	tmaxSourceType = null;
			
			// Clear the existing values
			ResetDragState();

			//	Set the new values
			m_eSourceRegisterType = eSourceType;
			
			//	Translate the registration source type to a valid media type
			m_eSourceMediaType = TmaxMediaTypes.Unknown;
			
			if((m_tmaxMediaTypes != null) && (m_tmaxSourceTypes != null))
			{
				if((tmaxSourceType = m_tmaxSourceTypes.Find(m_eSourceRegisterType)) != null)
				{
					if((tmaxMediaType = m_tmaxMediaTypes.Find(tmaxSourceType.MediaType)) != null)
					{
						m_eSourceMediaType = tmaxMediaType.PrimaryType;
					}
				
				}
				
			}// if((m_tmaxMediaTypes != null) && (m_tmaxSourceTypes != null))
			
			m_eDragState = PaneDragStates.Source;

		}// public virtual void OnStartSourceDrag(RegSourceTypes eSourceType)

		/// <summary>This method is called to notify the pane that the user has stopped dragging source files</summary>
		/// <param name="eSourceType">Enumerated source type identifier</param>
		public virtual void OnCompleteSourceDrag()
		{
			ResetDragState();
		}

		/// <summary>This method is called by the application to notify the pane that the user is dragging database records</summary>
		/// <param name="tmaxItems">TrialMax event items being dragged</param>
		public virtual void OnStartDragRecords(CTmaxItem tmaxItem)
		{
			//	Clear the existing values
			ResetDragState();
			
			//	Did the caller provide any items?
			if((tmaxItem != null) && (tmaxItem.SourceItems != null) && (tmaxItem.SourceItems.Count > 0))
			{
				m_tmaxDragData   = tmaxItem;
				m_eDragState     = PaneDragStates.Records;
			}
			
			/*
			if((tmaxItem != null) && (tmaxItem.SourceItems != null))
				FireDiagnostic("OnStartDragRecords", "Start Dragging " + tmaxItem.SourceItems.Count.ToString() + " media");
			else
				FireDiagnostic("OnStartDragRecords", "Start Media Null Items");
			*/
				
		}// public virtual void OnStartDragRecords(CTmaxItems tmaxItems)

		/// <summary>This method is called to notify the pane that the user has stopped dragging database records</summary>
		/// <param name="tmaxItems">TrialMax event items being dragged</param>
		public virtual void OnCompleteDataDrag()
		{
			ResetDragState();
		}

		/// <summary>This method is called to acknowledge processing of an asynchronous command</summary>
		/// <param name="Args">The asyncronous argument provided for the event</param>
		/// <param name="bSuccessful">true if successful</param>
		public virtual void AcknowledgeAsyncCommand(CTmaxCommandArgs Args, bool bSuccessful)
		{
			//	Clear the asynchronous argument if they match.
			//
			//	NOTE:	We have to check for a match because they may have changed
			//			since the operation was asynchronous
			if((Args != null) && (ReferenceEquals(Args, m_asyncCommandArgs) == true))
			{
				m_asyncCommandArgs = null;
			}
		
		}// public virtual void AcknowledgeAsyncCommand(CTmaxCommandArgs Args, bool bSuccessful)

		/// <summary>This method is called to reset the local members related to dragging operations</summary>
		public virtual void ResetDragState()
		{
			m_eSourceRegisterType = RegSourceTypes.NoSource;
			m_eSourceMediaType = TmaxMediaTypes.Unknown;
			m_tmaxDragData = null;
			m_eDragState = PaneDragStates.None;

		}// public virtual void ResetDragState()

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Designer uses this function to initialize child controls</summary>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		protected virtual void InitializeComponent()
		{
			// 
			// CBasePane
			// 
			this.AllowDrop = true;
			this.Name = "CBasePane";
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
			this.DragLeave += new System.EventHandler(this.OnDragLeave);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
			this.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.OnQueryContinueDrag);
			this.DragOver += new System.Windows.Forms.DragEventHandler(this.OnDragOver);

		}
		
		/// <summary>Clean up all resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
		
		/// <summary>This function is called when the pane's window gets created</summary>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		protected virtual void OnCreatePane()
		{
			//	Make sure the children are properly sized and positioned
			RecalcLayout();
		}
		
		/// <summary>This function is called the when the pane's window handle is destroyed</summary>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		protected virtual void OnDestroyPane()
		{
		}		
		
		/// <summary>
		/// This function is called to resize and reposition the panes child controls
		/// </summary>
		protected virtual void RecalcLayout()
		{
		}// RecalcLayout()

		/// <summary>This method will handle TmaxCommand events</summary>
		/// <param name="objSender">The object sending the event</param>
		/// <param name="Args">Command event arguments</param>
		protected virtual void OnTmaxCommand(object objSender, CTmaxCommandArgs Args)
		{
			//	Default behavior is to propagate the command but switch the sender
			//	to make is look like it originated with this pane
			if(TmaxCommandEvent != null)
				TmaxCommandEvent(this, Args);			
		}

		/// <summary>This function is called to set the PaneId property</summary>
		/// <param name="iPaneId">The application's pane identifier</param>
		protected virtual void SetPaneId(int iPaneId)
		{
			//	Update the id
			m_iPaneId = iPaneId;

			//	Notify the derived class
			OnPaneIdChanged();

		}// protected virtual void SetPaneId(int iPaneId)

		/// <summary>This function is called when the PaneId property changes</summary>
		protected virtual void OnPaneIdChanged()
		{
		}

		/// <summary>This function is called to set the PaneName property</summary>
		/// <param name="strPaneName">The application's pane name</param>
		protected virtual void SetPaneName(string strPaneName)
		{
			//	Update the name
			m_strPaneName = strPaneName;

			m_tmaxEventSource.Name = m_strPaneName;

			//	Notify the derived class
			OnPaneNameChanged();

		}// protected virtual void SetPaneName(string strPaneName)

		/// <summary>This function is called when the PaneName property changes</summary>
		protected virtual void OnPaneNameChanged()
		{
		}

		/// <summary>This function is called to set the PaneVisible property</summary>
		/// <param name="bVisible">true if the pane is visible on screen</param>
		/// <remarks>This is NOT the same as the base class Visible property</remarks>
		protected virtual void SetPaneVisible(bool bPaneVisible)
		{
			if(m_bPaneVisible != bPaneVisible)
			{
				m_bPaneVisible = bPaneVisible;

				//	Notify the derived class
				OnPaneVisibleChanged();
			}

		}// protected virtual void SetPaneVisible(bool bPaneVisible)

		/// <summary>This function is called when the PaneVisible property changes</summary>
		protected virtual void OnPaneVisibleChanged()
		{
		}

		/// <summary>This function is called to set the TmaxClipboard property</summary>
		/// <param name="tmaxClipboard">The application's clipboard manager</param>
		protected virtual void SetTmaxClipboard(CTmaxItems tmaxClipboard)
		{
			m_tmaxClipboard = tmaxClipboard;

			//	Notify the derived class
			OnTmaxClipboardChanged();

		}// protected virtual void SetTmaxClipboard(CTmaxItems tmaxClipboard)

		/// <summary>This function is called when the TmaxClipboard property changes</summary>
		protected virtual void OnTmaxClipboardChanged()
		{
		}

		/// <summary>This function is called to set the TmaxProductManager property</summary>
		/// <param name="tmaxProductManager">The application's product manager</param>
		protected virtual void SetTmaxProductManager(CTmaxProductManager tmaxProductManager)
		{
			m_tmaxProductManager = tmaxProductManager;

			//	Notify the derived class
			OnTmaxProductManagerChanged();

		}// protected virtual void SetTmaxProductManager(CTmaxProductManager tmaxProductManager)

		/// <summary>This function is called when the TmaxProductManager property changes</summary>
		protected virtual void OnTmaxProductManagerChanged()
		{
		}

		/// <summary>This function is called to set the TmaxRegistry property</summary>
		/// <param name="tmaxRegistry">The application's registry manager</param>
		protected virtual void SetTmaxRegistry(CTmaxRegistry tmaxRegistry)
		{
			m_tmaxRegistry = tmaxRegistry;

			//	Notify the derived class
			OnTmaxRegistryChanged();

		}// protected virtual void SetTmaxRegistry(CTmaxRegistry tmaxRegistry)

		/// <summary>This function is called when the TmaxRegistry property changes</summary>
		protected virtual void OnTmaxRegistryChanged()
		{
		}

		/// <summary>This function is called to set the PresentationOptionsFilename property</summary>
		/// <param name="strPresentationOptionsFilename">The path to the TmaxPresentation application options</param>
		protected virtual void SetPresentationOptionsFilename(string strPresentationOptionsFilename)
		{
			if(String.Compare(m_strPresentationOptionsFilename, strPresentationOptionsFilename, true) != 0)
			{
				m_strPresentationOptionsFilename = strPresentationOptionsFilename;

				//	Notify the derived class
				OnPresentationOptionsFilenameChanged();
			}

		}// protected virtual void SetPresentationOptionsFilename(string strPresentationOptionsFilename)

		/// <summary>This function is called when the PresentationOptionsFilename property changes</summary>
		protected virtual void OnPresentationOptionsFilenameChanged()
		{
		}

		/// <summary>This function is called to set the RegistrationOptions property</summary>
		/// <param name="tmaxRegOptions">The application's registration options</param>
		protected virtual void SetRegistrationOptions(CTmaxRegOptions tmaxRegOptions)
		{
			m_tmaxRegOptions = tmaxRegOptions;

			//	Notify the derived class
			OnRegistrationOptionsChanged();

		}// protected virtual void SetRegistrationOptions(CTmaxRegOptions tmaxRegOptions)

		/// <summary>This function is called when the RegistrationOptions property changes</summary>
		protected virtual void OnRegistrationOptionsChanged()
		{
		}

		/// <summary>This function is called to set the ApplicationOptions property</summary>
		/// <param name="tmaxAppOptions">The application options</param>
		protected virtual void SetApplicationOptions(CTmaxManagerOptions tmaxAppOptions)
		{
			m_tmaxAppOptions = tmaxAppOptions;

			//	Notify the derived class
			OnApplicationOptionsChanged();

		}// protected virtual void SetApplicationOptions(CTmaxManagerOptions tmaxAppOptions)

		/// <summary>This function is called when the ApplicationOptions property changes</summary>
		protected virtual void OnApplicationOptionsChanged()
		{
		}

		/// <summary>This function is called to set the PresentationOptions property</summary>
		/// <param name="tmaxPresentationOptions">The application options</param>
		protected virtual void SetPresentationOptions(CTmaxPresentationOptions tmaxPresentationOptions)
		{
			m_tmaxPresentationOptions = tmaxPresentationOptions;

			//	Notify the derived class
			OnPresentationOptionsChanged();

		}// protected virtual void SetPresentationOptions(CTmaxPresentationOptions tmaxPresentationOptions)

		/// <summary>This function is called when the PresentationOptions property changes</summary>
		protected virtual void OnPresentationOptionsChanged()
		{
		}

		/// <summary>This function is called to set the CaseOptions property</summary>
		/// <param name="tmaxCaseOptions">The application's case options</param>
		protected virtual void SetCaseOptions(CTmaxCaseOptions tmaxCaseOptions)
		{
			m_tmaxCaseOptions = tmaxCaseOptions;

			//	Notify the derived class
			OnCaseOptionsChanged();

		}// protected virtual void SetCaseOptions(CTmaxCaseOptions tmaxCaseOptions)

		/// <summary>This function is called when the CaseOptions property changes</summary>
		protected virtual void OnCaseOptionsChanged()
		{
		}

		/// <summary>This function is called to set the StationOptions property</summary>
		/// <param name="tmaxStationOptions">The active set of station options</param>
		protected virtual void SetStationOptions(CTmaxStationOptions tmaxStationOptions)
		{
			m_tmaxStationOptions = tmaxStationOptions;

			//	Notify the derived class
			OnStationOptionsChanged();

		}// protected virtual void SetStationOptions(CTmaxStationOptions tmaxStationOptions)

		/// <summary>This function is called when the CaseOptions property changes</summary>
		protected virtual void OnStationOptionsChanged()
		{
		}

		/// <summary>This function is called to set the MediaTypes property</summary>
		/// <param name="tmaxMediaTypes">The application's set of media types</param>
		protected virtual void SetMediaTypes(CTmaxMediaTypes tmaxMediaTypes)
		{
			m_tmaxMediaTypes = tmaxMediaTypes;

			//	Notify the derived class
			OnMediaTypesChanged();

		}// protected virtual void SetMediaTypes(CTmaxMediaTypes tmaxMediaTypes)

		/// <summary>This function is called when the MediaTypes property changes</summary>
		protected virtual void OnMediaTypesChanged()
		{
		}

		/// <summary>This function is called to set the CaseCodes property</summary>
		/// <param name="tmaxCaseCodes">The application's collection of case codes</param>
		protected virtual void SetCaseCodes(CTmaxCaseCodes tmaxCaseCodes)
		{
			if(ReferenceEquals(m_tmaxCaseCodes, tmaxCaseCodes) == false)
			{
				m_tmaxCaseCodes = tmaxCaseCodes;

				//	Notify the derived class
				OnCaseCodesChanged();
			}

		}// protected virtual void SetCaseCodes(CTmaxCaseCodes tmaxCaseCodes)

		/// <summary>This function is called when the CaseCodes property changes</summary>
		protected virtual void OnCaseCodesChanged()
		{
		}

		/// <summary>This function is called to set the SourceTypes property</summary>
		/// <param name="tmaxSourceTypes">The application's set of source types</param>
		protected virtual void SetSourceTypes(CTmaxSourceTypes tmaxSourceTypes)
		{
			m_tmaxSourceTypes = tmaxSourceTypes;

			//	Notify the derived class
			OnSourceTypesChanged();

		}// protected virtual void SetSourceTypes(CTmaxSourceTypes tmaxSourceTypes)

		/// <summary>This function is called when the SourceTypes property changes</summary>
		protected virtual void OnSourceTypesChanged()
		{
		}

		/// <summary>This function is called to set the Database property</summary>
		/// <param name="tmaxDatabase">The active case database</param>
		protected virtual void SetDatabase(CTmaxCaseDatabase tmaxDatabase)
		{
			if(ReferenceEquals(m_tmaxDatabase, tmaxDatabase) == false)
			{
				m_tmaxDatabase = tmaxDatabase;

				//	Notify the derived class
				OnDatabaseChanged();
			}

		}// protected virtual void SetDatabase(CTmaxCaseDatabase tmaxDatabase)

		/// <summary>This function is called when the value of the Database property changes</summary>
		protected virtual void OnDatabaseChanged()
		{
			//	Reset the flag to request confirmation before delete
			m_bDeleteConfirmed = false;
			
		}

		/// <summary>This function is called to set the Filtered property</summary>
		/// <param name="dxFiltered">The filtered collection</param>
		protected virtual void SetFiltered(CDxPrimaries dxFiltered)
		{
			m_dxFiltered = dxFiltered;

			//	Notify the derived class
			OnFilteredChanged();

		}// protected virtual void SetFiltered(CDxPrimaries dxFiltered)

		/// <summary>This function is called when the value of the Filtered property changes</summary>
		protected virtual void OnFilteredChanged()
		{
		}

		/// <summary>This function is called to set the ReportManager property</summary>
		/// <param name="tmaxReportManager">The application's report manager</param>
		protected virtual void SetReportManager(FTI.Trialmax.Reports.CTmaxReportManager tmaxReportManager)
		{
			m_tmaxReportManager = tmaxReportManager;

			//	Notify the derived class
			OnReportManagerChanged();

		}// protected virtual void SetReportManager(FTI.Trialmax.Reports.CTmaxReportManager tmaxReportManager)

		/// <summary>This function is called when the value of the ReportManager property changes</summary>
		protected virtual void OnReportManagerChanged()
		{
		}
		
		/// <summary>This function handles all HandleCreated events</summary>
		/// <param name="e">System event parameters - no data</param>
		protected override void OnHandleCreated(System.EventArgs e)
		{
			//	Notify the derived object
			OnCreatePane();
			
			//	Perform the base class processing
			base.OnHandleCreated(e);
		}
		
		/// <summary>This function handles all HandleDestroyed events</summary>
		/// <param name="e">System event parameters - no data</param>
		protected override void OnHandleDestroyed(System.EventArgs e)
		{
			//	Notify the derived object
			OnDestroyPane();
			
			//	Perform the base class processing
			base.OnHandleDestroyed(e);
		}

		/// <summary>This function handles all Resize events</summary>
		/// <param name="e">System event parameters - no data</param>
		protected override void OnResize(System.EventArgs e)
		{
			//	Perform the base class processing first
			base.OnResize(e);
			
			//	Notify the derived object
			RecalcLayout();
		}

		/// <summary>This method fires the command event to either set or refresh the active media filter</summary>
		/// <param name="bRefresh">True to refresh the active filter</param>
		protected virtual void FireSetFilterCommand(bool bRefresh)
		{
			CTmaxParameters tmaxParameters = null;
			
			//	Create the required parameters for the event
			tmaxParameters = new CTmaxParameters();
			
			if(bRefresh == true)
				tmaxParameters.Add(TmaxCommandParameters.SetFilterFlags, (long)0);
			else
				tmaxParameters.Add(TmaxCommandParameters.SetFilterFlags, (long)(TmaxSetFilterFlags.Advanced | TmaxSetFilterFlags.PromptUser));
			
			FireCommand(TmaxCommands.SetFilter, (CTmaxItems)null, tmaxParameters);
			
		}// protected virtual void FireSetFilterCommand(bool bRefresh)

		/// <summary>Called to add an objection to the database</summary>
		/// <param name="tmaxObjection">the objection used to initialize the form</param>
		/// <param name="bMultiple">true if multiple objections allowed</param>
		/// <param name="bRepeat">true to repeat the last argument</param>
		/// <returns>the newly added objection</returns>
		protected virtual CTmaxObjection AddObjection(CTmaxObjection tmaxObjection, bool bMultiple, bool bRepeat)
		{
			CFAddObjections wndAdd = null;
			CDxPrimaries dxDepositions = null;

			try
			{
				wndAdd = new CFAddObjections();

				m_tmaxEventSource.Attach(wndAdd.EventSource);
				wndAdd.TmaxCommandEvent += new FTI.Shared.Trialmax.TmaxCommandHandler(this.OnTmaxCommand);

				wndAdd.PaneId = PaneId;
				wndAdd.AllowMultiple = bMultiple;
				wndAdd.UseLastArgument = bRepeat;
				wndAdd.Objection = tmaxObjection;
				wndAdd.StationOptions = this.StationOptions;

				//	Make sure we have some states and rulings to choose from 
				if(m_tmaxDatabase.ObjectionsDatabase != null)
				{
					wndAdd.States = m_tmaxDatabase.ObjectionsDatabase.OxStates;
					wndAdd.Rulings = m_tmaxDatabase.ObjectionsDatabase.OxRulings;
				}
				else
				{
					MessageBox.Show("No objection states are available", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return null;
				}

				//	Get the registered depositions
				dxDepositions = new CDxPrimaries();
				foreach(CDxPrimary O in m_tmaxDatabase.Primaries)
				{
					if(O.MediaType == TmaxMediaTypes.Deposition)
						dxDepositions.AddList(O);
				}

				if(dxDepositions.Count > 0)
				{
					wndAdd.Depositions = dxDepositions;
				}
				else
				{
					MessageBox.Show("No depositions are available", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return null;
				}

				wndAdd.ShowDialog();
				return wndAdd.Objection;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdAdd", m_tmaxErrorBuilder.Message(ERROR_BASE_PANE_ADD_OBJECTION_EX), Ex);
				return null;
			}

		}// protected virtual CTmaxObjection AddObjection(CTmaxObjection tmaxObjection, bool bMultiple)

		/// <summary>Called to add an objection to the database</summary>
		/// <param name="tmaxObjection">the objection used to initialize the form</param>
		/// <param name="bMultiple">true if multiple objections allowed</param>
		/// <returns>the newly added objection</returns>
		protected virtual CTmaxObjection AddObjection(CTmaxObjection tmaxObjection, bool bMultiple)
		{
			return AddObjection(tmaxObjection, bMultiple, false);
		}

		/// <summary>Called to add an objection to the database</summary>
		/// <param name="dxRecord">The record used as the source of the objection</param>
		/// <param name="bMultiple">true to allow multiple objections</param>
		/// <param name="bRepeat">True to repeat the last argument</param>
		/// <returns>the newly added objection</returns>
		protected virtual CTmaxObjection AddObjection(CDxMediaRecord dxRecord, bool bMultiple, bool bRepeat)
		{
			CTmaxObjection	tmaxObjection = new CTmaxObjection();
			CDxPrimary		dxScript = null;
			CDxTertiary		dxDesignation = null;

			//	Do we have a valid record?
			if(dxRecord != null)
			{
				//	What type of media?
				switch(dxRecord.MediaType)
				{
					case TmaxMediaTypes.Deposition:

						tmaxObjection.ICaseDeposition = (ITmaxDeposition)dxRecord;
						break;

					case TmaxMediaTypes.Segment:

						if((dxRecord.GetParent() != null) && (dxRecord.GetParent().MediaType == TmaxMediaTypes.Deposition))
							tmaxObjection.ICaseDeposition = (ITmaxDeposition)(dxRecord.GetParent());
						break;

					case TmaxMediaTypes.Script:

						dxScript = (CDxPrimary)dxRecord;
						
						//	Find the first video designation in the script
						if((dxScript.Secondaries == null) || (dxScript.Secondaries.Count == 0))
							dxScript.Fill();
							
						foreach(CDxSecondary O in dxScript.Secondaries)
						{
							if((O.GetSource() != null) && (O.GetSource().MediaType == TmaxMediaTypes.Designation))
							{
								dxDesignation = (CDxTertiary)(O.GetSource());
								
								//	Initialize with the first deposition used by the script
								if(dxDesignation.Secondary != null)
									tmaxObjection.ICaseDeposition = (ITmaxDeposition)(dxDesignation.Secondary.Primary);
							}
							
						}// foreach(CDxSecondary O in dxScript.Secondaries)
						break;

					case TmaxMediaTypes.Scene:

						if(((CDxSecondary)dxRecord).GetSource() != null)
						{
							//	Is this a designation?
							if(((CDxSecondary)dxRecord).GetSource().MediaType == TmaxMediaTypes.Designation)
								return AddObjection(((CDxSecondary)dxRecord).GetSource(), false, bRepeat);
						}
						break;

					case TmaxMediaTypes.Designation:

						dxDesignation = (CDxTertiary)dxRecord;
						
						//	Get the parent deposition
						if((dxDesignation.Secondary != null) && (dxDesignation.Secondary.Primary != null))
						{
							tmaxObjection.ICaseDeposition = (ITmaxDeposition)(dxDesignation.Secondary.Primary);
						
							if(dxDesignation.Extent != null)
							{
								tmaxObjection.FirstPL = dxDesignation.Extent.StartPL;
								tmaxObjection.LastPL = dxDesignation.Extent.StopPL;
							}

						}// if((dxDesignation.Secondary != null) && (dxDesignation.Secondary.Primary != null))

						break;

				}// switch(dxRecord.MediaType)

			}// if(dxRecord != null)

			return AddObjection(tmaxObjection, bMultiple, bRepeat);

		}// protected virtual CTmaxObjection AddObjection(CDxMediaRecord dxRecord, bool bMultiple)

		/// <summary>Called to add an objection to the database</summary>
		/// <param name="dxRecord">The record used as the source of the objection</param>
		/// <param name="bMultiple">true to allow multiple objections</param>
		/// <returns>the newly added objection</returns>
		protected virtual CTmaxObjection AddObjection(CDxMediaRecord dxRecord, bool bMultiple)
		{
			return AddObjection(dxRecord, bMultiple, false);
		}

		/// <summary>Called to add an objection to the database</summary>
		/// <param name="bMultiple">true to allow multiple objections</param>
		/// <param name="bRepeat">True to repeat the last argument</param>
		/// <returns>the newly added objection</returns>
		protected virtual CTmaxObjection AddObjection(bool bMultiple, bool bRepeat)
		{
			return AddObjection((CTmaxObjection)null, bMultiple, bRepeat);
		}

		/// <summary>Called to add an objection to the database</summary>
		/// <param name="bMultiple">true to allow multiple objections</param>
		/// <returns>the newly added objection</returns>
		protected virtual CTmaxObjection AddObjection(bool bMultiple)
		{
			return AddObjection((CTmaxObjection)null, bMultiple);
		}

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected virtual void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to locate the toolbar tool named %1");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add an objection");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CBasePane error string 2");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CBasePane error string 3");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CBasePane error string 4");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CBasePane error string 5");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CBasePane error string 6");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CBasePane error string 7");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CBasePane error string 8");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CBasePane error string 9");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CBasePane error string 10");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CBasePane error string 11");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CBasePane error string 12");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CBasePane error string 13");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CBasePane error string 14");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CBasePane error string 15");

		}// protected virtual void SetErrorStrings()

		/// <summary>This method is called to warn the user when an invalid range value is encountered</summary>
		/// <param name="strMsg">The warning message</param>
		/// <param name="ctrlSelect">the control to select after the warning is displayed</param>
		protected virtual bool Warn(string strMsg, System.Windows.Forms.Control ctrlSelect)
		{
			MessageBox.Show(strMsg, "Warning", MessageBoxButtons.OK,
							MessageBoxIcon.Exclamation);
			
			if(ctrlSelect != null)
				ctrlSelect.Focus();	
				
			return false; // allows for cleaner code						
		
		}// private void Warn(string strMsg, System.Windows.Forms.TextBox ctrlSelect)
		
		/// <summary>This method is called to display a warning</summary>
		/// <param name="strWarning">The warning text</param>
		/// <return>false always</return>
		protected virtual bool Warn(string strWarning)
		{
			return Warn(strWarning, null);
		}
		
		/// <summary>This method will build a packed collection of media items using the items in the specified collection</summary>
		/// <param name="tmaxItems">The collection of items to be packed</param>
		/// <param name="bSubItems">true to use SubItems, false to use SourceItems</param>
		/// <param name="tmaxPacked">Collection in which to place the packed items</param>
		protected virtual void Pack(CTmaxItems tmaxItems, bool bSubItems, CTmaxItems tmaxPacked)
		{
			Debug.Assert(tmaxItems != null);
			Debug.Assert(tmaxPacked != null);
			
			foreach(CTmaxItem O in tmaxItems)
			{
				Pack(O, bSubItems, tmaxPacked);
			}
			
		}// protected virtual void Pack(CTmaxItems tmaxItems, bool bSubItems, CTmaxItems tmaxPacked)
		
		/// <summary>This method will iterate the specified collection and build a packed collection of media items</summary>
		/// <param name="tmaxItem">The item to be packed</param>
		/// <param name="bSubItems">true to use SubItems, false to use SourceItems</param>
		/// <param name="tmaxPacked">Collection in which to place the packed items</param>
		protected virtual void Pack(CTmaxItem tmaxItem, bool bSubItems, CTmaxItems tmaxPacked)
		{
			CTmaxItems tmaxCollection = null;
			
			Debug.Assert(tmaxItem != null);
			
			//	Is this item a valid media record?
			if(tmaxItem.GetMediaRecord() != null)
			{
				//	Add this item to the packed collection
				tmaxPacked.Add(new CTmaxItem(tmaxItem.GetMediaRecord()));
			}
			
			//	What collection should we use?
			if(bSubItems == true)
				tmaxCollection = tmaxItem.SubItems;
			else
				tmaxCollection = tmaxItem.SourceItems;
				
			//	Check all children
			if((tmaxCollection != null) && (tmaxCollection.Count > 0))
			{
				foreach(CTmaxItem O in tmaxCollection)
				{
					Pack(O, bSubItems, tmaxPacked);
				}
				
			}
		
		}// protected virtual void Pack(CTmaxItem tmaxItem, bool bSubItems, CTmaxItems tmaxPacked)
		
		/// <summary>This method is to determine if the media associated with the specified record can be rotated</summary>
		/// <param name="tmaxItem">The item to be activated</param>
		/// <returns>true if the media can be rotated</returns>
		protected virtual bool CanRotate(CDxMediaRecord dxRecord)
		{
			CDxPrimary dxScript = null;
			
			//	What type of media is this?
			switch(dxRecord.MediaType)
			{
				case TmaxMediaTypes.Document:
				case TmaxMediaTypes.Page:
				
					return true;
					
				case TmaxMediaTypes.Script:
				
					//	Get the source for the first scene to see if
					//	we can rotate that
					dxScript = (CDxPrimary)dxRecord;
					if((dxScript.Secondaries == null) || (dxScript.Secondaries.Count == 0))
						dxScript.Fill();
						
					if(dxScript.Secondaries.Count > 0)
					{
						if(dxScript.Secondaries[0].GetSource() != null)
							return CanRotate(dxScript.Secondaries[0].GetSource());
					}
					
					return false;
					
				case TmaxMediaTypes.Scene:
				
					//	Can we rotate the source?
					if(((CDxSecondary)dxRecord).GetSource() != null)
						return CanRotate(((CDxSecondary)dxRecord).GetSource());
					else
						return false;
						
				case TmaxMediaTypes.Link:
				
					//	Can we rotate the source?
					if(((CDxQuaternary)dxRecord).GetSource() != null)
						return CanRotate(((CDxQuaternary)dxRecord).GetSource());
					else
						return false;
				
				default:
				
					return false;
			}
			
		}// protected virtual bool CanRotate(CDxMediaRecord dxRecord)
		
		/// <summary>This method is called to determine if the user is pressing the Control key</summary>
		/// <returns>True if Control key is pressed</returns>
		protected virtual bool GetControlPressed()
		{
			return CTmaxToolbox.IsControlKeyPressed();
		}
		
		/// <summary>This method is called to determine if the user is pressing the Shift key</summary>
		/// <returns>True if Shift key is pressed</returns>
		protected virtual bool GetShiftPressed()
		{
			return CTmaxToolbox.IsShiftKeyPressed();
		}

		/// <summary>Called to confirm the user's desire to delete the selected media</summary>
		///	<returns>true to continue with the operation</returns>
		protected virtual bool GetDeleteConfirmation()
		{
			CFGetConfirmation	wndGetConfirmation = null;
			bool				bConfirmed = true;

			try
			{
				//	Do we need to prompt the user?
				if(m_bDeleteConfirmed == false)
				{
					wndGetConfirmation = new CFGetConfirmation();
					m_tmaxEventSource.Attach(wndGetConfirmation.EventSource);
					
					wndGetConfirmation.Message = "Are you sure you want to delete the selected records?";
					wndGetConfirmation.ApplyAllLabel = "Automatically confirm future attempts";
					
					if(wndGetConfirmation.ShowDialog() == DialogResult.Yes)
					{
						m_bDeleteConfirmed = wndGetConfirmation.ApplyAll;
					}
					else
					{
						bConfirmed = false;
					}

				}// if(m_bDeleteConfirmed == false)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetDeleteConfirmation", Ex);
				bConfirmed = true;
			}
			
			return bConfirmed;

		}// protected virtual bool GetDeleteConfirmation()

		/// <summary>This method is called to determine if the active database supports the use of document codes (fielded data)</summary>
		/// <returns>True if Shift key is pressed</returns>
		protected virtual bool GetCodesEnabled()
		{
			if(this.Database == null) return false;
			else return this.Database.CodesEnabled;
				
		}// protected virtual bool GetCodesEnabled()
		
		/// <summary>This method is called to determine if the user has set the IfTreated option for primary media filtering</summary>
		/// <returns>True if primary media is being filtered on IfTreated</returns>
		protected virtual bool GetIfTreated()
		{
			if(m_tmaxStationOptions == null) return false;
			if(m_tmaxStationOptions.AdvancedFilter == null) return false;
		
			//	Ignore the flag if the filtered collection has been fast filtered
			if((this.Filtered != null) && (this.Filtered.FastFiltered == true))
				return false;
				
			//	Use the filter setting
			return m_tmaxStationOptions.AdvancedFilter.IfTreated;
				
		}// protected virtual bool GetIfTreated()
		
		/// <summary>Overridden by derived classes to provide access to the pane's toolbar manager</summary>
		/// <returns>The pane's toolbar manager if available</returns>
		protected virtual Infragistics.Win.UltraWinToolbars.UltraToolbarsManager GetUltraToolbarManager()
		{
			return null;
		}

		/// <summary>This function will retrieve the tool with the specified key</summary>
		/// <param name="strKey">Alpha-numeric tool key identifier</param>
		///	<param name="bSuppress">true to suppress notifications fired when an exception is raised</param>
		/// <returns>Infragistic base class tool object</returns>
		protected virtual ToolBase GetUltraTool(string strKey, bool bSuppress)
		{
			ToolBase				toolBase = null;
			UltraToolbarsManager	toolbarsManager = null;		

			try
			{
				if((toolbarsManager = GetUltraToolbarManager()) != null)
				{
					toolBase = toolbarsManager.Tools[strKey];
				}
				else
				{
					Debug.Assert(false, "No toolbar manager available");
				}
				
			}
			catch(System.Exception Ex)
			{
				if(bSuppress == false)
					m_tmaxEventSource.FireError(this, "GetUltraTool", m_tmaxErrorBuilder.Message(ERROR_BASE_PANE_GET_ULTRA_TOOL_EX, strKey), Ex);
			}
			
			return toolBase;
		
		}// protected virtual ToolBase GetUltraTool(string strKey)
				
		/// <summary>This function will retrieve the tool with the specified key</summary>
		/// <param name="strKey">Alpha-numeric tool key identifier</param>
		/// <returns>Infragistic base class tool object</returns>
		protected virtual ToolBase GetUltraTool(string strKey)
		{
			return GetUltraTool(strKey, false);
		}
			
		/// <summary>This method is called when the user brings up the Set Highlighter menu</summary>
		/// <param name="popupMenu">The menu containing the highlighter selections</param>
		/// <param name="dxHighlighter">The pane's active highlighter</param>
		protected virtual void OnBeforeSetHighlighterMenu(PopupMenuTool popupMenu, CDxHighlighter dxActive)
		{
			string			strKey = "";
			StateButtonTool	toolHighlighter = null;
			CDxHighlighter	dxHighlighter = null;
			
			if(m_tmaxDatabase == null) return;
			if(m_tmaxDatabase.Highlighters == null) return;

			try
			{
				//	Suppress toolbar events while we process the menu items
				m_bIgnoreUltraEvents = true;
				
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
							
							if((dxActive != null) && (ReferenceEquals(dxActive, dxHighlighter) == true))
								toolHighlighter.Checked = true;
							else
								toolHighlighter.Checked = false;
							
							toolHighlighter.SharedProps.Visible = true;
						}
						else
						{
							toolHighlighter.SharedProps.Visible = false;
						}
					
					}// if((toolHighlighter = (StateButtonTool)GetUltraTool(strKey)) != null)
				
				}// for(int i = 0; i < m_tmaxDatabase.Highlighters.Count; i++)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnBeforeSetHighlighterMenu", Ex);
			}
			finally
			{
				//	restore toolbar events 
				m_bIgnoreUltraEvents = false;				
			}
				
		}// private void OnBeforeSetHighlighterMenu(PopupMenuTool popupMenu)

		#endregion Protected Methods
		
		#region Command Events
		
		/// <summary>This event is fired by a pane to issue a command</summary>
		public event FTI.Shared.Trialmax.TmaxCommandHandler TmaxCommandEvent;
		
		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="Args">Command argument object</param>
		/// <returns>true if successful</returns>
		protected virtual bool FireCommand(CTmaxCommandArgs Args)
		{
			try
			{
				//	Is anybody registered?
				if(TmaxCommandEvent != null)
				{
					TmaxCommandEvent(this, Args);
					return Args.Successful;
				}
				else
				{
					return true;
				}
			}
			catch
			{
				return false;
			}
			
		}
		
		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItems">The collection of items associated with the command event</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		/// <returns>true if successful</returns>
		protected virtual bool FireCommand(TmaxCommands eCommand, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)
		{
			CTmaxCommandArgs Args = null;
			
			try
			{
				// Get the command arguments
				if((Args = new CTmaxCommandArgs(eCommand, m_iPaneId, tmaxItems, tmaxParameters)) != null)
				{
					return FireCommand(Args);
				}
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "FireCommand", "Fire " + eCommand.ToString() + " command Ex: " + Ex.ToString());
			}
			
			return false;
		
		}//	FireCommand(TmaxCommands eCommand, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItems">The collection of items associated with the command event</param>
		/// <returns>true if successful</returns>
		protected virtual bool FireCommand(TmaxCommands eCommand, CTmaxItems tmaxItems)
		{
			return FireCommand(eCommand, tmaxItems, null);
		
		}//	FireCommand(TmaxCommands eCommand, CTmaxItems tmaxItems)

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItem">The item to be passed with the event</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		/// <returns>true if successful</returns>
		protected virtual bool FireCommand(TmaxCommands eCommand, CTmaxItem tmaxItem, CTmaxParameters tmaxParameters)
		{
			CTmaxItems tmaxItems = new CTmaxItems();
			
			tmaxItems.Add(tmaxItem);
			
			return FireCommand(eCommand, tmaxItems, tmaxParameters);
		
		}//	FireCommand(TmaxCommands eCommand, CTmaxItem tmaxItem, CTmaxParameters tmaxParameters)

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItem">The item to be passed with the event</param>
		/// <returns>true if successful</returns>
		protected virtual bool FireCommand(TmaxCommands eCommand, CTmaxItem tmaxItem)
		{
			return FireCommand(eCommand, tmaxItem, null);
		
		}//	FireCommand(TmaxCommands eCommand, CTmaxItem tmaxItem)

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <returns>true if successful</returns>
		protected virtual bool FireCommand(TmaxCommands eCommand)
		{
			return FireCommand(eCommand, (CTmaxItems)null, (CTmaxParameters)null);
		
		}//	FireCommand(TmaxCommands eCommand)

		/// <summary>This method is called to post a command message to the application's main window</summary>
		/// <param name="Args">Command argument object</param>
		/// <returns>true if successful</returns>
		protected virtual bool FireAsyncCommand(CTmaxCommandArgs Args)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Do we have the main window handle?
				if((m_tmaxAppOptions != null) && (m_tmaxAppOptions.AppMainWnd != System.IntPtr.Zero))
				{
					//	Save the arguments
					m_asyncCommandArgs = Args;
					
                    // .NET 2.0 MODIFICATION
                    //FTI.Shared.Win32.User.PostThreadMessage(System.Threading.Thread.CurrentThread.ManagedThreadId, 
                    //                                        (int)TmaxWindowMessages.Command,
                    //                                        (int)(Args.Command),
                    //                                        this.PaneId);
                    FTI.Shared.Win32.User.PostThreadMessage(AppDomain.GetCurrentThreadId(),
                                                            (int)TmaxWindowMessages.Command,
                                                            (int)(Args.Command),
                                                            this.PaneId);
                    bSuccessful = true;
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "FireAsyncCommand", Ex);
			}
			
			//	Fire a synchronous command if we fail
			if(bSuccessful == false)
			{
				FireCommand(Args);
				m_tmaxEventSource.FireDiagnostic(this, "FireAsyncCommand", "Asynchronous failed: command = " + Args.Command.ToString());
			}
				
			return bSuccessful;
			
		}// protected virtual bool FireAsyncCommand(CTmaxCommandArgs Args)
		
		/// <summary>This method is called to post a command message to the application's main window</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItems">The collection of items associated with the command event</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		/// <returns>true if successful</returns>
		protected virtual bool FireAsyncCommand(TmaxCommands eCommand, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)
		{
			CTmaxCommandArgs Args = null;
			
			try
			{
				// Get the command arguments
				if((Args = new CTmaxCommandArgs(eCommand, m_iPaneId, tmaxItems, tmaxParameters)) != null)
				{
					return FireAsyncCommand(Args);
				}
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "FireAsyncCommand", "Post " + eCommand.ToString() + " command Ex: " + Ex.ToString());
			}
			
			return false;
		
		}//	FireAsyncCommand(TmaxCommands eCommand, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)

		/// <summary>This method is called to post a command message to the application's main window</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItems">The collection of items associated with the command event</param>
		/// <returns>true if successful</returns>
		protected virtual bool FireAsyncCommand(TmaxCommands eCommand, CTmaxItems tmaxItems)
		{
			return FireAsyncCommand(eCommand, tmaxItems, null);
		
		}//	FireAsyncCommand(TmaxCommands eCommand, CTmaxItems tmaxItems)

		/// <summary>This method is called to post a command message to the application's main window</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItem">The item to be passed with the event</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		/// <returns>true if successful</returns>
		protected virtual bool FireAsyncCommand(TmaxCommands eCommand, CTmaxItem tmaxItem, CTmaxParameters tmaxParameters)
		{
			CTmaxItems tmaxItems = new CTmaxItems();
			
			tmaxItems.Add(tmaxItem);
			
			return FireAsyncCommand(eCommand, tmaxItems, tmaxParameters);
		
		}//	FireAsyncCommand(TmaxCommands eCommand, CTmaxItem tmaxItem, CTmaxParameters tmaxParameters)

		/// <summary>This method is called to post a command message to the application's main window</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItem">The item to be passed with the event</param>
		/// <returns>true if successful</returns>
		protected virtual bool FireAsyncCommand(TmaxCommands eCommand, CTmaxItem tmaxItem)
		{
			return FireAsyncCommand(eCommand, tmaxItem, null);
		
		}//	FireAsyncCommand(TmaxCommands eCommand, CTmaxItem tmaxItem)

		/// <summary>This method is called to post a command message to the application's main window</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <returns>true if successful</returns>
		protected virtual bool FireAsyncCommand(TmaxCommands eCommand)
		{
			return FireAsyncCommand(eCommand, (CTmaxItems)null, (CTmaxParameters)null);
		
		}//	FireAsyncCommand(TmaxCommands eCommand)

		#endregion Command Events
		
		#region Drag/Drop Operations

		/// <summary>This method traps all DragDrop events</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">System event arguements</param>
		protected virtual void OnDragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			//m_tmaxEventSource.FireDiagnostic(this, "OnDragDrop", "BasePane Event Handler");
		}

		/// <summary>This method traps all DragEnter events</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">System event arguements</param>
		protected virtual void OnDragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			//m_tmaxEventSource.FireDiagnostic(this, "OnDragEnter", "BasePane Event Handler");
		}

		/// <summary>This method traps all DragLeave events</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">System event arguements</param>
		protected virtual void OnDragLeave(object sender, System.EventArgs e)
		{
			//m_tmaxEventSource.FireDiagnostic(this, "OnDragLeave", "BasePane Event Handler");
		}

		/// <summary>This method traps all DragOver events</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">System event arguements</param>
		protected virtual void OnDragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			//m_tmaxEventSource.FireDiagnostic(this, "OnDragOver", "BasePane Event Handler");
		}

		/// <summary>This method traps all QueryContinueDrag events</summary>
		/// <param name="sender">Object firing the event</param>
		/// <param name="e">System event arguements</param>
		protected virtual void OnQueryContinueDrag(object sender, System.Windows.Forms.QueryContinueDragEventArgs e)
		{
			//	Did the user press escape? 
			if(e.EscapePressed)
			{
				//	Cancel the Drag
				e.Action = DragAction.Cancel;
			}
			//m_tmaxEventSource.FireDiagnostic(this, "OnQueryContinueDrag", e.Action.ToString());
		}

		#endregion Drag/Drop Operations

		#region Properties		

		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}

		/// <summary>Arguments used when posting an asynchronous command</summary>
		public CTmaxCommandArgs AsyncCommandArgs
		{
			get { return m_asyncCommandArgs; }
			set { m_asyncCommandArgs = value; }
		}

		/// <summary>This property is used to assign an identifier to the pane</summary>
		/// <remarks>The identifier is used when the pane fires events</remarks>
		public int PaneId
		{
			get { return m_iPaneId; }
			set { SetPaneId(value); }
		}

		/// <summary>This property is true when the pane is visible on the screen</summary>
		/// <remarks>This is NOT the same as the standard Visible property (the pane may be buried in a docking group)</remarks>
		public bool PaneVisible
		{
			get { return m_bPaneVisible; }
			set { SetPaneVisible(value); }
		}

		/// <summary>This property is used to assign a name to the pane</summary>
		public string PaneName
		{
			get { return m_strPaneName; }
			set { SetPaneName(value); }
		}
		
		/// <summary>Name of the file containing the configuration options for TmaxPresentation application</summary>
		public string PresentationOptionsFilename
		{
			get { return m_strPresentationOptionsFilename; }
			set	{ SetPresentationOptionsFilename(value); }
		}
		
		/// <summary>TrialMax application source registration options</summary>
		public CTmaxRegOptions RegistrationOptions
		{
			get { return m_tmaxRegOptions; }
			set { SetRegistrationOptions(value); } 
		}
		
		/// <summary>TmaxManager application options</summary>
		public CTmaxManagerOptions ApplicationOptions
		{
			get { return m_tmaxAppOptions; }
			set { SetApplicationOptions(value); }
		}

		/// <summary>TmaxPresentation application options</summary>
		public CTmaxPresentationOptions PresentationOptions
		{
			get { return m_tmaxPresentationOptions; }
			set { SetPresentationOptions(value); }
		}

		/// <summary>TrialMax case options</summary>
		public CTmaxCaseOptions CaseOptions
		{
			get { return m_tmaxCaseOptions;  }
			set { SetCaseOptions(value); }
		}
		
		/// <summary>TrialMax station specific options</summary>
		public CTmaxStationOptions StationOptions
		{
			get { return m_tmaxStationOptions; }
			set { SetStationOptions(value); }
		}
		
		/// <summary>TrialMax application database</summary>
		public CTmaxCaseDatabase Database
		{
			get { return m_tmaxDatabase; }
			set { SetDatabase(value); }
		}
		
		/// <summary>The collection of filtered records owned by the active database</summary>
		public FTI.Trialmax.Database.CDxPrimaries Filtered
		{
			get { return m_dxFiltered; }
			set { SetFiltered(value); }		
		}
		
		/// <summary>TrialMax application report manager</summary>
		public FTI.Trialmax.Reports.CTmaxReportManager ReportManager
		{
			get { return m_tmaxReportManager; }
			set { SetReportManager(value); }
		}
		
		/// <summary>TrialMax application source registration options</summary>
		public CTmaxMediaTypes MediaTypes
		{
			get { return m_tmaxMediaTypes; }
			set { SetMediaTypes(value); }
		}
		
		/// <summary>TrialMax database active case code descriptors</summary>
		public FTI.Shared.Trialmax.CTmaxCaseCodes CaseCodes
		{
			get { return m_tmaxCaseCodes; }
			set { SetCaseCodes(value); }		
		}
		
		/// <summary>TrialMax application source type options</summary>
		public CTmaxSourceTypes SourceTypes
		{
			get { return m_tmaxSourceTypes; }
			set { SetSourceTypes(value); }
		}
		
		/// <summary>TrialMax application copy/paste clipboard</summary>
		public CTmaxItems TmaxClipboard
		{
			get { return m_tmaxClipboard; }
			set { SetTmaxClipboard(value); }
		}
		
		/// <summary>TrialMax application product descriptor</summary>
		public FTI.Shared.Trialmax.CTmaxProductManager TmaxProductManager
		{
			get { return m_tmaxProductManager; }
			set { SetTmaxProductManager(value); }
		}
		
		/// <summary>TrialMax application registry interface</summary>
		public FTI.Shared.Trialmax.CTmaxRegistry TmaxRegistry
		{
			get { return m_tmaxRegistry; }
			set { SetTmaxRegistry(value); }
		}
		
		#endregion Properties
		
	}// CBasePane

} // namespace FTI

