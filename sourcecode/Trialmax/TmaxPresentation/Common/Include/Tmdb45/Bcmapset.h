//==============================================================================
//
// File Name:	bcmapset.h
//
// Description:	This file contains the declaration of the CBarcodeMapSet class
//
// See Also:	
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2001
//
//==============================================================================
//	Date		Revision    Description
//	09-23-01	1.00		Original Release
//==============================================================================
#if !defined(AFX_BCMAPSET_H__95747B43_B00D_11D5_8F0A_00802966F8C1__INCLUDED_)
#define AFX_BCMAPSET_H__95747B43_B00D_11D5_8F0A_00802966F8C1__INCLUDED_

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
class CBarcodeMapSet : public CDaoRecordset
{
	private:

						DECLARE_DYNAMIC(CBarcodeMapSet)

	public:
	
		long			GetCount();
		BOOL			FilterOnForeignCode(LPCSTR lpCode);

						CBarcodeMapSet(CDaoDatabase* pDatabase = NULL);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Field/Param Data
	//{{AFX_FIELD(CBarcodeMapSet, CDaoRecordset)
	CString	m_ForeignCode;
	CString	m_Barcode;
	//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CBarcodeMapSet)
	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_BCMAPSET_H__95747B43_B00D_11D5_8F0A_00802966F8C1__INCLUDED_)
