// Barsdoc.h : interface of the CTmbarsvcDoc class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_BARSDOC_H__3F4BEF23_C5E5_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_BARSDOC_H__3F4BEF23_C5E5_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


class CTmbarsvcDoc : public CDocument
{
protected: // create from serialization only
	CTmbarsvcDoc();
	DECLARE_DYNCREATE(CTmbarsvcDoc)

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmbarsvcDoc)
	public:
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CTmbarsvcDoc();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CTmbarsvcDoc)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_BARSDOC_H__3F4BEF23_C5E5_11D3_8177_00802966F8C1__INCLUDED_)
