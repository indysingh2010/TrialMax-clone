// Tmlpdoc.cpp : implementation of the CTmlpenvcDoc class
//

#include "stdafx.h"
#include "Tmlpenvc.h"

#include "Tmlpdoc.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CTmlpenvcDoc

IMPLEMENT_DYNCREATE(CTmlpenvcDoc, CDocument)

BEGIN_MESSAGE_MAP(CTmlpenvcDoc, CDocument)
	//{{AFX_MSG_MAP(CTmlpenvcDoc)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTmlpenvcDoc construction/destruction

CTmlpenvcDoc::CTmlpenvcDoc()
{
	// TODO: add one-time construction code here

}

CTmlpenvcDoc::~CTmlpenvcDoc()
{
}

BOOL CTmlpenvcDoc::OnNewDocument()
{
	if (!CDocument::OnNewDocument())
		return FALSE;

	// TODO: add reinitialization code here
	// (SDI documents will reuse this document)

	return TRUE;
}



/////////////////////////////////////////////////////////////////////////////
// CTmlpenvcDoc serialization

void CTmlpenvcDoc::Serialize(CArchive& ar)
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
// CTmlpenvcDoc diagnostics

#ifdef _DEBUG
void CTmlpenvcDoc::AssertValid() const
{
	CDocument::AssertValid();
}

void CTmlpenvcDoc::Dump(CDumpContext& dc) const
{
	CDocument::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CTmlpenvcDoc commands
