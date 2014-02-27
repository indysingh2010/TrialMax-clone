//==============================================================================
//
// File Name:	colorctl.h
//
// Description:	This file contains the declarations of the CColorPushbutton 
//				and CColorStatic classes
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	01-31-99	1.00		Original Release
//==============================================================================
#if !defined(__COLORCTL_H__)
#define __COLORCTL_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define RGB_BUTTON_LIGHT    (GetSysColor(COLOR_BTNFACE))
#define RGB_BUTTON_DARK     (GetSysColor(COLOR_BTNSHADOW))
#define RGB_BUTTON_BLACK    (GetSysColor(COLOR_WINDOWFRAME))
#define RGB_BUTTON_WHITE    (GetSysColor(COLOR_BTNHIGHLIGHT))

#define COLORSTATIC_SLOW	0
#define COLORSTATIC_NORMAL	1
#define COLORSTATIC_FAST	2

#define CS_SLOW_PERIOD		2000
#define CS_NORMAL_PERIOD	1000
#define CS_FAST_PERIOD		500

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CColorPushbutton : public CButton
{
	private:
	
		COLORREF	m_crColor;
		
	public:
	
					CColorPushbutton();
				   ~CColorPushbutton();

		void		SetColor(COLORREF crColor);
		COLORREF	GetColor(){ return m_crColor; }

	protected:

	//	The remainder of this declaration is managed by ClassWizard

	public:
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CColorPushbutton)
	public:
	virtual void DrawItem(LPDRAWITEMSTRUCT lpDrawItemStruct);
	//}}AFX_VIRTUAL


	protected:
	//{{AFX_MSG(CColorPushbutton)
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
};

class CColorStatic : public CStatic
{
	private:

		CWnd*		m_pParent;
		CBrush		m_brBkgnd;
		CBrush		m_brBlinkBkgnd[2];
		COLORREF	m_crTextColor;
		COLORREF	m_crBlinkTextColors[2];
		COLORREF	m_crBkColor;
		COLORREF	m_crBlinkBkColors[2];
		BOOL		m_bTextBlink;
		BOOL		m_bBkBlink;
		BOOL		m_bTextBlinkState;
		BOOL		m_bBkBlinkState;
		UINT		m_nTimerId;
		UINT		m_nMsg;

	public:
	
	
					CColorStatic();
				   ~CColorStatic();

		COLORREF	GetTextColor();
		COLORREF	GetBkColor();
	
		void		SetTextColor(COLORREF crTextColor = 0xFFFFFFFF);
		void		SetBkColor(COLORREF crBkColor = 0xFFFFFFFF);
		void		SetBlinkTextColors(COLORREF crColor1, COLORREF crColor2);
		void		SetBlinkBkColors(COLORREF crColor1, COLORREF crColor2);
		void		StartTextBlink(BOOL bStart = TRUE, 
								   int iRate = COLORSTATIC_NORMAL);
		void		StartBkBlink(BOOL bStart = TRUE, 
								 int iRate = COLORSTATIC_NORMAL);

		void		EnableNotify(CWnd* pParent = NULL, UINT nMsg = WM_USER);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard

	public:

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CColorStatic)
	//}}AFX_VIRTUAL



	protected:
	//{{AFX_MSG(CColorStatic)
	afx_msg HBRUSH CtlColor(CDC* pDC, UINT nCtlColor);
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg void OnDestroy();
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__COLORCTL_H__
