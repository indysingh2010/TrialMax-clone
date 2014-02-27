//==============================================================================
//
// File Name:	mediactl.cpp
//
// Description:	This file contains member functions of the CMediaCtrl class
//
// See Also:	mediactl.h
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	05-28-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <mediactl.h>
#include <xmlmedia.h>
#include <xmlpage.h>
#include <xmltreat.h>

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
BEGIN_MESSAGE_MAP(CMediaCtrl, CTreeCtrl)
	//{{AFX_MSG_MAP(CMediaCtrl)
	ON_NOTIFY_REFLECT(TVN_GETDISPINFO, OnGetDispInfo)
	ON_NOTIFY_REFLECT_EX(TVN_SELCHANGED, OnSelChanged)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CMediaCtrlItem::CMediaCtrlItem()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMediaCtrlItem::CMediaCtrlItem()
{
	m_hItem = 0;
	m_bQueued = FALSE;
	m_iAction = MEDIACTRL_ACTION_NONE;
	m_iXmlType = 0;
	m_dwUser1 = 0;
	m_dwUser2 = 0;
}

//==============================================================================
//
// 	Function Name:	CMediaCtrl::CMediaCtrl()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMediaCtrl::CMediaCtrl() : CTreeCtrl()
{
	m_pXmlTrees = 0;
	m_pXmlTree = 0;
	m_pXmlMedia = 0;
	m_pXmlPage = 0;
	m_pXmlTreatment = 0;
}

//==============================================================================
//
// 	Function Name:	CMediaCtrl::~CMediaCtrl()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMediaCtrl::~CMediaCtrl()
{
}

//==============================================================================
//
// 	Function Name:	CMediaCtrl::DeleteChildren()
//
// 	Description:	This function will remove all children of the parent item
//					provided by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMediaCtrl::DeleteChildren(HTREEITEM hParent) 
{
	HTREEITEM hItem;
	if((hItem = GetChildItem(hParent)) == NULL)
		return;

	while(1)
	{
		HTREEITEM hNextItem = GetNextSiblingItem(hItem);
		
		DeleteItem(hItem);
		hItem = hNextItem;

		if(hItem == NULL)
			break;
	}
}

//==============================================================================
//
// 	Function Name:	CMediaCtrl::EnsureVisible()
//
// 	Description:	This function will ensure that the specified item is visible
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMediaCtrl::EnsureVisible(CMediaCtrlItem* pItem)
{
	//	Make sure we have a valid handle
	if(pItem != 0 && pItem->m_hItem != 0)
	{
		CTreeCtrl::EnsureVisible(pItem->m_hItem);
	}
}

//==============================================================================
//
// 	Function Name:	CTMDBTree::GetDispInfo()
//
// 	Description:	This function is called when the control needs information
//					to display a media object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMediaCtrl::GetDispInfo(TV_DISPINFO* pInfo, CXmlMedia* pMedia) 
{
	//	Does the control need the text string?
	if(pInfo->item.mask & TVIF_TEXT)
	{
		if(pMedia->m_strTitle.GetLength() > 0)
			lstrcpyn(pInfo->item.pszText, pMedia->m_strTitle, 
					 pInfo->item.cchTextMax);
		else
			lstrcpyn(pInfo->item.pszText, pMedia->m_strId, 
					 pInfo->item.cchTextMax);
	}

	//	Does it need the icon index?
	if(pInfo->item.mask & TVIF_IMAGE)
		pInfo->item.iImage = GetIconIndex(pMedia);

	//	Does it need the selected icon index?
	if(pInfo->item.mask & TVIF_SELECTEDIMAGE)
		pInfo->item.iSelectedImage = GetIconIndex(pMedia);
}

//==============================================================================
//
// 	Function Name:	CTMDBTree::GetDispInfo()
//
// 	Description:	This function is called when the control needs information
//					to display a page.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMediaCtrl::GetDispInfo(TV_DISPINFO* pInfo, CXmlPage* pPage) 
{
	//	Does the control need the text string?
	if(pInfo->item.mask & TVIF_TEXT)
		pPage->GetDisplayText(pInfo->item.pszText, pInfo->item.cchTextMax);

	//	Does it need the icon index?
	if(pInfo->item.mask & TVIF_IMAGE)
		pInfo->item.iImage = GetIconIndex(pPage);

	//	Does it need the selected icon index?
	if(pInfo->item.mask & TVIF_SELECTEDIMAGE)
		pInfo->item.iSelectedImage = GetIconIndex(pPage);
}

//==============================================================================
//
// 	Function Name:	CTMDBTree::GetDispInfo()
//
// 	Description:	This function is called when the control needs information
//					to display a media tree.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMediaCtrl::GetDispInfo(TV_DISPINFO* pInfo, CXmlMediaTree* pTree) 
{
	//	Does the control need the text string?
	if(pInfo->item.mask & TVIF_TEXT)
		lstrcpyn(pInfo->item.pszText, pTree->m_strName, 
				 pInfo->item.cchTextMax);

	//	Does it need the icon index?
	if(pInfo->item.mask & TVIF_IMAGE)
		pInfo->item.iImage = GetIconIndex(pTree);

	//	Does it need the selected icon index?
	if(pInfo->item.mask & TVIF_SELECTEDIMAGE)
		pInfo->item.iSelectedImage = GetIconIndex(pTree);
}

//==============================================================================
//
// 	Function Name:	CTMDBTree::GetDispInfo()
//
// 	Description:	This function is called when the control needs information
//					to display a treatment.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMediaCtrl::GetDispInfo(TV_DISPINFO* pInfo, CXmlTreatment* pTreatment) 
{
	//	Does the control need the text string?
	if(pInfo->item.mask & TVIF_TEXT)
		lstrcpyn(pInfo->item.pszText, pTreatment->m_strSource, 
				 pInfo->item.cchTextMax);

	//	Does it need the icon index?
	if(pInfo->item.mask & TVIF_IMAGE)
		pInfo->item.iImage = GetIconIndex(pTreatment);

	//	Does it need the selected icon index?
	if(pInfo->item.mask & TVIF_SELECTEDIMAGE)
		pInfo->item.iSelectedImage = GetIconIndex(pTreatment);
}

//==============================================================================
//
// 	Function Name:	CMediaCtrl::GetIconIndex()
//
// 	Description:	This function is called to get the index of the icon for
//					the specified item
//
// 	Returns:		The index within the image strip
//
//	Notes:			None
//
//==============================================================================
int CMediaCtrl::GetIconIndex(CMediaCtrlItem* pItem)
{
	//	What is the current action?
	switch(pItem->m_iAction)
	{
		case MEDIACTRL_ACTION_ADD:

			return MEDIACTRL_ICON_ADD;

		case MEDIACTRL_ACTION_REMOVE:

			return MEDIACTRL_ICON_REMOVE;

		case MEDIACTRL_ACTION_NONE:
		default:

			//	Is this object in the queue?
			if(pItem->m_bQueued)
			{
				return MEDIACTRL_ICON_QUEUED;
			}
			else
			{
				//	What type of item is this?
				switch(pItem->m_iXmlType)
				{
					case MEDIACTRL_TREE:

						return MEDIACTRL_ICON_TREE;

					case MEDIACTRL_MEDIA:

						return MEDIACTRL_ICON_MEDIA;

					case MEDIACTRL_PAGE:

						return MEDIACTRL_ICON_PAGE;

					case MEDIACTRL_TREATMENT:

						return MEDIACTRL_ICON_TREATMENT;

					default:

						ASSERT(0);
						return 0;
				}

			}
	}
}

//==============================================================================
//
// 	Function Name:	CMediaCtrl::Initialize()
//
// 	Description:	This function is called to set the list of XmlMediaTrees
//					displayed by this control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMediaCtrl::Initialize(CXmlMediaTrees* pTrees)
{
	POSITION		Pos;
	CXmlMediaTree*	pTree;

	//	Create the image list
	m_Images.Create(IDB_MEDIA_CTRL, 16, 0, RGB(255, 0, 255));

	//	Attach the system image list to this control
	SetImageList(&m_Images, TVSIL_NORMAL);

	//	Save the list of trees
	m_pXmlTrees = pTrees;

	//	Add each Xml Media Tree
	if(m_pXmlTrees != 0)
	{
		Pos = m_pXmlTrees->GetHeadPosition();
		while(Pos != NULL)
		{
			if((pTree = (CXmlMediaTree*)m_pXmlTrees->GetNext(Pos)) != 0)
				Insert(pTree);
		}
	}

}

//==============================================================================
//
// 	Function Name:	CMediaCtrl::Insert()
//
// 	Description:	This function is called to insert the CXmlPage object
//					into the tree.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMediaCtrl::Insert(CXmlMedia* pMedia, CXmlPage* pPage)
{
	TV_INSERTSTRUCT	tvInsert;
	POSITION		Pos;
	CXmlTreatment*	pTreatment;

	ASSERT(pPage);
	ASSERT(pMedia);

	memset(&tvInsert, 0, sizeof(tvInsert));
	
	//	Initialize the tree related members
	pPage->m_iXmlType = MEDIACTRL_PAGE;

	//	Set up the insert structure
	tvInsert.item.mask = TVIF_TEXT | TVIF_IMAGE | TVIF_SELECTEDIMAGE | TVIF_PARAM;
	tvInsert.item.pszText = LPSTR_TEXTCALLBACK;
	tvInsert.item.iImage = I_IMAGECALLBACK;
	tvInsert.item.iSelectedImage = I_IMAGECALLBACK;
	tvInsert.item.lParam = (LPARAM)((CMediaCtrlItem*)pPage);

	//	Set the parent item
	tvInsert.hParent = pMedia->m_hItem;

	//	Add the item 
	pPage->m_hItem = InsertItem(&tvInsert);

	//	Now add each of the treatments that belong to this page
	Pos = pPage->m_Treatments.GetHeadPosition();
	while(Pos != NULL)
	{
		if((pTreatment = (CXmlTreatment*)pPage->m_Treatments.GetNext(Pos)) != 0)
			Insert(pPage, pTreatment);
	}
}

//==============================================================================
//
// 	Function Name:	CMediaCtrl::Insert()
//
// 	Description:	This function is called to insert the CXmlMedia object
//					into the tree.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMediaCtrl::Insert(CXmlMediaTree* pTree, CXmlMedia* pMedia)
{
	TV_INSERTSTRUCT	tvInsert;
	POSITION		Pos;
	CXmlPage*		pPage;

	ASSERT(pTree);
	ASSERT(pMedia);

	memset(&tvInsert, 0, sizeof(tvInsert));
	
	//	Initialize the tree related members
	pMedia->m_iXmlType = MEDIACTRL_MEDIA;

	//	Set up the insert structure
	tvInsert.item.mask = TVIF_TEXT | TVIF_IMAGE | TVIF_SELECTEDIMAGE | TVIF_PARAM;
	tvInsert.item.pszText = LPSTR_TEXTCALLBACK;
	tvInsert.item.iImage = I_IMAGECALLBACK;
	tvInsert.item.iSelectedImage = I_IMAGECALLBACK;
	tvInsert.item.lParam = (LPARAM)((CMediaCtrlItem*)pMedia);

	//	Set the parent item
	tvInsert.hParent = pTree->m_hItem;

	//	Add the item 
	pMedia->m_hItem = InsertItem(&tvInsert);

	//	Now add each of the pages that belong to this media object
	Pos = pMedia->m_Pages.GetHeadPosition();
	while(Pos != NULL)
	{
		if((pPage = (CXmlPage*)pMedia->m_Pages.GetNext(Pos)) != 0)
			Insert(pMedia, pPage);
	}
}

//==============================================================================
//
// 	Function Name:	CMediaCtrl::Insert()
//
// 	Description:	This function is called to insert the CXmlMediaTree object
//					into the tree.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMediaCtrl::Insert(CXmlMediaTree* pTree)
{
	TV_INSERTSTRUCT	tvInsert;
	POSITION		Pos;
	CXmlMedia*		pMedia;

	ASSERT(pTree);

	memset(&tvInsert, 0, sizeof(tvInsert));
	
	//	Initialize the tree related members
	pTree->m_iXmlType = MEDIACTRL_TREE;

	//	Set up the insert structure
	tvInsert.item.mask = TVIF_TEXT | TVIF_IMAGE | TVIF_SELECTEDIMAGE | TVIF_PARAM;
	tvInsert.item.pszText = LPSTR_TEXTCALLBACK;
	tvInsert.item.iImage = I_IMAGECALLBACK;
	tvInsert.item.iSelectedImage = I_IMAGECALLBACK;
	tvInsert.item.lParam = (LPARAM)((CMediaCtrlItem*)pTree);

	//	Add the item 
	pTree->m_hItem = InsertItem(&tvInsert);

	//	Now add each of the media objects that belong to this tree
	Pos = pTree->GetHeadPosition();
	while(Pos != NULL)
	{
		if((pMedia = (CXmlMedia*)pTree->GetNext(Pos)) != 0)
			Insert(pTree, pMedia);
	}
}

//==============================================================================
//
// 	Function Name:	CMediaCtrl::Insert()
//
// 	Description:	This function is called to insert the CXmlTreatment object
//					into the tree.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMediaCtrl::Insert(CXmlPage* pPage, CXmlTreatment* pTreatment)
{
	TV_INSERTSTRUCT	tvInsert;

	ASSERT(pPage);
	ASSERT(pTreatment);

	memset(&tvInsert, 0, sizeof(tvInsert));
	
	//	Initialize the tree related members
	pTreatment->m_iXmlType = MEDIACTRL_TREATMENT;

	//	Set up the insert structure
	tvInsert.item.mask = TVIF_TEXT | TVIF_IMAGE | TVIF_SELECTEDIMAGE | TVIF_PARAM;
	tvInsert.item.pszText = LPSTR_TEXTCALLBACK;
	tvInsert.item.iImage = I_IMAGECALLBACK;
	tvInsert.item.iSelectedImage = I_IMAGECALLBACK;
	tvInsert.item.lParam = (LPARAM)((CMediaCtrlItem*)pTreatment);

	//	Set the parent item
	tvInsert.hParent = pPage->m_hItem;

	//	Add the item 
	pTreatment->m_hItem = InsertItem(&tvInsert);
}

//==============================================================================
//
// 	Function Name:	CMediaCtrl::OnGetDispInfo()
//
// 	Description:	This function is called when Windows needs the information
//					required to display an object in the tree.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMediaCtrl::OnGetDispInfo(NMHDR* pNMHDR, LRESULT* pResult) 
{
	TV_DISPINFO*	pInfo = (TV_DISPINFO*)pNMHDR;
	CMediaCtrlItem*	pItem;

	*pResult = 0;
	
	//	Get the object associated with this item
	if((pItem = (CMediaCtrlItem*)pInfo->item.lParam) == 0)
		return;

	//	What type of object are we viewing?
	switch(pItem->m_iXmlType)
	{
		case MEDIACTRL_TREE:		
		
			GetDispInfo(pInfo, (CXmlMediaTree*)pItem);
			break;

		case MEDIACTRL_MEDIA:	
		
			GetDispInfo(pInfo, (CXmlMedia*)pItem);
			break;

		case MEDIACTRL_PAGE:	
		
			GetDispInfo(pInfo, (CXmlPage*)pItem);
			break;

		case MEDIACTRL_TREATMENT:	
		
			GetDispInfo(pInfo, (CXmlTreatment*)pItem);
			break;
	}
}

//==============================================================================
//
// 	Function Name:	CMediaCtrl::Redraw()
//
// 	Description:	This function will redraw the tree item specified by the
//					caller
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMediaCtrl::Redraw(CMediaCtrlItem* pItem)
{
	if(pItem && pItem->m_hItem)
	{
		RECT rcItem;
		if(GetItemRect(pItem->m_hItem, &rcItem, FALSE))
			RedrawWindow(&rcItem);
	}
}

//==============================================================================
//
// 	Function Name:	CMediaCtrl::Remove()
//
// 	Description:	This function will remove the specified item from the tree
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMediaCtrl::Remove(CMediaCtrlItem* pItem)
{
	//	Make sure we have a valid handle
	if(pItem != 0 && pItem->m_hItem != 0)
	{
		DeleteItem(pItem->m_hItem);

		//	Have we removed all the items
		if(GetCount() == 0)
		{
			m_pXmlTree = 0;
			m_pXmlMedia = 0;
			m_pXmlPage = 0;
			m_pXmlTreatment = 0;
		}
	}
}

//==============================================================================
//
// 	Function Name:	CMediaCtrl::Select()
//
// 	Description:	This function will select the item provided by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMediaCtrl::Select(CMediaCtrlItem* pItem)
{
	//	Make sure we have a valid handle
	if(pItem != 0 && pItem->m_hItem != 0)
	{
		//	Select the item
		if(SelectItem(pItem->m_hItem))
		{
			//	Make sure the item is visible
			EnsureVisible(pItem);
		}
	}
}

//==============================================================================
//
// 	Function Name:	CMediaCtrl::OnSelChanged()
//
// 	Description:	This function is called when the user makes a new selection
//					from the tree.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CMediaCtrl::OnSelChanged(NMHDR* pNMHDR, LRESULT* pResult) 
{
	NM_TREEVIEW*	pNMTreeView = (NM_TREEVIEW*)pNMHDR;
	CMediaCtrlItem*	pItem;

	if(pResult)
		*pResult = 1;
	
	//	Get the information for this item
	pItem = (CMediaCtrlItem*)pNMTreeView->itemNew.lParam;

	//	Is the item valid?
	if(!pItem)
	{
		m_pXmlTree = 0;
		m_pXmlMedia = 0;
		m_pXmlPage = 0;
		m_pXmlTreatment = 0;
	}
	else
	{
		//	What type of selection has been made?
		switch(pItem->m_iXmlType)
		{
			
			case MEDIACTRL_TREE:

				m_pXmlTree = (CXmlMediaTree*)pItem;
				m_pXmlMedia = 0;
				m_pXmlPage = 0;
				m_pXmlTreatment = 0;
				break;
			
			case MEDIACTRL_MEDIA:		
			
				m_pXmlMedia = (CXmlMedia*)pItem;
				m_pXmlTree = m_pXmlMedia->m_pXmlMediaTree;
				m_pXmlPage = 0;
				m_pXmlTreatment = 0;
				break;
			
			case MEDIACTRL_PAGE:		
			
				m_pXmlPage = (CXmlPage*)pItem;
				m_pXmlMedia = m_pXmlPage->m_pXmlMedia;
				if(m_pXmlMedia)
					m_pXmlTree = m_pXmlMedia->m_pXmlMediaTree;
				else
					m_pXmlTree = 0;
				m_pXmlTreatment = 0;
				break;
			
			case MEDIACTRL_TREATMENT:		
			
				m_pXmlTreatment = (CXmlTreatment*)pItem;
				m_pXmlPage = m_pXmlTreatment->m_pXmlPage;
				if(m_pXmlPage)
					m_pXmlMedia = m_pXmlPage->m_pXmlMedia;
				else
					m_pXmlMedia = 0;
				if(m_pXmlMedia)
					m_pXmlTree = m_pXmlMedia->m_pXmlMediaTree;
				else
					m_pXmlTree = 0;
				break;
			
			default:				
			
				m_pXmlTree = 0;
				m_pXmlMedia = 0;
				m_pXmlPage = 0;
				m_pXmlTreatment = 0;
				break;
		}
	}
	
	//	Propagate the message to the parent window
	return FALSE;
}


