// Tmstatvc.h : main header file for the TMSTATVC application
//

#if !defined(AFX_TMSTATVC_H__44ADEC2C_C265_11D2_8173_00802966F8C1__INCLUDED_)
#define AFX_TMSTATVC_H__44ADEC2C_C265_11D2_8173_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CTmstatvcApp:
// See Tmstatvc.cpp for the implementation of this class
//

class CTmstatvcApp : public CWinApp
{
public:
	CTmstatvcApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmstatvcApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation
	//{{AFX_MSG(CTmstatvcApp)
	afx_msg void OnAppAbout();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMSTATVC_H__44ADEC2C_C265_11D2_8173_00802966F8C1__INCLUDED_)
