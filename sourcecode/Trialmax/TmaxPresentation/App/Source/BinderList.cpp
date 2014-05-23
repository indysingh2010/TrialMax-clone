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

HHOOK g_hBinderHook;

// CBinderList dialog
#define BACK_BUTTON_ID 11111
#define CLOSE_BUTTON_ID 9999
#define BUTTON_WIDTH 153
#define BUTTON_HEIGHT 50

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
CBinderList::CBinderList(CMainView* parentWindow, int nTotalRecords,CWnd* pParent /*= NULL*/) : m_parentWindow(parentWindow),
	 CDialog(CBinderList::IDD, pParent)
{
	m_brush.CreateSolidBrush(RGB(245, 240, 217));
	m_nXPosition = 0;
	m_nYPosition = 0;			
	m_parentWindow = parentWindow;
	m_bIsShowBackButton = FALSE;	
	
	// TotalRecords = Total Items/Total Buttons which will be created
	// so the sum of all buttons height is equal to the height of list
	// +1 with Total Record means we have given some space between screen and list
	m_nListHeight = (nTotalRecords + 1) * BUTTON_HEIGHT;			
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
	ON_WM_SIZE()
	ON_WM_VSCROLL()		
	ON_WM_MOUSEWHEEL()
	ON_MESSAGE(WM_GESTURE, OnGesture)
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
  try
  {
  int buttonXPos = 0;
  int buttonYPos = 0;  
  int buttonHeight = BUTTON_HEIGHT;
  int buttonWidth = BUTTON_WIDTH;
  int counter = 0;

  // add the close button
	/*CreateButton(" ",WS_CHILD|WS_VISIBLE|BS_PUSHBUTTON,CLOSE_BUTTON_ID,buttonWidth,
	buttonHeight,buttonXPos,buttonYPos,counter, 0);
	 counter++;	*/


  // add the back button
  if(m_bIsShowBackButton == TRUE)
  {	  
	  
	  CMFCButton* backButton = CreateButton("...",WS_CHILD|WS_VISIBLE|BS_PUSHBUTTON,BACK_BUTTON_ID,buttonWidth,
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

	 CMFCButton* binderButton = CreateButton(buttonName,WS_CHILD|WS_VISIBLE|BS_PUSHBUTTON,(*b).m_AutoId,buttonWidth,
			buttonHeight,buttonXPos,buttonYPos,counter,(*b).m_MediaType);

	counter++;
  }	

	// calculate the list height
	int buttonListHeight = ((counter) * buttonHeight);// + buttonHeight;

	// Set the list height for scroll
	m_nListHeight = buttonListHeight;
	m_nScrollPos = 0;
	
	// get the screen height
	int screenHeight = GetSystemMetrics(SM_CYSCREEN);

	// Set the Dialog Position
	// if the number of items in the list exceed that increase the height of the list
	// so in that case we applied scroll and restrict the list height
	if(m_nListHeight > (screenHeight-(BUTTON_HEIGHT * 2)))
	{
		// calculate the Y Position for list
		m_nYPosition = m_nYPosition - screenHeight + (BUTTON_HEIGHT * 2);

		// show scroll bar				// additional width for scroll,
		SetWindowPos(NULL,m_nXPosition,m_nYPosition, buttonWidth + 20, screenHeight-(BUTTON_HEIGHT * 2), SWP_NOACTIVATE | SWP_NOZORDER);		
	}
	else
	{		
		// calculate the Y Position for list
		m_nYPosition = m_nYPosition - buttonListHeight;

		// else dont show scroll bar
		SetWindowPos(NULL,m_nXPosition,m_nYPosition,buttonWidth, buttonListHeight, SWP_NOACTIVATE | SWP_NOZORDER);
	}
	
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
CMFCButton* CBinderList::CreateButton(CString sButtonName, DWORD pButtonStyle, int nButtonId, int nButtonWidth, int nButtonHeight, 
				int nXPosition, int nYPosition,int nButtonNumber, int nMediaType)
{	

		CMFCButton* mfcButton;
		mfcButton = new CMFCButton();
		mfcButton->Create(_T(sButtonName), pButtonStyle,
		CRect(nXPosition,(nButtonNumber) * nButtonHeight,nButtonWidth,(nButtonNumber + 1) * nButtonHeight), this, nButtonId);  
		mfcButton->SetMouseCursorHand();
		mfcButton->m_nFlatStyle = CMFCButton::BUTTONSTYLE_SEMIFLAT;

		if(nButtonId == CLOSE_BUTTON_ID)
		{				
			//mfcButton->SetImage(IDB_BITMAP3);	
			//mfcButton->SetFaceColor(RGB(64,64,64));
			mfcButton->SetIcon((HICON)LoadImage(AfxGetApp()->m_hInstance,
								MAKEINTRESOURCE(IDI_CLOSE_BLACK),
								IMAGE_ICON, 24, 24, LR_DEFAULTCOLOR));
			mfcButton->SetTooltip("Close");		
			//mfcButton->m_bRightImage = TRUE;
			
		}
		else if (nButtonId == BACK_BUTTON_ID)
		{			
			mfcButton->SetIcon((HICON)LoadImage(AfxGetApp()->m_hInstance,
								MAKEINTRESOURCE(IDI_FOLDER_BACK),
								IMAGE_ICON, 24, 24, LR_DEFAULTCOLOR));
			mfcButton->SetTooltip("Back");
			//mfcButton->m_bRightImage = TRUE;
		}
		else
		{
			//mfcButton->SetFaceColor(RGB(64,64,64),true);
			//mfcButton->SetTextColor(RGB(244,244,244));
			if(nMediaType > 4)
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

			
			mfcButton->SetTooltip(sButtonName);
			mfcButton->m_nAlignStyle = CMFCButton::AlignStyle::ALIGN_LEFT;
			//mfcButton->m_bRightImage = TRUE;
		}

		// generating dynamic event for button
		m_msgMap[nButtonId] = &CBinderList::Recieved;
		return mfcButton;
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
void CBinderList::Recieved(int nButtonId)
{	
	if(nButtonId == CLOSE_BUTTON_ID)
	{
		m_msgMap.clear();
		CleanButtons();
		CDialog::OnCancel();		
		m_parentWindow->OnBinderDialogCloseButtonClickEvent();		
		//this->DestroyWindow();		
	}
	else if(nButtonId == BACK_BUTTON_ID)
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
			if((*b).m_AutoId == nButtonId)
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
	//list<CMFCButton*>::iterator b;
	//for(b = m_CMFCButtons.begin(); b!=m_CMFCButtons.end(); ++b)
	//{
	//	//int buttonId = (*b).m_AutoId;
	//	//CMFCButton* cmfcButton = (CMFCButton*)this->GetDlgItem(buttonId);
	//	CMFCButton* cmfcButton = (*b);
	//	if(cmfcButton != NULL && cmfcButton != 0)
	//	{				
	//		cmfcButton->CleanUp();
	//		cmfcButton->DestroyWindow();			
	//	}
	//}


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
	CDialog::PostNcDestroy();
	m_parentWindow = NULL;		
    delete this;
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
	CDialog::OnCancel();	
}

//==============================================================================
//
// 	Function Name:	CBinderList::LogMe()
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

//==============================================================================
//
// 	Function Name:	CBinderList::OnSize()
//
// 	Description:	This message handler is used to handle when the size is change
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBinderList::OnSize(UINT nType, int cx, int cy)
{	
	CDialog::OnSize(nType, cx, cy);

	int screenHeight = GetSystemMetrics(SM_CYSCREEN);
	
	if(m_nListHeight < screenHeight)
	return;

	// TODO: Add your message handler code here.
	m_nCurHeight = screenHeight - (BUTTON_HEIGHT * 2);
	int nScrollMax;
	if (m_nCurHeight < m_nListHeight)
	{								
	     nScrollMax = m_nListHeight - m_nCurHeight;
	}
	else
	     nScrollMax = 0;

	SCROLLINFO si;
	si.cbSize = sizeof(SCROLLINFO);
	si.fMask = SIF_ALL; // SIF_ALL = SIF_PAGE | SIF_RANGE | SIF_POS;
	si.nMin = 0;
	si.nMax = nScrollMax;
	si.nPage = si.nMax/m_nListHeight;
	si.nPos = 0;
    SetScrollInfo(SB_VERT, &si, TRUE); 	

}

//==============================================================================
//
// 	Function Name:	CBinderList::OnVScroll()
//
// 	Description:	This message handler is used to handle when the scrolling is performed
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBinderList::OnVScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar)
{	 
	// TODO: Add your message handler code here and/or call default.
	int nDelta;
	int nMaxPos = m_nListHeight - m_nCurHeight;

	switch (nSBCode)
	{
	case SB_LINEDOWN:
		if (m_nScrollPos >= nMaxPos)
			return;
		nDelta = min(nMaxPos/100,nMaxPos-m_nScrollPos);
		break;

	case SB_LINEUP:
		if (m_nScrollPos <= 0)
			return;
		nDelta = -min(nMaxPos/100,m_nScrollPos);
		break;

         case SB_PAGEDOWN:
		if (m_nScrollPos >= nMaxPos)
			return;
		nDelta = min(nMaxPos/10,nMaxPos-m_nScrollPos);
		break;

	case SB_THUMBPOSITION:
		nDelta = (int)nPos - m_nScrollPos;
		break;

	case SB_PAGEUP:
		if (m_nScrollPos <= 0)
			return;
		nDelta = -min(nMaxPos/10,m_nScrollPos);
		break;
	
	case SB_THUMBTRACK:
		if (m_nScrollPos < 0 || m_nScrollPos > nMaxPos)
			return;
		
		nDelta = (int)nPos - m_nScrollPos;		
		break;
        
	default:
		return;
	}

	m_nScrollPos += nDelta;	
	SetScrollPos(SB_VERT,m_nScrollPos,TRUE);	
	ScrollWindow(0,-nDelta);
	CDialog::OnVScroll(nSBCode, nPos, pScrollBar);
}

//==============================================================================
//
// 	Function Name:	CBinderList::HandleMouseClick()
//
// 	Description:	This method is used to handle mouse click from main window using hook
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CBinderList::HandleMouseClick()
{
	CPoint cursorPosition;
	GetCursorPos(&cursorPosition);

	// if click on the / inside the dialog than dont do any thing
	if((cursorPosition.x >= m_nXPosition && cursorPosition.x < (m_nXPosition + BUTTON_WIDTH + 20))
			&&
			(cursorPosition.y >= m_nYPosition))
		{ }
		else // else close the dialog
		{
			CDialog::OnCancel();
			m_parentWindow->OnBinderDialogCloseButtonClickEvent();				
		}	
}

//==============================================================================
//
// 	Function Name:	CBinderList::OnMouseWheel()
//
// 	Description:	This method is used to handle mouse wheel event on the dialog
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CBinderList::OnMouseWheel(UINT nFlags, short zDelta, CPoint pt)
{
	// TODO: Add your message handler code here and/or call default
	int screenHeight = GetSystemMetrics(SM_CYSCREEN);
	
	// if no scroll than don't bother
	if(m_nListHeight < screenHeight)
	return FALSE;
	
	// if wheeling in upward direction
	if(zDelta > 0)
	{
		if(m_nScrollPos <= 0)
			return FALSE;

		if((m_nScrollPos - zDelta) < 0)
			zDelta = m_nScrollPos;
	}
	else // if wheeling in donward direction
	{
		if(m_nScrollPos >= (m_nListHeight - (m_nCurHeight + BUTTON_HEIGHT)))
		{
			if(m_nScrollPos >= (m_nListHeight - m_nCurHeight))
			{
				return FALSE;
			}

			int differenceToScroll = m_nListHeight - m_nCurHeight;
			differenceToScroll = differenceToScroll - m_nScrollPos;
			zDelta = differenceToScroll - BUTTON_HEIGHT;
		}
	}
	
	m_nScrollPos -= zDelta;
	SetScrollPos(SB_VERT,m_nScrollPos,TRUE);	
	ScrollWindow(0, zDelta);
	
	return CDialog::OnMouseWheel(nFlags, zDelta, pt);
}

//==============================================================================
//
// 	Function Name:	CBinderList::OnGesture()
//
// 	Description:	WM_GESTURE message handler. Scroll up/down binder list 
//					on gesture pan
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================

void CBinderList::HandlePan(int diff)
{
	int screenHeight = GetSystemMetrics(SM_CYSCREEN);
	if(m_nListHeight < screenHeight)
		return;

	if(abs(diff) > 30) {
		if(diff < 0) {
			SendMessage(WM_VSCROLL, LOWORD(SB_LINEDOWN), NULL);
		} else if (diff > 0) {
			SendMessage(WM_VSCROLL, LOWORD(SB_LINEUP), NULL);
		}
	}
}