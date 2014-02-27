//==============================================================================
//
// File Name:	video.cpp
//
// Description:	This file contains member functions of the CVideo and
//				CVideos classes
//
// See Also:	video.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	04-04-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <video.h>

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
// 	Function Name:	CVideo::CVideo()
//
// 	Description:	This is the constructor for CVideo objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CVideo::CVideo()
{
	m_lVideoFileId = 0;
	m_lTranscriptId = 0;
	m_lBeginNum = 0;
	m_lEndNum = 0;
	m_lUnitType = 0;
	m_lMinSelStart = 0;
	m_lMaxSelStart = 0;
	m_lPrimaryMediaId = 0;
	m_lBarcodeId = 0;
	m_lChildren = 0;
	m_lAttributes = 0;
	m_lAliasId = 0;
	m_dStartPosition = 0;
	m_dStopPosition = 0;
	m_strRelativePath.Empty();
	m_strRootOverride.Empty();
	m_strFilename.Empty();
}

//==============================================================================
//
// 	Function Name:	CVideo::~CVideo()
//
// 	Description:	This is the destructor for CVideo objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CVideo::~CVideo()
{
}

//==============================================================================
//
// 	Function Name:	CVideo::operator < ()
//
// 	Description:	This is an overloaded version of the < operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			None
//
//==============================================================================
BOOL CVideo::operator < (const CVideo& Compare)
{
	return (m_lVideoFileId < Compare.m_lVideoFileId);
}

//==============================================================================
//
// 	Function Name:	CVideo::operator == ()
//
// 	Description:	This is an overloaded version of the == operator.
//
// 	Returns:		TRUE if the objects are equal.
//
//	Notes:			None
//
//==============================================================================
BOOL CVideo::operator == (const CVideo& Compare)
{
	return (m_lVideoFileId == Compare.m_lVideoFileId);
}

//==============================================================================
//
// 	Function Name:	CVideos::Add()
//
// 	Description:	This function will add an object to the list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CVideos::Add(CVideo* pVideo)
{
/*
	POSITION	Pos;
	POSITION	Prev;
	CVideo*		pCurrent;

	//	Add at the head of the list if the list is empty
	if(IsEmpty())
	{
		AddHead(pVideo);
		return;
	}

	//	Look for the correct position
	Pos = GetHeadPosition();
	Prev = Pos;
	while(Pos != NULL)
	{
		if((pCurrent = (CVideo*)GetNext(Pos)) == NULL)
			continue;

		if(*pVideo < *pCurrent)
		{
			InsertBefore(Prev, pVideo);
			return;
		}
		
		Prev = Pos;	
	}
*/
	//	If we made it this far we must have to add it to the end of the list
	AddTail(pVideo);
}

//==============================================================================
//
// 	Function Name:	CVideo::CVideos()
//
// 	Description:	This is the constructor for CVideos objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CVideos::CVideos()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CVideo::~CVideos()
//
// 	Description:	This is the destructor for CVideos objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CVideos::~CVideos()
{
	//	Flush the list and destroy it's objects
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CVideos::Find()
//
// 	Description:	This function will search the list for the item with the
//					file identifier specified by the caller.
//
// 	Returns:		A pointer to the object if found
//
//	Notes:			The local position markers are not modified with a call to
//					this function.
//
//==============================================================================
CVideo* CVideos::Find(long lVideoFileId)
{
	CVideo*		pVideo;
	POSITION	Pos;

	Pos = GetHeadPosition();
	while(Pos != NULL)
	{
		if((pVideo = (CVideo*)GetNext(Pos)) != 0)
		{
			if(pVideo->m_lVideoFileId == lVideoFileId)
				return pVideo;
		}
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CVideos::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CVideos::Find(CVideo* pVideo)
{
	return (CObList::Find(pVideo));
}

//==============================================================================
//
// 	Function Name:	CVideos::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CVideo* CVideos::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CVideo*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CVideos::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CVideos::Flush(BOOL bDelete)
{
	CVideo* pVideo;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pVideo = (CVideo*)GetNext(m_NextPos)) != 0)
				delete pVideo;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CVideos::GetVideoFile()
//
// 	Description:	This function will get the fully qualified file path for
//					the video with the specified identifier.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CVideos::GetVideoFile(long lVideoFileId, CString& rFilespec)
{
	CVideo*	pVideo = Find(lVideoFileId);
	
	//	Did we find the desired video?
	if(pVideo == 0)
		return FALSE;

	//	Build the file specification
	if(pVideo->m_strRootOverride.IsEmpty())
	{
		rFilespec = m_strRootFolder;
	}
	else
	{
		rFilespec = pVideo->m_strRootOverride;
		if(rFilespec.Right(1) != "\\")
			rFilespec += "\\";
	}
	if(!rFilespec.IsEmpty() && rFilespec.Right(1) != "\\")
		rFilespec += "\\";
	rFilespec += pVideo->m_strRelativePath;
	if(!rFilespec.IsEmpty() && rFilespec.Right(1) != "\\")
		rFilespec += "\\";
	rFilespec += pVideo->m_strFilename;

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CVideos::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CVideo* CVideos::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CVideo*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CVideos::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CVideo* CVideos::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CVideo*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CVideos::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CVideo* CVideos::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CVideo*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CVideos::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CVideos::Remove(CVideo* pVideo, BOOL bDelete)
{
	POSITION Pos = Find(pVideo);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pVideo;
	}
}

//==============================================================================
//
// 	Function Name:	CVideos::SetRootFolder()
//
// 	Description:	This function is called to set the root folder for all
//					videos stored in the list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CVideos::SetRootFolder(LPCSTR lpFolder)
{
	if(lpFolder != 0)
		m_strRootFolder = lpFolder;
	else
		m_strRootFolder.Empty();
}


