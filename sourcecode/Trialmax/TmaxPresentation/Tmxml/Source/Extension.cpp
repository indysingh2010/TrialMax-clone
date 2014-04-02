//==============================================================================
//
// File Name:	extension.cpp
//
// Description:	This file contains member functions of the CExtensions class
//
// See Also:	extension.h
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	02-22-02	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <extension.h>

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
// 	Function Name:	CExtension::CExtension()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CExtension::CExtension(LPCSTR lpszExtension)
{
	if(lpszExtension != 0)
		m_strExtension = lpszExtension;
	else
		m_strExtension.Empty();	
}

//==============================================================================
//
// 	Function Name:	CExtensions::Add()
//
// 	Description:	This function will add an object to the list
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CExtensions::Add(LPCSTR lpszExtension)
{
	CExtension* pAdd;

	ASSERT(lpszExtension);
	if(!lpszExtension)
		return FALSE;

	try
	{
		//	Allocate a new object
		pAdd = new CExtension(lpszExtension);
			
		//	Add at the head of the list if the list is empty
		if(IsEmpty())
			AddHead(pAdd);
		else
			AddTail(pAdd);

		return TRUE;
	}
	catch(...)
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CString::CExtensions()
//
// 	Description:	This is the constructor for CExtensions objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CExtensions::CExtensions() : CObList()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CString::~CExtensions()
//
// 	Description:	This is the destructor for CExtensions objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CExtensions::~CExtensions()
{
	//	Flush the list and destroy its objects
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CExtensions::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CExtensions::Find(CExtension* pExtension)
{
	return (CObList::Find(pExtension));
}

//==============================================================================
//
// 	Function Name:	CExtensions::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CExtension* CExtensions::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CExtension*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CExtensions::Find()
//
// 	Description:	This function will locate the specified extension
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			None
//
//==============================================================================
CExtension* CExtensions::Find(LPCSTR lpszExtension)
{
	POSITION	Pos;
	CExtension* pExtension;

	Pos = GetHeadPosition();

	while(Pos != NULL)
	{
		if((pExtension = (CExtension*)GetNext(Pos)) != 0)
		{
			if(pExtension->m_strExtension.CompareNoCase(lpszExtension) == 0)
				return pExtension;
		}
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CExtensions::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CExtensions::Flush(BOOL bDelete)
{
	CExtension* pExtension;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pExtension = (CExtension*)GetNext(m_NextPos)) != 0)
				delete pExtension;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CExtensions::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CExtension* CExtensions::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CExtension*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CExtensions::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CExtension* CExtensions::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CExtension*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CExtensions::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CExtension* CExtensions::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CExtension*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CExtensions::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CExtensions::Remove(CExtension* pExtension, BOOL bDelete)
{
	POSITION Pos = Find(pExtension);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pExtension;
	}
}

