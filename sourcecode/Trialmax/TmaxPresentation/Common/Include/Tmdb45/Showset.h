//==============================================================================
//
// File Name:	showset.h
//
// Description:	This file contains the declaration of the CShowItemSet class
//
// See Also:	
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	04-27-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_SHOWSET_H__EC852661_1C1F_11D4_8178_00802966F8C1__INCLUDED_)
#define AFX_SHOWSET_H__EC852661_1C1F_11D4_8178_00802966F8C1__INCLUDED_

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
class CShowItemSet : public CDaoRecordset
{
	private:

						DECLARE_DYNAMIC(CShowItemSet)

	public:
	
		long			GetCount();
		BOOL			FilterOnMedia(LPCSTR lpMediaId);

						CShowItemSet(CDaoDatabase* pDatabase = NULL);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Field/Param Data
	//{{AFX_FIELD(CShowItemSet, CDaoRecordset)
	CString	m_MediaID;
	long	m_ShowItemID;
	CString	m_Description;
	long	m_PlaybackOrder;
	CString	m_ItemBarcode;
	BOOL	m_Hide;
	//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CShowItemSet)
	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SHOWSET_H__EC852661_1C1F_11D4_8178_00802966F8C1__INCLUDED_)
