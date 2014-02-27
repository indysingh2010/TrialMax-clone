//==============================================================================
//
// File Name:	tmpower.cpp
//
// Description:	This file contains member functions of the CTMPowerCtrl class.
//
// See Also:	tmpower.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	05-09-99	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmpower.h>
#include <tpowerap.h>
#include <tpowerpg.h>
#include <tmppdefs.h>
#include <snapshot.h>
#include <regcats.h>
#include <dispid.h>
#include <filever.h>
#include <toolbox.h>
#include <TlHelp32.h>
#include <windows.h>

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
extern CTMPowerApp NEAR theApp;

//	These global variables are used to initialize and shut down the PowerPoint
//	dispatch interfaces
static HWND				_hPPWnd = 0;
static float			_fPPLeft = 0.0f;
static float			_fPPTop = 0.0f;
static float			_fPPWidth = 0.0f;
static float			_fPPHeight = 0.0f;
static long				_lPPState = 0;
static long				_lAttachments = 0;
static BOOL				_bPPActive = FALSE;
static CString			_strPPCaption;
static DWORD			_dwPPThread = 0;

/* Replace 2 */
const IID BASED_CODE IID_DTMPower6 =
		{ 0xd6cbe90, 0x9dd2, 0x4ac9, { 0x91, 0xa7, 0x19, 0x9a, 0x3e, 0x5e, 0xd3, 0xf0 } };
/* Replace 3 */
const IID BASED_CODE IID_DTMPower6Events =
		{ 0xbbfecf38, 0x3de1, 0x498b, { 0x9d, 0x96, 0x94, 0x6c, 0x40, 0xed, 0x2a, 0xc8 } };

// Control type information
static const DWORD BASED_CODE _dwTMPowerOleMisc =
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
BEGIN_MESSAGE_MAP(CTMPowerCtrl, COleControl)
	//{{AFX_MSG_MAP(CTMPowerCtrl)
	ON_WM_CREATE()
	ON_WM_SIZE()
	ON_WM_DESTROY()
	//}}AFX_MSG_MAP
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
	ON_MESSAGE(WM_ERROR_EVENT, OnWMErrorEvent)
	//ON_WM_PAINT()
END_MESSAGE_MAP()

// Dispatch map
BEGIN_DISPATCH_MAP(CTMPowerCtrl, COleControl)
	//{{AFX_DISPATCH_MAP(CTMPowerCtrl)
	DISP_PROPERTY_NOTIFY(CTMPowerCtrl, "AutoInit", m_bAutoInit, OnAutoInitChanged, VT_BOOL)
	DISP_PROPERTY_EX(CTMPowerCtrl, "VerBuild", GetVerBuild, SetNotSupported, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMPowerCtrl, "EnableErrors", m_bEnableErrors, OnEnableErrorsChanged, VT_BOOL)
	DISP_PROPERTY_EX(CTMPowerCtrl, "VerMajor", GetVerMajor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMPowerCtrl, "VerMinor", GetVerMinor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMPowerCtrl, "VerTextLong", GetVerTextLong, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMPowerCtrl, "SplitScreen", m_bSplitScreen, OnSplitScreenChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPowerCtrl, "SplitFrameThickness", m_sSplitFrameThickness, OnSplitFrameThicknessChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMPowerCtrl, "SplitFrameColor", m_lSplitFrameColor, OnSplitFrameColorChanged, VT_COLOR)
	DISP_PROPERTY_NOTIFY(CTMPowerCtrl, "RightFile", m_strRightFile, OnRightFileChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMPowerCtrl, "LeftFile", m_strLeftFile, OnLeftFileChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMPowerCtrl, "SyncViews", m_bSyncViews, OnSyncViewsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPowerCtrl, "ActiveView", m_sActiveView, OnActiveViewChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMPowerCtrl, "StartSlide", m_lStartSlide, OnStartSlideChanged, VT_I4)
	DISP_PROPERTY_NOTIFY(CTMPowerCtrl, "EnableAccelerators", m_bEnableAccelerators, OnEnableAcceleratorsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPowerCtrl, "UseSlideId", m_bUseSlideId, OnUseSlideIdChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPowerCtrl, "SaveFormat", m_sSaveFormat, OnSaveFormatChanged, VT_I2)
	DISP_FUNCTION(CTMPowerCtrl, "Initialize", Initialize, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMPowerCtrl, "IsInitialized", IsInitialized, VT_BOOL, VTS_NONE)
	DISP_FUNCTION(CTMPowerCtrl, "GetPPVersion", GetPPVersion, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMPowerCtrl, "GetPPBuild", GetPPBuild, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMPowerCtrl, "Next", Next, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMPowerCtrl, "Previous", Previous, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMPowerCtrl, "First", First, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMPowerCtrl, "Last", Last, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMPowerCtrl, "GetCurrentSlide", GetCurrentSlide, VT_I4, VTS_I2)
	DISP_FUNCTION(CTMPowerCtrl, "GetSlideCount", GetSlideCount, VT_I4, VTS_I2)
	DISP_FUNCTION(CTMPowerCtrl, "Close", Close, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMPowerCtrl, "Unload", Unload, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMPowerCtrl, "GetFilename", GetFilename, VT_BSTR, VTS_I2)
	DISP_FUNCTION(CTMPowerCtrl, "GetBitmap", GetBitmap, VT_I4, VTS_I4 VTS_I4 VTS_I2)
	DISP_FUNCTION(CTMPowerCtrl, "ShowSnapshot", ShowSnapshot, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMPowerCtrl, "SaveSlide", SaveSlide, VT_I2, VTS_BSTR VTS_I2)
	DISP_FUNCTION(CTMPowerCtrl, "CopySlide", CopySlide, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMPowerCtrl, "SetData", SetData, VT_EMPTY, VTS_I2 VTS_I4)
	DISP_FUNCTION(CTMPowerCtrl, "GetData", GetData, VT_I4, VTS_I2)
	DISP_FUNCTION(CTMPowerCtrl, "SetFocusWnd", SetFocusWnd, VT_I2, VTS_HANDLE)
	DISP_FUNCTION(CTMPowerCtrl, "Show", Show, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMPowerCtrl, "GetCurrentState", GetCurrentState, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMPowerCtrl, "SetSlide", SetSlide, VT_I2, VTS_I2 VTS_I4 VTS_I2)
	DISP_FUNCTION(CTMPowerCtrl, "LoadFile", LoadFile, VT_I2, VTS_BSTR VTS_I4 VTS_I2 VTS_I2)
	DISP_FUNCTION(CTMPowerCtrl, "GetSlideNumber", GetSlideNumber, VT_I4, VTS_I2 VTS_I4)
	DISP_FUNCTION(CTMPowerCtrl, "GetClassIdString", GetClassIdString, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMPowerCtrl, "GetRegisteredPath", GetRegisteredPath, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMPowerCtrl, "GetAnimationCount", GetAnimationCount, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMPowerCtrl, "GetAnimationIndex", GetAnimationIndex, VT_I2, VTS_I2)
	DISP_STOCKPROP_BACKCOLOR()
	DISP_STOCKPROP_BORDERSTYLE()
	//}}AFX_DISPATCH_MAP
	
	//	Added in rev 6.0
	DISP_PROPERTY_NOTIFY_ID(CTMPowerCtrl, "HideTaskBar", DISPID_HIDE_TASK_BAR, m_bHideTaskBar, OnHideTaskBarChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY_ID(CTMPowerCtrl, "EnableAxErrors", DISPID_ENABLE_AX_ERRORS, m_bEnableAxErrors, OnEnableAxErrorsChanged, VT_BOOL)

	//	Added rev 6.1.0
	DISP_PROPERTY_EX_ID(CTMPowerCtrl, "VerTextShort", DISPID_VERTEXTSHORT, GetVerTextShort, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_EX_ID(CTMPowerCtrl, "VerQEF", DISPID_VERQEF, GetVerQEF, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX_ID(CTMPowerCtrl, "VerBuildDate", DISPID_VERBUILDDATE, GetVerBuildDate, SetNotSupported, VT_BSTR)

END_DISPATCH_MAP()

// Event map
BEGIN_EVENT_MAP(CTMPowerCtrl, COleControl)
	//{{AFX_EVENT_MAP(CTMPowerCtrl)
	EVENT_CUSTOM("SelectView", FireSelectView, VTS_I2)
	EVENT_CUSTOM("FileChanged", FireFileChanged, VTS_BSTR  VTS_I2)
	EVENT_CUSTOM("SlideChanged", FireSlideChanged, VTS_I4  VTS_I2)
	EVENT_CUSTOM("ViewFocus", FireViewFocus, VTS_I2)
	EVENT_CUSTOM("StateChanged", FireStateChanged, VTS_I2  VTS_I2)
	EVENT_CUSTOM("AxError", FireAxError, VTS_BSTR)
	EVENT_CUSTOM("AxDiagnostic", FireAxDiagnostic, VTS_BSTR  VTS_BSTR)
	//}}AFX_EVENT_MAP
END_EVENT_MAP()

// Property pages
BEGIN_PROPPAGEIDS(CTMPowerCtrl, 2)
	PROPPAGEID(CTMPowerProperties::guid)
	PROPPAGEID(CLSID_CColorPropPage)
END_PROPPAGEIDS(CTMPowerCtrl)

/* Replace 4 */
IMPLEMENT_OLECREATE_EX(CTMPowerCtrl, "TMPOWER6.TMPowerCtrl.1",
	0xbd138fdb, 0x21b2, 0x4cf1, 0x81, 0x75, 0xa9, 0x41, 0x82, 0xfe, 0xd7, 0x81)

// Type library ID and version
IMPLEMENT_OLETYPELIB(CTMPowerCtrl, _tlid, _wVerMajor, _wVerMinor)
IMPLEMENT_OLECTLTYPE(CTMPowerCtrl, IDS_TMPOWER, _dwTMPowerOleMisc)

IMPLEMENT_DYNCREATE(CTMPowerCtrl, COleControl)

// Interface map for IObjectSafety
BEGIN_INTERFACE_MAP(CTMPowerCtrl, COleControl )
	INTERFACE_PART(CTMPowerCtrl, IID_IObjectSafety, ObjSafety)
END_INTERFACE_MAP()

//==============================================================================
//
// 	Function Name:	EnumDesktopWindows()
//
// 	Description:	This callback is used to enumerate the desktop windows
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CALLBACK EnumDesktopWindows(HWND hWnd, LPARAM lpParam)
{
	CTMPowerCtrl* pControl = (CTMPowerCtrl*)lpParam;

	if(pControl)
		return pControl->OnEnumWindow(hWnd);
	else
		return FALSE;
};

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::CTMPowerCtrlFactory::UpdateRegistry
//
// 	Description:	Adds or removes system registry entries for this control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPowerCtrl::CTMPowerCtrlFactory::UpdateRegistry(BOOL bRegister)
{
	BOOL	bReturn;
	HRESULT	hResult;

	if(bRegister)
	{
		bReturn = AfxOleRegisterControlClass(AfxGetInstanceHandle(),
											 m_clsid,
											 m_lpszProgID,
											 IDS_TMPOWER,
											 IDB_TMPOWER,
											 afxRegApartmentThreading,
											 _dwTMPowerOleMisc,
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
// 	Function Name:	CTMPowerCtrl::XObjSafety::AddRef()
//
// 	Description:	This function is called to attach to the nested
//					IObjectSafety dispatch interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMPowerCtrl::XObjSafety::AddRef()
{
    METHOD_PROLOGUE(CTMPowerCtrl, ObjSafety)
    return pThis->ExternalAddRef();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::XObjSafety::GetInterfaceSafetyOptions()
//
// 	Description:	This function is called to get this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMPowerCtrl::XObjSafety::GetInterfaceSafetyOptions( 
		/* [in]  */ REFIID riid,
        /* [out] */ DWORD __RPC_FAR *pdwSupportedOptions,
        /* [out] */ DWORD __RPC_FAR *pdwEnabledOptions)
{
	HRESULT			hReturn;
	IUnknown FAR*	pInterface;

	METHOD_PROLOGUE(CTMPowerCtrl, ObjSafety)

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
// 	Function Name:	CTMPowerCtrl::XObjSafety::QueryInterface()
//
// 	Description:	This function is called to query the IObjectSafety interface
//					for the requested method/property.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT FAR EXPORT CTMPowerCtrl::XObjSafety::QueryInterface(REFIID iid, 
														  void FAR* FAR* ppvObj)
{
    METHOD_PROLOGUE(CTMPowerCtrl, ObjSafety)
    return (HRESULT)pThis->ExternalQueryInterface(&iid, ppvObj);
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::XObjSafety::Release()
//
// 	Description:	This function is called to detach from the IObjectSafety
//					dispatch interface.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMPowerCtrl::XObjSafety::Release()
{
    METHOD_PROLOGUE(CTMPowerCtrl, ObjSafety)
    return pThis->ExternalRelease();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::XObjSafety::SetInterfaceSafetyOptions()
//
// 	Description:	This function is called to set this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMPowerCtrl::XObjSafety::SetInterfaceSafetyOptions( 
        /* [in] */ REFIID riid,
        /* [in] */ DWORD dwOptionSetMask,
        /* [in] */ DWORD dwEnabledOptions)
{
	IUnknown FAR* pInterface;

    METHOD_PROLOGUE(CTMPowerCtrl, ObjSafety)
	
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
// 	Function Name:	CTMPowerCtrl::CalcRects()
//
// 	Description:	This function will calculate the rectangles required to
//					draw the split screen views
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::CalcRects()
{
	//	Get the rectangle for the full client rectangle
	GetClientRect(&m_rcMax);

	//	Calculate the full extents
	m_iWidth  = m_rcMax.right - m_rcMax.left;
	m_iHeight = m_rcMax.bottom - m_rcMax.top;

	//	Calculate the left frame rectangle
	m_rcLFrame.left   = 0;
	m_rcLFrame.top    = 0;
	m_rcLFrame.right  = m_iWidth / 2;
	m_rcLFrame.bottom = m_iHeight;
	
	//	Calculate the right frame rectangle
	m_rcRFrame.left   = m_rcLFrame.right;
	m_rcRFrame.top    = 0;
	m_rcRFrame.right  = m_iWidth;
	m_rcRFrame.bottom = m_iHeight;

	//	Calculate the left view rectangle.
	//
	//	Note: The bottom/right members are used for width/height
	m_rcLView.left   = m_rcLFrame.left + m_sSplitFrameThickness;
	m_rcLView.top    = m_rcLFrame.top + m_sSplitFrameThickness;
	m_rcLView.right  = (m_rcLFrame.right - m_rcLFrame.left) - (2 * m_sSplitFrameThickness);
	m_rcLView.bottom = (m_rcLFrame.bottom - m_rcLFrame.top) - (2 * m_sSplitFrameThickness);

	//	Calculate the right view rectangle
	//
	//	Note: The bottom/right members are used for width/height
	m_rcRView.left   = m_rcRFrame.left + m_sSplitFrameThickness;
	m_rcRView.top    = m_rcRFrame.top + m_sSplitFrameThickness;
	m_rcRView.right  = (m_rcRFrame.right - m_rcRFrame.left) - (2 * m_sSplitFrameThickness);
	m_rcRView.bottom = (m_rcRFrame.bottom - m_rcRFrame.top) - (2 * m_sSplitFrameThickness);
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::CheckVersion()
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
BOOL CTMPowerCtrl::CheckVersion(DWORD dwVersion)
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
	              "TMPower ActiveX control. You should upgrade tm_power6.ocx "
				  "as soon as possible\n\n%s", 
				  (wMinor > _wVerMinor) ? "a newer" : "an older", strVersion);

	MessageBeep(MB_ICONEXCLAMATION);
	MessageBox(strMsg, "TrialMax Error", MB_ICONEXCLAMATION | MB_OK);

	//	The versions do not match
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::Close()
//
// 	Description:	This external method allows the caller to close the control
//					and all active PowerPoint interfaces
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::Close() 
{
	//	Unload the views and detach the PowerPoint interfaces
	PPDetach();

	return TMPOWER_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::CopySlide()
//
// 	Description:	This external method allows the caller to save the current
//					slide in the specified view to the clipboard
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::CopySlide(short sView) 
{
	return GetView(sView)->CopySlide();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::CTMPowerCtrl()
//
// 	Description:	This is the constructor for CTMPowerCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMPowerCtrl::CTMPowerCtrl()
{
	InitializeIIDs(&IID_DTMPower6, &IID_DTMPower6Events);

	//	Initialize the class data
	m_pActive       = 0;
	m_pLeft         = 0;
	m_pRight        = 0;
	m_bRedraw       = TRUE;
	m_bInitialized  = FALSE;
	m_iWidth        = 0;
	m_iHeight       = 0;
	m_hFocusWnd     = 0;
	m_dwFocusThread = 0;
	ZeroMemory(&m_rcLView, sizeof(m_rcLView));
	ZeroMemory(&m_rcRView, sizeof(m_rcRView));
	ZeroMemory(&m_rcMax, sizeof(m_rcMax));
	ZeroMemory(&m_rcLFrame, sizeof(m_rcLFrame));
	ZeroMemory(&m_rcRFrame, sizeof(m_rcRFrame));

	//	Get the registry information
	GetRegistration();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::~CTMPowerCtrl()
//
// 	Description:	This is the destructor for CTMPowerCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMPowerCtrl::~CTMPowerCtrl()
{
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::DoPropertyExchange()
//
// 	Description:	This function manages the exchange of persistant 
//					properties
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::DoPropExchange(CPropExchange* pPX)
{
	BOOL bAutoInit = FALSE;
	BOOL bEnableErrors = FALSE;
	BOOL bLeftFile = FALSE;
	BOOL bRightFile = FALSE;
	BOOL bSplitScreen = FALSE;
	BOOL bSyncViews = FALSE;
	BOOL bSplitFrameThickness = FALSE;
	BOOL bSplitFrameColor = FALSE;
	BOOL bActiveView = FALSE;
	BOOL bStartSlide = FALSE;
	BOOL bEnableAccelerators = FALSE;
	BOOL bUseSlideId = FALSE;
	BOOL bSaveFormat = FALSE;
	BOOL bHideTaskBar = FALSE;
	BOOL bEnableAxErrors = FALSE;

	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
	COleControl::DoPropExchange(pPX);

	//CheckVersion(pPX->GetVersion());

	//	Load the control's persistent properties
	try
	{
		bAutoInit = PX_Bool(pPX, _T("AutoInit"), m_bAutoInit, TRUE);
		bEnableErrors = PX_Bool(pPX, _T("EnableErrors"), m_bEnableErrors, TRUE);
		bLeftFile = PX_String(pPX, _T("LeftFile"), m_strLeftFile, "");
		bRightFile = PX_String(pPX, _T("RightFile"), m_strRightFile, "");
		bSplitScreen = PX_Bool(pPX, _T("SplitScreen"), m_bSplitScreen, TMPOWER_SPLITSCREEN);
		bSyncViews = PX_Bool(pPX, _T("SyncViews"), m_bSyncViews, TMPOWER_SYNCVIEWS);
		bSplitFrameThickness = PX_Short(pPX, _T("SplitFrameThickness"), m_sSplitFrameThickness, TMPOWER_SPLITFRAMETHICKNESS);
		bSplitFrameColor = PX_Color(pPX, _T("SplitFrameColor"), m_lSplitFrameColor, ((OLE_COLOR)(TMPOWER_SPLITFRAMECOLOR)));
		bActiveView = PX_Short(pPX, _T("ActiveView"), m_sActiveView, TMPOWER_ACTIVEVIEW);
		bStartSlide = PX_Long(pPX, _T("StartSlide"), m_lStartSlide, TMPOWER_STARTSLIDE);
		bEnableAccelerators = PX_Bool(pPX, _T("EnableAccelerators"), m_bEnableAccelerators, TMPOWER_ENABLEACCELERATORS);
		bUseSlideId = PX_Bool(pPX, _T("UseSlideId"), m_bUseSlideId, TMPOWER_USESLIDEID);
		bSaveFormat = PX_Short(pPX, _T("SaveFormat"), m_sSaveFormat, TMPOWER_SAVEFORMAT);
		bHideTaskBar = PX_Bool(pPX, _T("HideTaskBar"), m_bHideTaskBar, TRUE);
		bEnableAxErrors = PX_Bool(pPX, _T("EnableAxErrors"), m_bEnableAxErrors, TMPOWER_ENABLEAXERRORS);
	}
	catch(...)
	{
		if(!bAutoInit) m_bAutoInit = TRUE;
		if(!bEnableErrors) m_bEnableErrors = TRUE;
		if(!bLeftFile) m_strLeftFile = "";
		if(!bRightFile) m_strRightFile = "";
		if(!bSplitScreen) m_bSplitScreen = TMPOWER_SPLITSCREEN;
		if(!bSyncViews) m_bSyncViews = TMPOWER_SYNCVIEWS;
		if(!bSplitFrameThickness) m_sSplitFrameThickness = TMPOWER_SPLITFRAMETHICKNESS;
		if(!bSplitFrameColor) m_lSplitFrameColor = ((OLE_COLOR)(TMPOWER_SPLITFRAMECOLOR));
		if(!bActiveView) m_sActiveView = TMPOWER_ACTIVEVIEW;
		if(!bStartSlide) m_lStartSlide = TMPOWER_STARTSLIDE;
		if(!bEnableAccelerators) m_bEnableAccelerators = TMPOWER_ENABLEACCELERATORS;
		if(!bUseSlideId) m_bUseSlideId = TMPOWER_USESLIDEID;
		if(!bSaveFormat) m_sSaveFormat = TMPOWER_SAVEFORMAT;
		if(!bHideTaskBar) m_bHideTaskBar = TRUE;
		if(!bEnableAxErrors) m_bEnableAxErrors = TMPOWER_ENABLEAXERRORS;
	} 

	//	Set default values for new properties
	if(pPX->IsLoading())
	{
		//switch(LOWORD(pPX->GetVersion()))
		//{
		//}
	}
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::DrawSplitFrame()
//
// 	Description:	This function draws the highlight frame for the active view
//					in split screen mode
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::DrawSplitFrame(RECT& rRect)
{
	CPen	FramePen;
	CDC*	pDc;
	CPen*	pOldPen;
	POINT	Points[5];

	//	Are we in user mode?
	if(!AmbientUserMode())
		return;

	//	Get the dc for this window
	if((pDc = GetDC()) == 0)
		return;

	//	Create the pens used to draw the frames
	FramePen.CreatePen(PS_INSIDEFRAME, m_sSplitFrameThickness, 
					   TranslateColor(m_lSplitFrameColor));

	//	Set up the array of points
	Points[0].x = rRect.left;
	Points[0].y = rRect.top;
	Points[1].x = rRect.right;
	Points[1].y = rRect.top;
	Points[2].x = rRect.right;
	Points[2].y = rRect.bottom;
	Points[3].x = rRect.left;
	Points[3].y = rRect.bottom;
	Points[4].x = rRect.left;
	Points[4].y = rRect.top;

	//	Select the pen into the dc
	pOldPen = pDc->SelectObject(&FramePen);
		
	//	Draw the highlight frame
	pDc->Polyline(Points, 5);

	//	Select the original pen
	pDc->SelectObject(pOldPen);

	ReleaseDC(pDc);
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::EraseSplitFrame()
//
// 	Description:	This function draws the erase the split frame using the
//					rectangle provided by the caller
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::EraseSplitFrame(RECT& rRect)
{
	CPen	FramePen;
	CDC*	pDc;
	CPen*	pOldPen;
	POINT	Points[5];

	//	Are we in user mode?
	if(!AmbientUserMode())
		return;

	//	Get the dc for this window
	if((pDc = GetDC()) == 0)
		return;

	//	Create the pens used to draw the frames
	FramePen.CreatePen(PS_INSIDEFRAME, m_sSplitFrameThickness, 
					   TranslateColor(GetBackColor()));

	//	Set up the array of points
	Points[0].x = rRect.left;
	Points[0].y = rRect.top;
	Points[1].x = rRect.right;
	Points[1].y = rRect.top;
	Points[2].x = rRect.right;
	Points[2].y = rRect.bottom;
	Points[3].x = rRect.left;
	Points[3].y = rRect.bottom;
	Points[4].x = rRect.left;
	Points[4].y = rRect.top;

	//	Select the pen into the dc
	pOldPen = pDc->SelectObject(&FramePen);
		
	//	Draw the highlight frame
	pDc->Polyline(Points, 5);

	//	Select the original pen
	pDc->SelectObject(pOldPen);

	ReleaseDC(pDc);
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::FindFile()
//
// 	Description:	This function checks to see if the file exists.
//
// 	Returns:		TRUE if the file exists.
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPowerCtrl::FindFile(LPCSTR lpFile)
{
	WIN32_FIND_DATA	Find;
	HANDLE			hFind;

	ASSERT(lpFile);

	if((hFind = FindFirstFile(lpFile, &Find)) == INVALID_HANDLE_VALUE)
	{
		return FALSE;
	}
	else
	{
		FindClose(hFind);
		return TRUE;
	}	
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::First()
//
// 	Description:	This external method allows the caller to advance to the 
//					first slide in the file.
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::First(short sView) 
{
	return GetView(sView)->First();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetAnimationCount()
//
// 	Description:	This external method allows the caller to retrieve the total
//					number of Animations in the specified view
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::GetAnimationCount(short sView) 
{
	return GetView(sView)->GetAnimationCount();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetAnimationIndex()
//
// 	Description:	This external method allows the caller to retrieve the total
//					number of Animations in the specified view
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::GetAnimationIndex(short sView) 
{
	return GetView(sView)->GetAnimationIndex();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetBitmap()
//
// 	Description:	This external method allows the caller to get a handle to
//					a device dependent bitmap representing the current slide
//					in the specified view
//
// 	Returns:		A handle to the device dependent bitmap
//
//	Notes:			None
//
//==============================================================================
long CTMPowerCtrl::GetBitmap(long pWidth, long pHeight, short sView) 
{
	return (long)GetView(sView)->GetDDBitmap((int*)pWidth, (int*)pHeight);
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetClassIdString()
//
// 	Description:	This method is called to get the class GUID as a null
//					terminated string.
//
// 	Returns:		The string equivalent of the CLASS GUID
//
//	Notes:			None
//
//==============================================================================
BSTR CTMPowerCtrl::GetClassIdString() 
{
	CString strClsId = m_tmVersion.GetClsId();
	return strClsId.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetCurrentSlide()
//
// 	Description:	This external method allows the caller to retrieve the index
//					of the current slide in the specified view
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
long CTMPowerCtrl::GetCurrentSlide(short sView) 
{
	return GetView(sView)->GetCurrentSlide();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetCurrentState()
//
// 	Description:	This external method allows the caller to retrieve the 
//					current state of the viewer
//
// 	Returns:		The current state of the specified view
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::GetCurrentState(short sView) 
{
	return GetView(sView)->GetState();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetData()
//
// 	Description:	This external method allows the caller to get the user 
//					defined value associated with the specified view.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
long CTMPowerCtrl::GetData(short sView) 
{
	return GetView(sView)->GetUserData();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetFilename()
//
// 	Description:	This external method allows the caller to get the filename
//					loaded in the specified view
//
// 	Returns:		The requested filename
//
//	Notes:			None
//
//==============================================================================
BSTR CTMPowerCtrl::GetFilename(short sView) 
{
	CString strResult = GetView(sView)->GetFilename();
	return strResult.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetPosition()
//
// 	Description:	This function is called to get the position of the view
//					identified by the caller.
//
// 	Returns:		The current view position
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::GetPosition(long lId) 
{
	CPowerPoint* pView;

	//	Which view is this
	pView = (lId == TMPOWER_AVIEW_ID) ? &m_AView : &m_BView;

	//	Is this the left hand view?
	if(pView == GetView(TMPOWER_LEFTVIEW))
		return TMPOWER_LEFTVIEW;
	else
		return TMPOWER_RIGHTVIEW;
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetPPBuild()
//
// 	Description:	This function allows the caller to retrieve the PowerPoint
//					build description reported by the dispatch interface.
//
// 	Returns:		The build descriptor
//
//	Notes:			None
//
//==============================================================================
BSTR CTMPowerCtrl::GetPPBuild() 
{
	return m_strPPBuild.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetPPVersion()
//
// 	Description:	This function allows the caller to retrieve the PowerPoint
//					version description reported by the dispatch interface.
//
// 	Returns:		The version descriptor
//
//	Notes:			None
//
//==============================================================================
BSTR CTMPowerCtrl::GetPPVersion() 
{
	return m_strPPVersion.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetRegisteredPath()
//
// 	Description:	This method is called to get the ocx path stored in the
//					system registry.
//
// 	Returns:		The path to the ocx stored in the registry
//
//	Notes:			None
//
//==============================================================================
BSTR CTMPowerCtrl::GetRegisteredPath() 
{
	CString strRegistered = m_tmVersion.GetFileSpec();
	return strRegistered.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetRegistration()
//
// 	Description:	This function is called to get the control's registration
//					information.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::GetRegistration() 
{
	CLSID	clsid;

	//	Get the GUID 
	GetClassID(&clsid);

	//	Initialize the version information
	m_tmVersion.InitFromClsId("TMPower", "PowerPoint Viewer Control", clsid);

}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetSlideCount()
//
// 	Description:	This external method allows the caller to retrieve the total
//					number of slides in the specified view
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
long CTMPowerCtrl::GetSlideCount(short sView) 
{
	return GetView(sView)->GetSlideCount();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetSlideNumber()
//
// 	Description:	This external method allows the caller to retrieve the 
//					number of the slide with the specified id
//
// 	Returns:		The slide number if successful. 0 otherwise.
//
//	Notes:			None
//
//==============================================================================
long CTMPowerCtrl::GetSlideNumber(short sView, long lSlideId) 
{
	return GetView(sView)->GetSlideNumber(lSlideId, 0);
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetVerBuild()
//
// 	Description:	This method is called to get the value of the VerBuild
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::GetVerBuild() 
{
	return m_tmVersion.GetBuild();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetVerBuildDate()
//
// 	Description:	This method is called to get the value of the VerBuildDate
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMPowerCtrl::GetVerBuildDate() 
{
	CString strBuildDate = m_tmVersion.GetBuildDate();
	return strBuildDate.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetVerMajor()
//
// 	Description:	This method is called to get the value of the VerMajor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::GetVerMajor() 
{
	return m_tmVersion.GetMajor();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetVerMinor()
//
// 	Description:	This method is called to get the value of the VerMinor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::GetVerMinor() 
{
	return m_tmVersion.GetMinor();
}


//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetVerQEF()
//
// 	Description:	This method is called to get the value of the VerQEF
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::GetVerQEF() 
{
	return m_tmVersion.GetUpdate();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetVerTextLong()
//
// 	Description:	This method is called to get the value of the VerTextLong
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMPowerCtrl::GetVerTextLong() 
{
	CString strVer = m_tmVersion.GetTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetVerTextShort()
//
// 	Description:	This method is called to get the value of the VerTextShort
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMPowerCtrl::GetVerTextShort() 
{
	CString strVer = m_tmVersion.GetShortTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::GetView()
//
// 	Description:	This function is called to get the active view
//
// 	Returns:		A pointer to the view specified by the caller 
//
//	Notes:			None
//
//==============================================================================
CPowerPoint* CTMPowerCtrl::GetView(short sView) 
{
	//	Are we looking for the left view?
	if(sView == TMPOWER_LEFTVIEW)
	{
		return (m_pLeft) ? m_pLeft : &m_AView;
	}
	else if(sView == TMPOWER_RIGHTVIEW)
	{
		return (m_pRight) ? m_pRight : &m_BView;
	}
	else
	{
		//	Return the active view
		return (m_pActive) ? m_pActive : &m_AView;
	}
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::Initialize()
//
// 	Description:	This function will initialize the control
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::Initialize()
{
	short sError;
	float fVersion;

	//	Don't bother if not in user mode or if we are already initialized
	if(!AmbientUserMode() || m_bInitialized)
		return TMPOWER_NOERROR;

	//	Attach to the PowerPoint interfaces
	if((sError = PPAttach()) != TMPOWER_NOERROR)
		return sError;

	//	Set the application interface and handle in the views
	fVersion = (float)atof(m_strPPVersion);
	m_AView.SetPPApp(&m_PPApp);
	m_BView.SetPPApp(&m_PPApp);
	m_AView.SetPPWnd(_hPPWnd);
	m_BView.SetPPWnd(_hPPWnd);
	m_AView.SetVersion(fVersion);
	m_BView.SetVersion(fVersion);

	//	Set the error handler for each view
	m_AView.SetErrorHandler(&m_Errors);
	m_BView.SetErrorHandler(&m_Errors);

	//	Set the properties for each view
	m_AView.SetBackColor(TranslateColor(GetBackColor()));
	m_AView.SetEnableAccelerators(m_bEnableAccelerators);
	m_BView.SetBackColor(TranslateColor(GetBackColor()));
	m_BView.SetEnableAccelerators(m_bEnableAccelerators);

	//	Create the view windows
	m_AView.Create(this, TMPOWER_AVIEW_ID);
	m_BView.Create(this, TMPOWER_BVIEW_ID);

	//	Make sure the views were created
	if(!IsWindow(m_AView.m_hWnd) || !IsWindow(m_BView.m_hWnd))
	{
		m_Errors.Handle(0, IDS_TMPOWER_CREATEFAILED);
		return TMPOWER_CREATEFAILED;
	}

	//	Make the left view visible and then check to see if we should be
	//	in split screen mode
	m_AView.ShowWindow(SW_SHOW);
	m_BView.ShowWindow(SW_HIDE);
	OnSplitScreenChanged();

	//	Load the files if any are specified
	if(!m_strLeftFile.IsEmpty())
		m_AView.SetFilename(m_strLeftFile, 1, FALSE);
	if(!m_strRightFile.IsEmpty())
		m_BView.SetFilename(m_strRightFile, 1, FALSE);

	//	The control is now initialized
	m_bInitialized = TRUE;

	return TMPOWER_NOERROR;
}	

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::IsInitialized()
//
// 	Description:	This method allows the caller to determine if the control
//					has been initialized
//
// 	Returns:		TRUE if initialized
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPowerCtrl::IsInitialized() 
{
	return m_bInitialized;
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::Last()
//
// 	Description:	This external method allows the caller to advance to the 
//					last slide in the file.
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::Last(short sView) 
{
	return GetView(sView)->Last();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::LoadFile()
//
// 	Description:	This method is called to load a new file in the specified
//					view
//
// 	Returns:		TMPOWER_NOERROR if successful.
//
//	Notes:			The caller can perform the same action by setting the
//					filename property but no error level is returned unless
//					this method is used.
//
//==============================================================================
short CTMPowerCtrl::LoadFile(LPCTSTR lpszFilename, long lSlide, short bSlideId, 
							 short sView) 
{
	//	Are we in user mode?
	if(AmbientUserMode())
	{
		//	Is the user requesting the active pane?
		if(sView != TMPOWER_LEFTVIEW && sView != TMPOWER_RIGHTVIEW)
			sView = m_sActiveView;

		//	Set the appropriate filename property
		if(sView == TMPOWER_LEFTVIEW)
			m_strLeftFile = lpszFilename;
		else
			m_strRightFile = lpszFilename;

		//	Is the desired file already loaded?
		if(lstrlen(lpszFilename) > 0 && lstrcmpi(lpszFilename, GetView(sView)->GetFilename()) == 0)
		{
			//	Do we need to set to a specific slide?
			if(lSlide <= 0)
				lSlide = 1;

			return GetView(sView)->SetSlide(lSlide, bSlideId);
		}
		else
		{
			return GetView(sView)->SetFilename(lpszFilename, lSlide, bSlideId);
		}
	}
	
	return TMPOWER_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::Next()
//
// 	Description:	This external method allows the caller to advance to the 
//					next slide in the file.
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::Next(short sView) 
{
	return GetView(sView)->Next();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnActiveViewChanged()
//
// 	Description:	This function is called when the ActiveView property changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnActiveViewChanged() 
{
	if(AmbientUserMode())
	{
		//	Which view is active
		m_pActive = GetView(m_sActiveView);

		//	Are we in split screen mode?
		if(m_bSplitScreen)
		{
			//	Erase the inactive highlight
			EraseSplitFrame((m_sActiveView == TMPOWER_LEFTVIEW) ? m_rcRFrame : 
															      m_rcLFrame);

			//	Draw the active highlight
			DrawSplitFrame((m_sActiveView == TMPOWER_LEFTVIEW) ? m_rcLFrame : 
															     m_rcRFrame);
		}

		//	Notify the container
		FireSelectView(m_sActiveView);
	}
	
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnAutoInitChanged()
//
// 	Description:	This function is called when the AutoInit property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnAutoInitChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnCreate()
//
// 	Description:	This function is called by the framework when the control
//					window is created
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
int CTMPowerCtrl::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
	if(COleControl::OnCreate(lpCreateStruct) == -1)
		return -1;
	
	//	Initialize the error handler
	m_Errors.Enable(m_bEnableErrors);
	m_Errors.SetParent(m_hWnd);
	m_Errors.SetTitle("TMPOWER Error");
	m_Errors.SetMessageId(m_bEnableAxErrors == TRUE ? WM_ERROR_EVENT : 0);
	
	m_AView.SetHideTaskBar(m_bHideTaskBar);
	m_BView.SetHideTaskBar(m_bHideTaskBar);

	//	Should we automatically initialize the control?
	if(m_bAutoInit && AmbientUserMode())
		Initialize();

	return 0;
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnDestroy()
//
// 	Description:	This function is called when the window is about to be 
//					destroyed
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnDestroy() 
{
	//	Make sure everything is closed
	Close();

	//	Do the base class cleanup
	COleControl::OnDestroy();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnDraw()
//
// 	Description:	This function is called in response to WM_PAINT messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnDraw(CDC* pdc, const CRect& rcBounds,const CRect& rcInvalid)
{
	CBrush	brBackground;
	CRect	Rect;

	//	Don't bother if we are inhibiting redraw operations
	if(!m_bRedraw)
		return;

	//	Are we in user mode?
	if(AmbientUserMode())
	{
		brBackground.CreateSolidBrush(TranslateColor(GetBackColor()));
		pdc->FillRect(rcBounds, &brBackground);
		
		//	Draw the highlight if we are in split screen mode
		if(m_bSplitScreen)
			DrawSplitFrame(m_sActiveView == TMPOWER_LEFTVIEW ? m_rcLFrame : 
															   m_rcRFrame);
		//	Redraw the left view
		if(m_pLeft != 0)
			m_pLeft->Redraw();

		//	Redraw the right pane
		if(m_bSplitScreen && (m_pRight != 0))
			m_pRight->Redraw();
	}
	else
	{
		CString	strText;
		CRect ControlRect = rcBounds;

		strText.Format("FTI PowerPoint Control (rev. %d.%d)", _wVerMajor, _wVerMinor);

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
// 	Function Name:	CTMPowerCtrl::OnEnableAxErrorsChanged()
//
// 	Description:	This function is called when the EnableAxErrors property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnEnableAxErrorsChanged() 
{
	SetModifiedFlag();

	if(AmbientUserMode())
		m_Errors.SetMessageId(m_bEnableAxErrors == TRUE ? WM_ERROR_EVENT : 0);
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnEnableErrorsChanged()
//
// 	Description:	This function is called when the EnableErrors property 
//					changes.
//
// 	Returns:		void
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnEnableErrorsChanged() 
{
	// Set the state of the error handler
	m_Errors.Enable(m_bEnableErrors);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnEnableAcceleratorsChanged()
//
// 	Description:	This function is called when the EnableAccelerators property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnEnableAcceleratorsChanged() 
{
	if(AmbientUserMode())
	{
		m_AView.SetEnableAccelerators(m_bEnableAccelerators);
		m_BView.SetEnableAccelerators(m_bEnableAccelerators);
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnEnumWindow()
//
// 	Description:	This function is called by the window enumeration callback
//					when we are looking for the PowerPoint main window
//
// 	Returns:		TRUE if enumeration should continue
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPowerCtrl::OnEnumWindow(HWND hWnd) 
{
	CWnd*	pChild = CWnd::FromHandle(hWnd);
	char	szTitle[512];

	//	Get the title of this window
	if(pChild == 0)
		return TRUE;
	else
		pChild->GetWindowText(szTitle, sizeof(szTitle));

	//	Is this the main PowerPoint window?
	if(strncmp(POWERPOINT_CAPTION, szTitle, lstrlen(POWERPOINT_CAPTION)) == 0)
	{
		_hPPWnd = hWnd;
		return FALSE;
	}
	else
	{
		//	Keep enumerating - window not found
		return TRUE;
	}
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnHideTaskBarChanged()
//
// 	Description:	This function is called when the HideTaskBar property 
//					changes.
//
// 	Returns:		void
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnHideTaskBarChanged() 
{
	if(AmbientUserMode())
	{
		m_AView.SetHideTaskBar(m_bHideTaskBar);
		m_BView.SetHideTaskBar(m_bHideTaskBar);
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnLeftFileChanged()
//
// 	Description:	This function is called when the LeftFile property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnLeftFileChanged() 
{
	SetModifiedFlag();
	if(AmbientUserMode())
		LoadFile(m_strLeftFile, m_lStartSlide, m_bUseSlideId, TMPOWER_LEFTVIEW);
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnPPFileChange()
//
// 	Description:	This function is called by one of the views when it loads
//					a new file
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnPPFileChange(long lId, LPCSTR lpFilename) 
{
	//	Set the appropriate filename property
	if(GetPosition(lId) == TMPOWER_LEFTVIEW)
		m_strLeftFile = lpFilename;
	else
		m_strRightFile = lpFilename;

	//	Notify the containter
	FireFileChanged(lpFilename, GetPosition(lId));
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnPPFocus()
//
// 	Description:	This function is called by one of the views when it gains
//					the keyboard focus.
//
// 	Returns:		None
//
//	Notes:			When PowerPoint activates a slide show it sets the keyboard
//					focus to the slide show window. We can't afford that because
//					it will take the focus away from the container application.
//
//==============================================================================
void CTMPowerCtrl::OnPPFocus(long lId) 
{
	//	Set the appropriate view identifier
	if(GetPosition(lId) == TMPOWER_LEFTVIEW)
		FireViewFocus(TMPOWER_LEFTVIEW);
	else
		FireViewFocus(TMPOWER_RIGHTVIEW);
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnPPLoad()
//
// 	Description:	This function is called by one of the views when it loads
//					a new presentation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnPPLoad(long lId) 
{
	//PPMove();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnPPSlideChange()
//
// 	Description:	This function is called by one of the views when it changes
//					the current slide
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnPPSlideChange(long lId, long lSlide) 
{
	//	Notify the containter
	FireSlideChanged(lSlide, GetPosition(lId));
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnPPStateChange()
//
// 	Description:	This function is called by one of the views when it changes
//					the current state
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnPPStateChange(long lId, short sState) 
{
	//	Notify the containter
	FireStateChanged(sState, GetPosition(lId));
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnResetState()
//
// 	Description:	This method is called to reset the control to its default 
//					state.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnResetState()
{
	COleControl::OnResetState();  
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnRightFileChanged()
//
// 	Description:	This function is called when the RightFile property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnRightFileChanged() 
{
	SetModifiedFlag();
	if(AmbientUserMode())
		LoadFile(m_strRightFile, m_lStartSlide, m_bUseSlideId, TMPOWER_RIGHTVIEW);
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnSaveFormatChanged()
//
// 	Description:	This function is called when the SaveFormat property
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnSaveFormatChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnSize()
//
// 	Description:	This function handles all WM_SIZE messages sent to the 
//					control
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnSize(UINT nType, int cx, int cy) 
{
	//	Recalculate the rectangles
	CalcRects();

	//	Set the new extents for the viewers
	if(IsWindow(m_AView.m_hWnd) && IsWindow(m_BView.m_hWnd))
	{
		//	Block attempts to redraw while we resize everything
		m_bRedraw = FALSE;

		//	Size the left view
		if(m_pLeft)
			m_pLeft->SetMaxRect((m_bSplitScreen) ? m_rcLView : m_rcMax);

		//	Size the right view if it's visible
		if(m_pRight && m_bSplitScreen)
			m_pRight->SetMaxRect(m_rcRView);
		
		//	It's ok to draw now
		m_bRedraw = TRUE;
		RedrawWindow();
	}
	else
		COleControl::OnSize(nType, cx, cy);
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnSplitViewColorChanged()
//
// 	Description:	This function is called when the SplitViewColor property
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnSplitFrameColorChanged() 
{
	if(AmbientUserMode() && m_bSplitScreen)
		RedrawWindow();

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnSplitFrameThicknessChanged()
//
// 	Description:	This function is called when the SplitFrameThickness 
//					property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnSplitFrameThicknessChanged() 
{
	//	If we are in split screen mode update the frame
	if(AmbientUserMode())
	{
		//	Recalculate the rectangles
		CalcRects();

		//	Redraw if we are in split screen mode
		if(m_bSplitScreen)
			RedrawWindow();
	}

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnSplitScreenChanged()
//
// 	Description:	This function is called when the SplitScreen property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnSplitScreenChanged() 
{
	RECT rcInactive;

	SetModifiedFlag();

	//	Don't bother if we're not in user mode?
	if(!AmbientUserMode())
		return;

	//	Prevent attempts to redraw 
	m_bRedraw = FALSE;

	//	Make the active view the left view
	m_pLeft = GetView();
		
	//	Make the inactive view the right view
	m_pRight = (m_pLeft == &m_AView) ? &m_BView : &m_AView;

	ASSERT(m_pLeft);
	ASSERT(m_pRight);

	if(m_bSplitScreen)
	{
		//	Size the each view
		m_pLeft->SetMaxRect(m_rcLView);
		m_pRight->SetMaxRect(m_rcRView);

		//	Both views should be visible
		m_pLeft->ShowWindow(SW_SHOW);
		m_pRight->ShowWindow(SW_SHOW);
	}
	else
	{
		//	Size the active view
		m_pLeft->SetMaxRect(m_rcMax);

		//	The inactive view should not be visible
		m_pRight->ShowWindow(SW_HIDE);

		//	Make sure the inactive is shrunk to zero
		memset(&rcInactive, 0, sizeof(rcInactive));
		m_pRight->SetMaxRect(rcInactive);
	}

	//	Make sure the view is properly selected
	m_sActiveView = TMPOWER_LEFTVIEW;
	m_pActive = m_pLeft;
	FireSelectView(m_sActiveView);

	//	Redraw the windows
	m_bRedraw = TRUE;
	RedrawWindow();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnStartSlideChanged()
//
// 	Description:	This function is called when the StartSlide property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnStartSlideChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnRightFileChanged()
//
// 	Description:	This function is called when the SyncViews property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnSyncViewsChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnUseSlideIdChanged()
//
// 	Description:	This function is called when the UseSlideId property changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::OnUseSlideIdChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::OnWMErrorEvent()
//
// 	Description:	This function handles all WM_ERROR_EVENT messages
//
// 	Returns:		Zero if the message is handled
//
//	Notes:			None
//
//==============================================================================
LONG CTMPowerCtrl::OnWMErrorEvent(WPARAM wParam, LPARAM lParam)
{
	if((m_bEnableAxErrors == TRUE) && (lstrlen(m_Errors.GetMessage()) > 0))
	{
		FireAxError(m_Errors.GetMessage());
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::PPAttach()
//
// 	Description:	This function is called to attach the control to the 
//					PowerPoint OLE interfaces
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::PPAttach()
{
	COleException	OE;
	CLSID			ClassId;
	IUnknown*		pUnknown;
	HRESULT			hResult;
	char			szError[256];

	//	Have we already attached?
	if(m_PPApp.m_lpDispatch)
		return TMPOWER_NOERROR;

	//	Is this the first instance of the TMPower control?
	if(_lAttachments == 0)
	{
		//	Check to see if the PowerPoint application is already active
		CLSIDFromProgID(L"PowerPoint.Application", &ClassId);
		hResult = GetActiveObject(ClassId, NULL, (IUnknown**)&pUnknown);
		if(SUCCEEDED(hResult))
		{
			_bPPActive = TRUE;
			pUnknown->Release();
		}
	}

	//	Open the interface to the PowerPoint application
	if(!m_PPApp.CreateDispatch("PowerPoint.Application", &OE))
	{
		OE.GetErrorMessage(szError, sizeof(szError));
		m_Errors.Handle(0, IDS_TMPOWER_ATTACHFAILED, szError);
		m_strPPVersion = "PowerPoint Not Installed";
		m_strPPBuild.Empty();
		return TMPOWER_ATTACHFAILED;
	}
	else
	{
		//	Increment the instance counter
		InterlockedIncrement(&_lAttachments);

		//	Get the PowerPoint version description
		m_strPPVersion = m_PPApp.GetVersion();
		m_strPPBuild = m_PPApp.GetBuild();
	
		//	Has a previous instance of this control already retrieved the
		//	PowerPoint window?
		if(_hPPWnd && IsWindow(_hPPWnd))
			return TMPOWER_NOERROR;

		//	Store the information we're going to need to restore PowerPoint
		//	if it was already active
		if(_bPPActive)
		{
			//	Get the state of the PowerPoint window
			_lPPState  = m_PPApp.GetWindowState();
			
			//	If the window is not minimized or maximized get its actual
			//	size and position. Otherwise assign defaults
			if(_lPPState == ppWindowNormal)
			{
				//	Get the size and position of the PowerPoint window
				_fPPLeft   = m_PPApp.GetLeft();
				_fPPTop    = m_PPApp.GetTop();
				_fPPWidth  = m_PPApp.GetWidth();
				_fPPHeight = m_PPApp.GetHeight();
			}
			else
			{
				//	Use defaults because the values returned by the interface
				//	are only valid if the window is in a normal state.
				//
				//	NOTES:	We could set the window to a normal state but then
				//			it would flash unnecessarily between its max/min
				//			state and the normal state
				_fPPLeft   = 0.0;
				_fPPTop    = 0.0;
				_fPPWidth  = 267.0;
				_fPPHeight = 200.0;
			}
		}

		//	Locate the main window by setting the caption to a unique value and
		//	enumerating the desktop windows
		_strPPCaption = m_PPApp.GetCaption();
		m_PPApp.SetCaption(POWERPOINT_CAPTION);
		EnumChildWindows(GetDesktopWindow()->m_hWnd, EnumDesktopWindows, 
						(LPARAM)this);

		//	We can't go any further without the PowerPoint window
		if(_hPPWnd == 0)
		{
			m_PPApp.SetCaption(_strPPCaption);
			m_Errors.Handle(0, IDS_TMPOWER_NOMAINWINDOW);
			return TMPOWER_NOMAINWINDOW;
		}

		//	Get the identifier of the PowerPoint thread
		_dwPPThread = GetWindowThreadProcessId(_hPPWnd, 0);

		//	Do we need to redirect the keyboard focus?
		if((m_dwFocusThread != 0) && (_dwPPThread != 0))
		{
			//	Redirect the PowerPoint keyboard to the caller specified focus
			//	window
			if(!AttachThreadInput(_dwPPThread, m_dwFocusThread, TRUE))
				m_Errors.Handle(0, IDS_TMPOWER_SETFOCUSFAILED);
		}

		PPMove();

		return TMPOWER_NOERROR;
	}
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::PPDetach()
//
// 	Description:	This function is called to make sure PowerPoint is properly
//					closed when this control is destroyed
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::PPDetach()
{
	Presentations PPPresentations;

	//	Unload and reset each view
	m_AView.Unload();
	m_BView.Unload();
	m_AView.SetPPApp(0);
	m_BView.SetPPApp(0);
	m_AView.SetPPWnd(0);
	m_BView.SetPPWnd(0);

	//	The control is no longer intialized
	m_bInitialized = FALSE;

	//	Is this instance attached to PowerPoint?
	if(m_PPApp.m_lpDispatch == 0)
		return TMPOWER_NOERROR;

	//	Decrement the instance counter
	InterlockedDecrement(&_lAttachments);
	
	//	Is this the last instance of the TMPower control to detach?
	if(_lAttachments <= 0)
	{
		//	Do we need to detach the existing focus window?
		if(_dwPPThread != 0 && m_dwFocusThread != 0)
		{
			AttachThreadInput(_dwPPThread, m_dwFocusThread, FALSE);
			m_dwFocusThread = 0;
		}

		//	Should we restore PowerPoint?
		if(_bPPActive)
		{
			//	Restore the caption
			m_PPApp.SetCaption(_strPPCaption);

			//	Make sure the window is normal and visible so that we
			//	can resize and position it
			m_PPApp.SetVisible(PP_TRUE);
			m_PPApp.SetWindowState(ppWindowNormal);
			m_PPApp.SetLeft(_fPPLeft);
			m_PPApp.SetTop(_fPPTop);
			m_PPApp.SetWidth(_fPPWidth);
			m_PPApp.SetHeight(_fPPHeight);

			//	Restore the intial state
			m_PPApp.SetWindowState(_lPPState);
		}
		else
		{
			//	Shut down PowerPoint
			m_PPApp.Quit();
		}

		//	Reset the global data
		_hPPWnd = 0;
		_fPPLeft = 0.0f;
		_fPPTop = 0.0f;
		_fPPWidth = 0.0f;
		_fPPHeight = 0.0f;
		_lPPState = 0;
		_lAttachments = 0;
		_bPPActive = FALSE;
		_dwPPThread = 0;
	}

	//	Release the dispatch interface
	m_PPApp.ReleaseDispatch();

	return TMPOWER_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::PPMove()
//
// 	Description:	This function is called to make sure PowerPoint application
//					window is properly sized and positioned
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::PPMove()
{
	//	Did we have the main window?
	if(_hPPWnd == 0)
		return TMPOWER_NOMAINWINDOW;

	//	Was PowerPoint already active?
	if(_bPPActive)
	{
		//	If the window is already active it must be visible. If it's not
		//	already in a normal state we have to set it to normal so that we can
		//	move it off screen
		if(_lPPState != ppWindowNormal)
			m_PPApp.SetWindowState(ppWindowNormal);
	}
	else
	{
		//	If the window was not already active we have to make it visible
		//	and normal in order to resize and position it
		//
		//	NOTE:	Checking the current window state ahead of time does us no
		//			good because PowerPoint will always report that the window
		//			is normal until it is made visible
		m_PPApp.SetVisible(PP_TRUE);
		m_PPApp.SetWindowState(ppWindowNormal);
	}

	//	Move the window off screen
	CWnd* pPPWnd = CWnd::FromHandle(_hPPWnd);
	pPPWnd->MoveWindow(10000,10000,10,10);

	return TMPOWER_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::Previous()
//
// 	Description:	This external method allows the caller to advance to the 
//					previous slide in the file.
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::Previous(short sView) 
{
	return GetView(sView)->Previous();
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::SaveSlide()
//
// 	Description:	This external method allows the caller to save the current
//					slide in the specified view to the requested file
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::SaveSlide(LPCTSTR lpFilename, short sView) 
{
	return GetView(sView)->SaveSlide(lpFilename, m_sSaveFormat);
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::SetData()
//
// 	Description:	This external method allows the caller to set a user defined
//					value to be associated with the specified view.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPowerCtrl::SetData(short sView, long lData) 
{
	GetView(sView)->SetUserData(lData);
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::SetFocusWnd()
//
// 	Description:	This external method allows the caller to set the handle of
//					the window to which the keyboard focus should be assigned
//					when the slide show gains the focus.
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::SetFocusWnd(OLE_HANDLE hWnd) 
{
	//	Save the handle of the new focus window
	m_hFocusWnd = (HWND)hWnd;

	//	Do we need to detach the existing focus window?
	if(_dwPPThread != 0 && m_dwFocusThread != 0)
	{
		AttachThreadInput(_dwPPThread, m_dwFocusThread, FALSE);
		m_dwFocusThread = 0;
	}

	//	Are we supposed to reassign the focus?
	if(m_hFocusWnd != 0 && IsWindow(m_hFocusWnd))
	{
		m_dwFocusThread = GetWindowThreadProcessId(m_hFocusWnd, 0);

		//	Redirect the focus if we've got the PowerPoint thread
		if(_dwPPThread != 0)
		{
			if(AttachThreadInput(_dwPPThread, m_dwFocusThread, TRUE))
			{
				//	This will return control to the calling app
				/*
				keybd_event(VK_MENU, 0, 0, 0);
				keybd_event(VK_ESCAPE, 0, 0, 0);
				keybd_event(VK_ESCAPE, 0, KEYEVENTF_KEYUP, 0);
				keybd_event(VK_MENU, 0, KEYEVENTF_KEYUP, 0);
				*/
			}
			else
			{
				m_Errors.Handle(0, IDS_TMPOWER_SETFOCUSFAILED);
			}
		}
	}

	return TMPOWER_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::SetSlide()
//
// 	Description:	This external method allows the caller to advance to the 
//					specified slide in the file.
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::SetSlide(short sView, long lSlide, short bUseId) 
{
	return GetView(sView)->SetSlide(lSlide, bUseId);
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::Show()
//
// 	Description:	This method is called to show or hide the slide show windows
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::Show(short sShow) 
{
	int iCommand = (sShow != 0) ? SW_SHOW : SW_HIDE;

	if(m_bSplitScreen)
	{
		if(IsWindow(m_AView.m_hWnd))
			m_AView.ShowWindow(iCommand);
		if(IsWindow(m_BView.m_hWnd))
			m_BView.ShowWindow(iCommand);
	}
	else
	{
		if(IsWindow(GetView()->m_hWnd))
			GetView()->ShowWindow(iCommand);
	}

	ShowWindow(iCommand);
	return TMPOWER_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::ShowSnapshot()
//
// 	Description:	This external method allows the caller to display a 
//					snapshot of the frame in the specified view
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::ShowSnapshot(short sView) 
{
	CSnapshot*	pSnapshot;

	//	Create the snapshot for the requested view
	if((pSnapshot = GetView(sView)->GetSnapshot(TRUE)) != 0)
	{
		//	Show the snapshot
		pSnapshot->ShowWindow(SW_SHOW);
		return TMPOWER_NOERROR;
	}
	else
	{
		m_Errors.Handle(0, IDS_TMPOWER_CREATESNAPSHOTFAIL);
		return TMPOWER_CREATESNAPSHOTFAIL;
	}
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::Unload()
//
// 	Description:	This external method allows the caller to unload the file
//					in the specified view
//
// 	Returns:		TMPOWER_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPowerCtrl::Unload(short sView) 
{
	// check for windows version
	// no need to close key board on windows8 and onwords windows
	OSVERSIONINFO osvi;
	ZeroMemory(&osvi, sizeof(OSVERSIONINFO));
    osvi.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);

    GetVersionEx(&osvi);

	// version number of windows8 is 6.2
	// http://msdn.microsoft.com/en-us/library/ms724832(v=vs.85).aspx
	if (osvi.dwMajorVersion < 6 || (osvi.dwMajorVersion = 6 && osvi.dwMinorVersion < 2))
    {
		// Getting the handler of Virtual Keyboard 
		//HWND wKB = ::FindWindow(_TEXT("IPTip_Main_Window"), NULL);
 
		// Checking for null and sending the message to close the Virtual Keyboard
		//if (wKB != NULL)
		//{
			//::PostMessage(wKB,WM_CLOSE,NULL,NULL);
		//}
		KillProcessByName("TabTip.exe");
    }
		
	
	if(IsInitialized())
		return GetView(sView)->Unload();
	else
		return TMPOWER_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMPowerCtrl::KillProcessByName()
//
// 	Description:	Helper function used to kill any process by name.
//
//	Parameters:		*szProcessToKill	Name of the process
//
// 	Returns:		True if successful
//
//	Notes:			Taken from http://www.cplusplus.com/forum/general/6497
//					This method should be moved to a common base or helper class.
//
//==============================================================================
BOOL CTMPowerCtrl::KillProcessByName(char *szProcessToKill)
{
	HANDLE hProcessSnap;
	HANDLE hProcess;
	PROCESSENTRY32 pe32;
	DWORD dwPriorityClass;

	// Takes a snapshot of all the processes
	hProcessSnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);

	if(hProcessSnap == INVALID_HANDLE_VALUE){
		return( FALSE );
	}

	pe32.dwSize = sizeof(PROCESSENTRY32);

	if(!Process32First(hProcessSnap, &pe32))
	{
		CloseHandle(hProcessSnap);     
		return( FALSE );
	}

	do
	{
		//  checks if process at current position has the name of to be killed app
		if(!strcmp(pe32.szExeFile,szProcessToKill)){
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
	return( TRUE );
}

