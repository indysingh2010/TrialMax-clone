//==============================================================================
//
// File Name:	download.h
//
// Description:	This file contains the declarations of the CDownload class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2001
//
//==============================================================================
//	Date		Revision    Description
//	06-05-01	1.00		Original Release
//==============================================================================
#if !defined(__DOWNLOAD_H__)
#define __DOWNLOAD_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <progbar.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CXmlFrame;

//	This class implements a dialog box used to monitor and abort a file download
class CDownload : public CDialog
{
	private:

		CXmlFrame*			m_pXmlFrame;
		BOOL				m_bAborted;

	public:
	
							CDownload(CXmlFrame* pFrame);
						   ~CDownload();

		BOOL				GetAborted(){ return m_bAborted; }
		void				Update(ULONG ulProgress, ULONG ulMaximum,
								   LPCSTR lpszStatus);

		void				OnOK(){};
		void				OnCancel(){};

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	// Dialog Data
	//{{AFX_DATA(CDownload)
	enum { IDD = IDD_DOWNLOAD_PROGRESS };
	CProgressBar	m_ctrlProgressBar;
	CString	m_strProgress;
	CString	m_strMessage;
	//}}AFX_DATA

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDownload)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:

	// Generated message map functions
	//{{AFX_MSG(CDownload)
	afx_msg void OnAbort();
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__DOWNLOAD_H__)
