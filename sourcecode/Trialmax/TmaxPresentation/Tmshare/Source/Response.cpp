//==============================================================================
//
// File Name:	response.cpp
//
// Description:	This file contains member functions of the CResponse class
//
// See Also:	response.h
//
// Copyright:
//
//==============================================================================
//	Date		Revision    Description
//	04-19-2002	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <response.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//
// 	Function Name:	CShared::Close()
//
//	Parameters:		None
//
// 	Return Value:	TRUE if successful
//
// 	Description:	Called to free the shared memory region and all resources
//
//------------------------------------------------------------------------------
BOOL CResponse::Close()
{
	m_lpResponse = NULL;

	return CShared::Close();
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CResponse::CResponse()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Default constructor
//
//------------------------------------------------------------------------------
CResponse::CResponse() : CShared()
{
	m_lpResponse = NULL;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CResponse::~CResponse()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Destructor
//
//------------------------------------------------------------------------------
CResponse::~CResponse()
{
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CResponse::Open()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	This function is called to open the shared memory region.
//
//------------------------------------------------------------------------------
BOOL CResponse::Open()
{
	if(CShared::Open())
	{
		ASSERT(m_lpShared != NULL);
		m_lpResponse = (SResponse*)m_lpShared;
	}
	return (m_lpResponse != NULL);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CResponse::Read()
//
//	Parameters:		lpResponse - pointer to command response buffer
//					bZeroCoutn - TRUE to zero the write count after reading
//
// 	Return Value:	The current write count
//
// 	Description:	This function is called to read the contents of shared
//					memory into the specified response buffer.
//
//------------------------------------------------------------------------------
long CResponse::Read(SResponse* lpResponse, BOOL bZeroCount)
{
	ASSERT(lpResponse);
	return CShared::Read((LPBYTE)lpResponse, bZeroCount);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CResponse::SetProperties()
//
//	Parameters:		lpszName - name used to identify the shared memory region
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to set the properties used to create
//					the shared memory region.
//
//------------------------------------------------------------------------------
BOOL CResponse::SetProperties(LPCSTR lpszName)
{
	if((lpszName != 0) && (lstrlen(lpszName) > 0))
	{
		//	Format the name used for this buffer
		m_strName.Format("%s_Response", lpszName);

		//	Now initialize the complete set of properties
		return CShared::SetProperties(m_strName, sizeof(SResponse));
	}
	else
	{
		return FALSE;
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CResponse::Write()
//
//	Parameters:		lpResponse - pointer to command response
//
// 	Return Value:	The new write count
//
// 	Description:	This function is called to write the command response into
//					shared memory
//
//------------------------------------------------------------------------------
long CResponse::Write(SResponse* lpResponse, BOOL bZeroWrites)
{
	ASSERT(lpResponse);
	return CShared::Write((LPBYTE)lpResponse, bZeroWrites);
}

