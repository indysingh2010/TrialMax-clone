//==============================================================================
//
// File Name:	imgprop.h
//
// Description:	This file contains the declaration of the CImageProperties
//				class. This is a dialog box that allows the user to view the
//				properties of the specified image
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2001
//
//==============================================================================
//	Date		Revision    Description
//	03-10-01	1.00		Original Release
//==============================================================================
#if !defined(__IMGPROP_H__)
#define __IMGPROP_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <resource.h>
#include <tmvdefs.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CImageProperties : public CDialog
{
	private:

	public:
	
							CImageProperties(CWnd* pParent = NULL);   // standard constructor

		void				SetImageInfo(STMVImageProperties* pProperties);

	protected:

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Dialog Data
	//{{AFX_DATA(CImageProperties)
	enum { IDD = IDD_IMAGE_PROPERTIES };
	CString	m_strFilename;
	CString	m_strBitsPerPixel;
	CString	m_strDiskSize;
	CString	m_strRamSize;
	CString	m_strDimInches;
	CString	m_strDimPixels;
	CString	m_strPage;
	CString	m_strCompression;
	CString	m_strImageType;
	//}}AFX_DATA

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CImageProperties)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:

	// Generated message map functions
	//{{AFX_MSG(CImageProperties)
		// NOTE: the ClassWizard will add member functions here
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__IMGPROP_H__)
