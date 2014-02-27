//==============================================================================
//
// File Name:	tmsetup.h
//
// Description:	This file contains the declaration of the CTMSetupCtrl class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-29-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_TMSETUP_H__98CB02D4_D4CA_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_TMSETUP_H__98CB02D4_D4CA_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <handler.h>
#include <tmini.h>
#include <dbasepg.h>
#include <graphpg.h>
#include <textpg.h>
#include <videopg.h>
#include <diagpg.h>
#include <directpg.h>
#include <systempg.h>
#include <aboutpg.h>
#include <setuppg.h>
#include <cappg.h>
#include <rtailpg.h>
#include <objsafe.h>
#include <tmver.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define DATABASE_PAGE		0
#define GRAPHICS_PAGE		1
#define VIDEO_PAGE			2
#define TEXT_PAGE			3
#define DIAGNOSTIC_PAGE		4
#define DIRECTX_PAGE		5
#define ABOUT_PAGE			6
#define SYSTEM_PAGE			7
#define CAPTURE_PAGE		8
#define RINGTAIL_PAGE		9
#define MAX_PAGES			10

#define IDC_TABS			1111
#define BORDER				5

#define DATABASE_TITLE		"Database"
#define GRAPHICS_TITLE		"Graphics"
#define VIDEO_TITLE			"Video"
#define TEXT_TITLE			"Text"
#define DIAGNOSTIC_TITLE	"Diagnostic"
#define DIRECTX_TITLE		"DirectX"
#define ABOUT_TITLE			"About"
#define SYSTEM_TITLE		"System"
#define CAPTURE_TITLE		"Capture"
#define RINGTAIL_TITLE		"RingTail"

#define WM_ERROR_EVENT		(WM_USER + 1)

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMSetupCtrl : public COleControl
{
	private:

							DECLARE_DYNCREATE(CTMSetupCtrl)

		CTMIni				m_Ini;
		CTMVersion			m_tmVersion;
		CErrorHandler		m_Errors;
		int					m_iHeight;
		int					m_iWidth;

		CTabCtrl*			m_pTabs;
		CSetupPage*			m_pPage;
		CAboutPage			m_About;
		CDiagnosticPage		m_Diagnostic;
		CDirectxPage		m_DirectX;
		CSystemPage			m_System;
		CDatabasePage		m_Database;
		CGraphicsPage		m_Graphics;
		CTextPage			m_Text;
		CVideoPage			m_Video;
		CCapturePage		m_Capture;
		CRingtailPage		m_Ringtail;
		CSetupPage*			m_Pages[MAX_PAGES];
		
	public:
	
						CTMSetupCtrl();
					   ~CTMSetupCtrl();

		void			OnTabChange(NMHDR* pnmhdr, LRESULT* pResult);
		void			OnTabChanging(NMHDR* pnmhdr, LRESULT* pResult);

		LONG			OnWMErrorEvent(WPARAM wParam, LPARAM lParam);

		BOOL			GetLocation(LPCSTR lpszClsId, CString& rPath);

	protected:

		BOOL			CheckVersion(DWORD dwVersion);
		BOOL			FindFile(LPCSTR lpszFilename);
		void			GetRegistration();
		void			CalculateSize();
		void			AddPage(int iPage, int iResourceId);
		void			InsertPage(int iPage);
		void			RemovePage(int iPage);
		void			FireAxVersion(int iAxControl);
		void			FireAxVersion(CTMVersion& tmVersion);
		LPSTR			GetPageTitle(int iPage);

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
	//{{AFX_VIRTUAL(CTMSetupCtrl)
	public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual void DoPropExchange(CPropExchange* pPX);
	virtual BOOL OnSetExtent(LPSIZEL lpSizeL);
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	//}}AFX_VIRTUAL

	protected:

	DECLARE_OLECREATE_EX(CTMSetupCtrl)    // Class factory and guid
	DECLARE_OLETYPELIB(CTMSetupCtrl)      // GetTypeInfo
	DECLARE_PROPPAGEIDS(CTMSetupCtrl)     // Property page IDs
	DECLARE_OLECTLTYPE(CTMSetupCtrl)		// Type name and misc status

	//{{AFX_MSG(CTMSetupCtrl)
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnDestroy();
	afx_msg void OnSetFocus(CWnd* pOldWnd);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	public:
	//{{AFX_DISPATCH(CTMSetupCtrl)
	BOOL m_bAutoInit;
	afx_msg void OnAutoInitChanged();
	CString m_strIniFile;
	afx_msg void OnIniFileChanged();
	BOOL m_bEnableErrors;
	afx_msg void OnEnableErrorsChanged();
	afx_msg short GetVerMajor();
	afx_msg short GetVerMinor();
	afx_msg BSTR GetVerTextLong();
	afx_msg short GetVerBuild();
	BOOL m_bAboutPage;
	afx_msg void OnAboutPageChanged();
	BOOL m_bDatabasePage;
	afx_msg void OnDatabasePageChanged();
	BOOL m_bDirectXPage;
	afx_msg void OnDirectXPageChanged();
	BOOL m_bDiagnosticPage;
	afx_msg void OnDiagnosticPageChanged();
	BOOL m_bGraphicsPage;
	afx_msg void OnGraphicsPageChanged();
	BOOL m_bSystemPage;
	afx_msg void OnSystemPageChanged();
	BOOL m_bTextPage;
	afx_msg void OnTextPageChanged();
	BOOL m_bVideoPage;
	afx_msg void OnVideoPageChanged();
	CString m_strAboutName;
	afx_msg void OnAboutNameChanged();
	CString m_strAboutVersion;
	afx_msg void OnAboutVersionChanged();
	CString m_strAboutCopyright;
	afx_msg void OnAboutCopyrightChanged();
	CString m_strAboutPhone;
	afx_msg void OnAboutPhoneChanged();
	CString m_strAboutEmail;
	afx_msg void OnAboutEmailChanged();
	afx_msg short Initialize();
	afx_msg short Save();
	afx_msg short SetActiveFilters(LPCTSTR lpFilters, LPUNKNOWN lpMediaControl);
	afx_msg BSTR GetClassIdString();
	afx_msg BSTR GetRegisteredPath();
	afx_msg void EnumAxVersions();
	//}}AFX_DISPATCH
	DECLARE_DISPATCH_MAP()

	//	Added in rev 5.1
	BOOL m_bCapturePage;
	afx_msg void OnCapturePageChanged();

	//	Added in rev 6.0
	BOOL m_bRingtailPage;
	afx_msg void OnRingtailPageChanged();
	BOOL m_bEnableAxErrors;
	afx_msg void OnEnableAxErrorsChanged();

	//	Added in rev 6.1.0
	public:

	//	Make these accessible to the diagnostics page
	CString m_strPresentationFileSpec;

	protected:

	afx_msg short GetVerQEF();
	afx_msg BSTR GetVerTextShort();
	afx_msg BSTR GetVerBuildDate();
	afx_msg void OnPresentationFileSpecChanged();

	//{{AFX_EVENT(CTMSetupCtrl)
	void FireAxError(LPCTSTR lpszMessage)
		{FireEvent(eventidAxError,EVENT_PARAM(VTS_BSTR), lpszMessage);}
	void FireAxDiagnostic(LPCTSTR lpszMethod, LPCTSTR lpszMessage)
		{FireEvent(eventidAxDiagnostic,EVENT_PARAM(VTS_BSTR  VTS_BSTR), lpszMethod, lpszMessage);}
	void FireAxVersion(LPCTSTR lpszName, LPCTSTR lpszDescription, short sMajorVer, short sMinorVer, short sQEF, short sBuild, LPCTSTR lpszShortText, LPCTSTR lpszLongText, LPCTSTR lpszBuildDate, LPCTSTR lpszClsId, LPCTSTR lpszPath)
		{FireEvent(eventidAxVersion,EVENT_PARAM(VTS_BSTR  VTS_BSTR  VTS_I2  VTS_I2  VTS_I2  VTS_I2  VTS_BSTR  VTS_BSTR  VTS_BSTR VTS_BSTR  VTS_BSTR), lpszName, lpszDescription, sMajorVer, sMinorVer, sQEF, sBuild, lpszShortText, lpszLongText, lpszBuildDate, lpszClsId, lpszPath);}
	//}}AFX_EVENT
	DECLARE_EVENT_MAP()

	public:
	enum {
	//{{AFX_DISP_ID(CTMSetupCtrl)
	dispidAutoInit = 1L,
	dispidIniFile = 2L,
	dispidEnableErrors = 3L,
	dispidMajorVer = 4L,
	dispidMinorVer = 5L,
	dispidTextVer = 6L,
	dispidBuild = 7L,
	dispidAboutPage = 8L,
	dispidDatabasePage = 9L,
	dispidDirectXPage = 10L,
	dispidDiagnosticPage = 11L,
	dispidGraphicsPage = 12L,
	dispidSystemPage = 13L,
	dispidTextPage = 14L,
	dispidVideoPage = 15L,
	dispidAboutName = 16L,
	dispidAboutVersion = 17L,
	dispidAboutCopyright = 18L,
	dispidAboutPhone = 19L,
	dispidAboutEmail = 20L,
	dispidInitialize = 21L,
	dispidSave = 22L,
	dispidSetActiveFilters = 23L,
	dispidGetClassIdString = 24L,
	dispidGetRegisteredPath = 25L,
	dispidEnumAxVersions = 26L,
	eventidAxError = 1L,
	eventidAxDiagnostic = 2L,
	eventidAxVersion = 3L,
	//}}AFX_DISP_ID
	};
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMSETUP_H__98CB02D4_D4CA_11D3_8177_00802966F8C1__INCLUDED_)
