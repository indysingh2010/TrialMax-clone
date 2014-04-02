//==============================================================================
//
// File Name:	dbprimary.cpp
//
// Description:	This file contains member functions of the CDBDetails class.
//
// See Also:	dbdetails.h
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
#include <dbdetails.h>
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
IMPLEMENT_DYNAMIC(CDBDetails, CDaoRecordset)

//==============================================================================
//
// 	Function Name:	CDBDetails::CDBDetails()
//
// 	Description:	This is the constructor for CDBDetails objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDBDetails::CDBDetails(CDaoDatabase* pDatabase) : CDaoRecordset(pDatabase)
{
	//{{AFX_FIELD_INIT(CDBDetails)
	m_AutoId = 0;
	m_MasterId = _T("");
	m_DbMajor = 0;
	m_DbMinor = 0;
	m_DbBuild = 0;
	m_Name = _T("");
	m_Description = _T("");
	m_CreatedBy = 0;
	m_CreatedOn = (DATE)0;
	m_nFields = 9;
	//}}AFX_FIELD_INIT
	m_nDefaultType = dbOpenDynaset;
}

//==============================================================================
//
// 	Function Name:	CDBDetails::DoFieldExchange()
//
// 	Description:	This function binds the local members to the database fields
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBDetails::DoFieldExchange(CDaoFieldExchange* pFX)
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

	//{{AFX_FIELD_MAP(CDBDetails)
	pFX->SetFieldType(CDaoFieldExchange::outputColumn);
	DFX_Long(pFX, _T("[AutoId]"), m_AutoId);
	DFX_Text(pFX, _T("[MasterId]"), m_MasterId);
	DFX_Long(pFX, _T("[DbMajor]"), m_DbMajor);
	DFX_Long(pFX, _T("[DbMinor]"), m_DbMinor);
	DFX_Long(pFX, _T("[DbBuild]"), m_DbBuild);
	DFX_Text(pFX, _T("[Name]"), m_Name);
	DFX_Text(pFX, _T("[Description]"), m_Description);
	DFX_Long(pFX, _T("[CreatedBy]"), m_CreatedBy);
	DFX_DateTime(pFX, _T("[CreatedOn]"), m_CreatedOn);
	//}}AFX_FIELD_MAP
}

//==============================================================================
//
// 	Function Name:	CDBDetails::GetDefaultDBName()
//
// 	Description:	Called to retrieve the default database name.
//
// 	Returns:		The name of a default database.
//
//	Notes:			None
//
//==============================================================================
CString CDBDetails::GetDefaultDBName()
{
	return _T("");
}

//==============================================================================
//
// 	Function Name:	CDBDetails::GetDefaultSQL()
//
// 	Description:	Called to retrieve the default SQL statement.
//
// 	Returns:		Default SQL
//
//	Notes:			None
//
//==============================================================================
CString CDBDetails::GetDefaultSQL()
{
	return _T("[Details]");
}

