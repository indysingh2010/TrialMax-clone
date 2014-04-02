// Printdoc.cpp : implementation of the CTprintvcDoc class
//

#include "stdafx.h"
#include "Tprintvc.h"

#include "Printdoc.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CTprintvcDoc

IMPLEMENT_DYNCREATE(CTprintvcDoc, CDocument)

BEGIN_MESSAGE_MAP(CTprintvcDoc, CDocument)
	//{{AFX_MSG_MAP(CTprintvcDoc)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTprintvcDoc construction/destruction

CTprintvcDoc::CTprintvcDoc()
{
	// TODO: add one-time construction code here

}

CTprintvcDoc::~CTprintvcDoc()
{
}

BOOL CTprintvcDoc::OnNewDocument()
{
	if (!CDocument::OnNewDocument())
		return FALSE;

	// TODO: add reinitialization code here
	// (SDI documents will reuse this document)

	return TRUE;
}



/////////////////////////////////////////////////////////////////////////////
// CTprintvcDoc serialization

void CTprintvcDoc::Serialize(CArchive& ar)
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
// CTprintvcDoc diagnostics

#ifdef _DEBUG
void CTprintvcDoc::AssertValid() const
{
	CDocument::AssertValid();
}

void CTprintvcDoc::Dump(CDumpContext& dc) const
{
	CDocument::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CTprintvcDoc commands
