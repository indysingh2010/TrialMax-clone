// splash.cpp : implementation file
//

#include <stdafx.h>
#include <splash.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CSplashBox dialog


CSplashBox::CSplashBox(CWnd* pParent /*=NULL*/)
	: CDialog(CSplashBox::IDD, pParent)
{
	//{{AFX_DATA_INIT(CSplashBox)
	//}}AFX_DATA_INIT
	//	Create the window now
	m_brBackground.CreateSolidBrush(RGB(0,0,0));

	Create(CSplashBox::IDD, pParent);
}


void CSplashBox::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CSplashBox)
	DDX_Control(pDX, IDC_BITMAP, m_ctrlBitmap);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CSplashBox, CDialog)
	//{{AFX_MSG_MAP(CSplashBox)
	ON_WM_CTLCOLOR()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CSplashBox message handlers

BOOL CSplashBox::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	m_ctrlBitmap.MoveWindow(0,0,320,240);
	MoveWindow(0,0,320,240);
	CenterWindow();
	return TRUE;  
}

//==============================================================================
//
// 	Function Name:	CSplashBox::OnCtlColor()
//
// 	Description:	This function is overloaded to set the background of the
//					dialog to black.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
HBRUSH CSplashBox::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor) 
{
	//	Is this a request for the dialog brush?
	if(nCtlColor == CTLCOLOR_DLG)
		return m_brBackground;
	else
		return CDialog::OnCtlColor(pDC, pWnd, nCtlColor);
}

