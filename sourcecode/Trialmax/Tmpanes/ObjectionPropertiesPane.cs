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
	public class CObjectionPropertiesPane : FTI.Trialmax.Panes.CBasePane
	{
		#region Constants

		/// <summary>Local context menu command identifiers</summary>
		private enum ObjectionsPropertiesPaneCommands
		{
			Invalid = 0,
			First,
			Previous,
			Next,
			Last,
			GoTo,
			SetVisibleProps,
			DesignationOnly,
		}

		private const int ERROR_ON_CMD_DELETE_EX = (ERROR_BASE_PANE_MAX + 1);
		private const int ERROR_ON_CMD_ADD_EX = (ERROR_BASE_PANE_MAX + 2);
		private const int ERROR_ON_CMD_SHOW_ASSIGNED_EX = (ERROR_BASE_PANE_MAX + 3);
		private const int ERROR_FILL_EX = (ERROR_BASE_PANE_MAX + 4);
		private const int ERROR_FIRE_SET_CODES_EX = (ERROR_BASE_PANE_MAX + 5);
		private const int ERROR_ON_BEFORE_UPDATE_EX = (ERROR_BASE_PANE_MAX + 6);
		private const int ERROR_ADD_PICK_VALUE_EX = (ERROR_BASE_PANE_MAX + 7);
		private const int ERROR_ON_CMD_FILTER_EX = (ERROR_BASE_PANE_MAX + 8);
		private const int ERROR_ON_CMD_ADD_ANOTHER_EX = (ERROR_BASE_PANE_MAX + 9);
		private const int ERROR_ON_CMD_NAVIGATE_EX = (ERROR_BASE_PANE_MAX + 10);
		private const int ERROR_ACTIVATE_EX = (ERROR_BASE_PANE_MAX + 11);
		private const int ERROR_ON_OBJECTIONS_DELETED_EX = (ERROR_BASE_PANE_MAX + 12);
		private const int ERROR_ON_OBJECTIONS_UPDATED_EX = (ERROR_BASE_PANE_MAX + 13);
		private const int ERROR_UPDATE_PROPERTIES_EX = (ERROR_BASE_PANE_MAX + 14);
		private const int ERROR_VALIDATE_EX = (ERROR_BASE_PANE_MAX + 15);
		private const int ERROR_FIRE_UPDATE_EX = (ERROR_BASE_PANE_MAX + 16);
		private const int ERROR_ON_NAVIGATOR_CHANGED_EX = (ERROR_BASE_PANE_MAX + 17);
		private const int ERROR_ON_CMD_SET_VISIBLE_PROPS_EX = (ERROR_BASE_PANE_MAX + 18);
		private const int ERROR_ON_CMD_DESIGNATION_ONLY_EX = (ERROR_BASE_PANE_MAX + 19);

		#endregion Constants

		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>Infragistics toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager m_ultraToolbarManager;

		/// <summary>Infragistics toolbar manager left docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CObjectionPropertiesPane_Toolbars_Dock_Area_Left;

		/// <summary>Infragistics toolbar manager right docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CObjectionPropertiesPane_Toolbars_Dock_Area_Right;

		/// <summary>Infragistics toolbar manager top docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CObjectionPropertiesPane_Toolbars_Dock_Area_Top;

		/// <summary>Infragistics toolbar manager bottom docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CObjectionPropertiesPane_Toolbars_Dock_Area_Bottom;

		/// <summary>Image list assigned to the toolbar manager</summary>
		private ImageList m_ctrlToolbarImages;

		/// <summary>Property grid control used to display and edit codes</summary>
		private FTI.Trialmax.Controls.CTmaxPropGridCtrl m_ctrlPropGrid;

		/// <summary>Master collection of all available properties</summary>
		private CTmaxProperties m_tmaxMasterProps = new CTmaxProperties();

		/// <summary>Collection of visible properties</summary>
		private CTmaxProperties m_tmaxVisibleProps = new CTmaxProperties();

		/// <summary>A reference to the active objection object</summary>
		private CTmaxObjection m_tmaxObjection = null;

		/// <summary>A reference to the objection to be activated when the pane becomes visible</summary>
		private CTmaxObjection m_tmaxActivate = null;

		/// <summary>Local member to store a flag to inhibit handling of update notifications from the application</summary>
		private bool m_bIgnoreUpdates = false;

		/// <summary>Local member to keep track of the objection grid's Designation Only mode</summary>
		private bool m_bDesignationOnly = false;

		/// <summary>Local member to to keep track of the index of the current record in the navigation collection</summary>
		private int m_iNavigatorIndex = -1;

		/// <summary>Local member to to keep track of total records in the navigation collection</summary>
		private int m_iNavigatorTotal = -1;

		/// <summary>Available party values</summary>
		private ArrayList m_aParties = new ArrayList();

		#endregion Private Members

		#region Public Methods

		/// <summary>Constructor</summary>
		public CObjectionPropertiesPane() : base()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			
			m_aParties.Add(CTmaxObjection.TMAX_OBJECTION_PARTY_DEFENDANT);
			m_aParties.Add(CTmaxObjection.TMAX_OBJECTION_PARTY_PLAINTIFF);

			//	Initialize the property grid
			m_tmaxEventSource.Attach(m_ctrlPropGrid.EventSource);
			m_ctrlPropGrid.DropListBoolean = true;
			m_ctrlPropGrid.Initialize(null);
			
			//	Initialize the property collections
			FillMasterProps();

		}// public CObjectionPropertiesPane()

		/// <summary>This method is called by the application to initialize the pane</summary>
		/// <returns>true if successful</returns>
		public override bool Initialize(CXmlIni xmlINI)
		{
			string strKey = "";
			
			//	Do the base class initialization first
			if(base.Initialize(xmlINI) == false)
				return false;

			//	Make sure we are on the correct section
			xmlINI.SetSection(m_strPaneName);

			foreach(CTmaxProperty O in m_tmaxMasterProps)
			{
				strKey = O.Name.Replace(" ", "_");
				
				O.Visible = xmlINI.ReadBool(strKey, O.Visible);
			}

			return true;

		}// public override bool Initialize()

		/// <summary>This method is called by the application prior to closing the active database</summary>
		/// <returns>True if OK to close the database</returns>
		public override bool CanCloseDatabase()
		{
			try 
			{
				//	Make sure all changes have been committed
				if(m_tmaxObjection != null)
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
			string strKey = "";
			
			//	Make sure we are on the correct section
			xmlINI.SetSection(m_strPaneName);

			//	Clear the property grid control
			m_ctrlPropGrid.EndUserUpdate(true);
			m_ctrlPropGrid.Clear(true);

			foreach(CTmaxProperty O in m_tmaxMasterProps)
			{
				strKey = O.Name.Replace(" ", "_");
				xmlINI.Write(strKey, O.Visible);
			}
				
			//	Do the base class processing
			base.Terminate(xmlINI);

		}// public override void Terminate(CXmlIni xmlINI)

		/// <summary>This method is called by the application to notify the panes to refresh their text</summary>
		public override void RefreshText()
		{
			if(m_tmaxObjection != null)
				UpdateProperties();
		}

		/// <summary>This method is called by the application when a pane wants to update a navigator's position</summary>
		/// <param name="ePane">The pane firing the notification</param>
		/// <param name="tmaxItems">The items passed with the event</param>
		/// <param name="tmaxParameters">The parameters passed with the event</param>
		public override void OnNavigatorChanged(TmaxAppPanes ePane, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)
		{
			CTmaxParameter	tmaxParameter = null;
			bool			bObjections	= false;
			int				iIndex = -1;
			int				iTotal = 0;
			CTmaxObjection	tmaxObjection = null;
			
			try
			{
				if(tmaxParameters != null)
				{
					if((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.Objections)) != null)
						bObjections = tmaxParameter.AsBoolean();
				}
				
				while(true)
				{
					//	Don't bother if this is not the objections navigator
					if(bObjections == false)
						break;
						
					//	Get the new navigator information
					if((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.NavigatorTotal)) != null)
						iTotal = tmaxParameter.AsInteger();
					if((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.NavigatorIndex)) != null)
						iIndex = tmaxParameter.AsInteger();
					if((tmaxParameter = tmaxParameters.Find(TmaxCommandParameters.NavigatorMode)) != null)
						m_bDesignationOnly = tmaxParameter.AsBoolean();
					
					//	Update the class members
					m_iNavigatorIndex = iIndex;
					m_iNavigatorTotal = iTotal;
					
					//	Did the navigator specify the current selection?
					if((tmaxItems != null) && (tmaxItems.Count == 1))
						tmaxObjection = tmaxItems[0].Objection;
						
					if(ReferenceEquals(tmaxObjection, m_tmaxObjection) == false)
						Activate(tmaxObjection);
					
					//	We're done
					break;

				}// while(true)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnNavigatorChanged", m_tmaxErrorBuilder.Message(ERROR_ON_NAVIGATOR_CHANGED_EX), Ex);
			}
			finally
			{
				SetToolStates();
			}

		}// public override void OnNavigatorChanged(TmaxAppPanes ePane, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)

		/// <summary>This method is called by the application when objections have been deleted</summary>
		/// <param name="tmaxItems">The objections that have been deleted</param>
		public override void OnObjectionsDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			try
			{
				foreach(CTmaxItem O in tmaxItems)
				{
					//	Don't bother if nothing loaded and nothing pending
					if((m_tmaxActivate == null) && (m_tmaxObjection == null))
						break;
						
					if(O.Objection != null)
					{
						if(ReferenceEquals(O.Objection, m_tmaxObjection) == true)
							Activate(null);
						else if(ReferenceEquals(O.Objection, m_tmaxActivate) == true)
							m_tmaxActivate = null;
					}

				}// foreach(CTmaxItem O in tmaxItems)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnObjectionsDeleted", m_tmaxErrorBuilder.Message(ERROR_ON_OBJECTIONS_DELETED_EX), Ex);
			}

		}// public override void OnObjectionsDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)

		/// <summary>This method is called by the application when multiple records have been updated in an operation</summary>
		/// <param name="tmaxItems">The items that have been updated</param>
		public override void OnObjectionsUpdated(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
			try
			{
				//	Has the active objection been updated?
				//if((m_tmaxObjection != null) && (m_bIgnoreUpdates == false))
				if(m_tmaxObjection != null)
				{
					foreach(CTmaxItem O in tmaxItems)
					{
						if((O.Objection != null) && (ReferenceEquals(O.Objection, m_tmaxObjection) == true))
						{
							//	Update the properties for this objection
							UpdateProperties();
							break;
						}

					}// foreach(CTmaxItem O in tmaxItems)

				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnObjectionsUpdated", m_tmaxErrorBuilder.Message(ERROR_ON_OBJECTIONS_UPDATED_EX), Ex);
			}
			finally
			{
				m_bIgnoreUpdates = false;
			}

		}// public override void OnObjectionsUpdated(FTI.Shared.Trialmax.CTmaxItems tmaxItems)

		/// <summary>This method is called by the application when objections has been selected by the user</summary>
		/// <param name="tmaxItem">The objection that has been selected</param>
		/// <param name="ePane">The pane that fired the selection event</param>
		public override void OnObjectionSelected(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
			if(tmaxItem.Objection != null)
			{
				m_tmaxActivate = tmaxItem.Objection;

				if((m_bPaneVisible == true) && (ReferenceEquals(m_tmaxObjection, m_tmaxActivate) == false))
				{
					Activate(m_tmaxActivate);
				}

			}
			else
			{
				Activate(null);
			}

		}// public override void OnObjectionSelected(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This function is called when the value of the Database property changes</summary>
		protected override void OnDatabaseChanged()
		{
			//	Clear out the existing information
			Activate(null);
			
			if(m_tmaxDatabase != null)
			{
				//	Make sure we are in sync with the navigator
				FireNavigate(TmaxNavigatorRequests.QueryPosition, -1);
			}

		}// protected override void OnDatabaseChanged()

		/// <summary>This function is called when the PaneVisible property changes</summary>
		protected override void OnPaneVisibleChanged()
		{
			if(m_bPaneVisible == true)
			{
				if(m_tmaxDatabase != null)
				{
					//	Make sure we are in sync with the navigator
					FireNavigate(TmaxNavigatorRequests.QueryPosition, -1);
				}

			}// if(m_bPaneVisible == true)

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
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("First");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Previous");
			Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool1 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("GoTo");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Next");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Last");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("DesignationOnly", "");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SetVisibleProps");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("Total");
			Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ContextMenu");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("First");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Previous");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Next");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Last");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("DesignationOnly", "");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SetVisibleProps");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("Spacer");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("First");
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Next");
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Previous");
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Last");
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool2 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("GoTo");
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool3 = new Infragistics.Win.UltraWinToolbars.LabelTool("Total");
			Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SetVisibleProps");
			Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool3 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("DesignationOnly", "");
			Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CObjectionPropertiesPane));
			this.m_ctrlPropGrid = new FTI.Trialmax.Controls.CTmaxPropGridCtrl();
			this.m_ultraToolbarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
			this.m_ctrlToolbarImages = new System.Windows.Forms.ImageList(this.components);
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			((System.ComponentModel.ISupportInitialize)(this.m_ultraToolbarManager)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlPropGrid
			// 
			this.m_ctrlPropGrid.BackColor = System.Drawing.Color.White;
			this.m_ctrlPropGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlPropGrid.DropListBoolean = true;
			this.m_ctrlPropGrid.Location = new System.Drawing.Point(0, 26);
			this.m_ctrlPropGrid.Name = "m_ctrlPropGrid";
			this.m_ctrlPropGrid.PaneId = 0;
			this.m_ctrlPropGrid.Size = new System.Drawing.Size(316, 262);
			this.m_ctrlPropGrid.SortOn = ((long)(0));
			this.m_ctrlPropGrid.TabIndex = 0;
			this.m_ctrlPropGrid.AfterUpdate += new FTI.Trialmax.Controls.CTmaxPropGridCtrl.TmaxPropGridHandler(this.OnGridAfterUpdate);
			this.m_ctrlPropGrid.BeforeUpdate += new FTI.Trialmax.Controls.CTmaxPropGridCtrl.TmaxPropGridHandler(this.OnGridBeforeUpdate);
			this.m_ctrlPropGrid.GetDropListValues += new FTI.Trialmax.Controls.CTmaxPropGridCtrl.TmaxPropGridHandler(this.OnGridGetDropListValues);
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
			stateButtonTool1.InstanceProps.IsFirstInGroup = true;
			stateButtonTool1.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			labelTool1.InstanceProps.Width = 25;
			ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool2,
            textBoxTool1,
            buttonTool3,
            buttonTool4,
            stateButtonTool1,
            buttonTool5,
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
			popupMenuTool1.SharedProps.Caption = "ContextMenu";
			buttonTool6.InstanceProps.IsFirstInGroup = true;
			stateButtonTool2.InstanceProps.IsFirstInGroup = true;
			stateButtonTool2.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			popupMenuTool1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool6,
            buttonTool7,
            buttonTool8,
            buttonTool9,
            stateButtonTool2,
            buttonTool10});
			appearance1.Image = 3;
			buttonTool11.SharedProps.AppearancesSmall.Appearance = appearance1;
			buttonTool11.SharedProps.Caption = "First";
			appearance2.Image = 1;
			buttonTool12.SharedProps.AppearancesSmall.Appearance = appearance2;
			buttonTool12.SharedProps.Caption = "Next";
			appearance3.Image = 0;
			buttonTool13.SharedProps.AppearancesSmall.Appearance = appearance3;
			buttonTool13.SharedProps.Caption = "Previous";
			appearance4.Image = 2;
			buttonTool14.SharedProps.AppearancesSmall.Appearance = appearance4;
			buttonTool14.SharedProps.Caption = "Last";
			appearance5.TextHAlignAsString = "Right";
			textBoxTool2.EditAppearance = appearance5;
			appearance6.BackColorDisabled = System.Drawing.SystemColors.Control;
			textBoxTool2.SharedProps.AppearancesSmall.AppearanceOnToolbar = appearance6;
			textBoxTool2.SharedProps.Caption = "Go To";
			appearance7.TextHAlignAsString = "Left";
			labelTool3.SharedProps.AppearancesSmall.Appearance = appearance7;
			labelTool3.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
			labelTool3.SharedProps.ToolTipText = "Total Records";
			appearance8.Image = 4;
			buttonTool15.SharedProps.AppearancesSmall.Appearance = appearance8;
			buttonTool15.SharedProps.Caption = "Show / Hide Fields ...";
			stateButtonTool3.MenuDisplayStyle = Infragistics.Win.UltraWinToolbars.StateButtonMenuDisplayStyle.DisplayCheckmark;
			appearance9.Image = 5;
			stateButtonTool3.SharedProps.AppearancesSmall.Appearance = appearance9;
			stateButtonTool3.SharedProps.Caption = "Designation Only";
			this.m_ultraToolbarManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool1,
            labelTool2,
            buttonTool11,
            buttonTool12,
            buttonTool13,
            buttonTool14,
            textBoxTool2,
            labelTool3,
            buttonTool15,
            stateButtonTool3});
			this.m_ultraToolbarManager.BeforeToolDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventHandler(this.OnUltraBeforeDropDown);
			this.m_ultraToolbarManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.OnUltraToolClick);
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
			this.m_ctrlToolbarImages.Images.SetKeyName(4, "choose_columns.bmp");
			this.m_ctrlToolbarImages.Images.SetKeyName(5, "view_transcript.bmp");
			this.m_ctrlToolbarImages.Images.SetKeyName(6, "view_designation.bmp");
			// 
			// _CObjectionPropertiesPane_Toolbars_Dock_Area_Top
			// 
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Top.Name = "_CObjectionPropertiesPane_Toolbars_Dock_Area_Top";
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(316, 26);
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Top.ToolbarsManager = this.m_ultraToolbarManager;
			// 
			// _CObjectionPropertiesPane_Toolbars_Dock_Area_Bottom
			// 
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 288);
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Bottom.Name = "_CObjectionPropertiesPane_Toolbars_Dock_Area_Bottom";
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(316, 0);
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.m_ultraToolbarManager;
			// 
			// _CObjectionPropertiesPane_Toolbars_Dock_Area_Left
			// 
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 26);
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Left.Name = "_CObjectionPropertiesPane_Toolbars_Dock_Area_Left";
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 262);
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Left.ToolbarsManager = this.m_ultraToolbarManager;
			// 
			// _CObjectionPropertiesPane_Toolbars_Dock_Area_Right
			// 
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(316, 26);
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Right.Name = "_CObjectionPropertiesPane_Toolbars_Dock_Area_Right";
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 262);
			this._CObjectionPropertiesPane_Toolbars_Dock_Area_Right.ToolbarsManager = this.m_ultraToolbarManager;
			// 
			// CObjectionPropertiesPane
			// 
			this.m_ultraToolbarManager.SetContextMenuUltra(this, "ContextMenu");
			this.Controls.Add(this.m_ctrlPropGrid);
			this.Controls.Add(this._CObjectionPropertiesPane_Toolbars_Dock_Area_Left);
			this.Controls.Add(this._CObjectionPropertiesPane_Toolbars_Dock_Area_Right);
			this.Controls.Add(this._CObjectionPropertiesPane_Toolbars_Dock_Area_Top);
			this.Controls.Add(this._CObjectionPropertiesPane_Toolbars_Dock_Area_Bottom);
			this.Name = "CObjectionPropertiesPane";
			this.Size = new System.Drawing.Size(316, 288);
			((System.ComponentModel.ISupportInitialize)(this.m_ultraToolbarManager)).EndInit();
			this.ResumeLayout(false);

		}// protected override void InitializeComponent()

		/// <summary>Overloaded method called when the window is loaded the first time</summary>
		/// <param name="e">System event arguments</param>
		protected override void OnLoad(EventArgs e)
		{
			//	Set the initial states of the toolbar buttons
			SetToolStates();

			//	Do the base class processing
			base.OnLoad(e);

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

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised when activating an objection.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised when objections were deleted.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised when objections were updated.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised when updating the objection.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to validate the new value.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fire the application update command.");

			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while handling the notification that the navigator has changed.");
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to toggle the Designation Only mode.");

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
			int iSequence = -1;
			TextBoxTool goTo = null;

			try
			{
				//	Get the sequence specified by the user
				if((goTo = (TextBoxTool)GetUltraTool(ObjectionsPropertiesPaneCommands.GoTo.ToString())) == null)
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
					FireNavigate(TmaxNavigatorRequests.Absolute, iSequence - 1);
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnCmdGoTo", Ex);
			}

		}// private void OnCmdGoTo()

		/// <summary>This method handles the event fired when the user clicks on Show/Hide Fields from the context menu</summary>
		private void OnCmdSetVisibleProps()
		{
			CFSetPropsVisible wndProps = null;
			
			try
			{
				if((m_tmaxMasterProps != null) && (m_tmaxMasterProps.Count > 0))
				{
					//	Allocate and initialize the form
					wndProps = new CFSetPropsVisible();
					m_tmaxEventSource.Attach(wndProps.EventSource);

					//	Assign the properties
					wndProps.Properties = m_tmaxMasterProps;
					
					if(wndProps.ShowDialog() == DialogResult.OK)
					{
						m_tmaxVisibleProps.Clear();
						if(m_tmaxObjection != null)
							Activate(m_tmaxObjection);
					}
					
				}// if((m_tmaxMasterProps != null) && (m_tmaxMasterProps.Count > 0))

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdSetVisibleProps", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_SET_VISIBLE_PROPS_EX), Ex);
			}

		}// private void OnCmdSetVisibleProps()

		/// <summary>Event handler for the DesignationOnly command</summary>
		private void OnCmdDesignationOnly()
		{
			try
			{
				FireNavigate(TmaxNavigatorRequests.SetMode, -1);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdDesignationOnly", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_DESIGNATION_ONLY_EX), Ex);
			}

		}// private void OnCmdDesignationOnly()

		/// <summary>Called to populate the master properties collection</summary>
		private bool FillMasterProps()
		{
			try
			{
				Debug.Assert(m_tmaxMasterProps != null);
				
				if(m_tmaxMasterProps != null)
				{
					m_tmaxMasterProps.Clear();
					
					CTmaxObjection tmaxObjection = new CTmaxObjection();
					tmaxObjection.GetProperties(m_tmaxMasterProps);

				}// if(m_tmaxMasterProps != null)
				
				//	Default all read-only properties to OFF
				foreach(CTmaxProperty O in m_tmaxMasterProps)
				{
					if(O.Editor == TmaxPropGridEditors.None)
						O.Visible = false;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "FillMasterProps", Ex);
			}
			
			return ((m_tmaxMasterProps != null) && (m_tmaxMasterProps.Count > 0));

		}// private bool FillMasterProps()

		/// <summary>Called to populate the visible properties collection</summary>
		private bool FillVisibleProps()
		{
			try
			{
				Debug.Assert(m_tmaxMasterProps != null);
				Debug.Assert(m_tmaxVisibleProps != null);

				if((m_tmaxMasterProps != null) && (m_tmaxVisibleProps != null))
				{
					m_tmaxVisibleProps.Clear();

					foreach(CTmaxProperty O in m_tmaxMasterProps)
					{
						if(O.Visible == true)
							m_tmaxVisibleProps.Add(O);
					}
					
					//	This is just playing it safe
					if(m_tmaxVisibleProps.Count == 0)
					{
						foreach(CTmaxProperty O in m_tmaxMasterProps)
						{
							O.Visible = true;
							m_tmaxVisibleProps.Add(O);
						}
					}

				}// if((m_tmaxMasterProps != null) && (m_tmaxVisibleProps != null))

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "FillVisibleProps", Ex);
			}

			return ((m_tmaxVisibleProps != null) && (m_tmaxVisibleProps.Count > 0));

		}// private bool FillVisibleProps()

		/// <summary>Called to fire the application command to update the specified objection</summary>
		/// <param name="tmaxObjection">The objection being updated</param>
		/// <returns>true if successful</returns>
		private bool FireUpdate(CTmaxObjection tmaxObjection)
		{
			CTmaxItem		tmaxItem = null;
			CTmaxParameters tmaxParameters = null;
			bool			bSuccessful = false;

			try
			{
				tmaxItem = new CTmaxItem(tmaxObjection);

				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.Objections, true);

				m_bIgnoreUpdates = true;
				
				bSuccessful = FireCommand(TmaxCommands.Update, tmaxItem, tmaxParameters);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FireUpdate", m_tmaxErrorBuilder.Message(ERROR_FIRE_UPDATE_EX), Ex);
				m_bIgnoreUpdates = false;
			}
			
			return bSuccessful;

		}// private bool FireUpdate(CTmaxObjection tmaxObjection)

		/// <summary>This method is called to fire the command to navigate to the requested objection</summary>
		/// <param name="eRequest">The enumerated navigation request identifier</param>
		/// <param name="iIndex">The index of the objection to be activated</param>
		/// <returns>true if successful</returns>
		private bool FireNavigate(TmaxNavigatorRequests eRequest, int iIndex)
		{
			CTmaxParameters tmaxParameters = null;
			bool			bSuccessful = false;

			try
			{
				//	Initialize the parameters
				tmaxParameters = new CTmaxParameters();
				tmaxParameters.Add(TmaxCommandParameters.Objections, true);
				tmaxParameters.Add(TmaxCommandParameters.NavigatorRequest, (int)eRequest);
				
				if(iIndex >= 0)
					tmaxParameters.Add(TmaxCommandParameters.NavigatorIndex, iIndex);

				if(eRequest == TmaxNavigatorRequests.SetMode)
				{
					tmaxParameters.Add(TmaxCommandParameters.NavigatorMode, !m_bDesignationOnly);
				}
				
				bSuccessful = FireCommand(TmaxCommands.Navigate, (CTmaxItem)null, tmaxParameters); 

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "FireNavigate", Ex);
			}

			return bSuccessful;

		}// private bool FireNavigate(TmaxNavigatorRequests eRequest, int iIndex)

		/// <summary>This method handles events fired by the grid before it updates a code's value</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The property grid event arguments</param>
		private void OnGridBeforeUpdate(object sender, FTI.Trialmax.Controls.CTmaxPropGridCtrl.CTmaxPropGridArgs e)
		{
			CTmaxProperty tmaxProperty = null;

			try
			{
				if((tmaxProperty = (CTmaxProperty)(e.IProperty)) != null)
				{
					//	Is this a valid value?
					if(Validate(tmaxProperty, e.Value) == true)
						e.Cancel = false;
					else
						e.Cancel = true;
				}
				else
				{
					m_tmaxEventSource.FireDiagnostic(this, "OnGridBeforeUpdate", "NULL TAG");
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnGridBeforeUpdate", Ex);
			}

		}// private void OnGridBeforeUpdate(object sender, FTI.Trialmax.Controls.CTmaxPropGridCtrl.CTmaxPropGridArgs e)

		/// <summary>This method handles events fired by the grid when it has updated a code's value</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The property grid event arguments</param>
		private void OnGridAfterUpdate(object sender, FTI.Trialmax.Controls.CTmaxPropGridCtrl.CTmaxPropGridArgs e)
		{
			try
			{
				CTmaxProperty tmaxProperty = null;

				//	Make sure we have the property interface
				Debug.Assert(e.IProperty != null);
				if((tmaxProperty = (CTmaxProperty)(e.IProperty)) == null) return;

				//	This keeps the grid in sync just in case the value
				//	was not accepted when the Update command was fired
				if(m_tmaxObjection != null)
				{
					m_tmaxObjection.RefreshProperty(tmaxProperty);
					m_ctrlPropGrid.Update(tmaxProperty);
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnGridAfterUpdate", Ex);
			}
			finally
			{
				m_bIgnoreUpdates = false;
			}
			
		}// private void OnGridAfterUpdate(object sender, FTI.Trialmax.Controls.CTmaxPropGridCtrl.CTmaxPropGridArgs e)

		/// <summary>This method handles events fired by the grid when it needs to populate a property's drop list</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">The property grid event arguments</param>
		private void OnGridGetDropListValues(object sender, FTI.Trialmax.Controls.CTmaxPropGridCtrl.CTmaxPropGridArgs e)
		{
			try
			{
				CTmaxProperty tmaxProperty = null;

				//	Make sure we have the property interface
				Debug.Assert(e.IProperty != null);
				if((tmaxProperty = (CTmaxProperty)(e.IProperty)) == null) return;

				if((m_tmaxDatabase != null) && (m_tmaxDatabase.ObjectionsDatabase != null))
				{
					switch(tmaxProperty.Id)
					{
						case CTmaxObjection.TMAX_OBJECTION_PROP_PARTY:
						
							e.IDropListValues = m_aParties;
							break;

						case CTmaxObjection.TMAX_OBJECTION_PROP_RULING:

							e.IDropListValues = m_tmaxDatabase.ObjectionsDatabase.OxRulings;
							break;

						case CTmaxObjection.TMAX_OBJECTION_PROP_STATE:

							e.IDropListValues = m_tmaxDatabase.ObjectionsDatabase.OxStates;
							break;

					}// switch(tmaxProperty.Id)
	
				}// if((m_tmaxDatabase != null) && (m_tmaxDatabase.ObjectionsDatabase != null))
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnGridGetDropListValues", Ex);
			}

		}// private void OnGridGetDropListValues(object sender, FTI.Trialmax.Controls.CTmaxPropGridCtrl.CTmaxPropGridArgs e)

		/// <summary>This method handles events fired by the grid when it has updated a code's value</summary>
		private void OnAfterUpdate()
		{
			//CDxCode dxCode = null;

			//if(m_tmaxAfterUpdateArgs == null) return;

			//try
			//{
			//    //	Get the code associated with the update
			//    dxCode = (CDxCode)(m_tmaxAfterUpdateArgs.IProperty);
			//    m_tmaxAfterUpdateArgs = null;

			//    if(dxCode == null)
			//    {
			//        m_tmaxEventSource.FireDiagnostic(this, "OnAfterUpdate", "NULL TAG REFERENCE");
			//        return;
			//    }

			//    //	Has the user deleted this code?
			//    if(dxCode.IsNull() == true)
			//    {
			//        //	Delete this code
			//        Delete(dxCode);
			//    }
			//    else
			//    {
			//        //	Is this an existing code?
			//        if(m_dxCodes.Contains(dxCode) == true)
			//            Update(dxCode);
			//        else
			//            Add(dxCode);
			//    }

			//}
			//catch
			//{
			//    m_tmaxEventSource.FireDiagnostic(this, "OnAfterUpdate", "Exception");
			//}

		}// private void OnAfterUpdate()

		/// <summary>This method is called before the specified value gets assigned</summary>
		/// <param name="tmaxProperty">The property being modified</param>
		/// <param name="strValue">The new value to be assigned</param>
		/// <returns>True if the new value is valid</returns>
		private bool Validate(CTmaxProperty tmaxProperty, string strValue)
		{
			bool						bValid = true;
			long						lPage = -1;
			int							iLine = -1;
			string						strMsg = "";
			ITmaxBaseObjectionRecord	IOxRecord = null;

			Debug.Assert(m_tmaxObjection != null);
			if(m_tmaxObjection == null) return false;
			
			try
			{
				switch(tmaxProperty.Id)
				{
					case CTmaxObjection.TMAX_OBJECTION_PROP_PARTY:
					
						if((bValid = m_tmaxObjection.IsParty(strValue)) == false)
							strMsg = String.Format("{0} is not a valid party name. Choose a party from the drop list", strValue);
						else
							m_tmaxObjection.Party = strValue;
						break;

					case CTmaxObjection.TMAX_OBJECTION_PROP_STATE:

						if((m_tmaxDatabase != null) && (m_tmaxDatabase.ObjectionsDatabase != null))
						{
							if(m_tmaxDatabase.ObjectionsDatabase.OxStates != null)
							{
								IOxRecord = m_tmaxDatabase.ObjectionsDatabase.OxStates.Find(strValue, true);
								if(IOxRecord == null)
								{
									strMsg = String.Format("{0} is not a valid status identifier. Choose a status from the drop list", strValue);
									bValid = false;
								}
								else
								{
									m_tmaxObjection.IOxState = IOxRecord;
								}

							}// if(m_tmaxDatabase.ObjectionsDatabase.OxStates != null)
							
						}
						break;

					case CTmaxObjection.TMAX_OBJECTION_PROP_RULING:

						if((m_tmaxDatabase != null) && (m_tmaxDatabase.ObjectionsDatabase != null))
						{
							if(m_tmaxDatabase.ObjectionsDatabase.OxRulings != null)
							{
								IOxRecord = m_tmaxDatabase.ObjectionsDatabase.OxRulings.Find(strValue, true);
								if(IOxRecord == null)
								{
									strMsg = String.Format("{0} is not a valid ruling identifier. Choose a ruling from the drop list", strValue);
									bValid = false;
								}
								else
								{
									m_tmaxObjection.IOxRuling = IOxRecord;
								}

							}// if(m_tmaxDatabase.ObjectionsDatabase.OxRulings != null)
						
						}
						break;

					case CTmaxObjection.TMAX_OBJECTION_PROP_FIRST_PAGE:
					case CTmaxObjection.TMAX_OBJECTION_PROP_LAST_PAGE:

						//	Convert the value to a page number
						try { lPage = System.Convert.ToInt64(strValue); }
						catch { }

						if(lPage > 0)
						{
							bValid = ValidatePL(tmaxProperty, lPage);
						}
						else
						{
							strMsg = String.Format("{0} is not a valid page number", strValue);
							bValid = false;
						}
						break;

					case CTmaxObjection.TMAX_OBJECTION_PROP_FIRST_LINE:
					case CTmaxObjection.TMAX_OBJECTION_PROP_LAST_LINE:

						//	Convert the value to a line number
						try { iLine = System.Convert.ToInt32(strValue); }
						catch { }

						if(iLine > 0)
						{
							bValid = ValidatePL(tmaxProperty, (long)iLine);
						}
						else
						{
							strMsg = String.Format("{0} is not a valid line number", strValue);
							bValid = false;
						}
						break;

					case CTmaxObjection.TMAX_OBJECTION_PROP_ARGUMENT:
						
						m_tmaxObjection.Argument = strValue;
						break;

					case CTmaxObjection.TMAX_OBJECTION_PROP_RESPONSE_1:

						m_tmaxObjection.Response1 = strValue;
						break;

					case CTmaxObjection.TMAX_OBJECTION_PROP_RESPONSE_2:

						m_tmaxObjection.Response2 = strValue;
						break;

					case CTmaxObjection.TMAX_OBJECTION_PROP_RESPONSE_3:

						m_tmaxObjection.Response3 = strValue;
						break;

					case CTmaxObjection.TMAX_OBJECTION_PROP_RULING_TEXT:

						m_tmaxObjection.RulingText = strValue;
						break;

					case CTmaxObjection.TMAX_OBJECTION_PROP_WORK_PRODUCT:

						m_tmaxObjection.WorkProduct = strValue;
						break;

					case CTmaxObjection.TMAX_OBJECTION_PROP_COMMENTS:

						m_tmaxObjection.Comments = strValue;
						break;
						
					default:
					
						Debug.Assert(false, "Unhandled property update: " + tmaxProperty.Name);
						bValid = false;
						break;

				}// switch(tmaxProperty.Id)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Validate", m_tmaxErrorBuilder.Message(ERROR_VALIDATE_EX), Ex);
				bValid = false;
			}

			if(bValid == true)
			{
				//	Update the record in the database
				FireUpdate(m_tmaxObjection);
			}
			else
			{
				if(strMsg.Length > 0)
					Warn(strMsg);
			}
				
			return bValid;

		}// private bool Validate(CTmaxProperty tmaxProperty, string strValue)

		/// <summary>This method is called to validate the numerical property value</summary>
		/// <param name="tmaxProperty">The property being modified</param>
		/// <param name="lValue">The new value to be assigned</param>
		/// <returns>True if the new value is valid</returns>
		private bool ValidatePL(CTmaxProperty tmaxProperty, long lValue)
		{
			long		lPL = 0;
			long		lPage = 0;
			int			iLine = 0;
			CDxPrimary	dxDeposition = null;
			string		strMsg = "";
			
			//	We need to check the deposition that owns this objection
			if((dxDeposition = (CDxPrimary)(m_tmaxObjection.ICaseDeposition)) == null)
				dxDeposition = m_tmaxDatabase.Primaries.Find(m_tmaxObjection.Deposition);
			if(dxDeposition == null)
				return true; // Can't check the range so let it go

			//	First check maximum lines per page
			if((tmaxProperty.Id == CTmaxObjection.TMAX_OBJECTION_PROP_FIRST_LINE) || (tmaxProperty.Id == CTmaxObjection.TMAX_OBJECTION_PROP_LAST_LINE))
			{
				if((dxDeposition.Transcript.LinesPerPage > 0) && (lValue > dxDeposition.Transcript.LinesPerPage))
				{
					strMsg = String.Format("The maximum lines per page for {0} is {1}", dxDeposition.MediaId, dxDeposition.Transcript.LinesPerPage);
					return Warn(strMsg);
				}

			}// if((tmaxProperty.Id == TMAX_OBJECTION_PROP_FIRST_LINE) || (tmaxProperty.Id == TMAX_OBJECTION_PROP_LAST_LINE))
			
			switch(tmaxProperty.Id)
			{
				case CTmaxObjection.TMAX_OBJECTION_PROP_FIRST_PAGE:
				
					lPage = lValue;
					iLine = CTmaxToolbox.PLToLine(m_tmaxObjection.FirstPL);
					lPL = CTmaxToolbox.GetPL(lPage, iLine);
					
					if(lPL < dxDeposition.Transcript.FirstPL)
					{
						strMsg = String.Format("{0} is not a valid page number. The first page/line in {1} is {2}", lPage, dxDeposition.MediaId, CTmaxToolbox.PLToString(dxDeposition.Transcript.FirstPL));
						return Warn(strMsg);
					}
					else if(lPL > m_tmaxObjection.LastPL)
					{
						strMsg = String.Format("{0} is not a valid page number. The first page/line of the objection must occur before the last page/line", lPage);
						return Warn(strMsg);
					}
					else
					{
						m_tmaxObjection.FirstPL = lPL;
					}
					break;
				
				case CTmaxObjection.TMAX_OBJECTION_PROP_FIRST_LINE:

					lPage = CTmaxToolbox.PLToPage(m_tmaxObjection.FirstPL); ;
					iLine = (int)lValue;
					lPL = CTmaxToolbox.GetPL(lPage, iLine);

					if(lPL < dxDeposition.Transcript.FirstPL)
					{
						strMsg = String.Format("{0} is not a valid line number. The first page/line in {1} is {2}", iLine, dxDeposition.MediaId, CTmaxToolbox.PLToString(dxDeposition.Transcript.FirstPL));
						return Warn(strMsg);
					}
					else if(lPL > m_tmaxObjection.LastPL)
					{
						strMsg = String.Format("{0} is not a valid line number. The first page/line of the objection must occur before the last page/line", iLine);
						return Warn(strMsg);
					}
					else
					{
						m_tmaxObjection.FirstPL = lPL;
					}
					break;

				case CTmaxObjection.TMAX_OBJECTION_PROP_LAST_PAGE:

					lPage = lValue;
					iLine = CTmaxToolbox.PLToLine(m_tmaxObjection.LastPL);
					lPL = CTmaxToolbox.GetPL(lPage, iLine);

					if(lPL > dxDeposition.Transcript.LastPL)
					{
						strMsg = String.Format("{0} is not a valid page number. The last page/line in {1} is {2}", lPage, dxDeposition.MediaId, CTmaxToolbox.PLToString(dxDeposition.Transcript.LastPL));
						return Warn(strMsg);
					}
					else if(lPL < m_tmaxObjection.FirstPL)
					{
						strMsg = String.Format("{0} is not a valid page number. The last page/line of the objection must occur after the first page/line", lPage);
						return Warn(strMsg);
					}
					else
					{
						m_tmaxObjection.LastPL = lPL;
					}
					break;

				case CTmaxObjection.TMAX_OBJECTION_PROP_LAST_LINE:

					lPage = CTmaxToolbox.PLToPage(m_tmaxObjection.LastPL); ;
					iLine = (int)lValue;
					lPL = CTmaxToolbox.GetPL(lPage, iLine);

					if(lPL > dxDeposition.Transcript.LastPL)
					{
						strMsg = String.Format("{0} is not a valid line number. The last page/line in {1} is {2}", iLine, dxDeposition.MediaId, CTmaxToolbox.PLToString(dxDeposition.Transcript.LastPL));
						return Warn(strMsg);
					}
					else if(lPL < m_tmaxObjection.FirstPL)
					{
						strMsg = String.Format("{0} is not a valid line number. The last page/line of the objection must occur after the first page/line", iLine);
						return Warn(strMsg);
					}
					else
					{
						m_tmaxObjection.LastPL = lPL;
					}
					break;

			}// switch(tmaxProperty.Id)

			return true; // Must be OK if we made it this far

		}// private bool ValidatePL(CTmaxProperty tmaxProperty, long lValue)

		/// <summary>This method is called to convert the key to a command identifier</summary>
		/// <param name="strKey">The Infragistic key identifier</param>
		/// <returns>The associated pane command</returns>
		private ObjectionsPropertiesPaneCommands GetCommand(string strKey)
		{
			try
			{
				Array aCommands = Enum.GetValues(typeof(ObjectionsPropertiesPaneCommands));

				foreach(ObjectionsPropertiesPaneCommands eCommand in aCommands)
				{
					if(eCommand.ToString() == strKey)
						return eCommand;
				}

			}
			catch
			{
			}

			return ObjectionsPropertiesPaneCommands.Invalid;

		}// private ObjectionsPropertiesPaneCommands GetCommand(string strKey)

		/// <summary>This method is called to determine if the specified command should be enabled</summary>
		/// <param name="eCommand">The pane command enumeration</param>
		/// <returns>true if command should be enabled</returns>
		private bool GetCommandEnabled(ObjectionsPropertiesPaneCommands eCommand)
		{
			//	We have to have an active database with codes
			if(m_tmaxDatabase == null) return false;
			if(m_tmaxDatabase.Primaries == null) return false;
			if(m_tmaxDatabase.ObjectionsEnabled == false) return false;

			//	What is the command?
			switch(eCommand)
			{
				case ObjectionsPropertiesPaneCommands.GoTo:

					return (m_iNavigatorTotal > 0);

				case ObjectionsPropertiesPaneCommands.First:
				case ObjectionsPropertiesPaneCommands.Previous:

					if(m_iNavigatorTotal == 0) return false;

					if(m_iNavigatorIndex >= 0)
						return (m_iNavigatorIndex > 0);
					else
						return (eCommand == ObjectionsPropertiesPaneCommands.First);

				case ObjectionsPropertiesPaneCommands.Last:
				case ObjectionsPropertiesPaneCommands.Next:

					if(m_iNavigatorTotal == 0) return false;

					if(m_iNavigatorIndex >= 0)
						return (m_iNavigatorIndex < m_iNavigatorTotal - 1);
					else
						return (eCommand == ObjectionsPropertiesPaneCommands.Last);

				case ObjectionsPropertiesPaneCommands.SetVisibleProps:

					return ((m_tmaxMasterProps != null) && (m_tmaxMasterProps.Count > 0));

				case ObjectionsPropertiesPaneCommands.DesignationOnly:

					return true;

				default:

					break;

			}// switch(eCommand)	

			return false;

		}// private bool GetCommandEnabled(ObjectionsPropertiesPaneCommands eCommand)

		/// <summary>This method processes the specified command</summary>
		/// <param name="eCommand">The enumerated command identifier</param>
		private void OnCommand(ObjectionsPropertiesPaneCommands eCommand)
		{
			int iSelections = 0;

			//	Get the current number of row selections
			if((m_ctrlPropGrid != null) && (m_ctrlPropGrid.IsDisposed == false))
				iSelections = m_ctrlPropGrid.GetSelectedCount();

			try
			{
				//	Direct to the appropriate handler
				switch(eCommand)
				{
					case ObjectionsPropertiesPaneCommands.First:
					case ObjectionsPropertiesPaneCommands.Previous:
					case ObjectionsPropertiesPaneCommands.Next:
					case ObjectionsPropertiesPaneCommands.Last:

						OnCmdNavigate(eCommand);
						break;

					case ObjectionsPropertiesPaneCommands.SetVisibleProps:

						OnCmdSetVisibleProps();
						break;

					case ObjectionsPropertiesPaneCommands.DesignationOnly:

						OnCmdDesignationOnly();
						break;

					default:

						break;

				}// switch(eCommand)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnCommand", Ex);
			}

		}// private void OnCommand(ObjectionsPropertiesPaneCommands eCommand)

		/// <summary>Event handler for the Navigate commands</summary>
		/// <param name="eCommand">The navigation command being processed</param>
		private void OnCmdNavigate(ObjectionsPropertiesPaneCommands eCommand)
		{
			TmaxNavigatorRequests eRequest = TmaxNavigatorRequests.Absolute;
			
			try
			{
				//	Process any pending changes before moving to a new record
				m_ctrlPropGrid.EndUserUpdate(false);

				//	Calculate the new index
				if(m_iNavigatorTotal > 0)
				{
					switch(eCommand)
					{
						case ObjectionsPropertiesPaneCommands.First:

							eRequest = TmaxNavigatorRequests.First;
							break;

						case ObjectionsPropertiesPaneCommands.Last:

							eRequest = TmaxNavigatorRequests.Last;
							break;

						case ObjectionsPropertiesPaneCommands.Next:

							eRequest = TmaxNavigatorRequests.Next;
							break;

						case ObjectionsPropertiesPaneCommands.Previous:

							eRequest = TmaxNavigatorRequests.Previous;
							break;

					}// switch(eCommand)

					FireNavigate(eRequest, -1);

				}// if(m_iNavigatorTotal > 0)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdNavigate", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_NAVIGATE_EX, eCommand), Ex);
			}

		}// private void OnCmdNavigate(ObjectionsPropertiesPaneCommands eCommand)

		/// <summary>This method handles events fired by the toolbar manager before displaying the popup menu</summary>
		/// <param name="sender">The toolbar manager firing the event</param>
		/// <param name="e">The Infragistics event argument object</param>
		private void OnUltraBeforeDropDown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventArgs e)
		{
			SetToolStates();
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
			ObjectionsPropertiesPaneCommands eCommand = ObjectionsPropertiesPaneCommands.Invalid;

			//	Don't bother if ignoring events
			if(m_bIgnoreUltraEvents == true) return;

			//	Get the command
			if(e.Tool != null && e.Tool.Key != null)
				eCommand = GetCommand(e.Tool.Key);

			if(eCommand != ObjectionsPropertiesPaneCommands.Invalid)
				OnCommand(eCommand);

		}// OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)

		/// <summary>This method is called to enable/disable the tools in the manager's collection</summary>
		private void SetToolStates()
		{
			ObjectionsPropertiesPaneCommands eCommand;

			Debug.Assert(m_ultraToolbarManager != null);
			Debug.Assert(m_ultraToolbarManager.Tools != null);
			if((m_ultraToolbarManager == null) || (m_ultraToolbarManager.Tools == null)) return;

			//	Check each tool in the manager's collection
			foreach(ToolBase O in m_ultraToolbarManager.Tools)
			{
				if(O.Key == null) continue;

				try
				{
					if((eCommand = GetCommand(O.Key)) == ObjectionsPropertiesPaneCommands.Invalid)
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

					}// if((eCommand = GetCommand(O.Key)) == ObjectionsPropertiesPaneCommands.Invalid)

					//	Should the command be enabled?
					O.SharedProps.Enabled = GetCommandEnabled(eCommand);

					switch(eCommand)
					{
						case ObjectionsPropertiesPaneCommands.GoTo:

							if(m_iNavigatorIndex >= 0)
								((TextBoxTool)O).Text = (m_iNavigatorIndex + 1).ToString();
							else
								((TextBoxTool)O).Text = "";
							break;

						case ObjectionsPropertiesPaneCommands.DesignationOnly:

							m_bIgnoreUltraEvents = true;
							((StateButtonTool)O).Checked = m_bDesignationOnly;
							m_bIgnoreUltraEvents = false;

							if(m_bDesignationOnly == true)
								O.SharedProps.AppearancesSmall.Appearance.Image = 6;
							else
								O.SharedProps.AppearancesSmall.Appearance.Image = 5;
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

		/// <summary>This method will put the focus on the Go To barcode text box in the main menu</summary>
		private void SetFocusGoTo()
		{
			try
			{
				((TextBoxTool)(m_ultraToolbarManager.Toolbars["MainToolbar"].Tools[ObjectionsPropertiesPaneCommands.GoTo.ToString()])).IsActiveTool = true;
				((TextBoxTool)(m_ultraToolbarManager.Toolbars["MainToolbar"].Tools[ObjectionsPropertiesPaneCommands.GoTo.ToString()])).IsInEditMode = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetFocusGoTo", Ex);
			}

		}// private void SetFocusGoTo()

		/// <summary>This function handles events fired by the toolbar manager when the user releases a key in one of the tools</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event parameters</param>
		private void OnUltraToolKeyDown(object sender, Infragistics.Win.UltraWinToolbars.ToolKeyEventArgs e)
		{
			if((e.Tool.Key == ObjectionsPropertiesPaneCommands.GoTo.ToString()) && (e.KeyCode == Keys.Enter))
			{
				//	Mark the event as handled
				e.Handled = true;

				OnCmdGoTo();
			}

		}// private void OnUltraToolKeyDown(object sender, Infragistics.Win.UltraWinToolbars.ToolKeyEventArgs e)

		/// <summary>Called to activate the specified objection object</summary>
		/// <param name="tmaxObjection">the objection to be activated</param>
		/// <returns>true if successful</returns>
		private bool Activate(CTmaxObjection tmaxObjection)
		{
			bool bSuccessful = false;

			try
			{
				//	Get the properties associated with this objection
				if(tmaxObjection != null)
				{
					if(m_tmaxVisibleProps.Count == 0)
						FillVisibleProps();
						
					tmaxObjection.RefreshProperties(m_tmaxVisibleProps);
				}
				else
				{
					m_tmaxVisibleProps.Clear();
				}

				//	Refresh the grid control
				m_ctrlPropGrid.Add(m_tmaxVisibleProps, true);

				//	Update the local member
				m_tmaxObjection = tmaxObjection;
				m_tmaxActivate = null;

				bSuccessful = true;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Activate", m_tmaxErrorBuilder.Message(ERROR_ACTIVATE_EX), Ex);
			}
			finally
			{
				SetToolStates();
			}

			return bSuccessful;

		}// private bool Activate(CTmaxObjection tmaxObjection)

		/// <summary>Called to update the properties of the active objection</summary>
		/// <returns>true if successful</returns>
		private bool UpdateProperties()
		{
			bool bSuccessful = false;

			try
			{
				if(m_tmaxObjection != null)
				{
					//	Refresh the property bag
					m_tmaxObjection.RefreshProperties(m_tmaxMasterProps);

					//	Update all the properties
					m_ctrlPropGrid.Update(null);						
				}

				bSuccessful = true;

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "UpdateProperties", m_tmaxErrorBuilder.Message(ERROR_UPDATE_PROPERTIES_EX), Ex);
			}

			return bSuccessful;

		}// private bool UpdateProperties()

		#endregion Private Methods

	}// public class CObjectionPropertiesPane : FTI.Trialmax.Panes.CBasePane

}// namespace FTI.Trialmax.Panes
