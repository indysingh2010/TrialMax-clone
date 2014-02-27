// Tmpdlg.cpp : implementation file
//

#include "stdafx.h"
#include "Tprintvc.h"
#include "Tmpdlg.h"
#include <tmprint.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CTemplateDialog dialog

CTemplateDialog::CTemplateDialog(CWnd* pParent /*=NULL*/)
	: CDialog(CTemplateDialog::IDD, pParent)
{
	//{{AFX_DATA_INIT(CTemplateDialog)
	m_strTemplate = _T("");
	//}}AFX_DATA_INIT
	for(int i = 0; i < MAX_TEMPLATES; i++)
		m_Templates[i].Empty();
	m_iTemplates = 0;
}


void CTemplateDialog::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CTemplateDialog)
	DDX_Control(pDX, IDC_TEMPLATES, m_ctrlTemplates);
	DDX_LBString(pDX, IDC_TEMPLATES, m_strTemplate);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CTemplateDialog, CDialog)
	//{{AFX_MSG_MAP(CTemplateDialog)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTemplateDialog message handlers

BOOL CTemplateDialog::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	for(int i = 0; i < m_iTemplates; i++)
	{
		if(!m_Templates[i].IsEmpty())
			m_ctrlTemplates.AddString(m_Templates[i]);
	}
	
	UpdateData(FALSE);

	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}
