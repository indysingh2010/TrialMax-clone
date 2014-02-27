//==============================================================================
//
// File Name:	textset.cpp
//
// Description:	This file contains member functions of the CTextSet class.
//
// See Also:	textset.h
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	04-07-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <textset.h>
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
IMPLEMENT_DYNAMIC(CTextSet, CDaoRecordset)

//==============================================================================
//
// 	Function Name:	CTextSet::CTextSet()
//
// 	Description:	This is the constructor for CTextSet objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTextSet::CTextSet(CDaoDatabase* pDatabase) : CDaoRecordset(pDatabase)
{
	//{{AFX_FIELD_INIT(CTextSet)
	m_MediaID = _T("");
	m_DesignationID = 0;
	m_PageNum = 0;
	m_LineNum = 0;
	m_TextLine = _T("");
	m_FirstNum = 0;
	m_LastNum = 0;
	m_nFields = 7;
	//}}AFX_FIELD_INIT
	m_nDefaultType = dbOpenDynaset;
}

//==============================================================================
//
// 	Function Name:	CTextSet::DoFieldExchange()
//
// 	Description:	This function binds the local members to the database fields
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTextSet::DoFieldExchange(CDaoFieldExchange* pFX)
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

	//{{AFX_FIELD_MAP(CTextSet)
	pFX->SetFieldType(CDaoFieldExchange::outputColumn);
	DFX_Text(pFX, _T("[MediaID]"), m_MediaID);
	DFX_Long(pFX, _T("[DesignationID]"), m_DesignationID);
	DFX_Long(pFX, _T("[PageNum]"), m_PageNum);
	DFX_Long(pFX, _T("[LineNum]"), m_LineNum);
	DFX_Text(pFX, _T("[TextLine]"), m_TextLine);
	DFX_Long(pFX, _T("[FirstNum]"), m_FirstNum);
	DFX_Long(pFX, _T("[LastNum]"), m_LastNum);
	//}}AFX_FIELD_MAP
}

//==============================================================================
//
// 	Function Name:	CTextSet::FilterOnMedia()
//
// 	Description:	This function will filter the recordset on the media
//					identifier specified by the caller.
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CTextSet::FilterOnMedia(LPCSTR lpMediaId)
{
	//	Build the filter
	if((lpMediaId != 0) && (lstrlen(lpMediaId) > 0))
		m_strFilter.Format(" [MediaID] = \"%s\" ", lpMediaId);
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
// 	Function Name:	CTextSet::FilterOnDesignation()
//
// 	Description:	This function will filter the recordset on the media
//					identifier and page identifier specified by the caller.
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			This function assumes the caller is providing valid
//					identifiers. FilterOnMedia() should be used to restore the
//					record set.
//
//==============================================================================
BOOL CTextSet::FilterOnDesignation(LPCSTR lpMediaId, long lDesignationId)
{
	ASSERT(lpMediaId);
	ASSERT(lDesignationId >= 0);

	//	Build the filter
	m_strFilter.Format(" [MediaID] = \"%s\" AND [DesignationID] = %ld ", 
					   lpMediaId, lDesignationId);

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
// 	Function Name:	CTextSet::GetCount()
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
long CTextSet::GetCount()
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
// 	Function Name:	CTextSet::GetDefaultDBName()
//
// 	Description:	Called to retrieve the default database name.
//
// 	Returns:		The name of a default database.
//
//	Notes:			None
//
//==============================================================================
CString CTextSet::GetDefaultDBName()
{
	return _T("");
}

//==============================================================================
//
// 	Function Name:	CTextSet::GetDefaultSQL()
//
// 	Description:	Called to retrieve the default SQL statement.
//
// 	Returns:		Default SQL
//
//	Notes:			None
//
//==============================================================================
CString CTextSet::GetDefaultSQL()
{
	return _T("[DesignationLines]");
}

