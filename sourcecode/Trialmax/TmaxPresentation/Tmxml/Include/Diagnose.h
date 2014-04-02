//==============================================================================
//
// File Name:	diagnose.h
//
// Description:	This file contains the declaration of the CDiagnostics class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	07-14-01	1.00		Original Release
//==============================================================================
#if !defined(__DIAGNOSE_H__)
#define __DIAGNOSE_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <pprevs.h>
#include <pptmx.h>
#include <pptreat.h>
#include <ppdnload.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CDiagnostics : public CPropertySheet
{
	private:

							DECLARE_DYNAMIC(CDiagnostics)

	public:

		CPPRevisions		m_Revisions;
		CPPTmx				m_Tmx;
		CPPTreatment		m_Treatment;
		CPPDownload			m_Download;

							CDiagnostics(CWnd* pParentWnd = NULL); 
		virtual			   ~CDiagnostics();

	protected:

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Operations
	public:

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDiagnostics)
	public:
	virtual BOOL OnInitDialog();
	//}}AFX_VIRTUAL


	// Generated message map functions
	protected:
	//{{AFX_MSG(CDiagnostics)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__DIAGNOSE_H__)
