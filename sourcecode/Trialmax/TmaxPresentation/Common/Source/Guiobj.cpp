//------------------------------------------------------------------------------
//
// File Name:	GuiObj.cpp
//
// Description:	This file contains the implementation of the CGUIObject class.
//
// Author:		Kenneth Moore
//
//------------------------------------------------------------------------------
//	Date		Revision    Description
//	05-07-2006	1.00		Original Release
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <StdAfx.h>
#include <GuiObj.h>

//-----------------------------------------------------------------------------
//	DEFINES
//-----------------------------------------------------------------------------
#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//-----------------------------------------------------------------------------
//	GLOBALS
//-----------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUICtrlItem::CGUICtrlItem()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Constructor
//
//------------------------------------------------------------------------------
CGUICtrlItem::CGUICtrlItem()
{
	m_strName = "";
	m_strText = "";
	m_iHeaderWidth = -1;
	m_iIndex = 0;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUICtrlItem::~CGUICtrlItem()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Destructor
//
//------------------------------------------------------------------------------
CGUICtrlItem::~CGUICtrlItem()
{
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUICtrlItems::Add()
//
//	Parameters:		pGUICtrlItem - A pointer to the object being added
//
// 	Return Value:	None
//
// 	Description:	This function is called to add an object to the array
//
//------------------------------------------------------------------------------
void CGUICtrlItems::Add(CGUICtrlItem* pGUICtrlItem)
{
	ASSERT(pGUICtrlItem);
	
	//	Add the object to the array
	if(pGUICtrlItem)
	{
		CObArray::Add(pGUICtrlItem);
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUICtrlItems::CGUICtrlItems()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Constructor
//
//------------------------------------------------------------------------------
CGUICtrlItems::CGUICtrlItems() : CObArray()
{
	//	Set the initial size of the array
	SetSize(0);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUICtrlItems::~CGUICtrlItems()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Destructor 
//
//------------------------------------------------------------------------------
CGUICtrlItems::~CGUICtrlItems()
{
	//	Flush the array and destroy the objects
	Flush(TRUE);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUICtrlItems::Find()
//
//	Parameters:		pGUICtrlItem - the item to search for
//
// 	Return Value:	The index of the specified item
//
// 	Description:	This function is called to locate the specified item
//
//------------------------------------------------------------------------------
int CGUICtrlItems::Find(CGUICtrlItem* pGUICtrlItem)
{
	for(int i = 0; i <= GetUpperBound(); i++)
	{
		if(GetAt(i) == pGUICtrlItem)
			return i;
	}

	return -1; // Not found
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUICtrlItems::Flush()
//
//	Parameters:		bDelete - TRUE to force deallocation of all objects
//
// 	Return Value:	None
//
// 	Description:	This function is called to remove all objects from the
//					array
//
//------------------------------------------------------------------------------
void CGUICtrlItems::Flush(BOOL bDelete)
{
	//	Do we want to delete the objects?
	if(bDelete)
	{
		for(int i = 0; i <= GetUpperBound(); i++)
		{
			if(GetAt(i) != 0)
				delete GetAt(i);
		}
	}

	//	Remove all pointers from the array
	RemoveAll();
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUICtrlItems::GetAt()
//
//	Parameters:		iIndex - index of the object to retrieve
//
// 	Return Value:	A pointer to the specified object
//
// 	Description:	This function is called to retrieve the object at the 
//					specified index.
//
//------------------------------------------------------------------------------
CGUICtrlItem* CGUICtrlItems::GetAt(int iIndex)
{
	if((iIndex >= 0) && (iIndex <= GetUpperBound()))
		return ((CGUICtrlItem*)CObArray::GetAt(iIndex));
	else 
		return 0;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUICtrlItems::Remove()
//
//	Parameters:		pGUICtrlItem - A pointer to the object being removed
//					bDelete - true to delete after removal
//
// 	Return Value:	None
//
// 	Description:	This function is called to remove an object in the array
//
//------------------------------------------------------------------------------
void CGUICtrlItems::Remove(CGUICtrlItem* pGUICtrlItem, BOOL bDelete)
{
	int iIndex = -1;

	ASSERT(pGUICtrlItem);
	
	if((iIndex = Find(pGUICtrlItem)) >= 0)
		this->RemoveAt(iIndex);

	if(bDelete == TRUE)
		delete pGUICtrlItem;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUICtrlItems::SetAt()
//
//	Parameters:		iIndex - index at which to place the object
//					pGUICtrlItem - pointer to column being added to the array
//
// 	Return Value:	None
//
// 	Description:	This function is called to set the object at the 
//					specified index.
//
//------------------------------------------------------------------------------
void CGUICtrlItems::SetAt(int iIndex, CGUICtrlItem* pGUICtrlItem)
{
	ASSERT(iIndex >= 0);
	ASSERT(pGUICtrlItem != 0);

	CObArray::SetAtGrow(iIndex, pGUICtrlItem);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIObject::CGUIObject()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Constructor
//
//------------------------------------------------------------------------------
CGUIObject::CGUIObject()
{
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIObject::~CGUIObject()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Destructor
//
//------------------------------------------------------------------------------
CGUIObject::~CGUIObject()
{
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIObject::GetListCtrlComparison()
//
//	Parameters:		pCompare - the object to be compared
//					iColumn  - the id of the column provided in the call to Sort
//
// 	Return Value:	-1 if less than, 0 if equal, 1 if greater than
//
// 	Description:	Called to compare this object to the specified object when 
//					sorting the list
//
//------------------------------------------------------------------------------
int CGUIObject::GetListCtrlComparison(CGUIObject* pCompare, int iColumn)
{
	return -1;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIObject::GetListCtrlText()
//
//	Parameters:		pGUICtrlItems - the list of columns in the list box
//
// 	Return Value:	None
//
// 	Description:	Called to get the text for all columns in the list box
//
//------------------------------------------------------------------------------
void CGUIObject::GetListCtrlText(CGUICtrlItems* pGUICtrlItems)
{
	CGUICtrlItem* pGUICtrlItem = 0;

	for(int i = 0; i <= pGUICtrlItems->GetUpperBound(); i++)
	{
		if((pGUICtrlItem = pGUICtrlItems->GetAt(i)) != 0)
			GetListCtrlText(pGUICtrlItem);
	}

}

//------------------------------------------------------------------------------
//
// 	Function Name:	CGUIObject::GetPropCtrlText()
//
//	Parameters:		pRows - the list of rows in the property grid
//
// 	Return Value:	None
//
// 	Description:	Called to get the text for all rows in the property grid
//
//------------------------------------------------------------------------------
void CGUIObject::GetPropCtrlText(CGUICtrlItems* pRows)
{
	CGUICtrlItem* pGUICtrlItem = 0;

	for(int i = 0; i <= pRows->GetUpperBound(); i++)
	{
		if((pGUICtrlItem = pRows->GetAt(i)) != 0)
			GetPropCtrlText(pGUICtrlItem);
	}

}

