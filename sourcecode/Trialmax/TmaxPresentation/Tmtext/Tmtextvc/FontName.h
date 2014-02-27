#if !defined(AFX_FONTNAME_H__5BD9B384_B864_11D2_8173_00802966F8C1__INCLUDED_)
#define AFX_FONTNAME_H__5BD9B384_B864_11D2_8173_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// FontName.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CFontName dialog

class CFontName : public CDialog
{
// Construction
public:
	CFontName(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CFontName)
	enum { IDD = IDD_SETFONTNAME };
	CListBox	m_ctrlFonts;
	CString	m_strFont;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CFontName)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CFontName)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_FONTNAME_H__5BD9B384_B864_11D2_8173_00802966F8C1__INCLUDED_)
