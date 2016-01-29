//==============================================================================
//
// File Name:	tmini.cpp
//
// Description:	This file contains member functions of the CTMIni class and its
//				support classes.
//
// See Also:	tmini.h
//
//==============================================================================
//	Date		Revision    Description
//	08-30-97	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <direct.h>		// getcwd()
#include <tmini.h>
#include <string>
#include <Windows.h>

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------

//==============================================================================
//
// 	Function Name:	CTMIni::CTMIni()
//
// 	Description:	This is the constructor for CTMIni objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMIni::CTMIni(LPCSTR lpFilename, LPCSTR lpSection)
{
    char szDirectory[512];

	bFileFound = FALSE;

	//	Clear the local buffers
	strFileSpec.Empty();
	strDirectory.Empty();
	strSection.Empty();

    //	Get the current working directory
    _getcwd(szDirectory, sizeof(szDirectory));
	strDirectory = szDirectory;
	if(strDirectory.Right(1) != "\\")
		strDirectory += "\\";

	//	Attempt to open the file if a filename was provided
	if(lpFilename && strlen(lpFilename) > 0)
		Open(lpFilename, lpSection);
}

//==============================================================================
//
// 	Function Name:	CTMIni::~CTMIni()
//
// 	Description:	This is the destructor for CTMIni objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMIni::~CTMIni()
{

}

//==============================================================================
//
// 	Function Name:	CTMIni::Close()
//
// 	Description:	Windows caches read/write requests to ini files. This
//					function will flush the cache and then clear the filename
//					and section buffers. If the bDelete parameter is TRUE,
//					the current ini section will be deleted.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMIni::Close(BOOL bDelete)
{
	//	Has the ini file been opened?
	if(!bFileFound)
		return;
    else
    	bFileFound = FALSE;

    // 	Should the current section be deleted before closing the file?
    if(bDelete)
    	WritePrivateProfileString(strSection, NULL, NULL, strFileSpec);

    //	Flush the cache buffer
	WritePrivateProfileString(NULL, NULL, NULL, strFileSpec);

    //	Clear the local buffers
	strFileSpec.Empty();
	strSection.Empty();
}

//==============================================================================
//
// 	Function Name:	CTMIni::DeleteLine()
//
// 	Description:	This function will remove the specified line from the
//					active file and section.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMIni::DeleteLine(LPCSTR lpLine)
{
	if(!lpLine)
    	return;
    else
    	WritePrivateProfileString(strSection, lpLine, NULL, strFileSpec);

}

//==============================================================================
//
// 	Function Name:	CTMIni::DeleteSection()
//
// 	Description:	This function will remove the specified section from the
//					active file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMIni::DeleteSection(LPCSTR lpSection)
{
	if(!lpSection)
    	return;
    else
    	WritePrivateProfileString(lpSection, NULL, NULL, strFileSpec);

}

//==============================================================================
//
// 	Function Name:	CTMIni::Open()
//
// 	Description:	This function will open the ini file requested by the
//					caller. Ini files are not actually opened until an attempt
//					is made to read or write from the file. This function will
//					close any active file and then store the filename and
//					section name in the appropriate buffers.
//
// 	Returns:		TRUE if the file exists. FALSE otherwise 
//
//	Notes:			None
//
//==============================================================================
BOOL CTMIni::Open(LPCSTR lpFilename, LPCSTR lpSection)
{
	WIN32_FIND_DATA	FindData;
	HANDLE			hFind;

	//	First flush the cache buffer if a file is already open
	Close();

	//	Construct the complete file specification
	if(lpFilename)
    {
        //  If this is a local file reference, append current path
		if((strchr(lpFilename, ':') == NULL) && (strchr(lpFilename, '\\') == NULL))
		{
			strFileSpec = strDirectory;
			strFileSpec += lpFilename;
		}
		else
		{
			strFileSpec = lpFilename;
		}	
	
	}

	if(lpSection)
		strSection = lpSection;

    // 	See if the file exists. This gives the caller a means of checking
    // 	before calling a read function
	if((hFind = FindFirstFile(strFileSpec, &FindData)) != INVALID_HANDLE_VALUE)
	{
		bFileFound = TRUE;
		FindClose(hFind);
	}

    return bFileFound;

}

//==============================================================================
//
//	Function Name:	CTMIni::ReadBool()
//
//	Description:	This function will read a boolean value from the open ini
//					file. If the specified line item is not found, the default
//					value is returned.
//
//	Returns:		The ini value or the default
//
//	Notes:			If bDelete = TRUE, then the specified line will be deleted
//					once it is read in. 
//
//==============================================================================
BOOL CTMIni::ReadBool(LPCSTR lpLine, BOOL bDefault, BOOL bDelete)
{
	CString	strDefault;
    char	szBuffer[32];

    if(!lpLine)
    	return bDefault;

    //	Format the default string
    if(bDefault)
		strDefault = "TRUE";
	else
		strDefault = "FALSE";

    //	Read the ini string
    GetPrivateProfileString(strSection, lpLine, strDefault, szBuffer,
							sizeof(szBuffer), strFileSpec);

	//	Delete the ini line if requested
	if(bDelete)
		DeleteLine(lpLine);

	return (lstrcmpi(szBuffer, "TRUE") == 0) ? TRUE : FALSE;

}

//==============================================================================
//
//	Function Name:	CTMIni::ReadBool()
//
//	Description:	This form of the function allows the caller to provide a
//					line number instead of a line name.
//
//	Returns:		The ini value or the default
//
//	Notes:			If bDelete = TRUE, then the specified line will be deleted
//					once it is read in. 
//
//==============================================================================
BOOL CTMIni::ReadBool(int iLine, BOOL bDefault, BOOL bDelete)
{
	char szLine[32];

	//	Format the line identifier
	wsprintf(szLine, "%d", iLine);

	return ReadBool(szLine, bDefault, bDelete);
}

//==============================================================================
//
//	Function Name:	CTMIni::ReadCaptureOptions()
//
//	Description:	This function will read the screen capture options from the
//					ini file
//
//	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMIni::ReadCaptureOptions(SCaptureOptions* pOptions)
{
	char FilePath[200]="";
	char DefFilePath[MAX_PATH];
	_getcwd(DefFilePath, sizeof(DefFilePath));
	strDirectory = DefFilePath;
	
	strDirectory.MakeLower();
	if(strDirectory.Right(1) != "\\")
		strDirectory += "\\";
	strDirectory+= "video capture\\";
	if(CreateDirectory(strDirectory, NULL) || ERROR_ALREADY_EXISTS ==GetLastError())
	{
		//Directory Created
	}
	else
	{
		//Failed to create directory
	}
	
	if(bFileFound)
	{
		//	Line up on the correct section
		SetSection(TMGRAB_SECTION);
		ReadString(CAPTURE_FILE_PATH,FilePath,sizeof(FilePath) ,strDirectory);
		//AfxMessageBox("Test Tmini.cpp Read Capture Options");
		pOptions->bSilent = ReadBool(CAPTURE_SILENT_LINE, DEFAULT_CO_SILENT);
		pOptions->sHotkey = (short)ReadLong(CAPTURE_HOTKEY_LINE, DEFAULT_CO_HOTKEY);
		pOptions->sCancelKey = (short)ReadLong(CAPTURE_CANCELKEY_LINE, DEFAULT_CO_CANCELKEY);
		pOptions->sArea = (short)ReadLong(CAPTURE_AREA_LINE, DEFAULT_CO_AREA);
		pOptions->sFilePath=FilePath;
	}
	else
	{
		pOptions->bSilent = DEFAULT_CO_SILENT;
		pOptions->sHotkey = DEFAULT_CO_HOTKEY;
		pOptions->sCancelKey = DEFAULT_CO_CANCELKEY;
		pOptions->sArea = DEFAULT_CO_AREA;
		pOptions->sFilePath = strDirectory;
	}

	return TRUE;
}

//==============================================================================
//
//	Function Name:	CTMIni::ReadCommandLine()
//
//	Description:	This function will read the command line options from the 
//					ini file
//
//	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMIni::ReadCommandLine(SCommandLine* pCommandLine)
{
	//	Read the database options
	if(bFileFound)
	{
		SetSection(PRESENTATION_COMMANDLINE_SECTION);
		ReadString(COMMANDLINE_BARCODE, pCommandLine->szBarcode, 
				   sizeof(pCommandLine->szBarcode), DEFAULT_BARCODE);
		ReadString(COMMANDLINE_CASEFOLDER, pCommandLine->szCaseFolder, 
				   sizeof(pCommandLine->szCaseFolder), DEFAULT_CASEFOLDER);

		pCommandLine->lPageNumber = ReadLong(COMMANDLINE_PAGENUMBER, DEFAULT_PAGENUMBER);
		pCommandLine->iLineNumber = (int)ReadLong(COMMANDLINE_LINENUMBER, DEFAULT_LINENUMBER);
	}
	else
	{
		lstrcpyn(pCommandLine->szBarcode, DEFAULT_BARCODE, sizeof(pCommandLine->szBarcode));
		lstrcpyn(pCommandLine->szCaseFolder, DEFAULT_CASEFOLDER, sizeof(pCommandLine->szCaseFolder));
		pCommandLine->lPageNumber = 0;
		pCommandLine->iLineNumber = 0;
	}

	return TRUE;
}

//==============================================================================
//
//	Function Name:	CTMIni::ReadDatabaseOptions()
//
//	Description:	This function will read the database options from the ini
//					file
//
//	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMIni::ReadDatabaseOptions(SDatabaseOptions* pOptions)
{
	char szIniStr[512];

	//	Read the database options
	if(bFileFound)
	{
		ReadLastCase(szIniStr, sizeof(szIniStr));
		pOptions->strFolder = szIniStr;
		pOptions->bEnableErrors = ReadEnableErrors(PRESENTATION_APP);
		pOptions->bEnablePowerPoint = ReadEnablePowerPoint();
	}
	else
	{
		pOptions->strFolder.Empty();
		pOptions->bEnableErrors = DEFAULT_ENABLEERRORS;
		pOptions->bEnablePowerPoint = DEFAULT_ENABLEPOWERPOINT;
	}

	return TRUE;
}

//==============================================================================
//
// Function Name:	CTMIni::ReadDouble()
//
// Description:		This function will read a floating point value from the 
//					open ini file. If the specified line item is not found, 
//					the default value is returned.
//
// Returns:			The ini value or the default
//
//	Notes:			If bDelete = TRUE, then the specified line will be deleted
//					once it is read in. 
//
//==============================================================================
double CTMIni::ReadDouble(LPCSTR lpLine, double dDefault, BOOL bDelete)
{
	CString	strDefault;
    char	szBuffer[32];

    if(!lpLine)
		return dDefault;

    //	Format the default string
	strDefault.Format("%.10lg", dDefault);

    //	Read the ini string
    GetPrivateProfileString(strSection, lpLine, strDefault, szBuffer,
    						sizeof(szBuffer), strFileSpec);

	//	Delete the ini line if requested
	if(bDelete)
		DeleteLine(lpLine);

	return atof(szBuffer);
}

//==============================================================================
//
// Function Name:	CTMIni::ReadDouble()
//
//	Description:	This form of the function allows the caller to provide a
//					line number instead of a line name.
//
// Returns:			The ini value or the default
//
//	Notes:			If bDelete = TRUE, then the specified line will be deleted
//					once it is read in. 
//
//==============================================================================
double CTMIni::ReadDouble(int iLine, double dDefault, BOOL bDelete)
{
	char szLine[32];

	//	Format the line identifier
	wsprintf(szLine, "%d", iLine);

	return ReadDouble(szLine, dDefault, bDelete);
}

//==============================================================================
//
//	Function Name:	CTMIni::ReadLong()
//
//	Description:	This function will read a long integer from the open ini
//					file. If the specified line item is not found, the default
//					value is returned.
//
//	Returns:		The ini value or the default
//
//	Notes:			If bDelete = TRUE, then the specified line will be deleted
//					once it is read in. 
//
//==============================================================================
long CTMIni::ReadLong(LPCSTR lpLine, long lDefault, BOOL bDelete)
{
	CString	strDefault;
    char	szBuffer[32];

    if(!lpLine)
		return lDefault;

    //	Format the default string
	strDefault.Format("%ld", lDefault);

    //	Read the ini string
    GetPrivateProfileString(strSection, lpLine, strDefault, szBuffer,
    						sizeof(szBuffer), strFileSpec);

	//	Delete the ini line if requested
	if(bDelete)
		DeleteLine(lpLine);

	return atol(szBuffer);
}

//==============================================================================
//
//	Function Name:	CTMIni::ReadEnableErrors()
//
//	Description:	This function will read the EnableErrors option for the
//					the application specified by the caller.
//
//	Returns:		TRUE if enabled
//
//	Notes:			None
//
//==============================================================================
BOOL CTMIni::ReadEnableErrors(int iApplication)
{
	if(bFileFound)
	{
		//	Set the appropriate section
		if(!SetTMSection(iApplication))
			return DEFAULT_ENABLEERRORS;

		return ReadBool(ENABLEERRORS_LINE, DEFAULT_ENABLEERRORS);
	}
	else
	{
		return DEFAULT_ENABLEERRORS;
	}
}

//==============================================================================
//
//	Function Name:	CTMIni::ReadEnablePowerPoint()
//
//	Description:	This function will read the EnablePowerPoint flag from the
//					shared section of the ini file
//
//	Returns:		The FrameRate value
//
//	Notes:			None
//
//==============================================================================
BOOL CTMIni::ReadEnablePowerPoint()
{
	if(bFileFound)
	{
		//	Line up on the shared section
		SetSection(SHARED_SECTION);

		//	Read the value
		return ReadBool(ENABLEPOWERPOINT_LINE, DEFAULT_ENABLEPOWERPOINT);
	}
	else
	{
		return DEFAULT_ENABLEPOWERPOINT;
	}
}

//==============================================================================
//
//	Function Name:	CTMIni::ReadFrameRatee()
//
//	Description:	This function will read the FrameRate value from the
//					shared section of the ini file
//
//	Returns:		The FrameRate value
//
//	Notes:			None
//
//==============================================================================
double CTMIni::ReadFrameRate()
{
	if(bFileFound)
	{
		//	Line up on the shared section
		SetSection(SHARED_SECTION);

		//	Read the value
		return ReadDouble(FRAMERATE_LINE, DEFAULT_FRAMERATE);
	}
	else
	{
		return DEFAULT_FRAMERATE;
	}
}

//==============================================================================
//
//	Function Name:	CTMIni::ReadGraphicsOptions()
//
//	Description:	This function will read the graphics options from the ini
//					file
//
//	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMIni::ReadGraphicsOptions(SGraphicsOptions* pOptions)
{
	char szIniStr[512];

	if(bFileFound)
	{
		//	Line up on the correct section
		SetSection(PRESENTATION_SECTION);

		pOptions->sAnnColor = (short)ReadLong(ANNCOLOR_LINE, DEFAULT_GO_ANNCOLOR);
		pOptions->sAnnThickness = (short)ReadLong(ANNTHICKNESS_LINE, DEFAULT_GO_ANNTHICKNESS);
		pOptions->sHighlightColor = (short)ReadLong(HIGHLIGHTCOLOR_LINE, DEFAULT_GO_HIGHLIGHTCOLOR);
		pOptions->sRedactColor = (short)ReadLong(REDACTCOLOR_LINE, DEFAULT_GO_REDACTCOLOR);
		pOptions->sMaxZoom = (short)ReadLong(MAXZOOM_LINE, DEFAULT_GO_MAXZOOM);
		pOptions->sCalloutColor = (short)ReadLong(CALLOUTCOLOR_LINE, DEFAULT_GO_CALLOUTCOLOR);
		pOptions->sCalloutHandleColor = (short)ReadLong(CALLHANDLECOLOR_LINE, DEFAULT_GO_CALLHANDLECOLOR);
		pOptions->sCalloutFrameColor = (short)ReadLong(CALLFRAMECOLOR_LINE, DEFAULT_GO_CALLFRAMECOLOR);
		pOptions->sCalloutFrameThickness = (short)ReadLong(CALLFRAMETHICK_LINE, DEFAULT_GO_CALLFRAMETHICKNESS);
		pOptions->sBitonalScaling = (short)ReadLong(BITONALSCALING_LINE, DEFAULT_GO_BITONALSCALING);
		pOptions->sAnnTool = (short)ReadLong(DRAWTOOL_LINE, DEFAULT_GO_ANNTOOL);
		pOptions->sAnnFontSize = (short)ReadLong(ANNFONTSIZE_LINE, DEFAULT_GO_ANNFONTSIZE);
		pOptions->sLightPenColor = (short)ReadLong(LIGHTPENCOLOR_LINE, DEFAULT_GO_LIGHTPENCOLOR);
		pOptions->sLightPenSize = (short)ReadLong(LIGHTPENSIZE_LINE, DEFAULT_GO_LIGHTPENSIZE);
		pOptions->sUserSplitFrameColor = (short)ReadLong(USER_SPLITFRAMECOLOR_LINE, DEFAULT_GO_USER_SPLITFRAMECOLOR);
		pOptions->sZapSplitFrameColor = (short)ReadLong(ZAP_SPLITFRAMECOLOR_LINE, DEFAULT_GO_ZAP_SPLITFRAMECOLOR);
		pOptions->sCalloutShadeGrayscale = (short)ReadLong(CALLOUTSHADEGRAYSCALE_LINE, DEFAULT_GO_CALLOUTSHADEGRAYSCALE);
		pOptions->bShadeOnCallout = ReadBool(SHADEONCALLOUT_LINE, DEFAULT_GO_SHADEONCALLOUT);
		pOptions->bLightPenEnabled = ReadBool(LIGHTPENENABLED_LINE, DEFAULT_GO_LIGHTPENENABLED);
		pOptions->bAnnFontBold = ReadBool(ANNFONTBOLD_LINE, DEFAULT_GO_ANNFONTBOLD);
		pOptions->bAnnFontStrikeThrough = ReadBool(ANNFONTSTRIKETHROUGH_LINE, DEFAULT_GO_ANNFONTSTRIKETHROUGH);
		pOptions->bAnnFontUnderline = ReadBool(ANNFONTUNDERLINE_LINE, DEFAULT_GO_ANNFONTUNDERLINE);
		pOptions->bScaleDocuments = ReadBool(SCALEDOCS_LINE, DEFAULT_GO_SCALEDOCS);
		pOptions->bScaleGraphics = ReadBool(SCALEGRAPHICS_LINE, DEFAULT_GO_SCALEGRAPHICS);
		pOptions->bResizableCallouts = ReadBool(RESIZABLECALLOUTS_LINE, DEFAULT_GO_RESIZABLECALLOUTS);
		pOptions->bPanCallouts = ReadBool(PANCALLOUTS_LINE, DEFAULT_GO_PANCALLOUTS);
		pOptions->bZoomCallouts = ReadBool(ZOOMCALLOUTS_LINE, DEFAULT_GO_ZOOMCALLOUTS);
		ReadString(ANNFONTNAME_LINE, szIniStr, sizeof(szIniStr), DEFAULT_GO_ANNFONTNAME);
		pOptions->strAnnFontName = szIniStr;
	}
	else
	{
		pOptions->sAnnColor = DEFAULT_GO_ANNCOLOR;
		pOptions->sAnnThickness = DEFAULT_GO_ANNTHICKNESS;
		pOptions->sHighlightColor = DEFAULT_GO_HIGHLIGHTCOLOR;
		pOptions->sRedactColor = DEFAULT_GO_REDACTCOLOR;
		pOptions->sMaxZoom = DEFAULT_GO_MAXZOOM;
		pOptions->sCalloutColor = DEFAULT_GO_CALLOUTCOLOR;
		pOptions->sCalloutHandleColor = DEFAULT_GO_CALLHANDLECOLOR;
		pOptions->sCalloutFrameColor = DEFAULT_GO_CALLFRAMECOLOR;
		pOptions->sCalloutFrameThickness = DEFAULT_GO_CALLFRAMETHICKNESS;
		pOptions->sBitonalScaling = DEFAULT_GO_BITONALSCALING;
		pOptions->sAnnTool = DEFAULT_GO_ANNTOOL;
		pOptions->sAnnFontSize = DEFAULT_GO_ANNFONTSIZE;
		pOptions->sLightPenColor = DEFAULT_GO_LIGHTPENCOLOR;
		pOptions->sLightPenSize = DEFAULT_GO_LIGHTPENSIZE;
		pOptions->bLightPenEnabled = DEFAULT_GO_LIGHTPENENABLED;
		pOptions->sZapSplitFrameColor = DEFAULT_GO_ZAP_SPLITFRAMECOLOR;
		pOptions->sCalloutShadeGrayscale = DEFAULT_GO_CALLOUTSHADEGRAYSCALE;
		pOptions->bAnnFontBold = DEFAULT_GO_ANNFONTBOLD;
		pOptions->bAnnFontStrikeThrough = DEFAULT_GO_ANNFONTSTRIKETHROUGH;
		pOptions->bAnnFontUnderline = DEFAULT_GO_ANNFONTUNDERLINE;
		pOptions->bScaleDocuments = DEFAULT_GO_SCALEDOCS;
		pOptions->bScaleGraphics = DEFAULT_GO_SCALEGRAPHICS;
		pOptions->bResizableCallouts = DEFAULT_GO_RESIZABLECALLOUTS;
		pOptions->bPanCallouts = DEFAULT_GO_PANCALLOUTS;
		pOptions->bZoomCallouts = DEFAULT_GO_ZOOMCALLOUTS;
		pOptions->strAnnFontName = DEFAULT_GO_ANNFONTNAME;
		pOptions->sCalloutShadeGrayscale = DEFAULT_GO_CALLOUTSHADEGRAYSCALE;
		pOptions->bShadeOnCallout = DEFAULT_GO_SHADEONCALLOUT;
	}

	return TRUE;
}

//==============================================================================
//
//	Function Name:	CTMIni::ReadLastCase()
//
//	Description:	This function will read the information for the last case
//					opened by one of the FTI applications.
//
//	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
void CTMIni::ReadLastCase(LPSTR lpFolder, int iLength)
{
	if(bFileFound)
	{
		//	Line up on the shared section
		SetSection(SHARED_SECTION);

		//	Read the case information
		ReadString(LASTCASE_LINE, lpFolder, iLength);
	}
	else
	{
		memset(lpFolder, 0, iLength);
	}
}

//==============================================================================
//
//	Function Name:	CTMIni::ReadLong()
//
//	Description:	This form of the function allows the caller to provide a
//					line number instead of a line name.
//
//	Returns:		The ini value or the default
//
//	Notes:			If bDelete = TRUE, then the specified line will be deleted
//					once it is read in. 
//
//==============================================================================
long CTMIni::ReadLong(int iLine, long lDefault, BOOL bDelete)
{
	char szLine[32];

	//	Format the line identifier
	wsprintf(szLine, "%d", iLine);

	return ReadLong(szLine, lDefault, bDelete);
}

//==============================================================================
//
//	Function Name:	CTMIni::ReadString()
//
//	Description:	This function will read a character string from the 
//					open ini file. If the specified line item is not found, 
//					the default value is returned. 
//
//	Returns:		A pointer to the buffer provided by the caller
//
//	Note:			If bDelete = TRUE, then the specified line will be deleted
//					once it is read in. 
//
//					It is up to the caller to make sure that the default
//					value will fit in the string buffer.
//
//==============================================================================
LPSTR CTMIni::ReadString(LPCSTR lpLine, LPSTR lpString, int iLength,
						  LPCSTR lpDefault, BOOL bDelete)
{
	if(!lpLine || !lpString || !lpDefault)
    	return 0;

    //	Read the ini string
    GetPrivateProfileString(strSection, lpLine, lpDefault, lpString,
    						iLength, strFileSpec);

	//	Delete the ini line if requested
	if(bDelete)
		DeleteLine(lpLine);

	return lpString;

}

//==============================================================================
//
//	Function Name:	CTMIni::ReadRingtailOptions()
//
//	Description:	This function will read the ringtail options from the ini
//					file
//
//	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMIni::ReadRingtailOptions(SRingtailOptions* pOptions)
{
	char szIniStr[512];

	if(bFileFound)
	{
		//	Line up on the correct section
		SetSection(RINGTAIL_SECTION);

		pOptions->bShowRedactions = ReadBool(RT_SHOWREDACTIONS_LINE, DEFAULT_RT_SHOWREDACTIONS);
		pOptions->sRedactTransparency = (short)ReadLong(RT_REDACTTRANSPARENCY_LINE, DEFAULT_RT_REDACTTRANSPARENCY);
		pOptions->sLabelFontSize = (short)ReadLong(RT_REDACTLABELSIZE_LINE, DEFAULT_RT_REDACTLABELSIZE);
		pOptions->lRedactColor = ReadLong(RT_REDACTCOLOR_LINE, DEFAULT_RT_REDACTCOLOR);
		pOptions->lRedactLabelColor = ReadLong(RT_REDACTLABELCOLOR_LINE, DEFAULT_RT_REDACTLABELCOLOR);
		ReadString(RT_REDACTLABELFONT_LINE, szIniStr, sizeof(szIniStr), DEFAULT_RT_REDACTLABELFONT);
		pOptions->strLabelFontName = szIniStr;

	}
	else
	{
		pOptions->bShowRedactions = DEFAULT_RT_SHOWREDACTIONS;
		pOptions->sRedactTransparency = DEFAULT_RT_REDACTTRANSPARENCY;
		pOptions->sLabelFontSize = DEFAULT_RT_REDACTLABELSIZE;
		pOptions->lRedactColor = DEFAULT_RT_REDACTCOLOR;
		pOptions->lRedactLabelColor = DEFAULT_RT_REDACTLABELCOLOR;
		pOptions->strLabelFontName = DEFAULT_RT_REDACTLABELFONT;
	}
	return TRUE;
}

//==============================================================================
//
//	Function Name:	CTMIni::ReadString()
//
//	Description:	This form of the function allows the caller to provide a
//					line number instead of a line name.
//
//	Returns:		A pointer to the string buffer provided by the caller.
//
//	Note:			None
//
//==============================================================================
LPSTR CTMIni::ReadString(int iLine, LPSTR lpString, int iLength,
						  LPCSTR lpDefault, BOOL bDelete)
{
	char szLine[32];

	//	Format the line identifier
	wsprintf(szLine, "%d", iLine);

	return ReadString(szLine, lpString, iLength, lpDefault, bDelete);
}

//==============================================================================
//
//	Function Name:	CTMIni::ReadSystemOptions()
//
//	Description:	This function will read the system options from the ini
//					file
//
//	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMIni::ReadSystemOptions(SSystemOptions* pOptions)
{
	if(bFileFound)
	{
		//	Line up on the correct section
		SetSection(PRESENTATION_SECTION);

		pOptions->iImageSecondary = ReadLong(IMAGE_SECONDARY_LINE, DEFAULT_SO_IMAGESECONDARY);
		pOptions->iAnimationSecondary = ReadLong(ANIMATION_SECONDARY_LINE, DEFAULT_SO_ANIMATIONSECONDARY);
		pOptions->iPowerPointSecondary = ReadLong(POWERPOINT_SECONDARY_LINE, DEFAULT_SO_POWERPOINTSECONDARY);
		pOptions->iPlaylistSecondary = ReadLong(PLAYLIST_SECONDARY_LINE, DEFAULT_SO_PLAYLISTSECONDARY);
		pOptions->iCustomShowSecondary = ReadLong(CUSTOMSHOW_SECONDARY_LINE, DEFAULT_SO_CUSTOMSHOWSECONDARY);
		pOptions->iTreatmentTertiary = ReadLong(TREATMENT_TERTIARY_LINE, DEFAULT_SO_TREATMENTTERTIARY);
		pOptions->bOptimizeVideo = ReadBool(OPTIMIZEVIDEO_LINE, DEFAULT_SO_OPTIMIZEVIDEO);
		pOptions->bDualMonitors = ReadBool(DUALMONITORS_LINE, DEFAULT_SO_DUALMONITORS);
		pOptions->bOptimizeTablet = ReadBool(OPTIMIZETABLET_LINE, DEFAULT_SO_OPTIMIZETABLET);
		pOptions->bEnableBarcodeKeystrokes = ReadBool(ENABLEBARCODEKEYSTROKES_LINE, DEFAULT_SO_ENABLEBARCODEKEYSTROKES);
	}
	else
	{
		pOptions->iImageSecondary = DEFAULT_SO_IMAGESECONDARY;
		pOptions->iAnimationSecondary = DEFAULT_SO_ANIMATIONSECONDARY;
		pOptions->iPowerPointSecondary = DEFAULT_SO_POWERPOINTSECONDARY;
		pOptions->iPlaylistSecondary = DEFAULT_SO_PLAYLISTSECONDARY;
		pOptions->iCustomShowSecondary = DEFAULT_SO_CUSTOMSHOWSECONDARY;
		pOptions->iTreatmentTertiary = DEFAULT_SO_TREATMENTTERTIARY;
		pOptions->bOptimizeVideo = DEFAULT_SO_OPTIMIZEVIDEO;
		pOptions->bDualMonitors = DEFAULT_SO_DUALMONITORS;
		pOptions->bOptimizeTablet = DEFAULT_SO_OPTIMIZETABLET;
		pOptions->bEnableBarcodeKeystrokes = ReadBool(ENABLEBARCODEKEYSTROKES_LINE, DEFAULT_SO_ENABLEBARCODEKEYSTROKES);
	}
	return TRUE;
}

//==============================================================================
//
//	Function Name:	CTMIni::ReadTextOptions()
//
//	Description:	This function will read the text options from the ini
//					file
//
//	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMIni::ReadTextOptions(STextOptions* pOptions)
{
	char szIniStr[512];

	if(bFileFound)
	{
		//	Line up on the correct section
		SetSection(PRESENTATION_SECTION);

		pOptions->bDisableScrollText = ReadBool(DISABLESCROLLTEXT_LINE, DEFAULT_TO_DISABLESCROLLTEXT);
		pOptions->bUseAvgCharWidth = ReadBool(USEAVGCHAR_LINE, DEFAULT_TO_USEAVGCHAR);
		pOptions->bShowPageLine = ReadBool(SHOWPAGELINE_LINE, DEFAULT_TO_SHOWPAGELINE);
		pOptions->bCombineText = ReadBool(COMBINETEXT_LINE, DEFAULT_TO_COMBINETEXT);
		pOptions->bSmoothScroll = ReadBool(SMOOTHSCROLL_LINE, DEFAULT_TO_SMOOTHSCROLL);
		pOptions->bCenterVideo = ReadBool(CENTERVIDEO_LINE, DEFAULT_TO_CENTERVIDEO);
		pOptions->sDisplayLines = (short)ReadLong(DISPLAYLINES_LINE, DEFAULT_TO_DISPLAYLINES);
		pOptions->sHighlightLines = (short)ReadLong(HIGHLIGHTLINES_LINE, DEFAULT_TO_HIGHLIGHTLINES);
		pOptions->sMaxCharsPerLine = (short)ReadLong(MAXCHARSPERLINE_LINE, DEFAULT_TO_MAXCHARSPERLINE);
		pOptions->sScrollSteps = (short)ReadLong(SCROLLSTEPS_LINE, DEFAULT_TO_SCROLLSTEPS);
		pOptions->sScrollTime = (short)ReadLong(SCROLLTIME_LINE, DEFAULT_TO_SCROLLTIME);
		pOptions->sLeftMargin = (short)ReadLong(TEXT_LEFT_MARGIN_LINE, DEFAULT_TO_LEFT_MARGIN);
		pOptions->sRightMargin = (short)ReadLong(TEXT_RIGHT_MARGIN_LINE, DEFAULT_TO_RIGHT_MARGIN);
		pOptions->sTopMargin = (short)ReadLong(TEXT_TOP_MARGIN_LINE, DEFAULT_TO_TOP_MARGIN);
		pOptions->sBottomMargin = (short)ReadLong(TEXT_BOTTOM_MARGIN_LINE, DEFAULT_TO_BOTTOM_MARGIN);
		pOptions->lBackground = ReadLong(TEXTBACKGROUND_LINE, DEFAULT_TO_BACKGROUND);
		pOptions->lForeground = ReadLong(TEXTFOREGROUND_LINE, DEFAULT_TO_FOREGROUND);
		pOptions->lHighlightText = ReadLong(TEXTHIGHLIGHTTEXT_LINE, DEFAULT_TO_HIGHLIGHTTEXT);
		pOptions->lHighlight = ReadLong(TEXTHIGHLIGHT_LINE, DEFAULT_TO_HIGHLIGHT);
		ReadString(TEXTFONT_LINE, szIniStr, sizeof(szIniStr), DEFAULT_TO_TEXTFONT);
		pOptions->strTextFont = szIniStr;
		pOptions->bUseManagerHighlighter = ReadBool(USEMANAGERHIGHLIGHTER_LINE, DEFAULT_TO_USEMANAGERHIGHLIGHTER);
		pOptions->bShowText = ReadBool(TEXT_SHOW_TEXT_LINE, DEFAULT_TO_SHOW_TEXT);
		pOptions->sBulletStyle = (short)ReadLong(TEXT_BULLET_STYLE_LINE, DEFAULT_TO_BULLET_STYLE);
		pOptions->sBulletMargin = (short)ReadLong(TEXT_BULLET_MARGIN_LINE, DEFAULT_TO_BULLET_MARGIN);
	}
	else
	{
		pOptions->bDisableScrollText = DEFAULT_TO_DISABLESCROLLTEXT;
		pOptions->bUseAvgCharWidth = DEFAULT_TO_USEAVGCHAR;
		pOptions->bShowPageLine = DEFAULT_TO_SHOWPAGELINE;
		pOptions->bCombineText = DEFAULT_TO_COMBINETEXT;
		pOptions->bSmoothScroll = DEFAULT_TO_SMOOTHSCROLL;
		pOptions->bCenterVideo = DEFAULT_TO_CENTERVIDEO;
		pOptions->sDisplayLines = DEFAULT_TO_DISPLAYLINES;
		pOptions->sHighlightLines = DEFAULT_TO_HIGHLIGHTLINES;
		pOptions->sMaxCharsPerLine = DEFAULT_TO_MAXCHARSPERLINE;
		pOptions->sScrollSteps = DEFAULT_TO_SCROLLSTEPS;
		pOptions->sScrollTime = DEFAULT_TO_SCROLLTIME;
		pOptions->sLeftMargin = DEFAULT_TO_LEFT_MARGIN;
		pOptions->sRightMargin = DEFAULT_TO_RIGHT_MARGIN;
		pOptions->sTopMargin = DEFAULT_TO_TOP_MARGIN;
		pOptions->sBottomMargin = DEFAULT_TO_BOTTOM_MARGIN;
		pOptions->lBackground = DEFAULT_TO_BACKGROUND;
		pOptions->lForeground = DEFAULT_TO_FOREGROUND;
		pOptions->lHighlightText = DEFAULT_TO_HIGHLIGHTTEXT;
		pOptions->lHighlight = DEFAULT_TO_HIGHLIGHT;
		pOptions->strTextFont = DEFAULT_TO_TEXTFONT;
		pOptions->bUseManagerHighlighter = DEFAULT_TO_USEMANAGERHIGHLIGHTER;
		pOptions->bShowText = DEFAULT_TO_SHOW_TEXT;
		pOptions->sBulletStyle = DEFAULT_TO_BULLET_STYLE;
		pOptions->sBulletMargin = DEFAULT_TO_BULLET_MARGIN;
	}

	return TRUE;
}

//==============================================================================
//
//	Function Name:	CTMIni::ReadUseSnapshots()
//
//	Description:	This function will read the UseSnapshots option from the
//					shared section of the ini file
//
//	Returns:		The UseSnapshots option
//
//	Notes:			None
//
//==============================================================================
BOOL CTMIni::ReadUseSnapshots()
{
	if(bFileFound)
	{
		//	Line up on the shared section
		SetSection(SHARED_SECTION);

		//	Read the option
		return ReadBool(USESNAPSHOTS_LINE, DEFAULT_USESNAPSHOTS);
	}
	else
	{
		return DEFAULT_USESNAPSHOTS;
	}
}

//==============================================================================
//
//	Function Name:	CTMIni::SetApplicationSection()
//
//	Description:	This function will set the ini section to the value 
//					appropriate for the identifier provided by the caller.
//
//	Returns:		TRUE if the identifier is valid
//
//	Notes:			None
//
//==============================================================================
BOOL CTMIni::SetTMSection(int iApplication)
{
	//	What application or control?
	switch(iApplication)
	{
		case TMVIEW_CONTROL:	strSection = TMVIEW_SECTION;
								break;
		case PRESENTATION_APP:	strSection = PRESENTATION_SECTION;
								break;
		default:				return FALSE;
	}

	return TRUE;

}

//==============================================================================
//
//	Function Name:	CTMIni::ReadVideoOptions()
//
//	Description:	This function will read the video options from the ini
//					file
//
//	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMIni::ReadVideoOptions(SVideoOptions* pOptions)
{
	if(bFileFound)
	{
		//	Line up on the correct section
		SetSection(PRESENTATION_SECTION);

		pOptions->bClearMovie = ReadBool(CLEARMOVIE_LINE, DEFAULT_VO_CLEARMOVIE);
		pOptions->bClearPlaylist = ReadBool(CLEARPLAYLIST_LINE, DEFAULT_VO_CLEARPLAYLIST);
		pOptions->bResumeMovie = ReadBool(RESUMEMOVIE_LINE, DEFAULT_VO_RESUMEMOVIE);
		pOptions->bResumePlaylist = ReadBool(RESUMEPLAYLIST_LINE, DEFAULT_VO_RESUMEPLAYLIST);
		pOptions->bScaleAVI = ReadBool(SCALEAVI_LINE, DEFAULT_VO_SCALEAVI);
		pOptions->bClipsAsPlaylists = ReadBool(CLIPSASPLAYLISTS_LINE, DEFAULT_VO_CLIPSASPLAYLISTS);
		pOptions->bRunToEnd = ReadBool(RUNTOEND_LINE, DEFAULT_VO_RUNTOEND);
		pOptions->bSplitScreenDocuments = ReadBool(SPLITSCREENDOCUMENTS_LINE, DEFAULT_VO_SPLITSCREENDOCUMENTS);
		pOptions->bSplitScreenGraphics = ReadBool(SPLITSCREENGRAPHICS_LINE, DEFAULT_VO_SPLITSCREENGRAPHICS);
		pOptions->bSplitScreenPowerPoint = ReadBool(SPLITSCREENPOWERPOINT_LINE, DEFAULT_VO_SPLITSCREENPOWERPOINT);
		pOptions->iVideoSize = (int)ReadLong(VIDEOSIZE_LINE, DEFAULT_VO_VIDEOSIZE);
		pOptions->iVideoPosition = (int)ReadLong(VIDEOPOSITION_LINE, DEFAULT_VO_VIDEOPOSITION);
		pOptions->fMovieStep = (float)ReadDouble(MOVIESTEP_LINE, DEFAULT_VO_MOVIESTEP);
		pOptions->fPlaylistStep = (float)ReadDouble(PLAYLISTSTEP_LINE, DEFAULT_VO_PLAYLISTSTEP);
		pOptions->bClassicLinks = ReadBool(CLASSICLINKS_LINE, DEFAULT_VO_CLASSICLINKS);

		//	The frame rate is a shared option
		pOptions->dFrameRate = ReadFrameRate();
	}
	else
	{
		pOptions->bClearMovie = DEFAULT_VO_CLEARMOVIE;
		pOptions->bClearPlaylist = DEFAULT_VO_CLEARPLAYLIST;
		pOptions->bResumeMovie = DEFAULT_VO_RESUMEMOVIE;
		pOptions->bResumePlaylist = DEFAULT_VO_RESUMEPLAYLIST;
		pOptions->bScaleAVI = DEFAULT_VO_SCALEAVI;
		pOptions->bClipsAsPlaylists = DEFAULT_VO_CLIPSASPLAYLISTS;
		pOptions->bRunToEnd = DEFAULT_VO_RUNTOEND;
		pOptions->bSplitScreenDocuments = DEFAULT_VO_SPLITSCREENDOCUMENTS;
		pOptions->bSplitScreenGraphics = DEFAULT_VO_SPLITSCREENGRAPHICS;
		pOptions->bSplitScreenPowerPoint = DEFAULT_VO_SPLITSCREENPOWERPOINT;
		pOptions->iVideoSize = DEFAULT_VO_VIDEOSIZE;
		pOptions->iVideoPosition = DEFAULT_VO_VIDEOPOSITION;
		pOptions->fMovieStep = DEFAULT_VO_MOVIESTEP;
		pOptions->fPlaylistStep = DEFAULT_VO_PLAYLISTSTEP;
		pOptions->dFrameRate = DEFAULT_FRAMERATE;
		pOptions->bClassicLinks = DEFAULT_VO_CLASSICLINKS;
	}

	return TRUE;
}

//==============================================================================
//
//	Function Name:	CTMIni::SetDirectory()
//
//	Description:	This function allows the caller to set the directory
//					containing the ini file.
//
//	Returns:		None
//
//	Note:			None
//
//==============================================================================
void CTMIni::SetDirectory(LPCSTR lpDirectory)
{
	if(lpDirectory)
		strDirectory = lpDirectory;
}

//==============================================================================
//
//	Function Name:	CTMIni::SetFileSpec()
//
//	Description:	This function allows the caller to set the ini file 
//					specification.
//
//	Returns:		None
//
//	Note:			None
//
//==============================================================================
void CTMIni::SetFileSpec(LPCSTR lpFileSpec)
{
	if(lpFileSpec)
		strFileSpec = lpFileSpec;
}

//==============================================================================
//
// 	Function Name:	CTMIni::SetSection()
//
// 	Description:	This function will set the ini section name.
//
// 	Returns:		None
//
//	Notes:			If bDelete is TRUE, the current section is deleted.
//
//==============================================================================
void CTMIni::SetSection(LPCSTR lpSection, BOOL bDelete)
{
	if(!lpSection)
    	return;
    
    //	Should the current section be deleted?
    if(bDelete)
    	DeleteSection(lpSection);

    //  Save the new section name
	strSection = lpSection;
}

//==============================================================================
//
// Function Name:	CTMIni::WriteBool()
//
// Description:		This function will write a boolean value to the open ini
//					file at the specified line.
//
// Returns:			Nonzero on success. 0 otherwise.     	
//
//	Notes:			None
//
//==============================================================================
BOOL CTMIni::WriteBool(LPCSTR lpLine, BOOL bValue)
{
    CString strString;

	if(!lpLine)
    	return FALSE;

    //	Format the ini string
	if(bValue == TRUE)
		strString = "TRUE";
	else
		strString = "FALSE";

   	if(WritePrivateProfileString(strSection, lpLine, strString, strFileSpec))
	{
		bFileFound = TRUE;
        return TRUE;
    }
    else
    {
    	return FALSE;
    }

}

//==============================================================================
//
// Function Name:	CTMIni::WriteBool()
//
// Description:		This form of the function allows the caller to provide a 
//					line number instead of a name.
//
// Returns:			Nonzero on success. 0 otherwise.     	
//
//	Notes:			None
//
//==============================================================================
BOOL CTMIni::WriteBool(int iLine, BOOL bValue)
{
    char szLine[32];
	wsprintf(szLine, "%d", iLine);
	return WriteBool(szLine, bValue);
}

//==============================================================================
//
//	Function Name:	CTMIni::WriteCaptureOptions()
//
//	Description:	This function will write the screen capture options to the 
//					file
//
//	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMIni::WriteCaptureOptions(SCaptureOptions* pOptions)
{
	//	Line up on the correct section
	SetSection(TMGRAB_SECTION);

	WriteBool(CAPTURE_SILENT_LINE, pOptions->bSilent);
	WriteLong(CAPTURE_HOTKEY_LINE, pOptions->sHotkey);
	WriteLong(CAPTURE_CANCELKEY_LINE, pOptions->sCancelKey);
	WriteLong(CAPTURE_AREA_LINE, pOptions->sArea);
	WriteString(CAPTURE_FILE_PATH, pOptions->sFilePath);
}

//==============================================================================
//
//	Function Name:	CTMIni::WriteCommandLine()
//
//	Description:	This function will write the command line options to the
//					ini file
//
//	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMIni::WriteCommandLine(SCommandLine* pCommandLine)
{
	SetSection(PRESENTATION_COMMANDLINE_SECTION);
	WriteString(COMMANDLINE_BARCODE, pCommandLine->szBarcode);
	WriteString(COMMANDLINE_CASEFOLDER, pCommandLine->szCaseFolder);	WriteString(COMMANDLINE_CASEFOLDER, pCommandLine->szCaseFolder);
	WriteLong(COMMANDLINE_PAGENUMBER, pCommandLine->lPageNumber);
	WriteLong(COMMANDLINE_LINENUMBER, pCommandLine->iLineNumber);

}

//==============================================================================
//
//	Function Name:	CTMIni::WriteDatabaseOptions()
//
//	Description:	This function will write the database options to the ini
//					file
//
//	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMIni::WriteDatabaseOptions(SDatabaseOptions* pOptions)
{
	//	Write the database options
	WriteLastCase(pOptions->strFolder);
	WriteEnableErrors(PRESENTATION_APP, pOptions->bEnableErrors);
	WriteEnablePowerPoint(pOptions->bEnablePowerPoint);
}

//==============================================================================
//
// Function Name:	CTMIni::WriteDouble()
//
// Description:		This form of the function allows the caller to provide a 
//					line number instead of a name.
//
// Returns:			Nonzero on success. 0 otherwise.     	
//
//	Notes:			None
//
//==============================================================================
BOOL CTMIni::WriteDouble(int iLine, double dValue)
{
    char szLine[32];
	wsprintf(szLine, "%d", iLine);
	return WriteDouble(szLine, dValue);
}

//==============================================================================
//
// Function Name:	CTMIni::WriteDouble()
//
// Description:		This function will write a floating point value to the 
//					open ini file at the specified line.
//
// Returns:			Nonzero on success. 0 otherwise.     	
//
//	Notes:			None
//
//==============================================================================
BOOL CTMIni::WriteDouble(LPCSTR lpLine, double dValue)
{
	CString strString;

	if(!lpLine)
    	return FALSE;

    // Format the ini item
	strString.Format("%.10lg", dValue);

   	if(WritePrivateProfileString(strSection, lpLine, strString, strFileSpec))
	{
		bFileFound = TRUE;
        return TRUE;
    }
    else
    {
    	return FALSE;
    }
}

//==============================================================================
//
// Function Name:	CTMIni::WriteLong()
//
// Description:		This function will write a long integer value to the open 
//					file at the specified line.
//
// Returns:			Nonzero on success. 0 otherwise.     	
//
//	Notes:			None
//
//==============================================================================
BOOL CTMIni::WriteLong(LPCSTR lpLine, long lValue)
{
	CString strString;

	if(!lpLine)
    	return FALSE;

    // Format the ini string
	strString.Format("%ld", lValue);

   	if(WritePrivateProfileString(strSection, lpLine, strString, strFileSpec))
	{
		bFileFound = TRUE;
        return TRUE;
    }
    else
    {
    	return FALSE;
    }

}

//==============================================================================
//
//	Function Name:	CTMIni::WriteEnableErrors()
//
//	Description:	This function will write the EnableErrors option for the
//					application specified by the caller.
//
//	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMIni::WriteEnableErrors(int iApplication, BOOL bEnable)
{
	//	Set the appropriate section
	if(!SetTMSection(iApplication))
		return;

	WriteBool(ENABLEERRORS_LINE, bEnable);
}

//==============================================================================
//
//	Function Name:	CTMIni::WriteLastCase()
//
//	Description:	This function will write the case information to the 
//					ini file.
//
//	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMIni::WriteLastCase(LPCSTR lpFolder)
{
	ASSERT(lpFolder);

	//	Set the appropriate section
	SetSection(SHARED_SECTION);

	//	Write the string to the file
	WriteString(LASTCASE_LINE, lpFolder);
}

//==============================================================================
//
// Function Name:	CTMIni::WriteLong()
//
// Description:		This form of the function allows the caller to provide a
//					line number instead of a name.
//
// Returns:			Nonzero on success. 0 otherwise.     	
//
//	Notes:			None
//
//==============================================================================
BOOL CTMIni::WriteLong(int iLine, long lValue)
{
	char szLine[32];
	wsprintf(szLine, "%d", iLine);
	return WriteLong(szLine, lValue);
}

//==============================================================================
//
// Function Name:	CTMIni::WriteString()
//
// Description:		This function will write a character string to the open ini
//					file at the specified line.
//
// Returns:			Nonzero on success. 0 otherwise.     	
//
//	Notes:			None
//
//==============================================================================
BOOL CTMIni::WriteString(LPCSTR lpLine, LPCSTR lpString)
{
	if(!lpLine || !lpString)
    	return FALSE;

   	if(WritePrivateProfileString(strSection, lpLine, lpString, strFileSpec))
	{
		bFileFound = TRUE;
        return TRUE;
    }
    else
    {
    	return FALSE;
    }

}

//==============================================================================
//
//	Function Name:	CTMIni::WriteRingtailOptions()
//
//	Description:	This function will write the ringtail options to the file
//
//	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMIni::WriteRingtailOptions(SRingtailOptions* pOptions)
{
	//	Line up on the correct section
	SetSection(RINGTAIL_SECTION);

	WriteBool(RT_SHOWREDACTIONS_LINE, pOptions->bShowRedactions);
	WriteLong(RT_REDACTTRANSPARENCY_LINE, pOptions->sRedactTransparency);
	WriteLong(RT_REDACTLABELSIZE_LINE, pOptions->sLabelFontSize);
	WriteLong(RT_REDACTCOLOR_LINE, pOptions->lRedactColor);
	WriteLong(RT_REDACTLABELCOLOR_LINE, pOptions->lRedactLabelColor);
	WriteString(RT_REDACTLABELFONT_LINE, pOptions->strLabelFontName);
}

//==============================================================================
//
// Function Name:	CTMIni::WriteString()
//
// Description:		This form of the function allows the caller to specify a
//					line number instead of name.
//
// Returns:			Nonzero on success. 0 otherwise.     	
//
//	Notes:			None
//
//==============================================================================
BOOL CTMIni::WriteString(int iLine, LPCSTR lpString)
{
	char szLine[32];
	wsprintf(szLine, "%d", iLine);
	return WriteString(szLine, lpString);
}

//==============================================================================
//
//	Function Name:	CTMIni::WriteSystemOptions()
//
//	Description:	This function will write the system options to the file
//
//	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMIni::WriteSystemOptions(SSystemOptions* pOptions)
{
	//	Line up on the correct section
	SetSection(PRESENTATION_SECTION);

	WriteLong(IMAGE_SECONDARY_LINE, pOptions->iImageSecondary);
	WriteLong(ANIMATION_SECONDARY_LINE, pOptions->iAnimationSecondary);
	WriteLong(POWERPOINT_SECONDARY_LINE, pOptions->iPowerPointSecondary);
	WriteLong(PLAYLIST_SECONDARY_LINE, pOptions->iPlaylistSecondary);
	WriteLong(CUSTOMSHOW_SECONDARY_LINE, pOptions->iCustomShowSecondary);
	WriteLong(TREATMENT_TERTIARY_LINE, pOptions->iTreatmentTertiary);
	WriteBool(OPTIMIZEVIDEO_LINE, pOptions->bOptimizeVideo);
	WriteBool(DUALMONITORS_LINE, pOptions->bDualMonitors);
	WriteBool(OPTIMIZETABLET_LINE, pOptions->bOptimizeTablet);
	WriteBool(ENABLEBARCODEKEYSTROKES_LINE, pOptions->bEnableBarcodeKeystrokes);
	
	// setting small button size if optimize tablet is unchecked
	if(!pOptions->bOptimizeTablet)
	{
		SetSection(TMBARS_DOCUMENT_SECTION,0);
		WriteLong(TMTB_INI_SIZE_LINE,TMTB_SMALLBUTTONS);

		SetSection(TMBARS_GRAPHIC_SECTION,0);
		WriteLong(TMTB_INI_SIZE_LINE,TMTB_SMALLBUTTONS);

		SetSection(TMBARS_PLAYLIST_SECTION,0);
		WriteLong(TMTB_INI_SIZE_LINE,TMTB_SMALLBUTTONS);

		SetSection(TMBARS_LINK_SECTION,0);
		WriteLong(TMTB_INI_SIZE_LINE,TMTB_SMALLBUTTONS);

		SetSection(TMBARS_MOVIE_SECTION,0);
		WriteLong(TMTB_INI_SIZE_LINE,TMTB_SMALLBUTTONS);

		SetSection(TMBARS_POWERPOINT_SECTION,0);
		WriteLong(TMTB_INI_SIZE_LINE,TMTB_SMALLBUTTONS);
	}
	
	

}

//==============================================================================
//
//	Function Name:	CTMIni::WriteTextOptions()
//
//	Description:	This function will write the text options to the file
//
//	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMIni::WriteTextOptions(STextOptions* pOptions)
{
	//	Line up on the correct section
	SetSection(PRESENTATION_SECTION);

	WriteBool(DISABLESCROLLTEXT_LINE, pOptions->bDisableScrollText);
	WriteBool(USEAVGCHAR_LINE, pOptions->bUseAvgCharWidth);
	WriteBool(SHOWPAGELINE_LINE, pOptions->bShowPageLine);
	WriteBool(COMBINETEXT_LINE, pOptions->bCombineText);
	WriteBool(SMOOTHSCROLL_LINE, pOptions->bSmoothScroll);
	WriteBool(CENTERVIDEO_LINE, pOptions->bCenterVideo);
	WriteLong(DISPLAYLINES_LINE, pOptions->sDisplayLines);
	WriteLong(HIGHLIGHTLINES_LINE, pOptions->sHighlightLines);
	WriteLong(MAXCHARSPERLINE_LINE, pOptions->sMaxCharsPerLine);
	WriteLong(SCROLLSTEPS_LINE, pOptions->sScrollSteps);
	WriteLong(SCROLLTIME_LINE, pOptions->sScrollTime);
	WriteLong(TEXT_LEFT_MARGIN_LINE, pOptions->sLeftMargin);
	WriteLong(TEXT_RIGHT_MARGIN_LINE, pOptions->sRightMargin);
	WriteLong(TEXT_TOP_MARGIN_LINE, pOptions->sTopMargin);
	WriteLong(TEXT_BOTTOM_MARGIN_LINE, pOptions->sBottomMargin);
	WriteLong(TEXTBACKGROUND_LINE, pOptions->lBackground);
	WriteLong(TEXTFOREGROUND_LINE, pOptions->lForeground);
	WriteLong(TEXTHIGHLIGHTTEXT_LINE, pOptions->lHighlightText);
	WriteLong(TEXTHIGHLIGHT_LINE, pOptions->lHighlight);
	WriteString(TEXTFONT_LINE, pOptions->strTextFont);
	WriteBool(USEMANAGERHIGHLIGHTER_LINE, pOptions->bUseManagerHighlighter);
	WriteBool(TEXT_SHOW_TEXT_LINE, pOptions->bShowText);
	WriteLong(TEXT_BULLET_STYLE_LINE, pOptions->sBulletStyle);
	WriteLong(TEXT_BULLET_MARGIN_LINE, pOptions->sBulletMargin);
}

//==============================================================================
//
//	Function Name:	CTMIni::WriteUseSnapshots()
//
//	Description:	This function will write the UseCaseDrive option to the
//					shared section of the ini file
//
//	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMIni::WriteUseSnapshots(BOOL bDIBSnapshots)
{
	//	Line up on the shared section
	SetSection(SHARED_SECTION);

	//	Write the option
	WriteBool(USESNAPSHOTS_LINE, bDIBSnapshots);
}

//==============================================================================
//
//	Function Name:	CTMIni::WriteEnablePowerPoint()
//
//	Description:	This function will write the EnablePowerPoint flag to the
//					shared section of the ini file
//
//	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMIni::WriteEnablePowerPoint(BOOL bEnable)
{
	//	Line up on the shared section
	SetSection(SHARED_SECTION);

	//	Write the new value
	WriteBool(ENABLEPOWERPOINT_LINE, bEnable);
}

//==============================================================================
//
//	Function Name:	CTMIni::WriteFrameRate()
//
//	Description:	This function will write the FrameRate value to the shared
//					section of the ini file
//
//	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMIni::WriteFrameRate(double dFrameRate)
{
	//	Line up on the shared section
	SetSection(SHARED_SECTION);

	//	Write the new value
	WriteDouble(FRAMERATE_LINE, dFrameRate);
}

//==============================================================================
//
//	Function Name:	CTMIni::WriteGraphicsOptions()
//
//	Description:	This function will write the graphics options to the file
//
//	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMIni::WriteGraphicsOptions(SGraphicsOptions* pOptions)
{
	//	Line up on the correct section
	SetSection(PRESENTATION_SECTION);

	WriteLong(ANNCOLOR_LINE, pOptions->sAnnColor);
	WriteLong(ANNTHICKNESS_LINE, pOptions->sAnnThickness);
	WriteLong(HIGHLIGHTCOLOR_LINE, pOptions->sHighlightColor);
	WriteLong(REDACTCOLOR_LINE, pOptions->sRedactColor);
	WriteLong(MAXZOOM_LINE, pOptions->sMaxZoom);
	WriteLong(CALLOUTCOLOR_LINE, pOptions->sCalloutColor);
	WriteLong(CALLHANDLECOLOR_LINE, pOptions->sCalloutHandleColor);
	WriteLong(CALLFRAMECOLOR_LINE, pOptions->sCalloutFrameColor);
	WriteLong(CALLFRAMETHICK_LINE, pOptions->sCalloutFrameThickness);
	WriteLong(BITONALSCALING_LINE, pOptions->sBitonalScaling);
	WriteLong(DRAWTOOL_LINE, pOptions->sAnnTool);
	WriteLong(ANNFONTSIZE_LINE, pOptions->sAnnFontSize);
	WriteLong(LIGHTPENCOLOR_LINE, pOptions->sLightPenColor);
	WriteLong(LIGHTPENSIZE_LINE, pOptions->sLightPenSize);
	WriteLong(USER_SPLITFRAMECOLOR_LINE, pOptions->sUserSplitFrameColor);
	WriteLong(ZAP_SPLITFRAMECOLOR_LINE, pOptions->sZapSplitFrameColor);
	WriteLong(CALLOUTSHADEGRAYSCALE_LINE, pOptions->sCalloutShadeGrayscale);
	WriteBool(SHADEONCALLOUT_LINE, pOptions->bShadeOnCallout);
	WriteBool(LIGHTPENENABLED_LINE, pOptions->bLightPenEnabled);
	WriteBool(ANNFONTBOLD_LINE, pOptions->bAnnFontBold);
	WriteBool(ANNFONTSTRIKETHROUGH_LINE, pOptions->bAnnFontStrikeThrough);
	WriteBool(ANNFONTUNDERLINE_LINE, pOptions->bAnnFontUnderline);
	WriteBool(SCALEDOCS_LINE, pOptions->bScaleDocuments);
	WriteBool(SCALEGRAPHICS_LINE, pOptions->bScaleGraphics);
	WriteBool(RESIZABLECALLOUTS_LINE, pOptions->bResizableCallouts);
	WriteBool(PANCALLOUTS_LINE, pOptions->bPanCallouts);
	WriteBool(ZOOMCALLOUTS_LINE, pOptions->bZoomCallouts);
	WriteString(ANNFONTNAME_LINE, pOptions->strAnnFontName);
}

//==============================================================================
//
//	Function Name:	CTMIni::WriteVideoDrive()
//
//	Description:	This function is used by all TrialMax II controls and 
//					applications to store the video drive information.
//
//	Returns:		None
//
//	Notes:			Trialmax II applications and controls permit the user to
//					define a default drive where all MPEG files can be found. If
//					enabled, the default drive is used instead of the drive
//					specifications stored in the database.
//
//==============================================================================
void CTMIni::WriteVideoDrive(BOOL bUseDrive, LPCSTR lpDrive)
{
	ASSERT(lpDrive);

	//	Set the appropriate section
	SetSection(SHARED_SECTION);

	//	Write the video drive specification
	WriteString(VIDEODRIVE_LINE, lpDrive);

	//	Write the enable flag
	WriteBool(USEVIDEODRIVE_LINE, bUseDrive);

}

//==============================================================================
//
//	Function Name:	CTMIni::WriteVideoOptions()
//
//	Description:	This function will write the video options to the file
//
//	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMIni::WriteVideoOptions(SVideoOptions* pOptions)
{
	//	Line up on the correct section
	SetSection(PRESENTATION_SECTION);

	WriteBool(CLEARMOVIE_LINE, pOptions->bClearMovie);
	WriteBool(CLEARPLAYLIST_LINE, pOptions->bClearPlaylist);
	WriteBool(RESUMEMOVIE_LINE, pOptions->bResumeMovie);
	WriteBool(RESUMEPLAYLIST_LINE, pOptions->bResumePlaylist);
	WriteBool(SCALEAVI_LINE, pOptions->bScaleAVI);
	WriteBool(CLIPSASPLAYLISTS_LINE, pOptions->bClipsAsPlaylists);
	WriteBool(RUNTOEND_LINE, pOptions->bRunToEnd);
	WriteBool(SPLITSCREENDOCUMENTS_LINE, pOptions->bSplitScreenDocuments);
	WriteBool(SPLITSCREENGRAPHICS_LINE, pOptions->bSplitScreenGraphics);
	WriteBool(SPLITSCREENPOWERPOINT_LINE, pOptions->bSplitScreenPowerPoint);
	WriteLong(VIDEOSIZE_LINE, pOptions->iVideoSize);
	WriteLong(VIDEOPOSITION_LINE, pOptions->iVideoPosition);
	WriteDouble(MOVIESTEP_LINE, pOptions->fMovieStep);
	WriteDouble(PLAYLISTSTEP_LINE, pOptions->fPlaylistStep);
	WriteBool(CLASSICLINKS_LINE, pOptions->bClassicLinks);

	//	The frame rate is a shared option
	WriteFrameRate(pOptions->dFrameRate);
}



