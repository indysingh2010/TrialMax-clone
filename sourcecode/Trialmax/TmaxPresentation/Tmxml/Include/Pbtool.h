//==============================================================================
//
// File Name:	pbtool.h
//
// Description:	This file contains the declarations of the CPBToolbar class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	06-19-02	1.00		Original Release
//==============================================================================
//{{AFX_INCLUDES()
#include "tmtool.h"
//}}AFX_INCLUDES
#if !defined(__PBTOOL_H__)
#define __PBTOOL_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <pbband.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CPBToolbar : public CPBBand
{
	private:

	public:
	
						CPBToolbar(CPageBar* pPageBar = 0);
					   ~CPBToolbar();

		int				GetToolbarHeight();
		int				GetInitialWidth();
		BOOL			SetToolbarProps(CTMTool& rSource);
		BOOL			EnableButton(short sId, BOOL bEnabled);

	protected:

		void			RecalcLayout();

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Dialog Data
	//{{AFX_DATA(CPBToolbar)
	enum { IDD = IDD_PB_TOOLBAR };
	CTMTool	m_ctrlToolbar;
	//}}AFX_DATA

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CPBToolbar)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:

	// Generated message map functions
	//{{AFX_MSG(CPBToolbar)
	virtual BOOL OnInitDialog();
	afx_msg void OnButtonClick(short sId, BOOL bChecked);
	DECLARE_EVENTSINK_MAP()
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__PBTOOL_H__)
