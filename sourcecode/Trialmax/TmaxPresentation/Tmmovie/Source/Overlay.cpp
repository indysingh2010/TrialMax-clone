//==============================================================================
//
// File Name:	overlay.cpp
//
// Description:	This file contains member functions of the COverlay class.
//
// Functions:   COverlay::COverlay()
//				COverlay::~COverlay()
//				COverlay::Create()
//				COverlay::DoDataExchange()
//				COverlay::GetHeight()
//				COverlay::OnCtlColor()
//				COverlay::OnInitDialog()
//				COverlay::OnSize()
//				COverlay::Redraw()
//				COverlay::SetBackColor()
//				COverlay::SetFilename()
//				COverlay::SetMaxWidth()
//
// See Also:	overlay.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	09-25-98	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmmovie.h>
#include <overlay.h>
#include <tmvdefs.h>

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
BEGIN_MESSAGE_MAP(COverlay, CDialog)
	//{{AFX_MSG_MAP(COverlay)
	ON_WM_SIZE()
	ON_WM_CTLCOLOR()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	COverlay::COverlay()
//
// 	Description:	This is the constructor for COverlay objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
COverlay::COverlay(CTMMovieCtrl* pParent) : CDialog(COverlay::IDD, pParent)
{
	//{{AFX_DATA_INIT(COverlay)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	
	ASSERT(pParent);
	m_pTMMovie = pParent;
	m_crBackground = RGB(0,0,0);
	m_pBackground = new CBrush();
	m_pBackground->CreateSolidBrush(m_crBackground);
}

//==============================================================================
//
// 	Function Name:	COverlay::~COverlay()
//
// 	Description:	This is the destructor for COverlay objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
COverlay::~COverlay()
{
	//	Delete the background brush
	if(m_pBackground) delete m_pBackground;
}

//==============================================================================
//
// 	Function Name:	COverlay::Create()
//
// 	Description:	This is an overloaded version of the base class member. It
//					will create the overlay window with the default styles.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL COverlay::Create() 
{
	ASSERT(m_pTMMovie);
	if(!m_pTMMovie) return FALSE;

	//	Create the dialog box
	return CDialog::Create(COverlay::IDD, m_pTMMovie);
}

//==============================================================================
//
// 	Function Name:	COverlay::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the 
//					class members and dialog box controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COverlay::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(COverlay)
	DDX_Control(pDX, IDC_TMVIEW, m_TMView);
	//}}AFX_DATA_MAP
}


//==============================================================================
//
// 	Function Name:	COverlay::GetHeight()
//
// 	Description:	This function is called to get the height of the overlay
//					given the maximum width and aspect ratio of the overlay
//					image
//
// 	Returns:		The recommended height for the overlay window
//
//	Notes:			None
//
//==============================================================================
int COverlay::GetHeight() 
{
	float fRatio;

	//	Is the image loaded?
	if(!m_TMView.IsLoaded(TMV_ACTIVEPANE))
		return 0;

	//	Get the aspect ratio of the image
	if((fRatio = m_TMView.GetSrcRatio(TMV_ACTIVEPANE)) == 0)
		return 0;

	//	Round up the height to make sure we use the full available width
	return (int)(fRatio * (float)m_iMaxWidth + 0.99);
}

//==============================================================================
//
// 	Function Name:	COverlay::OnCtlColor()
//
// 	Description:	This function is overloaded to draw the dialog box in the
//					appropriate color
//
// 	Returns:		The handle of the brush used to paint the background
//
//	Notes:			None
//
//==============================================================================
HBRUSH COverlay::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor) 
{
	//	Is this a request for the dialog brush?
	if(nCtlColor == CTLCOLOR_DLG && m_pBackground)
		return (HBRUSH)(*m_pBackground);
	else
		return CDialog::OnCtlColor(pDC, pWnd, nCtlColor);
}

//==============================================================================
//
// 	Function Name:	COverlay::OnInitDialog()
//
// 	Description:	This function called to initialize the dialog when it is
//					created
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL COverlay::OnInitDialog() 
{
	RECT rcClient;
	RECT rcParent;

	//	Perform the base class initialization
	CDialog::OnInitDialog();
	
	//	Get the width of the parent window
	if(m_pTMMovie)
	{
		m_pTMMovie->GetClientRect(&rcParent);
		m_iMaxWidth = rcParent.right - rcParent.left;
	}
	
	//	Set the intial size of the TMView control
	if(IsWindow(m_TMView.m_hWnd))
	{
		GetClientRect(&rcClient);
		m_TMView.MoveWindow(&rcClient);
	}

	//	Disable the TMView's error handler
	m_TMView.SetEnableErrors(FALSE);

	return TRUE;  
}

//==============================================================================
//
// 	Function Name:	COverlay::OnSize()
//
// 	Description:	This function handles all WM_SIZE messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COverlay::OnSize(UINT nType, int cx, int cy) 
{
	//	Perform the base class processing
	CDialog::OnSize(nType, cx, cy);

	//	Make sure the TMView is sized correctly
	if(IsWindow(m_TMView.m_hWnd))
		m_TMView.MoveWindow(0, 0, cx, cy);
}

//==============================================================================
//
// 	Function Name:	COverlay::SetBackColor()
//
// 	Description:	This function is called to set the background color of the
//					overlay image and the dialog box
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COverlay::SetBackColor(COLORREF crBackground) 
{
	//	Save the new color
	m_crBackground = crBackground;

	//	Create a new brush
	if(m_pBackground) delete m_pBackground;
	m_pBackground = new CBrush();
	m_pBackground->CreateSolidBrush(crBackground);

	//	Set the background color
	m_TMView.SetBackColor((OLE_COLOR)m_crBackground);
}

//==============================================================================
//
// 	Function Name:	COverlay::Redraw()
//
// 	Description:	This function will force a redrawing of the image
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void COverlay::Redraw() 
{
	if(IsWindow(m_TMView.m_hWnd))
		m_TMView.Redraw();
}

//==============================================================================
//
// 	Function Name:	COverlay::SetFilename()
//
// 	Description:	This function is called to set the filename of the image to
//					be displayed in the overlay window
//
// 	Returns:		The recommended height for the overlay window
//
//	Notes:			None
//
//==============================================================================
int COverlay::SetFilename(LPCSTR lpFilename) 
{
	//	Set the image file
	if(m_TMView.LoadFile(lpFilename, TMV_ACTIVEPANE) != TMV_NOERROR)
		m_TMView.LoadFile("", TMV_ACTIVEPANE);

	//	Return the recommended height based on the aspect ratio of the image
	return GetHeight();
}

//==============================================================================
//
// 	Function Name:	COverlay::SetMaxWidth()
//
// 	Description:	This function is called to set the maximum width available
//					to the overlay
//
// 	Returns:		The recommended height for the overlay
//
//	Notes:			None
//
//==============================================================================
int COverlay::SetMaxWidth(int iMaxWidth) 
{
	//	Save the maximum width
	m_iMaxWidth = iMaxWidth;

	//	Return the recommended height based on the aspect ratio of the image
	return GetHeight();
}


