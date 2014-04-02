//==============================================================================
//
// File Name:	LCapture.cpp
//
// Description:	This file contains member functions of the LCaptureBitmap and
//				LCaptureCtrl classes.
//
// See Also:	LCapture.h
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	05-24-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <LCapture.h>
#include <Grabapp.h>
#include <Frame.h>

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
extern CTMGrabApp NEAR	theApp;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------

LEAD_START_CLASS_MAP(LCaptureBitmap,LBitmapBase)
   LEAD_INIT_LScreenCapture(LCaptureCtrl)
LEAD_END_CLASS_MAP(LCaptureBitmap,LBitmapBase)

LEAD_IMPLEMENTOBJECT(LCaptureBitmap);
LEAD_IMPLEMENTOBJECT(LCaptureCtrl);

//==============================================================================
//
// 	Function Name:	LCaptureBitmap::LCaptureBitmap()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
LCaptureBitmap::LCaptureBitmap() : LBitmapBase()
{
}

//==============================================================================
//
// 	Function Name:	LCaptureBitmap::~LCaptureBitmap()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
LCaptureBitmap::~LCaptureBitmap()
{
}

//==============================================================================
//
// 	Function Name:	LCaptureCtrl::LCaptureCtrl()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
LCaptureCtrl::LCaptureCtrl() : LScreenCapture()
{
	m_pFrameWnd = NULL;
	m_lCaptures = 0;
}

//==============================================================================
//
// 	Function Name:	LCaptureCtrl::~LCaptureCtrl()
//
// 	Description:	Destructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
LCaptureCtrl::~LCaptureCtrl()
{
}

//==============================================================================
//
// 	Function Name:	LCaptureCtrl::Initialize()
//
// 	Description:	Called to initialize the control
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL LCaptureCtrl::Initialize(CFrame* pOwner)
{
	m_pFrameWnd = pOwner;
	
	SetDefaultCaptureOptions();
	SetDefaultAreaOptions();
	SetDefaultObjectOptions();
	
	// Enable the callback functionality
	EnableCallBack(TRUE);
	
	EnableAutoErrorDisplay(FALSE);
	
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	LCaptureCtrl::ScreenCaptureCallBack()
//
// 	Description:	Called when the capture operation is complete
//
// 	Returns:		SUCCESS if successful
//
//	Notes:			None
//
//==============================================================================
L_INT LCaptureCtrl::ScreenCaptureCallBack()
{
	m_lCaptures += 1;
	
	if(m_pFrameWnd != NULL)
	{
	   if(LDictionary_IsBitmap(m_pBitmap))
	   {
		  if(m_pBitmap->IsAllocated())
		  {
				m_pFrameWnd->OnCaptureImage(m_pBitmap, m_lCaptures);
				return SUCCESS;
		  }
	   }
   
   }
   return FAILURE;
}

//==============================================================================
//
// 	Function Name:	LCaptureCtrl::SetCancelKey()
//
// 	Description:	Called to set the key used to cancel the operation
//
// 	Returns:		TRUE if successful
//
//==============================================================================
BOOL LCaptureCtrl::SetCancelKey(L_INT iKey)
{
	m_CaptureOptions.nCancelKey = iKey;
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	LCaptureCtrl::SetHotKey()
//
// 	Description:	Called to set the key used to start the operation
//
// 	Returns:		TRUE if successful
//
//==============================================================================
BOOL LCaptureCtrl::SetHotKey(L_INT iKey, L_INT iModifier)
{
	m_CaptureOptions.nHotKey = iKey;
	m_CaptureOptions.uHotKeyModifiers = iModifier;
	return TRUE;
}

