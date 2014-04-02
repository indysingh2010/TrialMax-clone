//==============================================================================
//
// File Name:	annotate.cpp
//
// Description:	This file contains member functions of the CAnnotation, and 
//				CAnnotations classes
//
// See Also:	annotate.h 
//
// Copyright FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	09-09-98	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <annotate.h>
#include <callout.h>

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
// 	Function Name:	CAnnotation::Add()
//
// 	Description:	This function add a callout to the list of those linked to
//					the annotation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotation::Add(CCallout* pCallout)
{
	if(m_pCallouts)
		m_pCallouts->Add(pCallout);
}

//==============================================================================
//
// 	Function Name:	CAnnotation::CAnnotation()
//
// 	Description:	This is the constructor for CAnnotation objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CAnnotation::CAnnotation(WORD wId)
{
	m_wId = wId;
	m_pCallouts = 0;
	m_bIsCallout = FALSE;
	m_bIsCalloutShade = FALSE;
	m_bIsLocked = FALSE;
	m_strText.Empty();
	ZeroMemory(&m_rcAnn, sizeof(m_rcAnn));

	//	Allocate the list for the callout references
	m_pCallouts = new CCallouts();
	ASSERT(m_pCallouts);
}

//==============================================================================
//
// 	Function Name:	CAnnotation::CAnnotation()
//
// 	Description:	This form of the constructor will create the annotation and
//					initialize it's members using the specified tag
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CAnnotation::CAnnotation(DWORD dwTag)
{
	m_wId = (WORD)(dwTag & 0x0000FFFFL);
	m_pCallouts = 0;
	m_bIsCallout = (dwTag & TMANN_CALLOUT);
	m_bIsCalloutShade = (dwTag & TMANN_CALLOUT_SHADE);
	m_bIsLocked = (dwTag & TMANN_LOCKED);
	ZeroMemory(&m_rcAnn, sizeof(m_rcAnn));

	//	Allocate the list for the callout references
	m_pCallouts = new CCallouts();
	ASSERT(m_pCallouts);
}

//==============================================================================
//
// 	Function Name:	CAnnotation::~CAnnotation()
//
// 	Description:	This is the destructor for CAnnotation objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CAnnotation::~CAnnotation()
{
	//	Flush the callout list
	if(m_pCallouts)
	{
		m_pCallouts->Flush(FALSE);
		delete m_pCallouts;
		m_pCallouts = 0;
	}
}

//==============================================================================
//
// 	Function Name:	CAnnotation::First()
//
// 	Description:	This function is called to get the first callout in the list
//					of those linked to the annotation.
//
// 	Returns:		A pointer to the callout object
//
//	Notes:			None
//
//==============================================================================
CCallout* CAnnotation::First()
{
	if(m_pCallouts)
		return m_pCallouts->First();
	else
		return 0;
}

//==============================================================================
//
// 	Function Name:	CAnnotation::GetAnnTag()
//
// 	Description:	This function is called to get the tag that gets attached
//					to the LeadTools annotation object
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
DWORD CAnnotation::GetAnnTag()
{
	DWORD dwTag = (DWORD)m_wId;

	//	Now add the flags
	if(m_bIsCallout)
		dwTag |= TMANN_CALLOUT;
	if(m_bIsCalloutShade)
		dwTag |= TMANN_CALLOUT_SHADE;
 	if(m_bIsLocked)
		dwTag |= TMANN_LOCKED;

	return dwTag;
}

//==============================================================================
//
// 	Function Name:	CAnnotation::Next()
//
// 	Description:	This function is called to get the next callout in the list
//					of those linked to the annotation.
//
// 	Returns:		A pointer to the callout object
//
//	Notes:			None
//
//==============================================================================
CCallout* CAnnotation::Next()
{
	if(m_pCallouts)
		return m_pCallouts->Next();
	else
		return 0;
}

//==============================================================================
//
// 	Function Name:	CAnnotation::Remove()
//
// 	Description:	This function remove a callout from the list of those linked
//					to the annotation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotation::Remove(CCallout* pCallout)
{
	if(m_pCallouts)
		m_pCallouts->Remove(pCallout, FALSE);
}

//==============================================================================
//
// 	Function Name:	CAnnotations::Add()
//
// 	Description:	This function allocate a new object and add it to the list
//					if it is not already in the list.
//
// 	Returns:		A pointer to the annotation object
//
//	Notes:			None
//
//==============================================================================
CAnnotation* CAnnotations::Add()
{
	CAnnotation* pAnn;

	//	Allocate a new object
	pAnn = new CAnnotation(++m_wAnnId);
	ASSERT(pAnn);

	//	Add it to the list
	Add(pAnn);

	return pAnn;
}

//==============================================================================
//
// 	Function Name:	CAnnotations::Add()
//
// 	Description:	This function will add a link object to the list
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CAnnotations::Add(CAnnotation* pAnn)
{
	ASSERT(pAnn);
	if(!pAnn)
		return FALSE;

	try
	{
		//	Update the local id in case this function is called directly
		if(pAnn->m_wId > m_wAnnId)
			m_wAnnId = pAnn->m_wId;

		AddTail(pAnn);
		return TRUE;
	}
	catch(...)
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CAnnotation::CAnnotations()
//
// 	Description:	This is the constructor for CAnnotations objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CAnnotations::CAnnotations()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;
	m_wAnnId  = 0;
}

//==============================================================================
//
// 	Function Name:	CAnnotation::~CAnnotations()
//
// 	Description:	This is the destructor for CAnnotations objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CAnnotations::~CAnnotations()
{
	//	Flush the list and destroy its objects
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CAnnotations::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CAnnotations::Find(CAnnotation* pAnn)
{
	return (CObList::Find(pAnn));
}

//==============================================================================
//
// 	Function Name:	CAnnotations::Find()
//
// 	Description:	This function will find the annotation object with the
//					specified tag.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CAnnotation* CAnnotations::Find(DWORD dwTag)
{
	CAnnotation* pAnn;
	POSITION	 Pos;

	Pos = GetHeadPosition();
	while(Pos != NULL)
	{
		if((pAnn = (CAnnotation*)GetNext(Pos)) != 0)
			if(pAnn->GetAnnTag() == dwTag)
				return pAnn;
	}

	return NULL;
}

//==============================================================================
//
// 	Function Name:	CAnnotations::Find()
//
// 	Description:	This function will find the annotation object with the
//					specified id.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CAnnotation* CAnnotations::Find(WORD wId)
{
	CAnnotation* pAnn;
	POSITION	 Pos;

	Pos = GetHeadPosition();
	while(Pos != NULL)
	{
		if((pAnn = (CAnnotation*)GetNext(Pos)) != 0)
			if(pAnn->m_wId == wId)
				return pAnn;
	}

	return NULL;
}

//==============================================================================
//
// 	Function Name:	CAnnotations::FindCalloutShade()
//
// 	Description:	This function will find locate the annotation object 
//					associated with the callout shade
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CAnnotation* CAnnotations::FindCalloutShade()
{
	CAnnotation* pAnn;
	POSITION	 Pos;

	Pos = GetHeadPosition();
	while(Pos != NULL)
	{
		if((pAnn = (CAnnotation*)GetNext(Pos)) != 0)
			if(pAnn->m_bIsCalloutShade)
				return pAnn;
	}

	return NULL;
}

//==============================================================================
//
// 	Function Name:	CAnnotations::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CAnnotation* CAnnotations::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CAnnotation*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CAnnotations::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CAnnotations::Flush(BOOL bDelete)
{
	CAnnotation* pAnn;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pAnn = (CAnnotation*)GetNext(m_NextPos)) != 0)
				delete pAnn;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
	m_wAnnId = 0;
}

//==============================================================================
//
// 	Function Name:	CAnnotations::HasLocked()
//
// 	Description:	This function is called to determine if there are one or
//					more locked annotations in the list.
//
// 	Returns:		True if at least one locked annotation
//
//	Notes:			None
//
//==============================================================================
BOOL CAnnotations::HasLocked()
{
	CAnnotation* pAnn;
	POSITION	 Pos;

	Pos = GetHeadPosition();
	while(Pos != NULL)
	{
		if((pAnn = (CAnnotation*)GetNext(Pos)) != 0)
			if(pAnn->m_bIsLocked == TRUE)
				return TRUE;
	}

	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CAnnotations::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CAnnotation* CAnnotations::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CAnnotation*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CAnnotations::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CAnnotation* CAnnotations::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CAnnotation*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CAnnotations::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CAnnotation* CAnnotations::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CAnnotation*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CAnnotations::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotations::Remove(CAnnotation* pAnn, BOOL bDelete)
{
	POSITION Pos = Find(pAnn);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pAnn;
	}
}

//==============================================================================
//
// 	Function Name:	CAnnotations::Remove()
//
// 	Description:	This function will remove the callout specified by the user
//					from the list of those associated with each annotation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotations::Remove(CCallout* pCallout)
{
	POSITION	 Pos;
	CAnnotation* pAnn;

	Pos = GetHeadPosition();
	while(Pos != NULL)
	{
		if((pAnn = (CAnnotation*)GetNext(Pos)) != 0)
			pAnn->Remove(pCallout);
	}
}

