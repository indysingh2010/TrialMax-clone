//==============================================================================
//
// File Name:	linkset.h
//
// Description:	This file contains the declaration of the CLinkSet class
//
// See Also:	
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	03-18-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_LINKSET_H__0E3B2A61_0A10_11D4_8178_00802966F8C1__INCLUDED_)
#define AFX_LINKSET_H__0E3B2A61_0A10_11D4_8178_00802966F8C1__INCLUDED_

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
class CLinkSet : public CDaoRecordset
{
	private:

							DECLARE_DYNAMIC(CLinkSet)

	public:
	
							CLinkSet(CDaoDatabase* pDatabase = NULL);

		long				GetCount();
		BOOL				FilterOnMedia(LPCSTR lpMediaId);
		BOOL				FilterOnDesignation(LPCSTR lpMediaId, long lDesignationId);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Field/Param Data
	//{{AFX_FIELD(CLinkSet, CDaoRecordset)
	long	m_LinkID;
	CString	m_MediaID;
	long	m_DesignationID;
	long	m_DisplayType;
	long	m_PageNum;
	long	m_LineNum;
	long	m_TriggerNum;
	CString	m_ItemBarcode;
	BOOL	m_HideLink;
	BOOL	m_SplitScreen;
	//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CLinkSet)
	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL

};

#endif // !defined(AFX_LINKSET_H__0E3B2A61_0A10_11D4_8178_00802966F8C1__INCLUDED_)
