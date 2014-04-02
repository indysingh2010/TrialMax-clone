//==============================================================================
//
// File Name:	mediaset.cpp
//
// Description:	This file contains member functions of the CMediaSet class.
//
// See Also:	mediaset.h
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-02-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <mediaset.h>
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
IMPLEMENT_DYNAMIC(CMediaSet, CDaoRecordset)

//==============================================================================
//
// 	Function Name:	CMediaSet::CMediaSet()
//
// 	Description:	This is the constructor for CMediaSet objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMediaSet::CMediaSet(CDaoDatabase* pDatabase) : CDaoRecordset(pDatabase)
{
	//{{AFX_FIELD_INIT(CMediaSet)
	m_MediaID = _T("");
	m_GhostMediaId = 0;
	m_MediaPlayerType = 0;
	m_MediaName = _T("");
	m_RelativePath = _T("");
	m_FileName = _T("");
	m_FlagsBinary = 0;
	m_UseTranscript = FALSE;
	m_nFields = 8;
	//}}AFX_FIELD_INIT
	m_nDefaultType = dbOpenDynaset;

	//	Initialize to sort the records on playback order
	m_strSort = "MediaName";
}

//==============================================================================
//
// 	Function Name:	CMediaSet::DoFieldExchange()
//
// 	Description:	This function binds the local members to the database fields
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMediaSet::DoFieldExchange(CDaoFieldExchange* pFX)
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

	//{{AFX_FIELD_MAP(CMediaSet)
	pFX->SetFieldType(CDaoFieldExchange::outputColumn);
	DFX_Text(pFX, _T("[MediaID]"), m_MediaID);
	DFX_Long(pFX, _T("[GhostMediaId]"), m_GhostMediaId);
	DFX_Long(pFX, _T("[MediaPlayerType]"), m_MediaPlayerType);
	DFX_Text(pFX, _T("[MediaName]"), m_MediaName);
	DFX_Text(pFX, _T("[RelativePath]"), m_RelativePath);
	DFX_Text(pFX, _T("[FileName]"), m_FileName);
	DFX_Long(pFX, _T("[FlagsBinary]"), m_FlagsBinary);
	DFX_Bool(pFX, _T("[UseTranscript]"), m_UseTranscript);
	//}}AFX_FIELD_MAP
}

//==============================================================================
//
// 	Function Name:	CMediaSet::FilterOnGhost()
//
// 	Description:	This function will filter the recordset on the numeric
//					identifier specified by the caller.
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CMediaSet::FilterOnGhost(long lId)
{
	//	Build the filter
	if(lId > 0)
		m_strFilter.Format(" [GhostMediaId] = %ld ", lId);
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
// 	Function Name:	CMediaSet::FilterOnId()
//
// 	Description:	This function will filter the recordset on the text
//					identifier specified by the caller.
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CMediaSet::FilterOnId(LPCSTR lpId)
{
	//	Build the filter
	if((lpId != 0) && (lstrlen(lpId) > 0))
		m_strFilter.Format(" [MediaID] = \"%s\" ", lpId);
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
// 	Function Name:	CMediaSet::GetCount()
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
long CMediaSet::GetCount()
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
// 	Function Name:	CMediaSet::GetDefaultDBName()
//
// 	Description:	Called to retrieve the default database name.
//
// 	Returns:		The name of a default database.
//
//	Notes:			None
//
//==============================================================================
CString CMediaSet::GetDefaultDBName()
{
	return _T("");
}

//==============================================================================
//
// 	Function Name:	CMediaSet::GetDefaultSQL()
//
// 	Description:	Called to retrieve the default SQL statement.
//
// 	Returns:		Default SQL
//
//	Notes:			None
//
//==============================================================================
CString CMediaSet::GetDefaultSQL()
{
	return _T("[Media]");
}



