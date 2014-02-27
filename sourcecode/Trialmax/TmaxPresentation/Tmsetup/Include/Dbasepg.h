//==============================================================================
//
// File Name:	dbasepg.h
//
// Description:	This file contains the declaration of the CDatabasePage class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-31-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_DBASEPG_H__64A24522_D7CD_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_DBASEPG_H__64A24522_D7CD_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <setuppg.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define DBPAGE_MAX_CASES	10

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CDatabasePage : public CSetupPage
{
	private:

		CString					m_aCases[DBPAGE_MAX_CASES];
		CBitmapButton			m_btnFolder;
		int						m_iCases;
	
	public:
	
								CDatabasePage(CWnd* pParent = 0);

		void					ReadOptions(CTMIni& rIni);
		BOOL					WriteOptions(CTMIni& rIni);

	protected:

		BOOL					FindFolder(LPCSTR lpFolder);
		void					InsertFolder(LPCSTR lpFolder);
		void					FillListBox();

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CDatabasePage)
	enum { IDD = IDD_DATABASE_PAGE };
	CStatic	m_ctrlCurrent;
	CListBox	m_ctrlCases;
	BOOL	m_bEnableErrors;
	BOOL	m_bEnablePowerPoint;
	CString	m_strCurrent;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDatabasePage)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(CDatabasePage)
	virtual BOOL OnInitDialog();
	afx_msg void OnBrowseFolder();
	afx_msg void OnSelChange();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DBASEPG_H__64A24522_D7CD_11D3_8177_00802966F8C1__INCLUDED_)
