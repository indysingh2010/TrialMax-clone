// Barsview.cpp : implementation of the CTmbarsvcView class
//

#include "stdafx.h"
#include "Tmbarsvc.h"

#include "Barsdoc.h"
#include "Barsview.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CTmbarsvcView

IMPLEMENT_DYNCREATE(CTmbarsvcView, CFormView)

BEGIN_MESSAGE_MAP(CTmbarsvcView, CFormView)
	//{{AFX_MSG_MAP(CTmbarsvcView)
	ON_COMMAND(ID_FILE_SAVE, OnFileSave)
	ON_COMMAND(ID_FILE_OPEN, OnFileOpen)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTmbarsvcView construction/destruction

CTmbarsvcView::CTmbarsvcView()
	: CFormView(CTmbarsvcView::IDD)
{
	//{{AFX_DATA_INIT(CTmbarsvcView)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// TODO: add construction code here

}

CTmbarsvcView::~CTmbarsvcView()
{
}

void CTmbarsvcView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CTmbarsvcView)
	DDX_Control(pDX, IDC_TMBARSCTRL1, m_TMBars);
	//}}AFX_DATA_MAP
}

BOOL CTmbarsvcView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	return CFormView::PreCreateWindow(cs);
}

void CTmbarsvcView::OnInitialUpdate()
{
	CFormView::OnInitialUpdate();
	ResizeParentToFit();

}

/////////////////////////////////////////////////////////////////////////////
// CTmbarsvcView diagnostics

#ifdef _DEBUG
void CTmbarsvcView::AssertValid() const
{
	CFormView::AssertValid();
}

void CTmbarsvcView::Dump(CDumpContext& dc) const
{
	CFormView::Dump(dc);
}

CTmbarsvcDoc* CTmbarsvcView::GetDocument() // non-debug version is inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CTmbarsvcDoc)));
	return (CTmbarsvcDoc*)m_pDocument;
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CTmbarsvcView message handlers

void CTmbarsvcView::OnFileSave() 
{
	m_TMBars.Save();	
}

void CTmbarsvcView::OnFileOpen() 
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
		m_TMBars.SetIniFile(szFilename);
	}
}
