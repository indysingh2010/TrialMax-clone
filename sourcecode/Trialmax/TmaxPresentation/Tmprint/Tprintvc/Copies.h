#if !defined(AFX_COPIES_H__A4EC5A4B_CB33_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_COPIES_H__A4EC5A4B_CB33_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Copies.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CCopies dialog

class CCopies : public CDialog
{
// Construction
public:
	CCopies(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CCopies)
	enum { IDD = IDD_COPIES };
	short	m_sCopies;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CCopies)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CCopies)
		// NOTE: the ClassWizard will add member functions here
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_COPIES_H__A4EC5A4B_CB33_11D3_8177_00802966F8C1__INCLUDED_)
