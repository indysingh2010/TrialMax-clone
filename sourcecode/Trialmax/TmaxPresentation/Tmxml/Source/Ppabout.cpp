//==============================================================================
//
// File Name:	ppabout.cpp
//
// Description:	This file contains member functions of the CPPAbout class
//
// See Also:	ppabout.h
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
#include <ppabout.h>

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
IMPLEMENT_DYNCREATE(CPPAbout, CPropertyPage)

BEGIN_MESSAGE_MAP(CPPAbout, CPropertyPage)
	//{{AFX_MSG_MAP(CPPAbout)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CPPAbout::CPPAbout()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPAbout::CPPAbout() : CPropertyPage(CPPAbout::IDD)
{
	//{{AFX_DATA_INIT(CPPAbout)
	m_strVersion = _T("");
	//}}AFX_DATA_INIT

	m_strVersion.Format("Revision %d.%d", _wVerMajor, _wVerMinor);
}

//==============================================================================
//
// 	Function Name:	CPPAbout::~CPPAbout()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPAbout::~CPPAbout()
{
}

//==============================================================================
//
// 	Function Name:	CPPAbout::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					dialog box controls and class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPAbout::DoDataExchange(CDataExchange* pDX)
{
	CPropertyPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPPAbout)
	DDX_Text(pDX, IDC_VERSION, m_strVersion);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CPPAbout::OnInitDialog()
//
// 	Description:	This function handles all WM_INITDIALOG messages
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CPPAbout::OnInitDialog() 
{
	//	Do the base class processing first
	CPropertyPage::OnInitDialog();
	
	return TRUE;
}


