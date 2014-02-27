#if !defined(AFX_SAVEPAGES_H__F2EAFB99_C145_477C_A56D_3E451E33CCA4__INCLUDED_)
#define AFX_SAVEPAGES_H__F2EAFB99_C145_477C_A56D_3E451E33CCA4__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// SavePages.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CSavePages dialog

class CSavePages : public CDialog
{
// Construction
public:
	CSavePages(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CSavePages)
	enum { IDD = IDD_SAVE_PAGES };
	CString	m_strFolder;
	CString	m_strPrefix;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CSavePages)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CSavePages)
		// NOTE: the ClassWizard will add member functions here
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SAVEPAGES_H__F2EAFB99_C145_477C_A56D_3E451E33CCA4__INCLUDED_)
