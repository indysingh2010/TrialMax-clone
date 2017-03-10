//==============================================================================
//
// File Name:	tables.h
//
// Description:	This file defines all system states, events, and actions and
//				commands. Tables used to control system states and map commands
//				are declared in this file 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	10-27-97	1.00		Original Release
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

//	Command identifiers
#define TMAX_NOCOMMAND					0
#define TMAX_CALLOUT					1
#define TMAX_CLEAR						2
#define TMAX_CONFIG						3
#define TMAX_DRAWTOOL					4
#define TMAX_ERASE						5
#define TMAX_PRINT						6
#define TMAX_FIRSTZAP					7
#define TMAX_NEXTZAP					8
#define TMAX_PREVZAP					9
#define TMAX_LASTZAP					10
#define TMAX_HIGHLIGHT					11
#define TMAX_STATUSBAR					12
#define TMAX_NEXTPAGE					13
#define TMAX_PREVPAGE					14
#define TMAX_NORMAL						15
#define TMAX_PAN						16
#define TMAX_PLAY						17
#define TMAX_REDACT						18
#define TMAX_ROTATECCW					19
#define TMAX_ROTATECW					20
#define TMAX_SAVEZAP					21
#define TMAX_ZOOM						22
#define TMAX_ZOOMWIDTH					23
#define TMAX_STARTMOVIE					24
#define TMAX_ENDMOVIE					25
#define TMAX_BACKMOVIE					26
#define TMAX_FWDMOVIE					27
#define TMAX_FIRSTDESIGNATION			28
#define TMAX_LASTDESIGNATION			29
#define TMAX_NEXTDESIGNATION			30
#define TMAX_PREVDESIGNATION			31
#define TMAX_BACKDESIGNATION			32
#define TMAX_FWDDESIGNATION				33
#define TMAX_STARTDESIGNATION			34
#define TMAX_CONFIGTOOLBARS				35
#define TMAX_DISABLELINKS				36
#define TMAX_RED						37
#define TMAX_GREEN						38
#define TMAX_BLUE						39
#define	TMAX_YELLOW						40
#define TMAX_BLACK						41
#define TMAX_WHITE						42
#define TMAX_SPLITVERTICAL				43
#define TMAX_EXIT						44
#define TMAX_SWITCHPANE					45
#define TMAX_FIRSTPAGE					46
#define TMAX_LASTPAGE					47
#define TMAX_NEWPAGE					48
#define TMAX_FILTERPROPS				49
#define TMAX_SETPAGELINE				50
#define TMAX_SETPAGELINENEXT			51
#define TMAX_DELETEANN					52
#define TMAX_SELECT						53
#define TMAX_VIDEOCAPTION				54
#define TMAX_PLAYTHROUGH				55
#define TMAX_HELP						56
#define TMAX_TEXT						57
#define TMAX_FREEHAND					58
#define TMAX_LINE						59
#define TMAX_ARROW						60
#define TMAX_ELLIPSE					61
#define TMAX_RECTANGLE					62
#define TMAX_FILLEDELLIPSE				63
#define TMAX_FILLEDRECTANGLE			64
#define TMAX_SELECTTOOL					65
#define TMAX_FULLSCREEN					66
#define TMAX_DARKRED					67
#define TMAX_DARKGREEN					68
#define TMAX_DARKBLUE					69
#define TMAX_LIGHTRED					70
#define TMAX_LIGHTGREEN					71
#define TMAX_LIGHTBLUE					72
#define TMAX_POLYLINE					73
#define TMAX_POLYGON					74
#define TMAX_ANNTEXT					75
#define TMAX_NEXTMEDIA					76
#define TMAX_PREVMEDIA					77
#define TMAX_FIRSTSHOWITEM				78
#define TMAX_PREVSHOWITEM				79
#define TMAX_NEXTSHOWITEM				80
#define TMAX_LASTSHOWITEM				81
#define TMAX_MOUSEMODE					82
#define TMAX_SHOWTOOLBAR				83
#define TMAX_ZOOMRESTRICTED				84
#define TMAX_SPLITHORIZONTAL			85
#define TMAX_NEXTPAGE_HORIZONTAL		86
#define TMAX_NEXTPAGE_VERTICAL			87
#define TMAX_SCREENCAPTURE				88
#define TMAX_SHADEONCALLOUT				89
#define TMAX_NEXT_BARCODE				90
#define TMAX_PREV_BARCODE				91
#define TMAX_ADD_TO_BINDER				92
#define TMAX_UPDATE_ZAP					93
#define TMAX_SPLITPAGES_NEXT			94
#define TMAX_SPLITPAGES_PREVIOUS		95
#define TMAX_SAVE_SPLIT_ZAP				96
#define TMAX_GESTURE_PAN				97
#define TMAX_BINDERLIST					98
#define TMAX_NUDGELEFT					99
#define TMAX_NUDGERIGHT					100
#define TMAX_SAVENUDGE					101
#define TMAX_ADJUSTABLECALLOUT			102
#define MAX_COMMANDS					103	//	UPDATE THIS WHEN COMMANDS ARE
											//	ADDED OR REMOVED !!!

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	System States
enum States
{
	S_CLEAR = 0,
	S_PLAYLIST,
	S_DOCUMENT,
	S_GRAPHIC,
	S_MOVIE,
	S_LINKEDIMAGE,
	S_TEXT,
	S_POWERPOINT,
	S_LINKEDPOWER,
	MAX_STATES,
	S_DOCUMENT_LARGE
};

//	System Events
enum Events
{
	E_LOADPLAYLIST = 0,		
	E_LOADDOCUMENT,
	E_LOADGRAPHIC,
	E_LOADMOVIE,
	E_LOADTEXT,
	E_LINKDOCUMENT,
	E_LINKGRAPHIC,
	E_LOADPOWER,
	E_LINKPOWER,
	MAX_EVENTS
};

//	System Actions
enum Actions
{
	A_NONE = 0,
	A_LOADPLAYLIST,
	A_LOADDOCUMENT,
	A_LOADGRAPHIC,
	A_LOADMOVIE,
	A_LOADTEXT,
	A_LINKDOCUMENT,
	A_LINKGRAPHIC,
	A_LOADPOWER,
	A_LINKPOWER,
	MAX_ACTIONS
};

//	This structure is used to transition between states
typedef struct
{
	short sAction;
	short sNextState;

}STransition;

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

extern const STransition STT[MAX_EVENTS][MAX_STATES];
extern const BOOL		 EnableCommand[MAX_COMMANDS][MAX_STATES];
extern const short		 ButtonMap[TMTB_MAXBUTTONS][MAX_STATES];

#endif