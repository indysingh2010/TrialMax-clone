//==============================================================================
//
// File Name:	progbar.cpp
//
// Description:	This file contains member functions of the CProgressBar class
//
// See Also:	progbar.h
//
// Copyright 
//
//==============================================================================
//	Date		Revision    Description
//	06-01-2001	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <progbar.h>
#include <gdihelp.h>

//-----------------------------------------------------------------------------
//	DEFINES
//-----------------------------------------------------------------------------
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

//-----------------------------------------------------------------------------
//	GLOBALS
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
//	MAPS
//-----------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CProgressBar, CProgressCtrl)
	//{{AFX_MSG_MAP(CProgressBar)
	ON_WM_ERASEBKGND()
	ON_WM_PAINT()
	ON_MESSAGE(PBM_SETBARCOLOR, OnSetBarColor)
	ON_MESSAGE(PBM_SETBKCOLOR, OnSetBkColor)
	ON_MESSAGE(PBM_SETPOS, OnSetPos)
	ON_MESSAGE(PBM_DELTAPOS, OnDeltaPos)
	ON_MESSAGE(PBM_STEPIT, OnStepIt)
	ON_MESSAGE(PBM_SETSTEP, OnSetStep)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//------------------------------------------------------------------------------
//
// 	Function Name:	CProgressBar::CProgressBar()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Constructor		
//
//------------------------------------------------------------------------------
CProgressBar::CProgressBar() : m_rcBorders(0,0,0,0)
{
	m_pbrBackground		= 0; 
	m_pbrBar			= 0;
	m_iStep				= 10;	// According to MSDN
	m_iTail				= 0;
	m_iTailSize			= 40;
	m_crBackground		= ::GetSysColor(COLOR_3DFACE);
	m_crBarText			= ::GetSysColor(COLOR_CAPTIONTEXT);
	m_crBackgroundText	= ::GetSysColor(COLOR_BTNTEXT);
	SetGradientColors(::GetSysColor(COLOR_ACTIVECAPTION), 
					  ::GetSysColor(COLOR_ACTIVECAPTION));
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CProgressBar::~CProgressBar()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Destructor		
//
//------------------------------------------------------------------------------
CProgressBar::~CProgressBar()
{
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CProgressBar::DrawClippedText()
//
//	Parameters:		rParams - reference to drawing parameters structure
//					rClip   - reference to clipping rectangle
//					rText   - reference to text string
//					rOrigin	- reference to point used as window origin
//
// 	Return Value:	None
//
// 	Description:	This function will draw the text and clip it to the
//					bounds defined by the clipping rectangle		
//
//------------------------------------------------------------------------------
void CProgressBar::DrawClippedText(const SPBDrawParams& rParams, 
								   const CRect& rClip, CString& rText, 
								   const CPoint& rOrigin)
{
	CDC*	pdc = rParams.pdc;
	CRgn	rgn;
	CRect	rcDraw = GetDrawRect(rParams, rClip);

	rcDraw.OffsetRect(-rOrigin);
	rgn.CreateRectRgn(rcDraw.left, rcDraw.top, rcDraw.right, rcDraw.bottom);
	pdc->SelectClipRgn(&rgn);
	pdc->TextOut(0, 0, rText);
	rgn.DeleteObject();
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CProgressBar::DrawGradient()
//
//	Parameters:		rParams - reference to drawing parameters structure
//					rGrad   - reference to gradient bounding rectangle
//					rClip   - reference to clipping rectangle
//					crStart - first color in the gradient
//					crEnd   - last color in the gradient
//
// 	Return Value:	None
//
// 	Description:	This function will draw the gradient bar within the 
//					specified rectangle.
//
//------------------------------------------------------------------------------
void CProgressBar::DrawGradient(const SPBDrawParams& rParams,const CRect& rGrad,
								const CRect& rClip, COLORREF crStart, 
								COLORREF crEnd)
{
	// Split colors to RGB channel, find channel with maximum difference 
	// between the start and end colors. This distance will determine 
	// number of steps of gradient
	int r = (GetRValue(crEnd) - GetRValue(crStart));
	int g = (GetGValue(crEnd) - GetGValue(crStart));
	int b = (GetBValue(crEnd) - GetBValue(crStart));
	int iSteps = max(abs(r), max(abs(g), abs(b)));
	
	// if number of pixels in gradient less than number of steps - 
	// use it as numberof steps
	int iPixels = rGrad.Width();
	iSteps = min(iPixels, iSteps);
	if(iSteps == 0) iSteps = 1;

	float fRStep = (float)r/iSteps;
	float fGStep = (float)g/iSteps;
	float fBStep = (float)b/iSteps;

	r = GetRValue(crStart);
	g = GetGValue(crStart);
	b = GetBValue(crStart);

	BOOL bLowColor = rParams.pdc->GetDeviceCaps(RASTERCAPS) & RC_PALETTE;
	if(!bLowColor && iSteps > 1)
		if(rParams.pdc->GetDeviceCaps(BITSPIXEL)*rParams.pdc->GetDeviceCaps(PLANES) < 8)
			iSteps = 1; // for 16 colors no gradient

	float fWidthPerStep = (float)rGrad.Width() / (float)iSteps;
	CRect rcFill(rGrad);
	CBrush br;
	
	// Start filling
	for(int i = 0; i < iSteps; i++) 
	{
		rcFill.left  = rGrad.left + (int)(fWidthPerStep * i);
		rcFill.right = rGrad.left + (int)(fWidthPerStep * (i+1));
		if(i == iSteps-1)	//last step (because of problems with float)
			rcFill.right = rGrad.right;

		if(rcFill.right < rClip.left)
			continue; // skip - band before cliping rect
		
		// clip it
		if(rcFill.left < rClip.left)
			rcFill.left = rClip.left;
		if(rcFill.right > rClip.right)
			rcFill.right = rClip.right;

		COLORREF clrFill = RGB(r + (int)(i * fRStep),
		                       g + (int)(i * fGStep),
		                       b + (int)(i * fBStep));
		if(bLowColor)
		{
			br.CreateSolidBrush(clrFill);
			// CDC::FillSolidRect is faster, but it does not handle 8-bit color depth
			rParams.pdc->FillRect(&GetDrawRect(rParams, rcFill), &br);
			br.DeleteObject();
		}
		else
		{
			rParams.pdc->FillSolidRect(&GetDrawRect(rParams, rcFill), clrFill);
		}
		if(rcFill.right >= rClip.right)
			break; // stop filling if we reach current position
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CProgressBar::DrawMultiGradient()
//
//	Parameters:		rParams - reference to drawing parameters structure
//					rGrad   - reference to gradient bounding rectangle
//					rClip   - reference to clipping rectangle
//
// 	Return Value:	None
//
// 	Description:	This function will draw the gradient bar within the 
//					specified rectangle using colors stored in the gradient
//					array.
//
//------------------------------------------------------------------------------
void CProgressBar::DrawMultiGradient(const SPBDrawParams& rParams, 
									 const CRect& rGrad, const CRect &rClip)
{
	int		iSteps = m_aGradColors.GetSize()-1; ASSERT(iSteps != 0);
	float	iWidthPerStep = (float)rGrad.Width() / iSteps;
	CRect	rGradBand(rGrad);

	for(int i = 0; i < iSteps; i++) 
	{
		rGradBand.left = rGrad.left + (int)(iWidthPerStep * i);
		rGradBand.right = rGrad.left + (int)(iWidthPerStep * (i+1));
		if(i == iSteps-1)	//last step (because of problems with float)
			rGradBand.right = rGrad.right;

		if(rGradBand.right < rClip.left)
			continue; // skip - band before cliping rect
		
		CRect rClipBand(rGradBand);
		if(rClipBand.left < rClip.left)
			rClipBand.left = rClip.left;
		if(rClipBand.right > rClip.right)
			rClipBand.right = rClip.right;

		DrawGradient(rParams, rGradBand, rClipBand, 
					 m_aGradColors[i], m_aGradColors[i+1]);

		if(rClipBand.right == rClip.right)
			break; // stop filling - next band is out of clipping rect
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CProgressBar::DrawText()
//
//	Parameters:		rParams - reference to drawing parameters structure
//					rMax    - reference to maximum bounding rectangle
//					rBar    - reference to progress indicator bar rectangle
//
// 	Return Value:	None
//
// 	Description:	This function will draw the text for the progress bar.
//
//------------------------------------------------------------------------------
void CProgressBar::DrawText(const SPBDrawParams& rParams, const CRect& rMax, 
							const CRect &rBar)
{
	BOOL	bVertical = rParams.dwStyle&PBS_VERTICAL;
	CDC*	pdc = rParams.pdc;
	int		iValue = 0;
	CString strFormat;
	CString	strText;
	LONG	lGrad = 0;

	//	Don't bother if not displaying text
	if(!(rParams.dwStyle & PBS_TEXTMASK))
		return;

	//	Get the format specifier for the text string
	GetWindowText(strFormat);

	//	What text is supposed to be displayed?
	switch(rParams.dwStyle & PBS_TEXTMASK)
	{
		case PBS_SHOW_PERCENT:

			if(strFormat.IsEmpty())
				strFormat = "%d%%";
			// retrieve current position and range
			iValue = (int)((float)(rParams.iCurPos-rParams.iLower) * 100 / (rParams.iUpper-rParams.iLower));

			break;

		case PBS_SHOW_POSITION:

			if(strFormat.IsEmpty())
				strFormat = "%d";
			// retrieve current position
			iValue = rParams.iCurPos;
			break;
	}

	if(strFormat.IsEmpty())
		return;

	//	Set the text properties
	CFont* pFont = GetFont();
	CGHSelFont sf(pdc, pFont);
	CGHSelTextColor tc(pdc, m_crBarText);
	CGHSelBkMode bm(pdc, TRANSPARENT);
	CGHSelTextAlign	ta(pdc, TA_BOTTOM | TA_CENTER);
  
	CPoint ptOrg = pdc->GetWindowOrg();

	strText.Format(strFormat, iValue);
	
	if(pFont)
	{
		LOGFONT lf;
		pFont->GetLogFont(&lf);
		lGrad = lf.lfEscapement / 10;
	}

	int x = 0, y = 0, dx = 0, dy = 0;
	CSize sizText = pdc->GetTextExtent(strText);
	if(lGrad == 0)         {	x = sizText.cx; y = sizText.cy; dx = 0; dy = sizText.cy;}
	else if(lGrad == 90)   {	x = sizText.cy; y = sizText.cx; dx = sizText.cy; dy = 0;}
	else if(lGrad == 180)  {	x = sizText.cx; y = sizText.cy; dx = 0; dy = -sizText.cy;}
	else if(lGrad == 270)  {	x = sizText.cy; y = sizText.cx; dx = -sizText.cy; dy = 0;}
	else ASSERT(0); // angle not supported

	CPoint pt = pdc->GetViewportOrg();
	if(rParams.dwStyle & PBS_TIED_TEXT)
	{
		CRect rcFill(GetDrawRect(rParams, rBar));
		if((bVertical ? y : x) <= rBar.Width())
		{
			pdc->SetViewportOrg(rcFill.left + (rcFill.Width() + dx)/2, 
											   rcFill.top + (rcFill.Height() + dy)/2);
			DrawClippedText(rParams, rBar, strText, ptOrg);
		}
	}
	else
	{
		pdc->SetViewportOrg(rParams.rcClient.left + (rParams.rcClient.Width() + dx)/2, 
												rParams.rcClient.top + (rParams.rcClient.Height() + dy)/2);
		if(m_crBarText == m_crBackgroundText)
			// if the same color for bar and background draw text once
			DrawClippedText(rParams, rMax, strText, ptOrg);
		else
		{	
			// else, draw clipped parts of text
			
			// draw text on gradient
			if(rBar.left != rBar.right)
				DrawClippedText(rParams, rBar, strText, ptOrg);

			// draw text out of gradient
			if(rMax.right > rBar.right)
			{
				tc.Select(m_crBackgroundText);
				CRect rc(rMax);
				rc.left = rBar.right;
				DrawClippedText(rParams, rc, strText, ptOrg);
			}
			if(rMax.left < rBar.left)
			{
				tc.Select(m_crBackgroundText);
				CRect rc(rMax);
				rc.right = rBar.left;
				DrawClippedText(rParams, rc, strText, ptOrg);
			}
		}
	}
	pdc->SetViewportOrg(pt);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CProgressBar::GetDrawRect()
//
//	Parameters:		rParams - reference to drawing parameters structure
//					rBar    - reference to raw drawing rectangle
//
// 	Return Value:	Actual rectangle used to draw the bar
//
// 	Description:	This function will calculate the rectangle required for
//					drawing the indicator bar within the client area of the
//					progress bar window.		
//
//------------------------------------------------------------------------------
CRect CProgressBar::GetDrawRect(const SPBDrawParams& rParams, const CRect& rcBar)
{
	BOOL	bReverse = rParams.dwStyle & PBS_REVERSE;
	CRect	rcDraw(rParams.rcClient);

	//	Is this a vertical bar?
	if(rParams.dwStyle & PBS_VERTICAL)
	{
		//	Is this a reverse bar?
		if(rParams.dwStyle & PBS_REVERSE)
			rcDraw.top = rParams.rcClient.top + rcBar.left;
		else 
			rcDraw.top = rParams.rcClient.top + (rParams.rcClient.Height() - rcBar.right);
		
		rcDraw.bottom = rcDraw.top + rcBar.Width();
	}
	else
	{
		//	Is this a reverse bar?
		if(rParams.dwStyle & PBS_REVERSE)
			rcDraw.left = rParams.rcClient.left + (rParams.rcClient.Width() - rcBar.right);
		else
			rcDraw.left = rcBar.left;

		rcDraw.right = rcDraw.left + rcBar.Width();
	}
	
	return rcDraw;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CProgressBar::GetMinHeight()
//
//	Parameters:		None
//
// 	Return Value:	The minimum height in pixels
//
// 	Description:	This function is called to get the minimum height required
//					to display the progress bar without clipping the text.	
//
//------------------------------------------------------------------------------
int CProgressBar::GetMinHeight()
{
	CDC*		pdc;
	TEXTMETRIC	tm;

	//	Are we displaying text?
	if(GetStyle() & PBS_TEXTMASK)
	{
		if((pdc = GetDC()) != 0)
		{
			if(pdc->GetTextMetrics(&tm))
			{
				return (tm.tmHeight + m_rcBorders.Height());
			}
		}
		
		//	Must not have been able to get font information
		return m_rcBorders.Height();
	}
	else
	{
		return m_rcBorders.Height();
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CProgressBar::OnDeltaPos()
//
//	Parameters:		iIncrement - the change in position
//
// 	Return Value:	None
//
// 	Description:	This function handles all PBM_DELTAPOS messages		
//
//------------------------------------------------------------------------------
LRESULT CProgressBar::OnDeltaPos(WPARAM iIncrement, LPARAM)
{
	int iOldPos;
	if(SetSnakePos(iOldPos, iIncrement, TRUE))
		return iOldPos;
	else
		return Default();
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CProgressBar::OnEraseBkgnd()
//
//	Parameters:		pdc - pointer to the window's device context
//
// 	Return Value:	TRUE to force background erase
//
// 	Description:	This function handles all WM_ERASEBKGND messages		
//
//------------------------------------------------------------------------------
BOOL CProgressBar::OnEraseBkgnd(CDC* pdc) 
{
	return TRUE; // erase in OnPaint()
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CProgressBar::OnPaint()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	This function handles all WM_PAINT messages		
//
//------------------------------------------------------------------------------
void CProgressBar::OnPaint() 
{
	CPaintDC dc(this);

	SPBDrawParams Params;
	GetClientRect(&Params.rcClient);

	//	Get the current position and range
	Params.iCurPos = GetPos();
	GetRange(Params.iLower, Params.iUpper);
	
	//	Create a memory dc for drawing
	CGHMemDC memDC(&dc);
	Params.pdc = &memDC;
	
	//	Fill the background
	if(m_pbrBackground)
		memDC.FillRect(&Params.rcClient, m_pbrBackground);
	else
		memDC.FillSolidRect(&Params.rcClient, m_crBackground);

	//	Adjust for the borders
	Params.rcClient.DeflateRect(m_rcBorders);
		
	//	Stop here if current position is out of range
	if(Params.iCurPos < Params.iLower || Params.iCurPos > Params.iUpper)
		return;

	//	Get the styles
	Params.dwStyle = GetStyle();
	BOOL bVertical = Params.dwStyle & PBS_VERTICAL;
	BOOL bSnake    = Params.dwStyle & PBS_SNAKE;
	BOOL bRubber   = Params.dwStyle & PBS_RUBBER_BAR;

	//	Calculate the width of the visible gradient
	CRect rcBar(0,0,0,0);
	CRect rcMax(0,0,0,0);
	rcMax.right = bVertical ? Params.rcClient.Height() : Params.rcClient.Width();
	rcBar.right = (int)((float)(Params.iCurPos-Params.iLower) * rcMax.right / (Params.iUpper-Params.iLower));
	if(bSnake)
		rcBar.left = (int)((float)(m_iTail-Params.iLower) * rcMax.right / (Params.iUpper-Params.iLower));
	
	//	Draw the bar
	if(m_pbrBar)
		memDC.FillRect(&GetDrawRect(Params, rcBar), m_pbrBar);
	else
		DrawMultiGradient(Params, bRubber ? rcBar : rcMax, rcBar);

	//	Draw the text
	DrawText(Params, rcMax, rcBar);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CProgressBar::OnSetBarColor()
//
//	Parameters:		crEnd - gradient end color
//					crStart - gradient start color
//
// 	Return Value:	None
//
// 	Description:	This function handles all PBM_SETBARCOLOR messages		
//
//------------------------------------------------------------------------------
LRESULT CProgressBar::OnSetBarColor(WPARAM crEnd, LPARAM crStart)
{
	SetGradientColors(crStart, crEnd ? crEnd : crStart);
	return CLR_DEFAULT;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CProgressBar::OnSetBkColor()
//
//	Parameters:		crColor - background color
//
// 	Return Value:	None
//
// 	Description:	This function handles all PBM_SETBKCOLOR messages		
//
//------------------------------------------------------------------------------
LRESULT CProgressBar::OnSetBkColor(WPARAM, LPARAM crColor)
{
	m_crBackground = crColor;
	return CLR_DEFAULT;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CProgressBar::OnSetPos()
//
//	Parameters:		iPos - new position
//
// 	Return Value:	None
//
// 	Description:	This function handles all PBM_SETPOS messages		
//
//------------------------------------------------------------------------------
LRESULT CProgressBar::OnSetPos(WPARAM iPos, LPARAM)
{
	int iOldPos;
	if(SetSnakePos(iOldPos, iPos))
		return iOldPos;
	else
		return Default();
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CProgressBar::OnSetStep()
//
//	Parameters:		iStep - new step increment
//
// 	Return Value:	None
//
// 	Description:	This function handles all PBM_SETSTEP messages		
//
//------------------------------------------------------------------------------
LRESULT CProgressBar::OnSetStep(WPARAM iStep, LPARAM)
{
	m_iStep = iStep;
	return Default();
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CProgressBar::OnStepIt()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	This function handles all PBM_STEPIT messages		
//
//------------------------------------------------------------------------------
LRESULT CProgressBar::OnStepIt(WPARAM, LPARAM)
{
	int iOldPos;
	if(SetSnakePos(iOldPos, m_iStep, TRUE))
		return iOldPos;
	else
		return Default();
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CProgressBar::SetSnakePos()
//
//	Parameters:		rOldPos    - reference to old position
//					iNew       - new position
//					bIncrement - TRUE to increment the position
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function will set the position of a snake style
//					progress bar.	
//
//------------------------------------------------------------------------------
BOOL CProgressBar::SetSnakePos(int& rOldPos, int iNew, BOOL bIncrement)
{
	DWORD	dwStyle = GetStyle();
	int		iLower;
	int		iUpper;
	int		iCurPos;
			
	//	Is this a snake bar?
	if(!(dwStyle & PBS_SNAKE))
		return FALSE;
	
	GetRange(iLower, iUpper);
	if(bIncrement)
	{
		iCurPos = GetPos();
		if(iCurPos == iUpper && iCurPos - m_iTail < m_iTailSize )
			iCurPos = m_iTail + m_iTailSize;
		iNew = iCurPos + abs(iNew);
	}
	if(iNew > iUpper+m_iTailSize)
	{
		iNew -= iUpper-iLower + m_iTailSize;
		if(iNew > iUpper + m_iTailSize)
		{
			ASSERT(0); // too far - reset
			iNew = iUpper + m_iTailSize;
		}
		if(dwStyle&PBS_REVERSE)
			ModifyStyle(PBS_REVERSE, 0);
		else
			ModifyStyle(0, PBS_REVERSE);
	}
	else if(iNew >= iUpper)
		Invalidate();
	
	m_iTail = iNew - m_iTailSize;
	if(m_iTail < iLower)
		m_iTail = iLower;

	rOldPos = DefWindowProc(PBM_SETPOS, iNew, 0);
	return TRUE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CProgressBar::SetTextFormat()
//
//	Parameters:		lpFormat - pointer to format string
//					dwFlags  - packed format flags
//
// 	Return Value:	None
//
// 	Description:	This function is called to set the text format to be
//					displayed in the progress bar.
//
//------------------------------------------------------------------------------
void CProgressBar::SetTextFormat(LPCTSTR lpFormat, DWORD dwFlags)
{
	ASSERT(::IsWindow(m_hWnd));
	
	if(!lpFormat || !lpFormat[0] || !dwFlags)
	{
		ModifyStyle(PBS_TEXTMASK, 0);
		SetWindowText("");
	}
	else
	{
		ModifyStyle(PBS_TEXTMASK, dwFlags);
		SetWindowText(lpFormat);
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CProgressBar::SetGradientColorsEx()
//
//	Parameters:		iCount  - number of colors in the list
//					crFirst - first color to put in the array
//					crNext  - next color to put in the array
//					...		- remaining colors to put in the array
//
// 	Return Value:	None
//
// 	Description:	This function is called to populate the array of colors 
//					use for multicolor gradients.
//
//------------------------------------------------------------------------------
void CProgressBar::SetGradientColorsEx(int iCount, COLORREF crFirst, 
									   COLORREF crNext, ...)
{ 
	m_aGradColors.SetSize(iCount); 
	
	m_aGradColors.SetAt(0, crFirst); 
	m_aGradColors.SetAt(1, crNext);  

	va_list pArgs;
	va_start(pArgs, crNext);
	for(int i = 2; i < iCount; i++)
		m_aGradColors.SetAt(i, va_arg(pArgs, COLORREF));
	va_end(pArgs);
}
