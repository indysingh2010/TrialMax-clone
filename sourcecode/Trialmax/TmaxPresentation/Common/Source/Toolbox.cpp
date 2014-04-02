//==============================================================================
//
// File Name:	toolbox.cpp
//
// Description:	This file contains member functions of the CTMToolbox class
//
// Copyright FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	09-18-04	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <toolbox.h>
#include <direct.h>		// getcwd()
#include <tlhelp32.h>

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
//
// 	Function Name:	EnumTaskbarWindows
//
//	Parameters:		hWnd - handle to window being enumerated by the system
//					lParam - application supplied enumeration parameter
//
// 	Return Value:	TRUE to continue the enumeration
//
// 	Description:	This is the enumeration callback invoked when enumerating
//					the windows in the system taskbar thread
//
//------------------------------------------------------------------------------
BOOL CALLBACK EnumTaskbarWindows(HWND hWnd, LPARAM lParam)
{
	char szLabel[512];

	if(IsWindow(hWnd))
	{
		//	Get the text label for this window
		if(GetWindowText(hWnd, szLabel, sizeof(szLabel) - 1) > 0)
		{
			if(lstrcmpi(szLabel, "Start") == 0)
			{
				if(lParam != 0)
				{
					*((HWND*)lParam) = hWnd;
					return FALSE; // STOP THE ENUMERATION
				}
			}
		
		}// if(GetWindowText(hWnd, szLabel, sizeof(szLabel) - 1) > 0)

	}// if(IsWindow(hWnd))

	return TRUE;
};

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMToolbox::CTMToolbox()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Constructor
//
//------------------------------------------------------------------------------
CTMToolbox::CTMToolbox()
{
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMToolbox::~CTMToolbox()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Destructor
//
//------------------------------------------------------------------------------
CTMToolbox::~CTMToolbox()
{
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMToolbox::FindAllFiles()
//
//	Parameters:		lpszFolder - the fully qualified path to the parent folder
//					lpszExtension - the file extension to search for
//					bFullPath - TRUE to return the fully qualified paths
//
// 	Return Value:	The collection of files with the specified extension
//
// 	Description:	This function is called to locate all files in the 
//					specified folder with the specified extension
//
//------------------------------------------------------------------------------
CStringArray* CTMToolbox::FindAllFiles(LPCSTR lpszFolder, LPCSTR lpszExtension, BOOL bFullPath)
{
	WIN32_FIND_DATA	Find;
	HANDLE			hFind;
	CString			strSearchFor = "";
	CString			strFileSpec = "";
	char			szFolder[MAX_PATH];
	char*			pToken = NULL;
	CStringArray*	pAllFiles = NULL;

	//	Get the path to use for the search operation
	strSearchFor = GetSearchPath(lpszFolder, lpszExtension);

	//	Find the first file that meets the search criteria
	if((hFind = ::FindFirstFile(strSearchFor, &Find)) == INVALID_HANDLE_VALUE)
		return NULL;

	//	Do we want the full path?
	if(bFullPath == TRUE)
	{
		//	Extract the folder
		//
		//	NOTE:	We can't user the caller's folder because it may have
		//			changed with the call to GetSearchPath()
		lstrcpyn(szFolder, strSearchFor, sizeof(szFolder));
		if((pToken = strrchr(szFolder, '\\')) != NULL)
			*(pToken + 1) = '\0';
	}
	else
	{
		memset(szFolder, 0, sizeof(szFolder));
	}

	//	Allocate the array and add the first file
	strFileSpec.Format("%s%s", szFolder, Find.cFileName);
	pAllFiles = new CStringArray();
	pAllFiles->Add(strFileSpec);

	//	Search for the rest of the files
	while(FindNextFile(hFind, &Find))
	{
		strFileSpec.Format("%s%s", szFolder, Find.cFileName);
		pAllFiles->Add(strFileSpec);
	}

	FindClose(hFind);
	
	return pAllFiles;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMToolbox::FindFile()
//
//	Parameters:		lpszFileSpec - the fully qualified path to the file
//
// 	Return Value:	TRUE if the file exists
//
// 	Description:	This function is called to determine if the specified file
//					exists
//
//------------------------------------------------------------------------------
BOOL CTMToolbox::FindFile(LPCSTR lpszFilename)
{
	WIN32_FIND_DATA	Find;
	HANDLE			hFind;

	ASSERT(lpszFilename);

	if((hFind = ::FindFirstFile(lpszFilename, &Find)) == INVALID_HANDLE_VALUE)
	{
		return FALSE;
	}
	else
	{
		FindClose(hFind);
		return TRUE;
	}		

}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMToolbox::FindFirstFile()
//
//	Parameters:		lpszFolder - the fully qualified path to the parent folder
//					lpszExtension - the file extension to search for
//					bFullPath - TRUE to return the fully qualified path
//
// 	Return Value:	The first file with the specified extension
//
// 	Description:	This function is called to locate the first file in the 
//					specified folder with the specified extension
//
//------------------------------------------------------------------------------
CString CTMToolbox::FindFirstFile(LPCSTR lpszFolder, LPCSTR lpszExtension, BOOL bFullPath)
{
	WIN32_FIND_DATA	Find;
	HANDLE			hFind;
	CString			strSearchFor = "";
	CString			strFileSpec = "";
	char			szFolder[MAX_PATH];
	char*			pToken = NULL;

	//	Get the path to use for the search operation
	strSearchFor = GetSearchPath(lpszFolder, lpszExtension);

	//	Search for the first file that meets the criteria
	if((hFind = ::FindFirstFile(strSearchFor, &Find)) != INVALID_HANDLE_VALUE)
	{
		//	Do we want the full path?
		if(bFullPath == TRUE)
		{
			//	Extract the folder
			//
			//	NOTE:	We can't user the caller's folder because it may have
			//			changed with the call to GetSearchPath()
			lstrcpyn(szFolder, strSearchFor, sizeof(szFolder));
			if((pToken = strrchr(szFolder, '\\')) != NULL)
				*(pToken + 1) = '\0';

			strFileSpec.Format("%s%s", szFolder, Find.cFileName);
		}
		else
		{
			strFileSpec = Find.cFileName;
		}

		FindClose(hFind);
	}
	
	return strFileSpec;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMToolbox::GetLongPath()
//
//	Parameters:		lpszPath - the path to be parsed
//					rLongPath  - the buffer in which to store the long path
//
// 	Return Value:	True if able to perform the conversion
//
// 	Description:	This function will convert the 8.3 path to a fully qualified
//					long path.
//
//------------------------------------------------------------------------------
BOOL CTMToolbox::GetLongPath(LPCSTR lpszPath, CString& rLongPath)
{
	WIN32_FIND_DATA	fd;
	HANDLE			hFile;
	char*			pToken;
	char			szBuffer[_MAX_PATH];
	CString			strLongFilename;
	CString			strPrevious;

	lstrcpyn(szBuffer, lpszPath, sizeof(szBuffer));

	while(1)
	{
		if((pToken = strrchr(szBuffer, '\\')) != 0)
		{
			if((hFile = ::FindFirstFile(szBuffer, &fd)) != INVALID_HANDLE_VALUE)
			{
				//	Now substitute the caller's filename for the name returned by 
				//	the system
				if(strLongFilename.GetLength() > 0)
				{
					strPrevious = strLongFilename;
					strLongFilename.Format("%s\\%s", fd.cFileName, strPrevious);
				}
				else
				{
					strLongFilename = fd.cFileName;
				}

				FindClose(hFile);
				
			}
			else
			{
				//	Use the tokenized portion if not found
				//
				//	NOTE: This should only happen if the path doesn't exist
				if(strLongFilename.GetLength() > 0)
				{
					strPrevious = strLongFilename;
					strLongFilename.Format("%s\\%s", (pToken + 1), strPrevious);
				}
				else
				{
					strLongFilename = (pToken + 1);
				}

			}
		
			//	Chop off this level
			*pToken = '\0';
		
		}
		else
		{
			//	We should be left with the UNC server or the drive specification
			if(lstrlen(szBuffer) > 0)
			{
				if(strLongFilename.GetLength() > 0)
				{
					strPrevious = strLongFilename;
					strLongFilename.Format("%s\\%s", szBuffer, strPrevious);
				}
				else
				{
					strLongFilename = szBuffer;
				}
			}

			break;
		}

	}// while(1)

	if(strLongFilename.GetLength() > 0)
		rLongPath = strLongFilename;
	else
		rLongPath = lpszPath;

	return TRUE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMToolbox::GetName()
//
//	Parameters:		lpszPath - the path to be parsed
//					rParent  - the buffer in which to store the name
//
// 	Return Value:	True if a valid name is extracted from the path
//
// 	Description:	This function will extract the name of the file or subfolder
//					from the path specified by the caller
//
//------------------------------------------------------------------------------
BOOL CTMToolbox::GetName(LPCSTR lpszPath, CString& rName)
{
	CString strParent;
	
	ASSERT(lpszPath != 0);
	if(lpszPath == 0) return FALSE;
	ASSERT(lstrlen(lpszPath) > 0);
	if(lstrlen(lpszPath) == 0) return FALSE;

	if(SplitPath(lpszPath, strParent, rName) == FALSE)
		return FALSE;
	else
		return (rName.GetLength() > 0);		
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMToolbox::GetParent()
//
//	Parameters:		lpszPath - the path to be parsed
//					rParent  - the buffer in which to store the parent path
//
// 	Return Value:	True if parent is located
//
// 	Description:	This function will extract the parent from the specified
//					path
//
//------------------------------------------------------------------------------
BOOL CTMToolbox::GetParent(LPCSTR lpszPath, CString& rParent)
{
	CString strName;
	
	ASSERT(lpszPath != 0);
	if(lpszPath == 0) return FALSE;
	ASSERT(lstrlen(lpszPath) > 0);
	if(lstrlen(lpszPath) == 0) return FALSE;

	if(SplitPath(lpszPath, rParent, strName) == FALSE)
		return FALSE;
	else
		return (rParent.GetLength() > 0);
		
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMToolbox::GetSearchPath()
//
//	Parameters:		lpszFolder - the fully qualified path to the parent folder
//					lpszExtension - the file extension to search for
//
// 	Return Value:	The path used to perform the file search
//
// 	Description:	This function is called to construct the path to be
//					submitted when searching for files in the folder specified
//					by the caller
//
//------------------------------------------------------------------------------
CString CTMToolbox::GetSearchPath(LPCSTR lpszFolder, LPCSTR lpszExtension)
{
	char			szDirectory[MAX_PATH];
	CString			strSearchFor = "";

	//	Did the caller provide a folder?
	if((lpszFolder != NULL) && (lstrlen(lpszFolder) > 0))
	{
		strSearchFor = lpszFolder;
	}
	else
	{
		//	Use the current working directory
		_getcwd(szDirectory, sizeof(szDirectory));
		strSearchFor = szDirectory;
	}

	if(strSearchFor.Right(1) != "\\")
		strSearchFor += "\\";

	//	Did the caller provide an extension?
	if((lpszExtension != NULL) && (lstrlen(lpszExtension) > 0))
	{
		if(lpszExtension[0] != '.')
			strSearchFor += "*.";
		else
			strSearchFor += "*";

		strSearchFor += lpszExtension;
	}
	else
	{
		strSearchFor += "*.*";
	}

	return strSearchFor;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMToolbox::GetVistaStartMenuWnd()
//
//	Parameters:		hwndTaskBar - the handle to the task bar window
//
// 	Return Value:	The handle to the start button
//
// 	Description:	This function is called to get the handle to the start
//					menu window
//
//------------------------------------------------------------------------------
HWND CTMToolbox::GetVistaStartMenuWnd(HWND hwndTaskBar)
{
	HWND			hwndStartMenu = NULL;
	DWORD			dwTaskBarProcessId = 0;
	HANDLE			hSnapThread = NULL;
	THREADENTRY32	threadEntry;

	if(IsWindow(hwndTaskBar))
	{
		//	Get the id of the process that owns the task bar
		GetWindowThreadProcessId(hwndTaskBar, &dwTaskBarProcessId);
		
		hSnapThread = CreateToolhelp32Snapshot(TH32CS_SNAPTHREAD, 0);
		if(hSnapThread != INVALID_HANDLE_VALUE) 
		{
			threadEntry.dwSize = sizeof(threadEntry);
 
			//	Iterate all the threads
			if(Thread32First(hSnapThread, &threadEntry)) 
			{
				do 
				{	//	Do we have a valid owner process id?
					if(threadEntry.dwSize >= FIELD_OFFSET(THREADENTRY32, th32OwnerProcessID) +
								             sizeof(threadEntry.th32OwnerProcessID)) 
					{
						//	Is this the process we're looking for?
						if(threadEntry.th32OwnerProcessID == dwTaskBarProcessId)
						{
							EnumThreadWindows(threadEntry.th32ThreadID, EnumTaskbarWindows, (LPARAM)(&hwndStartMenu));
						}

					}
				 
					threadEntry.dwSize = sizeof(threadEntry);
				
				}while(Thread32Next(hSnapThread, &threadEntry) && (hwndStartMenu == NULL));
			
			}// if(Thread32First(hSnapThread, &threadEntry)) 
		
			CloseHandle(hSnapThread);
		
		}// if(hSnapThread != INVALID_HANDLE_VALUE)

	}// if(IsWindow(hwndTaskBar))

	return hwndStartMenu;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMToolbox::SetTaskBarVisible()
//
//	Parameters:		bVisible - TRUE if visible / FALSE if hidden
//
// 	Return Value:	True if successful
//
// 	Description:	This function will set the visibility of the system task bar
//
//------------------------------------------------------------------------------
BOOL CTMToolbox::SetTaskBarVisible(BOOL bVisible)
{
	CRect	rectWorkArea = CRect(0,0,0,0);
	CRect	rectTaskBar  = CRect(0,0,0,0);
	CWnd*	pwndTaskBar  = NULL;
	HWND	hStartMenu   = NULL;
	
	if((pwndTaskBar = CWnd::FindWindow("Shell_TrayWnd", "")) == NULL)
		return FALSE; // Unable to find the task bar window

	//	If this is XP or earlier the start button will be a child of the task bar
	hStartMenu = FindWindowEx(pwndTaskBar->m_hWnd, NULL, "Button", "Start");

	//	If no button, this might be Vista so we have to take a different approach
	if(hStartMenu == NULL)
		hStartMenu = GetVistaStartMenuWnd(pwndTaskBar->m_hWnd);

	// Get the desktop's bounding rectangle
	SystemParametersInfo(SPI_GETWORKAREA,
						 0,
						 (LPVOID)&rectWorkArea,
						 0);
	
	//	Get the bounding rectangle of the task bar
	pwndTaskBar->GetWindowRect(rectTaskBar);

	//	Are we hiding the task bar
	if(bVisible == FALSE)
	{
/*
		//	Adjust the desktop work area to push the taskbar off the bottom of the screen
		rectWorkArea.bottom += rectTaskBar.Height();
		SystemParametersInfo(SPI_SETWORKAREA,
							 0,
							 (LPVOID)&rectWorkArea,
							 0);
*/
		::SetWindowPos(pwndTaskBar->GetSafeHwnd(), HWND_BOTTOM, 0, 0, 0, 0,
					   SWP_NOSIZE|SWP_NOMOVE|SWP_NOACTIVATE|SWP_HIDEWINDOW);

		if((hStartMenu != NULL) && IsWindow(hStartMenu))
			::ShowWindow(hStartMenu, SW_HIDE);
	}
	else
	{
/*
		//	Adjust the desktop work area to bring the taskbar back into the visible area
		rectWorkArea.bottom -= rectTaskBar.Height();
		SystemParametersInfo(SPI_SETWORKAREA,
							 0,
							 (LPVOID)&rectWorkArea,
							 0);
*/
		::SetWindowPos(pwndTaskBar->GetSafeHwnd(), HWND_BOTTOM, 0, 0, 0, 0,
					   SWP_NOSIZE|SWP_NOMOVE|SWP_NOACTIVATE|SWP_SHOWWINDOW);

		if((hStartMenu != NULL) && IsWindow(hStartMenu))
			::ShowWindow(hStartMenu, SW_SHOW);
	}

	return TRUE;

}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMToolbox::SplitPath()
//
//	Parameters:		lpszPath - the path to be parsed
//					rParent  - the buffer in which to store the parent folder
//					rName    - the buffer in which to store the name of the
//							   file or subfolder
//
// 	Return Value:	True if successful
//
// 	Description:	This function will split the path into a parent and name
//
//------------------------------------------------------------------------------
BOOL CTMToolbox::SplitPath(LPCSTR lpszPath, CString& rParent, CString& rName)
{
	char*	pSplit = 0;
	char	szPath[_MAX_PATH];
	
	ASSERT(lpszPath != 0);
	if(lpszPath == 0) return FALSE;
	ASSERT(lstrlen(lpszPath) > 0);
	if(lstrlen(lpszPath) == 0) return FALSE;

	//	Copy the path to our working buffer
	lstrcpyn(szPath, lpszPath, sizeof(szPath));

	//	Strip the trailing backslash if it exists
	if(szPath[lstrlen(szPath) - 1] == '\\')
		szPath[lstrlen(szPath) - 1] = '\0';

	//	Locate the last directory separator
	if((pSplit = strrchr(szPath, '\\')) != 0)
	{
		//	NULL terminate and store the parent
		*pSplit = '\0';
		rParent = szPath;

		//	Line up on the filename
		pSplit++;
		rName = pSplit;
	}
	
	//	How about a drive separator?
	else if((pSplit = strchr(szPath, ':')) != 0)
	{
		//	Line up on the name
		pSplit++;
		rName = pSplit;

		//	NULL terminate and store the parent
		*pSplit = '\0';
		rParent = szPath;
	}
	else
	{
		//	There is no parent
		rName = szPath;
		rParent.Empty();
	}

	return TRUE;	

}

