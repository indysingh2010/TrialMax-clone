//==============================================================================
//
// File Name:	propsht.cpp
//
// Description:	This file contains member functions of the CProperties class
//
// See Also:	propsht.h
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	05-20-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <propsht.h>

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
IMPLEMENT_DYNAMIC(CProperties, CPropertySheet)

BEGIN_MESSAGE_MAP(CProperties, CPropertySheet)
	//{{AFX_MSG_MAP(CProperties)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CProperties::CProperties()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CProperties::CProperties(CWnd* pParentWnd)
	        :CPropertySheet(_T("Properties"), pParentWnd, 0)
{
	//	Add the pages
	AddPage(&m_Image);
	AddPage(&m_Media);
	AddPage(&m_About);

	//	Turn off the apply button
    m_psh.dwFlags |= PSH_NOAPPLYNOW;
}

//==============================================================================
//
// 	Function Name:	CProperties::~CProperties()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CProperties::~CProperties()
{
}

//==============================================================================
//
// 	Function Name:	CProperties::OnInitDialog()
//
// 	Description:	This function handles all WM_INITDIALOG messages
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CProperties::OnInitDialog() 
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
