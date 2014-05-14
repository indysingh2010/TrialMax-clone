//==============================================================================
//
// File Name:	ColorPickerList.cpp
//
// Description:	This file contains dialog of color picker list
//				
//
// See Also:	ColorPickerList.h
//
// Author:		Muhammad Hussain
//
// Copyright	Tenpearls LLC 2014
//
//==============================================================================
//	Date		Revision    Description
//	24-03-14	1.00		Original Release
//==============================================================================

// ColorPickerList.cpp : implementation file
//

#include "stdafx.h"
#include "ColorPickerList.h"
#include "afxdialogex.h"
#include <afxstr.h>
#include <View.h>

#define BUTTON_WIDTH 48

// CColorPickerList dialog
IMPLEMENT_DYNAMIC(CColorPickerList, CDialog)

CColorPickerList::CColorPickerList(CMainView* parentWindow, CWnd* pParent /*=NULL*/)  : m_parentWindow(parentWindow),
	CDialog(CColorPickerList::IDD, pParent)
{
	m_brush.CreateSolidBrush(RGB(245, 240, 217));
	m_nXPosition = 10;
	m_nYPosition = 10;			
	m_parentWindow = parentWindow;
}

CColorPickerList::~CColorPickerList()
{
}

void CColorPickerList::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CColorPickerList, CDialog)	
END_MESSAGE_MAP()


// CColorPickerList message handlers

//==============================================================================
//
// 	Function Name:	CColorPickerList::OnInitDialog()
//
// 	Description:	This method is used to Initialize Color Picker List Dialog.
//
// 	Returns:		BOOL
//
//	Notes:			None
//
//==============================================================================
BOOL CColorPickerList::OnInitDialog()
{	
	CDialog::OnInitDialog();
	return CColorPickerList::AddButtons();	
}

//==============================================================================
//
// 	Function Name:	CColorPickerList::AddButtons()
//
// 	Description:	This method is used to Add Buttons in the Color Picker List Dialog.
//
// 	Returns:		BOOL
//
//	Notes:			None
//
//==============================================================================
BOOL CColorPickerList::AddButtons()
{
	
  try
  {
	int buttonXPos = 0;
	int buttonYPos = 0;  
	int buttonHeight = 25;
	int buttonWidth = BUTTON_WIDTH;
	int counter = 0;
	CString buttonName;

	 buttonName = "Black";
	 CreateButton(buttonName,WS_CHILD|WS_VISIBLE|BS_PUSHBUTTON,ColorType::BLACK,buttonWidth,
			buttonHeight,buttonXPos,buttonYPos,counter, 0,RGB(0,0,0));
	 counter++;	

	 buttonName = "Red";
	 CreateButton(buttonName,WS_CHILD|WS_VISIBLE|BS_PUSHBUTTON,ColorType::RED,buttonWidth,
			buttonHeight,buttonXPos,buttonYPos,counter, 0,RGB(255,16,16));
	 counter++;	
  
	 buttonName = "Green";
	 CreateButton(buttonName,WS_CHILD|WS_VISIBLE|BS_PUSHBUTTON,ColorType::GREEN,buttonWidth,
			buttonHeight,buttonXPos,buttonYPos,counter, 0,RGB(0,255,0));
	 counter++;	

	 buttonName = "Blue";
	 CreateButton(buttonName,WS_CHILD|WS_VISIBLE|BS_PUSHBUTTON,ColorType::BLUE,buttonWidth,
			buttonHeight,buttonXPos,buttonYPos,counter, 0,RGB(0,0,255));
	 counter++;	

	 buttonName = "Yellow";
	 CreateButton(buttonName,WS_CHILD|WS_VISIBLE|BS_PUSHBUTTON,ColorType::YELLOW,buttonWidth,
			buttonHeight,buttonXPos,buttonYPos,counter, 0,RGB(255,255,0));
	 counter++;	
	 
	/* buttonName = "White";
	 CreateButton(buttonName,WS_CHILD|WS_VISIBLE|BS_PUSHBUTTON,ColorType::WHITE,buttonWidth,
			buttonHeight,buttonXPos,buttonYPos,counter, 0,RGB(255,255,255));
	 counter++;	*/

	 buttonName = "Dark Red";
	 CreateButton(buttonName,WS_CHILD|WS_VISIBLE|BS_PUSHBUTTON,ColorType::DARKRED,buttonWidth,
			buttonHeight,buttonXPos,buttonYPos,counter, 0,RGB(132,0,0));
	 counter++;	

	 buttonName = "Dark Green";
	 CreateButton(buttonName,WS_CHILD|WS_VISIBLE|BS_PUSHBUTTON,ColorType::DARKGREEN,buttonWidth,
		 buttonHeight,buttonXPos,buttonYPos,counter, 0,RGB(0,132,0));
	 counter++;	

	 buttonName = "Dark Blue";
	 CreateButton(buttonName,WS_CHILD|WS_VISIBLE|BS_PUSHBUTTON,ColorType::DARKBLUE,buttonWidth,
			buttonHeight,buttonXPos,buttonYPos,counter, 0,RGB(0,0,132));
	 counter++;	

	 buttonName = "Light Red";
	 CreateButton(buttonName,WS_CHILD|WS_VISIBLE|BS_PUSHBUTTON,ColorType::LIGHTRED,buttonWidth,
			buttonHeight,buttonXPos,buttonYPos,counter, 0,RGB(255,132,132));
	 counter++;	

	 buttonName = "Light Green";
	 CreateButton(buttonName,WS_CHILD|WS_VISIBLE|BS_PUSHBUTTON,ColorType::LIGHTGREEN,buttonWidth,
			buttonHeight,buttonXPos,buttonYPos,counter, 0,RGB(132,255,132));
	 counter++;	

	 buttonName = "Light Blue";
	 CreateButton(buttonName,WS_CHILD|WS_VISIBLE|BS_PUSHBUTTON,ColorType::LIGHTBLUE,buttonWidth,
			buttonHeight,buttonXPos,buttonYPos,counter,0,RGB(132,132,255));
	 counter++;	

		// Set the Dialog Position
		m_nYPosition = m_nYPosition - ((counter) * buttonHeight);
		int buttonListHeight = ((counter) * buttonHeight);// + buttonHeight;
		SetWindowPos(NULL,m_nXPosition,m_nYPosition,buttonWidth, buttonListHeight, SWP_NOACTIVATE | SWP_NOZORDER);
		
		return TRUE;
	}
	catch(CException* e)
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CColorPickerList::CreateButton()
//
// 	Description:	This method is used for CreateButton.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CColorPickerList::CreateButton(CString pButtonName, DWORD pButtonStyle, int pButtonId, int pButtonWidth, int pButtonHeight, 
				int pXPosition, int pYPosition,int pButtonNumber, int ICON, COLORREF faceColor)
{	

		CMFCButton* mfcButton;
		mfcButton = new CMFCButton();
		mfcButton->Create(_T(""), pButtonStyle,
		CRect(pXPosition,(pButtonNumber) * pButtonHeight,pButtonWidth,(pButtonNumber + 1) * pButtonHeight), this, pButtonId);  
		mfcButton->SetMouseCursorHand();
		mfcButton->m_nFlatStyle = CMFCButton::BUTTONSTYLE_SEMIFLAT;

		mfcButton->SetFaceColor(faceColor,true);
		//mfcButton->SetImage(ICON);	
		mfcButton->SetTooltip(pButtonName);
		mfcButton->m_nAlignStyle = CMFCButton::AlignStyle::ALIGN_LEFT;
		mfcButton->m_bRightImage = TRUE;		

		// generating dynamic event for button
		m_msgMap[pButtonId] = &CColorPickerList::Recieved;
}

//==============================================================================
//
// 	Function Name:	CBinderList::Recieved()
//
// 	Description:	This method is used to handle button click events.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CColorPickerList::Recieved(int iButtonId)
{	
	m_msgMap.clear();						
	CDialog::OnCancel();
	m_parentWindow->OnColorPickerButtonClickEvent(iButtonId);
}

HBRUSH CColorPickerList::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	HBRUSH hbr = m_brush;//CDialog::OnCtlColor(pDC, pWnd, nCtlColor);
	// TODO:  Change any attributes of the DC here

	// TODO:  Return a different brush if the default is not desired
	return hbr;
}

//==============================================================================
//
// 	Function Name:	CColorPickerList::OnCmdMsg()
//
// 	Description:	This method is used to dynamically trigger click events of button.
//
// 	Returns:		BOOL
//
//	Notes:			None
//
//==============================================================================
BOOL CColorPickerList::OnCmdMsg(UINT nID, int nCode, void* pExtra, AFX_CMDHANDLERINFO* pHandlerInfo) 
{
	EventMessageMap::iterator itTrg = m_msgMap.find( nID );

	if(itTrg != m_msgMap.end())
	{				
		fn btnM = m_msgMap[nID];
		(this->*btnM)(nID);
	}
	
	return CDialog::OnCmdMsg(nID, nCode, pExtra, pHandlerInfo);
}

//==============================================================================
//
// 	Function Name:	CColorPickerList::PostNcDestroy()
//
// 	Description:	This method is used to destroy dialog.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CColorPickerList::PostNcDestroy() 
{		
	CDialog::PostNcDestroy();
	m_parentWindow = NULL;		
    delete this;    	
}

//==============================================================================
//
// 	Function Name:	CColorPickerList::OnCancel()
//
// 	Description:	This method is used to handle cancel event of dialog.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CColorPickerList::OnCancel()
{	
	CDialog::OnCancel();	
}

//==============================================================================
//
// 	Function Name:	CColorPickerList::HandleMouseClick()
//
// 	Description:	This method is used to handle mouse click from main window using hook
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CColorPickerList::HandleMouseClick()
{
	CPoint cursorPosition;
	GetCursorPos(&cursorPosition);

	// if click on the / inside the dialog than dont do any thing
	if((cursorPosition.x >= m_nXPosition && cursorPosition.x < (m_nXPosition + BUTTON_WIDTH ))
			&&
			(cursorPosition.y >= m_nYPosition))
	{ }
	else	// else close the dialog
	{
		CDialog::OnCancel();
		m_parentWindow->OnColorPickerCloseButtonClickEvent();
	}
}