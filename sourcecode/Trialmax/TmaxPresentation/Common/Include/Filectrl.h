//==============================================================================
//
// File Name:	filectrl.h
//
// Description:	This file contains the declaration of the CFileCtrl class. This
//				class is derived from CListCtrl. It is used to create an 
//				Explorer-style file selection list control.
//
// Author:		Kenneth Moore
//
//==============================================================================
//	Date		Revision    Description
//	07-09-97	1.00		Original Release
//==============================================================================
#if !defined(__FILECTRL_H__)
#define __FILECTRL_H__

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

//	Column indexes
#define FILENAME_COLUMN		0
#define SIZE_COLUMN			1
#define DATE_COLUMN			2

//	Delimiter for filter strings
#define FILTER_DELIMITER	"|"

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CFileCtrl : public CListCtrl
{
	private:
	
		CStringArray		m_Filters;
		CFileDataList*		m_pFiles;
		CFileDataList*		m_pSelections;
		CString				m_strFolder;	
		
		BOOL				m_bMultiColumn;
		BOOL				m_bSmallIcons;
		BOOL				m_bUseFilter;
		BOOL				m_bAscending;
		int					m_iSortField;


	public:
	
							CFileCtrl();
						   ~CFileCtrl();

		virtual void		Update(CString& strFolder);
		virtual void		SetFilters(CString& strFilterString);
		virtual void		SetSortOptions(int iSortField, BOOL bAscending);
		virtual void		Reset();
		virtual BOOL		CheckFilters(CString& strExtension);
		virtual BOOL		InsertItem(int nIndex, CFileData* pFileData);
		virtual BOOL		Initialize(int iSortField = FILESORT_NONE, 
									   BOOL bAscending = TRUE);
		CFileDataList&		GetSelections();

	protected:

	
		static int CALLBACK DoComparison(LPARAM, LPARAM, LPARAM);


	public:
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CFileCtrl)
	//}}AFX_VIRTUAL

	protected:
	//{{AFX_MSG(CFileCtrl)
	afx_msg void OnGetDisplayInfo(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnColumnClick(NMHDR* pNMHDR, LRESULT* pResult);
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
};

#endif // !defined(__FILECTRL_H__)
