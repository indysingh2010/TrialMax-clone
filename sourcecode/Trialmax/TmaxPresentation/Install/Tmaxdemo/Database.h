#if !defined(AFX_DATABASE_H__9CF345A1_B375_11D2_AD00_444553540000__INCLUDED_)
#define AFX_DATABASE_H__9CF345A1_B375_11D2_AD00_444553540000__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000
// Database.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CConfirmDatabase dialog

class CConfirmDatabase : public CDialog
{
// Construction
public:
	CConfirmDatabase(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CConfirmDatabase)
	enum { IDD = IDD_CONFIRMDB };
	CButton	m_crtlCancel;
	CString	m_strDrive;
	CString	m_strComment;
	//}}AFX_DATA

	BOOL m_bShowCancel;

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CConfirmDatabase)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CConfirmDatabase)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DATABASE_H__9CF345A1_B375_11D2_AD00_444553540000__INCLUDED_)
