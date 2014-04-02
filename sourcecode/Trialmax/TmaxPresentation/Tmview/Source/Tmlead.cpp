//==============================================================================
//
// File Name:	tmlead.cpp
//
// Description:	This file contains member functions of the CTMLead class.
//
// See Also:	tmlead.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	07-11-97	1.00		Original Release
//	01-09-98	1.10		Added members for retrieving image width, height
//							and aspect ratio
//	01-09-98	1.10		Added Render() member
//	03-21-98	2.00		Added ability to pan the image
//	03-22-98	2.00		Added ZoomToFullHeight() and ZoomToFullWidth()
//							capabilities
//	03-22-98	2.00		Modified to support automatic zoom states on load
//	01-03-99	3.00		Added support for restricting zoom display to user
//							defined rectangle
//	01-29-2014	7.0.27		Added public methods GesturePan() and GestureZoom()
//	02-28-2014	7.0.29		Reset document zoom on double click
//	03-25-2014	7.0.31		Added public method ZoomToFactor() to enable zoomed
//                          page swipe
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmviewap.h>
#include <tmview.h>
#include <tmlead.h>
#include <annprops.h>
#include <math.h>
#include <callout.h>
#include <anntext.h>
#include <imgprop.h>
#include <cleanup.h>
#include <diagnose.h>
#include <pathsplit.h>
#include <ltimgcor.h>

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
//const WCHAR szLicense[] = L"LEADTOOLS OCX Copyright (c) 1990-2000 LEAD Technologies, Inc."; // version 12 
//const WCHAR szLicense[] = L"LEADTOOLS OCX Copyright (c) 1991-2001 LEAD Technologies, Inc."; // version 14 
const WCHAR szLicense[] = L"LEADTOOLS OCX Copyright (c) 1991-2008 LEAD Technologies, Inc."; 

//const char szSupportKey[] = "juHyeTgs";	// version 11
//const char szSupportKey[] = "hju78Lt7";	// version 12
//const char szSupportKey[] = "IxKjEexxS";	// version 14
const char szSupportKey[] = "vhG42tyuh9";	// version 16.5

//const char SZ_KEY_GIFLZW[] = "sg8Z2XkjL";
//const char SZ_KEY_TIFLZW[] = "gZWEhj9ZX2j";

extern CTMViewApp		theApp;
extern CTMViewCtrl*		_pControl;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CTMLead, CLead)
	//{{AFX_MSG_MAP(CTMLead)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CTMLead::GetAnnHandle()
//
// 	Description:	This callback is used to walk the annotation container
//					in search of the object with the tag specified by the user.
//
// 	Returns:		SUCCESS to continue iteration
//
//	Notes:			None
//
//==============================================================================
L_INT EXT_CALLBACK GetAnnHandle(HANNOBJECT hAnn, L_INT* pUser)
{
	SGetAnnHandle*	pParams = (SGetAnnHandle*)pUser;
	DWORD			dwTag;

	//	Is the parameter structure valid?
	if(!pParams)
		return 0;

	//	Get the tag for this object
	if(L_AnnGetTag(hAnn, (L_UINT32*)(&dwTag)) != SUCCESS)
	{
		pParams->hAnn = 0;
		return SUCCESS;	//	Continue the enumeration
	}
	else
	{
		if(pParams->dwTag == dwTag)
		{
			pParams->hAnn = hAnn;
			return 0;	//	Stop the enumeration
		}
		else
		{
			pParams->hAnn = 0;
			return SUCCESS;
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::RebuildAnnList()
//
// 	Description:	This callback is used to walk the annotation container
//					and add annotation objects to the list provided by the 
//					caller
//
// 	Returns:		SUCCESS to continue iteration
//
//	Notes:			None
//
//==============================================================================
L_INT EXT_CALLBACK RebuildAnnList(HANNOBJECT hAnn, L_INT* pUser)
{
	CAnnotations*	pAnns = (CAnnotations*)pUser;
	CAnnotation*	pAnn;
	DWORD			dwTag;

	//	Is the annotation list valid?
	if(!pAnns)
		return 0;

	//	Get the tag for this object
	if(L_AnnGetTag(hAnn, (L_UINT32*)(&dwTag)) == SUCCESS)
	{
		//	Allocate a new annotation object
		pAnn = new CAnnotation(dwTag);
		ASSERT(pAnn);

		L_AnnGetRect(hAnn, &(pAnn->m_rcAnn), NULL);
		
		if(pAnn->m_bIsLocked)
			L_AnnShowLockedIcon(hAnn, FALSE, 0);

		//	Add it to the caller's list
		pAnns->Add(pAnn);
	}

	return SUCCESS;	//	Continue the enumeration
}

//==============================================================================
//
// 	Function Name:	CTMLead::AnnGetText()
//
// 	Description:	This function is called to get the text associated with the
//					specified annotation.
//
// 	Returns:		TRUE if successful.
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLead::AnnGetText(HANNOBJECT hAnn, CString& rText)
{
	L_SIZE_T	uLength;
	char*		pText;

	//	Get the text length
	if(L_AnnGetTextLen(hAnn, &uLength) == SUCCESS)
	{
		//	Is there any text with this annotation?
		if(uLength > 0)
		{
			//	Allocate a buffer large enough to hold the text
			pText = new char[uLength + 1];
			ASSERT(pText);
			
			if(pText != 0)
			{
				//	Clear the buffer
				memset(pText, 0, uLength + 1);

				//	Get the annotation text
				if(L_AnnGetText(hAnn, pText, &uLength) == SUCCESS)
				{
					rText = pText;
					delete [] pText;
					return TRUE;
				}
				else
				{
					//	Unable to get the text
					delete [] pText;
					return FALSE;
				}
				
			}
			else
			{
				//	Unable to allocate text buffer
				return FALSE;
			}	
		}
		else
		{
			//	No text with this annotation
			rText.Empty();
			return TRUE;
		}
	}
	else
	{
		//	Unable to retrieve text length
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::BoarderRemove()
//
// 	Description:	This function is called to remove the borders from a 1 bit
//					image.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMLead::BorderRemove(long lBorderPercent, long lWhiteNoise, 
							long lVariance, long lLocation)
{
	if(!IsLoaded())
	{
		return TMV_NOIMAGE;
	}
	else
	{
		//	Set this property to make sure the borders get removed
		SetDocCleanSuccess(SUCCESS_REMOVE);

		if((m_sLeadError = CLead::BorderRemove(BORDER_USE_VARIANCE,
											   lLocation, lBorderPercent,
											   lWhiteNoise, lVariance)) != 0)
		{
			HandleLeadError(m_sLeadError);
			return TMV_LEADERROR;
		}
	
		//	Repaint the image
		CLead::ForceRepaint();

		return TMV_NOERROR;
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::ChangeAnn()
//
// 	Description:	This function is called by a callout when the size or 
//					position of one of its annotations changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::ChangeAnn(CCallout* pSource, HANNOBJECT hAnn)
{
	HANNOBJECT		hLocal;
	CAnnotation*	pLocal;
	CCallout*		pCallout;
	DWORD			dwTag;

	//	Don't bother doing anything if we are not synchronizing annotations
	if(!m_bSyncCalloutAnn)
		return;
 
	//	Get the tag assigned to this annotation
	if(L_AnnGetTag((void*)hAnn, (L_UINT32*)(&dwTag)) != SUCCESS)
		return;

	//	Locate this annotation in the local list
	if((pLocal = m_Annotations.Find(dwTag)) == 0)
		return;

	//	Temporarily remove this from the local list. This will keep us from
	//	destroying the annotation in all the callouts in OnAnnDestroy()
	m_Annotations.Remove(pLocal, FALSE);

	//	Delete this annotation
	DeleteAnn(dwTag);

	//	Now copy the annotation back into the local list
	if((hLocal = CopyAnn(pSource->GetTMLead(), hAnn)) != 0)
	{
		//	Update each callout linked to this annotation except the source
		pCallout = pLocal->First();
		while(pCallout)
		{
			//	Make sure this callout is still in the list
			if((pSource != pCallout) && m_pCallouts->Find(pCallout))
			{
				pCallout->DeleteAnn(dwTag);
				pCallout->CopyAnn(this, (void*)hLocal);
			}

			//	Get the next callout 
			pCallout = pLocal->Next();
		}
	}

	//	Put the object back in the local list
	m_Annotations.Add(pLocal);
}

//==============================================================================
//
// 	Function Name:	CTMLead::Cleanup()
//
// 	Description:	This function invokes a dialog box that allows the user
//					to clean up a scanned image.
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMLead::Cleanup(LPCTSTR lpszSaveAs)
{
	if(!IsLoaded())
	{
		return TMV_NOIMAGE;
	}
	else
	{
		CCleanup Clean(this, lpszSaveAs);

		//	Let the container know we are about to open the dialog box
		if(m_pControl)
			m_pControl->PreModalDialog();

		//	Open the dialog box
		Clean.DoModal();

		//	Let the container know we are about to close the dialog box
		if(m_pControl)
			m_pControl->PostModalDialog();

		return TMV_NOERROR;
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::ClearSelections()
//
// 	Description:	This function will clear any current annotation selections.
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMLead::ClearSelections()
{
	HANNOBJECT*	pSelections;
	UINT		uSelections;

	//	Reset the text edit reference
	OnEndEditTextAnn(FALSE);

	//	Get the array of selections
	if((uSelections = GetSelections(&pSelections)) == 0)
		return TMV_NOERROR;

	//	Clear all the selections
	for(UINT i = 0; i < uSelections; i++)
		L_AnnSetSelected(pSelections[i], FALSE, 0);
	
	HeapFree(GetProcessHeap(), 0, pSelections);

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMLead::ClientToSource()
//
// 	Description:	This function will convert the rectangle's coordinates from
//					client window coordinates to source image coordinates
//				
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::ClientToSource(ANNRECT* pAnnRect)
{
	ClientToBitmap((float)(pAnnRect->left), (float)(pAnnRect->top));
	pAnnRect->left = GetConvertX();
	pAnnRect->top  = GetConvertY();

	ClientToBitmap((float)(pAnnRect->right), (float)(pAnnRect->bottom));
	pAnnRect->right  = GetConvertX();
	pAnnRect->bottom = GetConvertY();
}

//==============================================================================
//
// 	Function Name:	CTMLead::ClientToSource()
//
// 	Description:	This function will convert the rectangle's coordinates from
//					client window coordinates to source image coordinates
//				
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::ClientToSource(RECT* pRect)
{
	ANNRECT rcSrc;

	rcSrc.left   = (double)(pRect->left);
	rcSrc.top    = (double)(pRect->top);
	rcSrc.right  = (double)(pRect->right);
	rcSrc.bottom = (double)(pRect->bottom);

	ClientToSource(&rcSrc);

	pRect->left   = ROUND(rcSrc.left);
	pRect->top    = ROUND(rcSrc.top);
	pRect->right  = ROUND(rcSrc.right);
	pRect->bottom = ROUND(rcSrc.bottom);

}

//==============================================================================
//
// 	Function Name:	CTMLead::Copy()
//
// 	Description:	This function will copy the current image to the clipboard
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMLead::Copy()
{
	//	Now copy the data
	if(CLead::Copy(COPY_EMPTY | COPY_DIB) != 0)
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_COPYFAILED);
		return TMV_COPYFAILED;
	}
	else
	{
		return TMV_NOERROR;
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::Copy()
//
// 	Description:	This function will copy this objects properties to the
//					object provided by the caller
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::Copy(CTMLead* pLead)
{
	//	Rotate the file by the full amount when we load it
	pLead->SetRotation(m_sAngle);
	pLead->SetFilename(m_strFilename, FALSE);
	
	//	Set the other properties
	pLead->SetScaleImage(m_bScaleImage);
	pLead->SetKeepAspect(m_bKeepAspect);
	pLead->SetHideScrollBars(m_bHideScrollBars);
	pLead->SetPanPercent((short)m_fPanPercent);
	pLead->SetRightClickPan(m_bRightClickPan);
	pLead->SetFitToImage(m_bFitToImage);
	pLead->SetZoomOnLoad(m_sZoomOnLoad);
	pLead->SetRotation(m_sRotation);
	pLead->SetBitonal(m_sBitonal);
	pLead->SetAction(m_sAction);
	pLead->SetCallFrameThickness(m_sCallFrameThick);
	pLead->SetCallFrameColor(m_sCallFrameColor);
	pLead->SetCallHandleColor(m_sCallHandleColor);
	pLead->SetCalloutColor(m_sCalloutColor);
	pLead->SetResizeCallouts(m_bResizeCallouts);
	pLead->SetPanCallouts(m_bPanCallouts);
	pLead->SetZoomCallouts(m_bZoomCallouts);
	pLead->SetCalloutShadeColor(m_crCalloutShadeBackground);
	pLead->SetShadeOnCallout(m_bShadeOnCallout);
	pLead->SetBackColor((OLE_COLOR)m_crBackground, m_crBackground); 
	pLead->SetAnnTool(m_sAnnTool);
	pLead->SetAnnThickness(m_sAnnThickness);
	pLead->SetAnnColor(m_sAnnColor);
	pLead->SetAnnColorDepth(m_sAnnColorDepth);
	pLead->SetRedactColor(m_sRedactColor);
	pLead->SetHighlightColor(m_sHighlightColor);
	pLead->SetQFactor(m_sQFactor);
	pLead->SetMaxZoom((short)m_fMaxZoom);
	pLead->SetSyncCalloutAnn(m_bSyncCalloutAnn);
	pLead->SetZoomToRect(m_bZoomToRect);
	pLead->SetAnnFontName(m_strAnnFontName);
	pLead->SetAnnFontSize(m_sAnnFontSize);
	pLead->SetAnnFontBold(m_bAnnFontBold);
	pLead->SetAnnFontStrikeThrough(m_bAnnFontStrikeThrough);
	pLead->SetAnnFontUnderline(m_bAnnFontUnderline);
	pLead->SetDeskewBackColor(m_crDeskew);
	pLead->SetAnnotateCallouts(m_bAnnotateCallouts);
	pLead->SetPrintBorderColor(m_crPrintBorder);
	pLead->SetPrintBorderThickness(m_fPrintBorderThickness);
	pLead->SetPrintBorder(m_bPrintBorder);
	pLead->SetPrintCalloutBorders(m_bPrintBorder);
	pLead->EnableDIBPrinting(m_bDIBPrintingEnabled);
}

//==============================================================================
//
// 	Function Name:	CTMLead::CopyAnn()
//
// 	Description:	This function will copy the annotation provided by the
//					callout object to the local container as well as the
//					containers of all other callouts if synchronized
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::CopyAnn(CCallout* pSource, HANNOBJECT hAnn)
{
	CAnnotation*	pAnn;
	HANNOBJECT		hCopy;

	//	Don't bother if we are not synchronizing annotations
	if(!m_bSyncCalloutAnn)
		return;

	//	Add a copy to the local container
	if((hCopy = CopyAnn(pSource->GetTMLead(), hAnn)) == 0)
		return;

    //	Add a local reference for the new object
	if((pAnn = m_Annotations.Add()) == 0)
		return;

	L_AnnGetRect(hAnn, &(pAnn->m_rcAnn), NULL);

	//	Set the tag for the annotation objects
	L_AnnSetTag(hAnn, pAnn->GetAnnTag(), 0);
	L_AnnSetTag(hCopy, pAnn->GetAnnTag(), 0);

	//	Add a link to the callout's annotation
	pAnn->Add(pSource);

	//	Now propagate the annotations to the remaining callouts
	SyncAnnotation(pAnn, pSource);
}

//==============================================================================
//
// 	Function Name:	CTMLead::CopyAnn()
//
// 	Description:	This function will make a copy of the annotation object
//					provided by the caller and add it to the local container
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
HANNOBJECT CTMLead::CopyAnn(CTMLead* pSource, HANNOBJECT hAnn)
{
	HANNOBJECT	hCopy;
	long		lContainer;

	//	Get the root container
	if((lContainer = CreateAnnContainer()) == 0)
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_NOCONTAINER);
		return 0;
	}
	
	//	Now try to make a copy of the annotation object
	if(L_AnnCopy(hAnn, &hCopy) != SUCCESS)
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_ANNCOPYFAILED);
		return 0;
	}

	//	Insert the copy in our local container
	if(L_AnnInsert((void*)lContainer, hCopy, FALSE) != SUCCESS)
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_INSERTCOPYFAILED);
		return 0;
	}

	//	Get the offset applied to the target container and apply it to the copy
	//
	//	NOTE:	This change was required when we moved from version 8 of the
	//			LeadTool control to version 11
	//
	//			Removed this code for the upgrade from version 11 to version 12
	//double dOffsetX;
	//double dOffsetY;
	//L_AnnGetOffsetX((void*)lContainer, &dOffsetX);
	//L_AnnGetOffsetY((void*)lContainer, &dOffsetY);
	//L_AnnMove(hCopy, dOffsetX, dOffsetY, 0);

	//	This may be a newly created container
	if(GetAnnContainer() != lContainer)
	{
		//	I'm not exactly sure why we have to do this but the first annotation
		//	in the container has to be offset to allow for the frame thickness
		//
		//	NOTE:	This change was required when we upgraded from version 8 of
		//			the LeadTool control to version 11
		//
		//			Removed this code for the upgrade from version 11 to 12
		//L_AnnMove(hCopy, GetFrameThickness(), GetFrameThickness(), 0);
	
		SetAnnContainer(lContainer);
		L_AnnGetItem((void*)GetAnnContainer(), &hCopy);
		L_AnnDestroy((void*)lContainer, ANNFLAG_RECURSE);
	}

	return hCopy;
}

//==============================================================================
//
// 	Function Name:	CTMLead::CopyAnnContainer()
//
// 	Description:	This function will create a copy of the control's annotation
//					container
//
// 	Returns:		The handle to the duplicate container if successful. 
//					0 otherwise
//
//	Notes:			None
//
//==============================================================================
long CTMLead::CopyAnnContainer()
{
	HANNOBJECT		hContainer;
	HANNOBJECT		hCopy;

	//	Get the existing container if there is one
	if((hContainer = (HANNOBJECT)GetAnnContainer()) == 0)
		return 0;

	//	Make a copy of the container
	if(L_AnnCopy(hContainer, &hCopy) == SUCCESS)
		return (long)hCopy;
	else
		return 0;		
}

//==============================================================================
//
// 	Function Name:	CTMLead::Create()
//
// 	Description:	This function will create the TMLead window.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLead::Create(CWnd* pParent, UINT uId)
{
	BOOL bCreate;

	//	Allocate a system string for the license
	BSTR lpLic = SysAllocString(szLicense);
	
	//	Create the window
	bCreate = CLead::Create("CTMLead", WS_CHILD, CRect(0,0,0,0), pParent, 
							uId, NULL, FALSE, lpLic);
	
	//	Set the default properties
	if(bCreate)
	{
		//	Unlock support for document express
		//
		//	MODIFIED IN UPGRADE FROM 11 TO 12
		UnlockSupport(L_SUPPORT_DOCUMENT, szSupportKey);
		
		//	Unlock support for LZW compression
		//UnlockSupport(L_SUPPORT_GIFLZW, SZ_KEY_GIFLZW);
		//UnlockSupport(L_SUPPORT_TIFLZW, SZ_KEY_TIFLZW);

		SetEnableMethodErrors(TRUE);    //  Enable Error Messages
		SetBorderStyle(0);				//	No border
		SetAutoRepaint(FALSE);          //  We will control painting
		SetScaleMode(3);                //  Scale based on pixels
		SetPaintScaling(PAINTSCALING_RESAMPLE);	//	Resample color images
		SetBackErase(FALSE);            //  Don't erase background on redraw
		SetAutoSetRects(TRUE);			//	Auto control the rectangles
		SetAnnAutoMenuEnable(FALSE);	//	No popup menu on right click
		UNLOCKSUPPORT(*this);           //  Support compressed files

		SetAutoAnimate(FALSE);
		SetLoopAnimate(FALSE);

	}

	//	Deallocate the license string
	SysFreeString(lpLic);	

	return bCreate;
}

//==============================================================================
//
// 	Function Name:	CTMLead::CreateAnnContainer()
//
// 	Description:	This function will create a container to be used as the
//					root container for all annotations.
//
// 	Returns:		The handle to the container if successful. 0 otherwise
//
//	Notes:			None
//
//==============================================================================
long CTMLead::CreateAnnContainer()
{
	HANNOBJECT		hContainer;
	ANNRECT			rcContainer;

	//	Get the existing container if there is one
	if(GetAnnContainer() != 0)
		return GetAnnContainer();

	//	Is the window valid?
	if(!IsWindow(m_hWnd))
		return 0;

	//	Get the handle to the current bitmap
	if(GetBitmap() == 0)
		return 0;

	//	Set the container rectangle
	rcContainer.left   = 0;
	rcContainer.top    = 0;
	rcContainer.right  = GetSrcWidth()  - 1;
	rcContainer.bottom = GetSrcHeight() - 1;

	//	Create a new container
	if(L_AnnCreateContainer(m_hWnd, &rcContainer, TRUE, &hContainer) != SUCCESS)
		return 0;

	//	Set the containers scale factors
	if(GetSrcWidth() > 0)
		L_AnnSetScalarX(hContainer, (GetDstWidth() / GetSrcWidth()), 0);
	if(GetSrcHeight() > 0)
		L_AnnSetScalarY(hContainer, (GetDstHeight() / GetSrcHeight()), 0);
	
	//	Set the container's offsets
	L_AnnSetOffsetX(hContainer, GetDstLeft(), ANNFLAG_RECURSE);
	L_AnnSetOffsetY(hContainer, GetDstTop(), ANNFLAG_RECURSE);

	return (long)hContainer;
}

//==============================================================================
//
// 	Function Name:	CTMLead::CreateUserCallout()
//
// 	Description:	This function will create a callout of the rectangle
//					specified by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::CreateUserCallout(RECT* prcUser) 
{
	CCallout*		pCallout;
	CAnnotation*	pAnn;
	HANNOBJECT		hAnn;
	RECT			rcDst;
	RECT			rcWnd;
	RECT			rcBackground;
	float			fMargin;
	float			fWndWidth;
	float			fWndHeight;
	int				iWidth;
	int				iHeight;

	ASSERT(prcUser);
	if(!prcUser) return;

	//	Calculate the size of the callout rectangle
	iWidth  = prcUser->right - prcUser->left;
	iHeight = prcUser->bottom - prcUser->top;

	//	Is the callout rectangle too small?
	if(iWidth < MINIMUM_CALLOUTRECT || iHeight < MINIMUM_CALLOUTRECT)
		return;
			
	//	Allocate a new callout
	pCallout = new CCallout(m_pControl, this);
	ASSERT(pCallout);

	//	Was the callout window created successfully
	if(!IsWindow(pCallout->m_hWnd))
	{
		delete pCallout;
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_NOCALLOUTWND);
		return;
	}

	//	Add it to the list
	if(m_pCallouts)
		m_pCallouts->Add(pCallout);

	//	Get the rectangle of the control window
	ASSERT(m_pControl);
	m_pControl->GetWindowRect(&rcWnd);
	pCallout->SetOriginalContainer(&rcWnd);

	//	How large a margin do we want to leave?
	fWndWidth  = (float)(rcWnd.right - rcWnd.left);
	fWndHeight = (float)(rcWnd.bottom - rcWnd.top);
	if(fWndWidth < fWndHeight)
		fMargin = ((float)CALLOUT_MARGIN / (float)100.0) * fWndWidth;
	else
		fMargin = ((float)CALLOUT_MARGIN / (float)100.0) * fWndHeight;

	//	Adjust the rectangle to leave a margin around the callout
	rcWnd.left   += ROUND(fMargin);
	rcWnd.top    += ROUND(fMargin);
	rcWnd.right  -= ROUND(fMargin);
	rcWnd.bottom -= ROUND(fMargin);

	//	Get the current destination rectangle
	GetDstRects(&rcDst, 0);

	//	Offset the destination rectangle to line up with the region defined
	//	by the callout rectangle
	//
	//	Note: The top-left corner of the destination rectangle is always
	//		  less than or equal to zero
	rcDst.top  -= prcUser->top;
	rcDst.left -= prcUser->left;

	//	Set the callout rectangles
	pCallout->SetRects(&rcWnd, &rcDst, prcUser, m_sCallFrameThick);

	//	Initialize the rectangles used to rescale the callout
	pCallout->SetContainer(&rcWnd);

	//	Set the original position
	pCallout->GetWindowRect(&rcWnd);
	pCallout->SetOriginalPosition(&rcWnd);

	//	Set the callout flags
	pCallout->SetShaded(m_bShadeOnCallout);

	//	Copy all of the local annotations to the callout's container
	pAnn = m_Annotations.First();
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
				if(pCallout->CopyAnn(this, hAnn))
				{
					pAnn->Add(pCallout);

				}

			}
		}

		pAnn = m_Annotations.Next();
	}

	//	Draw the rectangular highlight for the callout area
	hAnn = DrawCalloutAnn(prcUser, FALSE);

	//	Add a local reference to the annotation list for the callout highlight
	if(hAnn)
	{
		//	Send the highlight to the back of the container to make sure it's
		//	properly displayed
		L_AnnSendToBack(hAnn);

		if((pAnn = m_Annotations.Add()) != 0)
		{
			//	Mark this as a callout annotation
			pAnn->m_bIsCallout = TRUE;
			pAnn->Add(pCallout);

			//	Set the tag for this annotation
			L_AnnSetTag(hAnn, pAnn->GetAnnTag(), 0);
			L_AnnGetRect(hAnn, &(pAnn->m_rcAnn), NULL);
			
			//	Set the annotation id in the callout
			pCallout->SetAnnId(pAnn->m_wId);

			//	Notify the container
			if(m_pControl != 0)
				m_pControl->OnAnnotationDrawn(this, (long)hAnn);
		}
	}

	//	Do we need to create the shaded background?
	if((m_bShadeOnCallout == TRUE) && (m_hCalloutShade == 0))
	{
		//	We want to overlay the entire image
		GetDstRects(&rcBackground, 0);

		//	Create the background annotation
		if((m_hCalloutShade = DrawCalloutAnn(&rcBackground, TRUE)) != 0)
		{
			if((pAnn = m_Annotations.Add()) != 0)
			{
				//	Mark this as a cut out background
				pAnn->m_bIsCalloutShade = TRUE;
				
				//	Mark it as being a callout highlight. This prevents it from
				//	being drawn inside the callout windows
				pAnn->m_bIsCallout = TRUE;

				//	Set the tag for this annotation
				L_AnnSetTag(m_hCalloutShade, pAnn->GetAnnTag(), 0);
				L_AnnGetRect(m_hCalloutShade, &(pAnn->m_rcAnn), NULL);
			}
		}
	}

	//	Make sure the callout background remains on the bottom
	if(m_hCalloutShade)
	{
		L_AnnSendToBack(m_hCalloutShade);
	}

	//	Notify the control
	if(m_pControl)
		m_pControl->OnCalloutCreated(this, pCallout);

	//	Show the callout
	pCallout->ShowWindow(SW_SHOW);
}

//==============================================================================
//
// 	Function Name:	CTMLead::CTMLead()
//
// 	Description:	This is the constructor for CTMLead objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMLead::CTMLead() : CLead()
{
	//	Set the default properties
	m_pErrors				= 0;
	m_sLeadError			= 0;
	m_sAnnTool				= DEFAULT_ANNTOOL;
	m_sAnnThickness			= DEFAULT_ANNTHICKNESS;
	m_sAnnColor				= DEFAULT_ANNCOLOR;
	m_sRedactColor			= DEFAULT_REDACTCOLOR;
	m_sHighlightColor		= DEFAULT_HIGHLIGHTCOLOR;
	m_sCalloutColor			= DEFAULT_CALLOUTCOLOR;
	m_sCallFrameColor		= DEFAULT_CALLFRAMECOLOR;
	m_sCallHandleColor		= DEFAULT_CALLHANDLECOLOR;
	m_sCallFrameThick		= DEFAULT_CALLFRAMETHICKNESS;
	m_fMaxZoom				= (float)DEFAULT_MAXZOOM;
	m_sRotation				= DEFAULT_ROTATION;
	m_bScaleImage			= DEFAULT_SCALEIMAGE;
	m_bKeepAspect			= DEFAULT_KEEPASPECT;
	m_bFitToImage			= DEFAULT_FITTOIMAGE;
	m_bHideScrollBars		= DEFAULT_HIDESCROLLBARS;
	m_bSyncCalloutAnn		= DEFAULT_SYNCCALLOUTANN;
	m_bSetAnnProps			= TRUE;
	m_bSplitScreen			= FALSE;
	m_fPanPercent			= ((float)DEFAULT_PANPERCENT / 100.0);
	m_sAction				= DEFAULT_ACTION;
	m_bRightClickPan		= DEFAULT_RIGHTCLICKPAN;
	m_sBitonal				= DEFAULT_BITONALSCALING;
	m_bZoomToRect			= DEFAULT_ZOOMTORECT;
	m_strAnnFontName		= DEFAULT_ANNFONTNAME;
	m_sAnnFontSize			= DEFAULT_ANNFONTSIZE;
	m_bAnnFontBold			= DEFAULT_ANNFONTBOLD;
	m_bAnnFontUnderline		= DEFAULT_ANNFONTUNDERLINE;
	m_bAnnFontStrikeThrough	= DEFAULT_ANNFONTSTRIKETHROUGH;
	m_bAnnotateCallouts		= DEFAULT_ANNOTATECALLOUTS;
	m_bShadeOnCallout       = DEFAULT_SHADEONCALLOUT;
	m_bDIBPrintingEnabled   = TRUE;
	m_crDeskew				= DEFAULT_DESKEWBACKCOLOR;
	m_crPrintBorder			= DEFAULT_PRINTBORDERCOLOR;
	m_fPrintBorderThickness = DEFAULT_PRINTBORDERTHICKNESS;
	m_bPrintBorder			= DEFAULT_PRINTBORDER;
	m_bPrintCalloutBorders	= DEFAULT_PRINTCALLOUTBORDERS;
	m_sAnnColorDepth        = DEFAULT_ANNCOLORDEPTH;
	m_sQFactor				= DEFAULT_QFACTOR;

    //	Initialize the local members
    m_iWidth = 0;
    m_iHeight = 0;
    m_iTop = 0;
    m_iLeft = 0;
	m_iFillMode = ANNMODE_OPAQUE;
    m_sPages = 0;
    m_sPage = 0;
	m_sAngle = 0;
	m_sZoomState = ZOOMED_NONE;
	m_sZoomOnLoad = ZOOMED_NONE;
	m_sLastAction = NONE;
    m_fImageHeight = 0.0;
    m_fImageWidth  = 0.0;
    m_fAspectRatio = 1.0;
    m_fZoomFactor  = 1.0;
	m_fZapFactor   = 0.0;
	m_fZapRatio    = 0.0;
	m_fZapWidth    = 0.0;
	m_fZapHeight   = 0.0;
	m_fPrintFactor = 0.0;
	m_fPrintCx = 0.0;
	m_fPrintCy = 0.0;
	m_lPanX = 0;
	m_lPanY = 0;
	m_lMouseX = 0;
	m_lMouseY = 0;
	m_lAnnX = 0;
	m_lAnnY = 0;
	m_lUserData = 0;
	m_crBackground = RGB(0,0,0);
    m_crDraw = GetColorRef(DEFAULT_ANNCOLOR);
    m_crRedact = GetColorRef(DEFAULT_REDACTCOLOR);
    m_crHighlight = GetColorRef(DEFAULT_HIGHLIGHTCOLOR);
	m_crCallout = GetColorRef(DEFAULT_CALLOUTCOLOR);
	m_crCallFrame = GetColorRef(DEFAULT_CALLFRAMECOLOR);
 	m_crCalloutShadeBackground = RGB(0xc0,0xc0,0xc0);
    m_crCalloutShadeForeground = (m_crCalloutShadeBackground ^ 0x00FFFFFF);
	m_pAnnMemory = 0;
	m_hEditTextAnn = 0;
	m_hCalloutShade = 0;
	m_bLoaded = FALSE;
	m_bAnimation = FALSE;
	m_bPlayingAnimation = FALSE;
	m_pCallout = 0;
	m_pOwner = 0;
	memset(&m_rcRubberBand, 0, sizeof(RECT));
	memset(&m_rcMax, 0, sizeof(RECT));
	memset(&m_rcZapControl, 0, sizeof(RECT));
	memset(&m_rcZapMax, 0, sizeof(RECT));
	m_bZoomedSwipe = false;
	
	//	Load the cursors
	m_aCursors[PAN_CURSOR] = theApp.LoadCursor(IDC_PANCURSOR);
	m_aCursors[ZOOM_CURSOR] = theApp.LoadCursor(IDC_ZOOMCURSOR);
	m_aCursors[CALLOUT_CURSOR] = theApp.LoadCursor(IDC_CALLOUTCURSOR);

	//	Save the pointer to the parent control
	m_pControl = _pControl;

	//	Allocate the array to hold this object's callouts
	m_pCallouts = new CCallouts();
	ASSERT(m_pCallouts);
}

//==============================================================================
//
// 	Function Name:	CTMLead::~CTMLead()
//
// 	Description:	This is the destructor for CTMLead objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMLead::~CTMLead()
{
	//  Free all existing annotation memory
    if(m_pAnnMemory != 0)
    {
        for(int i = 0; i < (m_sPages + 1); i++)
            if(m_pAnnMemory[i].hMemory != 0)
                GlobalFree(m_pAnnMemory[i].hMemory);
        delete [] m_pAnnMemory;
        m_pAnnMemory = 0;
    }

    //	Get rid of the annotation objects
	m_Annotations.Flush(TRUE);
	m_hCalloutShade = 0; //	This prevents attempts to delete the annotation

	//	Get rid of all callouts
	DestroyCallouts();

	//	Destroy the list
	delete m_pCallouts;
	m_pCallouts = 0;
}

//==============================================================================
//
// 	Function Name:	CTMLead::DeleteAnn()
//
// 	Description:	This function will delete the annotation specified by the
//					caller
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMLead::DeleteAnn(HANNOBJECT hAnn, BOOL bIgnoreLock) 
{
	CAnnotation* pAnn = 0;

	if((pAnn = GetAnnFromHandle(hAnn)) != 0)
	{
		//	Make sure this annotation is not locked
		if((pAnn->m_bIsLocked == TRUE) && (bIgnoreLock == FALSE))
		{
			if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_ANNLOCKED);
			return TMV_ANNLOCKED;
		}
	}

	//	Are there any annotations in the LeadTools container?
	if(GetAnnContainer() == 0)
		return TMV_NOERROR;

	//	Remove the annotation from the container
	try
	{
		L_AnnDestroy(hAnn, 0);
	}
	catch(...)
	{
	}

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMLead::DeleteAnn()
//
// 	Description:	This function will delete the annotation with the tag
//					specified by the caller
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::DeleteAnn(DWORD dwTag) 
{
	HANNOBJECT hAnn;

	//	Get the handle of the object with this tag
	hAnn = GetHandleFromTag(dwTag);

	if(hAnn)
		L_AnnDestroy(hAnn, 0);
}

//==============================================================================
//
// 	Function Name:	CTMLead::DeleteAnn()
//
// 	Description:	This function is called by a callout when the user deletes
//					one of its annotations
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::DeleteAnn(CCallout* pSource, HANNOBJECT hAnn)
{
	CAnnotation*	pAnn;
	CCallout*		pCallout;
	DWORD			dwTag;

	//	Don't bother doing anything if we are not synchronizing annotations
	if(!m_bSyncCalloutAnn)
		return;
 
	//	Get the tag assigned to this annotation
	if(L_AnnGetTag((void*)hAnn, (L_UINT32*)(&dwTag)) != SUCCESS)
		return;

	//	Locate this annotation in the local list
	if((pAnn = m_Annotations.Find(dwTag)) == 0)
		return;

	//	Remove this object from the local list
	m_Annotations.Remove(pAnn, FALSE);

	//	Delete this annotation
	DeleteAnn(dwTag);

	//	Update each callout linked to this annotation except the source
	pCallout = pAnn->First();
	while(pCallout)
	{
		//	Make sure this callout is still in the list
		if((pSource != pCallout) && m_pCallouts->Find(pCallout))
		{
			pCallout->DeleteAnn(dwTag);
		}

		//	Get the next callout 
		pCallout = pAnn->Next();
	}

	delete pAnn;
}

//==============================================================================
//
// 	Function Name:	CTMLead::DeleteCallout()
//
// 	Description:	This function will delete the callout window specified by
//					the caller
//
// 	Returns:		None
//
//	Notes:			This function assumes the callout has already been removed
//					from the local list.
//
//==============================================================================
void CTMLead::DeleteCallout(CCallout* pCallout) 
{
	ASSERT(pCallout);
	
	//	Notify the control that this callout's being destroyed
	if(m_pControl)
		m_pControl->OnCalloutDestroyed(this, pCallout);

	//	Remove all references to this callout from the annotations lists
	m_Annotations.Remove(pCallout);

	//	Reset the local reference if it's the same
	//
	//	NOTE:	Don't notify the container because it will cause activation of this
	//			pane but that may not be correct (eg the pane is being unloaded)
	if(m_pCallout == pCallout)
		OnActivateCallout(0, FALSE);

	//	Hide the callout window before destroying it
	if(IsWindow(pCallout->m_hWnd) && pCallout->IsWindowVisible())
		pCallout->ShowWindow(SW_HIDE);

	//	Delete the object
	delete pCallout;

	//	Should we delete the background?
	if((m_hCalloutShade != 0) && (m_pCallouts != 0) && (m_pCallouts->CheckShaded() == FALSE))
	{
		DeleteAnn(m_hCalloutShade, TRUE);
		m_hCalloutShade = 0;
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::DeleteLastAnn()
//
// 	Description:	This function will delete the last annotation added to the
//					container
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMLead::DeleteLastAnn() 
{
	CAnnotation* pAnn;
	HANNOBJECT	 hAnn;

	//	Get the last annotation
	if((pAnn = m_Annotations.Last()) == 0)
		return TMV_NOANNOTATIONS;

	//	If this is the cutout background get the next to the last annotation
	if(pAnn->m_bIsCalloutShade)
		pAnn = m_Annotations.Prev();

	ASSERT(pAnn != 0);
	if(pAnn == 0)
		return TMV_NOANNOTATIONS;

	//	Delete the annotation
	if((hAnn = GetHandleFromTag(pAnn->GetAnnTag())) != 0)
		DeleteAnn(hAnn, FALSE);

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMLead::DeleteSelections()
//
// 	Description:	This function will delete all annotations that have been
//					selected in the image
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMLead::DeleteSelections()
{
	HANNOBJECT*	pSelections;
	UINT		uSelections;

	//	If there is an active callout and annotations are not synchronized,
	//	delete only the selections in the active callout
	if(m_pCallout && !m_bSyncCalloutAnn)
		return m_pCallout->DeleteSelections();

	//	Get the array of selections
	if((uSelections = GetSelections(&pSelections)) == 0)
		return TMV_NOERROR;

	//	Delete all the selections
	for(UINT i = 0; i < uSelections; i++)
		DeleteAnn(pSelections[i], TRUE);
	
	HeapFree(GetProcessHeap(), 0, pSelections);

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMLead::Deskew()
//
// 	Description:	This function is called to deskew the current image.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMLead::Deskew() 
{
	if(!IsLoaded())
	{
		return TMV_NOIMAGE;
	}
	else
	{
		
		if((m_sLeadError = CLead::Deskew(m_crDeskew, DSKW_DOCUMENTIMAGE | DSKW_FILL)) != 0)
		{
			HandleLeadError(m_sLeadError);
			return TMV_LEADERROR;
		}
	
		//	Repaint the image
		CLead::ForceRepaint();

		return TMV_NOERROR;
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::Despeckle()
//
// 	Description:	This function is called to despeckle the current image.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMLead::Despeckle() 
{
	if(!IsLoaded())
	{
		return TMV_NOIMAGE;
	}
	else
	{
		if((m_sLeadError = CLead::Despeckle()) != 0)
		{
			HandleLeadError(m_sLeadError);
			return TMV_LEADERROR;
		}
	
		//	Repaint the image
		CLead::ForceRepaint();

		return TMV_NOERROR;
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::DestroyCallouts()
//
// 	Description:	This function will destroy all callouts in the list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
short CTMLead::DestroyCallouts() 
{	
	CCallout* pCallout;

	//	Do we have a valid list?
	if(m_pCallouts == 0)
		return TMV_NOERROR;

	//	Are there any in the list?
	if(m_pCallouts->IsEmpty())
		return TMV_NOERROR;

	//	Delete the cutout background if it exists
	if(m_hCalloutShade != 0)
		DeleteAnn(m_hCalloutShade, TRUE);

	//	Notify the control as we destroy each callout
	pCallout = m_pCallouts->First();
	while(pCallout)
	{
		DeleteCallout(pCallout);
		pCallout = m_pCallouts->Next();
	}

	//	Flush the list. The objects have already been destroyed
	m_pCallouts->Flush(FALSE);
	m_pCallout = 0;

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMLead::DotRemove()
//
// 	Description:	This function is called to remove dots from a 1 bit image.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMLead::DotRemove(long lMinWidth, long lMinHeight, long lMaxWidth,
						 long lMaxHeight)
{
	if(!IsLoaded())
	{
		return TMV_NOIMAGE;
	}
	else
	{
		//	Set this property to make sure the borders get removed
		SetDocCleanSuccess(SUCCESS_REMOVE);

		if((m_sLeadError = CLead::DotRemove(DOT_USE_SIZE | DOT_USE_DPI,
											lMinWidth, lMinHeight,
											lMaxWidth, lMaxHeight)) != 0)
		{
			HandleLeadError(m_sLeadError);
			return TMV_LEADERROR;
		}
	
		//	Repaint the image
		CLead::ForceRepaint();

		return TMV_NOERROR;
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::Draw()
//
// 	Description:	This function will draw an image when it is loaded for the
//					first time.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::Draw() 
{	
	//	What zoom state should we be using for new images?
	switch(m_sZoomOnLoad)
	{
		case ZOOMED_FULLWIDTH:		RedrawFullWidth();
									return;

		case ZOOMED_FULLHEIGHT:		RedrawFullHeight();
									return;

		case ZOOMED_NONE:
		default:	
			if(m_bZoomedSwipe)
				ZoomToFactor();
			else
				RedrawNormal();
			m_bZoomedSwipe = false;
			return;
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::DrawCalloutAnn()
//
// 	Description:	This function will draw a rectangular annotation at the
//					specified position to highlight the callout 
//				
//
// 	Returns:		A handle to the annotation object if successful
//
//	Notes:			None
//
//==============================================================================
HANNOBJECT CTMLead::DrawCalloutAnn(RECT* pRect, BOOL bBackground)
{
	POINT		Point; 
	HANNOBJECT	hObject; 
	long		lContainer;

	//	Get the root container
	if((lContainer = CreateAnnContainer()) == 0)
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_NOCONTAINER);
		return NULL;
	}
	
	//	Create the rectangle annotation 
    if(L_AnnCreateItem((HANNOBJECT)lContainer, 
					   ANNOBJECT_RECT, TRUE, &hObject) != SUCCESS)
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_CREATEANNFAILED);
		return NULL;
	}

	//	Define the start point of the rectangle
    Point.x = pRect->left;
    Point.y = pRect->top;
    L_AnnDefine(hObject, &Point, ANNDEFINE_BEGINSET);

    //	Define the end point of the rectangle
	Point.x = pRect->right - 1;
    Point.y = pRect->bottom - 1;
    L_AnnDefine(hObject, &Point, ANNDEFINE_END);

	//	Set the rectangle properties
	if(bBackground)
	{
		L_AnnSetFillMode(hObject, ANNMODE_TRANSLUCENT, 0, 0);
		L_AnnSetForeColor(hObject, m_crCalloutShadeBackground, 0);
		L_AnnSetBackColor(hObject, m_crCalloutShadeBackground, 0);
	}
	else
	{
		//	Are we using cut out highlights?
		if(m_bShadeOnCallout)
		{
			L_AnnSetFillMode(hObject, ANNMODE_TRANSLUCENT, 0, 0);
			L_AnnSetROP2(hObject, ANNROP2_XOR, 0);
			L_AnnSetForeColor(hObject, m_crCalloutShadeForeground, 0);
			L_AnnSetBackColor(hObject, m_crCalloutShadeForeground, 0);
		}
		else
		{
			L_AnnSetForeColor(hObject, m_crCallout, 0);
			L_AnnSetBackColor(hObject, m_crCallout, 0);
			L_AnnSetFillMode(hObject, ANNMODE_TRANSLUCENT, 0, 0);
		}
	
	}

	//	Do we have to set the container?
	if(GetAnnContainer() != lContainer)
	{
		SetAnnContainer(lContainer);
		L_AnnGetItem((void*)GetAnnContainer(), &hObject);
		L_AnnDestroy((void*)lContainer, ANNFLAG_RECURSE);
	}
	
	return hObject;
}

//==============================================================================
//
// 	Function Name:	CTMLead::DrawRectangle()
//
// 	Description:	This function will draw a rectangular annotation at the
//					specified position relative to the client area of the Lead
//					Tools window
//				
// 	Returns:		A handle to the annotation object if successful
//
//	Notes:			None
//
//==============================================================================
HANNOBJECT CTMLead::DrawRectangle(RECT rcBounds, COLORREF crColor, short sTransparency)
{
	CAnnotation*	pAnn;
	POINT			Point; 
	HANNOBJECT		hObject; 
	long			lContainer;

	//	Get the root container
	if((lContainer = CreateAnnContainer()) == 0)
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_NOCONTAINER);
		return NULL;
	}
	
	//	Create the rectangle annotation 
    if(L_AnnCreateItem((HANNOBJECT)lContainer, 
					   ANNOBJECT_RECT, TRUE, &hObject) != SUCCESS)
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_CREATEANNFAILED);
		return NULL;
	}

	//	Do not attempt to set the annotation properties
	m_bSetAnnProps = FALSE;

	//	Define the start point of the rectangle
    Point.x = rcBounds.left;
    Point.y = rcBounds.top;
    L_AnnDefine(hObject, &Point, ANNDEFINE_BEGINSET);

    //	Define the end point of the rectangle
	Point.x = rcBounds.right;
    Point.y = rcBounds.bottom;
    L_AnnDefine(hObject, &Point, ANNDEFINE_END);

	L_AnnSetForeColor(hObject, crColor, 0);
	L_AnnSetBackColor(hObject, crColor, 0);

	//	Set the transparency
	switch(sTransparency)
	{
		case TMV_TRANSPARENT:

			L_AnnSetFillMode(hObject, ANNMODE_TRANSPARENT, 0, 0);
			break;

		case TMV_TRANSLUCENT:

			L_AnnSetFillMode(hObject, ANNMODE_TRANSLUCENT, 0, 0);
			break;

		case TMV_OPAQUE:
		default:

			L_AnnSetFillMode(hObject, ANNMODE_OPAQUE, 0, 0);
			break;

	}

	//	Do we have to set the container?
	if(GetAnnContainer() != lContainer)
	{
		//	This actually creates a copy of the specified container
		SetAnnContainer(lContainer);
		L_AnnGetItem((void*)GetAnnContainer(), &hObject);

		//	Destroy the copied container
		L_AnnDestroy((void*)lContainer, ANNFLAG_RECURSE);

		//	This was put in under LeadTools version 12
		//
		//	There is a bug in which the offsets and scale factors are not applied
		//	to the container if the window is not visible
		if(!IsWindowVisible())
		{
			L_AnnSetScalarX((void*)GetAnnContainer(), (GetDstWidth() / GetSrcWidth()), 0);
			L_AnnSetScalarY((void*)GetAnnContainer(), (GetDstHeight() / GetSrcHeight()), 0);
			L_AnnSetOffsetX((void*)GetAnnContainer(), GetDstLeft(), 0);
			L_AnnSetOffsetY((void*)GetAnnContainer(), GetDstTop(), 0);
		}

	}
	
	if(hObject)
	{
		if((pAnn = m_Annotations.Add()) != 0)
		{
			pAnn->m_bIsCallout = FALSE;

			//	Set the tag for this annotation
			L_AnnSetTag(hObject, pAnn->GetAnnTag(), 0);
			L_AnnGetRect(hObject, &(pAnn->m_rcAnn), NULL);
		}
	}

	m_bSetAnnProps = TRUE;

	return hObject;
}

//==============================================================================
//
// 	Function Name:	CTMLead::DrawSourceRectangle()
//
// 	Description:	This function will draw a rectangular annotation at the
//					specified position relative to the source image.
//				
// 	Returns:		A handle to the annotation object if successful
//
//	Notes:			None
//
//==============================================================================
HANNOBJECT CTMLead::DrawSourceRectangle(RECT rcBounds, COLORREF crColor, short sTransparency)
{
	RECT rcClient;

	//	Lead Tools draws annotation relative to the client window so we need to
	//	convert the caller's coordinates
	BitmapToClient((float)rcBounds.left, (float)rcBounds.top);
	rcClient.left = ROUND(GetConvertX());
	rcClient.top  = ROUND(GetConvertY());

	BitmapToClient((float)rcBounds.right, (float)rcBounds.bottom);
	rcClient.right = ROUND(GetConvertX());
	rcClient.bottom = ROUND(GetConvertY());

	//	Draw the annotation
	return DrawRectangle(rcClient, crColor, sTransparency);
}

//==============================================================================
//
// 	Function Name:	CTMLead::DrawSourceText()
//
// 	Description:	This function will draw a text annotation at the
//					specified position relative to the source image
//				
// 	Returns:		A handle to the annotation object if successful
//
//	Notes:			None
//
//==============================================================================
HANNOBJECT CTMLead::DrawSourceText(LPCSTR lpszText, RECT rcBounds, 
								   COLORREF crColor, LPCSTR lpszFont, short sSize)
{
	RECT rcClient;

	//	Lead Tools draws annotation relative to the client window so we need to
	//	convert the caller's coordinates
	BitmapToClient((float)rcBounds.left, (float)rcBounds.top);
	rcClient.left = ROUND(GetConvertX());
	rcClient.top  = ROUND(GetConvertY());

	BitmapToClient((float)rcBounds.right, (float)rcBounds.bottom);
	rcClient.right = ROUND(GetConvertX());
	rcClient.bottom = ROUND(GetConvertY());

	//	Draw the text
	return DrawText(lpszText, rcClient, crColor, lpszFont, sSize);
}

//==============================================================================
//
// 	Function Name:	CTMLead::DrawText()
//
// 	Description:	This function will draw a text annotation at the
//					specified position relative to the client area of the Lead
//					Tools window
//				
// 	Returns:		A handle to the annotation object if successful
//
//	Notes:			None
//
//==============================================================================
HANNOBJECT CTMLead::DrawText(LPCSTR lpszText, RECT rcBounds, COLORREF crColor, 
							 LPCSTR lpszFont, short sSize)
{
	CAnnotation*	pAnn;
	POINT			Point; 
	HANNOBJECT		hObject; 
	long			lContainer;

	//	Don't bother if no text is supplied
	if((lpszText == 0) || (lstrlen(lpszText) == 0)) return 0;

	//	Get the root container
	if((lContainer = CreateAnnContainer()) == 0)
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_NOCONTAINER);
		return NULL;
	}
	
	//	Create the rectangle annotation 
    if(L_AnnCreateItem((HANNOBJECT)lContainer, 
					   ANNOBJECT_TEXT, TRUE, &hObject) != SUCCESS)
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_CREATEANNFAILED);
		return NULL;
	}

	//	Do not attempt to set the annotation properties
	m_bSetAnnProps = FALSE;

	//	Define the start point of the rectangle
    Point.x = rcBounds.left;
    Point.y = rcBounds.top;
    L_AnnDefine(hObject, &Point, ANNDEFINE_BEGINSET);

    //	Define the end point of the rectangle
	Point.x = rcBounds.right - 1;
    Point.y = rcBounds.bottom - 1;
    L_AnnDefine(hObject, &Point, ANNDEFINE_END);

	L_AnnSetText(hObject, (char*)lpszText, 0);

	if(sSize > 0)
		L_AnnSetFontSize(hObject, sSize, 0);
	else
		L_AnnSetFontSize(hObject, m_sAnnFontSize, 0);

	if((lpszFont != 0) && (lstrlen(lpszFont) != 0))
		L_AnnSetFontName(hObject, (char*)lpszFont, 0);
	else
		L_AnnSetFontName(hObject, m_strAnnFontName.GetBuffer(1), 0);
	
	L_AnnSetFontBold(hObject, FALSE, 0);
	L_AnnSetFontStrikeThrough(hObject, FALSE, 0);
	L_AnnSetFontUnderline(hObject, FALSE, 0);
	L_AnnSetFillMode(hObject, ANNMODE_TRANSPARENT, 0, 0);
	L_AnnSetForeColor(hObject, crColor, 0);

	//	Do we have to set the container?
	if(GetAnnContainer() != lContainer)
	{
		//	This actually creates a copy of the specified container
		SetAnnContainer(lContainer);
		L_AnnGetItem((void*)GetAnnContainer(), &hObject);

		//	Destroy the copied container
		L_AnnDestroy((void*)lContainer, ANNFLAG_RECURSE);

		//	This was put in under LeadTools version 12
		//
		//	There is a bug in which the offsets and scale factors are not applied
		//	to the container if the window is not visible
		if(!IsWindowVisible())
		{
			L_AnnSetScalarX((void*)GetAnnContainer(), (GetDstWidth() / GetSrcWidth()), 0);
			L_AnnSetScalarY((void*)GetAnnContainer(), (GetDstHeight() / GetSrcHeight()), 0);
			L_AnnSetOffsetX((void*)GetAnnContainer(), GetDstLeft(), 0);
			L_AnnSetOffsetY((void*)GetAnnContainer(), GetDstTop(), 0);
		}

	}
	
	//	Add a local reference to the annotation list for the callout highlight
	if(hObject)
	{
		if((pAnn = m_Annotations.Add()) != 0)
		{
			pAnn->m_bIsCallout = FALSE;

			//	Set the tag for this annotation
			L_AnnSetTag(hObject, pAnn->GetAnnTag(), 0);
			L_AnnGetRect(hObject, &(pAnn->m_rcAnn), NULL);
		}
	}

	m_bSetAnnProps = TRUE;

	return hObject;
}

//==============================================================================
//
// 	Function Name:	CTMLead::EnableDIBPrinting()
//
// 	Description:	This function is called to enable/disable DIB printing
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::EnableDIBPrinting(BOOL bEnable) 
{
	m_bDIBPrintingEnabled = bEnable;
}

//==============================================================================
//
// 	Function Name:	CTMLead::Erase()
//
// 	Description:	This function is called to erase the rectangle specified
//					by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::Erase(CDC* pdc, RECT* pRect) 
{
	CBrush	brErase;
	CBrush*	pOldBrush;
	CPen*	pOldPen;

	ASSERT(pdc);
	ASSERT(pRect);

	//	Initialize the device context
	brErase.CreateSolidBrush(RGB(0xFF,0xFF,0xFF));
	pOldBrush = pdc->SelectObject(&brErase);
	pOldPen = (CPen*)pdc->SelectStockObject(NULL_PEN);
	
	//	Erase the background
	pdc->Rectangle(pRect);
	
	//	Cleanup
	if(pOldBrush) pdc->SelectObject(pOldBrush);
	if(pOldPen) pdc->SelectObject(pOldPen);
}

//==============================================================================
//
// 	Function Name:	CTMLead::OnEndEditTextAnn()
//
// 	Description:	Called when the user finishes editing a text annotation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnEndEditTextAnn(BOOL bCancelled) 
{
	if(m_hEditTextAnn != 0)
	{
		//	Notify the host control that the session is over
		if(m_pControl)
			m_pControl->OnEndEditTextAnn(this);

		//	The text annotation is no longer in edit mode
		m_hEditTextAnn = 0;
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::Erase()
//
// 	Description:	This function allows the caller to erase all annotations on 
//					the current image.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::Erase(BOOL bIgnoreLocked) 
{	
    CAnnotations	Trash;
	CAnnotation*	pAnn;
	HANNOBJECT		hAnn;

	if(bIgnoreLocked == TRUE)
	{
		//	NOTE:	We don't want the AnnotationDestroyed event fired if we are
		//			loading a new file as is the case when bIgnoreLocked == TRUE

		//	Flush the local list of annotations
		m_Annotations.Flush(TRUE);

		//	Get rid of all callouts
		DestroyCallouts();

		L_AnnDestroy((void *)GetAnnContainer(), ANNFLAG_RECURSE | ANNFLAG_NOTTHIS);
	}
	else
	{
		//	Locate the annotations to be destroyed
		pAnn = m_Annotations.First();

		while(pAnn != 0)
		{
			if(pAnn->m_bIsLocked == FALSE && pAnn->m_bIsCalloutShade == FALSE)
				Trash.Add(pAnn);

			pAnn = m_Annotations.Next();
		}
	
		//	Now remove them from the local collection
		//
		//	NOTE:	We don't want them in the list when we delete the annotation
		pAnn = Trash.First();
		while(pAnn != 0)
		{
			m_Annotations.Remove(pAnn, FALSE);
			pAnn = Trash.Next();
		}

		//	Get rid of all callouts
		DestroyCallouts();

		//	Now destroy the annotations
		pAnn = Trash.First();
		while(pAnn != 0)
		{
			if((hAnn = GetHandleFromTag(pAnn->GetAnnTag())) != 0)
			{
				//	Notify the container
				//
				//	NOTE:	We have to do this here because nothing is going to happen
				//			when we process the AnnDestroyed event since we've removed
				//			this annotation from the local list
				if(m_pControl != 0)
					m_pControl->OnAnnotationDeleted(this, (long)hAnn);

				L_AnnDestroy(hAnn, 0);
			}

			pAnn = Trash.Next();
		}

		Trash.Flush(TRUE);

	}


	m_hCalloutShade = 0;
}

//==============================================================================
//
// 	Function Name:	CTMLead::FindFile()
//
// 	Description:	This function is called to determine if the specified file
//					exists
//
// 	Returns:		TRUE if the file is found
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLead::FindFile(LPCSTR lpFilespec) 
{
	WIN32_FIND_DATA	FindData;
	HANDLE			hFind;

	if((hFind = FindFirstFile(lpFilespec, &FindData)) == INVALID_HANDLE_VALUE)
	{
		return FALSE;
	}
	else
	{
		FindClose(hFind);
		return TRUE;
	}	
}

//==============================================================================
//
// 	Function Name:	CTMLead::FireRectangleDiagnostic()
//
// 	Description:	This function is provided as a debugging aid. It will 
//					display the coordinates of the specified rectangle.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::FireRectangleDiagnostic(RECT* pRect, LPSTR lpTitle) 
{
	CString	Msg;

	ASSERT(pRect);

	//	Source rectangle
	Msg.Format("L: %d T: %d R: %d B: %d",
			   pRect->left, pRect->top, pRect->right, pRect->bottom);

	if(m_pControl != 0)
		m_pControl->m_Diagnostics.Report(lpTitle, Msg);
}

//==============================================================================
//
// 	Function Name:	CTMLead::FirstPage()
//
// 	Description:	This external method allows the caller to go directly to the
//					first page of a multipage image
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMLead::FirstPage() 
{
	short sLeadError;

	//	Are we already on the first page?
	if((m_sPages <= 1) || (m_sPage ==1))
		return TMV_NOERROR;

	//	Attempt to load the first page
	if((sLeadError = LoadImage(m_strFilename, 1)) == TMV_NOERROR)
	{
		Draw();
		return TMV_NOERROR;
	}
	else
	{
		return HandleFileError(m_strFilename, sLeadError);
	}
	
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetAction()
//
// 	Description:	This function is called by the control window to determine
//					the current action for the control.
//
// 	Returns:		The current action.
//
//	Notes:			None
//
//==============================================================================
short CTMLead::GetAction() 
{
	return m_sAction;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetAngle()
//
// 	Description:	This function is called to get the cummulative angle of
//					rotation applied to the current image
//
// 	Returns:		The cumulative rotation
//
//	Notes:			None
//
//==============================================================================
short CTMLead::GetAngle() 
{
	return m_sAngle;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetAnnFromHandle()
//
// 	Description:	This function is called to get the annotation object with 
//					the handle specified by the caller
//
// 	Returns:		A pointer to the annotation object if found
//
//	Notes:			None
//
//==============================================================================
CAnnotation* CTMLead::GetAnnFromHandle(HANNOBJECT hAnn) 
{
	DWORD dwTag;

	//	Get the tag associated with this object
	if(L_AnnGetTag(hAnn, (L_UINT32*)(&dwTag)) != SUCCESS)
		return 0;
	else
		return GetAnnFromTag(dwTag);
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetAnnFromPt()
//
// 	Description:	This function is called to determine if an annotation exists
//					at the point specified by the caller
//
// 	Returns:		The handle of the annotation at the requested point if found
//
//	Notes:			None
//
//==============================================================================
HANNOBJECT CTMLead::GetAnnFromPt(POINT* pPoint) 
{
	ANNHITTESTINFO	hitTestInfo;	
	HANNOBJECT		hContainer = (HANNOBJECT)GetAnnContainer();
	HANNOBJECT		hAnn = 0;
	UINT			uPos;

	ASSERT(pPoint);

	//	Do we have any annotations
	if(hContainer == 0)
		return 0;

	if(L_AnnHitTest(hContainer, pPoint, &uPos, &hAnn, &hitTestInfo, sizeof(hitTestInfo)) != SUCCESS)
		return 0;

	//	Do not treat the container itself as a valid annotation
	if((uPos == ANNHIT_NONE) || (hAnn == hContainer))
		return 0;
	else
		return hAnn;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetAnnFromTag()
//
// 	Description:	This function is called to get the annotation object with 
//					the tag specified by the caller
//
// 	Returns:		A pointer to the annotation object if found
//
//	Notes:			None
//
//==============================================================================
CAnnotation* CTMLead::GetAnnFromTag(DWORD dwTag) 
{
	CAnnotation* pAnn = m_Annotations.First();

	while(pAnn)
	{
		if(pAnn->GetAnnTag() == dwTag)
			return pAnn;
		pAnn = m_Annotations.Next();
	}
	return 0;	
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetAspectRatio()
//
// 	Description:	This function is called to retrieve the aspect ratio of the 
//					current image.
//
// 	Returns:		The image height
//
//	Notes:			None
//
//==============================================================================
float CTMLead::GetAspectRatio() 
{
	return m_fAspectRatio;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetCallFrameColor()
//
// 	Description:	This function is called to get the color of callout frames
//
// 	Returns:		The current callout frame color
//
//	Notes:			None
//
//==============================================================================
COLORREF CTMLead::GetCallFrameColor() 
{
	return m_crCallFrame;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetCallHandleColor()
//
// 	Description:	This function is called to get the color of callout frames
//
// 	Returns:		The current callout frame color
//
//	Notes:			None
//
//==============================================================================
COLORREF CTMLead::GetCallHandleColor() 
{
	return m_crCallHandle;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetCalloutCount()
//
// 	Description:	This function is called to get the number of callouts
//					associated with this pane.
//
// 	Returns:		The number of active callouts
//
//	Notes:			None
//
//==============================================================================
long CTMLead::GetCalloutCount()
{
	if(m_pCallouts)
		return m_pCallouts->GetCount();
	else
		return 0;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetCallouts()
//
// 	Description:	This function is called to get the list of callouts 
//					associated with this pane.
//
// 	Returns:		The current list of callouts
//
//	Notes:			None
//
//==============================================================================
CCallouts* CTMLead::GetCallouts()
{
	return m_pCallouts;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetColor()
//
// 	Description:	This function is called to retrieve the color associated
//					with the active tool.
//
// 	Returns:		The color identifier as defined in tmvdefs.h
//
//	Notes:			None
//
//==============================================================================
int CTMLead::GetColor()
{
    //	What annotation tool is active
	switch(m_sAction)
	{
		case REDACT:	return m_sRedactColor;
		case HIGHLIGHT:	return m_sHighlightColor;
		case CALLOUT:	return m_sCalloutColor;
		case ZOOM:		
		case DRAW:		
		case PAN:	
		case SELECT:	
		default:		return m_sAnnColor;
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::GetColorId()
//
// 	Description:	This function translates a color reference to the appropriate
//					TMVIEW color identifier.
//
// 	Returns:		The corresponding color identifier.
//
//	Notes:			None
//
//==============================================================================
int CTMLead::GetColorId(COLORREF crColor)
{
	switch(crColor)
    {
        case RGB(0,0,0):		return TMV_BLACK;        
        case RGB(255,0,0):      return TMV_RED;
        case RGB(0,255,0):      return TMV_GREEN;
        case RGB(0,0,255):      return TMV_BLUE;
        case RGB(255,255,0):    return TMV_YELLOW;
        case RGB(255,0,255):    return TMV_MAGENTA;
        case RGB(0,255,255):    return TMV_CYAN;
        case RGB(128,128,128):  return TMV_GREY;
        case RGB(255,255,255):  return TMV_WHITE;
        case RGB(128,0,0):      return TMV_DARKRED;
        case RGB(0,128,0):      return TMV_DARKGREEN;
        case RGB(0,0,128):      return TMV_DARKBLUE;
        case RGB(255,128,128):  return TMV_LIGHTRED;
        case RGB(128,255,128):  return TMV_LIGHTGREEN;
        case RGB(128,128,255):  return TMV_LIGHTBLUE;
        default:				return TMV_BLACK;
    }
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetColorRef()
//
// 	Description:	This function translates a TMVIEW color identifier to the
//					appropriate color reference.
//
// 	Returns:		The corresponding color reference.
//
//	Notes:			None
//
//==============================================================================
COLORREF CTMLead::GetColorRef(int iColor)
{
    switch(iColor)
    {
        case TMV_BLACK:			return (RGB(0,0,0));        
        case TMV_RED:			return (RGB(255,0,0));      
        case TMV_GREEN:			return (RGB(0,255,0));      
        case TMV_BLUE:			return (RGB(0,0,255));      
        case TMV_YELLOW:		return (RGB(255,255,0));    
        case TMV_MAGENTA:		return (RGB(255,0,255));    
        case TMV_CYAN:			return (RGB(0,255,255));    
        case TMV_GREY:			return (RGB(128,128,128));  
        case TMV_WHITE:			return (RGB(255,255,255));  
        case TMV_DARKRED:       return (RGB(128,0,0));      
        case TMV_DARKGREEN:     return (RGB(0,128,0));      
        case TMV_DARKBLUE:      return (RGB(0,0,128));      
        case TMV_LIGHTRED:      return (RGB(255,128,128));      
        case TMV_LIGHTGREEN:	return (RGB(128,255,128));      
        case TMV_LIGHTBLUE:     return (RGB(128,128,255));      
        default:				return (RGB(0,0,0));
    }
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetControl()
//
// 	Description:	This function is called to retrieve the pointer to the
//					host control.
//
// 	Returns:		A pointer to the TMView control object
//
//	Notes:			None
//
//==============================================================================
CTMViewCtrl* CTMLead::GetControl() 
{
	return m_pControl;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetCurrentPage()
//
// 	Description:	This function is called to retrieve the number of the page
//					being displayed
//
// 	Returns:		The page number.
//
//	Notes:			None
//
//==============================================================================
short CTMLead::GetCurrentPage() 
{
	return m_sPage;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetDeskewBackColor()
//
// 	Description:	This function is called to retrieve the current deskew 
//					background color.
//
// 	Returns:		The current background color
//
//	Notes:			None
//
//==============================================================================
COLORREF CTMLead::GetDeskewBackColor() 
{
	return m_crDeskew;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetDstAspect()
//
// 	Description:	This function is called to get the aspect ratio of the
//					current source rectangle
//
// 	Returns:		The ratio of the source height to width
//
//	Notes:			None
//
//==============================================================================
float CTMLead::GetDstAspect() 
{
	//	Do we have an image?
	if(!IsLoaded())
		return 0;

	return (GetDstHeight() / GetDstWidth());
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetDstRects()
//
// 	Description:	This function is called to get the destination and 
//					destination clip rectangles
//
// 	Returns:		None
//
//	Notes:			RECT.right  = width (not right hand coordinate)
//					RECT.bottom = height (not bottom coordinate)
//
//==============================================================================
void CTMLead::GetDstRects(RECT* pDst, RECT* pClip) 
{
	//	Do we have an image?
	if(!IsLoaded())
	{
		memset(pDst, 0, sizeof(RECT));
		memset(pClip, 0, sizeof(RECT));
	}
	else
	{
		//	Get the coordinates of the destination rectangle
		if(pDst)
		{
			pDst->top    = (long)GetDstTop();
			pDst->left   = (long)GetDstLeft();
			pDst->bottom = (long)GetDstHeight();
			pDst->right  = (long)GetDstWidth();
		}
	
		//	Get the coordinates of the destination clip rectangle
		if(pClip)
		{
			pClip->top    = (long)GetDstClipTop();
			pClip->left   = (long)GetDstClipLeft();
			pClip->bottom = (long)GetDstClipHeight();
			pClip->right  = (long)GetDstClipWidth();
		}
	
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::GetFilename()
//
// 	Description:	This function allows the caller to retrieve the 
//					name of the current image file.
//
// 	Returns:		The current filename
//
//	Notes:			None
//
//==============================================================================
LPCTSTR CTMLead::GetFilename() 
{
	return m_strFilename;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetFormatText()
//
// 	Description:	This function gets the text descriptor for the Lead Tools
//					format identifier
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::GetFormatText(int iFormat, CString &rFormat)
{
	switch(iFormat)
	{
		case FILE_PCX:					rFormat = "ZSoft PCX";
										break;

		case FILE_GIF:					rFormat = "CompuServe GIF";
										break;

		case FILE_TIF:					rFormat = "Tagged Image File Format";
										break;

		case FILE_TGA:					rFormat = "Targa";
										break;

		case FILE_CMP:					rFormat = "LEAD CMP";
										break;

		case FILE_BMP:					rFormat = "Windows Bitmap";
										break;

		case FILE_BMP_RLE:				rFormat = "Windows RLE Bitmap";
										break;

		case FILE_JFIF:					rFormat = "Jpeg File Interchange Format";
										break;

		case FILE_JTIF:					rFormat = "Jpeg Tag Image File Format";
										break;

		case FILE_OS2:					rFormat = "OS/2 BMP";
										break;

		case FILE_WMF:					rFormat = "Windows Meta File";
										break;

		case FILE_EPS:					rFormat = "Encapsulated Post Script";
										break;

		case FILE_TIFLZW:				rFormat = "TIF Format with LZW compression";
										break;

		case FILE_LEAD:					rFormat = "LEAD  Proprietary";
										break;

		case FILE_LEAD1JFIF:			rFormat = "JPEG  4:1:1";
										break;

		case FILE_LEAD1JTIF:			rFormat = "JTIF  4:1:1";
										break;

		case FILE_LEAD2JFIF:			rFormat = "JPEG  4:2:2";
										break;

		case FILE_LEAD2JTIF:			rFormat = "JTIF  4:2:2";
										break;

		case FILE_CCITT:				rFormat = "TIFF  CCITT";
										break;

		case FILE_LEAD1BIT:				rFormat = "LEAD 1 bit, lossless compression";
										break;

		case FILE_CCITT_GROUP3_1DIM:	rFormat = "CCITT Group3 one dimension";
										break;

		case FILE_CCITT_GROUP3_2DIM:	rFormat = "CCITT Group3 two dimensions";
										break;

		case FILE_CCITT_GROUP4:			rFormat = "CCITT Group4 two dimensions";
										break;

		case FILE_LEAD1BITA:			rFormat = "old LEAD 1 bit, lossless compression";
										break;

		case FILE_PNG:					rFormat = "Portable Network Graphic";
										break;

		default:						rFormat.Empty();
										rFormat.Format("Lead Type = %d", iFormat);
										break;
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::GetHandleFromTag()
//
// 	Description:	This function is called to get the handle of the annotation
//					object that has the tag specified by the caller
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
HANNOBJECT CTMLead::GetHandleFromTag(DWORD dwTag)
{
	SGetAnnHandle Params;

	//	Initialize the parameter structure
	Params.dwTag = dwTag;
	Params.hAnn  = 0;

	//	Do we have a valid container?
	if(GetAnnContainer())
	{
		//	Enumerate the container in search of the requested annotation
		L_AnnEnumerate((void*)GetAnnContainer(), (ANNENUMCALLBACK)GetAnnHandle,
					   &Params, ANNFLAG_RECURSE | ANNFLAG_NOTCONTAINER, NULL);
	}

	return Params.hAnn;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetHandler()
//
// 	Description:	This function allows the caller to get the error handler
//
// 	Returns:		A pointer to the error handler
//
//	Notes:			None
//
//==============================================================================
CErrorHandler* CTMLead::GetHandler() 
{
	return m_pErrors;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetImageHeight()
//
// 	Description:	This function is called to retrieve the height of the 
//					current image.
//
// 	Returns:		The image height
//
//	Notes:			None
//
//==============================================================================
float CTMLead::GetImageHeight() 
{
	return m_fImageHeight;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetImageProperties()
//
// 	Description:	This function is called to get the set of properties 
//					associated with the current image.
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMLead::GetImageProperties(STMVImageProperties* pProperties) 
{
	FILEINFO	FileInfo;
	float		fXInches;
	float		fYInches;
	CString		strFormat;

	ASSERT(pProperties);
	memset(pProperties, 0, sizeof(STMVImageProperties));

	//	Do we have an image loaded in this pane?
	if(IsLoaded())
	{
		//	Store the filename
		LSTRCPYN(pProperties->szFilename, m_strFilename);

		//	Get the image information
		if(L_FileInfo(pProperties->szFilename, &FileInfo, sizeof(FileInfo), FILEINFO_TOTALPAGES, NULL) == SUCCESS)
		{
			//	Calculate the dimensions in inches
			fXInches = (float)FileInfo.Width / (float)FileInfo.XResolution;
			fYInches = (float)FileInfo.Height / (float)FileInfo.YResolution;

			GetFormatText(FileInfo.Format, strFormat);
			
			//	Set the structure members
			sprintf_s(pProperties->szDimPixels, sizeof(pProperties->szDimPixels), "%d x %d pixels", FileInfo.Width, FileInfo.Height);
			sprintf_s(pProperties->szDimInches, sizeof(pProperties->szDimInches), "%.2f x %.2f inches", fXInches, fYInches);
			sprintf_s(pProperties->szBitsPerPixel, sizeof(pProperties->szBitsPerPixel), "%d", GetBitmapBits());
			sprintf_s(pProperties->szDiskSize, sizeof(pProperties->szDiskSize), "%ld bytes", FileInfo.SizeDisk);
			sprintf_s(pProperties->szRamSize, sizeof(pProperties->szRamSize), "%ld bytes", FileInfo.SizeMem);
			sprintf_s(pProperties->szPage, sizeof(pProperties->szPage), "%d of %d", FileInfo.PageNumber, FileInfo.TotalPages);
			sprintf_s(pProperties->szCompression, sizeof(pProperties->szCompression), "%s", FileInfo.Compression);
			LSTRCPYN(pProperties->szType, strFormat);
		}
	}
	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetImageWidth()
//
// 	Description:	This function is called to retrieve the Width of the 
//					current image.
//
// 	Returns:		The image Width
//
//	Notes:			None
//
//==============================================================================
float CTMLead::GetImageWidth() 
{
	return m_fImageWidth;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetLeadError()
//
// 	Description:	This function is called to get the last LeadTools error code
//
// 	Returns:		The last known LeadTools error
//
//	Notes:			None
//
//==============================================================================
short CTMLead::GetLeadError() 
{
	return m_sLeadError;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetPageCount()
//
// 	Description:	This function allows the caller to retrieve the 
//					number of pages in the current image file.
//
// 	Returns:		The number of pages
//
//	Notes:			None
//
//==============================================================================
short CTMLead::GetPageCount() 
{
	return m_sPages;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetPanCallouts()
//
// 	Description:	This function is called to get flag used to control whether
//					or not callouts can be panned
//
// 	Returns:		The current flag
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLead::GetPanCallouts() 
{
	return m_bPanCallouts;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetPane ScreenRect()
//
// 	Description:	This function is called to get the coordinates of the 
//					pane or max rectangle in screen coordinates
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLead::GetPaneScreenRect(RECT* prcRect, BOOL bMax)
{
	bool bSuccessful = FALSE;

	//	Are we looking for the maximum bounding rectangle?
	if(bMax == TRUE)
	{
		if((GetParent() != NULL) && (IsWindow(GetParent()->m_hWnd)))
		{
			//	NOTE:	m_rcMax.right = width (not absolute coordinate)
			//			m_rcMax.bottom = height (not absolute coordinate)
			prcRect->left   = m_rcMax.left;
			prcRect->top    = m_rcMax.top;
			prcRect->right  = m_rcMax.left + m_rcMax.right;
			prcRect->bottom = m_rcMax.top + m_rcMax.bottom;
			GetParent()->ClientToScreen(prcRect);
			bSuccessful = TRUE;
		}

	}
	else
	{
		if(IsWindow(m_hWnd))
		{
			::GetWindowRect(m_hWnd, prcRect);
			bSuccessful = TRUE;
		}

	}// if(bMax == TRUE)

	return bSuccessful;

}

//==============================================================================
//
// 	Function Name:	CTMLead::GetPanStates()
//
// 	Description:	This function is called to determine if the image can be
//					panned in any direction.
//
// 	Returns:		A packed value to indicate the directional states
//
//	Notes:			None
//
//==============================================================================
short CTMLead::GetPanStates()
{
	short	sStates = 0;
	float	fDstTop;
	float	fDstLeft;
	float	fDstHeight;
	float	fDstWidth;

	//  Find the current size and position of the destination rectangle.
	fDstLeft   = GetDstLeft();
	fDstTop	   = GetDstTop();
	fDstWidth  = GetDstWidth();
	fDstHeight = GetDstHeight();

	//	Can we move to the left?
	if(fDstLeft < 0)
		sStates |= ENABLE_PANLEFT;

	//	Can we move to the right?
	if(fDstLeft > ((float)m_iWidth - fDstWidth))
		sStates |= ENABLE_PANRIGHT;

	//	Can we move to up?
	if(fDstTop < 0)
		sStates |= ENABLE_PANUP;

	//	Can we move down?
	if(fDstTop > ((float)m_iHeight - fDstHeight))
		sStates |= ENABLE_PANDOWN;

	return sStates;

}

//==============================================================================
//
// 	Function Name:	CTMLead::GetPrintBorder()
//
// 	Description:	This function is called to retrieve the current print 
//					border setting.
//
// 	Returns:		The current print border flag
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLead::GetPrintBorder() 
{
	return m_bPrintBorder;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetPrintBorderColor()
//
// 	Description:	This function is called to retrieve the current print 
//					border color.
//
// 	Returns:		The current print border color
//
//	Notes:			None
//
//==============================================================================
COLORREF CTMLead::GetPrintBorderColor() 
{
	return m_crPrintBorder;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetPrintBorderThickness()
//
// 	Description:	This function is called to retrieve the current print 
//					border thickness.
//
// 	Returns:		The current print border thickness
//
//	Notes:			None
//
//==============================================================================
float CTMLead::GetPrintBorderThickness() 
{
	return m_fPrintBorderThickness;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetPrintCalloutBorders()
//
// 	Description:	This function is called to retrieve the current print 
//					border setting.
//
// 	Returns:		The current print callout borders flag
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLead::GetPrintCalloutBorders() 
{
	return m_bPrintCalloutBorders;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetRenderAsDIB()
//
// 	Description:	This function is called to determine if the print job should
//					be rendered as a DIB when using the specified device contect
//
// 	Returns:		TRUE if rendering as DIB is supported by the context
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLead::GetRenderAsDIB(HDC hdc, int iPrintWidth, int iPrintHeight) 
{
	BOOL	bAsDIB = FALSE;
	float	fSrcWidth = GetBitmapWidth();
	float	fDevWidth = (float)iPrintWidth;
	CDC*	pdc = CDC::FromHandle(hdc);

	ASSERT(hdc != NULL);
	ASSERT(pdc != NULL);

	//	Make sure DIB printing has not been disabled by the host application
	if(m_bDIBPrintingEnabled == TRUE)
	{
		//	Does the device support DIB printing?
		if((pdc->GetDeviceCaps(RASTERCAPS) & RC_STRETCHDIB) != 0)
		{
			//	Should we check the scaling to optimize the decision?
			if((fSrcWidth > 0) && (fDevWidth > 0))
			{
				//	Using the device is best when we are scaling to a larger size
				//
				//	NOTE:	Because we always keep the print rectangle to maintain
				//			the same aspect ratio as the image, we only have to check
				//			one dimension
				bAsDIB = ((fDevWidth / fSrcWidth) > 0.85);
			}
			else
			{			
				bAsDIB = TRUE;
			}

		}// if((pdc->GetDeviceCaps(RASTERCAPS) & RC_STRETCHDIB) != 0)

	}// if(m_bDIBPrintingEnabled == TRUE)

	return bAsDIB;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetRenderAsDIB()
//
// 	Description:	This function is called to determine if the print job should
//					be rendered as a DIB when using the specified device contect
//
// 	Returns:		TRUE if rendering as DIB is supported by the context
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLead::GetRenderAsDIB(HDC hdc, RECT* prcPrint) 
{
	int	iWidth = 0;
	int iHeight = 0;

	ASSERT(hdc);

	if(prcPrint != NULL)
	{
		iWidth = prcPrint->right - prcPrint->left;
		iHeight = prcPrint->bottom - prcPrint->top;
	}

	return GetRenderAsDIB(hdc, iWidth, iHeight);
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetResizeCallouts()
//
// 	Description:	This function is called to get flag used to control whether
//					or not callouts can be resized
//
// 	Returns:		The current flag
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLead::GetResizeCallouts() 
{
	return m_bResizeCallouts;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetSaveQFactor()
//
// 	Description:	This function is called to get the Q factor to be used to
//					save the current bitmap.
//
// 	Returns:		The appropriate Q factor
//
//	Notes:			None
//
//==============================================================================
short CTMLead::GetSaveQFactor()
{
	short	sQFactor = 0;

	//	Should we specify a QFactor value?
	//
	//	NOTE:	LeadTools raises an exception if we specify a Q factor for some
	//			formats (such as png) that do not support it
	switch(GetInfoFormat())
	{
		case FILE_CMP:
		case FILE_JFIF:
		case FILE_LEAD2JFIF:
		case FILE_LEAD1JFIF:
		case FILE_JTIF:
		case FILE_LEAD2JTIF:
		case FILE_LEAD1JTIF:
		case FILE_EXIF_JPEG:
		case FILE_EXIF_JPEG_411:
		case FILE_JBIG:
		//case FILE_DICOM_JPEG_GRAY:
		//case FILE_DICOM_JPEG_COLOR:
	
			sQFactor = m_sQFactor;
			break;

		default:

			sQFactor = 0;
			break;

	}

	return sQFactor;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetScratchCopy()
//
// 	Description:	This function is called to copy the current bitmap into 
//					the control's scratch pane
//
// 	Returns:		A pointer to the scratch pane if successful
//
//	Notes:			None
//
//==============================================================================
CTMLead* CTMLead::GetScratchCopy() 
{
	CTMLead*		pScratch = 0;
	HGLOBAL			hAnnMem  = 0;
	long			lAnnSize = 0;
	RECT			rcRect;
	RECT			rcClip;

	//	Do we have a bitmap to work with?	
	ASSERT(GetBitmap() != 0);
	if(GetBitmap() == 0)
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
	pScratch->SetBitmap(GetBitmap());

	//	Set the source rectangles
	GetSrcRects(&rcRect, &rcClip);
	pScratch->SetSrcRects(&rcRect, &rcClip);

	//	Set the destination rectangles
	GetDstRects(&rcRect, &rcClip);
	pScratch->SetDstRects(&rcRect, &rcClip);

	//	Do we have any annotations?
	if((GetAnnContainer() != 0) && (m_Annotations.GetCount() > 0))
	{
		//	Save the annotations to memory
		AnnSaveMemory((long FAR *)&hAnnMem, ANNFMT_XML, FALSE, &lAnnSize, SAVE_OVERWRITE, 1);

		//	Set the annotations for the scratch pane
		if((hAnnMem != 0) && (lAnnSize > 0))
		{
			pScratch->SetAnnotations(hAnnMem, lAnnSize);

			//	Release the annotation buffer
			GlobalFree(hAnnMem);
		}
	}

	return pScratch;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetScreenHeight()
//
// 	Description:	This function is called to get the height of the screen in
//					pixels.
//
// 	Returns:		The screen width
//
//	Notes:			None
//
//==============================================================================
int CTMLead::GetScreenHeight()
{
	return GetSystemMetrics(SM_CYSCREEN);
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetScreenRatio()
//
// 	Description:	This function is called to get the aspect ratio of the 
//					screen.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
double CTMLead::GetScreenRatio() 
{
	//	Get the aspect ratio of the screen
	if(GetScreenHeight() != 0)
		return ((double)GetScreenWidth() / (double)GetScreenHeight());
	else
		return (4.0 / 3.0);	//	default ratio
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetScreenWidth()
//
// 	Description:	This function is called to get the width of the screen in
//					pixels.
//
// 	Returns:		The screen width
//
//	Notes:			None
//
//==============================================================================
int CTMLead::GetScreenWidth()
{
	return GetSystemMetrics(SM_CXSCREEN);
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetSelections()
//
// 	Description:	This function will get the array of annotation objects 
//					representing the current selections
//
// 	Returns:		The number of selections
//
//	Notes:			It is up to the caller to deallocate the array of objects
//
//==============================================================================
UINT CTMLead::GetSelections(HANNOBJECT** pSelections)
{
	HANNOBJECT	hContainer = (HANNOBJECT)GetAnnContainer();
	UINT		uSelections;

	ASSERT(pSelections);
	*pSelections = 0;

	//	Is there an image?
	if(!IsLoaded())
		return 0;

	//	Do we have a valid container?
	if(!hContainer)
		return 0;

	//	How many annotations are selected?
	if(L_AnnGetSelectCount(hContainer, &uSelections) != SUCCESS)
		return 0;
	if(uSelections == 0)
		return 0;

	//	Allocate memory for the selection handles
	*pSelections = (HANNOBJECT*)HeapAlloc(GetProcessHeap(), HEAP_ZERO_MEMORY,
										 (sizeof(HANNOBJECT) * uSelections));
	if(*pSelections == 0)
		return 0;

	//	Get the array of selections
	if(L_AnnGetSelectItems(hContainer, *pSelections) != SUCCESS)
	{
		HeapFree(GetProcessHeap(), 0, *pSelections);
		return 0;
	}
	return uSelections;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetSrcAspect()
//
// 	Description:	This function is called to get the aspect ratio of the
//					current source rectangle
//
// 	Returns:		The ratio of the source height to width
//
//	Notes:			None
//
//==============================================================================
float CTMLead::GetSrcAspect() 
{
	//	Do we have an image?
	if(!IsLoaded())
		return 0;

	return (GetSrcHeight() / GetSrcWidth());
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetSrcRects()
//
// 	Description:	This function is called to get the source and source 
//					clip rectangles
//
// 	Returns:		None
//
//	Notes:			RECT.right  = width (not right hand coordinate)
//					RECT.bottom = height (not bottom coordinate)
//
//==============================================================================
void CTMLead::GetSrcRects(RECT* pSrc, RECT* pClip) 
{
	//	Do we have an image?
	if(!IsLoaded())
	{
		memset(pSrc, 0, sizeof(RECT));
		memset(pClip, 0, sizeof(RECT));
	}
	else
	{
		//	Get the coordinates of the destination rectangle
		if(pSrc)
		{
			pSrc->top    = (long)GetSrcTop();
			pSrc->left   = (long)GetSrcLeft();
			pSrc->bottom = (long)GetSrcHeight();
			pSrc->right  = (long)GetSrcWidth();
		}
	
		//	Get the coordinates of the destination clip rectangle
		if(pClip)
		{
			pClip->top    = (long)GetSrcClipTop();
			pClip->left   = (long)GetSrcClipLeft();
			pClip->bottom = (long)GetSrcClipHeight();
			pClip->right  = (long)GetSrcClipWidth();
		}
	
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::GetSrcVisible()
//
// 	Description:	This function is called to get the rectangle that represents
//					the portion of the source that is currently visible
//
// 	Returns:		TRUE if the whole source is visible
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLead::GetSrcVisible(ANNRECT* pAnnSrc) 
{
	//	Do we have an image?
	if(!IsLoaded())
	{
		memset(pAnnSrc, 0, sizeof(ANNRECT));
		return FALSE;
	}
	else
	{
		//	Is the full source image visible?
		//
		//	NOTE:	When zoomed in on the image, the destination rectangle is
		//			always larger than client window
		if((GetDstLeft() == 0) && (GetDstTop() == 0) &&
		   (GetDstWidth() <= m_iWidth) && (GetDstHeight() <= m_iHeight))
		{
			pAnnSrc->left    = 0;
			pAnnSrc->top     = 0;
			pAnnSrc->right	 = GetSrcWidth();
			pAnnSrc->bottom  = GetSrcHeight();
			return TRUE;
		}
		else
		{
			//	Initialize with the current window coordinates
			pAnnSrc->left    = 0;
			pAnnSrc->top     = 0;
			pAnnSrc->right	 = (float)m_iWidth;
			pAnnSrc->bottom  = (float)m_iHeight;

			//	Convert from client coordinates to source coordinates
			ClientToSource(pAnnSrc);

			return FALSE;
		
		}
			
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::GetSrcVisible()
//
// 	Description:	This function is called to get the rectangle that represents
//					the portion of the source that is currently visible
//
// 	Returns:		TRUE if the whole source is visible
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLead::GetSrcVisible(RECT* pSrc) 
{
	BOOL	bFullVisible = FALSE;
	ANNRECT	rcSrc;

	rcSrc.left   = (double)(pSrc->left);
	rcSrc.top    = (double)(pSrc->top);
	rcSrc.right  = (double)(pSrc->right);
	rcSrc.bottom = (double)(pSrc->bottom);

	bFullVisible = GetSrcVisible(&rcSrc);

	pSrc->left   = (long)(rcSrc.left);
	pSrc->top    = (long)(rcSrc.top);
	pSrc->right  = (long)(rcSrc.right);
	pSrc->bottom = (long)(rcSrc.bottom);

	return bFullVisible;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetTagFromHandle()
//
// 	Description:	This function is called to get the tag of the annotation
//					object that has the handle specified by the caller
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
DWORD CTMLead::GetTagFromHandle(HANNOBJECT hAnn)
{
	DWORD dwTag;

	//	Get the tag
	if(L_AnnGetTag(hAnn, (L_UINT32*)(&dwTag)) == SUCCESS)
		return dwTag;
	else
		return 0;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetWndAspect()
//
// 	Description:	This function is called to get the aspect ratio of the
//					window
//
// 	Returns:		The ratio of the window height to width
//
//	Notes:			None
//
//==============================================================================
float CTMLead::GetWndAspect() 
{
	RECT	rcWnd;
	float	fRatio = 0;

	if(IsWindow(m_hWnd))
	{
		GetClientRect(&rcWnd);
		if(rcWnd.right - rcWnd.left != 0)
			fRatio = ((float)(rcWnd.bottom - rcWnd.top) / (float)(rcWnd.right - rcWnd.left));
	}
	return fRatio;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetZoomCallouts()
//
// 	Description:	This function is called to get flag used to control whether
//					or not callouts can be zoomed
//
// 	Returns:		The current flag
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLead::GetZoomCallouts() 
{
	return m_bZoomCallouts;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetZoomFactor()
//
// 	Description:	This function allows the caller to retrieve the zoom factor
//					applied to the current image. 
//
// 	Returns:		The current zoom factor.
//
//	Notes:			None
//
//==============================================================================
float CTMLead::GetZoomFactor() 
{
	return m_fZoomFactor;
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetZoomSate()
//
// 	Description:	This function allows the caller to retrieve the zoom state
//					of the current image. 
//
// 	Returns:		The current zoom state.
//
//	Notes:			None
//
//==============================================================================
short CTMLead::GetZoomState() 
{
	return m_sZoomState;
}

//==============================================================================
//
// 	Function Name:	CTMLead::HandleFileError()
//
// 	Description:	This function is called when a Lead Tools file error occurs.
//					It will translate the Lead error level to an appropriate
//					TMView error and display an appropriate error message.
//
// 	Returns:		The associated TMV error level
//
//	Notes:			We have to hard code the constants because there is a 
//					conflict in the Lead Tools headers.		
//
//==============================================================================
short CTMLead::HandleFileError(LPCTSTR lpszFile, short sLeadError) 
{
	CString strError;

	//	Is this an error?
	if(sLeadError == 0)
		return TMV_NOERROR;

	//	What Lead tool error occurred?
	switch(sLeadError)
	{
		case 20002:		
		
			if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_NOIMAGE);
			return TMV_NOIMAGE;

		case 20003:

			if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_LOWMEMORY, lpszFile);
			return TMV_LOWMEMORY;

		case 20007:
		case 20073:
		case 20074:
		case 20075:
		case 20076:
		case 20077:

			if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_FILEREAD, lpszFile);
			return TMV_FILEREAD;

		case 20008:

			if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_INVALIDNAME, lpszFile);
			return TMV_INVALIDNAME;

		case 20009:
		case 20016:

			if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_FILEFORMAT, lpszFile);
			return TMV_FILEFORMAT;

		case 20010:

			if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_FILENOTFOUND, lpszFile);
			return TMV_FILENOTFOUND;

		case 20012:

			if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_INVALIDSUBTYPE, lpszFile);
			return TMV_INVALIDSUBTYPE;

		case 20014:

			if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_FILEOPEN, lpszFile);
			return TMV_FILEOPEN;

		case 20015:

			if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_COMPRESSION, lpszFile);
			return TMV_COMPRESSION;

		case 20146:

			if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_LZWLOCKED, lpszFile);
			return TMV_LZWLOCKED;

		default:					
		
			strError.Format("%d", sLeadError);
			if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_UNKNOWNERROR, strError);
			return TMV_UNKNOWNERROR;


	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::HandleLeadError()
//
// 	Description:	This function is called to handle a LeadTools error code
//
// 	Returns:		The LeadTools error code provided by the caller
//
//	Notes:			None
//
//==============================================================================
short CTMLead::HandleLeadError(short sError) 
{
	CString	strMsg;

	//	Save the error identifier
	m_sLeadError = sError;

	//	Should we display an error message?
	if(sError != 0)
	{
		strMsg.Format("LeadTools Error - #%d", sError);
		if(m_pErrors != 0) m_pErrors->Handle(0, strMsg);
	}

	return m_sLeadError;
}

//==============================================================================
//
// 	Function Name:	CTMLead::HolePunchRemove()
//
// 	Description:	This function is called to remove hole punches from a 1 bit 
//					image.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMLead::HolePunchRemove(long lMinWidth, long lMinHeight, long lMaxWidth,
						       long lMaxHeight, long lLocation)
{
	if(!IsLoaded())
	{
		return TMV_NOIMAGE;
	}
	else
	{
		//	Set this property to make sure the borders get removed
		SetDocCleanSuccess(SUCCESS_REMOVE);

		if((m_sLeadError = CLead::HolePunchRemove(HOLEPUNCH_USE_SIZE | HOLEPUNCH_USE_DPI | HOLEPUNCH_USE_LOCATION,
												  0, 0,
												  lMinWidth, lMinHeight,
												  lMaxWidth, lMaxHeight,
												  lLocation)) != 0)
		{
			HandleLeadError(m_sLeadError);
			return TMV_LEADERROR;
		}
	
		//	Repaint the image
		CLead::ForceRepaint();

		return TMV_NOERROR;
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::HolePunchRemove2()
//
// 	Description:	This function is called to remove hole punches from a 1 bit 
//					image.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMLead::HolePunchRemove2(long lMinHoles, long lMaxHoles, long lLocation)
{
	if(!IsLoaded())
	{
		return TMV_NOIMAGE;
	}
	else
	{
		//	Set this property to make sure the borders get removed
		SetDocCleanSuccess(SUCCESS_REMOVE);

		if((m_sLeadError = CLead::HolePunchRemove(HOLEPUNCH_USE_COUNT | HOLEPUNCH_USE_LOCATION,
												  lMinHoles, lMaxHoles,
												  0, 0,
												  0, 0,
												  lLocation)) != 0)
		{
			HandleLeadError(m_sLeadError);
			return TMV_LEADERROR;
		}
	
		//	Repaint the image
		CLead::ForceRepaint();

		return TMV_NOERROR;
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::IsAnimation()
//
// 	Description:	This function is called to determine if the current image
//					is an animation.
//
// 	Returns:		TRUE if it is an animation.
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLead::IsAnimation()
{
    return m_bAnimation;
}

//==============================================================================
//
// 	Function Name:	CTMLead::IsLoaded()
//
// 	Description:	This function is called to determine if a file is loaded.
//
// 	Returns:		TRUE if loaded.
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLead::IsLoaded()
{
    return (GetBitmap() != 0);
}

//==============================================================================
//
// 	Function Name:	CTMLead::IsPlaying()
//
// 	Description:	This function is called to determine if the animation
//					is playing.
//
// 	Returns:		TRUE if playing animation.
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLead::IsPlaying()
{
    return m_bPlayingAnimation;
}

//==============================================================================
//
// 	Function Name:	CTMLead::IsPostScript()
//
// 	Description:	This function is called to determine if the device context
//					is associated with a postscript printer.
//
// 	Returns:		TRUE if postscript
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLead::IsPostScript(HDC hdc)
{
	int		nEscapeCode;
	TCHAR	szTechnology[MAX_PATH] = TEXT("");

	// If it supports POSTSCRIPT_PASSTHROUGH, it must be PS.
	nEscapeCode = POSTSCRIPT_PASSTHROUGH;
	if(ExtEscape(hdc, QUERYESCSUPPORT, sizeof(int), (LPCSTR)&nEscapeCode, 0, NULL) > 0)
		return TRUE;

	// If it doesn't support GETTECHNOLOGY, we won't be able to tell.
	nEscapeCode = GETTECHNOLOGY;
	if(ExtEscape(hdc, QUERYESCSUPPORT, sizeof(int), (LPCSTR)&nEscapeCode, 0, NULL ) <= 0)
		return FALSE;

	// Get the technology string and check to see if the word "postscript" is in it.
	if(ExtEscape(hdc, GETTECHNOLOGY, 0, NULL, MAX_PATH, (LPSTR)szTechnology) <= 0)
		return FALSE;
	_strupr_s(szTechnology, sizeof(szTechnology));
	if(strstr(szTechnology, "POSTSCRIPT") == NULL)
		return FALSE;

	// The word "postscript" was not found and it didn't support 
	//   POSTSCRIPT_PASSTHROUGH, so it's not a PS printer.
	return FALSE;
} 

//==============================================================================
//
// 	Function Name:	CTMLead::LastPage()
//
// 	Description:	This external method allows the caller to go directly to the
//					last page of a multipage image
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMLead::LastPage() 
{
	short sLeadError;

	//	Are we already on the last page?
	if((m_sPages <= 1) || (m_sPage == m_sPages))
		return TMV_NOERROR;

	//	Attempt to load the last page
	if((sLeadError = LoadImage(m_strFilename, m_sPages)) == TMV_NOERROR)
	{
		Draw();
		return TMV_NOERROR;
	}
	else
	{
		return HandleFileError(m_strFilename, sLeadError);
	}
	
}

//==============================================================================
//
// 	Function Name:	CTMLead::LoadCallout()
//
// 	Description:	This function create a callout based on a specification
//					stored in the zap file
//
// 	Returns:		TRUE if successful
//
//	Notes:			This function assumes the file pointer is positioned on the
//					header for the first callout that is to be loaded
//
//==============================================================================
void CTMLead::LoadCallout(SZapHeader* pHeader, SZapCallout* pZap, 
						  HGLOBAL hAnnMem) 
{
	CCallout*	pCallout;
	RECT		rcPosition;
	RECT		rcContainer;

	ASSERT(m_pControl);
	ASSERT(pZap);
	
	//	Do we have a list for callouts?
	if(!m_pCallouts) return;

	//	Allocate a new callout
	pCallout = new CCallout(m_pControl, this);
	ASSERT(pCallout);
	if(!pCallout) return;

	//	Was the callout window created successfully
	if(!IsWindow(pCallout->m_hWnd))
	{
		delete pCallout;
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_NOCALLOUTWND);
		return;
	}

	//	Add it to the list
	m_pCallouts->Add(pCallout);

	//	Set the annotation identifier
	pCallout->SetAnnId(pZap->wAnnId);

	//	Set the flags
	pCallout->SetShaded((pZap->wFlags & CALLOUT_ZAP_SHADED) != 0);

	//	Get the callout rectangles stored in the zap file
	memcpy(&(pCallout->m_rcDst), &(pZap->rcDstRect), sizeof(pCallout->m_rcDst));
	memcpy(&(pCallout->m_rcRubberBand), &(pZap->rcRubberBand), sizeof(pCallout->m_rcRubberBand));
	memcpy(&rcPosition, &(pZap->rcPosition), sizeof(rcPosition));

//MsgBox(pZap, "");

	//	Are we loading into split screen?
	if(m_bSplitScreen)
	{
		//	Use the whole control as the current container
		ASSERT(m_pControl);
		m_pControl->GetWindowRect(&rcContainer);
	}
	else
	{
		//	Use the maximum rectangle as the current container
		//
		//	NOTE:	m_rcMax.right = width (not absolute coordinate)
		//			m_rcMax.bottom = height (not absolute coordinate)

		rcContainer.left = m_rcMax.left;
		rcContainer.top = m_rcMax.top;
		rcContainer.right = m_rcMax.left + m_rcMax.right;
		rcContainer.bottom = m_rcMax.top + m_rcMax.bottom;
		GetParent()->ClientToScreen(&rcContainer);
	}

	//	Initialize the rectangles used to rescale the callout
	pCallout->SetOriginalContainer(&m_rcZapControl);
	pCallout->SetOriginalPosition(&rcPosition);
	pCallout->SetContainer(&rcContainer);

	//	Rescale to the current container
	pCallout->Rescale();

	//	Set the callout annotations
	pCallout->SetAnnotations(hAnnMem, pZap->lAnnBytes);

	//	Notify the control
	if(m_pControl)
		m_pControl->OnCalloutCreated(this, pCallout);
}

//==============================================================================
//
// 	Function Name:	CTMLead::LoadCallouts()
//
// 	Description:	This function will load the callouts specified in the zap
//					file provided by the caller
//
// 	Returns:		TRUE if successful
//
//	Notes:			This function assumes the file pointer is positioned on the
//					header for the first callout that is to be loaded
//
//==============================================================================
BOOL CTMLead::LoadCallouts(CFile* pFile, SZapHeader* pZap, SZapPane* pPane, 
						   BOOL bVisible) 
{
	SZapCallout	Header;
	HGLOBAL		hAnnMem;
	BOOL		bSync;

	ASSERT(m_pControl);
	ASSERT(pFile);
	ASSERT(pFile->m_hFile != CFile::hFileNull);
	ASSERT(pPane);
	
	//	Check the caller's parameters just to be safe
	if(!m_pControl || !pFile || !pPane || pFile->m_hFile == CFile::hFileNull)
		return FALSE;

	//	Are there any callouts to load?
	if(pPane->sCallouts <= 0)
		return TRUE;

	//	Do we have a list for callouts?
	if(!m_pCallouts)
		return FALSE;

	//	Delete any existing callouts
	DestroyCallouts();

	//	Disable syncronization of callout annotations while we load the callouts
	bSync = m_bSyncCalloutAnn;
	m_bSyncCalloutAnn = FALSE;

	//	Read each callout specification
	for(int i = 0; i < pPane->sCallouts; i++)
	{
		//	Read the zap descriptor from the file
		if(!ReadZapCallout(pFile, &Header, &hAnnMem))
			break;

		//	Load the callout
		LoadCallout(pZap, &Header, hAnnMem);

		//	Deallocate the annotation memory
		if(hAnnMem)
		{
			GlobalFree(hAnnMem);
			hAnnMem = 0;
		}
	}

	//	Restore the syncronization property
	m_bSyncCalloutAnn = bSync;

	//	If the annotations are synchronized, we have to reset all the links
	if(m_bSyncCalloutAnn)
		ResyncAnnotations();

	//	Should we display the callouts?
	if(bVisible)
		ShowCallouts(TRUE);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTMLead::LoadImage()
//
// 	Description:	This function will load the desired page of the specified
//					file. If neccessary it will set up an array of annotation
//					objects for the new file. It will save the annotations
//					associated with the current page.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			If the file is being changed, sPage should be less than zero
//
//==============================================================================
short CTMLead::LoadImage(LPCTSTR lpszFile, short sPage)
{
    short sLeadError;

	//	Make sure the local lists have been cleared out
	DestroyCallouts();
	m_Annotations.Flush(TRUE);

	//	Reset the zap parameters
	ResetZapParameters();

	//  If a page number is not specified, get the page count for this
    //  image file
    if(sPage < 0)
    {
        //  Free all existing annotation memory
		Erase(TRUE);
        if(m_pAnnMemory != 0)
        {
            for(int i = 0; i < (m_sPages + 1); i++)
                if(m_pAnnMemory[i].hMemory != 0)
                    GlobalFree(m_pAnnMemory[i].hMemory);
            delete [] m_pAnnMemory;
            m_pAnnMemory = 0;
        }

        //  Disable the error message while we check the file
        SetEnableMethodErrors(FALSE);   
            
        //  Request information for the highest page number possible. This
        //  will set the PageInfo property to the number of the highest page
        //  in the file
		//
		//	MODIFIED IN UPGRADE FROM 11 TO 12
        GetFileInfo(lpszFile, SHRT_MAX, FILEINFO_TOTALPAGES);
        m_sPages = (short)GetInfoTotalPages();

		//	Is this file an animation?
		m_bAnimation = GetInfoAnimation();

        //  Allocate array of annotation memory specifications. Add one extra
        //  structure so we can use the page number as the index
        m_pAnnMemory = new SAnnMemory[(m_sPages + 1)];
        if(m_pAnnMemory != 0)
            memset(m_pAnnMemory, 0, (sizeof(SAnnMemory) * (m_sPages + 1)));

        //  Set the desired page number to the first page in the image
        sPage = 1;

    }
    
    //  Save the annotation objects for the current page to memory
    if(m_pAnnMemory != 0)
    {   
        //  Get the annotation container for this image if it exists
        HANNOBJECT hContainer = (HANNOBJECT)GetAnnContainer();
        
        //  Save the annotations in memory
        if(hContainer != 0)
            L_AnnSaveMemory(hContainer, ANNFMT_XML,
                            FALSE, &(m_pAnnMemory[m_sPage].hMemory),
                            &(m_pAnnMemory[m_sPage].ulSize), NULL);
    }
    
	//  Load the image
    if((sLeadError = Load(lpszFile, 0, sPage, -1)) == 0)
    {
		//	Disable bitmap transparency
		//
		//	NOTE:	This was added to fix a bug discovered with 8-bit PNG
		//			images. If this flag is not disabled, the backgrounds
		//			are not properly rendered.
		SetBitmapEnableTransparency(FALSE);
		
		//  Update the page index
        m_sPage = sPage;

		//	Reset the cumulative rotation
		m_sAngle = 0;

        //  Calculate the size and aspect ratio of this image
        m_fImageHeight = GetSrcHeight();
        m_fImageWidth  = GetSrcWidth();
        m_fAspectRatio = m_fImageHeight / m_fImageWidth;
        
        //  Load annotation objects for this page from memory if they exist
        if(m_pAnnMemory && (m_pAnnMemory[m_sPage].hMemory != 0))
        {
            //	Set the annotations
			SetAnnotations(m_pAnnMemory[m_sPage].hMemory,
						   m_pAnnMemory[m_sPage].ulSize);

            //  Now free up the memory
            GlobalFree(m_pAnnMemory[m_sPage].hMemory);
            m_pAnnMemory[m_sPage].hMemory = 0;
            m_pAnnMemory[m_sPage].ulSize = 0;

        }

		//	Initialize the destination rectangle to match the source rectangles
		SetDstRect(0.0f, 0.0f, m_fImageWidth, m_fImageHeight);
		SetDstClipRect(0.0f, 0.0f, m_fImageWidth, m_fImageHeight);

        //  Renable the error messages
        SetEnableMethodErrors(TRUE);   
    
		//	The image is now loaded
		m_bLoaded = TRUE;
		return TMV_NOERROR;
    
    }
    else
    {
        //  Clear the page counters
        m_sPage = 0;
        m_sPages = 0;

        //  Renable the error messages
        SetEnableMethodErrors(TRUE);   
    
		m_fZoomFactor = 1.0f;
        m_bLoaded = FALSE;
		m_bAnimation = FALSE;
		m_sZoomState = ZOOMED_NONE;
		m_sAngle = 0;

		return sLeadError;
    }

}

//==============================================================================
//
// 	Function Name:	CTMLead::LoadOldCallouts()
//
// 	Description:	This function will load the callouts specified in the ini
//					file provided by the caller
//
// 	Returns:		None
//
//	Notes:			This function assumes the ini file is opened to the correct
//					section
//
//==============================================================================
void CTMLead::LoadOldCallouts(CTMIni* pIni) 
{
	CCallout*	pCallout;
	RECT		rcCall;
	RECT		rcDst;
	RECT		rcWnd;
	char		szIniStr[256];
	char		szLine[32];
	char*		pToken;
	char*		pNext;
	int			iLine = 1;
	int			iLeft;
	int			iTop;
	int			iMaxWidth;
	int			iMaxHeight;

	ASSERT(m_pControl);
	ASSERT(pIni);

	//	Do we have a list for callouts?
	if(!m_pCallouts)
		return;

	//	Delete any existing callouts
	DestroyCallouts();

	//	Get the screen extents
	iMaxWidth  = GetSystemMetrics(SM_CXSCREEN);
	iMaxHeight = GetSystemMetrics(SM_CYSCREEN);

	//	Get the rectangle of the control window
	m_pControl->GetWindowRect(&rcWnd);

	//	Adjust the rectangle to leave a margin around the callout
	rcWnd.left   += CALLOUT_MARGIN;
	rcWnd.top    += CALLOUT_MARGIN;
	rcWnd.right  -= CALLOUT_MARGIN;
	rcWnd.bottom -= CALLOUT_MARGIN;

	//	Read the callout specifications
	while(1)
	{
		//	Format the line specification
		sprintf_s(szLine, sizeof(szLine), "%s%d", CALLOUT_LINE, iLine++);

		//	Read the specification from the ini file
		pIni->ReadString(szLine, szIniStr, sizeof(szIniStr));

		//	Have we run out of callouts?
		if(lstrlen(szIniStr) == 0)
			break;

		//	Parse the callout specification
		if((pToken = strtok_s(szIniStr, ",", &pNext)) == NULL)
			continue;
		else
			rcCall.left = atoi(pToken);

		if((pToken = strtok_s(NULL, ",", &pNext)) == NULL)
			continue;
		else
			rcCall.top = atoi(pToken);

		if((pToken = strtok_s(NULL, ",", &pNext)) == NULL)
			continue;
		else
			rcCall.right = atoi(pToken);

		if((pToken = strtok_s(NULL, ",", &pNext)) == NULL)
			continue;
		else
			rcCall.bottom = atoi(pToken);

		if((pToken = strtok_s(NULL, ",", &pNext)) == NULL)
			continue;
		else
			rcDst.left = atoi(pToken);

		if((pToken = strtok_s(NULL, ",", &pNext)) == NULL)
			continue;
		else
			rcDst.top = atoi(pToken);

		if((pToken = strtok_s(NULL, ",", &pNext)) == NULL)
			continue;
		else
			rcDst.right = atoi(pToken);

		if((pToken = strtok_s(NULL, ",", &pNext)) == NULL)
			continue;
		else
			rcDst.bottom = atoi(pToken);

		if((pToken = strtok_s(NULL, ",", &pNext)) == NULL)
			continue;
		else
			iLeft = atoi(pToken);

		if((pToken = strtok_s(NULL, ",", &pNext)) == NULL)
			continue;
		else
			iTop = atoi(pToken);

		//	Allocate a new callout
		pCallout = new CCallout(m_pControl, this);
		ASSERT(pCallout);

		//	Was the callout window created successfully
		if(!IsWindow(pCallout->m_hWnd))
		{
			delete pCallout;
			if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_NOCALLOUTWND);
			continue;
		}

		//	Add it to the list
		m_pCallouts->Add(pCallout);

		//	Set the callout rectangles
		pCallout->SetRects(&rcWnd, &rcDst, &rcCall, m_sCallFrameThick);

		//	Set the top/left position of the callout using the stored
		//	coordinates. We have to make sure the screen resolution hasn't
		//	changed

		//	Make sure we're not too far left or right
		if((iLeft + pCallout->m_iWidth) < CALLOUT_MINGRABBORDER)
			iLeft = CALLOUT_MINGRABBORDER - pCallout->m_iWidth;
		else if(iLeft > (iMaxWidth - CALLOUT_MINGRABBORDER))
			iLeft = iMaxWidth - CALLOUT_MINGRABBORDER;
			
		//	Make sure we're not too far up or down
		if((iTop + pCallout->m_iHeight) < CALLOUT_MINGRABBORDER)
			iTop = CALLOUT_MINGRABBORDER - pCallout->m_iHeight;
		else if(iTop > (iMaxHeight - CALLOUT_MINGRABBORDER))
			iTop = iMaxHeight - CALLOUT_MINGRABBORDER;
			
		//	Position the callout
		pCallout->MoveWindow(iLeft, iTop, pCallout->m_iWidth, 
							 pCallout->m_iHeight, TRUE);

		//	Notify the control
		if(m_pControl)
			m_pControl->OnCalloutCreated(this, pCallout);

	}

	//	Display all the callouts at once
	ShowCallouts(TRUE);
}

//==============================================================================
//
// 	Function Name:	CTMLead::LoadOldZap()
//
// 	Description:	This function is provided to maintain support for zap files
//					created prior to revision 2.1.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMLead::LoadOldZap(LPCTSTR lpszFilename, LPCTSTR lpszSection, 
					   BOOL bUseView, BOOL bScaleView, BOOL bCallouts) 
{
	HANNOBJECT	hContainer;
	RECT		rcDst;
	char		szFilename[256];
	short		sError;
	short		sRotate;
	int			iViewWidth;
	int			iViewHeight;

	//	Open the ini file
	if(!m_Ini.Open(lpszFilename, lpszSection))
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_ZAPNOTFOUND, lpszFilename);
		return TMV_ZAPNOTFOUND;
	}
	
	//	Read the filename and rotation information
	m_Ini.ReadString(FILENAME_LINE, szFilename, sizeof(szFilename));
	sRotate = (short)m_Ini.ReadLong(ROTATION_LINE, 0);

	//	Is the filename valid?
	if(lstrlen(szFilename) == 0)
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_INVALIDZAP, lpszFilename);
		return TMV_INVALIDZAP;
	}

	//	Load from file into a new container
	L_AnnLoad((char*)lpszFilename, &hContainer, NULL);

	//	Do we need to load the file?
	if(!IsLoaded() || m_strFilename.CompareNoCase(szFilename))
	{
		//	Load the file without redrawing yet
		m_sRotation = sRotate;
		if((sError = SetFilename(szFilename, FALSE)) != TMV_NOERROR)
			return sError;
	}
	else
	{
		//	Delete any existing callouts
		DestroyCallouts();
	}

	//	If we're not using the viewport information just load the annotations
	//	and stop here
	if(!bUseView)
	{
		//	Do we need to rotate the image?
		if(m_sAngle != sRotate)
		{
			m_sRotation = sRotate - m_sAngle;
			Rotate(TRUE);
		}

		//	Prevent attempts to set the annotation properties
		m_bSetAnnProps = FALSE;

		AnnLoad(lpszFilename, 0);

		m_bSetAnnProps = TRUE;

		//	Restore the callouts if requested
		if(bCallouts)
			LoadOldCallouts(&m_Ini);

		Draw();

		return TMV_NOERROR;
	}

	//	Get the information from the ini file
	rcDst.top = (int)m_Ini.ReadLong(DSTTOP_LINE, 100);
	rcDst.left = (int)m_Ini.ReadLong(DSTLEFT_LINE, 100);
	rcDst.bottom = (int)m_Ini.ReadLong(DSTHEIGHT_LINE, -100);
	rcDst.right = (int)m_Ini.ReadLong(DSTWIDTH_LINE, -100);
	iViewWidth = (int)m_Ini.ReadLong(VIEWWIDTH_LINE, -100);
	iViewHeight = (int)m_Ini.ReadLong(VIEWHEIGHT_LINE, -100);
		
	//	Is the destination rectangle valid?
	if(rcDst.left > 0 || rcDst.top > 0 || rcDst.right <= 0 || rcDst.bottom <= 0)
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_INVALIDZAP, lpszFilename);
		return TMV_INVALIDZAP;
	}
			
	//	Do we need to rotate the image?
	if(m_sAngle != sRotate)
	{
		m_sRotation = sRotate - m_sAngle;
		Rotate(FALSE);
	}

	//	Set the destination rectangles to the stored values
	SetDstRects(&rcDst, 0);

	//	Are we scaling the stored zap to fit within the available window?
	if(bScaleView && iViewWidth > 0 && iViewWidth > 0)
	{
		m_bScaleImage = TRUE;

		//	Make it look like this window is the same size as the stored
		//	viewport
		m_iWidth  = iViewWidth;
		m_iHeight = iViewHeight;
	
	}
	else
	{
		m_bScaleImage = FALSE;
	}

	//	Redraw the image
	RedrawZap();

    //	Prevent attempts to set the annotation properties
	m_bSetAnnProps = FALSE;

	//	Load the annotations
	if(hContainer)
		SetAnnContainer((long)hContainer);
	else
		Erase(TRUE);
	
	m_bSetAnnProps = TRUE;

	//	Restore the callouts if requested
	if(bCallouts)
		LoadOldCallouts(&m_Ini);

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMLead::LoadZap()
//
// 	Description:	This function will use the information contained in the
//					zap file provided by the caller to set the state of the
//					control
//
// 	Returns:		TRUE if successful.
//
//	Notes:			This function assumes the file pointer is sitting on the
//					appropriate pane descriptor in the zap file.
//
//==============================================================================
BOOL CTMLead::LoadZap(CFile* pFile, SZapHeader* pHeader,
					  BOOL bUseView, BOOL bScaleView, 
					  BOOL bCallouts, LPCSTR lpszSourceFile) 
{
	SZapPane	Pane;
	RECT		rcDst;
	char		szErrorMsg[256];
	HGLOBAL		hAnnMem  = 0;
	LPBYTE		lpAnnMem = 0;
	char		szZapPath[256];
	char		szStoredPath[256];
	char*		pToken;
	short		sOldRotation = m_sRotation;

	ASSERT(pFile);
	ASSERT(pFile->m_hFile != CFile::hFileNull);
	if(!pFile || pFile->m_hFile == CFile::hFileNull) return FALSE;

	//	Get the path where the zap file is stored
	lstrcpyn(szZapPath, pFile->GetFilePath(), sizeof(szZapPath));
	if((pToken = strrchr(szZapPath, '\\')) != 0)
		*(pToken+1) = 0;
	else
		ZeroMemory(szZapPath, sizeof(szZapPath));

	//	Now write the information to the file
	try
	{
		//	Read the pane descriptor
		if(pFile->Read((void*)&Pane, sizeof(Pane)) != sizeof(Pane))
			return FALSE;

		//	Save the stored file specification in case we need to use it
		lstrcpyn(szStoredPath, Pane.szFilespec, sizeof(szStoredPath));

		//	Reformat the zap source file specification so that we override
		//	the stored path with the path containing the zap file. This allows
		//	for changes in the locations of zaps after they've been created
		//	assuming the zap remains stored with the base image
		if((pToken = strrchr(Pane.szFilespec, '\\')) != 0)
		{
			lstrcat(szZapPath, (pToken + 1));
			lstrcpyn(Pane.szFilespec, szZapPath, sizeof(Pane.szFilespec));
		}
		else
		{
			lstrcat(szZapPath, Pane.szFilespec);
			lstrcpyn(Pane.szFilespec, szZapPath, sizeof(Pane.szFilespec));
		}

		//	Is the file specification valid?
		if(lstrlen(Pane.szFilespec) == 0)
			return FALSE;

		//	Did the caller specify a source file path?
		if(lpszSourceFile && lstrlen(lpszSourceFile) > 0)
		{
			//	Substitute the caller's path
			lstrcpyn(Pane.szFilespec, lpszSourceFile, sizeof(Pane.szFilespec));
		}
		else
		{
			//	Use the stored path if this file does not exist
			if(!FindFile(Pane.szFilespec))
				lstrcpyn(Pane.szFilespec, szStoredPath, sizeof(Pane.szFilespec));
		}

		//	Allocate memory for the annotations if necessary
		if(Pane.lAnnBytes > 0)
		{
			if((hAnnMem = GlobalAlloc(GMEM_MOVEABLE, Pane.lAnnBytes)) == 0)
				return FALSE;

			//	Lock the memory so we can read in the file contents
			lpAnnMem = (LPBYTE)GlobalLock(hAnnMem);
			ASSERT(lpAnnMem);

			//	Read the annotations from the file
			if(pFile->Read(lpAnnMem, (UINT)Pane.lAnnBytes) != (UINT)Pane.lAnnBytes)
			{
				GlobalUnlock(hAnnMem);
				GlobalFree(hAnnMem);
				return FALSE;
			}

			//	Unlock the memory now
			GlobalUnlock(hAnnMem);
			lpAnnMem = 0;
		}

	}
	//	Catch all file exceptions
	catch(CFileException* pFileException)
	{
		//	Get the error message and pass it on to the handler
		pFileException->GetErrorMessage(szErrorMsg, sizeof(szErrorMsg));
		if(m_pErrors != 0) m_pErrors->Handle(0, szErrorMsg);
		pFileException->Delete();
		
		//	Free any memory allocated for annotations
		if(hAnnMem)
			GlobalFree(hAnnMem);

		return FALSE;
	}
	//	Catch all other exceptions
	catch(CException* pException)
	{
		//	Get the error message and pass it on to the handler
		pException->GetErrorMessage(szErrorMsg, sizeof(szErrorMsg));
		if(m_pErrors != 0) m_pErrors->Handle(0, szErrorMsg);
		pException->Delete();
		
		//	Free any memory allocated for annotations
		if(hAnnMem)
			GlobalFree(hAnnMem);

		return FALSE;
	}

//MsgBox(pHeader, "");

	//	Do we need to load a new file?
	if(!IsLoaded() || m_strFilename.CompareNoCase(Pane.szFilespec))
	{
		//	Load the file without redrawing yet
		m_sRotation = Pane.sAngle;
		if(SetFilename(Pane.szFilespec, FALSE) != TMV_NOERROR)
		{
			m_sRotation = sOldRotation;
			return FALSE;
		}

	}
	else
	{
		//	Delete any existing annotations and callouts
		Erase(TRUE);

		//	Rotate the image if we have to
		if(Pane.sAngle != m_sAngle)
		{
			m_sRotation = Pane.sAngle - m_sAngle;
			Rotate(FALSE);
		}
	}

	//	Restore the rotation value
	m_sRotation = sOldRotation;

	//	Reset the zap parameters
	ResetZapParameters();

	//	Are we supposed to ignore the viewport information contained in the
	//	zap file?
	if(!bUseView)
	{
		//	Load the callouts
		LoadCallouts(pFile, pHeader, &Pane, bCallouts);

		//	Draw the image using the default load options
		Draw();

		return TRUE;
	}

//MsgBox(&Pane);

	//	Set the view to match that stored in the zap file if the rectangle
	//	in the zap file is valid
	if(Pane.fDstLeft <= 0 && Pane.fDstTop <= 0 &&
	   Pane.fDstWidth > 0 && Pane.fDstHeight > 0)
	{
		//	Initialize the destination rectangle. We use width/height instead
		//	of right/bottom coordinates
		rcDst.left   = ROUND(Pane.fDstLeft);
		rcDst.top    = ROUND(Pane.fDstTop);
		rcDst.right  = ROUND(Pane.fDstWidth);
		rcDst.bottom = ROUND(Pane.fDstHeight);

		//	Set the rectangle
		SetDstRects(&rcDst, 0);
	}
	else
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_INVALIDZAPVIEW);	
	}

	//	Are we supposed to be scaling to fit the current window
	if(bScaleView)
	{
		m_bScaleImage = TRUE;

		//	Set the parameters used to maintain the relationship between the
		//	callouts and the source image if callouts are present
		if(Pane.sCallouts > 0)
		{
			if(!SetZapParameters(&(pHeader->rcWindow))) 
				return FALSE;
		}

		//	Make the window appear as though it is the same size as that 
		//	stored in the zap file
		m_iWidth  = Pane.iViewWidth;
		m_iHeight = Pane.iViewHeight;
	}
	else
	{
		m_bScaleImage = FALSE;
	}

	//	Redraw the zapped image
	RedrawZap();

	//	Add the annotations if we have any
	if(hAnnMem && Pane.lAnnBytes > 0)
	{
		SetAnnotations(hAnnMem, Pane.lAnnBytes);

		//	REMOVED THIS CODE FOR UPGRADE TO VERSION 12
		/*
		//	Since the offset of the destination rectangle may have changed, we
		//	need to adjust the annotations within the container to account for
		//	the new offset if the window is already visible
		//
		//	NOTE:	This change was required when we moved to LeadTools 11
		if(IsWindowVisible())
		{
			L_AnnMove((void*)GetAnnContainer(),
					  (GetDstLeft() - Pane.fDstLeft),
					  (GetDstTop() - Pane.fDstTop),
					  ANNFLAG_RECURSE | ANNFLAG_NOTTHIS);
		}
		*/ 
	}

	//	Deallocate the annotation memory
	if(hAnnMem)
		GlobalFree(hAnnMem);

	//	Restore the callouts
	LoadCallouts(pFile, pHeader, &Pane, bCallouts);	

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTMLead::LockAnn()
//
// 	Description:	This function will lock the annotation specified by the
//					caller
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMLead::LockAnn(HANNOBJECT hAnn) 
{
	CAnnotation* pAnn = 0;

	if((pAnn = GetAnnFromHandle(hAnn)) != 0)
	{
		//	Make sure this annotation is not locked
		if(pAnn->m_bIsLocked == FALSE)
		{
			L_AnnLock(hAnn, TMLEAD_LOCK_KEY, 0);
			L_AnnShowLockedIcon(hAnn, FALSE, 0);
			pAnn->m_bIsLocked = TRUE;
			L_AnnSetTag(hAnn, pAnn->GetAnnTag(), 0);
		}

		return TMV_NOERROR;
	}
	else
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_ANNNOTFOUND);
		return TMV_ANNNOTFOUND;
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::MsgBox()
//
// 	Description:	This is a debugging tool to view the values stored in the
//					specified zap file pane
//
// 	Returns:		IDOK / IDCANCEL
//
//	Notes:			None
//
//==============================================================================
UINT CTMLead::MsgBox(SZapPane* pPane, LPCSTR lpszTitle) 
{
	CString	strMsg;
	CString	strTemp = "";
	CString	strTitle = "";

	strTemp.Format("DstLeft: %f\n", pPane->fDstLeft);
	strMsg += strTemp;

	strTemp.Format("DstTop: %f\n", pPane->fDstTop);
	strMsg += strTemp;

	strTemp.Format("DstWidth: %f\n", pPane->fDstWidth);
	strMsg += strTemp;

	strTemp.Format("DstHeight: %f\n", pPane->fDstHeight);
	strMsg += strTemp;

	strTemp.Format("ViewWidth: %d\n", pPane->iViewWidth);
	strMsg += strTemp;

	strTemp.Format("ViewHeight: %d\n", pPane->iViewHeight);
	strMsg += strTemp;

	strTemp.Format("Angle: %d\n", pPane->sAngle);
	strMsg += strTemp;

	strTemp.Format("Callouts: %d\n", pPane->sCallouts);
	strMsg += strTemp;

	strTemp.Format("AnnBytes: %ld\n", pPane->lAnnBytes);
	strMsg += strTemp;

	strTemp.Format("Filespec: %s\n", pPane->szFilespec);
	strMsg += strTemp;

	strTemp.Format("TLMax: %lx\n", pPane->dwTLMax);
	strMsg += strTemp;

	strTemp.Format("BRMax: %lx\n", pPane->dwBRMax);
	strMsg += strTemp;

	strTemp.Format("PaneLeft: %x\n", pPane->wPaneLeft);
	strMsg += strTemp;

	strTemp.Format("PaneTop: %x\n", pPane->wPaneTop);
	strMsg += strTemp;


	if((lpszTitle != NULL) && (lstrlen(lpszTitle) > 0))
		strTitle = lpszTitle;
	else
		strTitle = "Zap";

	strTitle += " [Pane]";

	return MessageBox(strMsg, strTitle, MB_ICONINFORMATION | MB_OKCANCEL);
}

//==============================================================================
//
// 	Function Name:	CTMLead::MsgBox()
//
// 	Description:	This is a debugging tool to view the values stored in the
//					specified zap file pane
//
// 	Returns:		IDOK / IDCANCEL
//
//	Notes:			None
//
//==============================================================================
UINT CTMLead::MsgBox(SZapCallout* pCallout, LPCSTR lpszTitle) 
{
	CString	strMsg;
	CString	strTemp = "";
	CString	strTitle = "";

	strTemp.Format("rcRubberBand: L->%d T->%d R->%d B->%d \n", pCallout->rcRubberBand.left, pCallout->rcRubberBand.top, pCallout->rcRubberBand.right, pCallout->rcRubberBand.bottom);
	strMsg += strTemp;

	strTemp.Format("rcDstRect: L->%d T->%d R->%d B->%d \n", pCallout->rcDstRect.left, pCallout->rcDstRect.top, pCallout->rcDstRect.right, pCallout->rcDstRect.bottom);
	strMsg += strTemp;

	strTemp.Format("rcMax: L->%d T->%d R->%d B->%d \n", pCallout->rcMax.left, pCallout->rcMax.top, pCallout->rcMax.right, pCallout->rcMax.bottom);
	strMsg += strTemp;

	strTemp.Format("rcPosition: L->%d T->%d R->%d B->%d \n", pCallout->rcPosition.left, pCallout->rcPosition.top, pCallout->rcPosition.right, pCallout->rcPosition.bottom);
	strMsg += strTemp;

	strTemp.Format("Angle: %d\n", pCallout->sAngle);
	strMsg += strTemp;

	strTemp.Format("wAnnId: %x\n", pCallout->wAnnId);
	strMsg += strTemp;

	strTemp.Format("AnnBytes: %ld\n", pCallout->lAnnBytes);
	strMsg += strTemp;

	strTemp.Format("dwUnused1: %lx\n", pCallout->dwUnused1);
	strMsg += strTemp;

	strTemp.Format("dwUnused2: %lx\n", pCallout->dwUnused2);
	strMsg += strTemp;

	strTemp.Format("wUnused1: %x\n", pCallout->wUnused1);
	strMsg += strTemp;

	strTemp.Format("wFlags: %x\n", pCallout->wFlags);
	strMsg += strTemp;

	if((lpszTitle != NULL) && (lstrlen(lpszTitle) > 0))
		strTitle = lpszTitle;
	else
		strTitle = "Zap";

	strTitle += " [Callout]";

	return MessageBox(strMsg, strTitle, MB_ICONINFORMATION | MB_OKCANCEL);
}

//==============================================================================
//
// 	Function Name:	CTMLead::MsgBox()
//
// 	Description:	This is a debugging tool to view the values stored in the
//					specified zap file header
//
// 	Returns:		IDOK / IDCANCEL
//
//	Notes:			None
//
//==============================================================================
UINT CTMLead::MsgBox(SZapHeader* pHeader, LPCSTR lpszTitle) 
{
	CString	strMsg;
	CString	strTemp = "";
	CString	strTitle = "";

	strTemp.Format("Version: %ld\n", pHeader->lVersion);
	strMsg += strTemp;

	strTemp.Format("ScreenWidth: %d\n", pHeader->sScreenWidth);
	strMsg += strTemp;

	strTemp.Format("ScreenHeight: %d\n", pHeader->sScreenHeight);
	strMsg += strTemp;

	strTemp.Format("rcWindow: L->%d T->%d R->%d B->%d \n", pHeader->rcWindow.left, pHeader->rcWindow.top, pHeader->rcWindow.right, pHeader->rcWindow.bottom);
	strMsg += strTemp;

	strTemp.Format("SplitScreen: %s\n", pHeader->bSplitScreen ? "Yes" : "No");
	strMsg += strTemp;

	strTemp.Format("CtrlVersion: %lx\n", pHeader->dwCtrlVersion);
	strMsg += strTemp;

	strTemp.Format("dwUnused1: %lx\n", pHeader->dwUnused1);
	strMsg += strTemp;

	strTemp.Format("FooterSize: %x\n", pHeader->wFooterSize);
	strMsg += strTemp;

	strTemp.Format("wFlags: %x\n", pHeader->wFlags);
	strMsg += strTemp;


	if((lpszTitle != NULL) && (lstrlen(lpszTitle) > 0))
		strTitle = lpszTitle;
	else
		strTitle = "Zap";

	strTitle += " [Header]";

	return MessageBox(strMsg, strTitle, MB_ICONINFORMATION | MB_OKCANCEL);
}

//==============================================================================
//
// 	Function Name:	CTMLead::NextPage()
//
// 	Description:	This external method allows the caller to advance to the 
//					next page in the file.
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMLead::NextPage() 
{
	short sLeadError;

	//	Is another page available?
	if(m_sPage >= m_sPages)
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_PAGEBOUNDRYEXCEEDED);
		return TMV_PAGEBOUNDRYEXCEEDED;
	}

	//	Attempt to load the next page
	if((sLeadError = LoadImage(m_strFilename, (m_sPage + 1))) == TMV_NOERROR)
	{
		Draw();
		return TMV_NOERROR;
	}
	else
	{
		return HandleFileError(m_strFilename, sLeadError);
	}
	
}

//==============================================================================
//
// 	Function Name:	CTMLead::OnActivateCallout()
//
// 	Description:	This function is called by a CCallout object when it is
//					selected by the user.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnActivateCallout(CCallout* pCallout, BOOL bNotify) 
{
	//	Make this the active callout after verifying it's in the list
	if(m_pCallouts && m_pCallouts->Find(pCallout) != NULL)
		m_pCallout = pCallout;
	else
		m_pCallout = 0;

	//	Notify the parent control if requested
	if((m_pControl != 0) && (bNotify == TRUE))
		m_pControl->OnCalloutActivated(this, m_pCallout);
}

//==============================================================================
//
// 	Function Name:	CTMLead::OnAnimate()
//
// 	Description:	This function handles the event notification sent from the
//					lead control. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnAnimate(BOOL bEnable) 
{
	if(bEnable)
	{
		SetAutoRepaint(TRUE);
		m_bPlayingAnimation = TRUE;
	}
	else
	{
		SetAutoRepaint(FALSE);
		m_bPlayingAnimation = FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::OnAnnChange()
//
// 	Description:	This function handles the event notification sent from the
//					lead control when the properties of an annotation change
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnAnnChange(long hObject, long uType) 
{
	POSITION		Pos = NULL;
	CAnnotation*	pAnn;
	CCallout*		pCallout;
	ANNRECT			rcAnn;
	double			ddx;
	double			ddy;
	double			ddw;
	double			ddh;
	CString			strAnnText;
	BOOL			bTextChange = FALSE;

	//	Has the user finished editing a text annotation?
	//
	//	NOTE:	This has to be done first so that it gets done for
	//			callouts as well as source panes
	if(hObject == (long)m_hEditTextAnn)
	{
		//	Notify the host control that the session is over
		OnEndEditTextAnn(FALSE);
	}
	
	//	Don't bother doing anything if this is a callout or if we are not
	//	synchronizing annotations
	if((m_pOwner != 0) || !m_bSyncCalloutAnn)
		return;
 
	//	Get the rectangle of the annotation being changed
	if(L_AnnGetRect((void*)hObject, &rcAnn, NULL) != SUCCESS)
		return;

	//	Get the local annotation object
	if((pAnn = GetAnnFromHandle((void*)hObject)) == 0)
		return;

	//	Is this a text annotation?
	if(AnnGetType(hObject) == ANNOBJECT_TEXT)
	{
		//	Get the text associated with this annotation
		AnnGetText((void*)hObject, strAnnText);

		//	Has the text changed?
		if(pAnn->m_strText != strAnnText)
		{
			//	Set the flag to force an update
			bTextChange = TRUE;

			//	Update the local object
			pAnn->m_strText = strAnnText;
		}

	}

	//	If the text hasn't changed check to see if the size and/or
	//	position has changed
	if(bTextChange == FALSE)
	{
		//	Calculate the difference in the position and size
		ddx = fabs(rcAnn.left - pAnn->m_rcAnn.left);
		ddy = fabs(rcAnn.top  - pAnn->m_rcAnn.top);
		ddw = fabs((pAnn->m_rcAnn.right - pAnn->m_rcAnn.left) - 
				   (rcAnn.right - rcAnn.left));
		ddh = fabs((pAnn->m_rcAnn.bottom - pAnn->m_rcAnn.top) - 
				   (rcAnn.bottom - rcAnn.top));

		//	Don't process this event if the size or position hasn't changed
		//
		//	NOTE:	This event may have gotten fired because the selection
		//			state has changed
		if(ddx < 0.001 && ddy < 0.001 && ddh < 0.001 && ddw < 0.001)
			return;
	}
		
	//	Is this the highlight for a callout?
	if(pAnn->m_bIsCallout)
	{
		//	Prevent the user from moving the callout highlight
		L_AnnSetRect((void*)hObject, &(pAnn->m_rcAnn));
	}
	else
	{
		//	Notify the container that the text, size and/or position has changed
		if(m_pControl)
			m_pControl->OnAnnotationModified(this, hObject);

		//	Update each callout linked to this annotation
		pCallout = pAnn->First();
		while(pCallout)
		{
			//	Make sure this callout is still in the list
			if(m_pCallouts->Find(pCallout))
			{
				pCallout->DeleteAnn(pAnn->GetAnnTag());
				pCallout->CopyAnn(this, (void*)hObject);
			}
			pCallout = pAnn->Next();
		}

		//	Update the rectangle
		memcpy(&(pAnn->m_rcAnn), &rcAnn, sizeof(pAnn->m_rcAnn));
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::OnAnnCreate()
//
// 	Description:	This function handles the event notification sent from the
//					lead control. It sets the properties of the annotation as
//					it is being drawn.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnAnnCreate(long hObject) 
{
	HANNOBJECT		Object = (HANNOBJECT)hObject;
    double			XScalar;
    double			YScalar;
    double			LineWidth;

    //	Don't bother if we're not setting the annotation properties
	//
	//	NOTE:	If we are reloading the annotations from a file or copying
	//			source annotations we want to retain the original properties
	if(!m_bSetAnnProps)
		return;

	//  What action is taking place?
    switch(m_sAction)
    {
        case DRAW:

            //  Calculate the desired thickness using  current scale factors
            L_AnnGetScalarX(Object, &XScalar);
            L_AnnGetScalarY(Object, &YScalar);
            LineWidth = (double)((double)(m_sAnnThickness * 4) / (sqrt(XScalar * YScalar)));


			ANNTEXTOPTIONS TextOptions;
			// Change some options
			memset(&TextOptions, 0, sizeof(ANNTEXTOPTIONS));
			TextOptions.uStructSize = sizeof(ANNTEXTOPTIONS);
			TextOptions.bShowBorder = false;
			TextOptions.crText = m_crDraw;
			TextOptions.uFlags = ANNTEXT_SHOW_BORDER | ANNTEXT_TEXTCOLOR;
			L_AnnSetTextOptions(Object, &TextOptions, 0);

            L_AnnSetForeColor(Object, m_crDraw, 0);
            L_AnnSetBackColor(Object, m_crDraw, 0);
            L_AnnSetLineWidth(Object, LineWidth, 0);
            L_AnnSetLineStyle(Object, ANNLINE_SOLID, 0);
            L_AnnSetFillMode(Object, m_iFillMode, 0, 0);

            break;
            
        case REDACT:

            L_AnnSetForeColor(Object, m_crRedact, 0);
            L_AnnSetBackColor(Object, m_crRedact, 0);

            break;
            
        case HIGHLIGHT:

            L_AnnSetForeColor(Object, m_crHighlight, 0);
            L_AnnSetBackColor(Object, m_crHighlight, 0);
            L_AnnSetFillMode(Object, ANNMODE_TRANSLUCENT, 0, 0);

            break;
            
        case CALLOUT:

			if(m_hCalloutShade != 0)
				L_AnnSendToBack(m_hCalloutShade);
            
			break;
            
        default:
        
            break;
            
    }
}

//==============================================================================
//
// 	Function Name:	CTMLead::OnAnnDestroy()
//
// 	Description:	This function handles the event notification sent from the
//					lead control. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnAnnDestroy(long hObject) 
{
	CAnnotation*	pAnn;
	CCallout*		pCallout;
	DWORD			dwTag;

	//	We don't have to do anything if this object is owned by a callout
	if(m_pOwner != 0)
		return;

	//	Get the tag associated with this object
	if(L_AnnGetTag((void*)hObject, (L_UINT32*)(&dwTag)) != SUCCESS)
		return;

	//	Locate this annotation in the local list
	if((pAnn = m_Annotations.Find(dwTag)) == 0)
		return;

	//	Is this the cutout background?
	if(pAnn->m_bIsCalloutShade)
	{
		ASSERT((long)m_hCalloutShade == hObject);
		if((long)m_hCalloutShade == hObject)
			m_hCalloutShade = 0;
		
		//	Remove the annotation from the local list
		m_Annotations.Remove(pAnn, TRUE);
		return;
	}

	//	Is this annotation the highlight for a callout?
	if(pAnn->m_bIsCallout)
	{
		//	Destroy the callout associated with this annotation
		pCallout = pAnn->First();
		if(pCallout && m_pCallouts && m_pCallouts->Find(pCallout))
		{
			m_pCallouts->Remove(pCallout, FALSE);
			DeleteCallout(pCallout);
		}
		
	}
	else
	{
		//	Are we synchronizing callout annotations?
		if(m_bSyncCalloutAnn && m_pCallouts)
		{
			//	Delete each of the linked callouts
			pCallout = pAnn->First();
			while(pCallout)
			{
				//	Make sure this callout is still in the list
				if(pCallout && m_pCallouts->Find(pCallout))
					pCallout->DeleteAnn(dwTag);

				//	Get the next callout 
				pCallout = pAnn->Next();
			
			}
		}
	}

	//	Notify the container 
	if(m_pControl)
		m_pControl->OnAnnotationDeleted(this, hObject);

	//	Remove from the local list and destroy the object
	m_Annotations.Remove(pAnn, TRUE);

}

//==============================================================================
//
// 	Function Name:	CTMLead::OnAnnDrawn()
//
// 	Description:	This function handles the event notification sent from the
//					lead control. It sets the properties of the annotation.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnAnnDrawn(long hObject) 
{
	HANNOBJECT		Object = (HANNOBJECT)hObject;
	CAnnotation*	pAnn = 0;
	CAnnTextDlg		Dialog;
	BOOL			bFontChanged = FALSE;

	//	Add a local reference to the new annotation
	if(m_pOwner == 0)
	{
		if((pAnn = m_Annotations.Add()) != 0)
		{
			L_AnnSetTag(Object, pAnn->GetAnnTag(), 0);
			L_AnnGetRect(Object, &(pAnn->m_rcAnn), NULL);

			//	Store the text if this is a text annotation
			if(AnnGetType(hObject) == ANNOBJECT_TEXT)
				AnnGetText(Object, pAnn->m_strText);		
		}

		OnActivateCallout(0, TRUE);

		//	Notify the container
		if(m_pControl != 0)
			m_pControl->OnAnnotationDrawn(this, hObject);
	}
	
	//  What action is taking place?
    switch(m_sAction)
    {
        case DRAW:

			//	Is this a text annotation?
			if(m_sAnnTool == ANNTEXT)
			{
				//	Clear the existing text
				L_AnnSetText(Object, "", 0);

				//	Let the control know we are about to open the dialog box
				if(m_pControl)
				{
					m_pControl->OnOpenTextBox(this);
				}

				//	Initialize the dialog box
				Dialog.m_strName = m_strAnnFontName;
				Dialog.m_sSize = m_sAnnFontSize;
				Dialog.m_bBold = m_bAnnFontBold;
				Dialog.m_bStrikeThrough = m_bAnnFontStrikeThrough;
				Dialog.m_bUnderline = m_bAnnFontUnderline;

				//	Open the dialog box to prompt the user for the text
				if(m_pControl)
					m_pControl->PreModalDialog();
				Dialog.DoModal();
				if(m_pControl)
					m_pControl->PostModalDialog();

				//	This tricks lead tools into thinking the user has clicked
				//	the mouse button and therefore finished editing the text
				SendMessage(WM_RBUTTONDOWN, MK_RBUTTON, 0);
				SendMessage(WM_RBUTTONUP, MK_RBUTTON, 0);

				//	Update the new font properties
				if(Dialog.m_strName != m_strAnnFontName)
				{
					SetAnnFontName(Dialog.m_strName);
					bFontChanged = TRUE;
				}
				if(Dialog.m_sSize != m_sAnnFontSize)
				{
					SetAnnFontSize(Dialog.m_sSize);
					bFontChanged = TRUE;
				}
				if(Dialog.m_bBold != m_bAnnFontBold)
				{
					SetAnnFontBold(Dialog.m_bBold);
					bFontChanged = TRUE;
				}
				if(Dialog.m_bStrikeThrough != m_bAnnFontStrikeThrough)
				{
					SetAnnFontStrikeThrough(Dialog.m_bStrikeThrough);
					bFontChanged = TRUE;
				}
				if(Dialog.m_bUnderline != m_bAnnFontUnderline)
				{
					SetAnnFontUnderline(Dialog.m_bUnderline);
					bFontChanged = TRUE;
				}

				//	Set the text properties
				L_AnnSetText(Object, Dialog.m_strAnnText.GetBuffer(1), 0);
				L_AnnSetFontSize(Object, Dialog.m_sSize, 0);
				L_AnnSetFontName(Object, Dialog.m_strName.GetBuffer(1), 0);
				L_AnnSetFontBold(Object, Dialog.m_bBold, 0);
				L_AnnSetFontStrikeThrough(Object, Dialog.m_bStrikeThrough, 0);
				L_AnnSetFontUnderline(Object, Dialog.m_bUnderline, 0);
				
				//	Let the control know we closed the dialog box and changed
				//	the font properties
				if(m_pControl)
				{
					if(bFontChanged)
						m_pControl->OnAnnFontChanged(this);

					m_pControl->OnCloseTextBox(this);
				}
			
			}// if(m_sAnnTool == ANNTEXT)

			//	Drop through from here
			//				.
			//				.
			//				.

        case REDACT:
        case HIGHLIGHT:

			L_AnnSetSelected(Object, FALSE, 0);

			//	Copy the annotation to all callout objects
			if(pAnn && m_bSyncCalloutAnn)
				SyncAnnotation(pAnn);

            break;
            
        case CALLOUT:

			L_AnnSetSelected(Object, FALSE, 0);
			L_AnnSendToBack(Object);

			if(m_hCalloutShade != 0)
				L_AnnSendToBack(m_hCalloutShade);

            break;
            
        default:
        
            break;
            
    }

}

//==============================================================================
//
// 	Function Name:	CTMLead::OnAnnMouseDown()
//
// 	Description:	This function is called by the control when the Lead control
//					associated with this pane fires the AnnMouseDown event.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnAnnMouseDown(short Button, short Shift, long X, long Y) 
{
	//	Save the current mouse coordinates
	m_lAnnX = X;
	m_lAnnY = Y;
}

//==============================================================================
//
// 	Function Name:	CTMLead::OnAnnMouseUp()
//
// 	Description:	This function is called by the control when the Lead control
//					associated with this pane fires the AnnMouseUp event.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnAnnMouseUp(short Button, short Shift, long X, long Y) 
{
}

//==============================================================================
//
// 	Function Name:	CTMLead::OnAnnSelect()
//
// 	Description:	This function is called whenever an annotation is selected
//					by the caller
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnAnnSelect()
{
	CCallout*		pCallout;
	CAnnotation*	pAnn;
	UINT			uSelections;
	HANNOBJECT*		pSelections;
	DWORD			dwTag;

	//	Clear the active callout if this is the source image
	if(m_pOwner == 0)
		OnActivateCallout(0, TRUE);

	//	Clear the current selections if not in SELECT mode
	if(m_sAction != SELECT)
	{
		ClearSelections();
	}
	else
	{
		//	Don't bother if this is a callout window
		if(m_pOwner != 0) return;

		//	Clear the selections in the callout windows
		if(m_bSyncCalloutAnn && (m_pCallouts != 0))
		{
			pCallout = m_pCallouts->First();
			while(pCallout)
			{
				pCallout->ClearSelections();
				pCallout = m_pCallouts->Next();
			}
		}	

		//	Get the selections 
		if((uSelections = GetSelections(&pSelections)) == 0)
			return;

		//	Iterate the list of selections
		for(UINT i = 0; i < uSelections; i++)
		{
			//	Is this the cutout background?
			if(pSelections[i] == m_hCalloutShade)
			{
				//	Prevent the user from selecting the cutout background
				//L_AnnSetSelected(pSelections[i], FALSE, 0);

				//	We can stop here if we're not synchronizing callouts
				if(!m_bSyncCalloutAnn || (m_pCallouts == 0))
					break;
			}
			else
			{
				//	Do we need to synchronize the callouts?
				if(m_bSyncCalloutAnn && (m_pCallouts != 0))
				{
					//	Get the tag for this annotation
					if((dwTag = GetTagFromHandle(pSelections[i])) != 0)
					{
						if((pAnn = m_Annotations.Find(dwTag)) != 0)
						{
							//	Synchronize the callout windows
							SyncSelection(pAnn, 0);
						}
					}
				}
			
			}//if(pSelections[i] == m_hCalloutShade)
		
		}//for(UINT i = 0; i < uSelections; i++)

		//	Deallocate the handle array
		HeapFree(GetProcessHeap(), 0, pSelections);
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::OnCalloutModified()
//
// 	Description:	This function is called from a callout when the user 
//					changes the portion of source image being viewed
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnCalloutModified(CCallout* pCallout, RECT* pSource) 
{
	CAnnotation* pAnn;
	HANNOBJECT	 hAnn;

	ASSERT(pCallout != 0);
	if(!pCallout) return;

	if((pAnn = m_Annotations.Find(pCallout->GetAnnId())) != 0)
	{
		if((hAnn = GetHandleFromTag(pAnn->GetAnnTag())) != 0)
		{
			//	Update the annotation object's rectangle
			//
			//	NOTE:	This MUST be done first because OnAnnChange() is going
			//			to get called when we set the annotation rectangle
			pAnn->m_rcAnn.left   = pSource->left;
			pAnn->m_rcAnn.right  = pSource->right - (2 * m_sCallFrameThick);
			pAnn->m_rcAnn.top    = pSource->top;
			pAnn->m_rcAnn.bottom = pSource->bottom - (2 * m_sCallFrameThick);
			
			//	Move the annotation
			L_AnnSetRect(hAnn, &(pAnn->m_rcAnn));
		}
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::OnCalloutMoved()
//
// 	Description:	This function is called by a CCallout object when it is
//					moved by the user.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnCalloutMoved(CCallout* pCallout) 
{
	//	Make this the active callout after verifying it's in the list
	if(m_pCallouts && m_pCallouts->Find(pCallout) != NULL)
		m_pCallout = pCallout;
	else
		m_pCallout = 0;

	//	Notify the parent control
	if(m_pControl)
		m_pControl->OnCalloutMoved(this, m_pCallout);
}

//==============================================================================
//
// 	Function Name:	CTMLead::OnCalloutResized()
//
// 	Description:	This function is called by a CCallout object when it is
//					resized by the user.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnCalloutResized(CCallout* pCallout) 
{
	//	Make this the active callout after verifying it's in the list
	if(m_pCallouts && m_pCallouts->Find(pCallout) != NULL)
		m_pCallout = pCallout;
	else
		m_pCallout = 0;

	//	Notify the parent control
	if(m_pControl)
		m_pControl->OnCalloutResized(this, m_pCallout);
}

//==============================================================================
//
// 	Function Name:	CTMLead::OnCalloutSelection()
//
// 	Description:	This function is called from a callout when the user selects
//					one of it's annotations
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnCalloutSelection(CCallout* pSelector) 
{
	CTMLead*		pTMLead;
	CCallout*		pCallout;
	CAnnotation*	pAnn;
	UINT			uSelections;
	HANNOBJECT*		pSelections;
	DWORD			dwTag;

	//	Keep track of the callout in which the last selection was made
	OnActivateCallout(pSelector, TRUE);

	if(!pSelector || !m_pCallouts)
		return;

	//	Don't bother doing anything if we are not synchronizing callouts
	if(!m_bSyncCalloutAnn)
		return;
	
	//	Clear the current selections in the base image
	ClearSelections();

	//	Clear the selections in the other callouts
	pCallout = m_pCallouts->First();
	while(pCallout)
	{
		if(pCallout != pSelector)
			pCallout->ClearSelections();
		pCallout = m_pCallouts->Next();
	}	

	//	Get the TMLead object owned by the callout
	if((pTMLead = pSelector->GetTMLead()) == 0)
		return;

	//	Get the selections in the callout
	if((uSelections = pTMLead->GetSelections(&pSelections)) == 0)
		return;

	//	Synchronize each of the selections
	for(UINT i = 0; i < uSelections; i++)
	{
		//	Get the tag for this annotation
		if((dwTag = pSelector->GetTagFromHandle(pSelections[i])) != 0)
			if((pAnn = m_Annotations.Find(dwTag)) != 0)
				SyncSelection(pAnn, pSelector);
	}

	//	Deallocate the handle array
	HeapFree(GetProcessHeap(), 0, pSelections);
}

//==============================================================================
//
// 	Function Name:	CTMLead::OnClickCallout()
//
// 	Description:	This function is called by a callout when the user clicks
//					in the callout window.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnClickCallout(CCallout* pCallout, short sButton, short sKey) 
{
	//	Notify the parent control
	if(m_pControl)
		m_pControl->OnClickCallout(this, pCallout, sButton, sKey);
}

//==============================================================================
//
// 	Function Name:	CTMLead::OnCloseCallout()
//
// 	Description:	This function is called from a callout when it should be
//					closed.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnCloseCallout(CCallout* pCallout) 
{
	if(!pCallout)
		return;

	//	Notify the control that this callout's being destroyed
	if(m_pControl)
		m_pControl->OnCalloutDestroyed(this, pCallout);

	//	Remove all references to this callout from the annotations lists
	m_Annotations.Remove(pCallout);

	//	Reset the local reference if it's the same
	if(m_pCallout == pCallout)
		OnActivateCallout(0, FALSE);
}

//==============================================================================
//
// 	Function Name:	CTMLead::OnKeyUp()
//
// 	Description:	This function is called when the parent recieves a KeyUp
//					event.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnKeyUp(short* KeyCode, short Shift) 
{
}

//==============================================================================
//
// 	Function Name:	CTMLead::OnMouseDblClick()
//
// 	Description:	This function is called by the control when the pane's 
//					Lead control fires the MouseDblClick() event.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnMouseDblClick() 
{
    POINT			Point;
	HANNOBJECT		hAnn;

	//	Clear the existing reference
	OnEndEditTextAnn(FALSE);

	//	Are we in the mode that allows the user to modify annotations?
	if(m_sAction == SELECT)
	{
		//	Locate the annotation at the last known cursor position
		Point.x = m_lAnnX;
		Point.y = m_lAnnY;

		if((hAnn = GetAnnFromPt(&Point)) != 0)
		{
			//	Is the user starting to edit a text annotation
			if(AnnGetType((long)hAnn) == ANNOBJECT_TEXT)
			{
				//	Store a reference to the annotation being edited
				m_hEditTextAnn = hAnn;

				//	Notify the parent control
				if(m_pControl)
					m_pControl->OnStartEditTextAnn(this);

				return;
			}
		}
	}

	// reseting document zoom
	ResetZoom();
}

//==============================================================================
//
// 	Function Name:	CTMLead::OnMouseDown()
//
// 	Description:	This function will set up rubberbanding if the current 
//					action is to ZOOM.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnMouseDown(short Button, short Shift, long X, long Y) 
{
	//	Clear the active callout if this is the source image
	if(m_pOwner == 0)
		OnActivateCallout(0, TRUE);

    //  Is this the right mouse button?
    if(Button == RIGHT_MOUSEBUTTON)
	{
		//	Can we automatically engage panning on a right click?
		if(m_bRightClickPan)
		{
			//	Set the action to PAN
			m_sLastAction = m_sAction;
			SetAction(PAN);

			//	Save the pan start position
			m_lPanX = X;
			m_lPanY = Y;
	
			SetCursor(m_aCursors[PAN_CURSOR]);
			SetMousePointer(99);
		}
				
	}
	else
	{
		//  What action is in progress?
		switch(m_sAction)
		{
			case PAN:

				//	Save the pan start position
				m_lPanX = X;
				m_lPanY = Y;

				SetCursor(m_aCursors[PAN_CURSOR]);
				SetMousePointer(99);

				break;

			case ZOOM:

				if(m_fZoomFactor < m_fMaxZoom)
				{ 
					SetAutoRubberBand(TRUE);
					SetCursor(m_aCursors[ZOOM_CURSOR]);
					SetMousePointer(99);
				}
				else
				{
					SetMousePointer(MP_DEFAULT);
				}
				return;

			case CALLOUT:

				//	Let the user rubber band the callout region
				SetAutoRubberBand(TRUE);
				SetCursor(m_aCursors[CALLOUT_CURSOR]);
				SetMousePointer(99);
				return;

			default:
				return;

    
		}// switch(m_sAction)

	}
    
}

//==============================================================================
//
// 	Function Name:	CTMLead::OnMouseMove()
//
// 	Description:	This function will track mouse movements in the window
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnMouseMove(short Button, short Shift, long X, long Y) 
{
	//	Keep track of the current position. We use this for creating callouts
	m_lMouseX = X;
	m_lMouseY = Y;

	//	Is the left button down?
	if(Button == LEFT_MOUSEBUTTON)
	{
		//  What action is in progress?
		switch(m_sAction)
		{
			case PAN:

				//	Pan the image
				Pan((X - m_lPanX), (Y - m_lPanY));

				//	Save the new coordinates
				m_lPanX = X;
				m_lPanY = Y;

				break;

			default:

				break;

		}// switch(m_sAction)
	}
	else if(Button == RIGHT_BUTTON)
	{ 
		//  What action is in progress?
		switch(m_sAction)
		{
			case PAN:

				//	Pan the image
				Pan((X - m_lPanX), (Y - m_lPanY));

				//	Save the new coordinates
				m_lPanX = X;
				m_lPanY = Y;

				break;

			default:

				break;

		}// switch(m_sAction)
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::OnMouseUp()
//
// 	Description:	This function is called when the user releases the mouse
//					button
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnMouseUp(short Button, short Shift, long X, long Y) 
{
	//	Is the left button down?
	if(Button == LEFT_MOUSEBUTTON)
	{
		SetMousePointer(MP_DEFAULT);
	}
	else if(Button == RIGHT_BUTTON)
	{
		//	Can we automatically engage panning on a right click?
		if(m_bRightClickPan)
		{
			//	Restore the last action
			SetAction(m_sLastAction);

			//	Reset the pan start position
			m_lPanX = 0;
			m_lPanY = 0;
			
			SetMousePointer(MP_DEFAULT);
		}
				
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::OnRubberBand()
//
// 	Description:	This function is called when the user completes a rubber
//					banding operation. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::OnRubberBand() 
{
    float   RbLeft;
    float   RbTop;
    float   RbWidth;
    float   RbHeight;

	//	Clear the active callout if this is the source image
	if(m_pOwner == 0)
		OnActivateCallout(0, TRUE);

	//	Get the dimensions of the rubber band rectangle
	RbLeft   = GetRubberBandLeft();
	RbTop    = GetRubberBandTop();
	RbWidth  = GetRubberBandWidth();
	RbHeight = GetRubberBandHeight();

	//	Adjust the rubber band coordinates just in case the user did not
	//	go from the top left to the bottom right
	if(RbWidth < 0)
	{
		RbWidth *= -1;
		if(RbWidth > RbLeft)
			RbLeft = RbWidth - RbLeft;
		else
			RbLeft = RbLeft - RbWidth;
	}
	if(RbHeight < 0)
	{
		RbHeight *= -1;
		if(RbHeight > RbTop)
			RbTop = RbHeight - RbTop;
		else
			RbTop = RbTop - RbHeight;
	}

	//	Turn off the automatic rubberband
	SetAutoRubberBand(FALSE);
	SetMousePointer(MP_DEFAULT);

	//	Set the coordinates of the rubberband rectangle
	m_rcRubberBand.left   = ROUND(RbLeft);
	m_rcRubberBand.top    = ROUND(RbTop);
	m_rcRubberBand.right  = ROUND(RbLeft + RbWidth);
	m_rcRubberBand.bottom = ROUND(RbTop + RbHeight);

	//  What action is in progress?
	switch(m_sAction)
	{
		case ZOOM:		
		
			//	Do we need to restrict the display to the user 
			//	defined rectangle?
			if(m_bZoomToRect)
				ZoomRestricted(&m_rcRubberBand);
			else
				ZoomUnrestricted(&m_rcRubberBand);
			break;

		case CALLOUT:	
		
			CreateUserCallout(&m_rcRubberBand);
			break;

		case DRAW:		
		case REDACT:	
		case HIGHLIGHT:	
		case SELECT:	
		case PAN:		
		default:		break;
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::Pan()
//
// 	Description:	This function will pan the image in the direction specified
//					by the caller.
//
// 	Returns:		TRUE if successful.
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLead::Pan(short sDirection)
{
	float	fDeltaX;
	float	fDeltaY;

	//	How big a step are we taking?
	fDeltaX = (float)m_iWidth * m_fPanPercent;
	fDeltaY = (float)m_iHeight * m_fPanPercent;

	//	Translate the direction into specific offsets
	switch(sDirection)
	{
		case PAN_LEFT:

			Pan((long)fDeltaX, 0);
			break;

		case PAN_RIGHT:
			
			Pan((long)(fDeltaX * -1) , 0);
			break;

		case PAN_UP:

			Pan(0, (long)fDeltaY);
			break;

		case PAN_DOWN:

			Pan(0, (long)(fDeltaY * -1));
			break;

		default:

			return  FALSE;

	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTMLead::Pan()
//
// 	Description:	This function will pan the image by the offset specified by
//					the caller
//
// 	Returns:		TRUE if successful.
//
//	Notes:			This function is called to perform drag style panning
//
//==============================================================================
void CTMLead::Pan(long lX, long lY)
{
	float	fDstTop;
	float	fDstLeft;
	float	fDstHeight;
	float	fDstWidth;
	float	fDeltaX;
	float	fDeltaY;
	float	fNewLeft;
	float	fNewTop;
	short	sStates;

	//  Find the current size and position of the destination rectangle.
	fDstLeft   = GetDstLeft();
	fDstTop	   = GetDstTop();
	fDstWidth  = GetDstWidth();
	fDstHeight = GetDstHeight();

	//	How big a step are we taking?
	fDeltaX = (float)lX;
	fDeltaY = (float)lY;

	//	Get the directional states
	sStates = GetPanStates();

	//	Are we trying to pan right?
	if(lX < 0 && (sStates & ENABLE_PANRIGHT))
	{
		//	What is the new left hand coordinate?
		fNewLeft = fDstLeft + fDeltaX;
		if(fNewLeft < ((float)m_iWidth - fDstWidth))
			fNewLeft = ((float)m_iWidth - fDstWidth);
	}
	//	Are we trying to pan left?
	else if(lX > 0 && (sStates & ENABLE_PANLEFT))
	{
		//	What is the new left hand coordinate?
		fNewLeft = fDstLeft + fDeltaX;
		if(fNewLeft > 0)
			fNewLeft = 0.0;
	}
	//	We are not panning left or right
	else
	{
		fNewLeft = fDstLeft;
	}

	//	Are we trying to pan down?
	if(lY < 0 && (sStates & ENABLE_PANDOWN))
	{
		//	What is the new top coordinate?
		fNewTop = fDstTop + fDeltaY;
		if(fNewTop < ((float)m_iHeight - fDstHeight))
			fNewTop = ((float)m_iHeight - fDstHeight);
	}
	//	Are we trying to pan up?
	else if(lY > 0 && (sStates & ENABLE_PANUP))
	{
		//	What is the new top coordinate?
		fNewTop = fDstTop + fDeltaY;
		if(fNewTop > 0)
			fNewTop = 0.0;
	}
	//	We are not panning up or down
	else
	{
		fNewTop = fDstTop;
	}
		
	//	Move the destination rectangles
	SetDstRect(fNewLeft, fNewTop, fDstWidth, fDstHeight);
	SetDstClipRect(fNewLeft, fNewTop, fDstWidth, fDstHeight);

	//	Notify the parent if this is a callout
	if(m_pOwner != 0)
		m_pOwner->OnPanComplete();
	
	ForceRepaint();
}

//==============================================================================
//
// 	Function Name:	CTMLead::Paste()
//
// 	Description:	This function will paste the current contents of the 
//					clipboard
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMLead::Paste()
{
	//	First determine if the clipboard data is valid
	if(CLead::Paste(PASTE_ISREADY) == 0)
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_INVALIDCLIPBOARD);
		return TMV_INVALIDCLIPBOARD;
	}

	//	Clear the current file
	UnloadImage();

	//	Now paste the data
	if(CLead::Paste(0) != 0)
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_PASTEFAILED);
		return TMV_PASTEFAILED;
	}
	else
    {
		//  Update the page index
        m_sPage = 0;

		//	Reset the cumulative rotation
		m_sAngle = 0;

        //  Calculate the size and aspect ratio of this image
        m_fImageHeight = GetSrcHeight();
        m_fImageWidth  = GetSrcWidth();
        m_fAspectRatio = m_fImageHeight / m_fImageWidth;
        
		//	Initialize the destination rectangle to match the source rectangles
		SetDstRect(0.0f, 0.0f, m_fImageWidth, m_fImageHeight);
		SetDstClipRect(0.0f, 0.0f, m_fImageWidth, m_fImageHeight);

		//	The image is now loaded
		m_bLoaded = TRUE;

		Draw();

		return TMV_NOERROR;
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::PlayAnimation()
//
// 	Description:	This function is called to play the animation.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMLead::PlayAnimation(BOOL bPlay, BOOL bContinuous)
{
    //	Are we supposed to play it?
	if(bPlay)
	{
		//	Is this an animation file?
		if(!m_bAnimation)
		{
			if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_NOTANIMATION, m_strFilename);
			return TMV_NOTANIMATION;
		}

		//	Play the animation from the beginning
		SetAnimationLoop(bContinuous);
		SetAnimationEnable(TRUE);
		RedrawNormal();
		return TMV_NOERROR;
		
	} 
	else
	{
		SetAnimationEnable(FALSE);
		return TMV_NOERROR;
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::PrevPage()
//
// 	Description:	This external method allows the caller to go back to the 
//					previous page in the file.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMLead::PrevPage() 
{
	short sLeadError;

	//	Is another page available?
	if(m_sPage <= 1)
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_PAGEBOUNDRYEXCEEDED);
		return TMV_PAGEBOUNDRYEXCEEDED;
	}

	//	Attempt to load the previous page
	if((sLeadError = LoadImage(m_strFilename, (m_sPage - 1))) == TMV_NOERROR)
	{
		Draw();
		return TMV_NOERROR;
	}
	else
	{
		return HandleFileError(m_strFilename, sLeadError);
	}
	
}

//==============================================================================
//
// 	Function Name:	CTMLead::Print()
//
// 	Description:	This function will print the image loaded in this pane 
//					within the rectangle provided by the caller.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLead::Print(CDC* pdc, RECT* prcImage, RECT* prcCallouts, BOOL bFullImage, short sRotation) 
{
	float		fMaxWidth;
	float		fMaxHeight;
	float		fMaxCallWidth;
	float		fMaxCallHeight;
	float		fAspectRatio;
	float		fPrintLeft = 0;
	float		fPrintTop = 0;
	float		fPrintWidth = 0;
	float		fPrintHeight = 0;
	float		fCallWidth;
	float		fCallHeight;
	CTMLead*	pScratch = 0;
	RECT		rcVisible;
	RECT		rcClient;
	BOOL		bRealized = FALSE;
	BOOL		bRenderAsDIB = FALSE;
	
	ASSERT(prcImage);
	ASSERT(pdc);

#ifdef _DEBUG
	DWORD	dwStartTime = GetTickCount();
	FILE*	fptr = NULL;
	fopen_s(&fptr, "c:\\tmview.txt", "wt");
	
	if(fptr != NULL)
	{
		fprintf(fptr, "\nMax Rectangle: L: %d  T: %d  R: %d  B: %d  W: %d  H: %d\n",
				prcImage->left, prcImage->top, prcImage->right, prcImage->bottom,
				prcImage->right - prcImage->left, prcImage->bottom - prcImage->top);
		fprintf(fptr, "DST Rect: L: %.1f  T: %.1f  W: %.1f  H: %.1f\n",
				GetDstLeft(), GetDstTop(), GetDstWidth(), GetDstHeight());
		fprintf(fptr, "SRC Rect: L: %.1f  T: %.1f  W: %.1f  H: %.1f\n",
				GetSrcLeft(), GetSrcTop(), GetSrcWidth(), GetSrcHeight());
		fprintf(fptr, "WND Rect: L: %d  T: %d  W: %d  H: %d\n",
				m_iLeft, m_iTop, m_iWidth, m_iHeight);
		fprintf(fptr, "Full Image = %s\n", bFullImage ? "TRUE" : "FALSE");
		fprintf(fptr, "Rotation = %d\n", sRotation);
	}
#endif

	//	Should we print a border around the image?
	if(m_bPrintBorder)
		PrintBorder(pdc, m_crPrintBorder, m_fPrintBorderThickness, prcImage);
		
	//	Copy the bitmap into the control's scratch pane
	if((pScratch = GetScratchCopy()) == 0) return FALSE;

	//	Should we clip the source rectangle?
	if((bFullImage == FALSE) && (GetSrcVisible(&rcVisible) == FALSE))
	{
		//	We have to realize the annotations before trimming otherwise
		//	they will be lost
		if(m_Annotations.GetCount() > 0)
		{
			pScratch->Realize(TRUE);
			bRealized = TRUE;
		}

		//	Crop the bitmap to the visible area
		pScratch->Trim((float)rcVisible.left, 
					   (float)rcVisible.top,
					   (float)(rcVisible.right - rcVisible.left),
					   (float)(rcVisible.bottom - rcVisible.top));

		#ifdef _DEBUG
		if(fptr != NULL)
		{
			fprintf(fptr, "Trimmed: L: %d  T: %d  R: %d  B: %d  W: %d  H: %d\n",
					rcVisible.left, rcVisible.top, rcVisible.right, rcVisible.bottom,
					rcVisible.right - rcVisible.left, rcVisible.bottom - rcVisible.top);
		}
		#endif
		
	}// if(m_bFullImage == FALSE)

	#ifdef _DEBUG
	if(fptr != NULL)
	{
		fprintf(fptr, "Before: W: %f  H: %f\n",pScratch->GetBitmapWidth(), pScratch->GetBitmapHeight());
	}
	#endif

	//	Are we supposed to rotate prior to printing?
	if(sRotation < 0)
		pScratch->Rotate((short)-90);
	else if(sRotation > 0)
		pScratch->Rotate((short)90);

	//	Get the aspect ratio of the source
	//	
	//	NOTE:	We use the bitmap extents instead of the source extents because the
	//			source rectangles don't seem to update properly until we move the window
	fAspectRatio = pScratch->GetBitmapHeight() / pScratch->GetBitmapWidth();

	#ifdef _DEBUG
	if(fptr != NULL)
	{
		fprintf(fptr, "After: W: %f  H: %f  AR: %f\n",pScratch->GetBitmapWidth(), pScratch->GetBitmapHeight(), fAspectRatio);
	}
	#endif

	//	Get the size of the available image and callout print areas
	fMaxWidth  = (float)(prcImage->right - prcImage->left);
	fMaxHeight = (float)(prcImage->bottom - prcImage->top);
	
    //  Find the center point of the print area
    m_fPrintCx = (float)prcImage->left + (fMaxWidth / 2.0f);
    m_fPrintCy = (float)prcImage->top + (fMaxHeight / 2.0f);

	if(prcCallouts != NULL)
	{
		fMaxCallWidth  = (float)(prcCallouts->right - prcCallouts->left);
		fMaxCallHeight = (float)(prcCallouts->bottom - prcCallouts->top);
	}

	//  If we use the maximum width allowed, is the height still small
    //  enough to fit within the caller's rectangle?
    if((fMaxWidth * fAspectRatio) < fMaxHeight)
    {
        //  Use the full width allowed and adjust the height
        fPrintWidth  = fMaxWidth;
        fPrintHeight = fPrintWidth * fAspectRatio;

        fCallWidth  = fMaxCallWidth;
        fCallHeight = fCallWidth * fAspectRatio;
    }
    else
    {   
        //  Use the maximum height available and adjust the width
        fPrintHeight = fMaxHeight;
        fPrintWidth  = fPrintHeight / fAspectRatio;

        fCallHeight = fMaxCallHeight;
        fCallWidth  = fCallHeight / fAspectRatio;
    }

	//	Calculate the scale factor used to convert from print rectangle units
	//	to screen pixels. This is used to properly position callouts within
	//	the print rectangle. The position of a callout relative to the pane 
	//	window is calculated (pixels) and then converted to print rectangle
	//	units to position it within the desired rectangle
	if(sRotation != 0)
		m_fPrintFactor = fPrintHeight / (float)m_iWidth;
	else
		m_fPrintFactor = fPrintWidth / (float)m_iWidth;
//m_fPrintFactor *= 0.9;
/*
	if(sRotation != 0)
		m_fPrintFactor = fCallHeight / (float)m_iWidth;
	else
		m_fPrintFactor = fCallWidth / (float)m_iWidth;
*/

    //  Calculate the coordinates of the upper left corner so that the scaled
	//	image is centered in the print area
    fPrintLeft = m_fPrintCx - (fPrintWidth / 2.0f);
    fPrintTop  = m_fPrintCy - (fPrintHeight / 2.0f);

	//	Can we print as a DIB?
	bRenderAsDIB = pScratch->GetRenderAsDIB(pdc->GetSafeHdc(), (int)fPrintWidth, (int)fPrintHeight);

	//	Realize the image and annotations before printing if required
	//
	//	NOTE:	We do this because some printers (usually Postscript) get the
	//			Z order wrong on the annotations
	if((m_Annotations.GetCount() > 0) && (bRealized == FALSE))
	{
		if((bRenderAsDIB == TRUE) || (IsPostScript(pdc->GetSafeHdc())))
		{
			pScratch->Realize(TRUE);
			bRealized = TRUE;
		}

	}
		
	//	LeadTools must optimize it's drawing because not all changes to the scratch
	//	pane get committed immediately because the pane is not visible. This will force
	//	the changes to be committed before we print
	GetClientRect(&rcClient);
	pScratch->MoveWindow(&rcClient, FALSE);

	//	Print the image
	if(bRenderAsDIB == TRUE)
	{
		//	Attempt to render as a DIB
		//
		//	NOTE:	This is preferred to keep the spool file size small
		if(!pScratch->RenderDIB(pdc->GetSafeHdc(), (int)fPrintLeft, (int)fPrintTop, (int)fPrintWidth, (int)fPrintHeight))
			pScratch->Render((long)pdc->GetSafeHdc(), fPrintLeft, fPrintTop, fPrintWidth, fPrintHeight);
	}
	else
	{
		pScratch->Render((long)pdc->GetSafeHdc(), fPrintLeft, fPrintTop, fPrintWidth, fPrintHeight);
	}

	//	Put the scratch window back where it belongs
	pScratch->MoveWindow(0,0,1,1, FALSE);
	
	#ifdef _DEBUG
	if(fptr != NULL)
	{
		fprintf(fptr, "RenderAsDIB = %s\n", bRenderAsDIB ? "TRUE" : "FALSE");
		fprintf(fptr, "Ann Realized = %s\n", bRealized ? "TRUE" : "FALSE");
		fprintf(fptr, "fMaxWidth = %f\n", fMaxWidth);
		fprintf(fptr, "fMaxHeight = %f\n", fMaxHeight);
		fprintf(fptr, "fPrintCx = %f\n", m_fPrintCx);
		fprintf(fptr, "fPrintCy = %f\n", m_fPrintCy);
		fprintf(fptr, "fPrintWidth = %f\n", fPrintWidth);
		fprintf(fptr, "fPrintHeight = %f\n", fPrintHeight);
		fprintf(fptr, "fPrintLeft = %f\n", fPrintLeft);
		fprintf(fptr, "fPrintTop = %f\n", fPrintTop);
		fprintf(fptr, "m_fPrintFactor = %f\n", m_fPrintFactor);
		fprintf(fptr, "Elapsed time = %ld ms", GetTickCount() - dwStartTime);
		fclose(fptr);
	}
	#endif

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTMLead::PrintBorder()
//
// 	Description:	This function will print a border around the specified
//					rectangle.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::PrintBorder(CDC* pdc, COLORREF crColor, float fThickness, 
						  RECT* pRect) 
{
	int		iPixels;
	int		iLeft;
	int		iTop;
	int		iBottom;
	int		iRight;
	CPen	Pen;
	CPen*	pOldPen;
	CBrush*	pOldBrush;

	ASSERT(pdc);
	ASSERT(fThickness > 0);
	ASSERT(pRect);

	//	Convert from inches to pixels
	//
	//	NOTE:	We are assuming the printer resolution is the same in
	//			both directions
	iPixels = ROUND((float)pdc->GetDeviceCaps(LOGPIXELSY) * fThickness);

	//	Initialize the device context
	Pen.CreatePen(PS_SOLID, 1, crColor);
	pOldPen = pdc->SelectObject(&Pen);
	pOldBrush = (CBrush*)pdc->SelectStockObject(NULL_BRUSH);

	//	Set the initial coordinates
	iLeft   = pRect->left;
	iTop    = pRect->top;
	iRight  = pRect->right + 1;
	iBottom = pRect->bottom + 1;

	//	Draw the border by drawing successive rectangles
	//
	//	NOTE:	We do it this way rather than draw one rectangle with a pen
	//			that is iPixels thick because we want to keep the entire border
	//			within the rectangle defined by the caller. A thick pen uses the
	//			coordinates as the center of the line which means that half the
	//			border falls outside the rectangle boundry.
	for(int i = 0; i < iPixels; i++)
	{
		pdc->Rectangle(iLeft, iTop, iRight, iBottom);

		//	Move in one pixel at a time
		iLeft   += 1;
		iRight  -= 1;
		iTop    += 1;
		iBottom -= 1;
	}

	//	Restore the dc
	if(pOldPen)
		pdc->SelectObject(pOldPen);
	if(pOldBrush)
		pdc->SelectObject(pOldBrush);

	//	Adjust the caller's rectangle
	pRect->left   += iPixels;
	pRect->right  -= iPixels;
	pRect->top    += iPixels;
	pRect->bottom -= iPixels;
}

//==============================================================================
//
// 	Function Name:	CTMLead::PrintCallout()
//
// 	Description:	This function is called to print the specified callout using
//					the device context and rectangle specified by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::PrintCallout(CCallout* pCallout, CDC* pdc, RECT* pRect, short sRotation) 
{
	RECT	rcCallWnd;
	RECT	rcPaneWnd;
	RECT	rcPrint;
	double	dPaneWndCx;
	double	dPaneWndCy;
	double	dPaneWndWidth;
	double	dPaneWndHeight;
	double	dCallWndCx;
	double	dCallWndCy;
	double	dCallWndWidth;
	double	dCallWndHeight;
	double	dScreenXOffset;
	double	dScreenYOffset;
	double	dPrintXOffset;
	double	dPrintYOffset;
	double	dPrintWidth;
	double	dPrintHeight;
	double	dPrintCx;
	double	dPrintCy;

	ASSERT(pCallout);
	ASSERT(pdc);
	ASSERT(pRect);

	//	Get the current position of the callout and pane windows
	GetWindowRect(&rcPaneWnd);
	pCallout->GetWindowRect(&rcCallWnd);

	//	Calculate the size and center point of the callout window
	dCallWndWidth  = (double)(rcCallWnd.right - rcCallWnd.left);
	dCallWndHeight = (double)(rcCallWnd.bottom - rcCallWnd.top);
	dCallWndCx     = (double)rcCallWnd.left + (dCallWndWidth / 2.0);
	dCallWndCy     = (double)rcCallWnd.top  + (dCallWndHeight / 2.0);

	//	Calculate the size and center point of the pane window
	dPaneWndWidth  = (double)(rcPaneWnd.right - rcPaneWnd.left);
	dPaneWndHeight = (double)(rcPaneWnd.bottom - rcPaneWnd.top);
	dPaneWndCx     = (double)rcPaneWnd.left + (dPaneWndWidth / 2.0);
	dPaneWndCy     = (double)rcPaneWnd.top  + (dPaneWndHeight / 2.0);

	//	Calculate the offset between the two windows as they appear on the screen
	dScreenXOffset = dCallWndCx - dPaneWndCx;
	dScreenYOffset = dCallWndCy - dPaneWndCy;

	//	Calculate the print size and offset allowing for rotation
	if(sRotation < 0)
	{
		dPrintXOffset = dScreenYOffset;
		dPrintYOffset = dScreenXOffset * -1.0;
		dPrintWidth   = dCallWndHeight;
		dPrintHeight  = dCallWndWidth;
	}
	else if(sRotation > 0)
	{
		dPrintXOffset = dScreenYOffset * -1.0;
		dPrintYOffset = dScreenXOffset;
		dPrintWidth   = dCallWndHeight;
		dPrintHeight  = dCallWndWidth;
	}
	else
	{
		dPrintXOffset = dScreenXOffset;
		dPrintYOffset = dScreenYOffset;
		dPrintWidth   = dCallWndWidth;
		dPrintHeight  = dCallWndHeight;
	}

	//	Convert from screen coordinates to print rectangle coordinates
	//	using the screen to print factor calculated when the source image
	//	was printed
	dPrintXOffset *= m_fPrintFactor;
	dPrintYOffset *= m_fPrintFactor;
	dPrintWidth   *= m_fPrintFactor;
	dPrintHeight  *= m_fPrintFactor;

	//	Calculate the center point of the desired print rectangle
	dPrintCx = ((double)m_fPrintCx) + dPrintXOffset;
	dPrintCy = ((double)m_fPrintCy) + dPrintYOffset;

	//	Calculate the coordinates of the print rectangle
	rcPrint.left   = ROUND(dPrintCx - (dPrintWidth / 2.0));
	rcPrint.top    = ROUND(dPrintCy - (dPrintHeight / 2.0));
	rcPrint.right  = rcPrint.left + ROUND(dPrintWidth);
	rcPrint.bottom = rcPrint.top + ROUND(dPrintHeight);

	//	Erase the background
	//
	//	NOTE:	If we don't do this then any annotation that lies under
	//			the callout will show through
	Erase(pdc, &rcPrint);

	//	Print the border first
	if(m_bPrintCalloutBorders)
		PrintBorder(pdc, m_crCallFrame, m_fPrintBorderThickness, &rcPrint);

	//	Print the callout image
	pCallout->Print(pdc, &rcPrint, sRotation);

}

//==============================================================================
//
// 	Function Name:	CTMLead::PrintCallouts()
//
// 	Description:	This function is called to print all the callouts associated
//					with this pane.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::PrintCallouts(CDC* pdc, RECT* pRect, short sRotation) 
{
	POSITION	Pos;
	CCallout*	pCallout;

	ASSERT(pdc);
	ASSERT(pRect);

	if(m_pCallouts)
	{
		Pos = m_pCallouts->GetHeadPosition();
		while(Pos)
		{
			if((pCallout = (CCallout*)m_pCallouts->GetNext(Pos)) != 0)
				PrintCallout(pCallout, pdc, pRect, sRotation);
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::Rasterize()
//
// 	Description:	This function is called to rasterize the image and 
//					annotations into a scratch pane prior to printing and / or 
//					saving the bitmap.
//
// 	Returns:		A pointer to the scratch pane if successful
//
//	Notes:			None
//
//==============================================================================
CTMLead* CTMLead::Rasterize() 
{
	CTMLead*		pScratch = 0;
	HGLOBAL			hAnnMem  = 0;
	long			lAnnSize = 0;
	RECT			rcRect;
	RECT			rcClip;

	//	Do we have a bitmap to work with?	
	ASSERT(GetBitmap() != 0);
	if(GetBitmap() == 0)
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
	pScratch->SetBitmap(GetBitmap());

	//	Set the source rectangles
	GetSrcRects(&rcRect, &rcClip);
	pScratch->SetSrcRects(&rcRect, &rcClip);

	//	Set the destination rectangles
	GetDstRects(&rcRect, &rcClip);
	pScratch->SetDstRects(&rcRect, &rcClip);

	//	Do we have any annotations?
	if((GetAnnContainer() != 0) && (m_Annotations.GetCount() > 0))
	{
		//	Save the annotations to memory
		AnnSaveMemory((long FAR *)&hAnnMem, ANNFMT_XML, FALSE, &lAnnSize, SAVE_OVERWRITE, 1);

		//	Set the annotations for the scratch pane
		if((hAnnMem != 0) && (lAnnSize > 0))
		{
			pScratch->SetAnnotations(hAnnMem, lAnnSize);

			//	Realize the annotations in the scratch pane
			pScratch->Realize(TRUE);

			//	Release the annotation buffer
			GlobalFree(hAnnMem);
		}
	}

	return pScratch;
}

//==============================================================================
//
// 	Function Name:	CTMLead::ReadZapCallout()
//
// 	Description:	This function will read the information required to create
//					a callout from the zap file provided by the caller
//
// 	Returns:		TRUE if successful.
//
//	Notes:			This function assumes the file pointer is sitting on the
//					appropriate callout descriptor in the zap file.
//
//					It is up to the caller to deallocate the annotation memory
//
//==============================================================================
BOOL CTMLead::ReadZapCallout(CFile* pFile, SZapCallout* pHeader,
							 HGLOBAL* phAnnMem)
{
	LPVOID	lpAnnMem;
	char	szErrorMsg[256];

	ASSERT(pFile);
	ASSERT(pFile->m_hFile != CFile::hFileNull);
	ASSERT(pHeader);
	ASSERT(phAnnMem);

	//	Now read the callout information from the file
	try
	{
		//	Read the callout header
		if(pFile->Read((void*)pHeader, sizeof(SZapCallout)) != sizeof(SZapCallout))
			return FALSE;

		//	Allocate memory for the annotations if necessary
		if(pHeader->lAnnBytes > 0)
		{
			if((*phAnnMem = GlobalAlloc(GMEM_MOVEABLE, pHeader->lAnnBytes)) == 0)
				return FALSE;

			//	Lock the memory so we can read in the file contents
			lpAnnMem = (LPVOID)GlobalLock(*phAnnMem);
			ASSERT(lpAnnMem);

			//	Read the annotations from the file
			if(pFile->Read(lpAnnMem, (UINT)pHeader->lAnnBytes) != (UINT)pHeader->lAnnBytes)
			{
				GlobalUnlock(*phAnnMem);
				GlobalFree(*phAnnMem);
				*phAnnMem = 0;
				return FALSE;
			}

			//	Unlock the memory now
			GlobalUnlock(*phAnnMem);
			lpAnnMem = 0;
		}
		else
		{
			//	No annotations have been defined
			*phAnnMem = 0;
		}

		return TRUE;

	}
	//	Catch all file exceptions
	catch(CFileException* pFileException)
	{
		//	Get the error message and pass it on to the handler
		pFileException->GetErrorMessage(szErrorMsg, sizeof(szErrorMsg));
		if(m_pErrors != 0) m_pErrors->Handle(0, szErrorMsg);
		pFileException->Delete();
		
		//	Free any memory allocated for annotations
		if(*phAnnMem)
			GlobalFree(*phAnnMem);

		return FALSE;
	}
	//	Catch all other exceptions
	catch(CException* pException)
	{
		//	Get the error message and pass it on to the handler
		pException->GetErrorMessage(szErrorMsg, sizeof(szErrorMsg));
		if(m_pErrors != 0) m_pErrors->Handle(0, szErrorMsg);
		pException->Delete();
		
		//	Free any memory allocated for annotations
		if(*phAnnMem)
			GlobalFree(*phAnnMem);

		return FALSE;
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::Realize()
//
// 	Description:	This function allows the caller to realize the current 
//					annotations to the current image.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMLead::Realize(BOOL bRemove) 
{
	//	Increase the color depth before we realize if we are dealing with
	//	a black and white image
	if(GetBitmapBits() == 1)
	{
		//	What color depth are we supposed to use?
		if(m_sAnnColorDepth <= 8)
			ColorRes(8, CRP_FIXEDPALETTE, CRD_NODITHERING, 0);
		else if(m_sAnnColorDepth <= 16)
			ColorRes(16, CRP_OPTIMIZEDPALETTE | CRP_BYTEORDERRGB, CRD_NODITHERING, 0);
		else if(m_sAnnColorDepth <= 24)
			ColorRes(24, CRP_OPTIMIZEDPALETTE | CRP_BYTEORDERRGB, CRD_NODITHERING, 0);
		else
			ColorRes(32, CRP_OPTIMIZEDPALETTE | CRP_BYTEORDERRGB, CRD_NODITHERING, 0);
	}

	if((m_sLeadError = AnnRealize(FALSE)) != 0)
	{
		HandleLeadError(m_sLeadError);
		return TMV_LEADERROR;
	}
	
	//	Should we remove the editable annotations?
	if((bRemove == TRUE) && (GetAnnContainer() != 0))
	{
		AnnSetSelected((long)GetAnnContainer(), TRUE, FALSE);
		AnnDestroy((long)GetAnnContainer(), ANNFLAG_RECURSE | ANNFLAG_SELECTED);
	}

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMLead::Redraw()
//
// 	Description:	This function will redraw the current image.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::Redraw()
{
	//	What is the current zoomed state?
	switch(m_sZoomState)
	{
		case ZOOMED_FULLWIDTH:		RedrawFullWidth();
									break;

		case ZOOMED_FULLHEIGHT:		RedrawFullHeight();
									break;

		case ZOOMED_USER:			RedrawZoomed();
									break;

		case ZOOMED_ZAP:			RedrawZap();
									break;

		case ZOOMED_NONE:			
		default:					RedrawNormal();
									break;
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::RedrawFullHeight()
//
// 	Description:	This function will redraw the image using the full available
//					height
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::RedrawFullHeight() 
{
	float	fDstHeight;
	float	fDstWidth;
	float	fXFactor;
	float	fYFactor;

	//	Don't bother if not loaded
	if(!IsLoaded())
		return;

	//	Always hide the Lead control
	if(IsWindow(m_hWnd))
		ShowWindow(SW_HIDE);

	//	We are going to use the full available window height
	fDstHeight = (float)m_rcMax.bottom;
	fDstWidth = fDstHeight / m_fAspectRatio;

	SetDstRect(0, 0, fDstWidth, fDstHeight);
	SetDstClipRect(0, 0, fDstWidth, fDstHeight);

	//	Enable the auto scroll bars if we are not panning
	SetAutoScroll(!m_bHideScrollBars);

	//	Resize the lead control 
	ResizeWndToDst();
		
	//	We reset the destination rectangle once again because the call to
	//	ResizeToDestination() may inadvertently reset the rectangle when
	//	it moves the window
	SetDstRect(0, 0, fDstWidth, fDstHeight);
	SetDstClipRect(0, 0, fDstWidth, fDstHeight);

	//	Determine the new zoom factor
	fXFactor = GetDstWidth() / GetSrcWidth();
	fYFactor = GetDstHeight() / GetSrcHeight();
	m_fZoomFactor = (fXFactor > fYFactor) ? fXFactor : fYFactor;

	//	Change the zoomed state
	m_sZoomState = ZOOMED_FULLHEIGHT;
	
	//	Make sure the window is visible. 
	if(!IsWindowVisible())
		ShowWindow(SW_SHOW);

	//  Repaint the image
	GetParent()->RedrawWindow();

}

//==============================================================================
//
// 	Function Name:	CTMLead::RedrawFullWidth()
//
// 	Description:	This function will redraw the image using the maximum
//					available width
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::RedrawFullWidth() 
{
	float	fDstHeight;
	float	fDstWidth;
	float	fDstTop;
	float	fOldTop;
	float	fOldHeight;
	float	fXFactor;
	float	fYFactor;

	//	Don't bother if not loaded
	if(!IsLoaded())
		return;

	//	Always hide the Lead control
	if(IsWindow(m_hWnd))
		ShowWindow(SW_HIDE);

	fOldTop	   = GetDstTop();
	fOldHeight = GetDstHeight();

	//	We are going to use the full available window width
	fDstWidth = (float)m_rcMax.right;
	fDstHeight = fDstWidth * m_fAspectRatio;

	//	Adjust the top coordinate so that we are looking at the same portion
	//	of the image
	if(fOldTop == 0)
	{
		fDstTop = 0;
	}
	else
	{
		//	Scale the top coordinate to maintain the same position within
		//	the image
		fDstTop = fOldTop * (fDstHeight / fOldHeight);
	}

	SetDstRect(0, fDstTop, fDstWidth, fDstHeight);
	SetDstClipRect(0, fDstTop, fDstWidth, fDstHeight);

	//	Enable the auto scroll bars if we are not panning
	SetAutoScroll(!m_bHideScrollBars);

	//	Resize the lead control 
	ResizeWndToDst();
		
	//	Make sure we consume all the available area
	if(fDstTop < ((float)m_iHeight - fDstHeight))
		fDstTop = ((float)m_iHeight - fDstHeight);
	if(fDstTop > 0)
		fDstTop = 0;

	//	We reset the destination rectangle once again because the call to
	//	ResizeToDestination() may inadvertently reset the rectangle when
	//	it moves the window
	SetDstRect(0, fDstTop, fDstWidth, fDstHeight);
	SetDstClipRect(0, fDstTop, fDstWidth, fDstHeight);

	//	Change the zoomed state
	m_sZoomState = ZOOMED_FULLWIDTH;
	
	//	Determine the new zoom factor
	fXFactor = GetDstWidth() / GetSrcWidth();
	fYFactor = GetDstHeight() / GetSrcHeight();
	m_fZoomFactor = (fXFactor > fYFactor) ? fXFactor : fYFactor;

	//	Make sure the window is visible. 
	if(!IsWindowVisible())
		ShowWindow(SW_SHOW);

	//  Repaint the image
	GetParent()->RedrawWindow();
}

//==============================================================================
//
// 	Function Name:	CTMLead::RedrawNormal()
//
// 	Description:	This function will redraw the image. If the m_bScaleImage
//					property is TRUE the full image will be visible.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::RedrawNormal() 
{
	float	fDstTop;
	float	fDstLeft;
	float	fDstHeight;
	float	fDstWidth;
	float	fXFactor;
	float	fYFactor;

	//	Don't bother if not loaded
	if(!IsLoaded())
		return;

	//	Always hide the Lead control
	if(IsWindow(m_hWnd))
		ShowWindow(SW_HIDE);

    //	Resize the destination rectangles if we are scaling the image
	if(m_bScaleImage)
	{
		//  Always disable the scroll bars because the entire image always
		//	fits within the window when we are scaling
		SetAutoScroll(FALSE);

		//	Resize the lead control
		if(m_bKeepAspect)
			ResizeWndToSrc();
		else
			ResizeWndToMax();

		// Set the image display size to match that of the lead control
		SetDstRect(0.0f, 0.0f, GetScaleWidth(), GetScaleHeight()); 
		SetDstClipRect(0.0f, 0.0f, GetScaleWidth(), GetScaleHeight());

		//ShowRectangles("Scaled");	
	}
	else
	{
		//	Enable the auto scroll bars if we are not panning
		SetAutoScroll(!m_bHideScrollBars);

		//  Find the current size and position of the destination rectangle.
		fDstLeft   = GetDstLeft();
		fDstTop	   = GetDstTop();
		fDstWidth  = GetDstWidth();
		fDstHeight = GetDstHeight();

		//	Resize the lead control 
		ResizeWndToDst();
		
		//	Since the window size and position may have changed, check to make
		//	sure we don't have to reposition the destination rectangle
		//
		//	Note: fDstLeft and fDstTop are always less than or equal to 0
		if((fDstWidth + fDstLeft) < m_iWidth)
			fDstLeft += ((float)m_iWidth - (fDstWidth + fDstLeft));
		if((fDstHeight + fDstTop) < m_iHeight)
			fDstHeight += ((float)m_iHeight - (fDstHeight + fDstTop));

		//	This should never happen but just in case it does
		if(fDstLeft > 0)
			fDstLeft = 0;
		if(fDstTop > 0)
			fDstTop = 0;

		//	Reset the destination rectangle 
		SetDstRect(fDstLeft, fDstTop, fDstWidth, fDstHeight);
		SetDstClipRect(fDstLeft, fDstTop, fDstWidth, fDstHeight);

		//ShowRectangles("Unscaled");
	}
	
	//	Determine the new zoom factor
	fXFactor = GetDstWidth() / GetSrcWidth();
	fYFactor = GetDstHeight() / GetSrcHeight();
	m_fZoomFactor = (fXFactor > fYFactor) ? fXFactor : fYFactor;

	//	Change the zoomed state
	m_sZoomState = ZOOMED_NONE;

	//	Make sure the window is visible. 
	if(!IsWindowVisible())
		ShowWindow(SW_SHOW);

	//  Repaint the image
	GetParent()->RedrawWindow();
}

//==============================================================================
//
// 	Function Name:	CTMLead::RedrawZap()
//
// 	Description:	This function will redraw the viewport as defined by the
//					zap dimensions and scaling options.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::RedrawZap() 
{
	float	fDstTop;
	float	fDstLeft;
	float	fDstHeight;
	float	fDstWidth;
	float	fXFactor;
	float	fYFactor;
	RECT	rcMax;

	//	Don't bother if not loaded
	if(!IsLoaded())
		return;

	// Hiding the window
	ShowWindow(SW_HIDE);

	//	Enable the auto scroll bars if we are not panning
	SetAutoScroll(!m_bHideScrollBars);

    //	Are we using the aspect ratio of the bounding rectangle
	//	stored in the zap file?
	if(m_fZapRatio > 0 && !m_bSplitScreen)
	{
		//	Substitute the adjusted bounding rectangle for the normal
		//	maximum rectangle
		memcpy(&rcMax, &m_rcMax, sizeof(rcMax));
		memcpy(&m_rcMax, &m_rcZapMax, sizeof(m_rcMax));
	}

	//	Resize the destination rectangles if we are scaling the image
	if(m_bScaleImage)
	{
		//	Resize the viewport
		ResizeView();
	}
	else
	{
		//  Find the current size and position of the destination rectangle.
		fDstLeft   = GetDstLeft();
		fDstTop	   = GetDstTop();
		fDstWidth  = GetDstWidth();
		fDstHeight = GetDstHeight();

		//	Resize the lead control to view as much of the destination as
		//	possible
		ResizeWndToDst();
		
		//	Since the window size and position may have changed, check to make
		//	sure we don't have to reposition the destination rectangle
		//
		//	Note: fDstLeft and fDstTop are always less than or equal to 0
		if((fDstWidth + fDstLeft) < m_iWidth)
			fDstLeft += ((float)m_iWidth - (fDstWidth + fDstLeft));
		if((fDstHeight + fDstTop) < m_iHeight)
			fDstHeight += ((float)m_iHeight - (fDstHeight + fDstTop));

		//	This should never happen but just in case it does
		if(fDstLeft > 0)
			fDstLeft = 0;
		if(fDstTop > 0)
			fDstTop = 0;

		//	Reset the destination rectangle 
		SetDstRect(fDstLeft, fDstTop, fDstWidth, fDstHeight);
		SetDstClipRect(fDstLeft, fDstTop, fDstWidth, fDstHeight);

		//ShowRectangles("Unscaled");
	}
	
	//	Determine the new zoom factor
	fXFactor = GetDstWidth() / GetSrcWidth();
	fYFactor = GetDstHeight() / GetSrcHeight();
	m_fZoomFactor = (fXFactor > fYFactor) ? fXFactor : fYFactor;

	//	Change the zoomed state
	m_sZoomState = ZOOMED_ZAP;

    //	Restore the maximum rectangle
	if(m_fZapRatio > 0 && !m_bSplitScreen)
		memcpy(&m_rcMax, &rcMax, sizeof(m_rcMax));

	//	Make sure the window is visible. 
	if(!IsWindowVisible())
		ShowWindow(SW_SHOW);

	//  Repaint the image
	GetParent()->RedrawWindow();
}

//==============================================================================
//
// 	Function Name:	CTMLead::RedrawZoomed()
//
// 	Description:	This function will redraw the image using the current
//					zoomed rectangles
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::RedrawZoomed() 
{
	float	fDstTop;
	float	fDstLeft;
	float	fDstHeight;
	float	fDstWidth;
	float	fXFactor;
	float	fYFactor;

	//	Don't bother if not loaded
	if(!IsLoaded())
		return;

	//	Always hide the Lead control
	if(IsWindow(m_hWnd))
		ShowWindow(SW_HIDE);

	//	Enable the auto scroll bars if we are not panning
	SetAutoScroll(!m_bHideScrollBars);

	//	Are we restricting the zoomed view?
	if(m_bZoomToRect)
	{
		ResizeView();
	}
	else
	{
		//  Find the current size and position of the destination rectangle.
		fDstLeft   = GetDstLeft();
		fDstTop	   = GetDstTop();
		fDstWidth  = GetDstWidth();
		fDstHeight = GetDstHeight();

		//	Resize the lead control 
		ResizeWndToDst();
		
		//	Since the window size may have changed we might have to enlarge
		//	the visible portion of the destination rectangle
		//
		//	Note: fDstLeft and fDstTop are always less than or equal to 0
		if((fDstWidth + fDstLeft) < m_iWidth)
			fDstLeft += ((float)m_iWidth - (fDstWidth + fDstLeft));
		if((fDstHeight + fDstTop) < m_iHeight)
			fDstHeight += ((float)m_iHeight - (fDstHeight + fDstTop));

		//	This should never happen but just in case it does
		if(fDstLeft > 0)
			fDstLeft = 0;
		if(fDstTop > 0)
			fDstTop = 0;

		//	We reset the destination rectangle once again because the call to
		//	ResizeToDestination() may inadvertently reset the rectangle when
		//	it moves the window
		SetDstRect(fDstLeft, fDstTop, fDstWidth, fDstHeight);
		SetDstClipRect(fDstLeft, fDstTop, fDstWidth, fDstHeight);

	}

	//	Determine the new zoom factor
	ASSERT(GetSrcWidth() != 0);
	ASSERT(GetSrcHeight() != 0);
	fXFactor = GetDstWidth() / GetSrcWidth();
	fYFactor = GetDstHeight() / GetSrcHeight();
	m_fZoomFactor = (fXFactor > fYFactor) ? fXFactor : fYFactor;

	//	Change the zoomed state
	m_sZoomState = ZOOMED_USER;
	
	//	Make sure the window is visible. 
	if(!IsWindowVisible())
		ShowWindow(SW_SHOW);

	//  Repaint the image
	GetParent()->RedrawWindow();

}

//==============================================================================
//
// 	Function Name:	CTMLead::Render()
//
// 	Description:	This function will render the image into the dc provided
//					by the caller into the position provided by the caller.
//
// 	Returns:		0 if successful
//					TMV_NOIMAGE if not loaded
//					LeadTools error level otherwise
//
//	Notes:			None
//
//==============================================================================
short CTMLead::Render(OLE_HANDLE hDc, float fLeft, float fTop, 
					  float fWidth, float fHeight)
{
	short sReturn;

	//	Is there an image to render?
	if(GetBitmap() == 0)
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_NOIMAGE);
		return TMV_NOIMAGE;
	}

	//	Disable the error messages while we do the rendering
	//
	//	We need to do this because LeadTools renders in the background and
	//	the HDC being used may get destroyed before it finishes rendering
	SetEnableMethodErrors(FALSE);
	
	//	Turn resampling off or else printing gets very slow
	SetPaintScaling(PAINTSCALING_NORMAL);

	sReturn = CLead::Render((long)hDc, fLeft, fTop, fWidth, fHeight);

	//	Turn resampling back on
	SetPaintScaling(PAINTSCALING_RESAMPLE);

	return sReturn;
}

//==============================================================================
//
// 	Function Name:	CTMLead::RenderDIB()
//
// 	Description:	This function will render the image into the dc provided
//					by the caller into the position provided by the caller.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLead::RenderDIB(HDC hdc, int iLeft, int iTop, int iWidth, int iHeight)
{
	HGLOBAL			hDIB = NULL;
	BYTE*			pBitmapBits = NULL;
	LPBITMAPINFO	lpBitmapInfo = NULL;
	int				iBitmapHeight = 0;
	int				iBitmapWidth = 0;
	int				iColors = 0;

	//	Get the current image as a device independent bitmap
	if((hDIB = (HGLOBAL)GetDIB(DIB_BITMAPINFOHEADER)) != NULL)
	{
		lpBitmapInfo = (LPBITMAPINFO)GlobalLock(hDIB);

		iBitmapWidth = (int)(lpBitmapInfo->bmiHeader.biWidth);
		iBitmapHeight = (int)(lpBitmapInfo->bmiHeader.biHeight);
		
		//	How many colors are in the image?
		if(lpBitmapInfo->bmiHeader.biBitCount <= 8)
			iColors = 1 << lpBitmapInfo->bmiHeader.biBitCount;
		else
			iColors = 0; //	No color info required when > 256

		//	Get a pointer to the actual bitmap bytes
		pBitmapBits = (BYTE*)lpBitmapInfo + (WORD)sizeof(BITMAPINFOHEADER) +
					  (iColors * sizeof (RGBQUAD));
		
		//	Instruct the dc to scale the image into the caller defined rectangle
		StretchDIBits((HDC)hdc,
					  iLeft, 
					  iTop, 
					  iWidth, 
					  iHeight,
					  0, 
					  0, 
					  iBitmapWidth, 
					  iBitmapHeight, 
					  pBitmapBits, 
					  lpBitmapInfo,
                      DIB_RGB_COLORS, SRCCOPY);

		GlobalUnlock(hDIB);
		GlobalFree(hDIB);
	
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTMLead::RescaleZapCallouts()
//
// 	Description:	This function is called to force rescaling of the callouts
//					to match the current zap aspect ratio.
//
// 	Returns:		None
//
//	Notes:			This is only performed if currently in ZOOM_ZAP mode.
//
//==============================================================================
void CTMLead::RescaleZapCallouts() 
{
	RECT		rcContainer;
	CCallout*	pCallout;

	//	Don't bother if not viewing a zap
	if(m_sZoomState != ZOOMED_ZAP) return;

	if((m_pCallouts != 0) && (m_pCallouts->GetCount() > 0))
	{
		//	Are we loading into split screen?
		if(m_bSplitScreen)
		{
			//	Use the whole control as the current container
			ASSERT(m_pControl);
			m_pControl->GetWindowRect(&rcContainer);
		}
		else
		{
			//	Use the maximum rectangle as the current container
			//
			//	NOTE:	m_rcMax.right = width (not absolute coordinate)
			//			m_rcMax.bottom = height (not absolute coordinate)

			rcContainer.left = m_rcMax.left;
			rcContainer.top = m_rcMax.top;
			rcContainer.right = m_rcMax.left + m_rcMax.right;
			rcContainer.bottom = m_rcMax.top + m_rcMax.bottom;
			GetParent()->ClientToScreen(&rcContainer);
		}

		//	Notify all of the callouts
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetContainer(&rcContainer);
			pCallout->Rescale();
			pCallout = m_pCallouts->Next();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::ResetZapParameters()
//
// 	Description:	This function will reset the members used to draw images
//					loaded from a zap file
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::ResetZapParameters()
{
	m_fZapFactor   = 0.0;
	m_fZapRatio    = 0.0;
	m_fZapWidth    = 0.0;
	m_fZapHeight   = 0.0;
	memset(&m_rcZapControl, 0, sizeof(m_rcZapControl));
	memset(&m_rcZapMax, 0, sizeof(m_rcZapMax));

	if(m_sZoomState == ZOOMED_ZAP)
		m_sZoomState = ZOOMED_USER;
}

//==============================================================================
//
// 	Function Name:	CTMLead::ResetZoom()
//
// 	Description:	This function will reset the zoom factor to 1 and redraw
//					the image.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::ResetZoom()
{
	//	Reset the destination rectangles 
	SetDstRect(0.0f, 0.0f, m_fImageWidth, m_fImageHeight);
	SetDstClipRect(0.0f, 0.0f, m_fImageWidth, m_fImageHeight);

	//	Redraw the image
	RedrawNormal();

}

//==============================================================================
//
// 	Function Name:	CTMLead::ResizeSourceToImage()
//
// 	Description:	This function will resize the source rectangle to match
//					the current full image.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::ResizeSourceToImage()
{
	if(!IsLoaded())
		return;

	//	Set the source rectangles 
	SetSrcRect(0.0, 0.0, m_fImageWidth, m_fImageHeight);
	SetSrcClipRect(0.0, 0.0, m_fImageWidth, m_fImageHeight);
}

//==============================================================================
//
// 	Function Name:	CTMLead::ResizeSourceToView()
//
// 	Description:	This function will resize the source rectangle to match
//					the current visible portion of the image.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::ResizeSourceToView()
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

	if(!IsLoaded())
		return;

	//	Get the dimensions of the current view rectangle
	fTop    = GetDstTop();
	fLeft   = GetDstLeft();
	fWidth  = GetDstWidth();
	fHeight = GetDstHeight();
	
	//	Don't bother resizing the source if the entire destination is visible
	if(fTop == 0 && fLeft == 0 && fWidth <= m_iWidth && fHeight <= m_iHeight)
		return;
	
	//	Calculate the current scale factors of the current visible portion of the 
	//	image relative to the source rectangle. We assume the source rectangle is 
	//	for the full image
	fXScale = fWidth / m_fImageWidth;
	fYScale = fHeight / m_fImageHeight;

	//	Calculate the dimensions of the source rectangle using the specified
	//	width and height
	fSrcWidth  = (float)m_iWidth / fXScale;
	fSrcHeight = (float)m_iHeight / fYScale;
	fSrcLeft   = ((-1 * fLeft) / fXScale);
	fSrcTop    = ((-1 * fTop) / fYScale);

	//	Set the source rectangles 
	SetSrcRect(fSrcLeft, fSrcTop, fSrcWidth, fSrcHeight);
	SetSrcClipRect(fSrcLeft, fSrcTop, fSrcWidth, fSrcHeight);
}

//==============================================================================
//
// 	Function Name:	CTMLead::ResizeToRatio()
//
// 	Description:	This function will adjust the size of the specified 
//					rectangle to match use the requested ratio.
//
// 	Returns:		TRUE if successful
//
//	Notes:			pRect->right  = Width (not right hand coordinate)
//					pRect->bottom = Height (not bottom coordinate)
//
//==============================================================================
BOOL CTMLead::ResizeToRatio(RECT* pRect, float fRatio)
{
    float   fWidth;
    float   fHeight;
    int     fCx;
    int     fCy;

	ASSERT(pRect != 0);
	ASSERT(fRatio != 0);
	if(fRatio == 0) return FALSE;

	//  Find the center point of the rectangle
    fCx = (pRect->right / 2) + pRect->left;
    fCy = (pRect->bottom / 2) + pRect->top;

	//  If we use the maximum width allowed, is the height still small
    //  enough to fit on the screen?
    if((pRect->right * fRatio) < pRect->bottom)
    {
        //  Use the full width allowed and adjust the height
        fWidth  = (float)pRect->right;
        fHeight = fWidth * fRatio;
    }
    else
    {   
        //  Use the maximum height available and adjust the width
        fHeight = (float)pRect->bottom;
        fWidth = fHeight / fRatio;
    }

    if((fWidth <= 0) || (fHeight <= 0)) return FALSE;

	//  Calculate the new coordinates of the upper left corner
    pRect->left = (long)(fCx - (fWidth / 2));
    pRect->top  = (long)(fCy - (fHeight / 2));

	//	Update the dimensions
	pRect->right = (long)fWidth;
	pRect->bottom = (long)fHeight;

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTMLead::ResizeView()
//
// 	Description:	This function will resize the window and destination 
//					rectangle to maintain the current view of the image
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::ResizeView()
{
	BOOL	bFullSource = FALSE;
	ANNRECT	rcSrcVisible;

	if(IsLoaded() == TRUE)
	{
		//	Get the visible portion of the source image
		bFullSource = GetSrcVisible(&rcSrcVisible);

		//	Resize the window and maintain its aspect ratio
		ResizeWndToRatio(0,0);

		//	Restore the visible source region
		SetSrcVisible(&rcSrcVisible, bFullSource);
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::ResizeViewToSrc()
//
// 	Description:	This function will resize the window and destination 
//					rectangle to make the specified source region visible
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::ResizeViewToSrc(ANNRECT* prcSrc)
{
	float	fSrcWidth;
	float	fSrcHeight;

	if(!IsLoaded())
		return;

	//	Get the size of the source region
	if((fSrcWidth  = (float)(prcSrc->right - prcSrc->left)) <= 0)
		return;
	if((fSrcHeight = (float)(prcSrc->bottom - prcSrc->top)) <= 0)
		return;

	//	Resize the window to have the same aspect ratio as that of the desired region
	ResizeWndToRatio(fSrcHeight, fSrcWidth);

	//	Make this the visible source region
	SetSrcVisible(prcSrc, FALSE);
}

//==============================================================================
//
// 	Function Name:	CTMLead::ResizeWndToDst()
//
// 	Description:	This function will adjust the size of the image view to the 
//					size of the destination rectangle or the maximum extents, 
//					whichever is less. If we don't need the whole client area,
//					the image is centered in the control window.
//
// 	Returns:		None
//
//	Notes:			It is assumed that the destination rectangle has been set to
//					the desired size BEFORE calling this function.
//
//==============================================================================
void CTMLead::ResizeWndToDst()
{
    int     DstWidth;
    int     DstHeight;
    int     Cx;
    int     Cy;

    //  Find the current size of the destination rectangle
    DstWidth  = ROUND(GetDstWidth());
    DstHeight = ROUND(GetDstHeight());

    //  Determine how large to make the control window
    m_iWidth  = (DstWidth < m_rcMax.right) ? DstWidth : m_rcMax.right;
    m_iHeight = (DstHeight < m_rcMax.bottom) ? DstHeight : m_rcMax.bottom;

    //  Find the center point of the viewport
    Cx = (m_rcMax.right / 2) + m_rcMax.left;
    Cy = (m_rcMax.bottom / 2) + m_rcMax.top;

    //  Calculate the new coordinates of the upper left corner
    m_iLeft = Cx - (m_iWidth / 2);
    m_iTop  = Cy - (m_iHeight / 2);

    //  Set the image view size and offset it to center it in the window. 
    MoveWindow(m_iLeft, m_iTop, m_iWidth, m_iHeight, FALSE);

}

//==============================================================================
//
// 	Function Name:	CTMLead::ResizeWndToMax()
//
// 	Description:	This function will adjust the size of the image view to 
//					match the size of the available window
//
// 	Returns:		None
//
//	Notes:			The aspect ratio of the image in NOT maintained
//
//==============================================================================
void CTMLead::ResizeWndToMax()
{
    //  Update the size and offset using the full window
    m_iLeft   = 0;
    m_iTop    = 0;
    m_iWidth  = m_rcMax.right;
    m_iHeight = m_rcMax.bottom;

    //  Set the control size and offset it to center it in the window. 
    MoveWindow(m_iLeft, m_iTop, m_iWidth, m_iHeight, FALSE);
}

//==============================================================================
//
// 	Function Name:	CTMLead::ResizeWndToRatio()
//
// 	Description:	This function will adjust the size of the view window to
//					match the aspect ratio defined by the caller
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::ResizeWndToRatio(float fHeight, float fWidth)
{
    CPoint  TopLeft;
    float   Width;
    float   Height;
	float	Ratio;
    int     Cx;
    int     Cy;

    //	Should we use the current window dimensions?
	if((fHeight <= 0) || (fWidth <= 0))
	{
		fHeight = (float)m_iHeight;
		fWidth = (float)m_iWidth;
	}
	ASSERT((fHeight > 0) && (fWidth > 0));
	if((fHeight <= 0) || (fWidth <= 0)) return;

	//	Compute the aspect ratio of the current viewport
	Ratio = fHeight / fWidth;

	//  Find the center point of the maximum viewing area
    Cx = (m_rcMax.right / 2) + m_rcMax.left;
    Cy = (m_rcMax.bottom / 2) + m_rcMax.top;

	//  If we use the maximum width allowed, is the height still small
    //  enough to fit on the screen?
    if((m_rcMax.right * Ratio) < m_rcMax.bottom)
    {
        //  Use the full width allowed and adjust the height
        Width  = (float)m_rcMax.right;
        Height = Width * Ratio;
    }
    else
    {   
        //  Use the maximum height available and adjust the width
        Height = (float)m_rcMax.bottom;
        Width = Height / Ratio;
    }

    //  Calculate the new coordinates of the upper left corner
    TopLeft.x = Cx - (ROUND(Width) / 2);
    TopLeft.y = Cy - (ROUND(Height) / 2);

    //  Update the size and offset
    m_iLeft   = TopLeft.x;
    m_iTop    = TopLeft.y;
    m_iWidth  = ROUND(Width);
    m_iHeight = ROUND(Height);

//    //  Set the control size and offset it to center it in the window. 
//	m_fImageWidth=m_iWidth; 
//m_fImageHeight=m_iHeight;
//
//SetDstRect(0.0f, 0.0f, m_fImageWidth, m_fImageHeight);
//		//SetDstClipRect(0.0f, 0.0f, m_fImageWidth, m_fImageHeight);

	MoveWindow(m_iLeft, m_iTop, m_iWidth, m_iHeight, FALSE);
	
}

//==============================================================================
//
// 	Function Name:	CTMLead::ResizeWndToSrc()
//
// 	Description:	This function will adjust the size of the image view to 
//					the size of the source rectangle or the maximum extents,
//					whichever is less.
//
// 	Returns:		None
//
//	Notes:			The aspect ratio of the image is maintained.
//
//==============================================================================
void CTMLead::ResizeWndToSrc()
{
    if((m_fImageHeight > 0) && (m_fImageWidth > 0))
		ResizeWndToRatio(m_fImageHeight, m_fImageWidth);
}

//==============================================================================
//
// 	Function Name:	CTMLead::ResyncAnnotations()
//
// 	Description:	This function will reestablish the links between annotations
//					in the source image and those in the callouts
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::ResyncAnnotations() 
{
	CAnnotation*	pAnn;
	CCallout*		pCallout;

	//	Don't bother if we don't have any callouts
	if(!m_pCallouts || m_pCallouts->IsEmpty())
		return;

	//	Resync each of the local annotations
	pAnn = m_Annotations.First();
	while(pAnn)
	{
		//	Is this a callout?
		if(pAnn->m_bIsCallout)
		{
			//	Don't bother if this is the shaded background
			if(!pAnn->m_bIsCalloutShade)
			{
				//	Find the callout with the same identifier and link it with this
				//	annotation object
				if((pCallout = m_pCallouts->Find(pAnn->m_wId)) != 0)
					pAnn->Add(pCallout);
			}
		}
		else
		{
			//	Check each callout in the list and link it to the source
			//	annotation
			pCallout = m_pCallouts->First();
			while(pCallout)
			{
				if(pCallout->GetAnnFromTag(pAnn->GetAnnTag()))
					pAnn->Add(pCallout);
				pCallout = m_pCallouts->Next();
			}
		}

		pAnn = m_Annotations.Next();
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::Rotate()
//
// 	Description:	This function exposes the base LeadTools function. It 
//					rotates the image without regard for current rotation or
//					view.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::Rotate(short sAngle) 
{
	//	Rotate the image
	CLead::Rotate((sAngle * 100), 1, m_crBackground);
}
    
//==============================================================================
//
// 	Function Name:	CTMLead::Rotate()
//
// 	Description:	This function allows the caller to rotate the image
//					by the amount determined by the rotation property.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::Rotate(BOOL bRedraw) 
{
	double	dAngle = (double)(m_sRotation * 100);
	ANNRECT	rcSource;

	//	Get the portion of the source that is currently visible
	GetSrcVisible(&rcSource);

	//	Rotate the image
	CLead::Rotate((long)dAngle, 1, m_crBackground);

	//	Keep track of the cumulative rotation
	m_sAngle += m_sRotation;
	m_sAngle %= 360;

	//  Calculate the new size and aspect ratio
    m_fImageHeight = GetSrcHeight();
    m_fImageWidth  = GetSrcWidth();
    m_fAspectRatio = m_fImageHeight / m_fImageWidth;

	//	Are we zoomed in on part of the source image?
	if((m_sZoomState == ZOOMED_USER) || (m_sZoomState == ZOOMED_ZAP))
	{
		//	Are we rotating clockwise?
		if(m_sRotation == 90)
		{
			//	Rotate the source region
			RotateSrcRect(&rcSource, TRUE);

			//	Resize and position the view
			ResizeViewToSrc(&rcSource);

		}
		else if(m_sRotation == -90)
		{
			RotateSrcRect(&rcSource, FALSE);
			ResizeViewToSrc(&rcSource);
		}
	
	}// if((m_sZoomState == ZOOMED_USER) || (m_sZoomState == ZOOMED_ZAP))

	//	Do we need to redraw the image?
	if(bRedraw)
		Redraw();

	//	Rotate the callouts if this is the source image
	if(m_pOwner == 0)
	{
		//	Are we rotating clockwise?
		if(m_sRotation == 90)
			RotateCallouts(TRUE, bRedraw);
		else if(m_sRotation == -90)
			RotateCallouts(FALSE, bRedraw);
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::RotateAnnCopy()
//
// 	Description:	This function will rotate the annotation object being copied
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::RotateAnnCopy(CTMLead* pSource, HANNOBJECT hCopy) 
{
	HANNOBJECT	hContainer;
	ANNRECT		AnnRect;
	ANNPOINT	Center;
	int			iDelta;

	//	Calculate the difference between the source angle and this angle
	if((iDelta = pSource->GetAngle() - m_sAngle) == 0)
		return;

	//	Get the root container
	if((hContainer = (HANNOBJECT)GetAnnContainer()) == 0)
		return;

	//	Get the container rectangle
	if(L_AnnGetRect(hContainer, &AnnRect, NULL) != SUCCESS)
		return;

	//	Calculate the center point of the container
	//Center.x = (AnnRect.right - AnnRect.left) / 2.0;
	//Center.y = (AnnRect.bottom - AnnRect.top) / 2.0;
	Center.x = GetSrcWidth() / 2.0 + pSource->GetDstLeft();
	Center.y = GetSrcHeight() / 2.0 + pSource->GetDstTop();

	//	Rotate the annotation	
	L_AnnRotate(hCopy, (double)iDelta, &Center, 0);	
}

//==============================================================================
//
// 	Function Name:	CTMLead::RotateCallouts()
//
// 	Description:	This function will rotate all callouts to the same angle
//					as the base image
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::RotateCallouts(BOOL bClockwise, BOOL bRedraw) 
{
	CCallout* pCallout;

	if(!m_pCallouts)
		return;

	pCallout = m_pCallouts->First();
	while(pCallout)
	{
		pCallout->Rotate(bClockwise, bRedraw);
		pCallout = m_pCallouts->Next();
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::RotateCcw()
//
// 	Description:	This function allows the caller to rotate the image 90
//					degrees counter-clockwise.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::RotateCcw(BOOL bRedraw) 
{
	short sRotation = m_sRotation;

	//	NOTE:	LeadTools has a FastRotate() method but we don't use that 
	//			because it doesn't actually rotate the image. It sets a field
	//			in the file's header but not all viewers pick this up
	
	//	Rotate the image
	m_sRotation = -90;
	Rotate(bRedraw);

	//	Restore the rotation value
	m_sRotation = sRotation;
}

//==============================================================================
//
// 	Function Name:	CTMLead::RotateCw()
//
// 	Description:	This function allows the caller to rotate the image 90
//					degrees clockwise.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::RotateCw(BOOL bRedraw) 
{
	short sRotation = m_sRotation;

	//	NOTE:	LeadTools has a FastRotate() method but we don't use that 
	//			because it doesn't actually rotate the image. It sets a field
	//			in the file's header but not all viewers pick this up
	
	//	Rotate the image
	m_sRotation = 90;
	Rotate(bRedraw);

	//	Restore the rotation value
	m_sRotation = sRotation;
}

//==============================================================================
//
// 	Function Name:	CTMLead::RotateRect()
//
// 	Description:	This function is called to rotate the rectangle 90
//					degrees about it's center in the desired direction
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::RotateRect(RECT* pRect, BOOL bClockwise) 
{
	float	fWidth;
	float	fHeight;
	float	fCx;
	float	fCy;

	//	Get the current width and height
	fWidth  = (float)(pRect->right - pRect->left);
	fHeight = (float)(pRect->bottom - pRect->top);

	//	Get the center point
	fCx = (float)pRect->left + (fWidth / 2.0f);
	fCy = (float)pRect->top + (fHeight / 2.0f);

	pRect->top  = ROUND(fCy - (fWidth / 2.0f));
	pRect->left = ROUND(fCx - (fHeight / 2.0f));

	//	The width becomes the height and the height becomes the width
	pRect->right  = pRect->left + ROUND(fHeight);
	pRect->bottom = pRect->top + ROUND(fWidth);

}

//==============================================================================
//
// 	Function Name:	CTMLead::RotateSrcRect()
//
// 	Description:	This function is called to rotate the source rectangle 90
//					degrees in the desired direction
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::RotateSrcRect(ANNRECT* prcSrc, BOOL bClockwise) 
{
	double dWidth;
	double dHeight;

	//	Get the current width and height
	dWidth  = prcSrc->right - prcSrc->left;
	dHeight = prcSrc->bottom - prcSrc->top;

	if(bClockwise == TRUE)
	{
		prcSrc->top = prcSrc->left;
		prcSrc->left = (GetSrcWidth() - prcSrc->bottom);
	}
	else
	{
		prcSrc->left = prcSrc->top;
		prcSrc->top = (GetSrcHeight() - prcSrc->right);
	}

	//	The width becomes the height and the height becomes the width
	prcSrc->right  = prcSrc->left + dHeight;
	prcSrc->bottom = prcSrc->top + dWidth;

}

//==============================================================================
//
// 	Function Name:	CTMLead::Save()
//
// 	Description:	This function allows the caller to save the current image
//					using the specified filename.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMLead::Save(LPCTSTR lpszFilename) 
{
	CString strFilename;
	short	sQFactor = 0;
	short	sModify = 0;
	short	sReturn = 0;
	CString	strMsg;
	char	szExMsg[512];

	//	Did the user provide a filename?
	if((lpszFilename != 0) && (lstrlen(lpszFilename) > 0))
		strFilename = lpszFilename;
	else
		strFilename = m_strFilename;

	//	Should we specify a QFactor value?
	sQFactor = GetSaveQFactor();

	//	Is this a multipage file?
	if(m_sPages > 1)
	{
		SetSavePage(m_sPage);
		sModify = SAVE_REPLACE;
	}
	else
	{
		sModify = SAVE_OVERWRITE;
	}

	try
	{
		sReturn = CLead::Save(strFilename, GetInfoFormat(), GetInfoBits(), 
							  sQFactor, sModify);

		//	Were we successful?
		if(sReturn == 0)
		{
			//	Did we just save the active file?
			if(lstrcmpi(strFilename, m_strFilename) == 0)
			{
				//	Image is no longer rotated
				m_sAngle = 0;
			}

		}

		return sReturn;

	}
	catch(COleException *e)
	{
		strMsg.Format("LeadTools exception raised while attempting to save %s\n\n", strFilename);

		if(e->GetErrorMessage(szExMsg, sizeof(szExMsg)))
			strMsg += szExMsg;

		if(m_pErrors != 0)
			m_pErrors->Handle(0, strMsg);

		e->Delete();
		return TMV_SAVEEXCEPTION;
	}
	catch(COleDispatchException *e)
	{
		strMsg.Format("LeadTools exception raised while attempting to save %s\n\n", strFilename);

		if(e->GetErrorMessage(szExMsg, sizeof(szExMsg)))
			strMsg += szExMsg;

		if(m_pErrors != 0)
			m_pErrors->Handle(0, strMsg);

		e->Delete();
		return TMV_SAVEEXCEPTION;
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::SavePages()
//
// 	Description:	This function is called to save the pages of the specified
//					file to the requested folder
//
// 	Returns:		The total number of saved pages, 0 on error
//
//	Notes:			None
//
//==============================================================================
short CTMLead::SavePages(LPCTSTR lpszFilename, LPCSTR lpszFolder, LPCSTR lpszPrefix) 
{
	CString			strFilename = "";
	CString			strFolder = "";
	CString			strTarget = "";
	short			sQFactor = 0;
	short			sSaved = 0;
	CString			strMsg;
	CPathSplitter	splitter;
	char			szExMsg[512];

	//	Did the user provide a filename?
	if((lpszFilename != 0) && (lstrlen(lpszFilename) > 0))
		strFilename = lpszFilename;
	else
		strFilename = m_strFilename;
	if(strFilename.GetLength() == 0) return FALSE;

	//	Parse the file path
	splitter.Split(strFilename);

	//	Did the user provide a folder?
	if((lpszFolder != 0) && (lstrlen(lpszFolder) > 0))
	{
		strFolder = lpszFolder;
	}
	else
	{
		strFolder = splitter.GetFolder(TRUE);
	}
	if((strFolder.GetLength() > 0) && (strFolder.Right(1) != "\\"))
		strFolder += "\\";

	//	Make sure the target folder exists
	CreateDirectory(strFolder, 0);
	
	//	Try to load the file
	if(SetFilename(strFilename) != TMV_NOERROR) return FALSE;

	//	Get the Q factor to use for the operation
	sQFactor = GetSaveQFactor();

	try
	{
		for(int i = 1; i <= m_sPages; i++)
		{
			//	Construct the target filename
			if((lpszPrefix != NULL) && (lstrlen(lpszPrefix) > 0))
				strTarget.Format("%s%s%.04d%s", strFolder, lpszPrefix, i, splitter.GetExtension());
			else
				strTarget.Format("%s%.04d%s", strFolder, i, splitter.GetExtension());
			
			if(CLead::Save(strTarget, GetInfoFormat(), GetInfoBits(), sQFactor, SAVE_OVERWRITE) != 0)
			{
				if(m_pErrors != 0)
				{
					strMsg.Format("Error while attempting to save page %d of %d in %s to %s\n\nThe operation has been cancelled.",
							   i, m_sPages, strFilename, strTarget);
					m_pErrors->Handle(0, strMsg);
				}
				return 0;
			}
			else
			{
				sSaved++;

				//	Notify the host control
				if(m_pControl)
					m_pControl->OnSavedPage(strFilename, strTarget, i, m_sPages);

				//	Go to the next page
				if(i < m_sPages)
					if(NextPage() != TMV_NOERROR)
						return 0;
			}

		}// for(int i = 1; i <= m_sPages; i++)

	}
	catch(COleException *e)
	{
		strMsg.Format("LeadTools exception raised while attempting to save pages in %s\n\n", strFilename);

		if(e->GetErrorMessage(szExMsg, sizeof(szExMsg)))
			strMsg += szExMsg;

		if(m_pErrors != 0)
			m_pErrors->Handle(0, strMsg);

		e->Delete();
		return 0;
	}
	catch(COleDispatchException *e)
	{
		strMsg.Format("LeadTools exception raised while attempting to save pages in %s\n\n", strFilename);

		if(e->GetErrorMessage(szExMsg, sizeof(szExMsg)))
			strMsg += szExMsg;

		if(m_pErrors != 0)
			m_pErrors->Handle(0, strMsg);

		e->Delete();
		return 0;
	}

	return sSaved;

}

//==============================================================================
//
// 	Function Name:	CTMLead::SaveZap()
//
// 	Description:	This function will write the information required to restore
//					the object to the zap file provided by the caller.
//
// 	Returns:		TRUE if successful.
//
//	Notes:			This function assumes the file is properly positioned before
//					it is called.
//
//==============================================================================
BOOL CTMLead::SaveZap(CFile* pFile) 
{
	CCallout*	pCallout;
	SZapPane	Pane;
	HGLOBAL		hAnnMem  = 0;
	long		lAnnSize = 0;
	LPBYTE		lpAnnMem = 0;
	BOOL		bReturn = TRUE;
	char		szErrorMsg[256];

	//	Is the parameter valid?
	ASSERT(pFile);
	ASSERT(pFile->m_hFile != CFile::hFileNull);
	if(!pFile || pFile->m_hFile == CFile::hFileNull)
		return FALSE;

	//	Do we have an image?
	if(!IsLoaded())
		return FALSE;

	//	Do we have any annotations to save?
	if(GetAnnContainer() != 0)
		if(AnnSaveMemory((long FAR *)&hAnnMem, ANNFMT_XML, FALSE, &lAnnSize, SAVE_OVERWRITE, 1))
			return FALSE;

	//	Initialize the pane descriptor
	ZeroMemory(&Pane, sizeof(Pane));
	Pane.fDstTop = GetDstTop();
	Pane.fDstLeft = GetDstLeft();
	Pane.fDstHeight = GetDstHeight();
	Pane.fDstWidth = GetDstWidth();
	Pane.iViewHeight = m_iHeight;
	Pane.iViewWidth = m_iWidth;
	Pane.sAngle = m_sAngle;
	Pane.sCallouts = (m_pCallouts != 0) ? m_pCallouts->GetCount() : 0;
	Pane.lAnnBytes = lAnnSize;
	lstrcpyn(Pane.szFilespec, m_strFilename, sizeof(Pane.szFilespec));

	//	Store the current maximum rectangle in the unused
	//	members of the pane header. This maintains backward compatability and
	//	allows us to refine some of the rescaling at a later date
	Pane.dwTLMax	= MAKELONG(m_rcMax.left, m_rcMax.top);
	Pane.dwBRMax	= MAKELONG(m_rcMax.right, m_rcMax.bottom);
	Pane.wPaneLeft	= m_iLeft;
	Pane.wPaneTop	= m_iTop;

	//	Now write the information to the file
	try
	{
		//	Write the descriptor
		pFile->Write(&Pane, sizeof(Pane));

		//	Do we have any annotations to write?
		if(hAnnMem && lAnnSize > 0)
		{
			//	Lock the memory
			lpAnnMem = (LPBYTE)GlobalLock(hAnnMem);
			ASSERT(lpAnnMem);

			//	Write the annotations
			pFile->Write(lpAnnMem, lAnnSize);
		}	

		//	Write each callout to the zap file
		if(m_pCallouts && !m_pCallouts->IsEmpty())
		{
			pCallout = m_pCallouts->First();
			while(pCallout)
			{
				if(!pCallout->SaveZap(pFile))
				{
					bReturn = FALSE;
					break;
				}
				else
				{
					pCallout = m_pCallouts->Next();
				}
			}
		}

	}

	//	Catch all file exceptions
	catch(CFileException* pFileException)
	{
		//	Get the error message and pass it on to the handler
		pFileException->GetErrorMessage(szErrorMsg, sizeof(szErrorMsg));
		if(m_pErrors != 0) m_pErrors->Handle(0, szErrorMsg);
		pFileException->Delete();
		bReturn = FALSE;
	}
	//	Catch all other exceptions
	catch(CException* pException)
	{
		//	Get the error message and pass it on to the handler
		pException->GetErrorMessage(szErrorMsg, sizeof(szErrorMsg));
		if(m_pErrors != 0) m_pErrors->Handle(0, szErrorMsg);
		pException->Delete();
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
// 	Function Name:	CTMLead::SelectAnn()
//
// 	Description:	This function will select the annotation specified by the
//					caller
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SelectAnn(DWORD dwTag)
{
	HANNOBJECT hAnn;

	//	Get the handle to the specified annotation
	if((hAnn = GetHandleFromTag(dwTag)) == 0)
		return;

	//	Select the requested annotation
	L_AnnSetSelected(hAnn, TRUE, 0);
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetAction()
//
// 	Description:	This function is called by the control object when the
//					action property changes.
//
// 	Returns:		None
//
//	Notes:			This property the action taken when the user clicks in
//					the window.
//
//==============================================================================
void CTMLead::SetAction(short sAction) 
{
	CCallout* pCallout;

	//	What is the new action?
	switch(sAction)
	{
		case ZOOM:		SetZoom();
						break;
		case DRAW:		SetDraw();
						break;
		case REDACT:	SetRedact();
						break;
		case HIGHLIGHT:	SetHighlight();
						break;
		case SELECT:	SetSelect();
						break;
		case CALLOUT:	SetCallout();
						break;
		case PAN:		SetPan();
						break;
		default:		SetNone();
						break;
	}

	//	Clear the current selections if not in SELECT mode
	if(m_sAction != SELECT)
		ClearSelections();

	//	Propagate the change to all callouts
	if(m_pCallouts)
	{
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetAction(sAction);
			pCallout = m_pCallouts->Next();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetAnnColor()
//
// 	Description:	This function is called by the control object when the
//					annotation color changes.
//
// 	Returns:		None
//
//	Notes:			This property determines the color of the annotation tool.
//
//==============================================================================
void CTMLead::SetAnnColor(short sAnnColor) 
{
	CCallout* pCallout;

	m_sAnnColor = sAnnColor;
	m_crDraw = GetColorRef(sAnnColor);	

	//	Propagate the change to all callouts
	if(m_pCallouts)
	{
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetAnnColor(sAnnColor);
			pCallout = m_pCallouts->Next();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetAnnColorDepth()
//
// 	Description:	This function is called by the control object when the
//					annotation color depth changes.
//
// 	Returns:		None
//
//==============================================================================
void CTMLead::SetAnnColorDepth(short sAnnColorDepth) 
{
	CCallout* pCallout;

	m_sAnnColorDepth = sAnnColorDepth;

	//	Propagate the change to all callouts
	if(m_pCallouts)
	{
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetAnnColorDepth(sAnnColorDepth);
			pCallout = m_pCallouts->Next();
		}
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::SetAnnFontBold()
//
// 	Description:	This function is called by the control object when the
//					AnnFontBold property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetAnnFontBold(BOOL bBold) 
{
	CCallout* pCallout;

	m_bAnnFontBold = bBold;

	//	Propagate the change to all callouts
	if(m_pCallouts)
	{
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetAnnFontBold(bBold);
			pCallout = m_pCallouts->Next();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetAnnFontName()
//
// 	Description:	This function is called by the control object when the
//					AnnFontName property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetAnnFontName(LPCTSTR lpName) 
{
	CCallout* pCallout;

	m_strAnnFontName = lpName;

	//	Propagate the change to all callouts
	if(m_pCallouts)
	{
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetAnnFontName(lpName);
			pCallout = m_pCallouts->Next();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetAnnFontSize()
//
// 	Description:	This function is called by the control object when the
//					AnnFontSize property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetAnnFontSize(short sSize) 
{
	CCallout* pCallout;

	m_sAnnFontSize = sSize;

	//	Propagate the change to all callouts
	if(m_pCallouts)
	{
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetAnnFontSize(sSize);
			pCallout = m_pCallouts->Next();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetAnnFontStrikeThrough()
//
// 	Description:	This function is called by the control object when the
//					AnnFontStrikeThrough property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetAnnFontStrikeThrough(BOOL bStrikeThrough) 
{
	CCallout* pCallout;

	m_bAnnFontStrikeThrough = bStrikeThrough;

	//	Propagate the change to all callouts
	if(m_pCallouts)
	{
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetAnnFontStrikeThrough(bStrikeThrough);
			pCallout = m_pCallouts->Next();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetAnnFontUnderline()
//
// 	Description:	This function is called by the control object when the
//					AnnFontUnderline property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetAnnFontUnderline(BOOL bUnderline) 
{
	CCallout* pCallout;

	m_bAnnFontUnderline = bUnderline;

	//	Propagate the change to all callouts
	if(m_pCallouts)
	{
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetAnnFontUnderline(bUnderline);
			pCallout = m_pCallouts->Next();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetAnnotateCallouts()
//
// 	Description:	This function is called by the control object when the
//					AnnotateCallouts property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetAnnotateCallouts(BOOL bAnnotateCallouts) 
{
	CCallout* pCallout;

	m_bAnnotateCallouts = bAnnotateCallouts;

	//	Propagate the change to all callouts
	if(m_pCallouts)
	{
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetAnnotateCallouts(bAnnotateCallouts);
			pCallout = m_pCallouts->Next();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetAnnotations()
//
// 	Description:	This function is called to set the annotations for the
//					image from memory.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetAnnotations(HGLOBAL hAnnMem, long lAnnBytes) 
{
    CAnnotation* pAnn;

	//	Erase any existing annotations
	if(GetAnnContainer())
		L_AnnDestroy((void *)GetAnnContainer(), 
					  ANNFLAG_RECURSE | ANNFLAG_NOTTHIS);

	//	Add the annotations if we have any
	if(hAnnMem && lAnnBytes > 0)
	{
		//	Do not attempt to set the annotation properties
		m_bSetAnnProps = FALSE;

		//	Load the new annotations
		AnnLoadMemory((OLE_HANDLE)hAnnMem, lAnnBytes, 1);

		//	Rebuild the local annotations list?
		if(GetAnnContainer())
		{
			m_Annotations.Flush(TRUE);

			//	Enumerate the container 
			L_AnnEnumerate((void*)GetAnnContainer(), 
						   (ANNENUMCALLBACK)RebuildAnnList,
						   &m_Annotations, 
						   ANNFLAG_RECURSE | ANNFLAG_NOTCONTAINER, NULL);

			//	Is there a callout shade in the container
			if((pAnn = m_Annotations.FindCalloutShade()) != 0)
			{
				//	Restore the shade handle
				if((m_hCalloutShade = GetHandleFromTag(pAnn->GetAnnTag())) != 0)
					L_AnnSendToBack(m_hCalloutShade);
			}

		}

		m_bSetAnnProps = TRUE;
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetAnnThickness()
//
// 	Description:	This function is called by the control object when the
//					annotation thickness changes.
//
// 	Returns:		None
//
//	Notes:			This property determines the thickness of the drawing tool
//
//==============================================================================
void CTMLead::SetAnnThickness(short sAnnThickness) 
{
	CCallout* pCallout;

	m_sAnnThickness = sAnnThickness;

	//	Propagate the change to all callouts
	if(m_pCallouts)
	{
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetAnnThickness(sAnnThickness);
			pCallout = m_pCallouts->Next();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetAnnTool()
//
// 	Description:	This function is called by the control object when the
//					annotation tool changes.
//
// 	Returns:		None
//
//	Notes:			This property determines the active drawing tool.
//
//==============================================================================
void CTMLead::SetAnnTool(short sAnnTool) 
{
	CCallout* pCallout;

	m_sAnnTool = sAnnTool;

	//	Reset the tool if we are already drawing
	if(m_sAction == DRAW)
		SetDraw();

	//	Propagate the change to all callouts
	if(m_pCallouts)
	{
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetAnnTool(sAnnTool);
			pCallout = m_pCallouts->Next();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetAnnUserModeEx()
//
// 	Description:	This function is called to set the annotation user mode
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetAnnUserModeEx(short sMode) 
{
	//	NOTE:	This method is provided because the LeadTools control does not
	//			bother to check to see if the control is already in the desired 
	//			mode. Unfortunately when the mode switches, it causes flicker 
	//			because LeadTools repaints the annotations. We add this test
	//			to minimize the number of times that occurs
	if(GetAnnUserMode() != sMode)
		SetAnnUserMode(sMode);    
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetBackColor()
//
// 	Description:	This function is called by the control object when the
//					background color property changes. It sets the background
//					color of the Lead Tool control to match.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetBackColor(OLE_COLOR ocColor, COLORREF crColor) 
{
	//	Has the color changed?
	if(m_crBackground == crColor)
		return;
	else
		m_crBackground = crColor;

	CLead::SetBackColor(ocColor);
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetBitonal()
//
// 	Description:	This function is called when the BitonalScaling property is
//					changed
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetBitonal(short sBitonal) 
{
    m_sBitonal = sBitonal;
	switch(m_sBitonal)
	{
		case TMV_BITONALNORMAL:

			CLead::SetBitonalScaling(BITONALSCALING_NORMAL);
			break;

		case TMV_BITONALBLACK:

			CLead::SetBitonalScaling(BITONALSCALING_FAVORBLACK);
			break;

		case TMV_BITONALGRAY:

			CLead::SetBitonalScaling(BITONALSCALING_SCALETOGRAY);
			break;
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetCallFrameColor()
//
// 	Description:	This function is called to change the color of the frame for
//					callouts belonging to this pane.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetCallFrameColor(COLORREF crColor) 
{
	CCallout* pCallout;

	m_crCallFrame = crColor;
	m_sCallFrameColor = GetColorId(crColor);	

	//	Propagate the change to all callouts
	if(m_pCallouts)
	{
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetFrameColor(crColor);
			pCallout = m_pCallouts->Next();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetCallFrameColor()
//
// 	Description:	This function is called by the control object when the
//					callout frame color changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetCallFrameColor(short sColor) 
{
	//	Translate to a valid color reference
	SetCallFrameColor(GetColorRef(sColor));
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetCallFrameThickness()
//
// 	Description:	This function is called by the control object when the
//					callout frame thickness changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetCallFrameThickness(short sThickness) 
{
	CCallout* pCallout;

	m_sCallFrameThick = sThickness;

	//	Propagate the change to all callouts
	if(m_pCallouts)
	{
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetFrameThickness(sThickness);
			pCallout = m_pCallouts->Next();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetCallHandleColor()
//
// 	Description:	This function is called to change the color of the resizing
//					handles for callouts belonging to this pane.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetCallHandleColor(COLORREF crColor) 
{
	CCallout* pCallout;

	m_crCallHandle = crColor;	
	m_sCallHandleColor = GetColorId(crColor);	

	//	Propagate the change to all callouts
	if(m_pCallouts)
	{
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetHandleColor(crColor);
			pCallout = m_pCallouts->Next();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetCallHandleColor()
//
// 	Description:	This function is called by the control object when the
//					callout handle color changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetCallHandleColor(short sColor) 
{
	//	Translate to a valid color reference
	SetCallHandleColor(GetColorRef(sColor));
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetCallout()
//
// 	Description:	This function is called when the action property is changed
//					to CALLOUT.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetCallout() 
{
    m_sAction = CALLOUT;
	CLead::SetAnnTool(ANNTOOL_SELECT);
	SetAnnUserModeEx(ANNUSERMODE_RUN);    
	SetMousePointer(MP_CROSSHAIR);
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetCalloutColor()
//
// 	Description:	This function is called by the control object when the
//					callout color changes.
//
// 	Returns:		None
//
//	Notes:			This property establishes the color used for callouts.
//
//==============================================================================
void CTMLead::SetCalloutColor(short sCalloutColor) 
{
	m_sCalloutColor = sCalloutColor;
	m_crCallout = GetColorRef(sCalloutColor);	
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetCalloutShadeColor()
//
// 	Description:	This function is called to change the color of the callout 
//					shade
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetCalloutShadeColor(COLORREF crColor) 
{
	m_crCalloutShadeBackground = crColor;
    m_crCalloutShadeForeground = (m_crCalloutShadeBackground ^ 0x00FFFFFF);
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetColor()
//
// 	Description:	This function is called to set the color associated
//					with the active annotation tool.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetColor(short sColor)
{
    //	What annotation tool is active
	switch(m_sAction)
	{
		case REDACT:	SetRedactColor(sColor);
						return;
		case HIGHLIGHT:	SetHighlightColor(sColor);
						return;
		case CALLOUT:	SetCalloutColor(sColor);
						return;
		case ZOOM:		
		case DRAW:		
		case PAN:	
		case SELECT:	
		default:		SetAnnColor(sColor);
						return;
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::SetControl()
//
// 	Description:	This function is called set the pointer to the TMView 
//					control that uses this object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetControl(CTMViewCtrl* pControl, CErrorHandler* pErrors) 
{
	m_pControl = pControl;
	m_pErrors = pErrors;
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetDeskewBackColor()
//
// 	Description:	This function is called to set the background color for
//					deskewing operations.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetDeskewBackColor(COLORREF crColor) 
{
    m_crDeskew = crColor;
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetDraw()
//
// 	Description:	This function is called when the action property is changed
//					to DRAW.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetDraw() 
{
    m_sAction = DRAW;
	SetAnnUserModeEx(ANNUSERMODE_DESIGN);
	SetMousePointer(MP_DEFAULT);

	switch(m_sAnnTool)
	{
		case RECTANGLE:				CLead::SetAnnTool(ANNTOOL_RECT);
									m_iFillMode = ANNMODE_TRANSPARENT;
									break;

		case FILLED_RECTANGLE:		CLead::SetAnnTool(ANNTOOL_RECT);
									m_iFillMode = ANNMODE_OPAQUE;
									break;

		case ELLIPSE:				CLead::SetAnnTool(ANNTOOL_ELLIPSE);
									m_iFillMode = ANNMODE_TRANSPARENT;
									break;

		case FILLED_ELLIPSE:		CLead::SetAnnTool(ANNTOOL_ELLIPSE);
									m_iFillMode = ANNMODE_OPAQUE;
									break;

		case LINE:					CLead::SetAnnTool(ANNTOOL_LINE);
									m_iFillMode = ANNMODE_OPAQUE;
									break;

		case ARROW:					CLead::SetAnnTool(ANNTOOL_POINTER);
									m_iFillMode = ANNMODE_OPAQUE;
									break;

		case POLYLINE:				CLead::SetAnnTool(ANNTOOL_POLYLINE);
									m_iFillMode = ANNMODE_OPAQUE;
									break;

		case POLYGON:				CLead::SetAnnTool(ANNTOOL_POLYGON);
									m_iFillMode = ANNMODE_TRANSLUCENT;
									break;

		case ANNTEXT:				CLead::SetAnnTool(ANNTOOL_TEXT);
									m_iFillMode = ANNMODE_TRANSPARENT;
									break;
		case FREEHAND:
		default:					CLead::SetAnnTool(ANNTOOL_FREEHAND);
									m_iFillMode = ANNMODE_OPAQUE;
									break;

	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetDstRects()
//
// 	Description:	This function is called to set the source and source 
//					clip rectangles
//
// 	Returns:		None
//
//	Notes:			RECT.right  = width (not right hand coordinate)
//					RECT.bottom = height (not bottom coordinate)
//
//					If no rectangle is provided for the clip, the same will
//					be used for both
//
//==============================================================================
void CTMLead::SetDstRects(ANNRECT* pDst, ANNRECT* pClip) 
{
	ASSERT(pDst);

	//	Do we have an image?
	if(IsLoaded())
	{
		//	Set the coordinates of the destination rectangle
		SetDstRect((float)pDst->left, (float)pDst->top,
				   (float)pDst->right, (float)pDst->bottom);

		//	Set the coordinates of the source clip rectangle
		if(pClip)
			SetDstClipRect((float)pClip->left, (float)pClip->top,
						   (float)pClip->right, (float)pClip->bottom);
		else
			SetDstClipRect((float)pDst->left, (float)pDst->top,
						   (float)pDst->right, (float)pDst->bottom);
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::SetDstRects()
//
// 	Description:	This function is called to set the source and source 
//					clip rectangles
//
// 	Returns:		None
//
//	Notes:			RECT.right  = width (not right hand coordinate)
//					RECT.bottom = height (not bottom coordinate)
//
//					If no rectangle is provided for the clip, the same will
//					be used for both
//
//==============================================================================
void CTMLead::SetDstRects(RECT* pDst, RECT* pClip) 
{
	ASSERT(pDst);

	//	Do we have an image?
	if(IsLoaded())
	{
		//	Set the coordinates of the destination rectangle
		SetDstRect((float)pDst->left, (float)pDst->top,
				   (float)pDst->right, (float)pDst->bottom);

		//	Set the coordinates of the destination clip rectangle
		if(pClip)
			SetDstClipRect((float)pClip->left, (float)pClip->top,
						   (float)pClip->right, (float)pClip->bottom);
		else
			SetDstClipRect((float)pDst->left, (float)pDst->top,
						   (float)pDst->right, (float)pDst->bottom);
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::SetFilename()
//
// 	Description:	This function is called by the control object when the
//					filename property changes. It first checks to see if the 
//					file exists and then loads the file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
short CTMLead::SetFilename(LPCTSTR lpszNewValue, BOOL bDraw) 
{
	short sLeadError;

	//	Delete any existing callouts
	DestroyCallouts();

	//	Unload the current image if the filename is empty
	if(lpszNewValue == 0 || lstrlen(lpszNewValue) == 0)
	{
		UnloadImage();
		return TMV_NOERROR;
	}

	//	First check to see if this file exists
	if(!FindFile(lpszNewValue))
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_FILENOTFOUND, lpszNewValue);
		return TMV_FILENOTFOUND;
	}

    //	Display the wait cursor while we load the image
	if(IsWindowVisible())
		AfxGetApp()->DoWaitCursor(1);

	//  Load the image
	if((sLeadError = LoadImage(lpszNewValue, -1)) != TMV_NOERROR)
	{
		if(IsWindowVisible())
			AfxGetApp()->DoWaitCursor(-1);
		return HandleFileError(lpszNewValue, sLeadError);
	}

	//	Save the new filename
	m_strFilename = lpszNewValue;

	//	Do we need to rotate the image?
	if(m_sRotation != 0)
		Rotate(FALSE);

	//	Now draw the new image
	if(bDraw)
		Draw();

	//	Turn off the wait cursor
	if(IsWindowVisible())
		AfxGetApp()->DoWaitCursor(-1);

	return TMV_NOERROR;
	
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetFitToImage()
//
// 	Description:	This function is called by the control object when the
//					fit to image property changes.
//
// 	Returns:		None
//
//	Notes:			This property determines whether or not the window should
//					be resized to fit the image dimensions.
//
//==============================================================================
void CTMLead::SetFitToImage(BOOL bFitToImage) 
{
	m_bFitToImage = bFitToImage;	
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetHideScrollBars()
//
// 	Description:	This function is called by the control object when the
//					pan image property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetHideScrollBars(BOOL bHideScrollBars) 
{
	CCallout* pCallout;

	m_bHideScrollBars = bHideScrollBars;	
	
	//	Propagate the change to all callouts
	if(m_pCallouts)
	{
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetHideScrollBars(bHideScrollBars);
			pCallout = m_pCallouts->Next();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetHighlight()
//
// 	Description:	This function is called when the action property is changed
//					to HIGHLIGHT.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetHighlight() 
{
    m_sAction = HIGHLIGHT;
	CLead::SetAnnTool(ANNTOOL_HILITE);
	SetAnnUserModeEx(ANNUSERMODE_DESIGN);
	SetMousePointer(MP_DEFAULT);
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetHighlightColor()
//
// 	Description:	This function is called by the control object when the
//					highlight color changes.
//
// 	Returns:		None
//
//	Notes:			This property establishes the color used for highlighting.
//
//==============================================================================
void CTMLead::SetHighlightColor(short sHighlightColor) 
{
	CCallout* pCallout;

	m_sHighlightColor = sHighlightColor;
	m_crHighlight = GetColorRef(sHighlightColor);	

	//	Propagate the change to all callouts
	if(m_pCallouts)
	{
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetHighlightColor(sHighlightColor);
			pCallout = m_pCallouts->Next();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetKeepAspect()
//
// 	Description:	This function is called by the control object when the
//					KeepAspect property changes.
//
// 	Returns:		None
//
//	Notes:			This property determines whether or not the aspect ratio of
//					the source image should be changed when the image is scaled
//
//==============================================================================
void CTMLead::SetKeepAspect(BOOL bKeepAspect) 
{
	//	Has the value changed?
	if(m_bKeepAspect != bKeepAspect)
	{
		m_bKeepAspect = bKeepAspect;
		
		//	Redraw the image if it's scaled
		if(m_bScaleImage)
			Redraw();
	}	
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetLoopAnimate()
//
// 	Description:	This function is called by the control object when the
//					LoopAnimate property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetLoopAnimate(BOOL bLoop)
{
	if(bLoop)
		SetAutoAnimationLoop(AUTOANIMATIONLOOP_INFINITE);
	else
		SetAutoAnimationLoop(AUTOANIMATIONLOOP_DEFAULT);
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetMaxRect()
//
// 	Description:	This function is called by the control object when its size
//					changes. It sets the rectangle that defines the position and
//					size of the rectangle this object can use.
//
// 	Returns:		None
//
//	Notes:			The control window sets the extents so that they always 
//					reflect the maximum available size. 
//
//					pRect->right  = Width (not right hand coordinate)
//					pRect->bottom = Height (not bottom coordinate)
//
//==============================================================================
void CTMLead::SetMaxRect(RECT* pRect, BOOL bSplitScreen, BOOL bRedraw) 
{
	//RECT		rcContainer;
	//CCallout*	pCallout;

	ASSERT(pRect);

	//	Update the split screen flag
	m_bSplitScreen = bSplitScreen;

	//	Save the new rectangle
	m_rcMax.left   = pRect->left;
	m_rcMax.top    = pRect->top;
	m_rcMax.right  = pRect->right;
	m_rcMax.bottom = pRect->bottom;

	//	This prevents the possibility of divide by zero errors
	if(m_rcMax.right == 0)
		m_rcMax.right = 1;
	if(m_rcMax.bottom == 0)
		m_rcMax.bottom = 1;

	//	Do we need to update the zap drawing parameters?
	if((m_fZapWidth > 0) && (m_fZapHeight > 0))
	{
		SetZapParameters(&m_rcZapControl);
	}	

/*	COMMENTED OUT FOR NOW UNTIL WE HAVE TIME TO PROPERLY ADDRESS 
	THE RAMIFICATIONS OF DOING THIS EVERY TIME THE MAXIMUM SIZE CHANGES

	//	Propagate the change to all callouts
	if((m_pCallouts != 0) && (m_pCallouts->GetCount() > 0))
	{
		//	NOTE:	m_rcMax.right = width (not absolute coordinate)
		//			m_rcMax.bottom = height (not absolute coordinate)
		rcContainer.left   = m_rcMax.left;
		rcContainer.top    = m_rcMax.top;
		rcContainer.right  = m_rcMax.left + m_rcMax.right;
		rcContainer.bottom = m_rcMax.top + m_rcMax.bottom;
		GetParent()->ClientToScreen(&rcContainer);

		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetContainer(&rcContainer);
			pCallout->Rescale();
			pCallout = m_pCallouts->Next();
		}
	}
*/

	//	Redraw the image if we have a file loaded
	if(IsLoaded() && bRedraw)
	{
		Redraw();
	}	
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetMaxZoom()
//
// 	Description:	This function is called by the control object when the
//					maximum zoom factor changes.
//
// 	Returns:		None
//
//	Notes:			This property determines the maximum zoom factor allowed.
//
//==============================================================================
void CTMLead::SetMaxZoom(short sMaxZoom) 
{
	m_fMaxZoom = (float)sMaxZoom;	
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetNone()
//
// 	Description:	This function is called when the action property is changed
//					to NONE.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetNone() 
{
    m_sAction = NONE;
	CLead::SetAnnTool(ANNTOOL_SELECT);
	SetAnnUserModeEx(ANNUSERMODE_RUN);    
	SetMousePointer(MP_DEFAULT);
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetPan()
//
// 	Description:	This function is called when the action property is changed
//					to PAN.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetPan() 
{
    m_sAction = PAN;
	CLead::SetAnnTool(ANNTOOL_SELECT);
	SetAnnUserModeEx(ANNUSERMODE_RUN);    
	SetCursor(m_aCursors[PAN_CURSOR]);
	SetMousePointer(99);
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetPanCallouts()
//
// 	Description:	This function is called to set the flag used to enable and
//					disable panning of callouts.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetPanCallouts(BOOL bPan) 
{
	CCallout* pCallout;

	m_bPanCallouts = bPan;

	//	Propagate the change to all callouts
	if(m_pCallouts)
	{
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetPanCallouts(bPan);
			pCallout = m_pCallouts->Next();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetPanPercent()
//
// 	Description:	This function is called by the control object when the
//					pan percent property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetPanPercent(short sPanPercent) 
{
	m_fPanPercent = ((float)sPanPercent / 100.0f);	
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetPrintBorder()
//
// 	Description:	This function is called to set the print border flag
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetPrintBorder(BOOL bPrint) 
{
	m_bPrintBorder = bPrint;
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetPrintBorderColor()
//
// 	Description:	This function is called to set the print border color
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetPrintBorderColor(COLORREF crColor) 
{
    m_crPrintBorder = crColor;
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetPrintBorderThickness()
//
// 	Description:	This function is called to set the print border thickness
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetPrintBorderThickness(float fThickness) 
{
    m_fPrintBorderThickness = fThickness;
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetPrintCalloutBorders()
//
// 	Description:	This function is called to set the print callout borders
//					flag.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetPrintCalloutBorders(BOOL bPrint) 
{
	m_bPrintCalloutBorders = bPrint;
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetQFactor()
//
// 	Description:	This function is called to set the QFactor used for saving
//					JPEG images
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetQFactor(short sQFactor) 
{
	m_sQFactor = sQFactor;
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetRedact()
//
// 	Description:	This function is called when the action property is changed
//					to REDACT.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetRedact() 
{
    m_sAction = REDACT;
	CLead::SetAnnTool(ANNTOOL_REDACT);
	SetAnnUserModeEx(ANNUSERMODE_DESIGN);
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetRedactColor()
//
// 	Description:	This function is called by the control object when the
//					redact color property changes.
//
// 	Returns:		None
//
//	Notes:			This property determines the color used for redact drawing.
//
//==============================================================================
void CTMLead::SetRedactColor(short sRedactColor) 
{
	CCallout* pCallout;

	m_sRedactColor = sRedactColor;
	m_crRedact = GetColorRef(m_sRedactColor);	

	//	Propagate the change to all callouts
	if(m_pCallouts)
	{
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetRedactColor(sRedactColor);
			pCallout = m_pCallouts->Next();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetResizeCallouts()
//
// 	Description:	This function is called to set the flag used to enable and
//					disable resizing of callouts.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetResizeCallouts(BOOL bResize) 
{
	CCallout* pCallout;

	m_bResizeCallouts = bResize;

	//	Propagate the change to all callouts
	if(m_pCallouts)
	{
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetResizeable(bResize);
			pCallout = m_pCallouts->Next();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetRightClickPan()
//
// 	Description:	This function is called by the control object when the
//					right click pan property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetRightClickPan(BOOL bRightClickPan) 
{
	CCallout* pCallout;

	m_bRightClickPan = bRightClickPan;	

	//	Propagate the change to all callouts
	if(m_pCallouts)
	{
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetRightClickPan(bRightClickPan);
			pCallout = m_pCallouts->Next();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetRotation()
//
// 	Description:	This function is called by the control object when the
//					rotation property changes.
//
// 	Returns:		None
//
//	Notes:			This property only affects the rotation of a document when
//					it is initially loaded. To change the rotation of a loaded
//					image, the Rotate() method must be used.
//
//==============================================================================
void CTMLead::SetRotation(short sRotation) 
{
	m_sRotation = sRotation;	
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetScaleImage()
//
// 	Description:	This function is called by the control object when the
//					scale image property changes.
//
// 	Returns:		None
//
//	Notes:			This property determines whether or not the image should be
//					be scaled to fit the current window.
//
//==============================================================================
void CTMLead::SetScaleImage(BOOL bScaleImage) 
{
	//	Has the value changed?
	if(m_bScaleImage != bScaleImage)
	{
		m_bScaleImage = bScaleImage;
		
		// Set the image display size to match that of the lead control
		SetDstRect(0.0f, 0.0f, m_fImageWidth, m_fImageHeight); 
		SetDstClipRect(0.0f, 0.0f, m_fImageWidth, m_fImageHeight);

		//	Clear the zoom state
		m_sZoomState = ZOOMED_NONE;
	}	
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetSelect()
//
// 	Description:	This function is called when the action property is changed
//					to SELECT.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetSelect() 
{
    m_sAction = SELECT;
	CLead::SetAnnTool(ANNTOOL_SELECT);
	SetAnnUserModeEx(ANNUSERMODE_DESIGN);
	SetMousePointer(MP_DEFAULT);

	//	Reset the edit annotation pointer
	OnEndEditTextAnn(FALSE);
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetShadeOnCallout()
//
// 	Description:	This function is called to set the ShadeOnCallout property.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetShadeOnCallout(BOOL bShadeOnCallout)
{
	m_bShadeOnCallout = bShadeOnCallout;
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetSrcRects()
//
// 	Description:	This function is called to set the source and source 
//					clip rectangles
//
// 	Returns:		None
//
//	Notes:			RECT.right  = width (not right hand coordinate)
//					RECT.bottom = height (not bottom coordinate)
//
//					If no rectangle is provided for the clip, the same will
//					be used for both
//
//==============================================================================
void CTMLead::SetSrcRects(RECT* pSrc, RECT* pClip) 
{
	ASSERT(pSrc);

	//	Do we have an image?
	if(IsLoaded())
	{
		//	Set the coordinates of the source rectangle
		SetSrcRect((float)pSrc->left, (float)pSrc->top,
				   (float)pSrc->right, (float)pSrc->bottom);

		//	Set the coordinates of the source clip rectangle
		if(pClip)
			SetSrcClipRect((float)pClip->left, (float)pClip->top,
						   (float)pClip->right, (float)pClip->bottom);
		else
			SetSrcClipRect((float)pSrc->left, (float)pSrc->top,
						   (float)pSrc->right, (float)pSrc->bottom);
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::SetSrcVisible()
//
// 	Description:	This function will adjust the destination rectangle to
//					make sure the specified source region is visible
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetSrcVisible(ANNRECT* prcSrc, BOOL bFullSource)
{
	double	dXScale;
	double	dYScale;
	double	dSrcWidth;
	double	dSrcHeight;
	ANNRECT	rcDst;

	ASSERT(IsLoaded() == TRUE);
	if(!IsLoaded()) return;

	//	Is the complete source image supposed to be visible?
	if(bFullSource == TRUE)
	{
		//	Size the destination rectangle to match the client window
		rcDst.left   = 0.0;
		rcDst.top    = 0.0;
		rcDst.right  = (double)(m_iWidth);
		rcDst.bottom = (double)(m_iHeight);
	}
	else
	{
		//	Calculate the size of the desired source region
		dSrcWidth = (double)(prcSrc->right - prcSrc->left);
		dSrcHeight = (double)(prcSrc->bottom - prcSrc->top);
		ASSERT(dSrcWidth > 0);
		ASSERT(dSrcHeight > 0);
		if((dSrcWidth <= 0) || (dSrcHeight <= 0)) return;

		//	Calculate the factors for scaling the destination rectangle
		dXScale = (double)m_iWidth / dSrcWidth;
		dYScale = (double)m_iHeight / dSrcHeight;

		//	Scale the destination rectangle to keep the same source visible
		rcDst.left   = (prcSrc->left * -1.0 * dXScale);
		rcDst.top    = (prcSrc->top * -1.0 * dYScale) ;
		rcDst.right  = (GetSrcWidth() * dXScale);
		rcDst.bottom = (GetSrcHeight() * dYScale);

		rcDst.left = (double)(ROUND(rcDst.left));
		rcDst.top = (double)(ROUND(rcDst.top));
		rcDst.right = (double)(ROUND(rcDst.right));
		rcDst.bottom = (double)(ROUND(rcDst.bottom));

	}// if(bFullSource == TRUE)

	//	Set the new rectangles 
	SetDstRects(&rcDst, NULL);
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetSyncCalloutAnn()
//
// 	Description:	This function is called when the SyncCalloutAnn property
//					is changed
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetSyncCalloutAnn(BOOL bSync) 
{
    m_bSyncCalloutAnn = bSync;
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetToRatio()
//
// 	Description:	This function is called to adjust the specified rectangle so
//					that it's aspect ratio matches the specified ratio
//
// 	Returns:		None
//
//	Notes:			This method assumes ratio = width/height
//
//==============================================================================
void CTMLead::SetToRatio(RECT* pRect, double dRatio) 
{
	double	dHeight;
	double	dWidth;
	double	dCx;
	double	dCy;

	ASSERT(pRect);
	ASSERT(dRatio != 0.0);
	if((pRect == 0) || (dRatio == 0.0)) return;

	//	Get the current size of the rectangle
	dWidth  = (double)(pRect->right - pRect->left);
	dHeight = (double)(pRect->bottom - pRect->top);

	ASSERT(dHeight >= 2.0);
	ASSERT(dWidth >= 2.0);
	if((dWidth < 2.0) || (dHeight < 2.0))
		return;

	//	Does the rectangle need to be resized?
	if((dWidth / dHeight) != dRatio)
	{
		//	Calculate the center point
		dCx = (double)pRect->left + (dWidth / 2.0);
		dCy = (double)pRect->top + (dHeight / 2.0);

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
		pRect->left = ROUND((dCx - (dWidth / 2.0)));
		pRect->top = ROUND((dCy - (dHeight / 2.0)));
		pRect->right = pRect->left + ROUND(dWidth);
		pRect->bottom = pRect->top + ROUND(dHeight);
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::SetToScreenRatio()
//
// 	Description:	This function is called to adjust the specified rectangle so
//					that it's aspect ratio matches that of the screen.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetToScreenRatio(RECT* pRect) 
{
	double	dRatio = GetScreenRatio();

	ASSERT(pRect);

	if((pRect != 0) && (dRatio != 0.0))
		SetToRatio(pRect, dRatio);

}

//==============================================================================
//
// 	Function Name:	CTMLead::SetZapParameters()
//
// 	Description:	This function is called to set the parameters for drawing
//					zap images.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLead::SetZapParameters(RECT* pZapControl) 
{
	ASSERT(pZapControl);

	//	Copy the control rectangle coordinates
	memcpy(&m_rcZapControl, pZapControl, sizeof(m_rcZapControl));

	//	Get the dimensions of the control rectangle
	m_fZapWidth  = (float)(m_rcZapControl.right - m_rcZapControl.left);
	m_fZapHeight = (float)(m_rcZapControl.bottom - m_rcZapControl.top);

	//	Make sure we have valid dimensions
	if((m_fZapWidth <= 0) || (m_fZapHeight <= 0)) 
	{
		ResetZapParameters();
		return FALSE;
	}

	//	Get the aspect ratio of the zap file's control window
	m_fZapRatio = m_fZapHeight / m_fZapWidth;

	//	Make the maximum available rectangle for the zap the same as the
	//	maximum available rectangle for the pane
	memcpy(&m_rcZapMax, &m_rcMax, sizeof(m_rcZapMax));

	//	Resize the maximum available rectangle to have the same aspect ratio
	//	as the parent control stored in the zap file
	ResizeToRatio(&m_rcZapMax, m_fZapRatio);

	//	Compute the scale factor required to convert from the zap file
	//	coordinates to coordinates within our maximized rectangle
	//
	//	NOTE:	m_rcZapMax.right = width (not right hand coordinate)
	m_fZapFactor = ((float)m_rcZapMax.right / m_fZapWidth);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetZoom()
//
// 	Description:	This function is called when the action property is changed
//					to ZOOM.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetZoom() 
{
    m_sAction = ZOOM;
	CLead::SetAnnTool(ANNTOOL_SELECT);
	SetAnnUserModeEx(ANNUSERMODE_RUN);    
	SetMousePointer(MP_CROSSHAIR);
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetZoomCallouts()
//
// 	Description:	This function is called to set the flag used to enable and
//					disable panning of callouts.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetZoomCallouts(BOOL bZoom) 
{
	CCallout* pCallout;

	m_bZoomCallouts = bZoom;

	//	Propagate the change to all callouts
	if(m_pCallouts)
	{
		pCallout = m_pCallouts->First();
		while(pCallout)
		{
			pCallout->SetZoomCallouts(bZoom);
			pCallout = m_pCallouts->Next();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetZoomOnLoad()
//
// 	Description:	This function is called by the control object when the
//					zoom on load property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetZoomOnLoad(short sZoomOnLoad) 
{
	m_sZoomOnLoad = sZoomOnLoad;	
}

//==============================================================================
//
// 	Function Name:	CTMLead::SetZoomToRect()
//
// 	Description:	This function is called by the control object when the
//					ZoomToRect property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SetZoomToRect(BOOL bZoomToRect) 
{
	m_bZoomToRect = bZoomToRect;	
}

//==============================================================================
//
// 	Function Name:	CTMLead::ShowCallouts()
//
// 	Description:	This function is called to show or hide the callouts
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
short CTMLead::ShowCallouts(BOOL bShow) 
{
	CCallout* pCallout;

	if(!m_pCallouts)
		return TMV_NOERROR;

	//	Set the visibility of each callout in the list
	pCallout = m_pCallouts->First();
	while(pCallout)
	{
		pCallout->ShowWindow(bShow ? SW_SHOW : SW_HIDE);
		pCallout = m_pCallouts->Next();
	}

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMLead::ShowRectangle()
//
// 	Description:	This function is provided as a debugging aid. It will 
//					display the coordinates of the specified rectangle.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::ShowRectangle(ANNRECT* pAnnRect, LPSTR lpTitle) 
{
	CString	Msg;

	ASSERT(pAnnRect);

	//	Source rectangle
	Msg.Format("L: %f\nT: %f\nR: %f\nB: %f",
			   pAnnRect->left, pAnnRect->top, pAnnRect->right, pAnnRect->bottom);

	MessageBox(Msg, lpTitle);
}

//==============================================================================
//
// 	Function Name:	CTMLead::ShowRectangle()
//
// 	Description:	This function is provided as a debugging aid. It will 
//					display the coordinates of the specified rectangle.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::ShowRectangle(RECT* pRect, LPSTR lpTitle) 
{
	CString	Msg;

	ASSERT(pRect);

	//	Source rectangle
	Msg.Format("L: %d\nT: %d\nR: %d\nB: %d",
			   pRect->left, pRect->top, pRect->right, pRect->bottom);

	MessageBox(Msg, lpTitle);
}

//==============================================================================
//
// 	Function Name:	CTMLead::ShowRectangles()
//
// 	Description:	This function is provided as a debugging aid. It will 
//					display the dimensions of the source and destination
//					rectangles.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::ShowRectangles(LPSTR lpTitle) 
{
#ifdef _DEBUG // Prevent accidental display in release builds

	CString	Msg;
	CString Tmp;
	RECT	rcSrc;
	BOOL	bFull = FALSE;

	//	Source rectangle
	Msg.Format("SrcLeft %.0f\nSrcTop %.0f\nSrcWidth %.0f\nSrcHeight %.0f\n",
			   GetSrcLeft(), GetSrcTop(), GetSrcWidth(), GetSrcHeight());

	//	Source clip rectangle
	Tmp.Format("SrcClipLeft %.0f\nSrcClipTop %.0f\nSrcClipWidth %.0f\nSrcClipHeight %.0f\n",
			   GetSrcClipLeft(), GetSrcClipTop(), GetSrcClipWidth(), GetSrcClipHeight());
	Msg += Tmp;

	//	Destination rectangle
	Tmp.Format("DstLeft %.0f\nDstTop %.0f\nDstWidth %.0f\nDstHeight %.0f\n",
			   GetDstLeft(), GetDstTop(), GetDstWidth(), GetDstHeight());
	Msg += Tmp;

	//	Destination clip rectangle
	Tmp.Format("DstClipLeft %.0f\nDstClipTop %.0f\nDstClipWidth %.0f\nDstClipHeight %.0f\n",
			   GetDstClipLeft(), GetDstClipTop(), GetDstClipWidth(), GetDstClipHeight());
	Msg += Tmp;

	//	Image attributes
	Tmp.Format("Image Width %.0f\nImageHeight %.0f\nAspect Ratio %.3f\n",
			   m_fImageWidth, m_fImageHeight, m_fAspectRatio);
	Msg += Tmp;

	//	Zoom attributes
	Tmp.Format("Zoom Factor %f\n", m_fZoomFactor);
	Msg += Tmp;

	//	Window attributes
	Tmp.Format("Window Top %d\nWindow Left %d\nWindow Width %d\nWindow Height %d\n",
				m_iTop, m_iLeft, m_iWidth, m_iHeight);
	Msg += Tmp;

	//	Maximum extents
	Tmp.Format("Max Left %d\nMax Top %d\nMax Width %d\nMax Height %d\n",
				m_rcMax.left, m_rcMax.top, m_rcMax.right, m_rcMax.bottom);
	Msg += Tmp;

	//	Zap rectangles
	Tmp.Format("ZapControl Left %d\nZapControl Top %d\nZapControl Width %d\nZapControl Height %d\n",
				m_rcZapControl.left, m_rcZapControl.top, m_rcZapControl.right, m_rcZapControl.bottom);
	Msg += Tmp;

	Tmp.Format("ZapMax Left %d\nZapMax Top %d\nZapMax Width %d\nZapMax Height %d\n",
				m_rcZapMax.left, m_rcZapMax.top, m_rcZapMax.right, m_rcZapMax.bottom);
	Msg += Tmp;

	//	Zap drawing
	Tmp.Format("Zap Factor %f\nZap Ratio %f\nZap Width %f\nZap Height %f\n",
			   m_fZapFactor, m_fZapRatio, m_fZapWidth, m_fZapHeight);
	Msg += Tmp;

	//	Visible source
	bFull = GetSrcVisible(&rcSrc);
	Tmp.Format("Visible Src Left %d\nVisible Src Top %d\nVisible Src Width %d\nVisible Src Height %d\nFull Visible %s\n",
				rcSrc.left, rcSrc.top, rcSrc.right, rcSrc.bottom, bFull ? "Yes" : "No");
	Msg += Tmp;

	MessageBox(Msg, lpTitle, MB_ICONINFORMATION | MB_OK);

#endif // _DEBUG

}

//==============================================================================
//
// 	Function Name:	CTMLead::Smooth()
//
// 	Description:	This function is called to smooth bumps and nicks in 
//					1-bit images
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMLead::Smooth(long lLength, int iFavorLong)
{
	long lFlags = 0;

	if(!IsLoaded())
	{
		return TMV_NOIMAGE;
	}
	else
	{
		//	Set this property to make sure the borders get removed
		SetDocCleanSuccess(SUCCESS_REMOVE);

		//	Set the flags
		if(iFavorLong != 0)
			lFlags |= SMOOTH_FAVOR_LONG;

		if((m_sLeadError = CLead::Smooth(lLength, lFlags)) != 0)
		{
			HandleLeadError(m_sLeadError);
			return TMV_LEADERROR;
		}
	
		//	Repaint the image
		CLead::ForceRepaint();

		return TMV_NOERROR;
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SourceToClient()
//
// 	Description:	This function will convert the rectangle's coordinates from
//					source image coordinates to client window coordinates
//				
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SourceToClient(RECT* pRect)
{
	BitmapToClient((float)pRect->left, (float)pRect->top);
	pRect->left = ROUND(GetConvertX());
	pRect->top  = ROUND(GetConvertY());

	BitmapToClient((float)pRect->right, (float)pRect->bottom);
	pRect->right = ROUND(GetConvertX());
	pRect->bottom = ROUND(GetConvertY());
}

//==============================================================================
//
// 	Function Name:	CTMLead::SyncAnnotation()
//
// 	Description:	This function will propagate the annotation provided by 
//					the caller to all callouts in the list
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SyncAnnotation(CAnnotation* pAnn, CCallout* pExclude) 
{
	HANNOBJECT		hAnn;
	CCallout*		pCallout;

	if(!pAnn || !m_pCallouts)
		return;

	//	Get the annotation object
	if((hAnn = GetHandleFromTag(pAnn->GetAnnTag())) == 0)
		return;

	//	Copy the annotation to all callout objects
	pCallout = m_pCallouts->First();
	while(pCallout != 0)
	{
		//	Create a copy of this annotation in the callout's container
		if(pCallout != pExclude)
		{
			//	Copy the annotation and link to the source annotation
			if(pCallout->CopyAnn(this, hAnn))
				pAnn->Add(pCallout);
		}

		//	Get the next callout
		pCallout = m_pCallouts->Next();
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::SyncSelection()
//
// 	Description:	This function will select the annotation specified by the
//					caller in each callout linked to the annotation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::SyncSelection(CAnnotation* pAnn, CCallout* pExclude) 
{
	CCallout*	pCallout;
	DWORD		dwTag;

	if(!pAnn || !m_pCallouts)
		return;

	//	Get the tag associated with this annotation
	if((dwTag = pAnn->GetAnnTag()) == 0)
		return;

	//	Select the annotation in this image
	SelectAnn(dwTag);

	//	Select the annotation in all callouts linked to it
	pCallout = pAnn->First();
	while(pCallout != 0)
	{
		//	Make sure this is still a valid callout
		if(m_pCallouts->Find(pCallout))
		{
			//	Create a copy of this annotation in the callout's container
			if(pCallout != pExclude)
				pCallout->SelectAnn(dwTag);
		}

		//	Get the next callout
		pCallout = pAnn->Next();
	}
}

//==============================================================================
//
// 	Function Name:	CTMLead::UnloadImage()
//
// 	Description:	This function is called to unload the current image.
//
// 	Returns:		None
//
//	Notes:			None		
//
//==============================================================================
void CTMLead::UnloadImage() 
{
	//	Always hide the Lead control
	if(IsWindow(m_hWnd))
		ShowWindow(SW_HIDE);

	//	Clear the bitmap handle
	SetBitmap(0);

	//	Reset the filename and other properties
	m_strFilename.Empty();
	m_fImageHeight = 0;
	m_fImageWidth = 0;
	m_fAspectRatio = 1;
	m_bAnimation = FALSE;

	//	The file is no longer loaded
	m_bLoaded = FALSE;
	m_sZoomState = ZOOMED_NONE;
	m_fZoomFactor = 1.0f;

	//	Clear the zap members
	ResetZapParameters();
}

//==============================================================================
//
// 	Function Name:	CTMLead::UnlockAnn()
//
// 	Description:	This function will unlock the annotation specified by the
//					caller
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMLead::UnlockAnn(HANNOBJECT hAnn) 
{
	CAnnotation* pAnn = 0;

	if((pAnn = GetAnnFromHandle(hAnn)) != 0)
	{
		if(pAnn->m_bIsLocked == TRUE)
		{
			L_AnnUnlock(hAnn, TMLEAD_LOCK_KEY, 0);
			pAnn->m_bIsLocked = FALSE;
			L_AnnSetTag(hAnn, pAnn->GetAnnTag(), 0);
		}

		return TMV_NOERROR;
	}
	else
	{
		if(m_pErrors != 0) m_pErrors->Handle(0, IDS_TMV_ANNNOTFOUND);
		return TMV_ANNNOTFOUND;
	}

}

//==============================================================================
//
// 	Function Name:	CTMLead::ViewImageProperties()
//
// 	Description:	This function is called to view the properties for the
//					image loaded in this pane.
//
// 	Returns:		TMV_NOERROR if succesful
//
//	Notes:			None		
//
//==============================================================================
short CTMLead::ViewImageProperties() 
{
	STMVImageProperties	Properties;
	CImageProperties	ViewProps(this);
	short				sError;

	//	Get the properties for the current image
	if((sError = GetImageProperties(&Properties)) != TMV_NOERROR)
		return sError;

	//	Initialize the dialog
	ViewProps.SetImageInfo(&Properties);

	//	Let the container know we are about to open the dialog box
	if(m_pControl)
		m_pControl->PreModalDialog();

	//	Open the dialog
	ViewProps.DoModal();

	if(m_pControl)
		m_pControl->PostModalDialog();

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMLead::ZoomFullHeight()
//
// 	Description:	This function will zoom the image so that it uses all the
//					available height while maintaining the aspect ratio.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::ZoomFullHeight() 
{
	RedrawFullHeight();
}

//==============================================================================
//
// 	Function Name:	CTMLead::ZoomFullWidth()
//
// 	Description:	This function will zoom the image so that it uses all the
//					available width while maintaining the aspect ratio.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::ZoomFullWidth() 
{
	RedrawFullWidth();
}

//==============================================================================
//
// 	Function Name:	CTMLead::ZoomRestricted()
//
// 	Description:	This function will zoom the image in on the rectangle 
//					specified by the caller. The display will be restricted to
//					the rectangle selected by the user
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::ZoomRestricted(RECT* pRect) 
{
    float   DstWidth;
    float   DstHeight;
    float   DstLeft;
    float   DstTop;
	int		iWidth;
	int		iHeight;

	ASSERT(pRect);

	//	Calculate the size of the zoom rectangle
	iWidth  = pRect->right - pRect->left;
	iHeight = pRect->bottom - pRect->top;

	//	Make sure the zoom rectangle isn't too small
	if(iWidth < MINIMUM_ZOOMRECT || iHeight < MINIMUM_ZOOMRECT)
		return;

	//  Find the current size of the destination rectangle. We need these
	//	to restore the destination rectangle after we make the move
	DstLeft   = GetDstLeft();
	DstTop	  = GetDstTop();
	DstWidth  = GetDstWidth();
	DstHeight = GetDstHeight();

	//	Size and position the window to match the aspect ratio of the 
	//	rubber band rectangle
	ResizeWndToRatio((float)iHeight, (float)iWidth);

	//	Restore the destination rectangle
	SetDstRect(DstLeft, DstTop, DstWidth, DstHeight);
	SetDstClipRect(DstLeft, DstTop, DstWidth, DstHeight);

	//	Zoom in on the selection
	ZoomToRect((float)pRect->left, (float)pRect->top, 
			   (float)iWidth, (float)iHeight);
    
	//	Redraw the image
	RedrawZoomed();

	//	Notify the callout that owns this object
	if(m_pOwner != 0)
		m_pOwner->OnZoomComplete();
}


//==============================================================================
//
// 	Function Name:	CTMLead::ZoomUnrestricted()
//
// 	Description:	This function will zoom the image in on the rectangle 
//					specified by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::ZoomUnrestricted(RECT* pRect) 
{
    float   DstWidth;
    float   DstHeight;
    float   DstLeft;
    float   DstTop;
	int		iWidth;
	int		iHeight;

	ASSERT(pRect);

	//	Calculate the size of the zoom rectangle
	iWidth  = pRect->right - pRect->left;
	iHeight = pRect->bottom - pRect->top;

	//	Make sure the zoom rectangle isn't too small
	if(iWidth < MINIMUM_ZOOMRECT || iHeight < MINIMUM_ZOOMRECT)
		return;

	//	Always hide the Lead control
	if(IsWindow(m_hWnd))
		ShowWindow(SW_HIDE);

    //	If the window is not already maximized we have to move it to take up the
	//	full client area so that the Lead control knows the area it has 
	//	available to do the zoom.
	if((m_iWidth != m_rcMax.right) || (m_iHeight != m_rcMax.bottom))
	{
		//  Find the current size of the destination rectangle. We need these
		//	to restore the destination rectangle after we make the move
		DstLeft   = GetDstLeft();
		DstTop	  = GetDstTop();
		DstWidth  = GetDstWidth();
		DstHeight = GetDstHeight();

		//	Move the window to take up the full client area
		MoveWindow(0,0,m_rcMax.right,m_rcMax.bottom,TRUE);

		//	Restore the destination rectangle
		SetDstRect(DstLeft, DstTop, DstWidth, DstHeight);
		SetDstClipRect(DstLeft, DstTop, DstWidth, DstHeight);

	}

	//	Zoom in on the selection
	ZoomToRect((float)pRect->left, (float)pRect->top, 
			   (float)iWidth, (float)iHeight);

	//	Redraw the image
	RedrawZoomed();

	//	Notify the callout that owns this object
	if(m_pOwner != 0)
		m_pOwner->OnZoomComplete();
}


//==============================================================================
//
// 	Function Name:	CTMLead::GesturePan()
//
// 	Description:	Public setter for protected Pan function used by 
//					gesture control
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLead::GesturePan(long lX, long lY)
{
	Pan(lX, lY);
}

//==============================================================================
//
// 	Function Name:	CTMLead::GestureZoom()
//
// 	Description:	Will zoomin/zoomout document on according to given
//                  zoom factor
//
// 	Returns:		None
//
//	Notes:			zoomfactor > 1 = zoom in 
//					zoomfactor < 1 = zoom out
//
//==============================================================================
void CTMLead::GestureZoom(float zoomFactor)
{
	int		iWidth;
	int		iHeight;
    float   DstWidth;
    float   DstHeight;
    float   DstLeft;
    float   DstTop;
	POINT	pCenter;

	iWidth  = m_rcMax.right/zoomFactor;
	iHeight = m_rcMax.bottom/zoomFactor;
	pCenter.x = (m_rcMax.right - iWidth )/2;
	pCenter.y = (m_rcMax.bottom - iHeight)/2;
	

	// limit zoomin
	if (m_fZoomFactor > m_fMaxZoom &&
		(iWidth <= m_rcMax.right && iHeight <= m_rcMax.bottom)) {
		return;
	}

	//	Always hide the Lead control
	if(IsWindow(m_hWnd))
		ShowWindow(SW_HIDE);

    //	If the window is not already maximized we have to move it to take up the
	//	full client area so that the Lead control knows the area it has 
	//	available to do the zoom.
	if((m_iWidth != m_rcMax.right) || (m_iHeight != m_rcMax.bottom))
	{
		//  Find the current size of the destination rectangle. We need these
		//	to restore the destination rectangle after we make the move
		DstLeft   = GetDstLeft();
		DstTop	  = GetDstTop();
		DstWidth  = GetDstWidth();
		DstHeight = GetDstHeight();

		// limit the window for zoomout
		if (DstWidth < m_rcMax.right/4)
			DstWidth = m_rcMax.right/4;

		if (DstHeight < m_rcMax.bottom/2)
			DstHeight = m_rcMax.bottom/2;

		//	Move the window to take up the full client area
		MoveWindow(0,0,m_rcMax.right,m_rcMax.bottom,TRUE);

		//	Restore the destination rectangle
		SetDstRect(DstLeft, DstTop, DstWidth, DstHeight);
		SetDstClipRect(DstLeft, DstTop, DstWidth, DstHeight);

	}


	//	Zoom in on the selection
	ZoomToRect((float)pCenter.x, (float)pCenter.y, 
			   (float)iWidth, (float)iHeight);

	// without this, image will go all funcky
	ResizeWndToRatio((float)iWidth, (float)iHeight);

	//	Redraw the image
	RedrawZoomed();
}

//==============================================================================
//
// 	Function Name:	CTMLead::ZoomToFactor()
//
// 	Description:	Will zoomin/zoomout current document according to
//                  previus zoom factor
//
// 	Returns:		None
//
//	Notes:			zoomfactor > 1 = zoom in 
//					zoomfactor < 1 = zoom out
//
//==============================================================================
void CTMLead::ZoomToFactor()
{
	int		iWidth;
	int		iHeight;
    float   DstWidth;
    float   DstHeight;
    float   DstLeft;
    float   DstTop;
	POINT	pCenter;

	iWidth  = m_rcMax.right/m_fZoomFactor;
	iHeight = m_rcMax.bottom/m_fZoomFactor;	

	// limit zoomin
	if (m_fZoomFactor > m_fMaxZoom ) {
			m_fZoomFactor = m_fMaxZoom;
	}

	//	Always hide the Lead control
	if(IsWindow(m_hWnd))
		ShowWindow(SW_HIDE);

    //	If the window is not already maximized we have to move it to take up the
	//	full client area so that the Lead control knows the area it has 
	//	available to do the zoom.
	if((m_iWidth != m_rcMax.right) || (m_iHeight != m_rcMax.bottom))
	{
		//  Find the current size of the destination rectangle. We need these
		//	to restore the destination rectangle after we make the move
		DstLeft   = GetDstLeft();
		DstTop	  = GetDstTop();
		DstWidth  = GetDstWidth();
		DstHeight = GetDstHeight();

		pCenter.x = (m_rcMax.right - iWidth )/2;
		pCenter.y = (m_rcMax.bottom - iHeight)/2;

		// limit the window for zoomout
		if (DstWidth < m_rcMax.right/4)
			DstWidth = m_rcMax.right/4;

		if (DstHeight < m_rcMax.bottom/2)
			DstHeight = m_rcMax.bottom/2;

		//	Move the window to take up the full client area
		MoveWindow(0,0,m_rcMax.right,m_rcMax.bottom,TRUE);

		//	Restore the destination rectangle
		SetDstRect(DstLeft, DstTop, DstWidth, DstHeight);
		SetDstClipRect(DstLeft, DstTop, DstWidth, DstHeight);

		//	Zoom in on the selection
		ZoomToRect((float)pCenter.x, (float)pCenter.y, 
				   (float)iWidth, (float)iHeight);

		// without this, image will go all funcky
		ResizeWndToRatio((float)iWidth, (float)iHeight);

		//	Redraw the image
		RedrawZoomed();

	} 
	else 
	{
		RedrawFullWidth();
	}
}