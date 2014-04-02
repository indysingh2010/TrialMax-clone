//==============================================================================
//
// File Name:	tmcmdlin.h
//
// Description:	This file contains the declaration of the CTMCommandLineInfo 
//				classes. CTMCommandLineInfo is used to add custom processing
//				of all TrialMax II application command lines.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	08-28-97	1.00		Original Release
//==============================================================================
#if !defined(__TMCMDLIN_H__)
#define __TMCMDLIN_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
	
//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Command line switches
#define INIFILE_SWITCH			"I:"
#define CASEFOLDER_SWITCH		"C:"
#define BARCODE_SWITCH			"B:"
#define PAGE_NUMBER_SWITCH		"P:"
#define LINE_NUMBER_SWITCH		"L:"
#define TEST_SWITCH				"T"
#define SILENT_SWITCH			"Q"

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMCommandLineInfo : public CCommandLineInfo
{
	private:

	public:
	
		BOOL		m_bRunTest;
		BOOL		m_bSilent;
		long		m_lPageNumber;
		short		m_iLineNumber;
		CString		m_strIniFile;
		CString		m_strCaseFolder;
		CString		m_strBarcode;

					CTMCommandLineInfo();

	protected:
	
		void		ParseParamFlag(const char* pszParam);
		void		ParseParam(const char* pszParam,BOOL bFlag,BOOL bLast);
};

#endif // !defined(__TMCMDLIN_H__)
