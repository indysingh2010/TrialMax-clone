//==============================================================================
//
// File Name:	printpro.h
//
// Description:	This file contains the declarations of the CPrintProgress class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2001
//
//==============================================================================
//	Date		Revision    Description
//	06-30-01	1.00		Original Release
//==============================================================================
#if !defined(__PRINTPRO_H__)
#define __PRINTPRO_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <progbar.h>
#include <tmview.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CXmlFrame;

class CPrintProgress : public CDialog
{
	private:

		CXmlFrame*			m_pXmlFrame;
		long				m_lPages;
		CString				m_strName;

	public:
	
							CPrintProgress(CXmlFrame* pXmlFrame);

		int					GetHeight();

		void				Start(LPCSTR lpName, long lPages);
		void				Finish();
		void				SetPage(long lPage);
		void				SetFilename(LPCSTR lpFilename);
		void				SetProgress(ULONG ulProgress, ULONG ulMaximum,
								        LPCSTR lpszStatus);

	protected:

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	//{{AFX_DATA(CPrintProgress)
	enum { IDD = IDD_PRINT_PROGRESS };
	CStatic	m_ctrlStatus;
	CStatic	m_ctrlJob;
	CProgressBar	m_ctrlBar;
	CStatic	m_ctrlBytes;
	CButton	m_ctrlAbort;
	CString	m_strBytes;
	CString	m_strJob;
	CString	m_strStatus;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CPrintProgress)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:

	// Generated message map functions
	//{{AFX_MSG(CPrintProgress)
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnAbort();
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__PRINTPRO_H__)
