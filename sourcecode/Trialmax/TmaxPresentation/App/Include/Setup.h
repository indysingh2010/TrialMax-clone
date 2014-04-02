//==============================================================================
//
// File Name:	setup.h
//
// Description:	This file contains the declaration of the CSetup class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	02-27-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_SETUP_H__9CA0706D_ED16_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_SETUP_H__9CA0706D_ED16_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <tmsetup.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CSetup : public CDialog
{
	private:
	
		CString				m_strIniFile;
		CString				m_strPresentationFileSpec;
		CString				m_strFilters;
		LPUNKNOWN			m_lpMediaControl;

	public:
	
							CSetup(CWnd* pParent = NULL);

		void				OnOK();

		void				SetIniFile(LPCSTR lpFilename);
		void				SetPresentationFileSpec(LPCSTR lpszFileSpec);
		void				SetActiveFilters(LPCSTR lpFilters, LPUNKNOWN lpMediaControl); 


	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CSetup)
	enum { IDD = IDD_SETUP };
	CTMSetup	m_TMSetup;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CSetup)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(CSetup)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedOk();
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SETUP_H__9CA0706D_ED16_11D3_8177_00802966F8C1__INCLUDED_)
