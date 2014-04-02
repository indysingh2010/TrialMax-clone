//==============================================================================
//
// File Name:	tmprntap.h
//
// Description:	This file contains the declaration of the CTMPrintApp class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	12-13-99	1.00		Original Release
//==============================================================================
#if !defined(AFX_TM_PRINT6_H__CCAA236C_B13D_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_TM_PRINT6_H__CCAA236C_B13D_11D3_8177_00802966F8C1__INCLUDED_

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

class CTMPrintApp : public COleControlModule
{
	private:

	public:
	
		BOOL	InitInstance();
		int		ExitInstance();

	protected:
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TM_PRINT6_H__CCAA236C_B13D_11D3_8177_00802966F8C1__INCLUDED)
