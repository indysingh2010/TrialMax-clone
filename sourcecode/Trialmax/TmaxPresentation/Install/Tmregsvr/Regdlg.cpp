//==============================================================================
//
// File Name:	regdlg.cpp
//
// Description:	This file contains member functions of the CTmregsvrDlg class
//
// Functions:   CTmregsvrDlg::AddShortcut()
//				CTmregsvrDlg::CTmregsvrDlg()
//				CTmregsvrDlg::DeleteShortcut()
//				CTmregsvrDlg::DoDataExchange()
//				CTmregsvrDlg::FindFile()
//				CTmregsvrDlg::OnCtlColor()
//				CTmregsvrDlg::OnInitDialog()
//				CTmregsvrDlg::OnQueryDragIcon()
//				CTmregsvrDlg::OnTimer()
//				CTmregsvrDlg::Register()
//				CTmregsvrDlg::Register()
//
// See Also:	regdlg.h
//
// Copyright 1999, Forensic Technologies International
//
//==============================================================================
//	Date		Revision    Description
//	03-26-99	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmregsvr.h>
#include <regdlg.h>
#include <direct.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------
extern CTmregsvrApp theApp;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CTmregsvrDlg, CDialog)
	//{{AFX_MSG_MAP(CTmregsvrDlg)
	ON_WM_QUERYDRAGICON()
	ON_WM_CTLCOLOR()
	ON_WM_TIMER()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CTmregsvrDlg::AddShortcut()
//
// 	Description:	This function will add a shortcut for the specified file
//					to the system's startup menu
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTmregsvrDlg::AddShortcut()
{
	HKEY			hKey;
	DWORD			dwType = REG_SZ;
	char			szStartup[MAX_PATH];
	DWORD			dwSize = sizeof(szStartup);
	IShellLink*		pIShell;
	IPersistFile*	pIPersist;
	CString			strFile;
	CString			strLink;
	WORD			wszLink[MAX_PATH];
	
	//	Get the path to the startup group
	memset(szStartup, 0, sizeof(szStartup));
	if(RegOpenKey(HKEY_CURRENT_USER,
			   "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders",
			   &hKey) == ERROR_SUCCESS)
	{
		RegQueryValueEx(hKey, "Startup", 0, &dwType, (LPBYTE)szStartup,
						&dwSize);
	}
	
	//	Initialize OLE access
	if(FAILED(CoInitialize(NULL)))
		return;

	//	Get the shell interface
	if (FAILED(CoCreateInstance(CLSID_ShellLink, NULL, CLSCTX_INPROC_SERVER,
								IID_IShellLink, (LPVOID*) &pIShell)))
		return;

	//	Set the path specifications
	strFile = m_strControlFolder + "tmregsvr.exe";
	strLink = szStartup;
	if(strLink.Right(1) != "\\")
		strLink += "\\";
	strLink += "tmregsvr.lnk";

	if(SUCCEEDED(pIShell->QueryInterface(IID_IPersistFile, (LPVOID *) &pIPersist)))
	{
		//	Conver the link specification to wide character
		MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED, strLink, -1, (LPWSTR)wszLink,
							MAX_PATH);

		pIShell->SetPath(strFile);
		pIShell->SetDescription("TmaxPresentation Registration");
		pIShell->SetWorkingDirectory(m_strControlFolder);

		pIPersist->Save((LPCOLESTR)wszLink, TRUE);
		pIPersist->Release();
	}

	pIShell->Release();
	CoUninitialize();
}

//==============================================================================
//
// 	Function Name:	CTmregsvrDlg::CTmregsvrDlg()
//
// 	Description:	This is the constructor for CTMregsvrDlg objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTmregsvrDlg::CTmregsvrDlg(CWnd* pParent) : CDialog(CTmregsvrDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CTmregsvrDlg)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);

	m_brBackGnd.CreateSolidBrush(RGB(0,0,0));
	m_bCheckForReboot = FALSE;
	m_strControlFolder.Empty();
	m_iLine = 1;
}

//==============================================================================
//
// 	Function Name:	CTmregsvrDlg::DeleteShortcut()
//
// 	Description:	This function will delete it's shortcut from the startup
//					group
//
// 	Returns:		TRUE if successful
//
//	Notes:			On of the TrialMax installation programs may have placed the
//					shortcut if unable to register at the time of installation
//
//==============================================================================
void CTmregsvrDlg::DeleteShortcut()
{
	HKEY			hKey;
	DWORD			dwType = REG_SZ;
	char			szStartup[MAX_PATH];
	DWORD			dwSize = sizeof(szStartup);
	CString			strShortcut;
	
	//	Get the path to the startup group
	memset(szStartup, 0, sizeof(szStartup));
	if(RegOpenKey(HKEY_CURRENT_USER,
			   "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders",
			   &hKey) == ERROR_SUCCESS)
	{
		RegQueryValueEx(hKey, "Startup", 0, &dwType, (LPBYTE)szStartup,
						&dwSize);
	}
	
	//	Build the specification for the shortcut
	strShortcut = szStartup;
	if(strShortcut.Right(1) != "\\")
		strShortcut += "\\";
	strShortcut += "tmregsvr.lnk";

	//	Delete the shortcut
	_unlink(strShortcut);
}

//==============================================================================
//
// 	Function Name:	CTmregsvrDlg::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					dialog box and class members
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTmregsvrDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CTmregsvrDlg)
	DDX_Control(pDX, IDC_STATUS, m_ctrlStatus);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CTmregsvrDlg::FindFile()
//
// 	Description:	This function checks to see if the file exists.
//
// 	Returns:		TRUE if the file exists.
//
//	Notes:			None
//
//==============================================================================
BOOL CTmregsvrDlg::FindFile(LPCSTR lpFilespec)
{
	WIN32_FIND_DATA	Find;
	HANDLE			hFind;

	if((hFind = FindFirstFile(lpFilespec, &Find)) == INVALID_HANDLE_VALUE)
	{
		return FALSE;
	}
	else
	{
		//	Close the file find handle
		FindClose(hFind);
		return TRUE;
	}	
}

//==============================================================================
//
// 	Function Name:	CTmregsvrDlg::GetNextName()
//
// 	Description:	This function is called to get the name of the next control
//					to be registered.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTmregsvrDlg::GetNextName(CString& rName)
{
	char szIniStr[256];

	//	Clear the caller's buffer
	rName.Empty();

	//	Get the next name from the ini file
	m_Ini.ReadString(m_iLine, szIniStr, sizeof(szIniStr));
	rName = szIniStr;
	rName.TrimLeft();
	rName.TrimRight();

	m_iLine++;

	return !rName.IsEmpty();
}

//==============================================================================
//
// 	Function Name:	CTmregsvrDlg::OnCtlColor()
//
// 	Description:	This function traps all WM_CTLCOLOR messages sent to the
//					dialog box
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
HBRUSH CTmregsvrDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor) 
{
	//	Is this a request for the dialog brush?
	if(nCtlColor == CTLCOLOR_DLG || nCtlColor == CTLCOLOR_STATIC)
	{
		pDC->SetBkMode(TRANSPARENT);
		pDC->SetBkColor(RGB(0,0,0));
		pDC->SetTextColor(RGB(255,255,255));
		return (HBRUSH)m_brBackGnd;
	}
	else
		return CDialog::OnCtlColor(pDC, pWnd, nCtlColor);
}

//==============================================================================
//
// 	Function Name:	CTmregsvrDlg::OnInitDialog()
//
// 	Description:	This function is called to initialize the dialog
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CTmregsvrDlg::OnInitDialog()
{
	char	szFolder[256];
	CString	strFilespec;
	char*	pToken;

	//	Do the base class initialization
	CDialog::OnInitDialog();

	//	Initialize the error handler
	m_Errors.SetParent(m_hWnd);
	m_Errors.SetTitle("TMRegsvr Error");
	m_Errors.Enable(TRUE);

	//	Set the large and small icons for this dialog
	SetIcon(m_hIcon, TRUE);			
	SetIcon(m_hIcon, FALSE);		
	
	m_ctrlStatus.SetWindowText("");

	//	Build the file specification for the ini file
	//
	//	NOTE:	We assume the ini file is in the folder with the app
	lstrcpyn(szFolder, theApp.m_pszHelpFilePath, sizeof(szFolder));
	if((pToken = strrchr(szFolder, '\\')) != 0)
		*(pToken + 1) = 0;
	m_strControlFolder = szFolder;
	if(m_strControlFolder.Right(1) != "\\")
		m_strControlFolder += "\\";
	strFilespec = m_strControlFolder + TMREGSVR_FILENAME;

	//	Open the ini file
	if(m_Ini.Open(strFilespec, TMREGSVR_SECTION))
	{
		//	Start the timer to begin the registration
		SetTimer(1, 500, NULL);
		BringWindowToTop();
		return TRUE;
	}
	else
	{
		m_Errors.Handle(0, IDS_TMREGSVR_FILENOTFOUND, m_Ini.strFileSpec);
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CTmregsvrDlg::OnQueryDragIcon()
//
// 	Description:	This function is called to obtain the cursor to display 
//					while the user drags the minimized icon
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
HCURSOR CTmregsvrDlg::OnQueryDragIcon()
{
	return (HCURSOR)m_hIcon;
}

//==============================================================================
//
// 	Function Name:	CTmregsvrDlg::OnTimer()
//
// 	Description:	This function handles all WM_TIMER messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTmregsvrDlg::OnTimer(UINT nIDEvent) 
{
	CString strFilespec;
	CString strText;
	CString	strName;

	//	Kill the timer and start the registrations
	KillTimer(nIDEvent);

	//	Do we need to delay registration until the system is rebooted?
	if(RebootRequired())
	{
		AddShortcut();
		m_ctrlStatus.SetWindowText("Delayed until reboot");
		Sleep(2000);
		CDialog::OnCancel();
		return;
	}

	//	Get the next control
	while(GetNextName(strName))
	{
		//	Build the file specification
		strFilespec = m_strControlFolder + strName;

		//	Does the file exist?
		if(FindFile(strFilespec))
		{
			//	Attempt to register the file
			if(Register(strFilespec))
			{
				strText.Format("Registered %s", strName);
				m_ctrlStatus.SetWindowText(strText);
				Sleep(1000);
			}
			else
			{
				strText.Format("%s failed - Error 0x%.6lx", strName, m_dwLastError);
				MessageBeep(MB_ICONEXCLAMATION);
				m_ctrlStatus.SetWindowText(strText);
				Sleep(2000);
			}
		}
		else
		{
			strText.Format("%s failed - File not found", strName);
			MessageBeep(MB_ICONEXCLAMATION);
			m_ctrlStatus.SetWindowText(strText);
			Sleep(2000);
		}
	}

	//	Make sure the shortcut is removed from the startup group
	DeleteShortcut();

	//	Close the dialog
	CDialog::OnCancel();
}

//==============================================================================
//
// 	Function Name:	CTmregsvrDlg::RebootRequired()
//
// 	Description:	This function will check the system files to see if a reboot
//					is going to be required before we can register the TrialMax
//					controls
//
// 	Returns:		TRUE if reboot required
//
//	Notes:			None
//
//==============================================================================
BOOL CTmregsvrDlg::RebootRequired()
{
	WIN32_FIND_DATA	Find;
	HANDLE			hFind;
	char			szSystem[MAX_PATH];
	CString			strSearch;

	//	Are we supposed to check?
	if(!m_bCheckForReboot)
		return FALSE;

	//	We are going to search the system directory for any files with a .1 
	//	extension. We assume that these files will be replaced on reboot
	GetSystemDirectory(szSystem, sizeof(szSystem));
	strSearch.Format("%s\\*.1", szSystem);

	if((hFind = FindFirstFile(strSearch, &Find)) == INVALID_HANDLE_VALUE)
	{
		return FALSE;
	}
	else
	{
		//	Close the file find handle
		FindClose(hFind);
		return TRUE;
	}	
}

//==============================================================================
//
// 	Function Name:	CTmregsvrDlg::Register()
//
// 	Description:	This function will register the library specified by the
//					caller
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTmregsvrDlg::Register(LPCSTR lpLibrary)
{
	HINSTANCE hOcx = LoadLibrary(lpLibrary);

	if(hOcx != NULL)
	{
		// Find the entry point.
		FARPROC lpRegister = GetProcAddress(hOcx, _T("DllRegisterServer"));
		if(lpRegister != NULL)
		{
			(*lpRegister)();
			return TRUE;
		}
	}
	
	m_dwLastError = GetLastError();
	return FALSE;
}

