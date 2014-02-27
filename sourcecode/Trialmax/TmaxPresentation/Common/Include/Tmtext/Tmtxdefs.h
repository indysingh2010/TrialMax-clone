//==============================================================================
//
// File Name:	tmtxdefs.h
//
// Description:	This file contains defines used by the tm_text.ocx control. This
//				file is required to programmatically manage one of these 
//				controls.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	01-02-99	1.00		Original Release
//==============================================================================
#if !defined(__TMTXDEFS_H__)
#define __TMTXDEFS_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Bullet style identifiers
#define TMTEXT_BULLET_NONE			0
#define TMTEXT_BULLET_SQUARE		1
#define TMTEXT_BULLET_CIRCLE		2
#define TMTEXT_BULLET_DIAMOND		3

//	Default property values
#define TMTEXT_AUTOINIT				TRUE
#define TMTEXT_COMBINEDESIGNATIONS	FALSE
#define TMTEXT_SHOWPAGELINE			TRUE
#define TMTEXT_DISPLAYLINES			3
#define TMTEXT_HIGHLINES			1
#define TMTEXT_HIGHCOLOR			((OLE_COLOR)RGB(0,0,255))
#define TMTEXT_HIGHTEXTCOLOR		((OLE_COLOR)RGB(255,255,255))
#define TMTEXT_TOPMARGIN			5
#define TMTEXT_BOTTOMMARGIN			5
#define TMTEXT_LEFTMARGIN			5
#define TMTEXT_RIGHTMARGIN			5
#define TMTEXT_MAXCHARSPERLINE		30
#define TMTEXT_USEAVGCHARWIDTH		TRUE
#define TMTEXT_RESIZEONCHANGE		TRUE
#define TMTEXT_SMOOTHSCROLL			TRUE
#define TMTEXT_SCROLLSTEPS			10
#define TMTEXT_SCROLLTIME			200
#define TMTEXT_USELINECOLOR			FALSE
#define TMTEXT_SHOWTEXT				TRUE
#define TMTEXT_BULLETSTYLE			TMTEXT_BULLET_NONE
#define TMTEXT_BULLETMARGIN			5

//	Error levels
#define TMTEXT_NOERROR				0
#define TMTEXT_CREATEFONTFAILED		1
#define TMTEXT_NOPLAYLIST			2
#define TMTEXT_NOLINE				3
#define TMTEXT_LINENOTFOUND			4
#define TMTEXT_INVALIDLOGFONT		5
#define TMTEXT_SETTIMERFAILED		6

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------



#endif // !defined(__TMTXDEFS_H__)