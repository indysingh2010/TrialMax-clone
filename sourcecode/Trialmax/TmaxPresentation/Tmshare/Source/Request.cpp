//==============================================================================
//
// File Name:	request.cpp
//
// Description:	This file contains member functions of the CRequest class
//
// See Also:	request.h
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
#include <request.h>

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
BOOL CRequest::Close()
{
	m_lpRequest = NULL;

	return CShared::Close();
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRequest::CRequest()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Default constructor
//
//------------------------------------------------------------------------------
CRequest::CRequest() : CShared()
{
	m_lpRequest = NULL;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRequest::~CRequest()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Destructor
//
//------------------------------------------------------------------------------
CRequest::~CRequest()
{
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRequest::Open()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	This function is called to open the shared memory region.
//
//------------------------------------------------------------------------------
BOOL CRequest::Open()
{
	if(CShared::Open())
	{
		ASSERT(m_lpShared != NULL);
		m_lpRequest = (SRequest*)m_lpShared;
	}
	return (m_lpRequest != NULL);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRequest::Read()
//
//	Parameters:		lpRequest - pointer to command request buffer
//					bZeroCoutn - TRUE to zero the write count after reading
//
// 	Return Value:	The current write count
//
// 	Description:	This function is called to read the contents of shared
//					memory into the specified request buffer.
//
//------------------------------------------------------------------------------
long CRequest::Read(SRequest* lpRequest, BOOL bZeroCount)
{
	ASSERT(lpRequest);
	return CShared::Read((LPBYTE)lpRequest, bZeroCount);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRequest::SetProperties()
//
//	Parameters:		lpszName - name used to identify the shared memory region
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to set the properties used to create
//					the shared memory region.
//
//------------------------------------------------------------------------------
BOOL CRequest::SetProperties(LPCSTR lpszName)
{
	if((lpszName != 0) && (lstrlen(lpszName) > 0))
	{
		//	Format the name used for this buffer
		m_strName.Format("%s_Request", lpszName);

		//	Now initialize the complete set of properties
		return CShared::SetProperties(m_strName, sizeof(SRequest));
	}
	else
	{
		return FALSE;
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRequest::Write()
//
//	Parameters:		lpRequest - pointer to command request
//
// 	Return Value:	The new write count
//
// 	Description:	This function is called to write the command request into
//					shared memory
//
//------------------------------------------------------------------------------
long CRequest::Write(SRequest* lpRequest, BOOL bZeroWrites)
{
	ASSERT(lpRequest);
	return CShared::Write((LPBYTE)lpRequest, bZeroWrites);
}

