// Tmlpdoc.h : interface of the CTmlpenvcDoc class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_TMLPDOC_H__52B397B3_A291_11D2_8BFC_00802966F8C1__INCLUDED_)
#define AFX_TMLPDOC_H__52B397B3_A291_11D2_8BFC_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


class CTmlpenvcDoc : public CDocument
{
protected: // create from serialization only
	CTmlpenvcDoc();
	DECLARE_DYNCREATE(CTmlpenvcDoc)

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmlpenvcDoc)
	public:
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CTmlpenvcDoc();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CTmlpenvcDoc)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMLPDOC_H__52B397B3_A291_11D2_8BFC_00802966F8C1__INCLUDED_)
