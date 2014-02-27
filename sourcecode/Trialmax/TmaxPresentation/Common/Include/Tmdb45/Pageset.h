//==============================================================================
//
// File Name:	pageset.h
//
// Description:	This file contains the declaration of the CPageSet class
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
#if !defined(AFX_PAGESET_H__61E38F21_FCC0_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_PAGESET_H__61E38F21_FCC0_11D3_8177_00802966F8C1__INCLUDED_

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
class CPageSet : public CDaoRecordset
{
	private:

						DECLARE_DYNAMIC(CPageSet)

	public:
	
		long			GetCount();
		BOOL			FilterOnMedia(LPCSTR lpMediaId);

						CPageSet(CDaoDatabase* pDatabase = NULL);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Field/Param Data
	//{{AFX_FIELD(CPageSet, CDaoRecordset)
	CString	m_MediaID;
	long	m_PageID;
	long	m_PlaybackOrder;
	long	m_DisplayType;
	CString	m_FileName;
	long	m_SlideID;
	//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CPageSet)
	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_PAGESET_H__61E38F21_FCC0_11D3_8177_00802966F8C1__INCLUDED_)
