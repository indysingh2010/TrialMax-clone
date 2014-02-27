//==============================================================================
//
// File Name:	tmdepoap.h
//
// Description:	This file contains the declaration of the CTMTextApp class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	01-15-99	1.00		Original Release
//==============================================================================
#if !defined(AFX_TM_TEXT6_H__07F27A4C_ABF9_11D2_8C08_00802966F8C1__INCLUDED_)
#define AFX_TM_TEXT6_H__07F27A4C_ABF9_11D2_8C08_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#if !defined( __AFXCTL_H__ )
	#error include 'afxctl.h' before including this file
#endif
#include <resource.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------
extern const GUID CDECL _tlid;
extern const WORD		_wVerMajor;
extern const WORD		_wVerMinor;

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMTextApp : public COleControlModule
{
	private:

	public:
	
		BOOL	InitInstance();
		int		ExitInstance();
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TM_TEXT6_H__07F27A4C_ABF9_11D2_8C08_00802966F8C1__INCLUDED_)

