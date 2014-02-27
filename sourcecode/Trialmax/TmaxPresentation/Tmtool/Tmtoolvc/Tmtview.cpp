// Tmtview.cpp : implementation of the CTmtoolvcView class
//

#include "stdafx.h"
#include "Tmtoolvc.h"

#include "Tmtdoc.h"
#include "Tmtview.h"
#include <tmtbdefs.h>
#include "bmask.h"
#include "getnum.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CTmtoolvcView

IMPLEMENT_DYNCREATE(CTmtoolvcView, CFormView)

BEGIN_MESSAGE_MAP(CTmtoolvcView, CFormView)
	//{{AFX_MSG_MAP(CTmtoolvcView)
	ON_WM_SIZE()
	ON_COMMAND(ID_DOCKBOTTOM, OnDockbottom)
	ON_UPDATE_COMMAND_UI(ID_DOCKBOTTOM, OnUpdateDockbottom)
	ON_COMMAND(ID_DOCKLEFT, OnDockleft)
	ON_UPDATE_COMMAND_UI(ID_DOCKLEFT, OnUpdateDockleft)
	ON_COMMAND(ID_DOCKRIGHT, OnDockright)
	ON_UPDATE_COMMAND_UI(ID_DOCKRIGHT, OnUpdateDockright)
	ON_COMMAND(ID_DOCKTOP, OnDocktop)
	ON_UPDATE_COMMAND_UI(ID_DOCKTOP, OnUpdateDocktop)
	ON_COMMAND(ID_RAISED, OnRaised)
	ON_UPDATE_COMMAND_UI(ID_RAISED, OnUpdateRaised)
	ON_COMMAND(ID_STRETCH, OnStretch)
	ON_UPDATE_COMMAND_UI(ID_STRETCH, OnUpdateStretch)
	ON_COMMAND(ID_LARGEBUTTONS, OnLargebuttons)
	ON_UPDATE_COMMAND_UI(ID_LARGEBUTTONS, OnUpdateLargebuttons)
	ON_COMMAND(ID_MEDIUMBUTTONS, OnMediumbuttons)
	ON_UPDATE_COMMAND_UI(ID_MEDIUMBUTTONS, OnUpdateMediumbuttons)
	ON_COMMAND(ID_SMALLBUTTONS, OnSmallbuttons)
	ON_UPDATE_COMMAND_UI(ID_SMALLBUTTONS, OnUpdateSmallbuttons)
	ON_COMMAND(ID_BACKGROUND, OnBackground)
	ON_COMMAND(ID_BUTTONMASK, OnButtonmask)
	ON_COMMAND(ID_TOOLTIPS, OnTooltips)
	ON_UPDATE_COMMAND_UI(ID_TOOLTIPS, OnUpdateTooltips)
	ON_COMMAND(ID_TOGGLEPLAY, OnToggleplay)
	ON_COMMAND(ID_TOGGLESPLIT, OnTogglesplit)
	ON_COMMAND(ID_TOGGLELINKS, OnTogglelinks)
	ON_COMMAND(ID_CONFIGURABLE, OnConfigurable)
	ON_UPDATE_COMMAND_UI(ID_CONFIGURABLE, OnUpdateConfigurable)
	ON_COMMAND(ID_CONFIGURE, OnConfigure)
	ON_COMMAND(ID_VIEW_CLASS_ID, OnViewClassId)
	ON_COMMAND(ID_VIEW_REGISTERED_PATH, OnViewRegisteredPath)
	ON_COMMAND(ID_BUTTON_ROWS, OnButtonRows)
	ON_COMMAND(ID_AUTO_RESET, OnAutoReset)
	ON_COMMAND(ID_RESET, OnReset)
	ON_UPDATE_COMMAND_UI(ID_AUTO_RESET, OnUpdateAutoReset)
	//}}AFX_MSG_MAP
	// Standard printing commands
	ON_COMMAND(ID_FILE_PRINT, CFormView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_DIRECT, CFormView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_PREVIEW, CFormView::OnFilePrintPreview)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTmtoolvcView construction/destruction

CTmtoolvcView::CTmtoolvcView()
	: CFormView(CTmtoolvcView::IDD)
{
	//{{AFX_DATA_INIT(CTmtoolvcView)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// TODO: add construction code here
	bStretch = TRUE;
	bRaised = FALSE;
	bPlaying = FALSE;
	bSplit = FALSE;
	bDisableLinks = FALSE;
}

CTmtoolvcView::~CTmtoolvcView()
{
}

void CTmtoolvcView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CTmtoolvcView)
	DDX_Control(pDX, IDC_TMTOOLCTRL1, m_TMTool);
	//}}AFX_DATA_MAP
}

BOOL CTmtoolvcView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	return CFormView::PreCreateWindow(cs);
}

/////////////////////////////////////////////////////////////////////////////
// CTmtoolvcView printing

BOOL CTmtoolvcView::OnPreparePrinting(CPrintInfo* pInfo)
{
	// default preparation
	return DoPreparePrinting(pInfo);
}

void CTmtoolvcView::OnBeginPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: add extra initialization before printing
}

void CTmtoolvcView::OnEndPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: add cleanup after printing
}

void CTmtoolvcView::OnPrint(CDC* pDC, CPrintInfo*)
{
	// TODO: add code to print the controls
}

/////////////////////////////////////////////////////////////////////////////
// CTmtoolvcView diagnostics

#ifdef _DEBUG
void CTmtoolvcView::AssertValid() const
{
	CFormView::AssertValid();
}

void CTmtoolvcView::Dump(CDumpContext& dc) const
{
	CFormView::Dump(dc);
}

CTmtoolvcDoc* CTmtoolvcView::GetDocument() // non-debug version is inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CTmtoolvcDoc)));
	return (CTmtoolvcDoc*)m_pDocument;
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CTmtoolvcView message handlers

void CTmtoolvcView::OnSize(UINT nType, int cx, int cy) 
{
	CFormView::OnSize(nType, cx, cy);
	
	if(IsWindow(m_TMTool.m_hWnd))
		m_TMTool.ResetFrame();
	
}

void CTmtoolvcView::OnDockbottom() 
{
	m_TMTool.SetOrientation(TMTB_BOTTOM);
}

void CTmtoolvcView::OnUpdateDockbottom(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck((m_TMTool.GetOrientation() == TMTB_BOTTOM) ? 1 : 0);
}

void CTmtoolvcView::OnDockleft() 
{
	m_TMTool.SetOrientation(TMTB_LEFT);
}

void CTmtoolvcView::OnUpdateDockleft(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck((m_TMTool.GetOrientation() == TMTB_LEFT) ? 1 : 0);
}

void CTmtoolvcView::OnDockright() 
{
	m_TMTool.SetOrientation(TMTB_RIGHT);
}

void CTmtoolvcView::OnUpdateDockright(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck((m_TMTool.GetOrientation() == TMTB_RIGHT) ? 1 : 0);
}

void CTmtoolvcView::OnDocktop() 
{
	m_TMTool.SetOrientation(TMTB_TOP);
}

void CTmtoolvcView::OnUpdateDocktop(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck((m_TMTool.GetOrientation() == TMTB_TOP) ? 1 : 0);
}

void CTmtoolvcView::OnRaised() 
{
	bRaised = !bRaised;
	m_TMTool.SetStyle((bRaised) ? TMTB_RAISED : TMTB_FLAT);
}

void CTmtoolvcView::OnUpdateRaised(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck((m_TMTool.GetStyle() == TMTB_RAISED) ? 1 : 0);
}

void CTmtoolvcView::OnStretch() 
{
	bStretch = !bStretch;
	m_TMTool.SetStretch(bStretch);
}

void CTmtoolvcView::OnUpdateStretch(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck((m_TMTool.GetStretch() == TRUE) ? 1 : 0);
}

void CTmtoolvcView::OnLargebuttons() 
{
	m_TMTool.SetButtonSize(TMTB_LARGEBUTTONS);
}

void CTmtoolvcView::OnUpdateLargebuttons(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck((m_TMTool.GetButtonSize() == TMTB_LARGEBUTTONS) ? 1 : 0);
}

void CTmtoolvcView::OnMediumbuttons() 
{
	m_TMTool.SetButtonSize(TMTB_MEDIUMBUTTONS);
}

void CTmtoolvcView::OnUpdateMediumbuttons(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck((m_TMTool.GetButtonSize() == TMTB_MEDIUMBUTTONS) ? 1 : 0);
}

void CTmtoolvcView::OnSmallbuttons() 
{
	m_TMTool.SetButtonSize(TMTB_SMALLBUTTONS);
}

void CTmtoolvcView::OnUpdateSmallbuttons(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck((m_TMTool.GetButtonSize() == TMTB_SMALLBUTTONS) ? 1 : 0);
}

void CTmtoolvcView::OnBackground() 
{
	CColorDialog Dialog;

	if(Dialog.DoModal() == IDOK)
		m_TMTool.SetBackColor((OLE_COLOR)Dialog.GetColor());
	
}

BEGIN_EVENTSINK_MAP(CTmtoolvcView, CFormView)
    //{{AFX_EVENTSINK_MAP(CTmtoolvcView)
	ON_EVENT(CTmtoolvcView, IDC_TMTOOLCTRL1, 1 /* ButtonClick */, OnButtonClickTmtoolctrl1, VTS_I2 VTS_BOOL)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()

void CTmtoolvcView::OnButtonClickTmtoolctrl1(short sId, BOOL bChecked) 
{
	CString M;
	
	switch(sId)
	{
		case TMTB_BLUE:
		case TMTB_RED:
		case TMTB_GREEN:
		case TMTB_YELLOW:
		case TMTB_BLACK:
		case TMTB_WHITE:

			if(bChecked)
				m_TMTool.SetColorButton(sId);
			else
				m_TMTool.SetColorButton(-1);
			break;

		case TMTB_CALLOUT:
		case TMTB_PAN:
		case TMTB_DRAWTOOL:
		case TMTB_HIGHLIGHT:
		case TMTB_REDACT:
		case TMTB_ZOOM:

			if(bChecked)
				m_TMTool.SetToolButton(sId);
			else
				m_TMTool.SetToolButton(-1);
			break;

		default:

			M.Format("Click button id = %d", sId);
			MessageBox(M, "TMTool", MB_ICONINFORMATION | MB_OK);
			break;
	}
}

void CTmtoolvcView::OnButtonmask() 
{
	CButtonMask Dlg;

	Dlg.m_strMask = m_TMTool.GetButtonMask();
	if(Dlg.DoModal() == IDOK)
		m_TMTool.SetButtonMask(Dlg.m_strMask);
	
}


void CTmtoolvcView::OnTooltips() 
{
	m_TMTool.SetToolTips(!m_TMTool.GetToolTips());
}

void CTmtoolvcView::OnUpdateTooltips(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMTool.GetToolTips());
}

void CTmtoolvcView::OnToggleplay() 
{
	bPlaying = !bPlaying;
	m_TMTool.SetPlayButton(bPlaying);	
	
}

void CTmtoolvcView::OnTogglesplit() 
{
	bSplit = !bSplit;
	m_TMTool.SetSplitButton(bSplit, FALSE);
}

void CTmtoolvcView::OnTogglelinks() 
{
	bDisableLinks = !bDisableLinks;
	m_TMTool.SetLinkButton(bDisableLinks);
}

void CTmtoolvcView::OnConfigurable() 
{
	m_TMTool.SetConfigurable(!m_TMTool.GetConfigurable());	
}

void CTmtoolvcView::OnUpdateConfigurable(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck((m_TMTool.GetConfigurable() == TRUE) ? 1 : 0);
}

void CTmtoolvcView::OnConfigure() 
{
	m_TMTool.Configure();
}

void CTmtoolvcView::OnViewClassId() 
{
	CString Class = m_TMTool.GetClassIdString();
	MessageBox(Class);	
}

void CTmtoolvcView::OnViewRegisteredPath() 
{
	CString Path = m_TMTool.GetRegisteredPath();
	MessageBox(Path);	
}

void CTmtoolvcView::OnButtonRows() 
{
	CGetNumber Dlg;

	Dlg.m_iNumber = m_TMTool.GetButtonRows();
	if(Dlg.DoModal() == IDOK)
	{
		m_TMTool.SetButtonRows(Dlg.m_iNumber);
	}
	
}

void CTmtoolvcView::OnAutoReset() 
{
	m_TMTool.SetAutoReset(!m_TMTool.GetAutoReset());	
}

void CTmtoolvcView::OnReset() 
{
	m_TMTool.Reset();	
}

void CTmtoolvcView::OnUpdateAutoReset(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMTool.GetAutoReset());	
}
