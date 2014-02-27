// Frames.cpp : implementation file
//

#include "stdafx.h"
#include "Tmovievc.h"
#include "Frames.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CFrames dialog


CFrames::CFrames(CWnd* pParent /*=NULL*/)
	: CDialog(CFrames::IDD, pParent)
{
	//{{AFX_DATA_INIT(CFrames)
	m_lStart = 0;
	m_lStop = 0;
	//}}AFX_DATA_INIT
}


void CFrames::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CFrames)
	DDX_Text(pDX, IDC_START, m_lStart);
	DDX_Text(pDX, IDC_STOP, m_lStop);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CFrames, CDialog)
	//{{AFX_MSG_MAP(CFrames)
		// NOTE: the ClassWizard will add message map macros here
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CFrames message handlers
