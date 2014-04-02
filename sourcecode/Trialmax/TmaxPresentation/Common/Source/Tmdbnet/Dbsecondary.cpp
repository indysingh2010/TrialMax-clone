//==============================================================================
//
// File Name:	dbsecondary.cpp
//
// Description:	This file contains member functions of the CDBSecondary class.
//
// See Also:	dbsecondary.h
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
#include <dbsecondary.h>
#include <dao36.h>
#include <media.h>
#include <transcpt.h>
#include <secondary.h>
#include <showitem.h>
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
IMPLEMENT_DYNAMIC(CDBSecondary, CDaoRecordset)

//==============================================================================
//
// 	Function Name:	CDBSecondary::CDBSecondary()
//
// 	Description:	This is the constructor for CDBSecondary objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDBSecondary::CDBSecondary(CDaoDatabase* pDatabase) : CDaoRecordset(pDatabase)
{
	//{{AFX_FIELD_INIT(CDBSecondary)
	m_AutoId = 0;
	m_PrimaryMediaId = 0;
	m_BarcodeId = 0;
	m_Children = 0;
	m_Attributes = 0;
	m_MediaType = 0;
	m_SourceMediaType = 0;
	m_TransitionTime = 0;
	m_SourcePST = _T("");
	m_Filename = _T("");
	m_MultipageId = 0;
	m_Description = _T("");
	m_Name = _T("");
	m_DisplayOrder = 0;
	m_CreatedBy = 0;
	m_CreatedOn = (DATE)0;
	m_ModifiedBy = 0;
	m_ModifiedOn = (DATE)0;
	m_AliasId = 0;
	m_RelativePath = _T("");
	m_nFields = 20;
	//}}AFX_FIELD_INIT
	m_nDefaultType = dbOpenDynaset;

	//	Initialize to sort the records on playback order
	m_strSort = "DisplayOrder";
}

//==============================================================================
//
// 	Function Name:	CDBSecondary::DoFieldExchange()
//
// 	Description:	This function binds the local members to the database fields
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBSecondary::DoFieldExchange(CDaoFieldExchange* pFX)
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

	//{{AFX_FIELD_MAP(CDBSecondary)
	pFX->SetFieldType(CDaoFieldExchange::outputColumn);
	DFX_Long(pFX, _T("[AutoId]"), m_AutoId);
	DFX_Long(pFX, _T("[PrimaryMediaId]"), m_PrimaryMediaId);
	DFX_Long(pFX, _T("[BarcodeId]"), m_BarcodeId);
	DFX_Long(pFX, _T("[Children]"), m_Children);
	DFX_Long(pFX, _T("[Attributes]"), m_Attributes);
	DFX_Short(pFX, _T("[MediaType]"), m_MediaType);
	DFX_Short(pFX, _T("[SourceType]"), m_SourceMediaType);
	DFX_Short(pFX, _T("[TransitionTime]"), m_TransitionTime);
	DFX_Text(pFX, _T("[SourceId]"), m_SourcePST);
	DFX_Text(pFX, _T("[Filename]"), m_Filename);
	DFX_Long(pFX, _T("[MultipageId]"), m_MultipageId);
	DFX_Text(pFX, _T("[Description]"), m_Description);
	DFX_Text(pFX, _T("[Name]"), m_Name);
	DFX_Long(pFX, _T("[DisplayOrder]"), m_DisplayOrder);
	DFX_Long(pFX, _T("[CreatedBy]"), m_CreatedBy);
	DFX_DateTime(pFX, _T("[CreatedOn]"), m_CreatedOn);
	DFX_Long(pFX, _T("[ModifiedBy]"), m_ModifiedBy);
	DFX_DateTime(pFX, _T("[ModifiedOn]"), m_ModifiedOn);
	DFX_Long(pFX, _T("[AliasId]"), m_AliasId);
	DFX_Text(pFX, _T("[RelativePath]"), m_RelativePath);
	//}}AFX_FIELD_MAP
}

//==============================================================================
//
// 	Function Name:	CDBSecondary::FilterOnId()
//
// 	Description:	This function will filter the recordset on the numeric
//					identifier specified by the caller.
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CDBSecondary::FilterOnId(long lId, CDBAbstract* lpDatabase)
{
	//	Build the filter
	if(lId > 0)
		m_strFilter.Format(" [AutoId] = %ld ", lId);
	else
		m_strFilter.Empty();

	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, DBNET_SECONDARY_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

//==============================================================================
//
// 	Function Name:	CDBSecondary::Read()
//
// 	Description:	This function will read the values in the recordset and use
//					them to set the members of the secondary object
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CDBSecondary::Read(CSecondary* pSecondary)
{
	//	Are there any records?
	if(!IsOpen() || IsEOF() || IsBOF())
		return FALSE;

	pSecondary->m_lSecondaryId = m_AutoId;
	pSecondary->m_lSlideId = m_MultipageId;
	pSecondary->m_lPlaybackOrder = m_DisplayOrder;
	pSecondary->m_lAliasId = m_AliasId;
	pSecondary->m_strRelativePath = m_RelativePath;
	pSecondary->m_strFilename = m_Filename;
	pSecondary->m_bLinked = (pSecondary->m_lAliasId > 0);

	pSecondary->m_lPrimaryId = m_PrimaryMediaId;
	pSecondary->m_lBarcodeId = m_BarcodeId;
	pSecondary->m_lChildren = m_Children;
	pSecondary->m_lAttributes = m_Attributes;
	pSecondary->m_sMediaType = m_MediaType;
	pSecondary->m_strDescription = m_Description;
	pSecondary->m_strName = m_Name;

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CDBSecondary::Read()
//
// 	Description:	This function will read the values in the recordset and use
//					them to set the members of the show item object
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CDBSecondary::Read(CShowItem* pShowItem)
{
	BOOL bSuccessful = TRUE;

	if(!IsOpen() || IsEOF() || IsBOF())
		return FALSE;

	//	Get the base class secondary values first
	if(Read((CSecondary*)pShowItem) == FALSE)
		bSuccessful = FALSE;

	//	Get the show item specific members
	pShowItem->m_sSourceMediaType = m_SourceMediaType;
	pShowItem->m_strSourcePST = m_SourcePST;

	return bSuccessful;
}

//==============================================================================
//
// 	Function Name:	CDBSecondary::FilterOnDepositions()
//
// 	Description:	This function will filter the recordset to contain only
//					secondary records owned by the specified primary media
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CDBSecondary::FilterOnDepositions(CMedias* pMedias, CDBAbstract* lpDatabase)
{
	POSITION	Pos;
	CMedia*		pMedia;
	BOOL		bIsFirst = TRUE;
	CString		strTemp;

	//	Build the filter
	if((pMedias != 0) && (pMedias->GetCount() > 0))
	{
		m_strFilter = "[PrimaryMediaId] IN ( ";
		Pos = pMedias->GetHeadPosition();
		while(Pos != NULL)
		{
			if((pMedia = (CMedia*)(pMedias->GetNext(Pos))) != 0)
			{
				if(bIsFirst == TRUE)
					bIsFirst = FALSE;
				else
					m_strFilter += ",";

				strTemp.Format("%ld", pMedia->m_lPrimaryId);
				m_strFilter += strTemp;
			}

		}

		m_strFilter += " )";

	}
	else
	{
		m_strFilter.Empty();
	}

	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, DBNET_SECONDARY_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

//==============================================================================
//
// 	Function Name:	CDBSecondary::FilterOnPrimaryId()
//
// 	Description:	This function will filter the recordset on the numeric
//					identifier specified by the caller.
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CDBSecondary::FilterOnPrimaryId(long lId, CDBAbstract* lpDatabase)
{
	//	Build the filter
	if(lId > 0)
		m_strFilter.Format(" [PrimaryMediaId] = %ld ", lId);
	else
		m_strFilter.Empty();

	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, DBNET_SECONDARY_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

//==============================================================================
//
// 	Function Name:	CDBSecondary::FilterOnTranscripts()
//
// 	Description:	This function will filter the recordset to contain only
//					secondary records owned by the specified transcripts
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CDBSecondary::FilterOnTranscripts(CTranscripts* pTranscripts, CDBAbstract* lpDatabase)
{
	POSITION		Pos;
	CTranscript*	pTranscript;
	BOOL			bIsFirst = TRUE;
	CString			strTemp;

	//	Build the filter
	if((pTranscripts != 0) && (pTranscripts->GetCount() > 0))
	{
		m_strFilter = "[PrimaryMediaId] IN ( ";
		Pos = pTranscripts->GetHeadPosition();
		while(Pos != NULL)
		{
			if((pTranscript = (CTranscript*)(pTranscripts->GetNext(Pos))) != 0)
			{
				if(bIsFirst == TRUE)
					bIsFirst = FALSE;
				else
					m_strFilter += ",";

				strTemp.Format("%ld", pTranscript->m_lPrimaryMediaId);
				m_strFilter += strTemp;
			}

		}

		m_strFilter += " )";

	}
	else
	{
		m_strFilter.Empty();
	}

	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, DBNET_SECONDARY_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

//==============================================================================
//
// 	Function Name:	CDBSecondary::GetDefaultDBName()
//
// 	Description:	Called to retrieve the default database name.
//
// 	Returns:		The name of a default database.
//
//	Notes:			None
//
//==============================================================================
CString CDBSecondary::GetDefaultDBName()
{
	return _T("");
}

//==============================================================================
//
// 	Function Name:	CDBSecondary::GetDefaultSQL()
//
// 	Description:	Called to retrieve the default SQL statement.
//
// 	Returns:		Default SQL
//
//	Notes:			None
//
//==============================================================================
CString CDBSecondary::GetDefaultSQL()
{
	return _T("[SecondaryMedia]");
}

