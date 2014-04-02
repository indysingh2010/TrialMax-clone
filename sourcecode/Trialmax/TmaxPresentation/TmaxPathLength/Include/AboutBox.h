//==============================================================================
//
// File Name:	AboutBox.h
//
// Description:	This file contains the declaration of the CAboutBox class
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	03-30-2007	1.00		Original Release
//==============================================================================
#if !defined(__ABOUTBOX_H__)
#define __ABOUTBOX_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <Resource.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CAboutBox : public CDialog
{
	private:

	public:
	
						CAboutBox();

	protected:

	public:

	// Dialog Data
	//{{AFX_DATA(CAboutBox)
	enum { IDD = IDD_ABOUTBOX };
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAboutBox)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:
	//{{AFX_MSG(CAboutBox)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

#endif // !defined(__ABOUTBOX_H__)
