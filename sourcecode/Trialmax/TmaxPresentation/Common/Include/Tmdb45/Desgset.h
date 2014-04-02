//==============================================================================
//
// File Name:	desgset.h
//
// Description:	This file contains the declaration of the CDesignationSet class
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
#if !defined(AFX_DESGSET_H__5F24AF41_098F_11D4_8178_00802966F8C1__INCLUDED_)
#define AFX_DESGSET_H__5F24AF41_098F_11D4_8178_00802966F8C1__INCLUDED_

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
class CDesignationSet : public CDaoRecordset
{
	private:

						DECLARE_DYNAMIC(CDesignationSet)

	public:
	
		long			GetCount();
		BOOL			FilterOnMedia(LPCSTR lpMediaId);

						CDesignationSet(CDaoDatabase* pDatabase = NULL);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Field/Param Data
	//{{AFX_FIELD(CDesignationSet, CDaoRecordset)
	CString	m_MediaID;
	long	m_DesignationID;
	long	m_TranscriptID;
	CString	m_Description;
	long	m_PlaybackOrder;
	long	m_ColorID;
	long	m_DisplayType;
	long	m_StartPage;
	long	m_StartLine;
	long	m_StopPage;
	long	m_StopLine;
	long	m_SelStart;
	long	m_SelLength;
	long	m_StartNum;
	long	m_StopNum;
	long	m_VideoFileID;
	BOOL	m_StartTuned;
	BOOL	m_StopTuned;
	BOOL	m_HasObjections;
	CString	m_OverlayFileName;
	CString	m_OverlayRelativePath;
	//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDesignationSet)
	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DESGSET_H__5F24AF41_098F_11D4_8178_00802966F8C1__INCLUDED_)

