//==============================================================================
//
// File Name:	videoset.h
//
// Description:	This file contains the declaration of the CVideoSet class
//
// See Also:	
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	04-04-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_VIDEOSET_H__065832C1_0989_11D4_8178_00802966F8C1__INCLUDED_)
#define AFX_VIDEOSET_H__065832C1_0989_11D4_8178_00802966F8C1__INCLUDED_

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
class CVideoSet : public CDaoRecordset
{
	private:

							DECLARE_DYNAMIC(CVideoSet)

	public:
	
							CVideoSet(CDaoDatabase* pDatabase = NULL);

		long				GetCount();
		BOOL				FilterOnId(long lId);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Field/Param Data
	//{{AFX_FIELD(CVideoSet, CDaoRecordset)
	long	m_VideoFileID;
	long	m_TranscriptID;
	CString	m_RelativePath;
	long	m_UnitType;
	CString	m_FileName;
	long	m_BeginNum;
	long	m_EndNum;
	long	m_MinSelStart;
	long	m_MaxSelStart;
	BOOL	m_BeginTuned;
	BOOL	m_EndTuned;
	CString	m_RootOverride;
	//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CVideoSet)
	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_VIDEOSET_H__065832C1_0989_11D4_8178_00802966F8C1__INCLUDED_)
