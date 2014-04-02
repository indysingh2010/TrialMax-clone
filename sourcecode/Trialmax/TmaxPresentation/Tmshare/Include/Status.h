//==============================================================================
//
// File Name:	status.h
//
// Description:	This file contains the declaration of the CStatus class. This
//				class is used to create and manage a shared memory region for
//				monitoring app status across process boundries. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	04-19-2002	1.00		Original Release
//==============================================================================
#if !defined(__STATUS_H__)
#define __STATUS_H__

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <shared.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
typedef struct 
{
	DWORD	dwProcessId;
	HANDLE	hProcess;
	HWND	hWnd;
}SStatus;

class CStatus : public CShared
{
	private:

		SStatus*			m_lpStatus;

	public:
	
							CStatus();
		virtual			   ~CStatus();

		//	Overloaded base class members
		BOOL				SetProperties(LPCSTR lpszName);
		BOOL				Open();
		BOOL				Close();
		long				Read(SStatus* lpRequest, BOOL bZeroWrites = TRUE);
		long				Write(SStatus* lpRequest, BOOL bZeroWrites = FALSE);

	protected:

};

#endif // __STATUS_H__
