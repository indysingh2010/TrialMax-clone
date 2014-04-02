//==============================================================================
//
// File Name:	BinderEntry.cpp
//
// Description:	This file contains member functions of the CBinderEntry
//				
//
// See Also:	BinderEntry.h
//
// Copyright	Tenpearls LLC 2014
//
//==============================================================================
//	Date		Revision    Description
//	24-03-14	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <binderentry.h>

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
// 	Function Name:	CBinderEntry::CBinderEntry()
//
// 	Description:	This is the constructor for CBinderEntry objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CBinderEntry::CBinderEntry()
{
		m_AutoId = 0;
		m_ParentId = 0;
		m_Path = "";
		m_Children = 0;
		m_Attributes = 0;
		m_Name = "";
		m_Description = "";
		m_DisplayOrder = 0;
		m_CreatedBy = 0;
		m_CreatedOn = (DATE)0;
		m_ModifiedBy = 0;
		m_ModifiedOn = (DATE)0;;
		m_SpareText = "";
		m_SpareNumber = 0;

}

//==============================================================================
//
// 	Function Name:	CBinderEntry::~CBinderEntry()
//
// 	Description:	This is the destructor for CBinderEntry objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CBinderEntry::~CBinderEntry()
{
	
}



