//==============================================================================
//
// File Name:	textpage.cpp
//
// Description:	This file contains member functions of the CTextPage and 
//				CTextPages classes
//
// See Also:	textpage.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-10-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <textpage.h>

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
// 	Function Name:	CTextPage::CTextPage()
//
// 	Description:	This is the constructor for CTextPage objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTextPage::CTextPage()
{
	m_lPageNum = 0;
	m_lDesignationId = 0;
	m_lDesignationOrder = 0;
}

//==============================================================================
//
// 	Function Name:	CTextPage::~CTextPage()
//
// 	Description:	This is the destructor for CTextPage objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTextPage::~CTextPage()
{
	//	Flush the list of lines
	m_Lines.Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CTextPage::FirstLine()
//
// 	Description:	This function will retrieve the first line of text 
//
// 	Returns:		A pointer to the line object if found
//
//	Notes:			None
//
//==============================================================================
CTextLine* CTextPage::FirstLine()
{
	return m_Lines.First();
}

//==============================================================================
//
// 	Function Name:	CTextPage::GetTime()
//
// 	Description:	This function will get the frame number that corresponds to
//					the specified line in this page.
//
// 	Returns:		The requested frame if found. -1 otherwise
//
//	Notes:			None
//
//==============================================================================
double CTextPage::GetTime(long lLine, BOOL bStartLine)
{
	return m_Lines.GetTime(lLine, bStartLine);
}

//==============================================================================
//
// 	Function Name:	CTextPage::LastLine()
//
// 	Description:	This function will retrieve the last line of text 
//
// 	Returns:		A pointer to the line object if found
//
//	Notes:			None
//
//==============================================================================
CTextLine* CTextPage::LastLine()
{
	return m_Lines.Last();
}

//==============================================================================
//
// 	Function Name:	CTextPage::NextLine()
//
// 	Description:	This function will retrieve the next line of text 
//
// 	Returns:		A pointer to the line object if found
//
//	Notes:			None
//
//==============================================================================
CTextLine* CTextPage::NextLine()
{
	return m_Lines.Next();
}

//==============================================================================
//
// 	Function Name:	CTextPage::operator > ()
//
// 	Description:	This is an overloaded version of the > operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			None
//
//==============================================================================
BOOL CTextPage::operator > (const CTextPage& Compare)
{
	if(m_lDesignationOrder == Compare.m_lDesignationOrder)
	{
		return (m_lPageNum > Compare.m_lPageNum);
	}
	else
	{
		return (m_lDesignationOrder > Compare.m_lDesignationOrder);
	}
}

//==============================================================================
//
// 	Function Name:	CTextPage::operator < ()
//
// 	Description:	This is an overloaded version of the < operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			None
//
//==============================================================================
BOOL CTextPage::operator < (const CTextPage& Compare)
{
	if(m_lDesignationOrder == Compare.m_lDesignationOrder)
	{
		return (m_lPageNum < Compare.m_lPageNum);
	}
	else
	{
		return (m_lDesignationOrder < Compare.m_lDesignationOrder);
	}
}

//==============================================================================
//
// 	Function Name:	CTextPage::operator == ()
//
// 	Description:	This is an overloaded version of the == operator.
//
// 	Returns:		TRUE if the objects are equal.
//
//	Notes:			None
//
//==============================================================================
BOOL CTextPage::operator == (const CTextPage& Compare)
{
	return (m_lPageNum == Compare.m_lPageNum);
}

//==============================================================================
//
// 	Function Name:	CTextPage::PrevLine()
//
// 	Description:	This function will retrieve the previous line of text 
//
// 	Returns:		A pointer to the line object if found
//
//	Notes:			None
//
//==============================================================================
CTextLine* CTextPage::PrevLine()
{
	return m_Lines.Prev();
}

//==============================================================================
//
// 	Function Name:	CTextPages::Add()
//
// 	Description:	This function will add an object to the list
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTextPages::Add(CTextPage* pPage)
{
	POSITION	Pos;
	POSITION	Prev;
	CTextPage*		pCurrent;

	ASSERT(pPage);
	if(!pPage)
		return FALSE;

	try
	{
		//	Add at the head of the list if the list is empty
		if(IsEmpty())
		{
			AddHead(pPage);
			return TRUE;
		}

		//	Do a quick check to see if this should be put at the end of the 
		//	collection
		if(*pPage > *((CTextPage*)GetTail()))
		{
			AddTail(pPage);
			return TRUE;
		}

		//	Look for the correct position
		Pos = GetHeadPosition();
		Prev = Pos;
		while(Pos != NULL)
		{
			if((pCurrent = (CTextPage*)GetNext(Pos)) == NULL)
				continue;

			//	Should we insert here?
			if(*pPage < *pCurrent)
			{
				InsertBefore(Prev, pPage);
				return TRUE;
			}
		
			Prev = Pos;	
		}

		//	If we made it this far we must have to add it to the end of the list
		AddTail(pPage);
		return TRUE;

	}
	catch(...)
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CTextPage::CTextPages()
//
// 	Description:	This is the constructor for CTextPages objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTextPages::CTextPages()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CTextPage::~CTextPages()
//
// 	Description:	This is the destructor for CTextPages objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTextPages::~CTextPages()
{
	//	Flush the list and destroy it's objects
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CTextPages::Find()
//
// 	Description:	This function will search the list for the object with the
//					page number specified by the caller.
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			The local position markers are not modified with a call to
//					this function.
//
//==============================================================================
CTextPage* CTextPages::Find(long lPageNum)
{
	POSITION	Pos;
	CTextPage*	pPage;

	//	Get the first position
	Pos = GetHeadPosition();

	//	Check each object until we find a match
	while(Pos != NULL)
	{
		if((pPage = (CTextPage*)GetNext(Pos)) != 0)
		{
			if(pPage->m_lPageNum == lPageNum)
				return pPage;
		}
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CTextPages::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CTextPages::Find(CTextPage* pPage)
{
	return (CObList::Find(pPage));
}

//==============================================================================
//
// 	Function Name:	CTextPages::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CTextPage* CTextPages::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CTextPage*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CTextPages::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CTextPages::Flush(BOOL bDelete)
{
	CTextPage* pPage;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pPage = (CTextPage*)GetNext(m_NextPos)) != 0)
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
// 	Function Name:	CTextPages::GetTime()
//
// 	Description:	This function will get the time that corresponds to
//					the specified line in this page.
//
// 	Returns:		The requested time if found. -1 otherwise
//
//	Notes:			None
//
//==============================================================================
double CTextPages::GetTime(long lPage, long lLine, BOOL bStartLine)
{
	CTextPage* pPage = Find(lPage);

	if(pPage == 0)
	{ 
		return -1.0;
	}
	else
	{
		return pPage->GetTime(lLine, bStartLine);
	}
}

//==============================================================================
//
// 	Function Name:	CTextPages::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CTextPage* CTextPages::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CTextPage*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CTextPages::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CTextPage* CTextPages::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CTextPage*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CTextPages::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CTextPage* CTextPages::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CTextPage*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CTextPages::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTextPages::Remove(CTextPage* pPage, BOOL bDelete)
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
// 	Function Name:	CTextPages::SetPos()
//
// 	Description:	This function will set the position of the local iterator
//					on the page specified by the caller
//
// 	Returns:		A pointer to object if found
//
//	Notes:			None
//
//==============================================================================
CTextPage* CTextPages::SetPos(long lPageNum)
{
	CTextPage* pPage;

	pPage = First();
	while(pPage)
	{
		if(pPage->m_lPageNum == lPageNum)
			return pPage;
		else
			pPage = Next();
	}
	return 0;
}

