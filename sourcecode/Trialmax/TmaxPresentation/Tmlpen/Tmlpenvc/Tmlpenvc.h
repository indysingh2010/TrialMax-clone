// Tmlpenvc.h : main header file for the TMLPENVC application
//

#if !defined(AFX_TMLPENVC_H__52B397AB_A291_11D2_8BFC_00802966F8C1__INCLUDED_)
#define AFX_TMLPENVC_H__52B397AB_A291_11D2_8BFC_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CTmlpenvcApp:
// See Tmlpenvc.cpp for the implementation of this class
//

class CTmlpenvcApp : public CWinApp
{
public:
	CTmlpenvcApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmlpenvcApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation
	//{{AFX_MSG(CTmlpenvcApp)
	afx_msg void OnAppAbout();
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMLPENVC_H__52B397AB_A291_11D2_8BFC_00802966F8C1__INCLUDED_)
