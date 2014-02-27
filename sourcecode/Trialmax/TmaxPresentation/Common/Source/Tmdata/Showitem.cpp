//==============================================================================
//
// File Name:	showitem.cpp
//
// Description:	This file contains member functions of the CShowItem and
//				CShowItems classes
//
// See Also:	showitem.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	04-27-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <showitem.h>

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
// 	Function Name:	CShowItem::CShowItem()
//
// 	Description:	This is the constructor for CShowItem objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CShowItem::CShowItem() : CSecondary()
{
	m_bAutoTransition   = FALSE;
	m_bStaticScene		= TRUE;
	m_bTransitioned		= FALSE;
	m_lTransitionPeriod = -1L;
	m_ulGoToNext		= 0;
	m_sSourceMediaType  = 0;
	m_strItemBarcode.Empty();
	m_strSourcePST.Empty();
}

//==============================================================================
//
// 	Function Name:	CShowItem::~CShowItem()
//
// 	Description:	This is the destructor for CShowItem objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CShowItem::~CShowItem()
{
}

//==============================================================================
//
// 	Function Name:	CShowItem::operator < ()
//
// 	Description:	This is an overloaded version of the < operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			None
//
//==============================================================================
BOOL CShowItem::operator < (const CShowItem& Compare)
{
	return (m_lPlaybackOrder < Compare.m_lPlaybackOrder);
}

//==============================================================================
//
// 	Function Name:	CShowItem::operator == ()
//
// 	Description:	This is an overloaded version of the == operator.
//
// 	Returns:		TRUE if the objects are equal.
//
//	Notes:			None
//
//==============================================================================
BOOL CShowItem::operator == (const CShowItem& Compare)
{
	return (m_lPlaybackOrder == Compare.m_lPlaybackOrder);
}

//==============================================================================
//
// 	Function Name:	CShowItems::Add()
//
// 	Description:	This function will add an object to the list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CShowItems::Add(CShowItem* pShowItem, BOOL bSorted)
{
	POSITION	Pos;
	POSITION	Prev;
	CShowItem*	pCurrent;

	//	Add at the head of the list if the list is empty
	if(IsEmpty())
	{
		AddHead(pShowItem);
		return;
	}

	//	Look for the correct position if sorting on entry
	if(bSorted)
	{
		Pos = GetHeadPosition();
		Prev = Pos;
		while(Pos != NULL)
		{
			if((pCurrent = (CShowItem*)GetNext(Pos)) == NULL)
				continue;

			if(*pShowItem < *pCurrent)
			{
				InsertBefore(Prev, pShowItem);
				return;
			}
			
			Prev = Pos;	
		}
	}

	//	If we made it this far we must have to add it to the end of the list
	AddTail(pShowItem);
}

//==============================================================================
//
// 	Function Name:	CShowItem::CShowItems()
//
// 	Description:	This is the constructor for CShowItems objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CShowItems::CShowItems()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CShowItem::~CShowItems()
//
// 	Description:	This is the destructor for CShowItems objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CShowItems::~CShowItems()
{
	//	Flush the list and destroy it's objects
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CShowItems::Find()
//
// 	Description:	This function will search the list for the item with the
//					item barcode specified by the caller.
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			The local position markers are not modified with a call to
//					this function.
//
//==============================================================================
CShowItem* CShowItems::Find(LPCSTR lpItemBarcode)
{
	POSITION	Pos;
	CShowItem*	pShowItem;

	//	Get the first position
	Pos = GetHeadPosition();

	//	Check each item until we find a match
	while(Pos != NULL)
	{
		if((pShowItem = (CShowItem*)GetNext(Pos)) != 0)
		{
			if(lstrcmpi(pShowItem->m_strItemBarcode, lpItemBarcode) == 0)
				return pShowItem;
		}
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CShowItems::Find()
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
CShowItem* CShowItems::Find(long lId, BOOL bUsePlayback)
{
	POSITION	Pos;
	CShowItem*	pShowItem;

	//	Get the first position
	Pos = GetHeadPosition();

	//	Check each item until we find a match
	while(Pos != NULL)
	{
		if((pShowItem = (CShowItem*)GetNext(Pos)) != 0)
		{
			//	Are we using the playback order for comparison?
			if(bUsePlayback)
			{
				if(pShowItem->m_lPlaybackOrder == lId)
					return pShowItem;
			}
			else
			{
				if(pShowItem->m_lBarcodeId == lId)
					return pShowItem;
			}
		}
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CShowItems::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CShowItems::Find(CShowItem* pShowItem)
{
	return (CObList::Find(pShowItem));
}

//==============================================================================
//
// 	Function Name:	CShowItems::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CShowItem* CShowItems::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CShowItem*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CShowItems::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CShowItems::Flush(BOOL bDelete)
{
	CShowItem* pShowItem;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pShowItem = (CShowItem*)GetNext(m_NextPos)) != 0)
				delete pShowItem;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CShowItems::IsFirst()
//
// 	Description:	This function is called to determine if the object specified
//					by the caller is the first one in the list.
//
// 	Returns:		TRUE if first in list
//
//	Notes:			None
//
//==============================================================================
BOOL CShowItems::IsFirst(CShowItem* pShowItem)
{
	POSITION Pos = GetHeadPosition();

	if(Pos != NULL)
		return (pShowItem == (CShowItem*)GetNext(Pos));
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CShowItems::IsLast()
//
// 	Description:	This function is called to determine if the object specified
//					by the caller is the last one in the list.
//
// 	Returns:		TRUE if last in list
//
//	Notes:			None
//
//==============================================================================
BOOL CShowItems::IsLast(CShowItem* pShowItem)
{
	POSITION Pos = GetTailPosition();

	if(Pos != NULL)
		return (pShowItem == (CShowItem*)GetPrev(Pos));
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CShowItems::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CShowItem* CShowItems::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CShowItem*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CShowItems::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CShowItem* CShowItems::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CShowItem*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CShowItems::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CShowItem* CShowItems::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CShowItem*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CShowItems::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CShowItems::Remove(CShowItem* pShowItem, BOOL bDelete)
{
	POSITION Pos = Find(pShowItem);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pShowItem;
	}
}

