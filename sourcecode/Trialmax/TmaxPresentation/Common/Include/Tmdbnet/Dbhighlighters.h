//==============================================================================
//
// File Name:	dbhighlighters.h
//
// Description:	This file contains the declaration of the CDBHighlighters class
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
#if !defined(__DBHIGHLIGHTERS_H__)
#define __DBHIGHLIGHTERS_H__

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

class CDBHighlighters : public CDaoRecordset
{
	private:

						DECLARE_DYNAMIC(CDBHighlighters)

	public:
	
		BOOL			FilterOnId(long lId, CDBAbstract* lpDatabase);

						CDBHighlighters(CDaoDatabase* pDatabase = NULL);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Field/Param Data
	//{{AFX_FIELD(CDBHighlighters, CDaoRecordset)
	long	m_AutoId;
	long	m_Color;
	short	m_GroupId;
	CString	m_Name;
	long	m_CreatedBy;
	COleDateTime	m_CreatedOn;
	long	m_ModifiedBy;
	COleDateTime	m_ModifiedOn;
	//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDBHighlighters)
	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__DBHIGHLIGHTERS_H__)
