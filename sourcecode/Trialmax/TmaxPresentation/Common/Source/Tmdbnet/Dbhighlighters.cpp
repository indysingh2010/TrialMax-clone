//==============================================================================
//
// File Name:	dbhighlighters.cpp
//
// Description:	This file contains member functions of the CDBHighlighters class.
//
// See Also:	dbhighlighters.h
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
#include <dbhighlighters.h>
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
IMPLEMENT_DYNAMIC(CDBHighlighters, CDaoRecordset)

//==============================================================================
//
// 	Function Name:	CDBHighlighters::CDBHighlighters()
//
// 	Description:	This is the constructor for CDBHighlighters objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDBHighlighters::CDBHighlighters(CDaoDatabase* pDatabase) : CDaoRecordset(pDatabase)
{
	//{{AFX_FIELD_INIT(CDBHighlighters)
	m_AutoId = 0;
	m_Color = 0;
	m_GroupId = 0;
	m_Name = _T("");
	m_CreatedBy = 0;
	m_CreatedOn = (DATE)0;
	m_ModifiedBy = 0;
	m_ModifiedOn = (DATE)0;
	m_nFields = 8;
	//}}AFX_FIELD_INIT
	m_nDefaultType = dbOpenDynaset;

}

//==============================================================================
//
// 	Function Name:	CDBHighlighters::DoFieldExchange()
//
// 	Description:	This function binds the local members to the database fields
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBHighlighters::DoFieldExchange(CDaoFieldExchange* pFX)
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

	//{{AFX_FIELD_MAP(CDBHighlighters)
	pFX->SetFieldType(CDaoFieldExchange::outputColumn);
	DFX_Long(pFX, _T("[AutoId]"), m_AutoId);
	DFX_Long(pFX, _T("[Color]"), m_Color);
	DFX_Short(pFX, _T("[GroupId]"), m_GroupId);
	DFX_Text(pFX, _T("[Name]"), m_Name);
	DFX_Long(pFX, _T("[CreatedBy]"), m_CreatedBy);
	DFX_DateTime(pFX, _T("[CreatedOn]"), m_CreatedOn);
	DFX_Long(pFX, _T("[ModifiedBy]"), m_ModifiedBy);
	DFX_DateTime(pFX, _T("[ModifiedOn]"), m_ModifiedOn);
	//}}AFX_FIELD_MAP
}

//==============================================================================
//
// 	Function Name:	CDBHighlighters::FilterOnId()
//
// 	Description:	This function will filter the recordset on the numeric
//					identifier specified by the caller.
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CDBHighlighters::FilterOnId(long lId, CDBAbstract* lpDatabase)
{
	//	Build the filter
	if(lId > 0)
		m_strFilter.Format(" [AutoId] = %ld ", lId);
	else
		m_strFilter.Empty();

	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, DBNET_HIGHLIGHTERS_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

//==============================================================================
//
// 	Function Name:	CDBHighlighters::GetDefaultDBName()
//
// 	Description:	Called to retrieve the default database name.
//
// 	Returns:		The name of a default database.
//
//	Notes:			None
//
//==============================================================================
CString CDBHighlighters::GetDefaultDBName()
{
	return _T("");
}

//==============================================================================
//
// 	Function Name:	CDBHighlighters::GetDefaultSQL()
//
// 	Description:	Called to retrieve the default SQL statement.
//
// 	Returns:		Default SQL
//
//	Notes:			None
//
//==============================================================================
CString CDBHighlighters::GetDefaultSQL()
{
	return _T("[Highlighters]");
}


