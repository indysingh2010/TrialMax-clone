// TmbrowsevcView.h : interface of the CTmbrowsevcView class
//
/////////////////////////////////////////////////////////////////////////////
//{{AFX_INCLUDES()
//}}AFX_INCLUDES

#if !defined(AFX_TMBROWSEVCVIEW_H__E6904C25_1D6F_11D6_8F0B_00802966F8C1__INCLUDED_)
#define AFX_TMBROWSEVCVIEW_H__E6904C25_1D6F_11D6_8F0B_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include <tmbrowse.h>

class CTmbrowsevcView : public CFormView
{
protected: // create from serialization only
	CTmbrowsevcView();
	DECLARE_DYNCREATE(CTmbrowsevcView)

public:
	//{{AFX_DATA(CTmbrowsevcView)
	enum { IDD = IDD_TMBROWSEVC_FORM };
	CTMBrowse	m_Browser;
	//}}AFX_DATA

BOOL CopyFile(LPCSTR lpSource, LPCSTR lpTarget);

// Attributes
public:
	CTmbrowsevcDoc* GetDocument();

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmbrowsevcView)
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
	virtual ~CTmbrowsevcView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CTmbrowsevcView)
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnFileClose();
	afx_msg void OnCopy();
	afx_msg void OnUpdateCopy(CCmdUI* pCmdUI);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // debug version in TmbrowsevcView.cpp
inline CTmbrowsevcDoc* CTmbrowsevcView::GetDocument()
   { return (CTmbrowsevcDoc*)m_pDocument; }
#endif

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMBROWSEVCVIEW_H__E6904C25_1D6F_11D6_8F0B_00802966F8C1__INCLUDED_)
