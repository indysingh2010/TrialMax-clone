//==============================================================================
//
// File Name:	systempg.cpp
//
// Description:	This file contains member functions of the CSystemPage class.
//
// See Also:	systempg.h
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
#include <systempg.h>

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
BEGIN_MESSAGE_MAP(CSystemPage, CSetupPage)
	//{{AFX_MSG_MAP(CSystemPage)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CSystemPage::CSystemPage()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CSystemPage::CSystemPage(CWnd* pParent) : CSetupPage(CSystemPage::IDD, pParent)
{
	//{{AFX_DATA_INIT(CSystemPage)
	m_iAnimation = -1;
	m_iImage = -1;
	m_iPlaylist = -1;
	m_iPowerPoint = -1;
	m_iTreatment = -1;
	m_iShow = -1;
	m_bOptimizeVideo = FALSE;
	m_bDualMonitors = FALSE;
	m_bOptimizeTablet = FALSE;
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CSystemPage::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the class
//					members and the associated dialog box controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSystemPage::DoDataExchange(CDataExchange* pDX)
{
	CSetupPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CSystemPage)
	DDX_Radio(pDX, IDC_ANIMATION_ID, m_iAnimation);
	DDX_Radio(pDX, IDC_IMAGE_ID, m_iImage);
	DDX_Radio(pDX, IDC_PLAYLIST_ID, m_iPlaylist);
	DDX_Radio(pDX, IDC_POWERPOINT_ID, m_iPowerPoint);
	DDX_Radio(pDX, IDC_TREATMENT_ID, m_iTreatment);
	DDX_Radio(pDX, IDC_SHOW_ID, m_iShow);
	DDX_Check(pDX, IDC_OPTIMIZE_VIDEO, m_bOptimizeVideo);
	DDX_Check(pDX, IDC_DUAL_MONITORS, m_bDualMonitors);
	DDX_Check(pDX, IDC_OPTIMIZE_TABLET, m_bOptimizeTablet);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CSystemPage::ReadOptions()
//
// 	Description:	This function is called to read the page options from the
//					ini file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSystemPage::ReadOptions(CTMIni& rIni)
{
	SSystemOptions Options;

	//	Read the options from the ini file
	rIni.ReadSystemOptions(&Options);

	//	Set the class members
	m_iImage = Options.iImageSecondary;
	m_iAnimation = Options.iAnimationSecondary;
	m_iPlaylist = Options.iPlaylistSecondary;
	m_iPowerPoint = Options.iPowerPointSecondary;
	m_iShow = Options.iCustomShowSecondary;
	m_iTreatment = Options.iTreatmentTertiary;
	m_bOptimizeVideo = Options.bOptimizeVideo;
	m_bDualMonitors = Options.bDualMonitors;
	m_bOptimizeTablet = Options.bOptimizeTablet;


	//	Update the controls
	if(IsWindow(m_hWnd))
		UpdateData(FALSE);
}

//==============================================================================
//
// 	Function Name:	CSystemPage::WriteOptions()
//
// 	Description:	This function is called to write the page options to the
//					ini file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CSystemPage::WriteOptions(CTMIni& rIni)
{

	SSystemOptions Options;
	
	//	Refresh the class members
	UpdateData(TRUE);

	//	Fill the transfer structure
	Options.iAnimationSecondary = m_iAnimation;
	Options.iImageSecondary = m_iImage;
	Options.iPlaylistSecondary = m_iPlaylist;
	Options.iPowerPointSecondary = m_iPowerPoint;
	Options.iCustomShowSecondary = m_iShow;
	Options.iTreatmentTertiary = m_iTreatment;
	Options.bOptimizeVideo = m_bOptimizeVideo;
	Options.bDualMonitors = m_bDualMonitors;
	Options.bOptimizeTablet = m_bOptimizeTablet;
	//	Write the options to the ini file
	rIni.WriteSystemOptions(&Options);

	return TRUE;
}

