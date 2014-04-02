// Tmgrabvc.h : main header file for the TMGRABVC application
//

#if !defined(AFX_TMGRABVC_H__15EF4EA4_6F06_11D6_8F0B_00802966F8C1__INCLUDED_)
#define AFX_TMGRABVC_H__15EF4EA4_6F06_11D6_8F0B_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CTmgrabvcApp:
// See Tmgrabvc.cpp for the implementation of this class
//

class CTmgrabvcApp : public CWinApp
{
public:
	CTmgrabvcApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmgrabvcApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation
	//{{AFX_MSG(CTmgrabvcApp)
	afx_msg void OnAppAbout();
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMGRABVC_H__15EF4EA4_6F06_11D6_8F0B_00802966F8C1__INCLUDED_)
