//==============================================================================
//
// File Name:	tmaxdemo.cpp
//
// Description:	This file contains member functions of the CTmaxdemoApp class as
//				well as global functions exported by this dll
//
// Functions:   CTmaxdemoApp::CopyFile()
//				CTmaxdemoApp::CopyFolder()
//				CTmaxdemoApp::TmaxdemoApp()
//				CTmaxdemoApp::Database()
//				CTmaxdemoApp::Find()
//				CTmaxdemoApp::OpenIni()
//				CTmaxdemoApp::SetFolderAttributes()
//				CTmaxdemoApp::Videos()
//
//				CopyDatabase()
//				CopyIfConfirmed()
//				CopyVideos()
//
// See Also:	tmaxdemo.h
//
// Copyright 1999, Forensic Technologies International
//
//==============================================================================
//	Date		Revision    Description
//	01-23-99	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmaxdemo.h>
#include <tmini.h>
#include <shellapi.h>
#include <videos.h>
#include <database.h>

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
CTmaxdemoApp	theApp;
CTMIni			theIni;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CTmaxdemoApp, CWinApp)
	//{{AFX_MSG_MAP(CTmaxdemoApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CTmaxdemoApp::CopyFile()
//
// 	Description:	This function will copy the source file to the target file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTmaxdemoApp::CopyFile(LPCSTR lpSource, LPCSTR lpTarget, LPCSTR lpTitle)
{
	SHFILEOPSTRUCT	OpStruct;
	char			szFrom[512];
	char			szTo[512];

	ASSERT(lpSource);
	ASSERT(lpTarget);

	//	The file specifications have to be double null terminated
	memset(szFrom, 0, sizeof(szFrom));
	memset(szTo, 0, sizeof(szTo));
	lstrcpyn(szFrom, lpSource, (sizeof(szFrom) - 1));
	lstrcpyn(szTo, lpTarget, (sizeof(szTo) - 1));

	//	Set up the shell operation structure
	memset(&OpStruct, 0, sizeof(OpStruct));
	OpStruct.hwnd   = m_hWnd;
	OpStruct.wFunc  = FO_COPY;
	OpStruct.pFrom  = szFrom;
	OpStruct.pTo	= szTo;
	OpStruct.fFlags = (FOF_NOCONFIRMATION | FOF_NOCONFIRMMKDIR);
	OpStruct.lpszProgressTitle = lpTitle;
					  
	//	Copy the file
	if(SHFileOperation(&OpStruct) == 0)
	{
		SetFileAttributes(lpTarget, FILE_ATTRIBUTE_NORMAL);
		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CTmaxdemoApp::CopyFolder()
//
// 	Description:	This function will copy the source folder to the target 
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTmaxdemoApp::CopyFolder(LPCSTR lpSource, LPCSTR lpTarget, LPCSTR lpTitle)
{
	SHFILEOPSTRUCT	OpStruct;
	char			szFrom[512];
	char			szTo[512];
	CString			strTest;

	ASSERT(lpSource);
	ASSERT(lpTarget);

	//	The file specifications have to be double null terminated
	memset(szFrom, 0, sizeof(szFrom));
	memset(szTo, 0, sizeof(szTo));
	sprintf_s(szFrom, sizeof(szFrom), "%s\\*.*", lpSource);
	lstrcpyn(szTo, lpTarget, (sizeof(szTo) - 1));

	//	Set up the shell operation structure
	memset(&OpStruct, 0, sizeof(OpStruct));
	OpStruct.hwnd   = m_hWnd;
	OpStruct.wFunc  = FO_COPY;
	OpStruct.pFrom  = szFrom;
	OpStruct.pTo	= szTo;
	OpStruct.fFlags = (FOF_NOCONFIRMATION | FOF_NOCONFIRMMKDIR);
	OpStruct.lpszProgressTitle = lpTitle;
					  
	//	Copy the file
	if(SHFileOperation(&OpStruct) == 0)
	{
		SetFolderAttributes(lpTarget);
		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CTmaxdemoApp::CTmaxdemoApp()
//
// 	Description:	This is the constructor for CTmaxdemoApp objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTmaxdemoApp::CTmaxdemoApp()
{
	m_hWnd = 0;
	m_strSource.Empty();
	m_strSupport.Empty();
	m_strInstall.Empty();
}

//==============================================================================
//
// 	Function Name:	CTmaxdemoApp::Database()
//
// 	Description:	This function will copy the sample database to the 
//					installation directory
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTmaxdemoApp::Database(BOOL bConfirm)
{
	CConfirmDatabase	Dialog;
	char				szFolder[256];
	char				szComment[256];
	CString				strFrom;
	CString				strTo;
	CString				strFolder;
	CString				strError;
	CString				strDrive;
	CString				strFTI;

	//	Set the ini file to the database section
	theIni.SetSection(DATABASE_SECTION);

	//	Read the comment line from the ini file
	theIni.ReadString(COMMENT_LINE, szComment, sizeof(szComment));

	//	Get the database folder
	theIni.ReadString(ROOTFOLDER_LINE, szFolder, sizeof(szFolder));
	if(lstrlen(szFolder) == 0)
	{
		//	This must not be a demo version
		m_Flags.WriteBool(DBCOPIED_LINE, FALSE);
		return TRUE;
	}

	//	Set up the dialog 
	Dialog.m_strComment = szComment;
	Dialog.m_strDrive = m_strInstall.Left(1);
	Dialog.m_bShowCancel = bConfirm;
		
	if(Dialog.DoModal() == IDCANCEL)
	{
		m_Flags.WriteBool(DBCOPIED_LINE, FALSE);
		return TRUE;
	}
	else
	{
		strDrive = Dialog.m_strDrive.Left(1);
		if(strDrive.IsEmpty())
			strDrive = m_strInstall.Left(1);
	}

	//	Make sure the source folder exists
	strFrom = m_strSource;
	if(strFrom.Right(1) != "\\")
		strFrom += "\\";
	strFrom += szFolder;
	if(!Find(strFrom))
	{
		strError.Format("Unable to locate %s to copy database files.", strFrom);
		MessageBox(m_hWnd, strError, "Error", MB_ICONEXCLAMATION | MB_OK);
		m_Flags.WriteBool(DBCOPIED_LINE, FALSE);
		return FALSE;
	}
		
	//	Build the target folder specification
	strTo.Format("%s:\\%s", strDrive, szFolder);

	//	Copy the database to the installation directory
	if(!CopyFolder(strFrom, strTo, "Copying Sample Database"))
	{
		MessageBox(m_hWnd, "Unable to copy sample database files", "Error", 
				   MB_ICONEXCLAMATION | MB_OK);
		m_Flags.WriteBool(DBCOPIED_LINE, FALSE);
		return FALSE;
	}

	//	Set the flag in the ini file so that the video knows the database has
	//	been copied
	m_Flags.WriteBool(DBCOPIED_LINE, TRUE);

	//	Now write the case folder information to the FTI ini file
	strFTI.Format("%s%s", m_strInstall, DEFAULT_TMAXINI);
	theIni.Open(strFTI, SHARED_SECTION);
	theIni.WriteLastCase(strTo);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTmaxdemoApp::Find()
//
// 	Description:	This function checks to see if the file or folder exists.
//
// 	Returns:		TRUE if the file exists.
//
//	Notes:			None
//
//==============================================================================
BOOL CTmaxdemoApp::Find(LPCSTR lpFilespec)
{
	WIN32_FIND_DATA	FindData;
	HANDLE			hFind;

	if((hFind = FindFirstFile(lpFilespec, &FindData)) == INVALID_HANDLE_VALUE)
	{
		return FALSE;
	}
	else
	{
		FindClose(hFind);
		return TRUE;
	}
		
}

//==============================================================================
//
// 	Function Name:	CTmaxdemoApp::OpenIni()
//
// 	Description:	This function will open the ini file
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTmaxdemoApp::OpenIni()
{
	CString strFile;
	CString	strFlags;

	//	The ini file should be in the source directory
	strFile = m_strSource + TMAXDEMO_INIFILENAME;

	if(!theIni.Open(strFile))
	{
		return FALSE;
	}
	else
	{
		//	The flags ini should be in the target folder
		strFlags = m_strInstall + DEMOFLAGS_INIFILENAME;
		m_Flags.Open(strFlags, FLAGS_SECTION);

		return TRUE;
	}
}

//==============================================================================
//
// 	Function Name:	CTmaxdemoApp::SetFolderAttributes()
//
// 	Description:	This function will set the file attributes to normal for
//					all files in the specified folder
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
void CTmaxdemoApp::SetFolderAttributes(LPCSTR lpFolder)
{
	WIN32_FIND_DATA	FindData;
	HANDLE			hFind;
	CString			strSearch;
	CString			strFile;
	CString			strSubFolder;
	char			szBuffer[256];
	char*			pName;
	DWORD			dwAttribute = FILE_ATTRIBUTE_ARCHIVE;

	//	Strip the folder name
	lstrcpyn(szBuffer, lpFolder, sizeof(szBuffer));
	if(szBuffer[lstrlen(szBuffer) - 1] == '\\')
		szBuffer[lstrlen(szBuffer) - 1] = 0;
	if((pName = strrchr(szBuffer, '\\')) != 0)
		pName++;
	else
		pName = szBuffer;
		 
	//	If this is a log files folder we want to hide all the files
	if(lstrcmpi(pName, LOGFILES_TEST) == 0)
		dwAttribute = FILE_ATTRIBUTE_HIDDEN;
		
	//	Build the search specification
	strSearch = lpFolder;
	if(strSearch.Right(1) != "\\")
		strSearch += "\\*.*";
	else
		strSearch += "*.*";

	//	Are there any files or folders?
	if((hFind = FindFirstFile(strSearch, &FindData)) == INVALID_HANDLE_VALUE)
		return;	
	
	//	Add all the files and folders
	while(1)
	{
		//	Is this a folder?
		if(FindData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)
		{
			//	Add a new subfolder if we're supposed to
			if(lstrcmpi(FindData.cFileName, ".") &&
			   lstrcmpi(FindData.cFileName, ".."))
			{
				strSubFolder = lpFolder;
				if(strSubFolder.Right(1) != "\\")
					strSubFolder += "\\";
				strSubFolder += FindData.cFileName;
				
				//	Set the attributes for this folder
				SetFolderAttributes(strSubFolder);
			}

		}
		else
		{	
			strFile = lpFolder;
			if(strFile.Right(1) != "\\")
				strFile += "\\";
			strFile += FindData.cFileName;

			SetFileAttributes(strFile, dwAttribute);
		}

		//	Get the next file
		if(!FindNextFile(hFind, &FindData))
			break;

	} // while(1)

	CloseHandle(hFind);
}

//==============================================================================
//
// 	Function Name:	CTmaxdemoApp::Videos()
//
// 	Description:	This function will copy the sample videos to the 
//					installation directory
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTmaxdemoApp::Videos()
{
	CConfirmVideos	Confirm;
	CString			strFTI;
	CString			strDrive;
	CString			strSource;
	CString			strTarget;
	CString			strError;
	char			szFile[256];
	char			szComment[256];
	int				i = 1;

	//	Don't bother if the sample database wasn't copied
	theIni.SetSection(DATABASE_SECTION);
	if(!m_Flags.ReadBool(DBCOPIED_LINE))
	{
		_unlink(m_Flags.strFileSpec);
		return TRUE;
	}

	//	Set the ini file to the video section
	theIni.SetSection(VIDEOS_SECTION);

	//	Get the first filename
	theIni.ReadString(i++, szFile, sizeof(szFile));

	//	Don't bother if no files are specified
	if(lstrlen(szFile) == 0)
	{
		_unlink(m_Flags.strFileSpec);
		return TRUE;
	}

	//	Prompt user to confirm video copying
	theIni.ReadString(COMMENT_LINE, szComment, sizeof(szComment));
	Confirm.m_strComment = szComment;
	Confirm.m_strDrive = m_strInstall.Left(1);
	if(Confirm.DoModal() == IDOK)
	{
		//	Get the desired drive
		strDrive = Confirm.m_strDrive.Left(1);
		
		//	Copy each of the files specified in the ini file
		while(1)
		{
			//	Construct the file specifications
			strSource = m_strSource;
			if(strSource.Right(1) != "\\")
				strSource += "\\";
			strSource += szFile;
			strTarget.Format("%s:\\%s", strDrive, szFile);
	
			//	Make sure the source file exists
			if(!Find(strSource))
			{		
				strError.Format("Unable to copy %s. The file could not be found.", strSource);
				MessageBox(m_hWnd, strError, "Error", MB_ICONEXCLAMATION | MB_OK);
			}
			else
			{
				//	Copy the file to the target machine
				if(!CopyFile(strSource, strTarget, "Copying Videos"))
					break;
			}

			//	Get the next file
			theIni.ReadString(i++, szFile, sizeof(szFile));
			if(lstrlen(szFile) == 0)
				break;
		}
		//	Now write the default drive information to the FTI ini file
		strFTI.Format("%s%s", m_strInstall, DEFAULT_TMAXINI);
		theIni.Open(strFTI, SHARED_SECTION);
		theIni.WriteVideoDrive(TRUE, strDrive);
	}
	else
	{
		//	The source drive will be used as the default video drive
		strDrive = m_strSource.Left(1);
		
		//	Now write the default drive to the FTI ini file
		strFTI.Format("%s%s", m_strInstall, DEFAULT_TMAXINI);
		theIni.Open(strFTI, SHARED_SECTION);
		theIni.WriteVideoDrive(TRUE, strDrive);
	}

	_unlink(m_Flags.strFileSpec);
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CopyDatabase()
//
// 	Description:	This function is called by the installation program to copy
//					the source database without user confirmation
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
extern "C" CHAR WINAPI CopyDatabase(HWND hWnd, LPSTR lpSource, LPSTR lpSupport,
								    LPSTR lpInstall, LPSTR lpReserved)
{
	//	Save the parameters
	theApp.m_hWnd = hWnd;
	theApp.m_strSource  = lpSource;
	theApp.m_strSupport = lpSupport;
	theApp.m_strInstall = lpInstall;

	if(theApp.m_strSource.Right(1) != "\\")
		theApp.m_strSource += "\\";
	if(theApp.m_strSupport.Right(1) != "\\")
		theApp.m_strSupport += "\\";
	if(theApp.m_strInstall.Right(1) != "\\")
		theApp.m_strInstall += "\\";
	
	//	Open the ini file
	if(!theApp.OpenIni())
		return 1;

	//	Copy the sample database without confirmation
	if(!theApp.Database(FALSE))
		return 0;

	return 1;
}

//==============================================================================
//
// 	Function Name:	CopyIfConfirmed()
//
// 	Description:	This function is called by the installation program to copy
//					the source database with user confirmation
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
extern "C" CHAR WINAPI CopyIfConfirmed(HWND hWnd, LPSTR lpSource, LPSTR lpSupport,
								       LPSTR lpInstall, LPSTR lpReserved)
{
	//	Save the parameters
	theApp.m_hWnd = hWnd;
	theApp.m_strSource  = lpSource;
	theApp.m_strSupport = lpSupport;
	theApp.m_strInstall = lpInstall;

	if(theApp.m_strSource.Right(1) != "\\")
		theApp.m_strSource += "\\";
	if(theApp.m_strSupport.Right(1) != "\\")
		theApp.m_strSupport += "\\";
	if(theApp.m_strInstall.Right(1) != "\\")
		theApp.m_strInstall += "\\";
	
	//	Open the ini file
	if(!theApp.OpenIni())
		return 1;

	//	Copy the sample database with confirmation
	if(!theApp.Database(TRUE))
		return 0;

	return 1;
}

//==============================================================================
//
// 	Function Name:	CopyVideos()
//
// 	Description:	This function is called by the installation program to copy
//					the source videos
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
extern "C" CHAR WINAPI CopyVideos(HWND hWnd, LPSTR lpSource, LPSTR lpSupport,
								  LPSTR lpInstall, LPSTR lpReserved)
{
	//	Save the directory information
	theApp.m_strSource  = lpSource;
	theApp.m_strSupport = lpSupport;
	theApp.m_strInstall = lpInstall;
	
	if(theApp.m_strSource.Right(1) != "\\")
		theApp.m_strSource += "\\";
	if(theApp.m_strSupport.Right(1) != "\\")
		theApp.m_strSupport += "\\";
	if(theApp.m_strInstall.Right(1) != "\\")
		theApp.m_strInstall += "\\";
	
	//	Open the ini file
	if(!theApp.OpenIni())
		return 1;

	//	Copy the videos
	if(!theApp.Videos())
		return 0;

	return 1;
	
}

