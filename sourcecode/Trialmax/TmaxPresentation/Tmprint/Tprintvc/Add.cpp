// Add.cpp : implementation file
//

#include "stdafx.h"
#include "Tprintvc.h"
#include "Add.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CAdd dialog


CAdd::CAdd(CWnd* pParent /*=NULL*/)
	: CDialog(CAdd::IDD, pParent)
{
	//{{AFX_DATA_INIT(CAdd)
	m_strString = _T("");
	//}}AFX_DATA_INIT
}


void CAdd::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAdd)
	DDX_Text(pDX, IDC_STRING, m_strString);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CAdd, CDialog)
	//{{AFX_MSG_MAP(CAdd)
		// NOTE: the ClassWizard will add message map macros here
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CAdd message handlers
