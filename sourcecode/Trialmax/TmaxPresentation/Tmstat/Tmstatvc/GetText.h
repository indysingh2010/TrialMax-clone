#if !defined(AFX_GETTEXT_H__6381FAA1_C284_11D2_8173_00802966F8C1__INCLUDED_)
#define AFX_GETTEXT_H__6381FAA1_C284_11D2_8173_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// GetText.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CGetText dialog

class CGetText : public CDialog
{
// Construction
public:
	CGetText(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CGetText)
	enum { IDD = IDD_GETTEXT };
	CString	m_strText;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CGetText)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CGetText)
		// NOTE: the ClassWizard will add member functions here
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_GETTEXT_H__6381FAA1_C284_11D2_8173_00802966F8C1__INCLUDED_)
