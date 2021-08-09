//==============================================================================
//
// File Name:	tables.h
//
// Description:	This file defines the indices used to translate button
//				identifiers to their image strip index
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	09-26-98	1.00		Original Release
//==============================================================================
#if !defined(__TABLES_H__)
#define __TABLES_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <tmtbdefs.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
//	These constants are used to define the button sort order
#define SORTED_CONFIG						0
#define SORTED_CONFIGTOOLBARS				1
#define SORTED_BINDERLIST					2
#define SORTED_CLEAR						3
#define SORTED_ROTATECW						4
#define SORTED_ROTATECCW					5
#define SORTED_NORMAL						6
#define	SORTED_ZOOM							7
#define SORTED_ZOOMRESTRICTED				8
#define SORTED_ZOOMWIDTH					9
#define SORTED_PAN							10
#define SORTED_GESTUREPAN					11
#define SORTED_CALLOUT						12
#define SORTED_DRAWTOOL						13
#define SORTED_SELECTTOOL					14
#define SORTED_HIGHLIGHT					15
#define SORTED_REDACT						16
#define SORTED_ERASE						17
#define SORTED_DELETEANN					18
#define SORTED_SELECT						19
#define SORTED_FIRSTPAGE					20
#define SORTED_PREVPAGE						21
#define SORTED_NEXTPAGE						22
#define SORTED_LASTPAGE						23
#define SORTED_SAVEZAP						24
#define SORTED_SAVESPLITZAP					25
#define SORTED_UPDATEZAP					26
#define SORTED_DELETEZAP					27
#define SORTED_FIRSTZAP						28
#define SORTED_PREVZAP						29
#define SORTED_NEXTZAP						30
#define SORTED_LASTZAP						31
#define SORTED_STARTMOVIE					32
#define SORTED_BACKMOVIE					33
#define SORTED_PAUSEMOVIE					34
#define SORTED_PLAYMOVIE					35
#define SORTED_FWDMOVIE						36
#define SORTED_ENDMOVIE						37
#define SORTED_FIRSTDESIGNATION				38
#define SORTED_BACKDESIGNATION				39
#define SORTED_PREVDESIGNATION				40
#define SORTED_STARTDESIGNATION				41
#define SORTED_PAUSEDESIGNATION				42
#define SORTED_PLAYDESIGNATION				43
#define SORTED_NEXTDESIGNATION				44
#define SORTED_FWDDESIGNATION				45
#define SORTED_LASTDESIGNATION				46
#define SORTED_PLAYTHROUGH					47
#define SORTED_CUEPGLNCURRENT				48
#define SORTED_CUEPGLNNEXT					49
#define SORTED_PRINT						50
#define SORTED_SPLITVERTICAL				51
#define SORTED_SPLITHORIZONTAL				52
#define SORTED_DISABLELINKS					53
#define SORTED_ENABLELINKS					54
#define SORTED_STATUSBAR					55
#define SORTED_TEXT							56
#define SORTED_FULLSCREEN					57
#define SORTED_SHADEDCALLOUTS				58
#define SORTED_EXIT							59
#define SORTED_DARKRED						60
#define SORTED_RED							61
#define SORTED_LIGHTRED						62
#define SORTED_DARKGREEN					63
#define SORTED_GREEN						64
#define SORTED_LIGHTGREEN					65
#define SORTED_DARKBLUE						66
#define SORTED_BLUE							67
#define SORTED_LIGHTBLUE					68
#define SORTED_YELLOW						69
#define SORTED_BLACK						70
#define SORTED_WHITE						71
#define SORTED_ANNTEXT						72
#define SORTED_FREEHAND						73
#define SORTED_LINE							74
#define SORTED_ARROW						75
#define SORTED_ELLIPSE						76
#define SORTED_RECTANGLE					77
#define SORTED_POLYLINE						78
#define SORTED_FILLEDELLIPSE				79
#define SORTED_FILLEDRECTANGLE				80
#define SORTED_POLYGON						81
#define SORTED_NUDGELEFT					82
#define SORTED_NUDGERIGHT					83
#define SORTED_SAVENUDGE					84
#define SORTED_ADJUSTABLECALLOUT			85
#define SORTED_UNUSED1						86

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

extern const short ImageMap[TMTB_MAXBUTTONS];
extern const short Sorted[TMTB_MAXBUTTONS];

#endif