//==============================================================================
//
// File Name:	browsectl.cpp
//
// Description:	This file contains member functions of the CTMBrowseCtrl class.
//
// See Also:	browsectl.h
//
// Copyright	FTI Consulting 1997-2002
//
//==============================================================================
//	Date		Revision    Description
//	02-09-02	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <browseapp.h>
#include <browsectl.h>
#include <browsepg.h>
#include <tmbrowsedef.h>
#include <regcats.h>
#include <webframe.h>
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
extern CTMBrowseApp NEAR	theApp;

// Interface IDs

/* Replace 2 */
const IID BASED_CODE IID_DTMBrowse6 =
		{ 0x8d68471b, 0xf666, 0x42d4, { 0x8e, 0xe8, 0xa7, 0x2d, 0xd2, 0xb5, 0xea, 0xe5 } };
/* Replace 3 */
const IID BASED_CODE IID_DTMBrowse6Events =
		{ 0x9021da4c, 0xe3ea, 0x4ccc, { 0xa3, 0x9d, 0x7b, 0x50, 0x22, 0xfc, 0x52, 0xf5 } };

// Control type information
static const DWORD BASED_CODE _dwTMBrowseOleMisc =
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
BEGIN_MESSAGE_MAP(CTMBrowseCtrl, COleControl)
	//{{AFX_MSG_MAP(CTMBrowseCtrl)
	ON_WM_CREATE()
	ON_WM_SIZE()
	//}}AFX_MSG_MAP
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
END_MESSAGE_MAP()

// Dispatch map
BEGIN_DISPATCH_MAP(CTMBrowseCtrl, COleControl)
	//{{AFX_DISPATCH_MAP(CTMBrowseCtrl)
	DISP_PROPERTY_NOTIFY(CTMBrowseCtrl, "AutoInit", m_bAutoInit, OnAutoInitChanged, VT_BOOL)
	DISP_PROPERTY_EX(CTMBrowseCtrl, "VerBuild", GetVerBuild, SetNotSupported, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMBrowseCtrl, "EnableErrors", m_bEnableErrors, OnEnableErrorsChanged, VT_BOOL)
	DISP_PROPERTY_EX(CTMBrowseCtrl, "VerMajor", GetVerMajor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMBrowseCtrl, "VerMinor", GetVerMinor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMBrowseCtrl, "VerTextLong", GetVerTextLong, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMBrowseCtrl, "IniFile", m_strIniFile, OnIniFileChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMBrowseCtrl, "Filename", m_strFilename, OnFilenameChanged, VT_BSTR)
	DISP_FUNCTION(CTMBrowseCtrl, "GetRegisteredPath", GetRegisteredPath, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMBrowseCtrl, "GetClassIdString", GetClassIdString, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMBrowseCtrl, "Initialize", Initialize, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMBrowseCtrl, "Load", Load, VT_I2, VTS_BSTR)
	DISP_STOCKPROP_BACKCOLOR()
	DISP_STOCKPROP_BORDERSTYLE()
	DISP_STOCKPROP_FORECOLOR()
	DISP_STOCKPROP_HWND()
	//}}AFX_DISPATCH_MAP

	//	Added rev 6.1.0
	DISP_PROPERTY_EX_ID(CTMBrowseCtrl, "VerTextShort", DISPID_VERTEXTSHORT, GetVerTextShort, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_EX_ID(CTMBrowseCtrl, "VerQEF", DISPID_VERQEF, GetVerQEF, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX_ID(CTMBrowseCtrl, "VerBuildDate", DISPID_VERBUILDDATE, GetVerBuildDate, SetNotSupported, VT_BSTR)

	DISP_FUNCTION_ID(CTMBrowseCtrl, "AboutBox", DISPID_ABOUTBOX, AboutBox, VT_EMPTY, VTS_NONE)

END_DISPATCH_MAP()

BEGIN_EVENT_MAP(CTMBrowseCtrl, COleControl)
	//{{AFX_EVENT_MAP(CTMBrowseCtrl)
	EVENT_CUSTOM("LoadComplete", FireLoadComplete, VTS_BSTR)
	//}}AFX_EVENT_MAP
END_EVENT_MAP()

// Property pages
BEGIN_PROPPAGEIDS(CTMBrowseCtrl, 2)
	PROPPAGEID(CTMBrowseProperties::guid)
	PROPPAGEID(CLSID_CColorPropPage)
END_PROPPAGEIDS(CTMBrowseCtrl)

// Initialize class factory and guid
/* Replace 4 */
IMPLEMENT_OLECREATE_EX(CTMBrowseCtrl, "TMBROWSE6.TMBrowseCtrl.1",
	0x1b964711, 0x19a0, 0x4696, 0x94, 0x89, 0, 0x88, 0x29, 0xd8, 0x7d, 0x7e)

// Type library ID and version
IMPLEMENT_OLETYPELIB(CTMBrowseCtrl, _tlid, _wVerMajor, _wVerMinor)
IMPLEMENT_OLECTLTYPE(CTMBrowseCtrl, IDS_TMBROWSE, _dwTMBrowseOleMisc)

IMPLEMENT_DYNCREATE(CTMBrowseCtrl, COleControl)

// Interface map for IObjectSafety
BEGIN_INTERFACE_MAP(CTMBrowseCtrl, COleControl )
	INTERFACE_PART(CTMBrowseCtrl, IID_IObjectSafety, ObjSafety)
END_INTERFACE_MAP()

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::CTMBrowseCtrlFactory::UpdateRegistry
//
// 	Description:	Adds or removes system registry entries for this control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTMBrowseCtrl::CTMBrowseCtrlFactory::UpdateRegistry(BOOL bRegister)
{
	BOOL	bReturn;
	HRESULT	hResult;

	if(bRegister)
	{
		bReturn = AfxOleRegisterControlClass(AfxGetInstanceHandle(),
											 m_clsid,
											 m_lpszProgID,
											 IDS_TMBROWSE,
											 IDB_TMBROWSE,
											 afxRegApartmentThreading,
											 _dwTMBrowseOleMisc,
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
// 	Function Name:	CTMBrowseCtrl::XObjSafety::AddRef()
//
// 	Description:	This function is called to attach to the nested
//					IObjectSafety dispatch interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMBrowseCtrl::XObjSafety::AddRef()
{
    METHOD_PROLOGUE(CTMBrowseCtrl, ObjSafety)
    return pThis->ExternalAddRef();
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::XObjSafety::GetInterfaceSafetyOptions()
//
// 	Description:	This function is called to get this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMBrowseCtrl::XObjSafety::GetInterfaceSafetyOptions( 
		/* [in]  */ REFIID riid,
        /* [out] */ DWORD __RPC_FAR *pdwSupportedOptions,
        /* [out] */ DWORD __RPC_FAR *pdwEnabledOptions)
{
	HRESULT			hReturn;
	IUnknown FAR*	pInterface;

	METHOD_PROLOGUE(CTMBrowseCtrl, ObjSafety)

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
// 	Function Name:	CTMBrowseCtrl::XObjSafety::QueryInterface()
//
// 	Description:	This function is called to query the IObjectSafety interface
//					for the requested method/property.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT FAR EXPORT CTMBrowseCtrl::XObjSafety::QueryInterface(REFIID iid, 
														  void FAR* FAR* ppvObj)
{
    METHOD_PROLOGUE(CTMBrowseCtrl, ObjSafety)
    return (HRESULT)pThis->ExternalQueryInterface(&iid, ppvObj);
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::XObjSafety::Release()
//
// 	Description:	This function is called to detach from the IObjectSafety
//					dispatch interface.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMBrowseCtrl::XObjSafety::Release()
{
    METHOD_PROLOGUE(CTMBrowseCtrl, ObjSafety)
    return pThis->ExternalRelease();
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::XObjSafety::SetInterfaceSafetyOptions()
//
// 	Description:	This function is called to set this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMBrowseCtrl::XObjSafety::SetInterfaceSafetyOptions( 
        /* [in] */ REFIID riid,
        /* [in] */ DWORD dwOptionSetMask,
        /* [in] */ DWORD dwEnabledOptions)
{
	IUnknown FAR* pInterface;

    METHOD_PROLOGUE(CTMBrowseCtrl, ObjSafety)
	
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
// 	Function Name:	CTMBrowseCtrl::AboutBox()
//
// 	Description:	This function will display the control's about box.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBrowseCtrl::AboutBox()
{
	CDialog dlgAbout(IDD_ABOUTBOX_TMBROWSE, this);
	dlgAbout.DoModal();
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::CTMBrowseCtrl()
//
// 	Description:	This is the constructor for CTMBrowseCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMBrowseCtrl::CTMBrowseCtrl()
{
	InitializeIIDs(&IID_DTMBrowse6, &IID_DTMBrowse6Events);

	//	Initialize the local data
	m_pFrame = 0;

	//	Get the registry information
	GetRegistration();
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::~CTMBrowseCtrl()
//
// 	Description:	This is the destructor for CTMBrowseCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMBrowseCtrl::~CTMBrowseCtrl()
{
	if(m_pFrame != 0)
	{
		delete m_pFrame;
		m_pFrame = 0;
	}
}		

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::DoPropertyExchange()
//
// 	Description:	This function manages the exchange of persistant 
//					properties
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBrowseCtrl::DoPropExchange(CPropExchange* pPX)
{
	BOOL bAutoInit = FALSE;
	BOOL bEnableErrors = FALSE;
	BOOL bIniFile = FALSE;
	BOOL bFilename = FALSE;

	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
	COleControl::DoPropExchange(pPX);

	try
	{
		//	Load the control's persistent properties
		bAutoInit = PX_Bool(pPX, _T("AutoInit"), m_bAutoInit, TMBROWSE_DEFAULT_AUTO_INIT);
		bEnableErrors = PX_Bool(pPX, _T("EnableErrors"), m_bEnableErrors, TMBROWSE_DEFAULT_ENABLE_ERRORS);
		bIniFile = PX_String(pPX, _T("IniFile"), m_strIniFile, TMBROWSE_DEFAULT_INI_FILE);
		bFilename = PX_String(pPX, _T("Filename"), m_strFilename, TMBROWSE_DEFAULT_FILENAME);
	}
	catch(...)
	{
		if(!bAutoInit) m_bAutoInit = TMBROWSE_DEFAULT_AUTO_INIT;
		if(!bEnableErrors) m_bEnableErrors = TMBROWSE_DEFAULT_ENABLE_ERRORS;
		if(!bIniFile) m_strIniFile = TMBROWSE_DEFAULT_INI_FILE;
		if(!bFilename) m_strFilename = TMBROWSE_DEFAULT_FILENAME;
	}
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::FindFile()
//
// 	Description:	This function checks to see if the specified file exists.
//
// 	Returns:		TRUE if found
//
//	Notes:			None
//
//==============================================================================
BOOL CTMBrowseCtrl::FindFile(LPCSTR lpszFilename)
{
	WIN32_FIND_DATA	Find;
	HANDLE			hFind;

	ASSERT(lpszFilename);

	if((hFind = FindFirstFile(lpszFilename, &Find)) == INVALID_HANDLE_VALUE)
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
// 	Function Name:	CTMBrowseCtrl::GetClassIdString()
//
// 	Description:	This method is called to get the class GUID as a null
//					terminated string.
//
// 	Returns:		The string equivalent of the CLASS GUID
//
//	Notes:			None
//
//==============================================================================
BSTR CTMBrowseCtrl::GetClassIdString() 
{
	CString strClsId = m_tmVersion.GetClsId();
	return strClsId.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::GetRegisteredPath()
//
// 	Description:	This method is called to get the ocx path stored in the
//					system registry.
//
// 	Returns:		The path to the ocx stored in the registry
//
//	Notes:			None
//
//==============================================================================
BSTR CTMBrowseCtrl::GetRegisteredPath() 
{
	CString strRegistered = m_tmVersion.GetFileSpec();
	return strRegistered.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::GetRegistration()
//
// 	Description:	This function is called to get the control's registration
//					information.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBrowseCtrl::GetRegistration() 
{
	CLSID	clsid;

	//	Get the GUID 
	GetClassID(&clsid);

	//	Initialize the version information
	m_tmVersion.InitFromClsId("TMSetup", "TmaxPresentation Setup", clsid);

}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::GetVerBuild()
//
// 	Description:	This method is called to get the value of the VerBuild
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMBrowseCtrl::GetVerBuild() 
{
	return m_tmVersion.GetBuild();
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::GetVerBuildDate()
//
// 	Description:	This method is called to get the value of the VerBuildDate
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMBrowseCtrl::GetVerBuildDate() 
{
	CString strBuildDate = m_tmVersion.GetBuildDate();
	return strBuildDate.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::GetVerMajor()
//
// 	Description:	This method is called to get the value of the VerMajor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMBrowseCtrl::GetVerMajor() 
{
	return m_tmVersion.GetMajor();
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::GetVerMinor()
//
// 	Description:	This method is called to get the value of the VerMinor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMBrowseCtrl::GetVerMinor() 
{
	return m_tmVersion.GetMinor();
}


//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::GetVerQEF()
//
// 	Description:	This method is called to get the value of the VerQEF
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMBrowseCtrl::GetVerQEF() 
{
	return m_tmVersion.GetUpdate();
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::GetVerTextLong()
//
// 	Description:	This method is called to get the value of the VerTextLong
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMBrowseCtrl::GetVerTextLong() 
{
	CString strVer = m_tmVersion.GetTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::GetVerTextShort()
//
// 	Description:	This method is called to get the value of the VerTextShort
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMBrowseCtrl::GetVerTextShort() 
{
	CString strVer = m_tmVersion.GetShortTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::Initialize()
//
// 	Description:	This function will construct and initialize the lpenbar
//
// 	Returns:		TMDP_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMBrowseCtrl::Initialize()
{
	//	Don't bother if not in user mode
	if(!AmbientUserMode())
		return TMBROWSE_NOERROR;

	//	Is the control already initialized?
	if((m_pFrame != 0) && IsWindow(m_pFrame->m_hWnd))
		return TMBROWSE_NOERROR;

	//	Allocate the frame window
	if(m_pFrame)
		delete m_pFrame;
	m_pFrame = new CWebFrame(this, &m_Errors);
	ASSERT(m_pFrame);

	//	Create the frame window
	if(!m_pFrame->Create())
	{
		m_Errors.Handle(0, IDS_FRAMEFAILED);
		delete m_pFrame;
		m_pFrame = 0;
		return TMBROWSE_FRAMEFAILED;
	}

	//	Set the background color of the frame to match the background color of
	//	the control
	m_pFrame->SetBackgroundColor(TranslateColor(GetBackColor()));

	//	Make sure the browser is properly sized and positioned
	RecalcLayout();

	return TMBROWSE_NOERROR;
}	

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::Load()
//
// 	Description:	This function is called to load the specified file.
//
// 	Returns:		TMBROWSE_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMBrowseCtrl::Load(LPCTSTR lpszFilename) 
{
	//	Does the browser window exist?
	if((m_pFrame != 0) && IsWindow(m_pFrame->m_hWnd))
	{
		//	Are we attempting to load a new file?
		if((lpszFilename != 0) && (lstrlen(lpszFilename) > 0))
		{
			//	Verify that the file exists before loading the browser
			if(FindFile(lpszFilename) == FALSE)
			{
				m_Errors.Handle(0, IDS_FILENOTFOUND, lpszFilename);
				return TMBROWSE_FILENOTFOUND;
			}
			else
			{
				return m_pFrame->Load(lpszFilename);
			}
		}
		else
		{
			//	Unload the current content
			m_pFrame->Unload();
		}
	}

	return TMBROWSE_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::OnAutoInitChanged()
//
// 	Description:	This function is called when the AutoInit property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBrowseCtrl::OnAutoInitChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::OnBackColorChanged()
//
// 	Description:	This function is called when the BackColor property changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBrowseCtrl::OnBackColorChanged() 
{
	COleControl::OnBackColorChanged();

	//	Notify the frame window
	if(m_pFrame)
		m_pFrame->SetBackgroundColor(TranslateColor(GetBackColor()));
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::OnCreate()
//
// 	Description:	This fuction handles all WM_CREATE messages
//
// 	Returns:		0 if successful
//
//	Notes:			None
//
//==============================================================================
int CTMBrowseCtrl::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
	//	Do the base class processing first
	if(COleControl::OnCreate(lpCreateStruct) == -1)
		return -1;

	//	Initialize the error handler
	m_Errors.Enable(m_bEnableErrors);
	m_Errors.SetParent(m_hWnd);
	m_Errors.SetTitle("TMBrowse Error");
	
	//	Initialize the control
	if(m_bAutoInit)
		Initialize();
	
	return 0;
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::OnDraw()
//
// 	Description:	This function is called in response to WM_PAINT messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBrowseCtrl::OnDraw(CDC* pdc, const CRect& rcBounds,const CRect& rcInvalid)
{
	CBrush	brBackground;
	CString	strVersion;
	CRect	ControlRect = rcBounds;

	//	Paint the background
	brBackground.CreateSolidBrush(TranslateColor(GetBackColor()));
	
	if(AmbientUserMode())
	{
		//	Is the browser window available?
		if((m_pFrame != 0) && IsWindow(m_pFrame->m_hWnd))
		{
			m_pFrame->Redraw();
		}
		else
		{
			pdc->FillRect(&ControlRect, &brBackground);
		}
	}
	else
	{
		strVersion.Format("FTI Browse Template (rev. %d.%d)", 
						  _wVerMajor, _wVerMinor);
		
		pdc->SetBkMode(TRANSPARENT);
		pdc->SetTextColor(TranslateColor(GetForeColor()));
		
		pdc->FillRect(ControlRect, &brBackground);

		pdc->DrawText(strVersion, ControlRect, 
					  DT_CENTER | DT_NOCLIP | DT_VCENTER | DT_SINGLELINE); 
	}
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::OnEnableErrorsChanged()
//
// 	Description:	This function is called when the EnableErrors property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBrowseCtrl::OnEnableErrorsChanged() 
{
	m_Errors.Enable(m_bEnableErrors);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::OnFilenameChanged()
//
// 	Description:	This function is called when the Filename property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBrowseCtrl::OnFilenameChanged() 
{
	//	Are we in user mode?
	if(AmbientUserMode())
	{
		//	Load the new file
		Load(m_strFilename);
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::OnIniFileChanged()
//
// 	Description:	This function is called when the IniFile property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBrowseCtrl::OnIniFileChanged() 
{
	//	Are we in user mode?
	if(AmbientUserMode())
	{
		//	Open the ini file
		m_Ini.Open(m_strIniFile);

	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::OnSize()
//
// 	Description:	This function traps the WM_SIZE messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBrowseCtrl::OnSize(UINT nType, int cx, int cy) 
{
	//	Do the default processing
	COleControl::OnSize(nType, cx, cy);
	
	//	Resize the browser window
	RecalcLayout();
}

//==============================================================================
//
// 	Function Name:	CTMBrowseCtrl::RecalcLayout()
//
// 	Description:	This function is called to set the size and position of the
//					browser window within the available client area
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBrowseCtrl::RecalcLayout() 
{
	RECT rcMax;

	//	Don't bother if the browser window has not yet been created
	if((m_pFrame != 0) && IsWindow(m_pFrame->m_hWnd))
	{
		//	Get the maximum available area
		GetClientRect(&rcMax);

		//	Resize to use the full client area
		m_pFrame->MoveWindow(&rcMax);
	}
}


