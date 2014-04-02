//==============================================================================
//
// File Name:	playlist.h
//
// Description:	This file contains the declaration of the CPlaylist and 
//				CPlaylists classes.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-11-00	1.00		Original Release
//==============================================================================
#if !defined(__PLAYLIST_H__)
#define __PLAYLIST_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <media.h>
#include <video.h>
#include <designat.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	A playlist contains a sorted list of designations that should be played one
//	after another.  
class CPlaylist : public CMedia
{
	private:

		CVideos*		m_pVideos;
		BOOL			m_bIsRecording;
		BOOL			m_bIsDeposition;
		BOOL			m_bOwnsVideos;
		BOOL			m_bHasText;

	public:

		CDesignations	m_Designations;

						CPlaylist(CMedia* pMedia = 0);
					   ~CPlaylist();

		CVideos*		GetVideos(){ return m_pVideos; }
		void			SetVideos(CVideos* pVideos){ m_pVideos = pVideos; }

		BOOL			GetIsDeposition(){ return m_bIsDeposition; }
		void			SetIsDeposition(BOOL bIsDeposition){ m_bIsDeposition = bIsDeposition; }
		
		BOOL			GetIsRecording(){ return m_bIsRecording; }
		void			SetIsRecording(BOOL bIsRecording){ m_bIsRecording = bIsRecording; }
		
		BOOL			GetOwnsVideos(){ return m_bOwnsVideos; }
		void			SetOwnsVideos(BOOL bOwnsVideos){ m_bOwnsVideos = bOwnsVideos; }

		void			SetHasText(BOOL bHasText){ m_bHasText = bHasText; }
		BOOL			GetHasText(){ return m_bHasText; }
		BOOL			ScrollingTextEnabled();

		BOOL			GetVideoFile(long lFileId, CString& rFilespec);

		//	Designations list iteration routines
		CDesignation*	GetFirstDesignation();
		CDesignation*	GetLastDesignation();
		CDesignation*	GetNextDesignation();
		CDesignation*	GetPrevDesignation();
		CDesignation*	GetNextDesignation(CDesignation* pDesignation);
		CDesignation*	GetPrevDesignation(CDesignation* pDesignation);

		CDesignation*	GetDesignationFromOrder(long lOrder, BOOL bForward);
		CDesignation*	GetDesignationFromId(long lId);
		CDesignation*	GetFirstInRange(int iPage, int iLine, long lTranscript);
		double			GetTotalTime();
		double			GetTime(long lStartOrder, double dStartPosition,
								long lStopOrder, double dStopPosition);
		int				GetDesignationCount();

		BOOL			operator < (const CPlaylist& Compare);
		BOOL			operator == (const CPlaylist& Compare);
	
	protected:

};

//	Objects of this class are used to manage a list of CPlaylist objects
class CPlaylists : public CObList
{
	private:

	public:

							CPlaylists();
		virtual			   ~CPlaylists();

		BOOL				Add(CPlaylist* pPlaylist);
		void				Flush(BOOL bDelete);
		void				Remove(CPlaylist* pPlaylist, BOOL bDelete);
		POSITION			Find(CPlaylist* pPlaylist);
		CPlaylist*			Find(LPCSTR lpId);
		CPlaylist*			Find(long lId);

		//	List iteration members
		CPlaylist*			First();
		CPlaylist*			Last();
		CPlaylist*			Next();
		CPlaylist*			Prev();

	protected:

		POSITION			m_NextPos;
		POSITION			m_PrevPos;

};

#endif // !defined(__PLAYLIST_H__)
