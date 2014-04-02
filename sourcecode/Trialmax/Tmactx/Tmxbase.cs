using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

using Infragistics.Win.UltraWinToolbars;

/// <summary>Namespace containing all .NET / Trialmax ActiveX controls</summary>
namespace FTI.Trialmax.ActiveX
{
	/// <summary>Base class from which all Trialmax ActiveX wrapper controls are derived</summary>
	public class CTmxBase : System.Windows.Forms.UserControl
	{
		/// <summary>ActiveX viewer events (see TmxEvents enumeration)</summary>
		public event TmxEventHandler TmxEvent;
        public event TmxEventHandler OnRequestPresentation;
		
		#region Protected Members
		
		/// <summary>Local member used to fire diagnostic and error events</summary>
		protected FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Trialmax media toolbar image list</summary>
		protected FTI.Shared.Trialmax.CTmaxMediaBar m_tmaxMediaBar = null;
		
		/// <summary>Local member accessed by the Description property</summary>
		protected string m_strDescription = "TmxBase Viewer";
		
		/// <summary>Local member accessed by the Filename property</summary>
		protected string m_strFilename = "";
		
		/// <summary>Local member accessed by the IniFilename property</summary>
		protected string m_strIniFilename = "";
		
		/// <summary>Local member accessed by the IniSection property</summary>
		protected string m_strIniSection = "";
		
		/// <summary>Flag to inhibit processing of tool click events</summary>
		protected bool m_bProcessToolClicks = true;
		
		/// <summary>Local member bound to ShowToolbar property</summary>
		protected bool m_bShowToolbar = false;
		
		/// <summary>Local member bound to EnableToolbar property</summary>
		protected bool m_bEnableToolbar = true;
		
		/// <summary>Local member bound to AxAutoSave</summary>
		private bool m_bAxAutoSave = false;
		
		/// <summary>Local member accessed by the AxError property</summary>
		protected short m_sAxError = 0;
		
		/// <summary>Local member accessed by the AxError property</summary>
		protected TmxStates m_eTmxState = TmxStates.Unitialized;
		
		/// <summary>Local member accessed by the TmxPosition property</summary>
		protected double m_dTmxPosition = -1.0;
		
		/// <summary>Local member accessed by the TmxMaxPosition property</summary>
		protected double m_dTmxMaxPosition = -1.0;
		
		/// <summary>Local member accessed by the TmxMinPosition property</summary>
		protected double m_dTmxMinPosition = -1.0;
		
		/// <summary>Local member accessed by the TmxStart property</summary>
		protected double m_dTmxStart = -1.0;
		
		/// <summary>Local member accessed by the TmxStop property</summary>
		protected double m_dTmxStop = -1.0;
		
		/// <summary>Local member accessed by the TmxDuration property</summary>
		protected double m_dTmxDuration = 0.0;
		
		/// <summary>Local member accessed by the Initialized property</summary>
		protected bool m_bInitialized = false;
		
		/// <summary>Local member accessed by the NavigatorPosition property</summary>
		protected int m_iNavigatorPosition = -1;
		
		/// <summary>Local member accessed by the NavigatorTotal property</summary>
		protected int m_iNavigatorTotal = 0;
		
		#endregion Protected Members

		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmxBase()
		{
			//	NOTE:	It is up to the derived class to call InitializeComponent()
			//			in it's constructor. This ensures that the derived constructor
			//			gets called before InitializeComponent()
		}
		
		/// <summary>This function is notify the control that the parent window has been moved</summary>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		public virtual void OnParentMoved()
		{
		}
		
		/// <summary>This method handles all KeyDown notifications from the application</summary>
		/// <param name="eKey">The key being pressed</param>
		///	<param name="eModifiers">The current control/shift/alt key states</param>
		/// <returns>true if processed</returns>
		public virtual bool OnKeyDown(Keys eKey, Keys eModifiers)
		{
			return false;
		}
		
		/// <summary>
		/// This method is called to determine if the specified file is viewable
		/// by the derived control
		/// </summary>
		/// <param name="strFilename">Name of file to be viewed if possible</param>
		/// <returns>true if viewable</returns>
		public virtual bool IsViewable(string strFilename)
		{
			return false;
		}
		
		/// <summary>
		/// This method is called to activate the control
		/// </summary>
		public virtual void Activate()
		{
		}
		
		/// <summary>
		/// This method is called to deactivate the control
		/// </summary>
		public virtual void Deactivate()
		{
			//	Default bahavior is to unload the viewer
			Unload();
		}
		
		/// <summary>
		/// This method is called to perform the ActiveX initialization of the control
		/// </summary>
		/// <returns>true if successful</returns>
		public virtual bool AxInitialize()
		{
			//	Assume no ActiveX initialization required
			m_bInitialized = true;
			
			FireStateChange(TmxStates.Unloaded);
			
			return m_bInitialized;
		}
		
		/// <summary>
		/// This method is called to perform the ActiveX termination of the control
		/// </summary>
		public virtual void AxTerminate()
		{
			Unload();
			m_bInitialized = false;
		}
		
		/// <summary>This method is called to process a media bar command</summary>
		/// <param name="eCommand">The enumerated command identifier</param>
		/// <returns>true if processed</returns>
		public virtual bool ProcessCommand(TmaxMediaBarCommands eCommand)
		{
			return false;
		}
		
		/// <summary>This function is called to load the specified file</summary>
		/// <param name="strFilename">The name of the file to be viewed</param>
		/// <param name="lStart">The start page</param>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		/// <returns>true if successful</returns>
		public virtual bool View(string strFilename, long lStart)
		{
			if(strFilename == null)
				m_strFilename = "";
			else
				m_strFilename = strFilename;
				
			return true;
		}
		
		/// <summary>This function is called to play the specified file</summary>
		/// <param name="strFilename">The name of the file to be viewed</param>
		/// <param name="dStart">The start time</param>
		/// <param name="dStop">The stop time</param>
		/// <param name="bRun">True to run the playback after loading the file</param>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		/// <returns>true if successful</returns>
		public virtual bool Play(string strFilename, double dStart, double dStop, bool bRun)
		{
			if(strFilename == null)
				m_strFilename = "";
			else
				m_strFilename = strFilename;
				
			return true;
		}
		
		/// <summary>This function is called to play the specified file</summary>
		/// <param name="strFilename">The name of the file to be viewed</param>
		/// <param name="dStart">The start time</param>
		/// <param name="dStop">The stop time</param>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		/// <returns>true if successful</returns>
		public virtual bool Play(string strFilename, double dStart, double dStop)
		{				
			return Play(strFilename, dStart, dStop, true);
		}
		
		/// <summary>This function is called to play the specified file</summary>
		/// <param name="strFilename">The name of the file to be viewed</param>
		/// <param name="dStart">The start time</param>
		/// <param name="dStop">The stop time</param>
		/// <param name="bRun">True to run the playback after loading the file</param>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		/// <returns>true if successful</returns>
		public virtual bool Play(string strFilename, bool bRun)
		{				
			return Play(strFilename, 0, 0, bRun);
		}
		
		/// <summary>This function is called to play the specified file</summary>
		/// <param name="strFilename">The name of the file to be viewed</param>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		/// <returns>true if successful</returns>
		public virtual bool Play(string strFilename)
		{				
			return Play(strFilename, 0, 0, true);
		}
		
		/// <summary>This function is called to play the file that is currently loaded in the viewer</summary>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		/// <returns>true if successful</returns>
		public virtual bool Play()
		{				
			return Play(-1.0, -1.0);
		}
		
		/// <summary>This function is called to play the the active file using the specified range</summary>
		/// <param name="dStart">The start time</param>
		/// <param name="dStop">The stop time</param>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		/// <returns>true if successful</returns>
		public virtual bool Play(double dStart, double dStop)
		{
			return true;
		}
		
		/// <summary>This function is called to step the the active file from the start point to the stop point</summary>
		/// <param name="dStart">The start time</param>
		/// <param name="dStop">The stop time</param>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		/// <returns>true if successful</returns>
		public virtual bool Step(double dStart, double dStop)
		{
			return true;
		}
		
		/// <summary>This function is called to step the the active file from its current position the specified amount of time</summary>
		/// <param name="dTime">The step time</param>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		/// <returns>true if successful</returns>
		public virtual bool Step(double dTime)
		{
			return true;
		}
		
		/// <summary>This function is called to resume playback from the current positiion</summary>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		/// <returns>true if successful</returns>
		public virtual bool Resume()
		{
			return true;
		}
		
		/// <summary>This function is called to stop the playback</summary>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		/// <returns>true if successful</returns>
		public virtual bool Stop()
		{
			return true;
		}
		
		/// <summary>This function is called to pause the playback</summary>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		/// <returns>true if successful</returns>
		public virtual bool Pause()
		{
			return true;
		}
		
		/// <summary>This function is called to set the index and total used for navigator commands</summary>
		/// <param name="iPosition">The current position</param>
		/// <param name="iTotal">The total number of navigator positions</param>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		/// <returns>true if successful</returns>
		public virtual bool SetNavigatorPosition(int iPosition, int iTotal)
		{
			if(m_iNavigatorPosition != iPosition)
			{
				m_iNavigatorPosition = iPosition;
				OnNavigatorPositionChanged();
			}
			
			if(m_iNavigatorTotal != iTotal)
			{
				m_iNavigatorTotal = iTotal;
				OnNavigatorTotalChanged();
			}
			
			//	Alert the derived class
			OnNavigatorPropChanged();
				
			return true;
		
		}// public virtual bool SetNavigatorPosition(int iPosition, int iTotal)
		
		/// <summary>This function is called to unload the viewer</summary>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		public virtual void Unload()
		{
			//	Stop playback
			Stop();
			
			m_strFilename = "";
		}
		
		/// <summary>This function is called to process the specified request</summary>
		/// <param name="tmxRequest">The request parameters</param>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		/// <returns>true if successful</returns>
		public virtual bool ProcessRequest(CTmxRequest tmxRequest)
		{
			return true;
		}
		
		/// <summary>This function is called to cue the active file</summary>
		/// <param name="eMode">The enumerated cue mode</param>
		/// <param name="dPosition">The new position</param>
		/// <param name="bResume">true to resume playback</param>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		/// <returns>true if successful</returns>
		public virtual bool Cue(TmxCueModes eMode, double dPosition, bool bResume)
		{
			return true;
		}
		
		/// <summary>This function is called to cue the active file</summary>
		/// <param name="eMode">The enumerated cue mode</param>
		/// <param name="dPosition">The new position</param>
		/// <returns>true if successful</returns>
		public virtual bool Cue(TmxCueModes eMode, double dPosition)
		{
			return Cue(eMode, dPosition, false);
		}
		
		/// <summary>This function is called to cue the active file to the specified position</summary>
		/// <param name="dPosition">The new position</param>
		/// <returns>true if successful</returns>
		public virtual bool Cue(double dPosition)
		{
			return Cue(TmxCueModes.Absolute, dPosition, false);
		}
		
		/// <summary>This function is called to cue the active file</summary>
		/// <param name="eMode">The enumerated cue mode</param>
		/// <returns>true if successful</returns>
		public virtual bool Cue(TmxCueModes eMode)
		{
			return Cue(eMode, 0, false);
		}
		
		/// <summary>This method is called to save the ActiveX control properties to the IniFilename / IniSection</summary>
		/// <returns>true if successful</returns>
		public virtual bool AxSaveProperties()
		{
			return true;
		}
		
		/// <summary>This method is called to set the ActiveX control properties stored in the IniFilename / IniSection</summary>
		/// <returns>true if successful</returns>
		public virtual bool AxSetProperties()
		{
			return true;
		}
		
		#endregion Public Methods

		#region Properties
		
		/// <summary>This property gets a description of the viewer control</summary>
		public string Description
		{
			get	{ return m_strDescription; }
		}
		
		/// <summary>This property gets/sets the name of the file being displayed by the control </summary>
		public string Filename
		{
			get	{ return m_strFilename; }
		}
		
		/// <summary>This property gets/sets the visibility of the toolbar</summary>
		public bool ShowToolbar
		{
			get
			{
				return m_bShowToolbar;
			}
			set
			{
				m_bShowToolbar = value;
				
				//	Notify the derived class
				OnShowToolbarChanged();
			}
		
		}// ShowToolbar Property
		
		/// <summary>This property enables/disables the toolbar</summary>
		public bool EnableToolbar
		{
			get
			{
				return m_bEnableToolbar;
			}
			set
			{
				m_bEnableToolbar = value;
				
				//	Notify the derived class
				OnEnableToolbarChanged();
			}
		
		}// EnableToolbar Property
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}

		/// <summary>This property indicates if the ActiveX control has been initialized</summary>
		public bool Initialized
		{
			get	{ return m_bInitialized; }
		}
		
		/// <summary>INI file containing the ActiveX configuration information</summary>
		public string IniFilename
		{
			get	{ return m_strIniFilename; }
			set { m_strIniFilename = value; }
		}
		
		/// <summary>INI section containing the ActiveX configuration information</summary>
		public string IniSection
		{
			get	{ return m_strIniSection; }
			set { m_strIniSection = value; }
		}
		
		/// <summary>This property controls whether or not the ActiveX control automatically updates its configuration file when necessary</summary>
		public bool AxAutoSave
		{
			get	{ return m_bAxAutoSave; }
			set	{ m_bAxAutoSave = value; }
		}
		
		/// <summary>This property gets/sets the last ActiveX error code</summary>
		public short AxError
		{
			get	{ return m_sAxError; }
			set	{ m_sAxError = value; }
		}
		
		/// <summary>This property gets the current state of the control</summary>
		public TmxStates TmxState
		{
			get { return m_eTmxState; }
		}
		
		/// <summary>This current playback position of the control</summary>
		public double TmxPosition
		{
			get	{return m_dTmxPosition; }
		}
		
		/// <summary>This maximum allowed position</summary>
		public double TmxMaxPosition
		{
			get	{return m_dTmxMaxPosition; }
		}
		
		/// <summary>This minimum allowed position</summary>
		public double TmxMinPosition
		{
			get	{return m_dTmxMinPosition; }
		}
		
		/// <summary>This playback start position</summary>
		public double TmxStart
		{
			get	{return m_dTmxStart; }
		}
		
		/// <summary>This playback stop position</summary>
		public double TmxStop
		{
			get	{return m_dTmxStop; }
		}
		
		/// <summary>This playback duration</summary>
		public double TmxDuration
		{
			get	{return m_dTmxDuration; }
		}
		
		/// <summary>Id of current position used for navigator parameters</summary>
		public int NavigatorPosition
		{
			get	{return m_iNavigatorPosition; }
			set { SetNavigatorPosition(value, m_iNavigatorTotal); }
		}
		
		/// <summary>Total number of positions used for navigator parameters</summary>
		public int NavigatorTotal
		{
			get	{return m_iNavigatorTotal; }
			set { SetNavigatorPosition(m_iNavigatorPosition, value); }
		}
		
		#endregion Properties
		
		#region Protected Methods
		
		/// <summary>This method is called to fire an ActiveX viewer event</summary>
		/// <param name="Args">Event argument object</param>
		/// <returns>true if successful</returns>
		protected virtual bool FireEvent(CTmxEventArgs Args)
		{
			try
			{
				//	Is anybody registered?
				if(TmxEvent != null)
				{
					TmxEvent(this, Args);
				}

				return true;

			}
			catch
			{
				return false;
			}
			
		}
		
		/// <summary>This method is called to fire an Action event</summary>
		/// <param name="eAction">Enumerated action identifier</param>
		/// <param name="strFilename">Name of file associated with the event</param>
		///	<returns>The event arguments if successful</returns>
		protected CTmxEventArgs FireAction(TmxActions eAction, string strFilename)
		{
			//	Save the current image using a temporary filename
			try
			{
				CTmxEventArgs Args = new CTmxEventArgs(TmxEvents.Action, this);
			
				Args.Action   = eAction;
				Args.Filename = strFilename;
				
				//	Fire the event to notify the owners
				if(FireEvent(Args) == true)
					return Args;
			}
			catch
			{
				m_tmaxEventSource.FireError(this, "FireAction", "Unable to fire action event: " + eAction.ToString());
			}
			
			return null;

		}// protected void FireAction()
		
		/// <summary>This method is called to fire a StateChange event</summary>
		/// <param name="eState">Enumerated state identifier</param>
		///	<returns>The event arguments if successful</returns>
		protected CTmxEventArgs FireStateChange(TmxStates eState)
		{
			//	Update the local state property
			m_eTmxState = eState;
			
			try
			{
				CTmxEventArgs Args = new CTmxEventArgs(TmxEvents.StateChange, this);
			
				Args.State = eState;
				
				//	Fire the event to notify the owners
				if(FireEvent(Args) == true)
					return Args;
			}
			catch
			{
				m_tmaxEventSource.FireError(this, "FireStateChange", "Unable to fire state change event: " + eState.ToString());
			}
			
			return null;
			
		}// protected void FireStateChange()
		
		/// <summary>This method is called to fire a PositionChange event</summary>
		/// <param name="dPosition">The current position</param>
		///	<returns>The event arguments if successful</returns>
		protected CTmxEventArgs FirePositionChange(double dPosition)
		{
			//	Update the local property
			m_dTmxPosition = dPosition;
			
			try
			{
				CTmxEventArgs Args = new CTmxEventArgs(TmxEvents.PositionChange, this);
			
				Args.Position = dPosition;
				
				//	Fire the event to notify the owners
				if(FireEvent(Args) == true)
					return Args;
			}
			catch
			{
				m_tmaxEventSource.FireError(this, "FirePositionChange", "Unable to fire position change event: " + dPosition.ToString());
			}
			
			return null;
			
		}// protected void FireStateChange()
		
		/// <summary>This method is called to fire a QueryContinue event</summary>
		/// <param name="eAction">Enumerated action identifier</param>
		/// <param name="strFilename">Name of file associated with the event</param>
		/// <param name="iCallouts">Value to be assigned to Callout Count argument</param>
		///	<returns>The response to the query</returns>
		protected bool FireQueryContinue(TmxActions eAction, string strFilename, int iCallouts)
		{
			try
			{
				CTmxEventArgs Args = new CTmxEventArgs(TmxEvents.QueryContinue, this);
			
				Args.Action       = eAction;
				Args.Filename     = strFilename;
				Args.CalloutCount = iCallouts;
				
				//	Fire the event to notify the owners
				if(FireEvent(Args) == true)
					return Args.Continue;
			}
			catch
			{
				m_tmaxEventSource.FireError(this, "FireQueryContinue", "Unable to fire query contiue event: " + eAction.ToString());
			}
			
			return false;
			
		}// protected bool FireQueryContinue(TmxActions eAction, string strFilename, int iCallouts)
		
		/// <summary>This method is called to fire a QueryContinue event</summary>
		/// <param name="eAction">Enumerated action identifier</param>
		/// <param name="strFilename">Name of file associated with the event</param>
		///	<returns>The response to the query</returns>
		protected bool FireQueryContinue(TmxActions eAction, string strFilename)
		{
			return FireQueryContinue(eAction, strFilename, 0);
			
		}// protected bool FireQueryContinue(TmxActions eAction, string strFilename)
		
		/// <summary>This method is called to fire a QueryContinue event</summary>
		/// <param name="eAction">Enumerated action identifier</param>
		///	<returns>The response to the query</returns>
		protected bool FireQueryContinue(TmxActions eAction)
		{
			return FireQueryContinue(eAction, "", 0);
			
		}// protected bool FireQueryContinue(TmxActions eAction)
		
		/// <summary>Clean up any resources being used</summary>
		/// <param name="disposing">true if disposing of the object</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
			}
			base.Dispose(disposing);
		}

		/// <summary>Designer uses this function to initialize child controls</summary>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		protected virtual void InitializeComponent()
		{
		}
		
		/// <summary>Called by the base class when the value of the NavigatorPosition property changes</summary>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		protected virtual void OnNavigatorPositionChanged()
		{
		}
		
		/// <summary>Called by the base class when any navigator changes</summary>
		/// <remarks>It is expected that derived classes will override this function
		///			 It allows for trapping all notifications in a single method</remarks>
		protected virtual void OnNavigatorPropChanged()
		{
		}
		
		/// <summary>Called by the base class when the value of the NavigatorPosition property changes</summary>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		protected virtual void OnNavigatorTotalChanged()
		{
		}
		
		/// <summary>This method is called to notify the derived class that the ShowToolbar property has changed</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">Infragistics event object</param>
		protected virtual void OnShowToolbarChanged()
		{
			UltraToolbar Toolbar = null;
			
			if((GetUltraManager() != null) && (GetUltraManager().Toolbars != null))
			{
				if((Toolbar = GetUltraManager().Toolbars["MainToolbar"]) != null)
				{
					Toolbar.Visible = m_bShowToolbar;
					
					//	Make sure the button states are set if visible
					if(m_bShowToolbar == true)
						SetUltraToolStates();
				}
				
			}

		}
		
		/// <summary>This method is called to notify the derived class that the EnableToolbar property has changed</summary>
		protected virtual void OnEnableToolbarChanged()
		{
			SetUltraToolStates();
		}
		
		/// <summary>This method traps events fired when the user clicks on a toolbar button</summary>
		/// <param name="sender">the object sending the event</param>
		/// <param name="e">Infragistics event object</param>
		protected virtual void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		{
			Debug.Assert(e.Tool != null);
			Debug.Assert(e.Tool.Key != null);
			if((m_tmaxMediaBar == null) || (e.Tool == null) || (e.Tool.Key == null)) return;
			
			if(m_bProcessToolClicks == false) return;
			
			//	Get the associated media bar command identifier
			TmaxMediaBarCommands eCommand = m_tmaxMediaBar.GetCommand(e.Tool.Key);

            if (eCommand == TmaxMediaBarCommands.BlankPresentation)
            {
                if (OnRequestPresentation != null)
                    OnRequestPresentation(this, null);
            }
			
			//	Process the command
			ProcessCommand(eCommand);

		}// protected virtual void OnUltraToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		
		/// <summary>This method is called to get the Infragistics toolbar manager</summary>
		/// <returns>The viewer's toolbar manager</returns>
		/// <remarks>This method should be overridden by derived class that want to use the toolbar helper methods</remarks>
		protected virtual Infragistics.Win.UltraWinToolbars.UltraToolbarsManager GetUltraManager()
		{
			return null;
		}

		/// <summary>This method is called to get the object's component container</summary>
		/// <returns>The viewer's component container</returns>
		/// <remarks>This method should be overridden by derived class that want to use the toolbar helper methods</remarks>
		protected virtual System.ComponentModel.IContainer GetComponentContainer()
		{
			return null;
		}

		/// <summary>This method is called to initialize the viewer's toolbar</summary>
		protected bool InitializeUltraToolbar()
		{
			if(GetUltraManager() == null) return false;
			
			//	Initialize the local media bar object
			m_tmaxMediaBar = new CTmaxMediaBar();
			if(m_tmaxMediaBar.Initialize(GetComponentContainer()) == false) return false;
			
			//	Assign the image list
			GetUltraManager().ImageSizeSmall = new System.Drawing.Size(m_tmaxMediaBar.ButtonWidth, m_tmaxMediaBar.ButtonHeight);
			GetUltraManager().ImageListSmall = m_tmaxMediaBar.Images;
			
			// Assign the appropriate command enumerator to each tool
			foreach(ToolBase ultraTool in GetUltraManager().Tools)
			{
				ultraTool.Tag = m_tmaxMediaBar.GetCommand(ultraTool.Key);
				ultraTool.SharedProps.AppearancesSmall.Appearance.Image = m_tmaxMediaBar.GetImageIndex((TmaxMediaBarCommands)ultraTool.Tag);
			}
			
			//	Set the visibility of the toolbar
			OnShowToolbarChanged();
			
			return true;
		
		}// protected bool InitializeToolbar()

		/// <summary>This function will retrieve the tool with the specified key from the toolbar manager</summary>
		/// <param name="strKey">Alpha-numeric tool key identifier</param>
		/// <returns>Infragistic base class tool object</returns>
		protected ToolBase GetUltraTool(string strKey)
		{
			ToolBase Tool = null;
					
			try
			{
				if(GetUltraManager() != null)
				{
					Tool = GetUltraManager().Tools[strKey];
				}
			}
			catch
			{
			}
			
			return Tool;
		
		}// protected ButtonTool GetUltraTool(string strKey)
				
		/// <summary>This function will retrieve the tool associated with the specified media bar command</summary>
		/// <param name="eCommand">The media bar command enumeration</param>
		/// <returns>Infragistic base class tool object</returns>
		protected ToolBase GetUltraTool(TmaxMediaBarCommands eCommand)
		{
			return GetUltraTool(eCommand.ToString());
		}
		
		/// <summary>This function will retrieve the button tool with the specified key from the toolbar manager</summary>
		/// <param name="strKey">Alpha-numeric tool key identifier</param>
		/// <returns>Infragistic button class tool object</returns>
		protected ButtonTool GetUltraButton(string strKey)
		{
			ButtonTool Tool = null;
			
			try
			{
				Tool = (ButtonTool)GetUltraTool(strKey);
			}
			catch
			{
			}
			
			return Tool;			

		}// protected ButtonTool GetUltraButton(string strKey)
				
		/// <summary>This function is called to set the states of the toolbar buttons</summary>
		protected virtual void SetUltraToolStates()
		{
			if(GetUltraManager() != null)
			{
				// enable/disable the tools
				foreach(ToolBase ultraTool in GetUltraManager().Tools)
				{
					ultraTool.SharedProps.Enabled = m_bEnableToolbar;
				}
			
			}
		
		}//	protected virtual void SetUltraToolStates()
		
		/// <summary>This function will retrieve the button tool with the specified media bar command identifier</summary>
		/// <param name="eCommand">The media bar command enumeration</param>
		/// <returns>Infragistic button class tool object</returns>
		protected ButtonTool GetUltraButton(TmaxMediaBarCommands eCommand)
		{
			return GetUltraButton(eCommand.ToString());			

		}// protected ButtonTool GetUltraButton(TmaxMediaBarCommands eCommand)
				
		/// <summary>
		/// This function is called to set the check state of a toolbar/menu tool
		/// </summary>
		/// <param name="strKey">Key of the tool to be set</param>
		/// <param name="bChecked">New check state</param>
		protected void SetUltraToolChecked(string strKey, bool bChecked)
		{
			StateButtonTool Tool = null;
					
			try
			{
				if((Tool = (StateButtonTool)GetUltraButton(strKey)) != null)
					Tool.Checked = bChecked;
			}
			catch
			{
			}

		}//	protected void SetUltraToolChecked(string strKey, bool bChecked)
		
		/// <summary>This function is called to set the check state of the tool</summary>
		/// <param name="eCommand">Media bar command id of the tool to be set</param>
		/// <param name="bChecked">New check state</param>
		protected void SetUltraToolChecked(TmaxMediaBarCommands eCommand, bool bChecked)
		{
			SetUltraToolChecked(eCommand.ToString(), bChecked);

		}//	protected void SetUltraToolChecked(TmaxMediaBarCommands eCommand, bool bChecked)
		
		/// <summary>
		/// This function is called to enable/disable the specified tool
		/// </summary>
		/// <param name="strKey">Key of the tool to be set</param>
		/// <param name="bEnabled">true to enable the tool</param>
		protected void SetUltraToolEnabled(string strKey, bool bEnabled)
		{
			ToolBase Tool = null;
					
			try
			{
				if((Tool = GetUltraTool(strKey)) != null)
					Tool.SharedProps.Enabled = bEnabled;
			}
			catch
			{
			}

		}//	protected void SetUltraToolEnabled(string strKey, bool bChecked)
		
		/// <summary>This function is called to enable/disable the specified tool</summary>
		/// <param name="strKey">Media bar command id of the tool to be set</param>
		/// <param name="bEnabled">true to enable the tool</param>
		protected void SetUltraToolEnabled(TmaxMediaBarCommands eCommand, bool bEnabled)
		{
			SetUltraToolEnabled(eCommand.ToString(), bEnabled);

		}//	protected void SetUltraToolEnabled(TmaxMediaBarCommands eCommand, bool bChecked)
		
		/// <summary>This method will translate the specified ActiveX viewer mouse button to a .NET button identifier</summary>
		/// <param name="iButton">ActiveX viewer button identifier</param>
		/// <returns>The equivalent .NET identifier</returns>
		protected virtual System.Windows.Forms.MouseButtons TranslateMouseButton(int iButton)
		{
			//	During Drag/Drop within .NET the ActiveX controls tend to loose track
			//	of the correct MouseButton value. This will correct that problem
			
			return Control.MouseButtons;
			
		}// private MouseButtons TranslateMouseButton(TmxViewMouseButtons eButton)

		#endregion Protected Methods
		
	}
}
