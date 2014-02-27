//==============================================================================
//
// File Name:	browseapp.h
//
// Description:	This file contains the declaration of the CTMBrowseApp class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	02-09-02	1.00		Original Release
//==============================================================================
#if !defined(__BROWSEAPP_H__)
#define __BROWSEAPP_H__

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
class CTMBrowseApp : public COleControlModule
{
	private:

	public:
	
		BOOL		InitInstance();
		int			ExitInstance();
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations imbrowsetely before the previous line.

#endif // !defined(__BROWSEAPP_H__)
