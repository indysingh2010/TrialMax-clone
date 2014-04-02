//==============================================================================
//
// File Name:	tmsetap.h
//
// Description:	This file contains the declaration of the CTMSetupApp class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-29-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_TM_SETUP6_H__98CB02CC_D4CA_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_TM_SETUP6_H__98CB02CC_D4CA_11D3_8177_00802966F8C1__INCLUDED_

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

//	Control identifiers
#define TMAX_AXCTRL_TMSTAT			0
#define TMAX_AXCTRL_TMTEXT			1
#define TMAX_AXCTRL_TMVIEW			2
#define TMAX_AXCTRL_TMLPEN			3
#define TMAX_AXCTRL_TMTOOL			4
#define TMAX_AXCTRL_TMBARS			5
#define TMAX_AXCTRL_TMMOVIE			6
#define TMAX_AXCTRL_TMPOWER			7
#define TMAX_AXCTRL_TMPRINT			8
#define TMAX_AXCTRL_TMSHARE			9
#define TMAX_AXCTRL_TMGRAB			10
#define TMAX_AXCTRL_TMSETUP			11
#define TMAX_AXCTRL_MAX				12

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------
extern const GUID CDECL _tlid;
extern const WORD		_wVerMajor;
extern const WORD		_wVerMinor;

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMSetupApp : public COleControlModule
{
	private:

	public:
	
		BOOL	InitInstance();
		int		ExitInstance();

	protected:
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TM_SETUP6_H__98CB02CC_D4CA_11D3_8177_00802966F8C1__INCLUDED_)

