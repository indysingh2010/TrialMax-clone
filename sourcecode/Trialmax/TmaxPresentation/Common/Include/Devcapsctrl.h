//==============================================================================
//
// File Name:	devcapsctrl.h
//
// Description:	This file contains the declaration of the CDevCapsCtrl class. 
//				This is a custom list box class that will display the device
//				capabilities of a device context.
//
// Author:		Kenneth Moore
//
// Copyright FTI Consulting - All Rights Reserved
//
//==============================================================================
//	Date		Revision    Description
//	06-16-2006	1.00		Original Release
//==============================================================================
#if !defined(__DEVCAPSCTRL_H__)
#define __DEVCAPSCTRL_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define DEVCAPSCTRL_COLUMN_NAME				0
#define DEVCAPSCTRL_COLUMN_VALUE			1

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CDeviceCapsCtrl : public CListCtrl
{
	private:
		
	public:
	
						CDeviceCapsCtrl();
		virtual		   ~CDeviceCapsCtrl();

		virtual void	Clear();
		virtual void	Initialize(CDC* pdc = NULL);
		virtual BOOL	Fill(CDC* pdc);
		virtual BOOL	Fill(HGLOBAL hDevMode);

	protected:

		virtual void	Add(LPCSTR lpszName, int iValue, BOOL bYesNo = TRUE, BOOL bHex = FALSE);
		virtual void	Add(LPCSTR lpszName, LPCSTR lpszValue);
		virtual CString	GetPrintQuality(int iDevModePQ);
		
	//	Class Wizard from here on...

	// Attributes
	public:

	// Operations
	public:

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDeviceCapsCtrl)
	//}}AFX_VIRTUAL

	// Implementation
	public:

	// Generated message map functions
	protected:
	//{{AFX_MSG(CDeviceCapsCtrl)
		// NOTE - the ClassWizard will add and remove member functions here.
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__DEVCAPSCTRL_H__)
