//==============================================================================
//
// File Name:	ppmedia.cpp
//
// Description:	This file contains member functions of the CPPMedia class
//
// See Also:	ppmedia.h
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	05-28-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <ppmedia.h>

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
IMPLEMENT_DYNCREATE(CPPMedia, CPropertyPage)

BEGIN_MESSAGE_MAP(CPPMedia, CPropertyPage)
	//{{AFX_MSG_MAP(CPPMedia)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CPPMedia::CPPMedia()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPMedia::CPPMedia() : CPropertyPage(CPPMedia::IDD)
{
	//{{AFX_DATA_INIT(CPPMedia)
	//}}AFX_DATA_INIT

	m_pXmlTrees = 0;
}

//==============================================================================
//
// 	Function Name:	CPPMedia::~CPPMedia()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPMedia::~CPPMedia()
{
}

//==============================================================================
//
// 	Function Name:	CPPMedia::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					dialog box controls and class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPMedia::DoDataExchange(CDataExchange* pDX)
{
	CPropertyPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPPMedia)
	DDX_Control(pDX, IDC_TREE, m_ctrlTree);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CPPMedia::OnInitDialog()
//
// 	Description:	This function handles all WM_INITDIALOG messages
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CPPMedia::OnInitDialog() 
{
	//	Do the base class processing first
	CPropertyPage::OnInitDialog();
	
	//	Initialize the tree control
	m_ctrlTree.Initialize(m_pXmlTrees);

	return TRUE;
}


