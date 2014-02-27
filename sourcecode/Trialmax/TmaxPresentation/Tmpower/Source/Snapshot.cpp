//==============================================================================
//
// File Name:	snapshot.cpp
//
// Description:	This file contains member functions of the CSnapshot class.
//
// See Also:	snapshot.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	08-04-99	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <snapshot.h>
#include <powerpt.h>
#include <tpowerap.h>

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
extern CTMPowerApp NEAR theApp;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CSnapshot, CWnd)
	//{{AFX_MSG_MAP(CSnapshot)
	ON_WM_PAINT()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CSnapshot::Create()
//
// 	Description:	This is an overloaded version of the base class member. It
//					will create the snapshot window with the default style.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CSnapshot::Create(CPowerPoint* pParent, BOOL bPopup) 
{
	RECT Rect;

	ASSERT(pParent);

	//	Save a pointer to the parent control
	m_pParent = pParent;

	//	Are we supposed to be creating a popup snapshot?
	if(bPopup)
		return CreatePopup();

	//	Set the initial size to the full client area of the parent
	m_pParent->GetClientRect(&Rect);

	//	Create the window with the default properties
	return CWnd::Create(NULL, "", WS_CHILD, Rect, m_pParent, SNAPSHOT_ID);
}

//==============================================================================
//
// 	Function Name:	CSnapshot::Create()
//
// 	Description:	This is an overloaded version of the base class member. It
//					will create the snapshot window with the default style.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CSnapshot::CreatePopup() 
{
	RECT	Rect;
	CString	strClass;
	DWORD	dwStyle = (CS_DBLCLKS | CS_HREDRAW | CS_VREDRAW);
	
	ASSERT(m_pParent);

	//	Set the initial size to the full client area of the parent
	m_pParent->GetClientRect(&Rect);

	//	Get a class name
	strClass = AfxRegisterWndClass(dwStyle, theApp.LoadStandardCursor(IDC_ARROW),
								   (HBRUSH)(COLOR_WINDOW + 1),
								   theApp.LoadIcon(IDI_APPLICATION));
	
	//	Create the window with the default properties
	return CWnd::CreateEx(0, strClass, "", 
						  WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_MINIMIZEBOX, 
						  Rect, NULL, 0);
}

//==============================================================================
//
// 	Function Name:	CSnapshot::CSnapshot()
//
// 	Description:	This is the constructor for CSnapshot objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CSnapshot::CSnapshot()
{
	m_pParent    = 0;
	m_pScratch   = 0;
	m_pDIBHeader = 0;
	m_pDIBBytes  = 0;
	m_hDDBitmap  = 0;
	m_iDDBWidth  = 0;
	m_iDDBHeight = 0;
	m_iDIBWidth  = 0;
	m_iDIBHeight = 0;
}

//==============================================================================
//
// 	Function Name:	CSnapshot::~CSnapshot()
//
// 	Description:	This is the destructor for CSnapshot objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CSnapshot::~CSnapshot()
{
	//	Clean up
	FreeDDBitmap();
	FreeDIBitmap();
}

//==============================================================================
//
// 	Function Name:	CSnapshot::DrawDDB()
//
// 	Description:	This function will draw the device dependent bitmap 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSnapshot::DrawDDB(CPaintDC* pdc, RECT* pRect) 
{
	if(!m_pScratch)
		return;

	pdc->StretchBlt(0, 0, pRect->right, pRect->bottom, m_pScratch,
				    0, 0, m_iDDBWidth - 1, m_iDDBHeight - 1,
				    SRCCOPY);
}

//==============================================================================
//
// 	Function Name:	CSnapshot::DrawDIB()
//
// 	Description:	This function will draw the device independent bitmap 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSnapshot::DrawDIB(CPaintDC* pdc, RECT* pRect) 
{
	if(!m_pDIBHeader || !m_pDIBBytes)
		return;

	SetStretchBltMode(pdc->GetSafeHdc(), COLORONCOLOR);
	StretchDIBits(pdc->GetSafeHdc(),
				  0, 0, pRect->right, pRect->bottom,
				  0, 0, m_iDIBWidth, m_iDIBHeight,
				  m_pDIBBytes, (BITMAPINFO*)m_pDIBHeader,
				  DIB_RGB_COLORS, SRCCOPY);
}

//==============================================================================
//
// 	Function Name:	CSnapshot::FreeDDBitmap()
//
// 	Description:	This function will deallocate the memory used by the 
//					device dependent bitmap
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSnapshot::FreeDDBitmap() 
{
	//	Destroy the existing scratch dc
	if(m_pScratch)
	{
		delete m_pScratch;
		m_pScratch = 0;
	}

	//	Deallocate the bitmap handle
	if(m_hDDBitmap)
	{
		DeleteObject(m_hDDBitmap);
		m_hDDBitmap = 0;
	}

	m_iDDBWidth = 0;
	m_iDDBHeight = 0;
}

//==============================================================================
//
// 	Function Name:	CSnapshot::FreeDIBitmap()
//
// 	Description:	This function will deallocate the memory used by the 
//					device independent bitmap
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSnapshot::FreeDIBitmap() 
{
	if(m_pDIBHeader)
		HeapFree(GetProcessHeap(), 0, m_pDIBHeader);
	
	m_iDIBWidth  = 0;
	m_iDIBHeight = 0;
	m_pDIBBytes  = 0;
	m_pDIBHeader = 0;
}

//==============================================================================
//
// 	Function Name:	CSnapshot::OnPaint()
//
// 	Description:	This function handles WM_PAINT messages 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSnapshot::OnPaint() 
{
	CPaintDC	dc(this); 
	CBrush		brBackground;
	RECT		Rect;

	//	Get the available client area
	GetClientRect(&Rect);

	//	Are we displaying a device dependent bitmap ?
	if(m_pScratch)
	{	
		DrawDDB(&dc, &Rect);
	}
	else if(m_pDIBHeader && m_pDIBBytes)
	{
		DrawDIB(&dc, &Rect);
	}
	else
	{
		brBackground.CreateSolidBrush(RGB(0,0,0));
		dc.FillRect(&Rect, &brBackground);
	}
}

//==============================================================================
//
// 	Function Name:	CSnapshot::SetDDBitmap()
//
// 	Description:	This function is called to set the device dependent bitmap
//					used to draw the window
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSnapshot::SetDDBitmap(HBITMAP hDDB, int iWidth, int iHeight) 
{
	CDC* pdc;

	if(!IsWindow(m_hWnd)) return;

	//	Free the existing bitmaps
	FreeDDBitmap();
	FreeDIBitmap();

	//	Stop here if we're not loading a new bitmap
	if(hDDB == 0)
	{
		if(IsWindowVisible())
			RedrawWindow();
		return;
	}

	//	Save the new handle
	m_hDDBitmap  = hDDB;
	m_iDDBWidth  = iWidth;
	m_iDDBHeight = iHeight;

	//	Allocate a new scratch dc
	m_pScratch = new CDC();
	ASSERT(m_pScratch);
		
	//	Make the scratch dc compatible with the dc for this window
	pdc = GetDC();
	ASSERT(pdc);
	m_pScratch->CreateCompatibleDC(pdc);
	ReleaseDC(pdc);

	//	Select the new bitmap into the scratch dc
	m_pScratch->SelectObject(m_hDDBitmap);

	//	Update the window if it's visible
	if(IsWindowVisible())
		RedrawWindow();
}

//==============================================================================
//
// 	Function Name:	CSnapshot::SetDIBitmap()
//
// 	Description:	This function is called to set the device independent bitmap
//					used to draw the window
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSnapshot::SetDIBitmap(BITMAPINFOHEADER* pHeader) 
{
	if(!IsWindow(m_hWnd)) return;

	//	Free the existing bitmaps
	FreeDDBitmap();
	FreeDIBitmap();

	//	Stop here if we're not loading a new bitmap
	if(pHeader == 0)
	{
		if(IsWindowVisible())
			RedrawWindow();
		return;
	}
	
	//	Save the pointer to the header
	m_pDIBHeader = pHeader;

	//	Get the bitmap dimensions
	m_iDIBWidth  = m_pDIBHeader->biWidth;
	m_iDIBHeight = m_pDIBHeader->biHeight;

	//	Set the pointer to the actual image bytes
	m_pDIBBytes = (LPBYTE)m_pDIBHeader + m_pDIBHeader->biSize;
	
	//	Update the window if it's visible
	if(IsWindowVisible())
		RedrawWindow();
}

