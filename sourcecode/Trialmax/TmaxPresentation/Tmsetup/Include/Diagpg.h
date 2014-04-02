//==============================================================================
//
// File Name:	diagpg.h
//
// Description:	This file contains the declaration of the CDiagnosticPage class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-31-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_DIAGPG_H__53477F02_D7C5_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_DIAGPG_H__53477F02_D7C5_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <setuppg.h>
#include <tmver.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define DP_NAME_COLUMN			0
#define DP_VERSION_COLUMN		1
#define DP_BUILD_COLUMN			2
#define DP_DESCRIPTION_COLUMN	3
#define DP_PATH_COLUMN			4
#define DP_CLSID_COLUMN			5

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CDiagnosticPage : public CSetupPage
{
	private:

	public:
	
								CDiagnosticPage(CWnd* pParent = 0);
		virtual				   ~CDiagnosticPage();

	protected:

		void					Add(CTMVersion& rVersion);

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CDiagnosticPage)
	enum { IDD = IDD_DIAGNOSTIC_PAGE };
	CListCtrl	m_ctrlList;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDiagnosticPage)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(CDiagnosticPage)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DIAGPG_H__53477F02_D7C5_11D3_8177_00802966F8C1__INCLUDED_)
