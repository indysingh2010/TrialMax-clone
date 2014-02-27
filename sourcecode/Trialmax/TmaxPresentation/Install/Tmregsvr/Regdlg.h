//==============================================================================
//
// File Name:	regdlg.h
//
// Description:	This file contains the declaration of the CTmregsvrDlg class. 
//
// Author:		Kenneth Moore
//
// Copyright 1999 Forensics Technologies International
//
//==============================================================================
//	Date		Revision    Description
//	03-26-99	1.00		Original Release
//==============================================================================
#if !defined(AFX_REGDLG_H__5D8FFB66_E41C_11D2_8175_00802966F8C1__INCLUDED_)
#define AFX_REGDLG_H__5D8FFB66_E41C_11D2_8175_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <tmini.h>
#include <handler.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define TMREGSVR_FILENAME			"Tmregsvr.ini"
#define TMREGSVR_SECTION			"TMREGSVR"

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTmregsvrDlg : public CDialog
{
	private:

		CBrush			m_brBackGnd;
		DWORD			m_dwLastError;
		int				m_iLine;
		CTMIni			m_Ini;
		CErrorHandler	m_Errors;
		CString			m_strControlFolder;

	public:
	
		BOOL			m_bCheckForReboot;

						CTmregsvrDlg(CWnd* pParent = NULL);	
		void			OnCancel(){};
		void			OnOk(){};
	
	protected:
		
		BOOL			Register(LPCSTR lpLibrary);
		BOOL			FindFile(LPCSTR lpFilespec);
		BOOL			RebootRequired();
		BOOL			GetNextName(CString& rName);
		void			DeleteShortcut();
		void			AddShortcut();


	//	The remainder of this declaration is maintained by ClassWizard

	public:

	//{{AFX_DATA(CTmregsvrDlg)
	enum { IDD = IDD_TMREGSVR_DIALOG };
	CStatic	m_ctrlStatus;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmregsvrDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	//{{AFX_MSG(CTmregsvrDlg)
	virtual BOOL OnInitDialog();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg void OnTimer(UINT nIDEvent);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_REGDLG_H__5D8FFB66_E41C_11D2_8175_00802966F8C1__INCLUDED_)
