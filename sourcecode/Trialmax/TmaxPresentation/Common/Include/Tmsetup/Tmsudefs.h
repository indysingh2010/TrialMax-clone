//==============================================================================
//
// File Name:	tmsudefs.h
//
// Description:	This file contains defines used by the tm_setup.ocx control. This
//				file is required to programmatically manage one of these 
//				controls.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-27-00	1.00		Original Release
//==============================================================================
#if !defined(__TMSUDEFS_H__)
#define __TMSUDEFS_H__

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
#define TMSETUP_AUTOINIT				TRUE
#define TMSETUP_INIFILE					"Fti.ini"
#define TMSETUP_INISECTION				"TMSETUP"
#define TMSETUP_ENABLEERRORS			TRUE
#define TMSETUP_ABOUTPAGE				TRUE
#define TMSETUP_DATABASEPAGE			TRUE
#define TMSETUP_DIAGNOSTICPAGE			TRUE
#define TMSETUP_DIRECTXPAGE				TRUE
#define TMSETUP_GRAPHICSPAGE			TRUE
#define TMSETUP_SYSTEMPAGE				TRUE
#define TMSETUP_TEXTPAGE				TRUE
#define TMSETUP_VIDEOPAGE				TRUE
#define TMSETUP_CAPTUREPAGE				TRUE
#define TMSETUP_RINGTAILPAGE			TRUE
#define TMSETUP_ABOUTNAME				"TrialMax"
#define TMSETUP_ABOUTVERSION			"7.0"
#define TMSETUP_ABOUTCOPYRIGHT			"Copyright (C) FTI Consulting 1996-2012"
#define TMSETUP_ABOUTPHONE				""
#define TMSETUP_ABOUTEMAIL				"Trialmax@FTIConsulting.com"
#define TMSETUP_ENABLEAXERRORS			FALSE

//	Error levels
#define TMSETUP_NOERROR					0
#define TMSETUP_NOTINITIALIZED			1
#define TMSETUP_CREATETABFAILED			2
#define TMSETUP_INVALIDSETUPDATA		3

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------



#endif // !defined(__TMSUDEFS_H__)