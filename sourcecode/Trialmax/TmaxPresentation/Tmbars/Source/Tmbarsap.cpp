//==============================================================================
//
// File Name:	tmbarsap.cpp
//
// Description:	This file contains member functions of the CTMBarsApp class.
//
// See Also:	tmbarsap.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-09-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmbarsap.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

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
//	_wVerBuild:	Build version identifier is used to track controls and 
//				applications during development. This identifier is updated
//				on a weekly basis.
//
//	NOTE:		For ActiveX controls, the associated object definition library
//				(.odl) file MUST be updated when the major or minor version
//				identifiers are changed.				
//
//	REV 5.0:	Added Object Safety dispatch interface for embedding in IE
//				Added GetRegisteredPath() method
//				Added GetClassIdString() method
//				
//	rev 6.1.0:	Changed Build property to VerBuild
//				Changed TextVer property to VerTextLong
//				Changed MajorVer property to VerMajor
//				Changed MinorVer property to VerMinor
//				Added VerQEF property
//				Added VerTextShort property
//				Added VerBuildDate property
//				Modified constructor to extract version identifiers from
//				the control's Version resource
//				
//------------------------------------------------------------------------------
const WORD	_wVerMajor = 6;
const WORD	_wVerMinor = 4;

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------
CTMBarsApp NEAR theApp;

/* Replace 1 */
const GUID CDECL BASED_CODE _tlid =
		{ 0x4ad4775a, 0xabb, 0x484d, { 0xb4, 0x19, 0x32, 0x59, 0x64, 0x20, 0xc0, 0xb4 } };

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------

//==============================================================================
//
// 	Function Name:	CTMBarsApp::ExitInstance()
//
// 	Description:	This is called when a user releases an instance.
//
// 	Returns:		The exit level.
//
//	Notes:			None
//
//==============================================================================
int CTMBarsApp::ExitInstance()
{
	return COleControlModule::ExitInstance();
}

//==============================================================================
//
// 	Function Name:	CTMBarsApp::InitInstance()
//
// 	Description:	This is called when a user requests an instance.
//
// 	Returns:		TRUE if successful.
//
//	Notes:			None
//
//==============================================================================
BOOL CTMBarsApp::InitInstance()
{
	//	Perform base class initialization
	if(!COleControlModule::InitInstance())	
		return FALSE;

	//	Enable support for containment of other OCX controls
	AfxEnableControlContainer();

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	DllRegisterServer()
//
// 	Description:	This is called to register the ocx control.
//
// 	Returns:		None
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
// 	Description:	This is called to unregister the ocx control.
//
// 	Returns:		None
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

