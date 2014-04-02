//==============================================================================
//
// File Name:	config.cpp
//
// Description:	This file contains member functions of the CConfigure class.
//
// Functions:   CConfigure::CConfigure()
//				CConfigure::DoDataExchange()
//				CConfigure::GetCurSel()
//				CConfigure::OnClose()
//				CConfigure::OnInitDialog()
//				CConfigure::OnInsertAfter()
//				CConfigure::OnInsertBefore()
//				CConfigure::OnOK()
//				CConfigure::OnRemove()
//				CConfigure::OnTimer()
//				
// See Also:	config.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	05-04-98	1.30		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmtoolap.h>
#include <config.h>
#include <tmtool.h>
#include <resource.h>
#include <tables.h>

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
BEGIN_MESSAGE_MAP(CConfigure, CDialog)
	//{{AFX_MSG_MAP(CConfigure)
	ON_WM_TIMER()
	ON_BN_CLICKED(IDC_CONFIGURE_AFTER, OnInsertAfter)
	ON_BN_CLICKED(IDC_CONFIGURE_BEFORE, OnInsertBefore)
	ON_BN_CLICKED(IDC_CONFIGURE_REMOVE, OnRemove)
	ON_WM_CLOSE()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CConfigure::CConfigure()
//
// 	Description:	This is the constructor for CConfigure objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CConfigure::CConfigure(CTMToolCtrl* pParent, CString* pLabels) 
		   :CDialog(CConfigure::IDD, pParent)
{
	//{{AFX_DATA_INIT(CConfigure)
	m_bFlat = FALSE;
	m_iSize = -1;
	m_bToolTips = FALSE;
	m_iTop = -1;
	m_bStretch = FALSE;
	m_iRows = 0;
	//}}AFX_DATA_INIT

	m_pToolbar = pParent;
	m_pLabels  = pLabels;
	m_iImage   = -1;
	m_iToolbar = -1;
	memset(m_szMask, 0, sizeof(m_szMask));

	//	Initialize the mask to allow all buttons
	for(int i = 0; i < TMTB_MAXBUTTONS; i++)
		m_szMask[i] = '1';
}

//==============================================================================
//
// 	Function Name:	CConfigure::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					dialog box and class members
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CConfigure::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CConfigure)
	DDX_Control(pDX, IDC_CONFIGURE_REMOVE, m_ctrlRemove);
	DDX_Control(pDX, IDC_CONFIGURE_BEFORE, m_ctrlBefore);
	DDX_Control(pDX, IDC_CONFIGURE_AFTER, m_ctrlAfter);
	DDX_Control(pDX, IDC_CONFIGURE_IMAGES, m_ctrlImages);
	DDX_Control(pDX, IDC_CONFIGURE_TOOLBAR, m_ctrlToolbar);
	DDX_Check(pDX, IDC_CONFIGURE_FLAT, m_bFlat);
	DDX_Radio(pDX, IDC_CONFIGURE_SMALL, m_iSize);
	DDX_Check(pDX, IDC_CONFIGURE_TOOLTIPS, m_bToolTips);
	DDX_Radio(pDX, IDC_CONFIGURE_TOP, m_iTop);
	DDX_Check(pDX, IDC_CONFIGURE_STRETCH, m_bStretch);
	DDX_Text(pDX, IDC_ROWS, m_iRows);
	DDV_MinMaxInt(pDX, m_iRows, 1, 25);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CConfigure::DoComparison()
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
int CALLBACK CConfigure::DoComparison(LPARAM LPId1, LPARAM LPId2, 
									  LPARAM LPNotUsed)
{
	if(Sorted[LPId1] < Sorted[LPId2])
		return -1;
	else if(Sorted[LPId1] == Sorted[LPId2])
		return 0;
	else
		return 1;
}

//==============================================================================
//
// 	Function Name:	CConfigure::GetCurSel()
//
// 	Description:	This function will retrieve the index of the current 
//					selection in the list specified by the caller
//
// 	Returns:		The index of the current selection
//
//	Notes:			None
//
//==============================================================================
int CConfigure::GetCurSel(CListCtrl* pList)
{
	ASSERT(pList);

	return pList->GetNextItem(-1, LVNI_ALL | LVIS_SELECTED);
}

//==============================================================================
//
// 	Function Name:	CConfigure::OnClose()
//
// 	Description:	This function is called when the dialog is closed.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CConfigure::OnClose()
{
	//	Kill the timer 
	KillTimer(1);
	CDialog::OnClose();
}

//==============================================================================
//
// 	Function Name:	CConfigure::OnInitDialog()
//
// 	Description:	This function is called when the dialog box is created.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CConfigure::OnInitDialog() 
{
	int			i;
	int			iIndex;
	LV_FINDINFO	FindInfo;
	int			iTBList = 0;
	CBitmap		bmButtons;

	ASSERT(m_pToolbar);
	ASSERT(m_pLabels);

	//	Perform the base class processing
	CDialog::OnInitDialog();
	
	//	Set up the structure used to locate images
	FindInfo.flags  = LVFI_PARAM;
	FindInfo.psz    = 0;

	//	Load the bitmaps for the image lists
	bmButtons.LoadBitmap(IDB_TBSMALL);
	m_Images.Create(24, 18, ILC_MASK | ILC_COLOR24, 0, 1);
	m_Images.Add(&bmButtons, RGB(255,0,255));  

	//	Attach the image list to the list controls
	m_ctrlImages.SetImageList(&m_Images, LVSIL_SMALL);
	m_ctrlToolbar.SetImageList(&m_Images, LVSIL_SMALL);

	//	Place the images in the toolbar list
	for(i = 0; i < TMTB_MAXBUTTONS; i++)
	{
		if(m_aMap[i] < 0)
			break;
				
		if(m_aMap[i] >= TMTB_MAXBUTTONS)
			continue;

		//	Make sure this button is allowed
		if(m_szMask[m_aMap[i]] == '0')
			continue;

		iIndex = m_ctrlToolbar.InsertItem(i, m_pLabels[(m_aMap[i])], 
								          m_pToolbar->GetImageIndex(m_aMap[i]));
		m_ctrlToolbar.SetItemData(iIndex, (DWORD)m_aMap[i]);
	}

	//	Now place the images in the general list if they aren't already part
	//	of the toolbar
	for(i = 0; i < TMTB_MAXBUTTONS; i++)
	{
		//	Is this button allowed?
		if(m_szMask[i] == '0')
			continue;
		
		FindInfo.flags  = LVFI_PARAM;
		FindInfo.lParam = (DWORD)i;
		if(m_ctrlToolbar.FindItem(&FindInfo) >= 0)
			continue;

		m_ctrlImages.InsertItem(iTBList, m_pLabels[i], 
							    m_pToolbar->GetImageIndex(i));
		m_ctrlImages.SetItemData(iTBList, (DWORD)i);
		iTBList++;
	}

	m_ctrlImages.SetItemState(0, LVIS_SELECTED, LVIS_SELECTED);
	m_ctrlToolbar.SetItemState(0, LVIS_SELECTED, LVIS_SELECTED);

	//	Make sure the images are in the correct order
	m_ctrlImages.SortItems(DoComparison, 0);

	//	Start a timer to check the selections in each list
	SetTimer(1, 200, NULL);
	
	return TRUE;  
}

//==============================================================================
//
// 	Function Name:	CConfigure::OnInsertAfter()
//
// 	Description:	This function will insert the button image selected in the
//					image list into the toolbar list at the location after
//					the current selection
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CConfigure::OnInsertAfter() 
{
	int		iItem;
	int		iItems;
	int		iInsert;
	int		i;
	int		iReplace = 0;
	short	sId;
	short	Replace[TMTB_MAXBUTTONS];

	//	Do we have an image selected in the list?
	if((iItem = GetCurSel(&m_ctrlImages)) < 0)
	{
		MessageBeep(0xFFFFFFFF);
		return;
	}
	else
	{
		//	Get the command identifier
		sId = (short)m_ctrlImages.GetItemData(iItem);
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
	m_ctrlToolbar.InsertItem(iInsert, m_pLabels[sId], 
							 m_pToolbar->GetImageIndex(sId));
	m_ctrlToolbar.SetItemData(iInsert, (DWORD)sId);

	//	Where do we start putting back items?
	iItem = iInsert + 1;

	//	Now put the ones we removed back in the list
	for(i = 0; i < iReplace; i++)
	{
		sId = Replace[i];
		m_ctrlToolbar.InsertItem(iItem, m_pLabels[sId], 
							     m_pToolbar->GetImageIndex(sId));
		m_ctrlToolbar.SetItemData(iItem, (DWORD)sId);
		iItem++;
	} 
	
	m_ctrlToolbar.SetItemState(iInsert, LVIS_SELECTED, LVIS_SELECTED);
	m_ctrlToolbar.EnsureVisible(iInsert, FALSE);
}

//==============================================================================
//
// 	Function Name:	CConfigure::OnInsertBefore()
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
void CConfigure::OnInsertBefore() 
{
	int		iItem;
	int		iItems;
	int		iInsert;
	int		i;
	int		iReplace = 0;
	short	sId;
	short	Replace[TMTB_MAXBUTTONS];

	//	Do we have an image selected in the list?
	if((iItem = GetCurSel(&m_ctrlImages)) < 0)
	{
		MessageBeep(0xFFFFFFFF);
		return;
	}
	else
	{
		//	Get the command identifier
		sId = (short)m_ctrlImages.GetItemData(iItem);
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
	m_ctrlToolbar.InsertItem(iInsert, m_pLabels[sId], 
						     m_pToolbar->GetImageIndex(sId));
	m_ctrlToolbar.SetItemData(iInsert, (DWORD)sId);

	//	Where do we start putting back items?
	iItem = iInsert + 1;

	//	Now put the ones we removed back in the list
	for(i = 0; i < iReplace; i++)
	{
		sId = Replace[i];
		m_ctrlToolbar.InsertItem(iItem, m_pLabels[sId], 
							     m_pToolbar->GetImageIndex(sId));
		m_ctrlToolbar.SetItemData(iItem, (DWORD)sId);
		iItem++;
	} 
	
	m_ctrlToolbar.SetItemState(iInsert, LVIS_SELECTED, LVIS_SELECTED);
	m_ctrlToolbar.EnsureVisible(iInsert, FALSE);
}

//==============================================================================
//
// 	Function Name:	CConfigure::OnOK()
//
// 	Description:	This function is called when the user clicks on OK
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CConfigure::OnOK() 
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
	
	CDialog::OnOK();
}

//==============================================================================
//
// 	Function Name:	CConfigure::OnRemove()
//
// 	Description:	This function will remove the current selection from the
//					toolbar list and put it back in the image list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CConfigure::OnRemove() 
{
	short	sId;
	int		iImages;
	int		iItem;
	int		iIndex;

	//	Do we have an image selected in the toolbar?
	if((iItem = GetCurSel(&m_ctrlToolbar)) < 0)
	{
		MessageBeep(0xFFFFFFFF);
		return;
	}

	//	Get the command id for the selected image
	sId = (short)m_ctrlToolbar.GetItemData(iItem);
	
	//	Remove this item from the toolbar list
	m_ctrlToolbar.DeleteItem(iItem);
	if(iItem == m_ctrlToolbar.GetItemCount())
		iItem--;
	m_ctrlToolbar.SetItemState(iItem, LVIS_SELECTED, LVIS_SELECTED);
	m_ctrlToolbar.EnsureVisible(iItem, FALSE);

	//	Put it back in the image list
	iImages = m_ctrlImages.GetItemCount();
	iIndex = m_ctrlImages.InsertItem(iImages, m_pLabels[sId], 
							         m_pToolbar->GetImageIndex(sId));
	m_ctrlImages.SetItemData(iIndex, (DWORD)sId);
	m_ctrlImages.SortItems(DoComparison, 0);
}

//==============================================================================
//
// 	Function Name:	CConfigure::OnTimer()
//
// 	Description:	This function handles all WM_TIMER messages.
//
// 	Returns:		TRUE if successful
//
//	Notes:			This dialog uses a timer to check the selections in the
//					list boxes on a periodic basis and enable/disable the
//					associated controls. This is easier than keeping track of
//					the selections with change notifications
//
//==============================================================================
void CConfigure::OnTimer(UINT nIDEvent) 
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



