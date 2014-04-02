//==============================================================================
//
// File Name:	tmstdefs.h
//
// Description:	This file contains defines used by the tm_stat.ocx control. This
//				file is required to programmatically manage one of these 
//				controls.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	02-13-99	1.00		Original Release
//==============================================================================
#if !defined(__TMSTDEFS_H__)
#define __TMSTDEFS_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Extents
#define TMSTAT_MAXLEN_MEDIA_ID		256
#define TMSTAT_MAXLEN_LINK_ID		256

//	Display modes
#define TMSTAT_TEXTMODE				0
#define TMSTAT_PLAYLISTMODE			1

//	Default property values
#define TMSTAT_AUTOINIT				TRUE
#define TMSTAT_AUTOSIZEFONT			TRUE
#define TMSTAT_TOPMARGIN			2
#define TMSTAT_BOTTOMMARGIN			2
#define TMSTAT_LEFTMARGIN			2
#define TMSTAT_RIGHTMARGIN			2
#define TMSTAT_MODE					TMSTAT_TEXTMODE
#define TMSTAT_PLAYLISTTIME			0
#define TMSTAT_DESIGNATIONTIME		0
#define TMSTAT_DESIGNATIONCOUNT		0
#define TMSTAT_DESIGNATIONINDEX		0
#define TMSTAT_ELAPSEDPLAYLIST		0
#define TMSTAT_ELAPSEDDESIGNATION	0
#define TMSTAT_TEXTPAGE				0
#define TMSTAT_TEXTLINE				0

//	Error levels
#define TMSTAT_NOERROR				0
#define TMSTAT_INVALIDARG			1

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	This structure is used in calls to the SetPlaylistInfo() method
typedef struct
{
	double	dPlaylistTime;
	double	dElapsedPlaylist;
	double	dDesignationTime;
	double	dElapsedDesignation;
	long	lDesignationCount;
	long	lDesignationOrder;
	long	lTextPage;
	long	lTextLine;
	BOOL	bShowPageLine;
	BOOL	bShowPlaylist;
	BOOL	bShowLink;
	char	szMediaId[TMSTAT_MAXLEN_MEDIA_ID];
	char	szLinkId[TMSTAT_MAXLEN_LINK_ID];
}SPlaylistStatus;
	


#endif // !defined(__TMSTDEFS_H__)