#if !defined(AFX_VIDEOS_H__1FEC577A_B2AB_11D2_AD00_444553540000__INCLUDED_)
#define AFX_VIDEOS_H__1FEC577A_B2AB_11D2_AD00_444553540000__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000
// Videos.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CConfirmVideos dialog

class CConfirmVideos : public CDialog
{
// Construction
public:
	CConfirmVideos(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CConfirmVideos)
	enum { IDD = IDD_CONFIRMVIDEOS };
	CString	m_strDrive;
	CString	m_strComment;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CConfirmVideos)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CConfirmVideos)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_VIDEOS_H__1FEC577A_B2AB_11D2_AD00_444553540000__INCLUDED_)
