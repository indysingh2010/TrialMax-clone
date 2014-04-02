//==============================================================================
//
// File Name:	App.h
//
// Description:	This file contains the declaration of the CApp class
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	03-30-2007	1.00		Original Release
//==============================================================================
#if !defined(__APP_H__)
#define __APP_H__

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

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CApp : public CWinApp
{
	private:

		CString				m_strAppFolder;

	public:
	
							CApp();

		LPCSTR				GetAppFolder(){ return m_strAppFolder; }

	protected:


	public:

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

	// Implementation

	//{{AFX_MSG(CApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__APP_H__)
