//==============================================================================
//
// File Name:	tmxmdefs.h
//
// Description:	This file contains defines used by the tm_xml.ocx control. This
//				file is required to programmatically manage one of these 
//				controls.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	03-02-01	1.00		Original Release
//==============================================================================
#if !defined(__TMXMDEFS_H__)
#define __TMXMDEFS_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Connection type identifiers
#define TMXML_CONNECTION_DEFAULT		0
#define TMXML_CONNECTION_ASSIGNED		1
#define TMXML_CONNECTION_PROXY			2

//	Default property values
#define TMXML_AUTOINIT					TRUE
#define TMXML_ENABLEERRORS				TRUE
#define TMXML_FILENAME					""
#define TMXML_FLOAT_PRINT_PROGRESS		FALSE

//	Default option values
#define TMXML_PROGRESS_DELAY			2.0f
#define TMXML_DRAW_THICKNESS			1
#define TMXML_MAX_ZOOM					5
#define TMXML_BITONAL					2
#define TMXML_ZOOM_RECT					FALSE
#define TMXML_ANN_FONT_NAME				"Arial"
#define TMXML_ANN_FONT_SIZE				12
#define TMXML_ANN_FONT_BOLD				FALSE
#define TMXML_ANN_FONT_UNDERLINE		FALSE
#define TMXML_ANN_FONT_STRIKETHROUGH	FALSE
#define TMXML_PRINTER					""
#define TMXML_PRINT_TEMPLATE			""
#define TMXML_CONFIRM_BATCH				FALSE	
#define TMXML_PRINT_COPIES				1
#define TMXML_PRINT_COLLATE				FALSE
#define TMXML_DIAGNOSTICS				TRUE
#define TMXML_CONNECTION				TMXML_CONNECTION_DEFAULT
#define TMXML_INTERNET_PORT				0
#define TMXML_PROXY_SERVER				""
#define TMXML_PROXY_PORT				0
#define TMXML_MINIMIZE_COLOR_DEPTH		TRUE
#define TMXML_COMBINE_PRINT_PAGES		FALSE	
#define TMXML_SHOW_PAGE_NAVIGATION		TRUE			

//	Error levels
#define TMXML_NOERROR					0
#define TMXML_CREATEFRAMEFAILED			1
#define TMXML_FILENOTFOUND				2
#define TMXML_LOADFILEFAILED			3
#define TMXML_NOTINITIALIZED			4
#define TMXML_ATTACHFAILED				5
#define TMXML_CLSIDFAILED				6
#define TMXML_JUMPPAGEFAILED			7
#define TMXML_FINDPAGEFAILED			8

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------



#endif // !defined(__TMXMDEFS_H__)