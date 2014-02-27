//==============================================================================
//
// File Name:	textset.h
//
// Description:	This file contains the declaration of the CTextSet class
//
// See Also:	
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	04-07-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_TEXTSET_H__F5E87401_0C61_11D4_8178_00802966F8C1__INCLUDED_)
#define AFX_TEXTSET_H__F5E87401_0C61_11D4_8178_00802966F8C1__INCLUDED_

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
class CTextSet : public CDaoRecordset
{
	private:

							DECLARE_DYNAMIC(CTextSet)

	public:
	
							CTextSet(CDaoDatabase* pDatabase = NULL);

		long				GetCount();
		BOOL				FilterOnMedia(LPCSTR lpMediaId);
		BOOL				FilterOnDesignation(LPCSTR lpMediaId, long lDesignationId);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Field/Param Data
	//{{AFX_FIELD(CTextSet, CDaoRecordset)
	CString	m_MediaID;
	long	m_DesignationID;
	long	m_PageNum;
	long	m_LineNum;
	CString	m_TextLine;
	long	m_FirstNum;
	long	m_LastNum;
	//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTextSet)
	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL

};

#endif // !defined(AFX_TEXTSET_H__F5E87401_0C61_11D4_8178_00802966F8C1__INCLUDED_)
