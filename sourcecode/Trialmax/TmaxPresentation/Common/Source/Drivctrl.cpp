//==============================================================================
//
// File Name:	drivctrl.cpp
//
// Description:	This file contains member functions of the CDriveCtrl class.
//
// See Also:	drivctrl.h, filedata.h
//
//==============================================================================
//	Date		Revision    Description
//	07-09-97	1.00		Original Release
//	08-17-97	1.01		Removed DeleteFirstChild()
//	08-17-97	1.01		Added SetSelection() and GetItem()
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <drivctrl.h>
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
BEGIN_MESSAGE_MAP(CDriveCtrl, CTreeCtrl)
	//{{AFX_MSG_MAP(CDriveCtrl)
	ON_NOTIFY_REFLECT(TVN_ITEMEXPANDING, OnItemExpanding)
	ON_NOTIFY_REFLECT_EX(TVN_SELCHANGED, OnSelChanged)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CDriveCtrl::AddDrive()
//
// 	Description:	This function will add a drive specification to the tree.
//
// 	Returns:		TRUE if successful
//
//	Notes:			We cheat a little bit here for the sake of efficiency. If
//					the drive is a floppy drive or CD drive we just assume 
//					there's a disk in the drive and that it has subfolders.
//
//					The reason for this is to eliminate the need to check the
//					drive to see if media is present and then check for
//					subfolders. If we check the drive and no media is present
//					it will generate an error unless we turn the errors off
//					with SetErrorMode(SEM_FAILCRITICALERRORS).
//
//==============================================================================
BOOL CDriveCtrl::AddDrive(CString& strDrive) 
{
	HTREEITEM	hItem;
	CFileData	FileData(strDrive);
	int			iIcon;

	//	The first thing we want to do is get the display string for this drive
	FileData.GetShellInfo(TRUE);
	iIcon = FileData.m_iIcon;

	//	Get the drive type identifier
	UINT uType = ::GetDriveType((LPCTSTR)strDrive);

	//	Use the image appropriate for this drive type
	switch(uType)
	{
		//	Floppy?
		case DRIVE_REMOVABLE:

			hItem = InsertItem(FileData.m_strDisplayName, iIcon, iIcon); 
			
			//	We don't want to waste time checking floppy drives to see if
			//	folders exists so we just assume there is one
			InsertItem("", 0, 0, hItem);
			break;

		//	Hard drive?
		case DRIVE_FIXED:

			hItem = InsertItem(FileData.m_strDisplayName, iIcon, iIcon);
			SetButtonState(hItem, strDrive);

			//	If this is the first hard disk, select and expand it
			if(m_bFirstHardDisk)
			{
				SelectItem(hItem);
				Expand(hItem, TVE_EXPAND);
				m_bFirstHardDisk = FALSE;
			}
			break;

		//	Network drive?
		case DRIVE_REMOTE:

			hItem = InsertItem(FileData.m_strDisplayName, iIcon, iIcon);
			SetButtonState(hItem, strDrive);
			break;

		//	CDROM drive?
		case DRIVE_CDROM:

			hItem = InsertItem(FileData.m_strDisplayName, iIcon, iIcon);
			
			//	Assume there's a folder 
			InsertItem("", 0, 0, hItem);
			break;

		//	RAM disk?
		case DRIVE_RAMDISK:

			hItem = InsertItem(FileData.m_strDisplayName, iIcon, iIcon);
			SetButtonState(hItem, strDrive);
			break;

		default:

			return FALSE;

	}

	return TRUE;

}

//==============================================================================
//
// 	Function Name:	CDriveCtrl::AddFolders()
//
// 	Description:	This function will build the list of subfolders for the 
//					folder provided by the caller.
//
// 	Returns:		The number of folders added.
//
//	Notes:			None
//
//==============================================================================
int CDriveCtrl::AddFolders(HTREEITEM hItem, CString& strPath) 
{
	CFileDataList*  pFDList;
	CFileData*		pFileData;
	HANDLE			hFind;
	WIN32_FIND_DATA	FindData;
	HTREEITEM		hNewItem;
	CString			strSearch;
	int				nFolders = 0;
	int				iClosed;
	int				iOpen;
	
	//	Set up the search specification
	strSearch = strPath;
	if(strSearch.Right(1) != "\\")
		strSearch += "\\";
	strSearch += "*.*";
	
	hFind = FindFirstFile(strSearch, &FindData);
	if(hFind == INVALID_HANDLE_VALUE)
	{
		if(GetParentItem(hItem) == NULL)
			InsertItem("", 0, 0, hItem);
		return 0;
	}

	//	Allocate a new file data list
	pFDList = new CFileDataList(m_bAscending);
	ASSERT(pFDList);

	//	Locate the folders. Ignore . and .. specifiers
	while(1)
	{
		if(FindData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)
		{
			CString strFileName = (LPCTSTR) &FindData.cFileName;
			if((strFileName != ".") && (strFileName != ".."))
			{
				//	Allocate a file data object for this folder and add it
				//	to the list
				pFileData = new CFileData(strPath, &FindData, m_iSortField);
				pFDList->Add(pFileData);
				nFolders++;
			}

		}

		//	Check the next one
		if(!FindNextFile(hFind, &FindData))
			break;

	}

	//	Add each folder to the tree
	pFileData = pFDList->GetFirst();
	while(pFileData)
	{
		pFileData->GetShellInfo(TRUE);
		iClosed = pFileData->m_iIcon;
		pFileData->GetIconIndex(TRUE, TRUE);
		iOpen = pFileData->m_iIcon;

		hNewItem = InsertItem(pFileData->m_strDisplayName, iClosed, 
							  iOpen, hItem);

		CString strNewPath = strPath;
		if(strNewPath.Right(1) != "\\")
			strNewPath += "\\";
		strNewPath += pFileData->m_strFileName;
		SetButtonState(hNewItem, strNewPath);

		pFileData = pFDList->GetNext();
	}

	//	Destroy the list and its objects
	pFDList->Flush(TRUE);
	delete pFDList;
				
	::FindClose(hFind);
	return nFolders;

}

//==============================================================================
//
// 	Function Name:	CDriveCtrl::CDriveCtrl()
//
// 	Description:	This is the constructor for CDriveCtrl objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CDriveCtrl::CDriveCtrl()
{
	m_bFirstHardDisk = TRUE;
	m_bAscending = TRUE;
	m_iSortField = FILESORT_NONE;
	m_strSelection.Empty();
	m_bSuppress = FALSE;
}
	
//==============================================================================
//
// 	Function Name:	CDriveCtrl::CollapseAll()
//
// 	Description:	This function will collapse all folders.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDriveCtrl::CollapseAll() 
{
	HTREEITEM hItem = GetRootItem();

	while(hItem != NULL)
	{
		Expand(hItem, TVE_COLLAPSE);
		hItem = GetNextSiblingItem(hItem);
	}

}

//==============================================================================
//
// 	Function Name:	CDriveCtrl::DeleteAllChildren()
//
// 	Description:	This function will remove all children of the parent item
//					provided by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDriveCtrl::DeleteAllChildren(HTREEITEM hParent) 
{
	HTREEITEM hItem;
	if((hItem = GetChildItem(hParent)) == NULL)
		return;


	//	Suppress selection change notifications while we delete all the 
	//	children
	m_bSuppress = TRUE;

	while(1)
	{
		HTREEITEM hNextItem = GetNextSiblingItem(hItem);
		
		//	Reenable notifications if this is the last item
		if(hNextItem == NULL)
			m_bSuppress = FALSE;

		DeleteItem(hItem);
		hItem = hNextItem;

		if(hItem == NULL)
			return;

	}

}

//==============================================================================
//
// 	Function Name:	CDriveCtrl::GetDriveFromLabel()
//
// 	Description:	This function will use the drive label provided by the 
//					caller to build a drive specification. The drive 
//					specification is placed in the string provided by the caller
//
// 	Returns:		The string reference provided by the caller.
//
//	Notes:			None
//
//==============================================================================
CString& CDriveCtrl::GetDriveFromLabel(CString& strLabel) 
{
	int	nIndex = strLabel.ReverseFind(':');

	if(nIndex > 0)
		strLabel.Format("%c:\\", strLabel.GetAt(nIndex - 1));

	return strLabel; 
}
		
//==============================================================================
//
// 	Function Name:	CDriveCtrl::GetItem()
//
// 	Description:	This function will get the item handle for the drive/folder 
//					specified by the caller.
//
// 	Returns:		A handle to the item. NULL if not found.
//
//	Notes:			None
//
//==============================================================================
HTREEITEM CDriveCtrl::GetItem(HTREEITEM hParent, LPCSTR lpPath) 
{
	CFileData	FileData(lpPath);
	HTREEITEM	hItem;
	CString		strLabel;

	//	If no parent is provided, start with the root item. Otherwise, start
	//	with the first child
	if(hParent == NULL)
		hItem = GetRootItem();
	else
		hItem = GetChildItem(hParent);

	//	Get the display string for this drive
	FileData.GetShellInfo(TRUE);

	//	Check all children until we find the correct one
	while(hItem != NULL)
	{
		//	Is this the right item?
		strLabel = GetItemText(hItem);
	
		//	Is this the correct label?
		if(strLabel == FileData.m_strDisplayName)
			return hItem;

		//	Get the parent's next child
		hItem = GetNextSiblingItem(hItem);
	
	}

	return NULL;
}
		
//==============================================================================
//
// 	Function Name:	CDriveCtrl::GetPathFromItem()
//
// 	Description:	This function will get the path associated with the tree 
//					list item provided by the caller.
//
// 	Returns:		The item path
//
//	Notes:			None
//
//==============================================================================
CString CDriveCtrl::GetPathFromItem(HTREEITEM hItem) 
{
	HTREEITEM	hParent;
	CString		strResult;

	//	Get the text for this item
	strResult = GetItemText(hItem);
	
	//	Is this a drive label?
	if(strResult.Right(2) == ":)")
		GetDriveFromLabel(strResult);
	else
		//	Traverse backwards to assemble the path specification
		while((hParent = GetParentItem(hItem)) != NULL)
		{
			CString strParent = GetItemText(hParent);
		
			//	Is this a drive label?
			if(strParent.Right(2) == ":)")
				GetDriveFromLabel(strParent);
		
			if(strParent.Right(1) != "\\")
				strParent += "\\";
			strResult = strParent + strResult;
			hItem = hParent;
		}
	
	return strResult; 
}
		
//==============================================================================
//
// 	Function Name:	CDriveCtrl::GetSelection()
//
// 	Description:	This is a public member that can be called to get the
//					full path specification of the current selection.
//
// 	Returns:		The path specification
//
//	Notes:			None
//
//==============================================================================
LPCSTR CDriveCtrl::GetSelection() 
{
	return m_strSelection;
}

//==============================================================================
//
// 	Function Name:	CDriveCtrl::Initialize()
//
// 	Description:	This function initializes the tree by locating all drives.
//
// 	Returns:		The number of drives
//
//	Notes:			None
//
//==============================================================================
int CDriveCtrl::Initialize(int iSortField, BOOL bAscending) 
{
	CString		strSpec = ".";
	CFileData	FileData(strSpec);
	int			nPosition = 0;
	int			nDrivesAdded = 0;
	CString		strDrive = "?:\\";

	//	Display the wait cursor
	AfxGetApp()->DoWaitCursor(1);

	//	Set the sort options
	SetSortOptions(iSortField, bAscending);

	//	Get a handle to the system's small icon image list
	FileData.GetShellInfo(TRUE);

	//	Assign the list to this control
	SendMessage(TVM_SETIMAGELIST, TVSIL_NORMAL, (LPARAM)FileData.m_hImageList);

	//	Get the list of drives
	DWORD dwDriveList = ::GetLogicalDrives();

	//	Parse the drive list
	while(dwDriveList)
	{
		if(dwDriveList & 1)
		{
			strDrive.SetAt(0, 0x41 + nPosition);// Convert position to character
			if(AddDrive(strDrive))
				nDrivesAdded++;
		}

		dwDriveList >>= 1;
		nPosition++;
	}

	//	Turn off the wait cursor
	AfxGetApp()->DoWaitCursor(-1);

	return nDrivesAdded;

}

//==============================================================================
//
// 	Function Name:	CDriveCtrl::OnItemExpanding()
//
// 	Description:	This function will expand the folder selected by the user.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
void CDriveCtrl::OnItemExpanding(NMHDR* pNMHDR, LRESULT* pResult) 
{
	NM_TREEVIEW*	pNMTreeView = (NM_TREEVIEW*)pNMHDR;
	HTREEITEM		hItem = pNMTreeView->itemNew.hItem;
	CString			strPath = GetPathFromItem(hItem);

	*pResult = FALSE;

	//	Do we need to expand?
	if(pNMTreeView->action == TVE_EXPAND)
	{
		//	Display the wait cursor
		AfxGetApp()->DoWaitCursor(1);

		//	Get rid of the dummy placeholder
		DeleteAllChildren(hItem);

		if(AddFolders(hItem, strPath) == 0)
			*pResult = TRUE;
		
		//	Turn off the wait cursor
		AfxGetApp()->DoWaitCursor(-1);

	}
	else	//	We must be collapsing the folder
	{
		DeleteAllChildren(hItem);
		if(GetParentItem(hItem) == NULL)
			InsertItem("",0, 0, hItem);
		else
			SetButtonState(hItem, strPath);
	}				

}

//==============================================================================
//
// 	Function Name:	CDriveCtrl::OnSelChanged()
//
// 	Description:	This function is called when the user makes a new selection.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CDriveCtrl::OnSelChanged(NMHDR* pNMHDR, LRESULT* pResult) 
{
	NM_TREEVIEW* pNMTreeView = (NM_TREEVIEW*)pNMHDR;
	
	//	Don't process the notification if we're suppressing
	if(m_bSuppress)
	{
		*pResult = 0;
		return TRUE;
	}

	//	Store the current selection
	m_strSelection = GetPathFromItem(pNMTreeView->itemNew.hItem);
	
	//	Call virtual function to finish the response
	OnSelectionChanged(m_strSelection);
	
	//	Propagate the message to the parent window
	*pResult = 0;
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CDriveCtrl::OnSelectionChanged()
//
// 	Description:	This is a helper function to support derived classes. 
//					Override this function to provide custom handling of changed
//					selections. 
//
// 	Returns:		None
//
//	Notes:			By default, the TVN_SELCHANGED message is forwarded to the
//					parent window. If you trap the message at the parent you 
//					can add custom handling there.
//	
//					If you derive a class from CDriveCtrl you can overload this
//					function to add custom handling.
//
//==============================================================================
void CDriveCtrl::OnSelectionChanged(CString& strPath) 
{
	return;
}

//==============================================================================
//
// 	Function Name:	CDriveCtrl::SetButtonState()
//
// 	Description:	For improved performance, subfolders are not added to the
//					tree until the parent folder is expanded. But, we want a plus
//					sign to appear beside the parent if there are any children. 
//					This function will check to see if any subfolders exist and
//					insert a dummy if at least one exists.
//
// 	Returns:		TRUE if at least one exists
//
//	Notes:			None
//
//==============================================================================
BOOL CDriveCtrl::SetButtonState(HTREEITEM hItem, CString& strPath) 
{
	HANDLE			hFind;
	WIN32_FIND_DATA	FindData;
	BOOL			bResult = FALSE;
	
	//	Set up the search specification
	CString	strTemp = strPath;
	if(strTemp.Right(1) != "\\")
		strTemp += "\\";
	strTemp += "*.*";

	//	Is there anything in the folder?
	if((hFind = FindFirstFile(strTemp, &FindData)) == INVALID_HANDLE_VALUE)
		return FALSE;

	//	Locate the folders. Ignore . and .. specifiers
	while(1)
	{
		if(FindData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)
		{
			if(lstrcmp(FindData.cFileName, ".") && 
			   lstrcmp(FindData.cFileName, ".."))
			{
				InsertItem("", 0, 0, hItem);
				bResult = TRUE;
				break;
			}

		}

		//	Check the next one
		if(!FindNextFile(hFind, &FindData))
			break;

	}

	FindClose(hFind);
	return bResult;

}

//==============================================================================
//
// 	Function Name:	CDriveCtrl::SetSelection()
//
// 	Description:	This function will set the current selection to the folder 
//					provided by the caller.
//
// 	Returns:		None
//
//	Notes:			A full path specification (drive:\folder\...\folder) must
//					be provided.
//					
//
//==============================================================================
void CDriveCtrl::SetSelection(LPCSTR lpPath, BOOL bCollapseAll) 
{
	HTREEITEM	hItem = NULL;
	HTREEITEM	hLastFound = NULL;
	HGLOBAL		hBuffer = NULL;
	LPSTR		lpBuffer = NULL;
	CString		strPath;
	char*		pToken;
	char*		pNext;
	int			nLength;

	//	Is the buffer valid?
	if(!lpPath || (nLength = strlen(lpPath)) == 0)
		return;
	else
		strPath.Empty();

	//	If requested, collapse all branches
	if(bCollapseAll)
		CollapseAll();

	//	Set up the working buffer. This is done so that we don't destroy the
	//	path contained in the caller's buffer when we tokenize it
	if((hBuffer = GlobalAlloc(GHND, nLength)) != NULL)
		lpBuffer = (LPSTR)GlobalLock(hBuffer);
	if(lpBuffer == NULL)
		return;
	else
		lstrcpy(lpBuffer, lpPath);
	
	//	Separate the drive specification
	pToken = strtok_s(lpBuffer, "/\\", &pNext);
	
	//	Build the search path one folder at a time and expand each folder as
	//	we traverse the path
	while(pToken != NULL)
	{
		//	Add to the search path
		strPath += (LPSTR)pToken;

		//	Save a handle to the last valid item
		hLastFound = hItem;

		//	Get the item for this portion of the path
		if((hItem = GetItem(hItem, strPath)) == NULL)
			break;
		
		//	Expand this folder so we can check its children
		if(!(GetItemState(hItem, TVIS_EXPANDED) & TVIS_EXPANDED))
		{	
			//	We have to clear this flag first in case the folder has been
			//	expanded and collapsed prior to calling this function
			SetItemState(hItem, 0, TVIS_EXPANDEDONCE);

			//	Expand this branch of the tree
			Expand(hItem, TVE_EXPAND);
		}

		//	Add the delimiter for the next folder
		strPath += "\\";

		//	Parse out the next folder
		pToken = strtok_s(NULL, "/\\", &pNext);
	}

	//	Make this item the current selection if we found the full path and it
	//	is not already selected. Otherwise just make sure it's visible
	if(hItem != NULL)
	{ 
		if(GetSelectedItem() != hItem)
			Select(hItem, TVGN_CARET);
		EnsureVisible(hItem);
	}
	else
	{ 
		//	Make sure the last valid item is visible
		if(hLastFound != NULL)
			EnsureVisible(hLastFound);
	}

	//	Deallocate the working buffer
	GlobalUnlock(hBuffer);
	GlobalFree(hBuffer);

	return;
}	

//==============================================================================
//
// 	Function Name:	CDriveCtrl::SetSortOptions()
//
// 	Description:	This function allows the caller to set the options used by
//					the control to sort folders.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CDriveCtrl::SetSortOptions(int iSortField, BOOL bAscending) 
{
	m_iSortField = iSortField;
	m_bAscending = bAscending;
}






