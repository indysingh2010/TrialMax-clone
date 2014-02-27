//==============================================================================
//
// File Name:	tmbars.cpp
//
// Description:	This file contains member functions of the CTMBarsCtrl class.
//
// See Also:	tmbars.h
//
// Copyright	FTI Consulting 1997-2000
//
//==============================================================================
//	Date		Revision    Description
//	01-09-00	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmbarsap.h>
#include <tmbars.h>
#include <tmbarspg.h>
#include <tmbadefs.h>
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
extern CTMBarsApp NEAR theApp;

/* Replace 2 */
const IID BASED_CODE IID_DTMBars6 =
		{ 0x22e390ad, 0xe5e, 0x42a2, { 0x90, 0x70, 0xad, 0x26, 0x65, 0xc2, 0x52, 0xe } };
/* Replace 3 */
const IID BASED_CODE IID_DTMBars6Events =
		{ 0x5aeece8f, 0xec7d, 0x427d, { 0x87, 0x28, 0x67, 0xe1, 0x51, 0x72, 0x53, 0x8c } };

// Control type information
static const DWORD BASED_CODE _dwTMBarsOleMisc =
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
BEGIN_MESSAGE_MAP(CTMBarsCtrl, COleControl)
	//{{AFX_MSG_MAP(CTMBarsCtrl)
	ON_WM_CREATE()
	ON_WM_DESTROY()
	ON_WM_SETFOCUS()
	//}}AFX_MSG_MAP
	ON_NOTIFY(TCN_SELCHANGE, IDC_TABS, OnTabChange)
	ON_NOTIFY(TCN_SELCHANGING, IDC_TABS, OnTabChanging)
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
END_MESSAGE_MAP()

// Dispatch map
BEGIN_DISPATCH_MAP(CTMBarsCtrl, COleControl)
	//{{AFX_DISPATCH_MAP(CTMBarsCtrl)
	DISP_PROPERTY_NOTIFY(CTMBarsCtrl, "AutoInit", m_bAutoInit, OnAutoInitChanged, VT_BOOL)
	DISP_PROPERTY_EX(CTMBarsCtrl, "VerBuild", GetVerBuild, SetNotSupported, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMBarsCtrl, "EnableErrors", m_bEnableErrors, OnEnableErrorsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMBarsCtrl, "IniFile", m_strIniFile, OnIniFileChanged, VT_BSTR)
	DISP_PROPERTY_EX(CTMBarsCtrl, "VerMajor", GetVerMajor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMBarsCtrl, "VerMinor", GetVerMinor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMBarsCtrl, "VerTextLong", GetVerTextLong, SetNotSupported, VT_BSTR)
	DISP_FUNCTION(CTMBarsCtrl, "Initialize", Initialize, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMBarsCtrl, "Save", Save, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMBarsCtrl, "GetClassIdString", GetClassIdString, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMBarsCtrl, "GetRegisteredPath", GetRegisteredPath, VT_BSTR, VTS_NONE)
	//}}AFX_DISPATCH_MAP

	//	Added rev 6.1.0
	DISP_PROPERTY_EX_ID(CTMBarsCtrl, "VerTextShort", DISPID_VERTEXTSHORT, GetVerTextShort, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_EX_ID(CTMBarsCtrl, "VerQEF", DISPID_VERQEF, GetVerQEF, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX_ID(CTMBarsCtrl, "VerBuildDate", DISPID_VERBUILDDATE, GetVerBuildDate, SetNotSupported, VT_BSTR)

END_DISPATCH_MAP()

// Event map
BEGIN_EVENT_MAP(CTMBarsCtrl, COleControl)
	//{{AFX_EVENT_MAP(CTMBarsCtrl)
	// NOTE - ClassWizard will add and remove event map entries
	//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_EVENT_MAP
END_EVENT_MAP()

// Property pages
BEGIN_PROPPAGEIDS(CTMBarsCtrl, 1)
	PROPPAGEID(CTMBarsProperties::guid)
END_PROPPAGEIDS(CTMBarsCtrl)

/* Replace 4 */
IMPLEMENT_OLECREATE_EX(CTMBarsCtrl, "TMBARS6.TMBarsCtrl.1",
	0x5284e5b7, 0x9e77, 0x4200, 0x9e, 0x9f, 0xd5, 0xf2, 0x2c, 0xb4, 0xf, 0x2c)

// Type library ID and version
IMPLEMENT_OLETYPELIB(CTMBarsCtrl, _tlid, _wVerMajor, _wVerMinor)
IMPLEMENT_OLECTLTYPE(CTMBarsCtrl, IDS_TMBARS, _dwTMBarsOleMisc)

IMPLEMENT_DYNCREATE(CTMBarsCtrl, COleControl)

// Interface map for IObjectSafety
BEGIN_INTERFACE_MAP(CTMBarsCtrl, COleControl )
	INTERFACE_PART(CTMBarsCtrl, IID_IObjectSafety, ObjSafety)
END_INTERFACE_MAP()

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::CTMBarsCtrlFactory::UpdateRegistry
//
// 	Description:	Adds or removes system registry entries for this control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTMBarsCtrl::CTMBarsCtrlFactory::UpdateRegistry(BOOL bRegister)
{
	BOOL	bReturn;
	HRESULT	hResult;

	if(bRegister)
	{
		bReturn = AfxOleRegisterControlClass(AfxGetInstanceHandle(),
											 m_clsid,
											 m_lpszProgID,
											 IDS_TMBARS,
											 IDB_TMBARS,
											 afxRegApartmentThreading,
											 _dwTMBarsOleMisc,
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
// 	Function Name:	CTMBarsCtrl::XObjSafety::AddRef()
//
// 	Description:	This function is called to attach to the nested
//					IObjectSafety dispatch interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMBarsCtrl::XObjSafety::AddRef()
{
    METHOD_PROLOGUE(CTMBarsCtrl, ObjSafety)
    return pThis->ExternalAddRef();
}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::XObjSafety::GetInterfaceSafetyOptions()
//
// 	Description:	This function is called to get this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMBarsCtrl::XObjSafety::GetInterfaceSafetyOptions( 
		/* [in]  */ REFIID riid,
        /* [out] */ DWORD __RPC_FAR *pdwSupportedOptions,
        /* [out] */ DWORD __RPC_FAR *pdwEnabledOptions)
{
	HRESULT			hReturn;
	IUnknown FAR*	pInterface;

	METHOD_PROLOGUE(CTMBarsCtrl, ObjSafety)

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
// 	Function Name:	CTMBarsCtrl::XObjSafety::QueryInterface()
//
// 	Description:	This function is called to query the IObjectSafety interface
//					for the requested method/property.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT FAR EXPORT CTMBarsCtrl::XObjSafety::QueryInterface(REFIID iid, 
														  void FAR* FAR* ppvObj)
{
    METHOD_PROLOGUE(CTMBarsCtrl, ObjSafety)
    return (HRESULT)pThis->ExternalQueryInterface(&iid, ppvObj);
}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::XObjSafety::Release()
//
// 	Description:	This function is called to detach from the IObjectSafety
//					dispatch interface.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMBarsCtrl::XObjSafety::Release()
{
    METHOD_PROLOGUE(CTMBarsCtrl, ObjSafety)
    return pThis->ExternalRelease();
}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::XObjSafety::SetInterfaceSafetyOptions()
//
// 	Description:	This function is called to set this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMBarsCtrl::XObjSafety::SetInterfaceSafetyOptions( 
        /* [in] */ REFIID riid,
        /* [in] */ DWORD dwOptionSetMask,
        /* [in] */ DWORD dwEnabledOptions)
{
	IUnknown FAR* pInterface;

    METHOD_PROLOGUE(CTMBarsCtrl, ObjSafety)
	
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
// 	Function Name:	CTMBarsCtrl::AddPage()
//
// 	Description:	This function will add the requested toolbar configuration
//					page to the property sheet
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBarsCtrl::AddPage(int iPage)
{
	TC_ITEM	tciTab;
	char	szLabel[64];
	
	//	Copy the appropriate label to our local buffer
	lstrcpyn(szLabel, m_Labels[iPage], sizeof(szLabel));

	//	Initialize the tab information structure
	tciTab.mask = TCIF_TEXT;
	tciTab.iImage = -1;
	tciTab.cchTextMax = lstrlen(szLabel);
	tciTab.pszText = szLabel;

	//	Create the new page
	VERIFY(m_Pages[iPage].Create(IDD_BAR_PAGE, m_pTabs));

	//	Set the tab text for the page. 
	//
	//	Note:	For some reason, if we fail to send the WM_NCACTIVATE messaqe
	//			after inserting the text, edit controls will not give up the
	//			focus when another edit control is clicked on.
	m_pTabs->InsertItem(iPage, &tciTab);
	m_Pages[iPage].SendMessage(WM_NCACTIVATE, TRUE);
}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::CalculateSize()
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
void CTMBarsCtrl::CalculateSize()
{
	CDialog SizeDialog;

	//	This is the size of the control window in dialog units. These values
	//	were calculated by trial and error during development.
	int iDlgWidth  = 334;
	int iDlgHeight = 224;

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
// 	Function Name:	CTMBarsCtrl::CheckVersion()
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
BOOL CTMBarsCtrl::CheckVersion(DWORD dwVersion)
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
	              "TMBars ActiveX control. You should upgrade tm_bars6.ocx "
				  "as soon as possible\n\n%s", 
				  (wMinor > _wVerMinor) ? "a newer" : "an older", strVersion);

	MessageBeep(MB_ICONEXCLAMATION);
	MessageBox(strMsg, "TrialMax Error", MB_ICONEXCLAMATION | MB_OK);

	//	The versions do not match
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::CTMBarsCtrl()
//
// 	Description:	This is the constructor for CTMBarsCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMBarsCtrl::CTMBarsCtrl()
{
	InitializeIIDs(&IID_DTMBars6, &IID_DTMBars6Events);

	//	Initialize the local data
	m_pTabs		 = 0;
	m_pPage		 = 0;
	m_iHeight	 = 0;
	m_iWidth	 = 0;

	//	Calculate the size of the options dialog
	CalculateSize();

	//	Initialize the strings used to label the page tabs
	m_Labels[DOCUMENT_PAGE] = DOCUMENT_LABEL;
	m_Labels[GRAPHIC_PAGE] = GRAPHIC_LABEL;
	m_Labels[PLAYLIST_PAGE] = PLAYLIST_LABEL;
	m_Labels[LINK_PAGE] = LINK_LABEL;
	m_Labels[MOVIE_PAGE] = MOVIE_LABEL;
	m_Labels[POWERPOINT_PAGE] = POWERPOINT_LABEL;

	//	Initialize the strings used to label the page tabs
	m_Sections[DOCUMENT_PAGE] = TMBARS_DOCUMENT_SECTION;
	m_Sections[GRAPHIC_PAGE] = TMBARS_GRAPHIC_SECTION;
	m_Sections[PLAYLIST_PAGE] = TMBARS_PLAYLIST_SECTION;
	m_Sections[LINK_PAGE] = TMBARS_LINK_SECTION;
	m_Sections[MOVIE_PAGE] = TMBARS_MOVIE_SECTION;
	m_Sections[POWERPOINT_PAGE] = TMBARS_POWERPOINT_SECTION;

	//	Get the registry information
	GetRegistration();
}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::~CTMBarsCtrl()
//
// 	Description:	This is the destructor for CTMBarsCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMBarsCtrl::~CTMBarsCtrl()
{
}		

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::DoPropertyExchange()
//
// 	Description:	This function manages the exchange of persistant 
//					properties
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBarsCtrl::DoPropExchange(CPropExchange* pPX)
{
	BOOL bAutoInit = FALSE;
	BOOL bEnableErrors = FALSE;
	BOOL bIniFile = FALSE;

	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
	COleControl::DoPropExchange(pPX);

	//CheckVersion(pPX->GetVersion());

	try
	{
		//	Load the control's persistent properties
		bAutoInit = PX_Bool(pPX, _T("AutoInit"), m_bAutoInit, TMBARS_AUTOINIT);
		bEnableErrors = PX_Bool(pPX, _T("EnableErrors"), m_bEnableErrors, TRUE);
		bIniFile = PX_String(pPX, _T("IniFile"), m_strIniFile, TMBARS_INIFILE);
	}
	catch(...)
	{
		if(!bAutoInit) m_bAutoInit = TMBARS_AUTOINIT;
		if(!bEnableErrors) m_bEnableErrors = TRUE;
		if(!bIniFile) m_strIniFile = TMBARS_INIFILE;
	}
}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::GetClassIdString()
//
// 	Description:	This method is called to get the class GUID as a null
//					terminated string.
//
// 	Returns:		The string equivalent of the CLASS GUID
//
//	Notes:			None
//
//==============================================================================
BSTR CTMBarsCtrl::GetClassIdString() 
{
	CString strClsId = m_tmVersion.GetClsId();
	return strClsId.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::GetRegisteredPath()
//
// 	Description:	This method is called to get the ocx path stored in the
//					system registry.
//
// 	Returns:		The path to the ocx stored in the registry
//
//	Notes:			None
//
//==============================================================================
BSTR CTMBarsCtrl::GetRegisteredPath() 
{
	CString strRegistered = m_tmVersion.GetFileSpec();
	return strRegistered.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::GetRegistration()
//
// 	Description:	This function is called to get the control's registration
//					information.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBarsCtrl::GetRegistration() 
{
	CLSID	clsid;

	//	Get the GUID 
	GetClassID(&clsid);

	//	Initialize the version information
	m_tmVersion.InitFromClsId("TMBars", "Toolbar Setup Control", clsid);

}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::GetVerBuild()
//
// 	Description:	This method is called to get the value of the VerBuild
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMBarsCtrl::GetVerBuild() 
{
	return m_tmVersion.GetBuild();
}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::GetVerBuildDate()
//
// 	Description:	This method is called to get the value of the VerBuildDate
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMBarsCtrl::GetVerBuildDate() 
{
	CString strBuildDate = m_tmVersion.GetBuildDate();
	return strBuildDate.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::GetVerMajor()
//
// 	Description:	This method is called to get the value of the VerMajor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMBarsCtrl::GetVerMajor() 
{
	return m_tmVersion.GetMajor();
}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::GetVerMinor()
//
// 	Description:	This method is called to get the value of the VerMinor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMBarsCtrl::GetVerMinor() 
{
	return m_tmVersion.GetMinor();
}


//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::GetVerQEF()
//
// 	Description:	This method is called to get the value of the VerQEF
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMBarsCtrl::GetVerQEF() 
{
	return m_tmVersion.GetUpdate();
}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::GetVerTextLong()
//
// 	Description:	This method is called to get the value of the VerTextLong
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMBarsCtrl::GetVerTextLong() 
{
	CString strVer = m_tmVersion.GetTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::GetVerTextShort()
//
// 	Description:	This method is called to get the value of the VerTextShort
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMBarsCtrl::GetVerTextShort() 
{
	CString strVer = m_tmVersion.GetShortTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::Initialize()
//
// 	Description:	This function will construct and initialize the lpenbar
//
// 	Returns:		TMDP_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMBarsCtrl::Initialize()
{
	CRect	rcTabs(BORDER, BORDER, m_iWidth - (2*BORDER), m_iHeight - (2*BORDER));
	DWORD	dwTabsStyle = WS_CHILD | WS_VISIBLE | WS_CLIPSIBLINGS;

	//	Don't bother if not in user mode or if the control is already initialized
	if(!AmbientUserMode() || (m_pTabs != 0))
		return TMBARS_NOERROR;

	//	Create the tab control
	m_pTabs = new CTabCtrl;
	ASSERT(m_pTabs);
	if(!m_pTabs->Create(dwTabsStyle, rcTabs, this, IDC_TABS))
		return TMBARS_CREATETABFAILED;
	
	//	Add the pages
	for(int i = 0; i < MAX_PAGES; i++)
	{
		AddPage(i);
		m_Pages[i].SetId(i);
		m_Pages[i].SetButtonMask(i);
		m_Pages[i].SetHandler(&m_Errors);
		m_Pages[i].SetSection(m_Sections[i]);
	}

	//	Set the font of the tab control to match that of the pages
	m_pTabs->SetFont(m_Pages[0].GetFont(), TRUE);

	//	Set the focus to the first page
	OnTabChange(0, 0);

	//	Initialize from the ini file
	OnIniFileChanged();

	//	Has the control already been initialized
	return TMBARS_NOERROR;
}	

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::OnAutoInitChanged()
//
// 	Description:	This function is called when the AutoInit property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBarsCtrl::OnAutoInitChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::OnCreate()
//
// 	Description:	This fuction handles all WM_CREATE messages
//
// 	Returns:		0 if successful
//
//	Notes:			None
//
//==============================================================================
int CTMBarsCtrl::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
	//	Do the base class processing first
	if(COleControl::OnCreate(lpCreateStruct) == -1)
		return -1;

	//	Initialize the error handler
	m_Errors.Enable(m_bEnableErrors);
	m_Errors.SetParent(m_hWnd);
	m_Errors.SetTitle("TMBars Error");
	
	//	Initialize the control
	if(m_bAutoInit)
		Initialize();
	
	return 0;
}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::OnDestroy()
//
// 	Description:	This function is called when the window is destroyed
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBarsCtrl::OnDestroy() 
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
// 	Function Name:	CTMBarsCtrl::OnDraw()
//
// 	Description:	This function is called in response to WM_PAINT messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBarsCtrl::OnDraw(CDC* pdc, const CRect& rcBounds,const CRect& rcInvalid)
{
	CBrush brBackground;

	//	Are we in user mode?
	if(AmbientUserMode())
	{
		//	Paint the window only if the tab control has not been created
		//if((m_pTabs == 0) || !IsWindow(m_pTabs->m_hWnd))
		//{
			brBackground.CreateSolidBrush(GetSysColor(COLOR_BTNFACE));
			pdc->FillRect(rcBounds, &brBackground);
		//}
	}
	else
	{
		CString	strText;
		CRect ControlRect = rcBounds;

		strText.Format("FTI Toolbar Configuration Control (rev. %d.%d)", _wVerMajor, _wVerMinor);

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
// 	Function Name:	CTMBarsCtrl::OnEnableErrorsChanged()
//
// 	Description:	This function is called when the EnableErrors property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBarsCtrl::OnEnableErrorsChanged() 
{
	m_Errors.Enable(m_bEnableErrors);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::OnIniFileChanged()
//
// 	Description:	This function is called when the IniFile property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBarsCtrl::OnIniFileChanged() 
{
	SetModifiedFlag();

	//	Don't bother if we're in design mode or if the control has not been
	//	initialized yet
	if(!AmbientUserMode() || (m_pTabs == 0))
		return;

	//	Attempt to open the file
	if(m_Ini.Open(m_strIniFile, TMBARS_DOCUMENT_SECTION))
	{
		//	Read the information for each of the pages
		for(int i = 0; i < MAX_PAGES; i++)
			m_Pages[i].ReadIniFile(&m_Ini);
	}
	else
	{
		m_Errors.Handle("", IDS_TMBARS_ININOTFOUND, m_Ini.strFileSpec);
	}
}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::OnSetExtent()
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
BOOL CTMBarsCtrl::OnSetExtent(LPSIZEL lpSizeL) 
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
// 	Function Name:	CTMBarsCtrl::OnSetFocus()
//
// 	Description:	This function is called when the control gains the keyboard
//					focus.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBarsCtrl::OnSetFocus(CWnd* pOldWnd) 
{
	COleControl::OnSetFocus(pOldWnd);
	
	//	Set the focus to the current page
	if(m_pPage)
		m_pPage->SetFocus();
}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::OnTabChange()
//
// 	Description:	This function will enable the page selected by the user.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBarsCtrl::OnTabChange(NMHDR* pnmhdr, LRESULT* pResult)
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
	if((m_pPage = &(m_Pages[iTab])) == 0)
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
// 	Function Name:	CTMBarsCtrl::OnTabChanging()
//
// 	Description:	This function will hide the current page when the user
//					tabs to a new page.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMBarsCtrl::OnTabChanging(NMHDR* pnmhdr, LRESULT* pResult)
{
	CDialog* pPage;

	//	Stop here if no tab is selected
	if(m_pTabs->GetCurSel() < 0)
		return;

	//	Get the current page
	if((pPage = &(m_Pages[m_pTabs->GetCurSel()])) == 0)
	{
		if(pResult)
			*pResult = TRUE;	//	This prevents change from occuring
		return;
	}

	//	Hide this page
	pPage->ShowWindow(SW_HIDE);

	if(pResult)
		*pResult = FALSE;
}

//==============================================================================
//
// 	Function Name:	CTMBarsCtrl::PreTranslateMessage()
//
// 	Description:	This function is called trap all messages sent to the
//					control window.
//
// 	Returns:		TRUE if the message is handled
//
//	Notes:			None
//
//==============================================================================
BOOL CTMBarsCtrl::PreTranslateMessage(MSG* pMsg) 
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
// 	Function Name:	CTMBarsCtrl::Save()
//
// 	Description:	This method is called to save the new configuration 
//					information to the current ini file.
//
// 	Returns:		TMBARS_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMBarsCtrl::Save() 
{
	//	The control has to be initialized
	if(m_pTabs == 0)
		return TMBARS_NOTINITIALIZED;

	//	Write the information for each of the pages
	for(int i = 0; i < MAX_PAGES; i++)
		m_Pages[i].WriteIniFile(&m_Ini);
	
	return TMBARS_NOERROR;
}





