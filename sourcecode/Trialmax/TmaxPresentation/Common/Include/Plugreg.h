//==============================================================================
//
// File Name:	plugreg.h
//
// Description:	This file contains definitions and prototypes used to register
//				a plugin control.
//
// Author:		Kenneth Moore
//
// Copyright Oceaneering Technologies
//
//==============================================================================
//	Date		Revision    Description
//	12-31-01	1.00		Original Release
//==============================================================================
#if !defined(__PLUGREG_H__)
#define __PLUGREG_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	REGISTRY PATHS
#define TMPLUGIN_REGISTRY_MEDIA_PATH		"HKEY_LOCAL_MACHINE\\Software\\FTI Consulting\\TMMedia"
#define TMPLUGIN_REGISTRY_EXTENSIONS_PATH	"HKEY_LOCAL_MACHINE\\Software\\FTI Consulting\\TMMedia\\Extensions"

//	REGISTRY VALUES
#define TMPLUGIN_REGISTRY_CLSID_VALUE		"CLSID"

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	PROTOTYPES
//------------------------------------------------------------------------------
BOOL TMPlugin_GetClsString(REFCLSID rClsId, char* pString, int iLength);
BOOL TMPlugin_Register(char* pszName, REFCLSID rClsId, 
					   char** paExtensions, int iExtensions);

#endif // !defined(__PLUGREG_H__)
