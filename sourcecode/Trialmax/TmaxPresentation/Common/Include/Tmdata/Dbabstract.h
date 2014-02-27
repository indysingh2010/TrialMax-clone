//==============================================================================
//
// File Name:	dbabstract.h
//
// Description:	This file contains the declaration of the CDBAbstract class
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2003
//
//==============================================================================
//	Date		Revision    Description
//	12-26-2003	1.00		Original Release
//==============================================================================
#if !defined(__DBABSTRACT_H__)
#define __DBABSTRACT_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <handler.h>
#include <media.h>
#include <multipg.h>
#include <show.h>
#include <playlist.h>
#include <transcpt.h>
#include <video.h>
#include <deposit.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

#define CLOSE_RECORDSET(x)				if(x != 0)					\
										{							\
											if(x->IsOpen())			\
												x->Close();			\
											delete x;				\
											x = 0;					\
										}							\

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CDBAbstract : public CObject
{
	protected:

		CMedias					m_Playlists;
		CMedias					m_Multipages;
		CTranscripts			m_Transcripts;
		CVideos					m_Videos;


		CErrorHandler			m_Errors;
		CDaoDatabase*			m_pDatabase;
			
		int						m_iLastError;
		int						m_iMajorVer;
		int						m_iMinorVer;

		CString					m_strFilespec;
		CString					m_strRootFolder;
		CString					m_strFilename;
		CString					m_strRevision;
		CString					m_strCreator;

		CString					m_strRootPendingFolder;
		CString					m_strTreatmentPendingFolder;

		//	These members provided for .NET support
		long					m_lScriptId;
		long					m_lSceneId;

		//	These members are used for debugging
		BOOL					m_bCreateDocuments;
	
	public:

								CDBAbstract();
							   ~CDBAbstract();

		//	Public access
		virtual int				GetLastError(){ return m_iLastError; }
		virtual LPCSTR			GetLastErrorMsg(){ return m_Errors.GetMessage(); }
		virtual LPCSTR			GetFilename(){ return m_strFilename; }
		virtual LPCSTR			GetFilespec(){ return m_strFilespec; }
		virtual LPCSTR			GetRootFolder(){ return m_strRootFolder; }
		virtual LPCSTR			GetRevision(){ return m_strRevision; }
		virtual LPCSTR			GetCreator(){ return m_strCreator; }

		virtual CTranscripts*	GetTranscripts(){ return &m_Transcripts; }
		virtual CMedias&		GetPlaylistMedia(){ return m_Playlists; }
		virtual CMedias&		GetMultipageMedia(){ return m_Multipages; }
		virtual CVideos&		GetVideos(){ return m_Videos; }

		virtual void			GetErrorHandler(BOOL* pbEnabled, HWND* phParent);
		virtual int				GetMajorVer(){ return m_iMajorVer; }
		virtual int				GetMinorVer(){ return m_iMinorVer; }

		virtual void			SetErrorHandler(BOOL bEnable, HWND hParent);
		virtual void			SetCreateDocuments(BOOL bCreate){ m_bCreateDocuments = bCreate; }
		virtual BOOL			IsNETDatabase(){ return FALSE; }
		virtual BOOL			GetCreateDocuments(){ return m_bCreateDocuments; }

		//	File specifications
		virtual void			GetFilename(CMultipage* pMultipage, CSecondary* pPage, CString& rFilename){};
		virtual void			GetFilename(CTertiary* pTertiary, CString& rFilename){};
		virtual void			GetFilename(CTranscript* pTranscript, CString& rFilename){};
		virtual void			GetOverlayFilename(CDesignation* pDesignation){};
		virtual void			GetSaveZapFileSpec(CMultipage* pMultipage, CSecondary* pPage, CString& rFileSpec){};
		virtual void			GetSaveZapFileSpecs(CMultipage* pTLMultipage, CSecondary* pTLPage, CString& rTLFileSpec,
												    CMultipage* pBRMultipage, CSecondary* pBRPage, CString& rBRFileSpec,
													DWORD dwFlags = 0){};
		virtual LPCSTR			GetSaveZapFolder(){ return m_strTreatmentPendingFolder; }

		//	Database status checks
		virtual BOOL			IsOpen();
		virtual BOOL			IsFirstPlaylist(CMedia* pMedia){ return m_Playlists.IsFirst(pMedia); }
		virtual BOOL			IsLastPlaylist(CMedia* pMedia){ return m_Playlists.IsLast(pMedia); }
		virtual BOOL			IsFirstMultipage(CMedia* pMedia){ return m_Multipages.IsFirst(pMedia); }
		virtual BOOL			IsLastMultipage(CMedia* pMedia){ return m_Multipages.IsLast(pMedia); }

		//	Database operations
		virtual void			Close();
		virtual int				Open(LPCSTR lpFolder, LPCSTR lpFilename = 0){ return FALSE; }
		virtual int				OpenRecordset(CDaoRecordset* pSet, LPCSTR lpTable,
										      BOOL bMustExist = TRUE);
		//	Media 
		virtual CMedia*			GetMedia(LPCSTR lpMediaId);
		virtual CMedia*			GetMedia(long lId);
		virtual CMedia*			GetNextPlaylist(CMedia* pMedia){ return m_Playlists.FindNext(pMedia); }
		virtual CMedia*			GetPrevPlaylist(CMedia* pMedia){ return m_Playlists.FindPrev(pMedia); }
		virtual CMedia*			GetNextMultipage(CMedia* pMedia){ return m_Multipages.FindNext(pMedia); }
		virtual CMedia*			GetPrevMultipage(CMedia* pMedia){ return m_Multipages.FindPrev(pMedia); }

		//	Multipage
		virtual CMultipage*		GetMultipage(CMedia* pMedia);
		virtual void			GetMultipageFolder(CMultipage* pMultipage, 
											       CString& rFolder){};
		
		//	Treatments
		virtual BOOL			GetTertiaries(CSecondary* pSecondary){ return FALSE; };
		virtual BOOL			AddTreatment(CSecondary* pSecondary, long lTertiaryId, 
											 long lDisplayOrder, long lBarcodeId,
											 LPCSTR lpszFilename){ return FALSE; };
				
		//	Playlists
		virtual CPlaylist*		GetPlaylist(CMedia* pMedia);
		virtual CPlaylist*		ConvertToPlaylist(CMultipage* pMultipage);
		virtual CPlaylist*		ConvertToPlaylist(CDeposition* pDeposition);

		//	Depositions
		virtual CDeposition*	GetDeposition(CMedia* pMedia);

		//	Custom Shows
		virtual CShow*			GetShow(CMedia* pMedia, long lSecondary);

		//	Barcode lookup
		virtual BOOL			MapBarcode(LPCSTR lpForeign, LPSTR lpBarcode){ return FALSE; }
		virtual BOOL			GetBarcode(LPCSTR lpszUniqueId, CString& rBarcode){ return FALSE; }

		//	.NET Script support
		virtual long			GetScriptId(){ return m_lScriptId; }
		virtual void			SetScriptId(long lId){ m_lScriptId = lId; }
		virtual long			GetSceneId(){ return m_lSceneId; }
		virtual void			SetSceneId(long lId){ m_lSceneId = lId; }
		virtual CPlaylist*		GetPlaylistFromScene(long lSceneId){ return NULL; };
		virtual CTertiary*		GetClip(CSecondary* pSegment, long lTertiary, long lSceneId){ return NULL; }

	protected:

		virtual void			CreatePendingFolders();
		virtual void			GetSecondaries(CMultipage* pMultipage){};
		virtual void			GetShowItems(CShow* pShow, long lSecondary){};
		virtual void			GetDesignations(CPlaylist* pPlaylist){};
		
		virtual int				HandleError(int iError, LPCSTR lpMessage);
		virtual int				HandleError(int iError, UINT uString,
										    LPCSTR lpArg1 = 0, LPCSTR lpArg2 = 0);
		virtual BOOL			FindFile(CString& rFileSpec);
		virtual POSITION		Find(CMedia* pMedia);
};

#endif // !defined(__DBABSTRACT_H__)
