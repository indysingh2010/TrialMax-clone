//==============================================================================
//
// File Name:	tmview.h
//
// Description:	This file contains the declaration of the CTMViewApp class.
//				This is the application wrapper for tm_view6.ocx.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	07-11-97	1.00		Original Release
//==============================================================================
#if !defined(AFX_TMVIEWAP_H__FEB40DFC_FA01_11D0_B002_008029EFD140__INCLUDED_)
#define AFX_TMVIEWAP_H__FEB40DFC_FA01_11D0_B002_008029EFD140__INCLUDED_

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
#define ROUND(x) ((int)(((x) < 0) ? ((x) - 0.5) : ((x) + 0.5)))

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------
extern const GUID CDECL _tlid;
extern const WORD		_wVerMajor;
extern const WORD		_wVerMinor;

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMViewApp : public COleControlModule
{
	public:
	
		BOOL	InitInstance();
		int		ExitInstance();
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TM_VIEW6_H__FEB40DFC_FA01_11D0_B002_008029EFD140__INCLUDED)
