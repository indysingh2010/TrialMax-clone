//==============================================================================
//
// File Name:	filectrl.cpp
//
// Description:	This file contains member functions of the CFileCtrl class.
//
// See Also:	filectrl.h, filedata.h, filedata.cpp
//
//==============================================================================
//	Date		Revision    Description
//	07-09-97	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <filectrl.h>

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
BEGIN_MESSAGE_MAP(CFileCtrl, CListCtrl)
	//{{AFX_MSG_MAP(CFileCtrl)
	ON_NOTIFY_REFLECT(LVN_GETDISPINFO, OnGetDisplayInfo)
	ON_NOTIFY_REFLECT(LVN_COLUMNCLICK, OnColumnClick)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CFileCtrl::CFileCtrl()
//
// 	Description:	This is the constructor for CFileCtrl objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CFileCtrl::CFileCtrl()
{
	m_strFolder.Empty();
	m_bMultiColumn = FALSE;
	m_bSmallIcons = FALSE;
	m_bUseFilter = FALSE;
	m_iSortField = FILESORT_NONE;

	//	Allocate the file data lists
	m_pFiles = new CFileDataList();
	m_pSelections = new CFileDataList();

	ASSERT(m_pFiles);
	ASSERT(m_pSelections);
}

//==============================================================================
//
// 	Function Name:	CFileCtrl::~CFileCtrl()
//
// 	Description:	This is the destructor for CFileCtrl objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CFileCtrl::~CFileCtrl()
{
	//	Flush the file data lists
	m_pFiles->Flush(TRUE);
	m_pSelections->Flush(FALSE);

	//	Delete the lists
	delete m_pFiles;
	delete m_pSelections;
}

//==============================================================================
//
// 	Function Name:	CFileCtrl::CheckFilter()
//
// 	Description:	This function will check the extension provided by the 
//					caller to see if it exists in the filter list.
//
// 	Returns:		TRUE if in the list.
//
//	Notes:			None
//
//==============================================================================
BOOL CFileCtrl::CheckFilters(CString& strExtension)
{
	if(!m_bUseFilter)
		return TRUE;

	int nFilters = m_Filters.GetSize();

	for(int i = 0; i < nFilters; i++)
		if(!strExtension.CompareNoCase(m_Filters[i]))
			return TRUE;

	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CFileCtrl::DoComparison()
//
// 	Description:	This function is called by Windows to sort the items in a
//					a column.
//
// 	Returns:		> 0 if LPItem1 is greater than LPItem2
//					= 0 if LPItem1 equals LPItem2
//					< 0 if LPItem1 is less than LPItem2
//
//	Notes:			This function ignores the sort order of the file data 
//					objects and uses the column selected by the user to sort the
//					files.
//
//==============================================================================
int CALLBACK CFileCtrl::DoComparison(LPARAM LPItem1, LPARAM LPItem2, 
									 LPARAM LPColumnIndex)
{
	CFileData*	pData1 = (CFileData*)LPItem1;
	CFileData*  pData2 = (CFileData*)LPItem2;
	int			nReturn;

	switch(LPColumnIndex)
	{

		case FILENAME_COLUMN:

			nReturn = pData1->m_strDisplayName.CompareNoCase(pData2->m_strDisplayName);
			break;

		case SIZE_COLUMN:

			nReturn = pData1->m_dwFileSizeLow - pData2->m_dwFileSizeLow;
			break;

		case DATE_COLUMN:

			nReturn = CompareFileTime(&pData1->m_ftLastWrite, 
									  &pData2->m_ftLastWrite);
			break;

	}

	return nReturn;

}

//==============================================================================
//
// 	Function Name:	CFileCtrl::GetSelections()
//
// 	Description:	This function will fill the selections array with all
//					current selections.
//
// 	Returns:		A reference to the selections array.
//
//	Notes:			None
//
//==============================================================================
CFileDataList& CFileCtrl::GetSelections()
{
	CFileData*	pData;
	LV_ITEM		ItemInfo;
	int			iIndex = -1;


	//	Initialize the item information structure
	memset(&ItemInfo, 0, sizeof(ItemInfo));
	ItemInfo.mask = LVIF_PARAM;

	//	Clear any existing selections in the array. Don't destroy the objects
	//	because they belong to the m_pFiles list. 
	m_pSelections->Flush(FALSE);

	iIndex = GetNextItem(-1, LVNI_ALL | LVIS_SELECTED);
	while(iIndex >= 0)
	{
		//	Get the information for this item
		ItemInfo.iItem = iIndex;
		if(GetItem(&ItemInfo))
		{
			if((pData = (CFileData*)ItemInfo.lParam) != NULL)
				m_pSelections->Add(pData);
		}

		iIndex = GetNextItem(iIndex, LVNI_ALL | LVIS_SELECTED);
	}

	return *m_pSelections;

}

//==============================================================================
//
// 	Function Name:	CFileCtrl::Initialize()
//
// 	Description:	This function should be called from the parent window to
//					initialize the control.
//
// 	Returns:		TRUE if successful.
//
//	Notes:			None
//
//==============================================================================
BOOL CFileCtrl::Initialize(int iSortField, BOOL bAscending) 
{
	RECT		rcWindow;
	SHFILEINFO	FileInfo;
	HIMAGELIST	hImages;

	//	Turn on the wait cursor
	AfxGetApp()->DoWaitCursor(1);

	//	Set the sort options
	SetSortOptions(iSortField, bAscending);

	//	This control uses the system image lists so make sure it doesn't attempt
	//	to destroy them when it closes
	ModifyStyle(0, LVS_SHAREIMAGELISTS);
		
	// Add the filename column always
	GetClientRect(&rcWindow);
	InsertColumn(FILENAME_COLUMN, "Name", LVCFMT_LEFT, rcWindow.right);

	//	Do we want to add the additional columns. 
	if((GetStyle() & LVS_REPORT))
	{
		m_bMultiColumn = TRUE;
		InsertColumn(SIZE_COLUMN, "Size", LVCFMT_RIGHT, 96);
		InsertColumn(DATE_COLUMN, "Modified", LVCFMT_LEFT, 128);
	}

	//	Do we want to use small or large icons?
	//
	//	Note:	It would be nice to just check for the LVS_ICON style but, there
	//			is a bug in the VC resource editor. If Icon view is selected,
	//			no style is placed in the resource string.
	//
	//			Another bug in the VC++ editor appears if you choose the
	//			Always Show Selection option. Once you select it, even if you
	//			deselect it, the LVS_SHOWSELECTALWAYS style appears in the 
	//			resource string.
	if((GetStyle() & LVS_REPORT) || (GetStyle() & LVS_LIST) || 
	   (GetStyle() & LVS_SMALLICON))
	{
		m_bSmallIcons = TRUE;
		hImages = (HIMAGELIST)SHGetFileInfo("", 0, &FileInfo, sizeof(SHFILEINFO), 
											SHGFI_SYSICONINDEX | SHGFI_SMALLICON);
		SendMessage(LVM_SETIMAGELIST, LVSIL_SMALL, (LPARAM)hImages); 
	}
	else
	{
		m_bSmallIcons = FALSE;
		hImages = (HIMAGELIST)SHGetFileInfo("", 0, &FileInfo, sizeof(SHFILEINFO), 
											SHGFI_SYSICONINDEX | SHGFI_LARGEICON);
		SendMessage(LVM_SETIMAGELIST, LVSIL_NORMAL, (LPARAM)hImages); 
	}

	//	Turn off the wait cursor
	AfxGetApp()->DoWaitCursor(-1);

	return TRUE;

}

//==============================================================================
//
// 	Function Name:	CFileCtrl::InsertItem()
//
// 	Description:	This function will add a new file to the list.
//
// 	Returns:		TRUE if successful.
//
//	Notes:			None
//
//==============================================================================
BOOL CFileCtrl::InsertItem(int nIndex, CFileData* pFileData) 
{
	LV_ITEM	lvItem;

	lvItem.mask = LVIF_TEXT | LVIF_IMAGE | LVIF_PARAM;
	lvItem.iItem = nIndex;
	lvItem.iSubItem = 0;
	lvItem.iImage = pFileData->m_iIcon;
	lvItem.pszText = LPSTR_TEXTCALLBACK;
	lvItem.lParam = (LPARAM)pFileData;
	return (CListCtrl::InsertItem(&lvItem) != -1);

}

//==============================================================================
//
// 	Function Name:	CFileCtrl::OnColumnClick()
//
// 	Description:	This function is called when the user clicks on a column
//					header. It will resort the list based on the selected
//					column.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CFileCtrl::OnColumnClick(NMHDR* pNMHDR, LRESULT* pResult) 
{
	NM_LISTVIEW* pNMListView = (NM_LISTVIEW*)pNMHDR;
	
	SortItems(DoComparison, pNMListView->iSubItem);
	
	*pResult = 0;
}

//==============================================================================
//
// 	Function Name:	CFileCtrl::OnGetDisplayInfo()
//
// 	Description:	This function is called when the control needs information
//					to display an item.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CFileCtrl::OnGetDisplayInfo(NMHDR* pNMHDR, LRESULT* pResult) 
{
	LV_DISPINFO* pDisplayInfo = (LV_DISPINFO*)pNMHDR;
	CString		 strText;
	CFileData*	 pData;

	//	Does the control need the text string for a column?
	if(pDisplayInfo->item.mask & LVIF_TEXT)
	{
		pData = (CFileData*)pDisplayInfo->item.lParam;

		//	Which column do we need text for?
		switch(pDisplayInfo->item.iSubItem)
		{
			case FILENAME_COLUMN:

				lstrcpy(pDisplayInfo->item.pszText, pData->m_strDisplayName);
				break;

			case SIZE_COLUMN:

				strText.Format("%u", pData->m_dwFileSizeLow);
				lstrcpy(pDisplayInfo->item.pszText, strText);
				break;


			case DATE_COLUMN:

				CTime Time(pData->m_ftLastWrite);
				BOOL bPm = FALSE;
				int nHour = Time.GetHour();
				
				if(nHour == 0)
					nHour = 12;
				else if(nHour == 12)
					bPm = TRUE;
				else if(nHour > 12)
				{
					nHour -= 12;
					bPm = TRUE;
				}

				strText.Format("%d/%0.2d/%0.2d %d:%0.2d %s",
							   Time.GetMonth(), Time.GetDay(), 
							   Time.GetYear() % 100, nHour, Time.GetMinute(),
							   (bPm) ? "PM" : "AM");
				lstrcpy(pDisplayInfo->item.pszText, strText);
				break;

		}

	}
	
	
	*pResult = 0;
}

//==============================================================================
//
// 	Function Name:	CFileCtrl::Reset()
//
// 	Description:	This function will clear the list and destroy the file data
//					objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CFileCtrl::Reset() 
{
	m_strFolder.Empty();

	//	Delete all the file data objects
	m_pFiles->Flush(TRUE);
	m_pSelections->Flush(FALSE);

}

//==============================================================================
//
// 	Function Name:	CFileCtrl::SetFilters()
//
// 	Description:	This function will set the filter used to display files.
//
// 	Returns:		None
//
//	Notes:			This function DOES NOT update the control. It is up to the
//					caller to perform the update if required.
//
//==============================================================================
void CFileCtrl::SetFilters(CString& strFilterString) 
{
	CString		strFilters = strFilterString;
	CString*	pFilter;
	int			nToken;
	char		szFilter[] = FILTER_DELIMITER;

	//	Empty all the current filters
	m_Filters.RemoveAll();
	m_bUseFilter = FALSE;

	if(strFilters.IsEmpty())
		return;

	//	Replace , or ; with the appropriate delimiter
	strFilters.Replace(',', szFilter[0]);
	strFilters.Replace(';', szFilter[0]);

	//	Parse the filter string
	nToken = strFilters.Find(FILTER_DELIMITER);
	while(nToken > 0)
	{
		//	Allocate and add the new filter
		pFilter = new CString(strFilters.Left(nToken));
		m_Filters.Add(*pFilter);

		//	Check for wild card
		if(!pFilter->CompareNoCase("*"))
			return;

		//	Adjust the filter string
		strFilters = strFilters.Right(strFilters.GetLength() - (nToken + 1));

		//	Look for the next filter
		nToken = strFilters.Find(FILTER_DELIMITER);
	}

	//	Check for a trailing filter
	if(strFilters.GetLength() > 0)
	{
		pFilter = new CString(strFilters);
		m_Filters.Add(*pFilter); 
		
		//	Check for wild card
		if(!pFilter->CompareNoCase("*"))
			return;

	}
			
	if(m_Filters.GetSize() > 0)
		m_bUseFilter = TRUE;

}

//==============================================================================
//
// 	Function Name:	CFileCtrl::SetSortOptions()
//
// 	Description:	This function will set the options used to sort the file
//					data array.
//
// 	Returns:		None
//
//	Notes:			This function DOES NOT update the control. It is up to the
//					caller to perform the update if required.
//
//==============================================================================
void CFileCtrl::SetSortOptions(int iSortField, BOOL bAscending) 
{
	m_iSortField = iSortField;
	m_pFiles->m_bAscending = bAscending;
	m_pSelections->m_bAscending = bAscending;
}

//==============================================================================
//
// 	Function Name:	CFileCtrl::Update()
//
// 	Description:	This function is called to update the control contents.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CFileCtrl::Update(CString& strFolder) 
{
	WIN32_FIND_DATA	FindData;
	CString			strSearch;
	CFileData*		pData;
	HANDLE			hFind;
	int				nIndex = 0;

	//	Reset the control
	Reset();

	//	Save the folder and build the new search specification
	m_strFolder = strFolder;
	strSearch = m_strFolder;
	if(strSearch.Right(1) != "\\")
		strSearch += "\\";
	strSearch += "*.*";

	//	Are there any files?
	if((hFind = FindFirstFile(strSearch, &FindData)) == INVALID_HANDLE_VALUE)
		return;	
	
	//	Turn on the wait cursor
	AfxGetApp()->DoWaitCursor(1);

	//	Add all the files
	while(1)
	{
		//	Is this a folder?
		if(FindData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)
		{
			if(lstrcmpi(FindData.cFileName, ".") && 
			   lstrcmpi(FindData.cFileName, ".."))
			{
			
				//	Insert folder handling code here
			}
				
		}
		else
		{	
			//	Allocate a new file data object
			try
			{
				pData = new CFileData(m_strFolder, &FindData, m_iSortField);

				if(CheckFilters(pData->m_strExtension))
				{
					//	Add the file to the list
					m_pFiles->Add(pData);
					
					//	Get the display information for this file
					pData->GetShellInfo(m_bSmallIcons);
				}
				else
				{
					delete pData;
				}


			}
			catch(CMemoryException* e)
			{
				e->Delete();
	
				//	Turn off the wait cursor
				AfxGetApp()->DoWaitCursor(-1);

				return;
			}

		}

		//	Get the next file
		if(!FindNextFile(hFind, &FindData))
			break;

	} // while(1)

	//	Clear the list control and add all the new items
	DeleteAllItems();
	pData = m_pFiles->GetFirst();
	while(pData)
	{
		InsertItem(nIndex++, pData);
		pData = m_pFiles->GetNext();
	}

	FindClose(hFind);
	
	//	Turn off the wait cursor
	AfxGetApp()->DoWaitCursor(-1);

}
	
