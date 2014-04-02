//==============================================================================
//
// File Name:	vidprops.cpp
//
// Description:	This file contains member functions of the CVideoProperties 
//				class.
//
// Functions:   CVideoProperties::CVideoProperties()
//				CVideoProperties::DoDataExchange()
//
// See Also:	vidprops.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	07-03-98	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmmovie.h>
#include <vidprops.h>

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
BEGIN_MESSAGE_MAP(CVideoProperties, CDialog)
	//{{AFX_MSG_MAP(CVideoProperties)
		// NOTE: the ClassWizard will add message map macros here
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CVideoProperties::CVideoProperties
//
// 	Description:	This is the constructor for CVideoProperties objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CVideoProperties::CVideoProperties(CWnd* pParent)
				 :CDialog(CVideoProperties::IDD, pParent)
{
	//{{AFX_DATA_INIT(CVideoProperties)
	m_strAspectRatio = _T("");
	m_strFilename = _T("");
	m_strFrames = _T("");
	m_strHeight = _T("");
	m_strRate = _T("");
	m_strTime = _T("");
	m_strWidth = _T("");
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CVideoProperties::DoDataExchange
//
// 	Description:	This function manages the exchange of data between the 
//					dialog box and its controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CVideoProperties::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CVideoProperties)
	DDX_Text(pDX, IDC_VP_ASPECT, m_strAspectRatio);
	DDX_Text(pDX, IDC_VP_FILENAME, m_strFilename);
	DDX_Text(pDX, IDC_VP_FRAMES, m_strFrames);
	DDX_Text(pDX, IDC_VP_HEIGHT, m_strHeight);
	DDX_Text(pDX, IDC_VP_RATE, m_strRate);
	DDX_Text(pDX, IDC_VP_TIME, m_strTime);
	DDX_Text(pDX, IDC_VP_WIDTH, m_strWidth);
	//}}AFX_DATA_MAP
}



