//==============================================================================
//
// File Name:	grabctl.h
//
// Description:	This file contains the declaration of the CTMGrabCtrl class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	12-27-01	1.00		Original Release
//==============================================================================
#if !defined(__GRABCTL_H__)
#define __GRABCTL_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <handler.h>
#include <tmini.h>
#include <objsafe.h>
#include <frame.h>
#include <tmver.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMGrabCtrl : public COleControl
{
	private:

							DECLARE_DYNCREATE(CTMGrabCtrl)

		CFrame				m_Frame;
		CTMVersion			m_tmVersion;
		CTMIni				m_Ini;
		CErrorHandler		m_Errors;

		CString				m_strText;
		RECT				m_rcClient;
		RECT				m_rcTop;
		RECT				m_rcBottom;

	public:
	
							CTMGrabCtrl();
						   ~CTMGrabCtrl();

		void				OnCaptureImage();

	protected:

		void				GetRegistration();

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
	//{{AFX_VIRTUAL(CTMGrabCtrl)
	public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual void DoPropExchange(CPropExchange* pPX);
	virtual BOOL OnGetPredefinedStrings(DISPID dispid, CStringArray* pStringArray, CDWordArray* pCookieArray);
	virtual BOOL OnGetPredefinedValue(DISPID dispid, DWORD dwCookie, VARIANT* lpvarOut);
	virtual BOOL OnGetDisplayString(DISPID dispid, CString& strValue);
	//}}AFX_VIRTUAL

	protected:

	DECLARE_OLECREATE_EX(CTMGrabCtrl)    // Class factory and guid
	DECLARE_OLETYPELIB(CTMGrabCtrl)      // GetTypeInfo
	DECLARE_PROPPAGEIDS(CTMGrabCtrl)     // Property page IDs
	DECLARE_OLECTLTYPE(CTMGrabCtrl)		// Type name and misc status

	//{{AFX_MSG(CTMGrabCtrl)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	public:
	//{{AFX_DISPATCH(CTMGrabCtrl)
	afx_msg short GetVerBuild();
	BOOL m_bEnableErrors;
	afx_msg void OnEnableErrorsChanged();
	afx_msg short GetVerMajor();
	afx_msg short GetVerMinor();
	afx_msg BSTR GetVerTextLong();
	CString m_strIniFile;
	afx_msg void OnIniFileChanged();
	short m_sArea;
	afx_msg void OnAreaChanged();
	BOOL m_bSilent;
	afx_msg void OnSilentChanged();
	short m_sHotkey;
	afx_msg void OnHotkeyChanged();
	short m_sCancelKey;
	afx_msg void OnCancelKeyChanged();
	afx_msg BSTR GetRegisteredPath();
	afx_msg BSTR GetClassIdString();
	afx_msg short Initialize();
	afx_msg short Capture();
	afx_msg short Stop();
	afx_msg short Save(LPCTSTR pszName, short iFormat, short iBitsPerPixel, short iQuality, short iModify);
	//}}AFX_DISPATCH
	DECLARE_DISPATCH_MAP()

	//	Added in rev 6.1.0
	afx_msg short GetVerQEF();
	afx_msg BSTR GetVerTextShort();
	afx_msg BSTR GetVerBuildDate();

	BOOL m_bOneShot;
	afx_msg void OnOneShotChanged();

	afx_msg void AboutBox();

	//{{AFX_EVENT(CTMGrabCtrl)
	void FireCaptured()
		{FireEvent(eventidCaptured,EVENT_PARAM(VTS_NONE));}
	//}}AFX_EVENT
	DECLARE_EVENT_MAP()

	public:
	enum {
	//{{AFX_DISP_ID(CTMGrabCtrl)
	dispidBuild = 1L,
	dispidEnableErrors = 2L,
	dispidMajorVer = 3L,
	dispidMinorVer = 4L,
	dispidTextVer = 5L,
	dispidIniFile = 6L,
	dispidArea = 7L,
	dispidSilent = 8L,
	dispidHotkey = 9L,
	dispidCancelKey = 10L,
	dispidGetRegisteredPath = 11L,
	dispidGetClassIdString = 12L,
	dispidInitialize = 13L,
	dispidCapture = 14L,
	dispidStop = 15L,
	dispidSave = 16L,
	eventidCaptured = 1L,
	//}}AFX_DISP_ID
	};
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations imgrabtely before the previous line.

#endif // !defined(__GRABCTL_H__)

