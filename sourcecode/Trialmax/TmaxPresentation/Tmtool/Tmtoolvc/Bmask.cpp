// Bmask.cpp : implementation file
//

#include "stdafx.h"
#include "Tmtoolvc.h"
#include "Bmask.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CButtonMask dialog


CButtonMask::CButtonMask(CWnd* pParent /*=NULL*/)
	: CDialog(CButtonMask::IDD, pParent)
{
	//{{AFX_DATA_INIT(CButtonMask)
	m_strMask = _T("");
	//}}AFX_DATA_INIT
}


void CButtonMask::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CButtonMask)
	DDX_Text(pDX, IDC_EDIT1, m_strMask);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CButtonMask, CDialog)
	//{{AFX_MSG_MAP(CButtonMask)
		// NOTE: the ClassWizard will add message map macros here
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CButtonMask message handlers
