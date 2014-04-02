//==============================================================================
//
// File Name:	tmhelp.h
//
// Description:	This file contains the declaration of the CTMHelp class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	10-30-97	1.00		Original Release
//==============================================================================
#if !defined(__TMHELP_H__)
#define __TMHELP_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define TMHELP_FILENAME		"Trialmax.hlp"

//	Help Context Identifiers
#define TMHELP_CONTENTS			0
#define TMHELP_CMCASE			1
#define TMHELP_CMPARTY			2
#define TMHELP_CMDEPONENT		3
#define TMHELP_CMDEPOSITION		4
#define TMHELP_CMPLAYLIST		5
#define TMHELP_CMTUNE			6
#define TMHELP_CMREGISTER		7
#define TMHELP_CMVIEW			8
#define TMHELP_CMBARCODE		9
#define TMHELP_TMOVERVIEW		10
#define TMHELP_TMTOOLBAR		11
#define TMHELP_TMDATABASE		12
#define TMHELP_TMGRAPHICS		13
#define TMHELP_TMVIDEO			14
#define TMHELP_TMDIAGNOSTICS	15
#define TMHELP_TMABOUT			16
#define TMHELP_TMDIRECTX		17
#define TMHELP_CMSYSTEM			18
#define TMHELP_TMTEXT			19
#define TMHELP_TMSYSTEM			20

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
class CTMHelp : public CObject
{
	private:

		HWND		m_hParent;
		CString		m_strFilespec;
	
	public:

					CTMHelp();
				   ~CTMHelp();

		void		Initialize(HWND hParent);
		void		Open(short sContext = TMHELP_CONTENTS);
	
	protected:

};

#endif // !defined(__TMHELP_H__)
