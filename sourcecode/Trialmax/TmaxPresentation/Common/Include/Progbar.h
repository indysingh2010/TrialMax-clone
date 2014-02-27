//==============================================================================
//
// File Name:	progbar.h
//
// Description:	This file contains the declaration of the CProgressBar class. 
//				This is a CProgressCtrl derived class that implements a setup
//				style progress bar.
//
// Author:		Kenneth Moore
//
// Copyright:
//
//==============================================================================
//	Date		Revision    Description
//	06-01-2001	1.00		Original Release
//==============================================================================
#if !defined(__PROGBAR_H__)
#define __PROGBAR_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	SetTextFormat() and HideText() options
#define PBS_SHOW_PERCENT         0x0100
#define PBS_SHOW_POSITION        0x0200
#define PBS_SHOW_TEXTONLY        0x0300
#define PBS_TEXTMASK             0x0300

//	ModifyStyle() options
#define PBS_TIED_TEXT            0x1000
#define PBS_RUBBER_BAR           0x2000
#define PBS_REVERSE              0x4000
#define PBS_SNAKE                0x8000

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

typedef	struct 
{
	CDC*	pdc;
	DWORD	dwStyle;
	CRect	rcClient;
	int		iCurPos;
	int		iLower;
	int		iUpper;
}SPBDrawParams;

class CProgressBar : public CProgressCtrl
{
	private:

	public:
	
							CProgressBar();
		virtual			   ~CProgressBar();
		
		void				SetGradientColors(COLORREF crStart, COLORREF crEnd) { m_aGradColors.SetSize(2); m_aGradColors.SetAt(0, crStart); m_aGradColors.SetAt(1, crEnd); }
		void				GetGradientColors(COLORREF& crStart, COLORREF& crEnd) { crStart = m_aGradColors[0]; crEnd = m_aGradColors[1]; }

		void				SetGradientColorsEx(int iCount, COLORREF crFirst, COLORREF crNext, ...);
		const CDWordArray&	GetGradientColorsEx(){ return m_aGradColors; }
	
		void				SetBarBrush(CBrush* pbrBar){ m_pbrBar = pbrBar; }
		CBrush*				GetBarBrush(){ return m_pbrBar; }

		void				SetBkColor(COLORREF crBackground){ m_crBackground = crBackground; }
		COLORREF			GetBkColor(){ return m_crBackground; }

		void				SetBkBrush(CBrush* pbrBackground){ m_pbrBackground = pbrBackground; }
		CBrush*				GetBkBrush(){ return m_pbrBackground; }

		void				SetTextColor(COLORREF crBarText, COLORREF crBkgndText = -1){ m_crBarText = m_crBackgroundText = crBarText; if(crBkgndText != -1) m_crBackgroundText = crBkgndText;}
		COLORREF			GetTextColor(){ return m_crBarText; }
		COLORREF			GetBkgndTextColor(){ return m_crBackgroundText; }

		void				SetShowPercent(BOOL bShowPercent = TRUE){ SetTextFormat(bShowPercent ? "%d%%" : NULL, PBS_SHOW_PERCENT); }
		BOOL				GetShowPercent(){ return GetStyle()&PBS_SHOW_PERCENT; }

		void				SetTextFormat(LPCTSTR lpFormat, DWORD dwFlags = PBS_SHOW_TEXTONLY);
		void				HideText(){SetTextFormat(0);}

		void				SetTiedText(BOOL bTiedText = TRUE){ ModifyStyle(bTiedText ? 0 : PBS_TIED_TEXT, bTiedText ? PBS_TIED_TEXT : 0, SWP_DRAWFRAME); }
		BOOL				GetTiedText() { return GetStyle() & PBS_TIED_TEXT; }

		void				SetRubberBar(BOOL bRubber = TRUE){ ModifyStyle(bRubber ? 0 : PBS_RUBBER_BAR, bRubber ? PBS_RUBBER_BAR : 0, SWP_DRAWFRAME); }
		BOOL				GetRubberBar(){ return GetStyle() & PBS_RUBBER_BAR; }

		void				SetReverse(BOOL bReverse = TRUE){ ModifyStyle(bReverse ? 0 : PBS_REVERSE, bReverse ? PBS_REVERSE : 0, SWP_DRAWFRAME); }
		BOOL				GetReverse(){ return GetStyle() & PBS_REVERSE; }

		void				SetSnake(BOOL bSnake = TRUE){ ModifyStyle(bSnake ? 0 : PBS_SNAKE|PBS_RUBBER_BAR, bSnake ? PBS_SNAKE|PBS_RUBBER_BAR : 0, SWP_DRAWFRAME); }
		BOOL				GetSnake(){ return GetStyle() & PBS_SNAKE; }

		void				SetSnakeTail(int iTailSize) { m_iTailSize = iTailSize; }
		int					GetSnakeTail(){ return m_iTailSize; }

		void				SetBorders(const CRect& rcBorders){ m_rcBorders = rcBorders; }
		const CRect&		GetBorders(){ return m_rcBorders; }

		int					GetMinHeight();

	protected:

		CRect				GetDrawRect(const SPBDrawParams& rParams, 
										const CRect& rcBar);
		virtual void		DrawMultiGradient(const SPBDrawParams& rParams, 
											  const CRect& rGrad, 
											  const CRect& rClip);
		virtual void		DrawGradient(const SPBDrawParams& rParams, 
										 const CRect& rGrad, 
										 const CRect& rClip, 
										 COLORREF crStart, COLORREF crEnd);
		virtual void		DrawText(const SPBDrawParams& rParams, 
									 const CRect& rMax, const CRect& rBar);
		virtual void		DrawClippedText(const SPBDrawParams& rParams, 
											const CRect& rClip, 
											CString& rText, const CPoint& rOrigin);
		virtual BOOL		SetSnakePos(int& rOldPos, int iNewPos, 
										BOOL bIncrement = FALSE);

		CDWordArray			m_aGradColors;
		CBrush*				m_pbrBar; 
		COLORREF			m_crBackground;
		CBrush*				m_pbrBackground;
		COLORREF			m_crBarText;
		COLORREF			m_crBackgroundText;

		int					m_iTail;
		int					m_iTailSize;
		int					m_iStep;

		CRect				m_rcBorders;

	//	The remainder of this declaration is maintained by Class Wizard
	public:
	
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CProgressBar)
	//}}AFX_VIRTUAL

	protected:
	
	protected:
	
	//{{AFX_MSG(CProgressBar)
	afx_msg BOOL OnEraseBkgnd(CDC* pdc);
	afx_msg void OnPaint();
	afx_msg LRESULT OnSetBarColor(WPARAM, LPARAM);
	afx_msg LRESULT OnSetBkColor(WPARAM, LPARAM);
	afx_msg LRESULT OnSetPos(WPARAM, LPARAM);
	afx_msg LRESULT OnDeltaPos(WPARAM, LPARAM);
	afx_msg LRESULT OnStepIt(WPARAM, LPARAM);
	afx_msg LRESULT OnSetStep(WPARAM, LPARAM);
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(__PROGBAR_H__)
