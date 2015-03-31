//==============================================================================
//
// File Name:	sharedef.h
//
// Description:	This file contains defines used by the tm_share.ocx control. 
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	04-06-01	1.00		Original Release
//==============================================================================
#if !defined(__SHAREDEF_H__)
#define __SHAREDEF_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Owner identifiers
#define TMSHARE_MANAGER										0
#define TMSHARE_PRESENTATION								1

//	Application filenames
#define TMSHARE_MANAGER_FILENAME							"trialmax.exe"
#define TMSHARE_PRESENTATION_FILENAME						"tmaxPresentation.exe"

//	Command identifiers
#define TMSHARE_COMMAND_NONE								0
#define TMSHARE_COMMAND_LOAD								1
#define TMSHARE_COMMAND_ADD_TREATMENT						2
#define TMSHARE_COMMAND_ADD_TO_BINDER						3
#define TMSHARE_COMMAND_UPDATE_TREATMENT					4
#define TMSHARE_COMMAND_UPDATE_NUDGE						5

//	Error levels
#define TMSHARE_ERROR_NONE									0
#define TMSHARE_ERROR_NOT_INITIALIZED						(TMSHARE_ERROR_NONE + 1)
#define TMSHARE_ERROR_OPEN_MANAGER_REQUEST_FAILED			(TMSHARE_ERROR_NONE + 2)
#define TMSHARE_ERROR_OPEN_MANAGER_RESPONSE_FAILED			(TMSHARE_ERROR_NONE + 3)
#define TMSHARE_ERROR_OPEN_PRESENTATION_REQUEST_FAILED		(TMSHARE_ERROR_NONE + 4)
#define TMSHARE_ERROR_OPEN_PRESENTATION_RESPONSE_FAILED		(TMSHARE_ERROR_NONE + 5)
#define TMSHARE_ERROR_COMMAND_REQUEST_TIMER_FAILED			(TMSHARE_ERROR_NONE + 6)
#define TMSHARE_ERROR_READ_MANAGER_REQUEST_FAILED			(TMSHARE_ERROR_NONE + 7)
#define TMSHARE_ERROR_READ_MANAGER_RESPONSE_FAILED			(TMSHARE_ERROR_NONE + 8)
#define TMSHARE_ERROR_READ_PRESENTATION_REQUEST_FAILED		(TMSHARE_ERROR_NONE + 9)
#define TMSHARE_ERROR_READ_PRESENTATION_RESPONSE_FAILED		(TMSHARE_ERROR_NONE + 10)
#define TMSHARE_ERROR_WRITE_MANAGER_REQUEST_FAILED			(TMSHARE_ERROR_NONE + 11)
#define TMSHARE_ERROR_WRITE_MANAGER_RESPONSE_FAILED			(TMSHARE_ERROR_NONE + 12)
#define TMSHARE_ERROR_WRITE_PRESENTATION_REQUEST_FAILED		(TMSHARE_ERROR_NONE + 13)
#define TMSHARE_ERROR_WRITE_PRESENTATION_RESPONSE_FAILED	(TMSHARE_ERROR_NONE + 14)
#define TMSHARE_ERROR_NO_RESPONSE							(TMSHARE_ERROR_NONE + 15)
#define TMSHARE_ERROR_REQUEST_TIMED_OUT						(TMSHARE_ERROR_NONE + 16)
#define TMSHARE_ERROR_OPEN_APP_FAILED						(TMSHARE_ERROR_NONE + 17)
#define TMSHARE_ERROR_OPEN_MANAGER_STATUS_FAILED			(TMSHARE_ERROR_NONE + 18)
#define TMSHARE_ERROR_OPEN_PRESENTATION_STATUS_FAILED		(TMSHARE_ERROR_NONE + 19)
#define TMSHARE_ERROR_WRITE_PRESENTATION_STATUS_FAILED		(TMSHARE_ERROR_NONE + 20)
#define TMSHARE_ERROR_WRITE_MANAGER_STATUS_FAILED			(TMSHARE_ERROR_NONE + 21)
#define TMSHARE_ERROR_READ_PRESENTATION_STATUS_FAILED		(TMSHARE_ERROR_NONE + 22)
#define TMSHARE_ERROR_READ_MANAGER_STATUS_FAILED			(TMSHARE_ERROR_NONE + 23)
#define TMSHARE_ERROR_SINGLE_INSTANCE						(TMSHARE_ERROR_NONE + 24)
#define TMSHARE_ERROR_NO_REQUEST							(TMSHARE_ERROR_NONE + 25)

//	Default property values
#define TMSHARE_DEFAULT_ENABLE_ERRORS						TRUE
#define TMSHARE_DEFAULT_OWNER								0
#define TMSHARE_DEFAULT_TIME_OUT							2000
#define TMSHARE_DEFAULT_PEEK_PERIOD							250
#define TMSHARE_DEFAULT_ENABLE_AX_ERRORS					TRUE
#define TMSHARE_DEFAULT_APP_FOLDER							""

#endif