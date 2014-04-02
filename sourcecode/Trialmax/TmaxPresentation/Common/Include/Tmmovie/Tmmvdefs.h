//==============================================================================
//
// File Name:	tmmvdefs.h
//
// Description:	This file contains defines used by the tm_movie.ocx control. 
//				This file is required to programmatically manage one of these 
//				controls.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	06-20-98	1.00		Original Release
//==============================================================================
#if !defined(__TMMVDEFS_H__)
#define __TMMVDEFS_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	TMMovie Seek Modes
#define TMMOVIE_SEEK_FRAMES			0
#define TMMOVIE_SEEK_TIME			1

//	TMMovie Player States
#define TMMOVIE_NOTREADY			0
#define TMMOVIE_READY				1
#define TMMOVIE_PLAYING				2
#define TMMOVIE_PAUSED				3
#define TMMOVIE_STOPPED				4

// TMMovie cue types
#define TMMCUE_FIRST				0
#define TMMCUE_LAST					1
#define TMMCUE_START				2
#define TMMCUE_STOP					3
#define TMMCUE_ABSOLUTE				4
#define TMMCUE_RELATIVE				5	

//	TMMovie video file types
#define TMMOVIE_UNKNOWN				0
#define TMMOVIE_MPEG				1
#define TMMOVIE_AVI					2

//	TMMovie playlist states
#define TMMOVIE_PLNONE				0
#define TMMOVIE_PLSET				1
#define TMMOVIE_PLACTIVE			2
#define TMMOVIE_PLERROR				3
#define TMMOVIE_PLFINISHED			4
#define TMMOVIE_PLSTOPPED			5

//	TMMovie playlist cue types
#define TMMCUEPL_FIRST				0
#define TMMCUEPL_LAST				1
#define TMMCUEPL_NEXT				2
#define TMMCUEPL_PREVIOUS			3
#define TMMCUEPL_CURRENT			4
#define TMMCUEPL_STEP				5
	
//	DirectShow Interface identifiers
#define TMMOVIE_IGRAPHBUILDER		1
#define TMMOVIE_IBASICVIDEO			2
#define TMMOVIE_IBASICAUDIO			3
#define TMMOVIE_IMEDIACONTROL		4
#define TMMOVIE_IMEDIAEVENTEX		5
#define TMMOVIE_IMEDIAPOSITION		6
#define TMMOVIE_IMEDIASEEKING		7
#define TMMOVIE_IVIDEOWINDOW		8

//	Mouse button identifiers
#define TMMOVIE_NO_BUTTON			0
#define TMMOVIE_LEFT_BUTTON			1
#define TMMOVIE_RIGHT_BUTTON		2

//	Key identifiers used in events
#define TMMOVIE_SHIFT				1
#define TMMOVIE_CTRL				2
#define TMMOVIE_CTRLSHIFT			3
#define TMMOVIE_ALT					4
#define TMMOVIE_ALTSHIFT			5
#define TMMOVIE_CTRLALT				6
#define TMMOVIE_CTRLALTSHIFT		7

//	TMMOVIE error levels
#define TMMOVIE_NOERROR				0
#define TMMOVIE_INVALIDARG			(TMMOVIE_NOERROR + 1)
#define TMMOVIE_FILENOTFOUND		(TMMOVIE_NOERROR + 2)
#define TMMOVIE_INITIALIZEFAILED	(TMMOVIE_NOERROR + 3)
#define TMMOVIE_NOTINITIALIZED		(TMMOVIE_NOERROR + 4)
#define TMMOVIE_NOTLOADED			(TMMOVIE_NOERROR + 5)
#define TMMOVIE_PLAYFAILED			(TMMOVIE_NOERROR + 6)
#define TMMOVIE_PAUSEFAILED			(TMMOVIE_NOERROR + 7)
#define TMMOVIE_RESUMEFAILED		(TMMOVIE_NOERROR + 8)
#define TMMOVIE_STOPFAILED			(TMMOVIE_NOERROR + 9)
#define TMMOVIE_LOADFAILED			(TMMOVIE_NOERROR + 10)
#define TMMOVIE_CUEFAILED			(TMMOVIE_NOERROR + 11)
#define TMMOVIE_NOPLAYLIST			(TMMOVIE_NOERROR + 12)
#define TMMOVIE_NODESIGNATION		(TMMOVIE_NOERROR + 13)
#define TMMOVIE_NEXTNOTFOUND		(TMMOVIE_NOERROR + 14)
#define TMMOVIE_PREVNOTFOUND		(TMMOVIE_NOERROR + 15)
#define TMMOVIE_CUEDESIGNATION		(TMMOVIE_NOERROR + 16)
#define TMMOVIE_INVALIDVIDEO		(TMMOVIE_NOERROR + 17)
#define TMMOVIE_STEPFAILED			(TMMOVIE_NOERROR + 18)
#define TMMOVIE_UPDATEFAILED		(TMMOVIE_NOERROR + 19)
#define TMMOVIE_NOREVERSESTEP		(TMMOVIE_NOERROR + 20)
#define TMMOVIE_SNAPSHOTFAILED		(TMMOVIE_NOERROR + 21)
#define TMMOVIE_CAPFILEFAILED		(TMMOVIE_NOERROR + 22)
#define TMMOVIE_CAPTUREFAILED		(TMMOVIE_NOERROR + 23)
#define TMMOVIE_INVALIDDESIGNATION	(TMMOVIE_NOERROR + 24)
#define TMMOVIE_ENDOFSEGMENT		(TMMOVIE_NOERROR + 25)
#define TMMOVIE_UNDEFINEDVIDEOID	(TMMOVIE_NOERROR + 26)
#define TMMOVIE_ADDFILTERFAILED		(TMMOVIE_NOERROR + 27)
#define TMMOVIE_REMOVEFILTERFAILED	(TMMOVIE_NOERROR + 28)

#endif