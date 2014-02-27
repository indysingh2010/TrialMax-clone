//==============================================================================
//
// File Name:	pageset.cpp
//
// Description:	This file contains member functions of the CPageSetup class.
//
// See Also:	pageset.h
//
//==============================================================================
//	Date		Revision    Description
//	08-26-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmviewap.h>
#include <pageset.h>

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
extern CTMViewApp NEAR theApp;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CPageSetup, CDialog)
	//{{AFX_MSG_MAP(CPageSetup)
	ON_BN_CLICKED(IDC_BORDER_COLOR, OnBorderColor)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CPageSetup::CPageSetup()
//
// 	Description:	This is the constructor for CPageSetup objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPageSetup::CPageSetup(CWnd* pParent) : CDialog(CPageSetup::IDD, pParent)
{
	//{{AFX_DATA_INIT(CPageSetup)
	m_fBorderThickness = 0.0f;
	m_fBottomMargin = 0.0f;
	m_fLeftMargin = 0.0f;
	m_fRightMargin = 0.0f;
	m_fTopMargin = 0.0f;
	m_iOrientation = -1;
	m_bPrintCallouts = FALSE;
	m_bPrintBorder = FALSE;
	m_bPrintCalloutBorders = FALSE;
	//}}AFX_DATA_INIT

	m_crBorderColor = RGB(0,0,0);
}

//==============================================================================
//
// 	Function Name:	CPageSetup::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the 
//					dialog box controls and their associated class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPageSetup::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPageSetup)
	DDX_Control(pDX, IDC_BORDER_COLOR, m_ctrlBorderColor);
	DDX_Text(pDX, IDC_BORDER_THICKNESS, m_fBorderThickness);
	DDX_Text(pDX, IDC_BOTTOM_MARGIN, m_fBottomMargin);
	DDX_Text(pDX, IDC_LEFT_MARGIN, m_fLeftMargin);
	DDX_Text(pDX, IDC_RIGHT_MARGIN, m_fRightMargin);
	DDX_Text(pDX, IDC_TOP_MARGIN, m_fTopMargin);
	DDX_CBIndex(pDX, IDC_ORIENTATION, m_iOrientation);
	DDX_Check(pDX, IDC_PRINT_CALLOUTS, m_bPrintCallouts);
	DDX_Check(pDX, IDC_PRINT_BORDER, m_bPrintBorder);
	DDX_Check(pDX, IDC_PRINT_CALLOUT_BORDERS, m_bPrintCalloutBorders);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CPageSetup::OnBorderColor()
//
// 	Description:	This function is called when the user clicks on Border Color
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPageSetup::OnBorderColor() 
{
	CColorDialog Dialog(m_crBorderColor, CC_ANYCOLOR, this);

	if(Dialog.DoModal() == IDOK)
	{
		m_crBorderColor = Dialog.GetColor();
		m_ctrlBorderColor.SetColor(m_crBorderColor);
		m_ctrlBorderColor.RedrawWindow();
	}
}

//==============================================================================
//
// 	Function Name:	CPageSetup::OnInitDialog()
//
// 	Description:	This function traps the WM_INITDIALOG message
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CPageSetup::OnInitDialog() 
{
	//	Perform the base class initialization first
	CDialog::OnInitDialog();

	//	Set the correct color for the border color button
	m_ctrlBorderColor.SetColor(m_crBorderColor);

	return TRUE;
}

