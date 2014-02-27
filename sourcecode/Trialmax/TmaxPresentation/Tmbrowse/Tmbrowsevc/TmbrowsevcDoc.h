// TmbrowsevcDoc.h : interface of the CTmbrowsevcDoc class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_TMBROWSEVCDOC_H__E6904C23_1D6F_11D6_8F0B_00802966F8C1__INCLUDED_)
#define AFX_TMBROWSEVCDOC_H__E6904C23_1D6F_11D6_8F0B_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


class CTmbrowsevcDoc : public CDocument
{
protected: // create from serialization only
	CTmbrowsevcDoc();
	DECLARE_DYNCREATE(CTmbrowsevcDoc)

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmbrowsevcDoc)
	public:
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CTmbrowsevcDoc();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CTmbrowsevcDoc)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMBROWSEVCDOC_H__E6904C23_1D6F_11D6_8F0B_00802966F8C1__INCLUDED_)
