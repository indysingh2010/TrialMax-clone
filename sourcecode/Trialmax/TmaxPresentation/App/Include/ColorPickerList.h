//==============================================================================
//
// File Name:	ColorPickerList.h
//
// Description:	This file contains dialog of color picker list
//				
//
// See Also:	ColorPickerList.cpp
//
// Author:		Muhammad Hussain
//
// Copyright	Tenpearls LLC 2014
//
//==============================================================================
//	Date		Revision    Description
//	24-03-14	1.00		Original Release
//==============================================================================

#pragma once


// CColorPickerList dialog


#include "resource.h"
#include < map >
#include <vector>
#include <afxbutton.h>
#include <cstringt.h>

class CMainView;

class CColorPickerList : public CDialog
{
	DECLARE_DYNAMIC(CColorPickerList)

public:
	 // standard constructor
	CColorPickerList(CMainView* parentWindow, CWnd* pParent = NULL);   
	virtual ~CColorPickerList();

	typedef void (CColorPickerList::*fn)(int iButtonId);
	typedef std::map< UINT, fn > EventMessageMap;
	
	int					m_nXPosition;
	int					m_nYPosition;	

// Dialog Data
	enum { IDD = IDD_COLOR_PICKER_DLG };

	// the enum numbers are according to the image index defined in Tm_tool6
	enum ColorType { 		
		RED = 58,
		GREEN = 61,
		BLUE = 64,
		YELLOW = 66,
		BLACK = 67,
		WHITE = 68,		
		DARKRED = 57,
		DARKGREEN = 60,
		DARKBLUE = 63,
		LIGHTRED = 59,
		LIGHTGREEN = 62,
		LIGHTBLUE = 65
	};

public:
	virtual BOOL OnCmdMsg(UINT nID, int nCode, void* pExtra, AFX_CMDHANDLERINFO* pHandlerInfo);	
	void OnCancel();
	void HandleMouseClick();

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
			BOOL CColorPickerList::OnInitDialog();			
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);	

			void CColorPickerList::Recieved(int iButtonId);		
			void CColorPickerList::PostNcDestroy();

	DECLARE_MESSAGE_MAP()

private:
	CMainView*		m_parentWindow;
	CBrush			m_brush;
	EventMessageMap m_msgMap;	
	BOOL			AddButtons();
	void			CreateButton(CString sButtonName, DWORD pButtonStyle, int iButtonId, int iButtonWidth, int iButtonHeight, 
					int iXPosition, int iYPosition,int iButtonNumber, int ICON = 0, COLORREF faceColor = NULL);
public:
	
};
