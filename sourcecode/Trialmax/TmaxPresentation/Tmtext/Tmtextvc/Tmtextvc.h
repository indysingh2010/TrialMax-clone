// Tmtextvc.h : main header file for the TMTEXTVC application
//

#if !defined(AFX_TMTEXTVC_H__07F27A5C_ABF9_11D2_8C08_00802966F8C1__INCLUDED_)
#define AFX_TMTEXTVC_H__07F27A5C_ABF9_11D2_8C08_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CTmtextApp:
// See Tmtextvc.cpp for the implementation of this class
//

class CTmtextApp : public CWinApp
{
public:
	CTmtextApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmtextApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation
	//{{AFX_MSG(CTmtextApp)
	afx_msg void OnAppAbout();
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMTEXTVC_H__07F27A5C_ABF9_11D2_8C08_00802966F8C1__INCLUDED_)
