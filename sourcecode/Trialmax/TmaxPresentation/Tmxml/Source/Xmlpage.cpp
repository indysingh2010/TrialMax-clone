//==============================================================================
//
// File Name:	xmlpage.cpp
//
// Description:	This file contains member functions of the CXmlPage and
//				CXmlPages classes
//
// See Also:	xmlpage.h
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
#include <xmlpage.h>

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
// 	Function Name:	CXmlPage::CXmlPage()
//
// 	Description:	This is the constructor for CXmlPage objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlPage::CXmlPage(CXmlMedia* pMedia) : CMediaCtrlItem()
{
	m_pXmlMedia = pMedia;
	m_strNumber.Empty();
	m_strId.Empty();
	m_strType.Empty();
	m_strTitle.Empty();
	m_strSource.Empty();
	m_lPosition = 0;
}

//==============================================================================
//
// 	Function Name:	CXmlPage::~CXmlPage()
//
// 	Description:	This is the destructor for CXmlPage objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlPage::~CXmlPage()
{
	//	Flush the list of treatments
	m_Treatments.Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CXmlPage::GetDisplayText()
//
// 	Description:	This function is called to get the text used to display
//					the page
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlPage::GetDisplayText(CString& rText)
{
	//	Do we have a title?
	if(m_strTitle.GetLength() > 0)
		rText = m_strTitle;

	//	Do we have a valid page number?
	else if(m_strNumber.GetLength() > 0)
		rText = m_strNumber;

	else
		rText = m_strSource;
}

//==============================================================================
//
// 	Function Name:	CXmlPage::GetDisplayText()
//
// 	Description:	This function is called to get the text used to display
//					the page
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlPage::GetDisplayText(LPSTR lpszText, int iMaxLength)
{
	CString strText;

	GetDisplayText(strText);
	lstrcpyn(lpszText, strText, iMaxLength);
}

//==============================================================================
//
// 	Function Name:	CXmlPage::GetDuplicate()
//
// 	Description:	This function will allocate and initialize a duplicate page.
//
// 	Returns:		A pointer to the duplicate page
//
//	Notes:			None
//
//==============================================================================
CXmlPage* CXmlPage::GetDuplicate()
{
	CXmlPage*		pPage;
	CXmlTreatment*	pTreatment;
	CXmlTreatment*	pCopy;
	POSITION		Pos;

	//	Allocate a new page
	pPage = new CXmlPage();

	//	Copy the members
	pPage->m_strNumber = m_strNumber;
	pPage->m_strId = m_strId;
	pPage->m_strSource = m_strSource;
	pPage->m_strTitle = m_strTitle;
	pPage->m_strType = m_strType;

	//	Make a copy of each treatment
	Pos = m_Treatments.GetHeadPosition();
	while(Pos != NULL)
	{
		if((pTreatment = (CXmlTreatment*)m_Treatments.GetNext(Pos)) != 0)
		{
			pCopy = new CXmlTreatment(pPage);
			pCopy->m_strSource = pTreatment->m_strSource;
			pPage->m_Treatments.Add(pCopy);
		}
	}

	return pPage;
}

//==============================================================================
//
// 	Function Name:	CXmlPage::operator < ()
//
// 	Description:	This is an overloaded version of the < operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlPage::operator < (const CXmlPage& Compare)
{
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlPage::operator == ()
//
// 	Description:	This is an overloaded version of the == operator.
//
// 	Returns:		TRUE if the objects are equal.
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlPage::operator == (const CXmlPage& Compare)
{
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlPage::SetAttribute()
//
// 	Description:	This function will set the specified attribute to the 
//					requested value.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlPage::SetAttribute(LPCSTR lpAttribute, LPCSTR lpValue)
{
	ASSERT(lpAttribute);
	ASSERT(lpValue);

	if(lstrcmpi(lpAttribute, TMXML_PAGE_PAGEID) == 0)
	{
		m_strId = lpValue;
		return TRUE;
	}
	else if(lstrcmpi(lpAttribute, TMXML_PAGE_SOURCE) == 0)
	{
		m_strSource = lpValue;
		return TRUE;
	}
	else if(lstrcmpi(lpAttribute, TMXML_PAGE_TYPE) == 0)
	{
		m_strType = lpValue;
		return TRUE;
	}
	else if(lstrcmpi(lpAttribute, TMXML_PAGE_TITLE) == 0)
	{
		m_strTitle = lpValue;
		return TRUE;
	}
	else if(lstrcmpi(lpAttribute, TMXML_PAGE_NUMBER) == 0)
	{
		m_strNumber = lpValue;
		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlPage::SetAttributes()
//
// 	Description:	This function will set the attributes to match those of
//					the source object provided by the caller.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlPage::SetAttributes(const CXmlPage& rSource)
{
	m_strNumber = rSource.m_strNumber;
	m_strId = rSource.m_strId;
	m_strSource = rSource.m_strSource;
	m_strTitle = rSource.m_strTitle;
	m_strType = rSource.m_strType;
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CXmlPages::Add()
//
// 	Description:	This function will add an object to the list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlPages::Add(CXmlPage* pPage)
{
	POSITION	Pos;
	POSITION	Prev;
	CXmlPage*	pCurrent;

	//	Add at the head of the list if the list is empty
	if(IsEmpty())
	{
		AddHead(pPage);
		return;
	}

	//	Look for the correct position
	Pos = GetHeadPosition();
	Prev = Pos;
	while(Pos != NULL)
	{
		if((pCurrent = (CXmlPage*)GetNext(Pos)) == NULL)
			continue;

		if(*pPage < *pCurrent)
		{
			InsertBefore(Prev, pPage);
			return;
		}
		
		Prev = Pos;	
	}

	//	If we made it this far we must have to add it to the end of the list
	AddTail(pPage);
}

//==============================================================================
//
// 	Function Name:	CXmlPage::CXmlPages()
//
// 	Description:	This is the constructor for CXmlPages objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlPages::CXmlPages()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CXmlPage::~CXmlPages()
//
// 	Description:	This is the destructor for CXmlPages objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlPages::~CXmlPages()
{
	//	Flush the list and destroy it's objects
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CXmlPages::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CXmlPages::Find(CXmlPage* pPage)
{
	return (CObList::Find(pPage));
}

//==============================================================================
//
// 	Function Name:	CXmlPages::Find()
//
// 	Description:	This function will search the list for the item with the
//					identifier specified by the caller.
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			None
//
//==============================================================================
CXmlPage* CXmlPages::Find(LPCSTR lpId)
{
	POSITION	Pos = GetHeadPosition();
	CXmlPage*	pPage = 0;

	ASSERT(lpId);

	while(Pos != NULL)
	{
		if((pPage = (CXmlPage*)GetNext(Pos)) != 0)
		{
			if(lstrcmpi(pPage->m_strId, lpId) == 0)
				return pPage;
		}
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CXmlPages::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CXmlPage* CXmlPages::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CXmlPage*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CXmlPages::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CXmlPages::Flush(BOOL bDelete)
{
	CXmlPage* pPage;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pPage = (CXmlPage*)GetNext(m_NextPos)) != 0)
				delete pPage;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CXmlPages::IsFirst()
//
// 	Description:	This function is called to determine if the object specified
//					by the caller is the first one in the list.
//
// 	Returns:		TRUE if first in list
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlPages::IsFirst(CXmlPage* pPage)
{
	POSITION Pos = GetHeadPosition();

	if(Pos != NULL)
		return (pPage == (CXmlPage*)GetNext(Pos));
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlPages::IsLast()
//
// 	Description:	This function is called to determine if the object specified
//					by the caller is the last one in the list.
//
// 	Returns:		TRUE if last in list
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlPages::IsLast(CXmlPage* pPage)
{
	POSITION Pos = GetTailPosition();

	if(Pos != NULL)
		return (pPage == (CXmlPage*)GetPrev(Pos));
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlPages::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CXmlPage* CXmlPages::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CXmlPage*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CXmlPages::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CXmlPage* CXmlPages::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CXmlPage*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CXmlPages::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CXmlPage* CXmlPages::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CXmlPage*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CXmlPages::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlPages::Remove(CXmlPage* pPage, BOOL bDelete)
{
	POSITION Pos = Find(pPage);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pPage;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlPages::SetPosition()
//
// 	Description:	This function is called to set the iterators to the 
//					specified position.
//
// 	Returns:		The object at the new position
//
//	Notes:			None
//
//==============================================================================
CXmlPage* CXmlPages::SetPosition(CXmlPage* pPage)
{
	CXmlPage* pCurrent = First();
	
	while(pCurrent != 0)
	{
		if(pCurrent == pPage)
			break;
		else
			pCurrent = Next();
	}
	return pCurrent;
}

