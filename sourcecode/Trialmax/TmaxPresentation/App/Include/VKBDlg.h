#pragma once


#include <Resource.h>

// CVKBDlg dialog

class CVKBDlg : public CDialog
{
	DECLARE_DYNAMIC(CVKBDlg)

public:
	CVKBDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CVKBDlg();

// Dialog Data
	enum { IDD = IDD_KeyBoardDlg };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnBnClickedButton1();
	afx_msg void OnPaint();
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnSize(UINT nType, int cx, int cy);
};
