//==============================================================================
//
// File Name:	tmbarsap.h
//
// Description:	This file contains the declaration of the CTMBarsApp class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-09-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_TM_BARS6_H__3F4BEF0C_C5E5_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_TM_BARS6_H__3F4BEF0C_C5E5_11D3_8177_00802966F8C1__INCLUDED_

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
class CTMBarsApp : public COleControlModule
{
	private:

	public:
	
		BOOL					InitInstance();
		int						ExitInstance();

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TM_BARS6_H__3F4BEF0C_C5E5_11D3_8177_00802966F8C1__INCLUDED)
