// GetLine.cpp : implementation file
//

#include <stdafx.h>
#include "tmtextvc.h"
#include "getline.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CGetLine dialog


CGetLine::CGetLine(CWnd* pParent /*=NULL*/)
	: CDialog(CGetLine::IDD, pParent)
{
	//{{AFX_DATA_INIT(CGetLine)
	m_lDesignation = 0;
	m_sLine = 0;
	m_sPage = 0;
	//}}AFX_DATA_INIT
}


void CGetLine::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CGetLine)
	DDX_Text(pDX, IDC_DESIGNATION, m_lDesignation);
	DDX_Text(pDX, IDC_LINE, m_sLine);
	DDX_Text(pDX, IDC_PAGE, m_sPage);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CGetLine, CDialog)
	//{{AFX_MSG_MAP(CGetLine)
		// NOTE: the ClassWizard will add message map macros here
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CGetLine message handlers
