//==============================================================================
//
// File Name:	graphpg.cpp
//
// Description:	This file contains member functions of the CGraphicsPage class.
//
// See Also:	graphpg.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-29-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmsetap.h>
#include <graphpg.h>

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
BEGIN_MESSAGE_MAP(CGraphicsPage, CSetupPage)
	//{{AFX_MSG_MAP(CGraphicsPage)
	ON_BN_CLICKED(IDC_RED, OnRed)
	ON_BN_CLICKED(IDC_BLACK, OnBlack)
	ON_BN_CLICKED(IDC_BLUE, OnBlue)
	ON_BN_CLICKED(IDC_CYAN, OnCyan)
	ON_BN_CLICKED(IDC_GREEN, OnGreen)
	ON_BN_CLICKED(IDC_GREY, OnGrey)
	ON_BN_CLICKED(IDC_MAGENTA, OnMagenta)
	ON_BN_CLICKED(IDC_WHITE, OnWhite)
	ON_BN_CLICKED(IDC_YELLOW, OnYellow)
	ON_BN_CLICKED(IDC_HIGHLIGHT, OnHighlight)
	ON_BN_CLICKED(IDC_REDACT, OnRedact)
	ON_BN_CLICKED(IDC_DRAWTOOL, OnDrawtool)
	ON_BN_CLICKED(IDC_CALLOUT, OnCallout)
	ON_BN_CLICKED(IDC_CALLOUTFRAME, OnCalloutFrame)
	ON_BN_CLICKED(IDC_CALLOUTHANDLES, OnCalloutHandle)
	ON_BN_CLICKED(IDC_LIGHTPENRECT, OnLightPenRect)
	ON_BN_CLICKED(IDC_DARKBLUE, OnDarkBlue)
	ON_BN_CLICKED(IDC_DARKGREEN, OnDarkGreen)
	ON_BN_CLICKED(IDC_DARKRED, OnDarkRed)
	ON_BN_CLICKED(IDC_LIGHTBLUE, OnLightBlue)
	ON_BN_CLICKED(IDC_LIGHTGREEN, OnLightGreen)
	ON_BN_CLICKED(IDC_LIGHTRED, OnLightRed)
	ON_BN_CLICKED(IDC_CHANGEFONT, OnChangeFont)
	ON_BN_CLICKED(IDC_USERSPLITFRAMECOLOR, OnUserSplitFrame)
	ON_BN_CLICKED(IDC_ZAPSPLITFRAMECOLOR, OnZapSplitFrame)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CGraphicsPage::CGraphicsPage()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CGraphicsPage::CGraphicsPage(CWnd* pParent) : CSetupPage(CGraphicsPage::IDD, pParent)
{
	//{{AFX_DATA_INIT(CGraphicsPage)
	m_nThickness = GRAPHICS_ANNTHICKNESS;
	m_nZoomFactor = GRAPHICS_MAXZOOM;
	m_nColorSelect = DRAWCOLOR;
	m_bScaleGraphics = GRAPHICS_SCALEGRAPHICS;
	m_bScaleDocs = GRAPHICS_SCALEDOCUMENTS;
	m_nLightPenSize = GRAPHICS_PENSELECTORSIZE;
	m_nCallFrameThick = GRAPHICS_CALLFRAMETHICKNESS;
	m_iBitonal = -1;
	m_bLightPenControls = FALSE;
	m_nTool = -1;
	m_strFontName = _T("");
	m_bResizableCallouts = FALSE;
	m_bShadeOnCallout = FALSE;
	m_sCalloutShadeGrayscale = 0;
	m_bPanCallouts = FALSE;
	//}}AFX_DATA_INIT

	//	Set the default colors
	m_nDrawColor		   = GRAPHICS_ANNCOLOR;
	m_nHighlightColor	   = GRAPHICS_HIGHLIGHTCOLOR;
	m_nRedactColor		   = GRAPHICS_REDACTCOLOR;
	m_nCalloutColor		   = GRAPHICS_CALLOUTCOLOR;
	m_nCallFrameColor	   = GRAPHICS_CALLFRAMECOLOR;
	m_nCallHandleColor	   = GRAPHICS_CALLHANDLECOLOR;
	m_nLightPenColor       = GRAPHICS_PENSELECTORCOLOR;
	m_nUserSplitFrameColor = GRAPHICS_USER_SPLITFRAMECOLOR;
	m_nZapSplitFrameColor  = GRAPHICS_ZAP_SPLITFRAMECOLOR;
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the class
//					members and the associated dialog box controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::DoDataExchange(CDataExchange* pDX)
{
	CSetupPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CGraphicsPage)
	DDX_Control(pDX, IDC_FONTNAME, m_ctrlFontName);
	DDX_Control(pDX, IDC_DRAWTOOL, m_ctrlDrawTool);
	DDX_Control(pDX, IDC_LIGHTPENSIZESPIN, m_SpinLightPenSize);
	DDX_Control(pDX, IDC_CALLFRAMETHICKSPIN, m_SpinCallThick);
	DDX_Control(pDX, IDC_SPINZOOM, m_SpinZoom);
	DDX_Control(pDX, IDC_SPINTHICK, m_SpinThick);
	DDX_Text(pDX, IDC_THICKNESS, m_nThickness);
	DDX_Text(pDX, IDC_ZOOMFACTOR, m_nZoomFactor);
	DDX_Radio(pDX, IDC_DRAWTOOL, m_nColorSelect);
	DDX_Check(pDX, IDC_SCALEGRAPHICS, m_bScaleGraphics);
	DDX_Check(pDX, IDC_SCALEDOCS, m_bScaleDocs);
	DDX_Text(pDX, IDC_LIGHTPENSIZE, m_nLightPenSize);
	DDX_Text(pDX, IDC_CALLFRAMETHICK, m_nCallFrameThick);
	DDX_CBIndex(pDX, IDC_BITONAL, m_iBitonal);
	DDX_Check(pDX, IDC_LIGHTPENCONTROLS, m_bLightPenControls);
	DDX_CBIndex(pDX, IDC_ANNTOOL, m_nTool);
	DDX_Text(pDX, IDC_FONTNAME, m_strFontName);
	DDX_Check(pDX, IDC_RESIZABLE_CALLOUTS, m_bResizableCallouts);
	DDX_Check(pDX, IDC_SHADE_CALLOUTS, m_bShadeOnCallout);
	DDX_Text(pDX, IDC_SHADE_GRAYSCALE, m_sCalloutShadeGrayscale);
	DDV_MinMaxInt(pDX, m_sCalloutShadeGrayscale, 0, 255);
	DDX_Check(pDX, IDC_PAN_CALLOUTS, m_bPanCallouts);
	//}}AFX_DATA_MAP
	DDX_Control(pDX, IDC_HIGHLIGHT, m_ctrlHighlight);
	DDX_Control(pDX, IDC_REDACT, m_ctrlRedact);
	DDX_Control(pDX, IDC_CALLOUT, m_ctrlCallout);
	DDX_Control(pDX, IDC_CALLOUTFRAME, m_ctrlCalloutFrame);
	DDX_Control(pDX, IDC_CALLOUTHANDLES, m_ctrlCalloutHandle);
	DDX_Control(pDX, IDC_LIGHTPENRECT, m_ctrlLightPenRect);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnBlack()
//
// 	Description:	This function is called when the user clicks on the Black
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnBlack() 
{
	SetColor(TMV_BLACK);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnBlue()
//
// 	Description:	This function is called when the user clicks on the Blue
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnBlue() 
{
	SetColor(TMV_BLUE);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnCallout()
//
// 	Description:	This function is called when the user clicks on the 
//					Callout color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnCallout() 
{
	m_nColorSelect = CALLOUTCOLOR;
	SelectColor(m_nCalloutColor);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnCalloutFrame()
//
// 	Description:	This function is called when the user clicks on the 
//					Callout Frame color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnCalloutFrame() 
{
	m_nColorSelect = CALLFRAMECOLOR;
	SelectColor(m_nCallFrameColor);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnCalloutHandle()
//
// 	Description:	This function is called when the user clicks on the 
//					Callout Handle color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnCalloutHandle() 
{
	m_nColorSelect = CALLHANDLECOLOR;
	SelectColor(m_nCallHandleColor);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnChangeFont()
//
// 	Description:	This function is called when the user clicks the button to
//					set the annotation text font.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnChangeFont() 
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
// 	Function Name:	CGraphicsPage::OnCyan()
//
// 	Description:	This function is called when the user clicks on the Cyan
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnCyan() 
{
	SetColor(TMV_CYAN);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnDarkBlue()
//
// 	Description:	This function is called when the user clicks on the Dark
//					Blue button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnDarkBlue() 
{
	SetColor(TMV_DARKBLUE);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnDarkGreen()
//
// 	Description:	This function is called when the user clicks on the Dark
//					Green button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnDarkGreen() 
{
	SetColor(TMV_DARKGREEN);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnDarkRed()
//
// 	Description:	This function is called when the user clicks on the Dark
//					Red button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnDarkRed() 
{
	SetColor(TMV_DARKRED);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnDrawTool()
//
// 	Description:	This function is called when the user clicks on the 
//					Draw color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnDrawtool() 
{
	m_nColorSelect = DRAWCOLOR;
	SelectColor(m_nDrawColor);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnGreen()
//
// 	Description:	This function is called when the user clicks on the Green
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnGreen() 
{
	SetColor(TMV_GREEN);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnGrey()
//
// 	Description:	This function is called when the user clicks on the Grey
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnGrey() 
{
	SetColor(TMV_GREY);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnHighlight()
//
// 	Description:	This function is called when the user clicks on the 
//					Highlight color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnHighlight() 
{
	m_nColorSelect = HIGHLIGHTCOLOR;
	SelectColor(m_nHighlightColor);

}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnInitDialog()
//
// 	Description:	This function is called by the framework to initialize the
//					dialog box.
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CGraphicsPage::OnInitDialog() 
{
	CSetupPage::OnInitDialog();
	
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

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnLightBlue()
//
// 	Description:	This function is called when the user clicks on the Light
//					Blue button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnLightBlue() 
{
	SetColor(TMV_LIGHTBLUE);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnLightGreen()
//
// 	Description:	This function is called when the user clicks on the Light
//					Green button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnLightGreen() 
{
	SetColor(TMV_LIGHTGREEN);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnLightPenRect()
//
// 	Description:	This function is called when the user clicks on the 
//					Light Pen Rectangle color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnLightPenRect() 
{
	m_nColorSelect = LIGHTPENCOLOR;
	SelectColor(m_nLightPenColor);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnLightRed()
//
// 	Description:	This function is called when the user clicks on the Light
//					Red button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnLightRed() 
{
	SetColor(TMV_LIGHTRED);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnMagenta()
//
// 	Description:	This function is called when the user clicks on the Magenta
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnMagenta() 
{
	SetColor(TMV_MAGENTA);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnRed()
//
// 	Description:	This function is called when the user clicks on the Red
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnRed() 
{
	SetColor(TMV_RED);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnRedact()
//
// 	Description:	This function is called when the user clicks on the 
//					Redact color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnRedact() 
{
	m_nColorSelect = REDACTCOLOR;
	SelectColor(m_nRedactColor);

}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnUserSplitFrame()
//
// 	Description:	This function is called when the user clicks on the 
//					UserSplitFrame color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnUserSplitFrame() 
{
	m_nColorSelect = USERSPLITFRAMECOLOR;
	SelectColor(m_nUserSplitFrameColor);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnWhite()
//
// 	Description:	This function is called when the user clicks on the White
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnWhite() 
{
	SetColor(TMV_WHITE);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnYellow()
//
// 	Description:	This function is called when the user clicks on the Yellow
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnYellow() 
{
	SetColor(TMV_YELLOW);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::OnZapSplitFrame()
//
// 	Description:	This function is called when the user clicks on the 
//					ZapSplitFrame color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::OnZapSplitFrame() 
{
	m_nColorSelect = ZAPSPLITFRAMECOLOR;
	SelectColor(m_nZapSplitFrameColor);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::ReadOptions()
//
// 	Description:	This function is called to read the page options from the
//					ini file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::ReadOptions(CTMIni& rIni)
{
	SGraphicsOptions Options;

	//	Read the options from the ini file
	rIni.ReadGraphicsOptions(&Options);

	//	Set the class members
	m_nDrawColor = Options.sAnnColor;
	m_nThickness = Options.sAnnThickness;
	m_nHighlightColor = Options.sHighlightColor;
	m_nRedactColor = Options.sRedactColor;
	m_nZoomFactor = Options.sMaxZoom;
	m_nCalloutColor = Options.sCalloutColor;
	m_nCallHandleColor = Options.sCalloutHandleColor;
	m_nCallFrameColor = Options.sCalloutFrameColor;
	m_nCallFrameThick = Options.sCalloutFrameThickness;
	m_iBitonal = Options.sBitonalScaling;
	m_nTool = Options.sAnnTool;
	m_sFontSize = Options.sAnnFontSize;
	m_nLightPenColor = Options.sLightPenColor;
	m_nLightPenSize = Options.sLightPenSize;
	m_nUserSplitFrameColor = Options.sUserSplitFrameColor;
	m_nZapSplitFrameColor = Options.sZapSplitFrameColor;
	m_bLightPenControls = Options.bLightPenEnabled;
	m_bFontBold = Options.bAnnFontBold;
	m_bFontStrikeThrough = Options.bAnnFontStrikeThrough;
	m_bFontUnderline = Options.bAnnFontUnderline;
	m_bScaleDocs = Options.bScaleDocuments;
	m_bScaleGraphics = Options.bScaleGraphics;
	m_bResizableCallouts = Options.bResizableCallouts;
	m_bPanCallouts = Options.bPanCallouts;
	m_strFontName = Options.strAnnFontName;
	m_bShadeOnCallout = Options.bShadeOnCallout;
	m_sCalloutShadeGrayscale = Options.sCalloutShadeGrayscale;

	//	Update the controls
	if(IsWindow(m_hWnd))
		UpdateData(FALSE);
}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::SelectColor()
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
void CGraphicsPage::SelectColor(int nColor)
{
	switch(nColor)
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
// 	Function Name:	CGraphicsPage::SetColor()
//
// 	Description:	This function is called selects a new color. It sets the
//					color for the currently selected option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CGraphicsPage::SetColor(int nColor)
{
	switch(m_nColorSelect)
	{
		case DRAWCOLOR:			  m_nDrawColor = nColor;
								  return;
		case HIGHLIGHTCOLOR:	  m_nHighlightColor = nColor;
								  return;
		case REDACTCOLOR:		  m_nRedactColor = nColor;
								  return;
		case CALLOUTCOLOR:		  m_nCalloutColor = nColor;
								  return;
		case CALLFRAMECOLOR:	  m_nCallFrameColor = nColor;
								  return;
		case CALLHANDLECOLOR:	  m_nCallHandleColor = nColor;
								  return;
		case LIGHTPENCOLOR:		  m_nLightPenColor = nColor;
								  return;
		case USERSPLITFRAMECOLOR: m_nUserSplitFrameColor = nColor;
								  return;
		case ZAPSPLITFRAMECOLOR:  m_nZapSplitFrameColor = nColor;
								  return;
		default:				  return;
	}

}

//==============================================================================
//
// 	Function Name:	CGraphicsPage::WriteOptions()
//
// 	Description:	This function is called to write the page options to the
//					ini file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CGraphicsPage::WriteOptions(CTMIni& rIni)
{
	SGraphicsOptions Options;
	CString			 strError;

	//	Refresh the class members
	UpdateData(TRUE);

	//	Is the drawing thickness out of range?
	if((m_nThickness < GRAPHICS_MINIMUM_THICKNESS) ||
	   (m_nThickness > GRAPHICS_MAXIMUM_THICKNESS))
	{
		if(m_pErrors)
		{
			strError.Format("%d and %d", GRAPHICS_MINIMUM_THICKNESS, 
										 GRAPHICS_MAXIMUM_THICKNESS);
			m_pErrors->Handle(0, IDS_TMSETUP_INVALIDTHICKNESS, strError);
		}
		return FALSE;
	}

	//	Is the drawing thickness out of range?
	if((m_nZoomFactor < GRAPHICS_MINIMUM_ZOOMFACTOR) ||
	   (m_nZoomFactor > GRAPHICS_MAXIMUM_ZOOMFACTOR))
	{
		if(m_pErrors)
		{
			strError.Format("%d and %d", GRAPHICS_MINIMUM_ZOOMFACTOR, 
										 GRAPHICS_MAXIMUM_ZOOMFACTOR);
			m_pErrors->Handle(0, IDS_TMSETUP_INVALIDZOOMFACTOR, strError);
		}
		return FALSE;
	}

	//	Fill the transfer structure
	Options.sAnnColor = m_nDrawColor;
	Options.sAnnThickness = m_nThickness;
	Options.sHighlightColor = m_nHighlightColor;
	Options.sRedactColor = m_nRedactColor;
	Options.sMaxZoom = m_nZoomFactor;
	Options.sCalloutColor = m_nCalloutColor;
	Options.sCalloutHandleColor = m_nCallHandleColor;
	Options.sCalloutFrameColor = m_nCallFrameColor;
	Options.sCalloutFrameThickness = m_nCallFrameThick;
	Options.sBitonalScaling = m_iBitonal;
	Options.sAnnTool = m_nTool;
	Options.sAnnFontSize = m_sFontSize;
	Options.sUserSplitFrameColor = m_nUserSplitFrameColor;
	Options.sZapSplitFrameColor = m_nZapSplitFrameColor;
	Options.sLightPenColor = m_nLightPenColor;
	Options.sLightPenSize = m_nLightPenSize;
	Options.bLightPenEnabled = m_bLightPenControls;
	Options.bAnnFontBold = m_bFontBold;
	Options.bAnnFontStrikeThrough = m_bFontStrikeThrough;
	Options.bAnnFontUnderline = m_bFontUnderline;
	Options.bScaleDocuments = m_bScaleDocs;
	Options.bScaleGraphics = m_bScaleGraphics;
	Options.bResizableCallouts = m_bResizableCallouts;
	Options.bPanCallouts = m_bPanCallouts;
	Options.strAnnFontName = m_strFontName;
	Options.bShadeOnCallout = m_bShadeOnCallout;
	Options.sCalloutShadeGrayscale = m_sCalloutShadeGrayscale;

	//	Write the options to the ini file
	rIni.WriteGraphicsOptions(&Options);

	return TRUE;
}
