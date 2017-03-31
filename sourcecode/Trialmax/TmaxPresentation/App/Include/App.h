//==============================================================================
//
// File Name:	app.h
//
// Description:	This file contains the declaration of the CApp class. This
//				is the TrialMax II application.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	08-17-97	1.00		Original Release
//==============================================================================
#if !defined(__APP_H__)
#define __APP_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include <resource.h>
#include <tmcmdlin.h>
#include <dao36.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Keyboard hook states
#define WAITING_START					0
#define WAITING_VIRTUAL_CODE			1
#define WAITING_MEDIA_DELIMITER			2
#define WAITING_SECONDARY_DELIMITER		3
#define WAITING_PAGE_NUMBER				4
#define WAITING_RETURN					5

#define TRIALMAX_WINDOW_TITLE			"TmaxPresentation Viewer"

//	This value is used to locate previous instances of the application
//
//	This value must be different than that used by TmaxManager
#define TMAXPRESENTATION_INSTANCE_KEY	0x87654321L

//	Maximum length of keyboard buffer
#define MAXLEN_KBBUFFER					1024

//	Custom message identifiers
#define WM_MOUSEMODE					(WM_USER + 1)
#define WM_NEWINSTANCE					(WM_USER + 2)
#define WM_GRABFOCUS					(WM_USER + 3)

#define ASSERT_RET_VOID(x)				ASSERT(x); if(!(x)) return;
#define ASSERT_RET_BOOL(x)				ASSERT(x); if(!(x)) return FALSE;

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------
extern const WORD		_wVerMajor;
extern const WORD		_wVerMinor;

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CMainFrame;
class CSplashBox;

class CApp : public CWinApp
{
	private:
		char					m_FileName[200];
		char					m_temp[200];
		char					m_szKey[2];
		char					m_cPrimary;
		char					m_cAlternate;
		char					m_cVK;
		BOOL					m_bHook;
		BOOL					m_bEscapeHook;
		BOOL					m_bMouseHook;
		CString					m_strKBBuffer;
		CString					m_strAppFolder;
		CMainFrame*				m_pFrame;
		COleTemplateServer		m_Server;
		short					m_sHookState;
		CSplashBox*				m_pSplashBox;
		BOOL					m_bSilent;
		BOOL					m_bAlternate;
		BOOL					m_bDualMonitors;
		int						m_iPrimaryWidth;
		int						m_iPrimaryHeight;
		int						m_iSecondaryWidth;
		int						m_iSecondaryHeight;
		POINTL					SecondaryDisplayOffset; // This stores the location of the secondary monitor
		
		HANDLE					m_hFFmpeg;
		static LRESULT CALLBACK LowLevelKeyboardProc(int nCode, WPARAM wParam, LPARAM lParam);
	public:
	
		CTMCommandLineInfo		m_TMCmdLineInfo;

								CApp();
								~CApp();

		void					EnableHook(BOOL bEnable);
		void					EnableMouseHook(BOOL bEnable);
		void					EnableEscapeHook(BOOL bEnable);
		void					ResetHook();
		void					DoSplashBox(BOOL bShow);
		BOOL					GetSilent(){ return m_bSilent; }
		BOOL					GetDualMonitors(){ return m_bDualMonitors; }
		LPCSTR					GetAppFolder(){ return m_strAppFolder; }

		void					LockInstance(HWND hMainWnd);
		void					UnlockInstance();
		BOOL					bSetDisplay;
		POINTL					GetSecondaryDisplayDimensions();
		POINTL					GetPrimaryDisplayDimensions();
		POINTL					GetSecondaryDisplayOffset(){ return SecondaryDisplayOffset; }
		void					StartRecordingFFMpeg(char FileName[]);
		BOOL					CheckFolderAccess(CString Folder);
	protected:

		void					ActivateInstance(HWND hMainWnd);
		void					StoreCommandLine(CTMCommandLineInfo& rCmdLine);
		BOOL					GetMonitorInfo();
		HWND					GetPrevInstance();

		static LONG				GetInstanceKey(){ return TMAXPRESENTATION_INSTANCE_KEY; }
		static BOOL CALLBACK	OnEnumWindow(HWND hWnd, LPARAM lpParam);
		void					InitWmGesture(HWND hWnd);

	
	//	Class Wizard maintained
	
	//{{AFX_VIRTUAL(CApp)
	public:
	virtual BOOL InitInstance();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	virtual int ExitInstance();
	//}}AFX_VIRTUAL

	//{{AFX_MSG(CApp)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
	
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(__APP_H__)
