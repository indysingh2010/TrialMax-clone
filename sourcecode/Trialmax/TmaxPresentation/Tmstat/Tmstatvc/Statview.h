// Statview.h : interface of the CTmstatvcView class
//
/////////////////////////////////////////////////////////////////////////////
//{{AFX_INCLUDES()
#include "tmstat.h"
//}}AFX_INCLUDES

#if !defined(AFX_STATVIEW_H__44ADEC36_C265_11D2_8173_00802966F8C1__INCLUDED_)
#define AFX_STATVIEW_H__44ADEC36_C265_11D2_8173_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


class CTmstatvcView : public CFormView
{
protected: // create from serialization only
	CTmstatvcView();
	DECLARE_DYNCREATE(CTmstatvcView)

public:
	//{{AFX_DATA(CTmstatvcView)
	enum { IDD = IDD_TMSTATVC_FORM };
	CTMStat	m_TMStat;
	//}}AFX_DATA

// Attributes
public:
	CTmstatvcDoc* GetDocument();
	void MoveStatusBar();
	BOOL m_bTimerActive;
// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmstatvcView)
	public:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual void OnInitialUpdate(); // called first time after construct
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CTmstatvcView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CTmstatvcView)
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnPropAutosize();
	afx_msg void OnUpdatePropAutosize(CCmdUI* pCmdUI);
	afx_msg void OnPropBackcolor();
	afx_msg void OnPropFont();
	afx_msg void OnPropForecolor();
	afx_msg void OnPropText();
	afx_msg void OnPropBottommargin();
	afx_msg void OnPropLeftmargin();
	afx_msg void OnPropRightmargin();
	afx_msg void OnPropTopmargin();
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg void OnTimeofday();
	afx_msg void OnUpdateTimeofday(CCmdUI* pCmdUI);
	afx_msg void OnPropFlat();
	afx_msg void OnUpdatePropFlat(CCmdUI* pCmdUI);
	afx_msg void OnClickTmstatctrl1();
	afx_msg void OnDblClickTmstatctrl1();
	afx_msg void OnViewClassId();
	afx_msg void OnViewRegisteredPath();
	DECLARE_EVENTSINK_MAP()
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // debug version in Statview.cpp
inline CTmstatvcDoc* CTmstatvcView::GetDocument()
   { return (CTmstatvcDoc*)m_pDocument; }
#endif

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_STATVIEW_H__44ADEC36_C265_11D2_8173_00802966F8C1__INCLUDED_)
