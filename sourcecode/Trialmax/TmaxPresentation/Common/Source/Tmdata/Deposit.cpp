//==============================================================================
//
// File Name:	deposit.cpp
//
// Description:	This file contains member functions of the CDeposition and
//				CDepositions classes
//
// See Also:	deposit.h
//
// Copyright	FTI Consulting 1997-2008
//
//==============================================================================
//	Date		Revision    Description
//	07-10-08	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <deposit.h>

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
// 	Function Name:	CDeposition::CDeposition()
//
// 	Description:	This is the constructor for CDeposition objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDeposition::CDeposition(CMedia* pMedia) : CMedia(pMedia)
{
	m_pTranscript = NULL;
}

//==============================================================================
//
// 	Function Name:	CDeposition::~CDeposition()
//
// 	Description:	This is the destructor for CDeposition objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDeposition::~CDeposition()
{
	//	Flush the list of secondary segments
	m_Segments.Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CDepositions::Add()
//
// 	Description:	This function will add an object to the list
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CDepositions::Add(CDeposition* pDeposition)
{
	ASSERT(pDeposition);
	if(!pDeposition)
		return FALSE;

	//	MFC will throw a memory exception if it can't add the object to the list
	try
	{
		//	Add the link to the end of the list
		AddTail(pDeposition);
		return TRUE;
	}
	catch(...)
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CDeposition::CDepositions()
//
// 	Description:	This is the constructor for CDepositions objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDepositions::CDepositions()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CDeposition::~CDepositions()
//
// 	Description:	This is the destructor for CDepositions objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDepositions::~CDepositions()
{
	//	Flush the list and destroy it's objects
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CDepositions::Find()
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
CDeposition* CDepositions::Find(LPCSTR lpszMediaId)
{
	POSITION		Pos;
	CDeposition*	pDeposition;

	//	Get the first position
	Pos = GetHeadPosition();

	//	Check each item until we find a match
	while(Pos != NULL)
	{
		if((pDeposition = (CDeposition*)GetNext(Pos)) != 0)
		{
			if(lstrcmpi(pDeposition->m_strMediaId, lpszMediaId) == 0)
				return pDeposition;
		}
	}

	return NULL;
}

//==============================================================================
//
// 	Function Name:	CDepositions::Find()
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
CDeposition* CDepositions::Find(long lDatabaseId)
{
	POSITION		Pos;
	CDeposition*	pDeposition;

	//	Get the first position
	Pos = GetHeadPosition();

	//	Check each item until we find a match
	while(Pos != NULL)
	{
		if((pDeposition = (CDeposition*)GetNext(Pos)) != 0)
		{
			if(pDeposition->m_lPrimaryId == lDatabaseId)
				return pDeposition;
		}
	}

	return NULL;
}

//==============================================================================
//
// 	Function Name:	CDepositions::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CDepositions::Find(CDeposition* pDeposition)
{
	return (CObList::Find(pDeposition));
}

//==============================================================================
//
// 	Function Name:	CDepositions::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CDeposition* CDepositions::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CDeposition*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CDepositions::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CDepositions::Flush(BOOL bDelete)
{
	CDeposition* pDeposition;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pDeposition = (CDeposition*)GetNext(m_NextPos)) != 0)
				delete pDeposition;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CDepositions::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CDeposition* CDepositions::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CDeposition*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CDepositions::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CDeposition* CDepositions::Next()
{	
	if(m_NextPos == NULL)
	{
		return NULL;
	}
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CDeposition*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CDepositions::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CDeposition* CDepositions::Prev()
{
	if(m_PrevPos == NULL)
	{
		return NULL;
	}
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CDeposition*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CDepositions::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDepositions::Remove(CDeposition* pDeposition, BOOL bDelete)
{
	POSITION Pos = Find(pDeposition);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pDeposition;
	}
}

