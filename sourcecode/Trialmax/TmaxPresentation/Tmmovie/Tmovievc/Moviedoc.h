// Moviedoc.h : interface of the CMovieDoc class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_MOVIEDOC_H__10AFCB1D_02BA_11D2_B1BF_008029EFD140__INCLUDED_)
#define AFX_MOVIEDOC_H__10AFCB1D_02BA_11D2_B1BF_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000


class CMovieDoc : public CDocument
{
protected: // create from serialization only
	CMovieDoc();
	DECLARE_DYNCREATE(CMovieDoc)

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMovieDoc)
	public:
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CMovieDoc();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CMovieDoc)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_MOVIEDOC_H__10AFCB1D_02BA_11D2_B1BF_008029EFD140__INCLUDED_)
