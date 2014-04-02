//==============================================================================
//
// File Name:	select.h
//
// Description:	This file contains the declaration of the CSelect class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2001
//
//==============================================================================
//	Date		Revision    Description
//	04-27-01	1.00		Original Release
//==============================================================================
#if !defined(AFX_SELECT_H__9F65B740_3B55_11D5_8F0A_00802966F8C1__INCLUDED_)
#define AFX_SELECT_H__9F65B740_3B55_11D5_8F0A_00802966F8C1__INCLUDED_

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
class COptions;

class CSelect : public CDialog
{
	private:

		COptions*			m_pOptions;

	public:
	
							CSelect(COptions* pOptions);

	protected:

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Dialog Data
	//{{AFX_DATA(CSelect)
	enum { IDD = IDD_SELECT };
	CListBox	m_ctrlPrinters;
	BOOL	m_bCollate;
	int		m_iCopies;
	CString	m_strPrinter;
	//}}AFX_DATA

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CSelect)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:

	// Generated message map functions
	//{{AFX_MSG(CSelect)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SELECT_H__9F65B740_3B55_11D5_8F0A_00802966F8C1__INCLUDED_)
