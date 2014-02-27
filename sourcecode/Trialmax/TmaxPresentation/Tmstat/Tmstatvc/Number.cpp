// Number.cpp : implementation file
//

#include "stdafx.h"
#include "Tmstatvc.h"
#include "Number.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// GetNumber dialog


GetNumber::GetNumber(CWnd* pParent /*=NULL*/)
	: CDialog(GetNumber::IDD, pParent)
{
	//{{AFX_DATA_INIT(GetNumber)
	m_strLabel = _T("");
	m_sNumber = 0;
	//}}AFX_DATA_INIT
}


void GetNumber::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(GetNumber)
	DDX_Text(pDX, IDC_LABEL, m_strLabel);
	DDX_Text(pDX, IDC_NUMBER, m_sNumber);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(GetNumber, CDialog)
	//{{AFX_MSG_MAP(GetNumber)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

