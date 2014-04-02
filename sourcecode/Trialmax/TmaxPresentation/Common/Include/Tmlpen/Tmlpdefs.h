//==============================================================================
//
// File Name:	tmlpdefs.h
//
// Description:	This file contains defines used by the tm_lpen.ocx control. This
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
#if !defined(__TMLPDEFS_H__)
#define __TMLPDEFS_H__

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
#define TMLPEN_AUTOINIT			TRUE
#define TMLPEN_ALWAYSONTOP		TRUE

//	Mouse button identifiers	
#define TMLPEN_LEFT				0
#define TMLPEN_RIGHT			1

//	Key state identifiers
#define TMLPEN_NOKEYS			0
#define TMLPEN_ALT				1
#define TMLPEN_SHIFT			2
#define TMLPEN_CONTROL			4
#define TMLPEN_ALTSHIFT			(TMLPEN_ALT | TMLPEN_SHIFT)
#define TMLPEN_ALTCONTROL		(TMLPEN_ALT | TMLPEN_CONTROL)
#define TMLPEN_ALTSHIFTCONTROL	(TMLPEN_ALT | TMLPEN_CONTROL | TMLPEN_SHIFT)
#define TMLPEN_SHIFTCONTROL		(TMLPEN_SHIFT | TMLPEN_CONTROL)

//	Error levels
#define TMLP_NOERROR				0

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------



#endif // !defined(__TMLPDEFS_H__)