//==============================================================================
//
// File Name:	dbdetails.h
//
// Description:	This file contains the declaration of the CDBDetails class
//
// See Also:	
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2003
//
//==============================================================================
//	Date		Revision    Description
//	12-26-2003	1.00		Original Release
//==============================================================================
#if !defined(__DBDETAILS_H__)
#define __DBDETAILS_H__

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
class CDBDetails : public CDaoRecordset
{
	private:

						DECLARE_DYNAMIC(CDBDetails)

	public:
	

						CDBDetails(CDaoDatabase* pDatabase = NULL);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Field/Param Data
	//{{AFX_FIELD(CDBDetails, CDaoRecordset)
	long	m_AutoId;
	CString	m_MasterId;
	long	m_DbMajor;
	long	m_DbMinor;
	long	m_DbBuild;
	CString	m_Name;
	CString	m_Description;
	long	m_CreatedBy;
	COleDateTime	m_CreatedOn;
	//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDBDetails)
	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__DBDETAILS_H__)
