//==============================================================================
//
// File Name:	dbnet.h
//
// Description:	This file contains the declaration of the CDBNET class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	12-27-2003	1.00		Original Release
//==============================================================================
#if !defined(__DBNET_H__)
#define __DBNET_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <dbabstract.h>
#include <dbdefs.h>
#include <tmini.h>
#include <video.h>
#include <msxml3.h>

#include <alias.h>
#include <machine.h>
#include <dbdetails.h>
#include <dbusers.h>
#include <dbhighlighters.h>
#include <dbextents.h>
#include <dbprimary.h>
#include <dbsecondary.h>
#include <dbtertiary.h>
#include <dbquaternary.h>
#include <dbtranscripts.h>
#include <dbbarcodemap.h>
#include <Dbbinderentry.h>
#include <list>
using namespace std;
//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Case folder indexes
#define CASE_FOLDER_DOCUMENTS			0
#define CASE_FOLDER_TREATMENTS			1
#define CASE_FOLDER_POWERPOINTS			2
#define CASE_FOLDER_RECORDINGS			3
#define CASE_FOLDER_TRANSCRIPTS			4
#define CASE_FOLDER_VIDEOS				5
#define CASE_FOLDER_CLIPS				6
#define MAX_CASE_FOLDERS				7

//	Default subfolders
#define DEFAULT_DOCUMENTS_FOLDER		"_tmax_documents\\"
#define DEFAULT_TREATMENTS_FOLDER		"_tmax_treatments\\"
#define DEFAULT_POWERPOINTS_FOLDER		"_tmax_powerpoints\\"
#define DEFAULT_RECORDINGS_FOLDER		"_tmax_recordings\\"
#define DEFAULT_TRANSCRIPTS_FOLDER		"_tmax_transcripts\\"
#define DEFAULT_VIDEOS_FOLDER			"_tmax_videos\\"
#define DEFAULT_CLIPS_601_FOLDER		"_tmax_clips\\"
#define DEFAULT_CLIPS_600_FOLDER		"_tmax_designations\\"

//	Database table names
#define DBNET_DETAILS_TABLE				"Details"
#define DBNET_USERS_TABLE				"Users"
#define DBNET_HIGHLIGHTERS_TABLE		"Highlighters"
#define DBNET_EXTENTS_TABLE				"Extents"
#define DBNET_TRANSCRIPTS_TABLE			"Transcripts"
#define DBNET_PRIMARY_TABLE				"PrimaryMedia"
#define DBNET_SECONDARY_TABLE			"SecondaryMedia"
#define DBNET_TERTIARY_TABLE			"TertiaryMedia"
#define DBNET_QUATERNARY_TABLE			"QuaternaryMedia"
#define DBNET_BARCODEMAP_TABLE			"BarcodeMap"
#define DBNET_BINDERENTRIES_TABLE		"BinderEntries"


//#define DBNET_FORWARD_RECORDSETS	1	//	Use forward only record sets

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CDBNET : public CDBAbstract
{
	private:

		CString				m_aCaseFolders[MAX_CASE_FOLDERS];
		CMachine			m_Machine;
		CAliases			m_Aliases;
		CMedias				m_Depositions;
		CString				m_strCaseOptionsFilename;
		CString				m_strDocumentsSourceFolder;
		BOOL				m_bUse600FileSystem;
		BOOL				m_bSplitScreenTreatments;
		BOOL				m_bSyncXmlDesignations;

		CDBDetails*			m_pdbDetails;
		CDBUsers*			m_pdbUsers;
		CDBPrimary*			m_pdbPrimaries;
		CDBSecondary*		m_pdbSecondaries;
		CDBTertiary*		m_pdbTertiaries;
		CDBQuaternary*		m_pdbQuaternaries;
		CDBHighlighters*	m_pdbHighlighters;
		CDBExtents*			m_pdbExtents;
		CDBTranscripts*		m_pdbTranscripts;
		CDBBarcodeMap*		m_pdbBarcodeMap;
		CDBBinderEntries*	m_pdbBinderEntry;

		CIXMLDOMDocument*	m_pXmlCaseOptions;

	public:


							CDBNET();
						   ~CDBNET();

		//	File specifications
		void				GetFilename(CMultipage* pMultipage, CSecondary* pSecondary, CString& rFilename);
		void				GetFilename(CTertiary* pTertiary, CString& rFilename);
		void				GetFilename(CTranscript* pTranscript, CString& rFilename);
		void				GetOverlayFilename(CDesignation* pDesignation);
		void				GetMultipageFolder(CMultipage* pMultipage, CString& rFolder);
		void				GetSaveZapFileSpec(CMultipage* pMultipage, CSecondary* pSecondary, CString& rFileSpec);
		void				GetSaveZapFileSpecs(CMultipage* pTLMultipage, CSecondary* pTLPage, CString& rTLFileSpec,
												CMultipage* pBRMultipage, CSecondary* pBRPage, CString& rBRFileSpec,
												DWORD dwFlags = 0);

		void				Close();
		BOOL				IsNETDatabase(){ return TRUE; }
		int					Open(LPCSTR lpFolder, LPCSTR lpFilename = 0);
		int					OpenRecordset(CDaoRecordset* pSet, LPCSTR lpTable,
										  BOOL bMustExist = TRUE);
		
		CMedia*				GetMedia(LPCSTR lpMediaId);
		CMedia*				GetMedia(long lId);
		BOOL				GetTertiaries(CSecondary* pSecondary);
		CPlaylist*			ConvertToPlaylist(CMultipage* pMultipage);
		CPlaylist*			ConvertToPlaylist(CDeposition* pDeposition);
		BOOL				MapBarcode(LPCSTR lpForeign, LPSTR lpBarcode);
		BOOL				GetBarcode(LPCSTR lpszUniqueId, CString& rBarcode);
		BOOL				AddTreatment(CSecondary* pSecondary, long lTertiaryId,
										 long lDisplayOrder, long lBarcodeId,
										 LPCSTR lpszFilename);
		CPlaylist*			GetPlaylistFromScene(long lSceneId);
		CTertiary*			GetClip(CSecondary* pSegment, long lTertiaryBarcodeId, long lSceneId);

		CDeposition*		GetDeposition(CMedia* pMedia);
		list<CBinderEntry>	GetBinderEntryByParentId(long lParentId);
		list<CBinderEntry>	GetBinderEntryByParentIdWithAttribute(long lParentId);
		CBinderEntry		GetBinderEntryByAutoId(long lAutoId);
		CBinderEntry		GetBinderEntryByMediaId(CString pMediaId);
		CBinderEntry		GetBinderEntryFromSearchMediaId(CString pMediaId);
		CBinderEntry		GetPrimaryMediaById(long lAutoId);
		CBinderEntry		GetPrimaryMediaByMediaId(CString pMediaId);
		CBinderEntry		GetSecondaryMediaById(long lAutoId);
		list<CBinderEntry>	GetSecondaryMediaByPrimaryMediaId(long lprimaryMediaId);
		CBinderEntry		GetTertiaryMediaById(long lAutoId);
		list<CBinderEntry>	GetTertiaryMediaBySecondaryId(long lSecondaryMediaId);
		CBinderEntry		GetQuarternaryMediaById(long lAutoId);
		list<CBinderEntry>	GetQuarternaryMediaByTertiaryId(long lTertiaryMediaId);

	protected:

		void				CreatePendingFolders();

		void				AddVideos(CTranscripts* pTranscripts);
		void				AddTranscripts(CMedias* pDepositions);

		void				GetSecondaries(CMultipage* pMultipage);
		void				GetShowItems(CShow* pShow, long lSecondary);
		void				GetDesignations(CPlaylist* pPlaylist);
		void				GetLinks(CTertiary* pTertiary, long m_lSceneId);
		void				GetExtents(CVideo* pVideo);
		void				GetExtents(CTertiary* pTertiary);
		void				GetExtents(CSecondary* pSecondary);
		void				GetPrimaryFolder(CMultipage* pMultipage, CString& rFolder);
		void				GetMediaFolder(short sNetMediaType, CString& rFolder);
		void				SetPathDefaults();
		void				GetDocumentsSourceFolder();
		void				CopyToDocument(CMedia* pDocument);
		BOOL				CopyFile(LPCSTR lpSource, LPCSTR lpTarget);
		BOOL				GetFilename600(CTertiary* pTertiary, CString& rFilename);
		BOOL				GetFilename601(CTertiary* pTertiary, CString& rFilename);
		BOOL				OpenXmlCaseOptions();
		BOOL				GetXmlCasePaths();
		BOOL				GetXmlCasePath(int iIndex, LPCSTR lspzXPathMap, LPCSTR lpszName, BOOL bShowError);
		BOOL				GetXmlMachine();
		BOOL				GetXmlGlobal();
		BOOL				GetXmlAliases();
		BOOL				GetXmlAlias(CIXMLDOMNode* pXmlNode);
		BOOL				GetAliasedPath(long lAliasId, LPCSTR lpszRelativePath, CString& rPath);
		BOOL				GetText(CDesignation* pDesignation);
		BOOL				GetTextFlags(CDesignation* pDesignation, CIXMLDOMNode* pXmlNode);
		BOOL				GetSegments(CDeposition* pDeposition);
		int					LoadMedia();
		int					LoadVersion();
		int					OpenRecordsets();
		long				PLToPage(long lPL);
		long				PLToLine(long lPL);
		CDesignation*		GetDesignation(long lTertiaryId, long lPlaybackOrder);
		CDesignation*		GetDesignation(LPCSTR lpszPST, long lSceneId, long lPlaybackOrder);
		CDesignation*		ConvertToDesignation(CDeposition* pDeposition, CSecondary* pSegment);
		CIXMLDOMDocument*	CreateXmlDocument();
		COLORREF			GetHighlighterColor(long lHighlighter);
		LPSTR				StripFileExtension(LPSTR lpszFilename);

	private:
		list<CBinderEntry>  FillAndGetBinderEntry(CBinderEntry::TableType pTableType);
};

#endif // !defined(__DBNET_H__)
