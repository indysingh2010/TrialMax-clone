//==============================================================================
//
// File Name:	cappg.h
//
// Description:	This file contains the declaration of the CCapturePage class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	05-25-02	1.00		Original Release
//==============================================================================
#if !defined(__CAPPG_H__)
#define __CAPPG_H__

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
class CCapturePage : public CSetupPage
{
	private:

	public:

								CCapturePage(CWnd* pParent = 0);

		void					ReadOptions(CTMIni& rIni);
		BOOL					WriteOptions(CTMIni& rIni);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CCapturePage)
	enum { IDD = IDD_CAPTURE_PAGE };
	int		m_iArea;
	int		m_iHotkey;
	BOOL	m_bSilent;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CCapturePage)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(CCapturePage)
		// NOTE: the ClassWizard will add member functions here
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__CAPPG_H__)
