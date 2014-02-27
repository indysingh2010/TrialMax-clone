#pragma once

#include <Resource.h>

// CVKBWndDlg dialog

class CVKBWndDlg : public CDialog
{
	DECLARE_DYNAMIC(CVKBWndDlg)

public:
	CVKBWndDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CVKBWndDlg();

// Dialog Data
//	enum { IDD = IDD_DIALOG1 };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedButton1();
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
};
