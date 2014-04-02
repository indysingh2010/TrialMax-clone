//==============================================================================
//
// File Name:	dbtranscripts.cpp
//
// Description:	This file contains member functions of the CDBTranscripts class.
//
// See Also:	dbtranscripts.h
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
#include <dbtranscripts.h>
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
IMPLEMENT_DYNAMIC(CDBTranscripts, CDaoRecordset)

//==============================================================================
//
// 	Function Name:	CDBTranscripts::CDBTranscripts()
//
// 	Description:	This is the constructor for CDBTranscripts objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDBTranscripts::CDBTranscripts(CDaoDatabase* pDatabase) : CDaoRecordset(pDatabase)
{
	//{{AFX_FIELD_INIT(CDBTranscripts)
	m_AutoId = 0;
	m_PrimaryId = 0;
	m_Deponent = _T("");
	m_DeposedOn = _T("");
	m_Filename = _T("");
	m_FirstPL = 0;
	m_LastPL = 0;
	m_LinesPerPage = 0;
	m_nFields = 8;
	//}}AFX_FIELD_INIT
	m_nDefaultType = dbOpenDynaset;

}

//==============================================================================
//
// 	Function Name:	CDBTranscripts::DoFieldExchange()
//
// 	Description:	This function binds the local members to the database fields
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBTranscripts::DoFieldExchange(CDaoFieldExchange* pFX)
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

	//{{AFX_FIELD_MAP(CDBTranscripts)
	pFX->SetFieldType(CDaoFieldExchange::outputColumn);
	DFX_Long(pFX, _T("[AutoId]"), m_AutoId);
	DFX_Long(pFX, _T("[PrimaryId]"), m_PrimaryId);
	DFX_Text(pFX, _T("[Deponent]"), m_Deponent);
	DFX_Text(pFX, _T("[DeposedOn]"), m_DeposedOn);
	DFX_Text(pFX, _T("[Filename]"), m_Filename);
	DFX_Long(pFX, _T("[FirstPL]"), m_FirstPL);
	DFX_Long(pFX, _T("[LastPL]"), m_LastPL);
	DFX_Short(pFX, _T("[LinesPerPage]"), m_LinesPerPage);
	//}}AFX_FIELD_MAP
}

//==============================================================================
//
// 	Function Name:	CDBTranscripts::FilterOnId()
//
// 	Description:	This function will filter the recordset on the numeric
//					identifier specified by the caller.
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CDBTranscripts::FilterOnId(long lId, CDBAbstract* lpDatabase)
{
	//	Build the filter
	if(lId > 0)
		m_strFilter.Format(" [AutoId] = %ld ", lId);
	else
		m_strFilter.Empty();

	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, DBNET_TRANSCRIPTS_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

//==============================================================================
//
// 	Function Name:	CDBTranscripts::FilterOnPrimaryId()
//
// 	Description:	This function will filter the recordset on the numeric
//					identifier specified by the caller.
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CDBTranscripts::FilterOnPrimaryId(long lId, CDBAbstract* lpDatabase)
{
	//	Build the filter
	if(lId > 0)
		m_strFilter.Format(" [PrimaryId] = %ld ", lId);
	else
		m_strFilter.Empty();

	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, DBNET_TRANSCRIPTS_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

//==============================================================================
//
// 	Function Name:	CDBTranscripts::GetDefaultDBName()
//
// 	Description:	Called to retrieve the default database name.
//
// 	Returns:		The name of a default database.
//
//	Notes:			None
//
//==============================================================================
CString CDBTranscripts::GetDefaultDBName()
{
	return _T("");
}

//==============================================================================
//
// 	Function Name:	CDBTranscripts::GetDefaultSQL()
//
// 	Description:	Called to retrieve the default SQL statement.
//
// 	Returns:		Default SQL
//
//	Notes:			None
//
//==============================================================================
CString CDBTranscripts::GetDefaultSQL()
{
	return _T("[Transcripts]");
}


