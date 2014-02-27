// TmbrowsevcView.cpp : implementation of the CTmbrowsevcView class
//

#include "stdafx.h"
#include "Tmbrowsevc.h"

#include "TmbrowsevcDoc.h"
#include "TmbrowsevcView.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

extern CTmbrowsevcApp theApp;

/////////////////////////////////////////////////////////////////////////////
// CTmbrowsevcView

IMPLEMENT_DYNCREATE(CTmbrowsevcView, CFormView)

BEGIN_MESSAGE_MAP(CTmbrowsevcView, CFormView)
	//{{AFX_MSG_MAP(CTmbrowsevcView)
	ON_WM_SIZE()
	ON_COMMAND(ID_FILE_CLOSE, OnFileClose)
	ON_COMMAND(ID_COPY, OnCopy)
	ON_UPDATE_COMMAND_UI(ID_COPY, OnUpdateCopy)
	//}}AFX_MSG_MAP
	// Standard printing commands
	ON_COMMAND(ID_FILE_PRINT, CFormView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_DIRECT, CFormView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_PREVIEW, CFormView::OnFilePrintPreview)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTmbrowsevcView construction/destruction

CTmbrowsevcView::CTmbrowsevcView()
	: CFormView(CTmbrowsevcView::IDD)
{
	//{{AFX_DATA_INIT(CTmbrowsevcView)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// TODO: add construction code here

}

CTmbrowsevcView::~CTmbrowsevcView()
{
}

void CTmbrowsevcView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CTmbrowsevcView)
	DDX_Control(pDX, IDC_TMBROWSECTRL1, m_Browser);
	//}}AFX_DATA_MAP
}

BOOL CTmbrowsevcView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	return CFormView::PreCreateWindow(cs);
}

void CTmbrowsevcView::OnInitialUpdate()
{
	CFormView::OnInitialUpdate();
	ResizeParentToFit();

	//	Make sure the control uses the whole client area 
	if(IsWindow(m_Browser.m_hWnd))
	{	
		RECT rcClient;
		GetClientRect(&rcClient);
		m_Browser.MoveWindow(&rcClient);

		//	Set the filename to display
		CTmbrowsevcDoc* pDoc = GetDocument();
		m_Browser.Load(pDoc->GetPathName());
	}
}

/////////////////////////////////////////////////////////////////////////////
// CTmbrowsevcView printing

BOOL CTmbrowsevcView::OnPreparePrinting(CPrintInfo* pInfo)
{
	// default preparation
	return DoPreparePrinting(pInfo);
}

void CTmbrowsevcView::OnBeginPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: add extra initialization before printing
}

void CTmbrowsevcView::OnEndPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: add cleanup after printing
}

void CTmbrowsevcView::OnPrint(CDC* pDC, CPrintInfo* /*pInfo*/)
{
	// TODO: add customized printing code here
}

/////////////////////////////////////////////////////////////////////////////
// CTmbrowsevcView diagnostics

#ifdef _DEBUG
void CTmbrowsevcView::AssertValid() const
{
	CFormView::AssertValid();
}

void CTmbrowsevcView::Dump(CDumpContext& dc) const
{
	CFormView::Dump(dc);
}

CTmbrowsevcDoc* CTmbrowsevcView::GetDocument() // non-debug version is inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CTmbrowsevcDoc)));
	return (CTmbrowsevcDoc*)m_pDocument;
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CTmbrowsevcView message handlers

void CTmbrowsevcView::OnSize(UINT nType, int cx, int cy) 
{
	CFormView::OnSize(nType, cx, cy);
	
	//	Resize the control to match the new client area 
	if(IsWindow(m_Browser.m_hWnd))
	{	
		RECT rcClient;
		GetClientRect(&rcClient);
		m_Browser.MoveWindow(&rcClient);
	
	}
}

void CTmbrowsevcView::OnFileClose() 
{
	m_Browser.Load("");
	
}

//==============================================================================
//
// 	Function Name:	CTmbrowsevcView::CopyFile()
//
// 	Description:	This function will copy the source file to the target file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTmbrowsevcView::CopyFile(LPCSTR lpSource, LPCSTR lpTarget)
{
	SHFILEOPSTRUCT	OpStruct;
	char			szFrom[512];
	char			szTo[512];

	ASSERT(lpSource);
	ASSERT(lpTarget);

	//	The file specifications have to be double null terminated
	memset(szFrom, 0, sizeof(szFrom));
	memset(szTo, 0, sizeof(szTo));
	lstrcpyn(szFrom, lpSource, (sizeof(szFrom) - 1));
	lstrcpyn(szTo, lpTarget, (sizeof(szTo) - 1));

	//	Set up the shell operation structure
	memset(&OpStruct, 0, sizeof(OpStruct));
	OpStruct.hwnd   = m_hWnd;
	OpStruct.wFunc  = FO_COPY;
	OpStruct.pFrom  = szFrom;
	OpStruct.pTo	= szTo;
	OpStruct.fFlags = (FOF_NOCONFIRMATION | FOF_NOCONFIRMMKDIR);
					  
	//	Copy the file
	if(SHFileOperation(&OpStruct) == 0)
	{
		SetFileAttributes(lpTarget, FILE_ATTRIBUTE_NORMAL);
		return TRUE;
	}
	else
	{
		return FALSE;
	}
}


void CTmbrowsevcView::OnCopy() 
{
	CString strCopy;

	theApp.DoWaitCursor(1);
	for(int i = 300; i < 5001; i++)
	{
		strCopy.Format("f:\\0011a\\%0.4d.tif", i);
		CopyFile("f:\\0011a\\0001.tif", strCopy);
	}
	theApp.DoWaitCursor(-1);
	
}

void CTmbrowsevcView::OnUpdateCopy(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(TRUE);
	
}
