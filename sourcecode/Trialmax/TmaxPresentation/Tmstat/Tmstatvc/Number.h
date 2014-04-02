#if !defined(AFX_NUMBER_H__6381FAA0_C284_11D2_8173_00802966F8C1__INCLUDED_)
#define AFX_NUMBER_H__6381FAA0_C284_11D2_8173_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Number.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// GetNumber dialog

class GetNumber : public CDialog
{
// Construction
public:
	GetNumber(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(GetNumber)
	enum { IDD = IDD_GETNUMBER };
	CString	m_strLabel;
	short	m_sNumber;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(GetNumber)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(GetNumber)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_NUMBER_H__6381FAA0_C284_11D2_8173_00802966F8C1__INCLUDED_)
