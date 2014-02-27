// Tmtview.h : interface of the CTmtoolvcView class
//
/////////////////////////////////////////////////////////////////////////////
//{{AFX_INCLUDES()

//}}AFX_INCLUDES

#if !defined(AFX_TMTVIEW_H__BBD917B9_D89D_11D1_B16C_008029EFD140__INCLUDED_)
#define AFX_TMTVIEW_H__BBD917B9_D89D_11D1_B16C_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000
#include <tmtool.h>

class CTmtoolvcView : public CFormView
{
	BOOL bStretch;
	BOOL bRaised;
	BOOL bPlaying;
	BOOL bSplit;
	BOOL bDisableLinks;

protected: // create from serialization only
	CTmtoolvcView();
	DECLARE_DYNCREATE(CTmtoolvcView)

public:
	//{{AFX_DATA(CTmtoolvcView)
	enum { IDD = IDD_TMTOOLVC_FORM };
	CTMTool	m_TMTool;
	//}}AFX_DATA

// Attributes
public:
	CTmtoolvcDoc* GetDocument();

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmtoolvcView)
	public:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnPreparePrinting(CPrintInfo* pInfo);
	virtual void OnBeginPrinting(CDC* pDC, CPrintInfo* pInfo);
	virtual void OnEndPrinting(CDC* pDC, CPrintInfo* pInfo);
	virtual void OnPrint(CDC* pDC, CPrintInfo*);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CTmtoolvcView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CTmtoolvcView)
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnDockbottom();
	afx_msg void OnUpdateDockbottom(CCmdUI* pCmdUI);
	afx_msg void OnDockleft();
	afx_msg void OnUpdateDockleft(CCmdUI* pCmdUI);
	afx_msg void OnDockright();
	afx_msg void OnUpdateDockright(CCmdUI* pCmdUI);
	afx_msg void OnDocktop();
	afx_msg void OnUpdateDocktop(CCmdUI* pCmdUI);
	afx_msg void OnRaised();
	afx_msg void OnUpdateRaised(CCmdUI* pCmdUI);
	afx_msg void OnStretch();
	afx_msg void OnUpdateStretch(CCmdUI* pCmdUI);
	afx_msg void OnLargebuttons();
	afx_msg void OnUpdateLargebuttons(CCmdUI* pCmdUI);
	afx_msg void OnMediumbuttons();
	afx_msg void OnUpdateMediumbuttons(CCmdUI* pCmdUI);
	afx_msg void OnSmallbuttons();
	afx_msg void OnUpdateSmallbuttons(CCmdUI* pCmdUI);
	afx_msg void OnBackground();
	afx_msg void OnButtonmask();
	afx_msg void OnButtonClickTmtoolctrl1(short sId, BOOL bChecked);
	afx_msg void OnTooltips();
	afx_msg void OnUpdateTooltips(CCmdUI* pCmdUI);
	afx_msg void OnToggleplay();
	afx_msg void OnTogglesplit();
	afx_msg void OnTogglelinks();
	afx_msg void OnConfigurable();
	afx_msg void OnUpdateConfigurable(CCmdUI* pCmdUI);
	afx_msg void OnConfigure();
	afx_msg void OnViewClassId();
	afx_msg void OnViewRegisteredPath();
	afx_msg void OnButtonRows();
	afx_msg void OnAutoReset();
	afx_msg void OnReset();
	afx_msg void OnUpdateAutoReset(CCmdUI* pCmdUI);
	DECLARE_EVENTSINK_MAP()
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // debug version in Tmtview.cpp
inline CTmtoolvcDoc* CTmtoolvcView::GetDocument()
   { return (CTmtoolvcDoc*)m_pDocument; }
#endif

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMTVIEW_H__BBD917B9_D89D_11D1_B16C_008029EFD140__INCLUDED_)
