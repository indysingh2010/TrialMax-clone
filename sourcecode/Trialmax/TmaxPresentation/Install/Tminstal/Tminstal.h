//==============================================================================
//
// File Name:	Tminstal.h
//
// Description:	This file contains the declaration of the CTMInstall class. 
//				This is the application wrapper for the dll that gets linked 
//				into the installations for the TrialMax distributions.
//
// Author:		Kenneth Moore
//
// Copyright 1999 Forensics Technologies International
//
//==============================================================================
//	Date		Revision    Description
//	03-25-99	1.00		Original Release
//==============================================================================
#if !defined(AFX_TMINSTAL_H__5D8FFB78_E41C_11D2_8175_00802966F8C1__INCLUDED_)
#define AFX_TMINSTAL_H__5D8FFB78_E41C_11D2_8175_00802966F8C1__INCLUDED_

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
#define TMAXINSTALL_INIFILENAME				"Tminstall.ini"
#define TMAXINSTALL_FILESSECTION			"FILES"
#define TMAXINSTALL_FOLDERSSECTION			"FOLDERS"

#define TMAXINSTALL_TMVIEW_CLSID			"{5A3A9FC9-D747-4B92-9106-A32C7E6E84A3}"
#define TRIALMAX_INSTALLDIR_KEY_PATH		"Software\\FTI Consulting\\TrialMax 6\\Product\\TrialMax"
#define TMAXVIDEO_INSTALLDIR_KEY_PATH		"Software\\FTI Consulting\\TrialMax 6\\Product\\VideoViewer"
#define TMAXINSTALL_INSTALLDIR_VALUE_NAME	"InstallDir"

#define NET_10_INSTALL_KEY_PATH				"Software\\Microsoft\\.NETFramework\\Policy\\v1.0"
#define NET_10_INSTALL_VALUE_NAME			"3705"
#define NET_11_INSTALL_KEY_PATH				"Software\\Microsoft\\Net Framework Setup\\NDP\\v1.1.4322"
#define NET_11_INSTALL_VALUE_NAME			"Install"
#define NET_20_INSTALL_KEY_PATH				"Software\\Microsoft\\Net Framework Setup\\NDP\\v2.0.50727"
#define NET_20_INSTALL_VALUE_NAME			"Install"
#define NET_30_INSTALL_KEY_PATH				"Software\\Microsoft\\NET Framework Setup\\NDP\\v3.0\\Setup"
#define NET_30_INSTALL_VALUE_NAME			"InstallSuccess"

//	Control identifiers
#define TMAX_AXCTRL_TMSTAT					0
#define TMAX_AXCTRL_TMTEXT					1
#define TMAX_AXCTRL_TMVIEW					2
#define TMAX_AXCTRL_TMLPEN					3
#define TMAX_AXCTRL_TMTOOL					4
#define TMAX_AXCTRL_TMBARS					5
#define TMAX_AXCTRL_TMMOVIE					6
#define TMAX_AXCTRL_TMPOWER					7
#define TMAX_AXCTRL_TMPRINT					8
#define TMAX_AXCTRL_TMSHARE					9
#define TMAX_AXCTRL_TMGRAB					10
#define TMAX_AXCTRL_TMSETUP					11
#define TMAX_AXCTRL_MAX						12

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------
extern const WORD	_wVerMajor;
extern const WORD	_wVerMinor;

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

class CTMInstallApp : public CWinApp
{
	private:

		CTMIni			m_Ini;
		HWND			m_hWnd;
		CString			m_strTarget;
		CString			m_strSource;
		CString			m_strProductKey;
		BOOL			m_bVideoViewer;
	
	public:
	
						CTMInstallApp();


		BOOL			CopyFile(LPCSTR lpSource, LPCSTR lpTarget);
		BOOL			CopyFolder(LPCSTR lpSource, LPCSTR lpTarget);
		BOOL			CopyFiles(HWND hWnd, LPSTR lpSource, LPSTR lpTarget);
		BOOL			CopyFolders(HWND hWnd, LPSTR lpSource, LPSTR lpTarget);
		BOOL			ConfirmInstallDir(HWND hWnd, LPSTR lpSource, LPSTR lpTarget, BOOL bVideoViewer);
		BOOL			ConfirmNETInstallation(HWND hWnd, LPSTR lpSource, LPSTR lpTarget, float fVersion);
		BOOL			GetRegisteredPath(LPCSTR lpszClsId, CString& rPath);
		BOOL			WarningBox(LPCSTR lpszMessage);
		void			SetFolderAttributes(LPCSTR lpFolder);

	protected:

		void			GetAppFileSpec(LPCSTR lpszInstallDir, CString& rPath);
		BOOL			ReadInstallDir(CString& rPath);
		BOOL			WriteInstallDir(CString& rPath);
		BOOL			GetInstallDir(CString& rPath);
		BOOL			GetInstalledLocation(CString& rPath);
		BOOL			CheckSetupVersion(LPCSTR lpszInstallDir);
		BOOL			GetSetupVersion(LPCSTR lpszInstallDir, int& rMajor, int& rMinor);

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTMInstallApp)
	//}}AFX_VIRTUAL

	//{{AFX_MSG(CTMInstallApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TMINSTAL_H__5D8FFB78_E41C_11D2_8175_00802966F8C1__INCLUDED_)
