//==============================================================================
//
// File Name:	TmaxLauncher.cpp
//
// Description:	This file contains member functions of the CTmaxLauncherApp 
//				class
//
// See Also:	TmaxLauncher.h
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
CTmaxLauncherApp _theApp;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CTmaxLauncherApp, CWinApp)
	//{{AFX_MSG_MAP(CTmaxLauncherApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CTmaxLauncherApp::CTmaxLauncherApp()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTmaxLauncherApp::CTmaxLauncherApp()
{
	m_strAppFolder = "";
}

//==============================================================================
//
// 	Function Name:	CTmaxLauncherApp::InitInstance()
//
// 	Description:	Called to initialize the application instance
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTmaxLauncherApp::InitInstance()
{
	char	szFolder[MAX_PATH];
	char*	pToken;

	AfxEnableControlContainer();
	//Enable3dControlsStatic();	// Call this when linking to MFC statically

	//	Get the application folder
	lstrcpyn(szFolder, m_pszHelpFilePath, sizeof(szFolder));
	if((pToken = strrchr(szFolder, '\\')) != 0)
		*(pToken + 1) = 0;
	m_strAppFolder = szFolder;
	m_strAppFolder.MakeLower();

	ParseCommandLine(m_AppCmdLine);

	//	Create and display the main dialog window
	CTmaxLauncherDlg dlg;
	m_pMainWnd = &dlg;
	dlg.DoModal();

	return FALSE;
}
