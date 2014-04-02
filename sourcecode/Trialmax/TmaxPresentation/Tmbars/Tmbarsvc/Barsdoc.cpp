// Barsdoc.cpp : implementation of the CTmbarsvcDoc class
//

#include "stdafx.h"
#include "Tmbarsvc.h"

#include "Barsdoc.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CTmbarsvcDoc

IMPLEMENT_DYNCREATE(CTmbarsvcDoc, CDocument)

BEGIN_MESSAGE_MAP(CTmbarsvcDoc, CDocument)
	//{{AFX_MSG_MAP(CTmbarsvcDoc)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTmbarsvcDoc construction/destruction

CTmbarsvcDoc::CTmbarsvcDoc()
{
	// TODO: add one-time construction code here

}

CTmbarsvcDoc::~CTmbarsvcDoc()
{
}

BOOL CTmbarsvcDoc::OnNewDocument()
{
	if (!CDocument::OnNewDocument())
		return FALSE;

	// TODO: add reinitialization code here
	// (SDI documents will reuse this document)

	return TRUE;
}



/////////////////////////////////////////////////////////////////////////////
// CTmbarsvcDoc serialization

void CTmbarsvcDoc::Serialize(CArchive& ar)
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
// CTmbarsvcDoc diagnostics

#ifdef _DEBUG
void CTmbarsvcDoc::AssertValid() const
{
	CDocument::AssertValid();
}

void CTmbarsvcDoc::Dump(CDumpContext& dc) const
{
	CDocument::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CTmbarsvcDoc commands
