//==============================================================================
//
// File Name:	webframe.h
//
// Description:	This file contains the declaration of the CWebFrame class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	02-20-02	1.00		Original Release
//==============================================================================
#if !defined(__WEBFRAME_H__)
#define __WEBFRAME_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <resource.h>
#include <webbrowser2.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define ID_BROWSER_CONTROL	1000
#define WM_FRAME_REDRAW		(WM_USER + 1)

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMBrowseCtrl;
class CErrorHandler;

class CWebFrame : public CDialog
{
	private:

		CWebBrowser2*			m_pBrowser;
		CTMBrowseCtrl*			m_pControl;
		CErrorHandler*			m_pErrors;
		CBrush*					m_pBackground;
		COLORREF				m_crBackground;
		CString					m_strFilename;

	public:
	
								CWebFrame(CTMBrowseCtrl* pControl, 
										  CErrorHandler* pErrors); 
		virtual				   ~CWebFrame();

		void					SetBackgroundColor(COLORREF crColor);
		void					Unload();
		void					Redraw();
		BOOL					Create();
		int						Load(LPCSTR lpszFilename);

	protected:

		void					RecalcLayout();
		BOOL					CreateBrowser();

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Dialog Data
	//{{AFX_DATA(CWebFrame)
	enum { IDD = IDD_FRAME };
	//}}AFX_DATA

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CWebFrame)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:

	// Generated message map functions
	//{{AFX_MSG(CWebFrame)
	afx_msg void OnSize(UINT nType, int cx, int cy);
	virtual BOOL OnInitDialog();
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg void OnDocumentComplete(LPDISPATCH pDisp, VARIANT FAR* URL);
	DECLARE_EVENTSINK_MAP()
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__WEBFRAME_H__)
