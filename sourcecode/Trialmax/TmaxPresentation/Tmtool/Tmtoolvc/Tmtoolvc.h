// Tmtoolvc.h : main header file for the TMTOOLVC application
//

#if !defined(AFX_TMTOOLVC_H__BBD917AF_D89D_11D1_B16C_008029EFD140__INCLUDED_)
#define AFX_TMTOOLVC_H__BBD917AF_D89D_11D1_B16C_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CTmtoolvcApp:
// See Tmtoolvc.cpp for the implementation of this class
//

class CTmtoolvcApp : public CWinApp
{
public:
	CTmtoolvcApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmtoolvcApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation

	//{{AFX_MSG(CTmtoolvcApp)
	afx_msg void OnAppAbout();
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMTOOLVC_H__BBD917AF_D89D_11D1_B16C_008029EFD140__INCLUDED_)
