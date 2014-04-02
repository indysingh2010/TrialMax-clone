//==============================================================================
//
// File Name:	multipg.cpp
//
// Description:	This file contains member functions of the CMultipage and
//				CMultipages classes
//
// See Also:	multipg.h
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
#include <multipg.h>

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
// 	Function Name:	CMultipage::CMultipage()
//
// 	Description:	This is the constructor for CMultipage objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMultipage::CMultipage(CMedia* pMedia) : CMedia(pMedia)
{
}

//==============================================================================
//
// 	Function Name:	CMultipage::~CMultipage()
//
// 	Description:	This is the destructor for CMultipage objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMultipage::~CMultipage()
{
	//	Flush the list of pages
	m_Pages.Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CMultipages::Add()
//
// 	Description:	This function will add an object to the list
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMultipages::Add(CMultipage* pMultipage)
{
	ASSERT(pMultipage);
	if(!pMultipage)
		return FALSE;

	//	MFC will throw a memory exception if it can't add the object to the list
	try
	{
		//	Add the link to the end of the list
		AddTail(pMultipage);
		return TRUE;
	}
	catch(...)
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CMultipage::CMultipages()
//
// 	Description:	This is the constructor for CMultipages objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMultipages::CMultipages()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CMultipage::~CMultipages()
//
// 	Description:	This is the destructor for CMultipages objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMultipages::~CMultipages()
{
	//	Flush the list and destroy it's objects
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CMultipages::Find()
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
CMultipage* CMultipages::Find(LPCSTR lpId)
{
	POSITION		Pos;
	CMultipage*		pMultipage;

	//	Get the first position
	Pos = GetHeadPosition();

	//	Check each item until we find a match
	while(Pos != NULL)
	{
		if((pMultipage = (CMultipage*)GetNext(Pos)) != 0)
		{
			if(lstrcmpi(pMultipage->m_strMediaId, lpId) == 0)
				return pMultipage;
		}
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CMultipages::Find()
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
CMultipage* CMultipages::Find(long lId)
{
	POSITION	Pos;
	CMultipage*	pMultipage;

	//	Get the first position
	Pos = GetHeadPosition();

	//	Check each item until we find a match
	while(Pos != NULL)
	{
		if((pMultipage = (CMultipage*)GetNext(Pos)) != 0)
		{
			if(pMultipage->m_lPrimaryId == lId)
				return pMultipage;
		}
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CMultipages::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CMultipages::Find(CMultipage* pMultipage)
{
	return (CObList::Find(pMultipage));
}

//==============================================================================
//
// 	Function Name:	CMultipages::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CMultipage* CMultipages::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CMultipage*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CMultipages::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CMultipages::Flush(BOOL bDelete)
{
	CMultipage* pMultipage;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pMultipage = (CMultipage*)GetNext(m_NextPos)) != 0)
				delete pMultipage;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CMultipages::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CMultipage* CMultipages::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CMultipage*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CMultipages::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CMultipage* CMultipages::Next()
{	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CMultipage*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CMultipages::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CMultipage* CMultipages::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CMultipage*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CMultipages::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMultipages::Remove(CMultipage* pMultipage, BOOL bDelete)
{
	POSITION Pos = Find(pMultipage);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pMultipage;
	}
}

