//==============================================================================
//
// File Name:	dbquaternary.h
//
// Description:	This file contains the declaration of the CDBQuaternary class
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
#if !defined(__DBQUATERNARY_H__)
#define __DBQUATERNARY_H__

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

class CDBQuaternary : public CDaoRecordset
{
	private:

						DECLARE_DYNAMIC(CDBQuaternary)

	public:
	
		BOOL			FilterOnId(long lId, CDBAbstract* lpDatabase);
		BOOL			FilterOnTertiaryId(long lId, CDBAbstract* lpDatabase);

						CDBQuaternary(CDaoDatabase* pDatabase = NULL);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Field/Param Data
	//{{AFX_FIELD(CDBQuaternary, CDaoRecordset)
	long	m_AutoId;
	long	m_TertiaryMediaId;
	long	m_BarcodeId;
	long	m_Attributes;
	short	m_MediaType;
	CString	m_SourceId;
	short	m_SourceType;
	CString	m_Description;
	CString	m_Name;
	long	m_DisplayOrder;
	long	m_StartPL;
	double	m_Start;
	BOOL	m_StartTuned;
	long	m_CreatedBy;
	COleDateTime	m_CreatedOn;
	long	m_ModifiedBy;
	COleDateTime	m_ModifiedOn;
	//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDBQuaternary)
	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__DBQUATERNARY_H__)
