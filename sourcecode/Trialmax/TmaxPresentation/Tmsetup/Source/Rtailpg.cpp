//==============================================================================
//
// File Name:	rtailpg.cpp
//
// Description:	This file contains member functions of the CRingtailPage class.
//
// See Also:	rtailpg.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	06-07-03	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmsetap.h>
#include <rtailpg.h>
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
BEGIN_MESSAGE_MAP(CRingtailPage, CSetupPage)
	//{{AFX_MSG_MAP(CRingtailPage)
	ON_BN_CLICKED(IDC_RINGTAIL_REDACTCOLOR, OnRedactColor)
	ON_BN_CLICKED(IDC_RINGTAIL_REDACTLABELCOLOR, OnRedactLabelColor)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	EnumRingtailFonts()
//
// 	Description:	This callback is used by the system to enumerate the fonts
//
// 	Returns:		Non-zero to continue
//
//	Notes:			None
//
//==============================================================================
int CALLBACK EnumRingtailFonts(ENUMLOGFONTEX* lplfFont, NEWTEXTMETRIC* lptmFont,
						   int iType, LPARAM lParam)
{
	CListBox* pList = (CListBox*)lParam;

	if(pList)
		pList->AddString(lplfFont->elfLogFont.lfFaceName);
	return 1;
}

//==============================================================================
//
// 	Function Name:	CRingtailPage::CRingtailPage()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CRingtailPage::CRingtailPage(CWnd* pParent) : CSetupPage(CRingtailPage::IDD, pParent)
{
	//{{AFX_DATA_INIT(CRingtailPage)
	m_strRedactLabelFont = _T("");
	m_sRedactLabelSize = 0;
	m_iRedactTransparency = -1;
	m_bShowRedactions = FALSE;
	//}}AFX_DATA_INIT

	m_crRedact = RGB(0,0,0);
	m_crRedactLabel = RGB(255,255,255);
}

//==============================================================================
//
// 	Function Name:	CRingtailPage::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the class
//					members and the associated dialog box controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CRingtailPage::DoDataExchange(CDataExchange* pDX)
{
	CSetupPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CRingtailPage)
	DDX_Control(pDX, IDC_RINGTAIL_REDACTCOLOR, m_RedactColor);
	DDX_Control(pDX, IDC_RINGTAIL_REDACTLABELCOLOR, m_RedactLabelColor);
	DDX_Control(pDX, IDC_REDACTLABEL_FONTS, m_ctrlRedactLabelFonts);
	DDX_LBString(pDX, IDC_REDACTLABEL_FONTS, m_strRedactLabelFont);
	DDX_Text(pDX, IDC_RINGTAIL_REDACTLABELSIZE, m_sRedactLabelSize);
	DDX_CBIndex(pDX, IDC_RINGTAIL_REDACTTRANSPARENCY, m_iRedactTransparency);
	DDX_Check(pDX, IDC_RINGTAIL_SHOWREDACTIONS, m_bShowRedactions);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CRingtailPage::OnRedactColor()
//
// 	Description:	This function is called to change the redact color
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CRingtailPage::OnRedactColor() 
{
	SetColor(&m_crRedact, &m_RedactColor);	
}

//==============================================================================
//
// 	Function Name:	CRingtailPage::OnRedactLabelColor()
//
// 	Description:	This function is called to change the redact label color
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CRingtailPage::OnRedactLabelColor() 
{
	SetColor(&m_crRedactLabel, &m_RedactLabelColor);	
}

//==============================================================================
//
// 	Function Name:	CRingtailPage::OnInitDialog()
//
// 	Description:	This will initialize this page
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CRingtailPage::OnInitDialog() 
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
						  (FONTENUMPROC)EnumRingtailFonts, 
						  (LPARAM)&m_ctrlRedactLabelFonts, 0);
		ReleaseDC(pdc);
	}
	
	//	Select the current font
	if((iIndex = m_ctrlRedactLabelFonts.FindStringExact(-1, m_strRedactLabelFont)) != LB_ERR)
	{
		m_ctrlRedactLabelFonts.SetCurSel(iIndex);
		m_ctrlRedactLabelFonts.SetTopIndex(iIndex);
	}

	//	Set the button colors
	m_RedactColor.SetColor(m_crRedact);
	m_RedactLabelColor.SetColor(m_crRedactLabel);
	
	return TRUE;  
}

//==============================================================================
//
// 	Function Name:	CRingtailPage::SetColor()
//
// 	Description:	This function will open set the color reference provided by
//					the caller
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CRingtailPage::SetColor(COLORREF* pColor, CColorPushbutton* pButton) 
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
// 	Function Name:	CRingtailPage::ReadOptions()
//
// 	Description:	This function is called to read the page options from the
//					ini file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CRingtailPage::ReadOptions(CTMIni& rIni)
{
	SRingtailOptions Options;

	//	Read the options from the ini file
	rIni.ReadRingtailOptions(&Options);

	//	Set the class members
	m_bShowRedactions = Options.bShowRedactions;
	m_iRedactTransparency = Options.sRedactTransparency;
	m_sRedactLabelSize = Options.sLabelFontSize;
	m_crRedact = theControl->TranslateColor(Options.lRedactColor);
	m_crRedactLabel = theControl->TranslateColor(Options.lRedactLabelColor);
	m_strRedactLabelFont = Options.strLabelFontName;

	//	Update the controls
	if(IsWindow(m_hWnd))
	{
		UpdateData(FALSE);

		//	Update the button colors
		m_RedactColor.SetColor(m_crRedact);
		m_RedactLabelColor.SetColor(m_crRedactLabel);

		m_RedactColor.RedrawWindow();
		m_RedactLabelColor.RedrawWindow();
	}

}

//==============================================================================
//
// 	Function Name:	CRingtailPage::WriteOptions()
//
// 	Description:	This function is called to write the page options to the
//					ini file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CRingtailPage::WriteOptions(CTMIni& rIni)
{
	SRingtailOptions Options;

	//	Refresh the class members
	UpdateData(TRUE);


	//	Fill the transfer structure
	Options.bShowRedactions  = m_bShowRedactions;
	Options.sRedactTransparency = m_iRedactTransparency;
	Options.sLabelFontSize = m_sRedactLabelSize;
	Options.lRedactColor = (OLE_COLOR)m_crRedact;
	Options.lRedactLabelColor = (OLE_COLOR)m_crRedactLabel;
	Options.strLabelFontName = m_strRedactLabelFont;

	//	Write the options to the ini file
	rIni.WriteRingtailOptions(&Options);

	return TRUE;
}

