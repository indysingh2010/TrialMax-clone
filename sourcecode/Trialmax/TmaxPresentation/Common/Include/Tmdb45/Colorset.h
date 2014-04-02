//==============================================================================
//
// File Name:	colorset.h
//
// Description:	This file contains the declaration of the CColorSet class
//
// See Also:	
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	11-20-03	1.00		Original Release
//==============================================================================
#if !defined(AFX_COLORSET_H__43966D68_3235_4E4A_9589_C81F4384927F__INCLUDED_)
#define AFX_COLORSET_H__43966D68_3235_4E4A_9589_C81F4384927F__INCLUDED_

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
class CColorSet : public CDaoRecordset
{
	private:

						DECLARE_DYNAMIC(CColorSet)

	public:
	
						CColorSet(CDaoDatabase* pDatabase = NULL);

		long			GetCount();
		BOOL			FilterOnId(long lId);
		BOOL			GetColor(long lID, COLORREF* pcrColor);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Field/Param Data
	//{{AFX_FIELD(CColorSet, CDaoRecordset)
	long	m_ColorID;
	CString	m_Description;
	long	m_ColorRGB;
	CString	m_PlaintiffDefendant;
	//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CColorSet)
	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_COLORSET_H__43966D68_3235_4E4A_9589_C81F4384927F__INCLUDED_)

