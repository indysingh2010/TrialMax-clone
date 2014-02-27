//==============================================================================
//
// File Name:	mediaset.cpp
//
// Description:	This file contains member functions of the CVersionSet class.
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
#include <verset.h>
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
IMPLEMENT_DYNAMIC(CVersionSet, CDaoRecordset)

//==============================================================================
//
// 	Function Name:	CVersionSet::CVersionSet()
//
// 	Description:	This is the constructor for CVersionSet objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CVersionSet::CVersionSet(CDaoDatabase* pDatabase) : CDaoRecordset(pDatabase)
{
	//{{AFX_FIELD_INIT(CVersionSet)
	m_CreatedBy = _T("");
	m_CurrentTrialMaxDbVersion = _T("");
	m_CurrentViewerDbVersion = _T("");
	m_Key = 0;
	m_OriginalTrialMaxDbVersion = _T("");
	m_nFields = 5;
	//}}AFX_FIELD_INIT
	m_nDefaultType = dbOpenDynaset;
}

//==============================================================================
//
// 	Function Name:	CVersionSet::DoFieldExchange()
//
// 	Description:	This function binds the local members to the database fields
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CVersionSet::DoFieldExchange(CDaoFieldExchange* pFX)
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

	//{{AFX_FIELD_MAP(CVersionSet)
	pFX->SetFieldType(CDaoFieldExchange::outputColumn);
	DFX_Text(pFX, _T("[CreatedBy]"), m_CreatedBy);
	DFX_Text(pFX, _T("[CurrentTrialMaxDbVersion]"), m_CurrentTrialMaxDbVersion);
	DFX_Text(pFX, _T("[CurrentViewerDbVersion]"), m_CurrentViewerDbVersion);
	DFX_Long(pFX, _T("[Key]"), m_Key);
	DFX_Text(pFX, _T("[OriginalTrialMaxDbVersion]"), m_OriginalTrialMaxDbVersion);
	//}}AFX_FIELD_MAP
}

//==============================================================================
//
// 	Function Name:	CVersionSet::GetDefaultDBName()
//
// 	Description:	Called to retrieve the default database name.
//
// 	Returns:		The name of a default database.
//
//	Notes:			None
//
//==============================================================================
CString CVersionSet::GetDefaultDBName()
{
	return _T("");
}

//==============================================================================
//
// 	Function Name:	CVersionSet::GetDefaultSQL()
//
// 	Description:	Called to retrieve the default SQL statement.
//
// 	Returns:		Default SQL
//
//	Notes:			None
//
//==============================================================================
CString CVersionSet::GetDefaultSQL()
{
	return _T("[Version]");
}


