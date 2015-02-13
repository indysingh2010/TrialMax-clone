//==============================================================================
//
// File Name:	frame.h
//
// Description:	This file contains the declaration of the CMainFrame class.
//				This is the application's main window.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	08-17-97	1.00		Original Release
//==============================================================================
#if !defined(AFX_FRAME_H__AA00514D_16FD_11D1_B02E_008029EFD140__INCLUDED_)
#define AFX_FRAME_H__AA00514D_16FD_11D1_B02E_008029EFD140__INCLUDED_

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
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CMainFrame : public CFrameWnd
{
	private:

	public:

						DECLARE_DYNCREATE(CMainFrame)

						CMainFrame();
					   ~CMainFrame();

		void			LoadFromBarcode(LPCSTR lpBarcode, BOOL bAddBuffer, BOOL bAlternate);
		void			LoadPageFromKeyboard(long lPage);
		void			SetLoadPage(int iPage);
		void			SetLoadLine(int iLine);
		BOOL			ProcessCommandKey(char cKey);
		BOOL			ProcessVirtualKey(WORD wKey);
		BOOL			ProcessMouseMessage(MSG* pMsg);
		BOOL			GetUseSecondaryMonitor();
		char			GetVKChar();
		char			GetPrimaryBarcodeChar();
		char			GetAlternateBarcodeChar();
		void			UpdateBarcode(CString Barcode);
		LONG			OnWMNewInstance(WPARAM wParam, LPARAM lParam);

	protected:

	//	Class Wizard maintained
	public:

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMainFrame)
	public:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	virtual void RecalcLayout(BOOL bNotify = TRUE);
	//}}AFX_VIRTUAL

	protected:
	//{{AFX_MSG(CMainFrame)
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnClose();
	afx_msg void OnMove(int x, int y);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_FRAME_H__AA00514D_16FD_11D1_B02E_008029EFD140__INCLUDED_)
