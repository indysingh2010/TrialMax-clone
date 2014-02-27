//==============================================================================
//
// File Name:	pptool.h
//
// Description:	This file contains the declaration of the CPPToolbar class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	06-17-01	1.00		Original Release
//==============================================================================
#if !defined(__PPTOOL_H__)
#define __PPTOOL_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <tmtbdefs.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMTool;

class CPPToolbar : public CPropertyPage
{
	private:

							DECLARE_DYNCREATE(CPPToolbar)

		CTMTool*			m_pToolbar;
		CImageList			m_Images;
		int					m_iImage;
		int					m_iToolbar;
		short				m_aMap[TMTB_MAXBUTTONS];
		char				m_szMask[TMTB_MAXBUTTONS + 1];
		BOOL				m_bInitialized;

	public:

							CPPToolbar();
						   ~CPPToolbar();

		void				SetToolbar(CTMTool* pToolbar){ m_pToolbar = pToolbar; }
		short*				GetButtonMap(){ return m_aMap; }
		BOOL				GetInitialized(){ return m_bInitialized; }

		static int CALLBACK DoComparison(LPARAM, LPARAM, LPARAM);

	protected:

		void				FillImageLists();
		void				UpdateMap();
		int					GetCurSel(CListCtrl* pList);

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Dialog Data
	//{{AFX_DATA(CPPToolbar)
	enum { IDD = IDD_TOOLBAR_PAGE };
	CButton	m_ctrlRemove;
	CButton	m_ctrlBefore;
	CButton	m_ctrlAfter;
	CListCtrl	m_ctrlToolbar;
	CListCtrl	m_ctrlImages;
	int		m_iOrientation;
	int		m_iSize;
	short	m_sRows;
	BOOL	m_bFlat;
	//}}AFX_DATA


	// Overrides
	// ClassWizard generate virtual function overrides
	//{{AFX_VIRTUAL(CPPToolbar)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:
	// Generated message map functions
	//{{AFX_MSG(CPPToolbar)
	virtual BOOL OnInitDialog();
	afx_msg void OnAfter();
	afx_msg void OnBefore();
	afx_msg void OnRemove();
	afx_msg void OnClose();
	afx_msg void OnTimer(UINT nIDEvent);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__PPTOOL_H__)
