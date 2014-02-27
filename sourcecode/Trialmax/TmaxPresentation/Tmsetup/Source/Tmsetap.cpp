//==============================================================================
//
// File Name:	tmsetap.cpp
//
// Description:	This file contains member functions of the CTMSetupApp class.
//
// See Also:	tmprntap.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	12-13-99	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmsetap.h>

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
//	rev 5.0:	Added Object Safety dispatch interface for embedding in IE
//				Added GetRegisteredPath() method
//				Added GetClassIdString() method
//
//	rev	5.1:	Added capture page
//
//	rev 5.2:	Added ringtail page
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
CTMSetupApp NEAR theApp;

/* Replace 1 */
const GUID CDECL BASED_CODE _tlid =
		{ 0x655519f9, 0x33b5, 0x4ecd, { 0x8b, 0x3d, 0x96, 0xca, 0x5b, 0x44, 0xed, 0x85 } };

//	Active X CLSID identifiers
const char* _AxClsIds[TMAX_AXCTRL_MAX] =	{	"{0C69F0D1-9BB0-4DB0-A600-D98621E8D8B3}",	//	TMSTAT
												"{AA52288D-2A50-494F-98FE-FFF0D9FBDE56}",	//	TMTEXT
												"{5A3A9FC9-D747-4B92-9106-A32C7E6E84A3}",	//	TMVIEW
												"{7EFCBDC0-F749-4574-8DC1-2E5575DD9808}",	//	TMLPEN
												"{2341B5A2-769B-49CC-8652-B8914992AFB1}",	//	TMTOOL
												"{5284E5B7-9E77-4200-9E9F-D5F22CB40F2C}",	//	TMBARS
												"{D71D2494-B9CA-401F-8E24-1815E077CE64}",	//	TMMOVIE
												"{BD138FDB-21B2-4CF1-8175-A94182FED781}",	//	TMPOWER
												"{2B6165A5-C1FC-463E-9B56-20143BF4F627}",	//	TMPRINT
												"{CB5D5073-AB77-45F6-B728-1808DDC80026}",	//	TMSHARE
												"{4BA3488C-31EC-4619-9D96-1EFE592DD861}",	//	TMGRAB
												"{B581682E-5CC0-4E50-BBBC-582D78677E5A}",	//	TMSETUP
											};

const char* _AxNames[TMAX_AXCTRL_MAX] =	{	"TMStat",
											"TMText",
											"TMView",
											"TMLpen",
											"TMTool",
											"TMBars",
											"TMMovie",
											"TMPower",
											"TMPrint",
											"TMShare",
											"TMGrab",
											"TMSetup",
										};

const char* _AxDescriptions[TMAX_AXCTRL_MAX] = {	"TrialMax Status Bar ActiveX Control",
													"TrialMax Scrolling Text ActiveX Control",
													"TrialMax Image Viewer ActiveX Control",
													"TrialMax Light Pen ActiveX Control",
													"TrialMax Media Toolbar ActiveX Control",
													"TrialMax Toolbar Setup ActiveX Control",
													"TrialMax DirectX Playback ActiveX Control",
													"TrialMax PowerPoint Viewer ActiveX Control",
													"TrialMax Template Printer ActiveX Control",
													"TrialMax Shared Memory ActiveX Control",
													"TrialMax Screen Capture ActiveX Control",
													"TrialMax Presentation Setup ActiveX Control",
											   };

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------

//==============================================================================
//
// 	Function Name:	CTMSetupApp::ExitInstance()
//
// 	Description:	This is called when a user releases an instance.
//
// 	Returns:		The exit level.
//
//	Notes:			None
//
//==============================================================================
int CTMSetupApp::ExitInstance()
{
	return COleControlModule::ExitInstance();
}

//==============================================================================
//
// 	Function Name:	CTMSetupApp::InitInstance()
//
// 	Description:	This is called when a user requests an instance.
//
// 	Returns:		TRUE if successful.
//
//	Notes:			None
//
//==============================================================================
BOOL CTMSetupApp::InitInstance()
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

