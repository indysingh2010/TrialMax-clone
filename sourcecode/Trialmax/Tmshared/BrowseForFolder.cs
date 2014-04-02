using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;

using FTI.Shared;
using FTI.Shared.Win32;

namespace FTI.Shared
{
	/// <summary>This class wraps the Shell services for selecting folders</summary>
	public class CBrowseForFolder
	{
		#region Constants
		
		private const int MAX_PATH = 260;
		private const int WM_SETTEXT = 0xC;
		
		/// <summary>Enum of CSIDLs identifying standard shell folders</summary>
		public enum FolderID 
		{
			Desktop                   = 0x0000,	// <desktop>
			Programs				  = 0x0002,	// Start Menu\Programs
			Control					  = 0x0003,	// My Computer\Control Panel
			Printers                  = 0x0004,	// My Computer\Printers
			MyDocuments               = 0x0005,	// My Documents
			Favorites                 = 0x0006,	// <user name>\Favorites
			StartUp					  = 0x0007, // Start Menu\Programs\Startup
			Recent                    = 0x0008,	// <user name>\Recent
			SendTo                    = 0x0009,	// <user name>\Recent
			Recycle					  = 0x000a, // <desktop>\Recycle Bin
			StartMenu                 = 0x000b,	// <user name>\Start Menu
			MyComputer                = 0x0011,	// My Computer
			NetworkNeighborhood       = 0x0012, // Network Neighborhood (My Network Places)
			Templates                 = 0x0015,
			Windows                   = 0x0024,	// GetWindowsDirectory()
			System                    = 0x0025,	// GetSystemDirectory()
			MyPictures                = 0x0027,
			NetAndDialUpConnections   = 0x0031,
		}

		#endregion Constants
		
		#region Shell Imports
     
		// C# representation of the IMalloc interface
		[InterfaceType ( ComInterfaceType.InterfaceIsIUnknown ),
			Guid ( "00000002-0000-0000-C000-000000000046" )]
			public interface IMalloc
		{
			[PreserveSig] IntPtr Alloc ( [In] int cb );
			[PreserveSig] IntPtr Realloc ( [In] IntPtr pv, [In] int cb );
			[PreserveSig] void   Free ( [In] IntPtr pv );
			[PreserveSig] int    GetSize ( [In] IntPtr pv );
			[PreserveSig] int    DidAlloc ( IntPtr pv );
			[PreserveSig] void   HeapMinimize ( );
		}

		[DllImport("User32.DLL")]
		public static extern IntPtr GetActiveWindow ( );
		
		// Styles used in the BROWSEINFO.ulFlags field
		[Flags]    
		public enum BffStyles 
		{
			RestrictToFilesystem =	0x0001, // BIF_RETURNONLYFSDIRS
			RestrictToDomain =		0x0002, // BIF_DONTGOBELOWDOMAIN
			RestrictToSubfolders =	0x0008, // BIF_RETURNFSANCESTORS
			ShowTextBox =			0x0010, // BIF_EDITBOX
			ValidateSelection =		0x0020, // BIF_VALIDATE
			NewDialogStyle =		0x0040, // BIF_NEWDIALOGSTYLE
			BrowseForComputer =		0x1000, // BIF_BROWSEFORCOMPUTER
			BrowseForPrinter =		0x2000, // BIF_BROWSEFORPRINTER
			ShowFiles =				0x4000, // BIF_BROWSEINCLUDEFILES
			NoNewFolderButton =		0x0200, // BIF_NONEWFOLDERBUTTON
		}

		/// <summary>Messages sent by the browser</summary>
		public enum BfmFromBrowser
		{
			Initialized = 1,
			SelChanged = 2,
			ValidateFailedA = 3,	// lParam:szPath ret:1(cont),0(EndDialog)
			ValidateFailedB = 4,	// lParam:wzPath ret:1(cont),0(EndDialog)
		}

		/// <summary>Messages sent to the browser</summary>
		public enum BfmToBrowser
		{
			SetStatusTextA = (0x400 + 100),	//	(WM_USER + 100)
			EnableOK       = (0x400 + 101),	//	(WM_USER + 101)
			SetSelectionA  = (0x400 + 102),	//	(WM_USER + 102)
			SetSelectionW  = (0x400 + 103),	//	(WM_USER + 103)
			SetStatusTextW = (0x400 + 104),	//	(WM_USER + 104)
		}

		// Delegate type used in BROWSEINFO.lpfn field
		public delegate int BFFCALLBACK(IntPtr hwnd, uint uMsg, IntPtr lParam, IntPtr lpData);

		[StructLayout ( LayoutKind.Sequential, Pack=8)]
			public struct BROWSEINFO
		{
			public IntPtr       hwndOwner;
			public IntPtr       pidlRoot;
			public IntPtr       pszDisplayName;
			[MarshalAs ( UnmanagedType.LPTStr )]
			public string       lpszTitle;
			public int          ulFlags;
			[MarshalAs ( UnmanagedType.FunctionPtr )]
			public BFFCALLBACK  lpfn;
			public IntPtr       lParam;
			public int          iImage;
		}

		[DllImport ( "Shell32.DLL" )]
		public static extern int SHGetMalloc(out IMalloc ppMalloc);

		[DllImport ( "Shell32.DLL" )]
		public static extern int SHGetSpecialFolderLocation ( 
			IntPtr hwndOwner, int nFolder, out IntPtr ppidl );

		[DllImport ( "Shell32.DLL" )]
		public static extern int SHGetPathFromIDList ( 
			IntPtr pidl, StringBuilder Path);

		[DllImport ( "Shell32.DLL", CharSet=CharSet.Auto )]
		public static extern IntPtr SHBrowseForFolder ( ref BROWSEINFO bi );
		

		[DllImport("SHFolder", CharSet = CharSet.Auto)]
		public static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, int
			hToken, int dwFlags, [MarshalAsAttribute(UnmanagedType.LPTStr)] string
			pszPath);

		[DllImport("user32")] public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);

		#endregion Shell Imports

		#region Protected Members
		
		/// <summary>Local member to store user defined BFF sytle flags</summary>
		protected int m_iUserFlags = 0;
		
		/// <summary>Local member bound to Title property</summary>
		private string m_strPrompt = "Please select a TrialMax case folder:";

		/// <summary>Local member bound to Folder property</summary>
		protected string m_strFolder = "";	
		
		/// <summary>Local member bound to NoNewFolder property</summary>
		protected bool m_bNoNewFolder = true;	
		
		/// <summary>Local member bound to RootFolder property</summary>
		protected FolderID m_eRootFolder = FolderID.Desktop;
		
		#endregion PrivateMembers
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CBrowseForFolder()
		{
			SetUserFlag((int)BffStyles.RestrictToFilesystem, true);
			SetUserFlag((int)BffStyles.RestrictToDomain, false);
			SetUserFlag((int)BffStyles.RestrictToSubfolders, false);
			SetUserFlag((int)BffStyles.ShowTextBox, false);
			SetUserFlag((int)BffStyles.ValidateSelection, false);
			SetUserFlag((int)BffStyles.NewDialogStyle, false);
			SetUserFlag((int)BffStyles.BrowseForComputer, false);
			SetUserFlag((int)BffStyles.BrowseForPrinter, false);
			SetUserFlag((int)BffStyles.ShowFiles, false);
			SetUserFlag((int)BffStyles.NoNewFolderButton, true);
		}
		
		/// <summary>This method is called to show the dialog for the user to select a new folder</summary>
		/// <param name="hWnd">Handle of the parent window</param>
		/// <returns>true if successful</returns>
		public DialogResult ShowDialog(System.IntPtr hWndOwner)
		{
			IMalloc	malloc = null;
			IntPtr	pidlRoot = IntPtr.Zero;
			IntPtr	pidlRet = IntPtr.Zero;

			//	Get the handle to the parent window
			if(hWndOwner == IntPtr.Zero) 
				hWndOwner = GetActiveWindow();

			// Get the item list for the root folder
			SHGetSpecialFolderLocation(hWndOwner, (int)m_eRootFolder, out pidlRoot);
			if(pidlRoot == IntPtr.Zero) 
				return DialogResult.Cancel;

			//	Should we display the New Folder button?
			//
			//	NOTE: On Win 2K (XP works fine) the only way to show the New Folder 
			//		  button is to use New Style dialog. However, there's
			//		  no way to turn it off if New Sytle is used. That's why
			//		  we have to set BOTH flags here
			if(m_bNoNewFolder == false)
			{
				SetUserFlag((int)BffStyles.NoNewFolderButton, false);
				SetUserFlag((int)BffStyles.NewDialogStyle, true);
				
				//	Required for new dialog style
				Application.OleRequired();
			}

			try 
			{
				// Construct a BROWSEINFO
				BROWSEINFO bi = new BROWSEINFO();
				IntPtr buffer = Marshal.AllocHGlobal(MAX_PATH);

				bi.pidlRoot = pidlRoot;
				bi.hwndOwner = hWndOwner;
				bi.pszDisplayName = buffer;
				bi.lpszTitle = m_strPrompt;
				bi.ulFlags = m_iUserFlags;
				
				//	Attach a callback if we want to set the initial folder
				if((m_strFolder != null) && (m_strFolder.Length > 0))
					bi.lpfn = new BFFCALLBACK(this.OnBrowserCallback);

				// And show the dialog
				pidlRet = SHBrowseForFolder(ref bi);

				// Free the buffer we've allocated on the global heap
				Marshal.FreeHGlobal(buffer);

				if(pidlRet == IntPtr.Zero) 
				{
					// User pressed Cancel
					return DialogResult.Cancel;
				}

				// Then retrieve the path from the IDList
				StringBuilder sb = new StringBuilder(MAX_PATH);
				if(0 == SHGetPathFromIDList(pidlRet, sb))
				{
					return DialogResult.Cancel;
				}

				// Convert to a string
				m_strFolder = sb.ToString( );
			
			}
			finally 
			{
				malloc = GetSHMalloc();
				malloc.Free(pidlRoot);

				if(pidlRet != IntPtr.Zero) 
				{
					malloc.Free(pidlRet);
				}
			}

			return DialogResult.OK;

		}// public bool Show(System.IntPtr hWnd)

		/// <summary>This method converts the specified item identifier list to a file path</summary>
		/// <param name="pidl">The desired item list</param>
		/// <returns>The associated file system path</returns>
		public string GetPathFromPidl(System.IntPtr pidl)
		{
			int				iSuccessful = 0;
			StringBuilder	sbPath = null;
			
			if(pidl == IntPtr.Zero) return String.Empty;
			
			try
			{
				sbPath = new StringBuilder(MAX_PATH + 1);
				
				if((iSuccessful = SHGetPathFromIDList(pidl, sbPath)) == 0)
					return String.Empty;
				else
					return sbPath.ToString();

			}
			catch
			{
				return String.Empty;
			}
			
		}// public string GetPathFromPidl(int pidl)

		/// <summary>This method converts the specified folder identifier to a file system path</summary>
		/// <param name="csidlFolder">One of the locally defined folder identifiers</param>
		/// <returns>The associated file system path</returns>
		public string GetPathFromCsidl(int csidlFolder)
		{
			string	strPath = null;
			
			try
			{
				strPath = new String(' ', 256);
			
				SHGetFolderPath(IntPtr.Zero, csidlFolder, 0, 0, strPath);
			
				strPath = strPath.Replace('\0', ' ');
			
				return strPath.Trim();
			}
			catch
			{
				return String.Empty;
			}
			
		}// public string GetPathFromCsidl(int csidlFolder)

		/// <summary>This method gets the path to the My Documents folder</summary>
		/// <returns>The My Documents path</returns>
		public string GetMyDocumentsPath()
		{
			return GetPathFromCsidl((int)FolderID.MyDocuments);
		}

		/// <summary>This method gets the path to the My Computer folder</summary>
		/// <returns>The My Computer path</returns>
		public string GetMyComputerPath()
		{
			return GetPathFromCsidl((int)FolderID.MyComputer);
		}

		/// <summary>This method gets the path to the Desktop folder</summary>
		/// <returns>The Desktop path</returns>
		public string GetDesktopPath()
		{
			return GetPathFromCsidl((int)FolderID.Desktop);
		}

		/// <summary>This method gets the path to the Network Neighborhood folder</summary>
		/// <returns>The Network Neighborhood path</returns>
		public string GetNetworkNeighborhoodPath()
		{
			return GetPathFromCsidl((int)FolderID.NetworkNeighborhood);
		}

		/// <summary>Callback supplied to folder browser when displayed</summary>
		/// <param name="hwnd">Handle to browser window</param>
		/// <param name="uMsg">Message being sent by the browser</param>
		/// <param name="lParam">Message specific parameter</param>
		/// <param name="lpData">Message specific data</param>
		/// <returns>Always returns zero</returns>
		public int OnBrowserCallback(IntPtr hwnd, uint uMsg, IntPtr lParam, IntPtr lpData)
		{
			//	Is the browser being intialized
			if(uMsg == (uint)BfmFromBrowser.Initialized)
			{
				//	Did the user specify a startup folder?
				if((m_strFolder != null) && (m_strFolder.Length > 0))
				{
					//	Set the initial selection
					SendMessage(hwnd, (int)BfmToBrowser.SetSelectionW, 1, Marshal.StringToBSTR(m_strFolder));
				}
				
				SendMessage(hwnd, (int)WM_SETTEXT, 0, Marshal.StringToHGlobalAnsi(" "));
				
			}
			return 0;
		
		}// public int OnBrowserCallback(IntPtr hwnd, uint uMsg, IntPtr lParam, IntPtr lpData)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>
		/// Helper function that returns the IMalloc interface used by the shell.
		/// </summary>
		protected static IMalloc GetSHMalloc()
		{
			IMalloc malloc;
			SHGetMalloc(out malloc);
			return malloc;
			
		}// protected static IMalloc GetSHMalloc()
		
		/// <summary>Helper function used to set / reset bits in the User Flags bitfield</summary>
		private void SetUserFlag(int iFlag, bool bEnabled)
		{
			if(bEnabled == true)
				m_iUserFlags |= iFlag;
			else
				m_iUserFlags &= ~iFlag;
		
		}// private void SetUserFlag(int iFlag, bool bEnabled)

		#endregion Protected Methods
		
		#region Properties
		
		//	Root folder from which searching is allowed
		public FolderID RootFolder
		{
			get { return m_eRootFolder; }
			set { m_eRootFolder = value; }
		}
		
		/// <summary>Initialize folder and folder selected by the user</summary>
		public string Folder
		{
			get { return m_strFolder; }
			set { m_strFolder = value; }
		}
		
		/// <summary>Prompt to be displayed in the dialog box</summary>
		public string Prompt
		{
			get { return m_strPrompt; }
			set { m_strPrompt = value; }
		}
		
		/// <summary>True to prevent the New Folder button from appearing</summary>
		public bool NoNewFolder
		{
			get { return m_bNoNewFolder; }
			set { m_bNoNewFolder = value; }
		}
		
		/// <summary>Restrict user selection to file system folders</summary>
		public bool OnlyFileSystem
		{
			get 
			{
				return ((m_iUserFlags & (int)BffStyles.RestrictToFilesystem) != 0);
			}
			set 
			{
				SetUserFlag((int)BffStyles.RestrictToFilesystem, value);
			}
		}

		/// <summary>Restrict user selection to file system subfolders</summary>
		public bool OnlySubfolders
		{
			get 
			{
				return ((m_iUserFlags & (int)BffStyles.RestrictToSubfolders) != 0);
			}
			set 
			{
				SetUserFlag((int)BffStyles.RestrictToSubfolders, value);
			}
		}

		/// <summary>Restrict user selection to computers</summary>
		public bool OnlyComputers
		{
			get 
			{
				return ((m_iUserFlags & (int)BffStyles.BrowseForComputer) != 0);
			}
			set 
			{
				SetUserFlag((int)BffStyles.BrowseForComputer, value);
			}
		}

		/// <summary>Restrict user selection to Printers</summary>
		public bool OnlyPrinters
		{
			get 
			{
				return ((m_iUserFlags & (int)BffStyles.BrowseForPrinter) != 0);
			}
			set 
			{
				SetUserFlag((int)BffStyles.BrowseForPrinter, value);
			}
		}

		/// <summary>Confine selections to current domain</summary>
		public bool RestrictToDomain
		{
			get 
			{
				return ((m_iUserFlags & (int)BffStyles.RestrictToDomain) != 0);
			}
			set 
			{
				SetUserFlag((int)BffStyles.RestrictToDomain, value);
			}
		}

		/// <summary>Show the text box for user to type in folder name</summary>
		public bool ShowTextBox
		{
			get 
			{
				return ((m_iUserFlags & (int)BffStyles.ShowTextBox) != 0);
			}
			set 
			{
				SetUserFlag((int)BffStyles.ShowTextBox, value);
			}
		}

		/// <summary>Show the text box for user to type in folder name</summary>
		public bool ShowFiles
		{
			get 
			{
				return ((m_iUserFlags & (int)BffStyles.ShowFiles) != 0);
			}
			set 
			{
				SetUserFlag((int)BffStyles.ShowFiles, value);
			}
		}

		/// <summary>Validate text entered by the user in the edit box</summary>
		public bool ValidateSelection
		{
			get 
			{
				return ((m_iUserFlags & (int)BffStyles.ValidateSelection) != 0);
			}
			set 
			{
				SetUserFlag((int)BffStyles.ValidateSelection, value);
			}
		}

		/// <summary>Prevent display of New Folder button</summary>
		public bool NoNewFolderButton
		{
			get 
			{
				return ((m_iUserFlags & (int)BffStyles.NoNewFolderButton) != 0);
			}
			set 
			{
				SetUserFlag((int)BffStyles.NoNewFolderButton, value);
			}
		}

		/// <summary>Use the new dialog style</summary>
		public bool NewDialogStyle
		{
			get 
			{
				return ((m_iUserFlags & (int)BffStyles.NewDialogStyle) != 0);
			}
			set 
			{
				SetUserFlag((int)BffStyles.NewDialogStyle, value);
			}
		}

		#endregion Properties
		
	}// public class CBrowseForFolder

}// namespace FTI.Shared
