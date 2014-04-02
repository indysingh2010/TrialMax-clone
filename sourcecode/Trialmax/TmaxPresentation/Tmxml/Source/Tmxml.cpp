//==============================================================================
//
// File Name:	tmxml.cpp
//
// Description:	This file contains member functions of the CTMXmlCtrl class.
//
// See Also:	tmxml.h
//
// Copyright	FTI Consulting 2001
//
//==============================================================================
//	Date		Revision    Description
//	03-02-01	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmxmlap.h>
#include <tmxml.h>
#include <tmxmlpg.h>
#include <tmxmdefs.h>
#include <xmlframe.h>
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
extern CTMXmlApp NEAR	theApp;
CString					theClsId;
CString					thePath;

/* Replace 2 */
const IID BASED_CODE IID_DTMXml6 =
		{ 0x11553d25, 0x5fae, 0x4c65, { 0x85, 0xe7, 0x83, 0x68, 0xb4, 0x51, 0xe0, 0x24 } };
/* Replace 3 */
const IID BASED_CODE IID_DTMXml6Events =
		{ 0x4e3dcb8a, 0x7422, 0x4ff7, { 0x96, 0x6, 0x22, 0x23, 0x96, 0x74, 0x5d, 0x5a } };

// Control type information
static const DWORD BASED_CODE _dwTMXml6OleMisc =
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
BEGIN_MESSAGE_MAP(CTMXmlCtrl, COleControl)
	//{{AFX_MSG_MAP(CTMXmlCtrl)
	ON_WM_CREATE()
	ON_WM_SIZE()
	ON_WM_TIMER()
	//}}AFX_MSG_MAP
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
END_MESSAGE_MAP()

// Dispatch map
BEGIN_DISPATCH_MAP(CTMXmlCtrl, COleControl)
	//{{AFX_DISPATCH_MAP(CTMXmlCtrl)
	DISP_PROPERTY_EX(CTMXmlCtrl, "VerBuild", GetVerBuild, SetNotSupported, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMXmlCtrl, "EnableErrors", m_bEnableErrors, OnEnableErrorsChanged, VT_BOOL)
	DISP_PROPERTY_EX(CTMXmlCtrl, "VerMajor", GetVerMajor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMXmlCtrl, "VerMinor", GetVerMinor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMXmlCtrl, "VerTextLong", GetVerTextLong, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMXmlCtrl, "AutoInit", m_bAutoInit, OnAutoInitChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMXmlCtrl, "Filename", m_strFilename, OnFilenameChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMXmlCtrl, "FloatPrintProgress", m_bFloatPrintProgress, OnFloatPrintProgressChanged, VT_BOOL)
	DISP_FUNCTION(CTMXmlCtrl, "Initialize", Initialize, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMXmlCtrl, "LoadFile", LoadFile, VT_I2, VTS_BSTR)
	DISP_FUNCTION(CTMXmlCtrl, "GetClassIdString", GetClassIdString, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMXmlCtrl, "GetRegisteredPath", GetRegisteredPath, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMXmlCtrl, "loadDocument", loadDocument, VT_I2, VTS_BSTR)
	DISP_FUNCTION(CTMXmlCtrl, "jumpToPage", jumpToPage, VT_I2, VTS_BSTR)
	DISP_STOCKPROP_BACKCOLOR()
	DISP_STOCKPROP_FORECOLOR()
	//}}AFX_DISPATCH_MAP

	DISP_FUNCTION_ID(CTMXmlCtrl, "AboutBox", DISPID_ABOUTBOX, AboutBox, VT_EMPTY, VTS_NONE)

	//	Added rev 6.1.0
	DISP_PROPERTY_EX_ID(CTMXmlCtrl, "VerTextShort", DISPID_VERTEXTSHORT, GetVerTextShort, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_EX_ID(CTMXmlCtrl, "VerQEF", DISPID_VERQEF, GetVerQEF, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX_ID(CTMXmlCtrl, "VerBuildDate", DISPID_VERBUILDDATE, GetVerBuildDate, SetNotSupported, VT_BSTR)

END_DISPATCH_MAP()

// Event map
BEGIN_EVENT_MAP(CTMXmlCtrl, COleControl)
	//{{AFX_EVENT_MAP(CTMXmlCtrl)
	// NOTE - ClassWizard will add and remove event map entries
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_EVENT_MAP
END_EVENT_MAP()

// Property pages
BEGIN_PROPPAGEIDS(CTMXmlCtrl, 2)
	PROPPAGEID(CTMXmlProperties::guid)
	PROPPAGEID(CLSID_CColorPropPage)
END_PROPPAGEIDS(CTMXmlCtrl)

/* Replace 4 */
IMPLEMENT_OLECREATE_EX(CTMXmlCtrl, "TMXML6.TMXml6Ctrl.1",
	0x6a8f7fe8, 0x265a, 0x431b, 0xab, 0x92, 0xa3, 0x66, 0x18, 0x48, 0xd4, 0xa1)

// Type library ID and version
IMPLEMENT_OLETYPELIB(CTMXmlCtrl, _tlid, _wVerMajor, _wVerMinor)
IMPLEMENT_OLECTLTYPE(CTMXmlCtrl, IDS_TMXML, _dwTMXml6OleMisc)

IMPLEMENT_DYNCREATE(CTMXmlCtrl, COleControl)

// Interface map for IObjectSafety
BEGIN_INTERFACE_MAP(CTMXmlCtrl, COleControl )
	INTERFACE_PART(CTMXmlCtrl, IID_IObjectSafety, ObjSafety)
END_INTERFACE_MAP()

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::CTMXmlCtrlFactory::UpdateRegistry
//
// 	Description:	Adds or removes system registry entries for this control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTMXmlCtrl::CTMXmlCtrlFactory::UpdateRegistry(BOOL bRegister)
{
	BOOL	bReturn;
	HRESULT	hResult;

	if(bRegister)
	{
		bReturn = AfxOleRegisterControlClass(AfxGetInstanceHandle(),
											 m_clsid,
											 m_lpszProgID,
											 IDS_TMXML,
											 IDB_TMXML,
											 afxRegApartmentThreading,
											 _dwTMXml6OleMisc,
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
// 	Function Name:	CTMXmlCtrl::XObjSafety::AddRef()
//
// 	Description:	This function is called to attach to the nested
//					IObjectSafety dispatch interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMXmlCtrl::XObjSafety::AddRef()
{
    METHOD_PROLOGUE(CTMXmlCtrl, ObjSafety)
    return pThis->ExternalAddRef();
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::XObjSafety::GetInterfaceSafetyOptions()
//
// 	Description:	This function is called to get this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMXmlCtrl::XObjSafety::GetInterfaceSafetyOptions( 
		/* [in]  */ REFIID riid,
        /* [out] */ DWORD __RPC_FAR *pdwSupportedOptions,
        /* [out] */ DWORD __RPC_FAR *pdwEnabledOptions)
{
	HRESULT			hReturn;
	IUnknown FAR*	pInterface;

	METHOD_PROLOGUE(CTMXmlCtrl, ObjSafety)

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
// 	Function Name:	CTMXmlCtrl::XObjSafety::QueryInterface()
//
// 	Description:	This function is called to query the IObjectSafety interface
//					for the requested method/property.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT FAR EXPORT CTMXmlCtrl::XObjSafety::QueryInterface(REFIID iid, 
														  void FAR* FAR* ppvObj)
{
    METHOD_PROLOGUE(CTMXmlCtrl, ObjSafety)
    return (HRESULT)pThis->ExternalQueryInterface(&iid, ppvObj);
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::XObjSafety::Release()
//
// 	Description:	This function is called to detach from the IObjectSafety
//					dispatch interface.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMXmlCtrl::XObjSafety::Release()
{
    METHOD_PROLOGUE(CTMXmlCtrl, ObjSafety)
    return pThis->ExternalRelease();
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::XObjSafety::SetInterfaceSafetyOptions()
//
// 	Description:	This function is called to set this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMXmlCtrl::XObjSafety::SetInterfaceSafetyOptions( 
        /* [in] */ REFIID riid,
        /* [in] */ DWORD dwOptionSetMask,
        /* [in] */ DWORD dwEnabledOptions)
{
	IUnknown FAR* pInterface;

    METHOD_PROLOGUE(CTMXmlCtrl, ObjSafety)
	
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
// 	Function Name:	CTMXmlCtrl::AboutBox()
//
// 	Description:	This function is called to display the about box.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMXmlCtrl::AboutBox()
{
	CDialog dlgAbout(IDD_ABOUTBOX_TMXML);
	dlgAbout.DoModal();
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::CheckVersion()
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
BOOL CTMXmlCtrl::CheckVersion(DWORD dwVersion)
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
	              "TMXml ActiveX control. You should upgrade tm_xml6.ocx "
				  "as soon as possible\n\n%s", 
				  (wMinor > _wVerMinor) ? "a newer" : "an older", strVersion);

	MessageBeep(MB_ICONEXCLAMATION);
	MessageBox(strMsg, "Tmxml Error", MB_ICONEXCLAMATION | MB_OK);

	//	The versions do not match
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::CTMXmlCtrl()
//
// 	Description:	This is the constructor for CTMXmlCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMXmlCtrl::CTMXmlCtrl()
{
	InitializeIIDs(&IID_DTMXml6, &IID_DTMXml6Events);

	//	Initialize the local data
	m_pFrame	= 0;
	m_strSource.Empty();
	m_strFolder.Empty();

	//	Get the registry information
	GetRegistration();
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::~CTMXmlCtrl()
//
// 	Description:	This is the destructor for CTMXmlCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMXmlCtrl::~CTMXmlCtrl()
{
	//	Delete the frame if it exists
	if(m_pFrame)
	{
		delete m_pFrame;
		m_pFrame = 0;
	}
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::DoPropertyExchange()
//
// 	Description:	This function manages the exchange of persistant 
//					properties
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMXmlCtrl::DoPropExchange(CPropExchange* pPX)
{
	BOOL	bAutoInit = FALSE;
	BOOL	bEnableErrors = FALSE;
	BOOL	bFilename = FALSE;
	BOOL	bFloatPrintProgress = FALSE;

	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
	COleControl::DoPropExchange(pPX);

	//CheckVersion(pPX->GetVersion());

	try
	{
		//	Load the control's persistent properties
		bAutoInit = PX_Bool(pPX, _T("AutoInit"), m_bAutoInit, TMXML_AUTOINIT);
		bEnableErrors = PX_Bool(pPX, _T("EnableErrors"), m_bEnableErrors, TMXML_ENABLEERRORS);
		bFilename = PX_String(pPX, _T("Filename"), m_strFilename, TMXML_FILENAME);
		bFloatPrintProgress = PX_Bool(pPX, _T("FloatPrintProgress"), m_bFloatPrintProgress, TMXML_FLOAT_PRINT_PROGRESS);
	}
	catch(...)
	{
		if(!bAutoInit) m_bAutoInit = TMXML_AUTOINIT;
		if(!bEnableErrors) m_bEnableErrors = TMXML_ENABLEERRORS;
		if(!bFilename) m_strFilename = TMXML_FILENAME;
		if(!bFloatPrintProgress) m_bFloatPrintProgress = TMXML_FLOAT_PRINT_PROGRESS;
	}

	//	See if the SRC property has been set by linking to a web page
	try
	{
		PX_String(pPX, "SRC", m_strSource);
	}
	catch(...)
	{
		m_strSource.Empty();
	}
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::GetClassIdString()
//
// 	Description:	This method is called to get the class GUID as a null
//					terminated string.
//
// 	Returns:		The string equivalent of the CLASS GUID
//
//	Notes:			None
//
//==============================================================================
BSTR CTMXmlCtrl::GetClassIdString() 
{
	CString strClassId = m_tmVersion.GetClsId();
	return strClassId.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::GetRegisteredPath()
//
// 	Description:	This method is called to get the ocx path stored in the
//					system registry.
//
// 	Returns:		The path to the ocx stored in the registry
//
//	Notes:			None
//
//==============================================================================
BSTR CTMXmlCtrl::GetRegisteredPath() 
{
	CString strRegistered = m_tmVersion.GetFileSpec();
	return strRegistered.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::GetRegistration()
//
// 	Description:	This function is called to get the control's registration
//					information.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMXmlCtrl::GetRegistration() 
{
	CLSID	clsid;

	//	Get the GUID 
	GetClassID(&clsid);

	//	Initialize the version information
	m_tmVersion.InitFromClsId("TMXml", "XML Document Viewer", clsid);

}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::GetVerBuild()
//
// 	Description:	This method is called to get the value of the VerBuild
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMXmlCtrl::GetVerBuild() 
{
	return m_tmVersion.GetBuild();
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::GetVerBuildDate()
//
// 	Description:	This method is called to get the value of the VerBuildDate
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMXmlCtrl::GetVerBuildDate() 
{
	CString strBuildDate = m_tmVersion.GetBuildDate();
	return strBuildDate.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::GetVerMajor()
//
// 	Description:	This method is called to get the value of the VerMajor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMXmlCtrl::GetVerMajor() 
{
	return m_tmVersion.GetMajor();
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::GetVerMinor()
//
// 	Description:	This method is called to get the value of the VerMinor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMXmlCtrl::GetVerMinor() 
{
	return m_tmVersion.GetMinor();
}


//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::GetVerQEF()
//
// 	Description:	This method is called to get the value of the VerQEF
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMXmlCtrl::GetVerQEF() 
{
	return m_tmVersion.GetUpdate();
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::GetVerTextLong()
//
// 	Description:	This method is called to get the value of the VerTextLong
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMXmlCtrl::GetVerTextLong() 
{
	CString strVer = m_tmVersion.GetTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::GetVerTextShort()
//
// 	Description:	This method is called to get the value of the VerTextShort
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMXmlCtrl::GetVerTextShort() 
{
	CString strVer = m_tmVersion.GetShortTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::Initialize()
//
// 	Description:	This function will construct and initialize the xmlbar
//
// 	Returns:		TMDP_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMXmlCtrl::Initialize()
{
	RECT rcClient;

	//	Don't bother if not in user mode
	if(!AmbientUserMode())
		return TMXML_NOERROR;

	//	Is the control already initialized?
	if((m_pFrame != 0) && IsWindow(m_pFrame->m_hWnd))
		return TMXML_NOERROR;

	//	Initialize the error handler
	m_Errors.Enable(m_bEnableErrors);
	m_Errors.SetParent(m_hWnd);
	m_Errors.SetTitle("TMXml Error");
	
	//	Allocate the frame window
	if(m_pFrame)
		delete m_pFrame;
	m_pFrame = new CXmlFrame(this, &m_Errors);
	ASSERT(m_pFrame);

	//	Set the frame properties
	m_pFrame->SetFloatPrintProgress(m_bFloatPrintProgress);
	m_pFrame->SetEmbedded(m_strSource.GetLength() > 0);

	//	Create the window
	if(!m_pFrame->Create())
	{
		m_Errors.Handle(0, IDS_TMXML_CREATEFRAMEFAILED);
		if(m_pFrame)
		{
			delete m_pFrame;
			m_pFrame = 0;
		}

		return TMXML_CREATEFRAMEFAILED;
	}
	else
	{
		//	Make sure the background color matches
		m_pFrame->SetBackColor(TranslateColor(GetBackColor()));

		//	Make sure the frame uses the full client area
		GetClientRect(&rcClient);
		m_pFrame->MoveWindow(&rcClient);

		//	Make the frame visible
		m_pFrame->ShowWindow(SW_SHOW);

		//	Was the source (SRC) property set by the container?
		if(m_strSource.GetLength() > 0)
		{
			//	Set a timer to trigger the loading of the source so that the
			//	window can be created before we actually load the file
			SetTimer(TMXML_LOAD_SOURCE_TIMER, 250, NULL);
			return TMXML_NOERROR;
		}
		else
		{
			//	Load the file if specified
			if(m_strFilename.GetLength() > 0)
				return m_pFrame->LoadFile(m_strFilename);
			else
				return TMXML_NOERROR;
		}
	}

}	

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::jumpToPage()
//
// 	Description:	This method is called to load the specified page in the
//					active media tree. It is provided to support the Ringtail
//					software developers.
//
// 	Returns:		TMXML_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMXmlCtrl::jumpToPage(LPCTSTR lpszPageId) 
{
	//	Let the frame load the file
	if(m_pFrame)
	{
		return m_pFrame->jumpToPage(lpszPageId);
	}
	else
	{
		m_Errors.Handle(0, IDS_TMXML_NOTINITIALIZED);
		return TMXML_NOTINITIALIZED;
	}
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::loadDocument()
//
// 	Description:	This method is called to load a new document. It is 
//					provided specifically at the request of the Ringtail
//					software developers.
//
// 	Returns:		TMXML_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMXmlCtrl::loadDocument(LPCTSTR lpszUrl) 
{
	//	Let the frame load the file
	if(m_pFrame)
	{
		return m_pFrame->loadDocument(lpszUrl);
	}
	else
	{
		m_Errors.Handle(0, IDS_TMXML_NOTINITIALIZED);
		return TMXML_NOTINITIALIZED;
	}
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::LoadFile()
//
// 	Description:	This method is called to load a new file
//
// 	Returns:		TMXML_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMXmlCtrl::LoadFile(LPCTSTR lpFilename) 
{
	//	Let the frame load the file
	if(m_pFrame)
	{
		if(m_pFrame->LoadFile(lpFilename))
		{
			m_strFilename = lpFilename;
			return TMXML_NOERROR;
		}
		else
		{
			return TMXML_LOADFILEFAILED;
		}
	}
	else
	{
		m_Errors.Handle(0, IDS_TMXML_NOTINITIALIZED);
		return TMXML_NOTINITIALIZED;
	}
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::OnAutoInitChanged()
//
// 	Description:	This function is called when the AutoInit property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMXmlCtrl::OnAutoInitChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::OnBackColorChanged()
//
// 	Description:	This method is called when the background color is changed
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMXmlCtrl::OnBackColorChanged() 
{
	// Do the base class processing	
	COleControl::OnBackColorChanged();

	//	Set the overlay background
	if(m_pFrame)
		m_pFrame->SetBackColor(TranslateColor(GetBackColor()));
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::OnCreate()
//
// 	Description:	This fuction handles all WM_CREATE messages
//
// 	Returns:		0 if successful
//
//	Notes:			None
//
//==============================================================================
int CTMXmlCtrl::OnCreate(LPCREATESTRUCT lpCreateStruct) 
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
// 	Function Name:	CTMXmlCtrl::OnFloatPrintProgressChanged()
//
// 	Description:	This method is called when the FloatPrintProgress property
//					is changed
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMXmlCtrl::OnFloatPrintProgressChanged() 
{
	//	Notify the frame
	if(m_pFrame)
		m_pFrame->SetFloatPrintProgress(m_bFloatPrintProgress);
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::OnDraw()
//
// 	Description:	This function is called in response to WM_PAINT messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMXmlCtrl::OnDraw(CDC* pdc, const CRect& rcBounds,const CRect& rcInvalid)
{
	//	Are we in user mode?
	if(AmbientUserMode())
	{
		//	Redraw the frame if it exists
		if((m_pFrame != 0) && IsWindow(m_pFrame->m_hWnd))
		{
			m_pFrame->OnDraw();
			return;
		}
	}

	//	Draw default rendering
	CRect ControlRect = rcBounds;
	CString strText;
	CBrush	brBackground;

	strText.Format("FTI XML Interpreter Control (rev. %s  Build %s)",
				   m_tmVersion.GetShortTextVer(), m_tmVersion.GetBuildDate());

	//	Paint the background
	brBackground.CreateSolidBrush(TranslateColor(GetBackColor()));
	pdc->FillRect(ControlRect, &brBackground);

	pdc->SetBkMode(TRANSPARENT);
	pdc->SetTextColor(TranslateColor(GetForeColor()));
	pdc->DrawText(strText, ControlRect, 
				  DT_CENTER | DT_VCENTER | DT_SINGLELINE); 
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::OnEnableErrorsChanged()
//
// 	Description:	This function is called when the EnableErrors property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMXmlCtrl::OnEnableErrorsChanged() 
{
	m_Errors.Enable(m_bEnableErrors);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::OnFilenameChanged()
//
// 	Description:	This function is called when the Filename property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMXmlCtrl::OnFilenameChanged() 
{
	LoadFile(m_strFilename);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::OnSize()
//
// 	Description:	This function handles all WM_SIZE messages sent to the 
//					control
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMXmlCtrl::OnSize(UINT nType, int cx, int cy) 
{
	//	Perform the base class processing
	COleControl::OnSize(nType, cx, cy);
	
	//	Resize the frame
	if(m_pFrame && IsWindow(m_pFrame->m_hWnd))
		m_pFrame->MoveWindow(0, 0, cx, cy);
}

//==============================================================================
//
// 	Function Name:	CTMXmlCtrl::OnTimer()
//
// 	Description:	This function handles all WM_TIMER messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMXmlCtrl::OnTimer(UINT nIDEvent) 
{
	if(nIDEvent == TMXML_LOAD_SOURCE_TIMER)
	{
		//	Treat this as a one-shot timer
		KillTimer(nIDEvent);

		if(m_strSource.GetLength() > 0)
			LoadFile(m_strSource);
	}
	COleControl::OnTimer(nIDEvent);
}

