//==============================================================================
//
// File Name:	xmlset.cpp
//
// Description:	This file contains member functions of the CXmlSettings class.
//
// See Also:	xmlset.h
//
// Copyright	FTI Consulting 1997-2001
//
//==============================================================================
//	Date		Revision    Description
//	05-27-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <xmlset.h>

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
// 	Function Name:	CXmlSettings::Add()
//
// 	Description:	This function is called to add the setting specified by
//					the caller.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CXmlSettings::Add(LPCSTR lpName, LPCSTR lpValue)
{
	char	szVersion[16];
	char*	pToken;

	ASSERT(lpName);
	ASSERT(lpValue);

	if(lstrcmpi(lpName, TMXML_SETTING_VERSION) == 0)
	{
		//	Copy the version information to a working buffer
		lstrcpyn(szVersion, lpValue, sizeof(szVersion));

		//	Search for the major/minor delimiter
		if((pToken = strchr(szVersion, '.')) != 0)
		{
			*pToken = 0;
			m_iVerMajor = atoi(szVersion);
			m_iVerMinor = atoi(pToken + 1);
		}
		else
		{
			m_iVerMajor = atoi(szVersion);
			m_iVerMinor = 0;
		}
		return TRUE;

	}
	else if(lstrcmpi(lpName, TMXML_SETTING_BASEURL) == 0)
	{
		m_strBaseURL = lpValue;
		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CXmlSettings::CXmlSettings()
//
// 	Description:	This is the constructor for CXmlSettings objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlSettings::CXmlSettings()
{
	m_iVerMajor = 0;
	m_iVerMinor = 0;
	m_strBaseURL.Empty();
}

//==============================================================================
//
// 	Function Name:	CXmlSettings::~CXmlSettings()
//
// 	Description:	This is the destructor for CXmlSettings objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlSettings::~CXmlSettings()
{
}

