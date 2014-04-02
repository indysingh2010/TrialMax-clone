//==============================================================================
//
// File Name:	pagebar.cpp
//
// Description:	This file contains member functions of the CPageBar class.
//
// See Also:	pagebar.h
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
#include <pagebar.h>
#include <xmlframe.h>
#include <xmlpage.h>

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
BEGIN_MESSAGE_MAP(CPageBar, CReBar)
	//{{AFX_MSG_MAP(CNavigate)
	ON_WM_SIZE()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CPageBar::Add()
//
// 	Description:	This function is called to add the specified band
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPageBar::Add(CPBBand* pBand)
{
	REBARBANDINFO rbbInfo;

	//	The child should have been created
	ASSERT(pBand != 0);
	ASSERT(IsWindow(pBand->m_hWnd));
	if((pBand == 0) || (!IsWindow(pBand->m_hWnd))) return FALSE;

	memset(&rbbInfo, 0, sizeof(rbbInfo));
	rbbInfo.cbSize = sizeof(rbbInfo);

	rbbInfo.fMask        =	RBBIM_SIZE | 
							RBBIM_CHILD | 
							RBBIM_CHILDSIZE | 
							RBBIM_ID | 
							RBBIM_STYLE |
							RBBIM_COLORS;

	rbbInfo.cyMinChild   = 32000;	//	Allow any height
	rbbInfo.cxMinChild   = pBand->GetMinimumWidth();
	rbbInfo.cx           = pBand->GetInitialWidth() > 0 ? pBand->GetInitialWidth() : 1;
	rbbInfo.fStyle       = pBand->GetResizable() ? RBBS_GRIPPERALWAYS : 0;
	rbbInfo.wID          = pBand->GetId();
	rbbInfo.hwndChild    = pBand->m_hWnd;
	rbbInfo.clrFore		 = m_crForeground;
	rbbInfo.clrBack		 = m_crBackground;

/*
if(pBand->GetId() == PAGEBAR_TOOLBAR_BAND)
{
	CString M;
	M.Format("%d", rbbInfo.cx);
	MessageBox(M);
}
*/
	return GetReBarCtrl().InsertBand(-1, &rbbInfo);
}

//==============================================================================
//
// 	Function Name:	CPageBar::Build()
//
// 	Description:	This function is called to build the bar by adding each
//					of the bands
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPageBar::Build()
{
	//	Has the window been created
	if(!IsWindow(m_hWnd)) return FALSE;

	//	Add each of the bands
	for(int i = 0; i < PAGEBAR_MAX_BANDS; i++)
	{
		if((m_aBands[i] != 0) && (IsWindow(m_aBands[i]->m_hWnd)))
		{
			if(!Add(m_aBands[i]))
				return FALSE;
		}
	}
		
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPageBar::CPageBar()
//
// 	Description:	This is the constructor for CPageBar objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPageBar::CPageBar() : CReBar()
{
	m_pXmlFrame = 0;
	//m_crBackground = RGB(0xC0, 0xC0, 0xC0);
	//m_crForeground = RGB(0x00, 0x00, 0x00);
	m_crBackground = ::GetSysColor(COLOR_3DFACE);
	m_crForeground = ::GetSysColor(COLOR_BTNTEXT);
	m_bVisible = FALSE;

	//	Initialize the bands array
	m_aBands[PAGEBAR_LEFT_BAND] = &m_pbLeft;
	m_aBands[PAGEBAR_TOOLBAR_BAND] = &m_pbToolbar;
	m_aBands[PAGEBAR_TEXT_BAND] = &m_pbText;
	m_aBands[PAGEBAR_LIST_BAND] = &m_pbList;
	m_aBands[PAGEBAR_RIGHT_BAND] = &m_pbRight;

	//	Set the id of each band
	for(int i = 0; i < PAGEBAR_MAX_BANDS; i++)
	{
		if(m_aBands[i] != 0)
			m_aBands[i]->SetId(i);
	}

	//	The first band does not have a resize bar
	if(m_aBands[0] != 0)
		m_aBands[0]->SetResizable(FALSE);
}

//==============================================================================
//
// 	Function Name:	CPageBar::~CPageBar()
//
// 	Description:	This is the destructor for CPageBar objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPageBar::~CPageBar()
{
}

//==============================================================================
//
// 	Function Name:	CPageBar::Create()
//
// 	Description:	This function is called to create the window.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPageBar::Create(CXmlFrame* pFrame)
{
	ASSERT(pFrame != 0);
	m_pXmlFrame = pFrame;

	//	Create the window
	if(!CReBar::Create(m_pXmlFrame, RBS_FIXEDORDER | RBS_BANDBORDERS | RBS_DBLCLKTOGGLE,
					   WS_CHILD | WS_CLIPSIBLINGS | WS_CLIPCHILDREN))
		return FALSE;

	//	Create the fonts used for the controls
	m_LargeFont.CreatePointFont(120, "MS Sans Serif");
	m_MediumFont.CreatePointFont(100, "MS Sans Serif");
	m_SmallFont.CreatePointFont(80, "MS Sans Serif");

	//	Create each of the bands
	for(int i = 0; ((i < PAGEBAR_MAX_BANDS) && (m_aBands[i] != 0)); i++)
	{
		if(!m_aBands[i]->Create(this))
			return FALSE;
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPageBar::EnableButton()
//
// 	Description:	This function is called to set the enable/disable the
//					specified toolbar button.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPageBar::EnableButton(short sId, BOOL bEnabled)
{
	//	Notify the toolbar band
	if(m_aBands[PAGEBAR_TOOLBAR_BAND] != 0)
		return ((CPBToolbar*)m_aBands[PAGEBAR_TOOLBAR_BAND])->EnableButton(sId, bEnabled);
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CPageBar::GetMinHeight()
//
// 	Description:	This function is called to get the minimum height required
//					for this window.
//
// 	Returns:		The minimum height in pixels
//
//	Notes:			None
//
//==============================================================================
int CPageBar::GetMinHeight()
{
	if(IsWindow(m_pbToolbar.m_hWnd))
		return m_pbToolbar.GetToolbarHeight();
	else
		return 0;
}

//==============================================================================
//
// 	Function Name:	CPageBar::Initialize()
//
// 	Description:	This function is called to initialize the window using
//					the specified ini file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CPageBar::Initialize(LPCSTR lpszFilename, LPCSTR lpszSection)
{
	CTMIni	Ini;
	int		iWidth;

	//	Open the ini file
	if(!Ini.Open(lpszFilename, lpszSection)) return FALSE;

	//	Get the initial width for each band
	for(int i = 0; ((i < PAGEBAR_MAX_BANDS) && (m_aBands[i] != 0)); i++)
	{
		if((iWidth = Ini.ReadLong(i)) > 0)
			m_aBands[i]->SetInitialWidth(iWidth);
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPageBar::OnBandSize()
//
// 	Description:	This function is called by a band when its size gets changed
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPageBar::OnBandSize(int iId)
{
	RECT	rcClient;
	RECT	rcBand;

	ASSERT(iId >= 0);
	ASSERT(iId < PAGEBAR_MAX_BANDS);
	if((iId < 0) || (iId >= PAGEBAR_MAX_BANDS)) return;

	//	Get the available client area
	GetClientRect(&rcClient);
	if(rcClient.bottom <= rcClient.top) return;

	//	Keep the width of the band as determined by the system but adjust the
	//	height to consume the full client area
	if((m_aBands[iId] != 0) && (IsWindow(m_aBands[iId]->m_hWnd)))
	{
		m_aBands[iId]->GetWindowRect(&rcBand);
		ScreenToClient(&rcBand);

		rcBand.top = 0;
		rcBand.bottom = rcClient.bottom;
		m_aBands[iId]->MoveWindow(&rcBand);
		m_aBands[iId]->RecalcLayout();
	}
}

//==============================================================================
//
// 	Function Name:	CPageBar::OnButtonClick()
//
// 	Description:	This function handles the event fired by the toolbar when
//					the user clicks on a button.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPageBar::OnButtonClick(short sId, BOOL bChecked) 
{
	//	Notify the frame window
	if(m_pXmlFrame)
		m_pXmlFrame->OnPageBarClick(sId, bChecked);	
}

//==============================================================================
//
// 	Function Name:	CPageBar::OnSelChanged()
//
// 	Description:	This function is called when the user makes a new selection
//					in the list box.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPageBar::OnSelChanged(CXmlPage* pXmlPage) 
{
	//	Notify the frame window
	if(m_pXmlFrame)
		m_pXmlFrame->OnPageBarChange(pXmlPage);
}

//==============================================================================
//
// 	Function Name:	CPageBar::OnSize()
//
// 	Description:	This function handles all WM_SIZE messages sent to the 
//					window
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPageBar::OnSize(UINT nType, int cx, int cy) 
{
	//	Perform the base class processing
	CReBar::OnSize(nType, cx, cy);
	
	//	Make sure everything is positioned properly
	RecalcLayout();
}

//==============================================================================
//
// 	Function Name:	CPageBar::RecalcLayout()
//
// 	Description:	This function is called to adjust the band positions.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPageBar::RecalcLayout()
{
	//	Make sure all bands are properly positioned
	for(int i = 0; i < PAGEBAR_MAX_BANDS; i++)
		OnBandSize(i);
}

//==============================================================================
//
// 	Function Name:	CPageBar::Save()
//
// 	Description:	This function is called to save the configuration to file
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CPageBar::Save(LPCSTR lpszFilename, LPCSTR lpszSection)
{
	CTMIni	Ini;
	RECT	rcBand;

	//	Open the ini file
	Ini.Open(lpszFilename, lpszSection);

	//	Update the ini file
	for(int i = 0; i < PAGEBAR_MAX_BANDS; i++)
	{
		if((m_aBands[i] != 0) && IsWindow(m_aBands[i]->m_hWnd))
		{
			GetReBarCtrl().GetRect(i, &rcBand);
			Ini.WriteLong(i, rcBand.right - rcBand.left);
		}
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPageBar::SetToolbarProps()
//
// 	Description:	This function is called to set the toolbar properties to
//					match those of the specified source toolbar.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CPageBar::SetToolbarProps(CTMTool& rSource)
{
	CFont* pFont;

	//	Notify the toolbar band
	if(m_aBands[PAGEBAR_TOOLBAR_BAND] != 0)
		((CPBToolbar*)m_aBands[PAGEBAR_TOOLBAR_BAND])->SetToolbarProps(rSource);

	//	Which font should we use?
	switch(rSource.GetButtonSize())
	{
		case TMTB_LARGEBUTTONS:

			pFont = &m_LargeFont;
			break;

		case TMTB_MEDIUMBUTTONS:

			pFont = &m_MediumFont;
			break;

		case TMTB_SMALLBUTTONS:
		default:

			pFont = &m_SmallFont;
			break;
	}

	//	Set the font for each band
	for(int i = 0; i < PAGEBAR_MAX_BANDS; i++)
	{
		if((m_aBands[i] != 0) && IsWindow(m_aBands[i]->m_hWnd))
			m_aBands[i]->SetFont(pFont);
	}
	
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPageBar::SetXmlMedia()
//
// 	Description:	This function is called to set the current document
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPageBar::SetXmlMedia(CXmlMedia* pXmlMedia)
{
	//	Notify each band
	for(int i = 0; i < PAGEBAR_MAX_BANDS; i++)
	{
		if(m_aBands[i] != 0)
			m_aBands[i]->SetXmlMedia(pXmlMedia);
	}
}

//==============================================================================
//
// 	Function Name:	CPageBar::SetXmlPage()
//
// 	Description:	This function is called to set the active Xml page
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPageBar::SetXmlPage(CXmlPage* pXmlPage)
{
	//	Notify each band
	for(int i = 0; i < PAGEBAR_MAX_BANDS; i++)
	{
		if(m_aBands[i] != 0)
			m_aBands[i]->SetXmlPage(pXmlPage);
	}
}


