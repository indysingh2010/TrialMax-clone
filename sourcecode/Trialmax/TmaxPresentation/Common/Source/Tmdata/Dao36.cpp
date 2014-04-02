//==============================================================================
//
// File Name:	dao36.cpp
//
// Description:	This file contains support functions required for Access 2K support
//
// See Also:	dao36.h
//
// Copyright	FTI Consulting 1997-2003
//
//==============================================================================
//	Date		Revision    Description
//	12-11-03	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <dao36.h>

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
#if defined _USE_DAO36_
STDAPI Access2KStringAllocCallback(DWORD dwLen, DWORD pData, void** ppv)
{
	LPTSTR lpsz;
	CString* pstr = (CString*)pData;

	dwLen++;

	TRY
	{
		//Allocate twice the space needed so that DAO does not overwrite the buffer
		lpsz = pstr->GetBufferSetLength(2*dwLen/sizeof(TCHAR));
		*ppv = (void*)(dwLen > 0 ? lpsz : NULL);
	}
	CATCH_ALL(e)
	{
		e->Delete();
		return E_OUTOFMEMORY;
	}
	END_CATCH_ALL

	return S_OK;
}  
#endif

