#if !defined(AFX_BMASK_H__F68B7C61_DB09_11D1_B16F_008029EFD140__INCLUDED_)
#define AFX_BMASK_H__F68B7C61_DB09_11D1_B16F_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000
// Bmask.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CButtonMask dialog

class CButtonMask : public CDialog
{
// Construction
public:
	CButtonMask(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CButtonMask)
	enum { IDD = IDD_BUTTONMASK };
	CString	m_strMask;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CButtonMask)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CButtonMask)
		// NOTE: the ClassWizard will add member functions here
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_BMASK_H__F68B7C61_DB09_11D1_B16F_008029EFD140__INCLUDED_)
