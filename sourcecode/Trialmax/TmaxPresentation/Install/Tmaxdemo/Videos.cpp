// Videos.cpp : implementation file
//

#include "stdafx.h"
#include "Tmaxdemo.h"
#include "Videos.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CConfirmVideos dialog


CConfirmVideos::CConfirmVideos(CWnd* pParent /*=NULL*/)
	: CDialog(CConfirmVideos::IDD, pParent)
{
	//{{AFX_DATA_INIT(CConfirmVideos)
	m_strDrive = _T("");
	m_strComment = _T("");
	//}}AFX_DATA_INIT
}


void CConfirmVideos::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CConfirmVideos)
	DDX_Text(pDX, IDC_VIDEODRIVE, m_strDrive);
	DDV_MaxChars(pDX, m_strDrive, 1);
	DDX_Text(pDX, IDC_COMMENT, m_strComment);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CConfirmVideos, CDialog)
	//{{AFX_MSG_MAP(CConfirmVideos)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CConfirmVideos message handlers

BOOL CConfirmVideos::OnInitDialog() 
{
	CDialog::OnInitDialog();
	BringWindowToTop();	
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}
