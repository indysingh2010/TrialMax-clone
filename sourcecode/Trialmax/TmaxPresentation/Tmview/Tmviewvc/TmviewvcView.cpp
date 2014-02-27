// TmviewvcView.cpp : implementation of the CTmviewvcView class
//

#include "stdafx.h"
#include "Tmviewvc.h"

#include "TmviewvcDoc.h"
#include "TmviewvcView.h"
#include "prompt.h"
#include "rectangle.h"
#include "savepages.h"
#include <tmvdefs.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CTmviewvcView

IMPLEMENT_DYNCREATE(CTmviewvcView, CFormView)

BEGIN_MESSAGE_MAP(CTmviewvcView, CFormView)
	//{{AFX_MSG_MAP(CTmviewvcView)
	ON_COMMAND(ID_CONTROL_ABOUT, OnControlAbout)
	ON_COMMAND(ID_VIEW_PROPERTIES, OnViewProperties)
	ON_WM_SIZE()
	ON_COMMAND(ID_ACTION_ROTATECCW, OnActionRotateccw)
	ON_COMMAND(ID_PROPERTIES_ANNOTATIONS, OnPropertiesAnnotations)
	ON_COMMAND(ID_PROPERTIES_FITTOIMAGE, OnPropertiesFittoimage)
	ON_UPDATE_COMMAND_UI(ID_PROPERTIES_FITTOIMAGE, OnUpdatePropertiesFittoimage)
	ON_COMMAND(ID_PROPERTIES_SCALEIMAGE, OnPropertiesScaleimage)
	ON_UPDATE_COMMAND_UI(ID_PROPERTIES_SCALEIMAGE, OnUpdatePropertiesScaleimage)
	ON_COMMAND(ID_ACTION_DRAW, OnActionDraw)
	ON_UPDATE_COMMAND_UI(ID_ACTION_DRAW, OnUpdateActionDraw)
	ON_COMMAND(ID_ACTION_ERASE, OnActionErase)
	ON_COMMAND(ID_ACTION_HIGHLIGHT, OnActionHighlight)
	ON_UPDATE_COMMAND_UI(ID_ACTION_HIGHLIGHT, OnUpdateActionHighlight)
	ON_COMMAND(ID_ACTION_NONE, OnActionNone)
	ON_UPDATE_COMMAND_UI(ID_ACTION_NONE, OnUpdateActionNone)
	ON_COMMAND(ID_ACTION_REDACT, OnActionRedact)
	ON_UPDATE_COMMAND_UI(ID_ACTION_REDACT, OnUpdateActionRedact)
	ON_COMMAND(ID_ACTION_ROTATECW, OnActionRotatecw)
	ON_COMMAND(ID_ACTION_ZOOM, OnActionZoom)
	ON_UPDATE_COMMAND_UI(ID_ACTION_ZOOM, OnUpdateActionZoom)
	ON_COMMAND(ID_ACTION_NEXT, OnActionNext)
	ON_COMMAND(ID_ACTION_PREV, OnActionPrev)
	ON_UPDATE_COMMAND_UI(ID_ACTION_NEXT, OnUpdateActionNext)
	ON_UPDATE_COMMAND_UI(ID_ACTION_PREV, OnUpdateActionPrev)
	ON_COMMAND(ID_ACTION_SELECT, OnActionSelect)
	ON_UPDATE_COMMAND_UI(ID_ACTION_SELECT, OnUpdateActionSelect)
	ON_COMMAND(ID_ACTION_RESETZOOM, OnActionResetzoom)
	ON_UPDATE_COMMAND_UI(ID_ACTION_RESETZOOM, OnUpdateActionResetzoom)
	ON_COMMAND(ID_ACTION_PLAY, OnActionPlay)
	ON_UPDATE_COMMAND_UI(ID_ACTION_PLAY, OnUpdateActionPlay)
	ON_COMMAND(ID_ACTION_CONTINUOUS, OnActionContinuous)
	ON_UPDATE_COMMAND_UI(ID_ACTION_CONTINUOUS, OnUpdateActionContinuous)
	ON_COMMAND(ID_ACTION_STOP, OnActionStop)
	ON_UPDATE_COMMAND_UI(ID_ACTION_STOP, OnUpdateActionStop)
	ON_COMMAND(ID_ACTION_LOADANN, OnActionLoadann)
	ON_COMMAND(ID_ACTION_REALIZE, OnActionRealize)
	ON_COMMAND(ID_ACTION_SAVEANN, OnActionSaveann)
	ON_COMMAND(ID_ACTION_ZOOMHEIGHT, OnActionZoomheight)
	ON_COMMAND(ID_ACTION_ZOOMWIDTH, OnActionZoomwidth)
	ON_COMMAND(ID_FILE_SAVEAS, OnFileSaveas)
	ON_COMMAND(ID_PANDOWN, OnPandown)
	ON_COMMAND(ID_PANLEFT, OnPanleft)
	ON_COMMAND(ID_PANRIGHT, OnPanright)
	ON_COMMAND(ID_PANUP, OnPanup)
	ON_UPDATE_COMMAND_UI(ID_PANUP, OnUpdatePanup)
	ON_UPDATE_COMMAND_UI(ID_PANRIGHT, OnUpdatePanright)
	ON_UPDATE_COMMAND_UI(ID_PANLEFT, OnUpdatePanleft)
	ON_UPDATE_COMMAND_UI(ID_PANDOWN, OnUpdatePandown)
	ON_COMMAND(ID_PROPERTIES_PANIMAGE, OnPropertiesPanimage)
	ON_UPDATE_COMMAND_UI(ID_PROPERTIES_PANIMAGE, OnUpdatePropertiesPanimage)
	ON_COMMAND(ID_CALLOUT, OnCallout)
	ON_COMMAND(ID_ACTION_LOADVIEW, OnActionLoadview)
	ON_COMMAND(ID_MOUSEPAN, OnMousepan)
	ON_UPDATE_COMMAND_UI(ID_MOUSEPAN, OnUpdateMousepan)
	ON_UPDATE_COMMAND_UI(ID_CALLOUT, OnUpdateCallout)
	ON_COMMAND(ID_ACTION_LOADSCALED, OnActionLoadscaled)
	ON_COMMAND(ID_ACTION_LOADVIEWCALL, OnActionLoadviewcall)
	ON_COMMAND(ID_ACTION_LOADSCALECALL, OnActionLoadscalecall)
	ON_COMMAND(ID_ACTION_PAN, OnActionPan)
	ON_UPDATE_COMMAND_UI(ID_ACTION_PAN, OnUpdateActionPan)
	ON_COMMAND(ID_ACTION_SETPRINTER, OnActionSetprinter)
	ON_COMMAND(ID_SPLITSCREEN, OnSplitscreen)
	ON_UPDATE_COMMAND_UI(ID_SPLITSCREEN, OnUpdateSplitscreen)
	ON_COMMAND(ID_LOADACTIVE, OnLoadactive)
	ON_COMMAND(ID_LOADLEFT, OnLoadleft)
	ON_COMMAND(ID_LOADRIGHT, OnLoadright)
	ON_UPDATE_COMMAND_UI(ID_LOADRIGHT, OnUpdateLoadright)
	ON_COMMAND(ID_SYNCPROPS, OnSyncprops)
	ON_UPDATE_COMMAND_UI(ID_SYNCPROPS, OnUpdateSyncprops)
	ON_COMMAND(ID_PRINT_FULLBOTH, OnPrintFullboth)
	ON_COMMAND(ID_PRINT_FULLLEFT, OnPrintFullleft)
	ON_UPDATE_COMMAND_UI(ID_PRINT_FULLLEFT, OnUpdatePrintFullleft)
	ON_COMMAND(ID_PRINT_FULLRIGHT, OnPrintFullright)
	ON_UPDATE_COMMAND_UI(ID_PRINT_FULLRIGHT, OnUpdatePrintFullright)
	ON_COMMAND(ID_PRINT_VISIBLEBOTH, OnPrintVisibleboth)
	ON_COMMAND(ID_PRINT_VISIBLELEFT, OnPrintVisibleleft)
	ON_UPDATE_COMMAND_UI(ID_PRINT_VISIBLELEFT, OnUpdatePrintVisibleleft)
	ON_COMMAND(ID_PRINT_VISIBLERIGHT, OnPrintVisibleright)
	ON_UPDATE_COMMAND_UI(ID_PRINT_VISIBLERIGHT, OnUpdatePrintVisibleright)
	ON_COMMAND(ID_PROPERTIES_SYNCCALLANN, OnPropertiesSynccallann)
	ON_UPDATE_COMMAND_UI(ID_PROPERTIES_SYNCCALLANN, OnUpdatePropertiesSynccallann)
	ON_COMMAND(ID_DELETELAST, OnDeletelast)
	ON_COMMAND(ID_DELETESELECTIONS, OnDeleteselections)
	ON_UPDATE_COMMAND_UI(ID_DELETESELECTIONS, OnUpdateDeleteselections)
	ON_COMMAND(ID_PROPERTIES_SELECTOR, OnPropertiesSelector)
	ON_UPDATE_COMMAND_UI(ID_PROPERTIES_SELECTOR, OnUpdatePropertiesSelector)
	ON_COMMAND(ID_PROPERTIES_KEEPASPECT, OnPropertiesKeepaspect)
	ON_UPDATE_COMMAND_UI(ID_PROPERTIES_KEEPASPECT, OnUpdatePropertiesKeepaspect)
	ON_COMMAND(ID_PROPERTIES_ZOOMTORECT, OnPropertiesZoomtorect)
	ON_UPDATE_COMMAND_UI(ID_PROPERTIES_ZOOMTORECT, OnUpdatePropertiesZoomtorect)
	ON_COMMAND(ID_COPYCLIPBOARD, OnCopyclipboard)
	ON_UPDATE_COMMAND_UI(ID_COPYCLIPBOARD, OnUpdateCopyclipboard)
	ON_COMMAND(ID_PASTECLIPBOARD, OnPasteclipboard)
	ON_UPDATE_COMMAND_UI(ID_PASTECLIPBOARD, OnUpdatePasteclipboard)
	ON_COMMAND(ID_IMAGE_INFORMATION, OnImageInformation)
	ON_UPDATE_COMMAND_UI(ID_IMAGE_INFORMATION, OnUpdateImageInformation)
	ON_COMMAND(ID_PRINTER_DEFAULT, OnPrinterDefault)
	ON_COMMAND(ID_PRINTER_CURRENT, OnPrinterCurrent)
	ON_COMMAND(ID_DESKEW, OnDeskew)
	ON_COMMAND(ID_DESKEW_BACK_COLOR, OnDeskewBackColor)
	ON_COMMAND(ID_VIEW_REGISTERED_PATH, OnViewRegisteredPath)
	ON_COMMAND(ID_VIEW_CLASS_ID, OnViewClassId)
	ON_COMMAND(ID_PROPERTIES_ANNOTATECALLOUTS, OnPropertiesAnnotatecallouts)
	ON_UPDATE_COMMAND_UI(ID_PROPERTIES_ANNOTATECALLOUTS, OnUpdatePropertiesAnnotatecallouts)
	ON_COMMAND(ID_DESPECKLE, OnDespeckle)
	ON_COMMAND(ID_DOT_REMOVE, OnDotRemove)
	ON_COMMAND(ID_HOLE_REMOVE, OnHoleRemove)
	ON_COMMAND(ID_SMOOTH, OnSmooth)
	ON_COMMAND(ID_BORDER_REMOVE, OnBorderRemove)
	ON_COMMAND(ID_CLEANUP, OnCleanup)
	ON_COMMAND(ID_PAGE_SETUP, OnPageSetup)
	ON_COMMAND(ID_SPLIT_HORIZONTAL, OnSplitHorizontal)
	ON_UPDATE_COMMAND_UI(ID_SPLIT_HORIZONTAL, OnUpdateSplitHorizontal)
	ON_COMMAND(ID_QFACTOR, OnQfactor)
	ON_COMMAND(ID_SHOW_DIAGNOSTICS, OnShowDiagnostics)
	ON_COMMAND(ID_HIDE_DIAGNOSTICS, OnHideDiagnostics)
	ON_COMMAND(ID_DRAW_RECTANGLE, OnDrawRectangle)
	ON_UPDATE_COMMAND_UI(ID_DRAW_RECTANGLE, OnUpdateDrawRectangle)
	ON_COMMAND(ID_SOURCE_RECTANGLE, OnSourceRectangle)
	ON_UPDATE_COMMAND_UI(ID_SOURCE_RECTANGLE, OnUpdateSourceRectangle)
	ON_COMMAND(ID_VIEW_EVENTS, OnViewEvents)
	ON_UPDATE_COMMAND_UI(ID_VIEW_EVENTS, OnUpdateViewEvents)
	ON_COMMAND(ID_FILE_SAVE, OnFileSave)
	ON_COMMAND(ID_PRINT_SET_PROPERTIES, OnPrintSetProperties)
	ON_COMMAND(ID_SAVE_PAGES, OnSavePages)
	ON_UPDATE_COMMAND_UI(ID_SAVE_PAGES, OnUpdateSavePages)
	ON_COMMAND(ID_PRINTER_CAPS, OnPrinterCaps)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTmviewvcView construction/destruction

CTmviewvcView::CTmviewvcView()
	: CFormView(CTmviewvcView::IDD)
{
	//{{AFX_DATA_INIT(CTmviewvcView)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// TODO: add construction code here
	m_bScaleImage = FALSE;
	m_bFitToImage = FALSE;
	m_bPanImage = TRUE;
	m_bMousePan = TRUE;
	m_pDiagnostics = 0;

}

CTmviewvcView::~CTmviewvcView()
{
	if(m_pDiagnostics != 0)
		delete m_pDiagnostics;
}

void CTmviewvcView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CTmviewvcView)
	DDX_Control(pDX, IDC_TMVIEWCTRL1, m_ctrlTMView);
	//}}AFX_DATA_MAP
}

BOOL CTmviewvcView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	return CFormView::PreCreateWindow(cs);
}

#ifdef _DEBUG
void CTmviewvcView::AssertValid() const
{
	CFormView::AssertValid();
}

void CTmviewvcView::Dump(CDumpContext& dc) const
{
	CFormView::Dump(dc);
}

CTmviewvcDoc* CTmviewvcView::GetDocument() 
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CTmviewvcDoc)));
	return (CTmviewvcDoc*)m_pDocument;
}
#endif 


void CTmviewvcView::OnInitialUpdate() 
{
	//	Perform base class processing
	CFormView::OnInitialUpdate();
	
	//	Make sure the control uses the whole client area 
	if(IsWindow(m_ctrlTMView.m_hWnd))
	{	
		RECT rcClient;
		GetClientRect(&rcClient);
		m_ctrlTMView.MoveWindow(&rcClient);

		m_bScaleImage = m_ctrlTMView.GetScaleImage();
		m_bFitToImage = m_ctrlTMView.GetFitToImage();
		m_ctrlTMView.SetAnnTool(ANNTEXT);
		m_ctrlTMView.SetAction(DRAW);
	
		m_ctrlTMView.ShowDiagnostics(TRUE);
	}

	//	Set the filename to display
	CTmviewvcDoc* pDoc = GetDocument();
	m_ctrlTMView.LoadFile(pDoc->GetPathName(), -1);

}

void CTmviewvcView::OnControlAbout() 
{
	//	Show the TMView control's About Box
	m_ctrlTMView.AboutBox();
	
}

void CTmviewvcView::OnViewProperties() 
{
	CString title;
	GetWindowText(title);
	MessageBox(title, "View Properties");
	
}

void CTmviewvcView::OnSize(UINT nType, int cx, int cy) 
{
	CFormView::OnSize(nType, cx, cy);
	
	//	Resize the control to match the new client area 
	if(IsWindow(m_ctrlTMView.m_hWnd))
	{	
		RECT rcClient;
		GetClientRect(&rcClient);
		m_ctrlTMView.MoveWindow(&rcClient);

		float fWidth = (float)(rcClient.right - rcClient.left);
		float fHeight = (float)(rcClient.bottom - rcClient.top);
		float fRatio = 0;

		if(fWidth != 0)
			fRatio = fHeight / fWidth;

		CString M;
		M.Format("W: %d  H: %d  R: %f", (int)fWidth, (int)fHeight, fRatio);
		GetDocument()->SetTitle(M);
		GetParent()->SetWindowText(M);
	}
	
}

void CTmviewvcView::OnDraw(CDC* pDC) 
{
	//	Redraw the TMView control
	m_ctrlTMView.RedrawWindow();
}

void CTmviewvcView::OnActionRotateccw() 
{
//m_ctrlTMView.SetRotation(-30);
//m_ctrlTMView.Rotate(TRUE, -1);
	m_ctrlTMView.RotateCcw(TRUE, TMV_ACTIVEPANE);
}

void CTmviewvcView::OnPropertiesAnnotations() 
{
	long lFlags = 0;
	m_ctrlTMView.SetAnnotationProperties(lFlags);
	
	
}

void CTmviewvcView::OnPropertiesFittoimage() 
{
	m_bFitToImage = !m_bFitToImage;
	m_ctrlTMView.SetFitToImage(m_bFitToImage);
}

void CTmviewvcView::OnUpdatePropertiesFittoimage(CCmdUI* pCmdUI) 
{
	if(m_ctrlTMView.GetFitToImage())
		pCmdUI->SetCheck(TRUE);
	else
		pCmdUI->SetCheck(FALSE);
}

void CTmviewvcView::OnPropertiesScaleimage() 
{
	m_bScaleImage = !m_bScaleImage;
	m_ctrlTMView.SetScaleImage(m_bScaleImage);
	m_ctrlTMView.Redraw();
}

void CTmviewvcView::OnUpdatePropertiesScaleimage(CCmdUI* pCmdUI) 
{
	if(m_ctrlTMView.GetScaleImage())
		pCmdUI->SetCheck(TRUE);
	else
		pCmdUI->SetCheck(FALSE);
}

void CTmviewvcView::OnActionDraw() 
{
	m_ctrlTMView.SetAction(DRAW);
}

void CTmviewvcView::OnUpdateActionDraw(CCmdUI* pCmdUI) 
{
	if(m_ctrlTMView.GetAction() == DRAW)
		pCmdUI->SetCheck(TRUE);
	else
		pCmdUI->SetCheck(FALSE);
}

void CTmviewvcView::OnActionErase() 
{
	m_ctrlTMView.Erase(TMV_ACTIVEPANE);
}

void CTmviewvcView::OnActionHighlight() 
{
	m_ctrlTMView.SetAction(HIGHLIGHT);	
}

void CTmviewvcView::OnUpdateActionHighlight(CCmdUI* pCmdUI) 
{
	if(m_ctrlTMView.GetAction() == HIGHLIGHT)
		pCmdUI->SetCheck(TRUE);
	else
		pCmdUI->SetCheck(FALSE);
}

void CTmviewvcView::OnActionNone() 
{
	m_ctrlTMView.SetAction(NONE);	
}

void CTmviewvcView::OnUpdateActionNone(CCmdUI* pCmdUI) 
{
	if(m_ctrlTMView.GetAction() == NONE)
		pCmdUI->SetCheck(TRUE);
	else
		pCmdUI->SetCheck(FALSE);
}

void CTmviewvcView::OnActionRedact() 
{
	m_ctrlTMView.SetAction(REDACT);	
}

void CTmviewvcView::OnUpdateActionRedact(CCmdUI* pCmdUI) 
{
	if(m_ctrlTMView.GetAction() == REDACT)
		pCmdUI->SetCheck(TRUE);
	else
		pCmdUI->SetCheck(FALSE);
}

void CTmviewvcView::OnActionRotatecw() 
{
//m_ctrlTMView.SetRotation(30);
//m_ctrlTMView.Rotate(TRUE, -1);
	m_ctrlTMView.RotateCw(TRUE, TMV_ACTIVEPANE);
}

void CTmviewvcView::OnActionZoom() 
{
	m_ctrlTMView.SetAction(ZOOM);
	
}

void CTmviewvcView::OnUpdateActionZoom(CCmdUI* pCmdUI) 
{
	if(m_ctrlTMView.GetAction() == ZOOM)
		pCmdUI->SetCheck(TRUE);
	else
		pCmdUI->SetCheck(FALSE);

	if(m_ctrlTMView.GetZoomFactor(TMV_ACTIVEPANE) < (float)m_ctrlTMView.GetMaxZoom())
		pCmdUI->Enable(TRUE);
	else
		pCmdUI->Enable(FALSE);

}

void CTmviewvcView::OnActionNext() 
{
	m_ctrlTMView.NextPage(TMV_ACTIVEPANE);
}

void CTmviewvcView::OnActionPrev() 
{
	m_ctrlTMView.PrevPage(TMV_ACTIVEPANE);
}

void CTmviewvcView::OnUpdateActionNext(CCmdUI* pCmdUI) 
{
	if(m_ctrlTMView.GetCurrentPage(TMV_ACTIVEPANE) < m_ctrlTMView.GetPageCount(TMV_ACTIVEPANE))
		pCmdUI->Enable(TRUE);
	else
		pCmdUI->Enable(FALSE);
}

void CTmviewvcView::OnUpdateActionPrev(CCmdUI* pCmdUI) 
{
	if(m_ctrlTMView.GetCurrentPage(TMV_ACTIVEPANE) > 1)
		pCmdUI->Enable(TRUE);
	else
		pCmdUI->Enable(FALSE);
}

void CTmviewvcView::OnActionSelect() 
{
	m_ctrlTMView.SetAction(SELECT);	
}

void CTmviewvcView::OnUpdateActionSelect(CCmdUI* pCmdUI) 
{
	if(m_ctrlTMView.GetAction() == SELECT)
		pCmdUI->SetCheck(TRUE);
	else
		pCmdUI->SetCheck(FALSE);
}

BEGIN_EVENTSINK_MAP(CTmviewvcView, CFormView)
    //{{AFX_EVENTSINK_MAP(CTmviewvcView)
	ON_EVENT(CTmviewvcView, IDC_TMVIEWCTRL1, 1 /* MouseClick */, OnMouseClickTmviewctrl1, VTS_I2 VTS_I2)
	ON_EVENT(CTmviewvcView, IDC_TMVIEWCTRL1, 2 /* MouseDblClick */, OnMouseDblClickTmviewctrl1, VTS_I2 VTS_I2)
	ON_EVENT(CTmviewvcView, IDC_TMVIEWCTRL1, 5 /* SelectPane */, OnSelectPaneTmviewctrl1, VTS_I2)
	ON_EVENT(CTmviewvcView, IDC_TMVIEWCTRL1, -606 /* MouseMove */, OnMouseMoveTmviewctrl1, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
	ON_EVENT(CTmviewvcView, IDC_TMVIEWCTRL1, 8 /* SelectCallout */, OnSelectCalloutTmviewctrl1, VTS_I4 VTS_I2)
	ON_EVENT(CTmviewvcView, IDC_TMVIEWCTRL1, 11 /* AnnotationDeleted */, OnAnnotationDeletedTmviewctrl1, VTS_I4 VTS_I2)
	ON_EVENT(CTmviewvcView, IDC_TMVIEWCTRL1, 12 /* AnnotationModified */, OnAnnotationModifiedTmviewctrl1, VTS_I4 VTS_I2)
	ON_EVENT(CTmviewvcView, IDC_TMVIEWCTRL1, 13 /* AnnotationDrawn */, OnAnnotationDrawnTmviewctrl1, VTS_I4 VTS_I2)
	ON_EVENT(CTmviewvcView, IDC_TMVIEWCTRL1, 14 /* CalloutResized */, OnCalloutResizedTmviewctrl1, VTS_I4 VTS_I2)
	ON_EVENT(CTmviewvcView, IDC_TMVIEWCTRL1, 15 /* CalloutMoved */, OnCalloutMovedTmviewctrl1, VTS_I4 VTS_I2)
	ON_EVENT(CTmviewvcView, IDC_TMVIEWCTRL1, -605 /* MouseDown */, OnMouseDownTmviewctrl1, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
	ON_EVENT(CTmviewvcView, IDC_TMVIEWCTRL1, -607 /* MouseUp */, OnMouseUpTmviewctrl1, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()

void CTmviewvcView::OnMouseClickTmviewctrl1(short Button, short Key) 
{
	CString Msg;

	//	Which button?
	if(Button == LEFT_MOUSEBUTTON)
		Msg = "Left Button Click\n";
	else
		Msg = "Right Button Click\n";

	//	Which Key?
	switch(Key)
	{
		case SHIFT:			Msg += "Shift Key";
							break;
		case CTRL:			Msg += "Control Key";
							break;
		case ALT:			Msg += "Alt Key";
							break;
		case CTRLSHIFT:		Msg += "Control/Shift Keys";
							break;
		case ALTSHIFT:		Msg += "Alt/Shift Keys";
							break;
		case CTRLALT:		Msg += "Control/Alt Keys";
							break;
		case CTRLALTSHIFT:	Msg += "Control/Alt/Shift Keys";
							break;
		default:			Msg += "No key pressed";
							break;
	}

	if((m_pDiagnostics != 0) && (m_pDiagnostics->IsWindowVisible() == TRUE))
	{
		m_pDiagnostics->Report("", Msg);
	}
}

void CTmviewvcView::OnMouseDblClickTmviewctrl1(short Button, short Key) 
{
	CString Msg;

	//	Which button?
	if(Button == LEFT_MOUSEBUTTON)
		Msg = "Left Button Double Click\n";
	else
		Msg = "Right Button Double Click\n";

	//	Which Key?
	switch(Key)
	{
		case SHIFT:			Msg += "Shift Key";
							break;
		case CTRL:			Msg += "Control Key";
							break;
		case ALT:			Msg += "Alt Key";
							break;
		case CTRLSHIFT:		Msg += "Control/Shift Keys";
							break;
		case ALTSHIFT:		Msg += "Alt/Shift Keys";
							break;
		case CTRLALT:		Msg += "Control/Alt Keys";
							break;
		case CTRLALTSHIFT:	Msg += "Control/Alt/Shift Keys";
							break;
		default:			Msg += "No key pressed";
							break;
	}


	if((m_pDiagnostics != 0) && (m_pDiagnostics->IsWindowVisible() == TRUE))
	{
		m_pDiagnostics->Report("", Msg);
	}
}

void CTmviewvcView::OnActionResetzoom() 
{
	m_ctrlTMView.ResetZoom(TMV_ACTIVEPANE);
	
}

void CTmviewvcView::OnUpdateActionResetzoom(CCmdUI* pCmdUI) 
{
	if(m_ctrlTMView.GetZoomState(TMV_ACTIVEPANE) != ZOOMED_NONE)
		pCmdUI->Enable(TRUE);
	else
		pCmdUI->Enable(FALSE);

}

void CTmviewvcView::OnActionPlay() 
{
	m_ctrlTMView.PlayAnimation(TRUE, FALSE, TMV_ACTIVEPANE);
	
}

void CTmviewvcView::OnUpdateActionPlay(CCmdUI* pCmdUI) 
{
	if(m_ctrlTMView.IsAnimation(TMV_ACTIVEPANE) && !m_ctrlTMView.IsPlaying(TMV_ACTIVEPANE))
		pCmdUI->Enable(TRUE);
	else
		pCmdUI->Enable(FALSE);
}

void CTmviewvcView::OnActionContinuous() 
{
	m_ctrlTMView.PlayAnimation(TRUE, TRUE, TMV_ACTIVEPANE);
}

void CTmviewvcView::OnUpdateActionContinuous(CCmdUI* pCmdUI) 
{
	if(m_ctrlTMView.IsAnimation(TMV_ACTIVEPANE) && !m_ctrlTMView.IsPlaying(TMV_ACTIVEPANE))
		pCmdUI->Enable(TRUE);
	else
		pCmdUI->Enable(FALSE);
}

void CTmviewvcView::OnActionStop() 
{
	m_ctrlTMView.PlayAnimation(FALSE, FALSE, TMV_ACTIVEPANE);
}

void CTmviewvcView::OnUpdateActionStop(CCmdUI* pCmdUI) 
{
	if(m_ctrlTMView.IsPlaying(TMV_ACTIVEPANE))
		pCmdUI->Enable(TRUE);
	else
		pCmdUI->Enable(FALSE);
}

void CTmviewvcView::OnActionLoadann() 
{
	CFileDialog	FileDlg(TRUE);
	char		szFilter[] = "Zaps (*.zap)\0*.zap\0All Files (*.*)\0*.*\0\0";
	char		szFilename[512];

	//	Initialize the filename buffer
	memset(szFilename, 0, sizeof(szFilename));
	FileDlg.m_ofn.lpstrFile = szFilename;
	FileDlg.m_ofn.nMaxFile = sizeof(szFilename);

	//	Set the dialog flags and other parameters
	FileDlg.m_ofn.hwndOwner = m_hWnd;
	FileDlg.m_ofn.lpstrFilter = szFilter;
	FileDlg.m_ofn.Flags |= OFN_FILEMUSTEXIST | OFN_LONGNAMES | OFN_NONETWORKBUTTON | OFN_SHOWHELP;
	if(FileDlg.DoModal() == IDOK)
	{
		m_ctrlTMView.LoadZap(szFilename, FALSE, FALSE, FALSE, -1, 0);

	}
	
}

void CTmviewvcView::OnActionRealize() 
{
	m_ctrlTMView.Realize(TMV_ACTIVEPANE);
}

void CTmviewvcView::OnActionSaveann() 
{
	CFileDialog	FileDlg(FALSE);
	char		szFilter[] = "Zaps (*.zap)\0*.zap\0All Files (*.*)\0*.*\0\0";
	char		szFilename[512];

	//	Initialize the filename buffer
	memset(szFilename, 0, sizeof(szFilename));
	FileDlg.m_ofn.lpstrFile = szFilename;
	FileDlg.m_ofn.nMaxFile = sizeof(szFilename);

	//	Set the dialog flags and other parameters
	FileDlg.m_ofn.hwndOwner = m_hWnd;
	FileDlg.m_ofn.lpstrFilter = szFilter;
	FileDlg.m_ofn.Flags |= OFN_LONGNAMES | OFN_NONETWORKBUTTON | OFN_SHOWHELP;
	if(FileDlg.DoModal() == IDOK)
	{
		m_ctrlTMView.SaveZap(szFilename, TMV_ACTIVEPANE);

	}
	
}

void CTmviewvcView::OnActionZoomheight() 
{
	m_ctrlTMView.ZoomFullHeight(TMV_ACTIVEPANE);
}

void CTmviewvcView::OnActionZoomwidth() 
{
	m_ctrlTMView.ZoomFullWidth(TMV_ACTIVEPANE);
}

void CTmviewvcView::OnFileSaveas() 
{
	CFileDialog	FileDlg(FALSE);
	char		szFilter[] = "All Files (*.*)\0*.*\0\0";
	char		szFilename[512];

	//	Initialize the filename buffer
	memset(szFilename, 0, sizeof(szFilename));
	FileDlg.m_ofn.lpstrFile = szFilename;
	FileDlg.m_ofn.nMaxFile = sizeof(szFilename);

	//	Set the dialog flags and other parameters
	FileDlg.m_ofn.hwndOwner = m_hWnd;
	FileDlg.m_ofn.lpstrFilter = szFilter;
	FileDlg.m_ofn.Flags |= OFN_LONGNAMES | OFN_NONETWORKBUTTON | OFN_SHOWHELP;
	if(FileDlg.DoModal() == IDOK)
	{
		m_ctrlTMView.Save(szFilename, TMV_ACTIVEPANE);

	}
	
}

void CTmviewvcView::OnPandown() 
{
	m_ctrlTMView.Pan(PAN_DOWN, TMV_ACTIVEPANE);
}

void CTmviewvcView::OnPanleft() 
{
	m_ctrlTMView.Pan(PAN_LEFT, TMV_ACTIVEPANE);
}

void CTmviewvcView::OnPanright() 
{
	m_ctrlTMView.Pan(PAN_RIGHT, TMV_ACTIVEPANE);
}

void CTmviewvcView::OnPanup() 
{
	m_ctrlTMView.Pan(PAN_UP, TMV_ACTIVEPANE);
}

void CTmviewvcView::OnUpdatePanup(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_ctrlTMView.GetPanStates(TMV_ACTIVEPANE) & ENABLE_PANUP);
}

void CTmviewvcView::OnUpdatePanright(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_ctrlTMView.GetPanStates(TMV_ACTIVEPANE) & ENABLE_PANRIGHT);
}

void CTmviewvcView::OnUpdatePanleft(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_ctrlTMView.GetPanStates(TMV_ACTIVEPANE) & ENABLE_PANLEFT);
}

void CTmviewvcView::OnUpdatePandown(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_ctrlTMView.GetPanStates(TMV_ACTIVEPANE) & ENABLE_PANDOWN);
}

void CTmviewvcView::OnPropertiesPanimage() 
{
	m_bPanImage = !m_bPanImage;
	m_ctrlTMView.SetHideScrollBars(m_bPanImage);
	m_ctrlTMView.Redraw();
}

void CTmviewvcView::OnUpdatePropertiesPanimage(CCmdUI* pCmdUI) 
{
	if(m_ctrlTMView.GetHideScrollBars())
		pCmdUI->SetCheck(TRUE);
	else
		pCmdUI->SetCheck(FALSE);
}

void CTmviewvcView::OnCallout() 
{
	m_ctrlTMView.SetAction(CALLOUT);
}

void CTmviewvcView::OnActionLoadview() 
{
	CFileDialog	FileDlg(TRUE);
	char		szFilter[] = "Zaps (*.zap)\0*.zap\0All Files (*.*)\0*.*\0\0";
	char		szFilename[512];

	//	Initialize the filename buffer
	memset(szFilename, 0, sizeof(szFilename));
	FileDlg.m_ofn.lpstrFile = szFilename;
	FileDlg.m_ofn.nMaxFile = sizeof(szFilename);

	//	Set the dialog flags and other parameters
	FileDlg.m_ofn.hwndOwner = m_hWnd;
	FileDlg.m_ofn.lpstrFilter = szFilter;
	FileDlg.m_ofn.Flags |= OFN_FILEMUSTEXIST | OFN_LONGNAMES | OFN_NONETWORKBUTTON | OFN_SHOWHELP;
	if(FileDlg.DoModal() == IDOK)
	{
		m_ctrlTMView.LoadZap(szFilename, TRUE, FALSE, FALSE, -1, 0);

	}
	
}

void CTmviewvcView::OnActionLoadscaled() 
{
	CFileDialog	FileDlg(TRUE);
	char		szFilter[] = "Zaps (*.zap)\0*.zap\0All Files (*.*)\0*.*\0\0";
	char		szFilename[512];

	//	Initialize the filename buffer
	memset(szFilename, 0, sizeof(szFilename));
	FileDlg.m_ofn.lpstrFile = szFilename;
	FileDlg.m_ofn.nMaxFile = sizeof(szFilename);

	//	Set the dialog flags and other parameters
	FileDlg.m_ofn.hwndOwner = m_hWnd;
	FileDlg.m_ofn.lpstrFilter = szFilter;
	FileDlg.m_ofn.Flags |= OFN_FILEMUSTEXIST | OFN_LONGNAMES | OFN_NONETWORKBUTTON | OFN_SHOWHELP;
	if(FileDlg.DoModal() == IDOK)
	{
		m_ctrlTMView.LoadZap(szFilename, TRUE, TRUE, FALSE, -1, 0);

	}
	
}

void CTmviewvcView::OnMousepan() 
{
	m_bMousePan = !m_bMousePan;
	m_ctrlTMView.SetRightClickPan(m_bMousePan);
	m_ctrlTMView.Redraw();
}

void CTmviewvcView::OnUpdateMousepan(CCmdUI* pCmdUI) 
{
	if(m_ctrlTMView.GetRightClickPan())
		pCmdUI->SetCheck(TRUE);
	else
		pCmdUI->SetCheck(FALSE);
}

void CTmviewvcView::OnUpdateCallout(CCmdUI* pCmdUI) 
{
	if(m_ctrlTMView.GetAction() == CALLOUT)
		pCmdUI->SetCheck(TRUE);
	else
		pCmdUI->SetCheck(FALSE);
}


void CTmviewvcView::OnActionLoadviewcall() 
{
	CFileDialog	FileDlg(TRUE);
	char		szFilter[] = "Zaps (*.zap)\0*.zap\0All Files (*.*)\0*.*\0\0";
	char		szFilename[512];

	//	Initialize the filename buffer
	memset(szFilename, 0, sizeof(szFilename));
	FileDlg.m_ofn.lpstrFile = szFilename;
	FileDlg.m_ofn.nMaxFile = sizeof(szFilename);

	//	Set the dialog flags and other parameters
	FileDlg.m_ofn.hwndOwner = m_hWnd;
	FileDlg.m_ofn.lpstrFilter = szFilter;
	FileDlg.m_ofn.Flags |= OFN_FILEMUSTEXIST | OFN_LONGNAMES | OFN_NONETWORKBUTTON | OFN_SHOWHELP;
	if(FileDlg.DoModal() == IDOK)
	{
		m_ctrlTMView.LoadZap(szFilename, TRUE, FALSE, TRUE, -1, 0);

	}
	
}

void CTmviewvcView::OnActionLoadscalecall() 
{
	CFileDialog	FileDlg(TRUE);
	char		szFilter[] = "Zaps (*.zap)\0*.zap\0All Files (*.*)\0*.*\0\0";
	char		szFilename[512];

	//	Initialize the filename buffer
	memset(szFilename, 0, sizeof(szFilename));
	FileDlg.m_ofn.lpstrFile = szFilename;
	FileDlg.m_ofn.nMaxFile = sizeof(szFilename);

	//	Set the dialog flags and other parameters
	FileDlg.m_ofn.hwndOwner = m_hWnd;
	FileDlg.m_ofn.lpstrFilter = szFilter;
	FileDlg.m_ofn.Flags |= OFN_FILEMUSTEXIST | OFN_LONGNAMES | OFN_NONETWORKBUTTON | OFN_SHOWHELP;
	if(FileDlg.DoModal() == IDOK)
	{
		m_ctrlTMView.LoadZap(szFilename, TRUE, TRUE, TRUE, -1, "");

	}
	
}

void CTmviewvcView::OnActionPan() 
{
	m_ctrlTMView.SetAction(PAN);
}

void CTmviewvcView::OnUpdateActionPan(CCmdUI* pCmdUI) 
{
	if(m_ctrlTMView.GetAction() == PAN)
		pCmdUI->SetCheck(TRUE);
	else
		pCmdUI->SetCheck(FALSE);
}

void CTmviewvcView::OnActionSetprinter() 
{
	m_ctrlTMView.SetPrinter();
}

void CTmviewvcView::OnSplitscreen() 
{
	m_ctrlTMView.SetSplitScreen(!m_ctrlTMView.GetSplitScreen());
}

void CTmviewvcView::OnUpdateSplitscreen(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_ctrlTMView.GetSplitScreen());
}


void CTmviewvcView::OnLoadactive() 
{
	CFileDialog	FileDlg(TRUE);
	char		szFilter[] = "All Files (*.*)\0*.*\0\0";
	char		szFilename[512];

	//	Initialize the filename buffer
	memset(szFilename, 0, sizeof(szFilename));
	FileDlg.m_ofn.lpstrFile = szFilename;
	FileDlg.m_ofn.nMaxFile = sizeof(szFilename);

	//	Set the dialog flags and other parameters
	FileDlg.m_ofn.hwndOwner = m_hWnd;
	FileDlg.m_ofn.lpstrFilter = szFilter;
	FileDlg.m_ofn.Flags |= OFN_FILEMUSTEXIST | OFN_LONGNAMES | OFN_NONETWORKBUTTON | OFN_SHOWHELP;
	if(FileDlg.DoModal() == IDOK)
	{
		m_ctrlTMView.LoadFile(szFilename, -1);

	}
}

void CTmviewvcView::OnLoadleft() 
{
	CFileDialog	FileDlg(TRUE);
	char		szFilter[] = "All Files (*.*)\0*.*\0\0";
	char		szFilename[512];

	//	Initialize the filename buffer
	memset(szFilename, 0, sizeof(szFilename));
	FileDlg.m_ofn.lpstrFile = szFilename;
	FileDlg.m_ofn.nMaxFile = sizeof(szFilename);

	//	Set the dialog flags and other parameters
	FileDlg.m_ofn.hwndOwner = m_hWnd;
	FileDlg.m_ofn.lpstrFilter = szFilter;
	FileDlg.m_ofn.Flags |= OFN_FILEMUSTEXIST | OFN_LONGNAMES | OFN_NONETWORKBUTTON | OFN_SHOWHELP;
	if(FileDlg.DoModal() == IDOK)
	{
		m_ctrlTMView.SetLeftFile(szFilename);

	}
}

void CTmviewvcView::OnLoadright() 
{
	CFileDialog	FileDlg(TRUE);
	char		szFilter[] = "All Files (*.*)\0*.*\0\0";
	char		szFilename[512];

	//	Initialize the filename buffer
	memset(szFilename, 0, sizeof(szFilename));
	FileDlg.m_ofn.lpstrFile = szFilename;
	FileDlg.m_ofn.nMaxFile = sizeof(szFilename);

	//	Set the dialog flags and other parameters
	FileDlg.m_ofn.hwndOwner = m_hWnd;
	FileDlg.m_ofn.lpstrFilter = szFilter;
	FileDlg.m_ofn.Flags |= OFN_FILEMUSTEXIST | OFN_LONGNAMES | OFN_NONETWORKBUTTON | OFN_SHOWHELP;
	if(FileDlg.DoModal() == IDOK)
	{
		m_ctrlTMView.SetRightFile(szFilename);

	}
}

void CTmviewvcView::OnUpdateLoadright(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_ctrlTMView.GetSplitScreen());
}

void CTmviewvcView::OnSyncprops() 
{
	m_ctrlTMView.SetSyncPanes(!m_ctrlTMView.GetSyncPanes());
}

void CTmviewvcView::OnUpdateSyncprops(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_ctrlTMView.GetSyncPanes());
}

void CTmviewvcView::OnPrintFullboth() 
{
	m_ctrlTMView.Print(TRUE, -1);
}

void CTmviewvcView::OnPrintFullleft() 
{
	m_ctrlTMView.Print(TRUE, TMV_LEFTPANE);
}

void CTmviewvcView::OnUpdatePrintFullleft(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(lstrlen(m_ctrlTMView.GetRightFile()) != 0);
}

void CTmviewvcView::OnPrintFullright() 
{
	m_ctrlTMView.Print(TRUE, TMV_RIGHTPANE);
}

void CTmviewvcView::OnUpdatePrintFullright(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(lstrlen(m_ctrlTMView.GetRightFile()) != 0 && m_ctrlTMView.GetSplitScreen());
}

void CTmviewvcView::OnPrintVisibleboth() 
{
	m_ctrlTMView.Print(FALSE, -1);
}

void CTmviewvcView::OnPrintVisibleleft() 
{
	m_ctrlTMView.Print(FALSE, TMV_LEFTPANE);
}

void CTmviewvcView::OnUpdatePrintVisibleleft(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(lstrlen(m_ctrlTMView.GetLeftFile()) != 0);
}

void CTmviewvcView::OnPrintVisibleright() 
{
	m_ctrlTMView.Print(FALSE, TMV_RIGHTPANE);
}

void CTmviewvcView::OnUpdatePrintVisibleright(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(lstrlen(m_ctrlTMView.GetRightFile()) != 0 && m_ctrlTMView.GetSplitScreen());
}


void CTmviewvcView::OnSelectPaneTmviewctrl1(short sPane) 
{
	if(sPane == TMV_LEFTPANE)
		GetParent()->SetWindowText("Left Pane Active");
	else
		GetParent()->SetWindowText("Right Pane Active");
}

void CTmviewvcView::OnPropertiesSynccallann() 
{
	m_ctrlTMView.SetSyncCalloutAnn(!m_ctrlTMView.GetSyncCalloutAnn());
}

void CTmviewvcView::OnUpdatePropertiesSynccallann(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_ctrlTMView.GetSyncCalloutAnn());
}

void CTmviewvcView::OnDeletelast() 
{
	m_ctrlTMView.DeleteLastAnn(-1);
	
}

void CTmviewvcView::OnDeleteselections() 
{
	m_ctrlTMView.DeleteSelections(-1);
	
}

void CTmviewvcView::OnUpdateDeleteselections(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_ctrlTMView.GetSelectCount(-1) > 0);
}

void CTmviewvcView::OnPropertiesSelector() 
{
	m_ctrlTMView.SetPenSelectorVisible(!m_ctrlTMView.GetPenSelectorVisible());	
}

void CTmviewvcView::OnUpdatePropertiesSelector(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_ctrlTMView.GetPenSelectorVisible());
}

void CTmviewvcView::OnPropertiesKeepaspect() 
{
	m_ctrlTMView.SetKeepAspect(!m_ctrlTMView.GetKeepAspect());
	
}

void CTmviewvcView::OnUpdatePropertiesKeepaspect(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_ctrlTMView.GetKeepAspect());
}

void CTmviewvcView::OnPropertiesZoomtorect() 
{
	m_ctrlTMView.SetZoomToRect(!m_ctrlTMView.GetZoomToRect());
}

void CTmviewvcView::OnUpdatePropertiesZoomtorect(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_ctrlTMView.GetZoomToRect());
}

void CTmviewvcView::OnCopyclipboard() 
{
	m_ctrlTMView.Copy(-1);
}

void CTmviewvcView::OnUpdateCopyclipboard(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_ctrlTMView.IsLoaded(-1));
}

void CTmviewvcView::OnPasteclipboard() 
{
	m_ctrlTMView.Paste(-1);
}

void CTmviewvcView::OnUpdatePasteclipboard(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_ctrlTMView.IsLoaded(-1));
}

void CTmviewvcView::OnMouseMoveTmviewctrl1(short Button, short Shift, long x, long y) 
{
	CString Msg;

	//	Which button?
	if(Button == LEFT_MOUSEBUTTON)
		Msg.Format("Left Move (%d,%d)\n", x, y);
	else
		Msg.Format("Right Move (%d,%d)\n", x, y);

	//	Which Key?
	switch(Shift)
	{
		case SHIFT:			Msg += "Shift Key";
							break;
		case CTRL:			Msg += "Control Key";
							break;
		case ALT:			Msg += "Alt Key";
							break;
		case CTRLSHIFT:		Msg += "Control/Shift Keys";
							break;
		case ALTSHIFT:		Msg += "Alt/Shift Keys";
							break;
		case CTRLALT:		Msg += "Control/Alt Keys";
							break;
		case CTRLALTSHIFT:	Msg += "Control/Alt/Shift Keys";
							break;
		default:			Msg += "No key pressed";
							break;
	}


	if((m_pDiagnostics != 0) && (m_pDiagnostics->IsWindowVisible() == TRUE))
	{
		m_pDiagnostics->Report("", Msg);
	}
}

void CTmviewvcView::OnImageInformation() 
{
	m_ctrlTMView.ViewImageProperties(-1);
}

void CTmviewvcView::OnUpdateImageInformation(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_ctrlTMView.IsLoaded(-1));
}

void CTmviewvcView::OnPrinterDefault() 
{
	MessageBox(m_ctrlTMView.GetDefaultPrinter());	
}


void CTmviewvcView::OnPrinterCurrent() 
{
	MessageBox(m_ctrlTMView.GetCurrentPrinter());	
}

void CTmviewvcView::OnDeskew() 
{
	m_ctrlTMView.Deskew(-1);
}

void CTmviewvcView::OnDeskewBackColor() 
{
	CColorDialog Dlg;

	if(Dlg.DoModal() == IDOK)
	{
		m_ctrlTMView.SetDeskewBackColor((OLE_COLOR)Dlg.GetColor());
	}
	
}

void CTmviewvcView::OnViewRegisteredPath() 
{
	CString Path = m_ctrlTMView.GetRegisteredPath();
	MessageBox(Path);
}

void CTmviewvcView::OnViewClassId() 
{
	CString ClassId = m_ctrlTMView.GetClassIdString();
	MessageBox(ClassId);
}

void CTmviewvcView::OnPropertiesAnnotatecallouts() 
{
	m_ctrlTMView.SetAnnotateCallouts(!m_ctrlTMView.GetAnnotateCallouts());	
}

void CTmviewvcView::OnUpdatePropertiesAnnotatecallouts(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_ctrlTMView.GetAnnotateCallouts());	
}

void CTmviewvcView::OnDespeckle() 
{
	m_ctrlTMView.Despeckle(-1);
}

void CTmviewvcView::OnDotRemove() 
{
	m_ctrlTMView.DotRemove(-1, 10, 10, 100, 100);
}

void CTmviewvcView::OnHoleRemove() 
{
	m_ctrlTMView.HolePunchRemove(-1, 100, 100, 1000, 1000, 1);
}

void CTmviewvcView::OnSmooth() 
{
	m_ctrlTMView.Smooth(-1, 10, 1);	
}

void CTmviewvcView::OnBorderRemove() 
{
	m_ctrlTMView.BorderRemove(-1, 20, 10, 10, 0);	
}

void CTmviewvcView::OnCleanup() 
{
	m_ctrlTMView.Cleanup(-1, 0);	
}

void CTmviewvcView::OnPageSetup() 
{
	m_ctrlTMView.SetupPrintPage();	
}

void CTmviewvcView::OnSplitHorizontal() 
{
	m_ctrlTMView.SetSplitHorizontal(!m_ctrlTMView.GetSplitHorizontal());	
}

void CTmviewvcView::OnUpdateSplitHorizontal(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_ctrlTMView.GetSplitHorizontal());
}

void CTmviewvcView::OnQfactor() 
{
	CPrompt Prompt;
	Prompt.m_strTitle = "QFactor Value:";
	Prompt.m_strValue.Format("%d", m_ctrlTMView.GetQFactor());

	if(Prompt.DoModal() == IDOK)
	{
		m_ctrlTMView.SetQFactor(atoi(Prompt.m_strValue));
	}
	
}

void CTmviewvcView::OnSelectCalloutTmviewctrl1(long hCallout, short sPane) 
{
	
}

void CTmviewvcView::OnShowDiagnostics() 
{
	m_ctrlTMView.ShowDiagnostics(TRUE);
}

void CTmviewvcView::OnHideDiagnostics() 
{
	m_ctrlTMView.ShowDiagnostics(FALSE);
}


void CTmviewvcView::OnDrawRectangle() 
{
	CRectangle dlg;

	if(dlg.DoModal() == IDOK)
	{
		m_ctrlTMView.DrawRectangle(dlg.m_iLeft,
								   dlg.m_iTop,
								   dlg.m_iRight,
								   dlg.m_iBottom,
								   (OLE_COLOR)dlg.m_crColor,
								   dlg.m_iTransparency,
								   dlg.m_bLocked,
								   -1);
	}
	
}

void CTmviewvcView::OnUpdateDrawRectangle(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_ctrlTMView.IsLoaded(-1));
}

void CTmviewvcView::OnSourceRectangle() 
{
	CRectangle dlg;

	if(dlg.DoModal() == IDOK)
	{
		m_ctrlTMView.DrawSourceRectangle(dlg.m_iLeft,
								   dlg.m_iTop,
								   dlg.m_iRight,
								   dlg.m_iBottom,
								   (OLE_COLOR)dlg.m_crColor,
								   dlg.m_iTransparency,
								   dlg.m_bLocked,
								   -1);

		/*
		m_ctrlTMView.DrawSourceText("annotation",
								   dlg.m_iLeft,
								   dlg.m_iTop,
								   dlg.m_iRight,
								   dlg.m_iBottom,
								   (OLE_COLOR)dlg.m_crColor,
								   "Arial",
								   120,
								   dlg.m_bLocked,
								   -1);
		*/

	}
	
}

void CTmviewvcView::OnUpdateSourceRectangle(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_ctrlTMView.IsLoaded(-1));
}

void CTmviewvcView::OnAnnotationDeletedTmviewctrl1(long lAnnotation, short sPane) 
{
	if((m_pDiagnostics != 0) && (m_pDiagnostics->IsWindowVisible() == TRUE))
	{
		m_pDiagnostics->Report("", "AnnotationDeleted");
	}
}

void CTmviewvcView::OnAnnotationModifiedTmviewctrl1(long lAnnotation, short sPane) 
{
	if((m_pDiagnostics != 0) && (m_pDiagnostics->IsWindowVisible() == TRUE))
	{
		m_pDiagnostics->Report("", "AnnotationModified");
	}
}

void CTmviewvcView::OnAnnotationDrawnTmviewctrl1(long lAnnotation, short sPane) 
{
	if((m_pDiagnostics != 0) && (m_pDiagnostics->IsWindowVisible() == TRUE))
	{
		m_pDiagnostics->Report("", "AnnotationDrawn");
	}
}

void CTmviewvcView::OnCalloutResizedTmviewctrl1(long hCallout, short sPane) 
{
	if((m_pDiagnostics != 0) && (m_pDiagnostics->IsWindowVisible() == TRUE))
	{
		m_pDiagnostics->Report("", "CalloutResized");
	}
}

void CTmviewvcView::OnCalloutMovedTmviewctrl1(long hCallout, short sPane) 
{
	if((m_pDiagnostics != 0) && (m_pDiagnostics->IsWindowVisible() == TRUE))
	{
		m_pDiagnostics->Report("", "CalloutMoved");
	}
}

void CTmviewvcView::OnViewEvents() 
{
	//	Are we supposed to be hiding the window?
	if((m_pDiagnostics != 0) && (m_pDiagnostics->IsWindowVisible() == TRUE))
	{
		m_pDiagnostics->ShowWindow(SW_HIDE);
	}
	else
	{
		if(m_pDiagnostics == 0)
		{
			m_pDiagnostics = new CDiagnostics();
			m_pDiagnostics->SetLogEnabled(FALSE);
			m_pDiagnostics->Create(this);
		}
		else
		{
			m_pDiagnostics->ShowWindow(SW_SHOW);
		}

	}
	
}

void CTmviewvcView::OnUpdateViewEvents(CCmdUI* pCmdUI) 
{
	if((m_pDiagnostics != 0) && (m_pDiagnostics->IsWindowVisible() == TRUE))
	{
		pCmdUI->SetCheck(1);
	}
	else
	{
		pCmdUI->SetCheck(0);
	}
	
}

void CTmviewvcView::OnMouseDownTmviewctrl1(short Button, short Shift, long x, long y) 
{
	CString Msg;

	//	Which button?
	if(Button == LEFT_MOUSEBUTTON)
		Msg.Format("Left Down (%d,%d)\n", x, y);
	else
		Msg.Format("Right Down (%d,%d)\n", x, y);

	//	Which Key?
	switch(Shift)
	{
		case SHIFT:			Msg += "Shift Key";
							break;
		case CTRL:			Msg += "Control Key";
							break;
		case ALT:			Msg += "Alt Key";
							break;
		case CTRLSHIFT:		Msg += "Control/Shift Keys";
							break;
		case ALTSHIFT:		Msg += "Alt/Shift Keys";
							break;
		case CTRLALT:		Msg += "Control/Alt Keys";
							break;
		case CTRLALTSHIFT:	Msg += "Control/Alt/Shift Keys";
							break;
		default:			Msg += "No key pressed";
							break;
	}


	if((m_pDiagnostics != 0) && (m_pDiagnostics->IsWindowVisible() == TRUE))
	{
		m_pDiagnostics->Report("", Msg);
	}
}

void CTmviewvcView::OnMouseUpTmviewctrl1(short Button, short Shift, long x, long y) 
{
	CString Msg;

	//	Which button?
	if(Button == LEFT_MOUSEBUTTON)
		Msg.Format("Left Up (%d,%d)\n", x, y);
	else
		Msg.Format("Right Up (%d,%d)\n", x, y);

	//	Which Key?
	switch(Shift)
	{
		case SHIFT:			Msg += "Shift Key";
							break;
		case CTRL:			Msg += "Control Key";
							break;
		case ALT:			Msg += "Alt Key";
							break;
		case CTRLSHIFT:		Msg += "Control/Shift Keys";
							break;
		case ALTSHIFT:		Msg += "Alt/Shift Keys";
							break;
		case CTRLALT:		Msg += "Control/Alt Keys";
							break;
		case CTRLALTSHIFT:	Msg += "Control/Alt/Shift Keys";
							break;
		default:			Msg += "No key pressed";
							break;
	}


	if((m_pDiagnostics != 0) && (m_pDiagnostics->IsWindowVisible() == TRUE))
	{
		m_pDiagnostics->Report("", Msg);
	}
}

void CTmviewvcView::OnFileSave() 
{
	m_ctrlTMView.Save(m_ctrlTMView.GetLeftFile(), TMV_ACTIVEPANE);
}

void CTmviewvcView::OnPrintSetProperties() 
{
	m_ctrlTMView.SetPrinterProperties((long)m_hWnd);
}

void CTmviewvcView::OnSavePages() 
{
	CFileDialog	FileDlg(TRUE);
	CSavePages	SavePages;
	char		szFilter[] = "TIFF (*.tif;tiff)\0*.tif;*tiff\0All Files (*.*)\0*.*\0\0";
	char		szFilename[512];

	//	Initialize the filename buffer
	memset(szFilename, 0, sizeof(szFilename));
	FileDlg.m_ofn.lpstrFile = szFilename;
	FileDlg.m_ofn.nMaxFile = sizeof(szFilename);

	//	Set the dialog flags and other parameters
	FileDlg.m_ofn.hwndOwner = m_hWnd;
	FileDlg.m_ofn.lpstrFilter = szFilter;
	FileDlg.m_ofn.Flags |= OFN_FILEMUSTEXIST | OFN_LONGNAMES | OFN_NONETWORKBUTTON | OFN_SHOWHELP;
	if(FileDlg.DoModal() == IDOK)
	{
		if(SavePages.DoModal() == IDOK)
		{
			m_ctrlTMView.SavePages(szFilename, SavePages.m_strFolder, SavePages.m_strPrefix);
		}
	}
	
}

void CTmviewvcView::OnUpdateSavePages(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(TRUE);	
}

void CTmviewvcView::OnPrinterCaps() 
{
	m_ctrlTMView.ShowPrinterCaps();
}
