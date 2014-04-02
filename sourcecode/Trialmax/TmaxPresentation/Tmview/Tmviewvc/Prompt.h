#if !defined(AFX_PROMPT_H__1B948B45_6358_11D6_8F0B_00802966F8C1__INCLUDED_)
#define AFX_PROMPT_H__1B948B45_6358_11D6_8F0B_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Prompt.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CPrompt dialog

class CPrompt : public CDialog
{
// Construction
public:
	CPrompt(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CPrompt)
	enum { IDD = IDD_PROMPT };
	CString	m_strTitle;
	CString	m_strValue;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CPrompt)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CPrompt)
		// NOTE: the ClassWizard will add member functions here
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_PROMPT_H__1B948B45_6358_11D6_8F0B_00802966F8C1__INCLUDED_)
