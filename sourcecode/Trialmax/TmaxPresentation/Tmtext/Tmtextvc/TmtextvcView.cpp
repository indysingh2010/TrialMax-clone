#include "stdafx.h"
#include "Tmtextvc.h"
#include "Tmtdoc.h"
#include "Tmtview.h"
#include <font.h>
#include "gnumber.h"
#include "getline.h"
#include "mainfrm.h"
#include "fontname.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

extern CMainFrame*	pMainFrame;

IMPLEMENT_DYNCREATE(CTmtextView, CFormView)

BEGIN_MESSAGE_MAP(CTmtextView, CFormView)
	//{{AFX_MSG_MAP(CTmtextView)
	ON_WM_SIZE()
	ON_COMMAND(ID_PROP_BACKCOLOR, OnPropBackColor)
	ON_COMMAND(ID_PROP_FORECOLOR, OnPropForeColor)
	ON_COMMAND(ID_PROP_HIGHLIGHT, OnPropHighlightColor)
	ON_COMMAND(ID_PROP_HIGHTEXT, OnPropHighTextColor)
	ON_COMMAND(ID_PROP_FONT, OnPropFont)
	ON_COMMAND(ID_PROP_HIGHLINES, OnPropHighLines)
	ON_COMMAND(ID_PROP_DISPLAYLINES, OnPropDisplayLines)
	ON_COMMAND(ID_PROP_COMBINE, OnPropCombine)
	ON_UPDATE_COMMAND_UI(ID_PROP_COMBINE, OnUpdatePropCombine)
	ON_COMMAND(ID_PROP_ENABLEERRORS, OnPropEnableErrors)
	ON_UPDATE_COMMAND_UI(ID_PROP_ENABLEERRORS, OnUpdatePropEnableErrors)
	ON_COMMAND(ID_PROP_SHOWPGLN, OnPropShowPgln)
	ON_UPDATE_COMMAND_UI(ID_PROP_SHOWPGLN, OnUpdatePropShowPgln)
	ON_COMMAND(ID_PROP_BOTTOMMARGIN, OnPropBottommargin)
	ON_COMMAND(ID_PROP_LEFTMARGIN, OnPropLeftmargin)
	ON_COMMAND(ID_PROP_MAXCHARSPERLINE, OnPropMaxcharsperline)
	ON_COMMAND(ID_PROP_RESIZEONCHANGE, OnPropResizeonchange)
	ON_UPDATE_COMMAND_UI(ID_PROP_RESIZEONCHANGE, OnUpdatePropResizeonchange)
	ON_COMMAND(ID_PROP_RIGHTMARGIN, OnPropRightmargin)
	ON_COMMAND(ID_PROP_TOPMARGIN, OnPropTopmargin)
	ON_COMMAND(ID_PROP_USEAVGCHARWIDTH, OnPropUseavgcharwidth)
	ON_UPDATE_COMMAND_UI(ID_PROP_USEAVGCHARWIDTH, OnUpdatePropUseavgcharwidth)
	ON_COMMAND(ID_METHOD_GETMINHEIGHT, OnMethodGetminheight)
	ON_COMMAND(ID_METHOD_RESIZEFONT, OnMethodResizefont)
	ON_COMMAND(ID_METHOD_NEXT, OnMethodNext)
	ON_UPDATE_COMMAND_UI(ID_METHOD_NEXT, OnUpdateMethodNext)
	ON_COMMAND(ID_METHOD_PREVIOUS, OnMethodPrevious)
	ON_UPDATE_COMMAND_UI(ID_METHOD_PREVIOUS, OnUpdateMethodPrevious)
	ON_COMMAND(ID_METHOD_SETLINE, OnMethodSetline)
	ON_COMMAND(ID_METHOD_GETCHARWIDTH, OnMethodGetcharwidth)
	ON_COMMAND(ID_METHOD_GETCHARHEIGHT, OnMethodGetcharheight)
	ON_COMMAND(ID_METHOD_BARLINE, OnMethodBarline)
	ON_COMMAND(ID_METHOD_SETFONTBYNAME, OnMethodSetfontbyname)
	ON_COMMAND(ID_PROP_SCROLLSTEPS, OnPropScrollsteps)
	ON_COMMAND(ID_PROP_SCROLLTIME, OnPropScrolltime)
	ON_COMMAND(ID_PROP_SMOOTHSCROLL, OnPropSmoothscroll)
	ON_UPDATE_COMMAND_UI(ID_PROP_SMOOTHSCROLL, OnUpdatePropSmoothscroll)
	ON_COMMAND(ID_CLASS_ID, OnClassId)
	ON_COMMAND(ID_REGISTERED_PATH, OnRegisteredPath)
	//}}AFX_MSG_MAP
	// Standard printing commands
	ON_COMMAND(ID_FILE_PRINT, CFormView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_DIRECT, CFormView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_PREVIEW, CFormView::OnFilePrintPreview)
END_MESSAGE_MAP()

#ifdef _DEBUG
CTmtextDoc* CTmtextView::GetDocument() 
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CTmtextDoc)));
	return (CTmtextDoc*)m_pDocument;
}
#endif //_DEBUG

CTmtextView::CTmtextView() : CFormView(CTmtextView::IDD)
{
}

CTmtextView::~CTmtextView()
{
}

void CTmtextView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CTmtextView)
	DDX_Control(pDX, IDC_TMTEXTCTRL1, m_TMText);
	//}}AFX_DATA_MAP
}

void CTmtextView::OnInitialUpdate()
{
	CFormView::OnInitialUpdate();
	
	//	Get the active playlist
	CTmtextDoc* pDoc = GetDocument();

	//	Make sure the control uses the whole client area 
	if(IsWindow(m_TMText.m_hWnd))
	{	
		//m_TMText.SetPlaylist((long)m_pPlaylist, FALSE);

		RECT rcClient;
		GetClientRect(&rcClient);
		m_TMText.SetMaxWidth((short)rcClient.right, FALSE);
		m_TMText.MoveWindow(0, rcClient.bottom - m_TMText.GetMinHeight(), 
							rcClient.right, m_TMText.GetMinHeight());

	}


}

void CTmtextView::OnSize(UINT nType, int cx, int cy) 
{
	CFormView::OnSize(nType, cx, cy);
	
	//	Resize the control to match the new client area 
	if(IsWindow(m_TMText.m_hWnd))
	{	
		m_TMText.SetMaxWidth(cx, FALSE);
		m_TMText.MoveWindow(0, cy - m_TMText.GetMinHeight(), cx, m_TMText.GetMinHeight());
	}
}

void CTmtextView::OnPropBackColor() 
{
	COLORREF crBack;

	//	Translate the current background color
	OleTranslateColor(m_TMText.GetBackColor(), NULL, &crBack);

	CColorDialog Colors(crBack);
	if(Colors.DoModal() == IDOK)
		m_TMText.SetBackColor((OLE_COLOR)Colors.GetColor());
}

void CTmtextView::OnPropForeColor() 
{
	COLORREF crFore;

	//	Translate the current background color
	OleTranslateColor(m_TMText.GetForeColor(), NULL, &crFore);

	CColorDialog Colors(crFore);
	if(Colors.DoModal() == IDOK)
		m_TMText.SetForeColor((OLE_COLOR)Colors.GetColor());
}

void CTmtextView::OnPropHighlightColor() 
{
	COLORREF crHighlight;

	//	Translate the current background color
	OleTranslateColor(m_TMText.GetHighlightColor(), NULL, &crHighlight);

	CColorDialog Colors(crHighlight);
	if(Colors.DoModal() == IDOK)
		m_TMText.SetHighlightColor((OLE_COLOR)Colors.GetColor());
}

void CTmtextView::OnPropHighTextColor() 
{
	COLORREF crHighlightText;

	//	Translate the current background color
	OleTranslateColor(m_TMText.GetHighlightTextColor(), NULL, &crHighlightText);

	CColorDialog Colors(crHighlightText);
	if(Colors.DoModal() == IDOK)
		m_TMText.SetHighlightTextColor((OLE_COLOR)Colors.GetColor());
}

void CTmtextView::OnPropFont() 
{
	LOGFONT		lfFont;
	COleFont	OleFont = m_TMText.GetFont();
	CY			FontCy = OleFont.GetSize();
	CFontDialog Font;


	ZeroMemory(&lfFont, sizeof(lfFont));
	lfFont.lfItalic = OleFont.GetItalic();
	lfFont.lfUnderline = OleFont.GetUnderline();
	lfFont.lfCharSet = (unsigned char)OleFont.GetCharset();
	lfFont.lfWeight = OleFont.GetWeight();
	lfFont.lfStrikeOut = OleFont.GetStrikethrough();
	lfFont.lfHeight = m_TMText.GetCharHeight() * -1;
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
		m_TMText.SetFont(OleFont);
	}
}

void CTmtextView::OnPropHighLines() 
{
	CGetNumber Dialog;

	Dialog.m_strLabel = "Highlight Lines";
	Dialog.m_strNumber.Format("%d", m_TMText.GetHighlightLines());

	if(Dialog.DoModal() == IDOK)
		m_TMText.SetHighlightLines(atoi(Dialog.m_strNumber));
}

void CTmtextView::OnPropDisplayLines() 
{
	CGetNumber Dialog;

	Dialog.m_strLabel = "Display Lines";
	Dialog.m_strNumber.Format("%d", m_TMText.GetDisplayLines());

	if(Dialog.DoModal() == IDOK)
		m_TMText.SetDisplayLines(atoi(Dialog.m_strNumber));
}

void CTmtextView::OnPropCombine() 
{
	m_TMText.SetCombineDesignations(!m_TMText.GetCombineDesignations());	
}

void CTmtextView::OnUpdatePropCombine(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMText.GetCombineDesignations());
}

void CTmtextView::OnPropEnableErrors() 
{
	m_TMText.SetEnableErrors(!m_TMText.GetEnableErrors());	
}

void CTmtextView::OnUpdatePropEnableErrors(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMText.GetEnableErrors());
}

void CTmtextView::OnPropShowPgln() 
{
	m_TMText.SetShowPageLine(!m_TMText.GetShowPageLine());	
}

void CTmtextView::OnUpdatePropShowPgln(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMText.GetShowPageLine());
}

BEGIN_EVENTSINK_MAP(CTmtextView, CFormView)
    //{{AFX_EVENTSINK_MAP(CTmtextView)
	ON_EVENT(CTmtextView, IDC_TMTEXTCTRL1, 1 /* HeightChange */, OnHeightChangeTmtextctrl1, VTS_I2)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()

void CTmtextView::OnHeightChangeTmtextctrl1(short sHeight) 
{
	if(IsWindow(m_TMText.m_hWnd))
	{	
		RECT rcClient;
		GetClientRect(&rcClient);
		m_TMText.MoveWindow(0, rcClient.bottom - sHeight, 
							rcClient.right, sHeight);
	}
}

void CTmtextView::OnPropBottommargin() 
{
	CGetNumber Dialog;
	Dialog.m_strLabel = "Bottom Margin";
	Dialog.m_strNumber.Format("%d", m_TMText.GetBottomMargin());
	if(Dialog.DoModal() == IDOK)
		m_TMText.SetBottomMargin(atoi(Dialog.m_strNumber));
}

void CTmtextView::OnPropLeftmargin() 
{
	CGetNumber Dialog;
	Dialog.m_strLabel = "Left Margin";
	Dialog.m_strNumber.Format("%d", m_TMText.GetLeftMargin());
	if(Dialog.DoModal() == IDOK)
		m_TMText.SetLeftMargin(atoi(Dialog.m_strNumber));
}

void CTmtextView::OnPropMaxcharsperline() 
{
	CGetNumber Dialog;
	Dialog.m_strLabel = "Maximum Characters Per Line";
	Dialog.m_strNumber.Format("%d", m_TMText.GetMaxCharsPerLine());
	if(Dialog.DoModal() == IDOK)
		m_TMText.SetMaxCharsPerLine(atoi(Dialog.m_strNumber));
}

void CTmtextView::OnPropResizeonchange() 
{
	m_TMText.SetResizeOnChange(!m_TMText.GetResizeOnChange());	
}

void CTmtextView::OnUpdatePropResizeonchange(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMText.GetResizeOnChange());
}

void CTmtextView::OnPropRightmargin() 
{
	CGetNumber Dialog;
	Dialog.m_strLabel = "Right Margin";
	Dialog.m_strNumber.Format("%d", m_TMText.GetRightMargin());
	if(Dialog.DoModal() == IDOK)
		m_TMText.SetRightMargin(atoi(Dialog.m_strNumber));
}

void CTmtextView::OnPropTopmargin() 
{
	CGetNumber Dialog;
	Dialog.m_strLabel = "Top Margin";
	Dialog.m_strNumber.Format("%d", m_TMText.GetTopMargin());
	if(Dialog.DoModal() == IDOK)
		m_TMText.SetTopMargin(atoi(Dialog.m_strNumber));
}

void CTmtextView::OnPropUseavgcharwidth() 
{
	m_TMText.SetUseAvgCharWidth(!m_TMText.GetUseAvgCharWidth());	
}

void CTmtextView::OnUpdatePropUseavgcharwidth(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMText.GetUseAvgCharWidth());	
}

void CTmtextView::OnMethodGetminheight() 
{
	CString M;
	M.Format("Minimum Height: %d", m_TMText.GetMinHeight());
	MessageBox(M);
}

void CTmtextView::OnMethodResizefont() 
{
	m_TMText.ResizeFont(TRUE);	
}

void CTmtextView::OnMethodNext() 
{
	m_TMText.Next(TRUE);
}

void CTmtextView::OnUpdateMethodNext(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_TMText.GetCurrentLine() && !m_TMText.IsLastLine());
}

void CTmtextView::OnMethodPrevious() 
{
	m_TMText.Previous(TRUE);
}

void CTmtextView::OnUpdateMethodPrevious(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_TMText.GetCurrentLine() && !m_TMText.IsFirstLine());
}

void CTmtextView::OnMethodSetline() 
{
/*
	CLine* pLine;

	CGetLine GetLine;

	if((pLine = (CLine*)m_TMText.GetCurrentLine()) != 0)
	{
		GetLine.m_lDesignation = pLine->m_lDesignation;
		GetLine.m_sPage = pLine->m_sPage;
		GetLine.m_sLine = pLine->m_sNumber;
	}
	else
	{
		GetLine.m_lDesignation = 0;
		GetLine.m_sPage = 0;
		GetLine.m_sLine = 0;
	}

	if(GetLine.DoModal() == IDOK)
		m_TMText.SetLine(GetLine.m_lDesignation, GetLine.m_sPage,
						 GetLine.m_sLine, TRUE);
*/	
}

void CTmtextView::OnMethodGetcharwidth() 
{
	CString M;
	M.Format("Character Cell Width: %d", m_TMText.GetCharWidth());
	MessageBox(M);	
}

void CTmtextView::OnMethodGetcharheight() 
{
	CString M;
	M.Format("Character Cell Height: %d", m_TMText.GetCharHeight());
	MessageBox(M);	
}

void CTmtextView::OnMethodBarline() 
{
	CString strDesignation;
	CString	strPage;
	CString	strLine;
	long	lDesignation;
	short	sPage;
	short	sLine;

	if(!pMainFrame)
		return;

	pMainFrame->m_Designation.GetWindowText(strDesignation);
	pMainFrame->m_Page.GetWindowText(strPage);
	pMainFrame->m_Line.GetWindowText(strLine);
	
	lDesignation = atol(strDesignation);
	sPage = atoi(strPage);
	sLine = atoi(strLine);

	m_TMText.SetLine(lDesignation, sPage, sLine, TRUE);
}

void CTmtextView::OnMethodSetfontbyname() 
{
	CFontName	Fonts;
	COleFont	OleFont = m_TMText.GetFont();

	if(Fonts.DoModal() == IDOK)
	{
		OleFont.SetName(Fonts.m_strFont);
		m_TMText.SetFont(OleFont);
		m_TMText.ResizeFont(TRUE);
	}
	
}

void CTmtextView::OnPropScrollsteps() 
{
	CGetNumber Dialog;
	Dialog.m_strLabel = "Scroll Steps";
	Dialog.m_strNumber.Format("%d", m_TMText.GetScrollSteps());
	if(Dialog.DoModal() == IDOK)
		m_TMText.SetScrollSteps(atoi(Dialog.m_strNumber));
}

void CTmtextView::OnPropScrolltime() 
{
	CGetNumber Dialog;
	Dialog.m_strLabel = "Scroll Time (msec)";
	Dialog.m_strNumber.Format("%d", m_TMText.GetScrollTime());
	if(Dialog.DoModal() == IDOK)
		m_TMText.SetScrollTime(atoi(Dialog.m_strNumber));
}

void CTmtextView::OnPropSmoothscroll() 
{
	m_TMText.SetSmoothScroll(!m_TMText.GetSmoothScroll());
}

void CTmtextView::OnUpdatePropSmoothscroll(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMText.GetSmoothScroll());
}

void CTmtextView::OnClassId() 
{
	CString Class = m_TMText.GetClassIdString();
	MessageBox(Class);	
}

void CTmtextView::OnRegisteredPath() 
{
	CString Path = m_TMText.GetRegisteredPath();
	MessageBox(Path);	
}
