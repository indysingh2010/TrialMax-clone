// TmsetvcView.h : interface of the CTmsetvcView class
//
/////////////////////////////////////////////////////////////////////////////
//{{AFX_INCLUDES()
#include "tmsetup.h"
//}}AFX_INCLUDES

#if !defined(AFX_TMSETVCVIEW_H__110691EF_D4C7_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_TMSETVCVIEW_H__110691EF_D4C7_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


class CTmsetvcView : public CFormView
{
protected: // create from serialization only
	CTmsetvcView();
	DECLARE_DYNCREATE(CTmsetvcView)

public:
	//{{AFX_DATA(CTmsetvcView)
	enum { IDD = IDD_TMSETVC_FORM };
	CTMSetup	m_TMSetup;
	//}}AFX_DATA

// Attributes
public:
	CTmsetvcDoc* GetDocument();

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmsetvcView)
	public:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual void OnInitialUpdate(); // called first time after construct
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CTmsetvcView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CTmsetvcView)
	afx_msg void OnPropertyAboutpage();
	afx_msg void OnPropertyDatabasepage();
	afx_msg void OnPropertyDiagnosticpage();
	afx_msg void OnPropertyDirectxpage();
	afx_msg void OnPropertyGraphicspage();
	afx_msg void OnPropertySystempage();
	afx_msg void OnPropertyTextpage();
	afx_msg void OnPropertyVideopage();
	afx_msg void OnUpdatePropertyAboutpage(CCmdUI* pCmdUI);
	afx_msg void OnUpdatePropertyDatabasepage(CCmdUI* pCmdUI);
	afx_msg void OnUpdatePropertyDiagnosticpage(CCmdUI* pCmdUI);
	afx_msg void OnUpdatePropertyDirectxpage(CCmdUI* pCmdUI);
	afx_msg void OnUpdatePropertyGraphicspage(CCmdUI* pCmdUI);
	afx_msg void OnUpdatePropertySystempage(CCmdUI* pCmdUI);
	afx_msg void OnUpdatePropertyTextpage(CCmdUI* pCmdUI);
	afx_msg void OnUpdatePropertyVideopage(CCmdUI* pCmdUI);
	afx_msg void OnMethodSave();
	afx_msg void OnSave();
	afx_msg void OnViewClassId();
	afx_msg void OnViewRegisteredPath();
	afx_msg void OnEnumVersions();
	afx_msg void OnAxVersionCtmsetupctrl1(LPCTSTR lpszName, LPCTSTR lpszDescription, short sMajorVer, short sMinorVer, short sQEF, short sBuild, LPCTSTR lpszShortText, LPCTSTR lpszLongText, LPCTSTR lpszBuildDate, LPCTSTR lpszClsId, LPCTSTR lpszPath);
	DECLARE_EVENTSINK_MAP()
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // debug version in TmsetvcView.cpp
inline CTmsetvcDoc* CTmsetvcView::GetDocument()
   { return (CTmsetvcDoc*)m_pDocument; }
#endif

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMSETVCVIEW_H__110691EF_D4C7_11D3_8177_00802966F8C1__INCLUDED_)
