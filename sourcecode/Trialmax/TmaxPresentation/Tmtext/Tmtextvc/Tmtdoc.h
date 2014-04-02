// Tmtdoc.h : interface of the CTmtextDoc class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_TMTDOC_H__07F27A64_ABF9_11D2_8C08_00802966F8C1__INCLUDED_)
#define AFX_TMTDOC_H__07F27A64_ABF9_11D2_8C08_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class CTmtextDoc : public CDocument
{
protected: // create from serialization only
	CTmtextDoc();
	DECLARE_DYNCREATE(CTmtextDoc)

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmtextDoc)
	public:
	virtual BOOL OnNewDocument();
	virtual BOOL OnOpenDocument(LPCTSTR lpszPathName);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CTmtextDoc();

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CTmtextDoc)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMTDOC_H__07F27A64_ABF9_11D2_8C08_00802966F8C1__INCLUDED_)
