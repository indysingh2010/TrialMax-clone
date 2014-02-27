//==============================================================================
//
// File Name:	tmtoolpg.h
//
// Description:	This file contains the declaration of the CTMToolProperties
//				class. This is the property page that allows the user to define
//				the properties of the ocx control.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	04-21-98	1.00		Original Release
//==============================================================================
#if !defined(AFX_TMTOOLPG_H__BBD917A9_D89D_11D1_B16C_008029EFD140__INCLUDED_)
#define AFX_TMTOOLPG_H__BBD917A9_D89D_11D1_B16C_008029EFD140__INCLUDED_

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
class CTMToolProperties : public COlePropertyPage
{
	private:

						DECLARE_DYNCREATE(CTMToolProperties)
						DECLARE_OLECREATE_EX(CTMToolProperties)

	public:
	
						CTMToolProperties();

	//{{AFX_DATA(CTMToolProperties)
	enum { IDD = IDD_PROPPAGE_TMTOOL };
		// NOTE - ClassWizard will add data members here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA

	protected:
	
		virtual void	DoDataExchange(CDataExchange* pDX);   

	protected:
	//{{AFX_MSG(CTMToolProperties)
		// NOTE - ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMTOOLPG_H__BBD917A9_D89D_11D1_B16C_008029EFD140__INCLUDED)
