//==============================================================================
//
// File Name:	dbabstract.cpp
//
// Description:	This file contains member functions of the CDBAbstract class.
//
// See Also:	dbabstract.h
//
//==============================================================================
//	Date		Revision    Description
//	12-26-2003	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <dbabstract.h>
#include <dbdefs.h>
#include <shellapi.h>
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

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------

//==============================================================================
//
// 	Function Name:	CDBAbstract::Close()
//
// 	Description:	This function will delete the record sets and close the
//					database.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBAbstract::Close()
{
	//	Close the database
	if(m_pDatabase != 0)
	{
		if(m_pDatabase->IsOpen())
			m_pDatabase->Close();

		delete m_pDatabase;
		m_pDatabase = 0;
	}

	//	Flush the media lists
	m_Playlists.Flush(TRUE);
	m_Multipages.Flush(TRUE);

	//	Flush the list of transcripts
	m_Transcripts.Flush(TRUE);

	//	Reset the video list
	m_Videos.Flush(TRUE);
	m_Videos.SetRootFolder(0);

	//	Clear the folder and file specifications
	m_strRootFolder.Empty();
	m_strFilename.Empty();
	m_strFilespec.Empty();
	m_strRootPendingFolder.Empty();
	m_strTreatmentPendingFolder.Empty();

	//	Clear the revision information
	m_strRevision.Empty();
	m_strCreator.Empty();
	m_iMajorVer = -1;
	m_iMinorVer = -1;

	//	NOTE:	DO NOT clear the last error because we may be closing the
	//			database in response to the error.
}

//==============================================================================
//
// 	Function Name:	CDBAbstract::ConvertToPlaylist()
//
// 	Description:	This function convert the deposition media object to a
//					playlist.
//
// 	Returns:		A pointer to the playlist object if successful
//
//	Notes:			None
//
//==============================================================================
CPlaylist* CDBAbstract::ConvertToPlaylist(CDeposition* pDeposition)
{
	ASSERT(pDeposition);
	return NULL;
}

//==============================================================================
//
// 	Function Name:	CDBAbstract::ConvertToPlaylist()
//
// 	Description:	This function convert the multipage media object to a
//					playlist.
//
// 	Returns:		A pointer to the playlist object if successful
//
//	Notes:			None
//
//==============================================================================
CPlaylist* CDBAbstract::ConvertToPlaylist(CMultipage* pMultipage)
{
	ASSERT(pMultipage);
	return NULL;
}

//==============================================================================
//
// 	Function Name:	CDBAbstract::CDBAbstract()
//
// 	Description:	This is the constructor for CDBAbstract objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDBAbstract::CDBAbstract()
{
	m_pDatabase = 0;
	m_iLastError = 0;
	m_iMajorVer = -1;
	m_iMinorVer = -1;
	m_lScriptId = 0;
	m_lSceneId = 0;
	m_strRootFolder.Empty();
	m_strFilename.Empty();
	m_strFilespec.Empty();
	m_strRootPendingFolder.Empty();
	m_strTreatmentPendingFolder.Empty();
	m_strRevision.Empty();
	m_strCreator.Empty();

	//	Initialize the error handler
	m_Errors.Enable(TRUE);
	m_Errors.SetParent(0);
	m_Errors.SetTitle("TrialMax Database Error");
}

//==============================================================================
//
// 	Function Name:	CDBAbstract::~CDBAbstract()
//
// 	Description:	This is the destructor for CDBAbstract objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDBAbstract::~CDBAbstract()
{
	//	Make sure the case database is closed
	Close();
}

//==============================================================================
//
// 	Function Name:	CDBAbstract::CreatePendingFolders()
//
// 	Description:	This function is called to create the folders used to store
//					pending updates.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBAbstract::CreatePendingFolders()
{
	//	Assemble the path specifications
	m_strRootPendingFolder = m_strRootFolder;
	m_strRootPendingFolder += "Pending\\";
	m_strTreatmentPendingFolder = m_strRootPendingFolder + "Treatments\\";

	//	Create the folders
	CreateDirectory(m_strRootPendingFolder, 0);
	CreateDirectory(m_strTreatmentPendingFolder, 0);
}

//==============================================================================
//
// 	Function Name:	CDBAbstract::Find()
//
// 	Description:	This function is called to verify that the specified media
//					object is in one of the active lists.
//
// 	Returns:		The position of the object in the list
//
//	Notes:			None
//
//==============================================================================
POSITION CDBAbstract::Find(CMedia* pMedia)
{
	ASSERT(pMedia);
	if(pMedia == 0)
		return 0;

	//	What type of media is this?
	switch(pMedia->m_lPlayerType)
	{
		case MEDIA_TYPE_PLAYLIST:

			return m_Playlists.Find(pMedia);
							
		case MEDIA_TYPE_CUSTOMSHOW:
		case MEDIA_TYPE_IMAGE:				
		case MEDIA_TYPE_RECORDING:			
		case MEDIA_TYPE_POWERPOINT:			

			return m_Multipages.Find(pMedia);

		default:

			return 0;
	}
}

//==============================================================================
//
// 	Function Name:	CDBAbstract::FindFile()
//
// 	Description:	This function checks to see if the database file exists.
//
// 	Returns:		TRUE if the file exists.
//
//	Notes:			This function can also be called by anyone with access
//					to this object to determine if a file exists.
//
//==============================================================================
BOOL CDBAbstract::FindFile(CString& rFileSpec)
{
	WIN32_FIND_DATA	FindData;
	HANDLE			hFind;

	if((hFind = FindFirstFile(rFileSpec, &FindData)) == INVALID_HANDLE_VALUE)
	{
		return FALSE;
	}
	else
	{
		FindClose(hFind);
		return TRUE;
	}
		
}

//==============================================================================
//
// 	Function Name:	CDBAbstract::GetDeposition()
//
// 	Description:	This function is called to get the deposition object
//					identified by the CMedia object provided by the caller
//
// 	Returns:		A pointer to a deposition object if successful
//
//	Notes:			The caller is responsible for deallocation of the object
//					returned by this function.
//
//==============================================================================
CDeposition* CDBAbstract::GetDeposition(CMedia* pMedia)
{
	ASSERT(pMedia);
	return NULL;
}

//==============================================================================
//
// 	Function Name:	CDBAbstract::GetErrorHandler()
//
// 	Description:	This function is called to get the current state of the
//					error handler.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBAbstract::GetErrorHandler(BOOL* pbEnabled, HWND* phParent)
{
	ASSERT(pbEnabled);
	ASSERT(phParent);

	*pbEnabled = m_Errors.IsEnabled();
	*phParent  = m_Errors.GetParent();
}

//==============================================================================
//
// 	Function Name:	CDBAbstract::GetMedia()
//
// 	Description:	This function is called to get the media object with the id
//					specified by the caller.
//
// 	Returns:		A pointer to the media object if found.
//
//	Notes:			None
//
//==============================================================================
CMedia* CDBAbstract::GetMedia(LPCSTR lpMediaId)
{
	CMedia* pMedia;

	ASSERT(lpMediaId);
	if((lpMediaId == 0) || (lstrlen(lpMediaId) == 0))
		return 0;
	
	//	Is this a multipage object?
	if((pMedia = m_Multipages.Find(lpMediaId)) != 0)
		return pMedia;

	//	Is this a playlist object?
	if((pMedia = m_Playlists.Find(lpMediaId)) != 0)
		return pMedia;

	return 0;
}

//==============================================================================
//
// 	Function Name:	CDBAbstract::GetMedia()
//
// 	Description:	This function is called to get the media object with the id
//					specified by the caller.
//
// 	Returns:		A pointer to the media object if found.
//
//	Notes:			None
//
//==============================================================================
CMedia* CDBAbstract::GetMedia(long lId)
{
	CMedia* pMedia;

	ASSERT(lId > 0);
	
	//	Is this a multipage object?
	if((pMedia = m_Multipages.Find(lId)) != 0)
		return pMedia;

	//	Is this a playlist object?
	if((pMedia = m_Playlists.Find(lId)) != 0)
		return pMedia;

	return 0;
}

//==============================================================================
//
// 	Function Name:	CDBAbstract::GetMultipage()
//
// 	Description:	This function is called to get the multipage object
//					identified by the CMedia object provided by the caller
//
// 	Returns:		A pointer to a multipage object if found
//
//	Notes:			The caller is responsible for deallocation of the object
//					returned by this function.
//
//==============================================================================
CMultipage* CDBAbstract::GetMultipage(CMedia* pMedia)
{
	CMultipage* pMultipage;

	ASSERT(pMedia);
	
	//	Make sure this media object is in the multipage list
	if((pMedia == 0) || (m_Multipages.Find(pMedia) == NULL))
	{
		HandleError(TMDB_INVALIDPARAM, IDS_TMDB_INVALIDPARAM, "GetMultipage()");
		return 0;
	}

	//	Allocate the multipage object and set the media members
	pMultipage = new CMultipage(pMedia);

	//	Get the secondary pages associated with this object
	GetSecondaries(pMultipage);

	return pMultipage;
}

//==============================================================================
//
// 	Function Name:	CDBAbstract::GetPlaylist()
//
// 	Description:	This function is called to get the playlist object
//					identified by the CMedia object provided by the caller
//
// 	Returns:		A pointer to a playlist object if found
//
//	Notes:			The caller is responsible for deallocation of the object
//					returned by this function.
//
//==============================================================================
CPlaylist* CDBAbstract::GetPlaylist(CMedia* pMedia)
{
	CPlaylist* pPlaylist;

	ASSERT(pMedia);
	
	//	Make sure this playlist object is in the list
	if((pMedia == 0) || (m_Playlists.Find(pMedia) == NULL))
	{
		HandleError(TMDB_INVALIDPARAM, IDS_TMDB_INVALIDPARAM, "GetPlaylist()");
		return 0;
	}

	//	Allocate the playlist object and set the media members
	pPlaylist = new CPlaylist(pMedia);

	//	Set the video list
	pPlaylist->SetVideos(&m_Videos);

	//	Get the designations associated with this object
	GetDesignations(pPlaylist);

	return pPlaylist;
}

//==============================================================================
//
// 	Function Name:	CDBAbstract::GetShow()
//
// 	Description:	This function is called to get the custom show object
//					identified by the CMedia object provided by the caller
//
// 	Returns:		A pointer to a custom show object if found
//
//	Notes:			The caller is responsible for deallocation of the object
//					returned by this function.
//
//==============================================================================
CShow* CDBAbstract::GetShow(CMedia* pMedia, long lSecondary)
{
	CShow* pShow;

	ASSERT(pMedia);
	
	//	Make sure this media object is in the multipage list
	if((pMedia == 0) || (m_Multipages.Find(pMedia) == NULL))
	{
		HandleError(TMDB_INVALIDPARAM, IDS_TMDB_INVALIDPARAM, "GetMultipage()");
		return 0;
	}

	//	Allocate the custom show object and set the media members
	pShow = new CShow(pMedia);

	//	Get the show items associated with this object
	GetShowItems(pShow, lSecondary);

	return pShow;
}

//==============================================================================
//
// 	Function Name:	CDBAbstract::HandleError()
//
// 	Description:	This function is called internally to handle all errors.
//
// 	Returns:		The error level specified by the caller
//
//	Notes:			None
//
//==============================================================================
int CDBAbstract::HandleError(int iError, LPCSTR lpMessage)
{
	//	Save the error level
	m_iLastError = iError;

	if(m_iLastError != TMDB_NOERROR)
		m_Errors.Handle(0, lpMessage);

	return m_iLastError;
}

//==============================================================================
//
// 	Function Name:	CDBAbstract::HandleError()
//
// 	Description:	This function is called internally to handle all errors.
//
// 	Returns:		The error level specified by the caller
//
//	Notes:			None
//
//==============================================================================
int CDBAbstract::HandleError(int iError, UINT uString, LPCSTR lpArg1, LPCSTR lpArg2)
{
	//	Save the error level
	m_iLastError = iError;

	if(m_iLastError != TMDB_NOERROR)
		m_Errors.Handle(0, uString, lpArg1, lpArg2);

	return m_iLastError;
}

//==============================================================================
//
// 	Function Name:	CDBAbstract::IsOpen()
//
// 	Description:	This function is called to determine if the database is
//					open.
//
// 	Returns:		TRUE if open.
//
//	Notes:			None
//
//==============================================================================
BOOL CDBAbstract::IsOpen()
{
	if(m_pDatabase && m_pDatabase->IsOpen())
		return TRUE;
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CDBAbstract::OpenRecordset()
//
// 	Description:	This function will attempt to open the recordset provided
//					by the caller.
//
// 	Returns:		TMDB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
int CDBAbstract::OpenRecordset(CDaoRecordset* pSet, LPCSTR lpTable,
							     BOOL bMustExist) 
{
	ASSERT(pSet);

	//	Try to open the recordset
	try
	{
		//	Closing and reopening has the same effect as redoing the query
		if(pSet->IsOpen())
			pSet->Close();

		pSet->Open();
	}
	catch(CDaoException* e)	//	DAO errors
	{
		if(bMustExist)
			HandleError(TMDB_OPENRSFAILED, e->m_pErrorInfo->m_strDescription);

		return TMDB_OPENRSFAILED;
	}
	catch(...)	//	All other errors
	{
		if(bMustExist)
			HandleError(TMDB_OPENRSFAILED, IDS_TMDB_OPENRSFAILED, lpTable);

		return TMDB_OPENRSFAILED;
	}

	if(pSet->IsOpen())
	{
		return HandleError(TMDB_NOERROR, (UINT)0);
	}
	else
	{
		if(bMustExist)
			HandleError(TMDB_OPENRSFAILED, IDS_TMDB_OPENRSFAILED, lpTable);

		return TMDB_OPENRSFAILED;
	}
}

//==============================================================================
//
// 	Function Name:	CDBAbstract::SetErrorHandler()
//
// 	Description:	This function enables and disables runtime error messages.
//					hParent is the parent window for error message boxes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBAbstract::SetErrorHandler(BOOL bEnable, HWND hParent)
{
	m_Errors.Enable(bEnable);
	m_Errors.SetParent(hParent);
}

