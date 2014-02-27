//==============================================================================
//
// File Name:	dbbarcodemap.h
//
// Description:	This file contains the declaration of the CDBBarcodeMap class
//
// See Also:	
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2004
//
//==============================================================================
//	Date		Revision    Description
//	12-26-2003	1.00		Original Release
//==============================================================================
#if !defined(__DBBARCODEMAP_H__)
#define __DBBARCODEMAP_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CDBAbstract;

class CDBBarcodeMap : public CDaoRecordset
{
	private:

						DECLARE_DYNAMIC(CDBBarcodeMap)

	public:
	
		BOOL			FilterOnPSTQ(LPCSTR lpszPSTQ, CDBAbstract* lpDatabase);
		BOOL			FilterOnForeign(LPCSTR lpszForeign, CDBAbstract* lpDatabase);

						CDBBarcodeMap(CDaoDatabase* pDatabase = NULL);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Field/Param Data
	//{{AFX_FIELD(CDBBarcodeMap, CDaoRecordset)
	CString	m_PSTQ;
	CString	m_ForeignCode;
	//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDBBarcodeMap)
	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__DBBARCODEMAP_H__)
