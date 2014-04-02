//==============================================================================
//
// File Name:	pptools.h
//
// Description:	This file contains the declaration of the CPPTools class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	09-08-01	1.00		Original Release
//==============================================================================
#if !defined(__PPTOOLS_H__)
#define __PPTOOLS_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <tmini.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Color selection indices. The order here must match the order in which they
//	appear on the dialog box
#define DRAWCOLOR						0
#define HIGHLIGHTCOLOR					1
#define REDACTCOLOR						2
#define CALLOUTCOLOR					3
#define CALLFRAMECOLOR					4
#define CALLHANDLECOLOR					5

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CPPTools : public CPropertyPage
{
	private:

							DECLARE_DYNCREATE(CPPTools)

		CBitmapButton		m_btnRed;
		CBitmapButton		m_btnBlue;
		CBitmapButton		m_btnGreen;
		CBitmapButton		m_btnMagenta;
		CBitmapButton		m_btnCyan;
		CBitmapButton		m_btnYellow;
		CBitmapButton		m_btnBlack;
		CBitmapButton		m_btnGrey;
		CBitmapButton		m_btnWhite;
		CBitmapButton		m_btnDarkRed;
		CBitmapButton		m_btnDarkBlue;
		CBitmapButton		m_btnDarkGreen;
		CBitmapButton		m_btnLightRed;
		CBitmapButton		m_btnLightBlue;
		CBitmapButton		m_btnLightGreen;
	
		CButton				m_ctrlHighlight;
		CButton				m_ctrlRedact;
		CButton				m_ctrlCallout;
		CButton				m_ctrlCalloutFrame;
		CButton				m_ctrlCalloutHandle;
		CButton				m_ctrlLightPenRect;

		int					m_iColorTool;
		int					m_iHighlightColor;
		int					m_iRedactColor;
		int					m_iDrawColor;
		int					m_iCalloutColor;
		int					m_iCallFrameColor;
		int					m_iCallHandleColor;
		short				m_sFontSize;
		BOOL				m_bFontUnderline;
		BOOL				m_bFontStrikeThrough;
		BOOL				m_bFontBold;
					
	public:
	
							CPPTools();
						   ~CPPTools();

		void				SetOptions(SGraphicsOptions* pOptions);
		void				GetOptions(SGraphicsOptions* pOptions);

	protected:

		void				SetColor(int iColor);
		void				SelectColor(int iColorTool);

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Dialog Data
	//{{AFX_DATA(CPPTools)
	enum { IDD = IDD_TOOLS_PAGE };
	CStatic	m_ctrlFontName;
	CButton	m_ctrlDrawTool;
	CSpinButtonCtrl	m_SpinCallThick;
	CSpinButtonCtrl	m_SpinZoom;
	CSpinButtonCtrl	m_SpinThick;
	UINT	m_iThickness;
	UINT	m_iZoomFactor;
	int		m_iColorSelect;
	UINT	m_iCallFrameThick;
	int		m_iBitonal;
	int		m_iTool;
	CString	m_strFontName;
	BOOL	m_bResizableCallouts;
	//}}AFX_DATA

	// Overrides
	// ClassWizard generate virtual function overrides
	//{{AFX_VIRTUAL(CPPTools)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:
	// Generated message map functions
	//{{AFX_MSG(CPPTools)
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
	afx_msg void OnDarkBlue();
	afx_msg void OnDarkGreen();
	afx_msg void OnDarkRed();
	afx_msg void OnLightBlue();
	afx_msg void OnLightGreen();
	afx_msg void OnLightRed();
	afx_msg void OnHighlight();
	afx_msg void OnRedact();
	afx_msg void OnDrawtool();
	afx_msg void OnCallout();
	afx_msg void OnCalloutFrame();
	afx_msg void OnCalloutHandle();
	afx_msg void OnChangeFont();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_PPTOOLS_H__01D5D360_A3BB_11D5_8F0A_00802966F8C1__INCLUDED_)
