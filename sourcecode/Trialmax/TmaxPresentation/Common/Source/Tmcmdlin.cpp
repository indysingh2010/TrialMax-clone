//==============================================================================
//
// File Name:	tmcmdlin.cpp
//
// Description:	This file contains member functions of the CTMCommandLineInfo
//				class. This is a custom command line parser specifically for
//				use with all TrialMax II applications.
//
// See Also:	tmcmdlin.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	08-28-97	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmcmdlin.h>

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
//	MAPS
//------------------------------------------------------------------------------

//==============================================================================
//
// 	Function Name:	CTMCommandLineInfo::CTMCommandLineInfo()
//
// 	Description:	This is the constructor for CTMCommandLineInfo objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMCommandLineInfo::CTMCommandLineInfo() : CCommandLineInfo()
{
	m_bRunTest = FALSE;
	m_strCaseFolder.Empty();
	m_strIniFile.Empty();	
	m_strBarcode.Empty();
	m_bSilent = FALSE;
	m_lPageNumber = 0;
	m_iLineNumber = 0;
}

//==============================================================================
//
// 	Function Name:	CTMCommandLineInfo::ParseParam()
//
// 	Description:	This is an overloaded version of the base class member that
//					adds custom processing of command line flags.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMCommandLineInfo::ParseParam(const char* pszParam,BOOL bFlag,BOOL bLast)
{
	if(bFlag)
	{
		ParseParamFlag(pszParam);
	}
	else
		ParseParamNotFlag(pszParam);

	ParseLast(bLast);
}

//==============================================================================
//
// 	Function Name:	CTMCommandLineInfo::ParseParamFlag()
//
// 	Description:	This is an overloaded version of the base class member that
//					adds custom processing of command line flags.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMCommandLineInfo::ParseParamFlag(const char* pszParam)
{
	CString strParam = pszParam;
	CString	strSwitch;
	CString strOption;
	int		iColon;
	
	//	Is this the test mode switch?
	if(lstrcmpi(pszParam, TEST_SWITCH) == 0)
	{
		m_bRunTest = TRUE;
	}

	//	Is this the silent switch?
	else if(lstrcmpi(pszParam, SILENT_SWITCH) == 0)
	{
		m_bSilent = TRUE;
	}

	//	Is this one of the composite switches?
	else if((iColon = strParam.Find(':')) > 0)
	{
		strSwitch = strParam.Left(iColon + 1);
		strOption = strParam.Right(strParam.GetLength() - (iColon + 1));

		//	Check to see if this is one of our custom flags. If not, perform the
		//	base class processing
		if(!strSwitch.CompareNoCase(BARCODE_SWITCH))
			m_strBarcode = strOption;
		else if(!strSwitch.CompareNoCase(INIFILE_SWITCH))
			m_strIniFile = strOption;
		else if(!strSwitch.CompareNoCase(CASEFOLDER_SWITCH))
			m_strCaseFolder = strOption;
		else if(!strSwitch.CompareNoCase(PAGE_NUMBER_SWITCH))
			m_lPageNumber = atol(strOption);
		else if(!strSwitch.CompareNoCase(LINE_NUMBER_SWITCH))
			m_iLineNumber = atoi(strOption);
		else
			CCommandLineInfo::ParseParamFlag(pszParam);
	}
	
	else
	{
		//	Let the base class handle this switch
		CCommandLineInfo::ParseParamFlag(pszParam);
		return;
	}
		
}

