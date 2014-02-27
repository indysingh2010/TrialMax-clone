//==============================================================================
//
// File Name:	App.cpp
//
// Description:	This file contains member functions of the CApp class 
//
// See Also:	App.h
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
#include <TmaxPathLength.h>
#include <Mainwnd.h>

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
CApp _theApp;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CApp, CWinApp)
	//{{AFX_MSG_MAP(CApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CApp::CApp()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CApp::CApp()
{
	m_strAppFolder = "";
}

//==============================================================================
//
// 	Function Name:	CApp::InitInstance()
//
// 	Description:	Called by MFC to initialize the application instance
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CApp::InitInstance()
{
	char	szFolder[1024];
	char*	pToken = NULL;

	AfxEnableControlContainer();
//	Enable3dControlsStatic();	// Call this when linking to MFC statically

	//	Get the application folder
	lstrcpyn(szFolder, m_pszHelpFilePath, sizeof(szFolder));
	if((pToken = strrchr(szFolder, '\\')) != 0)
		*(pToken + 1) = 0;
	m_strAppFolder = szFolder;
	m_strAppFolder.MakeLower();

	CMainWnd dlg;
	m_pMainWnd = &dlg;
	dlg.DoModal();

	return FALSE;
}
