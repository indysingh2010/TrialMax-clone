//==============================================================================
//
// File Name:	pageset.h
//
// Description:	This file contains the declaration of the CPageSetup class. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2001
//
//==============================================================================
//	Date		Revision    Description
//	08-25-01	1.00		Original Release
//==============================================================================
#if !defined(AFX_PAGESET_H__852295C8_98B0_11D5_8F0A_00802966F8C1__INCLUDED_)
#define AFX_PAGESET_H__852295C8_98B0_11D5_8F0A_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <colorctl.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CPageSetup : public CDialog
{
	private:

	public:

		COLORREF			m_crBorderColor;

							CPageSetup(CWnd* pParent = NULL);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CPageSetup)
	enum { IDD = IDD_PRINTPAGE };
	CColorPushbutton	m_ctrlBorderColor;
	float	m_fBorderThickness;
	float	m_fBottomMargin;
	float	m_fLeftMargin;
	float	m_fRightMargin;
	float	m_fTopMargin;
	int		m_iOrientation;
	BOOL	m_bPrintCallouts;
	BOOL	m_bPrintBorder;
	BOOL	m_bPrintCalloutBorders;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CPageSetup)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(CPageSetup)
	virtual BOOL OnInitDialog();
	afx_msg void OnBorderColor();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_PAGESET_H__852295C8_98B0_11D5_8F0A_00802966F8C1__INCLUDED_)
