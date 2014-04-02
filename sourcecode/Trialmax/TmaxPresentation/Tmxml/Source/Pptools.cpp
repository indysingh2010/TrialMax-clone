//==============================================================================
//
// File Name:	pptools.cpp
//
// Description:	This file contains member functions of the CPPTools class
//
// See Also:	pptools.h
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	08-01-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <pptools.h>
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
IMPLEMENT_DYNCREATE(CPPTools, CPropertyPage)

BEGIN_MESSAGE_MAP(CPPTools, CPropertyPage)
	//{{AFX_MSG_MAP(CPPTools)
	ON_BN_CLICKED(IDC_RED, OnRed)
	ON_BN_CLICKED(IDC_BLACK, OnBlack)
	ON_BN_CLICKED(IDC_BLUE, OnBlue)
	ON_BN_CLICKED(IDC_CYAN, OnCyan)
	ON_BN_CLICKED(IDC_GREEN, OnGreen)
	ON_BN_CLICKED(IDC_GREY, OnGrey)
	ON_BN_CLICKED(IDC_MAGENTA, OnMagenta)
	ON_BN_CLICKED(IDC_WHITE, OnWhite)
	ON_BN_CLICKED(IDC_YELLOW, OnYellow)
	ON_BN_CLICKED(IDC_DARKBLUE, OnDarkBlue)
	ON_BN_CLICKED(IDC_DARKGREEN, OnDarkGreen)
	ON_BN_CLICKED(IDC_DARKRED, OnDarkRed)
	ON_BN_CLICKED(IDC_LIGHTBLUE, OnLightBlue)
	ON_BN_CLICKED(IDC_LIGHTGREEN, OnLightGreen)
	ON_BN_CLICKED(IDC_LIGHTRED, OnLightRed)
	ON_BN_CLICKED(IDC_HIGHLIGHT, OnHighlight)
	ON_BN_CLICKED(IDC_REDACT, OnRedact)
	ON_BN_CLICKED(IDC_DRAWTOOL, OnDrawtool)
	ON_BN_CLICKED(IDC_CALLOUT, OnCallout)
	ON_BN_CLICKED(IDC_CALLOUTFRAME, OnCalloutFrame)
	ON_BN_CLICKED(IDC_CALLOUTHANDLE, OnCalloutHandle)
	ON_BN_CLICKED(IDC_CHANGEFONT, OnChangeFont)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CPPTools::CPPTools()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPTools::CPPTools() : CPropertyPage(CPPTools::IDD)
{
	//{{AFX_DATA_INIT(CPPTools)
	m_iThickness = 1;
	m_iZoomFactor = 5;
	m_iColorSelect = 0;
	m_iCallFrameThick = 5;
	m_iBitonal = 0;
	m_iTool = 0;
	m_strFontName = _T("");
	m_bResizableCallouts = FALSE;
	//}}AFX_DATA_INIT

	m_iColorTool = 0;
}

//==============================================================================
//
// 	Function Name:	CPPTools::~CPPTools()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPTools::~CPPTools()
{
}

//==============================================================================
//
// 	Function Name:	CPPTools::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					dialog box controls and class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::DoDataExchange(CDataExchange* pDX)
{
	CPropertyPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPPTools)
	DDX_Control(pDX, IDC_FONTNAME, m_ctrlFontName);
	DDX_Control(pDX, IDC_DRAWTOOL, m_ctrlDrawTool);
	DDX_Control(pDX, IDC_CALLFRAMETHICKSPIN, m_SpinCallThick);
	DDX_Control(pDX, IDC_SPINZOOM, m_SpinZoom);
	DDX_Control(pDX, IDC_SPINTHICK, m_SpinThick);
	DDX_Text(pDX, IDC_THICKNESS, m_iThickness);
	DDX_Text(pDX, IDC_ZOOMFACTOR, m_iZoomFactor);
	DDX_Radio(pDX, IDC_DRAWTOOL, m_iColorSelect);
	DDX_Text(pDX, IDC_CALLFRAMETHICK, m_iCallFrameThick);
	DDX_CBIndex(pDX, IDC_BITONAL, m_iBitonal);
	DDX_CBIndex(pDX, IDC_ANNTOOL, m_iTool);
	DDX_Text(pDX, IDC_FONTNAME, m_strFontName);
	DDX_Check(pDX, IDC_RESIZABLECALLOUTS, m_bResizableCallouts);
	//}}AFX_DATA_MAP
	DDX_Control(pDX, IDC_HIGHLIGHT, m_ctrlHighlight);
	DDX_Control(pDX, IDC_REDACT, m_ctrlRedact);
	DDX_Control(pDX, IDC_CALLOUT, m_ctrlCallout);
	DDX_Control(pDX, IDC_CALLOUTFRAME, m_ctrlCalloutFrame);
	DDX_Control(pDX, IDC_CALLOUTHANDLE, m_ctrlCalloutHandle);
}

//==============================================================================
//
// 	Function Name:	CPPTools::GetOptions()
//
// 	Description:	This function is called to get the current options
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::GetOptions(SGraphicsOptions* pOptions)
{
	ASSERT(pOptions);

	//	Update the caller's structure
	pOptions->bResizableCallouts = m_bResizableCallouts;
	pOptions->sAnnColor = m_iDrawColor;
	pOptions->sHighlightColor = m_iHighlightColor;
	pOptions->sRedactColor = m_iRedactColor;
	pOptions->sCalloutColor = m_iCalloutColor;
	pOptions->sCalloutFrameColor = m_iCallFrameColor;
	pOptions->sCalloutHandleColor = m_iCallHandleColor;
	pOptions->sAnnThickness = m_iThickness;
	pOptions->sCalloutFrameThickness = m_iCallFrameThick;
	pOptions->sBitonalScaling = m_iBitonal;
	pOptions->sAnnTool = m_iTool;
	pOptions->sMaxZoom = m_iZoomFactor;
	pOptions->sAnnFontSize = m_sFontSize;
	pOptions->bAnnFontBold = m_bFontBold;
	pOptions->bAnnFontStrikeThrough = m_bFontStrikeThrough;
	pOptions->bAnnFontUnderline = m_bFontUnderline;
	pOptions->strAnnFontName = m_strFontName;
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnBlack()
//
// 	Description:	This function is called when the user clicks on the Black
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnBlack() 
{
	SetColor(TMV_BLACK);
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnBlue()
//
// 	Description:	This function is called when the user clicks on the Blue
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnBlue() 
{
	SetColor(TMV_BLUE);
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnCallout()
//
// 	Description:	This function is called when the user clicks on the 
//					Callout color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnCallout() 
{
	m_iColorSelect = CALLOUTCOLOR;
	SelectColor(m_iCalloutColor);
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnCalloutFrame()
//
// 	Description:	This function is called when the user clicks on the 
//					Callout Frame color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnCalloutFrame() 
{
	m_iColorSelect = CALLFRAMECOLOR;
	SelectColor(m_iCallFrameColor);
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnCalloutHandle()
//
// 	Description:	This function is called when the user clicks on the 
//					Callout Handle color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnCalloutHandle() 
{
	m_iColorSelect = CALLHANDLECOLOR;
	SelectColor(m_iCallHandleColor);
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnChangeFont()
//
// 	Description:	This function is called when the user clicks the button to
//					set the annotation text font.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnChangeFont() 
{
	LOGFONT	lfFont;
	CDC*	pdc = GetDC();
	int		iHeight = 0;

	//	Translate point size to logical height
	if(pdc)
		iHeight = -MulDiv(m_sFontSize, pdc->GetDeviceCaps(LOGPIXELSY), 72);

	ZeroMemory(&lfFont, sizeof(lfFont));
	lfFont.lfUnderline = m_bFontUnderline;
	lfFont.lfStrikeOut = m_bFontStrikeThrough;
	lfFont.lfHeight = iHeight;
	lfFont.lfWeight = (m_bFontBold) ? FW_BOLD : FW_NORMAL;
	lstrcpy(lfFont.lfFaceName, m_strFontName);

	CFontDialog	Dialog(&lfFont, CF_SCREENFONTS | CF_EFFECTS);
	if(Dialog.DoModal() == IDOK)
	{
		m_bFontUnderline = Dialog.IsUnderline();
		m_bFontStrikeThrough = Dialog.IsStrikeOut();
		m_bFontBold = Dialog.IsBold();
		m_strFontName = Dialog.GetFaceName();
		m_sFontSize = (short)(Dialog.GetSize() / 10);

		//	Update the font name
		m_ctrlFontName.SetWindowText(m_strFontName);
	}
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnCyan()
//
// 	Description:	This function is called when the user clicks on the Cyan
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnCyan() 
{
	SetColor(TMV_CYAN);
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnDarkBlue()
//
// 	Description:	This function is called when the user clicks on the Dark
//					Blue button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnDarkBlue() 
{
	SetColor(TMV_DARKBLUE);
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnDarkGreen()
//
// 	Description:	This function is called when the user clicks on the Dark
//					Green button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnDarkGreen() 
{
	SetColor(TMV_DARKGREEN);
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnDarkRed()
//
// 	Description:	This function is called when the user clicks on the Dark
//					Red button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnDarkRed() 
{
	SetColor(TMV_DARKRED);
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnDrawTool()
//
// 	Description:	This function is called when the user clicks on the 
//					Draw color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnDrawtool() 
{
	m_iColorSelect = DRAWCOLOR;
	SelectColor(m_iDrawColor);
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnGreen()
//
// 	Description:	This function is called when the user clicks on the Green
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnGreen() 
{
	SetColor(TMV_GREEN);
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnGrey()
//
// 	Description:	This function is called when the user clicks on the Grey
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnGrey() 
{
	SetColor(TMV_GREY);
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnHighlight()
//
// 	Description:	This function is called when the user clicks on the 
//					Highlight color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnHighlight() 
{
	m_iColorSelect = HIGHLIGHTCOLOR;
	SelectColor(m_iHighlightColor);
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnInitDialog()
//
// 	Description:	This function handles all WM_INITDIALOG messages
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CPPTools::OnInitDialog() 
{
	//	Do the base class processing first
	CPropertyPage::OnInitDialog();

	//	Load the bitmaps for the color buttons
	m_btnRed.AutoLoad(IDC_RED, this);
	m_btnBlue.AutoLoad(IDC_BLUE, this);
	m_btnGreen.AutoLoad(IDC_GREEN, this);
	m_btnMagenta.AutoLoad(IDC_MAGENTA, this);
	m_btnCyan.AutoLoad(IDC_CYAN, this);
	m_btnYellow.AutoLoad(IDC_YELLOW, this);
	m_btnBlack.AutoLoad(IDC_BLACK, this);
	m_btnGrey.AutoLoad(IDC_GREY, this);
	m_btnWhite.AutoLoad(IDC_WHITE, this);
	m_btnDarkRed.AutoLoad(IDC_DARKRED, this);
	m_btnDarkBlue.AutoLoad(IDC_DARKBLUE, this);
	m_btnDarkGreen.AutoLoad(IDC_DARKGREEN, this);
	m_btnLightRed.AutoLoad(IDC_LIGHTRED, this);
	m_btnLightBlue.AutoLoad(IDC_LIGHTBLUE, this);
	m_btnLightGreen.AutoLoad(IDC_LIGHTGREEN, this);

	m_SpinThick.SetRange(1, 15);
	m_SpinZoom.SetRange(1, 100);
	m_SpinThick.SetRange(1, 25);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnLightBlue()
//
// 	Description:	This function is called when the user clicks on the Light
//					Blue button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnLightBlue() 
{
	SetColor(TMV_LIGHTBLUE);
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnLightGreen()
//
// 	Description:	This function is called when the user clicks on the Light
//					Green button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnLightGreen() 
{
	SetColor(TMV_LIGHTGREEN);
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnLightRed()
//
// 	Description:	This function is called when the user clicks on the Light
//					Red button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnLightRed() 
{
	SetColor(TMV_LIGHTRED);
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnMagenta()
//
// 	Description:	This function is called when the user clicks on the Magenta
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnMagenta() 
{
	SetColor(TMV_MAGENTA);
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnRed()
//
// 	Description:	This function is called when the user clicks on the Red
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnRed() 
{
	SetColor(TMV_RED);
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnRedact()
//
// 	Description:	This function is called when the user clicks on the 
//					Redact color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnRedact() 
{
	m_iColorSelect = REDACTCOLOR;
	SelectColor(m_iRedactColor);

}

//==============================================================================
//
// 	Function Name:	CPPTools::OnWhite()
//
// 	Description:	This function is called when the user clicks on the White
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnWhite() 
{
	SetColor(TMV_WHITE);
}

//==============================================================================
//
// 	Function Name:	CPPTools::OnYellow()
//
// 	Description:	This function is called when the user clicks on the Yellow
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::OnYellow() 
{
	SetColor(TMV_YELLOW);
}

//==============================================================================
//
// 	Function Name:	CPPTools::SelectColor()
//
// 	Description:	This function is called when the user selects a new color
//					option. It selects the color button currently associated
//					with the new option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::SelectColor(int iColor)
{
	switch(iColor)
	{
		case TMV_RED:			m_btnRed.SetFocus();
								return;
		case TMV_BLUE:			m_btnBlue.SetFocus();
								return;
		case TMV_GREEN:			m_btnGreen.SetFocus();
								return;
		case TMV_MAGENTA:		m_btnMagenta.SetFocus();
								return;
		case TMV_CYAN:			m_btnCyan.SetFocus();
								return;
		case TMV_YELLOW:		m_btnYellow.SetFocus();
								return;
		case TMV_BLACK:			m_btnBlack.SetFocus();
								return;
		case TMV_GREY:			m_btnGrey.SetFocus();
								return;
		case TMV_WHITE:			m_btnWhite.SetFocus();
								return;
		case TMV_DARKRED:		m_btnDarkRed.SetFocus();
								return;
		case TMV_DARKBLUE:		m_btnDarkBlue.SetFocus();
								return;
		case TMV_DARKGREEN:		m_btnDarkGreen.SetFocus();
								return;
		case TMV_LIGHTRED:		m_btnLightRed.SetFocus();
								return;
		case TMV_LIGHTBLUE:		m_btnLightBlue.SetFocus();
								return;
		case TMV_LIGHTGREEN:	m_btnLightGreen.SetFocus();
								return;
		default:				return;
	
	}
}

//==============================================================================
//
// 	Function Name:	CPPTools::SetColor()
//
// 	Description:	This function is called selects a new color. It sets the
//					color for the currently selected option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::SetColor(int iColor)
{
	switch(m_iColorSelect)
	{
		case DRAWCOLOR:			m_iDrawColor = iColor;
								return;
		case HIGHLIGHTCOLOR:	m_iHighlightColor = iColor;
								return;
		case REDACTCOLOR:		m_iRedactColor = iColor;
								return;
		case CALLOUTCOLOR:		m_iCalloutColor = iColor;
								return;
		case CALLFRAMECOLOR:	m_iCallFrameColor = iColor;
								return;
		case CALLHANDLECOLOR:	m_iCallHandleColor = iColor;
								return;
		default:				return;
	}
}

//==============================================================================
//
// 	Function Name:	CPPTools::SetOptions()
//
// 	Description:	This function is called to set the current options
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPTools::SetOptions(SGraphicsOptions* pOptions)
{
	ASSERT(pOptions);

	//	Update the class members
	m_bResizableCallouts = pOptions->bResizableCallouts;
	m_iDrawColor = pOptions->sAnnColor;
	m_iHighlightColor = pOptions->sHighlightColor;
	m_iRedactColor = pOptions->sRedactColor;
	m_iCalloutColor = pOptions->sCalloutColor;
	m_iCallFrameColor = pOptions->sCalloutFrameColor;
	m_iCallHandleColor = pOptions->sCalloutHandleColor;
	m_iThickness = pOptions->sAnnThickness;
	m_iCallFrameThick = pOptions->sCalloutFrameThickness;
	m_iBitonal = pOptions->sBitonalScaling;
	m_iTool = pOptions->sAnnTool;
	m_iZoomFactor = pOptions->sMaxZoom;
	m_sFontSize = pOptions->sAnnFontSize;
	m_bFontBold = pOptions->bAnnFontBold;
	m_bFontStrikeThrough = pOptions->bAnnFontStrikeThrough;
	m_bFontUnderline = pOptions->bAnnFontUnderline;
	m_strFontName = pOptions->strAnnFontName;
}


