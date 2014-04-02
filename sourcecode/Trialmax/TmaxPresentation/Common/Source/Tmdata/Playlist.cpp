//==============================================================================
//
// File Name:	playlist.cpp
//
// Description:	This file contains member functions of the CLink, CLinks,
//				CDesignation, CDesignations, CPlaylist, and CPlaylists classes.
//
// See Also:	playlist.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	07-09-97	1.00		Original Release
//	01-10-99	3.00		Added transcript management members
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <playlist.h>

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
// 	Function Name:	CPlaylist::CPlaylist()
//
// 	Description:	This is the constructor for CPlaylist objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPlaylist::CPlaylist(CMedia* pMedia) : CMedia(pMedia)
{
	//	Initialize the class members
	m_pVideos = 0;
	m_bIsRecording = FALSE;
	m_bIsDeposition = FALSE;
	m_bOwnsVideos = FALSE;
	m_bHasText = FALSE;
}

//==============================================================================
//
// 	Function Name:	CPlaylist::~CPlaylist()
//
// 	Description:	This is the destructor for CPlaylist objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPlaylist::~CPlaylist()
{
	//	Flush the designation list
	m_Designations.Flush(TRUE);

	//	Deallocate the video list
	if(m_bOwnsVideos && (m_pVideos != 0))
		delete m_pVideos;
}

//==============================================================================
//
// 	Function Name:	CPlaylist::GetDesignation()
//
// 	Description:	This function will retrieve the designation at with the id
//					specified by the caller.
//
// 	Returns:		NULL if not found
//
//	Notes:			None
//
//==============================================================================
CDesignation* CPlaylist::GetDesignationFromId(long lId)
{
	POSITION		Pos;
	CDesignation*	pDesignation;

	//	Get the first position
	Pos = m_Designations.GetHeadPosition();

	while(Pos != NULL)
	{
		pDesignation = (CDesignation*)m_Designations.GetNext(Pos);

		if(pDesignation && (pDesignation->m_lTertiaryId == lId))
			return pDesignation;
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CPlaylist::GetDesignation()
//
// 	Description:	This function will retrieve the designation with the 
//					playback order provided by the caller.
//
// 	Returns:		NULL if not found
//
//	Notes:			If the order is less than zero, the first designation 
//					is returned.
//
//					If bForward == TRUE, the designation is retrieved and
//					the list is set up for forward iteration. If FALSE, the
//					designation is retrieved and the list is set up for
//					reverse iteration.
//
//==============================================================================
CDesignation* CPlaylist::GetDesignationFromOrder(long lOrder, BOOL bForward)
{
	CDesignation* pDesignation;

	if(lOrder < 0)
		return GetFirstDesignation();

	//	Do we want to iterate from the head to the tail in search of 
	//	the designation?
	if(bForward)
	{
		//	Get the first position
		pDesignation = m_Designations.First();

		while(pDesignation)
		{
			if(pDesignation && (pDesignation->m_lPlaybackOrder == lOrder))
				return pDesignation;
			else
				pDesignation = m_Designations.Next();
		}
	}
	else
	{
		//	Get the last position
		pDesignation = m_Designations.Last();

		while(pDesignation)
		{
			if(pDesignation && (pDesignation->m_lPlaybackOrder == lOrder))
				return pDesignation;
			else
				pDesignation = m_Designations.Prev();
		}
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CPlaylist::GetDesignationCount()
//
// 	Description:	This function will retrieve the total number of designations
//					in the list
//
// 	Returns:		The number of designations
//
//	Notes:			None
//
//==============================================================================
int CPlaylist::GetDesignationCount()
{
	return m_Designations.GetCount();
}

//==============================================================================
//
// 	Function Name:	CPlaylist::GetFirstDesignation()
//
// 	Description:	This function will retrieve the first Designation object in
//					the list.
//
// 	Returns:		A pointer to the first object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CDesignation* CPlaylist::GetFirstDesignation()
{
	return m_Designations.First();
}

//==============================================================================
//
// 	Function Name:	CPlaylist::GetFirstInRange()
//
// 	Description:	This function will retrieve the first designation containing
//					the page-line provided by the caller
//
// 	Returns:		A pointer to the first designation that has the page-line
//					if found
//
//	Notes:			None
//
//==============================================================================
CDesignation* CPlaylist::GetFirstInRange(int iPage, int iLine, long lTranscript)
{
	return m_Designations.GetFirstInRange(iPage, iLine, lTranscript);
}

//==============================================================================
//
// 	Function Name:	CPlaylist::GetLastDesignation()
//
// 	Description:	This function will retrieve the last Designation object in the
//					list.
//
// 	Returns:		A pointer to the last object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CDesignation* CPlaylist::GetLastDesignation()
{
	return m_Designations.Last();
}

//==============================================================================
//
// 	Function Name:	CPlaylist::GetNextDesignation()
//
// 	Description:	This function will retrieve the next Designation object in the
//					list.
//
// 	Returns:		A pointer to the next object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CDesignation* CPlaylist::GetNextDesignation()
{
	return m_Designations.Next();
}

//==============================================================================
//
// 	Function Name:	CPlaylist::GetNextDesignation()
//
// 	Description:	This function will retrieve the next designation object in 
//					list without disrupting the position of the local iterator
//
// 	Returns:		A pointer to the next object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CDesignation* CPlaylist::GetNextDesignation(CDesignation* pDesignation)
{
	return m_Designations.Next(pDesignation);
}

//==============================================================================
//
// 	Function Name:	CPlaylist::GetPrevDesignation()
//
// 	Description:	This function will retrieve the previous Designation object in the
//					list.
//
// 	Returns:		A pointer to the previous object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CDesignation* CPlaylist::GetPrevDesignation()
{
	return m_Designations.Prev();
}

//==============================================================================
//
// 	Function Name:	CPlaylist::GetPrevDesignation()
//
// 	Description:	This function will retrieve the previous designation object
//					int the list without disrupting the position of the local 
//					iterator
//
// 	Returns:		A pointer to the next object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CDesignation* CPlaylist::GetPrevDesignation(CDesignation* pDesignation)
{
	return m_Designations.Prev(pDesignation);
}

//==============================================================================
//
// 	Function Name:	CPlaylist::GetTime()
//
// 	Description:	This function will compute the time required to play the
//					designations from the specifed start position to the 
//					specified stop position
//
// 	Returns:		The playback time in seconds
//
//	Notes:			None
//
//==============================================================================
double CPlaylist::GetTime(long lStartOrder, double dStartPosition,
						  long lStopOrder, double dStopPosition)
{
	return m_Designations.GetTime(lStartOrder, dStartPosition,
								  lStopOrder, dStopPosition);
}

//==============================================================================
//
// 	Function Name:	CPlaylist::GetTotalTime()
//
// 	Description:	This function will compute the time required to play all
//					designations in the playlist.
//
// 	Returns:		The cummulative time in seconds
//
//	Notes:			None
//
//==============================================================================
double CPlaylist::GetTotalTime()
{
	return m_Designations.GetTotalTime();
}

//==============================================================================
//
// 	Function Name:	CPlaylist::GetVideoFile()
//
// 	Description:	This function will retrieve the full file specified for the
//					specified video file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPlaylist::GetVideoFile(long lFileId, CString& rFilespec)
{
	if(m_pVideos)
		return m_pVideos->GetVideoFile(lFileId, rFilespec);
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CPlaylist::operator < ()
//
// 	Description:	This is an overloaded version of the < operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			None
//
//==============================================================================
BOOL CPlaylist::operator < (const CPlaylist& Compare)
{
	return m_lPrimaryId < Compare.m_lPrimaryId;
}

//==============================================================================
//
// 	Function Name:	CPlaylist::operator == ()
//
// 	Description:	This is an overloaded version of the == operator.
//
// 	Returns:		TRUE if the objects are equal.
//
//	Notes:			None
//
//==============================================================================
BOOL CPlaylist::operator == (const CPlaylist& Compare)
{
	return m_lPrimaryId == Compare.m_lPrimaryId;
}

//==============================================================================
//
// 	Function Name:	CPlaylist::ScrollingTextEnabled()
//
// 	Description:	This function is called to determine if the text associated
//					with the playlist should be shown when the playlist is
//					executed.
//
// 	Returns:		TRUE if text should be shown.
//
//	Notes:			None
//
//==============================================================================
BOOL CPlaylist::ScrollingTextEnabled()
{
	//	NOTE:	The UseTranscript field in the database is used to indicate if
	//			the transcript text should be shown when the playlist is launched
	return (GetHasText());
}

//==============================================================================
//
// 	Function Name:	CPlaylists::Add()
//
// 	Description:	This function will add an object to the list
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPlaylists::Add(CPlaylist* pPlaylist)
{
	ASSERT(pPlaylist);
	if(!pPlaylist)
		return FALSE;

	//	MFC will throw a memory exception if it can't add the object to the list
	try
	{
		//	Add the link to the end of the list
		AddTail(pPlaylist);
		return TRUE;
	}
	catch(...)
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CPlaylist::CPlaylists()
//
// 	Description:	This is the constructor for CPlaylists objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPlaylists::CPlaylists()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CPlaylist::~CPlaylists()
//
// 	Description:	This is the destructor for CPlaylists objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPlaylists::~CPlaylists()
{
	//	Flush the list and destroy it's objects
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CPlaylists::Find()
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
CPlaylist* CPlaylists::Find(LPCSTR lpId)
{
	POSITION	Pos;
	CPlaylist*		pPlaylist;

	//	Get the first position
	Pos = GetHeadPosition();

	//	Check each item until we find a match
	while(Pos != NULL)
	{
		if((pPlaylist = (CPlaylist*)GetNext(Pos)) != 0)
		{
			if(lstrcmpi(pPlaylist->m_strMediaId, lpId) == 0)
				return pPlaylist;
		}
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CPlaylists::Find()
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
CPlaylist* CPlaylists::Find(long lId)
{
	POSITION	Pos;
	CPlaylist*		pPlaylist;

	//	Get the first position
	Pos = GetHeadPosition();

	//	Check each item until we find a match
	while(Pos != NULL)
	{
		if((pPlaylist = (CPlaylist*)GetNext(Pos)) != 0)
		{
			if(pPlaylist->m_lPrimaryId == lId)
				return pPlaylist;
		}
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CPlaylists::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CPlaylists::Find(CPlaylist* pPlaylist)
{
	return (CObList::Find(pPlaylist));
}

//==============================================================================
//
// 	Function Name:	CPlaylists::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CPlaylist* CPlaylists::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CPlaylist*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CPlaylists::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CPlaylists::Flush(BOOL bDelete)
{
	CPlaylist* pPlaylist;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pPlaylist = (CPlaylist*)GetNext(m_NextPos)) != 0)
				delete pPlaylist;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CPlaylists::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CPlaylist* CPlaylists::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CPlaylist*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CPlaylists::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CPlaylist* CPlaylists::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CPlaylist*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CPlaylists::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CPlaylist* CPlaylists::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CPlaylist*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CPlaylists::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPlaylists::Remove(CPlaylist* pPlaylist, BOOL bDelete)
{
	POSITION Pos = Find(pPlaylist);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pPlaylist;
	}
}

