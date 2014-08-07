//==============================================================================
//
// File Name:	frame.cpp
//
// Description:	This file contains member functions of the CMainFrame class.
//
// Functions:   CMainFrame::CMainFrame()
//				CMainFrame::~CMainFrame()
//				CMainFrame::LoadFromBarcode()
//				CMainFrame::LoadNewPage()
//				CMainFrame::OnClose()
//				CMainFrame::OnCreate()
//				CMainFrame::PreCreateWindow()
//				CMainFrame::ProcessCommandKey()
//				CMainFrame::ProcessVirtualKey()
//				CMainFrame::RecalcLayout()
//				
// See Also:	frame.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	08-17-97	1.00		Original Release
//	12-07-97	1.10		Added CuePlaylist()
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <app.h>
#include <frame.h>
#include <document.h>
#include <view.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------
extern CApp theApp;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
IMPLEMENT_DYNCREATE(CMainFrame, CFrameWnd)

BEGIN_MESSAGE_MAP(CMainFrame, CFrameWnd)
	//{{AFX_MSG_MAP(CMainFrame)
	ON_WM_CREATE()
	ON_WM_CLOSE()
	ON_WM_MOVE()
	//}}AFX_MSG_MAP
	ON_MESSAGE(WM_NEWINSTANCE, OnWMNewInstance)
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CMainFrame::CMainFrame()
//
// 	Description:	This is the constructor for CMainFrame objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMainFrame::CMainFrame()
{
	//{{AFX_DATA_INIT(CMainFrame)
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CMainFrame::~CMainFrame()
//
// 	Description:	This is the destructor for CMainFrame objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMainFrame::~CMainFrame()
{
}

//==============================================================================
//
// 	Function Name:	CMainFrame::GetAlternateBarcodeChar()
//
// 	Description:	This function retrieves the alternate barcode character from
//					the main view.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
char CMainFrame::GetAlternateBarcodeChar()
{
	CMainView* pView = (CMainView*)GetActiveView();

	if(pView)
		return pView->GetAlternateBarcodeChar();
	else
		return 0;
}

//==============================================================================
//
// 	Function Name:	CMainFrame::GetPrimaryBarcodeChar()
//
// 	Description:	This function retrieves the primary barcode character from
//					the main view.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
char CMainFrame::GetPrimaryBarcodeChar()
{
	CMainView* pView = (CMainView*)GetActiveView();

	if(pView)
		return pView->GetPrimaryBarcodeChar();
	else
		return 0;
}

//==============================================================================
//
// 	Function Name:	CMainFrame::GetUseSecondaryMonitor()
//
// 	Description:	This function retrieves the flag that indicates if the
//					app should be run on the secondary monitor.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CMainFrame::GetUseSecondaryMonitor()
{
	CMainView* pView = (CMainView*)GetActiveView();

	if(pView)
		return pView->GetUseSecondaryMonitor();
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CMainFrame::GetVKChar()
//
// 	Description:	This function retrieves the virtual key character from
//					the main view.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
char CMainFrame::GetVKChar()
{
	CMainView* pView = (CMainView*)GetActiveView();

	if(pView)
		return pView->GetVKChar();
	else
		return 0;
}

//==============================================================================
//
// 	Function Name:	CMainFrame::LoadFromBarcode()
//
// 	Description:	This function will inform the view that it should load a
//					new image based on a barcode.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainFrame::LoadFromBarcode(LPCSTR lpBarcode, BOOL bAddBuffer, BOOL bAlternate)
{
	CMainView* pView = (CMainView*)GetActiveView();

	//	Tell the view to load a new file using the barcode
	if(pView && lpBarcode)
		pView->LoadFromBarcode(lpBarcode, bAddBuffer, bAlternate);
}

//==============================================================================
//
// 	Function Name:	CMainFrame::LoadPageFromKeyboard()
//
// 	Description:	This function will inform the view that it should load a
//					new page from the current media database.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainFrame::LoadPageFromKeyboard(long lPage)
{
	CMainView* pView = (CMainView*)GetActiveView();

	//	Tell the view to load the new page
	if(pView)
		pView->LoadPageFromKeyboard(lPage);
}

//==============================================================================
//
// 	Function Name:	CMainFrame::OnClose()
//
// 	Description:	This function is called when the application is being
//					closed. It makes sure the view window is shut down
//					before closing the application.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainFrame::OnClose() 
{
	CMainView* pView = (CMainView*)GetActiveView();

	//	Tell the view to shut down
	if((pView == NULL) || (pView->Shutdown() == TRUE))
	{
		//	Perform the base class processing
		CFrameWnd::OnClose();
	}

}

//==============================================================================
//
// 	Function Name:	CMainFrame::OnCreate()
//
// 	Description:	This function is called when the window is created.
//
// 	Returns:		0 if successful.
//
//	Notes:			None
//
//==============================================================================
int CMainFrame::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	//	Perform base class processing
	if(CFrameWnd::OnCreate(lpCreateStruct) == -1)
		return -1;
	
	//	Set the instance key so that we can detect previous instances
	theApp.LockInstance(m_hWnd);

	//	Turn the menu off
	SetMenu(0);             

	//	Remove the caption and border
	ModifyStyle(WS_CAPTION | WS_BORDER, 0, SWP_FRAMECHANGED);
	ModifyStyleEx(WS_EX_CLIENTEDGE, 0, SWP_FRAMECHANGED);

	return 0;
}

//==============================================================================
//
// 	Function Name:	CMainFrame::OnWMNewInstance()
//
// 	Description:	This function handles all WM_NEWINSTANCE messages
//
// 	Returns:		Zero if the message is handled
//
//	Notes:			None
//
//==============================================================================
LONG CMainFrame::OnWMNewInstance(WPARAM wParam, LPARAM lParam)
{
	CMainView* pView = (CMainView*)GetActiveView();

	if(pView)
		pView->OnNewInstance();

	return 0;
}

//==============================================================================
//
// 	Function Name:	CMainFrame::PreCreateWindow()
//
// 	Description:	This function is called before the window is created. It is
//					overloaded to set the window style.
//
// 	Returns:		TRUE if successful.
//
//	Notes:			None
//
//==============================================================================
BOOL CMainFrame::PreCreateWindow(CREATESTRUCT& cs)
{
	cs.style = WS_OVERLAPPED;

	//cs.cx = 440;
    // cs.cy = 480;
    // cs.style &= ~WS_SIZEBOX;
	return CFrameWnd::PreCreateWindow(cs);
}

//==============================================================================
//
// 	Function Name:	CMainFrame::ProcessCommandKey()
//
// 	Description:	This function will give the view an opprotunity to translate
//					the key to a command.
//
// 	Returns:		TRUE if processed
//
//	Notes:			None
//
//==============================================================================
BOOL CMainFrame::ProcessCommandKey(char cKey)
{
	CMainView* pView = (CMainView*)GetActiveView();

	//	Tell the view to load the new page
	if(pView)
		return pView->ProcessCommandKey(cKey);
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CMainFrame::ProcessMouseMessage()
//
// 	Description:	This function is called by the application hook to process
//					the specified mouse message.
//
// 	Returns:		TRUE if processed
//
//	Notes:			None
//
//==============================================================================
BOOL CMainFrame::ProcessMouseMessage(MSG* pMsg)
{
	CMainView* pView = (CMainView*)GetActiveView();

	//	Tell the view to load the new page
	if(pView)
		return pView->ProcessMouseMessage(pMsg);
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CMainFrame::ProcessVirtualKey()
//
// 	Description:	This function will give the view an opprotunity to translate
//					the virtual key to a command.
//
// 	Returns:		TRUE if processed
//
//	Notes:			None
//
//==============================================================================
BOOL CMainFrame::ProcessVirtualKey(WORD wKey)
{
	CMainView* pView = (CMainView*)GetActiveView();

	//	Tell the view to load the new page
	if(pView)
		return pView->ProcessVirtualKey(wKey);
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CMainFrame::RecalcLayout()
//
// 	Description:	This function is called when the frame is resized. It will
//					reposition the toolbars and resize the child window to 
//					consume the available client area.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainFrame::RecalcLayout(BOOL bNotify) 
{
	CMainView* pView = (CMainView*)GetActiveView();
	
	//	Hide the view before we do the base class recalculations. This
	//	hides the redrawing of the view that occurs when the base
	//	class processing is performed
	if(pView)
	{	
		pView->m_bRedraw = FALSE;
		pView->ShowWindow(SW_HIDE);
	}

	// Perform base class processing 
	CFrameWnd::RecalcLayout(bNotify);
	
	//	Now show the view
	if(pView)
	{
		pView->m_bRedraw = TRUE;
		pView->ShowWindow(SW_SHOW);
	}
}

//==============================================================================
//
// 	Function Name:	CMainFrame::SetLoadLine()
//
// 	Description:	This function set the line used to cue the playback when
//					loading a new playlist or deposition
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainFrame::SetLoadLine(int iLine)
{
	CMainView* pView = (CMainView*)GetActiveView();

	if(pView)
		pView->SetLoadLine(iLine);
}

//==============================================================================
//
// 	Function Name:	CMainFrame::SetLoadPage()
//
// 	Description:	This function set the page used to cue the playback when
//					loading a new playlist or deposition
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainFrame::SetLoadPage(int iPage)
{
	CMainView* pView = (CMainView*)GetActiveView();

	if(pView)
		pView->SetLoadPage(iPage);
}


void CMainFrame::OnMove(int x, int y) 
{
	CFrameWnd::OnMove(x, y);
/*	
	RECT rcWnd;
	GetWindowRect(&rcWnd);
	CString M;
	M.Format("L: %d  T: %d  R:  %d  T:  %d", rcWnd.left, rcWnd.top, rcWnd.right, rcWnd.bottom);
	SetWindowText(M);
*/	
}


void CMainFrame::UpdateBarcode(CString Barcode)
{
	((CMainView*)GetActiveView())->UpdateBarcodeText(Barcode);
}