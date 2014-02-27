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
	public class CScriptReviewPane : FTI.Trialmax.Panes.CBasePane
	{
		#region Constants

		/// <summary>Local context menu command identifiers</summary>
		private enum ScriptReviewPaneCommands
		{
			Invalid = 0,
			Add,
			Delete,
			First,
			Previous,
			Next,
			Last,
			GoTo,
		}

		private const int ERROR_ON_CMD_DELETE_EX = (ERROR_BASE_PANE_MAX + 1);
		private const int ERROR_ON_CMD_ADD_EX = (ERROR_BASE_PANE_MAX + 2);
		private const int ERROR_FILL_EX = (ERROR_BASE_PANE_MAX + 3);

		#endregion Constants

		#region Private Members

		/// <summary>Component collection required by form designer</summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>Infragistics toolbar manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager m_ultraToolbarManager;

		/// <summary>Infragistics toolbar manager left docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CScriptReviewPane_Toolbars_Dock_Area_Left;

		/// <summary>Infragistics toolbar manager right docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CScriptReviewPane_Toolbars_Dock_Area_Right;

		/// <summary>Infragistics toolbar manager top docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CScriptReviewPane_Toolbars_Dock_Area_Top;

		/// <summary>Infragistics toolbar manager bottom docking area</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CScriptReviewPane_Toolbars_Dock_Area_Bottom;
        private Label label1;
        protected ImageList m_ctrlImages;

		private Infragistics.Win.UltraWinStatusBar.UltraStatusBar m_ctrlStatusBar;

		#endregion Private Members

		#region Public Methods

		/// <summary>Constructor</summary>
		public CScriptReviewPane()
			: base()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

		}// public CScriptReviewPane()

		/// <summary>This method is called by the application to activate the specified item</summary>
		/// <param name="tmaxItem">The item to be activated</param>
		/// <param name="ePane">The pane requesting activation</param>
		/// <returns>true if successful</returns>
		public override bool Activate(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)
		{
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
		}

		/// <summary>This method is called by the application to when the item has been updated by the user</summary>
		/// <param name="tmaxItem">The item that has been updated</param>
		public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{

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

		}// public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)

		/// <summary>This method is called by the application to when the item gets deleted</summary>
		/// <param name="tmaxItem">The item that has been deleted</param>
		public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{

			SetToolStates();

		}// public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)

		/// <summary>This method is called by the application to initialize the pane</summary>
		/// <returns>true if successful</returns>
		public override bool Initialize(CXmlIni xmlINI)
		{
			//	Do the base class initialization first
			if(base.Initialize(xmlINI) == false)
				return false;

			//	Make sure we are on the correct section
			xmlINI.SetSection(m_strPaneName);


			return true;

		}// public override bool Initialize()

		/// <summary>This method is called by the application when it is about to terminate</summary>
		/// <remarks>Derived classes should override for custom shutdown</remarks>
		public override void Terminate(CXmlIni xmlINI)
		{
			//	Make sure we are on the correct section
			xmlINI.SetSection(m_strPaneName);


			//	Do the base class processing
			base.Terminate(xmlINI);

		}// public override void Terminate(CXmlIni xmlINI)

		#endregion Public Methods

		#region Protected Methods

		/// <summary>This function is called when the value of the Database property changes</summary>
		protected override void OnDatabaseChanged()
		{

		}// protected override void OnDatabaseChanged()

		/// <summary>This function is called when the Active property changes</summary>
		protected override void OnPaneVisibleChanged()
		{
			//	Is the pane being made active?
			if(m_bPaneVisible == true)
			{

			}

		}// protected override void OnActiveChanged()

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
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("PinScript", "");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("ScriptLabel");
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool1 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("Scripts");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("PinScript", "");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("ScriptLabel");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool2 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("Scripts");
            Infragistics.Win.ValueList valueList1 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CScriptReviewPane));
            this.m_ultraToolbarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._CScriptReviewPane_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._CScriptReviewPane_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._CScriptReviewPane_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._CScriptReviewPane_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.m_ctrlStatusBar = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.label1 = new System.Windows.Forms.Label();
            this.m_ctrlImages = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.m_ultraToolbarManager)).BeginInit();
            this.SuspendLayout();
            // 
            // m_ultraToolbarManager
            // 
            this.m_ultraToolbarManager.DesignerFlags = 1;
            this.m_ultraToolbarManager.DockWithinContainer = this;
            this.m_ultraToolbarManager.ImageListSmall = this.m_ctrlImages;
            this.m_ultraToolbarManager.ShowFullMenusDelay = 500;
            this.m_ultraToolbarManager.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.Office2003;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            labelTool1.InstanceProps.IsFirstInGroup = true;
            labelTool1.InstanceProps.Width = 61;
            comboBoxTool1.InstanceProps.Width = 180;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool1,
            labelTool1,
            comboBoxTool1});
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
            appearance2.Image = 18;
            stateButtonTool2.SharedProps.AppearancesSmall.Appearance = appearance2;
            stateButtonTool2.SharedProps.Caption = "PinScript";
            appearance3.TextHAlignAsString = "Left";
            labelTool2.SharedProps.AppearancesSmall.AppearanceOnToolbar = appearance3;
            labelTool2.SharedProps.Caption = "Script:";
            comboBoxTool2.SharedProps.Caption = "Scripts";
            comboBoxTool2.SharedProps.Spring = true;
            comboBoxTool2.ValueList = valueList1;
            this.m_ultraToolbarManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool2,
            labelTool2,
            comboBoxTool2});
            this.m_ultraToolbarManager.BeforeToolDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventHandler(this.OnUltraBeforeDropDown);
            this.m_ultraToolbarManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.OnUltraToolClick);
            this.m_ultraToolbarManager.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.OnUltraBeforeToolbarListDropdown);
            this.m_ultraToolbarManager.ToolKeyDown += new Infragistics.Win.UltraWinToolbars.ToolKeyEventHandler(this.OnUltraToolKeyDown);
            // 
            // _CScriptReviewPane_Toolbars_Dock_Area_Top
            // 
            this._CScriptReviewPane_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CScriptReviewPane_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._CScriptReviewPane_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._CScriptReviewPane_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CScriptReviewPane_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._CScriptReviewPane_Toolbars_Dock_Area_Top.Name = "_CScriptReviewPane_Toolbars_Dock_Area_Top";
            this._CScriptReviewPane_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(316, 26);
            this._CScriptReviewPane_Toolbars_Dock_Area_Top.ToolbarsManager = this.m_ultraToolbarManager;
            // 
            // _CScriptReviewPane_Toolbars_Dock_Area_Bottom
            // 
            this._CScriptReviewPane_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CScriptReviewPane_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._CScriptReviewPane_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._CScriptReviewPane_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CScriptReviewPane_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 265);
            this._CScriptReviewPane_Toolbars_Dock_Area_Bottom.Name = "_CScriptReviewPane_Toolbars_Dock_Area_Bottom";
            this._CScriptReviewPane_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(316, 0);
            this._CScriptReviewPane_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.m_ultraToolbarManager;
            // 
            // _CScriptReviewPane_Toolbars_Dock_Area_Left
            // 
            this._CScriptReviewPane_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CScriptReviewPane_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._CScriptReviewPane_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._CScriptReviewPane_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CScriptReviewPane_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 26);
            this._CScriptReviewPane_Toolbars_Dock_Area_Left.Name = "_CScriptReviewPane_Toolbars_Dock_Area_Left";
            this._CScriptReviewPane_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 239);
            this._CScriptReviewPane_Toolbars_Dock_Area_Left.ToolbarsManager = this.m_ultraToolbarManager;
            // 
            // _CScriptReviewPane_Toolbars_Dock_Area_Right
            // 
            this._CScriptReviewPane_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CScriptReviewPane_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._CScriptReviewPane_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._CScriptReviewPane_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CScriptReviewPane_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(316, 26);
            this._CScriptReviewPane_Toolbars_Dock_Area_Right.Name = "_CScriptReviewPane_Toolbars_Dock_Area_Right";
            this._CScriptReviewPane_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 239);
            this._CScriptReviewPane_Toolbars_Dock_Area_Right.ToolbarsManager = this.m_ultraToolbarManager;
            // 
            // m_ctrlStatusBar
            // 
            appearance1.TextHAlignAsString = "Center";
            this.m_ctrlStatusBar.Appearance = appearance1;
            this.m_ctrlStatusBar.BorderStyle = Infragistics.Win.UIElementBorderStyle.InsetSoft;
            this.m_ctrlStatusBar.Location = new System.Drawing.Point(0, 265);
            this.m_ctrlStatusBar.Name = "m_ctrlStatusBar";
            this.m_ctrlStatusBar.Size = new System.Drawing.Size(316, 23);
            this.m_ctrlStatusBar.TabIndex = 5;
            this.m_ctrlStatusBar.WrapText = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "SCRIPT REVIEW PANE";
            // 
            // m_ctrlImages
            // 
            this.m_ctrlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlImages.ImageStream")));
            this.m_ctrlImages.TransparentColor = System.Drawing.Color.Magenta;
            this.m_ctrlImages.Images.SetKeyName(0, "");
            this.m_ctrlImages.Images.SetKeyName(1, "");
            this.m_ctrlImages.Images.SetKeyName(2, "");
            this.m_ctrlImages.Images.SetKeyName(3, "");
            this.m_ctrlImages.Images.SetKeyName(4, "");
            this.m_ctrlImages.Images.SetKeyName(5, "");
            this.m_ctrlImages.Images.SetKeyName(6, "");
            this.m_ctrlImages.Images.SetKeyName(7, "");
            this.m_ctrlImages.Images.SetKeyName(8, "");
            this.m_ctrlImages.Images.SetKeyName(9, "");
            this.m_ctrlImages.Images.SetKeyName(10, "");
            this.m_ctrlImages.Images.SetKeyName(11, "");
            this.m_ctrlImages.Images.SetKeyName(12, "");
            this.m_ctrlImages.Images.SetKeyName(13, "");
            this.m_ctrlImages.Images.SetKeyName(14, "");
            this.m_ctrlImages.Images.SetKeyName(15, "");
            this.m_ctrlImages.Images.SetKeyName(16, "");
            this.m_ctrlImages.Images.SetKeyName(17, "");
            this.m_ctrlImages.Images.SetKeyName(18, "");
            this.m_ctrlImages.Images.SetKeyName(19, "");
            this.m_ctrlImages.Images.SetKeyName(20, "");
            this.m_ctrlImages.Images.SetKeyName(21, "");
            this.m_ctrlImages.Images.SetKeyName(22, "");
            this.m_ctrlImages.Images.SetKeyName(23, "");
            this.m_ctrlImages.Images.SetKeyName(24, "");
            this.m_ctrlImages.Images.SetKeyName(25, "");
            this.m_ctrlImages.Images.SetKeyName(26, "");
            // 
            // CScriptReviewPane
            // 
            this.Controls.Add(this.label1);
            this.Controls.Add(this._CScriptReviewPane_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._CScriptReviewPane_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._CScriptReviewPane_Toolbars_Dock_Area_Top);
            this.Controls.Add(this._CScriptReviewPane_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this.m_ctrlStatusBar);
            this.Name = "CScriptReviewPane";
            this.Size = new System.Drawing.Size(316, 288);
            ((System.ComponentModel.ISupportInitialize)(this.m_ultraToolbarManager)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while populating the codes grid");
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
			try
			{

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnCmdGoTo", Ex);
			}

		}// private void OnCmdGoTo()

		/// <summary>This method will populate the property grid with the active codes collection</summary>
		private void Fill()
		{
			try
			{
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Fill", m_tmaxErrorBuilder.Message(ERROR_FILL_EX), Ex);
			}

		}// private void Fill()

		/// <summary>This method is called to convert the key to a command identifier</summary>
		/// <param name="strKey">The Infragistic key identifier</param>
		/// <returns>The associated pane command</returns>
		private ScriptReviewPaneCommands GetCommand(string strKey)
		{
			try
			{
				Array aCommands = Enum.GetValues(typeof(ScriptReviewPaneCommands));

				foreach(ScriptReviewPaneCommands eCommand in aCommands)
				{
					if(eCommand.ToString() == strKey)
						return eCommand;
				}

			}
			catch
			{
			}

			return ScriptReviewPaneCommands.Invalid;

		}// private ScriptReviewPaneCommands GetCommand(string strKey)

		/// <summary>This method is called to determine if the specified command should be enabled</summary>
		/// <param name="eCommand">The transcript pane command enumeration</param>
		/// <param name="iSelections">The number of rows currently selected</param>
		/// <returns>true if command should be enabled</returns>
		private bool GetCommandEnabled(ScriptReviewPaneCommands eCommand, int iSelections)
		{
			//	We have to have an active database with codes
			if(m_tmaxDatabase == null) return false;
			if(m_tmaxDatabase.Primaries == null) return false;

			//	What is the command?
			switch(eCommand)
			{

				case ScriptReviewPaneCommands.Add:
				case ScriptReviewPaneCommands.Delete:
				case ScriptReviewPaneCommands.GoTo:
				case ScriptReviewPaneCommands.First:
				case ScriptReviewPaneCommands.Previous:
				case ScriptReviewPaneCommands.Last:
				case ScriptReviewPaneCommands.Next:
					return true;

				default:

					break;

			}// switch(eCommand)	

			return false;

		}// private bool GetCommandEnabled(ScriptReviewPaneCommands eCommand, int iSelections)

		/// <summary>This method processes the specified command</summary>
		/// <param name="eCommand">The enumerated command identifier</param>
		private void OnCommand(ScriptReviewPaneCommands eCommand)
		{

			try
			{
				//	Direct to the appropriate handler
				switch(eCommand)
				{
					case ScriptReviewPaneCommands.First:
					case ScriptReviewPaneCommands.Previous:
					case ScriptReviewPaneCommands.Next:
					case ScriptReviewPaneCommands.Last:

						break;

					case ScriptReviewPaneCommands.Add:

						OnCmdAdd();
						break;


					case ScriptReviewPaneCommands.Delete:

						OnCmdDelete();
						break;

					default:

						break;

				}// switch(eCommand)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "OnCommand", Ex);
			}

		}// private void OnCommand(ScriptReviewPaneCommands eCommand)


		/// <summary>Event handler for the Add command</summary>
		private void OnCmdAdd()
		{
			try
			{

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdAdd", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_ADD_EX), Ex);
			}

		}// private void OnCmdAdd()

		/// <summary>Event handler for the Delete command</summary>
		private void OnCmdDelete()
		{
			try
			{
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OnCmdDeleted", m_tmaxErrorBuilder.Message(ERROR_ON_CMD_DELETE_EX), Ex);
			}

		}// private void OnCmdDelete()



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
			ScriptReviewPaneCommands eCommand = ScriptReviewPaneCommands.Invalid;

			//	Don't bother if ignoring events
			if(m_bIgnoreUltraEvents == true) return;

			//	Get the command
			if(e.Tool != null && e.Tool.Key != null)
				eCommand = GetCommand(e.Tool.Key);

			if(eCommand != ScriptReviewPaneCommands.Invalid)
				OnCommand(eCommand);

		}// OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)

		/// <summary>This method is called to enable/disable the tools in the manager's collection</summary>
		private void SetToolStates()
		{
			Debug.Assert(m_ultraToolbarManager != null);
			Debug.Assert(m_ultraToolbarManager.Tools != null);
			if((m_ultraToolbarManager == null) || (m_ultraToolbarManager.Tools == null)) return;

			//	Check each tool in the manager's collection
			foreach(ToolBase O in m_ultraToolbarManager.Tools)
			{
				if(O.Key == null) continue;

				try
				{
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


			m_ctrlStatusBar.Text = strText;

		}// private void SetStatusText()


		/// <summary>This function handles events fired by the toolbar manager when the user releases a key in one of the tools</summary>
		/// <param name="sender">The object firing the event</param>
		/// <param name="e">Infragistics event parameters</param>
		private void OnUltraToolKeyDown(object sender, Infragistics.Win.UltraWinToolbars.ToolKeyEventArgs e)
		{
			if((e.Tool.Key == ScriptReviewPaneCommands.GoTo.ToString()) && (e.KeyCode == Keys.Enter))
			{
				//	Mark the event as handled
				e.Handled = true;

			}

		}// private void OnUltraToolKeyDown(object sender, Infragistics.Win.UltraWinToolbars.ToolKeyEventArgs e)


		#endregion Private Methods

	}// public class CScriptReviewPane : FTI.Trialmax.Panes.CBasePane

}// namespace FTI.Trialmax.Panes
