//==============================================================================
//
// File Name:	pbtext.h
//
// Description:	This file contains the declarations of the CPBText class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	06-19-02	1.00		Original Release
//==============================================================================
#if !defined(__PBTEXT_H__)
#define __PBTEXT_H__

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
class CPBText : public CPBBand
{
	private:

	public:
	
						CPBText(CPageBar* pPageBar = 0);
					   ~CPBText();

		void			SetXmlMedia(CXmlMedia* pXmlMedia);
		void			SetXmlPage(CXmlPage* pXmlPage);
		void			SetFont(CFont* pFont);

	protected:

		void			RecalcLayout();
		void			Update();

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Dialog Data
	//{{AFX_DATA(CPBText)
	enum { IDD = IDD_PB_TEXT };
	CStatic	m_ctrlText;
	//}}AFX_DATA

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CPBText)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:

	// Generated message map functions
	//{{AFX_MSG(CPBText)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__PBTEXT_H__)
