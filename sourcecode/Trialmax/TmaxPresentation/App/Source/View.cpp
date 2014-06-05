//==============================================================================
//
// File Name:	view.cpp
//
// Description:	This file contains member functions of the CMainView class.
//
// See Also:	view.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-29-2014	7.0.27		Added methods to handle gestures for windows 7
//							and onwards
//	02-21-2014	7.0.29		Added method to display toolbar on blank presentation
//  03-25-2014	7.0.31		Added binder methods. Added method to handle run time
//                          configration of gesture on button click. Modified
//                          gesture handler to vertical zoomed swipe
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <afxpriv.h>
#include <app.h>
#include <db45.h>
#include <dbnet.h>
#include <document.h>
#include <view.h>
#include <tmvdefs.h>
#include <tables.h>
#include <frame.h>
#include <tmmvdefs.h>
#include <tmlpdefs.h>
#include <tmppdefs.h>
#include <tmbadefs.h>
#include <sharedef.h>
#include <toolbars.h>
#include <setup.h>
#include <direct.h>		//	getcwd()
#include <font.h>
#include <math.h>
#include <textline.h>
#include <toolbox.h>
#include <TlHelp32.h>
#include <windows.h>
#include <BinderList.h>


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
extern CApp theApp;
HHOOK			g_hDesktopHook;
CVKBDlg			*m_pVKBDlgPtr;
CBinderList*	m_BinderList; // declare these properties globaly here because we want to handle hook
CColorPickerList* m_ColorPickerList; // declare these properties globaly here because we want to handle hook

//	This is the button map for the drawing tools toolbar
static short aToolsMap[] =	{	TMTB_FREEHAND,
								TMTB_LINE,
								TMTB_ARROW,
								TMTB_ELLIPSE,
								TMTB_RECTANGLE,
								TMTB_FILLEDELLIPSE,
								TMTB_FILLEDRECTANGLE,
								TMTB_POLYLINE,
								TMTB_POLYGON,
								TMTB_ANNTEXT,
								-1
							};
static RECT m_ScreenResolution;

//---------------------------------on---------------------------------------------
//	MAPS
//------------------------------------------------------------------------------
IMPLEMENT_DYNCREATE(CMainView, CFormView)

BEGIN_MESSAGE_MAP(CMainView, CFormView)
	//{{AFX_MSG_MAP(CMainView)
	ON_WM_CTLCOLOR()
	ON_MESSAGE(WM_IDLEUPDATECMDUI, OnIdleUpdateCmdUI)
	ON_WM_SIZE()
	ON_COMMAND(ID_ZOOM, OnZoom)
	ON_COMMAND(ID_ROTATE_CCW, OnRotateCcw)
	ON_COMMAND(ID_ROTATE_CW, OnRotateCw)
	ON_COMMAND(ID_NORMAL, OnNormal)
	ON_COMMAND(ID_REDACT, OnRedact)
	ON_COMMAND(ID_ERASE, OnErase)
	ON_COMMAND(ID_DRAW, OnDrawTool)
	ON_COMMAND(ID_CONFIG, OnConfig)
	ON_COMMAND(ID_HIGHLIGHT, OnHighlight)
	ON_COMMAND(ID_NEXT, OnNextPage)
	ON_COMMAND(ID_PLAY, OnPlay)
	ON_WM_CREATE()
	ON_COMMAND(ID_CLEAR, OnClear)
	ON_WM_TIMER()
	ON_COMMAND(ID_ZAP_FIRST, OnFirstZap)
	ON_COMMAND(ID_ZAP_LAST, OnLastZap)
	ON_COMMAND(ID_ZAP_NEXT, OnNextZap)
	ON_COMMAND(ID_ZAP_PREVIOUS, OnPreviousZap)
	ON_COMMAND(ID_ZAP_SAVE, OnSaveZap)
	ON_COMMAND(ID_ZAP_SAVE_SPLIT, OnSaveSplitZap)
	ON_COMMAND(ID_FILE_PRINT, OnFilePrint)
	ON_COMMAND(ID_CALLOUT, OnCallout)
	ON_COMMAND(ID_PAN, OnPan)
	ON_COMMAND(ID_ZOOMWIDTH, OnZoomWidth)
	ON_COMMAND(ID_BACKDESIGNATION, OnBackDesignation)
	ON_COMMAND(ID_BACKMOVIE, OnBackMovie)
	ON_COMMAND(ID_FIRSTDESIGNATION, OnFirstDesignation)
	ON_COMMAND(ID_FWDDESIGNATION, OnFwdDesignation)
	ON_COMMAND(ID_FWDMOVIE, OnFwdMovie)
	ON_COMMAND(ID_LASTDESIGNATION, OnLastDesignation)
	ON_COMMAND(ID_NEXTDESIGNATION, OnNextDesignation)
	ON_COMMAND(ID_STARTMOVIE, OnStartMovie)
	ON_COMMAND(ID_ENDMOVIE, OnEndMovie)
	ON_COMMAND(ID_STARTDESIGNATION, OnStartDesignation)
	ON_COMMAND(ID_TMTOOL, OnToolbars)
	ON_COMMAND(ID_DRAWBLACK, OnBlack)
	ON_COMMAND(ID_DRAWBLUE, OnBlue)
	ON_COMMAND(ID_DRAWGREEN, OnGreen)
	ON_COMMAND(ID_DRAWRED, OnRed)
	ON_COMMAND(ID_DRAWYELLOW, OnYellow)
	ON_COMMAND(ID_SPLIT_VERTICAL, OnSplitVertical)
	ON_COMMAND(ID_SPLIT_HORIZONTAL, OnSplitHorizontal)
	ON_COMMAND(ID_DISABLELINKS, OnDisableLinks)
	ON_COMMAND(ID_DRAWWHITE, OnWhite)
	ON_COMMAND(ID_FIRSTPAGE, OnFirstPage)
	ON_COMMAND(ID_LASTPAGE, OnLastPage)
	ON_COMMAND(ID_FILTERPROPS, OnFilterProps)
	ON_COMMAND(ID_SETPAGELINE, OnSetLine)
	ON_COMMAND(ID_SETPAGELINENEXT, OnSetNextLine)
	ON_COMMAND(ID_SELECT, OnSelect)
	ON_COMMAND(ID_PLAYTHROUGH, OnPlayThrough)
	ON_COMMAND(ID_VIDEOCAPTION, OnVideoCaption)
	ON_COMMAND(ID_DELETEANN, OnDeleteAnn)
	ON_COMMAND(ID_TMAX_HELP, OnTMaxHelp)
	ON_COMMAND(ID_TEXT, OnToggleScrollText)
	ON_COMMAND(ID_SELECTTOOL, OnSelectTool)
	ON_COMMAND(ID_FREEHAND, OnFreehand)
	ON_COMMAND(ID_LINE, OnLine)
	ON_COMMAND(ID_ARROW, OnArrow)
	ON_COMMAND(ID_FILLEDELLIPSE, OnFilledEllipse)
	ON_COMMAND(ID_FILLEDRECTANGLE, OnFilledRectangle)
	ON_COMMAND(ID_ELLPSE, OnEllipse)
	ON_COMMAND(ID_RECTANGLE, OnRectangle)
	ON_COMMAND(ID_FULLSCREEN, OnFullScreen)
	ON_COMMAND(ID_STATUSBAR, OnStatusBar)
	ON_COMMAND(ID_DRAWDARKBLUE, OnDarkBlue)
	ON_COMMAND(ID_DRAWDARKGREEN, OnDarkGreen)
	ON_COMMAND(ID_DRAWDARKRED, OnDarkRed)
	ON_COMMAND(ID_DRAWLIGHTBLUE, OnLightBlue)
	ON_COMMAND(ID_DRAWLIGHTGREEN, OnLightGreen)
	ON_COMMAND(ID_DRAWLIGHTRED, OnLightRed)
	ON_COMMAND(ID_POLYGON, OnPolygon)
	ON_COMMAND(ID_POLYLINE, OnPolyline)
	ON_COMMAND(ID_ANNTEXT, OnAnnText)
	ON_COMMAND(ID_NEXTMEDIA, OnNextMedia)
	ON_COMMAND(ID_PREVMEDIA, OnPreviousMedia)
	ON_COMMAND(ID_FIRST_SHOW_ITEM, OnFirstShowItem)
	ON_COMMAND(ID_LAST_SHOW_ITEM, OnLastShowItem)
	ON_COMMAND(ID_NEXT_SHOW_ITEM, OnNextShowItem)
	ON_COMMAND(ID_PREVIOUS, OnPreviousPage)
	ON_COMMAND(ID_PREVDESIGNATION, OnPreviousDesignation)
	ON_COMMAND(ID_PREV_SHOW_ITEM, OnPreviousShowItem)
	ON_COMMAND(ID_MOUSE_MODE, OnMouseMode)
	ON_COMMAND(ID_SHOW_TOOLBAR, OnShowToolbar)
	ON_COMMAND(ID_ZOOM_RESTRICTED, OnZoomRestricted)
	ON_COMMAND(ID_HORIZONTAL_NEXT, OnNextPageHorizontal)
	ON_COMMAND(ID_SCREEN_CAPTURE, OnScreenCapture)
	ON_WM_DESTROY()
	ON_COMMAND(ID_SHADEONCALLOUT, OnShadeOnCallout)
	ON_COMMAND(ID_CAPTURE_BARCODES, OnCaptureBarcodes)
	ON_COMMAND(ID_ZAP_UPDATE, OnUpdateZap)
	ON_COMMAND(TMAX_NEXTPAGE_VERTICAL, OnNextPageVertical)

	//}}AFX_MSG_MAP
	ON_MESSAGE(WM_MOUSEMODE, OnWMMouseMode)
	ON_MESSAGE(WM_GRABFOCUS, OnWMGrabFocus)
	ON_MESSAGE(WM_GESTURE, OnGesture)
	ON_WM_PAINT()
	END_MESSAGE_MAP()

BEGIN_EVENTSINK_MAP(CMainView, CFormView)
    //{{AFX_EVENTSINK_MAP(CMainView)
	ON_EVENT(CMainView, IDC_TMVIEWCTRL, 3 /* CreateCallout */, OnAxCreateCallout, VTS_I4)
	ON_EVENT(CMainView, IDC_TMVIEWCTRL, 4 /* DestroyCallout */, OnAxDestroyCallout, VTS_I4)
	ON_EVENT(CMainView, IDC_TMVIEWCTRL, 5 /* SelectPane */, OnChangePane, VTS_I2)
	ON_EVENT(CMainView, IDC_TMVIEWCTRL, 6 /* OpenTextBox */, OnAxOpenTextBox, VTS_I2)
	ON_EVENT(CMainView, IDC_TMVIEWCTRL, 7 /* CloseTextBox */, OnAxCloseTextBox, VTS_I2)
	ON_EVENT(CMainView, IDC_TMVIEWCTRL, 9 /* StartTextEdit */, OnAxStartTextEdit, VTS_I2)
	ON_EVENT(CMainView, IDC_TMVIEWCTRL, 10 /* StopTextEdit */, OnAxStopTextEdit, VTS_I2)

	ON_EVENT(CMainView, IDC_TMVIEWCTRL2, 3 /* CreateCallout */, OnAxCreateCallout, VTS_I4)
	ON_EVENT(CMainView, IDC_TMVIEWCTRL2, 4 /* DestroyCallout */, OnAxDestroyCallout, VTS_I4)
	ON_EVENT(CMainView, IDC_TMVIEWCTRL2, 5 /* SelectPane */, OnChangePane, VTS_I2)
	ON_EVENT(CMainView, IDC_TMVIEWCTRL2, 6 /* OpenTextBox */, OnAxOpenTextBox, VTS_I2)
	ON_EVENT(CMainView, IDC_TMVIEWCTRL2, 7 /* CloseTextBox */, OnAxCloseTextBox, VTS_I2)
	ON_EVENT(CMainView, IDC_TMVIEWCTRL2, 9 /* StartTextEdit */, OnAxStartTextEdit, VTS_I2)
	ON_EVENT(CMainView, IDC_TMVIEWCTRL2, 10 /* StopTextEdit */, OnAxStopTextEdit, VTS_I2)

	ON_EVENT(CMainView, IDC_TMVIEWCTRL3, 3 /* CreateCallout */, OnAxCreateCallout, VTS_I4)
	ON_EVENT(CMainView, IDC_TMVIEWCTRL3, 4 /* DestroyCallout */, OnAxDestroyCallout, VTS_I4)
	ON_EVENT(CMainView, IDC_TMVIEWCTRL3, 5 /* SelectPane */, OnChangePane, VTS_I2)
	ON_EVENT(CMainView, IDC_TMVIEWCTRL3, 6 /* OpenTextBox */, OnAxOpenTextBox, VTS_I2)
	ON_EVENT(CMainView, IDC_TMVIEWCTRL3, 7 /* CloseTextBox */, OnAxCloseTextBox, VTS_I2)
	ON_EVENT(CMainView, IDC_TMVIEWCTRL3, 9 /* StartTextEdit */, OnAxStartTextEdit, VTS_I2)
	ON_EVENT(CMainView, IDC_TMVIEWCTRL3, 10 /* StopTextEdit */, OnAxStopTextEdit, VTS_I2)

	ON_EVENT(CMainView, IDC_DOCUMENTS, 1 /* ButtonClick */, OnAxButtonClick, VTS_I2 VTS_BOOL)
	ON_EVENT(CMainView, IDC_DOCUMENTS_LARGE, 1 /* ButtonClick */, OnAxButtonClickLarge, VTS_I2 VTS_BOOL)
	ON_EVENT(CMainView, IDC_TMSTATCTRL, -600 /* Click */, OnAxClickStatusBar, VTS_NONE)
	ON_EVENT(CMainView, IDC_TMLPENCTRL, 1 /* MouseClick */, OnAxClickLightPen, VTS_I2 VTS_I2)
	ON_EVENT(CMainView, IDC_TMPOWERCTRL, 4 /* ViewFocus */, OnAxPowerFocus, VTS_I2)
	ON_EVENT(CMainView, IDC_TMMOVIECTRL, 2 /* StateChange */, OnAxStateChange, VTS_I2)
	ON_EVENT(CMainView, IDC_TMMOVIECTRL, 3 /* PlaylistState */, OnAxPlaylistState, VTS_I2)
	ON_EVENT(CMainView, IDC_TMMOVIECTRL, 4 /* PlaybackError */, OnAxPlaybackError, VTS_I4 VTS_BOOL)
	ON_EVENT(CMainView, IDC_TMMOVIECTRL, 7 /* LineChange */, OnAxLineChange, VTS_I4)
	ON_EVENT(CMainView, IDC_TMMOVIECTRL, 8 /* PlaylistTime */, OnAxPlaylistTime, VTS_R8)
	ON_EVENT(CMainView, IDC_TMMOVIECTRL, 9 /* DesignationTime */, OnAxDesignationTime, VTS_R8)
	ON_EVENT(CMainView, IDC_TMMOVIECTRL, 10 /* ElapsedTimes */, OnAxElapsedTimes, VTS_R8 VTS_R8)
	ON_EVENT(CMainView, IDC_TMMOVIECTRL, 11 /* DesignationChange */, OnAxDesignationChange, VTS_I4 VTS_I4)
	ON_EVENT(CMainView, IDC_TMMOVIECTRL, 12 /* LinkChange */, OnAxLinkChange, VTS_BSTR VTS_I4 VTS_I4)
	ON_EVENT(CMainView, IDC_TMMOVIECTRL, 14 /* PositionChange */, OnAxPositionChange, VTS_R8)
	ON_EVENT(CMainView, IDC_TMSHARE, 1 /* CommandRequest */, OnAxManagerRequest, VTS_NONE)
	ON_EVENT(CMainView, IDC_TMSHARE, 4 /* CommandResponse */, OnAxManagerResponse, VTS_NONE)
	//}}AFX_EVENTSINK_MAP
	ON_EVENT(CMainView, IDC_GRAPHICS, 1 /* ButtonClick */, OnAxButtonClick, VTS_I2 VTS_BOOL)
	ON_EVENT(CMainView, IDC_LINKS, 1 /* ButtonClick */, OnAxButtonClick, VTS_I2 VTS_BOOL)
	ON_EVENT(CMainView, IDC_MOVIES, 1 /* ButtonClick */, OnAxButtonClick, VTS_I2 VTS_BOOL)
	ON_EVENT(CMainView, IDC_PLAYLISTS, 1 /* ButtonClick */, OnAxButtonClick, VTS_I2 VTS_BOOL)
	ON_EVENT(CMainView, IDC_TEXT, 1 /* ButtonClick */, OnAxButtonClick, VTS_I2 VTS_BOOL)
	ON_EVENT(CMainView, IDC_DRAWINGTOOLS, 1 /* ButtonClick */, OnAxButtonClick, VTS_I2 VTS_BOOL)
	ON_EVENT(CMainView, IDC_POWERPOINT, 1 /* ButtonClick */, OnAxButtonClick, VTS_I2 VTS_BOOL)
	ON_EVENT(CMainView, IDC_LINKPOWER, 1 /* ButtonClick */, OnAxButtonClick, VTS_I2 VTS_BOOL)
END_EVENTSINK_MAP()

//==============================================================================
//
// 	Function Name:	CMainView::AllocDatabase()
//
// 	Description:	This function will allocate the appropriate database object
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::AllocDatabase(LPCSTR lpszFolder)
{	
	CString strFileSpec = "";

	//	Make sure the existing database is closed
	CloseDatabase();
	
	//	Build the file specification for a .NET database
	strFileSpec = lpszFolder;
	if(strFileSpec.Right(1) != "\\")
		strFileSpec += "\\";
	strFileSpec += DEFAULT_DBNET_FILENAME;

	//	Does the .NET file exist?
	if(FindFile(strFileSpec) == TRUE)
	{
		//	Allocate a .NET database
		m_pDatabase = new CDBNET();
	}
	else
	{
		//	Assume we are dealing with a 4.x,5.x database
		m_pDatabase = new CDB45();
	}

	return TRUE;
}
	
//==============================================================================
//
// 	Function Name:	CMainView::CaptureBarcode()
//
// 	Description:	Called to capture the loaded barcode
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::CaptureBarcode()
{
	//	Start the screen capture
	if(m_ctrlTMGrab.Capture() == 0)
	{
		m_ctrlTMGrab.Save(m_strCaptureTargetFile, 
						  75, // Format PNG = 75
						  24, // Bits per pixel
						  0,  // Quality
						  0); // Overwrite
	}

	//	Get the next barcode
	while(1)
	{
		if(LoadCaptureBarcode() == TRUE)
			break;
	}
	
	return TRUE;		
}

//==============================================================================
//
// 	Function Name:	CMainView::CheckLinkOptions()
//
// 	Description:	Called to check the current link options to see if a screen
//					update is required
//
// 	Returns:		TRUE if the screen should be updated
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::CheckLinkOptions()
{
	//	Does the split screen option match?
	if(m_AppLink.GetSplitScreen() == TRUE)
	{
		if(m_bSplitScreenLink == FALSE)
			return TRUE;
	}
	else
	{
		if(m_bSplitScreenLink == TRUE)
			return TRUE;
	}

	//	Should the scrolling text be visible?
	if(GetLinkedTextEnabled() != IsScrollTextVisible())
		return TRUE;

	//	Does the hide video option match?
	if(m_AppLink.GetHideVideo() == TRUE)
	{
		//	Is the video playback visible?
		if(IsVideoVisible() == TRUE)
			return TRUE;
	}
	else if(m_sVideoSize != VIDEO0)
	{
		if(IsVideoVisible() == FALSE)
			return TRUE;
	}

	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CMainView::ClearTMViewInactive()
//
// 	Description:	Called to clear the inactive pane of the TMView control
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::ClearTMViewInactive() 
{
	SMultipageInfo*	pInfo;
	short			sInactive;

	//	Get the identifier of the active and inactive panes
	sInactive = (m_ctrlTMView->GetActivePane() == TMV_LEFTPANE) ? TMV_RIGHTPANE : TMV_LEFTPANE;

	//	Clear out the inactive pane
	m_ctrlTMView->LoadFile(0, sInactive);
	
	//	Reset the media descriptor attached to the pane
	if((pInfo = (SMultipageInfo*)m_ctrlTMView->GetData(sInactive)) != NULL)
		ResetMultipage(pInfo);
}


//==============================================================================
//
// 	Function Name:	CMainView::CloseDatabase()
//
// 	Description:	This function will close the system database
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::CloseDatabase()
{
	//	Close the database
	if(m_pDatabase != 0)
	{
		m_pDatabase->Close();
		delete m_pDatabase;
		m_pDatabase = 0;
	}
	
	//	Flush the test lists
	if(m_Test.pPlaylists)
		m_Test.pPlaylists->Flush(FALSE);
	if(m_Test.pImages)
		m_Test.pImages->Flush(FALSE);
	if(m_Test.pShows)
		m_Test.pShows->Flush(FALSE);
	if(m_Test.pMovies)
		m_Test.pMovies->Flush(FALSE);
	if(m_Test.pPowerPoints)
		m_Test.pPowerPoints->Flush(FALSE);

	//	Reset 
	m_pMedia = 0;
	SetLastPlaylist(0);

	//	Clear out the barcodes buffer
	m_aBarcodes.Clear();

	//	NOTE:	We do not clear the m_strCaseFolder member because this function
	//			may be getting called from OpenDatabase()
}

//==============================================================================
//
// 	Function Name:	CMainView::CMainView()
//
// 	Description:	This is the constructor for CMainView objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMainView::CMainView() : CFormView(CMainView::IDD), m_pVKBDlg(NULL), m_ctrlTMView(NULL) 
{
	//{{AFX_DATA_INIT(CMainView)
	//}}AFX_DATA_INIT

	m_pDatabase = 0;
	m_pFrame = 0;
	m_pMedia = 0;
	m_paCaptureFiles = NULL;
	m_fptrCaptureBarcodes = NULL;
	m_brBackground.CreateSolidBrush(RGB(0, 0, 0));
	m_strAppFolder = "";
	m_strCaseFolder = "";
	m_strIniFile = "";
	m_strLastPlaylist = "";
	m_strHelpFileSpec = "";
	m_bPlaying = FALSE;
	m_bRedraw = TRUE;
	m_iCuePage = 0;
	m_iCueLine = 0;
	m_iLoadPage = 0;
	m_iLoadLine = 0;
	m_iHalfWidth = 0;
	m_iHalfHeight = 0;
	m_iMaxWidth = 0;
	m_iMaxHeight = 0;
	m_iCaptureFileIndex = 0;
	m_iCapturedBarcodes = 0;
	m_iUserSplitFrameColor = 0;
	m_iZapSplitFrameColor = 0;
	m_sState = S_CLEAR;
	m_sPrevState = S_CLEAR;
	m_sVideoSize = 0;
	m_sVideoPosition = 0;
	m_bUseSecondaryMonitor = FALSE;
	m_bCenterVideo = FALSE;
	m_bEnableErrors =  TRUE;
	m_bEnablePowerPoint = TRUE;
	m_bMouseMode = FALSE;
	m_bDoUpdates = TRUE;
	m_bResumePlaylist = TRUE;
	m_bClearPlaylist = FALSE;
	m_bResumeMovie = TRUE;
	m_bClearMovie = FALSE;
	m_bScaleGraphics = FALSE;
	m_bScaleDocs = TRUE;
	m_bScaleAVI = TRUE;
	m_bScalePlaylists = TRUE;
	m_bClipsAsPlaylists = FALSE;
	m_bRunToEnd = TRUE;
	m_bCreateDocuments = FALSE;
	m_bEditTextAnn = FALSE;
	m_bSystemScrollEnabled = TRUE;
	m_bUserScrollEnabled = TRUE;
	m_bEnablePlay = FALSE;
	m_bCuedMovie = FALSE;
	m_bDisableLinks = FALSE;
	m_bLoadingPlaylist = FALSE;
	m_bPenControls = FALSE;
	m_bRestoreToolbar = FALSE;
	m_bSplitScreenDocuments = TRUE;
	m_bSplitScreenGraphics = FALSE;
	m_bSplitScreenPowerPoints = TRUE;
	m_bSplitScreenLink = FALSE;
	m_bClassicLinks = TRUE;
	m_bLoadingShowItem = FALSE;
	m_bLoadingSplitZap = FALSE;
	m_bZapSplitScreen = FALSE;
	m_sUpdateButton = 0;
	m_lCueTranscript = 0;
	m_fPlaylistStep = 5.0f;
	m_fMovieStep = 5.0f;
	m_dFrameRate = 29.97;
	m_dMinMovieCue = 0.0;
	m_dMaxMovieCue = 0.0;
	m_pToolbar = 0;
	m_iImageSecondary = 0;
	m_iAnimationSecondary = 0;
	m_iPlaylistSecondary = 0;
	m_iPowerPointSecondary = 0;
	m_iCustomShowSecondary = 0;
	m_iTreatmentTertiary = 0;
	m_bTabletMode = FALSE;
	m_bGestureHandled = FALSE;
	m_gestureStartTime = 0;
	m_BinderList = 0;
	m_ColorPickerList = 0;
	m_bIsBinderOpen = FALSE;
	m_bIsColorPickerOpen = FALSE;
	m_cVKChar = KEYBOARD_VKCODE;
	m_cPrimaryBarcodeChar = KEYBOARD_PRIMARY_BARCODE;
	m_cAlternateBarcodeChar = KEYBOARD_ALTERNATE_BARCODE;	
	ZeroMemory(&m_Hotkeys, sizeof(m_Hotkeys));
	ZeroMemory(m_aToolbars, sizeof(m_aToolbars));
	ZeroMemory(&m_rcView, sizeof(m_rcView));
	ZeroMemory(&m_rcMovie, sizeof(m_rcMovie));
	ZeroMemory(&m_rcText, sizeof(m_rcText));
	ZeroMemory(&m_rcPower, sizeof(m_rcPower));
	ZeroMemory(&m_rcStatus, sizeof(m_rcStatus));
	ZeroMemory(&m_PlaylistStatus, sizeof(m_PlaylistStatus));
	ZeroMemory(&m_TMPower1, sizeof(m_TMPower1));
	ZeroMemory(&m_TMPower2, sizeof(m_TMPower2));
	ZeroMemory(&m_TMMovie, sizeof(m_TMMovie));
	ZeroMemory(&m_Playlist, sizeof(m_Playlist));
	ZeroMemory(&m_Show, sizeof(m_Show));
	ZeroMemory(&m_Test, sizeof(m_Test));
	ZeroMemory(&m_ControlBar, sizeof(m_ControlBar));
	ZeroMemory(&m_ControlBarExtra, sizeof(m_ControlBarExtra));
	::GetWindowRect(::GetDesktopWindow(), &m_ScreenResolution);

	for(int i=0; i < SZ_ARR_TM_VW; i++) {

		CTm_view *pTmView = new CTm_view();
		m_arrTmView[i] = pTmView;
		hasPage[i] = false;
	}

	curIndexView = 1;
	m_ctrlTMView = m_arrTmView[curIndexView];
	hasPage[curIndexView] = true;

	toolbarForcedHidden = false;
	loadNextInOtherPanes = false;
	curPageNavCount = 0;
	countFrom = COUNT_FROM_CUR;
	scaleHist.clear();
	zoomFullWidth = false;
}

//==============================================================================
//
// 	Function Name:	CMainView::~CMainView()
//
// 	Description:	This is the destructor for CMainView objects.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CMainView::~CMainView()
{
	//	Flush the test lists
	if(m_Test.pPlaylists)
		delete m_Test.pPlaylists;
	if(m_Test.pImages)
		delete m_Test.pImages;
	if(m_Test.pShows)
		delete m_Test.pShows;
	if(m_Test.pMovies)
		delete m_Test.pMovies;
	if(m_Test.pPowerPoints)
		delete m_Test.pPowerPoints;

	//	Make sure the task bar is visible. 
	//
	//	NOTE:	We do this here so that the task bar gets restored even if the
	//			application crashes
	SetTaskBarVisible(TRUE);
}

//==============================================================================
//
// 	Function Name:	CMainView::CueMovie()
//
// 	Description:	This function will request that the TMMovie control 
//					adjust the position of the current movie.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::CueMovie(short sMove, BOOL bForward)
{
	double dStep;

	//	Calculate the number of frames we have to move
	dStep = m_fMovieStep; // MOD 6.0
	if(!bForward)
		dStep *= -1;

	//	Call the TMMovie control
	m_ctrlTMMovie.Cue(sMove, dStep, m_bResumeMovie);
	m_bCuedMovie = TRUE;
}

//==============================================================================
//
// 	Function Name:	CMainView::CuePlaylist()
//
// 	Description:	This function will request that the TMMovie control 
//					adjust the position of the current playlist.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::CuePlaylist(short sMove, BOOL bForward)
{
	float fStep;

	//	Are we cueing forward?
	fStep = (bForward) ? m_fPlaylistStep : (m_fPlaylistStep * -1.0f);

	//	Call the TMMovie control
	m_ctrlTMMovie.CuePlaylist(sMove, fStep, m_bResumePlaylist, m_bRunToEnd);
}

//==============================================================================
//
// 	Function Name:	CMainView::DbgMsg()
//
// 	Description:	This function logs a debug message.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::DbgMsg(LPCSTR lpszFormat, ...)
{
#ifdef _DEBUG

	char  szMessage[2048];
	FILE* fptr = NULL;

	fopen_s(&fptr, "c:\\tmaxPresentation_debug.txt", "at");
	
	if(fptr != NULL)
	{	
		//	Declare the variable list of arguements            
		va_list	Arguements;

		//	Insert the first variable arguement into the arguement list
		va_start(Arguements, lpszFormat);

		//	Format the message
		vsprintf_s(szMessage, sizeof(szMessage), lpszFormat, Arguements);

		//	Clean up the arguement list
		va_end(Arguements);

		fprintf(fptr, "%s\n", szMessage);
		fclose(fptr);

	}

#endif //  _DEBUG

}

//==============================================================================
//
// 	Function Name:	CMainView::DbgMsg()
//
// 	Description:	This function logs a debug message.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::DbgMsg(SMultipageInfo* pInfo, LPCSTR lpszFormat, ...)
{
#ifdef _DEBUG

	char	szMessage[2048];
	CString	strBarcode = "";
	FILE*	fptr = NULL;

	fopen_s(&fptr, "c:\\tmaxPresentation_debug.txt", "at");
	
	if(fptr != NULL)
	{	
		if(pInfo != NULL)
			strBarcode = GetBarcode(*pInfo);

		//	Declare the variable list of arguements            
		va_list	Arguements;

		//	Insert the first variable arguement into the arguement list
		va_start(Arguements, lpszFormat);

		//	Format the message
		vsprintf_s(szMessage, sizeof(szMessage), lpszFormat, Arguements);

		//	Clean up the arguement list
		va_end(Arguements);

		fprintf(fptr, "%s -> %s\n", szMessage, strBarcode);
		fclose(fptr);

	}

#endif //  _DEBUG

}

//==============================================================================
//
// 	Function Name:	CMainView::DoDataExchange()
//
// 	Description:	This function manages the exchange of data between the class
//					members and dialog box controls.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CMainView)
	DDX_Control(pDX, IDC_TMVIEWCTRL, *m_arrTmView[0]);
	DDX_Control(pDX, IDC_TMVIEWCTRL2, *m_arrTmView[1]);
	DDX_Control(pDX, IDC_TMVIEWCTRL3, *m_arrTmView[2]);
	DDX_Control(pDX, IDC_DOCUMENTS, m_ctrlTBDocuments);
	DDX_Control(pDX, IDC_DOCUMENTS_LARGE, m_ctrlTBDocumentsLarge);
	DDX_Control(pDX, IDC_GRAPHICS, m_ctrlTBGraphics);
	DDX_Control(pDX, IDC_LINKS, m_ctrlTBLinks);
	DDX_Control(pDX, IDC_MOVIES, m_ctrlTBMovies);
	DDX_Control(pDX, IDC_PLAYLISTS, m_ctrlTBPlaylists);
	DDX_Control(pDX, IDC_TMMOVIECTRL, m_ctrlTMMovie);
	DDX_Control(pDX, IDC_TMTEXTCTRL, m_ctrlTMText);
	DDX_Control(pDX, IDC_TEXT, m_ctrlTBText);
	DDX_Control(pDX, IDC_DRAWINGTOOLS, m_ctrlTBTools);
	DDX_Control(pDX, IDC_TMSTATCTRL, m_ctrlTMStat);
	DDX_Control(pDX, IDC_TMLPENCTRL, m_ctrlTMLpen);
	DDX_Control(pDX, IDC_POWERPOINT, m_ctrlTBPowerPoint);
	DDX_Control(pDX, IDC_TMPOWERCTRL, m_ctrlTMPower);
	DDX_Control(pDX, IDC_LINKPOWER, m_ctrlTBLinkPP);
	DDX_Control(pDX, IDC_TMGRABCTRL, m_ctrlTMGrab);
	DDX_Control(pDX, IDC_TMSHARE, m_ctrlManagerApp);

	//}}AFX_DATA_MAP
}

//==============================================================================
//
// 	Function Name:	CMainView::FindFile()
//
// 	Description:	This function checks to see if the file exists.
//
// 	Returns:		TRUE if the file exists.
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::FindFile(LPCSTR lpFile)
{
	WIN32_FIND_DATA	Find;
	HANDLE			hFind;

	if((hFind = FindFirstFile(lpFile, &Find)) == INVALID_HANDLE_VALUE)
	{
		return FALSE;
	}
	else
	{
		//	Close the file find handle
		FindClose(hFind);
		return TRUE;
	}	
}

//==============================================================================
//
// 	Function Name:	CMainView::FormatError()
//
// 	Description:	This function will format an error message using the 
//					caller's parameters
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::FormatError(CString& rString, UINT uId, LPCSTR lpArg1, 
						  LPCSTR lpArg2) 
{
	//	Format the error message
	if(lpArg2 != 0)
		AfxFormatString2(rString, uId, lpArg1, lpArg2);
	else if(lpArg1 != 0)
		AfxFormatString1(rString, uId, lpArg1);
	else
		rString.LoadString(uId);
}

//==============================================================================
//
// 	Function Name:	CMainView::GetBarcode()
//
// 	Description:	Called to get the barcode for the specified record
//
// 	Returns:		The barcode for the specified record
//
//	Notes:			None
//
//==============================================================================
CString CMainView::GetBarcode(CMedia* pPrimary, CSecondary* pSecondary, CTertiary* pTertiary)
{
	CString strBarcode = "";

	if(pPrimary != NULL)
	{
		if(pSecondary != NULL)
		{
			if(pTertiary != NULL)
				strBarcode.Format("%s.%ld.%ld", pPrimary->m_strMediaId, pSecondary->m_lBarcodeId, pTertiary->m_lBarcodeId);
			else
				strBarcode.Format("%s.%ld", pPrimary->m_strMediaId, pSecondary->m_lBarcodeId);
		}
		else
		{
				strBarcode.Format("%s", pPrimary->m_strMediaId);
		
		}// if(pSecondary != NULL)

	}// if(pPrimary != NULL)

	return strBarcode;
}

//==============================================================================
//
// 	Function Name:	CMainView::GetBarcode()
//
// 	Description:	Called to get the barcode for the record bound to the 
//					specified multipage data.
//
// 	Returns:		The barcode for the specified record
//
//	Notes:			None
//
//==============================================================================
CString CMainView::GetBarcode(SMultipageInfo& rMultipageInfo)
{
	return GetBarcode(rMultipageInfo.pMultipage, rMultipageInfo.pSecondary, 
					  rMultipageInfo.pTertiary);
}

//==============================================================================
//
// 	Function Name:	CMainView::GetCaptureSource()
//
// 	Description:	This function is called to populate the array with the
//					names of all capture source files in the specified folder
//
// 	Returns:		TRUE if at least one file is found
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::GetCaptureSource(LPCSTR lpFolder, CStringArray& aFiles)
{
	WIN32_FIND_DATA	FindData;
	HANDLE			hFind;
	CString			strSearch;

	//	Build the search specification
	strSearch = lpFolder;
	if(strSearch.Right(1) != "\\")
		strSearch += "\\*.txt";
	else
		strSearch += "*.txt";

	//	Are there any files or folders?
	if((hFind = FindFirstFile(strSearch, &FindData)) == INVALID_HANDLE_VALUE)
		return FALSE;	
	
	//	Add all the files and folders
	while(1)
	{
		//	Is this a file?
		if((FindData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) == 0)
		{	
			aFiles.Add(FindData.cFileName);
		}

		//	Get the next file
		if(!FindNextFile(hFind, &FindData))
			break;

	} // while(1)

	CloseHandle(hFind);

	return (aFiles.GetUpperBound() >= 0);
}

//==============================================================================
//
// 	Function Name:	CMainView::GetDocument()
//
// 	Description:	This function is called to get a pointer to the document
//					object.
//
// 	Returns:		A pointer to the document object.
//
//	Notes:			The non-debug version of this function is inline.
//
//==============================================================================
#ifdef _DEBUG
CTMDocument* CMainView::GetDocument() 
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CTMDocument)));
	return (CTMDocument*)m_pDocument;
}
#endif 

//==============================================================================
//
// 	Function Name:	CMainView::GetLastPlaylist()
//
// 	Description:	This function will retrieve the last playlist to have been
//					run in this session.
//
// 	Returns:		A pointer to the playlist object if successful
//
//	Notes:			The caller is responsible for deallocation of the object
//
//==============================================================================
CPlaylist* CMainView::GetLastPlaylist()
{
	CMedia*		pMedia = 0;
	CPlaylist*	pPlaylist = 0;

	//	Do we have a media id?
	if(m_strLastPlaylist.IsEmpty())
		return 0;

	if((m_pDatabase == 0) || (m_pDatabase->IsOpen() == FALSE))
		return 0;

	//	Get the basic media information for this playlist
	if((pMedia = m_pDatabase->GetMedia(m_strLastPlaylist)) != 0)
	{
		//	Now get the playlist object
		pPlaylist = m_pDatabase->GetPlaylist(pMedia);
	}

	//	Clear the last playlist information if it is invalid
	if(pPlaylist == 0)
		m_strLastPlaylist.Empty();

	return pPlaylist;
}

//==============================================================================
//
// 	Function Name:	CMainView::GetLinkedTextEnabled()
//
// 	Description:	This function is called to determine if the scrolling text
//					should be enabled when displaying a linked image or slide
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::GetLinkedTextEnabled() 
{
	//	Have to have an active designation to have text
	if(m_Playlist.pDesignation == NULL)
		return FALSE;

	//	Can't enable it if not enabled for the parent designation
	if(GetScrollTextEnabled(m_Playlist.pDesignation) == FALSE)
		return FALSE;

	//	Is this a hidden link?
	if(m_AppLink.GetHideLink() == TRUE)
	{
		//	Always enable the scrolling text if in classic mode
		if(m_bClassicLinks == TRUE)
			return TRUE;
	}
	else
	{
		//	Always disable the scrolling text if in classic mode
		if(m_bClassicLinks == TRUE)
			return FALSE;
	}

	//	Use the link's flag
	return (m_AppLink.GetHideText() == FALSE);
}

//==============================================================================
//
// 	Function Name:	CMainView::GetLinkedVideoSize()
//
// 	Description:	Called to get the size required for the video playback
//					window when linking an image or PowerPoint presentation
//
// 	Returns:		TRUE if the video is visible
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::GetLinkedVideoSize(SIZE& rSize)   
{
	//	Is video disabled by the link?
	if(m_AppLink.GetHideVideo() == TRUE)
	{
		rSize.cx = 0;
		rSize.cy = 0;
	}
	else
	{
		//	What size video is being used?
		switch(m_sVideoSize)
		{
			case VIDEO0:

				rSize.cx = 0;
				rSize.cy = 0;
				break;

			case VIDEO6:

				rSize.cx = m_iMaxWidth / 3;
				rSize.cy = m_iMaxHeight / 3;
				break;

			case VIDEO8:

				rSize.cx = m_iMaxWidth / 4;
				rSize.cy = m_iMaxHeight / 4;
				break;

			case VIDEO4:
			default:

				rSize.cx = m_iMaxWidth / 2;
				rSize.cy = m_iMaxHeight / 2;
				break;
		
		}// switch(m_sVideoSize)

	}// if(m_AppLink.GetHideVideo() == TRUE)

	return ((rSize.cx > 0) && (rSize.cy > 0));
}

//==============================================================================
//
// 	Function Name:	CMainView::GetLinkState()
//
// 	Description:	This function is called to determine if the media being
//					loaded should be linked to the active video
//
// 	Returns:		TRUE if the media should be linked
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::GetLinkState()
{
	//	Always link if this is being called as a result of a link event
	//
	//	NOTE:	If the user cued the playlist we may get a link event even
	//			though the video is not actually playing yet
	if(m_AppLink.GetIsEvent() == TRUE)
		return TRUE;

	//	Can't link if we're not playing
	if(!m_bPlaying)
		return FALSE;

	//	Is this a custom show item?
	if(m_bLoadingShowItem)
	{
		return FALSE;
	}
	else
	{
		//	Go ahead and link the media
		return TRUE;
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::GetMultipageInfo()
//
// 	Description:	This function is called to get a pointer to the multipage 
//					information structure to be used for the specified screen
//					state
//
// 	Returns:		A pointer to the information structure
//
//	Notes:			None
//
//==============================================================================
SMultipageInfo* CMainView::GetMultipageInfo(short sState) 
{
	SMultipageInfo* pInfo;

	//	What is the screen state?
	switch(sState)
	{
		case S_DOCUMENT:
		case S_GRAPHIC:		
		case S_LINKEDIMAGE:

			//	Get the media object attached to the active pane
			pInfo = (SMultipageInfo*)m_ctrlTMView->GetData(TMV_ACTIVEPANE);

			//	This is just to keep the app from crashing. It should never 
			//	happen once initialized
			if(pInfo == NULL)
				pInfo = new SMultipageInfo();
			
			break; 
		
		case S_POWERPOINT:
		case S_LINKEDPOWER:

			//	Get the media object attached to the active pane
			pInfo = (SMultipageInfo*)m_ctrlTMPower.GetData(TMV_ACTIVEPANE);

			if(pInfo == NULL)
				pInfo = &m_TMPower1;
			
			break; 
		
		case S_PLAYLIST:
		case S_TEXT:
		case S_MOVIE:

			pInfo = &m_TMMovie;
			break;

		default:				

			pInfo = new SMultipageInfo();
			break;
	}

	return pInfo;
}

//==============================================================================
//
// 	Function Name:	CMainView::GetScrollTextEnabled()
//
// 	Description:	This function is called to determine if the text associated
//					with the specified designation should be scrolled on screen
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::GetScrollTextEnabled(CDesignation* pDesignation) 
{
	if(pDesignation == NULL) return FALSE;

	//	Has the user turned off the system level flag for scrolling text?
	if(m_bSystemScrollEnabled == FALSE)
		return FALSE;

	//	Is scrolling text disabled for this designation?
	if(pDesignation->GetScrollTextEnabled() == FALSE)
		return FALSE;

	//	Has the user turned off scrolling text for this playlist?
	if(m_bUserScrollEnabled == FALSE)
		return FALSE;

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CMainView::GetSplitPageInfo()
//
// 	Description:	This function is called to get the page to be activated
//					when performing a split page next or split page previous
//					operation
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::GetSplitPageInfo(SMultipageInfo* pInfo, BOOL bPrevious)
{
	SMultipageInfo*	pLInfo = NULL;
	SMultipageInfo*	pRInfo = NULL;
	CSecondary*		pLPage = NULL;
	CSecondary*		pRPage = NULL;
	CSecondary*		pSplitSecondary = NULL;
	CMultipage*		pSplitMultipage = NULL;

	//	Are we in split screen mode?
	if(m_ctrlTMView->GetSplitScreen() == TRUE)
	{
		//	Get the information bound to each pane
		if((pLInfo = (SMultipageInfo*)m_ctrlTMView->GetData(TMV_LEFTPANE)) != NULL)
			pLPage = pLInfo->pSecondary;
		if((pRInfo = (SMultipageInfo*)m_ctrlTMView->GetData(TMV_RIGHTPANE)) != NULL)
			pRPage = pRInfo->pSecondary;

		//	Get the record required to perform the operation
		if(pLPage != NULL)
		{
			//	Are both panes occupied?
			if(pRPage != NULL)
			{
				//	If both panes are loaded they must be adjacent pages in the same document
				if((pLPage->m_lPrimaryId == pRPage->m_lPrimaryId) && (pLPage->m_lPlaybackOrder == (pRPage->m_lPlaybackOrder - 1)))
				{
					pSplitMultipage = pLInfo->pMultipage;

					if(bPrevious == TRUE)
						pSplitSecondary = pLInfo->pMultipage->m_Pages.FindPrev(pLInfo->pSecondary);
					else
						pSplitSecondary = pRInfo->pMultipage->m_Pages.FindNext(pRInfo->pSecondary);

				}
				else
				{
					//CString M;
					//M.Format("L Primary: %ld\nL Page: %ld\nR Primary: %ld\nR Page: %ld",
					//		  pLPage->m_lPrimaryId, pLPage->m_lPlaybackOrder,
					//		  pRPage->m_lPrimaryId, pRPage->m_lPlaybackOrder);
					//MessageBox(M);
				}

			}
			else
			{
				pSplitMultipage = pLInfo->pMultipage;

				if(bPrevious == TRUE)
					pSplitSecondary = pLInfo->pMultipage->m_Pages.FindPrev(pLInfo->pSecondary);
				else
					pSplitSecondary = pLInfo->pMultipage->m_Pages.FindNext(pLInfo->pSecondary);
			}

		}
		else if(pRPage != NULL)
		{
			pSplitMultipage = pRInfo->pMultipage;

			if(bPrevious == TRUE)
				pSplitSecondary = pRInfo->pMultipage->m_Pages.FindPrev(pRInfo->pSecondary);
			else
				pSplitSecondary = pRInfo->pMultipage->m_Pages.FindNext(pRInfo->pSecondary);
		}

	}// if(m_ctrlTMView->GetSplitScreen() == TRUE)

	//	Does the caller want the results?
	if(pInfo != NULL)
	{
		memset(pInfo, 0, sizeof(SMultipageInfo));
		pInfo->pMultipage = pSplitMultipage;
		pInfo->pSecondary = pSplitSecondary;
	}
	return (pSplitSecondary != NULL);
}

//==============================================================================
//
// 	Function Name:	CMainView::GetTestBarcode()
//
// 	Description:	This function is called to during test mode operations to
//					retrieve the next barcode to load.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::GetTestBarcode(CString& rBarcode)
{
	CString			strError;
	CMedia*			pMedia;
	SMultipageInfo* pInfo = GetMultipageInfo(m_sState);

	//	Clear the buffer
	rBarcode.Empty();
	
	//	What is the current test state?
	switch(m_Test.sState)
	{
		case TESTING_PLAYLIST:

			//	Load the next image if we are testing images
			if(m_Test.bImages && 
			   m_Test.pImages && (m_Test.pImages->GetCount() > 0))
			{
				//	Get the next image in the list
				if((pMedia = m_Test.pImages->Next()) == 0)
					pMedia = m_Test.pImages->First();
					
				//	Reset the counters used to manage treatments
				m_Test.sTreatment = -1;
				m_Test.sTreatments = -1;
				
				//	Set the period for images
				m_Test.lPeriod = m_Test.lMediaPeriod;
								
				//	Switch states
				m_Test.sState = TESTING_IMAGE;
				
				break;
			}
			else
			{
				//	Drop through if not testing images
			}

		case TESTING_IMAGE:

			//	Are we testing the treatments for each page?
			//
			//	NOTE:	The command will be disabled if no treatments are in the
			//			list associated with the current page
			if(m_Test.bTreatments && IsCommandEnabled(TMAX_NEXTZAP))
			{
				//	Are we loading the first treatment?
				if(m_Test.sTreatments < 0)
				{
					pInfo = GetMultipageInfo(m_sState);
					if(pInfo && pInfo->pSecondary)
					{
						m_Test.sTreatments = pInfo->pSecondary->m_Children.GetCount();
						m_Test.sTreatment = 1;
						OnNextZap();
						return TRUE;
					}
				}

				//	Load the next treatment unless we've loaded them all once
				else if(m_Test.sTreatment < m_Test.sTreatments)
				{
					m_Test.sTreatment++;
					OnNextZap();
					return TRUE;
				}
				
				//	We must have run through all the treatments
				else
				{
					//	Drop through to load a new page
				}				
			}

			//	Are we testing pages?
			if(m_Test.bPages && IsCommandEnabled(TMAX_NEXTPAGE))
			{
				//	Reset the counters used to manage this page's treatments
				m_Test.sTreatment = -1;
				m_Test.sTreatments = -1;
				
				OnNextPage();

				return TRUE;
			}
			else
			{
				//	Drop through to switch test states
			}

			//	Load the next show if we are testing custom shows
			if(m_Test.bShows && 
			   m_Test.pShows && (m_Test.pShows->GetCount() > 0))
			{
				//	Get the next custom show in the list
				if((pMedia = m_Test.pShows->Next()) == 0)
					pMedia = m_Test.pShows->First();
					
				//	Switch states
				m_Test.sState = TESTING_SHOW;				
				break;
			}
			else
			{
				//	Drop through if not testing custom shows
			}

		case TESTING_SHOW:

			//	Are we testing pages?
			if(m_Test.bPages && IsCommandEnabled(TMAX_NEXTSHOWITEM))
			{
				OnNextShowItem();
				return TRUE;
			}
			else
			{
				//	Drop through to switch test states
			}

			//	Load the next movie if we are testing movie
			if(m_Test.bMovies && 
			   m_Test.pMovies && (m_Test.pMovies->GetCount() > 0))
			{
				//	Get the next Movie presentation in the list
				if((pMedia = m_Test.pMovies->Next()) == 0)
					pMedia = m_Test.pMovies->First();
					
				//	Set the period for movies
				m_Test.lPeriod = m_Test.lMoviePeriod;
								
				//	Switch states
				m_Test.sState = TESTING_MOVIE;				
				break;
			}
			else
			{
				//	Drop through if not testing movies
			}


		case TESTING_MOVIE:

			//	Are we testing pages?
			if(m_Test.bPages && IsCommandEnabled(TMAX_NEXTPAGE))
			{
				OnNextPage();
				return TRUE;
			}
			else
			{
				//	Drop through to switch test states
			}

			//	Load the next presentation if we are testing PowerPoints
			if(m_Test.bPowerPoints && m_ctrlTMPower.IsInitialized() &&
			   m_Test.pPowerPoints && (m_Test.pPowerPoints->GetCount() > 0))
			{
				//	Get the next PowerPoint presentation in the list
				if((pMedia = m_Test.pPowerPoints->Next()) == 0)
					pMedia = m_Test.pPowerPoints->First();
					
				//	Set the period for images
				m_Test.lPeriod = m_Test.lMediaPeriod;
								
				//	Switch states
				m_Test.sState = TESTING_POWERPOINT;				
				break;
			}
			else
			{
				//	Drop through if not testing PowerPoint presentations 
			}

		case TESTING_POWERPOINT:

			//	Are we testing pages?
			if(m_Test.bPages && IsCommandEnabled(TMAX_NEXTPAGE))
			{
				OnNextPage();
				return TRUE;
			}
			else
			{
				//	Drop through to switch test states
			}

			//	Load the next playlist if we are testing playlists
			if(m_Test.bPlaylists && 
			   m_Test.pPlaylists && (m_Test.pPlaylists->GetCount() > 0))
			{
				//	Get the next playlist in the list
				if((pMedia = m_Test.pPlaylists->Next()) == 0)
					pMedia = m_Test.pPlaylists->First();
					
				//	Set the period for images
				m_Test.lPeriod = m_Test.lPlaylistPeriod;
								
				//	Switch states
				m_Test.sState = TESTING_PLAYLIST;				
				break;
			}
			else
			{
				//	Reset the state so that it starts at the top of the switch
				m_Test.sState = TESTING_PLAYLIST;
				m_Test.lPeriod = 0;
				break;
			}

		default:

			//	Add an entry to the activity log
			strError.Format("Invalid Test Mode State: %d", m_Test.sState);
			UpdateActivityLog(strError);

			//	Reset the state so that it starts at the top of the switch
			m_Test.sState = TESTING_PLAYLIST;
			m_Test.lPeriod = 0;
			break;
	}

	//	Do we need to construct a barcode?
	if(pMedia)
		rBarcode.Format("%s.0", pMedia->m_strMediaId);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CMainView::GetTMKeyState()
//
// 	Description:	This function will check the keyboard to get the state of
//					the control keys
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
short CMainView::GetTMKeyState()
{
	short sKeyState = TMKEY_NONE;

	//	Is the shift key pressed?
	if(GetKeyState(VK_SHIFT) & 0x8000)
		sKeyState |= TMKEY_SHIFT;

	//	Is the control key pressed?
	if(GetKeyState(VK_CONTROL) & 0x8000)
		sKeyState |= TMKEY_CONTROL;

	//	Is the alt key pressed?
	if(GetKeyState(VK_MENU) & 0x8000)
		sKeyState |= TMKEY_ALT;

	return sKeyState;
}

//==============================================================================
//
// 	Function Name:	CMainView::GetTreatmentFileSpecs()
//
// 	Description:	Called to get the fully qualified paths to the source image
//					and zap file for the specified treatment
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::GetTreatmentFileSpecs(SMultipageInfo* pInfo, CString& rPageFileSpec, 
									  CString& rZapFileSpec, BOOL bVerify)
{
	BOOL bSuccessful = FALSE;

	//	Just in case ...
	ASSERT_RET_BOOL(m_pDatabase != NULL);
	ASSERT_RET_BOOL(pInfo != NULL);
	ASSERT_RET_BOOL(pInfo->pMultipage != NULL);
	ASSERT_RET_BOOL(pInfo->pSecondary != NULL);
	ASSERT_RET_BOOL(pInfo->pTertiary != NULL);

	while(bSuccessful == FALSE)
	{
		//	Get the page file specification
		m_pDatabase->GetFilename(pInfo->pMultipage, pInfo->pSecondary, rPageFileSpec);
		if(rPageFileSpec.GetLength() == 0)
			break;

		//	Get the file specification for the treatment
		m_pDatabase->GetFilename(pInfo->pTertiary, rZapFileSpec);
		if(rZapFileSpec.GetLength() == 0)
			break;

		//	Are we supposed to verify that the files exist?
		if(bVerify == TRUE)
		{
			if(!FindFile(rPageFileSpec))
			{	
				HandleError(0, IDS_ZAP_PAGE_NOT_FOUND, rPageFileSpec);
				break;
			}

			if(!FindFile(rZapFileSpec))
			{	
				HandleError(0, IDS_ZAP_FILE_NOT_FOUND, rZapFileSpec);
				break;
			}


		}// if(bVerify == TRUE)

		//	All is good
		bSuccessful = TRUE;


	}// while(bSuccessful == FALSE)


	return bSuccessful;

}

//==============================================================================
//
// 	Function Name:	CMainView::HandleError()
//
// 	Description:	This function will handle all runtime errors.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::HandleError(LPCSTR lpTitle, UINT uId, LPCSTR lpArg1, LPCSTR lpArg2) 
{
	CString strMessage;

	//	Are we in test mode?
	if(m_Test.bActive)
	{
		//	Format the error message and add it to the log
		FormatError(strMessage, uId, lpArg1, lpArg2);
		UpdateActivityLog(strMessage);
	}
	else
	{
		m_Errors.Handle(lpTitle, uId, lpArg1, lpArg2);
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::InitializeTest()
//
// 	Description:	This function will initialize the test mode operation of
//					the program
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::InitializeTest() 
{
	CMedia* pMedia;

	//	The database must be open
	if((m_pDatabase == 0) || (m_pDatabase->IsOpen() == FALSE))
	{
		m_Errors.Enable(TRUE);
		m_Errors.Handle("TrailMax Test Error", IDS_TESTNODB);
		return;
	}
		
	//	Enable the error handler while we set up the test
	m_Errors.Enable(TRUE);
	m_pDatabase->SetErrorHandler(TRUE, m_hWnd);
	
	//	Read the test options from the ini file
	ReadTestSetup();

	//	Allocate the media lists
	m_Test.pPlaylists = new CMedias();
	m_Test.pImages = new CMedias();
	m_Test.pShows = new CMedias();
	m_Test.pPowerPoints = new CMedias();
	m_Test.pMovies = new CMedias();

	//	Build our own test lists so we don't have to worry about disrupting
	//	the local iterators
	pMedia = m_pDatabase->GetPlaylistMedia().First();
	while(pMedia)
	{
		m_Test.pPlaylists->Add(pMedia);
		pMedia = m_pDatabase->GetPlaylistMedia().Next();
	}

	pMedia = m_pDatabase->GetMultipageMedia().First();
	while(pMedia)
	{
		//	Which list should we put this media object in?
		switch(pMedia->m_lPlayerType)
		{
			case MEDIA_TYPE_CUSTOMSHOW:

				m_Test.pShows->Add(pMedia);
				break;

			case MEDIA_TYPE_IMAGE:				

				m_Test.pImages->Add(pMedia);
				break;

			case MEDIA_TYPE_RECORDING:			

				m_Test.pMovies->Add(pMedia);
				break;

			case MEDIA_TYPE_POWERPOINT:			

				m_Test.pPowerPoints->Add(pMedia);
				break;

			case MEDIA_TYPE_PLAYLIST:
			default:

				break;
		}
		
		pMedia = m_pDatabase->GetMultipageMedia().Next();
	}

	//	Disable all the runtime error handlers
	m_Errors.Enable(FALSE);
	m_ctrlTMMovie.SetEnableErrors(FALSE);
	for(int i = 0; i < SZ_ARR_TM_VW; i++) {
		m_arrTmView[i]->SetEnableErrors(FALSE);
	}
	m_pDatabase->SetErrorHandler(FALSE, 0);

	//	Mark the start of this test in the activity log
	UpdateActivityLog("\n\n=======================================================================", FALSE);
	UpdateActivityLog("TEST STARTED");
	UpdateActivityLog("=======================================================================", FALSE);
	
	//	Set the control bar
	SetControlBar(m_ControlBar.iId);

	//	Start the test
	m_Test.sState = TESTING_PLAYLIST;
	m_Test.lPeriod = 0;
	SetTimer(TESTMODE_TIMERID, 1000, NULL);
}

//==============================================================================
//
// 	Function Name:	CMainView::InitializeToolbars()
//
// 	Description:	This function is called to initialize the toolbars.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::InitializeToolbars()
{
	//	Initialize the information structures
	
	memset(m_aToolbars, 0, sizeof(m_aToolbars));

	//	Store the toolbar pointers
	m_aToolbars[S_DOCUMENT].pControl = &m_ctrlTBDocuments;
	m_aToolbars[S_GRAPHIC].pControl = &m_ctrlTBGraphics;
	m_aToolbars[S_LINKEDIMAGE].pControl = &m_ctrlTBLinks;
	m_aToolbars[S_PLAYLIST].pControl = &m_ctrlTBPlaylists;
	m_aToolbars[S_MOVIE].pControl = &m_ctrlTBMovies;
	m_aToolbars[S_TEXT].pControl = &m_ctrlTBText;
	m_aToolbars[S_POWERPOINT].pControl = &m_ctrlTBPowerPoint;
	m_aToolbars[S_LINKEDPOWER].pControl = &m_ctrlTBLinkPP;

	//	Read the setup information from the ini file for each toolbar
	//
	//	NOTE:	The S_TEXT toolbar copies the S_PLAYLIST toolbar
	//			The S_LINKEDPOWER toolbar copies the S_LINKEDIMAGE toolbar
	ReadToolbar(&(m_aToolbars[S_DOCUMENT]), TMBARS_DOCUMENT_SECTION, "");
	ReadToolbar(&(m_aToolbars[S_GRAPHIC]), TMBARS_GRAPHIC_SECTION, TMBARS_DOCUMENT_SECTION);
	ReadToolbar(&(m_aToolbars[S_LINKEDIMAGE]), TMBARS_LINK_SECTION, TMBARS_DOCUMENT_SECTION);
	ReadToolbar(&(m_aToolbars[S_PLAYLIST]), TMBARS_PLAYLIST_SECTION, TMBARS_DOCUMENT_SECTION);
	ReadToolbar(&(m_aToolbars[S_TEXT]), TMBARS_PLAYLIST_SECTION, TMBARS_DOCUMENT_SECTION);
	ReadToolbar(&(m_aToolbars[S_MOVIE]), TMBARS_MOVIE_SECTION, TMBARS_DOCUMENT_SECTION);
	ReadToolbar(&(m_aToolbars[S_POWERPOINT]), TMBARS_POWERPOINT_SECTION, TMBARS_DOCUMENT_SECTION);
	ReadToolbar(&(m_aToolbars[S_LINKEDPOWER]), TMBARS_LINK_SECTION, TMBARS_DOCUMENT_SECTION);
	
	// assigning the large toolbar with s_document toolbar 	
	m_aToolbars[S_DOCUMENT_LARGE] = m_aToolbars[S_DOCUMENT];
	m_aToolbars[S_DOCUMENT_LARGE].sSize = 2;
	m_aToolbars[S_DOCUMENT_LARGE].pControl = &m_ctrlTBDocumentsLarge;
    m_aToolbars[S_DOCUMENT_LARGE].pControl->SetButtonMap(m_aToolbars[S_DOCUMENT].aMap);

	//	Initialize the toolbars
	for(int i = 0; i < MAX_STATES+2; i++)
	{
		if(m_aToolbars[i].pControl == 0)
			continue;
		(m_aToolbars[i].pControl)->SetButtonSize(m_aToolbars[i].sSize);
		(m_aToolbars[i].pControl)->SetStretch(m_aToolbars[i].bStretch);
		(m_aToolbars[i].pControl)->SetStyle(m_aToolbars[i].bFlat ? TMTB_FLAT : TMTB_RAISED);
		(m_aToolbars[i].pControl)->SetButtonMap(m_aToolbars[i].aMap);
		(m_aToolbars[i].pControl)-> SetButtonLabel(TMTB_PLAYDESIGNATION, "Play/Pause Designation");
		(m_aToolbars[i].pControl)->SetButtonLabel(TMTB_PLAYMOVIE, "Play/Pause Movie");
		(m_aToolbars[i].pControl)->SetButtonLabel(TMTB_DISABLELINKS, "Enable/Disable Links");
		(m_aToolbars[i].pControl)->Initialize();
		(m_aToolbars[i].pControl)->ShowWindow(SW_HIDE);

	}

	//	We will set the properties of the drawing tools toolbar just before
	//	we display it
	m_ctrlTBTools.Initialize();
	m_ctrlTBTools.ShowWindow(SW_HIDE);

	//	Set the initial toolbar
	SelectToolbar(S_CLEAR);
}


//==============================================================================
//
// 	Function Name:	CMainView::IsCommandChecked()
//
// 	Description:	This function is called to determine if the specific
//					command is currently selected
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::IsCommandChecked(short sCommand)
{
	//	What command?
	switch(sCommand)
	{
		case TMAX_RED:
		
			return m_ctrlTMView->GetColor() == TMV_RED;
		
		case TMAX_GREEN:
		
			return m_ctrlTMView->GetColor() == TMV_GREEN;
		
		case TMAX_BLUE:
		
			return m_ctrlTMView->GetColor() == TMV_BLUE;
		
		case TMAX_YELLOW:
		
			return m_ctrlTMView->GetColor() == TMV_YELLOW;
		
		case TMAX_BLACK:
		
			return m_ctrlTMView->GetColor() == TMV_BLACK;
		
		case TMAX_WHITE:
		
			return m_ctrlTMView->GetColor() == TMV_WHITE;
		
		case TMAX_DARKRED:
		
			return m_ctrlTMView->GetColor() == TMV_DARKRED;
		
		case TMAX_DARKGREEN:
		
			return m_ctrlTMView->GetColor() == TMV_DARKGREEN;
		
		case TMAX_DARKBLUE:
		
			return m_ctrlTMView->GetColor() == TMV_DARKBLUE;
		
		case TMAX_LIGHTRED:
		
			return m_ctrlTMView->GetColor() == TMV_LIGHTRED;
		
		case TMAX_LIGHTGREEN:
		
			return m_ctrlTMView->GetColor() == TMV_LIGHTGREEN;
		
		case TMAX_LIGHTBLUE:
		
			return m_ctrlTMView->GetColor() == TMV_LIGHTBLUE;
		
		case TMAX_FREEHAND:
		
			return m_ctrlTMView->GetAnnTool() == FREEHAND;
		
		case TMAX_LINE:
		
			return m_ctrlTMView->GetAnnTool() == LINE;
		
		case TMAX_ARROW:
		
			return m_ctrlTMView->GetAnnTool() == ARROW;
		
		case TMAX_ELLIPSE:
		
			return m_ctrlTMView->GetAnnTool() == ELLIPSE;
		
		case TMAX_RECTANGLE:
		
			return m_ctrlTMView->GetAnnTool() == RECTANGLE;
		
		case TMAX_FILLEDELLIPSE:
		
			return m_ctrlTMView->GetAnnTool() == FILLED_ELLIPSE;
		
		case TMAX_FILLEDRECTANGLE:
		
			return m_ctrlTMView->GetAnnTool() == FILLED_RECTANGLE;
		
		case TMAX_POLYLINE:
		
			return m_ctrlTMView->GetAnnTool() == POLYLINE;
		
		case TMAX_POLYGON:
		
			return m_ctrlTMView->GetAnnTool() == POLYGON;
		
		case TMAX_ANNTEXT:
		
			return m_ctrlTMView->GetAnnTool() == ANNTEXT;
		
		case TMAX_DISABLELINKS:
		
			return m_bDisableLinks;

		case TMAX_SPLITVERTICAL:

			return (m_ctrlTMView->GetSplitScreen() && !m_ctrlTMView->GetSplitHorizontal());

		case TMAX_SPLITHORIZONTAL:

			return (m_ctrlTMView->GetSplitScreen() && m_ctrlTMView->GetSplitHorizontal());

		case TMAX_CALLOUT:
		
			return m_ctrlTMView->GetAction() == CALLOUT;
		
		case TMAX_DRAWTOOL:
		
			return m_ctrlTMView->GetAction() == DRAW;
		
		case TMAX_HIGHLIGHT:
		
			return m_ctrlTMView->GetAction() == HIGHLIGHT;
		
		case TMAX_REDACT:
		
			return m_ctrlTMView->GetAction() == REDACT;
		
		case TMAX_PAN:
		
			return m_ctrlTMView->GetAction() == PAN;
		
		case TMAX_SELECT:
		
			return m_ctrlTMView->GetAction() == SELECT;
		
		case TMAX_ZOOM:				
		
			if(m_ctrlTMView->GetAction() != ZOOM)
				return FALSE;
			if(m_ctrlTMView->GetZoomToRect())
				return FALSE;
			if(m_ctrlTMView->GetZoomFactor(TMV_ACTIVEPANE) >= (float)m_ctrlTMView->GetMaxZoom())
				return FALSE;
			else
				return TRUE;

		case TMAX_ZOOMWIDTH:

			return m_ctrlTMView->GetZoomState(TMV_ACTIVEPANE) == ZOOMED_FULLWIDTH;

		case TMAX_ZOOMRESTRICTED:

			if(m_ctrlTMView->GetAction() != ZOOM)
				return FALSE;
			if(!m_ctrlTMView->GetZoomToRect())
				return FALSE;
			if(m_ctrlTMView->GetZoomFactor(TMV_ACTIVEPANE) >= (float)m_ctrlTMView->GetMaxZoom())
				return FALSE;
			else
				return TRUE;

		case TMAX_PLAY:
		
			return m_bPlaying;

		case TMAX_VIDEOCAPTION:

			return (m_ctrlTMMovie.GetOverlayVisible());

		case TMAX_TEXT:

			return (m_bUserScrollEnabled == TRUE);

		case TMAX_STATUSBAR:

			return (m_ctrlTMStat.IsWindowVisible());

		case TMAX_SHADEONCALLOUT:

			return (m_ctrlTMView->GetShadeOnCallout());

		case TMAX_GESTURE_PAN:

			return m_ctrlTMView->GetAction() == TMAX_NOCOMMAND;

		default:
			
			return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::IsCommandEnabled()
//
// 	Description:	This function is called to determine if the specific
//					command should be enabled
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::IsCommandEnabled(short sCommand)
{
	SMultipageInfo*	pInfo = GetMultipageInfo(m_sState);
	short			sState;

	ASSERT(sCommand >= 0);
	ASSERT(sCommand < MAX_COMMANDS);

	//	First check the command based on the current screen state
	if(!EnableCommand[sCommand][m_sState])
		return FALSE;

	//	Now check additional criteria for some of the commands
	switch(sCommand)
	{
		case TMAX_CALLOUT:
		case TMAX_DRAWTOOL:
		case TMAX_ERASE:
		case TMAX_PRINT:
		case TMAX_HIGHLIGHT:
		case TMAX_REDACT:
		case TMAX_ROTATECCW:
		case TMAX_ROTATECW:
		case TMAX_PAN:
		case TMAX_BLACK:
		case TMAX_BLUE:
		case TMAX_GREEN:
		case TMAX_RED:
		case TMAX_YELLOW:
		case TMAX_WHITE:
		case TMAX_DARKRED:
		case TMAX_DARKGREEN:
		case TMAX_DARKBLUE:
		case TMAX_LIGHTRED:
		case TMAX_LIGHTGREEN:
		case TMAX_LIGHTBLUE:
		case TMAX_DELETEANN:
		case TMAX_SELECT:
		case TMAX_SHADEONCALLOUT:
		
			return m_ctrlTMView->IsLoaded(TMV_ACTIVEPANE);

		case TMAX_SELECTTOOL:

			return !m_ctrlTBTools.IsWindowVisible();

		case TMAX_SWITCHPANE:

			return m_ctrlTMView->GetSplitScreen();

		case TMAX_FIRSTZAP:
		case TMAX_LASTZAP:
		case TMAX_NEXTZAP:
		case TMAX_PREVZAP:
		
			//	Is the active pane of the viewer loaded?
			if(!m_ctrlTMView->IsLoaded(TMV_ACTIVEPANE))
				return FALSE;

			//	Do we have an active multipage object?
			if((pInfo == 0) || (pInfo->pSecondary == 0))
				return FALSE;
			else
				return (pInfo->pSecondary->m_Children.GetCount() > 0);

		case TMAX_FIRSTPAGE:
		case TMAX_PREVPAGE:
    
			//	Use the previous state if the current state is S_CLEAR
			sState = (m_sState == S_CLEAR) ? m_sPrevState : m_sState;

			//	Is this a PowerPoint presentation?
			if(sState == S_POWERPOINT || sState == S_LINKEDPOWER)
			{
				//	NOTE:	We removed this test because it prevented reverse
				//			iteration through animations if on page 1
				//return (m_ctrlTMPower.GetCurrentSlide(-1) > 1);
				return TRUE;
			}
			else
			{
				//	Do we have an active multipage object?
				if((pInfo == 0) || (pInfo->pSecondary == 0))
					return FALSE;
				else
					return (!pInfo->pMultipage->m_Pages.IsFirst(pInfo->pSecondary));
			}

		case TMAX_LASTPAGE:
		case TMAX_NEXTPAGE:
    
			//	Use the previous state if the current state is S_CLEAR
			sState = (m_sState == S_CLEAR) ? m_sPrevState : m_sState;

			//	Is this a PowerPoint presentation?
			if(sState == S_POWERPOINT || sState == S_LINKEDPOWER)
			{
				return m_ctrlTMPower.GetCurrentState(-1) != TMPOWER_DONE;
			}
			else
			{
				//	Do we have an active multipage object?
				if((pInfo == 0) || (pInfo->pSecondary == 0))
					return FALSE;
				else
					return (!pInfo->pMultipage->m_Pages.IsLast(pInfo->pSecondary));
			}
    
		case TMAX_NORMAL:
			
			//	Is the active pane of the viewer loaded?
			if(!m_ctrlTMView->IsLoaded(TMV_ACTIVEPANE))
				return FALSE;

			return (m_ctrlTMView->GetZoomState(TMV_ACTIVEPANE) != ZOOMED_NONE);
		
		case TMAX_SAVEZAP:
		case TMAX_UPDATE_ZAP:
			
			//	Is the active pane of the viewer loaded?
			if(!m_ctrlTMView->IsLoaded(TMV_ACTIVEPANE))
				return FALSE;

			//	Is this an image?
			if(pInfo == NULL) return FALSE;
			if(pInfo->pMultipage == NULL) return FALSE;
			if(pInfo->pSecondary == NULL) return FALSE;
			if(pInfo->pMultipage->m_lPlayerType != MEDIA_TYPE_IMAGE) return FALSE;

			if(sCommand == TMAX_UPDATE_ZAP)
			{
				//	The active pane must be loaded with a zap file
				if(pInfo->pTertiary == NULL) 
					return FALSE;
				//	Always allow updates to split screen treatments
				else if(m_bZapSplitScreen == TRUE)
					return TRUE;
				//	Update only if not one half of a split screen
				else
					return (pInfo->pTertiary->m_strSiblingId.GetLength() == 0);
			}
			else
			{
				return TRUE;
			}

		case TMAX_SAVE_SPLIT_ZAP:
			
			//	Must be in split screen mode
			if(!m_ctrlTMView->GetSplitScreen())
				return FALSE;

			//	Make sure both viewer panes are loaded
			if(!m_ctrlTMView->IsLoaded(TMV_LEFTPANE))
				return FALSE;
			if(!m_ctrlTMView->IsLoaded(TMV_RIGHTPANE))
				return FALSE;

			//	Verify the contents of the left pane
			pInfo = (SMultipageInfo*)m_ctrlTMView->GetData(TMV_LEFTPANE);
			if(pInfo == NULL) return FALSE;
			if(pInfo->pMultipage == NULL) return FALSE;
			if(pInfo->pSecondary == NULL) return FALSE;
			if(pInfo->pMultipage->m_lPlayerType != MEDIA_TYPE_IMAGE) return FALSE;

			//	Verify the contents of the right pane
			pInfo = (SMultipageInfo*)m_ctrlTMView->GetData(TMV_RIGHTPANE);
			if(pInfo == NULL) return FALSE;
			if(pInfo->pMultipage == NULL) return FALSE;
			if(pInfo->pSecondary == NULL) return FALSE;
			if(pInfo->pMultipage->m_lPlayerType != MEDIA_TYPE_IMAGE) return FALSE;

			//	All is well
			return TRUE;

		case TMAX_ZOOM:
		case TMAX_ZOOMRESTRICTED:
		
			//	Is the active pane of the viewer loaded?
			if(!m_ctrlTMView->IsLoaded(TMV_ACTIVEPANE))
				return FALSE;

			return (m_ctrlTMView->GetZoomFactor(TMV_ACTIVEPANE) < 
					(float)m_ctrlTMView->GetMaxZoom());

		case TMAX_ZOOMWIDTH:
		
			//	Is the active pane of the viewer loaded?
			if(!m_ctrlTMView->IsLoaded(TMV_ACTIVEPANE))
				return FALSE;
			else
				return TRUE;

		case TMAX_PLAY:
		
			//	Is playback disabled?
			if(!m_bEnablePlay)
				return FALSE;
			else
				return (m_ctrlTMMovie.GetPosition() < 
					   (m_ctrlTMMovie.GetMaxTime() - 0.33333));

		case TMAX_STARTMOVIE:
		case TMAX_BACKMOVIE:
		
			//	Are we already positioned on the start of the movie?
			return (m_ctrlTMMovie.GetPosition() > m_dMinMovieCue); 

		case TMAX_ENDMOVIE:
		case TMAX_FWDMOVIE:
		
			//	Are we already positioned on the end of the movie?
			return (m_ctrlTMMovie.GetPosition() < m_dMaxMovieCue);

		case TMAX_FIRSTDESIGNATION:
		case TMAX_LASTDESIGNATION:
		case TMAX_NEXTDESIGNATION:
		case TMAX_PREVDESIGNATION:
		case TMAX_BACKDESIGNATION:
		case TMAX_FWDDESIGNATION:
		case TMAX_STARTDESIGNATION:
		case TMAX_PLAYTHROUGH:
		
			//	Do we have a valid playlist?
			return (m_Playlist.pPlaylist != 0);

		case TMAX_SETPAGELINE:
		
			//	Do we have an active playlist
			if((m_Playlist.pPlaylist != NULL) && (m_Playlist.pPlaylist->GetIsRecording() == FALSE))
				return TRUE;
			else
				return (!m_strLastPlaylist.IsEmpty());// Use the last playlist if available

		case TMAX_FILTERPROPS:

			return m_ctrlTMMovie.IsReady();

		case TMAX_TEXT:

			if(m_ctrlTMText.IsReady() == FALSE)
				return FALSE;
			else if(m_bSystemScrollEnabled == FALSE)
				return FALSE;
			else if(m_Playlist.pPlaylist == NULL)
				return FALSE;
			else if(m_Playlist.pDesignation == NULL)
				return FALSE;
			else
				return TRUE;

		case TMAX_FIRSTSHOWITEM:
		case TMAX_PREVSHOWITEM:
    
			//	Do we have an active custom show object?
			if((m_Show.pShow == 0) || (m_Show.pItem == 0))
				return FALSE;
			else
				return (!m_Show.pShow->m_Items.IsFirst(m_Show.pItem));

		case TMAX_LASTSHOWITEM:
		case TMAX_NEXTSHOWITEM:
    
			//	Do we have an active custom show object?
			if((m_Show.pShow == 0) || (m_Show.pItem == 0))
				return FALSE;
			else
				return (!m_Show.pShow->m_Items.IsLast(m_Show.pItem));
    
		case TMAX_NEXTMEDIA:

			//	Do we have an active media object?
			if((m_pDatabase == 0) || (m_pMedia == 0))
				return FALSE;

			//	What is the current state?
			switch(m_sState)
			{
				case S_CLEAR:
					
					return FALSE;

				case S_DOCUMENT:
				case S_GRAPHIC:
				case S_MOVIE:
				case S_POWERPOINT:

					return (m_pDatabase->IsLastMultipage(m_pMedia) == FALSE);
		
				case S_LINKEDIMAGE:
				case S_PLAYLIST: 
				case S_TEXT:
				case S_LINKEDPOWER:

					return (m_pDatabase->IsLastPlaylist(m_pMedia) == FALSE);

				default:

					return FALSE;		
			}
			break;

		case TMAX_PREVMEDIA:

			//	Do we have an active media object?
			if((m_pDatabase == 0) || (m_pMedia == 0))
				return FALSE;

			//	What is the current state?
			switch(m_sState)
			{
				case S_CLEAR:
					
					return FALSE;

				case S_DOCUMENT:
				case S_GRAPHIC:
				case S_MOVIE:
				case S_POWERPOINT:

					return (m_pDatabase->IsFirstMultipage(m_pMedia) == FALSE);
		
				case S_LINKEDIMAGE:
				case S_PLAYLIST: 
				case S_TEXT:
				case S_LINKEDPOWER:

					return (m_pDatabase->IsFirstPlaylist(m_pMedia) == FALSE);

				default:

					return FALSE;		
			}
			break;

		case TMAX_PREV_BARCODE:

			if(m_aBarcodes.IsEmpty() == TRUE) 
				return FALSE;
			else if(m_aBarcodes.OnFirst() == TRUE) 
				return FALSE;
			else
				return TRUE;

		case TMAX_NEXT_BARCODE:

			if(m_aBarcodes.IsEmpty() == TRUE) 
				return FALSE;
			else if(m_aBarcodes.OnLast() == TRUE) 
				return FALSE;
			else
				return TRUE;

		case TMAX_NEXTPAGE_HORIZONTAL:
		case TMAX_NEXTPAGE_VERTICAL:
    
			//	Are we viewing a document / graphic?
			if((m_sState != S_DOCUMENT) && (m_sState != S_GRAPHIC))
				return FALSE;

			//	Are we already in split screen mode?
			if(m_ctrlTMView->GetSplitScreen())
				return FALSE;

			//	Must have a valid page
			if((pInfo == NULL) || (pInfo->pSecondary == NULL))
				return FALSE;

			//	Must not be viewing the last page
			return (!pInfo->pMultipage->m_Pages.IsLast(pInfo->pSecondary));

		case TMAX_SPLITPAGES_NEXT:
		case TMAX_SPLITPAGES_PREVIOUS:
    
			//	Are we viewing a document / graphic?
			if((m_sState != S_DOCUMENT) && (m_sState != S_GRAPHIC))
				return FALSE;

			//	Are we already in split screen mode?
			if(m_ctrlTMView->GetSplitScreen() == FALSE)
				return FALSE;

			//	Must have a valid page
			return GetSplitPageInfo(NULL, (sCommand == TMAX_SPLITPAGES_PREVIOUS));

		return TRUE;

		case TMAX_ADD_TO_BINDER:

			if(m_Barcode.m_strMediaId.GetLength() == 0) 
				return FALSE;
			else if(m_ctrlManagerApp.IsRunning() == FALSE)
				return FALSE;
			else
				return TRUE;

		case TMAX_HELP:

			if(m_strHelpFileSpec.IsEmpty() == TRUE) 
				return FALSE;
			else
				return TRUE;

		case TMAX_GESTURE_PAN:
			return m_bTabletMode;

		default:
			
			return TRUE;
	}

}

//==============================================================================
//
// 	Function Name:	CMainView::IsScrollTextVisible()
//
// 	Description:	Called to determine if the scrolling text window is visible
//
// 	Returns:		TRUE if visible
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::IsScrollTextVisible()
{
	BOOL bVisible = FALSE;

	if(IsWindow(m_ctrlTMText.m_hWnd))
	{
		if(m_ctrlTMText.IsWindowVisible())
		{
			if((m_rcText.bottom - m_rcText.top) > 1)
				bVisible = ((m_rcText.right - m_rcText.left) > 1);
		}

	}// if(IsWindow(m_ctrlTMText.m_hWnd))

	return bVisible;
}

//==============================================================================
//
// 	Function Name:	CMainView::IsVideoVisible()
//
// 	Description:	Called to determine if the video playback window is visible
//
// 	Returns:		TRUE if visible
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::IsVideoVisible()
{
	BOOL bVisible = FALSE;

	if(IsWindow(m_ctrlTMMovie.m_hWnd))
	{
		if(m_ctrlTMMovie.IsWindowVisible())
		{
			if((m_rcMovie.bottom - m_rcMovie.top) > 1)
				bVisible = ((m_rcMovie.right - m_rcMovie.left) > 1);
		}

	}// if(IsWindow(m_ctrlTMMovie.m_hWnd))

	return bVisible;
}

//==============================================================================
//
// 	Function Name:	CMainView::LoadAsPlaylist()
//
// 	Description:	This function will load the multipage animation provided by
//					the caller as a playlist
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::LoadAsPlaylist(CMultipage* pMultipage, long lSecondary)
{
	SPlaylistParams Params;

	ASSERT(pMultipage);
	ASSERT(m_pDatabase != 0);
	if(m_pDatabase == 0) return FALSE;

	//	Convert the multipage object to a playlist
	Params.pPlaylist = m_pDatabase->ConvertToPlaylist(pMultipage);

	//	We are done with the multipage object
	delete pMultipage;

	//	Was the conversion successful?
	if(Params.pPlaylist == 0)
	{
		HandleError(0, IDS_CONVERTANIMATION, pMultipage->m_strMediaId);
		return FALSE;
	}

	//	Set the appropriate designation
	if(lSecondary <= 0)
	{
		Params.pStart = Params.pPlaylist->m_Designations.First();
		Params.lStart = -1L;
	}
	else
	{
		//	Use the Animation flag instead of the playlist flag because this
		//	playlist started as a multipage animation
		if(m_iAnimationSecondary == SECONDARY_AS_ORDER)
			Params.pStart = Params.pPlaylist->m_Designations.FindFromOrder(lSecondary);
		else
			Params.pStart = Params.pPlaylist->m_Designations.FindFromBarcode(lSecondary);
			
		//	Assign the first designation if we did not find the requested page
		if(Params.pStart == 0)
		{
			Params.pStart = Params.pPlaylist->m_Designations.First();
			Params.lStart = -1L;
		}

		//	Use the playback order to set the start identifier
		if(Params.pStart)
			Params.lStart = Params.pStart->m_lPlaybackOrder;
	}
			
	//	Do we have a start designation?
	if(Params.pStart == 0)
	{
		delete Params.pPlaylist;
		return FALSE;
	}

	//	Set the stop designation
	if(m_bRunToEnd)
	{
		Params.lStop = -1;
		Params.pStop = Params.pPlaylist->m_Designations.Last();
	}
	else
	{
		Params.lStop = Params.pStart->m_lPlaybackOrder;
		Params.pStop = Params.pStart;
	}

	//	We are not cueing to a specific position
	Params.dPosition = -1.0;

	//	Execute the playlist
	if(!LoadPlaylist(&Params))
	{
		delete Params.pPlaylist;
		return FALSE;
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CMainView::LoadCaptureBarcode()
//
// 	Description:	Called to load the next barcode found in the active
//					capture source file
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::LoadCaptureBarcode()
{
	char	szBarcode[1024];
	CString	strBarcode = "";
	CString	strTarget = "";
	CString	strSource = "";
	CString	strFilename = "";
	CString	strMsg = "";
	char*	pExt = NULL;

	ASSERT(m_fptrCaptureBarcodes != NULL);

	//	Get the next barcode
	while(fgets(szBarcode, sizeof(szBarcode), m_fptrCaptureBarcodes) != NULL)
	{
		strBarcode = szBarcode;
		strBarcode.TrimLeft();
		strBarcode.TrimRight();

		if(strBarcode.Find('.') < 0)
			strBarcode += ".1";

		if(LoadFromBarcode(strBarcode, FALSE, FALSE) == TRUE)
		{
			//	Construct the path to the target file
			m_strCaptureTargetFile.Format("%s%05d_%s.png", m_strCaptureTargetFolder, ++m_iCapturedBarcodes, strBarcode);

			//	Set the timer to give time to draw before capture
			SetTimer(CAPTURE_BARCODE_TIMERID, 500, NULL);
			return TRUE;
			
		}// if(LoadFromBarcode(strBarcode, FALSE, FALSE) == TRUE)

	}// while(fgets(szBarcode, sizeof(szBarcode), m_fptrCaptureBarcodes) != NULL)
	
	//	Must be out of barcodes
	if(m_fptrCaptureBarcodes != NULL)
	{
		fclose(m_fptrCaptureBarcodes);
		m_fptrCaptureBarcodes = NULL;
	}

	//	Process the next file
	while(1)
	{
		if(OpenCaptureSource() == TRUE)
			break;
	}

	return TRUE;
		
}

//==============================================================================
//
// 	Function Name:	CMainView::LoadDeposition()
//
// 	Description:	This function is called to load the specified deposition
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::LoadDeposition(CMedia* pMedia, long lSegment, long lPage, int iLine)
{
	CPlaylist*		pPlaylist = NULL;
	CDeposition*	pDeposition = NULL;
	CString			strTranscriptFileSpec = "";
	CString			strError = "";
	SPlaylistParams	PLParams;
	BOOL			bSuccessful = FALSE;

	ASSERT(pMedia != 0);
	ASSERT(m_pDatabase != 0);
	if(pMedia == NULL) return FALSE;
	if(m_pDatabase == NULL) return FALSE;

	//	Create the deposition we need to build the playback script
	if((pDeposition = m_pDatabase->GetDeposition(pMedia)) == NULL)
	{
		HandleError(0, IDS_NODEPOSITION, pMedia->m_strMediaId);
		return FALSE;
	}
	
	ASSERT(pDeposition->m_pTranscript != NULL);

	//	Make sure we can locate the transcript file
	m_pDatabase->GetFilename(pDeposition->m_pTranscript, strTranscriptFileSpec);
	if(FindFile(strTranscriptFileSpec) == FALSE)
	{
		HandleError(0, IDS_TRANSRIPTNOTFOUND, strTranscriptFileSpec);
		delete pDeposition;
		return FALSE;
	}

	//	Make sure we can open the transcript file
	if(pDeposition->m_pTranscript->Open(strTranscriptFileSpec) == FALSE)
	{
		HandleError(0, IDS_OPENTRANSCRIPTFAILED, strTranscriptFileSpec);
		delete pDeposition;
		return FALSE;
	}

	//	Convert the deposition object to a playlist
	pPlaylist = m_pDatabase->ConvertToPlaylist(pDeposition);

	//	Was the conversion successful?
	if(pPlaylist != NULL)
	{
		//	Are we supposed to automatically start the playback?
		if((m_iLoadPage > 0) && (m_iLoadLine > 0))
		{
			//	Set the members needed to translate the page/line to a start time
			m_iCuePage = m_iLoadPage;
			m_iCueLine = m_iLoadLine;
			m_lCueTranscript = pDeposition->m_pTranscript->m_lTranscriptId;
			
			ZeroMemory(&PLParams, sizeof(PLParams));
			PLParams.pPlaylist = pPlaylist;

			//	Translate the specified page/line to a time
			if(TranslateLine(&PLParams))
			{
				LoadPlaylist(&PLParams);
			}
			else
			{
				//	Prompt the user for a valid page and line
				strError.Format("%ld:%d is not valid for this deposition", m_iCuePage, m_iCueLine);
				SetLine(strError, pPlaylist);
			}
		
		}
		else
		{
			//	Prompt the user for the page and line at which to start the playback
			SetLine("", pPlaylist);
		}

		bSuccessful = TRUE;
	}
	else
	{
		HandleError(0, IDS_CONVERTDEPOSITION, pMedia->m_strMediaId);

	}// if(pPlaylist != NULL)

	//	Clean up
	if(pDeposition != NULL)
	{
		if(pDeposition->m_pTranscript != NULL)
			pDeposition->m_pTranscript->Close();
		delete pDeposition;
	}
	m_iLoadPage = 0;
	m_iLoadLine = 0;

	return bSuccessful;
}

//==============================================================================
//
// 	Function Name:	CMainView::LoadFromBarcode()
//
// 	Description:	This function will lookup the filename associated with the
//					barcode provided by the caller. If found, it will load
//					the file.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::LoadFromBarcode(LPCSTR lpBarcode, BOOL bAddBuffer, BOOL bAlternate)
{
	CMedia*		pMedia;
	CBarcode	Barcode;
	CString		strActivity;
	char		szBarcode[512];
	
	//	Make sure the automatic transition is turned off if this is not a link event
	if(m_AppLink.GetIsEvent() == FALSE)
		StopAutoTransition();

	//	Make sure the database is open
	if((m_pDatabase == 0) || (m_pDatabase->IsOpen() == FALSE))
	{
		HandleError(0, IDS_DBNOTOPEN);
		theApp.ResetHook();
		return FALSE;
	}

	//	Is this an alternate barcode format?
	if(bAlternate)
	{
		//	Translate the foreign code to a Trialmax barcode
		if(!m_pDatabase->MapBarcode(lpBarcode, szBarcode))
		{
			HandleError(0, IDS_NOFOREIGNCODE, lpBarcode);
			theApp.ResetHook();
			return FALSE;
		}
	}
	else
	{
		//	Copy the barcode to our local buffer
		lstrcpyn(szBarcode, lpBarcode, sizeof(szBarcode));
	}

	//	Log the barcode if we're running a test
	if(m_Test.bActive)
	{
		strActivity.Format("Loading from barcode: %s", szBarcode);
		UpdateActivityLog(strActivity);
	}

	//	Initialize the barcode using the caller's string
	if(!Barcode.SetBarcode(szBarcode))
	{
		HandleError(0, IDS_INVALIDBARCODE, szBarcode);
		theApp.ResetHook();
		return FALSE;
	}

	//	Get the basic media information associated with this barcode
	if((pMedia = m_pDatabase->GetMedia(Barcode.m_strMediaId)) == 0)
	{
		HandleError(0, IDS_NOMEDIARECORD, szBarcode);
		theApp.ResetHook();
		return FALSE;
	}

	//	Turn off split-screen mode if the screen is blanked
	if((m_sState == S_CLEAR) && (m_ctrlTMView->GetSplitScreen() == TRUE))
	{
		if(m_ctrlTMView->GetSplitHorizontal())
			OnSplitHorizontal();
		else
			OnSplitVertical();
	}

	//	Load the media object
	if(LoadMedia(pMedia, Barcode.m_lSecondaryId, Barcode.m_lTertiaryId))
	{
		//	Reset the persistant custom show information if this is not
		//	a new custom show and not a linked image or presentation
		//
		//	NOTE:	We have to do this so that the system knows the current
		//			state is not the result of a custom show item.
		if((pMedia->m_lPlayerType != MEDIA_TYPE_CUSTOMSHOW) && 
		   (m_sState != S_LINKEDIMAGE) && (m_sState != S_LINKEDPOWER))
			ResetShowInfo();
			
		//	Update the member used to iterate the list of media records
		//
		//	NOTE:	We don't want to reset this member if the media object being
		//			loaded is an item in an active custom show
		if((pMedia->m_lPlayerType == MEDIA_TYPE_CUSTOMSHOW) || (m_Show.pItem == 0))
		{
			m_pMedia = pMedia;
		}

		//	Update the persistant barcode information
		m_Barcode = Barcode;
		if(bAddBuffer == TRUE)
			m_aBarcodes.Add(m_Barcode);

		//	Update the status bar
		UpdateStatusBar();

		return TRUE;
	}
	else
	{
		return FALSE;
	}

}

//==============================================================================
//
// 	Function Name:	CMainView::LoadMedia()
//
// 	Description:	This function is called to load the media object obtained
//					from the database.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
CMedia* g_pMedia; 
long g_lSecondary; 
long g_lTertiary;

BOOL CMainView::LoadMedia(CMedia* pMedia, long lSecondary, long lTertiary)
{
	SMultipageInfo	MPInfo;
	SPlaylistParams	PLParams;
	SShowInfo		ShowInfo;
	CString			strError;
	
	//	Make sure the automatic transition is turned off if this is not a link event
	if(m_AppLink.GetIsEvent() == FALSE)
		StopAutoTransition();

	//	Make sure the database is open
	if((m_pDatabase == 0) || (m_pDatabase->IsOpen() == FALSE))
		return FALSE;

	//	If this is a PowerPoint presentation make sure the TMPower control is
	//	properly initialized before doing anything
	if((pMedia->m_lPlayerType == MEDIA_TYPE_POWERPOINT) && 
	   (m_ctrlTMPower.IsInitialized() == FALSE))
	{
		//	Display an error if PowerPoint support is enabled
		if(m_bEnablePowerPoint)
			HandleError(0, IDS_POWERPOINT_NOTINITIALIZED);

		return FALSE;
	}

	//	Update the barcode information
	m_Barcode.m_strMediaId   = pMedia->m_strMediaId;
	m_Barcode.m_lSecondaryId = lSecondary;
	m_Barcode.m_lTertiaryId  = lTertiary;

	//	What type of media is this?
	switch(pMedia->m_lPlayerType)
	{
		case MEDIA_TYPE_IMAGE:
		case MEDIA_TYPE_RECORDING:
		case MEDIA_TYPE_POWERPOINT:
					
			if((MPInfo.pMultipage = m_pDatabase->GetMultipage(pMedia)) == 0)
			{
				HandleError(0, IDS_NOMULTIPAGE, pMedia->m_strMediaId);
				return FALSE;
			}
			
			//	Should we convert the recording to a playlist?
			if((pMedia->m_lPlayerType == MEDIA_TYPE_RECORDING) && 
			   (m_bLoadingShowItem == FALSE) &&
			   (m_bClipsAsPlaylists == TRUE))
			{
				return LoadAsPlaylist(MPInfo.pMultipage, lSecondary);
			}

			//	Get the page we are supposed to display
			if(!SetPageFromBarcode(&MPInfo, lSecondary, lTertiary))
			{
				//	We're we attempting to load a treatment?
				if(lTertiary > 0)
				{
					strError.Format("Media Id: %s Page: %ld Treatment: %ld",
									MPInfo.pMultipage->m_strMediaId, 
									lSecondary, lTertiary);
					HandleError(0, IDS_TREATMENTNOTFOUND, strError);
				}
				else
				{
					strError.Format("Media Id: %s Secondary Id: %ld", 
									MPInfo.pMultipage->m_strMediaId, lSecondary);
					HandleError(0, IDS_PAGENOTFOUND, strError);
				}

				DbgMsg(&MPInfo, "LoadMedia->SetPageFromBarcode Failed: ");
				delete MPInfo.pMultipage;
				return FALSE;
			}

			//	Load the page
			if(!LoadMultipage(&MPInfo))
			{
				DbgMsg(&MPInfo, "LoadMedia->LoadMultipage Failed: ");
				delete MPInfo.pMultipage;
				return FALSE;
			} else {

				g_pMedia = pMedia;
				g_lSecondary = lSecondary;
				g_lTertiary = lTertiary;
			}

			break;

		case MEDIA_TYPE_PLAYLIST:

			if((PLParams.pPlaylist = m_pDatabase->GetPlaylist(pMedia)) == 0)
			{
				HandleError(0, IDS_NOPLAYLIST, pMedia->m_strMediaId);
				return FALSE;
			}

			//	Now we need to set up the playlist information
			if((m_iCuePage > 0) && (m_iCueLine > 0))
			{
				//	Translate the line specification
				if(!TranslateLine(&PLParams))
				{
					delete PLParams.pPlaylist;
					strError = "Line specification out of range";
					SetNextLine(strError);
					return FALSE;
				}
			}
			else
			{
				//	Set the appropriate designation
				if(lSecondary <= 0)
				{
					PLParams.pStart = PLParams.pPlaylist->m_Designations.First();
					PLParams.lStart = -1L;
				}
				else
				{
					if(m_iPlaylistSecondary == SECONDARY_AS_ORDER)
						PLParams.pStart = PLParams.pPlaylist->m_Designations.FindFromOrder(lSecondary);
					else
						PLParams.pStart = PLParams.pPlaylist->m_Designations.FindFromBarcode(lSecondary);
			
					//	Use the playback order to set the start identifier
					if(PLParams.pStart)
						PLParams.lStart = PLParams.pStart->m_lPlaybackOrder;
				}
			
				//	Do we have a start designation?
				if(PLParams.pStart == 0)
				{
					strError.Format("Media Id: %s Secondary Id: %ld", 
									PLParams.pPlaylist->m_strMediaId, lSecondary);
					HandleError(0, IDS_PLAYLISTNOTFOUND, strError);
					delete PLParams.pPlaylist;
					return FALSE;
				}

				//	Set the stop designation
				if((m_bRunToEnd || lSecondary <= 0) && (m_bLoadingShowItem == FALSE))
				{
					PLParams.lStop = -1;
					PLParams.pStop = PLParams.pPlaylist->m_Designations.Last();
				}
				else
				{
					PLParams.lStop = PLParams.pStart->m_lPlaybackOrder;
					PLParams.pStop = PLParams.pStart;
				}

				//	We are not cueing to a specific frame
				PLParams.dPosition = -1.0;
			}

			//	Execute the playlist
			if(!LoadPlaylist(&PLParams))
			{
				delete PLParams.pPlaylist;
				return FALSE;
			}

			break;

		case MEDIA_TYPE_CUSTOMSHOW:

			//	Get the custom show object from the database
			if((ShowInfo.pShow = m_pDatabase->GetShow(pMedia, -1)) == 0)
			{
				HandleError(0, IDS_NOSHOW, pMedia->m_strMediaId);
				return FALSE;
			}
			
			//	What is the initial show item?
			if(lSecondary > 0)
			{
				//	Are we treating the secondary as a playback order?
				if(m_iCustomShowSecondary == SECONDARY_AS_ORDER)
					ShowInfo.pItem = ShowInfo.pShow->FindByOrder(lSecondary);
				else
					ShowInfo.pItem = ShowInfo.pShow->FindByBarcodeId(lSecondary);
			}
			else
			{
				ShowInfo.pItem = ShowInfo.pShow->m_Items.First();
			}
					
			//	Were we able to find an initial item
			if(ShowInfo.pItem == 0)
			{
				strError.Format("Media Id: %s Secondary Id: %ld", 
								ShowInfo.pShow->m_strMediaId, lSecondary);
				HandleError(0, IDS_SHOWITEMNOTFOUND, strError);
				delete ShowInfo.pShow;
				return FALSE;
			}

			//	Always reset the user runtime flag when we load a new playlist
			m_bUserScrollEnabled = TRUE;

			//	Load the custom show
			if(LoadShowItem(&ShowInfo))
			{
				//	Update the persistant custom show information
				if((m_Show.pShow != 0) && (m_Show.pShow != ShowInfo.pShow))
					delete m_Show.pShow;
				
				m_Show.pShow = ShowInfo.pShow;
				m_Show.pItem = ShowInfo.pItem;
			}
			else
			{
				delete ShowInfo.pShow;
				return FALSE;
			}

			break;
			
		case MEDIA_TYPE_DEPOSITION:

			//	Are we loading a custom show item?
			if(m_bLoadingShowItem == TRUE)
			{
				ASSERT(m_pDatabase->GetScriptId() > 0);
				ASSERT(m_pDatabase->GetSceneId() > 0);
				if(m_pDatabase->GetScriptId() <= 0) return FALSE;
				if(m_pDatabase->GetSceneId() <= 0) return FALSE;

				//	Create a playlist that contains only this scene
				if((PLParams.pPlaylist = m_pDatabase->GetPlaylistFromScene(m_pDatabase->GetSceneId())) == 0)
				{
					HandleError(0, IDS_NOPLAYLIST, pMedia->m_strMediaId);
					return FALSE;
				}

				//	There is only one designation to play
				PLParams.pStart = PLParams.pPlaylist->m_Designations.First();
				ASSERT(PLParams.pStart != 0);
				PLParams.lStart = PLParams.pStart->m_lPlaybackOrder;
				PLParams.lStop = PLParams.pStart->m_lPlaybackOrder;
				PLParams.pStop = PLParams.pStart;
				PLParams.dPosition = -1.0;

				//	Execute the playlist
				if(!LoadPlaylist(&PLParams))
				{
					delete PLParams.pPlaylist;
					return FALSE;
				}

			}
			else
			{
				//	Load this deposition
				return LoadDeposition(pMedia, lSecondary);

			}// if(m_bLoadingShowItem == TRUE)

			break;

		default:
		
			HandleError(0, IDS_INVALIDMEDIATYPE, pMedia->m_strMediaId);
			break;
	}			

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CMainView::LoadMultipage()
//
// 	Description:	This function is called to load the multipage object 
//					provided by the caller.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::LoadMultipage(SMultipageInfo* pInfo)
{
	ASSERT(pInfo);
	ASSERT(pInfo->pMultipage);
	ASSERT(pInfo->pSecondary);

	//	Just in case
	if((pInfo == 0) || (pInfo->pMultipage == 0) || (pInfo->pSecondary == 0))
		return FALSE;

DbgMsg("LoadMultipage: %s.%ld.%ld", pInfo->pMultipage->m_strMediaId,
								    pInfo->pSecondary->m_lBarcodeId,
								    pInfo->pTertiary != 0 ? pInfo->pTertiary->m_lBarcodeId : 0);

	//	What type of media is this?
	switch(pInfo->pMultipage->m_lPlayerType)
	{
		case MEDIA_TYPE_IMAGE:

			//	Treat this as a linked image if we are playing video
			if(GetLinkState())
			{
				//	Is this a high-res document?
				if(pInfo->pSecondary->m_lDisplayType == DISPLAY_TYPE_HIRESPAGE)
				{
					//	Set the link flags if this is not a link event and
					//	if the user is not already linked
					if(!m_AppLink.GetIsEvent())
					{
						m_AppLink.SetBarcode(GetBarcode(*pInfo));
						m_AppLink.SetHideText(!IsScrollTextVisible());
						m_AppLink.SetHideVideo(!IsVideoVisible());

						//	Use the system level setting if not already linked
						if(m_sState != S_LINKEDIMAGE) 
							m_AppLink.SetSplitScreen(m_bSplitScreenDocuments);
					}
					ProcessEvent(E_LINKDOCUMENT, (DWORD)pInfo);
				
				}
				else
				{
					//	Set the link flags if this is not a link event
					if(!m_AppLink.GetIsEvent())
					{
						m_AppLink.SetBarcode(GetBarcode(*pInfo));
						m_AppLink.SetHideText(!IsScrollTextVisible());
						m_AppLink.SetHideVideo(!IsVideoVisible());

						if(m_sState != S_LINKEDIMAGE)
							m_AppLink.SetSplitScreen(m_bSplitScreenGraphics);
					}
					ProcessEvent(E_LINKGRAPHIC, (DWORD)pInfo);

				}// if(pInfo->pSecondary->m_lDisplayType == DISPLAY_TYPE_HIRESPAGE)
			
			}
			else
			{

				//	Is this a high-res document?
				if(pInfo->pSecondary->m_lDisplayType == DISPLAY_TYPE_HIRESPAGE)
					ProcessEvent(E_LOADDOCUMENT, (DWORD)pInfo);
				else
					ProcessEvent(E_LOADGRAPHIC, (DWORD)pInfo);
			}
			break;

		case MEDIA_TYPE_RECORDING:
			
			ProcessEvent(E_LOADMOVIE, (DWORD)pInfo);
			break;

		case MEDIA_TYPE_POWERPOINT:

			//	Treat this as a linked presentation if we are playing video
			if(GetLinkState())
			{
				//	Set the link flags if this is not a link event
				if(!m_AppLink.GetIsEvent())
				{
					m_AppLink.SetBarcode(GetBarcode(*pInfo));
					m_AppLink.SetHideText(!IsScrollTextVisible());
					m_AppLink.SetHideVideo(!IsVideoVisible());

					if(m_sState != S_LINKEDPOWER)
						m_AppLink.SetSplitScreen(m_bSplitScreenPowerPoints);
				}
				ProcessEvent(E_LINKPOWER, (DWORD)pInfo);
			}
			else
			{

				ProcessEvent(E_LOADPOWER, (DWORD)pInfo);
			}
			break;

		default:
		
			return FALSE;
	}			

	return TRUE;
}
	
//==============================================================================
//
// 	Function Name:	CMainView::LoadPageFromKeyboard()
//
// 	Description:	This function is called by the keyboard hook to load the
//					page entered by the user
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::LoadPageFromKeyboard(long lPage)
{
	SShowInfo		Show;
	SMultipageInfo	Info;
	CString			strError;
	int				iLookup;

	if(!IsCommandEnabled(TMAX_NEWPAGE))
		return;

	if((m_sState != S_CLEAR) && (m_Show.pShow != 0) && (m_Show.pItem != 0))
	{
		//	Initialize the new show information
		Show.pShow = m_Show.pShow;
		if((Show.pItem = Show.pShow->FindByOrder(lPage)) != 0)
		{
			//	Load this item
			if(LoadShowItem(&Show))
				m_Show.pItem = Show.pItem;
		}
	}
	else
	{
		//	Which lookup method are we supposed to use?
		if(m_iImageSecondary == SECONDARY_AS_ORDER)
			iLookup = SETPAGE_BYORDER;
		else
			iLookup = SETPAGE_BYID;
			
		//	Load the specified page
		if(SetPageFromId(&Info, lPage, iLookup))
		{
			LoadMultipage(&Info);
		}
		else
		{
			strError.Format("Id -> %ld", lPage); 
			HandleError(0, IDS_PAGENOTFOUND, strError);
		}
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::LoadPlaylist()
//
// 	Description:	This function is called to load the playlist object obtained
//					from the database.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::LoadPlaylist(SPlaylistParams* pParams)
{
	BOOL	bReturn = FALSE;
	short	sEvent = E_LOADTEXT;

	ASSERT(pParams);
	ASSERT(pParams->pPlaylist);
	ASSERT(pParams->pStart);

	//	Always reset the user runtime flag when we load a new playlist
	m_bUserScrollEnabled = TRUE;

	//	Should we go to TEXT mode automatically?
	if(m_bSystemScrollEnabled == FALSE) 
		sEvent = E_LOADPLAYLIST;
	else if(pParams->pStart->GetScrollTextEnabled() == FALSE) 
		sEvent = E_LOADPLAYLIST;

	//	Process the event to load the playlist
	if((bReturn = ProcessEvent(sEvent, (DWORD)pParams)) == TRUE)
	{
		//	Save the information for the last playlist only if it's not an
		//	animation or a converted deposition
		if((pParams->pPlaylist->GetIsRecording() == TRUE) || 
		   (pParams->pPlaylist->GetIsDeposition() == TRUE))
		{
			//	Clear this without updating the ini file so that we can 
			//	determine if the SetPageLine commands should be enabled
			m_strLastPlaylist.Empty();
		}
		else
		{
			SetLastPlaylist(pParams->pPlaylist);
		}
	
	}// if((bReturn = ProcessEvent(sEvent, (DWORD)pParams)) == TRUE)
	
	return bReturn;
}

//==============================================================================
//
// 	Function Name:	CMainView::LoadShowItem()
//
// 	Description:	This function will load the custom show item provided by
//					the caller.
//
// 	Returns:		TRUE if successful
//
//	Notes:			We could probably process this through our normal barcode
//					processor but having a dedicated function allows us to
//					do some custom error handling.
//
//==============================================================================
BOOL CMainView::LoadShowItem(SShowInfo* pInfo)
{
	CMedia*		pMedia;
	CBarcode	Barcode;
	CString		strActivity;
	BOOL		bLoaded = FALSE;
		
	ASSERT(m_pDatabase != 0);
	ASSERT(m_pDatabase->IsOpen() == TRUE);
	if((m_pDatabase == 0) || (m_pDatabase->IsOpen() == FALSE))
		return FALSE;

	ASSERT(pInfo != 0);
	ASSERT(pInfo->pItem != 0);

	//	Kill the automatic transition
	StopAutoTransition();

	//	Do we need to get the item barcode?
	//
	//	NOTE:	.NET databases store unique identifiers that must be translated to
	//			user type barcodes
	if(pInfo->pItem->m_strItemBarcode.GetLength() == 0)
	{
		if(pInfo->pItem->m_strSourcePST.GetLength() > 0)
		{
			m_pDatabase->GetBarcode(pInfo->pItem->m_strSourcePST, pInfo->pItem->m_strItemBarcode);
		}

	}

	//	Log the barcode if we're running a test
	if(m_Test.bActive)
	{
		strActivity.Format("Loading Show Item: %s", pInfo->pItem->m_strItemBarcode);
		UpdateActivityLog(strActivity);
	}

	//	Initialize the barcode using the caller's string
	if(!Barcode.SetBarcode(pInfo->pItem->m_strItemBarcode))
	{
		HandleError(0, IDS_INVALIDITEMBARCODE, pInfo->pItem->m_strItemBarcode);
		return FALSE;
	}

	//	Custom show items require a secondary id
	if(Barcode.m_lSecondaryId <= 0)
	{
		HandleError(0, IDS_NOITEMSECONDARY, pInfo->pItem->m_strItemBarcode);
		return FALSE;
	}

	//	Get the primary media information associated with the show item
	if((pMedia = m_pDatabase->GetMedia(Barcode.m_strMediaId)) == 0)
	{
		HandleError(0, IDS_NOITEMMEDIA, pInfo->pItem->m_strItemBarcode);
		return FALSE;
	}

	//	We do not support nested custom shows
	if(pMedia->m_lPlayerType == MEDIA_TYPE_CUSTOMSHOW)
	{
		HandleError(0, IDS_NONESTEDSHOWS, pInfo->pItem->m_strItemBarcode);
		return FALSE;
	}

	//	Let the database know that we are loading a scene in a custom show
	m_pDatabase->SetScriptId(pInfo->pShow->m_lPrimaryId);
	m_pDatabase->SetSceneId(pInfo->pItem->m_lSecondaryId);

	//	Set the flag to indicate that we are loading a show item
	m_bLoadingShowItem = TRUE;

	//	Load the media object
	if((bLoaded = LoadMedia(pMedia, Barcode.m_lSecondaryId, Barcode.m_lTertiaryId)) == TRUE) 
	{
		//	Update the persistant barcode information
		UpdateStatusBar();
	}

	//	Reset the flags
	m_bLoadingShowItem = FALSE;
	m_pDatabase->SetScriptId(0);
	m_pDatabase->SetSceneId(0);

	//	Set the automatic transition?
	if((pInfo->pItem->m_bAutoTransition == TRUE) &&
	   (pInfo->pItem->m_bStaticScene == TRUE) && 
	   (pInfo->pShow->IsLast(pInfo->pItem) == FALSE))
	{
		pInfo->pItem->m_ulGoToNext = (GetTickCount() + pInfo->pItem->m_lTransitionPeriod);
	}
	else
	{
		pInfo->pItem->m_ulGoToNext = 0;
	}

	return bLoaded;
	
}

//==============================================================================
//
// 	Function Name:	CMainView::LoadTreatment()
//
// 	Description:	Called to load the specified treatment
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::LoadTreatment(SMultipageInfo* pMPTreatment, short sState)
{
	SMultipageInfo	MPPrimary;
	SMultipageInfo	MPSibling;
	SMultipageInfo*	pTMViewPrimary = NULL;
	SMultipageInfo*	pTMViewSibling = NULL;
	SMultipageInfo*	pMPUpdate = NULL;
	SMultipageInfo*	pMPActive = NULL;
	CString			strPageFileSpec = "";
	CString			strZapFileSpec = "";
	CString			strSiblingPageFileSpec = "";
	CString			strSiblingZapFileSpec = "";
	short			sActivePaneId = -1;
	short			sSiblingPaneId = -1;
	short			sPrimaryPaneId = -1;
		
	memset(&MPPrimary, 0, sizeof(MPPrimary));
	memset(&MPSibling, 0, sizeof(MPSibling));

	//	Just in case ...
	ASSERT_RET_BOOL(m_pDatabase != NULL);
	ASSERT_RET_BOOL(pMPTreatment != NULL);
	ASSERT_RET_BOOL(pMPTreatment->pMultipage != NULL);
	ASSERT_RET_BOOL(pMPTreatment->pSecondary != NULL);
	ASSERT_RET_BOOL(pMPTreatment->pTertiary != NULL);

	//	Get the page file and treatment file specifications
	if(GetTreatmentFileSpecs(pMPTreatment, strPageFileSpec, strZapFileSpec, TRUE) == FALSE)
		return FALSE;

	//	Is this as a split screen treatment?
	if(pMPTreatment->pTertiary->GetSplitScreen() == TRUE)
	{
		//	Only load as split screen for image viewing states
		if((sState == S_DOCUMENT) || (sState == S_GRAPHIC))
		{
			if(SetMultipageInfo(MPSibling, pMPTreatment->pTertiary->m_strSiblingId, FALSE) == FALSE)
			{
				HandleError(0, IDS_SIBLING_NOT_FOUND, pMPTreatment->pTertiary->m_strSiblingId);
				return FALSE;
			}
			else
			{
				//	Get the file paths for the sibling treatment
				if(GetTreatmentFileSpecs(&MPSibling, strSiblingPageFileSpec, strSiblingZapFileSpec, TRUE) == FALSE)
				{
					DbgMsg(&MPSibling, "LoadTreatment->GetTreatmentFileSpecs Failed: ");
					delete MPSibling.pMultipage;
					return FALSE;
				}
			}

		}// if((sState == S_GRAPHIC) || (sState == S_GRAPHIC))

	}// if(pInfo->pTertiary->GetSplitScreen() == TRUE)

	//	Are we loading a split screen treatment?
	if(MPSibling.pTertiary != NULL)
	{
		//	Make sure we are in split screen mode
		if(m_ctrlTMView->GetSplitHorizontal() != pMPTreatment->pTertiary->GetSplitHorizontal())
			m_ctrlTMView->SetSplitHorizontal(pMPTreatment->pTertiary->GetSplitHorizontal());
		
		if(m_ctrlTMView->GetSplitScreen() != TRUE)
			m_ctrlTMView->SetSplitScreen(TRUE);
		
		//	What pane is currently active?
		sActivePaneId = m_ctrlTMView->GetActivePane();

		//	Set the correct color of the split screen frame
		SetZapSplitScreen(TRUE);

		//	Which pane should the treatments be loaded in?
		if(MPSibling.pTertiary->GetSplitRight() == TRUE)
		{
			sPrimaryPaneId = TMV_LEFTPANE;
			sSiblingPaneId = TMV_RIGHTPANE;
		}
		else
		{
			sPrimaryPaneId = TMV_RIGHTPANE;
			sSiblingPaneId = TMV_LEFTPANE;
		}

		//	Get the multipage information bound to each pane
		pTMViewPrimary = (SMultipageInfo*)m_ctrlTMView->GetData(sPrimaryPaneId);
		pTMViewSibling = (SMultipageInfo*)m_ctrlTMView->GetData(sSiblingPaneId);

		ASSERT_RET_BOOL(pTMViewPrimary != NULL);
		ASSERT_RET_BOOL(pTMViewSibling != NULL);
		ASSERT_RET_BOOL(pTMViewPrimary != pTMViewSibling);

		//	Clear the values currently assigned to each of the panes
		//	before loading the treatments. 
		//
		//	NOTE:	We have to do this because we don't want to accidentally
		//			delete the primary treatment object by loading the 
		//			sibling into the pane that it might currently be assigned 
		//			to.
		//
		//			Also, we don't have to test for the MPSibling object because
		//			we know it was allocated in this call
		memcpy(&MPPrimary, pMPTreatment, sizeof(SMultipageInfo));

		if((pTMViewPrimary->pMultipage != NULL) && (pTMViewPrimary->pMultipage != MPPrimary.pMultipage))
			delete pTMViewPrimary->pMultipage;
		memset(pTMViewPrimary, 0, sizeof(SMultipageInfo));

		if((pTMViewSibling->pMultipage != NULL) && (pTMViewSibling->pMultipage != MPPrimary.pMultipage))
			delete pTMViewSibling->pMultipage;
		memset(pTMViewSibling, 0, sizeof(SMultipageInfo));

		//	Suppress handling of the ChangePane events while we load
		m_bLoadingSplitZap = TRUE;

		//	Load the treatments into the appropriate panes
		LoadTreatment(&MPSibling, strSiblingZapFileSpec, strSiblingPageFileSpec, sSiblingPaneId);
		LoadTreatment(&MPPrimary, strZapFileSpec, strPageFileSpec, sPrimaryPaneId);

		m_bLoadingSplitZap = FALSE;

		//	Restore the active pane
		m_ctrlTMView->SetActivePane(sActivePaneId);
	}
	else
	{
		//	Force single pane mode if running a custom show
		if(m_bLoadingShowItem == TRUE)
			SetSinglePaneMode();
		else
			SetZapSplitScreen(FALSE);

		LoadTreatment(pMPTreatment, strZapFileSpec, strPageFileSpec);
	
	}// if(MPSibling.pTertiary != NULL)

	//	Update the barcode information
	if((pMPActive = (SMultipageInfo*)m_ctrlTMView->GetData(-1)) != NULL)
	{
		m_Barcode.m_strMediaId   = pMPActive->pMultipage->m_strMediaId;
		m_Barcode.m_lSecondaryId = pMPActive->pSecondary->m_lBarcodeId;
		m_Barcode.m_lTertiaryId  = pMPActive->pTertiary->m_lBarcodeId;
		UpdateStatusBar();
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CMainView::LoadTreatment()
//
// 	Description:	Called to load the specified treatment into the specified
//					pane
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::LoadTreatment(SMultipageInfo* pMPTreatment, LPCSTR lpszZapFileSpec,
							  LPCSTR lpszSourceFileSpec, short sPane)
{
	SMultipageInfo*	pTMViewInfo = NULL;
		
	//	Just in case ...
	ASSERT_RET_BOOL(m_pDatabase != NULL);
	ASSERT_RET_BOOL(lpszZapFileSpec != NULL);
	ASSERT_RET_BOOL(pMPTreatment != NULL);
	ASSERT_RET_BOOL(pMPTreatment->pMultipage != NULL);
	ASSERT_RET_BOOL(pMPTreatment->pSecondary != NULL);
	ASSERT_RET_BOOL(pMPTreatment->pTertiary != NULL);

	//	Get the data descriptor bound to the TMView pane
	if((pTMViewInfo = (SMultipageInfo*)m_ctrlTMView->GetData(sPane)) != NULL)
	{
		//	Update the data associated with the pane
		//
		//	NOTE:	We do this before loading the viewer so that the correct
		//			record objects have been assigned to the pane before
		//			it fires any events
		UpdateMultipage(pTMViewInfo, pMPTreatment);

		//	Load the treatment
		if(pMPTreatment->pSecondary->m_lDisplayType == DISPLAY_TYPE_HIRESPAGE)
			m_ctrlTMView->LoadZap(lpszZapFileSpec, TRUE, m_bScaleDocs, m_ctrlTMView->IsWindowVisible(), sPane, lpszSourceFileSpec);
		else
			m_ctrlTMView->LoadZap(lpszZapFileSpec, TRUE, m_bScaleGraphics, m_ctrlTMView->IsWindowVisible(), sPane, lpszSourceFileSpec);
	}


	return TRUE;

}

//==============================================================================
//
// 	Function Name:	CMainView::OnAddToBinder()
//
// 	Description:	This function will fire the Manager request to add the
//					the active media barcode to Manager's active binder
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAddToBinder() 
{
	if(!IsCommandEnabled(TMAX_ADD_TO_BINDER))
		return;

	//	Do we have an active barcode?
	if(m_Barcode.m_strMediaId.GetLength() > 0)
	{
		//	Is the manager running
		if(m_ctrlManagerApp.IsRunning() == TRUE)
		{
			//	Format a request to add the barcode
			m_ctrlManagerApp.SetCaseFolder(m_strCaseFolder);
			m_ctrlManagerApp.SetBarcode(m_Barcode.GetBarcode());
			m_ctrlManagerApp.SetCommand(TMSHARE_COMMAND_ADD_TO_BINDER);
			m_ctrlManagerApp.SetRequest(0);
		}

	}// if(m_Barcode.m_strMediaId.GetLength() > 0)

}

//==============================================================================
//
// 	Function Name:	CMainView::OnAnnText()
//
// 	Description:	This function is called to make the text tool the 
//					current drawing tool
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAnnText() 
{
	if(!IsCommandEnabled(TMAX_ANNTEXT))
		return;
	
	SetDrawingTool(ANNTEXT);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnArrow()
//
// 	Description:	This function is called to make the arrow tool the 
//					current drawing tool
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnArrow() 
{
	if(!IsCommandEnabled(TMAX_ARROW))
		return;
	
	SetDrawingTool(ARROW);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxButtonClick()
//
// 	Description:	This function handles all button click notifications fired
//					by one of the toolbar controls.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxButtonClick(short sId, BOOL bChecked) 
{
	static int iCount = 0;
	char tmp[512];
	//	Is the id within range?
	if(sId < 0 || sId >= TMTB_MAXBUTTONS)
		return;
	//	Make sure the toolbar does not have focus

	::SetFocus(m_hWnd);
	
	/*CRect rect;
	CWnd *pWnd = this->GetDlgItem(sId);
	pWnd->GetWindowRect(&rect);

	long leftOfButton = rect.left;
	long topOfButton = rect.top;*/

	//	Process the command
	ProcessCommand(ButtonMap[sId][m_sState]);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxButtonClickLarge()
//
// 	Description:	This function handles all button click notifications fired
//					by 8 colour buttons toolbar control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================

void CMainView::OnAxButtonClickLarge(short sId, BOOL bChecked) 
{
	
	short index;
	// Getting reference of Document and Large Toolbars	
	CTMTool* tempControl = m_aToolbars[S_DOCUMENT_LARGE].pControl;
	CTMTool* docControl = m_aToolbars[S_DOCUMENT].pControl;

	index = docControl->GetImageIndex(sId);

	// replacing large toolbar red color with pallete's selected color 
	docControl->SetButtonImage(45,tempControl->GetImageIndex(sId));
		index = docControl->GetImageIndex(sId);

	// Making clicked button check and other uncheck
	for(int i = 0; i < TMTB_MAXBUTTONS; i++)
	{
		//	Have we run out of buttons?
		if((m_aToolbars[S_DOCUMENT_LARGE].aMap[i] < 0) || (m_aToolbars[S_DOCUMENT_LARGE].aMap[i] >= TMTB_MAXBUTTONS))
			break;

		//	Is this button eliminated?
		if(m_aToolbars[S_DOCUMENT_LARGE].aMap[i] == '0' || i < 22)
			continue;

		//	Set the information for this button


		if (sId == m_aToolbars[S_DOCUMENT_LARGE].aMap[i])
		{
			tempControl->CheckButton(m_aToolbars[S_DOCUMENT_LARGE].aMap[i],true);
		}
		else 
			tempControl->CheckButton(m_aToolbars[S_DOCUMENT_LARGE].aMap[i],false);
	}		
	
	// setting red button
	if (sId == 45)
	{
		if(!IsCommandEnabled(TMAX_RED))
			return;
		m_ctrlTMView->SetColor(TMV_RED);
		UpdateToolColor();
		return;
	}

	//	Is the id within range?
	if(sId < 0 || sId >= TMTB_MAXBUTTONS)
		return;
	//	Process the command
	ProcessCommand(ButtonMap[sId][m_sState]);

}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxClickLightPen()
//
// 	Description:	This function handles all MouseClick events fired by the
//					light pen control
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxClickLightPen(short sButton, short sKeyState) 
{
	//	Is this a right button click?
	if(sButton == TMLPEN_RIGHT)
	{
		//	Toggle the toolbar visibility
		OnShowToolbar();
	}
	else
	{
		//	Clear the screen or restore
		ProcessCommand(TMAX_CLEAR);
	}	
}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxClickStatusBar()
//
// 	Description:	This function handles click events fired by the status
//					bar control
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxClickStatusBar() 
{
	//	Turn the status bar off
	ProcessCommand(TMAX_STATUSBAR);	
}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxCloseTextBox()
//
// 	Description:	This function handle events fired by the TMView control
//					when it closes the dialog box that prompts the user for the
//					text associated with an annotation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxCloseTextBox(short sPane) 
{
	char szFont[256];

	//	Enable the keyboard hook
	theApp.EnableHook(TRUE);	
	theApp.EnableEscapeHook(FALSE);	

	//	Update the ini file in case the user changed the font
	m_Ini.SetTMSection(PRESENTATION_APP);
	lstrcpyn(szFont, m_ctrlTMView->GetAnnFontName(), sizeof(szFont));
	m_Ini.WriteLong(ANNFONTSIZE_LINE, m_ctrlTMView->GetAnnFontSize());
	m_Ini.WriteBool(ANNFONTSTRIKETHROUGH_LINE, m_ctrlTMView->GetAnnFontStrikeThrough());
	m_Ini.WriteBool(ANNFONTUNDERLINE_LINE, m_ctrlTMView->GetAnnFontUnderline());
	m_Ini.WriteBool(ANNFONTBOLD_LINE, m_ctrlTMView->GetAnnFontBold());
	m_Ini.WriteString(ANNFONTNAME_LINE, szFont);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxCreateCallout()
//
// 	Description:	This function traps callout creation events to ensure the
//					main window retains the keyboard focus.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxCreateCallout(long hCallout) 
{
	//	Grab the keyboard focus
	::SetFocus(m_hWnd);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxDesignationChange()
//
// 	Description:	This function handles DesignationChange events fired by the
//					TMMovie control
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxDesignationChange(long lId, long lOrder) 
{
	//	Update the status bar information
	m_PlaylistStatus.lDesignationOrder = lOrder;
	UpdateStatusBar();

	//	Get the active designation
	if(m_Playlist.pPlaylist != NULL)
		m_Playlist.pDesignation = m_Playlist.pPlaylist->m_Designations.FindFromId(lId);
	else
		m_Playlist.pDesignation = NULL;

	//	Check to see if we need to switch the screen state
	if(m_Playlist.pDesignation != NULL)
	{
		switch(m_sState)
		{
			case S_TEXT:

				//	Should we turn off the scrolling text?
				if(m_Playlist.pDesignation->GetScrollTextEnabled() == FALSE)
					SetDisplay(S_PLAYLIST);
				break;

			case S_PLAYLIST:

				//	Should we turn ON the scrolling text?
				if(GetScrollTextEnabled(m_Playlist.pDesignation) == TRUE)
					SetDisplay(S_TEXT);
				break;

			case S_LINKEDIMAGE:
			case S_LINKEDPOWER:

				//	Do we need to recalculate the layout?
				if(CheckLinkOptions() == TRUE)
					SetDisplay(m_sState);
				break;

		}// switch(m_sState)

	}// if(m_Playlist.pDesignation != NULL)

}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxDesignationTime()
//
// 	Description:	This function handles DesignationTime events fired by the
//					TMMovie control
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxDesignationTime(double dTime) 
{
	//	Update the status bar information
	UpdatePlaylistStatus();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxDestroyCallout()
//
// 	Description:	This function traps callout destruction events to ensure the
//					main window retains the keyboard focus.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxDestroyCallout(long hCallout) 
{
	//	Grab the keyboard focus
	::SetFocus(m_hWnd);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxElapsedTimes()
//
// 	Description:	This function handles ElapsedTime events fired by the
//					TMMovie control
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxElapsedTimes(double dPlaylist, double dDesignation) 
{
	//	Update the status bar information
	UpdatePlaylistStatus();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxLineChange()
//
// 	Description:	This function handles LineChange events fired by the
//					TMMovie control
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxLineChange(long lLine) 
{
	m_Playlist.pLine = (CTextLine*)lLine;

	//	Are we loading a playlist?
	if(m_bLoadingPlaylist && m_ctrlTMText.IsReady())
	{
		//	Set the initial line
		m_ctrlTMText.SetLineObject(lLine, FALSE);
	}
	else
	{
		if(m_Playlist.pPlaylist == 0) return;

		//	Notify the text control but only redraw if it's visible
		m_ctrlTMText.SetLineObject(lLine, IsScrollTextVisible());
	}

	//	Update the playlist status bar information
	if(m_Playlist.pLine != NULL)
	{
		m_PlaylistStatus.lTextPage = m_Playlist.pLine->m_lPageNum;
		m_PlaylistStatus.lTextLine = m_Playlist.pLine->m_lLineNum;
	}
	else
	{
		m_PlaylistStatus.lTextPage = 0;
		m_PlaylistStatus.lTextLine = 0;
	}
	UpdateStatusBar();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxLinkChange()
//
// 	Description:	This function handles LinkChange events fired by the
//					TMMovie control
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxLinkChange(LPCTSTR lpszBarcode, long lId, long lFlags) 
{
	CString	strEvent = "";
	CString strBarcode = "";

	//	Save the link information
	m_AppLink.SetBarcode(lpszBarcode);
	m_AppLink.SetAttributes(lFlags);

	//	Do we need to convert from PSTQ to barcode?
	if((m_AppLink.GetHideLink() == FALSE) && (m_pDatabase->IsNETDatabase() == TRUE))
	{
		//	Move to the PSTQ buffer so that we can refire this event if necessary
		//
		//	NOTE: .NET actually passes the PSTQ identifier instead of the barcode
		m_AppLink.SetDatabaseId(lpszBarcode);
		m_AppLink.SetBarcode("");

		//	Let the database perform the conversion
		m_pDatabase->GetBarcode(lpszBarcode, m_AppLink.GetBarcode());
	}

	//	Is this a Hide Link notification?
	if(m_AppLink.GetHideLink() == TRUE)
	{
		//	Reset the link information but retain the flags
		m_AppLink.Clear();
		m_AppLink.SetAttributes(lFlags);

		//	Don't bother if links are disabled or we're loading a playlist
		if(!m_bDisableLinks && !m_bLoadingPlaylist)
		{
			if(m_Test.bActive)
				UpdateActivityLog("Hide linked image");

			//	Are we running a playlist?
			if(m_Playlist.pPlaylist != NULL)
			{
				if(GetLinkedTextEnabled() == TRUE)
					SetDisplay(S_TEXT);
				else
					SetDisplay(S_PLAYLIST);
			}
			else
			{
				SetDisplay(S_MOVIE);
			}

		}// if(!m_bDisableLinks && !m_bLoadingPlaylist)
	
	}
	else
	{
		//	Don't bother if links have been disabled
		if(m_bDisableLinks == FALSE)
		{
			//	Are we in the process of loading a playlist?
			if(m_bLoadingPlaylist)
			{
				//	Set up a pending link event
				m_AppLink.SetIsPending(TRUE);
			}
			else
			{
				//	Update the activity log if this is a test
				if(m_Test.bActive)
				{
					strEvent.Format("TMMovie link %s", m_AppLink.GetBarcode());
					UpdateActivityLog(strEvent);
				}

				//	This prevents LoadMultipage() from setting the link flags
				m_AppLink.SetIsEvent(TRUE);

				//	Pump the link through the normal barcode processor
				LoadFromBarcode(m_AppLink.GetBarcode(), FALSE);

				//	Clear the flags once we've processed the link
				m_AppLink.SetIsEvent(FALSE);
				m_AppLink.SetIsPending(FALSE);

			}// if(m_bLoadingPlaylist)

		}// if(m_bDisableLinks == FALSE)

	}// if(IsHidden(m_Link) == TRUE)

	//	Make sure the status bar is displaying the correct information
	UpdatePlaylistStatus();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxManagerRequest()
//
// 	Description:	This function handles all command request events fired by
//					the application sharing control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxManagerRequest() 
{
	if(m_ctrlManagerApp.GetRequest() == TMSHARE_ERROR_NONE)
	{
		//	Which command is being requested
		switch(m_ctrlManagerApp.GetCommand())
		{
			case TMSHARE_COMMAND_LOAD:

				OnManagerReqLoad();
				break;

			case TMSHARE_COMMAND_ADD_TREATMENT:

				//	Treat this the same as processing the response
				OnManagerResAddTreatment();
				break;

			case TMSHARE_COMMAND_ADD_TO_BINDER:

				ASSERT(FALSE);
				break;

		}

	}
	
}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxManagerResponse()
//
// 	Description:	This function handles all command response events fired by
//					the application sharing control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxManagerResponse() 
{
	if(m_ctrlManagerApp.GetResponse() == TMSHARE_ERROR_NONE)
	{
		//	Which command is being responded to?
		switch(m_ctrlManagerApp.GetCommand())
		{
			case TMSHARE_COMMAND_ADD_TREATMENT:

				OnManagerResAddTreatment();
				break;

			case TMSHARE_COMMAND_ADD_TO_BINDER:
			case TMSHARE_COMMAND_UPDATE_TREATMENT:

				//	No response processing required
				break;

		}

	}
	
}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxOpenTextBox()
//
// 	Description:	This function handle events fired by the TMView control
//					when it opens the dialog box that prompts the user for the
//					text associated with an annotation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxOpenTextBox(short sPane) 
{
	//	Disable the keyboard hook
	theApp.EnableHook(FALSE);
	theApp.EnableEscapeHook(FALSE);	
}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxPlaybackError()
//
// 	Description:	This function handles PlaybackError events fired by the
//					TMMovie control
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxPlaybackError(long lError, BOOL bStopped) 
{
	//	Don't worry about it if it didn't disrupt the playback
	if(!bStopped)
		return;

	//	Add an entry to the activity log if we are in test mode
	if(m_Test.bActive)
	{
		CString strText;
		if(bStopped)
			strText.Format("Playback Error #%ld STOPPED", lError);
		else
			strText.Format("Playback Error #%ld NOT STOPPED", lError);
		
		UpdateActivityLog(strText);
	}

	//	We are no longer playing
	m_bPlaying = FALSE;
	
	//	Set the toolbar button
	if(m_pToolbar)
		m_pToolbar->SetPlayButton(FALSE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxPlaylistState()
//
// 	Description:	This function handles PlaylistState events fired by the
//					TMMovie control
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxPlaylistState(short sState) 
{

	//	What state is the control in?
	switch(sState)
	{
		case TMMOVIE_PLFINISHED:
		case TMMOVIE_PLSTOPPED:
			
			//	Are we running a custom show?
			if(m_Show.pItem != 0)
			{
			}
			else
			{
				//	Should we clear the display?
				if(m_bClearPlaylist)
					SetDisplay(S_CLEAR);
			}

			//	Drop through
			//		.
			//		.
		
		case TMMOVIE_PLNONE:		
		case TMMOVIE_PLERROR:

			m_bEnablePlay = FALSE;
			m_bPlaying = FALSE;
			break;
		
		case TMMOVIE_PLACTIVE:
		case TMMOVIE_PLSET:

			m_bEnablePlay = TRUE;
			break;
		
	}

	UpdatePlaylistStatus();

	//	Are we testing playlists?
	if(sState == TMMOVIE_PLFINISHED && m_Test.bPlaylists)
		m_Test.lElapsed = m_Test.lPeriod - 1;
}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxPlaylistTime()
//
// 	Description:	This function handles PlaylistTime events fired by the
//					TMMovie control
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxPlaylistTime(double dTime) 
{
	//	Update the status bar information
	UpdatePlaylistStatus();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxPositionChange()
//
// 	Description:	This function is handles events fired by the TMMovie control
//					when the playback position changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxPositionChange(double dPosition) 
{		
	//	Are we viewing a clip with links?
	if((m_TMMovie.pTertiary != 0) && (m_TMMovie.pTertiary->m_Links.GetCount() > 0))
	{
		if((m_sState == S_MOVIE) || (m_sState == S_LINKEDIMAGE) || (m_sState == S_LINKEDPOWER))
		{
			//	Get the link at this position
			CLink* pLink = m_TMMovie.pTertiary->m_Links.FindAtPosition(dPosition);

			//	Has the link changed?
			if(m_TMMovie.pTertiary->m_pActiveLink != pLink)
			{
				m_TMMovie.pTertiary->m_pActiveLink = pLink;

				//	Make it look like TMMovie fired a link change event
				if(pLink != 0)
					OnAxLinkChange(pLink->m_strItemBarcode, pLink->m_lId, pLink->m_lFlags);
				else
					OnAxLinkChange("", 0, TMFLAG_LINK_HIDE);
			}
		
		}

	}

	//	Are we playing a custom show item?
	if((m_Show.pShow != 0) && (m_Show.pItem != 0) && 
	   (m_Show.pShow->IsLast(m_Show.pItem) == FALSE) &&
	   (m_Show.pItem->m_bTransitioned == FALSE))
	{
		//	Should we set the auto transition time?
		if((m_Show.pItem->m_bAutoTransition == TRUE) && 
		   (m_Show.pItem->m_bStaticScene == FALSE))
		{
			//	Have we reached the end?
			if(fabs(m_ctrlTMMovie.GetStopPosition() - dPosition) <= 0.033333)
			{
				//	Should we go immediately to the next item?
				if(m_Show.pItem->m_lTransitionPeriod == 0)
				{
					m_Show.pItem->m_bTransitioned = TRUE;
					OnNextShowItem();
				}
				else
				{
					m_Show.pItem->m_ulGoToNext = (GetTickCount() + m_Show.pItem->m_lTransitionPeriod);
				}
			
			}

		}

	}
	
}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxPowerFocus()
//
// 	Description:	This function is handles events fired by the TMPower control
//					fired when the view gains the focus.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxPowerFocus(short sView) 
{
	//	Post a message to capture the focus and restore the task bar
	PostMessage(WM_GRABFOCUS, 0);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxStartTextEdit()
//
// 	Description:	This function handle events fired by the TMView control
//					when the user starts to edit a text annotation.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxStartTextEdit(short sPane) 
{
	//	Disable the keyboard hook
	theApp.EnableHook(FALSE);	

	//	This is to prevent a problem we have with Lead Tools if the user cancels
	//	the session by hitting escape. Lead Tools does not give us an event that
	//	allows us to know the session has ended
	theApp.EnableEscapeHook(TRUE);	
}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxStateChange()
//
// 	Description:	This function handles StateChange events fired by the
//					TMMovie control
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxStateChange(short sState) 
{
	//	What is the state of the TMMovie control?
	switch(sState)
	{
		case TMMOVIE_PLAYING:	

			m_bEnablePlay = TRUE;
			m_bPlaying = TRUE;
			m_bCuedMovie = FALSE;
			break;

		case TMMOVIE_PAUSED:

			m_bEnablePlay = TRUE;
			m_bPlaying = FALSE;
			break;

		case TMMOVIE_STOPPED:

			m_bEnablePlay = TRUE;
			m_bPlaying = FALSE;
		
			//	Stop here unless we're playing a movie
			if(m_sState != S_MOVIE)
				break;

			//	Stop here if we cued to this position or if we don't have
			//	to worry about clearing the screen
			if(!m_bClearMovie || m_bCuedMovie)
				break;

			//	Have we reached the end of the movie?
			if(m_ctrlTMMovie.GetPosition() > 
			   (m_ctrlTMMovie.GetMaxTime() - 0.33333)) // MOD 6.0
					SetDisplay(S_CLEAR);

			break;

		case TMMOVIE_READY:
		case TMMOVIE_NOTREADY:
		default:
				
			m_bEnablePlay = FALSE;
			m_bPlaying = FALSE;
			m_bCuedMovie = FALSE;
			break;
	}

	//	Set the toolbar button
	if(m_pToolbar)
		m_pToolbar->SetPlayButton(m_bPlaying);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnAxStopTextEdit()
//
// 	Description:	This function handle events fired by the TMView control
//					when the user stops editing a text annotation.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnAxStopTextEdit(short sPane) 
{
	//	Reenable the keyboard hook
	theApp.EnableHook(TRUE);	
	theApp.EnableEscapeHook(FALSE);	
}

//==============================================================================
//
// 	Function Name:	CMainView::OnBackDesignation()
//
// 	Description:	This function will rewind the current designation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnBackDesignation() 
{
	if(!IsCommandEnabled(TMAX_BACKDESIGNATION))
		return;
	
	CuePlaylist(TMMCUEPL_STEP, FALSE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnBackMovie()
//
// 	Description:	This function will rewind the current movie
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnBackMovie() 
{
	if(!IsCommandEnabled(TMAX_BACKMOVIE))
		return;
	
	CueMovie(TMMCUE_RELATIVE, FALSE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnBlack()
//
// 	Description:	This function will set the annotation color to black.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnBlack() 
{
	if(!IsCommandEnabled(TMAX_BLACK))
		return;
	for(int i=0; i < SZ_ARR_TM_VW; i++)
		m_arrTmView[i]->SetColor(TMV_BLACK);
	UpdateToolColor();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnBlue()
//
// 	Description:	This function will set the annotation color to blue.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnBlue() 
{
	if(!IsCommandEnabled(TMAX_BLUE))
		return;
	
	for(int i=0; i < SZ_ARR_TM_VW; i++)
		m_arrTmView[i]->SetColor(TMV_BLUE);
	UpdateToolColor();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnCallout()
//
// 	Description:	This function will enable the callout tool.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnCallout() 
{
	if(!IsCommandEnabled(TMAX_CALLOUT))
		return;
	
	for(int i = 0; i < SZ_ARR_TM_VW; i++)
		m_arrTmView[i]->SetAction(CALLOUT);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnCaptureBarcodes()
//
// 	Description:	Called when user hits hotkey to capture barcodes stored
//					in external files
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnCaptureBarcodes() 
{
	CString	strFolder = "";
	CString	strFileSpec = "";
	CString	strMsg = "";

	m_ctrlTMGrab.SetArea(0);
	m_ctrlTMGrab.SetHotkey(0);
	m_ctrlTMGrab.SetSilent(TRUE);

	//	Build the path to the default root capture folder
	m_strCaptureFolder = theApp.GetAppFolder();
	if(m_strCaptureFolder.Right(1) != "\\")
		m_strCaptureFolder += "\\";
	m_strCaptureFolder += "_tmax_captures\\";

	//	Allocate an array to store the names of all capture files in the folder
	if(m_paCaptureFiles != NULL)
		delete m_paCaptureFiles;
	m_paCaptureFiles = new CStringArray();

	//	Get the array of capture source files
	if(GetCaptureSource(m_strCaptureFolder, *m_paCaptureFiles) == TRUE)
	{
		m_iCaptureFileIndex = 0;
		while(m_iCaptureFileIndex <= m_paCaptureFiles->GetUpperBound())
		{
			if(OpenCaptureSource() == TRUE)
				break;
		}
	}
	else
	{
		strMsg.Format("Unable to locate any capture source files in %s", strFolder);
		MessageBox(strMsg);
	}

}

//==============================================================================
//
// 	Function Name:	CMainView::OnChangePane()
//
// 	Description:	This function will handle pane selection events fired by
//					the TMView control
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnChangePane(short sPane) 
{
	SMultipageInfo* pInfo;

	//	What is the current state?
	switch(m_sState)
	{
		case S_DOCUMENT:
		case S_GRAPHIC:

			//	Don't bother doing anything if we are not in split screen or
			//	if we are in the process of loading a split screen treatment
			if(!m_ctrlTMView->GetSplitScreen()) return;
			if(m_bLoadingSplitZap == TRUE) return;

			//	Get the media information attached to the active pane
			pInfo = (SMultipageInfo*)m_ctrlTMView->GetData(TMV_ACTIVEPANE);

			DbgMsg(pInfo, "OnChangePane (%d): ", sPane);

			//	Do we have an active page?
			if((pInfo == 0) || (pInfo->pSecondary == 0))
			{
				//	Clear the active barcode information
				m_Barcode.m_strMediaId.Empty();
				m_Barcode.m_lSecondaryId = 0;
				m_Barcode.m_lTertiaryId = 0;
				UpdateStatusBar();
				return;
			}
			else
			{
				m_Barcode.m_strMediaId   = pInfo->pSecondary->m_strMediaId;
				m_Barcode.m_lSecondaryId = pInfo->pSecondary->m_lBarcodeId;
				if(pInfo->pTertiary != 0)
					m_Barcode.m_lTertiaryId = pInfo->pTertiary->m_lBarcodeId;
				else
					m_Barcode.m_lTertiaryId = 0;
				UpdateStatusBar();
			}

			//	Is the active page a document?
			if(pInfo->pSecondary->m_lDisplayType == DISPLAY_TYPE_HIRESPAGE)
			{
				//	We have to set this so that the system realizes what 
				//	state it is actually in as the user toggles panes
				m_sState = S_DOCUMENT;

				//	Do we already have the correct toolbar?
				if(m_pToolbar == m_aToolbars[S_DOCUMENT].pControl) return;

				//	Set the appropriate toolbar
				SelectToolbar(S_DOCUMENT);
			}
			else if(pInfo->pSecondary->m_lDisplayType == DISPLAY_TYPE_SCREENRESPAGE)
			{
				//	We have to set this so that the system realizes what 
				//	state it is actually in as the user toggles panes
				m_sState = S_GRAPHIC;

				//	Do we already have the correct toolbar?
				if(m_pToolbar == m_aToolbars[S_GRAPHIC].pControl) return;

				//	Set the appropriate toolbar
				SelectToolbar(S_GRAPHIC);
			}
			else
			{
				//	Do nothing if no file loaded yet
				return;
			}

			//	Is the toolbar visible?
			if((m_pToolbar == 0) || (m_ControlBar.iId != CONTROL_BAR_TOOLS))
				return;

			//	If the toolbar is docked we have to resize because the button
			//	sizes may have changed
			if(m_ControlBar.bDocked)
			{
				RecalcLayout(m_sState);
				m_ctrlTMView->MoveWindow(&m_rcView);
			}
			
			//	Make sure the toolbar remains visible
			m_pToolbar->ShowWindow(SW_SHOW);
			m_pToolbar->BringWindowToTop();

			//	Make sure the light pen control is sized and positioned 
			//	correctly if visible
			if(m_bPenControls)
				ShowLightPen(TRUE);

			break;
		
		case S_CLEAR:
		case S_LINKEDIMAGE:
		case S_PLAYLIST: 
		case S_MOVIE:
		case S_TEXT:
			
			return;
			
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::OnClear()
//
// 	Description:	This function will clear the screen or restore it to its
//					previous state.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnClear() 
{
	if(!IsCommandEnabled(TMAX_CLEAR))
		return;

	if(m_sState == S_CLEAR)
	{
		RestoreDisplay();
	}
	else
	{
		// close virtual keyborad
		HANDLE hProcessSnap;
		HANDLE hProcess;
		PROCESSENTRY32 pe32;
		DWORD dwPriorityClass;

		// Takes a snapshot of all the processes
		hProcessSnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);

		if(hProcessSnap == INVALID_HANDLE_VALUE){
		}

		pe32.dwSize = sizeof(PROCESSENTRY32);

		if(!Process32First(hProcessSnap, &pe32))
		{
			CloseHandle(hProcessSnap);     
		}

		do
		{
			//  checks if process at current position has the name of to be killed app
			if(!strcmp(pe32.szExeFile,"TabTip.exe")){
				// gets handle to process
				hProcess = OpenProcess(PROCESS_TERMINATE,0, pe32.th32ProcessID);

				// Terminate process by handle
				TerminateProcess(hProcess,0);
				CloseHandle(hProcess);
			} 
		}
		// gets next member of snapshot
		while(Process32Next(hProcessSnap,&pe32));

		// closes the snapshot handle
		CloseHandle(hProcessSnap);

		SetDisplay(S_CLEAR);
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::OnConfig()
//
// 	Description:	This function will open a dialog box that allows the user
//					to configure the drawing tools.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnConfig() 
{
	CSetup		Setup;
	BOOL		bPowerPoint = m_bEnablePowerPoint;
	CString		strFilters;
	CString		strFileSpec;
	long		lFilters;
	LPUNKNOWN	pIMediaControl;
	char		szFolder[256];

	if(!IsCommandEnabled(TMAX_CONFIG))
		return;
	
	//	Disable the keyboard hook
	theApp.EnableHook(FALSE);
	theApp.EnableEscapeHook(FALSE);	

	//	Turn off the toolbar and status bar
	if(m_pToolbar)
		m_pToolbar->ShowWindow(SW_HIDE);
	m_ctrlTMStat.ShowWindow(SW_HIDE);
	SetControlBar(CONTROL_BAR_NONE);
	
	//	Clear the screen and prevent any attempts to return to the previous
	//	state
	SetDisplay(S_CLEAR);
	m_sPrevState = S_CLEAR;

	//	Reset the display controls. DO NOT reset the player because we may
	//	want to view the filter properties
	ResetTMView();
	ResetTMPower();

	//	Initialize the dialog box
	strFileSpec = theApp.GetAppFolder();
	if((strFileSpec.GetLength() > 0) && (strFileSpec.Right(1) != "\\"))
		strFileSpec += "\\";
	strFileSpec.MakeLower();
	strFileSpec += "tmaxPresentation.exe";
	Setup.SetIniFile(m_Ini.strFileSpec);
	Setup.SetPresentationFileSpec(strFileSpec);

	//	NOTE:	We do not want to unload the movie player because we may want
	//			to view the properties for the current filters
	m_ctrlTMMovie.Pause();
	strFilters = m_ctrlTMMovie.GetActFilters(FALSE, &lFilters);
	pIMediaControl = m_ctrlTMMovie.GetInterface(TMMOVIE_IMEDIACONTROL);
	Setup.SetActiveFilters(strFilters, pIMediaControl);

	if(Setup.DoModal() == IDCANCEL)
	{	
		//	Grab the keyboard focus
		::SetFocus(m_hWnd);
		
		//	Enable the keyboard hook
		theApp.EnableHook(TRUE);
		theApp.EnableEscapeHook(FALSE);	
		
		//	We have to do this because clicking OK to close the dialog somehow
		//	makes the child windows visible
		if(m_pToolbar)
			m_pToolbar->ShowWindow(SW_HIDE);
		m_ctrlTMStat.ShowWindow(SW_HIDE);
		SetControlBar(CONTROL_BAR_NONE);
		SetDisplay(S_CLEAR);

		return;
	}

	//	We have to do this because clicking OK to close the dialog somehow
	//	makes the child windows visible
	if(m_pToolbar)
		m_pToolbar->ShowWindow(SW_HIDE);
	m_ctrlTMStat.ShowWindow(SW_HIDE);
	SetControlBar(CONTROL_BAR_NONE);
	SetDisplay(S_CLEAR);

	//	Update the setup options
	ReadSetup(FALSE);

	//	Get the new PowerPoint option
	if(m_bEnablePowerPoint != bPowerPoint)
	{
		//	Alert the user to restart the application
		MessageBox("The option to support Microsoft PowerPoint has changed. You must restart the application for it to take effect.",
		           "TrialMax", MB_ICONINFORMATION | MB_OK);
	}

	//	Get the case that has been selected by the user
	m_Ini.ReadLastCase(szFolder, sizeof(szFolder));

	//	Has the database changed?
	if(m_strCaseFolder.CompareNoCase(szFolder))
		OpenDatabase(szFolder);

	//	Grab the keyboard focus
	::SetFocus(m_hWnd);
	
	//	Enable the keyboard hook
	theApp.EnableHook(TRUE);
	theApp.EnableEscapeHook(FALSE);	
}

//==============================================================================
//
// 	Function Name:	CMainView::OnCreate()
//
// 	Description:	This function is called when the window is created.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
LRESULT CALLBACK OnDTMouseEvent(int nCode, WPARAM wParam, LPARAM lParam)
{
	
	// Doesn't concern us
    //if(nCode < 0)
    //    return CallNextHookEx(g_hDesktopHook, nCode, wParam, lParam);
    if(nCode == HC_ACTION)
    {
		if(wParam == WM_LBUTTONDOWN)
        {
			
			int x;
			int y;
			POINT cursorPos;

			if (GetCursorPos(&cursorPos) && m_pVKBDlgPtr)
			{
				RECT VKBRect;
				m_pVKBDlgPtr->GetWindowRect(&VKBRect);

				if ((cursorPos.x >=VKBRect.left && cursorPos.x <=(VKBRect.right-5)) &&  (cursorPos.y >=VKBRect.top && cursorPos.y <= VKBRect.bottom) && m_pVKBDlgPtr->IsWindowVisible())
				{
					ShellExecute( NULL, "open", "C:\\Program Files\\Common Files\\microsoft shared\\ink\\TabTip.exe", 
						NULL, NULL, SW_SHOWNORMAL );
				}
			}
			
			if(m_BinderList != 0)
			{
				m_BinderList->HandleMouseClick();
			}

			if(m_ColorPickerList != 0)
			{
				m_ColorPickerList->HandleMouseClick();
			}

			
			//if (ScreenToClient( m_pVKBDlgPtr->m_hWnd, &cursorPos))
			//{
			//	x = cursorPos.x;
			//	y = cursorPos.y;
			//	//p.x and p.y are now relative to hwnd's client area
			//}
		}

    }    

    return CallNextHookEx(g_hDesktopHook, nCode, wParam, lParam);
}

int CMainView::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
	CRect bmpRect;
	if(CFormView::OnCreate(lpCreateStruct) == -1)
		return -1;
	
	m_pVKBDlg = new CVKBDlg(this);
	m_pVKBDlg->Create(CVKBDlg::IDD);
	m_pVKBDlgPtr = m_pVKBDlg;
	m_pVKBDlg->GetClientRect(&bmpRect);
	m_pVKBDlg->MoveWindow(m_ScreenResolution.right - bmpRect.right - kbIconPadding ,  kbIconPadding , bmpRect.right , bmpRect.bottom );

	if((g_hDesktopHook = SetWindowsHookEx(WH_MOUSE_LL, OnDTMouseEvent, NULL, 0)) == NULL)
	{
		//AfxMessageBox("no hook");
		// Sorry, no hook for you...
	}	

	//CRect rect2 = m_ScreenResolution;
	//rect2.bottom = 2 * m_ScreenResolution.bottom;

	//MoveWindow(&rect2);
	//SetWindowPos(NULL, 0,0,m_ScreenResolution.right*2, 2 * m_ScreenResolution.bottom, 0);

	return 0;
}

//==============================================================================
//
// 	Function Name:	CMainView::OnCtlColor()
//
// 	Description:	This function is overloaded to set the background of the
//					dialog to black.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
HBRUSH CMainView::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor) 
{
	//	Is this a request for the dialog brush?
	if(nCtlColor == CTLCOLOR_DLG)
		return (HBRUSH)m_brBackground;
	else
		return CFormView::OnCtlColor(pDC, pWnd, nCtlColor);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnDarkBlue()
//
// 	Description:	This function will set the annotation color to dark blue.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnDarkBlue() 
{
	if(!IsCommandEnabled(TMAX_DARKBLUE))
		return;
	
	for(int i=0; i < SZ_ARR_TM_VW; i++)
		m_arrTmView[i]->SetColor(TMV_DARKBLUE);
	UpdateToolColor();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnDarkGreen()
//
// 	Description:	This function will set the annotation color to dark green.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnDarkGreen() 
{
	if(!IsCommandEnabled(TMAX_DARKGREEN))
		return;
	
	for(int i=0; i < SZ_ARR_TM_VW; i++)
		m_arrTmView[i]->SetColor(TMV_DARKGREEN);
	UpdateToolColor();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnDarkRed()
//
// 	Description:	This function will set the annotation color to dark red.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnDarkRed() 
{
	if(!IsCommandEnabled(TMAX_DARKRED))
		return;
	
	for(int i=0; i < SZ_ARR_TM_VW; i++)
		m_arrTmView[i]->SetColor(TMV_DARKRED);
	UpdateToolColor();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnDeleteAnn()
//
// 	Description:	This function will delete the selected annotations or the
//					last annotation if none have been selected.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnDeleteAnn() 
{
	if(!IsCommandEnabled(TMAX_DELETEANN))
		return;
	
	//	Delete the selections in the active pane
	if(m_ctrlTMView->GetAction() == SELECT && 
       m_ctrlTMView->GetSelectCount(TMV_ACTIVEPANE) > 0)
		m_ctrlTMView->DeleteSelections(TMV_ACTIVEPANE);
	else
		m_ctrlTMView->DeleteLastAnn(TMV_ACTIVEPANE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnDestroy()
//
// 	Description:	This function traps the WM_DESTROY message
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnDestroy() 
{
	//	Make sure the task bar is restored. 
	SetTaskBarVisible(TRUE);

	//	Shut down the application sharing control
	if(IsWindow(m_ctrlManagerApp.m_hWnd))
		m_ctrlManagerApp.Terminate();

	//	Make sure the capture is shut down
	if(IsWindow(m_ctrlTMGrab.m_hWnd))
		m_ctrlTMGrab.Stop();

	//	Make sure the DAO stuff is shut down OK
	//AfxDaoTerm();

	//	Do the base class cleanup
	CFormView::OnDestroy();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnDisableLinks()
//
// 	Description:	This function will toggle the disable links option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnDisableLinks() 
{
	if(!IsCommandEnabled(TMAX_DISABLELINKS))
		return;
	
	//	Toggle the links 
	m_bDisableLinks = !m_bDisableLinks;
	
	//	Are we disabling the links?
	if(m_bDisableLinks == TRUE)
	{
		//	Are we in a linked state?
		if((m_sState == S_LINKEDIMAGE) || (m_sState == S_LINKEDPOWER))
		{
				if(m_Playlist.pPlaylist != NULL)
				{
					if(GetScrollTextEnabled(m_Playlist.pDesignation) == TRUE)
						SetDisplay(S_TEXT);
					else
						SetDisplay(S_PLAYLIST);
				}
				else
				{
					SetDisplay(S_MOVIE);
				}
		
		}// if((m_sState == S_LINKEDIMAGE) || (m_sState == S_LINKEDPOWER))

	}
	else
	{
		//	Do we have an active link?
		if(m_AppLink.GetHideLink() == FALSE)
		{
			//	Are we playing video?
			switch(m_sState)
			{
				case S_LINKEDIMAGE:
				case S_LINKEDPOWER:
				case S_TEXT:
				
					if(lstrlen(m_AppLink.GetDatabaseId()) > 0)
						OnAxLinkChange(m_AppLink.GetDatabaseId(), 0, m_AppLink.GetAttributes());
					else
						OnAxLinkChange(m_AppLink.GetBarcode(), 0, m_AppLink.GetAttributes());
					break;
			}

		}// if(m_AppLink.GetHideLink() == FALSE)
	
	}// if(m_bDisableLinks == TRUE)
	
	//	Update the toolbar button 
	if(m_pToolbar)
		m_pToolbar->SetLinkButton(m_bDisableLinks);	
}

//==============================================================================
//
// 	Function Name:	CMainView::OnDraw()
//
// 	Description:	This is an overloaded version of the base class member. It
//					will redraw the view unless redrawing has been inhibited
//					by the frame window.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnDraw(CDC* pDC) 
{
	if(m_bRedraw)
		CFormView::OnDraw(pDC);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnDrawTool()
//
// 	Description:	This function enable the drawing tool.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnDrawTool() 
{
	if(!IsCommandEnabled(TMAX_DRAWTOOL))
		return;
	
	// Update the color button color
	m_Ini.SetTMSection(PRESENTATION_APP);
	short drawToolColor =(short)m_Ini.ReadLong(ANNCOLOR_LINE);
	ChangeColorOfColorButton(drawToolColor);

	for(int i = 0; i < SZ_ARR_TM_VW; i++)
		m_arrTmView[i]->SetAction(DRAW);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnEllipse()
//
// 	Description:	This function is called to make the ellipse tool the 
//					current drawing tool
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnEllipse() 
{
	if(!IsCommandEnabled(TMAX_ELLIPSE))
		return;
	
	SetDrawingTool(ELLIPSE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnEndMovie()
//
// 	Description:	This function will fast forward to the end of the current
//					movie
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnEndMovie() 
{
	if(!IsCommandEnabled(TMAX_ENDMOVIE))
		return;
	
	//	Cue to the end
	CueMovie(TMMCUE_LAST, TRUE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnErase()
//
// 	Description:	This function will erase all annotations from the current
//					page.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnErase() 
{
	if(!IsCommandEnabled(TMAX_ERASE))
		return;
	
	m_ctrlTMView->Erase(TMV_ACTIVEPANE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnExit()
//
// 	Description:	This function handles TMAX_EXIT commands.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnExit() 
{
	if(m_bIsBinderOpen == TRUE)
	{
		m_bIsBinderOpen = FALSE;
		m_BinderList->OnCancel();
		return;
	}

	if(m_bIsColorPickerOpen == TRUE)
	{
		m_bIsColorPickerOpen = FALSE;
		m_ColorPickerList->OnCancel();
		return;
	}

	if(!IsCommandEnabled(TMAX_EXIT))
		return;
	
	m_pFrame->SendMessage(WM_COMMAND, ID_APP_EXIT, 0);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnFilePrint()
//
// 	Description:	This function will print the current image to the default
//					printer.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnFilePrint() 
{
	if(!IsCommandEnabled(TMAX_PRINT))
		return;
	
	theApp.DoWaitCursor(1);
	m_ctrlTMView->Print(FALSE, -1);
	theApp.DoWaitCursor(0);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnFilledEllipse()
//
// 	Description:	This function is called to make the filled ellipse tool the 
//					current drawing tool
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnFilledEllipse() 
{
	if(!IsCommandEnabled(TMAX_FILLEDELLIPSE))
		return;
	
	SetDrawingTool(FILLED_ELLIPSE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnFilledRectangle()
//
// 	Description:	This function is called to make the filled rectangle tool 
//					the current drawing tool
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnFilledRectangle() 
{
	if(!IsCommandEnabled(TMAX_FILLEDRECTANGLE))
		return;
	
	SetDrawingTool(FILLED_RECTANGLE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnFilterProps()
//
// 	Description:	This function is called to open the filter properties
//					dialog box for the current playback
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnFilterProps() 
{
	if(!IsCommandEnabled(TMAX_FILTERPROPS))
		return;

	//	Make sure the playback is paused
	m_ctrlTMMovie.Pause();

	//	Show the filter information
	m_ctrlTMMovie.ShowFilterInfo();	
}

//==============================================================================
//
// 	Function Name:	CMainView::OnFirstDesignation()
//
// 	Description:	This function will rewind the playlist to the first 
//					designation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnFirstDesignation() 
{
	if(!IsCommandEnabled(TMAX_FIRSTDESIGNATION))
		return;

	CuePlaylist(TMMCUEPL_FIRST, TRUE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnFirstPage()
//
// 	Description:	This function will load the first page in the file.
//
// 	Returns:		None
//
//	Notes:			There are two levels here. We could be dealing with a 
//					multipage file or we could be dealing with multiple pages
//					within the current database.
//
//==============================================================================
void CMainView::OnFirstPage() 
{
	SMultipageInfo  Info;
	short			sState;

	if(!IsCommandEnabled(TMAX_FIRSTPAGE))
		return;

	//	Use the previous screen state if the current screen is cleared
	if(m_sState == S_CLEAR)
		sState = m_sPrevState;
	else
		sState = m_sState;

	//	If the TMView control is the active viewer we have to first check to
	//	see if it's displaying a multipage image
	switch(sState)
	{
		case S_DOCUMENT:
		case S_GRAPHIC:		
		case S_LINKEDIMAGE:

			if(m_ctrlTMView == m_arrTmView[1]) {
				countFrom = COUNT_FROM_FIRST;
				curPageNavCount=0;
				loadNextInOtherPanes = false;
				scaleHist.clear();
				zoomFullWidth = false;
			}

			if(m_ctrlTMView->GetCurrentPage(TMV_ACTIVEPANE) > 1)
			{
				m_ctrlTMView->FirstPage(TMV_ACTIVEPANE);

				if(m_sState == S_CLEAR)
					RestoreDisplay();
				return;
			}
			else
			{
				//	Load the first page
				if(SetPageFromId(&Info, 0, SETPAGE_FIRST))
				{
					DbgMsg(&Info, "OnFirstPage");
					LoadMultipage(&Info);
				}
			}
			break;
		
		case S_POWERPOINT:
		case S_LINKEDPOWER:

			m_ctrlTMPower.First(-1);

			//	Update the status bar information
			m_Barcode.m_lSecondaryId = m_ctrlTMPower.GetCurrentSlide(-1);
			m_Barcode.m_lTertiaryId = 0;
			UpdateStatusBar();

			if(m_sState == S_CLEAR)
				RestoreDisplay();

			break;

		case S_MOVIE:

			if(SetPageFromId(&Info, 0, SETPAGE_FIRST))
				LoadMultipage(&Info);
			break;
		
		case S_PLAYLIST:
		case S_TEXT:
		case S_CLEAR:
		default:				

			break;
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::OnFirstShowItem()
//
// 	Description:	This function will load the first custom show item in the 
//					list.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnFirstShowItem() 
{
	SShowInfo Show;

	if(!IsCommandEnabled(TMAX_FIRSTSHOWITEM))
		return;

	//	If this command is enabled we must have an active show
	ASSERT(m_Show.pShow);
	if(m_Show.pShow == 0)
		return;

	//	Kill the automatic transition
	StopAutoTransition();

	//	Initialize the new show information
	Show.pShow = m_Show.pShow;
	if((Show.pItem = Show.pShow->m_Items.First()) == 0)
		return;

	//	Load this item
	if(LoadShowItem(&Show))
		m_Show.pItem = Show.pItem;
}

//==============================================================================
//
// 	Function Name:	CMainView::OnFirstZap()
//
// 	Description:	This function will load the first zap associated with the
//					current image.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnFirstZap() 
{
	SMultipageInfo* pInfo = GetMultipageInfo(m_sState);

	if(!IsCommandEnabled(TMAX_FIRSTZAP))
		return;
	
	//	Do we have a valid page object?
	if((pInfo == 0) || (pInfo->pSecondary == 0))
		return;

	DbgMsg(pInfo, "OnFirstZap");

	//	Get the treatment object
	if((pInfo->pTertiary = pInfo->pSecondary->m_Children.First()) != 0)
		LoadTreatment(pInfo, m_sState);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnFreehand()
//
// 	Description:	This function is called to make the freehand tool the 
//					current drawing tool
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnFreehand() 
{
	if(!IsCommandEnabled(TMAX_FREEHAND))
		return;
	
	SetDrawingTool(FREEHAND);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnFullScreen()
//
// 	Description:	This function is called to hide the current link and 
//					return to full screen video
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnFullScreen() 
{	
	if(!IsCommandEnabled(TMAX_FULLSCREEN))
		return;
	//	Make it look like TMMovie fired a Hide Link event
	OnAxLinkChange(0,0,TMFLAG_LINK_HIDE);	
}

//==============================================================================
//
// 	Function Name:	CMainView::OnFwdDesignation()
//
// 	Description:	This function will fast forward the current designation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnFwdDesignation() 
{
	if(!IsCommandEnabled(TMAX_FWDDESIGNATION))
		return;
	
	CuePlaylist(TMMCUEPL_STEP, TRUE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnFwdMovie()
//
// 	Description:	This function will fast forward the current movie
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnFwdMovie() 
{
	if(!IsCommandEnabled(TMAX_FWDMOVIE))
		return;
	
	CueMovie(TMMCUE_RELATIVE, TRUE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnGreen()
//
// 	Description:	This function will set the annotation color to green.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnGreen() 
{
	if(!IsCommandEnabled(TMAX_GREEN))
		return;
	
	for(int i=0; i < SZ_ARR_TM_VW; i++)
		m_arrTmView[i]->SetColor(TMV_GREEN);
	UpdateToolColor();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnHighlight()
//
// 	Description:	This function will enable the highlighting tool.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnHighlight() 
{
	if(!IsCommandEnabled(TMAX_HIGHLIGHT))
		return;
	
	// Update the color button color
	m_Ini.SetTMSection(PRESENTATION_APP);
	short highliterColor = (short)m_Ini.ReadLong(HIGHLIGHTCOLOR_LINE);
	ChangeColorOfColorButton(highliterColor);

	for(int i = 0; i < SZ_ARR_TM_VW; i++)
		m_arrTmView[i]->SetAction(HIGHLIGHT);	
}

//==============================================================================
//
// 	Function Name:	CMainView::OnIdleUpdateCmdUI()
//
// 	Description:	This function will update the toolbar if idle time 
//					is available.
//
// 	Returns:		0 
//
//	Notes:			None
//
//==============================================================================
LRESULT CMainView::OnIdleUpdateCmdUI(WPARAM wParam, LPARAM) 
{
	//	Update the buttons on the drawing tool toolbar if it's visible
	if(IsWindow(m_ctrlTBTools.m_hWnd) && m_ctrlTBTools.IsWindowVisible())
	{
		m_ctrlTBTools.EnableButton(TMTB_FREEHAND, IsCommandEnabled(TMAX_FREEHAND));
		m_ctrlTBTools.CheckButton(TMTB_FREEHAND, IsCommandChecked(TMAX_FREEHAND));

		m_ctrlTBTools.EnableButton(TMTB_LINE, IsCommandEnabled(TMAX_LINE));
		m_ctrlTBTools.CheckButton(TMTB_LINE, IsCommandChecked(TMAX_LINE));

		m_ctrlTBTools.EnableButton(TMTB_ARROW, IsCommandEnabled(TMAX_ARROW));
		m_ctrlTBTools.CheckButton(TMTB_ARROW, IsCommandChecked(TMAX_ARROW));

		m_ctrlTBTools.EnableButton(TMTB_ELLIPSE, IsCommandEnabled(TMAX_ELLIPSE));
		m_ctrlTBTools.CheckButton(TMTB_ELLIPSE, IsCommandChecked(TMAX_ELLIPSE));

		m_ctrlTBTools.EnableButton(TMTB_RECTANGLE, IsCommandEnabled(TMAX_RECTANGLE));
		m_ctrlTBTools.CheckButton(TMTB_RECTANGLE, IsCommandChecked(TMAX_RECTANGLE));

		m_ctrlTBTools.EnableButton(TMTB_FILLEDELLIPSE, IsCommandEnabled(TMAX_FILLEDELLIPSE));
		m_ctrlTBTools.CheckButton(TMTB_FILLEDELLIPSE, IsCommandChecked(TMAX_FILLEDELLIPSE));

		m_ctrlTBTools.EnableButton(TMTB_FILLEDRECTANGLE, IsCommandEnabled(TMAX_FILLEDRECTANGLE));
		m_ctrlTBTools.CheckButton(TMTB_FILLEDRECTANGLE, IsCommandChecked(TMAX_FILLEDRECTANGLE));
		
		m_ctrlTBTools.EnableButton(TMTB_POLYGON, IsCommandEnabled(TMAX_POLYGON));
		m_ctrlTBTools.CheckButton(TMTB_POLYGON, IsCommandChecked(TMAX_POLYGON));
		
		m_ctrlTBTools.EnableButton(TMTB_POLYLINE, IsCommandEnabled(TMAX_POLYLINE));
		m_ctrlTBTools.CheckButton(TMTB_POLYLINE, IsCommandChecked(TMAX_POLYLINE));
		
		m_ctrlTBTools.EnableButton(TMTB_ANNTEXT, IsCommandEnabled(TMAX_ANNTEXT));
		m_ctrlTBTools.CheckButton(TMTB_ANNTEXT, IsCommandChecked(TMAX_ANNTEXT));
		
		return 0;
	}

	//	Don't bother if the toolbar isn't visible
	if(!m_pToolbar || !m_pToolbar->IsWindowVisible())
		return 0;

	if(!m_bDoUpdates)
		return 0;

	//	Set the appropriate states for each button in the toolbar
	for(int i = 0; i < TMTB_MAXBUTTONS; i++)
	{
		//	Get the button identifier
		if(m_pToolbar->IsButton(i))
		{
			//	Is the button enabled?
			m_pToolbar->EnableButton(i, IsCommandEnabled(ButtonMap[i][m_sState]));
	
			//	Is the button checked?
			m_pToolbar->CheckButton(i, IsCommandChecked(ButtonMap[i][m_sState]));
		}

	}

	return 0L;
}

//==============================================================================
//
// 	Function Name:	CMainView::OnInitialUpdate()
//
// 	Description:	This function is called the first time the view is displayed
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnInitialUpdate() 
{
	char szFolder[512];
	
	//	base class processing
	CFormView::OnInitialUpdate();

	//	Attach multipage objects to each pane of the TMView control
	
	for(int i = 0; i < SZ_ARR_TM_VW; i++) {
		SMultipageInfo *tmViewMultipageInfoData = new SMultipageInfo();
		ZeroMemory(tmViewMultipageInfoData, sizeof(SMultipageInfo));
		m_arrMultiPageInfo.push_back(tmViewMultipageInfoData);
		m_arrTmView[i]->SetData(TMV_LEFTPANE, (long)tmViewMultipageInfoData);

		tmViewMultipageInfoData = new SMultipageInfo();
		ZeroMemory(tmViewMultipageInfoData, sizeof(SMultipageInfo));
		m_arrTmView[i]->SetData(TMV_RIGHTPANE, (long)tmViewMultipageInfoData);
		m_arrMultiPageInfo.push_back(tmViewMultipageInfoData);
	}

	//	Get a pointer to the frame window
	m_pFrame = (CMainFrame*)GetParent();
	ASSERT(m_pFrame);

	//	Get the folder where the application resides
	m_strAppFolder = theApp.GetAppFolder();

	//	Initialize the help engine
	m_Help.Initialize(m_hWnd);

	//	Get the command line parameters
	m_strCaseFolder  = theApp.m_TMCmdLineInfo.m_strCaseFolder;
	m_strIniFile	 = theApp.m_TMCmdLineInfo.m_strIniFile;
	m_Test.bActive	 = theApp.m_TMCmdLineInfo.m_bRunTest;

	//	Assign the default ini file if one not specified on the command line
	if(m_strIniFile.IsEmpty())
	{
		m_strIniFile = m_strAppFolder;
		if((m_strIniFile.GetLength() > 0) && (m_strIniFile.Right(1) != "\\"))
			m_strIniFile += "\\";
		m_strIniFile += DEFAULT_TMAXINI;
	}
			
	//	Open the ini file
	m_Ini.Open(m_strIniFile);

	//	Read the setup options from the ini file
	ReadSetup(TRUE);

	//	Initialize the PowerPoint control if requested
	if(m_bEnablePowerPoint)
	{
		m_ctrlTMPower.SetEnableErrors(FALSE);
		m_ctrlTMPower.SetData(TMPOWER_LEFTVIEW, (long)&m_TMPower1);
		m_ctrlTMPower.SetData(TMPOWER_RIGHTVIEW, (long)&m_TMPower2);

		if(m_ctrlTMPower.Initialize() == TMPOWER_NOERROR)
		{
			m_ctrlTMPower.SetFocusWnd((long)m_pFrame->m_hWnd);

			//	Only allow hiding the task bar if not using dual monitors
			if((theApp.GetDualMonitors() == FALSE) || (m_bUseSecondaryMonitor == FALSE))
				m_ctrlTMPower.SetHideTaskBar(TRUE);
			else
				m_ctrlTMPower.SetHideTaskBar(FALSE);

			//	Get control back from PowerPoint
			//keybd_event(VK_MENU, 0, 0, 0);
			//keybd_event(VK_ESCAPE, 0, 0, 0);
			//keybd_event(VK_ESCAPE, 0, KEYEVENTF_KEYUP, 0);
			//keybd_event(VK_MENU, 0, KEYEVENTF_KEYUP, 0);
		}

		//	Refresh the splash box because PowerPoint may have hidden it
		if(!theApp.GetSilent())
			theApp.DoSplashBox(TRUE);
	}

	//	Get the hotkey specifications
	ReadHotkeys();

	//	Were we able to initialize the PowerPoint control?
	if(m_bEnablePowerPoint)
	{
		if(!m_ctrlTMPower.IsInitialized())
		{
			m_Errors.Handle(0, IDS_POWERPOINT_NOTINITIALIZED);
		}
		else
		{
			//	We turned the error handler off
			m_ctrlTMPower.SetEnableErrors(m_bEnableErrors);
		}
	}

	//	This helps the TMView control initialize its panes
	RECT rcClient;
	GetClientRect(&rcClient);
	m_ctrlTMView->MoveWindow(&rcClient);

	//	Initialize the display
	SetDisplay(S_CLEAR);
	RecalcLayout(m_sState);
		
	//	Initialize the toolbars
	InitializeToolbars();

	//	Retrieve the last case folder from the ini file if not specified on
	//	the command line
	if(m_strCaseFolder.IsEmpty())
	{
		m_Ini.ReadLastCase(szFolder, sizeof(szFolder));
		m_strCaseFolder = szFolder;
	}

	//	Open the database if we have a case folder
	if(!m_strCaseFolder.IsEmpty())
		OpenDatabase(m_strCaseFolder);

	//	Make sure the last case information is stored in the ini file just
	//	in case the case folder was specified on the command line
	if(( m_pDatabase != 0) && (m_pDatabase->IsOpen() == TRUE))
		m_Ini.WriteLastCase(m_strCaseFolder);

	//	Are we supposed to be entering test mode?
	if(m_Test.bActive)
	{
		InitializeTest();
	}

	//	Set a timer to complete the initialization
	SetTimer(INITIALIZE_TIMERID, 500, NULL);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnLastDesignation()
//
// 	Description:	This function will advance the playlist to the start of the
//					last designation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnLastDesignation() 
{
	if(!IsCommandEnabled(TMAX_LASTDESIGNATION))
		return;
	
	CuePlaylist(TMMCUEPL_LAST, TRUE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnLastPage()
//
// 	Description:	This function will load the last page in the file.
//
// 	Returns:		None
//
//	Notes:			There are two levels here. We could be dealing with a 
//					multipage file or we could be dealing with multiple pages
//					within the current database.
//
//==============================================================================
void CMainView::OnLastPage() 
{
	SMultipageInfo	Info;
	short			sState;

	if(!IsCommandEnabled(TMAX_LASTPAGE))
		return;

	//	Use the previous screen state if the current screen is cleared
	if(m_sState == S_CLEAR)
		sState = m_sPrevState;
	else
		sState = m_sState;

	//	If the TMView control is the active viewer we have to first check to
	//	see if it's displaying a multipage image
	switch(sState)
	{
		case S_DOCUMENT:
		case S_GRAPHIC:		
		case S_LINKEDIMAGE:

			if(m_ctrlTMView == m_arrTmView[1]) {
				countFrom = COUNT_FROM_LAST;
				curPageNavCount=0;
				loadNextInOtherPanes = false;
				scaleHist.clear();
				zoomFullWidth = false;
			}

			if(m_ctrlTMView->GetCurrentPage(TMV_ACTIVEPANE) < 
			   m_ctrlTMView->GetPageCount(TMV_ACTIVEPANE))
			{
				m_ctrlTMView->LastPage(TMV_ACTIVEPANE);

				if(m_sState == S_CLEAR)
					RestoreDisplay();
				return;
			}
			else
			{
				//	Load the last page
				if(SetPageFromId(&Info, 0, SETPAGE_LAST))
				{
					DbgMsg(&Info, "OnLastPage");
					LoadMultipage(&Info);
				}
			}
			break;
		
		case S_POWERPOINT:
		case S_LINKEDPOWER:

			m_ctrlTMPower.Last(-1);

			//	Update the status bar information
			m_Barcode.m_lSecondaryId = m_ctrlTMPower.GetCurrentSlide(-1);
			m_Barcode.m_lTertiaryId = 0;
			UpdateStatusBar();

			if(m_sState == S_CLEAR)
				RestoreDisplay();

			break;

		case S_MOVIE:

			if(SetPageFromId(&Info, 0, SETPAGE_LAST))
				LoadMultipage(&Info);
			break;
		
		case S_PLAYLIST:
		case S_TEXT:
		case S_CLEAR:
		default:				

			break;
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::OnLastShowItem()
//
// 	Description:	This function will load the last custom show item in the 
//					list.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnLastShowItem() 
{
	SShowInfo Show;

	if(!IsCommandEnabled(TMAX_LASTSHOWITEM))
		return;

	//	If this command is enabled we must have an active show
	ASSERT(m_Show.pShow);
	if(m_Show.pShow == 0)
		return;

	//	Kill the automatic transition
	StopAutoTransition();

	//	Initialize the new show information
	Show.pShow = m_Show.pShow;
	if((Show.pItem = Show.pShow->m_Items.Last()) == 0)
		return;

	//	Load this item
	if(LoadShowItem(&Show))
		m_Show.pItem = Show.pItem;
}

//==============================================================================
//
// 	Function Name:	CMainView::OnLastZap()
//
// 	Description:	This function will load the last zap associated with the
//					current image.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnLastZap() 
{
	SMultipageInfo* pInfo = GetMultipageInfo(m_sState);

	if(!IsCommandEnabled(TMAX_LASTZAP))
		return;
	
	//	Do we have a valid page object?
	if((pInfo == 0) || (pInfo->pSecondary == 0))
		return;

	DbgMsg(pInfo, "OnLastZap");

	//	Get the treatment object
	if((pInfo->pTertiary = pInfo->pSecondary->m_Children.Last()) != 0)
		LoadTreatment(pInfo, m_sState);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnLightBlue()
//
// 	Description:	This function will set the annotation color to dark blue.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnLightBlue() 
{
	if(!IsCommandEnabled(TMAX_LIGHTBLUE))
		return;
	
	for(int i=0; i < SZ_ARR_TM_VW; i++)
		m_arrTmView[i]->SetColor(TMV_LIGHTBLUE);
	UpdateToolColor();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnLightGreen()
//
// 	Description:	This function will set the annotation color to dark green.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnLightGreen() 
{
	if(!IsCommandEnabled(TMAX_LIGHTGREEN))
		return;
	
	for(int i=0; i < SZ_ARR_TM_VW; i++)
		m_arrTmView[i]->SetColor(TMV_LIGHTGREEN);
	UpdateToolColor();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnLightRed()
//
// 	Description:	This function will set the annotation color to dark red.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnLightRed() 
{
	if(!IsCommandEnabled(TMAX_LIGHTRED))
		return;
	for(int i=0; i < SZ_ARR_TM_VW; i++)
		m_arrTmView[i]->SetColor(TMV_LIGHTRED);
	UpdateToolColor();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnLine()
//
// 	Description:	This function is called to make the line tool the 
//					current drawing tool
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnLine() 
{
	if(!IsCommandEnabled(TMAX_LINE))
		return;
	
	SetDrawingTool(LINE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnManagerReqAddTreatment()
//
// 	Description:	This function handles responses from the Manager application
//					to add a treatment
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnManagerResAddTreatment() 
{
	SMultipageInfo* pInfo = 0;

	//	Are we in an appropriate screen state
	if((m_sState == S_DOCUMENT) || (m_sState == S_GRAPHIC))
	{
		//	Check each of the TMView structures just in case we're toggling between
		//	split screen
		for(vector<SMultipageInfo *>::iterator it = m_arrMultiPageInfo.begin();
			it != m_arrMultiPageInfo.end(); it++) {
			
			if(((*it)->pSecondary != 0) && 
			   ((*it)->pSecondary->m_lSecondaryId == m_ctrlManagerApp.GetSecondaryId()))
			{
				m_pDatabase->AddTreatment((*it)->pSecondary, 
										  m_ctrlManagerApp.GetTertiaryId(),
										  m_ctrlManagerApp.GetDisplayOrder(),
										  m_ctrlManagerApp.GetBarcodeId(),
										  m_ctrlManagerApp.GetSourceFileName());
			}
		}

		//	Update the barcode information
		m_Barcode.SetBarcode(m_ctrlManagerApp.GetBarcode());

	}// if((m_sState == S_DOCUMENT) || (m_sState == S_GRAPHIC))

}

//==============================================================================
//
// 	Function Name:	CMainView::OnManagerReqLoad()
//
// 	Description:	This function handles requests from the Manager application
//					to load a new media object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnManagerReqLoad() 
{
	//	Do we need to open the database?
	if((m_pDatabase == 0) || (lstrcmpi(m_strCaseFolder, m_ctrlManagerApp.GetCaseFolder()) != 0))
	{
		if(OpenDatabase(m_ctrlManagerApp.GetCaseFolder()) == FALSE)
			return;
	}
	
	//	Bring the application to the foreground
	SetToForeground();

	//	Are we supposed to load a specific media record ?
	if(m_ctrlManagerApp.GetBarcode().GetLength() > 0)
	{
		//	Retrieve the page and line numbers to be used when loading the media
		m_iLoadPage = m_ctrlManagerApp.GetPageNumber();
		m_iLoadLine = m_ctrlManagerApp.GetLineNumber();

		LoadFromBarcode(m_ctrlManagerApp.GetBarcode(), TRUE);
	}
	else
	{
		//	Clear the screen if no barcode specified
		if(m_sState != S_CLEAR)
			SetDisplay(S_CLEAR);
	}

}

//==============================================================================
//
// 	Function Name:	CMainView::OnMouseMode()
//
// 	Description:	This function will toggle the mouse mode option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnMouseMode() 
{
	if(!IsCommandEnabled(TMAX_MOUSEMODE))
		return;
	
	//	Toggle the flag 
	m_bMouseMode = !m_bMouseMode;

	if(m_bMouseMode)
	{
		theApp.EnableMouseHook(TRUE);
	}
	else
	{
		theApp.EnableMouseHook(FALSE);
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::OnNewInstance()
//
// 	Description:	This function handles notifications from the main frame
//					when a new instance is started
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnNewInstance()
{
	SCommandLine	CmdLine;
	CTMIni			Ini;
	CString			strIniFile;
	
	//	Build the specification for the ini file containing the command line
	//	information
	strIniFile = m_strAppFolder;
	if(strIniFile.Right(1) != "\\")
		strIniFile += "\\";
	strIniFile += DEFAULT_COMMANDLINE_FILE;
		
	//	Get the command line information from the ini file
	Ini.Open(strIniFile);
	Ini.ReadCommandLine(&CmdLine);

	//	Has the database changed?
	if(lstrlen(CmdLine.szCaseFolder) > 0)
	{
		if(m_strCaseFolder.CompareNoCase(CmdLine.szCaseFolder))
			OpenDatabase(CmdLine.szCaseFolder);
	}

	//	Did the caller specify a barcode?
	if((m_pDatabase != 0) && (m_pDatabase->IsOpen() == TRUE) && (lstrlen(CmdLine.szBarcode) > 0))
		LoadFromBarcode(CmdLine.szBarcode, TRUE);

	//	Bring this window to the foreground
	SetToForeground();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnNextBarcode()
//
// 	Description:	This function will load the next barcode in the buffer
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnNextBarcode() 
{
	CBarcode* pBarcode = NULL;

	//	Is this command enabled?
	if(IsCommandEnabled(TMAX_NEXT_BARCODE))
	{
		if((pBarcode = m_aBarcodes.GetNext()) != NULL)
			LoadFromBarcode(pBarcode->GetBarcode(), FALSE);
	}
	else
	{
		//	Since we have no buttons we beep
		MessageBeep(0);
	}

}

//==============================================================================
//
// 	Function Name:	CMainView::OnNextDesignation()
//
// 	Description:	This function will advance the playlist to the start of the
//					next designation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnNextDesignation() 
{
	if(!IsCommandEnabled(TMAX_NEXTDESIGNATION))
		return;
	
	CuePlaylist(TMMCUEPL_NEXT, TRUE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnNextMedia()
//
// 	Description:	This function will load the next media record in the 
//					database.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnNextMedia() 
{
	CMedia*	pMedia = 0;

	//	Is this command enabled?
	if(!IsCommandEnabled(TMAX_NEXTMEDIA))
		return;

	//	Is the database closed?
	if((m_pDatabase == 0) || (m_pDatabase->IsOpen() == FALSE))
		return;

	//	Are we currently viewing a custom show?
	if(m_Show.pShow != 0)
	{
		//	Get the next multipage object
		pMedia = m_pDatabase->GetNextMultipage(m_pMedia);
	}
	else
	{
		//	What is the current state?
		switch(m_sState)
		{
			case S_DOCUMENT:
			case S_GRAPHIC:
			case S_MOVIE:
			case S_POWERPOINT:

				//	Get the next multipage record
				pMedia = m_pDatabase->GetNextMultipage(m_pMedia);
				break;

			case S_LINKEDIMAGE:
			case S_PLAYLIST: 
			case S_TEXT:
			case S_LINKEDPOWER:

				//	Get the next playlist object
				pMedia = m_pDatabase->GetNextPlaylist(m_pMedia);
				break;

			case S_CLEAR:
			default:

				break;		
		}
	
	}

	//	Load the new media object
	if(pMedia != 0)
	{
		if(LoadMedia(pMedia, -1, -1))
		{
			UpdateStatusBar();

			//	Reset the media reference
			m_pMedia = pMedia;
		}
	}

}

//==============================================================================
//
// 	Function Name:	CMainView::OnNextPage()
//
// 	Description:	This function will load the next page in the file.
//
// 	Returns:		None
//
//	Notes:			There are two levels here. We could be dealing with a 
//					multipage file or we could be dealing with multiple pages
//					within the current database.
//
//==============================================================================
void CMainView::OnNextPage() 
{
	SMultipageInfo	Info;
	short			sState;

	if(!IsCommandEnabled(TMAX_NEXTPAGE))
		return;

	//	Use the previous screen state if the current screen is cleared
	if(m_sState == S_CLEAR)
		sState = m_sPrevState;
	else
		sState = m_sState;

	//	If the TMView control is the active viewer we have to first check to
	//	see if it's displaying a multipage image
	switch(sState)
	{
		case S_DOCUMENT:
		case S_GRAPHIC:		
		case S_LINKEDIMAGE:

			if(m_ctrlTMView == m_arrTmView[1]) {
				curPageNavCount++;
				loadNextInOtherPanes = false;
				scaleHist.clear();
				zoomFullWidth = false;
			}

			if(m_ctrlTMView->GetCurrentPage(TMV_ACTIVEPANE) < 
			   m_ctrlTMView->GetPageCount(TMV_ACTIVEPANE))
			{
				m_ctrlTMView->NextPage(TMV_ACTIVEPANE);

				if(m_sState == S_CLEAR)
					RestoreDisplay();

				return;
			}
			else
			{
				//	Load the next page
				if(SetPageFromId(&Info, 0, SETPAGE_NEXT))
				{
					DbgMsg(&Info, "OnNextPage");
					LoadMultipage(&Info);
				}
			}
			break;
		
		case S_POWERPOINT:
		case S_LINKEDPOWER:

			m_ctrlTMPower.Next(-1);

			if(m_sState == S_CLEAR)
				RestoreDisplay();

			//	Update the status bar information
			m_Barcode.m_lSecondaryId = m_ctrlTMPower.GetCurrentSlide(-1);
			m_Barcode.m_lTertiaryId = 0;
			UpdateStatusBar();

			break;

		case S_MOVIE:

			if(SetPageFromId(&Info, 0, SETPAGE_NEXT))
				LoadMultipage(&Info);
			break;
		
		case S_PLAYLIST:
		case S_TEXT:
		case S_CLEAR:
		default:				

			break;
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::OnNextPageHorizontal()
//
// 	Description:	This function will switch to horizontal split screen and
//					load the next page in the bottom pane.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnNextPageHorizontal() 
{
	SMultipageInfo*	pInfo = GetMultipageInfo(m_sState);
	CSecondary*		pSecondary = 0;
	CString			strBarcode;

	if(!IsCommandEnabled(TMAX_NEXTPAGE_HORIZONTAL) || (pInfo == 0) || (pInfo->pSecondary == 0))
		return;

	//	Get the next page
	if((pSecondary = pInfo->pMultipage->m_Pages.FindNext(pInfo->pSecondary)) == 0)
		return;

	//	Format the barcode
	if(m_iImageSecondary == SECONDARY_AS_ORDER)
		strBarcode.Format("%s.%ld", pSecondary->m_strMediaId, pSecondary->m_lPlaybackOrder);
	else
		strBarcode.Format("%s.%ld", pSecondary->m_strMediaId, pSecondary->m_lBarcodeId);
			
	//	Put the viewer in split screen mode
	OnSplitHorizontal();
	
	//	Make sure we are at the bottom of the active page
	for(int i = 0; i < 10; i++)
		m_ctrlTMView->Pan(PAN_DOWN, TMV_ACTIVEPANE);
	
	//	Shift focus to the bottom pane
	SwitchPane();

	//	Load the next page
	LoadFromBarcode(strBarcode, TRUE);

	//	Zoom to full width
	OnZoomWidth();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnNextPageVertical()
//
// 	Description:	This function will switch to Vertical split screen and
//					load the next page in the bottom pane.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnNextPageVertical() 
{
	SMultipageInfo*	pInfo = GetMultipageInfo(m_sState);
	CSecondary*		pNextPage = NULL;
	CString			strBarcode = "";

	//	Must have a valid page
	if(IsCommandEnabled(TMAX_NEXTPAGE_VERTICAL) && (pInfo != NULL) && (pInfo->pSecondary != NULL))
	{
		//	Get the next page
		if((pNextPage = pInfo->pMultipage->m_Pages.FindNext(pInfo->pSecondary)) != NULL)
		{
			//	Format the barcode
			if(m_iImageSecondary == SECONDARY_AS_ORDER)
				strBarcode.Format("%s.%ld", pNextPage->m_strMediaId, pNextPage->m_lPlaybackOrder);
			else
				strBarcode.Format("%s.%ld", pNextPage->m_strMediaId, pNextPage->m_lBarcodeId);
					
			//	Put the viewer in split screen mode
			OnSplitVertical();
			
			//	Shift focus to the bottom pane
			SwitchPane();

			//	Load the next page
			LoadFromBarcode(strBarcode, TRUE);

		}// if((pNextPage = pInfo->pMultipage->m_Pages.FindNext(pInfo->pSecondary)) != NULL)

	}// if(IsCommandEnabled(TMAX_NEXTPAGE_VERTICAL) && (pInfo != NULL) && (pInfo->pSecondary != NULL))

}

//==============================================================================
//
// 	Function Name:	CMainView::OnNextShowItem()
//
// 	Description:	This function will load the next custom show item in the 
//					list.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnNextShowItem() 
{
	SShowInfo	Show;
	CShowItem*	pCurrent = m_Show.pItem;

	if(!IsCommandEnabled(TMAX_NEXTSHOWITEM))
		return;

	//	If this command is enabled we must have an active show
	ASSERT(m_Show.pShow);
	if(m_Show.pShow == 0)
		return;
	
	//	Kill the automatic transition
	StopAutoTransition();

	//	Initialize the new show information
	Show.pShow = m_Show.pShow;
	if((Show.pItem = Show.pShow->m_Items.Next()) == 0)
		return;

	//	Load this item
	if(LoadShowItem(&Show))
		m_Show.pItem = Show.pItem;

	//	Clear the transition flag on the previously active show item
	if(pCurrent != 0)
		pCurrent->m_bTransitioned = FALSE;
}

//==============================================================================
//
// 	Function Name:	CMainView::OnNextZap()
//
// 	Description:	This function will load the next zap associated with the
//					current image.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnNextZap() 
{
	SMultipageInfo* pInfo = GetMultipageInfo(m_sState);

	if(!IsCommandEnabled(TMAX_NEXTZAP))
		return;
	
	//	Do we have a valid page object?
	if((pInfo == NULL) || (pInfo->pSecondary == NULL))
		return;

	DbgMsg(pInfo, "OnNextZap");

	//	Get the treatment object. Cycle the list if we are already on the
	//	last treatment.
	if((pInfo->pTertiary = pInfo->pSecondary->m_Children.Next()) == NULL)
	{
		pInfo->pTertiary = pInfo->pSecondary->m_Children.First();
	}

	if(pInfo->pTertiary != NULL)
		LoadTreatment(pInfo, m_sState);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnNormal()
//
// 	Description:	This function will return the image to the normal zoom
//					factor.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnNormal() 
{
	if(!IsCommandEnabled(TMAX_NORMAL))
		return;
	
	for(int i = 0; i < SZ_ARR_TM_VW; i++)
		m_arrTmView[i]->ResetZoom(TMV_ACTIVEPANE);
	scaleHist.clear();
	zoomFullWidth = false;
}

//==============================================================================
//
// 	Function Name:	CMainView::OnPan()
//
// 	Description:	This function enables the pan tool
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnPan() 
{
	if(!IsCommandEnabled(TMAX_PAN))
		return;

	for(int i = 0; i < SZ_ARR_TM_VW; i++)
		m_arrTmView[i]->SetAction(PAN);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnPlay()
//
// 	Description:	This function is called when the play button
//                  on the toolbar is clicked.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnPlay()     
{
	if(!IsCommandEnabled(TMAX_PLAY))
		return;
	
	if(m_bPlaying)
		m_ctrlTMMovie.Pause();
	else
		m_ctrlTMMovie.Resume();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnPlayThrough()
//
// 	Description:	This function will set the TMMovie control to play the
//					current playlist through to the end.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnPlayThrough() 
{
	if(!IsCommandEnabled(TMAX_PLAYTHROUGH))
		return;
	
	//	Reset the playlist range to play through to the end
	if(m_ctrlTMMovie.IsReady())
		m_ctrlTMMovie.SetPlaylistRange(0,0);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnPolygon()
//
// 	Description:	This function is called to make the polyline tool the 
//					current drawing tool
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnPolygon() 
{
	if(!IsCommandEnabled(TMAX_POLYGON))
		return;
	
	SetDrawingTool(POLYGON);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnPolyline()
//
// 	Description:	This function is called to make the polyline tool the 
//					current drawing tool
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnPolyline() 
{
	if(!IsCommandEnabled(TMAX_POLYLINE))
		return;
	
	SetDrawingTool(POLYLINE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnPreviousBarcode()
//
// 	Description:	This function will load the next barcode in the buffer
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnPreviousBarcode() 
{
	CBarcode* pBarcode = NULL;

	//	Is this command enabled?
	if(IsCommandEnabled(TMAX_PREV_BARCODE))
	{
		if((pBarcode = m_aBarcodes.GetPrevious()) != NULL)
			LoadFromBarcode(pBarcode->GetBarcode(), FALSE);
	}
	else
	{
		//	Since we have no buttons we beep
		MessageBeep(0);
	}

}

//==============================================================================
//
// 	Function Name:	CMainView::OnPreviousDesignation()
//
// 	Description:	This function will rewind the playlist to the start of the
//					previous designation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnPreviousDesignation() 
{
	if(!IsCommandEnabled(TMAX_PREVDESIGNATION))
		return;
	
	CuePlaylist(TMMCUEPL_PREVIOUS, TRUE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnPreviousPage()
//
// 	Description:	This function will load the previous page in the file.
//
// 	Returns:		None
//
//	Notes:			There are two levels here. We could be dealing with a 
//					multipage file or we could be dealing with multiple pages
//					within the current database.
//
//==============================================================================
void CMainView::OnPreviousPage() 
{
	SMultipageInfo	Info;
	short			sState;

	if(!IsCommandEnabled(TMAX_PREVPAGE))
		return;

	//	Use the previous screen state if the current screen is cleared
	if(m_sState == S_CLEAR)
		sState = m_sPrevState;
	else
		sState = m_sState;

	//	If the TMView control is the active viewer we have to first check to
	//	see if it's displaying a multipage image
	switch(sState)
	{
		case S_DOCUMENT:
		case S_GRAPHIC:		
		case S_LINKEDIMAGE:

			if(m_ctrlTMView == m_arrTmView[1]) {
				curPageNavCount--;
				loadNextInOtherPanes = false;
				scaleHist.clear();
				zoomFullWidth = false;
			}

			if(m_ctrlTMView->GetCurrentPage(TMV_ACTIVEPANE) > 1)
			{
				m_ctrlTMView->PrevPage(TMV_ACTIVEPANE);

				if(m_sState == S_CLEAR)
					RestoreDisplay();

				return;
			}
			else
			{
				//	Load the previous page
				if(SetPageFromId(&Info, 0, SETPAGE_PREVIOUS))
				{
					DbgMsg(&Info, "OnPreviousPage");
					LoadMultipage(&Info);
				}
			}
			break;
		
		case S_POWERPOINT:
		case S_LINKEDPOWER:

			m_ctrlTMPower.Previous(-1);

			if(m_sState == S_CLEAR)
				RestoreDisplay();

			//	Update the status bar information
			m_Barcode.m_lSecondaryId = m_ctrlTMPower.GetCurrentSlide(-1);
			m_Barcode.m_lTertiaryId = 0;
			UpdateStatusBar();

			break;

		case S_MOVIE:

			if(SetPageFromId(&Info, 0, SETPAGE_PREVIOUS))
				LoadMultipage(&Info);
			break;
		
		case S_PLAYLIST:
		case S_TEXT:
		case S_CLEAR:
		default:				

			break;
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::OnPreviousShowItem()
//
// 	Description:	This function will load the previous custom show item in the 
//					list.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnPreviousShowItem() 
{
	SShowInfo Show;

	if(!IsCommandEnabled(TMAX_PREVSHOWITEM))
		return;

	//	If this command is enabled we must have an active show
	ASSERT(m_Show.pShow);
	if(m_Show.pShow == 0)
		return;

	//	Kill the automatic transition
	StopAutoTransition();

	//	Initialize the new show information
	Show.pShow = m_Show.pShow;
	if((Show.pItem = Show.pShow->m_Items.Prev()) == 0)
		return;

	//	Load this item
	if(LoadShowItem(&Show))
		m_Show.pItem = Show.pItem;
}

//==============================================================================
//
// 	Function Name:	CMainView::OnPreviousMedia()
//
// 	Description:	This function will load the previous media record in the 
//					database.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnPreviousMedia() 
{
	CMedia* pMedia = 0;

	//	Is this command enabled?
	if(!IsCommandEnabled(TMAX_PREVMEDIA))
		return;

	//	Is the database closed?
	if((m_pDatabase == 0) || (m_pDatabase->IsOpen() == FALSE))
		return;

	//	Are we currently viewing a custom show?
	if(m_Show.pShow != 0)
	{
		//	Get the previous multipage object
		pMedia = m_pDatabase->GetPrevMultipage(m_pMedia);
	}
	else
	{
		//	What is the current state?
		switch(m_sState)
		{
			case S_DOCUMENT:
			case S_GRAPHIC:
			case S_MOVIE:
			case S_POWERPOINT:

				//	Get the previous multipage record
				pMedia = m_pDatabase->GetPrevMultipage(m_pMedia);
				break;

			case S_LINKEDIMAGE:
			case S_PLAYLIST: 
			case S_TEXT:
			case S_LINKEDPOWER:

				//	Get the previous playlist object
				pMedia = m_pDatabase->GetPrevPlaylist(m_pMedia);
				break;

			case S_CLEAR:
			default:

				break;		
		}
	
	}

	//	Load the new media object
	if(pMedia != 0)
	{
		if(LoadMedia(pMedia, -1, -1))
		{
			UpdateStatusBar();

			//	Reset the media reference
			m_pMedia = pMedia;
		}
	}

}

//==============================================================================
//
// 	Function Name:	CMainView::OnPreviousZap()
//
// 	Description:	This function will load the previous zap associated with the
//					current image.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnPreviousZap() 
{
	SMultipageInfo* pInfo = GetMultipageInfo(m_sState);

	if(!IsCommandEnabled(TMAX_PREVZAP))
		return;
	
	//	Do we have a valid page object?
	if((pInfo == 0) || (pInfo->pSecondary == 0))
		return;

	DbgMsg(pInfo, "OnPreviousZap");

	//	Get the treatment object. Cycle the list if we are already on the
	//	last treatment.
	if((pInfo->pTertiary = pInfo->pSecondary->m_Children.Prev()) == 0)
		pInfo->pTertiary = pInfo->pSecondary->m_Children.Last();

	if(pInfo->pTertiary)
		LoadTreatment(pInfo, m_sState);
}


//==============================================================================
//
// 	Function Name:	CMainView::OnRectangle()
//
// 	Description:	This function is called to make the rectangle tool the 
//					current drawing tool
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnRectangle() 
{
	if(!IsCommandEnabled(TMAX_RECTANGLE))
		return;
	
	SetDrawingTool(RECTANGLE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnRed()
//
// 	Description:	This function will set the annotation color to red.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnRed() 
{
	m_bShowExtraToolBar = true;
	if(!IsCommandEnabled(TMAX_RED))
		return;
	
	for(int i=0; i < SZ_ARR_TM_VW; i++)
		m_arrTmView[i]->SetColor(TMV_RED);
	UpdateToolColor();
	
	

//	 Checking to show or hide extra buttons large toolbar
	//if (m_ControlBar.pWnd == m_aToolbars[S_DOCUMENT].pControl && m_aToolbars[S_DOCUMENT].sSize == TMTB_LARGEBUTTONS )
	//{	
	//	if (m_ControlBarExtra.iId==CONTROL_BAR_SHOW_LARGE)
	//		SetControlBar(CONTROL_BAR_HIDE_LARGE);
	//	else 
	//		SetControlBar(CONTROL_BAR_SHOW_LARGE);

	//}

	//else 
	//{
	//	if(!IsCommandEnabled(TMAX_RED))
	//		return;
	//
	//	m_ctrlTMView->SetColor(TMV_RED);
	//	UpdateToolColor();
	//}

}
//==============================================================================
//
// 	Function Name:	CMainView::OnRedact()
//
// 	Description:	This function will enable the redaction tool.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnRedact() 
{
	if(!IsCommandEnabled(TMAX_REDACT))
		return;
	
	// Update the color button color
	m_Ini.SetTMSection(PRESENTATION_APP);
	short redactColor = (short)m_Ini.ReadLong(REDACTCOLOR_LINE);
	ChangeColorOfColorButton(redactColor);

	for(int i = 0; i < SZ_ARR_TM_VW; i++)
		m_arrTmView[i]->SetAction(REDACT);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnRotateCcw()
//
// 	Description:	This function will rotate the image counter-clockwise.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnRotateCcw() 
{
	if(!IsCommandEnabled(TMAX_ROTATECCW))
		return;
	
	m_ctrlTMView->RotateCcw(TRUE, TMV_ACTIVEPANE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnRotateCw()
//
// 	Description:	This function will rotate the image clockwise.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnRotateCw() 
{
	if(!IsCommandEnabled(TMAX_ROTATECW))
		return;
	
	m_ctrlTMView->RotateCw(TRUE, TMV_ACTIVEPANE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnSaveSplitZap()
//
// 	Description:	This function will save the current split screen state to
//					a split screen zap file
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnSaveSplitZap() 
{
	if(IsCommandEnabled(TMAX_SAVE_SPLIT_ZAP))
		SaveSplitZap();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnSaveZap()
//
// 	Description:	This function will save a dynamic annotations file for the
//					current image.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnSaveZap() 
{
	if(IsCommandEnabled(TMAX_SAVEZAP))
		SaveZap();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnScreenCapture()
//
// 	Description:	This function will toggle the screen capture capabilities
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnScreenCapture() 
{
	if(!IsCommandEnabled(TMAX_SCREENCAPTURE))
		return;

	//	Start the screen capture
	m_ctrlTMGrab.Capture();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnSelect()
//
// 	Description:	This function will enable the select tool.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnSelect() 
{
	if(!IsCommandEnabled(TMAX_SELECT))
		return;
	
	for(int i = 0; i < SZ_ARR_TM_VW; i++)
		m_arrTmView[i]->SetAction(SELECT);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnSelectTool()
//
// 	Description:	This function will pop up the toolbar that allows the user
//					to select the drawing tool
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnSelectTool() 
{
	//	Is this command enabled?
	if(!IsCommandEnabled(TMAX_SELECTTOOL))
		return;
	
	//	Initialize the flag used to indicate if the toolbar should be made
	//	visible after selecting the drawing tool
	m_bRestoreToolbar = FALSE;

	//	Do we have a valid toolbar?
	if(m_pToolbar)
	{
		//	Match the existing toolbar's properties
		m_ctrlTBTools.SetButtonSize(m_pToolbar->GetButtonSize());
		m_ctrlTBTools.SetStretch(m_pToolbar->GetStretch());
		m_ctrlTBTools.SetStyle(m_pToolbar->GetStyle());
		
		//	Should we restore the toolbar once the tool has been selected
		if(m_ControlBar.iId == CONTROL_BAR_TOOLS)
			m_bRestoreToolbar = TRUE;
	}
	else
	{
		//	Use the graphics toolbar properties
		m_ctrlTBTools.SetButtonSize(m_ctrlTBGraphics.GetButtonSize());
		m_ctrlTBTools.SetStretch(m_ctrlTBGraphics.GetStretch());
		m_ctrlTBTools.SetStyle(m_ctrlTBGraphics.GetStyle());
	}

	//	Hide the current control bar
	if(m_ControlBar.iId != CONTROL_BAR_NONE)
		SetControlBar(CONTROL_BAR_NONE);

	//	Pop the drawing tools toolbar up over the existing toolbar
	m_ctrlTBTools.SetButtonMap(aToolsMap);
	m_ctrlTBTools.ShowWindow(SW_SHOW);
	m_ctrlTBTools.BringWindowToTop();
	
	//	Make sure the light pen control remains visible
	if(m_bPenControls)
		m_ctrlTMLpen.BringWindowToTop();	
}

//==============================================================================
//
// 	Function Name:	CMainView::OnSetLine()
//
// 	Description:	This function is called when the user wants to set the
//					current playlist to a specific line.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnSetLine() 
{
	if(!IsCommandEnabled(TMAX_SETPAGELINE))
		return;

	//	Do we have an playlist currently available
	if((m_Playlist.pPlaylist != NULL) && (m_Playlist.pPlaylist->GetIsRecording() == FALSE))
	{
		SetLine();
	}
	else if(!m_strLastPlaylist.IsEmpty())
	{
		SetLine();
	}
	else
	{
		m_Errors.Handle(0, IDS_SETPGLN_NOPLAYLIST);
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::OnSetNextLine()
//
// 	Description:	This function is called when the user wants to set the 
//					next playlist to start on a specific line.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnSetNextLine() 
{
	if(!IsCommandEnabled(TMAX_SETPAGELINENEXT))
		return;

	//	Open the dialog
	SetNextLine();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnShadeOnCallout()
//
// 	Description:	This function will toggle the shade on callout option
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnShadeOnCallout() 
{
	if(!IsCommandEnabled(TMAX_SHADEONCALLOUT))
		return;
	
	for(int i = 0; i < SZ_ARR_TM_VW; i++)
		m_arrTmView[i]->SetShadeOnCallout(!m_ctrlTMView->GetShadeOnCallout());
	
	//	Update the ini file
	m_Ini.SetTMSection(PRESENTATION_APP);
	m_Ini.WriteBool(SHADEONCALLOUT_LINE, m_ctrlTMView->GetShadeOnCallout());
}

//==============================================================================
//
// 	Function Name:	CMainView::OnShowToolbar()
//
// 	Description:	This function will toggle the visibility of the toolbar
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnShowToolbar()
{
	
	//	Is this command enabled?
	if(!IsCommandEnabled(TMAX_SHOWTOOLBAR) || (m_pToolbar == 0)) return;

	//	Is the user canceling the drawing tools toolbar?
	if(m_ctrlTBTools.IsWindowVisible())
	{
		//	This executes the code that normally turns the drawing toolbar off
		SetDrawingTool(m_ctrlTMView->GetAnnTool());
	}
	else
	{
		//	Toggle the visibility of the toolbar
		if(m_pToolbar->IsWindowVisible())
			SetControlBar(CONTROL_BAR_NONE);
		else
			SetControlBar(CONTROL_BAR_TOOLS);
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::OnShowToolbarLarge()
//
// 	Description:	This function will toggle the visibility of the toolbar
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnShowToolbarLarge()
{

	SetControlBar(CONTROL_BAR_TOOLS);

	if(m_ctrlTBTools.IsWindowVisible())
	{
		//	This executes  the code that normally turns the drawing toolbar off
		SetDrawingTool(m_ctrlTMView->GetAnnTool());
	}
	else
	{
		SetControlBar(CONTROL_BAR_TOOLS);
	}

}


//==============================================================================
//
// 	Function Name:	CMainView::OnSize()
//
// 	Description:	This function will resize the TMView control to consume the
//					full client area whenever the view size changes.
//
// 	Returns:		TRUE if successful.
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnSize(UINT nType, int cx, int cy) 
{
	CRect rcClient;

	//	Save the new extents
	GetClientRect(rcClient);
	m_iMaxWidth      = rcClient.Width();
	m_iMaxHeight     = rcClient.Height();
	m_iHalfWidth     = m_iMaxWidth / 2;
	m_iHalfHeight    = m_iMaxHeight / 2; 

	//	Set the maximum width of the text control
	if(IsWindow(m_ctrlTMText.m_hWnd))
		m_ctrlTMText.SetMaxWidth(m_iMaxWidth, FALSE);

	//	Resize the toolbars
	for(int i = 0; i < MAX_STATES; i++)
	{
		if(!m_aToolbars[i].pControl || !IsWindow(m_aToolbars[i].pControl->m_hWnd))
			continue;
		(m_aToolbars[i].pControl)->ResetFrame();
	}
	if(IsWindow(m_ctrlTBTools.m_hWnd))
		m_ctrlTBTools.ResetFrame();

	//	Have the control windows been created?
	if(IsWindow(m_ctrlTMView->m_hWnd) && 
	   IsWindow(m_ctrlTMMovie) &&
	   IsWindow(m_ctrlTMText))
	{
		//	Recalculate the control rectangles
		RecalcLayout(m_sState);	
	}
	else
	{
		CFormView::OnSize(nType, cx, cy);
	}
	
}

//==============================================================================
//
// 	Function Name:	CMainView::OnSplitHorizontal()
//
// 	Description:	This function will toggle the split horizontal option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================

void CMainView::OnSplitHorizontal() 
{
	if(!IsCommandEnabled(TMAX_SPLITHORIZONTAL))
		return;
	
	//	Are we currently in split horizontal mode?
	if(m_ctrlTMView->GetSplitScreen() && m_ctrlTMView->GetSplitHorizontal())
	{
		//	Turn off split screening
		m_ctrlTMView->SetSplitScreen(FALSE);
		
		//	Clear out the inactive pane
		ClearTMViewInactive();
	}
	else
	{
		//	Split the screen horizontally
		m_ctrlTMView->SetSplitHorizontal(TRUE);
		
		if(!m_ctrlTMView->GetSplitScreen())
			m_ctrlTMView->SetSplitScreen(TRUE);
	}
	
	//	Split screen state has been set by the user
	SetZapSplitScreen(FALSE);

	//	Set the correct bitmap on the toolbar button
	if(m_pToolbar)
		m_pToolbar->SetSplitButton(m_ctrlTMView->GetSplitScreen(),
								   m_ctrlTMView->GetSplitHorizontal());	
}

//==============================================================================
//
// 	Function Name:	CMainView::OnSplitPagesNext()
//
// 	Description:	This function will go to the next page in the document
//					in split screen mode with adjacent pages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnSplitPagesNext() 
{
	SMultipageInfo	pageInfo;
	CSecondary*		pNextPage = NULL;
	CString			strBarcode = "";

	//	Must have a valid page
	if(IsCommandEnabled(TMAX_SPLITPAGES_NEXT) == TRUE)
	{
		memset(&pageInfo, 0, sizeof(pageInfo));

		//	Get the information used to identify the next page
		if(GetSplitPageInfo(&pageInfo, FALSE) == TRUE)
		{
			//	If the right pane is loaded we have to swap panes
			if(m_ctrlTMView->IsLoaded(TMV_RIGHTPANE) == TRUE)
				m_ctrlTMView->SwapPanes();

			//	Make sure the right pane is the active pane
			if(m_ctrlTMView->GetActivePane() != TMV_RIGHTPANE)
				m_ctrlTMView->SetActivePane(TMV_RIGHTPANE);
			
			//	Format the barcode
			if(m_iImageSecondary == SECONDARY_AS_ORDER)
				strBarcode.Format("%s.%ld", pageInfo.pSecondary->m_strMediaId, pageInfo.pSecondary->m_lPlaybackOrder);
			else
				strBarcode.Format("%s.%ld", pageInfo.pSecondary->m_strMediaId, pageInfo.pSecondary->m_lBarcodeId);
					
			//	Load the next page
			LoadFromBarcode(strBarcode, TRUE);

			//	Go wide screen if horizontal
			if(m_ctrlTMView->GetSplitHorizontal() == TRUE)
				OnZoomWidth();

		}// if(GetSplitPageInfo(&pageInfo, FALSE) == TRUE)

	}
	else
	{
		MessageBeep(0);
	
	}// if(IsCommandEnabled(TMAX_SPLITPAGES_NEXT) == TRUE)
}

//==============================================================================
//
// 	Function Name:	CMainView::OnSplitPagesPrevious()
//
// 	Description:	This function will go to the previous page in the document
//					in split screen mode with adjacent pages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnSplitPagesPrevious() 
{
	SMultipageInfo	pageInfo;
	CSecondary*		pNextPage = NULL;
	CString			strBarcode = "";

	//	Must have a valid page
	if(IsCommandEnabled(TMAX_SPLITPAGES_PREVIOUS) == TRUE)
	{
		memset(&pageInfo, 0, sizeof(pageInfo));

		//	Get the information used to identify the previous page
		if(GetSplitPageInfo(&pageInfo, TRUE) == TRUE)
		{
			//	If the left pane is loaded we have to swap panes
			if(m_ctrlTMView->IsLoaded(TMV_LEFTPANE) == TRUE)
				m_ctrlTMView->SwapPanes();

			//	Make sure the left pane is the active pane
			if(m_ctrlTMView->GetActivePane() != TMV_LEFTPANE)
				m_ctrlTMView->SetActivePane(TMV_LEFTPANE);
			
			//	Format the barcode
			if(m_iImageSecondary == SECONDARY_AS_ORDER)
				strBarcode.Format("%s.%ld", pageInfo.pSecondary->m_strMediaId, pageInfo.pSecondary->m_lPlaybackOrder);
			else
				strBarcode.Format("%s.%ld", pageInfo.pSecondary->m_strMediaId, pageInfo.pSecondary->m_lBarcodeId);
					
			//	Load the next page
			LoadFromBarcode(strBarcode, TRUE);

			//	Go wide screen if horizontal
			if(m_ctrlTMView->GetSplitHorizontal() == TRUE)
				OnZoomWidth();

		}// if(GetSplitPageInfo(&pageInfo, FALSE) == TRUE)

	}
	else
	{
		MessageBeep(0);
	
	}// if(IsCommandEnabled(TMAX_SPLITPAGES_PREVIOUS) == TRUE)
}

//==============================================================================
//
// 	Function Name:	CMainView::OnSplitScreen()
//
// 	Description:	This function will toggle the split vertical option.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnSplitVertical() 
{
	if(!IsCommandEnabled(TMAX_SPLITVERTICAL))
		return;
	
	//	Are we currently in split vertical mode?
	if(m_ctrlTMView->GetSplitScreen() && !m_ctrlTMView->GetSplitHorizontal())
	{
		m_ctrlTMView->SetSplitScreen(FALSE);
	
		//	Clear out the inactive pane
		ClearTMViewInactive();
	}
	else
	{
		//	Split the screen vertically
		m_ctrlTMView->SetSplitHorizontal(FALSE);
		
		if(!m_ctrlTMView->GetSplitScreen())
			m_ctrlTMView->SetSplitScreen(TRUE);
	}
	
	//	Split screen state has been set by the user
	SetZapSplitScreen(FALSE);

	//	Set the correct bitmap on the toolbar button
	if(m_pToolbar)
		m_pToolbar->SetSplitButton(m_ctrlTMView->GetSplitScreen(),
								   m_ctrlTMView->GetSplitHorizontal());	
	
}

//==============================================================================
//
// 	Function Name:	CMainView::OnStartDesignation()
//
// 	Description:	This function rewind to the start of the current designation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnStartDesignation() 
{
	if(!IsCommandEnabled(TMAX_STARTDESIGNATION))
		return;
	
	CuePlaylist(TMMCUEPL_CURRENT, TRUE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnStartMovie()
//
// 	Description:	This function rewind to the start of the current movie
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnStartMovie() 
{
	if(!IsCommandEnabled(TMAX_STARTMOVIE))
		return;
	
	CueMovie(TMMCUE_FIRST, TRUE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnStatusBar()
//
// 	Description:	This function is called to toggle the visibility of the
//					status bar
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnStatusBar() 
{
	if(!IsCommandEnabled(TMAX_STATUSBAR))
		return;

	//	Toggle the visibility
	if(m_ControlBar.iId == CONTROL_BAR_STATUS)
		SetControlBar(CONTROL_BAR_NONE);
	else
		SetControlBar(CONTROL_BAR_STATUS);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnTimer()
//
// 	Description:	This function is called whenever a WM_TIMER message is
//					recieved
//
// 	Returns:		None
//
//	Notes:			Timer events are only used in the Loop Test versions of
//					this application to interrupt the active playlist.
//
//==============================================================================
void CMainView::OnTimer(UINT nIDEvent) 
{
	CString strBarcode;

	//	Which timer?
	switch(nIDEvent)
	{
		case CUSTOM_SHOW_TIMERID:

			//	Should we advance the custom show?
			if((m_Show.pItem != 0) && (m_Show.pItem->m_ulGoToNext > 0) &&
			   (m_Show.pItem->m_bTransitioned == FALSE))
			{
				if(GetTickCount() >= m_Show.pItem->m_ulGoToNext)
				{
					//	Advance to the next item
					m_Show.pItem->m_bTransitioned = TRUE;
					OnNextShowItem();
				}

			}
			break;

		case TESTMODE_TIMERID:

			//	Has enough time elapsed yet?
			if(++m_Test.lElapsed < m_Test.lPeriod)
				return;
			else
				m_Test.lElapsed = 0;

			//	Kill the timer while we load the next object
			KillTimer(nIDEvent);

			//	Get the next barcode
			if(!GetTestBarcode(strBarcode))
				return;

			//	We may not actually have a barcode because we may have just
			//	advanced the page or treatment
			if(!strBarcode.IsEmpty())
				LoadFromBarcode(strBarcode, FALSE);

			//	Restart the test timer
			SetTimer(TESTMODE_TIMERID, 1000, NULL);
			break;

		case INITIALIZE_TIMERID:

			KillTimer(INITIALIZE_TIMERID);
		
			//	Make sure the view is brought to the foreground
			SetToForeground();
		
			//	Set up the application sharing interface
			m_ctrlManagerApp.SetAppFolder(m_strAppFolder);
			m_ctrlManagerApp.Initialize();
			m_ctrlManagerApp.SetEnableErrors(m_bEnableErrors);
	
			//	Start the custom show timer
			SetTimer(CUSTOM_SHOW_TIMERID, 250, NULL);
			break;

		case CAPTURE_BARCODE_TIMERID:

			KillTimer(CAPTURE_BARCODE_TIMERID);
			CaptureBarcode();
			break;

	}

	//	Perform the base class processing
	CFormView::OnTimer(nIDEvent);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnTMaxHelp()
//
// 	Description:	This function will display the TrialMax help
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnTMaxHelp() 
{
	if(IsCommandEnabled(TMAX_HELP) == TRUE)
	{
		if(FindFile(m_strHelpFileSpec) == TRUE)
		{
			ShellExecute(m_hWnd, "open", m_strHelpFileSpec, "", "", SW_SHOW);
		}
		else
		{
			CString strMsg;
			strMsg.Format("Unable to locate %s", m_strHelpFileSpec);
			MessageBox(strMsg, "Error", MB_ICONEXCLAMATION | MB_OK);
		}

	}// if(IsCommandEnabled(TMAX_HELP) == TRUE)

}

//==============================================================================
//
// 	Function Name:	CMainView::OnToggleScrollText()
//
// 	Description:	This function will toggle the state of the text display
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnToggleScrollText() 
{
	//	Is this command enabled?
	if(!IsCommandEnabled(TMAX_TEXT))
		return;

	//	These conditions are confirmed by the call to IsCommandEnabled()
	ASSERT(m_Playlist.pPlaylist != NULL);
	ASSERT(m_Playlist.pDesignation != NULL);

	if((m_Playlist.pPlaylist != NULL) && (m_Playlist.pDesignation != NULL))
	{
		//	Toggle the user flag
		m_bUserScrollEnabled = !m_bUserScrollEnabled;

		//	Is the user enabling scrolling text?
		if(m_bUserScrollEnabled == TRUE)
		{
			switch(m_sState)
			{
				case S_PLAYLIST:

					if(GetScrollTextEnabled(m_Playlist.pDesignation) == TRUE)
					{
						SetDisplay(S_TEXT);
					}
					break;

				case S_LINKEDIMAGE:
				case S_LINKEDPOWER:

					if(GetLinkedTextEnabled() == TRUE)
					{
						SetDisplay(m_sState);
					}
					break;

			}// switch(m_sState)

		}
		else
		{
			switch(m_sState)
			{
				case S_TEXT:

					SetDisplay(S_PLAYLIST);
					break;

				case S_LINKEDIMAGE:
				case S_LINKEDPOWER:

					if(IsScrollTextVisible() == TRUE)
					{
						SetDisplay(m_sState);
					}
					break;

			}// switch(m_sState)


		
		}// if(m_bUserScrollEnabled == TRUE)

	}// if((m_Playlist.pPlaylist != NULL) && (m_Playlist.pDesignation != NULL))

}

//==============================================================================
//
// 	Function Name:	CMainView::OnToolbars()
//
// 	Description:	This function will open a property sheet that allows the
//					user to configure the application toolbars
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnToolbars() 
{
	CToolbars Toolbars;

	if(!IsCommandEnabled(TMAX_CONFIGTOOLBARS))
		return;

	//	Disable the keyboard hook
	theApp.EnableHook(FALSE);
	theApp.EnableEscapeHook(FALSE);	

	//	Turn off the toolbar and status bar
	if(m_pToolbar)
		m_pToolbar->ShowWindow(SW_HIDE);
	m_ctrlTMStat.ShowWindow(SW_HIDE);
	SetControlBar(CONTROL_BAR_NONE);
	
	//	Clear the screen and prevent any attempts to return to the previous
	//	state
	SetDisplay(S_CLEAR);
	m_sPrevState = S_CLEAR;

	//	Reset the display controls
	ResetTMMovie(TRUE, TRUE);
	ResetTMView();
	ResetTMPower();

	//	Set the ini file
	Toolbars.SetIniFile(m_Ini.strFileSpec);

	//	Open the dialog box
	if(Toolbars.DoModal() == IDCANCEL)
	{	
		//	Grab the keyboard focus
		::SetFocus(m_hWnd);
		
		//	Enable the keyboard hook
		theApp.EnableHook(TRUE);
		theApp.EnableEscapeHook(FALSE);	
		
		//	We have to do this because clicking OK to close the dialog somehow
		//	makes the child windows visible
		if(m_pToolbar)
			m_pToolbar->ShowWindow(SW_HIDE);
		m_ctrlTMStat.ShowWindow(SW_HIDE);
		SetControlBar(CONTROL_BAR_NONE);
		SetDisplay(S_CLEAR);

		return;
	}

	//	We have to do this because clicking OK to close the dialog somehow
	//	makes the child windows visible
	if(m_pToolbar)
		m_pToolbar->ShowWindow(SW_HIDE);
	m_ctrlTMStat.ShowWindow(SW_HIDE);
	SetControlBar(CONTROL_BAR_NONE);
	SetDisplay(S_CLEAR);


	theApp.DoWaitCursor(1);

	//	Read the setup information from the ini file for each toolbar
	//
	//	NOTE:	The S_TEXT toolbar copies the S_PLAYLIST toolbar
	//			The S_LINKEDPOWER toolbar copies the S_LINKEDIMAGE toolbar
	ReadToolbar(&(m_aToolbars[S_DOCUMENT]), TMBARS_DOCUMENT_SECTION, "");
	ReadToolbar(&(m_aToolbars[S_GRAPHIC]), TMBARS_GRAPHIC_SECTION, TMBARS_DOCUMENT_SECTION);
	ReadToolbar(&(m_aToolbars[S_LINKEDIMAGE]), TMBARS_LINK_SECTION, TMBARS_DOCUMENT_SECTION);
	ReadToolbar(&(m_aToolbars[S_PLAYLIST]), TMBARS_PLAYLIST_SECTION, TMBARS_DOCUMENT_SECTION);
	ReadToolbar(&(m_aToolbars[S_TEXT]), TMBARS_PLAYLIST_SECTION, TMBARS_DOCUMENT_SECTION);
	ReadToolbar(&(m_aToolbars[S_MOVIE]), TMBARS_MOVIE_SECTION, TMBARS_DOCUMENT_SECTION);
	ReadToolbar(&(m_aToolbars[S_POWERPOINT]), TMBARS_POWERPOINT_SECTION, TMBARS_DOCUMENT_SECTION);
	ReadToolbar(&(m_aToolbars[S_LINKEDPOWER]), TMBARS_LINK_SECTION, TMBARS_DOCUMENT_SECTION);
					  
	//	Reset the toolbars
	for(int i = 0; i < MAX_STATES; i++)
	{
		if(m_aToolbars[i].pControl == 0)
			continue;

		(m_aToolbars[i].pControl)->SetButtonSize(m_aToolbars[i].sSize);
		(m_aToolbars[i].pControl)->SetStretch(m_aToolbars[i].bStretch);
		(m_aToolbars[i].pControl)->SetStyle(m_aToolbars[i].bFlat ? TMTB_FLAT : TMTB_RAISED);
		(m_aToolbars[i].pControl)->SetButtonMap(m_aToolbars[i].aMap);
	}

	//	Grab the keyboard focus
	::SetFocus(m_hWnd);
	
	//	Enable the keyboard hook
	theApp.EnableHook(TRUE);
	theApp.EnableEscapeHook(FALSE);	

	theApp.DoWaitCursor(-1);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnUpdateZap()
//
// 	Description:	Called to update the active treatment
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnUpdateZap() 
{
	SMultipageInfo*	pTLInfo = NULL;
	SMultipageInfo*	pBRInfo = NULL;
	CString			strId = "";
	BOOL			bSplitScreen = FALSE;

	if(!IsCommandEnabled(TMAX_UPDATE_ZAP))
		return;

	//	Are we currently in split screen mode?
	if(m_ctrlTMView->GetSplitScreen())
	{
		//	Is each pane loaded?
		if(m_ctrlTMView->IsLoaded(TMV_LEFTPANE) && m_ctrlTMView->IsLoaded(TMV_RIGHTPANE))
		{
			//	Get the multipage descriptor bound to each pane
			pTLInfo = (SMultipageInfo*)m_ctrlTMView->GetData(TMV_LEFTPANE);
			pBRInfo = (SMultipageInfo*)m_ctrlTMView->GetData(TMV_RIGHTPANE);

			//	Do we have valid tertiary media records for each?
			if((pTLInfo->pTertiary != NULL) && (pBRInfo->pTertiary != NULL))
			{
				if((pTLInfo->pTertiary->m_strSiblingId.GetLength() > 0) &&
				   (pBRInfo->pTertiary->m_strSiblingId.GetLength() > 0))
				{
					ASSERT(pTLInfo->pMultipage != NULL);
					ASSERT(pTLInfo->pSecondary != NULL);
					ASSERT(pBRInfo->pMultipage != NULL);
					ASSERT(pBRInfo->pSecondary != NULL);

					//	Construct the unique id for the bottom/right pane
					strId.Format("%ld.%ld.%ld", pBRInfo->pMultipage->m_lPrimaryId,
												pBRInfo->pSecondary->m_lSecondaryId,
												pBRInfo->pTertiary->m_lTertiaryId);

					//	Is this the sibling?
					if(pTLInfo->pTertiary->m_strSiblingId.CompareNoCase(strId) == 0)
					{
						bSplitScreen = TRUE;
					}
					else
					{
						pTLInfo = NULL; // Force selection of the active pane
						pBRInfo = NULL;
					}

				}

			}// if((pTLInfo->pTertiary != NULL) && (pBRInfo->pTertiary != NULL))

		}// if(m_ctrlTMView->IsLoaded(TMV_LEFTPANE) && m_ctrlTMView->IsLoaded(TMV_RIGHTPANE))
	
	}// if(m_ctrlTMView->GetSplitScreen())

	//	Get the active pane
	if(bSplitScreen == TRUE)
	{
		UpdateZap(pTLInfo, TMV_LEFTPANE);
		UpdateZap(pBRInfo, TMV_RIGHTPANE);
	}
	else
	{
		//	Get the active records
		pTLInfo = GetMultipageInfo(m_sState);

		if(pTLInfo != NULL)
			UpdateZap(pTLInfo, TMV_ACTIVEPANE);
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::OnVideoCaption()
//
// 	Description:	This function will set the visibility of the video caption
//					window
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnVideoCaption() 
{
	if(!IsCommandEnabled(TMAX_VIDEOCAPTION))
		return;

	m_ctrlTMMovie.SetOverlayVisible(!m_ctrlTMMovie.GetOverlayVisible());
}

//==============================================================================
//
// 	Function Name:	CMainView::OnWhite()
//
// 	Description:	This function will set the annotation color to white.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnWhite() 
{
	if(!IsCommandEnabled(TMAX_WHITE))
		return;
	
	for(int i=0; i < SZ_ARR_TM_VW; i++)
		m_arrTmView[i]->SetColor(TMV_WHITE);
	UpdateToolColor();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnWMGrabFocus()
//
// 	Description:	This function handles all WM_GRABFOCUS messages
//
// 	Returns:		Zero if the message is handled
//
//	Notes:			None
//
//==============================================================================
LONG CMainView::OnWMGrabFocus(WPARAM wParam, LPARAM lParam)
{
	//	Is the main view still valid?
	if(IsWindow(m_hWnd))
	{
		//	Make sure we have the focus
		::SetFocus(m_hWnd);
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CMainView::OnWMMouseMode()
//
// 	Description:	This function handles all WM_MOUSEMODE messages
//
// 	Returns:		Zero if the message is handled
//
//	Notes:			None
//
//==============================================================================

LONG CMainView::OnWMMouseMode(WPARAM wParam, LPARAM lParam)
{
	//	Are we going left or right?
	if(wParam == 0)
		ProcessVirtualKey(VK_RIGHT);
	else
		ProcessVirtualKey(VK_LEFT);
	return 0;
}

//==============================================================================
//
// 	Function Name:	CMainView::OnYellow()
//
// 	Description:	This function will set the annotation color to yellow.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnYellow() 
{
	if(!IsCommandEnabled(TMAX_YELLOW))
		return;
	
	for(int i=0; i < SZ_ARR_TM_VW; i++)
		m_arrTmView[i]->SetColor(TMV_YELLOW);
	UpdateToolColor();
}

//==============================================================================
//
// 	Function Name:	CMainView::OnZoom()
//
// 	Description:	This function enables zoom selection when the user clicks
//					on the zoom button.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnZoom() 
{
	if(!IsCommandEnabled(TMAX_ZOOM))
		return;
	
	for(int i = 0; i < SZ_ARR_TM_VW; i++) {
		m_arrTmView[i]->SetAction(ZOOM);
		m_arrTmView[i]->SetZoomToRect(FALSE);
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::OnZoomRestricted()
//
// 	Description:	This function will handle all TMAX_ZOOM_RESTRICTED commands
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnZoomRestricted()
{
	if(!IsCommandEnabled(TMAX_ZOOM))
		return;
	
	for(int i = 0; i < SZ_ARR_TM_VW; i++) {
		m_arrTmView[i]->SetAction(ZOOM);
		m_arrTmView[i]->SetZoomToRect(TRUE);
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::OnZoomWidth()
//
// 	Description:	This function will zoom the current image to the full
//					available width
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnZoomWidth() 
{
	if(!IsCommandEnabled(TMAX_ZOOMWIDTH))
		return;
	
	//	Are we already zoomed to full width?
	if(m_ctrlTMView->GetZoomState(TMV_ACTIVEPANE) == ZOOMED_FULLWIDTH)
		OnNormal();
	else {
		m_ctrlTMView->ZoomFullWidth(TMV_ACTIVEPANE);
		scaleHist.clear();
		zoomFullWidth = true;
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::OpenCaptureSource()
//
// 	Description:	Called to open the next barcodes source file for the
//					capture operation
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::OpenCaptureSource()
{
	CString	strSource = "";
	CString	strFilename = "";
	CString	strMsg = "";
	char	szTargetFolder[1024];
	char*	pExt = NULL;

	//	Have we processed all capture files?
	if((m_paCaptureFiles == NULL) || (m_iCaptureFileIndex > m_paCaptureFiles->GetUpperBound()))
	{
		if(m_paCaptureFiles != NULL)
		{
			delete m_paCaptureFiles;
			m_paCaptureFiles = NULL;
		}

		MessageBox("Finished capture operation", "Finished", MB_OK | MB_ICONEXCLAMATION);
		return TRUE;
	}
	else
	{
		strFilename = m_paCaptureFiles->GetAt(m_iCaptureFileIndex);
		m_iCaptureFileIndex++;
	}

	//	Create the path to the source file containing the barcodes
	strSource = m_strCaptureFolder;
	strSource += strFilename;

	//	Open the file
	fopen_s(&m_fptrCaptureBarcodes, strSource, "rt");
	if(m_fptrCaptureBarcodes == NULL)
	{
		strMsg.Format("Unable to open %s to load capture barcodes", strSource);
		MessageBox(strMsg);
		return FALSE;
	}

	//	Create the path to the target folder for the captured images
	sprintf_s(szTargetFolder, sizeof(szTargetFolder), "%s%s", m_strCaptureFolder, strFilename);
	if((pExt = strrchr(szTargetFolder, '.')) != NULL)
		*pExt = '\0'; // Strip the extension
	lstrcat(szTargetFolder, "\\");
	m_strCaptureTargetFolder = szTargetFolder;

	//	Make sure the folder exists
	CreateDirectory(m_strCaptureTargetFolder, 0);

	//	Start capturing barcodes
	m_iCapturedBarcodes = 0;
	LoadCaptureBarcode();

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CMainView::OpenDatabase()
//
// 	Description:	This function will open the system database
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::OpenDatabase(LPCSTR lpFolder) 
{
	CString strStatus;

	//	Close the active database
	CloseDatabase();

	//	Stop here if no folder is specified
	if(!lpFolder || lstrlen(lpFolder) == 0)
	{
		m_strCaseFolder.Empty();
		UpdateStatusBar();
		return FALSE;
	}

	//	Create a new database
	if(AllocDatabase(lpFolder) == FALSE)
		return FALSE;

	//	Turn on the database's error handler while we open
	m_pDatabase->SetErrorHandler(TRUE, m_hWnd);

	//	Set the option to create the documents
	//
	//	NOTE: This is used for testing to create the document heiarchy for a database
	m_pDatabase->SetCreateDocuments(m_bCreateDocuments);
	
	//	Save the new folder specification
	m_strCaseFolder = lpFolder;

	//	Open the database
	switch(m_pDatabase->Open(m_strCaseFolder))
	{
		case TMDB_NOERROR:	

			//	Turn off the database's error handler
			m_pDatabase->SetErrorHandler(FALSE, 0);
	
			return TRUE;

		case TMDB_FILENOTFOUND:

			//m_Errors.Handle(0, IDS_DBFILENOTFOUND, m_strCaseFolder);
			break;

		case TMDB_OPENDBFAILED:
		default:

			//m_Errors.Handle(0, IDS_OPENDBFAILED, m_strCaseFolder, m_pDatabase->GetLastErrorMsg());
			break;
	}

	//	If we made it this far the operation must have failed
	m_strCaseFolder.Empty();

	UpdateStatusBar();

	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CMainView::ParseHotKeySpec()
//
// 	Description:	This function will parse the string specification for the
//					hotkey and store the information in the hotkey array
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::ParseHotKeySpec(short sIndex, LPSTR lpSpec) 
{
	int	iLength;

	ASSERT(sIndex >= 0);
	ASSERT(sIndex < MAX_HOTKEYS);
	ASSERT(lpSpec);

	//	Clear the specification
	m_Hotkeys[sIndex].cKey   = 0;
	m_Hotkeys[sIndex].sState = TMKEY_NONE;

	if((iLength = lstrlen(lpSpec)) == 0)
		return;

	//	Get the key specification
	m_Hotkeys[sIndex].cKey = toupper(lpSpec[0]);

	//	Get the keystate specification
	for(int i = 1; i < iLength; i++)
	{
		switch(lpSpec[i])
		{
			case 'C':
			case 'c':	m_Hotkeys[sIndex].sState |= TMKEY_CONTROL;
						break;

			case 'S':
			case 's':	m_Hotkeys[sIndex].sState |= TMKEY_SHIFT;
						break;

			default:	break;
		}
	}
	return;
}

//==============================================================================
//
// 	Function Name:	CMainView::PreCreateWindow()
//
// 	Description:	This function is called before the window is created. It is
//					overloaded to set the window style.
//
// 	Returns:		TRUE if successful.
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::PreCreateWindow(CREATESTRUCT& cs)
{
	cs.style &= ~WS_BORDER;
	return CFormView::PreCreateWindow(cs);
}

//==============================================================================
//
// 	Function Name:	CMainView::ProcessCommand()
//
// 	Description:	This function is called to process the command requested
//					by the caller
//
// 	Returns:		TRUE if processed
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::ProcessCommand(short sCommand)
{
	
	if((sCommand >= TMAX_STARTMOVIE && sCommand <= TMAX_SETPAGELINENEXT )|| sCommand == TMAX_FULLSCREEN)
	{
		theApp.bSetDisplay = FALSE;
	}	
	
	// disable gesture if some tool is being selected 
	DisableGestureOnCommand(sCommand);

	switch(sCommand)
	{
		case TMAX_NOCOMMAND:			return TRUE;
		case TMAX_CLEAR:				OnClear();
										return TRUE;
		case TMAX_CALLOUT:				OnCallout();
										return TRUE;
		case TMAX_DRAWTOOL:				OnDrawTool();
										return TRUE;	
		case TMAX_ERASE:				OnErase();
										return TRUE;
		case TMAX_PRINT:				OnFilePrint();
										return TRUE;
		case TMAX_HIGHLIGHT:			OnHighlight();
										return TRUE;
		case TMAX_REDACT:				OnRedact();
										return TRUE;
		case TMAX_SELECT:				OnSelect();
										return TRUE;
		case TMAX_ROTATECCW:			OnRotateCcw();
										return TRUE;
		case TMAX_ROTATECW:				OnRotateCw();
										return TRUE;
		case TMAX_PAN:					OnPan();
										return TRUE;
		case TMAX_CONFIG:				OnConfig();
										return TRUE;
		case TMAX_FIRSTZAP:				OnFirstZap();
										return TRUE;			
		case TMAX_LASTZAP:				OnLastZap();
										return TRUE;
		case TMAX_NEXTZAP:				OnNextZap();
										return TRUE;
		case TMAX_PREVZAP:				OnPreviousZap();
										return TRUE;
		case TMAX_NEXTPAGE:				OnNextPage();
										return TRUE;
		case TMAX_PREVPAGE:				OnPreviousPage();
										return TRUE;
		case TMAX_FIRSTPAGE:			OnFirstPage();
										return TRUE;
		case TMAX_LASTPAGE:				OnLastPage();
										return TRUE;
		case TMAX_NORMAL:				OnNormal();
										return TRUE;
		case TMAX_PLAY:					OnPlay();
										return TRUE;
		case TMAX_SAVEZAP:				OnSaveZap();
										return TRUE;
		case TMAX_SAVE_SPLIT_ZAP:		OnSaveSplitZap();
										return TRUE;
		case TMAX_ZOOM:					OnZoom();
										return TRUE;
		case TMAX_ZOOMWIDTH:			OnZoomWidth();
										return TRUE;
		case TMAX_ZOOMRESTRICTED:		OnZoomRestricted();
										return TRUE;
		case TMAX_STARTMOVIE:			OnStartMovie();
										return TRUE;
		case TMAX_ENDMOVIE:				OnEndMovie();
										return TRUE;
		case TMAX_BACKMOVIE:			OnBackMovie();
										return TRUE;
		case TMAX_FWDMOVIE:				OnFwdMovie();
										return TRUE;
		case TMAX_FIRSTDESIGNATION:		OnFirstDesignation();
										return TRUE;
		case TMAX_LASTDESIGNATION:		OnLastDesignation();
										return TRUE;
		case TMAX_NEXTDESIGNATION:		OnNextDesignation();
										return TRUE;
		case TMAX_PREVDESIGNATION:		OnPreviousDesignation();
										return TRUE;
		case TMAX_BACKDESIGNATION:		OnBackDesignation();
										return TRUE;
		case TMAX_FWDDESIGNATION:		OnFwdDesignation();
										return TRUE;
		case TMAX_STARTDESIGNATION:		OnStartDesignation();
										return TRUE;
		case TMAX_SETPAGELINE:			OnSetLine();
										return TRUE;
		case TMAX_SETPAGELINENEXT:		OnSetNextLine();
										return TRUE;
		case TMAX_CONFIGTOOLBARS:		OnToolbars();
										return TRUE;
		case TMAX_DISABLELINKS:			OnDisableLinks();
										return TRUE;
		case TMAX_RED:					OnRed();
										return TRUE;
		case TMAX_GREEN:				OnGreen();
										return TRUE;
		case TMAX_BLUE:					OnBlue();
										return TRUE;
		case TMAX_YELLOW:				OnOpenColorPicker();//OnYellow();
										return TRUE;
		case TMAX_BLACK:				OnBlack();
										return TRUE;
		case TMAX_WHITE:				OnWhite();
										return TRUE;
		case TMAX_DARKRED:				OnDarkRed();
										return TRUE;
		case TMAX_DARKGREEN:			OnDarkGreen();
										return TRUE;
		case TMAX_DARKBLUE:				OnDarkBlue();
										return TRUE;
		case TMAX_LIGHTRED:				OnLightRed();
										return TRUE;
		case TMAX_LIGHTGREEN:			OnLightGreen();
										return TRUE;
		case TMAX_LIGHTBLUE:			OnLightBlue();
										return TRUE;
		case TMAX_SPLITVERTICAL:		OnSplitVertical();
										return TRUE;
		case TMAX_SPLITHORIZONTAL:		OnSplitHorizontal();
										return TRUE;
		case TMAX_SWITCHPANE:			SwitchPane();
										return TRUE;
		case TMAX_DELETEANN:			OnDeleteAnn();
										return TRUE;
		case TMAX_PLAYTHROUGH:			OnPlayThrough();
										return TRUE;
		case TMAX_VIDEOCAPTION:			OnVideoCaption();
										return TRUE;
		case TMAX_EXIT:					OnExit();
										return TRUE;
		case TMAX_TEXT:					OnToggleScrollText();
										return TRUE;
		case TMAX_SELECTTOOL:			OnSelectTool();
										return TRUE;
		case TMAX_FREEHAND:				OnFreehand();
										return TRUE;
		case TMAX_LINE:					OnLine();
										return TRUE;
		case TMAX_ARROW:				OnArrow();
										return TRUE;
		case TMAX_ELLIPSE:				OnEllipse();
										return TRUE;
		case TMAX_RECTANGLE:			OnRectangle();
										return TRUE;
		case TMAX_FILLEDELLIPSE:		OnFilledEllipse();
										return TRUE;
		case TMAX_FILLEDRECTANGLE:		OnFilledRectangle();
										return TRUE;
		case TMAX_POLYLINE:				OnPolyline();
										return TRUE;
		case TMAX_POLYGON:				OnPolygon();
										return TRUE;
		case TMAX_ANNTEXT:				OnAnnText();
										return TRUE;
		case TMAX_FULLSCREEN:			OnFullScreen();
										return TRUE;
		case TMAX_STATUSBAR:			OnStatusBar();
										return TRUE;
		case TMAX_NEXTMEDIA:			OnNextMedia();
										return TRUE;
		case TMAX_PREVMEDIA:			OnPreviousMedia();
										return TRUE;
		case TMAX_NEXT_BARCODE:			OnNextBarcode();
										return TRUE;
		case TMAX_PREV_BARCODE:			OnPreviousBarcode();
										return TRUE;
		case TMAX_FIRSTSHOWITEM:		OnFirstShowItem();
										return TRUE;
		case TMAX_PREVSHOWITEM:			OnPreviousShowItem();
										return TRUE;
		case TMAX_NEXTSHOWITEM:			OnNextShowItem();
										return TRUE;
		case TMAX_LASTSHOWITEM:			OnLastShowItem();
										return TRUE;
		case TMAX_MOUSEMODE:			OnMouseMode();
										return TRUE;
		case TMAX_SHOWTOOLBAR:			OnShowToolbar();
										return TRUE;
		case TMAX_NEXTPAGE_HORIZONTAL:	OnNextPageHorizontal();
										return TRUE;
		case TMAX_NEXTPAGE_VERTICAL:	OnNextPageVertical();
										return TRUE;
		case TMAX_SPLITPAGES_NEXT:		OnSplitPagesNext();
										return TRUE;
		case TMAX_SPLITPAGES_PREVIOUS:	OnSplitPagesPrevious();
										return TRUE;
		case TMAX_SCREENCAPTURE:		OnScreenCapture();
										return TRUE;
		case TMAX_SHADEONCALLOUT:		OnShadeOnCallout();
										return TRUE;
		case TMAX_ADD_TO_BINDER:		OnAddToBinder();
										return TRUE;
		case TMAX_UPDATE_ZAP:			OnUpdateZap();
										return TRUE;
		case TMAX_GESTURE_PAN:			OnGesturePan();
										return TRUE;
		case TMAX_BINDERLIST:			OnOpenBinder();
										return TRUE;
		default:						return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::ProcessCommandKey()
//
// 	Description:	This function is called by the keyboard hook. It will map
//					ascii keystrokes to application commands
//
// 	Returns:		TRUE if processed
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::ProcessCommandKey(char cKey)
{
	char	cHotkey = toupper(cKey);
	short	sKeyState = GetTMKeyState();

	//	If the control key is pressed, we have to translate the character code
	if(sKeyState & TMKEY_CONTROL)
		cHotkey += 0x40;

/*
char cOriginal = cKey;
char cUpper = toupper(cKey);
CString M;
M.Format("State: %d\nOriginal: %c [%d]\nUpper: %c [%d]\nCommand: %c [%d]\nNext: %c [%d]\n", 
		 sKeyState, cOriginal, (int)cOriginal, cUpper, (int)cUpper, 
		 cHotkey, (int)cHotkey,
		 m_Hotkeys[HK_NEXT_BARCODE].cKey, m_Hotkeys[HK_NEXT_BARCODE].sState);
MessageBox(M, "ProcessCommandKey");
*/

	if(cHotkey == m_Hotkeys[HK_BLANK].cKey &&
	   sKeyState == m_Hotkeys[HK_BLANK].sState)
		return ProcessCommand(TMAX_CLEAR);
	
	else if(cHotkey == m_Hotkeys[HK_CALLOUT].cKey &&
	        sKeyState == m_Hotkeys[HK_CALLOUT].sState)
		return ProcessCommand(TMAX_CALLOUT);
	
	else if(cHotkey == m_Hotkeys[HK_PAN].cKey &&
	        sKeyState == m_Hotkeys[HK_PAN].sState)
		return ProcessCommand(TMAX_PAN);
	
	else if(cHotkey == m_Hotkeys[HK_DRAW].cKey &&
	        sKeyState == m_Hotkeys[HK_DRAW].sState)
		return ProcessCommand(TMAX_DRAWTOOL);
	
	else if(cHotkey == m_Hotkeys[HK_HIGHLIGHT].cKey &&
	        sKeyState == m_Hotkeys[HK_HIGHLIGHT].sState)
		return ProcessCommand(TMAX_HIGHLIGHT);
	
	else if(cHotkey == m_Hotkeys[HK_REDACT].cKey &&
	        sKeyState == m_Hotkeys[HK_REDACT].sState)
		return ProcessCommand(TMAX_REDACT);
	
	else if(cHotkey == m_Hotkeys[HK_ERASE].cKey &&
	        sKeyState == m_Hotkeys[HK_ERASE].sState)
		return ProcessCommand(TMAX_ERASE);
	
	else if(cHotkey == m_Hotkeys[HK_CW].cKey &&
	        sKeyState == m_Hotkeys[HK_CW].sState)
		return ProcessCommand(TMAX_ROTATECW);
	
	else if(cHotkey == m_Hotkeys[HK_CCW].cKey &&
	        sKeyState == m_Hotkeys[HK_CCW].sState)
		return ProcessCommand(TMAX_ROTATECCW);
	
	else if(cHotkey == m_Hotkeys[HK_PRINT].cKey &&
	        sKeyState == m_Hotkeys[HK_PRINT].sState)
		return ProcessCommand(TMAX_PRINT);
	
	else if(cHotkey == m_Hotkeys[HK_FULLPAGE].cKey &&
	        sKeyState == m_Hotkeys[HK_FULLPAGE].sState)
		return ProcessCommand(TMAX_NORMAL);
	
	else if(cHotkey == m_Hotkeys[HK_FULLWIDTH].cKey &&
	        sKeyState == m_Hotkeys[HK_FULLWIDTH].sState)
		return ProcessCommand(TMAX_ZOOMWIDTH);
	
	else if(cHotkey == m_Hotkeys[HK_MAGNIFY].cKey &&
	        sKeyState == m_Hotkeys[HK_MAGNIFY].sState)
		return ProcessCommand(TMAX_ZOOM);
	
	else if(cHotkey == m_Hotkeys[HK_ZAP].cKey &&
	        sKeyState == m_Hotkeys[HK_ZAP].sState)
		return ProcessCommand(TMAX_SAVEZAP);
	
	else if(cHotkey == m_Hotkeys[HK_NEXTPAGE].cKey &&
	        sKeyState == m_Hotkeys[HK_NEXTPAGE].sState)
		return ProcessCommand(TMAX_NEXTPAGE);
	
	else if(cHotkey == m_Hotkeys[HK_PREVPAGE].cKey &&
	        sKeyState == m_Hotkeys[HK_PREVPAGE].sState)
		return ProcessCommand(TMAX_PREVPAGE);
	
	else if(cHotkey == m_Hotkeys[HK_PLAYPAUSE].cKey &&
	        sKeyState == m_Hotkeys[HK_PLAYPAUSE].sState)
		return ProcessCommand(TMAX_PLAY);
	
	else if(cHotkey == m_Hotkeys[HK_SPLITVERTICAL].cKey &&
	        sKeyState == m_Hotkeys[HK_SPLITVERTICAL].sState)
		return ProcessCommand(TMAX_SPLITVERTICAL);
	
	else if(cHotkey == m_Hotkeys[HK_SPLITHORIZONTAL].cKey &&
	        sKeyState == m_Hotkeys[HK_SPLITHORIZONTAL].sState)
		return ProcessCommand(TMAX_SPLITHORIZONTAL);
	
	else if(cHotkey == m_Hotkeys[HK_CHANGEPANE].cKey &&
	        sKeyState == m_Hotkeys[HK_CHANGEPANE].sState)
		return ProcessCommand(TMAX_SWITCHPANE);
	
	else if(cHotkey == m_Hotkeys[HK_SETPAGELINE].cKey &&
	        sKeyState == m_Hotkeys[HK_SETPAGELINE].sState)
		return ProcessCommand(TMAX_SETPAGELINE);
	
	else if(cHotkey == m_Hotkeys[HK_SETPAGELINENEXT].cKey &&
	        sKeyState == m_Hotkeys[HK_SETPAGELINENEXT].sState)
		return ProcessCommand(TMAX_SETPAGELINENEXT);
	
	else if(cHotkey == m_Hotkeys[HK_DELETEANN].cKey &&
	        sKeyState == m_Hotkeys[HK_DELETEANN].sState)
		return ProcessCommand(TMAX_DELETEANN);
	
	else if(cHotkey == m_Hotkeys[HK_SELECT].cKey &&
	        sKeyState == m_Hotkeys[HK_SELECT].sState)
		return ProcessCommand(TMAX_SELECT);

	else if(cHotkey == m_Hotkeys[HK_PLAYTHROUGH].cKey &&
	        sKeyState == m_Hotkeys[HK_PLAYTHROUGH].sState)
		return ProcessCommand(TMAX_PLAYTHROUGH);

	else if(cHotkey == m_Hotkeys[HK_VIDEOCAPTION].cKey &&
	        sKeyState == m_Hotkeys[HK_VIDEOCAPTION].sState)
		return ProcessCommand(TMAX_VIDEOCAPTION);

	else if(cHotkey == m_Hotkeys[HK_TEXT].cKey &&
	        sKeyState == m_Hotkeys[HK_TEXT].sState)
		return ProcessCommand(TMAX_TEXT);

	else if(cHotkey == m_Hotkeys[HK_FULLSCREEN].cKey &&
	        sKeyState == m_Hotkeys[HK_FULLSCREEN].sState)
		return ProcessCommand(TMAX_FULLSCREEN);

	else if(cHotkey == m_Hotkeys[HK_SELECTTOOL].cKey &&
	        sKeyState == m_Hotkeys[HK_SELECTTOOL].sState)
		return ProcessCommand(TMAX_SELECTTOOL);

	else if(cHotkey == m_Hotkeys[HK_STATUSBAR].cKey &&
	        sKeyState == m_Hotkeys[HK_STATUSBAR].sState)
		return ProcessCommand(TMAX_STATUSBAR);

	else if(cHotkey == m_Hotkeys[HK_NEXTMEDIA].cKey &&
	        sKeyState == m_Hotkeys[HK_NEXTMEDIA].sState)
		return ProcessCommand(TMAX_NEXTMEDIA);

	else if(cHotkey == m_Hotkeys[HK_PREVMEDIA].cKey &&
	        sKeyState == m_Hotkeys[HK_PREVMEDIA].sState)
		return ProcessCommand(TMAX_PREVMEDIA);

	else if(cHotkey == m_Hotkeys[HK_NEXT_BARCODE].cKey &&
	        sKeyState == m_Hotkeys[HK_NEXT_BARCODE].sState)
		return ProcessCommand(TMAX_NEXT_BARCODE);

	else if(cHotkey == m_Hotkeys[HK_PREV_BARCODE].cKey &&
	        sKeyState == m_Hotkeys[HK_PREV_BARCODE].sState)
		return ProcessCommand(TMAX_PREV_BARCODE);

	else if(cHotkey == m_Hotkeys[HK_ADD_TO_BINDER].cKey &&
	        sKeyState == m_Hotkeys[HK_ADD_TO_BINDER].sState)
		return ProcessCommand(TMAX_ADD_TO_BINDER);

	else if(cHotkey == m_Hotkeys[HK_FREEHAND].cKey &&
	        sKeyState == m_Hotkeys[HK_FREEHAND].sState)
		return ProcessCommand(TMAX_FREEHAND);

	else if(cHotkey == m_Hotkeys[HK_ANNTEXT].cKey &&
	        sKeyState == m_Hotkeys[HK_ANNTEXT].sState)
		return ProcessCommand(TMAX_ANNTEXT);

	else if(cHotkey == m_Hotkeys[HK_LINE].cKey &&
	        sKeyState == m_Hotkeys[HK_LINE].sState)
		return ProcessCommand(TMAX_LINE);

	else if(cHotkey == m_Hotkeys[HK_ARROW].cKey &&
	        sKeyState == m_Hotkeys[HK_ARROW].sState)
		return ProcessCommand(TMAX_ARROW);

	else if(cHotkey == m_Hotkeys[HK_ELLIPSE].cKey &&
	        sKeyState == m_Hotkeys[HK_ELLIPSE].sState)
		return ProcessCommand(TMAX_ELLIPSE);

	else if(cHotkey == m_Hotkeys[HK_RECTANGLE].cKey &&
	        sKeyState == m_Hotkeys[HK_RECTANGLE].sState)
		return ProcessCommand(TMAX_RECTANGLE);

	else if(cHotkey == m_Hotkeys[HK_POLYLINE].cKey &&
	        sKeyState == m_Hotkeys[HK_POLYLINE].sState)
		return ProcessCommand(TMAX_POLYLINE);

	else if(cHotkey == m_Hotkeys[HK_POLYGON].cKey &&
	        sKeyState == m_Hotkeys[HK_POLYGON].sState)
		return ProcessCommand(TMAX_POLYGON);

	else if(cHotkey == m_Hotkeys[HK_FILLEDRECTANGLE].cKey &&
	        sKeyState == m_Hotkeys[HK_FILLEDRECTANGLE].sState)
		return ProcessCommand(TMAX_FILLEDRECTANGLE);

	else if(cHotkey == m_Hotkeys[HK_FILLEDELLIPSE].cKey &&
	        sKeyState == m_Hotkeys[HK_FILLEDELLIPSE].sState)
		return ProcessCommand(TMAX_FILLEDELLIPSE);

	else if(cHotkey == m_Hotkeys[HK_BLACK].cKey &&
	        sKeyState == m_Hotkeys[HK_BLACK].sState)
		return ProcessCommand(TMAX_BLACK);

	else if(cHotkey == m_Hotkeys[HK_YELLOW].cKey &&
	        sKeyState == m_Hotkeys[HK_YELLOW].sState)
		return ProcessCommand(TMAX_YELLOW);

	else if(cHotkey == m_Hotkeys[HK_WHITE].cKey &&
	        sKeyState == m_Hotkeys[HK_WHITE].sState)
		return ProcessCommand(TMAX_WHITE);

	else if(cHotkey == m_Hotkeys[HK_RED].cKey &&
	        sKeyState == m_Hotkeys[HK_RED].sState)
		return ProcessCommand(TMAX_RED);

	else if(cHotkey == m_Hotkeys[HK_GREEN].cKey &&
	        sKeyState == m_Hotkeys[HK_GREEN].sState)
		return ProcessCommand(TMAX_GREEN);

	else if(cHotkey == m_Hotkeys[HK_BLUE].cKey &&
	        sKeyState == m_Hotkeys[HK_BLUE].sState)
		return ProcessCommand(TMAX_BLUE);

	else if(cHotkey == m_Hotkeys[HK_LIGHTRED].cKey &&
	        sKeyState == m_Hotkeys[HK_LIGHTRED].sState)
		return ProcessCommand(TMAX_LIGHTRED);

	else if(cHotkey == m_Hotkeys[HK_LIGHTGREEN].cKey &&
	        sKeyState == m_Hotkeys[HK_LIGHTGREEN].sState)
		return ProcessCommand(TMAX_LIGHTGREEN);

	else if(cHotkey == m_Hotkeys[HK_LIGHTBLUE].cKey &&
	        sKeyState == m_Hotkeys[HK_LIGHTBLUE].sState)
		return ProcessCommand(TMAX_LIGHTBLUE);

	else if(cHotkey == m_Hotkeys[HK_MOUSEMODE].cKey &&
	        sKeyState == m_Hotkeys[HK_MOUSEMODE].sState)
		return ProcessCommand(TMAX_MOUSEMODE);

	else if(cHotkey == m_Hotkeys[HK_TOOLBAR].cKey &&
	        sKeyState == m_Hotkeys[HK_TOOLBAR].sState)
		return ProcessCommand(TMAX_SHOWTOOLBAR);

	else if(cHotkey == m_Hotkeys[HK_ZOOMRESTRICTED].cKey &&
	        sKeyState == m_Hotkeys[HK_ZOOMRESTRICTED].sState)
		return ProcessCommand(TMAX_ZOOMRESTRICTED);

	else if(cHotkey == m_Hotkeys[HK_NEXTPAGE_HORIZONTAL].cKey &&
	        sKeyState == m_Hotkeys[HK_NEXTPAGE_HORIZONTAL].sState)
		return ProcessCommand(TMAX_NEXTPAGE_HORIZONTAL);

	else if(cHotkey == m_Hotkeys[HK_NEXTPAGE_VERTICAL].cKey &&
	        sKeyState == m_Hotkeys[HK_NEXTPAGE_VERTICAL].sState)
		return ProcessCommand(TMAX_NEXTPAGE_VERTICAL);

	else if(cHotkey == m_Hotkeys[HK_SPLITPAGES_NEXT].cKey &&
	        sKeyState == m_Hotkeys[HK_SPLITPAGES_NEXT].sState)
		return ProcessCommand(TMAX_SPLITPAGES_NEXT);

	else if(cHotkey == m_Hotkeys[HK_SPLITPAGES_PREVIOUS].cKey &&
	        sKeyState == m_Hotkeys[HK_SPLITPAGES_PREVIOUS].sState)
		return ProcessCommand(TMAX_SPLITPAGES_PREVIOUS);

	else if(cHotkey == m_Hotkeys[HK_SHADEONCALLOUT].cKey &&
	        sKeyState == m_Hotkeys[HK_SHADEONCALLOUT].sState)
		return ProcessCommand(TMAX_SHADEONCALLOUT);

	else if(cHotkey == m_Hotkeys[HK_UPDATE_ZAP].cKey &&
	        sKeyState == m_Hotkeys[HK_UPDATE_ZAP].sState)
		return ProcessCommand(TMAX_UPDATE_ZAP);
	
	else if(cHotkey == m_Hotkeys[HK_SPLIT_ZAP].cKey &&
	        sKeyState == m_Hotkeys[HK_SPLIT_ZAP].sState)
		return ProcessCommand(TMAX_SAVE_SPLIT_ZAP);

	else if(cHotkey == m_Hotkeys[HK_GESTURE_PAN].cKey &&
	        sKeyState == m_Hotkeys[HK_GESTURE_PAN].sState)
		return ProcessCommand(TMAX_GESTURE_PAN);
	
	else

		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CMainView::ProcessEvent()
//
// 	Description:	This function is called when a system event is triggered.
//
// 	Returns:		TRUE if successful
//
//	Notes:			The dwParam parameters are dependent upon the event that
//					occurs. It will be cast to the appropriate value by the
//					event handler.
//
//==============================================================================
BOOL CMainView::ProcessEvent(short sEvent, DWORD dwParam1, DWORD dwParam2)
{
	short				sAction;
	short				sNextState;
	CString				strTreatment;
	SMultipageInfo*		pMPNew;
	SMultipageInfo*		pMPOld;
	SPlaylistParams*	pPLNew;
	CString				strEvent;
	CString				strFilename;
	//BOOL				bSetDisplay = FALSE;

	//	Get the action and next state from the state table
	sAction    = STT[sEvent][m_sState].sAction;
	sNextState = STT[sEvent][m_sState].sNextState;

	//	What action has to be taken?
	switch(sAction)
	{
		case A_LOADPLAYLIST:
		case A_LOADTEXT:

			//	Get the information structure
			if((pPLNew = (SPlaylistParams*)dwParam1) == 0)
				return FALSE;

			//	Do we have a valid playlist and designation?
			if((pPLNew->pPlaylist == 0) || (pPLNew->pStart == 0))
				return FALSE;

			//	Reset the link information
			//
			//	NOTE:	We have to do this between playlists in case the user turned
			//			off the video on a link event
			m_AppLink.Clear();

			//	Reset the playlist status information
			ZeroMemory(&m_PlaylistStatus, sizeof(m_PlaylistStatus));
			UpdateStatusBar();

			//	Initialize the status information
			m_PlaylistStatus.bShowPlaylist = TRUE;
			m_PlaylistStatus.lDesignationCount = pPLNew->pPlaylist->m_Designations.GetCount();
			
			//	This flag will inhibit processing of LinkChange events
			//	fired by the TMMovie control until we finish loading
			m_bLoadingPlaylist = TRUE;

			//	Set up the text display
			if(pPLNew->pPlaylist->GetHasText())
			{
				m_PlaylistStatus.bShowPageLine = TRUE;
					
				m_ctrlTMText.SetPlaylist((long)pPLNew->pPlaylist, FALSE);
				
				//	Combine all designations if the flag is set and we are
				//	playing from the start of the playlist
				if(m_bCombineText && (pPLNew->lStart <= 0))
					m_ctrlTMText.SetCombineDesignations(TRUE);
				else
					m_ctrlTMText.SetCombineDesignations(FALSE);
			}
			else
			{
				m_ctrlTMText.SetPlaylist(0, 0);
				m_ctrlTMText.SetCombineDesignations(FALSE);
			}

			//	Set up the TMMovie control to play the playlist
			if(m_ctrlTMMovie.PlayPlaylist((LONG)pPLNew->pPlaylist, 
										  pPLNew->lStart, 
										  pPLNew->lStop,
										  pPLNew->dPosition) != TMMOVIE_NOERROR) 
			{
				m_ctrlTMText.SetPlaylist(0, FALSE);
				m_bLoadingPlaylist = FALSE;
				return FALSE;
			}
			
			//	Reset the cue parameters if this is not an animation
			if(!pPLNew->pPlaylist->GetIsRecording())
			{
				m_iCuePage = 0;
				m_iCueLine = 0;
				m_lCueTranscript = 0;
			}

			//	Set up the status bar
			m_PlaylistStatus.lDesignationOrder = (pPLNew->lStart > 0) ? 
												  pPLNew->lStart : 1;
			//	Reset the pointer to the active playlist 
			if((m_Playlist.pPlaylist != 0) && (pPLNew->pPlaylist != m_Playlist.pPlaylist))
			{
				delete m_Playlist.pPlaylist;
				m_Playlist.pDesignation = NULL;
			}
			m_Playlist.pPlaylist = pPLNew->pPlaylist;
			m_Playlist.pDesignation = pPLNew->pStart;

			//	Do we need to change the screen state?
			if(sNextState != m_sState)
			{	
				SetDisplay(sNextState);
			}
			else
			{
				//	Make sure the text display is redrawn 
				if(sNextState == S_TEXT)
					m_ctrlTMText.RedrawWindow();
			}

			//	Start the playback
			if(m_Playlist.pPlaylist)
			{
				m_ctrlTMMovie.Play();
			
				if(m_Test.bActive)
				{
					strEvent.Format("Run playlist %ld.0", m_Playlist.pPlaylist->m_lPrimaryId);
					UpdateActivityLog(strEvent);
				}
			}

			//	Update the status bar
			//
			//	NOTE:	We can't do this until AFTER we set the display state
			UpdatePlaylistStatus();

			//	We can clear this flag now and check to see if there is a pending
			//	link
			m_bLoadingPlaylist = FALSE;
			if(m_AppLink.GetIsPending() == TRUE)
			{
				if(lstrlen(m_AppLink.GetDatabaseId()) > 0)
					OnAxLinkChange(m_AppLink.GetDatabaseId(), 0, m_AppLink.GetAttributes());
				else
					OnAxLinkChange(m_AppLink.GetBarcode(), 0, m_AppLink.GetAttributes());
			}
			theApp.bSetDisplay = FALSE;
			break;
	
		case A_LOADDOCUMENT:
		case A_LOADGRAPHIC:
		case A_LINKDOCUMENT:
		case A_LINKGRAPHIC:

			//	Get the information structure
			if((pMPNew = (SMultipageInfo*)dwParam1) == 0)
				return FALSE;

			//	Do we have a valid page?
			if((pMPNew->pMultipage == NULL) || (pMPNew->pSecondary == NULL))
				return FALSE;

			//	Reset the media information not being used
			//
			//	NOTE: Only reset the TMMovie information if not linking to a clip
			ResetMultipage(&m_TMPower1);
			ResetMultipage(&m_TMPower2);
			if((sAction == A_LOADDOCUMENT) || (sAction == A_LOADGRAPHIC) || (m_Playlist.pPlaylist != NULL))
				ResetMultipage(&m_TMMovie);
			
			//	Get the page file specification
			ASSERT(m_pDatabase != 0);
			m_pDatabase->GetFilename(pMPNew->pMultipage, pMPNew->pSecondary, strFilename);

			//	Does the file exist?
			if(!FindFile(strFilename))
			{
				if(sAction == A_LOADDOCUMENT || sAction == A_LINKDOCUMENT)
					HandleError(0, IDS_DOCNOTFOUND, strFilename);
				else
					HandleError(0, IDS_GRAPHICNOTFOUND, strFilename);
				return FALSE;
			}

			//	Temporarily disable the sync panes option if we are in split
			//	screen mode so that setting the scale option will not affect
			//	both panes
			if(m_ctrlTMView->GetSplitScreen())
				m_ctrlTMView->SetSyncPanes(FALSE);

			//	Do the processing that is ACTION specific here
			switch(sAction)
			{
				case A_LINKDOCUMENT:
				case A_LINKGRAPHIC:

					//	Disable split screen mode
					m_ctrlTMView->SetSplitScreen(FALSE);
					theApp.bSetDisplay = FALSE;
					break;

				case A_LOADDOCUMENT:
				case A_LOADGRAPHIC:
				default:
				
					//	Reset the TMMovie control 
					if(m_Playlist.pPlaylist)
						ResetTMMovie(FALSE, TRUE);

					break;
			}
		
			//	Are we supposed to be loading a treatment?
			if(pMPNew->pTertiary != NULL)
			{
				LoadTreatment(pMPNew, sNextState);
			}
			else
			{
				//	Is this a document or graphic?
				if(sAction == A_LOADDOCUMENT || sAction == A_LINKDOCUMENT)
					m_ctrlTMView->SetScaleImage(m_bScaleDocs);
				else
					m_ctrlTMView->SetScaleImage(m_bScaleGraphics);

				//	Force single pane mode if running a custom show
				//if(m_bLoadingShowItem == TRUE)
				//	SetSinglePaneMode();
				//else
				//	SetZapSplitScreen(FALSE);
				
				//	Do we need to change the screen state?
				if(sNextState != m_sState)
				{
					//m_ctrlTMView->ShowWindow(SW_HIDE);
				}
				//	Load the new file
				m_ctrlTMView->LoadFile(strFilename, TMV_ACTIVEPANE);

				//	Reset the multipage information
				pMPOld = (SMultipageInfo*)m_ctrlTMView->GetData(-1);
				ASSERT(pMPOld);
				if(pMPOld != NULL)
					UpdateMultipage(pMPOld, pMPNew);
			
			}// if(pMPNew->pTertiary != NULL)

			//	Make sure pane synchronization is reenabled
			m_ctrlTMView->SetSyncPanes(TRUE);
			
			//	Do we need to change the screen state?
			if(sNextState != m_sState)
			{
				if(theApp.bSetDisplay == FALSE)
				{
					SetDisplay(sNextState);
					theApp.bSetDisplay = TRUE;
				}
				else
				{
					m_sPrevState = m_sState;
					m_sState = sNextState;
					SetControlBar(m_ControlBar.iId);
				}
				//m_ctrlTMView->ShowWindow(SW_SHOW );
				for(int i =0; i < SZ_ARR_TM_VW; i++)
					m_arrTmView[i]->ShowWindow(SW_SHOW);
			}
			else if(m_sState == S_LINKEDIMAGE)
			{
				//	Do we need to update the display?
				if(CheckLinkOptions() == TRUE)
					SetDisplay(m_sState);
			}

			//	Update the playlist status if displaying a linked image
			if(m_sState == S_LINKEDIMAGE)
			{
				UpdatePlaylistStatus();
			}

			break;
	
		case A_LOADMOVIE:

			//	Get the information structure
			if((pMPNew = (SMultipageInfo*)dwParam1) == 0)
				return FALSE;

			//	Do we have a valid page?
			if((pMPNew->pMultipage == 0) || (pMPNew->pSecondary == 0))
				return FALSE;

			//	Get the page file specification
			ASSERT(m_pDatabase != 0);
			m_pDatabase->GetFilename(pMPNew->pMultipage, pMPNew->pSecondary, strFilename);

			//	Does the file exist?
			if(!FindFile(strFilename))
			{	
				HandleError(0, IDS_CLIPNOTFOUND, strFilename);
				return FALSE;
			}

			//	Reset the player if we were previously viewing a playlist
			if(m_Playlist.pPlaylist)
				ResetTMMovie(FALSE, TRUE);

			//	Load the file and set the playback range
			m_ctrlTMMovie.Load(strFilename, 0.0, 0.0, FALSE);
			m_ctrlTMMovie.SetOverlayVisible(FALSE);
			m_ctrlTMMovie.SetOverlayFile("");
			
			//	Is this a clip?
			if(pMPNew->pTertiary != 0)
			{
				m_dMinMovieCue = pMPNew->pTertiary->m_dStartTime;
				m_dMaxMovieCue = pMPNew->pTertiary->m_dStopTime;

				//	Confine the playback range
				m_ctrlTMMovie.SetRange(m_dMinMovieCue, m_dMaxMovieCue);
			}
			else
			{
				m_dMinMovieCue = 0.0;

				//	Set the maximum cue time for this movie. We only allow the user
				//	to cue within one step interval of the end of the file. We have
				//	to make sure the user can't cue to within 2 seconds of the
				//	actual end of the file
				m_dMaxMovieCue = m_ctrlTMMovie.GetMaxTime();
				if(m_fMovieStep > 1)
					m_dMaxMovieCue -= m_fMovieStep; 
				else
					m_dMaxMovieCue -= 2.0; 
				
				//	This ensures the full file gets played in case we are bouncing
				//	between the segment and a clip of the same segment
				m_ctrlTMMovie.SetRange(0.0, 0.0);
			}

			m_ctrlTMMovie.SetMinCuePosition(m_dMinMovieCue);
			m_ctrlTMMovie.SetMaxCuePosition(m_dMaxMovieCue);

			//	Reset the multipage information
			UpdateMultipage(&m_TMMovie, pMPNew);
				
			//	Do we need to change the screen state?
			if(sNextState != m_sState)
				SetDisplay(sNextState);
		
			m_ctrlTMMovie.Play();
			theApp.bSetDisplay = FALSE;
			break;
	
		case A_LOADPOWER:
		case A_LINKPOWER:

			//	Get the information structure
			if((pMPNew = (SMultipageInfo*)dwParam1) == 0)
				return FALSE;

			//	Do we have a valid page?
			if((pMPNew->pMultipage == 0) || (pMPNew->pSecondary == 0))
				return FALSE;

			//	Reset the media information not being used
			//
			//	NOTE: Only reset the TMMovie information if not linking to a clip
			for(vector<SMultipageInfo *>::iterator it = m_arrMultiPageInfo.begin();
			it != m_arrMultiPageInfo.end(); it++) {
				ResetMultipage(*it);
			}

			if((sAction == A_LOADPOWER) || (m_Playlist.pPlaylist != 0))
				ResetMultipage(&m_TMMovie);

			//	Get the page file specification
			ASSERT(m_pDatabase != 0);
			m_pDatabase->GetFilename(pMPNew->pMultipage, pMPNew->pSecondary, strFilename);

			//	Does the file exist?
			if(!FindFile(strFilename))
			{
				HandleError(0, IDS_POWERNOTFOUND, strFilename);
				return FALSE;
			}

			if(m_ctrlTMPower.LoadFile(strFilename, 
									  pMPNew->pSecondary->m_lSlideId, TRUE, -1) != TMPOWER_NOERROR)
				return FALSE;

			//	Are we linking or loading?
			if(sAction == A_LOADPOWER)
			{
				//	Reset the player if we were previously viewing a playlist 
				if(m_Playlist.pPlaylist)
					ResetTMMovie(FALSE, TRUE);
			}

			//	Reset the multipage information
			pMPOld = (SMultipageInfo*)m_ctrlTMPower.GetData(-1);
			ASSERT(pMPOld);
			if(pMPOld != NULL)
				UpdateMultipage(pMPOld, pMPNew);
				
			//	Do we need to change the screen state?
			if(sNextState != m_sState)
			{
				SetDisplay(sNextState);
			}
			else if(m_sState == S_LINKEDPOWER)
			{
				//	Do we need to update the display?
				if(CheckLinkOptions() == TRUE)
					SetDisplay(m_sState);
			}

			//	Update the playlist status if displaying a linked presentation
			if(m_sState == S_LINKEDPOWER)
				UpdatePlaylistStatus();

			theApp.bSetDisplay = FALSE;
			break;
	
		case A_NONE:

			//	Do we need to change the screen state?
			if(sNextState != m_sState)
				SetDisplay(sNextState);

			break;

	}//	switch(sAction)

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CMainView::ProcessMouseMessage()
//
// 	Description:	This function is called by the main frame to process
//					the specified mouse message.
//
// 	Returns:		TRUE if processed
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::ProcessMouseMessage(MSG* pMsg)
{

	//	Don't bother if mouse mode operation is not enabled
	if(!m_bMouseMode)
		return FALSE;

	//	What message?
	switch(pMsg->message)
	{
		case WM_LBUTTONDOWN:
			
			PostMessage(WM_MOUSEMODE, 0);
			return TRUE;

		case WM_RBUTTONDOWN:
			
			PostMessage(WM_MOUSEMODE, 1);
			return TRUE;

		default:

			return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::ProcessNotification()
//
// 	Description:	This function is called to determine if we should process
//					the WM_PARENTNOTIFY message.
//
// 	Returns:		None
//
//	Notes:			We only want to process WM_PARENTNOTIFY messages that get
//					sent by the TMMovie or TMPower controls.
//
//==============================================================================
BOOL CMainView::ProcessNotification(int iX, int iY) 
{
	POINT Point;
	CRect Rect;

	Point.x = iX;
	Point.y = iY;

	//	What is the current display state?
	switch(m_sState)
	{
		case S_PLAYLIST: 
		case S_MOVIE:
		case S_TEXT:
		case S_LINKEDIMAGE:
		
			Rect = m_rcMovie;
			return Rect.PtInRect(Point);

		case S_POWERPOINT:
		
			Rect = m_rcPower;
			return Rect.PtInRect(Point);

		case S_LINKEDPOWER:
		
			Rect = m_rcMovie;
			if(Rect.PtInRect(Point))
			{
				return TRUE;
			}
			else
			{
				Rect = m_rcPower;
				return Rect.PtInRect(Point);
			}

		case S_CLEAR:
		case S_DOCUMENT:
		case S_GRAPHIC:
		default:

			return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::ProcessVirtualKey()
//
// 	Description:	This function is called by the keyboard hook. It will map
//					virtual keystrokes to application commands
//
// 	Returns:		TRUE if processed
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::ProcessVirtualKey(WORD wKey)
{
	short	sKeyState = GetTMKeyState();
	WORD	wCmdKey = 0;

	//	Perform checks for modifier keys first
	switch(sKeyState)
	{
		case TMKEY_SHIFT:

			//	Is this one of the configuration keystrokes?
			if(wKey == VK_F1)
			{
				OnConfig();
				return TRUE;
			}
			else if(wKey == VK_F3)
			{
				OnToolbars();
				return TRUE;
			}
			else if(wKey == VK_F4)
			{
				OnFilterProps();
				return TRUE;
			}
			else
			{
				return FALSE;//	Return to caller for processing
			}

		case TMKEY_CONTROL:

			//	Exit the app on Control-End
			if(wKey == VK_END)
			{
				OnExit();
				return TRUE;
			}
			if(wKey == VK_F1)
			{
				OnTMaxHelp();
				return TRUE;
			}
			else
			{
				//	Should we attempt to translate
				//
				//	NOTE:	When control is pressed the WM_CHAR message never
				//			gets sent for keys with codes > 0x40
				if((wCmdKey = MapVirtualKey(wKey, 2)) < 0x40)
					return ProcessCommandKey(wCmdKey - 0x40);
				else
					return FALSE;
			}

		case TMKEY_SHIFTCONTROL:

			//	See notes above
			//
			//	The addition of 0x10 allows for the shift key
			if((wCmdKey = MapVirtualKey(wKey, 2)) < 0x40)
				return ProcessCommandKey(wCmdKey - 0x40 + 0x10);
			else
				return FALSE;

		case TMKEY_NONE:
		case TMKEY_ALT:
		case TMKEY_SHIFTALT:
		case TMKEY_SHIFTALTCONTROL:
		case TMKEY_ALTCONTROL:
		default:
			
			//	Drop through and process the keystroke
			break;
	
	}// switch(sKeyState)

	//	What key has been pressed?
	switch(wKey)
	{
		case VK_LEFT:

			//	If we have an active custom show override the default handling
			if(m_Show.pItem != 0)
			{
				OnPreviousShowItem();
			}
			else
			{
				//	Make the decision based on the screen state
				switch(m_sState)
				{
					case S_MOVIE:

						OnStartMovie();
						break;

					case S_PLAYLIST:
					case S_TEXT:
				
						OnPreviousDesignation();
						break;

					case S_LINKEDIMAGE:
					case S_DOCUMENT:
					case S_GRAPHIC:
					case S_POWERPOINT:
					case S_LINKEDPOWER:
					case S_CLEAR:

						OnPreviousPage();
						break;

					default:

						break;
				
				}//switch(m_sState)

			}//if((m_sState != S_CLEAR && (m_Show.pItem != 0))
			break;

		case VK_RIGHT:

			//	If we have an active custom show override the default handling
			if(m_Show.pItem != 0)
			{
				OnNextShowItem();
			}
			else
			{
				//	Make the decision based on the screen state
				switch(m_sState)
				{
					case S_MOVIE:

						OnEndMovie();
						break;

					case S_PLAYLIST:
					case S_TEXT:
				
						OnNextDesignation();
						break;

					case S_LINKEDIMAGE:
					case S_DOCUMENT:
					case S_GRAPHIC:
					case S_POWERPOINT:
					case S_LINKEDPOWER:
					case S_CLEAR:

						OnNextPage();
						break;

					default:

						break;
				
				}//switch(m_sState)

			}//if((m_sState != S_CLEAR && (m_Show.pItem != 0))
			break;

		case VK_HOME:

			//	If we have an active custom show override the default handling
			if(m_Show.pItem != 0)
			{
				OnFirstShowItem();
			}
			else
			{
				//	Make the decision based on the screen state
				switch(m_sState)
				{
					case S_MOVIE:

						OnStartMovie();
						break;

					case S_PLAYLIST:
					case S_LINKEDIMAGE:
					case S_LINKEDPOWER:
					case S_TEXT:
				
						OnFirstDesignation();
						break;

					case S_DOCUMENT:
					case S_GRAPHIC:
					case S_POWERPOINT:
					case S_CLEAR:

						OnFirstPage();
						break;

					default:
						break;
				
				}//switch(m_sState)
			
			}//if((m_sState != S_CLEAR && (m_Show.pItem != 0))
			break;

		case VK_END:

			//	If we have an active custom show override the default handling
			if(m_Show.pItem != 0)
			{
				OnLastShowItem();
			}
			else
			{
				//	Make the decision based on the screen state
				switch(m_sState)
				{
					case S_MOVIE:

						OnEndMovie();
						break;

					case S_PLAYLIST:
					case S_LINKEDIMAGE:
					case S_LINKEDPOWER:
					case S_TEXT:
				
						OnLastDesignation();
						break;
					
					case S_DOCUMENT:
					case S_GRAPHIC:
					case S_POWERPOINT:
					case S_CLEAR:

						OnLastPage();
						break;

					default:
						break;
				
				}//switch(m_sState)

			}
			break;

		case VK_PRIOR:// Page-up

			switch(m_sState)
			{
				case S_MOVIE:

					OnBackMovie();
					break;

				case S_PLAYLIST:
				case S_TEXT:
				
					OnBackDesignation();
					break;
				
				case S_LINKEDIMAGE:
				case S_DOCUMENT:
				case S_GRAPHIC:

					m_ctrlTMView->Pan(PAN_UP, TMV_ACTIVEPANE);
					break;

				case S_LINKEDPOWER:
				case S_POWERPOINT:
				case S_CLEAR:
				default:
					break;
			}
			break;

		case VK_NEXT:// Page-down

			switch(m_sState)
			{
				case S_MOVIE:

					OnFwdMovie();
					break;

				case S_PLAYLIST:
				case S_TEXT:
				
					OnFwdDesignation();
					break;
				
				case S_LINKEDIMAGE:
				case S_DOCUMENT:
				case S_GRAPHIC:

					m_ctrlTMView->Pan(PAN_DOWN, TMV_ACTIVEPANE);
					break;

				case S_LINKEDPOWER:
				case S_POWERPOINT:
				case S_CLEAR:
				default:
					break;
			}
			break;

		case VK_UP:

			switch(m_sState)
			{
				case S_MOVIE:

					OnStartMovie();
					break;

				case S_PLAYLIST:
				case S_TEXT:
				
					OnStartDesignation();
					break;

				case S_LINKEDIMAGE:
				case S_DOCUMENT:
				case S_GRAPHIC:

					ProcessCommand(TMAX_PREVZAP);
					break;

				case S_LINKEDPOWER:
				case S_POWERPOINT:

					//	Go to previous slide if viewing PowerPoint custom
					//	show item
					if(m_Show.pItem != 0)
						OnPreviousPage();
					break;

				case S_CLEAR:
				default:

					break;
			}
			break;

		case VK_DOWN:

			switch(m_sState)
			{
				case S_MOVIE:

					OnEndMovie();
					break;

				case S_PLAYLIST:
				case S_TEXT:
				
					OnNextDesignation();
					break;

				case S_LINKEDIMAGE:
				case S_DOCUMENT:
				case S_GRAPHIC:

					ProcessCommand(TMAX_NEXTZAP);
					break;

				case S_LINKEDPOWER:
				case S_POWERPOINT:

					//	Go to next slide if viewing PowerPoint custom
					//	show item
					if(m_Show.pItem != 0)
						OnNextPage();
					break;

				case S_CLEAR:
				default:

					break;
			}
			break;

		case VK_F1:

			OnPlay();
			break;

		case VK_F9:

			OnClear();
			break;

		case VK_F11:

			OnScreenCapture();
			break;

		case VK_ESCAPE:

			OnExit();			
			break;

		case VK_TAB:

			SwitchPane();
			break;

		case VK_DELETE:

			OnDeleteAnn();
			break;

		default:

			return FALSE;
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CMainView::ReadHotkeys()
//
// 	Description:	This function will read the hotkey specifications from the
//					ini file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::ReadHotkeys()
{
	char szIniStr[64];

	m_Ini.SetSection(HOTKEYS_SECTION);

	//	Clear Screen
	m_Ini.ReadString(HK_BLANK_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_BLANK);
	ParseHotKeySpec(HK_BLANK, szIniStr);

	//	Callout
	m_Ini.ReadString(HK_CALLOUT_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_CALLOUT);
	ParseHotKeySpec(HK_CALLOUT, szIniStr);

	//	Draw
	m_Ini.ReadString(HK_DRAW_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_DRAW);
	ParseHotKeySpec(HK_DRAW, szIniStr);

	//	Highlight
	m_Ini.ReadString(HK_HIGHLIGHT_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_HIGHLIGHT);
	ParseHotKeySpec(HK_HIGHLIGHT, szIniStr);

	//	Redact
	m_Ini.ReadString(HK_REDACT_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_REDACT);
	ParseHotKeySpec(HK_REDACT, szIniStr);

	//	Magnify
	m_Ini.ReadString(HK_MAGNIFY_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_MAGNIFY);
	ParseHotKeySpec(HK_MAGNIFY, szIniStr);

	//	Print
	// when optimized for tablet, no print on 'P' instead enable gesture
	if(!m_bOptimizedForTablet) {
		m_Ini.ReadString(HK_PRINT_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_PRINT);
		ParseHotKeySpec(HK_PRINT, szIniStr);
	}

	//	Erase
	m_Ini.ReadString(HK_ERASE_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_ERASE);
	ParseHotKeySpec(HK_ERASE, szIniStr);

	//	Full Page
	m_Ini.ReadString(HK_FULLPAGE_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_FULLPAGE);
	ParseHotKeySpec(HK_FULLPAGE, szIniStr);

	//	Play/Pause
	m_Ini.ReadString(HK_PLAYPAUSE_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_PLAYPAUSE);
	ParseHotKeySpec(HK_PLAYPAUSE, szIniStr);

	//	Zap
	m_Ini.ReadString(HK_ZAP_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_ZAP);
	ParseHotKeySpec(HK_ZAP, szIniStr);

	//	Full Width
	m_Ini.ReadString(HK_FULLWIDTH_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_FULLWIDTH);
	ParseHotKeySpec(HK_FULLWIDTH, szIniStr);

	//	Prev Page
	m_Ini.ReadString(HK_PREVPAGE_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_PREVPAGE);
	ParseHotKeySpec(HK_PREVPAGE, szIniStr);

	//	Next Page
	m_Ini.ReadString(HK_NEXTPAGE_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_NEXTPAGE);
	ParseHotKeySpec(HK_NEXTPAGE, szIniStr);

	//	Pan
	m_Ini.ReadString(HK_PAN_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_PAN);
	ParseHotKeySpec(HK_PAN, szIniStr);

	//	Rotate Cw
	m_Ini.ReadString(HK_CW_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_CW);
	ParseHotKeySpec(HK_CW, szIniStr);

	//	Rotate Ccw
	m_Ini.ReadString(HK_CCW_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_CCW);
	ParseHotKeySpec(HK_CCW, szIniStr);

	//	Split vertical
	m_Ini.ReadString(HK_SPLITVERTICAL_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_SPLIT_VERTICAL);
	ParseHotKeySpec(HK_SPLITVERTICAL, szIniStr);

	//	Split horizontal
	m_Ini.ReadString(HK_SPLITHORIZONTAL_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_SPLIT_HORIZONTAL);
	ParseHotKeySpec(HK_SPLITHORIZONTAL, szIniStr);

	//	Change Pane
	m_Ini.ReadString(HK_CHANGEPANE_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_CHANGEPANE);
	ParseHotKeySpec(HK_CHANGEPANE, szIniStr);

	//	Set playlist page-line
	m_Ini.ReadString(HK_SETPAGELINE_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_SETPAGELINE);
	ParseHotKeySpec(HK_SETPAGELINE, szIniStr);

	//	Set next playlist page-line
	m_Ini.ReadString(HK_SETPAGELINENEXT_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_SETPAGELINENEXT);
	ParseHotKeySpec(HK_SETPAGELINENEXT, szIniStr);

	//	Delete annotation
	m_Ini.ReadString(HK_DELETEANN_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_DELETEANN);
	ParseHotKeySpec(HK_DELETEANN, szIniStr);

	//	Select Annotations
	m_Ini.ReadString(HK_SELECT_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_SELECT);
	ParseHotKeySpec(HK_SELECT, szIniStr);

	//	Play through to end
	m_Ini.ReadString(HK_PLAYTHROUGH_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_PLAYTHROUGH);
	ParseHotKeySpec(HK_PLAYTHROUGH, szIniStr);

	//	Show/hide video caption
	m_Ini.ReadString(HK_VIDEOCAPTION_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_VIDEOCAPTION);
	ParseHotKeySpec(HK_VIDEOCAPTION, szIniStr);

	//	Show/hide text display
	m_Ini.ReadString(HK_TEXT_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_TEXT);
	ParseHotKeySpec(HK_TEXT, szIniStr);

	//	Select drawing tool
	m_Ini.ReadString(HK_SELECTTOOL_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_SELECTTOOL);
	ParseHotKeySpec(HK_SELECTTOOL, szIniStr);

	//	Full screen video
	m_Ini.ReadString(HK_FULLSCREEN_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_FULLSCREEN);
	ParseHotKeySpec(HK_FULLSCREEN, szIniStr);

	//	Status bar
	m_Ini.ReadString(HK_STATUSBAR_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_STATUSBAR);
	ParseHotKeySpec(HK_STATUSBAR, szIniStr);

	//	Next media
	m_Ini.ReadString(HK_NEXTMEDIA_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_NEXTMEDIA);
	ParseHotKeySpec(HK_NEXTMEDIA, szIniStr);

	//	Previous media
	m_Ini.ReadString(HK_PREVMEDIA_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_PREVMEDIA);
	ParseHotKeySpec(HK_PREVMEDIA, szIniStr);

	//	Next barcode
	m_Ini.ReadString(HK_NEXT_BARCODE_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_NEXT_BARCODE);
	ParseHotKeySpec(HK_NEXT_BARCODE, szIniStr);

	//	Previous barcode
	m_Ini.ReadString(HK_PREV_BARCODE_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_PREV_BARCODE);
	ParseHotKeySpec(HK_PREV_BARCODE, szIniStr);

	//	Add to binder
	m_Ini.ReadString(HK_ADD_TO_BINDER_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_ADD_TO_BINDER);
	ParseHotKeySpec(HK_ADD_TO_BINDER, szIniStr);

	//	Freehand tool
	m_Ini.ReadString(HK_FREEHAND_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_FREEHAND);
	ParseHotKeySpec(HK_FREEHAND, szIniStr);

	//	Text tool
	m_Ini.ReadString(HK_ANNTEXT_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_ANNTEXT);
	ParseHotKeySpec(HK_ANNTEXT, szIniStr);

	//	Line tool
	m_Ini.ReadString(HK_LINE_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_LINE);
	ParseHotKeySpec(HK_LINE, szIniStr);

	//	Arrow tool
	m_Ini.ReadString(HK_ARROW_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_ARROW);
	ParseHotKeySpec(HK_ARROW, szIniStr);

	//	Ellipse tool
	m_Ini.ReadString(HK_ELLIPSE_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_ELLIPSE);
	ParseHotKeySpec(HK_ELLIPSE, szIniStr);

	//	Rectangle tool
	m_Ini.ReadString(HK_RECTANGLE_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_RECTANGLE);
	ParseHotKeySpec(HK_RECTANGLE, szIniStr);

	//	Polygon tool
	m_Ini.ReadString(HK_POLYGON_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_POLYGON);
	ParseHotKeySpec(HK_POLYGON, szIniStr);

	//	Polyline tool
	m_Ini.ReadString(HK_POLYLINE_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_POLYLINE);
	ParseHotKeySpec(HK_POLYLINE, szIniStr);

	//	Filled ellipse tool
	m_Ini.ReadString(HK_FILLEDELLIPSE_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_FILLEDELLIPSE);
	ParseHotKeySpec(HK_FILLEDELLIPSE, szIniStr);

	//	Filled rectangle tool
	m_Ini.ReadString(HK_FILLEDRECTANGLE_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_FILLEDRECTANGLE);
	ParseHotKeySpec(HK_FILLEDRECTANGLE, szIniStr);

	//	Black color
	m_Ini.ReadString(HK_BLACK_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_BLACK);
	ParseHotKeySpec(HK_BLACK, szIniStr);

	//	Yellow color
	m_Ini.ReadString(HK_YELLOW_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_YELLOW);
	ParseHotKeySpec(HK_YELLOW, szIniStr);

	//	White color
	m_Ini.ReadString(HK_WHITE_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_WHITE);
	ParseHotKeySpec(HK_WHITE, szIniStr);

	//	Red color
	m_Ini.ReadString(HK_RED_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_RED);
	ParseHotKeySpec(HK_RED, szIniStr);

	//	Green color
	m_Ini.ReadString(HK_GREEN_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_GREEN);
	ParseHotKeySpec(HK_GREEN, szIniStr);

	//	Blue color
	m_Ini.ReadString(HK_BLUE_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_BLUE);
	ParseHotKeySpec(HK_BLUE, szIniStr);

	//	Light red color
	m_Ini.ReadString(HK_LIGHTRED_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_LIGHTRED);
	ParseHotKeySpec(HK_LIGHTRED, szIniStr);

	//	Light green color
	m_Ini.ReadString(HK_LIGHTGREEN_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_LIGHTGREEN);
	ParseHotKeySpec(HK_LIGHTGREEN, szIniStr);

	//	Light blue color
	m_Ini.ReadString(HK_LIGHTBLUE_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_LIGHTBLUE);
	ParseHotKeySpec(HK_LIGHTBLUE, szIniStr);

	//	Light blue color
	m_Ini.ReadString(HK_MOUSEMODE_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_MOUSEMODE);
	ParseHotKeySpec(HK_MOUSEMODE, szIniStr);

	//	Status bar
	m_Ini.ReadString(HK_TOOLBAR_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_TOOLBAR);
	ParseHotKeySpec(HK_TOOLBAR, szIniStr);

	//	Zoom mode
	m_Ini.ReadString(HK_ZOOMRESTRICTED_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_ZOOMRESTRICTED);
	ParseHotKeySpec(HK_ZOOMRESTRICTED, szIniStr);

	// Next page - split horizontal
	m_Ini.ReadString(HK_NEXTPAGE_HORIZONTAL_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_NEXTPAGE_HORIZONTAL);
	ParseHotKeySpec(HK_NEXTPAGE_HORIZONTAL, szIniStr);

	// Next page - split vertical
	m_Ini.ReadString(HK_NEXTPAGE_VERTICAL_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_NEXTPAGE_VERTICAL);
	ParseHotKeySpec(HK_NEXTPAGE_VERTICAL, szIniStr);

	// Next page - split screen adjacent pages
	m_Ini.ReadString(HK_SPLITPAGES_NEXT_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_SPLITPAGES_NEXT);
	ParseHotKeySpec(HK_SPLITPAGES_NEXT, szIniStr);

	// Previous page - split screen adjacent pages
	m_Ini.ReadString(HK_SPLITPAGES_PREVIOUS_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_SPLITPAGES_PREVIOUS);
	ParseHotKeySpec(HK_SPLITPAGES_PREVIOUS, szIniStr);

	//	Shade On Callout
	m_Ini.ReadString(HK_SHADEONCALLOUT_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_SHADEONCALLOUT);
	ParseHotKeySpec(HK_SHADEONCALLOUT, szIniStr);

	//	Update Zap
	m_Ini.ReadString(HK_UPDATE_ZAP_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_UPDATE_ZAP);
	ParseHotKeySpec(HK_UPDATE_ZAP, szIniStr);

	//	Save Split Zap
	m_Ini.ReadString(HK_SPLIT_ZAP_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_SPLIT_ZAP);
	ParseHotKeySpec(HK_SPLIT_ZAP, szIniStr);

	//	Enable Gestures
	// when optimized for tablet, no print on 'P' instead enable gesture
	if(m_bOptimizedForTablet) {
		m_Ini.ReadString(HK_PRINT_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HK_PRINT);
	} else {
		m_Ini.ReadString(HK_ENABLE_GESTURE, szIniStr, sizeof(szIniStr), DEFAULT_HK_ENABLE_GESTURE);
	}
	ParseHotKeySpec(HK_GESTURE_PAN, szIniStr);
}

//==============================================================================
//
// 	Function Name:	CMainView::ReadSetup()
//
// 	Description:	This function is called to read the application setup
//					options from the ini file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::ReadSetup(BOOL bFirstTime) 
{
	SGraphicsOptions	Graphics;
	SVideoOptions		Video;
	STextOptions		Text;
	SSystemOptions		System;
	SDatabaseOptions	Database;
	SCaptureOptions		Capture;
	char				szIniStr[256];

	//	Read all the options from the ini file
	m_Ini.ReadGraphicsOptions(&Graphics);
	m_Ini.ReadVideoOptions(&Video);
	m_Ini.ReadTextOptions(&Text);
	m_Ini.ReadDatabaseOptions(&Database);
	m_Ini.ReadSystemOptions(&System);
	m_Ini.ReadCaptureOptions(&Capture);

	if(m_Ini.bFileFound)
	{
		m_Ini.SetTMSection(PRESENTATION_APP);
		
		//	Do we require first time initialization?
		if(bFirstTime == TRUE)
		{
			//	Get the maximum size of the barcode buffer
			m_aBarcodes.SetMaxBarcodes(m_Ini.ReadLong(MAX_BARCODES_LINE, DEFAULT_MAX_BARCODES));

			//	Get the name of the application help file
			m_Ini.ReadString(HELP_FILENAME_LINE, szIniStr, sizeof(szIniStr), DEFAULT_HELP_FILENAME);
			if(lstrlen(szIniStr) == 0)
				lstrcpy(szIniStr, DEFAULT_HELP_FILENAME);

			//	Construct the path
			if(strchr(szIniStr, ':') != NULL)
			{
				m_strHelpFileSpec = szIniStr; // Absolute path
			}
			else
			{
				m_strHelpFileSpec = theApp.GetAppFolder();
				if(m_strHelpFileSpec.Right(1) != "\\")
					m_strHelpFileSpec += "\\";
				m_strHelpFileSpec += szIniStr;
			}

		}// if(bFirstTime == TRUE)

		m_Ini.ReadString(LASTPLAYLIST_LINE, szIniStr, sizeof(szIniStr));
		m_strLastPlaylist = szIniStr;

		//	Get the keyboard hook characters
		m_Ini.ReadString(VKCHAR_LINE, szIniStr, sizeof(szIniStr));
		if(szIniStr[0] != '\0')
			m_cVKChar = toupper(szIniStr[0]);
		else
			m_cVKChar = KEYBOARD_VKCODE;

		m_Ini.ReadString(PRIMARY_BARCODE_LINE, szIniStr, sizeof(szIniStr));
		if(szIniStr[0] != '\0')
			m_cPrimaryBarcodeChar = toupper(szIniStr[0]);
		else
			m_cPrimaryBarcodeChar = KEYBOARD_PRIMARY_BARCODE;

		m_Ini.ReadString(ALTERNATE_BARCODE_LINE, szIniStr, sizeof(szIniStr));
		if(szIniStr[0] != '\0')
			m_cAlternateBarcodeChar = toupper(szIniStr[0]);
		else
			m_cAlternateBarcodeChar = KEYBOARD_ALTERNATE_BARCODE;

		//	Are we supposed to create the source documents for debugging?
		m_bCreateDocuments = m_Ini.ReadBool(CREATE_DOCUMENTS_LINE, FALSE);
	}
	else
	{
		m_strLastPlaylist.Empty();
		m_cVKChar = KEYBOARD_VKCODE;
		m_cPrimaryBarcodeChar = KEYBOARD_PRIMARY_BARCODE;
		m_cAlternateBarcodeChar = KEYBOARD_ALTERNATE_BARCODE;
	}

	//	Set the application options
	m_bScaleGraphics = Graphics.bScaleGraphics;
	m_bScaleDocs = Graphics.bScaleDocuments;
	m_bPenControls = Graphics.bLightPenEnabled;
	m_iUserSplitFrameColor = Graphics.sUserSplitFrameColor;
	m_iZapSplitFrameColor = Graphics.sZapSplitFrameColor;
	m_fPlaylistStep = Video.fPlaylistStep;
	m_bResumePlaylist = Video.bResumePlaylist;
	m_bClearPlaylist = Video.bClearPlaylist;
	m_fMovieStep = Video.fMovieStep;
	m_bResumeMovie = Video.bResumeMovie;
	m_bClearMovie = Video.bClearMovie;
	m_bScaleAVI = Video.bScaleAVI;
	m_bRunToEnd = Video.bRunToEnd;
	m_bClipsAsPlaylists = Video.bClipsAsPlaylists;
	m_bSplitScreenDocuments = Video.bSplitScreenDocuments;
	m_bSplitScreenGraphics = Video.bSplitScreenGraphics;
	m_bSplitScreenPowerPoints = Video.bSplitScreenPowerPoint;
	m_bClassicLinks = Video.bClassicLinks;
	m_sVideoSize = Video.iVideoSize;
	m_sVideoPosition = Video.iVideoPosition;
	m_dFrameRate = Video.dFrameRate;
	m_bCenterVideo = Text.bCenterVideo;
	m_bCombineText = Text.bCombineText;
	m_bSystemScrollEnabled = !Text.bDisableScrollText;
	m_bEnableErrors = Database.bEnableErrors;
	m_bEnablePowerPoint = Database.bEnablePowerPoint;
	m_iImageSecondary = System.iImageSecondary;
	m_iAnimationSecondary = System.iAnimationSecondary;
	m_iPlaylistSecondary = System.iPlaylistSecondary;
	m_iPowerPointSecondary = System.iPowerPointSecondary;
	m_iCustomShowSecondary = System.iCustomShowSecondary;
	m_iTreatmentTertiary = System.iTreatmentTertiary;
	m_bUseSecondaryMonitor = System.bDualMonitors;

	//	Check the cue steps to make sure they're valid
	if(m_fPlaylistStep <= 0) m_fPlaylistStep = 1.0f;
	if(m_fMovieStep <= 0) m_fMovieStep = 1.0f;
	
	//	Initialize the TMView control
	for(int i=0; i < SZ_ARR_TM_VW; i++) {
		m_ctrlTMView = m_arrTmView[i];

		m_ctrlTMView->SetAnnColor(Graphics.sAnnColor);
		m_ctrlTMView->SetAnnThickness(Graphics.sAnnThickness);
		m_ctrlTMView->SetHighlightColor(Graphics.sHighlightColor);
		m_ctrlTMView->SetRedactColor(Graphics.sRedactColor);
		m_ctrlTMView->SetMaxZoom(Graphics.sMaxZoom);
		m_ctrlTMView->SetCalloutColor(Graphics.sCalloutColor);
		m_ctrlTMView->SetCalloutHandleColor(Graphics.sCalloutHandleColor);
		m_ctrlTMView->SetCalloutFrameColor(Graphics.sCalloutFrameColor);
		m_ctrlTMView->SetCalloutFrameThickness(Graphics.sCalloutFrameThickness);
		m_ctrlTMView->SetSplitFrameColor(Graphics.sUserSplitFrameColor);
		m_ctrlTMView->SetBitonalScaling(Graphics.sBitonalScaling);
		m_ctrlTMView->SetAnnTool(Graphics.sAnnTool);
		m_ctrlTMView->SetAnnFontSize(Graphics.sAnnFontSize);
		m_ctrlTMView->SetAnnFontBold(Graphics.bAnnFontBold);
		m_ctrlTMView->SetAnnFontStrikeThrough(Graphics.bAnnFontStrikeThrough);
		m_ctrlTMView->SetAnnFontUnderline(Graphics.bAnnFontUnderline);
		m_ctrlTMView->SetAnnFontName(Graphics.strAnnFontName);
		m_ctrlTMView->SetPenSelectorVisible(Graphics.bLightPenEnabled);
		m_ctrlTMView->SetPenSelectorColor(Graphics.sLightPenColor);
		m_ctrlTMView->SetPenSelectorSize(Graphics.sLightPenSize);
		m_ctrlTMView->SetResizeCallouts(Graphics.bResizableCallouts);
		m_ctrlTMView->SetPanCallouts(Graphics.bPanCallouts);
		m_ctrlTMView->SetZoomCallouts(Graphics.bZoomCallouts);
		m_ctrlTMView->SetShadeOnCallout(Graphics.bShadeOnCallout);
		m_ctrlTMView->SetCalloutShadeGrayscale(Graphics.sCalloutShadeGrayscale);
	}
	m_ctrlTMView = m_arrTmView[1];

	//	Initialize the TMLPen control
	m_ctrlTMLpen.SetBackColor((OLE_COLOR)m_ctrlTMView->GetRGBColor(Graphics.sLightPenColor));
	ShowLightPen(Graphics.bLightPenEnabled);

	//	Initialize the TMText control
	m_ctrlTMText.SetUseAvgCharWidth(Text.bUseAvgCharWidth);
	m_ctrlTMText.SetShowPageLine(Text.bShowPageLine);
	m_ctrlTMText.SetShowText(Text.bShowText);
	m_ctrlTMText.SetMaxCharsPerLine(Text.sMaxCharsPerLine);
	m_ctrlTMText.SetDisplayLines(Text.sDisplayLines);
	m_ctrlTMText.SetHighlightLines(Text.sHighlightLines);
	m_ctrlTMText.SetSmoothScroll(Text.bSmoothScroll);
	m_ctrlTMText.SetScrollSteps(Text.sScrollSteps);
	m_ctrlTMText.SetScrollTime(Text.sScrollTime);
	m_ctrlTMText.SetLeftMargin(Text.sLeftMargin);
	m_ctrlTMText.SetRightMargin(Text.sRightMargin);
	m_ctrlTMText.SetTopMargin(Text.sTopMargin);
	m_ctrlTMText.SetBottomMargin(Text.sBottomMargin);
	m_ctrlTMText.SetBackColor((OLE_COLOR)Text.lBackground);
	m_ctrlTMText.SetForeColor((OLE_COLOR)Text.lForeground);
	m_ctrlTMText.SetHighlightColor((OLE_COLOR)Text.lHighlight);
	m_ctrlTMText.SetHighlightTextColor((OLE_COLOR)Text.lHighlightText);
	m_ctrlTMText.SetUseLineColor(Text.bUseManagerHighlighter);
	m_ctrlTMText.SetBulletStyle(Text.sBulletStyle);
	m_ctrlTMText.SetBulletMargin(Text.sBulletMargin);
	
	if(bFirstTime)
		m_ctrlTMText.Initialize();

	COleFont OleFont = m_ctrlTMText.GetFont();
	OleFont.SetName(Text.strTextFont);
	m_ctrlTMText.SetFont(OleFont);
	m_ctrlTMText.ResizeFont(FALSE);

	//	Initialize the TMMovie control
	m_ctrlTMMovie.SetUseSnapshots(FALSE);
	m_ctrlTMMovie.SetDefaultRate(m_dFrameRate);
	m_ctrlTMMovie.SetDetachBeforeLoad(System.bOptimizeVideo);

	//  Checking for "Optimize for Tablet" if true setting buttons' large size 
	m_bOptimizedForTablet = System.bOptimizeTablet;
	if (System.bOptimizeTablet)
	{
		m_Ini.SetSection(TMBARS_DOCUMENT_SECTION,0);
		m_Ini.WriteLong(TMTB_INI_SIZE_LINE,TMTB_LARGEBUTTONS);
		m_Ini.WriteBool(TMBARS_SHOW_LINE, TRUE);

		m_Ini.SetSection(TMBARS_GRAPHIC_SECTION,0);
		m_Ini.WriteLong(TMTB_INI_SIZE_LINE,TMTB_LARGEBUTTONS);
		m_Ini.WriteBool(TMBARS_SHOW_LINE,TRUE);

		m_Ini.SetSection(TMBARS_PLAYLIST_SECTION,0);
		m_Ini.WriteLong(TMTB_INI_SIZE_LINE,TMTB_LARGEBUTTONS);
		m_Ini.WriteBool(TMBARS_SHOW_LINE,TRUE);

		m_Ini.SetSection(TMBARS_LINK_SECTION,0);
		m_Ini.WriteLong(TMTB_INI_SIZE_LINE,TMTB_LARGEBUTTONS);
		m_Ini.WriteBool(TMBARS_SHOW_LINE,TRUE);

		m_Ini.SetSection(TMBARS_MOVIE_SECTION,0);
		m_Ini.WriteLong(TMTB_INI_SIZE_LINE,TMTB_LARGEBUTTONS);
		m_Ini.WriteBool(TMBARS_SHOW_LINE,TRUE);

		m_Ini.SetSection(TMBARS_POWERPOINT_SECTION,0);
		m_Ini.WriteLong(TMTB_INI_SIZE_LINE,TMTB_LARGEBUTTONS);
		m_Ini.WriteBool(TMBARS_SHOW_LINE,TRUE);

		// enable gesture handling
		m_bTabletMode = TRUE;
	}

	if(bFirstTime)
	{
		m_ctrlTMMovie.SetIniFile(m_Ini.strFileSpec);
		m_ctrlTMMovie.Initialize();
		m_ctrlTMMovie.ShowWindow(FALSE);
		m_ctrlTMMovie.ShowVideo(FALSE);
	}

	//	Initialize the TMGrab control
	m_ctrlTMGrab.SetArea(Capture.sArea);
	m_ctrlTMGrab.SetHotkey(Capture.sHotkey);
	m_ctrlTMGrab.SetCancelKey(Capture.sCancelKey);
	m_ctrlTMGrab.SetSilent(Capture.bSilent);

	if(bFirstTime)
		m_ctrlTMGrab.Initialize();

	//	Initialize the error handlers. We disable the default database handler
	//	so that the application can handle database errors on its own
	m_ctrlTMMovie.SetEnableErrors(m_bEnableErrors);
	m_ctrlTMView->SetEnableErrors(m_bEnableErrors);
	m_ctrlTMText.SetEnableErrors(m_bEnableErrors);
	m_ctrlTMStat.SetEnableErrors(m_bEnableErrors);
	m_ctrlTMLpen.SetEnableErrors(m_bEnableErrors);
	m_ctrlTMPower.SetEnableErrors(m_bEnableErrors);
	m_ctrlTMGrab.SetEnableErrors(m_bEnableErrors);

	if(bFirstTime)
	{
		m_Errors.SetParent(m_hWnd);
		m_Errors.Enable(m_bEnableErrors);
		m_Errors.SetTitle("TrialMax Error");
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::ReadTestSetup()
//
// 	Description:	This function will read the test mode parameters from the
//					active ini file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::ReadTestSetup() 
{
	char szIniStr[512];

	//	Get the test mode options from the ini file
	m_Ini.SetSection(TMAX_TEST_SECTION);
	m_Test.bImages = m_Ini.ReadBool(TEST_IMAGES_LINE, TESTMODE_IMAGES);
	m_Test.bPlaylists = m_Ini.ReadBool(TEST_PLAYLISTS_LINE, TESTMODE_PLAYLISTS);
	m_Test.bMovies = m_Ini.ReadBool(TEST_MOVIES_LINE, TESTMODE_MOVIES);
	m_Test.bPowerPoints = m_Ini.ReadBool(TEST_POWERPOINTS_LINE, TESTMODE_POWERPOINTS);
	m_Test.bShows = m_Ini.ReadBool(TEST_SHOWS_LINE, TESTMODE_IMAGES);
	m_Test.bPages = m_Ini.ReadBool(TEST_PAGES_LINE, TESTMODE_PAGES);
	m_Test.bTreatments = m_Ini.ReadBool(TEST_TREATMENTS_LINE, TESTMODE_TREATMENTS);
	m_bDisableLinks = !m_Ini.ReadBool(TEST_LINKS_LINE, TESTMODE_LINKS);
	m_Test.lPlaylistPeriod = m_Ini.ReadLong(TEST_PLAYLISTPERIOD_LINE, TESTMODE_PLAYLISTPERIOD);
	m_Test.lMediaPeriod = m_Ini.ReadLong(TEST_MEDIAPERIOD_LINE, TESTMODE_MEDIAPERIOD);
	m_Test.lMoviePeriod = m_Ini.ReadLong(TEST_MOVIEPERIOD_LINE, TESTMODE_MOVIEPERIOD);
	m_Ini.ReadString(TEST_ACTIVITYLOG_LINE, szIniStr, sizeof(szIniStr), TESTMODE_ACTIVITYLOG);
	if(lstrlen(szIniStr) == 0)
		lstrcpy(szIniStr, TESTMODE_ACTIVITYLOG);
			
	//	Assemble the full path specification for the error log
	if(strchr(szIniStr, '\\') == NULL)
	{
		if(m_strAppFolder.Right(1) != "\\")
			sprintf_s(m_Test.szLogFile, sizeof(m_Test.szLogFile), "%s\\%s", m_strAppFolder, szIniStr);
		else
			sprintf_s(m_Test.szLogFile, sizeof(m_Test.szLogFile), "%s%s", m_strAppFolder, szIniStr);
	}
	else
	{
		lstrcpy(m_Test.szLogFile, szIniStr);
	}

	if(m_Ini.ReadBool(TEST_STATUSBAR_LINE, TESTMODE_STATUSBAR))
		m_ControlBar.iId = CONTROL_BAR_STATUS;

	//	Set the default options
	m_bClipsAsPlaylists = FALSE;
}

//==============================================================================
//
// 	Function Name:	CMainView::ReadToolbar()
//
// 	Description:	This function is called to read the toolbar information
//					from the ini file.
//
// 	Returns:		None
//
//	Notes:			This function assumes the ini file has already been set
//					to the appropriate section.
//
//==============================================================================
void CMainView::ReadToolbar(SToolbar* pToolbar, LPSTR lpSection, LPSTR lpszMaster)
{
	ASSERT(pToolbar);
	ASSERT(lpSection);

	//	Set the ini file to the correct section
	m_Ini.SetSection(lpSection);

	//	Should we attempt to retrieve the UseMaster option from the ini file?
	if((lpszMaster != NULL) && (lstrlen(lpszMaster) > 0))
	{
		if(m_Ini.ReadBool(TMBARS_USE_MASTER_LINE, DEFAULT_TMBARS_USE_MASTER) == TRUE)
		{
			//	Switch to the master section
			m_Ini.SetSection(lpszMaster);
		}

	}// if((lpszMaster != NULL) && (lstrlen(lpszMaster) > 0))

	//	Get the toolbar flags
	pToolbar->bShow    = m_Ini.ReadBool(TMBARS_SHOW_LINE, DEFAULT_TMBARS_SHOW); 
	pToolbar->bFlat    = m_Ini.ReadBool(TMBARS_FLAT_LINE, DEFAULT_TMBARS_FLAT); 
	pToolbar->bStretch = m_Ini.ReadBool(TMBARS_STRETCH_LINE, DEFAULT_TMBARS_STRETCH); 
	pToolbar->bDock    = m_Ini.ReadBool(TMBARS_DOCK_LINE, DEFAULT_TMBARS_DOCK); 
	pToolbar->sSize	= (short)m_Ini.ReadLong(TMBARS_SIZE_LINE, DEFAULT_TMBARS_SIZE);

	//	Now construct the button map
	for(int i = 0; i < TMTB_MAXBUTTONS; i++)
		pToolbar->aMap[i] = (short)m_Ini.ReadLong(i, -1);
}

//==============================================================================
//
// 	Function Name:	CMainView::RecalcLayout()
//
// 	Description:	This function will recalculate the rectangles used to
//					position the controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::RecalcLayout(short sState)   
{
	int		iCx;
	int		iWidth;
	int		iBarHeight;
	RECT*	pViewer;
	SIZE	videoSize;

	ASSERT(IsWindow(m_ctrlTMView->m_hWnd));
	ASSERT(IsWindow(m_ctrlTMMovie.m_hWnd));
	ASSERT(IsWindow(m_ctrlTMText.m_hWnd));
	ASSERT(IsWindow(m_ctrlTMPower.m_hWnd));
	if(!IsWindow(m_ctrlTMView->m_hWnd) ||
	   !IsWindow(m_ctrlTMMovie.m_hWnd) ||
	   !IsWindow(m_ctrlTMText.m_hWnd) ||
	   !IsWindow(m_ctrlTMPower.m_hWnd))
		return;

	//	Is the control bar docked and visible?
	if((m_ControlBar.pWnd != 0) && (m_ControlBar.bDocked == TRUE))
	{
		iBarHeight = m_ControlBar.iHeight;
	}
	else
	{
		iBarHeight = 0;
	}

	//	What is the current state?
	switch(sState)
	{
		case S_CLEAR:

			ZeroMemory(&m_rcView, sizeof(m_rcView));
			ZeroMemory(&m_rcMovie, sizeof(m_rcMovie));
			ZeroMemory(&m_rcText, sizeof(m_rcText));
			ZeroMemory(&m_rcPower, sizeof(m_rcPower));
			break;

		case S_DOCUMENT:
		case S_GRAPHIC:
			
			m_rcView.left   = 0;
			m_rcView.top    = 0;
			m_rcView.right  = m_iMaxWidth;
			m_rcView.bottom = m_iMaxHeight - iBarHeight;

			ZeroMemory(&m_rcMovie, sizeof(m_rcMovie));
			ZeroMemory(&m_rcText, sizeof(m_rcText));
			ZeroMemory(&m_rcPower, sizeof(m_rcPower));

			break;
							
		case S_POWERPOINT:
			
			m_rcPower.left   = 0;
			m_rcPower.top    = 0;
			m_rcPower.right  = m_iMaxWidth;
			m_rcPower.bottom = m_iMaxHeight - iBarHeight;

			ZeroMemory(&m_rcMovie, sizeof(m_rcMovie));
			ZeroMemory(&m_rcText, sizeof(m_rcText));
			ZeroMemory(&m_rcView, sizeof(m_rcView));

			break;
							
		case S_PLAYLIST:
		case S_MOVIE:
			
			//	Is this a hidden link with no video?
			if((sState == S_PLAYLIST) && (m_AppLink.GetHideLink() == TRUE) && (m_AppLink.GetHideVideo() == TRUE))
			{
				ZeroMemory(&m_rcMovie, sizeof(m_rcMovie));
			}
			else
			{
				m_rcMovie.left   = 0;
				m_rcMovie.top    = 0;
				m_rcMovie.right  = m_iMaxWidth;
				m_rcMovie.bottom = m_iMaxHeight - iBarHeight;
			}

			ZeroMemory(&m_rcView, sizeof(m_rcView));
			ZeroMemory(&m_rcText, sizeof(m_rcText));
			ZeroMemory(&m_rcPower, sizeof(m_rcPower));
			break;
							
		case S_LINKEDIMAGE:	
		case S_LINKEDPOWER:
			
			//	Set the bounds for the scrolling text control
			m_rcText.left   = 0;
			m_rcText.bottom = m_iMaxHeight - iBarHeight;

			//	Should scrolling text be visible?
			if(GetLinkedTextEnabled() == TRUE)
			{
				m_rcText.right = m_iMaxWidth;
				m_rcText.top   = m_rcText.bottom - m_ctrlTMText.GetMinHeight();
			}
			else
			{
				//	This sizes the rectangle to zero but allows us to use the
				//	coordinates to calculate the video and viewer rectangles
				m_rcText.right = m_rcText.left;
				m_rcText.top   = m_rcText.bottom;
			}

			//	What size video is being used?
			GetLinkedVideoSize(videoSize);

			//	What is the position?
			switch(m_sVideoPosition)
			{
				case VIDEO_UPPERRIGHT:

					m_rcMovie.top    = 0;
					m_rcMovie.right  = m_iMaxWidth;
					m_rcMovie.left   = m_rcMovie.right - videoSize.cx;
					m_rcMovie.bottom = m_rcMovie.top + videoSize.cy;
					break;

				case VIDEO_LOWERRIGHT:

					m_rcMovie.right  = m_iMaxWidth;
					m_rcMovie.left   = m_rcMovie.right - videoSize.cx;
					m_rcMovie.bottom = m_rcText.top;
					m_rcMovie.top    = m_rcMovie.bottom - videoSize.cy;
					break;

				case VIDEO_LOWERLEFT:

					m_rcMovie.left   = 0;
					m_rcMovie.right  = m_rcMovie.left + videoSize.cx;
					m_rcMovie.bottom = m_rcText.top;
					m_rcMovie.top    = m_rcMovie.bottom - videoSize.cy;
					break;

				case VIDEO_UPPERLEFT:
				default:

					m_rcMovie.left   = 0;
					m_rcMovie.right  = m_rcMovie.left + videoSize.cx;
					m_rcMovie.top    = 0;
					m_rcMovie.bottom = m_rcMovie.top + videoSize.cy;
					break;
			}

			//	Which viewer do we want to use?
			if(sState == S_LINKEDPOWER)
			{
				pViewer = &m_rcPower;
				ZeroMemory(&m_rcView, sizeof(m_rcView));
			}
			else
			{
				pViewer = &m_rcView;
				ZeroMemory(&m_rcPower, sizeof(m_rcPower));
			}

			//	Now size the viewer
			pViewer->top    = 0;
			pViewer->bottom = m_rcText.top;

			if(m_AppLink.GetSplitScreen() == TRUE)
			{
				//	Is the video on the right?
				if((m_sVideoPosition == VIDEO_UPPERRIGHT) ||
				   (m_sVideoPosition == VIDEO_LOWERRIGHT))
				{
					pViewer->left   = 0;
					pViewer->right  = m_rcMovie.left;
				}
				else
				{
					pViewer->left   = m_rcMovie.right;
					pViewer->right  = m_iMaxWidth;
				}

				//	We're displaying split screen link
				m_bSplitScreenLink = TRUE;
			}
			else
			{
				pViewer->left  = 0;
				pViewer->right = m_iMaxWidth;
				m_bSplitScreenLink = FALSE;
			}
						
			break;

		case S_TEXT:
			
			m_rcText.left   = 0;
			m_rcText.right  = m_iMaxWidth;
			m_rcText.bottom = m_iMaxHeight - iBarHeight;
			m_rcText.top    = m_rcText.bottom - m_ctrlTMText.GetMinHeight();

			//	Is this a hidden link with no video?
			if((m_AppLink.GetHideLink() == TRUE) && (m_AppLink.GetHideVideo() == TRUE))
			{
				ZeroMemory(&m_rcMovie, sizeof(m_rcMovie));
			}
			else
			{
				//	Are we centering the video in the remaining space?
				if(m_bCenterVideo)
				{
					m_rcMovie.top = 0;
					m_rcMovie.bottom = m_rcText.top - 1; // Subract 1 to prevent overlap

					//	Get the center point of the window and the width of the video
					iCx = m_iMaxWidth / 2;
					iWidth = m_rcMovie.bottom * 4 / 3;

					m_rcMovie.left = iCx - (iWidth / 2);
					m_rcMovie.right = m_rcMovie.left + iWidth;
				}
				else
				{
					m_rcMovie.top = 0;
					m_rcMovie.bottom = m_rcText.top;
					m_rcMovie.left = 0;
					m_rcMovie.right = m_rcMovie.bottom * 4 / 3;
				}

			}//	if(m_AppLink.GetHideLink() == TRUE && (m_AppLink.GetHideVideo() == TRUE))

			ZeroMemory(&m_rcView, sizeof(m_rcView));
			ZeroMemory(&m_rcPower, sizeof(m_rcPower));
			break;
							
	}//	switch(m_sState)

	//	Get the new coordinates for the status bar
	m_rcStatus.left   = 0;
	m_rcStatus.right  = m_iMaxWidth;
	m_rcStatus.bottom = m_iMaxHeight;
	if(m_ControlBar.iHeight > 0)
		m_rcStatus.top = m_iMaxHeight - m_ControlBar.iHeight;
	else
		m_rcStatus.top = m_iMaxHeight - m_ctrlTBGraphics.GetBarHeight();

	//	Resize the window if it's visible
	if(IsWindow(m_ctrlTMStat.m_hWnd) && m_ctrlTMStat.IsWindowVisible())
		m_ctrlTMStat.MoveWindow(&m_rcStatus);

	//	Make sure the light pen control is correctly sized
	ShowLightPen(m_bPenControls);

	// These scroll size setting will remove the scroll bar from the presentation
	// The scroll bar will only come in 16:9 resolution like (1920x1080), (1280,1024), etc.
	// it is not occured in every resolution
	SIZE sz;
	sz.cx = 1;
	sz.cy = 1;
	this->SetScrollSizes(MM_LOMETRIC, sz);
}

//==============================================================================
//
// 	Function Name:	CMainView::ResetMultipage()
//
// 	Description:	This function will reset the multipage information structure
//					specified by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::ResetMultipage(SMultipageInfo* pInfo)
{
	if(pInfo)
	{
		//	Deallocate the multipage object if it exists
		if(pInfo->pMultipage)
		{
			DbgMsg(pInfo, "ResetMultipage");
			delete pInfo->pMultipage;
		}

		ZeroMemory(pInfo, sizeof(SMultipageInfo));
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::ResetShowInfo()
//
// 	Description:	This function is called to reset the persistant custom
//					show information.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::ResetShowInfo()   
{
	if(m_Show.pShow)
		delete m_Show.pShow;
	ZeroMemory(&m_Show, sizeof(m_Show));	
}

//==============================================================================
//
// 	Function Name:	CMainView::ResetTMMovie()
//
// 	Description:	This function will reset the TMMovie and associated controls
//					to their default states.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::ResetTMMovie(BOOL bAnimation, BOOL bPlaylist)   
{
	//	Stop any active playback
	m_ctrlTMMovie.Stop();
	m_ctrlTMMovie.Unload();
	m_ctrlTMMovie.SetOverlayVisible(FALSE);
	m_ctrlTMMovie.SetOverlayFile("");
	
	//	Are we resetting an animation?
	if(bAnimation)
		ResetMultipage(&m_TMMovie);
	
	//	Are we resetting a playlist?
	if(bPlaylist)
	{
		//	Reset the playlist status information
		ZeroMemory(&m_PlaylistStatus, sizeof(m_PlaylistStatus));

		//	Detach the playlist from the controls
		m_ctrlTMMovie.PlayPlaylist(0,0,0,0);
		m_ctrlTMText.SetPlaylist(0, FALSE);
		UpdateStatusBar();

		//	Delete the playlist if it exists
		if(m_Playlist.pPlaylist)
			delete m_Playlist.pPlaylist;
		memset(&m_Playlist, 0, sizeof(m_Playlist));
	}

	//	Clear the link information
	m_AppLink.Clear();
}

//==============================================================================
//
// 	Function Name:	CMainView::ResetTMPower()
//
// 	Description:	This function will reset the TMPower control to its initial
//					state.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::ResetTMPower()   
{
	//	Unload the viewer
	m_ctrlTMPower.Unload(TMPOWER_ACTIVEVIEW);

	//	Deallocate the multipage objects
	ResetMultipage(&m_TMPower1);
	ResetMultipage(&m_TMPower2);
}

//==============================================================================
//
// 	Function Name:	CMainView::ResetTMView()
//
// 	Description:	This function will reset the TMView control to its initial
//					state.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::ResetTMView()   
{
	//	Unload the current images
	m_ctrlTMView->LoadFile(0, TMV_LEFTPANE);
	m_ctrlTMView->LoadFile(0, TMV_RIGHTPANE);
	m_ctrlTMView->SetSplitScreen(FALSE);
	SetZapSplitScreen(FALSE);
	
	//	Deallocate the multipage objects
	for(vector<SMultipageInfo *>::iterator it = m_arrMultiPageInfo.begin();
			it != m_arrMultiPageInfo.end(); it++) {
				ResetMultipage(*it);
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::RestoreDisplay()
//
// 	Description:	This function uses the state identifier provided by the 
//					caller to set the appropriate visibility, size, and position
//					of the TMView and TMMovie controls.
//
// 	Returns:		None
//
//	Notes:			We can not hide the TMMovie control just by turning off its
//					visibility because the mpeg board will still show through
//					any part of the screen with the chroma color. To hide it
//					we size it to zero.
//
//==============================================================================
void CMainView::RestoreDisplay()   
{

	//	What is the desired state?
	//AfxMessageBox("restore display");
	switch(m_sPrevState)
	{
		case S_CLEAR:
			
			break;
		
		case S_DOCUMENT:
		case S_GRAPHIC:

			for(int i =0; i < SZ_ARR_TM_VW; i++) {

				m_arrTmView[i]->ShowWindow(SW_SHOW);
				m_arrTmView[i]->BringWindowToTop();
				if(m_arrTmView[i]->GetSplitScreen() == TRUE)
				{
					m_arrTmView[i]->ShowCallouts(TRUE, TMV_LEFTPANE);
					m_arrTmView[i]->ShowCallouts(TRUE, TMV_RIGHTPANE);
				}
				else
				{
					m_arrTmView[i]->ShowCallouts(TRUE, TMV_ACTIVEPANE);
				}
			}
			m_ctrlTMStat.SetMode(TMSTAT_TEXTMODE);

			/*
			if(m_pVKBDlg) {
				delete m_pVKBDlg;
			}
			m_pVKBDlg = new CVKBDlg(&m_ctrlTMView);
			m_pVKBDlgPtr = m_pVKBDlg;*/
			break;
		
		case S_POWERPOINT:

			m_ctrlTMPower.Show(TRUE);
			m_ctrlTMPower.BringWindowToTop();
			m_ctrlTMStat.SetMode(TMSTAT_TEXTMODE);
			m_pVKBDlg->ShowWindow(SW_SHOWNORMAL);

			/*
			if(m_pVKBDlg) {
				delete m_pVKBDlg;
			}
			m_pVKBDlg = new CVKBDlg(&m_ctrlTMPower);
			m_pVKBDlgPtr = m_pVKBDlg;*/
			break;
		
		case S_LINKEDIMAGE:
		case S_LINKEDPOWER:

			//	Should the text control be visible?
			if(GetLinkedTextEnabled() == TRUE)
				m_ctrlTMText.ShowWindow(SW_SHOW);

			if(m_sPrevState == S_LINKEDIMAGE)
			{		
				for(int i =0; i < SZ_ARR_TM_VW; i++) {
					m_arrTmView[i]->ShowWindow(SW_SHOW);
					m_arrTmView[i]->ShowCallouts(TRUE, TMV_ACTIVEPANE);
				}
			}
			else
			{
				m_ctrlTMPower.Show(TRUE);
			}

			//	Drop through from here to display the video
			//						.
			//						.
			//						.
		
		case S_PLAYLIST: 
		case S_MOVIE:
			
			m_ctrlTMMovie.ShowVideo(TRUE);
			m_ctrlTMMovie.ShowWindow(SW_SHOW);
			m_ctrlTMMovie.RedrawWindow();
			m_ctrlTMMovie.BringWindowToTop();
			m_ctrlTMStat.SetMode((m_Playlist.pPlaylist == 0) ? TMSTAT_TEXTMODE :
															   TMSTAT_PLAYLISTMODE);

			//	We have to do this because some drivers do not properly draw the
			//	video if the loaded file does not change
			m_ctrlTMMovie.Invalidate();
			/*
			if(m_pVKBDlg) {
				delete m_pVKBDlg;
			}
			m_pVKBDlg = new CVKBDlg(&m_ctrlTMMovie);
			m_pVKBDlgPtr = m_pVKBDlg;*/
			break;

		case S_TEXT:

			m_ctrlTMMovie.ShowVideo(TRUE);
			m_ctrlTMMovie.ShowWindow(SW_SHOW);
			m_ctrlTMMovie.RedrawWindow();
			m_ctrlTMText.ShowWindow(SW_SHOW);
			m_ctrlTMText.RedrawWindow();
			m_ctrlTMStat.SetMode(TMSTAT_PLAYLISTMODE);
			
			//	We have to do this because some drivers do not properly draw the
			//	video if the loaded file does not change
			m_ctrlTMMovie.Invalidate();
			/*
			if(m_pVKBDlg) {
				delete m_pVKBDlg;
			}
			m_pVKBDlg = new CVKBDlg(&m_ctrlTMMovie);
			m_pVKBDlgPtr = m_pVKBDlg;*/
			break;
	}

	//	Set the control bar state
	SetControlBar(m_ControlBar.iId);

	//	Save the new state and previous state
	m_sState = m_sPrevState;

	//	Grab the keyboard focus
	//::SetFocus(m_hWnd);

	/*m_pVKBDlg->Create(CVKBDlg::IDD);
	CRect bmpRect;
	m_pVKBDlg->GetClientRect(&bmpRect);
	m_pVKBDlg->MoveWindow(m_ScreenResolution.right - bmpRect.right - kbIconPadding ,  kbIconPadding , bmpRect.right , bmpRect.bottom );

	m_pVKBDlg->ShowWindow(SW_SHOW);
	m_pVKBDlgPtr = m_pVKBDlg;*/
	
}

//==============================================================================
//
// 	Function Name:	CMainView::SaveSplitZap()
//
// 	Description:	This function will save the current split screen state to
//					a split screen zap file
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::SaveSplitZap() 
{
	SMultipageInfo*	pTLInfo = (SMultipageInfo*)m_ctrlTMView->GetData(TMV_LEFTPANE);
	SMultipageInfo*	pBRInfo = (SMultipageInfo*)m_ctrlTMView->GetData(TMV_RIGHTPANE);
	CString			strTLFilename = "";
	CString			strBRFilename = "";
	DWORD			dwFlags = 0;
	BOOL			bSuccessful = FALSE;

	//	Just in case ....
	ASSERT_RET_BOOL(m_pDatabase != NULL);
	ASSERT_RET_BOOL(pTLInfo != NULL);
	ASSERT_RET_BOOL(pTLInfo->pMultipage != NULL);
	ASSERT_RET_BOOL(pTLInfo->pSecondary != NULL);
	ASSERT_RET_BOOL(pBRInfo != NULL);
	ASSERT_RET_BOOL(pBRInfo->pMultipage != NULL);
	ASSERT_RET_BOOL(pBRInfo->pSecondary != NULL);

	//	We can only zap images
	if(pTLInfo->pMultipage->m_lPlayerType != MEDIA_TYPE_IMAGE) return FALSE;
	if(pBRInfo->pMultipage->m_lPlayerType != MEDIA_TYPE_IMAGE) return FALSE;

	//	Set the appropriate flags
	if(m_ctrlTMView->GetSplitHorizontal())
		dwFlags |= TMFLAG_SPLIT_ZAP_HORIZONTAL;

	// Get the file specifications
	m_pDatabase->GetSaveZapFileSpecs(pTLInfo->pMultipage, pTLInfo->pSecondary, strTLFilename,
									 pBRInfo->pMultipage, pBRInfo->pSecondary, strBRFilename,
									 dwFlags);
	
	ASSERT(strTLFilename.GetLength() > 0);
	if(strTLFilename.GetLength() == 0) return FALSE;
	ASSERT(strBRFilename.GetLength() > 0);
	if(strBRFilename.GetLength() == 0) return FALSE;
		
	//	First save the zap file for the right-hand pane
	if(m_ctrlTMView->SaveZap(strBRFilename, TMV_RIGHTPANE) == TMV_NOERROR)
	{
		//	Now save the zap file for the left pane
		if(m_ctrlTMView->SaveZap(strTLFilename, TMV_LEFTPANE) == TMV_NOERROR)
		{
			bSuccessful = TRUE;

			//	Is the manager running
			if(m_ctrlManagerApp.IsRunning() == TRUE)
			{
				//	Format a request to add the new treatment
				m_ctrlManagerApp.SetCaseFolder(m_strCaseFolder);
				m_ctrlManagerApp.SetSourceFilePath(strTLFilename);
				m_ctrlManagerApp.SetCommand(TMSHARE_COMMAND_ADD_TREATMENT);
				m_ctrlManagerApp.SetRequest(0);
			}

		}
		else
		{
			//	Delete the file we created for the right hand pane
			_unlink(strBRFilename);

		}// // if(m_ctrlTMView->SaveZap(strBRFilename, TMV_RIGHTPANE) == TMV_NOERROR)

	}// if(m_ctrlTMView->SaveZap(strBRFilename, TMV_RIGHTPANE) == TMV_NOERROR)

	return bSuccessful;
}

//==============================================================================
//
// 	Function Name:	CMainView::SaveZap()
//
// 	Description:	This function will save a dynamic annotations file for the
//					current image.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::SaveZap() 
{
	SMultipageInfo*	pInfo = GetMultipageInfo(m_sState);
	CString			strFilename;
	BOOL			bSuccessful = FALSE;

	//	Just in case ....
	ASSERT_RET_BOOL(m_pDatabase != NULL);
	ASSERT_RET_BOOL(pInfo != NULL);
	ASSERT_RET_BOOL(pInfo->pMultipage != NULL);
	ASSERT_RET_BOOL(pInfo->pSecondary != NULL);

	//	We can only zap images
	if(pInfo->pMultipage->m_lPlayerType != MEDIA_TYPE_IMAGE)
		return FALSE;

	// Get the file specification
	m_pDatabase->GetSaveZapFileSpec(pInfo->pMultipage, pInfo->pSecondary, strFilename);
	if(strFilename.GetLength() == 0) return FALSE;
		
	//	Now save the actual zap file
	//
	//	NOTE:	TMView will report any errors if reporting is turned on
	if(m_ctrlTMView->SaveZap(strFilename, TMV_ACTIVEPANE) == TMV_NOERROR)
	{
		bSuccessful = TRUE;

		//	Is the manager running
		if(m_ctrlManagerApp.IsRunning() == TRUE)
		{
			//	Format a request to add the new treatment
			m_ctrlManagerApp.SetCaseFolder(m_strCaseFolder);
			m_ctrlManagerApp.SetSourceFilePath(strFilename);
			m_ctrlManagerApp.SetCommand(TMSHARE_COMMAND_ADD_TREATMENT);
			m_ctrlManagerApp.SetRequest(0);
		}

	}

	return bSuccessful;
}

//==============================================================================
//
// 	Function Name:	CMainView::SelectToolbar()
//
// 	Description:	This function will set the toolbar properties to those
//					associated with the state specified by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::SelectToolbar(short sState)   
{
	BOOL bSetProperties = TRUE;

	ASSERT(sState >= 0);
	ASSERT(sState < MAX_STATES);

	//	Make sure the current toolbar is hidden
	if(m_pToolbar)
		m_pToolbar->ShowWindow(SW_HIDE);

	//	Make sure the drawing tools toolbar is hidden
	if(m_ctrlTBTools.IsWindowVisible())
		m_ctrlTBTools.ShowWindow(SW_HIDE);

	//	Get the toolbar for the specified state
 	if((m_pToolbar = m_aToolbars[sState].pControl) != 0)
	{
		//	Use the height of the active toolbar to control the
		//	height of the control bar 
		m_ControlBar.iHeight = m_pToolbar->GetBarHeight();
	}
	else
	{
		m_ControlBar.iHeight = m_ctrlTBGraphics.GetBarHeight();
	}

	//	What is the desired state?
	switch(sState)
	{
		case S_CLEAR:
			
			m_ControlBar.bDocked = FALSE;
			m_ControlBar.iId = CONTROL_BAR_NONE;
			bSetProperties = FALSE;
			break;
		
		case S_DOCUMENT:
		case S_GRAPHIC:

			//	What is the current state?
			switch(m_sState)
			{
				case S_DOCUMENT:
				case S_GRAPHIC:

					//	If we are currently using split screen we do not want
					//	to change the toolbar properties
					if(!m_ctrlTMView->GetSplitScreen())
						bSetProperties = FALSE;
					break;
		
				case S_CLEAR:
				case S_LINKEDIMAGE:
				case S_PLAYLIST: 
				case S_MOVIE:
				case S_TEXT:
				case S_POWERPOINT:
				case S_LINKEDPOWER:
			
					break;
			}
			break;
		
		case S_LINKEDIMAGE:
		case S_PLAYLIST: 
		case S_MOVIE:
		case S_TEXT:
		case S_POWERPOINT:
		case S_LINKEDPOWER:
			
			break;
	}

	//	Should we update the properties using the user defined values?
	if(bSetProperties)
	{
		//	Don't switch to the toolbar if the status bar is in use
		if(m_ControlBar.iId != CONTROL_BAR_STATUS)
		{
			if(m_aToolbars[sState].bShow)
			{
				m_ControlBar.iId = CONTROL_BAR_TOOLS;
			}
			else
			{
				m_ControlBar.iId = CONTROL_BAR_NONE;
			}
		}
		m_ControlBar.bDocked = m_aToolbars[sState].bDock;
	}

	//	Set the toolbar button images
	if(m_pToolbar)
	{
		m_pToolbar->SetPlayButton(m_bPlaying);
		m_pToolbar->SetSplitButton(m_ctrlTMView->GetSplitScreen(), m_ctrlTMView->GetSplitHorizontal());
		m_pToolbar->SetLinkButton(m_bDisableLinks);
		m_pToolbar->SetZoomButton(m_ctrlTMView->GetAction() == ZOOM,
								  m_ctrlTMView->GetZoomToRect());
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::SetControlBar()
//
// 	Description:	This function will activate the specified control bar
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::SetControlBar(int iId)   
{
	CWnd*	pOldWnd = m_ControlBar.pWnd;
	CWnd*	pOldWndExtra = m_ControlBarExtra.pWnd;
	BOOL	bRecalc = FALSE;

	//	Get the desired control bar window
	if (iId == CONTROL_BAR_SHOW_LARGE)
		m_ControlBarExtra.iId = iId;
	else if (iId == CONTROL_BAR_HIDE_LARGE)
		m_ControlBarExtra.iId = iId;
	else 
	{
		m_ControlBar.iId = iId;
		m_ControlBarExtra.iId = 0;
	}

	switch(iId)
	{
		case CONTROL_BAR_TOOLS:

			m_ControlBar.pWnd = m_pToolbar;
			if(IsWindow(m_ctrlTMStat.m_hWnd))
				m_ctrlTMStat.MoveWindow(0,0,1,1);
			break;

		case CONTROL_BAR_STATUS:
			m_ControlBar.pWnd = &m_ctrlTMStat;
			//	Make sure the status bar is properly sized
			if(IsWindow(m_ctrlTMStat.m_hWnd))
				m_ctrlTMStat.MoveWindow(&m_rcStatus);
			break;

		case CONTROL_BAR_SHOW_LARGE:
			m_ControlBar.pWnd = m_pToolbar;
			m_ControlBarExtra = m_ControlBar;
			m_ControlBarExtra.iId = CONTROL_BAR_SHOW_LARGE;
			m_ControlBarExtra.pWnd =  m_aToolbars[S_DOCUMENT_LARGE].pControl;
			if(IsWindow(m_ctrlTMStat.m_hWnd))
				m_ctrlTMStat.MoveWindow(0,0,1,1);
			break;

		case CONTROL_BAR_HIDE_LARGE:
			m_ControlBarExtra.pWnd = 0;
			break;
		
		case CONTROL_BAR_NONE:
		default:
			
			m_ControlBar.pWnd = 0;
			m_ControlBarExtra.pWnd = 0;
			//	Make sure the status bar remains invisible
			if(IsWindow(m_ctrlTMStat.m_hWnd))
				m_ctrlTMStat.MoveWindow(0,0,1,1);
			break;
	}

	//	Do we need to recalculate the screen layout?
	if(m_ControlBar.bDocked && ((m_ControlBar.pWnd == 0) || (pOldWnd == 0)))
	{
		RecalcLayout(m_sState);

		//	What is the current screen state?
		switch(m_sState)
		{
			case S_DOCUMENT:
			case S_GRAPHIC:

				m_ctrlTMView->MoveWindow(&m_rcView);
				m_ctrlTMView->RescaleZapCallouts();
				m_ctrlTMView->BringWindowToTop();
				break;
			
			case S_POWERPOINT:

				m_ctrlTMPower.MoveWindow(&m_rcPower);
				m_ctrlTMPower.BringWindowToTop();
				break;
			
			case S_MOVIE:
			case S_PLAYLIST: 

				m_ctrlTMMovie.MoveWindow(&m_rcMovie);
				m_ctrlTMMovie.BringWindowToTop();
				break;
			
			case S_TEXT:
				
				m_ctrlTMMovie.MoveWindow(&m_rcMovie);
				m_ctrlTMText.MoveWindow(&m_rcText);
				m_ctrlTMMovie.BringWindowToTop();
				m_ctrlTMText.BringWindowToTop();
				break;
			
			case S_LINKEDIMAGE:
				
				m_ctrlTMText.MoveWindow(&m_rcText);
				m_ctrlTMView->MoveWindow(&m_rcView);
				m_ctrlTMView->RescaleZapCallouts();
				if((m_sVideoPosition == VIDEO_LOWERLEFT) ||
				   (m_sVideoPosition == VIDEO_LOWERRIGHT))
					m_ctrlTMMovie.MoveWindow(&m_rcMovie);
				m_ctrlTMMovie.BringWindowToTop();
				break;
			
			case S_LINKEDPOWER:
				
				m_ctrlTMText.MoveWindow(&m_rcText);
				m_ctrlTMPower.MoveWindow(&m_rcPower);
				if((m_sVideoPosition == VIDEO_LOWERLEFT) ||
				   (m_sVideoPosition == VIDEO_LOWERRIGHT))
					m_ctrlTMMovie.MoveWindow(&m_rcMovie);
				m_ctrlTMMovie.BringWindowToTop();
				break;
			
			case S_CLEAR:
			default:
				break;
		}
	}


	//	Bring the new control bar to the top 
	if(m_ControlBar.pWnd != 0)
	{
		if (m_ControlBarExtra.pWnd != 0)
		{
			m_ControlBar.pWnd->ShowWindow(SW_SHOW);
			m_ControlBarExtra.pWnd->ShowWindow(SW_SHOW); 
			m_ControlBar.pWnd->BringWindowToTop();
			m_ControlBarExtra.pWnd->BringWindowToTop();
		}
		else 
		{
			m_ControlBar.pWnd->ShowWindow(SW_SHOW);
			m_ControlBar.pWnd->BringWindowToTop();
		}

	}


	//	Hide the current bar
	if (m_ControlBar.iId == CONTROL_BAR_NONE)
	{
		if(pOldWnd != 0)
			pOldWnd->ShowWindow(SW_HIDE);
	}

	else if (m_ControlBarExtra.iId != 0 && m_ControlBarExtra.iId < CONTROL_BAR_SHOW_LARGE )
	{
		if(pOldWnd != 0)
			pOldWnd->ShowWindow(SW_HIDE);
	}
	
	if(pOldWndExtra != 0)
	{
		pOldWndExtra->ShowWindow(SW_HIDE);
		m_ControlBarExtra.pWnd = 0;
	}

	//	Make sure the light pen control remains visible
	if(m_bPenControls)
		ShowLightPen(TRUE);
}

//==============================================================================
//
// 	Function Name:	CMainView::SetDisplay()
//
// 	Description:	This function uses the state identifier provided by the 
//					caller to set the appropriate visibility, size, and position
//					of the TMView and TMMovie controls.
//
// 	Returns:		None
//
//	Notes:			We can not hide the TMMovie control just by turning off its
//					visibility because the mpeg board will still show through
//					any part of the screen with the chroma color. To hide it
//					we size it to zero.
//
//==============================================================================
#define PAGES_MARGIN 15
void CMainView::SetDisplay(short sState)   
{
	//	Prevent attempts to update the toolbar buttons while we change
//	::ShowCursor(100);
	//AfxGetApp()->LoadCursorA(IDC_CURSOR1);
	//m_ctrlTMView->SetFocus();
	
	m_bDoUpdates = FALSE;

	//	Make sure the the task bar remains invisible while we move the windows around
	SetTaskBarVisible(FALSE);
	
	//	What is the desired state?
	switch(sState)
	{
		case S_CLEAR:
			
			//	Hide the control windows
			m_ctrlTMMovie.Pause();
			m_ctrlTMMovie.ShowWindow(SW_HIDE);
			m_ctrlTMMovie.ShowVideo(FALSE);
			for(int i=0; i < SZ_ARR_TM_VW; i++) {
				m_arrTmView[i]->ShowCallouts(FALSE, TMV_LEFTPANE);
				m_arrTmView[i]->ShowCallouts(FALSE, TMV_RIGHTPANE);
				m_arrTmView[i]->ShowWindow(SW_HIDE);
			}
			m_ctrlTMText.ShowWindow(SW_HIDE);
			m_ctrlTMStat.SetMode(TMSTAT_TEXTMODE);
			m_ctrlTMPower.Show(FALSE);
			break;
		
		case S_DOCUMENT:
		case S_GRAPHIC:

			//	Make sure the video is invisible
			//::ShowCursor(true);
			m_ctrlTMMovie.Pause();
			m_ctrlTMMovie.ShowWindow(SW_HIDE);
			m_ctrlTMMovie.ShowVideo(FALSE);
			m_ctrlTMPower.Show(FALSE);
			for(int i=0; i < SZ_ARR_TM_VW; i++)
				m_arrTmView[i]->BringWindowToTop();
			m_ctrlTMText.ShowWindow(SW_HIDE);
			m_ctrlTMStat.SetMode(TMSTAT_TEXTMODE);
//			m_ctrlTMText.ShowCursor(true);
			
			//	Setup the toolbar and resize the controls
			SelectToolbar(sState);
			RecalcLayout(sState);

			//	Size the TMView control
			for(int i = 0; i < SZ_ARR_TM_VW; i++) {
				m_arrTmView[i]->MoveWindow(0, (i-1)*(m_ScreenResolution.bottom + PAGES_MARGIN), m_ScreenResolution.right, m_ScreenResolution.bottom);
				// m_ctrlTMView->SetFocus();

				//	This ensures that callouts defined in the current zap
				//	file (if that's what is loaded) will be properly sized
				m_arrTmView[i]->RescaleZapCallouts();

				//	Make sure the TMView control is visible
				if(!m_arrTmView[i]->IsWindowVisible())
				{
					m_arrTmView[i]->ShowWindow(SW_SHOW);
					if(m_arrTmView[i]->GetSplitScreen() == TRUE)
					{
						m_arrTmView[i]->ShowCallouts(TRUE, TMV_LEFTPANE);
						m_arrTmView[i]->ShowCallouts(TRUE, TMV_RIGHTPANE);
					}
					else
					{
						m_arrTmView[i]->ShowCallouts(TRUE, TMV_ACTIVEPANE);
					}
				}

				//::ShowCursor(true);
				m_arrTmView[i]->ShowWindow(SW_SHOW);
				m_arrTmView[i]->BringWindowToTop();
			}
			UpdateWindow();

			// m_ctrlTMPower.SetFocus();
			// m_ctrlTMView->GetActiveWindow();
			// m_ctrlTMView->GetDSCCursor();
//			m_ctrlTMView->SetFocus();
//			::ShowCursor(true);

			
			break;
		
		case S_POWERPOINT:

			//	Make sure the video is invisible
			m_ctrlTMMovie.Pause();
			m_ctrlTMMovie.ShowWindow(SW_HIDE);
			m_ctrlTMMovie.ShowVideo(FALSE);
			m_ctrlTMText.ShowWindow(SW_HIDE);
			m_ctrlTMStat.SetMode(TMSTAT_TEXTMODE);
			
			//	Setup the toolbar and resize the controls
			SelectToolbar(sState);
			RecalcLayout(sState);

			//	Size the TMPower control
			m_ctrlTMPower.MoveWindow(&m_rcPower);

			//	Make sure the other controls are invisible
			if(m_ctrlTMView->IsWindowVisible())
			{
				for(int i=0; i < SZ_ARR_TM_VW; i++) {
					m_arrTmView[i]->ShowCallouts(FALSE, TMV_LEFTPANE);
					m_arrTmView[i]->ShowCallouts(FALSE, TMV_RIGHTPANE);
					m_arrTmView[i]->ShowWindow(SW_HIDE);
				}
				ResetTMView();
			}

			//	Make sure the TMPower control is visible
			m_ctrlTMPower.Show(TRUE);
			m_ctrlTMPower.RedrawWindow();
			m_ctrlTMPower.BringWindowToTop();

			break;
		
		case S_MOVIE:
		case S_PLAYLIST: 
			
			SelectToolbar(sState);
			
			//	Make sure the status bar mode is correct
			m_ctrlTMStat.SetMode(m_Playlist.pPlaylist == 0 ? TMSTAT_TEXTMODE :
													         TMSTAT_PLAYLISTMODE);
			//	Recalculate the window rectangles
			RecalcLayout(sState);

			//	Set the scaling options
			if(sState == S_MOVIE)
				m_ctrlTMMovie.SetScaleVideo(m_bScaleAVI);
			else
				m_ctrlTMMovie.SetScaleVideo(m_bScalePlaylists);

			//	Move the TMMovie control into position
			//
			//	NOTE:	Doing the redraw at the same time we do the move seems
			//			to give some boards (specifically ATI) a problem so we
			//			do it as two individual operations
			m_ctrlTMMovie.MoveWindow(&m_rcMovie, FALSE);

			//	Make sure the TMMovie control is visible
			if(!m_ctrlTMMovie.IsWindowVisible())
			{
				m_ctrlTMMovie.ShowWindow(SW_SHOW);
				m_ctrlTMMovie.ShowVideo(TRUE);
			}

			//	Make sure the other controls are invisible
			if(m_ctrlTMText.IsWindowVisible())
				m_ctrlTMText.ShowWindow(SW_HIDE);
			if(m_ctrlTMView->IsWindowVisible())
			{
				for(int i = 0; i < SZ_ARR_TM_VW; i++) {
					m_arrTmView[i]->ShowCallouts(FALSE, TMV_LEFTPANE);
					m_arrTmView[i]->ShowCallouts(FALSE, TMV_RIGHTPANE);
					m_arrTmView[i]->ShowWindow(SW_HIDE);
				}
				ResetTMView();
			}
			if(m_ctrlTMPower.IsWindowVisible())
			{
				m_ctrlTMPower.Show(FALSE);
				ResetTMPower();
			}

			m_ctrlTMMovie.BringWindowToTop();
			m_ctrlTMMovie.RedrawWindow();
			
			//	We have to do this because some drivers do not properly draw the
			//	video if the loaded file does not change
			m_ctrlTMMovie.Invalidate();

			break;
		
		case S_LINKEDIMAGE:


			//m_ctrlTMView->ShowWindow(SW_HIDE);
			//	Setup the toolbar
			SelectToolbar(S_LINKEDIMAGE);
			
			//	Look at TMMovie to see if we are playing a playlist
			m_ctrlTMStat.SetMode(m_Playlist.pPlaylist == 0 ? TMSTAT_TEXTMODE :
													TMSTAT_PLAYLISTMODE);

			//	Resize and position the controls
			RecalcLayout(sState);
			
			m_ctrlTMText.MoveWindow(&m_rcText);
			
			//	Should we make the scrolling text visible?
			if((m_rcText.bottom - m_rcText.top) > 1)
			{
				m_ctrlTMText.ShowWindow(SW_SHOW);
				m_ctrlTMText.BringWindowToTop();
			}
			else
			{
				m_ctrlTMText.ShowWindow(SW_HIDE);
			}

			m_ctrlTMPower.Show(FALSE);
			m_ctrlTMMovie.MoveWindow(&m_rcMovie);
			m_ctrlTMView->MoveWindow(&m_rcView);
			m_ctrlTMMovie.BringWindowToTop();

			//	Make sure the viewer is not in split screen mode
			if(m_ctrlTMView->GetSplitScreen())
				m_ctrlTMView->SetSplitScreen(FALSE);

			//	This ensures that callouts defined in the current zap
			//	file (if that's what is loaded) will be properly sized
			m_ctrlTMView->RescaleZapCallouts();

			//	Make sure the TMView control is visible
			if(!m_ctrlTMView->IsWindowVisible())
			{
				m_ctrlTMMovie.RedrawWindow();
				for(int i = 0; i < SZ_ARR_TM_VW; i++) {
					m_arrTmView[i]->ShowWindow(SW_SHOW);
					m_arrTmView[i]->ShowCallouts(TRUE, TMV_ACTIVEPANE);
				}
				UpdateWindow();
			}
				
			break;

		case S_TEXT:

			//	Setup the toolbar
			SelectToolbar(S_TEXT);
			
			//	Make sure the status bar mode is correct
			m_ctrlTMStat.SetMode(TMSTAT_PLAYLISTMODE);

			//	Resize the control rectangles
			RecalcLayout(sState);

			//	Make sure the viewers are turned off
			for(int i = 0; i < SZ_ARR_TM_VW; i++) {
				m_arrTmView[i]->ShowCallouts(FALSE, TMV_LEFTPANE);
				m_arrTmView[i]->ShowCallouts(FALSE, TMV_RIGHTPANE);
				m_arrTmView[i]->ShowWindow(SW_HIDE);
			}
			m_ctrlTMPower.Show(FALSE);
			ResetTMView();
			ResetTMPower();

			//	Make sure the TMText control is visible
			m_ctrlTMText.MoveWindow(&m_rcText);
			if(!m_ctrlTMText.IsWindowVisible())
			{
				m_ctrlTMText.ShowWindow(SW_SHOW);
				m_ctrlTMText.RedrawWindow();
			}

			//	Move the TMMovie control into position
			//
			//	NOTE:	Doing the redraw at the same time we do the move seems
			//			to give some boards (specifically ATI) a problem so we
			//			do it as two individual operations
			m_ctrlTMMovie.SetScaleVideo(m_bScalePlaylists);
			m_ctrlTMMovie.MoveWindow(&m_rcMovie, FALSE);
			m_ctrlTMMovie.RedrawWindow();
			m_ctrlTMMovie.BringWindowToTop();

			//	Make sure the TMMovie control is visible
			if(!m_ctrlTMMovie.IsWindowVisible())
			{
				m_ctrlTMMovie.ShowWindow(SW_SHOW);
				m_ctrlTMMovie.ShowVideo(TRUE);
			}

			//	We have to do this because some drivers do not properly draw the
			//	video if the loaded file does not change
			m_ctrlTMMovie.Invalidate();
			break;

		case S_LINKEDPOWER:

			//	Setup the toolbar
			SelectToolbar(S_LINKEDPOWER);
			m_ctrlTMStat.SetMode(m_Playlist.pPlaylist == 0 ? TMSTAT_TEXTMODE :
													TMSTAT_PLAYLISTMODE);

			//	Resize and position the controls
			RecalcLayout(sState);

			//	Should we make the scrolling text visible?
			if((m_rcText.bottom - m_rcText.top) > 1)
			{
				m_ctrlTMText.ShowWindow(SW_SHOW);
				m_ctrlTMText.BringWindowToTop();
			}
			else
			{
				m_ctrlTMText.ShowWindow(SW_HIDE);
			}

			for(int i = 0; i < SZ_ARR_TM_VW; i++) {
				m_arrTmView[i]->ShowCallouts(FALSE, TMV_LEFTPANE);
				m_arrTmView[i]->ShowCallouts(FALSE, TMV_RIGHTPANE);
				m_arrTmView[i]->ShowWindow(SW_HIDE);
			}
			m_ctrlTMMovie.MoveWindow(&m_rcMovie);
			m_ctrlTMPower.MoveWindow(&m_rcPower);
			m_ctrlTMMovie.BringWindowToTop();

			//	Make sure the TMPower control is visible
			m_ctrlTMMovie.RedrawWindow();
			m_ctrlTMPower.Show(TRUE);
			m_ctrlTMPower.RedrawWindow();
			m_ctrlTMPower.BringWindowToTop();
			m_ctrlTMMovie.BringWindowToTop();

			break;
	}

	//	Save the new state and previous state
	//
	//	NOTE:	This MUST be done before setting the control bar because
	//			it uses the state information
	m_sPrevState = m_sState;
	m_sState = sState;

	//	Make sure the we have the correct control bar
	//
	//	NOTE:	We do this because the toolbar may have changed
	SetControlBar(m_ControlBar.iId);

	//	Post a message to capture the focus and restore the task bar
	PostMessage(WM_GRABFOCUS, 0);

	//	OK to resume updates
	m_bDoUpdates = TRUE;

}

//==============================================================================
//
// 	Function Name:	CMainView::SetDrawingTool()
//
// 	Description:	This function is called to set the current drawing tool
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::SetDrawingTool(short sTool)   
{
	//	Are we supposed to restore the toolbar?
	if(m_bRestoreToolbar)
	{
		SetControlBar(CONTROL_BAR_TOOLS);
		m_bRestoreToolbar = FALSE;
	}

	//	Make sure the light pen control remains visible
	if(m_bPenControls)
		ShowLightPen(TRUE);

	//	Change the drawing tool
	for(int i = 0; i < SZ_ARR_TM_VW; i++) {
		m_arrTmView[i]->SetAnnTool(sTool);
		m_arrTmView[i]->SetAction(DRAW);
	}

	//	Update the ini file
	m_Ini.SetTMSection(PRESENTATION_APP);
	m_Ini.WriteLong(DRAWTOOL_LINE, m_ctrlTMView->GetAnnTool());

	//	Make sure the drawing tools toolbar is hidden
	if(m_ctrlTBTools.IsWindowVisible())
		m_ctrlTBTools.ShowWindow(SW_HIDE);
}

//==============================================================================
//
// 	Function Name:	CMainView::SetLastPlaylist()
//
// 	Description:	This function will set the members required to keep track of
//					the last playlist and update the ini file
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::SetLastPlaylist(CPlaylist* pPlaylist)   
{
	//	Save the master id of the last playlist
	if(pPlaylist)
		m_strLastPlaylist = pPlaylist->m_strMediaId;
	else
		m_strLastPlaylist.Empty();

	//	Update the information in the ini file
	m_Ini.SetTMSection(PRESENTATION_APP);
	m_Ini.WriteString(LASTPLAYLIST_LINE, m_strLastPlaylist);
}

//==============================================================================
//
// 	Function Name:	CMainView::SetLine()
//
// 	Description:	This function will open a dialog box that will allow the
//					user to set the line from which to resume playback of the
//					current playlist.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::SetLine(LPCSTR lpErrorMsg, CPlaylist* pPlaylist, long lDesignation) 
{
	CSetLine		Dialog(this);
	CDesignation*	pDesignation;
	SPlaylistParams	PLParams;
	CString			strError = (lpErrorMsg) ? lpErrorMsg : "";

	//	Do we have to assign a playlist?
	if(pPlaylist == NULL)
	{
		//	Load the last playlist if we do not have an active playlist
		if(m_Playlist.pPlaylist == NULL)
			pPlaylist = GetLastPlaylist();
		else
			pPlaylist = m_Playlist.pPlaylist;

	}// if(pPlaylist == NULL)

	//	Allow the user to cue the next playlist if we don't have an active
	//	playlist to work with
	if(pPlaylist == NULL)
	{
		SetNextLine(lpErrorMsg);
		return;
	}

	if(lDesignation < 0)
		lDesignation = m_PlaylistStatus.lDesignationOrder;

	//	Disable the keyboard hook
	theApp.EnableHook(FALSE);
	theApp.EnableEscapeHook(FALSE);	
	
	//	Make sure the current playback is paused
	m_ctrlTMMovie.Pause();

	//	Set the dialog box label and error message
	Dialog.SetLabel("Play Current From:");
	if(m_pDatabase != NULL)
		Dialog.SetTranscripts(m_pDatabase->GetTranscripts());
	Dialog.SetPlaylist(pPlaylist);

	//	Use the current designation to initialize the cue transcript
	if(lDesignation > 0)
	{
		pDesignation = pPlaylist->m_Designations.FindFromOrder(lDesignation);
		if(pDesignation)
			m_lCueTranscript = pDesignation->m_lTranscriptId;
	}

	//	Set the size and position of the dialog
	SetPosition(&Dialog);

	while(1)
	{
		//	Reset the dialog
		Dialog.SetPage(m_iCuePage);
		Dialog.SetLine(m_iCueLine);
		Dialog.SetTranscript(m_lCueTranscript);
		Dialog.SetMessage(strError);

		//	Open the dialog
		if(Dialog.DoModal() == IDCANCEL)
		{
			//	Clear the current cue position
			m_iCuePage = 0;
			m_iCueLine = 0;
			m_lCueTranscript = 0;

			//	Do we need to deallocate the playlist object?
			if(pPlaylist != m_Playlist.pPlaylist)
				delete pPlaylist;

			break;
		}

		//	Save the cue information specified by the user
		m_iCuePage = Dialog.GetPage();
		m_iCueLine = Dialog.GetLine();
		m_lCueTranscript = Dialog.GetTranscript();

		//	Fill the playlist event information
		ZeroMemory(&PLParams, sizeof(PLParams));
		PLParams.pPlaylist = pPlaylist;

		//	Translate the caller's specification to a start designation
		//	and frame number
		if(TranslateLine(&PLParams))
		{
			LoadPlaylist(&PLParams);
			break;
		}
		else
		{
			strError = "Specification is out of range";
		}

	}//	while(1)
	
	//	Enable the keyboard hook
	theApp.EnableHook(TRUE);
	theApp.EnableEscapeHook(FALSE);	
}

//==============================================================================
//
// 	Function Name:	CMainView::SetMultipageInfo()
//
// 	Description:	Called to set the multipage information for the record
//					associated with the specified barcode or unique database
//					identifer
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::SetMultipageInfo(SMultipageInfo& rMPInfo, LPCSTR lpszId, BOOL bBarcode) 
{
	CBarcode	Barcode;
	CMedia*		pMedia = NULL;
	CMultipage*	pMultipage = NULL;
	CSecondary*	pSecondary = NULL;
	CTertiary*	pTertiary = NULL;
	BOOL		bSuccessful = FALSE;

	ASSERT_RET_BOOL(m_pDatabase != NULL);

	//	Use a barcode object to parse the identifier provided by the caller
	if(Barcode.SetBarcode(lpszId) == TRUE)
	{
		//	Should we treat the id as a barcode?
		if(bBarcode == TRUE)
			pMedia = m_pDatabase->GetMedia(Barcode.m_strMediaId);
		else
			pMedia = m_pDatabase->GetMedia(atol(Barcode.m_strMediaId));

		//	We're we able to locate the primary media record?
		if(pMedia != NULL)
		{
			//	Get the populated multipage object for this media
			if((pMultipage = m_pDatabase->GetMultipage(pMedia)) != NULL)
			{
				//	Did the caller specify a secondary identifier?
				if(Barcode.m_lSecondaryId > 0)
				{
					if(bBarcode == TRUE)
						pSecondary = pMultipage->m_Pages.FindByBarcodeId(Barcode.m_lSecondaryId);
					else
						pSecondary = pMultipage->m_Pages.FindByDatabaseId(Barcode.m_lSecondaryId);

					if(pSecondary != NULL)
					{
						//	Get the treatments from the database
						if(pSecondary != 0)
							m_pDatabase->GetTertiaries(pSecondary);

						//	Did the caller specify a tertiary identifier?
						if(Barcode.m_lTertiaryId > 0)
						{
							if(bBarcode == TRUE)
								pTertiary = pSecondary->m_Children.FindByBarcodeId(Barcode.m_lTertiaryId);
							else
								pTertiary = pSecondary->m_Children.FindByDatabaseId(Barcode.m_lTertiaryId);
							
							if(pTertiary != NULL)
								bSuccessful = TRUE;
						}
						else
						{
							bSuccessful = TRUE; // All is good
						}

					}// if(pSecondary != NULL)

				}
				else
				{
					bSuccessful = TRUE; // All is good
				}

			}// if((pMultipage = m_pDatabase->GetMultipage(pMedia)) != NULL)

		}// if(pMedia != NULL)

	}// if(Barcode.SetBarcode(lpszId) == TRUE)

	//	Did we locate the record?
	if(bSuccessful == TRUE)
	{
		rMPInfo.pMultipage = pMultipage;
		rMPInfo.pSecondary = pSecondary;
		rMPInfo.pTertiary  = pTertiary;
	}
	else
	{
		if(pMultipage != NULL)
			delete pMultipage;
	}

	return bSuccessful;
}

//==============================================================================
//
// 	Function Name:	CMainView::SetNextLine()
//
// 	Description:	This function will open a dialog box that allows the user
//					to set the line at which playback of the next playlist will
//					begin.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::SetNextLine(LPCSTR lpErrorMsg) 
{
	CSetLine Dialog(this);

	//	Disable the keyboard hook
	theApp.EnableHook(FALSE);
	theApp.EnableEscapeHook(FALSE);	
	
	//	Make sure the current playback is paused
	m_ctrlTMMovie.Pause();

	//	Initialize the dialog box
	Dialog.SetLabel("Play Next From:");
	Dialog.SetMessage(lpErrorMsg);
	Dialog.SetPage(m_iCuePage);
	Dialog.SetLine(m_iCueLine);
	Dialog.SetTranscript(m_lCueTranscript);
	if(m_pDatabase != 0)
		Dialog.SetTranscripts(m_pDatabase->GetTranscripts());
	Dialog.SetPlaylist(0);

	//	Set the size and position of the dialog
	SetPosition(&Dialog);

	if(Dialog.DoModal() == IDCANCEL)
	{
		//	Clear the current cue indicators
		m_iCuePage = 0;
		m_iCueLine = 0;
		m_lCueTranscript = 0;

		//	Enable the keyboard hook
		theApp.EnableHook(TRUE);
		theApp.EnableEscapeHook(FALSE);	
		return;
	}

	//	All we can do is save the cue settings so that they get applied to the 
	//	next playlist that gets loaded
	m_iCuePage = Dialog.GetPage();
	m_iCueLine = Dialog.GetLine();
	m_lCueTranscript = Dialog.GetTranscript();

	//	Enable the keyboard hook
	theApp.EnableHook(TRUE);
	theApp.EnableEscapeHook(FALSE);	
}

//==============================================================================
//
// 	Function Name:	CMainView::SetPageFromBarcode()
//
// 	Description:	This function is called to set the active page in the 
//					multipage information structure provided by the caller.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::SetPageFromBarcode(SMultipageInfo* pInfo, long lSecondaryId, 
								 long lTertiaryId)  
{
	CString strError;

	ASSERT(pInfo);
	ASSERT(pInfo->pMultipage);
	if(pInfo->pMultipage == 0)
	{
		pInfo->pSecondary = 0;
		pInfo->pTertiary = 0;
		return FALSE;
	}

	//	Do we want the first page?
	if(lSecondaryId <= 0)
	{
		pInfo->pSecondary = pInfo->pMultipage->m_Pages.First();
		pInfo->pTertiary = 0;

		//	Get the list of treatments if this is an image
		if(pInfo->pMultipage->m_lPlayerType == MEDIA_TYPE_IMAGE)
		{
			//	Get the treatments from the database
			if(pInfo->pSecondary != 0)
				m_pDatabase->GetTertiaries(pInfo->pSecondary);
		}

		return (pInfo->pSecondary != 0);
	}

	//	What type of multipage media are we dealing with?
	switch(pInfo->pMultipage->m_lPlayerType)
	{
		case MEDIA_TYPE_IMAGE:
		
			if(m_iImageSecondary == SECONDARY_AS_ORDER)
				pInfo->pSecondary = pInfo->pMultipage->m_Pages.FindByOrder(lSecondaryId);
			else
				pInfo->pSecondary = pInfo->pMultipage->m_Pages.FindByBarcodeId(lSecondaryId);
			
			//	Get the list of treatments for this page
			if(pInfo->pSecondary != 0)
				m_pDatabase->GetTertiaries(pInfo->pSecondary);

			//	Do we need to find a treatment object for this page
			if((pInfo->pSecondary != 0) && (lTertiaryId > 0))
			{
				if(m_iTreatmentTertiary == TERTIARY_AS_ORDER)
					pInfo->pTertiary = pInfo->pSecondary->m_Children.FindByOrder(lTertiaryId);
				else
					pInfo->pTertiary = pInfo->pSecondary->m_Children.FindByBarcodeId(lTertiaryId);

				//	Did we find the treatment?
				if(pInfo->pTertiary == NULL)
					return FALSE;
			}
			else
			{
				pInfo->pTertiary = NULL;
			}
			break;
			
		case MEDIA_TYPE_POWERPOINT:
		
			if(m_iPowerPointSecondary == SECONDARY_AS_ORDER)
				pInfo->pSecondary = pInfo->pMultipage->m_Pages.FindByOrder(lSecondaryId);
			else if(m_iPowerPointSecondary == SECONDARY_AS_SLIDEINDEX)
				pInfo->pSecondary = pInfo->pMultipage->m_Pages.FindBySlide(lSecondaryId);
			else
				pInfo->pSecondary = pInfo->pMultipage->m_Pages.FindByBarcodeId(lSecondaryId);

			//	PowerPoint shows do not have treatments
			pInfo->pTertiary = 0;

			break;
			
		case MEDIA_TYPE_RECORDING:
		
			if(m_iAnimationSecondary == SECONDARY_AS_ORDER)
				pInfo->pSecondary = pInfo->pMultipage->m_Pages.FindByOrder(lSecondaryId);
			else
				pInfo->pSecondary = pInfo->pMultipage->m_Pages.FindByBarcodeId(lSecondaryId);

			//	Is the user attempting to load a clip in a custom show?
			if((m_bLoadingShowItem == TRUE) && (lTertiaryId > 0))
			{
				//	Attempt to locate the clip
				pInfo->pTertiary = m_pDatabase->GetClip(pInfo->pSecondary, lTertiaryId, m_pDatabase->GetSceneId());
				if(pInfo->pTertiary != 0)
				{
					pInfo->pSecondary->m_Children.Add(pInfo->pTertiary, FALSE);
				}
				else
				{
					lTertiaryId = 0;
				}
			}
			else
			{
				//	Make sure no attempt is made to load a clip outside of a custom show
				lTertiaryId = 0;
				pInfo->pTertiary = 0;
			}

			break;
			
		default:
		
			pInfo->pSecondary = 0;
			pInfo->pTertiary = 0;
			break;
	}
	
	if(pInfo->pSecondary != NULL)
	{
		//	Update the barcode information
		m_Barcode.m_strMediaId   = pInfo->pMultipage->m_strMediaId;
		m_Barcode.m_lSecondaryId = lSecondaryId;
		m_Barcode.m_lTertiaryId  = lTertiaryId;
		UpdateStatusBar();

		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::SetPageFromId()
//
// 	Description:	This function is called to load the specified page of the
//					current multipage object.
//
// 	Returns:		TRUE if successful
//
//	Notes:			It is assumed that the information structure provided by
//					the caller is NOT one of the structures attached to the
//					viewers. Otherwise a memory leak may result because the
//					multipage object may not be deallocated.
//
//==============================================================================
BOOL CMainView::SetPageFromId(SMultipageInfo* pInfo, long lPage, int iLookup)
{
	SMultipageInfo* pCurrent = GetMultipageInfo(m_sState);

	ASSERT(pInfo);

	//	Make sure the caller's information is reset
	//
	//	SEE NOTE IN HEADER
	ZeroMemory(pInfo, sizeof(SMultipageInfo));

	//	Do we have an active multipage object?
	if((pCurrent == 0) || (pCurrent->pMultipage == 0))
		return FALSE;

	//	Initialize the information for the new page
	pInfo->pMultipage = pCurrent->pMultipage;

	//	Now retrieve the specified page
	switch(iLookup)
	{
		case SETPAGE_FIRST:

			pInfo->pSecondary = pInfo->pMultipage->m_Pages.First();
			break;

		case SETPAGE_NEXT:

			pInfo->pSecondary = pInfo->pMultipage->m_Pages.Next();
			break;

		case SETPAGE_PREVIOUS:

			pInfo->pSecondary = pInfo->pMultipage->m_Pages.Prev();
			break;

		case SETPAGE_LAST:

			pInfo->pSecondary = pInfo->pMultipage->m_Pages.Last();
			break;

		case SETPAGE_BYID:

			pInfo->pSecondary = pInfo->pMultipage->m_Pages.FindByBarcodeId(lPage);
			break;

		case SETPAGE_BYORDER:
		default:

			pInfo->pSecondary = pInfo->pMultipage->m_Pages.FindByOrder(lPage);
			break;
	}

	if(pInfo->pSecondary != 0)
	{
		//	Get the list of treatments if this is an image
		if(pInfo->pMultipage->m_lPlayerType == MEDIA_TYPE_IMAGE)
		{
			//	Get the treatments from the database
			m_pDatabase->GetTertiaries(pInfo->pSecondary);
		}

		//	Update the barcode information
		m_Barcode.m_strMediaId   = pInfo->pMultipage->m_strMediaId;
		m_Barcode.m_lSecondaryId = pInfo->pSecondary->m_lBarcodeId;
		m_Barcode.m_lTertiaryId  = -1;
		UpdateStatusBar();

		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::SetPosition()
//
// 	Description:	This function will set the size and postion of the SetLine
//					dialog provided by the caller
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::SetPosition(CSetLine* pSetLine)   
{
	int		iHeight;
	RECT	Pos;

	ASSERT(pSetLine);

	//	How high should the dialog be
	iHeight = (m_ControlBar.iHeight > 0) ? m_ControlBar.iHeight : 10;

	//	Set the coordinates for the page-line dialog to cover the toolbar
	Pos.left   = 0;
	Pos.right  = m_iMaxWidth;
	Pos.top    = m_iMaxHeight - iHeight;
	Pos.bottom = m_iMaxHeight;

	//	Set the dialog size and position
	pSetLine->SetPos(&Pos);
}

//==============================================================================
//
// 	Function Name:	CMainView::SetSinglePaneMode()
//
// 	Description:	Called to make sure the TMView control is in single pane
//					viewer mode
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::SetSinglePaneMode()   
{
	if(m_ctrlTMView->GetSplitScreen() == TRUE)
	{
		//	Return to single pane mode
		if(m_ctrlTMView->GetSplitHorizontal() == TRUE)
			OnSplitHorizontal();
		else
			OnSplitVertical();
	}

	//	No longer in split screen mode
	SetZapSplitScreen(FALSE);
}

//==============================================================================
//
// 	Function Name:	CMainView::SetTaskBarVisible()
//
// 	Description:	This function will show/hide the system task bar
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::SetTaskBarVisible(BOOL bVisible)
{
	//	Are we hiding the task bar
	if(bVisible == FALSE)
	{
		//	Only hide the task bar if not using the secondary monitor
		if((theApp.GetDualMonitors() == FALSE) || (m_bUseSecondaryMonitor == FALSE))
			CTMToolbox::SetTaskBarVisible(FALSE);
	}
	else
	{
		CTMToolbox::SetTaskBarVisible(TRUE);
	}

}

//==============================================================================
//
// 	Function Name:	CMainView::SetToForeground()
//
// 	Description:	This function is called to make the view the foreground
//					window.
//
// 	Returns:		None
//
//	Notes:			This function works around the change made to 
//					SetForegroundWindow() in Win98 and Win2000
//
//==============================================================================
void CMainView::SetToForeground()
{
	DWORD	dwForegroundThread;
	DWORD	dwViewThread;

	//	Get the id of the foreground thread
	dwForegroundThread = GetWindowThreadProcessId(::GetForegroundWindow(), NULL);

	//	Get the thread id of this window
	dwViewThread = GetCurrentThreadId();

	//	Attach the view input to the foreground input
	if(dwForegroundThread != dwViewThread)
		AttachThreadInput(dwForegroundThread, dwViewThread, TRUE);

	//	Bring the view to the foreground
	::SetForegroundWindow(m_hWnd);
	m_pFrame->BringWindowToTop();

	//	Detach the threads
	if(dwForegroundThread != dwViewThread)
		AttachThreadInput(dwForegroundThread, dwViewThread, FALSE);
}

//==============================================================================
//
// 	Function Name:	CMainView::SetZapSplitScreen()
//
// 	Description:	This function is called to set the flag to indicate if a
//					split screen zap has been loaded
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::SetZapSplitScreen(BOOL bZapSplitScreen)
{
	m_bZapSplitScreen = bZapSplitScreen;

	if(m_bZapSplitScreen == TRUE)
		m_ctrlTMView->SetSplitFrameColor(m_iZapSplitFrameColor);
	else
		m_ctrlTMView->SetSplitFrameColor(m_iUserSplitFrameColor);
}

//==============================================================================
//
// 	Function Name:	CMainView::ShowLightPen()
//
// 	Description:	This function is called to set the visibility of the status
//					bar control
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::ShowLightPen(BOOL bShow) 
{
	int		iSize;
	RECT	rcPen;

	//	Set the status bar flag
	m_bPenControls = bShow;

	//	Are we showing the status bar?
	if(m_bPenControls)
	{
		//	How high should the status bar be?
		iSize = (m_ControlBar.iHeight > 0) ? m_ControlBar.iHeight : m_ctrlTBGraphics.GetBarHeight();

		//	Set the coordinates
		rcPen.left   = m_iMaxWidth - iSize;
		rcPen.right  = m_iMaxWidth;
		rcPen.top    = m_iMaxHeight - iSize;
		rcPen.bottom = m_iMaxHeight;

		m_ctrlTMLpen.ShowWindow(SW_SHOW);
		m_ctrlTMLpen.MoveWindow(&rcPen);
		m_ctrlTMLpen.BringWindowToTop();
	}
	else
	{
		m_ctrlTMLpen.ShowWindow(SW_HIDE);
		m_ctrlTMLpen.MoveWindow(10000,10000,1,1,FALSE);
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::ShowRectangle()
//
// 	Description:	This function is called to display the coordinates of the
//					rectangle
//
// 	Returns:		None
//
//	Notes:			This function is used for debugging only
//
//==============================================================================
void CMainView::ShowRectangle(RECT* pRect, LPCSTR lpTitle) 
{
	CString strRect;
	CString	strTitle = (lpTitle == 0) ? "Show Rectangle" : lpTitle;
	ASSERT(pRect);

	strRect.Format("Left: %d\nTop: %d\nRight: %d\nBottom: %d",
					pRect->left, pRect->top, pRect->right, pRect->bottom);

	MessageBox(strRect, strTitle, MB_OK);
}

//==============================================================================
//
// 	Function Name:	CMainView::Shutdown()
//
// 	Description:	This function is called whenever the frame window is being
//					closed. It performs an orderly shutdown of the view.
//
// 	Returns:		TRUE if ok to shutdown
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::Shutdown() 
{
	CDialog Confirm(IDD_CONFIRM_EXIT);

	// close virtual keyboard
	if (m_pVKBDlgPtr)
		m_pVKBDlgPtr->CloseWindow();

	//	Make sure the video is paused before we pop up the confirmation dialog
	if(m_bPlaying)
		OnPlay();

	//	Stop the custom show timer
	KillTimer(CUSTOM_SHOW_TIMERID);

	//	Disable the keyboard hook
	theApp.EnableHook(FALSE);
	theApp.EnableEscapeHook(FALSE);	

	//	Prompt the user for confirmation
	if(!theApp.GetSilent())
	{
		if(Confirm.DoModal() == IDCANCEL)
		{
			theApp.EnableHook(TRUE);
			theApp.EnableEscapeHook(FALSE);	
			return FALSE;
		}
	}

	//	Restore the system task bar
	SetTaskBarVisible(TRUE);

	//	Prevent any attempt to reuse this instance
	theApp.UnlockInstance();

	//	Turn off the windows
	for(int i = 0; i < SZ_ARR_TM_VW; i++) {
		m_arrTmView[i]->ShowCallouts(FALSE, TMV_LEFTPANE);
		m_arrTmView[i]->ShowCallouts(FALSE, TMV_RIGHTPANE);
		m_arrTmView[i]->ShowWindow(SW_HIDE);
	}
	m_ctrlTMText.ShowWindow(SW_HIDE);
	m_ctrlTMStat.ShowWindow(SW_HIDE);
	m_ctrlTMMovie.ShowWindow(SW_HIDE);
	m_ctrlTMMovie.ShowVideo(FALSE);
	m_ctrlTMPower.ShowWindow(SW_HIDE);
	
	//	Reset the display controls
	ResetTMMovie(TRUE, TRUE);
	ResetTMView();

	//	Close the PowerPoint dispatch interfaces
	if(m_ctrlTMPower.IsInitialized())
	{
		ResetTMPower();
		m_ctrlTMPower.Close();
	}

	//	Reset the custom show information
	ResetShowInfo();

	//	Close the database 
	CloseDatabase();

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CMainView::StopAutoTransition()
//
// 	Description:	This function will stop the automatic transitioning to the
//					next show item when called.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::StopAutoTransition()
{
	//	Are we running a custom show?
	if(m_Show.pItem != 0)
		m_Show.pItem->m_ulGoToNext = 0;	//	This kills the transition
}

//==============================================================================
//
// 	Function Name:	CMainView::SwitchPane()
//
// 	Description:	This function will switch the active pane in split screen
//					view
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::SwitchPane()
{
	//	Is this command enabled?
	if(!IsCommandEnabled(TMAX_SWITCHPANE))
		return;

	//	Switch the active pane
	if(m_ctrlTMView->GetActivePane() == TMV_LEFTPANE)
		m_ctrlTMView->SetActivePane(TMV_RIGHTPANE);
	else
		m_ctrlTMView->SetActivePane(TMV_LEFTPANE);

}

//==============================================================================
//
// 	Function Name:	CMainView::TranslateLine()
//
// 	Description:	This function will translate the stored playlist line
//					specification to a valid designation and frame number.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::TranslateLine(SPlaylistParams* pParams)
{
	CDesignation*	pDesignation;
	double			dPosition = -1.0;

	ASSERT(pParams);
	if(pParams == 0)
		return FALSE;

	//	There must be a playlist 
	if(pParams->pPlaylist == 0)
		return FALSE;

	//	Do we have valid line specifications?
	if((m_iCuePage <= 0) || (m_iCueLine <= 0))
		return FALSE;

	//	Locate the first designation that meets the cue criteria
	pDesignation = pParams->pPlaylist->GetFirstInRange(m_iCuePage, 
													   m_iCueLine, 
													   m_lCueTranscript);
	if(pDesignation == NULL)
		return FALSE;

	//	Get the time position for the specified page and line
	if((dPosition = pDesignation->GetTime(m_iCuePage, m_iCueLine, TRUE)) < 0)
		return FALSE;

	//	Set the event information
	pParams->dPosition = dPosition;
	pParams->pStart    = pDesignation;
	pParams->lStart    = pDesignation->m_lPlaybackOrder;

	//	Set the stop designation
	if(m_bRunToEnd)
	{
		pParams->lStop = -1;
		pParams->pStop = pParams->pPlaylist->m_Designations.Last();
	}
	else
	{
		pParams->lStop = pParams->pStart->m_lPlaybackOrder;
		pParams->pStop = pParams->pStart;
	}

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CMainView::UpdateActivityLog()
//
// 	Description:	This function will add an entry to the activity log used
//					during testing
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::UpdateActivityLog(LPCSTR lpText, BOOL bStamp) 
{
	CString strEntry;
	CString strTimeStamp;
	FILE*	pFile;
	CTime	Time = CTime::GetCurrentTime();

	//	Are we supposed to include the time stamp?
	if(bStamp)
	{
		//	Build the time stamp
		strTimeStamp = Time.Format("%m-%d-%Y %H:%M:%S");

		//	Format the log entry
		strEntry.Format("%s - %s", strTimeStamp, lpText);
	}
	else
	{
		strEntry = lpText;
	}
		
	fopen_s(&pFile, m_Test.szLogFile, "at");
	if(pFile != NULL)
	{
		fprintf(pFile, "%s\n", strEntry);
		fclose(pFile);
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::UpdateMultipage()
//
// 	Description:	Called to update the specified target multipage descriptor
//					with the values stored in the specified source descriptor
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::UpdateMultipage(SMultipageInfo* pTarget, SMultipageInfo* pSource)
{
	if(pSource != NULL)
	{
		//	Should we delete the existing multipage object?
		if(pTarget->pMultipage != NULL)
		{
			if(pSource->pMultipage != pTarget->pMultipage)
				delete pTarget->pMultipage;
		}

		//	Assign the new values
		pTarget->pMultipage = pSource->pMultipage;
		pTarget->pSecondary = pSource->pSecondary;
		pTarget->pTertiary  = pSource->pTertiary;
	}
	else
	{
		ResetMultipage(pTarget);
	}

}

//==============================================================================
//
// 	Function Name:	CMainView::UpdatePlaylistStatus()
//
// 	Description:	This function is called to update the playlist information
//					in the status bar
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::UpdatePlaylistStatus() 
{
	memset(m_PlaylistStatus.szMediaId, 0, sizeof(m_PlaylistStatus.szMediaId));
	memset(m_PlaylistStatus.szLinkId, 0, sizeof(m_PlaylistStatus.szLinkId));
	m_PlaylistStatus.bShowLink = FALSE;

	if((m_Playlist.pPlaylist != NULL) && (m_Playlist.pDesignation != NULL))
	{
		sprintf_s(m_PlaylistStatus.szMediaId, sizeof(m_PlaylistStatus.szMediaId), "%s.%d", m_Playlist.pPlaylist->m_strMediaId, m_Playlist.pDesignation->m_lBarcodeId);
	
		//	Are we displaying a link?
		if((m_sState == S_LINKEDIMAGE) || (m_sState == S_LINKEDPOWER))
		{
			if(m_AppLink.GetBarcode().GetLength() > 0)
			{
				lstrcpyn(m_PlaylistStatus.szLinkId, m_AppLink.GetBarcode(), sizeof(m_PlaylistStatus.szLinkId));
				m_PlaylistStatus.bShowLink = TRUE;
			}

		}

	}// if((m_Playlist.pPlaylist != NULL) && (m_Playlist.pDesignation != NULL))

	m_PlaylistStatus.dPlaylistTime = m_ctrlTMMovie.GetPlaylistTime(),
	m_PlaylistStatus.dElapsedPlaylist = m_ctrlTMMovie.GetElapsedPlaylist();
	m_PlaylistStatus.dDesignationTime = m_ctrlTMMovie.GetDesignationTime();
	m_PlaylistStatus.dElapsedDesignation = m_ctrlTMMovie.GetElapsedDesignation();
	
	UpdateStatusBar();
}

//==============================================================================
//
// 	Function Name:	CMainView::UpdateStatusBar()
//
// 	Description:	This function will update the status bar with the latest
//					values
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::UpdateStatusBar()
{
	//	Is the database open?
	if((m_pDatabase != 0) && (m_pDatabase->IsOpen() == TRUE))
	{
		//	Is this a newly opened database?
		if(m_Barcode.m_strMediaId.IsEmpty())
		{
			m_ctrlTMStat.SetPlaylistInfo(0);	
			m_ctrlTMStat.SetStatusText(m_pDatabase->GetFilespec());
		}
		else
		{
			m_ctrlTMStat.SetPlaylistInfo((long)&m_PlaylistStatus);	
			m_ctrlTMStat.SetStatusText(m_Barcode.GetBarcode());
		}
	}
	else
	{
		m_ctrlTMStat.SetPlaylistInfo(0);	
		m_ctrlTMStat.SetStatusText("No database");
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::UpdateToolColor()
//
// 	Description:	This function will update the tool color in the ini file
//					when it is changed by the user
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::UpdateToolColor()
{
	//	Set the ini file to the appropriate section
	m_Ini.SetTMSection(PRESENTATION_APP);

	//	What is the current action
	switch(m_ctrlTMView->GetAction())
	{
		case REDACT:

			m_Ini.WriteLong(REDACTCOLOR_LINE, m_ctrlTMView->GetColor());
			return;

		case HIGHLIGHT:	

			m_Ini.WriteLong(HIGHLIGHTCOLOR_LINE, m_ctrlTMView->GetColor());
			break;

		case CALLOUT:	

			m_Ini.WriteLong(CALLOUTCOLOR_LINE, m_ctrlTMView->GetColor());
			break;

		case ZOOM:		
		case DRAW:		
		case PAN:	
		case SELECT:	
		default:		

			m_Ini.WriteLong(ANNCOLOR_LINE, m_ctrlTMView->GetColor());
			break;

	}
}

//==============================================================================
//
// 	Function Name:	CMainView::UpdateZap()
//
// 	Description:	Called to update the specified zap file
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CMainView::UpdateZap(SMultipageInfo* pInfo, short sPaneId) 
{
	CString			strZapFileSpec = "";
	CString			strBackupFileSpec = "";
	BOOL			bSuccessful = FALSE;

	//	Do we have an active treatment?
	ASSERT_RET_BOOL(m_pDatabase != NULL);
	ASSERT_RET_BOOL(pInfo != NULL);
	ASSERT_RET_BOOL(pInfo->pMultipage != NULL);
	ASSERT_RET_BOOL(pInfo->pSecondary != NULL);
	ASSERT_RET_BOOL(pInfo->pTertiary != NULL);
	ASSERT_RET_BOOL(pInfo->pMultipage->m_lPlayerType == MEDIA_TYPE_IMAGE);
	
	//	Get the path to the existing treatment
	m_pDatabase->GetFilename(pInfo->pTertiary, strZapFileSpec);
	ASSERT_RET_BOOL(strZapFileSpec.GetLength() > 0);

	//	Should we create the backup?
	if(FindFile(strZapFileSpec) == TRUE)
	{
		strBackupFileSpec = strZapFileSpec;
		strBackupFileSpec.MakeLower();
		strBackupFileSpec.Replace(".zap", ".bak");

		//	Make sure the backup does not already exist
		_unlink(strBackupFileSpec);

		//	Create the backup file
		rename(strZapFileSpec, strBackupFileSpec);
	}
	else
	{
		//	This should never happen
		ASSERT(FALSE);
	}

	//	Now save the zap file
	//
	//	NOTE:	TMView will report any errors if reporting is turned on
	if(m_ctrlTMView->SaveZap(strZapFileSpec, sPaneId) == TMV_NOERROR)
	{
		//	Is the manager running
		if(m_ctrlManagerApp.IsRunning() == TRUE)
		{
			//	Format a request to add the new treatment
			m_ctrlManagerApp.SetPrimaryId(pInfo->pMultipage->m_lPrimaryId);
			m_ctrlManagerApp.SetSecondaryId(pInfo->pSecondary->m_lSecondaryId);
			m_ctrlManagerApp.SetTertiaryId(pInfo->pTertiary->m_lTertiaryId);
			m_ctrlManagerApp.SetCommand(TMSHARE_COMMAND_UPDATE_TREATMENT);
			m_ctrlManagerApp.SetRequest(0);
		}

		bSuccessful = TRUE;

	}
	else
	{
		//	Restore the original zap file
		if(strBackupFileSpec.GetLength() > 0)
		{
			_unlink(strZapFileSpec);
			rename(strBackupFileSpec, strZapFileSpec);
		}

	}// if(m_ctrlTMView->SaveZap(strZapFileSpec, TMV_ACTIVEPANE) == TMV_NOERROR)

	return bSuccessful;
}

//==============================================================================
//
// 	Function Name:	CMainView::WindowProc()
//
// 	Description:	This function overloads the standard window procedure to
//					perform custom message handling.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
LRESULT CMainView::WindowProc(UINT message, WPARAM wParam, LPARAM lParam) 
{
	//	Are we hooking mouse messages?
	if(m_bMouseMode)
	{
		//	What message?
		switch(message)
		{
			case WM_PARENTNOTIFY:

				//	What notification?
				switch(LOWORD(wParam))
				{
					case WM_LBUTTONDOWN:

						if(ProcessNotification(LOWORD(lParam), HIWORD(lParam)))
							PostMessage(WM_MOUSEMODE, 0);
						return 1;

					case WM_RBUTTONDOWN:

						if(ProcessNotification(LOWORD(lParam), HIWORD(lParam)))
							PostMessage(WM_MOUSEMODE, 1);
						return 1;

					default:

						return CFormView::WindowProc(message, wParam, lParam);
				}

			default:

				return CFormView::WindowProc(message, wParam, lParam);
		}
	}
	else
	{
		return CFormView::WindowProc(message, wParam, lParam);
	}
}

bool CMainView::IsNextPageAvailable() {
	return IsCommandEnabled(TMAX_NEXTPAGE);
}

bool CMainView::IsPrevPageAvailable() {
	return IsCommandEnabled(TMAX_PREVPAGE);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnGesture()
//
// 	Description:	WM_GESTURE message handler. Current setting is only for
//					pan and zoom. See CApp::InitWmGesture()
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
bool scrollUpDownInProgress = false;
LRESULT CMainView::OnGesture(WPARAM wParam, LPARAM lParam)
{
	// check if tablet mode is on
	//if (!IsCommandChecked(TMAX_GESTURE_PAN))
		//return FALSE;

	GESTUREINFO gi;  
    ZeroMemory(&gi, sizeof(GESTUREINFO));   
    gi.cbSize = sizeof(GESTUREINFO);

    BOOL bResult  = GetGestureInfo((HGESTUREINFO)lParam, &gi);
	CString str;
	if(bResult)
	{
		switch(gi.dwID)
		{
			case GID_BEGIN:
				str.Format("gesture started: x=%hd , y=%hd", gi.ptsLocation.x, gi.ptsLocation.y);
				LogMe((LPCTSTR) str);
				m_bMouseMode = FALSE;
				m_bGestureHandled = FALSE;
				m_gestureStartPoint = m_gestureLastPoint = gi.ptsLocation;
				m_gestureStartTime = GetTickCount();

				if(!m_bIsBinderOpen) 
				{
					if(gi.ptsLocation.y < (m_ScreenResolution.bottom*8)/10 &&
						gi.ptsLocation.y > (m_ScreenResolution.bottom*2)/10) {
						if(m_pToolbar->IsWindowVisible()) {
							SetControlBar(CONTROL_BAR_NONE);
							toolbarForcedHidden = true;
						}
					}
				}

				break;

			case GID_END:

				if(scrollUpDownInProgress) {
					LogMe("--------------Gesture Ended---------------/n");
					m_bMouseMode = TRUE;

					RECT curRect;
					m_ctrlTMView->GetWindowRect(&curRect);
					if(abs(curRect.top) < m_ScreenResolution.bottom / 20 ||
						abs(curRect.top) > m_ScreenResolution.bottom * 19/20 ||
						abs(curRect.bottom) < m_ScreenResolution.bottom / 20)
						SetViewingCtrl();

				} else {
					// not pan, zooming, show toolbar
					if(toolbarForcedHidden) {
						RECT wndRect;
						m_pToolbar->GetWindowRect(&wndRect);
						wndRect.top = m_ScreenResolution.bottom;
						wndRect.left = 0;
						m_pToolbar->MoveWindow(&wndRect);
						SetControlBar(CONTROL_BAR_TOOLS);
						toolbarForcedHidden = false;
					}
				}

				break;

			case GID_ZOOM:
				if(IsCommandChecked(TMAX_GESTURE_PAN) &&
					!scrollUpDownInProgress) {
					HandleZoom(gi);
				}
				break;

			case GID_PAN:
				if(m_bIsBinderOpen) {
					int diff = gi.ptsLocation.y - m_gestureLastPoint.y;
					m_BinderList->HandlePan(diff);
					if(abs(diff) > 30) {
						m_gestureLastPoint = gi.ptsLocation;
					}
				} else {
					if(IsCommandChecked(TMAX_GESTURE_PAN))
						HandlePan(gi);
				}
				break;
			/*case GID_ROTATE:
				handleRotate(hWnd, gi);
				bHandled = 1;
				break;
			case GID_TWOFINGERTAP:
				handle2FingerTap(hWnd, gi);
				bHandled = 1;
				break;
			case GID_PRESSANDTAP:
				handlePNT(hWnd, gi);
				bHandled = 1;
				break;*/
			default:
				break;
		}
	}
	else
	{
		//failed to get gesture info
	}

	DefWindowProc(WM_GESTURE, wParam, lParam);
	return (GID_END | GID_ZOOM | GID_PAN) << 1;	
}

void CMainView::EmptyMessageQueue() {
	MSG msg;
	while (PeekMessage(&msg,NULL,0,0,PM_REMOVE))
	{
		GetMessage(&msg, NULL, 0, 0);
		if (msg.message == WM_PAINT)
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}
}

void CMainView::SetViewingCtrl() {
	
		RECT curRect;
		m_ctrlTMView->GetWindowRect(&curRect);
		if(curRect.top <= -1 * (m_ScreenResolution.bottom / 3)) { // 50%
			curIndexView = 2;
		} else if(curRect.top > (m_ScreenResolution.bottom / 3)) { // 50%
			curIndexView = 0;
		}

		int lastIndexView = curIndexView;
		// reorder views to mimic continuous pages view
		if(curIndexView == 0) {
			// pan down
			RECT lastRect,
				nextRect;

			CTm_view *tmpVu = m_arrTmView[2];
			m_arrTmView[2] = m_arrTmView[1];
			m_arrTmView[1] = m_arrTmView[0];
			m_arrTmView[0] = tmpVu;

			bool tmpHasPage = hasPage[2];
			hasPage[2] = hasPage[1];
			hasPage[1] = hasPage[0];
			hasPage[0] = tmpHasPage;

			m_ctrlTMView = m_arrTmView[0];
			int loopLimit = 3;
			if(!hasPage[0])
				loopLimit = 2;
			for(int i = 0; i < loopLimit; i++)
				if(IsPrevPageAvailable()) {
					OnPreviousPage();
					hasPage[0] = true;
				} else {
					hasPage[0] = false;
					break;
				}

			m_bGestureHandled = TRUE;
			curPageNavCount--;

		} else if(curIndexView == 2) {
			// pan up
			RECT lastRect,
				nextRect;

			CTm_view *tmpVu = m_arrTmView[0];
			m_arrTmView[0] = m_arrTmView[1];
			m_arrTmView[1] = m_arrTmView[2];
			m_arrTmView[2] = tmpVu;

			bool tmpHasPage = hasPage[0];
			hasPage[0] = hasPage[1];
			hasPage[1] = hasPage[2];
			hasPage[2] = tmpHasPage;

			m_ctrlTMView = m_arrTmView[2];
			m_ctrlTMView->ResetZoom(TMV_ACTIVEPANE);
			int loopLimit = 3;
			if(!hasPage[2])
				loopLimit = 2;
			for(int i = 0; i < loopLimit; i++)
				if(IsNextPageAvailable()) {
					OnNextPage();
					hasPage[2] = true;
				} else {
					hasPage[2] = false;
					break;
				}

			m_bGestureHandled = TRUE;
			curPageNavCount++;

		} // else no page change, do nothing
			
		if(lastIndexView == 0) {
			if(hasPage[0]) {

				if(zoomFullWidth) {
					m_arrTmView[0]->ZoomFullWidth(TMV_ACTIVEPANE);
				}

				for(vector<float>::iterator scale=scaleHist.begin();
					scale != scaleHist.end(); scale++)
						m_arrTmView[0]->DoGestureZoomBottom(*scale);
			}
		} else if(lastIndexView == 2) {
			if(hasPage[2]) {

				if(zoomFullWidth) {
					m_arrTmView[2]->ZoomFullWidth(TMV_ACTIVEPANE);
				}

				for(vector<float>::iterator scale=scaleHist.begin();
					scale != scaleHist.end(); scale++)
						m_arrTmView[2]->DoGestureZoomTop(*scale);
			}
		}

		curIndexView = 1;
		m_ctrlTMView = m_arrTmView[curIndexView];

	RECT wndRect;
	m_arrTmView[1]->GetWindowRect(&wndRect);
	int diff = wndRect.top;
	
	/*int scrollDist = m_ScreenResolution.bottom / 100;
	for(int i=0; i < abs(diff); i+=scrollDist) {

		RECT rect;
		if(diff > 0) { // scroll Up
			
			ScrollWindow(0,-scrollDist);

		} else { // scroll Down

			ScrollWindow(0, scrollDist);
		}

		UpdateWindow();
	}

	m_arrTmView[1]->GetWindowRect(&wndRect);
	diff = wndRect.top;*/
	if(diff) {
		ScrollWindow(0, -diff);
		UpdateWindow();
	}

	m_arrTmView[0]->MoveWindow(0, -1 * (m_ScreenResolution.bottom + PAGES_MARGIN), m_ScreenResolution.right, m_ScreenResolution.bottom);
	m_arrTmView[2]->MoveWindow(0,  1 * (m_ScreenResolution.bottom + PAGES_MARGIN), m_ScreenResolution.right, m_ScreenResolution.bottom);

	EmptyMessageQueue();

	if(toolbarForcedHidden) {
		RECT wndRect;
		m_pToolbar->GetWindowRect(&wndRect);
		wndRect.top = m_ScreenResolution.bottom;
		wndRect.left = 0;
		m_pToolbar->MoveWindow(&wndRect);
		SetControlBar(CONTROL_BAR_TOOLS);
		toolbarForcedHidden = false;
	}

	scrollUpDownInProgress = false;
}

//==============================================================================
//
// 	Function Name:	CMainView::HandlePan()
//
// 	Description:	Handle all pan gesture functions including paning, swipe,
//					keyboard and toolbox display in presentation mode.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::HandlePan(GESTUREINFO gi)
{
	POINTS				pCurrent;					// new point
	int					iMonitor_width;
	int					iMonitor_height;
	HWND				hwnd;
	int					iDistX;
	int					iDistY;
	DWORD               dwCurrentTime;
	long                lTimeInterval;

	if (m_bGestureHandled)
		return;	

	pCurrent.x = gi.ptsLocation.x;
	pCurrent.y = gi.ptsLocation.y;

	// we find the distance between this point and point where gesture was started
	iDistX = pCurrent.x - m_gestureStartPoint.x;
	iDistY = pCurrent.y - m_gestureStartPoint.y;

	// get screen resolution details
	// if orentaion is not an issue, we can find this once on the start of app or presentaion
	iMonitor_width = GetSystemMetrics(SM_CXSCREEN);
	iMonitor_height = GetSystemMetrics(SM_CYSCREEN);

	dwCurrentTime = GetTickCount();
	lTimeInterval = dwCurrentTime - m_gestureStartTime;

	if(!scrollUpDownInProgress) {

		// 4. Swipe down from top of the screen to bring up the keyboard icon; opposite gesture hides keyboard

		// gesture starts at top of monitor. that mean y should be around 0
		// we setting the limit for this gesture within the top 12% of screen
		if (m_gestureStartPoint.y <= iMonitor_width/8) {
			if (abs(iDistY) < iMonitor_width/8) {
				DisplayKeyboardIconGesture(pCurrent);
			}
			//m_bGestureHandled = TRUE;
			// update last location
			m_gestureLastPoint = pCurrent;

			SetViewingCtrl();

			return;
		}


		// 5. Swipe up from the bottom of the screen to bring up tool bar; opposite gesture hides toolbar

		// gesture starts at bottom of monitor. that mean y should be around monitor height
		// we setting the limit for this gesture within the bottom 12% of screen
		if (m_gestureStartPoint.y >= (iMonitor_height - iMonitor_width/8)) {
			if (abs(iDistY) < iMonitor_width/8) {
				DisplayToolbarGesture(pCurrent);
			}
			//m_bGestureHandled = TRUE;
			// update last location
			m_gestureLastPoint = pCurrent;

			SetViewingCtrl();

			return;
		}


		// 3. Swipe right to left to advance to the next page and swipe left to right to go to the previous page

		// check for inertia
		// check if distnace between stating and ending point is greater than 1/4 of screen
		// check time interval to find if its pan or swipe
		if ((abs(iDistX) > iMonitor_width/4 && abs(iDistY) < iMonitor_height/8) && 
			gi.dwFlags == GF_INERTIA && lTimeInterval < 600) {
			// use keyboard arrow key to navigate to next "slide"
			// next page will take to next page insted of slide
			if (iDistX > 0) {
				// swipe was made from left to right
				//OnPreviousPage();
				BYTE keyState[256];
				// Simulate a key press
				keybd_event( VK_LEFT, 0x4B, KEYEVENTF_EXTENDEDKEY | 0, 0);
				// Simulate a key release
				keybd_event( VK_LEFT, 0x4B, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP,0);

			} else {
				//OnNextPage();
				BYTE keyState[256];
				// Simulate a key press
				keybd_event( VK_RIGHT, 0x4D, KEYEVENTF_EXTENDEDKEY | 0, 0);
				// Simulate a key release
				keybd_event( VK_RIGHT, 0x4D, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP,0);
			}
			m_bGestureHandled = TRUE;
			// update last location
			m_gestureLastPoint = pCurrent;

			//etViewingCtrl();

			return;
		}
	}

	if(m_sState != S_DOCUMENT) return;

	// 1. Moving the page with your finger on the screen; similar to what happens now when grabbing and moving the page with the mouse button
	//	Toggle the visibility of the toolbar

	bool *bSmooth = new bool;
	*bSmooth = false;

	if(!loadNextInOtherPanes) {

		m_ctrlTMView = m_arrTmView[0];
		LoadMedia(g_pMedia, g_lSecondary, g_lTertiary);
		
		if(countFrom == COUNT_FROM_FIRST) {
			OnFirstPage();
		} else if(countFrom == COUNT_FROM_LAST) {
			OnLastPage();
		}

		if(IsPrevPageAvailable()) {		
			OnPreviousPage();
			hasPage[0] = true;

		} else {
			hasPage[0] = false;
		}

		m_ctrlTMView = m_arrTmView[2];
		LoadMedia(g_pMedia, g_lSecondary, g_lTertiary);

		if(countFrom == COUNT_FROM_FIRST) {
			OnFirstPage();
		} else if(countFrom == COUNT_FROM_LAST) {
			OnLastPage();
		}

		if(IsNextPageAvailable()) {
			OnNextPage();
			hasPage[2] = true;

		} else {
			hasPage[2] = false;
		}
			
		for(int j = 0; j < SZ_ARR_TM_VW; j++) {
			if(j==1) continue;
		
			m_ctrlTMView = m_arrTmView[j];
			for(int i = 0; i < abs(curPageNavCount); i++) {
				if(curPageNavCount > 0) { // +ve

					if(j == 0 && i == 0 && !hasPage[j]) {
						hasPage[j] = true;
						continue;
					}

					if(IsNextPageAvailable()) {
						OnNextPage();
						hasPage[j] = true;
					} else {
						hasPage[j] = false;
						break;
					}
				} else {

					if(j == SZ_ARR_TM_VW - 1 && i == 0 && !hasPage[j]) {
						hasPage[j] = true;
						continue;
					}

					if(IsPrevPageAvailable()) {
						OnPreviousPage();
						hasPage[j] = true;
					} else {
						hasPage[j] = false;
						break;
					}
				}
			}
		}

		if(hasPage[0]) {
			
			if(zoomFullWidth) {
				m_arrTmView[0]->ZoomFullWidth(TMV_ACTIVEPANE);
			}

			for(vector<float>::iterator scale=scaleHist.begin();
				scale != scaleHist.end(); scale++)
					m_arrTmView[0]->DoGestureZoomBottom(*scale);
		}
	
		if(hasPage[2]) {
			
			if(zoomFullWidth) {
				m_arrTmView[2]->ZoomFullWidth(TMV_ACTIVEPANE);
			}
			
			for(vector<float>::iterator scale=scaleHist.begin();
				scale != scaleHist.end(); scale++)
					m_arrTmView[2]->DoGestureZoomTop(*scale);	
		}
	
		m_ctrlTMView = m_arrTmView[1];
		loadNextInOtherPanes = true;

	}
		
	if(scrollUpDownInProgress ||
		!m_ctrlTMView->DoGesturePan(pCurrent.x, pCurrent.y, m_gestureLastPoint.x, m_gestureLastPoint.y, bSmooth)) {
		
		RECT wndRect;
		m_ctrlTMView->GetWindowRect(&wndRect);
		int top = wndRect.top;

		int diff = pCurrent.y - m_gestureLastPoint.y;

		// pan or not
		if(diff < 0) {
			// pan down
			if(!hasPage[2] && top <= 0) 
				diff = 0;
			else
				diff = -1 * min(abs(diff), m_ScreenResolution.bottom);

			if(top - diff <= -m_ScreenResolution.bottom) {
				diff = 0; //-1 * (m_ScreenResolution.bottom - abs(top));
				m_bGestureHandled = TRUE;
			}

		} else if(diff > 0) {
			// pan up
			if(!hasPage[0] && top >= 0) 
				diff = 0;
			else
				diff = min(abs(diff), m_ScreenResolution.bottom);

			if(top + diff >= m_ScreenResolution.bottom) {
				diff = 0; //m_ScreenResolution.bottom - top;
				m_bGestureHandled = TRUE;
			}
		}

		if(!scrollUpDownInProgress &&
			abs(diff) < m_ScreenResolution.bottom / 10) return;

		if(diff != 0) {

			ScrollWindow(0, diff);
			UpdateWindow();

			scrollUpDownInProgress = true;
		}
	}

	delete bSmooth;
	bSmooth = NULL;
	

	// update last location
	m_gestureLastPoint = pCurrent;
}

//==============================================================================
//
// 	Function Name:	CMainView::HandleZoom()
//
// 	Description:	Handle zooming gesture in presentation mode.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::HandleZoom(GESTUREINFO gi)
{
	// 2. Pinch to zoom and un-pinch to expand the image

	// ullArguments is distance between two points
	// we save the first distance
	if (gi.dwFlags == GF_BEGIN) {
		m_ullArguments = gi.ullArguments;
	}
	else {

		// zoom factor
		float scaleNew = (float)gi.ullArguments / (float)m_ullArguments;

		if(scaleNew != 1.0) {
			m_arrTmView[0]->DoGestureZoomBottom(scaleNew);
			m_arrTmView[1]->DoGestureZoom(scaleNew);
			m_arrTmView[2]->DoGestureZoomTop(scaleNew);
			scaleHist.push_back(scaleNew);
		}

		// update current point and distance
		m_gestureLastPoint = gi.ptsLocation;
		m_ullArguments = gi.ullArguments;

	}
}

//==============================================================================
//
// 	Function Name:	CMainView::DisplayKeyboardIconGesture()
//
// 	Description:	Will display/hide keyborad icon during presentation on
//					gesture. If there is no icon, it will create one.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::DisplayKeyboardIconGesture(POINTS pCurrent)
{
	if (m_pVKBDlg) {
		/*
		CString temp;
		temp.Format("pCurrent.y=%hd , m_gestureLastPoint.y=%hd", pCurrent.y, m_gestureLastPoint.y);
		LogMe((LPCSTR)temp);
		*/
		// if y is increasing, its swipe down
		// the -+3 "jogaar" is to limit the effect if change gesture 
		// direction when user remove finger at end of gesture 
		if ((pCurrent.y - 3) > m_gestureLastPoint.y) {
			if (!m_pVKBDlg->IsWindowVisible()) {
				m_pVKBDlg->ShowWindow(SW_NORMAL);
			}
		}
		else if ((pCurrent.y + 3) < m_gestureLastPoint.y) {
			if (m_pVKBDlg->IsWindowVisible()) {
				m_pVKBDlg->ShowWindow(SW_HIDE);
			}
		}
	}
	else {
		// in case keyboard icon has not been created
		// same code as in OnCreate()
		CRect bmpRect;

		m_pVKBDlg = new CVKBDlg(this);
		m_pVKBDlg->Create(CVKBDlg::IDD);
		m_pVKBDlgPtr = m_pVKBDlg;
		m_pVKBDlg->GetClientRect(&bmpRect);
		m_pVKBDlg->MoveWindow(m_ScreenResolution.right - bmpRect.right - kbIconPadding ,  kbIconPadding , bmpRect.right , bmpRect.bottom );

		if((g_hDesktopHook = SetWindowsHookEx(WH_MOUSE_LL, OnDTMouseEvent, NULL, 0)) == NULL)
		{
			//AfxMessageBox("no hook");
			// Sorry, no hook for you...
		}
		
	}
		
}

//==============================================================================
//
// 	Function Name:	CMainView::DisplayToolbarGesture()
//
// 	Description:	Will display/hide toolbar on gesture.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::DisplayToolbarGesture(POINTS pCurrent)
{
	//LogMe("Gesture Even started");

	if (m_pToolbar) {
		//CString str;
		//str.Format("currentY=%hd, lastY=%hd", pCurrent.y, m_gestureLastPoint.y);
		//LogMe((LPCSTR) str);
		// if y is increasing, its swipe down
		// the -+3 "jogaar" is to limit the effect if change gesture 
		// direction when user remove finger at end of gesture
		if ((pCurrent.y -3) > m_gestureLastPoint.y) {
			if(m_pToolbar->IsWindowVisible()) {
				SetControlBar(CONTROL_BAR_NONE);
				toolbarForcedHidden = false;
			}
		}
		else if ((pCurrent.y + 3) < m_gestureLastPoint.y) {
			if(!m_pToolbar->IsWindowVisible()) {
				SetControlBar(CONTROL_BAR_TOOLS);
			}
		}
	}
	//LogMe("Gesture Event ends");
}

//==============================================================================
//
// 	Function Name:	CMainView::BlankPresentationToolbar()
//
// 	Description:	This fucntion will be called if user lanch a blank
//					presentation.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::BlankPresentationToolbar()
{
	// we are shwoing docmuent toolbar, for now, on blank presentation
	SelectToolbar(S_DOCUMENT);
	SetControlBar(CONTROL_BAR_TOOLS);
}

//==============================================================================
//
// 	Function Name:	CMainView::OnGesturePan()
//
// 	Description:	Called on Gesture Pan button click
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnGesturePan() 
{
	if(!IsCommandEnabled(TMAX_GESTURE_PAN))
		return;
	
	// enable gesture configration
	for(int i=0; i < SZ_ARR_TM_VW; i++)
		m_arrTmView[i]->SetAction(TMAX_NOCOMMAND);
	//m_ctrlTMView->SetAction(TMAX_NOCOMMAND);

	// re-setting gestures pan configration
	CGestureConfig config;
	// setting gestures on pan
				DWORD panWant = GC_PAN
							  | GC_PAN_WITH_SINGLE_FINGER_VERTICALLY
							  | GC_PAN_WITH_SINGLE_FINGER_HORIZONTALLY
							  | GC_PAN_WITH_INERTIA;
				config.EnablePan(TRUE,panWant);
				config.EnableZoom();
	SetGestureConfig(&config);
}


//==============================================================================
//
// 	Function Name:	CMainView::DisbaleGestureOnCommand()
//
// 	Description:	This function checks if gesture should be disabled
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::DisableGestureOnCommand(short sCommand)
{
	// re-setting gestures pan configration
	switch(sCommand)
	{
		case TMAX_CALLOUT:
		case TMAX_DRAWTOOL:	
		case TMAX_HIGHLIGHT:
		case TMAX_REDACT:
		case TMAX_SELECT:
		case TMAX_ZOOM:
		case TMAX_ZOOMRESTRICTED:
		case TMAX_FREEHAND:
		case TMAX_LINE:
		case TMAX_ARROW:
		case TMAX_ELLIPSE:
		case TMAX_RECTANGLE:
		case TMAX_FILLEDELLIPSE:
		case TMAX_FILLEDRECTANGLE:
		case TMAX_POLYLINE:
		case TMAX_POLYGON:
		case TMAX_ANNTEXT:
		case TMAX_PAN:
			{
				CGestureConfig config;
				config.EnablePan(FALSE);
				config.EnableZoom(FALSE);
				SetGestureConfig(&config);
			}
	}	
}


//==============================================================================
//
// 	Function Name:	CMainView::LogMe()
//
// 	Description:	Logging method for debugging
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::LogMe(LPCTSTR msg)
{
	#ifdef _DEBUG

		CString strFileName = "TrialMaxDebugLog.txt";
		FILE*	pFile = NULL;
		CString	strTime = CTime::GetCurrentTime().Format("%m-%d-%Y %H:%M:%S");

		if(fopen_s(&pFile, strFileName, "at") == 0)
		{		
			fprintf(pFile, "%s %s\n", strTime, msg);
			fflush(pFile);
			fclose(pFile);
		}

	#endif //  _DEBUG
}


//==============================================================================
//
// 	Function Name:	CMainView::OnOpenBinder()
//
// 	Description:	This method is used to open binder at first state. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnOpenBinder()
{
	if(m_bIsBinderOpen == FALSE)
	{		
		m_bIsBinderOpen = TRUE;
		
		SetBinderPosition(); 	
		OpenBinder();
	}
	else
	{
		m_BinderList->OnCancel();
		m_bIsBinderOpen = FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::OnOpenBinder()
//
// 	Description:	This method is used to open binder at first state by providing parentId 
//					if it is not provided by default it is zero(0). 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OpenBinder(int parentId)
 {
	 if((m_pDatabase == 0) || (m_pDatabase->IsOpen() == FALSE))
		return;
	 
	 if(m_currentBinderItem.m_AutoId > 0)
	 {
		OnBinderDialogButtonClickEvent(m_currentBinderItem);		
	 }
	 else
	 {	
		list<CBinderEntry> binderEntryList = m_pDatabase->GetBinderEntryByParentId(parentId);
		OpenBinder(binderEntryList);
	 }
}

//==============================================================================
//
// 	Function Name:	CMainView::OnBinderDialogButtonClickEvent()
//
// 	Description:	This method is used to handle button click event of Binder List Dialog. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnBinderDialogButtonClickEvent(CBinderEntry pBinderEntry)
{
	m_bIsBinderOpen = TRUE; // this is mark to true over here because now we have closed binder on inActive

	if((m_pDatabase == 0) || (m_pDatabase->IsOpen() == FALSE))
		return;
	
	m_currentBinderItem = pBinderEntry;
	int pButtonId = pBinderEntry.m_AutoId;

	switch(pBinderEntry.m_TableType)
	{
	case CBinderEntry::TableType::Primary:					
		if(pBinderEntry.m_Children > 0)
		{
			// Call in secondary for childrens
			list<CBinderEntry> secondaryMediaList = m_pDatabase->GetSecondaryMediaByPrimaryMediaId(pButtonId);			
			CBinderEntry parentBinder = m_pDatabase->GetBinderEntryByAutoId(pBinderEntry.m_ParentId);
			OpenBinderList(secondaryMediaList, pButtonId, parentBinder);			
		}
		else
		{
			// Blank Folder or show Presentation			
			m_currentBinderItem = m_pDatabase->GetBinderEntryByAutoId(pBinderEntry.m_ParentId);
			if(m_currentBinderItem.m_MediaType > 4)
			{
				LoadFromBarcode(pBinderEntry.m_Name,TRUE,FALSE);	
				loadNextInOtherPanes = false;
				curPageNavCount = 0;
				countFrom = COUNT_FROM_CUR;
				scaleHist.clear();
				zoomFullWidth = false;
				m_bIsBinderOpen = FALSE;
			}
			else
				OpenBinderList(list<CBinderEntry>(),m_currentBinderItem.m_AutoId);
		}
		break;		

	case CBinderEntry::TableType::Secondary:
		if(pBinderEntry.m_Children > 0)
		{
			// Call in tertiary for childrens
			list<CBinderEntry> tertiaryMediaList = m_pDatabase->GetTertiaryMediaBySecondaryId(pButtonId);
			OpenBinderList(tertiaryMediaList, pButtonId);
		}
		else
		{
			// Blank Folder or show Presentation			
			m_currentBinderItem = m_pDatabase->GetSecondaryMediaById(pButtonId);
			m_currentBinderItem = m_pDatabase->GetPrimaryMediaById(m_currentBinderItem.m_ParentId);
			CString mediaId;
			mediaId.Format("%ld", m_currentBinderItem.m_AutoId);
			CBinderEntry binderItemForParent = m_pDatabase->GetBinderEntryByMediaId(mediaId);
			if(binderItemForParent.m_AutoId == 0)
			{
				m_currentBinderItem = m_pDatabase->GetBinderEntryFromSearchMediaId(mediaId);
				m_currentBinderItem = m_pDatabase->GetBinderEntryByAutoId(m_currentBinderItem.m_ParentId);
			}
			else
			{
				m_currentBinderItem.m_ParentId = binderItemForParent.m_ParentId;
			}
			//m_currentBinderItem = m_parentBinderItem;
			LoadFromBarcode(pBinderEntry.m_Name,TRUE,FALSE);
			loadNextInOtherPanes = false;
			curPageNavCount = 0;
			countFrom = COUNT_FROM_CUR;
			scaleHist.clear();
			zoomFullWidth = false;
			m_bIsBinderOpen = FALSE;
		}
		break;

	case CBinderEntry::TableType::Tertiary:
		if(pBinderEntry.m_Children > 0)
		{
			// Call in Quaternary for childrens
			list<CBinderEntry> quarternaryMediaList = m_pDatabase->GetQuarternaryMediaByTertiaryId(pButtonId);
			OpenBinderList(quarternaryMediaList, pButtonId);
		}
		else
		{
			// Blank Folder or show Presentation		
				m_currentBinderItem = m_pDatabase->GetTertiaryMediaById(pButtonId);
				m_currentBinderItem = m_pDatabase->GetSecondaryMediaById(m_currentBinderItem.m_ParentId);	
				
				CString mediaId;
				mediaId.Format("%ld",m_currentBinderItem.m_ParentId);
				CBinderEntry binderItemForParent = m_pDatabase->GetBinderEntryByMediaId(mediaId);
				if(binderItemForParent.m_AutoId == 0)
				{
					m_currentBinderItem = m_pDatabase->GetBinderEntryFromSearchMediaId(mediaId);
					m_currentBinderItem = m_pDatabase->GetBinderEntryByAutoId(m_currentBinderItem.m_ParentId);
				}	
						
			LoadFromBarcode(pBinderEntry.m_Name,TRUE,FALSE);
			loadNextInOtherPanes = false;
			curPageNavCount = 0;
			countFrom = COUNT_FROM_CUR;
			scaleHist.clear();
			zoomFullWidth = false;
			m_bIsBinderOpen = FALSE;
		}
		break;

	case CBinderEntry::TableType::Quaternary:
		// Blank Folder or show Presentation
		m_currentBinderItem = m_pDatabase->GetQuarternaryMediaById(m_currentBinderItem.m_AutoId);
		m_currentBinderItem = m_pDatabase->GetTertiaryMediaById(m_currentBinderItem.m_ParentId);		
		LoadFromBarcode(pBinderEntry.m_Name,TRUE,FALSE);
		loadNextInOtherPanes = false;
		curPageNavCount = 0;
		countFrom = COUNT_FROM_CUR;
		scaleHist.clear();
		zoomFullWidth = false;
		m_bIsBinderOpen = FALSE;
		break;

	case CBinderEntry::TableType::Binder:
		BinderListAsBinder(pBinderEntry);
		break;
	}	
}

//==============================================================================
//
// 	Function Name:	CMainView::OnBinderDialogCloseButtonClickEvent()
//
// 	Description:	This method is used to handle close button click event of Binder List Dialog. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnBinderDialogCloseButtonClickEvent()
{
	m_bIsBinderOpen = FALSE;
}

//==============================================================================
//
// 	Function Name:	CMainView::OnBinderDialogBackButtonClickEvent()
//
// 	Description:	This method is used to handle back button click event of Binder List Dialog. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnBinderDialogBackButtonClickEvent(CBinderEntry pBinderEntry)
{
	m_bIsBinderOpen = TRUE;
	if((m_pDatabase == 0) || (m_pDatabase->IsOpen() == FALSE))
		return;

	int pButtonId = pBinderEntry.m_AutoId; 
	if(pButtonId > 0)
	{
		CBinderEntry binderEntry = m_pDatabase->GetBinderEntryByAutoId(pButtonId);
		CMainView::OnBinderDialogButtonClickEvent(pBinderEntry);
	}
	else
	{
		m_currentBinderItem.m_AutoId = 0;
		OpenBinder(pButtonId);
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::BinderListAsBinder()
//
// 	Description:	This method is used to handle BinderEntry Implementation
//					if found the record in the BinderEntries Table			
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::BinderListAsBinder(CBinderEntry pBinderEntry)
{
	int pButtonId = pBinderEntry.m_AutoId;		

		if(pBinderEntry.m_Attributes == 0)
		{
			list<CBinderEntry> binderEntryChildList;
			list<CBinderEntry> binderEntryList = m_pDatabase->GetBinderEntryByParentId(pButtonId);		
	
			// with attribute == 1
			list<CBinderEntry> binderEntryListForPrimaryMedia = m_pDatabase->GetBinderEntryByParentIdWithAttribute(pButtonId);
			list<CBinderEntry>::iterator primaryMediaEntry;
			for(primaryMediaEntry = binderEntryListForPrimaryMedia.begin(); primaryMediaEntry != binderEntryListForPrimaryMedia.end(); ++primaryMediaEntry)
			{
				binderEntryChildList.push_back(CMainView::FilterMediaByMediaId(*primaryMediaEntry,pBinderEntry.m_AutoId));
			}


			// fill complete list from above two lists and assign it to BinderList
			list<CBinderEntry> completeBinderList;
			if(binderEntryList.size() > 0)
			{
				list<CBinderEntry>::iterator parentBinders;
				for(parentBinders = binderEntryList.begin(); parentBinders != binderEntryList.end(); ++parentBinders)
				{
					completeBinderList.push_back(*parentBinders);
				}		
			}

			if(binderEntryChildList.size() > 0)
			{
				list<CBinderEntry>::iterator childerBinders;
				for(childerBinders = binderEntryChildList.begin(); childerBinders != binderEntryChildList.end(); ++childerBinders)
				{
					completeBinderList.push_back(*childerBinders);
				}
			}

			if(completeBinderList.size() > 0)
			{	
				CBinderEntry parentBinder = m_pDatabase->GetBinderEntryByAutoId(pBinderEntry.m_ParentId);
				OpenBinderList(completeBinderList, pButtonId, parentBinder);
			}
			else
			{
				OpenBinderList(list<CBinderEntry>(), pButtonId);
			}
		}
		else if(pBinderEntry.m_Attributes == 1)
		{			
			CBinderEntry binderEntry = FilterMediaByMediaId(pBinderEntry, pBinderEntry.m_ParentId);
			OnBinderDialogButtonClickEvent(binderEntry);
		}
}

//==============================================================================
//
// 	Function Name:	CMainView::FilterMediaByMediaId()
//
// 	Description:	This method is used to get data	from Primary or secondary media table
//					based on the media id
//
// 	Returns:		CBinderEntry
//
//	Notes:			None
//
//==============================================================================
CBinderEntry CMainView::FilterMediaByMediaId(CBinderEntry pBinerEntry, long lParentId)
{
	CString mediaId = pBinerEntry.m_Name;
	CBinderEntry binderEntry;

	if(mediaId.Find(".") > -1)
	{
		// get the dot information from the media id
		int dotCount = 0;
		CString mId = "";
		LPCTSTR changedString = (LPCTSTR)mediaId;
		TCHAR * str = (TCHAR*)changedString;
		TCHAR * pch = _tcstok (str,_T("."));
		while (pch != NULL)
		{  
			pch = _tcstok (NULL, _T("."));
			dotCount++;
			if(pch != NULL)
			mId = (CString)pch;
		}

		if(dotCount == 2)
		{
			// Get From Secondary Media
			binderEntry = m_pDatabase->GetSecondaryMediaById(atol(mId));
		}
		else if(dotCount == 3)
		{
			// Get From Tertiary Media
			binderEntry = m_pDatabase->GetTertiaryMediaById(atol(mId));				
		}			
	}
	else
	{
		binderEntry = m_pDatabase->GetPrimaryMediaByMediaId(mediaId);
		binderEntry.m_ParentId = lParentId;
	}

	m_parentBinderItem = m_pDatabase->GetBinderEntryByAutoId(pBinerEntry.m_ParentId);
	return binderEntry;
}

//==============================================================================
//
// 	Function Name:	CMainView::ConvertBinderEntry()
//
// 	Description:	This method is used to convert binderentry* to binderentry. 
//
// 	Returns:		CBinderEntry
//
//	Notes:			None
//
//==============================================================================
CBinderEntry CMainView::ConvertBinderEntry(CBinderEntry* pBinderEntry)
{
	CBinderEntry binderEntry;
	if(pBinderEntry->m_AutoId > 0)
	{
		binderEntry.m_AutoId = pBinderEntry->m_AutoId;
		binderEntry.m_ParentId = pBinderEntry->m_ParentId;
		binderEntry.m_Path = pBinderEntry->m_Path;
		binderEntry.m_Children = pBinderEntry->m_Children;
		binderEntry.m_Attributes = pBinderEntry->m_Attributes;
		binderEntry.m_Name = pBinderEntry->m_Name;
		binderEntry.m_Description = pBinderEntry->m_Description;
		binderEntry.m_DisplayOrder = pBinderEntry->m_DisplayOrder;
		binderEntry.m_CreatedBy = pBinderEntry->m_CreatedBy;
		binderEntry.m_CreatedOn = pBinderEntry->m_CreatedOn;
		binderEntry.m_ModifiedBy = pBinderEntry->m_ModifiedBy;
		binderEntry.m_ModifiedOn = pBinderEntry->m_ModifiedOn;
		binderEntry.m_SpareText = pBinderEntry->m_SpareText;
		binderEntry.m_SpareNumber = pBinderEntry->m_SpareNumber;
		binderEntry.m_TableType = pBinderEntry->m_TableType;
	}
	return binderEntry;
}

//==============================================================================
//
// 	Function Name:	CMainView::CreateBinder()
//
// 	Description:	This method is used to Initiate Binder List Dialog. 
//
// 	Returns:		CBinderEntry
//
//	Notes:			None
//
//==============================================================================
CBinderList* CMainView::CreateBinder(list<CBinderEntry> pBinderEntryList, BOOL bIsShowBackButton)
{		
	m_BinderList = new CBinderList(this, pBinderEntryList.size());
			
	long topDistance = m_BinderListPosition.y;
	long leftDistance = m_BinderListPosition.x;	
	
	m_BinderList->m_nXPosition = leftDistance;	
	m_BinderList->m_nYPosition = topDistance;
	m_BinderList->m_bIsShowBackButton = bIsShowBackButton;
	m_BinderList->m_binderEntryList = pBinderEntryList;
	m_BinderList->Create(IDD_BINDER_LIST);
	m_BinderList->ShowWindow(SW_SHOW);
		
	return m_BinderList;
}

//==============================================================================
//
// 	Function Name:	CMainView::OpenBinderList()
//
// 	Description:	This method is used to Initiate Binder List Dialog. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OpenBinderList(list<CBinderEntry> pBinderEntryList, int pButtonId, CBinderEntry pParentBinder)
{
	BOOL isShowBackButton = FALSE;

	if(pButtonId > 0)
		isShowBackButton = TRUE;
	
	
	m_BinderList = new CBinderList(this, pBinderEntryList.size());
		
	long topDistance = m_BinderListPosition.y; 
	long leftDistance =  m_BinderListPosition.x; 	
	
	m_BinderList->m_nXPosition = leftDistance;	
	m_BinderList->m_nYPosition = topDistance;	
	m_BinderList->m_bIsShowBackButton = isShowBackButton;
	m_BinderList->m_binderEntryList = pBinderEntryList;
	m_BinderList->m_parentBinder = pParentBinder;
	m_BinderList->Create(IDD_BINDER_LIST);
	m_BinderList->ShowWindow(SW_SHOW);
}

//==============================================================================
//
// 	Function Name:	CMainView::OpenBinder()
//
// 	Description:	This method is used to Create BinderEntry List for Binder List Dialog. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OpenBinder(list<CBinderEntry> binderEntryList)
 {	
	if(binderEntryList.size() > 0)
	{		
		CBinderList* binderList = CMainView::CreateBinder(binderEntryList,FALSE);
		return;
	}

	m_bIsBinderOpen = FALSE;
}

//==============================================================================
//
// 	Function Name:	CMainView::OpenBinderList()
//
// 	Description:	This method is used to Create BinderEntry List for Binder List Dialog. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OpenBinderList(list<CBinderEntry> pBinderEntryList, int pButtonId)
{
	BOOL isShowBackButton = FALSE;

	if(pButtonId > 0)
		isShowBackButton = TRUE;

	CBinderList* binderList = CMainView::CreateBinder(pBinderEntryList,isShowBackButton);	
		
}

//==============================================================================
//
// 	Function Name:	CMainView::SetBinderPosition()
//
// 	Description:	This method is used to Set Position for Binder List Dialog. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::SetBinderPosition()
{
	int width = GetSystemMetrics(SM_CXSCREEN);
	int height = GetSystemMetrics(SM_CYSCREEN);
	int binderListWidth = 148; // it is adjusted according to buttons width in binderListDialog it may not be the same.	

	int barHeight = m_pToolbar->GetBarHeight();
	int barXPosition = m_pToolbar->GetBarXPosition();		
	int buttonWidth =  m_pToolbar->GetButtonActualWidth();
	int buttonXPosition =  m_pToolbar->GetButtonXPosition(82);	
	int actualXPosition = buttonXPosition + barXPosition;
	
	m_BinderListPosition.x = actualXPosition;
	m_BinderListPosition.y = height - barHeight;

	int midPoint = width / 2;
	int halfButtonWidth = buttonWidth / 2;

	if(actualXPosition > (midPoint + halfButtonWidth))
	{
		// Right On The Screen
		m_BinderListPosition.x = m_BinderListPosition.x - binderListWidth + buttonWidth;
		return;

	}
	else if(actualXPosition < (midPoint - buttonWidth))
	{
		// Left On The Screen
		// leave the left handling because it is done
		return;
	}
	else
	{
		// Center On The Screen
		m_BinderListPosition.x = m_BinderListPosition.x - (binderListWidth/2) + halfButtonWidth;
	}

}


//==============================================================================
//
// 	Function Name:	CMainView::OnOpenColorPicker()
//
// 	Description:	This method is used to set position and open color picker. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnOpenColorPicker()
{
	if(m_bIsColorPickerOpen == FALSE)
	{
		m_bIsColorPickerOpen = TRUE;
		SetColorPickerPosition();
		OpenColorPicker();
	}
	else
	{
		m_bIsColorPickerOpen = FALSE;	
		m_ColorPickerList->OnCancel();
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::OpenColorPicker()
//
// 	Description:	This method is used to open Color Picker List Dialog. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OpenColorPicker()
{
	m_ColorPickerList = new CColorPickerList(this);
	m_ColorPickerList->m_nXPosition = m_ColorPickerListPosition.x;
	m_ColorPickerList->m_nYPosition = m_ColorPickerListPosition.y;

	m_ColorPickerList->Create(IDD_COLOR_PICKER_DLG);
	m_ColorPickerList->ShowWindow(SW_SHOW);	
}

//==============================================================================
//
// 	Function Name:	CMainView::OnColorPickerButtonClickEvent()
//
// 	Description:	This method is used to handle click event of Color Picker List Dialog. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::OnColorPickerButtonClickEvent(int iColorType)
{	
	m_bIsColorPickerOpen = TRUE;
		
	switch(iColorType)
	{
		case CColorPickerList::ColorType::BLACK:
			m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::BLACK);
			OnBlack();
			break;

		case CColorPickerList::ColorType::BLUE:
			m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::BLUE);
			OnBlue();
			break;

		case CColorPickerList::ColorType::DARKBLUE:
			m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::DARKBLUE);
			OnDarkBlue();
			break;

		case CColorPickerList::ColorType::DARKGREEN:
			m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::DARKGREEN);
			OnDarkGreen();
			break;

		case CColorPickerList::ColorType::DARKRED:
			m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::DARKRED);
			OnDarkRed();
			break;

		case CColorPickerList::ColorType::GREEN:
			m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::GREEN);
			OnGreen();
			break;

		case CColorPickerList::ColorType::LIGHTBLUE:
			m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::LIGHTBLUE);
			OnLightBlue();
			break;

		case CColorPickerList::ColorType::LIGHTGREEN:
			m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::LIGHTGREEN);
			OnLightGreen();
			break;

		case CColorPickerList::ColorType::LIGHTRED:
			m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::LIGHTRED);
			OnLightRed();
			break;

		case CColorPickerList::ColorType::RED:
			m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::RED);
			OnRed();
			break;

		case CColorPickerList::ColorType::WHITE:
			m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::WHITE);
			OnWhite();
			break;

		case CColorPickerList::ColorType::YELLOW:
			m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::YELLOW);
			OnYellow();
			break;

		default:
			break;
	}

	m_bIsColorPickerOpen = FALSE;

}

void CMainView::OnColorPickerCloseButtonClickEvent()
{
	m_bIsColorPickerOpen = FALSE;	
}

//==============================================================================
//
// 	Function Name:	CMainView::SetColorPickerPosition()
//
// 	Description:	This method is used to Set Position for Color Picker List Dialog. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::SetColorPickerPosition()
{	
	int width = GetSystemMetrics(SM_CXSCREEN);
	int height = GetSystemMetrics(SM_CYSCREEN);
	int colorPickerListWidth = 48;	

	int barHeight = m_pToolbar->GetBarHeight();
	int barXPosition = m_pToolbar->GetBarXPosition();		
	int buttonWidth =  m_pToolbar->GetButtonActualWidth();
	int buttonXPosition =  m_pToolbar->GetButtonXPosition(48);	
	int actualXPosition = buttonXPosition + barXPosition;
	
	m_ColorPickerListPosition.x = actualXPosition;
	m_ColorPickerListPosition.y = height - barHeight;

	if(buttonWidth > 40) // if large button
	{
		// adjusting the position of list to appear in the center of button
		m_ColorPickerListPosition.x = m_ColorPickerListPosition.x + 5;
		m_ColorPickerListPosition.y = m_ColorPickerListPosition.y - 2;
	}
}

//==============================================================================
//
// 	Function Name:	CMainView::ChangeColorOfColorButton()
//
// 	Description:	This function will update the button color with the color
//					previous set for button on selection of tool
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CMainView::ChangeColorOfColorButton(short sColorToChange)
{
	if(sColorToChange == TMV_RED)			
		m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::RED);
	else if(sColorToChange == TMV_GREEN)
		m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::GREEN);		
	else if(sColorToChange == TMV_BLUE)
			m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::BLUE);
	else if(sColorToChange == TMV_YELLOW)
			m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::YELLOW);			
	else if(sColorToChange == TMV_DARKRED)
			m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::DARKRED);
	else if(sColorToChange == TMV_DARKGREEN)
			m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::DARKGREEN);
	else if(sColorToChange == TMV_DARKBLUE)
			m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::DARKBLUE);
	else if(sColorToChange == TMV_LIGHTRED)
			m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::LIGHTRED);
	else if(sColorToChange == TMV_LIGHTGREEN)
			m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::LIGHTGREEN);
	else if(sColorToChange == TMV_LIGHTBLUE)
			m_pToolbar->SetButtonImage(48,CColorPickerList::ColorType::LIGHTBLUE);
}
