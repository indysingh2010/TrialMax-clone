//==============================================================================
//
// File Name:	tmxmlpg.h
//
// Description:	This file contains the declaration of the CTMXmlProperties
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
#if !defined(AFX_TMXMLPG_H__2CC295D5_27A3_11D5_8F0A_00802966F8C1__INCLUDED_)
#define AFX_TMXMLPG_H__2CC295D5_27A3_11D5_8F0A_00802966F8C1__INCLUDED_

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
class CTMXmlProperties : public COlePropertyPage
{
	private:

						DECLARE_DYNCREATE(CTMXmlProperties)
						DECLARE_OLECREATE_EX(CTMXmlProperties)

	public:
	
						CTMXmlProperties();

	//{{AFX_DATA(CTMXmlProperties)
	enum { IDD = IDD_PROPPAGE_TMXML };
		// NOTE - ClassWizard will add data members here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_DATA

	protected:
	
		virtual void	DoDataExchange(CDataExchange* pDX);   

	protected:
	//{{AFX_MSG(CTMXmlProperties)
		// NOTE - ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMXMLPG_H__2CC295D5_27A3_11D5_8F0A_00802966F8C1__INCLUDED_)
