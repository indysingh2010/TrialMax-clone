//==============================================================================
//
// File Name:	config.h
//
// Description:	This file contains the declaration of the CConfigure class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	05-04-98	1.30		Original Release
//==============================================================================
#if !defined(AFX_CONFIG_H__1B5D9B41_E2A0_11D1_B179_008029EFD140__INCLUDED_)
#define AFX_CONFIG_H__1B5D9B41_E2A0_11D1_B179_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <tmtbdefs.h>
#include <afxcmn.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	Forward declarations
class CTMToolCtrl;

class CConfigure : public CDialog
{
	private:

		CTMToolCtrl*	m_pToolbar;
		CString*		m_pLabels;
		CImageList		m_Images;
		int				m_iImage;
		int				m_iToolbar;
		
	public:

									
		short			m_aMap[TMTB_MAXBUTTONS];
		char			m_szMask[TMTB_MAXBUTTONS + 1]; //	NULL terminated
	
						CConfigure(CTMToolCtrl* pParent, CString* pLabels);  

		static int CALLBACK DoComparison(LPARAM, LPARAM, LPARAM);

	protected:

		int				GetCurSel(CListCtrl* pList);	
		
	//	The remainder of this declaration is maintained by Class Wizard
	public:
	//{{AFX_DATA(CConfigure)
	enum { IDD = IDD_CONFIGURE_TMTOOL };
	CButton	m_ctrlRemove;
	CButton	m_ctrlBefore;
	CButton	m_ctrlAfter;
	CListCtrl	m_ctrlImages;
	CListCtrl	m_ctrlToolbar;
	BOOL	m_bFlat;
	int		m_iSize;
	BOOL	m_bToolTips;
	int		m_iTop;
	BOOL	m_bStretch;
	int		m_iRows;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CConfigure)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);   
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(CConfigure)
	virtual BOOL OnInitDialog();
	virtual void OnOK();
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg void OnInsertAfter();
	afx_msg void OnInsertBefore();
	afx_msg void OnRemove();
	afx_msg void OnClose();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_CONFIG_H__1B5D9B41_E2A0_11D1_B179_008029EFD140__INCLUDED_)
