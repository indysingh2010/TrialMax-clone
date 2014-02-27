//==============================================================================
//
// File Name:	dbsecondary.h
//
// Description:	This file contains the declaration of the CDBSecondary class
//
// See Also:	
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2003
//
//==============================================================================
//	Date		Revision    Description
//	12-26-2003	1.00		Original Release
//==============================================================================
#if !defined(__DBSECONDARY_H__)
#define __DBSECONDARY_H__

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
class CDBAbstract;
class CMedia;
class CMedias;
class CTranscript;
class CTranscripts;
class CSecondary;
class CShowItem;

class CDBSecondary : public CDaoRecordset
{
	private:

						DECLARE_DYNAMIC(CDBSecondary)

	public:
	
		BOOL			FilterOnId(long lId, CDBAbstract* lpDatabase);
		BOOL			FilterOnPrimaryId(long lId, CDBAbstract* lpDatabase);
		BOOL			FilterOnDepositions(CMedias* pMedias, CDBAbstract* lpDatabase);
		BOOL			FilterOnTranscripts(CTranscripts* pTranscripts, CDBAbstract* lpDatabase);
		BOOL			Read(CSecondary* pSecondary);
		BOOL			Read(CShowItem*	pShowItem);

						CDBSecondary(CDaoDatabase* pDatabase = NULL);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Field/Param Data
	//{{AFX_FIELD(CDBSecondary, CDaoRecordset)
	long	m_AutoId;
	long	m_PrimaryMediaId;
	long	m_BarcodeId;
	long	m_Children;
	long	m_Attributes;
	short	m_MediaType;
	short	m_SourceMediaType;
	short	m_TransitionTime;
	CString	m_SourcePST;
	CString	m_Filename;
	long	m_MultipageId;
	CString	m_Description;
	CString	m_Name;
	long	m_DisplayOrder;
	long	m_CreatedBy;
	COleDateTime	m_CreatedOn;
	long	m_ModifiedBy;
	COleDateTime	m_ModifiedOn;
	long	m_AliasId;
	CString	m_RelativePath;
	//}}AFX_FIELD

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDBSecondary)
	public:
	virtual CString GetDefaultDBName();		// Default database name
	virtual CString GetDefaultSQL();		// Default SQL for Recordset
	virtual void DoFieldExchange(CDaoFieldExchange* pFX);  // RFX support
	//}}AFX_VIRTUAL
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__DBSECONDARY_H__)
