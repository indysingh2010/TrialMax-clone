//==============================================================================
//
// File Name:	barpage.h
//
// Description:	This file contains the declaration of the CBarPage class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-10-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_BARPAGE_H__2B610F41_C679_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_BARPAGE_H__2B610F41_C679_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <tmtool.h>
#include <tmini.h>
#include <tmtbdefs.h>
#include <handler.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CBarPage : public CDialog
{
	private:

		CString				m_strSection;
		CTMIni*				m_pIni;
		CErrorHandler*		m_pErrors;
		CImageList			m_Images;
		int					m_iId;
		int					m_iImage;
		int					m_iToolbar;
		short				m_aMap[TMTB_MAXBUTTONS];
		BOOL				m_aMask[TMTB_MAXBUTTONS];

	public:
	
							CBarPage(CWnd* pParent = NULL,
									 int iId = 0);
							
		void				ReadIniFile(CTMIni* pIni);
		void				WriteIniFile(CTMIni* pIni);
		void				SetSection(LPCSTR lpSection);
		void				SetHandler(CErrorHandler* pHandler); 
		void				SetId(int iId);
		void				SetButtonMask(int iId);

		static int CALLBACK DoComparison(LPARAM, LPARAM, LPARAM);

	protected:

		void				LoadTemplates();
		void				LoadTemplate(LPCSTR lpTemplate);
		void				ResetOptions();
		void				UpdateMap();
		void				SetControlStates();
		int					GetCurSel(CListCtrl* pList);
		BOOL				IsMasterToolbar();
		
	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CBarPage)
	enum { IDD = IDD_BAR_PAGE };
	CStatic	m_ctrlToolbarLabel;
	CStatic	m_ctrlTemplatesLabel;
	CButton	m_ctrlPropertiesGroup;
	CStatic	m_ctrlAvailableLabel;
	CButton	m_ctrlUseMaster;
	CButton	m_ctrlVisible;
	CButton	m_ctrlStretch;
	CButton	m_ctrlSmall;
	CButton	m_ctrlMedium;
	CButton	m_ctrlLarge;
	CButton	m_ctrlFloat;
	CButton	m_ctrlFlat;
	CTMTool	m_Toolbar;
	CButton	m_ctrlSave;
	CButton	m_ctrlLoad;
	CComboBox	m_ctrlTemplates;
	CButton	m_ctrlBefore;
	CButton	m_ctrlAfter;
	CButton	m_ctrlRemove;
	CListCtrl	m_ctrlToolbar;
	CListCtrl	m_ctrlImages;
	BOOL	m_bFlat;
	BOOL	m_bFloat;
	BOOL	m_bStretch;
	BOOL	m_bVisible;
	int		m_sSize;
	BOOL	m_bUseMaster;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CBarPage)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(CBarPage)
	virtual BOOL OnInitDialog();
	afx_msg void OnLoad();
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg void OnClose();
	afx_msg void OnDblClk();
	afx_msg void OnAfter();
	afx_msg void OnBefore();
	afx_msg void OnRemove();
	afx_msg void OnSave();
	afx_msg void OnClickUseMaster();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_BARPAGE_H__2B610F41_C679_11D3_8177_00802966F8C1__INCLUDED_)
