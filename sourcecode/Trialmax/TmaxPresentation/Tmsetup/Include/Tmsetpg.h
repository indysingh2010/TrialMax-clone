//==============================================================================
//
// File Name:	tmsetpg.h
//
// Description:	This file contains the declaration of the CTMSetupProperties
//				class. This is the property page that allows the user to define
//				the properties of the ocx control.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-29-00	1.00		Original Release
//==============================================================================
#if !defined(AFX_TMSETPG_H__98CB02D6_D4CA_11D3_8177_00802966F8C1__INCLUDED_)
#define AFX_TMSETPG_H__98CB02D6_D4CA_11D3_8177_00802966F8C1__INCLUDED_

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
class CTMSetupProperties : public COlePropertyPage
{
	private:

						DECLARE_DYNCREATE(CTMSetupProperties)
						DECLARE_OLECREATE_EX(CTMSetupProperties)

	public:
	
						CTMSetupProperties();

	protected:

	//	The remainder of this declaration is maintained by ClassWizard
	public:

	//{{AFX_DATA(CTMSetupProperties)
	enum { IDD = IDD_PROPPAGE_TMSETUP };
		// NOTE - ClassWizard will add data members here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	protected:
	//{{AFX_MSG(CTMSetupProperties)
		// NOTE - ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMSETPG_H__98CB02D6_D4CA_11D3_8177_00802966F8C1__INCLUDED_)
