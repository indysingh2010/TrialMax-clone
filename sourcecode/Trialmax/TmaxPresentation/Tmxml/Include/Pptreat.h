//==============================================================================
//
// File Name:	pptreat.h
//
// Description:	This file contains the declaration of the CPPTreatment class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	05-20-01	1.00		Original Release
//==============================================================================
#if !defined(__PPTREAT_H__)
#define __PPTREAT_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <xmlframe.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CPPTreatment : public CPropertyPage
{
	private:

							DECLARE_DYNCREATE(CPPTreatment)

	public:

		CXmlFrame*			m_pXmlFrame;
		SPutTreatment*		m_pPutTreatment;
							
							CPPTreatment();
						   ~CPPTreatment();

	protected:

		void				Refresh();

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Dialog Data
	//{{AFX_DATA(CPPTreatment)
	enum { IDD = IDD_TREATMENT_PAGE };
	CStatic	m_ctrlResponseCode;
	CStatic	m_ctrlTargetLabel;
	CButton	m_ctrlSaveResponse;
	CButton	m_ctrlSaveRequest;
	CEdit	m_ctrlResponse;
	CStatic	m_ctrlRequestLabel;
	CButton	m_ctrlPost;
	CEdit	m_ctrlHeaders;
	CEdit	m_ctrlTarget;
	CEdit	m_ctrlRequest;
	CString	m_strTarget;
	//}}AFX_DATA


	// Overrides
	// ClassWizard generate virtual function overrides
	//{{AFX_VIRTUAL(CPPTreatment)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:
	// Generated message map functions
	//{{AFX_MSG(CPPTreatment)
	virtual BOOL OnInitDialog();
	afx_msg void OnSaveRequest();
	afx_msg void OnSaveResponse();
	afx_msg void OnPost();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__PPTREAT_H__)
