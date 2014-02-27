//==============================================================================
//
// File Name:	desgset.cpp
//
// Description:	This file contains member functions of the CDesignationSet 
//				class
//
// See Also:	desgset.h
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
#include <desgset.h>
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
IMPLEMENT_DYNAMIC(CDesignationSet, CDaoRecordset)

//==============================================================================
//
// 	Function Name:	CDesignationSet::CDesignationSet()
//
// 	Description:	This is the constructor for CDesignationSet objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDesignationSet::CDesignationSet(CDaoDatabase* pDatabase) 
				:CDaoRecordset(pDatabase)
{
	//{{AFX_FIELD_INIT(CDesignationSet)
	m_MediaID = _T("");
	m_DesignationID = 0;
	m_TranscriptID = 0;
	m_Description = _T("");
	m_PlaybackOrder = 0;
	m_ColorID = 0;
	m_DisplayType = 0;
	m_StartPage = 0;
	m_StartLine = 0;
	m_StopPage = 0;
	m_StopLine = 0;
	m_SelStart = 0;
	m_SelLength = 0;
	m_StartNum = 0;
	m_StopNum = 0;
	m_VideoFileID = 0;
	m_StartTuned = FALSE;
	m_StopTuned = FALSE;
	m_HasObjections = FALSE;
	m_OverlayFileName = _T("");
	m_OverlayRelativePath = _T("");
	m_nFields = 21;
	//}}AFX_FIELD_INIT
	m_nDefaultType = dbOpenDynaset;

	//	Initialize to sort the records on playback order
	m_strSort = "PlaybackOrder";
}

//==============================================================================
//
// 	Function Name:	CDesignationSet::DoFieldExchange()
//
// 	Description:	This function binds the local members to the database fields
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDesignationSet::DoFieldExchange(CDaoFieldExchange* pFX)
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

	//{{AFX_FIELD_MAP(CDesignationSet)
	pFX->SetFieldType(CDaoFieldExchange::outputColumn);
	DFX_Text(pFX, _T("[MediaID]"), m_MediaID);
	DFX_Long(pFX, _T("[DesignationID]"), m_DesignationID);
	DFX_Long(pFX, _T("[TranscriptID]"), m_TranscriptID);
	DFX_Text(pFX, _T("[Description]"), m_Description);
	DFX_Long(pFX, _T("[PlaybackOrder]"), m_PlaybackOrder);
	DFX_Long(pFX, _T("[ColorID]"), m_ColorID);
	DFX_Long(pFX, _T("[DisplayType]"), m_DisplayType);
	DFX_Long(pFX, _T("[StartPage]"), m_StartPage);
	DFX_Long(pFX, _T("[StartLine]"), m_StartLine);
	DFX_Long(pFX, _T("[StopPage]"), m_StopPage);
	DFX_Long(pFX, _T("[StopLine]"), m_StopLine);
	DFX_Long(pFX, _T("[SelStart]"), m_SelStart);
	DFX_Long(pFX, _T("[SelLength]"), m_SelLength);
	DFX_Long(pFX, _T("[StartNum]"), m_StartNum);
	DFX_Long(pFX, _T("[StopNum]"), m_StopNum);
	DFX_Long(pFX, _T("[VideoFileID]"), m_VideoFileID);
	DFX_Bool(pFX, _T("[StartTuned]"), m_StartTuned);
	DFX_Bool(pFX, _T("[StopTuned]"), m_StopTuned);
	DFX_Bool(pFX, _T("[HasObjections]"), m_HasObjections);
	DFX_Text(pFX, _T("[OverlayFileName]"), m_OverlayFileName);
	DFX_Text(pFX, _T("[OverlayRelativePath]"), m_OverlayRelativePath);
	//}}AFX_FIELD_MAP
}

//==============================================================================
//
// 	Function Name:	CDesignationSet::FilterOnMedia()
//
// 	Description:	This function will filter the recordset on the media
//					identifier specified by the caller.
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CDesignationSet::FilterOnMedia(LPCSTR lpMediaId)
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
// 	Function Name:	CDesignationSet::GetCount()
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
long CDesignationSet::GetCount()
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
// 	Function Name:	CDesignationSet::GetDefaultDBName()
//
// 	Description:	Called to retrieve the default database name.
//
// 	Returns:		The name of a default database.
//
//	Notes:			None
//
//==============================================================================
CString CDesignationSet::GetDefaultDBName()
{
	return _T("");
}

//==============================================================================
//
// 	Function Name:	CDesignationSet::GetDefaultSQL()
//
// 	Description:	Called to retrieve the default SQL statement.
//
// 	Returns:		Default SQL
//
//	Notes:			None
//
//==============================================================================
CString CDesignationSet::GetDefaultSQL()
{
	return _T("[Designations]");
}

