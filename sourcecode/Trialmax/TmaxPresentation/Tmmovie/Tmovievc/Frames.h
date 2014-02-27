#if !defined(AFX_FRAMES_H__586A3DE1_1D0D_11D2_B1E9_008029EFD140__INCLUDED_)
#define AFX_FRAMES_H__586A3DE1_1D0D_11D2_B1E9_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000
// Frames.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CFrames dialog

class CFrames : public CDialog
{
// Construction
public:
	CFrames(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CFrames)
	enum { IDD = IDD_FRAMES };
	long	m_lStart;
	long	m_lStop;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CFrames)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CFrames)
		// NOTE: the ClassWizard will add member functions here
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_FRAMES_H__586A3DE1_1D0D_11D2_B1E9_008029EFD140__INCLUDED_)
