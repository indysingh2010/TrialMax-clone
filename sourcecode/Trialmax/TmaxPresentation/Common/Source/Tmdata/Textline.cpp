//==============================================================================
//
// File Name:	textline.cpp
//
// Description:	This file contains member functions of the CTextLine and 
//				CTextLines classes
//
// See Also:	textline.h
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
#include <textline.h>
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
// 	Function Name:	CTextLine::CTextLine()
//
// 	Description:	This is the constructor for CTextLine objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTextLine::CTextLine()
{
	m_lDesignationId    = 0;
	m_lDesignationOrder = 0;
	m_lLineNum	 = 0;
	m_lPageNum	 = 0;
	m_dStartTime = 0;
	m_dStopTime	 = 0;
	m_bEnableScroll = TRUE;
	m_strMediaId.Empty();
	m_strText.Empty();
	m_crColor = 0xFF000000; // Invalid RGB
}

//==============================================================================
//
// 	Function Name:	CTextLine::~CTextLine()
//
// 	Description:	This is the destructor for CTextLine objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTextLine::~CTextLine()
{
}

//==============================================================================
//
// 	Function Name:	CTextLine::operator < ()
//
// 	Description:	This is an overloaded version of the < operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			None
//
//==============================================================================
BOOL CTextLine::operator < (const CTextLine& Compare)
{
	if(m_lDesignationOrder == Compare.m_lDesignationOrder)
	{
		if(m_lPageNum == Compare.m_lPageNum)
		{
			return (m_lLineNum < Compare.m_lLineNum);
		}
		else
		{
			return (m_lPageNum < Compare.m_lPageNum);
		}
	}
	else
	{
		return (m_lDesignationOrder < Compare.m_lDesignationOrder);
	}
}

//==============================================================================
//
// 	Function Name:	CTextLine::operator > ()
//
// 	Description:	This is an overloaded version of the > operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			None
//
//==============================================================================
BOOL CTextLine::operator > (const CTextLine& Compare)
{
	if(m_lDesignationOrder == Compare.m_lDesignationOrder)
	{
		if(m_lPageNum == Compare.m_lPageNum)
		{
			return (m_lLineNum > Compare.m_lLineNum);
		}
		else
		{
			return (m_lPageNum > Compare.m_lPageNum);
		}
	}
	else
	{
		return (m_lDesignationOrder > Compare.m_lDesignationOrder);
	}
}

//==============================================================================
//
// 	Function Name:	CTextLine::operator == ()
//
// 	Description:	This is an overloaded version of the == operator.
//
// 	Returns:		TRUE if the objects are equal.
//
//	Notes:			None
//
//==============================================================================
BOOL CTextLine::operator == (const CTextLine& Compare)
{
	return ((m_lDesignationOrder == Compare.m_lDesignationOrder) &&
	        (m_lPageNum == Compare.m_lPageNum) && 
	        (m_lLineNum == Compare.m_lLineNum));
}

//==============================================================================
//
// 	Function Name:	CTextLines::Add()
//
// 	Description:	This function will add an object to the list
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTextLines::Add(CTextLine* pLine)
{
	POSITION	Pos;
	POSITION	Prev;
	CTextLine*		pCurrent;

	ASSERT(pLine);
	if(!pLine)
		return FALSE;

	try
	{
		//	Add at the head of the list if the list is empty
		if(IsEmpty())
		{
			AddHead(pLine);
			return TRUE;
		}

		//	Do a quick check to see if this should be put at the end of the 
		//	collection
		if(*pLine > *((CTextLine*)GetTail()))
		{
			AddTail(pLine);
			return TRUE;
		}

		//	Look for the correct position
		Pos = GetHeadPosition();
		Prev = Pos;
		while(Pos != NULL)
		{
			if((pCurrent = (CTextLine*)GetNext(Pos)) == NULL)
				continue;

			//	Should we insert here?
			if(*pLine < *pCurrent)
			{
				InsertBefore(Prev, pLine);
				return TRUE;
			}
		
			Prev = Pos;	
		}

		//	If we made it this far we must have to add it to the end of the list
		AddTail(pLine);
		return TRUE;

	}
	catch(...)
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CTextLines::Add()
//
// 	Description:	This function will add all lines in the specified page
//					to the list.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTextLines::Add(CTextPage* pPage)
{
	POSITION	Pos;
	CTextLine*	pLine;
	BOOL		bSuccess = TRUE;

	ASSERT(pPage);
	if(!pPage)
		return FALSE;

	//	Add each line
	Pos = pPage->m_Lines.GetHeadPosition();
	while(Pos)
	{
		if((pLine = (CTextLine*)pPage->m_Lines.GetNext(Pos)) != 0)
			if(!Add(pLine))
				bSuccess = FALSE;
	}

	return bSuccess;
}

//==============================================================================
//
// 	Function Name:	CTextLines::Add()
//
// 	Description:	This function will add all lines in each page in the list
//					to this list.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTextLines::Add(CTextPages* pPages)
{
	POSITION	Pos;
	CTextPage*	pPage;
	BOOL		bSuccess = TRUE;

	ASSERT(pPages);
	if(!pPages)
		return FALSE;

	//	Add each line
	Pos = pPages->GetHeadPosition();
	while(Pos)
	{
		if((pPage = (CTextPage*)pPages->GetNext(Pos)) != 0)
			if(!Add(pPage))
				bSuccess = FALSE;
	}

	return bSuccess;
}

//==============================================================================
//
// 	Function Name:	CTextLine::CTextLines()
//
// 	Description:	This is the constructor for CTextLines objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTextLines::CTextLines()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CTextLine::~CTextLines()
//
// 	Description:	This is the destructor for CTextLines objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTextLines::~CTextLines()
{
	//	Flush the list and destroy it's objects
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CTextLines::Find()
//
// 	Description:	This function will search the list for the object with the
//					line number specified by the caller.
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			The local position markers are not modified with a call to
//					this function.
//
//==============================================================================
CTextLine* CTextLines::Find(long lLineNum)
{
	POSITION	Pos;
	CTextLine*	pLine;

	//	Get the first position
	Pos = GetHeadPosition();

	//	Check each object until we find a match
	while(Pos != NULL)
	{
		if((pLine = (CTextLine*)GetNext(Pos)) != 0)
		{
			if(pLine->m_lLineNum == lLineNum)
				return pLine;
		}
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CTextLines::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CTextLines::Find(CTextLine* pLine)
{
	return (CObList::Find(pLine));
}

//==============================================================================
//
// 	Function Name:	CTextLines::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CTextLine* CTextLines::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CTextLine*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CTextLines::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CTextLines::Flush(BOOL bDelete, long lDelete)
{
	CTextLine* pLine;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pLine = (CTextLine*)GetNext(m_NextPos)) != 0)
			{
				//	Do we have a specific id?
				if(lDelete != 0)
				{
					if(pLine->m_lDesignationId == lDelete)
						delete pLine;
				}
				else
				{
					delete pLine;
				}
			}
		}
	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CTextLines::GetFrame()
//
// 	Description:	This function will get the frame number that corresponds
//					to the requested line.
//
// 	Returns:		The frame number if found. -1 otherwise
//
//	Notes:			None
//
//==============================================================================
double CTextLines::GetTime(long lLine, BOOL bStartLine)
{
	CTextLine* pLine = Find(lLine);

	if(pLine == 0)
	{ 
		return -1.0;
	}
	else
	{
		//	Are we searching for the start of the line?
		if(bStartLine)
			return pLine->m_dStartTime;
		else
			return pLine->m_dStopTime;
	}
}

//==============================================================================
//
// 	Function Name:	CTextLines::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CTextLine* CTextLines::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CTextLine*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CTextLines::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CTextLine* CTextLines::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CTextLine*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CTextLines::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CTextLine* CTextLines::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CTextLine*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CTextLines::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTextLines::Remove(CTextLine* pLine, BOOL bDelete)
{
	POSITION Pos = Find(pLine);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pLine;
	}
}

//==============================================================================
//
// 	Function Name:	CTextLines::SetPos()
//
// 	Description:	This function will set the position of the local iterator
//					on the line specified by the caller
//
// 	Returns:		A pointer to object if found
//
//	Notes:			None
//
//==============================================================================
CTextLine* CTextLines::SetPos(CTextLine* pLine)
{
	CTextLine* pCurrent;

	pCurrent = First();
	while(pCurrent)
	{
		if(pCurrent == pLine)
			return pLine;
		else
			pCurrent = Next();
	}

	return 0;
}

