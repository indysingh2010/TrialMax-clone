// Copies.cpp : implementation file
//

#include "stdafx.h"
#include "Tprintvc.h"
#include "Copies.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CCopies dialog


CCopies::CCopies(CWnd* pParent /*=NULL*/)
	: CDialog(CCopies::IDD, pParent)
{
	//{{AFX_DATA_INIT(CCopies)
	m_sCopies = 0;
	//}}AFX_DATA_INIT
}


void CCopies::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CCopies)
	DDX_Text(pDX, IDC_COPIES, m_sCopies);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CCopies, CDialog)
	//{{AFX_MSG_MAP(CCopies)
		// NOTE: the ClassWizard will add message map macros here
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CCopies message handlers
