//==============================================================================
//
// File Name:	tmtextpg.h
//
// Description:	This file contains the declaration of the CTMTextProperties
//				class. This is the property page that allows the user to define
//				the properties of the ocx control.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	01-02-99	1.00		Original Release
//==============================================================================
#if !defined(AFX_TMTEXTPG_H__07F27A56_ABF9_11D2_8C08_00802966F8C1__INCLUDED_)
#define AFX_TMTEXTPG_H__07F27A56_ABF9_11D2_8C08_00802966F8C1__INCLUDED_

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
class CTMTextProperties : public COlePropertyPage
{
	private:

						DECLARE_DYNCREATE(CTMTextProperties)
						DECLARE_OLECREATE_EX(CTMTextProperties)

	public:
	
						CTMTextProperties();

	//{{AFX_DATA(CTMTextProperties)
	enum { IDD = IDD_PROPPAGE_TMTEXT };
		// NOTE - ClassWizard will add data members here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA

	protected:
	
		virtual void	DoDataExchange(CDataExchange* pDX);   

	protected:
	//{{AFX_MSG(CTMTextProperties)
		// NOTE - ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMTEXTPG_H__07F27A56_ABF9_11D2_8C08_00802966F8C1__INCLUDED_)
