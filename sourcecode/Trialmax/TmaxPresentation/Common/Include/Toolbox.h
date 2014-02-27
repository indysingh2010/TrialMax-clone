//==============================================================================
//
// File Name:	toolbox.h
//
// Description:	This file contains the declarations of the CTMToolbox class.
//
// Author:		Kenneth Moore
//
// Copyright Oceaneering Technologies
//
//==============================================================================
//	Date		Revision    Description
//	09-18-04	1.00		Original Release
//==============================================================================
#if !defined(__TOOLBOX_H__)
#define __TOOLBOX_H__

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

//	This class contains generic functions shared by all TrialMax controls and
//	applications
class CTMToolbox
{
	private:

		static HWND				GetVistaStartMenuWnd(HWND hTaskBar);

	public:
	
								CTMToolbox();
		virtual				   ~CTMToolbox();

		static BOOL				GetName(LPCSTR lpszPath, CString& rName);
		static BOOL				GetParent(LPCSTR lpszPath, CString& rParent);
		static BOOL				GetLongPath(LPCSTR lpszPath, CString& rLongPath);
		static BOOL				SplitPath(LPCSTR lpszPath, CString& rParent, CString& rName);
		
		static BOOL				FindFile(LPCSTR lpszFileSpec);
		static CString			FindFirstFile(LPCSTR lpszFolder, LPCSTR lpszExtension, BOOL bFullPath = TRUE);
		static CString			GetSearchPath(LPCSTR lpszFolder, LPCSTR lpszExtension);
		static CStringArray*	FindAllFiles(LPCSTR lpszFolder, LPCSTR lpszExtension, BOOL bFullPath = TRUE);

		static BOOL				SetTaskBarVisible(BOOL bVisible);
};

#endif // !defined(__TOOLBOX_H__)
