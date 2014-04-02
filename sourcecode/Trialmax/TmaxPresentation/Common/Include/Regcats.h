//==============================================================================
//
// File Name:	regcats.h
//
// Description:	This file contains prototypes for functions used to register
//				the Trialmax ocx controls in the appropriate component 
//				categories.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2001
//
//==============================================================================
//	Date		Revision    Description
//	05-11-01	1.00		Original Release
//==============================================================================
#if !defined(__REGCATS_H__)
#define __REGCATS_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <comcat.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	PROTOTYPES
//------------------------------------------------------------------------------
HRESULT CreateComponentCategory(CATID catid, WCHAR* catDescription);
HRESULT RegisterCLSIDInCategory(REFCLSID clsid, CATID catid);

#endif // !defined(__REGCATS_H__)
