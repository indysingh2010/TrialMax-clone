//==============================================================================
//
// File Name:	xmlpage.h
//
// Description:	This file contains the declarations of the CXmlPage and 
//				CXmlPages classes. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2001
//
//==============================================================================
//	Date		Revision    Description
//	05-05-01	1.00		Original Release
//==============================================================================
#if !defined(__XMLPAGE_H__)
#define __XMLPAGE_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <xmltreat.h>
#include <mediactl.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	XML attribute tags
#define TMXML_PAGE_PAGEID			"pageId"
#define TMXML_PAGE_SOURCE			"src"
#define TMXML_PAGE_TYPE				"type"
#define TMXML_PAGE_TITLE			"title"
#define TMXML_PAGE_NUMBER			"pageNo"

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CXmlMedia;

//	This class manages information associated with image media records
class CXmlPage : public CObject, public CMediaCtrlItem
{
	private:

	public:

		CXmlTreatments		m_Treatments;
		CXmlMedia*			m_pXmlMedia;
		CString				m_strNumber;
		CString				m_strId;
		CString				m_strType;
		CString				m_strTitle;
		CString				m_strSource;
		long				m_lPosition;

							CXmlPage(CXmlMedia* pMedia = 0);
		virtual			   ~CXmlPage();

		BOOL				operator < (const CXmlPage& Compare);
		BOOL				operator == (const CXmlPage& Compare);
	
		BOOL				SetAttribute(LPCSTR lpAttribute, LPCSTR lpValue);
		BOOL				SetAttributes(const CXmlPage& rSource);
		CXmlPage*			GetDuplicate();

		void				GetDisplayText(CString& rText);
		void				GetDisplayText(LPSTR lpszText, int iMaxLength);

	protected:

};

//	Objects of this class are used to manage a list of CXmlPage objects
class CXmlPages : public CObList
{
	private:

	public:

							CXmlPages();
		virtual			   ~CXmlPages();

		void				Add(CXmlPage* pPage);
		void				Flush(BOOL bDelete);
		void				Remove(CXmlPage* pPage, BOOL bDelete);
		POSITION			Find(CXmlPage* pPage);
		CXmlPage*			Find(LPCSTR lpId);
		CXmlPage*			SetPosition(CXmlPage* pPage);

		BOOL				IsLast(CXmlPage* pPage);
		BOOL				IsFirst(CXmlPage* pPage);

		//	List iteration members
		CXmlPage*			First();
		CXmlPage*			Last();
		CXmlPage*			Next();
		CXmlPage*			Prev();

	protected:

		POSITION			m_NextPos;
		POSITION			m_PrevPos;

};

#endif // !defined(__XMLPAGE_H__)
