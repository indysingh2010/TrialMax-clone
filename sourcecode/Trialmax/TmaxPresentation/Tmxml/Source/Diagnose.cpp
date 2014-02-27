//==============================================================================
//
// File Name:	diagnose.cpp
//
// Description:	This file contains member functions of the CDiagnostics class
//
// See Also:	diagnose.h
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	07-14-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <diagnose.h>

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
IMPLEMENT_DYNAMIC(CDiagnostics, CPropertySheet)

BEGIN_MESSAGE_MAP(CDiagnostics, CPropertySheet)
	//{{AFX_MSG_MAP(CDiagnostics)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CDiagnostics::CDiagnostics()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDiagnostics::CDiagnostics(CWnd* pParentWnd)
	        :CPropertySheet(_T("Diagnostics"), pParentWnd, 0)
{
	//	Add the pages
	AddPage(&m_Revisions);
	AddPage(&m_Tmx);
	AddPage(&m_Download);
	AddPage(&m_Treatment);

	//	Turn off the apply button
    m_psh.dwFlags |= PSH_NOAPPLYNOW;
}

//==============================================================================
//
// 	Function Name:	CDiagnostics::~CDiagnostics()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDiagnostics::~CDiagnostics()
{
}

//==============================================================================
//
// 	Function Name:	CDiagnostics::OnInitDialog()
//
// 	Description:	This function handles all WM_INITDIALOG messages
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CDiagnostics::OnInitDialog() 
{
	CWnd* pButton;

	BOOL bResult = CPropertySheet::OnInitDialog();
	
	//	Get the OK button
	if((pButton = GetDlgItem(IDOK)) != 0)
	{
		pButton->ShowWindow(SW_HIDE);

		//	Now make the Cancel button look like the OK button
		if((pButton = GetDlgItem(IDCANCEL)) != 0)
			pButton->SetWindowText("OK");
	}
	
	return bResult;
}
