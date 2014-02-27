//==============================================================================
//
// File Name:	xmlmedia.cpp
//
// Description:	This file contains member functions of the CXmlMedia,
//				CXmlMediaTree and CXmlMediaTrees classes.
//
// See Also:	xmlmedia.h
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
#include <xmlmedia.h>

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
// 	Function Name:	CXmlMedia::CXmlMedia()
//
// 	Description:	This is the constructor for CXmlMedia objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlMedia::CXmlMedia(CXmlMediaTree* pTree) : CMediaCtrlItem()
{
	m_pXmlMediaTree = pTree;
	m_strId.Empty();
	m_strTitle.Empty();
}

//==============================================================================
//
// 	Function Name:	CXmlMedia::~CXmlMedia()
//
// 	Description:	This is the destructor for CXmlMedia objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlMedia::~CXmlMedia()
{
	//	Deallocate all the pages
	m_Pages.Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CXmlMedia::Find()
//
// 	Description:	This function is called to locate the page belonging to this
//					media object that has the specified id.
//
// 	Returns:		The requested page if found
//
//	Notes:			None
//
//==============================================================================
CXmlPage* CXmlMedia::Find(LPCSTR lpId)
{
	return m_Pages.Find(lpId);
}

//==============================================================================
//
// 	Function Name:	CXmlMedia::GetDuplicate()
//
// 	Description:	This function will allocate and initialize a duplicate 
//					media object.
//
// 	Returns:		A pointer to the duplicate media object
//
//	Notes:			None
//
//==============================================================================
CXmlMedia* CXmlMedia::GetDuplicate()
{
	CXmlMedia*	pMedia;
	CXmlPage*	pPage;
	CXmlPage*	pCopy;
	POSITION	Pos;

	//	Allocate a new media object
	pMedia = new CXmlMedia();

	//	Copy the members
	pMedia->m_strId = m_strId;
	pMedia->m_strTitle = m_strTitle;

	//	Make a copy of each page
	Pos = m_Pages.GetHeadPosition();
	while(Pos != NULL)
	{
		if((pPage = (CXmlPage*)m_Pages.GetNext(Pos)) != 0)
		{
			pCopy = pPage->GetDuplicate();
			pCopy->m_pXmlMedia = pMedia;
			pMedia->m_Pages.Add(pCopy);
		}
	}
	return pMedia;
}

//==============================================================================
//
// 	Function Name:	CXmlMedia::GetFileCount()
//
// 	Description:	This function is called to get the total number of files
//					represented by this media object.
//
// 	Returns:		A number of files
//
//	Notes:			None
//
//==============================================================================
long CXmlMedia::GetFileCount()
{
	CXmlPage*	pPage;
	POSITION	Pos;
	long		lCount = 0;

	//	Add up all the pages
	Pos = m_Pages.GetHeadPosition();
	while(Pos != NULL)
	{
		if((pPage = (CXmlPage*)m_Pages.GetNext(Pos)) != 0)
		{
			//	Add one for the page itself
			lCount++;

			//	Now add one for each treatment of the page
			lCount += pPage->m_Treatments.GetCount();
		}
	}
	return lCount;
}

//==============================================================================
//
// 	Function Name:	CXmlMedia::operator < ()
//
// 	Description:	This is an overloaded version of the < operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlMedia::operator < (const CXmlMedia& Compare)
{
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlMedia::operator == ()
//
// 	Description:	This is an overloaded version of the == operator.
//
// 	Returns:		TRUE if the objects are equal.
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlMedia::operator == (const CXmlMedia& Compare)
{
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlMedia::SetAttribute()
//
// 	Description:	This function will set the specified attribute to the 
//					requested value.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlMedia::SetAttribute(LPCSTR lpAttribute, LPCSTR lpValue)
{
	ASSERT(lpAttribute);
	ASSERT(lpValue);

	if(lstrcmpi(lpAttribute, TMXML_MEDIA_MEDIAID) == 0)
	{
		m_strId = lpValue;
		return TRUE;
	}
	else if(lstrcmpi(lpAttribute, TMXML_MEDIA_TITLE) == 0)
	{
		m_strTitle = lpValue;
		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlMedia::SetAttributes()
//
// 	Description:	This function will set the attributes to match those of
//					the source object provided by the caller.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlMedia::SetAttributes(const CXmlMedia& rSource)
{
	m_strId = rSource.m_strId;
	m_strTitle = rSource.m_strTitle;
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTree::Add()
//
// 	Description:	This function will add an object to the list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlMediaTree::Add(CXmlMedia* pMedia)
{
	POSITION	Pos;
	POSITION	Prev;
	CXmlMedia*	pCurrent;

	//	Add at the head of the list if the list is empty
	if(IsEmpty())
	{
		AddHead(pMedia);
		return;
	}

	//	Look for the correct position
	Pos = GetHeadPosition();
	Prev = Pos;
	while(Pos != NULL)
	{
		if((pCurrent = (CXmlMedia*)GetNext(Pos)) == NULL)
			continue;

		if(*pMedia < *pCurrent)
		{
			InsertBefore(Prev, pMedia);
			return;
		}
		
		Prev = Pos;	
	}

	//	If we made it this far we must have to add it to the end of the list
	AddTail(pMedia);
}

//==============================================================================
//
// 	Function Name:	CXmlMedia::CXmlMediaTree()
//
// 	Description:	This is the constructor for CXmlMediaTree objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlMediaTree::CXmlMediaTree() : CMediaCtrlItem()
{
	m_strName.Empty();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CXmlMedia::~CXmlMediaTree()
//
// 	Description:	This is the destructor for CXmlMediaTree objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlMediaTree::~CXmlMediaTree()
{
	//	Flush the list and destroy it's objects
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTree::Find()
//
// 	Description:	This function will search the list for the item with the
//					id specified by the caller.
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			None
//
//==============================================================================
CXmlMedia* CXmlMediaTree::Find(LPCSTR lpId)
{
	POSITION	Pos = GetHeadPosition();
	CXmlMedia*	pMedia;

	while(Pos != NULL)
	{
		if((pMedia = (CXmlMedia*)GetNext(Pos)) != 0)
		{
			if(lstrcmpi(pMedia->m_strId, lpId) == 0)
				return pMedia;
		}
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTree::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CXmlMediaTree::Find(CXmlMedia* pMedia)
{
	return (CObList::Find(pMedia));
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTree::FindPage()
//
// 	Description:	This function will search the list of media objects to 
//					locate the specified page.
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			None
//
//==============================================================================
CXmlPage* CXmlMediaTree::FindPage(LPCSTR lpId)
{
	POSITION	Pos = GetHeadPosition();
	CXmlMedia*	pMedia;
	CXmlPage*	pPage;

	while(Pos != NULL)
	{
		if((pMedia = (CXmlMedia*)GetNext(Pos)) != 0)
		{
			if((pPage = pMedia->Find(lpId)) != 0)
				return pPage;
		}
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTree::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CXmlMedia* CXmlMediaTree::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CXmlMedia*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTree::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CXmlMediaTree::Flush(BOOL bDelete)
{
	CXmlMedia* pMedia;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pMedia = (CXmlMedia*)GetNext(m_NextPos)) != 0)
				delete pMedia;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTree::GetDuplicate()
//
// 	Description:	This function will allocate and initialize a duplicate 
//					media tree.
//
// 	Returns:		A pointer to the duplicate media object
//
//	Notes:			None
//
//==============================================================================
CXmlMediaTree* CXmlMediaTree::GetDuplicate()
{
	CXmlMediaTree*	pTree;
	CXmlMedia*		pMedia;
	CXmlMedia*		pCopy;
	POSITION		Pos;

	//	Allocate a new media object
	pTree = new CXmlMediaTree();

	//	Copy the members
	pTree->m_strName = m_strName;

	//	Make a copy of each media object
	Pos = GetHeadPosition();
	while(Pos != NULL)
	{
		if((pMedia = (CXmlMedia*)GetNext(Pos)) != 0)
		{
			pCopy = pMedia->GetDuplicate();
			pCopy->m_pXmlMediaTree = pTree;
			pTree->Add(pCopy);
		}
	}
	return pTree;
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTree::GetFileCount()
//
// 	Description:	This function is called to get the total number of files
//					represented by this media tree.
//
// 	Returns:		A number of files
//
//	Notes:			None
//
//==============================================================================
long CXmlMediaTree::GetFileCount()
{
	CXmlMedia*	pMedia;
	POSITION	Pos;
	long		lCount = 0;

	//	Add up the file count for each media object
	Pos = GetHeadPosition();
	while(Pos != NULL)
	{
		if((pMedia = (CXmlMedia*)GetNext(Pos)) != 0)
		{
			lCount += pMedia->GetFileCount();
		}
	}
	return lCount;
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTree::IsFirst()
//
// 	Description:	This function is called to determine if the object specified
//					by the caller is the first one in the list.
//
// 	Returns:		TRUE if first in list
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlMediaTree::IsFirst(CXmlMedia* pMedia)
{
	POSITION Pos = GetHeadPosition();

	if(Pos != NULL)
		return (pMedia == (CXmlMedia*)GetNext(Pos));
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTree::IsLast()
//
// 	Description:	This function is called to determine if the object specified
//					by the caller is the last one in the list.
//
// 	Returns:		TRUE if last in list
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlMediaTree::IsLast(CXmlMedia* pMedia)
{
	POSITION Pos = GetTailPosition();

	if(Pos != NULL)
		return (pMedia == (CXmlMedia*)GetPrev(Pos));
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTree::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CXmlMedia* CXmlMediaTree::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CXmlMedia*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTree::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CXmlMedia* CXmlMediaTree::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CXmlMedia*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTree::operator < ()
//
// 	Description:	This is an overloaded version of the < operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlMediaTree::operator < (const CXmlMediaTree& Compare)
{
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTree::operator == ()
//
// 	Description:	This is an overloaded version of the == operator.
//
// 	Returns:		TRUE if the objects are equal.
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlMediaTree::operator == (const CXmlMediaTree& Compare)
{
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTree::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CXmlMedia* CXmlMediaTree::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CXmlMedia*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTree::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlMediaTree::Remove(CXmlMedia* pMedia, BOOL bDelete)
{
	POSITION Pos = Find(pMedia);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pMedia;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTree::SetAttribute()
//
// 	Description:	This function will set the specified attribute to the 
//					requested value.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlMediaTree::SetAttribute(LPCSTR lpAttribute, LPCSTR lpValue)
{
	ASSERT(lpAttribute);
	ASSERT(lpValue);

	if(lstrcmpi(lpAttribute, TMXML_MEDIATREE_NAME) == 0)
	{
		m_strName = lpValue;
		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTree::SetAttributes()
//
// 	Description:	This function will set the attributes to match those of
//					the source object provided by the caller.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlMediaTree::SetAttributes(const CXmlMediaTree& rSource)
{
	m_strName = rSource.m_strName;
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTree::SetPosition()
//
// 	Description:	This function is called to set the iterators to the 
//					specified position.
//
// 	Returns:		The object at the new position
//
//	Notes:			None
//
//==============================================================================
CXmlMedia* CXmlMediaTree::SetPosition(CXmlMedia* pMedia)
{
	CXmlMedia* pCurrent = First();
	
	while(pCurrent != 0)
	{
		if(pCurrent == pMedia)
			break;
		else
			pCurrent = Next();
	}
	return pCurrent;
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTrees::Add()
//
// 	Description:	This function will add an object to the list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlMediaTrees::Add(CXmlMediaTree* pTree)
{
	POSITION		Pos;
	POSITION		Prev;
	CXmlMediaTree*	pCurrent;

	//	Add at the head of the list if the list is empty
	if(IsEmpty())
	{
		AddHead((CObject*)pTree);
		return;
	}

	//	Look for the correct position
	Pos = GetHeadPosition();
	Prev = Pos;
	while(Pos != NULL)
	{
		if((pCurrent = (CXmlMediaTree*)GetNext(Pos)) == NULL)
			continue;

		if(*pTree < *pCurrent)
		{
			InsertBefore(Prev, (CObject*)pTree);
			return;
		}
		
		Prev = Pos;	
	}

	//	If we made it this far we must have to add it to the end of the list
	AddTail((CObject*)pTree);
}

//==============================================================================
//
// 	Function Name:	CXmlMedia::CXmlMediaTrees()
//
// 	Description:	This is the constructor for CXmlMediaTrees objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlMediaTrees::CXmlMediaTrees()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CXmlMedia::~CXmlMediaTrees()
//
// 	Description:	This is the destructor for CXmlMediaTrees objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlMediaTrees::~CXmlMediaTrees()
{
	//	Flush the list and destroy it's objects
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTrees::Find()
//
// 	Description:	This function will search the list for the item with the
//					name specified by the caller.
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			None
//
//==============================================================================
CXmlMediaTree* CXmlMediaTrees::Find(LPCSTR lpName)
{
	POSITION		Pos = GetHeadPosition();
	CXmlMediaTree*	pTree;

	while(Pos != NULL)
	{
		if((pTree = (CXmlMediaTree*)GetNext(Pos)) != 0)
		{
			if(lstrcmpi(pTree->m_strName, lpName) == 0)
				return pTree;
		}
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTrees::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CXmlMediaTrees::Find(CXmlMediaTree* pTree)
{
	return (CObList::Find((CObject*)pTree));
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTrees::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CXmlMediaTree* CXmlMediaTrees::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CXmlMediaTree*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTrees::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CXmlMediaTrees::Flush(BOOL bDelete)
{
	CXmlMediaTree* pTree;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pTree = (CXmlMediaTree*)GetNext(m_NextPos)) != 0)
				delete pTree;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTrees::IsFirst()
//
// 	Description:	This function is called to determine if the object specified
//					by the caller is the first one in the list.
//
// 	Returns:		TRUE if first in list
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlMediaTrees::IsFirst(CXmlMediaTree* pTree)
{
	POSITION Pos = GetHeadPosition();

	if(Pos != NULL)
		return (pTree == (CXmlMediaTree*)GetNext(Pos));
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTrees::IsLast()
//
// 	Description:	This function is called to determine if the object specified
//					by the caller is the last one in the list.
//
// 	Returns:		TRUE if last in list
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlMediaTrees::IsLast(CXmlMediaTree* pTree)
{
	POSITION Pos = GetTailPosition();

	if(Pos != NULL)
		return (pTree == (CXmlMediaTree*)GetPrev(Pos));
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTrees::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CXmlMediaTree* CXmlMediaTrees::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CXmlMediaTree*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTrees::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CXmlMediaTree* CXmlMediaTrees::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CXmlMediaTree*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTrees::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CXmlMediaTree* CXmlMediaTrees::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CXmlMediaTree*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTrees::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlMediaTrees::Remove(CXmlMediaTree* pTree, BOOL bDelete)
{
	POSITION Pos = Find(pTree);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pTree;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlMediaTrees::SetPosition()
//
// 	Description:	This function is called to set the iterators to the 
//					specified position.
//
// 	Returns:		The object at the new position
//
//	Notes:			None
//
//==============================================================================
CXmlMediaTree* CXmlMediaTrees::SetPosition(CXmlMediaTree* pTree)
{
	CXmlMediaTree* pCurrent = First();
	
	while(pCurrent != 0)
	{
		if(pCurrent == pTree)
			break;
		else
			pCurrent = Next();
	}
	return pCurrent;
}


