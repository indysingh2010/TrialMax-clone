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
#define SORTED_CLEAR						2
#define SORTED_ROTATECW						3
#define SORTED_ROTATECCW					4
#define SORTED_NORMAL						5
#define	SORTED_ZOOM							6
#define SORTED_ZOOMRESTRICTED				7
#define SORTED_ZOOMWIDTH					8
#define SORTED_PAN							9
#define SORTED_CALLOUT						10
#define SORTED_DRAWTOOL						11
#define SORTED_SELECTTOOL					12
#define SORTED_HIGHLIGHT					13
#define SORTED_REDACT						14
#define SORTED_ERASE						15
#define SORTED_DELETEANN					16
#define SORTED_SELECT						17
#define SORTED_FIRSTPAGE					18
#define SORTED_PREVPAGE						19
#define SORTED_NEXTPAGE						20
#define SORTED_LASTPAGE						21
#define SORTED_SAVEZAP						22
#define SORTED_SAVESPLITZAP					23
#define SORTED_UPDATEZAP					24
#define SORTED_DELETEZAP					25
#define SORTED_FIRSTZAP						26
#define SORTED_PREVZAP						27
#define SORTED_NEXTZAP						28
#define SORTED_LASTZAP						29
#define SORTED_STARTMOVIE					30
#define SORTED_BACKMOVIE					31
#define SORTED_PAUSEMOVIE					32
#define SORTED_PLAYMOVIE					33
#define SORTED_FWDMOVIE						34
#define SORTED_ENDMOVIE						35
#define SORTED_FIRSTDESIGNATION				36
#define SORTED_BACKDESIGNATION				37
#define SORTED_PREVDESIGNATION				38
#define SORTED_STARTDESIGNATION				39
#define SORTED_PAUSEDESIGNATION				40
#define SORTED_PLAYDESIGNATION				41
#define SORTED_NEXTDESIGNATION				42
#define SORTED_FWDDESIGNATION				43
#define SORTED_LASTDESIGNATION				44
#define SORTED_PLAYTHROUGH					45
#define SORTED_CUEPGLNCURRENT				46
#define SORTED_CUEPGLNNEXT					47
#define SORTED_PRINT						48
#define SORTED_SPLITVERTICAL				49
#define SORTED_SPLITHORIZONTAL				50
#define SORTED_DISABLELINKS					51
#define SORTED_ENABLELINKS					52
#define SORTED_STATUSBAR					53
#define SORTED_TEXT							54
#define SORTED_FULLSCREEN					55
#define SORTED_SHADEDCALLOUTS				56
#define SORTED_EXIT							57
#define SORTED_DARKRED						58
#define SORTED_RED							59
#define SORTED_LIGHTRED						60
#define SORTED_DARKGREEN					61
#define SORTED_GREEN						62
#define SORTED_LIGHTGREEN					63
#define SORTED_DARKBLUE						64
#define SORTED_BLUE							65
#define SORTED_LIGHTBLUE					66
#define SORTED_YELLOW						67
#define SORTED_BLACK						68
#define SORTED_WHITE						69
#define SORTED_ANNTEXT						70
#define SORTED_FREEHAND						71
#define SORTED_LINE							72
#define SORTED_ARROW						73
#define SORTED_ELLIPSE						74
#define SORTED_RECTANGLE					75
#define SORTED_POLYLINE						76
#define SORTED_FILLEDELLIPSE				77
#define SORTED_FILLEDRECTANGLE				78
#define SORTED_POLYGON						79
#define SORTED_UNUSED1						80

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

extern const short ImageMap[TMTB_MAXBUTTONS];
extern const short Sorted[TMTB_MAXBUTTONS];

#endif