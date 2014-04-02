// Gnumber.cpp : implementation file
//

#include "stdafx.h"
#include "Tmtextvc.h"
#include "Gnumber.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CGetNumber dialog


CGetNumber::CGetNumber(CWnd* pParent /*=NULL*/)
	: CDialog(CGetNumber::IDD, pParent)
{
	//{{AFX_DATA_INIT(CGetNumber)
	m_strNumber = _T("");
	m_strLabel = _T("");
	//}}AFX_DATA_INIT
}


void CGetNumber::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CGetNumber)
	DDX_Text(pDX, IDC_NUMBER, m_strNumber);
	DDX_Text(pDX, IDC_LABEL, m_strLabel);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CGetNumber, CDialog)
	//{{AFX_MSG_MAP(CGetNumber)
		// NOTE: the ClassWizard will add message map macros here
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CGetNumber message handlers
