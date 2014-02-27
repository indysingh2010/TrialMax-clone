//==============================================================================
//
// File Name:	dbnet.cpp
//
// Description:	This file contains member functions of the CDBNET class.
//
// See Also:	dbnet.h
//
//==============================================================================
//	Date		Revision    Description
//	12-27-2003	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <dbnet.h>
#include <shellapi.h>
#include <tmini.h>
#include <video.h>
#include <barcode.h>

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
extern const WORD  _wVerMajor;
extern const WORD  _wVerMinor;

//------------------------------------------------------------------------------
//	REVISION INFORMATION
//------------------------------------------------------------------------------
//
//	_wDBNETMajor: Major version identifer is changed when significant changes
//				  have been made to the structure of the database. Modification
//				  of tables that TrialMax uses should force an update of this
//				  identifier.
//	_wDBNETMinor: Minor version identifier is changed when changes have been 
//				  made that alter the meaning or use of data contained in the
//				  database. This is usually increased when new features have
//				  been added that will not break the current release version 
//				  but will not be supported by the current release either.
//
//	NOTE:		  Now that we are back in control of both Presentation and
//				  Manager we can set the database version identifiers to match
//				  the application version identifiers
//
//------------------------------------------------------------------------------
const WORD	_wDBNETMajor = _wVerMajor;
const WORD	_wDBNETMinor = _wVerMinor;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------

//==============================================================================
//
// 	Function Name:	CDBNET::AddTranscripts()
//
// 	Description:	This function is called to get the transcript information
//					for each deposition in the specified list.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::AddTranscripts(CMedias* pDepositions)
{
	CMedia*			pDeposition = 0;
	CTranscript*	pTranscript;

	ASSERT(pDepositions != 0);
	ASSERT(m_pdbTranscripts != 0);

	//	Get all the transcript records
	if(m_pdbTranscripts->FilterOnId(-1, this) == FALSE)
		return;

	//	Iterate the transcripts table and add a transcript for each record
	//m_pdbTranscripts->MoveFirst();
	while(!m_pdbTranscripts->IsEOF())
	{
		//	Get the deposition associated with this transcript
		pDeposition = pDepositions->Find(m_pdbTranscripts->m_PrimaryId);
		if(pDeposition == 0) 
		{
			ASSERT(pDeposition);
			m_pdbTranscripts->MoveNext();
			continue;
		}

		//	Create a new media object
		pTranscript = new CTranscript();
		pTranscript->m_lTranscriptId = m_pdbTranscripts->m_AutoId;
		pTranscript->m_strDate = m_pdbTranscripts->m_DeposedOn;
		pTranscript->m_lAttributes = pDeposition->m_lAttributes;
		pTranscript->m_strAltBarcode = pDeposition->m_strAltBarcode;
		pTranscript->m_strFilename = m_pdbTranscripts->m_Filename;
		pTranscript->m_lAliasId = pDeposition->m_lAliasId;
		pTranscript->m_strRelativePath = pDeposition->m_strRelativePath;
		pTranscript->m_bLinked = (pTranscript->m_lAliasId > 0);
		pTranscript->m_lPrimaryMediaId = m_pdbTranscripts->m_PrimaryId;
		pTranscript->m_lFirstPL = m_pdbTranscripts->m_FirstPL;
		pTranscript->m_lLastPL = m_pdbTranscripts->m_LastPL;
		pTranscript->m_sLinesPerPage = m_pdbTranscripts->m_LinesPerPage;

		if(m_pdbTranscripts->m_Deponent.GetLength() > 0)
			pTranscript->m_strTranscriptName = m_pdbTranscripts->m_Deponent;
		else
			pTranscript->m_strTranscriptName = pDeposition->m_strName;
		
		//	These members not used by .NET
		pTranscript->m_strBaseFilename = "";
		pTranscript->m_strCtxExtension = "";
		pTranscript->m_strDbExtension  = "";

		//	Add this video to the list
		m_Transcripts.Add(pTranscript);

		m_pdbTranscripts->MoveNext();

	}

}

//==============================================================================
//
// 	Function Name:	CDBNET::AddTreatment()
//
// 	Description:	This function is called to add a treatment to the caller's
//					secondary collection
//
// 	Returns:		TRUE if successful
//
//	Notes:			This function is called in response to Manager notifying
//					the application that a treatment has been added.
//
//==============================================================================
BOOL CDBNET::AddTreatment(CSecondary* pSecondary, long lTertiaryId,
						  long lDisplayOrder, long lBarcodeId, LPCSTR lpszFilename)
{
	CTertiary* pTertiary;

	ASSERT(pSecondary != 0);
	
	//	NOTE:	We could query the database using the tertiary identifier but
	//			there's no guarentee that Access has updated all the fields before
	//			we get this notification. In fact, testing has shown that only some
	//			of the fields get populated if we run the query without introducing
	//			some time delay. Therefore we use the values provided with the notification
	//			to create the new object

	//	Create and initialize the treatment object
	pTertiary = new CTertiary(pSecondary);

	pTertiary->m_sMediaType		 = NET_MEDIA_TYPE_TREATMENT;
	pTertiary->m_strMediaId		 = pSecondary->m_strMediaId;
	pTertiary->m_lPrimaryId		 = pSecondary->m_lPrimaryId;
	pTertiary->m_lSecondaryId	 = pSecondary->m_lSecondaryId;
	pTertiary->m_lTertiaryId	 = lTertiaryId;
	pTertiary->m_lPlaybackOrder  = lDisplayOrder;
	pTertiary->m_lBarcodeId		 = lBarcodeId;
	pTertiary->m_strFilename	 = lpszFilename;
	pTertiary->m_strDescription  = "";
	pTertiary->m_lAttributes	 = 0;
	pTertiary->m_strName		 = "";
	pTertiary->m_strRelativePath = "";

	//	Add to the list
	pSecondary->m_Children.Add(pTertiary, TRUE);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CDBNET::AddVideos()
//
// 	Description:	This function is called to add all videos associated with 
//					each of the transcripts in the list.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::AddVideos(CTranscripts* pTranscripts)
{
	CTranscript*	pTranscript = 0;
	CVideo*			pVideo = 0;

	ASSERT(pTranscripts != 0);
	ASSERT(m_pdbSecondaries != 0);

	//	Filter the secondaries set
	if(!m_pdbSecondaries->FilterOnTranscripts(pTranscripts, this)) return;

	//	Add each video to the list
	while(!m_pdbSecondaries->IsEOF())
	{
		//	Do we need to get the deposition for this video?
		if((pTranscript == NULL) || (pTranscript->m_lPrimaryMediaId != m_pdbSecondaries->m_PrimaryMediaId))
		{
			pTranscript = pTranscripts->FindByPrimary(m_pdbSecondaries->m_PrimaryMediaId);
			ASSERT(pTranscript);
			if(pTranscript == NULL)
				continue;
		}

		//	Create a new video object
		pVideo = new CVideo();
		pVideo->m_lVideoFileId = m_pdbSecondaries->m_AutoId;
		pVideo->m_lTranscriptId = pTranscript->m_lTranscriptId;
		pVideo->m_lAliasId = m_pdbSecondaries->m_AliasId;
		pVideo->m_strRelativePath = m_pdbSecondaries->m_RelativePath;
		pVideo->m_strFilename = m_pdbSecondaries->m_Filename;
		
		//	Has this video been linked?
		if(pVideo->m_lAliasId > 0)
		{
			GetAliasedPath(pVideo->m_lAliasId, pVideo->m_strRelativePath, pVideo->m_strRootOverride);
		}
		else if(pVideo->m_strRelativePath.GetLength() > 0)
		{
			//	Probably shouldn't have to do this but just playing it safe
			pVideo->m_strRootOverride = m_aCaseFolders[CASE_FOLDER_VIDEOS];
		}
		else if(pTranscript->m_bLinked == TRUE)
		{
			//	Override the root with the source path if the transcript is linked
			GetAliasedPath(pTranscript->m_lAliasId, pTranscript->m_strRelativePath, 
						   pVideo->m_strRootOverride);

		}
		else if(pTranscript->m_strRelativePath.GetLength() > 0)
		{
			//	Override the root with the transcript's relative path
			pVideo->m_strRootOverride = m_aCaseFolders[CASE_FOLDER_VIDEOS];
			pVideo->m_strRootOverride += pTranscript->m_strRelativePath;
			if(pVideo->m_strRootOverride.Right(1) != "\\")
				pVideo->m_strRootOverride += "\\";
		}
		
		//	These members are new to .NET
		pVideo->m_lPrimaryMediaId = m_pdbSecondaries->m_PrimaryMediaId;
		pVideo->m_lBarcodeId = m_pdbSecondaries->m_BarcodeId;
		pVideo->m_lChildren = m_pdbSecondaries->m_Children;
		pVideo->m_lAttributes = m_pdbSecondaries->m_Attributes;

		//	These members not used by .NET
		pVideo->m_lBeginNum = 0;
		pVideo->m_lEndNum = 0;
		pVideo->m_lMaxSelStart = 0;
		pVideo->m_lMinSelStart = 0;
		pVideo->m_lUnitType = 0;

		//	Get the playback extents
		//GetExtents(pVideo);

		//	Add the video to the list
		m_Videos.Add(pVideo);

		//	Move to the next record			
		m_pdbSecondaries->MoveNext();
	}

}

//==============================================================================
//
// 	Function Name:	CDBNET::CDBNET()
//
// 	Description:	This is the constructor for CDBNET objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDBNET::CDBNET() : CDBAbstract()
{
	m_pDatabase = 0;
	m_pdbDetails = 0;
	m_pdbUsers = 0;
	m_pdbPrimaries = 0;
	m_pdbSecondaries = 0;
	m_pdbTertiaries = 0;
	m_pdbQuaternaries = 0;
	m_pdbHighlighters = 0;
	m_pdbExtents = 0;
	m_pdbTranscripts = 0;
	m_pdbBarcodeMap = 0;
	m_pXmlCaseOptions = 0;
	m_bUse600FileSystem = FALSE;
	m_bSplitScreenTreatments = FALSE;
	m_bSyncXmlDesignations = TRUE;
	m_strCaseOptionsFilename.Empty();
}

//==============================================================================
//
// 	Function Name:	CDBNET::~CDBNET()
//
// 	Description:	This is the destructor for CDBNET objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDBNET::~CDBNET()
{
	//	Make sure the case database is closed
	Close();
}

//==============================================================================
//
// 	Function Name:	CDBNET::Close()
//
// 	Description:	This function will delete the record sets and close the
//					database.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::Close()
{
	//	Close the record sets
	CLOSE_RECORDSET(m_pdbQuaternaries);
	CLOSE_RECORDSET(m_pdbTertiaries);
	CLOSE_RECORDSET(m_pdbSecondaries);
	CLOSE_RECORDSET(m_pdbPrimaries);
	CLOSE_RECORDSET(m_pdbTranscripts);
	CLOSE_RECORDSET(m_pdbHighlighters);
	CLOSE_RECORDSET(m_pdbExtents);
	CLOSE_RECORDSET(m_pdbUsers);
	CLOSE_RECORDSET(m_pdbDetails);
	CLOSE_RECORDSET(m_pdbBarcodeMap);

	//	Close the case options
	m_strCaseOptionsFilename.Empty();
	DELETE_INTERFACE(m_pXmlCaseOptions);

	//	Flush the local collections
	m_Depositions.Flush(TRUE);
	m_Aliases.Flush(TRUE);
	for(int i = 0; i < MAX_CASE_FOLDERS; i++)
		m_aCaseFolders[i].Empty();

	m_bUse600FileSystem = FALSE;
	m_bSplitScreenTreatments = FALSE;
	m_bSyncXmlDesignations = TRUE;

	//	Do the base class cleanup
	CDBAbstract::Close();
}

//==============================================================================
//
// 	Function Name:	CDBNET::ConvertToDesignation()
//
// 	Description:	This function convert the video segment to a playlist
//					designation
//
// 	Returns:		A pointer to the designation object if successful
//
//	Notes:			None
//
//==============================================================================
CDesignation* CDBNET::ConvertToDesignation(CDeposition* pDeposition, CSecondary* pSegment)
{
	CDesignation*	pDesignation = NULL;
	CVideo*			pVideo = NULL;
	CString			strId = "";

	//	Get the video associated with this segment
	if((pVideo = m_Videos.Find(pSegment->m_lSecondaryId)) != NULL)
	{
		//	Allocate and initialize a new designation
		pDesignation = new CDesignation();
		pDesignation->m_strMediaId = pSegment->m_strMediaId;
		pDesignation->m_lTertiaryId = pSegment->m_lSecondaryId;
		pDesignation->m_lPlaybackOrder = pSegment->m_lPlaybackOrder;

		pDesignation->m_lVideoId = pVideo->m_lVideoFileId;
		pDesignation->m_lTranscriptId = pVideo->m_lTranscriptId;

		//	Set the playback extents
		pDesignation->m_lStartPage = pSegment->m_lStartPage;
		pDesignation->m_lStartLine = pSegment->m_lStartLine;
		pDesignation->m_lStopPage = pSegment->m_lStopPage;
		pDesignation->m_lStopLine = pSegment->m_lStopLine;
		pDesignation->m_dStartTime = pSegment->m_dStartTime;
		pDesignation->m_dStopTime = pSegment->m_dStopTime;

		//	Make sure scrolling text is enabled
		pDesignation->m_bScrollText = TRUE;
		pDesignation->m_lAttributes = NET_TERTIARY_SCROLL_TEXT;
	}
	else
	{
		strId.Format("%ld", pSegment->m_lSecondaryId);
		HandleError(TMDB_NOSEGMENTVIDEO, IDS_TMDB_NOSEGMENTVIDEO, strId);

	}// if((pVideo = m_Videos.Find(pSegment->m_lSecondaryId)) == NULL)

	return pDesignation;
}

//==============================================================================
//
// 	Function Name:	CDBNET::ConvertToPlaylist()
//
// 	Description:	This function convert the multipage media object to a
//					playlist.
//
// 	Returns:		A pointer to the playlist object if successful
//
//	Notes:			None
//
//==============================================================================
CPlaylist* CDBNET::ConvertToPlaylist(CMultipage* pMultipage)
{
	CPlaylist*		pPlaylist;
	CDesignation*	pDesignation;
	CSecondary*		pSecondary;
	CVideos*		pVideos;
	CVideo*			pVideo;
	long			lVideoId = 1;
	CString			strFilename;

	ASSERT(pMultipage);

	//	Make sure we have a valid multipage animation
	if((pMultipage == 0) || (pMultipage->m_lPlayerType != MEDIA_TYPE_RECORDING))
	{
		HandleError(TMDB_INVALIDPARAM, IDS_TMDB_INVALIDPARAM, "ConvertToPlaylist()");
		return 0;
	}

	//	Allocate and initialize a new playlist object
	pPlaylist = new CPlaylist();
	pPlaylist->SetIsRecording(TRUE);
	pPlaylist->m_lPlayerType = MEDIA_TYPE_PLAYLIST;
	pPlaylist->m_strMediaId = pMultipage->m_strMediaId;
	pPlaylist->m_lPrimaryId = pMultipage->m_lPrimaryId;

	//	Attach a video list to the playlist
	pVideos = new CVideos();
	pVideos->SetRootFolder(0);
	pPlaylist->SetVideos(pVideos);
	pPlaylist->SetOwnsVideos(TRUE);

	//	Now convert each page of the multipage object to a designation
	pSecondary = pMultipage->m_Pages.First();
	while(pSecondary)
	{
		//	Create a video and add it to the list
		pVideo = new CVideo();
		pVideo->m_lVideoFileId = lVideoId++;
		pVideos->Add(pVideo);

		//	Get the video filename
		GetFilename(pMultipage, pSecondary, pVideo->m_strFilename);

		//	Allocate and initialize a new designation
		pDesignation = new CDesignation();
		pDesignation->m_lTertiaryId = pSecondary->m_lSecondaryId;
		pDesignation->m_lPlaybackOrder = pSecondary->m_lPlaybackOrder;
		pDesignation->m_strMediaId = pSecondary->m_strMediaId;
		pDesignation->m_lVideoId = pVideo->m_lVideoFileId;
		pDesignation->m_dStartTime = pSecondary->m_dStartTime;
		pDesignation->m_dStopTime = pSecondary->m_dStopTime;

		//	Add the designation to the playlist
		pPlaylist->m_Designations.Add(pDesignation, TRUE);

		//	Get the next page
		pSecondary = pMultipage->m_Pages.Next();
	}

	return pPlaylist;
}

//==============================================================================
//
// 	Function Name:	CDBNET::ConvertToPlaylist()
//
// 	Description:	This function convert the deposition media object to a
//					playlist.
//
// 	Returns:		A pointer to the playlist object if successful
//
//	Notes:			None
//
//==============================================================================
CPlaylist* CDBNET::ConvertToPlaylist(CDeposition* pDeposition)
{
	CPlaylist*		pPlaylist = NULL;
	CDesignation*	pDesignation = NULL;
	CSecondary*		pSegment = NULL;

	ASSERT(pDeposition != NULL);
	ASSERT(pDeposition->m_lPlayerType == MEDIA_TYPE_DEPOSITION);

	//	Make sure we have a valid deposition
	if((pDeposition == NULL) || (pDeposition->m_lPlayerType != MEDIA_TYPE_DEPOSITION))
	{
		HandleError(TMDB_INVALIDPARAM, IDS_TMDB_INVALIDPARAM, "ConvertToPlaylist()");
		return NULL;
	}

	//	Allocate and initialize a new playlist object
	pPlaylist = new CPlaylist();
	pPlaylist->m_lPlayerType = MEDIA_TYPE_PLAYLIST;
	pPlaylist->m_strMediaId = pDeposition->m_strMediaId;
	pPlaylist->m_lPrimaryId = pDeposition->m_lPrimaryId;
	pPlaylist->SetIsRecording(FALSE);
	pPlaylist->SetIsDeposition(TRUE);

	//	Set the video list
	pPlaylist->SetVideos(&m_Videos);
	pPlaylist->SetOwnsVideos(FALSE);

	//	Convert each of the secondary segments to a designation
	pSegment = pDeposition->m_Segments.First();
	while(pSegment != NULL)
	{
		if((pDesignation = ConvertToDesignation(pDeposition, pSegment)) != NULL)
		{
			//	Get the text for this designation
			if(pDeposition->m_pTranscript != NULL)
			{
				pDeposition->m_pTranscript->GetText(pSegment->m_lXmlSegmentId, pDesignation);

				if(pDesignation->HasText() == TRUE)
				{
					pPlaylist->SetHasText(TRUE);
				}

			}// if(pDeposition->m_pTranscript != NULL)

			//	Add the designation to the playlist
			pPlaylist->m_Designations.Add(pDesignation, FALSE);

			//	Get the next segment
			pSegment = pDeposition->m_Segments.Next();
		}
		else
		{
			delete pPlaylist;
			pPlaylist = NULL;
			break;
		}
	
	}// while(pSegment != NULL)

	return pPlaylist;
}

//==============================================================================
//
// 	Function Name:	CDBNET::CopyFile()
//
// 	Description:	This function will copy the source file to the target file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CDBNET::CopyFile(LPCSTR lpSource, LPCSTR lpTarget)
{
	SHFILEOPSTRUCT	OpStruct;
	char			szFrom[512];
	char			szTo[512];

	ASSERT(lpSource);
	ASSERT(lpTarget);

	//	The file specifications have to be double null terminated
	memset(szFrom, 0, sizeof(szFrom));
	memset(szTo, 0, sizeof(szTo));
	lstrcpyn(szFrom, lpSource, (sizeof(szFrom) - 1));
	lstrcpyn(szTo, lpTarget, (sizeof(szTo) - 1));

	//	Set up the shell operation structure
	memset(&OpStruct, 0, sizeof(OpStruct));
	OpStruct.hwnd   = 0;
	OpStruct.wFunc  = FO_COPY;
	OpStruct.pFrom  = szFrom;
	OpStruct.pTo	= szTo;
	OpStruct.fFlags = (FOF_NOCONFIRMATION | FOF_NOCONFIRMMKDIR);
					  
	//	Copy the file
	if(SHFileOperation(&OpStruct) == 0)
	{
		SetFileAttributes(lpTarget, FILE_ATTRIBUTE_NORMAL);
		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CDBNET::CopyToDocument()
//
// 	Description:	This function copies the files needed to view the pages
//					of the specified document.
//
// 	Returns:		None
//
//	Notes:			This function is used for debugging
//
//==============================================================================
void CDBNET::CopyToDocument(CMedia* pDocument)
{
	CMultipage* pMultipage = 0;
	CSecondary*	pSecondary = 0;
	int			iFile = 1;
	CString		strSourceFile;
	CString		strPageFile;
	CString		strTargetFolder;
	CString		strTargetFile;

	if((pMultipage = GetMultipage(pDocument)) != 0)
	{
		pSecondary = pMultipage->m_Pages.First();
		while(pSecondary != 0)
		{
			GetFilename(pMultipage, pSecondary, strTargetFile);
			if(FindFile(strTargetFile) == FALSE)
			{
				//	Get the path to the source file
				strSourceFile.Format("%s%0.4d.tif", m_strDocumentsSourceFolder, iFile);
				if(FindFile(strSourceFile) == FALSE)
				{
					if(iFile == 1)
					{
						break;
					}
					else
					{
						iFile = 1;
						strSourceFile.Format("%s%0.4d.tif", m_strDocumentsSourceFolder, iFile);
						if(FindFile(strSourceFile) == FALSE) break;
					}

				}

				CopyFile(strSourceFile, strTargetFile);

			}

			//	Next page...
			iFile++;
			pSecondary = pMultipage->m_Pages.Next();
		}

		delete pMultipage;
	}

	//	Create the folders
	//CreateDirectory(m_strRootPendingFolder, 0);
	//CreateDirectory(m_strTreatmentPendingFolder, 0);
}

//==============================================================================
//
// 	Function Name:	CDBNET::CreatePendingFolders()
//
// 	Description:	This function is called to create the folders used to store
//					pending updates.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::CreatePendingFolders()
{
	//	Assemble the path specifications
	m_strRootPendingFolder = m_strRootFolder;
	m_strRootPendingFolder += "_tmax_presentation\\";
	m_strTreatmentPendingFolder = m_strRootPendingFolder + "treatments\\";

	//	Create the folders
	CreateDirectory(m_strRootPendingFolder, 0);
	CreateDirectory(m_strTreatmentPendingFolder, 0);
}

//==============================================================================
//
// 	Function Name:	CDBNET::CreateXmlDocument()
//
// 	Description:	This function is called to create an XML document object
//
// 	Returns:		A pointer to the new XML document interface
//
//	Notes:			The caller is responsible for deallocation of the interface
//
//==============================================================================
CIXMLDOMDocument* CDBNET::CreateXmlDocument()
{
	CIXMLDOMDocument*	pDocument;
	COleException		OE;
	CLSID				ClassId;
	char				szError[256];

	//	Get the Class ID for the XML parser
	if(CLSIDFromProgID(L"Microsoft.XMLDOM", &ClassId) != S_OK)
	{
		return NULL;
	}

	//	Allocate a new XML document interface
	pDocument = new CIXMLDOMDocument();
	ASSERT(pDocument);

	//	Open the interface to the XML Parser
	if(!pDocument->CreateDispatch(ClassId, &OE))
	{
		OE.GetErrorMessage(szError, sizeof(szError));
		return NULL;
	}
	else
	{ 
		//	Force synchronous loading of the file 
		pDocument->SetAsync(FALSE);

		return pDocument;
	}
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetAliasedPath()
//
// 	Description:	This function is called to build a fully qualified path using
//					the specified alias id and linked path
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CDBNET::GetAliasedPath(long lAliasId, LPCSTR lpszRelativePath, CString& rPath)
{
	CAlias*	pAlias = 0;
	CString	strId;

	rPath.Empty();

	//	Find the requested alias
	if((pAlias = m_Aliases.Find(lAliasId)) == 0)
	{
		strId.Format("%ld", lAliasId);
		HandleError(TMDB_ALIASNOTFOUND, IDS_TMDB_ALIASNOTFOUND, strId);
		return FALSE;
	}

	rPath = pAlias->m_strCurrent;
	if(rPath.GetLength() > 0)
	{
		if(rPath.GetLength() == 1)
			rPath += ":\\";
		else if(rPath.Right(1) != "\\")
			rPath += "\\";
	}

	if((lpszRelativePath != 0) && (lstrlen(lpszRelativePath) > 0))
	{
		rPath += lpszRelativePath;
		if(rPath.Right(1) != "\\")
			rPath += "\\";
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetBarcode()
//
// 	Description:	This function is called to get a barcode for the record
//					with the specified unique identifier.
//
// 	Returns:		The associated barcode
//
//	Notes:			None
//
//==============================================================================
BOOL CDBNET::GetBarcode(LPCSTR lpszUniqueId, CString& rBarcode)
{
	CBarcode	Barcode;
	CString		strBarcode = "";
	CString		strMediaId = "";
	long		lSecondaryId = 0;

	ASSERT(m_pdbPrimaries != 0);
	ASSERT(m_pdbSecondaries != 0);
	ASSERT(m_pdbTertiaries != 0);
	ASSERT(m_pdbQuaternaries != 0);

	if(Barcode.SetBarcode(lpszUniqueId) == TRUE)
	{
		if(m_pdbPrimaries->FilterOnId(atol(Barcode.m_strMediaId), this) == TRUE)
		{
			strMediaId = m_pdbPrimaries->m_MediaId;
			
			//	Do we have a secondary id?
			if(Barcode.m_lSecondaryId > 0)
			{
				if(m_pdbSecondaries->FilterOnId(Barcode.m_lSecondaryId, this) == TRUE)
				{
					lSecondaryId = m_pdbSecondaries->m_BarcodeId;

					//	Do we have a tertiary id?
					if(Barcode.m_lTertiaryId > 0)
					{
						if(m_pdbTertiaries->FilterOnId(Barcode.m_lTertiaryId, this) == TRUE)
						{
							strBarcode.Format("%s.%ld.%ld", strMediaId, lSecondaryId,
											  m_pdbTertiaries->m_BarcodeId);
						}

					}
					else
					{
						strBarcode.Format("%s.%ld", strMediaId, lSecondaryId);
					}
				}

			}
			else
			{
				strBarcode = strMediaId;
			}
		
		}
			
	}

	rBarcode = strBarcode;

	return (strBarcode.GetLength() > 0);
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetClip()
//
// 	Description:	This function is called to get the clip and it's links using
//					the specified parent segment and owner scene
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTertiary* CDBNET::GetClip(CSecondary* pSegment, long lTertiaryBarcodeId, long lSceneId)
{
	CTertiary*	pClip = 0;
	CMedia*		pMedia = 0;

	ASSERT(pSegment != 0);
	ASSERT(lTertiaryBarcodeId > 0);
	ASSERT(m_pdbTertiaries != 0);

	if(m_pdbTertiaries == 0)
		return NULL;
	//	Filter the tertiary set
	if(m_pdbTertiaries->FilterOnSecondaryId(pSegment->m_lSecondaryId, lTertiaryBarcodeId, this) == FALSE) 
		return NULL;

	//	Line up on the requested record
	//m_pdbTertiaries->MoveFirst();

	//	Create and initialize the clip
	pClip = new CTertiary(pSegment);

	pClip->m_lPrimaryId = pSegment->m_lPrimaryId;
	pClip->m_lSecondaryId = pSegment->m_lSecondaryId;
	pClip->m_lTertiaryId = m_pdbTertiaries->m_AutoId;
	pClip->m_lPlaybackOrder = m_pdbTertiaries->m_DisplayOrder;
	pClip->m_strDescription = m_pdbTertiaries->m_Description;
	pClip->m_strFilename = m_pdbTertiaries->m_Filename;
	pClip->m_lBarcodeId = m_pdbTertiaries->m_BarcodeId;
	pClip->m_lAttributes = m_pdbTertiaries->m_Attributes;
	pClip->m_strName = m_pdbTertiaries->m_Name;
	pClip->m_sMediaType = m_pdbTertiaries->m_MediaType;
	pClip->m_strRelativePath = "";

	if((pMedia = GetMedia(pClip->m_lPrimaryId)) != 0)
		pClip->m_strMediaId = pMedia->m_strMediaId;

	//	Get the extents for the clip
	GetExtents(pClip);

	//	Get the links for the clip
	GetLinks(pClip, lSceneId);

	return pClip;
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetDeposition()
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
CDeposition* CDBNET::GetDeposition(CMedia* pMedia)
{
	CDeposition*	pDeposition = NULL;
	CTranscript*	pTranscript = NULL;

	ASSERT(pMedia);
	
	//	Make sure this deposition object is in the list
	if((pMedia == NULL) || (m_Depositions.Find(pMedia) == NULL))
	{
		HandleError(TMDB_INVALIDPARAM, IDS_TMDB_INVALIDPARAM, "GetDeposition()");
		return NULL;
	}

	//	Get the transcript record associated with this deposition
	if((pTranscript = m_Transcripts.FindByPrimary(pMedia->m_lPrimaryId)) == NULL)
	{
		HandleError(TMDB_NOTRANSCRIPTRECORD, IDS_TMDB_NOTRANSCRIPTRECORD, pMedia->m_strMediaId);
		return NULL;
	}

	//	Allocate the deposition object and set the class members
	pDeposition = new CDeposition(pMedia);
	pDeposition->m_pTranscript = pTranscript;

	//	Set the video list
	//pDeposition->SetVideos(&m_Videos);

	//	Get the video segments associated with this deposition
	if(GetSegments(pDeposition) == FALSE)
	{
		HandleError(TMDB_NOSEGMENTS, IDS_TMDB_NOTRANSCRIPTRECORD, pMedia->m_strMediaId);
		delete pDeposition;
		pDeposition = NULL;
	}

	return pDeposition;
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetDesignation()
//
// 	Description:	This function is called to get the designation with the
//					specified tertiary id
//
// 	Returns:		The requested designation object
//
//	Notes:			None
//
//==============================================================================
CDesignation* CDBNET::GetDesignation(long lTertiaryId, long lPlaybackOrder)
{
	CDesignation*	pDesignation;
	CVideo*			pVideo;
	CMedia*			pMedia;

	ASSERT(lTertiaryId > 0);
	ASSERT(m_pdbTertiaries != 0);

	//	Filter the tertiary record set 
	if(m_pdbTertiaries->FilterOnId(lTertiaryId, this) == FALSE) return 0;

	//	Line up on the first (and only) record
	//m_pdbTertiaries->MoveFirst();

	//	The parent deposition video should be in the local collection
	if((pVideo = m_Videos.Find(m_pdbTertiaries->m_SecondaryMediaId)) == 0) return 0;

	//	Get the parent media record
	if((pMedia = GetMedia(pVideo->m_lPrimaryMediaId)) == 0) return 0;

	//	Create a new designation object
	pDesignation = new CDesignation();
	pDesignation->m_strMediaId = pMedia->m_strMediaId;
	pDesignation->m_lPrimaryId = pMedia->m_lPrimaryId;
	pDesignation->m_lSecondaryId = m_pdbTertiaries->m_SecondaryMediaId;
	pDesignation->m_lTertiaryId = m_pdbTertiaries->m_AutoId;
	pDesignation->m_lVideoId = pVideo->m_lVideoFileId;
	pDesignation->m_lTranscriptId = pVideo->m_lTranscriptId;
	pDesignation->m_lPlaybackOrder = lPlaybackOrder;
	pDesignation->m_strDescription = m_pdbTertiaries->m_Description;
	pDesignation->m_lBarcodeId = m_pdbTertiaries->m_BarcodeId;
	pDesignation->m_lAttributes = m_pdbTertiaries->m_Attributes;
	pDesignation->m_strName = m_pdbTertiaries->m_Name;
	pDesignation->m_strFilename = m_pdbTertiaries->m_Filename;
	pDesignation->m_sMediaType = m_pdbTertiaries->m_MediaType;

	//	Get the playback extents
	GetExtents(pDesignation);

	//	Overlays not supported by .NET
	pDesignation->m_strOverlayFilename = "";
	pDesignation->m_strOverlayRelativePath = "";
	GetOverlayFilename(pDesignation);
		
	return pDesignation;
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetDesignations()
//
// 	Description:	This function is called to get the designations associated 
//					with the playlist object provided by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::GetDesignations(CPlaylist* pPlaylist)
{
	CDesignation*	pDesignation;
	CBarcode		Barcode;

	ASSERT(pPlaylist != 0);
	ASSERT(m_pdbSecondaries != 0);
	ASSERT(m_pdbTertiaries != 0);
	
	//	Flush the existing designation list
	pPlaylist->m_Designations.Flush(TRUE);

	//	Filter the secondary set to retrieve all scenes for this playlist
	if(m_pdbSecondaries->FilterOnPrimaryId(pPlaylist->m_lPrimaryId, this) == FALSE) return;

	//	Add each designation to the list
	//m_pdbSecondaries->MoveFirst();
	while(!m_pdbSecondaries->IsEOF())
	{
		//	This should be a designation
		ASSERT(m_pdbSecondaries->m_SourceMediaType == NET_MEDIA_TYPE_DESIGNATION);
		if((m_pdbSecondaries->m_SourceMediaType == NET_MEDIA_TYPE_DESIGNATION) &&
		   ((m_pdbSecondaries->m_Attributes & NET_SECONDARY_HIDDEN) == 0))
		{
			//	Create a new designation object
			if((pDesignation = GetDesignation(m_pdbSecondaries->m_SourcePST, m_pdbSecondaries->m_AutoId, m_pdbSecondaries->m_DisplayOrder)) != 0)
			{
				//	Override the default barcode id and display order when the
				//	designation is being retrieved via a script scene
				pDesignation->m_lBarcodeId     = m_pdbSecondaries->m_BarcodeId;
				pDesignation->m_lPlaybackOrder = m_pdbSecondaries->m_DisplayOrder;

				//	Add the designation to the list
				pPlaylist->m_Designations.Add(pDesignation, FALSE);

				if(pDesignation->HasText() == TRUE)
				{
					pPlaylist->SetHasText(TRUE);
				}
			
			}

		}// if(m_pdbSecondaries->m_SceneType == NET_MEDIA_TYPE_DESIGNATION)

		//	Move to the next record			
		m_pdbSecondaries->MoveNext();
	}

}

//==============================================================================
//
// 	Function Name:	CDBNET::GetDocumentsSourceFolder()
//
// 	Description:	This function is called to prompt the user for the folder
//					containing the source files used to create the document
//					media for debugging purposes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::GetDocumentsSourceFolder()
{
	BROWSEINFO		BrowseInfo;
	IMalloc*		pMalloc;
	LPITEMIDLIST	pItemIDList;
	TCHAR			szFolder[MAX_PATH];
	CString			strFilename;
	CString			strMsg;
	
	m_strDocumentsSourceFolder.Empty();

	//	Initialize the browse information
	memset(&BrowseInfo, 0, sizeof(BrowseInfo));
	BrowseInfo.hwndOwner = NULL;
	BrowseInfo.lpszTitle = "Select the source folder";
	BrowseInfo.ulFlags = BIF_RETURNONLYFSDIRS;
	BrowseInfo.pszDisplayName = szFolder;

	//	Open the browser dialog
	if((pItemIDList = SHBrowseForFolder(&BrowseInfo)) == NULL)
		return;

	//	Translate the folder's display name to a path specification
	if(!SHGetPathFromIDList(pItemIDList, szFolder))
		return;

	// Convert to lower case
	m_strDocumentsSourceFolder = szFolder;
	if(m_strDocumentsSourceFolder.Right(1) != "\\")
		m_strDocumentsSourceFolder += "\\";
	m_strDocumentsSourceFolder.MakeLower();


	//	We must at least have 0001.tif in the folder
	strFilename = m_strDocumentsSourceFolder + "0001.tif";
	if(FindFile(strFilename) == FALSE)
	{
		strMsg.Format("Unable to create documents: 0001.tif not found in %s", m_strDocumentsSourceFolder);
		MessageBox(0, strMsg, "Error", MB_OK);
		m_strDocumentsSourceFolder.Empty();
	}

	//	Delete the PIDL using the shells task allocator
	if(SHGetMalloc(&pMalloc) != NOERROR)
		return;

	//	Free the memory returned by the shell
	if(pMalloc)
	{
		pMalloc->Free(pItemIDList);
		pMalloc->Release();
	}
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetExtents()
//
// 	Description:	This function is called to retrieve the extents for the
//					specified secondary video.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::GetExtents(CVideo* pVideo)
{
	ASSERT(pVideo != 0);
	ASSERT(m_pdbExtents != 0);

	pVideo->m_dStartPosition = 0.0;
	pVideo->m_dStopPosition = 1000000000.0;
	pVideo->m_bBeginTuned = FALSE;
	pVideo->m_bEndTuned = FALSE;

/*
	//	Filter the extents set
	if(!m_pdbExtents->FilterOnSecondaryId(pVideo->m_lVideoFileId)) return;

	//	Line up on the first (and hopefully only) record
	//m_pdbExtents->MoveFirst();

	//	Set the extents
	pVideo->m_dStartPosition = m_pdbExtents->m_Start;
	pVideo->m_dStopPosition = m_pdbExtents->m_Stop;
	pVideo->m_bBeginTuned = m_pdbExtents->m_StartTuned;
	pVideo->m_bEndTuned = m_pdbExtents->m_StopTuned;

*/
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetExtents()
//
// 	Description:	This function is called to retrieve the extents for the
//					specified secondary object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::GetExtents(CSecondary* pSecondary)
{
	ASSERT(pSecondary != 0);
	ASSERT(m_pdbExtents != 0);

	//	Filter the extents set
	if(!m_pdbExtents->FilterOnSecondaryId(pSecondary->m_lSecondaryId, this)) return;

	//	Line up on the first (and hopefully only) record
	//m_pdbExtents->MoveFirst();

	//	Set the extents
	pSecondary->m_dStartTime	= m_pdbExtents->m_Start;
	pSecondary->m_dStopTime		= m_pdbExtents->m_Stop;
	pSecondary->m_lStartPage	= PLToPage(m_pdbExtents->m_StartPL);
	pSecondary->m_lStartLine	= PLToLine(m_pdbExtents->m_StartPL);
	pSecondary->m_lStopPage		= PLToPage(m_pdbExtents->m_StopPL);
	pSecondary->m_lStopLine		= PLToLine(m_pdbExtents->m_StopPL);
	pSecondary->m_lXmlSegmentId	= m_pdbExtents->m_XmlSegmentId;
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetExtents()
//
// 	Description:	This function is called to retrieve the extents for the
//					specified tertiary object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::GetExtents(CTertiary* pTertiary)
{
	BOOL bSuccessful = FALSE;

	ASSERT(pTertiary != 0);
	ASSERT(m_pdbExtents != 0);

	//	Filter on both secondary and tertiary
	//
	//	Theoretically the tertiary id is unique but just in case (as has happended)
	//	something has gone wrong and there are duplicate tertiary identifiers in the
	//	Extents table we use both secondary and tertiary identifiers
	if(pTertiary->m_lSecondaryId > 0)
		bSuccessful = m_pdbExtents->FilterOnBoth(pTertiary->m_lSecondaryId, pTertiary->m_lTertiaryId, this);
	else
		bSuccessful = m_pdbExtents->FilterOnTertiaryId(pTertiary->m_lTertiaryId, this);

	if(bSuccessful == TRUE)
	{
		//	Line up on the first (and hopefully only) record
		//m_pdbExtents->MoveFirst();

		//	Set the extents
		pTertiary->m_dStartTime		= m_pdbExtents->m_Start;
		pTertiary->m_dStopTime		= m_pdbExtents->m_Stop;
		pTertiary->m_lStartPage		= PLToPage(m_pdbExtents->m_StartPL);
		pTertiary->m_lStartLine		= PLToLine(m_pdbExtents->m_StartPL);
		pTertiary->m_lStopPage		= PLToPage(m_pdbExtents->m_StopPL);
		pTertiary->m_lStopLine		= PLToLine(m_pdbExtents->m_StopPL);
		pTertiary->m_lHighlighterId	= m_pdbExtents->m_HighlighterId;

		//	If the start and stop times are equal we assume this is an unsynchronized 
		//	designation and we bump the stop time so that DirectX doesn't play the whole file
		if(pTertiary->m_dStopTime <= pTertiary->m_dStartTime)
		{
			pTertiary->m_dStopTime = pTertiary->m_dStartTime + 0.01;
		}

		if(pTertiary->m_lHighlighterId > 0)
		{
			pTertiary->m_crHighlighter = GetHighlighterColor(pTertiary->m_lHighlighterId);

			//	Were we unable to find the highlighter?
			if((pTertiary->m_crHighlighter & 0xFF000000L) != 0)
				pTertiary->m_lHighlighterId = 0;
		}

	}// if(bSuccessful == TRUE)

}

//==============================================================================
//
// 	Function Name:	CDBNET::GetFilename()
//
// 	Description:	This function is called to get the path to the XML file
//					containing the transcript text
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::GetFilename(CTranscript* pTranscript, CString& rFilename)
{
	CString strPrimaryId;

	ASSERT(pTranscript);
	ASSERT(pTranscript->m_lPrimaryMediaId > 0);
	ASSERT(pTranscript->m_strFilename.GetLength() > 0);

	//	Get the root folder for clips
	rFilename = m_aCaseFolders[CASE_FOLDER_TRANSCRIPTS];
	if((rFilename.GetLength() > 0) && (rFilename.Right(1) != "\\"))
		rFilename += "\\";

	//	Add the primary identifier
	strPrimaryId.Format("%ld", pTranscript->m_lPrimaryMediaId);
	rFilename += strPrimaryId;
	rFilename += "\\";

	//	Add the filename
	rFilename += pTranscript->m_strFilename;
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetFilename()
//
// 	Description:	This function is called to get the fully qualified path of
//					the file associated with the page specified by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::GetFilename(CMultipage* pMultipage, CSecondary* pSecondary, CString& rFilename)
{
	ASSERT(pMultipage);
	ASSERT(pSecondary);

	//	Get the folder for this multipage object
	GetMultipageFolder(pMultipage, rFilename);
	
	//	Now add the filename
	if(pMultipage->m_sMediaType == NET_MEDIA_TYPE_POWERPOINT)
		rFilename += pMultipage->m_strFilename;
	else
		rFilename += pSecondary->m_strFilename;		
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetFilename()
//
// 	Description:	This function is called to get the fully qualified path of
//					the file associated with the treatment specified by the 
//					caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::GetFilename(CTertiary* pTertiary, CString& rFilename)
{
	CString str601Filename;

	//	Are we using the 6.0.1 file system?
	if(m_bUse600FileSystem == TRUE)
	{
		//	Get the old path
		if(GetFilename600(pTertiary, rFilename) == FALSE)
		{
			//	Check to see if it was moved to the new path
			if(GetFilename601(pTertiary, str601Filename) == TRUE)
				rFilename = str601Filename;
		}

	}
	else
	{
		GetFilename601(pTertiary, rFilename);
	}

}

//==============================================================================
//
// 	Function Name:	CDBNET::GetFilename600()
//
// 	Description:	This function is called to get the path to the tertiary file
//					using the storage structure employed prior to version 6.0.1
//
// 	Returns:		TRUE if the old file exists
//
//	Notes:			None
//
//==============================================================================
BOOL CDBNET::GetFilename600(CTertiary* pTertiary, CString& rFilename)
{
	CString strFileSpec;
	CString strDeposition;

	ASSERT(pTertiary);

	switch(pTertiary->m_sMediaType)
	{
		case NET_MEDIA_TYPE_DESIGNATION:
		case NET_MEDIA_TYPE_CLIP:

			//	Get the root folder for clips
			strFileSpec = m_aCaseFolders[CASE_FOLDER_CLIPS];

			if((strFileSpec.GetLength() > 0) && (strFileSpec.Right(1) != "\\"))
				strFileSpec += "\\";

			//	Now add the filename
			strFileSpec += pTertiary->m_strFilename;

			break;

		case NET_MEDIA_TYPE_TREATMENT:

			//	Get the root folder 
			strFileSpec = m_aCaseFolders[CASE_FOLDER_TREATMENTS];

			if((strFileSpec.GetLength() > 0) && (strFileSpec.Right(1) != "\\"))
				strFileSpec += "\\";

			//	Now add the filename
			strFileSpec += pTertiary->m_strFilename;

			break;

		default:

			return FALSE;

	}

	if(strFileSpec.GetLength() > 0)
	{
		rFilename = strFileSpec;
		rFilename.MakeLower();

		return FindFile(rFilename);
	}
	else
	{
		return FALSE;
	}

}

//==============================================================================
//
// 	Function Name:	CDBNET::GetFilename601()
//
// 	Description:	This function is called to get the path to the tertiary file
//					using the storage structure employed starting in version 6.0.1
//
// 	Returns:		TRUE if the file exists
//
//	Notes:			None
//
//==============================================================================
BOOL CDBNET::GetFilename601(CTertiary* pTertiary, CString& rFilename)
{
	CString strFileSpec;
	CString strSubFolder;
	char	szFilename[MAX_PATH];

	ASSERT(pTertiary);

	//	Get the root folder for this media type
	GetMediaFolder(pTertiary->m_sMediaType, strFileSpec);

	if((strFileSpec.GetLength() > 0) && (strFileSpec.Right(1) != "\\"))
		strFileSpec += "\\";

	//	Now add the rest of the path
	if(pTertiary->m_sMediaType == NET_MEDIA_TYPE_DESIGNATION)
	{
		//	Add subfolder using primary deposition id
		strSubFolder.Format("%ld\\", pTertiary->m_lPrimaryId);
		strFileSpec += strSubFolder;

		strFileSpec += pTertiary->m_strFilename;
	}
	else
	{
		//	Add primary subfolder using the database id
		strSubFolder.Format("%ld\\", pTertiary->m_lPrimaryId);
		strFileSpec += strSubFolder;

		//	Add the secondary subfolder using the secondary filename (without extension)
		ASSERT(pTertiary->m_pSecondary != 0);
		lstrcpyn(szFilename, pTertiary->m_pSecondary->m_strFilename, sizeof(szFilename));
		
		if(lstrlen(szFilename) > 0)
		{
			strSubFolder.Format("%s\\", StripFileExtension(szFilename));
			strFileSpec += strSubFolder;
		}

		strFileSpec += pTertiary->m_strFilename;
	}

	if(strFileSpec.GetLength() > 0)
	{
		rFilename = strFileSpec;
		rFilename.MakeLower();

		return FindFile(rFilename);
	}
	else
	{
		return FALSE;
	}

}

//==============================================================================
//
// 	Function Name:	CDBNET::GetHighlighterColor()
//
// 	Description:	This function is called to get the RGB color associated 
//					with the specified highlighter
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
COLORREF CDBNET::GetHighlighterColor(long lHighlighter)
{
	BYTE	 byRed;
	BYTE	 byGreen;
	BYTE	 byBlue;

	ASSERT(lHighlighter > 0);
	ASSERT(m_pdbHighlighters != 0);

	if(m_pdbHighlighters == 0)
		return FALSE;

	//	Filter the highlighters set
	if(m_pdbHighlighters->FilterOnId(lHighlighter, this) == FALSE)
	{
		return 0xFFFFFFFF; // Invalid color
	}

	//	Line up on the requested record
//	m_pdbHighlighters->MoveFirst();

	byRed   = ((m_pdbHighlighters->m_Color >> 16) & 0x0000FF);
	byGreen = ((m_pdbHighlighters->m_Color >> 8) & 0x0000FF);
	byBlue  = (m_pdbHighlighters->m_Color & 0x0000FF);

	return RGB(byRed, byGreen, byBlue);
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetLinks()
//
// 	Description:	This function is called to get the links associated with
//					with the designation object provided by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::GetLinks(CTertiary* pTertiary, long lSceneId)
{
	CLink*	pLink = 0;

	ASSERT(lSceneId > 0);
	ASSERT(m_pdbQuaternaries != 0);
	
	//	Filter the quaternary record set 
	if(m_pdbQuaternaries->FilterOnTertiaryId(pTertiary->m_lTertiaryId, this) == FALSE) return;

	//	Flush the existing list
	pTertiary->m_Links.Flush(TRUE);

	//	Add each link to the list
	//m_pdbQuaternaries->MoveFirst();
	while(!m_pdbQuaternaries->IsEOF())
	{
		//	Create a new object
		pLink = new CLink();

		pLink->m_strMediaId = pTertiary->m_strMediaId;
		pLink->m_lOwnerId = lSceneId;
		pLink->m_lId = m_pdbQuaternaries->m_AutoId;
		pLink->m_strPST = m_pdbQuaternaries->m_SourceId;
		pLink->m_lDisplayType = DISPLAY_TYPE_BASICLINK;
		pLink->m_lFlags = m_pdbQuaternaries->m_Attributes;

		pLink->m_bHide = ((m_pdbQuaternaries->m_Attributes & TMFLAG_LINK_HIDE) != 0);
		pLink->m_bSplitScreen = ((m_pdbQuaternaries->m_Attributes & TMFLAG_LINK_SPLITSCREEN) != 0);
	
		//	Set the extents
		pLink->m_dTrigger = m_pdbQuaternaries->m_Start;
		pLink->m_lPage	  = PLToPage(m_pdbQuaternaries->m_StartPL);
		pLink->m_lLine	  = PLToLine(m_pdbQuaternaries->m_StartPL);

		//	Defer looking up the ItemBarcode until the user loads the item
		//
		//	NOTE:	We have to do this because resolving the unique id to a barcode
		//			will result in refiltering the secondary record set
		pLink->m_strItemBarcode = pLink->m_strPST;

		//	Add to the list
		pTertiary->m_Links.Add(pLink);

		//	Move to the next record			
		m_pdbQuaternaries->MoveNext();
	}

}

//==============================================================================
//
// 	Function Name:	CDBNET::GetMedia()
//
// 	Description:	This function is called to get the media object with the id
//					specified by the caller.
//
// 	Returns:		A pointer to the media object if found.
//
//	Notes:			None
//
//==============================================================================
CMedia* CDBNET::GetMedia(LPCSTR lpMediaId)
{
	CMedia* pMedia = 0;

	//	Perform base class processing first
	if((pMedia = CDBAbstract::GetMedia(lpMediaId)) != 0)
		return pMedia;

	//	It could be a deposition
	return m_Depositions.Find(lpMediaId);
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetMedia()
//
// 	Description:	This function is called to get the media object with the id
//					specified by the caller.
//
// 	Returns:		A pointer to the media object if found.
//
//	Notes:			None
//
//==============================================================================
CMedia* CDBNET::GetMedia(long lId)
{
	CMedia* pMedia = 0;

	//	Perform base class processing first
	if((pMedia = CDBAbstract::GetMedia(lId)) != 0)
		return pMedia;

	//	It could be a deposition
	return m_Depositions.Find(lId);
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetMultipageFolder()
//
// 	Description:	This function is called to get the default folder associated
//					with the specified multipage object
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::GetMultipageFolder(CMultipage* pMultipage, CString& rFolder)
{
	//	Is this a linked media?
	if(pMultipage->m_bLinked == TRUE)
	{
		GetAliasedPath(pMultipage->m_lAliasId, pMultipage->m_strRelativePath, rFolder);
	}
	else
	{
		//	Get the root primary folder
		GetPrimaryFolder(pMultipage, rFolder);
	}

}

//==============================================================================
//
// 	Function Name:	CDBNET::GetOverlayFilename()
//
// 	Description:	This function is called to get the fully qualified path of
//					the overlay file associated with the designation specified 
//					by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::GetOverlayFilename(CDesignation* pDesignation)
{
	ASSERT(pDesignation);

	//	.NET does not yet support overlay files
	pDesignation->m_strOverlay.Empty();
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetPrimaryFolder()
//
// 	Description:	This function is called to get the folder where the specified
//					primary types are stored
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::GetPrimaryFolder(CMultipage* pMultipage, CString& rFolder)
{
	CString strFolder;

	//	Get the folder for the associated media type
	GetMediaFolder(pMultipage->m_sMediaType, strFolder);

	if((strFolder.GetLength() > 0) && (strFolder.Right(1) != "\\"))
		strFolder += "\\";

	//	Do we need to add a relative path?
	if(pMultipage->m_strRelativePath.GetLength() > 0)
	{
		if(pMultipage->m_strRelativePath.Left(1) == "\\")
			strFolder += pMultipage->m_strRelativePath.Right(pMultipage->m_strRelativePath.GetLength() - 1);
		else
			strFolder += pMultipage->m_strRelativePath;

		if(strFolder.Right(1) != "\\")
			strFolder += "\\";
	}

	rFolder = strFolder;
	rFolder.MakeLower();

}

//==============================================================================
//
// 	Function Name:	CDBNET::GetPlaylistFromScene()
//
// 	Description:	This function is called to create a single designation
//					playlist to represent the specified scene
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPlaylist* CDBNET::GetPlaylistFromScene(long lSceneId)
{
	CMedia*			pPrimary;
	CPlaylist*		pPlaylist;
	CDesignation*	pDesignation;

	ASSERT(m_pdbSecondaries != 0);
	
	//	Filter the secondary set to line up on the specified scene
	if(m_pdbSecondaries->FilterOnId(lSceneId, this) == FALSE) return 0;

	//	Line up on the first record
	//m_pdbSecondaries->MoveFirst();

	//	This should be a designation
	ASSERT(m_pdbSecondaries->m_SourceMediaType == NET_MEDIA_TYPE_DESIGNATION);
	if(m_pdbSecondaries->m_SourceMediaType != NET_MEDIA_TYPE_DESIGNATION)
		return 0;

	//	Get the media object that represents the parent script
	if((pPrimary = GetMedia(m_pdbSecondaries->m_PrimaryMediaId)) == 0)
		return 0;

	//	Create a new designation object
	if((pDesignation = GetDesignation(m_pdbSecondaries->m_SourcePST, lSceneId, m_pdbSecondaries->m_DisplayOrder)) == 0)
		return 0;

	//	Override the default barcode id and display order when the
	//	designation is being retrieved via a script scene
	pDesignation->m_lBarcodeId     = m_pdbSecondaries->m_BarcodeId;
	pDesignation->m_lPlaybackOrder = m_pdbSecondaries->m_DisplayOrder;

	//	Create the playlist object
	pPlaylist = new CPlaylist(pPrimary);
	ASSERT(pPlaylist != 0);

	//	Set the video list
	pPlaylist->SetVideos(&m_Videos);

	//	Add the designation to the list
	pPlaylist->m_Designations.Add(pDesignation, FALSE);

	//	If the playlist has at least one designation that has text
	//	AND has the scroll text option turned on, then it will display
	//	the scrolling text box when played
	if(pDesignation->HasText() == TRUE)
	{
		pPlaylist->SetHasText(TRUE);
	}

	return pPlaylist;
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetDesignation()
//
// 	Description:	This function is called to get the designation associated
//					with the specified scene
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDesignation* CDBNET::GetDesignation(LPCSTR lpszPST, long lSceneId, long lPlaybackOrder)
{
	CDesignation*	pDesignation = 0;
	CBarcode		Barcode;

	ASSERT(lpszPST != 0);
	
	//	Use a barcode object to split the PST identifer
	Barcode.SetBarcode(lpszPST);

	//	Do we have a valid tertiary identifier
	ASSERT(Barcode.m_lTertiaryId > 0);
	if(Barcode.m_lTertiaryId > 0)
	{
		//	Create a new designation object
		if((pDesignation = GetDesignation(Barcode.m_lTertiaryId, lPlaybackOrder)) != 0)
		{
			//	Get the list of links for this designation
			GetLinks(pDesignation, lSceneId);

			//	Get the transcript text for this designation
			GetText(pDesignation);

		}
			
	}// if(Barcode.m_lTertiaryId > 0)

	return pDesignation;

}

//==============================================================================
//
// 	Function Name:	CDBNET::GetMediaFolder()
//
// 	Description:	This function is called to get the folder where the specified
//					primary types are stored
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::GetMediaFolder(short sNetMediaType, CString& rFolder)
{
	CString strF;
	//	What type of media?
	switch(sNetMediaType)
	{
		case NET_MEDIA_TYPE_DOCUMENT:
		
			rFolder = m_aCaseFolders[CASE_FOLDER_DOCUMENTS];
			break;
							
		case NET_MEDIA_TYPE_POWERPOINT:			
		
			rFolder = m_aCaseFolders[CASE_FOLDER_POWERPOINTS];
			break;
							
		case NET_MEDIA_TYPE_RECORDING:			
		
			rFolder = m_aCaseFolders[CASE_FOLDER_RECORDINGS];
			break;
							
		case NET_MEDIA_TYPE_DEPOSITION:			
		
			rFolder = m_aCaseFolders[CASE_FOLDER_VIDEOS];
			break;
							
		case NET_MEDIA_TYPE_TREATMENT:			
		
			rFolder = m_aCaseFolders[CASE_FOLDER_TREATMENTS];
			break;
							
		case NET_MEDIA_TYPE_CLIP:			
		
			rFolder = m_aCaseFolders[CASE_FOLDER_CLIPS];
			break;
							
		case NET_MEDIA_TYPE_DESIGNATION:			
		
			rFolder = m_aCaseFolders[CASE_FOLDER_TRANSCRIPTS];
			break;
							
		default:
		
			rFolder.Empty();
			break;
	}
	
	//	Append the trailing backslash if necessary
	if(!rFolder.IsEmpty())
	{
		if(rFolder.Right(1) != "\\")
			rFolder += "\\";
	}
	
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetSaveZapFileSpec()
//
// 	Description:	This function is called to get the fully qualified path of
//					the file used to save a new zap.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::GetSaveZapFileSpec(CMultipage* pMultipage, CSecondary* pSecondary, CString& rFileSpec)
{
	CString strFolder;

	ASSERT(pMultipage);
	ASSERT(pSecondary);

	strFolder = GetSaveZapFolder();
	if(strFolder.Right(1) != "\\")
		strFolder += "\\";

	//	Now get the file specification
	for(int i = 1; i < 10000; i++)
	{
		rFileSpec.Format("%s%ld.%ld_%d.zap_", strFolder,
						  pMultipage->m_lPrimaryId,
						  pSecondary->m_lSecondaryId, i);

		//	Does this file already exist?
		if(!FindFile(rFileSpec))
			break;
	}

}

//==============================================================================
//
// 	Function Name:	CDBNET::GetSaveZapFileSpecs()
//
// 	Description:	This function is called to get the fully qualified paths 
//					to the zap files used to create a split screen treatment
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::GetSaveZapFileSpecs(CMultipage* pTLMultipage, CSecondary* pTLSecondary, CString& rTLFileSpec,
								 CMultipage* pBRMultipage, CSecondary* pBRSecondary, CString& rBRFileSpec,
								 DWORD dwFlags)
{
	CString strFolder = "";
	CString	strBRFilename = "";

	ASSERT(pTLMultipage);
	ASSERT(pTLSecondary);
	ASSERT(pBRMultipage);
	ASSERT(pBRSecondary);

	strFolder = GetSaveZapFolder();
	if(strFolder.Right(1) != "\\")
		strFolder += "\\";

	//	First get the file specification for the bottom/right pane
	for(int i = 1; i < 10000; i++)
	{
		//	Format the name of the bottom/right file (no extension)
		strBRFilename.Format("%ld.%ld_%d",pBRMultipage->m_lPrimaryId, pBRSecondary->m_lSecondaryId, i);

		//	Now construct the fully qualified path
		rBRFileSpec.Format("%s%s.ssz_", strFolder, strBRFilename);

		//	Stop here if the file does not exist
		if(!FindFile(rBRFileSpec))
			break;
	}

	//	Construct the formatted name of the zap file for the left pane
	rTLFileSpec.Format("%s%ld.%ld_%d-%s.zap_", 
					   strFolder,
					   pTLMultipage->m_lPrimaryId,
					   pTLSecondary->m_lSecondaryId, 
					   dwFlags,
					   strBRFilename);
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetSecondaries()
//
// 	Description:	This function is called to get the pages associated with
//					the multipage object provided by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::GetSecondaries(CMultipage* pMultipage)
{
	CSecondary* pSecondary = 0;

	ASSERT(pMultipage != 0);
	ASSERT(m_pdbSecondaries != 0);

	//	Flush the existing page list
	pMultipage->m_Pages.Flush(TRUE);

	//	Filter the secondaries set
	if(!m_pdbSecondaries->FilterOnPrimaryId(pMultipage->m_lPrimaryId, this)) return;

	//	Add each page to the list
	//m_pdbSecondaries->MoveFirst();
	while(!m_pdbSecondaries->IsEOF())
	{
		//	Create a new secondary object
		pSecondary = new CSecondary();
		pSecondary->m_strMediaId = pMultipage->m_strMediaId;
		
		m_pdbSecondaries->Read(pSecondary);

		//	Set the presentation display type
		switch(pSecondary->m_sMediaType)
		{
			case NET_MEDIA_TYPE_PAGE:

				if((pSecondary->m_lAttributes & NET_SECONDARY_HIGHRES) != 0)
					pSecondary->m_lDisplayType = DISPLAY_TYPE_HIRESPAGE;
				else
					pSecondary->m_lDisplayType = DISPLAY_TYPE_SCREENRESPAGE;
				break;

			case NET_MEDIA_TYPE_SEGMENT:

				pSecondary->m_lDisplayType = DISPLAY_TYPE_BASICANIMATION;

				//	Get the playback extents
				GetExtents(pSecondary);

				break;
		
			case NET_MEDIA_TYPE_SLIDE:

				pSecondary->m_lDisplayType = DISPLAY_TYPE_BASICPOWERPOINT;
				break;
		
			default:

				delete pSecondary;
				pSecondary = 0;
				ASSERT(pSecondary != 0); // Force an assertion
				break;
		
		}

		//	Add the page to the list
		if(pSecondary != 0)
			pMultipage->m_Pages.Add(pSecondary, FALSE);

		//	Move to the next record			
		m_pdbSecondaries->MoveNext();
	}

}

//==============================================================================
//
// 	Function Name:	CDBNET::GetSegments()
//
// 	Description:	This function is called to populate the Segments collection
//					owned by the specified deposition
//
// 	Returns:		TRUE if at least one segment is located
//
//	Notes:			None
//
//==============================================================================
BOOL CDBNET::GetSegments(CDeposition* pDeposition)
{
	CSecondary* pSecondary = 0;

	ASSERT(pDeposition != NULL);
	ASSERT(m_pdbSecondaries != NULL);

	//	Flush the existing segments collection
	pDeposition->m_Segments.Flush(TRUE);

	//	Filter the secondaries set
	if(m_pdbSecondaries->FilterOnPrimaryId(pDeposition->m_lPrimaryId, this))
	{
		//	Process each of the segment records
		while(!m_pdbSecondaries->IsEOF())
		{
			//	Create a new secondary object
			pSecondary = new CSecondary();
			pSecondary->m_strMediaId = pDeposition->m_strMediaId;
			pSecondary->m_lDisplayType = DISPLAY_TYPE_BASICANIMATION;
			
			m_pdbSecondaries->Read(pSecondary);

			//	Get the playback extents
			GetExtents(pSecondary);
			
			//	Add the owner's collection
			pDeposition->m_Segments.Add(pSecondary, FALSE);

			//	Move to the next record			
			m_pdbSecondaries->MoveNext();
		
		}// while(!m_pdbSecondaries->IsEOF())

	}// if(m_pdbSecondaries->FilterOnPrimaryId(pDeposition->m_lPrimaryId, this))

	return (pDeposition->m_Segments.GetCount() > 0);
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetShowItems()
//
// 	Description:	This function is called to get the show items associated 
//					with the custom show object provided by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::GetShowItems(CShow* pShow, long lSecondary)
{
	CShowItem* pShowItem = 0;

	ASSERT(pShow);
	ASSERT(m_pdbSecondaries != 0);

	//	Flush the existing page list
	pShow->m_Items.Flush(TRUE);

	//	Filter the secondaries set
	if(!m_pdbSecondaries->FilterOnPrimaryId(pShow->m_lPrimaryId, this)) return;

	//	Add each scene to the list
	//m_pdbSecondaries->MoveFirst();
	while(!m_pdbSecondaries->IsEOF())
	{
		//	Add this item if it is not hidden
		if((m_pdbSecondaries->m_DisplayOrder == lSecondary) ||
		   ((m_pdbSecondaries->m_Attributes & NET_SECONDARY_HIDDEN) == 0))
		{
			//	Create a new object
			pShowItem = new CShowItem();

			//	Get the values stored in the record
			m_pdbSecondaries->Read(pShowItem);

			if((pShowItem->m_lAttributes & NET_SECONDARY_AUTO_TRANSITION) != 0)
			{
				pShowItem->m_bAutoTransition = TRUE;
				
				switch(pShowItem->m_sSourceMediaType)
				{
					case NET_MEDIA_TYPE_SEGMENT:
					case NET_MEDIA_TYPE_CLIP:
					case NET_MEDIA_TYPE_DESIGNATION:

						if(m_pdbSecondaries->m_TransitionTime < 0)
							pShowItem->m_lTransitionPeriod = 0;
						else
							pShowItem->m_lTransitionPeriod = (m_pdbSecondaries->m_TransitionTime * 1000);

						pShowItem->m_bStaticScene = FALSE;
						break;

					case NET_MEDIA_TYPE_PAGE:
					case NET_MEDIA_TYPE_SLIDE:
					case NET_MEDIA_TYPE_TREATMENT:
					case NET_MEDIA_TYPE_DOCUMENT:
					case NET_MEDIA_TYPE_POWERPOINT:
					case NET_MEDIA_TYPE_RECORDING:

						if(m_pdbSecondaries->m_TransitionTime <= 0)
							pShowItem->m_lTransitionPeriod = 1000;
						else
							pShowItem->m_lTransitionPeriod = (m_pdbSecondaries->m_TransitionTime * 1000);

						pShowItem->m_bStaticScene = TRUE;
						break;

					case NET_MEDIA_TYPE_UNKNOWN:
					case NET_MEDIA_TYPE_SCRIPT:
					case NET_MEDIA_TYPE_SCENE:
					case NET_MEDIA_TYPE_DEPOSITION:
					case NET_MEDIA_TYPE_LINK:
					default:

						ASSERT(0);

						if(m_pdbSecondaries->m_TransitionTime <= 0)
							pShowItem->m_lTransitionPeriod = 1000;
						else
							pShowItem->m_lTransitionPeriod = (m_pdbSecondaries->m_TransitionTime * 1000);

						pShowItem->m_bStaticScene = TRUE;
						break;
				}

			}
			else
			{
				pShowItem->m_bAutoTransition = FALSE;
				pShowItem->m_lTransitionPeriod = -1L;
			}

			//	Defer setting the ItemBarcode until the user loads the item
			//
			//	NOTE:	We have to do this because resolving the unique id to a barcode
			//			will result in refiltering the secondary record set
			//pShowItem->m_strItemBarcode = GetBarcode(pShowItem->m_strSceneId);

			pShow->m_Items.Add(pShowItem, FALSE);

		}

		//	Move to the next record			
		m_pdbSecondaries->MoveNext();
	}

}

//==============================================================================
//
// 	Function Name:	CDBNET::GetText()
//
// 	Description:	This function is called to get the links associated with
//					with the designation object provided by the caller.
//
// 	Returns:		TRUE if text was found in the database
//
//	Notes:			None
//
//==============================================================================
BOOL CDBNET::GetText(CDesignation* pDesignation)
{
	CString				strFilename;
	VARIANT				vFilename;
	CIXMLDOMDocument*	pXmlDocument = 0;
	CIXMLDOMNodeList*	pXmlText = 0;
	CIXMLDOMNode*		pXmlDesignation = 0;
	BOOL				bSuccessful = FALSE;

	ASSERT(pDesignation != 0);

	//	Get the path to the XML that contains the text
	GetFilename((CTertiary*)pDesignation, strFilename);
	if(strFilename.GetLength() == 0) return FALSE;

	//	Make sure the file exists
	if(FindFile(strFilename) == FALSE)
	{
		return FALSE;

	}// if(FindFile(strFilename) == FALSE)

	while(bSuccessful == FALSE)
	{
		//	Attach to the XML engine
		if((pXmlDocument = CreateXmlDocument()) == 0)
			break;

		VariantInit(&vFilename);
		V_VT(&vFilename) = VT_BSTR;
		V_BSTR(&vFilename) = strFilename.AllocSysString();

		//	Parse the XML file
		if(pXmlDocument->load(vFilename) == FALSE)
			break;

		pXmlDesignation = new CIXMLDOMNode(pXmlDocument->selectSingleNode("trialMax/designation"));
		if(pXmlDesignation->m_lpDispatch == 0)
			break;

		//	Get the text flags
		if(GetTextFlags(pDesignation, pXmlDesignation) == FALSE)
			break;

		//	Get the collection of transcript lines
		pXmlText = new CIXMLDOMNodeList(pXmlDocument->selectNodes("trialMax/designation/transcript"));
		if(pXmlText->m_lpDispatch == 0)
			break;

		if(pDesignation->GetText(pXmlText) == FALSE)
			break;

		//	We're done
		bSuccessful = TRUE;
	
	}// while(bSuccessful == FALSE)

	//	Clean up
	if(pXmlDocument != 0)
	{
		VariantClear(&vFilename);
		DELETE_INTERFACE(pXmlDocument);		
		DELETE_INTERFACE(pXmlDesignation);
		DELETE_INTERFACE(pXmlText);
	}

	return bSuccessful;
	
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetTertiaries()
//
// 	Description:	This function is called to get the tertiary children of the
//					specified secondary object
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CDBNET::GetTertiaries(CSecondary* pSecondary)
{
	CTertiary* pTertiary;

	ASSERT(pSecondary != 0);
	ASSERT(m_pdbTertiaries != 0);

	if(m_pdbTertiaries == 0)
		return FALSE;

	//	Flush the child collection
	pSecondary->m_Children.Flush(TRUE);

	//	Filter the tertiary set
	if(m_pdbTertiaries->FilterOnSecondaryId(pSecondary->m_lSecondaryId, -1, this) == FALSE) 
		return TRUE;

	//	Add each tertiary record to the list
	//m_pdbTertiaries->MoveFirst();
	while(!m_pdbTertiaries->IsEOF())
	{
		//	Create a new page object
		pTertiary = new CTertiary(pSecondary);

		pTertiary->m_strMediaId = pSecondary->m_strMediaId;
		pTertiary->m_lPrimaryId = pSecondary->m_lPrimaryId;
		pTertiary->m_lSecondaryId = pSecondary->m_lSecondaryId;
		pTertiary->m_lTertiaryId = m_pdbTertiaries->m_AutoId;
		pTertiary->m_lPlaybackOrder = m_pdbTertiaries->m_DisplayOrder;
		pTertiary->m_strDescription = m_pdbTertiaries->m_Description;
		pTertiary->m_strFilename = m_pdbTertiaries->m_Filename;
		pTertiary->m_lBarcodeId = m_pdbTertiaries->m_BarcodeId;
		pTertiary->m_lAttributes = m_pdbTertiaries->m_Attributes;
		pTertiary->m_strName = m_pdbTertiaries->m_Name;
		pTertiary->m_sMediaType = m_pdbTertiaries->m_MediaType;
		pTertiary->m_strSiblingId = m_pdbTertiaries->m_SiblingId;		
		
		//	Not used by .NET version
		pTertiary->m_strRelativePath = "";

		//	Add to the list
		pSecondary->m_Children.Add(pTertiary, FALSE);

		//	Move to the next record			
		m_pdbTertiaries->MoveNext();
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetTextFlags()
//
// 	Description:	This function is called to get the designation's text flags
//					using the specified XML node
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CDBNET::GetTextFlags(CDesignation* pDesignation, CIXMLDOMNode* pXmlNode)
{
	CIXMLDOMNamedNodeMap*	pAttributes = 0;
	CIXMLDOMNode*			pXmlScroll = 0;

	ASSERT(pXmlNode != 0);

	//	Get the node's attributes
	pAttributes = new CIXMLDOMNamedNodeMap(pXmlNode->GetAttributes());
	if((pAttributes == 0) || (pAttributes->m_lpDispatch == 0))
		return FALSE;

	//	Don't bother with the scroll text option if not synchronized
	if(m_bSyncXmlDesignations == TRUE)
	{
		//	Is the scrollText flag specified?
		pXmlScroll = new CIXMLDOMNode(pAttributes->getNamedItem("scrollText"));
		if((pXmlScroll != 0) || (pXmlScroll->m_lpDispatch != 0))
		{
			if(lstrcmpi(pXmlScroll->GetText(), "yes") == 0)
				pDesignation->m_bScrollText = TRUE;
			else
				pDesignation->m_bScrollText = FALSE;
		}

	}// if(m_bSyncXmlDesignations == TRUE)

	//	Clean up
	DELETE_INTERFACE(pXmlScroll);
	DELETE_INTERFACE(pAttributes);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetXmlAlias()
//
// 	Description:	This function is called to create a new drive alias from the
//					specified XML node.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CDBNET::GetXmlAlias(CIXMLDOMNode* pXmlAlias)
{
	CIXMLDOMNamedNodeMap*	pAttributes = 0;
	CIXMLDOMNode*			pXmlId = 0;
	CIXMLDOMNode*			pXmlCurrent = 0;
	CIXMLDOMNode*			pXmlPrevious = 0;
	CIXMLDOMNode*			pXmlOriginal = 0;
	CAlias*					pAlias = 0;
	BOOL					bSuccessful = FALSE;

	ASSERT(pXmlAlias != 0);

	//	Get the node's attributes
	pAttributes = new CIXMLDOMNamedNodeMap(pXmlAlias->GetAttributes());
	if((pAttributes == 0) || (pAttributes->m_lpDispatch == 0))
		return FALSE;

	//	Retrieve each of the required attributes
	while(bSuccessful == FALSE)
	{
		pXmlId = new CIXMLDOMNode(pAttributes->getNamedItem("id"));
		if((pXmlId == 0) || (pXmlId->m_lpDispatch == 0))
			break;

		pXmlCurrent = new CIXMLDOMNode(pAttributes->getNamedItem("current"));
		if((pXmlCurrent == 0) || (pXmlCurrent->m_lpDispatch == 0))
			break;

		pXmlPrevious = new CIXMLDOMNode(pAttributes->getNamedItem("previous"));
		if((pXmlPrevious == 0) || (pXmlPrevious->m_lpDispatch == 0))
			break;

		pXmlOriginal = new CIXMLDOMNode(pAttributes->getNamedItem("original"));
		if((pXmlOriginal == 0) || (pXmlOriginal->m_lpDispatch == 0))
			break;

		//	We're done
		bSuccessful = TRUE;

	}

	//	We're we able to get all the attributes?
	if(bSuccessful == TRUE)
	{
		pAlias = new CAlias();
		ASSERT(pAlias != 0);

		pAlias->m_lId			= atol(pXmlId->GetText());
		pAlias->m_strCurrent	= pXmlCurrent->GetText();
		pAlias->m_strPrevious	= pXmlPrevious->GetText();
		pAlias->m_strOriginal	= pXmlOriginal->GetText();

		//	Add to the local collection
		m_Aliases.Add(pAlias, TRUE);
	}

	//	Clean up
	DELETE_INTERFACE(pXmlId);
	DELETE_INTERFACE(pXmlCurrent);
	DELETE_INTERFACE(pXmlPrevious);
	DELETE_INTERFACE(pXmlOriginal);
	DELETE_INTERFACE(pAttributes);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetXmlAliases()
//
// 	Description:	This function is called to get the case drive/server
//					aliases.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CDBNET::GetXmlAliases()
{
	CString				strXPath;
	CString				strOldXPath;
	CIXMLDOMNode*		pXmlNode = 0;

	ASSERT(m_pXmlCaseOptions != 0);
	if(m_pXmlCaseOptions == 0) return FALSE;

	//	Verify that the aliases section exists
	strXPath = "trialMax/Sections/sect:Section[@Name=\"trialMax/case/aliases\"]";
	pXmlNode = new CIXMLDOMNode(m_pXmlCaseOptions->selectSingleNode(strXPath));
	if(pXmlNode->m_lpDispatch == 0)
	{
		//	Try again using the root from versions earlier than 6.1.0
		strOldXPath = "Sections/sect:Section[@Name=\"trialMax/case/aliases\"]";
		pXmlNode = new CIXMLDOMNode(m_pXmlCaseOptions->selectSingleNode(strOldXPath));
	}

	if(pXmlNode->m_lpDispatch == 0)
	{
		HandleError(TMDB_NOALIASNODE, IDS_TMDB_NOALIASNODE, m_strCaseOptionsFilename, strXPath);
		DELETE_INTERFACE(pXmlNode);
		return FALSE;
	}
	else
	{
		DELETE_INTERFACE(pXmlNode);
	}

	//	Retrieve all the alias nodes
	for(int i = 1; i < 10000; i++)
	{
	
		strXPath.Format("trialMax/Sections/sect:Section[@Name=\"trialMax/case/aliases\"]/Alias%d", i);
		pXmlNode = new CIXMLDOMNode(m_pXmlCaseOptions->selectSingleNode(strXPath));
		if(pXmlNode->m_lpDispatch == 0)
		{
			strOldXPath.Format("Sections/sect:Section[@Name=\"trialMax/case/aliases\"]/Alias%d", i);
			pXmlNode = new CIXMLDOMNode(m_pXmlCaseOptions->selectSingleNode(strOldXPath));
		}

		if(pXmlNode->m_lpDispatch == 0)
		{
			//	Must have run out of aliases
			break;
		}

		//	Add this alias to the list
		GetXmlAlias(pXmlNode);
		DELETE_INTERFACE(pXmlNode);
	
	}// for(int i = 1; i < 10000; i++)

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetXmlCasePath()
//
// 	Description:	This function is called to get the path assigned to the
//					specified folder in the XML case options file
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CDBNET::GetXmlCasePath(int iIndex, LPCSTR lspzXPathMap, LPCSTR lpszName, BOOL bShowError)
{
	CString					strXPath;
	CIXMLDOMNode*			pXmlNode = 0;
	CIXMLDOMNamedNodeMap*	pAttributes = 0;
	CIXMLDOMNode*			pXmlValue = 0;
	BOOL					bSuccessful = FALSE;

	ASSERT(m_pXmlCaseOptions != 0);
	if(m_pXmlCaseOptions == 0) return FALSE;

	while(bSuccessful == FALSE)
	{
		//	Get the element containing the path
		strXPath.Format("%s/%s", lspzXPathMap, lpszName);
		pXmlNode = new CIXMLDOMNode(m_pXmlCaseOptions->selectSingleNode(strXPath));
		if(pXmlNode->m_lpDispatch == 0)
		{
			if(bShowError == TRUE)
				HandleError(TMDB_NOCASEPATHNODE, IDS_TMDB_NOCASEPATHNODE, m_strCaseOptionsFilename, strXPath);
			break;
		}

		//	Get the node's attributes
		pAttributes = new CIXMLDOMNamedNodeMap(pXmlNode->GetAttributes());
		if((pAttributes == 0) || (pAttributes->m_lpDispatch == 0))
		{
			if(bShowError == TRUE)
				HandleError(TMDB_BADCASEPATHNODE, IDS_TMDB_BADCASEPATHNODE, lpszName, m_strCaseOptionsFilename);
			break;
		}

		//	Get the value assigned for this folder
		pXmlValue = new CIXMLDOMNode(pAttributes->getNamedItem("Value"));
		if((pXmlValue == 0) || (pXmlValue->m_lpDispatch == 0))
			break;

		//	Set the value
		if(lstrlen(pXmlValue->GetText()) > 0)
		{
			m_aCaseFolders[iIndex] = pXmlValue->GetText();
			if(m_aCaseFolders[iIndex].Right(1) != "\\")
				m_aCaseFolders[iIndex] += "\\";
			m_aCaseFolders[iIndex].MakeLower();
		}

		//	We're done
		bSuccessful = TRUE;
	
	}// while(bSuccessful == FALSE)

	//	Clean up
	DELETE_INTERFACE(pXmlValue);	
	DELETE_INTERFACE(pAttributes);
	DELETE_INTERFACE(pXmlNode);

	return bSuccessful;
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetXmlCasePaths()
//
// 	Description:	This function is called to get the case path information
//					from the XML case options file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CDBNET::GetXmlCasePaths()
{
	CString					strXPath;
	CString					strOldXPath;
	CIXMLDOMNode*			pXmlNode = 0;
	BOOL					bSuccessful = FALSE;

	//	Initialize the paths
	SetPathDefaults();

	ASSERT(m_pXmlCaseOptions != 0);
	if(m_pXmlCaseOptions == 0) return FALSE;

	while(bSuccessful == FALSE)
	{
		//	Get the node containing the path map assigned to the local machine
		strXPath.Format("trialMax/Sections/sect:Section[@Name=\"trialMax/pathMap/%d\"]", m_Machine.m_iPathMap);
		pXmlNode = new CIXMLDOMNode(m_pXmlCaseOptions->selectSingleNode(strXPath));
		if(pXmlNode->m_lpDispatch == 0)
		{
			//	This may be created prior to version 6.1.0
			strOldXPath.Format("Sections/sect:Section[@Name=\"trialMax/pathMap/%d\"]", m_Machine.m_iPathMap);
			pXmlNode = new CIXMLDOMNode(m_pXmlCaseOptions->selectSingleNode(strOldXPath));
			if(pXmlNode->m_lpDispatch != 0) strXPath = strOldXPath;
		}

		if(pXmlNode->m_lpDispatch == 0)
		{
			HandleError(TMDB_NOPATHMAPNODE, IDS_TMDB_NOPATHMAPNODE, m_strCaseOptionsFilename, strXPath);
			break;
		}

		//	Set each of the case paths defined in this map
		if(GetXmlCasePath(CASE_FOLDER_DOCUMENTS, strXPath, "Documents", TRUE) == FALSE)
			break;

		if(GetXmlCasePath(CASE_FOLDER_POWERPOINTS, strXPath, "PowerPoints", TRUE) == FALSE)
			break;

		if(GetXmlCasePath(CASE_FOLDER_RECORDINGS, strXPath, "Recordings", TRUE) == FALSE)
			break;

		if(GetXmlCasePath(CASE_FOLDER_VIDEOS, strXPath, "Videos", TRUE) == FALSE)
			break;

		//	These paths are not critical since the user has no way to change them anyway
		GetXmlCasePath(CASE_FOLDER_TREATMENTS, strXPath, "Treatments", FALSE);
		GetXmlCasePath(CASE_FOLDER_TRANSCRIPTS, strXPath, "Transcripts", FALSE);
		if(GetXmlCasePath(CASE_FOLDER_CLIPS, strXPath, "Clips", FALSE) == FALSE)
		{
			//	See if it's stored using the old name
			GetXmlCasePath(CASE_FOLDER_CLIPS, strXPath, "Designations", FALSE);
		}

		//	We're done
		bSuccessful = TRUE;
	
	}// while(bSuccessful == FALSE)

	//	Clean up
	DELETE_INTERFACE(pXmlNode);	

	return bSuccessful;
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetXmlGlobal()
//
// 	Description:	This function is called to get the global information
//					stored in the case options file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CDBNET::GetXmlGlobal()
{
	CString					strXPath;
	CIXMLDOMNode*			pXmlNode = 0;
	CIXMLDOMNamedNodeMap*	pAttributes = 0;
	CIXMLDOMNode*			pXmlValue = 0;
	BOOL					bSuccessful = FALSE;

	ASSERT(m_pXmlCaseOptions != 0);
	if(m_pXmlCaseOptions == 0) return FALSE;

	//	Reset the global members
	m_bSyncXmlDesignations = TRUE;
	
	while(bSuccessful == FALSE)
	{
		//	Get the node containing the Sync Xml Designations option
		strXPath = "trialMax/Sections/sect:Section[@Name=\"global\"]/syncXmld";
		pXmlNode = new CIXMLDOMNode(m_pXmlCaseOptions->selectSingleNode(strXPath));
		if(pXmlNode->m_lpDispatch != 0)
		{
			//	Get the node's Value attribute
			pAttributes = new CIXMLDOMNamedNodeMap(pXmlNode->GetAttributes());
			if((pAttributes != 0) && (pAttributes->m_lpDispatch != 0))
				pXmlValue = new CIXMLDOMNode(pAttributes->getNamedItem("Value"));

			//	Were we able to get the value node?
			if((pXmlValue != 0) && (pXmlValue->m_lpDispatch != 0))
			{
				m_bSyncXmlDesignations = (lstrcmpi(pXmlValue->GetText(), "true") == 0);
			}

		}// if(pXmlNode->m_lpDispatch != 0)

		//	We're done
		bSuccessful = TRUE;
	
	}// while(bSuccessful == FALSE)

	//	Clean up
	DELETE_INTERFACE(pXmlValue);	
	DELETE_INTERFACE(pAttributes);
	DELETE_INTERFACE(pXmlNode);

	return bSuccessful;
}

//==============================================================================
//
// 	Function Name:	CDBNET::GetXmlMachine()
//
// 	Description:	This function is called to get the machine information
//					stored in the case options file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CDBNET::GetXmlMachine()
{
	CString					strXPath;
	CString					strOldXPath;
	CIXMLDOMNode*			pXmlNode = 0;
	CIXMLDOMNamedNodeMap*	pAttributes = 0;
	CIXMLDOMNode*			pXmlValue = 0;
	char					szMachineName[MAX_COMPUTERNAME_LENGTH + 1];
	DWORD					dwMachineName = sizeof(szMachineName);
	BOOL					bSuccessful = FALSE;

	ASSERT(m_pXmlCaseOptions != 0);
	if(m_pXmlCaseOptions == 0) return FALSE;

	//	Reset the machine information
	m_Machine.m_strName.Empty();
	m_Machine.m_iPathMap = 0;
	
	//	Get the name of this machine
	if(::GetComputerName(szMachineName, &dwMachineName))
	{
		m_Machine.m_strName = szMachineName;
	}
	else
	{
		HandleError(TMDB_NOMACHINENAME, IDS_TMDB_NOMACHINENAME);
		return FALSE;
	}

	while(bSuccessful == FALSE)
	{
		//	Get the node containing the machine's path map identifier
		strXPath.Format("trialMax/Sections/sect:Section[@Name=\"trialMax/machine/%s\"]/PathMap", m_Machine.m_strName);
		pXmlNode = new CIXMLDOMNode(m_pXmlCaseOptions->selectSingleNode(strXPath));
		if(pXmlNode->m_lpDispatch == 0)
		{
			//	Try again using the old INI root from versions earlier than 6.1.0
			strOldXPath.Format("Sections/sect:Section[@Name=\"trialMax/machine/%s\"]/PathMap", m_Machine.m_strName);
			pXmlNode = new CIXMLDOMNode(m_pXmlCaseOptions->selectSingleNode(strOldXPath));
		}

		if(pXmlNode->m_lpDispatch != 0)
		{
			//	Get the node's Value attribute
			pAttributes = new CIXMLDOMNamedNodeMap(pXmlNode->GetAttributes());
			if((pAttributes != 0) && (pAttributes->m_lpDispatch != 0))
				pXmlValue = new CIXMLDOMNode(pAttributes->getNamedItem("Value"));

			//	Were we able to get the value node?
			if((pXmlValue != 0) && (pXmlValue->m_lpDispatch != 0))
			{
				//	Store the id of the path map used by this machine
				m_Machine.m_iPathMap = atoi(pXmlValue->GetText());
			}

		}

		//	Use the default path map if unable to retrieve a valid map for this machine
		if(m_Machine.m_iPathMap <= 0)
			m_Machine.m_iPathMap = 1;

		//	We're done
		bSuccessful = TRUE;
	
	}// while(bSuccessful == FALSE)

	//	Clean up
	DELETE_INTERFACE(pXmlValue);	
	DELETE_INTERFACE(pAttributes);
	DELETE_INTERFACE(pXmlNode);

	return bSuccessful;
}

//==============================================================================
//
// 	Function Name:	CDBNET::LoadMedia()
//
// 	Description:	This function is called to load the media lists using the
//					records in the media table.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
int CDBNET::LoadMedia()
{
	CMedia*	pMedia;

	ASSERT(m_pdbPrimaries);
	if(m_pdbPrimaries == 0)
		return HandleError(TMDB_INVALIDRECORDSET, IDS_TMDB_INVALIDRECORDSET);

	//	Get all the primary records
	if(m_pdbPrimaries->FilterOnId(-1, this) == FALSE)
		return m_iLastError;

//double dStart = (double)GetTickCount();
//long lRecords = 0;

	//	Move to the first record
	//m_pdbPrimaries->MoveFirst();

	//	Add a media object for each primary record
	while(!m_pdbPrimaries->IsEOF())
	{
		//	Don't bother if no children
		if(m_pdbPrimaries->m_Children > 0)
		{
		//lRecords++;
			//	Create a new media object
			pMedia = new CMedia();
			pMedia->m_strMediaId = m_pdbPrimaries->m_MediaId;
			pMedia->m_lPrimaryId = m_pdbPrimaries->m_AutoId;
			pMedia->m_strFilename = m_pdbPrimaries->m_Filename;
			pMedia->m_strName = m_pdbPrimaries->m_Name;
			
			//	These members are .NET specific
			pMedia->m_lAttributes = m_pdbPrimaries->m_Attributes;
			pMedia->m_lChildren = m_pdbPrimaries->m_Children;
			pMedia->m_sMediaType = m_pdbPrimaries->m_MediaType;
			pMedia->m_strAltBarcode = m_pdbPrimaries->m_AltBarcode;
			pMedia->m_strDescription = m_pdbPrimaries->m_Description;
			pMedia->m_strExhibit = m_pdbPrimaries->m_Exhibit;
			pMedia->m_lAliasId = m_pdbPrimaries->m_AliasId;
			pMedia->m_strRelativePath = m_pdbPrimaries->m_RelativePath;
			pMedia->m_bLinked = (pMedia->m_lAliasId > 0);
			
			//	Translate the .NET media type to a Presentation media type
			switch(pMedia->m_sMediaType)
			{
				case NET_MEDIA_TYPE_DOCUMENT:

					pMedia->m_lPlayerType = MEDIA_TYPE_IMAGE;
					m_Multipages.Add(pMedia);
					break;

				case NET_MEDIA_TYPE_POWERPOINT:

					pMedia->m_lPlayerType = MEDIA_TYPE_POWERPOINT;
					m_Multipages.Add(pMedia);
					break;

				case NET_MEDIA_TYPE_RECORDING:

					pMedia->m_lPlayerType = MEDIA_TYPE_RECORDING;
					m_Multipages.Add(pMedia);
					break;

				case NET_MEDIA_TYPE_SCRIPT:

					//	Is this a pure deposition playlist?
					if((pMedia->m_lAttributes & NET_PRIMARY_PLAYLIST) != 0)
					{
						pMedia->m_lPlayerType = MEDIA_TYPE_PLAYLIST;
						m_Playlists.Add(pMedia);
					}
					else
					{
						pMedia->m_lPlayerType = MEDIA_TYPE_CUSTOMSHOW;
						m_Multipages.Add(pMedia);
					}
					break;

				case NET_MEDIA_TYPE_DEPOSITION:

					pMedia->m_lPlayerType = MEDIA_TYPE_DEPOSITION;
					m_Depositions.Add(pMedia);

					break;

				default:

					ASSERT(0);
					break;

			}

		}// if(m_pdbPrimaries->m_Children > 0)

		//	Move to the next record			
		m_pdbPrimaries->MoveNext();
	}

	m_pdbPrimaries->Close();

//double dEnd = (double)GetTickCount();
//CString M;
//M.Format("Time to load %ld records: %.3f", lRecords, ((dEnd - dStart) / 1000.0));
//MessageBox(0, M, "", MB_OK);

	//	Do we have any depositions?
	if(m_Depositions.GetCount() > 0)
	{
		//	Get the transcripts associated with each primary deposition
		AddTranscripts(&m_Depositions);

		//	Add the videos for each transcript
		if(m_Transcripts.GetCount() > 0)
			AddVideos(&m_Transcripts);
	}

//	DEFER SETTING EXTENTS UNTIL USER ATTEMPTS TO ITERATE TO THE ENDPOINTS
//	m_Multipages.SetExtents();
//	m_Playlists.SetExtents();

	//	Are we supposed to create the documents?
	if((m_bCreateDocuments == TRUE) && (m_strDocumentsSourceFolder.GetLength() > 0))
	{
		pMedia = m_Multipages.First();
		while(pMedia != 0)
		{
			if(pMedia->m_sMediaType == NET_MEDIA_TYPE_DOCUMENT)
				CopyToDocument(pMedia);
			pMedia = m_Multipages.Next();
		}

	}

	return HandleError(TMDB_NOERROR, (UINT)0);
}

//==============================================================================
//
// 	Function Name:	CDBNET::LoadVersion()
//
// 	Description:	This function is called to load the version information.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
int CDBNET::LoadVersion()
{
	CString	strMsg;
	BOOL	bOldApplication = FALSE;
	long	lVersion = 0;

	ASSERT(m_pdbDetails);
	if((m_pdbDetails == 0) || !m_pdbDetails->IsOpen())
		return HandleError(TMDB_INVALIDRECORDSET, IDS_TMDB_INVALIDRECORDSET);

	ASSERT(m_pdbUsers);
	if(m_pdbUsers == 0)
		return HandleError(TMDB_INVALIDRECORDSET, IDS_TMDB_INVALIDRECORDSET);

	//	Are there any records?
	if(m_pdbDetails->IsBOF() || m_pdbDetails->IsEOF())
		return HandleError(TMDB_NOVERSIONINFO, IDS_TMDB_NOVERSIONINFO);

	//	Make sure we are on the first record (There should only be one)
	//m_pdbDetails->MoveFirst();

	//	Get the revision identifiers
	m_iMajorVer = m_pdbDetails->m_DbMajor;
	m_iMinorVer = m_pdbDetails->m_DbMinor;

	//	Save the revision information
	m_strRevision.Format("%d.%d", m_iMajorVer, m_iMinorVer);

	//	Attempt to locate the name of the creator
	if(m_pdbUsers->FilterOnId(m_pdbDetails->m_CreatedBy, this) == TRUE)
	{
		//m_pdbUsers->MoveFirst();
		m_strCreator = m_pdbUsers->m_Name;
	}
	else
	{
		m_strCreator = "Unknown";
	}

	//	Clear the error level
	//
	//	NOTE:	From this point on we will go ahead an allow the user to try
	//			to use the database but we will always report version differences
	//			even if the error handler is disabled.
	HandleError(TMDB_NOERROR, (UINT)0);
	strMsg.Empty();

	//	Is the revision information complete?
	if((m_iMajorVer < 0) || (m_iMinorVer < 0))
	{
		strMsg.LoadString(IDS_TMDB_INVALIDVERSION);
		MessageBox(0, strMsg, "Database Error", MB_TOPMOST | MB_ICONINFORMATION | MB_OK);
	}
	else
	{
		//	Is this an older version of the application?
		if(m_iMajorVer > (short)_wDBNETMajor)
			bOldApplication = TRUE;
		else if((m_iMajorVer == _wDBNETMajor) && (m_iMinorVer > (short)_wDBNETMinor))
			bOldApplication = TRUE;

		if(bOldApplication == TRUE)
		{
			strMsg.Format("The database was created with a newer version of TrialMax. TmaxPresentation: %d.%d Database: %d.%d. The program may not function properly. You should update as soon as possible.",
						  _wDBNETMajor, _wDBNETMinor, m_iMajorVer, m_iMinorVer);
			MessageBox(0, strMsg, "Database Error", MB_TOPMOST | MB_ICONINFORMATION | MB_OK);
		}

		//	If this database was created prior to 6.0.1 then we need to check the old
		//	designation and clip paths
		lVersion = (m_pdbDetails->m_DbMajor * 1000) + (m_pdbDetails->m_DbMinor * 100) + m_pdbDetails->m_DbBuild;
		if(lVersion < 6001)
		{
			m_bUse600FileSystem = TRUE;

			//	Refresh the case folder paths
			GetXmlCasePaths();
		}

		//	Split screen treatments weren't enabled until version 6.3.4
		if(lVersion >= 6304)
		{
			m_bSplitScreenTreatments = TRUE;
		}

	}

	return TMDB_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CDBNET::MapBarcode()
//
// 	Description:	This function is called to map the foreign barcode provided
//					by the caller to a valid Trialmax barcode.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CDBNET::MapBarcode(LPCSTR lpForeign, LPSTR lpBarcode)
{
	CString strMapped;
	BOOL	bSuccessful = FALSE;

	ASSERT(lpForeign);
	ASSERT(lpBarcode);
	
	ASSERT(m_pdbBarcodeMap != NULL);
	if(m_pdbBarcodeMap == NULL) return false;

	//	Filter the barcode map using the caller's foreign barcode
	if(m_pdbBarcodeMap->FilterOnForeign(lpForeign, this))
	{
		//	Make sure we are lined up on the correct record
		//m_pdbBarcodeMap->MoveFirst();

		//	Translate the PSTQ value to a TrialMax barcode
		if(GetBarcode(m_pdbBarcodeMap->m_PSTQ, strMapped))
		{
			//	Copy to the caller's buffer
			lstrcpy(lpBarcode, strMapped);
			bSuccessful = TRUE;
		}

	}

	return bSuccessful;
}

//==============================================================================
//
// 	Function Name:	CDBNET::Open()
//
// 	Description:	This function will open the database file in the folder
//					specified by the caller.
//
// 	Returns:		TMDB_NOERROR if successful
//
//	Notes:			If lpFilename is not provided, the default database filename
//					will be used.
//
//==============================================================================
int CDBNET::Open(LPCSTR lpFolder, LPCSTR lpFilename)
{
	CString	strIniFile;
	CString	strConnect;
	CString	strFolder;
	char	szEx[512];

	ASSERT(lpFolder);

	//	Make sure the current database is closed
	Close();

	//	Clear the last error level
	HandleError(TMDB_NOERROR, (UINT)0);

	//	Save the specified folder
	m_strRootFolder = lpFolder;
	if(m_strRootFolder.Right(1) != "\\")
		m_strRootFolder += "\\";
	
	//	What filename should we use?
	if((lpFilename != 0) && (lstrlen(lpFilename) > 0))
		m_strFilename = lpFilename;
	else
		m_strFilename = DEFAULT_DBNET_FILENAME;

	//	Build the full path specification
	m_strFilespec = (m_strRootFolder + m_strFilename);
	m_strCaseOptionsFilename = (m_strRootFolder + DEFAULT_DBNET_XML_FILENAME);

	//	Allocate a new database object
	m_pDatabase = new CDaoDatabase;
	ASSERT(m_pDatabase != 0);

	//	Does the file exist?
	if(!FindFile(m_strFilespec))
		return HandleError(TMDB_FILENOTFOUND, IDS_TMDB_FILENOTFOUND, m_strFilespec);


//double dStart = (double)GetTickCount();
		
	try
	{
		//	Load the case options XML file
		if(OpenXmlCaseOptions() == TRUE)
		{
			//	Get the drive aliases
			GetXmlAliases();

			//	Get the case folder paths
			GetXmlCasePaths();

			//	Get the global options
			GetXmlGlobal();
		}
		else
		{
			SetPathDefaults();
		}

		//	Are we going to create the source documents?
		if(m_bCreateDocuments == TRUE)
			GetDocumentsSourceFolder();

		//	Construct the password specification
		strConnect = ";PWD=";
		strConnect += TRIALMAX_PASSWORD;

		//	Open the database
		m_pDatabase->Open(m_strFilespec, FALSE, FALSE, strConnect);

		//	Open the record sets
		if(OpenRecordsets() == TMDB_NOERROR)
		{
			//	Set the default folder for all deposition videos
			m_Videos.SetRootFolder(m_aCaseFolders[CASE_FOLDER_VIDEOS]);

			//	Load the primary media records
			LoadMedia();

			//	Create the pending folders
			CreatePendingFolders();
		}
	
	}
	catch(CDaoException* e)	//	DAO errors
	{
		HandleError(TMDB_OPENDBFAILED, e->m_pErrorInfo->m_strDescription);
	}
	catch(COleException* e)	//	OLE errors
	{
		if(e->GetErrorMessage(szEx, sizeof(szEx)) == TRUE)
			HandleError(TMDB_OPENDBFAILED, szEx);
		else
			HandleError(TMDB_OPENDBFAILED, "An ole exception was raised");
			
	}
	catch(CArchiveException* e)	
	{
		if(e->GetErrorMessage(szEx, sizeof(szEx)) == TRUE)
			HandleError(TMDB_OPENDBFAILED, szEx);
		else
			HandleError(TMDB_OPENDBFAILED, "An archive exception was raised");
			
	}
	catch(CMemoryException e)	//	Memory errors
	{
		HandleError(TMDB_OPENDBFAILED, IDS_TMDB_MEMERROR);
	}
	catch(...)	//	All other errors
	{
		HandleError(TMDB_OPENDBFAILED, "A system exception was raised");
	}
//double dEnd = (double)GetTickCount();
//CString M;
//M.Format("Time to open: %.3f", ((dEnd - dStart) / 1000.0));
//MessageBox(0, M, "", MB_OK);

	//	Was there a problem?
	if(m_iLastError != TMDB_NOERROR)
		Close();

	return m_iLastError;
}

//==============================================================================
//
// 	Function Name:	CDBNET::OpenRecordset()
//
// 	Description:	This function will attempt to open the recordset provided
//					by the caller.
//
// 	Returns:		TMDB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
int CDBNET::OpenRecordset(CDaoRecordset* pSet, LPCSTR lpTable, BOOL bMustExist) 
{
	ASSERT(pSet);

	//	Try to open the recordset
	try
	{
		//	Closing and reopening has the same effect as redoing the query
		if(pSet->IsOpen())
			pSet->Close();

		#if defined DBNET_FORWARD_RECORDSETS

			pSet->Open(dbOpenSnapshot, pSet->GetDefaultSQL(), dbForwardOnly);

		#else

			pSet->Open();

			if(pSet->IsBOF() == FALSE)
				pSet->MoveFirst();

		#endif
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
// 	Function Name:	CDBNET::OpenRecordsets()
//
// 	Description:	This function will open all the database recordsets.
//
// 	Returns:		TMDB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
int CDBNET::OpenRecordsets() 
{
	//	Allocate the recordset objects
	m_pdbDetails = new CDBDetails(m_pDatabase);
	m_pdbUsers = new CDBUsers(m_pDatabase);
	m_pdbHighlighters = new CDBHighlighters(m_pDatabase);
	m_pdbExtents = new CDBExtents(m_pDatabase);
	m_pdbTranscripts = new CDBTranscripts(m_pDatabase);
	m_pdbPrimaries = new CDBPrimary(m_pDatabase);
	m_pdbSecondaries = new CDBSecondary(m_pDatabase);
	m_pdbTertiaries = new CDBTertiary(m_pDatabase);
	m_pdbQuaternaries = new CDBQuaternary(m_pDatabase);
	m_pdbBarcodeMap = new CDBBarcodeMap(m_pDatabase);

	//	Open the details recordset so that we can load the version information
	if(OpenRecordset(m_pdbDetails, DBNET_DETAILS_TABLE) != TMDB_NOERROR)
		return m_iLastError;

	//	Open the users recordset
	//if(OpenRecordset(m_pdbUsers, DBNET_USERS_TABLE) != TMDB_NOERROR)
		//return m_iLastError;

	//	Check the revision information before we go any further
	//
	//	NOTE:	We don't stop here if the revision doesn't match but we do alert
	//			the user to the potential problem
	LoadVersion();

	//	We're done with the details and users
	m_pdbDetails->Close();
	m_pdbUsers->Close();

	//	Set the split-screen treatment option for the tertiaries record set
	m_pdbTertiaries->SetSplitScreenTreatments(m_bSplitScreenTreatments);

/*
	//	Open the primary media recordset
	if(OpenRecordset(m_pdbPrimaries, DBNET_PRIMARY_TABLE) != TMDB_NOERROR)
		return m_iLastError;

	//	Open the secondary media recordset
	if(OpenRecordset(m_pdbSecondaries, DBNET_SECONDARY_TABLE) != TMDB_NOERROR)
		return m_iLastError;

	//	Open the tertiary media recordset
	if(OpenRecordset(m_pdbTertiaries, DBNET_TERTIARY_TABLE) != TMDB_NOERROR)
		return m_iLastError;

	//	Open the quaternary media recordset
	if(OpenRecordset(m_pdbQuaternaries, DBNET_QUATERNARY_TABLE) != TMDB_NOERROR)
		return m_iLastError;

	//	Open the highlighters
	if(OpenRecordset(m_pdbHighlighters, DBNET_HIGHLIGHTERS_TABLE) != TMDB_NOERROR)
		return m_iLastError;

	//	Open the extents recordset
	if(OpenRecordset(m_pdbExtents, DBNET_EXTENTS_TABLE) != TMDB_NOERROR)
		return m_iLastError;

	//	Open the transcripts recordset
	if(OpenRecordset(m_pdbTranscripts, DBNET_TRANSCRIPTS_TABLE) != TMDB_NOERROR)
		return m_iLastError;

	//	Open the barcode map recordset
	if(OpenRecordset(m_pdbBarcodeMap, DBNET_BARCODEMAP_TABLE) != TMDB_NOERROR)
		return m_iLastError;
*/
	return TMDB_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CDBNET::OpenXmlCaseOptions()
//
// 	Description:	This function is called to open the XML case options file
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CDBNET::OpenXmlCaseOptions()
{
	VARIANT				vFilename;
	CIXMLDOMDocument*	pXmlDocument = 0;
	BOOL				bSuccessful = FALSE;

	while(bSuccessful == FALSE)
	{
		//	Make sure the file exists
		if(FindFile(m_strCaseOptionsFilename) == FALSE)
		{
			HandleError(TMDB_NOCASEOPTIONS, IDS_TMDB_NOCASEOPTIONS, m_strCaseOptionsFilename);
			break;
		}

		//	Attach to the XML engine
		if((pXmlDocument = CreateXmlDocument()) == 0)
		{
			HandleError(TMDB_CREATECASEOPTIONS, IDS_TMDB_CREATECASEOPTIONS, m_strCaseOptionsFilename);
			break;
		}

		VariantInit(&vFilename);
		V_VT(&vFilename) = VT_BSTR;
		V_BSTR(&vFilename) = m_strCaseOptionsFilename.AllocSysString();

		//	Parse the XML file
		if(pXmlDocument->load(vFilename) == FALSE)
		{
			HandleError(TMDB_LOADCASEOPTIONS, IDS_TMDB_LOADCASEOPTIONS, m_strCaseOptionsFilename);
			break;
		}

		//	We're done
		bSuccessful = TRUE;
	
	}// while(bSuccessful == FALSE)

	//	Save the new document if successful
	if(bSuccessful == TRUE)
	{
		m_pXmlCaseOptions = pXmlDocument;

		//	We need the machine information to retrieve the rest of the database setup
		return GetXmlMachine();

	}
	else
	{
		DELETE_INTERFACE(pXmlDocument);		
		return FALSE;
	}

}

//==============================================================================
//
// 	Function Name:	CDBNET::PLToLine()
//
// 	Description:	This function will extract the line number from the
//					composite PL value
//
// 	Returns:		The PL line number
//
//	Notes:			None
//
//==============================================================================
long CDBNET::PLToLine(long lPL) 
{
	return (lPL % 100);
}

//==============================================================================
//
// 	Function Name:	CDBNET::PLToPage()
//
// 	Description:	This function will extract the page number from the
//					composite PL value
//
// 	Returns:		The PL page number
//
//	Notes:			None
//
//==============================================================================
long CDBNET::PLToPage(long lPL) 
{
	return ((lPL - PLToLine(lPL)) / 100);
}

//==============================================================================
//
// 	Function Name:	CDBNET::SetPathDefaults()
//
// 	Description:	This function will set default values for each of the media
//					root folder paths
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBNET::SetPathDefaults() 
{
	m_aCaseFolders[CASE_FOLDER_DOCUMENTS] = (m_strRootFolder + DEFAULT_DOCUMENTS_FOLDER);
	m_aCaseFolders[CASE_FOLDER_TREATMENTS] = (m_strRootFolder + DEFAULT_TREATMENTS_FOLDER);
	m_aCaseFolders[CASE_FOLDER_POWERPOINTS] = (m_strRootFolder + DEFAULT_POWERPOINTS_FOLDER);
	m_aCaseFolders[CASE_FOLDER_RECORDINGS] = (m_strRootFolder + DEFAULT_RECORDINGS_FOLDER);
	m_aCaseFolders[CASE_FOLDER_TRANSCRIPTS] = (m_strRootFolder + DEFAULT_TRANSCRIPTS_FOLDER);
	m_aCaseFolders[CASE_FOLDER_VIDEOS] = (m_strRootFolder + DEFAULT_VIDEOS_FOLDER);
	
	if(m_bUse600FileSystem == TRUE)
		m_aCaseFolders[CASE_FOLDER_CLIPS] = (m_strRootFolder + DEFAULT_CLIPS_600_FOLDER);
	else
		m_aCaseFolders[CASE_FOLDER_CLIPS] = (m_strRootFolder + DEFAULT_CLIPS_601_FOLDER);
}

//==============================================================================
//
// 	Function Name:	CDBNET::StripFileExtension()
//
// 	Description:	This function is will strip the extension from the specified
//					filename.
//
// 	Returns:		A pointer to the modified filename
//
//	Notes:			None
//
//==============================================================================
LPSTR CDBNET::StripFileExtension(LPSTR lpszFilename)
{
	char* pToken = 0;

	if((lpszFilename != 0) && (lstrlen(lpszFilename) > 0))
	{
		if((pToken = strrchr(lpszFilename, '.')) != 0)
		{
			*pToken = '\0';
		}

	}
	return lpszFilename;
}
