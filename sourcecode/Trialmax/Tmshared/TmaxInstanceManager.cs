using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices; 

namespace FTI.Shared.Trialmax
{
	/// <summary>This class is used to manage application instances</summary>
	public class CTmaxInstanceManager 
	{
		#region Imports
		
		/// <summary>This delegate prototypes the callback for Win32 EnumWindows API call</summary>
		/// <param name="hwnd">The enumerated window handle</param>
		/// <param name="lParam">Caller provided data</param>
		public delegate bool EnumWindowsCallback(int hwnd, int lParam);
		
		[DllImport("user32")] 
		public static extern int EnumWindows(EnumWindowsCallback x, int y); 

		[DllImport("user32")] 
		public static extern int SetWindowLong(int hwnd, int iIndex, int iValue);
		
		[DllImport("user32")] 
		public static extern int GetWindowLong(int hwnd, int iIndex);
		
		[DllImport("user32")] 
		public static extern int SetForegroundWindow(int hwnd);
		
		[DllImport("user32")] 
		public static extern int BringWindowToTop(int hwnd);
		
		[DllImport("user32")] 
		public static extern int IsIconic(int hwnd);

		[DllImport("user32")] 
		public static extern int ShowWindowAsync(int hwnd, int iCmdShow);
		
		[DllImport("user32")] 
		public static extern int SendMessage(int hwnd, int wMsg, int wParam, IntPtr lParam);
		
		// API constants
		public const int GWL_USERDATA = (-21);
		public const int SW_RESTORE = 9;

		#endregion Imports
		
		#region Private Members
		
		public static int m_hwndPrevInstance = 0;
		
		public static TmaxApplications m_eAppId = TmaxApplications.TmaxManager;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxInstanceManager()
		{
		}
	
		/// <summary>This method will return the previous instance of the current application</summary>
		/// <returns>The process that represents the previous instance if found</returns>
		public static Process GetPrevInstance_old()
		{
			Process		current = Process.GetCurrentProcess(); 
			Process[]	processes = Process.GetProcessesByName(current.ProcessName); 
			
			//	NOTE:	We no longer use this method because using the .NET Process
			//			objects fails on some machines where the Performance Monitor
			//			Counters have been corrupted or disabled
			
			//	Loop through the running processes with the same name 
			foreach(Process O in processes) 
			{ 
				//	Ignore the current process 
				if(O.Id != current.Id) 
				{ 
					return O; 
				} 
 
			}
 
			//	No other instance was found, return null. 
			return null; 
		
		}// public static Process GetPrevInstance_old()
		
		/// <summary>This method is called to determine if a previous instance exists</summary>
		/// <param name="eAppId">The TrialMax application identifier</param>
		public static bool GetPrevInstance(TmaxApplications eAppId)
		{
			//	Which application are we searching for?
			m_eAppId = eAppId;
			
			//	Clear the current handle value
			m_hwndPrevInstance = 0;
			
			//	Locate and activate the main window of the previous instance
			EnumWindows(new EnumWindowsCallback(CTmaxInstanceManager.OnEnumWindow), 0); 

			//	Did we find the window?
			return (m_hwndPrevInstance > 0);
			
		}// public static bool GetPrevInstance(TmaxApplications eAppId)
		
		/// <summary>This method sets the key identifier used to locate the main window for a TrialMax instance</summary>
		/// <param name="hMainWnd">The main window of the active instance</param>
		/// <param name="eAppId">The TrialMax application identifier</param>
		public static void SetInstanceKey(int hMainWnd, TmaxApplications eAppId)
		{
			m_eAppId = eAppId;
			SetWindowLong(hMainWnd, GWL_USERDATA, GetInstanceKey());
		
		}// public static void SetInstanceKey(int hMainWnd, TmaxApplications eAppId)
		
		/// <summary>This method is called to activate the previous instance</summary>
		/// <param name="args">Array of command line arguments to be sent to the previous instance</param>
		/// <param name="eAppId">The TrialMax application identifier</param>
		public static void ActivatePrevInstance(string[] args, TmaxApplications eAppId)
		{
			//	Set the application identifier
			m_eAppId = eAppId;

			//	Find the main window
			if(GetPrevInstance(m_eAppId) == true)
			{
				//	Activate the application
				Activate(m_hwndPrevInstance);
				
				if((args != null) && (args.Length > 0))
					SendCommandLine(args);
			}
			
		}// public static void ActivatePrevInstance(string[] args, TmaxApplications eAppId)
		
		/// <summary>This method is called each time a window is enumerated</summary>
		/// <param name="hwnd">The enumerated window handle</param>
		/// <param name="lParam">Caller provided data</param>
		public static bool OnEnumWindow(int hwnd, int lParam) 
		{ 
			//	Is this the TrialMax main window ?
			if(GetWindowLong(hwnd, GWL_USERDATA) == GetInstanceKey())
			{
				//	Set the instance window handle
				m_hwndPrevInstance = hwnd;
				
				//	Stop here
				return false;
			}
			else
			{
				//	Keep going
				return true;
			} 
		
		}// public static bool OnEnumWindow(int hwnd, int lParam) 
		 
		/// <summary>This method is called to activate the specified window</summary>
		/// <param name="hwnd">The handle of the window to be activated</param>
		public static void Activate(int hwnd) 
		{ 
			//	Restore the application window if it's minimized
			if(IsIconic(hwnd) != 0)
				ShowWindowAsync(hwnd, SW_RESTORE);

			//	Bring the application window to the top and give it focus
			SetForegroundWindow(hwnd);
			BringWindowToTop(hwnd);
				
		}// public static void Activate(int hwnd) 
		 
		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>This method is called to activate the specified window</summary>
		/// <param name="hwnd">The handle of the window to be activated</param>
		private static int GetInstanceKey() 
		{ 
			if(m_eAppId == TmaxApplications.VideoViewer)
				return 0x12071941;
			else
				return 0x12131960;
				
		}// private static int GetInstanceKey() 
		 
		/// <summary>This method will send the command line arguments to the previous instance</summary>
		/// <param name="args">Array of command line arguments to be sent to the previous instance</param>
		private static void SendCommandLine(string[] args)
		{
			CTmaxCommandLine cmdLine = null;
			string			 strPath = "";
			
			Debug.Assert(m_hwndPrevInstance > 0);
			Debug.Assert(args != null);
			Debug.Assert(args.Length > 0);
				
			if(m_hwndPrevInstance <= 0) return;
			if(args == null) return;
			if(args.Length == 0) return;
			
			try
			{
				strPath = Process.GetCurrentProcess().MainModule.FileName;
				
				cmdLine = new CTmaxCommandLine(m_eAppId);
				cmdLine.Folder = System.IO.Path.GetDirectoryName(strPath);
				
				//	Set the property values using the command line arguments
				cmdLine.SetProperties(args);
				
				//	Save the command line to file
				if(cmdLine.Save() == true)
				{				
					//	Send the command line message to the previous instance
					SendMessage(m_hwndPrevInstance, (int)TmaxWindowMessages.InstanceCommandLine, 0, IntPtr.Zero);
				}

			}
			catch
			{
			}
			
		}// private static void SendCommandLine(string[] args)
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>The TrialMax application bound to the instance</summary>
		public TmaxApplications AppId
		{
			get { return m_eAppId; }
			set { m_eAppId = value; }
		}
		
		#endregion Properties
	
	}// public class CTmaxInstanceManager

}// namespace FTI.Trialmax.App
