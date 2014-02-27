// TpowervcDoc.h : interface of the CTpowervcDoc class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_TPOWERVCDOC_H__F0E8E9CC_058D_11D3_8175_00802966F8C1__INCLUDED_)
#define AFX_TPOWERVCDOC_H__F0E8E9CC_058D_11D3_8175_00802966F8C1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


class CTpowervcDoc : public CDocument
{
protected: // create from serialization only
	CTpowervcDoc();
	DECLARE_DYNCREATE(CTpowervcDoc)

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTpowervcDoc)
	public:
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CTpowervcDoc();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CTpowervcDoc)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TPOWERVCDOC_H__F0E8E9CC_058D_11D3_8175_00802966F8C1__INCLUDED_)
