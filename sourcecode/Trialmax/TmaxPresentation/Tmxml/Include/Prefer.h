//==============================================================================
//
// File Name:	prefer.h
//
// Description:	This file contains the declaration of the CPreferences class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	03-10-01	1.00		Original Release
//==============================================================================
#if !defined(__PREFER_H__)
#define __PREFER_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <ppviewer.h>
#include <pptool.h>
#include <ppprint.h>
#include <pptools.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CPreferences : public CPropertySheet
{
	private:

							DECLARE_DYNAMIC(CPreferences)

	public:

		CPPViewer			m_Viewer;
		CPPToolbar			m_Toolbar;
		CPPPrint			m_Printer;
		CPPTools			m_Tools;

							CPreferences(CWnd* pParentWnd = NULL, int iPage = 0); 
		virtual			   ~CPreferences();

	protected:

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Operations
	public:

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CPreferences)
	//}}AFX_VIRTUAL


	// Generated message map functions
	protected:
	//{{AFX_MSG(CPreferences)
		// NOTE - the ClassWizard will add and remove member functions here.
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__PREFER_H__)
