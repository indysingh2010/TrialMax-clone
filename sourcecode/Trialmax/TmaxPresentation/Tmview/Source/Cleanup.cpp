//==============================================================================
//
// File Name:	cleanup.cpp
//
// Description:	This file contains member functions of the CCleanup class.
//
// See Also:	cleanup.h
//
//==============================================================================
//	Date		Revision    Description
//	06-26-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmviewap.h>
#include <cleanup.h>
#include <tmview.h>
#include <ltimgcor.h>

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
BEGIN_MESSAGE_MAP(CCleanup, CDialog)
	//{{AFX_MSG_MAP(CCleanup)
	ON_BN_CLICKED(IDC_CLEAN, OnClean)
	ON_BN_CLICKED(IDC_SAVE, OnSave)
	ON_BN_CLICKED(IDUNDO, OnUndo)
	ON_BN_CLICKED(IDC_REMOVEBORDERS, OnRemoveBorders)
	ON_BN_CLICKED(IDC_REMOVEDOTS, OnRemoveDots)
	ON_BN_CLICKED(IDC_REMOVEHOLES, OnRemoveHoles)
	ON_BN_CLICKED(IDC_SMOOTH, OnSmooth)
	ON_BN_CLICKED(IDC_DESKEW_COLOR, OnDeskewColor)
	ON_BN_CLICKED(IDC_DESKEW, OnDeskew)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CCleanup::CCleanup()
//
// 	Description:	This is the constructor for CCleanup objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CCleanup::CCleanup(CTMLead* pTMLead, LPCTSTR lpszSaveAs) 
		 :CDialog(CCleanup::IDD, pTMLead)
{
	//{{AFX_DATA_INIT(CCleanup)
	m_bDeskew = TRUE;
	m_bDespeckle = FALSE;
	m_bRemoveBorders = FALSE;
	m_bRemoveDots = FALSE;
	m_bRemoveHoles = FALSE;
	m_bSmooth = FALSE;
	m_lDotsMaxHeight = 50;
	m_lDotsMaxWidth = 50;
	m_lDotsMinHeight = 1;
	m_lDotsMinWidth = 1;
	m_lHolesMaxHeight = 500;
	m_lHolesMaxWidth = 500;
	m_lHolesMinHeight = 125;
	m_lHolesMinWidth = 125;
	m_bHolesBottom = FALSE;
	m_bHolesLeft = TRUE;
	m_bHolesRight = FALSE;
	m_bHolesTop = FALSE;
	m_bBordersBottom = TRUE;
	m_bBordersLeft = TRUE;
	m_bBordersRight = TRUE;
	m_bBordersTop = TRUE;
	m_lBordersPercent = 10;
	m_lBordersNoise = 10;
	m_lBordersVariance = 2;
	m_lSmoothLength = 4;
	m_bSmoothLong = FALSE;
	//}}AFX_DATA_INIT

	m_pTMLead = pTMLead;
	if(lpszSaveAs)
		m_strSaveAs = lpszSaveAs;
	else
		m_strSaveAs.Empty();
}

//==============================================================================
//
// 	Function Name:	CCleanup::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the 
//					dialog box controls and their associated class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCleanup::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CCleanup)
	DDX_Control(pDX, IDC_SAVE, m_ctrlSave);
	DDX_Control(pDX, IDUNDO, m_ctrlUndo);
	DDX_Control(pDX, IDC_LEAD, m_Undo);
	DDX_Control(pDX, IDC_DESKEW_COLOR_LABEL, m_ctrlDeskewColorLabel);
	DDX_Control(pDX, IDC_DESKEW_GROUP, m_ctrlDeskewGroup);
	DDX_Control(pDX, IDC_DESKEW_COLOR, m_ctrlDeskewColor);
	DDX_Control(pDX, IDC_SMOOTH_LONG, m_ctrlSmoothLong);
	DDX_Control(pDX, IDC_SMOOTH_LENGTH_LABEL, m_ctrlSmoothLengthLabel);
	DDX_Control(pDX, IDC_SMOOTH_LENGTH, m_ctrlSmoothLength);
	DDX_Control(pDX, IDC_SMOOTH_GROUP, m_ctrlSmoothGroup);
	DDX_Control(pDX, IDC_BORDERS_GROUP, m_ctrlBordersGroup);
	DDX_Control(pDX, IDC_BORDERS_PERCENT, m_ctrlBordersPercent);
	DDX_Control(pDX, IDC_BORDERS_PERCENT_LABEL, m_ctrlBordersPercentLabel);
	DDX_Control(pDX, IDC_BORDERS_NOISE, m_ctrlBordersNoise);
	DDX_Control(pDX, IDC_BORDERS_NOISE_LABEL, m_ctrlBordersNoiseLabel);
	DDX_Control(pDX, IDC_BORDERS_VARIANCE, m_ctrlBordersVariance);
	DDX_Control(pDX, IDC_BORDERS_VARIANCE_LABEL, m_ctrlBordersVarianceLabel);
	DDX_Control(pDX, IDC_BORDERS_TOP, m_ctrlBordersTop);
	DDX_Control(pDX, IDC_BORDERS_RIGHT, m_ctrlBordersRight);
	DDX_Control(pDX, IDC_BORDERS_LEFT, m_ctrlBordersLeft);
	DDX_Control(pDX, IDC_BORDERS_BOTTOM, m_ctrlBordersBottom);
	DDX_Control(pDX, IDC_HOLES_TOP, m_ctrlHolesTop);
	DDX_Control(pDX, IDC_HOLES_RIGHT, m_ctrlHolesRight);
	DDX_Control(pDX, IDC_HOLES_LEFT, m_ctrlHolesLeft);
	DDX_Control(pDX, IDC_HOLES_BOTTOM, m_ctrlHolesBottom);
	DDX_Control(pDX, IDC_DOTS_MIN_WIDTH_LABEL, m_ctrlDotsMinWidthLabel);
	DDX_Control(pDX, IDC_DOTS_MIN_WIDTH, m_ctrlDotsMinWidth);
	DDX_Control(pDX, IDC_DOTS_MIN_HEIGHT_LABEL, m_ctrlDotsMinHeightLabel);
	DDX_Control(pDX, IDC_DOTS_MIN_HEIGHT, m_ctrlDotsMinHeight);
	DDX_Control(pDX, IDC_DOTS_MAX_WIDTH_LABEL, m_ctrlDotsMaxWidthLabel);
	DDX_Control(pDX, IDC_DOTS_MAX_WIDTH, m_ctrlDotsMaxWidth);
	DDX_Control(pDX, IDC_DOTS_MAX_HEIGHT_LABEL, m_ctrlDotsMaxHeightLabel);
	DDX_Control(pDX, IDC_DOTS_MAX_HEIGHT, m_ctrlDotsMaxHeight);
	DDX_Control(pDX, IDC_DOTS_GROUP, m_ctrlDotsGroup);
	DDX_Check(pDX, IDC_DESKEW, m_bDeskew);
	DDX_Check(pDX, IDC_DESPECKLE, m_bDespeckle);
	DDX_Check(pDX, IDC_REMOVEBORDERS, m_bRemoveBorders);
	DDX_Check(pDX, IDC_REMOVEDOTS, m_bRemoveDots);
	DDX_Check(pDX, IDC_REMOVEHOLES, m_bRemoveHoles);
	DDX_Check(pDX, IDC_SMOOTH, m_bSmooth);
	DDX_Text(pDX, IDC_DOTS_MAX_HEIGHT, m_lDotsMaxHeight);
	DDX_Text(pDX, IDC_DOTS_MAX_WIDTH, m_lDotsMaxWidth);
	DDX_Text(pDX, IDC_DOTS_MIN_HEIGHT, m_lDotsMinHeight);
	DDX_Text(pDX, IDC_DOTS_MIN_WIDTH, m_lDotsMinWidth);
	DDX_Control(pDX, IDC_HOLES_MIN_WIDTH_LABEL, m_ctrlHolesMinWidthLabel);
	DDX_Control(pDX, IDC_HOLES_MIN_WIDTH, m_ctrlHolesMinWidth);
	DDX_Control(pDX, IDC_HOLES_MIN_HEIGHT_LABEL, m_ctrlHolesMinHeightLabel);
	DDX_Control(pDX, IDC_HOLES_MIN_HEIGHT, m_ctrlHolesMinHeight);
	DDX_Control(pDX, IDC_HOLES_MAX_WIDTH_LABEL, m_ctrlHolesMaxWidthLabel);
	DDX_Control(pDX, IDC_HOLES_MAX_WIDTH, m_ctrlHolesMaxWidth);
	DDX_Control(pDX, IDC_HOLES_MAX_HEIGHT_LABEL, m_ctrlHolesMaxHeightLabel);
	DDX_Control(pDX, IDC_HOLES_MAX_HEIGHT, m_ctrlHolesMaxHeight);
	DDX_Control(pDX, IDC_HOLES_GROUP, m_ctrlHolesGroup);
	DDX_Text(pDX, IDC_HOLES_MAX_HEIGHT, m_lHolesMaxHeight);
	DDX_Text(pDX, IDC_HOLES_MAX_WIDTH, m_lHolesMaxWidth);
	DDX_Text(pDX, IDC_HOLES_MIN_HEIGHT, m_lHolesMinHeight);
	DDX_Text(pDX, IDC_HOLES_MIN_WIDTH, m_lHolesMinWidth);
	DDX_Check(pDX, IDC_HOLES_BOTTOM, m_bHolesBottom);
	DDX_Check(pDX, IDC_HOLES_LEFT, m_bHolesLeft);
	DDX_Check(pDX, IDC_HOLES_RIGHT, m_bHolesRight);
	DDX_Check(pDX, IDC_HOLES_TOP, m_bHolesTop);
	DDX_Check(pDX, IDC_BORDERS_BOTTOM, m_bBordersBottom);
	DDX_Check(pDX, IDC_BORDERS_LEFT, m_bBordersLeft);
	DDX_Check(pDX, IDC_BORDERS_RIGHT, m_bBordersRight);
	DDX_Check(pDX, IDC_BORDERS_TOP, m_bBordersTop);
	DDX_Text(pDX, IDC_BORDERS_PERCENT, m_lBordersPercent);
	DDX_Text(pDX, IDC_BORDERS_NOISE, m_lBordersNoise);
	DDX_Text(pDX, IDC_BORDERS_VARIANCE, m_lBordersVariance);
	DDX_Text(pDX, IDC_SMOOTH_LENGTH, m_lSmoothLength);
	DDX_Check(pDX, IDC_SMOOTH_LONG, m_bSmoothLong);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CCleanup::EnableBorderRemoval()
//
// 	Description:	This function enables/disables the controls used to define
//					the border removal parameters.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCleanup::EnableBorderRemoval(BOOL bEnable)
{
	m_ctrlBordersGroup.EnableWindow(bEnable);
	m_ctrlBordersPercent.EnableWindow(bEnable);
	m_ctrlBordersNoise.EnableWindow(bEnable);
	m_ctrlBordersVariance.EnableWindow(bEnable);
	m_ctrlBordersPercentLabel.EnableWindow(bEnable);
	m_ctrlBordersNoiseLabel.EnableWindow(bEnable);
	m_ctrlBordersVarianceLabel.EnableWindow(bEnable);
	m_ctrlBordersTop.EnableWindow(bEnable);
	m_ctrlBordersBottom.EnableWindow(bEnable);
	m_ctrlBordersLeft.EnableWindow(bEnable);
	m_ctrlBordersRight.EnableWindow(bEnable);
}

//==============================================================================
//
// 	Function Name:	CCleanup::EnableDeskew()
//
// 	Description:	This function enables/disables the controls used to define
//					the deskew parameters.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCleanup::EnableDeskew(BOOL bEnable)
{
	m_ctrlDeskewGroup.EnableWindow(bEnable);
	m_ctrlDeskewColor.EnableWindow(bEnable);
	m_ctrlDeskewColorLabel.EnableWindow(bEnable);
}

//==============================================================================
//
// 	Function Name:	CCleanup::EnableDotRemoval()
//
// 	Description:	This function enables/disables the controls used to define
//					the dot removal parameters.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCleanup::EnableDotRemoval(BOOL bEnable)
{
	m_ctrlDotsGroup.EnableWindow(bEnable);
	m_ctrlDotsMinHeight.EnableWindow(bEnable);
	m_ctrlDotsMinWidth.EnableWindow(bEnable);
	m_ctrlDotsMaxHeight.EnableWindow(bEnable);
	m_ctrlDotsMaxWidth.EnableWindow(bEnable);
	m_ctrlDotsMinHeightLabel.EnableWindow(bEnable);
	m_ctrlDotsMinWidthLabel.EnableWindow(bEnable);
	m_ctrlDotsMaxHeightLabel.EnableWindow(bEnable);
	m_ctrlDotsMaxWidthLabel.EnableWindow(bEnable);
}

//==============================================================================
//
// 	Function Name:	CCleanup::EnableHolePunchRemoval()
//
// 	Description:	This function enables/disables the controls used to define
//					the hole punch removal parameters.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCleanup::EnableHolePunchRemoval(BOOL bEnable)
{
	m_ctrlHolesGroup.EnableWindow(bEnable);
	m_ctrlHolesMinHeight.EnableWindow(bEnable);
	m_ctrlHolesMinWidth.EnableWindow(bEnable);
	m_ctrlHolesMaxHeight.EnableWindow(bEnable);
	m_ctrlHolesMaxWidth.EnableWindow(bEnable);
	m_ctrlHolesMinHeightLabel.EnableWindow(bEnable);
	m_ctrlHolesMinWidthLabel.EnableWindow(bEnable);
	m_ctrlHolesMaxHeightLabel.EnableWindow(bEnable);
	m_ctrlHolesMaxWidthLabel.EnableWindow(bEnable);
	m_ctrlHolesTop.EnableWindow(bEnable);
	m_ctrlHolesBottom.EnableWindow(bEnable);
	m_ctrlHolesLeft.EnableWindow(bEnable);
	m_ctrlHolesRight.EnableWindow(bEnable);
}

//==============================================================================
//
// 	Function Name:	CCleanup::EnableSmooth()
//
// 	Description:	This function enables/disables the controls used to define
//					the Smooth parameters.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCleanup::EnableSmooth(BOOL bEnable)
{
	m_ctrlSmoothGroup.EnableWindow(bEnable);
	m_ctrlSmoothLengthLabel.EnableWindow(bEnable);
	m_ctrlSmoothLength.EnableWindow(bEnable);
	m_ctrlSmoothLong.EnableWindow(bEnable);
}

//==============================================================================
//
// 	Function Name:	CCleanup::OnClean()
//
// 	Description:	This function is called when the user clicks on Clean
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCleanup::OnClean() 
{
	long lLocation;

	ASSERT(m_pTMLead);

	//	Copy the current image to the undo buffer
	if(m_pTMLead->GetBitmap() != 0)
	{
		m_Undo.SetBitmap(m_pTMLead->GetBitmap());
		m_ctrlUndo.EnableWindow(TRUE);
		m_ctrlSave.EnableWindow(TRUE);
	}

	theApp.DoWaitCursor(1);

	//	Should we deskew?
	if(m_bDespeckle)
		m_pTMLead->Deskew();

	//	Should we despeckle?
	if(m_bDespeckle)
		m_pTMLead->Despeckle();

	//	Should we smooth
	if(m_bSmooth)
		m_pTMLead->Smooth(m_lSmoothLength, (int)m_bSmoothLong);

	//	Should we remove borders
	if(m_bRemoveBorders)
	{
		lLocation = 0;
		if(m_bBordersLeft)
			lLocation |= BORDER_LEFT;
		if(m_bBordersRight)
			lLocation |= BORDER_RIGHT;
		if(m_bBordersTop)
			lLocation |= BORDER_TOP;
		if(m_bBordersBottom)
			lLocation |= BORDER_BOTTOM;

		m_pTMLead->BorderRemove(m_lBordersPercent, m_lBordersNoise,
								m_lBordersNoise, lLocation);
	}

	//	Should we remove hole punches
	if(m_bRemoveHoles)
	{
		lLocation = 0;
		if(m_bHolesLeft)
			lLocation |= HOLEPUNCH_LEFT;
		if(m_bHolesRight)
			lLocation |= HOLEPUNCH_RIGHT;
		if(m_bHolesTop)
			lLocation |= HOLEPUNCH_TOP;
		if(m_bHolesBottom)
			lLocation |= HOLEPUNCH_BOTTOM;

		m_pTMLead->HolePunchRemove(m_lHolesMinWidth, m_lHolesMinHeight,
								   m_lHolesMaxWidth, m_lHolesMaxHeight,
								   lLocation);
	}

	//	Should we remove dots
	if(m_bRemoveDots)
	{
		m_pTMLead->DotRemove(m_lDotsMinWidth, m_lDotsMinHeight,
							 m_lDotsMaxWidth, m_lDotsMaxHeight);
	}

	theApp.DoWaitCursor(-1);
}

//==============================================================================
//
// 	Function Name:	CCleanup::OnDeskew()
//
// 	Description:	This function is called when the user clicks on the
//					Deskew option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCleanup::OnDeskew() 
{
	UpdateData(TRUE);
	EnableDeskew(m_bDeskew);
}

//==============================================================================
//
// 	Function Name:	CCleanup::OnDeskewColor()
//
// 	Description:	This function is called when the user clicks on Deskew Color
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCleanup::OnDeskewColor() 
{
	COLORREF crColor;

	//	Get the current deskew background color
	crColor = m_pTMLead->GetDeskewBackColor();

	CColorDialog Dialog(crColor, CC_ANYCOLOR, this);

	if(Dialog.DoModal() == IDOK)
	{
		crColor = Dialog.GetColor();
		m_ctrlDeskewColor.SetColor(crColor);
		m_ctrlDeskewColor.RedrawWindow();
		m_pTMLead->SetDeskewBackColor(crColor);
	}
}

//==============================================================================
//
// 	Function Name:	CCleanup::OnInitDialog()
//
// 	Description:	This function traps the WM_INITDIALOG message
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CCleanup::OnInitDialog() 
{
	//	Perform the base class initialization first
	CDialog::OnInitDialog();
	
	//	Set the correct color for the deskew background color button
	m_ctrlDeskewColor.SetColor(m_pTMLead->GetDeskewBackColor());

	//	Enable disable the controls
	EnableSmooth(m_bSmooth);
	EnableHolePunchRemoval(m_bRemoveHoles);
	EnableDotRemoval(m_bRemoveDots);
	EnableBorderRemoval(m_bRemoveBorders);
	EnableDeskew(m_bDeskew);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CCleanup::OnRemoveBorders()
//
// 	Description:	This function is called when the user clicks on the
//					Remove Borders option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCleanup::OnRemoveBorders() 
{
	UpdateData(TRUE);
	EnableBorderRemoval(m_bRemoveBorders);
}

//==============================================================================
//
// 	Function Name:	CCleanup::OnRemoveDots()
//
// 	Description:	This function is called when the user clicks on the
//					Remove Dots option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCleanup::OnRemoveDots() 
{
	UpdateData(TRUE);
	EnableDotRemoval(m_bRemoveDots);
}

//==============================================================================
//
// 	Function Name:	CCleanup::OnRemoveHoles()
//
// 	Description:	This function is called when the user clicks on the
//					Remove Hole Punches option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCleanup::OnRemoveHoles() 
{
	UpdateData(TRUE);
	EnableHolePunchRemoval(m_bRemoveHoles);
}

//==============================================================================
//
// 	Function Name:	CCleanup::OnSave()
//
// 	Description:	This function is called when the user clicks on Save
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCleanup::OnSave() 
{
	ASSERT(m_pTMLead);

	//	Enable error messages while we save the file
	m_pTMLead->SetEnableMethodErrors(TRUE);
	
	if(m_pTMLead->Save(0) == 0)
	{
		//	Clear the undo buffer
		m_Undo.SetBitmap(0);
		m_ctrlUndo.EnableWindow(FALSE);
		m_ctrlSave.EnableWindow(FALSE);
	}

	m_pTMLead->SetEnableMethodErrors(FALSE);
}

//==============================================================================
//
// 	Function Name:	CCleanup::OnSmooth()
//
// 	Description:	This function is called when the user clicks on the
//					Smooth option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCleanup::OnSmooth() 
{
	UpdateData(TRUE);
	EnableSmooth(m_bSmooth);
}

//==============================================================================
//
// 	Function Name:	CCleanup::OnUndo()
//
// 	Description:	This function is called when the user clicks on Undo
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCleanup::OnUndo() 
{
	ASSERT(m_pTMLead);

	//	Restore the previous image
	if(m_Undo.GetBitmap() != 0)
	{
		m_pTMLead->SetBitmap(m_Undo.GetBitmap());
		m_Undo.SetBitmap(0);
		m_ctrlUndo.EnableWindow(FALSE);
	}
}




