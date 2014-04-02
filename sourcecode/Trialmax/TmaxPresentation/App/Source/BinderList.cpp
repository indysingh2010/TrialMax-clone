//==============================================================================
//
// File Name:	BinderList.cpp
//
// Description:	This file contains dialog of binder list
//				
//
// See Also:	BinderList.h
//
// Author:		Muhammad Hussain
//
// Copyright	Tenpearls LLC 2014
//
//==============================================================================
//	Date		Revision    Description
//	24-03-14	1.00		Original Release
//==============================================================================
// BinderList.cpp : implementation file
//

#include "stdafx.h"
#include "BinderList.h"
#include "afxdialogex.h"
#include <afxstr.h>
#include <cstringt.h>
#include <View.h>


// CBinderList dialog
#define BACK_BUTTON_ID 11111
#define CLOSE_BUTTON_ID 9999

IMPLEMENT_DYNAMIC(CBinderList, CDialog)

//==============================================================================
//
// 	Function Name:	CBinderList::CBinderList()
//
// 	Description:	This is the constructor for CBinderList objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CBinderList::CBinderList(CMainView* parentWindow, CWnd* pParent /*= NULL*/) : m_parentWindow(parentWindow),
	 CDialog(CBinderList::IDD, pParent)
{
	m_brush.CreateSolidBrush(RGB(245, 240, 217));
	m_xPosition = 10;
	m_yPosition = 10;			
	m_parentWindow = parentWindow;
	m_isShowBackButton = FALSE;	
}

//==============================================================================
//
// 	Function Name:	CBinderList::~CBinderList()
//
// 	Description:	This is the destructor for CBinderList objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CBinderList::~CBinderList()
{
}

void CBinderList::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CBinderList, CDialog)
END_MESSAGE_MAP()


// CBinderList message handlers

//==============================================================================
//
// 	Function Name:	CBinderList::OnInitDialog()
//
// 	Description:	This method is used to Initialize Color Picker List Dialog.
//
// 	Returns:		BOOL
//
//	Notes:			None
//
//==============================================================================
BOOL CBinderList::OnInitDialog()
{	
	CDialog::OnInitDialog();
	return CBinderList::AddButtons();	
}

//==============================================================================
//
// 	Function Name:	CBinderList::AddButtons()
//
// 	Description:	This method is used to Add Buttons in the Binder List Dialog.
//
// 	Returns:		BOOL
//
//	Notes:			None
//
//==============================================================================
BOOL CBinderList::AddButtons()
{
	LogMe("AddButtons Start....");
  try
  {
  int buttonXPos = 0;
  int buttonYPos = 0;  
  int buttonHeight = 25;
  int buttonWidth = 153;
  int counter = 0;

  // add the close button
	CreateButton(" ",WS_CHILD|WS_VISIBLE|BS_PUSHBUTTON,CLOSE_BUTTON_ID,buttonWidth,
	buttonHeight,buttonXPos,buttonYPos,counter, 0);
	 counter++;	


  // add the back button
  if(m_isShowBackButton == TRUE)
  {	  
	  CreateButton("...",WS_CHILD|WS_VISIBLE|BS_PUSHBUTTON,BACK_BUTTON_ID,buttonWidth,
			buttonHeight,buttonXPos,0,counter,0);

	  counter++;	  
  }

  // add the content buttons
  list<CBinderEntry>::iterator b;
  for(b = m_binderEntryList.begin(); b!=m_binderEntryList.end(); ++b)
  {

	CString buttonName;	
	buttonName = (*b).m_Name;
	m_lCurrentParentId = (*b).m_ParentId;

	buttonYPos = counter * buttonHeight;

	CreateButton(buttonName,WS_CHILD|WS_VISIBLE|BS_PUSHBUTTON,(*b).m_AutoId,buttonWidth,
			buttonHeight,buttonXPos,buttonYPos,counter,(*b).m_MediaType);

	counter++;
  }

	// Set the Dialog Position
	m_yPosition = m_yPosition - ((counter) * buttonHeight);
	int buttonListHeight = ((counter) * buttonHeight);// + buttonHeight;
	SetWindowPos(NULL,m_xPosition,m_yPosition,buttonWidth, buttonListHeight, SWP_NOACTIVATE | SWP_NOZORDER);

	LogMe("AddButtons END....");
	return TRUE;
	}
	catch(CException* e)
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CBinderList::CreateButton()
//
// 	Description:	This method is used for CreateButton.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBinderList::CreateButton(CString pButtonName, DWORD pButtonStyle, int pButtonId, int pButtonWidth, int pButtonHeight, 
				int pXPosition, int pYPosition,int pButtonNumber, int lMediaType)
{	

		CMFCButton* mfcButton;
		mfcButton = new CMFCButton();
		mfcButton->Create(_T(pButtonName), pButtonStyle,
		CRect(pXPosition,(pButtonNumber) * pButtonHeight,pButtonWidth,(pButtonNumber + 1) * pButtonHeight), this, pButtonId);  
		mfcButton->SetMouseCursorHand();
		mfcButton->m_nFlatStyle = CMFCButton::BUTTONSTYLE_SEMIFLAT;

		if(pButtonId == CLOSE_BUTTON_ID)
		{				
			//mfcButton->SetImage(IDB_BITMAP3);	
			//mfcButton->SetFaceColor(RGB(64,64,64));
			mfcButton->SetIcon((HICON)LoadImage(AfxGetApp()->m_hInstance,
								MAKEINTRESOURCE(IDI_CLOSE_BLACK),
								IMAGE_ICON, 24, 24, LR_DEFAULTCOLOR));
			mfcButton->SetTooltip("Close");		
			mfcButton->m_bRightImage = TRUE;
			
		}
		else if (pButtonId == BACK_BUTTON_ID)
		{			
			mfcButton->SetIcon((HICON)LoadImage(AfxGetApp()->m_hInstance,
								MAKEINTRESOURCE(IDI_FOLDER_BACK),
								IMAGE_ICON, 24, 24, LR_DEFAULTCOLOR));
			mfcButton->SetTooltip("Back");
			mfcButton->m_bRightImage = TRUE;
		}
		else
		{
			//mfcButton->SetFaceColor(RGB(64,64,64),true);
			//mfcButton->SetTextColor(RGB(244,244,244));
			if(lMediaType > 4)
			{				
				mfcButton->SetIcon((HICON)LoadImage(AfxGetApp()->m_hInstance,
								MAKEINTRESOURCE(IDI_FILE),
								IMAGE_ICON, 24, 24, LR_DEFAULTCOLOR));
			}
			else
			{				
				mfcButton->SetIcon((HICON)LoadImage(AfxGetApp()->m_hInstance,
								MAKEINTRESOURCE(IDI_FOLDER),
								IMAGE_ICON, 24, 24, LR_DEFAULTCOLOR));
			}

			
			mfcButton->SetTooltip(pButtonName);
			mfcButton->m_nAlignStyle = CMFCButton::AlignStyle::ALIGN_LEFT;
			mfcButton->m_bRightImage = TRUE;
		}

		// generating dynamic event for button
		m_msgMap[pButtonId] = &CBinderList::Recieved;
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
void CBinderList::Recieved(int iButtonId)
{
	LogMe("Recieved Start....");
	if(iButtonId == CLOSE_BUTTON_ID)
	{
		m_msgMap.clear();
		CleanButtons();
		CDialog::OnCancel();		
		m_parentWindow->OnBinderDialogCloseButtonClickEvent();		
		//this->DestroyWindow();		
	}
	else if(iButtonId == BACK_BUTTON_ID)
	{
		m_msgMap.clear();
		CleanButtons();
		CDialog::OnCancel();		
		m_parentWindow->OnBinderDialogBackButtonClickEvent(m_parentBinder);		
		//this->DestroyWindow();
	}
	else
	{
		list<CBinderEntry>::iterator b;
		CBinderEntry binderEntry;
		for(b = m_binderEntryList.begin(); b!=m_binderEntryList.end(); ++b)
		{
			if((*b).m_AutoId == iButtonId)
			{
				binderEntry = (CBinderEntry)(*b);				
				break;
			}
		}

		m_msgMap.clear();				
		CleanButtons();
		CDialog::OnCancel();
		m_parentWindow->OnBinderDialogButtonClickEvent(binderEntry);				
		//this->DestroyWindow();
	}

	LogMe("Recieved End....");
	
}

//==============================================================================
//
// 	Function Name:	CBinderList::CleanButtons()
//
// 	Description:	This method is used to clean button resources.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBinderList::CleanButtons()
{
	/*list<CBinderEntry>::iterator b;
	for(b = m_binderEntryList.begin(); b!=m_binderEntryList.end(); ++b)
	{
		int buttonId = (*b).m_AutoId;
		CMFCButton* cmfcButton = (CMFCButton*)this->GetDlgItem(buttonId);
		if(cmfcButton != NULL && cmfcButton != 0)
		{				
			cmfcButton->CleanUp();
			cmfcButton->DestroyWindow();			
		}
	}

	CMFCButton* cmfcButtonBack = (CMFCButton*)this->GetDlgItem(BACK_BUTTON_ID);
	if(cmfcButtonBack != NULL && cmfcButtonBack != 0)
	{
		cmfcButtonBack->CleanUp();
		cmfcButtonBack->DestroyWindow();
	}

	CMFCButton* cmfcButtonClose = (CMFCButton*)this->GetDlgItem(CLOSE_BUTTON_ID);
	if(cmfcButtonClose != NULL && cmfcButtonClose != 0)
	{		
		cmfcButtonClose->CleanUp();			
		cmfcButtonClose->DestroyWindow();
	}*/
}

//==============================================================================
//
// 	Function Name:	CBinderList::OnCtlColor()
//
// 	Description:	This method is used to change the color of dialog.
//
// 	Returns:		HBRUSH
//
//	Notes:			None
//
//==============================================================================
HBRUSH CBinderList::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	HBRUSH hbr = m_brush;//CDialog::OnCtlColor(pDC, pWnd, nCtlColor);
	// TODO:  Change any attributes of the DC here

	// TODO:  Return a different brush if the default is not desired
	return hbr;
}

//==============================================================================
//
// 	Function Name:	CBinderList::OnCmdMsg()
//
// 	Description:	This method is used to dynamically trigger click events of button.
//
// 	Returns:		BOOL
//
//	Notes:			None
//
//==============================================================================
BOOL CBinderList::OnCmdMsg(UINT nID, int nCode, void* pExtra, AFX_CMDHANDLERINFO* pHandlerInfo) 
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
// 	Function Name:	CBinderList::PostNcDestroy()
//
// 	Description:	This method is used to destroy dialog.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBinderList::PostNcDestroy() 
{	
	LogMe("OnPostNCDestroy Start....");
	CDialog::PostNcDestroy();
	m_parentWindow = NULL;		
    delete this;    	
	LogMe("OnPostNCDestroy End....");
}

//==============================================================================
//
// 	Function Name:	CBinderList::PostNcDestroy()
//
// 	Description:	This method is used to handle cancel event of dialog.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBinderList::OnCancel()
{
	LogMe("OnCancel Start....");
	CDialog::OnCancel();
	LogMe("OnCancel End....");
}

//==============================================================================
//
// 	Function Name:	CMainView::LogMe()
//
// 	Description:	Logging method for debugging
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBinderList::LogMe(LPCTSTR msg)
{
	CString strFileName = "mylog.txt";
	FILE*	pFile = NULL;
	CString	strTime = CTime::GetCurrentTime().Format("%m-%d-%Y %H:%M:%S");

	if(fopen_s(&pFile, strFileName, "at") == 0)
	{		
		fprintf(pFile, "%s %s\n", strTime, msg);
		fflush(pFile);
		fclose(pFile);
	}
}