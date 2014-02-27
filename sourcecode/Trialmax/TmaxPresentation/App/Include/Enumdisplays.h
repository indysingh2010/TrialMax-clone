//==============================================================================
//
// File Name:	enumdisplays.h
//
// Description:	This file contains declarations required to enumerate the 
//				system displays
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2004
//
//==============================================================================
//	Date		Revision    Description
//	01-04-2004	1.00		Original Release
//==============================================================================
#if !defined(__ENUMDISPLAYS_H__)
#define __ENUMDISPLAYS_H__

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

#ifndef SM_CMONITORS

	typedef HANDLE HMONITOR;

#endif
   
#ifndef DISPLAY_DEVICE_PRIMARY_DEVICE

	typedef struct _DISPLAY_DEVICE {
	DWORD  cb;
	TCHAR  DeviceName[32];
	TCHAR  DeviceString[128];
	DWORD  StateFlags;
	} DISPLAY_DEVICE, *PDISPLAY_DEVICE, *LPDISPLAY_DEVICE;
	
	#define DISPLAY_DEVICE_ATTACHED_TO_DESKTOP	0x00000001
	#define DISPLAY_DEVICE_MULTI_DRIVER			0x00000002
	#define DISPLAY_DEVICE_PRIMARY_DEVICE		0x00000004
	#define DISPLAY_DEVICE_VGA					0x00000010

#endif

#ifndef EnumDisplayDevices

    typedef BOOL (WINAPI* ENUMDISPLAYDEVICESPROC)(PVOID,DWORD,PVOID,DWORD);
    typedef BOOL (WINAPI* ENUMDISPLAYSETTINGSSPROC)(PVOID,DWORD,PVOID);

#endif
//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

#endif // __ENUMDISPLAYS_H__			