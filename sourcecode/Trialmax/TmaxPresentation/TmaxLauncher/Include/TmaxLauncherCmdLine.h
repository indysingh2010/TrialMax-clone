//==============================================================================
//
// File Name:	TmaxLauncherCmdLine.h
//
// Description:	This file contains the declaration of the TmaxLaucherCmdLine
//				class
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting
//
//==============================================================================
//	Date		Revision    Description
//	02-07-2007	1.00		Original Release
//==============================================================================
#if !defined(__TMAXLAUNCHERCMDLINE_H__)
#define __TMAXLAUNCHERCMDLINE_H__

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
#define TEST_MODE_SWITCH		"T"

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTmaxLauncherCmdLine : public CCommandLineInfo
{
	private:

	public:
	
		BOOL		m_bTestMode;

					CTmaxLauncherCmdLine();

	protected:
	
		void		ParseParam(const char* pszParam,BOOL bFlag,BOOL bLast);
		void		ParseParamFlag(const char* pszParam);
};

#endif // !defined(__TMAXLAUNCHERCMDLINE_H__)
