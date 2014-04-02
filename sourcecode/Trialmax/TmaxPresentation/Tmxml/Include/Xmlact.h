//==============================================================================
//
// File Name:	xmlact.h
//
// Description:	This file contains the declarations of the CXmlAction and 
//				CXmlActions classes. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2001
//
//==============================================================================
//	Date		Revision    Description
//	05-05-01	1.00		Original Release
//==============================================================================
#if !defined(__XMLACT_H__)
#define __XMLACT_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	XML attribute tags
#define TMXML_ACTION_COMMAND		"command"
#define TMXML_ACTION_TREE			"tree"
#define TMXML_ACTION_PAGE			"page"
#define TMXML_ACTION_ONCOMPLETE		"onComplete"

//	XML Action command identifiers
#define TMXML_ACTION_COMMAND_VIEW	"view"
#define TMXML_ACTION_COMMAND_PRINT	"print"

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	This class manages information associated with XML action descriptors
class CXmlAction : public CObject
{
	private:

	public:

		CString				m_strCommand;
		CString				m_strTree;
		CString				m_strPageId;
		CString				m_strOnComplete;

							CXmlAction();
		virtual			   ~CXmlAction();

		BOOL				operator < (const CXmlAction& Compare);
		BOOL				operator == (const CXmlAction& Compare);
	
		BOOL				SetAttribute(LPCSTR lpAttribute, LPCSTR lpValue);

	protected:

};

//	Objects of this class are used to manage a list of CXmlAction objects
class CXmlActions : public CObList
{
	private:

	public:

							CXmlActions();
		virtual			   ~CXmlActions();

		void				Add(CXmlAction* pAction);
		void				Flush(BOOL bDelete);
		void				Remove(CXmlAction* pAction, BOOL bDelete);
		POSITION			Find(CXmlAction* pAction);

		BOOL				IsLast(CXmlAction* pAction);
		BOOL				IsFirst(CXmlAction* pAction);

		//	List iteration members
		CXmlAction*			First();
		CXmlAction*			Last();
		CXmlAction*			Next();
		CXmlAction*			Prev();

	protected:

		POSITION			m_NextPos;
		POSITION			m_PrevPos;

};

#endif // !defined(__XMLACT_H__)
