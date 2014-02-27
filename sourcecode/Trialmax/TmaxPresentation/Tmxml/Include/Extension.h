//==============================================================================
//
// File Name:	extension.h
//
// Description:	This file contains the declaration of the CExtensions class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	02-22-02	1.00		Original Release
//==============================================================================
#if !defined(__EXTENSION_H__)
#define __EXTENSION_H__

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
class CExtension : public CObject
{
	private:

	public:

		CString					m_strExtension;

								CExtension(LPCSTR lpszExtension = 0);
};

class CExtensions : public CObList
{
	private:

	public:

								CExtensions();
		virtual				   ~CExtensions();

		BOOL					Add(LPCSTR lpExtension);
		void					Flush(BOOL bDelete);
		void					Remove(CExtension* pExtension, BOOL bDelete);
		POSITION				Find(CExtension* pExtension);
		CExtension*				Find(LPCSTR lpszExtension);

		//	List iteration members
		CExtension*				First();
		CExtension*				Last();
		CExtension*				Next();
		CExtension*				Prev();

	protected:

		POSITION				m_NextPos;
		POSITION				m_PrevPos;
};

#endif // !defined(__EXTENSION_H__)
