//==============================================================================
//
// File Name:	msppenum.h
//
// Description:	This file contains enumerations used by the PowerPoint 
//				automation interfaces.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	07-03-99	1.00		Original Release
//==============================================================================
#if !defined(__MSPPENUM_H__)
#define __MSPPENUM_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#define PP_TRUE			((long)-1)
#define PP_FALSE		((long)0)

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------
typedef enum 
{
    ppWindowNormal = 1,
    ppWindowMinimized = 2,
    ppWindowMaximized = 3

} PpWindowState;

typedef enum 
{
    ppShowTypeSpeaker = 1,
    ppShowTypeWindow = 2,
    ppShowTypeKiosk = 3

}PpSlideShowType;

typedef enum 
{
    ppSlideShowRunning = 1,
    ppSlideShowPaused = 2,
    ppSlideShowBlackScreen = 3,
    ppSlideShowWhiteScreen = 4,
    ppSlideShowDone = 5

}pSlideShowState;

typedef enum 
{
    ppShowAll = 1,
    ppShowSlideRange = 2,
    ppShowNamedSlideShow = 3

}PpSlideShowRangeType;

typedef enum 
{
    ppSlideShowManualAdvance = 1,
    ppSlideShowUseSlideTimings = 2,
    ppSlideShowRehearseNewTimings = 3

}PpSlideShowAdvanceMode;

typedef enum
{
	ppAnimateByAllLevels = 16,
	ppAnimateByFifthLevel = 5,
	ppAnimateByFirstLevel = 1,
	ppAnimateByFourthLevel = 4,
	ppAnimateBySecondLevel = 2,
	ppAnimateByThirdLevel = 3,
	ppAnimateLevelMixed = -2,
	ppAnimateLevelNone = 0
}PpTextLevelEffect;


typedef enum
{
	ppVersion2003 = 11,
	ppVersion2007 = 13,
	ppVersion2010 = 14,
	ppVersion2013 = 15,
	ppVersion2016 = 16
}PpVersion;
#endif // !defined(__MSPPENUM_H__)
