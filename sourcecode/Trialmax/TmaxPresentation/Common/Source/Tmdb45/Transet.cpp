//==============================================================================
//
// File Name:	transet.cpp
//
// Description:	This file contains member functions of the CTranscriptSet class.
//
// See Also:	transet.h
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	04-20-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <transet.h>
#include <dao36.h>

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
IMPLEMENT_DYNAMIC(CTranscriptSet, CDaoRecordset)

//==============================================================================
//
// 	Function Name:	CTranscriptSet::CTranscriptSet()
//
// 	Description:	This is the constructor for CTranscriptSet objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTranscriptSet::CTranscriptSet(CDaoDatabase* pDatabase) : CDaoRecordset(pDatabase)
{
	//{{AFX_FIELD_INIT(CTranscriptSet)
	m_TranscriptID = 0;
	m_TranscriptName = _T("");
	m_TranscriptDate = (DATE)0;
	m_RelativePath = _T("");
	m_BaseFileName = _T("");
	m_OrigCtxFileExtension = _T("");
	m_OrigLogDbFileExtension = _T("");
	m_nFields = 7;
	//}}AFX_FIELD_INIT
	m_nDefaultType = dbOpenDynaset;
}

//==============================================================================
//
// 	Function Name:	CTranscriptSet::DoFieldExchange()
//
// 	Description:	This function binds the local members to the database fields
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTranscriptSet::DoFieldExchange(CDaoFieldExchange* pFX)
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

	//{{AFX_FIELD_MAP(CTranscriptSet)
	pFX->SetFieldType(CDaoFieldExchange::outputColumn);
	DFX_Long(pFX, _T("[TranscriptID]"), m_TranscriptID);
	DFX_Text(pFX, _T("[TranscriptName]"), m_TranscriptName);
	DFX_DateTime(pFX, _T("[TranscriptDate]"), m_TranscriptDate);
	DFX_Text(pFX, _T("[RelativePath]"), m_RelativePath);
	DFX_Text(pFX, _T("[BaseFileName]"), m_BaseFileName);
	DFX_Text(pFX, _T("[OrigCtxFileExtension]"), m_OrigCtxFileExtension);
	DFX_Text(pFX, _T("[OrigLogDbFileExtension]"), m_OrigLogDbFileExtension);
	//}}AFX_FIELD_MAP
}

//==============================================================================
//
// 	Function Name:	CTranscriptSet::FilterOnId()
//
// 	Description:	This function will filter the recordset on the numeric
//					identifier specified by the caller.
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CTranscriptSet::FilterOnId(long lId)
{
	//	Build the filter
	if(lId > 0)
		m_strFilter.Format(" [TranscriptID] = %ld ", lId);
	else
		m_strFilter.Empty();

	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(IsOpen())
		Close();
	Open();

	//	Did we find any records?
	return (!IsBOF());
}

//==============================================================================
//
// 	Function Name:	CTranscriptSet::GetCount()
//
// 	Description:	Called to retrieve the number of records in the recordset.
//
// 	Returns:		The number of records.
//
//	Notes:			This function is different from the base class member
//					GetRecordCount(). This function will return the total number
//					of records in the set. GetRecordCount() only returns the
//					number of records that have been ACCESSED.
//
//==============================================================================
long CTranscriptSet::GetCount()
{
	long lPosition;
	long lLast;

	//	Open the recordset if it's not already open
	if(!IsOpen())
		Open();

	//	Are there any records?
	if(!IsOpen() || IsEOF() || IsBOF())
		return 0;

	//	Get the position of the current record
	lPosition = GetAbsolutePosition();

	//	Move to the last record
	MoveLast();
	
	//	Get the position of the last record
	lLast = GetAbsolutePosition();

	//	Reposition the recordset to the original record
	if(lPosition < 0 || lPosition > lLast)
		MoveFirst();
	else
		SetAbsolutePosition(lPosition);

	//	The position is zero based
	return (lLast + 1);
}

//==============================================================================
//
// 	Function Name:	CTranscriptSet::GetDefaultDBName()
//
// 	Description:	Called to retrieve the default database name.
//
// 	Returns:		The name of a default database.
//
//	Notes:			None
//
//==============================================================================
CString CTranscriptSet::GetDefaultDBName()
{
	return _T("");
}

//==============================================================================
//
// 	Function Name:	CTranscriptSet::GetDefaultSQL()
//
// 	Description:	Called to retrieve the default SQL statement.
//
// 	Returns:		Default SQL
//
//	Notes:			None
//
//==============================================================================
CString CTranscriptSet::GetDefaultSQL()
{
	return _T("[Transcripts]");
}



