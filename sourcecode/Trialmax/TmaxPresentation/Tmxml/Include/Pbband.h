//==============================================================================
//
// File Name:	pbband.h
//
// Description:	This file contains the declarations of the CPBBand class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	06-19-02	1.00		Original Release
//==============================================================================
#if !defined(__PBBAND_H__)
#define __PBBAND_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <tmtool.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#include <tmtool.h>

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CPageBar;
class CXmlMedia;
class CXmlPage;

class CPBBand : public CDialog
{
	private:

	public:
	
							CPBBand(CPageBar* pPageBar = 0, UINT uResourceId = IDD_PB_BAND);
		virtual 		   ~CPBBand();

		virtual void		SetXmlMedia(CXmlMedia* pXmlMedia);
		virtual void		SetXmlPage(CXmlPage* pXmlPage);
		virtual void		SetInitialWidth(int iWidth){ m_iInitialWidth = iWidth; }
		virtual void		SetMinimumWidth(int iWidth){ m_iMinimumWidth = iWidth; }
		virtual void		SetResizable(BOOL bResizable){ m_bResizable = bResizable; }
		virtual void		SetId(int iId){ m_iId = iId; }
		virtual void		RecalcLayout();
		virtual void		SetFont(CFont* pFont);
		virtual int			GetId(){ return m_iId; }
		virtual int			GetInitialWidth();
		virtual int			GetMinimumWidth(){ return m_iMinimumWidth; }
		virtual BOOL		GetResizable(){ return m_bResizable; }
		virtual BOOL		Create(CPageBar* pPageBar);

	protected:

		CPageBar*			m_pPageBar;
		CXmlMedia*			m_pXmlMedia;
		CXmlPage*			m_pXmlPage;
		UINT				m_uResourceId;
		RECT				m_rcWnd;
		int					m_iId;
		int					m_iInitialWidth;
		int					m_iMinimumWidth;
		CBrush*				m_pbrBackground;
		BOOL				m_bResizable;


	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Dialog Data
	//{{AFX_DATA(CPBBand)
	enum { IDD = IDD_ABOUTBOX_TMXML };
		// NOTE: the ClassWizard will add data members here
	//}}AFX_DATA

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CPBBand)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:

	// Generated message map functions
	//{{AFX_MSG(CPBBand)
	virtual BOOL OnInitDialog();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__PBBAND_H__)
