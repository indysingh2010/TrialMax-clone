using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;
using FTI.Trialmax.ActiveX;
using FTI.Trialmax.Controls;
using FTI.Trialmax.Database;

using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinToolbars;

namespace FTI.Trialmax.Panes
{
    /// <summary>This class implements a TrialMax media viewer window</summary>
    public class CMediaViewer : FTI.Trialmax.Panes.CBasePane
    {
        public event EventHandler OnRequestPresentation;

        #region Constants

        /// <summary>Local context menu command identifiers</summary>
        protected enum MediaViewerCommands
        {
            Invalid = 0,
            LockRatio,
            Properties,
            Presentation,
            Builder,
            Tuner,
            Print,
            BlankPresentation
        }

        protected const string KEY_LOCKED_RATIO = "LockedRatio";

        #endregion Constants

        #region Private Members


        /// <summary>Flag to indicate if current media is composite media</summary>
        private bool m_bCompositeMedia = false;

        /// <summary>Flag to indicate if viewer should reload when activated</summary>
        private bool m_bReload = false;

        /// <summary>Flag to indicate if the user is editing a text annotation</summary>
        private bool m_bTextEditorActive = false;

        /// <summary>Primary exchange object for current image</summary>
        private FTI.Trialmax.Database.CDxPrimary m_dxPrimary = null;

        /// <summary>Secondary exchange object for current image</summary>
        private FTI.Trialmax.Database.CDxSecondary m_dxSecondary = null;

        /// <summary>The pane's status bar</summary>
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar m_ctrlStatusBar;

        /// <summary>Tertiary exchange object for current image</summary>
        private FTI.Trialmax.Database.CDxTertiary m_dxTertiary = null;

        /// <summary>Secondary exchange for script scene currently loaded in the viewer</summary>
        private FTI.Trialmax.Database.CDxSecondary m_dxScene = null;

        /// <summary>The pane's toolbar and menu manager</summary>
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager m_ultraToolbarManager;

        /// <summary>Left hand docking area used by toolbar manager</summary>
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CMediaViewer_Toolbars_Dock_Area_Left;

        /// <summary>Right hand docking area used by toolbar manager</summary>
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CMediaViewer_Toolbars_Dock_Area_Right;

        /// <summary>Top docking area used by toolbar manager</summary>
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CMediaViewer_Toolbars_Dock_Area_Top;

        /// <summary>Bottom docking area used by toolbar manager</summary>
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CMediaViewer_Toolbars_Dock_Area_Bottom;

        /// <summary>Components collection required by form designer</summary>
        private System.ComponentModel.IContainer components;

        /// <summary>Image list used for context menu</summary>
        protected System.Windows.Forms.ImageList m_ctrlMenuImages;

        /// <summary>Media viewer control</summary>
        private FTI.Trialmax.Controls.CTmaxViewerCtrl m_ctrlViewer;

        /// <summary>Tertiary exchange for the link currently loaded in the viewer</summary>
        private FTI.Trialmax.Database.CDxQuaternary m_dxLink = null;

        #endregion Private Members

        #region Public Methods

        /// <summary>Default constructor</summary>
        public CMediaViewer()
        {
            //	Initialize the child windows
            InitializeComponent();

            //	Set the viewer properties
            m_ctrlViewer.AxIniFilename = PresentationOptionsFilename;
            m_ctrlViewer.AxIniSection = "PRESENTATION";
            m_ctrlViewer.AxAutoSave = true;
            m_ctrlViewer.EnablePlayerSimulation = true;

        }// public CMediaViewer()

        /// <summary>This method is called by the application to open the specified item</summary>
        /// <param name="tmaxItem">The item to be opened</param>
        /// <param name="ePane">The pane making the request</param>
        /// <returns>true if successful</returns>
        public override bool Open(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)
        {
            return Activate(tmaxItem, ePane);
        }

        /// <summary>This method is called by the application to activate the specified item</summary>
        /// <param name="tmaxItem">The item to be activated</param>
        /// <param name="ePane">The pane requesting activation</param>
        /// <returns>true if successful</returns>
        public override bool Activate(FTI.Shared.Trialmax.CTmaxItem tmaxItem, FTI.Shared.Trialmax.TmaxAppPanes ePane)
        {
            //	Set the record interfaces
            if (SetRecords(tmaxItem) == true)
            {
                //	Do we need to unload the viewer?
                if (m_dxPrimary == null)
                {
                    Unload();
                }
                else
                {
                    //	Is this pane visible?
                    if (PaneVisible == true)
                    {
                        return View();
                    }
                    else
                    {
                        m_bReload = true;
                    }

                }

            }

            return true;

        }// public override bool Activate(FTI.Shared.Trialmax.CTmaxItem tmaxItem)

        /// <summary>This method is called by the application to rotate the specified item</summary>
        /// <param name="tmaxItem">The item to be activated</param>
        /// <param name="bCounterClockwise">True for counter clockwise rotation</param>
        /// <returns>true if successful</returns>
        public bool Rotate(FTI.Shared.Trialmax.CTmaxItem tmaxItem, bool bCounterClockwise)
        {
            CDxMediaRecord dxRecord = null;
            Keys eKey = Keys.None;
            Keys eModifiers = Keys.None;

            //	Must be valid media
            if ((dxRecord = (CDxMediaRecord)(tmaxItem.GetMediaRecord())) == null)
                return false;

            //	Can this record be rotated?
            if (CanRotate(dxRecord) == false)
                return false;

            //	Set the record interfaces
            if ((SetRecords(tmaxItem) == true) || (m_bReload == true))
            {
                //	Do we have valid media?
                if (m_dxPrimary == null) return false;

                //	Load the media even if the pane is not active
                if (View() == false) return false;

                //	Clear this flag because the pane is loaded
                m_bReload = false;

            }// if(SetRecords(tmaxItem) == true)

            //	Make it look like the user pressed the hotkey
            if (bCounterClockwise == true)
                CTmaxMediaBar.GetKeys(TmaxMediaBarCommands.RotateCCW, ref eKey, ref eModifiers);
            else
                CTmaxMediaBar.GetKeys(TmaxMediaBarCommands.RotateCW, ref eKey, ref eModifiers);

            if ((m_ctrlViewer != null) && (m_ctrlViewer.IsDisposed == false))
            {
                m_ctrlViewer.OnKeyDown(eKey, eModifiers);
            }

            return true;

        }// public override bool Activate(FTI.Shared.Trialmax.CTmaxItem tmaxItem)

        #endregion Public Methods

        #region Protected Methods

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("MainToolbar");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ContextMenu");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("ContextMenu");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Properties");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Presentation");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Builder");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Tuner");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Print");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("LockRatio", "");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("LockRatio", "");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Presentation");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Builder");
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Tuner");
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Properties");
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Print");
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CMediaViewer));
            this.m_ctrlStatusBar = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.m_ultraToolbarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this.m_ctrlMenuImages = new System.Windows.Forms.ImageList(this.components);
            this._CMediaViewer_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._CMediaViewer_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._CMediaViewer_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._CMediaViewer_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.m_ctrlViewer = new FTI.Trialmax.Controls.CTmaxViewerCtrl();
            ((System.ComponentModel.ISupportInitialize)(this.m_ultraToolbarManager)).BeginInit();
            this.SuspendLayout();
            // 
            // m_ctrlStatusBar
            // 
            appearance1.TextHAlignAsString = "Center";
            this.m_ctrlStatusBar.Appearance = appearance1;
            this.m_ctrlStatusBar.BorderStyle = Infragistics.Win.UIElementBorderStyle.InsetSoft;
            this.m_ctrlStatusBar.Location = new System.Drawing.Point(0, 257);
            this.m_ctrlStatusBar.Name = "m_ctrlStatusBar";
            this.m_ctrlStatusBar.Size = new System.Drawing.Size(388, 23);
            this.m_ctrlStatusBar.TabIndex = 2;
            this.m_ctrlStatusBar.WrapText = false;
            // 
            // m_ultraToolbarManager
            // 
            this.m_ultraToolbarManager.DesignerFlags = 1;
            this.m_ultraToolbarManager.DockWithinContainer = this;
            this.m_ultraToolbarManager.ImageListSmall = this.m_ctrlMenuImages;
            this.m_ultraToolbarManager.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_ultraToolbarManager.ShowFullMenusDelay = 500;
            this.m_ultraToolbarManager.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.Office2003;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool1});
            ultraToolbar1.Text = "MainToolbar";
            ultraToolbar1.Visible = false;
            this.m_ultraToolbarManager.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            popupMenuTool2.SharedProps.Caption = "ContextMenu";
            buttonTool5.InstanceProps.IsFirstInGroup = true;
            stateButtonTool1.InstanceProps.IsFirstInGroup = true;
            popupMenuTool2.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool2,
            buttonTool3,
            buttonTool4,
            buttonTool5,
            stateButtonTool1});
            appearance2.Image = 6;
            stateButtonTool2.SharedProps.AppearancesSmall.Appearance = appearance2;
            stateButtonTool2.SharedProps.Caption = "Fixed Aspect Ratio";
            appearance3.Image = 2;
            buttonTool6.SharedProps.AppearancesSmall.Appearance = appearance3;
            buttonTool6.SharedProps.Caption = "Open in Presentation ...";
            appearance4.Image = 5;
            buttonTool7.SharedProps.AppearancesSmall.Appearance = appearance4;
            buttonTool7.SharedProps.Caption = "Open in Builder";
            appearance5.Image = 3;
            buttonTool8.SharedProps.AppearancesSmall.Appearance = appearance5;
            buttonTool8.SharedProps.Caption = "Open in Tuner";
            appearance6.Image = 1;
            buttonTool9.SharedProps.AppearancesSmall.Appearance = appearance6;
            buttonTool9.SharedProps.Caption = "Open in Properties";
            appearance7.Image = 4;
            buttonTool10.SharedProps.AppearancesSmall.Appearance = appearance7;
            buttonTool10.SharedProps.Caption = "Print ...";
            buttonTool10.SharedProps.Shortcut = System.Windows.Forms.Shortcut.CtrlP;
            this.m_ultraToolbarManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool2,
            stateButtonTool2,
            buttonTool6,
            buttonTool7,
            buttonTool8,
            buttonTool9,
            buttonTool10});
            this.m_ultraToolbarManager.BeforeToolDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventHandler(this.OnUltraBeforeDropDown);
            this.m_ultraToolbarManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.OnUltraToolClick);
            this.m_ultraToolbarManager.AfterToolCloseup += new Infragistics.Win.UltraWinToolbars.ToolDropdownEventHandler(this.OnUltraAfterCloseup);
            this.m_ultraToolbarManager.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.OnUltraBeforeToolbarListDropdown);
            // 
            // m_ctrlMenuImages
            // 
            this.m_ctrlMenuImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("m_ctrlMenuImages.ImageStream")));
            this.m_ctrlMenuImages.TransparentColor = System.Drawing.Color.Magenta;
            this.m_ctrlMenuImages.Images.SetKeyName(0, "");
            this.m_ctrlMenuImages.Images.SetKeyName(1, "");
            this.m_ctrlMenuImages.Images.SetKeyName(2, "");
            this.m_ctrlMenuImages.Images.SetKeyName(3, "");
            this.m_ctrlMenuImages.Images.SetKeyName(4, "");
            this.m_ctrlMenuImages.Images.SetKeyName(5, "");
            this.m_ctrlMenuImages.Images.SetKeyName(6, "");
            this.m_ctrlMenuImages.Images.SetKeyName(7, "");
            // 
            // _CMediaViewer_Toolbars_Dock_Area_Left
            // 
            this._CMediaViewer_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CMediaViewer_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._CMediaViewer_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._CMediaViewer_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CMediaViewer_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 25);
            this._CMediaViewer_Toolbars_Dock_Area_Left.Name = "_CMediaViewer_Toolbars_Dock_Area_Left";
            this._CMediaViewer_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 232);
            this._CMediaViewer_Toolbars_Dock_Area_Left.ToolbarsManager = this.m_ultraToolbarManager;
            // 
            // _CMediaViewer_Toolbars_Dock_Area_Right
            // 
            this._CMediaViewer_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CMediaViewer_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._CMediaViewer_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._CMediaViewer_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CMediaViewer_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(388, 25);
            this._CMediaViewer_Toolbars_Dock_Area_Right.Name = "_CMediaViewer_Toolbars_Dock_Area_Right";
            this._CMediaViewer_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 232);
            this._CMediaViewer_Toolbars_Dock_Area_Right.ToolbarsManager = this.m_ultraToolbarManager;
            // 
            // _CMediaViewer_Toolbars_Dock_Area_Top
            // 
            this._CMediaViewer_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CMediaViewer_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._CMediaViewer_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._CMediaViewer_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CMediaViewer_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._CMediaViewer_Toolbars_Dock_Area_Top.Name = "_CMediaViewer_Toolbars_Dock_Area_Top";
            this._CMediaViewer_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(388, 25);
            this._CMediaViewer_Toolbars_Dock_Area_Top.ToolbarsManager = this.m_ultraToolbarManager;
            // 
            // _CMediaViewer_Toolbars_Dock_Area_Bottom
            // 
            this._CMediaViewer_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CMediaViewer_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._CMediaViewer_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._CMediaViewer_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CMediaViewer_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 257);
            this._CMediaViewer_Toolbars_Dock_Area_Bottom.Name = "_CMediaViewer_Toolbars_Dock_Area_Bottom";
            this._CMediaViewer_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(388, 0);
            this._CMediaViewer_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.m_ultraToolbarManager;
            // 
            // m_ctrlViewer
            // 
            this.m_ctrlViewer.AxAutoSave = false;
            this.m_ctrlViewer.AxIniFilename = "";
            this.m_ctrlViewer.AxIniSection = "";
            this.m_ctrlViewer.BackColor = System.Drawing.SystemColors.Control;
            this.m_ctrlViewer.EnablePlayerSimulation = false;
            this.m_ctrlViewer.EnableToolbar = true;
            this.m_ctrlViewer.Location = new System.Drawing.Point(46, 64);
            this.m_ctrlViewer.LockVideoRange = true;
            this.m_ctrlViewer.Name = "m_ctrlViewer";
            this.m_ctrlViewer.PlayOnLoad = false;
            this.m_ctrlViewer.ShowToolbar = true;
            this.m_ctrlViewer.SimulationText = "";
            this.m_ctrlViewer.Size = new System.Drawing.Size(296, 152);
            this.m_ctrlViewer.TabIndex = 8;
            this.m_ctrlViewer.UseScreenRatio = false;
            this.m_ctrlViewer.ZapSourceFile = "";
            // 
            // CMediaViewer
            // 
            this.m_ultraToolbarManager.SetContextMenuUltra(this, "ContextMenu");
            this.Controls.Add(this.m_ctrlViewer);
            this.Controls.Add(this._CMediaViewer_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._CMediaViewer_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._CMediaViewer_Toolbars_Dock_Area_Top);
            this.Controls.Add(this._CMediaViewer_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this.m_ctrlStatusBar);
            this.Name = "CMediaViewer";
            this.Size = new System.Drawing.Size(388, 280);
            ((System.ComponentModel.ISupportInitialize)(this.m_ultraToolbarManager)).EndInit();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// This method is called by the application to initialize the pane
        /// </summary>
        /// <returns>true if successful</returns>
        public override bool Initialize(CXmlIni xmlINI)
        {
            //	Do the base class initialization first
            if (base.Initialize(xmlINI) == false)
                return false;

            //	Make sure we are on the correct section
            xmlINI.SetSection(m_strPaneName);

            //	Are we in run mode?
            if (m_ctrlViewer != null)
            {
                //	Attach the events
                m_tmaxEventSource.Attach(m_ctrlViewer.EventSource);
                m_ctrlViewer.EventSource.MouseDblClickEvent += new System.Windows.Forms.MouseEventHandler(this.OnMouseDblClick);
                m_ctrlViewer.TmxEvent += new FTI.Trialmax.ActiveX.TmxEventHandler(this.OnTmxEvent);
                m_ctrlViewer.OnRequestPresentation += new TmxEventHandler(m_ctrlViewer_OnRequestPresentation);

                m_ctrlViewer.UseScreenRatio = xmlINI.ReadBool(KEY_LOCKED_RATIO, true);

                if (m_ctrlViewer.Initialize(false) == false) return false;

                //	Create the Tmview and Tmmovie viewers 
                if (m_ctrlViewer.Initialize("initialize.bmp") == false) return false;
                if (m_ctrlViewer.Initialize("initialize.mpg") == false) return false;

                return true;
            }
            else
            {
                return false;
            }

        }// public override bool Initialize()

        void m_ctrlViewer_OnRequestPresentation(object objSender, CTmxEventArgs Args)
        {
            if (OnRequestPresentation != null)
                OnRequestPresentation(null, null);
            //MessageBox.Show("Presentation Mode");


        }

        /// <summary>
        /// This method is called by the application when it is about to terminate
        /// </summary>
        /// <remarks>Derived classes should override for custom shutdown</remarks>
        public override void Terminate(CXmlIni xmlINI)
        {
            //	Make sure we are on the correct section
            xmlINI.SetSection(m_strPaneName);

            //	Are we in run mode?
            if (m_ctrlViewer != null)
            {
                xmlINI.Write(KEY_LOCKED_RATIO, m_ctrlViewer.UseScreenRatio);

                m_ctrlViewer.Terminate();
            }

            //	Do the base class processing
            base.Terminate(xmlINI);
        }

        /// <summary>
        /// This method is called by the application to notify the panes to refresh their text
        /// </summary>
        public override void RefreshText()
        {
            SetStatusText();
        }

        /// <summary>This method handles all KeyDown notifications from the application</summary>
        /// <param name="eKey">The key being pressed</param>
        ///	<param name="eModifiers">The current control/shift/alt key states</param>
        /// <returns>true if processed</returns>
        public override bool OnKeyDown(Keys eKey, Keys eModifiers)
        {
            if ((m_ctrlViewer != null) && (m_ctrlViewer.IsDisposed == false))
            {
                if (PaneVisible == true)
                {
                    return m_ctrlViewer.OnKeyDown(eKey, eModifiers);
                }

            }

            return false;

        }// public override bool OnKeyDown(Keys eKey, Keys eModifiers)

        /// <summary>This method handles all Hotkey notifications from the application</summary>
        /// <param name="eHotkey">The enumerated hotkey</param>
        /// <returns>true if processed by the pane</returns>
        public override bool OnHotkey(TmaxHotkeys eHotkey)
        {
            MediaViewerCommands eCommand = MediaViewerCommands.Invalid;

            //	Which hotkey has been pressed?
            switch (eHotkey)
            {
                case TmaxHotkeys.OpenPresentation:

                    eCommand = MediaViewerCommands.Presentation;
                    break;

                case TmaxHotkeys.Print:

                    eCommand = MediaViewerCommands.Print;
                    break;

                default:

                    break;
            }

            //	Did this hotkey translate to a command?
            if (eCommand != MediaViewerCommands.Invalid)
            {
                //	Is this command enabled
                if (GetCommandEnabled(eCommand) == true)
                {
                    OnCommand(eCommand);
                }

            }// if(eCommand != MediaViewerCommands.Invalid)

            return (eCommand != MediaViewerCommands.Invalid);

        }// public override bool OnHotkey(TmaxHotkeys eHotkey)

        /// <summary>This function is called when the the user has closed the Presentation Options form</summary>
        /// <param name="bCancelled">true if the user cancelled the operation</param>
        public override void OnAfterSetPresentationOptions(bool bCancelled)
        {
            if ((bCancelled == false) && (m_ctrlViewer != null) && (m_ctrlViewer.IsDisposed == false))
            {
                m_ctrlViewer.AxSetProperties();
            }

        }

        /// <summary>This function is called when the PresentationOptionsFilename property changes</summary>
        protected override void OnPresentationOptionsFilenameChanged()
        {
            if ((m_ctrlViewer != null) && (m_ctrlViewer.IsDisposed == false))
            {
                m_ctrlViewer.AxIniFilename = PresentationOptionsFilename;
            }

        }

        /// <summary>This function is called to resize and reposition the panes child controls</summary>
        protected override void RecalcLayout()
        {
            if (m_ctrlViewer == null) return;
            if (m_ctrlViewer.IsDisposed == true) return;

            if ((m_ctrlStatusBar != null) && (m_ctrlStatusBar.IsDisposed == false))
            {
                m_ctrlViewer.SetBounds(0, 0, this.Width, this.Height - m_ctrlStatusBar.Height, BoundsSpecified.All);
            }
            else
            {
                m_ctrlViewer.SetBounds(0, 0, this.Width, this.Height);
            }

        }// RecalcLayout()

        /// <summary>This function is called to set the status bar text</summary>
        protected void SetStatusText()
        {
            string strText = "";

            if (m_ctrlStatusBar == null) return;
            if (m_ctrlStatusBar.IsDisposed == true) return;

            //	Do we have media loaded?
            if (m_dxPrimary != null)
            {
                //	Are we viewing a scene?
                if (m_dxLink != null)
                {
                    strText = m_dxLink.GetText();
                }
                if (m_dxScene != null)
                {
                    strText = m_dxScene.GetText();
                }
                else
                {
                    if (m_dxTertiary != null)
                        strText = m_dxTertiary.GetText(TmaxTextModes.Barcode);
                    else if (m_dxSecondary != null)
                        strText = m_dxSecondary.GetText(TmaxTextModes.Barcode);
                    else
                        strText = m_dxPrimary.GetText(TmaxTextModes.Barcode);

                }
            }

            m_ctrlStatusBar.Text = strText;

        }// SetStatusText()

        /// <summary> This function is called to set navigator position for the viewer</summary>
        protected void SetNavigatorPosition()
        {
            int iPosition = 0;
            int iTotal = 0;

            if (m_ctrlViewer == null) return;
            if (m_ctrlViewer.IsDisposed == true) return;

            //	Do we have document loaded?
            if ((m_dxPrimary != null) && (m_dxPrimary.MediaType == TmaxMediaTypes.Document))
            {
                //	Can't be viewing a treatment or scene
                if ((m_dxTertiary == null) && (m_dxScene == null) && (m_dxLink == null))
                {
                    //	Do we have the secondary record?
                    if (m_dxSecondary != null)
                    {
                        iPosition = m_dxPrimary.Secondaries.IndexOf(m_dxSecondary) + 1;
                        iTotal = m_dxPrimary.Secondaries.Count;
                    }
                    else
                    {
                        iPosition = 1;
                        iTotal = (int)(m_dxPrimary.GetChildCount());
                    }

                }

            }

            m_ctrlViewer.SetNavigatorPosition(iPosition, iTotal);

        }// void SetNavigatorPosition()

        /// <summary>This method will unload the viewer</summary>
        protected bool Unload()
        {
            //	Unload the viewer 
            if ((m_ctrlViewer != null) && (m_ctrlViewer.IsDisposed == false))
            {
                m_ctrlViewer.Unload();
                m_ctrlViewer.SetNavigatorPosition(0, 0);
            }

            //	Reset the exchange objects
            m_dxPrimary = null;
            m_dxSecondary = null;
            m_dxTertiary = null;

            m_bTextEditorActive = false;

            SetStatusText();

            //	NOTE: DO NOT clear the local scene/link reference. This
            //		  allows us to respond to updates when the user changes
            //		  from a hidden link to a visible link

            return true;

        }// protected void Unload()

        /// <summary>This method is called to set the record interfaces using the specified event item</summary>
        /// <param name="tmaxItem">The TrialMax event item</param>
        /// <returns>true if the values have changed</returns>
        protected bool SetRecords(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
        {
            CDxPrimary dxPrimary = (CDxPrimary)tmaxItem.IPrimary;
            CDxSecondary dxSecondary = (CDxSecondary)tmaxItem.ISecondary;
            CDxTertiary dxTertiary = (CDxTertiary)tmaxItem.ITertiary;
            CDxQuaternary dxQuaternary = (CDxQuaternary)tmaxItem.IQuaternary;
            CDxSecondary dxScene = null;
            CDxQuaternary dxLink = null;
            bool bChanged = false;

            //	Are we loading a link?
            if (dxQuaternary != null)
            {
                Debug.Assert(tmaxItem.ParentItem != null);
                Debug.Assert(tmaxItem.ParentItem.GetMediaRecord() != null);
                Debug.Assert(tmaxItem.ParentItem.MediaType == TmaxMediaTypes.Scene);

                dxLink = dxQuaternary;
                dxScene = (CDxSecondary)tmaxItem.ParentItem.GetMediaRecord();

                //	Get the source record for this link
                if (dxQuaternary.GetSource() != null)
                {
                    if (dxLink.GetSource().GetMediaLevel() == TmaxMediaLevels.Tertiary)
                    {
                        dxTertiary = (CDxTertiary)dxLink.GetSource();
                        dxSecondary = dxTertiary.Secondary;
                        Debug.Assert(dxSecondary != null);
                        if (dxSecondary != null)
                        {
                            dxPrimary = dxSecondary.Primary;
                        }
                        else
                        {
                            dxPrimary = null;
                            dxSecondary = null;
                            dxTertiary = null;
                            dxQuaternary = null;
                            dxScene = null;
                            dxLink = null;
                        }

                    }
                    else if (dxLink.Source.GetMediaLevel() == TmaxMediaLevels.Secondary)
                    {
                        dxTertiary = null;
                        dxSecondary = (CDxSecondary)dxLink.GetSource();
                        dxPrimary = dxSecondary.Primary;
                        Debug.Assert(dxPrimary != null);
                        if (dxPrimary == null)
                        {
                            dxSecondary = null;
                            dxScene = null;
                            dxLink = null;
                        }

                    }
                    else
                    {
                        //	Shouldn't reach this point
                        Debug.Assert(false);
                        dxPrimary = null;
                        dxSecondary = null;
                        dxTertiary = null;
                        dxScene = null;
                        dxLink = null;
                    }

                }
                else
                {
                    //	Force the viewer to unload (probably a hidden link)
                    dxPrimary = null;
                    dxSecondary = null;
                    dxTertiary = null;
                }

            }

            //	Is this item associated with a script
            else if ((dxPrimary != null) && (dxPrimary.MediaType == TmaxMediaTypes.Script))
            {
                //	Do we need to get the first scene?
                if (dxSecondary == null)
                {
                    //	Do we need to fill the secondary collection?
                    if (dxPrimary.Secondaries.Count == 0)
                        dxPrimary.Fill();

                    if (dxPrimary.Secondaries.Count > 0)
                        dxSecondary = dxPrimary.Secondaries[0];
                }

                //	Do we have a valid scene to load?
                if ((dxSecondary != null) && (dxSecondary.GetSource() != null))
                {
                    //	Don't want to loose the scene reference
                    dxScene = dxSecondary;

                    if (dxSecondary.GetSource().GetMediaLevel() == TmaxMediaLevels.Tertiary)
                    {
                        dxTertiary = (CDxTertiary)dxSecondary.GetSource();
                        dxSecondary = dxTertiary.Secondary;
                        Debug.Assert(dxSecondary != null);
                        if (dxSecondary != null)
                        {
                            dxPrimary = dxSecondary.Primary;
                        }
                        else
                        {
                            dxPrimary = null;
                            dxSecondary = null;
                            dxTertiary = null;
                            dxScene = null;
                        }

                    }
                    else if (dxSecondary.GetSource().GetMediaLevel() == TmaxMediaLevels.Secondary)
                    {
                        dxTertiary = null;
                        dxSecondary = (CDxSecondary)dxSecondary.GetSource();
                        dxPrimary = dxSecondary.Primary;
                        Debug.Assert(dxPrimary != null);
                        if (dxPrimary == null)
                        {
                            dxSecondary = null;
                            dxScene = null;
                        }

                    }
                    else
                    {
                        //	Shouldn't reach this point
                        Debug.Assert(false);
                        dxPrimary = null;
                        dxSecondary = null;
                        dxTertiary = null;
                        dxScene = null;
                    }

                }
                else
                {
                    //	Force the viewer to unload
                    dxPrimary = null;
                    dxSecondary = null;
                }

            }// if((dxPrimary != null) && (dxPrimary.MediaType == TmaxMediaTypes.Script))

            //	Is this a composite media type?
            if ((dxPrimary != null) && (CTmaxMediaTypes.IsCompositeMedia(dxPrimary.MediaType) == true))
            {
                m_bCompositeMedia = true;

                //	We need the first secondary record because the primary
                //	record does not store the exported filename
                if (dxSecondary == null)
                {
                    if (dxPrimary.Secondaries.Count == 0)
                        dxPrimary.Fill();

                    if (dxPrimary.Secondaries.Count > 0)
                        dxSecondary = dxPrimary.Secondaries[0];
                }

            }
            else
            {
                m_bCompositeMedia = false;
            }

            //	Save the record interfaces
            if (ReferenceEquals(dxPrimary, m_dxPrimary) == false)
            {
                m_dxPrimary = dxPrimary;
                bChanged = true;
            }

            if (ReferenceEquals(dxSecondary, m_dxSecondary) == false)
            {
                m_dxSecondary = dxSecondary;
                bChanged = true;
            }

            if (ReferenceEquals(dxTertiary, m_dxTertiary) == false)
            {
                m_dxTertiary = dxTertiary;
                bChanged = true;
            }

            if (ReferenceEquals(dxScene, m_dxScene) == false)
            {
                m_dxScene = dxScene;
                bChanged = true;
            }
            if (ReferenceEquals(dxLink, m_dxLink) == false)
            {
                m_dxLink = dxLink;
                bChanged = true;
            }

            return bChanged;

        }// protected bool SetRecords(FTI.Shared.Trialmax.CTmaxItem tmaxItem)

        /// <summary>This function is called when the value of the Database property changes</summary>
        protected override void OnDatabaseChanged()
        {
            //	These are not reset in the call to Unload so we
            //	have to reset them here
            m_dxScene = null;
            m_dxLink = null;

            //	Unload the viewer whenever the database changes
            Unload();

        }// OnDatabaseChanged()

        /// <summary>
        /// This method is called by the application to when the item gets deleted
        /// </summary>
        /// <param name="tmaxItem">The item that has been deleted</param>
        public override void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
        {
            bool bUnload = false;

            //	Make sure the current record is still valid
            if (m_dxTertiary != null)
            {
                if (m_tmaxDatabase.IsValidRecord(m_dxTertiary) == false)
                    bUnload = true;
            }
            else if (m_dxSecondary != null)
            {
                if (m_tmaxDatabase.IsValidRecord(m_dxSecondary) == false)
                    bUnload = true;
            }
            else if (m_dxPrimary != null)
            {
                if (m_tmaxDatabase.IsValidRecord(m_dxPrimary) == false)
                    bUnload = true;
            }

            if (m_dxScene != null)
            {
                if (m_tmaxDatabase.IsValidRecord(m_dxScene) == false)
                    m_dxScene = null;
            }

            if (m_dxLink != null)
            {
                if (m_tmaxDatabase.IsValidRecord(m_dxLink) == false)
                {
                    m_dxScene = null;
                    m_dxLink = null;
                }
            }

            if (bUnload == true)
                Unload();
            else
                SetStatusText();
        }

        /// <summary>
        /// This method is called by the application to when the item's child collection has been reordered
        /// </summary>
        /// <param name="tmaxItem">The item that owns the child collection</param>
        public override void OnReordered(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
        {
            //	Ignore case codes
            if (tmaxItem.DataType == TmaxDataTypes.CaseCode) return;

            //	Status bar text may have changed
            SetStatusText();
        }

        /// <summary>This method is called by the application when it adds new media to the database</summary>
        /// <param name="tmaxParent">TrialMax event item that identifies the parent record</param>
        /// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
        public override void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
        {
            //	Status bar text may have changed
            SetStatusText();
        }

        /// <summary>This function is called when the ApplicationOptions property changes</summary>
        public override void OnApplicationActivated()
        {
            //	Make sure the user hasn't returned to the application after
            //	modifying the current media
            CheckMedia();
        }

        /// <summary>
        /// This method is called by the application to when the item has been updated by the user
        /// </summary>
        /// <param name="tmaxItem">The item that has been updated</param>
        public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
        {
            string strFileSpec = "";
            CTmaxItem tmaxLink = null;
            bool bReload = false;

            //	Has the active link changed?
            if ((m_dxLink != null) && (ReferenceEquals(m_dxLink, tmaxItem.GetMediaRecord()) == true))
            {
                //	Get the path to the link's source file
                if (m_dxLink.GetSource() != null)
                {
                    strFileSpec = m_tmaxDatabase.GetFileSpec(m_dxLink.GetSource());

                    //	Has the source file changed?
                    if (String.Compare(strFileSpec, m_ctrlViewer.Filename, true) != 0)
                        bReload = true;
                }
                else
                {
                    //	Has the user changed this link to be hidden?
                    if (m_dxPrimary != null)
                        bReload = true; //	Have to clear out the viewer
                }

                //	Do we need to reload?
                if (bReload == true)
                {
                    tmaxLink = new CTmaxItem(m_dxLink);
                    tmaxLink.ParentItem = new CTmaxItem(m_dxScene);
                    Activate(tmaxLink, TmaxAppPanes.Viewer);
                }
                else
                {
                    //	Make sure the status text is correct
                    SetStatusText();
                }
            }
            if ((m_dxTertiary != null) && (ReferenceEquals(m_dxTertiary, tmaxItem.GetMediaRecord()) == true))
            {
                //	Are we supposed to reload the treatment
                //
                //	NOTE: It may have been modified from within TmaxPresentation
                if ((m_dxTertiary.MediaType == TmaxMediaTypes.Treatment) && (tmaxItem.Reload == true))
                {
                    //	Is this pane visible?
                    if (PaneVisible == true)
                    {
                        View();
                    }
                    else
                    {
                        m_bReload = true;
                    }
                }
                else
                {
                    SetStatusText();
                }

            }
            else
            {
                //	Status bar text may have changed
                SetStatusText();
            }

        }// public override void OnUpdated(FTI.Shared.Trialmax.CTmaxItem tmaxItem)

        /// <summary>This function is called when the PaneVisible property changes</summary>
        protected override void OnPaneVisibleChanged()
        {
            //	Is this pane being activated?
            if (PaneVisible == true)
            {
                if (m_bReload == true)
                {
                    View();
                    m_bReload = false;
                }
                else
                {
                    SetStatusText();
                    m_ctrlViewer.ProcessRequest(new CTmxRequest(TmxActions.ShowCallouts));
                }

            }
            else
            {
                //	Make sure we're not playing video
                if (m_ctrlViewer != null)
                {
                    m_ctrlViewer.Stop();
                    m_ctrlViewer.ProcessRequest(new CTmxRequest(TmxActions.HideCallouts));
                }
            }

        }// protected override void OnPaneVisibleChanged()

        /// <summary>This method is called internally to view the active record</summary>
        /// <returns>true if successful</returns>
        public bool View()
        {
            string strFileSpec = "";
            string strZapSource = "";
            double dFrom = 0;
            double dTo = 0;
            TmaxMediaTypes eType = TmaxMediaTypes.Unknown;
            string strMsg = "";
            bool bSuccessful = false;

            //	We must at least have a primary record
            if (m_dxPrimary == null) return false;
            if (m_tmaxDatabase == null) return false;
            if (m_ctrlViewer == null) return false;

            if (m_dxTertiary != null)
            {
                if (m_dxTertiary.MediaType == TmaxMediaTypes.Clip ||
                   m_dxTertiary.MediaType == TmaxMediaTypes.Designation)
                {
                    eType = TmaxMediaTypes.Segment;

                    //	The secondary owner is the actual file
                    strFileSpec = m_tmaxDatabase.GetFileSpec(m_dxTertiary.Secondary);

                    if (m_dxTertiary.GetExtent() != null)
                    {
                        dFrom = m_dxTertiary.GetExtent().Start;
                        dTo = m_dxTertiary.GetExtent().Stop;
                    }

                }
                else
                {
                    eType = m_dxTertiary.MediaType;
                    strFileSpec = m_tmaxDatabase.GetFileSpec(m_dxTertiary);
                    strZapSource = m_tmaxDatabase.GetFileSpec(m_dxTertiary.Secondary);
                }

            }
            else if (m_dxSecondary != null)
            {
                eType = m_dxSecondary.MediaType;

                strFileSpec = m_tmaxDatabase.GetFileSpec(m_dxSecondary);

                if (m_dxSecondary.MediaType == TmaxMediaTypes.Segment)
                {
                    if (m_dxSecondary.GetExtent() != null)
                    {
                        dFrom = m_dxSecondary.GetExtent().Start;
                        dTo = m_dxSecondary.GetExtent().Stop;
                    }
                }
                else if (m_dxSecondary.MediaType == TmaxMediaTypes.Scene)
                {
                    //	Redirect to use the source type
                    eType = m_dxSecondary.SourceType;
                }
                else if (m_dxSecondary.MediaType == TmaxMediaTypes.Slide)
                {
                    //	Make sure the exported image is up to date
                    m_tmaxDatabase.CheckSlide(m_dxSecondary);
                }

            }
            else
            {
                eType = m_dxPrimary.MediaType;
                strFileSpec = m_tmaxDatabase.GetFileSpec(m_dxPrimary);
            }

            //	Are we supposed to be unloading the viewer?
            if ((strFileSpec == null) || (strFileSpec.Length == 0))
            {
                return Unload();
            }

            //	Update the status bar
            SetStatusText();

            //	Make sure the specified file exists
            if (System.IO.File.Exists(strFileSpec) == false)
            {
                //	Set the simulation text if this is a video segment
                if (eType == TmaxMediaTypes.Segment)
                {
                    m_ctrlViewer.SimulationText = (strFileSpec + " not found");
                    m_ctrlViewer.EnableToolbar = false;
                }
                else
                {
                    m_ctrlViewer.Unload();

                    strMsg = String.Format("Unable to locate {0} to load the viewer", strFileSpec);
                    MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;

                }// if(eType == TmaxMediaTypes.Segment)

            }// if(System.IO.File.Exists(strFileSpec) == false)
            else
            {
                m_ctrlViewer.EnableToolbar = true;
            }

            //	Make sure the treatment source file exists
            if ((strZapSource.Length > 0) &&
               (System.IO.File.Exists(strZapSource) == false))
            {
                m_ctrlViewer.Unload();

                strMsg = String.Format("Unable to locate {0} to load the treatment's source file", strZapSource);
                MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            //	What type of media are we loading?
            switch (eType)
            {
                case TmaxMediaTypes.Document:
                case TmaxMediaTypes.Powerpoint:
                case TmaxMediaTypes.Page:
                case TmaxMediaTypes.Slide:
                case TmaxMediaTypes.Treatment:

                    //	Set (or clear) the zap source file specification
                    m_ctrlViewer.ZapSourceFile = strZapSource;

                    if (m_ctrlViewer.View(strFileSpec, !m_bCompositeMedia) == true)
                    {
                        m_ctrlViewer.ProcessRequest(new CTmxRequest(TmxActions.ShowCallouts));
                        bSuccessful = true;
                    }
                    else
                    {
                        bSuccessful = false;
                    }
                    break;

                case TmaxMediaTypes.Deposition:
                case TmaxMediaTypes.Recording:
                case TmaxMediaTypes.Segment:

                    if (m_ctrlViewer.Play(strFileSpec, dFrom, dTo) == true)
                    {
                        bSuccessful = true;
                    }
                    else
                    {
                        bSuccessful = false;
                    }
                    break;

                default:

                    bSuccessful = false;
                    break;

            }// switch(eType)

            //	Update the navigator controls
            //
            //	NOTE:	We have to do this last to be sure the appropriate viewer is loaded
            SetNavigatorPosition();

            return bSuccessful;

        }// protected bool View()

        /// <summary>This method handles all TmxEvents trapped by this control</summary>
        /// <param name="O">The object sending the event</param>
        /// <param name="Args">Tmx event arguments</param>
        protected void OnTmxEvent(object O, FTI.Trialmax.ActiveX.CTmxEventArgs Args)
        {
            //	Is this an action event?
            if (Args.Event == TmxEvents.Action)
            {
                switch (Args.Action)
                {
                    case FTI.Trialmax.ActiveX.TmxActions.SaveZap:

                        OnTmxSaveZap(Args);
                        break;

                    case FTI.Trialmax.ActiveX.TmxActions.UpdateZap:

                        OnTmxUpdateZap(Args);
                        break;

                    case FTI.Trialmax.ActiveX.TmxActions.RotateImage:

                        OnTmxRotateImage(Args);
                        break;

                    case FTI.Trialmax.ActiveX.TmxActions.First:
                    case FTI.Trialmax.ActiveX.TmxActions.Last:
                    case FTI.Trialmax.ActiveX.TmxActions.Next:
                    case FTI.Trialmax.ActiveX.TmxActions.Previous:
                    case FTI.Trialmax.ActiveX.TmxActions.GoTo:

                        OnTmxNavigate(Args);
                        break;

                    default:

                        break;

                }

            }// if(Args.Event == TmxEvents.Action)

            else if (Args.Event == TmxEvents.QueryContinue)
            {
                switch (Args.Action)
                {
                    case FTI.Trialmax.ActiveX.TmxActions.SaveZap:

                        OnTmxQuerySaveZap(Args);
                        break;

                    case FTI.Trialmax.ActiveX.TmxActions.UpdateZap:

                        OnTmxQueryUpdateZap(Args);
                        break;

                    default:

                    case FTI.Trialmax.ActiveX.TmxActions.RotateImage:

                        OnTmxQueryRotateImage(Args);
                        break;

                }

            }

            //	Has the user created a new callout?
            else if (Args.Event == TmxEvents.CreatedCallout)
            {
                //	Post a message back to ourself to force selection
                //	of the parent because the callout is going to grab
                //	the focus after it's displayed
                FTI.Shared.Win32.User.PostMessage(Handle, FTI.Shared.Win32.User.WM_MOUSEACTIVATE, 0, 0);
            }

            //	Has the user started editing a text annotation?
            else if (Args.Event == TmxEvents.StartEditAnnText)
            {
                m_bTextEditorActive = true;
            }

            //	Has the user started editing a text annotation?
            else if (Args.Event == TmxEvents.FinishEditAnnText)
            {
                m_bTextEditorActive = false;
            }

        }// protected void OnTmxEvent(object O, FTI.Trialmax.ActiveX.CTmxEventArgs Args)

        /// <summary>Overridden base class member to perform custom message processing</summary>
        /// <param name="m">the message being sent to the window</param>
        protected override void DefWndProc(ref Message m)
        {
            //	Perform the base class processing
            base.DefWndProc(ref m);

            //	Is the user clicking on one of the grids?
            //
            //	NOTE:	We trap this because there is a discontinuity between the 
            //			property grid control and the parent pane. For some reason
            //			the pane does not get selected when the user clicks on
            //			the grid
            if (m.Msg == FTI.Shared.Win32.User.WM_MOUSEACTIVATE)
            {
                //	Make sure the parent is selected
                this.Parent.Select();
            }

        }// protected override void DefWndProc(ref Message m)

        /// <summary>This method handles all SaveZap events fired by the child viewer</summary>
        /// <param name="Args">Tmx event arguments</param>
        protected void OnTmxSaveZap(FTI.Trialmax.ActiveX.CTmxEventArgs Args)
        {
            string[] aFilenames = new string[1];
            CTmaxSourceFolder tmaxSource = null;
            CTmaxItem tmaxItem = null;
            CTmaxParameters tmaxParameters = null;

            Debug.Assert(m_dxPrimary != null);
            if (m_dxPrimary == null) return;

            //	We must have a secondary record
            if (m_dxSecondary == null)
            {
                if (m_dxPrimary.Fill() == true)
                {
                    if (m_dxPrimary.Secondaries != null && (m_dxPrimary.Secondaries.Count > 0))
                    {
                        m_dxSecondary = m_dxPrimary.Secondaries[0];
                    }
                }
            }
            Debug.Assert(m_dxSecondary != null);
            if (m_dxSecondary == null) return;

            Debug.Assert(m_dxSecondary.MediaType == TmaxMediaTypes.Page);

            //	Initialize a source folder to pass with the command event
            aFilenames[0] = Args.Filename;
            tmaxSource = new CTmaxSourceFolder(aFilenames);

            //	Initialize an event item
            tmaxItem = new CTmaxItem();
            tmaxItem.SourceFolder = tmaxSource;
            tmaxItem.IPrimary = m_dxPrimary;
            tmaxItem.ISecondary = m_dxSecondary;

            //	Create the required parameters for the event
            tmaxParameters = new CTmaxParameters();
            tmaxParameters.Add(TmaxCommandParameters.Activate, true);

            //	Fire the command to add a new treatment to the database
            FireCommand(TmaxCommands.Add, tmaxItem, tmaxParameters);

            //	Delete the file
            //
            //	NOTE:	The database always copies the source
            try
            {
                System.IO.File.Delete(Args.Filename);
            }
            catch
            {
            }

        }// protected void OnTmxSaveZap(FTI.Trialmax.ActiveX.CTmxEventArgs Args)

        /// <summary>This method handles all UpdateZap events fired by the child viewer</summary>
        /// <param name="Args">Tmx event arguments</param>
        protected void OnTmxUpdateZap(FTI.Trialmax.ActiveX.CTmxEventArgs Args)
        {
            CTmaxItem tmaxItem = null;

            Debug.Assert(m_dxPrimary != null);
            Debug.Assert(m_dxSecondary != null);
            Debug.Assert(m_dxTertiary != null);

            if ((m_dxPrimary == null) || (m_dxSecondary == null) || (m_dxTertiary == null)) return;

            //	Initialize an event item
            tmaxItem = new CTmaxItem();
            tmaxItem.IPrimary = m_dxPrimary;
            tmaxItem.ISecondary = m_dxSecondary;
            tmaxItem.ITertiary = m_dxTertiary;

            //	Fire the command to update the record
            FireCommand(TmaxCommands.Update, tmaxItem);

        }// protected void OnTmxSaveZap(FTI.Trialmax.ActiveX.CTmxEventArgs Args)

        /// <summary>This method handles all RotateImage events fired by the child viewer</summary>
        /// <param name="Args">Tmx event arguments</param>
        protected void OnTmxRotateImage(FTI.Trialmax.ActiveX.CTmxEventArgs Args)
        {
            Debug.Assert(m_dxPrimary != null);

            //	We must have a secondary record
            if ((m_dxPrimary != null) && (m_dxSecondary == null))
            {
                if (m_dxPrimary.Fill() == true)
                {
                    if (m_dxPrimary.Secondaries != null && (m_dxPrimary.Secondaries.Count > 0))
                    {
                        m_dxSecondary = m_dxPrimary.Secondaries[0];
                    }
                }
            }

            Debug.Assert(m_dxSecondary != null);
            Debug.Assert(m_dxSecondary.MediaType == TmaxMediaTypes.Page);

            if ((m_dxSecondary != null) &&
               (m_dxSecondary.MediaType == TmaxMediaTypes.Page))
            {
                //	Fire the command to update the record
                FireCommand(TmaxCommands.Update, new CTmaxItem(m_dxSecondary));
            }

        }// protected void OnTmxSaveZap(FTI.Trialmax.ActiveX.CTmxEventArgs Args)

        /// <summary>This method is called to determine if the current treatment can be updated</summary>
        /// <param name="Args">Tmx event arguments</param>
        protected void OnTmxQueryUpdateZap(CTmxEventArgs Args)
        {
            Debug.Assert(m_dxPrimary != null);
            Debug.Assert(m_dxSecondary != null);
            Debug.Assert(m_dxTertiary != null);

            if ((m_dxPrimary == null) ||
               (m_dxSecondary == null) ||
               (m_dxTertiary == null))
            {
                MessageBox.Show("Unable to perform the update. No treatment record is available",
                                "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Args.Continue = false;
                return;
            }

            //	Should the viewer be in fixed ratio mode?
            if ((Args.CalloutCount > 0) && (m_ctrlViewer.UseScreenRatio == false))
            {
                //	Make sure the user wants to continue
                if (MessageBox.Show("This treatment contains callouts but the viewer is not in fixed ratio mode. The callouts may not appear properly in Presentation.\n\nDo you want to continue?",
                                   "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    Args.Continue = false;
                    return;
                }

            }

            //	Everything is OK
            Args.Continue = true;

        }// protected void OnTmxConfirmUpdateZap(CTmxEventArgs Args)

        /// <summary>This method is called to determine if the current treatment can be saved</summary>
        /// <param name="Args">Tmx event arguments</param>
        protected void OnTmxQuerySaveZap(CTmxEventArgs Args)
        {
            Debug.Assert(m_dxPrimary != null);

            //	We must have a secondary record
            if ((m_dxPrimary != null) && (m_dxSecondary == null))
            {
                if (m_dxPrimary.Fill() == true)
                {
                    if (m_dxPrimary.Secondaries != null && (m_dxPrimary.Secondaries.Count > 0))
                    {
                        m_dxSecondary = m_dxPrimary.Secondaries[0];
                    }
                }
            }

            Debug.Assert(m_dxSecondary != null);
            Debug.Assert(m_dxSecondary.MediaType == TmaxMediaTypes.Page);

            if ((m_dxSecondary == null) ||
               (m_dxSecondary.MediaType != TmaxMediaTypes.Page))
            {
                MessageBox.Show("Unable to save the treatment. No parent page record is available",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Args.Continue = false;
                return;
            }

            //	Should the viewer be in fixed ratio mode?
            if ((Args.CalloutCount > 0) && (m_ctrlViewer.UseScreenRatio == false))
            {
                //	Make sure the user wants to continue
                if (MessageBox.Show("This treatment contains callouts but the viewer is not in fixed ratio mode. The callouts may not appear properly in Presentation.\n\nDo you want to continue?",
                    "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    Args.Continue = false;
                    return;
                }

            }

            //	Everything is OK
            Args.Continue = true;

        }// protected void OnTmxConfirmSaveZap(CTmxEventArgs Args)

        /// <summary>This method is called to determine if the current treatment can be saved</summary>
        /// <param name="Args">Tmx event arguments</param>
        protected void OnTmxQueryRotateImage(CTmxEventArgs Args)
        {
            Debug.Assert(m_dxPrimary != null);

            //	Is the user attempting to rotate a treatment?
            if ((m_dxTertiary != null) && (m_dxTertiary.MediaType == TmaxMediaTypes.Treatment))
            {
                //	Make sure the user wants to continue
                if (MessageBox.Show("You are about to rotate a treatment. Changes will not be saved unless you perform an update after rotating.\n\nDo you want to continue?",
                    "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    Args.Continue = false;
                    return;
                }

            }
            else
            {
                //	We must have a secondary record
                if ((m_dxPrimary != null) && (m_dxSecondary == null))
                {
                    if (m_dxPrimary.Fill() == true)
                    {
                        if (m_dxPrimary.Secondaries != null && (m_dxPrimary.Secondaries.Count > 0))
                        {
                            m_dxSecondary = m_dxPrimary.Secondaries[0];
                        }
                    }
                }

                Debug.Assert(m_dxSecondary != null);
                Debug.Assert(m_dxSecondary.MediaType == TmaxMediaTypes.Page);

                if ((m_dxSecondary == null) ||
                (m_dxSecondary.MediaType != TmaxMediaTypes.Page))
                {
                    MessageBox.Show("Unable to rotate the image. No associated page record is available",
                        "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Args.Continue = false;
                    return;
                }

                //	Has the user already created treatments for this page?
                if (m_dxSecondary.ChildCount > 0)
                {
                    //	Make sure the user wants to continue
                    if (MessageBox.Show("Treatments have already been created for this page.\n\nDo you want to continue?",
                        "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        Args.Continue = false;
                        return;
                    }

                }

            }

            //	Everything is OK
            Args.Continue = true;

        }// protected void OnTmxQueryRotateImage(CTmxEventArgs Args)

        /// <summary>This method handles all child viewer events related to navigation</summary>
        /// <param name="Args">Tmx event arguments</param>
        protected void OnTmxNavigate(FTI.Trialmax.ActiveX.CTmxEventArgs Args)
        {
            CTmaxItem tmaxItem = null;
            CDxSecondary dxSecondary = null;
            int iIndex = 0;

            //	Must have a primary document loaded
            if ((m_dxPrimary != null) && (m_dxPrimary.MediaType == TmaxMediaTypes.Document))
            {
                //	Can't be viewing a treatment or scene
                if ((m_dxTertiary == null) && (m_dxScene == null) && (m_dxLink == null))
                {
                    //	We have to have the secondaries
                    if (m_dxPrimary.Secondaries.Count == 0)
                        m_dxPrimary.Fill();
                    if (m_dxPrimary.Secondaries.Count == 0)
                        return;

                    //	Which command is being fired?
                    switch (Args.Action)
                    {
                        case TmxActions.First:

                            dxSecondary = m_dxPrimary.Secondaries[0];
                            break;

                        case TmxActions.Last:

                            dxSecondary = m_dxPrimary.Secondaries[m_dxPrimary.Secondaries.Count - 1];
                            break;

                        case TmxActions.Next:

                            if (m_dxSecondary != null)
                            {
                                if ((iIndex = m_dxPrimary.Secondaries.IndexOf(m_dxSecondary)) < (m_dxPrimary.Secondaries.Count - 1))
                                {
                                    dxSecondary = m_dxPrimary.Secondaries[iIndex + 1];
                                }

                            }
                            else
                            {
                                //	We must have been on the first page of the document
                                if (m_dxPrimary.Secondaries.Count > 1)
                                    dxSecondary = m_dxPrimary.Secondaries[1];
                            }
                            break;

                        case TmxActions.Previous:

                            if (m_dxSecondary != null)
                            {
                                if ((iIndex = m_dxPrimary.Secondaries.IndexOf(m_dxSecondary)) > 0)
                                {
                                    dxSecondary = m_dxPrimary.Secondaries[iIndex - 1];
                                }
                            }
                            break;

                        case TmxActions.GoTo:

                            iIndex = Args.GoTo - 1;

                            if ((iIndex >= 0) && (iIndex < m_dxPrimary.Secondaries.Count))
                            {
                                dxSecondary = m_dxPrimary.Secondaries[iIndex];
                            }
                            break;

                    }// switch(Args.Action)

                    //	Do we have a new record to activate?
                    if (dxSecondary != null)
                    {
                        //	Initialize an event item
                        tmaxItem = new CTmaxItem(dxSecondary);

                        //	Fire the command to activate the record
                        FireCommand(TmaxCommands.Activate, tmaxItem);
                    }

                }// if((m_dxTertiary == null) && (m_dxScene == null) && (m_dxLink == null))

            }// if((m_dxPrimary != null) && (m_dxPrimary.MediaType == TmaxMediaTypes.Document))

        }// protected void OnTmxNavigate(FTI.Trialmax.ActiveX.CTmxEventArgs Args)

        /// <summary>This method traps and handles double click events from the viewer control</summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The mouse event arguments</param>
        protected void OnMouseDblClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //	Don't bother if no media loaded
            if (m_dxPrimary == null) return;
            if (m_ctrlViewer == null) return;
            if (m_ctrlViewer.IsDisposed == true) return;

            //	What type of media?
            switch (m_dxPrimary.MediaType)
            {
                case TmaxMediaTypes.Powerpoint:

                    if (m_dxSecondary != null)
                        FireCommand(TmaxCommands.Edit, new CTmaxItem(m_dxSecondary));
                    else
                        FireCommand(TmaxCommands.Edit, new CTmaxItem(m_dxPrimary));

                    break;

                case TmaxMediaTypes.Recording:
                case TmaxMediaTypes.Deposition:

                    //	What it the current state of the viewer?
                    switch (m_ctrlViewer.State)
                    {
                        case TmxStates.Playing:

                            m_ctrlViewer.Pause();
                            break;

                        case TmxStates.Paused:
                        case TmxStates.Stopped:
                        case TmxStates.Loaded:

                            m_ctrlViewer.Resume();
                            break;

                    }// switch(m_ctrlViewer.State)

                    break;

                default:

                    break;

            }// switch(m_dxPrimary.MediaType)


        }// protected void OnMouseDblClick(object sender, System.Windows.Forms.MouseEventArgs e)

        #endregion Protected Methods

        /// <summary>Overridden by derived classes to provide access to the pane's toolbar manager</summary>
        /// <returns>The pane's toolbar manager if available</returns>
        protected override Infragistics.Win.UltraWinToolbars.UltraToolbarsManager GetUltraToolbarManager()
        {
            return m_ultraToolbarManager;
        }

        #region Private Methods

        /// <summary>This method is called to determine if the specified command should be enabled</summary>
        /// <param name="eCommand">The command enumeration</param>
        /// <returns>true if command should be enabled</returns>
        private bool GetCommandEnabled(MediaViewerCommands eCommand)
        {
            //	What is the command?
            switch (eCommand)
            {
                case MediaViewerCommands.Presentation:

                    if (GetActiveRecord() == null) return false;

                    //=====================================================
                    //	PLAY DEPOSITION : REMOVE THIS TEST FOR DEPOSITION
                    //=====================================================
                    if ((m_dxScene == null) &&
                       (m_dxPrimary.MediaType == TmaxMediaTypes.Deposition))
                        return false;
                    //=====================================================

                    else return true;

                case MediaViewerCommands.Properties:

                    return (GetActiveRecord() != null);

                case MediaViewerCommands.Print:

                    return ((m_tmaxDatabase != null) &&
                            (m_tmaxDatabase.Primaries != null) &&
                            (m_tmaxDatabase.Primaries.Count > 0));

                case MediaViewerCommands.Builder:
                case MediaViewerCommands.Tuner:

                    return (m_dxScene != null);

                case MediaViewerCommands.LockRatio:

                    return ((m_ctrlViewer != null) && (m_ctrlViewer.IsDisposed == false));

                default:

                    break;
            }

            return false;

        }// private virtual bool GetCommandEnabled(MediaViewerCommands eCommand)

        /// <summary>This method is called to get the shortcut key for the specified command</summary>
        /// <param name="eCommand">The tree pane command enumeration</param>
        /// <returns>The shortcut key if there is one</returns>
        private Shortcut GetCommandShortcut(MediaViewerCommands eCommand)
        {
            switch (eCommand)
            {
                case MediaViewerCommands.Print:

                    return Shortcut.CtrlP;

                default:

                    return Shortcut.None;

            }// switch(eCommand)

        }// protected virtual Shortcut GetCommandShortcut(MediaViewerCommands eCommand)

        /// <summary>
        /// This method is called internally to convert the key passed in
        ///	an Infragistics event to its associated command enumeration
        /// </summary>
        /// <param name="strKey">The Infragistic key identifier</param>
        /// <returns>The associated transcript pane command</returns>
        private MediaViewerCommands GetCommand(string strKey)
        {
            try
            {
                Array aCommands = Enum.GetValues(typeof(MediaViewerCommands));

                foreach (MediaViewerCommands eCommand in aCommands)
                {
                    if (eCommand.ToString() == strKey)
                        return eCommand;
                }

            }
            catch
            {
            }

            return MediaViewerCommands.Invalid;
        }

        /// <summary>This method handles events fired by the toolbar manager before displaying the popup menu</summary>
        /// <param name="sender">The toolbar manager firing the event</param>
        /// <param name="e">The Infragistics event argument object</param>
        private void OnUltraBeforeDropDown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventArgs e)
        {
            try
            {
                switch (e.Tool.Key)
                {
                    case "ContextMenu":

                        //	Is the user editing a text annotation?
                        if (m_bTextEditorActive == true)
                        {
                            //	This gets us around a problem with TMView ActiveX
                            //	control. It sends a right-mouse click when the text
                            //	box closes. That causes the context menu in this pane to pop up
                            e.Cancel = true;
                            m_bTextEditorActive = false;
                        }
                        else
                        {
                            //	Set the state of the selections in the menu
                            OnContextMenu();
                        }
                        break;
                }

            }
            catch
            {
            }

        }// OnUltraBeforeDropDown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolDropdownEventArgs e)

        /// <summary>This method handles events fired by the toolbar manager after displaying the popup menu</summary>
        /// <param name="sender">The toolbar manager firing the event</param>
        /// <param name="e">The Infragistics event argument object</param>
        private void OnUltraAfterCloseup(object sender, Infragistics.Win.UltraWinToolbars.ToolDropdownEventArgs e)
        {
            MediaViewerCommands eCommand = MediaViewerCommands.Invalid;

            //	Check each tool in the manager's collection
            foreach (ToolBase O in m_ultraToolbarManager.Tools)
            {
                //	Get the command for this tool
                if ((eCommand = GetCommand(O.Key)) != MediaViewerCommands.Invalid)
                {
                    //	Uninstall the shortcut if required
                    if (GetCommandShortcut(eCommand) != Shortcut.None)
                        O.SharedProps.Shortcut = Shortcut.None;
                }

            }// foreach(ToolBase O in m_ultraToolbarManager.Tools)

        }

        /// <summary>This event is fired by the toolbar manager when it is about to display the customize menu</summary>
        /// <param name="sender">The object sending the event</param>
        /// <param name="e">The cancelable event arguments</param>
        private void OnUltraBeforeToolbarListDropdown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventArgs e)
        {
            // Prevent this menu from coming up
            e.Cancel = true;
        }

        /// <summary>This method is called to check the current media to make sure it is up to date</summary>
        private void CheckMedia()
        {
            string strFileSpec = "";

            //	Do we have a PowerPoint slide loaded?
            if ((m_dxSecondary != null) && (m_dxSecondary.MediaType == TmaxMediaTypes.Slide))
            {
                if (m_tmaxDatabase.CheckSlide(m_dxSecondary, false) == false)
                {
                    if (m_tmaxDatabase.ExportSlide(m_dxSecondary, true) == true)
                    {
                        //	Reload the viewer
                        strFileSpec = m_tmaxDatabase.GetFileSpec(m_dxSecondary);
                        m_ctrlViewer.View(strFileSpec, false);
                    }

                }
            }

        }// private void CheckMedia()

        /// <summary>Traps the ToolClick event fired by the toolbar manager</summary>
        /// <param name="sender">The object firing the event</param>
        /// <param name="e">Infragistics event argument object</param>
        private void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            MediaViewerCommands eCommand = MediaViewerCommands.Invalid;

            //	Don't bother if ignoring events
            if (m_bIgnoreUltraEvents == true) return;

            //	Get the command
            if (e.Tool != null && e.Tool.Key != null)
                eCommand = GetCommand(e.Tool.Key);

            if (eCommand != MediaViewerCommands.Invalid)
                OnCommand(eCommand);

        }// OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)

        /// <summary>This method is called when the user brings up the context menu</summary>
        private void OnContextMenu()
        {
            MediaViewerCommands eCommand;
            StateButtonTool ctrlLockRatio = null;

            Debug.Assert(m_ultraToolbarManager != null);
            Debug.Assert(m_ultraToolbarManager.Tools != null);
            if ((m_ultraToolbarManager == null) || (m_ultraToolbarManager.Tools == null)) return;

            //	Check each tool in the manager's collection
            foreach (ToolBase ultraTool in m_ultraToolbarManager.Tools)
            {
                if (ultraTool.Key != null)
                {
                    try
                    {
                        if ((eCommand = GetCommand(ultraTool.Key)) != MediaViewerCommands.Invalid)
                        {
                            //	Should the command be enabled?
                            ultraTool.SharedProps.Enabled = GetCommandEnabled(eCommand);
                            ultraTool.SharedProps.Shortcut = GetCommandShortcut(eCommand);

                            if (eCommand == MediaViewerCommands.LockRatio)
                            {
                                if (m_ctrlViewer.UseScreenRatio == true)
                                    ultraTool.SharedProps.AppearancesSmall.Appearance.Image = 7;
                                else
                                    ultraTool.SharedProps.AppearancesSmall.Appearance.Image = 6;

                            }
                        }

                    }
                    catch
                    {
                    }

                }// if(ultraTool.Tag != null)

            }// foreach(ToolBase ultraTool in m_ultraToolbarManager.Tools)

            if ((ctrlLockRatio = (StateButtonTool)GetUltraTool("LockRatio")) != null)
            {
                if (m_ctrlViewer != null)
                    ctrlLockRatio.Checked = m_ctrlViewer.UseScreenRatio;
                else
                    ctrlLockRatio.Checked = false;
            }

        }// private void OnContextMenu()

        /// <summary>This method handles the pane's Properties command</summary>
        private void OnCmdProperties()
        {
            CTmaxParameters tmaxParameters = null;
            CDxMediaRecord dxRecord = null;

            //	Get the active record
            if ((dxRecord = GetActiveRecord()) == null) return;

            //	Create the required parameters for the event
            tmaxParameters = new CTmaxParameters();
            tmaxParameters.Add(TmaxCommandParameters.Properties, true);

            if (FireCommand(TmaxCommands.Open, new CTmaxItem(dxRecord), tmaxParameters) == true)
            {

            }

        }// private void OnCmdProperties()

        /// <summary>This method handles the pane's Builder command</summary>
        private void OnCmdBuilder()
        {
            CTmaxParameters tmaxParameters = null;

            //	Must have a scene to load in the builder
            if (m_dxScene == null) return;

            //	Create the required parameters for the event
            tmaxParameters = new CTmaxParameters();
            tmaxParameters.Add(TmaxCommandParameters.Builder, true);

            m_tmaxEventSource.FireDiagnostic(this, "OnCmdBuilder", "Firing command to open in builder");

            if (FireCommand(TmaxCommands.Open, new CTmaxItem(m_dxScene), tmaxParameters) == true)
            {

            }

        }// private void OnCmdBuilder()

        /// <summary>This method handles the pane's Tuner command</summary>
        private void OnCmdTuner()
        {
            CTmaxParameters tmaxParameters = null;
            CDxMediaRecord dxRecord = null;

            //	Must have a scene or link to load the tuner
            if (m_dxLink != null) dxRecord = m_dxLink;
            else if (m_dxScene != null) dxRecord = m_dxScene;

            if (dxRecord == null) return;

            //	Create the required parameters for the event
            tmaxParameters = new CTmaxParameters();
            tmaxParameters.Add(TmaxCommandParameters.Tuner, true);

            if (FireCommand(TmaxCommands.Open, new CTmaxItem(dxRecord), tmaxParameters) == true)
            {

            }

        }// private void OnCmdTuner()

        /// <summary>This method handles the pane's Presentation command</summary>
        private void OnCmdPresentation()
        {
            CTmaxParameters tmaxParameters = null;
            CDxMediaRecord dxRecord = null;

            //	Presentation can't view a link directly
            if (m_dxLink != null)
                dxRecord = m_dxScene; // use the parent scene
            else
                dxRecord = GetActiveRecord();

            if (dxRecord == null) return;

            //	Create the required parameters for the event
            tmaxParameters = new CTmaxParameters();
            tmaxParameters.Add(TmaxCommandParameters.Presentation, true);

            if (FireCommand(TmaxCommands.Open, new CTmaxItem(dxRecord), tmaxParameters) == true)
            {

            }

        }// private void OnCmdPresentation()

        /// <summary>This method handles the pane's Print command</summary>
        private void OnCmdPrint()
        {
            CTmaxItems tmaxItems = null;

            if ((tmaxItems = GetCmdPrintItems()) != null)
                FireCommand(TmaxCommands.Print, tmaxItems);
            else
                FireCommand(TmaxCommands.Print);

        }// private void OnCmdPrint()

        /// <summary>This method is called by the application to get a list of items that represent the current selections for a Print command</summary>
        /// <returns>The items that represent the current selections</returns>
        public override CTmaxItems GetCmdPrintItems()
        {
            CDxMediaRecord dxRecord = GetActiveRecord(); ;
            CTmaxItems tmaxItems = null;

            if (dxRecord != null)
            {
                tmaxItems = new CTmaxItems();
                tmaxItems.Add(new CTmaxItem(dxRecord));
            }

            return tmaxItems;

        }// public override CTmaxItems GetCmdPrintItems()

        /// <summary>This method handles the pane's LockRatio command</summary>
        private void OnCmdLockRatio()
        {
            ToolBase Tool = null;

            if ((m_ctrlViewer != null) && (m_ctrlViewer.IsDisposed == false))
            {
                if ((Tool = GetUltraTool("LockRatio")) != null)
                {
                    m_ctrlViewer.UseScreenRatio = ((StateButtonTool)Tool).Checked;
                }

            }

        }// private void OnCmdLockRatio()

        /// <summary>This method is called to get the record being viewed</summary>
        /// <returns>The record exchange object for the active media</returns>
        private CDxMediaRecord GetActiveRecord()
        {
            if (m_dxLink != null)
                return m_dxLink;
            else if (m_dxScene != null)
                return m_dxScene;
            else if (m_dxTertiary != null)
                return m_dxTertiary;
            else if (m_dxSecondary != null)
                return m_dxSecondary;
            else
                return m_dxPrimary;

        }// private CDxMediaRecord GetActiveRecord()

        /// <summary>This method will process the specified command</summary>
        /// <param name="eCommand">The command to be processed</param>
        private void OnCommand(MediaViewerCommands eCommand)
        {
            try
            {
                //	Direct to the appropriate handler
                switch (eCommand)
                {
                    case MediaViewerCommands.Presentation:

                        OnCmdPresentation();
                        break;

                    case MediaViewerCommands.Builder:

                        OnCmdBuilder();
                        break;

                    case MediaViewerCommands.Tuner:

                        OnCmdTuner();
                        break;

                    case MediaViewerCommands.Properties:

                        OnCmdProperties();
                        break;

                    case MediaViewerCommands.Print:

                        OnCmdPrint();
                        break;

                    case MediaViewerCommands.LockRatio:

                        OnCmdLockRatio();
                        break;

                    default:

                        Debug.Assert(false);
                        break;

                }// switch(eCommand)

            }
            catch (System.Exception Ex)
            {
                m_tmaxEventSource.FireDiagnostic(this, "OnCommand", "Ex: " + Ex.ToString());
            }

        }// private void OnCommand(MediaViewerCommands eCommand)

        #endregion Private Methods

        #region Properties

        //	This property exposes the name of the file loaded in the viewer
        public string Filename
        {
            get
            {
                if (m_ctrlViewer != null)
                    return m_ctrlViewer.Filename;
                else
                    return "";
            }

        }// Filename

        //	This property exposes the fully qualified path of the file loaded in the viewer
        public string Path
        {
            get
            {
                if (m_ctrlViewer != null)
                    return m_ctrlViewer.Path;
                else
                    return "";
            }

        }// Path

        #endregion Properties

    }//	public class CMediaViewer : FTI.Trialmax.Panes.CBasePane

}//namespace FTI.Trialmax.Panes
