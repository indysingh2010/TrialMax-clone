//==============================================================================
//
// File Name:	dbquaternary.cpp
//
// Description:	This file contains member functions of the CDBQuaternary class.
//
// See Also:	dbquaternary.h
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2004
//
//==============================================================================
//	Date		Revision    Description
//	12-26-2004	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <dbquaternary.h>
#include <dao36.h>
#include <dbabstract.h>
#include <dbnet.h>
#include <dbdefs.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
IMPLEMENT_DYNAMIC(CDBQuaternary, CDaoRecordset)

//==============================================================================
//
// 	Function Name:	CDBQuaternary::CDBQuaternary()
//
// 	Description:	This is the constructor for CDBQuaternary objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDBQuaternary::CDBQuaternary(CDaoDatabase* pDatabase) : CDaoRecordset(pDatabase)
{
	//{{AFX_FIELD_INIT(CDBQuaternary)
	m_AutoId = 0;
	m_TertiaryMediaId = 0;
	m_BarcodeId = 0;
	m_Attributes = 0;
	m_MediaType = 0;
	m_SourceId = _T("");
	m_SourceType = 0;
	m_Description = _T("");
	m_Name = _T("");
	m_DisplayOrder = 0;
	m_StartPL = 0;
	m_Start = 0.0;
	m_StartTuned = FALSE;
	m_CreatedBy = 0;
	m_CreatedOn = (DATE)0;
	m_ModifiedBy = 0;
	m_ModifiedOn = (DATE)0;
	m_nFields = 17;
	//}}AFX_FIELD_INIT
	m_nDefaultType = dbOpenDynaset;

	//	Initialize to sort the records on playback order
	m_strSort = "DisplayOrder";
}

//==============================================================================
//
// 	Function Name:	CDBQuaternary::DoFieldExchange()
//
// 	Description:	This function binds the local members to the database fields
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBQuaternary::DoFieldExchange(CDaoFieldExchange* pFX)
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

	//{{AFX_FIELD_MAP(CDBQuaternary)
	pFX->SetFieldType(CDaoFieldExchange::outputColumn);
	DFX_Long(pFX, _T("[AutoId]"), m_AutoId);
	DFX_Long(pFX, _T("[TertiaryMediaId]"), m_TertiaryMediaId);
	DFX_Long(pFX, _T("[BarcodeId]"), m_BarcodeId);
	DFX_Long(pFX, _T("[Attributes]"), m_Attributes);
	DFX_Short(pFX, _T("[MediaType]"), m_MediaType);
	DFX_Text(pFX, _T("[SourceId]"), m_SourceId);
	DFX_Short(pFX, _T("[SourceType]"), m_SourceType);
	DFX_Text(pFX, _T("[Description]"), m_Description);
	DFX_Text(pFX, _T("[Name]"), m_Name);
	DFX_Long(pFX, _T("[DisplayOrder]"), m_DisplayOrder);
	DFX_Long(pFX, _T("[StartPL]"), m_StartPL);
	DFX_Double(pFX, _T("[Start]"), m_Start);
	DFX_Bool(pFX, _T("[StartTuned]"), m_StartTuned);
	DFX_Long(pFX, _T("[CreatedBy]"), m_CreatedBy);
	DFX_DateTime(pFX, _T("[CreatedOn]"), m_CreatedOn);
	DFX_Long(pFX, _T("[ModifiedBy]"), m_ModifiedBy);
	DFX_DateTime(pFX, _T("[ModifiedOn]"), m_ModifiedOn);
	//}}AFX_FIELD_MAP
}

//==============================================================================
//
// 	Function Name:	CDBQuaternary::FilterOnId()
//
// 	Description:	This function will filter the recordset on the numeric
//					identifier specified by the caller.
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CDBQuaternary::FilterOnId(long lId, CDBAbstract* lpDatabase)
{
	//	Build the filter
	if(lId > 0)
		m_strFilter.Format(" [AutoId] = %ld ", lId);
	else
		m_strFilter.Empty();

	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, DBNET_QUATERNARY_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

//==============================================================================
//
// 	Function Name:	CDBQuaternary::FilterOnTertiaryId()
//
// 	Description:	This function will filter the recordset on the numeric
//					identifier specified by the caller.
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CDBQuaternary::FilterOnTertiaryId(long lId, CDBAbstract* lpDatabase)
{
	//	Build the filter
	if(lId > 0)
		m_strFilter.Format(" [TertiaryMediaId] = %ld ", lId);
	else
		m_strFilter.Empty();

	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, DBNET_QUATERNARY_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

//==============================================================================
//
// 	Function Name:	CDBQuaternary::GetDefaultDBName()
//
// 	Description:	Called to retrieve the default database name.
//
// 	Returns:		The name of a default database.
//
//	Notes:			None
//
//==============================================================================
CString CDBQuaternary::GetDefaultDBName()
{
	return _T("");
}

//==============================================================================
//
// 	Function Name:	CDBQuaternary::GetDefaultSQL()
//
// 	Description:	Called to retrieve the default SQL statement.
//
// 	Returns:		Default SQL
//
//	Notes:			None
//
//==============================================================================
CString CDBQuaternary::GetDefaultSQL()
{
	return _T("[QuaternaryMedia]");
}

