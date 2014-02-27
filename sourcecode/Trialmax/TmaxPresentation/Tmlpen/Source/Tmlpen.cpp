//==============================================================================
//
// File Name:	tmlpen.cpp
//
// Description:	This file contains member functions of the CTMLpenCtrl class.
//
// See Also:	tmlpen.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	01-02-99	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmlpenap.h>
#include <tmlpen.h>
#include <tmlpenpg.h>
#include <tmlpdefs.h>
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
extern CTMLpenApp NEAR theApp;

/* Replace 2 */
const IID BASED_CODE IID_DTMLpen6 =
		{ 0x32470b00, 0x8288, 0x4443, { 0xaa, 0xe1, 0x42, 0x6d, 0xc9, 0x3f, 0xfa, 0xa5 } };
/* Replace 3 */
const IID BASED_CODE IID_DTMLpen6Events =
		{ 0x33d55c15, 0xf74, 0x45ec, { 0x80, 0xf9, 0xbf, 0x9a, 0x9c, 0xd3, 0xfa, 0x62 } };

// Control type information
static const DWORD BASED_CODE _dwTMLpenOleMisc =
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
BEGIN_MESSAGE_MAP(CTMLpenCtrl, COleControl)
	//{{AFX_MSG_MAP(CTMLpenCtrl)
	ON_WM_CREATE()
	ON_WM_LBUTTONUP()
	ON_WM_RBUTTONUP()
	//}}AFX_MSG_MAP
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
END_MESSAGE_MAP()

// Dispatch map
BEGIN_DISPATCH_MAP(CTMLpenCtrl, COleControl)
	//{{AFX_DISPATCH_MAP(CTMLpenCtrl)
	DISP_PROPERTY_EX(CTMLpenCtrl, "VerBuild", GetVerBuild, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMLpenCtrl, "VerMajor", GetVerMajor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMLpenCtrl, "VerMinor", GetVerMinor, SetNotSupported, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMLpenCtrl, "AutoInit", m_bAutoInit, OnAutoInitChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMLpenCtrl, "EnableErrors", m_bEnableErrors, OnEnableErrorsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMLpenCtrl, "AlwaysOnTop", m_bAlwaysOnTop, OnAlwaysOnTopChanged, VT_BOOL)
	DISP_PROPERTY_EX(CTMLpenCtrl, "VerTextLong", GetVerTextLong, SetNotSupported, VT_BSTR)
	DISP_FUNCTION(CTMLpenCtrl, "Initialize", Initialize, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMLpenCtrl, "GetRegisteredPath", GetRegisteredPath, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMLpenCtrl, "GetClassIdString", GetClassIdString, VT_BSTR, VTS_NONE)
	DISP_STOCKPROP_BACKCOLOR()
	DISP_STOCKPROP_FORECOLOR()
	DISP_STOCKPROP_APPEARANCE()
	DISP_STOCKPROP_BORDERSTYLE()
	//}}AFX_DISPATCH_MAP

	//	Added rev 6.1.0
	DISP_PROPERTY_EX_ID(CTMLpenCtrl, "VerTextShort", DISPID_VERTEXTSHORT, GetVerTextShort, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_EX_ID(CTMLpenCtrl, "VerQEF", DISPID_VERQEF, GetVerQEF, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX_ID(CTMLpenCtrl, "VerBuildDate", DISPID_VERBUILDDATE, GetVerBuildDate, SetNotSupported, VT_BSTR)

END_DISPATCH_MAP()

// Event map
BEGIN_EVENT_MAP(CTMLpenCtrl, COleControl)
	//{{AFX_EVENT_MAP(CTMLpenCtrl)
	EVENT_CUSTOM("MouseClick", FireMouseClick, VTS_I2  VTS_I2)
	//}}AFX_EVENT_MAP
END_EVENT_MAP()

// Property pages
BEGIN_PROPPAGEIDS(CTMLpenCtrl, 2)
	PROPPAGEID(CTMLpenProperties::guid)
	PROPPAGEID(CLSID_CColorPropPage)
END_PROPPAGEIDS(CTMLpenCtrl)

/* Replace 4 */
IMPLEMENT_OLECREATE_EX(CTMLpenCtrl, "TMLPEN6.TMLpenCtrl.1",
	0x7efcbdc0, 0xf749, 0x4574, 0x8d, 0xc1, 0x2e, 0x55, 0x75, 0xdd, 0x98, 0x8)

// Type library ID and version
IMPLEMENT_OLETYPELIB(CTMLpenCtrl, _tlid, _wVerMajor, _wVerMinor)
IMPLEMENT_OLECTLTYPE(CTMLpenCtrl, IDS_TMLPEN, _dwTMLpenOleMisc)

IMPLEMENT_DYNCREATE(CTMLpenCtrl, COleControl)

// Interface map for IObjectSafety
BEGIN_INTERFACE_MAP(CTMLpenCtrl, COleControl )
	INTERFACE_PART(CTMLpenCtrl, IID_IObjectSafety, ObjSafety)
END_INTERFACE_MAP()

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::CTMLpenCtrlFactory::UpdateRegistry
//
// 	Description:	Adds or removes system registry entries for this control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTMLpenCtrl::CTMLpenCtrlFactory::UpdateRegistry(BOOL bRegister)
{
	BOOL	bReturn;
	HRESULT	hResult;

	if(bRegister)
	{
		bReturn = AfxOleRegisterControlClass(AfxGetInstanceHandle(),
											 m_clsid,
											 m_lpszProgID,
											 IDS_TMLPEN,
											 IDB_TMLPEN,
											 afxRegApartmentThreading,
											 _dwTMLpenOleMisc,
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
// 	Function Name:	CTMLpenCtrl::XObjSafety::AddRef()
//
// 	Description:	This function is called to attach to the nested
//					IObjectSafety dispatch interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMLpenCtrl::XObjSafety::AddRef()
{
    METHOD_PROLOGUE(CTMLpenCtrl, ObjSafety)
    return pThis->ExternalAddRef();
}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::XObjSafety::GetInterfaceSafetyOptions()
//
// 	Description:	This function is called to get this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMLpenCtrl::XObjSafety::GetInterfaceSafetyOptions( 
		/* [in]  */ REFIID riid,
        /* [out] */ DWORD __RPC_FAR *pdwSupportedOptions,
        /* [out] */ DWORD __RPC_FAR *pdwEnabledOptions)
{
	HRESULT			hReturn;
	IUnknown FAR*	pInterface;

	METHOD_PROLOGUE(CTMLpenCtrl, ObjSafety)

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
// 	Function Name:	CTMLpenCtrl::XObjSafety::QueryInterface()
//
// 	Description:	This function is called to query the IObjectSafety interface
//					for the requested method/property.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT FAR EXPORT CTMLpenCtrl::XObjSafety::QueryInterface(REFIID iid, 
														  void FAR* FAR* ppvObj)
{
    METHOD_PROLOGUE(CTMLpenCtrl, ObjSafety)
    return (HRESULT)pThis->ExternalQueryInterface(&iid, ppvObj);
}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::XObjSafety::Release()
//
// 	Description:	This function is called to detach from the IObjectSafety
//					dispatch interface.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMLpenCtrl::XObjSafety::Release()
{
    METHOD_PROLOGUE(CTMLpenCtrl, ObjSafety)
    return pThis->ExternalRelease();
}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::XObjSafety::SetInterfaceSafetyOptions()
//
// 	Description:	This function is called to set this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMLpenCtrl::XObjSafety::SetInterfaceSafetyOptions( 
        /* [in] */ REFIID riid,
        /* [in] */ DWORD dwOptionSetMask,
        /* [in] */ DWORD dwEnabledOptions)
{
	IUnknown FAR* pInterface;

    METHOD_PROLOGUE(CTMLpenCtrl, ObjSafety)
	
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
// 	Function Name:	CTMLpenCtrl::CheckVersion()
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
BOOL CTMLpenCtrl::CheckVersion(DWORD dwVersion)
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
	              "TMLpen ActiveX control. You should upgrade tm_lpen6.ocx "
				  "as soon as possible\n\n%s", 
				  (wMinor > _wVerMinor) ? "a newer" : "an older", strVersion);

	MessageBeep(MB_ICONEXCLAMATION);
	MessageBox(strMsg, "TrialMax Error", MB_ICONEXCLAMATION | MB_OK);

	//	The versions do not match
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::CTMLpenCtrl()
//
// 	Description:	This is the constructor for CTMLpenCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMLpenCtrl::CTMLpenCtrl()
{
	InitializeIIDs(&IID_DTMLpen6, &IID_DTMLpen6Events);

	//	Get the registry information
	GetRegistration();
}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::~CTMLpenCtrl()
//
// 	Description:	This is the destructor for CTMLpenCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMLpenCtrl::~CTMLpenCtrl()
{
}		

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::DoPropertyExchange()
//
// 	Description:	This function manages the exchange of persistant 
//					properties
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLpenCtrl::DoPropExchange(CPropExchange* pPX)
{
	BOOL bAutoInit = FALSE;
	BOOL bEnableErrors = FALSE;
	BOOL bAlwaysOnTop = FALSE;

	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
	COleControl::DoPropExchange(pPX);

	//CheckVersion(pPX->GetVersion());

	try
	{
		//	Load the control's persistent properties
		bAutoInit = PX_Bool(pPX, _T("AutoInit"), m_bAutoInit, TMLPEN_AUTOINIT);
		bEnableErrors = PX_Bool(pPX, _T("EnableErrors"), m_bEnableErrors, TRUE);
		bAlwaysOnTop = PX_Bool(pPX, _T("AlwaysOnTop"), m_bAlwaysOnTop, TMLPEN_ALWAYSONTOP);
	}
	catch(...)
	{
		if(!bAutoInit) m_bAutoInit = TMLPEN_AUTOINIT;
		if(!bEnableErrors) m_bEnableErrors = TRUE;
		if(!bAlwaysOnTop) m_bAlwaysOnTop = TMLPEN_ALWAYSONTOP;
	}
}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::GetClassIdString()
//
// 	Description:	This method is called to get the class GUID as a null
//					terminated string.
//
// 	Returns:		The string equivalent of the CLASS GUID
//
//	Notes:			None
//
//==============================================================================
BSTR CTMLpenCtrl::GetClassIdString() 
{
	CString strClsId = m_tmVersion.GetClsId();
	return strClsId.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::GetKeyState()
//
// 	Description:	This function will check the keyboard to get the state of
//					the control keys
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
short CTMLpenCtrl::GetKeyState()
{
	short sKeyState = TMLPEN_NOKEYS;

	//	Is the shift key pressed?
	if(::GetKeyState(VK_SHIFT) & 0x8000)
		sKeyState |= TMLPEN_SHIFT;

	//	Is the control key pressed?
	if(::GetKeyState(VK_CONTROL) & 0x8000)
		sKeyState |= TMLPEN_CONTROL;

	//	Is the alt key pressed?
	if(::GetKeyState(VK_MENU) & 0x8000)
		sKeyState |= TMLPEN_ALT;

	return sKeyState;
}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::GetRegisteredPath()
//
// 	Description:	This method is called to get the ocx path stored in the
//					system registry.
//
// 	Returns:		The path to the ocx stored in the registry
//
//	Notes:			None
//
//==============================================================================
BSTR CTMLpenCtrl::GetRegisteredPath() 
{
	CString strRegistered = m_tmVersion.GetFileSpec();
	return strRegistered.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::GetRegistration()
//
// 	Description:	This function is called to get the control's registration
//					information.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLpenCtrl::GetRegistration() 
{
	CLSID	clsid;

	//	Get the GUID 
	GetClassID(&clsid);

	//	Initialize the version information
	m_tmVersion.InitFromClsId("TMLpen", "Light Pen Control", clsid);

}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::GetVerBuild()
//
// 	Description:	This method is called to get the value of the VerBuild
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMLpenCtrl::GetVerBuild() 
{
	return m_tmVersion.GetBuild();
}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::GetVerBuildDate()
//
// 	Description:	This method is called to get the value of the VerBuildDate
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMLpenCtrl::GetVerBuildDate() 
{
	CString strBuildDate = m_tmVersion.GetBuildDate();
	return strBuildDate.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::GetVerMajor()
//
// 	Description:	This method is called to get the value of the VerMajor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMLpenCtrl::GetVerMajor() 
{
	return m_tmVersion.GetMajor();
}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::GetVerMinor()
//
// 	Description:	This method is called to get the value of the VerMinor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMLpenCtrl::GetVerMinor() 
{
	return m_tmVersion.GetMinor();
}


//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::GetVerQEF()
//
// 	Description:	This method is called to get the value of the VerQEF
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMLpenCtrl::GetVerQEF() 
{
	return m_tmVersion.GetUpdate();
}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::GetVerTextLong()
//
// 	Description:	This method is called to get the value of the VerTextLong
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMLpenCtrl::GetVerTextLong() 
{
	CString strVer = m_tmVersion.GetTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::GetVerTextShort()
//
// 	Description:	This method is called to get the value of the VerTextShort
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMLpenCtrl::GetVerTextShort() 
{
	CString strVer = m_tmVersion.GetShortTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::Initialize()
//
// 	Description:	This function will construct and initialize the lpenbar
//
// 	Returns:		TMDP_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMLpenCtrl::Initialize()
{
	//	Don't bother if not in user mode
	if(!AmbientUserMode())
		return TMLP_NOERROR;

	//	Make sure the TopMost option is set if requested
	OnAlwaysOnTopChanged(); 

	return TMLP_NOERROR;
}	

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::OnAlwaysOnTopChanged()
//
// 	Description:	This function is called when the AlwaysOnTop property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLpenCtrl::OnAlwaysOnTopChanged() 
{
	if(AmbientUserMode())
	{
		if(m_bAlwaysOnTop)
			SetWindowPos(&CWnd::wndTopMost, 0, 0, 0, 0, 
						 SWP_NOREDRAW | SWP_NOSIZE | SWP_NOMOVE);
		else
			SetWindowPos(&CWnd::wndNoTopMost, 0, 0, 0, 0,
						 SWP_NOREDRAW | SWP_NOSIZE | SWP_NOMOVE);
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::OnAutoInitChanged()
//
// 	Description:	This function is called when the AutoInit property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLpenCtrl::OnAutoInitChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::OnCreate()
//
// 	Description:	This fuction handles all WM_CREATE messages
//
// 	Returns:		0 if successful
//
//	Notes:			None
//
//==============================================================================
int CTMLpenCtrl::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
	//	Do the base class processing first
	if(COleControl::OnCreate(lpCreateStruct) == -1)
		return -1;
	
	//	Initialize the control
	if(m_bAutoInit)
		Initialize();
	
	return 0;
}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::OnDraw()
//
// 	Description:	This function is called in response to WM_PAINT messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLpenCtrl::OnDraw(CDC* pdc, const CRect& rcBounds,const CRect& rcInvalid)
{
	CBrush brBackground;

	//	Create a brush using the background color
	brBackground.CreateSolidBrush(TranslateColor(GetBackColor()));

	// Paint the control in the background color
	pdc->FillRect(rcBounds, &brBackground);
}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::OnEnableErrorsChanged()
//
// 	Description:	This function is called when the EnableErrors property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLpenCtrl::OnEnableErrorsChanged() 
{
	m_Errors.Enable(m_bEnableErrors);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::OnLButtonUp()
//
// 	Description:	This function traps WM_LBUTTONUP messages 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLpenCtrl::OnLButtonUp(UINT nFlags, CPoint point) 
{
	FireMouseClick(TMLPEN_LEFT, GetKeyState());
	COleControl::OnLButtonUp(nFlags, point);
}

//==============================================================================
//
// 	Function Name:	CTMLpenCtrl::OnRButtonUp()
//
// 	Description:	This function traps WM_RBUTTONUP messages 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMLpenCtrl::OnRButtonUp(UINT nFlags, CPoint point) 
{
	FireMouseClick(TMLPEN_RIGHT, GetKeyState());
	COleControl::OnRButtonUp(nFlags, point);
}

