//------------------------------------------------------------------------------
//
// File Name:	GUICtrl.h
//
// Description:	This file contains the declaration of CGUIListCtrl and
//				CGUIPropCtrl classes.
//
// Author:		Kenneth Moore
//
//------------------------------------------------------------------------------
//	Date		Revision    Description
//	02-19-2007	1.00		Original Release
//------------------------------------------------------------------------------
#if !defined(__GUICTRL_H__)
#define __GUICTRL_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <GuiObj.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define GUIPROPCTRL_COLUMN_NAME			0
#define GUIPROPCTRL_COLUMN_VALUE		1

#define GUIPROPCTRL_MIN_COLUMN_WIDTH	50

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
//	Extended list view to display CGUIObj derived objects
class CGUIListCtrl : public CListCtrl
{
	private:

		CImageList			m_Images;
		CGUICtrlItems*		m_pColumns;
		BOOL				m_bSuppress;
		BOOL				m_bUseImages;

	public:
	
	
							CGUIListCtrl();
		virtual			   ~CGUIListCtrl();

		virtual void		Clear();
		virtual void		Add(CObList* pGUIObjects, BOOL bClear);
		virtual void		AutoSizeColumns();
		virtual void		Sort(int iColumn);
		virtual void		Redraw(int iIndex, BOOL bAutoSizeColumns);
		virtual void		Redraw(CGUIObject* pGUIObject, BOOL bAutoSizeColumns);
		virtual void		Select(int iIndex, BOOL bSuppress);
		virtual void		Delete(int iIndex, BOOL bSuppress);
		virtual BOOL		Select(CGUIObject* pGUIObject, BOOL bSuppress = FALSE);
		virtual BOOL		Delete(CGUIObject* pGUIObject, BOOL bSuppress = FALSE);
		virtual BOOL		Initialize(CGUIObject* pOwnerClass, UINT uResourceId = 0);
		virtual BOOL		Add(CGUIObject* pGUIObject, BOOL bResizeColumns = TRUE);
		virtual BOOL		Insert(CGUIObject* pGUIObject, int iIndex, BOOL bResizeColumns = TRUE);
		virtual int			GetIndex(CGUIObject* pGUIObject);
		virtual int			GetIndexFromPt(POINT* pPoint);				
		virtual CGUIObject*	GetObjectFromPt(POINT* pPoint); 
		virtual CGUIObject*	GetSelection();

	protected:

		virtual void		AutoSizeColumn(int iIndex, CGUICtrlItem* pColumn);
		virtual BOOL		Add(CGUICtrlItem* pColumn, int iIndex);
		
		static int CALLBACK DoComparison(LPARAM, LPARAM, LPARAM);

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CGUIListCtrl)
	//}}AFX_VIRTUAL

	// Generated message map functions
	protected:
	//{{AFX_MSG(CGUIListCtrl)
	afx_msg void OnGetDispInfo(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg BOOL OnItemChanged(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg BOOL OnItemChanging(NMHDR* pNMHDR, LRESULT* pResult);
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
};

//	Simple property grid contorl to display CGUIObj derived objects
class CGUIPropCtrl : public CListCtrl
{
	private:

		CGUIObject*			m_pOwner;
		CGUICtrlItems*		m_pRows;
		BOOL				m_bSuppress;
		BOOL				m_bUseImages;
		CImageList			m_Images;

	public:
	
	
							CGUIPropCtrl();
		virtual			   ~CGUIPropCtrl();

		void				Clear();
		void				AutoSizeColumns();
		void				Redraw(int iIndex, BOOL bAutoSizeColumns);
		void				Redraw(CGUICtrlItem* pGUICtrlItem, BOOL bAutoSizeColumns);
		void				Select(int iIndex, BOOL bSuppress);
		void				Delete(int iIndex, BOOL bSuppress);
		BOOL				Select(CGUICtrlItem* pGUICtrlItem, BOOL bSuppress = FALSE);
		BOOL				Delete(CGUICtrlItem* pGUICtrlItem, BOOL bSuppress = FALSE);
		BOOL				Initialize(CGUIObject* pOwner, UINT uResourceId = 0);
		BOOL				Add(CGUICtrlItem* pGUICtrlItem, BOOL bResizeColumns = TRUE);
		BOOL				Insert(CGUICtrlItem* pGUICtrlItem, int iIndex, BOOL bResizeColumns = TRUE);
		BOOL				SetOwner(CGUIObject* pOwner);
		int					GetIndex(CGUICtrlItem* pGUICtrlItem);
		CGUICtrlItem*		GetSelection();

	protected:

		void				AutoSizeColumn(int iIndex);

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CGUIPropCtrl)
	//}}AFX_VIRTUAL

	// Generated message map functions
	protected:
	//{{AFX_MSG(CGUIPropCtrl)
	afx_msg void OnGetDispInfo(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg BOOL OnItemChanged(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg BOOL OnItemChanging(NMHDR* pNMHDR, LRESULT* pResult);
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
};

#endif // !defined(__GUICTRL_H__)

