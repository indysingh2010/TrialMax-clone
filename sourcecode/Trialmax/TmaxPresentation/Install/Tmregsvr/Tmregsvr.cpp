//==============================================================================
//
// File Name:	Tmregsvr.cpp
//
// Description:	This file contains member functions of the CTmregsvrApp class
//
// Functions:   CTmregsvrApp::CTmregsvrApp()
//				CTmregsvrApp::InitInstance()
//
// See Also:	tmregsvr.h
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
CTmregsvrApp theApp;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CTmregsvrApp, CWinApp)
	//{{AFX_MSG_MAP(CTmregsvrApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CTmregsvrApp::CTmregsvrApp()
//
// 	Description:	This is the constructor for CTmregsvrApp objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTmregsvrApp::CTmregsvrApp()
{
}

//==============================================================================
//
// 	Function Name:	CTmregsvrApp::InitInstance()
//
// 	Description:	This function is called to initialize the application
//					instance
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTmregsvrApp::InitInstance()
{
	AfxEnableControlContainer();

	//#ifdef _AFXDLL
	//Enable3dControls();			// Call this when using MFC in a shared DLL
	//#else
	//Enable3dControlsStatic();	// Call this when linking to MFC statically
	//#endif

	CTmregsvrDlg dlg;
	m_pMainWnd = &dlg;
	
	//	If the command line is set we assume this app is being launched by
	//	InstallShield
	if(lstrlen(m_lpCmdLine) > 0)
	{
		dlg.m_bCheckForReboot = TRUE;
	}

	dlg.DoModal();

	// Since the dialog has been closed, return FALSE so that we exit the
	//  application, rather than start the application's message pump.
	return FALSE;
}
