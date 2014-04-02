#if !defined(AFX_SPLASH_H__D3EE0091_ED01_11D1_B187_008029EFD140__INCLUDED_)
#define AFX_SPLASH_H__D3EE0091_ED01_11D1_B187_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000
// splash.h : header file
//
#include <resource.h>
/////////////////////////////////////////////////////////////////////////////
// CSplashBox dialog

class CSplashBox : public CDialog
{
	CBrush	m_brBackground;

// Construction
public:
	CSplashBox(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CSplashBox)
	enum { IDD = IDD_SPLASHBOX };
	CStatic	m_ctrlBitmap;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CSplashBox)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CSplashBox)
	virtual BOOL OnInitDialog();
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SPLASH_H__D3EE0091_ED01_11D1_B187_008029EFD140__INCLUDED_)
