//==============================================================================
//
// File Name:	tmpower.h
//
// Description:	This file contains the declaration of the CTMPowerCtrl class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	05-08-99	1.00		Original Release
//==============================================================================
#if !defined(AFX_TMPOWER_H__E2B9D713_0557_11D3_8175_00802966F8C1__INCLUDED_)
#define AFX_TMPOWER_H__E2B9D713_0557_11D3_8175_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <afxcmn.h>
#include <handler.h>
#include <tmini.h>
#include <powerpt.h>
#include <objsafe.h>
#include <tmver.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define TMPOWER_AVIEW_ID		1L
#define TMPOWER_BVIEW_ID		2L

#define POWERPOINT_CAPTION		_T("TMPower Main Window")

#define WM_ERROR_EVENT		    (WM_USER + 1)

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMPowerCtrl : public COleControl
{
	private:
	
							DECLARE_DYNCREATE(CTMPowerCtrl)

		CPowerPoint			m_AView;
		CPowerPoint			m_BView;
		CPowerPoint*		m_pActive;
		CPowerPoint*		m_pLeft;
		CPowerPoint*		m_pRight;
		CTMIni				m_Ini;
		CErrorHandler		m_Errors;
		CTMVersion			m_tmVersion;
		CString				m_strPPVersion;
		CString				m_strPPBuild;
		BOOL				m_bInitialized;
		BOOL				m_bRedraw;
		int					m_iWidth;
		int					m_iHeight;
		RECT				m_rcLView;
		RECT				m_rcRView;
		RECT				m_rcRFrame;
		RECT				m_rcLFrame;
		RECT				m_rcMax;
		HWND				m_hFocusWnd;
		DWORD				m_dwFocusThread;
	
		//	PowerPoint dispatch interfaces
		_Application		m_PPApp;

	public:
	
							CTMPowerCtrl();
						   ~CTMPowerCtrl();

		BOOL				OnEnumWindow(HWND hWnd);
		void				OnPPSlideChange(long lId, long lSlide);
		void				OnPPStateChange(long lId, short sState);
		void				OnPPFileChange(long lId, LPCSTR lpFilename);
		void				OnPPFocus(long lId);
		void				OnPPLoad(long lId);
		BOOL				KillProcessByName(char *szProcessToKill);

		LONG				OnWMErrorEvent(WPARAM wParam, LPARAM lParam);

	protected:

		void				CalcRects();
		void				GetRegistration();
		void				DrawSplitFrame(RECT& rRect);
		void				EraseSplitFrame(RECT& rRect);
		CPowerPoint*		GetView(short sView = -1);
		BOOL				CheckVersion(DWORD dwVersion);
		BOOL				FindFile(LPCSTR lpFile);
		short				PPDetach();
		short				PPAttach();
		short				PPMove();
		short				GetPosition(long lId);
	
	//--------------------------------------------------------------------
	//	Dispatch interface for implementing object safety
	//
	//	This is required to eliminate warnings when the control is invoked
	//	directly from a browser
	DECLARE_INTERFACE_MAP()

	BEGIN_INTERFACE_PART(ObjSafety, IObjectSafety)
		STDMETHOD_(HRESULT, GetInterfaceSafetyOptions) ( 
            /* [in] */ REFIID riid,
            /* [out] */ DWORD __RPC_FAR *pdwSupportedOptions,
            /* [out] */ DWORD __RPC_FAR *pdwEnabledOptions
		);
        
        STDMETHOD_(HRESULT, SetInterfaceSafetyOptions) ( 
            /* [in] */ REFIID riid,
            /* [in] */ DWORD dwOptionSetMask,
            /* [in] */ DWORD dwEnabledOptions
		);
	END_INTERFACE_PART(ObjSafety);
	//--------------------------------------------------------------------

	//	The remainder of this declaration is maintained by Class Wizard

	public:
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTMPowerCtrl)
	public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual void DoPropExchange(CPropExchange* pPX);
	virtual void OnResetState();
	//}}AFX_VIRTUAL

	protected:

	DECLARE_OLECREATE_EX(CTMPowerCtrl)		// Class factory and guid
	DECLARE_OLETYPELIB(CTMPowerCtrl)		// GetTypeInfo
	DECLARE_PROPPAGEIDS(CTMPowerCtrl)		// Property page IDs
	DECLARE_OLECTLTYPE(CTMPowerCtrl)		// Type name and misc status

	//{{AFX_MSG(CTMPowerCtrl)
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnDestroy();
	//afx_msg void OnPaint(CDC *pdc);

	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	//{{AFX_DISPATCH(CTMPowerCtrl)
	BOOL m_bAutoInit;
	afx_msg void OnAutoInitChanged();
	afx_msg short GetVerBuild();
	BOOL m_bEnableErrors;
	afx_msg void OnEnableErrorsChanged();
	afx_msg short GetVerMajor();
	afx_msg short GetVerMinor();
	afx_msg BSTR GetVerTextLong();
	BOOL m_bSplitScreen;
	afx_msg void OnSplitScreenChanged();
	short m_sSplitFrameThickness;
	afx_msg void OnSplitFrameThicknessChanged();
	OLE_COLOR m_lSplitFrameColor;
	afx_msg void OnSplitFrameColorChanged();
	CString m_strRightFile;
	afx_msg void OnRightFileChanged();
	CString m_strLeftFile;
	afx_msg void OnLeftFileChanged();
	BOOL m_bSyncViews;
	afx_msg void OnSyncViewsChanged();
	short m_sActiveView;
	afx_msg void OnActiveViewChanged();
	long m_lStartSlide;
	afx_msg void OnStartSlideChanged();
	BOOL m_bEnableAccelerators;
	afx_msg void OnEnableAcceleratorsChanged();
	BOOL m_bUseSlideId;
	afx_msg void OnUseSlideIdChanged();
	short m_sSaveFormat;
	afx_msg void OnSaveFormatChanged();
	afx_msg short Initialize();
	afx_msg BOOL IsInitialized();
	afx_msg BSTR GetPPVersion();
	afx_msg BSTR GetPPBuild();
	afx_msg short Next(short sView);
	afx_msg short Previous(short sView);
	afx_msg short First(short sView);
	afx_msg short Last(short sView);
	afx_msg long GetCurrentSlide(short sView);
	afx_msg long GetSlideCount(short sView);
	afx_msg short Close();
	afx_msg short Unload(short sView);
	afx_msg BSTR GetFilename(short sView);
	afx_msg long GetBitmap(long pWidth, long pHeight, short sView);
	afx_msg short ShowSnapshot(short sView);
	afx_msg short SaveSlide(LPCTSTR lpFilename, short sView);
	afx_msg short CopySlide(short sView);
	afx_msg void SetData(short sView, long lData);
	afx_msg long GetData(short sView);
	afx_msg short SetFocusWnd(OLE_HANDLE hWnd);
	afx_msg short Show(short sShow);
	afx_msg short GetCurrentState(short sView);
	afx_msg short SetSlide(short sView, long lSlide, short bUseId);
	afx_msg short LoadFile(LPCTSTR lpszFilename, long lSlide, short bUseId, short sView);
	afx_msg long GetSlideNumber(short sView, long lSlideId);
	afx_msg BSTR GetClassIdString();
	afx_msg BSTR GetRegisteredPath();
	afx_msg short GetAnimationCount(short sView);
	afx_msg short GetAnimationIndex(short sView);
	//}}AFX_DISPATCH
	DECLARE_DISPATCH_MAP()

	//	Added in rev 6.0
	BOOL m_bHideTaskBar;
	afx_msg void OnHideTaskBarChanged();
	BOOL m_bEnableAxErrors;
	afx_msg void OnEnableAxErrorsChanged();

	//	Added in rev 6.1.0
	afx_msg short GetVerQEF();
	afx_msg BSTR GetVerTextShort();
	afx_msg BSTR GetVerBuildDate();

	//{{AFX_EVENT(CTMPowerCtrl)
	void FireSelectView(short sView)
		{FireEvent(eventidSelectView,EVENT_PARAM(VTS_I2), sView);}
	void FireFileChanged(LPCTSTR lpszFilename, short sView)
		{FireEvent(eventidFileChanged,EVENT_PARAM(VTS_BSTR  VTS_I2), lpszFilename, sView);}
	void FireSlideChanged(long lSlide, short sView)
		{FireEvent(eventidSlideChanged,EVENT_PARAM(VTS_I4  VTS_I2), lSlide, sView);}
	void FireViewFocus(short sView)
		{FireEvent(eventidViewFocus,EVENT_PARAM(VTS_I2), sView);}
	void FireStateChanged(short sState, short sView)
		{FireEvent(eventidStateChanged,EVENT_PARAM(VTS_I2  VTS_I2), sState, sView);}
	void FireAxError(LPCTSTR lpszMessage)
		{FireEvent(eventidAxError,EVENT_PARAM(VTS_BSTR), lpszMessage);}
	void FireAxDiagnostic(LPCTSTR lpszMethod, LPCTSTR lpszMessage)
		{FireEvent(eventidAxDiagnostic,EVENT_PARAM(VTS_BSTR  VTS_BSTR), lpszMethod, lpszMessage);}
	//}}AFX_EVENT
	DECLARE_EVENT_MAP()

	enum {
	//{{AFX_DISP_ID(CTMPowerCtrl)
	dispidAutoInit = 1L,
	dispidBuild = 2L,
	dispidEnableErrors = 3L,
	dispidMajorVer = 4L,
	dispidMinorVer = 5L,
	dispidTextVer = 6L,
	dispidSplitScreen = 7L,
	dispidSplitFrameThickness = 8L,
	dispidSplitFrameColor = 9L,
	dispidRightFile = 10L,
	dispidLeftFile = 11L,
	dispidSyncViews = 12L,
	dispidActiveView = 13L,
	dispidStartSlide = 14L,
	dispidEnableAccelerators = 15L,
	dispidUseSlideId = 16L,
	dispidSaveFormat = 17L,
	dispidInitialize = 18L,
	dispidIsInitialized = 19L,
	dispidGetPPVersion = 20L,
	dispidGetPPBuild = 21L,
	dispidNext = 22L,
	dispidPrevious = 23L,
	dispidFirst = 24L,
	dispidLast = 25L,
	dispidGetCurrentSlide = 26L,
	dispidGetSlideCount = 27L,
	dispidClose = 28L,
	dispidUnload = 29L,
	dispidGetFilename = 30L,
	dispidGetBitmap = 31L,
	dispidShowSnapshot = 32L,
	dispidSaveSlide = 33L,
	dispidCopySlide = 34L,
	dispidSetData = 35L,
	dispidGetData = 36L,
	dispidSetFocusWnd = 37L,
	dispidShow = 38L,
	dispidGetCurrentState = 39L,
	dispidSetSlide = 40L,
	dispidLoadFile = 41L,
	dispidGetSlideNumber = 42L,
	dispidGetClassIdString = 43L,
	dispidGetRegisteredPath = 44L,
	dispidGetAnimationCount = 45L,
	dispidGetAnimationIndex = 46L,
	eventidSelectView = 1L,
	eventidFileChanged = 2L,
	eventidSlideChanged = 3L,
	eventidViewFocus = 4L,
	eventidStateChanged = 5L,
	eventidAxError = 6L,
	eventidAxDiagnostic = 7L,
	//}}AFX_DISP_ID
	};
public:
	afx_msg void OnPaint();
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMPOWER_H__E2B9D713_0557_11D3_8175_00802966F8C1__INCLUDED)
