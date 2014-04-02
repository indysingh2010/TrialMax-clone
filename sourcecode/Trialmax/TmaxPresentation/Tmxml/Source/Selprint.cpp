//==============================================================================
//
// File Name:	selprint.cpp
//
// Description:	This file contains member functions of the CSelectPrint class
//
// See Also:	selprint.h
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	07-26-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <selprint.h>
#include <xmlframe.h>

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
BEGIN_MESSAGE_MAP(CSelectPrint, CDialog)
	//{{AFX_MSG_MAP(CSelectPrint)
	ON_NOTIFY(TVN_SELCHANGED, IDC_TREES, OnTreeChanged)
	ON_NOTIFY(TVN_SELCHANGED, IDC_QUEUE, OnQueueChanged)
	ON_BN_CLICKED(IDC_ADD, OnAdd)
	ON_BN_CLICKED(IDC_REMOVE, OnRemove)
	ON_NOTIFY(TVN_BEGINDRAG, IDC_QUEUE, OnBeginQueueDrag)
	ON_NOTIFY(TVN_BEGINDRAG, IDC_TREES, OnBeginTreeDrag)
	ON_WM_LBUTTONUP()
	ON_WM_MOUSEMOVE()
	ON_NOTIFY(NM_CLICK, IDC_TREES, OnTreeClick)
	ON_NOTIFY(NM_CLICK, IDC_QUEUE, OnQueueClick)
	ON_BN_CLICKED(IDC_QUEUE_PAGES, OnQueuePages)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CSelectPrint::Add()
//
// 	Description:	This function is called to add a copy of the specified
//					page to the print queue.
//
// 	Returns:		A pointer to the queued object
//
//	Notes:			None
//
//==============================================================================
CXmlPage* CSelectPrint::Add(CXmlMedia* pQueueMedia, CXmlPage* pXmlPage,
							BOOL bTreatments)
{
	CXmlPage*		pQueuePage;
	CXmlTreatment*	pXmlTreatment;
	POSITION		Pos;

	ASSERT(pQueueMedia != 0);
	ASSERT(pXmlPage != 0);

	//	Is this page already in the queue?
	if(pXmlPage->m_dwUser1 != 0)
	{
		pQueuePage = (CXmlPage*)pXmlPage->m_dwUser1;
	}
	else
	{
		//	Make a copy of the specified page
		pQueuePage = new CXmlPage(pQueueMedia);
		ASSERT(pQueuePage);
		pQueuePage->SetAttributes(*pXmlPage);

		//	Add the copy to the queue
		pQueueMedia->m_Pages.Add(pQueuePage);
		m_ctrlQueue.Insert(pQueueMedia, pQueuePage);

		//	Link the two objects
		pQueuePage->m_dwUser1 = (DWORD)pXmlPage;
		pXmlPage->m_dwUser1 = (DWORD)pQueuePage;

		//	Set the queue state of this object
		pXmlPage->m_bQueued = TRUE;
		m_ctrlTrees.Redraw(pXmlPage);
	}

	//	Are we supposed to add all the treatmenets
	if(bTreatments)
	{
		Pos = pXmlPage->m_Treatments.GetHeadPosition();
		while(Pos != NULL)
		{
			if((pXmlTreatment = (CXmlTreatment*)pXmlPage->m_Treatments.GetNext(Pos)) != 0)
			{
				Add(pQueuePage, pXmlTreatment);
			}
		}
	}

	return pQueuePage;
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::Add()
//
// 	Description:	This function is called to add a copy of the specified
//					media object to the print queue.
//
// 	Returns:		A pointer to the queued object
//
//	Notes:			None
//
//==============================================================================
CXmlMedia* CSelectPrint::Add(CXmlMediaTree* pQueueTree, CXmlMedia* pXmlMedia,
							 BOOL bPages, BOOL bTreatments)
{
	CXmlMedia*	pQueueMedia;
	CXmlPage*	pXmlPage;
	POSITION	Pos;

	ASSERT(pQueueTree != 0);
	ASSERT(pXmlMedia != 0);

	//	Is this media object already in the queue?
	if(pXmlMedia->m_dwUser1 != 0)
	{
		pQueueMedia = (CXmlMedia*)pXmlMedia->m_dwUser1;
	}
	else
	{
		//	Make a copy of the specified media object
		pQueueMedia = new CXmlMedia(pQueueTree);
		ASSERT(pQueueMedia);
		pQueueMedia->SetAttributes(*pXmlMedia);

		//	Add the copy to the queue
		pQueueTree->Add(pQueueMedia);
		m_ctrlQueue.Insert(pQueueTree, pQueueMedia);

		//	Link the two objects
		pXmlMedia->m_dwUser1 = (DWORD)pQueueMedia;
		pQueueMedia->m_dwUser1 = (DWORD)pXmlMedia;
	}

	//	Are we supposed to add all the pages
	if(bPages)
	{
		Pos = pXmlMedia->m_Pages.GetHeadPosition();
		while(Pos != NULL)
		{
			if((pXmlPage = (CXmlPage*)pXmlMedia->m_Pages.GetNext(Pos)) != 0)
			{
				Add(pQueueMedia, pXmlPage, bTreatments);
			}
		}
	}

	return pQueueMedia;
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::Add()
//
// 	Description:	This function is called to add a copy of the specified
//					media tree to the print queue.
//
// 	Returns:		A pointer to the queued object
//
//	Notes:			None
//
//==============================================================================
CXmlMediaTree* CSelectPrint::Add(CXmlMediaTree* pXmlTree, BOOL bMedia, 
								 BOOL bTreatments)
{
	CXmlMediaTree*	pQueueTree;
	CXmlMedia*		pXmlMedia;
	POSITION		Pos;

	ASSERT(pXmlTree != 0);

	//	Is this tree already in the queue?
	if(pXmlTree->m_dwUser1 != 0)
	{
		pQueueTree = (CXmlMediaTree*)pXmlTree->m_dwUser1;
	}
	else
	{
		//	Make a copy of the specified media tree
		pQueueTree = new CXmlMediaTree();
		ASSERT(pQueueTree);
		pQueueTree->SetAttributes(*pXmlTree);

		//	Add the copy to the queue
		m_Queue.Add(pQueueTree);
		m_ctrlQueue.Insert(pQueueTree);

		//	Link the two objects
		pXmlTree->m_dwUser1 = (DWORD)pQueueTree;
		pQueueTree->m_dwUser1 = (DWORD)pXmlTree;
	}

	//	Are we supposed to add all the media objects?
	if(bMedia)
	{
		Pos = pXmlTree->GetHeadPosition();
		while(Pos != NULL)
		{
			if((pXmlMedia = (CXmlMedia*)pXmlTree->GetNext(Pos)) != 0)
				Add(pQueueTree, pXmlMedia, TRUE, bTreatments);
		}
	}

	return pQueueTree;
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::Add()
//
// 	Description:	This function is called to add a copy of the specified
//					treatment to the print queue.
//
// 	Returns:		A pointer to the queued object
//
//	Notes:			None
//
//==============================================================================
CXmlTreatment* CSelectPrint::Add(CXmlPage* pQueuePage, CXmlTreatment* pXmlTreatment)
{
	CXmlTreatment* pQueueTreatment;

	ASSERT(pQueuePage != 0);
	ASSERT(pXmlTreatment != 0);

	//	Is this media object already in the queue?
	if(pXmlTreatment->m_dwUser1 != 0)
	{
		pQueueTreatment = (CXmlTreatment*)pXmlTreatment->m_dwUser1;
	}
	else
	{
		//	Make a copy of the specified treatment
		pQueueTreatment = new CXmlTreatment(pQueuePage);
		ASSERT(pQueueTreatment);
		pQueueTreatment->SetAttributes(*pXmlTreatment);

		//	Add the copy to the queue
		pQueuePage->m_Treatments.Add(pQueueTreatment);
		m_ctrlQueue.Insert(pQueuePage, pQueueTreatment);

		//	Link the two objects
		pXmlTreatment->m_dwUser1 = (DWORD)pQueueTreatment;
		pQueueTreatment->m_dwUser1 = (DWORD)pXmlTreatment;

		//	Set the queue state of this object
		pXmlTreatment->m_bQueued = TRUE;
		m_ctrlTrees.Redraw(pXmlTreatment);
	}

	return pQueueTreatment;
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::AddMediaGroup()
//
// 	Description:	This function is called to add the group of media selections
//					to the print queue.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::AddMediaGroup() 
{
	CXmlMediaTree*	pQueueTree = 0;
	CXmlMedia*		pQueueMedia = 0;
	CXmlPage*		pQueuePage = 0;
	CXmlMediaTree*	pTree;
	CXmlMedia*		pMedia;
	CXmlPage*		pPage;
	CXmlTreatment*	pTreatment;
	POSITION		Pos;

	//	There must at least be a tree to add
	ASSERT(m_MediaAnchors.pTree != 0);
	if(m_MediaAnchors.pTree == 0)
		return;

	//	Get the current options
	UpdateData(TRUE);

	//	Make sure the parents nodes are added to the queue first
	if(m_MediaAnchors.pMedia != 0)
	{
		if((pQueueTree = Add(m_MediaAnchors.pTree, FALSE, FALSE)) == 0)
			return;
	}
	if(m_MediaAnchors.pPage != 0)
	{
		ASSERT(m_MediaAnchors.pMedia != 0);
		ASSERT(pQueueTree);
		if((pQueueMedia = Add(pQueueTree, m_MediaAnchors.pMedia, FALSE, FALSE)) == 0)
			return;
	}
	if(m_MediaAnchors.pTreatment != 0)
	{
		ASSERT(m_MediaAnchors.pPage != 0);
		ASSERT(pQueueMedia);
		if((pQueuePage = Add(pQueueMedia, m_MediaAnchors.pPage, FALSE)) == 0)
			return;
	}

	//	Are we anchored to a treatment?
	if((m_MediaAnchors.pTreatment != 0) && (m_MediaAnchors.pPage != 0))
	{
		ASSERT(pQueuePage);

		//	The current page selection should be the same as the anchor page
		ASSERT(m_pXmlPage == m_MediaAnchors.pPage);

		//	Add all of the selected treatments
		Pos = m_pXmlPage->m_Treatments.GetHeadPosition();
		while(Pos)
		{
			if((pTreatment = (CXmlTreatment*)m_pXmlPage->m_Treatments.GetNext(Pos)) != 0)
			{
				if(pTreatment->m_iAction == MEDIACTRL_ACTION_ADD)
					Add(pQueuePage, pTreatment);
			}
		}

	}

	//	Are we anchored to a page?
	else if((m_MediaAnchors.pPage != 0) && (m_MediaAnchors.pMedia != 0))
	{
		ASSERT(pQueueMedia);

		//	The current media selection should be the same as the anchor media
		ASSERT(m_pXmlMedia == m_MediaAnchors.pMedia);

		//	Add all of the selected pages
		Pos = m_pXmlMedia->m_Pages.GetHeadPosition();
		while(Pos)
		{
			if((pPage = (CXmlPage*)m_pXmlMedia->m_Pages.GetNext(Pos)) != 0)
			{
				if(pPage->m_iAction == MEDIACTRL_ACTION_ADD)
					Add(pQueueMedia, pPage, m_bQueueTreatments);
			}
		}
	}

	//	Are we anchored to a media object?
	else if((m_MediaAnchors.pMedia != 0) && (m_MediaAnchors.pTree != 0))
	{
		ASSERT(pQueueTree);

		//	The current tree selection should be the same as the anchor tree
		ASSERT(m_pXmlMediaTree == m_MediaAnchors.pTree);

		//	Add all of the selected pages
		Pos = m_pXmlMediaTree->GetHeadPosition();
		while(Pos)
		{
			if((pMedia = (CXmlMedia*)m_pXmlMediaTree->GetNext(Pos)) != 0)
			{
				if(pMedia->m_iAction == MEDIACTRL_ACTION_ADD)
					Add(pQueueTree, pMedia, TRUE, m_bQueueTreatments);
			}
		}
	}

	//	Are we anchored to a media tree?
	else if(m_pXmlMediaTrees != 0)
	{
		//	Add all of the selected trees
		Pos = m_pXmlMediaTrees->GetHeadPosition();
		while(Pos)
		{
			if((pTree = (CXmlMediaTree*)m_pXmlMediaTrees->GetNext(Pos)) != 0)
			{
				if(pTree->m_iAction == MEDIACTRL_ACTION_ADD)
					Add(pTree, TRUE, m_bQueueTreatments);
			}
		}
	}

	//	Clear the existing media selections
	SetAnchors(FALSE, FALSE, TRUE);
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::AddPages()
//
// 	Description:	This function is called to add the user defined range of
//					pages to the print queue.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::AddPages()
{
	CXmlPages		Pages;
	CXmlPage*		pPage;
	CXmlMedia*		pQueueMedia;
	CXmlMediaTree*	pQueueTree;
	POSITION		Pos;

	//	We must have an active document
	ASSERT(m_pXmlMediaTree);
	ASSERT(m_pXmlMedia);
	if((m_pXmlMediaTree == 0) || (m_pXmlMedia == 0))
		return;

	//	Make sure the page range is valid
	if(m_strPageFrom.IsEmpty() || m_strPageTo.IsEmpty())
	{
		//	Report the error
		if(m_pErrors)
			m_pErrors->Handle("Invalid Range", IDS_SELECT_INVALID_RANGE);
		return;
	}

	//	Find the first page in the requested range
	Pos = m_pXmlMedia->m_Pages.GetHeadPosition();
	while(Pos)
	{
		if((pPage = (CXmlPage*)m_pXmlMedia->m_Pages.GetNext(Pos)) != 0)
		{
			//	Is this the first page?
			if(lstrcmpi(pPage->m_strNumber, m_strPageFrom) == 0)
			{
				//	Add to the temporary list
				Pages.Add(pPage);
				break;
			}
		}
	}

	//	Now search for the last page
	while(Pos)
	{
		if((pPage = (CXmlPage*)m_pXmlMedia->m_Pages.GetNext(Pos)) != 0)
		{
			//	Is this the last page?
			if(lstrcmpi(pPage->m_strNumber, m_strPageTo) == 0)
			{
				//	Add to the temporary list
				Pages.Add(pPage);
				break;
			}
		}
	}

	//	Alert the user if not pages fall within the range
	if(Pages.GetCount() == 0)
	{
		//	Report the error
		if(m_pErrors)
			m_pErrors->Handle("Invalid Range", IDS_SELECT_NO_PAGES);
		return;
	}

	//	Make sure the parents nodes are added to the queue first
	if((pQueueTree = Add(m_pXmlMediaTree, FALSE, FALSE)) == 0)
			return;
	if((pQueueMedia = Add(pQueueTree, m_pXmlMedia, FALSE, FALSE)) == 0)
			return;

	//	Add each page to the queue
	pPage = Pages.First();
	while(pPage)
	{
		Add(pQueueMedia, pPage, m_bQueueTreatments);
		pPage = Pages.Next();
	}

	//	Select the first page in the range
	if((pPage = Pages.First()) != 0)
	{
		if(pPage->m_dwUser1 != 0)
			m_ctrlQueue.Select((CXmlPage*)pPage->m_dwUser1);
	}

	//	Flush the temporary list
	Pages.Flush(FALSE);
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::CheckShiftState()
//
// 	Description:	This function is called to determine if the Shift key is
//					currently pressed
//
// 	Returns:		TRUE if the Shift key is pressed
//
//	Notes:			None
//
//==============================================================================
BOOL CSelectPrint::CheckShiftState() 
{
	//	Is the shift key pressed?
	return (GetKeyState(VK_SHIFT) & 0x8000);
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::ClearMediaSelections()
//
// 	Description:	This function is called to clear the current media group
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::ClearSelections(BOOL bQueue, BOOL bRedraw) 
{
	CXmlMediaTrees*	pTrees;
	CXmlMediaTree*	pTree;
	CXmlMedia*		pMedia;
	CXmlPage*		pPage;
	CXmlTreatment*	pTreatment;
	CMediaCtrl*		pTreeCtrl;
	POSITION		Pos;

	//	Are we selecting within the print queue tree?
	if(bQueue)
	{
		pTrees = &m_Queue;
		pTree = m_QueueAnchors.pTree;
		pMedia = m_QueueAnchors.pMedia;
		pPage = m_QueueAnchors.pPage;
		pTreatment = m_QueueAnchors.pTreatment;
		pTreeCtrl = &m_ctrlQueue;
		m_iQueueSelections = 0;
	}
	else
	{
		pTrees = m_pXmlMediaTrees;
		pTree = m_MediaAnchors.pTree;
		pMedia = m_MediaAnchors.pMedia;
		pPage = m_MediaAnchors.pPage;
		pTreatment = m_MediaAnchors.pTreatment;
		pTreeCtrl = &m_ctrlTrees;
		m_iMediaSelections = 0;
	}

	//	Are we anchored to a treatment?
	if((pTreatment != 0) && (pPage != 0))
	{
		//	Clear all treatments in the page
		Pos = pPage->m_Treatments.GetHeadPosition();
		while(Pos)
		{
			if((pTreatment = (CXmlTreatment*)pPage->m_Treatments.GetNext(Pos)) != 0)
			{
				if(bRedraw)
					SetAction(pTreatment, MEDIACTRL_ACTION_NONE, pTreeCtrl);
				else
					SetAction(pTreatment, MEDIACTRL_ACTION_NONE, 0);
			}
		}
	}

	//	Are we anchored to a page?
	else if((pPage != 0) && (pMedia != 0))
	{
		//	Clear all pages in the document
		Pos = pMedia->m_Pages.GetHeadPosition();
		while(Pos)
		{
			if((pPage = (CXmlPage*)pMedia->m_Pages.GetNext(Pos)) != 0)
			{
				if(bRedraw)
					SetAction(pPage, MEDIACTRL_ACTION_NONE, pTreeCtrl, TRUE);
				else
					SetAction(pPage, MEDIACTRL_ACTION_NONE, 0, TRUE);
			}
		}
	}

	//	Are we anchored to a media object?
	else if((pMedia != 0) && (pTree != 0))
	{
		//	Clear all pages in the document
		Pos = pTree->GetHeadPosition();
		while(Pos)
		{
			if((pMedia = (CXmlMedia*)pTree->GetNext(Pos)) != 0)
			{
				if(bRedraw)
					SetAction(pMedia, MEDIACTRL_ACTION_NONE, pTreeCtrl, TRUE, TRUE);
				else
					SetAction(pMedia, MEDIACTRL_ACTION_NONE, 0, TRUE, TRUE);
			}
		}
	}

	//	Do we have a list of media trees?
	else if(pTrees != 0)
	{
		//	Clear all pages in the document
		Pos = pTrees->GetHeadPosition();
		while(Pos)
		{
			if((pTree = (CXmlMediaTree*)pTrees->GetNext(Pos)) != 0)
			{
				if(bRedraw)
					SetAction(pTree, MEDIACTRL_ACTION_NONE, pTreeCtrl, TRUE, TRUE);
				else
					SetAction(pTree, MEDIACTRL_ACTION_NONE, 0, TRUE, TRUE);
			}
		}
	}

}

//==============================================================================
//
// 	Function Name:	CSelectPrint::CSelectPrint()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CSelectPrint::CSelectPrint(CXmlFrame* pXmlFrame, CErrorHandler* pErrors) 
			 :CDialog(CSelectPrint::IDD, (CWnd*)pXmlFrame)
{
	//{{AFX_DATA_INIT(CSelectPrint)
	m_bQueueTreatments = FALSE;
	m_bQueuePages = FALSE;
	m_strPageFrom = _T("");
	m_strPageTo = _T("");
	//}}AFX_DATA_INIT

	m_pXmlFrame = pXmlFrame;
	m_pErrors = pErrors;
	m_pXmlMediaTrees = 0;
	m_pXmlMediaTree = 0;
	m_pXmlQueueTree = 0;
	m_pXmlMedia = 0;
	m_pXmlQueueMedia = 0;
	m_pXmlPage = 0;
	m_pXmlQueuePage = 0;
	m_pXmlTreatment = 0;
	m_pXmlQueueTreatment = 0;
	m_iDrag = CSP_DRAG_NONE;
	m_iMediaSelections = 0;
	m_iQueueSelections = 0;
	m_bRemoveGroup = FALSE;
	ZeroMemory(&m_MediaAnchors, sizeof(m_MediaAnchors));
	ZeroMemory(&m_QueueAnchors, sizeof(m_QueueAnchors));

	//	Load the drag/drop cursors we will use
	m_hArrow  = AfxGetApp()->LoadCursor(IDC_ARROW);
	m_hDrag   = AfxGetApp()->LoadCursor(IDC_DRAG);
	m_hNoDrop = AfxGetApp()->LoadCursor(IDC_NODROP);
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::~CSelectPrint()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CSelectPrint::~CSelectPrint()
{
	//	Flush the queue list
	m_Queue.Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::Detach()
//
// 	Description:	This function is called to detach the link that exists 
//					between the source object and its associated queue object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::Detach(CXmlPage* pXmlPage, BOOL bRedraw)
{
	POSITION		Pos;
	CXmlTreatment*	pTreatment;

	if(pXmlPage)
	{
		pXmlPage->m_dwUser1 = 0;
		pXmlPage->m_bQueued = FALSE;

		if(bRedraw && pXmlPage->m_hItem != 0)
			m_ctrlTrees.Redraw(pXmlPage);

		//	Detach all the children
		Pos = pXmlPage->m_Treatments.GetHeadPosition();
		while(Pos != NULL)
		{
			if((pTreatment = (CXmlTreatment*)pXmlPage->m_Treatments.GetNext(Pos)) != 0)
				Detach(pTreatment, bRedraw);
		}
	}
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::Detach()
//
// 	Description:	This function is called to detach the link that exists 
//					between the source object and its associated queue object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::Detach(CXmlMedia* pXmlMedia, BOOL bRedraw)
{
	POSITION	Pos;
	CXmlPage*	pPage;

	if(pXmlMedia)
	{
		pXmlMedia->m_dwUser1 = 0;
		pXmlMedia->m_bQueued = FALSE;

		if(bRedraw && pXmlMedia->m_hItem != 0)
			m_ctrlTrees.Redraw(pXmlMedia);

		//	Detach all the children
		Pos = pXmlMedia->m_Pages.GetHeadPosition();
		while(Pos != NULL)
		{
			if((pPage = (CXmlPage*)pXmlMedia->m_Pages.GetNext(Pos)) != 0)
				Detach(pPage, bRedraw);
		}
	}
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::Detach()
//
// 	Description:	This function is called to detach the link that exists 
//					between the source object and its associated queue object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::Detach(CXmlTreatment* pXmlTreatment, BOOL bRedraw)
{
	if(pXmlTreatment)
	{
		pXmlTreatment->m_dwUser1 = 0;
		pXmlTreatment->m_bQueued = FALSE;

		if(bRedraw && pXmlTreatment->m_hItem != 0)
			m_ctrlTrees.Redraw(pXmlTreatment);

	}
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::Detach()
//
// 	Description:	This function is called to detach the link that exists 
//					between the source object and its associated queue object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::Detach(CXmlMediaTree* pXmlTree, BOOL bRedraw)
{
	POSITION	Pos;
	CXmlMedia*	pMedia;

	if(pXmlTree)
	{
		pXmlTree->m_dwUser1 = 0;
		pXmlTree->m_bQueued = FALSE;

		if(bRedraw && pXmlTree->m_hItem != 0)
			m_ctrlTrees.Redraw(pXmlTree);

		//	Detach all the children
		Pos = pXmlTree->GetHeadPosition();
		while(Pos != NULL)
		{
			if((pMedia = (CXmlMedia*)pXmlTree->GetNext(Pos)) != 0)
				Detach(pMedia, bRedraw);
		}
	}
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the 
//					dialog box and their associated class members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CSelectPrint)
	DDX_Control(pDX, IDC_TO_LABEL, m_ctrlToLabel);
	DDX_Control(pDX, IDC_QUEUE_PAGES, m_ctrlQueuePages);
	DDX_Control(pDX, IDC_PAGE_TO, m_ctrlPageTo);
	DDX_Control(pDX, IDC_PAGE_FROM, m_ctrlPageFrom);
	DDX_Control(pDX, IDC_FROM_LABEL, m_ctrlFromLabel);
	DDX_Control(pDX, IDOK, m_ctrlOk);
	DDX_Control(pDX, IDC_REMOVE, m_ctrlRemove);
	DDX_Control(pDX, IDC_ADD, m_ctrlAdd);
	DDX_Control(pDX, IDC_QUEUE, m_ctrlQueue);
	DDX_Control(pDX, IDC_TREES, m_ctrlTrees);
	DDX_Check(pDX, IDC_QUEUE_TREATMENTS, m_bQueueTreatments);
	DDX_Check(pDX, IDC_QUEUE_PAGES, m_bQueuePages);
	DDX_Text(pDX, IDC_PAGE_FROM, m_strPageFrom);
	DDX_Text(pDX, IDC_PAGE_TO, m_strPageTo);
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::EnablePageRange()
//
// 	Description:	This function is called to enable/disable the page range
//					controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::EnablePageRange() 
{
	//	Do we have a document selected?
	if(m_pXmlMedia != 0)
	{
		m_ctrlQueuePages.EnableWindow(TRUE);
		m_ctrlFromLabel.EnableWindow(m_ctrlQueuePages.GetCheck());
		m_ctrlToLabel.EnableWindow(m_ctrlQueuePages.GetCheck());
		m_ctrlPageFrom.EnableWindow(m_ctrlQueuePages.GetCheck());
		m_ctrlPageTo.EnableWindow(m_ctrlQueuePages.GetCheck());
	}
	else
	{
		m_ctrlFromLabel.EnableWindow(FALSE);
		m_ctrlToLabel.EnableWindow(FALSE);
		m_ctrlQueuePages.SetCheck(0);
		m_ctrlQueuePages.EnableWindow(FALSE);
		m_ctrlPageFrom.SetWindowText("");
		m_ctrlPageFrom.EnableWindow(FALSE);
		m_ctrlPageTo.SetWindowText("");
		m_ctrlPageTo.EnableWindow(FALSE);
	}
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::OnAdd()
//
// 	Description:	This function is called when the user clicks on Add
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::OnAdd() 
{
	CXmlMediaTree*	pTree = 0;
	CXmlMedia*		pMedia = 0;
	CXmlPage*		pPage = 0;
	CXmlTreatment*	pTreatment = 0;
	BOOL			bTreatments;

	ASSERT(m_pXmlMediaTree);

	//	Do we have a group selection to add?
	if(m_iMediaSelections > 0)
	{
		AddMediaGroup();

		m_ctrlOk.EnableWindow(m_Queue.GetCount() > 0);
		return;
	}

	//	Get the user defined options
	UpdateData(TRUE);

	//	Are we supposed to add the user defined range of pages?
	if(m_bQueuePages)
	{
		AddPages();

		m_ctrlOk.EnableWindow(m_Queue.GetCount() > 0);
		return;
	}

	//	Do we have a media tree to add?
	if(m_pXmlMediaTree != 0)
	{
		//	Add the tree to the queue. Add all the child objects if
		//	no specific media object is selected
		pTree = Add(m_pXmlMediaTree, (m_pXmlMedia == 0), m_bQueueTreatments);
			
		//	Add the selected media object
		if((pTree != 0) && (m_pXmlMedia != 0))
		{
			pMedia = Add(pTree, m_pXmlMedia, (m_pXmlPage == 0), m_bQueueTreatments);

			//	Add the selected page
			if((pMedia != 0) && (m_pXmlPage != 0))
			{
				//	Should we also add all the treatments
				bTreatments = (m_bQueueTreatments && (m_pXmlTreatment == 0));

				pPage = Add(pMedia, m_pXmlPage, bTreatments);

				//	Add the selected treatment to the queue
				if((pPage != 0) && (m_pXmlTreatment != 0))
				{
					pTreatment = Add(pPage, m_pXmlTreatment);
				}

			}//if((pMedia != 0) && (m_pXmlPage != 0))
				
		}//if((pTree != 0) && (m_pXmlMedia != 0))

	}//if(m_pXmlMediaTree != 0)
	
	//	Set the current selection in the print queue
	if(pTreatment != 0)
		m_ctrlQueue.Select(pTreatment);
	else if(pPage != 0)
		m_ctrlQueue.Select(pPage);
	else if(pMedia != 0)
		m_ctrlQueue.Select(pMedia);
	else if(pTree != 0)
		m_ctrlQueue.Select(pTree);

	m_ctrlOk.EnableWindow(m_Queue.GetCount() > 0);
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::OnBeginQueueDrag()
//
// 	Description:	This function handles notifications fired when the user
//					begins to drag an item from the queue tree.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::OnBeginQueueDrag(NMHDR* pNMHDR, LRESULT* pResult) 
{
	NM_TREEVIEW*	pNMTreeView = (NM_TREEVIEW*)pNMHDR;
	CMediaCtrlItem*	pItem = (CMediaCtrlItem*)pNMTreeView->itemNew.lParam;
	CPoint			DragPos;

	ASSERT(pItem);
	if(pItem == 0)
		return;

	//	Set the drag state
	m_iDrag = CSP_DRAG_QUEUE;

	//	Make this the current selection in the queue
	m_ctrlQueue.Select(pItem);

	//	Set the cursor to indicate a drag operation is in effect
	SetCursor(m_hDrag);
	
	//	Capture the mouse input until the operation is complete
	SetCapture();

	*pResult = 0;
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::OnBeginTreeDrag()
//
// 	Description:	This function handles notifications fired when the user
//					begins to drag an item from the main media tree control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::OnBeginTreeDrag(NMHDR* pNMHDR, LRESULT* pResult) 
{
	NM_TREEVIEW*	pNMTreeView = (NM_TREEVIEW*)pNMHDR;
	CMediaCtrlItem*	pItem = (CMediaCtrlItem*)pNMTreeView->itemNew.lParam;

	ASSERT(pItem);
	if(pItem == 0)
		return;

	//	Set the drag state
	m_iDrag = CSP_DRAG_TREE;

	//	Make this the current selection in the queue
	m_ctrlTrees.Select(pItem);

	//	Set the cursor to indicate a drag operation is in effect
	SetCursor(m_hDrag);
	
	//	Capture the mouse input until the operation is complete
	SetCapture();

	*pResult = 0;
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::OnEndQueueDrag()
//
// 	Description:	This function is called when the user completes a drag 
//					operation from the print queue control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::OnEndQueueDrag() 
{
	//	Release the mouse
	ReleaseCapture();

	//	Restore the default cursor
	SetCursor(m_hArrow);

	//	Reset the drag state
	m_iDrag = CSP_DRAG_NONE;

	//	Remove the selection from the queue
	OnRemove();
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::OnEndTreeDrag()
//
// 	Description:	This function is called when the user completes a drag 
//					operation from the media tree control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::OnEndTreeDrag() 
{
	//	Release the mouse
	ReleaseCapture();

	//	Restore the default cursor
	SetCursor(m_hArrow);

	//	Reset the drag state
	m_iDrag = CSP_DRAG_NONE;

	//	Add the selection to the queue
	OnAdd();
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::OnInitDialog()
//
// 	Description:	This function handles the WM_INITDIALOG message
//
// 	Returns:		TRUE for default focus assignment
//
//	Notes:			None
//
//==============================================================================
BOOL CSelectPrint::OnInitDialog() 
{
	POSITION		Pos;
	CXmlMediaTree*	pTree;

	//	Do the base class processing first
	CDialog::OnInitDialog();
	
	//	Center the window within the client area of the frame window
	CenterWindow(m_pXmlFrame);

	//	Initialize the print queue control
	m_ctrlQueue.Initialize(&m_Queue);

	//	Do we have a valid list of media trees?
	if(m_pXmlMediaTrees != 0)
	{
		//	Make sure all the queue links are reset
		Pos = m_pXmlMediaTrees->GetHeadPosition();
		while(Pos != NULL)
		{
			if((pTree = (CXmlMediaTree*)m_pXmlMediaTrees->GetNext(Pos)) != 0)
				Detach(pTree, FALSE);
		}

		//	Initialize the tree selection control
		m_ctrlTrees.Initialize(m_pXmlMediaTrees);

		//	Set the current selection
		if(m_pXmlTreatment != 0)
			m_ctrlTrees.Select(m_pXmlTreatment);
		else if(m_pXmlPage != 0)
			m_ctrlTrees.Select(m_pXmlPage);
		else if(m_pXmlMedia != 0)
			m_ctrlTrees.Select(m_pXmlMedia);
		else if(m_pXmlMediaTree != 0)
			m_ctrlTrees.Select(m_pXmlMediaTree);
	}
	else
	{
		m_ctrlTrees.Initialize(0);
		m_pXmlTreatment = 0;
		m_pXmlPage = 0;
		m_pXmlMedia = 0;
		m_pXmlMediaTree = 0;
	}

	//	Enable the page range controls
	EnablePageRange();

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::OnLButtonUp()
//
// 	Description:	This function handles all WM_LBUTTONUP messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::OnLButtonUp(UINT nFlags, CPoint point) 
{
	//	Are we dragging?
	switch(m_iDrag)
	{
		case CSP_DRAG_TREE:

			OnEndTreeDrag();
			break;

		case CSP_DRAG_QUEUE:

			OnEndQueueDrag();
			break;

	}
	CDialog::OnLButtonUp(nFlags, point);
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::OnMouseMove()
//
// 	Description:	This function handles all WM_MOUSEMOVE messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::OnMouseMove(UINT nFlags, CPoint point) 
{
	//	Are we dragging?
	switch(m_iDrag)
	{
		case CSP_DRAG_TREE:

			SetTreeDragPos();
			break;

		case CSP_DRAG_QUEUE:

			SetQueueDragPos();
			break;

	}
	CDialog::OnMouseMove(nFlags, point);
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::OnQueueChanged()
//
// 	Description:	This function is called when the selection in the queue
//					control changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::OnQueueChanged(NMHDR* pNMHDR, LRESULT* pResult) 
{
	//	Don't process changes if we are removing a group of selections
	if(m_bRemoveGroup)
		return;

	//	Save the new selection
	m_pXmlQueueTree = m_ctrlQueue.m_pXmlTree;
	m_pXmlQueueMedia = m_ctrlQueue.m_pXmlMedia;
	m_pXmlQueuePage = m_ctrlQueue.m_pXmlPage;
	m_pXmlQueueTreatment = m_ctrlQueue.m_pXmlTreatment;
		
	//	Set the group select anchors
	SetAnchors(TRUE, CheckShiftState(), TRUE);

	//	Is there a valid selection?
	m_ctrlRemove.EnableWindow(m_ctrlQueue.m_pXmlTree != 0);

	//	Make the group selection
	if(CheckShiftState())
		SelectQueueGroup();

	if(pResult) *pResult = 0;
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::OnQueueClick()
//
// 	Description:	This function traps notifications fired when the user
//					clicks on an item in the print queue tree control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::OnQueueClick(NMHDR* pNMHDR, LRESULT* pResult) 
{
	//	Is the user pressing the shift key?
	if(CheckShiftState())
	{
		//	Set the anchors and update the selections
		SetAnchors(TRUE, TRUE, TRUE);
		SelectQueueGroup();
	}
	else
	{
		//	Reset the anchors and clear the selections
		SetAnchors(TRUE, FALSE, TRUE);
	}
	*pResult = 0;
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::OnQueuePages()
//
// 	Description:	This function is called when the user clicks on the
//					Queue Pages check box.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::OnQueuePages() 
{
	EnablePageRange();
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::OnRemove()
//
// 	Description:	This function is called when the user clicks on Remove
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::OnRemove() 
{
	//	Do we have a group selection to remove?
	if(m_iQueueSelections > 0)
	{
		RemoveQueueGroup();
	}

	//	Remove the selected object
	else if(m_pXmlQueueTreatment != 0)
	{
		Remove(m_pXmlQueueTreatment);
	}
	else if(m_pXmlQueuePage != 0)
	{
		Remove(m_pXmlQueuePage);
	}
	else if(m_pXmlQueueMedia != 0)
	{
		Remove(m_pXmlQueueMedia);
	}
	else if(m_pXmlQueueTree != 0)
	{
		Remove(m_pXmlQueueTree);
	}

	//	Is there anything left in the queue?
	if(m_Queue.GetCount() > 0)
	{
		m_ctrlOk.EnableWindow(TRUE);
	}
	else
	{
		m_ctrlOk.EnableWindow(FALSE);
		m_ctrlRemove.EnableWindow(FALSE);
	}
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::OnTreeChanged()
//
// 	Description:	This function is called when the selection in the media
//					trees control changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::OnTreeChanged(NMHDR* pNMHDR, LRESULT* pResult) 
{
	//	Save the new selection
	m_pXmlMediaTree = m_ctrlTrees.m_pXmlTree;
	m_pXmlMedia = m_ctrlTrees.m_pXmlMedia;
	m_pXmlPage = m_ctrlTrees.m_pXmlPage;
	m_pXmlTreatment = m_ctrlTrees.m_pXmlTreatment;
		
	//	Set the group select anchors
	SetAnchors(FALSE, CheckShiftState(), TRUE);

	//	Is there a valid selection?
	m_ctrlAdd.EnableWindow(m_ctrlTrees.m_pXmlTree != 0);

	//	Make the group selection
	if(CheckShiftState())
		SelectMediaGroup();

	//	Enable the page range controls
	EnablePageRange();

	if(pResult) *pResult = 0;
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::OnTreeClick()
//
// 	Description:	This function traps notifications fired when the user
//					clicks on an item in the media tree control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::OnTreeClick(NMHDR* pNMHDR, LRESULT* pResult) 
{
	//	Is the user pressing the shift key?
	if(CheckShiftState())
	{
		//	Set the anchors and update the selections
		SetAnchors(FALSE, TRUE, TRUE);
		SelectMediaGroup();
	}
	else
	{
		//	Reset the anchors and clear the selections
		SetAnchors(FALSE, FALSE, TRUE);
	}
	*pResult = 0;
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::Remove()
//
// 	Description:	This function is called to remove the specified media object
//					from the print queue.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::Remove(CXmlMedia* pMedia) 
{
	CXmlMediaTree* pTree;

	ASSERT(pMedia);
	if(pMedia == 0)
		return;

	//	Detach the link between the source object and the queue object
	Detach((CXmlMedia*)pMedia->m_dwUser1, TRUE);

	//	Remove from the display tree
	m_ctrlQueue.Remove(pMedia);

	//	Get the media tree that owns this object
	if((pTree = pMedia->m_pXmlMediaTree) != 0)
	{
		//	Remove from the parent object's list
		pTree->Remove(pMedia, TRUE);

		//	Remove the tree if there are no more media objects 
		if(pTree->GetCount() == 0)
			Remove(pTree);
	}
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::Remove()
//
// 	Description:	This function is called to remove the specified tree from
//					the queue.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::Remove(CXmlMediaTree* pXmlTree)
{
	ASSERT(pXmlTree);
	if(pXmlTree == 0)
		return;

	//	Detach the link between the source object and the queue object
	Detach((CXmlMediaTree*)pXmlTree->m_dwUser1, TRUE);

	//	Remove from the tree
	m_ctrlQueue.Remove(pXmlTree);

	//	Remove from the queue
	m_Queue.Remove(pXmlTree, TRUE);
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::Remove()
//
// 	Description:	This function is called to remove the specified page object
//					from the print queue.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::Remove(CXmlPage* pPage) 
{
	CXmlMedia*	pMedia;
	CXmlPage*	pSource;

	ASSERT(pPage);
	if(pPage == 0)
		return;

	//	Detach the link between the source object and the queue object
	if((pSource = (CXmlPage*)pPage->m_dwUser1) != 0)
	{
		Detach(pSource, TRUE);
	}

	//	Remove from the display tree
	m_ctrlQueue.Remove(pPage);

	//	Get the media tree that owns this object
	if((pMedia = pPage->m_pXmlMedia) != 0)
	{
		//	Remove from the parent object's list
		pMedia->m_Pages.Remove(pPage, TRUE);

		//	Remove the media object if there are no more pages
		if(pMedia->m_Pages.GetCount() == 0)
			Remove(pMedia);
	}
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::Remove()
//
// 	Description:	This function is called to remove the specified treatment
//					from the print queue.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::Remove(CXmlTreatment* pTreatment) 
{
	CXmlPage*		pPage;
	CXmlTreatment*	pSource;

	ASSERT(pTreatment);
	if(pTreatment == 0)
		return;

	//	Detach the link between the source object and the queue object
	if((pSource = (CXmlTreatment*)pTreatment->m_dwUser1) != 0)
	{
		Detach(pSource, TRUE);
	}

	//	Remove from the tree
	m_ctrlQueue.Remove(pTreatment);

	//	Get the page that owns this treatment
	if((pPage = pTreatment->m_pXmlPage) != 0)
	{
		//	Remove from the parent object's list
		pPage->m_Treatments.Remove(pTreatment, TRUE);
	}
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::SelectTrees()
//
// 	Description:	This function is called to select a range of media trees
//					from within one of the two tree controls.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::SelectTrees(BOOL bQueue, BOOL bTreatments) 
{
	CXmlMediaTrees*	pTrees;
	CXmlMediaTree*	pAnchor;
	CXmlMediaTree*	pSelection;
	CXmlMediaTree*	pCurrent;
	CMediaCtrl*		pTreeCtrl;
	int				iAction;
	POSITION		Pos;
	BOOL			bSelect = FALSE;
	BOOL			bContinue = TRUE;

	//	Are we selecting within the print queue tree?
	if(bQueue)
	{
		pTrees = &m_Queue;
		pSelection = m_pXmlQueueTree;
		pAnchor = m_QueueAnchors.pTree;
		pTreeCtrl = &m_ctrlQueue;
		iAction = MEDIACTRL_ACTION_REMOVE;
		m_iQueueSelections = 0;
	}
	else
	{
		pTrees = m_pXmlMediaTrees;
		pSelection = m_pXmlMediaTree;
		pAnchor = m_MediaAnchors.pTree;
		pTreeCtrl = &m_ctrlTrees;
		iAction = MEDIACTRL_ACTION_ADD;
		m_iMediaSelections = 0;
	}

	ASSERT(pTrees);
	ASSERT(pSelection);
	ASSERT(pAnchor);
	if(pTrees == 0 || pSelection == 0 || pAnchor == 0)
		return;

	//	Set the selection state of each media tree
	Pos = pTrees->GetHeadPosition();
	while(Pos && bContinue)
	{
		//	Get the page object
		if((pCurrent = (CXmlMediaTree*)pTrees->GetNext(Pos)) == 0)
			continue;

		//	Is this the anchor?
		if(pCurrent == pAnchor)
		{
			if(bQueue)
				m_iQueueSelections++;
			else
				m_iMediaSelections++;

			//	Set the action identifier
			SetAction(pCurrent, iAction, pTreeCtrl, TRUE, bTreatments);

			//	Is this also the current selection?
			if(pCurrent == pSelection)
				bSelect = FALSE;
			else
				bSelect = TRUE;

			//	Break out of the loop
			bContinue = FALSE;
		}
			
		//	Is this the current selection?
		else if(pCurrent == pSelection)
		{
			bSelect = TRUE;

			if(bQueue)
				m_iQueueSelections++;
			else
				m_iMediaSelections++;

			//	Set the action identifier
			SetAction(pCurrent, iAction, pTreeCtrl, TRUE, bTreatments);

			//	Break out of the loop
			bContinue = FALSE;
		}
			
		//	We haven't reached one of the extents yet
		else
		{
			//	No action to be taken on this object
			SetAction(pCurrent, MEDIACTRL_ACTION_NONE, pTreeCtrl, TRUE, TRUE);
		}
	}

	//	Now set the remaining objects
	while(Pos)
	{
		//	Get the treatment object
		if((pCurrent = (CXmlMediaTree*)pTrees->GetNext(Pos)) == 0)
			continue;

		//	Is this one of the extents?
		if((pCurrent == pAnchor) || (pCurrent == pSelection))
		{
			SetAction(pCurrent, iAction, pTreeCtrl, TRUE, bTreatments);
			if(bQueue)
				m_iQueueSelections++;
			else
				m_iMediaSelections++;
			bSelect = FALSE;
		}
		else
		{
			if(bSelect) 
			{
				if(bQueue)
					m_iQueueSelections++;
				else
					m_iMediaSelections++;
				SetAction(pCurrent, iAction, pTreeCtrl, TRUE, bTreatments);
			}
			else
			{
				//	This object is no longer selected
				SetAction(pCurrent, MEDIACTRL_ACTION_NONE, pTreeCtrl, TRUE, TRUE);
			}
		}
	}
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::SetAction()
//
// 	Description:	This function is called to set the action of the specified
//					media object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::SetAction(CXmlMedia* pMedia, int iAction, 
						     CMediaCtrl* pTreeCtrl, BOOL bPages, 
							 BOOL bTreatments) 
{
	POSITION	Pos;
	CXmlPage*	pPage;

	ASSERT(pMedia);

	pMedia->m_iAction = iAction;
		
	//	Should we redraw the item?
	if(pTreeCtrl)
		pTreeCtrl->Redraw(pMedia);

	//	Should we set the action of all the pages?
	if(bPages)
	{
		Pos = pMedia->m_Pages.GetHeadPosition();
		while(Pos)
		{
			if((pPage = (CXmlPage*)pMedia->m_Pages.GetNext(Pos)) != 0)
			{
				SetAction(pPage, iAction, pTreeCtrl, bTreatments);
			}
		}
	}
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::SetAction()
//
// 	Description:	This function is called to set the action of the specified
//					media tree.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::SetAction(CXmlMediaTree* pTree, int iAction, 
						     CMediaCtrl* pTreeCtrl, BOOL bMedia, BOOL bTreatments) 
{
	POSITION	Pos;
	CXmlMedia*	pMedia;

	ASSERT(pTree);

	pTree->m_iAction = iAction;
		
	//	Should we redraw the item?
	if(pTreeCtrl)
		pTreeCtrl->Redraw(pTree);

	//	Should we set the action of all the children?
	if(bMedia)
	{
		Pos = pTree->GetHeadPosition();
		while(Pos)
		{
			if((pMedia = (CXmlMedia*)pTree->GetNext(Pos)) != 0)
			{
				SetAction(pMedia, iAction, pTreeCtrl, TRUE, bTreatments);
			}
		}
	}
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::SetAction()
//
// 	Description:	This function is called to set the action of the specified
//					page.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::SetAction(CXmlPage* pPage, int iAction, 
						    CMediaCtrl* pTreeCtrl, BOOL bChildren) 
{
	POSITION		Pos;
	CXmlTreatment*	pTreatment;

	ASSERT(pPage);

	pPage->m_iAction = iAction;
		
	//	Should we redraw the item?
	if(pTreeCtrl)
		pTreeCtrl->Redraw(pPage);

	//	Should we set the action of all the children?
	if(bChildren)
	{
		Pos = pPage->m_Treatments.GetHeadPosition();
		while(Pos)
		{
			if((pTreatment = (CXmlTreatment*)pPage->m_Treatments.GetNext(Pos)) != 0)
			{
				SetAction(pTreatment, iAction, pTreeCtrl);
			}
		}
	}
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::SetAnchors()
//
// 	Description:	This function is called to set the anchors used to perform
//					group selections.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::SetAnchors(BOOL bQueue, BOOL bSelect, BOOL bRedraw) 
{
	SGroupAnchors*	pAnchors;
	CXmlMediaTree*	pTree;
	CXmlMedia*		pMedia;
	CXmlPage*		pPage;
	CXmlTreatment*	pTreatment;

	//	Are we selecting within the print queue tree?
	if(bQueue)
	{
		pTree = m_pXmlQueueTree;
		pMedia = m_pXmlQueueMedia;
		pPage = m_pXmlQueuePage;
		pTreatment = m_pXmlQueueTreatment;
		pAnchors = &m_QueueAnchors;
	}
	else
	{
		pTree = m_pXmlMediaTree;
		pMedia = m_pXmlMedia;
		pPage = m_pXmlPage;
		pTreatment = m_pXmlTreatment;
		pAnchors = &m_MediaAnchors;
	}

	//	Are we doing group selection?
	if(bSelect)
	{
		//	Set the anchor point as long as it has not already been set
		if(pAnchors->pTree == 0)
		{
			//	Anchor to the current selections
			pAnchors->pTree = pTree;
			pAnchors->pMedia = pMedia;
			pAnchors->pPage = pPage;
			pAnchors->pTreatment = pTreatment;
		}
	}
	else
	{
		//	Have we got an anchor?
		if(pAnchors->pTree != 0)
		{
			//	Clear the current selections
			ClearSelections(bQueue, bRedraw);

			//	Clear the anchors
			pAnchors->pTree = 0;
			pAnchors->pMedia = 0;
			pAnchors->pPage = 0;
			pAnchors->pTreatment = 0;
		}
	}
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::RemoveQueueGroup()
//
// 	Description:	This function is called to remove the current group
//					selection from the print queue.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::RemoveQueueGroup() 
{
	CXmlMediaTree*	pAnchorTree;
	CXmlMedia*		pAnchorMedia;
	CXmlPage*		pAnchorPage;
	CXmlTreatment*	pAnchorTreatment;
	CXmlMediaTree*	pTree;
	CXmlMedia*		pMedia;
	CXmlPage*		pPage;
	CXmlTreatment*	pTreatment;
	POSITION		Pos;

	//	Reset the current queue selections and inhibit change processing
	m_bRemoveGroup = TRUE;
	m_pXmlQueueTree = 0;
	m_pXmlQueueMedia = 0;
	m_pXmlQueuePage = 0;
	m_pXmlQueueTreatment = 0;

	//	Initialize the local pointers and reset the anchor
	pAnchorTree = m_QueueAnchors.pTree;
	pAnchorMedia = m_QueueAnchors.pMedia;
	pAnchorPage = m_QueueAnchors.pPage;
	pAnchorTreatment = m_QueueAnchors.pTreatment;
	ZeroMemory(&m_QueueAnchors, sizeof(m_QueueAnchors));

	//	Are we anchored to a treatment?
	if((pAnchorTreatment != 0) && (pAnchorPage != 0))
	{
		//	Remove all of the selected treatments
		Pos = pAnchorPage->m_Treatments.GetHeadPosition();
		while(Pos)
		{
			if((pTreatment = (CXmlTreatment*)pAnchorPage->m_Treatments.GetNext(Pos)) != 0)
			{
				if(pTreatment->m_iAction == MEDIACTRL_ACTION_REMOVE)
					Remove(pTreatment);
			}
		}

	}

	//	Are we anchored to a page?
	else if((pAnchorPage != 0) && (pAnchorMedia != 0))
	{
		//	Remove all of the selected pages
		Pos = pAnchorMedia->m_Pages.GetHeadPosition();
		while(Pos)
		{
			if((pPage = (CXmlPage*)pAnchorMedia->m_Pages.GetNext(Pos)) != 0)
			{
				if(pPage->m_iAction == MEDIACTRL_ACTION_REMOVE)
					Remove(pPage);
			}
		}
	}

	//	Are we anchored to a media object?
	else if((pAnchorMedia != 0) && (pAnchorTree != 0))
	{
		//	Remove all of the selected media objects
		Pos = pAnchorTree->GetHeadPosition();
		while(Pos)
		{
			if((pMedia = (CXmlMedia*)pAnchorTree->GetNext(Pos)) != 0)
			{
				if(pMedia->m_iAction == MEDIACTRL_ACTION_REMOVE)
					Remove(pMedia);
			}
		}
	}

	//	Are we anchored to a media tree?
	else if(pAnchorTree != 0)
	{
		//	Add all of the selected trees
		Pos = m_Queue.GetHeadPosition();
		while(Pos)
		{
			if((pTree = (CXmlMediaTree*)m_Queue.GetNext(Pos)) != 0)
			{
				if(pTree->m_iAction == MEDIACTRL_ACTION_REMOVE)
					Remove(pTree);
			}
		}
	}

	//	Update the current selection
	m_bRemoveGroup = FALSE;
	OnQueueChanged(0, 0);
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::SelectMedia()
//
// 	Description:	This function is called to select a range of media objects
//					from within one of the two tree controls.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::SelectMedia(BOOL bQueue, BOOL bTreatments) 
{
	CXmlMediaTree*	pTree;
	CXmlMedia*		pAnchor;
	CXmlMedia*		pSelection;
	CXmlMedia*		pCurrent;
	CMediaCtrl*		pTreeCtrl;
	int				iAction;
	POSITION		Pos;
	BOOL			bSelect = FALSE;
	BOOL			bContinue = TRUE;

	//	Are we selecting within the print queue tree?
	if(bQueue)
	{
		pTree = m_pXmlQueueTree;
		pSelection = m_pXmlQueueMedia;
		pAnchor = m_QueueAnchors.pMedia;
		pTreeCtrl = &m_ctrlQueue;
		iAction = MEDIACTRL_ACTION_REMOVE;
		m_iQueueSelections = 0;
	}
	else
	{
		pTree = m_pXmlMediaTree;
		pSelection = m_pXmlMedia;
		pAnchor = m_MediaAnchors.pMedia;
		pTreeCtrl = &m_ctrlTrees;
		iAction = MEDIACTRL_ACTION_ADD;
		m_iMediaSelections = 0;
	}

	ASSERT(pTree);
	ASSERT(pSelection);
	ASSERT(pAnchor);
	if(pTree == 0 || pSelection == 0 || pAnchor == 0)
		return;

	//	Set the selection state of each media object
	Pos = pTree->GetHeadPosition();
	while(Pos && bContinue)
	{
		//	Get the page object
		if((pCurrent = (CXmlMedia*)pTree->GetNext(Pos)) == 0)
			continue;

		//	Is this the anchor?
		if(pCurrent == pAnchor)
		{
			if(bQueue)
				m_iQueueSelections++;
			else
				m_iMediaSelections++;

			//	Set the action identifier
			SetAction(pCurrent, iAction, pTreeCtrl, TRUE, bTreatments);

			//	Is this also the current selection?
			if(pCurrent == pSelection)
				bSelect = FALSE;
			else
				bSelect = TRUE;

			//	Break out of the loop
			bContinue = FALSE;
		}
			
		//	Is this the current selection?
		else if(pCurrent == pSelection)
		{
			bSelect = TRUE;

			if(bQueue)
				m_iQueueSelections++;
			else
				m_iMediaSelections++;

			//	Set the action identifier
			SetAction(pCurrent, iAction, pTreeCtrl, TRUE, bTreatments);

			//	Break out of the loop
			bContinue = FALSE;
		}
			
		//	We haven't reached one of the extents yet
		else
		{
			//	No action to be taken on this object
			SetAction(pCurrent, MEDIACTRL_ACTION_NONE, pTreeCtrl, TRUE, TRUE);
		}
	}

	//	Now set the remaining objects
	while(Pos)
	{
		//	Get the treatment object
		if((pCurrent = (CXmlMedia*)pTree->GetNext(Pos)) == 0)
			continue;

		//	Is this one of the extents?
		if((pCurrent == pAnchor) || (pCurrent == pSelection))
		{
			SetAction(pCurrent, iAction, pTreeCtrl, TRUE, bTreatments);
			if(bQueue)
				m_iQueueSelections++;
			else
				m_iMediaSelections++;
			bSelect = FALSE;
		}
		else
		{
			if(bSelect) 
			{
				if(bQueue)
					m_iQueueSelections++;
				else
					m_iMediaSelections++;
				SetAction(pCurrent, iAction, pTreeCtrl, TRUE, bTreatments);
			}
			else
			{
				//	This object is no longer selected
				SetAction(pCurrent, MEDIACTRL_ACTION_NONE, pTreeCtrl, TRUE, TRUE);
			}
		}
	}
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::SelectMediaGroup()
//
// 	Description:	This function is called to select a range of objects in the
//					media tree.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::SelectMediaGroup() 
{
	//	Get the current options
	UpdateData(TRUE);

	//	Are we anchored to a treatment?
	if((m_MediaAnchors.pTreatment != 0) && (m_pXmlPage != 0))
	{
		//	Clear all selections if a different page object has been selected
		//	or if a treatment has not been selected
		if((m_pXmlTreatment == 0) || (m_pXmlPage != m_MediaAnchors.pPage))
		{
			//	Clear the existing selections
			SetAnchors(FALSE, FALSE, TRUE);
		}
		else
		{
			//	Select a range of treatments
			SelectTreatments(FALSE);
		}
	}

	//	Are we anchored to a page?
	else if((m_MediaAnchors.pPage != 0) && (m_pXmlMedia != 0))
	{
		//	Clear all selections if a different media object has been selected
		//	or if a page has not been selected
		if((m_pXmlPage == 0) || (m_pXmlMedia != m_MediaAnchors.pMedia))
		{
			//	Clear the existing selections
			SetAnchors(FALSE, FALSE, TRUE);
		}
		else
		{
			//	Select a range of pages
			SelectPages(FALSE, m_bQueueTreatments);
		}
	}

	//	Are we anchored to a media object?
	else if((m_MediaAnchors.pMedia != 0) && (m_pXmlMediaTree != 0))
	{
		//	Clear all selections if a different tree object has been selected
		//	or if a media has not been selected
		if((m_pXmlMedia == 0) || (m_pXmlMediaTree != m_MediaAnchors.pTree))
		{
			//	Clear the existing selections
			SetAnchors(FALSE, FALSE, TRUE);
		}
		else
		{
			//	Select a range of media objects
			SelectMedia(FALSE, m_bQueueTreatments);
		}
	}

	//	Are we anchored to a media tree?
	else if((m_MediaAnchors.pTree != 0) && (m_pXmlMediaTrees != 0))
	{
		//	Clear all selections if a media tree has not been selected
		if(m_pXmlMediaTree == 0)
		{
			//	Clear the existing selections
			SetAnchors(FALSE, FALSE, TRUE);
		}
		else
		{
			//	Select a range of media trees
			SelectTrees(FALSE, m_bQueueTreatments);
		}
	}

	else
	{
		//	Clear the existing media selections
		SetAnchors(FALSE, FALSE, TRUE);
	}
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::SelectPages()
//
// 	Description:	This function is called to select a range of pages
//					from within one of the two tree controls.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::SelectPages(BOOL bQueue, BOOL bTreatments) 
{
	CXmlMedia*	pMedia;
	CXmlPage*	pAnchor;
	CXmlPage*	pSelection;
	CXmlPage*	pCurrent;
	CMediaCtrl*	pTreeCtrl;
	int			iAction;
	POSITION	Pos;
	BOOL		bSelect = FALSE;
	BOOL		bContinue = TRUE;

	//	Are we selecting within the print queue tree?
	if(bQueue)
	{
		pMedia = m_pXmlQueueMedia;
		pSelection = m_pXmlQueuePage;
		pAnchor = m_QueueAnchors.pPage;
		pTreeCtrl = &m_ctrlQueue;
		iAction = MEDIACTRL_ACTION_REMOVE;
		m_iQueueSelections = 0;
	}
	else
	{
		pMedia = m_pXmlMedia;
		pSelection = m_pXmlPage;
		pAnchor = m_MediaAnchors.pPage;
		pTreeCtrl = &m_ctrlTrees;
		iAction = MEDIACTRL_ACTION_ADD;
		m_iMediaSelections = 0;
	}

	ASSERT(pMedia);
	ASSERT(pSelection);
	ASSERT(pAnchor);
	if(pMedia == 0 || pSelection == 0 || pAnchor == 0)
		return;

	//	Set the selection state of each page
	Pos = pMedia->m_Pages.GetHeadPosition();
	while(Pos && bContinue)
	{
		//	Get the page object
		if((pCurrent = (CXmlPage*)pMedia->m_Pages.GetNext(Pos)) == 0)
			continue;

		//	Is this the anchor?
		if(pCurrent == pAnchor)
		{
			if(bQueue)
				m_iQueueSelections++;
			else
				m_iMediaSelections++;

			//	Set the action identifier
			SetAction(pCurrent, iAction, pTreeCtrl, bTreatments);

			//	Is this also the current selection?
			if(pCurrent == pSelection)
				bSelect = FALSE;
			else
				bSelect = TRUE;

			//	Break out of the loop
			bContinue = FALSE;
		}
			
		//	Is this the current selection?
		else if(pCurrent == pSelection)
		{
			bSelect = TRUE;

			if(bQueue)
				m_iQueueSelections++;
			else
				m_iMediaSelections++;

			//	Set the action identifier
			SetAction(pCurrent, iAction, pTreeCtrl, bTreatments);

			//	Break out of the loop
			bContinue = FALSE;
		}
			
		//	We haven't reached one of the extents yet
		else
		{
			//	No action to be taken on this object
			SetAction(pCurrent, MEDIACTRL_ACTION_NONE, pTreeCtrl, TRUE);
		}
	}

	//	Now set the remaining objects
	while(Pos)
	{
		//	Get the treatment object
		if((pCurrent = (CXmlPage*)pMedia->m_Pages.GetNext(Pos)) == 0)
			continue;

		//	Is this one of the extents?
		if((pCurrent == pAnchor) || (pCurrent == pSelection))
		{
			SetAction(pCurrent, iAction, pTreeCtrl, bTreatments);
			if(bQueue)
				m_iQueueSelections++;
			else
				m_iMediaSelections++;
			bSelect = FALSE;
		}
		else
		{
			if(bSelect) 
			{
				if(bQueue)
					m_iQueueSelections++;
				else
					m_iMediaSelections++;
				SetAction(pCurrent, iAction, pTreeCtrl, bTreatments);
			}
			else
			{
				//	This object is no longer selected
				SetAction(pCurrent, MEDIACTRL_ACTION_NONE, pTreeCtrl, TRUE);
			}
		}
	}
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::SelectQueueGroup()
//
// 	Description:	This function is called to select a range of objects in the
//					print queue tree.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::SelectQueueGroup() 
{
	//	Are we anchored to a treatment?
	if((m_QueueAnchors.pTreatment != 0) && (m_pXmlQueuePage != 0))
	{
		//	Clear all selections if a different page object has been selected
		//	or if a treatment has not been selected
		if((m_pXmlQueueTreatment == 0) || (m_pXmlQueuePage != m_QueueAnchors.pPage))
		{
			//	Clear the existing selections
			SetAnchors(TRUE, FALSE, TRUE);
		}
		else
		{
			//	Select a range of treatments
			SelectTreatments(TRUE);
		}
	}

	//	Are we anchored to a page?
	else if((m_QueueAnchors.pPage != 0) && (m_pXmlQueueMedia != 0))
	{
		//	Clear all selections if a different media object has been selected
		//	or if a page has not been selected
		if((m_pXmlQueuePage == 0) || (m_pXmlQueueMedia != m_QueueAnchors.pMedia))
		{
			//	Clear the existing selections
			SetAnchors(TRUE, FALSE, TRUE);
		}
		else
		{
			//	Select a range of pages
			SelectPages(TRUE, TRUE);
		}
	}

	//	Are we anchored to a media object?
	else if((m_QueueAnchors.pMedia != 0) && (m_pXmlQueueTree != 0))
	{
		//	Clear all selections if a different tree object has been selected
		//	or if a media has not been selected
		if((m_pXmlQueueMedia == 0) || (m_pXmlQueueTree != m_QueueAnchors.pTree))
		{
			//	Clear the existing selections
			SetAnchors(TRUE, FALSE, TRUE);
		}
		else
		{
			//	Select a range of media objects
			SelectMedia(TRUE, TRUE);
		}
	}

	//	Are we anchored to a media tree?
	else if(m_QueueAnchors.pTree != 0)
	{
		//	Clear all selections if a media tree has not been selected
		if(m_pXmlQueueTree == 0)
		{
			//	Clear the existing selections
			SetAnchors(FALSE, FALSE, TRUE);
		}
		else
		{
			//	Select a range of media trees
			SelectTrees(TRUE, TRUE);
		}
	}

	else
	{
		//	Clear the existing media selections
		SetAnchors(TRUE, FALSE, TRUE);
	}
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::SelectTreatments()
//
// 	Description:	This function is called to select a range of treatments
//					from within one of the two tree controls.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::SelectTreatments(BOOL bQueue) 
{
	CXmlPage*		pPage;
	CXmlTreatment*	pAnchor;
	CXmlTreatment*	pSelection;
	CXmlTreatment*	pCurrent;
	CMediaCtrl*		pTreeCtrl;
	int				iAction;
	POSITION		Pos;
	BOOL			bSelect = FALSE;
	BOOL			bContinue = TRUE;

	//	Are we selecting within the print queue tree?
	if(bQueue)
	{
		pPage = m_pXmlQueuePage;
		pSelection = m_pXmlQueueTreatment;
		pAnchor = m_QueueAnchors.pTreatment;
		pTreeCtrl = &m_ctrlQueue;
		iAction = MEDIACTRL_ACTION_REMOVE;
	}
	else
	{
		pPage = m_pXmlPage;
		pSelection = m_pXmlTreatment;
		pAnchor = m_MediaAnchors.pTreatment;
		pTreeCtrl = &m_ctrlTrees;
		iAction = MEDIACTRL_ACTION_ADD;
	}

	ASSERT(pPage);
	ASSERT(pSelection);
	ASSERT(pAnchor);
	if(pPage == 0 || pSelection == 0 || pAnchor == 0)
		return;

	//	Clear the selection counter
	m_iQueueSelections = 0;

	//	Set the selection state of each media file object
	Pos = pPage->m_Treatments.GetHeadPosition();
	while(Pos && bContinue)
	{
		//	Get the treatment object
		if((pCurrent = (CXmlTreatment*)pPage->m_Treatments.GetNext(Pos)) == 0)
			continue;

		//	Is this the anchor?
		if(pCurrent == pAnchor)
		{
			m_iQueueSelections++;

			//	Set the action identifier
			SetAction(pCurrent, iAction, pTreeCtrl);

			//	Is this also the current selection?
			if(pCurrent == pSelection)
				bSelect = FALSE;
			else
				bSelect = TRUE;

			//	Break out of the loop
			bContinue = FALSE;
		}
			
		//	Is this the current selection?
		else if(pCurrent == pSelection)
		{
			bSelect = TRUE;
			m_iQueueSelections++;

			//	Set the action identifier
			SetAction(pCurrent, iAction, pTreeCtrl);

			//	Break out of the loop
			bContinue = FALSE;
		}
			
		//	We haven't reached one of the extents yet
		else
		{
			//	No action to be taken on this object
			SetAction(pCurrent, MEDIACTRL_ACTION_NONE, pTreeCtrl);
		}
	}

	//	Now set the remaining objects
	while(Pos)
	{
		//	Get the treatment object
		if((pCurrent = (CXmlTreatment*)pPage->m_Treatments.GetNext(Pos)) == 0)
			continue;

		//	Is this one of the extents?
		if((pCurrent == pAnchor) || (pCurrent == pSelection))
		{
			SetAction(pCurrent, iAction, pTreeCtrl);
			m_iQueueSelections++;
			bSelect = FALSE;
		}
		else
		{
			if(bSelect) 
			{
				m_iQueueSelections++;
				SetAction(pCurrent, iAction, pTreeCtrl);
			}
			else
			{
				//	This object is no longer selected
				SetAction(pCurrent, MEDIACTRL_ACTION_NONE, pTreeCtrl);
			}
		}
	}
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::SetAction()
//
// 	Description:	This function is called to set the action of the specified
//					treatment.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::SetAction(CXmlTreatment* pTreatment, int iAction, 
						    CMediaCtrl* pTreeCtrl) 
{
	ASSERT(pTreatment);

	pTreatment->m_iAction = iAction;
	
	if(pTreeCtrl)
		pTreeCtrl->Redraw(pTreatment);
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::SetQueueDragPos()
//
// 	Description:	This function is called to set the position of the drag
//					cursor when the user is dragging out of the print queue.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::SetQueueDragPos() 
{
	CPoint	Pos;
	RECT	rcTrees;

	//	Get the position of the cursor in screen coordinates
	GetCursorPos(&Pos);
	
	//	Get the position of the media tree control in screen coordinates
	m_ctrlTrees.GetWindowRect(&rcTrees);	
	
	//	Is the cursor in the media tree?
	if(PtInRect(&rcTrees, Pos))
	{
		SetCursor(m_hDrag);
	}
	else
	{
		SetCursor(m_hNoDrop);
	}
}

//==============================================================================
//
// 	Function Name:	CSelectPrint::SetTreeDragPos()
//
// 	Description:	This function is called to set the position of the drag
//					cursor when the user is dragging out of the media tree list.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CSelectPrint::SetTreeDragPos() 
{
	CPoint	Pos;
	RECT	rcQueue;

	//	Get the position of the cursor in screen coordinates
	GetCursorPos(&Pos);
	
	//	Get the position of the media tree control in screen coordinates
	m_ctrlQueue.GetWindowRect(&rcQueue);	
	
	//	Is the cursor in the media tree?
	if(PtInRect(&rcQueue, Pos))
	{
		SetCursor(m_hDrag);
	}
	else
	{
		SetCursor(m_hNoDrop);
	}
}


