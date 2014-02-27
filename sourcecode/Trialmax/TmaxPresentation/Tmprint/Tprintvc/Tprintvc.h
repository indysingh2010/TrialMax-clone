// Tprintvc.h : main header file for the TPRINTVC application
//

#if !defined(AFX_TPRINTVC_H__67E2EBA4_B2F6_11D3_BF86_0080296301C0__INCLUDED_)
#define AFX_TPRINTVC_H__67E2EBA4_B2F6_11D3_BF86_0080296301C0__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CTprintvcApp:
// See Tprintvc.cpp for the implementation of this class
//

class CTprintvcApp : public CWinApp
{
public:
	CTprintvcApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTprintvcApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation
	//{{AFX_MSG(CTprintvcApp)
	afx_msg void OnAppAbout();
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TPRINTVC_H__67E2EBA4_B2F6_11D3_BF86_0080296301C0__INCLUDED_)
