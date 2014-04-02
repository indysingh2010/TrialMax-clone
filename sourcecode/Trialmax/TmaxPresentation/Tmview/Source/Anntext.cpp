//==============================================================================
//
// File Name:	anntext.cpp
//
// Description:	This file contains member functions of the CAnnTextDlg class.
//
// See Also:	anntext.h 
//
// Copyright FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	10-01-99	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <anntext.h>

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
BEGIN_MESSAGE_MAP(CAnnTextDlg, CDialog)
	//{{AFX_MSG_MAP(CAnnTextDlg)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	EnumFontsProc()
//
// 	Description:	This callback function is used to fill the list box with
//					the names of fonts currently installed in the system
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
int CALLBACK EnumFontsProc(ENUMLOGFONTEX* lplfFont,
						   NEWTEXTMETRIC* lptmFont,
						   int iType, LPARAM lParam)
{
	CListBox* pList = (CListBox*)lParam;

	if(!pList)
		return 1;

	pList->AddString(lplfFont->elfLogFont.lfFaceName);
	return 1;
}

//==============================================================================
//
// 	Function Name:	CAnnTextDlg::CAnnTextDlg()
//
// 	Description:	This is the constructor for CAnnTextDlg objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CAnnTextDlg::CAnnTextDlg(CWnd* pParent) : CDialog(CAnnTextDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CAnnTextDlg)
	m_strAnnText = _T("");
	m_bBold = FALSE;
	m_strName = _T("");
	m_sSize = 0;
	m_bStrikeThrough = FALSE;
	m_bUnderline = FALSE;
	//}}AFX_DATA_INIT
}

//==============================================================================
//
// 	Function Name:	CAnnTextDlg::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the 
//					dialog box and class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CAnnTextDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAnnTextDlg)
	DDX_Control(pDX, IDC_FONTNAME, m_ctrlNames);
	DDX_Text(pDX, IDC_ANNTEXT, m_strAnnText);
	DDX_Check(pDX, IDC_BOLD, m_bBold);
	DDX_LBString(pDX, IDC_FONTNAME, m_strName);
	DDX_Text(pDX, IDC_FONTSIZE, m_sSize);
	DDX_Check(pDX, IDC_STRIKETHROUGH, m_bStrikeThrough);
	DDX_Check(pDX, IDC_UNDERLINE, m_bUnderline);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CAnnTextDlg::OnInitDialog()
//
// 	Description:	This function is called by the framework to initialize the
//					dialog box.
//
// 	Returns:		TRUE for default keyboard focus
//
//	Notes:			None
//
//==============================================================================
BOOL CAnnTextDlg::OnInitDialog() 
{
	LOGFONT	lfFont;
	CDC*	pdc = GetDC();

	//	Do the base class processing first
	CDialog::OnInitDialog();
	
	//	Fill the list box with the names of all registered fonts
	ZeroMemory(&lfFont, sizeof(lfFont));
	lfFont.lfCharSet = DEFAULT_CHARSET;
	EnumFontFamiliesEx(pdc->GetSafeHdc(), &lfFont, (FONTENUMPROC)EnumFontsProc, 
					   (LPARAM)&m_ctrlNames, 0);
	
	//	Set the current selection
	if(m_ctrlNames.SelectString(-1, m_strName) == LB_ERR)
		m_ctrlNames.SetCurSel(0);

	return TRUE;  
}
