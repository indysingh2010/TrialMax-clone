//==============================================================================
//
// File Name:	dbtranscripts.h
//
// Description:	This file contains the declaration of the CDBTranscripts class
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
#if !defined(__DBTRANSCRIPTS_H__)
#define __DBTRANSCRIPTS_H__

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

class CDBTranscripts : public CDaoRecordset
{
	private:

						DECLARE_DYNAMIC(CDBTranscripts)

	public:
	
		BOOL			FilterOnId(long lId, CDBAbstract* lpDatabase);
		BOOL			FilterOnPrimaryId(long lId, CDBAbstract* lpDatabase);

						CDBTranscripts(CDaoDatabase* pDatabase = NULL);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Field/Param Data
	//{{AFX_FIELD(CDBTranscripts, CDaoRecordset)
	long	m_AutoId;
	long	m_PrimaryId;
	CString	m_Deponent;
	CString	m_DeposedOn;
	CString	m_Filename;
	long	m_FirstPL;
	long	m_LastPL;
	short	m_LinesPerPage;
	//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDBTranscripts)
	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__DBTRANSCRIPTS_H__)
