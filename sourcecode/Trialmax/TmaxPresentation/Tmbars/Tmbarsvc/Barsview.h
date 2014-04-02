// Barsview.h : interface of the CTmbarsvcView class
//
/////////////////////////////////////////////////////////////////////////////
//{{AFX_INCLUDES()
#include "tmbars.h"
//}}AFX_INCLUDES

#if !defined(AFX_BARSVIEW_H__3F4BEF25_C5E5_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_BARSVIEW_H__3F4BEF25_C5E5_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


class CTmbarsvcView : public CFormView
{
protected: // create from serialization only
	CTmbarsvcView();
	DECLARE_DYNCREATE(CTmbarsvcView)

public:
	//{{AFX_DATA(CTmbarsvcView)
	enum { IDD = IDD_TMBARSVC_FORM };
	CTMBars	m_TMBars;
	//}}AFX_DATA

// Attributes
public:
	CTmbarsvcDoc* GetDocument();

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmbarsvcView)
	public:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual void OnInitialUpdate(); // called first time after construct
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CTmbarsvcView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CTmbarsvcView)
	afx_msg void OnFileSave();
	afx_msg void OnFileOpen();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // debug version in Barsview.cpp
inline CTmbarsvcDoc* CTmbarsvcView::GetDocument()
   { return (CTmbarsvcDoc*)m_pDocument; }
#endif

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_BARSVIEW_H__3F4BEF25_C5E5_11D3_8177_00802966F8C1__INCLUDED_)
