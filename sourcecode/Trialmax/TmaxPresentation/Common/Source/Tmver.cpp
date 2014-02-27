//==============================================================================
//
// File Name:	tmver.cpp
//
// Description:	This file contains member functions of the CTMVersion class
//
// Copyright FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	11-24-04	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
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
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMVersion::Clear()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	This function is called to clear the existing version
//					information
//
//------------------------------------------------------------------------------
void CTMVersion::Clear()
{
	m_iMajor = 0;
	m_iMinor = 0;
	m_iUpdate = 0;
	m_iBuild = 0;
	m_bFileExists = FALSE;
	m_bRegistered = FALSE;
	m_strClsId = "";
	m_strName = "";
	m_strDescription = "";
	m_strFileSpec = "";
	m_strTextVer = "";
	m_strShortTextVer = "";
	m_strBuildDate = "";
	m_fileVersion.Clear();
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMVersion::CTMVersion()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Constructor
//
//------------------------------------------------------------------------------
CTMVersion::CTMVersion()
{
	//	Initialize the class members
	Clear();
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMVersion::~CTMVersion()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Destructor
//
//------------------------------------------------------------------------------
CTMVersion::~CTMVersion()
{
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMVersion::GetLocation()
//
//	Parameters:		rLocation - reference to CString in which to store location
//
// 	Return Value:	pointer to the string passed by the caller
//
// 	Description:	This function is called to get a description of the location
//					where the file is stored.
//
//	Notes:			This function is used to get a formatted location 
//					description best for presenting the information to the user
//
//------------------------------------------------------------------------------
LPCSTR CTMVersion::GetLocation(CString& rLocation) 
{
	rLocation.Empty();

	//	Is this an ActiveX control?
	if(m_strClsId.GetLength() > 0)
	{
		//	Is the control properly registered?
		if(GetRegistered() == FALSE)
			rLocation = "NOT REGISTERED";
	}

	//	Do we still need to set the location?
	if(rLocation.GetLength() == 0)
	{
		//	Do we have a file path?
		if(lstrlen(GetFileSpec()) > 0)
		{
			rLocation = GetFileSpec();
			rLocation.MakeLower();

			if(GetFileExists() == FALSE)
				rLocation = ("NOT FOUND -> " + rLocation);
			
		}
		else
		{
			rLocation = "NO FILE SPECIFICATION";
		}

	}// if(rLocation.GetLength() == 0)

	return rLocation;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMVersion::GetPackedVer()
//
//	Parameters:		None
//
// 	Return Value:	The packed numeric version identifier associated with this
//					class instance.
//
// 	Description:	Called to get a composite numeric version identifier 
//					suitable for comparison
//
//	Notes:			None
//
//------------------------------------------------------------------------------
long CTMVersion::GetPackedVer()
{
	return GetPackedVer(m_iMajor, m_iMinor, m_iUpdate);
}
 
//------------------------------------------------------------------------------
//
// 	Function Name:	CTMVersion::GetPackedVer()
//
//	Parameters:		iMajor - the major version identifier
//					iMinor - the minor version identifier
//					iUpdate - the update version identifier
//
// 	Return Value:	The packed numeric version identifier
//
// 	Description:	Called to get a composite numeric version identifier 
//					suitable for comparison
//
//	Notes:			None
//
//------------------------------------------------------------------------------
long CTMVersion::GetPackedVer(int iMajor, int iMinor, int iUpdate)
{
	return ((long)((iMajor * 1000) + (iMinor * 100) + iUpdate));
}
 
//------------------------------------------------------------------------------
//
// 	Function Name:	CTMVersion::GetPackedVer()
//
//	Parameters:		lpszVersion - the version identifier string
//
// 	Return Value:	The packed numeric version identifier
//
// 	Description:	Called to get a composite numeric version identifier 
//					suitable for comparison
//
//	Notes:			None
//
//------------------------------------------------------------------------------
long CTMVersion::GetPackedVer(LPCSTR lpszVersion)
{
	int iMajor = 0;
	int	iMinor = 0;
	int	iUpdate = 0;

	if(SplitVersion(lpszVersion, iMajor, iMinor, iUpdate) == TRUE)
		return GetPackedVer(iMajor, iMinor, iUpdate);
	else
		return 0;
}
 
//------------------------------------------------------------------------------
//
// 	Function Name:	CTMVersion::GetRegisteredPath()
//
//	Parameters:		lpszClsId - Class identifier for the ActiveX control
//					rPath - string in which to store the registered path
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to get the path to the ActiveX
//					control stored in the system registry
//
//------------------------------------------------------------------------------
BOOL CTMVersion::GetRegisteredPath(LPCSTR lpszClsId, CString& rPath)
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
			CTMToolbox::GetLongPath(szPath, rPath);
			rPath.MakeLower();
		}

		RegCloseKey(hKey);
	
	}

	return (rPath.GetLength() > 0);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMVersion::InitFromClsId()
//
//	Parameters:		clsId - system class identifier
//					lpszDescription - Description used for the object
//					lpszFileSpec - the fully qualified path to the source file
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to initialize the object's members
//					using the class identifier specified by the caller
//
//------------------------------------------------------------------------------
BOOL CTMVersion::InitFromClsId(LPCSTR lpszName, LPCSTR lpszDescription, CLSID clsId)
{
	LPOLESTR	lpGUID;
	char		szCLSID[128];

	//	Convert the GUID to an OLE string
	StringFromCLSID(clsId, &lpGUID);

	//	Convert the OLE string to an ANSI string
	WideCharToMultiByte(CP_ACP, 0, lpGUID, -1, szCLSID,	sizeof(szCLSID), 0, 0);

	// Free memory used by StringFromCLSID
	CoTaskMemFree(lpGUID);

	return InitFromClsId(lpszName, lpszDescription, szCLSID);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMVersion::InitFromClsId()
//
//	Parameters:		lpszClsId - Class identifier assigned to the object
//					lpszDescription - Description used for the object
//					lpszFileSpec - the fully qualified path to the source file
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to initialize the object's members
//					using the class identifier specified by the caller
//
//------------------------------------------------------------------------------
BOOL CTMVersion::InitFromClsId(LPCSTR lpszName, LPCSTR lpszDescription, LPCSTR lpszClsId)
{
	CString	strVersion;

	//	Make sure the members have been cleared
	Clear();

	//	Set the name and description
	SetName(lpszName);
	SetDescription(lpszDescription);

	//	Do we have a valid file specification?
	ASSERT(lpszClsId != 0);
	if(lpszClsId == 0) return FALSE;
	ASSERT(lstrlen(lpszClsId) > 0);
	if(lstrlen(lpszClsId) == 0) return FALSE;

	//	Store the new class identifier
	m_strClsId = lpszClsId;

	//	Is the control registered?
	if((m_bRegistered = GetRegisteredPath(lpszClsId, m_strFileSpec)) == FALSE)
		return FALSE;

	//	Initialize using the registered path
	return InitFromFile(lpszName, lpszDescription, m_strFileSpec, FALSE);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMVersion::InitFromFile()
//
//	Parameters:		lpszName - Name assigned to the object
//					lpszDescription - Description used for the object
//					lpszFileSpec - the fully qualified path to the source file
//					bClear - TRUE to clear the members before retrieving new values
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to initialize the object's members
//					using the file specified by the caller
//
//------------------------------------------------------------------------------
BOOL CTMVersion::InitFromFile(LPCSTR lpszName, LPCSTR lpszDescription, 
							  LPCSTR lpszFileSpec, BOOL bClear)
{
	CString	strVersion;

	//	Clear the members if requested
	if(bClear == TRUE)
		Clear();

	//	Set the name and description
	SetName(lpszName);
	SetDescription(lpszDescription);

	//	Do we have a valid file specification?
	ASSERT(lpszFileSpec != 0);
	if(lpszFileSpec == 0) return FALSE;
	ASSERT(lstrlen(lpszFileSpec) > 0);
	if(lstrlen(lpszFileSpec) == 0) return FALSE;

	//	Store the new path
	m_strFileSpec = lpszFileSpec;

	//	Does the file exist?
	if((m_bFileExists = CTMToolbox::FindFile(lpszFileSpec)) == FALSE)
		return FALSE;

	//	Try to extract the version information from the file's resources
	m_fileVersion.ReadVersionInfo(m_strFileSpec);
	if(m_fileVersion.IsValid() == FALSE) return FALSE;

	strVersion = m_fileVersion.GetFileVersionString();
	if(strVersion.GetLength() == 0) return FALSE;

	//	Initialize the members using the version string
	if(InitFromString(strVersion) == FALSE) return FALSE;

	return TRUE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMVersion::InitFromString()
//
//	Parameters:		lpszVersion - the version string used for initialization
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to initialize the object's members
//					using the specified 3 or 4 part version string
//
//------------------------------------------------------------------------------
BOOL CTMVersion::InitFromString(LPCSTR lpszString, LPCSTR lpszDelimiters)
{
	char*	pToken = 0;
	char*	pNext = 0;
	char	szVer[512];
	char	szDelimiters[32];
	int		iMajor = 0;
	int		iMinor = 0;
	int		iUpdate = 0;
	int		iBuild = 0;

	//	NOTE:	We leave it up to the caller to clear the existing version
	//			information because this function may be getting called from
	//			one of our internal initialization functions

	//	Do we have a valid file specification?
	ASSERT(lpszString != 0);
	if(lpszString == 0) return FALSE;
	ASSERT(lstrlen(lpszString) > 0);
	if(lstrlen(lpszString) == 0) return FALSE;

	//	Copy the version to our working buffer
	lstrcpyn(szVer, lpszString, sizeof(szVer));

	//	Use default delimiters if not specified by the caller
	if((lpszDelimiters == 0) || (lstrlen(lpszDelimiters) == 0))
		lstrcpy(szDelimiters, ".");
	else
		lstrcpyn(szDelimiters, lpszDelimiters, sizeof(szDelimiters));

	while(1)
	{
		//	Get the major version identifier
		pToken = strtok_s(szVer, szDelimiters, &pNext);
		if((pToken != 0) && (lstrlen(pToken) > 0))
			iMajor = atoi(pToken);
		else
			break;

		//	Get the minor version identifier
		pToken = strtok_s(NULL, szDelimiters, &pNext);
		if((pToken != 0) && (lstrlen(pToken) > 0))
			iMinor = atoi(pToken);
		else
			break;

		//	Get the update version identifier
		pToken = strtok_s(NULL, szDelimiters, &pNext);
		if((pToken != 0) && (lstrlen(pToken) > 0))
			iUpdate = atoi(pToken);
		else
			break;

		//	Get the build version identifier
		pToken = strtok_s(NULL, szDelimiters, &pNext);
		if((pToken != 0) && (lstrlen(pToken) > 0))
			iBuild = atoi(pToken);
		else
			break;

		//	We're done
		break;

	}// while(1)

	//	We must at least have a valid major identifier
	if(iMajor <= 0) return FALSE;

	//	Update the class members
	m_iMajor  = iMajor;
	m_iMinor  = iMinor;
	m_iUpdate = iUpdate;
	m_iBuild  = iBuild;

	//	Set the text descriptors
	SetVersionText();

	return TRUE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMVersion::SetFileSpec()
//
//	Parameters:		lpszFileSpec - the fully qualified path to the source file
//
// 	Return Value:	None
//
// 	Description:	This function is called to set the path to the file 
//					file associated with this version descriptor
//
//------------------------------------------------------------------------------
void CTMVersion::SetFileSpec(LPCSTR lpszFileSpec)
{
	if(lpszFileSpec != 0)
		m_strFileSpec = lpszFileSpec;
	else
		m_strFileSpec = "";
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMVersion::SetVersionText()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	This function is called to set the object's text descriptors
//					using the current version identifier values
//
//------------------------------------------------------------------------------
void CTMVersion::SetVersionText()
{
	int				iMonth = 0;
	int				iDay = 0;
	int				iYear = 0;
	COleDateTime	dtNow = COleDateTime::GetCurrentTime();

	//	Do we have a valid Major identifier?
	if(m_iMajor > 0)
	{
		m_strTextVer.Format("%d.%d.%d.%d", m_iMajor, m_iMinor, m_iUpdate, m_iBuild);
		m_strShortTextVer.Format("%d.%d.%d", m_iMajor, m_iMinor, m_iUpdate);

		//	Is this a valid TrialMax build date?
		if(m_iBuild >= TMVERSION_MIN_BUILD_DATE)
		{
			iYear = m_iBuild % 10;
			iDay = ((m_iBuild - iYear) / 10) % 100;
			iMonth = (m_iBuild - (iDay * 10) - iYear) / 1000;
			
			iYear += 2000;
			while((iYear + 10) <= dtNow.GetYear())
				iYear += 10;
			
			m_strBuildDate.Format("%.02d-%.02d-%d", iMonth, iDay, iYear);
		}
		else
		{
			m_strBuildDate.Empty();
		}

	}
	else
	{
		m_strTextVer.Empty();
		m_strShortTextVer.Empty();
		m_strBuildDate.Empty();
	}

}

//------------------------------------------------------------------------------
//
// 	Function Name:	CTMVersion::SplitVersion()
//
//	Parameters:		None
//
// 	Return Value:	TRUE if at least a valid major identifier is found
//
// 	Description:	This function is called to split the specified version 
//					string into its numeric components.
//
//------------------------------------------------------------------------------
BOOL CTMVersion::SplitVersion(LPCSTR lpszVer, int& rMajor, int& rMinor, int& rUpdate, char cDelimiter)
{
	int		iMajor = 0;
	int		iMinor = 0;
	int		iUpdate = 0;
	char*	pToken = NULL;
	char	szBuffer[256];

	//	Copy to a working buffer so we can separate the components
	lstrcpyn(szBuffer, lpszVer, sizeof(szBuffer));

	//	Get the major version identifier
	if((pToken = strchr(szBuffer, cDelimiter)) != NULL)
	{
		*pToken = '\0';
		
		if((iMajor = atoi(szBuffer)) > 0)
		{
			lstrcpy(szBuffer, (pToken + sizeof(char)));

			if((pToken = strchr(szBuffer, cDelimiter)) != NULL)
			{
				*pToken = '\0';
				iMinor = atoi(szBuffer);
				lstrcpy(szBuffer, (pToken + sizeof(char)));

				if((pToken = strchr(szBuffer, cDelimiter)) != NULL)
					*pToken = '\0';

				if(lstrlen(szBuffer) > 0)
					iUpdate = atoi(szBuffer);
			}
			else if(lstrlen(szBuffer) > 0)
			{
				iMinor = atoi(szBuffer);
			}

			rMajor  = iMajor;
			rMinor  = iMinor;
			rUpdate = iUpdate;
			
		}// if((iMajor = atoi(szBuffer)) > 0)

	}// if((pToken = strchr(szBuffer, cDelimiter)) != NULL)

	return (iMajor > 0);
}
	
