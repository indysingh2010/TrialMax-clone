//==============================================================================
//
// File Name:	tmtoolap.h
//
// Description:	This file contains the declaration of the CTMToolApp class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	04-21-98	1.00		Original Release
//==============================================================================
#if !defined(AFX_TM_TOOL6_H__BBD9179F_D89D_11D1_B16C_008029EFD140__INCLUDED_)
#define AFX_TM_TOOL6_H__BBD9179F_D89D_11D1_B16C_008029EFD140__INCLUDED_

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

extern const GUID CDECL _tlid;
extern const WORD		_wVerMajor;
extern const WORD		_wVerMinor;

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMToolApp : public COleControlModule
{
	private:

	public:
	
		BOOL	InitInstance();
		int		ExitInstance();
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TM_TOOL6_H__BBD9179F_D89D_11D1_B16C_008029EFD140__INCLUDED)
