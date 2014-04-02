// Tmviewvc.h : main header file for the TMVIEWVC application
//

#if !defined(AFX_TMVIEWVC_H__CAEBF183_FABA_11D0_B003_008029EFD140__INCLUDED_)
#define AFX_TMVIEWVC_H__CAEBF183_FABA_11D0_B003_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CTmviewvcApp:
// See Tmviewvc.cpp for the implementation of this class
//

class CTmviewvcApp : public CWinApp
{
public:
	CTmviewvcApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmviewvcApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation
	COleTemplateServer m_server;
		// Server object for document creation

	//{{AFX_MSG(CTmviewvcApp)
	afx_msg void OnAppAbout();
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMVIEWVC_H__CAEBF183_FABA_11D0_B003_008029EFD140__INCLUDED_)
