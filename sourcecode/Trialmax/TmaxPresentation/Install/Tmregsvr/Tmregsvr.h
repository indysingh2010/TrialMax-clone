//==============================================================================
//
// File Name:	Tmregsvr.h
//
// Description:	This file contains the declaration of the TmregsvrApp class. 
//
// Author:		Kenneth Moore
//
// Copyright 1999 Forensics Technologies International
//
//==============================================================================
//	Date		Revision    Description
//	03-26-99	1.00		Original Release
//==============================================================================
#if !defined(AFX_TMREGSVR_H__5D8FFB64_E41C_11D2_8175_00802966F8C1__INCLUDED_)
#define AFX_TMREGSVR_H__5D8FFB64_E41C_11D2_8175_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTmregsvrApp : public CWinApp
{
	private:

	public:
	
						CTmregsvrApp();

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmregsvrApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

	//{{AFX_MSG(CTmregsvrApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMREGSVR_H__5D8FFB64_E41C_11D2_8175_00802966F8C1__INCLUDED_)
