//==============================================================================
//
// File Name:	dbtertiary.h
//
// Description:	This file contains the declaration of the CDBTertiary class
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
#if !defined(__DBTERTIARY_H__)
#define __DBTERTIARY_H__

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

class CDBTertiary : public CDaoRecordset
{
	private:

						DECLARE_DYNAMIC(CDBTertiary)

		BOOL			m_bSplitScreenTreatments;

	public:
	
		BOOL			FilterOnId(long lId, CDBAbstract* lpDatabase);
		BOOL			FilterOnSecondaryId(long lId, long lBarcodeId, CDBAbstract* lpDatabase);

						CDBTertiary(CDaoDatabase* pDatabase = NULL);

		void			SetSplitScreenTreatments(BOOL bEnabled);
		BOOL			GetSplitScreenTreatments(){ return m_bSplitScreenTreatments; }



	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Field/Param Data
	//{{AFX_FIELD(CDBTertiary, CDaoRecordset)
	long	m_AutoId;
	long	m_SecondaryMediaId;
	long	m_BarcodeId;
	long	m_Children;
	long	m_Attributes;
	short	m_MediaType;
	CString	m_Filename;
	CString	m_SourceId;
	short	m_SourceType;
	CString	m_Description;
	CString	m_Name;
	long	m_DisplayOrder;
	long	m_CreatedBy;
	COleDateTime	m_CreatedOn;
	long	m_ModifiedBy;
	COleDateTime	m_ModifiedOn;
	CString	m_SiblingId;
	//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDBTertiary)
	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__DBTERTIARY_H__)
