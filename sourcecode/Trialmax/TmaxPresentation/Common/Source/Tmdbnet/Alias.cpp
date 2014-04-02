//==============================================================================
//
// File Name:	alias.cpp
//
// Description:	This file contains member functions of the CAlias and
//				CAliases classes
//
// See Also:	alias.h
//
// Copyright	FTI Consulting 1997-2004
//
//==============================================================================
//	Date		Revision    Description
//	01-31-04	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <alias.h>

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
// 	Function Name:	CAlias::CAlias()
//
// 	Description:	This is the constructor for CAlias objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CAlias::CAlias() : CObject()
{
	m_lId = 0;
	m_strCurrent.Empty();
	m_strPrevious.Empty();
	m_strOriginal.Empty();
}

//==============================================================================
//
// 	Function Name:	CAlias::~CAlias()
//
// 	Description:	This is the destructor for CAlias objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CAlias::~CAlias()
{
}

//==============================================================================
//
// 	Function Name:	CAlias::operator < ()
//
// 	Description:	This is an overloaded version of the < operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			None
//
//==============================================================================
BOOL CAlias::operator < (const CAlias& Compare)
{
	return (m_lId < Compare.m_lId);
}

//==============================================================================
//
// 	Function Name:	CAlias::operator == ()
//
// 	Description:	This is an overloaded version of the == operator.
//
// 	Returns:		TRUE if the objects are equal.
//
//	Notes:			None
//
//==============================================================================
BOOL CAlias::operator == (const CAlias& Compare)
{
	return (m_lId == Compare.m_lId);
}

//==============================================================================
//
// 	Function Name:	CAliases::Add()
//
// 	Description:	This function will add an object to the list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAliases::Add(CAlias* pAlias, BOOL bSorted)
{
	POSITION	Pos;
	POSITION	Prev;
	CAlias*		pCurrent;

	//	Add at the head of the list if the list is empty
	if(IsEmpty())
	{
		AddHead(pAlias);
		return;
	}

	//	Look for the correct position if sorting on entry
	if(bSorted)
	{
		Pos = GetHeadPosition();
		Prev = Pos;
		while(Pos != NULL)
		{
			if((pCurrent = (CAlias*)GetNext(Pos)) == NULL)
				continue;

			if(*pAlias < *pCurrent)
			{
				InsertBefore(Prev, pAlias);
				return;
			}
			
			Prev = Pos;	
		}
	}

	//	If we made it this far we must have to add it to the end of the list
	AddTail(pAlias);
}

//==============================================================================
//
// 	Function Name:	CAlias::CAliases()
//
// 	Description:	This is the constructor for CAliases objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CAliases::CAliases()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CAlias::~CAliases()
//
// 	Description:	This is the destructor for CAliases objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CAliases::~CAliases()
{
	//	Flush the list and destroy it's objects
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CAliases::Find()
//
// 	Description:	This function will search the list for the item with the
//					current value specified by the caller.
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			The local position markers are not modified with a call to
//					this function.
//
//==============================================================================
CAlias* CAliases::Find(LPCSTR lpszCurrent)
{
	POSITION	Pos;
	CAlias*		pAlias;

	//	Get the first position
	Pos = GetHeadPosition();

	//	Check each item until we find a match
	while(Pos != NULL)
	{
		if((pAlias = (CAlias*)GetNext(Pos)) != 0)
		{
			if(lstrcmpi(pAlias->m_strCurrent, lpszCurrent) == 0)
				return pAlias;
		}
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CAliases::Find()
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
CAlias* CAliases::Find(long lId)
{
	POSITION	Pos;
	CAlias*		pAlias;

	//	Get the first position
	Pos = GetHeadPosition();

	//	Check each item until we find a match
	while(Pos != NULL)
	{
		if((pAlias = (CAlias*)GetNext(Pos)) != 0)
		{
			if(pAlias->m_lId == lId)
				return pAlias;
		}
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CAliases::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CAliases::Find(CAlias* pAlias)
{
	return (CObList::Find(pAlias));
}

//==============================================================================
//
// 	Function Name:	CAliases::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CAlias* CAliases::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CAlias*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CAliases::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CAliases::Flush(BOOL bDelete)
{
	CAlias* pAlias;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pAlias = (CAlias*)GetNext(m_NextPos)) != 0)
				delete pAlias;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CAliases::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CAlias* CAliases::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CAlias*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CAliases::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CAlias* CAliases::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CAlias*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CAliases::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CAlias* CAliases::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CAlias*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CAliases::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAliases::Remove(CAlias* pAlias, BOOL bDelete)
{
	POSITION Pos = Find(pAlias);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pAlias;
	}
}

