//==============================================================================
//
// File Name:	pptool.cpp
//
// Description:	This file contains member functions of the CPPToolbar class
//
// See Also:	pptool.h
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	06-17-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <pptool.h>
#include <tmtool.h>

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
static short _aSorted[TMTB_MAXBUTTONS];

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
IMPLEMENT_DYNCREATE(CPPToolbar, CPropertyPage)

BEGIN_MESSAGE_MAP(CPPToolbar, CPropertyPage)
	//{{AFX_MSG_MAP(CPPToolbar)
	ON_BN_CLICKED(IDC_PPTOOL_AFTER, OnAfter)
	ON_BN_CLICKED(IDC_PPTOOL_BEFORE, OnBefore)
	ON_BN_CLICKED(IDC_PPTOOL_REMOVE, OnRemove)
	ON_WM_CLOSE()
	ON_WM_TIMER()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CPPToolbar::CPPToolbar()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPToolbar::CPPToolbar() : CPropertyPage(CPPToolbar::IDD)
{
	//{{AFX_DATA_INIT(CPPToolbar)
	m_iOrientation = 0;
	m_iSize = 0;
	m_sRows = 1;
	m_bFlat = TRUE;
	//}}AFX_DATA_INIT

	m_pToolbar = 0;
	m_iImage   = -1;
	m_iToolbar = -1;
	m_bInitialized = FALSE;
	ZeroMemory(m_aMap, sizeof(m_aMap));
	ZeroMemory(m_szMask, sizeof(m_szMask));
}

//==============================================================================
//
// 	Function Name:	CPPToolbar::~CPPToolbar()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CPPToolbar::~CPPToolbar()
{
}

//==============================================================================
//
// 	Function Name:	CPPToolbar::DoComparison()
//
// 	Description:	This function is called by Windows to sort the items in a
//					list.
//
// 	Returns:		> 0 if LPItem1 is greater than LPItem2
//					= 0 if LPItem1 equals LPItem2
//					< 0 if LPItem1 is less than LPItem2
//
//	Notes:			None
//
//==============================================================================
int CALLBACK CPPToolbar::DoComparison(LPARAM LPId1, LPARAM LPId2, 
									  LPARAM LPNotUsed)
{
	if(_aSorted[LPId1] < _aSorted[LPId2])
		return -1;
	else if(_aSorted[LPId1] == _aSorted[LPId2])
		return 0;
	else
		return 1;
}

//==============================================================================
//
// 	Function Name:	CPPToolbar::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					dialog box controls and class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPToolbar::DoDataExchange(CDataExchange* pDX)
{
	CPropertyPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPPToolbar)
	DDX_Control(pDX, IDC_PPTOOL_REMOVE, m_ctrlRemove);
	DDX_Control(pDX, IDC_PPTOOL_BEFORE, m_ctrlBefore);
	DDX_Control(pDX, IDC_PPTOOL_AFTER, m_ctrlAfter);
	DDX_Control(pDX, IDC_PPTOOL_TOOLBAR, m_ctrlToolbar);
	DDX_Control(pDX, IDC_PPTOOL_IMAGES, m_ctrlImages);
	DDX_Radio(pDX, IDC_PPTOOL_BOTTOM, m_iOrientation);
	DDX_Radio(pDX, IDC_PPTOOL_SMALL, m_iSize);
	DDX_Text(pDX, IDC_PPTOOL_ROWS, m_sRows);
	DDV_MinMaxInt(pDX, m_sRows, 1, 20);
	DDX_Check(pDX, IDC_PPTOOL_FLAT, m_bFlat);
	//}}AFX_DATA_MAP
	
	//	Update the button map
	if(pDX->m_bSaveAndValidate)
		UpdateMap();
}

//==============================================================================
//
// 	Function Name:	CPPToolbar::FillImageLists()
//
// 	Description:	This function is called to populate the image list and
//					toolbar list.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPToolbar::FillImageLists()
{
	LV_FINDINFO		FindInfo;
	int				iTBList = 0;
	int				i;
	int				iIndex;
	
	//	Prevent updates while we change the options
	KillTimer(1);

	//	Set up the structure used to locate images
	FindInfo.flags  = LVFI_PARAM;
	FindInfo.psz    = 0;

	//	Place the images in the toolbar list
	for(i = 0; i < TMTB_MAXBUTTONS; i++)
	{
		if(m_aMap[i] < 0)
			break;
				
		if(m_aMap[i] >= TMTB_MAXBUTTONS)
			continue;

		//	Should we mask out this button?
		if(m_szMask[m_aMap[i]] == '0')
			continue;

		iIndex = m_ctrlToolbar.InsertItem(i, m_pToolbar->GetButtonLabel(m_aMap[i]), 
										  m_pToolbar->GetImageIndex(m_aMap[i]));
		m_ctrlToolbar.SetItemData(iIndex, (DWORD)m_aMap[i]);
	}

	//	Now place the images in the general list if they aren't already part
	//	of the toolbar
	for(i = 0; i < TMTB_MAXBUTTONS; i++)
	{
		FindInfo.lParam = i;
		if(m_ctrlToolbar.FindItem(&FindInfo) >= 0)
			continue;

		//	Should we mask out this button?
		if(m_szMask[i] == '0')
			continue;

		m_ctrlImages.InsertItem(iTBList, m_pToolbar->GetButtonLabel(i), 
								m_pToolbar->GetImageIndex(i));
		m_ctrlImages.SetItemData(iTBList, (DWORD)i);
		iTBList++;
	}

	m_ctrlImages.SortItems(DoComparison, 0);

	m_ctrlImages.SetItemState(0, LVIS_SELECTED, LVIS_SELECTED);
	m_ctrlToolbar.SetItemState(0, LVIS_SELECTED, LVIS_SELECTED);

	//	Start a timer to check the selections in each list
	SetTimer(1, 250, NULL);
}
 
//==============================================================================
//
// 	Function Name:	CPPToolbar::GetCurSel()
//
// 	Description:	This function will retrieve the index of the current 
//					selection in the list specified by the caller
//
// 	Returns:		The index of the current selection
//
//	Notes:			None
//
//==============================================================================
int CPPToolbar::GetCurSel(CListCtrl* pList)
{
	ASSERT(pList);

	return pList->GetNextItem(-1, LVNI_ALL | LVIS_SELECTED);
}

//==============================================================================
//
// 	Function Name:	CPPToolbar::OnAfter()
//
// 	Description:	This function will insert the button image selected in the
//					image list into the toolbar list at the location following
//					the current selection
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPToolbar::OnAfter() 
{
	int		iItem;
	int		iItems;
	int		iInsert;
	int		i;
	int		iReplace = 0;
	short	sImage;
	short	sCommand;
	short	Replace[TMTB_MAXBUTTONS];

	//	Do we have an image selected in the list?
	if((iItem = GetCurSel(&m_ctrlImages)) < 0)
	{
		MessageBeep(0xFFFFFFFF);
		return;
	}
	else
	{
		//	Get the image and command identifiers
		sCommand = (short)m_ctrlImages.GetItemData(iItem);
		sImage = m_pToolbar->GetImageIndex(sCommand);
	}

	//	Remove this image from the image list
	m_ctrlImages.DeleteItem(iItem);
	if(iItem >= m_ctrlImages.GetItemCount())
		iItem = m_ctrlImages.GetItemCount() - 1;
	m_ctrlImages.SetItemState(iItem, LVIS_SELECTED, LVIS_SELECTED);
	m_ctrlImages.EnsureVisible(iItem, FALSE);

	//	Get the index of the current selection in the toolbar list and the
	//	number of items in the list
	iItems = m_ctrlToolbar.GetItemCount();
	iInsert = GetCurSel(&m_ctrlToolbar) + 1;

	//	Delete all the items from the insertion point on
	for(iItem = iInsert; iItem < iItems; iItem++)
	{
		Replace[iReplace++] = (short)m_ctrlToolbar.GetItemData(iInsert);
		m_ctrlToolbar.DeleteItem(iInsert);
	}

	//	Add the new button
	m_ctrlToolbar.InsertItem(iInsert, m_pToolbar->GetButtonLabel(sCommand), 
						     sImage);
	m_ctrlToolbar.SetItemData(iInsert, (DWORD)sCommand);

	//	Where do we start putting back items?
	iItem = iInsert + 1;

	//	Now put the ones we removed back in the list
	for(i = 0; i < iReplace; i++)
	{
		sCommand = Replace[i];
		sImage = m_pToolbar->GetImageIndex(sCommand);
		m_ctrlToolbar.InsertItem(iItem, m_pToolbar->GetButtonLabel(sCommand), 
								 sImage);
		m_ctrlToolbar.SetItemData(iItem, (DWORD)sCommand);
		iItem++;
	} 
	
	m_ctrlToolbar.SetItemState(iInsert, LVIS_SELECTED, LVIS_SELECTED);
	m_ctrlToolbar.EnsureVisible(iInsert, FALSE);
}

//==============================================================================
//
// 	Function Name:	CPPToolbar::OnBefore()
//
// 	Description:	This function will insert the button image selected in the
//					image list into the toolbar list at the location before
//					the current selection
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPToolbar::OnBefore() 
{
	int		iItem;
	int		iItems;
	int		iInsert;
	int		i;
	int		iReplace = 0;
	short	sImage;
	short	sCommand;
	short	Replace[TMTB_MAXBUTTONS];

	//	Do we have an image selected in the list?
	if((iItem = GetCurSel(&m_ctrlImages)) < 0)
	{
		MessageBeep(0xFFFFFFFF);
		return;
	}
	else
	{
		//	Get the command and image identifiers
		sCommand = (short)m_ctrlImages.GetItemData(iItem);
		sImage = m_pToolbar->GetImageIndex(sCommand);
	}

	//	Remove this image from the image list
	m_ctrlImages.DeleteItem(iItem);
	if(iItem >= m_ctrlImages.GetItemCount())
		iItem = m_ctrlImages.GetItemCount() - 1;
	m_ctrlImages.SetItemState(iItem, LVIS_SELECTED, LVIS_SELECTED);
	m_ctrlImages.EnsureVisible(iItem, FALSE);

	//	Get the index of the current selection in the toolbar list and the
	//	number of items in the list
	if((iInsert = GetCurSel(&m_ctrlToolbar)) < 0)
		iInsert = 0;
	iItems = m_ctrlToolbar.GetItemCount();

	//	Delete all the items from the insertion point on
	for(iItem = iInsert; iItem < iItems; iItem++)
	{
		Replace[iReplace++] = (short)m_ctrlToolbar.GetItemData(iInsert);
		m_ctrlToolbar.DeleteItem(iInsert);
	}

	//	Add the new button
	m_ctrlToolbar.InsertItem(iInsert, m_pToolbar->GetButtonLabel(sCommand), 
							 sImage);
	m_ctrlToolbar.SetItemData(iInsert, (DWORD)sCommand);

	//	Where do we start putting back items?
	iItem = iInsert + 1;

	//	Now put the ones we removed back in the list
	for(i = 0; i < iReplace; i++)
	{
		sCommand = Replace[i];
		sImage = m_pToolbar->GetImageIndex(sCommand);
		m_ctrlToolbar.InsertItem(iItem, m_pToolbar->GetButtonLabel(sCommand), 
								 sImage);
		m_ctrlToolbar.SetItemData(iItem, (DWORD)sCommand);
		iItem++;
	} 
	
	m_ctrlToolbar.SetItemState(iInsert, LVIS_SELECTED, LVIS_SELECTED);
	m_ctrlToolbar.EnsureVisible(iInsert, FALSE);
}

//==============================================================================
//
// 	Function Name:	CPPToolbar::OnClose()
//
// 	Description:	This function is called when the page is closed.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPToolbar::OnClose()
{
	//	Kill the timer 
	KillTimer(1);
	CPropertyPage::OnClose();
}

//==============================================================================
//
// 	Function Name:	CPPToolbar::OnInitDialog()
//
// 	Description:	This function handles all WM_INITDIALOG messages
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CPPToolbar::OnInitDialog() 
{
	CBitmap	bmButtons;

	//	Do the base class processing first
	CPropertyPage::OnInitDialog();
	
	ASSERT(m_pToolbar);
	if(m_pToolbar == 0)
		return TRUE;

	//	Get the sorted command identifiers
	m_pToolbar->GetSortOrder(_aSorted);

	//	Load the bitmaps for the image lists
	bmButtons.LoadBitmap(IDB_TBBITMAPS);
	m_Images.Create(24, 18, ILC_COLOR16, 0, 1);
	m_Images.Add(&bmButtons, RGB(0,0,0));  

	//	Attach the image list to the list controls
	m_ctrlImages.SetImageList(&m_Images, LVSIL_SMALL);
	m_ctrlToolbar.SetImageList(&m_Images, LVSIL_SMALL);

	//	Get copies of the button mask and button map
	lstrcpyn(m_szMask, m_pToolbar->GetButtonMask(), sizeof(m_szMask));
	m_pToolbar->GetButtonMap(m_aMap);

	//	Initialize the orientation and size controls
	m_iOrientation = m_pToolbar->GetOrientation();
	m_iSize = m_pToolbar->GetButtonSize();
	m_sRows = m_pToolbar->GetButtonRows();
	m_bFlat = m_pToolbar->GetStyle() == TMTB_FLAT;
	UpdateData(FALSE);

	FillImageLists();

	m_bInitialized = TRUE;

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CPPToolbar::OnRemove()
//
// 	Description:	This function will remove the current selection from the
//					toolbar list and put it back in the image list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPToolbar::OnRemove() 
{
	short	sImage;
	short	sCommand;
	int		iImages;
	int		iItem;

	//	Do we have an image selected in the toolbar?
	if((iItem = GetCurSel(&m_ctrlToolbar)) < 0)
	{
		MessageBeep(0xFFFFFFFF);
		return;
	}

	//	Get the command id and image index for the selected image
	sCommand = (short)m_ctrlToolbar.GetItemData(iItem);
	sImage = m_pToolbar->GetImageIndex(sCommand);
	ASSERT(sImage >= 0);
	ASSERT(sCommand >= 0);
		
	//	Remove this item from the toolbar list
	m_ctrlToolbar.DeleteItem(iItem);
	if(iItem == m_ctrlToolbar.GetItemCount())
		iItem--;
	m_ctrlToolbar.SetItemState(iItem, LVIS_SELECTED, LVIS_SELECTED);
	m_ctrlToolbar.EnsureVisible(iItem, FALSE);

	//	Put it back in the image list
	iImages = m_ctrlImages.GetItemCount();
	m_ctrlImages.InsertItem(iImages, m_pToolbar->GetButtonLabel(sCommand), 
						    sImage);
	m_ctrlImages.SetItemData(iImages, (DWORD)sCommand);
	m_ctrlImages.SortItems(DoComparison, 0);
}

//==============================================================================
//
// 	Function Name:	CPPToolbar::OnTimer()
//
// 	Description:	This function handles all WM_TIMER messages.
//
// 	Returns:		None
//
//	Notes:			This page uses a timer to check the selections in the
//					list boxes on a periodic basis and enable/disable the
//					associated controls. This is easier than keeping track of
//					the selections with change notifications
//
//==============================================================================
void CPPToolbar::OnTimer(UINT nIDEvent) 
{
	//	Do we have an image selected?
	if(GetCurSel(&m_ctrlImages) < 0)
	{
		m_ctrlBefore.EnableWindow(FALSE);
		m_ctrlAfter.EnableWindow(FALSE);
	}
	else
	{
		m_ctrlBefore.EnableWindow(TRUE);
		m_ctrlAfter.EnableWindow(TRUE);
	}

	//	Do we have a toolbar button selected?
	if(GetCurSel(&m_ctrlToolbar) < 0)
	{
		m_ctrlRemove.EnableWindow(FALSE);
	}
	else
	{
		m_ctrlRemove.EnableWindow(TRUE);
	}
}

//==============================================================================
//
// 	Function Name:	CPPToolbar::UpdateMap()
//
// 	Description:	This function will reconstruct the button map using the
//					toolbar list.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CPPToolbar::UpdateMap()
{
	int iItems;

	//	How many items are in the list?
	iItems = m_ctrlToolbar.GetItemCount();
	if(iItems > TMTB_MAXBUTTONS)
		iItems = TMTB_MAXBUTTONS;

	//	Mark the end of the toolbar in the map
	if(iItems < TMTB_MAXBUTTONS)
		m_aMap[iItems] = -1;

	//	Rebuild the map
	for(int i = 0; i < iItems; i++)
		m_aMap[i] = (short)m_ctrlToolbar.GetItemData(i);
}




