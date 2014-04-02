//==============================================================================
//
// File Name:	pprevs.h
//
// Description:	This file contains the declaration of the CPPRevisions class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	06-25-01	1.00		Original Release
//==============================================================================
#if !defined(__PPREVS_H__)
#define __PPREVS_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <tmprint.h>
#include <tmview.h>
#include <tmtool.h>
#include <tmpower.h>
#include <tmbrowse.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define DIAGNOSTIC_NAME_COLUMN			0
#define DIAGNOSTIC_DESCRIPTION_COLUMN	1
#define DIAGNOSTIC_VERSION_COLUMN		2
#define DIAGNOSTIC_BUILD_COLUMN			3
#define DIAGNOSTIC_CLSID_COLUMN			4
#define DIAGNOSTIC_PATH_COLUMN			5

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMXmlCtrl;

class CPPRevisions : public CPropertyPage
{
	private:

							DECLARE_DYNCREATE(CPPRevisions)

	public:
		
		CTMXmlCtrl*			m_pTMXmlCtrl;
							
							CPPRevisions();
						   ~CPPRevisions();

	protected:

		void				Add(LPCSTR lpName, LPCSTR lpDescription, int iMajor,
								int iMinor, long lBuild, LPCSTR lpClsid,
								LPCSTR lpPath);

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Dialog Data
	//{{AFX_DATA(CPPRevisions)
	enum { IDD = IDD_REVISIONS_PAGE };
	CListCtrl	m_ctrlList;
	CTMPower	m_TMPower;
	CTMPrint	m_TMPrint;
	CTMTool	m_TMTool;
	CTm_view	m_TMView;
	CTMBrowse	m_TMBrowse;
	//}}AFX_DATA


	// Overrides
	// ClassWizard generate virtual function overrides
	//{{AFX_VIRTUAL(CPPRevisions)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:
	// Generated message map functions
	//{{AFX_MSG(CPPRevisions)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__PPREVS_H__)
