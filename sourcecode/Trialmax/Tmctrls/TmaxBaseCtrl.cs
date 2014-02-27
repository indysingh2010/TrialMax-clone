using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Trialmax.Controls
{
	/// <summary>This is the base class for TrialMax video controls</summary>
	public class CTmaxBaseCtrl : System.Windows.Forms.UserControl, ITmaxAppNotification
	{
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_TMAX_BASE_CONTROL_FIRE_COMMAND_EX	= 0;
		protected const int ERROR_TMAX_BASE_CONTROL_RESERVED_1		= 1;
		protected const int ERROR_TMAX_BASE_CONTROL_RESERVED_2		= 2;
		protected const int ERROR_TMAX_BASE_CONTROL_RESERVED_3		= 3;
		protected const int ERROR_TMAX_BASE_CONTROL_RESERVED_4		= 4;
		protected const int ERROR_TMAX_BASE_CONTROL_RESERVED_5		= 5;
		protected const int ERROR_TMAX_BASE_CONTROL_RESERVED_6		= 6;
		protected const int ERROR_TMAX_BASE_CONTROL_RESERVED_7		= 7;
		
		// Derived classes should start their error numbering with this value
		protected const int ERROR_TMAX_BASE_CONTROL_MAX			= 7;
		
		#endregion Constants
		
		#region Protected Members
		
		/// <summary>Local member bound to EventSource property</summary>
		protected FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Local member used to construct error messages</summary>
		protected FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();

		/// <summary>Local member bound to PaneId property</summary>
		protected int m_iPaneId = 0;
		
		#endregion Protected Members
		
		#region Public Methods
		
		/// <summary>This event is fired by a form to issue a command</summary>
		public event FTI.Shared.Trialmax.TmaxCommandHandler TmaxCommandEvent;
		
		/// <summary>Default constructor</summary>
		public CTmaxBaseCtrl()
		{
			//	Set the default event source name
			m_tmaxEventSource.Name = "Base TrialMax Control";
			
			//	Populate the error builder's format string collection
			SetErrorStrings();
		
			//	NOTE:	It is up to the derived class to call InitializeComponent()
			//			in it's constructor. This ensures that the derived constructor
			//			gets called before InitializeComponent()
		}
		
		/// <summary>This method is called by the application when it adds new media to the database</summary>
		/// <param name="tmaxParent">TrialMax event item that identifies the parent record</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		public virtual void OnAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		{
		}
		
		/// <summary>This method is called by the application to when the item gets deleted</summary>
		/// <param name="tmaxItem">The item that has been deleted</param>
		public virtual void OnDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
		}
		
		/// <summary>This method is called by the application when multiple records have been updated in an operation</summary>
		/// <param name="tmaxItems">The items that have been updated</param>
		public virtual void OnUpdated(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
		}
		
		/// <summary>This method is called by the application to when the item's child collection has been reordered</summary>
		/// <param name="tmaxItem">The item that owns the child collection</param>
		public virtual void OnReordered(FTI.Shared.Trialmax.CTmaxItem tmaxItem)
		{
		}

		/// <summary>This method is called by the application when it adds new objections to the database</summary>
		/// <param name="tmaxParent">TrialMax event item that identifies the parent record</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		public virtual void OnObjectionsAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		{
		}

		/// <summary>This method is called by the application to when the objection records are deleted</summary>
		/// <param name="tmaxItem">The records that have been deleted</param>
		public virtual void OnObjectionsDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
		}

		/// <summary>This method is called by the application when objection records have been updated</summary>
		/// <param name="tmaxItems">The records that have been updated</param>
		public virtual void OnObjectionsUpdated(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
		}

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			if(disposing == true)
			{
				if(m_tmaxErrorBuilder != null)
				{
					if(m_tmaxErrorBuilder.FormatStrings != null)
						m_tmaxErrorBuilder.FormatStrings.Clear();
						
					m_tmaxErrorBuilder = null;
				}
			
			}// if(disposing == true)
				
		}// protected override void Dispose(bool disposing)

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		protected virtual void InitializeComponent()
		{
			// 
			// CTmaxBaseCtrl
			// 
			this.Name = "CTmaxBaseCtrl";
			this.Size = new System.Drawing.Size(196, 188);

		}
		
		/// <summary>This function handles all Resize events</summary>
		/// <param name="e">System event parameters - no data</param>
		protected override void OnResize(System.EventArgs e)
		{
			//	Perform the base class processing first
			base.OnResize(e);
			
			//	Make sure the controls are properly sized
			RecalcLayout();
			
		}// protected override void OnResize(System.EventArgs e)

		/// <summary>This function overrides the default implementation</summary>
		/// <param name="e">System event parameters - no data</param>
		protected override void OnLoad(System.EventArgs e)
		{
			//	Perform the base class processing first
			base.OnLoad(e);
			
			//	Make sure the controls are properly sized
			RecalcLayout();
		}

		/// <summary>This function is called to resize and reposition the panes child controls</summary>
		protected virtual void RecalcLayout()
		{
		}
			
		/// <summary>This function is called to determine if the owner is connected to the command event</summary>
		protected virtual bool IsCommandConnected()
		{
			return (TmaxCommandEvent != null);
		}
			
		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected virtual void SetErrorStrings()
		{
			Debug.Assert(m_tmaxErrorBuilder != null);
			Debug.Assert(m_tmaxErrorBuilder.FormatStrings != null);
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;

			//	Add placeholders for the reserved strings
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fire the command event: command = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("CTmaxBaseCtrl reserved base class error #1");
			m_tmaxErrorBuilder.FormatStrings.Add("CTmaxBaseCtrl reserved base class error #2");
			m_tmaxErrorBuilder.FormatStrings.Add("CTmaxBaseCtrl reserved base class error #3");
			m_tmaxErrorBuilder.FormatStrings.Add("CTmaxBaseCtrl reserved base class error #4");
			m_tmaxErrorBuilder.FormatStrings.Add("CTmaxBaseCtrl reserved base class error #5");
			m_tmaxErrorBuilder.FormatStrings.Add("CTmaxBaseCtrl reserved base class error #6");
			m_tmaxErrorBuilder.FormatStrings.Add("CTmaxBaseCtrl reserved base class error #7");
			
		}// protected void SetErrorStrings()

		/// <summary>This method is called to warn the user when an invalid range value is encountered</summary>
		/// <param name="strMsg">The warning message</param>
		/// <param name="ctrlSelect">the control to select after the warning is displayed</param>
		protected virtual bool Warn(string strMsg, System.Windows.Forms.Control ctrlSelect)
		{
			MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation);
			
			if(ctrlSelect != null)
				ctrlSelect.Focus();	
				
			return false; // allows for cleaner code						
		
		}// protected virtual bool Warn(string strMsg, System.Windows.Forms.TextBox ctrlSelect)
		
		/// <summary>This method is called to warn the user when an invalid range value is encountered</summary>
		/// <param name="strMsg">The warning message</param>
		protected virtual bool Warn(string strMsg)
		{
			return Warn(strMsg, null);			
		}
		
		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="Args">Command argument object</param>
		/// <returns>The argument object used to fire the command event</returns>
		protected virtual CTmaxCommandArgs FireCommand(CTmaxCommandArgs Args)
		{
			try
			{
				//	Is anybody registered?
				if(TmaxCommandEvent != null)
				{
					TmaxCommandEvent(this, Args);
				}
				else
				{
					Args.Successful = false;
				}
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FireCommand", m_tmaxErrorBuilder.Message(ERROR_TMAX_BASE_CONTROL_FIRE_COMMAND_EX, Args.Command), Ex);
				Args.Successful = false;
			}
			
			return Args;
			
		}// protected virtual CTmaxCommandArgs FireCommand(CTmaxCommandArgs Args)

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItems">The collection of items associated with the command event</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		/// <returns>The argument object used to fire the command event</returns>
		protected virtual CTmaxCommandArgs FireCommand(TmaxCommands eCommand, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)
		{
			CTmaxCommandArgs Args = null;
			
			try
			{
				// Get the command arguments
				if((Args = new CTmaxCommandArgs(eCommand, m_iPaneId, tmaxItems, tmaxParameters)) != null)
				{
					Args.Successful = false;
					
					//	Fire the command event
					return FireCommand(Args);
				}
			
			}
			catch
			{
			}
			
			return Args;
		
		}//	protected virtual CTmaxCommandArgs FireCommand(TmaxCommands eCommand, CTmaxItems tmaxItems, CTmaxParameters tmaxParameters)

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItems">The collection of items associated with the command event</param>
		/// <returns>The argument object used to fire the command event</returns>
		protected virtual CTmaxCommandArgs FireCommand(TmaxCommands eCommand, CTmaxItems tmaxItems)
		{
			return FireCommand(eCommand, tmaxItems, null);
		
		}//	protected virtual CTmaxCommandArgs FireCommand(TmaxCommands eCommand, CTmaxItems tmaxItems)

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItem">The item to be passed with the event</param>
		/// <param name="tmaxParameters">The collection of parameters associated with the event</param>
		/// <returns>The argument object used to fire the command event</returns>
		protected virtual CTmaxCommandArgs FireCommand(TmaxCommands eCommand, CTmaxItem tmaxItem, CTmaxParameters tmaxParameters)
		{
			CTmaxItems tmaxItems = new CTmaxItems();
			
			tmaxItems.Add(tmaxItem);
			
			return FireCommand(eCommand, tmaxItems, tmaxParameters);
		
		}//	protected virtual CTmaxCommandArgs FireCommand(TmaxCommands eCommand, CTmaxItem tmaxItem, CTmaxParameters tmaxParameters)

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <param name="tmaxItem">The item to be passed with the event</param>
		/// <returns>The argument object used to fire the command event</returns>
		protected virtual CTmaxCommandArgs FireCommand(TmaxCommands eCommand, CTmaxItem tmaxItem)
		{
			return FireCommand(eCommand, tmaxItem, null);
		
		}//	protected virtual CTmaxCommandArgs FireCommand(TmaxCommands eCommand, CTmaxItem tmaxItem)

		/// <summary>This method is called to fire a command event using the specified values</summary>
		/// <param name="eCommand">The TrialMax command identifier</param>
		/// <returns>The argument object used to fire the command event</returns>
		protected virtual CTmaxCommandArgs FireCommand(TmaxCommands eCommand)
		{
			return FireCommand(eCommand, (CTmaxItems)null, (CTmaxParameters)null);
		
		}//	protected virtual CTmaxCommandArgs FireCommand(TmaxCommands eCommand)

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
		
		/// <summary>This method is called to request or withdraw application notifications</summary>
		/// <param name="bNotify">True to request notifications</param>
		/// <returns>True if successful</returns>
		protected virtual bool SetAppNotifications(bool bNotify)
		{
			CTmaxCommandArgs	Args = null;
			CTmaxItem			tmaxItem = null;
			bool				bSuccessful = false;
			
			try
			{
				//	Allocate and initialize the event item
				tmaxItem = new CTmaxItem();
				tmaxItem.IAppNotification = this;
				
				//	Fire the application command
				if((Args = FireCommand(bNotify == true ? TmaxCommands.AddNotification : TmaxCommands.EndNotification, tmaxItem)) != null)
					bSuccessful = Args.Successful;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SetAppNotification", Ex);
			}
			
			return bSuccessful;
			
		}// protected virtual bool SetAppNotifications(bool bNotify)

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
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get	{ return m_tmaxEventSource; }
		}
		
		/// <summary>The ID of the pane that should be used for command events</summary>
		public int PaneId
		{
			get { return m_iPaneId; }
			set { m_iPaneId = value; }
		}
		
		#endregion Properties
		
	}// public class CTmaxBaseCtrl : System.Windows.Forms.UserControl

}// namespace FTI.Trialmax.Controls
