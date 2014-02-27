//==============================================================================
//
// File Name:	plugindef.h
//
// Description:	This file contains defines used by the tm_browse.ocx control. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	02-10-02	1.00		Original Release
//==============================================================================
#if !defined(__TMBROWSEDEF_H__)
#define __TMBROWSEDEF_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Error levels
#define TMBROWSE_NOERROR					0
#define TMBROWSE_FRAMEFAILED				(TMBROWSE_NOERROR + 1)
#define TMBROWSE_FILENOTFOUND				(TMBROWSE_NOERROR + 2)
#define TMBROWSE_BROWSERFAILED				(TMBROWSE_NOERROR + 3)

//	Default property values
#define TMBROWSE_DEFAULT_FILENAME			""
#define TMBROWSE_DEFAULT_INI_FILE			""
#define TMBROWSE_DEFAULT_ENABLE_ERRORS		TRUE
#define TMBROWSE_DEFAULT_AUTO_INIT			TRUE

#endif