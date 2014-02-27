//==============================================================================
//
// File Name:	dbtertiary.cpp
//
// Description:	This file contains member functions of the CDBTertiary class.
//
// See Also:	dbtertiary.h
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
#include <dbtertiary.h>
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
IMPLEMENT_DYNAMIC(CDBTertiary, CDaoRecordset)

//==============================================================================
//
// 	Function Name:	CDBTertiary::CDBTertiary()
//
// 	Description:	This is the constructor for CDBTertiary objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDBTertiary::CDBTertiary(CDaoDatabase* pDatabase) : CDaoRecordset(pDatabase)
{
	//{{AFX_FIELD_INIT(CDBTertiary)
	m_AutoId = 0;
	m_SecondaryMediaId = 0;
	m_BarcodeId = 0;
	m_Children = 0;
	m_Attributes = 0;
	m_MediaType = 0;
	m_Filename = _T("");
	m_SourceId = _T("");
	m_SourceType = 0;
	m_Description = _T("");
	m_Name = _T("");
	m_DisplayOrder = 0;
	m_CreatedBy = 0;
	m_CreatedOn = (DATE)0;
	m_ModifiedBy = 0;
	m_ModifiedOn = (DATE)0;
	m_SiblingId = _T("");
	m_nFields = 17;
	//}}AFX_FIELD_INIT
	m_nDefaultType = dbOpenDynaset;

	//	Initialize to sort the records on playback order
	m_strSort = "DisplayOrder";

	m_bSplitScreenTreatments = FALSE;
}

//==============================================================================
//
// 	Function Name:	CDBTertiary::DoFieldExchange()
//
// 	Description:	This function binds the local members to the database fields
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBTertiary::DoFieldExchange(CDaoFieldExchange* pFX)
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
			if(pcb->dwDataType == DAO_CHAR)
			{
				pcb->cbDataOffset = (DWORD)Access2KStringAllocCallback;
			}			
		}
	} 

#endif

	//{{AFX_FIELD_MAP(CDBTertiary)
	pFX->SetFieldType(CDaoFieldExchange::outputColumn);
	DFX_Long(pFX, _T("[AutoId]"), m_AutoId);
	DFX_Long(pFX, _T("[SecondaryMediaId]"), m_SecondaryMediaId);
	DFX_Long(pFX, _T("[BarcodeId]"), m_BarcodeId);
	DFX_Long(pFX, _T("[Children]"), m_Children);
	DFX_Long(pFX, _T("[Attributes]"), m_Attributes);
	DFX_Short(pFX, _T("[MediaType]"), m_MediaType);
	DFX_Text(pFX, _T("[Filename]"), m_Filename);
	DFX_Text(pFX, _T("[SourceId]"), m_SourceId);
	DFX_Short(pFX, _T("[SourceType]"), m_SourceType);
	DFX_Text(pFX, _T("[Description]"), m_Description);
	DFX_Text(pFX, _T("[Name]"), m_Name);
	DFX_Long(pFX, _T("[DisplayOrder]"), m_DisplayOrder);
	DFX_Long(pFX, _T("[CreatedBy]"), m_CreatedBy);
	DFX_DateTime(pFX, _T("[CreatedOn]"), m_CreatedOn);
	DFX_Long(pFX, _T("[ModifiedBy]"), m_ModifiedBy);
	DFX_DateTime(pFX, _T("[ModifiedOn]"), m_ModifiedOn);
	//}}AFX_FIELD_MAP

	//	Retrieve the SiblingId value on if split screen treatments are enabled
	if(m_bSplitScreenTreatments == TRUE)
		DFX_Text(pFX, _T("[SiblingId]"), m_SiblingId);
	else
		m_SiblingId = "";
}

//==============================================================================
//
// 	Function Name:	CDBTertiary::FilterOnId()
//
// 	Description:	This function will filter the recordset on the numeric
//					identifier specified by the caller.
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CDBTertiary::FilterOnId(long lId, CDBAbstract* lpDatabase)
{
	//	Build the filter
	if(lId > 0)
		m_strFilter.Format(" [AutoId] = %ld ", lId);
	else
		m_strFilter.Empty();

	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, DBNET_TERTIARY_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

//==============================================================================
//
// 	Function Name:	CDBTertiary::FilterOnSecondaryId()
//
// 	Description:	This function will filter the recordset on the numeric
//					identifier specified by the caller.
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CDBTertiary::FilterOnSecondaryId(long lId, long lBarcodeId, CDBAbstract* lpDatabase)
{
	//	Build the filter
	if(lId > 0)
	{
		if(lBarcodeId > 0)
			m_strFilter.Format(" [SecondaryMediaId] = %ld AND [BarcodeId] = %ld ", lId, lBarcodeId);
		else
			m_strFilter.Format(" [SecondaryMediaId] = %ld ", lId);
	}
	else
	{
		m_strFilter.Empty();
	}

	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, DBNET_TERTIARY_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

//==============================================================================
//
// 	Function Name:	CDBTertiary::GetDefaultDBName()
//
// 	Description:	Called to retrieve the default database name.
//
// 	Returns:		The name of a default database.
//
//	Notes:			None
//
//==============================================================================
CString CDBTertiary::GetDefaultDBName()
{
	return _T("");
}

//==============================================================================
//
// 	Function Name:	CDBTertiary::GetDefaultSQL()
//
// 	Description:	Called to retrieve the default SQL statement.
//
// 	Returns:		Default SQL
//
//	Notes:			None
//
//==============================================================================
CString CDBTertiary::GetDefaultSQL()
{
	return _T("[TertiaryMedia]");
}

//==============================================================================
//
// 	Function Name:	CDBTertiary::SetSplitScreenTreatments()
//
// 	Description:	Called to enable/disable split screen treatments
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBTertiary::SetSplitScreenTreatments(BOOL bEnabled)
{
	m_bSplitScreenTreatments = bEnabled;

	//	The SiblingId field was added for split screen treatments in ver. 6.3.4
	m_nFields = (m_bSplitScreenTreatments == TRUE) ? 17 : 16;
}

