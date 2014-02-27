// VKBWndDlg.cpp : implementation file
//

#include "stdafx.h"
#include "VKBWndDlg.h"


// CVKBWndDlg dialog

IMPLEMENT_DYNAMIC(CVKBWndDlg, CDialog)

CVKBWndDlg::CVKBWndDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CVKBWndDlg::IDD, pParent)
{

}

CVKBWndDlg::~CVKBWndDlg()
{
}

void CVKBWndDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CVKBWndDlg, CDialog)
	ON_BN_CLICKED(IDC_BUTTON1, &CVKBWndDlg::OnBnClickedButton1)
	ON_WM_CREATE()
END_MESSAGE_MAP()


// CVKBWndDlg message handlers

void CVKBWndDlg::OnBnClickedButton1()
{
	ShellExecute( NULL, "open", "TabTip.exe", NULL, NULL, SW_SHOWNORMAL );
}

int CVKBWndDlg::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CDialog::OnCreate(lpCreateStruct) == -1)
		return -1;

	return 0;
}
