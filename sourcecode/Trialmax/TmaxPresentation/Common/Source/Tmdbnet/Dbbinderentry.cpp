//==============================================================================
//
// File Name:	dbbinderentries.cpp
//
// Description:	This file contains the implementation of the CDBBinderEntries class
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
#include <stdafx.h>
#include "Dbbinderentry.h"
#include <dao36.h>
#include <dbabstract.h>
#include <Dbnet.h>
#include <dbdefs.h>
#include <db45.h>


#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

IMPLEMENT_DYNAMIC(CDBBinderEntries, CDaoRecordset)


CDBBinderEntries::CDBBinderEntries(CDaoDatabase* pDatabase) : CDaoRecordset(pDatabase)
{
	m_AutoId = 0;
	m_ParentId = 0;
	m_Path = _T("");
	m_Children = 0;
	m_Attributes = 0;
	m_Name = _T("");
	m_Description = _T("");
	m_DisplayOrder = 0;
	m_CreatedBy = 0;
	m_CreatedOn = (DATE)0;
	m_ModifiedBy = 0;
	m_ModifiedOn = (DATE)0;
	m_SpareText = _T("");
	m_SpareNumber = 0;
	m_nDefaultType = dbOpenDynaset;
	m_nFields = 14;
}


void CDBBinderEntries::DoFieldExchange(CDaoFieldExchange* pFX)
{
	//	This code is added to work around a bug in the DAO 3.6 library
	//
	//	See MSDN article Q235507
#if defined _USE_DAO36_

	if(pFX->m_nOperation == CDaoFieldExchange::BindField)
	{
		for (int i=0;i<this->m_nFields;i++)
		{
			LPDAOCOLUMNBINDING pcb = &m_prgDaoColBindInfo[i];
			if (pcb->dwDataType == DAO_CHAR)
			{
				pcb->cbDataOffset = (DWORD)Access2KStringAllocCallback;
			}			
		}
	} 

#endif

	//{{AFX_FIELD_MAP(CDBBinderEntries)
	pFX->SetFieldType(CDaoFieldExchange::outputColumn);
	DFX_Long(pFX, _T("[AutoId]"), m_AutoId);
	DFX_Long(pFX, _T("[ParentId]"), m_ParentId);
	DFX_Text(pFX, _T("[Path]"), m_Path);
	DFX_Long(pFX, _T("[Children]"), m_Children);
	DFX_Long(pFX, _T("[Attributes]"), m_Attributes);
	DFX_Text(pFX, _T("[Name]"), m_Name);
	DFX_Text(pFX, _T("[Description]"), m_Description);
	DFX_Long(pFX, _T("[DisplayOrder]"), m_DisplayOrder);
	DFX_Long(pFX, _T("[CreatedBy]"), m_CreatedBy);
	DFX_DateTime(pFX, _T("[CreatedOn]"), m_CreatedOn);
	DFX_Long(pFX, _T("[ModifiedBy]"), m_ModifiedBy);
	DFX_DateTime(pFX, _T("[ModifiedOn]"), m_ModifiedOn);
	DFX_Text(pFX, _T("[SpareText]"), m_SpareText);
	DFX_Long(pFX, _T("[SpareNumber]"), m_SpareNumber);
	//}}AFX_FIELD_MAP
}

BOOL CDBBinderEntries::FilterOnParentId(long lParentId, CDBAbstract* lpDatabase)
{

	//	Build the filter	
	m_strFilter.Format(" [ParentId] = %ld And [Attributes] = %ld ", lParentId, 0);	

	m_strSort.Format(" [AutoId]");
	
	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, TMDBASE_BINDERENTRIES_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

BOOL CDBBinderEntries::FilterOnParentIdWithAttribute(long lParentId, CDBAbstract* lpDatabase)
{

	//	Build the filter	
	m_strFilter.Format(" [ParentId] = %ld And [Attributes] = %ld ", lParentId, 1);	

	m_strSort.Format(" [AutoId]");
	
	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, TMDBASE_BINDERENTRIES_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}


BOOL CDBBinderEntries::FilterOnId(long lId, CDBAbstract* lpDatabase)
{

	//	Build the filter	
	m_strFilter.Format(" [AutoId] = %ld ", lId);	

	m_strSort.Format(" [AutoId]");
	
	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, TMDBASE_BINDERENTRIES_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

BOOL CDBBinderEntries::GetAll(CDBAbstract* lpDatabase)
{
	m_strFilter.Empty();
	m_strSort.Format(" [AutoId]");
	
	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, TMDBASE_BINDERENTRIES_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

BOOL CDBBinderEntries::FilterOnMediaId(CString lMediaId, CDBAbstract* lpDatabase)
{
	// In Binder Entry the name field contains the PrimaryId or PrimaryId + SecondaryId
	//	Build the filter	
	m_strFilter.Format(" [Name] = '" + lMediaId + "' ");	

	m_strSort.Format(" [AutoId]");
	
	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, TMDBASE_BINDERENTRIES_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

CString CDBBinderEntries::GetDefaultDBName()
{
	return _T("");
}

CString CDBBinderEntries::GetDefaultSQL()
{
	return _T("[BinderEntries]");
}