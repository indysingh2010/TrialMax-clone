// Database.cpp : implementation file
//

#include "stdafx.h"
#include "Tmaxdemo.h"
#include "Database.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CConfirmDatabase dialog


CConfirmDatabase::CConfirmDatabase(CWnd* pParent /*=NULL*/)
	: CDialog(CConfirmDatabase::IDD, pParent)
{
	//{{AFX_DATA_INIT(CConfirmDatabase)
	m_strDrive = _T("");
	m_strComment = _T("");
	//}}AFX_DATA_INIT
	m_bShowCancel = FALSE;
}


void CConfirmDatabase::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CConfirmDatabase)
	DDX_Control(pDX, IDCANCEL, m_crtlCancel);
	DDX_Text(pDX, IDC_DRIVE, m_strDrive);
	DDV_MaxChars(pDX, m_strDrive, 1);
	DDX_Text(pDX, IDC_COMMENT, m_strComment);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CConfirmDatabase, CDialog)
	//{{AFX_MSG_MAP(CConfirmDatabase)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CConfirmDatabase message handlers

BOOL CConfirmDatabase::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	m_crtlCancel.ShowWindow(m_bShowCancel);
	BringWindowToTop();

	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}
