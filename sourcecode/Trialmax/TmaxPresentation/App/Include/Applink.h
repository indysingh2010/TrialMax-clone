//==============================================================================
//
// File Name:	applink.h
//
// Description:	This file contains the declaration of the CAppLink class
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2008
//
//==============================================================================
//	Date		Revision    Description
//	02-09-2008	1.00		Original Release
//==============================================================================
#if !defined(__APPLINK_H__)
#define __APPLINK_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <dbdefs.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	This class is used to manage the information used by the application when
//	displaying a linked image or presentation
class CAppLink : public CObject
{
	private:

		BOOL			m_bIsPending;
		BOOL			m_bIsEvent;
		long			m_lAttributes;

		CString			m_strBarcode;
		CString			m_strDatabaseId;

	public:

						CAppLink();
					   ~CAppLink();

		CString&		GetBarcode(){ return m_strBarcode; }
		CString&		GetDatabaseId(){ return m_strDatabaseId; }
		long			GetAttributes(){ return m_lAttributes; }
		BOOL			GetIsPending(){ return m_bIsPending; }
		BOOL			GetIsEvent(){ return m_bIsEvent; }
		BOOL			GetHideLink();
		BOOL			GetHideVideo(){ return GetFlag(TMFLAG_LINK_HIDE_VIDEO); }
		BOOL			GetHideText(){ return GetFlag(TMFLAG_LINK_HIDE_TEXT); }
		BOOL			GetSplitScreen(){ return GetFlag(TMFLAG_LINK_SPLITSCREEN); }

		void			SetBarcode(LPCSTR lpszBarcode){ m_strBarcode = (lpszBarcode != NULL) ? lpszBarcode : ""; }
		void			SetDatabaseId(LPCSTR lpszDatabaseId){ m_strDatabaseId = (lpszDatabaseId != NULL) ? lpszDatabaseId : ""; }
		void			SetAttributes(long lAttributes){ m_lAttributes = lAttributes; }
		void			SetIsPending(BOOL bIsPending){ m_bIsPending = bIsPending; }
		void			SetIsEvent(BOOL bIsEvent){ m_bIsEvent = bIsEvent; }
		void			SetHideLink(BOOL bHide){ SetFlag(TMFLAG_LINK_HIDE, bHide); }
		void			SetHideVideo(BOOL bHide){ SetFlag(TMFLAG_LINK_HIDE_VIDEO, bHide); }
		void			SetHideText(BOOL bHide){ SetFlag(TMFLAG_LINK_HIDE_TEXT, bHide); }
		void			SetSplitScreen(BOOL bSplitScreen){ SetFlag(TMFLAG_LINK_SPLITSCREEN, bSplitScreen); }

		UINT			MsgBox(HWND hWnd);
		void			Clear();

	protected:

		BOOL			GetFlag(long lMask){ return ((m_lAttributes & lMask) != 0); }
		void			SetFlag(long lMask, BOOL bState);
};

#endif // !defined(__APPLINK_H__)
