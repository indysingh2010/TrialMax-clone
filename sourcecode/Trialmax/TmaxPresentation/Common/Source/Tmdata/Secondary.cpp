//==============================================================================
//
// File Name:	secondary.cpp
//
// Description:	This file contains member functions of the CSecondary and
//				CSecondaries classes
//
// See Also:	secondary.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-18-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <secondary.h>

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
// 	Function Name:	CSecondary::CSecondary()
//
// 	Description:	This is the constructor for CSecondary objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CSecondary::CSecondary()
{
	m_lSecondaryId = 0;
	m_lPlaybackOrder = 0;
	m_lSlideId = 0;
	m_lDisplayType = 0;
	m_strMediaId.Empty();
	m_strFilename.Empty();

	m_lPrimaryId = 0;
	m_lBarcodeId = 0;
	m_lChildren = 0;
	m_lAliasId = 0;
	m_lAttributes = 0;
	m_lStartLine = -1;
	m_lStartPage = -1;
	m_lStopLine	= -1;
	m_lStopPage	= -1;
	m_lXmlSegmentId = 0;
	m_dStartTime = -1.0;
	m_dStopTime	= -1.0;
	m_sMediaType = 0;
	m_bLinked = FALSE;
	m_strDescription.Empty();
	m_strName.Empty();
	m_strRelativePath.Empty();
}

//==============================================================================
//
// 	Function Name:	CSecondary::~CSecondary()
//
// 	Description:	This is the destructor for CSecondary objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CSecondary::~CSecondary()
{
	m_Children.Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CSecondary::MsgBox()
//
// 	Description:	This is for debugging purposes to view the values assigned
//					to the class members
//
// 	Returns:		MB_OK or MB_CANCEL
//
//	Notes:			None
//
//==============================================================================
UINT CSecondary::MsgBox(HWND hWnd, LPCSTR lpszTitle)
{
	CString	strTemp = "";
	CString	strTitle = "";
	CString strMsg = "";

	strTemp.Format("MediaId: %s\n", m_strMediaId);
	strMsg += strTemp;

	strTemp.Format("PrimaryId: %ld\n", m_lPrimaryId);
	strMsg += strTemp;

	strTemp.Format("SecondaryId: %ld\n", m_lSecondaryId);
	strMsg += strTemp;

	strTemp.Format("BarcodeId: %ld\n", m_lBarcodeId);
	strMsg += strTemp;

	strTemp.Format("PlaybackOrder: %ld\n", m_lPlaybackOrder);
	strMsg += strTemp;

	strTemp.Format("SlideId: %ld\n", m_lSlideId);
	strMsg += strTemp;

	strTemp.Format("DisplayType: %ld\n", m_lDisplayType);
	strMsg += strTemp;

	strTemp.Format("XmlSegmentId: %ld\n", m_lXmlSegmentId);
	strMsg += strTemp;

	strTemp.Format("Children: %ld\n", m_lChildren);
	strMsg += strTemp;

	strTemp.Format("Attributes: %ld\n", m_lAttributes);
	strMsg += strTemp;

	strTemp.Format("MediaType: %d\n", m_sMediaType);
	strMsg += strTemp;

	strTemp.Format("AliasId: %ld\n", m_lAliasId);
	strMsg += strTemp;

	strTemp.Format("Linked: %d\n", m_bLinked);
	strMsg += strTemp;

	strMsg += "\n\n";

	strTemp.Format("StartPage: %ld\n", m_lStartPage);
	strMsg += strTemp;

	strTemp.Format("StartLine: %ld\n", m_lStartLine);
	strMsg += strTemp;

	strTemp.Format("StopPage: %ld\n", m_lStopPage);
	strMsg += strTemp;

	strTemp.Format("StopLine: %ld\n", m_lStopLine);
	strMsg += strTemp;

	strTemp.Format("StartTime: %.4fd\n", m_dStartTime);
	strMsg += strTemp;

	strTemp.Format("StopTime: %.4fd\n", m_dStopTime);
	strMsg += strTemp;

	strMsg += "\n\n";

	strTemp.Format("RelativePath: %s\n", m_strRelativePath);
	strMsg += strTemp;

	strTemp.Format("Filename: %s\n", m_strFilename);
	strMsg += strTemp;

	strTemp.Format("Name: %s\n", m_strName);
	strMsg += strTemp;

	strTemp.Format("Description: %s\n", m_strDescription);
	strMsg += strTemp;

	if(lpszTitle != NULL)
		strTitle = lpszTitle;

	return MessageBox(hWnd, strMsg, strTitle, MB_ICONINFORMATION | MB_OKCANCEL);
}

//==============================================================================
//
// 	Function Name:	CSecondary::operator < ()
//
// 	Description:	This is an overloaded version of the < operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			None
//
//==============================================================================
BOOL CSecondary::operator < (const CSecondary& Compare)
{
	return (m_lPlaybackOrder < Compare.m_lPlaybackOrder);
}

//==============================================================================
//
// 	Function Name:	CSecondary::operator > ()
//
// 	Description:	This is an overloaded version of the > operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			None
//
//==============================================================================
BOOL CSecondary::operator > (const CSecondary& Compare)
{
	return (m_lPlaybackOrder > Compare.m_lPlaybackOrder);
}

//==============================================================================
//
// 	Function Name:	CSecondary::operator == ()
//
// 	Description:	This is an overloaded version of the == operator.
//
// 	Returns:		TRUE if the objects are equal.
//
//	Notes:			None
//
//==============================================================================
BOOL CSecondary::operator == (const CSecondary& Compare)
{
	return (m_lPlaybackOrder == Compare.m_lPlaybackOrder);
}

//==============================================================================
//
// 	Function Name:	CSecondaries::Add()
//
// 	Description:	This function will add an object to the list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSecondaries::Add(CSecondary* pSecondary, BOOL bSorted)
{
	POSITION	Pos;
	POSITION	Prev;
	CSecondary*		pCurrent;

	//	Add at the head of the list if the list is empty
	if(IsEmpty())
	{
		AddHead(pSecondary);
		return;
	}

	//	Look for the correct position if we are sorting the list
	if(bSorted && (*((CSecondary*)GetTail()) > *pSecondary))
	{
		Pos  = GetHeadPosition();
		Prev = Pos;
		while(Pos != NULL)
		{
			if((pCurrent = (CSecondary*)GetNext(Pos)) == NULL)
				continue;

			if(*pSecondary < *pCurrent)
			{
				InsertBefore(Prev, pSecondary);
				return;
			}
			
			Prev = Pos;	
		}
	}

	//	If we made it this far we must have to add it to the end of the list
	AddTail(pSecondary);
}

//==============================================================================
//
// 	Function Name:	CSecondary::CSecondaries()
//
// 	Description:	This is the constructor for CSecondaries objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CSecondaries::CSecondaries()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CSecondary::~CSecondaries()
//
// 	Description:	This is the destructor for CSecondaries objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CSecondaries::~CSecondaries()
{
	//	Flush the list and destroy it's objects
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CSecondaries::FindByBarcodeId()
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
CSecondary* CSecondaries::FindByBarcodeId(long lId)
{
	CSecondary* pSecondary = First();
	while(pSecondary)
	{
		if(pSecondary->m_lBarcodeId == lId)
			return pSecondary;
		else
			pSecondary = Next();
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CSecondaries::FindByDatabaseId()
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
CSecondary* CSecondaries::FindByDatabaseId(long lId)
{
	CSecondary* pSecondary = First();
	while(pSecondary)
	{
		if(pSecondary->m_lSecondaryId == lId)
			return pSecondary;
		else
			pSecondary = Next();
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CSecondaries::FindByOrder()
//
// 	Description:	This function will search the list for the item with the
//					playback order specified by the caller.
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			None
//
//==============================================================================
CSecondary* CSecondaries::FindByOrder(long lOrder)
{
	CSecondary* pSecondary = First();
	while(pSecondary)
	{
		if(pSecondary->m_lPlaybackOrder == lOrder)
			return pSecondary;
		else
			pSecondary = Next();
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CSecondaries::FindBySlide()
//
// 	Description:	This function will search the list for the item with the
//					slide id specified by the caller.
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			None
//
//==============================================================================
CSecondary* CSecondaries::FindBySlide(long lSlide)
{
	CSecondary* pSecondary = First();
	while(pSecondary)
	{
		if(pSecondary->m_lSlideId == lSlide)
			return pSecondary;
		else
			pSecondary = Next();
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CSecondaries::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CSecondaries::Find(CSecondary* pSecondary)
{
	return (CObList::Find(pSecondary));
}

//==============================================================================
//
// 	Function Name:	CSecondaries::FindNext()
//
// 	Description:	This function will locate the next object without modifying
//					the local iterator
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			None
//
//==============================================================================
CSecondary* CSecondaries::FindNext(CSecondary* pSecondary)
{
	POSITION Pos;

	if(pSecondary != 0)
	{
		//	Find the position of the specified page
		if((Pos = Find(pSecondary)) != NULL)
		{
			//	Now get the next page
			GetNext(Pos);
			if(Pos != NULL)
				return (CSecondary*)GetAt(Pos);
		}
	}
	
	return 0;
}

//==============================================================================
//
// 	Function Name:	CSecondaries::FindPrev()
//
// 	Description:	This function will locate the previous object without 
//					changing the local iterator
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			None
//
//==============================================================================
CSecondary* CSecondaries::FindPrev(CSecondary* pSecondary)
{
	POSITION Pos;

	if(pSecondary != 0)
	{
		//	Find the position of the specified page
		if((Pos = Find(pSecondary)) != NULL)
		{
			//	Now get the previous page
			GetPrev(Pos);
			if(Pos != NULL)
				return (CSecondary*)GetAt(Pos);
		}
	}
	
	return 0;
}

//==============================================================================
//
// 	Function Name:	CSecondaries::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CSecondary* CSecondaries::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CSecondary*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CSecondaries::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CSecondaries::Flush(BOOL bDelete)
{
	CSecondary* pSecondary;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pSecondary = (CSecondary*)GetNext(m_NextPos)) != 0)
				delete pSecondary;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CSecondaries::IsFirst()
//
// 	Description:	This function is called to determine if the object specified
//					by the caller is the first one in the list.
//
// 	Returns:		TRUE if first in list
//
//	Notes:			None
//
//==============================================================================
BOOL CSecondaries::IsFirst(CSecondary* pSecondary)
{
	POSITION Pos = GetHeadPosition();

	if(Pos != NULL)
		return (pSecondary == (CSecondary*)GetNext(Pos));
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CSecondaries::IsLast()
//
// 	Description:	This function is called to determine if the object specified
//					by the caller is the last one in the list.
//
// 	Returns:		TRUE if last in list
//
//	Notes:			None
//
//==============================================================================
BOOL CSecondaries::IsLast(CSecondary* pSecondary)
{
	POSITION Pos = GetTailPosition();

	if(Pos != NULL)
		return (pSecondary == (CSecondary*)GetPrev(Pos));
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CSecondaries::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CSecondary* CSecondaries::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CSecondary*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CSecondaries::MsgBox()
//
// 	Description:	This is for debugging purposes to view the values assigned
//					to the class members
//
// 	Returns:		MB_OK or MB_CANCEL
//
//	Notes:			None
//
//==============================================================================
UINT CSecondaries::MsgBox(HWND hWnd, LPCSTR lpszTitle)
{
	CSecondary* pSecondary = NULL;
	POSITION	Pos = NULL;

	if(GetCount() > 0)
	{
		Pos = GetHeadPosition();

		while(Pos != NULL)
		{
			if((pSecondary = (CSecondary*)GetNext(Pos)) != NULL)
			{
				if(pSecondary->MsgBox(hWnd, lpszTitle) == IDCANCEL)
					return IDCANCEL;
			}

		}

		return IDOK;
	}
	else
	{
		CString strTitle = "";

		if(lpszTitle != NULL)
			strTitle = lpszTitle;

		return MessageBox(hWnd, "EMPTY", strTitle, MB_ICONINFORMATION | MB_OKCANCEL);
	}

}

//==============================================================================
//
// 	Function Name:	CSecondaries::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CSecondary* CSecondaries::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CSecondary*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CSecondaries::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CSecondary* CSecondaries::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CSecondary*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CSecondaries::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSecondaries::Remove(CSecondary* pSecondary, BOOL bDelete)
{
	POSITION Pos = Find(pSecondary);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pSecondary;
	}
}

