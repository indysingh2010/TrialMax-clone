// TmgrabvcDoc.cpp : implementation of the CTmgrabvcDoc class
//

#include "stdafx.h"
#include "Tmgrabvc.h"

#include "TmgrabvcDoc.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CTmgrabvcDoc

IMPLEMENT_DYNCREATE(CTmgrabvcDoc, CDocument)

BEGIN_MESSAGE_MAP(CTmgrabvcDoc, CDocument)
	//{{AFX_MSG_MAP(CTmgrabvcDoc)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTmgrabvcDoc construction/destruction

CTmgrabvcDoc::CTmgrabvcDoc()
{
	// TODO: add one-time construction code here

}

CTmgrabvcDoc::~CTmgrabvcDoc()
{
}

BOOL CTmgrabvcDoc::OnNewDocument()
{
	if (!CDocument::OnNewDocument())
		return FALSE;

	// TODO: add reinitialization code here
	// (SDI documents will reuse this document)

	return TRUE;
}



/////////////////////////////////////////////////////////////////////////////
// CTmgrabvcDoc serialization

void CTmgrabvcDoc::Serialize(CArchive& ar)
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
// CTmgrabvcDoc diagnostics

#ifdef _DEBUG
void CTmgrabvcDoc::AssertValid() const
{
	CDocument::AssertValid();
}

void CTmgrabvcDoc::Dump(CDumpContext& dc) const
{
	CDocument::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CTmgrabvcDoc commands
