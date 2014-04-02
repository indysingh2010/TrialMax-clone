//==============================================================================
//
// File Name:	BinderEntry.h
//
// Description:	This file contains member functions of the CBinderEntry
//				
//
// See Also:	BinderEntry.cpp
//
// Copyright	Tenpearls LLC 2014
//
//==============================================================================
//	Date		Revision    Description
//	24-03-14	1.00		Original Release
//==============================================================================


#if !defined(__BINDERENTRY_H__)
#define __BINDERENTRY_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <link.h>
#include <dbdefs.h>
#include <afxdao.h>
//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------


//	This class manages information associated with binder entry records
class CBinderEntry
{
	private:

	public:

		enum	TableType { Binder = 1, Primary = 2, Secondary = 3, Tertiary = 4, Quaternary = 5 };
		//enum	MediaType { 1,2,3,4,5,6,7,8,9,10,11,12,13 };

		long	m_AutoId;
		long	m_ParentId;
		CString	m_Path;
		long	m_Children;
		long	m_Attributes;
		CString	m_Name;
		CString	m_Description;
		long	m_DisplayOrder;
		long	m_CreatedBy;
		COleDateTime	m_CreatedOn;
		long	m_ModifiedBy;
		COleDateTime m_ModifiedOn;
		CString	m_SpareText;
		long	m_SpareNumber;
		long	m_BarcodeId;
		long	m_MediaType;
		TableType	m_TableType;
						CBinderEntry();
		virtual		   ~CBinderEntry();
		
	protected:

		
};

//	Objects of this class are used to manage a list of CBinderEntry objects
class CBinderEntries : public CObList
{
	private:

	public:

						CBinderEntries();
		virtual		   ~CBinderEntries();

		void			Add(CBinderEntry* pBinderEntry, BOOL bSorted);
		void			Flush(BOOL bDelete);
		void			Remove(CBinderEntry* pBinderEntry, BOOL bDelete);
		POSITION		Find(CBinderEntry* pBinderEntry);
		CBinderEntry*		FindByBarcodeId(long lId);
		CBinderEntry*		FindByDatabaseId(long lId);
		CBinderEntry*		FindByOrder(long lOrder);
		
	protected:


};

#endif // !defined(__BINDERENTRY_H__)
