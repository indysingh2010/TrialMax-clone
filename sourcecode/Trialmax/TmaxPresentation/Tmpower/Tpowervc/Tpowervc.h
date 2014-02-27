// Tpowervc.h : main header file for the TPOWERVC application
//

#if !defined(AFX_TPOWERVC_H__F0E8E9C4_058D_11D3_8175_00802966F8C1__INCLUDED_)
#define AFX_TPOWERVC_H__F0E8E9C4_058D_11D3_8175_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CTpowervcApp:
// See Tpowervc.cpp for the implementation of this class
//

class CTpowervcApp : public CWinApp
{
public:
	CTpowervcApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTpowervcApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation
	//{{AFX_MSG(CTpowervcApp)
	afx_msg void OnAppAbout();
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TPOWERVC_H__F0E8E9C4_058D_11D3_8175_00802966F8C1__INCLUDED_)
