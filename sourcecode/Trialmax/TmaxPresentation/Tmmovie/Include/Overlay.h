//==============================================================================
//
// File Name:	player.h
//
// Description:	This file contains the declaration of the COverlay class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	09-25-98	1.00		Original Release
//==============================================================================
#if !defined(__OVERLAY_H__)
#define __OVERLAY_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <resource.h>
#include <tmview.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

#define TMMOVIE_OVERLAYID	100

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	Forward declarations
class CTMMovieCtrl;

class COverlay : public CDialog
{
	private:

		CTMMovieCtrl*		m_pTMMovie;
		int					m_iMaxWidth;
		COLORREF			m_crBackground;
		CBrush*				m_pBackground;

	public:
	
							COverlay(CTMMovieCtrl* pParent);
						   ~COverlay();

		BOOL				Create();
		int					SetMaxWidth(int iWidth);
		int					SetFilename(LPCSTR lpFilename);
		int					GetHeight();
		void				SetBackColor(COLORREF crBackground);
		void				Redraw();

	protected:

	//	The remainder of this declaration is maintained by ClassWizard

	public:
	//{{AFX_DATA(COverlay)
	enum { IDD = IDD_OVERLAY };
	CTm_view	m_TMView;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(COverlay)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(COverlay)
	virtual BOOL OnInitDialog();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(__OVERLAY_H__)
