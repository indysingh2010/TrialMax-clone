// TpowervcView.cpp : implementation of the CTpowervcView class
//

#include "stdafx.h"
#include "Tpowervc.h"

#include "TpowervcDoc.h"
#include "TpowervcView.h"
#include "getprop.h"
#include "childfrm.h"
#include "format.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CTpowervcView

IMPLEMENT_DYNCREATE(CTpowervcView, CFormView)

BEGIN_MESSAGE_MAP(CTpowervcView, CFormView)
	//{{AFX_MSG_MAP(CTpowervcView)
	ON_WM_SIZE()
	ON_COMMAND(ID_METHOD_GETPPVERSION, OnMethodGetppversion)
	ON_UPDATE_COMMAND_UI(ID_METHOD_GETPPVERSION, OnUpdateMethodGetppversion)
	ON_COMMAND(ID_PROPERTY_SPLITFRAMECOLOR, OnPropertySplitframecolor)
	ON_UPDATE_COMMAND_UI(ID_PROPERTY_SPLITFRAMECOLOR, OnUpdatePropertySplitframecolor)
	ON_COMMAND(ID_PROPERTY_SPLITFRAMETHICKNESS, OnPropertySplitframethickness)
	ON_UPDATE_COMMAND_UI(ID_PROPERTY_SPLITFRAMETHICKNESS, OnUpdatePropertySplitframethickness)
	ON_COMMAND(ID_PROPERTY_SPLITSCREEN, OnPropertySplitscreen)
	ON_UPDATE_COMMAND_UI(ID_PROPERTY_SPLITSCREEN, OnUpdatePropertySplitscreen)
	ON_COMMAND(ID_PROPERTY_SYNCVIEWS, OnPropertySyncviews)
	ON_UPDATE_COMMAND_UI(ID_PROPERTY_SYNCVIEWS, OnUpdatePropertySyncviews)
	ON_COMMAND(ID_FILE_RELOAD, OnFileReload)
	ON_UPDATE_COMMAND_UI(ID_FILE_RELOAD, OnUpdateFileReload)
	ON_COMMAND(ID_METHOD_NEXT, OnMethodNext)
	ON_UPDATE_COMMAND_UI(ID_METHOD_NEXT, OnUpdateMethodNext)
	ON_COMMAND(ID_METHOD_PREVIOUS, OnMethodPrevious)
	ON_UPDATE_COMMAND_UI(ID_METHOD_PREVIOUS, OnUpdateMethodPrevious)
	ON_COMMAND(ID_METHOD_FIRST, OnMethodFirst)
	ON_UPDATE_COMMAND_UI(ID_METHOD_FIRST, OnUpdateMethodFirst)
	ON_COMMAND(ID_METHOD_LAST, OnMethodLast)
	ON_UPDATE_COMMAND_UI(ID_METHOD_LAST, OnUpdateMethodLast)
	ON_WM_CREATE()
	ON_COMMAND(ID_METHOD_SETSLIDE, OnMethodSetslide)
	ON_UPDATE_COMMAND_UI(ID_METHOD_SETSLIDE, OnUpdateMethodSetslide)
	ON_COMMAND(ID_PROPERTY_STARTSLIDE, OnPropertyStartslide)
	ON_COMMAND(ID_METHOD_COPYSLIDE, OnMethodCopyslide)
	ON_UPDATE_COMMAND_UI(ID_METHOD_COPYSLIDE, OnUpdateMethodCopyslide)
	ON_COMMAND(ID_METHOD_SAVESLIDE, OnMethodSaveslide)
	ON_UPDATE_COMMAND_UI(ID_METHOD_SAVESLIDE, OnUpdateMethodSaveslide)
	ON_COMMAND(ID_PROPERTY_ENABLEACCELERATORS, OnPropertyEnableaccelerators)
	ON_UPDATE_COMMAND_UI(ID_PROPERTY_ENABLEACCELERATORS, OnUpdatePropertyEnableaccelerators)
	ON_COMMAND(ID_PROPERTY_USESLIDEID, OnPropertyUseslideid)
	ON_UPDATE_COMMAND_UI(ID_PROPERTY_USESLIDEID, OnUpdatePropertyUseslideid)
	ON_COMMAND(ID_PROPERTY_SAVEFORMAT, OnPropertySaveformat)
	ON_UPDATE_COMMAND_UI(ID_PROPERTY_SAVEFORMAT, OnUpdatePropertySaveformat)
	ON_COMMAND(ID_CLASS_ID, OnClassId)
	ON_COMMAND(ID_REGISTRATION_PATH, OnRegistrationPath)
	ON_COMMAND(ID_METHOD_LOAD_FILE, OnMethodLoadFile)
	ON_COMMAND(ID_METHOD_ANIMATIONS, OnMethodAnimations)
	//}}AFX_MSG_MAP
	// Standard printing commands
	ON_COMMAND(ID_FILE_PRINT, CFormView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_DIRECT, CFormView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_PREVIEW, CFormView::OnFilePrintPreview)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTpowervcView construction/destruction

CTpowervcView::CTpowervcView()
	: CFormView(CTpowervcView::IDD)
{
	//{{AFX_DATA_INIT(CTpowervcView)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// TODO: add construction code here

}

CTpowervcView::~CTpowervcView()
{
}

void CTpowervcView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CTpowervcView)
	DDX_Control(pDX, IDC_TMPOWERCTRL1, m_Power);
	//}}AFX_DATA_MAP
}

BOOL CTpowervcView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	return CFormView::PreCreateWindow(cs);
}

void CTpowervcView::OnInitialUpdate()
{
	CFormView::OnInitialUpdate();

	if(IsWindow(m_Power.m_hWnd))
	{
		RECT rcWnd;
		GetClientRect(&rcWnd);
		m_Power.MoveWindow(&rcWnd);
	}

	//	Set the filename to display
	CTpowervcDoc* pDoc = GetDocument();
	m_Power.LoadFile(pDoc->GetPathName(), 1, FALSE, -1);
}

/////////////////////////////////////////////////////////////////////////////
// CTpowervcView printing

BOOL CTpowervcView::OnPreparePrinting(CPrintInfo* pInfo)
{
	// default preparation
	return DoPreparePrinting(pInfo);
}

void CTpowervcView::OnBeginPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: add extra initialization before printing
}

void CTpowervcView::OnEndPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: add cleanup after printing
}

void CTpowervcView::OnPrint(CDC* pDC, CPrintInfo* /*pInfo*/)
{
	// TODO: add customized printing code here
}

/////////////////////////////////////////////////////////////////////////////
// CTpowervcView diagnostics

#ifdef _DEBUG
void CTpowervcView::AssertValid() const
{
	CFormView::AssertValid();
}

void CTpowervcView::Dump(CDumpContext& dc) const
{
	CFormView::Dump(dc);
}

CTpowervcDoc* CTpowervcView::GetDocument() // non-debug version is inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CTpowervcDoc)));
	return (CTpowervcDoc*)m_pDocument;
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CTpowervcView message handlers

void CTpowervcView::OnSize(UINT nType, int cx, int cy) 
{
	CFormView::OnSize(nType, cx, cy);
	
	if(IsWindow(m_Power.m_hWnd))
	{
		RECT rcWnd;
		GetClientRect(&rcWnd);
		m_Power.MoveWindow(&rcWnd);
	}
}

void CTpowervcView::OnMethodGetppversion() 
{
	CString strVersion;
	strVersion.Format("Version: %s\nBuild: %s", m_Power.GetPPVersion(),
												m_Power.GetPPBuild());
	MessageBox(strVersion, "Power Point Version");	
}

void CTpowervcView::OnUpdateMethodGetppversion(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Power.IsInitialized());
}

void CTpowervcView::OnPropertySplitframecolor() 
{
	COLORREF crFrame;

	//	Translate the current background color
	OleTranslateColor(m_Power.GetSplitFrameColor(), NULL, &crFrame);

	CColorDialog Colors(crFrame);
	if(Colors.DoModal() == IDOK)
		m_Power.SetSplitFrameColor((OLE_COLOR)Colors.GetColor());
}

void CTpowervcView::OnUpdatePropertySplitframecolor(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Power.IsInitialized());
}

void CTpowervcView::OnPropertySplitframethickness() 
{
	CGetProp Prop;

	Prop.m_strName = "Split Frame Thickness";
	Prop.m_strValue.Format("%d", m_Power.GetSplitFrameThickness());

	if(Prop.DoModal() == IDOK)
		m_Power.SetSplitFrameThickness(atoi(Prop.m_strValue));
	
}

void CTpowervcView::OnUpdatePropertySplitframethickness(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Power.IsInitialized());
}

void CTpowervcView::OnPropertySplitscreen() 
{
	m_Power.SetSplitScreen(!m_Power.GetSplitScreen());	
}

void CTpowervcView::OnUpdatePropertySplitscreen(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Power.IsInitialized());
	pCmdUI->SetCheck(m_Power.GetSplitScreen());	
}

void CTpowervcView::OnPropertySyncviews() 
{
	m_Power.SetSyncViews(!m_Power.GetSyncViews());	
}

void CTpowervcView::OnUpdatePropertySyncviews(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Power.IsInitialized());
	pCmdUI->SetCheck(m_Power.GetSyncViews());	
}

void CTpowervcView::OnFileReload() 
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
		m_Power.SetLeftFile(szFilename);
	}
}

void CTpowervcView::OnUpdateFileReload(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Power.IsInitialized());
}

void CTpowervcView::OnMethodNext() 
{
	m_Power.Next(-1);
}

void CTpowervcView::OnUpdateMethodNext(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(TRUE);
	//pCmdUI->Enable(m_Power.IsInitialized() && (m_Power.GetCurrentSlide(-1) < m_Power.GetSlideCount(-1)));
}

void CTpowervcView::OnMethodPrevious() 
{
	m_Power.Previous(-1);
}

void CTpowervcView::OnUpdateMethodPrevious(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(TRUE);
	//pCmdUI->Enable(m_Power.IsInitialized() && (m_Power.GetCurrentSlide(-1) > 1));
}

void CTpowervcView::OnMethodFirst() 
{
	m_Power.First(-1);
}

void CTpowervcView::OnUpdateMethodFirst(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Power.IsInitialized() && (m_Power.GetCurrentSlide(-1) > 1));
}

void CTpowervcView::OnMethodLast() 
{
	m_Power.Last(-1);	
}

void CTpowervcView::OnUpdateMethodLast(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Power.IsInitialized() && (m_Power.GetCurrentSlide(-1) < m_Power.GetSlideCount(-1)));
}

BEGIN_EVENTSINK_MAP(CTpowervcView, CFormView)
    //{{AFX_EVENTSINK_MAP(CTpowervcView)
	ON_EVENT(CTpowervcView, IDC_TMPOWERCTRL1, 2 /* FileChanged */, OnFileChangedTmpowerctrl1, VTS_BSTR VTS_I2)
	ON_EVENT(CTpowervcView, IDC_TMPOWERCTRL1, 3 /* SlideChanged */, OnSlideChangedTmpowerctrl1, VTS_I4 VTS_I2)
	ON_EVENT(CTpowervcView, IDC_TMPOWERCTRL1, 4 /* ViewFocus */, OnViewFocusTmpowerctrl1, VTS_I2)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()

void CTpowervcView::OnFileChangedTmpowerctrl1(LPCTSTR lpszFilename, short sView) 
{
	if(m_pStatus && IsWindow(m_pStatus->m_hWnd))
	{
		m_pStatus->SetPaneText(STATUS_FILENAME_PANE, lpszFilename);
		m_pStatus->SetPaneText(STATUS_EVENT_PANE, "FileChange");
	}
}

void CTpowervcView::OnSlideChangedTmpowerctrl1(long lSlide, short sView) 
{
	CString M;
	int		iAnimations = m_Power.GetAnimationCount(-1);
	int		iAnimation = m_Power.GetAnimationIndex(-1);

	M.Format("Slide %ld of %ld - Animation %d of %d", lSlide, m_Power.GetSlideCount(-1), iAnimation, iAnimations);
	if(m_pStatus && IsWindow(m_pStatus->m_hWnd))
	{
		m_pStatus->SetPaneText(STATUS_SLIDE_PANE, M);
		m_pStatus->SetPaneText(STATUS_EVENT_PANE, "SlideChange");
	}
}

int CTpowervcView::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
	CChildFrame* pFrame;
	
	if (CFormView::OnCreate(lpCreateStruct) == -1)
		return -1;
	
	pFrame = (CChildFrame*)GetParent();
	if(pFrame)
		m_pStatus = &(pFrame->m_wndStatusBar);
	ASSERT(m_pStatus);
	
	return 0;
}

void CTpowervcView::OnMethodSetslide() 
{
	CGetProp Prop;

	Prop.m_strName = "Go To Slide: ";
	Prop.m_strValue.Format("%ld", m_Power.GetCurrentSlide(-1));

	if(Prop.DoModal() == IDOK)
		m_Power.SetSlide(-1, atol(Prop.m_strValue), m_Power.GetUseSlideId());
}

void CTpowervcView::OnUpdateMethodSetslide(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Power.IsInitialized());
}

void CTpowervcView::OnPropertyStartslide() 
{
	CGetProp Prop;

	Prop.m_strName = "Start Slide: ";
	Prop.m_strValue.Format("%ld", m_Power.GetStartSlide());

	if(Prop.DoModal() == IDOK)
		m_Power.SetStartSlide(atol(Prop.m_strValue));
}

void CTpowervcView::OnMethodCopyslide() 
{
	m_Power.CopySlide(-1);
}

void CTpowervcView::OnUpdateMethodCopyslide(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Power.IsInitialized());
}

void CTpowervcView::OnMethodSaveslide() 
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
	FileDlg.m_ofn.Flags |= OFN_LONGNAMES | OFN_NONETWORKBUTTON;
	if(FileDlg.DoModal() == IDOK)
	{
		m_Power.SaveSlide(szFilename, -1);
	}
}

void CTpowervcView::OnUpdateMethodSaveslide(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Power.IsInitialized());
}

void CTpowervcView::OnPropertyEnableaccelerators() 
{
	m_Power.SetEnableAccelerators(!m_Power.GetEnableAccelerators());	
}

void CTpowervcView::OnUpdatePropertyEnableaccelerators(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Power.IsInitialized());
	pCmdUI->SetCheck(m_Power.GetEnableAccelerators());	
}

void CTpowervcView::OnViewFocusTmpowerctrl1(short sView) 
{
	//	Reassign the focus if accelerators are disabled
	//if(m_Power.GetEnableAccelerators())
	//MessageBeep(MB_ICONEXCLAMATION);
		//::SetForegroundWindow(GetParent()->m_hWnd);
		//::SetActiveWindow(GetParent()->m_hWnd);
		::SetFocus(m_Power.m_hWnd);
	
}

void CTpowervcView::OnPropertyUseslideid() 
{
	m_Power.SetUseSlideId(!m_Power.GetUseSlideId());	
}

void CTpowervcView::OnUpdatePropertyUseslideid(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Power.IsInitialized());
	pCmdUI->SetCheck(m_Power.GetUseSlideId());	
}

void CTpowervcView::OnPropertySaveformat() 
{
	CFormat Format;

	Format.m_iFormat = m_Power.GetSaveFormat();
	if(Format.DoModal() == IDOK)
		m_Power.SetSaveFormat(Format.m_iFormat);	
}

void CTpowervcView::OnUpdatePropertySaveformat(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_Power.IsInitialized());
}

void CTpowervcView::OnClassId() 
{
	CString Class = m_Power.GetClassIdString();
	MessageBox(Class);	
}

void CTpowervcView::OnRegistrationPath() 
{
	CString Path = m_Power.GetRegisteredPath();
	MessageBox(Path);	
}

void CTpowervcView::OnMethodLoadFile() 
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
		m_Power.LoadFile(szFilename, 1, FALSE, -1);

	}
}

void CTpowervcView::OnMethodAnimations() 
{
	long lAnimations = m_Power.GetAnimationCount(-1);
	long lAnimation = m_Power.GetAnimationIndex(-1);
	CString strAnimations;
	strAnimations.Format("Index: %ld\nTotal: %ld", lAnimation, lAnimations);
	MessageBox(strAnimations);	
}
