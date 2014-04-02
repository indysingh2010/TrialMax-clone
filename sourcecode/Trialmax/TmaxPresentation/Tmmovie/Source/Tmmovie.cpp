//==============================================================================
//
// File Name:	tmmovie.cpp
//
// Description:	This file contains member functions of the CTMMovieCtrl class.
//
//
// See Also:	tmmovie.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	06-12-98	1.00		Original Release
//	03-12-99	3.00		Added total time and elapsed time counters
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <toolbox.h>
#include <tmmovie.h>
#include <tmovieap.h>
#include <tmoviepg.h>
#include <tmmvdefs.h>
#include <vidprops.h>
#include <filters.h>
#include <evcode.h>
#include <regcats.h>
#include <dispid.h>
#include <registry.h>
#include <filever.h>

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
extern CTMMovieApp NEAR theApp;
CTMMovieCtrl*			pControl = 0;

/* Replace 2 */
const IID BASED_CODE IID_DTMMovie6 =
		{ 0x2199ecb7, 0xd80f, 0x4188, { 0x9f, 0x90, 0x92, 0x93, 0x20, 0xb7, 0x7e, 0x5c } };
/* Replace 3 */
const IID BASED_CODE IID_DTMMovie6Events =
		{ 0x3084c1b8, 0xa25f, 0x4923, { 0x9b, 0x54, 0xd7, 0x3d, 0xeb, 0x76, 0x17, 0x80 } };

// Control type information
static const DWORD BASED_CODE _dwTMMovieOleMisc =
	OLEMISC_ACTIVATEWHENVISIBLE |
	OLEMISC_SETCLIENTSITEFIRST |
	OLEMISC_INSIDEOUT |
	OLEMISC_CANTLINKINSIDE |
	OLEMISC_RECOMPOSEONRESIZE;

//	Object safety interface options
static const DWORD _dwSupportedSafetyOptions = 
	INTERFACESAFE_FOR_UNTRUSTED_CALLER |
	INTERFACESAFE_FOR_UNTRUSTED_DATA;

static const DWORD _dwUnsupportedSafetyOptions = ~_dwSupportedSafetyOptions;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------

// Message map
BEGIN_MESSAGE_MAP(CTMMovieCtrl, COleControl)
	//{{AFX_MSG_MAP(CTMMovieCtrl)
	ON_WM_CREATE()
	ON_WM_SIZE()
	ON_WM_TIMER()
	ON_WM_LBUTTONDBLCLK()
	ON_WM_RBUTTONDBLCLK()
	//}}AFX_MSG_MAP
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
	ON_MESSAGE(WM_ERROR_NOTIFICATION, OnWMErrorNotification)
END_MESSAGE_MAP()

// Dispatch map
BEGIN_DISPATCH_MAP(CTMMovieCtrl, COleControl)
	//{{AFX_DISPATCH_MAP(CTMMovieCtrl)
	DISP_PROPERTY(CTMMovieCtrl, "PlaylistTime", m_dPlaylistTime, VT_R8)
	DISP_PROPERTY(CTMMovieCtrl, "ElapsedDesignation", m_dElapsedDesignation, VT_R8)
	DISP_PROPERTY(CTMMovieCtrl, "ElapsedPlaylist", m_dElapsedPlaylist, VT_R8)
	DISP_PROPERTY(CTMMovieCtrl, "DesignationTime", m_dDesignationTime, VT_R8)
	DISP_PROPERTY_NOTIFY(CTMMovieCtrl, "AutoInit", m_bAutoInit, OnAutoInitChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMMovieCtrl, "EnableErrors", m_bEnableErrors, OnEnableErrorsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMMovieCtrl, "IniFile", m_strIniFile, OnIniFileChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMMovieCtrl, "AutoPlay", m_bAutoPlay, OnAutoPlayChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMMovieCtrl, "ScaleVideo", m_bScaleVideo, OnScaleVideoChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMMovieCtrl, "Filename", m_strFilename, OnFilenameChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMMovieCtrl, "UpdateRate", m_sUpdateRate, OnUpdateRateChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMMovieCtrl, "AutoShow", m_bAutoShow, OnAutoShowChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMMovieCtrl, "KeepAspect", m_bKeepAspect, OnKeepAspectChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMMovieCtrl, "Balance", m_sBalance, OnBalanceChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMMovieCtrl, "Rate", m_sRate, OnRateChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMMovieCtrl, "Volume", m_sVolume, OnVolumeChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMMovieCtrl, "UseSnapshots", m_bUseSnapshots, OnUseSnapshotsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMMovieCtrl, "OverlayFile", m_strOverlay, OnOverlayFileChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMMovieCtrl, "OverlayVisible", m_bOverlayVisible, OnOverlayVisibleChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMMovieCtrl, "StartPosition", m_dStartPosition, OnStartPositionChanged, VT_R8)
	DISP_PROPERTY_NOTIFY(CTMMovieCtrl, "StopPosition", m_dStopPosition, OnStopPositionChanged, VT_R8)
	DISP_PROPERTY_NOTIFY(CTMMovieCtrl, "EnableAxErrors", m_bEnableAxErrors, OnEnableAxErrorsChanged, VT_BOOL)
	DISP_PROPERTY_EX(CTMMovieCtrl, "VerTextLong", GetVerTextLong, SetNotSupported, VT_BSTR)
	DISP_FUNCTION(CTMMovieCtrl, "Unload", Unload, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "Initialize", Initialize, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "Play", Play, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "Pause", Pause, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "Stop", Stop, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "Resume", Resume, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "IsReady", IsReady, VT_BOOL, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "GetState", GetState, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "GetFrameRate", GetFrameRate, VT_R4, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "GetSrcWidth", GetSrcWidth, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "GetSrcHeight", GetSrcHeight, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "ShowVideoProps", ShowVideoProps, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "CheckType", CheckType, VT_I2, VTS_BSTR)
	DISP_FUNCTION(CTMMovieCtrl, "GetPlaylistState", GetPlaylistState, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "GetType", GetType, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "ShowVideo", ShowVideo, VT_EMPTY, VTS_BOOL)
	DISP_FUNCTION(CTMMovieCtrl, "IsVideoVisible", IsVideoVisible, VT_BOOL, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "CanSetVolume", CanSetVolume, VT_BOOL, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "CanSetBalance", CanSetBalance, VT_BOOL, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "CanSetRate", CanSetRate, VT_BOOL, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "IsLoaded", IsLoaded, VT_BOOL, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "Update", Update, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "GetResolution", GetResolution, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "ShowSnapshot", ShowSnapshot, VT_HANDLE, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "Capture", Capture, VT_I2, VTS_BSTR VTS_BOOL)
	DISP_FUNCTION(CTMMovieCtrl, "GetRegFilters", GetRegFilters, VT_BSTR, VTS_PI4)
	DISP_FUNCTION(CTMMovieCtrl, "ShowFilterInfo", ShowFilterInfo, VT_EMPTY, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "GetActFilters", GetActFilters, VT_BSTR, VTS_BOOL VTS_PI4)
	DISP_FUNCTION(CTMMovieCtrl, "GetInterface", GetInterface, VT_UNKNOWN, VTS_I2)
	DISP_FUNCTION(CTMMovieCtrl, "SetDefaultRate", SetDefaultRate, VT_EMPTY, VTS_R8)
	DISP_FUNCTION(CTMMovieCtrl, "GetDefaultRate", GetDefaultRate, VT_R8, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "SetPlaylistRange", SetPlaylistRange, VT_I2, VTS_I4 VTS_I4)
	DISP_FUNCTION(CTMMovieCtrl, "GetClassIdString", GetClassIdString, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "GetRegisteredPath", GetRegisteredPath, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "AddFilter", AddFilter, VT_I2, VTS_BSTR)
	DISP_FUNCTION(CTMMovieCtrl, "RemoveFilter", RemoveFilter, VT_I2, VTS_BSTR)
	DISP_FUNCTION(CTMMovieCtrl, "GetUserFilters", GetUserFilters, VT_BSTR, VTS_PI4)
	DISP_FUNCTION(CTMMovieCtrl, "GetMinTime", GetMinTime, VT_R8, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "GetMaxTime", GetMaxTime, VT_R8, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "GetPosition", GetPosition, VT_R8, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "ConvertToFrames", ConvertToFrames, VT_I4, VTS_R8)
	DISP_FUNCTION(CTMMovieCtrl, "SetMaxCuePosition", SetMaxCuePosition, VT_I2, VTS_R8)
	DISP_FUNCTION(CTMMovieCtrl, "SetMinCuePosition", SetMinCuePosition, VT_I2, VTS_R8)
	DISP_FUNCTION(CTMMovieCtrl, "SetRange", SetRange, VT_I2, VTS_R8 VTS_R8)
	DISP_FUNCTION(CTMMovieCtrl, "Cue", Cue, VT_I2, VTS_I2 VTS_R8 VTS_BOOL)
	DISP_FUNCTION(CTMMovieCtrl, "Load", Load, VT_I2, VTS_BSTR VTS_R8 VTS_R8 VTS_BOOL)
	DISP_FUNCTION(CTMMovieCtrl, "Step", Step, VT_I2, VTS_R8 VTS_R8)
	DISP_FUNCTION(CTMMovieCtrl, "ConvertToTime", ConvertToTime, VT_R8, VTS_I4)
	DISP_FUNCTION(CTMMovieCtrl, "CuePlaylist", CuePlaylist, VT_I2, VTS_I2 VTS_R8 VTS_BOOL VTS_BOOL)
	DISP_FUNCTION(CTMMovieCtrl, "PlayPlaylist", PlayPlaylist, VT_I2, VTS_I4 VTS_I4 VTS_I4 VTS_R8)
	DISP_FUNCTION(CTMMovieCtrl, "CueDesignation", CueDesignation, VT_I2, VTS_I4 VTS_R8 VTS_I2)
	DISP_FUNCTION(CTMMovieCtrl, "GetDuration", GetDuration, VT_R8, VTS_BSTR)
	DISP_FUNCTION(CTMMovieCtrl, "UpdateScreenPosition", UpdateScreenPosition, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMMovieCtrl, "GetIsAudio", GetIsAudio, VT_BOOL, VTS_NONE)
	DISP_STOCKPROP_BACKCOLOR()
	DISP_STOCKPROP_BORDERSTYLE()
	//}}AFX_DISPATCH_MAP

	//	Added rev 5.1
	DISP_PROPERTY_NOTIFY_ID(CTMMovieCtrl, "DetachBeforeLoad", DISPID_DETACH_BEFORE_LOAD, m_bDetachBeforeLoad, OnDetachBeforeLoadChanged, VT_BOOL)
	
	//	Added in rev 5.2
	DISP_PROPERTY_NOTIFY_ID(CTMMovieCtrl, "HideTaskBar", DISPID_HIDE_TASK_BAR, m_bHideTaskBar, OnHideTaskBarChanged, VT_BOOL)

	//	Added in rev 6.1
	DISP_PROPERTY_NOTIFY_ID(CTMMovieCtrl, "EnableSimulation", DISPID_ENABLE_SIMULATION, m_bEnableSimulation, OnEnableSimulationChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY_ID(CTMMovieCtrl, "SimulationText", DISPID_SIMULATION_TEXT, m_strSimulationText, OnSimulationTextChanged, VT_BSTR)

	//	Added in rev 6.1.0
	DISP_PROPERTY_EX_ID(CTMMovieCtrl, "VerBuild", DISPID_VERBUILD, GetVerBuild, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX_ID(CTMMovieCtrl, "VerMajor", DISPID_VERMAJOR, GetVerMajor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX_ID(CTMMovieCtrl, "VerMinor", DISPID_VERMINOR, GetVerMinor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX_ID(CTMMovieCtrl, "VerTextShort", DISPID_VERTEXTSHORT, GetVerTextShort, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_EX_ID(CTMMovieCtrl, "VerQEF", DISPID_VERQEF, GetVerQEF, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX_ID(CTMMovieCtrl, "VerBuildDate", DISPID_VERBUILDDATE, GetVerBuildDate, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_NOTIFY_ID(CTMMovieCtrl, "ShowAudioImage", DISPID_SHOWAUDIOIMAGE, m_bShowAudioImage, OnShowAudioImageChanged, VT_BOOL)

END_DISPATCH_MAP()

// Event map
BEGIN_EVENT_MAP(CTMMovieCtrl, COleControl)
	//{{AFX_EVENT_MAP(CTMMovieCtrl)
	EVENT_CUSTOM("FileChange", FireFileChange, VTS_BSTR)
	EVENT_CUSTOM("StateChange", FireStateChange, VTS_I2)
	EVENT_CUSTOM("PlaylistState", FirePlaylistState, VTS_I2)
	EVENT_CUSTOM("PlaybackError", FirePlaybackError, VTS_I4  VTS_BOOL)
	EVENT_CUSTOM("PlaybackComplete", FirePlaybackComplete, VTS_NONE)
	EVENT_CUSTOM("DebugMessage", FireDebugMessage, VTS_BSTR  VTS_BSTR)
	EVENT_CUSTOM("LineChange", FireLineChange, VTS_I4)
	EVENT_CUSTOM("PlaylistTime", FirePlaylistTime, VTS_R8)
	EVENT_CUSTOM("DesignationTime", FireDesignationTime, VTS_R8)
	EVENT_CUSTOM("ElapsedTimes", FireElapsedTimes, VTS_R8  VTS_R8)
	EVENT_CUSTOM("DesignationChange", FireDesignationChange, VTS_I4  VTS_I4)
	EVENT_CUSTOM("LinkChange", FireLinkChange, VTS_BSTR  VTS_I4  VTS_I4)
	EVENT_CUSTOM("MouseDblClick", FireMouseDblClick, VTS_I2  VTS_XPOS_PIXELS  VTS_YPOS_PIXELS)
	EVENT_CUSTOM("PositionChange", FirePositionChange, VTS_R8)
	EVENT_CUSTOM("AxError", FireAxError, VTS_BSTR)
	EVENT_CUSTOM("AxDiagnostic", FireAxDiagnostic, VTS_BSTR  VTS_BSTR)
	EVENT_STOCK_DBLCLICK()
	EVENT_STOCK_MOUSEDOWN()
	EVENT_STOCK_MOUSEMOVE()
	EVENT_STOCK_MOUSEUP()
	//}}AFX_EVENT_MAP
END_EVENT_MAP()

// Property pages
BEGIN_PROPPAGEIDS(CTMMovieCtrl, 2)
	PROPPAGEID(CTMMovieProperties::guid)
	PROPPAGEID(CLSID_CColorPropPage)
END_PROPPAGEIDS(CTMMovieCtrl)

/* Replace 4 */
IMPLEMENT_OLECREATE_EX(CTMMovieCtrl, "TMMOVIE6.TMMovieCtrl.1",
	0xd71d2494, 0xb9ca, 0x401f, 0x8e, 0x24, 0x18, 0x15, 0xe0, 0x77, 0xce, 0x64)

// Type library ID and version
IMPLEMENT_OLETYPELIB(CTMMovieCtrl, _tlid, _wVerMajor, _wVerMinor)
IMPLEMENT_OLECTLTYPE(CTMMovieCtrl, IDS_TMMOVIE, _dwTMMovieOleMisc)

IMPLEMENT_DYNCREATE(CTMMovieCtrl, COleControl)

// Interface map for IObjectSafety
BEGIN_INTERFACE_MAP(CTMMovieCtrl, COleControl )
	INTERFACE_PART(CTMMovieCtrl, IID_IObjectSafety, ObjSafety)
END_INTERFACE_MAP()

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnActivateInPlace
//
// 	Description:	Added function to overwrite colecontrol::OnActivateInPlace
//
// 	Returns:		HRESULT 
//
//	Notes:			None
//
//==============================================================================
HRESULT CTMMovieCtrl::OnActivateInPlace(BOOL bUIActivate, LPMSG pMsg)
{
  static BOOL bInsideFunc = FALSE;
  if (!bInsideFunc)
  {
     bInsideFunc = TRUE;
     HRESULT hr = COleControl::OnActivateInPlace(bUIActivate, pMsg);
     bInsideFunc = FALSE;
     return hr;
  }
  return S_OK;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::CTMMovieCtrlFactory::UpdateRegistry
//
// 	Description:	Adds or removes system registry entries for this control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTMMovieCtrl::CTMMovieCtrlFactory::UpdateRegistry(BOOL bRegister)
{
	BOOL	bReturn;
	HRESULT	hResult;

	if(bRegister)
	{
		bReturn = AfxOleRegisterControlClass(AfxGetInstanceHandle(),
											 m_clsid,
											 m_lpszProgID,
											 IDS_TMMOVIE,
											 IDB_TMMOVIE,
											 afxRegApartmentThreading,
											 _dwTMMovieOleMisc,
											 _tlid,
											 _wVerMajor,
											 _wVerMinor);

		//	Mark the control as safe for scripting
		hResult = CreateComponentCategory(CATID_SafeForScripting, 
										  L"Controls that are safely scriptable");
		if(SUCCEEDED(hResult))
			RegisterCLSIDInCategory(m_clsid, CATID_SafeForScripting);

		//	Mark as safe for data initialization
		hResult = CreateComponentCategory(CATID_SafeForInitializing, 
										  L"Controls safely initializable from persistent data");
		if(SUCCEEDED(hResult))
			RegisterCLSIDInCategory(m_clsid, CATID_SafeForInitializing);

		return bReturn;
	}
	else
	{
		return AfxOleUnregisterClass(m_clsid, m_lpszProgID);
	}
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::XObjSafety::AddRef()
//
// 	Description:	This function is called to attach to the nested
//					IObjectSafety dispatch interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMMovieCtrl::XObjSafety::AddRef()
{
    METHOD_PROLOGUE(CTMMovieCtrl, ObjSafety)
    return pThis->ExternalAddRef();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::XObjSafety::GetInterfaceSafetyOptions()
//
// 	Description:	This function is called to get this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMMovieCtrl::XObjSafety::GetInterfaceSafetyOptions( 
		/* [in]  */ REFIID riid,
        /* [out] */ DWORD __RPC_FAR *pdwSupportedOptions,
        /* [out] */ DWORD __RPC_FAR *pdwEnabledOptions)
{
	HRESULT			hReturn;
	IUnknown FAR*	pInterface;

	METHOD_PROLOGUE(CTMMovieCtrl, ObjSafety)

	//	Initialize the return value
	hReturn = ResultFromScode(S_OK);

	//	Verify that the interface exists
	hReturn = pThis->ExternalQueryInterface(&riid, (void * *)&pInterface);
	if(hReturn != E_NOINTERFACE) 
		pInterface->Release(); // release it--just checking!
	
	//	Always enable the supported options
	*pdwSupportedOptions = _dwSupportedSafetyOptions;
	*pdwEnabledOptions   = _dwSupportedSafetyOptions;

	return hReturn; // E_NOINTERFACE if pThis->ExternalQueryInterface failed
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::XObjSafety::QueryInterface()
//
// 	Description:	This function is called to query the IObjectSafety interface
//					for the requested method/property.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT FAR EXPORT CTMMovieCtrl::XObjSafety::QueryInterface(REFIID iid, 
														  void FAR* FAR* ppvObj)
{
    METHOD_PROLOGUE(CTMMovieCtrl, ObjSafety)
    return (HRESULT)pThis->ExternalQueryInterface(&iid, ppvObj);
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::XObjSafety::Release()
//
// 	Description:	This function is called to detach from the IObjectSafety
//					dispatch interface.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMMovieCtrl::XObjSafety::Release()
{
    METHOD_PROLOGUE(CTMMovieCtrl, ObjSafety)
    return pThis->ExternalRelease();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::XObjSafety::SetInterfaceSafetyOptions()
//
// 	Description:	This function is called to set this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMMovieCtrl::XObjSafety::SetInterfaceSafetyOptions( 
        /* [in] */ REFIID riid,
        /* [in] */ DWORD dwOptionSetMask,
        /* [in] */ DWORD dwEnabledOptions)
{
	IUnknown FAR* pInterface;

    METHOD_PROLOGUE(CTMMovieCtrl, ObjSafety)
	
	//	Verify that the interface exists
	pThis->ExternalQueryInterface(&riid, (void * *)&pInterface);
	if (pInterface)
		pInterface->Release(); // release it--just checking!
	else
		return ResultFromScode(E_NOINTERFACE);

	//	Make sure we are not attempting to set any options that
	//	we don't support
	if(dwOptionSetMask & _dwUnsupportedSafetyOptions)
		return ResultFromScode(E_FAIL);
	
	//	Make sure we don't clear any options that we do support
	dwEnabledOptions &= _dwSupportedSafetyOptions;
	
	//	We already know there are no extra bits in mask
	if((dwOptionSetMask & dwEnabledOptions) != dwOptionSetMask)
		return ResultFromScode(E_FAIL);
	
	//	Don't need to change anything since we're always safe
	return ResultFromScode(S_OK);
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::AddDebugMessage()
//
// 	Description:	This method is called to add a message to the controls 
//					debug message file
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::AddDebugMessage(LPCSTR lpszFormat, ...)
{
	SYSTEMTIME	sysTime;
	char		szBuffer[1024];
	CString		strDate;
	CString		strTime;
	FILE*		fptr = NULL;

	try
	{
		//	Open the file
		if(m_strDebugFilename.GetLength() > 0)
			fopen_s(&fptr, m_strDebugFilename, "at");
		if(fptr == NULL) return;

		//	Get the current system time
		GetLocalTime(&sysTime);

		//	Declare the variable list of arguements            
		va_list	Arguements;

		//	Insert the first variable arguement into the arguement list
		va_start(Arguements, lpszFormat);

		//	Format the message
		vsprintf_s(szBuffer, sizeof(szBuffer), lpszFormat, Arguements);

		//	Clean up the arguement list
		va_end(Arguements);

		//	Initialize the error
		strDate.Format("%02d-%02d-%04d", sysTime.wMonth, sysTime.wDay, sysTime.wYear);
		strTime.Format("%02d:%02d:%02d", sysTime.wHour, sysTime.wMinute, sysTime.wSecond);
		
		fprintf(fptr, "%s %s %s\n", strDate, strTime, szBuffer);
		fclose(fptr);
	}
	catch(...)
	{
	}

}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::AddFilter()
//
// 	Description:	This method is called to add filter to be added to the
//					graph builder before rendering
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::AddFilter(LPCTSTR lpszName) 
{
	ASSERT(lpszName);

	//	Has the control been initialized ?
	if(!IsReady())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTINITIALIZED);
		return TMMOVIE_NOTINITIALIZED;
	}

	//	Notify the player
	if(m_Player.AddFilter(lpszName))
		return TMMOVIE_NOERROR;
	else
		return TMMOVIE_ADDFILTERFAILED;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::CalcPlaylistStep()
//
// 	Description:	This function will calculate the values required to move the
//					playlist from the position provided by the caller to the
//					relative position defined by dStep.
//
// 	Returns:		None
//
//	Notes:			The new designation and frame are returned in pIndex and
//					dPosition. If unable to calculate the appropriate step, 
//					pIndex will be less than 0
//
//					We intentionally DO NOT use the designation iteration
//					routines provided by the CPlaylist class because we don't
//					want to mess up the POSITION of the playlist while we
//					do this
//
//==============================================================================
void CTMMovieCtrl::CalcPlaylistStep(long* pIndex, double* pPosition, double dStep) 
{
	CDesignations*	pDesignations;
	CDesignation*	pDesignation = 0;
	POSITION		Pos;
	double			dRemaining;
	double			dAvailable;

	ASSERT(pIndex);
	ASSERT(pPosition);
	ASSERT(m_pPlaylist);

	//	Don't even bother if the current index and position are not valid
	if(*pIndex < 0 || *pPosition < 0 || dStep == 0)
		return;

	pDesignations = &(m_pPlaylist->m_Designations);

	//	Are we stepping forward?
	if(dStep > 0)
	{
		dRemaining = dStep;

		//	Iterate the list from the head to tail and line up on the 
		//	designation provided by the caller
		Pos = pDesignations->GetHeadPosition();
		while(Pos != NULL)
		{
			if((pDesignation = (CDesignation*)pDesignations->GetNext(Pos)) == 0)
				continue;

			//	Is this the current designation?
			if(pDesignation->m_lPlaybackOrder == *pIndex)
				break;

		}

		//	Did we find the caller's designation?
		if(!pDesignation || (pDesignation->m_lPlaybackOrder != *pIndex))
		{
			*pIndex = -1;
			*pPosition = 0.0;
			return;
		}

		//	How far can we step forward in the current designation?
		dAvailable = pDesignation->m_dStopTime - *pPosition;

		//	Can we do the full step without switching designations?
		if(dAvailable >= dRemaining)
		{
			*pPosition += dRemaining;
			return;
		}
		else
		{
			//	Move the frame index to the end of this designation just in
			//	case we don't have any more designations in the list
			*pPosition = pDesignation->m_dStopTime;
			dRemaining -= dAvailable;
		}

		//	Keep moving through the designations until we've gone far enough
		//	or we run out
		while(Pos != NULL && dRemaining > 0)
		{
			if((pDesignation = (CDesignation*)pDesignations->GetNext(Pos)) == 0)
				continue;

			//	What is the index of this designation?
			*pIndex = pDesignation->m_lPlaybackOrder;

			//	Line up on the start frame
			*pPosition = pDesignation->m_dStartTime;

			//	How many seconds are available with this designation?
			dAvailable = pDesignation->m_dStopTime - pDesignation->m_dStartTime;

			//	Do we have enough?
			if(dAvailable >= dRemaining)
			{
				*pPosition += dRemaining;
				return;
			}
			else
			{
				//	Move the position to the end of this designation just in
				//	case we don't have any more designations in the list
				*pPosition = pDesignation->m_dStopTime;
				dRemaining -= dAvailable;
			}

		}

	}
	else
	{
		dRemaining = dStep * -1.0;

		//	Iterate the list from the tail to head and line up on the 
		//	designation provided by the caller
		Pos = pDesignations->GetTailPosition();
		while(Pos != NULL)
		{
			if((pDesignation = (CDesignation*)pDesignations->GetPrev(Pos)) == 0)
				continue;

			//	Is this the current designation?
			if(pDesignation->m_lPlaybackOrder == *pIndex)
				break;

		}

		//	Did we find the caller's designation?
		if(!pDesignation || (pDesignation->m_lPlaybackOrder != *pIndex))
		{
			*pIndex = -1;
			*pPosition = 0;
			return;
		}

		//	How far can we step back in the current designation?
		dAvailable = *pPosition - pDesignation->m_dStartTime;

		//	Can we do the full step without switching designations?
		if(dAvailable >= dRemaining)
		{
			*pPosition -= dRemaining;
			return;
		}
		else
		{
			//	Move the position to the start of this designation just in
			//	case we don't have any more designations in the list
			*pPosition = pDesignation->m_dStartTime;
			dRemaining -= dAvailable;

		}

		//	Keep moving through the designations until we've gone far enough
		//	or we run out
		while(Pos != NULL && dRemaining > 0)
		{
			if((pDesignation = (CDesignation*)pDesignations->GetPrev(Pos)) == 0)
				continue;

			//	What is the index of this designation?
			*pIndex = pDesignation->m_lPlaybackOrder;

			//	Line up on the stop position
			*pPosition = pDesignation->m_dStopTime;

			//	How many frames are available with this designation?
			dAvailable = pDesignation->m_dStopTime - pDesignation->m_dStartTime;

			//	Do we have enough?
			if(dAvailable >= dRemaining)
			{
				*pPosition -= dRemaining;
				return;
			}
			else
			{
				//	Move the position to the start of this designation just
				//	in case we don't have any more designations in the list
				*pPosition = pDesignation->m_dStartTime;
				dRemaining -= dAvailable;
			}

		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::CanSetBalance()
//
// 	Description:	This method is called to determine if the balance can be
//					adjusted by the owner of the control
//
// 	Returns:		TRUE if capable
//
//	Notes:			None
//
//==============================================================================
BOOL CTMMovieCtrl::CanSetBalance() 
{
	return m_Player.m_bAdjBalance;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::CanSetRate()
//
// 	Description:	This method is called to determine if the rate can be
//					adjusted by the owner of the control
//
// 	Returns:		TRUE if capable
//
//	Notes:			None
//
//==============================================================================
BOOL CTMMovieCtrl::CanSetRate() 
{
	return m_Player.m_bAdjRate;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::CanSetVolume()
//
// 	Description:	This method is called to determine if the volume can be
//					adjusted by the owner of the control
//
// 	Returns:		TRUE if capable
//
//	Notes:			None
//
//==============================================================================
BOOL CTMMovieCtrl::CanSetVolume() 
{
	return m_Player.m_bAdjVolume;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::Capture()
//
// 	Description:	This method is called to capture the current frame and
//					write it to the file specified by the caller as a device
//					independent bitmap
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::Capture(LPCTSTR lpFilespec, BOOL bResume) 
{
	char	szErrorMsg[256];
	BOOL	bCaptured;

	//	Has the control been initialized ?
	if(!IsReady())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTINITIALIZED);
		return TMMOVIE_NOTINITIALIZED;
	}

	//	Has a file been loaded ?
	if(!m_Player.IsLoaded())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTLOADED);
		return TMMOVIE_NOTLOADED;
	}

	try
	{
		//	Create and open the capture file
		CFile Capture(lpFilespec, CFile::modeCreate | CFile::modeWrite);

		//	Let the player perform the capture
		bCaptured = m_Player.Capture(&Capture);

		//	Close the file
		Capture.Close();

		//	Should we resume the playback?
		if(bResume)
			Resume();

		return (bCaptured) ? TMMOVIE_NOERROR : TMMOVIE_CAPTUREFAILED;

	}
	//	Catch all file exceptions
	catch(CFileException* pFileException)
	{
		//	Get the error message and pass it on to the handler
		pFileException->GetErrorMessage(szErrorMsg, sizeof(szErrorMsg));
		m_Errors.Handle(0, szErrorMsg);
		pFileException->Delete();
		return TMMOVIE_CAPFILEFAILED;
	}
	//	Catch all other exceptions
	catch(CException* pException)
	{
		//	Get the error message and pass it on to the handler
		pException->GetErrorMessage(szErrorMsg, sizeof(szErrorMsg));
		m_Errors.Handle(0, szErrorMsg);
		pException->Delete();
		return TMMOVIE_CAPTUREFAILED;
	}
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::CheckType()
//
// 	Description:	This method allows the caller to determine if the specified
//					file is MPEG video or AVI
//
// 	Returns:		TMMOVIE_MPEG if MPEG video
//					TMMOVIE_AVI if AVI video
//					TMMOVIE_UNKNOWN if unable to determine file type
//
//	Notes:			This method uses the file extension to determine the file
//					type
//
//==============================================================================
short CTMMovieCtrl::CheckType(LPCTSTR lpFilename) 
{
	char* pExtension;

	//	Find the extension
	if((pExtension = (char*)strrchr(lpFilename, '.')) == 0)
		return TMMOVIE_UNKNOWN;
	else
		pExtension++;

	//	Is this an MPEG file?
	if(_strcmpi(pExtension, "mpeg") == 0 || _strcmpi(pExtension, "mpg") == 0)
		return TMMOVIE_MPEG;
	else if(_strcmpi(pExtension, "avi") == 0)
		return TMMOVIE_AVI;
	else
		return TMMOVIE_UNKNOWN;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::CheckVersion()
//
// 	Description:	This will check the control's current version against
//					the persistent version stored by the container.
//
// 	Returns:		None
//
//	Notes:			All TrialMax II controls use the following scheme for
//					revision descriptors:
//
//					Major:	Incremented when new properties or methods have
//							been added or when the changes are significant
//							enough to break applications using the control.
//
//					Minor:	The minor revision is incremented when modifications
//							made to the control are unlikely to break 
//							applications using the control.
//
//==============================================================================
BOOL CTMMovieCtrl::CheckVersion(DWORD dwVersion)
{
	WORD	wMajor;
	WORD	wMinor;
	CString	strMsg;
	CString	strVersion;

	//	Do a quick check to see if the versions match
	if(dwVersion == (DWORD)MAKELONG(_wVerMinor, _wVerMajor))
		return TRUE;

	//	Get the persistant major/minor revisions
	wMajor = HIWORD(dwVersion);
	wMinor = LOWORD(dwVersion);

	//	Format the version information
	strVersion.Format("Application version: %d.%d\nControl Version: %d.%d",
					  wMajor, wMinor, _wVerMajor, _wVerMinor);

	//	Format the error message
	//
	//	NOTE:	We don't have to compare the major version identifiers because
	//			we always change the filename and dispatch interfaces on a major
	//			version upgrade
	strMsg.Format("This application was created with %s version of the "
	              "TMMovie ActiveX control. You should upgrade tm_movie6.ocx "
				  "as soon as possible\n\n%s", 
				  (wMinor > _wVerMinor) ? "a newer" : "an older", strVersion);

	MessageBeep(MB_ICONEXCLAMATION);
	MessageBox(strMsg, "TrialMax Error", MB_ICONEXCLAMATION | MB_OK);

	//	The versions do not match
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::ConvertToFrames()
//
// 	Description:	This method is called to convert the seconds to frames based
//					on the current video's frame rate
//
// 	Returns:		The frame equivalent
//
//	Notes:			None
//
//==============================================================================
long CTMMovieCtrl::ConvertToFrames(double dSeconds)
{
	return m_Player.ConvertToFrames(dSeconds);
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::ConvertToTime()
//
// 	Description:	This method is called to convert the frame count to decimal
//					seconds
//
// 	Returns:		The frame equivalent
//
//	Notes:			None
//
//==============================================================================
double CTMMovieCtrl::ConvertToTime(long lFrames)
{
	return m_Player.ConvertToTime(lFrames);
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::CTMMovieCtrl()
//
// 	Description:	This is the constructor for CTMMovieCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMMovieCtrl::CTMMovieCtrl()
{
	InitializeIIDs(&IID_DTMMovie6, &IID_DTMMovie6Events);

	//	Initialize the class data
	m_pOverlay				= 0;
	m_pPlaylist				= 0;
	m_pDesignation			= 0;
	m_pLink					= 0;
	m_pLine					= 0;
	m_dStartPosition		= (double)0;
	m_dStopPosition			= (double)0;
	m_dMinimumCue			= (double)0;
	m_dMaximumCue			= (double)0;
	m_dPosition				= (double)0;
	m_dTimePos				= (double)0;
	m_uTimer				= 0;
	m_sState				= TMMOVIE_NOTREADY;
	m_sPlaylistState		= TMMOVIE_PLNONE;
	m_lPlaylistStart		= -1;
	m_lPlaylistStop			= -1;
	m_bPlayDesignation		= FALSE;
	m_bNotify				= TRUE;
	m_bLastFrame			= FALSE;
	m_bDoDraw				= TRUE;
	m_bEnforceRange			= FALSE;
	m_dPlaylistTime			= 0.0;
	m_dElapsedPlaylist		= 0.0;
	m_dDesignationTime		= 0.0;
	m_dElapsedDesignation	= 0.0;
	m_hAudioBitmap			= 0;
	m_strDebugFilename		= "";
	ZeroMemory(&m_bmAudioInfo, sizeof(m_bmAudioInfo));
	ZeroMemory(&m_rcOverlay, sizeof(m_rcOverlay));

	pControl = this;

	//	Get the registry information
	GetRegistration();

	//	Set the path to the debug message file
	//SetDebugFilename();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::~CTMMovieCtrl()
//
// 	Description:	This is the destructor for CTMMovieCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMMovieCtrl::~CTMMovieCtrl()
{
	//	Flush the transcript text
	m_Lines.Flush(FALSE);

	//	Reset the global pointer
	pControl = 0;

	//	Destroy the overlay if it exists
	if(m_pOverlay)
		delete m_pOverlay;

	//	Make sure the player is unloaded
	m_Player.Unload();

	//	Cleanup the audio bitmap
	ReleaseAudioBitmap();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::DrawAudio()
//
// 	Description:	This function is called to draw the control when playing
//					audio files
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::DrawAudio(CDC* pdc, CRect& rcBounds)
{
	CDC		dcScratch;
	HBITMAP hOldBitmap;
	int		iLeft = 0;
	int		iTop = 0;

	//	Paint the background of the simulated window
	pdc->FillRect(rcBounds, CBrush::FromHandle((HBRUSH)GetStockObject(BLACK_BRUSH)));

	//	Should we stop here?
	if((m_hAudioBitmap == 0) || (m_bShowAudioImage == FALSE)) return;
	
	//	Make the scratch dc compatible with the dc for this window
	dcScratch.CreateCompatibleDC(pdc);

	//	Select the new bitmap into the scratch dc
	hOldBitmap = (HBITMAP)dcScratch.SelectObject(m_hAudioBitmap);
		
	//	Center the bitmap if necessary
	if(m_bmAudioInfo.bmWidth < rcBounds.Width())
	{
		iLeft = (rcBounds.left + (rcBounds.Width() / 2)) - (m_bmAudioInfo.bmWidth / 2);
	}
	if(m_bmAudioInfo.bmHeight < rcBounds.Height())
	{
		iTop = (rcBounds.top + (rcBounds.Height() / 2)) - (m_bmAudioInfo.bmHeight / 2);
	}

	pdc->BitBlt(iLeft, iTop, rcBounds.Width(), rcBounds.Height(), &dcScratch,
				0, 0, SRCCOPY);

	//	Clean up the scratch dc
	dcScratch.SelectObject(hOldBitmap);
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::FindFile()
//
// 	Description:	This function checks to see if the file exists.
//
// 	Returns:		TRUE if the file exists.
//
//	Notes:			None
//
//==============================================================================
BOOL CTMMovieCtrl::FindFile(LPCSTR lpFile, BOOL bPrompt)
{
	WIN32_FIND_DATA	Find;
	HANDLE			hFind;
	CString			Prompt;

	ASSERT(lpFile);

	//	Set up the user prompt
	Prompt.Format("Unable to locate %s for playback!", lpFile);

	while((hFind = FindFirstFile(lpFile, &Find)) == INVALID_HANDLE_VALUE)
	{
		//	Do we want to prompt the user?
		if(!bPrompt)
			return FALSE;

		//	Give the user the opportunity to retry or cancel
		if(MessageBox(Prompt, "File Not Found", 
					  MB_ICONEXCLAMATION | MB_RETRYCANCEL) == IDCANCEL)
		{
			return FALSE;
		}
	}
		
	//	Close the file find handle
	FindClose(hFind);
	return TRUE;
		
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetActFilters()
//
// 	Description:	This method is called to retrieve a list of all filters used
//					by the active graph manager
//
// 	Returns:		A string representation of the filter list. Filters are
//					delimited with a carraige return
//
//	Notes:			Vendor information for each filter is included if
//					bVendorInfo is TRUE. The vender info is also CR delimited
//
//==============================================================================
BSTR CTMMovieCtrl::GetActFilters(BOOL bVendorInfo, long FAR* pCount) 
{
	CString strFilters;
	
	//	Get the active filter list
	if(!m_Player.GetActFilters(strFilters, pCount, bVendorInfo))
	{
		strFilters.Empty();
		if(pCount) *pCount = 0;
	}

	return strFilters.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetAudioBitmap()
//
// 	Description:	This method is called to get the audio bitmap from the 
//					resource block
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::GetAudioBitmap()
{
	//	Deallocate the existing bitmap handle
	ReleaseAudioBitmap();

	//	Load the bitmap from the resource file
	//m_hAudioBitmap = ::LoadBitmap(AfxGetInstanceHandle(), "AUDIO_BITMAP");

		m_hAudioBitmap = (HBITMAP)::LoadImage(AfxGetInstanceHandle(), 
										 "AUDIO_BITMAP", IMAGE_BITMAP, 
									 0, 0, LR_VGACOLOR);

	//	Get the bitmap extents
	if(m_hAudioBitmap)
		::GetObject(m_hAudioBitmap, sizeof(m_bmAudioInfo), &m_bmAudioInfo);
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetClassIdString()
//
// 	Description:	This method is called to get the class GUID as a null
//					terminated string.
//
// 	Returns:		The string equivalent of the CLASS GUID
//
//	Notes:			None
//
//==============================================================================
BSTR CTMMovieCtrl::GetClassIdString() 
{
	CString strClsId = m_tmVersion.GetClsId();
	return strClsId.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetControlKeys()
//
// 	Description:	This function will check the keyboard to get the state of
//					the control keys
//
// 	Returns:		The appropriate key state identifier
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::GetControlKeys()
{
	short sKeyState = 0;

	//	Is the shift key pressed?
	if(GetKeyState(VK_SHIFT) & 0x8000)
	{
		//	Is the control key also pressed?
		if(GetKeyState(VK_CONTROL))
		{
			//	Is the Alt key also pressed?
			if(GetKeyState(VK_MENU))
				sKeyState = TMMOVIE_CTRLALTSHIFT;
			else
				sKeyState = TMMOVIE_CTRLSHIFT;
		}
		else if(GetKeyState(VK_MENU))
		{
			sKeyState = TMMOVIE_ALTSHIFT;
		}
		else
		{
			sKeyState = TMMOVIE_SHIFT;
		}
	}
	else if(GetKeyState(VK_CONTROL))
	{
		//	Is the Alt key pressed?
		if(GetKeyState(VK_MENU))
			sKeyState = TMMOVIE_CTRLALT;
		else
			sKeyState = TMMOVIE_CTRL;
	}
	else if(GetKeyState(VK_MENU))
	{
		sKeyState = TMMOVIE_ALT;
	}

	return sKeyState;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetPosition()
//
// 	Description:	This method is called to retrieve the current postion
//
// 	Returns:		The current postion if available. 0 otherwise
//
//	Notes:			None
//
//==============================================================================
double CTMMovieCtrl::GetPosition() 
{
	return m_dPosition;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetDefaultRate()
//
// 	Description:	This method is called to get the default frame rate used by
//					the player if no specific file is loaded
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
double CTMMovieCtrl::GetDefaultRate()
{
	return ((double)m_Player.m_fDefaultRate);
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetDuration()
//
// 	Description:	This method is called to get the duration of the specified
//					file without actually playing it.
//
// 	Returns:		The duration in decimal seconds, -1.0 on error
//
//	Notes:			None
//
//==============================================================================
double CTMMovieCtrl::GetDuration(LPCTSTR lpszFilename) 
{
	return m_Player.GetDuration(lpszFilename);
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetFrameRate()
//
// 	Description:	This method is called to retrieve the recorded frame rate
//					for the current video
//
// 	Returns:		The recorded frame rate
//
//	Notes:			None
//
//==============================================================================
float CTMMovieCtrl::GetFrameRate() 
{
	return m_Player.m_fFrameRate;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetInterface()
//
// 	Description:	This method is called to retrieve a pointer to the requested
//					COM interface in use by the current playback
//
// 	Returns:		A pointer to the requested interface
//
//	Notes:			None
//
//==============================================================================
LPUNKNOWN CTMMovieCtrl::GetInterface(short sInterface) 
{
	return (LPUNKNOWN)m_Player.GetInterface(sInterface);
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetIsAudio()
//
// 	Description:	This method is called to determine if the active file is
//					an audio file
//
// 	Returns:		TRUE if it is an audio file
//
//	Notes:			None
//
//==============================================================================
BOOL CTMMovieCtrl::GetIsAudio() 
{
	return m_Player.IsAudio();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetMaxTime()
//
// 	Description:	This method is called to retrieve the maximum time for the
//					current video
//
// 	Returns:		The maximum time in seconds
//
//	Notes:			None
//
//==============================================================================
double CTMMovieCtrl::GetMaxTime() 
{
	return m_Player.m_dMaxTime;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetMinTime()
//
// 	Description:	This method is called to retrieve the minimum frame for the
//					current video
//
// 	Returns:		The minimum frame
//
//	Notes:			None
//
//==============================================================================
double CTMMovieCtrl::GetMinTime() 
{
	return m_Player.m_dMinTime;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetPlaylistState()
//
// 	Description:	This method is called to retrieve the current playlist
//					state
//
// 	Returns:		One of the playlist state identifiers defined in 
//					tmovdefs.h
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::GetPlaylistState() 
{
	return m_sPlaylistState;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetRegFilters()
//
// 	Description:	This method is called to retrieve a list of all registered
//					filters
//
// 	Returns:		A string representation of the filter list. Filters are
//					delimited with a carraige return
//
//	Notes:			None
//
//==============================================================================
BSTR CTMMovieCtrl::GetRegFilters(long FAR* pCount) 
{
	CString strFilters;
	
	//	Get the filter list
	if(!m_Player.GetRegFilters(strFilters, pCount))
	{
		strFilters.Empty();
		if(pCount) *pCount = 0;
	}

	return strFilters.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetRegisteredPath()
//
// 	Description:	This method is called to get the ocx path stored in the
//					system registry.
//
// 	Returns:		The path to the ocx stored in the registry
//
//	Notes:			None
//
//==============================================================================
BSTR CTMMovieCtrl::GetRegisteredPath() 
{
	CString strRegistered = m_tmVersion.GetFileSpec();
	return strRegistered.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetRegistration()
//
// 	Description:	This function is called to get the control's registration
//					information.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::GetRegistration() 
{
	CLSID	clsid;

	//	Get the GUID 
	GetClassID(&clsid);

	//	Initialize the version information
	m_tmVersion.InitFromClsId("TMMovie", "Video Viewer Control", clsid);

}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetResolution()
//
// 	Description:	This method uses the current frame rate and the update rate
//					to determine the possible error in the reported frame
//					position
//
// 	Returns:		The possible error
//
//	Notes:			The error is + or -
//
//==============================================================================
short CTMMovieCtrl::GetResolution() 
{
	float fResolution;

	//	No error possible if not initialized and loaded
	if(!IsReady() || !IsLoaded())
		return 0;

	//	This should never happen but JUST IN CASE
	ASSERT(m_sUpdateRate != 0);
	if(m_sUpdateRate == 0)
		return 0;

	//	Calculate the resolution
	fResolution = m_Player.m_fFrameRate / (1000.0f / (float)m_sUpdateRate);

	return (short)(fResolution + 0.5);	
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetSrcHeight()
//
// 	Description:	This method is called to retrieve the recorded video height
//					for the current video
//
// 	Returns:		The recorded height
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::GetSrcHeight() 
{
	return (short)m_Player.m_lHeight;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetSrcWidth()
//
// 	Description:	This method is called to retrieve the recorded video width
//					for the current video
//
// 	Returns:		The recorded width
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::GetSrcWidth() 
{
	return (short)m_Player.m_lWidth;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::Cue()
//
// 	Description:	This method is called to cue the video to an absolute or
//					relative position
//
// 	Returns:		TMMOVIE_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::Cue(short sType, double dSeconds, BOOL bResume) 
{
	double dPosition;

	//	Has the control been initialized ?
	if(!IsReady())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTINITIALIZED);
		return TMMOVIE_NOTINITIALIZED;
	}

	//	Has a file been loaded ?
	if(!m_Player.IsLoaded())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTLOADED);
		return TMMOVIE_NOTLOADED;
	}

	//	Update the current position
	m_Player.GetPos(&m_dPosition);

	//	What type of move are we making?
	switch(sType)
	{
		case TMMCUE_FIRST:  
		
			dPosition = m_dMinimumCue;
			break;

		case TMMCUE_LAST:  
		
			dPosition = m_dMaximumCue;
			break;

		case TMMCUE_START:  
		
			dPosition = m_dStartPosition;
			break;

		case TMMCUE_STOP:
		
			dPosition = m_dStopPosition;
			break;

		case TMMCUE_ABSOLUTE:

			dPosition = dSeconds;
			break;

		case TMMCUE_RELATIVE:

			dPosition = m_dPosition + dSeconds;
			break;
			
		default:
			
			return TMMOVIE_INVALIDARG;
	}

	//	Is the desired position in range?
	if(dPosition < m_dMinimumCue)
		dPosition = m_dMinimumCue;
	else if(dPosition > m_dMaximumCue)
		dPosition = m_dMaximumCue;

	//	Cue to the new position
	if(!m_Player.SetPos(dPosition))
		return TMMOVIE_CUEFAILED;

	//	Make sure we resume the playback if requested
	if(bResume)
	{
		return Resume();
	}
	else
	{
		//	Make sure the video is visible
		if(m_bAutoShow && !m_Player.IsVisible())
			m_Player.Show(TRUE, TRUE);

		return TMMOVIE_NOERROR;
	}
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::CueDesignation()
//
// 	Description:	This function will permit the caller to cue the current
//					playlist to the frame within the specified designation.
//
// 	Returns:		TMMOVIE_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::CueDesignation(long lDesignation, double dPosition, short bResume) 
{
	BOOL	bOldAuto;
	short	sError;

	//	Has the control been initialized ?
	if(!IsReady())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTINITIALIZED);
		return TMMOVIE_NOTINITIALIZED;
	}

	//	We must have a playlist
	if(!m_pPlaylist)
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOPLAYLIST);
		return TMMOVIE_NOPLAYLIST;
	}

	//	This will keep the control from automatically trying to start the
	//	playlist until we have time to adjust the frames if necessary
	bOldAuto = m_bAutoPlay;
	m_bAutoPlay = FALSE;

	//	Make sure we pause the current playback
	Pause();

	//	If the caller has cued beyond the current range reset the range to
	//	include the entire playlist
	if(lDesignation > m_lPlaylistStop)
		m_lPlaylistStop = -1;

	//	Play the playlist
	sError = PlayPlaylist((long)m_pPlaylist, lDesignation, m_lPlaylistStop, dPosition);
	
	//	Restore the autoplay property
	m_bAutoPlay = bOldAuto;
		
	//	Stop here if there was an error
	if(sError != TMMOVIE_NOERROR)
		return sError;

	//	Make sure we are lined up on the appropriate link and text
	SetPlaylistLink(dPosition);
	SetPlaylistLine(dPosition);

	//	Now resume playing if requested. Otherwise cue the video 
	if(bResume)
		sError = Play();
	else
		sError = Cue(TMMCUE_START, 0.0, FALSE);

	return sError;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::CuePlaylist()
//
// 	Description:	This function will adjust the playback position of the
//					current playlist in accordance with parameters passed
//					by the caller.
//
// 	Returns:		TMMOVIE_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::CuePlaylist(short sType, double dSeconds, BOOL bResume,
								BOOL bToEnd) 
{
	CDesignations*	pDesignations;
	long			lIndex = -1;
	BOOL			bOldAuto;
	short			sError;
	double			dPosition = 0;

	//	Has the control been initialized ?
	if(!IsReady())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTINITIALIZED);
		return TMMOVIE_NOTINITIALIZED;
	}

	//	We must have a playlist
	if(!m_pPlaylist)
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOPLAYLIST);
		return TMMOVIE_NOPLAYLIST;
	}
	else
	{
		//	Get the list of designations for the current playlist
		pDesignations = &(m_pPlaylist->m_Designations);
	}

	//	We must have a list of designations
	if(!pDesignations)
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NODESIGNATION);
		return TMMOVIE_NODESIGNATION;
	}

	//	What type of move are we making?
	switch(sType)
	{
		case TMMCUEPL_FIRST:

			lIndex = pDesignations->GetFirstOrder();
			break;

		case TMMCUEPL_LAST:

			lIndex = pDesignations->GetLastOrder();
			break;

		case TMMCUEPL_NEXT:

			//	Do we have an active designation?
			if(!m_pDesignation)
			{
				//	If the playlist is finished we wrap around
				if(m_sPlaylistState == TMMOVIE_PLFINISHED)
					lIndex = pDesignations->GetFirstOrder();
			}
			else
				lIndex = pDesignations->GetNextOrder(m_pDesignation->m_lPlaybackOrder);

			//	If there is no NEXT designation stop here without 
			//	generating an error message
			if(lIndex < 0)
				return TMMOVIE_NEXTNOTFOUND;

			break;

		case TMMCUEPL_PREVIOUS:

			//	Do we have an active designation?
			if(!m_pDesignation)
			{
				//	If the playlist is finished we use the last designation
				if(m_sPlaylistState == TMMOVIE_PLFINISHED)
					lIndex = pDesignations->GetLastOrder();
			}
			else
				lIndex = pDesignations->GetPrevOrder(m_pDesignation->m_lPlaybackOrder);

			//	If there is no PREVIOUS designation stop here without 
			//	generating an error message
			if(lIndex < 0)
				return TMMOVIE_PREVNOTFOUND;

			break;

		case TMMCUEPL_CURRENT:

			//	Do we have an active designation?
			if(!m_pDesignation)
			{
				//	If the playlist is finished we use the last designation
				if(m_sPlaylistState == TMMOVIE_PLFINISHED)
					lIndex = pDesignations->GetLastOrder();
			}
			else
				lIndex = m_pDesignation->m_lPlaybackOrder;

			break;

		case TMMCUEPL_STEP:

			//	Did the caller specify a step?
			if(dSeconds == 0)
				return TMMOVIE_NOERROR;

			//	Get the values that define the current position. The current
			//	position is defined by the index of the current designation 
			//	and the time position within that designation
			
			//	Is there an active designation?
			if(m_pDesignation)
			{
				lIndex = m_pDesignation->m_lPlaybackOrder;
				m_Player.GetPos(&dPosition);
			}
			else
			{
				//	Are we stepping forward?
				if(dSeconds > 0)
				{
					//	Since there is no current designation, we have to
					//	assume we are at the end of the playlist and we can
					//	go no further. If we wanted to wrap around to the 
					//	beginning of the playlist we would place that code
					//	here
					return TMMOVIE_NEXTNOTFOUND;

				}
				else
				{
					//	Since we're at the end of the playlist we use the last
					//	frame of the last designation as the current position
					m_pDesignation = m_pPlaylist->m_Designations.Last();
					if(m_pDesignation)
					{
						lIndex = m_pDesignation->m_lPlaybackOrder;
						dPosition = m_pDesignation->m_dStopTime;
					}
				}

			}

			//	Calculate the step values
			CalcPlaylistStep(&lIndex, &dPosition, dSeconds);

			break;

		default:

			return TMMOVIE_INVALIDARG;

	}

	//	Do we have an index for the desired designation?
	if(lIndex < 0)
	{
		m_Errors.Handle(0, IDS_TMMOVIE_CUEDESIGNATION);
		return TMMOVIE_CUEDESIGNATION;
	}

	//	This will keep the control from automatically trying to start the
	//	playlist until we have time to adjust the frames if necessary
	bOldAuto    = m_bAutoPlay;
	m_bAutoPlay = FALSE;

	//	Make sure we pause the current playback
	Pause();

	//	Does the caller want to use the full playlist or restrict playback
	//	to the current designation?
	m_lPlaylistStop = (bToEnd) ? -1 : lIndex;

	//	Play the playlist
	sError = PlayPlaylist((long)m_pPlaylist, lIndex, m_lPlaylistStop, -1);
	
	//	Restore the autoplay property
	m_bAutoPlay = bOldAuto;
		
	//	Stop here if there was an error
	if(sError != TMMOVIE_NOERROR)
		return sError;

	//	We have to do some special processing when the user steps through the
	//	playlist because this is the only mode in which we do not necessarily
	//	resume the playback from the start of a designation. We also have to
	//	make sure we are lined up on the appropriate link 
	if(sType == TMMCUEPL_STEP)
	{
		SetPlaylistPosition(dPosition);
		SetPlaylistLink(dPosition);
		SetPlaylistLine(dPosition);
		ResetDesignationTimes(dPosition);
	}
	else
	{
		//	We are going to start the playback from the start of the designation
		SetPlaylistLink(-1);
		SetPlaylistLine(-1);
	}

	//	Now resume playing if requested. Otherwise cue the video to the start
	//	of the new position
	if(bResume)
		sError = Play();
	else
		sError = Cue(TMMCUE_START, 0.0, FALSE);

	return sError;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::DefWindowProc()
//
// 	Description:	This function overloads the base class member to add
//					processing of events from the DirectShow interface
//
// 	Returns:		0 if handled
//
//	Notes:			None
//
//==============================================================================
LRESULT CTMMovieCtrl::DefWindowProc(UINT uMessage, WPARAM wParam, LPARAM lParam) 
{
	long lEvent;

	//	Is this an event notification from DirectShow?
	if(uMessage == WM_IDXSHOWNOTIFY)
	{
		//	Get all the events in the queue
		while(m_Player.GetEvent(lParam, &lEvent))
			ProcessEvent(lEvent);
		return (LRESULT)0;
	}
	else
	{
		return COleControl::DefWindowProc(uMessage, wParam, lParam);
	}
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::DoPropertyExchange()
//
// 	Description:	This function manages the exchange of persistant 
//					properties
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::DoPropExchange(CPropExchange* pPX)
{
	BOOL bAutoInit = FALSE;
	BOOL bAutoPlay = FALSE;
	BOOL bAutoShow = FALSE;
	BOOL bKeepAspect = FALSE;
	BOOL bEnableErrors = FALSE;
	BOOL bScaleVideo = FALSE;
	BOOL bUseSnapshots = FALSE;
	BOOL bFilename = FALSE;
	BOOL bIniFile = FALSE;
	BOOL bOverlayFile = FALSE;
	BOOL bStartPosition = FALSE;
	BOOL bStopPosition = FALSE;
	BOOL bUpdateRate = FALSE;
	BOOL bRate = FALSE;
	BOOL bVolume = FALSE;
	BOOL bBalance = FALSE;
	BOOL bOverlayVisible = FALSE;
	BOOL bDetachBeforeLoad = FALSE;
	BOOL bHideTaskBar = FALSE;
	BOOL bEnableAxErrors = FALSE;
	BOOL bEnableSimulation = FALSE;
	BOOL bSimulationText = FALSE;
	BOOL bShowAudioImage = FALSE;

	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
	COleControl::DoPropExchange(pPX);

	//CheckVersion(pPX->GetVersion());

	//	Load the control's persistent properties
	try
	{
		bAutoInit = PX_Bool(pPX, _T("AutoInit"), m_bAutoInit, TRUE);
		bAutoPlay = PX_Bool(pPX, _T("AutoPlay"), m_bAutoPlay, TRUE);
		bAutoShow = PX_Bool(pPX, _T("AutoShow"), m_bAutoShow, TRUE);
		bKeepAspect = PX_Bool(pPX, _T("KeepAspect"), m_bKeepAspect, TRUE);
		bEnableErrors = PX_Bool(pPX, _T("EnableErrors"), m_bEnableErrors, TRUE);
		bScaleVideo = PX_Bool(pPX, _T("ScaleVideo"), m_bScaleVideo, TRUE);
		bUseSnapshots = PX_Bool(pPX, _T("UseSnapshots"), m_bUseSnapshots, DEFAULT_USESNAPSHOTS);
		bFilename = PX_String(pPX, _T("Filename"), m_strFilename, "");   
  		bIniFile = PX_String(pPX, _T("IniFile"), m_strIniFile, DEFAULT_TMAXINI);
		bOverlayFile = PX_String(pPX, _T("OverlayFile"), m_strOverlay, "");                   
		bStartPosition = PX_Double(pPX, _T("StartPosition"), m_dStartPosition, 0);   
		bStopPosition = PX_Double(pPX, _T("StopPosition"), m_dStopPosition, 0);
		bUpdateRate = PX_Short(pPX, _T("UpdateRate"), m_sUpdateRate, 200);  
		bRate = PX_Short(pPX, _T("Rate"), m_sRate, 0);  
		bVolume = PX_Short(pPX, _T("Volume"), m_sVolume, 100);  
		bBalance = PX_Short(pPX, _T("Balance"), m_sBalance, 0);
		bOverlayVisible = PX_Bool(pPX, _T("OverlayVisible"), m_bOverlayVisible, FALSE); 
		bDetachBeforeLoad = PX_Bool(pPX, _T("DetachBeforeLoad"), m_bDetachBeforeLoad, FALSE);
		bHideTaskBar = PX_Bool(pPX, _T("HideTaskBar"), m_bHideTaskBar, FALSE);
		bEnableAxErrors = PX_Bool(pPX, _T("EnableAxErrors"), m_bEnableAxErrors, FALSE);
		bEnableSimulation = PX_Bool(pPX, _T("EnableSimulation"), m_bEnableSimulation, FALSE);
		bSimulationText = PX_String(pPX, _T("SimulationText"), m_strSimulationText, "");   
		bShowAudioImage = PX_Bool(pPX, _T("ShowAudioImage"), m_bShowAudioImage, FALSE);
	}
	catch(...)
	{
		if(!bAutoInit) m_bAutoInit = TRUE;
		if(!bAutoPlay) m_bAutoPlay = TRUE;
		if(!bAutoShow) m_bAutoShow = TRUE;
		if(!bKeepAspect) m_bKeepAspect = TRUE;
		if(!bEnableErrors) m_bEnableErrors = TRUE;
		if(!bScaleVideo) m_bScaleVideo = TRUE;
		if(!bUseSnapshots) m_bUseSnapshots = DEFAULT_USESNAPSHOTS;
		if(!bFilename) m_strFilename = "";   
  		if(!bIniFile) m_strIniFile = DEFAULT_TMAXINI;
		if(!bOverlayFile) m_strOverlay = "";                   
		if(!bStartPosition) m_dStartPosition = 0;   
		if(!bStopPosition) m_dStopPosition = 0;
		if(!bUpdateRate) m_sUpdateRate = 200;  
		if(!bRate) m_sRate = 0;  
		if(!bVolume) m_sVolume = 50;  
		if(!bBalance) m_sBalance = 0;
		if(!bOverlayVisible) m_bOverlayVisible = FALSE; 
		if(!bDetachBeforeLoad) m_bDetachBeforeLoad = FALSE;
		if(!bHideTaskBar) m_bHideTaskBar = FALSE;
		if(!bEnableAxErrors) m_bEnableAxErrors = FALSE;
		if(!bEnableSimulation) m_bEnableSimulation = FALSE;
		if(!bSimulationText) m_strSimulationText = "";   
		if(!bShowAudioImage) m_bShowAudioImage = FALSE;   
	} 

	//	Override the stored volume to make sure we are using the full system
	//	volume level
	//
	//	We do this because the default value use to be 50% and that meant that
	//	Trailmax users could never get above 50% of the full volume
	if(AmbientUserMode() && pPX->IsLoading())
		m_sVolume = 100;

	//	Set default values for new properties
	if(pPX->IsLoading())
	{
		switch(LOWORD(pPX->GetVersion()))
		{
			case 0:

				m_bEnableSimulation = FALSE;
				m_strSimulationText = "";   
				break;

		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetState()
//
// 	Description:	This method is called to get the current state of the player
//
// 	Returns:		The current state as defined in tmmvdefs.h
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::GetState() 
{
	return m_sState;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetType()
//
// 	Description:	This method allows the caller to determine what type of
//					file this control is currently ready to play
//
// 	Returns:		TMMOVIE_AVI or TMMOVIE_MPEG if loaded and ready 
//					TMMOVIE_UNKNOWN otherwise
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::GetType() 
{
	//	Has the control been initialized ?
	if(!IsReady() || !m_Player.IsLoaded())
		return TMMOVIE_UNKNOWN;
	else
		return CheckType(m_strFilename);
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetUserFilters()
//
// 	Description:	This method is called to retrieve a list of all user specified
//					filters.
//
// 	Returns:		A string representation of the filter list. Filters are
//					delimited with a carraige return
//
//	Notes:			None
//
//==============================================================================
BSTR CTMMovieCtrl::GetUserFilters(long FAR* pCount) 
{
	CString strFilters;
	
	//	Get the filter list
	if(!m_Player.GetUserFilters(strFilters, pCount))
	{
		strFilters.Empty();
		if(pCount) *pCount = 0;
	}

	return strFilters.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetVerBuild()
//
// 	Description:	This method is called to get the value of the VerBuild
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::GetVerBuild() 
{
	return m_tmVersion.GetBuild();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetVerBuildDate()
//
// 	Description:	This method is called to get the value of the VerBuildDate
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMMovieCtrl::GetVerBuildDate() 
{
	CString strBuildDate = m_tmVersion.GetBuildDate();
	return strBuildDate.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetVerMajor()
//
// 	Description:	This method is called to get the value of the VerMajor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::GetVerMajor() 
{
	return m_tmVersion.GetMajor();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetVerMinor()
//
// 	Description:	This method is called to get the value of the VerMinor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::GetVerMinor() 
{
	return m_tmVersion.GetMinor();
}


//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetVerQEF()
//
// 	Description:	This method is called to get the value of the VerQEF
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::GetVerQEF() 
{
	return m_tmVersion.GetUpdate();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetVerTextLong()
//
// 	Description:	This method is called to get the value of the VerTextLong
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMMovieCtrl::GetVerTextLong() 
{
	CString strVer = m_tmVersion.GetTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::GetVerTextShort()
//
// 	Description:	This method is called to get the value of the VerTextShort
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMMovieCtrl::GetVerTextShort() 
{
	CString strVer = m_tmVersion.GetShortTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::Initialize()
//
// 	Description:	This method is called to initialize the control
//
// 	Returns:		None
//
//	Notes:			If the AutoInit property is TRUE, the control is initialized
//					when it is created
//
//==============================================================================
short CTMMovieCtrl::Initialize() 
{
	//	Initialize the video player
	if(!m_Player.Initialize(this, &m_Errors))
	{
		m_Errors.Handle(0, IDS_TMMOVIE_INITIALIZEFAILED);
		return TMMOVIE_INITIALIZEFAILED;
	}

	//	Set the player properties
	m_Player.SetScaleProps(m_bScaleVideo, m_bKeepAspect);
	m_Player.SetRate(m_sRate);
	m_Player.SetVolume(m_sVolume);
	m_Player.SetBalance(m_sBalance);
	m_Player.EnableSnapshots(m_bUseSnapshots);
	m_Player.SetDetachBeforeLoad(m_bDetachBeforeLoad);
	m_Player.SetHideTaskBar(m_bHideTaskBar);

	//	Build the player's filter list
	SetFilters();

	//	Allocate the overlay window
	m_pOverlay = new COverlay(this);
	ASSERT(m_pOverlay);

	//	Get the bitmap used as the audio playback indicator
	GetAudioBitmap();

	//	Create the window
	if(!m_pOverlay->Create())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_CREATEOVERLAYFAILED);
		if(m_pOverlay)
		{
			delete m_pOverlay;
			m_pOverlay = 0;
		}
	}
	else
	{
		//	Make sure the background color matches
		m_pOverlay->SetBackColor(TranslateColor(GetBackColor()));

		//	Make sure the correct overlay file is loaded
		OnOverlayFileChanged();
	}

	//	Set the current state
	SetState(TMMOVIE_READY);

	return TMMOVIE_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::IsLoaded()
//
// 	Description:	This function is called to see if a file is loaded and ready
//					for playback
//
// 	Returns:		TRUE if loaded
//
//	Notes:			None
//
//==============================================================================
BOOL CTMMovieCtrl::IsLoaded() 
{
	return (m_Player.IsLoaded());
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::IsReady()
//
// 	Description:	This method is called to determine if the control has been
//					properly initialized
//
// 	Returns:		TRUE if initialized
//
//	Notes:			None
//
//==============================================================================
BOOL CTMMovieCtrl::IsReady() 
{
	return (m_sState != TMMOVIE_NOTREADY);
}

//==============================================================================
//
// 	Function Name:	CPlayer::IsVideoVisible()
//
// 	Description:	This function is called to see if the video is visible or
//					hidden
//
// 	Returns:		TRUE if visible. FALSE if hidden.
//
//	Notes:			None
//
//==============================================================================
BOOL CTMMovieCtrl::IsVideoVisible() 
{
	return m_Player.IsVisible();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::Load()
//
// 	Description:	This method is called to load a new file
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::Load(LPCTSTR lpFilename, double dStart, double dStop, BOOL bPlay) 
{
	ASSERT(lpFilename);

	//	Has the control been initialized ?
	if(!IsReady())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTINITIALIZED);
		return TMMOVIE_NOTINITIALIZED;
	}

	//	Is the caller unloading the current video
	if(lpFilename == 0)
	{
		Unload();
		return TMMOVIE_NOERROR;
	}	

	//	Is this already the active file?
	if(m_Player.m_strFilename.CompareNoCase(lpFilename) == 0)
	{
		//	Reset the playback range
		SetRange(dStart, dStop);

		//	Should we start playing?
		if(bPlay)
			return Play();
		else
			return Cue(TMMCUE_START, 0, FALSE); // MODIFIED REV 6.0
	}

	//	Check to see if the file exists
	if(!FindFile(lpFilename, (m_bEnableErrors && !m_bEnableAxErrors && !m_bEnableSimulation)))
	{
		//	Set up the simulation if enabled
		if(m_bEnableSimulation)
		{
			if(!m_Player.Simulate(lpFilename))
				return TMMOVIE_LOADFAILED;
		}
		else
		{
			//	Assume the user canceled the operation
			return TMMOVIE_NOERROR;
		}
	}
	else
	{
		//	Set the filename for playback
		if(!m_Player.Load(lpFilename))
			return TMMOVIE_LOADFAILED;
	}

	//	Initialize the cue range
	m_dMinimumCue = m_Player.m_dMinTime;
	m_dMaximumCue = m_Player.m_dMaxTime;

	//	The file has changed
	m_strFilename = lpFilename;
	FireFileChange(m_strFilename);

	//	Set the start and stop frame
	SetRange(dStart, dStop); 

	//	Make sure the window gets drawn properly in case we are switching between
	//	simulate and playback
	if(m_bEnableSimulation || m_Player.IsAudio())
		InvalidateControl();

	//	Should we automatically start the playback?
	if(bPlay)
		return Play();

	//	Update the state and position
	UpdateState();
	UpdatePos();

	return TMMOVIE_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::LoadDesignation
//
// 	Description:	This function takes a pointer to a designation,
//                  retrieves information pertaining to the designation,
//                  and calls OnFilenameChanged(). 
//
// 	Returns:		short
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::LoadDesignation(double dPosition)
{
	CTextPage*	pPage;
	CTextLine*	pLine;
	CString		strError;
	CString		strFilename;
	short		sError;
	double		dFirst;

	ASSERT(m_pPlaylist);
	ASSERT(m_pDesignation);
	if(!m_pDesignation)
		return TMMOVIE_NODESIGNATION;

	//	Does this designation have a undefined video file id?
	if(m_pDesignation->m_lVideoId < 0)
	{
		//	Set the error level
		SetPlaylistState(TMMOVIE_PLERROR);
		m_Errors.Handle(0, IDS_TMMOVIE_UNDEFINEDVIDEOID);
		return TMMOVIE_UNDEFINEDVIDEOID;
	}

	//	Get the video file associated with this designation 
	if(m_pPlaylist->GetVideoFile(m_pDesignation->m_lVideoId, strFilename) == FALSE)
	{
		//	Format the error message
		//
		//	NOTE:	We have to do this first because SetPlaylistState() will
		//			NULL the current designation
		strError.Format("%ld", m_pDesignation->m_lVideoId);

		//	Set the error level
		SetPlaylistState(TMMOVIE_PLERROR);

		m_Errors.Handle(0, IDS_TMMOVIE_INVALIDVIDEO, strError);
		return TMMOVIE_INVALIDVIDEO;
	}
	else
	{   
		//	Stop the timer while we load the new designation
		StopTimer();
	
		//	Load the deposition text for this designation.
		//
		//	NOTE:	Keeping the lines in our own list instead of using the
		//			transcript object does two things for us. We don't have to
		//			deal with the page-line heirarchy and we don't have to worry
		//			about somebody messing up our iterator
		//
		//			Loading the list in reverse is MUCH more efficient because
		//			the list object will only have to do one comparison to
		//			locate the correct insertion point
		m_Lines.Flush(FALSE);
		pPage = m_pDesignation->LastPage();
		while(pPage)
		{
			pLine = pPage->LastLine();
			while(pLine)
			{
				m_Lines.Add(pLine);
				pLine = pPage->PrevLine();
			}
			pPage = m_pDesignation->PrevPage();
		}

		//	Let the container know we're loading a new designation
		FireDesignationChange(m_pDesignation->m_lTertiaryId, 
							  m_pDesignation->m_lPlaybackOrder);

		//	Do we use the default start position?
		if(dPosition < 0)
			dFirst = m_pDesignation->m_dStartTime;
		else
			dFirst = dPosition;

		ASSERT(dFirst <= m_pDesignation->m_dStopTime);
		if(dFirst > m_pDesignation->m_dStopTime)
			dFirst = m_pDesignation->m_dStopTime;

		//	Has the filename changed?
		if(m_Player.m_strFilename.CompareNoCase(strFilename))
		{
			//	Load the new file
			sError = Load(strFilename, dFirst, 
						  m_pDesignation->m_dStopTime, FALSE);
				
			if(sError != TMMOVIE_NOERROR)
				return sError;
		}
		else
		{
			//	Set the start/stop range for this designation
			SetRange(dFirst, m_pDesignation->m_dStopTime);
		}

		//	Make sure the playback range gets enforced for a designation
		m_bEnforceRange = TRUE;

		//	Set the appropriate link and line states
		if(dPosition < 0)
		{
			m_pLink = m_pDesignation->FirstLink();
			SetPlaylistLine(dPosition);
		}
		else
		{
			SetPlaylistLink(dFirst);
			SetPlaylistLine(dFirst);
		}

		//	Reset the designation time counters
		ResetDesignationTimes(dFirst);

		//	Reset the overlay file if it's changed
		if(m_strOverlay.CompareNoCase(m_pDesignation->m_strOverlay))
		{
			m_strOverlay = m_pDesignation->m_strOverlay;
			OnOverlayFileChanged();
		}

		//	Play the new designation
		if(m_bPlayDesignation)
		{
			StartTimer();
			Play();
		}
		
	}

	return TMMOVIE_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnAutoInitChanged()
//
// 	Description:	The function is called when the AutoInit property changes
//
// 	Returns:		None
//
//	Notes:			This is a design time property. If changed at runtime, no
//					action is taken.
//
//==============================================================================
void CTMMovieCtrl::OnAutoInitChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnAutoPlayChanged()
//
// 	Description:	This function is called when the AutoPlay property changes
//
// 	Returns:		None
//
//	Notes:			This property is checked when a new file is loaded to see
//					if the control should automatically begin playing the file.
//
//==============================================================================
void CTMMovieCtrl::OnAutoPlayChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnDetachBeforeLoadChanged()
//
// 	Description:	This function is called when the DetachBeforeLoad property 
//					changes.
//
// 	Returns:		void
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnDetachBeforeLoadChanged() 
{
	if(AmbientUserMode())
		m_Player.SetDetachBeforeLoad(m_bDetachBeforeLoad);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnDraw()
//
// 	Description:	This function is called in response to WM_PAINT messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnDraw(CDC* pdc, const CRect& rcBounds,const CRect& rcInvalid)
{
	CBrush	brBackground;
	CRect	Rect;
	BOOL	bRedrawOverlay = FALSE;
	
	//	Are we in user mode?
	if(AmbientUserMode())
	{
		if(!m_bDoDraw) return;

		//	Is the overlay visible?
		if(m_pOverlay && IsWindow(m_pOverlay->m_hWnd) && m_bOverlayVisible)
		{
			//	Clip the drawing rectangle
			Rect.SetRect(0, 0, rcBounds.Width(), m_rcOverlay.top);

			//	Force redrawing of the overlay after we repaint the playback area
			bRedrawOverlay = TRUE;
		}
		else
		{
			//	Use the full area
			Rect = rcBounds;
		}

		//	Paint the background
// TEST brBackground.CreateSolidBrush(RGB(0xFF,0xFF,0x00));
		brBackground.CreateSolidBrush(TranslateColor(GetBackColor()));
		pdc->FillRect(Rect, &brBackground);

		//	Are we simulating playback?
		if(m_Player.GetSimulating() == TRUE)
		{
			//	Resize the rectangle to get a simlated video rectangle
			if(m_bKeepAspect)
				ResizeToRatio(Rect, m_Player.m_fAspectRatio);

			//	Paint the background of the simulated window
		//CBrush brSimulate;
		//brSimulate.CreateSolidBrush(RGB(0xFF,0x00,0xFF));
		//pdc->FillRect(Rect, &brSimulate);
			pdc->FillRect(Rect, CBrush::FromHandle((HBRUSH)GetStockObject(BLACK_BRUSH)));

			if(m_strSimulationText.GetLength() > 0)
			{
				pdc->SetBkMode(TRANSPARENT);
				pdc->SetBkColor(RGB(0x00,0x00,0x00));
				pdc->SetTextColor(RGB(0x00,0x00,0xFF));
				pdc->DrawText(m_strSimulationText, Rect, 0); 
				
//				RECT r;
//				r.left = 500;
//				r.top = 500;
//				r.right = 1000;
//				r.bottom = 1000;

//				pdc->DrawText("What the hell!", &r, 0); 
//CString M;
//M.Format("L: %d\nT: %d\nR: %d\nB: %d", Rect.left, Rect.top, Rect.right, Rect.bottom);
//MessageBox(M, m_strSimulationText, MB_OK);
			}

		
		}
		else if(m_Player.IsAudio() == TRUE)
		{
			DrawAudio(pdc, Rect);
		}
		else
		{
			//	Redraw the video window
			m_Player.Redraw();
		}

		//	Should we redraw the overlay?
		if(bRedrawOverlay == TRUE)
			m_pOverlay->Redraw();
	
	}
	else
	{
		CString	strText;
		CRect ControlRect = rcBounds;

		strText.Format("FTI Movie Control (rev. %d.%d)", _wVerMajor, _wVerMinor);

		//	Paint the background
		pdc->FillRect(ControlRect,
			  CBrush::FromHandle((HBRUSH)GetStockObject(LTGRAY_BRUSH)));

		pdc->Draw3dRect(ControlRect, RGB(0x00,0x00,0x00), 
									 RGB(0xFF,0xFF,0xFF));

		pdc->SetBkMode(TRANSPARENT);
		pdc->SetTextColor(RGB(0x00,0x00,0x00));
		pdc->DrawText(strText, ControlRect, 
					  DT_CENTER | DT_NOCLIP | DT_VCENTER | DT_SINGLELINE); 
	}

}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnEnableAxErrorsChanged()
//
// 	Description:	This function is called when the EnableAxErrors property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnEnableAxErrorsChanged() 
{
	SetModifiedFlag();

	if(AmbientUserMode())
		m_Errors.SetMessageId(m_bEnableAxErrors == TRUE ? WM_ERROR_NOTIFICATION : 0);
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnEnableErrorsChanged()
//
// 	Description:	This function is called when the EnableErrors property 
//					changes.
//
// 	Returns:		void
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnEnableErrorsChanged() 
{
	// Set the state of the error handler
	if(AmbientUserMode() == TRUE)
		m_Errors.Enable(m_bEnableErrors);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnEnableSimulationChanged()
//
// 	Description:	This function is called when the EnableSimulation property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnEnableSimulationChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnEvComplete()
//
// 	Description:	This function handles EC_COMPLETE event notifications sent
//					by the DirectMedia layer
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnEvComplete() 
{
	CDesignation* pDesignation;

	SendDebug("Event", "OnEvComplete", 0);

	//	Do we need to load a new designation?
	if(m_pDesignation && m_sPlaylistState == TMMOVIE_PLACTIVE)  
	{
		//	We don't have a pending link right now
		m_pLink = 0;
		m_pLine = 0;

		//	Update the elapsed time counters
		OnTimer(m_uTimer);

		//	Get the next designation.
		if((pDesignation = m_pPlaylist->GetNextDesignation()) == 0)
		{	
			SetPlaylistState(TMMOVIE_PLFINISHED);
			
			//	Stop the playback
			Stop(); 	
		}
		// Have we reached the stopping point?
		else if((m_lPlaylistStop > 0) &&
			    (pDesignation->m_lPlaybackOrder > m_lPlaylistStop))
		{	
			SetPlaylistState(TMMOVIE_PLSTOPPED);
			
			//	Stop the playback
			Stop(); 	
		}
		else
		{    
			//	Save the new designation
			m_pDesignation = pDesignation;

			//	Load the next designtation
			LoadDesignation(-1);
		}	
	}
	else
	{
		// Notify the container
		FirePlaybackComplete();

		//	Stop the playback
		Stop();
	}
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnEvError()
//
// 	Description:	This function handles all playback error notifications sent
//					from the DirectMedia layer
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnEvError(long lError, BOOL bStopped) 
{
	// Notify the container
	FirePlaybackError(lError, bStopped);

	//	Set the state if the playback was stopped
	if(bStopped)
		SetState(TMMOVIE_STOPPED);
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnFileNameChanged()
//
// 	Description:	This function is called when the Filename property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnFilenameChanged() 
{
	SetModifiedFlag();

	//	Stop here if we are not in user mode
	if(!AmbientUserMode())
		return;

	//	Load the new file
	Load(m_strFilename, m_dStartPosition, m_dStopPosition, m_bAutoPlay);

}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnHideTaskBarChanged()
//
// 	Description:	This function is called when the HideTaskBar property 
//					changes.
//
// 	Returns:		void
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnHideTaskBarChanged() 
{
	if(AmbientUserMode())
		m_Player.SetHideTaskBar(m_bHideTaskBar);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnIniFileChanged()
//
// 	Description:	This function is called when the IniFile property changes. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnIniFileChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnKeepAspectChanged()
//
// 	Description:	This function is called when the KeepAspect property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnKeepAspectChanged() 
{
	if(AmbientUserMode())
		m_Player.SetScaleProps(m_bScaleVideo, m_bKeepAspect);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnLButtonDblClk()
//
// 	Description:	This function traps WM_LBUTTONDBLCLICK messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnLButtonDblClk(UINT nFlags, CPoint point) 
{
	if(AmbientUserMode())
	{
		FireMouseDblClick(TMMOVIE_LEFT_BUTTON, point.x, point.y);
	}

	COleControl::OnLButtonDblClk(nFlags, point);
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnOverlayFileChanged()
//
// 	Description:	This function is called when the OverlayFile property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnOverlayFileChanged() 
{
	SetModifiedFlag();

	//	Do nothing if we aren't in user mode or if we don't have an overlay
	if(!AmbientUserMode() || (m_pOverlay == 0))
		return;

	//	Set the filename for the overlay
	m_pOverlay->SetFilename(m_strOverlay);

	//	Make sure the visibility is correct
	OnOverlayVisibleChanged();	
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnOverlayVisibleChanged()
//
// 	Description:	This function is called when the OverlayVisible property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnOverlayVisibleChanged() 
{
	RECT rcControl;

	SetModifiedFlag();

	//	Do nothing unless we are in user mode
	if(!AmbientUserMode())
		return;

	//	Set the visibility of the overlay window
	if(m_pOverlay)
	{
		//	Get the control's client rectangle
		GetClientRect(&rcControl);

		//	Show the window if we have a file and we can set its position
		if(m_bOverlayVisible && !m_strOverlay.IsEmpty() &&
		   SetOverlayPos(&rcControl))
		{
			m_pOverlay->ShowWindow(SW_SHOW);
			m_pOverlay->Redraw();
			m_pOverlay->BringWindowToTop();
		}
		else
		{
			ZeroMemory(&rcControl, sizeof(rcControl));
			m_pOverlay->ShowWindow(SW_HIDE);
			m_pOverlay->MoveWindow(&rcControl);
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnPlayerSetPos()
//
// 	Description:	This function is called by the player when the video is 
//					positioned to a particular frame
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnPlayerSetPos() 
{
	//	Make sure the overlay is on top if it's visible
	if(m_pOverlay && IsWindow(m_pOverlay->m_hWnd) &&
	   m_pOverlay->IsWindowVisible())
		m_pOverlay->BringWindowToTop();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnPlayerSetWnd()
//
// 	Description:	This function is called by the player when it sets the video
//					rendering window
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnPlayerSetWnd() 
{
	//	Make sure the overlay is on top if it's visible
	if(m_pOverlay && IsWindow(m_pOverlay->m_hWnd) &&
	   m_pOverlay->IsWindowVisible())
		m_pOverlay->BringWindowToTop();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnPlayerShow()
//
// 	Description:	This function is called by the player when the video is
//					shown or hidden
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnPlayerShow(BOOL bShow)
{
	//	Make sure the overlay is on top if it's visible
	if(bShow && m_pOverlay && IsWindow(m_pOverlay->m_hWnd) &&
	   m_pOverlay->IsWindowVisible())
		m_pOverlay->BringWindowToTop();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnRateChanged()
//
// 	Description:	This function is called when the Rate property changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnRateChanged() 
{
	if(AmbientUserMode())
	{
		//	Is the rate within range?
		if(m_sRate < -100)
			m_sRate = -100;
		else if(m_sRate > 100)
			m_sRate = 100;

		//	Set the player rate
		m_Player.SetRate(m_sRate);
	}

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnRButtonDblClk()
//
// 	Description:	This function traps WM_RBUTTONDBLCLICK messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnRButtonDblClk(UINT nFlags, CPoint point) 
{
	if(AmbientUserMode())
	{
		FireMouseDblClick(TMMOVIE_RIGHT_BUTTON,point.x, point.y);
	}

	COleControl::OnRButtonDblClk(nFlags, point);
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnResetState()
//
// 	Description:	This method is called to reset the control to its default 
//					state.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnResetState()
{
	COleControl::OnResetState();  
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnScaleAVIChanged()
//
// 	Description:	This function is called when the ScaleVideo property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnScaleVideoChanged() 
{
	if(AmbientUserMode())
		m_Player.SetScaleProps(m_bScaleVideo, m_bKeepAspect);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnShowAudioImageChanged()
//
// 	Description:	This function is called when the ShowAudioImage property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnShowAudioImageChanged() 
{
	if(AmbientUserMode() && GetIsAudio())
		InvalidateControl();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnSimulationTextChanged()
//
// 	Description:	This function is called when the SimulationText property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnSimulationTextChanged() 
{
	if(AmbientUserMode())
		InvalidateControl();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnSize()
//
// 	Description:	This function handles all WM_SIZE messages sent to the 
//					control
//
// 	Returns:		TMMOVIE_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnSize(UINT nType, int cx, int cy) 
{
	//	Perform the base class processing
	COleControl::OnSize(nType, cx, cy);

	//	Make sure the video is properly sized and positioned
	UpdateScreenPosition();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnStartPositionChanged()
//
// 	Description:	This function is called when the StartPosition property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnStartPositionChanged() 
{
	//	Notify the player if it's loaded
	if(AmbientUserMode())
		SetRange(m_dStartPosition, m_dStopPosition);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnStopPositionChanged()
//
// 	Description:	This function is called when the StopPosition property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnStopPositionChanged() 
{
	//	Notify the player if it's loaded
	if(AmbientUserMode())
		SetRange(m_dStartPosition, m_dStopPosition);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnTimer()
//
// 	Description:	This function handles all WM_TIMER messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnTimer(UINT nIDEvent) 
{
	//	Stop the timer while we process this message
	StopTimer();

	//	Stop here if the control is not ready
	if(m_sState == TMMOVIE_NOTREADY)
		return;

	//	Update the current position and state
	if(UpdateState() == TMMOVIE_NOTREADY)
		return;

	//	Update the current position
	UpdatePos();

	//	Do we need to check playlist links?
	//
	//	Note:	The check to see if we're on the last frame of the file is
	//			needed because a call to SetRange() will result in the position
	//			being reported as the last frame even though that's not really
	//			where playback will resume from
	if((m_sPlaylistState == TMMOVIE_PLACTIVE) &&
	   (m_dPosition != m_Player.m_dMaxTime))
	{
		//	Update the elapsed time counters
		UpdateElapsedTimes();

		//	Do we need to fire a link event?
		while((m_pLink != 0) && (m_dPosition >= m_pLink->m_dTrigger))
		{
			//	Fire the link change event
			FireLinkChange(m_pLink->m_strItemBarcode, m_pLink->m_lId,
						   m_pLink->m_lFlags);
		
			//	Get the next link
			m_pLink = m_pDesignation->NextLink();
		}

		//	Do we need to fire a line change event?
		while((m_pLine != 0) && (m_dPosition >= m_pLine->m_dStartTime))
		{
			//	Fire the line change event
			FireLineChange((long)m_pLine);
		
			//	Get the next line
			m_pLine = m_Lines.Next();
		}
	}

	//	Restart the timer
	StartTimer();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnUpdateRateChanged()
//
// 	Description:	This function is called when the UpdateRate property
//					changes
//
// 	Returns:		None
//
//	Notes:			This property controls the rate at which current frame and
//					state variables are updated.
//
//==============================================================================
void CTMMovieCtrl::OnUpdateRateChanged() 
{
	//	Reset the timer if we are in user mode
	if(AmbientUserMode())
	{
		//	Stop the current timer if it's active
		if(m_uTimer > 0)
		{
			StopTimer();
			StartTimer();
		}
	}
	
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnAutoShowChanged()
//
// 	Description:	This function is called when the AutoShow property changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnAutoShowChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnBackColorChanged()
//
// 	Description:	This method is called when the background color is changed
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnBackColorChanged() 
{
	// Do the base class processing	
	COleControl::OnBackColorChanged();

	//	Set the overlay background
	if(m_pOverlay)
		m_pOverlay->SetBackColor(TranslateColor(GetBackColor()));
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnBalanceChanged()
//
// 	Description:	This function is called when the Balance property changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnBalanceChanged() 
{
	if(AmbientUserMode())
	{
		//	Is the balance within range?
		if(m_sBalance < -100)
			m_sBalance = -100;
		else if(m_sBalance > 100)
			m_sBalance = 100;

		//	Set the player rate
		m_Player.SetBalance(m_sBalance);
	}

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnCreate()
//
// 	Description:	This function is called by the framework when the control
//					window is created
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
int CTMMovieCtrl::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
	if(COleControl::OnCreate(lpCreateStruct) == -1)
		return -1;
	
	//	Initialize the error handler
	m_Errors.Enable(m_bEnableErrors);
	m_Errors.SetParent(m_hWnd);
	m_Errors.SetTitle("TMMOVIE Error");
	m_Errors.SetMessageId(m_bEnableAxErrors == TRUE ? WM_ERROR_NOTIFICATION : 0);
	
	//	Should we automatically initialize the control?
	if(m_bAutoInit && AmbientUserMode())
		Initialize();

	return 0;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnUseSnapshotsChanged()
//
// 	Description:	This function is called when the UseSnapshosts property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnUseSnapshotsChanged() 
{
	m_Player.EnableSnapshots(m_bUseSnapshots);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnVolumeChanged()
//
// 	Description:	This function is called when the Volume property changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::OnVolumeChanged() 
{
	if(AmbientUserMode())
	{
		//	Is the volume within range?
		if(m_sVolume < 0)
			m_sVolume = 0;
		else if(m_sVolume > 100)
			m_sVolume = 100;

		//	Set the player rate
		m_Player.SetVolume(m_sVolume);
	}

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::OnWMErrorNotification()
//
// 	Description:	This method is handles WM_ERROR_NOTIFICATION messages sent
//					by the error handler
//
// 	Returns:		Zero
//
//	Notes:			None
//
//==============================================================================
LONG CTMMovieCtrl::OnWMErrorNotification(WPARAM wParam, LPARAM lParam)
{
	if((m_bEnableAxErrors == TRUE) && (lstrlen(m_Errors.GetMessage()) > 0))
	{
		FireAxError(m_Errors.GetMessage());
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::Pause()
//
// 	Description:	This method is called to pause the playback
//
// 	Returns:		TMMOVIE_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::Pause() 
{
	//	Has the control been initialized ?
	if(!IsReady())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTINITIALIZED);
		return TMMOVIE_NOTINITIALIZED;
	}

	//	Don't bother if not loaded
	if(!IsLoaded())
		return TMMOVIE_NOERROR;

	//	Pause the playback
	if(!m_Player.Pause())
		return TMMOVIE_PAUSEFAILED;

	//	Update the current position
	UpdatePos();
	UpdateState();

	//	Make sure a state change event gets sent
	m_bNotify = TRUE;

	return TMMOVIE_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::Play()
//
// 	Description:	This method is called to start the playback
//
// 	Returns:		TMMOVIE_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::Play() 
{
	BOOL	bPlaying;
	double	dPosition = 0;
	int		iAttempts = 0;

	//	Has the control been initialized ?
	if(!IsReady())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTINITIALIZED);
		return TMMOVIE_NOTINITIALIZED;
	}

	//	Has a file been loaded ?
	if(!m_Player.IsLoaded())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTLOADED);
		return TMMOVIE_NOTLOADED;
	}

	//	Update the playlist state
	if(m_sPlaylistState == TMMOVIE_PLSET)
		SetPlaylistState(TMMOVIE_PLACTIVE);

	//	The user may be calling this method to start playing a playlist. Setting
	//	this flag insures that all designations continue to play
	m_bPlayDesignation = TRUE;

	//	Update the current position
	m_Player.GetPos(&dPosition);

	//	Are we supposed to stick to the playback range?
	if(m_bEnforceRange == TRUE)
	{
		while(1)
		{
			//	Is the current position within range?
			if(dPosition <= m_dStopPosition)
			{
				bPlaying = m_Player.Play(dPosition, m_dStopPosition);
				break;
			}
			else
			{
				//	Should we attempt to set the range again?
				if(iAttempts < 5)
				{
					AddDebugMessage("Enforcing playback range: Current = %f  Start = %f  Stop = %f  Max = %f", dPosition, m_dStartPosition, m_dStopPosition, m_Player.m_dMaxTime);
					SetRange(m_dStartPosition, m_dStopPosition);
					Sleep(200);

					iAttempts++;
				}
				else
				{
					AddDebugMessage("Unable to enforce playback range: Current = %f  Start = %f  Stop = %f  Max = %f", dPosition, m_dStartPosition, m_dStopPosition, m_Player.m_dMaxTime);

					//	Go ahead and try anyway
					bPlaying = m_Player.Play(dPosition, m_dStopPosition);
					break;
				}
			
			}// if(dPosition <= m_dStopPosition)

		}// while(1)

	}
	else
	{
		//	Play from the current position to the stop position
		if(dPosition < m_dStopPosition)
			bPlaying = m_Player.Play(dPosition, m_dStopPosition);

		//	Play from the current position to the end of the file
		else if(dPosition < m_Player.m_dMaxTime)
			bPlaying = m_Player.Play(dPosition, m_Player.m_dMaxTime);
		
		else
			bPlaying = m_Player.Play(m_dStartPosition, m_dStopPosition);

	}// if(m_bEnforceRange == TRUE)

	//	Clear the flag
	m_bEnforceRange = FALSE;

	//	Are we playing the video?
	if(!bPlaying)
		return TMMOVIE_PLAYFAILED;

	//	Make sure the video is visible
	if(m_bAutoShow && !m_Player.IsVisible())
		m_Player.Show(TRUE, FALSE);

	//	Make sure the timer is active
	StartTimer();

	//	Make sure a state change event gets sent
	m_bNotify = TRUE;

	return TMMOVIE_NOERROR;

}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::PlayPlaylist()
//
// 	Description:	This method is called to start playing a playlist at the
//					specified designation
//
// 	Returns:		TMPLAY_NOERROR if successful
//
//	Notes:			If a NULL playlist pointer is provided, playback of
//					the current playlist is cleared.
//
//==============================================================================
short CTMMovieCtrl::PlayPlaylist(long pPlaylist, long lStart, long lStop, 
								 double dPosition) 
{
	CDesignation* pDesignation;

	//	Has the control been initialized ?
	if(!IsReady())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTINITIALIZED);
		return TMMOVIE_NOTINITIALIZED;
	}

	//	Is the caller clearing the current playlist?
	if(pPlaylist == 0)
	{
		SetPlaylistState(TMMOVIE_PLNONE);
		return TMMOVIE_NOERROR;
	}

	//	Get the designation requested by the caller before we change anything
	if((pDesignation = ((CPlaylist*)pPlaylist)->GetDesignationFromOrder(lStart, TRUE)) == 0)
	{
		SetPlaylistState(TMMOVIE_PLERROR);
		m_Errors.Handle(0, IDS_TMMOVIE_INVALIDDESIGNATION);
		return TMMOVIE_INVALIDDESIGNATION;
	}

	//	Reset the pointers
	m_pPlaylist = (CPlaylist*)pPlaylist;
	m_pDesignation = pDesignation;
	m_pLink = 0;
	m_pLine = 0;

	//	Save the specified range
	m_lPlaylistStart = lStart;
	m_lPlaylistStop  = lStop;

	//	Set the playlist state
	SetPlaylistState(TMMOVIE_PLSET);

	//	Update the playlist times
	ResetPlaylistTimes(lStart);

	//	Do we want to automatically start playing the playlist?
	m_bPlayDesignation = m_bAutoPlay;

	//	Load the designation
	return LoadDesignation(dPosition);
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::ProcessEvent()
//
// 	Description:	This function will process all events recieved from the
//					DirectShow interface
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::ProcessEvent(long lEvent) 
{
	//	Which event has occurred?
	switch(lEvent)
	{
		case EC_COMPLETE:

			OnEvComplete();
			break;

		case EC_ERRORABORT:
		case EC_STREAM_ERROR_STOPPED:

			OnEvError(lEvent, TRUE);
			break;

		case EC_STREAM_ERROR_STILLPLAYING:

			OnEvError(lEvent, FALSE);
			break;

		case EC_CLOCK_CHANGED:
		case EC_USERABORT:
		case EC_TIME:
		case EC_REPAINT:
		case EC_ERROR_STILLPLAYING:
		case EC_PALETTE_CHANGED:
		case EC_VIDEO_SIZE_CHANGED:
		case EC_QUALITY_CHANGE:
		case EC_SHUTTING_DOWN:
		case EC_OPENING_FILE:
		case EC_BUFFERING_DATA:
		case EC_FULLSCREEN_LOST:
		case EC_ACTIVATE:
		case EC_NEED_RESTART:
		case EC_WINDOW_DESTROYED:
		case EC_DISPLAY_CHANGED:
		case EC_STARVATION:
		case EC_OLE_EVENT:
		case EC_NOTIFY_WINDOW:
		case EC_STREAM_CONTROL_STOPPED:
		case EC_STREAM_CONTROL_STARTED:
		case EC_END_OF_SEGMENT:
		case EC_SEGMENT_STARTED:
		case EC_LENGTH_CHANGED:
		default:

			break;

	}//	switch(lEvent)


	#if defined(_DEBUG)
	CString Event;
	Event.Format("%ld", lEvent);
	SendDebug("DS Event", Event, 0);
	#endif

}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::ReleaseAudioBitmap()
//
// 	Description:	This method is called to get the release the audio bitmap
//					resources
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::ReleaseAudioBitmap()
{
	//	Deallocate the existing bitmap handle
	if(m_hAudioBitmap)
	{
		DeleteObject(m_hAudioBitmap);
		m_hAudioBitmap = 0;
	}
	ZeroMemory(&m_bmAudioInfo, sizeof(m_bmAudioInfo));
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::RemoveFilter()
//
// 	Description:	This method is called to remove a filter to be added to the
//					graph builder before rendering
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::RemoveFilter(LPCTSTR lpszName) 
{
	ASSERT(lpszName);

	//	Has the control been initialized ?
	if(!IsReady())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTINITIALIZED);
		return TMMOVIE_NOTINITIALIZED;
	}

	//	Notify the player
	if(m_Player.RemoveFilter(lpszName))
		return TMMOVIE_NOERROR;
	else
		return TMMOVIE_REMOVEFILTERFAILED;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::ResetDesignationTimes()
//
// 	Description:	This function will reset the counters used to maintain
//					the designation time
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::ResetDesignationTimes(double dPosition) 
{
	//	Clear the current counters
	m_dDesignationTime		= 0.0;
	m_dElapsedDesignation	= 0.0;
	
	//	Do we have a valid designation?
	if(m_pDesignation)
	{
		m_dDesignationTime = m_pDesignation->GetTotalTime();
		if(dPosition >= 0)
		{
			m_dElapsedDesignation = m_pDesignation->GetElapsedTime(dPosition);
			m_dElapsedPlaylist += m_dElapsedDesignation;
			m_dTimePos = dPosition;
		}

	}
	
	//	Fire the time change event
	FireDesignationTime(m_dDesignationTime);
	FireElapsedTimes(m_dElapsedPlaylist, m_dElapsedDesignation);	
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::ResetPlaylistTimes()
//
// 	Description:	This function will calculate the time required to play the
//					playlist and initialize the elapsed time counter
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::ResetPlaylistTimes(double dStart) 
{
	//	Clear the current counters
	m_dPlaylistTime			= 0.0;
	m_dDesignationTime		= 0.0;
	m_dElapsedPlaylist		= 0.0;
	m_dElapsedDesignation	= 0.0;
	m_dTimePos				= 0;

	//	Do we have a valid playlist?
	if(m_pPlaylist)
	{
		//	Get the total time for this playlist
		m_dPlaylistTime = m_pPlaylist->GetTotalTime();

		//	Initialize the elapsed time counter
		if(m_pDesignation && dStart > 0)
			m_dElapsedPlaylist = m_pPlaylist->GetTime(-1, -1.0, 
													  m_pDesignation->m_lPlaybackOrder,
													  m_pDesignation->m_dStartTime);
	}
	
	//	Fire the time change event
	FirePlaylistTime(m_dPlaylistTime);	
	FireElapsedTimes(m_dElapsedPlaylist, m_dElapsedDesignation);
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::ResizeToRatio()
//
// 	Description:	This function will adjust the size of the specified 
//					rectangle to match the requested ratio.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::ResizeToRatio(CRect& rRect, float fRatio)
{
    float   fWidth;
    float   fHeight;
    int     fCx;
    int     fCy;

	ASSERT(fRatio != 0);
	if(fRatio == 0) return;

	//	Just playing is safe...
	if(rRect.Width()  <= 3) return;
	if(rRect.Height() <= 3) return;

	//  Find the center point of the rectangle
    fCx = (rRect.Width() / 2)  + rRect.left;
    fCy = (rRect.Height() / 2) + rRect.top;

	//  If we use the maximum height allowed, is the width still small
    //  enough to fit within the rectangle?
    if((((float)(rRect.Height())) * fRatio) < rRect.Width())
    {
        //  Use the full height allowed and adjust the width
		fHeight = (float)rRect.Height();
        fWidth  = fHeight * fRatio;
    }
    else
    {   
        //  Use the maximum width available and adjust the height
		fWidth = (float)rRect.Width();
		fHeight = fWidth / fRatio;
    }

    if((fWidth <= 0) || (fHeight <= 0)) return;

	//  Calculate the new coordinates of the rectangle
    rRect.left = (int)(fCx - (fWidth / 2));
    rRect.top  = (int)(fCy - (fHeight / 2));
	rRect.right = rRect.left + (int)fWidth;
	rRect.bottom = rRect.top + (int)fHeight;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::Resume()
//
// 	Description:	This method is called to resume the playback
//
// 	Returns:		TMMOVIE_NOERROR if successful
//
//	Notes:			There is actually no difference between calling Play() and
//					calling Resume()
//
//==============================================================================
short CTMMovieCtrl::Resume() 
{
	double dPosition;

	//	Has the control been initialized ?
	if(!IsReady())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTINITIALIZED);
		return TMMOVIE_NOTINITIALIZED;
	}

	//	Has a file been loaded ?
	if(!m_Player.IsLoaded())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTLOADED);
		return TMMOVIE_NOTLOADED;
	}

	//	Update the current position
	m_Player.GetPos(&dPosition);

	//	Have we reached the end of the segment?
	if(dPosition >= m_dStopPosition)
	{
		//	Are we playing a playlist?
		if(m_pDesignation && m_sPlaylistState == TMMOVIE_PLACTIVE)  
		{
			//	We are at the end of the active designation but go ahead and play so
			//	that the Complete event gets fired to load the next designation
			m_bEnforceRange = TRUE;
		}
		else
		{
			return TMMOVIE_ENDOFSEGMENT;
		}
	}

	//	Resume the playback
	if(Play() != TMMOVIE_NOERROR)
		return TMMOVIE_RESUMEFAILED;

	return TMMOVIE_NOERROR;

}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::SendDebug()
//
// 	Description:	This function is called to fire a debug message event
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::SendDebug(LPCSTR lpMsg1, LPCSTR lpMsg2, short sSleep) 
{
#if defined(_DEBUG)
	FireDebugMessage(lpMsg1, lpMsg2);
	Sleep((DWORD)(sSleep * 1000));
	FireDebugMessage("Last Msg", lpMsg2);
#endif	
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::SetDebugFilename()
//
// 	Description:	This method is called to construct the fully qualified path
//					to the file used for debugging messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::SetDebugFilename() 
{
	//	Get the folder containing where this control is registered
	if(CTMToolbox::GetParent(m_tmVersion.GetFileSpec(), m_strDebugFilename) == TRUE)
	{
		if(m_strDebugFilename.Right(1) != "\\")
			m_strDebugFilename += "\\";

		m_strDebugFilename += "_tm_movie_log.txt";
	}

}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::SetDefaultRate()
//
// 	Description:	This method is called to set the default frame rate to be
//					used by the player if no specific file is loaded
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::SetDefaultRate(double dFrameRate) 
{
	m_Player.m_fDefaultRate = (float)dFrameRate;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::SetFilters()
//
// 	Description:	This function is called to retrieve the filter 
//					the filter specifications from the ini file
//
// 	Returns:		TMMOVIE_NOERROR if successful
//
//	Notes:			After loading, the cue range is set to the full file 
//					extents. This method can be called to redefine the cue range
//
//==============================================================================
void CTMMovieCtrl::SetFilters() 
{
	char szLine[16];
	char szIniStr[256];
	int  i;
	
	//	Add MPEG-1 playback filters to override system defaults
	//
	//	NOTE:	We do this because many of the modern video editors install custom
	//			MPEG-1 filters that do not seem to be able to handle frame based positioning
	m_Player.AddFilter("MPEG Video Decoder");
	m_Player.AddFilter("MPEG-I Stream Splitter");

	//	Assign a default name if no filename is specified
	if(m_strIniFile.IsEmpty())
		m_strIniFile = DEFAULT_TMAXINI;

	//	Attempt to open the file
	if(!m_Ini.Open(m_strIniFile, TMMOVIE_FILTERS_SECTION)) return;

	//	First read the filters we are supposed to remove. This allows the application
	//	to remove filters that TMMovie adds by default
	for(i = 1; ; i++)
	{
		//	Format the line identifier
		sprintf_s(szLine, sizeof(szLine), "REMOVE%d", i);

		//	Read the line
		m_Ini.ReadString(szLine, szIniStr, sizeof(szIniStr));

		//	Have we run out of filters to remove
		if(lstrlen(szIniStr) == 0)
			break;

		//	Remove this filter
		m_Player.RemoveFilter(szIniStr);

	}

	//	Now read the filters we are supposed to add. These filters will override the
	//	default filters defined in the registry when TMMovie renders a new file
	for(i = 1; ; i++)
	{
		//	Format the line identifier
		sprintf_s(szLine, sizeof(szLine), "ADD%d", i);

		//	Read the line
		m_Ini.ReadString(szLine, szIniStr, sizeof(szIniStr));

		//	Have we run out of filters to remove
		if(lstrlen(szIniStr) == 0)
			break;

		//	Add this filter
		m_Player.AddFilter(szIniStr);

	}

}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::SetMaxCuePosition()
//
// 	Description:	This method is called to set the maximum frame to which
//					the video may be cued
//
// 	Returns:		TMMOVIE_NOERROR if successful
//
//	Notes:			After loading, the cue range is set to the full file 
//					extents. This method can be called to redefine the cue range
//
//==============================================================================
short CTMMovieCtrl::SetMaxCuePosition(double dPosition)
{
	//	Has the control been initialized ?
	if(!IsReady())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTINITIALIZED);
		return TMMOVIE_NOTINITIALIZED;
	}

	//	Make sure the new value is within range
	if(dPosition > m_Player.m_dMaxTime)
		dPosition = m_Player.m_dMaxTime;
	if(dPosition <= m_dMinimumCue)
		dPosition = m_Player.m_dMaxTime;

	//	Save the new value
	m_dMaximumCue = dPosition;

	return TMMOVIE_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::SetMinCuePosition()
//
// 	Description:	This method is called to set the minimum time to which
//					the video may be cued
//
// 	Returns:		TMMOVIE_NOERROR if successful
//
//	Notes:			After loading, the cue range is set to the full file 
//					extents. This method can be called to redefine the cue range
//
//==============================================================================
short CTMMovieCtrl::SetMinCuePosition(double dPosition) 
{
	//	Has the control been initialized ?
	if(!IsReady())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTINITIALIZED);
		return TMMOVIE_NOTINITIALIZED;
	}

	//	Make sure the new value is within range
	if(dPosition < m_Player.m_dMinTime)
		dPosition = m_Player.m_dMinTime;
	if(dPosition >= m_dMaximumCue)
		dPosition = m_Player.m_dMinTime;

	m_dMinimumCue = dPosition;

	return TMMOVIE_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::SetOverlayPos()
//
// 	Description:	This function set the size and position of the overlay
//					using the rectangle provided by the caller as the maximum
//					extents
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMMovieCtrl::SetOverlayPos(RECT* pRect)
{	
	int		iMaxWidth;
	int		iMaxHeight;
	int		iHeight;

	ASSERT(pRect);
	
	//	Do we have a valid overlay window?
	if(!m_pOverlay || !IsWindow(m_pOverlay->m_hWnd))
		return FALSE;

	//	Calculate the width and height of the available viewport
	if((iMaxWidth = pRect->right - pRect->left) == 0)
		return FALSE;
	if((iMaxHeight = pRect->bottom - pRect->top) == 0)
		return FALSE;

	//	Set the maximum width in the overlay object
	if((iHeight = m_pOverlay->SetMaxWidth(iMaxWidth)) == 0)
		return FALSE;
	if(iHeight > iMaxHeight)
		iHeight = iMaxHeight;

	//	Calculate the new overlay rectangle
	m_rcOverlay.left   = 0;
	m_rcOverlay.right  = iMaxWidth;
	m_rcOverlay.bottom = iMaxHeight;
	m_rcOverlay.top    = m_rcOverlay.bottom - iHeight;

	//	Reposition the overlay
	m_pOverlay->MoveWindow(&m_rcOverlay);

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::SetPlaylistFrame()
//
// 	Description:	This function will set the position of the playlist to the
//					value specified by the caller. 
//
// 	Returns:		None
//
//	Notes:			The position specified by the caller must be within the 
//					range defined by the current designation.
//
//==============================================================================
void CTMMovieCtrl::SetPlaylistPosition(double dPosition)
{	
	//	Set the start position to the value specified by the caller. Make sure
	//	there is a gap of at least one frame between the start and stop frame
	m_dStartPosition = dPosition;
	if(m_dStartPosition >= m_dStopPosition)
		m_dStartPosition = m_dStopPosition - IDXSHOW_SECONDS_PER_FRAME;
	OnStartPositionChanged();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::SetPlaylistLine()
//
// 	Description:	This function will reset the current page and line to the
//					position specified by the caller
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::SetPlaylistLine(double dPosition)
{	
	CTextLine*	pPrev = 0;

	ASSERT(m_pDesignation);
	if(!m_pDesignation) return;

	//	Clear the current pointers
	m_pLine = 0;

	//	Don't bother if we don't have any lines
	if((m_pLine = m_Lines.First()) == 0)
		return;

	//	Is the caller requesting the first line?
	if((dPosition < 0) || (dPosition <= m_pLine->m_dStartTime))
	{
		//	Fire the change event
		FireLineChange((long)m_pLine);

		//	Set up for the next line
		m_pLine = m_Lines.Next();
	}
	else
	{
		//	Look for the first line that goes beyond our current position
		while(m_pLine && m_pLine->m_dStartTime < dPosition)
		{
			//	Get the next line
			pPrev = m_pLine;
			m_pLine = m_Lines.Next();
		}

		//	Fire the change event. If we weren't able to locate the appropriate
		//	line, the video must be positioned ahead of the first line. In this
		//	case we will use the first line
		if(pPrev)
		{
			FireLineChange((long)pPrev);
		}
		else
		{
			ASSERT(m_pLine);
			FireLineChange((long)pPrev);
			m_pLine = m_Lines.Next();
		}

	}//if(dPosition < 0)
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::SetPlaylistLink()
//
// 	Description:	This function will reset the current link to the position
//					indicated by the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::SetPlaylistLink(double dPosition)
{	
	CDesignation*	pDesignation;
	CLink*			pPrevLink = 0;
	long			lIndex;

	ASSERT(m_pDesignation);

	//-------------------------------------------------------------------------
	//	We have to locate the link that is associated with this position.
	//	This might be tricky because the link associated with this position
	//	may have actually been declared in a previous designation
	//-------------------------------------------------------------------------

	//	First we check for the special case where we are positioned on the 
	//	start of the current designation and the first link of the designation
	//	coincides with the start
	if(dPosition < 0)
	{
		//	Get the first link in the list
		pPrevLink = m_pDesignation->FirstLink();

		//	Does this link coincide with the start of the current designation?
		if(pPrevLink)
		{
			//	Does this link coincide with the start of the designation?
			if(pPrevLink->m_dTrigger <= m_dStartPosition)
			{
				//	Fire the event to notify the container
				FireLinkChange(pPrevLink->m_strItemBarcode, pPrevLink->m_lId,
							   pPrevLink->m_lFlags);

				//	Now set up the pending link for playback
				m_pLink = m_pDesignation->NextLink();

				return;
			}
			else
			{
				//	This link is no longer valid
				pPrevLink = 0;
			}

		}
	}
	else
	{
		//	Iterate the current designation's links in reverse until we find the
		//	link that precedes the current position
		pPrevLink = m_pDesignation->LastLink();
		while(pPrevLink != 0)
		{
			//	Is this link in front of the current position?
			if(pPrevLink->m_dTrigger <= dPosition)
			{
				//	Fire the event to notify the container
				FireLinkChange(pPrevLink->m_strItemBarcode, pPrevLink->m_lId,
							   pPrevLink->m_lFlags);

				//	Now set up the pending link for playback
				m_pLink = m_pDesignation->NextLink();
				return;
			}
			else
			{
				//	Get the previous link in the list
				pPrevLink = m_pDesignation->PrevLink();
			}

		}//	while(pPrevLink != 0)

	}// if(dPosition < 0)

	//	The fact that we got this far means that the link associated with the
	//	current position (if any) must have come from a previous designation.
	//	We start by setting up the pending link since we know it has to be the
	//	first link associated with the current designation
	m_pLink = m_pDesignation->FirstLink();

	//	Save the index of the current designation
	lIndex = m_pDesignation->m_lPlaybackOrder;

	//	Reposition the list on the current designation but this time set it
	//	up for reverse iteration
	pDesignation = m_pPlaylist->GetDesignationFromOrder(lIndex, FALSE);

	//	Now iterate in reverse in search of a designation with a link
	pDesignation = m_pPlaylist->GetPrevDesignation();
	while(pDesignation != 0)
	{
		//	The last link of the earlier designation is what we're looking for
		if((pPrevLink = pDesignation->LastLink()) != 0)
			break;
		else
			pDesignation = m_pPlaylist->GetPrevDesignation();
	}

	//	Fire the link event
	if(pPrevLink)
		FireLinkChange(pPrevLink->m_strItemBarcode, pPrevLink->m_lId,
					   pPrevLink->m_lFlags);
	else
		FireLinkChange("", -1, 0);

	//	Reset the playlist to position its iterator on the current 
	//	designation and the list is set up for forward iteration
	pDesignation = m_pPlaylist->GetDesignationFromOrder(lIndex, TRUE);
	ASSERT(m_pDesignation == pDesignation);
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::SetPlaylistRange()
//
// 	Description:	This method is called to reset the playlist range.
//
// 	Returns:		None
//
//	Notes:			This method should only be called AFTER calling starting a
//					playlist with PlayPlaylist(). 
//
//==============================================================================
short CTMMovieCtrl::SetPlaylistRange(long lStart, long lStop) 
{
	//	Has the control been initialized ?
	if(!IsReady())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTINITIALIZED);
		return TMMOVIE_NOTINITIALIZED;
	}

	//	We must have a playlist
	if(!m_pPlaylist)
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOPLAYLIST);
		return TMMOVIE_NOPLAYLIST;
	}

	//	Save the new range
	m_lPlaylistStart = lStart;
	m_lPlaylistStop  = lStop;

	//	Do we need to stop the current playlist?
	if(m_lPlaylistStop > 0 && m_pDesignation && 
	   m_pDesignation->m_lPlaybackOrder > m_lPlaylistStop)
	{
		SetPlaylistState(TMMOVIE_STOPPED);
		Stop();
	}

	return TMMOVIE_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::SetPlaylistState()
//
// 	Description:	This function will set the playlist state and fire an
//					event to notify the container.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::SetPlaylistState(short sState)
{	
	m_sPlaylistState = sState;

	//	What state are we in?
	switch(m_sPlaylistState)
	{
		case TMMOVIE_PLNONE:
			
			//	Reset the pointers
			m_pPlaylist = 0;
			m_pDesignation = 0;
			m_pLink = 0;
			m_pLine = 0;
			m_lPlaylistStart = -1;
			m_lPlaylistStop = -1;
			m_Lines.Flush(FALSE);
			ResetPlaylistTimes(0);
			m_strOverlay.Empty();
			OnOverlayFileChanged();
			break;

		case TMMOVIE_PLERROR:
		case TMMOVIE_PLFINISHED:

			//	Reset the pointers
			m_pDesignation = 0;
			m_pLink = 0;
			m_pLine = 0;
			break;

		case TMMOVIE_PLSET:
		case TMMOVIE_PLACTIVE:
		case TMMOVIE_PLSTOPPED:

			break;
	}
	
	//	Fire the state change event
	FirePlaylistState(m_sPlaylistState); 	
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::SetRange()
//
// 	Description:	This method is called to set the playback range
//
// 	Returns:		TMMOVIE_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::SetRange(double dStart, double dStop) 
{	
	//	Save the new range
	m_dStartPosition = dStart;
	m_dStopPosition  = dStop;

	//	Has the control been initialized? If not, we don't bother generating
	//	an error because these values won't actually be used until the player
	//	is loaded
	if(!m_Player.IsLoaded())
		return TMMOVIE_NOERROR;

	//	Set the player range
	m_Player.SetRange(dStart, dStop);

	//	Update the local members using the actual player values
	if(!m_Player.GetStartPos(&m_dStartPosition))
		m_dStartPosition = 0;
	if(!m_Player.GetStopPos(&m_dStopPosition))
		m_dStopPosition = 0;

	return TMMOVIE_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::SetState()
//
// 	Description:	This function will set the player state and fire an
//					event to notify the container.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::SetState(short sState)
{	
	//	Has the state changed?
	if(m_sState == sState)
	{
		//	Do we have to send notification anyway?
		if(m_bNotify)
		{
			FireStateChange(sState); 
			m_bNotify = FALSE;
		}	

		return;
	}

	//	What is the new state?
	switch(sState)
	{
		case TMMOVIE_PLAYING:

			break;

		case TMMOVIE_PAUSED:

			break;

		case TMMOVIE_STOPPED:

			break;

		case TMMOVIE_READY:

			//	Start the update timer
			StartTimer();

			break;

		case TMMOVIE_NOTREADY:

			//	Kill the timer
			StopTimer();

			//	Reset the runtime flags
			m_bPlayDesignation	= FALSE;
			
			//	Set the playlist state
			SetPlaylistState(TMMOVIE_PLNONE);

			//	Reset the frame information
			m_dPosition		= 0;
			m_dMinimumCue	= 0;
			m_dMaximumCue	= 0;
			FirePositionChange(m_dPosition);
			
			//	Unload the player
			m_Player.Unload();

			break;

		default:
			
			return;
	}
	
	//	Save the new state
	m_sState = sState;
	
	//	Fire the state change event
	FireStateChange(sState);
	m_bNotify = FALSE; 	
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::ShowFilterInfo()
//
// 	Description:	This method is called to display a dialog box that will
//					show the registered and active filter information
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::ShowFilterInfo() 
{
	CFilterInfo	FilterInfo(this);

	//	Get the registered filters
	m_Player.GetRegFilters(FilterInfo.m_strRegFilters,
						   &FilterInfo.m_lRegFilters);
	
	//	Pause the playback
	m_Player.Pause();

	//	Get the active filters
	m_Player.GetActFilters(FilterInfo.m_strActFilters,
						   &FilterInfo.m_lActFilters, TRUE);

	//	Set the media control interface
	FilterInfo.m_pIMediaControl = (IMediaControl*)m_Player.GetInterface(TMMOVIE_IMEDIACONTROL);

	FilterInfo.DoModal();
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::ShowSnapshot()
//
// 	Description:	This method is called to open a window containing a 
//					snapshot of the current frame
//
// 	Returns:		A handle to the window if successful
//
//	Notes:			None
//
//==============================================================================
OLE_HANDLE CTMMovieCtrl::ShowSnapshot() 
{
	CSnapshot*	pSnapshot;
	CString		strTitle;
	double		dPosition;

	//	Don't bother if not loaded
	if(!IsLoaded())
		return NULL;

	//	Get the current position
	if(!m_Player.GetPos(&dPosition))
		dPosition = m_dPosition;

	//	Get a new snapshot
	if((pSnapshot = m_Player.GetSnapshot(TRUE)) == 0)
		return NULL;

	//	Set the snapshot title
	strTitle.Format("%s: %ld", m_Player.m_strFilename, dPosition);
	pSnapshot->SetWindowText(strTitle);
		
	//	Center the snapshot
	pSnapshot->CenterWindow();

	//	Show the new snapshot
	pSnapshot->ShowWindow(SW_SHOW);

	return (OLE_HANDLE)pSnapshot->m_hWnd;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::ShowVideo()
//
// 	Description:	This method is called to control the visibility of the
//					video playback window.
//
// 	Returns:		None
//
//	Notes:			This is not the same as ShowWindow(). ShowWindow is called
//					to control the visibility of the whole control window. This
//					method controls the visibility of the video rendering 
//					window.
//
//==============================================================================
void CTMMovieCtrl::ShowVideo(BOOL bShow) 
{
	//	Set the state of the video
	m_Player.Show(bShow, TRUE);
	
	//	Do we have an overlay window?
	if(!m_pOverlay || !IsWindow(m_pOverlay->m_hWnd))
		return;

	//	Are we showing the video?
	if(bShow)
	{
		//	Are we supposed to show the overlay?
		if(!m_bOverlayVisible)
			return;

		//	Make sure it's visible and on top
		m_pOverlay->ShowWindow(SW_SHOW);
		m_pOverlay->Redraw();
		m_pOverlay->BringWindowToTop();
	}
	else
	{
		//	Make sure the overlay is not visible
		m_pOverlay->ShowWindow(SW_HIDE);
	}
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::ShowVideoProps()
//
// 	Description:	This function will open a dialog box that displays the
//					properties of the current video
//
// 	Returns:		TMMOVIE_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::ShowVideoProps() 
{
	CVideoProperties	Props(this);
	double				dHours;
	double				dMinutes;
	double				dSeconds;

	//	Has the control been initialized ?
	if(!IsReady())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTINITIALIZED);
		return TMMOVIE_NOTINITIALIZED;
	}

	//	Has a file been loaded ?
	if(!m_Player.IsLoaded())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTLOADED);
		return TMMOVIE_NOTLOADED;
	}

	dHours = m_Player.m_dMaxTime / 3600.0;
	dMinutes = (m_Player.m_dMaxTime - (dHours * 3600.0)) / 60.0;
	dSeconds = (m_Player.m_dMaxTime - (dHours * 3600.0) - (dMinutes * 60));

	Props.m_strFilename = m_strFilename;
	Props.m_strHeight.Format("%d pixels", GetSrcHeight());
	Props.m_strWidth.Format("%d pixels", GetSrcWidth());
	Props.m_strAspectRatio.Format("%.3f (W/H)", m_Player.m_fAspectRatio);
	Props.m_strFrames.Format("%ld", m_Player.ConvertToFrames((m_Player.m_dMaxTime - m_Player.m_dMinTime)));
	Props.m_strRate.Format("%.3f", m_Player.m_fFrameRate);
	Props.m_strTime.Format("%d:%0.2d:%0.2d", (int)dHours, (int)dMinutes, (int)dSeconds);

	Props.DoModal();

	return TMMOVIE_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::StartTimer()
//
// 	Description:	This function will start the update timer
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::StartTimer() 
{
	//	Has the timer already been activated?
	if(m_uTimer > 0)
		return;

	//	Start the timer using the requested update interval
	if((m_uTimer = SetTimer(1, m_sUpdateRate, NULL)) == 0) 
		m_Errors.Handle(0, IDS_TMMOVIE_TIMERFAILED); 

}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::Step()
//
// 	Description:	This method is called to step the video between two frames
//
// 	Returns:		TMMOVIE_NOERROR if successful
//
//	Notes:			This method is the same as playing the video from one frame
//					to another
//
//==============================================================================
short CTMMovieCtrl::Step(double dFrom, double dTo) 
{
	double dPosition = 0;

	//	Has the control been initialized ?
	if(!IsReady())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTINITIALIZED);
		return TMMOVIE_NOTINITIALIZED;
	}

	//	Has a file been loaded ?
	if(!m_Player.IsLoaded())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTLOADED);
		return TMMOVIE_NOTLOADED;
	}

	//	Does the user want to step from the current position?
	if(dFrom < 0)
	{
		m_Player.GetPos(&dPosition);
		dFrom = dPosition;
		dTo += dFrom;
	}
	
	//	Make sure we are within range for the current file
	if(dFrom < m_Player.m_dMinTime)
		dFrom = m_Player.m_dMinTime;
	if(dTo > m_Player.m_dMaxTime)
		dTo = m_Player.m_dMaxTime;

	//	Make sure we are not trying to step in reverse
	if(dFrom > dTo)
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOREVERSESTEP);
		return TMMOVIE_NOREVERSESTEP;
	}

	//	Step the video
	if(!m_Player.Step(dFrom, dTo))
		return TMMOVIE_STEPFAILED;
	else
		return TMMOVIE_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::Stop()
//
// 	Description:	This method is called to stop the playback
//
// 	Returns:		TMMOVIE_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::Stop() 
{
	//	Has the control been initialized ?
	if(!IsReady())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTINITIALIZED);
		return TMMOVIE_NOTINITIALIZED;
	}

	//	Don't bother if not loaded
	if(!IsLoaded())
		return TMMOVIE_NOERROR;

	//	Stop the playback
	if(!m_Player.Stop())
		return TMMOVIE_STOPFAILED;

	//	Update the current position
	UpdatePos();

	//	Make sure a state change event gets sent
	m_bNotify = TRUE;

	return TMMOVIE_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::StopTimer()
//
// 	Description:	This function is called to stop the update timer
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::StopTimer() 
{
	//	Stop the timer if it's active
	if(m_uTimer > 0)
		KillTimer(m_uTimer);

	m_uTimer = 0;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::Unload()
//
// 	Description:	This method is called to unload the current video
//
// 	Returns:		TMMOVIE_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::Unload() 
{
	//	Has the control been initialized ?
	if(!IsReady())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTINITIALIZED);
		return TMMOVIE_NOTINITIALIZED;
	}

	//	Unload the player
	m_Player.Unload();
	m_strFilename.Empty();
	
	//	Return to the initialized state
	SetState(TMMOVIE_READY);

	return TMMOVIE_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::Update()
//
// 	Description:	This method is called to update the rendering window
//
// 	Returns:		TMMOVIE_NOERROR if successful
//
//	Notes:			Applications should not need to call this method if the 
//					AutoShow property is TRUE. If not, and if cueing the video
//					with the rendering window hidden, it may be necessary to
//					update the window when it is made visible
//
//==============================================================================
short CTMMovieCtrl::Update() 
{
	//	Has the control been initialized ?
	if(!IsReady())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTINITIALIZED);
		return TMMOVIE_NOTINITIALIZED;
	}

	//	Has a file been loaded ?
	if(!m_Player.IsLoaded())
	{
		m_Errors.Handle(0, IDS_TMMOVIE_NOTLOADED);
		return TMMOVIE_NOTLOADED;
	}

	//	Update the display
	return (m_Player.Update()) ? TMMOVIE_NOERROR : TMMOVIE_UPDATEFAILED;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::UpdateElapsedTimes()
//
// 	Description:	This function will update the elapsed time counters
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::UpdateElapsedTimes() 
{
	double dElapsed;

	//	Do we have a valid designation and has the position changed since the
	//	last update?
	if(m_pDesignation && (m_dPosition > m_dTimePos))
	{
		//	Get the time elapsed in this designation
		dElapsed = m_pDesignation->GetElapsedTime(m_dPosition); 

		//	Add the difference between the last update and now to the playlist
		m_dElapsedPlaylist += (dElapsed - m_dElapsedDesignation);

		//	Update the designation
		m_dElapsedDesignation = dElapsed;

		m_dTimePos = m_dPosition;
	
		FireElapsedTimes(m_dElapsedPlaylist, m_dElapsedDesignation);
	}
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::UpdatePos()
//
// 	Description:	This function will update the current position and notify
//					the container
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMMovieCtrl::UpdatePos()
{	
	double dPosition;

	if(!m_Player.GetPos(&dPosition))
		return;

	//	This check is a workaround for to keep from firing a position change
	//	that is not actually correct. Sometimes when we set the range, or
	//	step the video, the interface will report that it's on the last frame
	//	even though it's really not. This only occurs until the playback resumes.
	//	this workaround allows for at least one update interval to expire before
	//	we notify the user that the video is on the last frame. We can't ignore
	//	it altogether because we may actually be on the last frame
	//
	//	This anomoly only seems to occur if we redefine the range such that the
	//	new start position coincides with the current stop position and the new
	//	stop position is the last frame
	if(!m_bLastFrame && dPosition == m_Player.m_dMaxTime)
	{
		m_bLastFrame = TRUE;
		return;
	}

	//	Clear the workaround flag
	m_bLastFrame = FALSE;

	if(dPosition != m_dPosition)
	{
		m_dPosition = dPosition;
		FirePositionChange(m_dPosition);
	}
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::UpdateScreenPosition()
//
// 	Description:	This method is called to make sure the video window is
//					properly sized and positioned
//
// 	Returns:		TMMOVIE_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::UpdateScreenPosition() 
{
	if(IsWindow(m_hWnd))
	{
		//	Hide the caption while we resize
		if(m_pOverlay && m_pOverlay->IsWindowVisible())
			m_pOverlay->ShowWindow(SW_HIDE);

		//	Resize the playback window
		m_Player.Resize();

		//	Now restore the overlay
		OnOverlayVisibleChanged();
	}

	return TMMOVIE_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMMovieCtrl::UpdateState()
//
// 	Description:	This function will update the control playback state
//
// 	Returns:		The new state
//
//	Notes:			None
//
//==============================================================================
short CTMMovieCtrl::UpdateState()
{	
	short sState;

	//	Get the state from the player?
	if(m_Player.GetState(&sState))
		SetState(sState);
	return m_sState;
}

