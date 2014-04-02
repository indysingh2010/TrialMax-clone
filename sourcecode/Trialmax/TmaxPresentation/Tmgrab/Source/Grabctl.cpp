//==============================================================================
//
// File Name:	grabctl.cpp
//
// Description:	This file contains member functions of the CTMGrabCtrl class.
//
// See Also:	grabctl.h
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	12-27-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <grabapp.h>
#include <grabctl.h>
#include <grabpg.h>
#include <grabdefs.h>
#include <regcats.h>
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
extern CTMGrabApp NEAR	theApp;


// Interface IDs

/* Replace 2 */
const IID BASED_CODE IID_DTMGrab6 =
		{ 0x7eb545c4, 0xfb50, 0x41eb, { 0xaa, 0xf, 0xdf, 0xc6, 0x79, 0xf2, 0x46, 0x80 } };
/* Replace 3 */
const IID BASED_CODE IID_DTMGrab6Events =
		{ 0xab015be7, 0x6e4b, 0x4991, { 0x95, 0x17, 0xe, 0xef, 0x54, 0xf0, 0xb5, 0xa } };

// Control type information
static const DWORD BASED_CODE _dwTMGrabOleMisc =
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
BEGIN_MESSAGE_MAP(CTMGrabCtrl, COleControl)
	//{{AFX_MSG_MAP(CTMGrabCtrl)
	//}}AFX_MSG_MAP
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
END_MESSAGE_MAP()

// Dispatch map
BEGIN_DISPATCH_MAP(CTMGrabCtrl, COleControl)
	//{{AFX_DISPATCH_MAP(CTMGrabCtrl)
	DISP_PROPERTY_EX(CTMGrabCtrl, "VerBuild", GetVerBuild, SetNotSupported, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMGrabCtrl, "EnableErrors", m_bEnableErrors, OnEnableErrorsChanged, VT_BOOL)
	DISP_PROPERTY_EX(CTMGrabCtrl, "VerMajor", GetVerMajor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMGrabCtrl, "VerMinor", GetVerMinor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMGrabCtrl, "VerTextLong", GetVerTextLong, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMGrabCtrl, "IniFile", m_strIniFile, OnIniFileChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMGrabCtrl, "Area", m_sArea, OnAreaChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMGrabCtrl, "Silent", m_bSilent, OnSilentChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMGrabCtrl, "Hotkey", m_sHotkey, OnHotkeyChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMGrabCtrl, "CancelKey", m_sCancelKey, OnCancelKeyChanged, VT_I2)
	DISP_FUNCTION(CTMGrabCtrl, "GetRegisteredPath", GetRegisteredPath, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMGrabCtrl, "GetClassIdString", GetClassIdString, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMGrabCtrl, "Initialize", Initialize, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMGrabCtrl, "Capture", Capture, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMGrabCtrl, "Stop", Stop, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMGrabCtrl, "Save", Save, VT_I2, VTS_BSTR VTS_I2 VTS_I2 VTS_I2 VTS_I2)
	//}}AFX_DISPATCH_MAP

	//	Added rev 6.1.0
	DISP_PROPERTY_EX_ID(CTMGrabCtrl, "VerTextShort", DISPID_VERTEXTSHORT, GetVerTextShort, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_EX_ID(CTMGrabCtrl, "VerQEF", DISPID_VERQEF, GetVerQEF, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX_ID(CTMGrabCtrl, "VerBuildDate", DISPID_VERBUILDDATE, GetVerBuildDate, SetNotSupported, VT_BSTR)

	DISP_PROPERTY_NOTIFY_ID(CTMGrabCtrl, "OneShot", DISPID_ONESHOT, m_bOneShot, OnOneShotChanged, VT_BOOL)

	DISP_FUNCTION_ID(CTMGrabCtrl, "AboutBox", DISPID_ABOUTBOX, AboutBox, VT_EMPTY, VTS_NONE)

END_DISPATCH_MAP()

BEGIN_EVENT_MAP(CTMGrabCtrl, COleControl)
	//{{AFX_EVENT_MAP(CTMGrabCtrl)
	EVENT_CUSTOM("Captured", FireCaptured, VTS_NONE)
	//}}AFX_EVENT_MAP
END_EVENT_MAP()

// Property pages
BEGIN_PROPPAGEIDS(CTMGrabCtrl, 1)
	PROPPAGEID(CTMGrabProperties::guid)
END_PROPPAGEIDS(CTMGrabCtrl)

// Initialize class factory and guid
/* Replace 4 */
IMPLEMENT_OLECREATE_EX(CTMGrabCtrl, "TMGRAB6.TMGrabCtrl.1",
	0x4ba3488c, 0x31ec, 0x4619, 0x9d, 0x96, 0x1e, 0xfe, 0x59, 0x2d, 0xd8, 0x61)

// Type library ID and version
IMPLEMENT_OLETYPELIB(CTMGrabCtrl, _tlid, _wVerMajor, _wVerMinor)
IMPLEMENT_OLECTLTYPE(CTMGrabCtrl, IDS_TMGRAB, _dwTMGrabOleMisc)

IMPLEMENT_DYNCREATE(CTMGrabCtrl, COleControl)

// Interface map for IObjectSafety
BEGIN_INTERFACE_MAP(CTMGrabCtrl, COleControl )
	INTERFACE_PART(CTMGrabCtrl, IID_IObjectSafety, ObjSafety)
END_INTERFACE_MAP()

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::CTMGrabCtrlFactory::UpdateRegistry
//
// 	Description:	Adds or removes system registry entries for this control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTMGrabCtrl::CTMGrabCtrlFactory::UpdateRegistry(BOOL bRegister)
{
	BOOL	bReturn;
	HRESULT	hResult;

	if(bRegister)
	{
		bReturn = AfxOleRegisterControlClass(AfxGetInstanceHandle(),
											 m_clsid,
											 m_lpszProgID,
											 IDS_TMGRAB,
											 IDB_TMGRAB,
											 afxRegApartmentThreading,
											 _dwTMGrabOleMisc,
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
// 	Function Name:	CTMGrabCtrl::XObjSafety::AddRef()
//
// 	Description:	This function is called to attach to the nested
//					IObjectSafety dispatch interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMGrabCtrl::XObjSafety::AddRef()
{
    METHOD_PROLOGUE(CTMGrabCtrl, ObjSafety)
    return pThis->ExternalAddRef();
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::XObjSafety::GetInterfaceSafetyOptions()
//
// 	Description:	This function is called to get this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMGrabCtrl::XObjSafety::GetInterfaceSafetyOptions( 
		/* [in]  */ REFIID riid,
        /* [out] */ DWORD __RPC_FAR *pdwSupportedOptions,
        /* [out] */ DWORD __RPC_FAR *pdwEnabledOptions)
{
	HRESULT			hReturn;
	IUnknown FAR*	pInterface;

	METHOD_PROLOGUE(CTMGrabCtrl, ObjSafety)

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
// 	Function Name:	CTMGrabCtrl::XObjSafety::QueryInterface()
//
// 	Description:	This function is called to query the IObjectSafety interface
//					for the requested method/property.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT FAR EXPORT CTMGrabCtrl::XObjSafety::QueryInterface(REFIID iid, 
														  void FAR* FAR* ppvObj)
{
    METHOD_PROLOGUE(CTMGrabCtrl, ObjSafety)
    return (HRESULT)pThis->ExternalQueryInterface(&iid, ppvObj);
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::XObjSafety::Release()
//
// 	Description:	This function is called to detach from the IObjectSafety
//					dispatch interface.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMGrabCtrl::XObjSafety::Release()
{
    METHOD_PROLOGUE(CTMGrabCtrl, ObjSafety)
    return pThis->ExternalRelease();
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::XObjSafety::SetInterfaceSafetyOptions()
//
// 	Description:	This function is called to set this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMGrabCtrl::XObjSafety::SetInterfaceSafetyOptions( 
        /* [in] */ REFIID riid,
        /* [in] */ DWORD dwOptionSetMask,
        /* [in] */ DWORD dwEnabledOptions)
{
	IUnknown FAR* pInterface;

    METHOD_PROLOGUE(CTMGrabCtrl, ObjSafety)
	
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
// 	Function Name:	CTMGrabCtrl::AboutBox()
//
// 	Description:	This function will display the control's about box.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMGrabCtrl::AboutBox()
{
	CDialog dlgAbout(IDD_ABOUTBOX_TMGRAB, this);
	dlgAbout.DoModal();
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::Capture()
//
// 	Description:	This method is called to trigger a capture session
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
short CTMGrabCtrl::Capture() 
{
	//	Make sure the control has been initialized
	if(IsWindow(m_Frame.m_hWnd))
	{
		if(!m_Frame.Capture())
		{
			return TMGRAB_ERROR_CAPTURE_FAILED;
		}
		else
		{
			return TMGRAB_ERROR_NONE;
		}
	}
	else
	{
		m_Errors.Handle(0, IDS_NOT_INITIALIZED);
		return TMGRAB_ERROR_NOT_INITIALIZED;
	}
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::CTMGrabCtrl()
//
// 	Description:	This is the constructor for CTMGrabCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMGrabCtrl::CTMGrabCtrl()
{
	InitializeIIDs(&IID_DTMGrab6, &IID_DTMGrab6Events);

	//	Get the registry information
	GetRegistration();
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::~CTMGrabCtrl()
//
// 	Description:	This is the destructor for CTMGrabCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMGrabCtrl::~CTMGrabCtrl()
{
}		

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::DoPropertyExchange()
//
// 	Description:	This function manages the exchange of persistant 
//					properties
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMGrabCtrl::DoPropExchange(CPropExchange* pPX)
{
	BOOL bEnableErrors = FALSE;
	BOOL bIniFile = FALSE;
	BOOL bArea = FALSE;
	BOOL bSilent = FALSE;
	BOOL bHotkey = FALSE;
	BOOL bCancelKey = FALSE;
	BOOL bOneShot = FALSE;

	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
	COleControl::DoPropExchange(pPX);

	try
	{
		//	Load the control's persistent properties
		bEnableErrors = PX_Bool(pPX, _T("EnableErrors"), m_bEnableErrors, TMGRAB_DEFAULT_ENABLE_ERRORS);
		bIniFile = PX_String(pPX, _T("IniFile"), m_strIniFile, TMGRAB_DEFAULT_INI_FILE);
		bArea = PX_Short(pPX, _T("Area"), m_sArea, TMGRAB_DEFAULT_AREA);
		bSilent = PX_Bool(pPX, _T("Silent"), m_bSilent, TMGRAB_DEFAULT_SILENT);
		bHotkey = PX_Short(pPX, _T("Hotkey"), m_sHotkey, TMGRAB_DEFAULT_HOTKEY);
		bCancelKey = PX_Short(pPX, _T("CancelKey"), m_sCancelKey, TMGRAB_DEFAULT_CANCELKEY);
		bOneShot = PX_Bool(pPX, _T("OneShot"), m_bOneShot, TMGRAB_DEFAULT_ONESHOT);
	}
	catch(...)
	{
		if(!bEnableErrors) m_bEnableErrors = TMGRAB_DEFAULT_ENABLE_ERRORS;
		if(!bIniFile) m_strIniFile = TMGRAB_DEFAULT_INI_FILE;
		if(!bArea) m_sArea = TMGRAB_DEFAULT_AREA;
		if(!bSilent) m_bSilent = TMGRAB_DEFAULT_SILENT;
		if(!bHotkey) m_sHotkey = TMGRAB_DEFAULT_HOTKEY;
		if(!bCancelKey) m_sCancelKey = TMGRAB_DEFAULT_CANCELKEY;
		if(!bOneShot) m_bOneShot = TMGRAB_DEFAULT_ONESHOT;
	}

	if(pPX->IsLoading())
	{
		//	Set default values for properties added after initial release
		//	of the major version
		//
		//	NOTE:	The drop through design of the switch statement is intentional
		switch(LOWORD(pPX->GetVersion()))
		{
			case 0:
			case 1:

				m_bOneShot = TMGRAB_DEFAULT_ONESHOT;
				break;

		}

	}
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::GetClassIdString()
//
// 	Description:	This method is called to get the class GUID as a null
//					terminated string.
//
// 	Returns:		The string equivalent of the CLASS GUID
//
//	Notes:			None
//
//==============================================================================
BSTR CTMGrabCtrl::GetClassIdString() 
{
	CString strClassId = m_tmVersion.GetClsId();
	return strClassId.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::GetRegisteredPath()
//
// 	Description:	This method is called to get the ocx path stored in the
//					system registry.
//
// 	Returns:		The path to the ocx stored in the registry
//
//	Notes:			None
//
//==============================================================================
BSTR CTMGrabCtrl::GetRegisteredPath() 
{
	CString strRegistered = m_tmVersion.GetFileSpec();
	return strRegistered.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::GetRegistration()
//
// 	Description:	This function is called to get the control's registration
//					information.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMGrabCtrl::GetRegistration() 
{
	CLSID	clsid;

	//	Get the GUID 
	GetClassID(&clsid);

	//	Initialize the version information
	m_tmVersion.InitFromClsId("TMGrab", "Screen Capture Control", clsid);

}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::GetVerBuild()
//
// 	Description:	This method is called to get the value of the VerBuild
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMGrabCtrl::GetVerBuild() 
{
	return m_tmVersion.GetBuild();
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::GetVerBuildDate()
//
// 	Description:	This method is called to get the value of the VerBuildDate
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMGrabCtrl::GetVerBuildDate() 
{
	CString strBuildDate = m_tmVersion.GetBuildDate();
	return strBuildDate.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::GetVerMajor()
//
// 	Description:	This method is called to get the value of the VerMajor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMGrabCtrl::GetVerMajor() 
{
	return m_tmVersion.GetMajor();
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::GetVerMinor()
//
// 	Description:	This method is called to get the value of the VerMinor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMGrabCtrl::GetVerMinor() 
{
	return m_tmVersion.GetMinor();
}


//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::GetVerQEF()
//
// 	Description:	This method is called to get the value of the VerQEF
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMGrabCtrl::GetVerQEF() 
{
	return m_tmVersion.GetUpdate();
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::GetVerTextLong()
//
// 	Description:	This method is called to get the value of the VerTextLong
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMGrabCtrl::GetVerTextLong() 
{
	CString strVer = m_tmVersion.GetTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::GetVerTextShort()
//
// 	Description:	This method is called to get the value of the VerTextShort
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMGrabCtrl::GetVerTextShort() 
{
	CString strVer = m_tmVersion.GetShortTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::Initialize()
//
// 	Description:	This function will construct and initialize the lpenbar
//
// 	Returns:		TMDP_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMGrabCtrl::Initialize()
{
	SCaptureOptions Options;

	//	Don't bother if not in user mode
	if(!AmbientUserMode())
		return TMGRAB_ERROR_NONE;

	//	Initialize the error handler
	//
	//	NOTE:	Since this is an invisible control we have to wait to set the
	//			window handle until after we've created the frame window
	m_Errors.Enable(m_bEnableErrors);
	m_Errors.SetTitle("TMGrab Error");
	
	//	Initialize from the ini file if specified
	if(m_strIniFile.GetLength() > 0)
	{
		//	Attempt to open the file
		if(m_Ini.Open(m_strIniFile, ""))
		{
			//	Read the capture options
			m_Ini.ReadCaptureOptions(&Options);

			//	Set the property values
			m_sArea = Options.sArea;
			m_bSilent = Options.bSilent;
			m_sHotkey = Options.sHotkey;
			m_sCancelKey = Options.sCancelKey;
		}
		else
		{
			m_Errors.Handle("", IDS_OPEN_INI_FAILED, m_Ini.strFileSpec);
		}
	}

	//	Create the frame window
	if(!m_Frame.Create(this, &m_Errors))
		return TMGRAB_ERROR_INITIALIZE_FAILED;

	//	Initialize the frame window properties
	m_Frame.SetArea(m_sArea);
	m_Frame.SetSilent(m_bSilent);
	m_Frame.SetOneShot(m_bOneShot);
	m_Frame.SetHotkey(m_sHotkey);
	m_Frame.SetCancelKey(m_sCancelKey);

	return TMGRAB_ERROR_NONE;
}	

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::OnAreaChanged()
//
// 	Description:	This function is called when the Area property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMGrabCtrl::OnAreaChanged() 
{
	//	Notify the frame window if it exists
	if(IsWindow(m_Frame.m_hWnd))
		m_Frame.SetArea(m_sArea);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::OnCancelKeyChanged()
//
// 	Description:	This function is called when the CancelKey property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMGrabCtrl::OnCancelKeyChanged() 
{
	//	Notify the frame window if it exists
	if(IsWindow(m_Frame.m_hWnd))
		m_Frame.SetCancelKey(m_sCancelKey);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::OnCaptureImage()
//
// 	Description:	This function is called by the frame when a new capture
//					is available.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMGrabCtrl::OnCaptureImage() 
{
	//	Notify the container
	FireCaptured();
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::OnDraw()
//
// 	Description:	This function is called in response to WM_PAINT messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMGrabCtrl::OnDraw(CDC* pdc, const CRect& rcBounds,const CRect& rcInvalid)
{
	if(!AmbientUserMode())
	{
		CRect ControlRect = rcBounds;
		CString strText;

		strText.Format("FTI Screen Capture Control (rev. %d.%d)",
					   _wVerMajor, _wVerMinor);

		//	Paint the background
		pdc->FillRect(ControlRect,
				      CBrush::FromHandle((HBRUSH)GetStockObject(LTGRAY_BRUSH)));
		pdc->Draw3dRect(ControlRect, RGB(0x00,0x00,0x00), 
									 RGB(0xFF,0xFF,0xFF));

		pdc->SetBkMode(TRANSPARENT);
		pdc->SetTextColor(RGB(0x00,0x00,0x00));
		pdc->DrawText(strText, ControlRect, 
					  DT_CENTER | DT_VCENTER | DT_SINGLELINE); 
	}
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::OnEnableErrorsChanged()
//
// 	Description:	This function is called when the EnableErrors property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMGrabCtrl::OnEnableErrorsChanged() 
{
	m_Errors.Enable(m_bEnableErrors);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::OnGetDisplayString()
//
// 	Description:	This function is called by the framework to get the display
//					string for the specified property value.
//
// 	Returns:		TRUE if the string has been set
//
//	Notes:			None
//
//==============================================================================
BOOL CTMGrabCtrl::OnGetDisplayString(DISPID dispid, CString& strValue) 
{
	//	Is this the Area property?
	if(dispid == dispidArea)
	{
		switch(m_sArea)
		{
			case TMGRAB_AREA_FULL_SCREEN:	

				strValue = _T("0 - Full Screen");
				return TRUE;
			
			case TMGRAB_AREA_ACTIVE_WINDOW:	

				strValue = _T("1 - Active Window");
				return TRUE;
			
			case TMGRAB_AREA_SELECTION:

				strValue = _T("2 - User Selection");
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
// 	Function Name:	CTMGrabCtrl::OnGetPredefinedStrings()
//
// 	Description:	This function is called by the framework to get the display
//					strings associated with a property.
//
// 	Returns:		TRUE if values have been provided
//
//	Notes:			None
//
//==============================================================================
BOOL CTMGrabCtrl::OnGetPredefinedStrings(DISPID dispid, 
										  CStringArray* pStringArray, 
										  CDWordArray* pCookieArray) 
{
	BOOL bResult = FALSE;

	//	Is this the Area property?
	if(dispid == dispidArea)
	{
		TRY
		{
			// Fill in the values in pStringArray and pCookieArray
			CString Label(_T("0 - Full Screen"));
			pStringArray->Add(Label);
			pCookieArray->Add(TMGRAB_AREA_FULL_SCREEN);
        
			Label = _T("1 - Active Window");
			pStringArray->Add(Label);
			pCookieArray->Add(TMGRAB_AREA_ACTIVE_WINDOW);

			Label = _T("2 - User Selection");
			pStringArray->Add(Label);
			pCookieArray->Add(TMGRAB_AREA_SELECTION);

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
// 	Function Name:	CTMGrabCtrl::OnGetPredefinedValue()
//
// 	Description:	This function is called by the framework to get the variant
//					form of the specified property value.
//
// 	Returns:		TRUE if values have been provided
//
//	Notes:			None
//
//==============================================================================
BOOL CTMGrabCtrl::OnGetPredefinedValue(DISPID dispid, DWORD dwCookie, 
									    VARIANT* lpvarOut) 
{
	//	Is this the Area property?
	if(dispid == dispidArea)
	{
		VariantClear(lpvarOut);
		V_VT(lpvarOut) = VT_I2;
		V_I2(lpvarOut) = (short)dwCookie;
		return TRUE;
	}
	return COleControl::OnGetPredefinedValue(dispid, dwCookie, lpvarOut);
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::OnHotkeyChanged()
//
// 	Description:	This function is called when the Hotkey property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMGrabCtrl::OnHotkeyChanged() 
{
	//	Notify the frame window if it exists
	if(IsWindow(m_Frame.m_hWnd))
		m_Frame.SetHotkey(m_sHotkey);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::OnIniFileChanged()
//
// 	Description:	This function is called when the IniFile property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMGrabCtrl::OnIniFileChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::OnOneShotChanged()
//
// 	Description:	This function is called when the OneShot property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMGrabCtrl::OnOneShotChanged() 
{
	//	Notify the frame window if it exists
	if(IsWindow(m_Frame.m_hWnd))
		m_Frame.SetOneShot(m_bOneShot);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::OnSilentChanged()
//
// 	Description:	This function is called when the Silent property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMGrabCtrl::OnSilentChanged() 
{
	//	Notify the frame window if it exists
	if(IsWindow(m_Frame.m_hWnd))
		m_Frame.SetSilent(m_bSilent);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::Save()
//
// 	Description:	This method is called to save the current capture to file
//
// 	Returns:		TMGRAB_ERROR_NONE if successful
//
//	Notes:			None
//
//==============================================================================
short CTMGrabCtrl::Save(LPCTSTR pszName, short iFormat, short iBitsPerPixel, 
						short iQuality, short iModify) 
{
	//	Make sure the control has been initialized
	if(IsWindow(m_Frame.m_hWnd))
	{
		if(!m_Frame.Save(pszName, iFormat, iBitsPerPixel, iQuality, iModify))
		{
			return TMGRAB_ERROR_SAVE_FAILED;
		}
		else
		{
			return TMGRAB_ERROR_NONE;
		}
	}
	else
	{
		m_Errors.Handle(0, IDS_NOT_INITIALIZED);
		return TMGRAB_ERROR_NOT_INITIALIZED;
	}
}

//==============================================================================
//
// 	Function Name:	CTMGrabCtrl::Stop()
//
// 	Description:	This method is called to stop a capture session
//
// 	Returns:		TMGRAB_ERROR_NONE if successful
//
//	Notes:			None
//
//==============================================================================
short CTMGrabCtrl::Stop() 
{
	//	Make sure the control has been initialized
	if(IsWindow(m_Frame.m_hWnd))
	{
		if(!m_Frame.Stop())
		{
			//m_Errors.Handle(0, IDS_STOP_FAILED);
			return TMGRAB_ERROR_STOP_FAILED;
		}
		else
		{
			return TMGRAB_ERROR_NONE;
		}
	}
	else
	{
		m_Errors.Handle(0, IDS_NOT_INITIALIZED);
		return TMGRAB_ERROR_NOT_INITIALIZED;
	}
}


