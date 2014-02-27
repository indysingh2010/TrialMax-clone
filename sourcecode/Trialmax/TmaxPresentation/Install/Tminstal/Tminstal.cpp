//==============================================================================
//
// File Name:	tminstal.cpp
//
// Description:	This file contains member functions of the CTMInstallApp class as
//				well as global functions exported by this dll
//
// Functions:   CTMInstallApp::CTMInstallApp()
//
//				InstallDirectMedia()
//				Register()
//				RegisterDao()
//				RegisterControls()
//
// See Also:	tminstal.h
//
// Copyright 1999, Forensic Technologies International
//
//==============================================================================
//	Date		Revision    Description
//	03-25-99	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tminstal.h>
#include <shlobj.h>
#include <tmver.h>
#include <toolbox.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

//------------------------------------------------------------------------------
//	REVISION INFORMATION
//------------------------------------------------------------------------------
//
//	_wVerMajor:	Major version identifer is changed when significant changes
//				have been made in the entire suite of controls and applications
//				All controls and applications should ALWAYS have the same
//				major version identifier
//
//	_wVerMinor:	Minor version identifier is changed when changes have been made
//				to a control and/or application that would render it unusable
//				with the existing release. All controls and applications will
//				have the same minor revision identifier when bundled as a new
//				release but individual controls and/or applications may be
//				upgraded between releases.
//
//	NOTE:		The major and minor identifiers for this library should match
//				the major and minor identifiers of the matching baseline
//				TrialMax installation.
//
//------------------------------------------------------------------------------
const WORD	_wVerMajor = 6;
const WORD	_wVerMinor = 4;

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------
CTMInstallApp		theApp;
CTMIni				theIni;

STARTUPINFO			_StartupInfo;
PROCESS_INFORMATION	_ProcessInfo;

//	Active X CLSID identifiers
const char* _AxClsIds[TMAX_AXCTRL_MAX] =	{	"{0C69F0D1-9BB0-4DB0-A600-D98621E8D8B3}",	//	TMSTAT
												"{AA52288D-2A50-494F-98FE-FFF0D9FBDE56}",	//	TMTEXT
												"{5A3A9FC9-D747-4B92-9106-A32C7E6E84A3}",	//	TMVIEW
												"{7EFCBDC0-F749-4574-8DC1-2E5575DD9808}",	//	TMLPEN
												"{2341B5A2-769B-49CC-8652-B8914992AFB1}",	//	TMTOOL
												"{5284E5B7-9E77-4200-9E9F-D5F22CB40F2C}",	//	TMBARS
												"{D71D2494-B9CA-401F-8E24-1815E077CE64}",	//	TMMOVIE
												"{BD138FDB-21B2-4CF1-8175-A94182FED781}",	//	TMPOWER
												"{2B6165A5-C1FC-463E-9B56-20143BF4F627}",	//	TMPRINT
												"{CB5D5073-AB77-45F6-B728-1808DDC80026}",	//	TMSHARE
												"{4BA3488C-31EC-4619-9D96-1EFE592DD861}",	//	TMGRAB
												"{B581682E-5CC0-4E50-BBBC-582D78677E5A}",	//	TMSETUP
											};

//------------------------------------------------------------------------------
//	PROTOTYPES
//------------------------------------------------------------------------------
STDAPI	Register(char* pLibrary);
void	AddShortcut(char* pPath, char* pFilename);
BOOL	IsRebootRequired();

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CTMInstallApp, CWinApp)
	//{{AFX_MSG_MAP(CTMInstallApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

//==============================================================================
//
// 	Function Name:	CTMInstallApp::CheckSetupVersion()
//
// 	Description:	This function is called to confirm that the major.minor
//					version identifiers for the baseline setup match those
//					assigned to this update
//
// 	Returns:		TRUE if OK to continue
//
//	Notes:			None
//
//==============================================================================
BOOL CTMInstallApp::CheckSetupVersion(LPCSTR lpszInstallDir)
{
    int		iMajorInstalled;
	int		iMinorInstalled;
	int		iPackedInstalled;
	int		iPackedInstallation;
	CString	strMsg = "";

	ASSERT(lpszInstallDir != 0);
	ASSERT(lstrlen(lpszInstallDir) > 0);

	if(lpszInstallDir == 0) return FALSE;
	if(lstrlen(lpszInstallDir) == 0) return FALSE;

	//	Get the installed version
	if(GetSetupVersion(lpszInstallDir, iMajorInstalled, iMinorInstalled) == TRUE)
	{
		//	Pack the version identifiers
		iPackedInstalled = (iMajorInstalled * 10) + iMinorInstalled;
		iPackedInstallation = ((int)_wVerMajor * 10) + (int)_wVerMinor;

		if(iPackedInstalled != iPackedInstallation)
		{
			strMsg.Format("TrialMax %d.%d is installed on this machine but this update is provided for TrialMax %d.%d. The update version must match the installed baseline version.",
						  iMajorInstalled, iMinorInstalled, (int)_wVerMajor, (int)_wVerMinor);
			return WarningBox(strMsg);
		}
	
	}// if(GetSetupVersion(lpszInstallDir, iMajorInstalled, iMinorInstalled) == TRUE)

	return TRUE; // Allow to proceed if unable to get version information	
}

//==============================================================================
//
// 	Function Name:	CTMInstallApp::ConfirmInstallDir()
//
// 	Description:	This function will confirm the value stored in registry
//					that is used to set the installation's <INSTALLDIR> path
//
// 	Returns:		Non-zero if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMInstallApp::ConfirmInstallDir(HWND hWnd, LPSTR lpSource, LPSTR lpTarget, BOOL bVideoViewer) 
{
	CTMVersion	verInstalled;
	CString		strPath;
	CString		strParent;
	CString		strInstallDir;
	CString		strMsg;

	//	Save the values provided by the caller
	m_hWnd = hWnd;
	m_bVideoViewer = bVideoViewer;
	m_strSource = (lpSource != 0) ? lpSource : "";
	m_strTarget = (lpTarget != 0) ? lpTarget : "";

	//	Are we updating the video viewer?
	if(bVideoViewer == TRUE)
		m_strProductKey = TMAXVIDEO_INSTALLDIR_KEY_PATH;
	else
		m_strProductKey = TRIALMAX_INSTALLDIR_KEY_PATH;

	//	Get the target installation folder
	if(GetInstallDir(strInstallDir) == FALSE) 
		return FALSE;

	//	Compare the versions to make sure this update is appropriate
	//if(CheckSetupVersion(strInstallDir) == FALSE) 
		//return FALSE;
	
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTMInstallApp::ConfirmNETInstallation()
//
// 	Description:	This function will confirm that the required version of
//					the .NET framework has been installed on the target machine
//
// 	Returns:		Non-zero if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMInstallApp::ConfirmNETInstallation(HWND hWnd, LPSTR lpSource, LPSTR lpTarget, float fVersion) 
{
    HKEY        hKey = NULL;
 	char		szValue[256];
	DWORD		dwLength = sizeof(szValue);
	DWORD		dwValue = 0;
	CString		strPath = "";
	CString		strName = "";
	CString		strErrorMsg = "";
	BOOL		bInstalled = FALSE;

	memset(szValue, 0, sizeof(szValue));

	//	Which value are we checking for?
	if(fVersion <= 1.0)
	{
		strPath = NET_10_INSTALL_KEY_PATH;
		strName = NET_10_INSTALL_VALUE_NAME;
	}
	else if(fVersion <= 1.1)
	{
		strPath = NET_11_INSTALL_KEY_PATH;
		strName = NET_11_INSTALL_VALUE_NAME;
	}
	else if(fVersion <= 2.0)
	{
		strPath = NET_20_INSTALL_KEY_PATH;
		strName = NET_20_INSTALL_VALUE_NAME;
	}
	else
	{
		strPath = NET_30_INSTALL_KEY_PATH;
		strName = NET_30_INSTALL_VALUE_NAME;
	}

	//	Open the key to the .NET 2.0 installation information
	if(RegOpenKeyEx(HKEY_LOCAL_MACHINE, strPath, 0, KEY_QUERY_VALUE, &hKey) == ERROR_SUCCESS)
	{
		if(fVersion <= 1.0)
		{
			dwLength = sizeof(szValue);
			bInstalled = (RegQueryValueEx(hKey, strName, NULL, NULL, (LPBYTE)szValue, &dwLength) == ERROR_SUCCESS);
		}
		else
		{
			dwLength = sizeof(dwValue);
			if(RegQueryValueEx(hKey, strName, NULL, NULL, (LPBYTE)(&dwValue), &dwLength) == ERROR_SUCCESS)
			{
				bInstalled = (dwValue != 0);
			}

		}

		RegCloseKey(hKey);	
	}

	//	Should we display an error message?
	if(bInstalled == FALSE)
	{
		strErrorMsg.Format("Microsoft .NET Framework %.1f has not been installed. Check the TrialMax update site for the .NET Framework update and install before running this update again.", fVersion);
		WarningBox(strErrorMsg);
	}

	return bInstalled;
}

//==============================================================================
//
// 	Function Name:	CTMInstallApp::CopyFile()
//
// 	Description:	This function will copy the source file to the target file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMInstallApp::CopyFile(LPCSTR lpSource, LPCSTR lpTarget)
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
// 	Function Name:	CTMInstallApp::CopyFiles()
//
// 	Description:	This function will copy all files listed in the default
//					ini file to the target directory
//
// 	Returns:		Non-zero if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMInstallApp::CopyFiles(HWND hWnd, LPSTR lpSource, LPSTR lpTarget) 
{
	char	szIniStr[256];
	CString	strFilename;
	CString	strSource;
	CString	strTarget;
	CString	strIniFile;

	m_hWnd = hWnd;

	//	Open the ini file
	strIniFile = lpSource;
	if(strIniFile.Right(1) != "\\")
		strIniFile += "\\";
	strIniFile += TMAXINSTALL_INIFILENAME;

	if(!m_Ini.Open(strIniFile, TMAXINSTALL_FILESSECTION))
		return FALSE;

	//	Copy each file listed from the source to the target location
	for(int i = 1; ; i++)
	{
		m_Ini.ReadString(i, szIniStr, sizeof(szIniStr));

		//	Strip all whitespace
		strFilename = szIniStr;
		strFilename.TrimRight();
		strFilename.TrimLeft();

		//	Have we run out of filenames?
		if(strFilename.IsEmpty())
			break;

		//	Build the source and target specifications
		strSource = lpSource;
		if(strSource.Right(1) != "\\")
			strSource += "\\";
		strSource += strFilename;

		strTarget = lpTarget;
		if(strTarget.Right(1) != "\\")
			strTarget += "\\";
		strTarget += strFilename;

		//	Copy the file	
		CopyFile(strSource, strTarget);
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTMInstallApp::CopyFolder()
//
// 	Description:	This function will copy the source folder to the target 
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMInstallApp::CopyFolder(LPCSTR lpSource, LPCSTR lpTarget)
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
	sprintf_s(szTo, sizeof(szTo), lpTarget, (sizeof(szTo) - 1));

	//	Set up the shell operation structure
	memset(&OpStruct, 0, sizeof(OpStruct));
	OpStruct.hwnd   = m_hWnd;
	OpStruct.wFunc  = FO_COPY;
	OpStruct.pFrom  = szFrom;
	OpStruct.pTo	= szTo;
	OpStruct.fFlags = (FOF_NOCONFIRMATION | FOF_NOCONFIRMMKDIR);
					  
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
// 	Function Name:	CTMInstallApp::CopyFolders()
//
// 	Description:	This function will copy all folders listed in the default
//					ini file to the target directory
//
// 	Returns:		Non-zero if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMInstallApp::CopyFolders(HWND hWnd, LPSTR lpSource, LPSTR lpTarget) 
{
	char	szIniStr[256];
	CString	strFolder;
	CString	strSource;
	CString	strTarget;
	CString	strIniFile;

	m_hWnd = hWnd;

	//	Open the ini file
	strIniFile = lpSource;
	if(strIniFile.Right(1) != "\\")
		strIniFile += "\\";
	strIniFile += TMAXINSTALL_INIFILENAME;

	if(!m_Ini.Open(strIniFile, TMAXINSTALL_FOLDERSSECTION))
		return FALSE;

	//	Copy each folder listed from the source to the target location
	for(int i = 1; ; i++)
	{
		m_Ini.ReadString(i, szIniStr, sizeof(szIniStr));

		//	Strip all whitespace
		strFolder = szIniStr;
		strFolder.TrimRight();
		strFolder.TrimLeft();

		//	Have we run out of folders?
		if(strFolder.IsEmpty())
			break;

		//	Build the source and target specifications
		strSource = lpSource;
		if(strSource.Right(1) != "\\")
			strSource += "\\";
		strSource += strFolder;

		strTarget = lpTarget;
		if(strTarget.Right(1) != "\\")
			strTarget += "\\";
		strTarget += strFolder;

		//	Copy the folder	
		CopyFolder(strSource, strTarget);
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTMInstallApp::CTMInstallApp()
//
// 	Description:	This is the constructor for CTMInstallApp objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMInstallApp::CTMInstallApp()
{
	m_hWnd = 0;
	m_strProductKey = "";
	m_strSource = "";
	m_strTarget = "";
	m_bVideoViewer = FALSE;
}

//==============================================================================
//
// 	Function Name:	CTMInstallApp::GetAppFileSpec()
//
// 	Description:	This function is called to retrieve the fully qualified
//					path to the application executable.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMInstallApp::GetAppFileSpec(LPCSTR lpszInstallDir, CString& rPath)
{
    ASSERT(lpszInstallDir != 0);
	ASSERT(lstrlen(lpszInstallDir) > 0);

	if((lpszInstallDir != 0) && (lstrlen(lpszInstallDir) > 0))
	{
		rPath = lpszInstallDir;
		if(rPath.Right(1) != "\\")
			rPath += "\\";

		if(m_bVideoViewer == TRUE)
			rPath += "TrialMax 6 Video Viewer\\TmaxVideo.exe";
		else
			rPath += "TrialMax 6\\TmaxManager.exe";
	}

}

//==============================================================================
//
// 	Function Name:	CTMInstallApp::GetInstallDir()
//
// 	Description:	This function is called to get the INSTALLDIR value that
//					represents the root installation folder
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMInstallApp::GetInstallDir(CString& rPath)
{
 	CString	strInstallDir = "";
	CString	strInstalled = "";
	CString	strApp = "";
	CString	strMsg = "";

	//	First attempt to read the value stored in the system registry
	if(ReadInstallDir(strInstallDir) == TRUE)
	{
		//	Get the path to the application's executable
		GetAppFileSpec(strInstallDir, strApp);

		//	Does the file exist?
		if(CTMToolbox::FindFile(strApp) == TRUE)
		{
			rPath = strInstallDir;
			return TRUE;	//	OK to use the stored path
		}

	}// if(ReadInstallDir(strInstallDir) == TRUE)

	//	Either the InstallDir path doesn't exist or is invalid. Try searching 
	//	for the actual installed location
	if(GetInstalledLocation(strInstalled) == TRUE)
	{
		//	Update the information stored in the registry
		if(WriteInstallDir(strInstalled) == TRUE)
		{
			rPath = strInstalled;
			return TRUE;
		}

	}

	//	Unable to locate the installation folder
	strMsg.Format("Unable to locate TrialMax %d.%d installation. You must run the baseline installation before installing the update.",
				  (int)_wVerMajor, (int)_wVerMinor);
	return WarningBox(strMsg);

}

//==============================================================================
//
// 	Function Name:	CTMInstallApp::GetInstalledLocation()
//
// 	Description:	This function is called to locate the folder where the
//					application and it's components are stored
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMInstallApp::GetInstalledLocation(CString& rPath)
{
    CTMVersion	verInstalled;
	CString		strPath = "";
	CString		strParent = "";
	CString		strInstallation = "";

	//	Check each of the TrialMax ActiveX controls until we locate the 
	//	registration for one of them
	for(int i = 0; i < TMAX_AXCTRL_MAX; i++)
	{
		//	Get the path where the TMView control is registered
		if(verInstalled.GetRegisteredPath(_AxClsIds[i], strPath) == TRUE)
		{
			//	Does the file exist?
			if(CTMToolbox::FindFile(strPath) == TRUE)
				break;
		}

		//	Try again
		strPath = "";

	}// for(int i = 0; i < TMAX_AXCTRL_MAX; i++)	
	
	//	Did we find one of the ActiveX controls?
	if(strPath.GetLength() > 0)
	{
		//	Strip the filename and it's parent folder to get back to the root installation folder
		if(CTMToolbox::GetParent(strPath, strParent) == TRUE)
		{
			strPath = strParent;
			if(CTMToolbox::GetParent(strPath, strParent) == TRUE)
			{
				strInstallation = strParent;
			}
		}

	}

	rPath = strInstallation;
	return (rPath.GetLength() > 0);
	
}

//==============================================================================
//
// 	Function Name:	CTMInstallApp::GetRegisteredPath()
//
// 	Description:	This function is called to get the path where the control
//					with the specified class id is located
//
// 	Returns:		Non-zero if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMInstallApp::GetRegisteredPath(LPCSTR lpszClsId, CString& rPath)
{
    HKEY        hKey = NULL;
    char        szKey[513];
	char		szPath[_MAX_PATH];
	DWORD		dwLength = sizeof(szPath);

	memset(szPath, 0, sizeof(szPath));
	rPath.Empty();

	// Open the key under the control's clsid HKEY_CLASSES_ROOT\CLSID\<CLSID>
	wsprintf(szKey, "CLSID\\%s\\InprocServer32", lpszClsId);
	if(RegOpenKeyEx(HKEY_CLASSES_ROOT, szKey, 0, KEY_QUERY_VALUE, &hKey) == ERROR_SUCCESS)
	{
		if((RegQueryValueEx(hKey, "", NULL, NULL, (LPBYTE)szPath, &dwLength) == ERROR_SUCCESS) &&
		   (lstrlen(szPath) > 0))
		{
			rPath = szPath;
			rPath.MakeLower();
		}

		RegCloseKey(hKey);
	
	}

	return (rPath.GetLength() > 0);
}

//==============================================================================
//
// 	Function Name:	CTMInstallApp::GetSetupVersion()
//
// 	Description:	This function is called to get the major and minor version
//					identifiers for the current installation.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMInstallApp::GetSetupVersion(LPCSTR lpszInstallDir, int& rMajor, int& rMinor)
{
 	CTMVersion	ver;
	CString		strApp;
	
	ASSERT(lpszInstallDir != 0);
	ASSERT(lstrlen(lpszInstallDir) > 0);

	if(lpszInstallDir == 0) return FALSE;
	if(lstrlen(lpszInstallDir) == 0) return FALSE;

	//	Get the path to application
	GetAppFileSpec(lpszInstallDir, strApp);

	//	Get the version information for the application
	if(ver.InitFromFile("", "", strApp) == TRUE)
	{
		rMajor = ver.GetMajor();
		rMinor = ver.GetMinor();
		return TRUE;
	}

	//	Get the baseline version from one of the ActiveX controls
	for(int i = 0; i < TMAX_AXCTRL_MAX; i++)
	{
		//	Get the version information for the control
		if(ver.InitFromClsId("", "", _AxClsIds[i]) == TRUE)
		{
			rMajor = ver.GetMajor();
			rMinor = ver.GetMinor();
			return TRUE;
		}

	}// for(int i = 0; i < TMAX_AXCTRL_MAX; i++)
	
	//	Must not have been able to get version information
	return FALSE;	
}

//==============================================================================
//
// 	Function Name:	CTMInstallApp::ReadInstallDir()
//
// 	Description:	This function is called to read the INSTALLDIR value stored
//					in the system registry
//
// 	Returns:		Non-zero if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMInstallApp::ReadInstallDir(CString& rPath)
{
    HKEY        hKey = NULL;
 	char		szPath[_MAX_PATH];
	DWORD		dwLength = sizeof(szPath);

	rPath = "";

	// Locate the folder where the tm_view6.ocx control is registered
	memset(szPath, 0, sizeof(szPath));
	if(RegOpenKeyEx(HKEY_LOCAL_MACHINE, m_strProductKey, 0, KEY_QUERY_VALUE, &hKey) == ERROR_SUCCESS)
	{
		if(RegQueryValueEx(hKey, TMAXINSTALL_INSTALLDIR_VALUE_NAME, NULL, NULL, (LPBYTE)szPath, &dwLength) == ERROR_SUCCESS)
		{
			rPath = szPath;
		}

		RegCloseKey(hKey);	
	}

	return (rPath.GetLength() > 0);
	
}

//==============================================================================
//
// 	Function Name:	CTMInstallApp::SetFolderAttributes()
//
// 	Description:	This function will set the file attributes to normal for
//					all files in the specified folder
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
void CTMInstallApp::SetFolderAttributes(LPCSTR lpFolder)
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
// 	Function Name:	CTMInstallApp::WarningBox()
//
// 	Description:	This function is called to display a warning message
//
// 	Returns:		Always FALSE
//
//	Notes:			None
//
//==============================================================================
BOOL CTMInstallApp::WarningBox(LPCSTR lpszMessage)
{
   MessageBox(m_hWnd, lpszMessage, "Installation Error", MB_OK | MB_ICONWARNING);
   return FALSE;
}

//==============================================================================
//
// 	Function Name:	CTMInstallApp::WriteInstallDir()
//
// 	Description:	This function is called to set the key used by the update
//					installer to find the target location
//
// 	Returns:		Non-zero if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMInstallApp::WriteInstallDir(CString& rPath)
{
    HKEY        hKey = NULL;
	char		szPath[_MAX_PATH];
	BOOL		bSuccessful = FALSE;
	CString		strInstallDir;

	lstrcpyn(szPath, rPath, sizeof(szPath));

	//	Open the key that contains the value for the TrialMax installation folder
	if(RegCreateKeyEx(HKEY_LOCAL_MACHINE, m_strProductKey, 0, NULL, 0, KEY_CREATE_SUB_KEY | KEY_READ | KEY_WRITE, NULL, &hKey, NULL) == ERROR_SUCCESS)
	{
		//	Read the installation folder value stored in the registry
		if(RegSetValueEx(hKey, TMAXINSTALL_INSTALLDIR_VALUE_NAME, NULL, REG_SZ, (LPBYTE)szPath, lstrlen(szPath)) == ERROR_SUCCESS)
		{
			bSuccessful = TRUE;
		}

		RegCloseKey(hKey);	
	}

	//	If not successful check to see if the key already contains the correct value
	if(bSuccessful == FALSE)
	{
		if(ReadInstallDir(strInstallDir) == TRUE)
		{
			//	OK if they match
			if(strInstallDir.CompareNoCase(rPath) == 0)
				bSuccessful = TRUE;
		}
	}

	return bSuccessful;
	
}

//==============================================================================
//
// 	Function Name:	InstallDirectMedia()
//
// 	Description:	This function is called by the installation program to 
//					install the direct media runtime extensions
//
// 	Returns:		Always non-zero to prevent closure of installation
//
//	Notes:			None
//
//==============================================================================
extern "C" CHAR WINAPI InstallDirectMedia(HWND hWnd, LPSTR lpSource, 
										  LPSTR lpSupport, LPSTR lpInstall, 
										  LPSTR lpReserved)
{
	char	szFile[MAX_PATH];
	char	szDir[MAX_PATH];
	char	szTemp[16];
	DWORD	dwExitCode = NOERROR;
	
	//	Get the path to the window's system directory. This is where the
	//	installation will put the runtime distribution
	GetSystemDirectory(szDir, sizeof(szDir));

	//	Build the specification for the Direct Media Distribution
	sprintf_s(szFile, sizeof(szFile), "%s\\Dxmedia.exe", szDir);

	//	Create the temporary directory
	//
	//	NOTE:	We do this because of a bug in the runtime distribution.
	//			On NT, if the default temporary directory does not exist, the
	//			distribution runs but does not report an error even though the
	//			files don't get installed
	lstrcpyn(szTemp, szDir, sizeof(szTemp));
	lstrcpy(&(szTemp[3]), "Temp");
	CreateDirectory(szTemp, 0);

    ZeroMemory(&_StartupInfo, sizeof(_StartupInfo));
	ZeroMemory(&_ProcessInfo, sizeof(_ProcessInfo));
	_StartupInfo.cb = sizeof(_StartupInfo);

    if(!CreateProcess(szFile, " -NQ -id:fticorpmedia", 0, 0, 0, 0, 0, 0, 
					 &_StartupInfo, &_ProcessInfo))
	{
        MessageBeep(0xFFFFFFFF);
		MessageBox(hWnd, "Unable to launch Direct Media installation", 
				   "Error", MB_ICONEXCLAMATION | MB_OK);
		return 1L;
	}
	
	//	Wait for the installation to finish
	WaitForSingleObject(_ProcessInfo.hProcess, INFINITE);

	//	Get the installation's exit code 
	GetExitCodeProcess(_ProcessInfo.hProcess, &dwExitCode);
      
	//	What was the code
	switch(dwExitCode)
    { 

		case ERROR_MEMBER_NOT_IN_GROUP:
		
			MessageBeep(MB_ICONEXCLAMATION);
			MessageBox(hWnd, "You must have administrator privledges to install Direct Media on NT",
			           "Error", MB_ICONEXCLAMATION | MB_OK);
			break;

		case ERROR_OLD_WIN_VERSION:
		
			MessageBeep(MB_ICONEXCLAMATION);
			MessageBox(hWnd, "Direct Media requires service pack 3 or greater for Windows NT",
			           "TMAX Error", MB_ICONEXCLAMATION | MB_OK);
			break;

		case E_FAIL:

            MessageBeep(MB_ICONEXCLAMATION);
			MessageBox(hWnd,"The direct media installation failed",
					   "TMAX Error", MB_ICONEXCLAMATION | MB_OK);
            break;

		case ERROR_SUCCESS:
        case ERROR_SUCCESS_REBOOT_REQUIRED:
		default:
            break;

	} 

	//	Delete the runtime distribution
	_unlink(szFile);
	return 1;			
}

//==============================================================================
//
// 	Function Name:	RegisterControls()
//
// 	Description:	This function is called by the installation program to 
//					register the TrialMax II controls
//
// 	Returns:		Always non-zero to prevent closure of installation
//
//	Notes:			This version of the function will register all TMAX controls
//
//==============================================================================
extern "C" CHAR WINAPI RegisterControls(HWND hWnd, LPSTR lpSource, 
										LPSTR lpSupport, LPSTR lpInstall, 
										LPSTR lpReserved)
{
	char szFilespec[MAX_PATH];
	char szCmdLine[MAX_PATH];
	
	//	Build the specification for the TrialMax II registration program
	//
	//	NOTE:	It is assumed the installation program has copied the program
	//			to the same folder as the controls
	sprintf_s(szFilespec, sizeof(szFilespec), "%s\\Common\\Tmregsvr.exe", lpInstall);
	sprintf_s(szCmdLine, sizeof(szCmdLine), " %s", lpInstall);

    ZeroMemory(&_StartupInfo, sizeof(_StartupInfo));
	ZeroMemory(&_ProcessInfo, sizeof(_ProcessInfo));
	_StartupInfo.cb = sizeof(_StartupInfo);

    if(!CreateProcess(szFilespec, szCmdLine, 0, 0, 0, 0, 0, 0, 
					 &_StartupInfo, &_ProcessInfo))
	{
        MessageBeep(0xFFFFFFFF);
		MessageBox(hWnd, "Unable to launch TrialMax Registration Server", 
				   "Error", MB_ICONEXCLAMATION | MB_OK);
		return 1L;
	}
	
	//	Wait for the installation to finish
	WaitForSingleObject(_ProcessInfo.hProcess, INFINITE);

	return 1;			
}

//==============================================================================
//
// 	Function Name:	RegisterDao()
//
// 	Description:	This function is called by the installation program to 
//					register the DAO libraries
//
// 	Returns:		Non-zero if successful
//
//	Notes:			None
//
//==============================================================================
extern "C" CHAR WINAPI RegisterDao(HWND hWnd, LPSTR lpSource, 
								   LPSTR lpSupport, LPSTR lpInstall, 
								   LPSTR lpReserved)
{
	HKEY	hKey;
	DWORD	dwType = REG_SZ;
	char	szFile[MAX_PATH];
	char	szDir[MAX_PATH];
	DWORD	dwSize = sizeof(szDir);
	
	//	Get the path to the common files
	memset(szDir, 0, sizeof(szDir));
	if(RegOpenKey(HKEY_LOCAL_MACHINE,
			   "Software\\Microsoft\\Windows\\CurrentVersion",
			   &hKey) == ERROR_SUCCESS)
	{
		RegQueryValueEx(hKey, "CommonFilesDir", 0, &dwType, (LPBYTE)szDir,
						&dwSize);
	}
	
	//	Build the specification for the dao dll
	sprintf_s(szFile, sizeof(szFile), "%s\\Microsoft Shared\\Dao\\Dao350.dll", szDir);	

	//	Register the dao dlls
	Register("msjet35.dll");
	Register("msrd2x35.dll");
	Register(szFile);

	return 1;
}

//==============================================================================
//
// 	Function Name:	CopyFiles()
//
// 	Description:	This function is called by the installation program to 
//					copy the files not built into the installation
//
// 	Returns:		Non-zero if successful
//
//	Notes:			None
//
//==============================================================================
extern "C" CHAR WINAPI CopyFiles(HWND hWnd, LPSTR lpSource, 
								 LPSTR lpSupport, LPSTR lpInstall, 
								 LPSTR lpReserved)
{
	theApp.CopyFiles(hWnd, lpSource, lpInstall);
	theApp.CopyFolders(hWnd, lpSource, lpInstall);
	return 1;
}

//==============================================================================
//
// 	Function Name:	Register()
//
// 	Description:	This function will register the library specified by the
//					caller
//
// 	Returns:		Non-zero if successful
//
//	Notes:			None
//
//==============================================================================
STDAPI Register(char* pLibrary)
{
	HINSTANCE hOcx = LoadLibrary(pLibrary);

	if(hOcx != NULL)
	{
		// Find the entry point.
		FARPROC lpRegister = GetProcAddress(hOcx, _T("DllRegisterServer"));
		if(lpRegister != NULL)
		{
			(*lpRegister)();
			return 1L;
		}
	}

	return 0L;
}

//==============================================================================
//
// 	Function Name:	ConfirmInstallDir()
//
// 	Description:	This function is called by the installation to confirm the
//					registry value used to set the <INSTALLDIR> path
//
// 	Returns:		Non-zero to prevent closure of installation
//
//	Notes:			None
//
//==============================================================================
extern "C" CHAR WINAPI ConfirmInstallDir(HWND hWnd, LPSTR lpSource, 
									     LPSTR lpSupport, LPSTR lpInstall, 
									     LPSTR lpReserved)
{
	if(theApp.ConfirmInstallDir(hWnd, lpSource, lpInstall, FALSE) == TRUE)
		return 1; // OK to continue
	else
		return 0; // Stop the installation
}

//==============================================================================
//
// 	Function Name:	ConfirmInstallDir()
//
// 	Description:	This function is called by the installation to confirm the
//					registry value used to set the <INSTALLDIR> path for
//					the video viewer update
//
// 	Returns:		Non-zero to prevent closure of installation
//
//	Notes:			None
//
//==============================================================================
extern "C" CHAR WINAPI ConfirmVVInstallDir(HWND hWnd, LPSTR lpSource, 
									     LPSTR lpSupport, LPSTR lpInstall, 
									     LPSTR lpReserved)
{
	if(theApp.ConfirmInstallDir(hWnd, lpSource, lpInstall, TRUE) == TRUE)
		return 1; // OK to continue
	else
		return 0; // Stop the installation
}

//==============================================================================
//
// 	Function Name:	ConfirmNET10Installed()
//
// 	Description:	This function is called to confirm that .NET 1.0 has been 
//					installed on the target machine
//
// 	Returns:		Non-zero to prevent closure of installation
//
//	Notes:			None
//
//==============================================================================
extern "C" CHAR WINAPI ConfirmNET10Installed(HWND hWnd, LPSTR lpSource, 
											 LPSTR lpSupport, LPSTR lpInstall, 
											 LPSTR lpReserved)
{
	if(theApp.ConfirmNETInstallation(hWnd, lpSource, lpInstall, 1.0f) == TRUE)
		return 1; // OK to continue
	else
		return 0; // Stop the installation
}

//==============================================================================
//
// 	Function Name:	ConfirmNET11Installed()
//
// 	Description:	This function is called to confirm that .NET 1.1 has been 
//					installed on the target machine
//
// 	Returns:		Non-zero to prevent closure of installation
//
//	Notes:			None
//
//==============================================================================
extern "C" CHAR WINAPI ConfirmNET11Installed(HWND hWnd, LPSTR lpSource, 
											 LPSTR lpSupport, LPSTR lpInstall, 
											 LPSTR lpReserved)
{
	if(theApp.ConfirmNETInstallation(hWnd, lpSource, lpInstall, 1.1f) == TRUE)
		return 1; // OK to continue
	else
		return 0; // Stop the installation
}

//==============================================================================
//
// 	Function Name:	ConfirmNET20Installed()
//
// 	Description:	This function is called to confirm that .NET 2.0 has been 
//					installed on the target machine
//
// 	Returns:		Non-zero to prevent closure of installation
//
//	Notes:			None
//
//==============================================================================
extern "C" CHAR WINAPI ConfirmNET20Installed(HWND hWnd, LPSTR lpSource, 
											 LPSTR lpSupport, LPSTR lpInstall, 
											 LPSTR lpReserved)
{
	if(theApp.ConfirmNETInstallation(hWnd, lpSource, lpInstall, 2.0f) == TRUE)
		return 1; // OK to continue
	else
		return 0; // Stop the installation
}

//==============================================================================
//
// 	Function Name:	ConfirmNET30Installed()
//
// 	Description:	This function is called to confirm that .NET 30 has been 
//					installed on the target machine
//
// 	Returns:		Non-zero to prevent closure of installation
//
//	Notes:			None
//
//==============================================================================
extern "C" CHAR WINAPI ConfirmNET30Installed(HWND hWnd, LPSTR lpSource, 
											 LPSTR lpSupport, LPSTR lpInstall, 
											 LPSTR lpReserved)
{
	if(theApp.ConfirmNETInstallation(hWnd, lpSource, lpInstall, 3.0f) == TRUE)
		return 1; // OK to continue
	else
		return 0; // Stop the installation
}

