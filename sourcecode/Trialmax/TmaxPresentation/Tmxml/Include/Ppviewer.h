//==============================================================================
//
// File Name:	ppviewer.h
//
// Description:	This file contains the declaration of the CPPViewer class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	03-10-01	1.00		Original Release
//==============================================================================
#if !defined(__PPVIEWER_H__)
#define __PPVIEWER_H__

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
class CPPViewer : public CPropertyPage
{
	private:

							DECLARE_DYNCREATE(CPPViewer)

	public:
							
							CPPViewer();
						   ~CPPViewer();

	protected:

		void				EnablePortControls();

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Dialog Data
	//{{AFX_DATA(CPPViewer)
	enum { IDD = IDD_VIEWER_PAGE };
	CStatic	m_ctrlProxyPortLabel;
	CEdit	m_ctrlProxyPort;
	CStatic	m_ctrlProxyAddressLabel;
	CEdit	m_ctrlInternetPort;
	CEdit	m_ctrlProxyAddress;
	CSpinButtonCtrl	m_ctrlSpinDelay;
	float	m_fProgressDelay;
	BOOL	m_bEnableErrors;
	BOOL	m_bFloatProgress;
	BOOL	m_bConfirmBatch;
	int		m_iConnection;
	UINT	m_uInternetPort;
	CString	m_strProxyAddress;
	UINT	m_uProxyPort;
	BOOL	m_bShowPageNavigation;
	//}}AFX_DATA


	// Overrides
	// ClassWizard generate virtual function overrides
	//{{AFX_VIRTUAL(CPPViewer)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:
	// Generated message map functions
	//{{AFX_MSG(CPPViewer)
	virtual BOOL OnInitDialog();
	afx_msg void OnAssignPort();
	afx_msg void OnProxyServer();
	afx_msg void OnDefaultPort();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__PPVIEWER_H__)
