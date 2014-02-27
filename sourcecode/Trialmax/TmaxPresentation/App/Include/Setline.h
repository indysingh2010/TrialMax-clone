//==============================================================================
//
// File Name:	setline.h
//
// Description:	This file contains the declarations of the CSetLine class
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	04-21-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_SETLINE_H__B513E421_1835_11D4_8178_00802966F8C1__INCLUDED_)
#define AFX_SETLINE_H__B513E421_1835_11D4_8178_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define MINIMUM_SETLINE_HEIGHT	36

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTranscripts;
class CTranscript;
class CPlaylist;

class CSetLine : public CDialog
{
	private:

		CTranscripts*		m_pTranscripts;
		CPlaylist*			m_pPlaylist;
		long				m_lTranscript;

		RECT				m_rcPos;
		CBrush				m_brBackGnd;
		CBrush				m_brForeGnd;

	public:
		
							
							CSetLine(CWnd* pParent = NULL);

		void				SetTranscripts(CTranscripts* pTranscripts);
		void				SetPlaylist(CPlaylist* pPlaylist);
		void				SetMessage(LPCSTR lpMessage);
		void				SetLabel(LPCSTR lpLabel);
		void				SetPage(int iPage);
		void				SetLine(int iLine);
		void				SetTranscript(long lTranscriptId);
		void				SetPos(RECT* pPos);

		int					GetPage(){ return m_iPage; }
		int					GetLine(){ return m_iLine; }
		long				GetTranscript(){ return m_lTranscript; }

		void				OnOK();

	protected:

		void				FillTranscripts();
		void				AddTranscript(CTranscript* pTranscript);
		void				MsgBox(LPCSTR lpszTitle);
		int					GetIndex(long lTranscriptId);

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CSetLine)
	enum { IDD = IDD_SETLINE };
	CStatic	m_ctrlMessage;
	CComboBox	m_ctrlTranscripts;
	CEdit	m_ctrlLine;
	CEdit	m_ctrlPage;
	CString	m_strLabel;
	int		m_iLine;
	CString	m_strMessage;
	int		m_iPage;
	int		m_iTranscripts;
	//}}AFX_DATA


	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CSetLine)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:

	// Generated message map functions
	//{{AFX_MSG(CSetLine)
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SETLINE_H__B513E421_1835_11D4_8178_00802966F8C1__INCLUDED_)
