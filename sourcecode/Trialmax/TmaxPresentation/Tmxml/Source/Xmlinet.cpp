//==============================================================================
//
// File Name:	xmlinet.cpp
//
// Description:	This file contains member functions of the CXmlSession class.
//
// See Also:	xmlinet.h
//
//==============================================================================
//	Date		Revision    Description
//	07-15-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <xmlinet.h>
#include <xmlframe.h>

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

//==============================================================================
//
// 	Function Name:	CXmlSession::CXmlSession()
//
// 	Description:	This is the constructor for CXmlSession objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlSession::CXmlSession(CXmlFrame* pFrame, DWORD dwContext, DWORD dwAccessType,
						 LPCSTR lpProxyName) 
			:CInternetSession(NULL, dwContext, dwAccessType, lpProxyName)
{
	m_pXmlFrame = pFrame;
}

//==============================================================================
//
// 	Function Name:	CXmlSession::~CXmlSession()
//
// 	Description:	This is the destructor for CXmlSession objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CXmlSession::~CXmlSession()
{
}

//==============================================================================
//
// 	Function Name:	CXmlSession::OnStatusCallback()
//
// 	Description:	Overloaded base class function to track progress of the
//					operation.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CXmlSession::OnStatusCallback(DWORD dwContext, DWORD dwInternetStatus, 
								   LPVOID lpvStatusInformation, 
								   DWORD dwStatusInformationLength)
{
	AFX_MANAGE_STATE(AfxGetAppModuleState());
}

