//==============================================================================
//
// File Name:	cleanup.h
//
// Description:	This file contains the declaration of the CCleanup class. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2001
//
//==============================================================================
//	Date		Revision    Description
//	06-26-01	1.00		Original Release
//==============================================================================
#if !defined(AFX_CLEANUP_H__B97E7540_6A53_11D5_8F0A_00802966F8C1__INCLUDED_)
#define AFX_CLEANUP_H__B97E7540_6A53_11D5_8F0A_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <colorctl.h>
#include <lead.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMLead;

class CCleanup : public CDialog
{
	private:

		CTMLead*		m_pTMLead;
		CString			m_strSaveAs;

	public:

						CCleanup(CTMLead* pTMLead, LPCTSTR lpszSaveAs = 0);

	protected:

		void			EnableDeskew(BOOL bEnable);
		void			EnableHolePunchRemoval(BOOL bEnable);
		void			EnableBorderRemoval(BOOL bEnable);
		void			EnableSmooth(BOOL bEnable);
		void			EnableDotRemoval(BOOL bEnable);

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CCleanup)
	enum { IDD = IDD_CLEANUP };
	CButton	m_ctrlSave;
	CButton	m_ctrlUndo;
	CLead	m_Undo;
	CStatic	m_ctrlDeskewColorLabel;
	CButton	m_ctrlDeskewGroup;
	CColorPushbutton m_ctrlDeskewColor;
	CButton	m_ctrlSmoothLong;
	CStatic	m_ctrlSmoothLengthLabel;
	CEdit	m_ctrlSmoothLength;
	CButton	m_ctrlSmoothGroup;
	CButton	m_ctrlBordersGroup;
	CEdit	m_ctrlBordersPercent;
	CStatic	m_ctrlBordersPercentLabel;
	CEdit	m_ctrlBordersNoise;
	CStatic	m_ctrlBordersNoiseLabel;
	CEdit	m_ctrlBordersVariance;
	CStatic	m_ctrlBordersVarianceLabel;
	CButton	m_ctrlBordersTop;
	CButton	m_ctrlBordersRight;
	CButton	m_ctrlBordersLeft;
	CButton	m_ctrlBordersBottom;
	CButton	m_ctrlHolesTop;
	CButton	m_ctrlHolesRight;
	CButton	m_ctrlHolesLeft;
	CButton	m_ctrlHolesBottom;
	CStatic	m_ctrlDotsMinWidthLabel;
	CEdit	m_ctrlDotsMinWidth;
	CStatic	m_ctrlDotsMinHeightLabel;
	CEdit	m_ctrlDotsMinHeight;
	CStatic	m_ctrlDotsMaxWidthLabel;
	CEdit	m_ctrlDotsMaxWidth;
	CStatic	m_ctrlDotsMaxHeightLabel;
	CEdit	m_ctrlDotsMaxHeight;
	CButton	m_ctrlDotsGroup;
	BOOL	m_bDeskew;
	BOOL	m_bDespeckle;
	BOOL	m_bRemoveBorders;
	BOOL	m_bRemoveDots;
	BOOL	m_bRemoveHoles;
	BOOL	m_bSmooth;
	long	m_lDotsMaxHeight;
	long	m_lDotsMaxWidth;
	long	m_lDotsMinHeight;
	long	m_lDotsMinWidth;
	CStatic	m_ctrlHolesMinWidthLabel;
	CEdit	m_ctrlHolesMinWidth;
	CStatic	m_ctrlHolesMinHeightLabel;
	CEdit	m_ctrlHolesMinHeight;
	CStatic	m_ctrlHolesMaxWidthLabel;
	CEdit	m_ctrlHolesMaxWidth;
	CStatic	m_ctrlHolesMaxHeightLabel;
	CEdit	m_ctrlHolesMaxHeight;
	CButton	m_ctrlHolesGroup;
	long	m_lHolesMaxHeight;
	long	m_lHolesMaxWidth;
	long	m_lHolesMinHeight;
	long	m_lHolesMinWidth;
	BOOL	m_bHolesBottom;
	BOOL	m_bHolesLeft;
	BOOL	m_bHolesRight;
	BOOL	m_bHolesTop;
	BOOL	m_bBordersBottom;
	BOOL	m_bBordersLeft;
	BOOL	m_bBordersRight;
	BOOL	m_bBordersTop;
	long	m_lBordersPercent;
	long	m_lBordersNoise;
	long	m_lBordersVariance;
	long	m_lSmoothLength;
	BOOL	m_bSmoothLong;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CCleanup)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:

	// Generated message map functions
	//{{AFX_MSG(CCleanup)
	virtual BOOL OnInitDialog();
	afx_msg void OnClean();
	afx_msg void OnSave();
	afx_msg void OnUndo();
	afx_msg void OnRemoveBorders();
	afx_msg void OnRemoveDots();
	afx_msg void OnRemoveHoles();
	afx_msg void OnSmooth();
	afx_msg void OnDeskewColor();
	afx_msg void OnDeskew();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_CLEANUP_H__B97E7540_6A53_11D5_8F0A_00802966F8C1__INCLUDED_)
