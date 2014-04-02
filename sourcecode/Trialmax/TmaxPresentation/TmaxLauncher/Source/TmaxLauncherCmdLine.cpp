//==============================================================================
//
// File Name:	TmaxLauncherCmdLine.cpp
//
// Description:	This file contains member functions of the CTmaxLauncherCmdLine 
//				class
//
// See Also:	TmaxLauncherCmdLine.h
//
// Copyright	FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	02-07-2007	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <StdAfx.h>
#include <TmaxLauncherCmdLine.h>

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
// 	Function Name:	CTmaxLauncherCmdLine::CTmaxLauncherCmdLine()
//
// 	Description:	Constructor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTmaxLauncherCmdLine::CTmaxLauncherCmdLine() : CCommandLineInfo()
{
	m_bTestMode = FALSE;
}

//==============================================================================
//
// 	Function Name:	CTmaxLauncherCmdLine::ParseParam()
//
// 	Description:	Called to parse the command line switch
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTmaxLauncherCmdLine::ParseParam(const char* pszParam,BOOL bFlag,BOOL bLast)
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
// 	Function Name:	CTmaxLauncherCmdLine::ParseParamFlag()
//
// 	Description:	Called to parse the command line switch
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTmaxLauncherCmdLine::ParseParamFlag(const char* pszParam)
{
	//	Is this the test mode switch?
	if(lstrcmpi(pszParam, TEST_MODE_SWITCH) == 0)
	{
		m_bTestMode = TRUE;
	}
	else
	{
		//	Let the base class handle this switch
		CCommandLineInfo::ParseParamFlag(pszParam);
	}
		
}

