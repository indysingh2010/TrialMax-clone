//==============================================================================
//
// File Name:	pathsplit.cpp
//
// Description:	This file contains the implementation of the CPathSplitter class.
//
// Author:		Kenneth Moore
//
// Copyright Oceaneering International
//
//==============================================================================
//	Date		Revision    Description
//	05-13-2006	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <pathsplit.h>

//-----------------------------------------------------------------------------
//	DEFINES
//-----------------------------------------------------------------------------
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

//-----------------------------------------------------------------------------
//	GLOBALS
//-----------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------

//==============================================================================
//
// 	Function Name:	CPathSplitter::CPathSplitter()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Constructor
//
//==============================================================================
CPathSplitter::CPathSplitter(LPCTSTR lpszPath)
{
    memset(m_tszComputer, 0, sizeof(m_tszComputer));
    memset(m_tszShare, 0, sizeof(m_tszShare));
	memset(m_tszPath, 0, sizeof(m_tszPath));
    memset(m_tszDrive, 0, sizeof(m_tszDrive));
    memset(m_tszFolder, 0, sizeof(m_tszFolder));
    memset(m_tszFilename, 0, sizeof(m_tszFilename));
    memset(m_tszExtension, 0, sizeof(m_tszExtension));
    m_strRequest = "";

    if((lpszPath != NULL) && (lstrlen(lpszPath) > 0))
		Split(lpszPath);
}

//==============================================================================
//
// 	Function Name:	CPathSplitter::~CPathSplitter()
//
//	Parameters:		None
//
// 	Return Value:	None
//
// 	Description:	Destructor
//
//==============================================================================
CPathSplitter::~CPathSplitter()
{
}

//==============================================================================
//
// 	Function Name:	CPathSplitter::GetFileName()
//
//	Parameters:		bIncludeExtension - TRUE to include the extension
//
// 	Return Value:	The file name
//
// 	Description:	Called to get the filename
//
//==============================================================================
CString CPathSplitter::GetFileName(BOOL bIncludeExtension)
{
	if(!bIncludeExtension)
	{
		return m_tszFilename;
	}
	else
	{
		m_strRequest.Format("%s%s", m_tszFilename, m_tszExtension);
		return m_strRequest;
	}

}

//==============================================================================
//
// 	Function Name:	CPathSplitter::GetFolder()
//
//	Parameters:		bFullPath - TRUE to get complete folder path, FALSE for 
//								folder without driver or UNC volumn
//
// 	Return Value:	The folder path 
//
// 	Description:	Called to get the folder
//
//==============================================================================
CString CPathSplitter::GetFolder(BOOL bFullPath)
{
	if(!bFullPath)
	{
		return m_tszFolder;
	}
	else
	{
		if(IsUNC())
		{
			m_strRequest.Format("\\\\%s\\%s%s", m_tszComputer, m_tszShare, m_tszFolder);
		}
		else
		{
			m_strRequest.Format("%s%s", GetDrive(), m_tszFolder);
		}
		
		return m_strRequest;
	}

}

//==============================================================================
//
// 	Function Name:	CPathSplitter::MsgBox()
//
//	Parameters:		hWnd - handle to the parent window
//					lpszTitle - title to be displayed in the message box
//
// 	Return Value:	None
//
// 	Description:	Called to display the values in a standard message box
//
//==============================================================================
void CPathSplitter::MsgBox(HWND hWnd, LPCTSTR lpszTitle)
{
    CString Msg = "";

	Msg += "Path: ";
	Msg += m_tszPath;
	Msg += "\n";

	Msg += "Drive: ";
	Msg += m_tszDrive;
	Msg += "\n";

	Msg += "Folder: ";
	Msg += m_tszFolder;
	Msg += "\n";

	Msg += "Filename: ";
	Msg += m_tszFilename;
	Msg += "\n";

	Msg += "Extension: ";
	Msg += m_tszExtension;
	Msg += "\n\n";

	Msg += "Is UNC? : ";
	Msg += IsUNC() ? "TRUE" : "FALSE";
	Msg += "\n";

 	Msg += "Computer: ";
	Msg += m_tszComputer;
	Msg += "\n";

 	Msg += "Share: ";
	Msg += m_tszShare;
	Msg += "\n\n";

 	Msg += "Full Filename: ";
	Msg += GetFileName(TRUE);
	Msg += "\n";

 	Msg += "Folder Path: ";
	Msg += GetFolder(TRUE);
	Msg += "\n";

	MessageBox(hWnd, Msg, lpszTitle, MB_ICONINFORMATION | MB_OK);	
}

//==============================================================================
//
// 	Function Name:	CPathSplitter::Split()
//
//	Parameters:		lpszPath - the source path
//
// 	Return Value:	TRUE if successful
//
// 	Description:	Called to split the path into its component parts
//
//==============================================================================
BOOL CPathSplitter::Split(LPCTSTR lpszPath)
{
	int i = 0;
	
    if(lpszPath == NULL) return FALSE;
	if(lstrlen(lpszPath) == 0) return FALSE;

    // Copy the path
    lstrcpyn(m_tszPath, lpszPath, sizeof(m_tszPath));
 
    // Split the given path
    _tsplitpath_s(m_tszPath, 
				  m_tszDrive, sizeof(m_tszDrive),
				  m_tszFolder, sizeof(m_tszFolder), 
				  m_tszFilename, sizeof(m_tszFilename), 
				  m_tszExtension, sizeof(m_tszExtension));

	if(IsUNC())
	{
		//	Strip the leading / \ characters from the folder
		while((m_tszFolder[0] == '\\') || (m_tszFolder[0] == '/'))
			lstrcpy(m_tszFolder, &(m_tszFolder[1]));

		// Get the computer name
		lstrcpyn(m_tszComputer, m_tszFolder, sizeof(m_tszComputer));
		for(i = 0; i < lstrlen(m_tszComputer); i++)
		{
			//	Null terminate on the first occurance of a path delimiter
			if((m_tszComputer[i] == '\\') || (m_tszComputer[i] == '/'))
			{
				m_tszComputer[i] = '\0';
				break;
			}
		}
		
		//	Strip the computer name from the folder
		lstrcpy(m_tszFolder, &(m_tszFolder[lstrlen(m_tszComputer)]));
		while((m_tszFolder[0] == '\\') || (m_tszFolder[0] == '/'))
			lstrcpy(m_tszFolder, &(m_tszFolder[1]));

		//// Get the share name
		lstrcpyn(m_tszShare, m_tszFolder, sizeof(m_tszShare));
		for(i = 0; i < lstrlen(m_tszShare); i++)
		{
			if((m_tszShare[i] == '\\') || (m_tszShare[i] == '/'))
			{
				m_tszShare[i] = '\0';
				break;
			}
		}
		
		//	Strip the share name from the folder but leave the leading delimiter
		//	on the folder name 
		lstrcpy(m_tszFolder, &(m_tszFolder[lstrlen(m_tszShare)]));
	}
	
	return TRUE;
}
