//==============================================================================
//
// File Name:	aboutpg.h
//
// Description:	This file contains the declaration of the CAboutPage class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-31-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_ABOUTPG_H__64A24521_D7CD_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_ABOUTPG_H__64A24521_D7CD_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <setuppg.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CAboutPage : public CSetupPage
{
	private:

	public:
	
								CAboutPage(CWnd* pParent = 0);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CAboutPage)
	enum { IDD = IDD_ABOUT_PAGE };
	CString	m_strCopyright;
	CString	m_strEmail;
	CString	m_strName;
	CString	m_strVersion;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAboutPage)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(CAboutPage)
		// NOTE: the ClassWizard will add member functions here
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_ABOUTPG_H__64A24521_D7CD_11D3_8177_00802966F8C1__INCLUDED_)
