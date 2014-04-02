// TpowervcDoc.cpp : implementation of the CTpowervcDoc class
//

#include "stdafx.h"
#include "Tpowervc.h"

#include "TpowervcDoc.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CTpowervcDoc

IMPLEMENT_DYNCREATE(CTpowervcDoc, CDocument)

BEGIN_MESSAGE_MAP(CTpowervcDoc, CDocument)
	//{{AFX_MSG_MAP(CTpowervcDoc)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTpowervcDoc construction/destruction

CTpowervcDoc::CTpowervcDoc()
{
	// TODO: add one-time construction code here

}

CTpowervcDoc::~CTpowervcDoc()
{
}

BOOL CTpowervcDoc::OnNewDocument()
{
	//if (!CDocument::OnNewDocument())

	//	Don't process new documents
	return FALSE;
}




/////////////////////////////////////////////////////////////////////////////
// CTpowervcDoc serialization

void CTpowervcDoc::Serialize(CArchive& ar)
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
// CTpowervcDoc diagnostics

#ifdef _DEBUG
void CTpowervcDoc::AssertValid() const
{
	CDocument::AssertValid();
}

void CTpowervcDoc::Dump(CDumpContext& dc) const
{
	CDocument::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CTpowervcDoc commands
