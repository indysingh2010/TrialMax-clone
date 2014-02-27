//==============================================================================
//
// File Name:	tmtbdefs.h
//
// Description:	This file contains defines used by the tm_tool.ocx control. This
//				file is required to programmatically manage one of these 
//				controls.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	04-21-98	1.00		Original Release
//==============================================================================
#if !defined(__TMTBDEFS_H__)
#define __TMTBDEFS_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	TrialMax toolbar styles
#define TMTB_FLAT						0
#define TMTB_RAISED						1

//	TrialMax toolbar button sizes
#define TMTB_SMALLBUTTONS				0
#define TMTB_MEDIUMBUTTONS				1
#define TMTB_LARGEBUTTONS				2

//	TrialMax toolbar orientations	
#define TMTB_BOTTOM						0
#define TMTB_TOP						1
#define TMTB_LEFT						2
#define TMTB_RIGHT						3

//	These constants are used to identify the toolbar buttons
#define TMTB_CONFIG						0
#define TMTB_UNUSED1					1
#define TMTB_CONFIGTOOLBARS				2
#define TMTB_CLEAR						3
#define TMTB_ROTATECW					4
#define TMTB_ROTATECCW					5
#define TMTB_NORMAL						6
#define	TMTB_ZOOM						7
#define TMTB_ZOOMWIDTH					8
#define TMTB_PAN						9
#define TMTB_CALLOUT					10
#define TMTB_DRAWTOOL					11
#define TMTB_HIGHLIGHT					12
#define TMTB_REDACT						13
#define TMTB_ERASE						14
#define TMTB_FIRSTPAGE					15
#define TMTB_PREVPAGE					16
#define TMTB_NEXTPAGE					17
#define TMTB_LASTPAGE					18
#define TMTB_SAVEZAP					19
#define TMTB_FIRSTZAP					20
#define TMTB_PREVZAP					21
#define TMTB_NEXTZAP					22
#define TMTB_LASTZAP					23
#define TMTB_STARTMOVIE					24
#define TMTB_BACKMOVIE					25
#define TMTB_PAUSEMOVIE					26
#define TMTB_PLAYMOVIE					27
#define TMTB_FWDMOVIE					28
#define TMTB_ENDMOVIE					29
#define TMTB_FIRSTDESIGNATION			30
#define TMTB_BACKDESIGNATION			31
#define TMTB_PREVDESIGNATION			32
#define TMTB_STARTDESIGNATION			33
#define TMTB_PAUSEDESIGNATION			34
#define TMTB_PLAYDESIGNATION			35
#define TMTB_NEXTDESIGNATION			36
#define TMTB_FWDDESIGNATION				37
#define TMTB_LASTDESIGNATION			38
#define TMTB_PRINT						39
#define TMTB_SPLITVERTICAL				40
#define TMTB_SPLITHORIZONTAL			41
#define TMTB_DISABLELINKS				42
#define TMTB_ENABLELINKS				43
#define TMTB_EXIT						44
#define TMTB_RED						45
#define TMTB_GREEN						46
#define TMTB_BLUE						47
#define TMTB_YELLOW						48
#define TMTB_BLACK						49
#define TMTB_WHITE						50
#define TMTB_PLAYTHROUGH				51
#define TMTB_CUEPGLNCURRENT				52
#define TMTB_CUEPGLNNEXT				53
#define TMTB_DELETEANN					54
#define TMTB_SELECT						55
#define TMTB_TEXT						56
#define TMTB_SELECTTOOL					57
#define TMTB_FREEHAND					58
#define TMTB_LINE						59
#define TMTB_ARROW						60
#define TMTB_ELLIPSE					61
#define TMTB_RECTANGLE					62
#define TMTB_FILLEDELLIPSE				63
#define TMTB_FILLEDRECTANGLE			64
#define TMTB_FULLSCREEN					65
#define TMTB_STATUSBAR					66
#define TMTB_SHADEDCALLOUTS				67
#define TMTB_DARKRED					68
#define TMTB_DARKGREEN					69
#define TMTB_DARKBLUE					70
#define TMTB_LIGHTRED					71
#define TMTB_LIGHTGREEN					72
#define TMTB_LIGHTBLUE					73
#define TMTB_POLYLINE					74
#define TMTB_POLYGON					75
#define TMTB_ANNTEXT					76
#define TMTB_UPDATEZAP					77
#define TMTB_DELETEZAP					78
#define TMTB_ZOOMRESTRICTED				79
#define TMTB_SAVESPLITZAP				80
#define TMTB_MAXBUTTONS					81

//	These constants are used to map a button's identifier to its index in the
//	image strip. They should be enumerated in the same order in which the
//	images appear in the image strip
#define TMTB_IMAGE_CONFIG				0
#define TMTB_IMAGE_UNUSED1				1
#define TMTB_IMAGE_CONFIGTOOLBARS		2
#define TMTB_IMAGE_CLEAR				3
#define TMTB_IMAGE_ROTATECW				4
#define TMTB_IMAGE_ROTATECCW			5
#define TMTB_IMAGE_NORMAL				6
#define	TMTB_IMAGE_ZOOM					7
#define TMTB_IMAGE_ZOOMWIDTH			8
#define TMTB_IMAGE_PAN					9
#define TMTB_IMAGE_CALLOUT				10
#define TMTB_IMAGE_DRAWTOOL				11
#define TMTB_IMAGE_SELECTTOOL			12
#define TMTB_IMAGE_HIGHLIGHT			13
#define TMTB_IMAGE_REDACT				14
#define TMTB_IMAGE_ERASE				15
#define TMTB_IMAGE_DELETEANN			16
#define TMTB_IMAGE_SELECT				17
#define TMTB_IMAGE_FIRSTPAGE			18
#define TMTB_IMAGE_PREVPAGE				19
#define TMTB_IMAGE_NEXTPAGE				20
#define TMTB_IMAGE_LASTPAGE				21
#define TMTB_IMAGE_SAVEZAP				22
#define TMTB_IMAGE_UPDATEZAP			23
#define TMTB_IMAGE_DELETEZAP			24
#define TMTB_IMAGE_FIRSTZAP				25
#define TMTB_IMAGE_PREVZAP				26
#define TMTB_IMAGE_NEXTZAP				27
#define TMTB_IMAGE_LASTZAP				28
#define TMTB_IMAGE_STARTMOVIE			29
#define TMTB_IMAGE_BACKMOVIE			30
#define TMTB_IMAGE_PAUSEMOVIE			31
#define TMTB_IMAGE_PLAYMOVIE			32
#define TMTB_IMAGE_FWDMOVIE				33
#define TMTB_IMAGE_ENDMOVIE				34
#define TMTB_IMAGE_FIRSTDESIGNATION		35
#define TMTB_IMAGE_BACKDESIGNATION		36
#define TMTB_IMAGE_PREVDESIGNATION		37
#define TMTB_IMAGE_STARTDESIGNATION		38
#define TMTB_IMAGE_PAUSEDESIGNATION		39
#define TMTB_IMAGE_PLAYDESIGNATION		40
#define TMTB_IMAGE_NEXTDESIGNATION		41
#define TMTB_IMAGE_FWDDESIGNATION		42
#define TMTB_IMAGE_LASTDESIGNATION		43
#define TMTB_IMAGE_PLAYTHROUGH			44
#define TMTB_IMAGE_CUEPGLNNEXT			45
#define TMTB_IMAGE_CUEPGLNCURRENT		46
#define TMTB_IMAGE_PRINT				47
#define TMTB_IMAGE_SPLITVERTICAL		48
#define TMTB_IMAGE_SPLITHORIZONTAL		49
#define TMTB_IMAGE_DISABLELINKS			50
#define TMTB_IMAGE_ENABLELINKS			51
#define TMTB_IMAGE_STATUSBAR			52
#define TMTB_IMAGE_TEXT					53
#define TMTB_IMAGE_FULLSCREEN			54
#define TMTB_IMAGE_SHADEDCALLOUTS		55
#define TMTB_IMAGE_EXIT					56
#define TMTB_IMAGE_DARKRED				57
#define TMTB_IMAGE_RED					58
#define TMTB_IMAGE_LIGHTRED				59
#define TMTB_IMAGE_DARKGREEN			60
#define TMTB_IMAGE_GREEN				61
#define TMTB_IMAGE_LIGHTGREEN			62
#define TMTB_IMAGE_DARKBLUE				63
#define TMTB_IMAGE_BLUE					64
#define TMTB_IMAGE_LIGHTBLUE			65
#define TMTB_IMAGE_YELLOW				66
#define TMTB_IMAGE_BLACK				67
#define TMTB_IMAGE_WHITE				68
#define TMTB_IMAGE_ANNTEXT				69		
#define TMTB_IMAGE_FREEHAND				70
#define TMTB_IMAGE_LINE					71
#define TMTB_IMAGE_ARROW				72
#define TMTB_IMAGE_ELLIPSE				73
#define TMTB_IMAGE_RECTANGLE			74
#define TMTB_IMAGE_POLYLINE				75
#define TMTB_IMAGE_FILLEDELLIPSE		76
#define TMTB_IMAGE_FILLEDRECTANGLE		77
#define TMTB_IMAGE_POLYGON				78
#define TMTB_IMAGE_ZOOMRESTRICTED		79	
#define TMTB_IMAGE_SAVESPLITZAP			80	
#define TMTB_MAXIMAGES					81

//	Default property values
#define DEFAULT_TBINIFILENAME			""
#define DEFAULT_TBINISECTION			""
#define DEFAULT_TBSTYLE					TMTB_RAISED
#define DEFAULT_TBBUTTONSIZE			TMTB_SMALLBUTTONS
#define DEFAULT_TBORIENTATION			TMTB_BOTTOM
#define DEFAULT_TBSTRETCH				TRUE
#define DEFAULT_TBAUTOINIT				TRUE
#define DEFAULT_TBENABLED				TRUE
#define DEFAULT_TBTOOLTIPS				TRUE
#define DEFAULT_TBMASK					"11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111"
#define DEFAULT_TBCONFIGURABLE			TRUE
#define DEFAULT_TBBUTTONROWS			1
#define DEFAULT_TBAUTORESET				TRUE
#define DEFAULT_TBUSESYSTEMBACKGROUND	FALSE

//	Error levels
#define TMTB_NOERROR					0
#define TMTB_CREATEBARFAILED			1
#define TMTB_BUTTONNOTFOUND				2
#define TMTB_NOTINITIALIZED				3
#define TMTB_INVALIDBUTTONID			4
#define TMTB_INVALIDIMAGEINDEX			5
#define TMTB_NOFILE						6

#endif // !defined(__TMTBDEFS_H__)