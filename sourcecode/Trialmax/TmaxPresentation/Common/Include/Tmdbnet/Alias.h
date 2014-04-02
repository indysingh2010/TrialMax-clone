//==============================================================================
//
// File Name:	alias.h
//
// Description:	This file contains the declarations of the CAlias and
//				CAliass classes. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2004
//
//==============================================================================
//	Date		Revision    Description
//	01-31-04	1.00		Original Release
//==============================================================================
#if !defined(__ALIAS_H__)
#define __ALIAS_H__

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

//	This class manages information associated with drive / server alias
class CAlias : public CObject
{
	private:

	public:

		long				m_lId;
		CString				m_strCurrent;
		CString				m_strPrevious;
		CString				m_strOriginal;


							CAlias();
		virtual			   ~CAlias();

		BOOL				operator < (const CAlias& Compare);
		BOOL				operator == (const CAlias& Compare);

	protected:

};

//	Objects of this class are used to manage a list of CAlias objects
class CAliases : public CObList
{
	private:

	public:

							CAliases();
		virtual			   ~CAliases();

		void				Add(CAlias* pAlias, BOOL bSorted);
		void				Flush(BOOL bDelete);
		void				Remove(CAlias* pAlias, BOOL bDelete);
		POSITION			Find(CAlias* pAlias);
		CAlias*				Find(LPCSTR lpszCurrent);
		CAlias*				Find(long lId);

		//	List iteration members
		CAlias*				First();
		CAlias*				Last();
		CAlias*				Next();
		CAlias*				Prev();

	protected:

		POSITION			m_NextPos;
		POSITION			m_PrevPos;

};

#endif // !defined(__ALIAS_H__)
