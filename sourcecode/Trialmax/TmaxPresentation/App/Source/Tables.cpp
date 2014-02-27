//==============================================================================
//
// File Name:   tables.cpp
//
// Description: This file contains the tables used to control system states and
//              map commands.
//
// Functions:   
//              
// See Also:    states.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//  Date        Revision    Description
//  10-27-97    1.00        Original Release
//==============================================================================

//------------------------------------------------------------------------------
//  INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tables.h>

//------------------------------------------------------------------------------
//  DEFINES
//------------------------------------------------------------------------------
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

//------------------------------------------------------------------------------
//  GLOBALS
//------------------------------------------------------------------------------

//  The system's state transition table is used to transition between states
//  when events are triggered. State changes determine the current display
//  mode. The action determines what work has to be done before switching
//  screen states.
const STransition STT[MAX_EVENTS][MAX_STATES] = {
    
    //  E_LOADPLAYLIST
    A_LOADPLAYLIST,         S_PLAYLIST,		//  S_CLEAR
    A_LOADPLAYLIST,         S_PLAYLIST,		//  S_PLAYLIST
    A_LOADPLAYLIST,         S_PLAYLIST,		//  S_DOCUMENT
    A_LOADPLAYLIST,         S_PLAYLIST,		//  S_GRAPHIC
    A_LOADPLAYLIST,         S_PLAYLIST,		//  S_MOVIE
    A_LOADPLAYLIST,         S_PLAYLIST,		//  S_LINKEDIMAGE
    A_LOADPLAYLIST,         S_PLAYLIST,		//  S_TEXT
    A_LOADPLAYLIST,			S_PLAYLIST,		//  S_POWERPOINT
    A_LOADPLAYLIST,			S_PLAYLIST,		//  S_LINKEDPOWER

    //  E_LOADDOCUMENT
    A_LOADDOCUMENT,			S_DOCUMENT,		//  S_CLEAR
    A_LOADDOCUMENT,         S_DOCUMENT,		//  S_PLAYLIST
    A_LOADDOCUMENT,         S_DOCUMENT,		//  S_DOCUMENT
    A_LOADDOCUMENT,         S_DOCUMENT,		//  S_GRAPHIC
    A_LOADDOCUMENT,         S_DOCUMENT,		//  S_MOVIE
    A_LOADDOCUMENT,         S_DOCUMENT,		//  S_LINKEDIMAGE
    A_LOADDOCUMENT,         S_DOCUMENT,		//  S_TEXT
    A_LOADDOCUMENT,			S_DOCUMENT,		//  S_POWERPOINT
    A_LOADDOCUMENT,			S_DOCUMENT,		//  S_LINKEDPOWER
											
    //  E_LOADGRAPHIC						
    A_LOADGRAPHIC,          S_GRAPHIC,		//  S_CLEAR
    A_LOADGRAPHIC,          S_GRAPHIC,		//  S_PLAYLIST
    A_LOADGRAPHIC,          S_GRAPHIC,		//  S_DOCUMENT
    A_LOADGRAPHIC,          S_GRAPHIC,		//  S_GRAPHIC
    A_LOADGRAPHIC,          S_GRAPHIC,		//  S_MOVIE
    A_LOADGRAPHIC,          S_GRAPHIC,		//  S_LINKEDIMAGE
    A_LOADGRAPHIC,          S_GRAPHIC,		//  S_TEXT
    A_LOADGRAPHIC,			S_GRAPHIC,		//  S_POWERPOINT
    A_LOADGRAPHIC,			S_GRAPHIC,		//  S_LINKEDPOWER
											
    //  E_LOADMOVIE							
    A_LOADMOVIE,            S_MOVIE,		//  S_CLEAR
    A_LOADMOVIE,            S_MOVIE,		//  S_PLAYLIST
    A_LOADMOVIE,            S_MOVIE,		//  S_DOCUMENT
    A_LOADMOVIE,            S_MOVIE,		//  S_GRAPHIC
    A_LOADMOVIE,            S_MOVIE,		//  S_MOVIE
    A_LOADMOVIE,            S_MOVIE,		//  S_LINKEDIMAGE
    A_LOADMOVIE,            S_MOVIE,		//  S_TEXT
    A_LOADMOVIE,			S_MOVIE,		//  S_POWERPOINT
    A_LOADMOVIE,			S_MOVIE,		//  S_LINKEDPOWER

	//  E_LOADTEXT							
    A_LOADTEXT,             S_TEXT,			//  S_CLEAR
    A_LOADTEXT,             S_TEXT,			//  S_PLAYLIST
    A_LOADTEXT,             S_TEXT,			//  S_DOCUMENT
    A_LOADTEXT,             S_TEXT,			//  S_GRAPHIC
    A_LOADTEXT,             S_TEXT,			//  S_MOVIE
    A_LOADTEXT,             S_TEXT,			//  S_LINKEDIMAGE
    A_LOADTEXT,             S_TEXT,			//  S_TEXT
    A_LOADTEXT,				S_TEXT,			//  S_POWERPOINT
    A_LOADTEXT,				S_TEXT,			//  S_LINKEDPOWER

    //  E_LINKDOCUMENT
    A_LOADDOCUMENT,			S_DOCUMENT,		//  S_CLEAR
    A_LINKDOCUMENT,         S_LINKEDIMAGE,	//  S_PLAYLIST
    A_LOADDOCUMENT,         S_DOCUMENT,		//  S_DOCUMENT
    A_LOADDOCUMENT,         S_DOCUMENT,		//  S_GRAPHIC
    A_LINKDOCUMENT,         S_LINKEDIMAGE,	//  S_MOVIE
    A_LINKDOCUMENT,         S_LINKEDIMAGE,	//  S_LINKEDIMAGE
    A_LINKDOCUMENT,         S_LINKEDIMAGE,	//  S_TEXT
    A_LOADDOCUMENT,			S_DOCUMENT,		//  S_POWERPOINT
    A_LINKDOCUMENT,			S_LINKEDIMAGE,	//  S_LINKEDPOWER

    //  E_LINKGRAPHIC
    A_LOADGRAPHIC,          S_GRAPHIC,		//  S_CLEAR
    A_LINKGRAPHIC,          S_LINKEDIMAGE,	//  S_PLAYLIST
    A_LOADGRAPHIC,          S_GRAPHIC,		//  S_DOCUMENT
    A_LOADGRAPHIC,          S_GRAPHIC,		//  S_GRAPHIC
    A_LINKGRAPHIC,          S_LINKEDIMAGE,	//  S_MOVIE
    A_LINKGRAPHIC,          S_LINKEDIMAGE,	//  S_LINKEDIMAGE
    A_LINKGRAPHIC,          S_LINKEDIMAGE,	//  S_TEXT
    A_LOADGRAPHIC,			S_GRAPHIC,		//  S_POWERPOINT
    A_LINKGRAPHIC,			S_LINKEDIMAGE,	//  S_LINKEDPOWER

    //  E_LOADPOWER
    A_LOADPOWER,			S_POWERPOINT,	//  S_CLEAR
    A_LOADPOWER,			S_POWERPOINT,	//  S_PLAYLIST
    A_LOADPOWER,			S_POWERPOINT,	//  S_POWERPOINT
    A_LOADPOWER,			S_POWERPOINT,	//  S_GRAPHIC
    A_LOADPOWER,			S_POWERPOINT,	//  S_MOVIE
    A_LOADPOWER,			S_POWERPOINT,	//  S_LINKEDIMAGE
    A_LOADPOWER,			S_POWERPOINT,	//  S_TEXT
    A_LOADPOWER,			S_POWERPOINT,	//  S_POWERPOINT
    A_LOADPOWER,			S_POWERPOINT,	//  S_LINKEDPOWER
											
    //  E_LINKPOWER
    A_LOADPOWER,			S_POWERPOINT,	//  S_CLEAR
    A_LINKPOWER,			S_LINKEDPOWER,	//  S_PLAYLIST
    A_LOADPOWER,			S_POWERPOINT,	//  S_DOCUMENT
    A_LOADPOWER,			S_POWERPOINT,	//  S_POWER
    A_LINKPOWER,			S_LINKEDPOWER,	//  S_MOVIE
    A_LINKPOWER,			S_LINKEDPOWER,	//  S_LINKEDIMAGE
    A_LINKPOWER,			S_LINKEDPOWER,	//  S_TEXT
    A_LOADPOWER,			S_POWERPOINT,	//  S_POWERPOINT
    A_LINKPOWER,			S_LINKEDPOWER,	//  S_LINKEDPOWER									
};

//  This table is used to enable and disable commands based on screen state
const BOOL EnableCommand[MAX_COMMANDS][MAX_STATES] = {
    
//CLEAR		PLAYLIST    DOCUMENT    GRAPHIC     MOVIE   LINKDOC	TEXT	POWERPOINT	LINKPP
FALSE,      FALSE,      FALSE,      FALSE,      FALSE,  FALSE,  FALSE,  FALSE,		FALSE,		//  TMAX_NOCOMMAND
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_CALLOUT
TRUE,       TRUE,       TRUE,       TRUE,       TRUE,   TRUE,   TRUE,	TRUE,		TRUE,		//  TMAX_CLEAR
TRUE,       TRUE,       TRUE,       TRUE,       TRUE,   TRUE,   TRUE,	TRUE,		TRUE,		//  TMAX_CONFIG
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_DRAWTOOL
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_ERASE
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_PRINT
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_FIRSTZAP
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_NEXTZAP
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_PREVZAP
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_LASTZAP
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_HIGHLIGHT
TRUE,		TRUE,		TRUE,		TRUE,		TRUE,	TRUE,	TRUE,	TRUE,		TRUE,		//  TMAX_STATUSBAR
TRUE,       FALSE,      TRUE,       TRUE,       TRUE,	TRUE,	FALSE,  TRUE,		TRUE,		//  TMAX_NEXTPAGE
TRUE,       FALSE,      TRUE,       TRUE,       TRUE,	TRUE,	FALSE,  TRUE,		TRUE,		//  TMAX_PREVPAGE
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_NORMAL
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_PAN
FALSE,      TRUE,       FALSE,      FALSE,      TRUE,   TRUE,   TRUE,	FALSE,		TRUE,		//  TMAX_PLAY
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_REDACT
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_ROTATECCW
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_ROTATECW
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_SAVEZAP
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_ZOOM
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_ZOOMWIDTH
FALSE,      FALSE,      FALSE,      FALSE,      TRUE,   FALSE,  FALSE,  FALSE,		FALSE,		//  TMAX_STARTMOVIE
FALSE,      FALSE,      FALSE,      FALSE,      TRUE,   FALSE,  FALSE,  FALSE,		FALSE,		//  TMAX_ENDMOVIE
FALSE,      FALSE,      FALSE,      FALSE,      TRUE,   FALSE,  FALSE,  FALSE,		FALSE,		//  TMAX_BACKMOVIE
FALSE,      FALSE,      FALSE,      FALSE,      TRUE,   FALSE,  FALSE,  FALSE,		FALSE,		//  TMAX_FWDMOVIE
FALSE,      TRUE,       FALSE,      FALSE,      FALSE,  TRUE,   TRUE,	FALSE,		TRUE,		//  TMAX_FIRSTDESIGNATION
FALSE,      TRUE,       FALSE,      FALSE,      FALSE,  TRUE,   TRUE,	FALSE,		TRUE,		//  TMAX_LASTDESIGNATION
FALSE,      TRUE,       FALSE,      FALSE,      FALSE,  TRUE,   TRUE,	FALSE,		TRUE,		//  TMAX_NEXTDESIGNATION
FALSE,      TRUE,       FALSE,      FALSE,      FALSE,  TRUE,   TRUE,	FALSE,		TRUE,		//  TMAX_PREVDESIGNATION
FALSE,      TRUE,       FALSE,      FALSE,      FALSE,  TRUE,   TRUE,	FALSE,		TRUE,		//  TMAX_BACKDESIGNATION
FALSE,      TRUE,       FALSE,      FALSE,      FALSE,  TRUE,   TRUE,	FALSE,		TRUE,		//  TMAX_FWDDESIGNATION
FALSE,      TRUE,       FALSE,      FALSE,      FALSE,  TRUE,   TRUE,	FALSE,		TRUE,		//  TMAX_STARTDESIGNATION
TRUE,       TRUE,       TRUE,       TRUE,       TRUE,   TRUE,   TRUE,	TRUE,		TRUE,		//  TMAX_CONFIGTOOLBARS
TRUE,       TRUE,       TRUE,       TRUE,       TRUE,   TRUE,   TRUE,	FALSE,		TRUE,		//  TMAX_DISABLELINKS
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_RED
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_GREEN
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_BLUE
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_YELLOW 
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_BLACK
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_WHITE
TRUE,		FALSE,      TRUE,       TRUE,       FALSE,  FALSE,  FALSE,  FALSE,		FALSE,		//  TMAX_SPLITVERTICAL
TRUE,       TRUE,       TRUE,       TRUE,       TRUE,   TRUE,   TRUE,	TRUE,		TRUE,		//  TMAX_EXIT
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  FALSE,  FALSE,  FALSE,		FALSE,		//  TMAX_SWITCHPANE
TRUE,       FALSE,      TRUE,       TRUE,       TRUE,	TRUE,	TRUE,	TRUE,		TRUE,		//  TMAX_FIRSTPAGE
TRUE,       FALSE,      TRUE,       TRUE,       TRUE,	TRUE,	TRUE,	TRUE,		TRUE,		//  TMAX_LASTPAGE
FALSE,      FALSE,      TRUE,       TRUE,       TRUE,	TRUE,	TRUE,	TRUE,		TRUE,		//  TMAX_NEWPAGE
TRUE,		TRUE,		FALSE,      FALSE,      TRUE,	TRUE,	TRUE,	TRUE,		TRUE,		//  TMAX_FILTERPROPS
TRUE,       TRUE,       TRUE,       TRUE,       TRUE,   TRUE,	TRUE,	FALSE,		TRUE,		//  TMAX_SETPAGELINE
TRUE,		TRUE,		TRUE,		TRUE,		TRUE,	TRUE,	TRUE,	FALSE,		TRUE,		//  TMAX_SETPAGELINENEXT
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_DELETEANN
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_SELECT
TRUE,		TRUE,		TRUE,		TRUE,		TRUE,	TRUE,	TRUE,	FALSE,		TRUE,		//	TMAX_VIDEOCAPTION
FALSE,		TRUE,		FALSE,		FALSE,		TRUE,	TRUE,	TRUE,	FALSE,		TRUE,		//	TMAX_PLAYTHROUGH
TRUE,       TRUE,       TRUE,       TRUE,       TRUE,   TRUE,   TRUE,	TRUE,		TRUE,		//  TMAX_HELP
FALSE,      TRUE,		FALSE,      FALSE,      FALSE,  TRUE,	TRUE,	FALSE,		TRUE,		//  TMAX_TEXT
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_FREEHAND
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_LINE
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_ARROW
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_ELLIPSE
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_RECTANGLE
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_FILLEDELLIPSE
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_FILLEDRECTANGLE
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_SELECTTOOL
FALSE,      FALSE,      FALSE,      FALSE,      FALSE,  TRUE,	FALSE,  FALSE,		TRUE,		//  TMAX_FULLSCREEN
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_DARKRED
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_DARKGREEN
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_DARKBLUE
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_LIGHTRED
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_LIGHTGREEN
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_LIGHTBLUE
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_POLYLINE
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_POLYGON
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_ANNTEXT
FALSE,		TRUE,		TRUE,		TRUE,		TRUE,	TRUE,	TRUE,	TRUE,		TRUE,		//  TMAX_NEXTMEDIA
FALSE,		TRUE,		TRUE,		TRUE,		TRUE,	TRUE,	TRUE,	TRUE,		TRUE,		//  TMAX_PREVMEDIA
TRUE,		TRUE,		TRUE,		TRUE,		TRUE,	TRUE,	TRUE,	TRUE,		TRUE,		//  TMAX_FIRSTSHOWITEM
TRUE,		TRUE,		TRUE,		TRUE,		TRUE,	TRUE,	TRUE,	TRUE,		TRUE,		//  TMAX_PREVSHOWITEM
TRUE,		TRUE,		TRUE,		TRUE,		TRUE,	TRUE,	TRUE,	TRUE,		TRUE,		//  TMAX_NEXTSHOWITEM
TRUE,		TRUE,		TRUE,		TRUE,		TRUE,	TRUE,	TRUE,	TRUE,		TRUE,		//  TMAX_LASTSHOWITEM
TRUE,       TRUE,       TRUE,       TRUE,       TRUE,   TRUE,   TRUE,	TRUE,		TRUE,		//  TMAX_MOUSEMODE
TRUE,       TRUE,       TRUE,       TRUE,       TRUE,   TRUE,   TRUE,	TRUE,		TRUE,		//  TMAX_SHOWTOOLBAR
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_ZOOMRESTRICTED
TRUE,		FALSE,      TRUE,       TRUE,       FALSE,  FALSE,  FALSE,  FALSE,		FALSE,		//  TMAX_SPLITHORIZONTAL
FALSE,		FALSE,      TRUE,       TRUE,       FALSE,  FALSE,  FALSE,  FALSE,		FALSE,		//  TMAX_NEXTPAGE_HORIZONTAL
FALSE,		FALSE,      TRUE,       TRUE,       FALSE,  FALSE,  FALSE,  FALSE,		FALSE,		//  TMAX_NEXTPAGE_VERTICAL
TRUE,       TRUE,       TRUE,       TRUE,       TRUE,   TRUE,   TRUE,	TRUE,		TRUE,		//  TMAX_SCREENCAPTURE
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,   FALSE,  FALSE,		FALSE,		//  TMAX_SHADEONCALLOUT 
TRUE,       TRUE,       TRUE,       TRUE,       TRUE,   TRUE,   TRUE,	TRUE,		TRUE,		//  TMAX_NEXT_BARCODE
TRUE,		TRUE,		TRUE,       TRUE,       TRUE,	TRUE,   TRUE,	TRUE,		TRUE,		//  TMAX_PREV_BARCODE
FALSE,		TRUE,		TRUE,       TRUE,       TRUE,	TRUE,   TRUE,	TRUE,		TRUE,		//  TMAX_ADD_TO_BINDER
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_UPDATE_ZAP
FALSE,		FALSE,      TRUE,       TRUE,       FALSE,  FALSE,  FALSE,  FALSE,		FALSE,		//  TMAX_SPLITPAGES_NEXT
FALSE,		FALSE,      TRUE,       TRUE,       FALSE,  FALSE,  FALSE,  FALSE,		FALSE,		//  TMAX_SPLITPAGES_PREVIOUS
FALSE,      FALSE,      TRUE,       TRUE,       FALSE,  TRUE,	FALSE,  FALSE,		FALSE,		//  TMAX_SAVE_SPLIT_ZAP
//CLEAR		PLAYLIST    DOCUMENT    GRAPHIC     MOVIE   LINKDOC	TEXT	POWERPOINT	LINKPP
};

//  This table is used to translate TMTool button identifiers to TMAXPRESENTATION
//  command identifiers. The translation is based on screen states so that
//  we can map common buttons to different commands for different states
const short ButtonMap[TMTB_MAXBUTTONS][MAX_STATES] = {

    
//  TMTB_CONFIG
TMAX_CONFIG,                    //  S_CLEAR
TMAX_CONFIG,                    //  S_PLAYLIST
TMAX_CONFIG,                    //  S_DOCUMENT
TMAX_CONFIG,                    //  S_GRAPHIC
TMAX_CONFIG,                    //  S_MOVIE
TMAX_CONFIG,                    //  S_LINKEDIMAGE
TMAX_CONFIG,                    //  S_TEXT
TMAX_CONFIG,                    //  S_POWERPOINT
TMAX_CONFIG,                    //  S_LINKEDPOWER
            
//  TMTB_UNUSED1
TMAX_NOCOMMAND,                 //  S_CLEAR
TMAX_NOCOMMAND,                 //  S_PLAYLIST
TMAX_NOCOMMAND,                 //  S_DOCUMENT
TMAX_NOCOMMAND,                 //  S_GRAPHIC
TMAX_NOCOMMAND,                 //  S_MOVIE
TMAX_NOCOMMAND,                 //  S_LINKEDIMAGE
TMAX_NOCOMMAND,                 //  S_TEXT
TMAX_NOCOMMAND,                 //  S_POWERPOINT
TMAX_NOCOMMAND,                 //  S_LINKEDPOWER
            
//  TMTB_CONFIGTOOLBARS
TMAX_CONFIGTOOLBARS,            //  S_CLEAR
TMAX_CONFIGTOOLBARS,            //  S_PLAYLIST
TMAX_CONFIGTOOLBARS,            //  S_DOCUMENT
TMAX_CONFIGTOOLBARS,            //  S_GRAPHIC
TMAX_CONFIGTOOLBARS,            //  S_MOVIE
TMAX_CONFIGTOOLBARS,            //  S_LINKEDIMAGE
TMAX_CONFIGTOOLBARS,            //  S_TEXT
TMAX_CONFIGTOOLBARS,            //  S_POWERPOINT
TMAX_CONFIGTOOLBARS,            //  S_LINKEDPOWER
            
//  TMTB_CLEAR
TMAX_CLEAR,                     //  S_CLEAR
TMAX_CLEAR,                     //  S_PLAYLIST
TMAX_CLEAR,                     //  S_DOCUMENT
TMAX_CLEAR,                     //  S_GRAPHIC
TMAX_CLEAR,                     //  S_MOVIE
TMAX_CLEAR,                     //  S_LINKEDIMAGE
TMAX_CLEAR,                     //  S_TEXT
TMAX_CLEAR,                     //  S_POWERPOINT
TMAX_CLEAR,                     //	S_LINKEDPOWER
            
//  TMTB_ROTATECW
TMAX_ROTATECW,                  //  S_CLEAR
TMAX_ROTATECW,                  //  S_PLAYLIST
TMAX_ROTATECW,                  //  S_DOCUMENT
TMAX_ROTATECW,                  //  S_GRAPHIC
TMAX_ROTATECW,                  //  S_MOVIE
TMAX_ROTATECW,                  //  S_LINKEDIMAGE
TMAX_ROTATECW,                  //  S_TEXT
TMAX_ROTATECW,                  //  S_POWERPOINT
TMAX_ROTATECW,                  //  S_LINKEDPOWER
            
//  TMTB_ROTATECCW
TMAX_ROTATECCW,                 //  S_CLEAR
TMAX_ROTATECCW,                 //  S_PLAYLIST
TMAX_ROTATECCW,                 //  S_DOCUMENT
TMAX_ROTATECCW,                 //  S_GRAPHIC
TMAX_ROTATECCW,                 //  S_MOVIE
TMAX_ROTATECCW,                 //  S_LINKEDIMAGE
TMAX_ROTATECCW,                 //  S_TEXT
TMAX_ROTATECCW,                 //  S_POWERPOINT
TMAX_ROTATECCW,                 //  S_LINKEDPOWER
            
//  TMTB_NORMAL
TMAX_NORMAL,                    //  S_CLEAR
TMAX_NORMAL,                    //  S_PLAYLIST
TMAX_NORMAL,                    //  S_DOCUMENT
TMAX_NORMAL,                    //  S_GRAPHIC
TMAX_NORMAL,                    //  S_MOVIE
TMAX_NORMAL,                    //  S_LINKEDIMAGE
TMAX_NORMAL,                    //  S_TEXT
TMAX_NORMAL,                    //  S_POWERPOINT
TMAX_NORMAL,                    //  S_LINKEDPOWER
            
//  TMTB_ZOOM
TMAX_ZOOM,                      //  S_CLEAR
TMAX_ZOOM,                      //  S_PLAYLIST
TMAX_ZOOM,                      //  S_DOCUMENT
TMAX_ZOOM,                      //  S_GRAPHIC
TMAX_ZOOM,                      //  S_MOVIE
TMAX_ZOOM,                      //  S_LINKEDIMAGE
TMAX_ZOOM,                      //  S_TEXT
TMAX_ZOOM,                      //  S_POWERPOINT
TMAX_ZOOM,                      //  S_LINKEDPOWER
            
//  TMTB_ZOOMWIDTH
TMAX_ZOOMWIDTH,                 //  S_CLEAR
TMAX_ZOOMWIDTH,                 //  S_PLAYLIST
TMAX_ZOOMWIDTH,                 //  S_DOCUMENT
TMAX_ZOOMWIDTH,                 //  S_GRAPHIC
TMAX_ZOOMWIDTH,                 //  S_MOVIE
TMAX_ZOOMWIDTH,                 //  S_LINKEDIMAGE
TMAX_ZOOMWIDTH,                 //  S_TEXT
TMAX_ZOOMWIDTH,                 //  S_POWERPOINT
TMAX_ZOOMWIDTH,                 //  S_LINKEDPOWER
            
//  TMTB_PAN
TMAX_PAN,                       //  S_CLEAR
TMAX_PAN,                       //  S_PLAYLIST
TMAX_PAN,                       //  S_DOCUMENT
TMAX_PAN,                       //  S_GRAPHIC
TMAX_PAN,                       //  S_MOVIE
TMAX_PAN,                       //  S_LINKEDIMAGE
TMAX_PAN,                       //  S_TEXT
TMAX_PAN,                       //  S_POWERPOINT
TMAX_PAN,                       //  S_LINKEDPOWER
            
//  TMTB_CALLOUT
TMAX_CALLOUT,                   //  S_CLEAR
TMAX_CALLOUT,                   //  S_PLAYLIST
TMAX_CALLOUT,                   //  S_DOCUMENT
TMAX_CALLOUT,                   //  S_GRAPHIC
TMAX_CALLOUT,                   //  S_MOVIE
TMAX_CALLOUT,                   //  S_LINKEDIMAGE
TMAX_CALLOUT,                   //  S_TEXT
TMAX_CALLOUT,                   //  S_POWERPOINT
TMAX_CALLOUT,                   //  S_LINKEDPOWER
            
//  TMTB_DRAWTOOL
TMAX_DRAWTOOL,                  //  S_CLEAR
TMAX_DRAWTOOL,                  //  S_PLAYLIST
TMAX_DRAWTOOL,                  //  S_DOCUMENT
TMAX_DRAWTOOL,                  //  S_GRAPHIC
TMAX_DRAWTOOL,                  //  S_MOVIE
TMAX_DRAWTOOL,                  //  S_LINKEDIMAGE
TMAX_DRAWTOOL,                  //  S_TEXT
TMAX_DRAWTOOL,                  //  S_POWERPOINT
TMAX_DRAWTOOL,                  //  S_LINKEDPOWER
            
//  TMTB_HIGHLIGHT
TMAX_HIGHLIGHT,                 //  S_CLEAR
TMAX_HIGHLIGHT,                 //  S_PLAYLIST
TMAX_HIGHLIGHT,                 //  S_DOCUMENT
TMAX_HIGHLIGHT,                 //  S_GRAPHIC
TMAX_HIGHLIGHT,                 //  S_MOVIE
TMAX_HIGHLIGHT,                 //  S_LINKEDIMAGE
TMAX_HIGHLIGHT,                 //  S_TEXT
TMAX_HIGHLIGHT,                 //  S_POWERPOINT
TMAX_HIGHLIGHT,                 //  S_LINKEDPOWER
            
//  TMTB_REDACT
TMAX_REDACT,                    //  S_CLEAR
TMAX_REDACT,                    //  S_PLAYLIST
TMAX_REDACT,                    //  S_DOCUMENT
TMAX_REDACT,                    //  S_GRAPHIC
TMAX_REDACT,                    //  S_MOVIE
TMAX_REDACT,                    //  S_LINKEDIMAGE
TMAX_REDACT,                    //  S_TEXT
TMAX_REDACT,                    //  S_POWERPOINT
TMAX_REDACT,                    //  S_LINKEDPOWER
            
//  TMTB_ERASE
TMAX_ERASE,                     //  S_CLEAR
TMAX_ERASE,                     //  S_PLAYLIST
TMAX_ERASE,                     //  S_DOCUMENT
TMAX_ERASE,                     //  S_GRAPHIC
TMAX_ERASE,                     //  S_MOVIE
TMAX_ERASE,                     //  S_LINKEDIMAGE
TMAX_ERASE,                     //  S_TEXT
TMAX_ERASE,                     //  S_POWERPOINT
TMAX_ERASE,                     //  S_LINKEDPOWER
            
//  TMTB_FIRSTPAGE
TMAX_FIRSTPAGE,                 //  S_CLEAR
TMAX_FIRSTPAGE,                 //  S_PLAYLIST
TMAX_FIRSTPAGE,                 //  S_DOCUMENT
TMAX_FIRSTPAGE,                 //  S_GRAPHIC
TMAX_FIRSTPAGE,                 //  S_MOVIE
TMAX_FIRSTPAGE,                 //  S_LINKEDIMAGE
TMAX_FIRSTPAGE,                 //  S_TEXT
TMAX_FIRSTPAGE,                 //  S_POWERPOINT
TMAX_FIRSTPAGE,                 //  S_LINKEDPOWER
            
//  TMTB_PREVPAGE
TMAX_PREVPAGE,                  //  S_CLEAR
TMAX_PREVPAGE,                  //  S_PLAYLIST
TMAX_PREVPAGE,                  //  S_DOCUMENT
TMAX_PREVPAGE,                  //  S_GRAPHIC
TMAX_PREVPAGE,                  //  S_MOVIE
TMAX_PREVPAGE,                  //  S_LINKEDIMAGE
TMAX_PREVPAGE,                  //  S_TEXT
TMAX_PREVPAGE,                  //  S_POWERPOINT
TMAX_PREVPAGE,                  //  S_LINKEDPOWER
            
//  TMTB_NEXTPAGE
TMAX_NEXTPAGE,                  //  S_CLEAR
TMAX_NEXTPAGE,                  //  S_PLAYLIST
TMAX_NEXTPAGE,                  //  S_DOCUMENT
TMAX_NEXTPAGE,                  //  S_GRAPHIC
TMAX_NEXTPAGE,                  //  S_MOVIE
TMAX_NEXTPAGE,                  //  S_LINKEDIMAGE
TMAX_NEXTPAGE,                  //  S_TEXT
TMAX_NEXTPAGE,                  //  S_POWERPOINT
TMAX_NEXTPAGE,                  //  S_LINKEDPOWER
            
//  TMTB_LASTPAGE
TMAX_LASTPAGE,                  //  S_CLEAR
TMAX_LASTPAGE,                  //  S_PLAYLIST
TMAX_LASTPAGE,                  //  S_DOCUMENT
TMAX_LASTPAGE,                  //  S_GRAPHIC
TMAX_LASTPAGE,                  //  S_MOVIE
TMAX_LASTPAGE,                  //  S_LINKEDIMAGE
TMAX_LASTPAGE,                  //  S_TEXT
TMAX_LASTPAGE,                  //  S_POWERPOINT
TMAX_LASTPAGE,                  //  S_LINKEDPOWER
            
//  TMTB_SAVEZAP
TMAX_SAVEZAP,                   //  S_CLEAR
TMAX_SAVEZAP,                   //  S_PLAYLIST
TMAX_SAVEZAP,                   //  S_DOCUMENT
TMAX_SAVEZAP,                   //  S_GRAPHIC
TMAX_SAVEZAP,                   //  S_MOVIE
TMAX_SAVEZAP,                   //  S_LINKEDIMAGE
TMAX_SAVEZAP,                   //  S_TEXT
TMAX_SAVEZAP,                   //  S_POWERPOINT
TMAX_SAVEZAP,                   //  S_LINKEDPOWER
            
//  TMTB_FIRSTZAP
TMAX_FIRSTZAP,                  //  S_CLEAR
TMAX_FIRSTZAP,                  //  S_PLAYLIST
TMAX_FIRSTZAP,                  //  S_DOCUMENT
TMAX_FIRSTZAP,                  //  S_GRAPHIC
TMAX_FIRSTZAP,                  //  S_MOVIE
TMAX_FIRSTZAP,                  //  S_LINKEDIMAGE
TMAX_FIRSTZAP,                  //  S_TEXT
TMAX_FIRSTZAP,                  //  S_POWERPOINT
TMAX_FIRSTZAP,                  //  S_LINKEDPOWER
            
//  TMTB_PREVZAP
TMAX_PREVZAP,                   //  S_CLEAR
TMAX_PREVZAP,                   //  S_PLAYLIST
TMAX_PREVZAP,                   //  S_DOCUMENT
TMAX_PREVZAP,                   //  S_GRAPHIC
TMAX_PREVZAP,                   //  S_MOVIE
TMAX_PREVZAP,                   //  S_LINKEDIMAGE
TMAX_PREVZAP,                   //  S_TEXT
TMAX_PREVZAP,                   //  S_POWERPOINT
TMAX_PREVZAP,                   //  S_LINKEDPOWER
            
//  TMTB_NEXTZAP
TMAX_NEXTZAP,                   //  S_CLEAR
TMAX_NEXTZAP,                   //  S_PLAYLIST
TMAX_NEXTZAP,                   //  S_DOCUMENT
TMAX_NEXTZAP,                   //  S_GRAPHIC
TMAX_NEXTZAP,                   //  S_MOVIE
TMAX_NEXTZAP,                   //  S_LINKEDIMAGE
TMAX_NEXTZAP,                   //  S_TEXT
TMAX_NEXTZAP,                   //  S_POWERPOINT
TMAX_NEXTZAP,                   //  S_LINKEDPOWER
            
//  TMTB_LASTZAP
TMAX_LASTZAP,                   //  S_CLEAR
TMAX_LASTZAP,                   //  S_PLAYLIST
TMAX_LASTZAP,                   //  S_DOCUMENT
TMAX_LASTZAP,                   //  S_GRAPHIC
TMAX_LASTZAP,                   //  S_MOVIE
TMAX_LASTZAP,                   //  S_LINKEDIMAGE
TMAX_LASTZAP,                   //  S_TEXT
TMAX_LASTZAP,                   //  S_POWERPOINT
TMAX_LASTZAP,                   //  S_LINKEDPOWER
            
//  TMTB_STARTMOVIE
TMAX_STARTMOVIE,                //  S_CLEAR
TMAX_STARTMOVIE,                //  S_PLAYLIST
TMAX_STARTMOVIE,                //  S_DOCUMENT
TMAX_STARTMOVIE,                //  S_GRAPHIC
TMAX_STARTMOVIE,                //  S_MOVIE
TMAX_STARTMOVIE,                //  S_LINKEDIMAGE
TMAX_STARTMOVIE,                //  S_TEXT
TMAX_STARTMOVIE,                //  S_POWERPOINT
TMAX_STARTMOVIE,                //  S_LINKEDPOWER
            
//  TMTB_BACKMOVIE
TMAX_BACKMOVIE,                 //  S_CLEAR
TMAX_BACKMOVIE,                 //  S_PLAYLIST
TMAX_BACKMOVIE,                 //  S_DOCUMENT
TMAX_BACKMOVIE,                 //  S_GRAPHIC
TMAX_BACKMOVIE,                 //  S_MOVIE
TMAX_BACKMOVIE,                 //  S_LINKEDIMAGE
TMAX_BACKMOVIE,                 //  S_TEXT
TMAX_BACKMOVIE,                 //  S_POWERPOINT
TMAX_BACKMOVIE,                 //  S_LINKEDPOWER
            
//  TMTB_PAUSEMOVIE
TMAX_PLAY,                      //  S_CLEAR
TMAX_PLAY,                      //  S_PLAYLIST
TMAX_PLAY,                      //  S_DOCUMENT
TMAX_PLAY,                      //  S_GRAPHIC
TMAX_PLAY,                      //  S_MOVIE
TMAX_PLAY,                      //  S_LINKEDIMAGE
TMAX_PLAY,                      //  S_TEXT
TMAX_PLAY,                      //  S_POWERPOINT
TMAX_PLAY,                      //  S_LINKEDPOWER
            
//  TMTB_PLAYMOVIE
TMAX_PLAY,                      //  S_CLEAR
TMAX_PLAY,                      //  S_PLAYLIST
TMAX_PLAY,                      //  S_DOCUMENT
TMAX_PLAY,                      //  S_GRAPHIC
TMAX_PLAY,                      //  S_MOVIE
TMAX_PLAY,                      //  S_LINKEDIMAGE
TMAX_PLAY,                      //  S_TEXT
TMAX_PLAY,                      //  S_POWERPOINT
TMAX_PLAY,                      //  S_LINKEDPOWER
            
//  TMTB_FWDMOVIE
TMAX_FWDMOVIE,                  //  S_CLEAR
TMAX_FWDMOVIE,                  //  S_PLAYLIST
TMAX_FWDMOVIE,                  //  S_DOCUMENT
TMAX_FWDMOVIE,                  //  S_GRAPHIC
TMAX_FWDMOVIE,                  //  S_MOVIE
TMAX_FWDMOVIE,                  //  S_LINKEDIMAGE
TMAX_FWDMOVIE,                  //  S_TEXT
TMAX_FWDMOVIE,                  //  S_POWERPOINT
TMAX_FWDMOVIE,                  //  S_LINKEDPOWER
            
//  TMTB_ENDMOVIE
TMAX_ENDMOVIE,                  //  S_CLEAR
TMAX_ENDMOVIE,                  //  S_PLAYLIST
TMAX_ENDMOVIE,                  //  S_DOCUMENT
TMAX_ENDMOVIE,                  //  S_GRAPHIC
TMAX_ENDMOVIE,                  //  S_MOVIE
TMAX_ENDMOVIE,                  //  S_LINKEDIMAGE
TMAX_ENDMOVIE,                  //  S_TEXT
TMAX_ENDMOVIE,                  //  S_POWERPOINT
TMAX_ENDMOVIE,                  //  S_LINKEDPOWER
            
//  TMTB_FIRSTDESIGNATION
TMAX_FIRSTDESIGNATION,          //  S_CLEAR
TMAX_FIRSTDESIGNATION,          //  S_PLAYLIST
TMAX_FIRSTDESIGNATION,          //  S_DOCUMENT
TMAX_FIRSTDESIGNATION,          //  S_GRAPHIC
TMAX_FIRSTDESIGNATION,          //  S_MOVIE
TMAX_FIRSTDESIGNATION,          //  S_LINKEDIMAGE
TMAX_FIRSTDESIGNATION,          //  S_TEXT
TMAX_FIRSTDESIGNATION,          //  S_POWERPOINT
TMAX_FIRSTDESIGNATION,          //  S_LINKEDPOWER
            
//  TMTB_BACKDESIGNATION
TMAX_BACKDESIGNATION,           //  S_CLEAR
TMAX_BACKDESIGNATION,           //  S_PLAYLIST
TMAX_BACKDESIGNATION,           //  S_DOCUMENT
TMAX_BACKDESIGNATION,           //  S_GRAPHIC
TMAX_BACKDESIGNATION,           //  S_MOVIE
TMAX_BACKDESIGNATION,           //  S_LINKEDIMAGE
TMAX_BACKDESIGNATION,           //  S_TEXT
TMAX_BACKDESIGNATION,           //  S_POWERPOINT
TMAX_BACKDESIGNATION,           //  S_LINKEDPOWER
            
//  TMTB_PREVDESIGNATION
TMAX_PREVDESIGNATION,           //  S_CLEAR
TMAX_PREVDESIGNATION,           //  S_PLAYLIST
TMAX_PREVDESIGNATION,           //  S_DOCUMENT
TMAX_PREVDESIGNATION,           //  S_GRAPHIC
TMAX_PREVDESIGNATION,           //  S_MOVIE
TMAX_PREVDESIGNATION,           //  S_LINKEDIMAGE
TMAX_PREVDESIGNATION,           //  S_TEXT
TMAX_PREVDESIGNATION,           //  S_POWERPOINT
TMAX_PREVDESIGNATION,           //  S_LINKEDPOWER
            
//  TMTB_STARTDESIGNATION
TMAX_STARTDESIGNATION,          //  S_CLEAR
TMAX_STARTDESIGNATION,          //  S_PLAYLIST
TMAX_STARTDESIGNATION,          //  S_DOCUMENT
TMAX_STARTDESIGNATION,          //  S_GRAPHIC
TMAX_STARTDESIGNATION,          //  S_MOVIE
TMAX_STARTDESIGNATION,          //  S_LINKEDIMAGE
TMAX_STARTDESIGNATION,          //  S_TEXT
TMAX_STARTDESIGNATION,          //  S_POWERPOINT
TMAX_STARTDESIGNATION,          //  S_LINKEDPOWER
            
//  TMTB_PAUSEDESIGNATION
TMAX_PLAY,                      //  S_CLEAR
TMAX_PLAY,                      //  S_PLAYLIST
TMAX_PLAY,                      //  S_DOCUMENT
TMAX_PLAY,                      //  S_GRAPHIC
TMAX_PLAY,                      //  S_MOVIE
TMAX_PLAY,                      //  S_LINKEDIMAGE
TMAX_PLAY,                      //  S_TEXT
TMAX_PLAY,                      //  S_POWERPOINT
TMAX_PLAY,                      //  S_LINKEDPOWER
            
//  TMTB_PLAYDESIGNATION
TMAX_PLAY,                      //  S_CLEAR
TMAX_PLAY,                      //  S_PLAYLIST
TMAX_PLAY,                      //  S_DOCUMENT
TMAX_PLAY,                      //  S_GRAPHIC
TMAX_PLAY,                      //  S_MOVIE
TMAX_PLAY,                      //  S_LINKEDIMAGE
TMAX_PLAY,                      //  S_TEXT
TMAX_PLAY,                      //  S_POWERPOINT
TMAX_PLAY,                      //  S_LINKEDPOWER
           
//  TMTB_NEXTDESIGNATION
TMAX_NEXTDESIGNATION,           //  S_CLEAR
TMAX_NEXTDESIGNATION,           //  S_PLAYLIST
TMAX_NEXTDESIGNATION,           //  S_DOCUMENT
TMAX_NEXTDESIGNATION,           //  S_GRAPHIC
TMAX_NEXTDESIGNATION,           //  S_MOVIE
TMAX_NEXTDESIGNATION,           //  S_LINKEDIMAGE
TMAX_NEXTDESIGNATION,           //  S_TEXT
TMAX_NEXTDESIGNATION,           //  S_POWERPOINT
TMAX_NEXTDESIGNATION,           //  S_LINKEDPOWER
            
//  TMTB_FWDDESIGNATION
TMAX_FWDDESIGNATION,            //  S_CLEAR
TMAX_FWDDESIGNATION,            //  S_PLAYLIST
TMAX_FWDDESIGNATION,            //  S_DOCUMENT
TMAX_FWDDESIGNATION,            //  S_GRAPHIC
TMAX_FWDDESIGNATION,            //  S_MOVIE
TMAX_FWDDESIGNATION,            //  S_LINKEDIMAGE
TMAX_FWDDESIGNATION,            //  S_TEXT
TMAX_FWDDESIGNATION,            //  S_POWERPOINT
TMAX_FWDDESIGNATION,            //  S_LINKEDPOWER
            
//  TMTB_LASTDESIGNATION
TMAX_LASTDESIGNATION,           //  S_CLEAR
TMAX_LASTDESIGNATION,           //  S_PLAYLIST
TMAX_LASTDESIGNATION,           //  S_DOCUMENT
TMAX_LASTDESIGNATION,           //  S_GRAPHIC
TMAX_LASTDESIGNATION,           //  S_MOVIE
TMAX_LASTDESIGNATION,           //  S_LINKEDIMAGE
TMAX_LASTDESIGNATION,           //  S_TEXT
TMAX_LASTDESIGNATION,           //  S_POWERPOINT
TMAX_LASTDESIGNATION,           //  S_LINKEDPOWER
            
//  TMTB_PRINT
TMAX_PRINT,                     //  S_CLEAR
TMAX_PRINT,                     //  S_PLAYLIST
TMAX_PRINT,                     //  S_DOCUMENT
TMAX_PRINT,                     //  S_GRAPHIC
TMAX_PRINT,                     //  S_MOVIE
TMAX_PRINT,                     //  S_LINKEDIMAGE
TMAX_PRINT,                     //  S_TEXT
TMAX_PRINT,                     //  S_POWERPOINT
TMAX_PRINT,                     //  S_LINKEDPOWER
            
//  TMTB_SPLITVERTICAL
TMAX_SPLITVERTICAL,             //  S_CLEAR
TMAX_SPLITVERTICAL,             //  S_PLAYLIST
TMAX_SPLITVERTICAL,             //  S_DOCUMENT
TMAX_SPLITVERTICAL,             //  S_GRAPHIC
TMAX_SPLITVERTICAL,             //  S_MOVIE
TMAX_SPLITVERTICAL,             //  S_LINKEDIMAGE
TMAX_SPLITVERTICAL,             //  S_TEXT
TMAX_SPLITVERTICAL,             //  S_POWERPOINT
TMAX_SPLITVERTICAL,             //  S_LINKEDPOWER
            
//  TMTB_SPLITHORIZONAL
TMAX_SPLITHORIZONTAL,           //  S_CLEAR
TMAX_SPLITHORIZONTAL,           //  S_PLAYLIST
TMAX_SPLITHORIZONTAL,           //  S_DOCUMENT
TMAX_SPLITHORIZONTAL,           //  S_GRAPHIC
TMAX_SPLITHORIZONTAL,           //  S_MOVIE
TMAX_SPLITHORIZONTAL,           //  S_LINKEDIMAGE
TMAX_SPLITHORIZONTAL,           //  S_TEXT
TMAX_SPLITHORIZONTAL,           //  S_POWERPOINT
TMAX_SPLITHORIZONTAL,           //  S_LINKEDPOWER
            
//  TMTB_DISABLELINKS
TMAX_DISABLELINKS,              //  S_CLEAR
TMAX_DISABLELINKS,              //  S_PLAYLIST
TMAX_DISABLELINKS,              //  S_DOCUMENT
TMAX_DISABLELINKS,              //  S_GRAPHIC
TMAX_DISABLELINKS,              //  S_MOVIE
TMAX_DISABLELINKS,              //  S_LINKEDIMAGE
TMAX_DISABLELINKS,              //  S_TEXT
TMAX_DISABLELINKS,              //  S_POWERPOINT
TMAX_DISABLELINKS,              //  S_LINKEDPOWER
            
//  TMTB_ENABLELINKS
TMAX_DISABLELINKS,              //  S_CLEAR
TMAX_DISABLELINKS,              //  S_PLAYLIST
TMAX_DISABLELINKS,              //  S_DOCUMENT
TMAX_DISABLELINKS,              //  S_GRAPHIC
TMAX_DISABLELINKS,              //  S_MOVIE
TMAX_DISABLELINKS,              //  S_LINKEDIMAGE
TMAX_DISABLELINKS,              //  S_TEXT
TMAX_DISABLELINKS,              //  S_POWERPOINT
TMAX_DISABLELINKS,              //  S_LINKEDPOWER
            
//  TMTB_EXIT
TMAX_EXIT,						//  S_CLEAR
TMAX_EXIT,						//  S_PLAYLIST
TMAX_EXIT,						//  S_DOCUMENT
TMAX_EXIT,						//  S_GRAPHIC
TMAX_EXIT,						//  S_MOVIE
TMAX_EXIT,						//  S_LINKEDIMAGE
TMAX_EXIT,						//  S_TEXT
TMAX_EXIT,                      //  S_POWERPOINT
TMAX_EXIT,                      //  S_LINKEDPOWER

//  TMTB_RED
TMAX_RED,                       //  S_CLEAR
TMAX_RED,                       //  S_PLAYLIST
TMAX_RED,                       //  S_DOCUMENT
TMAX_RED,                       //  S_GRAPHIC
TMAX_RED,                       //  S_MOVIE
TMAX_RED,                       //  S_LINKEDIMAGE
TMAX_RED,                       //  S_TEXT
TMAX_RED,                       //  S_POWERPOINT
TMAX_RED,                       //  S_LINKEDPOWER
            
//  TMTB_GREEN
TMAX_GREEN,                     //  S_CLEAR
TMAX_GREEN,                     //  S_PLAYLIST
TMAX_GREEN,                     //  S_DOCUMENT
TMAX_GREEN,                     //  S_GRAPHIC
TMAX_GREEN,                     //  S_MOVIE
TMAX_GREEN,                     //  S_LINKEDIMAGE
TMAX_GREEN,                     //  S_TEXT
TMAX_GREEN,                     //  S_POWERPOINT
TMAX_GREEN,                     //  S_LINKEDPOWER
            
//  TMTB_BLUE
TMAX_BLUE,                      //  S_CLEAR
TMAX_BLUE,                      //  S_PLAYLIST
TMAX_BLUE,                      //  S_DOCUMENT
TMAX_BLUE,                      //  S_GRAPHIC
TMAX_BLUE,                      //  S_MOVIE
TMAX_BLUE,                      //  S_LINKEDIMAGE
TMAX_BLUE,                      //  S_TEXT
TMAX_BLUE,                      //  S_POWERPOINT
TMAX_BLUE,                      //  S_LINKEDPOWER
            
//  TMTB_YELLOW
TMAX_YELLOW,                    //  S_CLEAR
TMAX_YELLOW,                    //  S_PLAYLIST
TMAX_YELLOW,                    //  S_DOCUMENT
TMAX_YELLOW,                    //  S_GRAPHIC
TMAX_YELLOW,                    //  S_MOVIE
TMAX_YELLOW,                    //  S_LINKEDIMAGE
TMAX_YELLOW,                    //  S_TEXT
TMAX_YELLOW,                    //  S_POWERPOINT
TMAX_YELLOW,                    //  S_LINKEDPOWER
            
//  TMTB_BLACK
TMAX_BLACK,                     //  S_CLEAR
TMAX_BLACK,                     //  S_PLAYLIST
TMAX_BLACK,                     //  S_DOCUMENT
TMAX_BLACK,                     //  S_GRAPHIC
TMAX_BLACK,                     //  S_MOVIE
TMAX_BLACK,                     //  S_LINKEDIMAGE
TMAX_BLACK,                     //  S_TEXT
TMAX_BLACK,                     //  S_POWERPOINT
TMAX_BLACK,                     //  S_LINKEDPOWER
            
//  TMTB_WHITE
TMAX_WHITE,                     //  S_CLEAR
TMAX_WHITE,                     //  S_PLAYLIST
TMAX_WHITE,                     //  S_DOCUMENT
TMAX_WHITE,                     //  S_GRAPHIC
TMAX_WHITE,                     //  S_MOVIE
TMAX_WHITE,                     //  S_LINKEDIMAGE
TMAX_WHITE,                     //  S_TEXT
TMAX_WHITE,                     //  S_POWERPOINT
TMAX_WHITE,                     //  S_LINKEDPOWER

//  TMTB_PLAYTHROUGH
TMAX_PLAYTHROUGH,               //  S_CLEAR
TMAX_PLAYTHROUGH,               //  S_PLAYLIST
TMAX_PLAYTHROUGH,               //  S_DOCUMENT
TMAX_PLAYTHROUGH,               //  S_GRAPHIC
TMAX_PLAYTHROUGH,               //  S_MOVIE
TMAX_PLAYTHROUGH,               //  S_LINKEDIMAGE
TMAX_PLAYTHROUGH,               //  S_TEXT
TMAX_PLAYTHROUGH,               //  S_POWERPOINT
TMAX_PLAYTHROUGH,               //  S_LINKEDPOWER

//  TMTB_CUEPGLNCURRENT
TMAX_SETPAGELINE,				//  S_CLEAR
TMAX_SETPAGELINE,				//  S_PLAYLIST
TMAX_SETPAGELINE,				//  S_DOCUMENT
TMAX_SETPAGELINE,				//  S_GRAPHIC
TMAX_SETPAGELINE,				//  S_MOVIE
TMAX_SETPAGELINE,				//  S_LINKEDIMAGE
TMAX_SETPAGELINE,				//  S_TEXT
TMAX_SETPAGELINE,               //  S_POWERPOINT
TMAX_SETPAGELINE,               //  S_LINKEDPOWER

//  TMTB_CUEPGLNNEXT
TMAX_SETPAGELINENEXT,			//  S_CLEAR
TMAX_SETPAGELINENEXT,			//  S_PLAYLIST
TMAX_SETPAGELINENEXT,			//  S_DOCUMENT
TMAX_SETPAGELINENEXT,			//  S_GRAPHIC
TMAX_SETPAGELINENEXT,			//  S_MOVIE
TMAX_SETPAGELINENEXT,			//  S_LINKEDIMAGE
TMAX_SETPAGELINENEXT,			//  S_TEXT
TMAX_SETPAGELINENEXT,           //  S_POWERPOINT
TMAX_SETPAGELINENEXT,           //  S_LINKEDPOWER

//  TMTB_DELETEANN
TMAX_DELETEANN,					//  S_CLEAR
TMAX_DELETEANN,					//  S_PLAYLIST
TMAX_DELETEANN,					//  S_DOCUMENT
TMAX_DELETEANN,					//  S_GRAPHIC
TMAX_DELETEANN,					//  S_MOVIE
TMAX_DELETEANN,					//  S_LINKEDIMAGE
TMAX_DELETEANN,					//  S_TEXT
TMAX_DELETEANN,                 //  S_POWERPOINT
TMAX_DELETEANN,                 //  S_LINKEDPOWER

//  TMTB_SELECT
TMAX_SELECT,					//  S_CLEAR
TMAX_SELECT,					//  S_PLAYLIST
TMAX_SELECT,					//  S_DOCUMENT
TMAX_SELECT,					//  S_GRAPHIC
TMAX_SELECT,					//  S_MOVIE
TMAX_SELECT,					//  S_LINKEDIMAGE
TMAX_SELECT,					//  S_TEXT
TMAX_SELECT,                    //  S_POWERPOINT
TMAX_SELECT,                    //  S_LINKEDPOWER

//  TMTB_TEXT
TMAX_TEXT,						//  S_CLEAR
TMAX_TEXT,						//  S_PLAYLIST
TMAX_TEXT,						//  S_DOCUMENT
TMAX_TEXT,						//  S_GRAPHIC
TMAX_TEXT,						//  S_MOVIE
TMAX_TEXT,						//  S_LINKEDIMAGE
TMAX_TEXT,						//  S_TEXT
TMAX_TEXT,                      //  S_POWERPOINT
TMAX_TEXT,                      //  S_LINKEDPOWER

//  TMTB_SELECTTOOL
TMAX_SELECTTOOL,				//  S_CLEAR
TMAX_SELECTTOOL,				//  S_PLAYLIST
TMAX_SELECTTOOL,				//  S_DOCUMENT
TMAX_SELECTTOOL,				//  S_GRAPHIC
TMAX_SELECTTOOL,				//  S_MOVIE
TMAX_SELECTTOOL,				//  S_LINKEDIMAGE
TMAX_SELECTTOOL,				//  S_TEXT
TMAX_SELECTTOOL,                //  S_POWERPOINT
TMAX_SELECTTOOL,                //  S_LINKEDPOWER

//  TMTB_FREEHAND
TMAX_FREEHAND,					//  S_CLEAR
TMAX_FREEHAND,					//  S_PLAYLIST
TMAX_FREEHAND,					//  S_DOCUMENT
TMAX_FREEHAND,					//  S_GRAPHIC
TMAX_FREEHAND,					//  S_MOVIE
TMAX_FREEHAND,					//  S_LINKEDIMAGE
TMAX_FREEHAND,					//  S_TEXT
TMAX_FREEHAND,                  //  S_POWERPOINT
TMAX_FREEHAND,                  //  S_LINKEDPOWER

//  TMTB_LINE
TMAX_LINE,						//  S_CLEAR
TMAX_LINE,						//  S_PLAYLIST
TMAX_LINE,						//  S_DOCUMENT
TMAX_LINE,						//  S_GRAPHIC
TMAX_LINE,						//  S_MOVIE
TMAX_LINE,						//  S_LINKEDIMAGE
TMAX_LINE,						//  S_TEXT
TMAX_LINE,                      //  S_POWERPOINT
TMAX_LINE,                      //  S_LINKEDPOWER

//  TMTB_ARROW
TMAX_ARROW,						//  S_CLEAR
TMAX_ARROW,						//  S_PLAYLIST
TMAX_ARROW,						//  S_DOCUMENT
TMAX_ARROW,						//  S_GRAPHIC
TMAX_ARROW,						//  S_MOVIE
TMAX_ARROW,						//  S_LINKEDIMAGE
TMAX_ARROW,						//  S_TEXT
TMAX_ARROW,                     //  S_POWERPOINT
TMAX_ARROW,                     //  S_LINKEDPOWER

//  TMTB_ELLIPSE
TMAX_ELLIPSE,					//  S_CLEAR
TMAX_ELLIPSE,					//  S_PLAYLIST
TMAX_ELLIPSE,					//  S_DOCUMENT
TMAX_ELLIPSE,					//  S_GRAPHIC
TMAX_ELLIPSE,					//  S_MOVIE
TMAX_ELLIPSE,					//  S_LINKEDIMAGE
TMAX_ELLIPSE,					//  S_TEXT
TMAX_ELLIPSE,                   //  S_POWERPOINT
TMAX_ELLIPSE,                   //  S_LINKEDPOWER

//  TMTB_RECTANGLE
TMAX_RECTANGLE,					//  S_CLEAR
TMAX_RECTANGLE,					//  S_PLAYLIST
TMAX_RECTANGLE,					//  S_DOCUMENT
TMAX_RECTANGLE,					//  S_GRAPHIC
TMAX_RECTANGLE,					//  S_MOVIE
TMAX_RECTANGLE,					//  S_LINKEDIMAGE
TMAX_RECTANGLE,					//  S_TEXT
TMAX_RECTANGLE,                 //  S_POWERPOINT
TMAX_RECTANGLE,                 //  S_LINKEDPOWER

//  TMTB_FILLEDELLIPSE
TMAX_FILLEDELLIPSE,				//  S_CLEAR
TMAX_FILLEDELLIPSE,				//  S_PLAYLIST
TMAX_FILLEDELLIPSE,				//  S_DOCUMENT
TMAX_FILLEDELLIPSE,				//  S_GRAPHIC
TMAX_FILLEDELLIPSE,				//  S_MOVIE
TMAX_FILLEDELLIPSE,				//  S_LINKEDIMAGE
TMAX_FILLEDELLIPSE,				//  S_TEXT
TMAX_FILLEDELLIPSE,             //  S_POWERPOINT
TMAX_FILLEDELLIPSE,             //  S_LINKEDPOWER

//  TMTB_FILLEDRECTANGLE
TMAX_FILLEDRECTANGLE,			//  S_CLEAR
TMAX_FILLEDRECTANGLE,			//  S_PLAYLIST
TMAX_FILLEDRECTANGLE,			//  S_DOCUMENT
TMAX_FILLEDRECTANGLE,			//  S_GRAPHIC
TMAX_FILLEDRECTANGLE,			//  S_MOVIE
TMAX_FILLEDRECTANGLE,			//  S_LINKEDIMAGE
TMAX_FILLEDRECTANGLE,			//  S_TEXT
TMAX_FILLEDRECTANGLE,           //  S_POWERPOINT
TMAX_FILLEDRECTANGLE,           //  S_LINKEDPOWER

//  TMTB_FULLSCREEN
TMAX_FULLSCREEN,				//  S_CLEAR
TMAX_FULLSCREEN,				//  S_PLAYLIST
TMAX_FULLSCREEN,				//  S_DOCUMENT
TMAX_FULLSCREEN,				//  S_GRAPHIC
TMAX_FULLSCREEN,				//  S_MOVIE
TMAX_FULLSCREEN,				//  S_LINKEDIMAGE
TMAX_FULLSCREEN,				//  S_TEXT
TMAX_FULLSCREEN,                //  S_POWERPOINT
TMAX_FULLSCREEN,                //  S_LINKEDPOWER

//  TMTB_STATUSBAR
TMAX_STATUSBAR,					//  S_CLEAR
TMAX_STATUSBAR,					//  S_PLAYLIST
TMAX_STATUSBAR,					//  S_DOCUMENT
TMAX_STATUSBAR,					//  S_GRAPHIC
TMAX_STATUSBAR,					//  S_MOVIE
TMAX_STATUSBAR,					//  S_LINKEDIMAGE
TMAX_STATUSBAR,					//  S_TEXT
TMAX_STATUSBAR,					//  S_POWERPOINT
TMAX_STATUSBAR,                 //  S_LINKEDPOWER

//  TMTB_UNUSED2
TMAX_SHADEONCALLOUT,			//  S_CLEAR
TMAX_SHADEONCALLOUT,			//  S_PLAYLIST
TMAX_SHADEONCALLOUT,			//  S_DOCUMENT
TMAX_SHADEONCALLOUT,			//  S_GRAPHIC
TMAX_SHADEONCALLOUT,			//  S_MOVIE
TMAX_SHADEONCALLOUT,			//  S_LINKEDIMAGE
TMAX_SHADEONCALLOUT,			//  S_TEXT
TMAX_SHADEONCALLOUT,            //  S_POWERPOINT
TMAX_SHADEONCALLOUT,            //  S_LINKEDPOWER

//  TMTB_DARKRED
TMAX_DARKRED,                   //  S_CLEAR
TMAX_DARKRED,                   //  S_PLAYLIST
TMAX_DARKRED,                   //  S_DOCUMENT
TMAX_DARKRED,                   //  S_GRAPHIC
TMAX_DARKRED,                   //  S_MOVIE
TMAX_DARKRED,                   //  S_LINKEDIMAGE
TMAX_DARKRED,                   //  S_TEXT
TMAX_DARKRED,                   //  S_POWERPOINT
TMAX_DARKRED,                   //  S_LINKEDPOWER
            
//  TMTB_DARKGREEN
TMAX_DARKGREEN,                 //  S_CLEAR
TMAX_DARKGREEN,                 //  S_PLAYLIST
TMAX_DARKGREEN,                 //  S_DOCUMENT
TMAX_DARKGREEN,                 //  S_GRAPHIC
TMAX_DARKGREEN,                 //  S_MOVIE
TMAX_DARKGREEN,                 //  S_LINKEDIMAGE
TMAX_DARKGREEN,                 //  S_TEXT
TMAX_DARKGREEN,                 //  S_POWERPOINT
TMAX_DARKGREEN,                 //  S_LINKEDPOWER
            
//  TMTB_DARKBLUE
TMAX_DARKBLUE,                  //  S_CLEAR
TMAX_DARKBLUE,                  //  S_PLAYLIST
TMAX_DARKBLUE,                  //  S_DOCUMENT
TMAX_DARKBLUE,                  //  S_GRAPHIC
TMAX_DARKBLUE,                  //  S_MOVIE
TMAX_DARKBLUE,                  //  S_LINKEDIMAGE
TMAX_DARKBLUE,                  //  S_TEXT
TMAX_DARKBLUE,                  //  S_POWERPOINT
TMAX_DARKBLUE,                  //  S_LINKEDPOWER
            
//  TMTB_LIGHTRED
TMAX_LIGHTRED,                  //  S_CLEAR
TMAX_LIGHTRED,                  //  S_PLAYLIST
TMAX_LIGHTRED,                  //  S_DOCUMENT
TMAX_LIGHTRED,                  //  S_GRAPHIC
TMAX_LIGHTRED,                  //  S_MOVIE
TMAX_LIGHTRED,                  //  S_LINKEDIMAGE
TMAX_LIGHTRED,                  //  S_TEXT
TMAX_LIGHTRED,                  //  S_POWERPOINT
TMAX_LIGHTRED,                  //  S_LINKEDPOWER
            
//  TMTB_LIGHTGREEN
TMAX_LIGHTGREEN,                //  S_CLEAR
TMAX_LIGHTGREEN,                //  S_PLAYLIST
TMAX_LIGHTGREEN,                //  S_DOCUMENT
TMAX_LIGHTGREEN,                //  S_GRAPHIC
TMAX_LIGHTGREEN,                //  S_MOVIE
TMAX_LIGHTGREEN,                //  S_LINKEDIMAGE
TMAX_LIGHTGREEN,                //  S_TEXT
TMAX_LIGHTGREEN,                //  S_POWERPOINT
TMAX_LIGHTGREEN,                //  S_LINKEDPOWER
            
//  TMTB_LIGHTBLUE
TMAX_LIGHTBLUE,                 //  S_CLEAR
TMAX_LIGHTBLUE,                 //  S_PLAYLIST
TMAX_LIGHTBLUE,                 //  S_DOCUMENT
TMAX_LIGHTBLUE,                 //  S_GRAPHIC
TMAX_LIGHTBLUE,                 //  S_MOVIE
TMAX_LIGHTBLUE,                 //  S_LINKEDIMAGE
TMAX_LIGHTBLUE,                 //  S_TEXT
TMAX_LIGHTBLUE,                 //  S_POWERPOINT
TMAX_LIGHTBLUE,                 //  S_LINKEDPOWER

//  TMTB_POLYLINE
TMAX_POLYLINE,					//  S_CLEAR
TMAX_POLYLINE,					//  S_PLAYLIST
TMAX_POLYLINE,					//  S_DOCUMENT
TMAX_POLYLINE,					//  S_GRAPHIC
TMAX_POLYLINE,					//  S_MOVIE
TMAX_POLYLINE,					//  S_LINKEDIMAGE
TMAX_POLYLINE,					//  S_TEXT
TMAX_POLYLINE,					//  S_POWERPOINT
TMAX_POLYLINE,					//  S_LINKEDPOWER

//  TMTB_POLYGON
TMAX_POLYGON,					//  S_CLEAR
TMAX_POLYGON,					//  S_PLAYLIST
TMAX_POLYGON,					//  S_DOCUMENT
TMAX_POLYGON,					//  S_GRAPHIC
TMAX_POLYGON,					//  S_MOVIE
TMAX_POLYGON,					//  S_LINKEDIMAGE
TMAX_POLYGON,					//  S_TEXT
TMAX_POLYGON,					//  S_POWERPOINT
TMAX_POLYGON,					//  S_LINKEDPOWER

//  TMTB_ANNTEXT
TMAX_ANNTEXT,					//  S_CLEAR
TMAX_ANNTEXT,					//  S_PLAYLIST
TMAX_ANNTEXT,					//  S_DOCUMENT
TMAX_ANNTEXT,					//  S_GRAPHIC
TMAX_ANNTEXT,					//  S_MOVIE
TMAX_ANNTEXT,					//  S_LINKEDIMAGE
TMAX_ANNTEXT,					//  S_TEXT
TMAX_ANNTEXT,					//  S_POWERPOINT
TMAX_ANNTEXT,					//  S_LINKEDPOWER

//  TMTB_UPDATEZAP
TMAX_UPDATE_ZAP,				//  S_CLEAR
TMAX_UPDATE_ZAP,				//  S_PLAYLIST
TMAX_UPDATE_ZAP,				//  S_DOCUMENT
TMAX_UPDATE_ZAP,				//  S_GRAPHIC
TMAX_UPDATE_ZAP,				//  S_MOVIE
TMAX_UPDATE_ZAP,				//  S_LINKEDIMAGE
TMAX_UPDATE_ZAP,				//  S_TEXT
TMAX_UPDATE_ZAP,				//  S_POWERPOINT
TMAX_UPDATE_ZAP,				//  S_LINKEDPOWER

//  TMTB_DELETEZAP
TMAX_NOCOMMAND,					//  S_CLEAR
TMAX_NOCOMMAND,					//  S_PLAYLIST
TMAX_NOCOMMAND,					//  S_DOCUMENT
TMAX_NOCOMMAND,					//  S_GRAPHIC
TMAX_NOCOMMAND,					//  S_MOVIE
TMAX_NOCOMMAND,					//  S_LINKEDIMAGE
TMAX_NOCOMMAND,					//  S_TEXT
TMAX_NOCOMMAND,					//  S_POWERPOINT
TMAX_NOCOMMAND,					//  S_LINKEDPOWER

//  TMTB_ZOOMRESTRICTED
TMAX_ZOOMRESTRICTED,			//  S_CLEAR
TMAX_ZOOMRESTRICTED,			//  S_PLAYLIST
TMAX_ZOOMRESTRICTED,			//  S_DOCUMENT
TMAX_ZOOMRESTRICTED,			//  S_GRAPHIC
TMAX_ZOOMRESTRICTED,			//  S_MOVIE
TMAX_ZOOMRESTRICTED,			//  S_LINKEDIMAGE
TMAX_ZOOMRESTRICTED,			//  S_TEXT
TMAX_ZOOMRESTRICTED,			//  S_POWERPOINT
TMAX_ZOOMRESTRICTED,			//  S_LINKEDPOWER

//  TMTB_SAVESPLITZAP
TMAX_SAVE_SPLIT_ZAP,            //  S_CLEAR
TMAX_SAVE_SPLIT_ZAP,            //  S_PLAYLIST
TMAX_SAVE_SPLIT_ZAP,			//  S_DOCUMENT
TMAX_SAVE_SPLIT_ZAP,            //  S_GRAPHIC
TMAX_SAVE_SPLIT_ZAP,            //  S_MOVIE
TMAX_SAVE_SPLIT_ZAP,            //  S_LINKEDIMAGE
TMAX_SAVE_SPLIT_ZAP,            //  S_TEXT
TMAX_SAVE_SPLIT_ZAP,            //  S_POWERPOINT
TMAX_SAVE_SPLIT_ZAP,            //  S_LINKEDPOWER
            
};
            

