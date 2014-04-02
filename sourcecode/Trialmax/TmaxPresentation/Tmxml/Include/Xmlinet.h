//==============================================================================
//
// File Name:	xmlinet.h
//
// Description:	This file contains the declarations of the CXmlSession class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2001
//
//==============================================================================
//	Date		Revision    Description
//	07-15-01	1.00		Original Release
//==============================================================================
#if !defined(__XMLINET_H__)
#define __XMLINET_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <afxinet.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CXmlFrame;

class CXmlSession : public CInternetSession
{
	private:

		CXmlFrame*			m_pXmlFrame;

	public:
	
							CXmlSession(CXmlFrame* pFrame, DWORD dwContext,
										DWORD dwAccessType, LPCSTR lpProxyName);
		virtual			   ~CXmlSession();

		void				OnStatusCallback(DWORD dwContext, 
											 DWORD dwInternetStatus, 
											 LPVOID lpvStatusInformation, 
											 DWORD dwStatusInformationLength);

	protected:

};

#endif // !defined(__XMLINET_H__)
