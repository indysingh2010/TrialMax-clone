//==============================================================================
//
// File Name:	grabpg.h
//
// Description:	This file contains the declaration of the CTMGrabProperties
//				class. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	12-27-01	1.00		Original Release
//==============================================================================
#if !defined(__GRABPG_H__)
#define __GRABPG_H__

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
class CTMGrabProperties : public COlePropertyPage
{
	private:

						DECLARE_DYNCREATE(CTMGrabProperties)
						DECLARE_OLECREATE_EX(CTMGrabProperties)

	public:
	
						CTMGrabProperties();

	//{{AFX_DATA(CTMGrabProperties)
	enum { IDD = IDD_PROPPAGE_TMGRAB };
	//}}AFX_DATA

	protected:
		virtual void	DoDataExchange(CDataExchange* pDX);  

	protected:
	//{{AFX_MSG(CTMGrabProperties)
		// NOTE - ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations imgrabtely before the previous line.

#endif // !defined(__GRABPG_H__)
