#if !defined(AFX_GNUMBER_H__6C3F7A84_AD64_11D2_8173_00802966F8C1__INCLUDED_)
#define AFX_GNUMBER_H__6C3F7A84_AD64_11D2_8173_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Gnumber.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CGetNumber dialog

class CGetNumber : public CDialog
{
// Construction
public:
	CGetNumber(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CGetNumber)
	enum { IDD = IDD_GETNUMBER };
	CString	m_strNumber;
	CString	m_strLabel;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CGetNumber)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CGetNumber)
		// NOTE: the ClassWizard will add member functions here
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_GNUMBER_H__6C3F7A84_AD64_11D2_8173_00802966F8C1__INCLUDED_)
