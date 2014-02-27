//==============================================================================
//
// File Name:	tmbadefs.h
//
// Description:	This file contains defines used by the tm_bars.ocx control. This
//				file is required to programmatically manage one of these 
//				controls.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-09-00	1.00		Original Release
//==============================================================================
#if !defined(__TMBARS_H__)
#define __TMBARS_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Default property values
#define TMBARS_AUTOINIT					TRUE
#define TMBARS_INIFILE					"Fti.ini"
#define TMBARS_INISECTION				"TMBARS"

//	Error levels
#define TMBARS_NOERROR					0
#define TMBARS_ININOTFOUND				1
#define TMBARS_NOTINITIALIZED			2
#define TMBARS_CREATETABFAILED			3

//	Toolbar configuration page ini sections
#define TMBARS_DOCUMENT_SECTION			"DOCUMENT TOOLBAR"
#define TMBARS_GRAPHIC_SECTION			"GRAPHIC TOOLBAR"
#define TMBARS_PLAYLIST_SECTION			"PLAYLIST TOOLBAR"
#define TMBARS_LINK_SECTION				"LINK TOOLBAR"
#define TMBARS_MOVIE_SECTION			"MOVIE TOOLBAR"
#define TMBARS_POWERPOINT_SECTION		"POWERPOINT TOOLBAR"
#define TMBARS_TEMPLATES_SECTION		"TOOLBAR TEMPLATES"

//	Default toolbar settings			
#define DEFAULT_TMBARS_SHOW				FALSE
#define DEFAULT_TMBARS_DOCK				FALSE
#define DEFAULT_TMBARS_FLAT				TRUE
#define DEFAULT_TMBARS_STRETCH			TRUE
#define DEFAULT_TMBARS_SIZE				0
#define DEFAULT_TMBARS_USE_MASTER		FALSE

//	Toolbar lines
#define TMBARS_SHOW_LINE				"Show"
#define TMBARS_DOCK_LINE				"Dock"
#define TMBARS_FLAT_LINE				"Flat"
#define TMBARS_STRETCH_LINE				"Stretch"
#define TMBARS_SIZE_LINE				"Size"
#define TMBARS_USE_MASTER_LINE			"UseMaster"

#define TMTB_INI_SIZE_LINE				"Size"

//	TrialMax toolbar button sizes
#define TMTB_SMALLBUTTONS				0
#define TMTB_MEDIUMBUTTONS				1
#define TMTB_LARGEBUTTONS				2


//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------



#endif // !defined(__TMBARS_H__)