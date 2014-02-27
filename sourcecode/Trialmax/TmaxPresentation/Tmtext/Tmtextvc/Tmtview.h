// Tmtview.h : interface of the CTmtextView class
//
/////////////////////////////////////////////////////////////////////////////
#include <tmtext.h>

#if !defined(AFX_TMTVIEW_H__07F27A66_ABF9_11D2_8C08_00802966F8C1__INCLUDED_)
#define AFX_TMTVIEW_H__07F27A66_ABF9_11D2_8C08_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


class CTmtextView : public CFormView
{
protected: // create from serialization only
	CTmtextView();
	DECLARE_DYNCREATE(CTmtextView)

public:

	//{{AFX_DATA(CTmtextView)
	enum { IDD = IDD_TMTEXTVC_FORM };
	CTMText	m_TMText;
	//}}AFX_DATA

// Attributes
public:
	CTmtextDoc* GetDocument();

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmtextView)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual void OnInitialUpdate(); // called first time after construct
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CTmtextView();

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CTmtextView)
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnPropBackColor();
	afx_msg void OnPropForeColor();
	afx_msg void OnPropHighlightColor();
	afx_msg void OnPropHighTextColor();
	afx_msg void OnPropFont();
	afx_msg void OnPropHighLines();
	afx_msg void OnPropDisplayLines();
	afx_msg void OnPropCombine();
	afx_msg void OnUpdatePropCombine(CCmdUI* pCmdUI);
	afx_msg void OnPropEnableErrors();
	afx_msg void OnUpdatePropEnableErrors(CCmdUI* pCmdUI);
	afx_msg void OnPropShowPgln();
	afx_msg void OnUpdatePropShowPgln(CCmdUI* pCmdUI);
	afx_msg void OnHeightChangeTmtextctrl1(short sHeight);
	afx_msg void OnPropBottommargin();
	afx_msg void OnPropLeftmargin();
	afx_msg void OnPropMaxcharsperline();
	afx_msg void OnPropResizeonchange();
	afx_msg void OnUpdatePropResizeonchange(CCmdUI* pCmdUI);
	afx_msg void OnPropRightmargin();
	afx_msg void OnPropTopmargin();
	afx_msg void OnPropUseavgcharwidth();
	afx_msg void OnUpdatePropUseavgcharwidth(CCmdUI* pCmdUI);
	afx_msg void OnMethodGetminheight();
	afx_msg void OnMethodResizefont();
	afx_msg void OnMethodNext();
	afx_msg void OnUpdateMethodNext(CCmdUI* pCmdUI);
	afx_msg void OnMethodPrevious();
	afx_msg void OnUpdateMethodPrevious(CCmdUI* pCmdUI);
	afx_msg void OnMethodSetline();
	afx_msg void OnMethodGetcharwidth();
	afx_msg void OnMethodGetcharheight();
	afx_msg void OnMethodBarline();
	afx_msg void OnMethodSetfontbyname();
	afx_msg void OnPropScrollsteps();
	afx_msg void OnPropScrolltime();
	afx_msg void OnPropSmoothscroll();
	afx_msg void OnUpdatePropSmoothscroll(CCmdUI* pCmdUI);
	afx_msg void OnClassId();
	afx_msg void OnRegisteredPath();
	DECLARE_EVENTSINK_MAP()
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // debug version in TmtextvcView.cpp
inline CTmtextDoc* CTmtextView::GetDocument()
   { return (CTmtextDoc*)m_pDocument; }
#endif

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMTVIEW_H__07F27A66_ABF9_11D2_8C08_00802966F8C1__INCLUDED_)
