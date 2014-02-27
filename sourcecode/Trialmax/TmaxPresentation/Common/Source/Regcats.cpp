//==============================================================================
//
// File Name:	regcats.cpp
//
// Description:	This file contains helper functions used to register the ocx
//				controls in the appropriate component categories.
//
// See Also:	regcats.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	05-11-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <regcats.h>

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
//	MAPS
//------------------------------------------------------------------------------

//==============================================================================
//
// 	Function Name:	CreateComponentCategory()
//
// 	Description:	This function is called to ensure that the specified
//					component category exists in the system registry.
//
// 	Returns:		S_OK if successful
//
//	Notes:			Copied from the ActiveX SDK source
//
//==============================================================================
HRESULT CreateComponentCategory(CATID catid, WCHAR* catDescription)
{

    ICatRegister* pcr = NULL ;	// interface pointer
    HRESULT hr = S_OK ;

    hr = CoCreateInstance(CLSID_StdComponentCategoriesMgr, 
			NULL, CLSCTX_INPROC_SERVER, IID_ICatRegister, (void**)&pcr);
	if (FAILED(hr))
		return hr;

    // Make sure the HKCR\Component Categories\{..catid...}
    // key is registered
    CATEGORYINFO catinfo;
    catinfo.catid = catid;
    catinfo.lcid = 0x0409 ; // english

	// Make sure the provided description is not too long.
	// Only copy the first 127 characters if it is
	int len = wcslen(catDescription);
	if(len>127)
		len = 127;
    wcsncpy_s(catinfo.szDescription, 128, catDescription, len);
	// Make sure the description is null terminated
	catinfo.szDescription[len] = '\0';

    hr = pcr->RegisterCategories(1, &catinfo);
	pcr->Release();

	return hr;
}

//==============================================================================
//
// 	Function Name:	RegisterCLSIDInCategory()
//
// 	Description:	This function is called to register the ocx class id in the
//					specified component category
//
// 	Returns:		S_OK if successful
//
//	Notes:			Copied from the ActiveX SDK source
//
//==============================================================================
HRESULT RegisterCLSIDInCategory(REFCLSID clsid, CATID catid)
{
// Register your component categories information.
    ICatRegister* pcr = NULL ;
    HRESULT hr = S_OK ;
    hr = CoCreateInstance(CLSID_StdComponentCategoriesMgr, 
			NULL, CLSCTX_INPROC_SERVER, IID_ICatRegister, (void**)&pcr);
    if (SUCCEEDED(hr))
    {
       // Register this category as being "implemented" by
       // the class.
       CATID rgcatid[1] ;
       rgcatid[0] = catid;
       hr = pcr->RegisterClassImplCategories(clsid, 1, rgcatid);
    }

    if (pcr != NULL)
        pcr->Release();
  
	return hr;
}
