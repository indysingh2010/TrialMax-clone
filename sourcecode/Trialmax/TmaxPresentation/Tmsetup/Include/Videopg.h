//==============================================================================
//
// File Name:	videopg.h
//
// Description:	This file contains the declaration of the CVideoPage class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-29-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_VIDEOPG_H__98CB02D9_D4CA_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_VIDEOPG_H__98CB02D9_D4CA_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <setuppg.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define VIDEO_MINIMUM_FRAMERATE		1.0
#define VIDEO_MAXIMUM_FRAMERATE		250.0

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CVideoPage : public CSetupPage
{
	private:

	public:
	
								CVideoPage(CWnd* pParent = 0);

		void					ReadOptions(CTMIni& rIni);
		BOOL					WriteOptions(CTMIni& rIni);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CVideoPage)
	enum { IDD = IDD_VIDEO_PAGE };
	BOOL	m_bClearMovie;
	BOOL	m_bClearPlaylist;
	BOOL	m_bResumeMovie;
	BOOL	m_bResumePlaylist;
	BOOL	m_bScaleAVI;
	BOOL	m_bClipsAsPlaylists;
	BOOL	m_bRunToEnd;
	float	m_fMovieStep;
	float	m_fPlaylistStep;
	int		m_iVideoSize;
	double	m_dFrameRate;
	int		m_iVideoPosition;
	BOOL	m_bSplitScreenDocuments;
	BOOL	m_bSplitScreenGraphics;
	BOOL	m_bSplitScreenPowerPoints;
	BOOL	m_bClassicLinks;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CVideoPage)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(CVideoPage)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_VIDEOPG_H__98CB02D9_D4CA_11D3_8177_00802966F8C1__INCLUDED_)
