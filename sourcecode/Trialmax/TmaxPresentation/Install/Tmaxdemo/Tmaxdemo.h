//==============================================================================
//
// File Name:	Tmaxdemo.h
//
// Description:	This file contains the declaration of the TmaxdemoApp class. 
//				This is the application wrapper for the dll that gets linked 
//				into the installations for the TrialMax demo program
//
// Author:		Kenneth Moore
//
// Copyright 1999 Forensics Technologies International
//
//==============================================================================
//	Date		Revision    Description
//	01-23-99	1.00		Original Release
//==============================================================================
#if !defined(AFX_TMAXDEMO_H__1FEC5773_B2AB_11D2_AD00_444553540000__INCLUDED_)
#define AFX_TMAXDEMO_H__1FEC5773_B2AB_11D2_AD00_444553540000__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols
#include <tmini.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define TMAXDEMO_INIFILENAME	"tmaxdemo.ini"
#define DEMOFLAGS_INIFILENAME	"demoflag.ini"

#define DATABASE_SECTION		"DATABASE"
#define VIDEOS_SECTION			"VIDEOS"
#define FLAGS_SECTION			"FLAGS"
#define ROOTFOLDER_LINE			"RootFolder"
#define DBCOPIED_LINE			"DBCopied"
#define COMMENT_LINE			"Comment"

#define DEFAULT_CASENAME		"TrialMax Demo Database"
#define LOGFILES_TEST			"TM_Logfiles"

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CTmaxdemoApp : public CWinApp
{
	private:

	public:

		CTMIni			m_Flags;
		CString			m_strSource;
		CString			m_strSupport;
		CString			m_strInstall;
		HWND			m_hWnd;
	
						CTmaxdemoApp();

		BOOL			OpenIni();
		BOOL			Database(BOOL bConfirm = FALSE);
		BOOL			Videos();

	protected:

		BOOL			Find(LPCSTR lpFilespec);
		BOOL			CopyFile(LPCSTR lpSource, LPCSTR lpTarget, LPCSTR lpTitle);
		BOOL			CopyFolder(LPCSTR lpSource, LPCSTR lpTarget, LPCSTR lpTitle);
		void			SetFolderAttributes(LPCSTR lpFolder);

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTmaxdemoApp)
	//}}AFX_VIRTUAL

	//{{AFX_MSG(CTmaxdemoApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMAXDEMO_H__1FEC5773_B2AB_11D2_AD00_444553540000__INCLUDED_)
