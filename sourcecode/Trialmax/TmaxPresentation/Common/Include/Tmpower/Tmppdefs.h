//==============================================================================
//
// File Name:	tmppdefs.h
//
// Description:	This file contains defines used by the tm_power.ocx control. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	05-09-99	1.00		Original Release
//==============================================================================
#if !defined(__TMPPDEFS_H__)
#define __TMPPDEFS_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	View identifiers	
#define TMPOWER_ACTIVEVIEW		   -1
#define TMPOWER_LEFTVIEW			0
#define TMPOWER_RIGHTVIEW			1

//	PowerPoint state identifiers
#define TMPOWER_DONE				0
#define TMPOWER_RUNNING				1
#define TMPOWER_PAUSED				2
#define TMPOWER_WHITESCREEN			3
#define TMPOWER_BLACKSCREEN			4

// Save file format identifiers
#define TMPOWER_TIF					0
#define TMPOWER_WMF					1
#define TMPOWER_BMP					2
#define TMPOWER_PNG					3
#define TMPOWER_JPG					4
#define TMPOWER_GIF					5
				
//	Default property values
#define TMPOWER_SPLITFRAMETHICKNESS	5
#define TMPOWER_SPLITFRAMECOLOR		RGB(128,128,128)
#define TMPOWER_SPLITSCREEN			FALSE
#define TMPOWER_DEFAULTVIEW			TMPOWER_LEFTVIEW
#define TMPOWER_SYNCVIEWS			FALSE
#define TMPOWER_STARTSLIDE			1
#define TMPOWER_ENABLEACCELERATORS	FALSE
#define TMPOWER_ENABLEAXERRORS		FALSE
#define TMPOWER_USESLIDEID			FALSE
#define TMPOWER_SAVEFORMAT			TMPOWER_JPG

//	TMPOWER error levels
#define TMPOWER_NOERROR				0
#define TMPOWER_FILENOTFOUND		(TMPOWER_NOERROR + 1)
#define TMPOWER_ATTACHFAILED		(TMPOWER_NOERROR + 2)
#define TMPOWER_CREATEFAILED		(TMPOWER_NOERROR + 3)
#define TMPOWER_NOMAINWINDOW		(TMPOWER_NOERROR + 4)
#define TMPOWER_NOTREADY			(TMPOWER_NOERROR + 5)
#define TMPOWER_NOPRESENTATIONS		(TMPOWER_NOERROR + 6)
#define TMPOWER_NOPRESENTATION		(TMPOWER_NOERROR + 7)
#define TMPOWER_NOSETTINGS			(TMPOWER_NOERROR + 8)
#define TMPOWER_NOSLIDEWINDOW		(TMPOWER_NOERROR + 9)
#define TMPOWER_NOSLIDEVIEW			(TMPOWER_NOERROR + 10)
#define TMPOWER_NOSLIDEHANDLE		(TMPOWER_NOERROR + 11)
#define TMPOWER_NOSLIDES			(TMPOWER_NOERROR + 12)
#define TMPOWER_OUTOFRANGE			(TMPOWER_NOERROR + 13)
#define TMPOWER_CREATESNAPSHOTFAIL	(TMPOWER_NOERROR + 14)
#define TMPOWER_NOSLIDEFOUND		(TMPOWER_NOERROR + 15)
#define TMPOWER_SAVESLIDEFAILED		(TMPOWER_NOERROR + 16)
#define TMPOWER_INVALIDSLIDEID		(TMPOWER_NOERROR + 17)
#define TMPOWER_OLEEXCEPTION		(TMPOWER_NOERROR + 18)
#define TMPOWER_LOAD_TIMED_OUT		(TMPOWER_NOERROR + 19)

#endif