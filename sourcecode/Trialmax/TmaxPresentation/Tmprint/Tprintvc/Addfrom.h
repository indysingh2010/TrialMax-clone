#if !defined(AFX_ADDFROM_H__FACC2042_BC55_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_ADDFROM_H__FACC2042_BC55_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Addfrom.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CAddFromIni dialog

class CAddFromIni : public CDialog
{
// Construction
public:
	CAddFromIni(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CAddFromIni)
	enum { IDD = IDD_ADDFROMINI };
	CString	m_strFilename;
	CString	m_strSection;
	BOOL	m_bFlush;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAddFromIni)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CAddFromIni)
		// NOTE: the ClassWizard will add member functions here
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_ADDFROM_H__FACC2042_BC55_11D3_8177_00802966F8C1__INCLUDED_)
