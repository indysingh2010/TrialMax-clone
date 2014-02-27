// Addfrom.cpp : implementation file
//

#include "stdafx.h"
#include "Tprintvc.h"
#include "Addfrom.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CAddFromIni dialog


CAddFromIni::CAddFromIni(CWnd* pParent /*=NULL*/)
	: CDialog(CAddFromIni::IDD, pParent)
{
	//{{AFX_DATA_INIT(CAddFromIni)
	m_strFilename = _T("");
	m_strSection = _T("");
	m_bFlush = FALSE;
	//}}AFX_DATA_INIT
}


void CAddFromIni::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAddFromIni)
	DDX_Text(pDX, IDC_FILENAME, m_strFilename);
	DDX_Text(pDX, IDC_SECTION, m_strSection);
	DDX_Check(pDX, IDC_FLUSH, m_bFlush);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CAddFromIni, CDialog)
	//{{AFX_MSG_MAP(CAddFromIni)
		// NOTE: the ClassWizard will add message map macros here
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CAddFromIni message handlers
