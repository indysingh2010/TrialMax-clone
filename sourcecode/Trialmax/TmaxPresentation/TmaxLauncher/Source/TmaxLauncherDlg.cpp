//==============================================================================
//
// File Name:	TmaxLauncherDlg.cpp
//
// Description:	This file contains member functions of the CTmaxLauncherDlg 
//				class
//
// See Also:	TmaxLauncherDlg.h
//
// Copyright	FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	02-07-2007	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <StdAfx.h>
#include <TmaxLauncher.h>
#include <TmaxLauncherDlg.h>
#include <SplashWnd.h>
#include <Registry.h>
#include <Toolbox.h>

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
extern CTmaxLauncherApp _theApp;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CTmaxLauncherDlg, CDialog)
	//{{AFX_MSG_MAP(CTmaxLauncherDlg)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_WM_TIMER()
	ON_BN_CLICKED(IDC_LAUNCH, OnClickLaunch)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CTmaxLauncherDlg::CTmaxLauncherDlg()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTmaxLauncherDlg::CTmaxLauncherDlg(CWnd* pParent) : CDialog(CTmaxLauncherDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CTmaxLauncherDlg)
	m_strSourceFile = _T("");
	m_strVideoPath = _T("");
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32

	m_pwndSplash = NULL;
	m_strExecutableFolder = "";
	m_strExecutableFileSpec = "";
	m_strSetupFileSpec = "";
	m_bInstalled = FALSE;

	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

//==============================================================================
//
// 	Function Name:	CTmaxLauncherDlg::~CTmaxLauncherDlg()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTmaxLauncherDlg::~CTmaxLauncherDlg()
{
	if(m_pwndSplash != NULL)
		delete m_pwndSplash;
}

//==============================================================================
//
// 	Function Name:	CTmaxLauncherDlg::DoDataExchange()
//
// 	Description:	Called by MFC to manage exchange of data between the 
//					dialog box controls and class members
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTmaxLauncherDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);

	//{{AFX_DATA_MAP(CTmaxLauncherDlg)
	DDX_Text(pDX, IDC_SOURCE_FILE, m_strSourceFile);
	DDX_Text(pDX, IDC_VIDEO_PATH, m_strVideoPath);
	//}}AFX_DATA_MAP

	DDX_Text(pDX, IDC_EXECUTABLE_FILESPEC, m_strExecutableFileSpec);
	DDX_Text(pDX, IDC_SETUP_FILESPEC, m_strSetupFileSpec);

	DDX_Text(pDX, IDC_PRODUCT_INFO_FOLDER, m_tmaxProductInfo.strFolder);
	DDX_Text(pDX, IDC_PRODUCT_INFO_FILENAME, m_tmaxProductInfo.strFilename);
	DDX_Text(pDX, IDC_PRODUCT_INFO_VERSION, m_tmaxProductInfo.strVersion);
	DDX_Text(pDX, IDC_PRODUCT_INFO_DESCRIPTION, m_tmaxProductInfo.strDescription);
	DDX_Text(pDX, IDC_PRODUCT_INFO_INSTALLDIR, m_tmaxProductInfo.strInstallDir);

}

//==============================================================================
//
// 	Function Name:	CTmaxLauncherDlg::Execute()
//
// 	Description:	Called to execute the program at the specified path
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTmaxLauncherDlg::Execute(LPCSTR lpszFileSpec, LPCSTR lpszCmdLine, BOOL bWait)
{
	STARTUPINFO			StartupInfo;
	PROCESS_INFORMATION	ProcessInfo;
	DWORD				dwExitCode = NOERROR;
	char				szWorkingDir[MAX_PATH];
	char				szCmdLine[MAX_PATH];
	char*				pToken = NULL;

    ZeroMemory(&StartupInfo, sizeof(StartupInfo));
	ZeroMemory(&ProcessInfo, sizeof(ProcessInfo));
	StartupInfo.cb = sizeof(StartupInfo);

 	ASSERT(lpszFileSpec != NULL);
	ASSERT(lstrlen(lpszFileSpec) > 0);
	
	//	Set the working directory to the executable's folder
	lstrcpyn(szWorkingDir, lpszFileSpec, sizeof(szWorkingDir));
	if((pToken = strrchr(szWorkingDir, '\\')) != NULL)
		*pToken = '\0';
	
	//	Construct the command line
	//
	//	NOTE:	The command line MUST start with a space or it will fail
	if((lpszCmdLine != NULL) && (lstrlen(lpszCmdLine) > 0))
		sprintf_s(szCmdLine, sizeof(szCmdLine), " %s", lpszCmdLine);
	else
		memset(szCmdLine, 0, sizeof(szCmdLine));

	if(!CreateProcess(lpszFileSpec, szCmdLine, 0, 0, 0, 0, 0, szWorkingDir, &StartupInfo, &ProcessInfo))
	{
		OnError(IDS_ERROR_EXECUTE_FAILED, lpszFileSpec);
		return FALSE;
	}
	
	//	Should we wait for the process to finish?
	if(bWait == TRUE)
	{
		//	Wait for the generator to finish
		WaitForSingleObject(ProcessInfo.hProcess, INFINITE);

		//	Get the generator's exit code 
		GetExitCodeProcess(ProcessInfo.hProcess, &dwExitCode);
      
		//	Was there an error creating the process?
		if(dwExitCode != ERROR_SUCCESS)
		{ 
			OnError(IDS_ERROR_EXIT_CODE, lpszFileSpec, dwExitCode);
			return FALSE;
		}

	}// if(bWait == TRUE)

	//	All's good ...
	return TRUE;

}

//==============================================================================
//
// 	Function Name:	CTmaxLauncherDlg::GetExecutableFileSpec()
//
// 	Description:	Called to get the path to the product executable
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTmaxLauncherDlg::GetExecutableFileSpec()
{
	CRegistry Registry;

	m_strExecutableFolder = "";
	m_strExecutableFileSpec = "";
	m_strSourceFile = "";
	m_strVideoPath = "";
	m_tmaxProductInfo.strFilename = "";
	m_tmaxProductInfo.strFolder = "";
	m_tmaxProductInfo.strVersion = "";
	m_tmaxProductInfo.strDescription = "";
	m_tmaxProductInfo.strInstallDir = "";

	//	Get the product information for the video viewer
	Registry.GetProductInfo("VideoViewer", m_tmaxProductInfo);
	
	if(m_tmaxProductInfo.strFolder.GetLength() > 0)
	{
		m_strExecutableFolder = m_tmaxProductInfo.strFolder;
	}
	else if(m_tmaxProductInfo.strInstallDir.GetLength() > 0)
	{
		m_strExecutableFolder = m_tmaxProductInfo.strInstallDir;
		if(m_strExecutableFolder.Right(1) != "\\")
			m_strExecutableFolder += "\\";
		m_strExecutableFolder += DEFAULT_VIDEO_VIEWER_FOLDER;
	}
	else
	{
		//	Assign the default folder path
		m_strExecutableFolder.Format("%s%s", DEFAULT_FTI_INSTALL_FOLDER, DEFAULT_VIDEO_VIEWER_FOLDER);
	}

	//	Construct the path to the executable
	m_strExecutableFileSpec = m_strExecutableFolder;
	if(m_strExecutableFileSpec.Right(1) != "\\")
		m_strExecutableFileSpec += "\\";
	if(m_tmaxProductInfo.strFilename.GetLength() > 0)
	{
		m_strExecutableFileSpec += m_tmaxProductInfo.strFilename;
	}
	else
	{
		//	Assign the default filename
		m_strExecutableFileSpec += DEFAULT_VIDEO_VIEWER_EXECUTABLE;
	}

	//	Pass the first available source file on the command line
	m_strSourceFile = GetSourceFileSpec();

	//	Get the video path if we found a source file
	if(m_strSourceFile.GetLength() > 0)
		m_strVideoPath = GetVideoPath(m_strSourceFile);

	//	Update the dialog box controls
	UpdateData(FALSE);

	return CTMToolbox::FindFile(m_strExecutableFileSpec);
}

//==============================================================================
//
// 	Function Name:	CTmaxLauncherDlg::GetSourceFileSpec()
//
// 	Description:	Called to get the path to the source file that should
//					be specified on the command line when the application
//					gets executed
//
// 	Returns:		The fully qualified path to the source file
//
//	Notes:			None
//
//==============================================================================
CString CTmaxLauncherDlg::GetSourceFileSpec()
{
	CString strFileSpec = "";

	//	Search for a transcript
	strFileSpec = CTMToolbox::FindFirstFile(_theApp.GetAppFolder(), DEFAULT_TRANSCRIPT_EXTENSION);
	
	//	Look for a script if no transcript was found
	if(strFileSpec.GetLength() == 0)
		strFileSpec = CTMToolbox::FindFirstFile(_theApp.GetAppFolder(), DEFAULT_SCRIPT_EXTENSION);

	return strFileSpec;
}

//==============================================================================
//
// 	Function Name:	CTmaxLauncherDlg::GetVideoPath()
//
// 	Description:	Called to get the path to the videos for the specified
//					source file
//
// 	Returns:		The fully qualified path to the video files folder
//
//	Notes:			None
//
//==============================================================================
CString CTmaxLauncherDlg::GetVideoPath(LPCSTR lpszSourceFileSpec)
{
	CString strFolder = "";
	char	szBuffer[MAX_PATH];
	char*	pToken = NULL;

	//	Strip off the filename and use the parent folder as the video folder
	lstrcpyn(szBuffer, lpszSourceFileSpec, sizeof(szBuffer));
	if((pToken = strrchr(szBuffer, '\\')) != NULL)
		*pToken = '\0'; //	Make sure to strip the trailing backslash - otherwise the command line parser fails
	else
		memset(szBuffer, 0, sizeof(szBuffer));

	strFolder = szBuffer;
	return strFolder;
}

//==============================================================================
//
// 	Function Name:	CTmaxLauncherDlg::Install()
//
// 	Description:	Called to install the product
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTmaxLauncherDlg::Install()
{
	CString strFolder = "";

	m_bInstalled = FALSE;

	//	Assume the setup file is in the same folder as this application
	strFolder = _theApp.GetAppFolder();
	if(strFolder.Right(1) != "\\")
		strFolder += "\\";

	//	Search for the standard setup first
	m_strSetupFileSpec.Format("%s%s", strFolder, DEFAULT_VIDEO_STANDARD_SETUP);
	if(CTMToolbox::FindFile(m_strSetupFileSpec) == FALSE)
	{
		//	Try the custom setup
		m_strSetupFileSpec.Format("%s%s", strFolder, DEFAULT_VIDEO_CUSTOM_SETUP);
		if(CTMToolbox::FindFile(m_strSetupFileSpec) == FALSE)
		{
			OnError(IDS_ERROR_INSTALLATION_NOT_FOUND);
			return FALSE;
		}

	}

	//	Update the dialog box
	UpdateData(FALSE);

	//	Execute the setup
	if(Execute(m_strSetupFileSpec, "", TRUE) == TRUE)
	{
		//	This flag prevents an endless loop
		m_bInstalled = TRUE;

		//	Now launch the application
		Launch();
	}
	
	return m_bInstalled;
}

//==============================================================================
//
// 	Function Name:	CTmaxLauncherDlg::Launch()
//
// 	Description:	Called to launch the product executable or installation
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTmaxLauncherDlg::Launch()
{
	BOOL	bSuccessful = FALSE;
	CString	strCmdLine = "";

	//	Get the path to the product's executable file
	if(GetExecutableFileSpec() == TRUE)
	{
		//	Construct the command line
		if(m_strSourceFile.GetLength() > 0)
		{
			if(m_strVideoPath.GetLength() > 0)
				strCmdLine.Format("Source=\"%s\" Video=\"%s\"", m_strSourceFile, m_strVideoPath);
			else
				strCmdLine.Format("Source=\"%s\"", m_strSourceFile);
		}

		//	Open the program
		bSuccessful = Execute(m_strExecutableFileSpec, strCmdLine, FALSE);

	}
	else if(m_bInstalled == FALSE)
	{
		bSuccessful = Install();
	}

	
	return bSuccessful;
}

//==============================================================================
//
// 	Function Name:	CTmaxLauncherDlg::OnClickLaunch()
//
// 	Description:	Called when the user clicks on the Launch button
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTmaxLauncherDlg::OnClickLaunch() 
{
	m_bInstalled = FALSE;
	Launch();	
}

//==============================================================================
//
// 	Function Name:	CTmaxLauncherDlg::OnError()
//
// 	Description:	Called to display a formatted error message
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTmaxLauncherDlg::OnError(LPCSTR lpszFormat, ...)
{
	char szBuffer[1024];
	
	//	Declare the variable list of arguements            
	va_list	Arguements;

	//	Insert the first variable arguement into the arguement list
	va_start(Arguements, lpszFormat);

	//	Format the message
	vsprintf_s(szBuffer, sizeof(szBuffer), lpszFormat, Arguements);

	//	Clean up the arguement list
	va_end(Arguements);

	MessageBox(szBuffer, "Error", MB_ICONEXCLAMATION | MB_OK);
}

//==============================================================================
//
// 	Function Name:	CTmaxLauncherDlg::OnError()
//
// 	Description:	Called to display a formatted error message
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTmaxLauncherDlg::OnError(UINT uId, ...)
{
	CString	strFormat;
	char	szBuffer[1024];
	
	//	Load the format string from the resources
	strFormat.LoadString(uId);
	
	//	Declare the variable list of arguements            
	va_list	Arguements;

	//	Insert the first variable arguement into the arguement list
	va_start(Arguements, uId);

	//	Format the message
	vsprintf_s(szBuffer, sizeof(szBuffer), strFormat, Arguements);

	//	Clean up the arguement list
	va_end(Arguements);

	MessageBox(szBuffer, "Error", MB_ICONEXCLAMATION | MB_OK);
}

//==============================================================================
//
// 	Function Name:	CTmaxLauncherDlg::OnInitDialog()
//
// 	Description:	Handles the WM_INITDIALOG message
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CTmaxLauncherDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	//	Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	//	Are we running in test mode?
	if(_theApp.GetAppCmdLine().m_bTestMode == TRUE)
	{
		//	Center this window on the desktop
		CenterWindow();

	}
	else
	{
		m_pwndSplash = new CSplashWnd();

		//	Move the dialog to make it invisible
		MoveWindow(0,0,1,1);
		
		//	Show the splash screen without a timer
		m_pwndSplash->Show(IDB_TMAX_VIDEO_VIEWER, this);

		//	Now launch the executable or the installation
		SetTimer(LAUNCH_TIMER_ID, 250, NULL);
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTmaxLauncherDlg::OnPaint()
//
// 	Description:	Called when the window requires painting
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTmaxLauncherDlg::OnPaint() 
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, (WPARAM) dc.GetSafeHdc(), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

//==============================================================================
//
// 	Function Name:	CTmaxLauncherDlg::OnQueryDragIcon()
//
// 	Description:	Called to obtain a handle to the application's icon
//
// 	Returns:		The icon handle
//
//	Notes:			None
//
//==============================================================================
HCURSOR CTmaxLauncherDlg::OnQueryDragIcon()
{
	return (HCURSOR) m_hIcon;
}

//==============================================================================
//
// 	Function Name:	CTmaxLauncherDlg::OnTimer()
//
// 	Description:	Handles WM_TIMER messages sent to the window
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTmaxLauncherDlg::OnTimer(UINT nIDEvent) 
{
	//	Stop the timer while we process the message
	KillTimer(nIDEvent);
	
	switch(nIDEvent)
	{
		case LAUNCH_TIMER_ID:

			Launch();

			ASSERT(_theApp.GetAppCmdLine().m_bTestMode == FALSE);
			if(_theApp.GetAppCmdLine().m_bTestMode == FALSE)
			{
				//	Close the application
				OnCancel();
			}
			break;

		default:

			ASSERT(FALSE);
			break;

	}
	
	CDialog::OnTimer(nIDEvent);
}


