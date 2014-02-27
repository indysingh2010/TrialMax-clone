//==============================================================================
//
// File Name:	show.h
//
// Description:	This file contains the declarations of the CShow and
//				CShows classes. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-03-00	1.00		Original Release
//==============================================================================
#if !defined(__SHOW_H__)
#define __SHOW_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <media.h>
#include <showitem.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	This class manages information associated with show media records
class CShow : public CMedia
{
	private:

	public:
		
		CShowItems			m_Items;

							CShow(CMedia* pMedia = 0);
		virtual			   ~CShow();

		CShowItem*			FindByBarcodeId(long lId);
		CShowItem*			FindByDatabaseId(long lId);
		CShowItem*			FindByOrder(long lId);

		BOOL				IsLast(CShowItem* pItem){ return m_Items.IsLast(pItem); }
		BOOL				IsFirst(CShowItem* pItem){ return m_Items.IsFirst(pItem); }

	protected:

};

//	Objects of this class are used to manage a list of CShow objects
class CShows : public CObList
{
	private:

	public:

							CShows();
		virtual			   ~CShows();

		BOOL				Add(CShow* pShow);
		void				Flush(BOOL bDelete);
		void				Remove(CShow* pShow, BOOL bDelete);
		POSITION			Find(CShow* pShow);
		CShow*				Find(LPCSTR lpId);
		CShow*				Find(long lId);

		//	List iteration members
		CShow*				First();
		CShow*				Last();
		CShow*				Next();
		CShow*				Prev();

	protected:

		POSITION			m_NextPos;
		POSITION			m_PrevPos;

};

#endif // !defined(__SHOW_H__)
