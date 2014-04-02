//==============================================================================
//
// File Name:	mediaset.h
//
// Description:	This file contains the declaration of the CMediaSet class
//
// See Also:	
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-02-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_MEDIASET_H__9E0FB5C1_F061_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_MEDIASET_H__9E0FB5C1_F061_11D3_8177_00802966F8C1__INCLUDED_

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
class CMediaSet : public CDaoRecordset
{
	private:

						DECLARE_DYNAMIC(CMediaSet)

	public:
	
		long			GetCount();
		BOOL			FilterOnGhost(long lId);
		BOOL			FilterOnId(LPCSTR lpId);

						CMediaSet(CDaoDatabase* pDatabase = NULL);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Field/Param Data
	//{{AFX_FIELD(CMediaSet, CDaoRecordset)
	CString	m_MediaID;
	long	m_GhostMediaId;
	long	m_MediaPlayerType;
	CString	m_MediaName;
	CString	m_RelativePath;
	CString	m_FileName;
	long	m_FlagsBinary;
	BOOL	m_UseTranscript;
	//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMediaSet)
	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_MEDIASET_H__9E0FB5C1_F061_11D3_8177_00802966F8C1__INCLUDED_)
