//==============================================================================
//
// File Name:	tmver.h
//
// Description:	This file contains the declarations of the CTMVersion class.
//
// Author:		Kenneth Moore
//
// Copyright Oceaneering Technologies
//
//==============================================================================
//	Date		Revision    Description
//	11-24-04	1.00		Original Release
//==============================================================================
#if !defined(__TMVER_H__)
#define __TMVER_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <filever.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

#define TMVERSION_MIN_BUILD_DATE	1013	// 01-01-03

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	This class encapsulates the version information for a TrialMax application
//	or ActiveX control
class CTMVersion
{
	private:

		CFileVersionInfo	m_fileVersion;

		int					m_iMajor;
		int					m_iMinor;
		int					m_iUpdate;
		int					m_iBuild;

		CString				m_strFileSpec;
		CString				m_strTextVer;
		CString				m_strShortTextVer;
		CString				m_strBuildDate;
		CString				m_strDescription;
		CString				m_strName;
		CString				m_strClsId;

		BOOL				m_bFileExists;
		BOOL				m_bRegistered;

	public:
	
							CTMVersion();
		virtual			   ~CTMVersion();

		//	Public access
		int					GetMajor(){ return m_iMajor; }
		int					GetMinor(){ return m_iMinor; }
		int					GetUpdate(){ return m_iUpdate; }
		int					GetBuild(){ return m_iBuild; }
		long				GetPackedVer();
		long				GetPackedVer(int iMajor, int iMinor, int iUpdate = 0);
		long				GetPackedVer(LPCSTR lpszVersion);
		BOOL				GetFileExists(){ return m_bFileExists; }
		BOOL				GetRegistered(){ return m_bRegistered; }
		LPCSTR				GetFileSpec(){ return m_strFileSpec; }
		LPCSTR				GetTextVer(){ return m_strTextVer; }
		LPCSTR				GetShortTextVer(){ return m_strShortTextVer; }
		LPCSTR				GetBuildDate(){ return m_strBuildDate; }
		//LPCSTR				GetName(){ return m_strName; }
		CString				GetName(){ return m_strName; }
		LPCSTR				GetDescription(){ return m_strDescription; }
		LPCSTR				GetClsId(){ return m_strClsId; }
		LPCSTR				GetLocation(CString& rLocation);
		CFileVersionInfo&	GetFileVersionInfo(){ return m_fileVersion; }

		void				SetMajor(int iMajor){ m_iMajor = iMajor; }
		void				SetMinor(int iMinor){ m_iMinor = iMinor; }
		void				SetUpdate(int iUpdate){ m_iUpdate = iUpdate; }
		void				SetBuild(int iBuild){ m_iBuild = iBuild; }
		void				SetFileExists(BOOL bFileExists){ m_bFileExists = bFileExists; }
		void				SetRegistered(BOOL bRegistered){ m_bRegistered = bRegistered; }
		void				SetFileSpec(LPCSTR lpszFileSpec);
		void				SetTextVer(LPCSTR lpszVer){ m_strTextVer = lpszVer != 0 ? lpszVer : ""; }
		void				SetShortTextVer(LPCSTR lpszVer){ m_strShortTextVer = (lpszVer != 0 ? lpszVer : ""); }
		void				SetBuildDate(LPCSTR lpszBuildDate){ m_strBuildDate = (lpszBuildDate != 0 ? lpszBuildDate : ""); }
		//void				SetName(LPCSTR lpszName){ m_strName = (lpszName != 0 ? lpszName : ""); }
		void				SetName(CString lpszName){ m_strName = lpszName; }
		void				SetDescription(LPCSTR lpszDescription){ m_strDescription = (lpszDescription != 0 ? lpszDescription : ""); }
		void				SetClsId(LPCSTR lpszClsId){ m_strClsId = (lpszClsId != 0 ? lpszClsId : ""); }

		//	Operations
		void				Clear();
		void				SetVersionText();
		BOOL				InitFromClsId(LPCSTR lpszName, LPCSTR lpszDescription, CLSID clsId);
		BOOL				InitFromClsId(LPCSTR lpszName, LPCSTR lpszDescription, LPCSTR lpszClsId);
		BOOL				InitFromFile(LPCSTR lpszName, LPCSTR lpszDescription, LPCSTR lpszFileSpec, BOOL bClear = TRUE);
		BOOL				InitFromString(LPCSTR lpszVersion, LPCSTR lpszDelimiters = ".");
		BOOL				GetRegisteredPath(LPCSTR lpszClsId, CString& rPath);
		BOOL				SplitVersion(LPCSTR lpszVer, int& rMajor, int& rMinor, int& rUpdate, char cDelimiter = '.');
};

#endif // !defined(__TMVER_H__)
