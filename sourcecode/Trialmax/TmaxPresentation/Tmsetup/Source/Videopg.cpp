//==============================================================================
//
// File Name:	videopg.cpp
//
// Description:	This file contains member functions of the CVideoPage class.
//
// See Also:	videopg.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-29-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmsetap.h>
#include <videopg.h>

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
BEGIN_MESSAGE_MAP(CVideoPage, CSetupPage)
	//{{AFX_MSG_MAP(CVideoPage)
		// NOTE: the ClassWizard will add message map macros here
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CVideoPage::CVideoPage()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CVideoPage::CVideoPage(CWnd* pParent) : CSetupPage(CVideoPage::IDD, pParent)
{
	//{{AFX_DATA_INIT(CVideoPage)
	m_bClearMovie = FALSE;
	m_bClearPlaylist = FALSE;
	m_bResumeMovie = FALSE;
	m_bResumePlaylist = FALSE;
	m_bScaleAVI = FALSE;
	m_bClipsAsPlaylists = FALSE;
	m_bRunToEnd = FALSE;
	m_fMovieStep = 0.0f;
	m_fPlaylistStep = 0.0f;
	m_iVideoSize = 0;
	m_dFrameRate = 30.0;
	m_iVideoPosition = -1;
	m_bSplitScreenDocuments = FALSE;
	m_bSplitScreenGraphics = FALSE;
	m_bSplitScreenPowerPoints = FALSE;
	m_bClassicLinks = FALSE;
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CVideoPage::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the class
//					members and the associated dialog box controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CVideoPage::DoDataExchange(CDataExchange* pDX)
{
	CSetupPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CVideoPage)
	DDX_Check(pDX, IDC_CLEARCLIP, m_bClearMovie);
	DDX_Check(pDX, IDC_CLEARPLAYLIST, m_bClearPlaylist);
	DDX_Check(pDX, IDC_RESUMECLIP, m_bResumeMovie);
	DDX_Check(pDX, IDC_RESUMEPLAYLIST, m_bResumePlaylist);
	DDX_Check(pDX, IDC_SCALEAVICLIPS, m_bScaleAVI);
	DDX_Check(pDX, IDC_CLIPSASPLAYLISTS, m_bClipsAsPlaylists);
	DDX_Check(pDX, IDC_RUNTOEND, m_bRunToEnd);
	DDX_Text(pDX, IDC_CLIPSTEP, m_fMovieStep);
	DDX_Text(pDX, IDC_PLAYLISTSTEP, m_fPlaylistStep);
	DDX_Radio(pDX, IDC_VIDEO4, m_iVideoSize);
	DDX_Text(pDX, IDC_FRAMERATE, m_dFrameRate);
	DDX_Radio(pDX, IDC_UPPERLEFT, m_iVideoPosition);
	DDX_Check(pDX, IDC_SPLITDOCUMENTS, m_bSplitScreenDocuments);
	DDX_Check(pDX, IDC_SPLITGRAPHICS, m_bSplitScreenGraphics);
	DDX_Check(pDX, IDC_SPLITPOWERPOINTS, m_bSplitScreenPowerPoints);
	DDX_Check(pDX, IDC_CLASSIC_LINKS, m_bClassicLinks);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CVideoPage::OnInitDialog()
//
// 	Description:	This function handles initialization of the dialog box
//					controls.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CVideoPage::OnInitDialog() 
{
	//	Perform base class initialization
	CDialog::OnInitDialog();
	
	return TRUE;  
}

//==============================================================================
//
// 	Function Name:	CVideoPage::ReadOptions()
//
// 	Description:	This function is called to read the page options from the
//					ini file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CVideoPage::ReadOptions(CTMIni& rIni)
{
	SVideoOptions Options;

	//	Read the options from the ini file
	rIni.ReadVideoOptions(&Options);

	//	Set the class members
	m_bClearMovie = Options.bClearMovie;
	m_bClearPlaylist = Options.bClearPlaylist;
	m_bResumeMovie = Options.bResumeMovie;
	m_bResumePlaylist = Options.bResumePlaylist;
	m_bScaleAVI = Options.bScaleAVI;
	m_bClipsAsPlaylists = Options.bClipsAsPlaylists;
	m_bRunToEnd = Options.bRunToEnd;
	m_bSplitScreenDocuments = Options.bSplitScreenDocuments;
	m_bSplitScreenGraphics = Options.bSplitScreenGraphics;
	m_bSplitScreenPowerPoints = Options.bSplitScreenPowerPoint;
	m_iVideoSize = Options.iVideoSize;
	m_iVideoPosition = Options.iVideoPosition;
	m_fMovieStep = Options.fMovieStep;
	m_fPlaylistStep = Options.fPlaylistStep;
	m_dFrameRate = Options.dFrameRate;
	m_bClassicLinks = Options.bClassicLinks;

	//	Update the controls
	if(IsWindow(m_hWnd))
		UpdateData(FALSE);
}

//==============================================================================
//
// 	Function Name:	CVideoPage::WriteOptions()
//
// 	Description:	This function is called to write the page options to the
//					ini file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CVideoPage::WriteOptions(CTMIni& rIni)
{
	CString strError;
	SVideoOptions Options;

	//	Refresh the class members
	UpdateData(TRUE);

	//	Is the drawing thickness out of range?
	if((m_dFrameRate < VIDEO_MINIMUM_FRAMERATE) ||
	   (m_dFrameRate > VIDEO_MAXIMUM_FRAMERATE))
	{
		if(m_pErrors)
		{
			strError.Format("%d and %d", VIDEO_MINIMUM_FRAMERATE, 
										 VIDEO_MINIMUM_FRAMERATE);
			m_pErrors->Handle(0, IDS_TMSETUP_INVALIDFRAMERATE, strError);
		}
		return FALSE;
	}

	//	Fill the transfer structure
	Options.bClearMovie = m_bClearMovie;
	Options.bClearPlaylist = m_bClearPlaylist;
	Options.bResumeMovie = m_bResumeMovie;
	Options.bResumePlaylist = m_bResumePlaylist;
	Options.bScaleAVI = m_bScaleAVI;
	Options.bClipsAsPlaylists = m_bClipsAsPlaylists;
	Options.bRunToEnd = m_bRunToEnd;
	Options.bSplitScreenDocuments = m_bSplitScreenDocuments;
	Options.bSplitScreenGraphics = m_bSplitScreenGraphics;
	Options.bSplitScreenPowerPoint = m_bSplitScreenPowerPoints;
	Options.iVideoSize = m_iVideoSize;
	Options.iVideoPosition = m_iVideoPosition;
	Options.fMovieStep = m_fMovieStep;
	Options.fPlaylistStep = m_fPlaylistStep;
	Options.dFrameRate = m_dFrameRate;
	Options.bClassicLinks = m_bClassicLinks;

	//	Write the options to the ini file
	rIni.WriteVideoOptions(&Options);

	return TRUE;
}

