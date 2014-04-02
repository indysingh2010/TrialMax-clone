//==============================================================================
//
// File Name:	xmltreat.h
//
// Description:	This file contains the declarations of the CXmlMedia, 
//				CXmlMediaTree, and CXmlMediaTrees classes. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2001
//
//==============================================================================
//	Date		Revision    Description
//	05-05-00	1.00		Original Release
//==============================================================================
#if !defined(__XMLMEDIA_H__)
#define __XMLMEDIA_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <xmlpage.h>
#include <mediactl.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	XML attribute tags
#define TMXML_MEDIATREE_NAME		"name"
#define TMXML_MEDIA_MEDIAID			"mediaId"
#define TMXML_MEDIA_TITLE			"title"

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CXmlMedia;
class CXmlMediaTree;
class CXmlMediaTrees;

//	This class manages information associated with XML media elements
class CXmlMedia : public CObject, public CMediaCtrlItem
{
	private:

	public:

		CXmlPages			m_Pages;
		CXmlMediaTree*		m_pXmlMediaTree;
		CString				m_strId;
		CString				m_strTitle;

							CXmlMedia(CXmlMediaTree* pTree = 0);
		virtual			   ~CXmlMedia();

		BOOL				operator < (const CXmlMedia& Compare);
		BOOL				operator == (const CXmlMedia& Compare);
	
		BOOL				SetAttribute(LPCSTR lpAttribute, LPCSTR lpValue);
		BOOL				SetAttributes(const CXmlMedia& rSource);
		CXmlPage*			Find(LPCSTR lpId);
		CXmlMedia*			GetDuplicate();
		long				GetFileCount();

	protected:

};

//	Objects of this class are used to manage a list of CXmlMedia objects
class CXmlMediaTree : public CObList, public CMediaCtrlItem 
{
	private:

	public:

		CString				m_strName;

							CXmlMediaTree();
		virtual			   ~CXmlMediaTree();

		void				Add(CXmlMedia* pMedia);
		void				Flush(BOOL bDelete);
		void				Remove(CXmlMedia* pMedia, BOOL bDelete);
		POSITION			Find(CXmlMedia* pMedia);
		CXmlMedia*			Find(LPCSTR lpId);
		CXmlPage*			FindPage(LPCSTR lpId);
		CXmlMedia*			SetPosition(CXmlMedia* pMedia);
		CXmlMediaTree*		GetDuplicate();
		BOOL				IsFirst(CXmlMedia* pMedia);
		BOOL				IsLast(CXmlMedia* pMedia);
		BOOL				SetAttribute(LPCSTR lpAttribute, LPCSTR lpValue);
		BOOL				SetAttributes(const CXmlMediaTree& rSource);
		long				GetFileCount();

		BOOL				operator < (const CXmlMediaTree& Compare);
		BOOL				operator == (const CXmlMediaTree& Compare);
	
		//	List iteration members
		CXmlMedia*			First();
		CXmlMedia*			Last();
		CXmlMedia*			Next();
		CXmlMedia*			Prev();

	protected:

		POSITION			m_NextPos;
		POSITION			m_PrevPos;

};

//	Objects of this class are used to manage a list of CXmlMedia objects
class CXmlMediaTrees : public CObList
{
	private:

	public:

							CXmlMediaTrees();
		virtual			   ~CXmlMediaTrees();

		void				Add(CXmlMediaTree* pTree);
		void				Flush(BOOL bDelete);
		void				Remove(CXmlMediaTree* pTree, BOOL bDelete);
		POSITION			Find(CXmlMediaTree* pTree);
		CXmlMediaTree*		Find(LPCSTR lpName);
		CXmlMediaTree*		SetPosition(CXmlMediaTree* pTree);
		BOOL				IsFirst(CXmlMediaTree* pTree);
		BOOL				IsLast(CXmlMediaTree* pTree);

		//	List iteration members
		CXmlMediaTree*		First();
		CXmlMediaTree*		Last();
		CXmlMediaTree*		Next();
		CXmlMediaTree*		Prev();

	protected:

		POSITION			m_NextPos;
		POSITION			m_PrevPos;

};

#endif // !defined(__XMLMEDIA_H__)
