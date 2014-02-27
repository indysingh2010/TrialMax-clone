// Tprintvw.h : interface of the CTprintvcView class
//
/////////////////////////////////////////////////////////////////////////////
#include <tmini.h>
#include <tmprint.h>
#include "tmpdlg.h"
#include "prtdlg.h"

#if !defined(AFX_TPRINTVW_H__67E2EBAE_B2F6_11D3_BF86_0080296301C0__INCLUDED_)
#define AFX_TPRINTVW_H__67E2EBAE_B2F6_11D3_BF86_0080296301C0__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class CTprintvcDoc;

class CTprintvcView : public CFormView
{
protected: // create from serialization only
	CTprintvcView();
	DECLARE_DYNCREATE(CTprintvcView)
	CTMIni	m_Ini;
	CTemplateDialog* m_pTemplateDlg;
	CPrinterDialog* m_pPrinterDlg;
	void AddToQueue(LPCSTR lpFilename, LPCSTR lpSection);
public:
	//{{AFX_DATA(CTprintvcView)
	enum { IDD = IDD_TPRINTVC_FORM };
	CTMPrint	m_TMPrint;
	//}}AFX_DATA

// Attributes
public:
	CTprintvcDoc* GetDocument();

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTprintvcView)
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
	virtual ~CTprintvcView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CTprintvcView)
	afx_msg void OnQueueAdd();
	afx_msg void OnQueueAddfromini();
	afx_msg void OnQueueFlush();
	afx_msg void OnUpdateQueueFlush(CCmdUI* pCmdUI);
	afx_msg void OnQueueGetcount();
	afx_msg void OnQueuePrint();
	afx_msg void OnUpdateQueuePrint(CCmdUI* pCmdUI);
	afx_msg void OnQueueRefresh();
	afx_msg void OnPropIncludepagetotal();
	afx_msg void OnUpdatePropIncludepagetotal(CCmdUI* pCmdUI);
	afx_msg void OnPropIncludepath();
	afx_msg void OnUpdatePropIncludepath(CCmdUI* pCmdUI);
	afx_msg void OnPropCollate();
	afx_msg void OnUpdatePropCollate(CCmdUI* pCmdUI);
	afx_msg void OnPropCopies();
	afx_msg void OnPropPrintbarcodegraphic();
	afx_msg void OnUpdatePropPrintbarcodegraphic(CCmdUI* pCmdUI);
	afx_msg void OnPropPrintbarcodetext();
	afx_msg void OnUpdatePropPrintbarcodetext(CCmdUI* pCmdUI);
	afx_msg void OnPropPrintcellborder();
	afx_msg void OnUpdatePropPrintcellborder(CCmdUI* pCmdUI);
	afx_msg void OnPropPrintdeponent();
	afx_msg void OnUpdatePropPrintdeponent(CCmdUI* pCmdUI);
	afx_msg void OnPropPrintfilename();
	afx_msg void OnUpdatePropPrintfilename(CCmdUI* pCmdUI);
	afx_msg void OnPropPrintimage();
	afx_msg void OnUpdatePropPrintimage(CCmdUI* pCmdUI);
	afx_msg void OnPropPrintname();
	afx_msg void OnUpdatePropPrintname(CCmdUI* pCmdUI);
	afx_msg void OnPropPrintpagenumber();
	afx_msg void OnUpdatePropPrintpagenumber(CCmdUI* pCmdUI);
	afx_msg void OnPropTemplatename();
	afx_msg void OnFirstTemplateTmprintctrl1(LPCTSTR lpszTemplate);
	afx_msg void OnNextTemplateTmprintctrl1(LPCTSTR lpszTemplate);
	afx_msg void OnFirstPrinterTmprintctrl1(LPCTSTR lpszPrinter);
	afx_msg void OnNextPrinterTmprintctrl1(LPCTSTR lpszPrinter);
	afx_msg void OnPropPrinter();
	afx_msg void OnGetPrinter();
	afx_msg void OnPropForcenewpage();
	afx_msg void OnUpdatePropForcenewpage(CCmdUI* pCmdUI);
	afx_msg void OnPropUseslideids();
	afx_msg void OnUpdatePropUseslideids(CCmdUI* pCmdUI);
	afx_msg void OnPropShowoptions();
	afx_msg void OnUpdatePropShowoptions(CCmdUI* pCmdUI);
	afx_msg void OnInvisible();
	afx_msg void OnSelectPrinter();
	afx_msg void OnClassId();
	afx_msg void OnRegisteredPath();
	DECLARE_EVENTSINK_MAP()
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // debug version in Tprintvw.cpp
inline CTprintvcDoc* CTprintvcView::GetDocument()
   { return (CTprintvcDoc*)m_pDocument; }
#endif

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TPRINTVW_H__67E2EBAE_B2F6_11D3_BF86_0080296301C0__INCLUDED_)
