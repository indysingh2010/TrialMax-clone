// MainFrm.cpp : implementation of the CMainFrame class
//

#include "stdafx.h"
#include "Tmtextvc.h"

#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

extern CMainFrame* pMainFrame;

/////////////////////////////////////////////////////////////////////////////
// CMainFrame

IMPLEMENT_DYNAMIC(CMainFrame, CMDIFrameWnd)

BEGIN_MESSAGE_MAP(CMainFrame, CMDIFrameWnd)
	//{{AFX_MSG_MAP(CMainFrame)
	ON_WM_CREATE()
	ON_COMMAND(ID_METHOD_BARLINE, OnMethodBarline)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

static UINT indicators[] =
{
	ID_SEPARATOR,           // status line indicator
	ID_INDICATOR_CAPS,
	ID_INDICATOR_NUM,
	ID_INDICATOR_SCRL,
};

CMainFrame* pMainFrame;

/////////////////////////////////////////////////////////////////////////////
// CMainFrame construction/destruction

CMainFrame::CMainFrame()
{
	// TODO: add member initialization code here
	pMainFrame = this;	
}

CMainFrame::~CMainFrame()
{
	pMainFrame = 0;
}

int CMainFrame::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CMDIFrameWnd::OnCreate(lpCreateStruct) == -1)
		return -1;
	
	if (!m_wndToolBar.CreateEx(this, TBSTYLE_FLAT, WS_CHILD | WS_VISIBLE | CBRS_TOP
		| CBRS_GRIPPER | CBRS_TOOLTIPS | CBRS_FLYBY | CBRS_SIZE_DYNAMIC) ||
		!m_wndToolBar.LoadToolBar(IDR_MAINFRAME))
	{
		TRACE0("Failed to create toolbar\n");
		return -1;      // fail to create
	}

	if (!m_wndStatusBar.Create(this) ||
		!m_wndStatusBar.SetIndicators(indicators,
		  sizeof(indicators)/sizeof(UINT)))
	{
		TRACE0("Failed to create status bar\n");
		return -1;      // fail to create
	}

	// TODO: Delete these three lines if you don't want the toolbar to
	//  be dockable
	m_wndToolBar.EnableDocking(CBRS_ALIGN_ANY);
	EnableDocking(CBRS_ALIGN_ANY);
	DockControlBar(&m_wndToolBar);

	TBBUTTON	Button;
	CRect		Rect;

	memset(&Button, 0, sizeof(Button));
	Button.iBitmap   = 100;
	Button.idCommand = 100;
	//Button.fsState = TBSTATE_ENABLED;
	Button.fsStyle = TBSTYLE_SEP;
	Button.iString = 100;

	m_wndToolBar.GetToolBarCtrl().AddButtons(1, &Button);
	m_wndToolBar.GetItemRect(m_wndToolBar.GetToolBarCtrl().GetButtonCount() - 1, &Rect);
	Rect.top = 1;
	Rect.bottom = 22;
	Rect.left += 5;
	Rect.right += 5;
	m_Designation.Create(WS_CHILD | WS_BORDER | WS_VISIBLE,
				  Rect,(CWnd *)&m_wndToolBar, 200);
	m_Designation.ModifyStyleEx(0,WS_EX_CLIENTEDGE);
	

	m_wndToolBar.GetToolBarCtrl().AddButtons(1, &Button);
	m_wndToolBar.GetItemRect(m_wndToolBar.GetToolBarCtrl().GetButtonCount() - 1, &Rect);
	Rect.top = 1;
	Rect.bottom = 22;
	Rect.left += 10;
	Rect.right += 10;
	
	m_Page.Create(WS_CHILD | WS_BORDER | WS_VISIBLE,
				  Rect,(CWnd *)&m_wndToolBar, 201);
	m_Page.ModifyStyleEx(0,WS_EX_CLIENTEDGE);
	
	m_wndToolBar.GetToolBarCtrl().AddButtons(1, &Button);
	m_wndToolBar.GetItemRect(m_wndToolBar.GetToolBarCtrl().GetButtonCount() - 1, &Rect);
	Rect.top = 1;
	Rect.bottom = 22;
	Rect.left += 15;
	Rect.right += 20;
	
	m_Line.Create(WS_CHILD | WS_BORDER | WS_VISIBLE,
				  Rect,(CWnd *)&m_wndToolBar, 201);
	m_Line.ModifyStyleEx(0,WS_EX_CLIENTEDGE);
	
	return 0;
}

BOOL CMainFrame::PreCreateWindow(CREATESTRUCT& cs)
{
	if( !CMDIFrameWnd::PreCreateWindow(cs) )
		return FALSE;
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	cs.style = WS_OVERLAPPED | WS_CAPTION | FWS_ADDTOTITLE
		| WS_THICKFRAME | WS_SYSMENU | WS_MINIMIZEBOX | WS_MAXIMIZEBOX | WS_MAXIMIZE;

	return TRUE;
}

/////////////////////////////////////////////////////////////////////////////
// CMainFrame diagnostics

#ifdef _DEBUG
void CMainFrame::AssertValid() const
{
	CMDIFrameWnd::AssertValid();
}

void CMainFrame::Dump(CDumpContext& dc) const
{
	CMDIFrameWnd::Dump(dc);
}

#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CMainFrame message handlers


void CMainFrame::OnMethodBarline() 
{
	// TODO: Add your command handler code here
	
}
