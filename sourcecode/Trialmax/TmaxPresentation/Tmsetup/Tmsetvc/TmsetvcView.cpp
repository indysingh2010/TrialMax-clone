// TmsetvcView.cpp : implementation of the CTmsetvcView class
//

#include "stdafx.h"
#include "Tmsetvc.h"

#include "TmsetvcDoc.h"
#include "TmsetvcView.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CTmsetvcView

IMPLEMENT_DYNCREATE(CTmsetvcView, CFormView)

BEGIN_MESSAGE_MAP(CTmsetvcView, CFormView)
	//{{AFX_MSG_MAP(CTmsetvcView)
	ON_COMMAND(ID_PROPERTY_ABOUTPAGE, OnPropertyAboutpage)
	ON_COMMAND(ID_PROPERTY_DATABASEPAGE, OnPropertyDatabasepage)
	ON_COMMAND(ID_PROPERTY_DIAGNOSTICPAGE, OnPropertyDiagnosticpage)
	ON_COMMAND(ID_PROPERTY_DIRECTXPAGE, OnPropertyDirectxpage)
	ON_COMMAND(ID_PROPERTY_GRAPHICSPAGE, OnPropertyGraphicspage)
	ON_COMMAND(ID_PROPERTY_SYSTEMPAGE, OnPropertySystempage)
	ON_COMMAND(ID_PROPERTY_TEXTPAGE, OnPropertyTextpage)
	ON_COMMAND(ID_PROPERTY_VIDEOPAGE, OnPropertyVideopage)
	ON_UPDATE_COMMAND_UI(ID_PROPERTY_ABOUTPAGE, OnUpdatePropertyAboutpage)
	ON_UPDATE_COMMAND_UI(ID_PROPERTY_DATABASEPAGE, OnUpdatePropertyDatabasepage)
	ON_UPDATE_COMMAND_UI(ID_PROPERTY_DIAGNOSTICPAGE, OnUpdatePropertyDiagnosticpage)
	ON_UPDATE_COMMAND_UI(ID_PROPERTY_DIRECTXPAGE, OnUpdatePropertyDirectxpage)
	ON_UPDATE_COMMAND_UI(ID_PROPERTY_GRAPHICSPAGE, OnUpdatePropertyGraphicspage)
	ON_UPDATE_COMMAND_UI(ID_PROPERTY_SYSTEMPAGE, OnUpdatePropertySystempage)
	ON_UPDATE_COMMAND_UI(ID_PROPERTY_TEXTPAGE, OnUpdatePropertyTextpage)
	ON_UPDATE_COMMAND_UI(ID_PROPERTY_VIDEOPAGE, OnUpdatePropertyVideopage)
	ON_COMMAND(ID_METHOD_SAVE, OnMethodSave)
	ON_BN_CLICKED(IDC_SAVE, OnSave)
	ON_COMMAND(ID_VIEW_CLASS_ID, OnViewClassId)
	ON_COMMAND(ID_VIEW_REGISTERED_PATH, OnViewRegisteredPath)
	ON_COMMAND(ID_ENUM_VERSIONS, OnEnumVersions)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTmsetvcView construction/destruction

CTmsetvcView::CTmsetvcView()
	: CFormView(CTmsetvcView::IDD)
{
	//{{AFX_DATA_INIT(CTmsetvcView)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// TODO: add construction code here

}

CTmsetvcView::~CTmsetvcView()
{
}

void CTmsetvcView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CTmsetvcView)
	DDX_Control(pDX, IDC_CTMSETUPCTRL1, m_TMSetup);
	//}}AFX_DATA_MAP
}

BOOL CTmsetvcView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	return CFormView::PreCreateWindow(cs);
}

void CTmsetvcView::OnInitialUpdate()
{
	CFormView::OnInitialUpdate();
	ResizeParentToFit(FALSE);

}

/////////////////////////////////////////////////////////////////////////////
// CTmsetvcView diagnostics

#ifdef _DEBUG
void CTmsetvcView::AssertValid() const
{
	CFormView::AssertValid();
}

void CTmsetvcView::Dump(CDumpContext& dc) const
{
	CFormView::Dump(dc);
}

CTmsetvcDoc* CTmsetvcView::GetDocument() // non-debug version is inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CTmsetvcDoc)));
	return (CTmsetvcDoc*)m_pDocument;
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CTmsetvcView message handlers

void CTmsetvcView::OnPropertyAboutpage() 
{
	m_TMSetup.SetAboutPage(!m_TMSetup.GetAboutPage());	
}

void CTmsetvcView::OnPropertyDatabasepage() 
{
	m_TMSetup.SetDatabasePage(!m_TMSetup.GetDatabasePage());	
}

void CTmsetvcView::OnPropertyDiagnosticpage() 
{
	m_TMSetup.SetDiagnosticPage(!m_TMSetup.GetDiagnosticPage());	
}

void CTmsetvcView::OnPropertyDirectxpage() 
{
	m_TMSetup.SetDirectXPage(!m_TMSetup.GetDirectXPage());	
}

void CTmsetvcView::OnPropertyGraphicspage() 
{
	m_TMSetup.SetGraphicsPage(!m_TMSetup.GetGraphicsPage());	
}

void CTmsetvcView::OnPropertySystempage() 
{
	m_TMSetup.SetSystemPage(!m_TMSetup.GetSystemPage());	
}

void CTmsetvcView::OnPropertyTextpage() 
{
	m_TMSetup.SetTextPage(!m_TMSetup.GetTextPage());	
}

void CTmsetvcView::OnPropertyVideopage() 
{
	m_TMSetup.SetVideoPage(!m_TMSetup.GetVideoPage());	
}



void CTmsetvcView::OnUpdatePropertyAboutpage(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMSetup.GetAboutPage());	
}

void CTmsetvcView::OnUpdatePropertyDatabasepage(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMSetup.GetDatabasePage());	
}

void CTmsetvcView::OnUpdatePropertyDiagnosticpage(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMSetup.GetDiagnosticPage());	
}

void CTmsetvcView::OnUpdatePropertyDirectxpage(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMSetup.GetDirectXPage());	
}

void CTmsetvcView::OnUpdatePropertyGraphicspage(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMSetup.GetGraphicsPage());	
}

void CTmsetvcView::OnUpdatePropertySystempage(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMSetup.GetSystemPage());	
}

void CTmsetvcView::OnUpdatePropertyTextpage(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMSetup.GetTextPage());	
}

void CTmsetvcView::OnUpdatePropertyVideopage(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMSetup.GetVideoPage());	
}

void CTmsetvcView::OnMethodSave() 
{
	m_TMSetup.Save();	
}

void CTmsetvcView::OnSave() 
{
	m_TMSetup.Save();	
}

void CTmsetvcView::OnViewClassId() 
{
	CString Class = m_TMSetup.GetClassIdString();
	MessageBox(Class);	
}

void CTmsetvcView::OnViewRegisteredPath() 
{
	CString Path = m_TMSetup.GetRegisteredPath();
	MessageBox(Path);	
}

void CTmsetvcView::OnEnumVersions() 
{
	m_TMSetup.EnumAxVersions();	
}

BEGIN_EVENTSINK_MAP(CTmsetvcView, CFormView)
    //{{AFX_EVENTSINK_MAP(CTmsetvcView)
	ON_EVENT(CTmsetvcView, IDC_CTMSETUPCTRL1, 3 /* AxVersion */, OnAxVersionCtmsetupctrl1, VTS_BSTR VTS_BSTR VTS_I2 VTS_I2 VTS_I2 VTS_I2 VTS_BSTR VTS_BSTR VTS_BSTR VTS_BSTR VTS_BSTR)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()

void CTmsetvcView::OnAxVersionCtmsetupctrl1(LPCTSTR lpszName, LPCTSTR lpszDescription, short sMajorVer, short sMinorVer, short sQEF, short sBuild, LPCTSTR lpszShortText, LPCTSTR lpszLongText, LPCTSTR lpszBuildDate, LPCTSTR lpszClsId, LPCTSTR lpszPath) 
{
	MessageBox(lpszDescription, lpszLongText, MB_OK);
	
}
