//==============================================================================
//
// File Name:	filedata.cpp
//
// Description:	This file contains member functions of the CFileData and
//				CFileDataList classes.
//
// See Also:	filedata.h, filectrl.h, drivctrl.h
//
//==============================================================================
//	Date		Revision    Description
//	07-09-97	1.00		Original Release
//	04-20-98	1.01		Added CFileDataList iteration functions
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <filedata.h>

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
// 	Function Name:	CFileData::CFileData()
//
// 	Description:	This is the constructor for CFileData objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CFileData::CFileData(LPCSTR lpParent, WIN32_FIND_DATA* pFindData,
					 int iSortField)
{
	int iIndex;

	//	Initialize local data
	if(pFindData)
	{
		m_dwAttributes	 = pFindData->dwFileAttributes;
		m_dwFileSizeHigh = pFindData->nFileSizeHigh;
		m_dwFileSizeLow	 = pFindData->nFileSizeLow;
		m_ftCreated		 = pFindData->ftCreationTime;
		m_ftLastAccess	 = pFindData->ftLastAccessTime;
		m_ftLastWrite	 = pFindData->ftLastWriteTime;
		m_strFileName	 = pFindData->cFileName;
		m_strAlternate	 = pFindData->cAlternateFileName;

		iIndex = m_strFileName.ReverseFind('.');
		if(iIndex > 0)
			m_strExtension = m_strFileName.Right(m_strFileName.GetLength() - (iIndex + 1));
		else
			m_strExtension.Empty();
	}
	else
	{
		m_dwAttributes	 = 0;
		m_dwFileSizeHigh = 0;
		m_dwFileSizeLow	 = 0;
		memset(&m_ftCreated, 0, sizeof(m_ftCreated));
		memset(&m_ftLastAccess, 0, sizeof(m_ftLastAccess));
		memset(&m_ftLastWrite, 0, sizeof(m_ftLastWrite));
		m_strFileName.Empty();
		m_strExtension.Empty();
		m_strAlternate.Empty();
	}

	//	These members don't depend on the FINDDATA structure
	m_strParent = lpParent;
	m_strDisplayName.Empty();
	m_strTypeName.Empty();
	m_strFilespec.Empty();
	m_iIcon = 0;
	m_hIcon = 0;
	m_hImageList = 0;
	m_iSortField = iSortField;

}

//==============================================================================
//
// 	Function Name:	CFileData::CFileData()
//
// 	Description:	This is the copy constructor for CFileData objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CFileData::CFileData(CFileData& FD)
{
	//	Initialize local data
	m_dwAttributes	 = FD.m_dwAttributes;
	m_dwFileSizeHigh = FD.m_dwFileSizeHigh;
	m_dwFileSizeLow	 = FD.m_dwFileSizeLow;
	m_ftCreated		 = FD.m_ftCreated;
	m_ftLastAccess	 = FD.m_ftLastAccess;
	m_ftLastWrite	 = FD.m_ftLastWrite;
	m_strFileName	 = FD.m_strFileName;
	m_strAlternate	 = FD.m_strAlternate;
	m_strExtension	 = FD.m_strExtension;
	m_strParent		 = FD.m_strParent;
	m_strFilespec	 = FD.m_strFilespec;
	m_strDisplayName = FD.m_strDisplayName;
	m_strTypeName	 = FD.m_strTypeName;
	m_iIcon			 = FD.m_iIcon;
	m_hIcon			 = FD.m_hIcon;
	m_hImageList	 = FD.m_hImageList;
	m_iSortField	 = FD.m_iSortField;

}

//==============================================================================
//
// 	Function Name:	CFileData::GetFilespec()
//
// 	Description:	This function is called to retrieve the full file 
//					specification for the file.
//
// 	Returns:		A pointer to the full file specification
//
//	Notes:			None
//
//==============================================================================
LPCSTR CFileData::GetFilespec()
{
	m_strFilespec = m_strParent;
	if(m_strFilespec.Right(1) != "\\")
		m_strFilespec += "\\";
	m_strFilespec += m_strFileName;

	return m_strFilespec;
}

//==============================================================================
//
// 	Function Name:	CFileData::GetIconHandle()
//
// 	Description:	This function will retrieve the handle of the icon 
//					associated with this file. The icon should be freed by the
//					caller before the application terminates.
//
// 	Returns:		TRUE if successful
//
//	Notes:			This function will store the handle of the icon specified
//					in the call in the m_hIcon member. 
//
//==============================================================================
BOOL CFileData::GetIconHandle(BOOL bSmallIcon, BOOL bOpenIcon)
{
	SHFILEINFO	FileInfo;
	UINT		uFlags = SHGFI_ICON;
	CString		strFileSpec;

	strFileSpec = m_strParent;
	if(strFileSpec.Right(1) != "\\")
		strFileSpec += "\\";
	strFileSpec += m_strFileName;
	
	//	Modify the flags to get the appropriate icon
	if(bSmallIcon)
		uFlags |= SHGFI_SMALLICON;
	else
		uFlags |= SHGFI_LARGEICON;
	
	if(bOpenIcon)
		uFlags |= SHGFI_OPENICON;
		
	if(!SHGetFileInfo(strFileSpec, 0, &FileInfo, sizeof(SHFILEINFO), uFlags))
		return FALSE;

	m_hIcon = FileInfo.hIcon;

	return TRUE;

}

//==============================================================================
//
// 	Function Name:	CFileData::GetIconIndex()
//
// 	Description:	This function will retrieve the index of the icon specified
//					by the caller.
//
// 	Returns:		TRUE if successful
//
//	Notes:			This function will store the index of the icon specified
//					in the call in the m_iIcon member. The handle of the image
//					list containing the icon is stored in m_hImageList.
//
//==============================================================================
BOOL CFileData::GetIconIndex(BOOL bSmallIcon, BOOL bOpenIcon)
{
	SHFILEINFO	FileInfo;
	UINT		uFlags = SHGFI_SYSICONINDEX;
	CString		strFileSpec;

	strFileSpec = m_strParent;
	if(strFileSpec.Right(1) != "\\")
		strFileSpec += "\\";
	strFileSpec += m_strFileName;
	
	//	Modify the flags to get the appropriate icon
	if(bSmallIcon)
		uFlags |= SHGFI_SMALLICON;
	else
		uFlags |= SHGFI_LARGEICON;
	
	if(bOpenIcon)
		uFlags |= SHGFI_OPENICON;
		
	m_hImageList = (HIMAGELIST)SHGetFileInfo(strFileSpec, 0, &FileInfo, 
											 sizeof(SHFILEINFO), uFlags);
	if(m_hImageList == NULL)
		return FALSE;

	m_hIcon = FileInfo.hIcon;
	m_iIcon = FileInfo.iIcon;

	return TRUE;

}

//==============================================================================
//
// 	Function Name:	CFileData::GetShellInfo()
//
// 	Description:	This function will retrieve the shell information for this
//					file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			This function will store the index of the icon specified
//					in the call in the m_iIcon member. The handle of the image
//					list containing the icon is stored in m_hImageList.
//
//==============================================================================
BOOL CFileData::GetShellInfo(BOOL bSmallIcon, BOOL bOpenIcon)
{
	SHFILEINFO	FileInfo;
	UINT		uFlags = SHGFI_DISPLAYNAME | SHGFI_SYSICONINDEX | SHGFI_TYPENAME;
	CString		strFileSpec;

	strFileSpec = m_strParent;
	if(strFileSpec.Right(1) != "\\")
		strFileSpec += "\\";
	strFileSpec += m_strFileName;
	
	//	Modify the flags to get the appropriate icon
	if(bSmallIcon)
		uFlags |= SHGFI_SMALLICON;
	else
		uFlags |= SHGFI_LARGEICON;
	
	if(bOpenIcon)
		uFlags |= SHGFI_OPENICON;
		
	m_hImageList = (HIMAGELIST)SHGetFileInfo(strFileSpec, 0, &FileInfo, 
											 sizeof(SHFILEINFO), uFlags);
	if(m_hImageList == NULL)
		return FALSE;

	m_strDisplayName = FileInfo.szDisplayName;
	m_strTypeName = FileInfo.szTypeName;
	m_hIcon = FileInfo.hIcon;
	m_iIcon = FileInfo.iIcon;

	return TRUE;

}

//==============================================================================
//
// 	Function Name:	CFileData::operator < ()
//
// 	Description:	This is an overloaded version of the < operator.
//
// 	Returns:		TRUE if this object is less than the comparison object.
//
//	Notes:			None
//
//==============================================================================
BOOL CFileData::operator < (const CFileData& fdCompare)
{
	int iCompare;

	switch(m_iSortField)
	{
		case FILESORT_EXTENSION:

			if((iCompare = m_strExtension.CompareNoCase(fdCompare.m_strExtension)) != 0)
				return (iCompare < 0);

			//	Drop through if the extensions are equal

		case FILESORT_NAME:
			return (m_strFileName.CompareNoCase(fdCompare.m_strFileName) < 0);

		case FILESORT_SIZE:
			return (m_dwFileSizeLow < fdCompare.m_dwFileSizeLow);

		case FILESORT_DATE:

			return (CompareFileTime(&m_ftLastWrite, &fdCompare.m_ftLastWrite) < 0);

		default:
			return FALSE;

	}

}

//==============================================================================
//
// 	Function Name:	CFileData::operator == ()
//
// 	Description:	This is an overloaded version of the == operator.
//
// 	Returns:		TRUE if the objects are equal.
//
//	Notes:			None
//
//==============================================================================
BOOL CFileData::operator == (const CFileData& fdCompare)
{
	switch(m_iSortField)
	{
		case FILESORT_EXTENSION:
		case FILESORT_NAME:
			return (m_strFileName.CompareNoCase(fdCompare.m_strFileName) == 0);

		case FILESORT_SIZE:
			return (m_dwFileSizeLow == fdCompare.m_dwFileSizeLow);

		case FILESORT_DATE:

			return (CompareFileTime(&m_ftLastWrite, &fdCompare.m_ftLastWrite) == 0);

		default:
			return FALSE;

	}

}

//==============================================================================
//
// 	Function Name:	CFileData::Show()
//
// 	Description:	This is a debugging aid that will use a standard message box
//					to display the value of this object's members.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
#ifdef _DEBUG
void CFileData::Show(HWND hParent)
{
	CString strTemp;
	CString	strMembers = "";
	
	strTemp.Format("Parent = %s\nFilename = %s\n", m_strParent, m_strFileName);
	strMembers += strTemp;
	strTemp.Format("Display Name = %s\n", m_strDisplayName);
	strMembers += strTemp;
	strTemp.Format("Alternate Name = %s\n", m_strAlternate);
	strMembers += strTemp;
	strTemp.Format("Type Name = %s\n", m_strTypeName);
	strMembers += strTemp;
	strTemp.Format("High Size = %ld\nLow Size = %ld\n", m_dwFileSizeHigh, m_dwFileSizeLow);
	strMembers += strTemp;
	strTemp.Format("hImageList = %lu\n", m_hImageList);
	strMembers += strTemp;
	strTemp.Format("hIcon = %lu\niIcon = %d\n", m_hIcon, m_iIcon);
	strMembers += strTemp;
		
	MessageBox(hParent, strMembers, "CFileData", MB_OK);

}
#endif

//==============================================================================
//
// 	Function Name:	CFileDataList::Add()
//
// 	Description:	This function will add the object pointer to the list in the
//					correct position based on the current sort order. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CFileDataList::Add(CFileData* pData)
{
	POSITION	Pos;
	POSITION	Prev;
	CFileData*	pFD;

	//	Add at the head of the list if the list is empty
	if(IsEmpty())
	{
		AddHead(pData);
		return;
	}

	//	Look for the correct position
	Pos = GetHeadPosition();
	Prev = Pos;
	while(Pos != NULL)
	{
		if((pFD = (CFileData*)CObList::GetNext(Pos)) != NULL)
		{
			if(m_bAscending)
			{
				if(*pData < *pFD)
				{	
					InsertBefore(Prev, pData);
					return;
				}
			}
			else
			{
				if(*pFD < *pData)
				{
					InsertBefore(Prev, pData);
					return;
				}

			}

			Prev = Pos;

		}
	}

	//	If we made it this far we must have to add it to the end of the list
	AddTail(pData);

}

//==============================================================================
//
// 	Function Name:	CFileDataList::CFileDataList()
//
// 	Description:	This is the constructor for CFileDataList objects. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CFileDataList::CFileDataList(BOOL bAscending, int iBlockSize) 
			  :CObList(iBlockSize)
{
	//	Initialize the local members
	m_bAscending = bAscending;
	m_NextPos = NULL;
	m_PrevPos = NULL;

}

//==============================================================================
//
// 	Function Name:	CFileDataList::Flush()
//
// 	Description:	This function will flush all CFileData objects from the 
//					array. If bDeleteObj is TRUE, the objects are deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CFileDataList::Flush(BOOL bDeleteAll)
{
	POSITION Pos;

	//	Do we want to delete the objects?
	if(bDeleteAll)
	{
		Pos = GetHeadPosition();

		while(Pos != NULL)
		{
			CFileData* pData = (CFileData*)CObList::GetNext(Pos);
			if(pData != NULL)
				delete pData;
		}

	}

	//	Remove all pointers from the list
	RemoveAll();
	m_NextPos = NULL;
	m_PrevPos = NULL;

}

//==============================================================================
//
// 	Function Name:	CFileDataList::GetFirst()
//
// 	Description:	This function will retrieve the first object in the list.
//
// 	Returns:		A pointer to the first object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CFileData* CFileDataList::GetFirst()
{
	//	Get the first position
	m_NextPos = GetHeadPosition();
	m_PrevPos = NULL;

	if(m_NextPos == NULL)
		return NULL;
	else
		return (CFileData*)CObList::GetNext(m_NextPos);
}

//==============================================================================
//
// 	Function Name:	CFileDataList::GetLast()
//
// 	Description:	This function will retrieve the last object in the list.
//
// 	Returns:		A pointer to the last object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CFileData* CFileDataList::GetLast()
{
	//	Get the last position
	m_PrevPos = GetTailPosition();
	m_NextPos = NULL;

	if(m_PrevPos == NULL)
		return NULL;
	else
		return (CFileData*)CObList::GetPrev(m_PrevPos);
}

//==============================================================================
//
// 	Function Name:	CFileDataList::GetNext()
//
// 	Description:	This function will retrieve the next object in the list.
//
// 	Returns:		A pointer to the next object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CFileData* CFileDataList::GetNext()
{
	if(m_NextPos == NULL)
		return NULL;
	else
	{
		m_PrevPos = m_NextPos;
		CObList::GetPrev(m_PrevPos);
		return (CFileData*)CObList::GetNext(m_NextPos);
	}
}

//==============================================================================
//
// 	Function Name:	CFileDataList::GetPrev()
//
// 	Description:	This function will retrieve the previous object in the list.
//
// 	Returns:		A pointer to the previous object in the list. NULL if not
//					found.
//
//	Notes:			None
//
//==============================================================================
CFileData* CFileDataList::GetPrev()
{
	if(m_PrevPos == NULL)
		return NULL;
	else
	{
		m_NextPos = m_PrevPos;
		CObList::GetNext(m_NextPos);
		return (CFileData*)CObList::GetPrev(m_PrevPos);
	}
}

