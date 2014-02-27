//==============================================================================
//
// File Name:	video.h
//
// Description:	This file contains the declarations of the CVideo and CVideos 
//				classes. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	04-04-00	1.00		Original Release
//==============================================================================
#if !defined(__VIDEO_H__)
#define __VIDEO_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	This class manages information associated with image media records
class CVideo : public CObject
{
	private:

	public:

		long				m_lVideoFileId;
		long				m_lTranscriptId;
		long				m_lUnitType;
		long				m_lBeginNum;
		long				m_lEndNum;
		long				m_lMinSelStart;
		long				m_lMaxSelStart;
		CString				m_strRelativePath;
		CString				m_strRootOverride;
		CString				m_strFilename;
		BOOL				m_bBeginTuned;
		BOOL				m_bEndTuned;

		//	These members were added for .NET
		long				m_lPrimaryMediaId;
		long				m_lBarcodeId;
		long				m_lChildren;
		long				m_lAttributes;
		long				m_lAliasId;
		double				m_dStartPosition;
		double				m_dStopPosition;

							CVideo();
		virtual			   ~CVideo();

		BOOL				operator < (const CVideo& Compare);
		BOOL				operator == (const CVideo& Compare);
	
	protected:

};

//	Objects of this class are used to manage a list of CVideo objects
class CVideos : public CObList
{
	private:

		CString				m_strRootFolder;

	public:

							CVideos();
		virtual			   ~CVideos();

		void				Add(CVideo* pVideo);
		void				Flush(BOOL bDelete);
		void				Remove(CVideo* pVideo, BOOL bDelete);
		void				SetRootFolder(LPCSTR lpFolder);
		BOOL				GetVideoFile(long lVideoFileId, CString& rFilespec);
		LPCSTR				GetRootFolder(){ return m_strRootFolder; }
		POSITION			Find(CVideo* pVideo);
		CVideo*				Find(long lVideoFileId);

		//	List iteration members
		CVideo*				First();
		CVideo*				Last();
		CVideo*				Next();
		CVideo*				Prev();

	protected:

		POSITION			m_NextPos;
		POSITION			m_PrevPos;

};

#endif // !defined(__VIDEO_H__)
