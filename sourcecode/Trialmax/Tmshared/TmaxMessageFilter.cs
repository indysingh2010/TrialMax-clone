using System;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class implements the filter for custom windows messages sent to the application</summary>
	public class CTmaxAsyncFilter : System.Windows.Forms.IMessageFilter
	{
		#region Public Methods
		
		/// <summary>This is the delegate used to events fired by the filter when it traps a message</summary>
		/// <param name="objSender">Object firing the event</param>
		/// <param name="eTmaxWindowMessage">Enumerated custom message identifier</param>
		/// <param name="rMsg">The system message structure</param>
		/// <returns>true if handled</returns>
		public delegate bool TmaxAsyncHandler(object objSender, TmaxWindowMessages eTmaxWindowMessage, ref Message rMsg);
		
		/// <summary>This event is fired when an application hotkey is pressed</summary>
		public event TmaxAsyncHandler TmaxAsyncMessage;
		
		/// <summary>Constructor</summary>
		public CTmaxAsyncFilter()
		{
		}
	
		/// <summary>This function is called by the system to allow the filter to trap the custom message</summary>
		/// <param name="wndMessage">The window message</param>
		/// <returns>true if processed</returns>
		public bool PreFilterMessage(ref Message wndMessage)
		{
			TmaxWindowMessages eMessage = TmaxWindowMessages.Invalid;
			
			try
			{
				if((eMessage = GetAppMessage(ref wndMessage)) != TmaxWindowMessages.Invalid)
				{
					if(TmaxAsyncMessage != null)
					{
						return TmaxAsyncMessage(this, eMessage, ref wndMessage);
					}
					
				}
				
			}
			catch
			{
			}
			
			//	Must not have been processed
			return false;
		
		}// public bool PreFilterMessage(ref Message wndMessage)
		
		/// <summary>This method retrieves the TrialMax custom message identifier associated with the specified message</summary>
		/// <param name="rMsg">The system window message</param>
		/// <returns>The TrialMax custom message identifier</returns>
		public TmaxWindowMessages GetAppMessage(ref Message rMsg)
		{
			TmaxWindowMessages eMessage = TmaxWindowMessages.Invalid;
			
			try
			{
				if((rMsg.Msg > ((int)TmaxWindowMessages.Invalid)) && (rMsg.Msg < ((int)TmaxWindowMessages.Max)))
				{
					eMessage = (TmaxWindowMessages)(rMsg.Msg);
				}
			}
			catch
			{
			}
			
			return eMessage;
		
		}// public TmaxAppMessages GetAppMessage(ref Message rMsg)
		
		#endregion Public Methods
		
		#region Properties
		
		
		#endregion Properties
		
	}// public class CTmaxAsyncFilter : System.Windows.Forms.IMessageFilter
		
}// namespace FTI.Shared.Trialmax
