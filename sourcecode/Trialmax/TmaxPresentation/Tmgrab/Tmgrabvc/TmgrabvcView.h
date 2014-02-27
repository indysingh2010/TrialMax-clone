// TmgrabvcView.h : interface of the CTmgrabvcView class
//
/////////////////////////////////////////////////////////////////////////////
//{{AFX_INCLUDES()
#include "tmgrab.h"
//}}AFX_INCLUDES

#if !defined(AFX_TMGRABVCVIEW_H__15EF4EAC_6F06_11D6_8F0B_00802966F8C1__INCLUDED_)
#define AFX_TMGRABVCVIEW_H__15EF4EAC_6F06_11D6_8F0B_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


class CTmgrabvcView : public CFormView
{
protected: // create from serialization only
	CTmgrabvcView();
	DECLARE_DYNCREATE(CTmgrabvcView)

public:
	//{{AFX_DATA(CTmgrabvcView)
	enum { IDD = IDD_TMGRABVC_FORM };
	CTMGrab	m_TMGrab;
	//}}AFX_DATA

// Attributes
public:
	CTmgrabvcDoc* GetDocument();

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmgrabvcView)
	public:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual void OnInitialUpdate(); // called first time after construct
	virtual BOOL OnPreparePrinting(CPrintInfo* pInfo);
	virtual void OnBeginPrinting(CDC* pDC, CPrintInfo* pInfo);
	virtual void OnEndPrinting(CDC* pDC, CPrintInfo* pInfo);
	virtual void OnPrint(CDC* pDC, CPrintInfo* pInfo);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CTmgrabvcView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CTmgrabvcView)
	afx_msg void OnCapture();
	afx_msg void OnCaptureActive();
	afx_msg void OnUpdateCaptureActive(CCmdUI* pCmdUI);
	afx_msg void OnCaptureFullscreen();
	afx_msg void OnUpdateCaptureFullscreen(CCmdUI* pCmdUI);
	afx_msg void OnCaptureSelection();
	afx_msg void OnUpdateCaptureSelection(CCmdUI* pCmdUI);
	afx_msg void OnHotkey();
	afx_msg void OnUpdateHotkey(CCmdUI* pCmdUI);
	afx_msg void OnSilent();
	afx_msg void OnUpdateSilent(CCmdUI* pCmdUI);
	afx_msg void OnStop();
	afx_msg void OnCapturedTmgrabctrl1();
	DECLARE_EVENTSINK_MAP()
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // debug version in TmgrabvcView.cpp
inline CTmgrabvcDoc* CTmgrabvcView::GetDocument()
   { return (CTmgrabvcDoc*)m_pDocument; }
#endif

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMGRABVCVIEW_H__15EF4EAC_6F06_11D6_8F0B_00802966F8C1__INCLUDED_)
