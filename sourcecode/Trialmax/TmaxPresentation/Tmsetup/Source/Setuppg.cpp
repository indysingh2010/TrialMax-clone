//==============================================================================
//
// File Name:	setuppg.cpp
//
// Description:	This file contains member functions of the CSetupPage class.
//
// See Also:	setuppg.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-31-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmsetap.h>
#include <setuppg.h>
#include <tmsetup.h>

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
BEGIN_MESSAGE_MAP(CSetupPage, CDialog)
	//{{AFX_MSG_MAP(CSetupPage)
		// NOTE: the ClassWizard will add message map macros here
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CSetupPage::CSetupPage()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CSetupPage::CSetupPage(int iResourceId, CWnd* pParent) : CDialog(iResourceId, pParent)
{
	//{{AFX_DATA_INIT(CSetupPage)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT

	m_pControl = 0;
	m_pErrors = 0;
	m_iTab = -1;
	m_iPage = -1;
}

//==============================================================================
//
// 	Function Name:	CSetupPage::~CSetupPage()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CSetupPage::~CSetupPage()
{
}

//==============================================================================
//
// 	Function Name:	CSetupPage::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the class
//					members and their associated dialog box controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSetupPage::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CSetupPage)
		// NOTE: the ClassWizard will add DDX and DDV calls here
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CSetupPage::ReadOptions()
//
// 	Description:	This function is called to read the page options from the
//					ini file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSetupPage::ReadOptions(CTMIni& rIni)
{
}

//==============================================================================
//
// 	Function Name:	CSetupPage::WriteOptions()
//
// 	Description:	This function is called to write the page options to the
//					ini file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CSetupPage::WriteOptions(CTMIni& rIni)
{
	return FALSE;
}




