//==============================================================================
//
// File Name:	dbextents.h
//
// Description:	This file contains the declaration of the CDBExtents class
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
#if !defined(__DBEXTENTS_H__)
#define __DBEXTENTS_H__

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

class CDBExtents : public CDaoRecordset
{
	private:

						DECLARE_DYNAMIC(CDBExtents)

	public:
	
		BOOL			FilterOnId(long lId, CDBAbstract* lpDatabase);
		BOOL			FilterOnSecondaryId(long lId, CDBAbstract* lpDatabase);
		BOOL			FilterOnTertiaryId(long lId, CDBAbstract* lpDatabase);
		BOOL			FilterOnBoth(long lSecondaryId, long lTertiaryId, CDBAbstract* lpDatabase);

						CDBExtents(CDaoDatabase* pDatabase = NULL);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Field/Param Data
	//{{AFX_FIELD(CDBExtents, CDaoRecordset)
	long	m_AutoId;
	long	m_SecondaryId;
	long	m_TertiaryId;
	long	m_XmlSegmentId;
	long	m_HighlighterId;
	double	m_Start;
	double	m_Stop;
	BOOL	m_StartTuned;
	BOOL	m_StopTuned;
	long	m_StartPL;
	long	m_StopPL;
	//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDBExtents)
	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__DBEXTENTS_H__)
