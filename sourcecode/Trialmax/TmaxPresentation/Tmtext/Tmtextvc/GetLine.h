#if !defined(AFX_GETLINE_H__9C826640_B6F5_11D2_8173_00802966F8C1__INCLUDED_)
#define AFX_GETLINE_H__9C826640_B6F5_11D2_8173_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// GetLine.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CGetLine dialog

class CGetLine : public CDialog
{
// Construction
public:
	CGetLine(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CGetLine)
	enum { IDD = IDD_GETPAGELINE };
	long	m_lDesignation;
	short	m_sLine;
	short	m_sPage;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CGetLine)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CGetLine)
		// NOTE: the ClassWizard will add member functions here
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_GETLINE_H__9C826640_B6F5_11D2_8173_00802966F8C1__INCLUDED_)
