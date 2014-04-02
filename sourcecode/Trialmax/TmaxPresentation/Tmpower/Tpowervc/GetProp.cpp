// GetProp.cpp : implementation file
//

#include "stdafx.h"
#include "Tpowervc.h"
#include "GetProp.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CGetProp dialog


CGetProp::CGetProp(CWnd* pParent /*=NULL*/)
	: CDialog(CGetProp::IDD, pParent)
{
	//{{AFX_DATA_INIT(CGetProp)
	m_strName = _T("");
	m_strValue = _T("");
	//}}AFX_DATA_INIT
}


void CGetProp::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CGetProp)
	DDX_Text(pDX, IDC_NAME, m_strName);
	DDX_Text(pDX, IDC_VALUE, m_strValue);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CGetProp, CDialog)
	//{{AFX_MSG_MAP(CGetProp)
		// NOTE: the ClassWizard will add message map macros here
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CGetProp message handlers
