//==============================================================================
//
// File Name:	dbbarcodemap.cpp
//
// Description:	This file contains member functions of the CDBBarcodeMap class.
//
// See Also:	dbbarcodemap.h
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2001
//
//==============================================================================
//	Date		Revision    Description
//	09-23-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <dbbarcodemap.h>
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
IMPLEMENT_DYNAMIC(CDBBarcodeMap, CDaoRecordset)

//==============================================================================
//
// 	Function Name:	CDBBarcodeMap::CDBBarcodeMap()
//
// 	Description:	This is the constructor for CDBBarcodeMap objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDBBarcodeMap::CDBBarcodeMap(CDaoDatabase* pDatabase) : CDaoRecordset(pDatabase)
{
	//{{AFX_FIELD_INIT(CDBBarcodeMap)
	m_PSTQ = _T("");
	m_ForeignCode = _T("");
	m_nFields = 2;
	//}}AFX_FIELD_INIT
	m_nDefaultType = dbOpenDynaset;
}

//==============================================================================
//
// 	Function Name:	CDBBarcodeMap::DoFieldExchange()
//
// 	Description:	This function binds the local members to the database fields
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDBBarcodeMap::DoFieldExchange(CDaoFieldExchange* pFX)
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

	//{{AFX_FIELD_MAP(CDBBarcodeMap)
	pFX->SetFieldType(CDaoFieldExchange::outputColumn);
	DFX_Text(pFX, _T("[PSTQ]"), m_PSTQ);
	DFX_Text(pFX, _T("[ForeignCode]"), m_ForeignCode);
	//}}AFX_FIELD_MAP
}

//==============================================================================
//
// 	Function Name:	CDBBarcodeMap::FilterOnForeign()
//
// 	Description:	This function will filter the recordset on the foreign
//					barcode specified by the caller
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CDBBarcodeMap::FilterOnForeign(LPCSTR lpszForeign, CDBAbstract* lpDatabase)
{
	//	Build the filter
	if((lpszForeign != 0) && (lstrlen(lpszForeign) > 0))
		m_strFilter.Format(" [ForeignCode] = \"%s\" ", lpszForeign);
	else
		m_strFilter.Empty();

	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, DBNET_BARCODEMAP_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

//==============================================================================
//
// 	Function Name:	CDBBarcodeMap::FilterOnPSTQ()
//
// 	Description:	This function will filter the recordset on the foreign
//					barcode specified by the caller
//
// 	Returns:		TRUE if any records are found 
//
//	Notes:			None
//
//==============================================================================
BOOL CDBBarcodeMap::FilterOnPSTQ(LPCSTR lpszPSTQ, CDBAbstract* lpDatabase)
{
	//	Build the filter
	if((lpszPSTQ != 0) && (lstrlen(lpszPSTQ) > 0))
		m_strFilter.Format(" [PSTQ] = \"%s\" ", lpszPSTQ);
	else
		m_strFilter.Empty();

	//	Reopen the recordset. This is more efficient than Requery() when the
	//	filter has changed.
	if(lpDatabase->OpenRecordset(this, DBNET_BARCODEMAP_TABLE) != TMDB_NOERROR)
		return FALSE;

	//	Did we find any records?
	return (!IsBOF());
}

//==============================================================================
//
// 	Function Name:	CDBBarcodeMap::GetDefaultDBName()
//
// 	Description:	Called to retrieve the default database name.
//
// 	Returns:		The name of a default database.
//
//	Notes:			None
//
//==============================================================================
CString CDBBarcodeMap::GetDefaultDBName()
{
	return _T("");
}

//==============================================================================
//
// 	Function Name:	CDBBarcodeMap::GetDefaultSQL()
//
// 	Description:	Called to retrieve the default SQL statement.
//
// 	Returns:		Default SQL
//
//	Notes:			None
//
//==============================================================================
CString CDBBarcodeMap::GetDefaultSQL()
{
	return _T("[BarcodeMap]");
}

