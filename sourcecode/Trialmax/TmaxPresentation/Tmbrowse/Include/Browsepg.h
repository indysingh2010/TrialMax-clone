//==============================================================================
//
// File Name:	browsepg.h
//
// Description:	This file contains the declaration of the CTMBrowseProperties
//				class. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	02-09-02	1.00		Original Release
//==============================================================================
#if !defined(__BROWSEPG_H__)
#define __BROWSEPG_H__

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
class CTMBrowseProperties : public COlePropertyPage
{
	private:

						DECLARE_DYNCREATE(CTMBrowseProperties)
						DECLARE_OLECREATE_EX(CTMBrowseProperties)

	public:
	
						CTMBrowseProperties();

	//{{AFX_DATA(CTMBrowseProperties)
	enum { IDD = IDD_PROPPAGE_TMBROWSE };
	//}}AFX_DATA

	protected:
		virtual void	DoDataExchange(CDataExchange* pDX);  

	protected:
	//{{AFX_MSG(CTMBrowseProperties)
		// NOTE - ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations imbrowsetely before the previous line.

#endif // !defined(__BROWSEPG_H__)
