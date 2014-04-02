//==============================================================================
//
// File Name:	graphpg.h
//
// Description:	This file contains the declaration of the CGraphicsPage class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-29-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_GRAPHPG_H__98CB02D8_D4CA_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_GRAPHPG_H__98CB02D8_D4CA_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <setuppg.h>
#include <tmvdefs.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Default graphics options
#define GRAPHICS_ANNTHICKNESS			1
#define GRAPHICS_MAXZOOM				5
#define GRAPHICS_PENSELECTORSIZE		5
#define GRAPHICS_CALLFRAMETHICKNESS		5
#define GRAPHICS_SCALEGRAPHICS			FALSE
#define GRAPHICS_SCALEDOCUMENTS			TRUE
#define GRAPHICS_ANNCOLOR				TMV_RED
#define GRAPHICS_REDACTCOLOR			TMV_RED
#define GRAPHICS_HIGHLIGHTCOLOR			TMV_YELLOW
#define GRAPHICS_CALLOUTCOLOR			TMV_CYAN
#define GRAPHICS_BACKGROUNDCOLOR		TMV_BLACK
#define GRAPHICS_CALLFRAMECOLOR			TMV_BLUE
#define GRAPHICS_CALLHANDLECOLOR		TMV_BLUE
#define GRAPHICS_USER_SPLITFRAMECOLOR	TMV_GREY
#define GRAPHICS_ZAP_SPLITFRAMECOLOR	TMV_GREY
#define GRAPHICS_PENSELECTORCOLOR		TMV_GREY

//	Color selection indices. The order here must match the order in which they
//	appear on the dialog box
#define DRAWCOLOR						0
#define HIGHLIGHTCOLOR					1
#define REDACTCOLOR						2
#define CALLOUTCOLOR					3
#define CALLFRAMECOLOR					4
#define CALLHANDLECOLOR					5
#define LIGHTPENCOLOR					6
#define USERSPLITFRAMECOLOR				7
#define ZAPSPLITFRAMECOLOR				8

//	Valid ranges
#define GRAPHICS_MINIMUM_THICKNESS		1
#define GRAPHICS_MAXIMUM_THICKNESS		20
#define GRAPHICS_MINIMUM_ZOOMFACTOR		1
#define GRAPHICS_MAXIMUM_ZOOMFACTOR		100

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CGraphicsPage : public CSetupPage
{
	private:

		CBitmapButton			m_btnRed;
		CBitmapButton			m_btnBlue;
		CBitmapButton			m_btnGreen;
		CBitmapButton			m_btnMagenta;
		CBitmapButton			m_btnCyan;
		CBitmapButton			m_btnYellow;
		CBitmapButton			m_btnBlack;
		CBitmapButton			m_btnGrey;
		CBitmapButton			m_btnWhite;
		CBitmapButton			m_btnDarkRed;
		CBitmapButton			m_btnDarkBlue;
		CBitmapButton			m_btnDarkGreen;
		CBitmapButton			m_btnLightRed;
		CBitmapButton			m_btnLightBlue;
		CBitmapButton			m_btnLightGreen;
	
		int						m_nColorTool;
		int						m_nHighlightColor;
		int						m_nRedactColor;
		int						m_nDrawColor;
		int						m_nCalloutColor;
		int						m_nCallFrameColor;
		int						m_nCallHandleColor;
		int						m_nLightPenColor;
		int						m_nUserSplitFrameColor;
		int						m_nZapSplitFrameColor;
		short					m_sFontSize;
		BOOL					m_bFontUnderline;
		BOOL					m_bFontStrikeThrough;
		BOOL					m_bFontBold;
					
	public:
	
								CGraphicsPage(CWnd* pParent = 0);

		void					ReadOptions(CTMIni& rIni);
		BOOL					WriteOptions(CTMIni& rIni);

	protected:

		void					SetColor(int nColor);
		void					SelectColor(int nColorTool);

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	CButton	 m_ctrlHighlight;
	CButton	 m_ctrlRedact;
	CButton	 m_ctrlCallout;
	CButton	 m_ctrlCalloutFrame;
	CButton	 m_ctrlCalloutHandle;
	CButton	 m_ctrlLightPenRect;

	//{{AFX_DATA(CGraphicsPage)
	enum { IDD = IDD_GRAPHICS_PAGE };
	CStatic	m_ctrlFontName;
	CButton	m_ctrlDrawTool;
	CSpinButtonCtrl	m_SpinLightPenSize;
	CSpinButtonCtrl	m_SpinCallThick;
	CSpinButtonCtrl	m_SpinZoom;
	CSpinButtonCtrl	m_SpinThick;
	UINT	m_nThickness;
	UINT	m_nZoomFactor;
	int		m_nColorSelect;
	BOOL	m_bScaleGraphics;
	BOOL	m_bScaleDocs;
	UINT	m_nLightPenSize;
	UINT	m_nCallFrameThick;
	int		m_iBitonal;
	BOOL	m_bLightPenControls;
	int		m_nTool;
	CString	m_strFontName;
	BOOL	m_bResizableCallouts;
	BOOL	m_bShadeOnCallout;
	short	m_sCalloutShadeGrayscale;
	BOOL	m_bPanCallouts;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CGraphicsPage)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(CGraphicsPage)
	virtual BOOL OnInitDialog();
	afx_msg void OnRed();
	afx_msg void OnBlack();
	afx_msg void OnBlue();
	afx_msg void OnCyan();
	afx_msg void OnGreen();
	afx_msg void OnGrey();
	afx_msg void OnMagenta();
	afx_msg void OnWhite();
	afx_msg void OnYellow();
	afx_msg void OnHighlight();
	afx_msg void OnRedact();
	afx_msg void OnDrawtool();
	afx_msg void OnCallout();
	afx_msg void OnCalloutFrame();
	afx_msg void OnCalloutHandle();
	afx_msg void OnLightPenRect();
	afx_msg void OnDarkBlue();
	afx_msg void OnDarkGreen();
	afx_msg void OnDarkRed();
	afx_msg void OnLightBlue();
	afx_msg void OnLightGreen();
	afx_msg void OnLightRed();
	afx_msg void OnChangeFont();
	afx_msg void OnUserSplitFrame();
	afx_msg void OnZapSplitFrame();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_GRAPHPG_H__98CB02D8_D4CA_11D3_8177_00802966F8C1__INCLUDED_)
