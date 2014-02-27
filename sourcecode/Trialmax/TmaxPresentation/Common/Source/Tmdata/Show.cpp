//==============================================================================
//
// File Name:	show.cpp
//
// Description:	This file contains member functions of the CShow and
//				CShows classes
//
// See Also:	show.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-03-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <show.h>

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
// 	Function Name:	CShow::CShow()
//
// 	Description:	This is the constructor for CShow objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CShow::CShow(CMedia* pMedia) : CMedia(pMedia)
{
}

//==============================================================================
//
// 	Function Name:	CShow::~CShow()
//
// 	Description:	This is the destructor for CShow objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CShow::~CShow()
{
	//	Flush the item list
	m_Items.Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CShow::FindByBarcodeId()
//
// 	Description:	This function is called to locate the show item with the
//					database id specified by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CShowItem* CShow::FindByBarcodeId(long lId)
{
	CShowItem* pItem = m_Items.First();
	while(pItem)
	{
		if(pItem->m_lBarcodeId == lId)
			return pItem;
		pItem = m_Items.Next();
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CShow::FindByDatabaseId()
//
// 	Description:	This function is called to locate the show item with the
//					database id specified by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CShowItem* CShow::FindByDatabaseId(long lId)
{
	CShowItem* pItem = m_Items.First();
	while(pItem)
	{
		if(pItem->m_lPrimaryId == lId)
			return pItem;
		pItem = m_Items.Next();
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CShow::FindByOrder()
//
// 	Description:	This function is called to locate the show item with the
//					playback order specified by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CShowItem* CShow::FindByOrder(long lOrder)
{
	CShowItem* pItem = m_Items.First();
	while(pItem)
	{
		if(pItem->m_lPlaybackOrder == lOrder)
			return pItem;
		pItem = m_Items.Next();
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CShows::Add()
//
// 	Description:	This function will add an object to the list
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CShows::Add(CShow* pShow)
{
	ASSERT(pShow);
	if(!pShow)
		return FALSE;

	//	MFC will throw a memory exception if it can't add the object to the list
	try
	{
		//	Add the link to the end of the list
		AddTail(pShow);
		return TRUE;
	}
	catch(...)
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CShow::CShows()
//
// 	Description:	This is the constructor for CShows objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CShows::CShows()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CShow::~CShows()
//
// 	Description:	This is the destructor for CShows objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CShows::~CShows()
{
	//	Flush the list and destroy it's objects
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CShows::Find()
//
// 	Description:	This function will search the list for the item with the
//					text identifier specified by the caller.
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			The local position markers are not modified with a call to
//					this function.
//
//==============================================================================
CShow* CShows::Find(LPCSTR lpId)
{
	POSITION	Pos;
	CShow*		pShow;

	//	Get the first position
	Pos = GetHeadPosition();

	//	Check each item until we find a match
	while(Pos != NULL)
	{
		if((pShow = (CShow*)GetNext(Pos)) != 0)
		{
			if(lstrcmpi(pShow->m_strMediaId, lpId) == 0)
				return pShow;
		}
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CShows::Find()
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
CShow* CShows::Find(long lId)
{
	POSITION	Pos;
	CShow*		pShow;

	//	Get the first position
	Pos = GetHeadPosition();

	//	Check each item until we find a match
	while(Pos != NULL)
	{
		if((pShow = (CShow*)GetNext(Pos)) != 0)
		{
			if(pShow->m_lPrimaryId == lId)
				return pShow;
		}
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CShows::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CShows::Find(CShow* pShow)
{
	return (CObList::Find(pShow));
}

//==============================================================================
//
// 	Function Name:	CShows::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CShow* CShows::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CShow*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CShows::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CShows::Flush(BOOL bDelete)
{
	CShow* pShow;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pShow = (CShow*)GetNext(m_NextPos)) != 0)
				delete pShow;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CShows::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CShow* CShows::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CShow*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CShows::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CShow* CShows::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CShow*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CShows::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CShow* CShows::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CShow*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CShows::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CShows::Remove(CShow* pShow, BOOL bDelete)
{
	POSITION Pos = Find(pShow);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pShow;
	}
}

