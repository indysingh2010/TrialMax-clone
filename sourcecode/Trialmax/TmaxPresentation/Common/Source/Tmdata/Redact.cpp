//==============================================================================
//
// File Name:	redact.cpp
//
// Description:	This file contains member functions of the CRedaction and 
//				CRedactions	classes.
//
// See Also:	redact.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	06-04-03	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <redact.h>

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
// 	Function Name:	CRedaction::CRedaction()
//
// 	Description:	This is the constructor for CRedaction objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CRedaction::CRedaction()
{
	//	Initialize the class members
	m_lId			= 0;
	m_lPage			= 0;
	m_lAnnRedaction	= 0;
	m_lAnnLabel		= 0;
	m_sFontSize		= 6;
	m_bOpaque		= TRUE;
	m_crRedaction   = RGB(0x00,0x00,0x00);
	m_crLabel		= RGB(0xFF,0xFF,0xFF);
	m_strFontName   = "Arial";
	m_strLabel.Empty();
	memset(&m_rcBounds, 0, sizeof(m_rcBounds));
}

//==============================================================================
//
// 	Function Name:	CRedaction::~CRedaction()
//
// 	Description:	This is the destructor for CRedaction objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CRedaction::~CRedaction()
{
}

//==============================================================================
//
// 	Function Name:	CRedaction::operator < ()
//
// 	Description:	This is an overloaded version of the < operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			CRedaction objects are sorted by the frame. This function assumes
//					the link objects reference the same designation.
//
//==============================================================================
BOOL CRedaction::operator < (const CRedaction& Compare)
{
	return m_lId < Compare.m_lId;
}

//==============================================================================
//
// 	Function Name:	CRedaction::operator == ()
//
// 	Description:	This is an overloaded version of the == operator.
//
// 	Returns:		TRUE if the objects are equal.
//
//	Notes:			None
//
//==============================================================================
BOOL CRedaction::operator == (const CRedaction& Compare)
{
	return (m_lId == Compare.m_lId);
}

//==============================================================================
//
// 	Function Name:	CRedactions::Add()
//
// 	Description:	This function will add the object pointer to the list in the
//					correct position based on the current sort order. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CRedactions::Add(CRedaction* pRedaction)
{
	POSITION	Pos;
	POSITION	Prev;
	CRedaction*	pCurrent;

	//	Add at the head of the list if the list is empty
	if(IsEmpty())
	{
		AddHead(pRedaction);
		return;
	}

	//	Look for the correct position
	Pos = GetHeadPosition();
	Prev = Pos;
	while(Pos != NULL)
	{
		if((pCurrent = (CRedaction*)GetNext(Pos)) == NULL)
			continue;

		//	Are we sorting in ascending order?
		if(m_bAscending)
		{
			if(*pRedaction < *pCurrent)
			{
				InsertBefore(Prev, pRedaction);
				return;
			}
		}
		else
		{
			if(*pCurrent < *pRedaction)
			{
				InsertBefore(Prev, pRedaction);
				return;
			}
		}
		
		Prev = Pos;	
	}

	//	If we made it this far we must have to add it to the end of the list
	AddTail(pRedaction);

}

//==============================================================================
//
// 	Function Name:	CRedactions::CRedactions()
//
// 	Description:	This is the constructor for CRedactions objects. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CRedactions::CRedactions(BOOL bAscending, int iAllocSize) : CObList(iAllocSize)
{
	//	Initialize the local members
	m_bAscending = bAscending;
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

///==============================================================================
//
// 	Function Name:	CRedactions::~CRedactions()
//
// 	Description:	This is the destructor for CRedactions objects. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CRedactions::~CRedactions()
{
	//	Flush the list
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CRedactions::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CRedactions::Find(CRedaction* pRedaction)
{
	return (CObList::Find(pRedaction));
}

//==============================================================================
//
// 	Function Name:	CRedactions::Find()
//
// 	Description:	This function will retrieve the link with the id specified
//					by the caller.
//
// 	Returns:		NULL if not found
//
//	Notes:			None
//
//==============================================================================
CRedaction* CRedactions::Find(long lId)
{
	POSITION	Pos;
	CRedaction*	pRedaction;

	//	Get the first position
	Pos = GetHeadPosition();
	while(Pos != NULL)
	{
		pRedaction = (CRedaction*)GetNext(Pos);

		if(pRedaction && (pRedaction->m_lId == lId))
			return pRedaction;
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CRedactions::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CRedaction* CRedactions::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CRedaction*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CRedactions::Flush()
//
// 	Description:	This function will flush all CRedaction objects from the 
//					list. If bDeleteAll is TRUE, the objects are deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CRedactions::Flush(BOOL bDeleteAll)
{
	//	Do we want to delete the objects?
	if(bDeleteAll)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			CRedaction* pRedaction = (CRedaction*)CObList::GetNext(m_NextPos);
			if(pRedaction != NULL)
				delete pRedaction;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CRedactions::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CRedaction* CRedactions::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CRedaction*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CRedactions::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CRedaction* CRedactions::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CRedaction*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CRedactions::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CRedaction* CRedactions::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CRedaction*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CRedactions::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CRedactions::Remove(CRedaction* pRedaction, BOOL bDelete)
{
	POSITION Pos = Find(pRedaction);

	//	Is this object in the list
	if(Pos != NULL)
		RemoveAt(Pos);

	//	Do we need to delete the object?
	if(bDelete)
		delete pRedaction;
}

