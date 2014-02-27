//==============================================================================
//
// File Name:	tmxml.h
//
// Description:	This file contains the declaration of the CTMXmlCtrl class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	03-02-01	1.00		Original Release
//==============================================================================
#if !defined(AFX_TMXML6CTL_H__2CC295D3_27A3_11D5_8F0A_00802966F8C1__INCLUDED_)
#define AFX_TMXML6CTL_H__2CC295D3_27A3_11D5_8F0A_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <handler.h>
#include <xmlframe.h>
#include <objsafe.h>
#include <urlmon.h>
#include <tmver.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define TMXML_LOAD_SOURCE_TIMER		1

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMXmlCtrl : public COleControl
{
	private:

								DECLARE_DYNCREATE(CTMXmlCtrl)

		CXmlFrame*				m_pFrame;
		CTMVersion				m_tmVersion;
		CErrorHandler			m_Errors;
		CString					m_strSource;
		CString					m_strFolder;

	public:
	
								CTMXmlCtrl();
							   ~CTMXmlCtrl();

		LPCSTR					GetFolder(){ return m_strFolder; }

	protected:

		BOOL					CheckVersion(DWORD dwVersion);
		void					GetRegistration();
	
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
	//{{AFX_VIRTUAL(CTMXmlCtrl)
	public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual void DoPropExchange(CPropExchange* pPX);
	virtual void OnBackColorChanged();
	//}}AFX_VIRTUAL

	protected:

	DECLARE_OLECREATE_EX(CTMXmlCtrl)    // Class factory and guid
	DECLARE_OLETYPELIB(CTMXmlCtrl)      // GetTypeInfo
	DECLARE_PROPPAGEIDS(CTMXmlCtrl)     // Property page IDs
	DECLARE_OLECTLTYPE(CTMXmlCtrl)		// Type name and misc status

	//{{AFX_MSG(CTMXmlCtrl)
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnTimer(UINT nIDEvent);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	public:
	//{{AFX_DISPATCH(CTMXmlCtrl)
	afx_msg short GetVerBuild();
	BOOL m_bEnableErrors;
	afx_msg void OnEnableErrorsChanged();
	afx_msg short GetVerMajor();
	afx_msg short GetVerMinor();
	afx_msg BSTR GetVerTextLong();
	BOOL m_bAutoInit;
	afx_msg void OnAutoInitChanged();
	CString m_strFilename;
	afx_msg void OnFilenameChanged();
	BOOL m_bFloatPrintProgress;
	afx_msg void OnFloatPrintProgressChanged();
	afx_msg short Initialize();
	afx_msg short LoadFile(LPCTSTR lpFilename);
	afx_msg BSTR GetClassIdString();
	afx_msg BSTR GetRegisteredPath();
	afx_msg short loadDocument(LPCTSTR lpszUrl);
	afx_msg short jumpToPage(LPCTSTR lpszPageId);
	//}}AFX_DISPATCH
	DECLARE_DISPATCH_MAP()

	//	Added in rev 6.1.0
	afx_msg short GetVerQEF();
	afx_msg BSTR GetVerTextShort();
	afx_msg BSTR GetVerBuildDate();

	afx_msg void AboutBox();

	//{{AFX_EVENT(CTMXmlCtrl)
	//}}AFX_EVENT
	DECLARE_EVENT_MAP()

	public:
	enum {
	//{{AFX_DISP_ID(CTMXmlCtrl)
	dispidBuild = 1L,
	dispidEnableErrors = 2L,
	dispidMajorVer = 3L,
	dispidMinorVer = 4L,
	dispidTextVer = 5L,
	dispidAutoInit = 6L,
	dispidFilename = 7L,
	dispidDockPrintProgress = 8L,
	dispidInitialize = 9L,
	dispidLoadFile = 10L,
	dispidGetClassIdString = 11L,
	dispidGetRegisteredPath = 12L,
	dispidLoadDocument = 13L,
	dispidJumpToPage = 14L,
	//}}AFX_DISP_ID
	};
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMXML6CTL_H__2CC295D3_27A3_11D5_8F0A_00802966F8C1__INCLUDED)
