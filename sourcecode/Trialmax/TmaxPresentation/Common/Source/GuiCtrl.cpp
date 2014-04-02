//------------------------------------------------------------------------------
//
// File Name:	GuiCtrl.cpp
//
// Description:	This file contains member functions of the CGUIListCtrl class
//
// See Also:	GuiCtrl.h
//
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <StdAfx.h>
#include <GuiCtrl.h>

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
BEGIN_MESSAGE_MAP(CGUIListCtrl, CListCtrl)
	//{{AFX_MSG_MAP(CGUIListCtrl)
	ON_NOTIFY_REFLECT(LVN_GETDISPINFO, OnGetDispInfo)
	ON_NOTIFY_REFLECT_EX(LVN_ITEMCHANGED, OnItemChanged)
	ON_NOTIFY_REFLECT_EX(LVN_ITEMCHANGING, OnItemChanging)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

BEGIN_MESSAGE_MAP(CGUIPropCtrl, CListCtrl)
	//{{AFX_MSG_MAP(CGUIPropCtrl)
	ON_NOTIFY_REFLECT(LVN_GETDISPINFO, OnGetDispInfo)
	ON_NOTIFY_REFLECT_EX(LVN_ITEMCHANGED, OnItemChanged)
	ON_NOTIFY_REFLECT_EX(LVN_ITEMCHANGING, OnItemChanging)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::Add()
//
//	Parameters:		pGUIObject - pointer to object to be added to the control
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to add a new object to the list 
//
//------------------------------------------------------------------------------
BOOL CGUIListCtrl::Add(CGUIObject* pGUIObject, BOOL bResizeColumns)
{
	return Insert(pGUIObject, -1, bResizeColumns);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::Add()
//
//	Parameters:		pGUIObjects - a dynamic list of objects to be added
//					bClear - TRUE to clear existing contents
//
// 	Return Value:	None
//
// 	Description:	Called add all objects in the specified list
//
//------------------------------------------------------------------------------
void CGUIListCtrl::Add(CObList* pGUIObjects, BOOL bClear)
{
	POSITION	Pos = NULL;
	CGUIObject*	pGUIObject = NULL;

	if(bClear == TRUE)
		Clear();

	//	Should we repopulate the list?
	if(pGUIObjects != NULL)
	{
		Pos = pGUIObjects->GetHeadPosition();
		while(Pos != NULL)
		{
			if((pGUIObject = (CGUIObject*)(pGUIObjects->GetNext(Pos))) != 0)
				Add(pGUIObject, FALSE);
		}

	}// if(pGUIObjects != NULL)

	AutoSizeColumns();
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::Add()
//
//	Parameters:		pColumn - column to be added to the list view control
//					iIndex - index of column to be added
//
// 	Return Value:	TRUE if successful
//
// 	Description:	Called to add a column to the list view control
//
//------------------------------------------------------------------------------
BOOL CGUIListCtrl::Add(CGUICtrlItem* pColumn, int iIndex)
{
	ASSERT(pColumn != 0);

	iIndex = this->InsertColumn(iIndex, pColumn->m_strName, LVCFMT_LEFT, 10);
	
	return (iIndex >= 0);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::AutoSizeColumn()
//
//	Parameters:		iIndex - index of the column to be sized
//					pColumn - the associated column descriptor
//
// 	Return Value:	None
//
// 	Description:	Called to resize the column to fit its content
//
//------------------------------------------------------------------------------
void CGUIListCtrl::AutoSizeColumn(int iIndex, CGUICtrlItem* pColumn)
{
	int iData = 0;

	//	Get the width required to display the header
	if(pColumn->m_iHeaderWidth <= 0)
	{
		this->SetColumnWidth(iIndex, LVSCW_AUTOSIZE_USEHEADER);
		pColumn->m_iHeaderWidth = this->GetColumnWidth(iIndex);
	}

	//	Resize to fit the data
	this->SetColumnWidth(iIndex, LVSCW_AUTOSIZE);
	iData = this->GetColumnWidth(iIndex);

	//	Switch back to header width if it's greater
	if(pColumn->m_iHeaderWidth > iData)
		this->SetColumnWidth(iIndex, pColumn->m_iHeaderWidth);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::AutoSizeColumns()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Called to resize the columns to fit their content
//
//------------------------------------------------------------------------------
void CGUIListCtrl::AutoSizeColumns()
{
	//	Size each of the columns
	if(m_pColumns != 0)
	{
		for(int i = 0; i <= m_pColumns->GetUpperBound(); i++)
			AutoSizeColumn(i, m_pColumns->GetAt(i));
	}

}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::Clear()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Called to clear all objects in the list box
//
//------------------------------------------------------------------------------
void CGUIListCtrl::Clear()
{
	m_bSuppress = TRUE;

	DeleteAllItems();

	m_bSuppress = FALSE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::CGUIListCtrl()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Constructor
//
//------------------------------------------------------------------------------
CGUIListCtrl::CGUIListCtrl()
{
	m_pColumns = 0;
	m_bSuppress = FALSE;
	m_bUseImages = FALSE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::~CGUIListCtrl()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Destructor
//
//------------------------------------------------------------------------------
CGUIListCtrl::~CGUIListCtrl()
{
	//	Delete the existing columns
	if(m_pColumns != 0)
	{
		m_pColumns->Flush(TRUE);
		delete m_pColumns;
		m_pColumns = 0;
	}

}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::Delete()
// 
//	Parameters:		pGUIObject     - the object to be selected
//					bSuppress - TRUE to suppress state change notifications
//
// 	Return Value:	None
//
// 	Description:	This function is called to delete an object in the list
//
//------------------------------------------------------------------------------
BOOL CGUIListCtrl::Delete(CGUIObject* pGUIObject, BOOL bSuppress)
{
	int iIndex;

	if((iIndex = GetIndex(pGUIObject)) >= 0)
	{
		Delete(iIndex, bSuppress);
	}

	return (iIndex >= 0);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::Delete()
// 
//	Parameters:		iIndex    - index of the object to delete
//					bSuppress - TRUE to suppress state change notifications
//
// 	Return Value:	None
//
// 	Description:	This function is called to delete an object in the list
//
//------------------------------------------------------------------------------
void CGUIListCtrl::Delete(int iIndex, BOOL bSuppress)
{
	//	Is the index within range?
	if((iIndex >= 0) && (iIndex < GetItemCount()))
	{
		m_bSuppress = bSuppress;

		//	Delete the object
		DeleteItem(iIndex);

		m_bSuppress = FALSE;
	}	
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::DoComparison()
//
//	Parameters:		LPItem1  - First object for comparison
//					LPItem2  - Second object for comparison
//					LPColumn - Index of column to sort on
//
// 	Return Value:	> 0 if LPItem1 is greater than LPItem2
//					= 0 if LPItem1 equals LPItem2
//					< 0 if LPItem1 is less than LPItem2
//
// 	Description:	This function is called by Windows to sort the list box
//
//------------------------------------------------------------------------------
int CALLBACK CGUIListCtrl::DoComparison(LPARAM LPItem1,LPARAM LPItem2,LPARAM LPColumn)
{
	CGUIObject* pGUIObject1 = (CGUIObject*)LPItem1;
	CGUIObject* pGUIObject2 = (CGUIObject*)LPItem2;

	return pGUIObject1->GetListCtrlComparison(pGUIObject2, (int)LPColumn);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::GetIndex()
//
//	Parameters:		pGUIObject - pointer to object to search for
//
// 	Return Value:	The index of the object in the list
//
// 	Description:	This function is called to get the list index of the
//					specified object.
//
//------------------------------------------------------------------------------
int CGUIListCtrl::GetIndex(CGUIObject* pGUIObject)
{
	LVFINDINFO	Find;

	ZeroMemory(&Find, sizeof(Find));
	Find.flags = LVFI_PARAM;
	Find.lParam = (LPARAM)pGUIObject;

	return FindItem(&Find);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::GetIndexFromPt()
//
//	Parameters:		pPoint - the screen coordinates
//
// 	Return Value:	The index of the item at the specified point
//
// 	Description:	Called to get the index at the specified screen position
//
//------------------------------------------------------------------------------
int CGUIListCtrl::GetIndexFromPt(POINT* pPoint) 
{
	UINT uFlags = 0;

	//	Convert the screen coordinates to client coordinates
	ScreenToClient(pPoint);

	//	Get the item we're over top of
	return HitTest(*pPoint, &uFlags);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::GetObjectFromPt()
//
//	Parameters:		pPoint - the screen coordinates
//
// 	Return Value:	The item at the specified location
//
// 	Description:	Called to get the item at the specified screen position
//
//------------------------------------------------------------------------------
CGUIObject* CGUIListCtrl::GetObjectFromPt(POINT* pPoint) 
{
	CGUIObject*	pGUIObj = NULL;
	int			iIndex = -1;

	//	Convert the screen coordinates to client coordinates
	ScreenToClient(pPoint);

	//	Get the item we're over top of
	if((iIndex = GetIndexFromPt(pPoint)) >= 0)
	{
		//	Get a pointer to the object
		pGUIObj = (CGUIObject*)GetItemData(iIndex);
	}

	return pGUIObj;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::GetSelection()
//
//	Parameters:		None
//
// 	Return Value:	The object currently selected in the list box
//
// 	Description:	Called to get the object that is the current selection
//
//------------------------------------------------------------------------------
CGUIObject* CGUIListCtrl::GetSelection()
{
	POSITION	Pos = NULL;
	CGUIObject*	pGUIObject = 0;
	int			iIndex = -1;

	if((Pos = this->GetFirstSelectedItemPosition()) != NULL)
	{
		if((iIndex = this->GetNextSelectedItem(Pos)) >= 0)
			pGUIObject = (CGUIObject*)(this->GetItemData(iIndex));
	}

	return pGUIObject;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::Initialize()
//
//	Parameters:		pOwnerClass - an object of the class to be displayed
//					uResourceId - resource id of image list bitmap
//
// 	Return Value:	None
//
// 	Description:	TRUE if successful
//
//------------------------------------------------------------------------------
BOOL CGUIListCtrl::Initialize(CGUIObject* pOwnerClass, UINT uResourceId)
{
	CBitmap bmImages;

	SetExtendedStyle(GetExtendedStyle() | LVS_EX_FULLROWSELECT);

	//	Should we create an image list?
	if(uResourceId > 0)
	{
		//	Create the image list
		bmImages.LoadBitmap(uResourceId);
		m_Images.Create(16, 16, ILC_MASK | ILC_COLOR24, 0, 1);
		m_Images.Add(&bmImages, RGB(255,0,255));  

		//	Attach the image list
		SetImageList(&m_Images, LVSIL_SMALL);

		m_bUseImages = TRUE;
	}

	//	Delete any columns that may have already been added to the list view
	if(this->GetHeaderCtrl() != 0)
	{
		while(this->GetHeaderCtrl()->GetItemCount() > 0)	
			this->DeleteColumn(0);
	}
	
	//	Delete the existing columns
	if(m_pColumns != 0)
	{
		m_pColumns->Flush(TRUE);
		delete m_pColumns;
		m_pColumns = 0;
	}

	//	Get the new column collection from the owner
	if(pOwnerClass != 0)
		m_pColumns = pOwnerClass->GetListCtrlColumns();

	//	Add the new columns
	if(m_pColumns != 0)
	{
		for(int i = 0; i <= m_pColumns->GetUpperBound(); i++)
		{
			if(m_pColumns->GetAt(i) != 0)
				Add(m_pColumns->GetAt(i), i);
		}

		AutoSizeColumns();

	}

	return TRUE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::Insert()
//
//	Parameters:		pGUIObject - pointer to object to be added to the list control
//					iIndex - index at which to insert the object
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to insert an object at the specified
//					index
//
//------------------------------------------------------------------------------
BOOL CGUIListCtrl::Insert(CGUIObject* pGUIObject, int iIndex, BOOL bResizeColumns)
{
	LV_ITEM	lvItem;
	BOOL	bSuccessful = FALSE;

	ASSERT(pGUIObject);

	//	Is the index within range
	if((iIndex < 0) || (iIndex >= this->GetItemCount()))
		iIndex = this->GetItemCount();

	memset(&lvItem, 0, sizeof(lvItem));
	lvItem.mask = LVIF_TEXT | LVIF_PARAM;
	lvItem.iItem = iIndex;
	lvItem.pszText = LPSTR_TEXTCALLBACK;
	lvItem.lParam = (LPARAM)pGUIObject;

	//	Are we using images?
	if(m_bUseImages == TRUE)
	{
		lvItem.mask |= LVIF_IMAGE;
		lvItem.iImage = I_IMAGECALLBACK;
	}

	if((bSuccessful = (CListCtrl::InsertItem(&lvItem) != -1)) == TRUE)
	{
		if(bResizeColumns ==  TRUE)
			AutoSizeColumns();
	}

	return bSuccessful;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::OnGetDispInfo()
//
//	Parameters:		pNMHDR  - pointer to message header structure
//					pResult - pointer to return value buffer
//
// 	Return Value:	None
//
// 	Description:	This function is called by the control to get the 
//					information it needs to display a list box selection
//
//------------------------------------------------------------------------------
void CGUIListCtrl::OnGetDispInfo(NMHDR* pNMHDR, LRESULT* pResult) 
{
	LV_DISPINFO*	pDispInfo = (LV_DISPINFO*)pNMHDR;
	CGUIObject*		pGUIObject = 0;
	CGUICtrlItem*	pColumn = 0;

	if(pResult) *pResult = 0;
	
	//	Does the control need the text string?
	if(pDispInfo->item.mask & LVIF_TEXT)
	{
		//	Get the list object
		if((pGUIObject = (CGUIObject*)(pDispInfo->item.lParam)) == 0) return;

		//	Get the column
		if((m_pColumns != 0) && (pDispInfo->item.iSubItem >= 0) && (pDispInfo->item.iSubItem <= m_pColumns->GetUpperBound()))
		{
			//	Get the column
			if((pColumn = m_pColumns->GetAt(pDispInfo->item.iSubItem)) != 0)
			{
				//	Get the column text
				pGUIObject->GetListCtrlText(pColumn);

				//	Update the control
				lstrcpy(pDispInfo->item.pszText, pColumn->m_strText);
			}

		}	

	}// if(pDispInfo->object.mask & LVIF_TEXT)

	//	Does it need the icon index?
	if(pDispInfo->item.mask & LVIF_IMAGE)
		pDispInfo->item.iImage = pGUIObject->GetListCtrlImageIndex();
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::OnItemChanged()
//
//	Parameters:		pNMHDR  - pointer to message header structure
//					pResult - pointer to return value buffer
//
// 	Return Value:	TRUE to discard the notification
//
// 	Description:	This function is called when the selection state of an object
//					has changed.
//
//------------------------------------------------------------------------------
BOOL CGUIListCtrl::OnItemChanged(NMHDR* pNMHDR, LRESULT* pResult) 
{
	if(m_bSuppress)
	{
		*pResult = 0;
		return TRUE;
	}
	else
	{
		*pResult = 0;
		return FALSE;
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::OnItemChanging()
//
//	Parameters:		pNMHDR  - pointer to message header structure
//					pResult - pointer to return value buffer
//
// 	Return Value:	TRUE to discard the notification
//
// 	Description:	This function is called when the selection state of an object
//					is about to change.
//
//------------------------------------------------------------------------------
BOOL CGUIListCtrl::OnItemChanging(NMHDR* pNMHDR, LRESULT* pResult) 
{
	if(m_bSuppress)
	{
		*pResult = 0;
		return TRUE;
	}
	else
	{
		*pResult = 0;
		return FALSE;
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::Redraw()
//
//	Parameters:		iIndex - index of the object to redraw
//					bAutoSizeColumns - TRUE to resize the columns
//
// 	Return Value:	None
//
// 	Description:	This function is called to redraw an object in the list
//
//------------------------------------------------------------------------------
void CGUIListCtrl::Redraw(int iIndex, BOOL bAutoSizeColumns)
{
	//	Is the index within range?
	if((iIndex >= 0) && (iIndex < this->GetItemCount()))
	{
		RedrawItems(iIndex, iIndex);

		if(bAutoSizeColumns == TRUE)
			AutoSizeColumns();
	}	
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::Redraw()
//
//	Parameters:		pGUIObject - the object to be refreshed
//					bAutoSizeColumns - TRUE to resize the columns
//
// 	Return Value:	None
//
// 	Description:	This function is called to redraw an object in the list
//
//------------------------------------------------------------------------------
void CGUIListCtrl::Redraw(CGUIObject* pGUIObject, BOOL bAutoSizeColumns)
{
	int iIndex = -1;

	if((iIndex = GetIndex(pGUIObject)) >= 0)
		Redraw(iIndex, bAutoSizeColumns);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::Select()
// 
//	Parameters:		iIndex    - index of the object to select
//					bSuppress - TRUE to suppress state change notifications
//
// 	Return Value:	None
//
// 	Description:	This function is called to select an object in the list
//
//------------------------------------------------------------------------------
void CGUIListCtrl::Select(int iIndex, BOOL bSuppress)
{
	//	Is the index within range?
	if((iIndex >= 0) && (iIndex < GetItemCount()))
	{
		m_bSuppress = bSuppress;

		//	Select the object
		SetItemState(iIndex, LVIS_SELECTED, LVIS_SELECTED);
		SetItemState(iIndex, LVIS_FOCUSED, LVIS_FOCUSED);
		EnsureVisible(iIndex, FALSE);

		m_bSuppress = FALSE;
	}	
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::Select()
// 
//	Parameters:		pGUICtrlItem - the row to be selected
//					bSuppress - TRUE to suppress state change notifications
//
// 	Return Value:	None
//
// 	Description:	This function is called to select an object in the list
//
//------------------------------------------------------------------------------
BOOL CGUIListCtrl::Select(CGUIObject* pGUIObject, BOOL bSuppress)
{
	int iIndex;

	if((iIndex = GetIndex(pGUIObject)) >= 0)
	{
		Select(iIndex, bSuppress);
	}

	return (iIndex >= 0);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIListCtrl::Sort()
//
//	Parameters:		iColumn - the id of the column to be sorted
//
// 	Return Value:	None
//
// 	Description:	This function is called to sort the list box.
//
//------------------------------------------------------------------------------
void CGUIListCtrl::Sort(int iColumn) 
{
	SortItems(DoComparison, iColumn);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIPropCtrl::Add()
//
//	Parameters:		pGUIObject - pointer to object to be added to the control
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to add a new object to the list 
//
//------------------------------------------------------------------------------
BOOL CGUIPropCtrl::Add(CGUICtrlItem* pGUICtrlItem, BOOL bResizeColumns)
{
	return Insert(pGUICtrlItem, -1, bResizeColumns);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIPropCtrl::AutoSizeColumn()
//
//	Parameters:		iIndex - index of the column to be sized
//					pColumn - the associated column descriptor
//
// 	Return Value:	None
//
// 	Description:	Called to resize the column to fit its content
//
//------------------------------------------------------------------------------
void CGUIPropCtrl::AutoSizeColumn(int iIndex)
{
	//	Resize to fit the data
	this->SetColumnWidth(iIndex, LVSCW_AUTOSIZE);

	//	Is this smaller than allowed?
	if(this->GetColumnWidth(iIndex) < GUIPROPCTRL_MIN_COLUMN_WIDTH)
		this->SetColumnWidth(iIndex, GUIPROPCTRL_MIN_COLUMN_WIDTH);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIPropCtrl::AutoSizeColumns()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Called to resize the columns to fit their content
//
//------------------------------------------------------------------------------
void CGUIPropCtrl::AutoSizeColumns()
{
	//	Size each of the columns
	AutoSizeColumn(GUIPROPCTRL_COLUMN_NAME);
	AutoSizeColumn(GUIPROPCTRL_COLUMN_VALUE);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIPropCtrl::Clear()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Called to clear all objects in the list box
//
//------------------------------------------------------------------------------
void CGUIPropCtrl::Clear()
{
	m_bSuppress = TRUE;

	//	Delete the existing rows
	DeleteAllItems();

	if(m_pRows != 0)
	{
		m_pRows->Flush(TRUE);
		delete m_pRows;
		m_pRows = NULL;
	}

	m_bSuppress = FALSE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIPropCtrl::CGUIPropCtrl()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Constructor
//
//------------------------------------------------------------------------------
CGUIPropCtrl::CGUIPropCtrl()
{
	m_pOwner = NULL;
	m_pRows = NULL;
	m_bSuppress = FALSE;
	m_bUseImages = FALSE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIPropCtrl::~CGUIPropCtrl()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Destructor
//
//------------------------------------------------------------------------------
CGUIPropCtrl::~CGUIPropCtrl()
{
	//	Delete the existing rows
	if(m_pRows != 0)
	{
		m_pRows->Flush(TRUE);
		delete m_pRows;
		m_pRows = NULL;
	}

}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIPropCtrl::Delete()
// 
//	Parameters:		iIndex    - index of the object to delete
//					bSuppress - TRUE to suppress state change notifications
//
// 	Return Value:	None
//
// 	Description:	This function is called to delete an object in the list
//
//------------------------------------------------------------------------------
void CGUIPropCtrl::Delete(int iIndex, BOOL bSuppress)
{
	//	Is the index within range?
	if((iIndex >= 0) && (iIndex < GetItemCount()))
	{
		m_bSuppress = bSuppress;

		//	Delete the object
		DeleteItem(iIndex);

		m_bSuppress = FALSE;
	}	
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIPropCtrl::Delete()
// 
//	Parameters:		pGUICtrlItem - the row to be deleted
//					bSuppress - TRUE to suppress state change notifications
//
// 	Return Value:	None
//
// 	Description:	This function is called to delete a row in the grid
//
//------------------------------------------------------------------------------
BOOL CGUIPropCtrl::Delete(CGUICtrlItem* pGUICtrlItem, BOOL bSuppress)
{
	int iIndex;

	if((iIndex = GetIndex(pGUICtrlItem)) >= 0)
	{
		Delete(iIndex, bSuppress);
	}

	if(m_pRows != NULL)
		m_pRows->Remove(pGUICtrlItem, TRUE);

	return (iIndex >= 0);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIPropCtrl::GetIndex()
//
//	Parameters:		pGUICtrlItem - pointer to row to search for
//
// 	Return Value:	The index of the object in the list
//
// 	Description:	This function is called to get the list index of the
//					specified row item.
//
//------------------------------------------------------------------------------
int CGUIPropCtrl::GetIndex(CGUICtrlItem* pGUICtrlItem)
{
	LVFINDINFO	Find;

	ZeroMemory(&Find, sizeof(Find));
	Find.flags = LVFI_PARAM;
	Find.lParam = (LPARAM)pGUICtrlItem;

	return FindItem(&Find);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIPropCtrl::GetSelection()
//
//	Parameters:		None
//
// 	Return Value:	The object currently selected in the list box
//
// 	Description:	Called to get the object that is the current selection
//
//------------------------------------------------------------------------------
CGUICtrlItem* CGUIPropCtrl::GetSelection()
{
	POSITION		Pos = NULL;
	CGUICtrlItem*	pGUICtrlItem = NULL;
	int				iIndex = -1;

	if((Pos = this->GetFirstSelectedItemPosition()) != NULL)
	{
		if((iIndex = this->GetNextSelectedItem(Pos)) >= 0)
			pGUICtrlItem = (CGUICtrlItem*)(this->GetItemData(iIndex));
	}

	return pGUICtrlItem;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIPropCtrl::Initialize()
//
//	Parameters:		pOwner - an object of the class to be displayed
//					uResourceId - resource id of image list bitmap
//
// 	Return Value:	None
//
// 	Description:	TRUE if successful
//
//------------------------------------------------------------------------------
BOOL CGUIPropCtrl::Initialize(CGUIObject* pOwner, UINT uResourceId)
{
	CBitmap bmImages;

	SetExtendedStyle(GetExtendedStyle() | LVS_EX_FULLROWSELECT);

	//	Should we create an image list?
	if(uResourceId > 0)
	{
		//	Create the image list
		bmImages.LoadBitmap(uResourceId);
		m_Images.Create(16, 16, ILC_MASK | ILC_COLOR24, 0, 1);
		m_Images.Add(&bmImages, RGB(255,0,255));  

		//	Attach the image list
		SetImageList(&m_Images, LVSIL_SMALL);

		m_bUseImages = TRUE;
	}


	//	Add the columns
	this->InsertColumn(GUIPROPCTRL_COLUMN_NAME, "Name", LVCFMT_LEFT, 10);
	this->InsertColumn(GUIPROPCTRL_COLUMN_VALUE, "Value", LVCFMT_LEFT, 10);
	AutoSizeColumns();

	if(pOwner != NULL)
		SetOwner(pOwner);

	return TRUE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIPropCtrl::Insert()
//
//	Parameters:		pGUICtrlItem - pointer to object to be added to the grid
//					iIndex - index at which to insert the object
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to insert an object at the specified
//					index
//
//------------------------------------------------------------------------------
BOOL CGUIPropCtrl::Insert(CGUICtrlItem* pGUICtrlItem, int iIndex, BOOL bResizeColumns)
{
	LV_ITEM	lvItem;
	BOOL	bSuccessful = FALSE;

	ASSERT(pGUICtrlItem);

	//	Is the index within range
	if((iIndex < 0) || (iIndex >= this->GetItemCount()))
		iIndex = this->GetItemCount();

	memset(&lvItem, 0, sizeof(lvItem));
	lvItem.mask = LVIF_TEXT | LVIF_PARAM;
	lvItem.iItem = iIndex;
	lvItem.pszText = LPSTR_TEXTCALLBACK;
	lvItem.lParam = (LPARAM)pGUICtrlItem;

	//	Are we using images?
	if(m_bUseImages == TRUE)
	{
		lvItem.mask |= LVIF_IMAGE;
		lvItem.iImage = I_IMAGECALLBACK;
	}

	if((bSuccessful = (CListCtrl::InsertItem(&lvItem) != -1)) == TRUE)
	{
		if(bResizeColumns ==  TRUE)
			AutoSizeColumns();
	}

	return bSuccessful;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIPropCtrl::OnGetDispInfo()
//
//	Parameters:		pNMHDR  - pointer to message header structure
//					pResult - pointer to return value buffer
//
// 	Return Value:	None
//
// 	Description:	This function is called by the control to get the 
//					information it needs to display a list box selection
//
//------------------------------------------------------------------------------
void CGUIPropCtrl::OnGetDispInfo(NMHDR* pNMHDR, LRESULT* pResult) 
{
	LV_DISPINFO*	pDispInfo = (LV_DISPINFO*)pNMHDR;
	CGUIObject*		pGUIObject = NULL;
	CGUICtrlItem*	pRow = NULL;

	if(pResult) *pResult = 0;
	
	//	Get the row being updated
	pRow = (CGUICtrlItem*)(pDispInfo->item.lParam);

	//	Must have a row and the owner
	if((m_pOwner != NULL) && (pRow != NULL))
	{
		//	Does the control need the text string?
		if(pDispInfo->item.mask & LVIF_TEXT)
		{
			//	Which column ?
			if(pDispInfo->item.iSubItem == GUIPROPCTRL_COLUMN_NAME)
			{
				lstrcpy(pDispInfo->item.pszText, pRow->m_strName);
			}
			else if(pDispInfo->item.iSubItem == GUIPROPCTRL_COLUMN_VALUE)
			{
				//	Get the value text
				m_pOwner->GetPropCtrlText(pRow);
				lstrcpy(pDispInfo->item.pszText, pRow->m_strText);
			}

		}// if(pDispInfo->object.mask & LVIF_TEXT)

		//	Does it need the icon index?
		if(pDispInfo->item.mask & LVIF_IMAGE)
			pDispInfo->item.iImage = pGUIObject->GetPropCtrlImageIndex(pRow);

	}// if((m_pOwner != NULL) && (pRow != NULL))
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIPropCtrl::OnItemChanged()
//
//	Parameters:		pNMHDR  - pointer to message header structure
//					pResult - pointer to return value buffer
//
// 	Return Value:	TRUE to discard the notification
//
// 	Description:	This function is called when the selection state of an object
//					has changed.
//
//------------------------------------------------------------------------------
BOOL CGUIPropCtrl::OnItemChanged(NMHDR* pNMHDR, LRESULT* pResult) 
{
	if(m_bSuppress)
	{
		*pResult = 0;
		return TRUE;
	}
	else
	{
		*pResult = 0;
		return FALSE;
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIPropCtrl::OnItemChanging()
//
//	Parameters:		pNMHDR  - pointer to message header structure
//					pResult - pointer to return value buffer
//
// 	Return Value:	TRUE to discard the notification
//
// 	Description:	This function is called when the selection state of an object
//					is about to change.
//
//------------------------------------------------------------------------------
BOOL CGUIPropCtrl::OnItemChanging(NMHDR* pNMHDR, LRESULT* pResult) 
{
	if(m_bSuppress)
	{
		*pResult = 0;
		return TRUE;
	}
	else
	{
		*pResult = 0;
		return FALSE;
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIPropCtrl::Redraw()
//
//	Parameters:		iIndex - index of the object to redraw
//					bAutoSizeColumns - TRUE to resize the columns
//
// 	Return Value:	None
//
// 	Description:	This function is called to redraw an object in the list
//
//------------------------------------------------------------------------------
void CGUIPropCtrl::Redraw(int iIndex, BOOL bAutoSizeColumns)
{
	//	Is the index within range?
	if((iIndex >= 0) && (iIndex < this->GetItemCount()))
	{
		RedrawItems(iIndex, iIndex);

		if(bAutoSizeColumns == TRUE)
			AutoSizeColumns();
	}	
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIPropCtrl::Redraw()
//
//	Parameters:		pGUICtrlItem - the item to be refreshed
//					bAutoSizeColumns - TRUE to resize the columns
//
// 	Return Value:	None
//
// 	Description:	This function is called to redraw an object in the list
//
//------------------------------------------------------------------------------
void CGUIPropCtrl::Redraw(CGUICtrlItem* pGUICtrlItem, BOOL bAutoSizeColumns)
{
	int iIndex = -1;

	if((iIndex = GetIndex(pGUICtrlItem)) >= 0)
		Redraw(iIndex, bAutoSizeColumns);
	else
		this->RedrawWindow();
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIPropCtrl::Select()
// 
//	Parameters:		iIndex    - index of the object to select
//					bSuppress - TRUE to suppress state change notifications
//
// 	Return Value:	None
//
// 	Description:	This function is called to select an object in the list
//
//------------------------------------------------------------------------------
void CGUIPropCtrl::Select(int iIndex, BOOL bSuppress)
{
	//	Is the index within range?
	if((iIndex >= 0) && (iIndex < GetItemCount()))
	{
		m_bSuppress = bSuppress;

		//	Select the object
		SetItemState(iIndex, LVIS_SELECTED, LVIS_SELECTED);
		SetItemState(iIndex, LVIS_FOCUSED, LVIS_FOCUSED);
		EnsureVisible(iIndex, FALSE);

		m_bSuppress = FALSE;
	}	
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIPropCtrl::Select()
// 
//	Parameters:		pGUICtrlItem - the row to be selected
//					bSuppress - TRUE to suppress state change notifications
//
// 	Return Value:	None
//
// 	Description:	This function is called to select an row in the grid
//
//------------------------------------------------------------------------------
BOOL CGUIPropCtrl::Select(CGUICtrlItem* pGUICtrlItem, BOOL bSuppress)
{
	int iIndex;

	if((iIndex = GetIndex(pGUICtrlItem)) >= 0)
	{
		Select(iIndex, bSuppress);
	}

	return (iIndex >= 0);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIPropCtrl::SetOwner()
//
//	Parameters:		pOwner - the object that is bound to the grid
//
// 	Return Value:	TRUE if successful
//
// 	Description:	Called to set the owner of the grid
//
//------------------------------------------------------------------------------
BOOL CGUIPropCtrl::SetOwner(CGUIObject* pOwner)
{
	//	Clear the existing values
	Clear();

	//	Get the new row collection from the owner
	if((m_pOwner = pOwner) != NULL)
		m_pRows = m_pOwner->GetPropCtrlRows();

	//	Add the new columns
	if(m_pRows != 0)
	{
		for(int i = 0; i <= m_pRows->GetUpperBound(); i++)
		{
			if(m_pRows->GetAt(i) != NULL)
				Add(m_pRows->GetAt(i), i);
		}

		AutoSizeColumns();

	}

	return TRUE;
}


