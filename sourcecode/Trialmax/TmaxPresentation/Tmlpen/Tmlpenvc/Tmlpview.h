// Tmlpview.h : interface of the CTmlpenvcView class
//
/////////////////////////////////////////////////////////////////////////////
//{{AFX_INCLUDES()
#include "tmlpen.h"
//}}AFX_INCLUDES

#if !defined(AFX_TMLPVIEW_H__52B397B5_A291_11D2_8BFC_00802966F8C1__INCLUDED_)
#define AFX_TMLPVIEW_H__52B397B5_A291_11D2_8BFC_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


class CTmlpenvcView : public CFormView
{
protected: // create from serialization only
	CTmlpenvcView();
	DECLARE_DYNCREATE(CTmlpenvcView)

public:
	//{{AFX_DATA(CTmlpenvcView)
	enum { IDD = IDD_TMLPENVC_FORM };
	CTMLpen	m_TMLpen;
	//}}AFX_DATA

// Attributes
public:
	CTmlpenvcDoc* GetDocument();

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmlpenvcView)
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
	virtual ~CTmlpenvcView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CTmlpenvcView)
	afx_msg void OnPropAppearance();
	afx_msg void OnUpdatePropAppearance(CCmdUI* pCmdUI);
	afx_msg void OnPropBackcolor();
	afx_msg void OnPropForecolor();
	afx_msg void OnPropBorder();
	afx_msg void OnUpdatePropBorder(CCmdUI* pCmdUI);
	afx_msg void OnMouseClickTmlpenctrl1(short sButton, short sKeyState);
	afx_msg void OnViewClassId();
	afx_msg void OnViewRegisteredPath();
	DECLARE_EVENTSINK_MAP()
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // debug version in Tmlpview.cpp
inline CTmlpenvcDoc* CTmlpenvcView::GetDocument()
   { return (CTmlpenvcDoc*)m_pDocument; }
#endif

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMLPVIEW_H__52B397B5_A291_11D2_8BFC_00802966F8C1__INCLUDED_)
