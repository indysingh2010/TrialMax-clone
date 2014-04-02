//==============================================================================
//
// File Name:	fprintercaps.h
//
// Description:	This file contains the declaration of the CFPrinterCaps class. 
//				This class manages a dialog that allows the user to review the
//				device capabilities of the system printers.
//
// Author:		Kenneth Moore
//
// Copyright FTI Consulting - All Rights Reserved
//
//==============================================================================
//	Date		Revision    Description
//	06-16-2006	1.00		Original Release
//==============================================================================
#if !defined(__FPRINTERCAPS_H__)
#define __FPRINTERCAPS_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <resource.h>
#include <printer.h>
#include <devcapsctrl.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CFPrinterCaps : public CDialog
{
	private:

		CTMPrinter		m_tmPrinter;

	public:
	
						CFPrinterCaps(CWnd* pParent = NULL);

	protected:

		void			FillPrinters();

	//	Remainder of this file is managed by ClassWizard
	public:

	// Dialog Data
	//{{AFX_DATA(CFPrinterCaps)
	enum { IDD = IDD_PRINTER_CAPS };
	CDeviceCapsCtrl	m_ctrlDevMode;
	CDeviceCapsCtrl	m_ctrlDeviceCaps;
	CListBox	m_ctrlPrinters;
	//}}AFX_DATA


	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CFPrinterCaps)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:

	// Generated message map functions
	//{{AFX_MSG(CFPrinterCaps)
	virtual BOOL OnInitDialog();
	afx_msg void OnPrinterChanged();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__FPRINTERCAPS_H__)
