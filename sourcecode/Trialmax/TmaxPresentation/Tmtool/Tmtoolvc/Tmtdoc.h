// Tmtdoc.h : interface of the CTmtoolvcDoc class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_TMTDOC_H__BBD917B7_D89D_11D1_B16C_008029EFD140__INCLUDED_)
#define AFX_TMTDOC_H__BBD917B7_D89D_11D1_B16C_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000


class CTmtoolvcDoc : public CDocument
{
protected: // create from serialization only
	CTmtoolvcDoc();
	DECLARE_DYNCREATE(CTmtoolvcDoc)

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmtoolvcDoc)
	public:
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CTmtoolvcDoc();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CTmtoolvcDoc)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMTDOC_H__BBD917B7_D89D_11D1_B16C_008029EFD140__INCLUDED_)
