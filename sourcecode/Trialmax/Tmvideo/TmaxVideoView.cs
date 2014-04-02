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

/// <summary>Namespace containing support services for TrialMax video viewer application</summary>
namespace FTI.Trialmax.TMVV.Tmvideo
{
	/// <summary>Base class used to create all child views</summary>
	public class CTmaxVideoView : System.Windows.Forms.UserControl
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_BASE_VIEW_RESERVED_0			= 0;
		protected const int ERROR_BASE_VIEW_RESERVED_1			= 1;
		protected const int ERROR_BASE_VIEW_RESERVED_2			= 2;
		protected const int ERROR_BASE_VIEW_RESERVED_3			= 3;
		protected const int ERROR_BASE_VIEW_RESERVED_4			= 4;
		protected const int ERROR_BASE_VIEW_RESERVED_5			= 5;
		protected const int ERROR_BASE_VIEW_RESERVED_6			= 6;
		protected const int ERROR_BASE_VIEW_RESERVED_7			= 7;
		protected const int ERROR_BASE_VIEW_RESERVED_8			= 8;
		protected const int ERROR_BASE_VIEW_RESERVED_9			= 9;
		protected const int ERROR_BASE_VIEW_RESERVED_10			= 10;
		protected const int ERROR_BASE_VIEW_RESERVED_11			= 11;
		protected const int ERROR_BASE_VIEW_RESERVED_12			= 12;
		protected const int ERROR_BASE_VIEW_RESERVED_13			= 13;
		protected const int ERROR_BASE_VIEW_RESERVED_14			= 14;
		protected const int ERROR_BASE_VIEW_RESERVED_15			= 15;
		
		// Derived classes should start their error numbering with this value
		protected const int ERROR_BASE_VIEW_MAX					= 15;
		
		#endregion Constants
		
		#region Protected Members
		
		/// <summary>Local member used to construct error messages</summary>
		protected FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();
		
		/// <summary>Local member bounded to EventSource property</summary>
		protected FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member bounded to AppOptions property</summary>
		protected FTI.Trialmax.TMVV.Tmvideo.CTmaxVideoOptions m_tmaxAppOptions = null;
		
		/// <summary>Local member bound to the AppId property</summary>
		protected TmaxVideoViews m_eAppId = TmaxVideoViews.MaxViews;
		
		/// <summary>Local member bound to the ViewName property</summary>
		protected string m_strViewName = "";
		
		/// <summary>Local member associated with the Active property</summary>
		protected bool m_bActive = false;
		
		/// <summary>The application's active script</summary>
		protected FTI.Shared.Xml.CXmlScript m_xmlScript = null;
		
		#endregion Protected Members

		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxVideoView()
		{
			m_tmaxEventSource.Name = this.ViewName;
			
			//	Populate the error builder's format string collection
			SetErrorStrings();
			
			//	NOTE:	It is up to the derived class to call InitializeComponent()
			//			in it's constructor. This ensures that the derived constructor
			//			gets called before InitializeComponent()
		}
		
		/// <summary>This method is called by the application to initialize the view</summary>
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
		
		/// <summary>This function is called by the application when the user opens a new XML script</summary>
		/// <param name="xmlScript">The new XML script</param>
		/// <returns>true if successful</returns>
		public virtual bool OnAppOpened(CXmlScript xmlScript)
		{
			m_xmlScript = xmlScript;
			return true;	
		}
		
		/// <summary>This function is called by the application when the user is about to open the preferences form</summary>
		public virtual void OnAppBeforeSetPreferences()
		{
		}
		
		/// <summary>This function is called by the application when the user is is finished setting the preferences</summary>
		/// <param name="bApplied">true if changes have been applied</param>
		public virtual void OnAppAfterSetPreferences(bool bApplied)
		{
		}
		
		/// <summary>This function is called when the application options object changes</summary>
		public virtual void OnAppOptionsChanged()
		{
		}
		
		/// <summary>This method is called by the application when it processes a Select command event</summary>
		/// <param name="tmaxItem">The item to be selected</param>
		/// <param name="eView">The view requesting selection</param>
		/// <returns>true if successful</returns>
		public virtual bool OnTmaxVideoActivate(CTmaxItem tmaxItem, TmaxVideoViews eView)
		{
			return true;
		}
		
		/// <summary>This method is called by the application when it processes an Add command event</summary>
		/// <param name="tmaxItem">The item that represents the parent element</param>
		/// <param name="eView">The view that added the new elements</param>
		/// <returns>true if successful</returns>
		public virtual bool OnTmaxVideoAdd(CTmaxItem tmaxItem, TmaxVideoViews eView)
		{
			return true;
		}
		
		/// <summary>This method is called by the application when it processes a Delete command event</summary>
		/// <param name="tmaxItem">The item that represents the parent element</param>
		/// <param name="eView">The view that deleted the new elements</param>
		/// <returns>true if successful</returns>
		public virtual bool OnTmaxVideoDelete(CTmaxItem tmaxItem, TmaxVideoViews eView)
		{
			return true;
		}
		
		/// <summary>This method is called by the application when it processes a Reorder command event</summary>
		/// <param name="tmaxItem">The item that represents the parent element</param>
		/// <param name="eView">The view that deleted the new elements</param>
		/// <returns>true if successful</returns>
		public virtual bool OnTmaxVideoReorder(CTmaxItem tmaxItem, TmaxVideoViews eView)
		{
			return true;
		}
		
		/// <summary>This method is called by the application when it processes an Update command event</summary>
		/// <param name="tmaxItem">The item that represents the parent element</param>
		/// <param name="eView">The view that deleted the new elements</param>
		/// <returns>true if successful</returns>
		public virtual bool OnTmaxVideoUpdate(CTmaxItem tmaxItem, TmaxVideoViews eView)
		{
			return true;
		}
		
		/// <summary>This method is called by the application when it processes an Update command event</summary>
		/// <param name="tmaxItem">The item that represents the parent element</param>
		/// <param name="eView">The view that deleted the new elements</param>
		/// <returns>true if successful</returns>
		public virtual bool OnTmaxVideoLoadResult(CTmaxVideoResult tmaxResult)
		{
			return true;
		}
		
		/// <summary>This method handles all Hotkey notifications from the application</summary>
		/// <param name="eHotkey">The enumerated hotkey</param>
		/// <returns>true if processed by the view</returns>
		public virtual bool OnAppHotkey(TmaxHotkeys eHotkey)
		{
			return false;
		}
		
		/// <summary>This method handles all KeyDown notifications from the application</summary>
		/// <param name="eKey">The key being pressed</param>
		///	<param name="eModifiers">The current control/shift/alt key states</param>
		/// <returns>true if processed</returns>
		public virtual bool OnAppKeyDown(Keys eKey, Keys eModifiers)
		{
			return false;
		}
		
		/// <summary>This method is called by the application to get a list of items that represent the current selections for a Find command</summary>
		/// <returns>The items that represent the current selections</returns>
		public virtual CTmaxItems GetSearchItems()
		{
			return null;
		
		}// public virtual CTmaxItems GetSearchItems()
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Called to retrieve the path to the video file associated with the specified designation</summary>
		/// <param name="xmlDesignation">The desired XML designation</param>
		/// <param name="bConfirm">true to confirm it's existance</param>
		/// <param name="bSilent">true to inhibit warning messages</param>
		/// <returns>The path to the video file</returns>
		protected string GetVideoFileSpec(CXmlDesignation xmlDesignation, bool bConfirm, bool bSilent)
		{
			if((m_tmaxAppOptions != null) && (m_xmlScript != null))
				return m_tmaxAppOptions.GetVideoFileSpec(m_xmlScript, xmlDesignation, bConfirm, bSilent);
			else
				return xmlDesignation.FileSpec;
			
		}// protected string GetVideoFileSpec(CXmlDesignation xmlDesignation, bool bConfirm, bool bSilent)
		
		/// <summary>This method is called to warn the user when an invalid range value is encountered</summary>
		/// <param name="strMsg">The warning message</param>
		protected virtual bool Warn(string strMsg)
		{
			MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation);
			
			return false; // allows for cleaner code						
		
		}// protected virtual void Warn(string strMsg)
		
		/// <summary>Designer uses this function to initialize child controls</summary>
		/// <remarks>It is expected that derived classes will override this function</remarks>
		protected virtual void InitializeComponent()
		{
			// 
			// CTmaxVideoView
			// 
			this.AllowDrop = true;
			this.Name = "CTmaxVideoView";

		}
		
		/// <summary>Clean up all resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
		
		/// <summary>This function is called when the ViewId property changes</summary>
		protected virtual void OnAppIdChanged()
		{
		}
		
		/// <summary>This function is called when the Active property changes</summary>
		protected virtual void OnActiveChanged()
		{
		}
		
		/// <summary>This function is called when the ViewName property changes</summary>
		protected virtual void OnViewNameChanged()
		{
			m_tmaxEventSource.Name = m_strViewName;
		}
		
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected virtual void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CTmaxVideoView error string 0");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CTmaxVideoView error string 1");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CTmaxVideoView error string 2");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CTmaxVideoView error string 3");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CTmaxVideoView error string 4");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CTmaxVideoView error string 5");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CTmaxVideoView error string 6");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CTmaxVideoView error string 7");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CTmaxVideoView error string 8");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CTmaxVideoView error string 9");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CTmaxVideoView error string 10");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CTmaxVideoView error string 11");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CTmaxVideoView error string 12");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CTmaxVideoView error string 13");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CTmaxVideoView error string 14");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CTmaxVideoView error string 15");

		}// protected virtual void SetErrorStrings()

		/// <summary>This method is called to determine if the user is pressing the Control key</summary>
		/// <returns>True if Control key is pressed</returns>
		protected virtual bool GetControlPressed()
		{
			//	Is the user pressing the control key?
			if((Shared.Win32.User.GetKeyState(Shared.Win32.User.VK_CONTROL) & 0x8000) != 0)
				return true;
			else
				return false;
				
		}// protected virtual bool GetControlPressed()
		
		/// <summary>This method is called to determine if the user is pressing the Shift key</summary>
		/// <returns>True if Shift key is pressed</returns>
		protected virtual bool GetShiftPressed()
		{
			//	Is the user pressing the control key?
			if((Shared.Win32.User.GetKeyState(Shared.Win32.User.VK_SHIFT) & 0x8000) != 0)
				return true;
			else
				return false;
				
		}// protected virtual bool GetShiftPressed()
		
		#endregion Protected Methods
		
		#region Command Events
		
		/// <summary>This event is fired by a view to issue a command</summary>
		public event TmaxVideoHandler TmaxVideoCommandEvent;
		
		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="Args">Command argument object</param>
		/// <returns>The command argument object</returns>
		protected virtual CTmaxVideoArgs FireCommand(CTmaxVideoArgs Args)
		{
			try
			{
				//	Is anybody registered?
				if(TmaxVideoCommandEvent != null)
					TmaxVideoCommandEvent(this, Args);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "FireCommand", Ex);
			}
			
			return Args;
		}
		
		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItems">The collection of items associated with the command event</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		/// <returns>The command argument object</returns>
		protected virtual CTmaxVideoArgs FireCommand(TmaxVideoCommands eCommand, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)
		{
			CTmaxVideoArgs Args = null;
			
			try
			{
				// Get the command arguments
				if((Args = new CTmaxVideoArgs(eCommand, m_eAppId, tmaxItems, tmaxParameters)) != null)
				{
					return FireCommand(Args);
				}
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "FireCommand", "Fire " + eCommand.ToString() + " command Ex: " + Ex.ToString());
			}
			
			return Args;
		
		}

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItems">The collection of items associated with the command event</param>
		/// <returns>The command argument object</returns>
		protected virtual CTmaxVideoArgs FireCommand(TmaxVideoCommands eCommand, CTmaxItems tmaxItems)
		{
			return FireCommand(eCommand, tmaxItems, null);
		}

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItem">The item to be passed with the event</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		/// <returns>The command argument object</returns>
		protected virtual CTmaxVideoArgs FireCommand(TmaxVideoCommands eCommand, CTmaxItem tmaxItem, CTmaxParameters tmaxParameters)
		{
			CTmaxItems tmaxItems = new CTmaxItems();
			
			tmaxItems.Add(tmaxItem);
			
			return FireCommand(eCommand, tmaxItems, tmaxParameters);
		}

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItem">The item to be passed with the event</param>
		/// <returns>The command argument object</returns>
		protected virtual CTmaxVideoArgs FireCommand(TmaxVideoCommands eCommand, CTmaxItem tmaxItem)
		{
			return FireCommand(eCommand, tmaxItem, null);
		}

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <returns>The command argument object</returns>
		protected virtual CTmaxVideoArgs FireCommand(TmaxVideoCommands eCommand)
		{
			return FireCommand(eCommand, (CTmaxItems)null, (CTmaxParameters)null);
		}

		/// <summary>This method will handle TmaxCommand events</summary>
		/// <param name="objSender">The object sending the event</param>
		/// <param name="Args">Command event arguments</param>
		protected virtual void OnTmaxCommand(object objSender, CTmaxVideoArgs Args)
		{
			//	Default behavior is to propagate the command but switch the sender
			//	to make is look like it originated with this pane
			if(TmaxVideoCommandEvent != null)
				TmaxVideoCommandEvent(this, Args);			
		}
		
		
		#endregion Command Events
		
		#region Properties		

		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}
		
		/// <summary>The global application options</summary>
		public CTmaxVideoOptions AppOptions
		{
			get { return m_tmaxAppOptions; }
			set 
			{ 
				m_tmaxAppOptions = value; 
				OnAppOptionsChanged(); 
			}
			
		}
		
		/// <summary>This property is used to control whether or not the view is active</summary>
		public bool Active
		{
			get
			{
				return m_bActive;
			}
			set
			{
				if(m_bActive != value)
				{
					m_bActive = value;
				
					//	Notify the derived class
					OnActiveChanged();
				}
			}
		
		}
		
		/// <summary>This property is used to assign an identifier to the view</summary>
		/// <remarks>The identifier is used when the view fires events</remarks>
		public TmaxVideoViews AppId
		{
			get
			{
				return m_eAppId;
			}
			set
			{
				//	Update the id
				m_eAppId = value;
				
				//	Notify the derived class
				OnAppIdChanged();
			}
		}
		
		/// <summary>This property is used to assign a name to the view</summary>
		public string ViewName
		{
			get
			{
				return m_strViewName;
			}
			set
			{
				//	Update the name
				m_strViewName = value;
				
				//	Notify the derived class
				OnViewNameChanged();
			}
		}
		
		#endregion Properties
		
	}// CTmaxVideoView

} // namespace FTI.Trialmax.TMVV.Tmvideo

