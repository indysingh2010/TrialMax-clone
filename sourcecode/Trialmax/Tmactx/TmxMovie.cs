using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using Infragistics.Win.UltraWinToolbars;

using FTI.Shared.Trialmax;
using FTI.Shared;

namespace FTI.Trialmax.ActiveX
{
	/// <summary>
	/// This class serves as a wrapper for the Trialmax Tmmovie ActiveX control
	/// </summary>
	public class CTmxMovie : FTI.Trialmax.ActiveX.CTmxBase
	{
		private double TMXMOVIE_VIDEO_POSITION_TOLERANCE = 0.03333;
		
		#region Private Members

		/// <summary>Local member required by form designer</summary>
		private System.ComponentModel.IContainer components;
		
		/// <summary>Infragistics library toolbar/menu manager left-side docking zone</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmxMovie_Toolbars_Dock_Area_Left;
		
		/// <summary>Infragistics library toolbar/menu manager right-side docking zone</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmxMovie_Toolbars_Dock_Area_Right;
		
		/// <summary>Infragistics library toolbar/menu manager top docking zone</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmxMovie_Toolbars_Dock_Area_Top;
		
		/// <summary>Infragistics library toolbar/menu manager bottom docking zone</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _CTmxMovie_Toolbars_Dock_Area_Bottom;
		
		/// <summary>Infragistics library toolbar/menu manager</summary>
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager m_ctrlUltraToolbarManager;
		
		/// <summary>TrialMax ActiveX video player control</summary>
		private AxTM_MOVIE6Lib.AxTMMovie6 m_ctrlTmmovie;
		
		/// <summary>Background panel to act as container for child controls</summary>
		private System.Windows.Forms.Panel m_ctrlFillPanel;
		
		/// <summary>Local member bound to LockRange property</summary>
		private bool m_bLockRange = false;
		
		/// <summary>Local member bound to CueStep property</summary>
		private double m_dCueStep = 10;
		
		/// <summary>Local member bound to EnableSimulation property</summary>
		private bool m_bEnableSimulation = false;
		
		/// <summary>Local member bound to SimulationText property</summary>
		private string m_strSimulationText = "";
		
		#region Private Methods
		
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected override void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("MainToolbar");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("Spacer");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PlayStart");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PlayBack");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Play", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Pause", "");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PlayFwd");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PlayEnd");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("Spacer");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool3 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Play", "");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool4 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Pause", "");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PlayFwd");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PlayEnd");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PlayStart");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("PlayBack");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonToolShowPresentation1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BlankPresentation");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonToolShowPresentation2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BlankPresentation");
			Infragistics.Win.UltraWinToolbars.LabelTool labelTool3 = new Infragistics.Win.UltraWinToolbars.LabelTool("Spacer");
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CTmxMovie));
			this.m_ctrlUltraToolbarManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
			this.m_ctrlFillPanel = new System.Windows.Forms.Panel();
			this.m_ctrlTmmovie = new AxTM_MOVIE6Lib.AxTMMovie6();
			this._CTmxMovie_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTmxMovie_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTmxMovie_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._CTmxMovie_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraToolbarManager)).BeginInit();
			this.m_ctrlFillPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlTmmovie)).BeginInit();
			this.SuspendLayout();
			// 
			// m_ctrlUltraToolbarManager
			// 
			this.m_ctrlUltraToolbarManager.DockWithinContainer = this;
			this.m_ctrlUltraToolbarManager.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_ctrlUltraToolbarManager.LockToolbars = true;
			this.m_ctrlUltraToolbarManager.MdiMergeable = false;
			this.m_ctrlUltraToolbarManager.ShowFullMenusDelay = 500;
			this.m_ctrlUltraToolbarManager.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.Office2003;
			ultraToolbar1.DockedColumn = 0;
			ultraToolbar1.DockedRow = 0;
			ultraToolbar1.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowDockTop = Infragistics.Win.DefaultableBoolean.True;
			ultraToolbar1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
			ultraToolbar1.Settings.FillEntireRow = Infragistics.Win.DefaultableBoolean.True;
			ultraToolbar1.Settings.GrabHandleStyle = Infragistics.Win.UltraWinToolbars.GrabHandleStyle.None;
			ultraToolbar1.ShowInToolbarList = false;
			ultraToolbar1.Text = "MainToolbar";
			stateButtonTool1.InstanceProps.IsFirstInGroup = true;
			buttonTool3.InstanceProps.IsFirstInGroup = true;
            ultraToolbar1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																							  labelTool1,
																							  buttonTool1,
																							  buttonTool2,
																							  stateButtonTool1,
																							  stateButtonTool2,
																							  buttonTool3,
																							  buttonTool4,
																							  labelTool2, 
                                                                                              buttonToolShowPresentation1});
			this.m_ctrlUltraToolbarManager.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
																													  ultraToolbar1});
			this.m_ctrlUltraToolbarManager.ToolbarSettings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraToolbarManager.ToolbarSettings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraToolbarManager.ToolbarSettings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraToolbarManager.ToolbarSettings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraToolbarManager.ToolbarSettings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraToolbarManager.ToolbarSettings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraToolbarManager.ToolbarSettings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
			this.m_ctrlUltraToolbarManager.ToolbarSettings.FillEntireRow = Infragistics.Win.DefaultableBoolean.True;
			stateButtonTool3.SharedProps.Caption = "Play";
			stateButtonTool4.SharedProps.Caption = "Pause";
			buttonTool5.SharedProps.Caption = "Forward 10 sec";
			buttonTool6.SharedProps.Caption = "Go To End";
			buttonTool7.SharedProps.Caption = "Go To Start";
			buttonTool8.SharedProps.Caption = "Rewind 10 sec";
			labelTool3.SharedProps.Spring = true;
            buttonToolShowPresentation2.SharedProps.Caption = "TrailMax Presentation";
			this.m_ctrlUltraToolbarManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																											   stateButtonTool3,
																											   stateButtonTool4,
																											   buttonTool5,
																											   buttonTool6,
																											   buttonTool7,
																											   buttonTool8,
																											   labelTool3});
			this.m_ctrlUltraToolbarManager.BeforeToolbarListDropdown += new Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventHandler(this.OnUltraBeforeToolbarListDropdown);
			this.m_ctrlUltraToolbarManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.OnUltraToolClick);
			// 
			// m_ctrlFillPanel
			// 
			this.m_ctrlFillPanel.Controls.Add(this.m_ctrlTmmovie);
			this.m_ctrlFillPanel.Cursor = System.Windows.Forms.Cursors.Default;
			this.m_ctrlFillPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlFillPanel.Location = new System.Drawing.Point(0, 27);
			this.m_ctrlFillPanel.Name = "m_ctrlFillPanel";
			this.m_ctrlFillPanel.Size = new System.Drawing.Size(204, 123);
			this.m_ctrlFillPanel.TabIndex = 0;
			// 
			// m_ctrlTmmovie
			// 
			this.m_ctrlTmmovie.ContainingControl = this;
			this.m_ctrlTmmovie.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ctrlTmmovie.Enabled = true;
			this.m_ctrlTmmovie.Location = new System.Drawing.Point(0, 0);
			this.m_ctrlTmmovie.Name = "m_ctrlTmmovie";
			this.m_ctrlTmmovie.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("m_ctrlTmmovie.OcxState")));
			this.m_ctrlTmmovie.Size = new System.Drawing.Size(204, 123);
			this.m_ctrlTmmovie.TabIndex = 0;
			this.m_ctrlTmmovie.MouseMoveEvent += new AxTM_MOVIE6Lib._DTMMovie6Events_MouseMoveEventHandler(this.OnAxMouseMove);
			this.m_ctrlTmmovie.StateChange += new AxTM_MOVIE6Lib._DTMMovie6Events_StateChangeEventHandler(this.OnAxStateChange);
			this.m_ctrlTmmovie.PositionChange += new AxTM_MOVIE6Lib._DTMMovie6Events_PositionChangeEventHandler(this.OnAxPositionChange);
			this.m_ctrlTmmovie.MouseUpEvent += new AxTM_MOVIE6Lib._DTMMovie6Events_MouseUpEventHandler(this.OnAxMouseUp);
			this.m_ctrlTmmovie.MouseDownEvent += new AxTM_MOVIE6Lib._DTMMovie6Events_MouseDownEventHandler(this.OnAxMouseDown);
			this.m_ctrlTmmovie.AxError += new AxTM_MOVIE6Lib._DTMMovie6Events_AxErrorEventHandler(this.OnAxError);
			this.m_ctrlTmmovie.MouseDblClick += new AxTM_MOVIE6Lib._DTMMovie6Events_MouseDblClickEventHandler(this.OnAxMouseDblClick);
			this.m_ctrlTmmovie.AxDiagnostic += new AxTM_MOVIE6Lib._DTMMovie6Events_AxDiagnosticEventHandler(this.OnAxDiagnostic);
			// 
			// _CTmxMovie_Toolbars_Dock_Area_Left
			// 
			this._CTmxMovie_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmxMovie_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
			this._CTmxMovie_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
			this._CTmxMovie_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmxMovie_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
			this._CTmxMovie_Toolbars_Dock_Area_Left.Name = "_CTmxMovie_Toolbars_Dock_Area_Left";
			this._CTmxMovie_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 123);
			this._CTmxMovie_Toolbars_Dock_Area_Left.ToolbarsManager = this.m_ctrlUltraToolbarManager;
			// 
			// _CTmxMovie_Toolbars_Dock_Area_Right
			// 
			this._CTmxMovie_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmxMovie_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
			this._CTmxMovie_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
			this._CTmxMovie_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmxMovie_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(204, 27);
			this._CTmxMovie_Toolbars_Dock_Area_Right.Name = "_CTmxMovie_Toolbars_Dock_Area_Right";
			this._CTmxMovie_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 123);
			this._CTmxMovie_Toolbars_Dock_Area_Right.ToolbarsManager = this.m_ctrlUltraToolbarManager;
			// 
			// _CTmxMovie_Toolbars_Dock_Area_Top
			// 
			this._CTmxMovie_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmxMovie_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
			this._CTmxMovie_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
			this._CTmxMovie_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmxMovie_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
			this._CTmxMovie_Toolbars_Dock_Area_Top.Name = "_CTmxMovie_Toolbars_Dock_Area_Top";
			this._CTmxMovie_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(204, 27);
			this._CTmxMovie_Toolbars_Dock_Area_Top.ToolbarsManager = this.m_ctrlUltraToolbarManager;
			// 
			// _CTmxMovie_Toolbars_Dock_Area_Bottom
			// 
			this._CTmxMovie_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._CTmxMovie_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
			this._CTmxMovie_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
			this._CTmxMovie_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
			this._CTmxMovie_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 150);
			this._CTmxMovie_Toolbars_Dock_Area_Bottom.Name = "_CTmxMovie_Toolbars_Dock_Area_Bottom";
			this._CTmxMovie_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(204, 0);
			this._CTmxMovie_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.m_ctrlUltraToolbarManager;
			// 
			// CTmxMovie
			// 
			this.Controls.Add(this.m_ctrlFillPanel);
			this.Controls.Add(this._CTmxMovie_Toolbars_Dock_Area_Left);
			this.Controls.Add(this._CTmxMovie_Toolbars_Dock_Area_Right);
			this.Controls.Add(this._CTmxMovie_Toolbars_Dock_Area_Top);
			this.Controls.Add(this._CTmxMovie_Toolbars_Dock_Area_Bottom);
			this.Name = "CTmxMovie";
			this.Size = new System.Drawing.Size(204, 150);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlUltraToolbarManager)).EndInit();
			this.m_ctrlFillPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_ctrlTmmovie)).EndInit();
			this.ResumeLayout(false);

		}
		
		#endregion Private Methods

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmxMovie() : base()
		{
			m_strDescription = "TrialMax Video Player (tmmovie)";
			m_tmaxEventSource.Name = m_strDescription;
			
			//	Initialize the child components
			InitializeComponent();
			
			//	Initilize the toolbar
			InitializeUltraToolbar();
		}
		
		/// <summary>
		/// This method is called to perform the ActiveX initialization of the control
		/// </summary>
		/// <returns>true if successful</returns>
		public override bool AxInitialize()
		{
			//	Initialize the Tmmovie control
			if(m_ctrlTmmovie != null)
			{
				m_sAxError = m_ctrlTmmovie.Initialize();
				m_bInitialized = m_ctrlTmmovie.IsReady();
			}
			else
			{
				m_sAxError = -1;
				m_bInitialized = false;
			}
			
			if(m_bInitialized == true)
				FireStateChange(TmxStates.Unloaded);
				
			return m_bInitialized;
		}
		
		/// <summary>
		/// This method is called to activate the control
		/// </summary>
		public override void Activate()
		{
			SetUltraToolStates();
		}
		
		/// <summary>
		/// This method is called to determine if the specified file is viewable
		/// by the derived control
		/// </summary>
		/// <param name="strFilename">Name of file to be viewed if possible</param>
		/// <returns>true if viewable</returns>
		public override bool IsViewable(string strFilename)
		{
			string strExtension = System.IO.Path.GetExtension(strFilename);
			return CTmxMovie.CheckExtension(strExtension);
		}
		
		/// <summary>This function is called to determine if files with the specified extension can be rendered by this control</summary>
		/// <param name="strExtension">File extension to be checked</param>
		/// <returns>true if viewable</returns>
		public static bool CheckExtension(string strExtension)
		{
			if((strExtension != null) && (strExtension.Length > 0))
			{
				//	Strip the leading period if found
				if(strExtension.StartsWith("."))
					strExtension = strExtension.Remove(0,1);
					
				switch(strExtension.ToLower())
				{
					case "avi":
					case "mpg":
					case "mpeg":
					case "wmv":
                    case "mp4":                    
					case "mp3":
					case "wma":
					case "wav":
					
						return true;
					
					default:
					
						break;
				}
				
			}
			
			return false;

		}
		
		/// <summary>This function is called to retrieve the total time for the specified file</summary>
		/// <param name="strFilename">The file to be queried</param>
		/// <returns>The total duration in seconds</returns>
		public double GetDuration(string strFilename)
		{
			double dDuration = -1.0;
			
			try
			{
				if(m_ctrlTmmovie != null)
				{
					dDuration = m_ctrlTmmovie.GetDuration(strFilename);
				}
			}
			catch
			{
			}
			
			return dDuration;
			
		}// public double GetDuration(string strFilename)
		
		/// <summary>This function is called to stop the playback</summary>
		/// <returns>true if successful</returns>
		public override bool Stop()
		{
			try
			{
				if(m_ctrlTmmovie != null)
				{
					m_ctrlTmmovie.Stop();
				}
				
				GetPosition();
				return true;
			}
			catch
			{
				return false;
			}
		
		}
		
		/// <summary>This function is called to pause the playback</summary>
		/// <returns>true if successful</returns>
		public override bool Pause()
		{
			bool bSuccessful = false;
			
			try
			{
				//	Make sure the control is ready to play
				if((m_ctrlTmmovie == null) || (m_ctrlTmmovie.IsLoaded() == false))
					return false;
				else
				{
					bSuccessful = (m_ctrlTmmovie.Pause() == 0);
					
					GetPosition();
					
					return bSuccessful;
				}				
			}
			catch
			{
				return false;
			}
		}
		
		/// <summary>This function is called to play the the active file using the specified range</summary>
		/// <param name="dStart">The start time</param>
		/// <param name="dStop">The stop time</param>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		/// <returns>true if successful</returns>
		public override bool Play(double dStart, double dStop)
		{
			bool bSuccessful = false;
			
			//	Do we have a valid control?
			if((m_ctrlTmmovie == null) || (m_bInitialized == false)) return false;
			
			try
			{
				//	Should we reset the start and stop positions?
				if((dStart >= 0) && (dStop >= 0))
				{
					m_ctrlTmmovie.StartPosition = dStart;
					m_ctrlTmmovie.StopPosition = dStop;
					
					//	Tmmovie may have adjusted the values
					m_dTmxStart = m_ctrlTmmovie.StartPosition;
					m_dTmxStop = m_ctrlTmmovie.StopPosition;
					
					//	Set the cue extents
					SetCueRange();
				
				}
				
				bSuccessful = (m_ctrlTmmovie.Play() == 0);
				
				GetPosition();
				return bSuccessful;
			}
			catch
			{
				return false;
			}
			
		}// public override bool Play(double dStart, double dStop)
		
		/// <summary>This function is called to load the specified file</summary>
		/// <param name="strFilename">The name of the file to be viewed</param>
		/// <param name="lStart">The frame at which to start viewing</param>
		/// <param name="lStop">The frame at which to stop viewing</param>
		/// <returns>true if successful</returns>
		public override bool Play(string strFilename, double dStart, double dStop, bool bRun)
		{
			//	Are we actually unloading?
			if((strFilename == null) || (strFilename.Length == 0))
			{
				Unload();
			
				SetUltraToolStates();
				return true;
			}
			
			//	Do we have a valid control?
			if((m_ctrlTmmovie == null) || (m_bInitialized == false)) return false;
			
			m_sAxError = m_ctrlTmmovie.Load(strFilename, dStart, dStop, false);
			
			if(m_sAxError == 0)
			{
				m_strFilename		= m_ctrlTmmovie.Filename;
				m_dTmxPosition		= m_ctrlTmmovie.GetPosition();
				m_dTmxStart			= m_ctrlTmmovie.StartPosition;
				m_dTmxStop			= m_ctrlTmmovie.StopPosition;
				m_dTmxDuration		= m_ctrlTmmovie.GetMaxTime();
				m_dTmxMaxPosition	= m_ctrlTmmovie.GetMaxTime();
				m_dTmxMinPosition	= m_ctrlTmmovie.GetMinTime();
			
				SetCueRange();
				
				FireStateChange(TmxStates.Loaded);
				
				if(bRun == true)
				{
					return (m_ctrlTmmovie.Play() == 0);
				}
				else
				{
					//	Cue to the start
					return Cue(TmxCueModes.Start);
				}
			
			}
			else
			{
				SetUltraToolStates();
				return false;
			}
				
		}//	Play(string strFilename, double dStart, double dStop)
		
		/// <summary>This function is called to step the the active file from the start point to the stop point</summary>
		/// <param name="dStart">The start time</param>
		/// <param name="dStop">The stop time</param>
		/// <returns>true if successful</returns>
		public override bool Step(double dStart, double dStop)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Make sure the control is ready to play
				if((m_ctrlTmmovie == null) || (m_ctrlTmmovie.IsLoaded() == false))
				{
					return false;
				}
				else
				{
					bSuccessful = (m_ctrlTmmovie.Step(dStart, dStop) == 0);
					
					GetPosition();
					return bSuccessful;
				}		
			}
			catch
			{
				return false;
			}
		}
		
		/// <summary>This function is called to step the the active file from its current position the specified amount of time</summary>
		/// <param name="dTime">The step time</param>
		/// <returns>true if successful</returns>
		public override bool Step(double dTime)
		{
			//	Reverse stepping is not supported
			if(dTime < 0)
			{
				string Msg = String.Format("Stepping in reverse is not supported: time = {0}", dTime);
				m_tmaxEventSource.FireError(this, "Step", Msg);
				return false;
			}
			
			return Step(-1.0, dTime);
		}
		
		/// <summary>This function is called to resume playback from the current positiion</summary>
		/// <returns>true if successful</returns>
		public override bool Resume()
		{
			try
			{
				//	Make sure the control is ready to play
				if((m_ctrlTmmovie == null) || (m_ctrlTmmovie.IsLoaded() == false))
					return false;
				else
					return (m_ctrlTmmovie.Resume() == 0);				
			}
			catch
			{
				return false;
			}
		}
		
		/// <summary>This function is called to cue the active file</summary>
		/// <param name="eMode">The enumerated cue mode</param>
		/// <param name="dPosition">The new position</param>
		/// <param name="bResume">true to resume playback</param>
		/// <returns>true if successful</returns>
		public override bool Cue(TmxCueModes eMode, double dPosition, bool bResume)
		{
			bool bSuccessful = false;
			
			//	Make sure the control is ready to play
			if((m_ctrlTmmovie == null) || (m_ctrlTmmovie.IsLoaded() == false))
				return false;
				
			//	Which mode?
			switch(eMode)
			{
				case TmxCueModes.Absolute:
				
					bSuccessful = (m_ctrlTmmovie.Cue((short)TmxMovieCueRequests.Absolute, dPosition, bResume) == 0);
					break;
					
				case TmxCueModes.Start:

					bSuccessful = (m_ctrlTmmovie.Cue((short)TmxMovieCueRequests.Start, 0, bResume) == 0);
					break;
					
				case TmxCueModes.Stop:
				
					bSuccessful = (m_ctrlTmmovie.Cue((short)TmxMovieCueRequests.Stop, 0, bResume) == 0);
					break;
					
				case TmxCueModes.First:
				
					bSuccessful = (m_ctrlTmmovie.Cue((short)TmxMovieCueRequests.First, 0, bResume) == 0);
					break;
					
				case TmxCueModes.Last:
				
					bSuccessful = (m_ctrlTmmovie.Cue((short)TmxMovieCueRequests.Last, 0, bResume) == 0);
					break;
					
				case TmxCueModes.Relative:
					
					bSuccessful = (m_ctrlTmmovie.Cue((short)TmxMovieCueRequests.Relative, dPosition, bResume) == 0);
					break;
					
				default:
				
					break;
					
			}// switch(eMode)
			
			GetPosition();
			
			return bSuccessful;
			
		}
		
		/// <summary>This function is called to process the specified request</summary>
		/// <param name="tmxRequest">The request parameters</param>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		/// <returns>true if successful</returns>
		public override bool ProcessRequest(CTmxRequest tmxRequest)
		{
			if(m_bInitialized == false) return false;
			
			//	What action is being requested?
			switch(tmxRequest.Action)
			{
				case TmxActions.Pause:
				
					Pause();
					break;
					
				case TmxActions.Resume:
				
					Resume();
					break;
					
				case TmxActions.Play:
				
					Play(tmxRequest.Start, tmxRequest.Stop);
					break;
					
				case TmxActions.Stop:
				
					Stop();
					break;
					
			}
			
			return true;
		}
		
		/// <summary>This method is called to process a media bar command</summary>
		/// <param name="eCommand">The enumerated command identifier</param>
		/// <returns>true if processed</returns>
		public override bool ProcessCommand(TmaxMediaBarCommands eCommand)
		{
			bool bSuccessful = false;
			
			//	Make sure the control is ready to play
			if((m_ctrlTmmovie == null) || (m_ctrlTmmovie.IsLoaded() == false))
				return false;
				
			//	Which command?
			switch(eCommand)
			{
				case TmaxMediaBarCommands.Play:
				
					bSuccessful = (m_ctrlTmmovie.Play() == 0);
					break;
					
				case TmaxMediaBarCommands.Pause:
				
					bSuccessful = (m_ctrlTmmovie.Pause() == 0);
					break;
					
				case TmaxMediaBarCommands.PlayStart:
				
					bSuccessful = Cue(TmxCueModes.Start);
					break;
					
				case TmaxMediaBarCommands.PlayEnd:
				
					bSuccessful = Cue(TmxCueModes.Stop);
					break;
					
				case TmaxMediaBarCommands.PlayBack:
					
					bSuccessful = Cue(TmxCueModes.Relative, -m_dCueStep);
					break;
					
				case TmaxMediaBarCommands.PlayFwd:
					
					bSuccessful = Cue(TmxCueModes.Relative, m_dCueStep);
					break;
					
				default:
				
					break;
					
			}// switch(eCommand)
			
			//	Make sure we have the correct position
			GetPosition();
			
			return bSuccessful;
			
		}// public override bool ProcessCommand(TmaxMediaBarCommands eCommand)
		
		/// <summary>This method is called to unload the viewer</summary>
		public override void Unload()
		{
			//	Do we have a valid control?
			if((m_ctrlTmmovie != null) && (m_bInitialized == true))
			{
				m_ctrlTmmovie.Stop();
				m_ctrlTmmovie.Unload();
				
				//	Reset the playback extents
				m_dTmxPosition = -1.0;
				m_dTmxStart = -1.0;
				m_dTmxStop = -1.0;
				m_dTmxDuration = 0.0;
				m_dTmxMinPosition = -1.0;
				m_dTmxMaxPosition = -1.0;
			
				FireStateChange(TmxStates.Unloaded);
			}

			m_strFilename = "";
		
		}//	Unload()
		
		/// <summary>This function is notify the control that the parent window has been moved</summary>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		public override void OnParentMoved()
		{
			//	Do we have a valid control?
			if((m_ctrlTmmovie != null) && (m_bInitialized == true))
			{
				//	Make sure the video is in the private position
				m_ctrlTmmovie.UpdateScreenPosition();
			}

		}// public override void OnParentMoved()
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Clean up any resources being used</summary>
		/// <param name="disposing">true if disposing of the object</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(components != null)
					components.Dispose();
			}
			base.Dispose(disposing);
		}

		/// <summary>This method is called to get the Infragistics toolbar manager</summary>
		/// <returns>The viewer's toolbar manager</returns>
		/// <remarks>This method should be overridden by derived class that want to use the toolbar helper methods</remarks>
		protected override Infragistics.Win.UltraWinToolbars.UltraToolbarsManager GetUltraManager()
		{
			return m_ctrlUltraToolbarManager;
		}

		/// <summary>This method is called to get the object's component container</summary>
		/// <returns>The viewer's component container</returns>
		protected override System.ComponentModel.IContainer GetComponentContainer()
		{
			return components;
		}

		/// <summary>This method is called to set the states of the toolbar buttons</summary>
		protected override void SetUltraToolStates()
		{
			bool	bEnabled = true;
			double	dPosition = 0.0;
			bool	bOnStop = false;
			bool	bOnStart = false;
			
			//	Don't bother if not displaying the toolbar
			if(m_ctrlUltraToolbarManager == null) return;
			if(m_bShowToolbar == false) return;
			
			if((m_bEnableToolbar == false) || 
			   (m_ctrlTmmovie == null) || 
			   (m_ctrlTmmovie.IsReady() == false) || 
			   (m_ctrlTmmovie.IsLoaded() == false))
				bEnabled = false;
				
			//	Make sure we get the latest position
			if((dPosition = m_ctrlTmmovie.GetPosition()) != m_dTmxPosition)
				FirePositionChange(dPosition);
			
			//	Is the playback range locked?
			if(LockRange == true)
			{
				if((dPosition - TMXMOVIE_VIDEO_POSITION_TOLERANCE) <= m_dTmxStart)
					bOnStart = true;
				if((dPosition + TMXMOVIE_VIDEO_POSITION_TOLERANCE) >= m_dTmxStop)
					bOnStop = true;
			}
			else
			{
				if((dPosition - TMXMOVIE_VIDEO_POSITION_TOLERANCE) <= m_dTmxMinPosition)
					bOnStart = true;
				if((dPosition + TMXMOVIE_VIDEO_POSITION_TOLERANCE) >= m_dTmxMaxPosition)
					bOnStop = true;
			}
			
			//	Do not process click events while we set the states
			m_bProcessToolClicks = false;
			
			SetUltraToolEnabled(TmaxMediaBarCommands.Play, (bEnabled && !bOnStop));
			SetUltraToolEnabled(TmaxMediaBarCommands.Pause, bEnabled);
			SetUltraToolEnabled(TmaxMediaBarCommands.PlayBack, (bEnabled && !bOnStart));
			SetUltraToolEnabled(TmaxMediaBarCommands.PlayFwd, (bEnabled && !bOnStop));
			SetUltraToolEnabled(TmaxMediaBarCommands.PlayEnd, (bEnabled && !bOnStop));
			SetUltraToolEnabled(TmaxMediaBarCommands.PlayStart, (bEnabled && !bOnStart));
			
			if(bEnabled == true)
			{
				SetUltraToolChecked(TmaxMediaBarCommands.Play, m_ctrlTmmovie.GetState() == (short)TmxMovieStates.Playing);
				SetUltraToolChecked(TmaxMediaBarCommands.Pause, m_ctrlTmmovie.GetState() == (short)TmxMovieStates.Paused);
			}
			else
			{
				SetUltraToolChecked(TmaxMediaBarCommands.Play, false);
				SetUltraToolChecked(TmaxMediaBarCommands.Pause, false);
			}
			
			//	OK to process click events now
			m_bProcessToolClicks = true;
			
		}// protected void SetUltraToolStates()

		/// <summary>This event is fired by the toolbar manager when it is about to display the customize menu</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The cancelable event arguments</param>
		protected void OnUltraBeforeToolbarListDropdown(object sender, Infragistics.Win.UltraWinToolbars.BeforeToolbarListDropdownEventArgs e)
		{
			// Prevent this menu from coming up
			e.Cancel = true;
		}

		/// <summary>This method traps events fired when the user clicks on a toolbar button</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">Infragistics event object</param>
		protected override void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		{
			//	Call the base class handler
			//
			//	NOTE:	We have to put this wrapper in the derived class otherwise the
			//			form designer in VS 2005 will raise an exception when we attempt
			//			to edit the form
			base.OnUltraToolClick(sender, e);

		}// protected override void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)

		#endregion Protected Methods

		#region Private Methods
		
		private void OnAxStateChange(object sender, AxTM_MOVIE6Lib._DTMMovie6Events_StateChangeEvent e)
		{
			TmxStates eState = TmxStates.Invalid;
			
			SetUltraToolStates();
			
			try
			{
				switch((TmxMovieStates)e.sState)
				{
					case TmxMovieStates.NotReady:
					case TmxMovieStates.Ready:
					
						break;
						
					case TmxMovieStates.Playing:
					
						eState = TmxStates.Playing;
						break;
						
					case TmxMovieStates.Paused:
					
						eState = TmxStates.Paused;
						break;
						
					case TmxMovieStates.Stopped:
					
						eState = TmxStates.Stopped;
						break;
				}
			
				if(eState != TmxStates.Invalid)
					FireStateChange(eState);
			}
			catch
			{
			}
			
		}

		/// <summary>This method traps MouseDblClick events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxMouseDblClick(object sender, AxTM_MOVIE6Lib._DTMMovie6Events_MouseDblClickEvent e)
		{
			if(m_tmaxEventSource != null)
				m_tmaxEventSource.FireMouseDblClick(this, TranslateMouseButton(e.sButton), 0, 0, 2);	
		}

		/// <summary>This method traps MouseDown events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxMouseDown(object sender, AxTM_MOVIE6Lib._DTMMovie6Events_MouseDownEvent e)
		{
			if(m_tmaxEventSource != null)
				m_tmaxEventSource.FireMouseDown(this, TranslateMouseButton(e.button), e.x, e.y);	
		}
		
		/// <summary>This method traps MouseDown events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxMouseUp(object sender, AxTM_MOVIE6Lib._DTMMovie6Events_MouseUpEvent e)
		{
			if(m_tmaxEventSource != null)
				m_tmaxEventSource.FireMouseUp(this, TranslateMouseButton(e.button), e.x, e.y);	
		}

		/// <summary>This method traps MouseDown events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxMouseMove(object sender, AxTM_MOVIE6Lib._DTMMovie6Events_MouseMoveEvent e)
		{
			if(m_tmaxEventSource != null)
				m_tmaxEventSource.FireMouseMove(this, TranslateMouseButton(e.button), e.x, e.y);	
		}

		/// <summary>This method traps PositionChange events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxPositionChange(object sender, AxTM_MOVIE6Lib._DTMMovie6Events_PositionChangeEvent e)
		{
			GetPosition();
			SetUltraToolStates();
		}

		/// <summary>This method traps AxError events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxError(object sender, AxTM_MOVIE6Lib._DTMMovie6Events_AxErrorEvent e)
		{
			if(m_tmaxEventSource != null)
				m_tmaxEventSource.FireError(this, "TMMovie", e.lpszMessage);	
		}

		/// <summary>This method traps AxDiagnostic events fired by the ActiveX control</summary>
		/// <param name="sender">The object sending the event</param>
		/// <param name="e">The event arguments</param>
		private void OnAxDiagnostic(object sender, AxTM_MOVIE6Lib._DTMMovie6Events_AxDiagnosticEvent e)
		{
			if(m_tmaxEventSource != null)
				m_tmaxEventSource.FireDiagnostic(this, e.lpszMethod, e.lpszMessage);	
		}

		/// <summary>This method is called to update the current position value</summary>
		private void GetPosition()
		{
			double dPosition = 0.0;
			
			if(m_ctrlTmmovie == null) return;
			if(m_ctrlTmmovie.IsReady() == false) return;
			if(m_ctrlTmmovie.IsLoaded() == false) return;
				
			//	Make sure we get the latest position
			if((dPosition = m_ctrlTmmovie.GetPosition()) != m_dTmxPosition)
				FirePositionChange(dPosition);

		}// private void GetPosition()

		/// <summary>This method is called to set the cue range for the active video</summary>
		private void SetCueRange()
		{
			//	Do we have an active video
			if(m_ctrlTmmovie == null) return;
			if(m_bInitialized == false) return;
			if(m_ctrlTmmovie.IsLoaded() == false) return;
			
			try
			{
				//	Set the cue extents
				if(LockRange == true)
				{
					m_ctrlTmmovie.SetMinCuePosition(m_dTmxStart);
					m_ctrlTmmovie.SetMaxCuePosition(m_dTmxStop);
				}
				else
				{
					m_ctrlTmmovie.SetMinCuePosition(m_dTmxMinPosition);
					m_ctrlTmmovie.SetMaxCuePosition(m_dTmxMaxPosition);
				}
				
			}
			catch
			{
			}
		}
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Locks the playback and cue range to start/stop position</summary>
		public bool LockRange
		{
			get 
			{ 
				return m_bLockRange; 
			}
			set
			{
				m_bLockRange = value;
				
				SetCueRange();
			}
			
		}// LockRange properties
		
		/// <summary>Sets the time used for cueing forward and back</summary>
		public double CueStep
		{
			get 
			{ 
				return m_dCueStep; 
			}
			set
			{
				m_dCueStep = value;
				
				SetUltraToolStates();
			}
			
		}// CueStep properties
		
		/// <summary>Enables the control to simulate playback if unable to load the specified video</summary>
		public bool EnableSimulation
		{
			get 
			{ 
				return m_bEnableSimulation; 
			}
			set
			{
				m_bEnableSimulation = value;
				
				if((m_ctrlTmmovie != null) && (m_ctrlTmmovie.IsDisposed == false))
					m_ctrlTmmovie.EnableSimulation = m_bEnableSimulation;
			}
			
		}// EnableSimulation property

		/// <summary>Text displayed when simulating video playback</summary>
		public string SimulationText
		{
			get 
			{ 
				return m_strSimulationText; 
			}
			set
			{
				m_strSimulationText = value;
				
				if((m_ctrlTmmovie != null) && (m_ctrlTmmovie.IsDisposed == false))
					m_ctrlTmmovie.SimulationText = m_strSimulationText;
			}
			
		}// SimulationText property

		#endregion Properties

	}//	class CTmxMovie

}// namespace FTI.Trialmax.ActiveX
