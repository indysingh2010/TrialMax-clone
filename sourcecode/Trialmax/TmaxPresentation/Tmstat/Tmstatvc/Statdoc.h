// Statdoc.h : interface of the CTmstatvcDoc class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_STATDOC_H__44ADEC34_C265_11D2_8173_00802966F8C1__INCLUDED_)
#define AFX_STATDOC_H__44ADEC34_C265_11D2_8173_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


class CTmstatvcDoc : public CDocument
{
protected: // create from serialization only
	CTmstatvcDoc();
	DECLARE_DYNCREATE(CTmstatvcDoc)

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmstatvcDoc)
	public:
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CTmstatvcDoc();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CTmstatvcDoc)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_STATDOC_H__44ADEC34_C265_11D2_8173_00802966F8C1__INCLUDED_)
