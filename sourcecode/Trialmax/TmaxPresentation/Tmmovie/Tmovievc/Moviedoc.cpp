// Moviedoc.cpp : implementation of the CMovieDoc class
//

#include "stdafx.h"
#include "Tmovievc.h"

#include "Moviedoc.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CMovieDoc

IMPLEMENT_DYNCREATE(CMovieDoc, CDocument)

BEGIN_MESSAGE_MAP(CMovieDoc, CDocument)
	//{{AFX_MSG_MAP(CMovieDoc)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CMovieDoc construction/destruction

CMovieDoc::CMovieDoc()
{
}

CMovieDoc::~CMovieDoc()
{
}

BOOL CMovieDoc::OnNewDocument()
{

	return FALSE;
}



/////////////////////////////////////////////////////////////////////////////
// CMovieDoc serialization

void CMovieDoc::Serialize(CArchive& ar)
{
	if (ar.IsStoring())
	{
	}
	else
	{
	}
}

/////////////////////////////////////////////////////////////////////////////
// CMovieDoc diagnostics

#ifdef _DEBUG
void CMovieDoc::AssertValid() const
{
	CDocument::AssertValid();
}

void CMovieDoc::Dump(CDumpContext& dc) const
{
	CDocument::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CMovieDoc commands
