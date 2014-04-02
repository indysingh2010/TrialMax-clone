//==============================================================================
//
// File Name:	xmlact.cpp
//
// Description:	This file contains member functions of the CXmlAction and
//				CXmlActions classes
//
// See Also:	xmlact.h
//
// Copyright	FTI Consulting 1997-2001
//
//==============================================================================
//	Date		Revision    Description
//	05-06-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <xmlact.h>

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
// 	Function Name:	CXmlAction::CXmlAction()
//
// 	Description:	This is the constructor for CXmlAction objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlAction::CXmlAction()
{
	m_strCommand.Empty();
	m_strOnComplete.Empty();
	m_strPageId.Empty();
	m_strTree.Empty();
}

//==============================================================================
//
// 	Function Name:	CXmlAction::~CXmlAction()
//
// 	Description:	This is the destructor for CXmlAction objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlAction::~CXmlAction()
{
}

//==============================================================================
//
// 	Function Name:	CXmlAction::operator < ()
//
// 	Description:	This is an overloaded version of the < operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlAction::operator < (const CXmlAction& Compare)
{
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlAction::operator == ()
//
// 	Description:	This is an overloaded version of the == operator.
//
// 	Returns:		TRUE if the objects are equal.
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlAction::operator == (const CXmlAction& Compare)
{
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlAction::SetAttribute()
//
// 	Description:	This function will set the specified attribute to the 
//					requested value.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlAction::SetAttribute(LPCSTR lpAttribute, LPCSTR lpValue)
{
	ASSERT(lpAttribute);
	ASSERT(lpValue);

	if(lstrcmpi(lpAttribute, TMXML_ACTION_COMMAND) == 0)
	{
		m_strCommand = lpValue;
		return TRUE;
	}
	else if(lstrcmpi(lpAttribute, TMXML_ACTION_TREE) == 0)
	{
		m_strTree = lpValue;
		return TRUE;
	}
	else if(lstrcmpi(lpAttribute, TMXML_ACTION_PAGE) == 0)
	{
		m_strPageId = lpValue;
		return TRUE;
	}
	else if(lstrcmpi(lpAttribute, TMXML_ACTION_ONCOMPLETE) == 0)
	{
		m_strOnComplete = lpValue;
		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlActions::Add()
//
// 	Description:	This function will add an object to the list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlActions::Add(CXmlAction* pAction)
{
	POSITION	Pos;
	POSITION	Prev;
	CXmlAction*	pCurrent;

	//	Add at the head of the list if the list is empty
	if(IsEmpty())
	{
		AddHead(pAction);
		return;
	}

	//	Look for the correct position
	Pos = GetHeadPosition();
	Prev = Pos;
	while(Pos != NULL)
	{
		if((pCurrent = (CXmlAction*)GetNext(Pos)) == NULL)
			continue;

		if(*pAction < *pCurrent)
		{
			InsertBefore(Prev, pAction);
			return;
		}
		
		Prev = Pos;	
	}

	//	If we made it this far we must have to add it to the end of the list
	AddTail(pAction);
}

//==============================================================================
//
// 	Function Name:	CXmlAction::CXmlActions()
//
// 	Description:	This is the constructor for CXmlActions objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlActions::CXmlActions()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CXmlAction::~CXmlActions()
//
// 	Description:	This is the destructor for CXmlActions objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlActions::~CXmlActions()
{
	//	Flush the list and destroy it's objects
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CXmlActions::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CXmlActions::Find(CXmlAction* pAction)
{
	return (CObList::Find(pAction));
}

//==============================================================================
//
// 	Function Name:	CXmlActions::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CXmlAction* CXmlActions::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CXmlAction*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CXmlActions::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CXmlActions::Flush(BOOL bDelete)
{
	CXmlAction* pAction;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pAction = (CXmlAction*)GetNext(m_NextPos)) != 0)
				delete pAction;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CXmlActions::IsFirst()
//
// 	Description:	This function is called to determine if the object specified
//					by the caller is the first one in the list.
//
// 	Returns:		TRUE if first in list
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlActions::IsFirst(CXmlAction* pAction)
{
	POSITION Pos = GetHeadPosition();

	if(Pos != NULL)
		return (pAction == (CXmlAction*)GetNext(Pos));
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlActions::IsLast()
//
// 	Description:	This function is called to determine if the object specified
//					by the caller is the last one in the list.
//
// 	Returns:		TRUE if last in list
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlActions::IsLast(CXmlAction* pAction)
{
	POSITION Pos = GetTailPosition();

	if(Pos != NULL)
		return (pAction == (CXmlAction*)GetPrev(Pos));
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlActions::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CXmlAction* CXmlActions::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CXmlAction*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CXmlActions::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CXmlAction* CXmlActions::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CXmlAction*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CXmlActions::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CXmlAction* CXmlActions::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CXmlAction*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CXmlActions::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlActions::Remove(CXmlAction* pAction, BOOL bDelete)
{
	POSITION Pos = Find(pAction);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pAction;
	}
}

