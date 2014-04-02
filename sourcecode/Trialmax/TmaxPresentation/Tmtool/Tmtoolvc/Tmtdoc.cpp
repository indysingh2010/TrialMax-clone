// Tmtdoc.cpp : implementation of the CTmtoolvcDoc class
//

#include "stdafx.h"
#include "Tmtoolvc.h"

#include "Tmtdoc.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CTmtoolvcDoc

IMPLEMENT_DYNCREATE(CTmtoolvcDoc, CDocument)

BEGIN_MESSAGE_MAP(CTmtoolvcDoc, CDocument)
	//{{AFX_MSG_MAP(CTmtoolvcDoc)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTmtoolvcDoc construction/destruction

CTmtoolvcDoc::CTmtoolvcDoc()
{
	// TODO: add one-time construction code here

}

CTmtoolvcDoc::~CTmtoolvcDoc()
{
}

BOOL CTmtoolvcDoc::OnNewDocument()
{
	if (!CDocument::OnNewDocument())
		return FALSE;

	// TODO: add reinitialization code here
	// (SDI documents will reuse this document)

	return TRUE;
}



/////////////////////////////////////////////////////////////////////////////
// CTmtoolvcDoc serialization

void CTmtoolvcDoc::Serialize(CArchive& ar)
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
// CTmtoolvcDoc diagnostics

#ifdef _DEBUG
void CTmtoolvcDoc::AssertValid() const
{
	CDocument::AssertValid();
}

void CTmtoolvcDoc::Dump(CDumpContext& dc) const
{
	CDocument::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CTmtoolvcDoc commands
