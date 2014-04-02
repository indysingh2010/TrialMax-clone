// TmviewvcView.h : interface of the CTmviewvcView class
//
/////////////////////////////////////////////////////////////////////////////
#include <tmview.h>
#include <diagnose.h>

#if !defined(AFX_TMVIEWVCVIEW_H__CAEBF18E_FABA_11D0_B003_008029EFD140__INCLUDED_)
#define AFX_TMVIEWVCVIEW_H__CAEBF18E_FABA_11D0_B003_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

class CTmviewvcView : public CFormView
{
protected: // create from serialization only
	CTmviewvcView();
	DECLARE_DYNCREATE(CTmviewvcView)

public:
	//{{AFX_DATA(CTmviewvcView)
	enum { IDD = IDD_TMVIEWVC_FORM };
	CTm_view	m_ctrlTMView;
	//}}AFX_DATA

// Attributes
public:
	CTmviewvcDoc* GetDocument();
	BOOL m_bScaleImage;
	BOOL m_bFitToImage;
	BOOL m_bPanImage;
	BOOL m_bMousePan;
	CDiagnostics* m_pDiagnostics;

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmviewvcView)
	public:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	virtual void OnInitialUpdate();
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual void OnDraw(CDC* pDC);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CTmviewvcView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CTmviewvcView)
	afx_msg void OnControlAbout();
	afx_msg void OnViewProperties();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnActionRotateccw();
	afx_msg void OnPropertiesAnnotations();
	afx_msg void OnPropertiesFittoimage();
	afx_msg void OnUpdatePropertiesFittoimage(CCmdUI* pCmdUI);
	afx_msg void OnPropertiesScaleimage();
	afx_msg void OnUpdatePropertiesScaleimage(CCmdUI* pCmdUI);
	afx_msg void OnActionDraw();
	afx_msg void OnUpdateActionDraw(CCmdUI* pCmdUI);
	afx_msg void OnActionErase();
	afx_msg void OnActionHighlight();
	afx_msg void OnUpdateActionHighlight(CCmdUI* pCmdUI);
	afx_msg void OnActionNone();
	afx_msg void OnUpdateActionNone(CCmdUI* pCmdUI);
	afx_msg void OnActionRedact();
	afx_msg void OnUpdateActionRedact(CCmdUI* pCmdUI);
	afx_msg void OnActionRotatecw();
	afx_msg void OnActionZoom();
	afx_msg void OnUpdateActionZoom(CCmdUI* pCmdUI);
	afx_msg void OnActionNext();
	afx_msg void OnActionPrev();
	afx_msg void OnUpdateActionNext(CCmdUI* pCmdUI);
	afx_msg void OnUpdateActionPrev(CCmdUI* pCmdUI);
	afx_msg void OnActionSelect();
	afx_msg void OnUpdateActionSelect(CCmdUI* pCmdUI);
	afx_msg void OnMouseClickTmviewctrl1(short Button, short Key);
	afx_msg void OnMouseDblClickTmviewctrl1(short Button, short Key);
	afx_msg void OnActionResetzoom();
	afx_msg void OnUpdateActionResetzoom(CCmdUI* pCmdUI);
	afx_msg void OnActionPlay();
	afx_msg void OnUpdateActionPlay(CCmdUI* pCmdUI);
	afx_msg void OnActionContinuous();
	afx_msg void OnUpdateActionContinuous(CCmdUI* pCmdUI);
	afx_msg void OnActionStop();
	afx_msg void OnUpdateActionStop(CCmdUI* pCmdUI);
	afx_msg void OnActionLoadann();
	afx_msg void OnActionRealize();
	afx_msg void OnActionSaveann();
	afx_msg void OnActionZoomheight();
	afx_msg void OnActionZoomwidth();
	afx_msg void OnFileSaveas();
	afx_msg void OnPandown();
	afx_msg void OnPanleft();
	afx_msg void OnPanright();
	afx_msg void OnPanup();
	afx_msg void OnUpdatePanup(CCmdUI* pCmdUI);
	afx_msg void OnUpdatePanright(CCmdUI* pCmdUI);
	afx_msg void OnUpdatePanleft(CCmdUI* pCmdUI);
	afx_msg void OnUpdatePandown(CCmdUI* pCmdUI);
	afx_msg void OnPropertiesPanimage();
	afx_msg void OnUpdatePropertiesPanimage(CCmdUI* pCmdUI);
	afx_msg void OnCallout();
	afx_msg void OnActionLoadview();
	afx_msg void OnMousepan();
	afx_msg void OnUpdateMousepan(CCmdUI* pCmdUI);
	afx_msg void OnUpdateCallout(CCmdUI* pCmdUI);
	afx_msg void OnActionLoadscaled();
	afx_msg void OnActionLoadviewcall();
	afx_msg void OnActionLoadscalecall();
	afx_msg void OnActionPan();
	afx_msg void OnUpdateActionPan(CCmdUI* pCmdUI);
	afx_msg void OnActionSetprinter();
	afx_msg void OnSplitscreen();
	afx_msg void OnUpdateSplitscreen(CCmdUI* pCmdUI);
	afx_msg void OnLoadactive();
	afx_msg void OnLoadleft();
	afx_msg void OnLoadright();
	afx_msg void OnUpdateLoadright(CCmdUI* pCmdUI);
	afx_msg void OnSyncprops();
	afx_msg void OnUpdateSyncprops(CCmdUI* pCmdUI);
	afx_msg void OnPrintFullboth();
	afx_msg void OnPrintFullleft();
	afx_msg void OnUpdatePrintFullleft(CCmdUI* pCmdUI);
	afx_msg void OnPrintFullright();
	afx_msg void OnUpdatePrintFullright(CCmdUI* pCmdUI);
	afx_msg void OnPrintVisibleboth();
	afx_msg void OnPrintVisibleleft();
	afx_msg void OnUpdatePrintVisibleleft(CCmdUI* pCmdUI);
	afx_msg void OnPrintVisibleright();
	afx_msg void OnUpdatePrintVisibleright(CCmdUI* pCmdUI);
	afx_msg void OnSelectPaneTmviewctrl1(short sPane);
	afx_msg void OnPropertiesSynccallann();
	afx_msg void OnUpdatePropertiesSynccallann(CCmdUI* pCmdUI);
	afx_msg void OnDeletelast();
	afx_msg void OnDeleteselections();
	afx_msg void OnUpdateDeleteselections(CCmdUI* pCmdUI);
	afx_msg void OnPropertiesSelector();
	afx_msg void OnUpdatePropertiesSelector(CCmdUI* pCmdUI);
	afx_msg void OnPropertiesKeepaspect();
	afx_msg void OnUpdatePropertiesKeepaspect(CCmdUI* pCmdUI);
	afx_msg void OnPropertiesZoomtorect();
	afx_msg void OnUpdatePropertiesZoomtorect(CCmdUI* pCmdUI);
	afx_msg void OnCopyclipboard();
	afx_msg void OnUpdateCopyclipboard(CCmdUI* pCmdUI);
	afx_msg void OnPasteclipboard();
	afx_msg void OnUpdatePasteclipboard(CCmdUI* pCmdUI);
	afx_msg void OnMouseMoveTmviewctrl1(short Button, short Shift, long x, long y);
	afx_msg void OnImageInformation();
	afx_msg void OnUpdateImageInformation(CCmdUI* pCmdUI);
	afx_msg void OnPrinterDefault();
	afx_msg void OnPrinterCurrent();
	afx_msg void OnDeskew();
	afx_msg void OnDeskewBackColor();
	afx_msg void OnViewRegisteredPath();
	afx_msg void OnViewClassId();
	afx_msg void OnPropertiesAnnotatecallouts();
	afx_msg void OnUpdatePropertiesAnnotatecallouts(CCmdUI* pCmdUI);
	afx_msg void OnDespeckle();
	afx_msg void OnDotRemove();
	afx_msg void OnHoleRemove();
	afx_msg void OnSmooth();
	afx_msg void OnBorderRemove();
	afx_msg void OnCleanup();
	afx_msg void OnPageSetup();
	afx_msg void OnSplitHorizontal();
	afx_msg void OnUpdateSplitHorizontal(CCmdUI* pCmdUI);
	afx_msg void OnQfactor();
	afx_msg void OnSelectCalloutTmviewctrl1(long hCallout, short sPane);
	afx_msg void OnShowDiagnostics();
	afx_msg void OnHideDiagnostics();
	afx_msg void OnDrawRectangle();
	afx_msg void OnUpdateDrawRectangle(CCmdUI* pCmdUI);
	afx_msg void OnSourceRectangle();
	afx_msg void OnUpdateSourceRectangle(CCmdUI* pCmdUI);
	afx_msg void OnAnnotationDeletedTmviewctrl1(long lAnnotation, short sPane);
	afx_msg void OnAnnotationModifiedTmviewctrl1(long lAnnotation, short sPane);
	afx_msg void OnAnnotationDrawnTmviewctrl1(long lAnnotation, short sPane);
	afx_msg void OnCalloutResizedTmviewctrl1(long hCallout, short sPane);
	afx_msg void OnCalloutMovedTmviewctrl1(long hCallout, short sPane);
	afx_msg void OnViewEvents();
	afx_msg void OnUpdateViewEvents(CCmdUI* pCmdUI);
	afx_msg void OnMouseDownTmviewctrl1(short Button, short Shift, long x, long y);
	afx_msg void OnMouseUpTmviewctrl1(short Button, short Shift, long x, long y);
	afx_msg void OnFileSave();
	afx_msg void OnPrintSetProperties();
	afx_msg void OnSavePages();
	afx_msg void OnUpdateSavePages(CCmdUI* pCmdUI);
	afx_msg void OnPrinterCaps();
	DECLARE_EVENTSINK_MAP()
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // debug version in TmviewvcView.cpp
inline CTmviewvcDoc* CTmviewvcView::GetDocument()
   { return (CTmviewvcDoc*)m_pDocument; }
#endif

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMVIEWVCVIEW_H__CAEBF18E_FABA_11D0_B003_008029EFD140__INCLUDED_)
