//==============================================================================
//
// File Name:	setuppg.h
//
// Description:	This file contains the declaration of the CSetupPage class. This
//				class serves as the base class for all property pages used in
//				this control.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-31-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_SETUPPG_H__53477F01_D7C5_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_SETUPPG_H__53477F01_D7C5_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <tmini.h>
#include <handler.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMSetupCtrl;

class CSetupPage : public CDialog
{
	private:

	public:
	
		CTMSetupCtrl*		m_pControl;
		CErrorHandler*		m_pErrors;
		int					m_iTab;
		int					m_iPage;
	
							CSetupPage(int iResourceId = 0, CWnd* pParent = 0);
		virtual			   ~CSetupPage();

		virtual void		ReadOptions(CTMIni& rIni);
		virtual BOOL		WriteOptions(CTMIni& rIni);

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CSetupPage)
	enum { IDD = IDD_ABOUTBOX_TMSETUP };
		// NOTE: the ClassWizard will add data members here
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CSetupPage)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(CSetupPage)
		// NOTE: the ClassWizard will add member functions here
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SETUPPG_H__53477F01_D7C5_11D3_8177_00802966F8C1__INCLUDED_)
