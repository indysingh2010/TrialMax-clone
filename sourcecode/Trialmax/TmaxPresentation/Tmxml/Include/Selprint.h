//==============================================================================
//
// File Name:	selprint.h
//
// Description:	This file contains the declaration of the CSelectPrint class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	07-26-01	1.00		Original Release
//==============================================================================
#if !defined(__SELPRINT_H__)
#define __SELPRINT_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <xmlmedia.h>
#include <mediactl.h>
#include <handler.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define CSP_DRAG_NONE		0
#define CSP_DRAG_TREE		1
#define CSP_DRAG_QUEUE		2

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
typedef struct
{
	CXmlMediaTree*	pTree;
	CXmlMedia*		pMedia;
	CXmlPage*		pPage;
	CXmlTreatment*	pTreatment;
}SGroupAnchors;

class CXmlFrame;

class CSelectPrint : public CDialog
{
	private:

		CXmlFrame*			m_pXmlFrame;
		CErrorHandler*		m_pErrors;
		SGroupAnchors		m_MediaAnchors;
		SGroupAnchors		m_QueueAnchors;
		int					m_iMediaSelections;
		int					m_iQueueSelections;
		BOOL				m_bRemoveGroup;

	public:
	
		HCURSOR				m_hArrow;
		HCURSOR				m_hDrag;
		HCURSOR				m_hNoDrop;
		CXmlMediaTrees		m_Queue;
		CXmlMediaTrees*		m_pXmlMediaTrees;
		CXmlMediaTree*		m_pXmlMediaTree;
		CXmlMediaTree*		m_pXmlQueueTree;
		CXmlMedia*			m_pXmlMedia;
		CXmlMedia*			m_pXmlQueueMedia;
		CXmlPage*			m_pXmlPage;
		CXmlPage*			m_pXmlQueuePage;
		CXmlTreatment*		m_pXmlTreatment;
		CXmlTreatment*		m_pXmlQueueTreatment;
		int					m_iDrag;
						
							CSelectPrint(CXmlFrame* pFrame = 0, 
										 CErrorHandler* pErrors = 0);
						   ~CSelectPrint();

	protected:

		void				EnablePageRange();
		void				AddPages();

		void				Detach(CXmlMediaTree* pXmlTree, BOOL bRedraw);
		void				Detach(CXmlMedia* pXmlMedia, BOOL bRedraw);
		void				Detach(CXmlPage* pXmlPage, BOOL bRedraw);
		void				Detach(CXmlTreatment* pXmlTreatment, BOOL bRedraw);

		void				Remove(CXmlMediaTree* pXmlTree);
		void				Remove(CXmlMedia* pXmlMedia);
		void				Remove(CXmlPage* pXmlPage);
		void				Remove(CXmlTreatment* pXmlTreatment);

		CXmlMediaTree*		Add(CXmlMediaTree* pXmlTree, BOOL bMedia, BOOL bTreatments);
		CXmlMedia*			Add(CXmlMediaTree* pQueueTree, CXmlMedia* pXmlMedia,
								BOOL bPages, BOOL bTreatments);
		CXmlPage*			Add(CXmlMedia* pQueueMedia, CXmlPage* pXmlPage,
								BOOL bChildren);
		CXmlTreatment*		Add(CXmlPage* pQueuePage, CXmlTreatment* pTreatment);

		//	Drag & Drop support
		void				OnEndTreeDrag();
		void				OnEndQueueDrag();
		void				SetTreeDragPos();
		void				SetQueueDragPos();

		//	Group selection support
		void				SelectMediaGroup();
		void				SelectQueueGroup();
		void				RemoveQueueGroup();
		void				AddMediaGroup();
		void				SelectTreatments(BOOL bQueue);
		void				SelectPages(BOOL bQueue, BOOL bTreatments);
		void				SelectMedia(BOOL bQueue, BOOL bTreatments);
		void				SelectTrees(BOOL bQueue, BOOL bTreatments);
		void				ClearSelections(BOOL bQueue, BOOL bRedraw);
		void				SetAnchors(BOOL bQueue, BOOL bSelect, BOOL bRedraw);
		void				SetAction(CXmlMediaTree* pTree, int iAction, 
									  CMediaCtrl* pTreeCtrl, BOOL bMedia,
									  BOOL bTreatments);
		void				SetAction(CXmlMedia* pMedia, int iAction, 
									  CMediaCtrl* pTreeCtrl, BOOL bPages,
									  BOOL bTreatments);
		void				SetAction(CXmlPage* pPage, int iAction, 
									  CMediaCtrl* pTreeCtrl, BOOL bChildren);
		void				SetAction(CXmlTreatment* pTreatment, int iAction, 
									  CMediaCtrl* pTreeCtrl);
		BOOL				CheckShiftState();

	//	The remainder of this declaration is maintained by Class Wizard
	public:

	//{{AFX_DATA(CSelectPrint)
	enum { IDD = IDD_SELECT_PRINT };
	CStatic	m_ctrlToLabel;
	CButton	m_ctrlQueuePages;
	CEdit	m_ctrlPageTo;
	CEdit	m_ctrlPageFrom;
	CStatic	m_ctrlFromLabel;
	CButton	m_ctrlOk;
	CButton	m_ctrlRemove;
	CButton	m_ctrlAdd;
	CMediaCtrl	m_ctrlQueue;
	CMediaCtrl	m_ctrlTrees;
	BOOL	m_bQueueTreatments;
	BOOL	m_bQueuePages;
	CString	m_strPageFrom;
	CString	m_strPageTo;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CSelectPrint)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

	protected:

	// Generated message map functions
	//{{AFX_MSG(CSelectPrint)
	virtual BOOL OnInitDialog();
	afx_msg void OnTreeChanged(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnQueueChanged(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnAdd();
	afx_msg void OnRemove();
	afx_msg void OnBeginQueueDrag(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnBeginTreeDrag(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg void OnTreeClick(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnQueueClick(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnQueuePages();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(__SELPRINT_H__)
