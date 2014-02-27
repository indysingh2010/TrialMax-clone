//==============================================================================
//
// File Name:	pblist.cpp
//
// Description:	This file contains member functions of the CPBList class.
//
// See Also:	pblist.h
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
#include <pblist.h>
#include <pagebar.h>
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
BEGIN_MESSAGE_MAP(CPBList, CPBBand)
	//{{AFX_MSG_MAP(CPBList)
	ON_CBN_SELCHANGE(IDC_PAGES, OnSelChanged)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CPBList::CPBList()
//
// 	Description:	This is the constructor for CPBList objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPBList::CPBList(CPageBar* pPageBar) : CPBBand(pPageBar, CPBList::IDD)
{
	//{{AFX_DATA_INIT(CPBList)
	//}}AFX_DATA_INIT

	//	Set the default initial width
	m_iInitialWidth = 150;
}

//==============================================================================
//
// 	Function Name:	CPBList::~CPBList()
//
// 	Description:	This is the destructor for CPBList objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPBList::~CPBList()
{
}

//==============================================================================
//
// 	Function Name:	CPBList::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the 
//					dialog box controls and class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBList::DoDataExchange(CDataExchange* pDX)
{
	CPBBand::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPBList)
	DDX_Control(pDX, IDC_PAGES, m_ctrlPages);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CPBList::SetFont()
//
// 	Description:	This function is called to set the dialog font
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBList::SetFont(CFont* pFont)
{
	//	Do the base class processing
	CPBBand::SetFont(pFont);

	m_ctrlPages.SetFont(pFont);
}

//==============================================================================
//
// 	Function Name:	CPBList::SetXmlMedia()
//
// 	Description:	This function is called to set the current document
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBList::SetXmlMedia(CXmlMedia* pXmlMedia)
{
	//	Do the base class processing first
	CPBBand::SetXmlMedia(pXmlMedia);

	//	Refresh the list box
	if(IsWindow(m_ctrlPages.m_hWnd))
		FillPageList();
}

//==============================================================================
//
// 	Function Name:	CPBList::FillPageList()
//
// 	Description:	This function is called to fill the drop down list of
//					pages.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBList::FillPageList()
{
	POSITION	Pos;
	CXmlPage*	pXmlPage;
	CString		strText;
	int			iIndex;

	if(!IsWindow(m_ctrlPages.m_hWnd)) return;

	//	Clear the existing list
	m_ctrlPages.ResetContent();
	m_pXmlPage = 0;

	//	Do we have an Xml document?
	if(m_pXmlMedia != 0)
	{
		//	Iterate the list of pages
		Pos = m_pXmlMedia->m_Pages.GetHeadPosition();
		while(Pos != NULL)
		{
			if((pXmlPage = (CXmlPage*)m_pXmlMedia->m_Pages.GetNext(Pos)) != 0)
			{
				//	Get the text used to display the page
				pXmlPage->GetDisplayText(strText);

				//	Add this page to the list
				if((iIndex = m_ctrlPages.AddString(strText)) != LB_ERR)
				{
					m_ctrlPages.SetItemData(iIndex, (DWORD)pXmlPage);
				}
			}
		}
	
	}// if(m_pXmlPages != 0)
}

//==============================================================================
//
// 	Function Name:	CPBList::GetPageIndex()
//
// 	Description:	This function is called to get the index of the specified
//					page in the drop down list box
//
// 	Returns:		The list box index if found
//
//	Notes:			None
//
//==============================================================================
int CPBList::GetPageIndex(CXmlPage* pXmlPage)
{
	int	iPages;

	if(pXmlPage == 0 || !IsWindow(m_ctrlPages.m_hWnd)) return -1;

	//	How many pages are in the list box?
	iPages = m_ctrlPages.GetCount();
	
	//	Check each page in the list
	for(int i = 0; i < iPages; i++)
	{
		if((CXmlPage*)m_ctrlPages.GetItemData(i) == pXmlPage)
			return i;
	}

	//	Not found
	return -1;
}

//==============================================================================
//
// 	Function Name:	CPBList::OnSelChanged()
//
// 	Description:	This function is called when the user makes a new selection
//					in the list box.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBList::OnSelChanged() 
{
	int			iIndex;
	CXmlPage*	pXmlPage;

	//	Get the index of the new selection
	if((iIndex = m_ctrlPages.GetCurSel()) >= 0)
	{
		//	Get the xml page
		if((pXmlPage = (CXmlPage*)m_ctrlPages.GetItemData(iIndex)) != 0)
		{
			//	Notify the rebar window
			if(m_pPageBar)
				m_pPageBar->OnSelChanged(pXmlPage);
		}
	}	
}

//==============================================================================
//
// 	Function Name:	CPBList::RecalcLayout()
//
// 	Description:	This function is called to recalculate the size and 
//					position of controls in the band.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBList::RecalcLayout() 
{
	int		iTotalHeight;
	int		iEditHeight;
	int		iCy;
	RECT	rcPages;

	//	Do the base class processing
	CPBBand::RecalcLayout();
	
	//	Has the list box been created yet?
	if(IsWindow(m_ctrlPages.m_hWnd))
	{
		m_ctrlPages.GetWindowRect(&rcPages);
		iTotalHeight = rcPages.bottom - rcPages.top;
		iEditHeight  = m_ctrlPages.GetItemHeight(-1) + (2 * GetSystemMetrics(SM_CYEDGE));

		//	Get the verticle center point of the client area
		iCy = m_rcWnd.top + ((m_rcWnd.bottom - m_rcWnd.top) / 2);

		//	Center the edit box portion of the combobox vertically
		//	within the client area
		if(iEditHeight < (m_rcWnd.bottom - m_rcWnd.top))
			rcPages.top = iCy - (iEditHeight / 2);
		else
			rcPages.top = 0;

		rcPages.bottom = rcPages.top + iTotalHeight;
		rcPages.left   = m_rcWnd.left;
		rcPages.right  = m_rcWnd.right;

		m_ctrlPages.MoveWindow(&rcPages);	
	}	

}

//==============================================================================
//
// 	Function Name:	CPBList::SetXmlPage()
//
// 	Description:	This function is called to set the active Xml page
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPBList::SetXmlPage(CXmlPage* pXmlPage)
{
	int iIndex;

	//	Do the base class processing first
	CPBBand::SetXmlPage(pXmlPage);

	//	Find the index of the specified page in the drop down list
	if((iIndex = GetPageIndex(pXmlPage)) >= 0)
	{
		m_pXmlPage = pXmlPage;
		m_ctrlPages.SetCurSel(iIndex);
	}
	else
	{
		ASSERT(0);
		m_pXmlPage = 0;
		m_ctrlPages.SetCurSel(-1);
	}

}


