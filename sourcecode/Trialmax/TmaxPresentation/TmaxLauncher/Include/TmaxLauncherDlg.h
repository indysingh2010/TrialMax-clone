//==============================================================================
//
// File Name:	TmaxLauncherDlg.h
//
// Description:	This file contains the declaration of the TmaxLaucherDlg class
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	02-07-2007	1.00		Original Release
//==============================================================================
#if !defined(__TMAXLAUNCHERDLG_H__)
#define __TMAXLAUNCHERDLG_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <Registry.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define DEFAULT_FTI_INSTALL_FOLDER		"C:\\Program Files\\FTI\\"
#define DEFAULT_VIDEO_VIEWER_FOLDER		"TrialMax 6 Video Viewer"
#define DEFAULT_VIDEO_VIEWER_EXECUTABLE	"TmaxVideo.exe"
#define DEFAULT_VIDEO_STANDARD_SETUP	"trialmax_video_6.2_standard_setup.exe"
#define DEFAULT_VIDEO_CUSTOM_SETUP		"trialmax_video_6.2_custom_setup.exe"
#define DEFAULT_TRANSCRIPT_EXTENSION	"xmlt"
#define DEFAULT_SCRIPT_EXTENSION		"xmlv"
#define DEFAULT_VIDEO_EXTENSION			"mpg"

#define LAUNCH_TIMER_ID					1

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CSplashWnd;

class CTmaxLauncherDlg : public CDialog
{
	private:

		CSplashWnd*			m_pwndSplash;

		STmaxProductInfo	m_tmaxProductInfo;
		CString				m_strExecutableFolder;
		CString				m_strExecutableFileSpec;
		CString				m_strSetupFileSpec;
		BOOL				m_bInstalled;

	public:
	
							CTmaxLauncherDlg(CWnd* pParent = NULL);
						   ~CTmaxLauncherDlg();

	protected:

		void				OnError(LPCSTR lpszFormat, ...);
		void				OnError(UINT uId, ...);
		BOOL				Launch();
		BOOL				Install();
		BOOL				Execute(LPCSTR lpszFileSpec, LPCSTR lpszCmdLine, BOOL bWait);
		BOOL				GetExecutableFileSpec();
		CString				GetSourceFileSpec();
		CString				GetVideoPath(LPCSTR lpszSourceFileSpec);


	public:

	// Dialog Data
	//{{AFX_DATA(CTmaxLauncherDlg)
	enum { IDD = IDD_TMAXLAUNCHER_DIALOG };
	CString	m_strSourceFile;
	CString	m_strVideoPath;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmaxLauncherDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:
	HICON m_hIcon;

	// Generated message map functions
	//{{AFX_MSG(CTmaxLauncherDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg void OnClickLaunch();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__TMAXLAUNCHERDLG_H__)
