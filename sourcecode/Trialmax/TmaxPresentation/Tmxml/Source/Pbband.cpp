//==============================================================================
//
// File Name:	pbband.cpp
//
// Description:	This file contains member functions of the CPBBand class.
//
// See Also:	pbband.h
//
//==============================================================================
//	Date		Revision    Description
//	06-19-02	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <pbband.h>
#include <xmlframe.h>
#include <xmlpage.h>
#include <tmtbdefs.h>

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
BEGIN_MESSAGE_MAP(CPBBand, CDialog)
	//{{AFX_MSG_MAP(CPBBand)
	ON_WM_SIZE()
	ON_WM_CTLCOLOR()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CPBBand::CPBBand()
//
// 	Description:	This is the constructor for CPBBand objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPBBand::CPBBand(CPageBar* pPageBar, UINT uResourceId) 
		:CDialog(uResourceId, pPageBar)
{
	//{{AFX_DATA_INIT(CPBBand)
	//}}AFX_DATA_INIT

	m_pPageBar		= pPageBar;
	m_pXmlMedia		= 0;
	m_pXmlPage		= 0;
	m_uResourceId	= uResourceId;
	m_bResizable	= TRUE;
	m_iInitialWidth	= 0;	//	Derived classes should override this
	m_iMinimumWidth = 1;	//	CReBarCtrl doesn't do well with zero width
	memset(&m_rcWnd, 0, sizeof(m_rcWnd));

	m_pbrBackground = 0;
}

//==============================================================================
//
// 	Function Name:	CPBBand::~CPBBand()
//
// 	Description:	This is the destructor for CPBBand objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPBBand::~CPBBand()
{
	if(m_pbrBackground)
		delete m_pbrBackground;
}

//==============================================================================
//
// 	Function Name:	CPBBand::Create()
//
// 	Description:	This function is called to create the window.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPBBand::Create(CPageBar* pPageBar)
{
	ASSERT(pPageBar != 0);
	m_pPageBar = pPageBar;

	//	Create the window
	if(CDialog::Create(m_uResourceId, m_pPageBar) == FALSE)
		return FALSE;

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPBBand::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the 
//					dialog box controls and class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBBand::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPBBand)
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CPBBand::GetInitialWidth()
//
// 	Description:	This function is called to get the initial width of this
//					band.
//
// 	Returns:		The initial width in pixels
//
//	Notes:			None
//
//==============================================================================
int CPBBand::GetInitialWidth()
{
	return m_iInitialWidth;
}

//==============================================================================
//
// 	Function Name:	CPBBand::OnCtlColor()
//
// 	Description:	This function traps the WM_CTLCOLOR message
//
// 	Returns:		Handle to the background brush
//
//	Notes:			None
//
//==============================================================================
HBRUSH CPBBand::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor) 
{
	if(m_pbrBackground != 0)
	{
		if((nCtlColor == CTLCOLOR_DLG) || (nCtlColor == CTLCOLOR_STATIC))
		{
			return ((HBRUSH)(*m_pbrBackground));
		}
	}

	return CDialog::OnCtlColor(pDC, pWnd, nCtlColor);
}

//==============================================================================
//
// 	Function Name:	CPBBand::OnInitDialog()
//
// 	Description:	This function traps the	WM_INITDIALOG message
//
// 	Returns:		TRUE for default focus
//
//	Notes:			None
//
//==============================================================================
BOOL CPBBand::OnInitDialog() 
{
	//	Do the base class initialization
	CDialog::OnInitDialog();
	
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPBBand::OnSize()
//
// 	Description:	This function traps the WM_SIZE message
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBBand::OnSize(UINT nType, int cx, int cy) 
{
	//	Do the base class processing
	CDialog::OnSize(nType, cx, cy);
	
	//	Notify the parent
	if(m_pPageBar != 0)
		m_pPageBar->OnBandSize(m_iId);
}

//==============================================================================
//
// 	Function Name:	CPBBand::RecalcLayout()
//
// 	Description:	This function is called to recalculate the layout of the
//					controls in the band.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBBand::RecalcLayout()
{
	if(IsWindow(m_hWnd))
		GetClientRect(&m_rcWnd);
}

//==============================================================================
//
// 	Function Name:	CPBBand::SetFont()
//
// 	Description:	This function is called to set the dialog font
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBBand::SetFont(CFont* pFont)
{
	if(IsWindow(m_hWnd))
		CDialog::SetFont(pFont);
}

//==============================================================================
//
// 	Function Name:	CPBBand::SetXmlMedia()
//
// 	Description:	This function is called to set the current document
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBBand::SetXmlMedia(CXmlMedia* pXmlMedia)
{
	m_pXmlMedia = pXmlMedia;
}

//==============================================================================
//
// 	Function Name:	CPBBand::SetXmlPage()
//
// 	Description:	This function is called to set the active Xml page
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBBand::SetXmlPage(CXmlPage* pXmlPage)
{
	m_pXmlPage = pXmlPage;
}


