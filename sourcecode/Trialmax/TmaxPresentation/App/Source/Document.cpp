//==============================================================================
//
// File Name:	document.cpp
//
// Description:	This file contains member functions of the CTMDocument class.
//
// Functions:   CTMDocument::CTMDocument()
//				CTMDocument::~CTMDocument()
//				
// See Also:	document.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	08-17-97	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <app.h>
#include <document.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------
// Note: we add support for IID_ITmaxPresentation to support typesafe binding
//  from VBA.  This IID must match the GUID that is attached to the 
//  dispinterface in the .ODL file.

// {1639AC24-53BD-11D1-B0A9-008029EFD140}
static const IID IID_ITmaxPresentation =
{ 0x1639ac24, 0x53bd, 0x11d1, { 0xb0, 0xa9, 0x0, 0x80, 0x29, 0xef, 0xd1, 0x40 } };

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
IMPLEMENT_DYNCREATE(CTMDocument, CDocument)

BEGIN_MESSAGE_MAP(CTMDocument, CDocument)
	//{{AFX_MSG_MAP(CTMDocument)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

BEGIN_DISPATCH_MAP(CTMDocument, CDocument)
	//{{AFX_DISPATCH_MAP(CTMDocument)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//      DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_DISPATCH_MAP
END_DISPATCH_MAP()

BEGIN_INTERFACE_MAP(CTMDocument, CDocument)
	INTERFACE_PART(CTMDocument, IID_ITmaxPresentation, Dispatch)
END_INTERFACE_MAP()

//==============================================================================
//
// 	Function Name:	CTMDocument::CTMDocument()
//
// 	Description:	This is the constructor for CTMDocument objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMDocument::CTMDocument()
{
	// TODO: add one-time construction code here

	EnableAutomation();

	AfxOleLockApp();
}

//==============================================================================
//
// 	Function Name:	CTMDocument::~CTMDocument()
//
// 	Description:	This is the destructor for CTMDocument objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMDocument::~CTMDocument()
{
	AfxOleUnlockApp();
}

