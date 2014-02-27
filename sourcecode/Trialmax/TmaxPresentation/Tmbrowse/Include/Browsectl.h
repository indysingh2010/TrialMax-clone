//==============================================================================
//
// File Name:	browsectl.h
//
// Description:	This file contains the declaration of the CTMBrowseCtrl class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	02-09-02	1.00		Original Release
//==============================================================================
#if !defined(__BROWSECTL_H__)
#define __BROWSECTL_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <handler.h>
#include <tmini.h>
#include <objsafe.h>
#include <tmver.h>
//#include <browser.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define TMBROWSE_BROWSER_ID					100

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CWebFrame;

class CTMBrowseCtrl : public COleControl
{
	private:

							DECLARE_DYNCREATE(CTMBrowseCtrl)

		CWebFrame*			m_pFrame;
		CTMIni				m_Ini;
		CTMVersion			m_tmVersion;
		CErrorHandler		m_Errors;

		CString				m_strText;
		RECT				m_rcClient;
		RECT				m_rcTop;
		RECT				m_rcBottom;

	public:
	
							CTMBrowseCtrl();
		virtual			   ~CTMBrowseCtrl();

	protected:

		void				GetRegistration();
		void				RecalcLayout();
		BOOL				FindFile(LPCSTR lpszFilename);

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
	//{{AFX_VIRTUAL(CTMBrowseCtrl)
	public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual void DoPropExchange(CPropExchange* pPX);
	virtual void OnBackColorChanged();
	//}}AFX_VIRTUAL

	protected:

	DECLARE_OLECREATE_EX(CTMBrowseCtrl)    // Class factory and guid
	DECLARE_OLETYPELIB(CTMBrowseCtrl)      // GetTypeInfo
	DECLARE_PROPPAGEIDS(CTMBrowseCtrl)     // Property page IDs
	DECLARE_OLECTLTYPE(CTMBrowseCtrl)		// Type name and misc status

	//{{AFX_MSG(CTMBrowseCtrl)
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	public:
	//{{AFX_DISPATCH(CTMBrowseCtrl)
	BOOL m_bAutoInit;
	afx_msg void OnAutoInitChanged();
	afx_msg short GetVerBuild();
	BOOL m_bEnableErrors;
	afx_msg void OnEnableErrorsChanged();
	afx_msg short GetVerMajor();
	afx_msg short GetVerMinor();
	afx_msg BSTR GetVerTextLong();
	CString m_strIniFile;
	afx_msg void OnIniFileChanged();
	CString m_strFilename;
	afx_msg void OnFilenameChanged();
	afx_msg BSTR GetRegisteredPath();
	afx_msg BSTR GetClassIdString();
	afx_msg short Initialize();
	afx_msg short Load(LPCTSTR lpszFilename);
	//}}AFX_DISPATCH
	DECLARE_DISPATCH_MAP()

	//	Added in rev 6.1.0
	afx_msg short GetVerQEF();
	afx_msg BSTR GetVerTextShort();
	afx_msg BSTR GetVerBuildDate();

	afx_msg void AboutBox();

	public:
	//{{AFX_EVENT(CTMBrowseCtrl)
	void FireLoadComplete(LPCTSTR lpszFilename)
		{FireEvent(eventidLoadComplete,EVENT_PARAM(VTS_BSTR), lpszFilename);}
	//}}AFX_EVENT
	DECLARE_EVENT_MAP()

	public:
	enum {
	//{{AFX_DISP_ID(CTMBrowseCtrl)
	dispidAutoInit = 1L,
	dispidBuild = 2L,
	dispidEnableErrors = 3L,
	dispidMajorVer = 4L,
	dispidMinorVer = 5L,
	dispidTextVer = 6L,
	dispidIniFile = 7L,
	dispidFilename = 8L,
	dispidGetRegisteredPath = 9L,
	dispidGetClassIdString = 10L,
	dispidInitialize = 11L,
	dispidLoad = 12L,
	eventidLoadComplete = 1L,
	//}}AFX_DISP_ID
	};
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations imbrowsetely before the previous line.

#endif // !defined(__BROWSECTL_H__)

