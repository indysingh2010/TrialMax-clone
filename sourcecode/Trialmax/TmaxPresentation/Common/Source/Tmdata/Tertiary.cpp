//==============================================================================
//
// File Name:	tertiary.cpp
//
// Description:	This file contains member functions of the CTertiary and
//				CTertiaries classes
//
// See Also:	tertiary.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-18-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tertiary.h>

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
// 	Function Name:	CTertiary::CTertiary()
//
// 	Description:	This is the constructor for CTertiary objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTertiary::CTertiary(CSecondary* pSecondary)
{
	m_pSecondary		= pSecondary;
	m_pActiveLink		= 0;
	m_lTertiaryId		= 0;
	m_lSecondaryId		= 0;
	m_lPrimaryId		= 0;
	m_lPlaybackOrder	= 0;
	m_lBarcodeId		= 0;
	m_lAttributes		= 0;
	m_lStartLine		= -1;
	m_lStartPage		= -1;
	m_lStopLine			= -1;
	m_lStopPage			= -1;
	m_dStartTime		= -1.0;
	m_dStopTime			= -1.0;
	m_lHighlighterId	= 0;
	m_crHighlighter		= RGB(255,0,0);
	m_sMediaType		= -1;
	m_strName			= "";
	m_strMediaId		= "";
	m_strFilename		= "";
	m_strDescription	= "";
	m_strRelativePath	= "";
	m_strSiblingId		= "";
}

//==============================================================================
//
// 	Function Name:	CTertiary::~CTertiary()
//
// 	Description:	This is the destructor for CTertiary objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTertiary::~CTertiary()
{
	m_Links.Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CTertiary::FirstLink()
//
// 	Description:	This function will retrieve the first link object in the
//					list.
//
// 	Returns:		A pointer to the first object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CLink* CTertiary::FirstLink()
{
	return m_Links.First();
}

//==============================================================================
//
// 	Function Name:	CTertiary::LastLink()
//
// 	Description:	This function will retrieve the last link object in the
//					list.
//
// 	Returns:		A pointer to the last object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CLink* CTertiary::LastLink()
{
	return m_Links.Last();
}

//==============================================================================
//
// 	Function Name:	CTertiary::NextLink()
//
// 	Description:	This function will retrieve the next link object in the
//					list.
//
// 	Returns:		A pointer to the next object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CLink* CTertiary::NextLink()
{
	return m_Links.Next();
}

//==============================================================================
//
// 	Function Name:	CTertiary::operator < ()
//
// 	Description:	This is an overloaded version of the < operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			CTertiary objects are sorted by the frame. This function assumes
//					the link objects reference the same designation.
//
//==============================================================================
BOOL CTertiary::operator < (const CTertiary& Compare)
{
	return (m_lPlaybackOrder < Compare.m_lPlaybackOrder);
}

//==============================================================================
//
// 	Function Name:	CTertiary::operator > ()
//
// 	Description:	This is an overloaded version of the > operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			CTertiary objects are sorted by the frame. This function assumes
//					the link objects reference the same designation.
//
//==============================================================================
BOOL CTertiary::operator > (const CTertiary& Compare)
{
	return (m_lPlaybackOrder > Compare.m_lPlaybackOrder);
}

//==============================================================================
//
// 	Function Name:	CTertiary::operator == ()
//
// 	Description:	This is an overloaded version of the == operator.
//
// 	Returns:		TRUE if the objects are equal.
//
//	Notes:			None
//
//==============================================================================
BOOL CTertiary::operator == (const CTertiary& Compare)
{
	return (m_lPlaybackOrder == Compare.m_lPlaybackOrder);
}

//==============================================================================
//
// 	Function Name:	CTertiary::PrevLink()
//
// 	Description:	This function will retrieve the previous link object in the
//					list.
//
// 	Returns:		A pointer to the previous object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CLink* CTertiary::PrevLink()
{
	return m_Links.Prev();
}

//==============================================================================
//
// 	Function Name:	CTertiary::SetFlag()
//
// 	Description:	Called to set the specified attributes flag
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTertiary::SetFlag(long lMask, BOOL bState)
{
	if(bState == TRUE)
		m_lAttributes |= lMask;  // Set the associated bit
	else
		m_lAttributes &= ~lMask; //	Clear the associated bit

}

//==============================================================================
//
// 	Function Name:	CTertiaries::Add()
//
// 	Description:	This function will add an object to the list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTertiaries::Add(CTertiary* pTertiary, BOOL bSorted)
{
	POSITION	Pos;
	POSITION	Prev;
	CTertiary*	pCurrent;

	//	Add at the head of the list if the list is empty
	if(IsEmpty())
	{
		AddHead(pTertiary);
		return;
	}

	//	Look for the correct position if sorting on entry
	if(bSorted && (*((CTertiary*)GetTail()) > *pTertiary))
	{
		Pos = GetHeadPosition();
		Prev = Pos;
		while(Pos != NULL)
		{
			if((pCurrent = (CTertiary*)GetNext(Pos)) == NULL)
				continue;

			if(*pTertiary < *pCurrent)
			{
				InsertBefore(Prev, pTertiary);
				return;
			}
			
			Prev = Pos;	
		}
	}

	//	If we made it this far we must have to add it to the end of the list
	AddTail(pTertiary);
}

//==============================================================================
//
// 	Function Name:	CTertiary::CTertiaries()
//
// 	Description:	This is the constructor for CTertiaries objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTertiaries::CTertiaries()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CTertiary::~CTertiaries()
//
// 	Description:	This is the destructor for CTertiaries objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTertiaries::~CTertiaries()
{
	//	Flush the list and destroy it's objects
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CTertiaries::FindByBarcodeId()
//
// 	Description:	This function will search the list for the item with the
//					numeric identifier specified by the caller.
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			The local position markers are not modified with a call to
//					this function.
//
//==============================================================================
CTertiary* CTertiaries::FindByBarcodeId(long lId)
{
	CTertiary* pTertiary = First();
	while(pTertiary)
	{
		if(pTertiary->m_lBarcodeId == lId)
			return pTertiary;
		else
			pTertiary = Next();
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CTertiaries::FindByDatabaseId()
//
// 	Description:	This function will search the list for the item with the
//					numeric identifier specified by the caller.
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			The local position markers are not modified with a call to
//					this function.
//
//==============================================================================
CTertiary* CTertiaries::FindByDatabaseId(long lId)
{
	CTertiary* pTertiary = First();
	while(pTertiary)
	{
		if(pTertiary->m_lTertiaryId == lId)
			return pTertiary;
		else
			pTertiary = Next();
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CTertiaries::FindByOrder()
//
// 	Description:	This function will search the list for the item with the
//					playback order specified by the caller.
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			None
//
//==============================================================================
CTertiary* CTertiaries::FindByOrder(long lOrder)
{
	CTertiary* pTertiary = First();
	while(pTertiary)
	{
		if(pTertiary->m_lPlaybackOrder == lOrder)
			return pTertiary;
		else
			pTertiary = Next();
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CTertiaries::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CTertiaries::Find(CTertiary* pTertiary)
{
	return (CObList::Find(pTertiary));
}

//==============================================================================
//
// 	Function Name:	CTertiaries::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CTertiary* CTertiaries::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CTertiary*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CTertiaries::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CTertiaries::Flush(BOOL bDelete)
{
	CTertiary* pTertiary;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pTertiary = (CTertiary*)GetNext(m_NextPos)) != 0)
				delete pTertiary;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CTertiaries::IsFirst()
//
// 	Description:	This function is called to determine if the object specified
//					by the caller is the first one in the list.
//
// 	Returns:		TRUE if first in list
//
//	Notes:			None
//
//==============================================================================
BOOL CTertiaries::IsFirst(CTertiary* pTertiary)
{
	POSITION Pos = GetHeadPosition();

	if(Pos != NULL)
		return (pTertiary == (CTertiary*)GetNext(Pos));
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CTertiaries::IsLast()
//
// 	Description:	This function is called to determine if the object specified
//					by the caller is the last one in the list.
//
// 	Returns:		TRUE if last in list
//
//	Notes:			None
//
//==============================================================================
BOOL CTertiaries::IsLast(CTertiary* pTertiary)
{
	POSITION Pos = GetTailPosition();

	if(Pos != NULL)
		return (pTertiary == (CTertiary*)GetPrev(Pos));
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CTertiaries::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CTertiary* CTertiaries::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CTertiary*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CTertiaries::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CTertiary* CTertiaries::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CTertiary*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CTertiaries::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CTertiary* CTertiaries::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CTertiary*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CTertiaries::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTertiaries::Remove(CTertiary* pTertiary, BOOL bDelete)
{
	POSITION Pos = Find(pTertiary);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pTertiary;
	}
}

