// TmbrowsevcDoc.cpp : implementation of the CTmbrowsevcDoc class
//

#include "stdafx.h"
#include "Tmbrowsevc.h"

#include "TmbrowsevcDoc.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CTmbrowsevcDoc

IMPLEMENT_DYNCREATE(CTmbrowsevcDoc, CDocument)

BEGIN_MESSAGE_MAP(CTmbrowsevcDoc, CDocument)
	//{{AFX_MSG_MAP(CTmbrowsevcDoc)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTmbrowsevcDoc construction/destruction

CTmbrowsevcDoc::CTmbrowsevcDoc()
{
	// TODO: add one-time construction code here

}

CTmbrowsevcDoc::~CTmbrowsevcDoc()
{
}

BOOL CTmbrowsevcDoc::OnNewDocument()
{
	//if (!CDocument::OnNewDocument())
		return FALSE;

	// TODO: add reinitialization code here
	// (SDI documents will reuse this document)

	//return TRUE;
}



/////////////////////////////////////////////////////////////////////////////
// CTmbrowsevcDoc serialization

void CTmbrowsevcDoc::Serialize(CArchive& ar)
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
// CTmbrowsevcDoc diagnostics

#ifdef _DEBUG
void CTmbrowsevcDoc::AssertValid() const
{
	CDocument::AssertValid();
}

void CTmbrowsevcDoc::Dump(CDumpContext& dc) const
{
	CDocument::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CTmbrowsevcDoc commands
