//==============================================================================
//
// File Name:	sharectl.h
//
// Description:	This file contains the declaration of the CTMShareCtrl class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	04-05-02	1.00		Original Release
//==============================================================================
#if !defined(__SHARECTL_H__)
#define __SHARECTL_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <handler.h>
#include <objsafe.h>
#include <request.h>
#include <response.h>
#include <status.h>
#include <tmver.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define TMSHARE_PRESENTATION_NAME		"Tmshare_Presentation"
#define TMSHARE_MANAGER_NAME			"Tmshare_Manager"

#define WM_ERROR_EVENT					(WM_USER + 1)

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMShareCtrl : public COleControl
{
	private:

							DECLARE_DYNCREATE(CTMShareCtrl)

		CTMVersion			m_tmVersion;
		CRequest			m_reqPresentation;
		CRequest			m_reqManager;
		CResponse			m_resPresentation;
		CResponse			m_resManager;
		CStatus				m_statusPresentation;
		CStatus				m_statusManager;
		SRequest			m_CmdRequest;
		SResponse			m_CmdResponse;
		SStatus				m_Status;
		CErrorHandler		m_Errors;
		CString				m_strAppFileSpec;
		CString				m_strCommandLine;
		UINT				m_uRequestTimer;
		BOOL				m_bInitialized;

		DWORD				m_dwProcessId;
		HANDLE				m_hProcess;
		HWND				m_hMainWnd;

		STARTUPINFO			m_StartupInfo;
		PROCESS_INFORMATION	m_ProcessInfo;

	public:
	
							CTMShareCtrl();
						   ~CTMShareCtrl();

		void				OnTimer(UINT idEvent, DWORD dwTime);
		LONG				OnWMErrorEvent(WPARAM wParam, LPARAM lParam);
	
	protected:

		void				GetRegistration();
		void				StopTimer();
		void				SetCommandLine();
		BOOL				StartTimer();
		short				ReadResponse();
		short				ReadRequest();
		short				ReadStatus();
		short				SetStatus();

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
	//{{AFX_VIRTUAL(CTMShareCtrl)
	public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual void DoPropExchange(CPropExchange* pPX);
	virtual BOOL OnGetDisplayString(DISPID dispid, CString& strValue);
	virtual BOOL OnGetPredefinedStrings(DISPID dispid, CStringArray* pStringArray, CDWordArray* pCookieArray);
	virtual BOOL OnGetPredefinedValue(DISPID dispid, DWORD dwCookie, VARIANT* lpvarOut);
	//}}AFX_VIRTUAL

	protected:

	DECLARE_OLECREATE_EX(CTMShareCtrl)    // Class factory and guid
	DECLARE_OLETYPELIB(CTMShareCtrl)      // GetTypeInfo
	DECLARE_PROPPAGEIDS(CTMShareCtrl)     // Property page IDs
	DECLARE_OLECTLTYPE(CTMShareCtrl)		// Type name and misc status

	//{{AFX_MSG(CTMShareCtrl)
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	public:
	//{{AFX_DISPATCH(CTMShareCtrl)
	afx_msg short GetVerBuild();
	BOOL m_bEnableErrors;
	afx_msg void OnEnableErrorsChanged();
	afx_msg short GetVerMajor();
	afx_msg short GetVerMinor();
	afx_msg BSTR GetVerTextLong();
	short m_sOwner;
	afx_msg void OnOwnerChanged();
	long m_lPeekPeriod;
	afx_msg void OnPeekPeriodChanged();
	long m_lCommand;
	afx_msg void OnCommandChanged();
	long m_lError;
	afx_msg void OnErrorChanged();
	CString m_strMediaId;
	afx_msg void OnMediaIdChanged();
	BOOL m_bEnableAxErrors;
	afx_msg void OnEnableAxErrorsChanged();
	CString m_strBarcode;
	afx_msg void OnBarcodeChanged();
	CString m_strAppFolder;
	afx_msg void OnAppFolderChanged();
	long m_lPrimaryId;
	afx_msg void OnPrimaryIdChanged();
	long m_lSecondaryId;
	afx_msg void OnSecondaryIdChanged();
	long m_lTertiaryId;
	afx_msg void OnTertiaryIdChanged();
	long m_lQuaternaryId;
	afx_msg void OnQuaternaryIdChanged();
	CString m_strSourceFileName;
	afx_msg void OnSourceFileNameChanged();
	CString m_strSourceFilePath;
	afx_msg void OnSourceFilePathChanged();
	CString m_strCaseFolder;
	afx_msg void OnCaseFolderChanged();
	long m_lBarcodeId;
	afx_msg void OnBarcodeIdChanged();
	long m_lDisplayOrder;
	afx_msg void OnDisplayOrderChanged();
	afx_msg BSTR GetRegisteredPath();
	afx_msg BSTR GetClassIdString();
	afx_msg BOOL GetInitialized();
	afx_msg short GetResponse();
	afx_msg short GetRequest();
	afx_msg short SetResponse();
	afx_msg void Terminate();
	afx_msg BOOL IsRunning();
	afx_msg BSTR GetSisterFileSpec();
	afx_msg short Initialize();
	afx_msg short SetRequest(long lWaitResponse);
	afx_msg short Execute();
	//}}AFX_DISPATCH
	DECLARE_DISPATCH_MAP()

	//	Added in rev 6.1.0
	afx_msg short GetVerQEF();
	afx_msg BSTR GetVerTextShort();
	afx_msg BSTR GetVerBuildDate();

	//	Added in rev 6.3.3
	afx_msg void OnPageNumberChanged();
	long m_lPageNumber;
	afx_msg void OnLineNumberChanged();
	short m_sLineNumber;

	afx_msg void AboutBox();

	//{{AFX_EVENT(CTMShareCtrl)
	void FireCommandRequest()
		{FireEvent(eventidCommandRequest,EVENT_PARAM(VTS_NONE));}
	void FireAxError(LPCTSTR lpszMessage)
		{FireEvent(eventidAxError,EVENT_PARAM(VTS_BSTR), lpszMessage);}
	void FireAxDiagnostic(LPCTSTR lpszMethod, LPCTSTR lpszMessage)
		{FireEvent(eventidAxDiagnostic,EVENT_PARAM(VTS_BSTR  VTS_BSTR), lpszMethod, lpszMessage);}
	void FireCommandResponse()
		{FireEvent(eventidCommandResponse,EVENT_PARAM(VTS_NONE));}
	//}}AFX_EVENT
	DECLARE_EVENT_MAP()

	public:
	enum {
	//{{AFX_DISP_ID(CTMShareCtrl)
	dispidBuild = 1L,
	dispidEnableErrors = 2L,
	dispidMajorVer = 3L,
	dispidMinorVer = 4L,
	dispidTextVer = 5L,
	dispidOwner = 6L,
	dispidPeekPeriod = 7L,
	dispidCommand = 8L,
	dispidError = 9L,
	dispidMediaId = 10L,
	dispidEnableAxErrors = 11L,
	dispidBarcode = 12L,
	dispidAppFolder = 13L,
	dispidPrimaryId = 14L,
	dispidSecondaryId = 15L,
	dispidTertiaryId = 16L,
	dispidQuaternaryId = 17L,
	dispidSourceFileName = 18L,
	dispidSourceFilePath = 19L,
	dispidCaseFolder = 20L,
	dispidBarcodeId = 21L,
	dispidDisplayOrder = 22L,
	dispidGetRegisteredPath = 23L,
	dispidGetClassIdString = 24L,
	dispidGetInitialized = 25L,
	dispidGetResponse = 26L,
	dispidGetRequest = 27L,
	dispidSetResponse = 28L,
	dispidTerminate = 29L,
	dispidIsRunning = 30L,
	dispidGetSisterFileSpec = 31L,
	dispidInitialize = 32L,
	dispidSetRequest = 33L,
	dispidExecute = 34L,
	eventidCommandRequest = 1L,
	eventidAxError = 2L,
	eventidAxDiagnostic = 3L,
	eventidCommandResponse = 4L,
	//}}AFX_DISP_ID
	};
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations imsharetely before the previous line.

#endif // !defined(__SHARECTL_H__)

