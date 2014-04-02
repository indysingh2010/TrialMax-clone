// Tmovievc.h : main header file for the TMOVIEVC application
//

#if !defined(AFX_TMOVIEVC_H__10AFCB15_02BA_11D2_B1BF_008029EFD140__INCLUDED_)
#define AFX_TMOVIEVC_H__10AFCB15_02BA_11D2_B1BF_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CTmovievcApp:
// See Tmovievc.cpp for the implementation of this class
//

class CTmovievcApp : public CWinApp
{
public:
	CTmovievcApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmovievcApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation

	//{{AFX_MSG(CTmovievcApp)
	afx_msg void OnAppAbout();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMOVIEVC_H__10AFCB15_02BA_11D2_B1BF_008029EFD140__INCLUDED_)
