using System;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class is used to add error and diagnostic event sourcing to a class</summary>
	public class CTmaxEventSource
	{
		#region Protected Members
		
		/// <summary>Local member associated with the Name property</summary>
		protected string m_strName = "Event Source";
		
		/// <summary>Local member to keep track of start time for an Elapsed diagnostic event</summary>
		protected System.DateTime m_dtStartElapsed = System.DateTime.Now;
		
		/// <summary>Local member bound to LastErrorArgs</summary>
		protected CTmaxErrorArgs m_lastErrorArgs = null;
		
		#endregion Protected Members
		
		#region Public Methods
		
		/// <summary>This event is fired to report an error</summary>
		public event FTI.Shared.Trialmax.ErrorEventHandler ErrorEvent;
		
		/// <summary>This event is fired to send diagnostic information</summary>
		public event FTI.Shared.Trialmax.DiagnosticEventHandler DiagnosticEvent;
		
		/// <summary>This event is fired to send MouseDown notifications</summary>
		public event System.Windows.Forms.MouseEventHandler MouseDownEvent;
		
		/// <summary>This event is fired to send MouseUp notifications</summary>
		public event System.Windows.Forms.MouseEventHandler MouseUpEvent;
		
		/// <summary>This event is fired to send MouseMove notifications</summary>
		public event System.Windows.Forms.MouseEventHandler MouseMoveEvent;
		
		/// <summary>This event is fired to send MouseDblClick notifications</summary>
		public event System.Windows.Forms.MouseEventHandler MouseDblClickEvent;
		
		public CTmaxEventSource()
		{
		}
		
		/// <summary>This method attaches to the caller's event source to allow for the bubbling of events</summary>
		/// <param name="eventSource">The event source to attach to</param>
		/// <returns>true if successful</returns>
		public bool Attach(CTmaxEventSource eventSource)
		{
			bool bSuccessful = false;
			
			Debug.Assert(eventSource != null);
			
			if(eventSource != null)
			{
				try
				{
					eventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.OnDiagnostic);
					eventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);
					bSuccessful = true;
				}
				catch
				{
				}
				
			}
			
			return bSuccessful;
		
		}// public bool Attach(CTmaxEventSource eventSource)
		
		/// <summary>This method detaches this event source from the caller's source</summary>
		/// <param name="eventSource">The event source to detach from</param>
		public void Detach(CTmaxEventSource eventSource)
		{
			
			Debug.Assert(eventSource != null);
			
			if(eventSource != null)
			{
				try
				{
					eventSource.DiagnosticEvent -= new FTI.Shared.Trialmax.DiagnosticEventHandler(this.OnDiagnostic);
					eventSource.ErrorEvent -= new FTI.Shared.Trialmax.ErrorEventHandler(this.OnError);
				}
				catch
				{
				}
				
			}
		
		}// public void Detach(CTmaxEventSource eventSource)
		
		/// <summary>This method is used to daisy chain error events</summary>
		/// <param name="objSender">The object sending the event</param>
		/// <param name="Args">Error event arguments</param>
		public virtual void OnError(object O, CTmaxErrorArgs Args)
		{
			//	Propagate the event
			FireError(O, Args);
		}
			
		/// <summary>This method is called to report an error</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="strMethod">The calling method that encountered the error</param>
		/// <param name="strMessage">The error message</param>
		/// <param name="aItems">The list of items associated with the message</param>
		/// <param name="strException">The exception message</param>
		/// <param name="strDetails">The error details</param>
		///	<param name="bShow">true to display the error message in the application</param>
		public virtual void FireError(object O, string strMethod, string strMessage, CTmaxErrorArgs.CErrorItems aItems, string strException, string strDetails, bool bShow)
		{
			string		strSource = "";
			string		strTitle = "";
			CTmaxErrorArgs	Args = null;
			
			try
			{
				//	Build the source descriptor
				strSource = (O.GetType().FullName + "::" + strMethod);
				
				//	Set the default title
				if((m_strName != null) && (m_strName.Length > 0))
					strTitle = m_strName + " Error";
				else
					strTitle = "Error";
			
				if((Args = new CTmaxErrorArgs()) != null)
				{
					Args.Initialize(strTitle, strSource, strMessage, aItems, strException, strDetails);
					Args.Show = bShow;
					
					//	Fire the error
					FireError(O, Args);
				}
			
			}
			catch
			{
			}
			

		}

		/// <summary>This method is called to report an error</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="strMethod">The calling method that encountered the error</param>
		/// <param name="strMessage">The error message</param>
		/// <param name="aItems">The list of items associated with the message</param>
		/// <param name="strException">The exception message</param>
		/// <param name="strDetails">The error details</param>
		public virtual void FireError(object O, string strMethod, string strMessage, CTmaxErrorArgs.CErrorItems aItems, string strException, string strDetails)
		{
			FireError(O, strMethod, strMessage, aItems, strException, strDetails, true);
		}

		/// <summary>This method is called to report an error</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="strMethod">The calling method that encountered the error</param>
		/// <param name="strMessage">The error message</param>
		///	<param name="bShow">true to display the error message in the application</param>
		public virtual void FireError(object O, string strMethod, string strMessage, bool bShow)
		{
			FireError(O, strMethod, strMessage, null, null, null, bShow);
		}

		/// <summary>This method is called to report an error</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="strMethod">The calling method that encountered the error</param>
		/// <param name="strMessage">The error message</param>
		public virtual void FireError(object O, string strMethod, string strMessage)
		{
			FireError(O, strMethod, strMessage, null, null, null, true);
		}

		/// <summary>This method is called to report an error</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="strMethod">The calling method that encountered the error</param>
		/// <param name="strMessage">The error message</param>
		/// <param name="Ex">The system exception associated with the error</param>
		/// <param name="aItems">The collection of items assocaited with the error</param>
		///	<param name="bShow">true to display the error message in the application</param>
		public virtual void FireError(object O, string strMethod, string strMessage, System.Exception Ex, CTmaxErrorArgs.CErrorItems aItems, bool bShow)
		{
			FireError(O, strMethod, strMessage, aItems, Ex.Message, Ex.ToString(), bShow);
		}

		/// <summary>This method is called to report an error</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="strMethod">The calling method that encountered the error</param>
		/// <param name="strMessage">The error message</param>
		/// <param name="Ex">The system exception associated with the error</param>
		/// <param name="aItems">The collection of items assocaited with the error</param>
		public virtual void FireError(object O, string strMethod, string strMessage, System.Exception Ex, CTmaxErrorArgs.CErrorItems aItems)
		{
			FireError(O, strMethod, strMessage, aItems, Ex.Message, Ex.ToString(), true);
		}

		/// <summary>This method is called to report an error</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="strMethod">The calling method that encountered the error</param>
		/// <param name="strMessage">The error message</param>
		/// <param name="Ex">The system exception associated with the error</param>
		///	<param name="bShow">true to display the error message in the application</param>
		public virtual void FireError(object O, string strMethod, string strMessage, System.Exception Ex, bool bShow)
		{
			FireError(O, strMethod, strMessage, null, Ex.Message, Ex.ToString(), bShow);
		}

		/// <summary>This method is called to report an error</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="strMethod">The calling method that encountered the error</param>
		/// <param name="strMessage">The error message</param>
		/// <param name="Ex">The system exception associated with the error</param>
		public virtual void FireError(object O, string strMethod, string strMessage, System.Exception Ex)
		{
			FireError(O, strMethod, strMessage, null, Ex.Message, Ex.ToString(), true);
		}

		/// <summary>This method is called to fire an error event</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="Args">Argument object to be passed in the event</param>
		public virtual void FireError(object O, CTmaxErrorArgs Args)
		{
			if(Args != null)
			{
				m_lastErrorArgs = Args;
				
				if(ErrorEvent != null)
					ErrorEvent(O, Args);
			}
		
		}
		
		/// <summary>This method is used to daisy chain diagnostic events</summary>
		/// <param name="objSender">The object sending the event</param>
		/// <param name="Args">Error event arguments</param>
		public virtual void OnDiagnostic(object O, CTmaxDiagnosticArgs Args)
		{
			//	Propagate the event
			FireDiagnostic(O, Args);
		}
			
		/// <summary>This method is called to fire a diagnostic event</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="Args">Argument object to be passed in the event</param>
		public virtual void FireDiagnostic(object O, CTmaxDiagnosticArgs Args)
		{
			//	Is anybody registered?
			if((Args != null) && (DiagnosticEvent != null))
			{
				DiagnosticEvent(O, Args);
			}
		}
		
		/// <summary>This method is called to fire a diagnostic event</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="strMethod">The calling method</param>
		/// <param name="strMessage">The diagnostic message</param>
		/// <param name="aItems">The collection of items assocaited with the event</param>
		/// <param name="Ex">The exception associated with the event</param>
		public virtual void FireDiagnostic(object O, string strMethod, string strMessage, CTmaxDiagnosticArgs.CDiagnosticItems aItems, System.Exception Ex)
		{
			string			strSource;
			CTmaxDiagnosticArgs	Args;
			
			try
			{
				if((Args = new CTmaxDiagnosticArgs()) != null)
				{
					//	Construct the source path
					strSource= (O.GetType().FullName + "::" + strMethod);
					
					//	Initialize the argument object
					Args.Initialize(strSource, strMessage, m_strName, aItems, Ex);
					
					//	Fire the event
					FireDiagnostic(O, Args);
				}
			}
			catch
			{
			}

		}
		
		/// <summary>This method is called to fire a diagnostic event</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="strMethod">The calling method</param>
		/// <param name="strMessage">The diagnostic message</param>
		/// <param name="Ex">The exception associated with the event</param>
		public virtual void FireDiagnostic(object O, string strMethod, string strMessage, System.Exception Ex)
		{
			FireDiagnostic(O, strMethod, strMessage, null, Ex);
		}
		
		/// <summary>This method is called to fire a diagnostic event</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="strMethod">The calling method</param>
		/// <param name="Ex">The exception associated with the event</param>
		public virtual void FireDiagnostic(object O, string strMethod, System.Exception Ex)
		{
			FireDiagnostic(O, strMethod, "EXCEPTION", null, Ex);
		}
		
		/// <summary>This method is called to fire a diagnostic event</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="strMethod">The calling method</param>
		/// <param name="strMessage">The diagnostic message</param>
		/// <param name="aItems">The collection of items assocaited with the event</param>
		public virtual void FireDiagnostic(object O, string strMethod, string strMessage, CTmaxDiagnosticArgs.CDiagnosticItems aItems)
		{
			FireDiagnostic(O, strMethod, strMessage, aItems, null);
		}
		
		/// <summary>This method is called to fire a diagnostic event</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="strMethod">The calling method</param>
		/// <param name="strMessage">The diagnostic message</param>
		public virtual void FireDiagnostic(object O, string strMethod, string strMessage)
		{
			FireDiagnostic(O, strMethod, strMessage, ((CTmaxDiagnosticArgs.CDiagnosticItems)(null)));
		}
		
		/// <summary>This method is called to initialize the object to fire an Elapsed time diagnostic</summary>
		public virtual void InitElapsed()
		{				
			m_dtStartElapsed = System.DateTime.Now;
		}
		
		/// <summary>This method is called to fire an elapsed time diagnostic event</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="strMethod">The calling method</param>
		/// <param name="strMessage">The diagnostic message</param>
		public virtual void FireElapsed(object O, string strMethod, string strMessage)
		{				
			TimeSpan tsElapsed = System.DateTime.Now.Subtract(m_dtStartElapsed);
			FireDiagnostic(this, strMethod, strMessage + ": " + tsElapsed.ToString());
		}
		
		/// <summary>This method is called to fire an elapsed time diagnostic event</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="strMethod">The calling method</param>
		public virtual void FireElapsed(object O, string strMethod)
		{				
			FireElapsed(this, strMethod, "Time to " + strMethod);
		}
		
		/// <summary>This method is called to handle MouseDblClick events</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="Args">Argument object to be passed in the event</param>
		///	<remarks>This method is used to bubble the events to higher levels</remarks>
		public virtual void OnMouseDblClick(object O, System.Windows.Forms.MouseEventArgs Args)
		{
			//	Propagate the event
			FireMouseDblClick(O, Args);
		}
		
		/// <summary>This method is called to fire a MouseDblClick event</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="Args">Argument object to be passed in the event</param>
		public virtual void FireMouseDblClick(object O, System.Windows.Forms.MouseEventArgs Args)
		{
			//	Is anybody registered?
			if((Args != null) && (MouseDblClickEvent != null))
			{
				MouseDblClickEvent(O, Args);
			}
		}
		
		/// <summary>This method is called to fire a MouseDblClick event</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="Button">Mouse button identifier</param>
		///	<param name="iX">X coordinate of mouse position</param>
		///	<param name="iY">Y coordinate of mouse position</param>
		///	<param name="iClicks">Number of clicks</param>
		public virtual void FireMouseDblClick(object O, System.Windows.Forms.MouseButtons Button, int iX, int iY, int iClicks)
		{
			System.Windows.Forms.MouseEventArgs Args = new MouseEventArgs(Button, iClicks, iX, iY, 0);
			FireMouseDblClick(O, Args);
		}
		
		/// <summary>This method is called to fire a MouseDblClick event</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="Button">Mouse button identifier</param>
		///	<param name="iX">X coordinate of mouse position</param>
		///	<param name="iY">Y coordinate of mouse position</param>
		public virtual void FireMouseDblClick(object O, System.Windows.Forms.MouseButtons Button, int iX, int iY)
		{
			FireMouseDblClick(O, Button, iX, iY, 1);
		}
		
		/// <summary>This method is called to handle MouseDown events</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="Args">Argument object to be passed in the event</param>
		///	<remarks>This method is used to bubble the events to higher levels</remarks>
		public virtual void OnMouseDown(object O, System.Windows.Forms.MouseEventArgs Args)
		{
			//	Propagate the event
			FireMouseDown(O, Args);
		}
		
		/// <summary>This method is called to fire a MouseDown event</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="Args">Argument object to be passed in the event</param>
		public virtual void FireMouseDown(object O, System.Windows.Forms.MouseEventArgs Args)
		{
			//	Is anybody registered?
			if((Args != null) && (MouseDownEvent != null))
			{
				MouseDownEvent(O, Args);
			}
		}
		
		/// <summary>This method is called to fire a MouseDown event</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="Button">Mouse button identifier</param>
		///	<param name="iX">X coordinate of mouse position</param>
		///	<param name="iY">Y coordinate of mouse position</param>
		///	<param name="iClicks">Number of clicks</param>
		public virtual void FireMouseDown(object O, System.Windows.Forms.MouseButtons Button, int iX, int iY, int iClicks)
		{
			System.Windows.Forms.MouseEventArgs Args = new MouseEventArgs(Button, iClicks, iX, iY, 0);
			FireMouseDown(O, Args);
		}
		
		/// <summary>This method is called to fire a MouseDown event</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="Button">Mouse button identifier</param>
		///	<param name="iX">X coordinate of mouse position</param>
		///	<param name="iY">Y coordinate of mouse position</param>
		public virtual void FireMouseDown(object O, System.Windows.Forms.MouseButtons Button, int iX, int iY)
		{
			FireMouseDown(O, Button, iX, iY, 1);
		}
		
		/// <summary>This method is called to handle MouseUp events</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="Args">Argument object to be passed in the event</param>
		///	<remarks>This method is used to bubble the events to higher levels</remarks>
		public virtual void OnMouseUp(object O, System.Windows.Forms.MouseEventArgs Args)
		{
			//	Propagate the event
			FireMouseUp(O, Args);
		}
		
		/// <summary>This method is called to fire a MouseUp event</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="Args">Argument object to be passed in the event</param>
		public virtual void FireMouseUp(object O, System.Windows.Forms.MouseEventArgs Args)
		{
			//	Is anybody registered?
			if((Args != null) && (MouseUpEvent != null))
			{
				MouseUpEvent(O, Args);
			}
		}
		
		/// <summary>This method is called to fire a MouseUp event</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="Button">Mouse button identifier</param>
		///	<param name="iX">X coordinate of mouse position</param>
		///	<param name="iY">Y coordinate of mouse position</param>
		///	<param name="iClicks">Number of clicks</param>
		public virtual void FireMouseUp(object O, System.Windows.Forms.MouseButtons Button, int iX, int iY, int iClicks)
		{
			System.Windows.Forms.MouseEventArgs Args = new MouseEventArgs(Button, iClicks, iX, iY, 0);
			FireMouseUp(O, Args);
		}
		
		/// <summary>This method is called to fire a MouseUp event</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="Button">Mouse button identifier</param>
		///	<param name="iX">X coordinate of mouse position</param>
		///	<param name="iY">Y coordinate of mouse position</param>
		public virtual void FireMouseUp(object O, System.Windows.Forms.MouseButtons Button, int iX, int iY)
		{
			FireMouseUp(O, Button, iX, iY, 1);
		}
		
		/// <summary>This method is called to handle MouseMove events</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="Args">Argument object to be passed in the event</param>
		///	<remarks>This method is used to bubble the events to higher levels</remarks>
		public virtual void OnMouseMove(object O, System.Windows.Forms.MouseEventArgs Args)
		{
			//	Propagate the event
			FireMouseMove(O, Args);
		}
		
		/// <summary>This method is called to fire a MouseMove event</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="Args">Argument object to be passed in the event</param>
		public virtual void FireMouseMove(object O, System.Windows.Forms.MouseEventArgs Args)
		{
			//	Is anybody registered?
			if((Args != null) && (MouseMoveEvent != null))
			{
				MouseMoveEvent(O, Args);
			}
		}
		
		/// <summary>This method is called to fire a MouseMove event</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="Button">Mouse button identifier</param>
		///	<param name="iX">X coordinate of mouse position</param>
		///	<param name="iY">Y coordinate of mouse position</param>
		///	<param name="iClicks">Number of clicks</param>
		public virtual void FireMouseMove(object O, System.Windows.Forms.MouseButtons Button, int iX, int iY, int iClicks)
		{
			System.Windows.Forms.MouseEventArgs Args = new MouseEventArgs(Button, iClicks, iX, iY, 0);
			FireMouseMove(O, Args);
		}
		
		/// <summary>This method is called to fire a MouseMove event</summary>
		/// <param name="O">The object firing the event</param>
		/// <param name="Button">Mouse button identifier</param>
		///	<param name="iX">X coordinate of mouse position</param>
		///	<param name="iY">Y coordinate of mouse position</param>
		public virtual void FireMouseMove(object O, System.Windows.Forms.MouseButtons Button, int iX, int iY)
		{
			FireMouseMove(O, Button, iX, iY, 1);
		}
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Name used in reporting errors and diagnostics</summary>
		public string Name
		{
			get	{ return m_strName; }
			set { m_strName = value; }
		}
		
		/// <summary>Arguments associated with the last error event</summary>
		public CTmaxErrorArgs LastErrorArgs
		{
			get	{ return m_lastErrorArgs; }
			set { m_lastErrorArgs = value; }
		}
		
		/// <summary>Called to determine if Error event is hooked</summary>
		public bool ErrorHooked
		{
			get	{ return (ErrorEvent != null); }
		}
		
		/// <summary>Called to determine if Diagnostic event is hooked</summary>
		public bool DiagnosticHooked
		{
			get	{ return (DiagnosticEvent != null); }
		}
		
		#endregion Properties
		
	}// public class CTmaxEventSource

}// namespace FTI.Shared
