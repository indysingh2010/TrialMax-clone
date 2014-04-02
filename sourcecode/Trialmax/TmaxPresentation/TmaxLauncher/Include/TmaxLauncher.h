//==============================================================================
//
// File Name:	TmaxLauncher.h
//
// Description:	This file contains the declaration of the TmaxLaucherApp class
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	02-07-2007	1.00		Original Release
//==============================================================================
#if !defined(__TMAXLAUNCHER_H__)
#define __TMAXLAUNCHER_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include <Resource.h>
#include <TmaxLauncherCmdLine.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CTmaxLauncherApp : public CWinApp
{
	private:

		CString					m_strAppFolder;
		CTmaxLauncherCmdLine	m_AppCmdLine;

	public:
	
								CTmaxLauncherApp();

		LPCSTR					GetAppFolder(){ return m_strAppFolder; }
		CTmaxLauncherCmdLine&	GetAppCmdLine(){ return m_AppCmdLine; }

	protected:

	public:

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmaxLauncherApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

	// Implementation

	//{{AFX_MSG(CTmaxLauncherApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__TMAXLAUNCHER_H__)
