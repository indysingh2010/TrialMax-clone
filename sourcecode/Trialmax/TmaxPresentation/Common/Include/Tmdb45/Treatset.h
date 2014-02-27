//==============================================================================
//
// File Name:	treatset.h
//
// Description:	This file contains the declaration of the CTreatmentSet class
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
#if !defined(AFX_TREATSET_H__61E38F22_FCC0_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_TREATSET_H__61E38F22_FCC0_11D3_8177_00802966F8C1__INCLUDED_

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
class CTreatmentSet : public CDaoRecordset
{
	private:

							DECLARE_DYNAMIC(CTreatmentSet)

	public:
	
							CTreatmentSet(CDaoDatabase* pDatabase = NULL);

		long				GetCount();
		BOOL				FilterOnMedia(LPCSTR lpMediaId);
		BOOL				FilterOnPage(LPCSTR lpMediaId, long lPageId);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Field/Param Data
	//{{AFX_FIELD(CTreatmentSet, CDaoRecordset)
	CString	m_MediaID;
	long	m_PageID;
	long	m_TreatmentID;
	CString	m_Description;
	long	m_PlaybackOrder;
	CString	m_RelativePath;
	CString	m_FileName;
	//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTreatmentSet)
	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL

};

#endif // !defined(AFX_TREATSET_H__61E38F22_FCC0_11D3_8177_00802966F8C1__INCLUDED_)
