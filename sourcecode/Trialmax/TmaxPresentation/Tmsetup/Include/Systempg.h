//==============================================================================
//
// File Name:	systempg.h
//
// Description:	This file contains the declaration of the CSystemPage class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-31-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_SYSTEMPG_H__64A24523_D7CD_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_SYSTEMPG_H__64A24523_D7CD_11D3_8177_00802966F8C1__INCLUDED_

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
class CSystemPage : public CSetupPage
{
	private:

	public:

								CSystemPage(CWnd* pParent = 0);

		void					ReadOptions(CTMIni& rIni);
		BOOL					WriteOptions(CTMIni& rIni);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CSystemPage)
	enum { IDD = IDD_SYSTEM_PAGE };
	int		m_iAnimation;
	int		m_iImage;
	int		m_iPlaylist;
	int		m_iPowerPoint;
	int		m_iTreatment;
	int		m_iShow;
	BOOL	m_bOptimizeVideo;
	BOOL	m_bDualMonitors;
	BOOL	m_bOptimizeTablet;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CSystemPage)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(CSystemPage)
		// NOTE: the ClassWizard will add member functions here
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SYSTEMPG_H__64A24523_D7CD_11D3_8177_00802966F8C1__INCLUDED_)
