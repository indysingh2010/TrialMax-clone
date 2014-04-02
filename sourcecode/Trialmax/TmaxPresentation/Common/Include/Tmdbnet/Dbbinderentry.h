//==============================================================================
//
// File Name:	dbbinderentries.h
//
// Description:	This file contains the declaration of the CDBBinderEntries class
//
// See Also:	
//
// Author:		Muhammad Hussain
//
// Copyright	Tenpearls 2014
//
//==============================================================================
//	Date		Revision    Description
//	24-03-2014	1.00		Original Release
//==============================================================================
#if !defined(__DBBINDERENTRIES_H__)
#define __DBBINDERENTRIES_H__

#if _MSC_VER >= 1000
#pragma once
#endif 


//#include "Dao36.h"
//#include "afxdao.h"

class CDBAbstract;

class CDBBinderEntries : public CDaoRecordset
{
	private:

						DECLARE_DYNAMIC(CDBBinderEntries)

	public:
	
		BOOL			FilterOnId(long lId, CDBAbstract* lpDatabase);
		BOOL			FilterOnParentId(long lParentId, CDBAbstract* lpDatabase);
		BOOL			FilterOnParentIdWithAttribute(long lParentId, CDBAbstract* lpDatabase);
		BOOL			FilterOnMediaId(CString lMediaId, CDBAbstract* lpDatabase);
		BOOL			CDBBinderEntries::GetAll(CDBAbstract* lpDatabase);
						CDBBinderEntries(CDaoDatabase* pDatabase = NULL);
						


	protected:

	public:
		// Field/Param Data
	//{{AFX_FIELD(CDBBinderEntries, CDaoRecordset)
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
		//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDBBinderEntries)

	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL
};

#endif