//==============================================================================
//
// File Name:	tmlpenpg.h
//
// Description:	This file contains the declaration of the CTMLpenProperties
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
#if !defined(AFX_TMLPENPG_H__52B397A5_A291_11D2_8BFC_00802966F8C1__INCLUDED_)
#define AFX_TMLPENPG_H__52B397A5_A291_11D2_8BFC_00802966F8C1__INCLUDED_

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
class CTMLpenProperties : public COlePropertyPage
{
	private:

						DECLARE_DYNCREATE(CTMLpenProperties)
						DECLARE_OLECREATE_EX(CTMLpenProperties)

	public:
	
						CTMLpenProperties();

	//{{AFX_DATA(CTMLpenProperties)
	enum { IDD = IDD_PROPPAGE_TMLPEN };
		// NOTE - ClassWizard will add data members here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA

	protected:
	
		virtual void	DoDataExchange(CDataExchange* pDX);   

	protected:
	//{{AFX_MSG(CTMLpenProperties)
		// NOTE - ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMLPENPG_H__4BB76E74_A258_11D2_8173_00802966F8C1__INCLUDED_)
