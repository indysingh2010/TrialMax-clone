//==============================================================================
//
// File Name:	vidprops.h
//
// Description:	This file contains the declaration of the CVideoProperties class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	07-03-98	1.00		Original Release
//==============================================================================
#if !defined(AFX_VIDPROPS_H__9C00F721_1284_11D2_B1DB_008029EFD140__INCLUDED_)
#define AFX_VIDPROPS_H__9C00F721_1284_11D2_B1DB_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <resource.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CVideoProperties : public CDialog
{
	private:


	public:
	
				CVideoProperties(CWnd* pParent);   

	//{{AFX_DATA(CVideoProperties)
	enum { IDD = IDD_VIDEOPROPS };
	CString	m_strAspectRatio;
	CString	m_strFilename;
	CString	m_strFrames;
	CString	m_strHeight;
	CString	m_strRate;
	CString	m_strTime;
	CString	m_strWidth;
	//}}AFX_DATA


	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CVideoProperties)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    //
	//}}AFX_VIRTUAL

	// Implementation
	protected:

	// Generated message map functions
	//{{AFX_MSG(CVideoProperties)
		// NOTE: the ClassWizard will add member functions here
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_VIDPROPS_H__9C00F721_1284_11D2_B1DB_008029EFD140__INCLUDED_)
