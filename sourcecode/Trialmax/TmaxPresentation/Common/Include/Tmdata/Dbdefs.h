//==============================================================================
//
// File Name:	dbdefs.h
//
// Description:	This file contains decalarations and definitions related to 
//				TrialMax database management.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	06-28-97	1.00		Original Release
//==============================================================================
#if !defined(__DBDEFS_H__)
#define __DBDEFS_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <tmstring.h>	//	Error message string identifiers


//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Database password
#define TRIALMAX_PASSWORD				"wthatggtton1"	//	What the hell are these guys going to think of next

#define DEFAULT_DB45_FILENAME			"case.mdb"
#define DEFAULT_DBNET_FILENAME			"_tmax_case.mdb"
#define DEFAULT_DBNET_XML_FILENAME		"_tmax_case.xml"

#define DB45_FRAMES_PER_SECOND			29.97

//	Media Types
#define MEDIA_TYPE_IMAGE				1
#define MEDIA_TYPE_PLAYLIST				2
#define MEDIA_TYPE_RECORDING			3
#define MEDIA_TYPE_POWERPOINT			4
#define MEDIA_TYPE_CUSTOMSHOW			5
#define MEDIA_TYPE_DEPOSITION			6

//	Display Types	
#define DISPLAY_TYPE_HIRESPAGE			1
#define DISPLAY_TYPE_SCREENRESPAGE		2
#define DISPLAY_TYPE_BASICDESIGNATION	3
#define DISPLAY_TYPE_BASICLINK			4
#define DISPLAY_TYPE_BASICANIMATION		5
#define DISPLAY_TYPE_BASICPOWERPOINT	6

//	These flags are used to manage linked documents and graphics
//
//	NOTE:	These values MUST match the enumeration used by TmaxManager
#define TMFLAG_LINK_SPLITSCREEN			0x0001L
#define TMFLAG_LINK_HIDE				0x0002L
#define TMFLAG_LINK_HIDE_VIDEO			0x0004L
#define TMFLAG_LINK_HIDE_TEXT			0x0008L

//	These flags are used to manage custom show items
#define TMFLAG_SHOWITEM_LINK			0x0001L

//	These flags are used to create split screen treatments
#define TMFLAG_SPLIT_ZAP_HORIZONTAL		0x0001

#define DELETE_INTERFACE(x) { if (x) delete x; x = 0; }
#define RELEASE_INTERFACE(x) { if (x) x->Release(); x = 0; }

//	TrialMax database error levels
#define TMDB_NOERROR					0
#define TMDB_FILENOTFOUND				1
#define TMDB_OPENDBFAILED				2
#define TMDB_OPENRSFAILED				3
#define TMDB_MEMERROR					4
#define TMDB_INVALIDRECORDSET			5
#define TMDB_DBNOTOPEN					6
#define TMDB_NOVERSIONINFO				7
#define TMDB_INVALIDPARAM				8
#define TMDB_INVALIDVERSION				9
#define TMDB_VERSIONMATCH				10
#define TMDB_ALIASNOTFOUND				11
#define TMDB_NOALIASNODE				12
#define TMDB_NOCASEOPTIONS				13
#define TMDB_CREATECASEOPTIONS			14
#define TMDB_LOADCASEOPTIONS			15
#define TMDB_NOCASEPATHNODE				16
#define TMDB_BADCASEPATHNODE			17
#define TMDB_NOMACHINENAME				18
#define TMDB_NOMACHINENODE				19
#define TMDB_BADMACHINENODE				20
#define TMDB_NOPATHMAPNODE				21
#define TMDB_NOTRANSCRIPTRECORD			22
#define TMDB_NOSEGMENTS					23
#define TMDB_NOSEGMENTVIDEO				24

//	.NET enumerations
typedef enum
{
	NET_MEDIA_TYPE_UNKNOWN = 0,
	NET_MEDIA_TYPE_DOCUMENT,
	NET_MEDIA_TYPE_POWERPOINT,
	NET_MEDIA_TYPE_RECORDING,
	NET_MEDIA_TYPE_SCRIPT,
	NET_MEDIA_TYPE_PAGE,
	NET_MEDIA_TYPE_SEGMENT,
	NET_MEDIA_TYPE_SLIDE,
	NET_MEDIA_TYPE_SCENE,
	NET_MEDIA_TYPE_TREATMENT,
	NET_MEDIA_TYPE_CLIP,
	NET_MEDIA_TYPE_DEPOSITION,
	NET_MEDIA_TYPE_DESIGNATION,
	NET_MEDIA_TYPE_LINK,

}NetMediaTypes;

typedef enum
{
	NET_PRIMARY_MERGED = 1,
	NET_PRIMARY_SPLIT = 2,
	NET_PRIMARY_UNUSED = 4,
	NET_PRIMARY_PLAYLIST = 8,

}NetPrimaryAttributes;

typedef enum
{
	NET_SECONDARY_HIGHRES = 1,
	NET_SECONDARY_HIDDEN = 2,
	NET_SECONDARY_AUTO_TRANSITION = 4,

}NetSecondaryAttributes;

typedef enum
{
	NET_TERTIARY_HAS_SHORTCUTS = 1,
	NET_TERTIARY_SCROLL_TEXT = 2,
	NET_TERTIARY_SPLIT_SCREEN = 4,
	NET_TERTIARY_SPLIT_HORIZONTAL = 8,
	NET_TERTIARY_SPLIT_RIGHT = 16,

}NetTertiaryAttributes;

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

	
#endif // !defined(__DBDEFS_H__)
