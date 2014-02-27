//==============================================================================
//
// File Name:	directpg.h
//
// Description:	This file contains the declaration of the CDirectxPage class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-31-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_DIRECTPG_H__64A24524_D7CD_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_DIRECTPG_H__64A24524_D7CD_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <setuppg.h>
#include <streams.h>
#include <tmmovie.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CDirectxPage : public CSetupPage
{
	private:

		CString			m_strActive;
		IMediaControl*	m_pIMediaControl;

	public:
	
						CDirectxPage(CWnd* pParent = 0);

		void			SetActiveFilters(LPCTSTR lpFilters, 
										 LPUNKNOWN lpMediaControl);
	protected:

		void			InitActive();
		void			InitRegistered();

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CDirectxPage)
	enum { IDD = IDD_DIRECTX_PAGE };
	CListCtrl	m_Registered;
	CListCtrl	m_Active;
	CButton	m_Properties;
	CTMMovie m_TMMovie;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDirectxPage)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(CDirectxPage)
	virtual BOOL OnInitDialog();
	afx_msg void OnProperties();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DIRECTPG_H__64A24524_D7CD_11D3_8177_00802966F8C1__INCLUDED_)
