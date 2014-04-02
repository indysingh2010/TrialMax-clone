//==============================================================================
//
// File Name:	videoset.cpp
//
// Description:	This file contains member functions of the CVideoSet class.
//
// See Also:	videoset.h
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	04-04-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <videoset.h>
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
IMPLEMENT_DYNAMIC(CVideoSet, CDaoRecordset)

//==============================================================================
//
// 	Function Name:	CVideoSet::CVideoSet()
//
// 	Description:	This is the constructor for CVideoSet objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CVideoSet::CVideoSet(CDaoDatabase* pDatabase) : CDaoRecordset(pDatabase)
{
	//{{AFX_FIELD_INIT(CVideoSet)
	m_VideoFileID = 0;
	m_TranscriptID = 0;
	m_RelativePath = _T("");
	m_UnitType = 0;
	m_FileName = _T("");
	m_BeginNum = 0;
	m_EndNum = 0;
	m_MinSelStart = 0;
	m_MaxSelStart = 0;
	m_BeginTuned = FALSE;
	m_EndTuned = FALSE;
	m_RootOverride = _T("");
	m_nFields = 12;
	//}}AFX_FIELD_INIT
	m_nDefaultType = dbOpenDynaset;
}

//==============================================================================
//
// 	Function Name:	CVideoSet::DoFieldExchange()
//
// 	Description:	This function binds the local members to the database fields
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CVideoSet::DoFieldExchange(CDaoFieldExchange* pFX)
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

	//{{AFX_FIELD_MAP(CVideoSet)
	pFX->SetFieldType(CDaoFieldExchange::outputColumn);
	DFX_Long(pFX, _T("[VideoFileID]"), m_VideoFileID);
	DFX_Long(pFX, _T("[TranscriptID]"), m_TranscriptID);
	DFX_Text(pFX, _T("[RelativePath]"), m_RelativePath);
	DFX_Long(pFX, _T("[UnitType]"), m_UnitType);
	DFX_Text(pFX, _T("[FileName]"), m_FileName);
	DFX_Long(pFX, _T("[BeginNum]"), m_BeginNum);
	DFX_Long(pFX, _T("[EndNum]"), m_EndNum);
	DFX_Long(pFX, _T("[MinSelStart]"), m_MinSelStart);
	DFX_Long(pFX, _T("[MaxSelStart]"), m_MaxSelStart);
	DFX_Bool(pFX, _T("[BeginTuned]"), m_BeginTuned);
	DFX_Bool(pFX, _T("[EndTuned]"), m_EndTuned);
	DFX_Text(pFX, _T("[RootOverride]"), m_RootOverride);
	//}}AFX_FIELD_MAP
}

//==============================================================================
//
// 	Function Name:	CVideoSet::FilterOnId()
//
// 	Description:	This function will filter the recordset on the video file
//					identifier specified by the caller.
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CVideoSet::FilterOnId(long lId)
{
	//	Build the filter
	if(lId > 0)
		m_strFilter.Format(" [VideoFileID] = %ld ", lId);
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
// 	Function Name:	CVideoSet::GetCount()
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
long CVideoSet::GetCount()
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
// 	Function Name:	CVideoSet::GetDefaultDBName()
//
// 	Description:	Called to retrieve the default database name.
//
// 	Returns:		The name of a default database.
//
//	Notes:			None
//
//==============================================================================
CString CVideoSet::GetDefaultDBName()
{
	return _T("");
}

//==============================================================================
//
// 	Function Name:	CVideoSet::GetDefaultSQL()
//
// 	Description:	Called to retrieve the default SQL statement.
//
// 	Returns:		Default SQL
//
//	Notes:			None
//
//==============================================================================
CString CVideoSet::GetDefaultSQL()
{
	return _T("[VideoFiles]");
}



