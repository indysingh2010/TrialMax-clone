// FontName.cpp : implementation file
//

#include "stdafx.h"
#include "Tmtextvc.h"
#include "FontName.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

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


CFontName::CFontName(CWnd* pParent /*=NULL*/)
	: CDialog(CFontName::IDD, pParent)
{
	//{{AFX_DATA_INIT(CFontName)
	m_strFont = _T("");
	//}}AFX_DATA_INIT
}


void CFontName::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CFontName)
	DDX_Control(pDX, IDC_FONTS, m_ctrlFonts);
	DDX_LBString(pDX, IDC_FONTS, m_strFont);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CFontName, CDialog)
	//{{AFX_MSG_MAP(CFontName)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CFontName message handlers

BOOL CFontName::OnInitDialog() 
{
	LOGFONT	lfFont;
	CDC*	pdc = GetDC();

	CDialog::OnInitDialog();
	
	ZeroMemory(&lfFont, sizeof(lfFont));
	lfFont.lfCharSet = ANSI_CHARSET;
	EnumFontFamiliesEx(pdc->GetSafeHdc(), &lfFont, (FONTENUMPROC)EnumFontsProc, 
					   (LPARAM)&m_ctrlFonts, 0);
	m_ctrlFonts.SetCurSel(0);
	
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}
