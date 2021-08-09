//==============================================================================
//
// File Name:	tmvdefs.h
//
// Description:	This file contains defines used by the tm_view.ocx control. This
//				file is required to programmatically manage one of these 
//				controls.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	07-12-97	1.00		Original Release
//==============================================================================
#if !defined(__TMVDEFS_H__)
#define __TMVDEFS_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	User actions
#define NONE							0
#define ZOOM							1
#define DRAW							2
#define REDACT							3
#define HIGHLIGHT						4
#define SELECT							5
#define CALLOUT							6
#define PAN								7

//	Pane identifiers	
#define TMV_ACTIVEPANE				   -1
#define TMV_LEFTPANE					0
#define TMV_RIGHTPANE					1

//	Color definitions 
#define TMV_BLACK						0
#define TMV_RED							1
#define TMV_GREEN						2
#define TMV_BLUE						3
#define TMV_YELLOW						4
#define TMV_MAGENTA						5
#define TMV_CYAN						6
#define TMV_GREY						7
#define TMV_WHITE						8
#define TMV_DARKRED						9
#define TMV_DARKGREEN					10
#define TMV_DARKBLUE					11
#define TMV_LIGHTRED					12
#define TMV_LIGHTGREEN					13
#define TMV_LIGHTBLUE					14

//	Bitonal scaling options
#define TMV_BITONALNORMAL				0
#define TMV_BITONALBLACK				1
#define TMV_BITONALGRAY					2

//	Transparency levels
#define TMV_OPAQUE						0
#define TMV_TRANSLUCENT					1
#define TMV_TRANSPARENT					2

//	Drawing tools. The order here must match the order that appears
//	on the Annotation Properties dialog
#define FREEHAND						0
#define LINE							1
#define ARROW							2
#define ELLIPSE							3
#define RECTANGLE						4
#define FILLED_ELLIPSE					5
#define FILLED_RECTANGLE				6
#define POLYLINE						7
#define POLYGON							8
#define ANNTEXT							9

//	Pan direction identifiers
#define PAN_LEFT						0
#define PAN_RIGHT						1
#define PAN_UP							2
#define PAN_DOWN						3

//	These bits are packed together to indicate if the image can be panned
//	in any direction. The packed value is returned by the method GetPanStates()
#define ENABLE_PANLEFT					0x0001
#define ENABLE_PANRIGHT					0x0002
#define ENABLE_PANUP					0x0004
#define ENABLE_PANDOWN					0X0008

//	These identifiers are used to determine the current zoom state of the image
//	in calls to the GetZoomState() method
#define ZOOMED_NONE						0
#define ZOOMED_FULLWIDTH				1
#define ZOOMED_FULLHEIGHT				2
#define ZOOMED_USER						3
#define ZOOMED_ZAP						4

//	Print orientation identifiers
#define PRINT_ORIENTATION_AUTO			0
#define PRINT_ORIENTATION_PORTRAIT		1
#define PRINT_ORIENTATION_LANDSCAPE		2

//	Relative Zap Modes
#define RELATIVE_MODE_ZAPPED			0
#define RELATIVE_MODE_CONTROL			1
#define RELATIVE_MODE_SCREEN			2
				
//	Options for runtime property configuration. OR these flags together.
//	By default, all options are enabled. Use these flags to disable specific
//	properties.
#define DISABLE_SCALEIMAGE				0x00000001L
#define DISABLE_FITTOIMAGE				0x00000002L
#define DISABLE_ROTATION				0x00000004L
#define DISABLE_HIDESCROLLBARS			0x00000008L
#define DISABLE_PANPERCENT				0x00000010L

//	Default property values
#define DEFAULT_ANNTOOL					FREEHAND
#define DEFAULT_ANNTHICKNESS			1
#define DEFAULT_ANNCOLOR				TMV_RED
#define DEFAULT_REDACTCOLOR				TMV_RED
#define DEFAULT_HIGHLIGHTCOLOR			TMV_YELLOW
#define DEFAULT_CALLOUTCOLOR			TMV_CYAN
#define DEFAULT_BACKGROUNDCOLOR			TMV_BLACK
#define DEFAULT_MAXZOOM					5
#define DEFAULT_ROTATION				0
#define DEFAULT_ACTION					NONE
#define DEFAULT_ASPECTRATIO				0
#define DEFAULT_SCALEIMAGE				TRUE
#define DEFAULT_FITTOIMAGE				FALSE
#define DEFAULT_HIDESCROLLBARS			TRUE
#define DEFAULT_PANPERCENT				25
#define DEFAULT_CALLFRAMETHICKNESS		5
#define DEFAULT_CALLFRAMECOLOR			TMV_BLUE
#define DEFAULT_CALLHANDLECOLOR			TMV_YELLOW
#define DEFAULT_RESIZECALLOUTS			TRUE
#define DEFAULT_RIGHTCLICKPAN			TRUE
#define DEFAULT_SPLITFRAMETHICKNESS		5
#define DEFAULT_SPLITFRAMECOLOR			TMV_GREY
#define DEFAULT_SPLITSCREEN				FALSE
#define DEFAULT_ACTIVEPANE				TMV_LEFTPANE
#define DEFAULT_SYNCPANES				TRUE
#define DEFAULT_SYNCCALLOUTANN			TRUE
#define DEFAULT_PENSELECTORVISIBLE		FALSE
#define DEFAULT_PENSELECTORCOLOR		TMV_GREY
#define DEFAULT_PENSELECTORSIZE			5
#define DEFAULT_KEEPASPECT				TRUE
#define DEFAULT_BITONALSCALING			TMV_BITONALNORMAL
#define DEFAULT_ZOOMTORECT				FALSE
#define DEFAULT_ANNFONTNAME				"Arial"
#define DEFAULT_ANNFONTSIZE				12
#define DEFAULT_ANNFONTBOLD				FALSE
#define DEFAULT_ANNFONTUNDERLINE		FALSE
#define DEFAULT_ANNFONTSTRIKETHROUGH	FALSE
#define DEFAULT_DESKEWBACKCOLOR			(RGB(0xFF,0xFF,0xFF))
#define DEFAULT_ANNOTATECALLOUTS		TRUE
#define DEFAULT_PRINTORIENTATION		0
#define DEFAULT_PRINTBORDERCOLOR		((OLE_COLOR)RGB(0,0,0))
#define DEFAULT_PRINTBORDERTHICKNESS	0.025f
#define DEFAULT_PRINTLEFTMARGIN			0.50f
#define DEFAULT_PRINTRIGHTMARGIN		0.50f
#define DEFAULT_PRINTTOPMARGIN			0.50f
#define DEFAULT_PRINTBOTTOMMARGIN		0.50f
#define DEFAULT_PRINTCALLOUTS			FALSE
#define DEFAULT_PRINTBORDER				FALSE
#define DEFAULT_PRINTCALLOUTBORDERS		TRUE
#define DEFAULT_ANNCOLORDEPTH			8
#define DEFAULT_SPLITHORIZONTAL			FALSE
#define DEFAULT_QFACTOR					16
#define DEFAULT_SHADEONCALLOUT			FALSE
#define DEFAULT_CALLOUTSHADEGRAYSCALE	0xc0
#define DEFAULT_RELATIVEZAPMODE			RELATIVE_MODE_ZAPPED
#define DEFAULT_PANCALLOUTS				FALSE
#define DEFAULT_ZOOMCALLOUTS			FALSE
#define DEFAULT_LOADASYNC				FALSE
#define DEFAULT_ENABLEAXERRORS			FALSE
#define DEFAULT_USESCREENRATIO			FALSE

//	Mouse pointers
#define MP_DEFAULT						0
#define	MP_ARROW						1
#define MP_CROSSHAIR					2
#define MP_IBEAM						3
#define MP_SQUAREINSQUARE				4
#define MP_CROSS						5
#define MP_NESW							6
#define MP_NS							7
#define MP_NWSE							8
#define MP_WE							9
#define MP_UP							10
#define MP_HOURGLASS					11
#define MP_NODROP						12
#define MP_ARROWHOURGLASS				13
#define MP_ARROWQUESTION				14

//	Mouse button identifiers used in events
#define NO_MOUSEBUTTON					0
#define LEFT_MOUSEBUTTON				1
#define RIGHT_MOUSEBUTTON				2

//	Key identifiers used in events
#define SHIFT							1
#define CTRL							2
#define ALT								4
#define CTRLSHIFT						3
#define ALTSHIFT						5
#define CTRLALT							6
#define CTRLALTSHIFT					7

//	Error levels
#define TMV_NOERROR						0
#define TMV_FILENOTFOUND				1	//	Unable to locate the file
#define TMV_INVALIDNAME					2	//	Filename is not valid	
#define TMV_NOIMAGE						3	//	Image is not loaded
#define TMV_UNKNOWNERROR				4	//	Unknown error occurred
#define TMV_PAGEBOUNDRYEXCEEDED			5	//	Page not available
#define TMV_LOWMEMORY					6	//	Insufficient memory for operation
#define TMV_FILEREAD					7	//	Unable to read the file
#define TMV_FILEFORMAT					8	//	File format not supported
#define TMV_INVALIDSUBTYPE				9	//	Format supported but not subtype
#define TMV_FILEOPEN					10	//	Unable to open the file
#define TMV_COMPRESSION					11	//	Compression format not supported
#define TMV_NOTANIMATION				12	//	File is not an animation
#define TMV_NODEFAULTPRINTER			13	//	Default printer not defined
#define TMV_STARTJOBFAILED				14	//	Unable to start print job
#define TMV_ZAPNOTFOUND					15	//	Unable to locate zap file
#define TMV_INVALIDZAP					16	//	Zap file is not valid
#define TMV_NOCALLOUTWND				17	//	Unable to create callout window
#define TMV_SAVEZAPFAILED				18	//	Unable to save the zap file
#define TMV_LOADZAPFAILED				19  //	Unable to load the zap file
#define TMV_NOANNOTATIONS				20	//	No annotations were found
#define TMV_LZWLOCKED					21	//	LZW compression is locked
#define TMV_INVALIDCLIPBOARD			22	//	Clipboard data is not valid
#define TMV_COPYFAILED					23	//	Unable to copy to clipboard
#define TMV_PASTEFAILED					24	//	Unable to paste from clipboard
#define TMV_LEADERROR					25	//	LeadTools generated an error
#define TMV_ANNNOTFOUND					26	//	Unable to locate the specified annotation
#define TMV_ANNLOCKED					27	//	Unable to perform the operation. The annotation is locked
#define TMV_OPENINIFAILED				28	//	Unable to open the INI file to set the properties
#define TMV_SAVEEXCEPTION				29	//	Lead Tools exception raised on attempt to Save
#define TMV_NOTSPLITSCREEN				30	//	Control is not in split screen mode

//	Lengths for image property buffers
#define TMV_MAXLEN_PROP_FILENAME		512
#define TMV_MAXLEN_PROP_BITSPERPIXEL	8
#define TMV_MAXLEN_PROP_DISKSIZE		32
#define TMV_MAXLEN_PROP_RAMSIZE			32
#define TMV_MAXLEN_PROP_INCHES			64
#define TMV_MAXLEN_PROP_PIXELS			64
#define TMV_MAXLEN_PROP_PAGE			32
#define TMV_MAXLEN_PROP_COMPRESSION		64
#define TMV_MAXLEN_PROP_TYPE			64

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	This structure is used to retrieve image properties from the viewer
typedef struct
{
	char	szFilename[TMV_MAXLEN_PROP_FILENAME];
	char	szBitsPerPixel[TMV_MAXLEN_PROP_BITSPERPIXEL];
	char	szDiskSize[TMV_MAXLEN_PROP_DISKSIZE];
	char	szRamSize[TMV_MAXLEN_PROP_RAMSIZE];
	char	szDimInches[TMV_MAXLEN_PROP_INCHES];
	char	szDimPixels[TMV_MAXLEN_PROP_PIXELS];
	char	szPage[TMV_MAXLEN_PROP_PAGE];
	char	szCompression[TMV_MAXLEN_PROP_COMPRESSION];
	char	szType[TMV_MAXLEN_PROP_TYPE];
}STMVImageProperties;


#endif // !defined(__TMVDEFS_H__)