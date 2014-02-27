//==============================================================================
//
// File Name:	registry.h
//
// Description:	This file contains the declarations of the CRegistryPath and
//				CRegistry classes. These classes provide a wrapper for the
//				system registry API.
//
// Author:		Kenneth Moore
//
// Copyright Oceaneering Technologies
//
//==============================================================================
//	Date		Revision    Description
//	12-28-01	1.00		Original Release
//==============================================================================
#if !defined(__REGISTRY_H__)
#define __REGISTRY_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
typedef struct
{
	CString	strDescription;
	CString	strFilename;
	CString	strFolder;
	CString	strVersion;
	CString	strInstallDir;

}STmaxProductInfo;

class CRegistryPath  
{
	private:

	public:
	
							CRegistryPath();
		virtual			   ~CRegistryPath();

		void				Trim(CString& rPath, TCHAR cTrim= _T('\\'));
		int					GetLevels(LPCTSTR lpszPath);
		BOOL				Append(CString& rPath, LPCTSTR lpszAppend);
		BOOL				CheckPath(LPCTSTR lpszPath , CString& rStrKey);
		BOOL				ReplaceRootWithKey(CString &rPath, HKEY &hKey);
		BOOL				RootStrToKey(CString& rPath, HKEY& rKey);
		BOOL				RootKeyToStr(CString& rPath, HKEY& rKey);
		BOOL				IsEqual(const CString Str1, const CString Str2, BOOL bNoCase = TRUE);
		BOOL				ExtractLevel(int iLevel, LPCTSTR lpszPath, CString& rLevel);
		BOOL				ExtractLastLevel(LPCTSTR lpszPath, CString& rLevel);
		BOOL				ExtractPrevLevel(LPCTSTR lpszPath , LPCTSTR lpszLevel, 
										     CString& rPrevious, BOOL bAllowRoot = FALSE);
		BOOL				ExtractNextLevel(LPCTSTR lpszPath , LPCTSTR lpszLevel, 
										     CString& rNext);
		BOOL				RemoveLastLevel(CString& rPath);
};

class CRegistry  
{
	private:

		CRegistryPath		m_Path;
		BOOL				m_bAutoFlush;
		long				m_lError;

	public:
	
							CRegistry();
		virtual			   ~CRegistry();

		CRegistryPath&		GetPathObject(){ return m_Path; }
		long				GetError(){ return m_lError; }
		BOOL				GetAutoFlush(){ return m_bAutoFlush; }
		BOOL				SetAutoFlush(BOOL bAutoFlush);
		

		BOOL				CreateKey(LPCTSTR lpszPath);
		BOOL				OpenKey(LPCTSTR lpszPath, HKEY& rKey, REGSAM rsSecurity = KEY_READ);
		BOOL				DeleteKey(LPCTSTR lpszPath);
		BOOL				FindKey(LPCTSTR lpszPath);
		
		BOOL				FindValue(LPCTSTR lpszKeyName, LPCTSTR lpszValue);
		BOOL				DeleteValue(LPCTSTR lpszPath, LPCTSTR lpszName);
	
		BOOL				CreateValue(LPCTSTR lpszPath, LPCTSTR lpszName, float* lpData);
		BOOL				CreateValue(LPCTSTR lpszPath, LPCTSTR lpszName, double* lpData);
		BOOL				CreateValue(LPCTSTR lpszPath, LPCTSTR lpszName, int* lpData);
		BOOL				CreateValue(LPCTSTR lpszPath, LPCTSTR lpszName, short* lpData);
		BOOL				CreateValue(LPCTSTR lpszPath, LPCTSTR lpszName, long* lpData);
		BOOL				CreateValue(LPCTSTR lpszPath, LPCTSTR lpszName, LPCTSTR lpData);
		BOOL				CreateValue(LPCTSTR lpszPath, LPCTSTR lpszName, DWORD* lpData);
		BOOL				CreateValue(LPCTSTR lpszPath, LPCTSTR lpszName, LPBYTE lpData = NULL, 
										unsigned int uSize = 0, DWORD dwType = REG_BINARY);

		BOOL				SetValue(LPCTSTR lpszPath, LPCTSTR lpszName, float* lpData);
		BOOL				SetValue(LPCTSTR lpszPath, LPCTSTR lpszName, double* lpData);
		BOOL				SetValue(LPCTSTR lpszPath, LPCTSTR lpszName, int* lpData);
		BOOL				SetValue(LPCTSTR lpszPath, LPCTSTR lpszName, short* lpData);
		BOOL				SetValue(LPCTSTR lpszPath, LPCTSTR lpszName, long* lpData);
		BOOL				SetValue(LPCTSTR lpszPath, LPCTSTR lpszName, LPCTSTR lpData);
		BOOL				SetValue(LPCTSTR lpszPath, LPCTSTR lpszName, DWORD* lpData);
		BOOL				SetValue(LPCTSTR lpszPath, LPCTSTR lpszName, LPBYTE lpData = NULL, 
									 unsigned int uSize = 0, DWORD dwType = REG_BINARY);

		BOOL				GetValue(LPCTSTR lpszPath, LPCTSTR lpszName, float* lpData);
		BOOL				GetValue(LPCTSTR lpszPath, LPCTSTR lpszName, double* lpData);
		BOOL				GetValue(LPCTSTR lpszPath, LPCTSTR lpszName, int* lpData);
		BOOL				GetValue(LPCTSTR lpszPath, LPCTSTR lpszName, short* lpData);
		BOOL				GetValue(LPCTSTR lpszPath, LPCTSTR lpszName, long* lpData);
		BOOL				GetValue(LPCTSTR lpszPath, LPCTSTR lpszName, DWORD* lpData);
		BOOL				GetValue(LPCTSTR lpszPath, LPCTSTR lpszName, CString* lpData);
		BOOL				GetValue(LPCTSTR lpszPath, LPCTSTR lpszName, LPCTSTR lpData,
									 unsigned int uLength);
		BOOL				GetValue(LPCTSTR lpszPath, LPCTSTR lpszName, LPBYTE lpData = NULL, 
									 unsigned int uSize = 0, DWORD dwType = REG_BINARY,
									 unsigned int* pActualSize = 0);

		BOOL				GetProductInfo(LPCSTR lpszProduct, STmaxProductInfo& rInfo);

		int					Enum(LPCTSTR lpszPath, CStringArray *paKeys, 
								 CStringArray *paClasses = NULL, BOOL bFullPath = FALSE);
		int					GetSubKeyCount(LPCTSTR lpszPath);

	protected:

		BOOL				CheckResult(long lResult);
		BOOL				Close(HKEY& rKey);
		BOOL				Flush(HKEY& rKey);
		BOOL				IsRoot(HKEY& rKey, LPCTSTR lpszPath = NULL);
		BOOL				IsRoot(LPCTSTR lpszPath);
		BOOL				CreateKey(HKEY& rKey, LPCTSTR lpszPath);
		BOOL				DeleteKey(HKEY& rKey, LPCTSTR lpszPath);
		BOOL				DeleteValue(HKEY& rKey, LPCTSTR lpszName);
		BOOL				FindValue(HKEY& rKey, LPCTSTR lpszName);
		BOOL				OpenSubKey(HKEY& rKey, LPCTSTR lpszPath, HKEY& rSubKey, 
									   REGSAM rsSecurity = KEY_READ);
		int					Enum(HKEY& rRootKey, LPCTSTR lpszPath, CStringArray *paKeys, 
								 CStringArray *paClasses = NULL, BOOL bFullPath = FALSE);
		int					GetSubKeyCount(HKEY& rKey);
};

#endif // !defined(__REGISTRY_H__)
