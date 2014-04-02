//==============================================================================
//
// File Name:	view.h
//
// Description:	This file contains the declaration of the CMainView class.
//
// Author:		Kenneth Moore
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	08-17-97	1.00		Original Release
//	10-27-97	1.01		Added TMPlay control and capabilities
//==============================================================================
#if !defined(AFX_VIEW_H__AA005151_16FD_11D1_B02E_008029EFD140__INCLUDED_)
#define AFX_VIEW_H__AA005151_16FD_11D1_B02E_008029EFD140__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif 

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------

#include <tmini.h>
#include <handler.h>
#include <dbabstract.h>
#include <tables.h>
#include <setline.h>
#include <barcode.h>
#include <playlist.h>
#include <show.h>
#include <tmstdefs.h>
#include <tmtbdefs.h>
#include <tmhelp.h>
#include <applink.h>

#include <tmview.h>		//	Control headers
#include <tmmovie.h>
#include <tmpower.h>
#include <tmtext.h>
#include <tmtool.h>
#include <tmstat.h>
#include <tmlpen.h>
#include <tmgrab.h>
#include <tmshare.h>
#include <VKBDlg.h>
#include <atlimage.h>
#include <Document.h>
#include <BinderList.h>
#include <ColorPickerList.h>


//------------------------------------------------------------------------------
//	DEFINES
//------------------------------------------------------------------------------

//	Video size identifiers for S_LINK state
//
//	NOTE:	These constants should be enumerated in the same order in which the
//			options appear on the Video configuration page
#define VIDEO4							0		//	Quarter screen video
#define VIDEO6							1		//	Sixth screen video
#define VIDEO8							2		//	Eighth screen video
#define VIDEO0							3		//	No video

//	Video position identifiers for S_LINK state
//
//	NOTE:	These constants should be enumerated in the same order in which the
//			options appear on the Video configuration page
#define VIDEO_UPPERLEFT					0		//	Upper left
#define VIDEO_UPPERRIGHT				1		//	Upper right
#define VIDEO_LOWERLEFT					2		//	Lower left
#define VIDEO_LOWERRIGHT				3		//	Lower right

//	Identifiers used in calls to SetPageFromId()
#define SETPAGE_BYORDER					0
#define SETPAGE_BYID					1
#define SETPAGE_FIRST					2
#define SETPAGE_NEXT					3
#define SETPAGE_PREVIOUS				4
#define SETPAGE_LAST					5

//	Control bar identifiers
#define CONTROL_BAR_NONE				0
#define CONTROL_BAR_TOOLS				1
#define CONTROL_BAR_STATUS				2
#define CONTROL_BAR_SHOW_LARGE			3
#define CONTROL_BAR_HIDE_LARGE			4


//	User split screen states
#define USER_SPLITSCREEN_SINGLE			0
#define USER_SPLITSCREEN_VERTICAL		1
#define USER_SPLITSCREEN_HORIZONTAL		2

//	Control key states
#define TMKEY_NONE						0x0000
#define TMKEY_SHIFT						0x0001
#define TMKEY_ALT						0x0002
#define TMKEY_CONTROL					0x0004
#define TMKEY_SHIFTALT					(TMKEY_SHIFT | TMKEY_ALT)
#define TMKEY_SHIFTCONTROL				(TMKEY_SHIFT | TMKEY_CONTROL)
#define TMKEY_SHIFTALTCONTROL			(TMKEY_SHIFT | TMKEY_ALT | TMKEY_CONTROL)
#define TMKEY_ALTCONTROL				(TMKEY_ALT | TMKEY_CONTROL)

//	Keyboard hook characters
#define KEYBOARD_VKCODE					'~'
#define KEYBOARD_PRIMARY_BARCODE		'X'
#define KEYBOARD_ALTERNATE_BARCODE		'`'

//	Indices into hotkeys array
#define HK_BLANK						0
#define HK_CALLOUT						1
#define HK_DRAW							2
#define HK_HIGHLIGHT					3
#define HK_REDACT						4
#define HK_MAGNIFY						5
#define HK_PRINT						6
#define HK_ERASE						7
#define HK_FULLPAGE						8
#define HK_PLAYPAUSE					9
#define HK_ZAP							10
#define HK_FULLWIDTH					11
#define HK_PREVPAGE						12
#define HK_NEXTPAGE						13
#define HK_PAN							14
#define HK_CW							15
#define HK_CCW							16
#define HK_SPLITVERTICAL				17
#define HK_CHANGEPANE					18
#define HK_SETPAGELINE					19
#define HK_SETPAGELINENEXT				20
#define HK_DELETEANN					21
#define HK_SELECT						22
#define HK_PLAYTHROUGH					23
#define HK_VIDEOCAPTION					24
#define HK_TEXT							25
#define HK_SELECTTOOL					26
#define HK_FULLSCREEN					27
#define HK_STATUSBAR					28
#define HK_NEXTMEDIA					29
#define HK_PREVMEDIA					30
#define HK_FREEHAND						31
#define HK_ANNTEXT						32
#define HK_LINE							33
#define HK_ARROW						34
#define HK_ELLIPSE						35
#define HK_RECTANGLE					36
#define	HK_POLYGON						37
#define HK_POLYLINE						38
#define HK_FILLEDELLIPSE				39
#define HK_FILLEDRECTANGLE				40
#define HK_BLACK						41
#define HK_YELLOW						42
#define HK_WHITE						43
#define HK_RED							44
#define HK_GREEN						45
#define HK_BLUE							46
#define HK_LIGHTRED						47
#define HK_LIGHTGREEN					48
#define HK_LIGHTBLUE					49
#define HK_MOUSEMODE					50
#define HK_TOOLBAR						51
#define HK_ZOOMRESTRICTED				52
#define HK_SPLITHORIZONTAL				53
#define HK_NEXTPAGE_HORIZONTAL			54
#define HK_NEXTPAGE_VERTICAL			55
#define HK_SPLITPAGES_NEXT				56
#define HK_SPLITPAGES_PREVIOUS			57
#define HK_SHADEONCALLOUT				58
#define HK_NEXT_BARCODE					59
#define HK_PREV_BARCODE					60
#define HK_ADD_TO_BINDER				61
#define HK_UPDATE_ZAP					62
#define HK_SPLIT_ZAP					63
#define HK_GESTURE_PAN					64
#define MAX_HOTKEYS						65

//	INI section identifiers
#define HOTKEYS_SECTION					"TM HOTKEYS"
#define TMAX_TEST_SECTION				"PRESENTATION TEST"
#define TMMOVIE_FILTERS_SECTION			"TMMOVIE FILTERS"

//	Ini configuration lines
#define LASTPLAYLIST_LINE				"LastPlaylist"
#define VKCHAR_LINE						"VKChar"
#define PRIMARY_BARCODE_LINE			"PrimaryBarcodeChar"
#define ALTERNATE_BARCODE_LINE			"AlternateBarcodeChar"
#define CREATE_DOCUMENTS_LINE			"CreateDocuments"
#define MAX_BARCODES_LINE				"MaxBarcodes"
#define	HELP_FILENAME_LINE				"HelpFilename"
#define TEST_PLAYLISTS_LINE				"Playlists"
#define TEST_MEDIA_LINE					"Media"
#define TEST_PLAYLISTINTERVAL_LINE		"PlaylistInterval"
#define TEST_MEDIAINTERVAL_LINE			"MediaInterval"
#define TEST_PAGES_LINE					"Pages"
#define TEST_ACTIVITYLOG_LINE			"ActivityLog"
#define TEST_FIRSTPLAYLIST_LINE			"FirstPlaylist"
#define TEST_LASTPLAYLIST_LINE			"LastPlaylist"
#define TEST_FIRSTMEDIA_LINE			"FirstMedia"
#define TEST_LASTMEDIA_LINE				"LastMedia"
#define TEST_LINKS_LINE					"Links"
#define TEST_ZAPS_LINE					"Zaps"
#define TEST_STATUSBAR_LINE				"StatusBar"

//	Default configuration values
#define DEFAULT_MAX_BARCODES			2
#define DEFAULT_HELP_FILENAME			"TrialMax Manual.pdf"

//	Hotkeys lines
#define HK_BLANK_LINE					"BlankScreen"
#define HK_CALLOUT_LINE					"Callout"
#define HK_DRAW_LINE					"Draw"
#define HK_HIGHLIGHT_LINE				"Highlight"
#define HK_REDACT_LINE					"Redact"
#define HK_MAGNIFY_LINE					"Magnify"
#define HK_PRINT_LINE					"Print"
#define HK_ERASE_LINE					"Erase"
#define HK_FULLPAGE_LINE				"FullPage"
#define HK_PLAYPAUSE_LINE				"PlayPause"
#define HK_ZAP_LINE						"Zap"
#define HK_FULLWIDTH_LINE				"FullWidth"
#define HK_PREVPAGE_LINE				"PrevPage"
#define HK_NEXTPAGE_LINE				"NextPage"
#define HK_PAN_LINE						"Pan"
#define HK_CW_LINE						"RotateCw"
#define HK_CCW_LINE						"RotateCcw"
#define HK_SPLITVERTICAL_LINE			"SplitScreen"
#define HK_SPLITHORIZONTAL_LINE			"SplitHorizontal"
#define HK_CHANGEPANE_LINE				"ChangePane"
#define HK_SETPAGELINE_LINE				"SetPageLine"
#define HK_SETPAGELINENEXT_LINE			"SetPageLineNext"
#define HK_DELETEANN_LINE				"DeleteAnn"
#define HK_SELECT_LINE					"Select"
#define HK_PLAYTHROUGH_LINE				"PlayThrough"
#define HK_VIDEOCAPTION_LINE			"VideoCaption"
#define HK_TEXT_LINE					"Text"
#define HK_FULLSCREEN_LINE				"FullScreen"
#define HK_SELECTTOOL_LINE				"SelectTool"
#define HK_STATUSBAR_LINE				"StatusBar"
#define HK_TOOLBAR_LINE					"Toolbar"
#define HK_NEXTMEDIA_LINE				"NextMedia"
#define HK_PREVMEDIA_LINE				"PrevMedia"
#define HK_FREEHAND_LINE				"Freehand"
#define HK_ANNTEXT_LINE					"AnnText"
#define HK_LINE_LINE					"Line"
#define HK_ARROW_LINE					"Arrow"
#define HK_ELLIPSE_LINE					"Circle"
#define HK_RECTANGLE_LINE				"Square"
#define	HK_POLYGON_LINE					"Polygon"
#define HK_POLYLINE_LINE				"Polyline"
#define HK_FILLEDELLIPSE_LINE			"FilledCircle"
#define HK_FILLEDRECTANGLE_LINE			"FilledSquare"
#define HK_BLACK_LINE					"Black"
#define HK_YELLOW_LINE					"Yellow"
#define HK_WHITE_LINE					"White"
#define HK_RED_LINE						"Red"
#define HK_GREEN_LINE					"Green"
#define HK_BLUE_LINE					"Blue"
#define HK_LIGHTRED_LINE				"LightRed"
#define HK_LIGHTGREEN_LINE				"LightGreen"
#define HK_LIGHTBLUE_LINE				"LightBlue"
#define HK_MOUSEMODE_LINE				"MouseMode"
#define HK_ZOOMRESTRICTED_LINE			"ZoomRestricted"
#define HK_NEXTPAGE_HORIZONTAL_LINE		"NextPageHorizontal"
#define HK_NEXTPAGE_VERTICAL_LINE		"NextPageVertical"
#define HK_SPLITPAGES_NEXT_LINE			"SplitPagesNext"
#define HK_SPLITPAGES_PREVIOUS_LINE		"SplitPagesPrev"
#define HK_SHADEONCALLOUT_LINE			"ShadeOnCallout"
#define HK_NEXT_BARCODE_LINE			"NextBarcode"
#define HK_PREV_BARCODE_LINE			"PrevBarcode"
#define HK_ADD_TO_BINDER_LINE			"AddToBinder"
#define HK_UPDATE_ZAP_LINE				"UpdateZap"
#define HK_SPLIT_ZAP_LINE				"SaveSplitZap"
#define HK_ENABLE_GESTURE				"EnableGesture"

//	Default hotkey assignments
#define DEFAULT_HK_BLANK				"B"
#define DEFAULT_HK_CALLOUT				"C"
#define DEFAULT_HK_DRAW					"D"
#define DEFAULT_HK_HIGHLIGHT			"H"
#define DEFAULT_HK_REDACT				"R"
#define DEFAULT_HK_MAGNIFY				"Z"
#define DEFAULT_HK_PRINT				"P"
#define DEFAULT_HK_ERASE				"E"
#define DEFAULT_HK_FULLPAGE				"F"
#define DEFAULT_HK_PLAYPAUSE			"V"
#define DEFAULT_HK_ZAP					"M"
#define DEFAULT_HK_SPLIT_ZAP			"MS"
#define DEFAULT_HK_FULLWIDTH			"W"
#define DEFAULT_HK_PREVPAGE				","
#define DEFAULT_HK_NEXTPAGE				"."
#define DEFAULT_HK_PAN					"S"
#define DEFAULT_HK_CW					"]"
#define DEFAULT_HK_CCW					"["
#define DEFAULT_HK_SPLIT_VERTICAL		"/"
#define DEFAULT_HK_SPLIT_HORIZONTAL		"\\"
#define DEFAULT_HK_CHANGEPANE			"="
#define DEFAULT_HK_SETPAGELINE			"G"
#define DEFAULT_HK_SETPAGELINENEXT		"GC"
#define DEFAULT_HK_DELETEANN			"EC"
#define DEFAULT_HK_SELECT				"A"
#define DEFAULT_HK_PLAYTHROUGH			"VC"
#define DEFAULT_HK_VIDEOCAPTION			"CC"
#define DEFAULT_HK_TEXT					"T"
#define DEFAULT_HK_FULLSCREEN			"FC"
#define DEFAULT_HK_SELECTTOOL			"DC"
#define DEFAULT_HK_STATUSBAR			"SC"
#define DEFAULT_HK_TOOLBAR				"TC"
#define DEFAULT_HK_NEXTMEDIA			">S"
#define DEFAULT_HK_PREVMEDIA			"<S"
#define DEFAULT_HK_FREEHAND				"FS"
#define DEFAULT_HK_ANNTEXT				"TS"
#define DEFAULT_HK_LINE					"LS"
#define DEFAULT_HK_ARROW				"AS"
#define DEFAULT_HK_ELLIPSE				"CS"
#define DEFAULT_HK_RECTANGLE			"SS"
#define	DEFAULT_HK_POLYGON				"HS"
#define DEFAULT_HK_POLYLINE				"PS"
#define DEFAULT_HK_FILLEDELLIPSE		"CCS"
#define DEFAULT_HK_FILLEDRECTANGLE		"SCS"
#define DEFAULT_HK_BLACK				")S"
#define DEFAULT_HK_YELLOW				"!S"
#define DEFAULT_HK_WHITE				"@S"
#define DEFAULT_HK_RED					"#S"
#define DEFAULT_HK_GREEN				"$S"
#define DEFAULT_HK_BLUE					"%S"
#define DEFAULT_HK_LIGHTRED				"^S"
#define DEFAULT_HK_LIGHTGREEN			"&S"
#define DEFAULT_HK_LIGHTBLUE			"*S"
#define DEFAULT_HK_MOUSEMODE			"MC"
#define DEFAULT_HK_ZOOMRESTRICTED		"ZS"
#define DEFAULT_HK_NEXTPAGE_HORIZONTAL	"\\C"
#define DEFAULT_HK_NEXTPAGE_VERTICAL	"/C"
#define DEFAULT_HK_SHADEONCALLOUT		"HC"
#define DEFAULT_HK_NEXT_BARCODE			">CS"
#define DEFAULT_HK_PREV_BARCODE			"<CS"
#define DEFAULT_HK_ADD_TO_BINDER		"BC"
#define DEFAULT_HK_UPDATE_ZAP			"U"
#define DEFAULT_HK_SPLITPAGES_PREVIOUS	",C"
#define DEFAULT_HK_SPLITPAGES_NEXT		".C"
#define DEFAULT_HK_ENABLE_GESTURE		"PC"

//	Test setup ini line identifiers
#define TEST_PLAYLISTS_LINE				"Playlists"
#define TEST_IMAGES_LINE				"Images"
#define TEST_MOVIES_LINE				"Movies"
#define TEST_POWERPOINTS_LINE			"PowerPoints"
#define TEST_SHOWS_LINE					"Shows"
#define TEST_TREATMENTS_LINE			"Treatments"
#define TEST_LINKS_LINE					"Links"
#define TEST_PAGES_LINE					"Pages"
#define TEST_PLAYLISTPERIOD_LINE		"PlaylistPeriod"
#define TEST_MEDIAPERIOD_LINE			"MediaPeriod"
#define TEST_MOVIEPERIOD_LINE			"MoviePeriod"
#define TEST_ACTIVITYLOG_LINE			"ActivityLog"
#define TEST_STATUSBAR_LINE				"StatusBar"

//	Default test options		
#define TESTMODE_PLAYLISTS				TRUE
#define TESTMODE_IMAGES					TRUE
#define TESTMODE_LINKS					TRUE
#define TESTMODE_TREATMENTS				TRUE
#define TESTMODE_STATUSBAR				TRUE
#define TESTMODE_PAGES					TRUE
#define TESTMODE_MOVIES					TRUE
#define TESTMODE_POWERPOINTS			TRUE
#define TESTMODE_SHOWS					TRUE
#define TESTMODE_PLAYLISTPERIOD			30	//	Seconds
#define	TESTMODE_MEDIAPERIOD			2	//	Seconds
#define TESTMODE_MOVIEPERIOD			30	//	Seconds
#define TESTMODE_ACTIVITYLOG			"Tmaxtest.txt"

//	Test mode states
#define TESTING_PLAYLIST				0
#define TESTING_IMAGE					1
#define TESTING_SHOW					2
#define TESTING_POWERPOINT				3
#define TESTING_MOVIE					4

#define INITIALIZE_TIMERID				1
#define TESTMODE_TIMERID				2
#define CUSTOM_SHOW_TIMERID				3
#define CAPTURE_BARCODE_TIMERID			4

#define MAXLEN_LINK_BARCODE				256
#define MAXLEN_LINK_PSTQ				64

#define kbIconPadding					15


//------------------------------------------------------------------------------
//	GLOBALS
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//	DECLARATIONS
//------------------------------------------------------------------------------

//	These structures are used to manage multipage information retrieved from
//	the database
typedef struct
{
	CMultipage*		pMultipage;
	CSecondary*		pSecondary;
	CTertiary*		pTertiary;

}SMultipageInfo;

//	These structures are used to manage playlist information retrieved from
//	the database
typedef struct
{
	CPlaylist*		pPlaylist;
	CDesignation*	pDesignation;
	CTextLine*		pLine;
}SPlaylistInfo;

//	These structures are used to manage custom show information retrieved from
//	the database
typedef struct
{
	CShow*			pShow;
	CShowItem*		pItem;
}SShowInfo;

//	These structures are used to manage link information during video playback
typedef struct
{
	long	lFlags;
	BOOL	bEvent;
	BOOL	bPending;
	char	szBarcode[MAXLEN_LINK_BARCODE];
	char	szPSTQ[MAXLEN_LINK_PSTQ];
}SLinkInfo;

//	These structures are used to pass information to the event processor for
//	the PLAYLIST_REQUEST event
typedef struct
{
	CPlaylist*		pPlaylist;
	CDesignation*	pStart;
	CDesignation*	pStop;
	long			lStart;
	long			lStop;
	double			dPosition;
	CString*		pError;
}SPlaylistParams;

//	These structures are used to identify hotkey combinations
typedef struct
{
	char		cKey;
	short		sState;
} SHotKey;

//	These structures are used to manage the toolbar associated with each
//	display state
typedef struct
{
	BOOL		bShow;
	BOOL		bDock;
	BOOL		bStretch;
	BOOL		bFlat;
	short		aMap[TMTB_MAXBUTTONS];
	short		sSize;
	CTMTool*	pControl;

} SToolbar;

//	This structure is used to manage the information associated with the
//	application's active control bar
typedef struct
{
	int			iId;
	int			iHeight;
	BOOL		bDocked;
	CWnd*		pWnd;
} SControlBar;

typedef struct
{
	CMedias*	pPlaylists;
	CMedias*	pImages;
	CMedias*	pPowerPoints;
	CMedias*	pMovies;
	CMedias*	pShows;
	long		lElapsed;
	long		lPeriod;
	long		lPlaylistPeriod;
	long		lMoviePeriod;
	long		lMediaPeriod;
	short		sState;
	short		sTreatment;
	short		sTreatments;
	BOOL		bActive;
	BOOL		bPlaylists;
	BOOL		bImages;
	BOOL		bMovies;
	BOOL		bPowerPoints;
	BOOL		bShows;
	BOOL		bTreatments;
	BOOL		bPages;
	char		szLogFile[512];

} STestInfo;

//	Forward declarations
class CMainFrame;

class CMainView : public CFormView
{
	private:

		CTMHelp			m_Help;
		CMedia*			m_pMedia;
	
		CBarcodeBuffer	m_aBarcodes;
		CBarcode		m_Barcode;

		SMultipageInfo	m_TMView1;
		SMultipageInfo	m_TMView2;
		SMultipageInfo	m_TMPower1;
		SMultipageInfo	m_TMPower2;
		SMultipageInfo	m_TMMovie;

		SPlaylistInfo	m_Playlist;
		SShowInfo		m_Show;
		CAppLink		m_AppLink;

		int				m_iImageSecondary;
		int				m_iAnimationSecondary;
		int				m_iPlaylistSecondary;
		int				m_iPowerPointSecondary;
		int				m_iCustomShowSecondary;
		int				m_iTreatmentTertiary;

		SToolbar		m_aToolbars[MAX_STATES+2];
		SControlBar		m_ControlBar;
		SControlBar		m_ControlBarExtra;
		CTMTool*		m_pToolbar;

		SPlaylistStatus	m_PlaylistStatus;
		CErrorHandler	m_Errors;
		CMainFrame*		m_pFrame;
		CTMIni			m_Ini;
		CDBAbstract*	m_pDatabase;
		CString			m_strCaseFolder;
		CString			m_strIniFile;
		CString			m_strAppFolder;
		CString			m_strLastPlaylist;
		CString			m_strHelpFileSpec;
		CBrush			m_brBackground;
		RECT			m_rcView;
		RECT			m_rcMovie;
		RECT			m_rcText;
		RECT			m_rcPower;
		RECT			m_rcStatus;
		int				m_iMaxWidth;
		int				m_iMaxHeight;
		int				m_iHalfWidth;
		int				m_iHalfHeight;
		int				m_iCuePage;
		int				m_iCueLine;
		int				m_iLoadPage;
		int				m_iLoadLine;
		int				m_iUserSplitFrameColor;
		int				m_iZapSplitFrameColor;
		short			m_sState;
		short			m_sPrevState;
		short			m_sUpdateButton;
		short			m_sVideoSize;
		short			m_sVideoPosition;
		float			m_fMovieStep;
		float			m_fPlaylistStep;
		double			m_dFrameRate;
		long			m_lCueTranscript;
		double			m_dMinMovieCue;
		double			m_dMaxMovieCue; 
		char			m_cPrimaryBarcodeChar;
		char			m_cAlternateBarcodeChar;
		char			m_cVKChar;
		BOOL			m_bUseSecondaryMonitor;
		BOOL			m_bMouseMode;
		BOOL			m_bPlaying;
		BOOL			m_bEnableErrors;
		BOOL			m_bEnablePowerPoint;
		BOOL			m_bEnablePlay;
		BOOL			m_bResumePlaylist;
		BOOL			m_bResumeMovie;
		BOOL			m_bClipsAsPlaylists;
		BOOL			m_bRunToEnd;
		BOOL			m_bClearMovie;
		BOOL			m_bClearPlaylist;
		BOOL			m_bScaleAVI;
		BOOL			m_bScalePlaylists;
		BOOL			m_bScaleGraphics;
		BOOL			m_bScaleDocs;
		BOOL			m_bDisableLinks;
		BOOL			m_bDoUpdates;
		BOOL			m_bCuedMovie;
		BOOL			m_bUseSnapshots;
		BOOL			m_bLoadingPlaylist;
		BOOL			m_bLoadingShowItem;
		BOOL			m_bLoadingSplitZap;
		BOOL			m_bCenterVideo;
		BOOL			m_bCombineText;
		BOOL			m_bPenControls;
		BOOL			m_bSplitScreenDocuments;
		BOOL			m_bSplitScreenGraphics;
		BOOL			m_bSplitScreenPowerPoints;
		BOOL			m_bSplitScreenLink;
		BOOL			m_bClassicLinks;
		BOOL			m_bEditTextAnn;
		BOOL			m_bRestoreToolbar;
		BOOL			m_bCreateDocuments;
		BOOL			m_bSystemScrollEnabled;
		BOOL			m_bUserScrollEnabled;
		BOOL			m_bZapSplitScreen;
		BOOL			m_bShowExtraToolBar;
		SHotKey			m_Hotkeys[MAX_HOTKEYS + 1];
		STestInfo		m_Test;

		CString			m_strCaptureFolder;
		CString			m_strCaptureTargetFolder;
		CString			m_strCaptureTargetFile;
		CStringArray*	m_paCaptureFiles;
		FILE*			m_fptrCaptureBarcodes;
		int				m_iCaptureFileIndex;
		int				m_iCapturedBarcodes;

		CVKBDlg			*m_pVKBDlg;
		DWORD			g_tcLastLeftButtonClickTime;

		// WM_GESTURE variables
		POINTS			m_gestureStartPoint; 
		POINTS			m_gestureLastPoint;
		UINT64			m_ullArguments;
		DWORD			m_gestureStartTime;
		BOOL			m_bGestureHandled;

		BOOL			m_bTabletMode;
		CBinderEntry	m_currentBinderItem;
		CBinderEntry	m_parentBinderItem;	
		POINT			m_BinderListPosition;
		CBinderList*	m_BinderList;
		BOOL			m_bIsBinderOpen;
		
		POINT			m_ColorPickerListPosition;
		BOOL			m_bIsColorPickerOpen;
		CColorPickerList* m_ColorPickerList;
		
	public:
		


		//	This member is accessed by the frame window to inhibit
		//	redrawing of the view
		BOOL			m_bRedraw;

						DECLARE_DYNCREATE(CMainView)
						
						CMainView();
					   ~CMainView();
					   CColorDialog m_wndLum;

		void			LoadPageFromKeyboard(long lPage);
		void			CuePlaylist(short sMove, BOOL bForward);
		void			CueMovie(short sMove, BOOL bForward);
		void			SetToForeground();
		void			OnNewInstance();
		void			SetLoadPage(int iPage){ m_iLoadPage = iPage; }
		void			SetLoadLine(int iLine){ m_iLoadLine = iLine; }
		BOOL			LoadFromBarcode(LPCSTR lpBarcode, BOOL bAddBuffer, BOOL bAlternate = FALSE);
		BOOL			Shutdown();
		BOOL			ProcessCommandKey(char cKey);
		BOOL			ProcessVirtualKey(WORD wKey);
		BOOL			ProcessMouseMessage(MSG* pMsg);
		BOOL			GetUseSecondaryMonitor(){ return m_bUseSecondaryMonitor; }
		CTMDocument*	GetDocument();
		void			BlankPresentationToolbar();
		void            DisableGestureOnCommand(short sCommand);


		//	Keyboard hook characters
		char			GetPrimaryBarcodeChar(){ return m_cPrimaryBarcodeChar; }
		char			GetAlternateBarcodeChar(){ return m_cAlternateBarcodeChar; }
		char			GetVKChar(){ return m_cVKChar; }
	
		LONG			OnWMMouseMode(WPARAM wParam, LPARAM lParam);
		LONG			OnWMGrabFocus(WPARAM wParam, LPARAM lParam);
		void			OnBinderDialogButtonClickEvent(CBinderEntry pBinderEntry);		
		void			OnBinderDialogBackButtonClickEvent(CBinderEntry pBinderEntry);
		void			OnBinderDialogCloseButtonClickEvent();
		void            OnGesturePan();
		void			OnColorPickerButtonClickEvent(int iColorType);				
		void			OnColorPickerCloseButtonClickEvent();

	protected: 		
		void			SetTaskBarVisible(BOOL bVisible);
		void			UpdateToolColor();
		void			UpdateStatusBar();
		void			UpdatePlaylistStatus();
		void			UpdateActivityLog(LPCSTR lpText, BOOL bStamp = TRUE);
		void			SetDisplay(short sState);
		void			SetDrawingTool(short sTool);
		void			RestoreDisplay();
		void			SetSinglePaneMode();
		void			SetZapSplitScreen(BOOL bZapSplitScreen);
		void			InitializeTest();
		void			ReadHotkeys();
		void			ReadSetup(BOOL bFirstTime = FALSE);
		void			ReadTestSetup();
		void			InitializeToolbars();
		void			SwitchPane();
		void			ReadToolbar(SToolbar* pToolbar, LPSTR lpSection, LPSTR lpszMaster);
		void			OnExit();
		void			SelectToolbar(short sState);
		void			ResetTMMovie(BOOL bAnimation, BOOL bPlaylist);
		void			ResetTMView();
		void			ResetTMPower();
		void			ResetShowInfo();
		void			ClearTMViewInactive();
		void			HandleError(LPCSTR lpTitle, UINT uId, 
									LPCSTR lpArg1 = 0, LPCSTR lpArg2 = 0);
		void			FormatError(CString& rString, UINT uId, 
									LPCSTR lpArg1 = 0, LPCSTR lpArg2 = 0);
		void			SetPosition(CSetLine* pSetLine);
		void			SetNextLine(LPCSTR lpErrorMsg = NULL);
		void			SetLine(LPCSTR lpErrorMsg = NULL, CPlaylist* pPlaylist = NULL, long lDesignation = -1);
		void			SetLastPlaylist(CPlaylist* pPlaylist);
		void			ParseHotKeySpec(short sIndex, LPSTR lpSpec);
		void			RecalcLayout(short sState);
		void			ShowLightPen(BOOL bShow);
		void			ShowRectangle(RECT* pRect, LPCSTR lpTitle);
		void			ResetMultipage(SMultipageInfo* pInfo);
		void			UpdateMultipage(SMultipageInfo* pTarget, SMultipageInfo* pSource);
		void			CloseDatabase();
		void			SetControlBar(int iId);
		void			StopAutoTransition();
		void			OnManagerReqLoad();
		void			OnManagerResAddTreatment();
		void			DbgMsg(LPCSTR lpszFormat, ...);
		void			DbgMsg(SMultipageInfo* pInfo, LPCSTR lpszFormat, ...);
		BOOL			CheckLinkOptions();
		BOOL			SaveZap();
		BOOL			SaveSplitZap();
		BOOL			GetLinkedVideoSize(SIZE& rSize);
		BOOL			GetLinkedTextEnabled();
		BOOL			ProcessNotification(int iX, int iY);
		BOOL			ProcessEvent(short sEvent, DWORD dwParam1 = 0,
									 DWORD dwParam2 = 0);
		BOOL			SetPageFromBarcode(SMultipageInfo* pInfo, 
										   long lSecondaryId, long lTertiaryId);
		BOOL			SetPageFromId(SMultipageInfo* pInfo, long lPage,
									  int iLookup);
		BOOL			LoadMedia(CMedia* pMedia, long lSecondary, long lTertiary);
		BOOL			LoadAsPlaylist(CMultipage* pMultipage, long lSecondary);
		BOOL			LoadMultipage(SMultipageInfo* pInfo);
		BOOL			LoadTreatment(SMultipageInfo* pMPTreatment, short sState);
		BOOL			LoadTreatment(SMultipageInfo* pMPTreatment, LPCSTR lpszZapFileSpec,
									  LPCSTR lpszSourceFileSpec, short sPane = -1);
		BOOL			LoadPlaylist(SPlaylistParams* pParams);
		BOOL			LoadShowItem(SShowInfo* pInfo);
		BOOL			LoadDeposition(CMedia* pDeposition, long lSegment, long lPage = 0, int iLine = 0);
		BOOL			ProcessCommand(short sCommand);
		BOOL			IsCommandEnabled(short sCommand);
		BOOL			IsCommandChecked(short sCommand);
		BOOL			IsScrollTextVisible();
		BOOL			IsVideoVisible();
		BOOL			OpenDatabase(LPCSTR lpFolder);
		BOOL			AllocDatabase(LPCSTR lpszFolder);
		BOOL			FindFile(LPCSTR lpFile);
		BOOL			TranslateLine(SPlaylistParams* pParams);
		BOOL			GetTestBarcode(CString& rBarcode);
		BOOL			GetLinkState();
		BOOL			GetScrollTextEnabled(CDesignation* pDesignation);
		BOOL			OpenCaptureSource();
		BOOL			LoadCaptureBarcode();
		BOOL			GetCaptureSource(LPCSTR lpFolder, CStringArray& aFiles);
		BOOL			CaptureBarcode();
		BOOL			GetSplitPageInfo(SMultipageInfo* pInfo, BOOL bPrevious);
		BOOL			GetTreatmentFileSpecs(SMultipageInfo* pInfo, CString& rPageFileSpec, CString& rZapFileSpec, BOOL bVerify);
		BOOL			SetMultipageInfo(SMultipageInfo& rMPInfo, LPCSTR lpszId, BOOL bBarcode);
		BOOL			UpdateZap(SMultipageInfo* pInfo, short sPaneId);
		CString			GetBarcode(CMedia* pPrimary, CSecondary* pSecondary = NULL, CTertiary* pTertiary = NULL);
		CString			GetBarcode(SMultipageInfo& rMultipageInfo);
		short			GetTMKeyState();
		SMultipageInfo*	GetMultipageInfo(short sState);
		CPlaylist*		GetLastPlaylist();
		//LRESULT CALLBACK OnDTMouseEvent(int nCode, WPARAM wParam, LPARAM lParam);

		LRESULT			OnGesture(WPARAM wParam, LPARAM lParam);
		void			HandlePan(GESTUREINFO gi);
		void			HandleZoom(GESTUREINFO gi);
		void			DisplayKeyboardIconGesture(POINTS pCurrent);
		void			DisplayToolbarGesture(POINTS pCurrent);
		void            LogMe(LPCTSTR msg);
		
		CBinderList*	CMainView::CreateBinder(list<CBinderEntry> pBinderEntryList, BOOL bIsShowBackButton);
		void			OpenBinder(int parentId = 0);
		void			OpenBinder(list<CBinderEntry> pBinderEntry);
		void			OpenBinderList(list<CBinderEntry> pBinderEntry, int pButtonId);
		void			OpenBinderList(list<CBinderEntry> pBinderEntryList, int pButtonId, CBinderEntry pParentBinder);
		void			BinderListAsBinder(CBinderEntry pBinderEntry);
		CBinderEntry	FilterMediaByMediaId(CBinderEntry pBinerEntry, long lParentId);
		CBinderEntry	ConvertBinderEntry(CBinderEntry* pBinerEntry);
		void			SetBinderPosition();
		void			OpenColorPicker();
		void			SetColorPickerPosition();
		void			ChangeColorOfColorButton(short sColorToChange);
	//	Class Wizard Maintained
	public:
	//{{AFX_DATA(CMainView)
	enum { IDD = IDD_TMAXPRESENTATION_FORM };
	CTm_view	m_ctrlTMView;
	CTMTool	m_ctrlTBDocuments;
	CTMTool	m_ctrlTBDocumentsLarge;
	CTMTool	m_ctrlTBGraphics;
	CTMTool	m_ctrlTBLinks;
	CTMTool	m_ctrlTBMovies;
	CTMTool	m_ctrlTBPlaylists;
	CTMMovie	m_ctrlTMMovie;
	CTMText	m_ctrlTMText;
	CTMTool	m_ctrlTBText;
	CTMTool	m_ctrlTBTools;
	CTMStat	m_ctrlTMStat;
	CTMLpen	m_ctrlTMLpen;
	CTMTool	m_ctrlTBPowerPoint;
	CTMPower	m_ctrlTMPower;
	CTMTool	m_ctrlTBLinkPP;
	CTMGrab	m_ctrlTMGrab;
	CTMShare	m_ctrlManagerApp;	

	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMainView)
	public:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	virtual void OnInitialUpdate();
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual void OnDraw(CDC* pDC);
	virtual LRESULT WindowProc(UINT message, WPARAM wParam, LPARAM lParam);
	//}}AFX_VIRTUAL

	protected:
	//{{AFX_MSG(CMainView)
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg LRESULT OnIdleUpdateCmdUI(WPARAM wParam, LPARAM);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnZoom();
	afx_msg void OnRotateCcw();
	afx_msg void OnRotateCw();
	afx_msg void OnNormal();
	afx_msg void OnRedact();
	afx_msg void OnErase();
	afx_msg void OnDrawTool();
	afx_msg void OnConfig();
	afx_msg void OnHighlight();
	afx_msg void OnNextPage();
	afx_msg void OnPlay();
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnClear();
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg void OnFirstZap();
	afx_msg void OnLastZap();
	afx_msg void OnNextZap();
	afx_msg void OnPreviousZap();
	afx_msg void OnSaveZap();
	afx_msg void OnSaveSplitZap();
	afx_msg void OnFilePrint();
	afx_msg void OnCallout();
	afx_msg void OnPan();
	afx_msg void OnZoomWidth();
	afx_msg void OnAxCreateCallout(long hCallout);
	afx_msg void OnAxDestroyCallout(long hCallout);
	afx_msg void OnBackDesignation();
	afx_msg void OnBackMovie();
	afx_msg void OnFirstDesignation();
	afx_msg void OnFwdDesignation();
	afx_msg void OnFwdMovie();
	afx_msg void OnLastDesignation();
	afx_msg void OnNextDesignation();
	afx_msg void OnStartMovie();
	afx_msg void OnEndMovie();
	afx_msg void OnStartDesignation();
	afx_msg void OnToolbars();
	afx_msg void OnBlack();
	afx_msg void OnBlue();
	afx_msg void OnGreen();
	afx_msg void OnRed();
	afx_msg void OnYellow();
	afx_msg void OnSplitVertical();
	afx_msg void OnVScroll(UINT nSBCode,UINT nPos, CScrollBar* pScrollBar);
	afx_msg void OnSplitHorizontal();
	afx_msg void OnAxButtonClick(short sId, BOOL bChecked);
	afx_msg void OnAxButtonClickLarge(short sId, BOOL bChecked);
	afx_msg void OnDisableLinks();
	afx_msg void OnWhite();
	afx_msg void OnFirstPage();
	afx_msg void OnLastPage();
	afx_msg void OnChangePane(short sPane);
	afx_msg void OnFilterProps();
	afx_msg void OnSetLine();
	afx_msg void OnSetNextLine();
	afx_msg void OnSelect();
	afx_msg void OnPlayThrough();
	afx_msg void OnVideoCaption();
	afx_msg void OnDeleteAnn();
	afx_msg void OnTMaxHelp();
	afx_msg void OnToggleScrollText();
	afx_msg void OnSelectTool();
	afx_msg void OnFreehand();
	afx_msg void OnLine();
	afx_msg void OnArrow();
	afx_msg void OnFilledEllipse();
	afx_msg void OnFilledRectangle();
	afx_msg void OnEllipse();
	afx_msg void OnRectangle();
	afx_msg void OnFullScreen();
	afx_msg void OnStatusBar();
	afx_msg void OnAxClickStatusBar();
	afx_msg void OnAxClickLightPen(short sButton, short sKeyState);
	afx_msg void OnDarkBlue();
	afx_msg void OnDarkGreen();
	afx_msg void OnDarkRed();
	afx_msg void OnLightBlue();
	afx_msg void OnLightGreen();
	afx_msg void OnLightRed();
	afx_msg void OnPolygon();
	afx_msg void OnPolyline();
	afx_msg void OnAnnText();
	afx_msg void OnAxOpenTextBox(short sPane);
	afx_msg void OnAxCloseTextBox(short sPane);
	afx_msg void OnAxPowerFocus(short sView);
	afx_msg void OnNextMedia();
	afx_msg void OnPreviousMedia();
	afx_msg void OnNextBarcode();
	afx_msg void OnPreviousBarcode();
	afx_msg void OnAddToBinder();
	afx_msg void OnAxStateChange(short sState);
	afx_msg void OnAxPlaylistState(short sState);
	afx_msg void OnAxPlaybackError(long lError, BOOL bStopped);
	afx_msg void OnAxLineChange(long lLine);
	afx_msg void OnAxPlaylistTime(double dTime);
	afx_msg void OnAxDesignationTime(double dTime);
	afx_msg void OnAxElapsedTimes(double dPlaylist, double dDesignation);
	afx_msg void OnAxDesignationChange(long lId, long lOrder);
	afx_msg void OnAxLinkChange(LPCTSTR lpszBarcode, long lId, long lFlags);
	afx_msg void OnFirstShowItem();
	afx_msg void OnLastShowItem();
	afx_msg void OnNextShowItem();
	afx_msg void OnPreviousPage();
	afx_msg void OnPreviousDesignation();
	afx_msg void OnPreviousShowItem();
	afx_msg void OnMouseMode();
	afx_msg void OnShowToolbar();
	afx_msg void OnShowToolbarLarge();
	afx_msg void OnZoomRestricted();
	afx_msg void OnNextPageHorizontal();
	afx_msg void OnScreenCapture();
	afx_msg void OnDestroy();
	afx_msg void OnAxStartTextEdit(short sPane);
	afx_msg void OnAxStopTextEdit(short sPane);
	afx_msg void OnShadeOnCallout();
	afx_msg void OnAxPositionChange(double dPosition);
	afx_msg void OnAxManagerRequest();
	afx_msg void OnAxManagerResponse();
	afx_msg void OnCaptureBarcodes();
	afx_msg void OnUpdateZap();
	afx_msg void OnNextPageVertical();
	afx_msg void OnSplitPagesPrevious();
	afx_msg void OnSplitPagesNext();
	afx_msg void OnOpenBinder();
	afx_msg void OnOpenColorPicker();
	DECLARE_EVENTSINK_MAP()
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  
inline CTMDocument* CMainView::GetDocument()
{ 
	return (CTMDocument*)m_pDocument; 
}
#endif

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_VIEW_H__AA005151_16FD_11D1_B02E_008029EFD140__INCLUDED_)
