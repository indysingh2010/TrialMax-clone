//==============================================================================
//
// File Name:	aboutpg.cpp
//
// Description:	This file contains member functions of the CAboutPage class.
//
// See Also:	graphpg.h
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
#include <aboutpg.h>

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
BEGIN_MESSAGE_MAP(CAboutPage, CSetupPage)
	//{{AFX_MSG_MAP(CAboutPage)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CAboutPage::CAboutPage()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CAboutPage::CAboutPage(CWnd* pParent) : CSetupPage(CAboutPage::IDD, pParent)
{
	//{{AFX_DATA_INIT(CAboutPage)
	m_strCopyright = _T("");
	m_strEmail = _T("");
	m_strName = _T("");
	m_strVersion = _T("");
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CAboutPage::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the class
//					members and the associated dialog box controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAboutPage::DoDataExchange(CDataExchange* pDX)
{
	//Defining Copyright to handle it in exception
	CString Copyright = "Copyright (C) FTI Consulting 1996-2012";
	CString Version = "7.0";

	CSetupPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAboutPage)
	//Commented purposely 
	//DDX_Text(pDX, IDC_COPYRIGHT, m_strCopyright);
	DDX_Text(pDX, IDC_COPYRIGHT, Copyright);
	DDX_Text(pDX, IDC_EMAIL, m_strEmail);
	DDX_Text(pDX, IDC_NAME, m_strName);
	DDX_Text(pDX, IDC_VERSION, Version);
	//Commented purposely 
	//DDX_Text(pDX, IDC_VERSION, m_strVersion);
	//}}AFX_DATA_MAP
}

