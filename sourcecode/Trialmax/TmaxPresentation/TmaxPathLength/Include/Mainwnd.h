//==============================================================================
//
// File Name:	MainWnd.h
//
// Description:	This file contains the declaration of the CMainWnd class
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	03-30-2007	1.00		Original Release
//==============================================================================
#if !defined(__MAINWND_H__)
#define __MAINWND_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <GuiCtrl.h>
#include <TMIni.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define SEARCH_PROGRESS_MODULO	100

#define APP_SECTION				"TMAX PATH LENGTH"
#define APP_ROOT_FOLDER_LINE	"RootFolder"
#define	APP_LOG_FILE_LINE		"LogFile"
#define APP_MAX_LENGTH_LINE		"MaxLength"

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CMainWnd : public CDialog
{
	private:

		CTMIni				m_Ini;
		CBitmapButton		m_ctrlBrowseRoot;
		CBitmapButton		m_ctrlBrowseLog;
		int					m_iViolations;
		int					m_iProgress;
		long				m_lFiles;
		long				m_lFolders;
		CString				m_strIniFileSpec;
		FILE*				m_fptrLog;

	public:
	
							CMainWnd(CWnd* pParent = NULL);
		virtual			   ~CMainWnd();

	protected:

		void				Search(LPCSTR lpszFolder);
		void				Reset();
		void				Load();
		void				Save();
		void				Add(LPCSTR lpszFileSpec);

	public:
	// Dialog Data
	//{{AFX_DATA(CMainWnd)
	enum { IDD = IDD_TMAXPATHLENGTH_DIALOG };
	CProgressCtrl	m_ctrlProgressBar;
	CStatic	m_ctrlViolations;
	CEdit	m_ctrlMaxLength;
	CEdit	m_ctrlRootFolder;
	CEdit	m_ctrlLogFile;
	CString	m_strRootFolder;
	CString	m_strLogFile;
	int		m_iMaxLength;
	CString	m_strViolations;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMainWnd)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:
	HICON m_hIcon;

	// Generated message map functions
	//{{AFX_MSG(CMainWnd)
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg void OnBrowseRoot(); 
	afx_msg void OnBrowseLog(); 
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnSearch();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__MAINWND_H__)
