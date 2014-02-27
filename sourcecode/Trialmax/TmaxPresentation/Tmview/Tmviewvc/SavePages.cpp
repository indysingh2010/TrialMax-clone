// SavePages.cpp : implementation file
//

#include "stdafx.h"
#include "Tmviewvc.h"
#include "SavePages.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CSavePages dialog


CSavePages::CSavePages(CWnd* pParent /*=NULL*/)
	: CDialog(CSavePages::IDD, pParent)
{
	//{{AFX_DATA_INIT(CSavePages)
	m_strFolder = _T("");
	m_strPrefix = _T("");
	//}}AFX_DATA_INIT
}


void CSavePages::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CSavePages)
	DDX_Text(pDX, IDC_FOLDER, m_strFolder);
	DDX_Text(pDX, IDC_PREFIX, m_strPrefix);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CSavePages, CDialog)
	//{{AFX_MSG_MAP(CSavePages)
		// NOTE: the ClassWizard will add message map macros here
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CSavePages message handlers
