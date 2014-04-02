//==============================================================================
//
// File Name:	document.h
//
// Description:	This file contains the declaration of the CTMDocument class.
//				This is the application's base document.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	08-17-97	1.00		Original Release
//==============================================================================
#if !defined(AFX_DOCUMENT_H__AA00514F_16FD_11D1_B02E_008029EFD140__INCLUDED_)
#define AFX_DOCUMENT_H__AA00514F_16FD_11D1_B02E_008029EFD140__INCLUDED_

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
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMDocument : public CDocument
{
	private:

	public:

						DECLARE_DYNCREATE(CTMDocument)

						CTMDocument();
					   ~CTMDocument();

	protected:


	//	Class Wizard maintained
	//{{AFX_VIRTUAL(CTMDocument)
	public:
	//}}AFX_VIRTUAL

	protected:
	//{{AFX_MSG(CTMDocument)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	// Generated OLE dispatch map functions
	//{{AFX_DISPATCH(CTMDocument)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DISPATCH
	DECLARE_DISPATCH_MAP()
	DECLARE_INTERFACE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DOCUMENT_H__AA00514F_16FD_11D1_B02E_008029EFD140__INCLUDED_)
