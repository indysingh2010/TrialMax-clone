//==============================================================================
//
// File Name:	rtailpg.h
//
// Description:	This file contains the declaration of the CRingtailPage class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	06-07-03	1.00		Original Release
//==============================================================================
#if !defined(__RTAILPG_H__)
#define __RTAILPG_H__

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

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CRingtailPage : public CSetupPage
{
	private:

		COLORREF		m_crRedact;
		COLORREF		m_crRedactLabel;
	
	public:
	
						CRingtailPage(CWnd* pParent = 0);

		void			ReadOptions(CTMIni& rIni);
		BOOL			WriteOptions(CTMIni& rIni);

	protected:

		void			SetColor(COLORREF* pColor, CColorPushbutton* pButton);

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CRingtailPage)
	enum { IDD = IDD_RINGTAIL_PAGE };
	CColorPushbutton	m_RedactColor;
	CColorPushbutton	m_RedactLabelColor;
	CListBox	m_ctrlRedactLabelFonts;
	CString	m_strRedactLabelFont;
	short	m_sRedactLabelSize;
	int		m_iRedactTransparency;
	BOOL	m_bShowRedactions;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CRingtailPage)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(CRingtailPage)
	virtual BOOL OnInitDialog();
	afx_msg void OnRedactColor();
	afx_msg void OnRedactLabelColor();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__RTAILPG_H__)
