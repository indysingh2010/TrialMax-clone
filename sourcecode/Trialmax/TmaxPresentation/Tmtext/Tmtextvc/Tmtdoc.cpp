// Tmtdoc.cpp : implementation of the CTmtextDoc class
//

#include "stdafx.h"
#include "Tmtextvc.h"

#include "Tmtdoc.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CTmtextDoc

IMPLEMENT_DYNCREATE(CTmtextDoc, CDocument)

BEGIN_MESSAGE_MAP(CTmtextDoc, CDocument)
	//{{AFX_MSG_MAP(CTmtextDoc)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTmtextDoc construction/destruction

CTmtextDoc::CTmtextDoc()
{
}

CTmtextDoc::~CTmtextDoc()
{
}

BOOL CTmtextDoc::OnNewDocument()
{
	return FALSE;
}

BOOL CTmtextDoc::OnOpenDocument(LPCTSTR lpszPathName) 
{
	if(!CDocument::OnOpenDocument(lpszPathName))
		return FALSE;

	return TRUE;
}
