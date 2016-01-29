//==============================================================================
//
// File Name:	cappg.cpp
//
// Description:	This file contains member functions of the CCapturePage class.
//
// See Also:	cappg.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	05-25-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmsetap.h>
#include <cappg.h>
#include  <io.h>
#include  <stdio.h>
#include  <stdlib.h>

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

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CCapturePage, CSetupPage)
	//{{AFX_MSG_MAP(CCapturePage)
	ON_BN_CLICKED(IDC_BROWSEFILEPATH, OnBrowseFilePath)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CCapturePage::CCapturePage()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CCapturePage::CCapturePage(CWnd* pParent) : CSetupPage(CCapturePage::IDD, pParent)
{
	//{{AFX_DATA_INIT(CCapturePage)
	m_iArea = -1;
	m_iHotkey = -1;
	m_bSilent = FALSE;
	//}}AFX_DATA_INIT
}
//==============================================================================
//
// 	Function Name:	CCapturePage::OnInitDialog()
//
// 	Description:	This function manages the exchange of data between the class
//					members and the associated dialog box controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================

BOOL CCapturePage::OnInitDialog() 
{
	//	Perform base class initialization
	CDialog::OnInitDialog();
	
	//	Load the bitmaps for the browse buttons
	m_btnFolder.AutoLoad(IDC_BROWSEFILEPATH, this);

	//	Fill the list box and set the current 
	return TRUE;  
}

//==============================================================================
//
// 	Function Name:	CCapturePage::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the class
//					members and the associated dialog box controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCapturePage::DoDataExchange(CDataExchange* pDX)
{
	CSetupPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CCapturePage)
	DDX_CBIndex(pDX, IDC_AREA, m_iArea);
	DDX_CBIndex(pDX, IDC_HOTKEY, m_iHotkey);
	DDX_Check(pDX, IDC_SILENT, m_bSilent);
	DDX_Text(pDX, IDC_FILEPATH, m_sFilePath);
	DDX_Control(pDX, IDC_FILEPATH, m_ctrlFilePath);
	//}}AFX_DATA_MAP
}



//==============================================================================
//
// 	Function Name:	CCapturePage::OnBrowseFilePath()
//
// 	Description:	This function is called to read the page options from the
//					ini file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCapturePage::OnBrowseFilePath()
{
	BROWSEINFO		BrowseInfo;
	IMalloc*		pMalloc;
	LPITEMIDLIST	pItemIDList;
	TCHAR			szFolder[MAX_PATH];
	CString			strFolder;
	CString			oldstrFolder;
	int				iIndex;
	SCaptureOptions Options;
	bool			successful=true;
	DWORD			cchCurDir = MAX_PATH;
	char			szCurDir[MAX_PATH];	
	
	GetCurrentDirectory(cchCurDir, szCurDir);
	strcat(szCurDir,"\\FTI.ini");
	
	CTMIni*	m_pIni = new CTMIni(szCurDir,"TMGRAB");
	m_pIni ->ReadCaptureOptions(&Options);
	oldstrFolder = Options.sFilePath;
	
	//	Initialize the browse information
	memset(&BrowseInfo, 0, sizeof(BrowseInfo));
	BrowseInfo.hwndOwner = m_hWnd;
	BrowseInfo.lpszTitle = "Select the case folder";
	BrowseInfo.ulFlags = BIF_RETURNONLYFSDIRS;
	BrowseInfo.pszDisplayName = szFolder;

	//	Open the browser dialog
	if((pItemIDList = SHBrowseForFolder(&BrowseInfo)) == NULL)
		return;

	//	Translate the folder's display name to a path specification
	if(!SHGetPathFromIDList(pItemIDList, szFolder))
		return;

	// Convert to lower case so we can do exact comparisons
	strFolder = szFolder;
	strFolder.MakeLower();
	
	//to avoid double back slash on root directories
	if(strFolder.GetLength()>3){
		strFolder.Append("\\");
	}
	
	FILE *fp = fopen(strFolder +"folderaccess.txt", "w");
	if (fp == NULL) {
		if (errno == EACCES)
		{
			AfxMessageBox("TrialMax does not have access to save files in this folder! Please correct Video Export Path.", MB_OK|MB_ICONEXCLAMATION);
		}
		else
		{
			AfxMessageBox(strerror(errno));
		}
		successful = false;
	}
	else
	{
		 fclose(fp);
		 if( remove(strFolder +"folderaccess.txt") != 0 )
		 {
			AfxMessageBox("Failed to delete file");
		 }
	}

	if(successful)
	{
		Options.sFilePath=strFolder;
		m_ctrlFilePath.SetWindowText(strFolder);
		m_pIni ->WriteCaptureOptions(&Options);
	}
	else
	{
		Options.sFilePath=oldstrFolder;
		m_ctrlFilePath.SetWindowText(oldstrFolder);
	}

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
// 	Function Name:	CCapturePage::ReadOptions()
//
// 	Description:	This function is called to read the page options from the
//					ini file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCapturePage::ReadOptions(CTMIni& rIni)
{
	SCaptureOptions Options;

	//	Read the options from the ini file
	rIni.ReadCaptureOptions(&Options);

	//	Set the class members
	m_iArea = Options.sArea;
	m_bSilent = Options.bSilent;
	m_sFilePath= Options.sFilePath;

	switch(Options.sHotkey)
	{
		case VK_F1:		m_iHotkey = 1;
						break;
		case VK_F2:		m_iHotkey = 2;
						break;
		case VK_F3:		m_iHotkey = 3;
						break;
		case VK_F4:		m_iHotkey = 4;
						break;
		case VK_F5:		m_iHotkey = 5;
						break;
		case VK_F6:		m_iHotkey = 6;
						break;
		case VK_F7:		m_iHotkey = 7;
						break;
		case VK_F8:		m_iHotkey = 8;
						break;
		case VK_F9:		m_iHotkey = 9;
						break;
		case VK_F10:	m_iHotkey = 10;
						break;
		case VK_F11:	m_iHotkey = 11;
						break;
		case VK_F12:	m_iHotkey = 12;
						break;
		default:		m_iHotkey = 0;
						break;
	}

	//	Update the controls
	if(IsWindow(m_hWnd))
		UpdateData(FALSE);
}

//==============================================================================
//
// 	Function Name:	CCapturePage::WriteOptions()
//
// 	Description:	This function is called to write the page options to the
//					ini file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CCapturePage::WriteOptions(CTMIni& rIni)
{
	SCaptureOptions Options;

	//	Refresh the class members
	UpdateData(TRUE);

	//	Initialize the structure with the defaults
	//
	//	NOTE:	We do this because we don't allow the user to define all values
	rIni.ReadCaptureOptions(&Options);

	//	Fill the transfer structure
	Options.sArea = m_iArea;
	Options.bSilent = m_bSilent;
	Options.sFilePath = m_sFilePath;

	switch(m_iHotkey)
	{
		case 1:		Options.sHotkey = VK_F1;
					break;
		case 2:		Options.sHotkey = VK_F2;
					break;
		case 3:		Options.sHotkey = VK_F3;
					break;
		case 4:		Options.sHotkey = VK_F4;
					break;
		case 5:		Options.sHotkey = VK_F5;
					break;
		case 6:		Options.sHotkey = VK_F6;
					break;
		case 7:		Options.sHotkey = VK_F7;
					break;
		case 8:		Options.sHotkey = VK_F8;
					break;
		case 9:		Options.sHotkey = VK_F9;
					break;
		case 10:	Options.sHotkey = VK_F10;
					break;
		case 11:	Options.sHotkey = VK_F11;
					break;
		case 12:	Options.sHotkey = VK_F12;
					break;
		default:	Options.sHotkey = 0;
					break;
	}

	//	Write the options to the ini file
	rIni.WriteCaptureOptions(&Options);

	return TRUE;
}

