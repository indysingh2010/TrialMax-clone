//==============================================================================
//
// File Name:	powerpt.h
//
// Description:	This file contains the declaration of the CPowerPoint class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	06-26-99	1.00		Original Release
//==============================================================================
#if !defined(AFX_POWERPT_H__85B96742_2C81_11D3_8176_00802966F8C1__INCLUDED_)
#define AFX_POWERPT_H__85B96742_2C81_11D3_8176_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <resource.h>
#include <msppt8.h>
#include <msppenum.h>
#include <handler.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
//#define _USE_LOCK 1

#define RELEASE_INTERFACE(x) { if (x) delete x; x = 0; }

#define CHECKWND_TIMER		1
#define FOCUS_TIMER			2
#define UPDATE_TIMER		3
#define ASSIGN_NEW_TIMER	4

#define ASSIGN_NEW_TIMEOUT	10000	//	Time (ms) allowed for new file to settle
									//	before advancing to specified slide

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	Forward declarations
class CTMPowerCtrl;
class CSnapshot;

class CPowerPoint : public CDialog
{
	private:

		CTMPowerCtrl*			m_pControl;
		CErrorHandler*			m_pErrors;
		CSnapshot*				m_pSnapshot;
		long					m_lId;
		HWND					m_hPPWnd;
		HWND					m_hSSWnd;
		HWND					m_hSSParent;
public:
		HWND					m_hNSSWnd;
		float					m_fPPVersion;
private:
		HWND					m_hNSSParent;
		RECT					m_rcMax;
		long					m_lSlides;
		long					m_lCurrent;
		long					m_lSetSlide;
		long					m_lUserData;
		long					m_lAnimations;
		long					m_lAnimation;
		short					m_sPPState;
private:
		COLORREF				m_crBackground;
		CBrush*					m_pBackground;
		CString					m_strFilename;
		CString					m_strNIFilename;
		CString					m_strEnum;
		BOOL					m_bEnableAccelerators;
		BOOL					m_bCheckWndError;
		BOOL					m_bHideTaskBar;
		UINT					m_uFocusTimer;
		DWORD					m_dwAssignNew;
		
		//	PowerPoint dispatch interfaces
		_Application*			m_pIApp;
		_Presentation*			m_pIPresentation;
		Presentations*			m_pIPresentations;
		Slides*					m_pISlides;
		SlideShowSettings*		m_pISettings;
		SlideShowWindow*		m_pIWindow;
		SlideShowView*			m_pIView;
		_Slide*					m_pISlide;

		_Presentation*			m_pNIPresentation;
		Presentations*			m_pNIPresentations;
		Slides*					m_pNISlides;
		SlideShowSettings*		m_pNISettings;
		SlideShowWindow*		m_pNIWindow;
		SlideShowView*			m_pNIView;

		CRITICAL_SECTION		m_Lock;

	public:
	
								CPowerPoint();  
		virtual				   ~CPowerPoint();

		//	Public Access
		HWND					GetPPWnd(){ return m_hPPWnd; }
		_Application*			GetPPApp(){ return m_pIApp; }
		LPCSTR					GetFilename(){ return m_strFilename; }
		BOOL					GetEnableAccelerators(){ return m_bEnableAccelerators; }
		BOOL					GetHideTaskBar(){ return m_bHideTaskBar; }
		CErrorHandler*			GetErrorHandler(){ return m_pErrors; }
		short					GetState();
		short					GetAnimationCount(){ return (short)m_lAnimations; }
		short					GetAnimationIndex(){ return (short)m_lAnimation; }
		void					SetPPApp(_Application* pApp){ m_pIApp = pApp; }
		void					SetPPWnd(HWND hWnd){ m_hPPWnd = hWnd; }
		void					SetErrorHandler(CErrorHandler* pErrors){ m_pErrors = pErrors; }
		void					SetMaxRect(RECT& rRect);
		void					AttachWnd(HWND hWnd);
		void					SetSnapshot();
		void					CheckWnd();
		void					SetUserData(long lData){ m_lUserData = lData; }
		void					SetVersion(float fVersion){ m_fPPVersion = fVersion; }
		void					SetEnableAccelerators(BOOL bEnable){ m_bEnableAccelerators = bEnable; }
		void					SetHideTaskBar(BOOL bHide){ m_bHideTaskBar = bHide; }
		void					Redraw();
		short					SetFilename(LPCSTR lpFilename, long lStart, BOOL bSlideId);
		short					Unload();
		short					Next();
		short					Previous();
		short					First();
		short					Last();
		short					SetSlide(long lSlide, BOOL bSlideId);
		short					SaveSlide(LPCTSTR lpFilename, int iFormat);
		short					CopySlide();
		long					GetSlideCount(){ return m_lSlides; }
		long					GetUserData(){ return m_lUserData; }
		long					GetCurrentSlide(){ return m_lCurrent; }
		long					GetSlideNumber(long lSlideId, Slides* pISlides = 0);
		void					SetBackColor(COLORREF crBackground);
		BOOL					Create(CTMPowerCtrl* pControl, long lId);
		BOOL					FindFile(LPCSTR lpFilespec);
		BOOL					OnEnumWindow(HWND hWnd);
		BOOL					MyEnumDesktopWindows(HWND hWnd);
		HBITMAP					GetDDBitmap(int* pWidth, int* pHeight);
		CSnapshot*				GetSnapshot(BOOL bPopup); 

		void					Lock();
		void					Unlock();

	protected:

		void					Resize();
		void					ReleaseActive();
		void					ReleaseNew();
		void					AssignNew(long lStart);
		void					SetCurrent();
		void					StartTimers();
		void					StopTimers();
		void					OnChangeSlide();
		void					HandleException(COleException* pException);
		void					HandleException(COleDispatchException* pException);
		short					UpdateState();
		long					GetAnimations();
		BOOL					SetInitialSlide(long lSlide);
		BOOL					GetAnimated(Shape* pIShape);
		BOOL					GetSSHandle(LPCTSTR lpName);
		_Slide*					GetISlide();
		_Presentation*			AddPresentation();
		SlideRange*				GetSlideRange(long lFirst, long lLast);
		
	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Dialog Data
	//{{AFX_DATA(CPowerPoint)
	enum { IDD = IDD_POWERPOINT };
		// NOTE: the ClassWizard will add data members here
	//}}AFX_DATA

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CPowerPoint)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:

	// Generated message map functions
	//{{AFX_MSG(CPowerPoint)
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg void OnTimer(UINT nIDEvent);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_POWERPT_H__85B96742_2C81_11D3_8176_00802966F8C1__INCLUDED_)
