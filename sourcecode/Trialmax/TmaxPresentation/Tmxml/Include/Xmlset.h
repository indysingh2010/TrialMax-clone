//==============================================================================
//
// File Name:	xmlset.h
//
// Description:	This file contains the declarations of the CXmlSettings class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2001
//
//==============================================================================
//	Date		Revision    Description
//	05-27-01	1.00		Original Release
//==============================================================================
#if !defined(__XMLSET_H__)
#define __XMLSET_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	XML child node identifiers
#define TMXML_SETTING_VERSION		"reqVersion"
#define TMXML_SETTING_BASEURL		"baseURL"

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	This class manages the collection of settings for a txm/xml session
class CXmlSettings
{
	private:

	public:

		int					m_iVerMajor;
		int					m_iVerMinor;
		CString				m_strBaseURL;

							CXmlSettings();
		virtual			   ~CXmlSettings();

		BOOL				Add(LPCSTR lpName, LPCSTR lpValue);

	protected:

};

#endif // !defined(__XMLSET_H__)
