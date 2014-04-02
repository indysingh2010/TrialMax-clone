// Statdoc.cpp : implementation of the CTmstatvcDoc class
//

#include "stdafx.h"
#include "Tmstatvc.h"

#include "Statdoc.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CTmstatvcDoc

IMPLEMENT_DYNCREATE(CTmstatvcDoc, CDocument)

BEGIN_MESSAGE_MAP(CTmstatvcDoc, CDocument)
	//{{AFX_MSG_MAP(CTmstatvcDoc)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTmstatvcDoc construction/destruction

CTmstatvcDoc::CTmstatvcDoc()
{
}

CTmstatvcDoc::~CTmstatvcDoc()
{
}

BOOL CTmstatvcDoc::OnNewDocument()
{
	if (!CDocument::OnNewDocument())
		return FALSE;

	return TRUE;
}



/////////////////////////////////////////////////////////////////////////////
// CTmstatvcDoc serialization

void CTmstatvcDoc::Serialize(CArchive& ar)
{
	if (ar.IsStoring())
	{
	}
	else
	{
	}
}

/////////////////////////////////////////////////////////////////////////////
// CTmstatvcDoc diagnostics

#ifdef _DEBUG
void CTmstatvcDoc::AssertValid() const
{
	CDocument::AssertValid();
}

void CTmstatvcDoc::Dump(CDumpContext& dc) const
{
	CDocument::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CTmstatvcDoc commands
