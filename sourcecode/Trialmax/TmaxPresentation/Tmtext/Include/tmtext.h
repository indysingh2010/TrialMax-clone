//==============================================================================
//
// File Name:	tmtext.h
//
// Description:	This file contains the declaration of the CTMTextCtrl class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	01-02-99	1.00		Original Release
//==============================================================================
#if !defined(AFX_TMTEXT_H__07F27A54_ABF9_11D2_8C08_00802966F8C1__INCLUDED_)
#define AFX_TMTEXT_H__07F27A54_ABF9_11D2_8C08_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <handler.h>
#include <playlist.h>
#include <textpage.h>
#include <textline.h>
#include <afxmt.h>
#include <objsafe.h>
#include <tmver.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define PAGELINE_CHARACTERS		10
#define MINIMUM_SCROLLPERIOD	5
#define TMTEXT_ALLOCID			50000L

//------------------------------------------------------------------------------
//	PROTOTYPES
//------------------------------------------------------------------------------
void CALLBACK ProcessTimer(UINT uId, UINT uMsg, DWORD dwUser, 
						   DWORD dw1, DWORD dw2); 

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMTextCtrl : public COleControl
{
	private:

							DECLARE_DYNCREATE(CTMTextCtrl)

		CCriticalSection	m_CriticalSection;
		CErrorHandler		m_Errors;
		CTMVersion			m_tmVersion;
		LOGFONT				m_lfFont;
		CTextLines			m_Lines;
		CTextLines			m_Available;
		CPlaylist*			m_pPlaylist;
		CTextLine*			m_pLine;
		CTextLine*			m_pNext;
		CTextLine*			m_pPending;
		CFont*				m_pFont;
		int					m_iMaxWidth;
		int					m_iCharWidth;
		int					m_iCharHeight;
		UINT				m_uPeriod;
		UINT				m_uTimer;
		short				m_sStepCount;
		float				m_fStepSize;
		BOOL				m_bScrolling;
		BOOL				m_bProcessingTimer;
		RECT				m_rcTextBounds;
		RECT				m_rcBulletBounds;

	public:
	
							CTMTextCtrl();
						   ~CTMTextCtrl();

		void				OnScrollTimer();

	protected:

		void				DrawLines(CDC* pdc);
		void				DrawLines(CDC* pdc, float fOffset);
		void				DrawLine(CDC* pdc, CTextLine* pLine, int iTop, int iBottom, BOOL bHighlighted);
		void				DrawBullet(CDC* pdc, CTextLine* pLine, int iTop, int iBottom, BOOL bHighlighted);
		void				LoadLines();
		void				LoadAvailable();
		void				KillScrollTimer();
		void				CalcScrollPeriod();
		void				ScrollNext();
		void				GetRegistration();
		void				RecalcLayout();
		BOOL				SetScrollTimer();
		BOOL				CheckVersion(DWORD dwVersion);
		CDesignation*		GetDesignation(long lId);
		COLORREF			GetForeColorEx(CTextLine* pLine, BOOL bHighlighted);
		COLORREF			GetBackColorEx(CTextLine* pLine, BOOL bHighlighted);

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
	//{{AFX_VIRTUAL(CTMTextCtrl)
	public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual void DoPropExchange(CPropExchange* pPX);
	virtual void OnFontChanged();
	//}}AFX_VIRTUAL

	protected:

	DECLARE_OLECREATE_EX(CTMTextCtrl)    // Class factory and guid
	DECLARE_OLETYPELIB(CTMTextCtrl)      // GetTypeInfo
	DECLARE_PROPPAGEIDS(CTMTextCtrl)     // Property page IDs
	DECLARE_OLECTLTYPE(CTMTextCtrl)		// Type name and misc status

	//{{AFX_MSG(CTMTextCtrl)
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnDestroy();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	public:
	//{{AFX_DISPATCH(CTMTextCtrl)
	BOOL m_bAutoInit;
	afx_msg void OnAutoInitChanged();
	afx_msg short GetVerBuild();
	BOOL m_bEnableErrors;
	afx_msg void OnEnableErrorsChanged();
	afx_msg short GetVerMajor();
	afx_msg short GetVerMinor();
	OLE_COLOR m_ocHighlight;
	afx_msg void OnHighlightColorChanged();
	OLE_COLOR m_ocHighlightText;
	afx_msg void OnHighlightTextColorChanged();
	short m_sHighlight;
	afx_msg void OnHighlightLinesChanged();
	short m_sDisplay;
	afx_msg void OnDisplayLinesChanged();
	BOOL m_bCombine;
	afx_msg void OnCombineDesignationsChanged();
	BOOL m_bShowPgLn;
	afx_msg void OnShowPageLineChanged();
	BOOL m_bUseAvgCharWidth;
	afx_msg void OnUseAvgCharWidthChanged();
	BOOL m_bResizeOnChange;
	afx_msg void OnResizeOnChangeChanged();
	short m_sMaxCharsPerLine;
	afx_msg void OnMaxCharsPerLineChanged();
	short m_sTopMargin;
	afx_msg void OnTopMarginChanged();
	short m_sBottomMargin;
	afx_msg void OnBottomMarginChanged();
	short m_sLeftMargin;
	afx_msg void OnLeftMarginChanged();
	short m_sRightMargin;
	afx_msg void OnRightMarginChanged();
	afx_msg BSTR GetVerTextLong();
	BOOL m_bSmoothScroll;
	afx_msg void OnSmoothScrollChanged();
	short m_sScrollTime;
	afx_msg void OnScrollTimeChanged();
	short m_sScrollSteps;
	afx_msg void OnScrollStepsChanged();
	afx_msg short Initialize();
	afx_msg short GetMinHeight();
	afx_msg long GetCurrentLine();
	afx_msg short ResizeFont(short bRedraw);
	afx_msg short IsFirstLine();
	afx_msg short IsLastLine();
	afx_msg short Next(short bRedraw);
	afx_msg short Previous(short bRedraw);
	afx_msg short SetLineObject(long lLine, short bRedraw);
	afx_msg short SetMaxWidth(short sWidth, short bRedraw);
	afx_msg short SetPlaylist(long lPlaylist, short bRedraw);
	afx_msg short GetCharHeight();
	afx_msg short GetCharWidth();
	afx_msg short GetLogFont(long lLogFont);
	afx_msg BOOL IsReady();
	afx_msg short SetLine(long lDesignation, long lPageNum, long lLineNum, short bRedraw);
	afx_msg BSTR GetRegisteredPath();
	afx_msg BSTR GetClassIdString();
	//}}AFX_DISPATCH
	DECLARE_DISPATCH_MAP()

	//	Added in rev 5.1
	BOOL m_bUseLineColor;
	afx_msg void OnUseLineColorChanged();

	//	Added in rev 6.1.0
	afx_msg short GetVerQEF();
	afx_msg BSTR GetVerTextShort();
	afx_msg BSTR GetVerBuildDate();

	//	Added in rev 6.3.4
	BOOL m_bShowText;
	afx_msg void OnShowTextChanged();
	short m_sBulletStyle;
	afx_msg void OnBulletStyleChanged();
	short m_sBulletMargin;
	afx_msg void OnBulletMarginChanged();

	afx_msg void AboutBox();

	//{{AFX_EVENT(CTMTextCtrl)
	void FireHeightChange(short sHeight)
		{FireEvent(eventidHeightChange,EVENT_PARAM(VTS_I2), sHeight);}
	//}}AFX_EVENT
	DECLARE_EVENT_MAP()

	public:
	enum {
	//{{AFX_DISP_ID(CTMTextCtrl)
	dispidAutoInit = 1L,
	dispidBuild = 2L,
	dispidEnableErrors = 3L,
	dispidMajorVer = 4L,
	dispidMinorVer = 5L,
	dispidHighlightColor = 6L,
	dispidHighlightTextColor = 7L,
	dispidHighlightLines = 8L,
	dispidDisplayLines = 9L,
	dispidCombineDesignations = 10L,
	dispidShowPageLine = 11L,
	dispidUseAvgCharWidth = 12L,
	dispidResizeOnChange = 13L,
	dispidMaxCharsPerLine = 14L,
	dispidTopMargin = 15L,
	dispidBottomMargin = 16L,
	dispidLeftMargin = 17L,
	dispidRightMargin = 18L,
	dispidTextVer = 19L,
	dispidSmoothScroll = 20L,
	dispidScrollTime = 21L,
	dispidScrollSteps = 22L,
	dispidInitialize = 23L,
	dispidGetMinHeight = 24L,
	dispidGetCurrentLine = 25L,
	dispidResizeFont = 26L,
	dispidIsFirstLine = 27L,
	dispidIsLastLine = 28L,
	dispidNext = 29L,
	dispidPrevious = 30L,
	dispidSetLineObject = 31L,
	dispidSetMaxWidth = 32L,
	dispidSetPlaylist = 33L,
	dispidGetCharHeight = 34L,
	dispidGetCharWidth = 35L,
	dispidGetLogFont = 36L,
	dispidIsReady = 37L,
	dispidSetLine = 38L,
	dispidGetRegisteredPath = 39L,
	dispidGetClassIdString = 40L,
	eventidHeightChange = 1L,
	//}}AFX_DISP_ID
	};
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMTEXT_H__07F27A54_ABF9_11D2_8C08_00802966F8C1__INCLUDED_)
