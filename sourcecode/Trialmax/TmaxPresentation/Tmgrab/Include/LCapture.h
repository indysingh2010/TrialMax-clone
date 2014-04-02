//==============================================================================
//
// File Name:	lcapture.h
//
// Description:	This file contains the declaration of the LCaptureBitmap and
//				LCaptureCtrl classes.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	05-24-01	1.00		Original Release
//==============================================================================
#if !defined(__LCAPTURE_H__)
#define __LCAPTURE_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <Classlib\ltwrappr.h> // LeadTools Class Library

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	These definitions were extracted from LeadTools files
#define SUCCESS_ABORT				2   /** Function successful. You can quit now. **/
#define SUCCESS						1   /** Function successful        **/
#define FAILURE						0   /** Function not successful    **/

typedef enum
{
   SUCCESS_DLG_OK             = 100,   // The "OK" button was pressed, and the dialog exited successfully
   SUCCESS_DLG_CANCEL         = 101,   // The "Cancel" button was pressed, and the dialog exited successfully.
   SUCCESS_DLG_CLOSE          = 102,   // The "Close" button was pressed, and the dialog exited successfully.
   SUCCESS_DLG_EXIT           = 103,   // The dialog exits successfully after selecting exit option from menu or by closing the window.
   SUCCESS_DLG_EXPORTANDEXIT  = 104,   // The dialog exits successfully after selecting exit and export option, dialog parameters will have the resulting bitmap allocated.
} L_ERROR_DLG_SUCCESS;

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CFrame; //Forward declaration

class LCaptureBitmap : public LBitmapBase
{
	private:

						LEAD_DECLAREOBJECT(LCaptureBitmap);
						LEAD_DECLARE_CLASS_MAP();

	public:
	
						LCaptureBitmap();
		virtual		   ~LCaptureBitmap();
		
};

class LCaptureCtrl : public LScreenCapture
{
	private:
						LEAD_DECLAREOBJECT(LCaptureCtrl);
						
		CFrame*			m_pFrameWnd;
		long			m_lCaptures;

	public:
	
						LCaptureCtrl();
		virtual		   ~LCaptureCtrl();
		
		BOOL			SetHotKey(L_INT iKey, L_INT iModifier = 0);
		BOOL			SetCancelKey(L_INT iKey);
		
		BOOL			Initialize(CFrame* m_pOwner);
		
		virtual L_INT	ScreenCaptureCallBack();
};

#endif // !defined(__LCAPTURE_H__)
