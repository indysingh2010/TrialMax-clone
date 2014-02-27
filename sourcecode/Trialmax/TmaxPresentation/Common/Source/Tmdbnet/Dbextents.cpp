//==============================================================================
//
// File Name:	dbextents.cpp
//
// Description:	This file contains member functions of the CDBExtents class.
//
// See Also:	dbextents.h
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2003
//
//==============================================================================
//	Date		Revision    Description
//	12-26-2003	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <dbextents.h>
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
IMPLEMENT_DYNAMIC(CDBExtents, CDaoRecordset)

//==============================================================================
//
// 	Function Name:	CDBExtents::CDBExtents()
//
// 	Description:	This is the constructor for CDBExtents objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDBExtents::CDBExtents(CDaoDatabase* pDatabase) : CDaoRecordset(pDatabase)
{
	//{{AFX_FIELD_INIT(CDBExtents)
	m_AutoId = 0;
	m_SecondaryId = 0;
	m_TertiaryId = 0;
	m_XmlSegmentId = 0;
	m_HighlighterId = 0;
	m_Start = 0.0;
	m_Stop = 0.0;
	m_StartTuned = FALSE;
	m_StopTuned = FALSE;
	m_StartPL = 0;
	m_StopPL = 0;
	m_nFields = 11;
	//}}AFX_FIELD_INIT
	m_nDefaultType = dbOpenDynaset;

}

//==============================================================================
//
// 	Function Name:	CDBExtents::DoFieldExchange()
//
// 	Description:	This function binds the local members to the database fields
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBExtents::DoFieldExchange(CDaoFieldExchange* pFX)
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

	//{{AFX_FIELD_MAP(CDBExtents)
	pFX->SetFieldType(CDaoFieldExchange::outputColumn);
	DFX_Long(pFX, _T("[AutoId]"), m_AutoId);
	DFX_Long(pFX, _T("[SecondaryId]"), m_SecondaryId);
	DFX_Long(pFX, _T("[TertiaryId]"), m_TertiaryId);
	DFX_Long(pFX, _T("[XmlSegmentId]"), m_XmlSegmentId);
	DFX_Long(pFX, _T("[HighlighterId]"), m_HighlighterId);
	DFX_Double(pFX, _T("[Start]"), m_Start);
	DFX_Double(pFX, _T("[Stop]"), m_Stop);
	DFX_Bool(pFX, _T("[StartTuned]"), m_StartTuned);
	DFX_Bool(pFX, _T("[StopTuned]"), m_StopTuned);
	DFX_Long(pFX, _T("[StartPL]"), m_StartPL);
	DFX_Long(pFX, _T("[StopPL]"), m_StopPL);
	//}}AFX_FIELD_MAP
}

//==============================================================================
//
// 	Function Name:	CDBExtents::FilterOnBoth()
//
// 	Description:	This function will filter the recordset on the secondary
//					id and tertiary id specified by the caller
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CDBExtents::FilterOnBoth(long lSecondaryId, long lTertiaryId, CDBAbstract* lpDatabase)
{
	//	Build the filter
	if(lSecondaryId > 0)
		m_strFilter.Format(" [SecondaryId] = %ld AND [TertiaryId] = %ld", lSecondaryId, lTertiaryId);
	else
		m_strFilter.Empty();

	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, DBNET_EXTENTS_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

//==============================================================================
//
// 	Function Name:	CDBExtents::FilterOnId()
//
// 	Description:	This function will filter the recordset on the numeric
//					identifier specified by the caller.
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CDBExtents::FilterOnId(long lId, CDBAbstract* lpDatabase)
{
	//	Build the filter
	if(lId > 0)
		m_strFilter.Format(" [AutoId] = %ld ", lId);
	else
		m_strFilter.Empty();

	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, DBNET_EXTENTS_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

//==============================================================================
//
// 	Function Name:	CDBExtents::FilterOnSecondaryId()
//
// 	Description:	This function will filter the recordset on the secondary
//					id specified by the caller
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CDBExtents::FilterOnSecondaryId(long lId, CDBAbstract* lpDatabase)
{
	//	Build the filter
	if(lId > 0)
		m_strFilter.Format(" [SecondaryId] = %ld ", lId);
	else
		m_strFilter.Empty();

	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, DBNET_EXTENTS_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

//==============================================================================
//
// 	Function Name:	CDBExtents::FilterOnTertiaryId()
//
// 	Description:	This function will filter the recordset on the tertiary
//					id specified by the caller
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CDBExtents::FilterOnTertiaryId(long lId, CDBAbstract* lpDatabase)
{
	//	Build the filter
	if(lId > 0)
		m_strFilter.Format(" [TertiaryId] = %ld ", lId);
	else
		m_strFilter.Empty();

	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, DBNET_EXTENTS_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

//==============================================================================
//
// 	Function Name:	CDBExtents::GetDefaultDBName()
//
// 	Description:	Called to retrieve the default database name.
//
// 	Returns:		The name of a default database.
//
//	Notes:			None
//
//==============================================================================
CString CDBExtents::GetDefaultDBName()
{
	return _T("");
}

//==============================================================================
//
// 	Function Name:	CDBExtents::GetDefaultSQL()
//
// 	Description:	Called to retrieve the default SQL statement.
//
// 	Returns:		Default SQL
//
//	Notes:			None
//
//==============================================================================
CString CDBExtents::GetDefaultSQL()
{
	return _T("[Extents]");
}

