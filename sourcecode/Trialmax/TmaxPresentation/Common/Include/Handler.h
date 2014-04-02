//==============================================================================
//
// File Name:	handler.h
//
// Description:	This file contains the declaration of the CErrorHandler class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	08-09-97	1.00		Original Release
//==============================================================================
#if !defined(__HANDLER_H__)
#define __HANDLER_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CErrorHandler : public CObject
{
	private:

		HWND		m_hParent;
		BOOL		m_bEnabled;
		CString		m_strTitle;
		UINT		m_uMessageId;
		CString		m_strMessage;
	
	public:

					CErrorHandler();
				   ~CErrorHandler();

		void		Enable(BOOL bEnable);
		void		SetParent(HWND hParent);
		void		SetTitle(LPCSTR lpTitle);
		void		SetMessageId(UINT uId);
		BOOL		IsEnabled();
		HWND		GetParent();
		LPCSTR		GetMessage(){ return m_strMessage; }

		void		Handle(LPCSTR lpTitle, UINT uErrorId, 
						   LPCSTR lpArg1 = 0, LPCSTR lpArg2 = 0);
		void		Handle(LPCSTR lpTitle, LPCSTR lpMessage);
	
	protected:

};

#endif // !defined(__HANDLER_H__)
