// Tmbarsvc.h : main header file for the TMBARSVC application
//

#if !defined(AFX_TMBARSVC_H__3F4BEF1B_C5E5_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_TMBARSVC_H__3F4BEF1B_C5E5_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CTmbarsvcApp:
// See Tmbarsvc.cpp for the implementation of this class
//

class CTmbarsvcApp : public CWinApp
{
public:
	CTmbarsvcApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmbarsvcApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation
	//{{AFX_MSG(CTmbarsvcApp)
	afx_msg void OnAppAbout();
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMBARSVC_H__3F4BEF1B_C5E5_11D3_8177_00802966F8C1__INCLUDED_)
