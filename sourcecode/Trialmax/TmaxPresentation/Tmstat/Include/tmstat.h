//==============================================================================
//
// File Name:	tmstat.h
//
// Description:	This file contains the declaration of the CTMStatCtrl class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	01-02-99	1.00		Original Release
//==============================================================================
#if !defined(AFX_TMSTAT_H__647E4E64_C20F_11D2_8C24_00802966F8C1__INCLUDED_)
#define AFX_TMSTAT_H__647E4E64_C20F_11D2_8C24_00802966F8C1__INCLUDED_

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
#define TMSTAT_BORDER	5

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CTMStatCtrl : public COleControl
{
	private:

						DECLARE_DYNCREATE(CTMStatCtrl)

		CErrorHandler	m_Errors;
		CTMVersion		m_tmVersion;
		RECT			m_rcText;
		RECT			m_rcClient;
		CFont*			m_pFont;
		BOOL			m_bShowPageLine;
		BOOL			m_bShowPlaylist;
		BOOL			m_bShowLink;
		CString			m_strPlaylistId;
		CString			m_strLinkId;
		CString			m_strText;
		CString			m_strPlaylistTime;
		CString			m_strElapsedPlaylist;
		CString			m_strRemainingPlaylist;
		CString			m_strDesignationTime;
		CString			m_strElapsedDesignation;
		CString			m_strRemainingDesignation;

	public:
	
						CTMStatCtrl();
					   ~CTMStatCtrl();

	protected:

		BOOL			CheckVersion(DWORD dwVersion);
		void			DrawText(CDC* pdc, LPCSTR lpText);
		void			Recalc(BOOL bRedraw);
		void			FormatText();
		void			TimeToStr(long lTime, CString& strTime);
		void			ConvertPlaylistTimes();
		void			ConvertDesignationTimes();
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
	//{{AFX_VIRTUAL(CTMStatCtrl)
	public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual void DoPropExchange(CPropExchange* pPX);
	virtual BOOL OnSetExtent(LPSIZEL lpSizeL);
	//}}AFX_VIRTUAL

	protected:

	DECLARE_OLECREATE_EX(CTMStatCtrl)    // Class factory and guid
	DECLARE_OLETYPELIB(CTMStatCtrl)      // GetTypeInfo
	DECLARE_PROPPAGEIDS(CTMStatCtrl)     // Property page IDs
	DECLARE_OLECTLTYPE(CTMStatCtrl)		// Type name and misc status

	//{{AFX_MSG(CTMStatCtrl)
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	public:
	//{{AFX_DISPATCH(CTMStatCtrl)
	BOOL m_bAutoInit;
	afx_msg void OnAutoInitChanged();
	afx_msg short GetVerBuild();
	afx_msg short GetVerMajor();
	afx_msg short GetVerMinor();
	BOOL m_bEnableErrors;
	afx_msg void OnEnableErrorsChanged();
	BOOL m_bAutosizeFont;
	afx_msg void OnAutosizeFontChanged();
	CString m_strStatusText;
	afx_msg void OnStatusTextChanged();
	short m_sLeftMargin;
	afx_msg void OnLeftMarginChanged();
	short m_sRightMargin;
	afx_msg void OnRightMarginChanged();
	short m_sTopMargin;
	afx_msg void OnTopMarginChanged();
	short m_sBottomMargin;
	afx_msg void OnBottomMarginChanged();
	afx_msg BSTR GetVerTextLong();
	double m_dPlaylistTime;
	afx_msg void OnPlaylistTimeChanged();
	double m_dElapsedPlaylist;
	afx_msg void OnElapsedPlaylistChanged();
	double m_dDesignationTime;
	afx_msg void OnDesignationTimeChanged();
	double m_dElapsedDesignation;
	afx_msg void OnElapsedDesignationChanged();
	short m_sMode;
	afx_msg void OnModeChanged();
	long m_lDesignationCount;
	afx_msg void OnDesignationCountChanged();
	long m_lDesignationIndex;
	afx_msg void OnDesignationIndexChanged();
	long m_lTextLine;
	afx_msg void OnTextLineChanged();
	long m_lTextPage;
	afx_msg void OnTextPageChanged();
	afx_msg short Initialize();
	afx_msg short SetPlaylistInfo(long lInfo);
	afx_msg BSTR GetClassIdString();
	afx_msg BSTR GetRegisteredPath();
	//}}AFX_DISPATCH
	DECLARE_DISPATCH_MAP()

	//	Added in rev 6.1.0
	afx_msg short GetVerQEF();
	afx_msg BSTR GetVerTextShort();
	afx_msg BSTR GetVerBuildDate();

	afx_msg void AboutBox();

	//{{AFX_EVENT(CTMStatCtrl)
	//}}AFX_EVENT
	DECLARE_EVENT_MAP()

	public:
	enum {
		dispidGetStatusBarWidth = 26L,
		//{{AFX_DISP_ID(CTMStatCtrl)
	dispidAutoInit = 1L,
	dispidBuild = 2L,
	dispidMajorVer = 3L,
	dispidMinorVer = 4L,
	dispidEnableErrors = 5L,
	dispidAutosizeFont = 6L,
	dispidStatusText = 7L,
	dispidLeftMargin = 8L,
	dispidRightMargin = 9L,
	dispidTopMargin = 10L,
	dispidBottomMargin = 11L,
	dispidTextVer = 12L,
	dispidPlaylistTime = 13L,
	dispidElapsedPlaylist = 14L,
	dispidDesignationTime = 15L,
	dispidElapsedDesignation = 16L,
	dispidMode = 17L,
	dispidDesignationCount = 18L,
	dispidDesignationIndex = 19L,
	dispidTextLine = 20L,
	dispidTextPage = 21L,
	dispidInitialize = 22L,
	dispidSetPlaylistInfo = 23L,
	dispidGetClassIdString = 24L,
	dispidGetRegisteredPath = 25L,
	//}}AFX_DISP_ID
	};
protected:
	LONG GetStatusBarWidth(void);
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMSTAT_H__07F27A54_ABF9_11D2_8C08_00802966F8C1__INCLUDED_)

