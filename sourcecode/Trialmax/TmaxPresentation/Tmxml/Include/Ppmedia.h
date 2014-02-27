//==============================================================================
//
// File Name:	ppmedia.h
//
// Description:	This file contains the declaration of the CPPMedia class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	05-28-01	1.00		Original Release
//==============================================================================
#if !defined(__PPMEDIA_H__)
#define __PPMEDIA_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <mediactl.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CXmlMediaTrees;

class CPPMedia : public CPropertyPage
{
	private:

							DECLARE_DYNCREATE(CPPMedia)

	public:
							
		CXmlMediaTrees*		m_pXmlTrees;

							CPPMedia();
						   ~CPPMedia();

	protected:

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Dialog Data
	//{{AFX_DATA(CPPMedia)
	enum { IDD = IDD_MEDIA_PAGE };
	CMediaCtrl	m_ctrlTree;
	//}}AFX_DATA


	// Overrides
	// ClassWizard generate virtual function overrides
	//{{AFX_VIRTUAL(CPPMedia)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:
	// Generated message map functions
	//{{AFX_MSG(CPPMedia)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__PPMEDIA_H__)
