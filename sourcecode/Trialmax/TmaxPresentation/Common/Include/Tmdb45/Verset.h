//==============================================================================
//
// File Name:	verset.h
//
// Description:	This file contains the declaration of the CVersionSet class
//
// See Also:	
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-18-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_VERSET_H__366F0C41_FCAF_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_VERSET_H__366F0C41_FCAF_11D3_8177_00802966F8C1__INCLUDED_

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
class CVersionSet : public CDaoRecordset
{
	private:

							DECLARE_DYNAMIC(CVersionSet)

	public:
	
							CVersionSet(CDaoDatabase* pDatabase = NULL);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Field/Param Data
	//{{AFX_FIELD(CVersionSet, CDaoRecordset)
	CString	m_CreatedBy;
	CString	m_CurrentTrialMaxDbVersion;
	CString	m_CurrentViewerDbVersion;
	long	m_Key;
	CString	m_OriginalTrialMaxDbVersion;
	//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CVersionSet)
	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_VERSET_H__366F0C41_FCAF_11D3_8177_00802966F8C1__INCLUDED_)
