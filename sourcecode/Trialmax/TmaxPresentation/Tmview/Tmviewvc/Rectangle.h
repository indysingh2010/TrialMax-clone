#if !defined(AFX_RECTANGLE_H__A3664500_C9AE_11D7_A32D_00E0290EC243__INCLUDED_)
#define AFX_RECTANGLE_H__A3664500_C9AE_11D7_A32D_00E0290EC243__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Rectangle.h : header file
//

#include <colorctl.h>

class CRectangle : public CDialog
{
// Construction
public:
	CRectangle(CWnd* pParent = NULL);   // standard constructor

	COLORREF m_crColor;

	//{{AFX_DATA(CRectangle)
	enum { IDD = IDD_RECTANGLE };
	CColorPushbutton	m_ctrlColor;
	int		m_iBottom;
	int		m_iLeft;
	int		m_iRight;
	BOOL	m_bLocked;
	int		m_iTop;
	int		m_iTransparency;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CRectangle)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CRectangle)
	virtual BOOL OnInitDialog();
	afx_msg void OnColor();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_RECTANGLE_H__A3664500_C9AE_11D7_A32D_00E0290EC243__INCLUDED_)
