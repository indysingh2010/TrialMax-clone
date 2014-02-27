//==============================================================================
//
// File Name:	dbprimary.cpp
//
// Description:	This file contains member functions of the CDBPrimary class.
//
// See Also:	dbprimary.h
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
#include <dbprimary.h>
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
IMPLEMENT_DYNAMIC(CDBPrimary, CDaoRecordset)

//==============================================================================
//
// 	Function Name:	CDBPrimary::CDBPrimary()
//
// 	Description:	This is the constructor for CDBPrimary objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDBPrimary::CDBPrimary(CDaoDatabase* pDatabase) : CDaoRecordset(pDatabase)
{
	//{{AFX_FIELD_INIT(CDBPrimary)
	m_AutoId = 0;
	m_Children = 0;
	m_Attributes = 0;
	m_MediaType = 0;
	m_MediaId = _T("");
	m_Exhibit = _T("");
	m_Filename = _T("");
	m_Description = _T("");
	m_AltBarcode = _T("");
	m_Name = _T("");
	m_CreatedBy = 0;
	m_CreatedOn = (DATE)0;
	m_ModifiedBy = 0;
	m_ModifiedOn = (DATE)0;
	m_AliasId = 0;
	m_RegisterPath = _T("");
	m_RelativePath = _T("");
	m_nFields = 17;
	//}}AFX_FIELD_INIT
	m_nDefaultType = dbOpenDynaset;

	//	Initialize to sort the records on user defined media id
	//m_strSort = "MediaId";
}

//==============================================================================
//
// 	Function Name:	CDBPrimary::DoFieldExchange()
//
// 	Description:	This function binds the local members to the database fields
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBPrimary::DoFieldExchange(CDaoFieldExchange* pFX)
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

	//{{AFX_FIELD_MAP(CDBPrimary)
	pFX->SetFieldType(CDaoFieldExchange::outputColumn);
	DFX_Long(pFX, _T("[AutoId]"), m_AutoId);
	DFX_Long(pFX, _T("[Children]"), m_Children);
	DFX_Long(pFX, _T("[Attributes]"), m_Attributes);
	DFX_Short(pFX, _T("[MediaType]"), m_MediaType);
	DFX_Text(pFX, _T("[MediaId]"), m_MediaId);
	DFX_Text(pFX, _T("[Exhibit]"), m_Exhibit);
	DFX_Text(pFX, _T("[Filename]"), m_Filename);
	DFX_Text(pFX, _T("[Description]"), m_Description);
	DFX_Text(pFX, _T("[AltBarcode]"), m_AltBarcode);
	DFX_Text(pFX, _T("[Name]"), m_Name);
	DFX_Long(pFX, _T("[CreatedBy]"), m_CreatedBy);
	DFX_DateTime(pFX, _T("[CreatedOn]"), m_CreatedOn);
	DFX_Long(pFX, _T("[ModifiedBy]"), m_ModifiedBy);
	DFX_DateTime(pFX, _T("[ModifiedOn]"), m_ModifiedOn);
	DFX_Long(pFX, _T("[AliasId]"), m_AliasId);
	DFX_Text(pFX, _T("[RegisterPath]"), m_RegisterPath);
	DFX_Text(pFX, _T("[RelativePath]"), m_RelativePath);
	//}}AFX_FIELD_MAP
}

//==============================================================================
//
// 	Function Name:	CDBPrimary::FilterOnId()
//
// 	Description:	This function will filter the recordset on the numeric
//					identifier specified by the caller.
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CDBPrimary::FilterOnId(long lId, CDBAbstract* lpDatabase)
{
	//	Build the filter
	if(lId > 0)
		m_strFilter.Format(" [AutoId] = %ld ", lId);
	else
		m_strFilter.Empty();

	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, DBNET_PRIMARY_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

//==============================================================================
//
// 	Function Name:	CDBPrimary::FilterOnMediaId()
//
// 	Description:	This function will filter the recordset on the text
//					identifier specified by the caller.
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CDBPrimary::FilterOnMediaId(LPCSTR lpId, CDBAbstract* lpDatabase)
{
	//	Build the filter
	if((lpId != 0) && (lstrlen(lpId) > 0))
		m_strFilter.Format(" [MediaId] = \"%s\" ", lpId);
	else
		m_strFilter.Empty();

	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, DBNET_PRIMARY_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

//==============================================================================
//
// 	Function Name:	CDBPrimary::GetDefaultDBName()
//
// 	Description:	Called to retrieve the default database name.
//
// 	Returns:		The name of a default database.
//
//	Notes:			None
//
//==============================================================================
CString CDBPrimary::GetDefaultDBName()
{
	return _T("");
}

//==============================================================================
//
// 	Function Name:	CDBPrimary::GetDefaultSQL()
//
// 	Description:	Called to retrieve the default SQL statement.
//
// 	Returns:		Default SQL
//
//	Notes:			None
//
//==============================================================================
CString CDBPrimary::GetDefaultSQL()
{
	return _T("[PrimaryMedia]");
}
