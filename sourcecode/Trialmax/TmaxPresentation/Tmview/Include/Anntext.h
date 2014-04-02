//==============================================================================
//
// File Name:	anntext.h
//
// Description:	This file contains the declaration of the CAnnTextDlg class.
//
// Author:		Kenneth Moore
//
// Copyright FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	10-01-99	1.00		Original Release
//==============================================================================
#if !defined(AFX_ANNTEXT_H__B406AB21_7719_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_ANNTEXT_H__B406AB21_7719_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <resource.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	This class manages a dialog box that allows the user to enter the text
//	used for an annotation
class CAnnTextDlg : public CDialog
{
	private:

	public:

							CAnnTextDlg(CWnd* pParent = NULL);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CAnnTextDlg)
	enum { IDD = IDD_ANNTEXT };
	CListBox	m_ctrlNames;
	CString	m_strAnnText;
	BOOL	m_bBold;
	CString	m_strName;
	short	m_sSize;
	BOOL	m_bStrikeThrough;
	BOOL	m_bUnderline;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAnnTextDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(CAnnTextDlg)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_ANNTEXT_H__B406AB21_7719_11D3_8177_00802966F8C1__INCLUDED_)
