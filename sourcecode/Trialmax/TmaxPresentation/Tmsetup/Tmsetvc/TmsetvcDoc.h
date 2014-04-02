// TmsetvcDoc.h : interface of the CTmsetvcDoc class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_TMSETVCDOC_H__110691ED_D4C7_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_TMSETVCDOC_H__110691ED_D4C7_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


class CTmsetvcDoc : public CDocument
{
protected: // create from serialization only
	CTmsetvcDoc();
	DECLARE_DYNCREATE(CTmsetvcDoc)

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmsetvcDoc)
	public:
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CTmsetvcDoc();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CTmsetvcDoc)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMSETVCDOC_H__110691ED_D4C7_11D3_8177_00802966F8C1__INCLUDED_)
