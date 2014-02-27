//==============================================================================
//
// File Name:	tmprntpg.h
//
// Description:	This file contains the declaration of the CTMPrintProperties
//				class. This is the property page that allows the user to define
//				the properties of the ocx control.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	12-13-99	1.00		Original Release
//==============================================================================
#if !defined(AFX_TMPRNTPG_H__CCAA2376_B13D_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_TMPRNTPG_H__CCAA2376_B13D_11D3_8177_00802966F8C1__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMPrintProperties : public COlePropertyPage
{
	private:

						DECLARE_DYNCREATE(CTMPrintProperties)
						DECLARE_OLECREATE_EX(CTMPrintProperties)

	public:
	
						CTMPrintProperties();

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CTMPrintProperties)
	enum { IDD = IDD_PROPPAGE_TMPRINT };
		// NOTE - ClassWizard will add data members here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	protected:
	//{{AFX_MSG(CTMPrintProperties)
		// NOTE - ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMPRNTPG_H__CCAA2376_B13D_11D3_8177_00802966F8C1__INCLUDED)
