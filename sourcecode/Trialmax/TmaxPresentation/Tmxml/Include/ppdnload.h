//==============================================================================
//
// File Name:	ppdnload.h
//
// Description:	This file contains the declaration of the CPPDownload class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 2002
//
//==============================================================================
//	Date		Revision    Description
//	09-25-01	1.00		Original Release
//==============================================================================
#if !defined(__PPDNLOAD_H__)
#define __PPDNLOAD_H__

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
class CPPDownload : public CPropertyPage
{
	private:

							DECLARE_DYNCREATE(CPPDownload)

	public:
							
							CPPDownload();
						   ~CPPDownload();

	protected:

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Dialog Data
	//{{AFX_DATA(CPPDownload)
	enum { IDD = IDD_DOWNLOAD_PAGE };
	CString	m_strAborted;
	CString	m_strCached;
	CString	m_strComplete;
	CString	m_strError;
	CString	m_strErrorMsg;
	CString	m_strLParam;
	CString	m_strMaxProgress;
	CString	m_strProgress;
	CString	m_strRemote;
	CString	m_strSource;
	CString	m_strStatus;
	//}}AFX_DATA


	// Overrides
	// ClassWizard generate virtual function overrides
	//{{AFX_VIRTUAL(CPPDownload)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:
	// Generated message map functions
	//{{AFX_MSG(CPPDownload)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__PPDNLOAD_H__)
