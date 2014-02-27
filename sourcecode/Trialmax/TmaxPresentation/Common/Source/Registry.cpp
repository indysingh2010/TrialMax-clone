//==============================================================================
//
// File Name:	registry.cpp
//
// Description:	This file contains member functions of the CRegistryPath and
//				CRegistry classes.
//
// Copyright Oceaneering Technologies
//
//==============================================================================
//	Date		Revision    Description
//	12-28-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <registry.h>

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
// 	Function Name:	CRegistryPath::Append()
//
//	Parameters:		rPath - reference to existing path
//					lpszAppend - pointer to path to be appended
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to add a level(s) to the end of a
//					path.
//
//------------------------------------------------------------------------------
BOOL CRegistryPath::Append(CString& rPath, LPCTSTR lpszAppend)
{
    CString strAppend = lpszAppend;

	//	Trim each part
	Trim(rPath);
	Trim(strAppend);

	//	Add the new level(s)
	rPath += _T('\\');
	rPath += strAppend;
	
	return TRUE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistryPath::CheckPath()
//
//	Parameters:		lpszPath    - buffer containing the path specification
//					rKeyString  - key string to search for
//
// 	Return Value:	TRUE if the key is in the path
//
// 	Description:	This function is called to determine if the specified key
//					is part of the path.
//
//------------------------------------------------------------------------------
BOOL CRegistryPath::CheckPath(LPCTSTR lpszPath , CString& rStrKey)
{
	CString strPath;
	CString	strLevel;
	int		iLevels;
	
	strPath = lpszPath;
	Trim(strPath);

	//	How many levels are in the path?
	if((iLevels = GetLevels(strPath)) == 0)
		return FALSE;

	//	Check each level of the path
	for(int i = 0; i < iLevels; i++)
	{
		//	Extract this level
		if(ExtractLevel(i, strPath, strLevel))
		{
			//	Is this the desired key
			if(IsEqual(strLevel, rStrKey))
				return TRUE;
		}
	}
	
	//	Not found
	return FALSE; 
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistryPath::CRegistryPath()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Constructor
//
//------------------------------------------------------------------------------
CRegistryPath::CRegistryPath()
{
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistryPath::~CRegistryPath()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Destructor
//
//------------------------------------------------------------------------------
CRegistryPath::~CRegistryPath()
{
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistryPath::ExtractLastLevel()
//
//	Parameters:		lpszPath - pointer to buffer containing the full path
//					rLevel   - reference to string in which to store the level
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to extract a the last level in 
//					the full path specification.
//
//------------------------------------------------------------------------------
BOOL CRegistryPath::ExtractLastLevel(LPCTSTR lpszPath, CString& rLevel)
{
	CString strPath;
	int		iLevels;

	strPath = lpszPath;
	Trim(strPath);

	//	How many levels are in the path?
	if((iLevels = GetLevels(strPath)) == 0)
		return FALSE;
	else
		return ExtractLevel(iLevels - 1, lpszPath, rLevel);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistryPath::ExtractLevel()
//
//	Parameters:		iLevel - zero based level within the path
//					lpszPath - pointer to buffer containing the full path
//					rLevel   - reference to string in which to store the level
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to extract a specific level from
//					the full path specification.
//
//------------------------------------------------------------------------------
BOOL CRegistryPath::ExtractLevel(int iLevel, LPCTSTR lpszPath, CString& rLevel)
{
	CString strPath;
	int		iLevels;
	int		iCurrent = 0;
	int		i;

	strPath = lpszPath;
	Trim(strPath);

	//	How many levels are in the path?
	if((iLevels = GetLevels(strPath)) == 0)
		return FALSE;

	//	Is the requested level within range?
	if((iLevel < 0) || (iLevel >= iLevels))
		return FALSE;

	//	Line up on the start of the requested level
	for(i = 0; iCurrent < iLevel; i++)
	{
		if(strPath[i] == _T('\\'))  
			iCurrent++;
	}

	//	Extract the requested level
	strPath = strPath.Mid(i);
	if((i = strPath.Find(_T('\\'))) >= 0)
		strPath = strPath.Left(i);
	
	rLevel = strPath;
	return TRUE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistryPath::ExtractNextLevel()
//
//	Parameters:		lpszPath  - pointer to null terminated registry path
//					lpszLevel - pointer to null terminated level string
//					rNext     - reference of string in which to store the result
//					bAllowRoot - TRUE to allow use of the root level
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to get the level of the path that
//					occurs previous to the specified level.
//
//------------------------------------------------------------------------------
BOOL CRegistryPath::ExtractNextLevel(LPCTSTR lpszPath , LPCTSTR lpszLevel, 
								     CString& rNext)
{
	CString strPath;
	CString	strExtract;
	int		iLevels;
	
	rNext.Empty();

	strPath = lpszPath;
	Trim(strPath);

	//	How many levels are in the path?
	if((iLevels = GetLevels(strPath)) < 2)
		return FALSE;
	
	for(int i = 0; i < iLevels - 1; i++)
	{
		//	Extract the next level
		if(ExtractLevel(i, strPath, strExtract))
		{
			//	Is this the specified level?
			if(IsEqual(strExtract, lpszLevel))
			{
				return ExtractLevel(i + 1, strPath, rNext);
			}

		}//if(ExtractLevel(i, strPath, strExtract))

	}//for(int i = 1; i < iLevels; i++)
	
	return FALSE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistryPath::ExtractPrevLevel()
//
//	Parameters:		lpszPath   - pointer to null terminated registry path
//					lpszLevel  - pointer to null terminated level string
//					rPrevious  - reference of string in which to store the result
//					bAllowRoot - TRUE to allow use of the root level
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to get the level of the path that
//					occurs previous to the specified level.
//
//------------------------------------------------------------------------------
BOOL CRegistryPath::ExtractPrevLevel(LPCTSTR lpszPath , LPCTSTR lpszLevel, 
								     CString& rPrevious, BOOL bAllowRoot)
{
	CString strPath;
	CString	strExtract;
	int		iLevels;
	
	rPrevious.Empty();

	strPath = lpszPath;
	Trim(strPath);

	//	How many levels are in the path?
	if((iLevels = GetLevels(strPath)) < 2)
		return FALSE;
	
	for(int i = 1; i < iLevels; i++)
	{
		//	Extract the next level
		if(ExtractLevel(i, strPath, strExtract))
		{
			//	Is this the specified level?
			if(IsEqual(strExtract, lpszLevel))
			{
				//	NOTE: Level numbering starts at zero
				if(i > 1)
					return ExtractLevel(i - 1, strPath, rPrevious);
				else if(bAllowRoot == TRUE)
					return ExtractLevel(0, strPath, rPrevious);
				else
					return FALSE;
			}

		}//if(ExtractLevel(i, strPath, strExtract))

	}//for(int i = 1; i < iLevels; i++)
	
	return FALSE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistryPath::GetLevels()
//
//	Parameters:		lpszPath - pointer to null terminated registry path
//
// 	Return Value:	The number of levels in the path
//
// 	Description:	This function is called to count the number of levels that
//					exist in the specified path.
//
//------------------------------------------------------------------------------
int CRegistryPath::GetLevels(LPCTSTR lpszPath)
{
	CString strPath;
	int		iTotal  = lstrlen(lpszPath);
	int		iCount;
	int		i;
	
	if(iTotal > 0)
	{
		//	Remove leading/trailing slash
		strPath = lpszPath;
		Trim(strPath);

		//	Count the number of levels
		for(iCount = 1 , i = 0; i < iTotal; i++)
		{
			if(strPath.GetAt(i) == _T('\\'))  
				iCount++;
		}
		return iCount;
	}
	else
	{
		return 0;
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistryPath::IsEqual()
//
//	Parameters:		Str1 - first string
//					Str2 - second string
//					bNoCase - TRUE to perform case insensitive comparison
//
// 	Return Value:	TRUE if equal
//
// 	Description:	This function is called to compare two strings.
//
//------------------------------------------------------------------------------
BOOL CRegistryPath::IsEqual(const CString Str1, const CString Str2, BOOL bNoCase)
{
	if(bNoCase)
		return (Str1.CompareNoCase(Str2) == 0);
	else
		return (Str1 == Str2);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistryPath::RemoveLastLevel()
//
//	Parameters:		rPath - reference to path string
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to remove the last level in the path
//
//------------------------------------------------------------------------------
BOOL CRegistryPath::RemoveLastLevel(CString& rPath)
{
	int iSeparator;

	Trim(rPath);

	//	Locate the last backslash
	if((iSeparator = rPath.ReverseFind(_T('\\'))) < 0)
		return TRUE; // The same string returned if only 1 level

	//	Strip the last level
	rPath = rPath.Left(iSeparator);
	Trim(rPath);

	return TRUE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistryPath::ReplaceRootWithKey()
//
//	Parameters:		rPath - reference to registry path
//					rKey  - reference to key
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to get the root key from the 
//					specified path.
//
//	Notes:			The portion of the path that represents the root is 
//					extracted and the subkey path is returned.
//
//------------------------------------------------------------------------------
BOOL CRegistryPath::ReplaceRootWithKey(CString &rPath, HKEY &hKey)
{
	CString strPath;
	CString	strRoot;
	int		iSeparator;
	
	strPath = rPath;
	Trim(strPath);

	//	Look for the root separator
	iSeparator = strPath.Find(_T('\\'));
	if(iSeparator < 0) 
	{
		iSeparator = strPath.GetLength(); // Only includes a root specification
	}

	//	Extract the root text
	strRoot = strPath.Left(iSeparator);

	//	Get the key identified by the root text
	if(RootStrToKey(strRoot, hKey) == FALSE)
		return FALSE;	

	//	Return the remainder of the path
	rPath = strPath.Mid(iSeparator);
	Trim(rPath);

	return TRUE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistryPath::RootKeyToStr()
//
//	Parameters:		rRoot - string form of root key
//					rKey  - key in which to store the root
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to translate the root key to a
//					string path.
//
//------------------------------------------------------------------------------
BOOL CRegistryPath::RootKeyToStr(CString& rPath, HKEY& rKey)
{
	rPath.Empty();

	if(rKey == HKEY_CLASSES_ROOT)
		rPath = _T("HKEY_CLASSES_ROOT");
	else if(rKey == HKEY_CURRENT_CONFIG)
		rPath = _T("HKEY_CURRENT_CONFIG");
	else if(rKey == HKEY_CURRENT_USER)
		rPath = _T("HKEY_CURRENT_USER");
	else if(rKey == HKEY_LOCAL_MACHINE) 
		rPath = _T("HKEY_LOCAL_MACHINE");
	else if(rKey == HKEY_USERS)
		rPath = _T("HKEY_USERS");
	else if(rKey == HKEY_DYN_DATA)
		rPath = _T("HKEY_DYN_DATA");
	else if (rKey == HKEY_PERFORMANCE_DATA)
		rPath = _T("HKEY_PERFORMANCE_DATA");

	return (rPath.IsEmpty() == FALSE);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistryPath::RootStrToKey()
//
//	Parameters:		rRoot - string form of root key
//					rKey  - key in which to store the root
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to translate the root portion of the
//					path string to its equivalent registry key value.
//
//------------------------------------------------------------------------------
BOOL CRegistryPath::RootStrToKey(CString& rPath, HKEY& rKey)
{
	CString strRoot;
	CString strPath;

	rKey = NULL;

	strPath = rPath;
	Trim(strPath);

	//	Get the root portion of the path
	ExtractLevel(0, strPath, strRoot);

	//	Did we find a string root?
	if(strRoot.IsEmpty())
		return FALSE;

	if((IsEqual(strRoot, _T("HKEY_CLASSES_ROOT"))) || 
	   (IsEqual(strRoot, _T("HKCR")))) 
	{
		rKey = HKEY_CLASSES_ROOT;
	}
	if((IsEqual(strRoot, _T("HKEY_CURRENT_CONFIG"))) || 
	   (IsEqual(strRoot, _T("HKCC")))) 
	{
		rKey =  HKEY_CURRENT_CONFIG;
	}
	if((IsEqual(strRoot, _T("HKEY_CURRENT_USER"))) || 
	   (IsEqual(strRoot, _T("HKCU")))) 
	{
		rKey = HKEY_CURRENT_USER;
	}
	if((IsEqual(strRoot, _T("HKEY_LOCAL_MACHINE"))) || 
	   (IsEqual(strRoot, _T("HKLM")))) 
	{
		rKey = HKEY_LOCAL_MACHINE;
	}
	if((IsEqual(strRoot, _T("HKEY_USERS"))) || 
	   (IsEqual(strRoot, _T("HKU")))) 
	{
		rKey = HKEY_USERS;
	}
	if((IsEqual(strRoot, _T("HKEY_DYN_DATA"))) || 
	   (IsEqual(strRoot, _T("HKDD")))) 
	{
		rKey = HKEY_DYN_DATA;
	}
	if((IsEqual(strRoot, _T("HKEY_PERFORMANCE_DATA"))) || 
	   (IsEqual(strRoot, _T("HKPD")))) 
	{
		rKey = HKEY_PERFORMANCE_DATA;
	}

	return (rKey != NULL);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistryPath::Trim()
//
//	Parameters:		rPath - path to trim character from
//					cTrim - character to remove
//
// 	Return Value:	None
//
// 	Description:	This function is called to remove the leading and trailing
//					character(s) from a path.
//
//------------------------------------------------------------------------------
void CRegistryPath::Trim(CString& rPath, TCHAR cTrim)
{
	rPath.TrimLeft(cTrim);
	rPath.TrimRight(cTrim);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::CheckResult()
//
//	Parameters:		lResult - return code from a registry API function
//
// 	Return Value:	TRUE if the operation was successful
//
// 	Description:	This function is called to determine if the registry API
//					return value indicates success.
//
//------------------------------------------------------------------------------
BOOL CRegistry::CheckResult(long lResult)
{
	if(lResult == ERROR_SUCCESS)
	{
		return TRUE;
	}
	else
	{
		//	Store the value so that the user can determine which error occurred
		m_lError = lResult;
		
		return FALSE;
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::Close()
//
//	Parameters:		rKey - reference to key to be closed
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to close the specified key
//
//------------------------------------------------------------------------------
BOOL CRegistry::Close(HKEY &rKey)
{
	//	Should we commit all changes to disk?
	if(m_bAutoFlush)
	{
		if(!Flush(rKey))
			return FALSE;
	} 

	//	Close this key as long as it is not a root key
	if(!IsRoot(rKey))
	{ 
		return CheckResult(::RegCloseKey(rKey));
	}
	else
	{
		return TRUE;
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::CreateKey()
//
//	Parameters:		lpszPath - pointer to string path specification
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to create the specified key
//
//------------------------------------------------------------------------------
BOOL CRegistry::CreateKey(LPCTSTR lpszPath)
{
	CString	strPath = lpszPath;
	HKEY	hRoot;

	//	Don't bother if the key already exists
	if(FindKey(lpszPath))
		return TRUE;

	//	Get the root key
	if(!m_Path.ReplaceRootWithKey(strPath, hRoot))
		return FALSE;

	return CreateKey(hRoot, strPath);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::CreateKey()
//
//	Parameters:		rKey     - reference to open registry key or root key
//					lpszPath - pointer to string path specification
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to create the specified key
//
//------------------------------------------------------------------------------
BOOL CRegistry::CreateKey(HKEY &rKey, LPCTSTR lpszPath)
{
	HKEY	hNewKey;
	long	lResult;
	
	ASSERT(lpszPath);

	lResult = RegCreateKeyEx(rKey, lpszPath, 0, NULL, REG_OPTION_NON_VOLATILE,
							 KEY_CREATE_SUB_KEY|KEY_ENUMERATE_SUB_KEYS, 
							 NULL, &hNewKey, NULL);
	Close(hNewKey);

	return CheckResult(lResult);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::CreateValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to create the specified value
//					and set the data.
//
//------------------------------------------------------------------------------
BOOL CRegistry::CreateValue(LPCTSTR lpszPath, LPCTSTR lpszName, float* lpData)
{
	return SetValue(lpszPath, lpszName, (LPBYTE)lpData, sizeof(float), REG_BINARY);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::CreateValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to create the specified value
//					and set the data.
//
//------------------------------------------------------------------------------
BOOL CRegistry::CreateValue(LPCTSTR lpszPath, LPCTSTR lpszName, short* lpData)
{
	return SetValue(lpszPath, lpszName, (LPBYTE)lpData, sizeof(short), REG_BINARY);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::CreateValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to create the specified value
//					and set the data.
//
//------------------------------------------------------------------------------
BOOL CRegistry::CreateValue(LPCTSTR lpszPath, LPCTSTR lpszName, int* lpData)
{
	return SetValue(lpszPath, lpszName, (LPBYTE)lpData, sizeof(int), REG_BINARY);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::CreateValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//					uSize    - size in bytes of data to be stored
//					dwType	 - registry data type identifier
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to create the specified value
//					and set the data.
//
//------------------------------------------------------------------------------
BOOL CRegistry::CreateValue(LPCTSTR lpszPath, LPCTSTR lpszName, LPBYTE lpData,
							unsigned int uSize, DWORD dwType)
{
	return SetValue(lpszPath, lpszName, lpData, uSize, dwType);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::CreateValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to create the specified value
//					and set the data.
//
//------------------------------------------------------------------------------
BOOL CRegistry::CreateValue(LPCTSTR lpszPath, LPCTSTR lpszName, DWORD* lpData)
{
	return SetValue(lpszPath, lpszName, (LPBYTE)lpData, sizeof(DWORD), REG_DWORD);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::CreateValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to create the specified value
//					and set the data.
//
//------------------------------------------------------------------------------
BOOL CRegistry::CreateValue(LPCTSTR lpszPath, LPCTSTR lpszName, LPCTSTR lpData)
{
	return SetValue(lpszPath, lpszName, (LPBYTE)lpData, _tcslen(lpData), REG_SZ);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::CreateValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to create the specified value
//					and set the data.
//
//------------------------------------------------------------------------------
BOOL CRegistry::CreateValue(LPCTSTR lpszPath, LPCTSTR lpszName, long* lpData)
{
	return SetValue(lpszPath, lpszName, (LPBYTE)lpData, sizeof(long), REG_BINARY);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::CreateValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to create the specified value
//					and set the data.
//
//------------------------------------------------------------------------------
BOOL CRegistry::CreateValue(LPCTSTR lpszPath, LPCTSTR lpszName, double* lpData)
{
	return SetValue(lpszPath, lpszName, (LPBYTE)lpData, sizeof(double), REG_BINARY);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::CRegistry()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Constructor
//
//------------------------------------------------------------------------------
CRegistry::CRegistry()
{
	m_bAutoFlush = FALSE;
	m_lError = ERROR_SUCCESS;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::~CRegistry()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Destructor
//
//------------------------------------------------------------------------------
CRegistry::~CRegistry()
{
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::DeleteKey()
//
//	Parameters:		lpszPath - pointer to string path specification
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to delete the specified key
//
//------------------------------------------------------------------------------
BOOL CRegistry::DeleteKey(LPCTSTR lpszPath)
{
	CString	strPath = lpszPath;
	HKEY	hRootKey;

	//	Get the root key
	if(m_Path.ReplaceRootWithKey(strPath, hRootKey) == FALSE)
		return FALSE;
	
	#ifdef UNICODE
	
		CRegKey atlRegKey;
		atlRegKey.Attach(hRootKey);
		return CheckResult(atlRegKey.RecurseDeleteKey(strPath);
	
	#else
		
		return DeleteKey(hRootKey, strPath);
	
	#endif
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::DeleteKey()
//
//	Parameters:		rKey     - reference to open registry key or root key
//					lpszPath - pointer to string path specification
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to delete the specified key
//
//------------------------------------------------------------------------------
BOOL CRegistry::DeleteKey(HKEY &rKey, LPCTSTR lpszPath)
{
	return CheckResult(RegDeleteKey(rKey, lpszPath));
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::DeleteValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of value to be deleted
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to delete the specified value
//
//------------------------------------------------------------------------------
BOOL CRegistry::DeleteValue(LPCTSTR lpszPath, LPCTSTR lpszName)
{
	CString	strPath = lpszPath;
	HKEY	hRootKey;
	HKEY	hSubKey;
	BOOL	bReturn;

	//	Get the root key
	if(m_Path.ReplaceRootWithKey(strPath, hRootKey) == FALSE)
		return FALSE;
	
	//	Open the key that contains the specified value
	if(!OpenSubKey(hRootKey, strPath, hSubKey, KEY_SET_VALUE ))
		return FALSE;
	
	bReturn = DeleteValue(hSubKey, lpszName);

	Close(hSubKey);
	
	return bReturn;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::DeleteValue()
//
//	Parameters:		rKey     - reference to open registry key or root key
//					lpszName - pointer to name of value to be deleted
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to delete the specified value
//
//------------------------------------------------------------------------------
BOOL CRegistry::DeleteValue(HKEY& rKey, LPCTSTR lpszName)
{
	return CheckResult(RegDeleteValue(rKey, lpszName));
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::Enum()
//
//	Parameters:		rRootKey  - reference to root key
//					lpszPath  - pointer to registry path
//					paKeys	  - pointer to array of key names
//					paClasses - pointer to array of class names
//					bFullPath - TRUE to use full path
//
// 	Return Value:	The number of objects added to the array
//
// 	Description:	This function is called to enumerate the subkeys associated
//					with the specified path.
//
//------------------------------------------------------------------------------
int CRegistry::Enum(HKEY& rRootKey, LPCTSTR lpszPath, CStringArray* paKeys, 
					CStringArray* paClasses, BOOL bFullPath)
{
	HKEY	hSubKey;
	TCHAR	tszKey[MAX_PATH * 2];
	TCHAR	tszClass[MAX_PATH * 2];
	DWORD	dwKeySize;
	DWORD	dwClassSize;
	DWORD	i = 0;
	long	lResult;
	int		iCount = 0;

	//	Open the specified subkey
	if(!OpenSubKey(rRootKey, lpszPath, hSubKey))   
		return FALSE;

	for(i = 0; ; i++)
	{
		dwKeySize  = sizeof(tszKey);
		dwClassSize = sizeof(tszClass);

        lResult = RegEnumKeyEx(hSubKey, i, tszKey, &dwKeySize, NULL,
							   tszClass, &dwClassSize, NULL);

        if(lResult == ERROR_SUCCESS)
        {
			paKeys->Add((LPCTSTR)tszKey);
			if(paClasses)  
				paClasses->Add((LPCTSTR)tszClass);

			iCount++;
		}
		else
		{
			break;
		}

	}

	Close(hSubKey);
	return iCount;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::Enum()
//
//	Parameters:		lpszPath  - pointer to registry path
//					paKeys	  - pointer to array of key names
//					paClasses - pointer to array of class names
//					bFullPath - TRUE to use full path
//
// 	Return Value:	The number of objects added to the array
//
// 	Description:	This function is called to enumerate the subkeys associated
//					with the specified path.
//
//------------------------------------------------------------------------------
int CRegistry::Enum(LPCTSTR lpszPath, CStringArray* paKeys, 
					CStringArray* paClasses, BOOL bFullPath)
{
	CString	strPath = lpszPath;
	HKEY	hRootKey;

	//	Get the root key
	if(m_Path.ReplaceRootWithKey(strPath, hRootKey) == FALSE)
		return FALSE;
	
	return Enum(hRootKey, strPath, paKeys, paClasses, bFullPath);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::FindKey()
//
//	Parameters:		lpszPath - pointer to string path specification
//
// 	Return Value:	TRUE if the key is found
//
// 	Description:	This function is called to determine if the specified key
//					exists
//
//------------------------------------------------------------------------------
BOOL CRegistry::FindKey(LPCTSTR lpszPath)
{
	CString strPath = lpszPath;
	HKEY	hKey;

	m_Path.Trim(strPath);

	if(OpenKey(strPath, hKey))
	{
		Close(hKey);
		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::FindValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of value to be searched for
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to determine if the specified value
//					exists in the path.
//
//------------------------------------------------------------------------------
BOOL CRegistry::FindValue(LPCTSTR lpszPath, LPCTSTR lpszName)
{
	CString strPath = lpszPath;
	HKEY	hKey;
	BOOL	bFound;

	m_Path.Trim(strPath);

	if(OpenKey(strPath, hKey, KEY_QUERY_VALUE))
	{
		bFound  = FindValue(hKey, lpszName);
		Close(hKey);
		return bFound;
	}
	else
	{
		return FALSE;
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::FindValue()
//
//	Parameters:		rKey     - reference to open registry key or root key
//					lpszName - pointer to name of value to be deleted
//
// 	Return Value:	TRUE if the value exists
//
// 	Description:	This function is called to determine if the specified value
//					exists.
//
//------------------------------------------------------------------------------
BOOL CRegistry::FindValue(HKEY& rKey, LPCTSTR lpszName)
{
	return CheckResult(RegQueryValueEx(rKey, lpszName, 0, NULL, NULL, NULL));
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::Flush()
//
//	Parameters:		rKey - reference to key to be flushed
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to flush the specified key. Flushing
//					a key commits pending changes to disk immediately instead of
//					waiting for Window's background processing.
//
//------------------------------------------------------------------------------
BOOL CRegistry::Flush(HKEY &rKey)
{
	return CheckResult(::RegFlushKey(rKey));
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::GetSubKeyCount()
//
//	Parameters:		rKey - reference to key to be checked
//
// 	Return Value:	The number of subkeys
//
// 	Description:	This function is called to determine how many subkeys are
//					associated with the specified key.
//
//------------------------------------------------------------------------------
int CRegistry::GetSubKeyCount(HKEY& rKey)
{
	DWORD dwSubKeys = 0;

	if(CheckResult(RegQueryInfoKey(rKey, NULL, NULL, NULL, &dwSubKeys, NULL, 
								   NULL, NULL, NULL, NULL, NULL, NULL)))
	{
		return (int)dwSubKeys;
	}
	else
	{
		return -1;
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::GetProductInfo()
//
//	Parameters:		lpszProduct - name of the product
//					rInfo - structure in which to store the information
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to retrieve the information stored
//					in the registry for the specified TrialMax 7 product
//
//------------------------------------------------------------------------------
BOOL CRegistry::GetProductInfo(LPCSTR lpszProduct, STmaxProductInfo& rInfo)
{
	CString strKey = "HKEY_LOCAL_MACHINE\\SOFTWARE\\FTI Consulting\\TrialMax 7\\Product\\";

	//	Complete the path to the product key
	ASSERT(lpszProduct);
	ASSERT(lstrlen(lpszProduct) > 0);
	strKey += lpszProduct;

	//	Make sure the key exists
	if(!FindKey(strKey)) return FALSE;

	//	Get the values
	GetValue(strKey, "Folder", &(rInfo.strFolder));
	GetValue(strKey, "Filename", &(rInfo.strFilename));
	GetValue(strKey, "Version", &(rInfo.strVersion));
	GetValue(strKey, "Description", &(rInfo.strDescription));
	GetValue(strKey, "InstallDir", &(rInfo.strInstallDir));

	return TRUE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::GetSubKeyCount()
//
//	Parameters:		lpszPath - pointer to string path specification
//
// 	Return Value:	The number of subkeys
//
// 	Description:	This function is called to determine how many subkeys are
//					associated with the specified path.
//
//------------------------------------------------------------------------------
int CRegistry::GetSubKeyCount(LPCTSTR lpszPath)
{
	CString	strPath = lpszPath;
	HKEY	hRootKey;
	HKEY	hSubKey;
	int		iCount;

	//	Get the root key
	if(m_Path.ReplaceRootWithKey(strPath, hRootKey) == FALSE)
		return -1;
	
	//	Open the specified key
	if(OpenSubKey(hRootKey, strPath, hSubKey) == FALSE)
		return -1;

	//	Get the subkey count
	iCount = GetSubKeyCount(hSubKey);
	
	Close(hSubKey);

	return iCount;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::GetValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data buffer
//					uSize    - size in bytes of data to be stored
//					dwType	 - registry data type identifier
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to get the specified value
//
//------------------------------------------------------------------------------
BOOL CRegistry::GetValue(LPCTSTR lpszPath, LPCTSTR lpszName, LPBYTE lpData,
						 unsigned int uSize, DWORD dwType, unsigned int* pActualSize)
{
	CString	strPath = lpszPath;
	HKEY	hRootKey;
	HKEY	hSubKey;
	long	lResult;
	DWORD	dwRegType;
	DWORD	dwBytes;
	LPBYTE	lpBuffer;
	BOOL	bReturn;

	//	Initialize the actual size
	if(pActualSize) *pActualSize = 0;

	//	Get the root key
	if(m_Path.ReplaceRootWithKey(strPath, hRootKey) == FALSE)
		return FALSE;
	
	//	Open the specified key
	if(OpenSubKey(hRootKey, strPath, hSubKey, KEY_WRITE | KEY_READ) == FALSE)
		return FALSE;

	//	Get the length and type of data stored in the registry
	lResult = ::RegQueryValueEx(hSubKey, lpszName, NULL, &dwRegType, NULL, &dwBytes);
	if(!CheckResult(lResult))
		return FALSE;

	//	Make sure the types match
	if(dwRegType != dwType)
		return FALSE;
	
	//	Allocate a buffer to read the data from the registry
	if((lpBuffer = new BYTE[dwBytes + 1]) == NULL)
		return FALSE;
	else
		memset(lpBuffer, 0, dwBytes + 1);

	//	Get the data from the registry
	lResult = ::RegQueryValueEx(hSubKey, lpszName, NULL, &dwRegType, lpBuffer, &dwBytes); 
	
	if((bReturn = CheckResult(lResult)) == TRUE)
	{
		if(dwType == REG_SZ)
		{
			//	Is the data buffer actually a CString pointer?
			if(uSize == 0)
				(*(CString*)lpData) = (LPCTSTR)lpBuffer;
			else
				lstrcpyn((LPTSTR)lpData, (LPCTSTR)lpBuffer, uSize);
		}
		else
		{
			::CopyMemory(lpData, lpBuffer, min(dwBytes,uSize));
		}
	}

	//	Store the actual size of the buffer
	if(pActualSize != 0) *pActualSize = (unsigned int)dwBytes;

	//	Clean up
	delete [] lpBuffer;
	Close(hSubKey);	

	return bReturn;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::GetValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to get the specified value
//
//------------------------------------------------------------------------------
BOOL CRegistry::GetValue(LPCTSTR lpszPath, LPCTSTR lpszName, DWORD* lpData)
{
	return GetValue(lpszPath, lpszName, (LPBYTE)lpData, sizeof(DWORD), REG_DWORD);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::GetValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to get the specified value
//
//------------------------------------------------------------------------------
BOOL CRegistry::GetValue(LPCTSTR lpszPath, LPCTSTR lpszName, long* lpData)
{
	return GetValue(lpszPath, lpszName, (LPBYTE)lpData, sizeof(long), REG_BINARY);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::GetValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to get the specified value
//
//------------------------------------------------------------------------------
BOOL CRegistry::GetValue(LPCTSTR lpszPath, LPCTSTR lpszName, short* lpData)
{
	return GetValue(lpszPath, lpszName, (LPBYTE)lpData, sizeof(short), REG_BINARY);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::GetValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to get the specified value
//
//------------------------------------------------------------------------------
BOOL CRegistry::GetValue(LPCTSTR lpszPath, LPCTSTR lpszName, int* lpData)
{
	return GetValue(lpszPath, lpszName, (LPBYTE)lpData, sizeof(int), REG_BINARY);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::GetValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to get the specified value
//
//------------------------------------------------------------------------------
BOOL CRegistry::GetValue(LPCTSTR lpszPath, LPCTSTR lpszName, double* lpData)
{
	return GetValue(lpszPath, lpszName, (LPBYTE)lpData, sizeof(double), REG_BINARY);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::GetValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to get the specified value
//
//------------------------------------------------------------------------------
BOOL CRegistry::GetValue(LPCTSTR lpszPath, LPCTSTR lpszName, float* lpData)
{
	return GetValue(lpszPath, lpszName, (LPBYTE)lpData, sizeof(float), REG_BINARY);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::GetValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to get the specified value
//
//------------------------------------------------------------------------------
BOOL CRegistry::GetValue(LPCTSTR lpszPath, LPCTSTR lpszName, CString* lpData)
{
	return GetValue(lpszPath, lpszName, (LPBYTE)lpData, 0, REG_SZ);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::GetValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//					uLength  - maximum length of data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to get the specified value
//
//------------------------------------------------------------------------------
BOOL CRegistry::GetValue(LPCTSTR lpszPath, LPCTSTR lpszName, LPCTSTR lpData,
						 unsigned int uLength)
{
	return GetValue(lpszPath, lpszName, (LPBYTE)lpData, uLength, REG_SZ);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::IsRoot()
//
//	Parameters:		rKey - reference to key to be checked
//					lpszPath - pointer to string path specification
//
// 	Return Value:	TRUE if the key is a root key
//
// 	Description:	This function is called to determine if the specified key
//					is a root key.
//
//------------------------------------------------------------------------------
BOOL CRegistry::IsRoot(HKEY &rKey, LPCTSTR lpszPath)
{
	// If not one of the system roots return FALSE
	if(!((rKey == HKEY_CLASSES_ROOT)  || 
		 (rKey == HKEY_CURRENT_CONFIG) || 
		 (rKey == HKEY_CURRENT_USER) ||
		 (rKey == HKEY_LOCAL_MACHINE) || 
		 (rKey == HKEY_PERFORMANCE_DATA) || 
		 (rKey == HKEY_USERS) ||
		 (rKey == HKEY_DYN_DATA)))   return FALSE;

	if((lpszPath == NULL) || (_tcslen(lpszPath) == 0))
		return TRUE; //pointer is NULL : this is root
	
	return FALSE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::OpenKey()
//
//	Parameters:		lpszPath   - pointer to string path specification
//					rKey	   - reference to key to be opened
//					rsSecurity - security attributes
//					
//
// 	Return Value:	TRUE if the key is a root key
//
// 	Description:	This function is called to determine if the specified key
//					is a root key.
//
//------------------------------------------------------------------------------
BOOL CRegistry::OpenKey(LPCTSTR lpszPath, HKEY& rKey, REGSAM rsSecurity)
{
	CString	strPath = lpszPath;
	HKEY	hRootKey;

	//	Get the root key
	if(m_Path.ReplaceRootWithKey(strPath, hRootKey) == FALSE)
		return FALSE;
	
	//	Now open the subkey
	return OpenSubKey(hRootKey, strPath, rKey, rsSecurity);	
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::OpenSubKey()
//
//	Parameters:		rKey       - reference to parent key
//					lpszPath   - pointer to string path specification
//					rSubKey	   - reference to key to be opened
//					rsSecurity - security attributes
//					
//
// 	Return Value:	TRUE if the key is a root key
//
// 	Description:	This function is called to determine if the specified key
//					is a root key.
//
//------------------------------------------------------------------------------
BOOL CRegistry::OpenSubKey(HKEY& rKey, LPCTSTR lpszPath, HKEY& rSubKey, 
						   REGSAM rsSecurity)
{
	//	Is this a root key?
	if(lstrlen(lpszPath) == 0)
	{
		//	Nothing to open
		rSubKey = rKey;
		return TRUE;
	}
	else
	{
		//	Open the specified key
		return CheckResult(RegOpenKeyEx(rKey, lpszPath, 0, rsSecurity, &rSubKey));
	}
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::IsRoot()
//
//	Parameters:		lpszPath - pointer to string path specification
//
// 	Return Value:	TRUE if the key is a root key
//
// 	Description:	This function is called to determine if the specified key
//					is a root key.
//
//------------------------------------------------------------------------------
BOOL CRegistry::IsRoot(LPCTSTR lpszPath)
{
	CString strPath = lpszPath;
	m_Path.Trim(strPath);
	
	if(m_Path.IsEqual(strPath, _T("HKEY_CLASSES_ROOT")))
		return TRUE;
	if(m_Path.IsEqual(strPath, _T("HKEY_CURRENT_CONFIG")))
		return TRUE;
	if(m_Path.IsEqual(strPath, _T("HKEY_CURRENT_USER")))
		return TRUE;
	if(m_Path.IsEqual(strPath, _T("HKEY_LOCAL_MACHINE")))
		return TRUE;
	if(m_Path.IsEqual(strPath, _T("HKEY_USERS")))
		return TRUE;
	if(m_Path.IsEqual(strPath, _T("HKEY_DYN_DATA")))
		return TRUE;
	if(m_Path.IsEqual(strPath, _T("HKEY_PERFORMANCE_DATA")))
		return TRUE;
	return FALSE;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::SetAutoFlush()
//
//	Parameters:		bAutoFlush - TRUE to enable auto flushing
//
// 	Return Value:	The previous Auto Flush state
//
// 	Description:	This function is called to set the AutoFlush flag
//
//------------------------------------------------------------------------------
BOOL CRegistry::SetAutoFlush(BOOL bAutoFlush)
{
	BOOL bOldState = m_bAutoFlush;
	m_bAutoFlush = bAutoFlush;
	return bOldState;
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::SetValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to set the specified value
//
//------------------------------------------------------------------------------
BOOL CRegistry::SetValue(LPCTSTR lpszPath, LPCTSTR lpszName, int* lpData)
{
	return SetValue(lpszPath, lpszName, (LPBYTE)lpData, sizeof(int), REG_BINARY);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::SetValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to set the specified value
//
//------------------------------------------------------------------------------
BOOL CRegistry::SetValue(LPCTSTR lpszPath, LPCTSTR lpszName, LPCTSTR lpData)
{
	return SetValue(lpszPath, lpszName, (LPBYTE)lpData, _tcslen(lpData), REG_SZ);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::SetValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to set the specified value
//
//------------------------------------------------------------------------------
BOOL CRegistry::SetValue(LPCTSTR lpszPath, LPCTSTR lpszName, short* lpData)
{
	return SetValue(lpszPath, lpszName, (LPBYTE)lpData, sizeof(short), REG_BINARY);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::SetValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to set the specified value
//
//------------------------------------------------------------------------------
BOOL CRegistry::SetValue(LPCTSTR lpszPath, LPCTSTR lpszName, double* lpData)
{
	return SetValue(lpszPath, lpszName, (LPBYTE)lpData, sizeof(double), REG_BINARY);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::SetValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to set the specified value
//
//------------------------------------------------------------------------------
BOOL CRegistry::SetValue(LPCTSTR lpszPath, LPCTSTR lpszName, float* lpData)
{
	return SetValue(lpszPath, lpszName, (LPBYTE)lpData, sizeof(float), REG_BINARY);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::SetValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to set the specified value
//
//------------------------------------------------------------------------------
BOOL CRegistry::SetValue(LPCTSTR lpszPath, LPCTSTR lpszName, long* lpData)
{
	return SetValue(lpszPath, lpszName, (LPBYTE)lpData, sizeof(long), REG_BINARY);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::SetValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to set the specified value
//
//------------------------------------------------------------------------------
BOOL CRegistry::SetValue(LPCTSTR lpszPath, LPCTSTR lpszName, DWORD* lpData)
{
	return SetValue(lpszPath, lpszName, (LPBYTE)lpData, sizeof(DWORD), REG_DWORD);
}

//------------------------------------------------------------------------------
//
// 	Function Name:	CRegistry::SetValue()
//
//	Parameters:		lpszPath - pointer to string path specification
//					lpszName - pointer to name of the value
//					lpData   - pointer to data to be stored
//					uSize    - size in bytes of data to be stored
//					dwType	 - registry data type identifier
//
// 	Return Value:	TRUE if successful
//
// 	Description:	This function is called to set the specified value
//
//------------------------------------------------------------------------------
BOOL CRegistry::SetValue(LPCTSTR lpszPath, LPCTSTR lpszName, LPBYTE lpData,
						 unsigned int uSize, DWORD dwType)
{
	CString	strPath = lpszPath;
	HKEY	hRootKey;
	HKEY	hNewKey;
	long	lResult;

	//	Get the root key
	if(m_Path.ReplaceRootWithKey(strPath, hRootKey) == FALSE)
		return FALSE;
	
	//	Open the specified key
	if(OpenSubKey(hRootKey, strPath, hNewKey, KEY_WRITE | KEY_READ) == FALSE)
		return FALSE;

	//	Set the data
	lResult = ::RegSetValueEx(hNewKey, lpszName, 0, dwType, 
							  (const LPBYTE)lpData, (DWORD)uSize);

	Close(hNewKey);

	return CheckResult(lResult);
}





