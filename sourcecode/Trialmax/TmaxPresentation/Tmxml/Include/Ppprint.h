//==============================================================================
//
// File Name:	ppprint.h
//
// Description:	This file contains the declaration of the CPPPrint class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	07-03-01	1.00		Original Release
//==============================================================================
#if !defined(__PPPRINT_H__)
#define __PPPRINT_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <template.h>
#include <tmprint.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CPPPrint : public CPropertyPage
{
	private:
		
							DECLARE_DYNCREATE(CPPPrint)

	public:

		CTemplates*			m_pTemplates;
	
							CPPPrint();
						   ~CPPPrint();

	protected:

		void				FillTemplates();

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CPPPrint)
	enum { IDD = IDD_PRINT_PAGE };
	CListBox	m_ctrlTemplates;
	CListBox	m_ctrlPrinters;
	CTMPrint	m_TMPrint;
	CString	m_strPrinter;
	BOOL	m_bCurrentSession;
	CString	m_strTemplate;
	BOOL	m_bCollate;
	int		m_iCopies;
	BOOL	m_bCombinePrintPages;
	BOOL	m_bMinimizeColorDepth;
	//}}AFX_DATA


	// ClassWizard generate virtual function overrides
	//{{AFX_VIRTUAL(CPPPrint)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:
	// Generated message map functions
	//{{AFX_MSG(CPPPrint)
	virtual BOOL OnInitDialog();
	afx_msg void OnFirstPrinter(LPCTSTR lpszPrinter);
	afx_msg void OnNextPrinter(LPCTSTR lpszPrinter);
	DECLARE_EVENTSINK_MAP()
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__PPPRINT_H__)
