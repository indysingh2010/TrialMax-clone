//==============================================================================
//
// File Name:	shared.cpp
//
// Description:	This file contains member functions of the CShared class
//
// See Also:	shared.h
//
// Copyright:
//
//==============================================================================
//	Date		Revision    Description
//	04-11-2002	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <shared.h>

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
BOOL CShared::Close()
{
	//	Release the file and resources
	if(m_hMutex != NULL)
	{
		CloseHandle(m_hMutex);
		m_hMutex = NULL;
	}

	if(m_lpShared != NULL)
	{
		::UnmapViewOfFile(m_lpShared);
		m_lpShared = NULL;
	}

	if(m_hShared != NULL)
	{
		CloseHandle(m_hShared);
		m_hShared = NULL;
	}

	m_bFirst = FALSE;

	return TRUE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CShared::CShared()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Default constructor
//
//------------------------------------------------------------------------------
CShared::CShared()
{
	m_lpShared  = NULL;
	m_hMutex    = NULL;
	m_hShared   = NULL;
	m_bFirst    = FALSE;
	m_dwSize    = 0;
	m_strName.Empty();
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CShared::~CShared()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Destructor
//
//------------------------------------------------------------------------------
CShared::~CShared()
{
	//	Make sure shared memory has been deallocated
	Close();
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CShared::GetWrites()
//
//	Parameters:		None
//
// 	Return Value:	The current write count
//
// 	Description:	Called to read the current write count for the region
//
//------------------------------------------------------------------------------
long CShared::GetWrites()
{
	long	lWrites;
	SHeader	Header;

	//	Make sure we've created the memory region
	ASSERT(m_lpShared != NULL);
	if(m_lpShared == NULL) return -1;

	//	Lock access to the memory
	if(!Lock())
	{
		ASSERT(0);
		return -1;
	}

	//	What is the current write count ?
	memcpy(&Header, m_lpShared, sizeof(SHeader));
	lWrites = Header.lWrites;

	Unlock();
	return lWrites;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CShared::Lock()
//
//	Parameters:		None
//
// 	Return Value:	TRUE if successful
//
// 	Description:	Called to lock access to the memory
//
//------------------------------------------------------------------------------
BOOL CShared::Lock()
{
	if(m_hMutex != NULL)
	{
		if(::WaitForSingleObject(m_hMutex, INFINITE) == WAIT_OBJECT_0)
		{
			return TRUE;
		}
	}
	return FALSE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CShared::Open()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	This function is called to open the shared memory region.
//
//------------------------------------------------------------------------------
BOOL CShared::Open()
{
	DWORD	dwResult;
	CString	strMutex;
	DWORD	dwSize;

	ASSERT(m_strName.GetLength() > 0);
	ASSERT(m_dwSize > 0);

	//	Make sure we have not already created the shared memory region
	ASSERT(m_lpShared == NULL);
	if(m_lpShared != NULL) return FALSE;

	//	We want to put a header in front of the user's buffer
	dwSize = m_dwSize + sizeof(SHeader);

	//	Create a shared memory region
	m_hShared = ::CreateFileMapping((HANDLE)0xFFFFFFFF, NULL, PAGE_READWRITE,
								  0, dwSize, m_strName);

	//	Check to see if the file was already created by another process
	dwResult = ::GetLastError();
	m_bFirst = (dwResult == ERROR_ALREADY_EXISTS) ? FALSE : TRUE;

	ASSERT(m_hShared != NULL);
	ASSERT(m_hShared != INVALID_HANDLE_VALUE);

	if((m_hShared == NULL) || (m_hShared == INVALID_HANDLE_VALUE))
		return FALSE;

	//	Convert the file handle into a pointer
	if((m_lpShared = (LPBYTE)::MapViewOfFile(m_hShared, FILE_MAP_ALL_ACCESS, 0, 0, 0)) == NULL)
	{
		ASSERT(m_lpShared != NULL);
		Close();
		return FALSE;
	}

	// Create (or connect to) a shared mutex for locking and unlocking
	strMutex.Format("%s_Mutex", m_strName);
	if((m_hMutex = ::CreateMutex(NULL, TRUE, strMutex)) == NULL)
	{
		ASSERT(m_hMutex != NULL);
		Close();
		return FALSE;
	}

	// Release the mutex just in case we gained ownership when we created it
	ReleaseMutex(m_hMutex);

	//	Initialize the memory if we were the first to create it
	if(m_bFirst)
	{
		//	Lock access so that we can initialize the memory
		if(!Lock())
		{
			ASSERT(0);
			Close();
			return FALSE;
		}

		//	Zero out the entire memory region
		memset(m_lpShared, 0, dwSize);

		//	Release the lock
		Unlock();
	}

	return TRUE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CShared::Read()
//
//	Parameters:		lpBuffer    - pointer to buffer in which to store the data
//					bZeroWrites - TRUE to reset the write count to zero
//
// 	Return Value:	The current write count
//
// 	Description:	Called to read the shared memory into the caller's buffer
//
//------------------------------------------------------------------------------
long CShared::Read(LPBYTE lpBuffer, BOOL bZeroWrites)
{
	long	lCount;
	SHeader	Header;

	ASSERT(lpBuffer != 0);

	//	Make sure we've created the memory region
	ASSERT(m_lpShared != NULL);
	if(m_lpShared == NULL) return -1;

	//	Lock access to the memory
	if(!Lock())
	{
		ASSERT(0);
		return -1;
	}

	//	What is the current write count ?
	memcpy(&Header, m_lpShared, sizeof(SHeader));
	lCount = Header.lWrites;

	//	Read the data in shared memory
	//
	//	NOTE:	We assume the caller provided a buffer of the correct size
	memcpy(lpBuffer, (m_lpShared + sizeof(SHeader)), m_dwSize);

	//	Should we reset the write count?
	if(bZeroWrites)
	{
		Header.lWrites = 0;
		memcpy(m_lpShared, &Header, sizeof(Header));
	}

	Unlock();
	return lCount;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CShared::SetProperties()
//
//	Parameters:		lpszName - name used to identify the shared memory region
//					dwSize   - size in bytes of the desired region
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to set the properties used to create
//					the shared memory region.
//
//------------------------------------------------------------------------------
BOOL CShared::SetProperties(LPCSTR lpszName, DWORD dwSize)
{
	if((lpszName != 0) && (lstrlen(lpszName) > 0) && (dwSize > 0))
	{
		m_strName = lpszName;
		m_dwSize  = dwSize;
		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CShared::Unlock()
//
//	Parameters:		None
//
// 	Return Value:	TRUE if successful
//
// 	Description:	Called to unlock access to the shared memory region

//
//------------------------------------------------------------------------------
BOOL CShared::Unlock()
{
	if(m_hMutex != NULL)
	{
		ReleaseMutex(m_hMutex);
	}
	return TRUE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CShared::Write()
//
//	Parameters:		lpBuffer - pointer to data to be transferred
//					bZeroWrites - TRUE to reset the write count to zero
//				
//
// 	Return Value:	The new write count
//
// 	Description:	Called to write the data in the caller's buffer into shared
//					memory
//
//------------------------------------------------------------------------------
long CShared::Write(LPBYTE lpBuffer, BOOL bZeroWrites)
{
	SHeader Header;

	//	Make sure we've created the memory region
	ASSERT(m_lpShared != NULL);
	if(m_lpShared == NULL) return -1;

	//	Lock access to the memory
	if(!Lock())
	{
		ASSERT(0);
		return -1;
	}

	//	Write the data
	//
	//	NOTE:	We assume the caller provided a buffer of the correct size
	memcpy((m_lpShared + sizeof(SHeader)), lpBuffer, m_dwSize);

	//	Set the write count
	memcpy(&Header, m_lpShared, sizeof(SHeader));
	if(bZeroWrites)
		Header.lWrites = 0;
	else
		Header.lWrites++;
	memcpy(m_lpShared, &Header, sizeof(SHeader));

	Unlock();
	return Header.lWrites;
}

