//==============================================================================
//
// File Name:	textpg.cpp
//
// Description:	This file contains member functions of the CTextPage class.
//
// See Also:	textpg.h
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
#include <textpg.h>
#include <tmsetup.h>

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
extern CTMSetupCtrl* theControl;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CTextPage, CSetupPage)
	//{{AFX_MSG_MAP(CTextPage)
	ON_BN_CLICKED(IDC_TEXT_BACKCOLOR, OnBackColor)
	ON_BN_CLICKED(IDC_TEXT_FORECOLOR, OnForeColor)
	ON_BN_CLICKED(IDC_TEXT_HIGHLIGHTCOLOR, OnHighlightColor)
	ON_BN_CLICKED(IDC_TEXT_HIGHTEXTCOLOR, OnHighlightTextColor)
	ON_BN_CLICKED(IDC_USEMANAGERHIGHLIGHTERS, OnUseManagerHighlightersClicked)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	EnumFontsProc()
//
// 	Description:	This callback is used by the system to enumerate the fonts
//
// 	Returns:		Non-zero to continue
//
//	Notes:			None
//
//==============================================================================
int CALLBACK EnumFontsProc(ENUMLOGFONTEX* lplfFont, NEWTEXTMETRIC* lptmFont,
						   int iType, LPARAM lParam)
{
	CComboBox* pList = (CComboBox*)lParam;

	if(pList)
		pList->AddString(lplfFont->elfLogFont.lfFaceName);
	return 1;
}

//==============================================================================
//
// 	Function Name:	CTextPage::CTextPage()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTextPage::CTextPage(CWnd* pParent) : CSetupPage(CTextPage::IDD, pParent)
{
	//{{AFX_DATA_INIT(CTextPage)
	m_bUseAvgCharWidth = FALSE;
	m_sMaxCharsPerLine = 0;
	m_bShowPageLine = FALSE;
	m_bShowText = TRUE;
	m_sHighlightLines = 0;
	m_sDisplayLines = 0;
	m_bCenterVideo = FALSE;
	m_bCombineText = FALSE;
	m_sScrollSteps = 0;
	m_sScrollTime = 0;
	m_bSmoothScroll = FALSE;
	m_bUseManagerHighlighters = FALSE;
	m_bDisableScrollText = FALSE;
	m_sBottomMargin = 0;
	m_sLeftMargin = 0;
	m_sRightMargin = 0;
	m_sTopMargin = 0;
	m_iBulletStyle = 0;
	//}}AFX_DATA_INIT

	m_crBackground = RGB(0,0,0);
	m_crForeground = RGB(128,128,128);
	m_crHighlight = RGB(255,255,0);
	m_crHighlightText = RGB(0,0,0);
	m_strFont = "";
}

//==============================================================================
//
// 	Function Name:	CTextPage::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the class
//					members and the associated dialog box controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTextPage::DoDataExchange(CDataExchange* pDX)
{
	CSetupPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CTextPage)
	DDX_Control(pDX, IDC_USEMANAGERHIGHLIGHTERS, m_ctrlUseManagerHighlighters);
	DDX_Control(pDX, IDC_BACKCOLOR_LABEL, m_ctrlBackColorLabel);
	DDX_Control(pDX, IDC_HIGHTEXTCOLOR_LABEL, m_ctrlHighTextColorLabel);
	DDX_Control(pDX, IDC_HIGHCOLOR_LABEL, m_ctrlHighColorLabel);
	DDX_Control(pDX, IDC_FORECOLOR_LABEL, m_ctrlForeColorLabel);
	DDX_Control(pDX, IDC_TEXT_HIGHTEXTCOLOR, m_HighTextColor);
	DDX_Control(pDX, IDC_TEXT_HIGHLIGHTCOLOR, m_HighlightColor);
	DDX_Control(pDX, IDC_TEXT_FORECOLOR, m_ForeColor);
	DDX_Control(pDX, IDC_TEXT_BACKCOLOR, m_BackColor);
	DDX_Control(pDX, IDC_TEXT_FONTS, m_ctrlFonts);
	DDX_Check(pDX, IDC_TEXT_AVGCHARWIDTH, m_bUseAvgCharWidth);
	DDX_Text(pDX, IDC_TEXT_MAXCHARSPERLINE, m_sMaxCharsPerLine);
	DDX_Check(pDX, IDC_TEXT_SHOWPAGELINE, m_bShowPageLine);
	DDX_Check(pDX, IDC_TEXT_SHOWTEXT, m_bShowText);
	DDX_Text(pDX, IDC_TEXT_HIGHLIGHTLINES, m_sHighlightLines);
	DDX_Text(pDX, IDC_TEXT_DISPLAYLINES, m_sDisplayLines);
	DDX_Check(pDX, IDC_TEXT_CENTERVIDEO, m_bCenterVideo);
	DDX_Check(pDX, IDC_COMBINETEXT, m_bCombineText);
	DDX_Text(pDX, IDC_TEXT_SCROLLSTEPS, m_sScrollSteps);
	DDX_Text(pDX, IDC_TEXT_SCROLLTIME, m_sScrollTime);
	DDX_Check(pDX, IDC_TEXT_SMOOTHSCROLL, m_bSmoothScroll);
	DDX_Check(pDX, IDC_USEMANAGERHIGHLIGHTERS, m_bUseManagerHighlighters);
	DDX_Check(pDX, IDC_TEXT_DISABLESCROLLTEXT, m_bDisableScrollText);
	DDX_Text(pDX, IDC_TEXT_BOTTOM_MARGIN, m_sBottomMargin);
	DDX_Text(pDX, IDC_TEXT_LEFT_MARGIN, m_sLeftMargin);
	DDX_Text(pDX, IDC_TEXT_RIGHT_MARGIN, m_sRightMargin);
	DDX_Text(pDX, IDC_TEXT_TOP_MARGIN, m_sTopMargin);
	DDX_CBIndex(pDX, IDC_BULLET_STYLE, m_iBulletStyle);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CTextPage::OnBackColor()
//
// 	Description:	This function is called to change the background color
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTextPage::OnBackColor() 
{
	SetColor(&m_crBackground, &m_BackColor);	
}

//==============================================================================
//
// 	Function Name:	CTextPage::OnForeColor()
//
// 	Description:	This function is called to change the foreground color
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTextPage::OnForeColor() 
{
	SetColor(&m_crForeground, &m_ForeColor);	
}

//==============================================================================
//
// 	Function Name:	CTextPage::OnHighlightColor()
//
// 	Description:	This function is called to change the highlight color
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTextPage::OnHighlightColor() 
{
	SetColor(&m_crHighlight, &m_HighlightColor);	
}

//==============================================================================
//
// 	Function Name:	CTextPage::OnHighlightTextColor()
//
// 	Description:	This function is called to change the highlighted text color
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTextPage::OnHighlightTextColor() 
{
	SetColor(&m_crHighlightText, &m_HighTextColor);	
}

//==============================================================================
//
// 	Function Name:	CTextPage::OnInitDialog()
//
// 	Description:	This will initialize this page
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CTextPage::OnInitDialog() 
{
	LOGFONT	lfFont;
	CDC*	pdc = GetDC();
	int		iIndex;

	//	Do base class initialization
	CDialog::OnInitDialog();
	
	//	Fill the fonts list box
	if(pdc)
	{
		ZeroMemory(&lfFont, sizeof(lfFont));
		lfFont.lfCharSet = ANSI_CHARSET;
	
		EnumFontFamiliesEx(pdc->GetSafeHdc(), &lfFont, 
						  (FONTENUMPROC)EnumFontsProc, 
						  (LPARAM)&m_ctrlFonts, 0);
		ReleaseDC(pdc);
	}
	
	//	Select the current font
	if((iIndex = m_ctrlFonts.FindStringExact(-1, m_strFont)) != LB_ERR)
	{
		m_ctrlFonts.SetCurSel(iIndex);
		m_ctrlFonts.SetTopIndex(iIndex);
	}

	//	Set the button colors
	m_BackColor.SetColor(m_crBackground);
	m_ForeColor.SetColor(m_crForeground);
	m_HighlightColor.SetColor(m_crHighlight);
	m_HighTextColor.SetColor(m_crHighlightText);
	
	return TRUE;  
}

//==============================================================================
//
// 	Function Name:	CTextPage::SetColor()
//
// 	Description:	This function will open set the color reference provided by
//					the caller
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTextPage::SetColor(COLORREF* pColor, CColorPushbutton* pButton) 
{
	ASSERT(pColor);
	ASSERT(pButton);

	CColorDialog Dialog(*pColor, CC_PREVENTFULLOPEN | CC_SOLIDCOLOR, this);
	if(Dialog.DoModal() == IDOK)
	{
		*pColor = Dialog.GetColor();
		pButton->SetColor(*pColor);
		pButton->RedrawWindow();
	}
}

//==============================================================================
//
// 	Function Name:	CTextPage::OnUseManagerHighlightersClicked()
//
// 	Description:	This function is called when the user clicks on the
//					UseManagerHighlighters option check box
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTextPage::OnUseManagerHighlightersClicked() 
{
	BOOL bEnable = (m_ctrlUseManagerHighlighters.GetCheck() == 0);

	m_ctrlForeColorLabel.EnableWindow(bEnable);
	m_ctrlHighColorLabel.EnableWindow(bEnable);
	m_ctrlHighTextColorLabel.EnableWindow(bEnable);
	m_ForeColor.ShowWindow(bEnable ? SW_SHOW : SW_HIDE);
	m_HighlightColor.ShowWindow(bEnable ? SW_SHOW : SW_HIDE);
	m_HighTextColor.ShowWindow(bEnable ? SW_SHOW : SW_HIDE);
			
}

//==============================================================================
//
// 	Function Name:	CTextPage::ReadOptions()
//
// 	Description:	This function is called to read the page options from the
//					ini file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTextPage::ReadOptions(CTMIni& rIni)
{

	//	Read the options from the ini file
	rIni.ReadTextOptions(&m_Options);

	//	Set the class members
	m_bDisableScrollText = m_Options.bDisableScrollText;
	m_bUseAvgCharWidth = m_Options.bUseAvgCharWidth;
	m_bShowText = m_Options.bShowText;
	m_bShowPageLine = m_Options.bShowPageLine;
	m_bCombineText = m_Options.bCombineText;
	m_bSmoothScroll = m_Options.bSmoothScroll;
	m_bCenterVideo = m_Options.bCenterVideo;
	m_sDisplayLines = m_Options.sDisplayLines;
	m_sHighlightLines = m_Options.sHighlightLines;
	m_sMaxCharsPerLine = m_Options.sMaxCharsPerLine;
	m_sScrollSteps = m_Options.sScrollSteps;
	m_sScrollTime = m_Options.sScrollTime;
	m_sLeftMargin = m_Options.sLeftMargin;
	m_sRightMargin = m_Options.sRightMargin;
	m_sTopMargin = m_Options.sTopMargin;
	m_sBottomMargin = m_Options.sBottomMargin;
	m_crBackground = theControl->TranslateColor(m_Options.lBackground);
	m_crForeground = theControl->TranslateColor(m_Options.lForeground);
	m_crHighlight = theControl->TranslateColor(m_Options.lHighlight);
	m_crHighlightText = theControl->TranslateColor(m_Options.lHighlightText);
	m_strFont = m_Options.strTextFont;
	m_bUseManagerHighlighters = m_Options.bUseManagerHighlighter;
	m_iBulletStyle = m_Options.sBulletStyle;

	//	Update the controls
	if(IsWindow(m_hWnd))
	{
		UpdateData(FALSE);

		//	Enable/disable the color buttons
		OnUseManagerHighlightersClicked();

		//	Update the button colors
		m_BackColor.SetColor(m_crBackground);
		m_ForeColor.SetColor(m_crForeground);
		m_HighlightColor.SetColor(m_crHighlight);
		m_HighTextColor.SetColor(m_crHighlightText);

		m_BackColor.RedrawWindow();
		m_ForeColor.RedrawWindow();
		m_HighlightColor.RedrawWindow();
		m_HighTextColor.RedrawWindow();
	}
}

//==============================================================================
//
// 	Function Name:	CTextPage::WriteOptions()
//
// 	Description:	This function is called to write the page options to the
//					ini file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTextPage::WriteOptions(CTMIni& rIni)
{
	CString		strError;
	int			iIndex = -1;

	//	Refresh the class members
	UpdateData(TRUE);

	//	Is the maximum characters per line value out of range?
	if((m_sMaxCharsPerLine < TEXT_MINIMUM_CHARSPERLINE) ||
	   (m_sMaxCharsPerLine > TEXT_MAXIMUM_CHARSPERLINE))
	{
		if(m_pErrors)
		{
			strError.Format("%d and %d", TEXT_MINIMUM_CHARSPERLINE, 
										 TEXT_MAXIMUM_CHARSPERLINE);
			m_pErrors->Handle(0, IDS_TMSETUP_INVALIDCHARSPERLINE, strError);
		}
		return FALSE;
	}

	//	Is the highlight lines value out of range?
	/*
	if((m_sHighlightLines < TEXT_MINIMUM_HIGHLIGHTLINES) ||
	   (m_sHighlightLines > TEXT_MAXIMUM_HIGHLIGHTLINES))
	{
		if(m_pErrors)
		{
			strError.Format("%d and %d", TEXT_MINIMUM_HIGHLIGHTLINES, 
										 TEXT_MAXIMUM_HIGHLIGHTLINES);
			m_pErrors->Handle(0, IDS_TMSETUP_INVALIDHIGHLIGHTLINES, strError);
		}
		return FALSE;
	}
	*/

	//	Is the display lines value out of range?
	if((m_sDisplayLines < TEXT_MINIMUM_DISPLAYLINES) ||
	   (m_sDisplayLines > TEXT_MAXIMUM_DISPLAYLINES))
	{
		if(m_pErrors)
		{
			strError.Format("%d and %d", TEXT_MINIMUM_DISPLAYLINES, 
										 TEXT_MAXIMUM_DISPLAYLINES);
			m_pErrors->Handle(0, IDS_TMSETUP_INVALIDDISPLAYLINES, strError);
		}
		return FALSE;
	}

	//	Get the selected font
	if((iIndex = m_ctrlFonts.GetCurSel()) != LB_ERR)
	{
		m_ctrlFonts.GetLBText(iIndex, m_strFont);
	}

	//	Fill the transfer structure
	m_Options.bDisableScrollText = m_bDisableScrollText;
	m_Options.bUseAvgCharWidth = m_bUseAvgCharWidth;
	m_Options.bShowText = m_bShowText;
	m_Options.bShowPageLine = m_bShowPageLine;
	m_Options.bCombineText = m_bCombineText;
	m_Options.bSmoothScroll = m_bSmoothScroll;
	m_Options.bCenterVideo = m_bCenterVideo;
	m_Options.sDisplayLines = m_sDisplayLines;
	m_Options.sHighlightLines = m_sHighlightLines;
	m_Options.sMaxCharsPerLine = m_sMaxCharsPerLine;
	m_Options.sScrollSteps = m_sScrollSteps;
	m_Options.sScrollTime = m_sScrollTime;
	m_Options.sLeftMargin = m_sLeftMargin;
	m_Options.sRightMargin = m_sRightMargin;
	m_Options.sTopMargin = m_sTopMargin;
	m_Options.sBottomMargin = m_sBottomMargin;
	m_Options.lBackground = (OLE_COLOR)m_crBackground;
	m_Options.lForeground = (OLE_COLOR)m_crForeground;
	m_Options.lHighlight = (OLE_COLOR)m_crHighlight;
	m_Options.lHighlightText = (OLE_COLOR)m_crHighlightText;
	m_Options.strTextFont = m_strFont;
	m_Options.bUseManagerHighlighter = m_bUseManagerHighlighters;
	m_Options.sBulletStyle = (short)m_iBulletStyle;

	//	Write the options to the ini file
	rIni.WriteTextOptions(&m_Options);

	return TRUE;
}


