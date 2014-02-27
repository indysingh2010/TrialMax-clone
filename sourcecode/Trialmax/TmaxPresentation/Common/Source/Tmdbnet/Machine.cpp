//==============================================================================
//
// File Name:	alias.cpp
//
// Description:	This file contains member functions of the CMachine class
//
// See Also:	machine.h
//
// Copyright	FTI Consulting 1997-2004
//
//==============================================================================
//	Date		Revision    Description
//	08-13-04	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <machine.h>

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

//==============================================================================
//
// 	Function Name:	CMachine::CMachine()
//
// 	Description:	This is the constructor for CMachine objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMachine::CMachine() : CObject()
{
	m_iPathMap = 0;
	m_strName.Empty();
}

//==============================================================================
//
// 	Function Name:	CMachine::~CMachine()
//
// 	Description:	This is the destructor for CMachine objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMachine::~CMachine()
{
}

