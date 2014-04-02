//==============================================================================
//
// File Name:	drivctrl.h
//
// Description:	This file contains the declaration of the CDriveCtrl class.
//				This is a subclass of the CTreeCtrl class that is used to 
//				view the drives and folders.
//
// Author:		Kenneth Moore
//
//==============================================================================
//	Date		Revision    Description
//	07-09-97	1.00		Original Release
//	08-17-97	1.01		Removed DeleteFirstChild()
//	08-17-97	1.01		Added SetSelection() and GetItem()
//==============================================================================
#if !defined(__DRIVCTRL_H__)
#define __DRIVCTRL_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <filedata.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CDriveCtrl : public CTreeCtrl
{
	private:
	
		CString				m_strSelection;
		BOOL				m_bFirstHardDisk;
		BOOL				m_bAscending;
		BOOL				m_bSuppress;
		int					m_iSortField;

	public:

							CDriveCtrl();
		
		virtual int			Initialize(int iSortField = FILESORT_NONE,
									   BOOL bAscending = TRUE);
		virtual void		SetSortOptions(int iSortField, BOOL bAscending);
		virtual void		CollapseAll();
		virtual void		SetSelection(LPCSTR lpPath, BOOL bCollapseAll);
		virtual LPCSTR		GetSelection();
	
	protected:

		virtual int			AddFolders(HTREEITEM hItem, CString& strPath);
		virtual BOOL		AddDrive(CString& strDrive);
		virtual BOOL		SetButtonState(HTREEITEM hItem, CString& strPath);
		virtual CString		GetPathFromItem(HTREEITEM hItem);
		virtual CString&	GetDriveFromLabel(CString& strLabel);
		virtual HTREEITEM	GetItem(HTREEITEM hParent, LPCSTR lpPath);
		virtual void		DeleteAllChildren(HTREEITEM hItem);
		virtual void		OnSelectionChanged(CString& strPath);

	public:

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDriveCtrl)
	//}}AFX_VIRTUAL

	protected:

	//{{AFX_MSG(CDriveCtrl)
	afx_msg void OnItemExpanding(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg BOOL OnSelChanged(NMHDR* pNMHDR, LRESULT* pResult);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(__DRIVCTRL_H__)
