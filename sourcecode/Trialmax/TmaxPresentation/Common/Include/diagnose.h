//==============================================================================
//
// File Name:	diagnose.h
//
// Description:	This file contains the declaration of classes used to implement
//				a Trailmax diagnostics window.
//
// Author:		Kenneth Moore
//
// Copyright FTI Consulting - All Rights Reserved
//
//==============================================================================
//	Date		Revision    Description
//	07-16-2002	1.00		Original Release
//==============================================================================
#if !defined(__DIAGNOSTICS_H__)
#define __DIAGNOSTICS_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <resource.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define DIAGNOSTICS_LIST_BORDER		4
#define DIAGNOSTICS_MAX_MESSAGES	32

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CDiagMessage : public CObject
{
	private:

	public:

		CString				m_strTime;		
		CString				m_strDate;		
		CString				m_strText;

     						CDiagMessage();
						   ~CDiagMessage();
	protected:

};

class CDiagMessages : public CObList
{
	private:

	public:

							CDiagMessages();
						   ~CDiagMessages();

		BOOL				Add(CDiagMessage* pMessage);
		void				Flush(BOOL bDelete);
		void				Remove(CDiagMessage* pMessage, BOOL bDelete);
		void				RemoveTail(BOOL bDelete);
		POSITION			Find(CDiagMessage* pMessage);

		//	List iteration members
		CDiagMessage*		First();
		CDiagMessage*		Last();
		CDiagMessage*		Next();
		CDiagMessage*		Prev();

	protected:

		POSITION			m_NextPos;	//	Next position of local iterator
		POSITION			m_PrevPos;	//	Previous position of local iterator
};

class CDiagnostics : public CDialog
{
	private:

		CDiagMessages		m_Messages;

		SYSTEMTIME			m_Time;

		HWND				m_hNotify;
		UINT				m_uNotify;

		BOOL				m_bLogEnabled;
		CString				m_strLogFolder;
		CString				m_strLogFilename;
	
	public:
	
							CDiagnostics(CWnd* pParent = 0);
						   ~CDiagnostics();

		void				OnOK(){};
		void				OnCancel();
		BOOL				Create(CWnd* pParent);

		void				SetLogEnabled(BOOL bEnabled);
		void				SetLogFolder(LPCSTR lpFolder);
		void				SetLogFilename(LPCSTR lpFilename);
		void				SetNotificationHandle(HWND hWnd);
		void				SetNotificationMessage(UINT uMsg);

		void				Report(LPCSTR lpUser, LPCSTR lpFormat, ...);
		void				Report(LPCSTR lpUser, UINT uId, LPCSTR lpArg1 = 0, 
								   LPCSTR lpArg2 = 0);
	
	protected:

		void				SetListPosition();
		void				LogMessage(CDiagMessage* pMessage);
		void				AddMessage(CDiagMessage* pMessage);

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	//{{AFX_DATA(CDiagnostics)
	enum { IDD = IDD_DIAGNOSTICS };
	CListBox	m_ctrlMessages;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDiagnostics)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:

	// Generated message map functions
	//{{AFX_MSG(CDiagnostics)
	afx_msg void OnSize(UINT nType, int cx, int cy);
	virtual BOOL OnInitDialog();
	afx_msg void OnDoubleClick();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__DIAGNOSTICS_H__)
