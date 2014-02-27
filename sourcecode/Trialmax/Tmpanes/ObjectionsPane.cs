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
	public class CObjectionsPane : FTI.Trialmax.Panes.CBasePane
	{
		#region Constants

		/// <summary>Local context menu command identifiers</summary>
		private enum ObjectionsPaneCommands
		{
			Invalid = 0,
			Add,
			Delete,
			DesignationOnly,
			ChooseColumns,
			Repeat,
		}

		private const int ERROR_ON_OBJECTIONS_ADDED_EX		= (ERROR_BASE_PANE_MAX + 1);
		private const int ERROR_ON_CMD_DELETE_EX			= (ERROR_BASE_PANE_MAX + 2);
		private const int ERROR_ON_CMD_ADD_EX				= (ERROR_BASE_PANE_MAX + 3);
		private const int ERROR_ACTIVATE_EX					= (ERROR_BASE_PANE_MAX + 4);
		private const int ERROR_FIRE_OBJECTION_SELECTED_EX	= (ERROR_BASE_PANE_MAX + 5);
		private const int ERROR_FILL_EX						= (ERROR_BASE_PANE_MAX + 6);
		private const int ERROR_ON_CMD_SHOW_DESIGNATION_EX	= (ERROR_BASE_PANE_MAX + 7);
		private const int ERROR_SET_DESIGNATION_EX			= (ERROR_BASE_PANE_MAX + 8);
		private const int ERROR_CHECK_VISIBLE_EX			= (ERROR_BASE_PANE_MAX + 9);
		private const int ERROR_FIRE_NAVIGATOR_CHANGED_EX	= (ERROR_BASE_PANE_MAX + 10);
		private const int ERROR_ON_NAVIGATE_EX				= (ERROR_BASE_PANE_MAX + 11);

		#endregion Constants

		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>Infragistics toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager m_ultraToolbarManager;

		/// <summary>Infragistics toolbar manager left docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CObjectionsPane_Toolbars_Dock_Area_Left;

		/// <summary>Infragistics toolbar manager right docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CObjectionsPane_Toolbars_Dock_Area_Right;

		/// <summary>Infragistics toolbar manager top docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CObjectionsPane_Toolbars_Dock_Area_Top;

		/// <summary>Infragistics toolbar manager bottom docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CObjectionsPane_Toolbars_Dock_Area_Bottom;

		/// <summary>Image list bound to the toolbar</summary>
		private ImageList m_ctrlToolbarImages;

		/// <summary>Grid to display the available objections</summary>
		private CTmaxObjectionGridCtrl m_ctrlGrid;

		/// <summary>Local member to indicate that the selection change should be cancelled</summary>
		private bool m_bCancelSelChange = false;

		/// <summary>Local member to indicate that we should check for changes before activating a new objection</summary>
		private bool m_bCheckForChanges = true;

		/// <summary>Local member to indicate that we should view only those objections for current designation</summary>
		private bool m_bDesignationOnly = false;

		/// <summary>Local member to store a reference to the application's active deposition</summary>
		private FTI.Trialmax.Database.CDxPrimary m_dxDeposition = null;

		/// <summary>Local member to store a reference to the application's active script scene</summary>
		private FTI.Trialmax.Database.CDxSecondary m_dxScene = null;

		/// <summary>Local member to store a reference to the application's active designation</summary>
		private FTI.Trialmax.Database.CDxTertiary m_dxDesignation = null;

		/// <summary>Local member to store the collection of objections to be loaded in the grid</summary>
		private FTI.Shared.Trialmax.CTmaxObjections m_tmaxObjections = new CTmaxObjections();

		#endregion Private Members

		#region Public Methods

		/// <summary>Constructor</summary>
		public CObjectionsPane() : base()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			
			m_tmaxEventSource.Name = "Objections Pane";
			m_tmaxEventSource.Attach(m_ctrlGrid.EventSource);

		}// public CObjectionsPane()

		/// <summary>This method is called by the application to activate the specified item</summary>
		/// <param name="tmaxItem">The item to be activated</param>
		/// <param name="ePane">The pane requesting activation</param>
		/// <returns>true if successful</returns>
		public override bool Activate(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
			CDxMediaRecord	dxRecord = null;
			CDxSecondary	dxScene = null;

			//	Don't bother if not valid media
			if((dxRecord = (CDxMediaRecord)(tmaxItem.GetMediaRecord())) != null)
			{
				//	Is this a scene in a script?
				if(dxRecord.MediaType == TmaxMediaTypes.Scene)
				{
					//	Is the source record a designation?
					if(((CDxSecondary)dxRecord).GetSource() != null)
					{
						if(((CDxSecondary)dxRecord).GetSource().MediaType == TmaxMediaTypes.Designation)
							dxScene = ((CDxSecondary)dxRecord);
					}

				}// if(dxRecord.MediaType == TmaxMediaTypes.Scene)

			}// if((dxRecord = (CDxMediaRecord)(tmaxItem.GetMediaRecord())) != null)

			//	Set the active designation
			SetDesignation(dxScene);
			
			return true;

		}// public override bool Activate(FTI.Shared.Trialmax.CTmaxItem tmaxItem)

		/// <summary>This method is called by the application when it adds new media to the database</summary>
		/// <param name="tmaxParent">TrialMax event item that identifies the parent record</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		public override void OnObjectionsAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		{
			CTmaxObjection tmaxSelection = null;
			
			try
			{
				//	New records are stored in the child collection
				if((tmaxChildren != null) && (tmaxChildren.Count > 0))
				{
					foreach(CTmaxItem O in tmaxChildren)
					{
						if(O.Objection != null)
						{
							//	Should we display this objection?
							if(CheckVisible(O.Objection) == true)
							{
								m_tmaxObjections.Add(O.Objection);

								m_ctrlGrid.Add(((COxObjection)(O.IObjection)).TmaxObjection);

								if(tmaxSelection == null)
									tmaxSelection = O.Objection;

							}// if(CheckVisible(O.Objection) == true)

						}// if(O.Objection != null)
						
					}// foreach(CTmaxItem O in tmaxChildren)

				}// if((tmaxChildren != null) && (tmaxChildren.Count > 0))
				
				//	Select the first objection that was added
				if(tmaxSelection != null)
					m_ctrlGrid.SetSelection(tmaxSelection);

				FireNavigatorChanged();
				
				SetToolStates(false);
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnObjectionsAdded", m_tmaxErrorBuilder.Message(ERROR_ON_OBJECTIONS_ADDED_EX), Ex);
			}

		}// public override void OnObjectionsAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)

		/// <summary>This method is called by the application when multiple records have been updated in an operation</summary>
		/// <param name="tmaxItems">The items that have been updated</param>
		public override void OnObjectionsUpdated(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			CTmaxObjection	tmaxSelection = null;
			int				iIndex = -1;
			
			foreach(CTmaxItem O in tmaxItems)
			{
				if(O.Objection != null)
				{
					//	Is this objection already in the grid?
					if(m_tmaxObjections.Contains(O.Objection) == true)
					{
						//	Make sure the case deposition has been set
						if(O.Objection.ICaseDeposition == null)
						{
							//	Since it's in the list we know it belongs to the active deposition
							O.Objection.ICaseDeposition = m_dxDeposition;

						}// if(tmaxObjection.ICaseDeposition == null)
						
						//	Should we remove the objection?
						if(CheckVisible(O.Objection) == false)
						{
							//	Remove from the grid
							iIndex = m_ctrlGrid.Delete(O.Objection);
							m_tmaxObjections.Remove(O.Objection);
							
							//	Set the new selection
							if(iIndex < m_tmaxObjections.Count)
								tmaxSelection = m_ctrlGrid.GetObjection(iIndex);
							else
								tmaxSelection = m_ctrlGrid.GetObjection(m_tmaxObjections.Count - 1);
						}
						else
						{
							//	Update the values in the grid
							m_ctrlGrid.Update(O.Objection);
						}
						
					}
					else
					{
						//	Should it be added?
						if(CheckVisible(O.Objection) == true)
						{
							m_tmaxObjections.Add(O.Objection);

							m_ctrlGrid.Add(O.Objection);

							if(tmaxSelection == null)
								tmaxSelection = O.Objection;
						}

					}// if(m_tmaxObjections.Contains(O.Objection) == true)

				}// if(O.Objection != null)
				
			}// foreach(CTmaxItem O in tmaxItems)

			//	Should we set the selection?
			if(tmaxSelection != null)
				m_ctrlGrid.SetSelection(tmaxSelection);

			SetToolStates(false);

		}// public override void OnObjectionsUpdated(FTI.Shared.Trialmax.CTmaxItems tmaxItems)

		/// <summary>This method is called by the application when objections have been deleted</summary>
		/// <param name="tmaxItems">The objections that have been deleted</param>
		public override void OnObjectionsDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			int				iIndex = -1;
			CTmaxObjection	tmaxSelection = null;
						
			foreach(CTmaxItem O in tmaxItems)
			{
				//	Is this objection in the current collection?
				if((O.Objection != null) && (m_tmaxObjections.Contains(O.Objection) == true))
				{
					//	Remove from the grid
					iIndex = m_ctrlGrid.Delete(O.Objection);

					//	Remove from the master list
					m_tmaxObjections.Remove(O.Objection);

					//	Get the new selection
					if(iIndex < m_tmaxObjections.Count)
						tmaxSelection = m_ctrlGrid.GetObjection(iIndex);
					else
						tmaxSelection = m_ctrlGrid.GetObjection(m_tmaxObjections.Count - 1);

				}// if(O.Objection != null)

			}// foreach(CTmaxItem O in tmaxItems)

			//	Should we set the selection?
			if(tmaxSelection != null)
				m_ctrlGrid.SetSelection(tmaxSelection);
			else if((m_ctrlGrid.GetSelection() == null) && (m_ctrlGrid.GetTotalCount() > 0))
				m_ctrlGrid.SetSelection(0);

			FireNavigatorChanged();
			SetToolStates(false);

		}// public override void OnObjectionsDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)

		/// <summary>This method is called by the application to when the item gets deleted</summary>
		/// <param name="tmaxItem">The item that has been deleted</param>
		public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			SetToolStates(false);

		}// public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)

		/// <summary>This method is called by the application to initialize the pane</summary>
		/// <returns>true if successful</returns>
		public override bool Initialize(CXmlIni xmlINI)
		{
			//	Do the base class initialization first
			if(base.Initialize(xmlINI) == false)
				return false;

			//	Line up on the section containing the grid configuration
			xmlINI.SetSection(m_strPaneName + "\\Grid");

			//	Initialize the grid and load it's configuration
			m_ctrlGrid.Initialize(xmlINI);

			return true;

		}// public override bool Initialize()

		/// <summary>This method is called by the application when it is about to terminate</summary>
		/// <remarks>Derived classes should override for custom shutdown</remarks>
		public override void Terminate(CXmlIni xmlINI)
		{
			//	Line up on the section containing the grid configuration
			xmlINI.SetSection(m_strPaneName + "\\Grid");

			//	Save the grid's configuration
			m_ctrlGrid.Save(xmlINI);
			
			//	Do the base class processing
			base.Terminate(xmlINI);

		}// public override void Terminate(CXmlIni xmlINI)

		/// <summary>This method is called by the application when a pane wants to move a record collection navigator</summary>
		/// <param name="ePane">The pane requesting the move</param>
		/// <param name="tmaxItems">The items passed with the event</param>
		/// <param name="tmaxParameters">The parameters passed with the event</param>
		public override void OnNavigate(TmaxAppPanes ePane, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)
		{
			CTmaxParameter			tmaxParameter = null;
			bool					bObjections = false;
			TmaxNavigatorRequests	eRequest = TmaxNavigatorRequests.Invalid;
			int						iIndex = -1;
			CTmaxObjection			tmaxObjection = null;

			if(m_tmaxObjections == null) return;
			
			try
			{
				if(tmaxParameters != null)
				{
					if((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.Objections)) != null)
						bObjections = tmaxParameter.AsBoolean();
					if((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.NavigatorRequest)) != null)
						eRequest = (TmaxNavigatorRequests)(tmaxParameter.AsInteger());
				}

				if((bObjections == true) && (eRequest != TmaxNavigatorRequests.Invalid))
				{
					switch(eRequest)
					{
						case TmaxNavigatorRequests.First:
						
							if(m_ctrlGrid.GetTotalCount() > 0)
								tmaxObjection = m_ctrlGrid.GetObjection(0);
							break;

						case TmaxNavigatorRequests.Last:

							if(m_ctrlGrid.GetTotalCount() > 0)
								tmaxObjection = m_ctrlGrid.GetObjection(m_ctrlGrid.GetTotalCount() - 1);
							break;

						case TmaxNavigatorRequests.Next:

							//	Get the index of the current selection
							if((tmaxObjection = m_ctrlGrid.GetSelection()) != null)
								iIndex = m_ctrlGrid.GetIndex(tmaxObjection);
								
							if(iIndex < 0)
								tmaxObjection = m_ctrlGrid.GetObjection(0);
							else if((iIndex + 1) < m_ctrlGrid.GetTotalCount())
								tmaxObjection = m_ctrlGrid.GetObjection(iIndex + 1);
							
							break;

						case TmaxNavigatorRequests.Previous:

							//	Get the index of the current selection
							if((tmaxObjection = m_ctrlGrid.GetSelection()) != null)
								iIndex = m_ctrlGrid.GetIndex(tmaxObjection);

							if(iIndex > 0)
								tmaxObjection = m_ctrlGrid.GetObjection(iIndex - 1);
							else if(iIndex < 0)
								tmaxObjection = m_ctrlGrid.GetObjection(0);

							break;

						case TmaxNavigatorRequests.Absolute:

							if(m_ctrlGrid.GetTotalCount() > 0)
							{
								//	Get the index requested by the trigger
								if((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.NavigatorIndex)) != null)
									iIndex = tmaxParameter.AsInteger();

								if(iIndex< 0)
									tmaxObjection = m_ctrlGrid.GetObjection(0);
								else if(iIndex >= m_ctrlGrid.GetTotalCount())
									tmaxObjection = m_ctrlGrid.GetObjection(m_ctrlGrid.GetTotalCount() - 1);
								else
									tmaxObjection = m_ctrlGrid.GetObjection(iIndex);
							}
							break;

						case TmaxNavigatorRequests.QueryPosition:

							FireNavigatorChanged();
							break;

						case TmaxNavigatorRequests.SetMode:

							//	Get the mode requested by the trigger
							if((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.NavigatorMode)) != null)
							{
								if(m_bDesignationOnly != tmaxParameter.AsBoolean())
								{
									OnCmdDesignationOnly();
								}
							}
							break;

					}// switch(eRequest)

				}// if((bObjections == true) && (eRequest != TmaxNavigatorRequests.Invalid))
				
				//	Activate the requested objection
				if(tmaxObjection != null)
					m_ctrlGrid.SetSelection(tmaxObjection);
					
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnNavigate", m_tmaxErrorBuilder.Message(ERROR_ON_NAVIGATE_EX), Ex);
			}

		}// public override void OnNavigate(TmaxAppPanes ePane, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)

		/// <summary>This method is called by the application when the user sets the active deponent</summary>
		/// <param name="tmaxItem">The item that identifies the deponent</param>
		/// <param name="ePane">The pane requesting activation</param>
		public override void OnSetDeposition(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
			CDxPrimary dxDeposition = null;
			
			if((tmaxItem != null) && (tmaxItem.IPrimary != null))
			{
				//	Is this a deposition?
				if(tmaxItem.IPrimary.GetMediaType() == TmaxMediaTypes.Deposition)
					dxDeposition = (CDxPrimary)(tmaxItem.IPrimary);
			}

			//	Has the deposition changed?
			if(ReferenceEquals(m_dxDeposition, dxDeposition) == false)
			{
				m_dxDeposition = dxDeposition;
			
				//	Refill the grid
				Fill();
			}

		}// public override void OnSetDeposition(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)

		#endregion Public Methods

		#region Protected Methods

		/// <summary>This function is called when the value of the Database property changes</summary>
		protected override void OnDatabaseChanged()
		{
			//	Do the base class processing first
			base.OnDatabaseChanged();

			if((m_tmaxDatabase != null) && (m_tmaxDatabase.Objections != null))
			{
				//	The deposition may have already been assigned by the transcripts pane
				if(m_dxDeposition != null)
					Fill();
			}
			else
			{
				//	Clear the objections
				Clear();
			}
			
			//	Make sure the tools are enabled/disabled
			SetToolStates(false);
			
		}// protected override void OnDatabaseChanged()

		/// <summary>This function is called when the PaneVisible property changes</summary>
		protected override void OnPaneVisibleChanged()
		{
			//	Is the pane being made active?
			if(m_bPaneVisible == true)
			{
				//	Make sure the tools are enabled/disabled
				SetToolStates(false);
			}
			else
			{
			}

		}// protected override void OnPaneVisibleChanged()

		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);

		}// protected override void Dispose( bool disposing )

		/// <summary>Required method for Designer support</summary>
		protected override void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("MainToolbar");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Add");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Repeat");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("DesignationOnly", "");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ChooseColumns");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("DepositionLabel");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Add");
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ContextMenu");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Add");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Repeat");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("DesignationOnly", "");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ChooseColumns");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Delete");
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool3 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("DesignationOnly", "");
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ChooseColumns");
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Repeat");
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("DepositionLabel");
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CObjectionsPane));
			this.m_ultraToolbarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
			this.m_ctrlToolbarImages = new System.Windows.Forms.ImageList(this.components);
			this._CObjectionsPane_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CObjectionsPane_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CObjectionsPane_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CObjectionsPane_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this.m_ctrlGrid = new FTI.Trialmax.Controls.CTmaxObjectionGridCtrl();
			((System.ComponentModel.ISupportInitialize)(this.m_ultraToolbarManager)).BeginInit();
			this.SuspendLayout();
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
			stateButtonTool1.InstanceProps.IsFirstInGroup = true;
			stateButtonTool1.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			labelTool1.InstanceProps.IsFirstInGroup = true;
			ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool2,
            buttonTool3,
            stateButtonTool1,
            buttonTool4,
            labelTool1});
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
			appearance1.Image = 0;
			buttonTool5.SharedProps.AppearancesSmall.Appearance = appearance1;
			buttonTool5.SharedProps.Caption = "&Add Objection(s) ...";
			buttonTool5.SharedProps.ToolTipText = "Add";
			popupMenuTool1.SharedProps.Caption = "ContextMenu";
			buttonTool6.InstanceProps.IsFirstInGroup = true;
			stateButtonTool2.InstanceProps.IsFirstInGroup = true;
			stateButtonTool2.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			popupMenuTool1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool6,
            buttonTool7,
            buttonTool8,
            stateButtonTool2,
            buttonTool9});
			appearance2.Image = 1;
			buttonTool10.SharedProps.AppearancesSmall.Appearance = appearance2;
			buttonTool10.SharedProps.Caption = "&Delete Objection";
			stateButtonTool3.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			appearance3.Image = 2;
			stateButtonTool3.SharedProps.AppearancesSmall.Appearance = appearance3;
			stateButtonTool3.SharedProps.Caption = "&View Designation Only";
			stateButtonTool3.SharedProps.ToolTipText = "Toggle Designation Only";
			appearance4.Image = 4;
			buttonTool11.SharedProps.AppearancesSmall.Appearance = appearance4;
			buttonTool11.SharedProps.Caption = "&Show / Hide Fields ...";
			appearance5.Image = 5;
			buttonTool12.SharedProps.AppearancesSmall.Appearance = appearance5;
			buttonTool12.SharedProps.Caption = "Repeat Objection(s) ...";
			appearance6.TextHAlignAsString = "Left";
			labelTool2.SharedProps.AppearancesSmall.AppearanceOnToolbar = appearance6;
			labelTool2.SharedProps.Caption = "DepositionLabel";
			labelTool2.SharedProps.Spring = true;
			labelTool2.SharedProps.ToolTipText = "Active Deposition";
			this.m_ultraToolbarManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool5,
            popupMenuTool1,
            buttonTool10,
            stateButtonTool3,
            buttonTool11,
            buttonTool12,
            labelTool2});
			this.m_ultraToolbarManager.BeforeToolDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventHandler(this.OnUltraBeforeDropDown);
			this.m_ultraToolbarManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.OnUltraToolClick);
			this.m_ultraToolbarManager.AfterToolCloseup += new Infragistics.Win.UltraWinToolbars.ToolDropdownEventHandler(this.OnUltraAfterCloseup);
			this.m_ultraToolbarManager.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.OnUltraBeforeToolbarListDropdown);
			// 
			// m_ctrlToolbarImages
			// 
			this.m_ctrlToolbarImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlToolbarImages.ImageStream")));
			this.m_ctrlToolbarImages.TransparentColor = System.Drawing.Color.Magenta;
			this.m_ctrlToolbarImages.Images.SetKeyName(0, "objection_add.bmp");
			this.m_ctrlToolbarImages.Images.SetKeyName(1, "objection_delete.bmp");
			this.m_ctrlToolbarImages.Images.SetKeyName(2, "view_designation.bmp");
			this.m_ctrlToolbarImages.Images.SetKeyName(3, "view_transcript.bmp");
			this.m_ctrlToolbarImages.Images.SetKeyName(4, "choose_columns.bmp");
			this.m_ctrlToolbarImages.Images.SetKeyName(5, "objection_repeat.bmp");
			// 
			// _CObjectionsPane_Toolbars_Dock_Area_Top
			// 
			this._CObjectionsPane_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CObjectionsPane_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
			this._CObjectionsPane_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
			this._CObjectionsPane_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CObjectionsPane_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
			this._CObjectionsPane_Toolbars_Dock_Area_Top.Name = "_CObjectionsPane_Toolbars_Dock_Area_Top";
			this._CObjectionsPane_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(316, 73);
			this._CObjectionsPane_Toolbars_Dock_Area_Top.ToolbarsManager = this.m_ultraToolbarManager;
			// 
			// _CObjectionsPane_Toolbars_Dock_Area_Bottom
			// 
			this._CObjectionsPane_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CObjectionsPane_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
			this._CObjectionsPane_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
			this._CObjectionsPane_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CObjectionsPane_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 288);
			this._CObjectionsPane_Toolbars_Dock_Area_Bottom.Name = "_CObjectionsPane_Toolbars_Dock_Area_Bottom";
			this._CObjectionsPane_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(316, 0);
			this._CObjectionsPane_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.m_ultraToolbarManager;
			// 
			// _CObjectionsPane_Toolbars_Dock_Area_Left
			// 
			this._CObjectionsPane_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CObjectionsPane_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
			this._CObjectionsPane_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
			this._CObjectionsPane_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CObjectionsPane_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 73);
			this._CObjectionsPane_Toolbars_Dock_Area_Left.Name = "_CObjectionsPane_Toolbars_Dock_Area_Left";
			this._CObjectionsPane_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 215);
			this._CObjectionsPane_Toolbars_Dock_Area_Left.ToolbarsManager = this.m_ultraToolbarManager;
			// 
			// _CObjectionsPane_Toolbars_Dock_Area_Right
			// 
			this._CObjectionsPane_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CObjectionsPane_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
			this._CObjectionsPane_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
			this._CObjectionsPane_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CObjectionsPane_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(316, 73);
			this._CObjectionsPane_Toolbars_Dock_Area_Right.Name = "_CObjectionsPane_Toolbars_Dock_Area_Right";
			this._CObjectionsPane_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 215);
			this._CObjectionsPane_Toolbars_Dock_Area_Right.ToolbarsManager = this.m_ultraToolbarManager;
			// 
			// m_ctrlGrid
			// 
			this.m_ctrlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlGrid.Location = new System.Drawing.Point(0, 73);
			this.m_ctrlGrid.Name = "m_ctrlGrid";
			this.m_ctrlGrid.PaneId = 0;
			this.m_ctrlGrid.Size = new System.Drawing.Size(316, 215);
			this.m_ctrlGrid.TabIndex = 1;
			this.m_ctrlGrid.DblClick += new System.EventHandler(this.OnGridDblClick);
			this.m_ctrlGrid.Enter += new System.EventHandler(this.OnEnterGrid);
			this.m_ctrlGrid.SortChanged += new System.EventHandler(this.OnGridSortChanged);
			this.m_ctrlGrid.SelectionChanged += new System.EventHandler(this.OnGridSelectionChanged);
			this.m_ctrlGrid.SelectionChanging += new FTI.Trialmax.Controls.CTmaxObjectionGridCtrl.SelectionChangingHandler(this.OnGridSelectionChanging);
			this.m_ctrlGrid.Leave += new System.EventHandler(this.OnLeaveGrid);
			// 
			// CObjectionsPane
			// 
			this.m_ultraToolbarManager.SetContextMenuUltra(this, "ContextMenu");
			this.Controls.Add(this.m_ctrlGrid);
			this.Controls.Add(this._CObjectionsPane_Toolbars_Dock_Area_Left);
			this.Controls.Add(this._CObjectionsPane_Toolbars_Dock_Area_Right);
			this.Controls.Add(this._CObjectionsPane_Toolbars_Dock_Area_Top);
			this.Controls.Add(this._CObjectionsPane_Toolbars_Dock_Area_Bottom);
			this.Name = "CObjectionsPane";
			this.Size = new System.Drawing.Size(316, 288);
			((System.ComponentModel.ISupportInitialize)(this.m_ultraToolbarManager)).EndInit();
			this.ResumeLayout(false);

		}// protected override void InitializeComponent()

		/// <summary>Overloaded method called when the window is loaded the first time</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Set the initial states of the toolbar buttons
			SetToolStates(false);

			//	Do the base class processing
			base.OnLoad(e);

		}// protected override void OnLoad(EventArgs e)

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			base.SetErrorStrings();

			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while handling notification that objections have been added");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to delete objections");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to add objections");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the active objection");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fire the objection selected command event");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fill the objections grid");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to change the Show Designation Only state");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to set the active designation.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to check the visibility of the objection.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fire the navigator changed event.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while handling the request to move the navigator.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while handling the request to move the navigator.");

		}// protected override void SetErrorStrings()

		/// <summary>Overridden by derived classes to provide access to the pane's toolbar manager</summary>
		/// <returns>The pane's toolbar manager if available</returns>
		protected override Infragistics.Win.UltraWinToolbars.UltraToolbarsManager GetUltraToolbarManager()
		{
			return m_ultraToolbarManager;
		}

		#endregion Protected Methods

		#region Private Methods

		/// <summary>This method is called to make the specified object the active objection</summary>
		/// <param name="tmaxObjection">The objection to be activated</param>
		/// <param name="bOpenTranscript">true to open the transcripts pane</param>
		/// <returns>true if successful</returns>
		private bool Activate(CTmaxObjection tmaxObjection, bool bOpenTranscript)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Update the toolbar button states
				SetToolStates(false);

				//	Notify the application
				FireObjectionSelected(tmaxObjection, bOpenTranscript);
				FireNavigatorChanged();

				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Activate", m_tmaxErrorBuilder.Message(ERROR_ACTIVATE_EX), Ex);
			}
			
			return bSuccessful;

		}// private bool Activate(CTmaxObjection tmaxObjection)

		/// <summary>This method is called to convert the key to a command identifier</summary>
		/// <param name="strKey">The Infragistic key identifier</param>
		/// <returns>The associated pane command</returns>
		private ObjectionsPaneCommands GetCommand(string strKey)
		{
			try
			{
				Array aCommands = Enum.GetValues(typeof(ObjectionsPaneCommands));

				foreach(ObjectionsPaneCommands eCommand in aCommands)
				{
					if(eCommand.ToString() == strKey)
						return eCommand;
				}

			}
			catch
			{
			}

			return ObjectionsPaneCommands.Invalid;

		}// private ObjectionsPaneCommands GetCommand(string strKey)

		/// <summary>This method is called to determine if the specified command should be enabled</summary>
		/// <param name="eCommand">The transcript pane command enumeration</param>
		/// <param name="iSelections">The number of rows currently selected</param>
		/// <returns>true if command should be enabled</returns>
		private bool GetCommandEnabled(ObjectionsPaneCommands eCommand, int iSelections)
		{
			//	We have to have an active database with codes
			if(m_tmaxDatabase == null) return false;
			if(m_tmaxDatabase.Primaries == null) return false;
			if(m_tmaxDatabase.ObjectionsEnabled == false) return false;

			//	What is the command?
			switch(eCommand)
			{
				case ObjectionsPaneCommands.Add:
				case ObjectionsPaneCommands.Repeat:
				case ObjectionsPaneCommands.DesignationOnly:
				case ObjectionsPaneCommands.ChooseColumns:
					return true;
					
				case ObjectionsPaneCommands.Delete:
					return (iSelections > 0);
					
				default:

					break;

			}// switch(eCommand)	

			return false;

		}// private bool GetCommandEnabled(ObjectionsPaneCommands eCommand, int iSelections)

		/// <summary>This method is called to get the shortcut key for the specified command</summary>
		/// <param name="eCommand">The tree pane command enumeration</param>
		/// <returns>The shortcut key if there is one</returns>
		private Shortcut GetCommandShortcut(ObjectionsPaneCommands eCommand)
		{
			switch(eCommand)
			{
				case ObjectionsPaneCommands.Add:

					return Shortcut.CtrlJ;

				case ObjectionsPaneCommands.Delete:

					return Shortcut.ShiftDel;

				default:

					return Shortcut.None;

			}// switch(eCommand)

		}// protected virtual Shortcut GetCommandShortcut(ObjectionsPaneCommands eCommand)

		/// <summary>This method processes the specified command</summary>
		/// <param name="eCommand">The enumerated command identifier</param>
		private void OnCommand(ObjectionsPaneCommands eCommand)
		{

			try
			{
				//	Direct to the appropriate handler
				switch(eCommand)
				{
					case ObjectionsPaneCommands.Add:
					case ObjectionsPaneCommands.Repeat:

						OnCmdAdd(eCommand == ObjectionsPaneCommands.Repeat);
						break;


					case ObjectionsPaneCommands.Delete:

						OnCmdDelete();
						break;

					case ObjectionsPaneCommands.DesignationOnly:

						OnCmdDesignationOnly();
						break;

					case ObjectionsPaneCommands.ChooseColumns:

						OnCmdChooseColumns();
						break;

					default:

						break;

				}// switch(eCommand)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnCommand", Ex);
			}

		}// private void OnCommand(ObjectionsPaneCommands eCommand)

		/// <summary>This method handles all Hotkey notifications from the application</summary>
		/// <param name="eHotkey">The enumerated hotkey</param>
		/// <returns>true if processed by the pane</returns>
		public override bool OnHotkey(TmaxHotkeys eHotkey)
		{
			ObjectionsPaneCommands	eCommand = ObjectionsPaneCommands.Invalid;
			int						iSelections = 0;

			//	Which hotkey has been pressed?
			switch(eHotkey)
			{
				case TmaxHotkeys.AddObjection:

					eCommand = ObjectionsPaneCommands.Add;
					break;

				case TmaxHotkeys.RepeatObjection:

					eCommand = ObjectionsPaneCommands.Repeat;
					break;

				case TmaxHotkeys.Delete:

					eCommand = ObjectionsPaneCommands.Delete;
					break;

				default:

					break;
			}

			//	Did this hotkey translate to a command?
			if(eCommand != ObjectionsPaneCommands.Invalid)
			{
				//	Get the current number of row selections
				if((m_ctrlGrid != null) && (m_ctrlGrid.IsDisposed == false))
					iSelections = m_ctrlGrid.GetSelectedCount();

				//	Is this command enabled
				if(GetCommandEnabled(eCommand, iSelections) == true)
				{
					//	Prompt for confirmation if attempting to delete records
					if((eCommand != ObjectionsPaneCommands.Delete) || (GetDeleteConfirmation() == true))
						OnCommand(eCommand);
				}

			}// if(eCommand != ObjectionsPaneCommands.Invalid)

			return (eCommand != ObjectionsPaneCommands.Invalid);

		}// public override bool OnHotkey(TmaxHotkeys eHotkey)

		/// <summary>Event handler for the Add command</summary>
		/// <param name="bRepeat">True to repeat last used argument</param>
		private void OnCmdAdd(bool bRepeat)
		{
			try
			{
				AddObjection(m_dxDeposition, true, bRepeat);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdAdd", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_ADD_EX), Ex);
			}

		}// private void OnCmdAdd()

		/// <summary>Event handler for the Delete command</summary>
		private void OnCmdDelete()
		{
			CTmaxItem		tmaxItem = null;
			CTmaxParameters tmaxParameters = null;
			CTmaxObjections	tmaxSelections = null;

			try
			{
				//	Are any objections selected?
				if((tmaxSelections = m_ctrlGrid.GetSelected()) != null)
				{
					tmaxItem = new CTmaxItem();
					tmaxItem.DataType = TmaxDataTypes.Objection;
					
					foreach(CTmaxObjection O in tmaxSelections)
					{
						tmaxItem.SubItems.Add(new CTmaxItem(O));
					}

					//	Fire the command to delete the objections
					if(tmaxItem.SubItems.Count > 0)
					{
						tmaxParameters = new CTmaxParameters();
						tmaxParameters.Add(TmaxCommandParameters.Objections, true);

						FireCommand(TmaxCommands.Delete, tmaxItem, tmaxParameters);
					}

				}// if((tmaxSelections = m_ctrlGrid.GetSelected()) != null)
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdDelete", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_DELETE_EX), Ex);
			}

		}// private void OnCmdDelete()

		/// <summary>This method handles the event fired when the user clicks on ChooseColumns from the context menu</summary>
		private void OnCmdChooseColumns()
		{
			try
			{
				m_ctrlGrid.ChooseColumns();
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnCmdChooseColumns", Ex);
			}

		}// private void OnCmdPreferences(CScriptScenes Scenes)

		/// <summary>This method handles events fired by the toolbar manager after displaying the popup menu</summary>
		/// <param name="sender">The toolbar manager firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnUltraAfterCloseup(object sender, Infragistics.Win.UltraWinToolbars.ToolDropdownEventArgs e)
		{
			ObjectionsPaneCommands eCommand = ObjectionsPaneCommands.Invalid;

			//	Check each tool in the manager's collection
			foreach(ToolBase O in m_ultraToolbarManager.Tools)
			{
				//	Get the command for this tool
				if((eCommand = GetCommand(O.Key)) != ObjectionsPaneCommands.Invalid)
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
			ObjectionsPaneCommands eCommand = ObjectionsPaneCommands.Invalid;

			//	Don't bother if ignoring events
			if(m_bIgnoreUltraEvents == true) return;

			//	Get the command
			if(e.Tool != null && e.Tool.Key != null)
				eCommand = GetCommand(e.Tool.Key);

			if(eCommand != ObjectionsPaneCommands.Invalid)
				OnCommand(eCommand);

		}// OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)

		/// <summary>This method is called to enable/disable the tools in the manager's collection</summary>
		/// <param name="bShortcuts">true to assign shortcuts</param>
		private void SetToolStates(bool bShortcuts)
		{
			ObjectionsPaneCommands	eCommand = ObjectionsPaneCommands.Invalid;
			int						iSelections = 0;
			Shortcut				eShortcut = Shortcut.None;

			Debug.Assert(m_ultraToolbarManager != null);
			Debug.Assert(m_ultraToolbarManager.Tools != null);
			if((m_ultraToolbarManager == null) || (m_ultraToolbarManager.Tools == null)) return;

			//	Get the current number of row selections
			if((m_ctrlGrid != null) && (m_ctrlGrid.IsDisposed == false))
				iSelections = m_ctrlGrid.GetSelectedCount();

			//	Check each tool in the manager's collection
			foreach(ToolBase O in m_ultraToolbarManager.Tools)
			{
				if(O.Key == null) continue;
			
				try
				{
					//	Do we have a valid command?
					eCommand = GetCommand(O.Key);
					if(eCommand == ObjectionsPaneCommands.Invalid)
					{
						//	Is this the deposition label?
						if(O.Key == "DepositionLabel")
						{
							if(m_dxDeposition != null)
							{
								O.SharedProps.Caption = m_dxDeposition.MediaId;
								if(m_dxDeposition.Name.Length > 0)
									O.SharedProps.Caption += (" - " + m_dxDeposition.Name);
							}
							else
							{
								O.SharedProps.Caption = "";
							}

						}// if(O.Key == "DepositionLabel")
						
						continue;

					}// if(eCommand == ObjectionsPaneCommands.Invalid)
					
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
						case ObjectionsPaneCommands.DesignationOnly:

							m_bIgnoreUltraEvents = true;
							((StateButtonTool)O).Checked = m_bDesignationOnly;
							m_bIgnoreUltraEvents = false;

							if(m_bDesignationOnly == true)
								O.SharedProps.AppearancesSmall.Appearance.Image = 2;
							else
								O.SharedProps.AppearancesSmall.Appearance.Image = 3;
							break;

							
						default:
						
							break;
							
					}// switch(eCommand)
						
				}
				catch
				{
				}

			}// foreach(ToolBase ultraTool in m_ultraToolbarManager.Tools)

		}// private void SetToolStates()

		/// <summary>This function handles events fired by the grid when the selection has changed</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Event argument</param>
		private void OnGridSelectionChanged(object sender, EventArgs e)
		{//FTI.Shared.Win32.User.MessageBeep(0);
			Activate(m_ctrlGrid.GetSelection(), false);
		}

		/// <summary>This function handles events fired by the grid when the selection is changing</summary>
		/// <param name="sender">The object firing the event</param>
		private bool OnGridSelectionChanging(object sender)
		{
			bool bCancel = false;
			
			//	Should we cancel the change?
			if(m_bCancelSelChange == true)
			{
				bCancel = true;
			}
			else
			{
				//	Check for changes unless it was already done in OnLeaveEditor()
				if(m_bCheckForChanges == true)
					bCancel = CheckForChanges();
			}

			//m_tmaxEventSource.FireDiagnostic(this, "OnGridSelectionChanging", "CHANGING -> Cancel = " + bCancel.ToString());
			
			//	Clear these flags now that we've handled the event
			m_bCheckForChanges = true;
			m_bCancelSelChange = false;
			
			return bCancel;
			
		}// private bool OnGridSelectionChanging(object sender)

		/// <summary>Handles DblClick events fired by the child grid control</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">the system event arguments</param>
		private void OnGridDblClick(object sender, EventArgs e)
		{
			//	Activate the current selection and open the transcripts pane
			Activate(m_ctrlGrid.GetSelection(), true);
		}

		/// <summary>This function handles events fired by the grid when the sort order or column changes</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Event argument</param>
		private void OnGridSortChanged(object sender, EventArgs e)
		{
			FireNavigatorChanged();
		}
		
		/// <summary>This function handles events fired when the editor looses focus</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Event argument</param>
		private void OnLeaveEditor(object sender, EventArgs e)
		{
			//m_tmaxEventSource.FireDiagnostic(this, "OnLeaveEditor", "Leave Editor");
			
			//	Check to see if the active objection has been modified
			CheckForChanges();

		}// private void OnLeaveEditor(object sender, EventArgs e)

		/// <summary>This function handles events fired when the editor gains focus</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Event argument</param>
		private void OnEnterEditor(object sender, EventArgs e)
		{
			//m_tmaxEventSource.FireDiagnostic(this, "OnEnterEditor", "Enter Editor");

		}// private void OnEnterEditor(object sender, EventArgs e)

		/// <summary>This function handles events fired when the editor looses focus</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Event argument</param>
		private void OnLeaveGrid(object sender, EventArgs e)
		{
			//m_tmaxEventSource.FireDiagnostic(this, "OnLeaveGrid", "Leave Grid");

		}// private void OnLeaveGrid(object sender, EventArgs e)

		/// <summary>This function handles events fired when the editor gains focus</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Event argument</param>
		private void OnEnterGrid(object sender, EventArgs e)
		{
			//m_tmaxEventSource.FireDiagnostic(this, "OnEnterGrid", "Enter Grid");

		}// private void OnEnterGrid(object sender, EventArgs e)

		/// <summary>This function checks to see if the active objection should be updated</summary>
		/// <returns>true if the selection change operation should be cancelled</returns>
		private bool CheckForChanges()
		{
			//CTmaxItem tmaxItem = null;
			//CTmaxParameters tmaxParameters = null;

			//	Clear the flags to inhibit selection change processing
			m_bCheckForChanges = true;
			m_bCancelSelChange = false;
			
			////	Has the user modified the active objection?
			//if((m_ctrlEditor.Objection != null) && (m_ctrlEditor.IsModified() == true))
			//{
			//    //m_tmaxEventSource.FireDiagnostic(this, "CheckForChanges", "Setting Properties");

			//    //	Set the new values
			//    if(m_ctrlEditor.SetProps(m_ctrlEditor.Objection, false, true) == true)
			//    {
			//        tmaxItem = new CTmaxItem(m_ctrlEditor.Objection.IOxObjection);

			//        tmaxParameters = new CTmaxParameters();
			//        tmaxParameters.Add(TmaxCommandParameters.Objections, true);

			//        FireCommand(TmaxCommands.Update, tmaxItem, tmaxParameters);
				
			//        //	No need to check for changes when the selection changes
			//        m_bCheckForChanges = false;
			//    }
			//    else
			//    {
			//        //	Did the user keep the invalid changes?
			//        if(m_ctrlEditor.Cancelled == false)
			//            m_bCancelSelChange = true; // don't switch to a new selection in the grid
						
			//        //m_tmaxEventSource.FireDiagnostic(this, "CheckForChanges", "Cancelled = " + m_ctrlEditor.Cancelled.ToString());
			//    }

			//}
			//else
			//{
			//    //m_tmaxEventSource.FireDiagnostic(this, "CheckForChanges", "No Modifications");

			//}// if(m_ctrlEditor.IsModified() == true)

			return m_bCancelSelChange;

		}// private bool CheckForChanges()

		/// <summary>Called to fire the command event to indicate that the user has selected a new objection</summary>
		/// <param name="tmaxObjection">The objection to be activated</param>
		/// <param name="bOpenTranscript">true to open the transcripts pane</param>
		/// <returns>true if successful</returns>
		private bool FireObjectionSelected(CTmaxObjection tmaxObjection, bool bOpenTranscript)
		{
			bool			bSuccessful = false;
			CTmaxItem		tmaxItem = null;
			CTmaxParameters	tmaxParameters = null;

			try
			{
				//	Allocate an event item to represent the objection
				tmaxItem = new CTmaxItem(tmaxObjection);
				
				//	Add a parameter to identify this as an objection operation
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.Objections, true));

				//	Are we forcing the transcript pane to open?
				if(bOpenTranscript == true)
					tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.Transcripts, true));
					
				bSuccessful = FireCommand(TmaxCommands.Activate, tmaxItem, tmaxParameters);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FireObjectionSelected", m_tmaxErrorBuilder.Message(ERROR_FIRE_OBJECTION_SELECTED_EX), Ex);
			}

			return bSuccessful;

		}// private bool FireObjectionSelected(CTmaxObjection tmaxObjection)

		/// <summary>Called to fire the command event to indicate that the navigator properties have changed</summary>
		/// <returns>true if successful</returns>
		private bool FireNavigatorChanged()
		{
			bool			bSuccessful = false;
			CTmaxObjection	tmaxSelection = null;
			CTmaxItem		tmaxItem = null;
			CTmaxParameters tmaxParameters = null;
			int				iIndex = -1;
			int				iTotal = 0;

			try
			{
				//	Do we have any active objections?
				if((m_tmaxObjections != null) && (m_tmaxObjections.Count > 0))
				{
					iTotal = m_tmaxObjections.Count;
					
					//	Get the current selection
					if((tmaxSelection = m_ctrlGrid.GetSelection()) != null)
					{
						//	Get the index of the current selection
						iIndex = m_ctrlGrid.GetIndex(tmaxSelection);
						
						//	Allocate an event item to represent the objection
						tmaxItem = new CTmaxItem(tmaxSelection);

					}// if((tmaxObjection = m_ctrlGrid.GetSelection()) != null)

				}// if((m_tmaxObjections != null) && (m_tmaxObjections.Count > 0))
				
				//	Populate the parameters collection
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.Objections, true));
				tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.NavigatorIndex, iIndex));
				tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.NavigatorTotal, iTotal));
				tmaxParameters.Add(new CTmaxParameter(TmaxCommandParameters.NavigatorMode, m_bDesignationOnly));
				bSuccessful = FireCommand(TmaxCommands.NavigatorChanged, tmaxItem, tmaxParameters);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FireNavigatorChanged", m_tmaxErrorBuilder.Message(ERROR_FIRE_NAVIGATOR_CHANGED_EX), Ex);
			}

			return bSuccessful;

		}// private bool FireNavigatorChanged()
		
		/// <summary>This function is called to fill the grid</summary>
		/// <returns>true if successful</returns>
		private bool Fill()
		{
			bool			bSuccessful = false;
			CTmaxObjection	tmaxSelection = null;
			
			try
			{
				//	Get the current selection if there is one
				tmaxSelection = m_ctrlGrid.GetSelection();
				
				//	Flush the existing collection
				m_tmaxObjections.Clear();
				
				//	Refill the collection
				if((m_tmaxDatabase != null) && (m_tmaxDatabase.Objections != null))
				{
					//	Do we have an active deposition?
					if(m_dxDeposition != null)
					{ 
						//	Are we viewing only those for the active designation?
						if(m_bDesignationOnly == true)
						{
							//	Do we have a valid designation?
							if((m_dxDesignation != null) && (ReferenceEquals(m_dxDesignation.Secondary.Primary, m_dxDeposition) == true))
							{
								//	Get the objections for this deposition
								m_tmaxDatabase.Objections.FindAll(m_tmaxObjections, m_dxDeposition, m_dxDesignation.StartPL, m_dxDesignation.StopPL);
							}
							
						}
						else
						{
							//	Get the objections for this deposition
							m_tmaxDatabase.Objections.FindAll(m_tmaxObjections, m_dxDeposition);
						}

					}// if(m_dxDeposition != null)
	
				}// if((m_tmaxDatabase != null) && (m_tmaxDatabase.Objections != null))
				
				//	Update the child controls
				if(m_tmaxObjections.Count > 0)
				{
					m_ctrlGrid.Add(m_tmaxObjections, true);
					
					//	Is the previous selection still in the list?
					if((tmaxSelection != null) && (m_tmaxObjections.Contains(tmaxSelection) == true))
						m_ctrlGrid.SetSelection(tmaxSelection);
					else
						m_ctrlGrid.SetSelection(0);
				}
				else
				{
					m_ctrlGrid.Clear();
					FireNavigatorChanged();

				}// if(m_tmaxObjections.Count > 0)
				
				bSuccessful = true;			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Fill", m_tmaxErrorBuilder.Message(ERROR_FILL_EX), Ex);
			}

			//	Make sure the tools are enabled/disabled
			SetToolStates(false);
			
			return bSuccessful;

		}// private bool Fill()

		/// <summary>This function is called to clear the objections in the grid</summary>
		private void Clear()
		{
			try
			{
				m_ctrlGrid.Clear();
				m_dxDeposition = null;
				m_dxScene = null;
				m_dxDesignation = null;
				m_tmaxObjections.Clear();
				
				FireNavigatorChanged();
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "Clear", Ex);
			}

		}// private void Clear()

		/// <summary>Event handler for the DesignationOnly command</summary>
		private void OnCmdDesignationOnly()
		{
			try
			{
				//	Toggle the state of the unassigned codes
				m_bDesignationOnly = !m_bDesignationOnly;
				
				//	Refill the grid if we have a valid deposition
				if(m_dxDeposition != null)
				{
					Fill();
				}
				else
				{
					//	Notify the properties pane that the mode has changed
					FireNavigatorChanged();
				}

					
				//	Make sure the toolbar button has the right image
				SetToolStates(false);

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdDesignationOnly", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_SHOW_DESIGNATION_EX), Ex);
			}

		}// private void OnCmdShowDesignation()

		/// <summary>This method is called to set the active designation</summary>
		/// <param name="dxScene">The scene that owns the designation</param>
		private void SetDesignation(CDxSecondary dxScene)
		{
			CDxTertiary dxDesignation = null;
			
			try
			{
				if((dxScene != null) && (dxScene.GetSource() != null))
				{
					if(dxScene.GetSource().MediaType == TmaxMediaTypes.Designation)
						dxDesignation = (CDxTertiary)(dxScene.GetSource());
				}
				
				//	Has the designation changed?
				if(ReferenceEquals(dxDesignation, m_dxDesignation) == false)
				{
					//	Update the class members
					m_dxScene = dxScene;
					m_dxDesignation = dxDesignation;

					//	Rebuild the list if viewing only designation objections
					if(m_bDesignationOnly == true)
						Fill();
						
				}// if(ReferenceEquals(dxDesignation, m_dxDesignation) == false)
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetDesignation", m_tmaxErrorBuilder.Message(ERROR_SET_DESIGNATION_EX), Ex);
			}

		}// private void SetDesignation(CDxSecondary dxScene)

		/// <summary>This method is called to determine if the specified objection should be made visible in the grid</summary>
		/// <param name="tmaxObjection">The objection to be evaluated</param>
		/// <returns>True if visible</returns>
		private bool CheckVisible(CTmaxObjection tmaxObjection)
		{
			bool bVisible = false;
			
			Debug.Assert(tmaxObjection != null);
			
			try
			{
				//	Make sure we have the parent deposition for this objection
				if(tmaxObjection.ICaseDeposition == null)
				{
					if(tmaxObjection.Deposition.Length > 0)
						tmaxObjection.ICaseDeposition = m_tmaxDatabase.Primaries.Find(tmaxObjection.Deposition);
					
					//	Were we successful?
					if(tmaxObjection.ICaseDeposition == null)
					{
						m_tmaxEventSource.FireDiagnostic(this, "CheckVisible", "no case deposition available");
						return false;
					}

				}// if(tmaxObjection.ICaseDeposition == null)
				
				//	Do the deposition's match?
				if((m_dxDeposition != null) && (ReferenceEquals(m_dxDeposition, tmaxObjection.ICaseDeposition) == true))
				{
					//	Are we displaying only for the active designation?
					if(m_bDesignationOnly == true)
					{
						//	Verify this objection falls within the range defined by the designation
						if(m_dxDesignation != null)
						{
							if((m_dxDesignation.StartPL > 0) && (m_dxDesignation.StopPL > 0))
							{
								if((tmaxObjection.FirstPL <= m_dxDesignation.StopPL) && (tmaxObjection.LastPL >= m_dxDesignation.StartPL))
								{
									bVisible = true;
								}

							}
							else
							{
								bVisible = true;

							}// if((lFirstPL > 0) && (lLastPL > 0))

						}// if(m_dxDesignation != null)

					}
					else
					{
						//	Depositions match so it's OK to display this objection
						bVisible = true;
					}

				}// if((m_dxDeposition != null) && (ReferenceEquals(m_dxDeposition, tmaxObjection.ICaseDeposition) == true))

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CheckVisible", m_tmaxErrorBuilder.Message(ERROR_CHECK_VISIBLE_EX), Ex);
			}
			
			return bVisible;

		}// private bool CheckVisible(CTmaxObjection tmaxObjection)

		#endregion Private Methods

	}// private void OnGridDblClick(object sender, EventArgs e)

}// namespace FTI.Trialmax.Panes
