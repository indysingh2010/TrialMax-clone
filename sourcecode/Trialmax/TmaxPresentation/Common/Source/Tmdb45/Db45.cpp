//==============================================================================
//
// File Name:	tmdbase.cpp
//
// Description:	This file contains member functions of the CDB45 class.
//
// See Also:	tmdbase.h
//
//==============================================================================
//	Date		Revision    Description
//	08-04-97	1.00		Original Release
//	01-25-98	2.00		Added Barcode Manager and related members
//	02-20-98	2.00		Added FindParty() and FindDeponent()
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <db45.h>
#include <shellapi.h>
#include <tmini.h>
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
//	REVISION INFORMATION
//------------------------------------------------------------------------------
//
//	_wVerDBMajor: Major version identifer is changed when significant changes
//				  have been made to the structure of the database. Modification
//				  of tables that TrialMax uses should force an update of this
//				  identifier.
//	_wVerDBMinor: Minor version identifier is changed when changes have been 
//				  made that alter the meaning or use of data contained in the
//				  database. This is usually increased when new features have
//				  been added that will not break the current release version 
//				  but will not be supported by the current release either.
//------------------------------------------------------------------------------
const WORD	_wVerDBMajor = 1;
const WORD	_wVerDBMinor = 0;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------

//==============================================================================
//
// 	Function Name:	CDB45::AddVideoOverride()
//
// 	Description:	This function will add the RootOverride field to the 
//					VideoFiles table.	
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CDB45::AddVideoOverride()
{
	CDaoTableDef*	pTableDef = 0;
	CDaoFieldInfo	Info;
	int				iFields;

	ASSERT(m_pDatabase);
	ASSERT(m_pDatabase->IsOpen());
	ASSERT(m_pVersionSet);
	ASSERT(m_pVersionSet->IsOpen());

	//	Do we have an open database?
	if(m_pDatabase == 0 || !m_pDatabase->IsOpen())
		return FALSE;

	try
	{
		//	Allocate a new table definition
		pTableDef = new CDaoTableDef(m_pDatabase);
		ASSERT(pTableDef);

		//	Open the table definition
		pTableDef->Open(TMDBASE_VIDEO_TABLE);

		//	See if this field already exists
		iFields = pTableDef->GetFieldCount();
		for(int i = 0; i < iFields; i++)
		{
			pTableDef->GetFieldInfo(i, Info);
			if(Info.m_strName.CompareNoCase("RootOverride") == 0)
			{
				pTableDef->Close();
				delete pTableDef;
				return TRUE;
			}
		}

		//	Add the field
		Info.m_nType = dbText;
		Info.m_lAttributes = dbVariableField | dbUpdatableField;
		Info.m_bAllowZeroLength = TRUE;
		Info.m_nOrdinalPosition = 0;
		Info.m_bRequired = FALSE;
		Info.m_lCollatingOrder = 0;
		Info.m_strForeignName.Empty();
		Info.m_strSourceField.Empty();
		Info.m_strSourceTable.Empty();
		Info.m_strValidationRule.Empty();
		Info.m_strValidationText.Empty();
		Info.m_strDefaultValue.Empty();			
		Info.m_strName = "RootOverride";
		Info.m_lSize = 255;
		pTableDef->CreateField(Info);

		pTableDef->Close();
		delete pTableDef;

		//	Update the version information to reflect the change
		m_pVersionSet->MoveFirst();
		m_pVersionSet->Edit();
		m_iMajorVer = 1;
		m_iMinorVer = 0;
		m_strRevision.Format("%d.%d.%d", m_iShareVer, m_iMajorVer, m_iMinorVer);
		m_pVersionSet->m_CurrentViewerDbVersion = m_strRevision;
		m_pVersionSet->Update();

		return TRUE;
	}
	catch(...)	//	All errors
	{
		//	Delete the table definition
		if(pTableDef)
			delete pTableDef;

		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CDB45::CheckForPages()
//
// 	Description:	This function is called to verify that the database contains
//					pages for the specified media object.
//
// 	Returns:		TRUE if pages exist
//
//	Notes:			None
//
//==============================================================================
BOOL CDB45::CheckForPages(CMedia* pMedia)
{
	CString	strSQL;

	ASSERT(pMedia);
	ASSERT(m_pPageSet);

//	OOPS ! HAD TO REMOVE THIS TEST BECAUSE IT WAS TOO MUCH OF A PERFORMANCE
//	HIT FOR CASES WITH LARGE NUMBERS OF DOCUMENTS
	//strSQL.Format("[MediaID] = \"%s\" ", pMedia->m_strMediaId);
	//return m_pPageSet->FindFirst(strSQL);

return TRUE;

}

//==============================================================================
//
// 	Function Name:	CDB45::Close()
//
// 	Description:	This function will delete the record sets and close the
//					database.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDB45::Close()
{
	//	Close the record sets
	CLOSE_RECORDSET(m_pColorSet);
	CLOSE_RECORDSET(m_pBarcodeMapSet);
	CLOSE_RECORDSET(m_pShowItemSet);
	CLOSE_RECORDSET(m_pTranscriptSet);
	CLOSE_RECORDSET(m_pTextSet);
	CLOSE_RECORDSET(m_pLinkSet);
	CLOSE_RECORDSET(m_pDesignationSet);
	CLOSE_RECORDSET(m_pVideoSet);
	CLOSE_RECORDSET(m_pTreatmentSet);
	CLOSE_RECORDSET(m_pPageSet);
	CLOSE_RECORDSET(m_pMediaSet);
	CLOSE_RECORDSET(m_pVersionSet);

	//	Clear the folder and file specifications
	m_strDocumentFolder.Empty();
	m_strPowerPointFolder.Empty();
	m_strAnimationFolder.Empty();
	m_strTranscriptFolder.Empty();
	m_strTreatmentFolder.Empty();
	m_strVideoFolder.Empty();
	m_strOverlayFolder.Empty();

	//	Do the base class cleanup
	CDBAbstract::Close();
}

//==============================================================================
//
// 	Function Name:	CDB45::ConvertToPlaylist()
//
// 	Description:	This function convert the multipage media object to a
//					playlist.
//
// 	Returns:		A pointer to the playlist object if successful
//
//	Notes:			None
//
//==============================================================================
CPlaylist* CDB45::ConvertToPlaylist(CMultipage* pMultipage)
{
	CPlaylist*		pPlaylist;
	CDesignation*	pDesignation;
	CSecondary*			pSecondary;
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
	pPlaylist->m_strMediaId  = pMultipage->m_strMediaId;
	pPlaylist->m_lPrimaryId  = pMultipage->m_lPrimaryId;

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
		pDesignation->m_dStartTime = 0;
		pDesignation->m_dStopTime  = 0;

		//	Add the designation to the playlist
		pPlaylist->m_Designations.Add(pDesignation, TRUE);

		//	Get the next page
		pSecondary = pMultipage->m_Pages.Next();
	}

	return pPlaylist;
}

//==============================================================================
//
// 	Function Name:	CDB45::CDB45()
//
// 	Description:	This is the constructor for CDB45 objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDB45::CDB45() : CDBAbstract()
{
	m_pDatabase = 0;
	m_pMediaSet = 0;
	m_pVersionSet = 0;
	m_pPageSet = 0;
	m_pTreatmentSet = 0;
	m_pTextSet = 0;
	m_pVideoSet = 0;
	m_pShowItemSet = 0;
	m_pDesignationSet = 0;
	m_pLinkSet = 0;
	m_pTranscriptSet = 0;
	m_pBarcodeMapSet = 0;
	m_pColorSet = 0;
	m_iShareVer = -1;
	m_strDocumentFolder.Empty();
	m_strPowerPointFolder.Empty();
	m_strAnimationFolder.Empty();
	m_strTranscriptFolder.Empty();
	m_strTreatmentFolder.Empty();
	m_strVideoFolder.Empty();
	m_strOverlayFolder.Empty();
}

//==============================================================================
//
// 	Function Name:	CDB45::~CDB45()
//
// 	Description:	This is the destructor for CDB45 objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDB45::~CDB45()
{
	//	Make sure the case database is closed
	Close();
}

//==============================================================================
//
// 	Function Name:	CDB45::GetDesignations()
//
// 	Description:	This function is called to get the designations associated 
//					with the playlist object provided by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDB45::GetDesignations(CPlaylist* pPlaylist)
{
	CDesignation* pDesignation;

	ASSERT(pPlaylist);
	ASSERT(m_pDesignationSet);
	
	//	Flush the existing designation list
	pPlaylist->m_Designations.Flush(TRUE);

	//	Filter the designation set
	if(!m_pDesignationSet->FilterOnMedia(pPlaylist->m_strMediaId))
	{
		m_pDesignationSet->FilterOnMedia(0);
		return;
	}

	//	Add each designation to the list
	m_pDesignationSet->MoveFirst();
	while(!m_pDesignationSet->IsEOF())
	{
		//	Create a new designation object
		pDesignation = new CDesignation();
		pDesignation->m_strMediaId = m_pDesignationSet->m_MediaID;
		pDesignation->m_strDescription = m_pDesignationSet->m_Description;
		pDesignation->m_lTertiaryId = m_pDesignationSet->m_DesignationID;
		pDesignation->m_lBarcodeId = m_pDesignationSet->m_DesignationID;
		pDesignation->m_lTranscriptId = m_pDesignationSet->m_TranscriptID;
		pDesignation->m_lVideoId = m_pDesignationSet->m_VideoFileID;
		pDesignation->m_lPlaybackOrder = m_pDesignationSet->m_PlaybackOrder;
		pDesignation->m_lStartLine = m_pDesignationSet->m_StartLine;
		pDesignation->m_lStartPage = m_pDesignationSet->m_StartPage;
		pDesignation->m_dStartTime = (m_pDesignationSet->m_StartNum * DB45_FRAMES_PER_SECOND);
		pDesignation->m_lStopLine = m_pDesignationSet->m_StopLine;
		pDesignation->m_lStopPage = m_pDesignationSet->m_StopPage;
		pDesignation->m_dStopTime = (m_pDesignationSet->m_StopNum * DB45_FRAMES_PER_SECOND);
		pDesignation->m_lHighlighterId = m_pDesignationSet->m_ColorID;
		pDesignation->m_strOverlayFilename = m_pDesignationSet->m_OverlayFileName;
		pDesignation->m_strOverlayRelativePath = m_pDesignationSet->m_OverlayRelativePath;
		
		//	For some stupid reason the database allows NULL values in the VideoID
		//	field
		COleVariant Var = m_pDesignationSet->GetFieldValue("VideoFileID");
		if(Var.vt == VT_NULL)
			pDesignation->m_lVideoId = -1;

		//	Do we need to get the color for this designation?
		if((pDesignation->m_lHighlighterId > 0) && (m_pColorSet != 0))
		{
			if(m_pColorSet->GetColor(pDesignation->m_lHighlighterId, &(pDesignation->m_crHighlighter)) == FALSE)
				pDesignation->m_lHighlighterId = -1;
		}
		else if(m_pColorSet == 0)
		{
			pDesignation->m_lHighlighterId = -1;
		}

		//	Get the fully qualified specification for the overlay file
		GetOverlayFilename(pDesignation);
		
		//	Add the designation to the list
		pPlaylist->m_Designations.Add(pDesignation, FALSE);

		//	Get the list of links for this designation
		GetLinks(pDesignation);

		//	Get the transcript text for this designation
		GetText(pDesignation);
		if(pDesignation->HasText() == TRUE)
		{
			pPlaylist->SetHasText(TRUE);
		}

		//	Move to the next record			
		m_pDesignationSet->MoveNext();
	}

	//	Restore the record set
	m_pDesignationSet->FilterOnMedia(0);
}

//==============================================================================
//
// 	Function Name:	CDB45::GetFilename()
//
// 	Description:	This function is called to get the fully qualified path of
//					the file associated with the page specified by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDB45::GetFilename(CMultipage* pMultipage, CSecondary* pSecondary, CString& rFilename)
{
	ASSERT(pMultipage);
	ASSERT(pSecondary);

	//	Get the folder for this multipage object
	GetMultipageFolder(pMultipage, rFilename);
	
	//	Now add the filename
	if(pMultipage->m_lPlayerType == MEDIA_TYPE_POWERPOINT)
		rFilename += pMultipage->m_strFilename;
	else
		rFilename += pSecondary->m_strFilename;		
}

//==============================================================================
//
// 	Function Name:	CDB45::GetFilename()
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
void CDB45::GetFilename(CTertiary* pTreatment, CString& rFilename)
{
	ASSERT(pTreatment);

	//	Get the root folder for treatments
	rFilename = GetTreatmentFolder();

	if(!rFilename.IsEmpty())
	{
		if(rFilename.Right(1) != "\\")
			rFilename += "\\";
	}

	//	Now append the relative path
	if(!pTreatment->m_strRelativePath.IsEmpty())
	{
		rFilename += pTreatment->m_strRelativePath;

		if(rFilename.Right(1) != "\\")
			rFilename += "\\";
	}

	//	Now add the filename
	rFilename += pTreatment->m_strFilename;
}

//==============================================================================
//
// 	Function Name:	CDB45::GetLinks()
//
// 	Description:	This function is called to get the links associated with
//					with the designation object provided by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDB45::GetLinks(CDesignation* pDesignation)
{
	CLink* pLink;

	ASSERT(pDesignation);
	ASSERT(m_pLinkSet);

	//	Flush the existing list
	pDesignation->m_Links.Flush(TRUE);

	//	Filter the link set
	if(!m_pLinkSet->FilterOnDesignation(pDesignation->m_strMediaId, 
									    pDesignation->m_lTertiaryId))
	{
		m_pLinkSet->FilterOnMedia(0);
		return;
	}

	//	Add each link to the list
	m_pLinkSet->MoveFirst();
	while(!m_pLinkSet->IsEOF())
	{
		//	Create a new page object
		pLink = new CLink();
		pLink->m_strMediaId = m_pLinkSet->m_MediaID;
		pLink->m_strItemBarcode = m_pLinkSet->m_ItemBarcode;
		pLink->m_lOwnerId = m_pLinkSet->m_DesignationID;
		pLink->m_lId = m_pLinkSet->m_LinkID;
		pLink->m_lLine = m_pLinkSet->m_LineNum;
		pLink->m_lPage = m_pLinkSet->m_PageNum;
		pLink->m_dTrigger = (m_pLinkSet->m_TriggerNum * DB45_FRAMES_PER_SECOND);
		pLink->m_lDisplayType = m_pLinkSet->m_DisplayType;
		pLink->m_bHide = m_pLinkSet->m_HideLink;
		pLink->m_bSplitScreen = m_pLinkSet->m_SplitScreen;

		//	Create the packed flags version of the link options
		if(pLink->m_bHide)
			pLink->m_lFlags |= TMFLAG_LINK_HIDE;
		if(pLink->m_bSplitScreen)
			pLink->m_lFlags |= TMFLAG_LINK_SPLITSCREEN;
		
		//	Add the treatment to the list
		pDesignation->m_Links.Add(pLink);

		//	Move to the next record			
		m_pLinkSet->MoveNext();
	}

	//	Restore the record set
	m_pLinkSet->FilterOnMedia(0);
}

//==============================================================================
//
// 	Function Name:	CDB45::GetMultipageFolder()
//
// 	Description:	This function is called to get the default folder associated
//					with the specified multipage object
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDB45::GetMultipageFolder(CMultipage* pMultipage, CString& rFolder)
{
	//	What type of media?
	switch(pMultipage->m_lPlayerType)
	{
		case MEDIA_TYPE_IMAGE:
		
			rFolder = GetDocumentFolder();
			break;
							
		case MEDIA_TYPE_RECORDING:			
		
			rFolder = GetAnimationFolder();
			break;
							
		case MEDIA_TYPE_POWERPOINT:			
		
			rFolder = GetPowerPointFolder();
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
	
	//	Do we need to add a relative path?
	if(!pMultipage->m_strRelativePath.IsEmpty())
	{
		rFolder += pMultipage->m_strRelativePath;

		if(rFolder.Right(1) != "\\")
			rFolder += "\\";
	}			
}

//==============================================================================
//
// 	Function Name:	CDB45::GetOverlayFilename()
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
void CDB45::GetOverlayFilename(CDesignation* pDesignation)
{
	ASSERT(pDesignation);

	//	The overlay file for a designation is optional
	if(pDesignation->m_strOverlayFilename.IsEmpty())
	{
		pDesignation->m_strOverlay.Empty();
	}
	else
	{
		pDesignation->m_strOverlay = GetOverlayFolder();
		if(pDesignation->m_strOverlay.Right(1) != "\\")
			pDesignation->m_strOverlay += "\\";
		if(!pDesignation->m_strOverlayRelativePath.IsEmpty())
		{
			pDesignation->m_strOverlay += pDesignation->m_strOverlayRelativePath;
			if(pDesignation->m_strOverlay.Right(1) != "\\")
				pDesignation->m_strOverlay += "\\";
		}
		pDesignation->m_strOverlay += pDesignation->m_strOverlayFilename;
	}
}

//==============================================================================
//
// 	Function Name:	CDB45::GetSaveZapFileSpec()
//
// 	Description:	This function is called to get the fully qualified path of
//					the file used to save a new zap.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDB45::GetSaveZapFileSpec(CMultipage* pMultipage, CSecondary* pSecondary, CString& rFileSpec)
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
		rFileSpec.Format("%s%s.%s.%02d.zap!", strFolder,
						  pMultipage->m_strMediaId,
						  pSecondary->m_strFilename, i);

		//	Does this file already exist?
		if(!FindFile(rFileSpec))
			break;
	}

}

//==============================================================================
//
// 	Function Name:	CDB45::GetSecondaries()
//
// 	Description:	This function is called to get the secondary objects
//					associated with the specified multipage object
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDB45::GetSecondaries(CMultipage* pMultipage)
{
	CSecondary* pSecondary;

	ASSERT(pMultipage);
	ASSERT(m_pPageSet);

	//	Flush the existing page list
	pMultipage->m_Pages.Flush(TRUE);

	//	Filter the pages set
	if(!m_pPageSet->FilterOnMedia(pMultipage->m_strMediaId))
	{
		m_pPageSet->FilterOnMedia(0);
		return;
	}

	//	Add each page to the list
	m_pPageSet->MoveFirst();
	while(!m_pPageSet->IsEOF())
	{
		//	Create a new page object
		pSecondary = new CSecondary();
		pSecondary->m_strMediaId = m_pPageSet->m_MediaID;
		pSecondary->m_lSecondaryId = m_pPageSet->m_PageID;
		pSecondary->m_lSlideId = m_pPageSet->m_SlideID;
		pSecondary->m_lPlaybackOrder = m_pPageSet->m_PlaybackOrder;
		pSecondary->m_lDisplayType = m_pPageSet->m_DisplayType;
		pSecondary->m_strFilename = m_pPageSet->m_FileName;

		pSecondary->m_lBarcodeId = pSecondary->m_lSecondaryId;
		pSecondary->m_lPrimaryId = pMultipage->m_lPrimaryId;

		//	Add the page to the list
		pMultipage->m_Pages.Add(pSecondary, FALSE);

		//	Move to the next record			
		m_pPageSet->MoveNext();
	}

	//	Restore the record set
	m_pPageSet->FilterOnMedia(0);
}

//==============================================================================
//
// 	Function Name:	CDB45::GetShowItems()
//
// 	Description:	This function is called to get the show items associated 
//					with the custom show object provided by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDB45::GetShowItems(CShow* pShow, long lSecondary)
{
	CShowItem* pShowItem;

	ASSERT(pShow);
	ASSERT(m_pShowItemSet);

	//	Flush the existing page list
	pShow->m_Items.Flush(TRUE);

	//	Filter the pages set
	if(!m_pShowItemSet->FilterOnMedia(pShow->m_strMediaId))
	{
		m_pShowItemSet->FilterOnMedia(0);
		return;
	}

	//	Add each item to the list
	m_pShowItemSet->MoveFirst();
	while(!m_pShowItemSet->IsEOF())
	{
		//	Add this item if it is not hidden
		if(!m_pShowItemSet->m_Hide)
		{
			//	Create a new object
			pShowItem = new CShowItem();
			pShowItem->m_lSecondaryId = m_pShowItemSet->m_ShowItemID;
			pShowItem->m_lPlaybackOrder = m_pShowItemSet->m_PlaybackOrder;
			pShowItem->m_strMediaId = m_pShowItemSet->m_MediaID;
			pShowItem->m_strDescription = m_pShowItemSet->m_Description;
			pShowItem->m_strItemBarcode = m_pShowItemSet->m_ItemBarcode;

			pShowItem->m_lBarcodeId = pShowItem->m_lSecondaryId;

			//	Add the item to the list
			pShow->m_Items.Add(pShowItem, FALSE);
		}

		//	Move to the next record			
		m_pShowItemSet->MoveNext();
	}

	//	Restore the record set
	m_pShowItemSet->FilterOnMedia(0);
}

//==============================================================================
//
// 	Function Name:	CDB45::GetText()
//
// 	Description:	This function is called to get the links associated with
//					with the designation object provided by the caller.
//
// 	Returns:		TRUE if text was found in the database
//
//	Notes:			None
//
//==============================================================================
BOOL CDB45::GetText(CDesignation* pDesignation)
{
	CTextLine*	pLine;
	CTextPage*	pPage;

	ASSERT(pDesignation);
	ASSERT(m_pTextSet);

	//	Flush the existing list
	pDesignation->m_Pages.Flush(TRUE);

	//	Filter the record set
	if(!m_pTextSet->FilterOnDesignation(pDesignation->m_strMediaId, 
									    pDesignation->m_lTertiaryId))
	{
		m_pTextSet->FilterOnMedia(0);
		return FALSE;
	}

	m_pTextSet->MoveFirst();
	while(!m_pTextSet->IsEOF())
	{
		//	Create a new line object
		pLine = new CTextLine();
		pLine->m_strMediaId = m_pTextSet->m_MediaID;
		pLine->m_lDesignationId = m_pTextSet->m_DesignationID;
		pLine->m_lLineNum = m_pTextSet->m_LineNum;
		pLine->m_lPageNum = m_pTextSet->m_PageNum;
		pLine->m_dStartTime = (m_pTextSet->m_FirstNum * DB45_FRAMES_PER_SECOND);
		pLine->m_dStopTime  = (m_pTextSet->m_LastNum * DB45_FRAMES_PER_SECOND);
		pLine->m_strText = m_pTextSet->m_TextLine;
		pLine->m_lDesignationOrder = pDesignation->m_lPlaybackOrder;

		//	Should we assign the line color?
		if(pDesignation->m_lHighlighterId > 0)
			pLine->m_crColor = pDesignation->m_crHighlighter;
		else
			pLine->m_crColor = 0xFF000000; // Invalid RGB 

		//	Does this page already exist?
		if((pPage = pDesignation->m_Pages.Find(pLine->m_lPageNum)) != 0)
		{
			//	Add this line
			pPage->m_Lines.Add(pLine);
		}
		else
		{
			//	Allocate and initialize the new page
			pPage = new CTextPage();
			pPage->m_lDesignationId = pLine->m_lDesignationId;
			pPage->m_lPageNum = pLine->m_lPageNum;
			pPage->m_lDesignationOrder = pDesignation->m_lPlaybackOrder;

			//	Add the page
			pDesignation->m_Pages.Add(pPage);

			//	Add the line
			pPage->m_Lines.Add(pLine);
		}

		//	Move to the next record			
		m_pTextSet->MoveNext();
	}

	//	Restore the record set
	m_pTextSet->FilterOnMedia(0);

	return (pDesignation->m_Pages.GetCount() > 0);
}

//==============================================================================
//
// 	Function Name:	CDB45::GetTreatments()
//
// 	Description:	This function is called to get the treatments associated 
//					with the page object provided by the caller.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CDB45::GetTertiaries(CSecondary* pSecondary)
{
	CTertiary* pTreatment;

	ASSERT(pSecondary);
	ASSERT(m_pTreatmentSet);

	if(m_pTreatmentSet == 0)
		return FALSE;

	//	Flush the existing treatment list
	pSecondary->m_Children.Flush(TRUE);

	//	Filter the treatment set
	if(!m_pTreatmentSet->FilterOnPage(pSecondary->m_strMediaId, pSecondary->m_lSecondaryId))
	{
		m_pTreatmentSet->FilterOnMedia(0);
		return TRUE;
	}

	//	Add each treatment to the list
	m_pTreatmentSet->MoveFirst();
	while(!m_pTreatmentSet->IsEOF())
	{
		//	Create a new object
		pTreatment = new CTertiary(pSecondary);
		pTreatment->m_strMediaId = m_pTreatmentSet->m_MediaID;
		pTreatment->m_lPrimaryId = pSecondary->m_lPrimaryId;
		pTreatment->m_lSecondaryId = m_pTreatmentSet->m_PageID;
		pTreatment->m_lTertiaryId = m_pTreatmentSet->m_TreatmentID;
		pTreatment->m_lPlaybackOrder = m_pTreatmentSet->m_PlaybackOrder;
		pTreatment->m_strDescription = m_pTreatmentSet->m_Description;
		pTreatment->m_strFilename = m_pTreatmentSet->m_FileName;
		pTreatment->m_strRelativePath = m_pTreatmentSet->m_RelativePath;

		//	.NET version uses treatment id as the barcode id
		pTreatment->m_lBarcodeId = pTreatment->m_lTertiaryId;

		//	Add to the list
		pSecondary->m_Children.Add(pTreatment, FALSE);

		//	Move to the next record			
		m_pTreatmentSet->MoveNext();
	}

	//	Restore the record set
	m_pTreatmentSet->FilterOnMedia(0);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CDB45::LoadMedia()
//
// 	Description:	This function is called to load the media lists using the
//					records in the media table.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
int CDB45::LoadMedia()
{
	CMedia*	pMedia;

	ASSERT(m_pMediaSet);
	if((m_pMediaSet == 0) || !m_pMediaSet->IsOpen())
		return HandleError(TMDB_INVALIDRECORDSET, IDS_TMDB_INVALIDRECORDSET);

	//	Are there any records?
	if(m_pMediaSet->IsBOF() || m_pMediaSet->IsEOF())
		return HandleError(TMDB_NOERROR, (UINT)0);

	//	Move to the first record
	m_pMediaSet->MoveFirst();

	//	Search for the correct page
	while(!m_pMediaSet->IsEOF())
	{
		//	Create a new media object
		pMedia = new CMedia();
		pMedia->m_strMediaId = m_pMediaSet->m_MediaID;
		pMedia->m_lPrimaryId = m_pMediaSet->m_GhostMediaId;
		pMedia->m_lPlayerType = m_pMediaSet->m_MediaPlayerType;
		pMedia->m_lFlags = m_pMediaSet->m_FlagsBinary;
		pMedia->m_strFilename = m_pMediaSet->m_FileName;
		pMedia->m_strName = m_pMediaSet->m_MediaName;
		pMedia->m_strRelativePath = m_pMediaSet->m_RelativePath;

		//	What type of media is this?
		switch(pMedia->m_lPlayerType)
		{
			case MEDIA_TYPE_PLAYLIST:

				m_Playlists.Add(pMedia);
				break;
							
			case MEDIA_TYPE_CUSTOMSHOW:
			case MEDIA_TYPE_IMAGE:				
			case MEDIA_TYPE_RECORDING:			
			case MEDIA_TYPE_POWERPOINT:			
			default:

				//	Make sure this document has pages
				if(!CheckForPages(pMedia))
				{
					delete pMedia;
				}
				else
				{
					m_Multipages.Add(pMedia);
				}
				break;
		}
		
		//	Move to the next record			
		m_pMediaSet->MoveNext();
	}

	m_Multipages.SetExtents();
	m_Playlists.SetExtents();

	return HandleError(TMDB_NOERROR, (UINT)0);
}

//==============================================================================
//
// 	Function Name:	CDB45::LoadTranscripts()
//
// 	Description:	This function is called to load the transcript list using 
//					the records in the transcripts table.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
int CDB45::LoadTranscripts()
{
	CTranscript* pTranscript;

	ASSERT(m_pTranscriptSet);
	if((m_pTranscriptSet == 0) || !m_pTranscriptSet->IsOpen())
		return HandleError(TMDB_INVALIDRECORDSET, IDS_TMDB_INVALIDRECORDSET);

	//	Are there any records?
	if(m_pTranscriptSet->IsBOF() || m_pTranscriptSet->IsEOF())
		return HandleError(TMDB_NOERROR, (UINT)0);

	//	Move to the first record
	m_pTranscriptSet->MoveFirst();

	while(!m_pTranscriptSet->IsEOF())
	{
		//	Create a new media object
		pTranscript = new CTranscript();
		pTranscript->m_lTranscriptId = m_pTranscriptSet->m_TranscriptID;
		pTranscript->m_strBaseFilename = m_pTranscriptSet->m_BaseFileName;
		pTranscript->m_strCtxExtension = m_pTranscriptSet->m_OrigCtxFileExtension;
		pTranscript->m_strDbExtension = m_pTranscriptSet->m_OrigLogDbFileExtension;
		pTranscript->m_strRelativePath = m_pTranscriptSet->m_RelativePath;
		pTranscript->m_strTranscriptName = m_pTranscriptSet->m_TranscriptName;
		pTranscript->m_strDate = m_pTranscriptSet->m_TranscriptDate.Format("%m-%d-%Y");

		//	Add this video to the list
		m_Transcripts.Add(pTranscript);
		
		//	Move to the next record			
		m_pTranscriptSet->MoveNext();
	}

	return HandleError(TMDB_NOERROR, (UINT)0);
}

//==============================================================================
//
// 	Function Name:	CDB45::LoadVersion()
//
// 	Description:	This function is called to load the version information.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
int CDB45::LoadVersion()
{
	char	szVersion[256];
	char*	pToken = NULL;
	char*	pNext = NULL;
	CString	strMsg;
	BOOL	bMajorError = FALSE;

	ASSERT(m_pVersionSet);
	if((m_pVersionSet == 0) || !m_pVersionSet->IsOpen())
		return HandleError(TMDB_INVALIDRECORDSET, IDS_TMDB_INVALIDRECORDSET);

	//	Are there any records?
	if(m_pVersionSet->IsBOF() || m_pVersionSet->IsEOF())
		return HandleError(TMDB_NOVERSIONINFO, IDS_TMDB_NOVERSIONINFO);

	//	Make sure we are on the first record (There should only be one)
	m_pVersionSet->MoveFirst();

	//	Save the revision information
	m_strRevision = m_pVersionSet->m_CurrentViewerDbVersion;
	m_strCreator  = m_pVersionSet->m_CreatedBy;

	//	For some stupid reason the database allows NULL values in the 
	//	version field the we need to check
	COleVariant Var = m_pVersionSet->GetFieldValue("CurrentViewerDbVersion");
	if(Var.vt == VT_NULL)
		m_strRevision = "-1.0";

	//	Parse the revision information into it's numerical identifiers
	m_iShareVer = -1;
	m_iMajorVer = -1;
	m_iMinorVer = -1;
	lstrcpyn(szVersion, m_strRevision, sizeof(szVersion));
	if((pToken = strtok_s(szVersion, ".", &pNext)) != 0)
	{
		m_iShareVer = atoi(pToken);
		
		//	Get the major version number
		if((pToken = strtok_s(NULL, ".", &pNext)) != 0)
		{
			m_iMajorVer = atoi(pToken);
		
			//	Get the minor revision number
			if((pToken = strtok_s(NULL, ".", &pNext)) != 0)
				m_iMinorVer = atoi(pToken);
		}
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
	}
	else
	{
		//	Are the major revision identifiers different?
		if(m_iMajorVer != (short)_wVerDBMajor)
		{
			//	Are we moving from rev 0 to 1?
			if((short)_wVerDBMajor == 1)
			{
				bMajorError = (AddVideoOverride() == FALSE);
			}
			else
			{
				bMajorError = TRUE;
			}

			if(bMajorError)
			{
				//	Is this a newer version of the application
				if(m_iMajorVer < _wVerDBMajor)
					strMsg.Format("The application and database revision identifiers do not match. This is a newer version of the application: TrialMax: %d.%d Database: %d.%d. The program may be unable to open the database.",
								  _wVerDBMajor, _wVerDBMinor, m_iMajorVer, m_iMinorVer);
				else
					strMsg.Format("The application and database revision identifiers do not match. This is an older version of the application: TrialMax: %d.%d Database: %d.%d. The program may be unable to open the database.",
								  _wVerDBMajor, _wVerDBMinor, m_iMajorVer, m_iMinorVer);
			}
		}
		else
		{
			//	If the minor revisions are different, we only worry if this is
			//	a newer database because some features may not be supported
			if(m_iMinorVer > _wVerDBMinor)
			{
				strMsg.Format("The application and database revision identifiers do not match. This is a newer version of the database: TrialMax: %d.%d Database: %d.%d. Some features may not be supported.",
							  _wVerDBMajor, _wVerDBMinor, m_iMajorVer, m_iMinorVer);
			}
		}
	}

	//	Do we need to display an error message?
	if(!strMsg.IsEmpty())
		MessageBox(0, strMsg, "Database Error", MB_TOPMOST | MB_ICONINFORMATION | MB_OK);

	return TMDB_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CDB45::LoadVideos()
//
// 	Description:	This function is called to load the video list using the
//					records in the video files table.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
int CDB45::LoadVideos()
{
	CVideo*	pVideo;

	ASSERT(m_pVideoSet);
	if((m_pVideoSet == 0) || !m_pVideoSet->IsOpen())
		return HandleError(TMDB_INVALIDRECORDSET, IDS_TMDB_INVALIDRECORDSET);

	//	Are there any records?
	if(m_pVideoSet->IsBOF() || m_pVideoSet->IsEOF())
		return HandleError(TMDB_NOERROR, (UINT)0);

	//	Move to the first record
	m_pVideoSet->MoveFirst();

	//	Search for the correct page
	while(!m_pVideoSet->IsEOF())
	{
		//	Create a new media object
		pVideo = new CVideo();
		pVideo->m_lVideoFileId = m_pVideoSet->m_VideoFileID;
		pVideo->m_lTranscriptId = m_pVideoSet->m_TranscriptID;
		pVideo->m_lBeginNum = m_pVideoSet->m_BeginNum;
		pVideo->m_lEndNum = m_pVideoSet->m_EndNum;
		pVideo->m_lMaxSelStart = m_pVideoSet->m_MaxSelStart;
		pVideo->m_lMinSelStart = m_pVideoSet->m_MinSelStart;
		pVideo->m_lUnitType = m_pVideoSet->m_UnitType;
		pVideo->m_bBeginTuned = m_pVideoSet->m_BeginTuned;
		pVideo->m_bEndTuned = m_pVideoSet->m_EndTuned;
		pVideo->m_strFilename = m_pVideoSet->m_FileName;
		pVideo->m_strRelativePath = m_pVideoSet->m_RelativePath;
		pVideo->m_strRootOverride = m_pVideoSet->m_RootOverride;

		//	Add this video to the list
		m_Videos.Add(pVideo);
		
		//	Move to the next record			
		m_pVideoSet->MoveNext();
	}

	//	Set the root path for the video files
	m_Videos.SetRootFolder(GetVideoFolder());

	return HandleError(TMDB_NOERROR, (UINT)0);
}

//==============================================================================
//
// 	Function Name:	CDB45::MapBarcode()
//
// 	Description:	This function is called to map the foreign barcode provided
//					by the caller to a valid Trialmax barcode.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CDB45::MapBarcode(LPCSTR lpForeign, LPSTR lpBarcode)
{
	ASSERT(lpForeign);
	ASSERT(lpBarcode);

	//	Does this database have the barcode map table?
	if(m_pBarcodeMapSet != 0)
	{
		//	Filter the barcode map using the caller's foreign barcode
		if(m_pBarcodeMapSet->FilterOnForeignCode(lpForeign))
		{
			//	Make sure we are lined up on the correct record
			m_pBarcodeMapSet->MoveFirst();

			//	Copy to the caller's buffer
			lstrcpy(lpBarcode, m_pBarcodeMapSet->m_Barcode);
		
			m_pBarcodeMapSet->FilterOnForeignCode(0);
			return TRUE;
		}
		else
		{
			m_pBarcodeMapSet->FilterOnForeignCode(0);
			return FALSE;
		}
	}
	else
	{
		return FALSE;
	}
}


CBinderEntry CDB45::GetBinderEntryByAutoId(long lAutoId)
{
	//m_pdbBinderEntry = new CDBBinderEntry(m_pDatabase);

	/*if(m_pdbBinderEntry->FilterOnId(lAutoId, this) == TRUE)
		return TRUE;*/

	CBinderEntry	binderEntry;
	return binderEntry;
}

list<CBinderEntry> CDB45::GetBinderEntryByParentId(long lAutoId)
{
	//m_pdbBinderEntry = new CDBBinderEntry(m_pDatabase);

	/*if(m_pdbBinderEntry->FilterOnId(lAutoId, this) == TRUE)
		return TRUE;*/

	list<CBinderEntry>	binderEntryList;
	return binderEntryList;
}
//==============================================================================
//
// 	Function Name:	CDB45::Open()
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
int CDB45::Open(LPCSTR lpFolder, LPCSTR lpFilename)
{
	CString	strIniFile;
	CString	strConnect;

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
		m_strFilename = DEFAULT_DB45_FILENAME;

	//	Build the full path specification
	m_strFilespec = m_strRootFolder + m_strFilename;

	//	Allocate a new database object
	m_pDatabase = new CDaoDatabase;
	ASSERT(m_pDatabase != 0);

	//	Does the file exist?
	if(!FindFile(m_strFilespec))
		return HandleError(TMDB_FILENOTFOUND, IDS_TMDB_FILENOTFOUND, m_strFilespec);
		
	try
	{
		//	Construct the password specification
		strConnect = ";PWD=";
		strConnect += TRIALMAX_PASSWORD;

		//	Open the database
		//m_pDatabase->Open(m_strFilespec);
		m_pDatabase->Open(m_strFilespec, FALSE, FALSE, strConnect);
		AfxMessageBox(_T("Opened"));

		//	Open the record sets
		if(OpenRecordsets() == TMDB_NOERROR)
		{
			//	Open the ini file for this case
			strIniFile = m_strRootFolder + DEFAULT_DB45_INI_FILENAME;
			m_CaseIni.Open(strIniFile);

			//	Set the paths to the various media locations
			SetMediaPaths();

			//	Load the video list
			LoadVideos();

			//	Load the transcript list
			LoadTranscripts();

			//	Load the media lists
			LoadMedia();

			//	Create the pending folders
			CreatePendingFolders();
		}
	
	}
	catch(CDaoException* e)	//	DAO errors
	{
		HandleError(TMDB_OPENDBFAILED, e->m_pErrorInfo->m_strDescription);
	}
	catch(CMemoryException e)	//	Memory errors
	{
		HandleError(TMDB_OPENDBFAILED, IDS_TMDB_MEMERROR);
	}
	catch(...)	//	All other errors
	{
		HandleError(TMDB_OPENDBFAILED, IDS_TMDB_OPENDBFAILED);
	}

	//	Was there a problem?
	if(m_iLastError != TMDB_NOERROR)
		Close();

	return m_iLastError;
}

//==============================================================================
//
// 	Function Name:	CDB45::OpenRecordsets()
//
// 	Description:	This function will open all the database recordsets.
//
// 	Returns:		TMDB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
int CDB45::OpenRecordsets() 
{
	//	Allocate the recordset objects
	m_pVersionSet = new CVersionSet(m_pDatabase);
	m_pMediaSet = new CMediaSet(m_pDatabase);
	m_pPageSet = new CPageSet(m_pDatabase);
	m_pTreatmentSet = new CTreatmentSet(m_pDatabase);
	m_pVideoSet = new CVideoSet(m_pDatabase);
	m_pDesignationSet = new CDesignationSet(m_pDatabase);
	m_pLinkSet = new CLinkSet(m_pDatabase);
	m_pTextSet = new CTextSet(m_pDatabase);
	m_pTranscriptSet = new CTranscriptSet(m_pDatabase);
	m_pShowItemSet = new CShowItemSet(m_pDatabase);
	m_pBarcodeMapSet = new CBarcodeMapSet(m_pDatabase);
	m_pColorSet = new CColorSet(m_pDatabase);
	m_pdbBinderEntry = new CDBBinderEntries(m_pDatabase);

	//	Open the version recordset
	if(OpenRecordset(m_pVersionSet, TMDBASE_VERSION_TABLE) != TMDB_NOERROR)
		return m_iLastError;

	//	Check the revision information before we go any further
	//
	//	NOTE:	We don't stop here if the revision doesn't match but we do alert
	//			the user to the potential problem
	LoadVersion();

	//	Open the media recordset
	if(OpenRecordset(m_pMediaSet, TMDBASE_MEDIA_TABLE) != TMDB_NOERROR)
		return m_iLastError;

	//	Open the pages recordset
	if(OpenRecordset(m_pPageSet, TMDBASE_PAGES_TABLE) != TMDB_NOERROR)
		return m_iLastError;

	//	Open the treatments recordset
	if(OpenRecordset(m_pTreatmentSet, TMDBASE_TREATMENTS_TABLE) != TMDB_NOERROR)
		return m_iLastError;

	//	Open the video recordset
	if(OpenRecordset(m_pVideoSet, TMDBASE_VIDEO_TABLE) != TMDB_NOERROR)
		return m_iLastError;

	//	Open the designation recordset
	if(OpenRecordset(m_pDesignationSet, TMDBASE_DESIGNATIONS_TABLE) != TMDB_NOERROR)
		return m_iLastError;

	//	Open the link recordset
	if(OpenRecordset(m_pLinkSet, TMDBASE_LINKS_TABLE) != TMDB_NOERROR)
		return m_iLastError;

	//	Open the text recordset
	if(OpenRecordset(m_pTextSet, TMDBASE_TEXT_TABLE) != TMDB_NOERROR)
		return m_iLastError;

	//	Open the transcript recordset
	if(OpenRecordset(m_pTranscriptSet, TMDBASE_TRANSCRIPTS_TABLE) != TMDB_NOERROR)
		return m_iLastError;

	//	Open the custom show items recordset
	if(OpenRecordset(m_pShowItemSet, TMDBASE_SHOWITEMS_TABLE) != TMDB_NOERROR)
		return m_iLastError;

	
	if(OpenRecordset(m_pdbBinderEntry, TMDBASE_BINDERENTRIES_TABLE) != TMDB_NOERROR)
		return m_iLastError;

	//	Open the barcode map recordset
	//	
	//	NOTE:	Databases created prior to version 5 do not have this table
	if(OpenRecordset(m_pBarcodeMapSet, TMDBASE_BARCODEMAP_TABLE, FALSE) != TMDB_NOERROR)
	{
		delete m_pBarcodeMapSet;
		m_pBarcodeMapSet = 0;
	}

	//	Open the colors recordset
	if(OpenRecordset(m_pColorSet, TMDBASE_COLORS_TABLE) != TMDB_NOERROR)
	{
		delete m_pColorSet;
		m_pColorSet = 0;
	}


	return m_iLastError;
}

//==============================================================================
//
// 	Function Name:	CDB45::SetMediaPaths()
//
// 	Description:	This function is called to set the paths to the media
//					subfolders.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDB45::SetMediaPaths() 
{
	char szIniStr[512];

	//	Build the default subfolder specifications
	m_strDocumentFolder = m_strRootFolder + DEFAULT_DOCUMENT_FOLDER;
	m_strPowerPointFolder = m_strRootFolder + DEFAULT_POWERPOINT_FOLDER;
	m_strAnimationFolder = m_strRootFolder + DEFAULT_ANIMATION_FOLDER;
	m_strTranscriptFolder = m_strRootFolder + DEFAULT_TRANSCRIPT_FOLDER;
	m_strTreatmentFolder = m_strRootFolder + DEFAULT_TREATMENT_FOLDER;
	m_strVideoFolder = m_strRootFolder + DEFAULT_VIDEO_FOLDER;
	m_strOverlayFolder = m_strRootFolder + DEFAULT_OVERLAY_FOLDER;

	//	Has the ini file been opened?
	if(!m_CaseIni.bFileFound)
	{
		return;
	}
	else
	{
		//	Set the ini file to the appropriate section
		m_CaseIni.SetSection(CASEINI_PATHS_SECTION);
	}

	//	Has the document path been overridden?
	m_CaseIni.ReadString(CASEINI_DOCUMENTPATH_LINE, szIniStr, sizeof(szIniStr));
	if(lstrlen(szIniStr) > 0)
		m_strDocumentFolder = szIniStr;

	//	Has the PowerPoint path been overridden?
	m_CaseIni.ReadString(CASEINI_POWERPOINTPATH_LINE, szIniStr, sizeof(szIniStr));
	if(lstrlen(szIniStr) > 0)
		m_strPowerPointFolder = szIniStr;

	//	Has the animation path been overridden?
	m_CaseIni.ReadString(CASEINI_ANIMATIONPATH_LINE, szIniStr, sizeof(szIniStr));
	if(lstrlen(szIniStr) > 0)
		m_strAnimationFolder = szIniStr;

	//	Has the treatment path been overridden?
	m_CaseIni.ReadString(CASEINI_TREATMENTPATH_LINE, szIniStr, sizeof(szIniStr));
	if(lstrlen(szIniStr) > 0)
		m_strTreatmentFolder = szIniStr;

	//	Has the transcript path been overridden?
	m_CaseIni.ReadString(CASEINI_TRANSCRIPTPATH_LINE, szIniStr, sizeof(szIniStr));
	if(lstrlen(szIniStr) > 0)
		m_strTranscriptFolder = szIniStr;

	//	Has the video path been overridden?
	m_CaseIni.ReadString(CASEINI_VIDEOPATH_LINE, szIniStr, sizeof(szIniStr));
	if(lstrlen(szIniStr) > 0)
		m_strVideoFolder = szIniStr;

	//	Has the overlay path been overridden?
	m_CaseIni.ReadString(CASEINI_OVERLAYPATH_LINE, szIniStr, sizeof(szIniStr));
	if(lstrlen(szIniStr) > 0)
		m_strOverlayFolder = szIniStr;
}


