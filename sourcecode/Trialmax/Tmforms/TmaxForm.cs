using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Win32;

namespace FTI.Trialmax.Forms
{
	/// <summary>This class manages a form that allows the user to add new movie clips</summary>
	public class CFTmaxBaseForm : System.Windows.Forms.Form, ITmaxAppNotification
	{
		/// <summary>This event is fired by a form to issue a command</summary>
		public event FTI.Shared.Trialmax.TmaxCommandHandler TmaxCommandEvent;
		
		#region Constants
		
		/// <summary>Error message identifiers</summary>
		protected const int ERROR_TMAX_FORM_FIRE_COMMAND_EX		= 0;
		protected const int ERROR_TMAX_FORM_RESERVED_2			= 1;
		protected const int ERROR_TMAX_FORM_RESERVED_3			= 2;
		protected const int ERROR_TMAX_FORM_RESERVED_4			= 3;
		protected const int ERROR_TMAX_FORM_RESERVED_5			= 4;
		protected const int ERROR_TMAX_FORM_RESERVED_6			= 5;
		protected const int ERROR_TMAX_FORM_RESERVED_7			= 6;
		protected const int ERROR_TMAX_FORM_RESERVED_8			= 7;
		
		// Derived classes should start their error numbering with this value
		protected const int ERROR_TMAX_FORM_MAX					= 7;
		
		#endregion Constants
		
		#region Protected Members
		
		/// <summary>Local member bounded to EventSource property</summary>
		protected FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member used to construct error messages</summary>
		protected FTI.Shared.Trialmax.CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();
		
		/// <summary>Local member bound to PaneId property</summary>
		protected int m_iPaneId = 0;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CFTmaxBaseForm() : base()
		{
			//	Populate the error builder collection
			SetErrorStrings();
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
			
		}// public virtual void OnReordered(FTI.Shared.Trialmax.CTmaxItem tmaxItem)

		/// <summary>This method is called by the application when it adds new objections to the database</summary>
		/// <param name="tmaxParent">TrialMax event item that identifies the parent record</param>
		/// <param name="tmaxChildren">TrialMax event item collection of new child records</param>
		public virtual void OnObjectionsAdded(CTmaxItem tmaxParent, CTmaxItems tmaxChildren)
		{
		}

		/// <summary>This method is called by the application to when objections get deleted</summary>
		/// <param name="tmaxItem">The item that has been deleted</param>
		public virtual void OnObjectionsDeleted(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
		}

		/// <summary>This method is called by the application when objections have been updated in an operation</summary>
		/// <param name="tmaxItems">The items that have been updated</param>
		public virtual void OnObjectionsUpdated(FTI.Shared.Trialmax.CTmaxItems tmaxItems)
		{
		}

		#endregion Public Methods
		
		#region Protected Methods

		/// <summary>Clean up any resources being used</summary>
		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(m_tmaxErrorBuilder != null)
				{
					if(m_tmaxErrorBuilder.FormatStrings != null)
						m_tmaxErrorBuilder.FormatStrings.Clear();
					m_tmaxErrorBuilder = null;
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>This method is called to populate the error builder's format string collection</summary>
		protected virtual void SetErrorStrings()
		{
			if(m_tmaxErrorBuilder == null) return;
			if(m_tmaxErrorBuilder.FormatStrings == null) return;
			
			m_tmaxErrorBuilder.FormatStrings.Add("An exception was raised while attempting to fire the command event: command = %1");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CFTmaxBaseForm error string 2");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CFTmaxBaseForm error string 3");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CFTmaxBaseForm error string 4");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CFTmaxBaseForm error string 5");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CFTmaxBaseForm error string 6");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CFTmaxBaseForm error string 7");
			m_tmaxErrorBuilder.FormatStrings.Add("Reserved CFTmaxBaseForm error string 8");

		}// protected override void SetErrorStrings()

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
		
		}// private void Warn(string strMsg, System.Windows.Forms.TextBox ctrlSelect)
		
		/// <summary>This method is called to display a warning</summary>
		/// <param name="strWarning">The warning text</param>
		/// <return>false always</return>
		protected virtual bool Warn(string strWarning)
		{
			return Warn(strWarning, null);
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
				m_tmaxEventSource.FireError(this, "FireCommand", m_tmaxErrorBuilder.Message(ERROR_TMAX_FORM_FIRE_COMMAND_EX, Args.Command), Ex);
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

		/// <summary>This method is called to get the window handle of the specified tooltip control</summary>
		/// <param name="ctrlToolTip">The desired tooltip control</param>
		/// <returns>true ifssss successful</returns>
		protected virtual System.IntPtr GetHandle(System.Windows.Forms.ToolTip ctrlToolTip)
		{
			try
			{
				FieldInfo fieldInfo = ctrlToolTip.GetType().GetField("window", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance );
				NativeWindow window  = (NativeWindow)fieldInfo.GetValue ( ctrlToolTip );
				return window.Handle;
			}
			catch
			{
				return IntPtr.Zero;
			}
		
		}// protected virtual System.IntPtr GetHandle(System.Windows.Forms.ToolTip ctrlToolTip)
		
		/// <summary>The method sets the specified tool tip control to balloon style hints</summary>
		/// <param name="ctrlToolTip">The desired tool tip control</param>
		protected virtual void SetBalloonStyle(ToolTip ctrlToolTip)
		{
			System.IntPtr	hwnd = IntPtr.Zero;
			long			WS_POPUP = 0x80000000;
			long			TTS_BALLOON = 0x40;
			long			TTS_NOFADE = 0x20;
			int				GWL_STYLE = -16;
			
			try
			{
				if((hwnd = GetHandle(ctrlToolTip)) != IntPtr.Zero)
				{
					User.SetWindowLong(hwnd, GWL_STYLE , (int)(WS_POPUP | TTS_BALLOON | TTS_NOFADE));
				}
			
			}
			catch
			{
			}
		
		}// protected virtual void SetBalloonStyle(ToolTip ctrlToolTip)
	
		/// <summary>The method sets the specified tool tip background color</summary>
		/// <param name="ctrlToolTip">The desired tool tip control</param>
		/// <param name="color">The desired color</param>
		protected virtual void SetBackColor(ToolTip ctrlToolTip, System.Drawing.Color color)
		{
			System.IntPtr	hwnd = IntPtr.Zero;
			int				TTM_SETTIPBKCOLOR = (FTI.Shared.Win32.User.WM_USER + 19);
			int				iBackColor = 0;

			try
			{
				if((hwnd = GetHandle(ctrlToolTip)) != IntPtr.Zero)
				{
					iBackColor = ColorTranslator.ToWin32(color);

					User.SendMessage(hwnd, TTM_SETTIPBKCOLOR, iBackColor, IntPtr.Zero);  
				}
			
			}
			catch
			{
			}
		
		}// protected virtual void SetBalloonStyle(ToolTip ctrlToolTip)
	
		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }

		}// EventSource property
		
		/// <summary>The ID of the pane that owns the popup form</summary>
		public int PaneId
		{
			get { return m_iPaneId; }
			set { m_iPaneId = value; }

		}// PaneId property
		
		#endregion Properties
		
	}// public class CFTmaxBaseForm : System.Windows.Forms.Form

}// namespace FTI.Trialmax.Panes
