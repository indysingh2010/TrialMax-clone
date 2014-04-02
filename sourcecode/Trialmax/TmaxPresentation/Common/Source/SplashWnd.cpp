//==============================================================================
//
// File Name:	SplashWnd.cpp
//
// Description:	This file contains member functions of the CSplashWnd class 
//
// See Also:	SplashWnd.h
//
// Copyright	FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	02-07-2007	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <StdAfx.h>
#include <SplashWnd.h>

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

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CSplashWnd, CWnd)
	//{{AFX_MSG_MAP(CSplashWnd)
	ON_WM_PAINT()
	ON_WM_TIMER()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CSplashWnd::Close()
//
// 	Description:	Called to close the window
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSplashWnd::Close()
{
	// Destroy the window
	DestroyWindow();

	//	Force an upate of the main window
	if(AfxGetMainWnd() != NULL)
		AfxGetMainWnd()->UpdateWindow();
}

//==============================================================================
//
// 	Function Name:	CSplashWnd::CSplashWnd()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CSplashWnd::CSplashWnd()
{
}

//==============================================================================
//
// 	Function Name:	CSplashWnd::~CSplashWnd()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CSplashWnd::~CSplashWnd()
{
}

//==============================================================================
//
// 	Function Name:	CSplashWnd::OnPaint()
//
// 	Description:	Handles WM_PAINT messages sent to the window
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSplashWnd::OnPaint()
{
	CPaintDC dc(this);

	CDC dcImage;
	if(dcImage.CreateCompatibleDC(&dc))
	{
		BITMAP bm;
		m_Bitmap.GetBitmap(&bm);

		// Paint the image.
		CBitmap* pOldBitmap = dcImage.SelectObject(&m_Bitmap);
		dc.BitBlt(0, 0, bm.bmWidth, bm.bmHeight, &dcImage, 0, 0, SRCCOPY);
		dcImage.SelectObject(pOldBitmap);
	}
}

//==============================================================================
//
// 	Function Name:	CSplashWnd::OnTimer()
//
// 	Description:	Handles WM_TIMER messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSplashWnd::OnTimer(UINT nIDEvent)
{
	KillTimer(nIDEvent);

	// Destroy the splash screen window.
	Close();
}

//==============================================================================
//
// 	Function Name:	CSplashWnd::Show()
//
// 	Description:	Called to create and display the window
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CSplashWnd::Show(UINT uBitmapId, CWnd* pwndParent, UINT uTimeOut)
{
	HWND	hwndParent = NULL;
	BITMAP	bm;
	CString	strWndClass = "";

	ASSERT(uBitmapId > 0);
	
	//	Attempt to load the requested bitmap
	if(!m_Bitmap.LoadBitmap(uBitmapId)) 
	{
		TRACE0("Failed to create splash screen bitmap.\n");
		return FALSE;
	}

	//	Retrieve the bitmap descriptor
	m_Bitmap.GetBitmap(&bm);

	strWndClass = AfxRegisterWndClass(0, AfxGetApp()->LoadStandardCursor(IDC_ARROW));

	//	Did the caller specify a parent window?
	if(pwndParent != NULL)
		hwndParent = pwndParent->GetSafeHwnd();
	else
		hwndParent = AfxGetMainWnd()->GetSafeHwnd();

	if(!CreateEx(0, strWndClass, NULL, WS_POPUP | WS_VISIBLE,
		0, 0, bm.bmWidth, bm.bmHeight, hwndParent, NULL))
	{
		TRACE0("Failed to create splash screen.\n");
		return FALSE;
	}

	// Center the window on the display screen
	CenterWindow();
	UpdateWindow();

	// Set a timer to destroy the splash screen if requested
	if(uTimeOut > 0)
		SetTimer(1, uTimeOut, NULL);

	return TRUE;
}


