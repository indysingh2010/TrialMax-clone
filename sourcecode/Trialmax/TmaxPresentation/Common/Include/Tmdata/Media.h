//==============================================================================
//
// File Name:	media.h
//
// Description:	This file contains the declarations of the CMedia and
//				CMedias classes. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-03-00	1.00		Original Release
//==============================================================================
#if !defined(__MEDIA_H__)
#define __MEDIA_H__

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

//	This class is used as a base class for all media data types
class CMedia : public CObject
{
	private:

	public:

		long				m_lPrimaryId;
		long				m_lPlayerType;
		long				m_lFlags;
		CString				m_strMediaId;
		CString				m_strName;
		CString				m_strRelativePath;
		CString				m_strFilename;

		//	These members added for .NET support
		long				m_lChildren;
		long				m_lAttributes;
		long				m_lAliasId;
		short				m_sMediaType;
		BOOL				m_bLinked;
		CString				m_strAltBarcode;
		CString				m_strDescription;
		CString				m_strExhibit;

							CMedia(CMedia* pMedia = 0);
		virtual			   ~CMedia();

		int					Compare(CMedia* pCompare);
		int					Compare(CString& rString1, CString& rString2, BOOL bIgnoreCase);

		BOOL				operator < (const CMedia& Compare);
		BOOL				operator > (const CMedia& Compare);
		BOOL				operator == (const CMedia& Compare);
	
	protected:

		void				Extract(char** ppString, char* pExtracted, int iMaxExtracted);
};

//	Objects of this class are used to manage a list of CMedia objects
class CMedias : public CObList
{
	private:

	public:

							CMedias(BOOL bKeepSorted = FALSE);
		virtual			   ~CMedias();

		virtual BOOL		Add(CMedia* pMedia);
		virtual void		Flush(BOOL bDelete);
		virtual void		Remove(CMedia* pMedia, BOOL bDelete);
		virtual POSITION	Find(CMedia* pMedia);
		virtual CMedia*		Find(LPCSTR lpId);
		virtual CMedia*		Find(long lId);

		virtual BOOL		IsLast(CMedia* pMedia);
		virtual BOOL		IsFirst(CMedia* pMedia);

		virtual BOOL		GetKeepSorted() { return m_bKeepSorted; }
		virtual void		SetKeepSorted(BOOL bKeepSorted);

		virtual void		Sort();
		virtual void		SetExtents();

		static int __cdecl 	Compare(const void* pObj1, const void* pObj2);

		//	List iteration members
		virtual	CMedia*		First();
		virtual	CMedia*		Last();
		virtual	CMedia*		Next();
		virtual	CMedia*		Prev();

		//	List access by position
		virtual CMedia*		FindNext(CMedia* pMedia);
		virtual CMedia*		FindPrev(CMedia* pMedia);
		virtual CMedia*		FindFirst();
		virtual CMedia*		FindLast();

	protected:

		POSITION			m_NextPos;
		POSITION			m_PrevPos;	
		BOOL				m_bKeepSorted;
		CMedia*				m_pFirst;
		CMedia*				m_pLast;

};

#endif // !defined(__MEDIA_H__)
