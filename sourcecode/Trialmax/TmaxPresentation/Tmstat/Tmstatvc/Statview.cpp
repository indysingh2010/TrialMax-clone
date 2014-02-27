// Statview.cpp : implementation of the CTmstatvcView class
//

#include "stdafx.h"
#include "Tmstatvc.h"

#include "Statdoc.h"
#include "Statview.h"
#include <tmstdefs.h>
#include <font.h>
#include <number.h>
#include <gettext.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CTmstatvcView

IMPLEMENT_DYNCREATE(CTmstatvcView, CFormView)

BEGIN_MESSAGE_MAP(CTmstatvcView, CFormView)
	//{{AFX_MSG_MAP(CTmstatvcView)
	ON_WM_SIZE()
	ON_COMMAND(ID_PROP_AUTOSIZE, OnPropAutosize)
	ON_UPDATE_COMMAND_UI(ID_PROP_AUTOSIZE, OnUpdatePropAutosize)
	ON_COMMAND(ID_PROP_BACKCOLOR, OnPropBackcolor)
	ON_COMMAND(ID_PROP_FONT, OnPropFont)
	ON_COMMAND(ID_PROP_FORECOLOR, OnPropForecolor)
	ON_COMMAND(ID_PROP_TEXT, OnPropText)
	ON_COMMAND(ID_PROP_BOTTOMMARGIN, OnPropBottommargin)
	ON_COMMAND(ID_PROP_LEFTMARGIN, OnPropLeftmargin)
	ON_COMMAND(ID_PROP_RIGHTMARGIN, OnPropRightmargin)
	ON_COMMAND(ID_PROP_TOPMARGIN, OnPropTopmargin)
	ON_WM_TIMER()
	ON_COMMAND(ID_TIMEOFDAY, OnTimeofday)
	ON_UPDATE_COMMAND_UI(ID_TIMEOFDAY, OnUpdateTimeofday)
	ON_COMMAND(ID_PROP_FLAT, OnPropFlat)
	ON_UPDATE_COMMAND_UI(ID_PROP_FLAT, OnUpdatePropFlat)
	ON_COMMAND(ID_VIEW_CLASS_ID, OnViewClassId)
	ON_COMMAND(ID_VIEW_REGISTERED_PATH, OnViewRegisteredPath)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTmstatvcView construction/destruction

CTmstatvcView::CTmstatvcView()
	: CFormView(CTmstatvcView::IDD)
{
	//{{AFX_DATA_INIT(CTmstatvcView)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	m_bTimerActive = FALSE;
}

CTmstatvcView::~CTmstatvcView()
{
}

void CTmstatvcView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CTmstatvcView)
	DDX_Control(pDX, IDC_TMSTATCTRL1, m_TMStat);
	//}}AFX_DATA_MAP
}

BOOL CTmstatvcView::PreCreateWindow(CREATESTRUCT& cs)
{
	return CFormView::PreCreateWindow(cs);
}

void CTmstatvcView::OnInitialUpdate()
{
	CFormView::OnInitialUpdate();
	ResizeParentToFit();
	MoveStatusBar();
}

/////////////////////////////////////////////////////////////////////////////
// CTmstatvcView diagnostics

#ifdef _DEBUG
void CTmstatvcView::AssertValid() const
{
	CFormView::AssertValid();
}

void CTmstatvcView::Dump(CDumpContext& dc) const
{
	CFormView::Dump(dc);
}

CTmstatvcDoc* CTmstatvcView::GetDocument() // non-debug version is inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CTmstatvcDoc)));
	return (CTmstatvcDoc*)m_pDocument;
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CTmstatvcView message handlers

void CTmstatvcView::OnSize(UINT nType, int cx, int cy) 
{
	CFormView::OnSize(nType, cx, cy);
	MoveStatusBar();
}

void CTmstatvcView::MoveStatusBar()
{
	RECT rcClient;

	if(!IsWindow(m_TMStat.m_hWnd))
		return;

	GetClientRect(&rcClient);

	int iHeight = (int)((double)rcClient.bottom * 0.15);
	if(iHeight < 1)
		iHeight = 1;
	rcClient.top = rcClient.bottom - iHeight;
	m_TMStat.MoveWindow(&rcClient, TRUE);
}

void CTmstatvcView::OnPropAutosize() 
{
	m_TMStat.SetAutosizeFont(!m_TMStat.GetAutosizeFont());	
	m_TMStat.RedrawWindow();
}

void CTmstatvcView::OnUpdatePropAutosize(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMStat.GetAutosizeFont());	
}

void CTmstatvcView::OnPropBackcolor() 
{
	COLORREF crBack;

	//	Translate the current background color
	OleTranslateColor(m_TMStat.GetBackColor(), NULL, &crBack);

	CColorDialog Colors(crBack);
	if(Colors.DoModal() == IDOK)
		m_TMStat.SetBackColor((OLE_COLOR)Colors.GetColor());
}

void CTmstatvcView::OnPropFont() 
{
	LOGFONT		lfFont;
	COleFont	OleFont = m_TMStat.GetFont();
	CY			FontCy = OleFont.GetSize();
	CFontDialog Font;
	int			iPointSize;
	int			iHeight = 0;
	CDC*		pDc = GetDC();

	//	Translate the OLE font size to logical height
	if(pDc)
	{
		iPointSize = (int)(((float)FontCy.Lo / 10000.0) + 0.5);
		iHeight = -MulDiv(iPointSize, pDc->GetDeviceCaps(LOGPIXELSY), 72);
	}

	ZeroMemory(&lfFont, sizeof(lfFont));
	lfFont.lfItalic = OleFont.GetItalic();
	lfFont.lfUnderline = OleFont.GetUnderline();
	lfFont.lfCharSet = (unsigned char)OleFont.GetCharset();
	lfFont.lfWeight = OleFont.GetWeight();
	lfFont.lfStrikeOut = OleFont.GetStrikethrough();
	lfFont.lfHeight = iHeight;
	lstrcpy(lfFont.lfFaceName, OleFont.GetName());

	CFontDialog FontDlg(&lfFont, CF_SCREENFONTS | CF_EFFECTS);

	if(FontDlg.DoModal() == IDOK)
	{
		//	Dialog returns size in tenth's of a point
		FontCy.Lo = (unsigned long)(FontDlg.GetSize() * 1000);
	
		OleFont.SetUnderline(lfFont.lfUnderline);
		OleFont.SetItalic(lfFont.lfItalic);
		OleFont.SetCharset(lfFont.lfCharSet);
		OleFont.SetWeight((short)lfFont.lfWeight);
		OleFont.SetStrikethrough(lfFont.lfStrikeOut);
		OleFont.SetSize(FontCy);
		OleFont.SetName(lfFont.lfFaceName);
		m_TMStat.SetFont(OleFont);
	}
}

void CTmstatvcView::OnPropForecolor() 
{
	COLORREF crFore;

	//	Translate the current background color
	OleTranslateColor(m_TMStat.GetForeColor(), NULL, &crFore);

	CColorDialog Colors(crFore);
	if(Colors.DoModal() == IDOK)
		m_TMStat.SetForeColor((OLE_COLOR)Colors.GetColor());
}

void CTmstatvcView::OnPropText() 
{
	CGetText Dialog;
	Dialog.m_strText = m_TMStat.GetStatusText();
	if(Dialog.DoModal() == IDOK)
		m_TMStat.SetStatusText(Dialog.m_strText);	
}

void CTmstatvcView::OnPropBottommargin() 
{
	GetNumber Dialog;

	Dialog.m_strLabel = "Bottom Margin";
	Dialog.m_sNumber = m_TMStat.GetBottomMargin();
	if(Dialog.DoModal() == IDOK)
		m_TMStat.SetBottomMargin(Dialog.m_sNumber);
}

void CTmstatvcView::OnPropLeftmargin() 
{
	GetNumber Dialog;

	Dialog.m_strLabel = "Left Margin";
	Dialog.m_sNumber = m_TMStat.GetLeftMargin();
	if(Dialog.DoModal() == IDOK)
		m_TMStat.SetLeftMargin(Dialog.m_sNumber);
}

void CTmstatvcView::OnPropRightmargin() 
{
	GetNumber Dialog;

	Dialog.m_strLabel = "Right Margin";
	Dialog.m_sNumber = m_TMStat.GetRightMargin();
	if(Dialog.DoModal() == IDOK)
		m_TMStat.SetRightMargin(Dialog.m_sNumber);
}

void CTmstatvcView::OnPropTopmargin() 
{
	GetNumber Dialog;

	Dialog.m_strLabel = "Top Margin";
	Dialog.m_sNumber = m_TMStat.GetTopMargin();
	if(Dialog.DoModal() == IDOK)
		m_TMStat.SetTopMargin(Dialog.m_sNumber);
}

void CTmstatvcView::OnTimer(UINT nIDEvent) 
{
	CTime Time = CTime::GetCurrentTime();
	CString strTime = Time.Format("%A, %B %d, %Y  %H:%M:%S"); 
	m_TMStat.SetStatusText(strTime);
	CFormView::OnTimer(nIDEvent);
}

void CTmstatvcView::OnTimeofday() 
{
	if(m_bTimerActive)
	{
		KillTimer(1);
		m_bTimerActive = FALSE;
	}
	else
	{
		SetTimer(1, 1000,0);
		m_bTimerActive = TRUE;
		OnTimer(1);
	}
}

void CTmstatvcView::OnUpdateTimeofday(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_bTimerActive);
}

void CTmstatvcView::OnPropFlat() 
{
	if(m_TMStat.GetAppearance())
		m_TMStat.SetAppearance(0);
	else
		m_TMStat.SetAppearance(1);
}

void CTmstatvcView::OnUpdatePropFlat(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMStat.GetAppearance() == 0);
}

BEGIN_EVENTSINK_MAP(CTmstatvcView, CFormView)
    //{{AFX_EVENTSINK_MAP(CTmstatvcView)
	ON_EVENT(CTmstatvcView, IDC_TMSTATCTRL1, -600 /* Click */, OnClickTmstatctrl1, VTS_NONE)
	ON_EVENT(CTmstatvcView, IDC_TMSTATCTRL1, -601 /* DblClick */, OnDblClickTmstatctrl1, VTS_NONE)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()

void CTmstatvcView::OnClickTmstatctrl1() 
{
	//MessageBox("Click");
	
}

void CTmstatvcView::OnDblClickTmstatctrl1() 
{
	MessageBox("DoubleClick");
}

void CTmstatvcView::OnViewClassId() 
{
	CString Class = m_TMStat.GetClassIdString();
	MessageBox(Class);	
}

void CTmstatvcView::OnViewRegisteredPath() 
{
	CString Path = m_TMStat.GetRegisteredPath();
	MessageBox(Path);	
}
