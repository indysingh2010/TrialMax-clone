//==============================================================================
//
// File Name:	SplashWnd.h
//
// Description:	This file contains the declaration of the CSplashWnd class
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	02-07-2007	1.00		Original Release
//==============================================================================
#if !defined(__SPLASHWND_H__)
#define __SPLASHWND_H__

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
class CSplashWnd : public CWnd
{
	private:
		
		CBitmap				m_Bitmap;

	public:

							CSplashWnd();
	virtual				   ~CSplashWnd();

		void				Close();
		BOOL				Show(UINT uBitmapId, CWnd* pwndParent = NULL, UINT uTimeOut = 0);

	protected:

	public:

	//{{AFX_VIRTUAL(CSplashWnd)
	//}}AFX_VIRTUAL

	protected:

	//{{AFX_MSG(CSplashWnd)
	afx_msg void OnPaint();
	afx_msg void OnTimer(UINT nIDEvent);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // __SPLASHWND_H__
