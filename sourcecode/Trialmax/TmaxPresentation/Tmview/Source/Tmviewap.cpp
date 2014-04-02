//==============================================================================
//
// File Name:	tmviewap.cpp
//
// Description:	This file contains member functions of the CTMViewApp class.
//
// See Also:	tmviewap.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	07-11-97	1.00		Original Release
//	11-29-97	1.10		Increased minor revision
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmviewap.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

CTMViewApp NEAR theApp;

/* Replace 1 */
const GUID CDECL BASED_CODE _tlid =
{ 0x230c066f, 0x16df, 0x4a07, { 0x8f, 0x28, 0x8f, 0xd7, 0xf2, 0xe7, 0xd7, 0x21 } };

//------------------------------------------------------------------------------
//	REVISION INFORMATION
//------------------------------------------------------------------------------
//
//	_wVerMajor:	Major version identifer is changed when significant changes
//				have been made in the entire suite of controls and applications
//				All controls and applications should ALWAYS have the same
//				major version identifier
//	_wVerMinor:	Minor version identifier is changed when changes have been made
//				to a control and/or application that would render it unusable
//				with the existing release. All controls and applications will
//				have the same minor revision identifier when bundled as a new
//				release but individual controls and/or applications may be
//				upgraded between releases.
//
//	NOTE:		The major and minor identifiers for this library should match
//				the major and minor identifiers of the matching baseline
//				TrialMax installation.
//
//------------------------------------------------------------------------------
const WORD	_wVerMajor = 6;
const WORD	_wVerMinor = 4;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------


//==============================================================================
//
// 	Function Name:	CTMViewApp::ExitInstance()
//
// 	Description:	This function is called when all users have released the
//					control.
//
// 	Returns:		0 if no errors
//
//	Notes:			None
//
//==============================================================================
int CTMViewApp::ExitInstance()
{
	// TODO: Add your own module termination code here.

	return COleControlModule::ExitInstance();
}

//==============================================================================
//
// 	Function Name:	CTMViewApp::InitInstance()
//
// 	Description:	This function is called when the ocx is initialized.
//
// 	Returns:		TRUE if successful.
//
//	Notes:			None
//
//==============================================================================
BOOL CTMViewApp::InitInstance()
{
	BOOL bInit = COleControlModule::InitInstance();

	if(bInit)
	{
		// Add support for OLE control containment
		AfxEnableControlContainer();
	}

	return bInit;
}

//==============================================================================
//
// 	Function Name:	DllRegisterServer()
//
// 	Description:	This function provides and external interface to be called
//					for registering the control.
//
// 	Returns:		0 if successful.
//
//	Notes:			None
//
//==============================================================================
STDAPI DllRegisterServer(void)
{
	AFX_MANAGE_STATE(_afxModuleAddrThis);

	if (!AfxOleRegisterTypeLib(AfxGetInstanceHandle(), _tlid))
		return ResultFromScode(SELFREG_E_TYPELIB);

	if (!COleObjectFactoryEx::UpdateRegistryAll(TRUE))
		return ResultFromScode(SELFREG_E_CLASS);

	return NOERROR;
}

//==============================================================================
//
// 	Function Name:	DllUnregisterServer()
//
// 	Description:	This function provides and external interface to be called
//					for removing the control from the registry.
//
// 	Returns:		0 if successful.
//
//	Notes:			None
//
//==============================================================================
STDAPI DllUnregisterServer(void)
{
	AFX_MANAGE_STATE(_afxModuleAddrThis);

	if (!AfxOleUnregisterTypeLib(_tlid, _wVerMajor, _wVerMinor))
		return ResultFromScode(SELFREG_E_TYPELIB);

	if (!COleObjectFactoryEx::UpdateRegistryAll(FALSE))
		return ResultFromScode(SELFREG_E_CLASS);

	return NOERROR;
}
