//==============================================================================
//
// File Name:	webframe.cpp
//
// Description:	This file contains member functions of the CWebFrame class.
//
// See Also:	webframe.h
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	02-20-02	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <webframe.h>
#include <handler.h>
#include <tmbrowsedef.h>
#include <browsectl.h>

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
BEGIN_MESSAGE_MAP(CWebFrame, CDialog)
	//{{AFX_MSG_MAP(CWebFrame)
	ON_WM_SIZE()
	ON_WM_CTLCOLOR()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

BEGIN_EVENTSINK_MAP(CWebFrame, CDialog)
    //{{AFX_EVENTSINK_MAP(CWebFrame)
	ON_EVENT(CWebFrame, ID_BROWSER_CONTROL, 259 /* DocumentComplete */, OnDocumentComplete, VTS_DISPATCH VTS_PVARIANT)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()

//==============================================================================
//
// 	Function Name:	OnEnumChild()
//
// 	Description:	This callback is used to for child window enumeration
//
// 	Returns:		TRUE to continue the enumeration
//
//	Notes:			None
//
//==============================================================================
BOOL CALLBACK OnEnumChild(HWND hWnd, LPARAM lpParam)
{
	CWnd* pWnd = CWnd::FromHandle(hWnd);

	//	Force a redrawing of the child window
	if(pWnd)
		pWnd->RedrawWindow();

	return TRUE;
};

//==============================================================================
//
// 	Function Name:	CWebFrame::Create()
//
// 	Description:	This is an overloaded version of the base class member. It
//					will create the frame window.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CWebFrame::Create() 
{
	ASSERT(m_pControl);
	if(!m_pControl) return FALSE;

	//	Create the dialog box
	return CDialog::Create(CWebFrame::IDD, (CWnd*)m_pControl);
}

//==============================================================================
//
// 	Function Name:	CWebFrame::CreateBrowser()
//
// 	Description:	This function will create the browser window
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CWebFrame::CreateBrowser()
{
	//	Do we already have a browser window?
	if((m_pBrowser != 0) && IsWindow(m_pBrowser->m_hWnd))
		return TRUE;

	//	Deallocate the existing object if there is one
	if(m_pBrowser)
		delete m_pBrowser;

	//	Allocate a new object
	m_pBrowser = new CWebBrowser2();
	ASSERT(m_pBrowser);

	//	Create the new window
	if(!m_pBrowser->Create("", WS_CHILD | WS_VISIBLE, CRect(0,0,0,0),
						   this, ID_BROWSER_CONTROL))
	{
		delete m_pBrowser;
		return FALSE;
	}

	//	Make sure the control is properly sized
	RecalcLayout();

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CWebFrame::CWebFrame()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CWebFrame::CWebFrame(CTMBrowseCtrl* pControl, CErrorHandler* pErrors)
		  :CDialog(CWebFrame::IDD, (CWnd*)pControl)
{
	//{{AFX_DATA_INIT(CWebFrame)
	//}}AFX_DATA_INIT

	m_pControl = pControl;
	m_pErrors  = pErrors;
	m_pBrowser = 0;
	m_crBackground = RGB(0,0,0);
	m_pBackground = new CBrush();
	m_pBackground->CreateSolidBrush(m_crBackground);
	m_strFilename.Empty();
}

//==============================================================================
//
// 	Function Name:	CWebFrame::~CWebFrame()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CWebFrame::~CWebFrame()
{
	if(m_pBackground)
	{
		delete m_pBackground;
		m_pBackground = 0;
	}
	if(m_pBrowser)
	{
		delete m_pBrowser;
		m_pBrowser = 0;
	}
}

//==============================================================================
//
// 	Function Name:	CWebFrame::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					dialog box and the class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CWebFrame::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CWebFrame)
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CWebFrame::Load()
//
// 	Description:	This function will load the specified file into the browser
//
// 	Returns:		TMBROWSE_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
int CWebFrame::Load(LPCSTR lpszFilename)
{
	ASSERT(lpszFilename);

	COleSafeArray	vPostData;
	COleVariant		vURL(lpszFilename, VT_BSTR);
	COleVariant		vHeaders("", VT_BSTR);
	COleVariant		vTargetFrameName("", VT_BSTR);
	COleVariant		vFlags((long)0, VT_I4);

	//	Make sure we have the browser window
	if(!CreateBrowser())
	{
		if(m_pErrors)
			m_pErrors->Handle(0, IDS_BROWSERFAILED);
		return TMBROWSE_BROWSERFAILED;
	}

	//	Save the filename
	m_strFilename = lpszFilename;

	//	Load the requested file
	m_pBrowser->Navigate2(vURL, vFlags, vTargetFrameName, vPostData, vHeaders);
	
	return TMBROWSE_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CWebFrame::OnCtlColor()
//
// 	Description:	This function traps all WM_CTLCOLOR messages
//
// 	Returns:		Handle to the brush used to paint the background
//
//	Notes:			None
//
//==============================================================================
HBRUSH CWebFrame::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor) 
{
	//	Is this a request for the dialog brush?
	if(nCtlColor == CTLCOLOR_DLG && m_pBackground)
		return (HBRUSH)(*m_pBackground);
	else
		return CDialog::OnCtlColor(pDC, pWnd, nCtlColor);
}

//==============================================================================
//
// 	Function Name:	CWebFrame::OnDocumentComplete()
//
// 	Description:	This functions traps all DocumentComplete notifications 
//					fired by the browser control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CWebFrame::OnDocumentComplete(LPDISPATCH pDisp, VARIANT FAR* URL) 
{
	//	Notify the container
	if(m_pControl && m_pBrowser)
		m_pControl->FireLoadComplete(m_strFilename);

	//	Make sure everything is drawn properly
	Redraw();
}

//==============================================================================
//
// 	Function Name:	CWebFrame::OnInitDialog()
//
// 	Description:	This function traps the WM_INITDIALOG message
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CWebFrame::OnInitDialog() 
{
	//	Do the base class processing first
	CDialog::OnInitDialog();
	
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CWebFrame::OnSize()
//
// 	Description:	This function handles all WM_SIZE messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CWebFrame::OnSize(UINT nType, int cx, int cy) 
{
	//	Do the base class processing
	CDialog::OnSize(nType, cx, cy);
	
	//	Resize the browser window
	RecalcLayout();
}

//==============================================================================
//
// 	Function Name:	CWebFrame::RecalcLayout()
//
// 	Description:	This function is called to set the size and position of the
//					browser window within the available client area
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CWebFrame::RecalcLayout() 
{
	RECT rcMax;

	//	Resize the browser control
	if((m_pBrowser != 0) && IsWindow(m_pBrowser->m_hWnd))
	{
		//	Get the maximum available area
		GetClientRect(&rcMax);

		//	Resize to use the full client area
		m_pBrowser->MoveWindow(&rcMax);
		
		//	Force a redrawing of the browser
		Redraw();
	}
}

//==============================================================================
//
// 	Function Name:	CWebFrame::Redraw()
//
// 	Description:	This function is called to redraw the browser window
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CWebFrame::Redraw() 
{
	if((m_pBrowser != 0) && IsWindow(m_pBrowser->m_hWnd))
	{
		//	Explicitly redraw each child
		//
		//	NOTE:	We do this because the browser control does not always draw
		//			properly after the window has been resized
		::EnumChildWindows(m_pBrowser->m_hWnd, OnEnumChild, 0);
	}
	else
	{
		RedrawWindow();
	}
}

//==============================================================================
//
// 	Function Name:	CWebFrame::SetBackgroundColor()
//
// 	Description:	This function is called to set the background color of the
//					dialog box.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CWebFrame::SetBackgroundColor(COLORREF crBackground) 
{
	//	Save the new color
	m_crBackground = crBackground;

	//	Create a new brush
	if(m_pBackground) delete m_pBackground;
	m_pBackground = new CBrush();
	m_pBackground->CreateSolidBrush(crBackground);
}

//==============================================================================
//
// 	Function Name:	CWebFrame::Unload()
//
// 	Description:	This function is called to unload the browser window
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CWebFrame::Unload() 
{
	//	The browser control does not provide a means for unloading so we have to
	//	dynamically allocate and destroy instances of the control
	if(m_pBrowser != 0)
	{
		delete m_pBrowser;
		m_pBrowser = 0;
	}

	m_strFilename.Empty();
}


