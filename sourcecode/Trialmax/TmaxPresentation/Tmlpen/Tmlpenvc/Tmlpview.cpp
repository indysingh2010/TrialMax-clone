// Tmlpview.cpp : implementation of the CTmlpenvcView class
//

#include "stdafx.h"
#include "Tmlpenvc.h"

#include "Tmlpdoc.h"
#include "Tmlpview.h"
#include <tmlpdefs.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CTmlpenvcView

IMPLEMENT_DYNCREATE(CTmlpenvcView, CFormView)

BEGIN_MESSAGE_MAP(CTmlpenvcView, CFormView)
	//{{AFX_MSG_MAP(CTmlpenvcView)
	ON_COMMAND(ID_PROP_APPEARANCE, OnPropAppearance)
	ON_UPDATE_COMMAND_UI(ID_PROP_APPEARANCE, OnUpdatePropAppearance)
	ON_COMMAND(ID_PROP_BACKCOLOR, OnPropBackcolor)
	ON_COMMAND(ID_PROP_FORECOLOR, OnPropForecolor)
	ON_COMMAND(ID_PROP_BORDER, OnPropBorder)
	ON_UPDATE_COMMAND_UI(ID_PROP_BORDER, OnUpdatePropBorder)
	ON_COMMAND(ID_VIEW_CLASS_ID, OnViewClassId)
	ON_COMMAND(ID_VIEW_REGISTERED_PATH, OnViewRegisteredPath)
	//}}AFX_MSG_MAP
	// Standard printing commands
	ON_COMMAND(ID_FILE_PRINT, CFormView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_DIRECT, CFormView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_PREVIEW, CFormView::OnFilePrintPreview)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTmlpenvcView construction/destruction

CTmlpenvcView::CTmlpenvcView()
	: CFormView(CTmlpenvcView::IDD)
{
	//{{AFX_DATA_INIT(CTmlpenvcView)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// TODO: add construction code here

}

CTmlpenvcView::~CTmlpenvcView()
{
}

void CTmlpenvcView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CTmlpenvcView)
	DDX_Control(pDX, IDC_TMLPENCTRL1, m_TMLpen);
	//}}AFX_DATA_MAP
}

BOOL CTmlpenvcView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	return CFormView::PreCreateWindow(cs);
}

void CTmlpenvcView::OnInitialUpdate()
{
	CFormView::OnInitialUpdate();
	ResizeParentToFit();

}

/////////////////////////////////////////////////////////////////////////////
// CTmlpenvcView printing

BOOL CTmlpenvcView::OnPreparePrinting(CPrintInfo* pInfo)
{
	// default preparation
	return DoPreparePrinting(pInfo);
}

void CTmlpenvcView::OnBeginPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: add extra initialization before printing
}

void CTmlpenvcView::OnEndPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: add cleanup after printing
}

void CTmlpenvcView::OnPrint(CDC* pDC, CPrintInfo* /*pInfo*/)
{
	// TODO: add customized printing code here
}

/////////////////////////////////////////////////////////////////////////////
// CTmlpenvcView diagnostics

#ifdef _DEBUG
void CTmlpenvcView::AssertValid() const
{
	CFormView::AssertValid();
}

void CTmlpenvcView::Dump(CDumpContext& dc) const
{
	CFormView::Dump(dc);
}

CTmlpenvcDoc* CTmlpenvcView::GetDocument() // non-debug version is inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CTmlpenvcDoc)));
	return (CTmlpenvcDoc*)m_pDocument;
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CTmlpenvcView message handlers

BEGIN_EVENTSINK_MAP(CTmlpenvcView, CFormView)
    //{{AFX_EVENTSINK_MAP(CTmlpenvcView)
	ON_EVENT(CTmlpenvcView, IDC_TMLPENCTRL1, 1 /* MouseClick */, OnMouseClickTmlpenctrl1, VTS_I2 VTS_I2)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()

void CTmlpenvcView::OnPropAppearance() 
{
	if(m_TMLpen.GetAppearance())
		m_TMLpen.SetAppearance(0);
	else
		m_TMLpen.SetAppearance(1);	
}

void CTmlpenvcView::OnUpdatePropAppearance(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMLpen.GetAppearance() == 0);
}

void CTmlpenvcView::OnPropBackcolor() 
{
	COLORREF crBack;

	//	Translate the current background color
	OleTranslateColor(m_TMLpen.GetBackColor(), NULL, &crBack);

	CColorDialog Colors(crBack);
	if(Colors.DoModal() == IDOK)
		m_TMLpen.SetBackColor((OLE_COLOR)Colors.GetColor());
}

void CTmlpenvcView::OnPropForecolor() 
{
	COLORREF crFore;

	//	Translate the current background color
	OleTranslateColor(m_TMLpen.GetForeColor(), NULL, &crFore);

	CColorDialog Colors(crFore);
	if(Colors.DoModal() == IDOK)
		m_TMLpen.SetForeColor((OLE_COLOR)Colors.GetColor());
}

void CTmlpenvcView::OnPropBorder() 
{
	if(m_TMLpen.GetBorderStyle())
		m_TMLpen.SetBorderStyle(0);
	else
		m_TMLpen.SetBorderStyle(1);	
}

void CTmlpenvcView::OnUpdatePropBorder(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMLpen.GetBorderStyle());
}

void CTmlpenvcView::OnMouseClickTmlpenctrl1(short sButton, short sKeyState) 
{
	CString strButton;
	CString strKeys;
	CString strMessage;

	strButton = (sButton == TMLPEN_LEFT) ? "Left" : "Right";

	switch(sKeyState)
	{
		case TMLPEN_NOKEYS:				strKeys = "None";
										break;
		case TMLPEN_ALT:				strKeys = "Alt";
										break;
		case TMLPEN_CONTROL:			strKeys = "Control";
										break;
		case TMLPEN_SHIFT:				strKeys = "Shift";
										break;
		case TMLPEN_ALTCONTROL:			strKeys = "Alt-Control";
										break;
		case TMLPEN_ALTSHIFT:			strKeys = "Alt-Shift";
										break;
		case TMLPEN_ALTSHIFTCONTROL:	strKeys = "Alt-Shift-Control";
										break;
		case TMLPEN_SHIFTCONTROL:		strKeys = "Shift-Control";
										break;
		default:						strKeys = "Invalid";
										break;
	}

	strMessage.Format("Button: %s\nKeys: %s", strButton, strKeys);
	MessageBox(strMessage);	
}

void CTmlpenvcView::OnViewClassId() 
{
	CString Class = m_TMLpen.GetClassIdString();
	MessageBox(Class);	
}

void CTmlpenvcView::OnViewRegisteredPath() 
{
	CString Path = m_TMLpen.GetRegisteredPath();
	MessageBox(Path);	
}
