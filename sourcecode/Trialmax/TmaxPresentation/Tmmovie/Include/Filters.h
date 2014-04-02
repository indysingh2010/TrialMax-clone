//==============================================================================
//
// File Name:	filters.h
//
// Description:	This file contains the declaration of the CFilterInfo class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	08-04-98	1.00		Original Release
//==============================================================================
#if !defined(AFX_FILTERS_H__AC439051_2DFB_11D2_B216_008029EFD140__INCLUDED_)
#define AFX_FILTERS_H__AC439051_2DFB_11D2_B216_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <resource.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CFilterInfo : public CDialog
{
	private:

	public:

		IGraphBuilder*	m_pIGraphBuilder;
		IMediaControl*	m_pIMediaControl;
		CString			m_strRegFilters;
		CString			m_strActFilters;
		long			m_lRegFilters;
		long			m_lActFilters;
	
						CFilterInfo(CWnd* pParent); 
					   ~CFilterInfo();

	protected:

		void			InitActive();
		void			InitRegistered();

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CFilterInfo)
	enum { IDD = IDD_FILTERINFO };
	CButton	m_Advanced;
	CListCtrl	m_Active;
	CListCtrl	m_Registered;
	//}}AFX_DATA


	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CFilterInfo)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(CFilterInfo)
	virtual BOOL OnInitDialog();
	afx_msg void OnAdvanced();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_FILTERS_H__AC439051_2DFB_11D2_B216_008029EFD140__INCLUDED_)
