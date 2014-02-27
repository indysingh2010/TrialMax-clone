#if !defined(AFX_FILTERS_H__6B946020_EDC3_11D6_9C3D_00E02987E1B1__INCLUDED_)
#define AFX_FILTERS_H__6B946020_EDC3_11D6_9C3D_00E02987E1B1__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Filters.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CFilters dialog
class CTMMovie;

class CFilters : public CDialog
{
	CTMMovie* m_pTMMovie;

// Construction
public:
	CFilters(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CFilters)
	enum { IDD = IDD_FILTERS };
	CListBox	m_ctrlUser;
	CButton	m_ctrlRemove;
	CListBox	m_ctrlRegistered;
	CButton	m_ctrlAdd;
	//}}AFX_DATA

	void SetTMMovie(CTMMovie* pTMMovie){ m_pTMMovie = pTMMovie; }
	void FillListBox(CString& rFilters, CListBox& rListBox);


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CFilters)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CFilters)
	virtual BOOL OnInitDialog();
	afx_msg void OnRegisteredChanged();
	afx_msg void OnUserChanged();
	afx_msg void OnAdd();
	afx_msg void OnRemove();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_FILTERS_H__6B946020_EDC3_11D6_9C3D_00E02987E1B1__INCLUDED_)
