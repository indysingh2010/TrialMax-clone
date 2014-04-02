//==============================================================================
//
// File Name:	MainWnd.cpp
//
// Description:	This file contains member functions of the CMainWnd class 
//
// See Also:	MainWnd.h
//
// Copyright	FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	03-30-2007	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <StdAfx.h>
#include <TmaxPathLength.h>
#include <Mainwnd.h>
#include <AboutBox.h>

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
extern CApp _theApp;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CMainWnd, CDialog)
	//{{AFX_MSG_MAP(CMainWnd)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_BN_CLICKED(IDC_BROWSE_ROOT, OnBrowseRoot)
	ON_BN_CLICKED(IDC_BROWSE_LOG, OnBrowseLog)
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_SEARCH, OnSearch)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CMainWnd::Add()
//
// 	Description:	Called to add the file to the log
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainWnd::Add(LPCSTR lpszFileSpec)
{
	FILE*	fptr = NULL;
	BOOL	bClose = FALSE;

	m_iViolations++;
	m_strViolations.Format("%d", m_iViolations);
	m_ctrlViolations.SetWindowText(m_strViolations);

	//	Should we try to open the log file
	if(m_fptrLog == NULL)
	{
		fopen_s(&m_fptrLog, m_strLogFile, "at");
		if(m_fptrLog != NULL)
			bClose = TRUE; // Close when finished
	}

	if(m_fptrLog != NULL)
	{
		fprintf(m_fptrLog, "%s\n", lpszFileSpec);
		
		if(bClose == TRUE)
		{
			fclose(m_fptrLog);
			m_fptrLog = NULL;
		}
		
	}			
}

//==============================================================================
//
// 	Function Name:	CMainWnd::CMainWnd()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMainWnd::CMainWnd(CWnd* pParent) : CDialog(CMainWnd::IDD, pParent)
{
	//{{AFX_DATA_INIT(CMainWnd)
	m_strRootFolder = _T("");
	m_strLogFile = _T("");
	m_iMaxLength = 255;
	m_strViolations = _T("");
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);

	m_fptrLog = NULL;
	m_iProgress = 0;
	m_lFiles = 0;
	m_lFolders = 0;
	m_iViolations = 0;
	m_strIniFileSpec = "";
}

//==============================================================================
//
// 	Function Name:	CMainWnd::~CMainWnd()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMainWnd::~CMainWnd()
{
}

//==============================================================================
//
// 	Function Name:	CMainWnd::DoDataExchange()
//
// 	Description:	Manages the exchange between child controls and class 
//					members
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainWnd::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CMainWnd)
	DDX_Control(pDX, IDC_PROGRESS_BAR, m_ctrlProgressBar);
	DDX_Control(pDX, IDC_VIOLATIONS, m_ctrlViolations);
	DDX_Control(pDX, IDC_MAX_LENGTH, m_ctrlMaxLength);
	DDX_Control(pDX, IDC_ROOT_FOLDER, m_ctrlRootFolder);
	DDX_Control(pDX, IDC_LOG_FILE, m_ctrlLogFile);
	DDX_Text(pDX, IDC_ROOT_FOLDER, m_strRootFolder);
	DDX_Text(pDX, IDC_LOG_FILE, m_strLogFile);
	DDX_Text(pDX, IDC_MAX_LENGTH, m_iMaxLength);
	DDV_MinMaxInt(pDX, m_iMaxLength, 1, 32000);
	DDX_Text(pDX, IDC_VIOLATIONS, m_strViolations);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CMainWnd::Load()
//
// 	Description:	Called to load the values stored in the configuration file
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainWnd::Load() 
{
	char szIniStr[1024];

	//	Build the path to the ini file
	m_strIniFileSpec = _theApp.GetAppFolder();
	if(m_strIniFileSpec.Right(1) != "\\")
		m_strIniFileSpec += "\\";
	m_strIniFileSpec += "tmaxMaxPath.ini";

	if(m_Ini.Open(m_strIniFileSpec, APP_SECTION))
	{
		m_strRootFolder = m_Ini.ReadString(APP_ROOT_FOLDER_LINE, szIniStr, sizeof(szIniStr));
		m_strLogFile = m_Ini.ReadString(APP_LOG_FILE_LINE, szIniStr, sizeof(szIniStr));
		m_iMaxLength = (int)(m_Ini.ReadLong(APP_MAX_LENGTH_LINE, 255));

		UpdateData(FALSE);
	}

}

//==============================================================================
//
// 	Function Name:	CMainWnd::OnBrowseLog()
//
// 	Description:	Called when user clicks on log file browse button
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainWnd::OnBrowseLog() 
{
	CFileDialog	FileDlg(TRUE);
	char		szFilter[] = "Log Files (*.txt)\0*.txt\0All Files (*.*)\0*.*\0\0";
	char		szFilename[1024];

	//	Set the dialog flags and other parameters
	memset(szFilename, 0, sizeof(szFilename));
	FileDlg.m_ofn.lpstrFile = szFilename;
	FileDlg.m_ofn.nMaxFile = sizeof(szFilename);
	FileDlg.m_ofn.hwndOwner = m_hWnd;
	FileDlg.m_ofn.lpstrFilter = szFilter;
	FileDlg.m_ofn.Flags |= (OFN_LONGNAMES);
	
	m_ctrlLogFile.GetWindowText(szFilename, sizeof(szFilename));

	if(FileDlg.DoModal() == IDOK)
	{
		m_ctrlLogFile.SetWindowText(szFilename);
	}

}

//==============================================================================
//
// 	Function Name:	CMainWnd::OnBrowseClientsRoot()
//
// 	Description:	Called when user clicks on root folder browse button
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainWnd::OnBrowseRoot() 
{
	BROWSEINFO		BrowseInfo;
	IMalloc*		pMalloc;
	LPITEMIDLIST	pItemIDList;
	TCHAR			szFolder[2048];
	
	//	Initialize the browse information
	m_ctrlRootFolder.GetWindowText(szFolder, sizeof(szFolder));
	memset(&BrowseInfo, 0, sizeof(BrowseInfo));
	BrowseInfo.hwndOwner = m_hWnd;
	BrowseInfo.lpszTitle = "Select a folder";
	BrowseInfo.ulFlags = BIF_RETURNONLYFSDIRS;
	BrowseInfo.pszDisplayName = szFolder;

	//	Open the browser dialog
	if((pItemIDList = SHBrowseForFolder(&BrowseInfo)) == NULL)
		return;

	//	Translate the folder's display name to a path specification
	if(!SHGetPathFromIDList(pItemIDList, szFolder))
		return;

	// Change the folder
	m_ctrlRootFolder.SetWindowText(szFolder);

	//	Delete the PIDL using the shells task allocator
	if(SHGetMalloc(&pMalloc) != NOERROR)
		return;

	//	Free the memory returned by the shell
	if(pMalloc)
	{
		pMalloc->Free(pItemIDList);
		pMalloc->Release();
	}
}

//==============================================================================
//
// 	Function Name:	CMainWnd::OnInitDialog()
//
// 	Description:	Handles the WM_INITDIALOG message
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CMainWnd::OnInitDialog()
{
	CDialog::OnInitDialog();

	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		CString strAboutMenu;
		strAboutMenu.LoadString(IDS_ABOUTBOX);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon
	
	//	Initialize the child controls
	m_ctrlBrowseRoot.AutoLoad(IDC_BROWSE_ROOT, this);
	m_ctrlBrowseLog.AutoLoad(IDC_BROWSE_LOG, this);

	m_ctrlProgressBar.SetRange(0,SEARCH_PROGRESS_MODULO);
	Reset();

	//	Load the saved configuration
	Load();
	
	return TRUE;  // return TRUE  unless you set the focus to a control
}

//==============================================================================
//
// 	Function Name:	CMainWnd::OnPaint()
//
// 	Description:	Handles the WM_PAINT message
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainWnd::OnPaint() 
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
// 	Function Name:	CMainWnd::OnQueryDragIcon()
//
// 	Description:	Handles the WM_QUERYDRAGICON message
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
HCURSOR CMainWnd::OnQueryDragIcon()
{
	return (HCURSOR) m_hIcon;
}

//==============================================================================
//
// 	Function Name:	CMainWnd::OnSearch()
//
// 	Description:	Called when the user clicks on Search
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainWnd::OnSearch() 
{
	CString	strMsg = "";
	CString	strViolations = "";

	while(1)
	{
		//	Set up the new search operation
		Reset();

		//	Get the current values
		if(!UpdateData(TRUE))
			break;

		//	Must supply a root folder
		if(m_strRootFolder.GetLength() == 0)
		{
			MessageBox("You must specify a root folder", "Error", MB_ICONWARNING | MB_OK);
			m_ctrlRootFolder.SetFocus();
			break;
		}

		//	Must supply a log filename
		if(m_strLogFile.GetLength() == 0)
		{
			MessageBox("You must specify a log file", "Error", MB_ICONWARNING | MB_OK);
			m_ctrlLogFile.SetFocus();
			break;
		}

		//	Make sure we can open the log file
		fopen_s(&m_fptrLog, m_strLogFile, "at");
		if(m_fptrLog == NULL)
		{
			strMsg.Format("Unable to open %s to log the results", m_strLogFile);
			MessageBox(strMsg, "Error", MB_ICONWARNING | MB_OK);
			m_ctrlLogFile.SetFocus();
			break;
		}
		else
		{
			//	Comment these lines out to only open the file once per operation
			//fclose(m_fptrLog);
			//m_fptrLog = NULL;
		}

		//	Make sure the maximum length is within range
		if(m_iMaxLength <= 0)
		{
			m_iMaxLength = 255;
			UpdateData(FALSE);
		}
			 
		//	Start the search operation
		Search(m_strRootFolder);
		
		//	Clean up
		m_ctrlProgressBar.SetPos(0);
		if(m_fptrLog != NULL)
		{
			fclose(m_fptrLog);
			m_fptrLog = NULL;
		}

		if(m_lFolders > 0)
		{
			strMsg.Format("%ld Files Processed in %ld Folders\n\nMax Length: %d\nViolations: %d", m_lFiles, m_lFolders, m_iMaxLength, m_iViolations);
			MessageBox(strMsg, "Finished", MB_ICONINFORMATION | MB_OK);
			
			//	Should we open the log file
			if(m_iViolations > 0)
			{
				ShellExecute(m_hWnd, "open", m_strLogFile, "", "", SW_SHOW);
			}

		}

		//	Save the last search options
		Save();

		break;// done

	}// while(1)

}

//==============================================================================
//
// 	Function Name:	CMainWnd::OnSysCommand()
//
// 	Description:	Handles the WM_SYSCOMMAND message
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainWnd::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutBox wndAbout;
		wndAbout.DoModal();
	}
	else
	{
		CDialog::OnSysCommand(nID, lParam);
	}
}

//==============================================================================
//
// 	Function Name:	CMainWnd::Reset()
//
// 	Description:	Called to reset to prepare for a new search
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainWnd::Reset() 
{
	m_iViolations = 0;
	m_iProgress = 0;
	m_lFiles = 0;
	m_lFolders = 0;
	m_strViolations = "";
	m_ctrlViolations.SetWindowText(m_strViolations);
	m_ctrlProgressBar.SetPos(0);
}

//==============================================================================
//
// 	Function Name:	CMainWnd::Save()
//
// 	Description:	Called to save the current values to the configuration file
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainWnd::Save() 
{
	if(m_strIniFileSpec.GetLength() > 0)
	{
		m_Ini.Open(m_strIniFileSpec, APP_SECTION);
		m_Ini.WriteString(APP_ROOT_FOLDER_LINE, m_strRootFolder);
		m_Ini.WriteString(APP_LOG_FILE_LINE, m_strLogFile);
		m_Ini.WriteLong(APP_MAX_LENGTH_LINE, m_iMaxLength);
	}

}

//==============================================================================
//
// 	Function Name:	CMainWnd::Search()
//
// 	Description:	Called to search the specified folder
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainWnd::Search(LPCSTR lpszFolder) 
{
	WIN32_FIND_DATA	FindData;
	HANDLE			hFind;
	CString			strSearch;
	CString			strFile;
	CString			strSubFolder;

	m_lFolders++;

	//	Build the search specification
	strSearch = lpszFolder;
	if(strSearch.Right(1) != "\\")
		strSearch += "\\*.*";
	else
		strSearch += "*.*";

	//	Are there any files or folders?
	if((hFind = FindFirstFile(strSearch, &FindData)) == INVALID_HANDLE_VALUE)
		return;	
	
	//	Add all the files and folders
	while(1)
	{
		//	Is this a folder?
		if(FindData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)
		{
			if(lstrcmpi(FindData.cFileName, ".") &&
			   lstrcmpi(FindData.cFileName, ".."))
			{
				strSubFolder = lpszFolder;
				if(strSubFolder.Right(1) != "\\")
					strSubFolder += "\\";
				strSubFolder += FindData.cFileName;
				
				//	Drill down into this subfolder
				Search(strSubFolder);
			}

		}
		else
		{	
			strFile = lpszFolder;
			if(strFile.Right(1) != "\\")
				strFile += "\\";
			strFile += FindData.cFileName;

			//	Does this path exceed the maximum value
			if(strFile.GetLength() >= m_iMaxLength)
				Add(strFile);

			m_lFiles++;
			m_iProgress = m_lFiles % SEARCH_PROGRESS_MODULO;
			if(m_iProgress != m_ctrlProgressBar.GetPos())
				m_ctrlProgressBar.SetPos(m_iProgress);
		}

		//	Get the next file
		if(!FindNextFile(hFind, &FindData))
			break;

	} // while(1)

	CloseHandle(hFind);
}

