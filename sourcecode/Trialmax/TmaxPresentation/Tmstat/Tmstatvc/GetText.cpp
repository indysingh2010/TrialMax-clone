// GetText.cpp : implementation file
//

#include "stdafx.h"
#include "Tmstatvc.h"
#include "GetText.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CGetText dialog


CGetText::CGetText(CWnd* pParent /*=NULL*/)
	: CDialog(CGetText::IDD, pParent)
{
	//{{AFX_DATA_INIT(CGetText)
	m_strText = _T("");
	//}}AFX_DATA_INIT
}


void CGetText::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CGetText)
	DDX_Text(pDX, IDC_TEXT, m_strText);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CGetText, CDialog)
	//{{AFX_MSG_MAP(CGetText)
		// NOTE: the ClassWizard will add message map macros here
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CGetText message handlers
