//==============================================================================
//
// File Name:	dbprimary.h
//
// Description:	This file contains the declaration of the CDBPrimary class
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
#if !defined(__DBPRIMARY_H__)
#define __DBPRIMARY_H__

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

class CDBPrimary : public CDaoRecordset
{
	private:

						DECLARE_DYNAMIC(CDBPrimary)

	public:
	
		BOOL			FilterOnId(long lId, CDBAbstract* lpDatabase);
		BOOL			FilterOnMediaId(LPCSTR lpId, CDBAbstract* lpDatabase);

						CDBPrimary(CDaoDatabase* pDatabase = NULL);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Field/Param Data
	//{{AFX_FIELD(CDBPrimary, CDaoRecordset)
	long	m_AutoId;
	long	m_Children;
	long	m_Attributes;
	short	m_MediaType;
	CString	m_MediaId;
	CString	m_Exhibit;
	CString	m_Filename;
	CString	m_Description;
	CString	m_AltBarcode;
	CString	m_Name;
	long	m_CreatedBy;
	COleDateTime	m_CreatedOn;
	long	m_ModifiedBy;
	COleDateTime	m_ModifiedOn;
	long	m_AliasId;
	CString	m_RegisterPath;
	CString	m_RelativePath;
	//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDBPrimary)
	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__DBPRIMARY_H__)
