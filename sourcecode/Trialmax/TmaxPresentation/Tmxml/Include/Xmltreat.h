//==============================================================================
//
// File Name:	xmltreat.h
//
// Description:	This file contains the declarations of the CXmlTreatment and 
//				CXmlTreatments classes. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2001
//
//==============================================================================
//	Date		Revision    Description
//	05-05-00	1.00		Original Release
//==============================================================================
#if !defined(__XMLTREAT_H__)
#define __XMLTREAT_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <mediactl.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	XML attribute tags
#define TMXML_TREATMENT_SOURCE		"src"
#define TMXML_TREATMENT_ID			"TreatmentID"

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CXmlPage;

//	This class manages information associated with page treatments
class CXmlTreatment : public CObject, public CMediaCtrlItem
{
	private:

	public:

		CXmlPage*			m_pXmlPage;
		CString				m_strSource;
		CString				m_strId;

							CXmlTreatment(CXmlPage* pPage = 0);
		virtual			   ~CXmlTreatment();

		BOOL				operator < (const CXmlTreatment& Compare);
		BOOL				operator == (const CXmlTreatment& Compare);
	
		BOOL				SetAttribute(LPCSTR lpAttribute, LPCSTR lpValue);
		BOOL				SetAttributes(const CXmlTreatment& rSource);

	protected:

};

//	Objects of this class are used to manage a list of CXmlTreatment objects
class CXmlTreatments : public CObList
{
	private:

	public:

							CXmlTreatments();
		virtual			   ~CXmlTreatments();

		void				Add(CXmlTreatment* pTreatment);
		void				Flush(BOOL bDelete);
		void				Remove(CXmlTreatment* pTreatment, BOOL bDelete);
		POSITION			Find(CXmlTreatment* pTreatment);
		BOOL				IsFirst(CXmlTreatment* pTreatment);
		BOOL				IsLast(CXmlTreatment* pTreatment);
		CXmlTreatment*		Find(LPCSTR lpId);
		CXmlTreatment*		SetPosition(CXmlTreatment* pTreatment);

		//	List iteration members
		CXmlTreatment*		First();
		CXmlTreatment*		Last();
		CXmlTreatment*		Next();
		CXmlTreatment*		Prev();

	protected:

		POSITION			m_NextPos;
		POSITION			m_PrevPos;

};

#endif // !defined(__XMLTREAT_H__)
