//==============================================================================
//
// File Name:	tmviewpg.h
//
// Description:	This file contains the declaration of the CTMViewProperties
//				class. This is the primary property page for tm_view6.ocx.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	07-11-97	1.00		Original Release
//==============================================================================
#if !defined(AFX_TMVIEWPG_H__FEB40E06_FA01_11D0_B002_008029EFD140__INCLUDED_)
#define AFX_TMVIEWPG_H__FEB40E06_FA01_11D0_B002_008029EFD140__INCLUDED_

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
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMViewProperties : public COlePropertyPage
{
	private:

	DECLARE_DYNCREATE(CTMViewProperties)
	DECLARE_OLECREATE_EX(CTMViewProperties)

	public:
	
						CTMViewProperties();

	protected:

		void			DoDataExchange(CDataExchange* pDX);   

	public:
	//{{AFX_DATA(CTMViewProperties)
	enum { IDD = IDD_PROPPAGE_TM_VIEW6 };
		// NOTE - ClassWizard will add data members here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA

	protected:
	//{{AFX_MSG(CTMViewProperties)
		// NOTE - ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMVIEWPG_H__FEB40E06_FA01_11D0_B002_008029EFD140__INCLUDED)
