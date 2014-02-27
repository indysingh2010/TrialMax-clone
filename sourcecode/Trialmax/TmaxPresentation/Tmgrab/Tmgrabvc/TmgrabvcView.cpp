// TmgrabvcView.cpp : implementation of the CTmgrabvcView class
//

#include "stdafx.h"
#include "Tmgrabvc.h"

#include "TmgrabvcDoc.h"
#include "TmgrabvcView.h"
#include "grabdefs.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CTmgrabvcView

IMPLEMENT_DYNCREATE(CTmgrabvcView, CFormView)

BEGIN_MESSAGE_MAP(CTmgrabvcView, CFormView)
	//{{AFX_MSG_MAP(CTmgrabvcView)
	ON_COMMAND(ID_CAPTURE, OnCapture)
	ON_COMMAND(ID_CAPTURE_ACTIVE, OnCaptureActive)
	ON_UPDATE_COMMAND_UI(ID_CAPTURE_ACTIVE, OnUpdateCaptureActive)
	ON_COMMAND(ID_CAPTURE_FULLSCREEN, OnCaptureFullscreen)
	ON_UPDATE_COMMAND_UI(ID_CAPTURE_FULLSCREEN, OnUpdateCaptureFullscreen)
	ON_COMMAND(ID_CAPTURE_SELECTION, OnCaptureSelection)
	ON_UPDATE_COMMAND_UI(ID_CAPTURE_SELECTION, OnUpdateCaptureSelection)
	ON_COMMAND(ID_HOTKEY, OnHotkey)
	ON_UPDATE_COMMAND_UI(ID_HOTKEY, OnUpdateHotkey)
	ON_COMMAND(ID_SILENT, OnSilent)
	ON_UPDATE_COMMAND_UI(ID_SILENT, OnUpdateSilent)
	ON_COMMAND(ID_STOP, OnStop)
	//}}AFX_MSG_MAP
	// Standard printing commands
	ON_COMMAND(ID_FILE_PRINT, CFormView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_DIRECT, CFormView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_PREVIEW, CFormView::OnFilePrintPreview)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTmgrabvcView construction/destruction

CTmgrabvcView::CTmgrabvcView()
	: CFormView(CTmgrabvcView::IDD)
{
	//{{AFX_DATA_INIT(CTmgrabvcView)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// TODO: add construction code here

}

CTmgrabvcView::~CTmgrabvcView()
{
}

void CTmgrabvcView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CTmgrabvcView)
	DDX_Control(pDX, IDC_TMGRABCTRL1, m_TMGrab);
	//}}AFX_DATA_MAP
}

BOOL CTmgrabvcView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	return CFormView::PreCreateWindow(cs);
}

void CTmgrabvcView::OnInitialUpdate()
{
	CFormView::OnInitialUpdate();
	GetParentFrame()->RecalcLayout();
	ResizeParentToFit();

	m_TMGrab.Initialize();

}

/////////////////////////////////////////////////////////////////////////////
// CTmgrabvcView printing

BOOL CTmgrabvcView::OnPreparePrinting(CPrintInfo* pInfo)
{
	// default preparation
	return DoPreparePrinting(pInfo);
}

void CTmgrabvcView::OnBeginPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: add extra initialization before printing
}

void CTmgrabvcView::OnEndPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: add cleanup after printing
}

void CTmgrabvcView::OnPrint(CDC* pDC, CPrintInfo* /*pInfo*/)
{
	// TODO: add customized printing code here
}

/////////////////////////////////////////////////////////////////////////////
// CTmgrabvcView diagnostics

#ifdef _DEBUG
void CTmgrabvcView::AssertValid() const
{
	CFormView::AssertValid();
}

void CTmgrabvcView::Dump(CDumpContext& dc) const
{
	CFormView::Dump(dc);
}

CTmgrabvcDoc* CTmgrabvcView::GetDocument() // non-debug version is inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CTmgrabvcDoc)));
	return (CTmgrabvcDoc*)m_pDocument;
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CTmgrabvcView message handlers

void CTmgrabvcView::OnCapture() 
{
	m_TMGrab.Capture();	
}

void CTmgrabvcView::OnCaptureActive() 
{
	m_TMGrab.SetArea(TMGRAB_AREA_ACTIVE_WINDOW);	
}

void CTmgrabvcView::OnUpdateCaptureActive(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMGrab.GetArea() == TMGRAB_AREA_ACTIVE_WINDOW);	
}

void CTmgrabvcView::OnCaptureFullscreen() 
{
	m_TMGrab.SetArea(TMGRAB_AREA_FULL_SCREEN);	
}

void CTmgrabvcView::OnUpdateCaptureFullscreen(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMGrab.GetArea() == TMGRAB_AREA_FULL_SCREEN);	
}

void CTmgrabvcView::OnCaptureSelection() 
{
	m_TMGrab.SetArea(TMGRAB_AREA_SELECTION);	
}

void CTmgrabvcView::OnUpdateCaptureSelection(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMGrab.GetArea() == TMGRAB_AREA_SELECTION);	
}

void CTmgrabvcView::OnHotkey() 
{
	if(m_TMGrab.GetHotkey() > 0)
		m_TMGrab.SetHotkey(0);
	else
		m_TMGrab.SetHotkey(VK_F9);	
}

void CTmgrabvcView::OnUpdateHotkey(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMGrab.GetHotkey() > 0);	
}

void CTmgrabvcView::OnSilent() 
{
	m_TMGrab.SetSilent(!m_TMGrab.GetSilent());	
}

void CTmgrabvcView::OnUpdateSilent(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMGrab.GetSilent());	
}

void CTmgrabvcView::OnStop() 
{
	m_TMGrab.Stop();
	
}

BEGIN_EVENTSINK_MAP(CTmgrabvcView, CFormView)
    //{{AFX_EVENTSINK_MAP(CTmgrabvcView)
	ON_EVENT(CTmgrabvcView, IDC_TMGRABCTRL1, 1 /* Captured */, OnCapturedTmgrabctrl1, VTS_NONE)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()

void CTmgrabvcView::OnCapturedTmgrabctrl1() 
{
	MessageBeep(0);
	
}
