//==============================================================================
//
// File Name:	 annotate.cpp
//
// Description:	This file contains member functions of the CAnnotationProperties
//				class. 
//
// See Also:	annprops.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	07-14-97	1.00		Original Release
//	04-04-98	2.00		Added options for Callout color and Mouse Pan
//	04-05098	2.00		Added options for Callout Frame color and thickness
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmviewap.h>
#include <annprops.h>
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
BEGIN_MESSAGE_MAP(CAnnotationProperties, CDialog)
	//{{AFX_MSG_MAP(CAnnotationProperties)
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
	ON_BN_CLICKED(IDC_DEFAULTS, OnDefaults)
	ON_BN_CLICKED(IDC_BACKGROUND, OnBackground)
	ON_BN_CLICKED(IDC_CALLOUT, OnCallout)
	ON_BN_CLICKED(IDC_FRAMECOLOR, OnFrameColor)
	ON_BN_CLICKED(IDC_HANDLECOLOR, OnHandleColor)
	ON_BN_CLICKED(IDC_SPLITFRAME, OnSplitFrame)
	ON_BN_CLICKED(IDC_SELECTCOLOR, OnSelectorColor)
	ON_BN_CLICKED(IDC_DARKBLUE, OnDarkBlue)
	ON_BN_CLICKED(IDC_DARKGREEN, OnDarkGreen)
	ON_BN_CLICKED(IDC_DARKRED, OnDarkRed)
	ON_BN_CLICKED(IDC_LIGHTBLUE, OnLightBlue)
	ON_BN_CLICKED(IDC_LIGHTGREEN, OnLightGreen)
	ON_BN_CLICKED(IDC_LIGHTRED, OnLightRed)
	ON_BN_CLICKED(IDC_FONT, OnChangeFont)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::CAnnotationProperties()
//
// 	Description:	This is the constructor for CAnnotationProperties objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CAnnotationProperties::CAnnotationProperties(long lFlags, CWnd* pParent)
					  :CDialog(CAnnotationProperties::IDD, pParent)
{

	//{{AFX_DATA_INIT(CAnnotationProperties)
	m_nThickness = DEFAULT_ANNTHICKNESS;
	m_nZoomFactor = DEFAULT_MAXZOOM;
	m_nColorSelect = DRAWCOLOR;
	m_bFitToImage = DEFAULT_FITTOIMAGE;
	m_bScaleImage = DEFAULT_SCALEIMAGE;
	m_nRotation = DEFAULT_ROTATION;
	m_nPanPercent = DEFAULT_PANPERCENT;
	m_bHideScrollBars = DEFAULT_HIDESCROLLBARS;
	m_sZoomOnLoad = -1;
	m_nFrameThickness = DEFAULT_CALLFRAMETHICKNESS;
	m_bRightClickPan = DEFAULT_RIGHTCLICKPAN;
	m_bSplitScreen = DEFAULT_SPLITSCREEN;
	m_bSyncPanes = DEFAULT_SYNCPANES;
	m_nSplitThickness = DEFAULT_SPLITFRAMETHICKNESS;
	m_bSyncCalloutAnn = DEFAULT_SYNCCALLOUTANN;
	m_bSelectorVisible = DEFAULT_PENSELECTORVISIBLE;
	m_nSelectorSize = DEFAULT_PENSELECTORSIZE;
	m_bKeepAspect = DEFAULT_KEEPASPECT;
	m_iBitonal = -1;
	m_bZoomToRect = FALSE;
	m_nTool = -1;
	m_bResizeableCallouts = DEFAULT_RESIZECALLOUTS;
	m_strFontName = _T("");
	m_bShadeOnCallout = FALSE;
	m_sCalloutShadeGrayscale = 0;
	m_bPanCallouts = FALSE;
	m_bZoomCallouts = FALSE;
	//}}AFX_DATA_INIT

	//	Set the default colors
	m_nDrawColor			 = DEFAULT_ANNCOLOR;
	m_nHighlightColor		 = DEFAULT_HIGHLIGHTCOLOR;
	m_nRedactColor			 = DEFAULT_REDACTCOLOR;
	m_nBackgroundColor		 = DEFAULT_BACKGROUNDCOLOR;
	m_nCalloutColor			 = DEFAULT_CALLOUTCOLOR;
	m_nCalloutFrameColor	 = DEFAULT_CALLFRAMECOLOR;
	m_nCalloutHandleColor	 = DEFAULT_CALLHANDLECOLOR;
	m_nSplitFrameColor		 = DEFAULT_SPLITFRAMECOLOR;
	m_nSelectorColor		 = DEFAULT_PENSELECTORCOLOR;

	//	Save the flags for use when we initialize the dialog box
	m_lFlags = lFlags;
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the 
//					dialog box controls and class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	
	//{{AFX_DATA_MAP(CAnnotationProperties)
	DDX_Control(pDX, IDC_FONTNAME, m_ctrlFontName);
	DDX_Control(pDX, IDC_SPINSELECT, m_SpinSelector);
	DDX_Control(pDX, IDC_SPINSPLITTHICK, m_SpinSplitThickness);
	DDX_Control(pDX, IDC_SPINFRAMETHICK, m_SpinFrameThickness);
	DDX_Control(pDX, IDC_PANPERCENTAGELABEL, m_ctrlPanPercentageLabel);
	DDX_Control(pDX, IDC_HIDESCROLLBARS, m_ctrlHideScrollBars);
	DDX_Control(pDX, IDC_PANPERCENTAGE, m_ctrlPanPercent);
	DDX_Control(pDX, IDC_SPINPAN, m_SpinPan);
	DDX_Control(pDX, IDC_ROTATIONLABEL, m_ctrlRotationLabel);
	DDX_Control(pDX, IDC_FITTOIMAGE, m_ctrlFitToImage);
	DDX_Control(pDX, IDC_ROTATION, m_ctrlRotation);
	DDX_Control(pDX, IDC_SCALEIMAGE, m_ctrlScaleImage);
	DDX_Control(pDX, IDC_SPINROTATION, m_SpinRotation);
	DDX_Control(pDX, IDC_SPINZOOM, m_SpinZoom);
	DDX_Control(pDX, IDC_SPINTHICK, m_SpinThick);
	DDX_Text(pDX, IDC_THICKNESS, m_nThickness);
	DDV_MinMaxUInt(pDX, m_nThickness, 1, 20);
	DDX_Text(pDX, IDC_ZOOMFACTOR, m_nZoomFactor);
	DDV_MinMaxUInt(pDX, m_nZoomFactor, 1, 100);
	DDX_Radio(pDX, IDC_DRAWTOOL, m_nColorSelect);
	DDX_Check(pDX, IDC_FITTOIMAGE, m_bFitToImage);
	DDX_Check(pDX, IDC_SCALEIMAGE, m_bScaleImage);
	DDX_Text(pDX, IDC_ROTATION, m_nRotation);
	DDV_MinMaxInt(pDX, m_nRotation, -360, 360);
	DDX_Text(pDX, IDC_PANPERCENTAGE, m_nPanPercent);
	DDV_MinMaxUInt(pDX, m_nPanPercent, 1, 100);
	DDX_Check(pDX, IDC_HIDESCROLLBARS, m_bHideScrollBars);
	DDX_Radio(pDX, IDC_INITIALNONE, m_sZoomOnLoad);
	DDX_Text(pDX, IDC_FRAMETHICK, m_nFrameThickness);
	DDV_MinMaxInt(pDX, m_nFrameThickness, 0, 25);
	DDX_Check(pDX, IDC_RIGHTCLICKPAN, m_bRightClickPan);
	DDX_Check(pDX, IDC_SPLITSCREEN, m_bSplitScreen);
	DDX_Check(pDX, IDC_SYNCPANES, m_bSyncPanes);
	DDX_Text(pDX, IDC_SPLITTHICK, m_nSplitThickness);
	DDV_MinMaxInt(pDX, m_nSplitThickness, 0, 100);
	DDX_Check(pDX, IDC_SYNCCALLOUTANN, m_bSyncCalloutAnn);
	DDX_Check(pDX, IDC_SELECTVISIBLE, m_bSelectorVisible);
	DDX_Text(pDX, IDC_SELECTSIZE, m_nSelectorSize);
	DDX_Check(pDX, IDC_KEEPASPECT, m_bKeepAspect);
	DDX_CBIndex(pDX, IDC_BITONAL, m_iBitonal);
	DDX_Check(pDX, IDC_ZOOMTORECT, m_bZoomToRect);
	DDX_CBIndex(pDX, IDC_ANNTOOL, m_nTool);
	DDX_Check(pDX, IDC_RESIZEABLE_CALLOUTS, m_bResizeableCallouts);
	DDX_Text(pDX, IDC_FONTNAME, m_strFontName);
	DDX_Check(pDX, IDC_SHADEONCALLOUT, m_bShadeOnCallout);
	DDX_Text(pDX, IDC_SHADE_GRAYSCALE, m_sCalloutShadeGrayscale);
	DDX_Check(pDX, IDC_PAN_CALLOUTS, m_bPanCallouts);
	DDX_Check(pDX, IDC_ZOOM_CALLOUTS, m_bZoomCallouts);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnBackground()
//
// 	Description:	This function is called when the user clicks on the 
//					Background color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnBackground() 
{
	m_nColorSelect = BACKGROUNDCOLOR;
	SelectColor(m_nBackgroundColor);
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnBlack()
//
// 	Description:	This function is called when the user clicks on the Black
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnBlack() 
{
	SetColor(TMV_BLACK);
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnBlue()
//
// 	Description:	This function is called when the user clicks on the Blue
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnBlue() 
{
	SetColor(TMV_BLUE);
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnCallout()
//
// 	Description:	This function is called when the user clicks on the 
//					Callout color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnCallout() 
{
	m_nColorSelect = CALLOUTCOLOR;
	SelectColor(m_nCalloutColor);

}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnChangeFont()
//
// 	Description:	This function is called when the user clicks on the Font
//					button.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnChangeFont() 
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
// 	Function Name:	CAnnotationProperties::OnCyan()
//
// 	Description:	This function is called when the user clicks on the Cyan
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnCyan() 
{
	SetColor(TMV_CYAN);
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnDarkBlue()
//
// 	Description:	This function is called when the user clicks on the Dark
//					Blue button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnDarkBlue() 
{
	SetColor(TMV_DARKBLUE);
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnDarkGreen()
//
// 	Description:	This function is called when the user clicks on the Dark
//					Green button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnDarkGreen() 
{
	SetColor(TMV_DARKGREEN);
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnDarkRed()
//
// 	Description:	This function is called when the user clicks on the Dark
//					Red button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnDarkRed() 
{
	SetColor(TMV_DARKRED);
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnDefaults()
//
// 	Description:	This function is called when the user clicks on the 
//					Default button. It resets all values to their defaults.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnDefaults() 
{
	CDataExchange Exchange(this, FALSE);
	
	m_nDrawColor		  = DEFAULT_ANNCOLOR;
	m_nHighlightColor	  = DEFAULT_HIGHLIGHTCOLOR;
	m_nCalloutColor		  = DEFAULT_CALLOUTCOLOR;
	m_nRedactColor		  = DEFAULT_REDACTCOLOR;
	m_nBackgroundColor	  = DEFAULT_BACKGROUNDCOLOR;
	m_nZoomFactor		  = DEFAULT_MAXZOOM;
	m_nTool				  = DEFAULT_ANNTOOL;
	m_nThickness		  = DEFAULT_ANNTHICKNESS;
	m_nColorSelect		  = DRAWCOLOR;
	m_bFitToImage		  = DEFAULT_FITTOIMAGE;
	m_bScaleImage		  = DEFAULT_SCALEIMAGE;
	m_bKeepAspect		  = DEFAULT_KEEPASPECT;
	m_bHideScrollBars	  = DEFAULT_HIDESCROLLBARS;
	m_nRotation			  = DEFAULT_ROTATION;
	m_nPanPercent		  = DEFAULT_PANPERCENT;
	m_nFrameThickness	  = DEFAULT_CALLFRAMETHICKNESS;
	m_nCalloutFrameColor  = DEFAULT_CALLFRAMECOLOR;
	m_nCalloutHandleColor = DEFAULT_CALLHANDLECOLOR;
	m_nSplitFrameColor	  = DEFAULT_SPLITFRAMECOLOR;
	m_nSplitThickness	  = DEFAULT_SPLITFRAMETHICKNESS;
	m_bSplitScreen		  = DEFAULT_SPLITSCREEN;
	m_bSyncPanes		  = DEFAULT_SYNCPANES;
	m_bSyncCalloutAnn	  = DEFAULT_SYNCCALLOUTANN;
	m_bSelectorVisible	  = DEFAULT_PENSELECTORVISIBLE;
	m_nSelectorColor	  = DEFAULT_PENSELECTORCOLOR;
	m_nSelectorSize		  = DEFAULT_PENSELECTORSIZE;
	m_bResizeableCallouts = DEFAULT_RESIZECALLOUTS;
	m_bShadeOnCallout     = DEFAULT_SHADEONCALLOUT;
	
	//	Reset the controls
	DoDataExchange(&Exchange);

	//	Select the appropriate color button
	SelectColor(m_nDrawColor);
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnHighlight()
//
// 	Description:	This function is called when the user clicks on the 
//					Draw color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnDrawtool() 
{
	m_nColorSelect = DRAWCOLOR;
	SelectColor(m_nDrawColor);
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnFrameColor()
//
// 	Description:	This function is called when the user clicks on the 
//					Callout Frame Color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnFrameColor() 
{
	m_nColorSelect = CALLFRAMECOLOR;
	SelectColor(m_nCalloutFrameColor);
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnGreen()
//
// 	Description:	This function is called when the user clicks on the Green
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnGreen() 
{
	SetColor(TMV_GREEN);
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnGrey()
//
// 	Description:	This function is called when the user clicks on the Grey
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnGrey() 
{
	SetColor(TMV_GREY);
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnHandleColor()
//
// 	Description:	This function is called when the user clicks on the 
//					Callout Handle Color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnHandleColor() 
{
	m_nColorSelect = CALLHANDLECOLOR;
	SelectColor(m_nCalloutHandleColor);
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnHighlight()
//
// 	Description:	This function is called when the user clicks on the 
//					Highlight color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnHighlight() 
{
	m_nColorSelect = HIGHLIGHTCOLOR;
	SelectColor(m_nHighlightColor);

}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnInitDialog()
//
// 	Description:	This function is called to initialize the dialog box.
//
// 	Returns:		TRUE if successful.
//
//	Notes:			None
//
//==============================================================================
BOOL CAnnotationProperties::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
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

	//	Set up the spin controls
	UDACCEL Accel;
	Accel.nSec = 0;
	Accel.nInc = 5;

	m_SpinZoom.SetRange(1,100);
	m_SpinZoom.SetAccel(1,&Accel);

	m_SpinPan.SetRange(1,100);
	m_SpinPan.SetAccel(1,&Accel);

	m_SpinThick.SetRange(1,20);
	m_SpinFrameThickness.SetRange(0,25);
	m_SpinSplitThickness.SetRange(0,50);
	m_SpinSelector.SetRange(1,25);

	Accel.nInc = 10;
	m_SpinRotation.SetRange(-360,360);
	m_SpinRotation.SetAccel(1,&Accel);

	//	Check the flags to see which controls should be disabled
	if(m_lFlags & DISABLE_SCALEIMAGE)
		m_ctrlScaleImage.EnableWindow(FALSE);
	if(m_lFlags & DISABLE_FITTOIMAGE)
		m_ctrlFitToImage.EnableWindow(FALSE);
	if(m_lFlags & DISABLE_HIDESCROLLBARS)
		m_ctrlHideScrollBars.EnableWindow(FALSE);
	if(m_lFlags & DISABLE_ROTATION)
	{
		m_ctrlRotationLabel.EnableWindow(FALSE);
		m_ctrlRotation.EnableWindow(FALSE);
		m_SpinRotation.EnableWindow(FALSE);
	}
	if(m_lFlags & DISABLE_PANPERCENT)
	{
		m_ctrlPanPercentageLabel.EnableWindow(FALSE);
		m_ctrlPanPercent.EnableWindow(FALSE);
		m_SpinPan.EnableWindow(FALSE);
	}

	SelectColor(m_nDrawColor);

	return FALSE;  
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnLightBlue()
//
// 	Description:	This function is called when the user clicks on the Light
//					Blue button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnLightBlue() 
{
	SetColor(TMV_LIGHTBLUE);
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnLightGreen()
//
// 	Description:	This function is called when the user clicks on the Light
//					Green button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnLightGreen() 
{
	SetColor(TMV_LIGHTGREEN);
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnLightRed()
//
// 	Description:	This function is called when the user clicks on the Light
//					Red button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnLightRed() 
{
	SetColor(TMV_LIGHTRED);
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnMagenta()
//
// 	Description:	This function is called when the user clicks on the Magenta
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnMagenta() 
{
	SetColor(TMV_MAGENTA);
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnRed()
//
// 	Description:	This function is called when the user clicks on the Red
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnRed() 
{
	SetColor(TMV_RED);
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnRedact()
//
// 	Description:	This function is called when the user clicks on the 
//					Redact color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnRedact() 
{
	m_nColorSelect = REDACTCOLOR;
	SelectColor(m_nRedactColor);

}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnSelectorColor()
//
// 	Description:	This function is called when the user clicks on the 
//					Pen Selector color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnSelectorColor() 
{
	m_nColorSelect = SELECTORCOLOR;
	SelectColor(m_nSelectorColor);
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnSplitFrame()
//
// 	Description:	This function is called when the user clicks on the 
//					Split Frame color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnSplitFrame() 
{
	m_nColorSelect = SPLITFRAMECOLOR;
	SelectColor(m_nSplitFrameColor);

}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnWhite()
//
// 	Description:	This function is called when the user clicks on the White
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnWhite() 
{
	SetColor(TMV_WHITE);
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::OnYellow()
//
// 	Description:	This function is called when the user clicks on the Yellow
//					button. It sets the color of the selected color option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::OnYellow() 
{
	SetColor(TMV_YELLOW);
}

//==============================================================================
//
// 	Function Name:	CAnnotationProperties::SelectColor()
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
void CAnnotationProperties::SelectColor(int nColor)
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
// 	Function Name:	CAnnotationProperties::SetColor()
//
// 	Description:	This function is called selects a new color. It sets the
//					color for the currently selected option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnotationProperties::SetColor(int nColor)
{
	switch(m_nColorSelect)
	{
		case DRAWCOLOR:			m_nDrawColor = nColor;
								return;
		case HIGHLIGHTCOLOR:	m_nHighlightColor = nColor;
								return;
		case REDACTCOLOR:		m_nRedactColor = nColor;
								return;
		case BACKGROUNDCOLOR:	m_nBackgroundColor = nColor;
								return;
		case CALLOUTCOLOR:		m_nCalloutColor = nColor;
								return;
		case CALLFRAMECOLOR:	m_nCalloutFrameColor = nColor;
								return;
		case CALLHANDLECOLOR:	m_nCalloutHandleColor = nColor;
								return;
		case SPLITFRAMECOLOR:	m_nSplitFrameColor = nColor;
								return;
		case SELECTORCOLOR:		m_nSelectorColor = nColor;
								return;
		default:				return;
	}

}

