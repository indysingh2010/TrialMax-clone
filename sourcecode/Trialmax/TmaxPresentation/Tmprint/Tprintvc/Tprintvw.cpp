// Tprintvw.cpp : implementation of the CTprintvcView class
//

#include "stdafx.h"
#include "Tprintvc.h"

#include "Printdoc.h"
#include "Tprintvw.h"
#include "Add.h"
#include "Addfrom.h"
#include "copies.h"
#include "invis.h"
#include "..\..\common\include\template.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

extern BOOL _bShowOptions;

/////////////////////////////////////////////////////////////////////////////
// CTprintvcView

IMPLEMENT_DYNCREATE(CTprintvcView, CFormView)

BEGIN_MESSAGE_MAP(CTprintvcView, CFormView)
	//{{AFX_MSG_MAP(CTprintvcView)
	ON_COMMAND(ID_QUEUE_ADD, OnQueueAdd)
	ON_COMMAND(ID_QUEUE_ADDFROMINI, OnQueueAddfromini)
	ON_COMMAND(ID_QUEUE_FLUSH, OnQueueFlush)
	ON_UPDATE_COMMAND_UI(ID_QUEUE_FLUSH, OnUpdateQueueFlush)
	ON_COMMAND(ID_QUEUE_GETCOUNT, OnQueueGetcount)
	ON_COMMAND(ID_QUEUE_PRINT, OnQueuePrint)
	ON_UPDATE_COMMAND_UI(ID_QUEUE_PRINT, OnUpdateQueuePrint)
	ON_COMMAND(ID_QUEUE_REFRESH, OnQueueRefresh)
	ON_COMMAND(ID_PROP_INCLUDEPAGETOTAL, OnPropIncludepagetotal)
	ON_UPDATE_COMMAND_UI(ID_PROP_INCLUDEPAGETOTAL, OnUpdatePropIncludepagetotal)
	ON_COMMAND(ID_PROP_INCLUDEPATH, OnPropIncludepath)
	ON_UPDATE_COMMAND_UI(ID_PROP_INCLUDEPATH, OnUpdatePropIncludepath)
	ON_COMMAND(ID_PROP_COLLATE, OnPropCollate)
	ON_UPDATE_COMMAND_UI(ID_PROP_COLLATE, OnUpdatePropCollate)
	ON_COMMAND(ID_PROP_COPIES, OnPropCopies)
	ON_COMMAND(ID_PROP_PRINTBARCODEGRAPHIC, OnPropPrintbarcodegraphic)
	ON_UPDATE_COMMAND_UI(ID_PROP_PRINTBARCODEGRAPHIC, OnUpdatePropPrintbarcodegraphic)
	ON_COMMAND(ID_PROP_PRINTBARCODETEXT, OnPropPrintbarcodetext)
	ON_UPDATE_COMMAND_UI(ID_PROP_PRINTBARCODETEXT, OnUpdatePropPrintbarcodetext)
	ON_COMMAND(ID_PROP_PRINTCELLBORDER, OnPropPrintcellborder)
	ON_UPDATE_COMMAND_UI(ID_PROP_PRINTCELLBORDER, OnUpdatePropPrintcellborder)
	ON_COMMAND(ID_PROP_PRINTDEPONENT, OnPropPrintdeponent)
	ON_UPDATE_COMMAND_UI(ID_PROP_PRINTDEPONENT, OnUpdatePropPrintdeponent)
	ON_COMMAND(ID_PROP_PRINTFILENAME, OnPropPrintfilename)
	ON_UPDATE_COMMAND_UI(ID_PROP_PRINTFILENAME, OnUpdatePropPrintfilename)
	ON_COMMAND(ID_PROP_PRINTIMAGE, OnPropPrintimage)
	ON_UPDATE_COMMAND_UI(ID_PROP_PRINTIMAGE, OnUpdatePropPrintimage)
	ON_COMMAND(ID_PROP_PRINTNAME, OnPropPrintname)
	ON_UPDATE_COMMAND_UI(ID_PROP_PRINTNAME, OnUpdatePropPrintname)
	ON_COMMAND(ID_PROP_PRINTPAGENUMBER, OnPropPrintpagenumber)
	ON_UPDATE_COMMAND_UI(ID_PROP_PRINTPAGENUMBER, OnUpdatePropPrintpagenumber)
	ON_COMMAND(ID_PROP_TEMPLATENAME, OnPropTemplatename)
	ON_COMMAND(ID_PROP_PRINTER, OnPropPrinter)
	ON_COMMAND(ID_GET_PRINTER, OnGetPrinter)
	ON_COMMAND(ID_PROP_FORCENEWPAGE, OnPropForcenewpage)
	ON_UPDATE_COMMAND_UI(ID_PROP_FORCENEWPAGE, OnUpdatePropForcenewpage)
	ON_COMMAND(ID_PROP_USESLIDEIDS, OnPropUseslideids)
	ON_UPDATE_COMMAND_UI(ID_PROP_USESLIDEIDS, OnUpdatePropUseslideids)
	ON_COMMAND(ID_PROP_SHOWOPTIONS, OnPropShowoptions)
	ON_UPDATE_COMMAND_UI(ID_PROP_SHOWOPTIONS, OnUpdatePropShowoptions)
	ON_COMMAND(ID_INVISIBLE, OnInvisible)
	ON_COMMAND(ID_SELECT_PRINTER, OnSelectPrinter)
	ON_COMMAND(ID_CLASS_ID, OnClassId)
	ON_COMMAND(ID_REGISTERED_PATH, OnRegisteredPath)
	//}}AFX_MSG_MAP
	// Standard printing commands
	ON_COMMAND(ID_FILE_PRINT, CFormView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_DIRECT, CFormView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_PREVIEW, CFormView::OnFilePrintPreview)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTprintvcView construction/destruction

CTprintvcView::CTprintvcView()
	: CFormView(CTprintvcView::IDD)
{
	//{{AFX_DATA_INIT(CTprintvcView)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// TODO: add construction code here
	m_pTemplateDlg = 0;
	m_pPrinterDlg = 0;
}

CTprintvcView::~CTprintvcView()
{
}

void CTprintvcView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CTprintvcView)
	DDX_Control(pDX, IDC_TMPRINTCTRL1, m_TMPrint);
	//}}AFX_DATA_MAP
}

BOOL CTprintvcView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	return CFormView::PreCreateWindow(cs);
}

void CTprintvcView::OnInitialUpdate()
{
	CFormView::OnInitialUpdate();
	ResizeParentToFit(FALSE);
	m_Ini.SetFileSpec(m_TMPrint.GetIniFile());
	m_Ini.SetSection(m_TMPrint.GetIniSection());
	m_TMPrint.SetShowOptions(_bShowOptions);
	m_TMPrint.Initialize();

	//	This is for test mode operation
	AddToQueue(m_TMPrint.GetIniFile(), "TREATMENTS");
}

/////////////////////////////////////////////////////////////////////////////
// CTprintvcView printing

BOOL CTprintvcView::OnPreparePrinting(CPrintInfo* pInfo)
{
	// default preparation
	return DoPreparePrinting(pInfo);
}

void CTprintvcView::OnBeginPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: add extra initialization before printing
}

void CTprintvcView::OnEndPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: add cleanup after printing
}

void CTprintvcView::OnPrint(CDC* pDC, CPrintInfo* /*pInfo*/)
{
	// TODO: add customized printing code here
}

/////////////////////////////////////////////////////////////////////////////
// CTprintvcView diagnostics

#ifdef _DEBUG
void CTprintvcView::AssertValid() const
{
	CFormView::AssertValid();
}

void CTprintvcView::Dump(CDumpContext& dc) const
{
	CFormView::Dump(dc);
}

CTprintvcDoc* CTprintvcView::GetDocument() // non-debug version is inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CTprintvcDoc)));
	return (CTprintvcDoc*)m_pDocument;
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CTprintvcView message handlers

void CTprintvcView::OnQueueAdd() 
{
	CAdd Add;

	if(Add.DoModal() == IDOK)
		m_TMPrint.Add(Add.m_strString);
}

void CTprintvcView::OnQueueAddfromini() 
{
	CString	Msg;
	CAddFromIni	AddIni;
	int	i = 1;
	char szString[512];
	
	AddIni.m_strFilename = m_Ini.strFileSpec;
	AddIni.m_strSection = m_Ini.strSection;
	AddIni.m_bFlush = TRUE;
	
	if(AddIni.DoModal() == IDOK)
	{
		if(!m_Ini.Open(AddIni.m_strFilename, AddIni.m_strSection))
		{
			Msg.Format("Unable to open %s to read print specifications", m_Ini.strFileSpec);
			MessageBox(Msg, "Error");
			return;
		}

		//	Flush the queue if requested
		if(AddIni.m_bFlush)
			m_TMPrint.Clear();

		//	Read each specification defined in the file
		while(1)
		{
			m_Ini.ReadString(i, szString, sizeof(szString));

			//	Have we run out?
			if(lstrlen(szString) == 0)
				break;

			m_TMPrint.Add(szString);

			//	Next Line
			i++;
		}
		 
	}
	
}

void CTprintvcView::OnQueueFlush() 
{
	m_TMPrint.Clear();	
}

void CTprintvcView::OnUpdateQueueFlush(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_TMPrint.GetQueueCount() > 0);
}

void CTprintvcView::OnQueueGetcount() 
{
	CString strMsg;

	strMsg.Format("Current Count = %ld", m_TMPrint.GetQueueCount());
	MessageBox(strMsg, "TMPrint");	
}

void CTprintvcView::OnQueuePrint() 
{
	m_TMPrint.Print();	
}

void CTprintvcView::OnUpdateQueuePrint(CCmdUI* pCmdUI) 
{
	pCmdUI->Enable(m_TMPrint.GetQueueCount() > 0);	
}

void CTprintvcView::OnQueueRefresh() 
{
	m_TMPrint.RefreshTemplates();	
}

void CTprintvcView::OnPropIncludepagetotal() 
{
	m_TMPrint.SetIncludePageTotal(!m_TMPrint.GetIncludePageTotal());
}

void CTprintvcView::OnUpdatePropIncludepagetotal(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMPrint.GetIncludePageTotal());
}

void CTprintvcView::OnPropIncludepath() 
{
	m_TMPrint.SetIncludePathInFileName(!m_TMPrint.GetIncludePathInFileName());
}

void CTprintvcView::OnUpdatePropIncludepath(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMPrint.GetIncludePathInFileName());
}

void CTprintvcView::OnPropCollate() 
{
	m_TMPrint.SetCollate(!m_TMPrint.GetCollate());
}

void CTprintvcView::OnUpdatePropCollate(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMPrint.GetCollate());
}

void CTprintvcView::OnPropCopies() 
{
	CCopies Dialog;

	Dialog.m_sCopies = m_TMPrint.GetCopies();
	if(Dialog.DoModal() == IDOK)
		m_TMPrint.SetCopies(Dialog.m_sCopies);	
}

void CTprintvcView::OnPropPrintbarcodegraphic() 
{
	m_TMPrint.SetPrintBarcodeGraphic(!m_TMPrint.GetPrintBarcodeGraphic());
}

void CTprintvcView::OnUpdatePropPrintbarcodegraphic(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMPrint.GetPrintBarcodeGraphic());
}

void CTprintvcView::OnPropPrintbarcodetext() 
{
	m_TMPrint.SetPrintBarcodeText(!m_TMPrint.GetPrintBarcodeText());
}

void CTprintvcView::OnUpdatePropPrintbarcodetext(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMPrint.GetPrintBarcodeText());
}

void CTprintvcView::OnPropPrintcellborder() 
{
	m_TMPrint.SetPrintCellBorder(!m_TMPrint.GetPrintCellBorder());
}

void CTprintvcView::OnUpdatePropPrintcellborder(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMPrint.GetPrintCellBorder());
}

void CTprintvcView::OnPropPrintdeponent() 
{
	m_TMPrint.SetPrintDeponent(!m_TMPrint.GetPrintDeponent());
}

void CTprintvcView::OnUpdatePropPrintdeponent(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMPrint.GetPrintDeponent());
}

void CTprintvcView::OnPropPrintfilename() 
{
	m_TMPrint.SetPrintFileName(!m_TMPrint.GetPrintFileName());
}

void CTprintvcView::OnUpdatePropPrintfilename(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMPrint.GetPrintFileName());
}

void CTprintvcView::OnPropPrintimage() 
{
	m_TMPrint.SetPrintImage(!m_TMPrint.GetPrintImage());
}

void CTprintvcView::OnUpdatePropPrintimage(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMPrint.GetPrintImage());
}

void CTprintvcView::OnPropPrintname() 
{
	m_TMPrint.SetPrintName(!m_TMPrint.GetPrintName());
}

void CTprintvcView::OnUpdatePropPrintname(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMPrint.GetPrintName());
}

void CTprintvcView::OnPropPrintpagenumber() 
{
	m_TMPrint.SetPrintPageNumber(!m_TMPrint.GetPrintPageNumber());
}

void CTprintvcView::OnUpdatePropPrintpagenumber(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMPrint.GetPrintPageNumber());
}

void CTprintvcView::OnPropTemplatename() 
{
	ASSERT(m_pTemplateDlg == 0);

	m_pTemplateDlg = new CTemplateDialog();

	m_pTemplateDlg->m_strTemplate = m_TMPrint.GetTemplateName();
	m_TMPrint.EnumerateTemplates();

	if(m_pTemplateDlg->DoModal() == IDOK)
	{
		m_TMPrint.SetTemplateName(m_pTemplateDlg->m_strTemplate);
	}

	delete m_pTemplateDlg;
	m_pTemplateDlg = 0;	
}

BEGIN_EVENTSINK_MAP(CTprintvcView, CFormView)
    //{{AFX_EVENTSINK_MAP(CTprintvcView)
	ON_EVENT(CTprintvcView, IDC_TMPRINTCTRL1, 1 /* FirstTemplate */, OnFirstTemplateTmprintctrl1, VTS_BSTR)
	ON_EVENT(CTprintvcView, IDC_TMPRINTCTRL1, 2 /* NextTemplate */, OnNextTemplateTmprintctrl1, VTS_BSTR)
	ON_EVENT(CTprintvcView, IDC_TMPRINTCTRL1, 3 /* FirstPrinter */, OnFirstPrinterTmprintctrl1, VTS_BSTR)
	ON_EVENT(CTprintvcView, IDC_TMPRINTCTRL1, 4 /* NextPrinter */, OnNextPrinterTmprintctrl1, VTS_BSTR)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()

void CTprintvcView::OnFirstTemplateTmprintctrl1(LPCTSTR lpszTemplate) 
{
	if(m_pTemplateDlg)
	{
		m_pTemplateDlg->m_Templates[0] = lpszTemplate;
		m_pTemplateDlg->m_iTemplates = 1;
	}	
}

void CTprintvcView::OnNextTemplateTmprintctrl1(LPCTSTR lpszTemplate) 
{
	if(m_pTemplateDlg && m_pTemplateDlg->m_iTemplates < MAX_TEMPLATES)
	{
		m_pTemplateDlg->m_Templates[m_pTemplateDlg->m_iTemplates] = lpszTemplate;
		m_pTemplateDlg->m_iTemplates++;
	}	
}

void CTprintvcView::OnFirstPrinterTmprintctrl1(LPCTSTR lpszPrinter) 
{
	if(m_pPrinterDlg)
	{
		m_pPrinterDlg->m_Printers[0] = lpszPrinter;
		m_pPrinterDlg->m_iPrinters = 1;
	}	
}

void CTprintvcView::OnNextPrinterTmprintctrl1(LPCTSTR lpszPrinter) 
{
	if(m_pPrinterDlg && m_pPrinterDlg->m_iPrinters < MAX_PRINTERS)
	{
		m_pPrinterDlg->m_Printers[m_pPrinterDlg->m_iPrinters] = lpszPrinter;
		m_pPrinterDlg->m_iPrinters++;
	}	
}

void CTprintvcView::OnPropPrinter() 
{
	ASSERT(m_pPrinterDlg == 0);

	m_pPrinterDlg = new CPrinterDialog();

	m_pPrinterDlg->m_strPrinter = m_TMPrint.GetPrinter();
	m_TMPrint.EnumeratePrinters();

	if(m_pPrinterDlg->DoModal() == IDOK)
	{
		m_TMPrint.SetPrinter(m_pPrinterDlg->m_strPrinter);
	}

	delete m_pPrinterDlg;
	m_pPrinterDlg = 0;	
}

void CTprintvcView::OnGetPrinter() 
{
	CString strPrinter = m_TMPrint.GetDefaultPrinter();
	MessageBox(strPrinter);	
}

void CTprintvcView::OnPropForcenewpage() 
{
	m_TMPrint.SetForceNewPage(!m_TMPrint.GetForceNewPage());
}

void CTprintvcView::OnUpdatePropForcenewpage(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMPrint.GetForceNewPage());	
}

void CTprintvcView::OnPropUseslideids() 
{
	m_TMPrint.SetUseSlideIDs(!m_TMPrint.GetUseSlideIDs());
}

void CTprintvcView::OnUpdatePropUseslideids(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMPrint.GetUseSlideIDs());	
}

void CTprintvcView::OnPropShowoptions() 
{
	m_TMPrint.SetShowOptions(!m_TMPrint.GetShowOptions());
}

void CTprintvcView::OnUpdatePropShowoptions(CCmdUI* pCmdUI) 
{
	pCmdUI->SetCheck(m_TMPrint.GetShowOptions());	
}

void CTprintvcView::AddToQueue(LPCSTR lpFilename, LPCSTR lpSection) 
{
	int	i = 1;
	char szString[512];
	
	if(!m_Ini.Open(lpFilename, lpSection))
		return;

	//	Read each specification defined in the file
	while(1)
	{
		m_Ini.ReadString(i, szString, sizeof(szString));

		//	Have we run out?
		if(lstrlen(szString) == 0)
			break;

		m_TMPrint.Add(szString);

		//	Next Line
		i++;
	}
	
}


void CTprintvcView::OnInvisible() 
{
	CInvisible Invisible;
	Invisible.DoModal();	
}

void CTprintvcView::OnSelectPrinter() 
{
	m_TMPrint.SelectPrinter();
}

void CTprintvcView::OnClassId() 
{
	CString Class = m_TMPrint.GetClassIdString();
	MessageBox(Class);	
}

void CTprintvcView::OnRegisteredPath() 
{
	CString Path = m_TMPrint.GetRegisteredPath();
	MessageBox(Path);	
}
