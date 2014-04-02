//==============================================================================
//
// File Name:	tmxmlap.cpp
//
// Description:	This file contains member functions of the CTMXmlApp class.
//
// Functions:   CTMXmlApp::ExitInstance()
//				CTMXmlApp::InitInstance()
//
//				DllRegisterServer()
//				DllUnregisterServer()
//				DllRegisterAsDefault()
//
// See Also:	tmxml.h
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	03-02-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>

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
//	rev 5.0:	Original Release
//
//	rev	5.1:	Added jumpToPage() method
//				Added loadDocument() method
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

//	These identifiers are used to indicate the highest version of Tmx/Xml file
//	this control supports
const int _iTmxVerMajor = 4;
const int _iTmxVerMinor = 1;

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------
CTMXmlApp NEAR theApp;

/* Replace 1 */
const GUID CDECL BASED_CODE _tlid =
		{ 0x584fd442, 0xeaaa, 0x47b5, { 0xb8, 0x64, 0xad, 0x60, 0x9f, 0x91, 0x69, 0x76 } };

const char* _pszMTCLSID = "{6A8F7FE8-265A-431B-AB92-A3661848D4A1}";

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------

//==============================================================================
//
// 	Function Name:	CTMXmlApp::ExitInstance()
//
// 	Description:	This is called when a user releases an instance.
//
// 	Returns:		The exit level.
//
//	Notes:			None
//
//==============================================================================
int CTMXmlApp::ExitInstance()
{
	return COleControlModule::ExitInstance();
}

//==============================================================================
//
// 	Function Name:	CTMXmlApp::InitInstance()
//
// 	Description:	This is called when a user requests an instance.
//
// 	Returns:		TRUE if successful.
//
//	Notes:			None
//
//==============================================================================
BOOL CTMXmlApp::InitInstance()
{
	if(COleControlModule::InitInstance())
	{
		// Add support for OLE control containment
		AfxEnableControlContainer();
		
		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	DllRegisterAsDefault()
//
// 	Description:	This function is called to register the control as the
//					default viewer for the specified MIME type
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void DllRegisterAsDefault(char* pExtension, char* pMimeType, char* pMimeExtension)
{
    HKEY        hKey = NULL;
    HKEY        hSubKey = NULL;
    BOOL        bFailed = TRUE;
    char        szSubKey[513];
	char		szMimeKey[128];
	char		szMessage[256];

    while(1)
    {
        //	Make sure the entry for the specified type exists in the MIME database
		sprintf_s(szMimeKey, sizeof(szMimeKey), "MIME\\DataBase\\Content Type\\%s", pMimeType);
        if(ERROR_SUCCESS != RegCreateKey(HKEY_CLASSES_ROOT, szMimeKey, &hKey))
            break;

        //	Clear any default value that may already exist
        //if(ERROR_SUCCESS != RegSetValueEx(hKey, NULL, 0, REG_SZ, (const BYTE *)"", 0)) 
            //break;

        //	Add the "Extension=" value to associate files that have the specified 
		//	extension with the specified MIME type
        if(ERROR_SUCCESS != RegSetValueEx(hKey, "Extension", 0, REG_SZ, 
										 (const BYTE *)pMimeExtension, lstrlen(pMimeExtension)))
            break;

        //	Add class id to associate this control with the MIME type
        if(ERROR_SUCCESS != RegSetValueEx(hKey, "CLSID", 0, REG_SZ,
										 (const BYTE *)_pszMTCLSID, 
										  lstrlen(_pszMTCLSID)))
            break;

        //	We are done editing the MIME database
		RegCloseKey(hKey);

        //	Make sure we are registered as a valid file extension
        if(ERROR_SUCCESS != RegCreateKey(HKEY_CLASSES_ROOT, pExtension, &hKey))
            break;

        //	Clear any default value that may already exist
        //if(ERROR_SUCCESS != RegSetValueEx(hKey, NULL, 0, REG_SZ, (const BYTE *)"", 0)) 
            //break;

        //	Add content type to associate this extension with the content type.
		//  This is required and is used when the mime type is unknown and IE 
		//	looks up associations by extension
        if(ERROR_SUCCESS != RegSetValueEx(hKey, "Content Type", 0, REG_SZ,
										 (const BYTE *)pMimeType, lstrlen(pMimeType)))
            break;

        RegCloseKey(hKey);

        // Open the key under the control's clsid HKEY_CLASSES_ROOT\CLSID\<CLSID>
        wsprintf(szSubKey, "%s\\%s", "CLSID", _pszMTCLSID);
        if(ERROR_SUCCESS != RegOpenKey(HKEY_CLASSES_ROOT, szSubKey, &hKey))
            break;

        //	Create the EnableFullPage and extension key under this so that we 
		//	can display files with the extension full frame in the browser
        wsprintf(szSubKey, "%s\\%s", "EnableFullPage", pExtension);
        if(ERROR_SUCCESS != RegCreateKey(hKey, szSubKey, &hSubKey))
            break;

        //	We're finished
		bFailed = FALSE;
		break;

    }

    //	Close the open keys
	if(hKey)
		RegCloseKey(hKey);
    if(hSubKey)
        RegCloseKey(hSubKey);

    //	Did an error occur?
	if(bFailed)
	{
		sprintf_s(szMessage, sizeof(szMessage), "Unable to register tm_xml6.ocx as default viewer for %s files !",
				pExtension);
        MessageBox(0, szMessage, "Registration Error", MB_OK | MB_ICONEXCLAMATION);
	}

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

	//	Register as default viewer for MIME types
	/*
	DllRegisterAsDefault(".tif", "image/tif", ".tif");
	DllRegisterAsDefault(".tiff","image/tiff", ".tiff");
	DllRegisterAsDefault(".bmp", "application/ftic-bmp", ".bmp");
	DllRegisterAsDefault(".png", "application/ftic-png", ".png");
	DllRegisterAsDefault(".pcx", "application/ftic-pcx", ".pcx");
	DllRegisterAsDefault(".tmx", "application/ftic-tmx4",".tmx");
	DllRegisterAsDefault(".zap", "application/ftic-zap4",".zap");
	*/

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

