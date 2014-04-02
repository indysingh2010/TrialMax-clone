#if !defined(AFX_ADD_H__FACC2041_BC55_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_ADD_H__FACC2041_BC55_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Add.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CAdd dialog

class CAdd : public CDialog
{
// Construction
public:
	CAdd(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CAdd)
	enum { IDD = IDD_ADD };
	CString	m_strString;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAdd)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CAdd)
		// NOTE: the ClassWizard will add member functions here
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_ADD_H__FACC2041_BC55_11D3_8177_00802966F8C1__INCLUDED_)
