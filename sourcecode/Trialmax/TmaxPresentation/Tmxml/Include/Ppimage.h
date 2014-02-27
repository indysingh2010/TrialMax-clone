//==============================================================================
//
// File Name:	ppimage.h
//
// Description:	This file contains the declaration of the CPPImage class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	03-20-01	1.00		Original Release
//==============================================================================
#if !defined(__PPIMAGE_H__)
#define __PPIMAGE_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <tmvdefs.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CPPImage : public CPropertyPage
{
	private:

							DECLARE_DYNCREATE(CPPImage)

	public:
							
							CPPImage();
						   ~CPPImage();

		void				SetImageProperties(STMVImageProperties* pProperties);

	protected:

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Dialog Data
	//{{AFX_DATA(CPPImage)
	enum { IDD = IDD_IMAGE_PAGE };
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
	// ClassWizard generate virtual function overrides
	//{{AFX_VIRTUAL(CPPImage)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:
	// Generated message map functions
	//{{AFX_MSG(CPPImage)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__PPIMAGE_H__)
