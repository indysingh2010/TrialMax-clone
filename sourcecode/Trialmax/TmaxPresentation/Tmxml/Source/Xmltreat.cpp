//==============================================================================
//
// File Name:	xmltreat.cpp
//
// Description:	This file contains member functions of the CXmlTreatment and
//				CXmlTreatments classes
//
// See Also:	xmltreat.h
//
// Copyright	FTI Consulting 1997-2001
//
//==============================================================================
//	Date		Revision    Description
//	05-05-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <xmltreat.h>

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
// 	Function Name:	CXmlTreatment::CXmlTreatment()
//
// 	Description:	This is the constructor for CXmlTreatment objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlTreatment::CXmlTreatment(CXmlPage* pPage) : CMediaCtrlItem()
{
	m_pXmlPage = pPage;
	m_strSource.Empty();
	m_strId.Empty();
}

//==============================================================================
//
// 	Function Name:	CXmlTreatment::~CXmlTreatment()
//
// 	Description:	This is the destructor for CXmlTreatment objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlTreatment::~CXmlTreatment()
{
}

//==============================================================================
//
// 	Function Name:	CXmlTreatment::operator < ()
//
// 	Description:	This is an overloaded version of the < operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlTreatment::operator < (const CXmlTreatment& Compare)
{
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlTreatment::operator == ()
//
// 	Description:	This is an overloaded version of the == operator.
//
// 	Returns:		TRUE if the objects are equal.
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlTreatment::operator == (const CXmlTreatment& Compare)
{
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlTreatment::SetAttribute()
//
// 	Description:	This function will set the specified attribute to the 
//					requested value.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlTreatment::SetAttribute(LPCSTR lpAttribute, LPCSTR lpValue)
{
	ASSERT(lpAttribute);
	ASSERT(lpValue);

	if(lstrcmpi(lpAttribute, TMXML_TREATMENT_SOURCE) == 0)
	{
		m_strSource = lpValue;
		return TRUE;
	}
	if(lstrcmpi(lpAttribute, TMXML_TREATMENT_ID) == 0)
	{
		m_strId = lpValue;
		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlTreatment::SetAttributes()
//
// 	Description:	This function will set the attributes to match those of
//					the source object provided by the caller.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlTreatment::SetAttributes(const CXmlTreatment& rSource)
{
	m_strSource = rSource.m_strSource;
	m_strId = rSource.m_strId;
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CXmlTreatments::Add()
//
// 	Description:	This function will add an object to the list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlTreatments::Add(CXmlTreatment* pTreatment)
{
	POSITION		Pos;
	POSITION		Prev;
	CXmlTreatment*	pCurrent;

	//	Add at the head of the list if the list is empty
	if(IsEmpty())
	{
		AddHead(pTreatment);
		return;
	}

	//	Look for the correct position
	Pos = GetHeadPosition();
	Prev = Pos;
	while(Pos != NULL)
	{
		if((pCurrent = (CXmlTreatment*)GetNext(Pos)) == NULL)
			continue;

		if(*pTreatment < *pCurrent)
		{
			InsertBefore(Prev, pTreatment);
			return;
		}
		
		Prev = Pos;	
	}

	//	If we made it this far we must have to add it to the end of the list
	AddTail(pTreatment);
}

//==============================================================================
//
// 	Function Name:	CXmlTreatment::CXmlTreatments()
//
// 	Description:	This is the constructor for CXmlTreatments objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlTreatments::CXmlTreatments()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CXmlTreatment::~CXmlTreatments()
//
// 	Description:	This is the destructor for CXmlTreatments objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlTreatments::~CXmlTreatments()
{
	//	Flush the list and destroy it's objects
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CXmlTreatments::Find()
//
// 	Description:	This function will find the treatment object that has the
//					specified id.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
CXmlTreatment* CXmlTreatments::Find(LPCSTR lpId)
{
	POSITION		Pos;
	CXmlTreatment*	pTreatment;

	ASSERT(lpId);

	Pos = GetHeadPosition();
	while(Pos != NULL)
	{
		if((pTreatment = (CXmlTreatment*)GetNext(Pos)) != 0)
		{
			if(lstrcmpi(pTreatment->m_strId, lpId) == 0)
				return pTreatment;
		}
	}
	return NULL;
}

//==============================================================================
//
// 	Function Name:	CXmlTreatments::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CXmlTreatments::Find(CXmlTreatment* pTreatment)
{
	return (CObList::Find(pTreatment));
}

//==============================================================================
//
// 	Function Name:	CXmlTreatments::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CXmlTreatment* CXmlTreatments::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CXmlTreatment*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CXmlTreatments::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CXmlTreatments::Flush(BOOL bDelete)
{
	CXmlTreatment* pTreatment;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pTreatment = (CXmlTreatment*)GetNext(m_NextPos)) != 0)
				delete pTreatment;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CXmlTreatments::IsFirst()
//
// 	Description:	This function is called to determine if the object specified
//					by the caller is the first one in the list.
//
// 	Returns:		TRUE if first in list
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlTreatments::IsFirst(CXmlTreatment* pTreatment)
{
	POSITION Pos = GetHeadPosition();

	if(Pos != NULL)
		return (pTreatment == (CXmlTreatment*)GetNext(Pos));
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlTreatments::IsLast()
//
// 	Description:	This function is called to determine if the object specified
//					by the caller is the last one in the list.
//
// 	Returns:		TRUE if last in list
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlTreatments::IsLast(CXmlTreatment* pTreatment)
{
	POSITION Pos = GetTailPosition();

	if(Pos != NULL)
		return (pTreatment == (CXmlTreatment*)GetPrev(Pos));
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlTreatments::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CXmlTreatment* CXmlTreatments::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CXmlTreatment*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CXmlTreatments::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CXmlTreatment* CXmlTreatments::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CXmlTreatment*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CXmlTreatments::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CXmlTreatment* CXmlTreatments::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CXmlTreatment*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CXmlTreatments::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlTreatments::Remove(CXmlTreatment* pTreatment, BOOL bDelete)
{
	POSITION Pos = Find(pTreatment);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pTreatment;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlTreatments::SetPosition()
//
// 	Description:	This function is called to set the iterators to the 
//					specified position.
//
// 	Returns:		The object at the new position
//
//	Notes:			None
//
//==============================================================================
CXmlTreatment* CXmlTreatments::SetPosition(CXmlTreatment* pTreatment)
{
	CXmlTreatment* pCurrent = First();
	
	while(pCurrent != 0)
	{
		if(pCurrent == pTreatment)
			break;
		else
			pCurrent = Next();
	}
	return pCurrent;
}


