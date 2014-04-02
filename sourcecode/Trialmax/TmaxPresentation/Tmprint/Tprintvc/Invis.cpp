// Invis.cpp : implementation file
//

#include "stdafx.h"
#include "Tprintvc.h"
#include "Invis.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CInvisible dialog


CInvisible::CInvisible(CWnd* pParent /*=NULL*/)
	: CDialog(CInvisible::IDD, pParent)
{
	//{{AFX_DATA_INIT(CInvisible)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
}


void CInvisible::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CInvisible)
	DDX_Control(pDX, IDC_TMPRINT, m_TMPrint);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CInvisible, CDialog)
	//{{AFX_MSG_MAP(CInvisible)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CInvisible message handlers

BOOL CInvisible::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	m_TMPrint.SetIniFile("tmprint.h");
	m_TMPrint.Initialize();
	
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}
