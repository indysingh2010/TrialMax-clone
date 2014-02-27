//{{AFX_INCLUDES()
//}}AFX_INCLUDES
#if !defined(AFX_INVIS_H__6833E4C1_2979_11D4_8178_00802966F8C1__INCLUDED_)
#define AFX_INVIS_H__6833E4C1_2979_11D4_8178_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Invis.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CInvisible dialog
#include <tmprint.h>

class CInvisible : public CDialog
{
// Construction
public:
	CInvisible(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CInvisible)
	enum { IDD = IDD_INVISIBLE };
	CTMPrint	m_TMPrint;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CInvisible)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CInvisible)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_INVIS_H__6833E4C1_2979_11D4_8178_00802966F8C1__INCLUDED_)
