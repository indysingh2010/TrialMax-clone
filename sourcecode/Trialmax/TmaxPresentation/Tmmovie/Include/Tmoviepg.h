//==============================================================================
//
// File Name:	tmoviepg.h
//
// Description:	This file contains the declaration of the CTMMovieProperties
//				class. This is the property page that allows the user to define
//				the properties of the ocx control.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	06-11-98	1.00		Original Release
//==============================================================================
#if !defined(AFX_TMOVIEPG_H__828750F4_0139_11D2_B1BD_008029EFD140__INCLUDED_)
#define AFX_TMOVIEPG_H__828750F4_0139_11D2_B1BD_008029EFD140__INCLUDED_

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
class CTMMovieProperties : public COlePropertyPage
{
	private:

						DECLARE_DYNCREATE(CTMMovieProperties)
						DECLARE_OLECREATE_EX(CTMMovieProperties)

	public:
	
						CTMMovieProperties();

	//{{AFX_DATA(CTMMovieProperties)
	enum { IDD = IDD_PROPPAGE_TMMOVIE };
	//}}AFX_DATA

	protected:
		virtual void	DoDataExchange(CDataExchange* pDX);  

	protected:
	//{{AFX_MSG(CTMMovieProperties)
		// NOTE - ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMOVIEPG_H__828750F4_0139_11D2_B1BD_008029EFD140__INCLUDED)
