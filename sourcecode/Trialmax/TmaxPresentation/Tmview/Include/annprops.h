//==============================================================================
//
// File Name:	annprops.h
//
// Description:	This file contains the declaration of the CAnnotationProperties
//				class. This is a dialog box that allows the user to set 
//				properties associated with annotation of the image.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	07-14-97	1.00		Original Release
//==============================================================================
#if !defined(__ANNPROPS_H__)
#define __ANNPROPS_H__

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

//	Color selection indices. The order here must match the order in which they
//	appear on the dialog box
#define DRAWCOLOR						0
#define HIGHLIGHTCOLOR					1
#define REDACTCOLOR						2
#define CALLOUTCOLOR					3
#define CALLFRAMECOLOR					4
#define CALLHANDLECOLOR					5
#define BACKGROUNDCOLOR					6
#define SPLITFRAMECOLOR					7
#define SELECTORCOLOR					8

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CAnnotationProperties : public CDialog
{
	private:

		CBitmapButton		m_btnRed;
		CBitmapButton		m_btnBlue;
		CBitmapButton		m_btnGreen;
		CBitmapButton		m_btnMagenta;
		CBitmapButton		m_btnCyan;
		CBitmapButton		m_btnYellow;
		CBitmapButton		m_btnBlack;
		CBitmapButton		m_btnGrey;
		CBitmapButton		m_btnWhite;
		CBitmapButton		m_btnDarkRed;
		CBitmapButton		m_btnDarkBlue;
		CBitmapButton		m_btnDarkGreen;
		CBitmapButton		m_btnLightRed;
		CBitmapButton		m_btnLightBlue;
		CBitmapButton		m_btnLightGreen;

		long				m_lFlags;
		
	public:
	
		int					m_nColorTool;
		int					m_nHighlightColor;
		int					m_nBackgroundColor;
		int					m_nRedactColor;
		int					m_nDrawColor;
		int					m_nCalloutColor;
		int					m_nCalloutFrameColor;
		int					m_nCalloutHandleColor;
		int					m_nSplitFrameColor;
		int					m_nSelectorColor;
		short				m_sFontSize;
		BOOL				m_bFontUnderline;
		BOOL				m_bFontStrikeThrough;
		BOOL				m_bFontBold;

							CAnnotationProperties(long lFlags,
												  CWnd* pParent = NULL);   
	
	protected:

		void				SetColor(int nColor);
		void				SelectColor(int nColorTool);

	public:
	//{{AFX_DATA(CAnnotationProperties)
	enum { IDD = IDD_ANNOTATION };
	CStatic	m_ctrlFontName;
	CSpinButtonCtrl	m_SpinSelector;
	CSpinButtonCtrl	m_SpinSplitThickness;
	CSpinButtonCtrl	m_SpinFrameThickness;
	CStatic	m_ctrlPanPercentageLabel;
	CButton	m_ctrlHideScrollBars;
	CEdit	m_ctrlPanPercent;
	CSpinButtonCtrl	m_SpinPan;
	CStatic	m_ctrlRotationLabel;
	CButton	m_ctrlFitToImage;
	CEdit	m_ctrlRotation;
	CButton	m_ctrlScaleImage;
	CSpinButtonCtrl	m_SpinRotation;
	CSpinButtonCtrl	m_SpinZoom;
	CSpinButtonCtrl	m_SpinThick;
	UINT	m_nThickness;
	UINT	m_nZoomFactor;
	int		m_nColorSelect;
	BOOL	m_bFitToImage;
	BOOL	m_bScaleImage;
	int		m_nRotation;
	UINT	m_nPanPercent;
	BOOL	m_bHideScrollBars;
	int		m_sZoomOnLoad;
	int		m_nFrameThickness;
	BOOL	m_bRightClickPan;
	BOOL	m_bSplitScreen;
	BOOL	m_bSyncPanes;
	int		m_nSplitThickness;
	BOOL	m_bSyncCalloutAnn;
	BOOL	m_bSelectorVisible;
	int		m_nSelectorSize;
	BOOL	m_bKeepAspect;
	int		m_iBitonal;
	BOOL	m_bZoomToRect;
	int		m_nTool;
	BOOL	m_bResizeableCallouts;
	CString	m_strFontName;
	BOOL	m_bShadeOnCallout;
	short   m_sCalloutShadeGrayscale;
	BOOL	m_bPanCallouts;
	BOOL	m_bZoomCallouts;
	//}}AFX_DATA

	//{{AFX_VIRTUAL(CAnnotationProperties)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    
	//}}AFX_VIRTUAL

	protected:
	//{{AFX_MSG(CAnnotationProperties)
	afx_msg void OnRed();
	virtual BOOL OnInitDialog();
	afx_msg void OnBlack();
	afx_msg void OnBlue();
	afx_msg void OnCyan();
	afx_msg void OnGreen();
	afx_msg void OnGrey();
	afx_msg void OnMagenta();
	afx_msg void OnWhite();
	afx_msg void OnYellow();
	afx_msg void OnHighlight();
	afx_msg void OnRedact();
	afx_msg void OnDrawtool();
	afx_msg void OnDefaults();
	afx_msg void OnBackground();
	afx_msg void OnCallout();
	afx_msg void OnFrameColor();
	afx_msg void OnHandleColor();
	afx_msg void OnSplitFrame();
	afx_msg void OnSelectorColor();
	afx_msg void OnDarkBlue();
	afx_msg void OnDarkGreen();
	afx_msg void OnDarkRed();
	afx_msg void OnLightBlue();
	afx_msg void OnLightGreen();
	afx_msg void OnLightRed();
	afx_msg void OnChangeFont();
	//}}AFX_MSG
	
	DECLARE_MESSAGE_MAP()
};

#endif	//	#if !defined(__ANNPROPS_H__)
