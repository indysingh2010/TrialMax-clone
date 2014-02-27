//==============================================================================
//
// File Name:	callout.cpp
//
// Description:	This file contains member functions of the CCallout and
//				CCallouts classes
//
// See Also:	callout.h
//
//==============================================================================
//	Date		Revision    Description
//	03-22-98	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmviewap.h>
#include <callout.h>
#include <tmview.h>

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
extern CTMViewCtrl*	_pControl;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CCallout, CDialog)
	//{{AFX_MSG_MAP(CCallout)
	ON_WM_PALETTECHANGED()
	ON_WM_QUERYNEWPALETTE()
	ON_WM_CREATE()
	ON_WM_LBUTTONDOWN()
	ON_WM_RBUTTONDOWN()
	ON_WM_CTLCOLOR()
	ON_WM_PAINT()
	ON_WM_PARENTNOTIFY()
	ON_WM_SETCURSOR()
	ON_WM_LBUTTONUP()
	ON_WM_MOUSEMOVE()
	ON_WM_RBUTTONUP()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

BEGIN_EVENTSINK_MAP(CCallout, CDialog)
    //{{AFX_EVENTSINK_MAP(CTMViewCtrl)
	ON_EVENT(CCallout, IDC_TMLEAD, 12 /* AnnChange */, OnEvAnnChange, VTS_I4 VTS_I4)
	ON_EVENT(CCallout, IDC_TMLEAD, 7 /* AnnCreate */, OnEvAnnCreate, VTS_I4)
	ON_EVENT(CCallout, IDC_TMLEAD, 8 /* AnnDestroy */, OnEvAnnDestroy, VTS_I4)
	ON_EVENT(CCallout, IDC_TMLEAD, 10 /* AnnDrawn */, OnEvAnnDrawn, VTS_I4)
	ON_EVENT(CCallout, IDC_TMLEAD, 28 /* AnnMouseDown */, OnEvAnnMouseDown, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
	ON_EVENT(CCallout, IDC_TMLEAD, 16 /* AnnSelect */, OnEvAnnSelect, VTS_VARIANT VTS_I2)
	ON_EVENT(CCallout, IDC_TMLEAD, DISPID_CLICK/* Click */, OnEvMouseClick, VTS_NONE)
	ON_EVENT(CCallout, IDC_TMLEAD, DISPID_DBLCLICK /* DblClick */, OnEvMouseDblClick, VTS_NONE)
	ON_EVENT(CCallout, IDC_TMLEAD, DISPID_MOUSEDOWN /* MouseDown */, OnEvMouseDown, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
	ON_EVENT(CCallout, IDC_TMLEAD, DISPID_MOUSEMOVE /* MouseMove */, OnEvMouseMove, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
	ON_EVENT(CCallout, IDC_TMLEAD, DISPID_MOUSEUP /* MouseUp */, OnEvMouseUp, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
	ON_EVENT(CCallout, IDC_TMLEAD, 15/* RubberBand */, OnEvRubberBand, VTS_NONE)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()

//==============================================================================
//
// 	Function Name:	CCallout::CCallout()
//
// 	Description:	This is the constructor for CCallout objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CCallout::CCallout(CTMViewCtrl* pControl, CTMLead* pSource) 
		 :CDialog(CCallout::IDD, pSource)
{
	//{{AFX_DATA_INIT(CCallout)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT

	ASSERT(pControl);
	ASSERT(pSource);

	m_pControl      = pControl;
	m_pSource       = pSource;
	m_bDrag         = FALSE;
	m_bParentNotify = FALSE;
	m_bEventNotify  = TRUE;
	m_bSetCursor    = FALSE;
	m_iHeight       = 0;
	m_iWidth        = 0;
	m_iMouseButton	= NO_MOUSEBUTTON;
	m_lDragX        = 0;
	m_lDragY        = 0;
	m_wAnnId        = 0;
	m_sAction       = NONE;
	m_fScaleFactor  = 1.0f;
	m_bAnnotateCallouts	= DEFAULT_ANNOTATECALLOUTS;
	m_bPanCallouts = DEFAULT_PANCALLOUTS;
	m_bZoomCallouts = DEFAULT_ZOOMCALLOUTS;
	m_bShaded = FALSE;
	m_bResizeable = m_pSource->GetResizeCallouts();
	memset(&m_rcMax, 0, sizeof(m_rcMax));
	memset(&m_rcDst, 0, sizeof(m_rcDst));
	memset(&m_rcRubberBand, 0, sizeof(m_rcRubberBand));
	memset(&m_rcOriginalContainer, 0, sizeof(m_rcOriginalContainer));
	memset(&m_rcOriginalPosition, 0, sizeof(m_rcOriginalPosition));
	memset(&m_rcContainer, 0, sizeof(m_rcContainer));

	//	Create the background brush
	m_pBackground = new CBrush();
	if(m_pSource)
		m_pBackground->CreateSolidBrush(m_pSource->GetCallFrameColor());
	else
		m_pBackground->CreateSolidBrush(RGB(0,0,0));

	//	Set the member to indicate that the TMLead object is owned by a callout
	m_TMLead.m_pOwner = this;

	//	Create the dialog 
	Create(CCallout::IDD, pSource);
}

//==============================================================================
//
// 	Function Name:	CCallout::~CCallout()
//
// 	Description:	This is the destructor for CCallout objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CCallout::~CCallout()
{
	if(IsWindow(m_TMLead.m_hWnd))
		m_TMLead.DestroyWindow();
}

//==============================================================================
//
// 	Function Name:	CCallout::ClearSelections()
//
// 	Description:	This function will clear all annotation selections in the
//					callout
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CCallout::ClearSelections() 
{
	if(IsWindow(m_TMLead.m_hWnd))
		return m_TMLead.ClearSelections();
	else
		return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CCallout::CopyAnn()
//
// 	Description:	This function will place a copy of the annotation object
//					provided by the source image in the callout's container
//
// 	Returns:		A handle to the copy if successful
//
//	Notes:			None
//
//==============================================================================
HANNOBJECT CCallout::CopyAnn(CTMLead* pSource, HANNOBJECT hAnn) 
{
	if(IsWindow(m_TMLead.m_hWnd))
		return m_TMLead.CopyAnn(pSource, hAnn);
	else
		return 0;
}

//==============================================================================
//
// 	Function Name:	CCallout::DeleteAnn()
//
// 	Description:	This function will delete the annotation object specified
//					by the caller
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::DeleteAnn(HANNOBJECT hAnn) 
{
	if(IsWindow(m_TMLead.m_hWnd))
	{
		//	Inhibit handling of the OnAnnDestroy event
		m_bEventNotify = FALSE;

		m_TMLead.DeleteAnn(hAnn, TRUE);

		m_bEventNotify = TRUE;
	}
}

//==============================================================================
//
// 	Function Name:	CCallout::DeleteAnn()
//
// 	Description:	This function will delete the annotation object specified
//					by the tag provided by the caller
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::DeleteAnn(DWORD dwTag) 
{
	if(IsWindow(m_TMLead.m_hWnd))
	{
		//	Inhibit handling of the OnAnnDestroy event
		m_bEventNotify = FALSE;

		m_TMLead.DeleteAnn(dwTag);

		m_bEventNotify = TRUE;
	}
}

//==============================================================================
//
// 	Function Name:	CCallout::DeleteSelections()
//
// 	Description:	This function will delete all annotations selected in the
//					callout
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CCallout::DeleteSelections() 
{
	short sReturn = TMV_NOERROR;

	if(IsWindow(m_TMLead.m_hWnd))
	{
		//	Inhibit handling of the OnAnnDestroy event
		m_bEventNotify = FALSE;

		sReturn = m_TMLead.DeleteSelections();

		m_bEventNotify = TRUE;
	}

	return sReturn;
}

//==============================================================================
//
// 	Function Name:	CCallout::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the
//					dialog box and the class members
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CCallout)
		// NOTE: the ClassWizard will add DDX and DDV calls here
	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CCallout::DrawDragRect()
//
// 	Description:	This function will draw the drag rectangle.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::DrawDragRect() 
{
	HDC		hDC     = ::GetDC(0);
	HBRUSH	hbrNull = (HBRUSH)GetStockObject(NULL_BRUSH);
	HBRUSH	hbrOld;
	int		iOldROP;

	hbrOld = (HBRUSH)SelectObject(hDC, hbrNull);
	iOldROP = ::SetROP2(hDC, R2_NOT);
		
	Rectangle(hDC, m_rcDrag.left, m_rcDrag.top, m_rcDrag.right, m_rcDrag.bottom);

	SelectObject(hDC, hbrOld);
	SetROP2(hDC, iOldROP);

	::ReleaseDC(0, hDC);

}

//==============================================================================
//
// 	Function Name:	CCallout::EndDrag()
//
// 	Description:	This function is called to terminate a drag operation.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::EndDrag() 
{
	RECT	rcWnd;
	BOOL	bNotify;

	//	Clear the drag rectangle
	DrawDragRect();

	//	Move the window
	GetWindowRect(&rcWnd);
	MoveWindow(&m_rcDrag, TRUE);
	m_pControl->RedrawWindow();

	//	Do we need to notify the owner?
	if(abs(rcWnd.top - m_rcDrag.top) > 1)
		bNotify = TRUE;
	else if(abs(rcWnd.left - m_rcDrag.left) > 1)
		bNotify = TRUE;
	else
		bNotify = FALSE;

	m_bDrag = FALSE;
	m_lDragX = 0;
	m_lDragY = 0;
	memset(&m_rcDrag, 0, sizeof(m_rcDrag));
	::ReleaseCapture();

	//	Notify the source
	if((m_pSource != 0) && (bNotify == TRUE))
		m_pSource->OnCalloutMoved(this);
}

//==============================================================================
//
// 	Function Name:	CCallout::GetAnnFromTag()
//
// 	Description:	This function is called to get the annotation object with 
//					the tag specified by the caller
//
// 	Returns:		A pointer to the annotation object if found
//
//	Notes:			None
//
//==============================================================================
CAnnotation* CCallout::GetAnnFromTag(DWORD dwTag) 
{
	if(IsWindow(m_TMLead.m_hWnd))
		return m_TMLead.GetAnnFromTag(dwTag);
	else
		return 0;
}

//==============================================================================
//
// 	Function Name:	CCallout::GetAnnId()
//
// 	Description:	This function is called to get the identifier of the 
//					callout's highlight in the source image
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
WORD CCallout::GetAnnId() 
{
	return m_wAnnId;
}

//==============================================================================
//
// 	Function Name:	CCallout::GetFrameThickness()
//
// 	Description:	This function is called to get the thickness in pixels
//					of the frame around the callout
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
short CCallout::GetFrameThickness() 
{
	//	Use the callout frame thickness defined for the source pane
	if(m_pSource)
		return m_pSource->m_sCallFrameThick;
	else
		return 0;
}

//==============================================================================
//
// 	Function Name:	CCallout::GetHandleFromTag()
//
// 	Description:	This function will retrieve the handle of an annotation 
//					in the callout using the specified tag
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
HANNOBJECT CCallout::GetHandleFromTag(DWORD dwTag) 
{
	if(IsWindow(m_TMLead.m_hWnd))
		return m_TMLead.GetHandleFromTag(dwTag);
	else
		return 0;
}

//==============================================================================
//
// 	Function Name:	CCallout::GetScratchCopy()
//
// 	Description:	This function is called to copy the current image to the
//					scratch pane
//
// 	Returns:		A pointer to the scratch pane if successful
//
//	Notes:			None
//
//==============================================================================
CTMLead* CCallout::GetScratchCopy() 
{
	CAnnotation*	pAnn;
	HANNOBJECT		hAnn;
	CTMLead*		pScratch = 0;
	HGLOBAL			hAnnMem  = 0;
	long			lAnnSize = 0;
	RECT			rcRect;
	RECT			rcClip;

	//	Do we have a bitmap to work with?	
	ASSERT(m_TMLead.GetBitmap() != 0);
	if(m_TMLead.GetBitmap() == 0)
		return 0;

	//	Get the scratch pane from the main control
	if(m_pControl)
		pScratch = m_pControl->GetScratchPane();
	if(pScratch == 0)
		return 0;

	//	Clear out the existing bitmap and annotations
	pScratch->Erase(TRUE);
	pScratch->UnloadImage();

	//	Copy the bitmap into the scratch pane 
	pScratch->SetBitmap(m_TMLead.GetBitmap());

	//	Set the source rectangles
	m_TMLead.GetSrcRects(&rcRect, &rcClip);
	pScratch->SetSrcRects(&rcRect, &rcClip);

	//	Set the destination rectangles
	m_TMLead.GetDstRects(&rcRect, &rcClip);
	pScratch->SetDstRects(&rcRect, &rcClip);

	//	Copy all of the local annotations to the callout's container
	CAnnotations& rAnnotations = m_pSource->GetAnnotations();
	pAnn = rAnnotations.First();
	while(pAnn)
	{
		//	Don't copy the annotation if it's a callout highlight
		if(!pAnn->m_bIsCallout)
		{
			//	Get the handle of the annotation object
			if((hAnn = GetHandleFromTag(pAnn->GetAnnTag())) != 0)
			{
				//	Create a copy of the annotation in the callout and then link
				//	the callout to the source annotation
				pScratch->CopyAnn(m_pSource, hAnn);
			}
		}

		pAnn = rAnnotations.Next();
	}

	//	Realize the annotations in the scratch pane
	pScratch->Realize(TRUE);

	return pScratch;
}

//==============================================================================
//
// 	Function Name:	CCallout::GetShaded()
//
// 	Description:	This function is called to determine if the highlight for
//					this callout is shaded
//
// 	Returns:		TRUE if highlight is shaded
//
//	Notes:			None
//
//==============================================================================
BOOL CCallout::GetShaded() 
{
	return m_bShaded;
}

//==============================================================================
//
// 	Function Name:	CCallout::GetTagFromHandle()
//
// 	Description:	This function will retrieve the tag of an annotation 
//					in the callout using the specified handle
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
DWORD CCallout::GetTagFromHandle(HANNOBJECT hAnn) 
{
	if(IsWindow(m_TMLead.m_hWnd))
		return m_TMLead.GetTagFromHandle(hAnn);
	else
		return 0;
}

//==============================================================================
//
// 	Function Name:	CCallout::GetTMLead()
//
// 	Description:	This function is called to retrieve the CTMLead object 
//					associated with the callout.
//
// 	Returns:		A pointer to the callout's CTMLead object
//
//	Notes:			None
//
//==============================================================================
CTMLead* CCallout::GetTMLead() 
{
	return &m_TMLead;
}

//==============================================================================
//
// 	Function Name:	CCallout::OnCreate()
//
// 	Description:	This function is called when the dialog box is created
//
// 	Returns:		0 if successful
//
//	Notes:			None
//
//==============================================================================
int CCallout::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
	if(CDialog::OnCreate(lpCreateStruct) == -1)
		return -1;
	
	if(!m_TMLead.Create(this))
		return -1;

	//	Set the pointer to the control window
	m_TMLead.SetControl(m_pControl, m_pSource->GetHandler());

	//	Copy the properties of the source control
	m_pSource->Copy(&m_TMLead);

	//	Override the source properties with callout defaults
	m_TMLead.SetEnabled(TRUE);
	m_TMLead.SetScaleImage(TRUE);
	m_TMLead.SetHideScrollBars(TRUE);
	m_TMLead.SetFitToImage(FALSE);
	m_TMLead.SetZoomOnLoad(ZOOMED_NONE);
	m_TMLead.SetZoomToRect(TRUE);
	m_TMLead.SetRotation(0);
	
	//	What annotations permitted?
	m_bPanCallouts = m_TMLead.GetPanCallouts();
	m_bZoomCallouts = m_TMLead.GetZoomCallouts();
	m_bAnnotateCallouts = m_TMLead.m_bAnnotateCallouts;
	if((m_TMLead.GetAction() == CALLOUT) || (m_bAnnotateCallouts == FALSE))
		m_TMLead.SetAction(NONE);
	else if((m_TMLead.GetAction() == PAN) && (m_bPanCallouts == FALSE))
		m_TMLead.SetAction(NONE);
	else if((m_TMLead.GetAction() == ZOOM) && (m_bZoomCallouts == FALSE))
		m_TMLead.SetAction(NONE);
	
	//	Attach and initialize the tracker
	m_Tracker.Attach(m_hWnd);
	m_Tracker.SetHandleColor(m_pSource->GetCallHandleColor());
	
	return 0;
}

//==============================================================================
//
// 	Function Name:	CTMView::OnCtlColor()
//
// 	Description:	This function is overloaded to set the background of the
//					dialog to black.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
HBRUSH CCallout::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor) 
{
	//	Is this a request for the dialog brush?
	if(nCtlColor == CTLCOLOR_DLG && m_pBackground)
		return (HBRUSH)(*m_pBackground);
	else
		return CDialog::OnCtlColor(pDC, pWnd, nCtlColor);
}

//==============================================================================
//
// 	Function Name:	CCallout::OnEvAnnChange()
//
// 	Description:	This function handles the event notification sent from the
//					lead control when one of it's annotations changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::OnEvAnnChange(long hObject, long uType) 
{
	if(IsWindow(m_TMLead.m_hWnd) && m_TMLead.GetAction() != CALLOUT)
		m_TMLead.OnAnnChange(hObject, uType);

	//	Notify the source control
	if(m_pSource)
	{
		m_pSource->OnActivateCallout(this, TRUE);
		m_pSource->ChangeAnn(this, (HANNOBJECT)hObject);
	}
}

//==============================================================================
//
// 	Function Name:	CCallout::OnEvAnnCreate()
//
// 	Description:	This function handles the event notification sent from the
//					lead control. It sets the properties of the annotation.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::OnEvAnnCreate(long hObject) 
{
	if(IsWindow(m_TMLead.m_hWnd) && m_TMLead.GetAction() != CALLOUT)
		m_TMLead.OnAnnCreate(hObject);
}

//==============================================================================
//
// 	Function Name:	CCallout::OnEvAnnDestroy()
//
// 	Description:	This function handles the event notification sent from the
//					lead control. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::OnEvAnnDestroy(long hObject) 
{
	L_BOOL	bSelected;

	//	Make sure this annotation is one that is in our list
	//
	//	NOTE:	Starting with Lead Tools 12 they delete the annotation container
	//			if you delete the last annotation in the container. We do not
	//			want to process events that do not relate to annotations that
	//			were not created by the user.
	if(m_TMLead.GetAnnFromHandle((void*)hObject) == 0) return;

	//	Is this object selected?
	L_AnnGetSelected((void*)hObject, &bSelected);

	//	Notify the source control if the object has been selected and event
	//	notifications are enabled
	if(m_pSource && m_bEventNotify && bSelected)
	{
		m_pSource->DeleteAnn(this, (HANNOBJECT)hObject);
	}

}

//==============================================================================
//
// 	Function Name:	CCallout::OnEvAnnDrawn()
//
// 	Description:	This function handles the event notification sent from the
//					lead control. It sets the properties of the annotation.
//
// 	Returns:		None
//
//	Notes:			This function will prevent attempts to do recursive 
//					callouts
//
//==============================================================================
void CCallout::OnEvAnnDrawn(long hObject) 
{
	if(IsWindow(m_TMLead.m_hWnd) && m_TMLead.GetAction() != CALLOUT)
		m_TMLead.OnAnnDrawn(hObject);

	//	Notify the source control
	if(m_pSource)
	{
		m_pSource->OnActivateCallout(this, TRUE);
		m_pSource->CopyAnn(this, (HANNOBJECT)hObject);
	}
}

//==============================================================================
//
// 	Function Name:	CCallout::OnAnnMouseDown()
//
// 	Description:	This function traps the AnnMouseDown event
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::OnEvAnnMouseDown(short Button, short Shift, long X, long Y) 
{
	if(IsWindow(m_TMLead.m_hWnd))
		m_TMLead.OnAnnMouseDown(Button, Shift, X, Y);

}

//==============================================================================
//
// 	Function Name:	CCallout::OnEvAnnSelect()
//
// 	Description:	This function handles the event notification sent from the
//					PaneA control when the user selects a new annotation 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::OnEvAnnSelect(const VARIANT FAR& aObjects, short uCount) 
{
	//	Notify the source
	if(m_pSource)
	{
		m_pSource->OnActivateCallout(this, TRUE);
		m_pSource->OnCalloutSelection(this);
	}
}

//==============================================================================
//
// 	Function Name:	CCallout::OnEvMouseClick()
//
// 	Description:	This function will respond to a click in the Lead Tools
//					window.
//
// 	Returns:		None
//
//==============================================================================
void CCallout::OnEvMouseClick() 
{
}

//==============================================================================
//
// 	Function Name:	CCallout::OnEvMouseDblClick()
//
// 	Description:	This function traps the MouseDblClick event
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::OnEvMouseDblClick() 
{
	if(IsWindow(m_TMLead.m_hWnd))
		m_TMLead.OnMouseDblClick();

}

//==============================================================================
//
// 	Function Name:	CCallout::OnEvMouseDown()
//
// 	Description:	This function will notify the TMLead object that the user
//					has clicked in the window.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::OnEvMouseDown(short Button, short Shift, long x, long y) 
{
	//	Notify the control window
	if(m_pSource)
		m_pSource->OnActivateCallout(this, TRUE);

	//	Store the button identifier
	//
	//	NOTE:	We do this because of a LeadTools bug where it reports the wrong
	//			Button value in MouseMove events under some conditions
	m_iMouseButton = Button;

	//	Are we about to drag?
	if(m_iMouseButton == RIGHT_BUTTON)
	{
		//	Are we allowed to pan callouts?
		if(m_bPanCallouts && m_TMLead.m_bRightClickPan)
			m_TMLead.OnMouseDown(m_iMouseButton, Shift, x, y);
		
		//
		//	REMOVED THE ABILITY TO RIGHT CLICK DRAG IN VERSION 5.0
		/*
		if(m_bAnnotateCallouts)
		{
			//	Use the right mouse button for dragging
			StartDrag(x, y);
		}
		*/
	}
	else
	{
		//	What is the current action?
		switch(m_TMLead.GetAction())
		{
			case NONE:
			case CALLOUT:

				//	Set up a drag operation
				StartDrag(x, y);
				break;

			case ZOOM:

				if((m_bDrag == FALSE) && (m_bZoomCallouts == TRUE))
					m_TMLead.OnMouseDown(m_iMouseButton, Shift, x, y);
				break;

			case PAN:

				if((m_bDrag == FALSE) && (m_bPanCallouts == TRUE))
					m_TMLead.OnMouseDown(m_iMouseButton, Shift, x, y);
				break;

			default:

				m_bDrag = FALSE;
				m_lDragX = 0;
				m_lDragY = 0;
				memset(&m_rcDrag, 0, sizeof(m_rcDrag));
				break;
		}
	}
}

//==============================================================================
//
// 	Function Name:	CCallout::OnEvMouseMove()
//
// 	Description:	This function will keep track of mouse movements and drag
//					the window if neccessary
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::OnEvMouseMove(short Button, short Shift, long x, long y) 
{
   	if(m_bPanCallouts && (m_TMLead.GetAction() == PAN))
	{
		m_TMLead.OnMouseMove(m_iMouseButton, Shift, x, y);
	}
	else if(m_bZoomCallouts && (m_TMLead.GetAction() == ZOOM) && (Button == LEFT_BUTTON))
	{
		m_TMLead.OnMouseMove(m_iMouseButton, Shift, x, y);
	}
}

//==============================================================================
//
// 	Function Name:	CCallout::OnEvMouseUp()
//
// 	Description:	This function will keep move the window at the end of a
//					drag operation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::OnEvMouseUp(short Button, short Shift, long x, long y) 
{
	//	Are we dragging?
	if(m_bDrag)
	{
		//	End the drag operation
		EndDrag();
	}
	else
	{
		//	Is this the right button?
		if(m_iMouseButton == RIGHT_BUTTON)
		{
			//	Notify the source control
			m_TMLead.OnClickCallout(this, m_iMouseButton, Shift);
	
			if(m_bPanCallouts && (m_TMLead.GetAction() == PAN))
			{
				m_TMLead.OnMouseUp(m_iMouseButton, Shift, x, y);
			}
		}
		else
		{
			//	What is the current action?
			switch(m_TMLead.GetAction())
			{
				case NONE:
				case CALLOUT:

					//	Notify the source control
					m_TMLead.OnClickCallout(this, m_iMouseButton, Shift);
					break;

				case ZOOM:

					if(m_bZoomCallouts == TRUE)
						m_TMLead.OnMouseUp(m_iMouseButton, Shift, x, y);
					break;

				case PAN:

					if(m_bPanCallouts == TRUE)
						m_TMLead.OnMouseUp(m_iMouseButton, Shift, x, y);
					break;

				default:

					break;
			}
		}
	}

	m_iMouseButton = NO_MOUSEBUTTON;
}

//==============================================================================
//
// 	Function Name:	CCallout::OnLButtonDown()
//
// 	Description:	This function is called when the user clicks in the dialog
//					with the left mouse button.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::OnLButtonDown(UINT nFlags, CPoint point) 
{
	m_iMouseButton = LEFT_MOUSEBUTTON;

	//	Is the user resizing the callout?
	if(!Resize())
	{
		//	Perform the base class processing
		CDialog::OnLButtonDown(nFlags, point);
	}
}

//==============================================================================
//
// 	Function Name:	CCallout::OnLButtonUp()
//
// 	Description:	This function will trap all WM_LBUTTONUP messages.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::OnLButtonUp(UINT nFlags, CPoint point) 
{
	m_iMouseButton = NO_MOUSEBUTTON;

	//	Should we stop dragging?
	if(m_bDrag)
		EndDrag();

	//	Do the base class processing
	CDialog::OnLButtonUp(nFlags, point);
}

//==============================================================================
//
// 	Function Name:	CCallout::OnModified()
//
// 	Description:	This function is called after the user modifies the viewport
//					by zooming or panning
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::OnModified() 
{
	RECT rcSource;

	//	Update the rectangles used to resize the callout
	m_TMLead.GetDstRects(&m_rcDst, 0);
	GetClientRect(&m_rcRubberBand);	//	Make it look like the whole window is selected

	//	Notify the host control
	memcpy(&rcSource, &m_rcRubberBand, sizeof(rcSource));
	m_TMLead.ClientToSource(&rcSource);
	m_pSource->OnCalloutModified(this, &rcSource);
}

//==============================================================================
//
// 	Function Name:	CCallout::OnMouseMove()
//
// 	Description:	This function will trap all WM_MOUSEMOVE messages.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::OnMouseMove(UINT nFlags, CPoint point) 
{
	//	Are we dragging the window?
	if(m_bDrag)
	{
		//	Erase the current drag rectangle
		DrawDragRect();

		//	Set the new draw position
		m_rcDrag.left += (point.x - m_lDragX);
		m_rcDrag.right += (point.x - m_lDragX);
		m_rcDrag.top += (point.y - m_lDragY);
		m_rcDrag.bottom += (point.y - m_lDragY);
		m_lDragX = point.x;
		m_lDragY = point.y;
		
		//	Draw the new drag rectangle
		DrawDragRect();
	}

	CDialog::OnMouseMove(nFlags, point);
}

//==============================================================================
//
// 	Function Name:	CCallout::OnPaint()
//
// 	Description:	This function handles WM_PAINT messages sent to the callout
//					dialog
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::OnPaint() 
{
	//	Perform the base class processing. CDialog::OnPaint() should NOT be
	//	called
	CWnd::OnPaint();
	
	//	Update the TMLead window
	if(IsWindow(m_TMLead.m_hWnd))
		m_TMLead.ForceRepaint();

	//	Make sure the tracker is drawn in the correct position
	if(m_bResizeable)
		m_Tracker.Draw();
}

//==============================================================================
//
// 	Function Name:	CCallout::OnPaletteChanged()
//
// 	Description:	This function forwards the WM_PALETTECHANGED message 
//					to the lead control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::OnPaletteChanged(CWnd* pFocusWnd) 
{
	CDialog::OnPaletteChanged(pFocusWnd);
	m_TMLead.SendMessage(WM_PALETTECHANGED, (WPARAM)pFocusWnd->m_hWnd);
}

//==============================================================================
//
// 	Function Name:	CCallout::OnPanComplete()
//
// 	Description:	This function is called by the TMLead object when the user
//					pans the image
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::OnPanComplete() 
{
	OnModified();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnParentNotify()
//
// 	Description:	This function handles WM_PARENTNOTIFY messages sent from
//					one of the lead tool controls
//
// 	Returns:		None
//
//	Notes:			This function works with OnSetCursor() to work around a bug
//					in the LeadTools control. See OnSetCursor() for more info.
//
//==============================================================================
void CCallout::OnParentNotify(UINT message, LPARAM lParam) 
{

	//	Perform the base class processing first
	CDialog::OnParentNotify(message, lParam);
	
	//	What notification is being sent?
	switch(message)
	{
		case WM_LBUTTONDOWN:
		case WM_RBUTTONDOWN:

			//	Is the user currently selecting annotations?
			if(m_TMLead.m_sAction == SELECT)
			{
				m_bParentNotify = TRUE;
				m_bSetCursor    = FALSE;
			}
			break;
		
		default:
			
			break;
	}
	
}

//==============================================================================
//
// 	Function Name:	CCallout::OnQueryNewPalette()
//
// 	Description:	This function forwards the WM_QUERYNEWPALETTE message 
//					to the lead control.
//
// 	Returns:		FALSE if not successful
//
//	Notes:			None
//
//==============================================================================
BOOL CCallout::OnQueryNewPalette() 
{
	m_TMLead.SendMessage(WM_QUERYNEWPALETTE);
	return CDialog::OnQueryNewPalette();
}

//==============================================================================
//
// 	Function Name:	CCallout::OnRButtonDown()
//
// 	Description:	This function is called when the user clicks in the dialog
//					with the left mouse button.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::OnRButtonDown(UINT nFlags, CPoint point) 
{
	m_iMouseButton = RIGHT_MOUSEBUTTON;

	//	Notify the control window
	if(m_pSource)
		m_pSource->OnActivateCallout(this, TRUE);

	//	Perform the base class processing
	CDialog::OnRButtonDown(nFlags, point);
}

//==============================================================================
//
// 	Function Name:	CCallout::OnRButtonUp()
//
// 	Description:	This function will trap all WM_RBUTTONUP messages.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::OnRButtonUp(UINT nFlags, CPoint point) 
{
	m_iMouseButton = NO_MOUSEBUTTON;

	//	Should we stop dragging?
	if(m_bDrag)
		EndDrag();

	//	Do the base class processing
	CDialog::OnRButtonUp(nFlags, point);
}

//==============================================================================
//
// 	Function Name:	CCallout::OnEvRubberBand()
//
// 	Description:	This function is called when the user completes a rubber
//					banding operation. It will zoom the image to the selection
//					rectangle.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::OnEvRubberBand() 
{
	//	Notify the control window
	if(m_pSource)
		m_pSource->OnActivateCallout(this, TRUE);

	if(IsWindow(m_TMLead.m_hWnd))
		m_TMLead.OnRubberBand();
}

//==============================================================================
//
// 	Function Name:	CCallout::OnSetCursor()
//
// 	Description:	This function handles WM_SETCURSOR messages.
//
// 	Returns:		None
//
//	Notes:			This function works with OnParentNotify() to work around a
//					bug in the LeadTools control. If the user selects an 
//					annotation in SELECT mode the control will fire an AnnSelect
//					event. However, if the user clears all the selections or
//					selects annototations by rubberbanding, the control does
//					not fire the AnnSelect event.
//
//					To make sure we keep the selections synchronized, we trap
//					the WM_PARENTNOTIFY and WM_SETCURSOR messages. When the user
//					clicks in the window, a WM_PARENTNOTIFY message will be 
//					sent followed by a WM_SETCURSOR message. Then, when the user
//					completes the rubber banding, another WM_SETCURSOR message
//					is sent. We wait for that second WM_SETCURSOR to update
//					the selections.
//
//==============================================================================
BOOL CCallout::OnSetCursor(CWnd* pWnd, UINT nHitTest, UINT message) 
{
	//	Do we need to update the annotation selections?
	if(m_bParentNotify)
	{
		m_bParentNotify = FALSE;
		m_bSetCursor    = TRUE;
	}
	else if(m_bSetCursor)
	{
		
		m_bParentNotify = FALSE;
		m_bSetCursor    = FALSE;
		
		//	Notify the source
		if(m_pSource)
			m_pSource->OnCalloutSelection(this);
	}

	//::ShowCursor(true);

	if(m_bResizeable && m_Tracker.SetCursor(this, nHitTest))
		return TRUE;
	else
		return CDialog::OnSetCursor(pWnd, nHitTest, message);
}

//==============================================================================
//
// 	Function Name:	CCallout::OnZoomComplete()
//
// 	Description:	This function is called by the TMLead object when the user
//					zooms the image
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::OnZoomComplete() 
{
	OnModified();
}

//==============================================================================
//
// 	Function Name:	CCallout::Print()
//
// 	Description:	This function is called to print the callout within the
//					rectangle defined by the caller.
//
// 	Returns:		None
//
//	Notes:			This function assumes the rectangle provided by the caller
//					has already been properly sized for the callout.
//
//==============================================================================
void CCallout::Print(CDC* pdc, RECT* pRect, short sRotation)
{
	CTMLead* pScratch = 0;
	RECT	 rcVisible;
	RECT	 rcClient;
	BOOL	 bRendered = FALSE;

	ASSERT(pRect);
	ASSERT(pdc);

	//	Copy the bitmap into the control's scratch pane
	//
	//	NOTE:	Annotations in the callout window get realized when we make
	//			the scratch copy
	if((pScratch = GetScratchCopy()) != 0)
	{
		//	Should we clip the source rectangle to the visible region
		m_TMLead.GetSrcVisible(&rcVisible);

		//	Crop the bitmap to the visible area
		pScratch->Trim((float)rcVisible.left, 
					   (float)rcVisible.top,
					   (float)(rcVisible.right - rcVisible.left),
					   (float)(rcVisible.bottom - rcVisible.top));

		//	Are we supposed to rotate prior to printing?
		if(sRotation < 0)
			pScratch->Rotate((short)-90);
		else if(sRotation > 0)
			pScratch->Rotate((short)90);

		//	LeadTools must optimize it's drawing because not all changes to the scratch
		//	pane get committed immediately because the pane is not visible. This will force
		//	the changes to be committed before we print
		m_pSource->GetClientRect(&rcClient);
		pScratch->MoveWindow(&rcClient, FALSE);

		//	Can we render as a DIB?
		if(pScratch->GetRenderAsDIB(pdc->GetSafeHdc(), pRect) == TRUE)
		{
			//	Attempt to render as a DIB
			//
			//	NOTE:	This is preferred to keep the spool file size small
			if(pScratch->RenderDIB(pdc->GetSafeHdc(), 
								   pRect->left,
								   pRect->top,
								   (pRect->right - pRect->left),
								   (pRect->bottom - pRect->top)) == TRUE)
				bRendered = TRUE;
		}
		
		//	Use the LeadTools control to do the rendering if necessary
		if(bRendered == FALSE)
		{
			pScratch->Render((long)pdc->GetSafeHdc(), 
							(float)pRect->left,
							(float)pRect->top,
							(float)(pRect->right - pRect->left),
							(float)(pRect->bottom - pRect->top));
		}

		//	Put the scratch window back where it belongs
		pScratch->MoveWindow(0,0,1,1, FALSE);
	}

}

//==============================================================================
//
// 	Function Name:	CCallout::Rescale()
//
// 	Description:	This function is called to rescale the window using the
//					reference rectangle and the current container rectangle.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::Rescale() 
{
	RECT	rcMax;
	RECT	rcPosition;
	float	fOriginalWidth;
	float	fOriginalHeight;
	float	fOriginalRatio;
	float	fWidth;
	float	fHeight;
	float	fXOffset;
	float	fYOffset;
	int		iFrame;

	//	Get the dimensions of the reference rectangle
	fOriginalWidth  = (float)(m_rcOriginalContainer.right  - m_rcOriginalContainer.left);
	fOriginalHeight = (float)(m_rcOriginalContainer.bottom - m_rcOriginalContainer.top);

	//	Make sure we have valid dimensions
	if((fOriginalWidth <= 0) || (fOriginalHeight <= 0)) return; 

	//	Get the aspect ratio of the reference rectangle
	fOriginalRatio = fOriginalHeight / fOriginalWidth;

	//	Adjust the container rectangle to have the same aspect ratio
	//	as the reference rectangle
	//
	//	NOTE:	ResizeToRatio() expects right/bottom members to be width/height
	//			rather than absolute coordinates
	rcMax.left   = m_rcContainer.left;
	rcMax.top    = m_rcContainer.top;
	rcMax.right  = (m_rcContainer.right - m_rcContainer.left);
	rcMax.bottom = (m_rcContainer.bottom - m_rcContainer.top);
	m_TMLead.ResizeToRatio(&rcMax, fOriginalRatio);

	//	Compute the scale factor required to do the conversions
	//
	//	NOTE:	rcMax.right = width (not right hand coordinate)
	m_fScaleFactor = ((float)rcMax.right / fOriginalWidth);

	//	Get the dimensions of the original window
	fWidth = (float)(m_rcOriginalPosition.right - m_rcOriginalPosition.left);
	fHeight = (float)(m_rcOriginalPosition.bottom - m_rcOriginalPosition.top);

	//	Compute the offset of the upper left corner of the original window to
	//	the upper left coordinate of the original container
	fXOffset = (float)(m_rcOriginalPosition.left - m_rcOriginalContainer.left);
	fYOffset = (float)(m_rcOriginalPosition.top - m_rcOriginalContainer.top);

	//	Apply the scale factors to the offsets and dimensions
	fXOffset *= m_fScaleFactor;
	fYOffset *= m_fScaleFactor;
	fWidth   *= m_fScaleFactor;
	fHeight  *= m_fScaleFactor;

	//fXOffset += (m_fXChange * m_fScaleFactor);
	//fYOffset += (m_fYChange * m_fScaleFactor);

	//	We must also scale the frame around the callout
	iFrame = ROUND((float)GetFrameThickness() * m_fScaleFactor);

	//	Get the new position for the callout window
	rcPosition.left   = rcMax.left + ROUND(fXOffset);
	rcPosition.top    = rcMax.top + ROUND(fYOffset);
	rcPosition.right  = rcPosition.left + ROUND(fWidth);
	rcPosition.bottom = rcPosition.top  + ROUND(fHeight);

	//	Set the callout rectangles
	SetRects(&rcPosition, &m_rcDst, &m_rcRubberBand, iFrame);
//m_pSource->ShowRectangles("rescale rects");

}

//==============================================================================
//
// 	Function Name:	CCallout::Resize()
//
// 	Description:	This function is called to resize the window when the
//					user grabs one of the handles.
//
// 	Returns:		TRUE if resized
//
//	Notes:			None
//
//==============================================================================
BOOL CCallout::Resize() 
{
	RECT rcTrack;
	RECT rcWnd;

	//	Is resizing disabled?
	if(!m_bResizeable) return FALSE;

	//	Is the user resizing the callout?
	if(!m_Tracker.Track(&rcTrack))
		return FALSE;

	//	Get the current position of the callout window
	GetWindowRect(&rcWnd);

	//	Adjust for the new size / position
	rcWnd.left  += rcTrack.left;
	rcWnd.top   += rcTrack.top;
	rcWnd.right  = rcWnd.left + (rcTrack.right - rcTrack.left);
	rcWnd.bottom = rcWnd.top + (rcTrack.bottom - rcTrack.top);

	SetRects(&rcWnd, &m_rcDst, &m_rcRubberBand, m_Tracker.GetFrameThickness());

	//	Notify the source
	if(m_pSource != 0)
		m_pSource->OnCalloutResized(this);
	
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CCallout::ResizeSourceToImage()
//
// 	Description:	This function will resize the source rectangle to contain
//					the full image.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::ResizeSourceToImage()
{
	m_TMLead.ResizeSourceToImage();
}

//==============================================================================
//
// 	Function Name:	CCallout::ResizeSourceToView()
//
// 	Description:	This function will resize the source rectangle to match
//					the current view.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::ResizeSourceToView()
{
	float	fSrcLeft;
	float	fSrcTop;
	float	fSrcWidth;
	float	fSrcHeight;
	float	fLeft;
	float	fTop;
	float	fWidth;
	float	fHeight;
	float	fXScale;
	float	fYScale;

	//	Get the dimensions of the current destination rectangle so that we can
	//	translate the the current view coordinates into source rectangle
	//	coordinates
	fTop    = m_TMLead.GetDstTop();
	fLeft   = m_TMLead.GetDstLeft();
	fWidth  = m_TMLead.GetDstWidth();
	fHeight = m_TMLead.GetDstHeight();

	//	Calculate the current scale factors used to convert points in the 
	//	destination rectangle to points in the source rectangle
	fXScale = fWidth / m_TMLead.GetImageWidth();
	fYScale = fHeight /  m_TMLead.GetImageHeight();

	//	Calculate the coordinates of the source rectangle using the size of the
	//	current view (window). The window rectangle determines the portion of
	//	the destination rectangle that we are currently viewing
	//
	//	NOTE:	We have to subtract out the size of the frame being placed around
	//			the image to get the correct image rectangle
	fSrcWidth  = (float)m_iWidth / fXScale;
	fSrcHeight = (float)m_iHeight / fYScale;
	fSrcLeft   = ((-1 * fLeft) / fXScale);
	fSrcTop    = ((-1 * fTop) / fYScale);

	//	Set the source rectangles 
	m_TMLead.SetSrcRect(fSrcLeft, fSrcTop, fSrcWidth, fSrcHeight);
	m_TMLead.SetSrcClipRect(fSrcLeft, fSrcTop, fSrcWidth, fSrcHeight);
}

//==============================================================================
//
// 	Function Name:	CCallout::Rotate()
//
// 	Description:	This function is called to set the rotate the callout
//					clockwise or counterclockwise
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::Rotate(BOOL bClockwise, BOOL bRedraw)
{

/*
	short sRotation = m_TMLead.m_sRotation;

	RECT rcWnd;
	GetWindowRect(&rcWnd);
	m_TMLead.RotateRect(&rcWnd, bClockwise);
	MoveWindow(&rcWnd);

	if(bClockwise == TRUE)
		m_TMLead.RotateCw(bRedraw);
	else
		m_TMLead.RotateCcw(bRedraw);

*/
}

//==============================================================================
//
// 	Function Name:	CCallout::SaveZap()
//
// 	Description:	This function will write the information required to restore
//					the callout to the zap file provided by the caller.
//
// 	Returns:		TRUE if successful.
//
//	Notes:			This function assumes the file is properly positioned before
//					it is called.
//
//==============================================================================
BOOL CCallout::SaveZap(CFile* pFile) 
{
	SZapCallout	Callout;
	HGLOBAL		hAnnMem  = 0;
	long		lAnnSize = 0;
	LPBYTE		lpAnnMem = 0;
	BOOL		bReturn;
	char		szErrorMsg[256];

	//	Is the parameter valid?
	ASSERT(pFile);
	ASSERT(pFile->m_hFile != CFile::hFileNull);
	if(!pFile || pFile->m_hFile == CFile::hFileNull)
		return FALSE;

	//	Do we have any annotations to save?
	if(m_TMLead.GetAnnContainer() != 0)
		if(m_TMLead.AnnSaveMemory((long FAR *)&hAnnMem, ANNFMT_XML, 
								  FALSE, &lAnnSize, SAVE_OVERWRITE, 1))
			return FALSE;

	//	Now write the information to the file
	try
	{
		//	Initialize the callout descriptor
		ZeroMemory(&Callout, sizeof(Callout));
		Callout.lAnnBytes = lAnnSize;
		Callout.sAngle = m_TMLead.GetAngle();
		Callout.wAnnId = m_wAnnId;
		memcpy(&(Callout.rcRubberBand), &m_rcRubberBand, sizeof(RECT));
		memcpy(&(Callout.rcDstRect), &m_rcDst, sizeof(RECT));
		memcpy(&(Callout.rcMax), &m_rcMax, sizeof(RECT));
		GetWindowRect(&(Callout.rcPosition));

		if(m_bShaded)
			Callout.wFlags |= CALLOUT_ZAP_SHADED;

		//	Write the descriptor to the file
		pFile->Write(&Callout, sizeof(Callout));

		//	Do we have any annotations to write?
		if(hAnnMem && lAnnSize > 0)
		{
			//	Lock the memory
			lpAnnMem = (LPBYTE)GlobalLock(hAnnMem);
			ASSERT(lpAnnMem);

			pFile->Write(lpAnnMem, lAnnSize);
		}	

		bReturn = TRUE;
	}

	//	Catch all file exceptions
	catch(CFileException* pFileException)
	{
		//	Get the error message and pass it on to the handler
		pFileException->GetErrorMessage(szErrorMsg, sizeof(szErrorMsg));
		pFileException->Delete();

		if(m_pSource)
			(m_pSource->GetHandler())->Handle(0, szErrorMsg);
		bReturn = FALSE;
	}
	//	Catch all other exceptions
	catch(CException* pException)
	{
		//	Get the error message and pass it on to the handler
		pException->GetErrorMessage(szErrorMsg, sizeof(szErrorMsg));
		pException->Delete();

		if(m_pSource)
			(m_pSource->GetHandler())->Handle(0, szErrorMsg);
		bReturn = FALSE;
	}

	//	Deallocate the annotation memory
	if(lpAnnMem)
		GlobalUnlock(hAnnMem);
	if(hAnnMem)
		GlobalFree(hAnnMem);
		
	return bReturn;
}

//==============================================================================
//
// 	Function Name:	CCallout::ScalePrintRect()
//
// 	Description:	This function is called to adjust the specified rectangle so
//					that it's aspect ratio matches that of the container.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CCallout::ScalePrintRect(RECT* prcPrint, double* pdWidth, double* pdHeight) 
{
	double	dRatio = 0;
	double	dHeight;
	double	dWidth;
	double	dCx;
	double	dCy;

	ASSERT(prcPrint);
	ASSERT(pdWidth);
	ASSERT(pdHeight);

	//	Get the dimensions of the container
	*pdWidth = (double)(m_rcContainer.bottom - m_rcContainer.top);
	*pdHeight  = (double)(m_rcContainer.right - m_rcContainer.left);

	//	Get the aspect ratio of the container
	if(*pdHeight != 0.0)
		dRatio = *pdWidth / *pdHeight;
	if(dRatio <= 0)
		return FALSE;

	//	Get the size of the caller's rectangle
	dWidth  = (double)(prcPrint->right - prcPrint->left);
	dHeight = (double)(prcPrint->bottom - prcPrint->top);

	ASSERT(dHeight >= 2.0);
	ASSERT(dWidth >= 2.0);
	if((dWidth < 2.0) || (dHeight < 2.0))
		return FALSE;

	//	Does the rectangle need to be resized?
	if((dWidth / dHeight) != dRatio)
	{
		//	Calculate the center point
		dCx = (double)prcPrint->left + (dWidth / 2.0);
		dCy = (double)prcPrint->top + (dHeight / 2.0);

		if((dHeight * dRatio) > dWidth)
		{
			//	Resize the height
			dHeight = dWidth / dRatio;
		}
		else
		{
			//	Set the width
			dWidth = dHeight * dRatio;
		}

		//	Reset the coordinates
		prcPrint->left   = ROUND((dCx - (dWidth / 2.0)));
		prcPrint->top    = ROUND((dCy - (dHeight / 2.0)));
		prcPrint->right  = prcPrint->left + ROUND(dWidth);
		prcPrint->bottom = prcPrint->top + ROUND(dHeight);
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CCallout::SelectAnn()
//
// 	Description:	This function will select the annotation object specified
//					by the caller
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SelectAnn(DWORD dwTag) 
{
	if(IsWindow(m_TMLead.m_hWnd))
		m_TMLead.SelectAnn(dwTag);
}

//==============================================================================
//
// 	Function Name:	CCallout::SetAction()
//
// 	Description:	This function is called by the source control to set the
//					current action for the callout
//
// 	Returns:		None
//
//	Notes:			Callouts are not permitted from callouts
//
//==============================================================================
void CCallout::SetAction(short sAction) 
{
	//	Save the requested action
	m_sAction = sAction;

	//	Are annotations enabled?
	if(m_bAnnotateCallouts)
	{
		//	What is the new action?
		switch(sAction)
		{
			case DRAW:
			case REDACT:
			case HIGHLIGHT:
			case SELECT:		m_TMLead.SetAction(sAction);
								break;		
			
			case PAN:			m_TMLead.SetAction(m_bPanCallouts ? PAN : NONE);
								break;

			case ZOOM:			m_TMLead.SetAction(m_bZoomCallouts ? ZOOM : NONE);
								break;

			case CALLOUT:
			default:			m_TMLead.SetAction(NONE);
								break;
		}
	}
}

//==============================================================================
//
// 	Function Name:	CCallout::SetAnnColor()
//
// 	Description:	This function is called by the source control when the
//					annotation color is changed
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetAnnColor(short sAnnColor) 
{
	m_TMLead.SetAnnColor(sAnnColor);
}

//==============================================================================
//
// 	Function Name:	CCallout::SetAnnColorDepth()
//
// 	Description:	This function is called by the source control when the
//					annotation color depth is changed
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetAnnColorDepth(short sAnnColorDepth) 
{
	m_TMLead.SetAnnColorDepth(sAnnColorDepth);
}

//==============================================================================
//
// 	Function Name:	CCallout::SetAnnFontBold()
//
// 	Description:	This function is called by the source control when the
//					AnnFontBold property is changed
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetAnnFontBold(BOOL bBold) 
{
	m_TMLead.SetAnnFontBold(bBold);
}

//==============================================================================
//
// 	Function Name:	CCallout::SetAnnFontName()
//
// 	Description:	This function is called by the source control when the
//					AnnFontName property is changed
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetAnnFontName(LPCTSTR lpName) 
{
	m_TMLead.SetAnnFontName(lpName);
}

//==============================================================================
//
// 	Function Name:	CCallout::SetAnnFontSize()
//
// 	Description:	This function is called by the source control when the
//					AnnFontSize property is changed
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetAnnFontSize(short sSize) 
{
	m_TMLead.SetAnnFontSize(sSize);
}

//==============================================================================
//
// 	Function Name:	CCallout::SetAnnFontStrikeThrough()
//
// 	Description:	This function is called by the source control when the
//					AnnFontStrikeThrough property is changed
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetAnnFontStrikeThrough(BOOL bStrikeThrough) 
{
	m_TMLead.SetAnnFontStrikeThrough(bStrikeThrough);
}

//==============================================================================
//
// 	Function Name:	CCallout::SetAnnFontUnderline()
//
// 	Description:	This function is called by the source control when the
//					AnnFontUnderline property is changed
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetAnnFontUnderline(BOOL bUnderline) 
{
	m_TMLead.SetAnnFontUnderline(bUnderline);
}

//==============================================================================
//
// 	Function Name:	CCallout::SetAnnId()
//
// 	Description:	This function is called to set the identifier of the 
//					callout's highlight in the source image
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetAnnId(WORD wId) 
{
	m_wAnnId = wId;
}

//==============================================================================
//
// 	Function Name:	CCallout::SetAnnotateCallouts()
//
// 	Description:	This function is called by the control object when the
//					AnnotateCallouts property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetAnnotateCallouts(BOOL bAnnotateCallouts) 
{
	m_bAnnotateCallouts = bAnnotateCallouts;
	
	//	Set the appropriate tool
	if(bAnnotateCallouts)
		m_TMLead.SetAction(m_sAction);
	else
		m_TMLead.SetAction(NONE);
}

//==============================================================================
//
// 	Function Name:	CCallout::SetAnnotations()
//
// 	Description:	This function is to set the annotations for the callout
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetAnnotations(HGLOBAL hAnnMem, long lAnnBytes) 
{
	m_TMLead.SetAnnotations(hAnnMem, lAnnBytes);
}

//==============================================================================
//
// 	Function Name:	CCallout::SetAnnThickness()
//
// 	Description:	This function is called by the source control when the
//					annotation thickness changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetAnnThickness(short sAnnThickness) 
{
	m_TMLead.SetAnnThickness(sAnnThickness);
}

//==============================================================================
//
// 	Function Name:	CCallout::SetAnnTool()
//
// 	Description:	This function is called by the source control when the
//					annotation tool changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetAnnTool(short sAnnTool) 
{
	m_TMLead.SetAnnTool(sAnnTool);
}

//==============================================================================
//
// 	Function Name:	CCallout::SetContainer()
//
// 	Description:	This function is called to set the rectangle used as a
//					reference for rescaling the window.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetContainer(RECT* pContainer) 
{
	ASSERT(pContainer);
	memcpy(&m_rcContainer, pContainer, sizeof(m_rcContainer));
}

//==============================================================================
//
// 	Function Name:	CCallout::SetFrameColor()
//
// 	Description:	This function is called by the source pane when the frame
//					color gets changed.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetFrameColor(COLORREF crColor) 
{
	//	Create a new brush
	if(m_pBackground) delete m_pBackground;
	m_pBackground = new CBrush();
	m_pBackground->CreateSolidBrush(crColor);

	//	Keep the panes synchronized
	m_TMLead.SetCallFrameColor(crColor);

	//	Repaint the border if visible
	if(IsWindow(m_hWnd) && IsWindowVisible())
		RedrawWindow();
}

//==============================================================================
//
// 	Function Name:	CCallout::SetFrameThickness()
//
// 	Description:	This function is called by the source control when the
//					callout frame thickness changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetFrameThickness(short sThickness) 
{
	m_Tracker.SetFrameThickness(sThickness);

	//	Keep the panes synchronized
	m_TMLead.SetCallFrameThickness(sThickness);
}

//==============================================================================
//
// 	Function Name:	CCallout::SetHandleColor()
//
// 	Description:	This function is called to set the color used to draw
//					the grab handles
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetHandleColor(COLORREF crColor)
{
	//	Notify the tracker
	m_Tracker.SetHandleColor(crColor);
	
	//	Redraw if the window is visible
	if(m_bResizeable && IsWindow(m_hWnd) && IsWindowVisible())
		m_Tracker.Draw();
}

//==============================================================================
//
// 	Function Name:	CCallout::SetHideScrollBars()
//
// 	Description:	This function is called by the source control when the
//					pan image property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetHideScrollBars(BOOL bHideScrollBars) 
{
	m_TMLead.SetHideScrollBars(bHideScrollBars);
}

//==============================================================================
//
// 	Function Name:	CCallout::SetHighlightColor()
//
// 	Description:	This function is called by the source control when the
//					highlight color changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetHighlightColor(short sHighlightColor) 
{
	m_TMLead.SetHighlightColor(sHighlightColor);
}

//==============================================================================
//
// 	Function Name:	CCallout::SetOriginalContainer()
//
// 	Description:	This function is called to set the rectangle used as a
//					reference for rescaling the window.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetOriginalContainer(RECT* pContainer) 
{
	ASSERT(pContainer);
	memcpy(&m_rcOriginalContainer, pContainer, sizeof(m_rcOriginalContainer));
}

//==============================================================================
//
// 	Function Name:	CCallout::SetOriginalPosition()
//
// 	Description:	This function is called to set the rectangle used as a
//					reference for rescaling the window.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetOriginalPosition(RECT* pPosition) 
{
	ASSERT(pPosition);
	memcpy(&m_rcOriginalPosition, pPosition, sizeof(m_rcOriginalPosition));
}

//==============================================================================
//
// 	Function Name:	CCallout::SetPanCallouts()
//
// 	Description:	This function is called by the control object when the
//					PanCallouts property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetPanCallouts(BOOL bPan) 
{
	m_bPanCallouts = bPan;
	
	//	Is the source zooming?
	if(m_pSource->GetAction() == ZOOM)
	{
		if(m_bPanCallouts)
			m_TMLead.SetAction(PAN);
		else
			m_TMLead.SetAction(NONE);
	}

}

//==============================================================================
//
// 	Function Name:	CCallout::SetRects()
//
// 	Description:	This function is called to set the rectangles used to size
//					and position the callout
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetRects(RECT* pMax, RECT *pDst, RECT* pRubberBand, int iFrame) 
{
	RECT	rcDstCall;
	RECT	rcTMLead;
	RECT	rcWnd;
	float	fMaxWidth;
	float	fMaxHeight;
	float	fRubberBandWidth;
	float	fRubberBandHeight;
	float	fRubberBandRatio;
	float	fWidth;
	float	fHeight;
	float	fCx;
	float	fCy;
	float	fDstXFactor;
	float	fDstYFactor;
			
	ASSERT(pMax);
	ASSERT(pDst);
	ASSERT(pRubberBand);

	//	Save the rubber band rectangle
	m_rcRubberBand.left   = pRubberBand->left;
	m_rcRubberBand.top    = pRubberBand->top;
	m_rcRubberBand.right  = pRubberBand->right;		//	Actual coordinate (not width)
	m_rcRubberBand.bottom = pRubberBand->bottom;	//	Actual coordinage (not height)

	//	Save the coordinates of the bounding rectangle
	m_rcMax.left   = pMax->left;
	m_rcMax.top    = pMax->top;
	m_rcMax.right  = pMax->right;	//	Actual coordinate (not width)
	m_rcMax.bottom = pMax->bottom;	//	Actual coordinate (not height)

	//	Save the destination rectangle of the source image
	//
	//	NOTE:	The top/left position has already been offset by the top/left
	//			position of the rubber band rectangle
	m_rcDst.left   = pDst->left;
	m_rcDst.top    = pDst->top;
	m_rcDst.right  = pDst->right;	//	Width of rectangle
	m_rcDst.bottom = pDst->bottom;	//	Height of rectangle

	//	Get the size of the called out section
	fRubberBandWidth  = (float)(pRubberBand->right  - pRubberBand->left);
	fRubberBandHeight = (float)(pRubberBand->bottom - pRubberBand->top);

	//	Get the maximum available area for displaying the image
	//
	//	NOTE: We have to subtract out the border all the way around the image
	fMaxWidth  = (float)(pMax->right - pMax->left) - (2.0f * (float)iFrame);
	fMaxHeight = (float)(pMax->bottom - pMax->top) - (2.0f * (float)iFrame);

	//	Get the aspect ratio of the called out section
	ASSERT(fRubberBandWidth != 0);
	ASSERT(fRubberBandHeight != 0);
	fRubberBandRatio = fRubberBandHeight / fRubberBandWidth;

	//	Now calculate the dimensions of the TMLead window such that it will
	//	have the desired aspect ratio
	if((fMaxWidth * fRubberBandRatio) <= fMaxHeight)
	{
		fWidth  = fMaxWidth;
		fHeight = fMaxWidth * fRubberBandRatio;
	}
	else
	{
		fHeight = fMaxHeight;
		ASSERT(fRubberBandRatio != 0);
		fWidth  = fMaxHeight / fRubberBandRatio;
	}

    //  Find the center point of the bounding rectangle (not the Lead window)
	//
	//	NOTE:	Keep in mind that these are screen coordinates
    fCx = (float)pMax->left + ((float)(pMax->right - pMax->left) / 2);
    fCy = (float)pMax->top + ((float)(pMax->bottom - pMax->top) / 2);

	//	Get the dimensions of the callout window (not the Lead window)
	m_iWidth  = ROUND(fWidth) + (2 * iFrame);
	m_iHeight = ROUND(fHeight) + (2 * iFrame);

	//	Calculate the coordinates of the callout window
	rcWnd.left   = ROUND(fCx - ((float)m_iWidth / 2.0));
	rcWnd.top    = ROUND(fCy - ((float)m_iHeight / 2.0));
	rcWnd.right  = rcWnd.left + m_iWidth;
	rcWnd.bottom = rcWnd.top + m_iHeight;

    //  Calculate the coordinates of the LeadTool window
	//
	//	NOTE: The TMLead coordinates are in client area coordinates and the
	//		  right/bottom members are for width/height (not absolute coordinates)
    rcTMLead.left   = iFrame;
    rcTMLead.top    = iFrame;
	rcTMLead.right  = ROUND(fWidth);
	rcTMLead.bottom = ROUND(fHeight);

    //  Set the view size and offset it to center it in the bounding window 
	if(IsWindow(m_hWnd))
	{
		//	Move the window into position on the screen
		//
		//	NOTE:	We MUST force redrawing here for the change to get propagated
		//			to the parent window
		MoveWindow(&rcWnd, TRUE);

		//	Move the Lead control into position
		m_TMLead.SetMaxRect(&rcTMLead, FALSE, TRUE);

		//	Update the tracker 
		m_Tracker.Move();
	}

	//	Make sure we are allowed to specify the destination rectangle
	m_TMLead.SetZoomToRect(FALSE);

	//	Now we have to determine the factor we are going to multiply the
	//	destination rectangle by in order to get the callout region to 
	//	fill up the window. 
	ASSERT(fRubberBandWidth != 0);
	ASSERT(fRubberBandHeight != 0);
	fDstXFactor = fWidth  / fRubberBandWidth;
	fDstYFactor = fHeight  / fRubberBandHeight;

	//	Set the coordinates of the destination rectangle for the callout
	rcDstCall.top    = ROUND((float)pDst->top * fDstYFactor);
	rcDstCall.left   = ROUND((float)pDst->left * fDstXFactor);
	rcDstCall.bottom = ROUND((float)pDst->bottom * fDstYFactor);
	rcDstCall.right  = ROUND((float)pDst->right * fDstXFactor);

	//	Set the callout's destination rectangle
	m_TMLead.SetDstRects(&rcDstCall, 0);

	//	We could just force a repaint here but this puts the TMLead object in
	//	the appropriate state
	m_TMLead.RedrawZoomed();
	//m_TMLead.SetZoomToRect(TRUE);

	//	Set the values used by the tracker
	m_Tracker.SetAspectRatio(m_TMLead.GetWndAspect());
	m_Tracker.SetFrameThickness(iFrame);
}

//==============================================================================
//
// 	Function Name:	CCallout::SetRedactColor()
//
// 	Description:	This function is called by the source control when the
//					redact color property changes.
//
// 	Returns:		None
//
//	Notes:			This property determines the color used for redact drawing.
//
//==============================================================================
void CCallout::SetRedactColor(short sRedactColor) 
{
	m_TMLead.SetRedactColor(sRedactColor);
}

//==============================================================================
//
// 	Function Name:	CCallout::SetResizable()
//
// 	Description:	This function is called to set the flag used to enable
//					resizing
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetResizeable(BOOL bResizeable)
{
	m_bResizeable = bResizeable;

	//	Force a redrawing if the window is visible
	if(IsWindow(m_hWnd) && IsWindowVisible())
		RedrawWindow();
}

//==============================================================================
//
// 	Function Name:	CCallout::SetRightClickPan()
//
// 	Description:	This function is called by the source control when the
//					right click pan property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetRightClickPan(BOOL bRightClickPan) 
{
	m_TMLead.SetRightClickPan(bRightClickPan);
}

//==============================================================================
//
// 	Function Name:	CCallout::SetShaded()
//
// 	Description:	This function is called to set the flag that indicates 
//					whether or not the highlight for this callout is shaded
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetShaded(BOOL bShaded) 
{
	m_bShaded = bShaded;
}

//==============================================================================
//
// 	Function Name:	CCallout::SetZoomCallouts()
//
// 	Description:	This function is called by the control object when the
//					ZoomCallouts property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::SetZoomCallouts(BOOL bZoom) 
{
	m_bZoomCallouts = bZoom;
	
	//	Is the source zooming?
	if(m_pSource->GetAction() == ZOOM)
	{
		if(m_bZoomCallouts)
			m_TMLead.SetAction(ZOOM);
		else
			m_TMLead.SetAction(NONE);
	}

}

//==============================================================================
//
// 	Function Name:	CCallout::ShowRectangle()
//
// 	Description:	This function is provided as a debugging aid. It will 
//					display the dimensions of the specified rectangle.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::ShowRectangle(RECT* pRect, LPCSTR lpTitle) 
{
	CString	Msg;
	//	Bounding window rectangle
	Msg.Format("L: %d\nT: %d\nR: %d\nB: %d\nW: %d\nH: %d",
			   pRect->left, pRect->top, pRect->right, pRect->bottom,
			   (pRect->right - pRect->left), (pRect->bottom - pRect->top));


	MessageBox(Msg, lpTitle, MB_ICONINFORMATION | MB_OK);
}

//==============================================================================
//
// 	Function Name:	CCallout::ShowRectangles()
//
// 	Description:	This function is provided as a debugging aid. It will 
//					display the dimensions of the callout rectangles.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::ShowRectangles(LPCSTR lpTitle) 
{
	CString	Msg;
	CString Tmp;

	//	Bounding window rectangle
	Msg.Format("MaxLeft %d\nMaxTop %d\nMaxRight %d\nMaxBottom %d\n",
			   m_rcMax.left, m_rcMax.top, m_rcMax.right, m_rcMax.bottom);

	//	Callout rectangle
	Tmp.Format("CallLeft %d\nCallTop %d\nCallRight %d\nCallBottom %d\n",
			   m_rcRubberBand.left, m_rcRubberBand.top, m_rcRubberBand.right, m_rcRubberBand.bottom);
	Msg += Tmp;

	//	Destination rectangle
	Tmp.Format("DstLeft %d\nDstTop %d\nDstRight %d\nDstBottom %d\n",
			   m_rcDst.left, m_rcDst.top, m_rcDst.right, m_rcDst.bottom);
	Msg += Tmp;

	//	Original container rectangle
	Tmp.Format("OrgContainLeft %d\nOrgContainTop %d\nOrgContainRight %d\nOrgContainBottom %d\n",
			   m_rcOriginalContainer.left, m_rcOriginalContainer.top, m_rcOriginalContainer.right, m_rcOriginalContainer.bottom);
	Msg += Tmp;

	//	Original position rectangle
	Tmp.Format("OrgPosLeft %d\nOrgPosTop %d\nOrgPosRight %d\nOrgPosBottom %d\n",
			   m_rcOriginalPosition.left, m_rcOriginalPosition.top, m_rcOriginalPosition.right, m_rcOriginalPosition.bottom);
	Msg += Tmp;

	//	Original position rectangle
	Tmp.Format("ContainerLeft %d\nContainerTop %d\nContainerRight %d\nContainerBottom %d\n",
			   m_rcContainer.left, m_rcContainer.top, m_rcContainer.right, m_rcContainer.bottom);
	Msg += Tmp;

	MessageBox(Msg, lpTitle, MB_ICONINFORMATION | MB_OK);

}

//==============================================================================
//
// 	Function Name:	CCallout::StartDrag()
//
// 	Description:	This function is called to initialize a drag operation.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallout::StartDrag(long lX, long lY) 
{
	m_bDrag  = TRUE;
	m_lDragX = lX;
	m_lDragY = lY;
	GetWindowRect(&m_rcDrag);
	DrawDragRect();
	::SetCapture(m_hWnd);
}

//==============================================================================
//
// 	Function Name:	CCallouts::Add()
//
// 	Description:	This function will add a callout object to the list
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CCallouts::Add(CCallout* pCallout)
{
	ASSERT(pCallout);
	if(!pCallout)
		return FALSE;

	try
	{
		AddTail(pCallout);
		return TRUE;
	}
	catch(...)
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CCallout::CCallouts()
//
// 	Description:	This is the constructor for CCallouts objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CCallouts::CCallouts()
{
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CCallout::~CCallouts()
//
// 	Description:	This is the destructor for CCallouts objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CCallouts::~CCallouts()
{
	//	Flush the list without destroying its objects
	Flush(TRUE);
}

//==============================================================================
//
// 	Function Name:	CCallouts::Find()
//
// 	Description:	This function will find the position of the object in the
//					list.
//
// 	Returns:		NULL if not found.
//
//	Notes:			None
//
//==============================================================================
POSITION CCallouts::Find(CCallout* pCallout)
{
	return (CObList::Find(pCallout));
}

//==============================================================================
//
// 	Function Name:	CCallouts::CheckShaded()
//
// 	Description:	This function will check the list to see if any of the
//					callout objects have shaded highlights
//
// 	Returns:		TRUE if one or more have shaded highlights
//
//	Notes:			None
//
//==============================================================================
BOOL CCallouts::CheckShaded()
{
	POSITION	Pos;
	CCallout*	pCallout;

	Pos = GetHeadPosition();
	while(Pos != NULL)
	{
		if((pCallout = (CCallout*)GetNext(Pos)) != 0)
			if(pCallout->GetShaded())
				return TRUE;
	}
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CCallouts::Find()
//
// 	Description:	This function will locate the object with the same 
//					annotation id as that provided by the caller.
//
// 	Returns:		A pointer to the callout object
//
//	Notes:			None
//
//==============================================================================
CCallout* CCallouts::Find(WORD wAnnId)
{
	POSITION	Pos;
	CCallout*	pCallout;

	Pos = GetHeadPosition();
	while(Pos != NULL)
	{
		if((pCallout = (CCallout*)GetNext(Pos)) != 0)
			if(pCallout->GetAnnId() == wAnnId)
				return pCallout;
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CCallouts::First()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list
//
//	Notes:			This function is typically called to set the list up for
//					forward iteration
//
//==============================================================================
CCallout* CCallouts::First()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CCallout*)GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CCallouts::Flush()
//
// 	Description:	This function will flush all objects from the list
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the objects are destroyed when they
//					are removed from the list
//
//==============================================================================
void CCallouts::Flush(BOOL bDelete)
{
	CCallout* pCallout;

	//	Do we want to delete the objects?
	if(bDelete)
	{
		m_NextPos = GetHeadPosition();

		while(m_NextPos != NULL)
		{
			if((pCallout = (CCallout*)GetNext(m_NextPos)) != 0)
				delete pCallout;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;
}

//==============================================================================
//
// 	Function Name:	CCallouts::Last()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list
//
//	Notes:			This function is typically called to set the list up for
//					reverse iteration
//
//==============================================================================
CCallout* CCallouts::Last()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CCallout*)GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CCallouts::Next()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list
//
//	Notes:			None
//
//==============================================================================
CCallout* CCallouts::Next()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		GetPrev(m_PrevPos);
		return (CCallout*)GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CCallouts::Prev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. 
//
//	Notes:			None
//
//==============================================================================
CCallout* CCallouts::Prev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		GetNext(m_NextPos);
		return (CCallout*)GetPrev(m_PrevPos);
	}
}

//==============================================================================
//
// 	Function Name:	CCallouts::Remove()
//
// 	Description:	This function will remove the object provided by the caller
//					from the list. If bDelete == TRUE, the object is deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CCallouts::Remove(CCallout* pCallout, BOOL bDelete)
{
	POSITION Pos = Find(pCallout);

	//	Is this object in the list
	if(Pos != NULL)
	{
		RemoveAt(Pos);

		//	Do we need to delete the object?
		if(bDelete)
			delete pCallout;
	}
}

