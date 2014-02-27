//==============================================================================
//
// File Name:	tmsetup.cpp
//
// Description:	This file contains member functions of the CTMSetupCtrl class.
//
// See Also:	tmlpen.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-27-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmsetap.h>
#include <tmsetup.h>
#include <tmsetpg.h>
#include <tmsudefs.h>
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
extern CTMSetupApp NEAR theApp;

extern const char* _AxClsIds[TMAX_AXCTRL_MAX];
extern const char* _AxNames[TMAX_AXCTRL_MAX];
extern const char* _AxDescriptions[TMAX_AXCTRL_MAX];

CTMSetupCtrl* theControl = 0;

/* Replace 2 */
const IID BASED_CODE IID_DTMSetup6 =
		{ 0xcfa6855c, 0x3eff, 0x4822, { 0x9e, 0xeb, 0xbd, 0xa7, 0x9e, 0xe8, 0x80, 0xd8 } };
/* Replace 3 */
const IID BASED_CODE IID_DTMSetup6Events =
		{ 0xe43e0287, 0xf454, 0x402a, { 0xa0, 0x5b, 0xe2, 0xf7, 0x9e, 0x71, 0x29, 0xd } };

// Control type information
static const DWORD BASED_CODE _dwTMSetupOleMisc =
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
BEGIN_MESSAGE_MAP(CTMSetupCtrl, COleControl)
	//{{AFX_MSG_MAP(CTMSetupCtrl)
	ON_WM_CREATE()
	ON_WM_DESTROY()
	ON_WM_SETFOCUS()
	//}}AFX_MSG_MAP
	ON_NOTIFY(TCN_SELCHANGE, IDC_TABS, OnTabChange)
	ON_NOTIFY(TCN_SELCHANGING, IDC_TABS, OnTabChanging)
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
	ON_MESSAGE(WM_ERROR_EVENT, OnWMErrorEvent)
END_MESSAGE_MAP()

// Dispatch map
BEGIN_DISPATCH_MAP(CTMSetupCtrl, COleControl)
	//{{AFX_DISPATCH_MAP(CTMSetupCtrl)
	DISP_PROPERTY_NOTIFY(CTMSetupCtrl, "AutoInit", m_bAutoInit, OnAutoInitChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMSetupCtrl, "IniFile", m_strIniFile, OnIniFileChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMSetupCtrl, "EnableErrors", m_bEnableErrors, OnEnableErrorsChanged, VT_BOOL)
	DISP_PROPERTY_EX(CTMSetupCtrl, "VerMajor", GetVerMajor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMSetupCtrl, "VerMinor", GetVerMinor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMSetupCtrl, "VerTextLong", GetVerTextLong, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_EX(CTMSetupCtrl, "VerBuild", GetVerBuild, SetNotSupported, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMSetupCtrl, "AboutPage", m_bAboutPage, OnAboutPageChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMSetupCtrl, "DatabasePage", m_bDatabasePage, OnDatabasePageChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMSetupCtrl, "DirectXPage", m_bDirectXPage, OnDirectXPageChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMSetupCtrl, "DiagnosticPage", m_bDiagnosticPage, OnDiagnosticPageChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMSetupCtrl, "GraphicsPage", m_bGraphicsPage, OnGraphicsPageChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMSetupCtrl, "SystemPage", m_bSystemPage, OnSystemPageChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMSetupCtrl, "TextPage", m_bTextPage, OnTextPageChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMSetupCtrl, "VideoPage", m_bVideoPage, OnVideoPageChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMSetupCtrl, "AboutName", m_strAboutName, OnAboutNameChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMSetupCtrl, "AboutVersion", m_strAboutVersion, OnAboutVersionChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMSetupCtrl, "AboutCopyright", m_strAboutCopyright, OnAboutCopyrightChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMSetupCtrl, "AboutPhone", m_strAboutPhone, OnAboutPhoneChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMSetupCtrl, "AboutEmail", m_strAboutEmail, OnAboutEmailChanged, VT_BSTR)
	DISP_FUNCTION(CTMSetupCtrl, "Initialize", Initialize, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMSetupCtrl, "Save", Save, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMSetupCtrl, "SetActiveFilters", SetActiveFilters, VT_I2, VTS_BSTR VTS_UNKNOWN)
	DISP_FUNCTION(CTMSetupCtrl, "GetClassIdString", GetClassIdString, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMSetupCtrl, "GetRegisteredPath", GetRegisteredPath, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMSetupCtrl, "EnumAxVersions", EnumAxVersions, VT_EMPTY, VTS_NONE)
	DISP_STOCKPROP_BACKCOLOR()
	//}}AFX_DISPATCH_MAP
	
	//	Added rev 5.1
	DISP_PROPERTY_NOTIFY_ID(CTMSetupCtrl, "CapturePage", DISPID_CAPTUREPAGE, m_bCapturePage, OnCapturePageChanged, VT_BOOL)

	//	Added rev 6.0
	DISP_PROPERTY_NOTIFY_ID(CTMSetupCtrl, "RingtailPage", DISPID_RINGTAILPAGE, m_bRingtailPage, OnRingtailPageChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY_ID(CTMSetupCtrl, "EnableAxErrors", DISPID_ENABLEAXERRORS, m_bEnableAxErrors, OnEnableAxErrorsChanged, VT_BOOL)

	//	Added rev 6.1.0
	DISP_PROPERTY_EX_ID(CTMSetupCtrl, "VerTextShort", DISPID_VERTEXTSHORT, GetVerTextShort, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_EX_ID(CTMSetupCtrl, "VerQEF", DISPID_VERQEF, GetVerQEF, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX_ID(CTMSetupCtrl, "VerBuildDate", DISPID_VERBUILDDATE, GetVerBuildDate, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_NOTIFY_ID(CTMSetupCtrl, "PresentationFileSpec", DISPID_PRESENTATIONFILESPEC, m_strPresentationFileSpec, OnPresentationFileSpecChanged, VT_BSTR)

END_DISPATCH_MAP()

// Event map
BEGIN_EVENT_MAP(CTMSetupCtrl, COleControl)
	//{{AFX_EVENT_MAP(CTMSetupCtrl)
	EVENT_CUSTOM("AxError", FireAxError, VTS_BSTR)
	EVENT_CUSTOM("AxDiagnostic", FireAxDiagnostic, VTS_BSTR  VTS_BSTR)
	EVENT_CUSTOM("AxVersion", FireAxVersion, VTS_BSTR  VTS_BSTR  VTS_I2  VTS_I2  VTS_I2  VTS_I2  VTS_BSTR  VTS_BSTR  VTS_BSTR  VTS_BSTR  VTS_BSTR)
	//}}AFX_EVENT_MAP
END_EVENT_MAP()

// Property pages
BEGIN_PROPPAGEIDS(CTMSetupCtrl, 2)
	PROPPAGEID(CTMSetupProperties::guid)
	PROPPAGEID(CLSID_CColorPropPage)
END_PROPPAGEIDS(CTMSetupCtrl)

/* Replace 4 */
IMPLEMENT_OLECREATE_EX(CTMSetupCtrl, "TMSETUP6.CTMSetupCtrl.1",
	0xb581682e, 0x5cc0, 0x4e50, 0xbb, 0xbc, 0x58, 0x2d, 0x78, 0x67, 0x7e, 0x5a)

// Type library ID and version
IMPLEMENT_OLETYPELIB(CTMSetupCtrl, _tlid, _wVerMajor, _wVerMinor)
IMPLEMENT_OLECTLTYPE(CTMSetupCtrl, IDS_TMSETUP, _dwTMSetupOleMisc)

IMPLEMENT_DYNCREATE(CTMSetupCtrl, COleControl)

// Interface map for IObjectSafety
BEGIN_INTERFACE_MAP(CTMSetupCtrl, COleControl )
	INTERFACE_PART(CTMSetupCtrl, IID_IObjectSafety, ObjSafety)
END_INTERFACE_MAP()

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::CTMSetupCtrlFactory::UpdateRegistry
//
// 	Description:	Adds or removes system registry entries for this control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTMSetupCtrl::CTMSetupCtrlFactory::UpdateRegistry(BOOL bRegister)
{
	BOOL	bReturn;
	HRESULT	hResult;

	if(bRegister)
	{
		bReturn = AfxOleRegisterControlClass(AfxGetInstanceHandle(),
											 m_clsid,
											 m_lpszProgID,
											 IDS_TMSETUP,
											 IDB_TMSETUP,
											 afxRegApartmentThreading,
											 _dwTMSetupOleMisc,
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
// 	Function Name:	CTMSetupCtrl::XObjSafety::AddRef()
//
// 	Description:	This function is called to attach to the nested
//					IObjectSafety dispatch interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMSetupCtrl::XObjSafety::AddRef()
{
    METHOD_PROLOGUE(CTMSetupCtrl, ObjSafety)
    return pThis->ExternalAddRef();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::XObjSafety::GetInterfaceSafetyOptions()
//
// 	Description:	This function is called to get this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMSetupCtrl::XObjSafety::GetInterfaceSafetyOptions( 
		/* [in]  */ REFIID riid,
        /* [out] */ DWORD __RPC_FAR *pdwSupportedOptions,
        /* [out] */ DWORD __RPC_FAR *pdwEnabledOptions)
{
	HRESULT			hReturn;
	IUnknown FAR*	pInterface;

	METHOD_PROLOGUE(CTMSetupCtrl, ObjSafety)

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
// 	Function Name:	CTMSetupCtrl::XObjSafety::QueryInterface()
//
// 	Description:	This function is called to query the IObjectSafety interface
//					for the requested method/property.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT FAR EXPORT CTMSetupCtrl::XObjSafety::QueryInterface(REFIID iid, 
														  void FAR* FAR* ppvObj)
{
    METHOD_PROLOGUE(CTMSetupCtrl, ObjSafety)
    return (HRESULT)pThis->ExternalQueryInterface(&iid, ppvObj);
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::XObjSafety::Release()
//
// 	Description:	This function is called to detach from the IObjectSafety
//					dispatch interface.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMSetupCtrl::XObjSafety::Release()
{
    METHOD_PROLOGUE(CTMSetupCtrl, ObjSafety)
    return pThis->ExternalRelease();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::XObjSafety::SetInterfaceSafetyOptions()
//
// 	Description:	This function is called to set this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMSetupCtrl::XObjSafety::SetInterfaceSafetyOptions( 
        /* [in] */ REFIID riid,
        /* [in] */ DWORD dwOptionSetMask,
        /* [in] */ DWORD dwEnabledOptions)
{
	IUnknown FAR* pInterface;

    METHOD_PROLOGUE(CTMSetupCtrl, ObjSafety)
	
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
// 	Function Name:	CTMSetupCtrl::AddPage()
//
// 	Description:	This function will add the requested property page to the
//					property sheet
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::AddPage(int iPage, int iResourceId)
{
	TC_ITEM	tciTab;
	char	szTitle[64];
	
	//	Copy the appropriate label to our local buffer
	lstrcpyn(szTitle, GetPageTitle(iPage), sizeof(szTitle));

	//	Initialize the tab information structure
	tciTab.mask = TCIF_TEXT;
	tciTab.iImage = -1;
	tciTab.cchTextMax = lstrlen(szTitle);
	tciTab.pszText = szTitle;

	//	Set the control pointer and error handler
	m_Pages[iPage]->m_pControl = this;
	m_Pages[iPage]->m_pErrors = &m_Errors;
	
	//	Create the new page
	VERIFY(m_Pages[iPage]->Create(iResourceId, m_pTabs));

	//	Set the index and page number of the property page
	m_Pages[iPage]->m_iTab = iPage;
	m_Pages[iPage]->m_iPage = iPage;

	//	Set the tab text for the page. 
	//
	//	Note:	For some reason, if we fail to send the WM_NCACTIVATE messaqe
	//			after inserting the text, edit controls will not give up the
	//			focus when another edit control is clicked on.
	m_pTabs->InsertItem(iPage, &tciTab);
	m_Pages[iPage]->SendMessage(WM_NCACTIVATE, TRUE);
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::CalculateSize()
//
// 	Description:	This function will calculate the size of the control window
//					in pixels. We have to convert from dialog units to pixels
//					to support small and large fonts.
//
// 	Returns:		None
//
//	Notes:			The size dialog (IDD_SIZE_DIALOG) is used to calculate
//					the size of the control window.
//
//==============================================================================
void CTMSetupCtrl::CalculateSize()
{
	CDialog SizeDialog;

	//	This is the size of the control window in dialog units. These values
	//	were calculated by trial and error during development.
	int iDlgWidth  = 327;
	int iDlgHeight = 241;

	//	We have to use the desktop window here because the control window is
	//	not valid yet
	if(!SizeDialog.Create(IDD_SIZE_DIALOG, GetDesktopWindow()))
		return;

	//	Convert the dialog coordinates to pixels
	CRect rc(0, 0, iDlgWidth, iDlgHeight);
	SizeDialog.MapDialogRect(&rc);

	//	Save the dimensions
	m_iWidth  = rc.right;
	m_iHeight = rc.bottom;

	//	Set the initial size of the control window
	SetInitialSize(m_iWidth, m_iHeight);
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::CheckVersion()
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
BOOL CTMSetupCtrl::CheckVersion(DWORD dwVersion)
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
	              "TMSetup ActiveX control. You should upgrade tm_setup6.ocx "
				  "as soon as possible\n\n%s", 
				  (wMinor > _wVerMinor) ? "a newer" : "an older", strVersion);

	MessageBeep(MB_ICONEXCLAMATION);
	MessageBox(strMsg, "TrialMax Error", MB_ICONEXCLAMATION | MB_OK);

	//	The versions do not match
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::CTMSetupCtrl()
//
// 	Description:	This is the constructor for CTMSetupCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMSetupCtrl::CTMSetupCtrl()
{
	InitializeIIDs(&IID_DTMSetup6, &IID_DTMSetup6Events);

	theControl = this;

	//	Initialize the page array
	m_Pages[DATABASE_PAGE] = &m_Database;
	m_Pages[GRAPHICS_PAGE] = &m_Graphics;
	m_Pages[TEXT_PAGE] = &m_Text;
	m_Pages[VIDEO_PAGE] = &m_Video;
	m_Pages[DIAGNOSTIC_PAGE] = &m_Diagnostic;
	m_Pages[DIRECTX_PAGE] = &m_DirectX;
	m_Pages[SYSTEM_PAGE] = &m_System;
	m_Pages[ABOUT_PAGE] = &m_About;
	m_Pages[CAPTURE_PAGE] = &m_Capture;
	m_Pages[RINGTAIL_PAGE] = &m_Ringtail;

	//	Initialize the local data
	m_pTabs		= 0;
	m_iHeight	= 0;
	m_iWidth	= 0;
	m_pPage		= 0;
	m_strPresentationFileSpec.Empty();

	//	Calculate the size of the options dialog
	CalculateSize();

	//	Get the registry information
	GetRegistration();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::~CTMSetupCtrl()
//
// 	Description:	This is the destructor for CTMSetupCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMSetupCtrl::~CTMSetupCtrl()
{
}		

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::DoPropertyExchange()
//
// 	Description:	This function manages the exchange of persistant 
//					properties
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::DoPropExchange(CPropExchange* pPX)
{
	BOOL bAutoInit = FALSE;
	BOOL bEnableErrors = FALSE;
	BOOL bIniFile = FALSE;
	BOOL bAboutPage = FALSE;
	BOOL bDatabasePage = FALSE;
	BOOL bDirectXPage = FALSE;
	BOOL bDiagnosticPage = FALSE;
	BOOL bGraphicsPage = FALSE;
	BOOL bSystemPage = FALSE;
	BOOL bTextPage = FALSE;
	BOOL bVideoPage = FALSE;
	BOOL bAboutName = FALSE;
	BOOL bAboutVersion = FALSE;
	BOOL bAboutCopyright = FALSE;
	BOOL bAboutPhone = FALSE;
	BOOL bAboutEmail = FALSE;
	BOOL bCapturePage = FALSE;
	BOOL bRingtailPage = FALSE;
	BOOL bEnableAxErrors = FALSE;
	BOOL bPresentationFileSpec = FALSE;

	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
	COleControl::DoPropExchange(pPX);

	//CheckVersion(pPX->GetVersion());

	try
	{
		//	Load the control's persistent properties
		bAutoInit = PX_Bool(pPX, _T("AutoInit"), m_bAutoInit, TMSETUP_AUTOINIT);
		bEnableErrors = PX_Bool(pPX, _T("EnableErrors"), m_bEnableErrors, TRUE);
		bIniFile = PX_String(pPX, _T("IniFile"), m_strIniFile, TMSETUP_INIFILE);
		bAboutPage = PX_Bool(pPX, _T("AboutPage"), m_bAboutPage, TMSETUP_ABOUTPAGE);
		bDatabasePage = PX_Bool(pPX, _T("DatabasePage"), m_bDatabasePage, TMSETUP_DATABASEPAGE);
		bDirectXPage = PX_Bool(pPX, _T("DirectXPage"), m_bDirectXPage, TMSETUP_DIRECTXPAGE);
		bDiagnosticPage = PX_Bool(pPX, _T("DiagnosticPage"), m_bDiagnosticPage, TMSETUP_DIAGNOSTICPAGE);
		bGraphicsPage = PX_Bool(pPX, _T("GraphicsPage"), m_bGraphicsPage, TMSETUP_GRAPHICSPAGE);
		bSystemPage = PX_Bool(pPX, _T("SystemPage"), m_bSystemPage, TMSETUP_SYSTEMPAGE);
		bTextPage = PX_Bool(pPX, _T("TextPage"), m_bTextPage, TMSETUP_TEXTPAGE);
		bVideoPage = PX_Bool(pPX, _T("VideoPage"), m_bVideoPage, TMSETUP_VIDEOPAGE);
		bAboutName = PX_String(pPX, _T("AboutName"), m_strAboutName, TMSETUP_ABOUTNAME);
		bAboutVersion = PX_String(pPX, _T("AboutVersion"), m_strAboutVersion, TMSETUP_ABOUTVERSION);
		bAboutCopyright = PX_String(pPX, _T("AboutCopyright"), m_strAboutCopyright, TMSETUP_ABOUTCOPYRIGHT);
		bAboutPhone = PX_String(pPX, _T("AboutPhone"), m_strAboutPhone, TMSETUP_ABOUTPHONE);
		bAboutEmail = PX_String(pPX, _T("AboutEmail"), m_strAboutEmail, TMSETUP_ABOUTEMAIL);
		bCapturePage = PX_Bool(pPX, _T("CapturePage"), m_bCapturePage, TMSETUP_CAPTUREPAGE);
		bRingtailPage = PX_Bool(pPX, _T("RingtailPage"), m_bRingtailPage, TMSETUP_RINGTAILPAGE);
		bEnableAxErrors = PX_Bool(pPX, _T("EnableAxErrors"), m_bEnableAxErrors, TMSETUP_ENABLEAXERRORS);
		bPresentationFileSpec = PX_String(pPX, _T("PresentationFileSpec"), m_strPresentationFileSpec, "");
	}
	catch(...)
	{
		if(!bAutoInit) m_bAutoInit = TMSETUP_AUTOINIT;
		if(!bEnableErrors) m_bEnableErrors = TRUE;
		if(!bIniFile) m_strIniFile = TMSETUP_INIFILE;
		if(!bAboutPage) m_bAboutPage = TMSETUP_ABOUTPAGE;
		if(!bDatabasePage) m_bDatabasePage = TMSETUP_DATABASEPAGE;
		if(!bDirectXPage) m_bDirectXPage = TMSETUP_DIRECTXPAGE;
		if(!bDiagnosticPage) m_bDiagnosticPage = TMSETUP_DIAGNOSTICPAGE;
		if(!bGraphicsPage) m_bGraphicsPage = TMSETUP_GRAPHICSPAGE;
		if(!bSystemPage) m_bSystemPage = TMSETUP_SYSTEMPAGE;
		if(!bTextPage) m_bTextPage = TMSETUP_TEXTPAGE;
		if(!bVideoPage) m_bVideoPage = TMSETUP_VIDEOPAGE;
		if(!bAboutName) m_strAboutName = TMSETUP_ABOUTNAME;
		if(!bAboutVersion) m_strAboutVersion = TMSETUP_ABOUTVERSION;
		if(!bAboutCopyright) m_strAboutCopyright = TMSETUP_ABOUTCOPYRIGHT;
		if(!bAboutPhone) m_strAboutPhone = TMSETUP_ABOUTPHONE;
		if(!bAboutEmail) m_strAboutEmail = TMSETUP_ABOUTEMAIL;
		if(!bCapturePage) m_bCapturePage = TMSETUP_CAPTUREPAGE;
		if(!bRingtailPage) m_bRingtailPage = TMSETUP_RINGTAILPAGE;
		if(!bEnableAxErrors) m_bEnableAxErrors = TMSETUP_ENABLEAXERRORS;
		if(!bPresentationFileSpec) m_strPresentationFileSpec = "";
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
		
				m_strPresentationFileSpec = "";

		}

	}

}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::EnumAxVersions()
//
// 	Description:	This function is called to enumerate the version information
//					for all the TrialMax ActiveX controls
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::EnumAxVersions() 
{
	CTMVersion ver;

	//	Do we have a path to TmaxPresentation?
	if(m_strPresentationFileSpec.GetLength() > 0)
	{
		//	Get the version information
		ver.InitFromFile("TmaxPresentation", "TrialMax Courtroom Presentation Application",
						 m_strPresentationFileSpec);
		FireAxVersion(ver);
	}

	//	ActiveX controls
	for(int i = 0; i < TMAX_AXCTRL_MAX; i++)
	{
		ver.InitFromClsId(_AxNames[i], _AxDescriptions[i], _AxClsIds[i]);
		FireAxVersion(ver);
	}
	
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::FindFile()
//
// 	Description:	This function is called to determine if the specified file
//					exists.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTMSetupCtrl::FindFile(LPCSTR lpFilename)
{
	WIN32_FIND_DATA	Find;
	HANDLE			hFind;

	ASSERT(lpFilename);

	if((hFind = FindFirstFile(lpFilename, &Find)) == INVALID_HANDLE_VALUE)
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
// 	Function Name:	CTMSetupCtrl::FireVersion()
//
// 	Description:	This function is will fire the AxVersion event for the 
//					specified TrialMax version object
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::FireAxVersion(CTMVersion& tmVersion) 
{
	CString strLocation;

	//	Fire the event
	FireAxVersion(tmVersion.GetName(), 
				  tmVersion.GetDescription(),
				  tmVersion.GetMajor(),
				  tmVersion.GetMinor(),
				  tmVersion.GetUpdate(),
				  tmVersion.GetBuild(),
				  tmVersion.GetShortTextVer(),
				  tmVersion.GetTextVer(),
				  tmVersion.GetBuildDate(),
				  tmVersion.GetClsId(),
				  tmVersion.GetLocation(strLocation));
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::GetClassIdString()
//
// 	Description:	This method is called to get the class GUID as a null
//					terminated string.
//
// 	Returns:		The string equivalent of the CLASS GUID
//
//	Notes:			None
//
//==============================================================================
BSTR CTMSetupCtrl::GetClassIdString() 
{
	CString strClsId = m_tmVersion.GetClsId();
	return strClsId.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::GetLocation()
//
// 	Description:	This function is called to get the location where the 
//					specified control is registered
//
// 	Returns:		TRUE if registered and the path is valid
//
//	Notes:			None
//
//==============================================================================
BOOL CTMSetupCtrl::GetLocation(LPCSTR lpszClsId, CString& rPath)
{
    HKEY        hKey = NULL;
    char        szKey[513];
	char		szPath[_MAX_PATH];
	DWORD		dwLength = sizeof(szPath);
	BOOL		bSuccessful = FALSE;
	CString		strParent;

	memset(szPath, 0, sizeof(szPath));

	// Open the key under the control's clsid HKEY_CLASSES_ROOT\CLSID\<CLSID>
	wsprintf(szKey, "CLSID\\%s\\InprocServer32", lpszClsId);
	if(RegOpenKeyEx(HKEY_CLASSES_ROOT, szKey, 0, KEY_QUERY_VALUE, &hKey) == ERROR_SUCCESS)
	{
		if((RegQueryValueEx(hKey, "", NULL, NULL, (LPBYTE)szPath, &dwLength) == ERROR_SUCCESS) &&
		   (lstrlen(szPath) > 0))
		{
			CTMToolbox::GetLongPath(szPath, rPath);
			rPath.MakeLower();

			//	Does this file exist?
			if(FindFile(szPath) == TRUE)
			{
				bSuccessful = TRUE;
			}
			else
			{
				rPath = ("NOT FOUND: " + rPath);
			}

		}
		else
		{
			rPath = "NO REGISTERED PATH";
		}
	
	}
	else
	{
		rPath = "CONTROL NOT REGISTERED";
	}

	return bSuccessful;
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::GetPageTitle()
//
// 	Description:	This function is called to get the title for the specified
//					property page.
//
// 	Returns:		A pointer to the title string
//
//	Notes:			None
//
//==============================================================================
LPSTR CTMSetupCtrl::GetPageTitle(int iPage)
{
	//	Which page?
	switch(iPage)
	{
		case DATABASE_PAGE:		return DATABASE_TITLE;
		case GRAPHICS_PAGE:		return GRAPHICS_TITLE;
		case VIDEO_PAGE:		return VIDEO_TITLE;
		case TEXT_PAGE:			return TEXT_TITLE;
		case DIAGNOSTIC_PAGE:	return DIAGNOSTIC_TITLE;
		case DIRECTX_PAGE:		return DIRECTX_TITLE;
		case SYSTEM_PAGE:		return SYSTEM_TITLE;
		case ABOUT_PAGE:		return ABOUT_TITLE;
		case CAPTURE_PAGE:		return CAPTURE_TITLE;
		case RINGTAIL_PAGE:		return RINGTAIL_TITLE;
		default:				return "Unknown";
	}
}	

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::GetRegisteredPath()
//
// 	Description:	This method is called to get the ocx path stored in the
//					system registry.
//
// 	Returns:		The path to the ocx stored in the registry
//
//	Notes:			None
//
//==============================================================================
BSTR CTMSetupCtrl::GetRegisteredPath() 
{
	CString strRegistered = m_tmVersion.GetFileSpec();
	return strRegistered.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::GetRegistration()
//
// 	Description:	This function is called to get the control's registration
//					information.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::GetRegistration() 
{
	CLSID	clsid;

	//	Get the GUID 
	GetClassID(&clsid);

	//	Initialize the version information
	m_tmVersion.InitFromClsId("TMSetup", "TmaxPresentation Setup", clsid);

}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::GetVerBuild()
//
// 	Description:	This method is called to get the value of the VerBuild
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMSetupCtrl::GetVerBuild() 
{
	return m_tmVersion.GetBuild();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::GetVerBuildDate()
//
// 	Description:	This method is called to get the value of the VerBuildDate
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMSetupCtrl::GetVerBuildDate() 
{
	CString strBuildDate = m_tmVersion.GetBuildDate();
	return strBuildDate.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::GetVerMajor()
//
// 	Description:	This method is called to get the value of the VerMajor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMSetupCtrl::GetVerMajor() 
{
	return m_tmVersion.GetMajor();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::GetVerMinor()
//
// 	Description:	This method is called to get the value of the VerMinor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMSetupCtrl::GetVerMinor() 
{
	return m_tmVersion.GetMinor();
}


//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::GetVerQEF()
//
// 	Description:	This method is called to get the value of the VerQEF
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMSetupCtrl::GetVerQEF() 
{
	return m_tmVersion.GetUpdate();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::GetVerTextLong()
//
// 	Description:	This method is called to get the value of the VerTextLong
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMSetupCtrl::GetVerTextLong() 
{
	CString strVer = m_tmVersion.GetTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::GetVerTextShort()
//
// 	Description:	This method is called to get the value of the VerTextShort
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMSetupCtrl::GetVerTextShort() 
{
	CString strVer = m_tmVersion.GetShortTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::Initialize()
//
// 	Description:	This function will construct and initialize the lpenbar
//
// 	Returns:		TMDP_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMSetupCtrl::Initialize()
{
	CRect	rcTabs(BORDER, BORDER, m_iWidth - (2*BORDER), m_iHeight - (2*BORDER));
	DWORD	dwTabsStyle = WS_CHILD | WS_VISIBLE | WS_CLIPSIBLINGS;

	//	Don't bother if not in user mode or if the control is already initialized
	if(!AmbientUserMode() || (m_pTabs != 0))
		return TMSETUP_NOERROR;

	//	Set the about box members
	m_About.m_strName = m_strAboutName;
	m_About.m_strVersion = m_strAboutVersion;
	m_About.m_strCopyright = m_strAboutCopyright;
	m_About.m_strEmail = m_strAboutEmail;

	//	Create the tab control
	m_pTabs = new CTabCtrl;
	ASSERT(m_pTabs);
	if(!m_pTabs->Create(dwTabsStyle, rcTabs, this, IDC_TABS))
		return TMSETUP_CREATETABFAILED;
	
	//	Add each page
	//
	//	NOTE:	Initially we add each page so that the window gets created. If
	//			any pages have been disabled we will remove them later
	AddPage(DATABASE_PAGE, IDD_DATABASE_PAGE);
	AddPage(GRAPHICS_PAGE, IDD_GRAPHICS_PAGE);
	AddPage(VIDEO_PAGE, IDD_VIDEO_PAGE);
	AddPage(TEXT_PAGE, IDD_TEXT_PAGE);
	AddPage(DIAGNOSTIC_PAGE, IDD_DIAGNOSTIC_PAGE);
	AddPage(DIRECTX_PAGE, IDD_DIRECTX_PAGE);
	AddPage(ABOUT_PAGE, IDD_ABOUT_PAGE);
	AddPage(SYSTEM_PAGE, IDD_SYSTEM_PAGE);
	AddPage(CAPTURE_PAGE, IDD_CAPTURE_PAGE);
	AddPage(RINGTAIL_PAGE, IDD_RINGTAIL_PAGE);

	//	Set the font of the tab control to match that of the pages
	m_pTabs->SetFont(m_Pages[0]->GetFont(), TRUE);

	//	Remove any pages that have been disabled
	if(!m_bDatabasePage)
		RemovePage(DATABASE_PAGE);
	if(!m_bGraphicsPage)
		RemovePage(GRAPHICS_PAGE);
	if(!m_bVideoPage)
		RemovePage(VIDEO_PAGE);
	if(!m_bTextPage)
		RemovePage(TEXT_PAGE);
	if(!m_bDiagnosticPage)
		RemovePage(DIAGNOSTIC_PAGE);
	if(!m_bDirectXPage)
		RemovePage(DIRECTX_PAGE);
	if(!m_bAboutPage)
		RemovePage(ABOUT_PAGE);
	if(!m_bSystemPage)
		RemovePage(SYSTEM_PAGE);
	if(!m_bCapturePage)
		RemovePage(CAPTURE_PAGE);
	//if(!m_bRingtailPage)
		RemovePage(RINGTAIL_PAGE);

	//	Set the focus to the first page
	OnTabChange(0, 0);

	//	Initialize from the ini file
	OnIniFileChanged();

	return TMSETUP_NOERROR;
}	

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::InsertPage()
//
// 	Description:	This function will insert the specified page into the 
//					property sheet.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::InsertPage(int iPage)
{
	TC_ITEM	tciTab;
	char	szTitle[64];
	int		iIndex = 0;
	int		i;
	
	ASSERT(m_pTabs);
	if(m_pTabs == 0)
		return;

	//	Copy the appropriate label to our local buffer
	lstrcpyn(szTitle, GetPageTitle(iPage), sizeof(szTitle));

	//	Initialize the tab information structure
	tciTab.mask = TCIF_TEXT;
	tciTab.iImage = -1;
	tciTab.cchTextMax = lstrlen(szTitle);
	tciTab.pszText = szTitle;

	//	Look for the location at which to insert this page
	for(i = 0; i < iPage; i++)
	{
		//	Is this page currently visible?
		if(m_Pages[i]->m_iTab >= 0)
			iIndex = m_Pages[i]->m_iTab + 1;
	}

	//	Set the tab text for the page. 
	//
	//	Note:	For some reason, if we fail to send the WM_NCACTIVATE messaqe
	//			after inserting the text, edit controls will not give up the
	//			focus when another edit control is clicked on.
	m_pTabs->InsertItem(iIndex, &tciTab);
	m_Pages[iPage]->m_iTab = iIndex;
	m_Pages[iPage]->SendMessage(WM_NCACTIVATE, TRUE);

	//	Now we have to reset the tab indexes for all pages that appear after
	//	the page we just inserted
	for(i = (iPage + 1); i < MAX_PAGES; i++)
	{
		if(m_Pages[i]->m_iTab >= 0)
			m_Pages[i]->m_iTab++;
	}
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnAboutCopyrightChanged()
//
// 	Description:	This function is called when the AboutCopyright property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnAboutCopyrightChanged() 
{
	if(AmbientUserMode())
	{
		m_About.m_strCopyright = m_strAboutCopyright;
		if(IsWindow(m_About.m_hWnd))
			m_About.UpdateData(FALSE);
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnAboutEmailChanged()
//
// 	Description:	This function is called when the AboutEmail property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnAboutEmailChanged() 
{
	if(AmbientUserMode())
	{
		m_About.m_strEmail = m_strAboutEmail;
		if(IsWindow(m_About.m_hWnd))
			m_About.UpdateData(FALSE);
	}
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnAboutNameChanged()
//
// 	Description:	This function is called when the AboutName property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnAboutNameChanged() 
{
	if(AmbientUserMode())
	{
		m_About.m_strName = m_strAboutName;
		if(IsWindow(m_About.m_hWnd))
			m_About.UpdateData(FALSE);
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnAboutPageChanged()
//
// 	Description:	This function is called when the AboutPage property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnAboutPageChanged() 
{
	if(AmbientUserMode())
	{
		if(m_bAboutPage)
		{
			//	Insert the page if not already in the control
			if(m_Pages[ABOUT_PAGE]->m_iTab < 0)
				InsertPage(ABOUT_PAGE);
		}
		else
		{
			//	Remove the page if it's in the control
			if(m_Pages[ABOUT_PAGE]->m_iTab >= 0)
				RemovePage(ABOUT_PAGE);
		}
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnAboutPhoneChanged()
//
// 	Description:	This function is called when the AboutPhone property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnAboutPhoneChanged() 
{
	if(AmbientUserMode())
	{
		if(IsWindow(m_About.m_hWnd))
			m_About.UpdateData(FALSE);
	}
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnAboutVersionChanged()
//
// 	Description:	This function is called when the AboutVersion property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnAboutVersionChanged() 
{
	if(AmbientUserMode())
	{
		m_About.m_strVersion = m_strAboutVersion;
		if(IsWindow(m_About.m_hWnd))
			m_About.UpdateData(FALSE);
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnAutoInitChanged()
//
// 	Description:	This function is called when the AutoInit property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnAutoInitChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnCapturePageChanged()
//
// 	Description:	This function is called when the CapturePage property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnCapturePageChanged() 
{
	if(AmbientUserMode())
	{
		if(m_bCapturePage)
		{
			//	Insert the page if not already in the control
			if(m_Pages[CAPTURE_PAGE]->m_iTab < 0)
				InsertPage(CAPTURE_PAGE);
		}
		else
		{
			//	Remove the page if it's in the control
			if(m_Pages[CAPTURE_PAGE]->m_iTab >= 0)
				RemovePage(CAPTURE_PAGE);
		}
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnCreate()
//
// 	Description:	This fuction handles all WM_CREATE messages
//
// 	Returns:		0 if successful
//
//	Notes:			None
//
//==============================================================================
int CTMSetupCtrl::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
	//	Do the base class processing first
	if(COleControl::OnCreate(lpCreateStruct) == -1)
		return -1;

	//	Initialize the error handler
	m_Errors.Enable(m_bEnableErrors);
	m_Errors.SetParent(m_hWnd);
	m_Errors.SetTitle("TMSetup Error");
	m_Errors.SetMessageId(m_bEnableAxErrors == TRUE ? WM_ERROR_EVENT : 0);
	
	//	Initialize the control
	if(m_bAutoInit)
		Initialize();
	
	return 0;
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnDatabasePageChanged()
//
// 	Description:	This function is called when the DatabasePage property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnDatabasePageChanged() 
{
	if(AmbientUserMode())
	{
		if(m_bDatabasePage)
		{
			//	Insert the page if not already in the control
			if(m_Pages[DATABASE_PAGE]->m_iTab < 0)
				InsertPage(DATABASE_PAGE);
		}
		else
		{
			//	Remove the page if it's in the control
			if(m_Pages[DATABASE_PAGE]->m_iTab >= 0)
				RemovePage(DATABASE_PAGE);
		}
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnDestroy()
//
// 	Description:	This function is called when the window is destroyed
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnDestroy() 
{
	//	Delete the tab control
	if(m_pTabs != 0)
	{
		delete m_pTabs;
		m_pTabs = 0;
	}
		
	//	Do the base class cleanup
	COleControl::OnDestroy();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnDiagnosticPageChanged()
//
// 	Description:	This function is called when the DiagnosticPage property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnDiagnosticPageChanged() 
{
	if(AmbientUserMode())
	{
		if(m_bDiagnosticPage)
		{
			//	Insert the page if not already in the control
			if(m_Pages[DIAGNOSTIC_PAGE]->m_iTab < 0)
				InsertPage(DIAGNOSTIC_PAGE);
		}
		else
		{
			//	Remove the page if it's in the control
			if(m_Pages[DIAGNOSTIC_PAGE]->m_iTab >= 0)
				RemovePage(DIAGNOSTIC_PAGE);
		}
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnDirectXPageChanged()
//
// 	Description:	This function is called when the DirectXPage property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnDirectXPageChanged() 
{
	if(AmbientUserMode())
	{
		if(m_bDirectXPage)
		{
			//	Insert the page if not already in the control
			if(m_Pages[DIRECTX_PAGE]->m_iTab < 0)
				InsertPage(DIRECTX_PAGE);
		}
		else
		{
			//	Remove the page if it's in the control
			if(m_Pages[DIRECTX_PAGE]->m_iTab >= 0)
				RemovePage(DIRECTX_PAGE);
		}
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnDraw()
//
// 	Description:	This function is called in response to WM_PAINT messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnDraw(CDC* pdc, const CRect& rcBounds,const CRect& rcInvalid)
{
	CBrush brBackground;

	//	Are we in user mode?
	if(AmbientUserMode())
	{
		//	Paint the window only if the tab control has not been created
		//if((m_pTabs == 0) || !IsWindow(m_pTabs->m_hWnd))
		//{
			brBackground.CreateSolidBrush(TranslateColor(GetBackColor()));
			pdc->FillRect(rcBounds, &brBackground);
		//}
	}
	else
	{
		CString	strText;
		CRect ControlRect = rcBounds;

		strText.Format("FTI Setup Control (rev. %d.%d)", _wVerMajor, _wVerMinor);

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
// 	Function Name:	CTMSetupCtrl::OnEnableAxErrorsChanged()
//
// 	Description:	This function is called when the EnableAxErrors property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnEnableAxErrorsChanged() 
{
	SetModifiedFlag();

	if(AmbientUserMode())
		m_Errors.SetMessageId(m_bEnableAxErrors == TRUE ? WM_ERROR_EVENT : 0);
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnEnableErrorsChanged()
//
// 	Description:	This function is called when the EnableErrors property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnEnableErrorsChanged() 
{
	m_Errors.Enable(m_bEnableErrors);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnGraphicsPageChanged()
//
// 	Description:	This function is called when the GraphicsPage property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnGraphicsPageChanged() 
{
	if(AmbientUserMode())
	{
		if(m_bGraphicsPage)
		{
			//	Insert the page if not already in the control
			if(m_Pages[GRAPHICS_PAGE]->m_iTab < 0)
				InsertPage(GRAPHICS_PAGE);
		}
		else
		{
			//	Remove the page if it's in the control
			if(m_Pages[GRAPHICS_PAGE]->m_iTab >= 0)
				RemovePage(GRAPHICS_PAGE);
		}
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnIniFileChanged()
//
// 	Description:	This function is called when the IniFile property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnIniFileChanged() 
{
	//	Are we in user mode?
	if(AmbientUserMode())
	{
		//	Open the ini file
		m_Ini.Open(m_strIniFile);

		//	Notify each of the pages
		for(int i = 0; i < MAX_PAGES; i++)
			m_Pages[i]->ReadOptions(m_Ini);
	}

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnPresentationFileSpecChanged()
//
// 	Description:	This function is called when the PresentationFileSpec 
//					property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnPresentationFileSpecChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnRingtailPageChanged()
//
// 	Description:	This function is called when the RingtailPage property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnRingtailPageChanged() 
{
	if(AmbientUserMode())
	{
		if(m_bRingtailPage)
		{
			//	Insert the page if not already in the control
			if(m_Pages[RINGTAIL_PAGE]->m_iTab < 0)
				InsertPage(RINGTAIL_PAGE);
		}
		else
		{
			//	Remove the page if it's in the control
			if(m_Pages[RINGTAIL_PAGE]->m_iTab >= 0)
				RemovePage(RINGTAIL_PAGE);
		}
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnSetExtent()
//
// 	Description:	This function is called whenever an attempt is made to 
//					resize the control. It fixes the control size based on
//					the width/height defined in the constructor.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTMSetupCtrl::OnSetExtent(LPSIZEL lpSizeL) 
{
	CWnd* pWindow;

	//	Get a CDC object so we can call HIMETRICtoDP() and DPtoHIMETRIC()
	pWindow = CWnd::FromHandle(::GetDesktopWindow());
	CClientDC dc(pWindow);

	//	Set the size to that of the property sheet
	CSize size(m_iWidth, m_iHeight);
	dc.DPtoHIMETRIC(&size);
	lpSizeL->cx = size.cx;
	lpSizeL->cy = size.cy;	
	
	return COleControl::OnSetExtent(lpSizeL);
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnSetFocus()
//
// 	Description:	This function is called when the control gains the keyboard
//					focus.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnSetFocus(CWnd* pOldWnd) 
{
	COleControl::OnSetFocus(pOldWnd);
	
	//	Set the focus to the current page
	if(m_pPage)
		m_pPage->SetFocus();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnSystemPageChanged()
//
// 	Description:	This function is called when the SystemPage property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnSystemPageChanged() 
{
	if(AmbientUserMode())
	{
		if(m_bSystemPage)
		{
			//	Insert the page if not already in the control
			if(m_Pages[SYSTEM_PAGE]->m_iTab < 0)
				InsertPage(SYSTEM_PAGE);
		}
		else
		{
			//	Remove the page if it's in the control
			if(m_Pages[SYSTEM_PAGE]->m_iTab >= 0)
				RemovePage(SYSTEM_PAGE);
		}
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnTabChange()
//
// 	Description:	This function will enable the page selected by the user.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnTabChange(NMHDR* pnmhdr, LRESULT* pResult)
{
	RECT		rcTabCtrl;
	int			iTab;
	
	//	Get the current selection
	if((iTab = m_pTabs->GetCurSel()) < 0)
		iTab = 0;

	//	Get the size of the tab control so we can display the new dialog
	//	page properly
	m_pTabs->GetItemRect(0, &rcTabCtrl);

	//	Get the newly selected page
	for(int i = 0; i < MAX_PAGES; i++)
	{
		if(m_Pages[i]->m_iTab == iTab)
		{
			m_pPage = m_Pages[i];
			break;
		}
	}

	if(m_pPage == 0)
	{
		if(pResult)
			*pResult = FALSE;
		return;
	}

	//	Set the page position
	m_pPage->SetWindowPos(NULL, rcTabCtrl.left + 5, rcTabCtrl.bottom + 5, 
						0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_SHOWWINDOW);
	m_pPage->SetFocus();

	if(pResult)
		*pResult = TRUE;
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnTabChanging()
//
// 	Description:	This function will hide the current page when the user
//					tabs to a new page.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnTabChanging(NMHDR* pnmhdr, LRESULT* pResult)
{
	CDialog* pPage;

	//	Stop here if no tab is selected
	if(m_pTabs->GetCurSel() < 0)
		return;

	//	Get the newly selected page
	for(int i = 0; i < MAX_PAGES; i++)
	{
		if(m_Pages[i]->m_iTab == m_pTabs->GetCurSel())
		{
			pPage = m_Pages[i];
			break;
		}
	}

	if(pPage == 0)
	{
		if(pResult)
			*pResult = TRUE;
		return;
	}

	//	Hide this page
	pPage->ShowWindow(SW_HIDE);

	if(pResult)
		*pResult = FALSE;
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnTextPageChanged()
//
// 	Description:	This function is called when the TextPage property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnTextPageChanged() 
{
	if(AmbientUserMode())
	{
		if(m_bTextPage)
		{
			//	Insert the page if not already in the control
			if(m_Pages[TEXT_PAGE]->m_iTab < 0)
				InsertPage(TEXT_PAGE);
		}
		else
		{
			//	Remove the page if it's in the control
			if(m_Pages[TEXT_PAGE]->m_iTab >= 0)
				RemovePage(TEXT_PAGE);
		}
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnVideoPageChanged()
//
// 	Description:	This function is called when the VideoPage property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::OnVideoPageChanged() 
{
	if(AmbientUserMode())
	{
		if(m_bVideoPage)
		{
			//	Insert the page if not already in the control
			if(m_Pages[VIDEO_PAGE]->m_iTab < 0)
				InsertPage(VIDEO_PAGE);
		}
		else
		{
			//	Remove the page if it's in the control
			if(m_Pages[VIDEO_PAGE]->m_iTab >= 0)
				RemovePage(VIDEO_PAGE);
		}
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::OnWMErrorEvent()
//
// 	Description:	This function handles all WM_ERROR_EVENT messages
//
// 	Returns:		Zero if the message is handled
//
//	Notes:			None
//
//==============================================================================
LONG CTMSetupCtrl::OnWMErrorEvent(WPARAM wParam, LPARAM lParam)
{
	if((m_bEnableAxErrors == TRUE) && (lstrlen(m_Errors.GetMessage()) > 0))
	{
		FireAxError(m_Errors.GetMessage());
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::PreTranslateMessage()
//
// 	Description:	This function is called trap all messages sent to the
//					control window.
//
// 	Returns:		TRUE if the message is handled
//
//	Notes:			None
//
//==============================================================================
BOOL CTMSetupCtrl::PreTranslateMessage(MSG* pMsg) 
{
	BOOL		bShift;
	RECT		rcTab;
	int			nTotal;
	int			nCurrent;
	int			nNew;

	//	Trap the Escape and Return keys to prevent closing the active page
	if(pMsg->wParam == VK_ESCAPE || pMsg->wParam == VK_RETURN)
		return TRUE;

	//	Check to see if the user is attempting to change pages
	if(pMsg->wParam == VK_TAB && GetKeyState(VK_CONTROL) < 0)
	{
		//	Only deal with the WM_KEYDOWN messages
		if(pMsg->message != WM_KEYDOWN)
			return TRUE;

		//	Is the shift key pressed also?
		bShift = (GetKeyState(VK_SHIFT) < 0);

		//	What is the current page index and total pages?
		nTotal = m_pTabs->GetItemCount();
		nCurrent = m_pTabs->GetCurSel();
		
		//	What is the index of the new selection?
		nNew = (bShift) ? ((nCurrent + nTotal - 1) % nTotal) :
						  ((nCurrent + nTotal + 1) % nTotal);

		//	Now simulate the user clicking on the desired tab
		m_pTabs->GetItemRect(nNew, &rcTab);
		m_pTabs->SendMessage(WM_LBUTTONDOWN, MK_LBUTTON, 
							 MAKELONG(rcTab.left, rcTab.top));
		return TRUE;
	}
		
	//	Get the current page and give it the first opportunity to process
	//	this message
	if(m_pPage && m_pPage->PreTranslateMessage(pMsg))
		return TRUE;
	else
		return COleControl::PreTranslateMessage(pMsg);
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::RemovePage()
//
// 	Description:	This function will remove the specified page from the tab
//					control
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMSetupCtrl::RemovePage(int iPage)
{
	int iTab;

	ASSERT(m_pTabs);
	if(m_pTabs == 0)
		return;

	//	Get the current selection
	iTab = m_pTabs->GetCurSel();

	//	Is this page in the sheet?
	if(m_Pages[iPage]->m_iTab >= 0)
	{
		//	Remove the tab
		m_pTabs->DeleteItem(m_Pages[iPage]->m_iTab);

		if(::IsWindowVisible(m_Pages[iPage]->m_hWnd))
			m_Pages[iPage]->ShowWindow(SW_HIDE);

		//	Adjust the tab indexes of all pages that appear after this page
		for(int i = (iPage + 1); i < MAX_PAGES; i++)
		{
			if(m_Pages[i]->m_iTab > 0)
				m_Pages[i]->m_iTab--;
		}

		//	Is the current page being removed?
		if(m_Pages[iPage]->m_iTab == iTab)
		{
			//	Clear the index for this page
			m_Pages[iPage]->m_iTab = -1;
			
			m_pTabs->SetCurSel(0);
			OnTabChange(0,0);
		}
		else
		{
			//	Clear the index for this page
			m_Pages[iPage]->m_iTab = -1;
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::Save()
//
// 	Description:	This function will save the current settings to the ini file
//
// 	Returns:		TMSETUP_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMSetupCtrl::Save() 
{
	BOOL bWriteError = FALSE;

	//	The control has to be initialized
	if(m_pTabs == 0)
	{
		m_Errors.Handle(0, IDS_TMSETUP_NOTINITIALIZED);
		return TMSETUP_NOTINITIALIZED;
	}

	//	Notify each of the pages
	for(int i = 0; i < MAX_PAGES; i++)
	{
		if(m_Pages[i]->WriteOptions(m_Ini) == FALSE)
			bWriteError = TRUE;
	}
	return (bWriteError) ? TMSETUP_INVALIDSETUPDATA : TMSETUP_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMSetupCtrl::SetActiveFilters()
//
// 	Description:	This function is called to set the active filters and
//					media control interface used by the diagnostic page to 
//					display active multimedia filters
//
// 	Returns:		TMSETUP_NOERROR if successful
//
//	Notes:			It is assumed that the active filters and media control
//					interface are obtained from an active TMMovie control.
//
//==============================================================================
short CTMSetupCtrl::SetActiveFilters(LPCTSTR lpFilters, LPUNKNOWN lpMediaControl) 
{
	//	Set the filters in the DirectX page
	m_DirectX.SetActiveFilters(lpFilters, lpMediaControl);

	return TMSETUP_NOERROR;
}


