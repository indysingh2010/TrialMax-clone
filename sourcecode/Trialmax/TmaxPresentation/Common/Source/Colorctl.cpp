//==============================================================================
//
// File Name:	colorctl.cpp
//
// Description:	This file contains member functions of the CColorPushbutton 
//				and CColorStatic classes.
//
// See Also:	color.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	01-31-97	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <colorctl.h>

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
BEGIN_MESSAGE_MAP(CColorPushbutton, CButton)
	//{{AFX_MSG_MAP(CColorPushbutton)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

BEGIN_MESSAGE_MAP(CColorStatic, CStatic)
	//{{AFX_MSG_MAP(CColorStatic)
	ON_WM_CTLCOLOR_REFLECT()
	ON_WM_TIMER()
	ON_WM_DESTROY()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CColorPushbutton::CColorPushbutton()
//
// 	Description:	This is the constructor for CColorPushbutton objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CColorPushbutton::CColorPushbutton()
{
	m_crColor = RGB(0,0,0);
}

//==============================================================================
//
// 	Function Name:	CColorPushbutton::~CColorPushbutton()
//
// 	Description:	This is the destructor for CColorPushbutton objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CColorPushbutton::~CColorPushbutton()
{
}

//==============================================================================
//
// 	Function Name:	CColorPushbutton::DrawItem()
//
// 	Description:	This function is called to draw the button
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CColorPushbutton::DrawItem(LPDRAWITEMSTRUCT lpDrawItemStruct) 
{
	CDC*	pDC;
	CRect	rcButton;
	CBrush	brBrush;

	// Get the device context from the context
	if((pDC = CDC::FromHandle(lpDrawItemStruct->hDC)) == 0)
		return;
	
	switch(lpDrawItemStruct->itemAction)
	{
		case ODA_SELECT:
		case ODA_DRAWENTIRE:
			
			brBrush.CreateSolidBrush(m_crColor);

			rcButton = lpDrawItemStruct->rcItem;
			rcButton.InflateRect(-2, -2);
			
			if(lpDrawItemStruct->itemState & ODS_SELECTED)
			{
				pDC->Draw3dRect(rcButton, RGB_BUTTON_DARK, RGB_BUTTON_WHITE);
				rcButton.InflateRect(-1, -1);
				pDC->Draw3dRect(rcButton, RGB_BUTTON_DARK, RGB_BUTTON_WHITE);
				rcButton.InflateRect(-1, -1);
			}
			else
			{
				pDC->Draw3dRect(rcButton, RGB_BUTTON_WHITE, RGB_BUTTON_DARK);
				rcButton.InflateRect(-1, -1);
				pDC->Draw3dRect(rcButton, RGB_BUTTON_WHITE, RGB_BUTTON_DARK);
				rcButton.InflateRect(-1, -1);
			}

			// Then the button face in the correct color
			pDC->FillRect(&rcButton, &brBrush);
			break;

		case ODA_FOCUS:
			break;
	}
}

//==============================================================================
//
// 	Function Name:	CColorPushbutton::SetColor()
//
// 	Description:	This function is called to set the button color
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CColorPushbutton::SetColor(COLORREF crColor)
{
	m_crColor = crColor;
}

//==============================================================================
//
// 	Function Name:	CColorStatic::CColorStatic()
//
// 	Description:	This is the constructor for CColorStatic objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CColorStatic::CColorStatic()
{
	m_pParent = 0;
	m_nMsg = WM_USER;
	m_bTextBlink = FALSE;
	m_bTextBlinkState = FALSE;
	m_bBkBlink = FALSE;
	m_bBkBlinkState = FALSE;
	m_nTimerId = 0;
	m_crTextColor = ::GetSysColor(COLOR_BTNTEXT);
	m_crBlinkTextColors[0] = m_crTextColor;
	m_crBlinkTextColors[1] = m_crTextColor;
	m_crBkColor = ::GetSysColor(COLOR_BTNFACE);
	m_crBlinkBkColors[0] = m_crBkColor;
	m_crBlinkBkColors[1] = m_crBkColor;
	m_brBkgnd.CreateSolidBrush(m_crBkColor);
	m_brBlinkBkgnd[0].CreateSolidBrush(m_crBkColor);
	m_brBlinkBkgnd[1].CreateSolidBrush(m_crBkColor);
} 

//==============================================================================
//
// 	Function Name:	CColorStatic::~CColorStatic()
//
// 	Description:	This is the destructor for CColorStatic objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CColorStatic::~CColorStatic()
{
} 

//==============================================================================
//
// 	Function Name:	CColorStatic::CtlColor()
//
// 	Description:	This function handles reflected WM_CTLCOLOR messages
//
// 	Returns:		A handle to the brush to use for painting
//
//	Notes:			None
//
//==============================================================================
HBRUSH CColorStatic::CtlColor(CDC* pDC, UINT nCtlColor) 
{
	// Set foreground color
	if(m_bTextBlink)
		pDC->SetTextColor(m_crBlinkTextColors[m_bTextBlinkState]);
	else
		pDC->SetTextColor(m_crTextColor);

	// Set background color & brush
	if(m_bBkBlink)
	{
		pDC->SetBkColor(m_crBlinkBkColors[m_bBkBlinkState]);
		return (HBRUSH)m_brBlinkBkgnd[m_bBkBlinkState];
	}
	else
	{
		pDC->SetBkColor(m_crBkColor);
		return (HBRUSH)m_brBkgnd;
	}
} 

//==============================================================================
//
// 	Function Name:	CColorStatic::EnableNotify()
//
// 	Description:	This function is called to enable or disable blink 
//					notification
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CColorStatic::EnableNotify(CWnd* pParent, UINT nMsg)
{
	m_pParent = pParent;
	m_nMsg = nMsg;
} 

//==============================================================================
//
// 	Function Name:	CColorStatic::GetBkColor()
//
// 	Description:	This function is called to retrieve the background color
//
// 	Returns:		The current background color
//
//	Notes:			None
//
//==============================================================================
COLORREF CColorStatic::GetBkColor()
{
	return m_crBkColor;
} 

//==============================================================================
//
// 	Function Name:	CColorStatic::GetTextColor()
//
// 	Description:	This function is called to retrieve the text color
//
// 	Returns:		The current text color
//
//	Notes:			None
//
//==============================================================================
COLORREF CColorStatic::GetTextColor()
{
	return m_crTextColor;
} 

//==============================================================================
//
// 	Function Name:	CColorStatic::OnDestroy()
//
// 	Description:	This function handles WM_DESTROY messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CColorStatic::OnDestroy() 
{
	//	Do the base class processing
	CStatic::OnDestroy();
	
	// Destroy timer
	if(m_nTimerId > 0) 
		KillTimer(m_nTimerId);

	// Destroy resources
    m_brBkgnd.DeleteObject();
    m_brBlinkBkgnd[0].DeleteObject();
    m_brBlinkBkgnd[1].DeleteObject();
} 

//==============================================================================
//
// 	Function Name:	CColorStatic::OnTimer()
//
// 	Description:	This function handles WM_TIMER messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CColorStatic::OnTimer(UINT nIDEvent) 
{
	if(nIDEvent == m_nTimerId)	
	{
		// If control is blinking (text) switch its color
		if(m_bTextBlink) m_bTextBlinkState = !m_bTextBlinkState;

		// If control is blinking (background) switch its color
		if(m_bBkBlink) m_bBkBlinkState = !m_bBkBlinkState;

		//	Redraw if we are blinking
		if(m_bBkBlink || m_bTextBlink) 
		{
			Invalidate();
			
			// Send notification message only on rising edge
			if(m_pParent && (m_bBkBlinkState || m_bTextBlinkState)) 
				m_pParent->PostMessage(m_nMsg, GetDlgCtrlID(), 0);
		}
	}
	else
	{
		CStatic::OnTimer(nIDEvent);
	}
} 

//==============================================================================
//
// 	Function Name:	CColorStatic::SetBkColor()
//
// 	Description:	This function will set the background color
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CColorStatic::SetBkColor(COLORREF crBkColor)
{
	// Set new background color
	if(crBkColor != 0xFFFFFFFF)
	{
		m_crBkColor = crBkColor;
	}
	else 
	{
		m_crBkColor = ::GetSysColor(COLOR_BTNFACE);
	}

    m_brBkgnd.DeleteObject();
    m_brBkgnd.CreateSolidBrush(m_crBkColor);

	// Repaint control
	Invalidate();
} 

//==============================================================================
//
// 	Function Name:	CColorStatic::SetBlinkBkColors()
//
// 	Description:	This function will set blink background colors
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CColorStatic::SetBlinkBkColors(COLORREF crColor1, COLORREF crColor2)
{
	// Set new blink background colors
	m_crBlinkBkColors[0] = crColor1;
	m_crBlinkBkColors[1] = crColor2;

    m_brBlinkBkgnd[0].DeleteObject();
    m_brBlinkBkgnd[0].CreateSolidBrush(m_crBlinkBkColors[0]);
    m_brBlinkBkgnd[1].DeleteObject();
    m_brBlinkBkgnd[1].CreateSolidBrush(m_crBlinkBkColors[1]);

	// Repaint control
	Invalidate();
} 

//==============================================================================
//
// 	Function Name:	CColorStatic::SetBlinkTextColors()
//
// 	Description:	This function will set blink text colors
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CColorStatic::SetBlinkTextColors(COLORREF crColor1, COLORREF crColor2)
{
	// Set new blink text colors
	m_crBlinkTextColors[0] = crColor1;
	m_crBlinkTextColors[1] = crColor2;
} 

//==============================================================================
//
// 	Function Name:	CColorStatic::SetTextColor()
//
// 	Description:	This function will set the text color
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CColorStatic::SetTextColor(COLORREF crTextColor)
{
	// Set new foreground color
	if(crTextColor != 0xFFFFFFFF)
	{
		m_crTextColor = crTextColor;
	}
	else 
	{
		m_crTextColor = ::GetSysColor(COLOR_BTNTEXT);
	}

	// Repaint control
	Invalidate();
}

//==============================================================================
//
// 	Function Name:	CColorStatic::SetBkBlink()
//
// 	Description:	This function will set background blink state
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CColorStatic::StartBkBlink(BOOL bStart, int iRate)
{
	// Destroy any previous timer
	if(m_nTimerId > 0)	
	{
		KillTimer(m_nTimerId);
		m_nTimerId = 0;
	}

	m_bBkBlink = bStart;
	m_bBkBlinkState = FALSE;

	//	Stop here if disabling blink
	if(!m_bBkBlink) 
		return;

	//	What rate?
	switch(iRate)
	{
		case COLORSTATIC_SLOW:

			m_nTimerId = SetTimer(1, CS_SLOW_PERIOD, 0);
			break;
		
		case COLORSTATIC_FAST:

			m_nTimerId = SetTimer(1, CS_FAST_PERIOD, 0);
			break;
		
		case COLORSTATIC_NORMAL:
		default:

			m_nTimerId = SetTimer(1, CS_NORMAL_PERIOD, 0);
			break;
	}

} 

//==============================================================================
//
// 	Function Name:	CColorStatic::SetTextBlink()
//
// 	Description:	This function will set background blink state
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CColorStatic::StartTextBlink(BOOL bStart, int iRate)
{
	// Destroy any previous timer
	if(m_nTimerId > 0)	
	{
		KillTimer(m_nTimerId);
		m_nTimerId = 0;
	}

	m_bTextBlink = bStart;
	m_bTextBlinkState = FALSE;

	//	Stop here if disabling blink
	if(!m_bTextBlink) 
		return;

	//	What rate?
	switch(iRate)
	{
		case COLORSTATIC_SLOW:

			m_nTimerId = SetTimer(1, CS_SLOW_PERIOD, 0);
			break;
		
		case COLORSTATIC_FAST:

			m_nTimerId = SetTimer(1, CS_FAST_PERIOD, 0);
			break;
		
		case COLORSTATIC_NORMAL:
		default:

			m_nTimerId = SetTimer(1, CS_NORMAL_PERIOD, 0);
			break;
	}

} 










