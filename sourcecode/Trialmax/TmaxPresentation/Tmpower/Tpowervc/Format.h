#if !defined(AFX_FORMAT_H__BCD0CDA1_33CD_11D4_8178_00802966F8C1__INCLUDED_)
#define AFX_FORMAT_H__BCD0CDA1_33CD_11D4_8178_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Format.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CFormat dialog

class CFormat : public CDialog
{
// Construction
public:
	CFormat(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CFormat)
	enum { IDD = IDD_FORMAT };
	CListBox	m_ctrlFormats;
	int		m_iFormat;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CFormat)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CFormat)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_FORMAT_H__BCD0CDA1_33CD_11D4_8178_00802966F8C1__INCLUDED_)
