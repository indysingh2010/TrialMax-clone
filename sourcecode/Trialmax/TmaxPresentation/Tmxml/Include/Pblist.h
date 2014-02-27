//==============================================================================
//
// File Name:	pblist.h
//
// Description:	This file contains the declarations of the CPBList class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	06-19-02	1.00		Original Release
//==============================================================================
#if !defined(__PBLIST_H__)
#define __PBLIST_H__

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
class CPBList : public CPBBand
{
	private:
CBrush m_brBrush;
	public:
	
						CPBList(CPageBar* pPageBar = 0);
					   ~CPBList();

		void			SetXmlMedia(CXmlMedia* pXmlMedia);
		void			SetXmlPage(CXmlPage* pXmlPage);
		void			RecalcLayout();
		void			SetFont(CFont* pFont);

	protected:

		void				FillPageList();
		int					GetPageIndex(CXmlPage* pXmlPage);

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Dialog Data
	//{{AFX_DATA(CPBList)
	enum { IDD = IDD_PB_LIST };
	CComboBox	m_ctrlPages;
	//}}AFX_DATA

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CPBList)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:

	// Generated message map functions
	//{{AFX_MSG(CPBList)
	afx_msg void OnSelChanged();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__PBLIST_H__)
