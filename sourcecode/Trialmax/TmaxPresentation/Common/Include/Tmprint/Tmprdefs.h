//==============================================================================
//
// File Name:	tmprdefs.h
//
// Description:	This file contains defines used by the tm_print.ocx control. This
//				file is required to programmatically manage one of these 
//				controls.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	12-13-99	1.00		Original Release
//==============================================================================
#if !defined(__TMPRDEFS_H__)
#define __TMPRDEFS_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Print flag identifiers
#define TMPRINT_SPLITZAP_RIGHT			0x0001
#define TMPRINT_SPLITZAP_HORIZONTAL		0x0002

//	Default property values
#define TMPRINT_AUTOINIT				TRUE
#define TMPRINT_INIFILE					"Fti.ini"
#define TMPRINT_INISECTION				"TMPRINT SETUP"
#define TMPRINT_ENABLEPOWERPOINT		TRUE
#define TMPRINT_COLLATE					FALSE
#define TMPRINT_COPIES					1
#define TMPRINT_INCLUDEPATHINFILENAME	TRUE
#define TMPRINT_INCLUDEPAGETOTAL		TRUE
#define TMPRINT_PRINTIMAGE				TRUE
#define TMPRINT_PRINTBARCODEGRAPHIC		TRUE					
#define TMPRINT_PRINTBARCODETEXT		TRUE
#define TMPRINT_PRINTFOREIGNBARCODE		FALSE
#define TMPRINT_PRINTSOURCEBARCODE		FALSE
#define TMPRINT_PRINTNAME				TRUE
#define TMPRINT_PRINTFILENAME			TRUE
#define TMPRINT_PRINTDEPONENT			TRUE
#define TMPRINT_PRINTPAGENUMBER			TRUE
#define TMPRINT_PRINTCELLBORDER			TRUE					
#define TMPRINT_PRINTER					""
#define TMPRINT_TEMPLATENAME			""
#define TMPRINT_AUTOROTATE				FALSE
#define TMPRINT_FORCENEWPAGE			FALSE
#define TMPRINT_INSERTSLIPSHEET			FALSE
#define TMPRINT_SHOWOPTIONS				TRUE
#define TMPRINT_USESLIDEIDS				FALSE
#define TMPRINT_BARCODECHARACTER		"x"
#define TMPRINT_BARCODEFONT				"Free 3 of 9"
#define TMPRINT_SHOWSTATUS				TRUE
#define TMPRINT_LEFTMARGIN				-1.00f
#define TMPRINT_TOPMARGIN				-1.00f
#define TMPRINT_PRINTCALLOUTS			TRUE
#define TMPRINT_PRINTCALLOUTBORDERS		TRUE
#define TMPRINT_PRINTBORDERCOLOR		((OLE_COLOR)RGB(0x00,0x00,0x00))
#define TMPRINT_PRINTBORDERTHICKNESS	0.025f
#define TMPRINT_CALLOUTFRAMECOLOR		3
#define TMPRINT_ENABLEAXERRORS			FALSE
#define TMPRINT_JOBNAME					""

//	Error levels
#define TMPRINT_NOERROR					0
#define TMPRINT_CREATEOPTIONSFAILED		1
#define TMPRINT_ININOTFOUND				2
#define TMPRINT_NOTINITIALIZED			3
#define TMPRINT_INVALIDSTRING			4
#define TMPRINT_START_FAILED			5
#define TMPRINT_TEMPLATENOTFOUND		6
#define TMPRINT_PRINTERNOTFOUND			7
#define TMPRINT_ATTACH_FAILED			8
#define TMPRINT_SET_PROPERTIES_FAILED	9
#define TMPRINT_TEXTFIELD_NOT_FOUND		10

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------



#endif // !defined(__TMPRDEFS_H__)