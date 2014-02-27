// Prtdlg.cpp : implementation file
//

#include "stdafx.h"
#include "Tprintvc.h"
#include "Prtdlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CPrinterDialog dialog


CPrinterDialog::CPrinterDialog(CWnd* pParent /*=NULL*/)
	: CDialog(CPrinterDialog::IDD, pParent)
{
	//{{AFX_DATA_INIT(CPrinterDialog)
	m_strPrinter = _T("");
	//}}AFX_DATA_INIT
	for(int i = 0; i < MAX_PRINTERS; i++)
		m_Printers[i].Empty();
	m_iPrinters = 0;
}


void CPrinterDialog::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPrinterDialog)
	DDX_Control(pDX, IDC_PRINTERS, m_ctrlPrinters);
	DDX_LBString(pDX, IDC_PRINTERS, m_strPrinter);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CPrinterDialog, CDialog)
	//{{AFX_MSG_MAP(CPrinterDialog)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CPrinterDialog message handlers

BOOL CPrinterDialog::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	for(int i = 0; i < m_iPrinters; i++)
	{
		if(!m_Printers[i].IsEmpty())
			m_ctrlPrinters.AddString(m_Printers[i]);
	}
	
	UpdateData(FALSE);

	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}
