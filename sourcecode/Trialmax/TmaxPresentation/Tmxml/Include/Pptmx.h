//==============================================================================
//
// File Name:	pptmx.h
//
// Description:	This file contains the declaration of the CPPTmx class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	05-20-01	1.00		Original Release
//==============================================================================
#if !defined(__PPTMX_H__)
#define __PPTMX_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CIXMLDOMDocument;

class CPPTmx : public CPropertyPage
{
	private:

							DECLARE_DYNCREATE(CPPTmx)

	public:
							
		CIXMLDOMDocument*	m_pXmlDocument;

							CPPTmx();
						   ~CPPTmx();

	protected:

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Dialog Data
	//{{AFX_DATA(CPPTmx)
	enum { IDD = IDD_TMX_PAGE };
	CButton	m_ctrlSaveAs;
	CEdit	m_ctrlXml;
	CString	m_strFilename;
	CString	m_strSource;
	//}}AFX_DATA


	// Overrides
	// ClassWizard generate virtual function overrides
	//{{AFX_VIRTUAL(CPPTmx)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:
	// Generated message map functions
	//{{AFX_MSG(CPPTmx)
	virtual BOOL OnInitDialog();
	afx_msg void OnSaveAs();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__PPTMX_H__)
