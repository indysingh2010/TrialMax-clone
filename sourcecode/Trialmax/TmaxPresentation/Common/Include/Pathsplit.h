//==============================================================================
//
// File Name:	pathsplit.h
//
// Description:	This file contains the declaration of the CPathSplitter class. 
//
// Author:		Kenneth Moore
//
// Copyright Oceaneering International
//
//==============================================================================
//	Date		Revision    Description
//	05-13-2006	1.00		Original Release
//==============================================================================
#if !defined(__PATHSPLIT_H__)
#define __PATHSPLIT_H__

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CPathSplitter
{
	private:

		TCHAR			m_tszComputer[_MAX_FNAME];
		TCHAR			m_tszShare[_MAX_FNAME];
		TCHAR			m_tszPath[_MAX_PATH];
		TCHAR			m_tszDrive[_MAX_DRIVE];
		TCHAR			m_tszFolder[_MAX_DIR];
		TCHAR			m_tszFilename[_MAX_FNAME];
		TCHAR			m_tszExtension[_MAX_EXT];
		CString			m_strRequest;

	public:

						CPathSplitter(LPCTSTR lpszPath = NULL);
					   ~CPathSplitter();

		CString			GetComputer()	{ return m_tszComputer; }
		CString			GetShare()		{ return m_tszShare; }
		CString			GetPath()		{ return m_tszPath; }
		CString			GetDrive()		{ return m_tszDrive; }
		CString			GetExtension()	{ return m_tszExtension; }
		
		CString			GetFolder(BOOL bFullPath = TRUE);
		CString			GetFileName(BOOL bIncludeExtension = TRUE);

		BOOL			IsUNC(){ return (_tcsncmp(m_tszPath,_T("\\\\"), 2)==0); }
		BOOL			Split(LPCTSTR lpszPath);
		void			MsgBox(HWND hWnd, LPCTSTR lpszTitle = "");
	
	protected:

};

#endif // !defined(__PATHSPLIT_H__)
