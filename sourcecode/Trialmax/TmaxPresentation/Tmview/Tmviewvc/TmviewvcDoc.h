// TmviewvcDoc.h : interface of the CTmviewvcDoc class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_TMVIEWVCDOC_H__CAEBF18C_FABA_11D0_B003_008029EFD140__INCLUDED_)
#define AFX_TMVIEWVCDOC_H__CAEBF18C_FABA_11D0_B003_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000


class CTmviewvcDoc : public CDocument
{
protected: // create from serialization only
	CTmviewvcDoc();
	DECLARE_DYNCREATE(CTmviewvcDoc)

// Attributes
public:


// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmviewvcDoc)
	public:
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CTmviewvcDoc();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CTmviewvcDoc)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	// Generated OLE dispatch map functions
	//{{AFX_DISPATCH(CTmviewvcDoc)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DISPATCH
	DECLARE_DISPATCH_MAP()
	DECLARE_INTERFACE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMVIEWVCDOC_H__CAEBF18C_FABA_11D0_B003_008029EFD140__INCLUDED_)
