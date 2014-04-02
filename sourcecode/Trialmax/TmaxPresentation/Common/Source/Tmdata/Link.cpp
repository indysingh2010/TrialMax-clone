//==============================================================================
//
// File Name:	link.cpp
//
// Description:	This file contains member functions of the CLink and CLinks
//				classes.
//
// See Also:	link.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-11-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <link.h>

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
// 	Function Name:	CLink::CLink()
//
// 	Description:	This is the constructor for CLink objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CLink::CLink()
{
	//	Initialize the class members
	m_lId			= 0;
	m_lOwnerId		= 0;
	m_lDisplayType	= 0;
	m_lPage			= 0;
	m_lLine			= 0;
	m_dTrigger		= 0.0;
	m_lFlags		= 0;
	m_bHide			= FALSE;
	m_bSplitScreen	= FALSE;
	m_strPST.Empty();
	m_strMediaId.Empty();
	m_strItemBarcode.Empty();
}

//==============================================================================
//
// 	Function Name:	CLink::CLink()
//
// 	Description:	This is the copy constructor for CLink objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CLink::CLink(const CLink& rLink)
{
	m_lId			 = rLink.m_lId;
	m_lOwnerId		 = rLink.m_lOwnerId;
	m_lDisplayType	 = rLink.m_lDisplayType;
	m_lPage			 = rLink.m_lPage;
	m_lLine			 = rLink.m_lLine;
	m_dTrigger		 = rLink.m_dTrigger;
	m_bHide			 = rLink.m_bHide;
	m_bSplitScreen	 = rLink.m_bSplitScreen;
	m_strMediaId	 = rLink.m_strMediaId;
	m_strItemBarcode = rLink.m_strItemBarcode;
	m_strPST		 = rLink.m_strPST;
}

//==============================================================================
//
// 	Function Name:	CLink::~CLink()
//
// 	Description:	This is the destructor for CLink objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CLink::~CLink()
{
}

//==============================================================================
//
// 	Function Name:	CLink::operator < ()
//
// 	Description:	This is an overloaded version of the < operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			CLink objects are sorted by the frame. This function assumes
//					the link objects reference the same designation.
//
//==============================================================================
BOOL CLink::operator < (const CLink& Compare)
{
	if(m_lOwnerId == Compare.m_lOwnerId)
		return m_dTrigger < Compare.m_dTrigger;
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CLink::operator == ()
//
// 	Description:	This is an overloaded version of the == operator.
//
// 	Returns:		TRUE if the objects are equal.
//
//	Notes:			None
//
//==============================================================================
BOOL CLink::operator == (const CLink& Compare)
{
	return ((m_lOwnerId == Compare.m_lOwnerId) &&
		    (m_dTrigger == Compare.m_dTrigger));
}

//==============================================================================
//
// 	Function Name:	CLinks::Add()
//
// 	Description:	This function will add the object pointer to the list in the
//					correct position based on the current sort order. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CLinks::Add(CLink* pLink)
{
	POSITION	Pos;
	POSITION	Prev;
	CLink*		pCurrent;

	//	Add at the head of the list if the list is empty
	if(IsEmpty())
	{
		AddHead(pLink);
		return;
	}

	//	Look for the correct position
	Pos = GetHeadPosition();
	Prev = Pos;
	while(Pos != NULL)
	{
		if((pCurrent = (CLink*)GetNext(Pos)) == NULL)
			continue;

		//	Are we sorting in ascending order?
		if(m_bAscending)
		{
			if(*pLink < *pCurrent)
			{
				InsertBefore(Prev, pLink);
				return;
			}
		}
		else
		{
			if(*pCurrent < *pLink)
			{
				InsertBefore(Prev, pLink);
				return;
			}
		}
		
		Prev = Pos;	
	}

	//	If we made it this far we must have to add it to the end of the list
	AddTail(pLink);

}

//==============================================================================
//
// 	Function Name:	CLinks::CLinks()
//
// 	Description:	This is the constructor for CLinks objects. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CLinks::CLinks(BOOL bAscending, int iAllocSize) : CObList(iAllocSize)
{
	//	Initialize the local members
	m_bAscending = bAscending;
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

///==============================================================================
//
// 	Function Name:	CLinks::~CLinks()
//
// 	Description:	This is the destructor for CLinks objects. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CLinks::~CLinks()
{
	//	Flush the list
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CLinks::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CLinks::Find(CLink* pLink)
{
	return (CObList::Find(pLink));
}

//==============================================================================
//
// 	Function Name:	CLinks::Find()
//
// 	Description:	This function will retrieve the link with the id specified
//					by the caller.
//
// 	Returns:		NULL if not found
//
//	Notes:			None
//
//==============================================================================
CLink* CLinks::Find(long lId)
{
	POSITION	Pos;
	CLink*		pLink;

	//	Get the first position
	Pos = GetHeadPosition();
	while(Pos != NULL)
	{
		pLink = (CLink*)GetNext(Pos);

		if(pLink && (pLink->m_lId == lId))
			return pLink;
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CLinks::FindAtPosition()
//
// 	Description:	This function will retrieve the link that corresponds to
//					the specified position
//
// 	Returns:		NULL if not found
//
//	Notes:			None
//
//==============================================================================
CLink* CLinks::FindAtPosition(double dPosition)
{
	POSITION	Pos;
	CLink*		pPrev = 0;
	CLink*		pLink = 0;

	//	Is the list empty?
	if(GetCount() == 0) return 0;

	//	Search for the link
	Pos = GetHeadPosition();
	while(Pos != NULL)
	{
		if((pLink = (CLink*)GetNext(Pos)) != 0)
		{
			if(pLink->m_dTrigger > dPosition)
				return pPrev;
			else
				pPrev = pLink;
		}

	}
	
	//	Must be the last in the list
	return ((CLink*)GetTail());
}

//==============================================================================
//
// 	Function Name:	CLinks::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CLink* CLinks::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CLink*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CLinks::Flush()
//
// 	Description:	This function will flush all CLink objects from the 
//					list. If bDeleteAll is TRUE, the objects are deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CLinks::Flush(BOOL bDeleteAll)
{
	//	Do we want to delete the objects?
	if(bDeleteAll)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			CLink* pLink = (CLink*)CObList::GetNext(m_NextPos);
			if(pLink != NULL)
				delete pLink;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CLinks::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CLink* CLinks::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CLink*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CLinks::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CLink* CLinks::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CLink*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CLinks::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CLink* CLinks::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CLink*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CLinks::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CLinks::Remove(CLink* pLink, BOOL bDelete)
{
	POSITION Pos = Find(pLink);

	//	Is this object in the list
	if(Pos != NULL)
		RemoveAt(Pos);

	//	Do we need to delete the object?
	if(bDelete)
		delete pLink;
}

//==============================================================================
//
// 	Function Name:	CLinks::SetPos()
//
// 	Description:	This function will set the position of the local iterator
//					on the line specified by the caller
//
// 	Returns:		A pointer to object if found
//
//	Notes:			None
//
//==============================================================================
CLink* CLinks::SetPos(CLink* pLink)
{
	CLink* pCurrent;

	pCurrent = First();
	while(pCurrent)
	{
		if(pCurrent == pLink)
			return pLink;
		else
			pCurrent = Next();
	}

	return 0;
}

