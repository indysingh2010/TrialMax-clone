//==============================================================================
//
// File Name:	sharepg.h
//
// Description:	This file contains the declaration of the CTMShareProperties
//				class. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	04-05-02	1.00		Original Release
//==============================================================================
#if !defined(__SHAREPG_H__)
#define __SHAREPG_H__

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
class CTMShareProperties : public COlePropertyPage
{
	private:

						DECLARE_DYNCREATE(CTMShareProperties)
						DECLARE_OLECREATE_EX(CTMShareProperties)

	public:
	
						CTMShareProperties();

	//{{AFX_DATA(CTMShareProperties)
	enum { IDD = IDD_PROPPAGE_TMSHARE };
	//}}AFX_DATA

	protected:
		virtual void	DoDataExchange(CDataExchange* pDX);  

	protected:
	//{{AFX_MSG(CTMShareProperties)
		// NOTE - ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations imsharetely before the previous line.

#endif // !defined(__SHAREPG_H__)
