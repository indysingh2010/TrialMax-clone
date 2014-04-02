//==============================================================================
//
// File Name:	propsht.h
//
// Description:	This file contains the declaration of the CProperties class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	05-20-01	1.00		Original Release
//==============================================================================
#if !defined(__PROPSHT_H__)
#define __PROPSHT_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <ppimage.h>
#include <pptmx.h>
#include <ppmedia.h>
#include <ppabout.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CProperties : public CPropertySheet
{
	private:

							DECLARE_DYNAMIC(CProperties)

	public:

		CPPImage			m_Image;
		CPPMedia			m_Media;
		CPPAbout			m_About;

							CProperties(CWnd* pParentWnd = NULL); 
		virtual			   ~CProperties();

	protected:

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Operations
	public:

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CProperties)
	public:
	virtual BOOL OnInitDialog();
	//}}AFX_VIRTUAL


	// Generated message map functions
	protected:
	//{{AFX_MSG(CProperties)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__PROPSHT_H__)
