#if !defined(AFX_TMPDLG_H__B4268321_CB63_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_TMPDLG_H__B4268321_CB63_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Tmpdlg.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CTemplateDialog dialog
#define MAX_TEMPLATES	100

class CTemplateDialog : public CDialog
{
// Construction
public:
	CTemplateDialog(CWnd* pParent = NULL);   // standard constructor
	
	CString m_Templates[MAX_TEMPLATES];
	int m_iTemplates;

// Dialog Data
	//{{AFX_DATA(CTemplateDialog)
	enum { IDD = IDD_TEMPLATES };
	CListBox	m_ctrlTemplates;
	CString	m_strTemplate;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTemplateDialog)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CTemplateDialog)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMPDLG_H__B4268321_CB63_11D3_8177_00802966F8C1__INCLUDED_)
