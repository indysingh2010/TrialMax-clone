//==============================================================================
//
// File Name:	textpg.h
//
// Description:	This file contains the declaration of the CTextPage class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-29-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_TEXTPG_H__98CB02DA_D4CA_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_TEXTPG_H__98CB02DA_D4CA_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <setuppg.h>
#include <colorctl.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Valid ranges
#define TEXT_MINIMUM_CHARSPERLINE		1
#define TEXT_MAXIMUM_CHARSPERLINE		256
#define TEXT_MINIMUM_HIGHLIGHTLINES		1
#define TEXT_MAXIMUM_HIGHLIGHTLINES		50
#define TEXT_MINIMUM_DISPLAYLINES		1
#define TEXT_MAXIMUM_DISPLAYLINES		50

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTextPage : public CSetupPage
{
	private:

		COLORREF		m_crBackground;
		COLORREF		m_crForeground;
		COLORREF		m_crHighlight;
		COLORREF		m_crHighlightText;
		CString			m_strFont;
		STextOptions	m_Options;
	
	public:
	
						CTextPage(CWnd* pParent = 0);

		void			ReadOptions(CTMIni& rIni);
		BOOL			WriteOptions(CTMIni& rIni);

	protected:

		void			SetColor(COLORREF* pColor, CColorPushbutton* pButton);

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CTextPage)
	enum { IDD = IDD_TEXT_PAGE };
	CButton	m_ctrlUseManagerHighlighters;
	CStatic	m_ctrlBackColorLabel;
	CStatic	m_ctrlHighTextColorLabel;
	CStatic	m_ctrlHighColorLabel;
	CStatic	m_ctrlForeColorLabel;
	CColorPushbutton	m_HighTextColor;
	CColorPushbutton	m_HighlightColor;
	CColorPushbutton	m_ForeColor;
	CColorPushbutton	m_BackColor;
	CComboBox	m_ctrlFonts;
	BOOL	m_bUseAvgCharWidth;
	short	m_sMaxCharsPerLine;
	BOOL	m_bShowPageLine;
	BOOL	m_bShowText;
	short	m_sHighlightLines;
	short	m_sDisplayLines;
	BOOL	m_bCenterVideo;
	BOOL	m_bCombineText;
	short	m_sScrollSteps;
	short	m_sScrollTime;
	BOOL	m_bSmoothScroll;
	BOOL	m_bUseManagerHighlighters;
	BOOL	m_bDisableScrollText;
	int		m_sBottomMargin;
	int		m_sLeftMargin;
	int		m_sRightMargin;
	int		m_sTopMargin;
	int		m_iBulletStyle;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTextPage)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(CTextPage)
	virtual BOOL OnInitDialog();
	afx_msg void OnBackColor();
	afx_msg void OnForeColor();
	afx_msg void OnHighlightColor();
	afx_msg void OnHighlightTextColor();
	afx_msg void OnUseManagerHighlightersClicked();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TEXTPG_H__98CB02DA_D4CA_11D3_8177_00802966F8C1__INCLUDED_)
