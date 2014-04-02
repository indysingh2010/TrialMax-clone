// TpowervcView.h : interface of the CTpowervcView class
//
/////////////////////////////////////////////////////////////////////////////
//{{AFX_INCLUDES()
//}}AFX_INCLUDES

#if !defined(AFX_TPOWERVCVIEW_H__F0E8E9CE_058D_11D3_8175_00802966F8C1__INCLUDED_)
#define AFX_TPOWERVCVIEW_H__F0E8E9CE_058D_11D3_8175_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include <tmpower.h>

class CTpowervcView : public CFormView
{
protected: // create from serialization only
	CTpowervcView();
	DECLARE_DYNCREATE(CTpowervcView)

public:
	//{{AFX_DATA(CTpowervcView)
	enum { IDD = IDD_TPOWERVC_FORM };
	CTMPower	m_Power;
	//}}AFX_DATA
	CStatusBar*  m_pStatus;

// Attributes
public:
	CTpowervcDoc* GetDocument();

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTpowervcView)
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
	virtual ~CTpowervcView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CTpowervcView)
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnMethodGetppversion();
	afx_msg void OnUpdateMethodGetppversion(CCmdUI* pCmdUI);
	afx_msg void OnPropertySplitframecolor();
	afx_msg void OnUpdatePropertySplitframecolor(CCmdUI* pCmdUI);
	afx_msg void OnPropertySplitframethickness();
	afx_msg void OnUpdatePropertySplitframethickness(CCmdUI* pCmdUI);
	afx_msg void OnPropertySplitscreen();
	afx_msg void OnUpdatePropertySplitscreen(CCmdUI* pCmdUI);
	afx_msg void OnPropertySyncviews();
	afx_msg void OnUpdatePropertySyncviews(CCmdUI* pCmdUI);
	afx_msg void OnFileReload();
	afx_msg void OnUpdateFileReload(CCmdUI* pCmdUI);
	afx_msg void OnMethodNext();
	afx_msg void OnUpdateMethodNext(CCmdUI* pCmdUI);
	afx_msg void OnMethodPrevious();
	afx_msg void OnUpdateMethodPrevious(CCmdUI* pCmdUI);
	afx_msg void OnMethodFirst();
	afx_msg void OnUpdateMethodFirst(CCmdUI* pCmdUI);
	afx_msg void OnMethodLast();
	afx_msg void OnUpdateMethodLast(CCmdUI* pCmdUI);
	afx_msg void OnFileChangedTmpowerctrl1(LPCTSTR lpszFilename, short sView);
	afx_msg void OnSlideChangedTmpowerctrl1(long lSlide, short sView);
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnMethodSetslide();
	afx_msg void OnUpdateMethodSetslide(CCmdUI* pCmdUI);
	afx_msg void OnPropertyStartslide();
	afx_msg void OnMethodCopyslide();
	afx_msg void OnUpdateMethodCopyslide(CCmdUI* pCmdUI);
	afx_msg void OnMethodSaveslide();
	afx_msg void OnUpdateMethodSaveslide(CCmdUI* pCmdUI);
	afx_msg void OnPropertyEnableaccelerators();
	afx_msg void OnUpdatePropertyEnableaccelerators(CCmdUI* pCmdUI);
	afx_msg void OnViewFocusTmpowerctrl1(short sView);
	afx_msg void OnPropertyUseslideid();
	afx_msg void OnUpdatePropertyUseslideid(CCmdUI* pCmdUI);
	afx_msg void OnPropertySaveformat();
	afx_msg void OnUpdatePropertySaveformat(CCmdUI* pCmdUI);
	afx_msg void OnClassId();
	afx_msg void OnRegistrationPath();
	afx_msg void OnMethodLoadFile();
	afx_msg void OnMethodAnimations();
	DECLARE_EVENTSINK_MAP()
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // debug version in TpowervcView.cpp
inline CTpowervcDoc* CTpowervcView::GetDocument()
   { return (CTpowervcDoc*)m_pDocument; }
#endif

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TPOWERVCVIEW_H__F0E8E9CE_058D_11D3_8175_00802966F8C1__INCLUDED_)
