// TmviewvcDoc.cpp : implementation of the CTmviewvcDoc class
//

#include "stdafx.h"
#include "Tmviewvc.h"

#include "TmviewvcDoc.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CTmviewvcDoc

IMPLEMENT_DYNCREATE(CTmviewvcDoc, CDocument)

BEGIN_MESSAGE_MAP(CTmviewvcDoc, CDocument)
	//{{AFX_MSG_MAP(CTmviewvcDoc)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

BEGIN_DISPATCH_MAP(CTmviewvcDoc, CDocument)
	//{{AFX_DISPATCH_MAP(CTmviewvcDoc)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//      DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_DISPATCH_MAP
END_DISPATCH_MAP()

// Note: we add support for IID_ITmviewvc to support typesafe binding
//  from VBA.  This IID must match the GUID that is attached to the 
//  dispinterface in the .ODL file.

// {CAEBF180-FABA-11D0-B003-008029EFD140}
static const IID IID_ITmviewvc =
{ 0xcaebf180, 0xfaba, 0x11d0, { 0xb0, 0x3, 0x0, 0x80, 0x29, 0xef, 0xd1, 0x40 } };

BEGIN_INTERFACE_MAP(CTmviewvcDoc, CDocument)
	INTERFACE_PART(CTmviewvcDoc, IID_ITmviewvc, Dispatch)
END_INTERFACE_MAP()

CTmviewvcDoc::CTmviewvcDoc()
{
	// TODO: add one-time construction code here

	EnableAutomation();

	AfxOleLockApp();
}

CTmviewvcDoc::~CTmviewvcDoc()
{
	AfxOleUnlockApp();
}

BOOL CTmviewvcDoc::OnNewDocument()
{
	//if (!CDocument::OnNewDocument())

	//	Don't process new documents
	return FALSE;
}

void CTmviewvcDoc::Serialize(CArchive& ar)
{
	if (ar.IsStoring())
	{
		// TODO: add storing code here
	}
	else
	{
		// TODO: add loading code here
	}
}

/////////////////////////////////////////////////////////////////////////////
// CTmviewvcDoc diagnostics

#ifdef _DEBUG
void CTmviewvcDoc::AssertValid() const
{
	CDocument::AssertValid();
}

void CTmviewvcDoc::Dump(CDumpContext& dc) const
{
	CDocument::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CTmviewvcDoc commands
