//==============================================================================
//
// File Name:	acknow.h
//
// Description:	This file contains the declaration of the CAcknowledge class. 
//				This class is used to create and manage a shared memory region 
//				that allows the apps to acknowledge requests and responses 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 2009
//
//==============================================================================
//	Date		Revision    Description
//	03-18-2008	1.00		Original Release
//==============================================================================
#if !defined(__ACKNOW_H__)
#define __ACKNOW_H__

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

class CAcknowledge : public CShared
{
	private:

		SStatus*			m_lpStatus;

	public:
	
							CAcknowledge();
		virtual			   ~CAcknowledge();

		//	Overloaded base class members
		BOOL				SetProperties(LPCSTR lpszName);
		BOOL				Open();
		BOOL				Close();
		long				Read(SStatus* lpRequest, BOOL bZeroWrites = TRUE);
		long				Write(SStatus* lpRequest, BOOL bZeroWrites = FALSE);

	protected:

};

#endif // __ACKNOW_H__
