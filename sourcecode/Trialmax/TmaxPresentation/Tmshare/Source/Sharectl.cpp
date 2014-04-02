//==============================================================================
//
// File Name:	sharectl.cpp
//
// Description:	This file contains member functions of the CTMShareCtrl class.
//
// See Also:	sharectl.h
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	04-05-02	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <shareapp.h>
#include <sharectl.h>
#include <sharepg.h>
#include <sharedef.h>
#include <regcats.h>
#include <direct.h>		// getcwd()
#include <dispid.h>
#include <filever.h>
#include <toolbox.h>

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
extern CTMShareApp NEAR		theApp;
CTMShareCtrl*				_pTMShareCtrl = NULL;

// Interface IDs

/* Replace 2 */
const IID BASED_CODE IID_DTMShare6 =
		{ 0xb3a5f2e5, 0x42dd, 0x4d3f, { 0xa7, 0xb5, 0xf5, 0xa0, 0xb7, 0x69, 0x35, 0x65 } };
/* Replace 3 */
const IID BASED_CODE IID_DTMShare6Events =
		{ 0x3ca9ff70, 0xe3f1, 0x4cee, { 0x95, 0x4a, 0x91, 0x84, 0, 0xf0, 0xf7, 0xa6 } };

// Control type information
static const DWORD BASED_CODE _dwTMShareOleMisc =
	OLEMISC_INVISIBLEATRUNTIME |
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
BEGIN_MESSAGE_MAP(CTMShareCtrl, COleControl)
	//{{AFX_MSG_MAP(CTMShareCtrl)
	ON_WM_CREATE()
	//}}AFX_MSG_MAP
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
	ON_MESSAGE(WM_ERROR_EVENT, OnWMErrorEvent)
END_MESSAGE_MAP()

// Dispatch map
BEGIN_DISPATCH_MAP(CTMShareCtrl, COleControl)
	//{{AFX_DISPATCH_MAP(CTMShareCtrl)
	DISP_PROPERTY_EX(CTMShareCtrl, "VerBuild", GetVerBuild, SetNotSupported, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMShareCtrl, "EnableErrors", m_bEnableErrors, OnEnableErrorsChanged, VT_BOOL)
	DISP_PROPERTY_EX(CTMShareCtrl, "VerMajor", GetVerMajor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMShareCtrl, "VerMinor", GetVerMinor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMShareCtrl, "VerTextLong", GetVerTextLong, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMShareCtrl, "Owner", m_sOwner, OnOwnerChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMShareCtrl, "PeekPeriod", m_lPeekPeriod, OnPeekPeriodChanged, VT_I4)
	DISP_PROPERTY_NOTIFY(CTMShareCtrl, "Command", m_lCommand, OnCommandChanged, VT_I4)
	DISP_PROPERTY_NOTIFY(CTMShareCtrl, "Error", m_lError, OnErrorChanged, VT_I4)
	DISP_PROPERTY_NOTIFY(CTMShareCtrl, "MediaId", m_strMediaId, OnMediaIdChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMShareCtrl, "EnableAxErrors", m_bEnableAxErrors, OnEnableAxErrorsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMShareCtrl, "Barcode", m_strBarcode, OnBarcodeChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMShareCtrl, "AppFolder", m_strAppFolder, OnAppFolderChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMShareCtrl, "PrimaryId", m_lPrimaryId, OnPrimaryIdChanged, VT_I4)
	DISP_PROPERTY_NOTIFY(CTMShareCtrl, "SecondaryId", m_lSecondaryId, OnSecondaryIdChanged, VT_I4)
	DISP_PROPERTY_NOTIFY(CTMShareCtrl, "TertiaryId", m_lTertiaryId, OnTertiaryIdChanged, VT_I4)
	DISP_PROPERTY_NOTIFY(CTMShareCtrl, "QuaternaryId", m_lQuaternaryId, OnQuaternaryIdChanged, VT_I4)
	DISP_PROPERTY_NOTIFY(CTMShareCtrl, "SourceFileName", m_strSourceFileName, OnSourceFileNameChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMShareCtrl, "SourceFilePath", m_strSourceFilePath, OnSourceFilePathChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMShareCtrl, "CaseFolder", m_strCaseFolder, OnCaseFolderChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMShareCtrl, "BarcodeId", m_lBarcodeId, OnBarcodeIdChanged, VT_I4)
	DISP_PROPERTY_NOTIFY(CTMShareCtrl, "DisplayOrder", m_lDisplayOrder, OnDisplayOrderChanged, VT_I4)
	DISP_FUNCTION(CTMShareCtrl, "GetRegisteredPath", GetRegisteredPath, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMShareCtrl, "GetClassIdString", GetClassIdString, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMShareCtrl, "GetInitialized", GetInitialized, VT_BOOL, VTS_NONE)
	DISP_FUNCTION(CTMShareCtrl, "GetResponse", GetResponse, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMShareCtrl, "GetRequest", GetRequest, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMShareCtrl, "SetResponse", SetResponse, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMShareCtrl, "Terminate", Terminate, VT_EMPTY, VTS_NONE)
	DISP_FUNCTION(CTMShareCtrl, "IsRunning", IsRunning, VT_BOOL, VTS_NONE)
	DISP_FUNCTION(CTMShareCtrl, "GetSisterFileSpec", GetSisterFileSpec, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMShareCtrl, "Initialize", Initialize, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMShareCtrl, "SetRequest", SetRequest, VT_I2, VTS_I4)
	DISP_FUNCTION(CTMShareCtrl, "Execute", Execute, VT_I2, VTS_NONE)
	//}}AFX_DISPATCH_MAP
	
	//	Added rev 6.1.0
	DISP_PROPERTY_EX_ID(CTMShareCtrl, "VerTextShort", DISPID_VERTEXTSHORT, GetVerTextShort, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_EX_ID(CTMShareCtrl, "VerQEF", DISPID_VERQEF, GetVerQEF, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX_ID(CTMShareCtrl, "VerBuildDate", DISPID_VERBUILDDATE, GetVerBuildDate, SetNotSupported, VT_BSTR)

	//	Added rev 6.3.3
	DISP_PROPERTY_NOTIFY_ID(CTMShareCtrl, "PageNumber", DISPID_PAGE_NUMBER, m_lPageNumber, OnPageNumberChanged, VT_I4)
	DISP_PROPERTY_NOTIFY_ID(CTMShareCtrl, "LineNumber", DISPID_LINE_NUMBER, m_sLineNumber, OnLineNumberChanged, VT_I2)


	DISP_FUNCTION_ID(CTMShareCtrl, "AboutBox", DISPID_ABOUTBOX, AboutBox, VT_EMPTY, VTS_NONE)

END_DISPATCH_MAP()

BEGIN_EVENT_MAP(CTMShareCtrl, COleControl)
	//{{AFX_EVENT_MAP(CTMShareCtrl)
	EVENT_CUSTOM("CommandRequest", FireCommandRequest, VTS_NONE)
	EVENT_CUSTOM("AxError", FireAxError, VTS_BSTR)
	EVENT_CUSTOM("AxDiagnostic", FireAxDiagnostic, VTS_BSTR  VTS_BSTR)
	EVENT_CUSTOM("CommandResponse", FireCommandResponse, VTS_NONE)
	//}}AFX_EVENT_MAP
END_EVENT_MAP()

// Property pages
BEGIN_PROPPAGEIDS(CTMShareCtrl, 2)
	PROPPAGEID(CTMShareProperties::guid)
	PROPPAGEID(CLSID_CColorPropPage)
END_PROPPAGEIDS(CTMShareCtrl)

// Initialize class factory and guid
/* Replace 4 */
IMPLEMENT_OLECREATE_EX(CTMShareCtrl, "TMSHARE6.TMShareCtrl.1",
	0xcb5d5073, 0xab77, 0x45f6, 0xb7, 0x28, 0x18, 0x8, 0xdd, 0xc8, 0, 0x26)

// Type library ID and version
IMPLEMENT_OLETYPELIB(CTMShareCtrl, _tlid, _wVerMajor, _wVerMinor)
IMPLEMENT_OLECTLTYPE(CTMShareCtrl, IDS_TMSHARE, _dwTMShareOleMisc)

IMPLEMENT_DYNCREATE(CTMShareCtrl, COleControl)

// Interface map for IObjectSafety
BEGIN_INTERFACE_MAP(CTMShareCtrl, COleControl )
	INTERFACE_PART(CTMShareCtrl, IID_IObjectSafety, ObjSafety)
END_INTERFACE_MAP()

//------------------------------------------------------------------------------
//
// 	Function Name:	TimerProc()
//
//	Parameters:		hwnd    - not used
//					uMsg    - WM_TIMER message identifier
//					idEvent - timer event identifier
//					dwTime  - current system time
//
// 	Return Value:	None
//
// 	Description:	This is the callback for the windowless timer created when
//					the control was initialized.
//
//------------------------------------------------------------------------------
void CALLBACK TimerProc(HWND hwnd, UINT uMsg, UINT idEvent, DWORD dwTime)
{
	//	Notify the control
	if(_pTMShareCtrl != NULL)
		_pTMShareCtrl->OnTimer(idEvent, dwTime);
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::CTMShareCtrlFactory::UpdateRegistry
//
// 	Description:	Adds or removes system registry entries for this control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTMShareCtrl::CTMShareCtrlFactory::UpdateRegistry(BOOL bRegister)
{
	BOOL	bReturn;
	HRESULT	hResult;

	if(bRegister)
	{
		bReturn = AfxOleRegisterControlClass(AfxGetInstanceHandle(),
											 m_clsid,
											 m_lpszProgID,
											 IDS_TMSHARE,
											 IDB_TMSHARE,
											 afxRegApartmentThreading,
											 _dwTMShareOleMisc,
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
// 	Function Name:	CTMShareCtrl::XObjSafety::AddRef()
//
// 	Description:	This function is called to attach to the nested
//					IObjectSafety dispatch interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMShareCtrl::XObjSafety::AddRef()
{
    METHOD_PROLOGUE(CTMShareCtrl, ObjSafety)
    return pThis->ExternalAddRef();
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::XObjSafety::GetInterfaceSafetyOptions()
//
// 	Description:	This function is called to get this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMShareCtrl::XObjSafety::GetInterfaceSafetyOptions( 
		/* [in]  */ REFIID riid,
        /* [out] */ DWORD __RPC_FAR *pdwSupportedOptions,
        /* [out] */ DWORD __RPC_FAR *pdwEnabledOptions)
{
	HRESULT			hReturn;
	IUnknown FAR*	pInterface;

	METHOD_PROLOGUE(CTMShareCtrl, ObjSafety)

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
// 	Function Name:	CTMShareCtrl::XObjSafety::QueryInterface()
//
// 	Description:	This function is called to query the IObjectSafety interface
//					for the requested method/property.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT FAR EXPORT CTMShareCtrl::XObjSafety::QueryInterface(REFIID iid, 
														  void FAR* FAR* ppvObj)
{
    METHOD_PROLOGUE(CTMShareCtrl, ObjSafety)
    return (HRESULT)pThis->ExternalQueryInterface(&iid, ppvObj);
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::XObjSafety::Release()
//
// 	Description:	This function is called to detach from the IObjectSafety
//					dispatch interface.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMShareCtrl::XObjSafety::Release()
{
    METHOD_PROLOGUE(CTMShareCtrl, ObjSafety)
    return pThis->ExternalRelease();
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::XObjSafety::SetInterfaceSafetyOptions()
//
// 	Description:	This function is called to set this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMShareCtrl::XObjSafety::SetInterfaceSafetyOptions( 
        /* [in] */ REFIID riid,
        /* [in] */ DWORD dwOptionSetMask,
        /* [in] */ DWORD dwEnabledOptions)
{
	IUnknown FAR* pInterface;

    METHOD_PROLOGUE(CTMShareCtrl, ObjSafety)
	
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
// 	Function Name:	CTMShareCtrl::AboutBox()
//
// 	Description:	This function will display the control's about box.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::AboutBox()
{
	CDialog dlgAbout(IDD_ABOUTBOX_TMSHARE, this);
	dlgAbout.DoModal();
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::CTMShareCtrl()
//
// 	Description:	This is the constructor for CTMShareCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMShareCtrl::CTMShareCtrl()
{
	char szFolder[512];

	InitializeIIDs(&IID_DTMShare6, &IID_DTMShare6Events);

	//	Initialize the non-persistant properties
	m_strMediaId			= "";
	m_strCaseFolder			= "";
	m_strSourceFileName		= "";
	m_strSourceFilePath		= "";
	m_strAppFileSpec		= "";
	m_strAppFolder			= "";
	m_strBarcode			= "";
	m_lCommand				= TMSHARE_COMMAND_NONE;
	m_lPrimaryId			= 0;
	m_lSecondaryId			= 0;
	m_lTertiaryId			= 0;
	m_lQuaternaryId			= 0;
	m_lError				= 0;
	m_lBarcodeId			= 0;
	m_lDisplayOrder			= 0;
	m_lPageNumber			= 0;
	m_sLineNumber			= 0;
	m_uRequestTimer			= 0;
	m_dwProcessId			= 0;
	m_hProcess				= 0;
	m_hMainWnd				= 0;	
	m_bInitialized			= FALSE;	
	memset(&m_CmdRequest, 0, sizeof(m_CmdRequest));
	memset(&m_CmdResponse, 0, sizeof(m_CmdResponse));
	memset(&m_Status, 0, sizeof(m_Status));
	memset(&m_StartupInfo, 0, sizeof(m_StartupInfo));
	memset(&m_ProcessInfo, 0, sizeof(m_ProcessInfo));

	//	Get the registry information
	GetRegistration();

    //	Set default application path
	if(AmbientUserMode() == TRUE)
	{
		_getcwd(szFolder, sizeof(szFolder));
		m_strAppFolder = szFolder;
		if(m_strAppFolder.Right(1) != "\\")
			m_strAppFolder += "\\";

		OnAppFolderChanged();
	}
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::~CTMShareCtrl()
//
// 	Description:	This is the destructor for CTMShareCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMShareCtrl::~CTMShareCtrl()
{
	//	Make sure the control is shut down
	Terminate();
}		

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::DoPropertyExchange()
//
// 	Description:	This function manages the exchange of persistant 
//					properties
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::DoPropExchange(CPropExchange* pPX)
{
	BOOL bEnableErrors = FALSE;
	BOOL bOwner = FALSE;
	BOOL bPeekPeriod = FALSE;
	BOOL bEnableAxErrors = FALSE;
	BOOL bAppFolder = FALSE;

	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
	COleControl::DoPropExchange(pPX);

	try
	{
		//	Load the control's persistent properties
		bEnableErrors = PX_Bool(pPX, _T("EnableErrors"), m_bEnableErrors, TMSHARE_DEFAULT_ENABLE_ERRORS);
		bOwner = PX_Short(pPX, _T("Owner"), m_sOwner, TMSHARE_DEFAULT_OWNER);
		bPeekPeriod = PX_Long(pPX, _T("PeekPeriod"), m_lPeekPeriod, TMSHARE_DEFAULT_PEEK_PERIOD);
		bEnableAxErrors = PX_Bool(pPX, _T("EnableAxErrors"), m_bEnableAxErrors, TMSHARE_DEFAULT_ENABLE_AX_ERRORS);
		bAppFolder = PX_String(pPX, _T("AppFolder"), m_strAppFolder, TMSHARE_DEFAULT_APP_FOLDER);
	}
	catch(...)
	{
		if(!bEnableErrors) m_bEnableErrors = TMSHARE_DEFAULT_ENABLE_ERRORS;
		if(!bOwner) m_sOwner = TMSHARE_DEFAULT_OWNER;
		if(!bPeekPeriod) m_lPeekPeriod = TMSHARE_DEFAULT_PEEK_PERIOD;
		if(!bEnableAxErrors) m_bEnableAxErrors = TMSHARE_DEFAULT_ENABLE_AX_ERRORS;
		if(!bAppFolder) m_strAppFolder = TMSHARE_DEFAULT_APP_FOLDER;
	}
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::Execute()
//
// 	Description:	This function is called to execute the sister application
//
// 	Returns:		TMSHARE_ERROR_NONE if successful
//
//	Notes:			None
//
//==============================================================================
short CTMShareCtrl::Execute() 
{
	CString	strMsg;
	char	szCommandLine[512];

	memset(&m_StartupInfo, 0, sizeof(m_StartupInfo));
	memset(&m_ProcessInfo, 0, sizeof(m_ProcessInfo));
	m_StartupInfo.cb = sizeof(STARTUPINFO);

	if(m_strCommandLine.GetLength() > 0)
		lstrcpyn(szCommandLine, m_strCommandLine, sizeof(szCommandLine));
	else
		memset(szCommandLine, 0, sizeof(szCommandLine));

	//	Launch the process
	if(CreateProcess(m_strAppFileSpec, szCommandLine, NULL, NULL, TRUE,
				     NORMAL_PRIORITY_CLASS, NULL, NULL,
					 &m_StartupInfo, &m_ProcessInfo) == 0)
	{
		strMsg.Format("Unable to open the application:\n\n%s", m_strAppFileSpec);
		m_Errors.Handle(0, strMsg);

		//	Unable to execute the application
		return TMSHARE_ERROR_OPEN_APP_FAILED;
	}

	return TMSHARE_ERROR_NONE;
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::GetClassIdString()
//
// 	Description:	This method is called to get the class GUID as a null
//					terminated string.
//
// 	Returns:		The string equivalent of the CLASS GUID
//
//	Notes:			None
//
//==============================================================================
BSTR CTMShareCtrl::GetClassIdString() 
{
	CString strClsId = m_tmVersion.GetClsId();
	return strClsId.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::GetInitialized()
//
// 	Description:	This function is called to determine if the control has
//					been initialized.
//
// 	Returns:		TRUE if initialized
//
//	Notes:			None
//
//==============================================================================
BOOL CTMShareCtrl::GetInitialized() 
{
	return (m_reqPresentation.GetOpened() && m_resPresentation.GetOpened() &&
			m_reqManager.GetOpened() && m_resManager.GetOpened());
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::GetRegisteredPath()
//
// 	Description:	This method is called to get the ocx path stored in the
//					system registry.
//
// 	Returns:		The path to the ocx stored in the registry
//
//	Notes:			None
//
//==============================================================================
BSTR CTMShareCtrl::GetRegisteredPath() 
{
	CString strRegistered = m_tmVersion.GetFileSpec();
	return strRegistered.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::GetRegistration()
//
// 	Description:	This function is called to get the control's registration
//					information.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::GetRegistration() 
{
	CLSID	clsid;

	//	Get the GUID 
	GetClassID(&clsid);

	//	Initialize the version information
	m_tmVersion.InitFromClsId("TMShare", "Shared Memory Control", clsid);

}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::GetRequest()
//
// 	Description:	This method is called to update the property values
//					using the current command request
//
// 	Returns:		TMSHARE_NO_ERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMShareCtrl::GetRequest() 
{
	short sReturn;

	//	Make sure the control has been initialized
	if(!GetInitialized())
	{
		m_Errors.Handle(0, IDS_NOT_INITIALIZED);
		return TMSHARE_ERROR_NOT_INITIALIZED;
	}

	//	Do we need to check for a request
	if(m_CmdRequest.lCommand == TMSHARE_COMMAND_NONE)
	{
		if((sReturn = ReadRequest()) != TMSHARE_ERROR_NONE)
			return sReturn;
	}

	//	Are we still waiting for a request?
	if(m_CmdRequest.lCommand == TMSHARE_COMMAND_NONE)
	{
		return TMSHARE_ERROR_NO_REQUEST;
	}
	else
	{
		//	Update the property values
		m_lCommand          = m_CmdRequest.lCommand;
		m_lPrimaryId        = m_CmdRequest.lPrimaryId;
		m_lSecondaryId		= m_CmdRequest.lSecondaryId;
		m_lTertiaryId		= m_CmdRequest.lTertiaryId;
		m_lQuaternaryId		= m_CmdRequest.lQuaternaryId;
		m_lDisplayOrder		= m_CmdRequest.lDisplayOrder;
		m_lBarcodeId		= m_CmdRequest.lBarcodeId;
		m_lPageNumber		= m_CmdRequest.lPageNumber;
		m_sLineNumber		= m_CmdRequest.sLineNumber;
		m_strCaseFolder     = m_CmdRequest.szCaseFolder;
		m_strSourceFileName = m_CmdRequest.szSourceFileName;
		m_strSourceFilePath = m_CmdRequest.szSourceFilePath;
		m_strMediaId        = m_CmdRequest.szMediaId;
		m_strBarcode        = m_CmdRequest.szBarcode;

		//	Clear the request buffer
		memset(&m_CmdRequest, 0, sizeof(m_CmdRequest));
		m_CmdRequest.lCommand = TMSHARE_COMMAND_NONE;

		return TMSHARE_ERROR_NONE;
	}

}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::GetResponse()
//
// 	Description:	This method is called to update the property values
//					using the response to the last command request.
//
// 	Returns:		TMSHARE_NO_ERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMShareCtrl::GetResponse() 
{
	short sReturn;

	//	Make sure the control has been initialized
	if(!GetInitialized())
	{
		m_Errors.Handle(0, IDS_NOT_INITIALIZED);
		return TMSHARE_ERROR_NOT_INITIALIZED;
	}

	//	Do we need to read the response
	//
	//	NOTE:	The response may have already been retrieved by the call to
	//			SetReqest()
	if(m_CmdResponse.lCommand == TMSHARE_COMMAND_NONE)
	{
		if((sReturn = ReadResponse()) != TMSHARE_ERROR_NONE)
			return sReturn;
	}

	//	Are we still waiting for a response?
	if(m_CmdResponse.lCommand == TMSHARE_COMMAND_NONE)
	{
		return TMSHARE_ERROR_NO_RESPONSE;
	}
	else
	{
		//	Update the property values
		m_lError            = m_CmdResponse.lError;
		m_lCommand          = m_CmdResponse.lCommand;
		m_lPrimaryId        = m_CmdResponse.lPrimaryId;
		m_lSecondaryId		= m_CmdResponse.lSecondaryId;
		m_lTertiaryId		= m_CmdResponse.lTertiaryId;
		m_lQuaternaryId		= m_CmdResponse.lQuaternaryId;
		m_lDisplayOrder		= m_CmdResponse.lDisplayOrder;
		m_lBarcodeId		= m_CmdResponse.lBarcodeId;
		m_strCaseFolder     = m_CmdResponse.szCaseFolder;
		m_strSourceFileName = m_CmdResponse.szSourceFileName;
		m_strSourceFilePath = m_CmdResponse.szSourceFilePath;
		m_strMediaId        = m_CmdResponse.szMediaId;
		m_strBarcode        = m_CmdResponse.szBarcode;

		//	Clear the response buffer
		memset(&m_CmdResponse, 0, sizeof(m_CmdResponse));
		m_CmdResponse.lCommand = TMSHARE_COMMAND_NONE;

		return TMSHARE_ERROR_NONE;
	}
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::GetSisterFileSpec()
//
// 	Description:	This function is called to get the path to the sister
//					application.
//
// 	Returns:		The sister application path
//
//	Notes:			None
//
//==============================================================================
BSTR CTMShareCtrl::GetSisterFileSpec() 
{
	return m_strAppFileSpec.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::GetVerBuild()
//
// 	Description:	This method is called to get the value of the VerBuild
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMShareCtrl::GetVerBuild() 
{
	return m_tmVersion.GetBuild();
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::GetVerBuildDate()
//
// 	Description:	This method is called to get the value of the VerBuildDate
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMShareCtrl::GetVerBuildDate() 
{
	CString strBuildDate = m_tmVersion.GetBuildDate();
	return strBuildDate.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::GetVerMajor()
//
// 	Description:	This method is called to get the value of the VerMajor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMShareCtrl::GetVerMajor() 
{
	return m_tmVersion.GetMajor();
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::GetVerMinor()
//
// 	Description:	This method is called to get the value of the VerMinor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMShareCtrl::GetVerMinor() 
{
	return m_tmVersion.GetMinor();
}


//==============================================================================
//
// 	Function Name:	CTMShareCtrl::GetVerQEF()
//
// 	Description:	This method is called to get the value of the VerQEF
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMShareCtrl::GetVerQEF() 
{
	return m_tmVersion.GetUpdate();
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::GetVerTextLong()
//
// 	Description:	This method is called to get the value of the VerTextLong
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMShareCtrl::GetVerTextLong() 
{
	CString strVer = m_tmVersion.GetTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::GetVerTextShort()
//
// 	Description:	This method is called to get the value of the VerTextShort
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMShareCtrl::GetVerTextShort() 
{
	CString strVer = m_tmVersion.GetShortTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::Initialize()
//
// 	Description:	This function will initialize the control
//
// 	Returns:		TMDP_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMShareCtrl::Initialize()
{
	short sReturn;

	//	Don't bother if not in user mode
	if(!AmbientUserMode())
		return TMSHARE_ERROR_NONE;

	//	Have we already been initialized?
	if(m_bInitialized == TRUE) return TMSHARE_ERROR_NONE;

	//	Should only be one running instance per application
	if(_pTMShareCtrl != NULL)
	{
		m_Errors.Handle(0, IDS_TMSHARE_ERROR_SINGLE_INSTANCE);
		return TMSHARE_ERROR_SINGLE_INSTANCE;
	}

	//	Set the buffer names
	m_reqPresentation.SetProperties(TMSHARE_PRESENTATION_NAME);
	m_reqManager.SetProperties(TMSHARE_MANAGER_NAME);
	m_resPresentation.SetProperties(TMSHARE_PRESENTATION_NAME);
	m_resManager.SetProperties(TMSHARE_MANAGER_NAME);
	m_statusPresentation.SetProperties(TMSHARE_PRESENTATION_NAME);
	m_statusManager.SetProperties(TMSHARE_MANAGER_NAME);

	//	Open the shared command request buffers
	if(!m_reqPresentation.Open())
	{
		m_Errors.Handle(0, IDS_OPEN_PRESENTATION_REQUEST_FAILED);
		return TMSHARE_ERROR_OPEN_PRESENTATION_REQUEST_FAILED;
	}
	if(!m_reqManager.Open())
	{
		m_Errors.Handle(0, IDS_OPEN_MANAGER_REQUEST_FAILED);
		return TMSHARE_ERROR_OPEN_MANAGER_REQUEST_FAILED;
	}

	//	Open the shared command response buffers
	if(!m_resPresentation.Open())
	{
		m_Errors.Handle(0, IDS_OPEN_PRESENTATION_RESPONSE_FAILED);
		return TMSHARE_ERROR_OPEN_PRESENTATION_RESPONSE_FAILED;
	}
	if(!m_resManager.Open())
	{
		m_Errors.Handle(0, IDS_OPEN_MANAGER_RESPONSE_FAILED);
		return TMSHARE_ERROR_OPEN_MANAGER_RESPONSE_FAILED;
	}

	//	Open the shared status buffers
	if(!m_statusPresentation.Open())
	{
		m_Errors.Handle(0, IDS_OPEN_PRESENTATION_STATUS_FAILED);
		return TMSHARE_ERROR_OPEN_PRESENTATION_STATUS_FAILED;
	}
	if(!m_statusManager.Open())
	{
		m_Errors.Handle(0, IDS_OPEN_MANAGER_STATUS_FAILED);
		return TMSHARE_ERROR_OPEN_MANAGER_STATUS_FAILED;
	}

	//	Get the owner process information
	m_dwProcessId = GetCurrentProcessId();
	m_hProcess    = OpenProcess(PROCESS_ALL_ACCESS, FALSE, m_dwProcessId);
	m_hMainWnd    = AfxGetMainWnd()->m_hWnd;

	//	Notify the sister application that we are available
	if((sReturn = SetStatus()) != TMSHARE_ERROR_NONE)
		return sReturn;

	//	Start the command request timer
	if(!StartTimer())
	{
		m_Errors.Handle(0, IDS_COMMAND_REQUEST_TIMER_FAILED);
		return TMSHARE_ERROR_COMMAND_REQUEST_TIMER_FAILED;
	}

	//	Build the path to the executable
	m_strAppFileSpec.Empty();

	if(m_strAppFolder.GetLength() > 0)
	{
		m_strAppFileSpec = m_strAppFolder;
		if(m_strAppFileSpec.Right(1) != "\\")
			m_strAppFileSpec += "\\";
	}

	if(m_sOwner == TMSHARE_PRESENTATION)
		m_strAppFileSpec += TMSHARE_MANAGER_FILENAME;
	else
		m_strAppFileSpec += TMSHARE_PRESENTATION_FILENAME;

	//	Get the owner process information
	m_dwProcessId = GetCurrentProcessId();
	m_hProcess    = OpenProcess(PROCESS_ALL_ACCESS, FALSE, m_dwProcessId);
	m_hMainWnd    = AfxGetMainWnd()->m_hWnd;

	m_bInitialized = TRUE;

	return TMSHARE_ERROR_NONE;
}	

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::IsRunning()
//
// 	Description:	This function is called to check if the sister application
//					is running
//
// 	Returns:		TRUE if running
//
//	Notes:			None
//
//==============================================================================
BOOL CTMShareCtrl::IsRunning() 
{
	//	Get the current status
	if(ReadStatus() != TMSHARE_ERROR_NONE) return FALSE;

	//	Do we have a valid process handle?
	if(m_Status.hProcess == 0) return FALSE;

	if(IsWindow(m_Status.hWnd) == 0) return FALSE;

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnAppFolderChanged()
//
// 	Description:	This function is called when the AppFolder property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnAppFolderChanged() 
{
	if(AmbientUserMode() == TRUE)
	{
		//	Build the complete path to the application
	
	}

}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnBarcodeChanged()
//
// 	Description:	This function is called when the Barcode property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnBarcodeChanged() 
{
	//	Not persistant
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnBarcodeIdChanged()
//
// 	Description:	This function is called when the BarcodeId property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnBarcodeIdChanged() 
{
	//	Not persistant
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnCommandChanged()
//
// 	Description:	This function is called when the Command property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnCommandChanged() 
{
	//	Not persistant
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnCreate()
//
// 	Description:	This fuction handles all WM_CREATE messages
//
// 	Returns:		0 if successful
//
//	Notes:			None
//
//==============================================================================
int CTMShareCtrl::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
	//	Do the base class processing first
	if(COleControl::OnCreate(lpCreateStruct) == -1)
		return -1;

	//	Initialize the error handler
	m_Errors.Enable(m_bEnableErrors);
	m_Errors.SetParent(m_hWnd);
	m_Errors.SetTitle("TMShare Error");
	m_Errors.SetMessageId(m_bEnableAxErrors == TRUE ? WM_ERROR_EVENT : 0);

	return 0;
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnCaseFolderChanged()
//
// 	Description:	This function is called when the CaseFolder property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnCaseFolderChanged() 
{
	//	Not persistant
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnDisplayOrderChanged()
//
// 	Description:	This function is called when the DisplayOrder property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnDisplayOrderChanged() 
{
	//	Not persistant
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnDraw()
//
// 	Description:	This function is called in response to WM_PAINT messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnDraw(CDC* pdc, const CRect& rcBounds,const CRect& rcInvalid)
{
	CBrush	brBackground;
	CString	strVersion;
	CRect	ControlRect = rcBounds;

	if(!AmbientUserMode())
	{
		//	Paint the background
		brBackground.CreateSolidBrush(RGB(0,0,0));
		pdc->FillRect(ControlRect, &brBackground);

		pdc->SetBkMode(TRANSPARENT);
		pdc->SetTextColor(RGB(128,128,128));

		strVersion.Format("FTI Shared (rev. %d.%d)", _wVerMajor, _wVerMinor);
	
		pdc->DrawText(strVersion, ControlRect, 
					  DT_CENTER | DT_NOCLIP | DT_VCENTER | DT_SINGLELINE); 
	}
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnEnableAxErrorsChanged()
//
// 	Description:	This function is called when the EnableAxErrors property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnEnableAxErrorsChanged() 
{
	SetModifiedFlag();

	if(AmbientUserMode())
		m_Errors.SetMessageId(m_bEnableAxErrors == TRUE ? WM_ERROR_EVENT : 0);
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnEnableErrorsChanged()
//
// 	Description:	This function is called when the EnableErrors property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnEnableErrorsChanged() 
{
	if(AmbientUserMode())
		m_Errors.Enable(m_bEnableErrors);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnErrorChanged()
//
// 	Description:	This function is called when the Error property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnErrorChanged() 
{
	//	Not persistant
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnGetDisplayString()
//
// 	Description:	This function is called by the framework to get the display
//					string for the specified property value.
//
// 	Returns:		TRUE if the string has been set
//
//	Notes:			None
//
//==============================================================================
BOOL CTMShareCtrl::OnGetDisplayString(DISPID dispid, CString& strValue) 
{
	//	Is this the Owner property?
	if(dispid == dispidOwner)
	{
		switch(m_sOwner)
		{
			case TMSHARE_MANAGER:	

				strValue = _T("Manager");
				return TRUE;
			
			case TMSHARE_PRESENTATION:	

				strValue = _T("Presentation");
				return TRUE;
			
			default:			

				return FALSE;
		}
		
	}
	//	Is this the Command property?
	if(dispid == dispidCommand)
	{
		switch(m_lCommand)
		{
			case TMSHARE_COMMAND_NONE:	

				strValue = _T("None");
				return TRUE;
			
			case TMSHARE_COMMAND_ADD_TREATMENT:	

				strValue = _T("Add Treatment");
				return TRUE;
			
			case TMSHARE_COMMAND_ADD_TO_BINDER:	

				strValue = _T("Add To Binder");
				return TRUE;
			
			case TMSHARE_COMMAND_UPDATE_TREATMENT:	

				strValue = _T("Update Treatment");
				return TRUE;
			
			default:			

				return FALSE;
		}
		
	}
	else
	{
		return COleControl::OnGetDisplayString(dispid, strValue);
	}
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnGetPredefinedStrings()
//
// 	Description:	This function is called by the framework to get the display
//					strings associated with a property.
//
// 	Returns:		TRUE if values have been provided
//
//	Notes:			None
//
//==============================================================================
BOOL CTMShareCtrl::OnGetPredefinedStrings(DISPID dispid, 
										  CStringArray* pStringArray, 
										  CDWordArray* pCookieArray) 
{
	BOOL bResult = FALSE;

	//	Is this the Update property?
	if(dispid == dispidOwner)
	{
		TRY
		{
			// Fill in the values in pStringArray and pCookieArray
			CString Label(_T("Manager"));
			pStringArray->Add(Label);
			pCookieArray->Add(TMSHARE_MANAGER);
        
			Label = _T("Presentation");
			pStringArray->Add(Label);
			pCookieArray->Add(TMSHARE_PRESENTATION);

			bResult = TRUE;
		}
		CATCH(CException, e)
		{
			pStringArray->RemoveAll();
			pCookieArray->RemoveAll();
        
			bResult = FALSE;
		}
		END_CATCH
	}
		
	//	Is this the Command property?
	else if(dispid == dispidCommand)
	{
		TRY
		{
			// Fill in the values in pStringArray and pCookieArray
			CString Label;
			Label.Format("%d - None", TMSHARE_COMMAND_NONE);
			pStringArray->Add(Label);
			pCookieArray->Add(TMSHARE_COMMAND_NONE);
        
			Label.Format("%d - Add Treatment", TMSHARE_COMMAND_ADD_TREATMENT);
			pStringArray->Add(Label);
			pCookieArray->Add(TMSHARE_COMMAND_ADD_TREATMENT);

			Label.Format("%d - Add To Binder", TMSHARE_COMMAND_ADD_TO_BINDER);
			pStringArray->Add(Label);
			pCookieArray->Add(TMSHARE_COMMAND_ADD_TO_BINDER);

			Label.Format("%d - Update Treatment", TMSHARE_COMMAND_UPDATE_TREATMENT);
			pStringArray->Add(Label);
			pCookieArray->Add(TMSHARE_COMMAND_UPDATE_TREATMENT);

			bResult = TRUE;
		}
		CATCH(CException, e)
		{
			pStringArray->RemoveAll();
			pCookieArray->RemoveAll();
        
			bResult = FALSE;
		}
		END_CATCH
	}
		
	if(bResult)
		return TRUE;
	else
		return COleControl::OnGetPredefinedStrings(dispid, pStringArray, 
												   pCookieArray);
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnGetPredefinedValue()
//
// 	Description:	This function is called by the framework to get the variant
//					form of the specified property value.
//
// 	Returns:		TRUE if values have been provided
//
//	Notes:			None
//
//==============================================================================
BOOL CTMShareCtrl::OnGetPredefinedValue(DISPID dispid, DWORD dwCookie, 
									    VARIANT* lpvarOut) 
{
	//	Is this the Owner property?
	if(dispid == dispidOwner)
	{
		VariantClear(lpvarOut);
		V_VT(lpvarOut) = VT_I2;
		V_I2(lpvarOut) = (short)dwCookie;
		return TRUE;
	}

	//	Is this the Command property?
	if(dispid == dispidCommand)
	{
		VariantClear(lpvarOut);
		V_VT(lpvarOut) = VT_I4;
		V_I2(lpvarOut) = (short)dwCookie;
		return TRUE;
	}

	return COleControl::OnGetPredefinedValue(dispid, dwCookie, lpvarOut);
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnLineNumberChanged()
//
// 	Description:	This function is called when the LineNumber property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnLineNumberChanged() 
{
	//	Not persistant
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnMediaIdChanged()
//
// 	Description:	This function is called when the MediaId property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnMediaIdChanged() 
{
	//	Not persistant
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnBuildChanged()
//
// 	Description:	This function is called when the Owner property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnOwnerChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnPageNumberChanged()
//
// 	Description:	This function is called when the PageNumber property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnPageNumberChanged() 
{
	//	Not persistant
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnPeekPeriodChanged()
//
// 	Description:	This function is called when the PeekPeriod property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnPeekPeriodChanged() 
{
	if(AmbientUserMode())
	{
		//	Restart the timer if the control has been initialized
		if(m_uRequestTimer > 0)
			StartTimer();
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnPrimaryIdChanged()
//
// 	Description:	This function is called when the PrimaryId property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnPrimaryIdChanged() 
{
	//	Not persistant
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnQuaternaryIdChanged()
//
// 	Description:	This function is called when the QuaternaryId property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnQuaternaryIdChanged() 
{
	//	Not persistant
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnSecondaryIdChanged()
//
// 	Description:	This function is called when the SecondaryId property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnSecondaryIdChanged() 
{
	//	Not persistant
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnSourceFileNameChanged()
//
// 	Description:	This function is called when the SourceFileName property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnSourceFileNameChanged() 
{
	//	Not persistant
}

//==============================================================================
//
// 	Function Path:	CTMShareCtrl::OnSourceFilePathChanged()
//
// 	Description:	This function is called when the SourceFilePath property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnSourceFilePathChanged() 
{
	//	Not persistant
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnTertiaryIdChanged()
//
// 	Description:	This function is called when the TertiaryId property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnTertiaryIdChanged() 
{
	//	Not persistant
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnTimer()
//
// 	Description:	This function is called to handle all timer notifications
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::OnTimer(UINT idEvent, DWORD dwTime) 
{
	long lWrites;

	//	Make sure the control is properly initialized
	if(!GetInitialized()) return;

	//	Check for a command request
	if(m_sOwner == TMSHARE_MANAGER)
		lWrites = m_reqManager.Read(&m_CmdRequest);
	else
		lWrites = m_reqPresentation.Read(&m_CmdRequest);

	//	Is there a new reqest?
	if(lWrites > 0)
	{
		//	Notify the container
		FireCommandRequest();
	}

	//	Check for a command response
	if(m_sOwner == TMSHARE_MANAGER)
		lWrites = m_resManager.Read(&m_CmdResponse);
	else
		lWrites = m_resPresentation.Read(&m_CmdResponse);

	//	Is there a new response?
	if(lWrites > 0)
	{
		//	Notify the container
		FireCommandResponse();
	}

}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::OnWMErrorEvent()
//
// 	Description:	This function handles all WM_ERROR_EVENT messages
//
// 	Returns:		Zero if the message is handled
//
//	Notes:			None
//
//==============================================================================
LONG CTMShareCtrl::OnWMErrorEvent(WPARAM wParam, LPARAM lParam)
{
	if((m_bEnableAxErrors == TRUE) && (lstrlen(m_Errors.GetMessage()) > 0))
	{
		FireAxError(m_Errors.GetMessage());
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::ReadRequest()
//
// 	Description:	This method is called to read the request buffer
//
// 	Returns:		TMSHARE_ERROR_NONE if successful
//
//	Notes:			None
//
//==============================================================================
short CTMShareCtrl::ReadRequest() 
{
	long lWrites;

	ASSERT(GetInitialized());

	//	Read the request buffer for this application
	if(m_sOwner == TMSHARE_PRESENTATION)
	{
		if((lWrites = m_reqPresentation.Read(&m_CmdRequest)) < 0)
		{
			m_Errors.Handle(0, IDS_READ_PRESENTATION_REQUEST_FAILED);
			return TMSHARE_ERROR_READ_PRESENTATION_REQUEST_FAILED;
		}
	}
	else
	{
		if((lWrites = m_reqManager.Read(&m_CmdRequest)) < 0)
		{
			m_Errors.Handle(0, IDS_READ_MANAGER_REQUEST_FAILED);
			return TMSHARE_ERROR_READ_MANAGER_REQUEST_FAILED;
		}
	}

	return TMSHARE_ERROR_NONE;
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::ReadResponse()
//
// 	Description:	This method is called to read the response from the slave
//					application
//
// 	Returns:		TMSHARE_ERROR_NONE if successful
//
//	Notes:			None
//
//==============================================================================
short CTMShareCtrl::ReadResponse() 
{
	long lWrites;

	ASSERT(GetInitialized());

	//	Read the response buffer for this application
	if(m_sOwner == TMSHARE_PRESENTATION)
	{
		if((lWrites = m_resPresentation.Read(&m_CmdResponse)) < 0)
		{
			m_Errors.Handle(0, IDS_READ_PRESENTATION_RESPONSE_FAILED);
			return TMSHARE_ERROR_READ_PRESENTATION_RESPONSE_FAILED;
		}
	}
	else
	{
		if((lWrites = m_resManager.Read(&m_CmdResponse)) < 0)
		{
			m_Errors.Handle(0, IDS_READ_MANAGER_RESPONSE_FAILED);
			return TMSHARE_ERROR_READ_MANAGER_RESPONSE_FAILED;
		}
	}

	return TMSHARE_ERROR_NONE;
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::ReadStatus()
//
// 	Description:	This method is called to read the status from the sister
//					application
//
// 	Returns:		TMSHARE_ERROR_NONE if successful
//
//	Notes:			None
//
//==============================================================================
short CTMShareCtrl::ReadStatus() 
{
	long lWrites;

	ASSERT(GetInitialized());

	//	Read the status buffer of the sister application
	if(m_sOwner == TMSHARE_MANAGER)
	{
		if((lWrites = m_statusPresentation.Read(&m_Status, FALSE)) < 0)
		{
			m_Errors.Handle(0, IDS_READ_PRESENTATION_STATUS_FAILED);
			return TMSHARE_ERROR_READ_PRESENTATION_STATUS_FAILED;
		}
	}
	else
	{
		if((lWrites = m_statusManager.Read(&m_Status, FALSE)) < 0)
		{
			m_Errors.Handle(0, IDS_READ_MANAGER_STATUS_FAILED);
			return TMSHARE_ERROR_READ_MANAGER_STATUS_FAILED;
		}
	}

	return TMSHARE_ERROR_NONE;
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::SetCommandLine()
//
// 	Description:	This method is called to build a command line to launch
//					the sister application with
//
// 	Returns:		TMSHARE_NO_ERROR if successful
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::SetCommandLine() 
{
	CString strPageNumber = "";
	CString	strLineNumber = "";

	if(m_sOwner == TMSHARE_MANAGER)
	{
		//	Set up the command line for Presentation
		m_strCommandLine = " -Q";	//	Quiet mode

		if(m_strCaseFolder.GetLength() > 0)
		{
			m_strCommandLine += (" -C:\"" + m_strCaseFolder);
			m_strCommandLine += "\"";
		}

		if(m_strBarcode.GetLength() > 0)
		{
			m_strCommandLine += (" -B:\"" + m_strBarcode);
			m_strCommandLine += "\"";
		}

		if(m_lPageNumber > 0)
		{
			strPageNumber.Format("%ld", m_lPageNumber);
			m_strCommandLine += (" -P:\"" + strPageNumber);
			m_strCommandLine += "\"";
		}

		if(m_sLineNumber > 0)
		{
			strLineNumber.Format("%d", m_sLineNumber);
			m_strCommandLine += (" -L:\"" + strLineNumber);
			m_strCommandLine += "\"";
		}

	}
	else
	{
		m_strCommandLine = "";
	}

}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::SetRequest()
//
// 	Description:	This method is called to trigger a command request
//
// 	Returns:		TMSHARE_NO_ERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMShareCtrl::SetRequest(long lWaitRespose) 
{
	SRequest		Request;
	long			lWrites;
	DWORD			dwStart;
	short			sReturn;

	//	Make sure the control has been initialized
	if(!GetInitialized())
	{
		m_Errors.Handle(0, IDS_NOT_INITIALIZED);
		return TMSHARE_ERROR_NOT_INITIALIZED;
	}

	//	Do we need to launch the sister application?
	if(IsRunning() == FALSE)
	{
		//	Set the command line using the current request parameters
		SetCommandLine();

		if((sReturn = Execute()) != TMSHARE_ERROR_NONE)
		{
			return sReturn;
		}

	}
	else
	{
		//	Use the current property values to set the request information
		memset(&Request, 0, sizeof(Request));
		Request.lCommand		= m_lCommand;
		Request.lPrimaryId		= m_lPrimaryId;
		Request.lSecondaryId	= m_lSecondaryId;
		Request.lTertiaryId		= m_lTertiaryId;
		Request.lQuaternaryId	= m_lQuaternaryId;
		Request.lDisplayOrder	= m_lDisplayOrder;
		Request.lBarcodeId		= m_lBarcodeId;
		Request.lPageNumber		= m_lPageNumber;
		Request.sLineNumber		= m_sLineNumber;
		lstrcpyn(Request.szCaseFolder, m_strCaseFolder, sizeof(Request.szCaseFolder));
		lstrcpyn(Request.szSourceFileName, m_strSourceFileName, sizeof(Request.szSourceFileName));
		lstrcpyn(Request.szSourceFilePath, m_strSourceFilePath, sizeof(Request.szSourceFilePath));
		lstrcpyn(Request.szMediaId, m_strMediaId, sizeof(Request.szMediaId));
		lstrcpyn(Request.szBarcode, m_strBarcode, sizeof(Request.szBarcode));

		//	Transfer to the request buffer associated with the attached application
		if(m_sOwner == TMSHARE_MANAGER)
		{
			if((lWrites = m_reqPresentation.Write(&Request)) < 0)
			{
				m_Errors.Handle(0, IDS_WRITE_PRESENTATION_REQUEST_FAILED);
				return TMSHARE_ERROR_WRITE_PRESENTATION_REQUEST_FAILED;
			}
		}
		else
		{
			if((lWrites = m_reqManager.Write(&Request)) < 0)
			{
				m_Errors.Handle(0, IDS_WRITE_MANAGER_REQUEST_FAILED);
				return TMSHARE_ERROR_WRITE_MANAGER_REQUEST_FAILED;
			}
		}
	}

	//	Does the caller want to wait for a response?
	if(lWaitRespose > 0)
	{
		dwStart = GetTickCount();
		
		//	Loop until there is a valid response
		while(1)
		{
			//	Get the response from the slave application
			if((sReturn = ReadResponse()) == TMSHARE_ERROR_NONE)
			{
				//	Are we still waiting for a response?
				if(m_CmdResponse.lCommand == TMSHARE_COMMAND_NONE)
				{
					//	Have we timed out?
					if(((long)(GetTickCount() - dwStart)) > lWaitRespose)
					{
						m_Errors.Handle(0, IDS_REQUEST_TIMED_OUT);
						return TMSHARE_ERROR_REQUEST_TIMED_OUT;
					}
					else
					{
						//	Wait a little bit
						Sleep(100);
					}
				}
				else
				{
					//	Stop here
					return TMSHARE_ERROR_NONE;
				}
			}
			else
			{
				//	Unable to read slave response
				return sReturn;
			}
		}
	}
	else
	{
		return TMSHARE_ERROR_NONE;
	}

}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::SetResponse()
//
// 	Description:	This method is called to respond to a pending
//					command request.
//
// 	Returns:		TMSHARE_NO_ERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMShareCtrl::SetResponse() 
{
	SResponse	Response;
	long		lWrites;

	//	Make sure the control has been initialized
	if(!GetInitialized())
	{
		m_Errors.Handle(0, IDS_NOT_INITIALIZED);
		return TMSHARE_ERROR_NOT_INITIALIZED;
	}

	//	Use the current property values to set the command response
	memset(&Response, 0, sizeof(Response));
	Response.lError			= m_lError;
	Response.lCommand		= m_lCommand;
	Response.lPrimaryId		= m_lPrimaryId;
	Response.lSecondaryId	= m_lSecondaryId;
	Response.lTertiaryId	= m_lTertiaryId;
	Response.lQuaternaryId	= m_lQuaternaryId;
	Response.lDisplayOrder	= m_lDisplayOrder;
	Response.lBarcodeId		= m_lBarcodeId;
	lstrcpyn(Response.szCaseFolder, m_strCaseFolder, sizeof(Response.szCaseFolder));
	lstrcpyn(Response.szSourceFileName, m_strSourceFileName, sizeof(Response.szSourceFileName));
	lstrcpyn(Response.szSourceFilePath, m_strSourceFilePath, sizeof(Response.szSourceFilePath));
	lstrcpyn(Response.szMediaId, m_strMediaId, sizeof(Response.szMediaId));
	lstrcpyn(Response.szBarcode, m_strBarcode, sizeof(Response.szBarcode));

	//	Update the response buffer associated with the sister application
	if(m_sOwner == TMSHARE_PRESENTATION)
	{
		if((lWrites = m_resManager.Write(&Response)) < 0)
		{
			m_Errors.Handle(0, IDS_WRITE_MANAGER_RESPONSE_FAILED);
			return TMSHARE_ERROR_WRITE_MANAGER_RESPONSE_FAILED;
		}
	}
	else
	{
		if((lWrites = m_resPresentation.Write(&Response)) < 0)
		{
			m_Errors.Handle(0, IDS_WRITE_PRESENTATION_RESPONSE_FAILED);
			return TMSHARE_ERROR_WRITE_PRESENTATION_RESPONSE_FAILED;
		}
	}

	return TMSHARE_ERROR_NONE;
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::SetStatus()
//
// 	Description:	This function is called to set the status for the owner
//					application.
//
// 	Returns:		TMSHARE_ERROR_NONE if successful
//
//	Notes:			None
//
//==============================================================================
short CTMShareCtrl::SetStatus() 
{
	long	lWrites = 0;
	SStatus	Status;

	//	Initialize the transfer structure
	Status.dwProcessId = m_dwProcessId;
	Status.hProcess    = m_hProcess;
	Status.hWnd        = m_hMainWnd;

	//	Write to the owners shared memory region
	if(m_sOwner == TMSHARE_MANAGER)
	{
		if((lWrites = m_statusManager.Write(&Status)) < 0)
		{
			m_Errors.Handle(0, IDS_WRITE_MANAGER_STATUS_FAILED);
			return TMSHARE_ERROR_WRITE_MANAGER_STATUS_FAILED;
		}
	}
	else
	{
		if((lWrites = m_statusPresentation.Write(&Status)) < 0)
		{
			m_Errors.Handle(0, IDS_WRITE_PRESENTATION_STATUS_FAILED);
			return TMSHARE_ERROR_WRITE_PRESENTATION_STATUS_FAILED;
		}
	}
	return TMSHARE_ERROR_NONE;
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::StartTimer()
//
// 	Description:	This function is called to start the timer used to
//					monitor the shared memory buffer.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMShareCtrl::StartTimer() 
{
	//	Kill the existing timer
	StopTimer();

	//	Start the timer using the specified peek period
	if((m_uRequestTimer = ::SetTimer(0, 0, m_lPeekPeriod, TimerProc)) > 0)
	{
		_pTMShareCtrl = this;
		return TRUE;
	}
	else
	{
		return FALSE;
	}

}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::StopTimer()
//
// 	Description:	This function is called to stop the timer used to
//					monitor the shared memory buffer.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::StopTimer() 
{
	//	This prevents callback processing
	_pTMShareCtrl = NULL;

	//	Kill the command request timer
	if(m_uRequestTimer > 0)
	{
		::KillTimer(0, m_uRequestTimer);
		m_uRequestTimer = 0;
	}
}

//==============================================================================
//
// 	Function Name:	CTMShareCtrl::Terminate()
//
// 	Description:	This function is called to terminate all operations.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMShareCtrl::Terminate() 
{
	//	Have we already terminated?
	if(m_bInitialized == TRUE)
	{
		m_bInitialized = FALSE;

		//	Kill the timer
		StopTimer();

		//	Clear the owner process information
		if(m_hProcess != 0)
			CloseHandle(m_hProcess);
		m_dwProcessId = 0;
		m_hProcess    = 0;
		m_hMainWnd    = 0;
		SetStatus();

		//	Close the sister handles if we launched it
		if(m_ProcessInfo.hProcess != 0)
			CloseHandle(m_ProcessInfo.hProcess); 
		if(m_ProcessInfo.hProcess != 0)
			CloseHandle(m_ProcessInfo.hThread); 

		//	Make sure the shared memory objects are closed
		m_reqPresentation.Close();
		m_resPresentation.Close();
		m_statusPresentation.Close();
		m_reqManager.Close();
		m_resManager.Close();
		m_statusManager.Close();

	}

}

