using System;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Win32;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class translates keystrokes to TrialMax application hotkeys</summary>
	public class CTmaxKeyboard : System.Windows.Forms.IMessageFilter
	{
		#region Private Members
		
		private FTI.Shared.Trialmax.CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>This is the delegate used to handle hotkey events fired by the filter</summary>
		/// <param name="objSender">Object firing the event</param>
		/// <param name="Args">Object containing the event arguments</param>
		public delegate void TmaxHotkeyHandler(object objSender, TmaxHotkeys eHotkey, ref bool rProcessed);
		
		/// <summary>This is the delegate used to handle keydown events fired by the filter</summary>
		/// <param name="objSender">Object firing the event</param>
		/// <param name="Args">Object containing the event arguments</param>
		public delegate void TmaxKeyDownHandler(object objSender, Keys eKey, Keys eModifiers, ref bool rProcessed);
		
		/// <summary>This event is fired when an application hotkey is pressed</summary>
		public event TmaxHotkeyHandler TmaxHotkey;
		
		/// <summary>This event is fired when an application hotkey is pressed</summary>
		public event TmaxKeyDownHandler TmaxKeyDown;
		
		/// <summary>Constructor</summary>
		public CTmaxKeyboard()
		{
			m_tmaxEventSource.Name = "Keyboard Filter Events";
		}
		
		/// <summary>Called by the application to preprocess a message</summary>
		/// <param name="m">The message to be preprocessed</param>
		/// <returns>true to prevent this message from being dispatched</returns>
		public bool PreFilterMessage(ref Message m) 
		{
			if(m.Msg == User.WM_KEYDOWN)
			{
				try
				{
					return OnKeyDown((System.Windows.Forms.Keys)((int)m.WParam));
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireDiagnostic(this, "PreFilterMessage", ("Exception: " + Ex.ToString()));
					return false;
				}
			
			}
			else
			{
				//	Perform default processing
				return false;
			}
			
		}// public bool PreFilterMessage(ref Message m)
	
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>Handles all WM_KEYDOWN messages trapped by the filter</summary>
		/// <param name="iKeyCode">The system keycode identifier</param>
		/// <returns>true if processed</returns>
		private bool OnKeyDown(System.Windows.Forms.Keys eKey)
		{
			TmaxHotkeys eHotkey = TmaxHotkeys.None;
			bool bProcessed = false;

			//	Are no special modifier keys (Ctrl/Shift/Alt) being pressed?
			if(Control.ModifierKeys == Keys.None)
			{
				switch(eKey)
				{
					case Keys.F1:	eHotkey = TmaxHotkeys.OpenHelp;
						break;
									
					case Keys.F2:	eHotkey = TmaxHotkeys.CaseOptions;
						break;
									
					case Keys.F3:	eHotkey = TmaxHotkeys.FindNext;
						break;
									
					case Keys.F4:	eHotkey = TmaxHotkeys.ViewProperties;
						break;
									
					case Keys.F5:	eHotkey = TmaxHotkeys.OpenPresentation;
						break;
									
					case Keys.F6:	eHotkey = TmaxHotkeys.ViewBuilder;
						break;
									
					case Keys.F7:	eHotkey = TmaxHotkeys.ViewTuner;
						break;
									
					case Keys.F8:	eHotkey = TmaxHotkeys.ViewMediaViewer;
						break;
									
					case Keys.F9:	eHotkey = TmaxHotkeys.ReloadCase;
						break;
									
					case Keys.F10:	eHotkey = TmaxHotkeys.ViewCodes;
						break;
									
					case Keys.F11:	eHotkey = TmaxHotkeys.ScreenCapture;
						break;
									
					case Keys.F12:	eHotkey = TmaxHotkeys.SetFilter;
						break;
									
				}// switch(eKey);
			
			}
			
				//	Is the user pressing only the control key?
			else if(Control.ModifierKeys == Keys.Control)
			{
				switch(eKey)
				{
					case Keys.B:	eHotkey = TmaxHotkeys.AddToBinder;
						break;

					case Keys.Insert:
					case Keys.C:	eHotkey = TmaxHotkeys.Copy;
						break;

					case Keys.F:	eHotkey = TmaxHotkeys.Find;
						break;
									
					case Keys.G:	eHotkey = TmaxHotkeys.GoTo;
						break;

					case Keys.J:	eHotkey = TmaxHotkeys.AddObjection;
						break;

					case Keys.L:	eHotkey = TmaxHotkeys.OpenLast;
						break;
									
					case Keys.N:	eHotkey = TmaxHotkeys.FileNew;
						break;
									
					case Keys.O:	eHotkey = TmaxHotkeys.FileOpen;
						break;
									
					case Keys.P:	eHotkey = TmaxHotkeys.Print;
						break;
									
					case Keys.S:	eHotkey = TmaxHotkeys.AddToScript;
						break;
									
					case Keys.T:	eHotkey = TmaxHotkeys.RefreshTreatments;
						break;

					case Keys.V:	eHotkey = TmaxHotkeys.Paste;
						break;

					case Keys.X:	eHotkey = TmaxHotkeys.GoToBarcode;
						break;
									
					case Keys.F5:	eHotkey = TmaxHotkeys.BlankPresentation;
						break;
									
					case Keys.F12:	eHotkey = TmaxHotkeys.FastFilter;
						break;

					case Keys.Subtract:
					case Keys.OemMinus: eHotkey = TmaxHotkeys.RepeatObjection;
						break;

				}// switch(eKey);
			
			}
				
			//	Is the user pressing only the shift key?
			else if(Control.ModifierKeys == Keys.Shift)
			{
				switch(eKey)
				{
					case Keys.Insert:	eHotkey = TmaxHotkeys.Paste;
						break;

					case Keys.Delete: eHotkey = TmaxHotkeys.Delete;
						break;
					
				}// switch(eKey);
			
			}
				
			//	Is the user pressing control and shift?
			else if(((Control.ModifierKeys & Keys.Shift) == Keys.Shift) &&
					((Control.ModifierKeys & Keys.Control) == Keys.Control))
			{
				switch(eKey)
				{
					case Keys.S:	eHotkey = TmaxHotkeys.Save;
						break;
				}// switch(eKey);
			
			}
				
			//	Did the keystroke translate to an application hotkey?
			if(eHotkey != TmaxHotkeys.None)
			{
				try
				{
					//	Fire a hotkey event to notify the application
					if(TmaxHotkey != null)
						TmaxHotkey(this, eHotkey, ref bProcessed);
				}
				catch(System.Exception Ex)
				{
					m_tmaxEventSource.FireDiagnostic(this, "OnKeyDown", "Exception: " + Ex.ToString());
				}
					
				return bProcessed;
			}
			else
			{
				//	Is the keydown event being trapped
				if(TmaxKeyDown != null)
				{
					TmaxKeyDown(this, eKey, Control.ModifierKeys, ref bProcessed);

					return bProcessed;
				}
				else
				{
					return false;
				}
			
			}
			
		}
		 
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Event source for TrialMax error and diagnostic events</summary>
		public FTI.Shared.Trialmax.CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
			
		}
		
		#endregion Properties
	
	}// public class CTmaxKeyboard : System.Windows.Forms.IMessageFilter

}// namespace FTI.Shared.Trialmax
