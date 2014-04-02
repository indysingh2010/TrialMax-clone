//==============================================================================
//
// File Name:	showset.cpp
//
// Description:	This file contains member functions of the CShowItemSet 
//				class
//
// See Also:	showset.h
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	04-27-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <showset.h>
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
IMPLEMENT_DYNAMIC(CShowItemSet, CDaoRecordset)

//==============================================================================
//
// 	Function Name:	CShowItemSet::CShowItemSet()
//
// 	Description:	This is the constructor for CShowItemSet objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CShowItemSet::CShowItemSet(CDaoDatabase* pDatabase) 
				:CDaoRecordset(pDatabase)
{
	//{{AFX_FIELD_INIT(CShowItemSet)
	m_MediaID = _T("");
	m_ShowItemID = 0;
	m_Description = _T("");
	m_PlaybackOrder = 0;
	m_ItemBarcode = _T("");
	m_Hide = FALSE;
	m_nFields = 6;
	//}}AFX_FIELD_INIT
	m_nDefaultType = dbOpenDynaset;

	//	Initialize to sort the records on playback order
	m_strSort = "PlaybackOrder";
}

//==============================================================================
//
// 	Function Name:	CShowItemSet::DoFieldExchange()
//
// 	Description:	This function binds the local members to the database fields
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CShowItemSet::DoFieldExchange(CDaoFieldExchange* pFX)
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

	//{{AFX_FIELD_MAP(CShowItemSet)
	pFX->SetFieldType(CDaoFieldExchange::outputColumn);
	DFX_Text(pFX, _T("[MediaID]"), m_MediaID);
	DFX_Long(pFX, _T("[ShowItemID]"), m_ShowItemID);
	DFX_Text(pFX, _T("[Description]"), m_Description);
	DFX_Long(pFX, _T("[PlaybackOrder]"), m_PlaybackOrder);
	DFX_Text(pFX, _T("[ItemBarcode]"), m_ItemBarcode);
	DFX_Bool(pFX, _T("[Hide]"), m_Hide);
	//}}AFX_FIELD_MAP
}

//==============================================================================
//
// 	Function Name:	CShowItemSet::FilterOnMedia()
//
// 	Description:	This function will filter the recordset on the media
//					identifier specified by the caller.
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CShowItemSet::FilterOnMedia(LPCSTR lpMediaId)
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
// 	Function Name:	CShowItemSet::GetCount()
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
long CShowItemSet::GetCount()
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
// 	Function Name:	CShowItemSet::GetDefaultDBName()
//
// 	Description:	Called to retrieve the default database name.
//
// 	Returns:		The name of a default database.
//
//	Notes:			None
//
//==============================================================================
CString CShowItemSet::GetDefaultDBName()
{
	return _T("");
}

//==============================================================================
//
// 	Function Name:	CShowItemSet::GetDefaultSQL()
//
// 	Description:	Called to retrieve the default SQL statement.
//
// 	Returns:		Default SQL
//
//	Notes:			None
//
//==============================================================================
CString CShowItemSet::GetDefaultSQL()
{
	return _T("[CustomShowItems]");
}
