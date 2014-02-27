//==============================================================================
//
// File Name:	mediactl.h
//
// Description:	This file contains the declaration of the CMediaCtrl class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	05-28-01	1.00		Original Release
//==============================================================================
#if !defined(__MEDIACTL_H__)
#define __MEDIACTL_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Xml type identifiers
#define MEDIACTRL_TREE				1
#define MEDIACTRL_MEDIA				2
#define MEDIACTRL_PAGE				3
#define MEDIACTRL_TREATMENT			4

//	Image strip icon indexes
#define MEDIACTRL_ICON_TREE			0
#define MEDIACTRL_ICON_MEDIA		1
#define MEDIACTRL_ICON_PAGE			2
#define MEDIACTRL_ICON_TREATMENT	3
#define MEDIACTRL_ICON_QUEUED		4
#define MEDIACTRL_ICON_ADD			5
#define MEDIACTRL_ICON_REMOVE		6

//	Item action identifiers
#define MEDIACTRL_ACTION_NONE		0
#define MEDIACTRL_ACTION_ADD		1
#define MEDIACTRL_ACTION_REMOVE		2

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CXmlMediaTrees;
class CXmlMediaTree;
class CXmlMedia;
class CXmlPage;
class CXmlTreatment;

class CMediaCtrlItem
{
	private:

	public:

		HTREEITEM			m_hItem;
		int					m_iXmlType;		
		int					m_iAction;
		BOOL				m_bQueued;
		DWORD				m_dwUser1;
		DWORD				m_dwUser2;

							CMediaCtrlItem();
};

class CMediaCtrl : public CTreeCtrl
{
	private:

		CImageList			m_Images;
		CXmlMediaTrees*		m_pXmlTrees;

	public:

		CXmlMediaTree*		m_pXmlTree;
		CXmlMedia*			m_pXmlMedia;
		CXmlPage*			m_pXmlPage;
		CXmlTreatment*		m_pXmlTreatment;
	
							CMediaCtrl();
						   ~CMediaCtrl();

		void				Initialize(CXmlMediaTrees* pTrees);

		void				Select(CMediaCtrlItem* pItem);
		void				EnsureVisible(CMediaCtrlItem* pItem);

		void				Insert(CXmlMediaTree* pXmlTree);
		void				Insert(CXmlMediaTree* pTree, CXmlMedia* pXmlMedia);
		void				Insert(CXmlMedia* pMedia, CXmlPage* pXmlPage);
		void				Insert(CXmlPage* pPage, CXmlTreatment* pXmlTreatment);

		void				Remove(CMediaCtrlItem* pItem);
		void				Redraw(CMediaCtrlItem* pItem);

	protected:

		void				DeleteChildren(HTREEITEM hParent);

		void				GetDispInfo(TV_DISPINFO* pInfo, CXmlMediaTree* pTree);
		void				GetDispInfo(TV_DISPINFO* pInfo, CXmlMedia* pMedia);
		void				GetDispInfo(TV_DISPINFO* pInfo, CXmlPage* pPage);
		void				GetDispInfo(TV_DISPINFO* pInfo, CXmlTreatment* pTreatment);

		int					GetIconIndex(CMediaCtrlItem* pItem);

	//	The remainder of this declaration is maintained by ClassWizard

	public:

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMediaCtrl)
	//}}AFX_VIRTUAL

	// Generated message map functions
	protected:
	//{{AFX_MSG(CMediaCtrl)
	afx_msg void OnGetDispInfo(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg BOOL OnSelChanged(NMHDR* pNMHDR, LRESULT* pResult);
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__MEDIACTL_H__)
