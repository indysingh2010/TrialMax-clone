//==============================================================================
//
// File Name:	plugreg.cpp
//
// Description:	This file contains functions used to register TMPlugin controls
//
// Copyright Oceaneering Technologies
//
//==============================================================================
//	Date		Revision    Description
//	12-26-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <plugreg.h>
#include <registry.h>

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

//------------------------------------------------------------------------------
//
// 	Function Name:	TMPlugin_GetClsString()
//
//	Parameters:		rClsId  - reference to class id GUID structure
//					pString - string in which to store the class id
//					iLength - length of buffer pointed to by pString
//
// 	Return Value:	None
//
// 	Description:	This function converts the GUID to a string
//
//------------------------------------------------------------------------------
BOOL TMPlugin_GetClsString(REFCLSID rClsId, char* pString, int iLength)
{
	LPOLESTR lpOleId;

	//	Get the Class Id string
	if(StringFromCLSID(rClsId, &lpOleId) != S_OK)
		return FALSE;
	
	//	Convert to Ascii
	if(WideCharToMultiByte(CP_ACP, 0, lpOleId, -1, pString, iLength, NULL, NULL) == 0)
		return FALSE;
	else
		return TRUE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	TMPlugin_Register()
//
//	Parameters:		pszName      - name of the plugin control
//					rClsId       - reference to class id GUID structure
//					paExtensions - array of file extensions
//					iExtensions  - number of extensions in the array
//
// 	Return Value:	None
//
// 	Description:	This function allows the caller to set the class identifier
//					for the plug in
//
//------------------------------------------------------------------------------
BOOL TMPlugin_Register(char* pszName, REFCLSID rClsId, 
					   char** paExtensions, int iExtensions)
{
	CRegistry	Registry;
	char		szClsId[128];
	char		szKey[256];
	BOOL		bReturn = TRUE;

	//	Get the class id as a string
	if(!TMPlugin_GetClsString(rClsId, szClsId, sizeof(szClsId)))
		return FALSE;

	//	Make sure the registry key exists
	if(!Registry.CreateKey(TMPLUGIN_REGISTRY_EXTENSIONS_PATH))
		return FALSE;

	//	Register each of the extensions
	for(int i = 0; i < iExtensions; i++)
	{
		//	Create the extension key
		sprintf(szKey, "%s\\.%s", TMPLUGIN_REGISTRY_EXTENSIONS_PATH, paExtensions[i]);

		if(Registry.CreateKey(szKey))
		{
			//	Now define this control as the default plugin
			if(!Registry.CreateValue(szKey, TMPLUGIN_REGISTRY_CLSID_VALUE, (LPCTSTR)szClsId))
				return FALSE;
		}
		else
		{
			bReturn = FALSE;
		}
	}

	return bReturn;
}

