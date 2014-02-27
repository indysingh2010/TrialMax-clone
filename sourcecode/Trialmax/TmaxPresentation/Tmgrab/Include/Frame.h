//==============================================================================
//
// File Name:	grabctl.h
//
// Description:	This file contains the declaration of the CFrame class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	05-24-01	1.00		Original Release
//==============================================================================
#if !defined(__FRAME_H__)
#define __FRAME_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <lead.h>
#include <LCapture.h>
#include "afxwin.h"

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CErrorHandler;
class CTMGrabCtrl;

class CFrame : public CDialog
{
	private:

		LCaptureBitmap	m_bmpCapture;
		LBitmapWindow	m_wndBitmap;
		HWND			m_hwndBitmap;
		CErrorHandler*	m_pErrors;
		CTMGrabCtrl*	m_pControl;
		short			m_sHotkey;
		short			m_sCancelKey;
		short			m_sArea;
		BOOL			m_bSilent;
		BOOL			m_bOneShot;
		UINT			m_uLTLibsLoaded;

	public:
	
						CFrame(CWnd* pParent = NULL);
					   ~CFrame();
		
		short			GetArea(){ return m_sArea; }
		short			GetHotkey(){ return m_sHotkey; }
		short			GetCancelKey(){ return m_sCancelKey; }
		BOOL			GetSilent(){ return m_bSilent; }
		BOOL			GetOneShot(){ return m_bOneShot; }

		void			SetArea(short sArea){ m_sArea = sArea; }
		void			SetHotkey(short sHotkey){ m_sHotkey = sHotkey; }
		void			SetCancelKey(short sCancelKey){ m_sCancelKey = sCancelKey; }
		void			SetSilent(BOOL bSilent){ m_bSilent = bSilent; }
		void			SetOneShot(BOOL bOneShot){ m_bOneShot = bOneShot; }

		BOOL			Create(CTMGrabCtrl* pControl, CErrorHandler* pErrors);
		BOOL			Capture();
		BOOL			Stop();
		BOOL			Save(LPCTSTR pszName, short iFormat, short iBitsPerPixel, 
							 short iQuality, short iModify);

		void			OnOK(){};
		void			OnCancel();
		void			OnCaptureImage(LBitmapBase* pLBitmap, long CaptureNumber);

	protected:

		BOOL			Initialize(LCaptureCtrl* pCaptureCtrl);
		BOOL			CaptureScreen();
		BOOL			CaptureActive();
		BOOL			CaptureSelection();

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Dialog Data
	//{{AFX_DATA(CFrame)
	enum { IDD = IDD_FRAME };
	CButton	m_ctrlStop;
	CButton	m_ctrlCancel;
	//}}AFX_DATA

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CFrame)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	// Implementation
	protected:

	// Generated message map functions
	//{{AFX_MSG(CFrame)
	virtual BOOL OnInitDialog();
	afx_msg void OnDestroy();
	afx_msg void OnClipboard();
	afx_msg void OnSaveAs();
	afx_msg void OnStop();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
public:
	CStatic m_wndBitmapPanel;
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__FRAME_H__)
