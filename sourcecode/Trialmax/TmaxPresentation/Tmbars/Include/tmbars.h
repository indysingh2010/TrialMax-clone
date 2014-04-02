//==============================================================================
//
// File Name:	tmbars.h
//
// Description:	This file contains the declaration of the CTMBarsCtrl class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-09-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_TMBARS_H__3F4BEF13_C5E5_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_TMBARS_H__3F4BEF13_C5E5_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <handler.h>
#include <tmini.h>
#include <barpage.h>
#include <objsafe.h>
#include <tmver.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

#define IDC_TABS					1111
#define BORDER						5

//	Toolbar configuration page indices
#define DOCUMENT_PAGE				0
#define GRAPHIC_PAGE				1
#define PLAYLIST_PAGE				2
#define LINK_PAGE					3
#define MOVIE_PAGE					4
#define POWERPOINT_PAGE				5
#define MAX_PAGES					6

//	Toolbar configuration page tab labels
#define DOCUMENT_LABEL				"Documents"
#define GRAPHIC_LABEL				"Graphics"
#define PLAYLIST_LABEL				"Playlists"
#define LINK_LABEL					"Links"
#define MOVIE_LABEL					"Movies"
#define POWERPOINT_LABEL			"PowerPoint"

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CBarPage;

class CTMBarsCtrl : public COleControl
{
	private:

						DECLARE_DYNCREATE(CTMBarsCtrl)

		CTMIni			m_Ini;
		CTMVersion		m_tmVersion;
		CBarPage*		m_pPage;
		CBarPage		m_Pages[MAX_PAGES];
		CString			m_Labels[MAX_PAGES];
		CString			m_Sections[MAX_PAGES];
		CTabCtrl*		m_pTabs;
		CErrorHandler	m_Errors;
		int				m_iHeight;
		int				m_iWidth;

	public:
	
						CTMBarsCtrl();
					   ~CTMBarsCtrl();

	protected:

		void			CalculateSize();
		void			GetRegistration();
		void			AddPage(int iPage);
		void			OnTabChange(NMHDR* pnmhdr, LRESULT* pResult);
		void			OnTabChanging(NMHDR* pnmhdr, LRESULT* pResult);
		BOOL			CheckVersion(DWORD dwVersion);

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
	//{{AFX_VIRTUAL(CTMBarsCtrl)
	public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual void DoPropExchange(CPropExchange* pPX);
	virtual BOOL OnSetExtent(LPSIZEL lpSizeL);
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	//}}AFX_VIRTUAL

	protected:

	DECLARE_OLECREATE_EX(CTMBarsCtrl)    // Class factory and guid
	DECLARE_OLETYPELIB(CTMBarsCtrl)      // GetTypeInfo
	DECLARE_PROPPAGEIDS(CTMBarsCtrl)     // Property page IDs
	DECLARE_OLECTLTYPE(CTMBarsCtrl)		// Type name and misc status

	//{{AFX_MSG(CTMBarsCtrl)
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnDestroy();
	afx_msg void OnSetFocus(CWnd* pOldWnd);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	public:
	//{{AFX_DISPATCH(CTMBarsCtrl)
	BOOL m_bAutoInit;
	afx_msg void OnAutoInitChanged();
	afx_msg short GetVerBuild();
	BOOL m_bEnableErrors;
	afx_msg void OnEnableErrorsChanged();
	CString m_strIniFile;
	afx_msg void OnIniFileChanged();
	afx_msg short GetVerMajor();
	afx_msg short GetVerMinor();
	afx_msg BSTR GetVerTextLong();
	afx_msg short Initialize();
	afx_msg short Save();
	afx_msg BSTR GetClassIdString();
	afx_msg BSTR GetRegisteredPath();
	//}}AFX_DISPATCH
	DECLARE_DISPATCH_MAP()

	//	Added in rev 6.1.0
	afx_msg short GetVerQEF();
	afx_msg BSTR GetVerTextShort();
	afx_msg BSTR GetVerBuildDate();

	//{{AFX_EVENT(CTMBarsCtrl)
	//}}AFX_EVENT
	DECLARE_EVENT_MAP()

	public:
	enum {
	//{{AFX_DISP_ID(CTMBarsCtrl)
	dispidAutoInit = 1L,
	dispidBuild = 2L,
	dispidEnableErrors = 3L,
	dispidIniFile = 4L,
	dispidMajorVer = 5L,
	dispidMinorVer = 6L,
	dispidTextVer = 7L,
	dispidInitialize = 8L,
	dispidSave = 9L,
	dispidGetClassIdString = 10L,
	dispidGetRegisteredPath = 11L,
	//}}AFX_DISP_ID
	};
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMBARS_H__3F4BEF13_C5E5_11D3_8177_00802966F8C1__INCLUDED_)
