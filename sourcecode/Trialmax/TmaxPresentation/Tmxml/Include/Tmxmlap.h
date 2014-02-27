//==============================================================================
//
// File Name:	tmxmlap.h
//
// Description:	This file contains the declaration of the CTMXmlApp class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	03-02-01	1.00		Original Release
//==============================================================================
#if !defined(AFX_TM_XML6_H__2CC295CB_27A3_11D5_8F0A_00802966F8C1__INCLUDED_)
#define AFX_TM_XML6_H__2CC295CB_27A3_11D5_8F0A_00802966F8C1__INCLUDED_

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
extern const int		_iTmxVerMajor;
extern const int		_iTmxVerMinor;

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMXmlApp : public COleControlModule
{
	private:

	public:

		CString		m_strPrinter;
	
		BOOL		InitInstance();
		int			ExitInstance();

	protected:

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TM_XML6_H__2CC295CB_27A3_11D5_8F0A_00802966F8C1__INCLUDED_)

