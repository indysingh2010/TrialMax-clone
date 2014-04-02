#if !defined(AFX_GETPROP_H__F15AFEC1_2C98_11D3_8176_00802966F8C1__INCLUDED_)
#define AFX_GETPROP_H__F15AFEC1_2C98_11D3_8176_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// GetProp.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CGetProp dialog

class CGetProp : public CDialog
{
// Construction
public:
	CGetProp(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CGetProp)
	enum { IDD = IDD_GETPROPERTY };
	CString	m_strName;
	CString	m_strValue;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CGetProp)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CGetProp)
		// NOTE: the ClassWizard will add member functions here
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_GETPROP_H__F15AFEC1_2C98_11D3_8176_00802966F8C1__INCLUDED_)
