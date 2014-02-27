//==============================================================================
//
// File Name:	tmtool.h
//
// Description:	This file contains the declaration of the CTMToolCtrl class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	04-21-98	1.00		Original Release
//==============================================================================
#if !defined(AFX_TMTOOL_H__BBD917A7_D89D_11D1_B16C_008029EFD140__INCLUDED_)
#define AFX_TMTOOL_H__BBD917A7_D89D_11D1_B16C_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <afxcmn.h>
#include <handler.h>
#include <tmini.h>
#include <tmtbdefs.h>
#include <objsafe.h>
#include <tmver.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define TMTOOLBAR_ID				26000
#define TMTBERRORS_TITLE			"TrialMax Toolbar Error"
#define TMTB_COMMANDOFFSET			WM_USER

#define TMTB_INI_SIZE_LINE			"Size"
#define TMTB_INI_ORIENTATION_LINE	"Orientation"
#define TMTB_INI_TIPS_LINE			"Tips"
#define TMTB_INI_STRETCH_LINE		"Stretch"
#define TMTB_INI_STYLE_LINE			"Style"
#define TMTB_INI_ROWS_LINE			"Rows"

#define BUTTON_VERTPADDING			0
#define BUTTON_HORZPADDING			0
#define STACKED_COLORS				8



//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CTMToolCtrl : public COleControl
{
	private:

						DECLARE_DYNCREATE(CTMToolCtrl)

		CErrorHandler	m_Errors;
		CTMVersion		m_tmVersion;
		CToolBarCtrl	m_Toolbar;
		CImageList*		m_pEnabled;
		CImageList*		m_pDisabled;
		DWORD			m_dwStyle;
		RECT			m_rcFrame;
		RECT			m_rcBar;
		RECT			m_rcButtons;
		short			m_sButtons;
		short			m_sExtraButtonsCount;
		short			m_aMap[TMTB_MAXBUTTONS];
		CString			m_aLabels[TMTB_MAXBUTTONS];
		CScrollBar		m_HScrollBar;
		
		

	public:
	
						CTMToolCtrl();
					   ~CTMToolCtrl();

						const CSize& GetScrollPos() const;

						void   ScrollToOrigin(bool scrollLeft, 
                                  bool scrollTop);

	protected:

		void			GetRegistration();
		void			OnButtonClick(UINT uId);
		void			Reposition();
		void			ShowRects(LPSTR lpTitle);
		void			AddButtons();
		void			AddExtraLargeButtons();
		void			ReadIniFile();
		void			WriteIniFile();
		void			SetCtrlSize();
		short			GetBitmapWidth();
		short			GetBitmapHeight();
		short			GetButtonWidth();
		short			GetButtonHeight();
		short			GetEnabledStrip();
		short			GetDisabledStrip();
		BOOL			IsToolGroup(short sId);
		BOOL			IsColorGroup(short sId);
		BOOL			IsCheckButton(short sId);
		BOOL			IsPlayGroup(short sId);
		BOOL			IsShapeGroup(short sId);
		BOOL			IsHorizontal();
		BOOL			CreateToolbar();
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
	//{{AFX_VIRTUAL(CTMToolCtrl)
	public:
	virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
	virtual void DoPropExchange(CPropExchange* pPX);
	virtual BOOL OnSetExtent(LPSIZEL lpSizeL);
	virtual DWORD GetControlFlags();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	protected:
	virtual BOOL OnNotify(WPARAM wParam, LPARAM lParam, LRESULT* pResult);
	//}}AFX_VIRTUAL

	protected:

	DECLARE_OLECREATE_EX(CTMToolCtrl)    // Class factory and guid
	DECLARE_OLETYPELIB(CTMToolCtrl)      // GetTypeInfo
	DECLARE_PROPPAGEIDS(CTMToolCtrl)     // Property page IDs
	DECLARE_OLECTLTYPE(CTMToolCtrl)		// Type name and misc status

	//{{AFX_MSG(CTMToolCtrl)
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg int OnMouseActivate(CWnd* pDesktopWnd, UINT nHitTest, UINT message);
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnHScroll(UINT nSBCode,UINT nPos, CScrollBar* pScrollBar);
    afx_msg void OnVScroll(UINT nSBCode,UINT nPos, CScrollBar* pScrollBar);
    afx_msg BOOL OnMouseWheel(UINT nFlags,short zDelta, CPoint pt);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	public:
	//{{AFX_DISPATCH(CTMToolCtrl)
	CString m_strIniFile;
	afx_msg void OnIniFileChanged();
	short m_sOrientation;
	afx_msg void OnOrientationChanged();
	short m_sButtonSize;
	afx_msg void OnButtonSizeChanged();
	BOOL m_bAutoInit;
	afx_msg void OnAutoInitChanged();
	BOOL m_bEnableErrors;
	afx_msg void OnEnableErrorsChanged();
	short m_sStyle;
	afx_msg void OnStyleChanged();
	BOOL m_bStretch;
	afx_msg void OnStretchChanged();
	CString m_strButtonMask;
	afx_msg void OnButtonMaskChanged();
	BOOL m_bToolTips;
	afx_msg void OnToolTipsChanged();
	BOOL m_bConfigurable;
	afx_msg void OnConfigurableChanged();
	CString m_strIniSection;
	afx_msg void OnIniSectionChanged();
	short m_sButtonRows;
	afx_msg void OnButtonRowsChanged();
	BOOL m_bAutoReset;
	afx_msg void OnAutoResetChanged();
	afx_msg short GetVerMajor();
	afx_msg short GetVerMinor();
	afx_msg short GetVerBuild();
	afx_msg BSTR GetVerTextLong();
	afx_msg short GetBarWidth();
	afx_msg short GetBarHeight();
	afx_msg short SetButtonImage(short sId, short sImage);
	afx_msg short Initialize();
	afx_msg short ResetFrame();
	afx_msg void  ResetExtraLargeButtonsFrame();
	afx_msg short SetColorButton(short sId);
	afx_msg short SetToolButton(short sId);
	afx_msg BOOL IsButton(short sId);
	afx_msg BSTR GetButtonLabel(short sId);
	afx_msg short SetButtonMap(short FAR* pMap);
	afx_msg short SetPlayButton(BOOL bPlaying);
	afx_msg short SetSplitButton(BOOL bSplit, BOOL bHorizontal);
	afx_msg short SetLinkButton(BOOL bDisabled);
	afx_msg short Configure();
	afx_msg short SetButtonLabel(short sId, LPCTSTR lpLabel);
	afx_msg short CheckButton(short sId, BOOL bCheck);
	afx_msg short EnableButton(short sId, BOOL bEnable);
	afx_msg short HideButton(short sId, BOOL bHide);
	afx_msg short Popup(OLE_HANDLE hWnd);
	afx_msg short GetImageIndex(short sId);
	afx_msg short GetButtonId(short sImageIndex);
	afx_msg short SetShapeButton(short sId);
	afx_msg short GetButtonMap(short FAR* paMap);
	afx_msg short Save();
	afx_msg BSTR GetClassIdString();
	afx_msg BSTR GetRegisteredPath();
	afx_msg short Reset();
	afx_msg short SetZoomButton(BOOL bZoom, BOOL bRestricted);
	afx_msg short GetSortOrder(short FAR* pOrder);
	afx_msg short GetSortedId(short sId);
	//}}AFX_DISPATCH
	DECLARE_DISPATCH_MAP()

	afx_msg void AboutBox();

	//	Added in rev 5.2
	BOOL m_bUseSystemBackground;
	afx_msg void OnUseSystemBackgroundChanged();

	//	Added in rev 6.1.0
	afx_msg short GetVerQEF();
	afx_msg BSTR GetVerTextShort();
	afx_msg BSTR GetVerBuildDate();

	//{{AFX_EVENT(CTMToolCtrl)
	void FireButtonClick(short sId, BOOL bChecked)
		{FireEvent(eventidButtonClick,EVENT_PARAM(VTS_I2  VTS_BOOL), sId, bChecked);}
	void FireReconfigure()
		{FireEvent(eventidReconfigure,EVENT_PARAM(VTS_NONE));}
	//}}AFX_EVENT
	DECLARE_EVENT_MAP()

	public:
	enum {
		eventidSetRedButton = 3L,
		//{{AFX_DISP_ID(CTMToolCtrl)
	dispidIniFile = 1L,
	dispidOrientation = 2L,
	dispidButtonSize = 3L,
	dispidVerBuild = 4L,
	dispidVerMajor = 5L,
	dispidVerMinor = 6L,
	dispidAutoInit = 7L,
	dispidEnableErrors = 8L,
	dispidStyle = 9L,
	dispidStretch = 10L,
	dispidButtonMask = 11L,
	dispidToolTips = 12L,
	dispidConfigurable = 13L,
	dispidVerTextLong = 14L,
	dispidIniSection = 15L,
	dispidButtonRows = 16L,
	dispidAutoReset = 17L,
	dispidGetBarWidth = 18L,
	dispidGetBarHeight = 19L,
	dispidSetButtonImage = 20L,
	dispidInitialize = 21L,
	dispidResetFrame = 22L,
	dispidSetColorButton = 23L,
	dispidSetToolButton = 24L,
	dispidIsButton = 25L,
	dispidGetButtonLabel = 26L,
	dispidSetButtonMap = 27L,
	dispidSetPlayButton = 28L,
	dispidSetSplitButton = 29L,
	dispidSetLinkButton = 30L,
	dispidConfigure = 31L,
	dispidSetButtonLabel = 32L,
	dispidCheckButton = 33L,
	dispidEnableButton = 34L,
	dispidHideButton = 35L,
	dispidPopup = 36L,
	dispidGetImageIndex = 37L,
	dispidGetButtonId = 38L,
	dispidSetShapeButton = 39L,
	dispidGetButtonMap = 40L,
	dispidSave = 41L,
	dispidGetClassIdString = 42L,
	dispidGetRegisteredPath = 43L,
	dispidReset = 44L,
	dispidSetZoomButton = 45L,
	dispidGetSortOrder = 46L,
	dispidGetSortedId = 47L,
	eventidButtonClick = 1L,
	eventidReconfigure = 2L,
	//}}AFX_DISP_ID
	};
protected:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
public:
	int myRedButton;
protected:

	void SetRedButton(ULONG color)
	{
		
		FireEvent(eventidSetRedButton, EVENT_PARAM(VTS_UI4), color);
	}
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMTOOL_H__BBD917A7_D89D_11D1_B16C_008029EFD140__INCLUDED)
