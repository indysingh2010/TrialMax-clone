//==============================================================================
//
// File Name:	grabdefs.h
//
// Description:	This file contains defines used by the tm_grab.ocx control. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	05-24-01	1.00		Original Release
//==============================================================================
#if !defined(__GRABDEFS_H__)
#define __GRABDEFS_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <winuser.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Area property identifiers
#define TMGRAB_AREA_FULL_SCREEN				0
#define TMGRAB_AREA_ACTIVE_WINDOW			1
#define TMGRAB_AREA_SELECTION				2

//	Error levels
#define TMGRAB_ERROR_NONE					0
#define TMGRAB_ERROR_FILE_NOT_FOUND			(TMGRAB_ERROR_NONE + 1)
#define TMGRAB_ERROR_INITIALIZE_FAILED		(TMGRAB_ERROR_NONE + 2)
#define TMGRAB_ERROR_NOT_INITIALIZED		(TMGRAB_ERROR_NONE + 3)
#define TMGRAB_ERROR_CAPTURE_FAILED			(TMGRAB_ERROR_NONE + 4)
#define TMGRAB_ERROR_STOP_FAILED			(TMGRAB_ERROR_NONE + 5)
#define TMGRAB_ERROR_SAVE_FAILED			(TMGRAB_ERROR_NONE + 6)

//	Default property values
#define TMGRAB_DEFAULT_INI_FILE				""
#define TMGRAB_DEFAULT_ENABLE_ERRORS		TRUE
#define TMGRAB_DEFAULT_AREA					TMGRAB_AREA_FULL_SCREEN
#define TMGRAB_DEFAULT_HOTKEY				0
#define TMGRAB_DEFAULT_SILENT				TRUE
#define TMGRAB_DEFAULT_CANCELKEY			VK_ESCAPE
#define TMGRAB_DEFAULT_ONESHOT				FALSE

#endif