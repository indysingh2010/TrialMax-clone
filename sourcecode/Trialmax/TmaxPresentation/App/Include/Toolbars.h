//==============================================================================
//
// File Name:	toolbars.h
//
// Description:	This file contains the declaration of the CToolbars class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	02-27-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_TOOLBARS_H__6D75C681_ED09_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_TOOLBARS_H__6D75C681_ED09_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <tmbars.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CToolbars : public CDialog
{
	private:
	
		CString				m_strIniFile;

	public:
	
							CToolbars(CWnd* pParent = NULL);

		void				OnOK();

		void				SetIniFile(LPCSTR lpFilename);
		LPCSTR				GetIniFile();

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CToolbars)
	enum { IDD = IDD_TOOLBARS };
	CTMBars	m_TMBars;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CToolbars)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(CToolbars)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TOOLBARS_H__6D75C681_ED09_11D3_8177_00802966F8C1__INCLUDED_)
