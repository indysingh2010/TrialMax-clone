#pragma once

#include <Resource.h>
// TransparentDlg dialog

class TransparentDlg : public CDialog
{
	DECLARE_DYNAMIC(TransparentDlg)

public:
	TransparentDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~TransparentDlg();

// Dialog Data
	enum { IDD = IDD_TRANSPARENT };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
//	afx_msg void OnPaint();
};
