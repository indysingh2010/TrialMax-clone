//==============================================================================
//
// File Name:	prefer.cpp
//
// Description:	This file contains member functions of the CPreferences class
//
// See Also:	prefer.h
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	03-10-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <prefer.h>

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
IMPLEMENT_DYNAMIC(CPreferences, CPropertySheet)

BEGIN_MESSAGE_MAP(CPreferences, CPropertySheet)
	//{{AFX_MSG_MAP(CPreferences)
		// NOTE - the ClassWizard will add and remove mapping macros here.
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CPreferences::CPreferences()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPreferences::CPreferences(CWnd* pParentWnd, int iPage)
	         :CPropertySheet(_T("Preferences"), pParentWnd, iPage)
{
	//	Add the pages
	AddPage(&m_Viewer);
	AddPage(&m_Tools);
	AddPage(&m_Printer);
	AddPage(&m_Toolbar);

	//	Turn off the apply button
    m_psh.dwFlags |= PSH_NOAPPLYNOW;
}

//==============================================================================
//
// 	Function Name:	CPreferences::~CPreferences()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPreferences::~CPreferences()
{
}

