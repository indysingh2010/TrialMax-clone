//==============================================================================
//
// File Name:	showitem.h
//
// Description:	This file contains the declarations of the CShowItem and
//				CShowItems classes. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	04-27-00	1.00		Original Release
//==============================================================================
#if !defined(__SHOWITEM_H__)
#define __SHOWITEM_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <secondary.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	This class manages information associated with show media records
class CShowItem : public CSecondary
{
	private:

	public:

		CString				m_strItemBarcode;

		//	These members were added for .NET
		short				m_sSourceMediaType;
		CString				m_strSourcePST;
		BOOL				m_bAutoTransition;
		BOOL				m_bStaticScene;
		BOOL				m_bTransitioned;
		long				m_lTransitionPeriod;
		unsigned long		m_ulGoToNext;

							CShowItem();
		virtual			   ~CShowItem();

		BOOL				operator < (const CShowItem& Compare);
		BOOL				operator == (const CShowItem& Compare);

	protected:

};

//	Objects of this class are used to manage a list of CShowItem objects
class CShowItems : public CObList
{
	private:

	public:

							CShowItems();
		virtual			   ~CShowItems();

		void				Add(CShowItem* pShowItem, BOOL bSorted);
		void				Flush(BOOL bDelete);
		void				Remove(CShowItem* pShowItem, BOOL bDelete);
		POSITION			Find(CShowItem* pShowItem);
		CShowItem*			Find(LPCSTR lpItemBarcode);
		CShowItem*			Find(long lId, BOOL bUsePlayback);

		BOOL				IsFirst(CShowItem* pShowItem);
		BOOL				IsLast(CShowItem* pShowItem);

		//	List iteration members
		CShowItem*			First();
		CShowItem*			Last();
		CShowItem*			Next();
		CShowItem*			Prev();

	protected:

		POSITION			m_NextPos;
		POSITION			m_PrevPos;

};

#endif // !defined(__SHOWITEM_H__)
