//==============================================================================
//
// File Name:	tmtrack.cpp
//
// Description:	This file contains member functions of the CTMTracker class
//
// See Also:	tmtrack.h 
//
// Copyright FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	06-01-02	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmtrack.h>
#include <tmviewap.h>

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

//==============================================================================
//
// 	Function Name:	CTMTracker::AdjustRect()
//
// 	Description:	This is an overloaded version of the base class member
//					that gets called as the user changes the size of the
//					tracking rectangle.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTracker::AdjustRect(int iHandle, LPRECT lpRect)
{
	float	fWidth = (float)(lpRect->right - lpRect->left) - (2.0f * (float)m_iFrameThickness);
	float	fHeight = (float)(lpRect->bottom - lpRect->top) - (2.0f * (float)m_iFrameThickness);

	if (!m_sMaintainAspectRatio) 
	{
	//  Keep the maximum available rectangle while maintaining the requested
	//	adjustment ratio
    if((fWidth * m_fAspectRatio) <= fHeight)
    {
        fHeight = fWidth * m_fAspectRatio + (2.0f * (float)m_iFrameThickness);
        fWidth += (2.0f * (float)m_iFrameThickness);
    }
    else
    {   
        fWidth = fHeight / m_fAspectRatio + (2.0f * (float)m_iFrameThickness);
        fHeight += (2.0f * (float)m_iFrameThickness);
   }
	}

	//	Has the owner set the adjustment ratio?
	if(m_fAspectRatio > 0)
	{
		//	Which handle is active?
		switch(Translate(iHandle))
		{
			case TMTRACKER_GH_BOTTOMRIGHT:	
			
				//	Keep top/left pinned
				lpRect->right = lpRect->left + ROUND(fWidth );
				lpRect->bottom = lpRect->top + ROUND(fHeight);
				break;

			case TMTRACKER_GH_BOTTOMLEFT:
			
				//	Keep top/right pinned
				lpRect->left = lpRect->right - ROUND(fWidth);
				lpRect->bottom = lpRect->top + ROUND(fHeight);
				break;

			case TMTRACKER_GH_TOPRIGHT:	
			
				//	Keep bottom/left pinned
				lpRect->right = lpRect->left + ROUND(fWidth);
				lpRect->top = lpRect->bottom - ROUND(fHeight);
				break;

			case TMTRACKER_GH_TOPLEFT:	
			
				//	Keep bottom/right pinned
				lpRect->left = lpRect->right - ROUND(fWidth);
				lpRect->top = lpRect->bottom - ROUND(fHeight);
				break;

			case TMTRACKER_GH_RIGHT:	
			case TMTRACKER_GH_LEFT:		
			case TMTRACKER_GH_TOP:		
			case TMTRACKER_GH_BOTTOM:	
			default:				
			
				ASSERT(0);
				return;
		}

	}

	CRectTracker::AdjustRect(iHandle, lpRect);
}

//==============================================================================
//
// 	Function Name:	CTMTracker::Attach()
//
// 	Description:	This function is called to attach the tracker to the 
//					specified window.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTracker::Attach(HWND hWnd, short maintainAspectRatio)
{
	m_hAttachment = hWnd;
	m_sMaintainAspectRatio = maintainAspectRatio;

	//	Set the initial size and position
	Move();
}

//==============================================================================
//
// 	Function Name:	CTMTracker::CTMTracker()
//
// 	Description:	This is the constructor for CTMTracker objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMTracker::CTMTracker() : CRectTracker()
{
	m_hAttachment = 0;
	m_nHandleSize = 5;
	m_nStyle = CRectTracker::resizeInside;
	m_fAspectRatio = 0;
	m_iFrameThickness = 0;
	m_crHandle = RGB(0xFF, 0xFF, 0xFF);
}

//==============================================================================
//
// 	Function Name:	CTMTracker::~CTMTracker()
//
// 	Description:	This is the destructor for CTMTracker objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMTracker::~CTMTracker()
{
}

//==============================================================================
//
// 	Function Name:	CTMTracker::Draw()
//
// 	Description:	This function is called to draw the tracker grab handles
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTracker::Draw()
{
	CDC*	pdc;
	CRect	rcNormalized;

	//	Get the device context for the attached window
	if((pdc = CDC::FromHandle(GetDC(m_hAttachment))) == 0) return;

	//	Save the current state of the device context
	VERIFY(pdc->SaveDC() != 0);

	//	Initialize the device context for drawing the grab handles
	pdc->SetMapMode(MM_TEXT);
	pdc->SetViewportOrg(0, 0);
	pdc->SetWindowOrg(0, 0);

	//	Get a normalized version of the tracker rectangle
	rcNormalized = m_rect;
	rcNormalized.NormalizeRect();

	//	Draw the grab handles
	for(int i = 0; i < 4; ++i)
	{
		GetHandleRect((TrackerHit)i, &rcNormalized);
		pdc->FillSolidRect(rcNormalized, m_crHandle);
	}

	//	Restore the saved state of the device context
	VERIFY(pdc->RestoreDC(-1));
}

//==============================================================================
//
// 	Function Name:	CTMTracker::GetHandle()
//
// 	Description:	This function is called to determine which grab handle is
//					under the current mouse position.
//
// 	Returns:		One of the defined grab handle identifiers (tmtrack.h)
//
//	Notes:			None
//
//==============================================================================
int CTMTracker::GetHandle()
{
	CPoint Pos;

	//	Get the current position of the mouse
	if(!GetPosition(&Pos))
		return TMTRACKER_GH_NONE;

	//	Are we on one of the grab handles?
	return Translate(HitTest(Pos));
}

//==============================================================================
//
// 	Function Name:	CTMTracker::GetHandleMask()
//
// 	Description:	This function is overloaded to enable only the handles that
//					appear at the window corners
//
// 	Returns:		The appropriate handle mask
//
//	Notes:			None
//
//==============================================================================
UINT CTMTracker::GetHandleMask() const
{
	return (UINT)0x0F;
}

//==============================================================================
//
// 	Function Name:	CTMTracker::GetPosition()
//
// 	Description:	This function is called to get the current position of the
//					mouse within the client area of the attached window
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMTracker::GetPosition(LPPOINT lpPoint)
{
	//	Make sure we have an attached window
	if((m_hAttachment == 0) || !IsWindow(m_hAttachment))
		return FALSE;

	//	Get the current position of the cursor on the screen
	//	and convert to client coordinates
	GetCursorPos(lpPoint);
	ScreenToClient(m_hAttachment, lpPoint);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTMTracker::Move()
//
// 	Description:	This function is called to resize the tracker rectangle to
//					match the current size and position of the attached window
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTracker::Move()
{
	RECT rcWnd;

	if((m_hAttachment != 0) && IsWindow(m_hAttachment))
	{
		//	Get the client area rectangle for the attached window
		::GetClientRect(m_hAttachment, &rcWnd);

		//	Set the position of the tracker to appear on the border
		m_rect.left   = rcWnd.left;
		m_rect.right  = rcWnd.right;
		m_rect.top    = rcWnd.top;
		m_rect.bottom = rcWnd.bottom;
	}
	else
	{
		m_rect.left = 0;
		m_rect.right = 0;
		m_rect.top = 0;
		m_rect.bottom = 0;
	}
}

//==============================================================================
//
// 	Function Name:	CTMTracker::SetCursor()
//
// 	Description:	This function is called to set the cursor based on the
//					current position of the mouse
//
// 	Returns:		TRUE if the mouse is on one of the grab handles
//
//	Notes:			None
//
//==============================================================================
BOOL CTMTracker::SetCursor(CWnd* pWnd, UINT uHitTest)
{
	//	Are we on one of the grab handles?
	switch(GetHandle())
	{
		case TMTRACKER_GH_RIGHT:
		case TMTRACKER_GH_LEFT:
		case TMTRACKER_GH_TOP:
		case TMTRACKER_GH_BOTTOM:
		case TMTRACKER_GH_BOTTOMRIGHT:
		case TMTRACKER_GH_BOTTOMLEFT:
		case TMTRACKER_GH_TOPRIGHT:
		case TMTRACKER_GH_TOPLEFT:

			//	Load the appropriate cursor			
			return CRectTracker::SetCursor(pWnd, uHitTest);
		
		default:
		
			return FALSE;
	} 
}

//==============================================================================
//
// 	Function Name:	CTMTracker::Track()
//
// 	Description:	This function is called to start a tracking move
//
// 	Returns:		TRUE if the user adjusted the rectangle
//
//	Notes:			None
//
//==============================================================================
BOOL CTMTracker::Track(LPRECT lpRect)
{
	CPoint Pos;

	ASSERT(lpRect);

	//	Get the current position of the mouse
	if(!GetPosition(&Pos))
		return FALSE;

	//	Make sure we are on one of the grab handles
	if(GetHandle() == TMTRACKER_GH_NONE)
		return FALSE;

	//	Allow the user to adjust the rectangle
	if(CRectTracker::Track(CWnd::FromHandle(m_hAttachment), Pos, FALSE,
						   CWnd::FromHandle(GetDesktopWindow())))
	{
		//	Get the new rectangle
		if(lpRect != 0)
			GetTrueRect(lpRect);

		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CTMTracker::Translate()
//
// 	Description:	This function is called to translate the system handle
//					identifier to the associated TMTracker identifier
//
// 	Returns:		One of the defined grab handle identifiers (tmtrack.h)
//
//	Notes:			None
//
//==============================================================================
int CTMTracker::Translate(int iHandle)
{
	switch(iHandle)
	{
		case hitRight:			return TMTRACKER_GH_RIGHT;
		case hitLeft:			return TMTRACKER_GH_LEFT;
		case hitTop:			return TMTRACKER_GH_TOP;
		case hitBottom:			return TMTRACKER_GH_BOTTOM;
		case hitBottomRight:	return TMTRACKER_GH_BOTTOMRIGHT;
		case hitBottomLeft:		return TMTRACKER_GH_BOTTOMLEFT;
		case hitTopRight:		return TMTRACKER_GH_TOPRIGHT;
		case hitTopLeft:		return TMTRACKER_GH_TOPLEFT;
		default:				return TMTRACKER_GH_NONE;
	} 
}





