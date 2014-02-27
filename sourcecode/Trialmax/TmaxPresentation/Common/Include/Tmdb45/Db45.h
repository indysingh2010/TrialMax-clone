//==============================================================================
//
// File Name:	db45.h
//
// Description:	This file contains the declaration of the CDB45 class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	08-04-97	1.00		Original Release
//	01-25-98	2.00		Added Barcode Manager and related members
//==============================================================================
#if !defined(__DB45_H__)
#define __DB45_H__

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

#include <mediaset.h>
#include <verset.h>
#include <pageset.h>
#include <treatset.h>
#include <videoset.h>
#include <desgset.h>
#include <linkset.h>
#include <textset.h>
#include <transet.h>
#include <showset.h>
#include <bcmapset.h>
#include <colorset.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Default filenames
#define DEFAULT_DB45_INI_FILENAME		"Case.ini"

//	Ini section names and line identifiers
#define CASEINI_PATHS_SECTION			"PATHS"
#define CASEINI_DOCUMENTPATH_LINE		"DocumentsPathOverride"
#define CASEINI_ANIMATIONPATH_LINE		"MoviesPathOverride"
#define CASEINI_POWERPOINTPATH_LINE		"PowerPointsPathOverride"
#define CASEINI_TREATMENTPATH_LINE		"TreatmentsPathOverride"
#define CASEINI_TRANSCRIPTPATH_LINE		"TranscriptsPathOverride"
#define CASEINI_VIDEOPATH_LINE			"VideoFilesPathOverride"
#define CASEINI_OVERLAYPATH_LINE		"OverlaysPathOverride"
#define CASEINI_PENDINGTREATMENTS_LINE	"PendingTreatmentsPathOverride"

//	Default subfolders
#define DEFAULT_DOCUMENT_FOLDER			"Documents\\"
#define DEFAULT_ANIMATION_FOLDER		"Movies\\"
#define DEFAULT_POWERPOINT_FOLDER		"PowerPoints\\"
#define DEFAULT_TREATMENT_FOLDER		"Treatments\\"
#define DEFAULT_TRANSCRIPT_FOLDER		"Transcripts\\"
#define DEFAULT_VIDEO_FOLDER			"VideoFiles\\"
#define DEFAULT_OVERLAY_FOLDER			"Overlays\\"
#define DEFAULT_PENDINGTREATMENT_FOLDER	"Pending\\Treatments\\"

//	Database table names
#define TMDBASE_MEDIA_TABLE				"Media"
#define TMDBASE_VERSION_TABLE			"Version"
#define TMDBASE_PAGES_TABLE				"Pages"
#define TMDBASE_TREATMENTS_TABLE		"Treatments"
#define TMDBASE_VIDEO_TABLE				"VideoFiles"
#define TMDBASE_DESIGNATIONS_TABLE		"Designations"
#define TMDBASE_LINKS_TABLE				"Links"
#define TMDBASE_TEXT_TABLE				"DesignationLines"
#define TMDBASE_TRANSCRIPTS_TABLE		"Transcripts"
#define TMDBASE_SHOWITEMS_TABLE			"CustomShowItems"
#define TMDBASE_BARCODEMAP_TABLE		"BarcodeMap"
#define TMDBASE_COLORS_TABLE			"Colors"

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CDB45 : public CDBAbstract
{
	private:

		CTMIni				m_CaseIni;
		int					m_iShareVer;
			
		CMediaSet*			m_pMediaSet;
		CVersionSet*		m_pVersionSet;
		CPageSet*			m_pPageSet;
		CTreatmentSet*		m_pTreatmentSet;
		CVideoSet*			m_pVideoSet;
		CDesignationSet*	m_pDesignationSet;
		CLinkSet*			m_pLinkSet;
		CTextSet*			m_pTextSet;
		CTranscriptSet*		m_pTranscriptSet;
		CShowItemSet*		m_pShowItemSet;
		CBarcodeMapSet*		m_pBarcodeMapSet;
		CColorSet*			m_pColorSet;

		CString				m_strDocumentFolder;
		CString				m_strPowerPointFolder;
		CString				m_strAnimationFolder;
		CString				m_strTranscriptFolder;
		CString				m_strTreatmentFolder;
		CString				m_strVideoFolder;
		CString				m_strOverlayFolder;
	
	public:


							CDB45();
						   ~CDB45();

		LPCSTR				GetRootPendingFolder(){ return m_strRootPendingFolder; }
		LPCSTR				GetDocumentFolder(){ return m_strDocumentFolder; }
		LPCSTR				GetPowerPointFolder(){ return m_strPowerPointFolder; }
		LPCSTR				GetAnimationFolder(){ return m_strAnimationFolder; }
		LPCSTR				GetTranscriptFolder(){ return m_strTranscriptFolder; }
		LPCSTR				GetTreatmentFolder(){ return m_strTreatmentFolder; }
		LPCSTR				GetTreatmentPendingFolder(){ return m_strTreatmentPendingFolder; }
		LPCSTR				GetVideoFolder(){ return m_strVideoFolder; }
		LPCSTR				GetOverlayFolder(){ return m_strOverlayFolder; }
		
		//	File specifications
		void				GetFilename(CMultipage* pMultipage, CSecondary* pSecondary, CString& rFilename);
		void				GetFilename(CTertiary* pTreatment, CString& rFilename);
		void				GetOverlayFilename(CDesignation* pDesignation);
		void				GetMultipageFolder(CMultipage* pMultipage, CString& rFolder);
		void				GetSaveZapFileSpec(CMultipage* pMultipage, CSecondary* pPage, CString& rFileSpec);

		void				Close();
		int					Open(LPCSTR lpFolder, LPCSTR lpFilename = 0);
		int					GetShareVer(){ return m_iShareVer; }
		
		BOOL				GetTertiaries(CSecondary* pSecondary);
		CPlaylist*			ConvertToPlaylist(CMultipage* pMultipage);
		BOOL				MapBarcode(LPCSTR lpForeign, LPSTR lpBarcode);

	protected:

		void				SetMediaPaths();
		void				GetSecondaries(CMultipage* pMultipage);
		void				GetShowItems(CShow* pShow, long lSecondary);
		void				GetDesignations(CPlaylist* pPlaylist);
		void				GetLinks(CDesignation* pDesignation);
		BOOL				GetText(CDesignation* pDesignation);
		BOOL				AddVideoOverride();
		BOOL				CheckForPages(CMedia* pMedia);
		int					LoadMedia();
		int					LoadTranscripts();
		int					LoadVideos();
		int					LoadVersion();
		int					OpenRecordsets();
};

#endif // !defined(__DB45_H__)
