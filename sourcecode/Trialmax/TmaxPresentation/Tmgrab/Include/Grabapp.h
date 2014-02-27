//==============================================================================
//
// File Name:	grabapp.h
//
// Description:	This file contains the declaration of the CTMGrabApp class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	12-27-01	1.00		Original Release
//==============================================================================
#if !defined(__GRABAPP_H__)
#define __GRABAPP_H__

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
class CTMGrabApp : public COleControlModule
{
	private:
	
	public:
	
					CTMGrabApp();
		BOOL		InitInstance();
		int			ExitInstance();
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations imgrabtely before the previous line.

#endif // !defined(__GRABAPP_H__)
