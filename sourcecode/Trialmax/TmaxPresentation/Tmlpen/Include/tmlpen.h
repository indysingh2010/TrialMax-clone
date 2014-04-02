//==============================================================================
//
// File Name:	tmlpen.h
//
// Description:	This file contains the declaration of the CTMLpenCtrl class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	01-02-99	1.00		Original Release
//==============================================================================
#if !defined(AFX_TMLPEN_H__52B397A3_A291_11D2_8BFC_00802966F8C1__INCLUDED_)
#define AFX_TMLPEN_H__52B397A3_A291_11D2_8BFC_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <handler.h>
#include <objsafe.h>
#include <tmver.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMLpenCtrl : public COleControl
{
	private:

						DECLARE_DYNCREATE(CTMLpenCtrl)

		CErrorHandler	m_Errors;
		CTMVersion		m_tmVersion;
		BOOL			m_bDouble;

	public:
	
						CTMLpenCtrl();
					   ~CTMLpenCtrl();

	protected:

		BOOL			CheckVersion(DWORD dwVersion);
		short			GetKeyState();
		void			GetRegistration();

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
	//{{AFX_VIRTUAL(CTMLpenCtrl)
	public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual void DoPropExchange(CPropExchange* pPX);
	//}}AFX_VIRTUAL

	protected:

	DECLARE_OLECREATE_EX(CTMLpenCtrl)    // Class factory and guid
	DECLARE_OLETYPELIB(CTMLpenCtrl)      // GetTypeInfo
	DECLARE_PROPPAGEIDS(CTMLpenCtrl)     // Property page IDs
	DECLARE_OLECTLTYPE(CTMLpenCtrl)		// Type name and misc status

	//{{AFX_MSG(CTMLpenCtrl)
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnRButtonUp(UINT nFlags, CPoint point);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	public:
	//{{AFX_DISPATCH(CTMLpenCtrl)
	afx_msg short GetVerBuild();
	afx_msg short GetVerMajor();
	afx_msg short GetVerMinor();
	BOOL m_bAutoInit;
	afx_msg void OnAutoInitChanged();
	BOOL m_bEnableErrors;
	afx_msg void OnEnableErrorsChanged();
	BOOL m_bAlwaysOnTop;
	afx_msg void OnAlwaysOnTopChanged();
	afx_msg BSTR GetVerTextLong();
	afx_msg short Initialize();
	afx_msg BSTR GetRegisteredPath();
	afx_msg BSTR GetClassIdString();
	//}}AFX_DISPATCH
	DECLARE_DISPATCH_MAP()

	//	Added in rev 6.1.0
	afx_msg short GetVerQEF();
	afx_msg BSTR GetVerTextShort();
	afx_msg BSTR GetVerBuildDate();

	//{{AFX_EVENT(CTMLpenCtrl)
	void FireMouseClick(short sButton, short sKeyState)
		{FireEvent(eventidMouseClick,EVENT_PARAM(VTS_I2  VTS_I2), sButton, sKeyState);}
	//}}AFX_EVENT
	DECLARE_EVENT_MAP()

	public:
	enum {
	//{{AFX_DISP_ID(CTMLpenCtrl)
	dispidBuild = 1L,
	dispidMajorVer = 2L,
	dispidMinorVer = 3L,
	dispidAutoInit = 4L,
	dispidEnableErrors = 5L,
	dispidAlwaysOnTop = 6L,
	dispidTextVer = 7L,
	dispidInitialize = 8L,
	dispidGetRegisteredPath = 9L,
	dispidGetClassIdString = 10L,
	eventidMouseClick = 1L,
	//}}AFX_DISP_ID
	};
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMLPEN_H__52B397A3_A291_11D2_8BFC_00802966F8C1__INCLUDED_)
