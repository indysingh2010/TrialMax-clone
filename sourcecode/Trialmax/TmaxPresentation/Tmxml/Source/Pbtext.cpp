//==============================================================================
//
// File Name:	pbtext.cpp
//
// Description:	This file contains member functions of the CPBText class.
//
// See Also:	pbtext.h
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
#include <pbtext.h>
#include <xmlmedia.h>

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
BEGIN_MESSAGE_MAP(CPBText, CPBBand)
	//{{AFX_MSG_MAP(CPBText)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CPBText::CPBText()
//
// 	Description:	This is the constructor for CPBText objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPBText::CPBText(CPageBar* pPageBar) : CPBBand(pPageBar, CPBText::IDD)
{
	//{{AFX_DATA_INIT(CPBText)
	//}}AFX_DATA_INIT
	
	//	Set the default initial width
	m_iInitialWidth = 125;
}

//==============================================================================
//
// 	Function Name:	CPBText::~CPBText()
//
// 	Description:	This is the destructor for CPBText objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPBText::~CPBText()
{
}

//==============================================================================
//
// 	Function Name:	CPBText::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the 
//					dialog box controls and class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBText::DoDataExchange(CDataExchange* pDX)
{
	CPBBand::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPBText)
	DDX_Control(pDX, IDC_TEXT, m_ctrlText);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CPBText::RecalcLayout()
//
// 	Description:	This function is called to recalculate the size and 
//					position of controls in the band.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBText::RecalcLayout() 
{
	//	Do the base class processing
	CPBBand::RecalcLayout();
	
	//	Has the text window been created yet?
	if(IsWindow(m_ctrlText.m_hWnd))
	{
		m_ctrlText.MoveWindow(&m_rcWnd, FALSE);
		m_ctrlText.RedrawWindow();
	}
}

//==============================================================================
//
// 	Function Name:	CPBText::SetFont()
//
// 	Description:	This function is called to set the dialog font
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBText::SetFont(CFont* pFont)
{
	//	Do the base class processing
	CPBBand::SetFont(pFont);

	m_ctrlText.SetFont(pFont);
}

//==============================================================================
//
// 	Function Name:	CPBText::SetXmlMedia()
//
// 	Description:	This function is called to set the current document
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBText::SetXmlMedia(CXmlMedia* pXmlMedia)
{
	//	Do the base class processing first
	CPBBand::SetXmlMedia(pXmlMedia);

	//	Update the text
	Update();
}

//==============================================================================
//
// 	Function Name:	CPBText::SetXmlPage()
//
// 	Description:	This function is called to set the active Xml page
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBText::SetXmlPage(CXmlPage* pXmlPage)
{
	//	Do the base class processing first
	CPBBand::SetXmlPage(pXmlPage);

	//	Update the text
	Update();
}

//==============================================================================
//
// 	Function Name:	CPBText::Update()
//
// 	Description:	This function is called to update the text
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBText::Update()
{
	CString		strText;
	POSITION	Pos;
	CXmlPage*	pXmlPage;
	long		lPage = 0;

	if(!IsWindow(m_ctrlText.m_hWnd)) return;

	strText.Empty();

	//	Do we have valid objects?
	if((m_pXmlPage != 0) && (m_pXmlMedia != 0))
	{
		//	Get this page's position in the list
		Pos = m_pXmlMedia->m_Pages.GetHeadPosition();
		while(Pos != NULL)
		{
			//	Is this the page?
			if((pXmlPage = (CXmlPage*)m_pXmlMedia->m_Pages.GetNext(Pos)) == m_pXmlPage)
			{
				strText.Format("Page %ld of %ld", lPage + 1, m_pXmlMedia->m_Pages.GetCount());
				break;
			}
			else
			{
				//	Increment the page counter
				lPage++;
			}
		}
	
	}

	//	Set the text
	m_ctrlText.SetWindowText(strText);	
}

