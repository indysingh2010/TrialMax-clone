//==============================================================================
//
// File Name:	transet.h
//
// Description:	This file contains the declaration of the CTranscriptSet class
//
// See Also:	
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	04-20-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_TRANSET_H__5C9CAEA1_17A2_11D4_8178_00802966F8C1__INCLUDED_)
#define AFX_TRANSET_H__5C9CAEA1_17A2_11D4_8178_00802966F8C1__INCLUDED_

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
class CTranscriptSet : public CDaoRecordset
{
	private:

						DECLARE_DYNAMIC(CTranscriptSet)

	public:
	
		long			GetCount();
		BOOL			FilterOnId(long lId);

						CTranscriptSet(CDaoDatabase* pDatabase = NULL);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Field/Param Data
	//{{AFX_FIELD(CTranscriptSet, CDaoRecordset)
	long	m_TranscriptID;
	CString	m_TranscriptName;
	COleDateTime	m_TranscriptDate;
	CString	m_RelativePath;
	CString	m_BaseFileName;
	CString	m_OrigCtxFileExtension;
	CString	m_OrigLogDbFileExtension;
	//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTranscriptSet)
	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TRANSET_H__5C9CAEA1_17A2_11D4_8178_00802966F8C1__INCLUDED_)
