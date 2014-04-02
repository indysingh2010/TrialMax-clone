//==============================================================================
//
// File Name:	status.cpp
//
// Description:	This file contains member functions of the CStatus class
//
// See Also:	status.h
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
#include <status.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//
// 	Function Name:	CStatus::Close()
//
//	Parameters:		None
//
// 	Return Value:	TRUE if successful
//
// 	Description:	Called to free the shared memory region and all resources
//
//------------------------------------------------------------------------------
BOOL CStatus::Close()
{
	m_lpStatus = NULL;

	return CShared::Close();
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CStatus::CStatus()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Default constructor
//
//------------------------------------------------------------------------------
CStatus::CStatus() : CShared()
{
	m_lpStatus = NULL;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CStatus::~CStatus()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Destructor
//
//------------------------------------------------------------------------------
CStatus::~CStatus()
{
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CStatus::Open()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	This function is called to open the shared memory region.
//
//------------------------------------------------------------------------------
BOOL CStatus::Open()
{
	if(CShared::Open())
	{
		ASSERT(m_lpShared != NULL);
		m_lpStatus = (SStatus*)m_lpShared;
	}
	return (m_lpStatus != NULL);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CStatus::Read()
//
//	Parameters:		lpRequest - pointer to command request buffer
//					bZeroCount - TRUE to zero the write count after reading
//
// 	Return Value:	The current write count
//
// 	Description:	This function is called to read the contents of shared
//					memory into the specified request buffer.
//
//------------------------------------------------------------------------------
long CStatus::Read(SStatus* lpRequest, BOOL bZeroCount)
{
	ASSERT(lpRequest);
	return CShared::Read((LPBYTE)lpRequest, bZeroCount);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CStatus::SetProperties()
//
//	Parameters:		lpszName - name used to identify the shared memory region
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to set the properties used to create
//					the shared memory region.
//
//------------------------------------------------------------------------------
BOOL CStatus::SetProperties(LPCSTR lpszName)
{
	if((lpszName != 0) && (lstrlen(lpszName) > 0))
	{
		//	Format the name used for this buffer
		m_strName.Format("%s_Status", lpszName);

		//	Now initialize the complete set of properties
		return CShared::SetProperties(m_strName, sizeof(SStatus));
	}
	else
	{
		return FALSE;
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CStatus::Write()
//
//	Parameters:		lpRequest - pointer to command request
//
// 	Return Value:	The new write count
//
// 	Description:	This function is called to write the command request into
//					shared memory
//
//------------------------------------------------------------------------------
long CStatus::Write(SStatus* lpRequest, BOOL bZeroWrites)
{
	ASSERT(lpRequest);
	return CShared::Write((LPBYTE)lpRequest, bZeroWrites);
}

