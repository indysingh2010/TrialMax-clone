//==============================================================================
//
// File Name:	tmini.h
//
// Description:	This file contains the declaration of the CTMIni class. This
//				class is responsible for management of TrialMax II ini files.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	08-30-97	1.00		Original Release
//==============================================================================
#if !defined(__TMINI_H__)
#define __TMINI_H__

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Maximum buffer lengths
#define MAXLEN_CASENAME						128
#define MAXLEN_CASEFOLDER					256
#define MAXLEN_BARCODE						64

//	Default TrialMax II ini filenames
#define DEFAULT_TMAXINI						"Fti.ini"
#define DEFAULT_COMMANDLINE_FILE			"Tmaxcmd.ini"

//	Application & control identifiers
#define TMVIEW_CONTROL						0
#define PRESENTATION_APP					1

//	Section identifiers
#define SHARED_SECTION						"SHARED"
#define MANAGER_SECTION						"MANAGER"
#define VIEWER_SECTION						"VIEWER"
#define TMCASE_SECTION						"TMCASE"
#define TMVIEW_SECTION						"TMVIEW"
#define PRESENTATION_SECTION				"PRESENTATION"
#define PRESENTATION_CASES_SECTION			"RECENT CASES"
#define PRESENTATION_COMMANDLINE_SECTION	"COMMAND LINE"
#define TMGRAB_SECTION						"TMGRAB"
#define RINGTAIL_SECTION					"RINGTAIL"
#define TMMOVIE_FILTERS_SECTION				"TMMOVIE FILTERS"

//	Secondary identifier usage
#define SECONDARY_AS_ID						0
#define SECONDARY_AS_ORDER					1
#define SECONDARY_AS_SLIDEINDEX				2

//	Tertiary identifier usage
#define TERTIARY_AS_ID						0
#define TERTIARY_AS_ORDER					1

//	Shared line identifiers
#define LASTCASE_LINE						"LastCase"
#define VIDEODRIVE_LINE						"VideoDrive"
#define USEVIDEODRIVE_LINE					"UseVideoDrive"
#define ENABLEERRORS_LINE					"EnableErrors"
#define ENABLEPOWERPOINT_LINE				"EnablePowerPoint"
#define USESNAPSHOTS_LINE					"UseSnapshots"
#define FRAMERATE_LINE						"FrameRate"
#define AUTOBARCODEDESIGNATIONS_LINE		"AutoBarcodeDesignations"
#define ALLOWAUTO_CASES_LINE				"AllowAutoCases"
#define ALLOWAUTO_PARTIES_LINE				"AllowAutoParties"
#define ALLOWAUTO_DEPONENTS_LINE			"AllowAutoDeponents"
#define ALLOWAUTO_DEPOSITIONS_LINE			"AllowAutoDepositions"
#define ALLOWAUTO_PLAYLISTS_LINE			"AllowAutoPlaylists"
#define ALLOWAUTO_TUNE_LINE					"AllowAutoTune"
#define ALLOWAUTO_VIEW_LINE					"AllowAutoView"

//	Command line identifiers
#define COMMANDLINE_BARCODE					"Barcode"
#define COMMANDLINE_CASEFOLDER				"CaseFolder"
#define COMMANDLINE_PAGENUMBER				"PageNumber"
#define COMMANDLINE_LINENUMBER				"LineNumber"

//	Graphics options lines
#define SCALEGRAPHICS_LINE					"ScaleGraphics"
#define SCALEDOCS_LINE						"ScaleDocuments"
#define ANNTHICKNESS_LINE					"AnnThickness"
#define MAXZOOM_LINE						"MaxZoom"
#define HIGHLIGHTCOLOR_LINE					"HighlightColor"
#define ANNCOLOR_LINE						"AnnColor"
#define REDACTCOLOR_LINE					"RedactColor"
#define CALLOUTCOLOR_LINE					"CalloutColor"
#define CALLHANDLECOLOR_LINE				"CallHandleColor"
#define CALLFRAMECOLOR_LINE					"CallFrameColor"
#define CALLFRAMETHICK_LINE					"CallFrameThick"
#define DRAWTOOL_LINE						"DrawTool"
#define ANNFONTNAME_LINE					"AnnFontName"
#define ANNFONTSIZE_LINE					"AnnFontSize"
#define ANNFONTSTRIKETHROUGH_LINE			"AnnFontStrikeThrough"
#define ANNFONTBOLD_LINE					"AnnFontBold"
#define ANNFONTUNDERLINE_LINE				"AnnFontUnderline"
#define LIGHTPENENABLED_LINE				"LightPenEnabled"
#define LIGHTPENCOLOR_LINE					"LightPenColor"
#define LIGHTPENSIZE_LINE					"LightPenSize"
#define ZOOMTORECT_LINE						"ZoomToRect"
#define BITONALSCALING_LINE					"BitonalScaling"
#define RESIZABLECALLOUTS_LINE				"ResizableCallouts"
#define PANCALLOUTS_LINE					"PanCallouts"
#define ZOOMCALLOUTS_LINE					"ZoomCallouts"
#define SHADEONCALLOUT_LINE					"ShadeOnCallout"
#define CALLOUTSHADEGRAYSCALE_LINE			"CalloutShadeGrayscale"
#define USER_SPLITFRAMECOLOR_LINE			"UserSplitFrameColor"
#define ZAP_SPLITFRAMECOLOR_LINE			"ZapSplitFrameColor"

//	Video options lines
#define PLAYLISTSTEP_LINE					"PlaylistStep"
#define RESUMEPLAYLIST_LINE					"ResumePlaylist"
#define CLEARPLAYLIST_LINE					"ClearPlaylist"
#define MOVIESTEP_LINE						"MovieStep"
#define RESUMEMOVIE_LINE					"ResumeMovie"
#define CLEARMOVIE_LINE						"ClearMovie"
#define SCALEAVI_LINE						"ScaleAVI"
#define RUNTOEND_LINE						"RunToEnd"
#define CLIPSASPLAYLISTS_LINE				"ClipsAsPlaylists"
#define VIDEOSIZE_LINE						"VideoSize"
#define VIDEOPOSITION_LINE					"VideoPosition"
#define SPLITSCREENDOCUMENTS_LINE			"SplitScreenDocuments"
#define SPLITSCREENGRAPHICS_LINE			"SplitScreenGraphics"
#define SPLITSCREENPOWERPOINT_LINE			"SplitScreenPowerPoint"
#define CLASSICLINKS_LINE					"ClassicLinks"

//	Text options lines
#define DISABLESCROLLTEXT_LINE				"DisableScrollText"
#define USEAVGCHAR_LINE						"UseAvgChar"
#define SHOWPAGELINE_LINE					"ShowPageLine"
#define MAXCHARSPERLINE_LINE				"MaxCharsPerLine"
#define DISPLAYLINES_LINE					"DisplayLines"
#define HIGHLIGHTLINES_LINE					"HighlightLines"
#define TEXTFONT_LINE						"TextFont"
#define TEXTBACKGROUND_LINE					"TextBackColor"
#define TEXTHIGHLIGHT_LINE					"TextHighColor"
#define TEXTFOREGROUND_LINE					"TextForeColor"
#define TEXTHIGHLIGHTTEXT_LINE				"TextHighTextColor"
#define CENTERVIDEO_LINE					"CenterVideo"
#define COMBINETEXT_LINE					"MergeText"
#define SMOOTHSCROLL_LINE					"SmoothScroll"
#define SCROLLSTEPS_LINE					"ScrollSteps"
#define SCROLLTIME_LINE						"ScrollTime"
#define USEMANAGERHIGHLIGHTER_LINE			"UseManagerHighlighter"
#define TEXT_LEFT_MARGIN_LINE				"LeftMargin"
#define TEXT_RIGHT_MARGIN_LINE				"RightMargin"
#define TEXT_TOP_MARGIN_LINE				"TopMargin"
#define TEXT_BOTTOM_MARGIN_LINE				"BottomMargin"
#define TEXT_SHOW_TEXT_LINE					"ShowText"
#define TEXT_BULLET_MARGIN_LINE				"BulletMargin"
#define TEXT_BULLET_STYLE_LINE				"BulletStyle"

//	Ringtail options lines
#define RT_SHOWREDACTIONS_LINE				"ShowRedactions"
#define RT_REDACTCOLOR_LINE					"RedactColor"
#define RT_REDACTLABELCOLOR_LINE			"RedactLabelColor"
#define RT_REDACTLABELSIZE_LINE				"RedactLabelSize"
#define RT_REDACTLABELFONT_LINE				"RedactLabelFont"
#define RT_REDACTTRANSPARENCY_LINE			"RedactTransparency"

//	System options lines
#define IMAGE_SECONDARY_LINE				"ImageSecondary"
#define ANIMATION_SECONDARY_LINE			"AnimationSecondary"
#define PLAYLIST_SECONDARY_LINE				"PlaylistSecondary"
#define POWERPOINT_SECONDARY_LINE			"PowerPointSecondary"
#define CUSTOMSHOW_SECONDARY_LINE			"CustomShowSecondary"
#define TREATMENT_TERTIARY_LINE				"TreatmentTertiary"
#define OPTIMIZEVIDEO_LINE					"OptimizeVideo"
#define DUALMONITORS_LINE					"DualMonitors"
#define OPTIMIZETABLET_LINE					"OptimizeTablet"
#define ENABLEBARCODEKEYSTROKES_LINE		"EnableBarcodeKeystrokes"

//	Capture options lines
#define CAPTURE_HOTKEY_LINE					"Hotkey"
#define CAPTURE_CANCELKEY_LINE				"CancelKey"
#define CAPTURE_SILENT_LINE					"Silent"
#define CAPTURE_AREA_LINE					"Area"

//	Default shared values
#define DEFAULT_VIDEODRIVE					"D"
#define DEFAULT_USEVIDEODRIVE				FALSE
#define DEFAULT_USECASEDRIVE				FALSE
#define DEFAULT_ENABLEERRORS				TRUE
#define DEFAULT_USESNAPSHOTS				FALSE
#define DEFAULT_FRAMERATE					29.97
#define DEFAULT_AUTOBARCODEDESIGNATIONS		FALSE
#define DEFAULT_ENABLEPOWERPOINT			FALSE
#define DEFAULT_CASEFOLDER					""
#define DEFAULT_BARCODE						""
#define DEFAULT_PAGENUMBER					0
#define DEFAULT_LINENUMBER					0

//	Default graphics options
#define DEFAULT_GO_SCALEGRAPHICS			TRUE
#define DEFAULT_GO_SCALEDOCS				TRUE
#define DEFAULT_GO_ANNTOOL					0
#define DEFAULT_GO_ANNTHICKNESS				1
#define DEFAULT_GO_ANNCOLOR					1
#define DEFAULT_GO_REDACTCOLOR				1
#define DEFAULT_GO_HIGHLIGHTCOLOR			4
#define DEFAULT_GO_CALLOUTCOLOR				6
#define DEFAULT_GO_MAXZOOM					5
#define DEFAULT_GO_CALLFRAMETHICKNESS		5
#define DEFAULT_GO_CALLFRAMECOLOR			3
#define DEFAULT_GO_BITONALSCALING			0
#define DEFAULT_GO_ZOOMTORECT				FALSE
#define DEFAULT_GO_ANNFONTNAME				"Arial"
#define DEFAULT_GO_ANNFONTSIZE				12
#define DEFAULT_GO_ANNFONTBOLD				FALSE
#define DEFAULT_GO_ANNFONTUNDERLINE			FALSE
#define DEFAULT_GO_ANNFONTSTRIKETHROUGH		FALSE
#define DEFAULT_GO_LIGHTPENENABLED			FALSE
#define DEFAULT_GO_LIGHTPENCOLOR			7
#define DEFAULT_GO_LIGHTPENSIZE				5
#define DEFAULT_GO_CALLHANDLECOLOR			3
#define DEFAULT_GO_RESIZABLECALLOUTS		FALSE
#define DEFAULT_GO_PANCALLOUTS				FALSE
#define DEFAULT_GO_ZOOMCALLOUTS				FALSE
#define DEFAULT_GO_SHADEONCALLOUT			FALSE
#define DEFAULT_GO_CALLOUTSHADEGRAYSCALE	0xc0
#define DEFAULT_GO_USER_SPLITFRAMECOLOR		7
#define DEFAULT_GO_ZAP_SPLITFRAMECOLOR		7

//	Default video options
#define DEFAULT_VO_PLAYLISTSTEP				10.0f
#define DEFAULT_VO_RESUMEPLAYLIST			FALSE
#define DEFAULT_VO_CLEARPLAYLIST			TRUE
#define DEFAULT_VO_MOVIESTEP				5.0f
#define DEFAULT_VO_RESUMEMOVIE				FALSE
#define DEFAULT_VO_CLEARMOVIE				TRUE
#define DEFAULT_VO_SCALEAVI					TRUE
#define DEFAULT_VO_SCALEPLAYLISTS			TRUE
#define DEFAULT_VO_CLIPSASPLAYLISTS			FALSE
#define DEFAULT_VO_RUNTOEND					TRUE
#define DEFAULT_VO_VIDEOSIZE				0
#define DEFAULT_VO_VIDEOPOSITION			0
#define DEFAULT_VO_SPLITSCREENDOCUMENTS		TRUE
#define DEFAULT_VO_SPLITSCREENGRAPHICS		FALSE
#define DEFAULT_VO_SPLITSCREENPOWERPOINT	TRUE
#define DEFAULT_VO_CLASSICLINKS				FALSE

//	Default text options
#define DEFAULT_TO_DISABLESCROLLTEXT		FALSE
#define DEFAULT_TO_USEAVGCHAR				TRUE
#define DEFAULT_TO_SHOWPAGELINE				TRUE
#define DEFAULT_TO_MAXCHARSPERLINE			60
#define DEFAULT_TO_DISPLAYLINES				5
#define DEFAULT_TO_HIGHLIGHTLINES			1
#define DEFAULT_TO_CENTERVIDEO				TRUE
#define DEFAULT_TO_COMBINETEXT				TRUE
#define DEFAULT_TO_TEXTFONT					"Arial"
#define DEFAULT_TO_BACKGROUND				8388608L
#define DEFAULT_TO_FOREGROUND				12632256L
#define DEFAULT_TO_HIGHLIGHT				16777215L
#define DEFAULT_TO_HIGHLIGHTTEXT			8388608L
#define DEFAULT_TO_SMOOTHSCROLL				TRUE
#define DEFAULT_TO_SCROLLSTEPS				10
#define DEFAULT_TO_SCROLLTIME				200
#define DEFAULT_TO_USEMANAGERHIGHLIGHTER	FALSE
#define DEFAULT_TO_LEFT_MARGIN				0
#define DEFAULT_TO_RIGHT_MARGIN				0
#define DEFAULT_TO_TOP_MARGIN				0
#define DEFAULT_TO_BOTTOM_MARGIN			0
#define DEFAULT_TO_SHOW_TEXT				TRUE
#define DEFAULT_TO_BULLET_MARGIN			5
#define DEFAULT_TO_BULLET_STYLE				0

#define TEXT_SHOWTEXT_LINE					"ShowText"
#define TEXT_BULLETMARGIN_LINE				"BulletMargin"
#define TEXT_BULLET_STYLE_LINE				"BulletStyle"
//	Default system options
#define DEFAULT_SO_IMAGESECONDARY			SECONDARY_AS_ID
#define DEFAULT_SO_ANIMATIONSECONDARY		SECONDARY_AS_ID
#define DEFAULT_SO_POWERPOINTSECONDARY		SECONDARY_AS_ID
#define DEFAULT_SO_PLAYLISTSECONDARY		SECONDARY_AS_ID
#define DEFAULT_SO_CUSTOMSHOWSECONDARY		SECONDARY_AS_ID
#define DEFAULT_SO_TREATMENTTERTIARY		TERTIARY_AS_ID
#define DEFAULT_SO_OPTIMIZEVIDEO			TRUE
#define DEFAULT_SO_DUALMONITORS				FALSE
#define DEFAULT_SO_OPTIMIZETABLET			FALSE
#define DEFAULT_SO_ENABLEBARCODEKEYSTROKES	FALSE


#define DEFAULT_CO_HOTKEY					0
#define DEFAULT_CO_CANCELKEY				27
#define DEFAULT_CO_SILENT					TRUE
#define DEFAULT_CO_AREA						2

//	Default Ringtail options
#define DEFAULT_RT_SHOWREDACTIONS			TRUE
#define DEFAULT_RT_REDACTCOLOR				0L
#define DEFAULT_RT_REDACTLABELCOLOR			16777215L
#define DEFAULT_RT_REDACTLABELSIZE			8
#define DEFAULT_RT_REDACTLABELFONT			"Arial"
#define DEFAULT_RT_REDACTTRANSPARENCY		0

//	Toolbar configuration page ini sections
#define TMBARS_DOCUMENT_SECTION			"DOCUMENT TOOLBAR"
#define TMBARS_GRAPHIC_SECTION			"GRAPHIC TOOLBAR"
#define TMBARS_PLAYLIST_SECTION			"PLAYLIST TOOLBAR"
#define TMBARS_LINK_SECTION				"LINK TOOLBAR"
#define TMBARS_MOVIE_SECTION			"MOVIE TOOLBAR"
#define TMBARS_POWERPOINT_SECTION		"POWERPOINT TOOLBAR"
#define TMBARS_TEMPLATES_SECTION		"TOOLBAR TEMPLATES"


#define TMTB_INI_SIZE_LINE				"Size"

//	TrialMax toolbar button sizes
#define TMTB_SMALLBUTTONS				0
#define TMTB_MEDIUMBUTTONS				1
#define TMTB_LARGEBUTTONS				2


//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	Forward declarations
class CTMIni;

//	This parameter structure is used to transfer command line options between
//	application instances
typedef struct
{
	long	lPageNumber;
	int		iLineNumber;
	char	szBarcode[MAXLEN_BARCODE];
	char	szCaseFolder[MAXLEN_CASEFOLDER];

}SCommandLine;

//	This structure is used to read and write database options
typedef struct
{
	CString		strFolder;
	BOOL		bEnableErrors;
	BOOL		bEnablePowerPoint;

}SDatabaseOptions;

//	This structure is used to read and write graphics options
typedef struct
{
	short		sAnnColor;
	short		sAnnThickness;
	short		sHighlightColor;
	short		sRedactColor;
	short		sMaxZoom;
	short		sCalloutColor;
	short		sCalloutHandleColor;
	short		sCalloutFrameColor;
	short		sCalloutFrameThickness;
	short		sBitonalScaling;
	short		sAnnTool;
	short		sAnnFontSize;
	short		sLightPenColor;
	short		sLightPenSize;
	short		sCalloutShadeGrayscale;
	short		sUserSplitFrameColor;
	short		sZapSplitFrameColor;
	BOOL		bShadeOnCallout;
	BOOL		bLightPenEnabled;
	BOOL		bAnnFontBold;
	BOOL		bAnnFontStrikeThrough;
	BOOL		bAnnFontUnderline;
	BOOL		bScaleDocuments;
	BOOL		bScaleGraphics;
	BOOL		bResizableCallouts;
	BOOL		bPanCallouts;
	BOOL		bZoomCallouts;
	CString		strAnnFontName;
}SGraphicsOptions;

//	This structure is used to read and write video options
typedef struct
{
	BOOL		bClearMovie;
	BOOL		bClearPlaylist;
	BOOL		bResumeMovie;
	BOOL		bResumePlaylist;
	BOOL		bScaleAVI;
	BOOL		bClipsAsPlaylists;
	BOOL		bRunToEnd;
	BOOL		bSplitScreenDocuments;
	BOOL		bSplitScreenGraphics;
	BOOL		bSplitScreenPowerPoint;
	BOOL		bClassicLinks;
	float		fMovieStep;
	float		fPlaylistStep;
	int			iVideoSize;
	int			iVideoPosition;
	double		dFrameRate;

}SVideoOptions;

//	This structure is used to transfer scrolling text options
typedef struct
{
	BOOL		bDisableScrollText;
	BOOL		bUseAvgCharWidth;
	BOOL		bShowPageLine;
	BOOL		bCombineText;
	BOOL		bSmoothScroll;
	BOOL		bCenterVideo;
	BOOL		bUseManagerHighlighter;
	BOOL		bShowText;
	short		sDisplayLines;
	short		sHighlightLines;
	short		sMaxCharsPerLine;
	short		sScrollSteps;
	short		sScrollTime;
	short		sLeftMargin;
	short		sRightMargin;
	short		sTopMargin;
	short		sBottomMargin;
	short		sBulletMargin;
	short		sBulletStyle;
	long		lBackground;
	long		lForeground;
	long		lHighlight;
	long		lHighlightText;
	CString		strTextFont;

}STextOptions;

//	This structure is used to transfer system options
typedef struct
{
	int			iImageSecondary;
	int			iAnimationSecondary;
	int			iPowerPointSecondary;
	int			iPlaylistSecondary;
	int			iCustomShowSecondary;
	int			iTreatmentTertiary;
	BOOL		bOptimizeVideo;
	BOOL		bDualMonitors;
	BOOL		bOptimizeTablet;
	BOOL		bEnableBarcodeKeystrokes;

}SSystemOptions;

//	This structure is used to transfer capture options
typedef struct
{
	BOOL		bSilent;
	short		sHotkey;
	short		sCancelKey;
	short		sArea;
}SCaptureOptions;

//	This structure is used to transfer RingTail display options
typedef struct
{
	BOOL		bShowRedactions;
	short		sRedactTransparency;
	short		sLabelFontSize;
	long		lRedactColor;
	long		lRedactLabelColor;
	CString		strLabelFontName;

}SRingtailOptions;

//	This class provides basic ini management services as well as specific 
//	services related to management of TrialMax II initialization files
class CTMIni : public CObject
{
    private:

    public:

        CString		strDirectory;
		CString		strFileSpec;
		CString		strSection;
        BOOL        bFileFound;

                    CTMIni(LPCSTR lpFilename = 0, LPCSTR lpSection = 0);
                   ~CTMIni();

        BOOL        Open(LPCSTR lpFilename, LPCSTR lpSection = 0);
        void        Close(BOOL bDelete = FALSE);

        //	TrialMax II application routines
		BOOL		ReadEnableErrors(int iApplication);
		BOOL		ReadEnablePowerPoint();
		BOOL		SetTMSection(int iApplication);
		BOOL		ReadUseSnapshots();
		BOOL		ReadGraphicsOptions(SGraphicsOptions* pOptions);
		BOOL		ReadDatabaseOptions(SDatabaseOptions* pOptions);
		BOOL		ReadVideoOptions(SVideoOptions* pOptions);
		BOOL		ReadTextOptions(STextOptions* pOptions);
		BOOL		ReadRingtailOptions(SRingtailOptions* pOptions);
		BOOL		ReadSystemOptions(SSystemOptions* pOptions);
		BOOL		ReadCaptureOptions(SCaptureOptions* pOptions);
		BOOL		ReadCommandLine(SCommandLine* pCommandLine);
		double		ReadFrameRate();
		void		WriteVideoDrive(BOOL bUseDrive, LPCSTR lpDrive);
		void		WriteEnableErrors(int iApplication, BOOL bEnable);
		void		WriteUseSnapshots(BOOL bDIBSnapshots);
		void		WriteEnablePowerPoint(BOOL bEnable);
		void		WriteFrameRate(double dFrameRate);
		void		WriteGraphicsOptions(SGraphicsOptions* pOptions);
		void		WriteDatabaseOptions(SDatabaseOptions* pOptions);
		void		WriteVideoOptions(SVideoOptions* pOptions);
		void		WriteTextOptions(STextOptions* pOptions);
		void		WriteRingtailOptions(SRingtailOptions* pOptions);
		void		WriteSystemOptions(SSystemOptions* pOptions);
		void		WriteCaptureOptions(SCaptureOptions* pOptions);
		void		WriteCommandLine(SCommandLine* pCommandLine);

		//	Functions used to read/write case descriptors
		void		ReadLastCase(LPSTR lpFolder, int iLength);
		void		WriteLastCase(LPCSTR lpFolder);

		//	Generic ini management routines
		void		SetDirectory(LPCSTR lpDirectory);
		void		SetFileSpec(LPCSTR lpFileSpec);
		void        SetSection(LPCSTR lpSection, BOOL bDelete = FALSE);
        void        DeleteSection(LPCSTR lpSection);
        void        DeleteLine(LPCSTR lpLine);

        BOOL        ReadBool(LPCSTR lpLine, BOOL bDefault = FALSE,
                             BOOL bDelete = FALSE);
        BOOL        ReadBool(int iLine, BOOL bDefault = FALSE,
                             BOOL bDelete = FALSE);
        long        ReadLong(LPCSTR lpLine, long lDefault = 0L,
                             BOOL bDelete = FALSE);
        long        ReadLong(int iLine, long lDefault = 0L,
                             BOOL bDelete = FALSE);
        double      ReadDouble(LPCSTR lpLine, double dDefault = 0.0,
                               BOOL bDelete = FALSE);
        double      ReadDouble(int iLine, double dDefault = 0.0,
                               BOOL bDelete = FALSE);
        LPSTR       ReadString(LPCSTR lpLine, LPSTR lpString, int iLength,
                               LPCSTR lpDefault = "", BOOL bDelete = FALSE);
        LPSTR       ReadString(int iLine, LPSTR lpString, int iLength,
                               LPCSTR lpDefault = "", BOOL bDelete = FALSE);

        BOOL        WriteBool(LPCSTR lpLine, BOOL bValue);
        BOOL        WriteBool(int iLine, BOOL bValue);
        BOOL        WriteLong(LPCSTR lpLine, long lValue);
        BOOL        WriteLong(int iLine, long lValue);
        BOOL        WriteDouble(LPCSTR lpLine, double dValue);
        BOOL        WriteDouble(int iLine, double dValue);
        BOOL        WriteString(LPCSTR lpLine, LPCSTR lpString);
        BOOL        WriteString(int iLine, LPCSTR lpString);

};

#endif // __TMINI_H__
