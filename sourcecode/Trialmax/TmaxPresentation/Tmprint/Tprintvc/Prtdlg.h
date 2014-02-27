#if !defined(AFX_PRTDLG_H__64E3C221_CB70_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_PRTDLG_H__64E3C221_CB70_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Prtdlg.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CPrinterDialog dialog
#define MAX_PRINTERS	50

class CPrinterDialog : public CDialog
{
// Construction
public:
	CPrinterDialog(CWnd* pParent = NULL);   // standard constructor

	CString m_Printers[MAX_PRINTERS];
	int m_iPrinters;
// Dialog Data
	//{{AFX_DATA(CPrinterDialog)
	enum { IDD = IDD_PRINTERS };
	CListBox	m_ctrlPrinters;
	CString	m_strPrinter;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CPrinterDialog)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CPrinterDialog)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_PRTDLG_H__64E3C221_CB70_11D3_8177_00802966F8C1__INCLUDED_)
