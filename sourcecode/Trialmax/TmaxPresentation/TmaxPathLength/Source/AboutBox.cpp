//==============================================================================
//
// File Name:	AboutBox.cpp
//
// Description:	This file contains member functions of the CAboutBox class 
//
// See Also:	AboutBox.h
//
// Copyright	FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	03-30-2007	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <StdAfx.h>
#include <AboutBox.h>

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
BEGIN_MESSAGE_MAP(CAboutBox, CDialog)
	//{{AFX_MSG_MAP(CAboutBox)
		// No message handlers
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CAboutBox::CAboutBox()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CAboutBox::CAboutBox() : CDialog(CAboutBox::IDD)
{
	//{{AFX_DATA_INIT(CAboutBox)
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CAboutBox::DoDataExchange()
//
// 	Description:	Manages the exchange between child controls and class 
//					members
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAboutBox::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAboutBox)
	//}}AFX_DATA_MAP
}


