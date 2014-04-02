//------------------------------------------------------------------------------
//
// File Name:	GuiObj.h
//
// Description:	This file contains the declaration of the CGUIObject class.
//
// Author:		Kenneth Moore
//
//------------------------------------------------------------------------------
//	Date		Revision    Description
//	05-07-2006	1.00		Original Release
//------------------------------------------------------------------------------
#if !defined(__GUIOBJ_H__)
#define __GUIOBJ_H__

#pragma once

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
//	This class manages the information used to display a column in a list
//	view control
class CGUICtrlItem : public CObject
{
	private:

	public:

		CString						m_strName;
		CString						m_strText;
		int							m_iHeaderWidth;
		int							m_iIndex;

     								CGUICtrlItem();
								   ~CGUICtrlItem();

	protected:

};

//	Objects of this class are used to manage a dynamic array of list control columns
class CGUICtrlItems : public CObArray
{
	private:

	public:

									CGUICtrlItems();
		virtual					   ~CGUICtrlItems();

		void						Add(CGUICtrlItem* pGUICtrlItem);
		void						Flush(BOOL bDelete);
		void						Remove(CGUICtrlItem* pGUICtrlItem, BOOL bDelete = FALSE);
		int							Find(CGUICtrlItem* pGUICtrlItem);

		void						SetAt(int iIndex, CGUICtrlItem* pGUICtrlItem);
		CGUICtrlItem*				GetAt(int iIndex);

	protected:
};

class CGUIObject : public CObject
{
	private:

	public:

									CGUIObject();
								   ~CGUIObject();
				
		//	Support for display in GUI list view control
		virtual	CGUICtrlItems*		GetListCtrlColumns(){ return NULL; }
		virtual void				GetListCtrlText(CGUICtrlItem* pItem){}
		virtual void				GetListCtrlText(CGUICtrlItems* pItems);
		virtual int					GetListCtrlImageIndex(){ return -1; }
		virtual int					GetListCtrlComparison(CGUIObject* pCompare, int iColumn);

		//	Support for display in GUI property grid control
		virtual	CGUICtrlItems*		GetPropCtrlRows(){ return NULL; }
		virtual void				GetPropCtrlText(CGUICtrlItem* pRow){}
		virtual void				GetPropCtrlText(CGUICtrlItems* pRows);
		virtual int					GetPropCtrlImageIndex(CGUICtrlItem* pRow){ return -1; }

	protected:
	
};

#endif // !defined(__GUIOBJ_H__)
