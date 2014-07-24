//==============================================================================
//
// File Name:	tmtool.cpp
//
// Description:	This file contains member functions of the CTMToolCtrl class.
//
// See Also:	tmtool.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	04-21-98	1.00		Original Release
//  03-25-2014	7.0.31		Added methods to find button postion. Modified large
//                          toolbar to add button according to screen resolution
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmtoolap.h>
#include <tmtool.h>
#include <tmtoolpg.h>
#include <config.h>
#include <tables.h>
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
static 	short m_sTBID = TMTOOLBAR_ID;
extern CTMToolApp NEAR theApp;

/* Replace 2 */
const IID BASED_CODE IID_DTMTool6 =
		{ 0x3cbdf435, 0x832a, 0x4a4d, { 0x80, 0x5, 0xb7, 0x32, 0x17, 0x6, 0x41, 0xf7 } };
/* Replace 3 */
const IID BASED_CODE IID_DTMTool6Events =
		{ 0x9259569a, 0x473c, 0x416a, { 0xa7, 0x80, 0xfc, 0x65, 0x5c, 0xfd, 0xe9, 0x23 } };

// Control type information
static const DWORD BASED_CODE _dwTMToolOleMisc =
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
static int m_Counter = 0;
static RECT m_ScreenResolution;

//------------------------------------------------------------------------------
//	MAPS
//------------------------------------------------------------------------------

// Message map
BEGIN_MESSAGE_MAP(CTMToolCtrl, COleControl)
	//{{AFX_MSG_MAP(CTMToolCtrl)
	ON_WM_CREATE()
	ON_WM_MOUSEACTIVATE()
	
	ON_WM_LBUTTONUP()
	ON_WM_SIZE()
	ON_WM_HSCROLL()
	//}}AFX_MSG_MAP
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
	ON_COMMAND_RANGE(TMTB_COMMANDOFFSET, (TMTB_COMMANDOFFSET + TMTB_MAXBUTTONS), OnButtonClick)
	ON_WM_LBUTTONDOWN()
END_MESSAGE_MAP()

// Dispatch map
BEGIN_DISPATCH_MAP(CTMToolCtrl, COleControl)
	//{{AFX_DISPATCH_MAP(CTMToolCtrl)
	DISP_PROPERTY_NOTIFY(CTMToolCtrl, "IniFile", m_strIniFile, OnIniFileChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMToolCtrl, "Orientation", m_sOrientation, OnOrientationChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMToolCtrl, "ButtonSize", m_sButtonSize, OnButtonSizeChanged, VT_I2)
	DISP_PROPERTY_EX(CTMToolCtrl, "VerBuild", GetVerBuild, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMToolCtrl, "VerMajor", GetVerMajor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMToolCtrl, "VerMinor", GetVerMinor, SetNotSupported, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMToolCtrl, "AutoInit", m_bAutoInit, OnAutoInitChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMToolCtrl, "EnableErrors", m_bEnableErrors, OnEnableErrorsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMToolCtrl, "Style", m_sStyle, OnStyleChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMToolCtrl, "Stretch", m_bStretch, OnStretchChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMToolCtrl, "ButtonMask", m_strButtonMask, OnButtonMaskChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMToolCtrl, "ToolTips", m_bToolTips, OnToolTipsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMToolCtrl, "Configurable", m_bConfigurable, OnConfigurableChanged, VT_BOOL)
	DISP_PROPERTY_EX(CTMToolCtrl, "VerTextLong", GetVerTextLong, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMToolCtrl, "IniSection", m_strIniSection, OnIniSectionChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMToolCtrl, "ButtonRows", m_sButtonRows, OnButtonRowsChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMToolCtrl, "AutoReset", m_bAutoReset, OnAutoResetChanged, VT_BOOL)
	DISP_FUNCTION(CTMToolCtrl, "GetBarWidth", GetBarWidth, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMToolCtrl, "GetBarHeight", GetBarHeight, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMToolCtrl, "SetButtonImage", SetButtonImage, VT_I2, VTS_I2 VTS_I2)
	DISP_FUNCTION(CTMToolCtrl, "Initialize", Initialize, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMToolCtrl, "ResetFrame", ResetFrame, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMToolCtrl, "SetColorButton", SetColorButton, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMToolCtrl, "SetToolButton", SetToolButton, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMToolCtrl, "IsButton", IsButton, VT_BOOL, VTS_I2)
	DISP_FUNCTION(CTMToolCtrl, "GetButtonLabel", GetButtonLabel, VT_BSTR, VTS_I2)
	DISP_FUNCTION(CTMToolCtrl, "SetButtonMap", SetButtonMap, VT_I2, VTS_PI2)
	DISP_FUNCTION(CTMToolCtrl, "SetPlayButton", SetPlayButton, VT_I2, VTS_BOOL)
	DISP_FUNCTION(CTMToolCtrl, "SetSplitButton", SetSplitButton, VT_I2, VTS_BOOL VTS_BOOL)
	DISP_FUNCTION(CTMToolCtrl, "SetLinkButton", SetLinkButton, VT_I2, VTS_BOOL)
	DISP_FUNCTION(CTMToolCtrl, "Configure", Configure, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMToolCtrl, "SetButtonLabel", SetButtonLabel, VT_I2, VTS_I2 VTS_BSTR)
	DISP_FUNCTION(CTMToolCtrl, "CheckButton", CheckButton, VT_I2, VTS_I2 VTS_BOOL)
	DISP_FUNCTION(CTMToolCtrl, "EnableButton", EnableButton, VT_I2, VTS_I2 VTS_BOOL)
	DISP_FUNCTION(CTMToolCtrl, "HideButton", HideButton, VT_I2, VTS_I2 VTS_BOOL)
	DISP_FUNCTION(CTMToolCtrl, "Popup", Popup, VT_I2, VTS_HANDLE)
	DISP_FUNCTION(CTMToolCtrl, "GetImageIndex", GetImageIndex, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMToolCtrl, "GetButtonId", GetButtonId, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMToolCtrl, "SetShapeButton", SetShapeButton, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMToolCtrl, "GetButtonMap", GetButtonMap, VT_I2, VTS_PI2)
	DISP_FUNCTION(CTMToolCtrl, "Save", Save, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMToolCtrl, "GetClassIdString", GetClassIdString, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMToolCtrl, "GetRegisteredPath", GetRegisteredPath, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMToolCtrl, "Reset", Reset, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMToolCtrl, "SetZoomButton", SetZoomButton, VT_I2, VTS_BOOL VTS_BOOL)
	DISP_FUNCTION(CTMToolCtrl, "GetSortOrder", GetSortOrder, VT_I2, VTS_PI2)
	DISP_FUNCTION(CTMToolCtrl, "GetSortedId", GetSortedId, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMToolCtrl, "GetButtonActualWidth", GetButtonActualWidth, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMToolCtrl, "GetBarXPosition", GetBarXPosition, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMToolCtrl, "GetButtonXPosition", GetButtonXPosition, VT_I2, VTS_I2)	
	DISP_STOCKPROP_BACKCOLOR()
	DISP_STOCKPROP_ENABLED()
	//}}AFX_DISPATCH_MAP
	DISP_FUNCTION_ID(CTMToolCtrl, "AboutBox", DISPID_ABOUTBOX, AboutBox, VT_EMPTY, VTS_NONE)

	//	Added rev 5.2
	DISP_PROPERTY_NOTIFY_ID(CTMToolCtrl, "UseSystemBackground", DISPID_USESYSTEMBACKGROUND, m_bUseSystemBackground, OnUseSystemBackgroundChanged, VT_BOOL)

	//	Added rev 6.1.0
	DISP_PROPERTY_EX_ID(CTMToolCtrl, "VerTextShort", DISPID_VERTEXTSHORT, GetVerTextShort, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_EX_ID(CTMToolCtrl, "VerQEF", DISPID_VERQEF, GetVerQEF, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX_ID(CTMToolCtrl, "VerBuildDate", DISPID_VERBUILDDATE, GetVerBuildDate, SetNotSupported, VT_BSTR)

END_DISPATCH_MAP()

// Event map
BEGIN_EVENT_MAP(CTMToolCtrl, COleControl)
	//{{AFX_EVENT_MAP(CTMToolCtrl)
	EVENT_CUSTOM("ButtonClick", FireButtonClick, VTS_I2  VTS_BOOL)
	EVENT_CUSTOM("Reconfigure", FireReconfigure, VTS_NONE)
	//}}AFX_EVENT_MAP
	EVENT_CUSTOM_ID("SetRedButton", eventidSetRedButton, SetRedButton, VTS_UI4)
END_EVENT_MAP()

// Property pages
BEGIN_PROPPAGEIDS(CTMToolCtrl, 2)
	PROPPAGEID(CTMToolProperties::guid)
	PROPPAGEID(CLSID_CColorPropPage)
END_PROPPAGEIDS(CTMToolCtrl)

/* Replace 4 */
IMPLEMENT_OLECREATE_EX(CTMToolCtrl, "TMTOOL6.TMToolCtrl.1",
	0x2341b5a2, 0x769b, 0x49cc, 0x86, 0x52, 0xb8, 0x91, 0x49, 0x92, 0xaf, 0xb1)


// Type library ID and version
IMPLEMENT_OLETYPELIB(CTMToolCtrl, _tlid, _wVerMajor, _wVerMinor)
IMPLEMENT_OLECTLTYPE(CTMToolCtrl, IDS_TMTOOL, _dwTMToolOleMisc)

IMPLEMENT_DYNCREATE(CTMToolCtrl, COleControl)

// Interface map for IObjectSafety
BEGIN_INTERFACE_MAP(CTMToolCtrl, COleControl )
	INTERFACE_PART(CTMToolCtrl, IID_IObjectSafety, ObjSafety)
END_INTERFACE_MAP()

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::CTMToolCtrlFactory::UpdateRegistry
//
// 	Description:	Adds or removes system registry entries for this control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTMToolCtrl::CTMToolCtrlFactory::UpdateRegistry(BOOL bRegister)
{
	BOOL	bReturn;
	HRESULT	hResult;

	if(bRegister)
	{
		bReturn = AfxOleRegisterControlClass(AfxGetInstanceHandle(),
											 m_clsid,
											 m_lpszProgID,
											 IDS_TMTOOL,
											 IDB_TMTOOL,
											 afxRegApartmentThreading,
											 _dwTMToolOleMisc,
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
// 	Function Name:	CTMToolCtrl::XObjSafety::AddRef()
//
// 	Description:	This function is called to attach to the nested
//					IObjectSafety dispatch interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMToolCtrl::XObjSafety::AddRef()
{
    METHOD_PROLOGUE(CTMToolCtrl, ObjSafety)
    return pThis->ExternalAddRef();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::XObjSafety::GetInterfaceSafetyOptions()
//
// 	Description:	This function is called to get this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMToolCtrl::XObjSafety::GetInterfaceSafetyOptions( 
		/* [in]  */ REFIID riid,
        /* [out] */ DWORD __RPC_FAR *pdwSupportedOptions,
        /* [out] */ DWORD __RPC_FAR *pdwEnabledOptions)
{
	HRESULT			hReturn;
	IUnknown FAR*	pInterface;

	METHOD_PROLOGUE(CTMToolCtrl, ObjSafety)

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
// 	Function Name:	CTMToolCtrl::XObjSafety::QueryInterface()
//
// 	Description:	This function is called to query the IObjectSafety interface
//					for the requested method/property.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT FAR EXPORT CTMToolCtrl::XObjSafety::QueryInterface(REFIID iid, 
														  void FAR* FAR* ppvObj)
{
    METHOD_PROLOGUE(CTMToolCtrl, ObjSafety)
    return (HRESULT)pThis->ExternalQueryInterface(&iid, ppvObj);
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::XObjSafety::Release()
//
// 	Description:	This function is called to detach from the IObjectSafety
//					dispatch interface.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMToolCtrl::XObjSafety::Release()
{
    METHOD_PROLOGUE(CTMToolCtrl, ObjSafety)
    return pThis->ExternalRelease();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::XObjSafety::SetInterfaceSafetyOptions()
//
// 	Description:	This function is called to set this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMToolCtrl::XObjSafety::SetInterfaceSafetyOptions( 
        /* [in] */ REFIID riid,
        /* [in] */ DWORD dwOptionSetMask,
        /* [in] */ DWORD dwEnabledOptions)
{
	IUnknown FAR* pInterface;

    METHOD_PROLOGUE(CTMToolCtrl, ObjSafety)
	
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
// 	Function Name:	CTMToolCtrl::AboutBox()
//
// 	Description:	This function will display the control's about box when
//					called.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::AboutBox()
{
	CDialog dlgAbout(IDD_ABOUTBOX_TMTOOL, this);
	dlgAbout.DoModal();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::AddButtons()
//
// 	Description:	This function will add the buttons to the toolbar when
//					it is initialized
//
// 	Returns:		TMTB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::AddButtons()
{	
	TBBUTTON	TBButtons[TMTB_MAXBUTTONS];
	char		szMask[TMTB_MAXBUTTONS + 1];
	char		tmp[512];

	//	Don't bother if not in user mode
	if(!AmbientUserMode())
		return;
	
	// get number of button that can be added according to resolution
	short numberOfButtonsToAdd = GetNumberOfButtonsToAdd();

	//	Clear the button information
	memset(&TBButtons, 0, sizeof(TBButtons));

	//	Transfer the mask to the local buffer
	lstrcpyn(szMask, m_strButtonMask, sizeof(szMask));

	//	Right now we don't have any buttons in the toolbar
	m_sButtons = 0;

	//	Fill the button information structures for those buttons to be added
	//	to the toolbar
	
	for(int i = 0; i < TMTB_MAXBUTTONS; i++)
	{
		//	Have we run out of buttons?
		if((m_aMap[i] < 0) || (m_aMap[i] >= TMTB_MAXBUTTONS))
			break;
		
		// //if large toolbar than only add 22 buttons
		/*if( i > 22 && this->m_sButtonSize == 2)
			break;*/

		//	Is this button eliminated?
		if(szMask[m_aMap[i]] == '0')
			continue;
		
		// eliminate the color buttons except yellow button because it is a color picker
		if(m_aMap[i] == TMTB_RED || m_aMap[i] == TMTB_GREEN || m_aMap[i] == TMTB_BLUE
			|| m_aMap[i] == TMTB_BLACK || m_aMap[i] == TMTB_WHITE
			|| m_aMap[i] == TMTB_DARKRED ||m_aMap[i] == TMTB_DARKGREEN || m_aMap[i] == TMTB_DARKBLUE
			|| m_aMap[i] == TMTB_LIGHTRED || m_aMap[i] == TMTB_LIGHTGREEN || m_aMap[i] == TMTB_LIGHTBLUE)
			continue;

		// If number of buttons added is required more space than screen resolution
		// than do not add these buttons. So no scroll bar will be shown
		if((i+1) > numberOfButtonsToAdd)
			continue;

		//	Set the information for this button
		TBButtons[m_sButtons].iBitmap   = GetImageIndex(m_aMap[i]);	
		TBButtons[m_sButtons].idCommand = TMTB_COMMANDOFFSET + m_aMap[i];
		TBButtons[m_sButtons].fsState   = TBSTATE_ENABLED;
		
		if(IsCheckButton(m_aMap[i]))
			TBButtons[m_sButtons].fsStyle = TBSTYLE_CHECK;
		else
			TBButtons[m_sButtons].fsStyle = TBSTYLE_BUTTON;


		//	We've added a new button
		m_sButtons++;
	}		
		
	//	Add the buttons	
	m_Toolbar.AddButtons(m_sButtons, TBButtons);
}	

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::AddExtraLargeButtons()
//
// 	Description:	This function will add the large buttons to the Extra Toolbar when
//					it is initialized
//
// 	Returns:		TMTB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::AddExtraLargeButtons()
{	
	char tmp[512];
	TBBUTTON	TBButtons[TMTB_MAXBUTTONS];
	char		szMask[TMTB_MAXBUTTONS + 1];

	//	Don't bother if not in user mode
	if(!AmbientUserMode())
		return;

	//	Clear the button information
	memset(&TBButtons, 0, sizeof(TBButtons));

	//	Transfer the mask to the local buffer
	lstrcpyn(szMask, m_strButtonMask, sizeof(szMask));

	//	Right now we don't have any buttons in the toolbar
	m_sButtons = 0;

	//	Fill the button information structures for those buttons to be added
	//	to the toolbar
	for(int i = 0; i < TMTB_MAXBUTTONS; i++)
	{
		//	Have we run out of buttons?
		if((m_aMap[i] < 0) || (m_aMap[i] >= TMTB_MAXBUTTONS))
			break;

		//	Is this button eliminated? or add only the eight colour buttons out of 30
		if(szMask[m_aMap[i]] == '0' || i < 22)
			continue;

		//	Set the information for this button
		TBButtons[m_sButtons].iBitmap   = GetImageIndex(m_aMap[i]);	
		TBButtons[m_sButtons].idCommand = TMTB_COMMANDOFFSET + m_aMap[i];
		TBButtons[m_sButtons].fsState   = TBSTATE_ENABLED;

		TBButtons[m_sButtons].fsStyle = TBSTYLE_BUTTON ;

		//	We've added a new button
		m_sButtons++;
	}		

	//	Add the buttons
	m_sExtraButtonsCount = m_sButtons;
	m_Toolbar.AddButtons(m_sButtons, TBButtons);

}	

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::CheckButton()
//
// 	Description:	This method allows the caller to set the check state of
//					a button
//
// 	Returns:		TMTB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::CheckButton(short sId, BOOL bCheck) 
{
	int iState;
	int	iCommandId = sId + TMTB_COMMANDOFFSET;

	if(!IsWindow(m_Toolbar.m_hWnd))
		return TMTB_NOTINITIALIZED;

	if((iState = m_Toolbar.GetState(iCommandId)) < 0)
		return TMTB_INVALIDBUTTONID;

	m_Toolbar.CheckButton(iCommandId, bCheck);
	return TMTB_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::CheckVersion()
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
BOOL CTMToolCtrl::CheckVersion(DWORD dwVersion)
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
	              "TMTool ActiveX control. You should upgrade tm_tool6.ocx "
				  "as soon as possible\n\n%s", 
				  (wMinor > _wVerMinor) ? "a newer" : "an older", strVersion);

	MessageBeep(MB_ICONEXCLAMATION);
	MessageBox(strMsg, "TrialMax Error", MB_ICONEXCLAMATION | MB_OK);

	//	The versions do not match
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::Configure()
//
// 	Description:	This function is called to perform runtime configuration
//					of the toolbar
//
// 	Returns:		TMTB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::Configure()
{
	CConfigure	Config(this, m_aLabels);
	short		sError;

	//	Set up the dialog members
	Config.m_bFlat = (m_sStyle == TMTB_FLAT) ? TRUE : FALSE;
	Config.m_bStretch = m_bStretch;
	Config.m_bToolTips = m_bToolTips;
	Config.m_iRows = m_sButtonRows;
	switch(m_sOrientation)
	{
		case TMTB_TOP:		Config.m_iTop = 0; break;
		case TMTB_BOTTOM:	Config.m_iTop = 1; break;
		case TMTB_LEFT:		Config.m_iTop = 2; break;
		case TMTB_RIGHT:	Config.m_iTop = 3; break;
	}
	switch(m_sButtonSize)
	{
		case TMTB_SMALLBUTTONS:		Config.m_iSize = 0; break;
		case TMTB_MEDIUMBUTTONS:	Config.m_iSize = 1; break;
		case TMTB_LARGEBUTTONS:		Config.m_iSize = 2; break;
	}
	memcpy(Config.m_aMap, m_aMap, sizeof(Config.m_aMap));
	lstrcpyn(Config.m_szMask, m_strButtonMask, sizeof(Config.m_szMask));

	//	Create the configuration dialog
	if(Config.DoModal() == IDCANCEL)
		return TMTB_NOERROR;

	//	Get the new selections
	m_sStyle = (Config.m_bFlat) ? TMTB_FLAT : TMTB_RAISED;
	m_bStretch = Config.m_bStretch;
	m_bToolTips = Config.m_bToolTips;
	m_sButtonRows = Config.m_iRows;
	if(m_sButtonRows < 1)
		m_sButtonRows = 1;
	switch(Config.m_iTop)
	{
		case 0:		m_sOrientation = TMTB_TOP; break;
		case 1:		m_sOrientation = TMTB_BOTTOM; break;
		case 2:		m_sOrientation = TMTB_LEFT; break;
		case 3:		m_sOrientation = TMTB_RIGHT; break;
	}
	switch(Config.m_iSize)
	{
		case 0:		m_sButtonSize = TMTB_SMALLBUTTONS; break;
		case 1:		m_sButtonSize = TMTB_MEDIUMBUTTONS; break;
		case 2:		m_sButtonSize = TMTB_LARGEBUTTONS; break;
	}

	//	Update the button map
	if((sError = SetButtonMap(Config.m_aMap)) == TMTB_NOERROR)
	{
		//	Update the ini file if it's in use
		if(!m_strIniFile.IsEmpty() && !m_strIniSection.IsEmpty())
			WriteIniFile();

		//	Fire the event to alert the container that the toolbar has been
		//	reconfigured
		FireReconfigure();

		return TMTB_NOERROR;
	}
	else
	{
		return sError;
	}
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::CreateToolbar()
//
// 	Description:	This function will create the toolbar window
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMToolCtrl::CreateToolbar()
{
	RECT	Rect;
	SIZE	Size;
	CBitmap	bmEnabled;
	CBitmap	bmDisabled;

	//	Don't bother if not in user mode
	if(!AmbientUserMode())
		return FALSE;

	//	Destroy the existing toolbar
	if(IsWindow(m_Toolbar.m_hWnd))
		m_Toolbar.DestroyWindow();

	//	Delete the existing image lists and allocate a new ones
	if(m_pEnabled)
	{
		delete m_pEnabled;	
		m_pEnabled = 0;
	}
	if(m_pDisabled)
	{
		delete m_pDisabled;
		m_pDisabled = 0;
	}
	m_pEnabled  = new CImageList();
	m_pDisabled = new CImageList();
	ASSERT(m_pEnabled);
	ASSERT(m_pDisabled);

	//	What style of toolbar?
	m_dwStyle = WS_CHILD | WS_VISIBLE | CCS_NORESIZE | CCS_NODIVIDER |
				CCS_NOPARENTALIGN | TBSTYLE_WRAPABLE ;



//	m_dwStyle = WS_CHILD | WS_VISIBLE | CCS_NORESIZE | CCS_NODIVIDER /*|
//		CCS_NOPARENTALIGN | TBSTYLE_WRAPABLE |*/;

	//	Flat buttons?
	if(m_sStyle == TMTB_FLAT)
		m_dwStyle |= TBSTYLE_FLAT;

	//	Flyby tool tips?
	if(m_bToolTips)
		m_dwStyle |= TBSTYLE_TOOLTIPS;

	//	Create the toolbar control
	memset(&Rect, 0, sizeof(Rect));
	if(m_Toolbar.Create(m_dwStyle, Rect, this, m_sTBID++) == -1)
		return FALSE;

	// Modyfying styles to reduce space between 8 color toolbar buttons
	if (m_Counter == 9)
	{
		m_Toolbar.ModifyStyle(WS_DLGFRAME,0);
		m_Toolbar.ModifyStyle(WS_CAPTION,0);
		m_Toolbar.ModifyStyle(WS_EX_WINDOWEDGE,0);
		m_Toolbar.ModifyStyle(WS_BORDER,0);

		m_Toolbar.ModifyStyle(WS_EX_DLGMODALFRAME,0);
		m_Toolbar.ModifyStyle(WS_EX_STATICEDGE,0);
		m_Toolbar.ModifyStyle(0,TBSTYLE_TOOLTIPS);
		m_Toolbar.ModifyStyle(0,BS_FLAT);

	}

	m_Toolbar.SetOwner(this);

	//	Remove this style bit. MFC adds it by default but it screws up drawing
	//	of buttons when the TBSTYLE_FLAT bit is set
	//m_Toolbar.ModifyStyle(TBSTYLE_TRANSPARENT, 0);

	//	Set the size of the bitmaps in the toolbar
	Size.cx = GetBitmapWidth();
	Size.cy = GetBitmapHeight();
	m_Toolbar.SetBitmapSize(Size);

	//	Set the size of the buttons
	Size.cx = GetButtonWidth();
	Size.cy = GetButtonHeight();
	m_Toolbar.SetButtonSize(Size);

	//	Load the bitmaps with no spaces between buttons for 8 color toolbar buttons
	if (m_Counter == 9)
	{
		bmEnabled.LoadBitmap(IDB_COLORTBLARGE);
	}
	else 
		bmEnabled.LoadBitmap(GetEnabledStrip());

	bmDisabled.LoadBitmap(GetDisabledStrip());

	//	Create the image lists
	m_pEnabled->Create(GetBitmapWidth(), GetBitmapHeight(), ILC_MASK | ILC_COLOR24, 0, 1);
	m_pDisabled->Create(GetBitmapWidth(), GetBitmapHeight(), ILC_MASK | ILC_COLOR24, 0, 1);

	//	Add the bitmaps to the image lists
	m_pEnabled->Add(&bmEnabled, RGB(255,0,255));  
	m_pDisabled->Add(&bmDisabled, RGB(255,0,255));
	 
	//	Attach the image lists to the toolbar control
	m_Toolbar.SendMessage(TB_SETIMAGELIST, 0, (LPARAM)m_pEnabled->GetSafeHandle());
	m_Toolbar.SendMessage(TB_SETDISABLEDIMAGELIST, 0, (LPARAM)m_pDisabled->GetSafeHandle());

	return TRUE;
}	

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::CTMToolCtrl()
//
// 	Description:	This is the constructor for CTMToolCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMToolCtrl::CTMToolCtrl()
: myRedButton(0)
{
	InitializeIIDs(&IID_DTMTool6, &IID_DTMTool6Events);

	//	Initialize the local data
	memset(&m_rcBar, 0, sizeof(RECT));
	memset(&m_rcFrame, 0, sizeof(RECT));
	memset(&m_rcButtons, 0, sizeof(RECT));
	m_sButtons = 0;
	m_dwStyle = 0;
	m_pEnabled = 0;
	m_pDisabled = 0;

	//	Initialize the map to contain all buttons
	for(int i = 0; i < TMTB_MAXBUTTONS; i++)
		m_aMap[i] = i;

	//	Initialize the array containing the button text
	m_aLabels[TMTB_CONFIG].LoadString(IDS_TMTB_CONFIG);
	m_aLabels[TMTB_CONFIGTOOLBARS].LoadString(IDS_TMTB_CONFIGTOOLBARS);
	m_aLabels[TMTB_CLEAR].LoadString(IDS_TMTB_CLEAR);
	m_aLabels[TMTB_ROTATECW].LoadString(IDS_TMTB_ROTATECW);
	m_aLabels[TMTB_ROTATECCW].LoadString(IDS_TMTB_ROTATECCW);
	m_aLabels[TMTB_NORMAL].LoadString(IDS_TMTB_NORMAL);
	m_aLabels[TMTB_ZOOM].LoadString(IDS_TMTB_ZOOM);
	m_aLabels[TMTB_ZOOMWIDTH].LoadString(IDS_TMTB_ZOOMWIDTH);
	m_aLabels[TMTB_PAN].LoadString(IDS_TMTB_PAN);
	m_aLabels[TMTB_CALLOUT].LoadString(IDS_TMTB_CALLOUT);
	m_aLabels[TMTB_DRAWTOOL].LoadString(IDS_TMTB_DRAWTOOL);
	m_aLabels[TMTB_HIGHLIGHT].LoadString(IDS_TMTB_HIGHLIGHT);
	m_aLabels[TMTB_REDACT].LoadString(IDS_TMTB_REDACT);
	m_aLabels[TMTB_ERASE].LoadString(IDS_TMTB_ERASE);
	m_aLabels[TMTB_FIRSTPAGE].LoadString(IDS_TMTB_FIRSTPAGE);
	m_aLabels[TMTB_NEXTPAGE].LoadString(IDS_TMTB_NEXTPAGE);
	m_aLabels[TMTB_PREVPAGE].LoadString(IDS_TMTB_PREVPAGE);
	m_aLabels[TMTB_LASTPAGE].LoadString(IDS_TMTB_LASTPAGE);
	m_aLabels[TMTB_SAVEZAP].LoadString(IDS_TMTB_SAVEZAP);
	m_aLabels[TMTB_FIRSTZAP].LoadString(IDS_TMTB_FIRSTZAP);
	m_aLabels[TMTB_PREVZAP].LoadString(IDS_TMTB_PREVZAP);
	m_aLabels[TMTB_NEXTZAP].LoadString(IDS_TMTB_NEXTZAP);
	m_aLabels[TMTB_LASTZAP].LoadString(IDS_TMTB_LASTZAP);
	m_aLabels[TMTB_STARTMOVIE].LoadString(IDS_TMTB_STARTMOVIE);
	m_aLabels[TMTB_BACKMOVIE].LoadString(IDS_TMTB_BACKMOVIE);
	m_aLabels[TMTB_PAUSEMOVIE].LoadString(IDS_TMTB_PAUSEMOVIE);
	m_aLabels[TMTB_PLAYMOVIE].LoadString(IDS_TMTB_PLAYMOVIE);
	m_aLabels[TMTB_FWDMOVIE].LoadString(IDS_TMTB_FWDMOVIE);
	m_aLabels[TMTB_ENDMOVIE].LoadString(IDS_TMTB_ENDMOVIE);
	m_aLabels[TMTB_FIRSTDESIGNATION].LoadString(IDS_TMTB_FIRSTDESIGNATION);
	m_aLabels[TMTB_BACKDESIGNATION].LoadString(IDS_TMTB_BACKDESIGNATION);
	m_aLabels[TMTB_PREVDESIGNATION].LoadString(IDS_TMTB_PREVDESIGNATION);
	m_aLabels[TMTB_STARTDESIGNATION].LoadString(IDS_TMTB_STARTDESIGNATION);
	m_aLabels[TMTB_PAUSEDESIGNATION].LoadString(IDS_TMTB_PAUSEDESIGNATION);
	m_aLabels[TMTB_PLAYDESIGNATION].LoadString(IDS_TMTB_PLAYDESIGNATION);
	m_aLabels[TMTB_NEXTDESIGNATION].LoadString(IDS_TMTB_NEXTDESIGNATION);
	m_aLabels[TMTB_FWDDESIGNATION].LoadString(IDS_TMTB_FWDDESIGNATION);
	m_aLabels[TMTB_LASTDESIGNATION].LoadString(IDS_TMTB_LASTDESIGNATION);
	m_aLabels[TMTB_PRINT].LoadString(IDS_TMTB_PRINT);
	m_aLabels[TMTB_SPLITVERTICAL].LoadString(IDS_TMTB_SPLITVERTICAL);
	m_aLabels[TMTB_SPLITHORIZONTAL].LoadString(IDS_TMTB_SPLITHORIZONTAL);
	m_aLabels[TMTB_DISABLELINKS].LoadString(IDS_TMTB_DISABLELINKS);
	m_aLabels[TMTB_ENABLELINKS].LoadString(IDS_TMTB_ENABLELINKS);
	m_aLabels[TMTB_RED].LoadString(IDS_TMTB_RED);
	m_aLabels[TMTB_GREEN].LoadString(IDS_TMTB_GREEN);
	m_aLabels[TMTB_BLUE].LoadString(IDS_TMTB_BLUE);
	m_aLabels[TMTB_YELLOW].LoadString(IDS_TMTB_YELLOW);
	m_aLabels[TMTB_BLACK].LoadString(IDS_TMTB_BLACK);
	m_aLabels[TMTB_WHITE].LoadString(IDS_TMTB_WHITE);
	m_aLabels[TMTB_DARKRED].LoadString(IDS_TMTB_DARKRED);
	m_aLabels[TMTB_DARKGREEN].LoadString(IDS_TMTB_DARKGREEN);
	m_aLabels[TMTB_DARKBLUE].LoadString(IDS_TMTB_DARKBLUE);
	m_aLabels[TMTB_LIGHTRED].LoadString(IDS_TMTB_LIGHTRED);
	m_aLabels[TMTB_LIGHTGREEN].LoadString(IDS_TMTB_LIGHTGREEN);
	m_aLabels[TMTB_LIGHTBLUE].LoadString(IDS_TMTB_LIGHTBLUE);
	m_aLabels[TMTB_EXIT].LoadString(IDS_TMTB_EXIT);
	m_aLabels[TMTB_PLAYTHROUGH].LoadString(IDS_TMTB_PLAYTHROUGH);
	m_aLabels[TMTB_DELETEANN].LoadString(IDS_TMTB_DELETEANN);
	m_aLabels[TMTB_CUEPGLNNEXT].LoadString(IDS_TMTB_CUEPGLNNEXT);
	m_aLabels[TMTB_CUEPGLNCURRENT].LoadString(IDS_TMTB_CUEPGLNCURRENT);
	m_aLabels[TMTB_SELECT].LoadString(IDS_TMTB_SELECT);
	m_aLabels[TMTB_TEXT].LoadString(IDS_TMTB_TEXT);
	m_aLabels[TMTB_SELECTTOOL].LoadString(IDS_TMTB_SELECTTOOL);
	m_aLabels[TMTB_FREEHAND].LoadString(IDS_TMTB_FREEHAND);
	m_aLabels[TMTB_LINE].LoadString(IDS_TMTB_LINE);
	m_aLabels[TMTB_ARROW].LoadString(IDS_TMTB_ARROW);
	m_aLabels[TMTB_ELLIPSE].LoadString(IDS_TMTB_ELLIPSE);
	m_aLabels[TMTB_RECTANGLE].LoadString(IDS_TMTB_RECTANGLE);
	m_aLabels[TMTB_FILLEDELLIPSE].LoadString(IDS_TMTB_FILLEDELLIPSE);
	m_aLabels[TMTB_FILLEDRECTANGLE].LoadString(IDS_TMTB_FILLEDRECTANGLE);
	m_aLabels[TMTB_FULLSCREEN].LoadString(IDS_TMTB_FULLSCREEN);
	m_aLabels[TMTB_STATUSBAR].LoadString(IDS_TMTB_STATUSBAR);
	m_aLabels[TMTB_POLYLINE].LoadString(IDS_TMTB_POLYLINE);
	m_aLabels[TMTB_POLYGON].LoadString(IDS_TMTB_POLYGON);
	m_aLabels[TMTB_ANNTEXT].LoadString(IDS_TMTB_ANNTEXT);
	m_aLabels[TMTB_UPDATEZAP].LoadString(IDS_TMTB_UPDATEZAP);
	m_aLabels[TMTB_DELETEZAP].LoadString(IDS_TMTB_DELETEZAP);
	m_aLabels[TMTB_ZOOMRESTRICTED].LoadString(IDS_TMTB_ZOOMRESTRICTED);
	m_aLabels[TMTB_SHADEDCALLOUTS].LoadString(IDS_TMTB_SHADEDCALLOUTS);
	m_aLabels[TMTB_SAVESPLITZAP].LoadString(IDS_TMTB_SAVESPLITZAP);
	m_aLabels[TMTB_GESTUREPAN].LoadString(IDS_TMTB_GESTUREPAN);
	m_aLabels[TMTB_BINDERLIST].LoadString(IDS_TMTB_BINDERLIST);

	//	Get the registry information
	GetRegistration();
	::GetWindowRect(::GetDesktopWindow(), &m_ScreenResolution);
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::~CTMToolCtrl()
//
// 	Description:	This is the destructor for CTMToolCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMToolCtrl::~CTMToolCtrl()
{
	if(m_pEnabled)
	{
		delete m_pEnabled;	
		m_pEnabled = 0;
	}
	if(m_pDisabled)
	{
		delete m_pDisabled;
		m_pDisabled = 0;
	}
}		

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::DoPropertyExchange()
//
// 	Description:	This function manages the exchange of persistant 
//					properties
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::DoPropExchange(CPropExchange* pPX)
{
	BOOL bAutoInit = FALSE;
	BOOL bStretch = FALSE;
	BOOL bEnableErrors = FALSE;
	BOOL bToolTips = FALSE;
	BOOL bConfigurable = FALSE;
	BOOL bStyle = FALSE;
	BOOL bButtonSize = FALSE;
	BOOL bOrientation = FALSE;
	BOOL bIniFile = FALSE;
	BOOL bButtonMask = FALSE;
	BOOL bIniSection = FALSE;
	BOOL bButtonRows = FALSE;
	BOOL bAutoReset = FALSE;
	BOOL bUseSystemBackground = FALSE;

	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
	COleControl::DoPropExchange(pPX);

	//CheckVersion(pPX->GetVersion());

	try
	{
		//	Load the control's persistent properties
		bAutoInit = PX_Bool(pPX, _T("AutoInit"), m_bAutoInit, DEFAULT_TBAUTOINIT);
		bStretch = PX_Bool(pPX, _T("Stretch"), m_bStretch, DEFAULT_TBSTRETCH);
		bEnableErrors = PX_Bool(pPX, _T("EnableErrors"), m_bEnableErrors, TRUE);
		bToolTips = PX_Bool(pPX, _T("ToolTips"), m_bToolTips, DEFAULT_TBTOOLTIPS);
		bConfigurable = PX_Bool(pPX, _T("Configurable"), m_bConfigurable, DEFAULT_TBCONFIGURABLE);
		bStyle = PX_Short(pPX, _T("Style"), m_sStyle, DEFAULT_TBSTYLE);
		bButtonSize = PX_Short(pPX, _T("ButtonSize"), m_sButtonSize, DEFAULT_TBBUTTONSIZE);
		bOrientation = PX_Short(pPX, _T("Orientation"), m_sOrientation, DEFAULT_TBORIENTATION);
		bIniFile = PX_String(pPX, _T("IniFile"), m_strIniFile, DEFAULT_TBINIFILENAME);
		bButtonMask = PX_String(pPX, _T("ButtonMask"), m_strButtonMask, DEFAULT_TBMASK);
		bIniSection = PX_String(pPX, _T("IniSection"), m_strIniSection, DEFAULT_TBINISECTION);
		bButtonRows = PX_Short(pPX, _T("ButtonRows"), m_sButtonRows, DEFAULT_TBBUTTONROWS);
		bAutoReset = PX_Bool(pPX, _T("AutoReset"), m_bAutoReset, DEFAULT_TBAUTORESET);
		bUseSystemBackground = PX_Bool(pPX, _T("UseSystemBackground"), m_bUseSystemBackground, DEFAULT_TBUSESYSTEMBACKGROUND);
	}
	catch(...)
	{
		if(!bAutoInit) m_bAutoInit = DEFAULT_TBAUTOINIT;
		if(!bStretch) m_bStretch = DEFAULT_TBSTRETCH;
		if(!bEnableErrors) m_bEnableErrors = TRUE;
		if(!bToolTips) m_bToolTips = DEFAULT_TBTOOLTIPS;
		if(!bConfigurable) m_bConfigurable = DEFAULT_TBCONFIGURABLE;
		if(!bStyle) m_sStyle = DEFAULT_TBSTYLE;
		if(!bButtonSize) m_sButtonSize = DEFAULT_TBBUTTONSIZE;
		if(!bOrientation) m_sOrientation = DEFAULT_TBORIENTATION;
		if(!bIniFile) m_strIniFile = DEFAULT_TMAXINI;
		if(!bButtonMask) m_strButtonMask = DEFAULT_TBMASK;
		if(!bIniSection) m_strIniSection = DEFAULT_TBINISECTION;
		if(!bButtonRows) m_sButtonRows = DEFAULT_TBBUTTONROWS;
		if(!bAutoReset) m_bAutoReset = DEFAULT_TBAUTORESET;
		if(!bUseSystemBackground) m_bUseSystemBackground = DEFAULT_TBUSESYSTEMBACKGROUND;
	}

}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::EnableButton()
//
// 	Description:	This method allows the caller to enable or disable a 
//					button
//
// 	Returns:		TMTB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::EnableButton(short sId, BOOL bEnable) 
{
	int iState;
	int	iCommandId = sId + TMTB_COMMANDOFFSET;

	if(!IsWindow(m_Toolbar.m_hWnd))
		return TMTB_NOTINITIALIZED;

	if((iState = m_Toolbar.GetState(iCommandId)) < 0)
		return TMTB_INVALIDBUTTONID;

	//	Are we enabling the button
	m_Toolbar.EnableButton(iCommandId, bEnable);
	
	return TMTB_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetBarHeight()
//
// 	Description:	This method is called to get the height of the toolbar
//
// 	Returns:		The current height
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::GetBarHeight() 
{
	return (short)(m_rcBar.bottom - m_rcBar.top);
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetBarWidth()
//
// 	Description:	This method is called to get the width of the toolbar
//
// 	Returns:		The current width
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::GetBarWidth() 
{
	return (short)(m_rcBar.right - m_rcBar.left);
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetBitmapHeight()
//
// 	Description:	This function is called to get the height of the bitmaps on
//					the toolbar buttons
//
// 	Returns:		The height in pixels
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::GetBitmapHeight()
{
	switch(m_sButtonSize)
	{
		case TMTB_LARGEBUTTONS:		return 36;
		case TMTB_MEDIUMBUTTONS:	return 27;
		case TMTB_SMALLBUTTONS:
		default:					return 18;
	}
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetBitmapWidth()
//
// 	Description:	This function is called to get the width of the bitmaps on
//					the toolbar buttons
//
// 	Returns:		The width in pixels
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::GetBitmapWidth()
{
	switch(m_sButtonSize)
	{
		case TMTB_LARGEBUTTONS:		return 48;
		case TMTB_MEDIUMBUTTONS:	return 36;
		case TMTB_SMALLBUTTONS:
		default:					return 24;
	}
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetButtonHeight()
//
// 	Description:	This function is called to get the height of the buttons on
//					the toolbar
//
// 	Returns:		The height in pixels
//
//	Notes:			The button height is a function of the bitmap height
//
//==============================================================================
short CTMToolCtrl::GetButtonHeight()
{
	return (GetBitmapHeight() + BUTTON_VERTPADDING);//(2*BUTTON_VERTPADDING));
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetButtonId()
//
// 	Description:	This method is called to get the button associated with the
//					specified index in the image list
//
// 	Returns:		The id of the button if found
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::GetButtonId(short sImageIndex) 
{
	if(sImageIndex >= TMTB_MAXIMAGES || sImageIndex < 0)
		return -1;
	
	for(int i = 0; i < TMTB_MAXBUTTONS; i++)
		if(ImageMap[i] == sImageIndex)
			return i;

	return -1;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetButtonLabel()
//
// 	Description:	This method is called to get the text associated with a
//					button
//
// 	Returns:		TMTB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
BSTR CTMToolCtrl::GetButtonLabel(short sId) 
{
	CString strResult;
	
	//	Is the id valid?
	if(sId < 0 || sId >= TMTB_MAXBUTTONS)
		strResult.Empty();
	else
		strResult = m_aLabels[sId];

	return strResult.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetButtonMap()
//
// 	Description:	This method is called to retrieve a copy of the current
//					button map.
//
// 	Returns:		The number of buttons put in the map
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::GetButtonMap(short* paMap) 
{
	short i = 0;
	
	for(i = 0; i < TMTB_MAXBUTTONS; i++)
	{
		if(m_aMap[i] < 0)
		{
			paMap[i] = -1;
			break;
		}
		else
		{
			paMap[i] = m_aMap[i];
		}
	}
	return i;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetButtonWidth()
//
// 	Description:	This function is called to get the width of the buttons on
//					the toolbar
//
// 	Returns:		The width in pixels
//
//	Notes:			The button height is a function of the bitmap width
//
//==============================================================================
short CTMToolCtrl::GetButtonWidth()
{
	return (GetBitmapWidth() + BUTTON_HORZPADDING);
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetClassIdString()
//
// 	Description:	This method is called to get the class GUID as a null
//					terminated string.
//
// 	Returns:		The string equivalent of the CLASS GUID
//
//	Notes:			None
//
//==============================================================================
BSTR CTMToolCtrl::GetClassIdString() 
{
	CString strClsId = m_tmVersion.GetClsId();
	return strClsId.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetControlFlags()
//
// 	Description:	This function overrides the base class behavior to prevent
//					drawing the bar differently when it's deactivated
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
DWORD CTMToolCtrl::GetControlFlags()
{
	//	Get the base class settings
	DWORD dwFlags = COleControl::GetControlFlags();

	dwFlags |= noFlickerActivate;
	return dwFlags;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetDisabledStrip()
//
// 	Description:	This function is called to get the identifier of the 
//					appropriate image strip for disabled buttons
//
// 	Returns:		The resource identifier of the appropriate strip
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::GetDisabledStrip()
{
	switch(m_sButtonSize)
	{
		case TMTB_LARGEBUTTONS:		return IDB_TBDLARGE;
		case TMTB_MEDIUMBUTTONS:	return IDB_TBDMEDIUM;
		case TMTB_SMALLBUTTONS:
		default:					return IDB_TBDSMALL;
	}
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetEnabledStrip()
//
// 	Description:	This function is called to get the identifier of the 
//					appropriate image strip for enabled buttons
//
// 	Returns:		The resource identifier of the appropriate strip
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::GetEnabledStrip()
{
	switch(m_sButtonSize)
	{
		case TMTB_LARGEBUTTONS:		return IDB_TBLARGE;
		case TMTB_MEDIUMBUTTONS:	return IDB_TBMEDIUM;
		case TMTB_SMALLBUTTONS:
		default:					return IDB_TBSMALL;
	}
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetImageIndex()
//
// 	Description:	This method is called to get the index of the image 
//					associated with the specified button
//
// 	Returns:		The index of the button in the image strip
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::GetImageIndex(short sId) 
{
	if(sId >= TMTB_MAXBUTTONS || sId < 0)
		return 0;
	else
		return ImageMap[sId];
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetRegisteredPath()
//
// 	Description:	This method is called to get the ocx path stored in the
//					system registry.
//
// 	Returns:		The path to the ocx stored in the registry
//
//	Notes:			None
//
//==============================================================================
BSTR CTMToolCtrl::GetRegisteredPath() 
{
	CString strRegistered = m_tmVersion.GetFileSpec();
	return strRegistered.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetRegistration()
//
// 	Description:	This function is called to get the control's registration
//					information.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::GetRegistration() 
{
	CLSID	clsid;

	//	Get the GUID 
	GetClassID(&clsid);

	//	Initialize the version information
	m_tmVersion.InitFromClsId("TMTool", "Toolbar Control", clsid);

}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetSortedId()
//
// 	Description:	This method is called to get the sorted id of the specified
//					button
//
// 	Returns:		The id of the button in sorted order
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::GetSortedId(short sId) 
{
	if(sId >= TMTB_MAXBUTTONS || sId < 0)
		return 0;
	else
		return Sorted[sId];
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetSortOrder()
//
// 	Description:	This method is called to get the array used to sort the
//					toolbar buttons
//
// 	Returns:		TMTB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::GetSortOrder(short FAR* pOrder) 
{
	if(pOrder != 0)
		memcpy(pOrder, Sorted, sizeof(Sorted));

	return TMTB_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetVerBuild()
//
// 	Description:	This method is called to get the value of the VerBuild
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::GetVerBuild() 
{
	return m_tmVersion.GetBuild();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetVerBuildDate()
//
// 	Description:	This method is called to get the value of the VerBuildDate
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMToolCtrl::GetVerBuildDate() 
{
	CString strBuildDate = m_tmVersion.GetBuildDate();
	return strBuildDate.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetVerMajor()
//
// 	Description:	This method is called to get the value of the VerMajor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::GetVerMajor() 
{
	return m_tmVersion.GetMajor();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetVerMinor()
//
// 	Description:	This method is called to get the value of the VerMinor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::GetVerMinor() 
{
	return m_tmVersion.GetMinor();
}


//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetVerQEF()
//
// 	Description:	This method is called to get the value of the VerQEF
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::GetVerQEF() 
{
	return m_tmVersion.GetUpdate();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetVerTextLong()
//
// 	Description:	This method is called to get the value of the VerTextLong
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMToolCtrl::GetVerTextLong() 
{
	CString strVer = m_tmVersion.GetTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetVerTextShort()
//
// 	Description:	This method is called to get the value of the VerTextShort
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMToolCtrl::GetVerTextShort() 
{
	CString strVer = m_tmVersion.GetShortTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::HideButton()
//
// 	Description:	This method allows the caller to control the visibility
//					of a button
//
// 	Returns:		TMTB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::HideButton(short sId, BOOL bHide) 
{
	if(!IsWindow(m_Toolbar.m_hWnd))
		return TMTB_NOERROR;

	//	Are we hiding the button?
	if(bHide)
	{
		//	Is the button already hidden?
		if(m_Toolbar.IsButtonHidden((TMTB_COMMANDOFFSET + sId)))
			return TMTB_NOERROR;

		//	Hide the button. 
		m_Toolbar.HideButton((TMTB_COMMANDOFFSET + sId), TRUE);
		m_sButtons--;

		//	Reposition the toolbar
		Reposition();
	}
	else
	{
		//	Is the button already visible?
		if(!m_Toolbar.IsButtonHidden((TMTB_COMMANDOFFSET + sId)))
			return TMTB_NOERROR;

		//	Show the button. 
		m_Toolbar.HideButton((TMTB_COMMANDOFFSET + sId), FALSE);
		m_sButtons++;

		//	Reposition the toolbar
		Reposition();
	}

	return TMTB_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::Initialize()
//
// 	Description:	This function will construct and initialize the toolbar
//
// 	Returns:		TMTB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::Initialize()
{
	//	Don't bother if not in user mode
	m_Counter++;

	if(!AmbientUserMode())
		return TMTB_NOERROR;
	
	//	Read the configuration information from file if specified
	if(!m_strIniFile.IsEmpty() && !m_strIniSection.IsEmpty())
		ReadIniFile();

	//	Create the toolbar control
	if(!CreateToolbar())
	{
		m_Errors.Handle(0, IDS_TMTB_CREATEBARFAILED);
		return TMTB_CREATEBARFAILED;
	}

	//	if extra large toolbar then add extra large buttons
	/*if (m_Counter == 9)
	{	
		AddExtraLargeButtons();
	}
	
	else*/
		AddButtons();

	ResetFrame();

	return TMTB_NOERROR;

}	

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::IsButton()
//
// 	Description:	This method allows the caller to determine if a button
//					is in the bar
//
// 	Returns:		TRUE if the button is in the bar
//
//	Notes:			None
//
//==============================================================================
BOOL CTMToolCtrl::IsButton(short sId) 
{
	//	Is this toolbar available?
	if(!IsWindow(m_Toolbar.m_hWnd))
		return FALSE;

	//	Is the button in the bar?
	return (m_Toolbar.GetState(sId + TMTB_COMMANDOFFSET) >= 0);
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::IsCheckButton()
//
// 	Description:	This function is called to see if the button is a check
//					style button
//
// 	Returns:		TRUE if check style
//
//	Notes:			None
//
//==============================================================================
BOOL CTMToolCtrl::IsCheckButton(short sId) 
{
	switch(sId)
	{
		case TMTB_SPLITVERTICAL:
		case TMTB_SPLITHORIZONTAL:
		case TMTB_PLAYDESIGNATION:
		case TMTB_PAUSEDESIGNATION:	
		case TMTB_PLAYMOVIE:
		case TMTB_PAUSEMOVIE:	
		case TMTB_BLUE:
		case TMTB_RED:
		case TMTB_GREEN:
		case TMTB_BLACK:
		case TMTB_YELLOW:
		case TMTB_WHITE:	
		case TMTB_DARKBLUE:
		case TMTB_DARKRED:
		case TMTB_DARKGREEN:
		case TMTB_LIGHTBLUE:
		case TMTB_LIGHTRED:
		case TMTB_LIGHTGREEN:
		case TMTB_CALLOUT:
		case TMTB_SELECT:
		case TMTB_PAN:
		case TMTB_DRAWTOOL:
		case TMTB_HIGHLIGHT:
		case TMTB_REDACT:
		case TMTB_DISABLELINKS:
		case TMTB_ENABLELINKS:
		case TMTB_TEXT:
		case TMTB_FREEHAND:
		case TMTB_LINE:
		case TMTB_ARROW:
		case TMTB_ELLIPSE:
		case TMTB_RECTANGLE:
		case TMTB_FILLEDELLIPSE:
		case TMTB_FILLEDRECTANGLE:
		case TMTB_POLYLINE:
		case TMTB_POLYGON:
		case TMTB_ANNTEXT:
		case TMTB_STATUSBAR:
		case TMTB_SHADEDCALLOUTS:
		case TMTB_ZOOM:
		case TMTB_GESTUREPAN:	
		case TMTB_BINDERLIST:
		case TMTB_ZOOMRESTRICTED:	return TRUE;
		default:					return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::IsColorGroup()
//
// 	Description:	This function is called to check if a button is in the
//					color group
//
// 	Returns:		TRUE if in group
//
//	Notes:			None
//
//==============================================================================
BOOL CTMToolCtrl::IsColorGroup(short sId) 
{
	switch(sId)
	{
		case TMTB_BLUE:
		case TMTB_RED:
		case TMTB_GREEN:
		case TMTB_WHITE:
		case TMTB_BLACK:
		case TMTB_DARKBLUE:
		case TMTB_DARKRED:
		case TMTB_DARKGREEN:
		case TMTB_LIGHTBLUE:
		case TMTB_LIGHTRED:
		case TMTB_LIGHTGREEN:
		case TMTB_YELLOW:	return TRUE;
		default:			return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::IsPlayGroup()
//
// 	Description:	This function is called to check if a button is in the
//					video playback group
//
// 	Returns:		TRUE if in group
//
//	Notes:			None
//
//==============================================================================
BOOL CTMToolCtrl::IsPlayGroup(short sId) 
{
	switch(sId)
	{
		case TMTB_PLAYDESIGNATION:
		case TMTB_PAUSEDESIGNATION:	
		case TMTB_PLAYMOVIE:
		case TMTB_PAUSEMOVIE:		return TRUE;
		default:					return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::IsShapeGroup()
//
// 	Description:	This function is called to check if a button is in the
//					drawing pens/shapes group
//
// 	Returns:		TRUE if in group
//
//	Notes:			None
//
//==============================================================================
BOOL CTMToolCtrl::IsShapeGroup(short sId) 
{
	switch(sId)
	{
		case TMTB_FREEHAND:
		case TMTB_LINE:
		case TMTB_ARROW:
		case TMTB_ELLIPSE:
		case TMTB_RECTANGLE:
		case TMTB_FILLEDELLIPSE:
		case TMTB_FILLEDRECTANGLE:
		case TMTB_POLYLINE:
		case TMTB_POLYGON:
		case TMTB_ANNTEXT:			return TRUE;
		default:					return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::IsToolGroup()
//
// 	Description:	This function is called to check if a button is in the
//					annotation tool group
//
// 	Returns:		TRUE if in group
//
//	Notes:			None
//
//==============================================================================
BOOL CTMToolCtrl::IsToolGroup(short sId) 
{
	switch(sId)
	{
		case TMTB_CALLOUT:
		case TMTB_PAN:
		case TMTB_DRAWTOOL:
		case TMTB_HIGHLIGHT:
		case TMTB_REDACT:
		case TMTB_SELECT:
		case TMTB_ZOOM:			return TRUE;
		default:				return FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::IsHorizontal()
//
// 	Description:	This function is called to determine if the toolbar uses
//					a horizontal orientation.
//
// 	Returns:		TRUE if horizontal
//
//	Notes:			None
//
//==============================================================================
BOOL CTMToolCtrl::IsHorizontal() 
{
	if((m_sOrientation == TMTB_LEFT) || (m_sOrientation == TMTB_RIGHT))
		return FALSE;
	else
		return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnAutoInitChanged()
//
// 	Description:	This function is called when the AutoInit property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::OnAutoInitChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnAutoResetChanged()
//
// 	Description:	This function is called when the AutoReset property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::OnAutoResetChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnButtonClick()
//
// 	Description:	This function will fire an event when a button on the
//					toolbar is clicked.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::OnButtonClick(UINT uId)
{
	//	Is the user requesting configuration?
	
	if(m_bConfigurable && (GetKeyState(VK_SHIFT) & 0x8000))
	{	
		Configure();
		return;
	}

	FireButtonClick((short)(uId - TMTB_COMMANDOFFSET), 
				    m_Toolbar.IsButtonChecked(uId));
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnButtonMaskChanged()
//
// 	Description:	This function is called when the ButtonMask property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::OnButtonMaskChanged() 
{
	if(IsWindow(m_Toolbar.m_hWnd) && (m_bAutoReset == TRUE))
		Initialize();

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnButtonRowsChanged()
//
// 	Description:	This function is called when the ButtonRows property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::OnButtonRowsChanged() 
{
	//	Reposition the toolbar
	if(IsWindow(m_Toolbar.m_hWnd) && (m_bAutoReset == TRUE))
		Reposition();

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnButtonSizeChanged()
//
// 	Description:	This function is called when the ButtonSize property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::OnButtonSizeChanged() 
{
	if(IsWindow(m_Toolbar.m_hWnd) && (m_bAutoReset == TRUE))
		Initialize();

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnConfigurableChanged()
//
// 	Description:	This function is called when the Configurable property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::OnConfigurableChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnCreate()
//
// 	Description:	This function is called when the control is created.
//
// 	Returns:		0 if successful
//
//	Notes:			None
//
//==============================================================================
int CTMToolCtrl::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{

	if (COleControl::OnCreate(lpCreateStruct) == -1)
		return -1;

	//	Initialize the error handler
	m_Errors.Enable(m_bEnableErrors);
	m_Errors.SetParent(m_hWnd);
	m_Errors.SetTitle(TMTBERRORS_TITLE);
	
	//	Should we automatically create the toolbar?
	if(m_bAutoInit && AmbientUserMode())
		Initialize();

	return 0;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnDraw()
//
// 	Description:	This function is called in response to WM_PAINT messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::OnDraw(CDC* pdc, const CRect& rcBounds,const CRect& rcInvalid)
{
	CBrush brBackground;

	//	Create a brush using the background color
	if(m_bUseSystemBackground)
		brBackground.CreateSolidBrush(::GetSysColor(COLOR_3DFACE));
	else
		brBackground.CreateSolidBrush(TranslateColor(GetBackColor()));

	// Paint the control in the background color
	pdc->FillRect(rcBounds, &brBackground);

	if(AmbientUserMode())
	{
		if(IsWindow(m_Toolbar.m_hWnd))
			m_Toolbar.RedrawWindow();
	}
	else
	{
		CRect ControlRect = rcBounds;
		CString strText;

		strText.Format("FTI Toolbar Control (rev. %d.%d)",
					   _wVerMajor, _wVerMinor);

		pdc->SetBkMode(TRANSPARENT);
		if(TranslateColor(GetBackColor()) == RGB(0x00,0x00,0x00))
			pdc->SetTextColor(RGB(0xFF,0xFF,0xFF));
		else
			pdc->SetTextColor(RGB(0x00,0x00,0x00));

		pdc->DrawText(strText, ControlRect, 
					  DT_CENTER | DT_VCENTER | DT_SINGLELINE); 
	}

}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnEnableErrorsChanged()
//
// 	Description:	This function is called when the EnableErrors property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::OnEnableErrorsChanged() 
{
	m_Errors.Enable(m_bEnableErrors);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnIniFileChanged()
//
// 	Description:	This function is called when the IniFile property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::OnIniFileChanged() 
{
	//	Do not respond to runtime changes. This property must be set prior to
	//	initialization of the control

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnIniSectionChanged()
//
// 	Description:	This function is called when the IniSection property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::OnIniSectionChanged() 
{
	//	Do not respond to runtime changes. This property must be set prior to
	//	initialization of the control

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnLButtonDown()
//
// 	Description:	This function traps left button clicks to prevent messages
//					from being sent to the parent window when the user clicks
//					outside the button rectangle.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::OnLButtonDown(UINT nFlags, CPoint point) 
{
	
	//	Is the user requesting configuration?
	if(m_bConfigurable && (GetKeyState(VK_SHIFT) & 0x8000))
	{	
		Configure();
		return;
	}

}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnLButtonUp()
//
// 	Description:	This function traps left button clicks to prevent messages
//					from being sent to the parent window when the user clicks
//					outside the button rectangle.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::OnLButtonUp(UINT nFlags, CPoint point) 
{
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnMouseActivate()
//
// 	Description:	This function overrides the base class behavior to prevent
//					the bar from deactivating with mouse clicks..
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
int CTMToolCtrl::OnMouseActivate(CWnd* pDesktopWnd,UINT nHitTest,UINT message) 
{
	//	Trap the message here
	return 1;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnNotify()
//
// 	Description:	This function traps notifications sent from the toolbar
//
// 	Returns:		TRUE if handled
//
//	Notes:			None
//
//==============================================================================
BOOL CTMToolCtrl::OnNotify(WPARAM wParam, LPARAM lParam, LRESULT* pResult) 
{
	LPTOOLTIPTEXT	lpTip;
	short			sId;
	char			szLabel[64];

	if(((LPNMHDR) lParam)->code == TTN_NEEDTEXT && m_bToolTips)
	{
		//	Get the button identifier
		lpTip = (LPTOOLTIPTEXT)lParam;
		sId = lpTip->hdr.idFrom - TMTB_COMMANDOFFSET;

		if(sId >= 0 && sId < TMTB_MAXBUTTONS)
		{
			lstrcpyn(szLabel, m_aLabels[sId], sizeof(szLabel));
			lpTip->lpszText = szLabel;
			return TRUE;
		}

	}
	
	return COleControl::OnNotify(wParam, lParam, pResult);
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnOrientationChanged()
//
// 	Description:	This function is called when the Orientation property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::OnOrientationChanged() 
{
	if(IsWindow(m_Toolbar.m_hWnd) && (m_bAutoReset == TRUE))
		Reposition();

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnSetExtent()
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
BOOL CTMToolCtrl::OnSetExtent(LPSIZEL lpSizeL) 
{
	//CWnd* pWindow;

	if(!AmbientUserMode())
		return COleControl::OnSetExtent(lpSizeL);

	////	Get a CDC object so we can call HIMETRICtoDP() and DPtoHIMETRIC()
	//pWindow = CWnd::FromHandle(::GetDesktopWindow());
	//CClientDC dc(pWindow);

	////	Set the size to that of the bar
	//CSize size((m_rcBar.right - m_rcBar.left), 
	//		   (m_rcBar.bottom - m_rcBar.top));
	//dc.DPtoHIMETRIC(&size);
	//lpSizeL->cx = size.cx;
	//lpSizeL->cy = size.cy;	
	
	return COleControl::OnSetExtent(lpSizeL);
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnSize()
//
// 	Description:	This method traps WM_SIZE messages to keep the toolbar
//					appropriately sized and positioned
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::OnSize(UINT nType, int cx, int cy) 
{
	//	Resize the toolbar to the frame
	ResetFrame();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnHScroll()
//
// 	Description:	This function is called whenever an attempt is made to 
//					horizontally scroll the scrolbar. It positions the scroll bar
//					accordingly.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::OnHScroll(UINT nSBCode,UINT nPos, CScrollBar* pScrollBar)
{
	int iWidth = m_rcButtons.right - m_rcButtons.left;
	int minpos;
	int maxpos;
	m_rcButtons.left = -nPos;
	m_rcButtons.right = m_rcButtons.left + iWidth;

   // Get range of the scroll
   GetScrollRange(SB_HORZ, &minpos, &maxpos); 
   maxpos = GetScrollLimit(SB_HORZ);

   // Get the current position of scroll box.
   int curpos = COleControl::GetScrollPos(SB_HORZ);

   // Determine the new position of scroll box.
   switch (nSBCode)
   {
   case SB_LEFT:      // Scroll to far left.
      curpos = minpos;
      break;

   case SB_RIGHT:      // Scroll to far right.
      curpos = maxpos;
      break;

   case SB_ENDSCROLL:   // End scroll.
      break;

   case SB_LINELEFT:      // Scroll left.
      if (curpos > minpos)
	  {
		  curpos--;
		  nPos = curpos;
	  }
      break;

   case SB_LINERIGHT:   // Scroll right.
      if (curpos < maxpos)
	  {      
		  curpos++;
		  nPos = curpos;
	  }
      break;

   case SB_PAGELEFT:    // Scroll one page left.
   {
      // Get the page size. 
      SCROLLINFO   info;
      GetScrollInfo(SB_HORZ, &info, SIF_ALL);

      if (curpos > minpos)
      curpos = max(minpos, curpos - (int) info.nPage);
   }
      break;

   case SB_PAGERIGHT:      // Scroll one page right.
   {
      // Get the page size. 
      SCROLLINFO   info;
      GetScrollInfo(SB_HORZ, &info, SIF_ALL);

      if (curpos < maxpos)
         curpos = min(maxpos, curpos + (int) info.nPage);
   }
      break;

   case SB_THUMBPOSITION: // Scroll to absolute position. nPos is the position
      curpos = nPos;      // of the scroll box at the end of the drag operation.
      break;

   case SB_THUMBTRACK:   // Drag scroll box to specified position. nPos is the
      curpos = nPos;     // position that the scroll box has been dragged to.
      break;
   default: 
		curpos = nPos;

   }

   // Set the new position of the thumb (scroll box).
	SetScrollPos(SB_HORZ, curpos);
	COleControl::OnHScroll(nSBCode, nPos, pScrollBar);
	if (nPos>0)
	{
		if(IsWindow(m_Toolbar.m_hWnd)) 
		{ 
			iWidth = m_rcButtons.right - m_rcButtons.left;
			m_rcButtons.left = -nPos;
			m_rcButtons.right = m_rcButtons.left + iWidth;
			m_Toolbar.MoveWindow(&m_rcButtons);
		}
	}

}

void CTMToolCtrl::OnVScroll(UINT nSBCode,UINT nPos, CScrollBar* pScrollBar)
{
}

BOOL CTMToolCtrl::OnMouseWheel(UINT nFlags,short zDelta, CPoint pt)
{
    BOOL wasScrolled =  CTMToolCtrl::OnMouseWheel(nFlags,zDelta, pt);
	//BOOL wasScrolled = m_scrollHelper->OnMouseWheel(nFlags,zDelta, pt);
    return wasScrolled;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnStretchChanged()
//
// 	Description:	This function is called when the Stretch property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::OnStretchChanged() 
{
	if(IsWindow(m_Toolbar.m_hWnd) && (m_bAutoReset == TRUE))
		Reposition();

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnStyleChanged()
//
// 	Description:	This function is called when the Style property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::OnStyleChanged() 
{
	if(IsWindow(m_Toolbar.m_hWnd) && (m_bAutoReset == TRUE))
		Initialize();

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnToolTipsChanged()
//
// 	Description:	This function is called when the ToolTips property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::OnToolTipsChanged() 
{
	if(IsWindow(m_Toolbar.m_hWnd))
	{
		if(m_bToolTips)
			m_Toolbar.ModifyStyle(TBSTYLE_TOOLTIPS, 0, 0);
		else
			m_Toolbar.ModifyStyle(0, TBSTYLE_TOOLTIPS, 0);
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::OnUseSystemBackgroundChanged()
//
// 	Description:	This function is called when the UseSystemBackground 
//					property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::OnUseSystemBackgroundChanged() 
{
	InvalidateControl();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::Popup()
//
// 	Description:	This method is called to to resize and reposition the popup
//					window provided by the caller to cover the toolbar
//
// 	Returns:		TMTB_NOERROR if successful
//
//	Notes:			If the window provided by the caller is a child window this
//					function will to work properly because it uses screen
//					coordinates instead of client coordinates
//
//==============================================================================
short CTMToolCtrl::Popup(OLE_HANDLE hWnd) 
{
	POINT	Pt;

	//	Convert top left position to screen coordinates
	Pt.x = m_rcBar.left;
	Pt.y = m_rcBar.top;
	ClientToScreen(&Pt);

	//	Postion the caller's window
	::MoveWindow((HWND)hWnd, Pt.x, Pt.y, GetBarWidth(), GetBarHeight(), TRUE);

	return TMTB_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::PreTranslateMessage()
//
// 	Description:	This function is overloaded to trap and inhibit the 
//					processing of keystroke messages.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTMToolCtrl::PreTranslateMessage(MSG* pMsg) 
{
	switch(pMsg->message)
	{
		case WM_KEYDOWN:
		case WM_KEYUP:

			return FALSE;

		default:

			return COleControl::PreTranslateMessage(pMsg);

	}
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::ReadIniFile()
//
// 	Description:	This function is called to read the toolbar configuration
//					information from the ini file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::ReadIniFile() 
{

	CTMIni	Ini;
	short	aMap[TMTB_MAXBUTTONS];

	//	Make sure we can open the ini file
	//
	//	NOTE:	We don't consider it an error if the file is not found because
	//			it may not have ever been saved as the result of a user changing
	//			the configuration
	if(Ini.Open(m_strIniFile, m_strIniSection) == FALSE)
		return;

	//	Read the control properties
	m_sStyle = (short)Ini.ReadLong(TMTB_INI_STYLE_LINE, DEFAULT_TBSTYLE);
	m_sButtonSize = (short)Ini.ReadLong(TMTB_INI_SIZE_LINE, DEFAULT_TBBUTTONSIZE);
	m_sOrientation = (short)Ini.ReadLong(TMTB_INI_ORIENTATION_LINE, DEFAULT_TBORIENTATION);
	m_bStretch = Ini.ReadBool(TMTB_INI_STRETCH_LINE, DEFAULT_TBSTRETCH);
	m_bToolTips = Ini.ReadBool(TMTB_INI_TIPS_LINE, DEFAULT_TBTOOLTIPS);
	m_sButtonRows = (short)Ini.ReadLong(TMTB_INI_ROWS_LINE, DEFAULT_TBBUTTONROWS);
	if(m_sButtonRows < 1)
		m_sButtonRows = 1;

	//	Read the first entry in the button map and break out if the file does
	//	not contain a map
	if((aMap[0] = (short)Ini.ReadLong(0, -1)) >= 0)
	{
		//	Read the rest of the map
		for(int i = 1; i < TMTB_MAXBUTTONS; i++)
		{
			aMap[i] = (short)Ini.ReadLong(i, -1);

			if(aMap[i] < 0)
				break;
		}

		//	Reset the button map
		SetButtonMap(aMap);
	}
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::Reposition()
//
// 	Description:	This function is called to reposition the bar when the
//					size of the frame or number of buttons change
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::Reposition() 
{
	::GetWindowRect(::GetDesktopWindow(), &m_ScreenResolution);
	
	int	iMaxWidth;
	int	iMaxHeight;
	int iWidth;
	int iHeight;
	int	iCx;
	int	iCy;
	char tmp[512];
	//	Don't bother if not in user mode
	if(!AmbientUserMode() || !IsWindow(m_Toolbar.m_hWnd))
		return;

	//	Get the maximum available width and height
	iMaxWidth  = m_rcFrame.right  - m_rcFrame.left;
	iMaxHeight = m_rcFrame.bottom - m_rcFrame.top;

	//	Get the center point of the frame
	iCx = iMaxWidth / 2;
	iCy = iMaxHeight / 2;

	//	Make sure we have a valid number of rows
	if(m_sButtonRows < 1)
		m_sButtonRows = 1;

	//	Get the rectangle required for the toolbar
	if(IsHorizontal())
	{
		m_Toolbar.SetRows(m_sButtonRows, TRUE, &m_rcButtons);
	}
	else
	{
		//	For a vertical toolbar the number of rows is actually the number
		//	of columns. Since we have wrapping enabled, this results in a 
		//	M x N toolbar where N = number of rows
		m_Toolbar.SetRows(m_sButtons / m_sButtonRows, TRUE, &m_rcButtons);

		//	We have to work around a bug in the window's common toolbar control
		//	If we only have one column of buttons, Windows gets	the size 
		//	wrong (it returns a NULL rectangle).
		if(m_sButtonRows == 1)
		{
			m_rcButtons.left = 0;
			m_rcButtons.right = GetButtonWidth();
			m_rcButtons.top = 0;
			m_rcButtons.bottom = (GetButtonHeight() * m_sButtons) + 2;
		}

	}
		
	//	What are the dimensions of the rectangle required for all the buttons?
	iWidth  = m_rcButtons.right  - m_rcButtons.left;
	iHeight = m_rcButtons.bottom - m_rcButtons.top;

	//	Are we streching the bar to the full width or height?
	if(m_bStretch)
	{
		//	The size and position are dependent upon the orientation
		switch(m_sOrientation)
		{
			case TMTB_LEFT:		
			
				//	The bar uses the full frame height
				m_rcBar.left   = 0;
				m_rcBar.right  = iWidth;
				m_rcBar.top    = 0;
				m_rcBar.bottom = m_rcFrame.bottom;

				//	Make sure the buttons are properly positioned within the
				//	client area
				m_rcButtons.left  = 0;
				m_rcButtons.right = iWidth;

				//	Do we need to center the buttons within the client area?
				if((iHeight + 2) < iMaxHeight)
				{
					m_rcButtons.top    = iCy - (iHeight / 2);
					m_rcButtons.bottom = m_rcButtons.top + iHeight;
				}
				else
				{
					m_rcButtons.top    = 0;
					m_rcButtons.bottom = m_rcButtons.top + iHeight;
				}
				break;
		
			case TMTB_RIGHT:	
			
				//	The bar uses the full frame height
				m_rcBar.right  = m_rcFrame.right;
				m_rcBar.left   = m_rcBar.right - iWidth;
				m_rcBar.top    = 0;
				m_rcBar.bottom = m_rcFrame.bottom;

				//	Make sure the buttons are properly positioned within the
				//	client area
				m_rcButtons.left  = 0;
				m_rcButtons.right = iWidth;

				//	Do we need to center the buttons within the client area?
				if((iHeight + 2) < iMaxHeight)
				{
					m_rcButtons.top    = iCy - (iHeight / 2);
					m_rcButtons.bottom = m_rcButtons.top + iHeight;
				}
				else
				{
					m_rcButtons.top    = 0;
					m_rcButtons.bottom = m_rcButtons.top + iHeight;
				}
				break;
		
			case TMTB_TOP:		
			
				//	The bar uses the full width of the frame
				m_rcBar.left	= 0;
				m_rcBar.right   = m_rcFrame.right;
				m_rcBar.top     = 0;
				m_rcBar.bottom  = iHeight;

				//	Make sure the buttons are properly positioned within the
				//	client area
				m_rcButtons.top    = 0;
				m_rcButtons.bottom = iHeight;

				//	Do we need to center the buttons within the client area?
				if((iWidth + 2) < iMaxWidth)
				{
					m_rcButtons.left  = iCx - (iWidth / 2);
					m_rcButtons.right = m_rcButtons.left + iWidth;
				}
				else
				{
					m_rcButtons.left  = 0;
					m_rcButtons.right = m_rcButtons.left + iWidth;
				}
				break;
		
			case TMTB_BOTTOM:	
			default:			
				
				//	The bar uses the full width of the frame
				m_rcBar.left	= 0;
				m_rcBar.right   = m_rcFrame.right;
				m_rcBar.bottom  = m_rcFrame.bottom;
				m_rcBar.top     = m_rcBar.bottom - iHeight;

				//	Make sure the buttons are properly positioned within the
				//	client area
				m_rcButtons.top = 0;
				m_rcButtons.bottom = iHeight;

				//	Do we need to center the buttons within the client area?
				if((iWidth + 2) < iMaxWidth)
				{
					m_rcButtons.left  = iCx - (iWidth / 2);
					m_rcButtons.right = m_rcButtons.left + iWidth;
				}
				else
				{
					m_rcButtons.left  = 0;
					m_rcButtons.right = m_rcButtons.left + iWidth;
				}
				break;
		
		}
		
	}
	else
	{
		switch(m_sOrientation)
		{
			case TMTB_LEFT:		
			
				//	Center the bar vertically within the frame
				m_rcBar.left = 0;
				m_rcBar.right = m_rcBar.left + iWidth;
				m_rcBar.top = iCy - (iHeight / 2);
				if(m_rcBar.top < 0)
					m_rcBar.top = 0;
				m_rcBar.bottom = m_rcBar.top + iHeight;

				break;
		
			case TMTB_RIGHT:	
			
				//	Center the bar vertically within the frame
				m_rcBar.right = m_rcFrame.right;
				m_rcBar.left = m_rcBar.right - iWidth;
				m_rcBar.top = iCy - (iHeight / 2);
				if(m_rcBar.top < 0)
					m_rcBar.top = 0;
				m_rcBar.bottom = m_rcBar.top + iHeight;

				break;
		
			case TMTB_TOP:		
			
				//	Center the bar horizontally within the frame				
				m_rcBar.left = iCx - (iWidth / 2);
				if(m_rcBar.left < 0)
					m_rcBar.left = 0;
				m_rcBar.right = m_rcBar.left + iWidth;
				m_rcBar.top = 0;
				m_rcBar.bottom = m_rcBar.top + iHeight;
				break;
		
			case TMTB_BOTTOM:	
			default:			
			
				//	Center the bar horizontally within the frame				
				m_rcBar.left = iCx - (iWidth / 2);
				if(m_rcBar.left < 0)
					m_rcBar.left = 0;
				m_rcBar.right = m_rcBar.left + iWidth;
				m_rcBar.bottom = m_rcFrame.bottom;
				m_rcBar.top = m_rcBar.bottom - iHeight;
				break;
		
		}
	
		//	The position of the buttons within the bar do not change with the
		//	orientation when we are not stretching the bar
		m_rcButtons.left   = 0;
		m_rcButtons.right  = iWidth;
		m_rcButtons.top    = 0;
		m_rcButtons.bottom = iHeight;
	}

	//	Windows appears to do some clipping of the window when it's drawn. This
	//	should account for that clipping
	/*
	switch(m_sOrientation)
	{
		case TMTB_LEFT:		
		case TMTB_RIGHT:	
			
			m_rcBar.bottom += 2;
			m_rcButtons.bottom += 2;
			break;
		
		case TMTB_TOP:		
			
			m_rcBar.bottom += 1;
			m_rcBar.right  += 1;
			m_rcButtons.bottom += 1;
			m_rcButtons.right += 1;
			break;
		
		case TMTB_BOTTOM:	
		default:			

			m_rcBar.top -= 1;
			m_rcBar.right  += 1;
			m_rcButtons.bottom += 1;
			m_rcButtons.right += 1;
			break;
		
	}
	*/

	//	Move the window into position
	if(IsWindow(m_hWnd))
	{

		// setting some identification for extra large toolbar
		if (m_Counter ==9)
		{
		
		if ( m_rcButtons.right  > m_ScreenResolution.right)
			m_tmVersion.SetName("colorToolbarscroll");
		else 
			m_tmVersion.SetName("colorToolbar");

		}

		// checking if this is extra large toolbar
		if (this->m_tmVersion.GetName()== "colorToolbar" || this->m_tmVersion.GetName()== "colorToolbarscroll")
		{
			int a;
			SIZE size;
			size.cx = 48;
			size.cy = 28;
			this->m_Toolbar.SetBitmapSize(size);
			
			m_rcBar.top -= 2;
			m_rcButtons.top -= 2;
			m_rcBar.left -= 3;
			m_rcButtons.left -= 3;
			m_rcButtons.right = this->GetButtonWidth();
			
			if (m_sExtraButtonsCount > STACKED_COLORS)
				m_rcButtons.bottom = 35 * STACKED_COLORS;// 344
			else 
				m_rcButtons.bottom = 35 * m_sExtraButtonsCount;// 344

			m_rcBar.left = this->m_rcFrame.right - this->GetButtonWidth();
			m_rcBar.right = this->m_rcFrame.right; 
			m_rcBar.top = m_rcBar.top - m_rcButtons.bottom;
			m_rcBar.bottom = m_rcBar.bottom - this->GetButtonHeight();
			m_rcBar.bottom -= 7;
			m_rcBar.top -=10;
			m_rcBar.bottom -= 10;
			
		}

		if ( m_rcButtons.right  > m_ScreenResolution.right)
		{
			m_rcBar.top -=17;
			m_rcBar.right = m_ScreenResolution.right;
		}
		


		//	Move the toolbar into position
		SetCtrlSize();
		MoveWindow(&m_rcBar);
	}

		if(IsWindow(m_Toolbar.m_hWnd)) { 
			m_Toolbar.MoveWindow(&m_rcButtons);
		}
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::Reset()
//
// 	Description:	This function is called to force rebuilding of the toolbar
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::Reset() 
{
	//	Recreate the toolbar control
	if(!CreateToolbar())
	{
		m_Errors.Handle(0, IDS_TMTB_CREATEBARFAILED);
		return TMTB_CREATEBARFAILED;
	}

	//	Add the buttons
	AddButtons();

	//	Make sure everything is properly positioned
	ResetFrame();

	return TMTB_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::ResetFrame()
//
// 	Description:	This function is called when the size of the parent frame
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::ResetFrame() 
{
	CWnd* pParent;

	//	Get the parent window
	if((pParent = GetParent()) == NULL)
		return TMTB_NOERROR;

	//	Get the parent's client rectangle
	pParent->GetClientRect(&m_rcFrame);
	
	//	Reposition the toolbar
	Reposition();

	return TMTB_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::ResetFrame()
//
// 	Description:	This function is called when the size of the parent frame
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================

void CTMToolCtrl::ResetExtraLargeButtonsFrame()
{

	//m_rcBar.left = 200;
	//m_rcBar.right = 900;

	////m_rcBar.left = 833;
	////m_rcBar.right = 1281;
	//m_rcBar.top = 931;
	//m_rcBar.bottom = 718;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::Save()
//
// 	Description:	This method is called to save the current configuration to
//					the ini file.
//
// 	Returns:		TMTB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::Save() 
{
	//	Has an ini file been specified?
	if(!m_strIniFile.IsEmpty() && !m_strIniSection.IsEmpty())
	{
		WriteIniFile();
		return TMTB_NOERROR;
	}
	else
	{
		m_Errors.Handle(0, IDS_TMTB_NOFILE);
		return TMTB_NOFILE;
	}
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::SetButtonImage()
//
// 	Description:	This method allows the caller to set the image associated
//					with a button
//
// 	Returns:		TMTB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================


short CTMToolCtrl::SetButtonImage(short sId, short sImage) 
{
	//	Is this toolbar available?
	if(!IsWindow(m_Toolbar.m_hWnd))
	{
		m_Errors.Handle(0, IDS_TMTB_NOTINITIALIZED);
		return TMTB_NOTINITIALIZED;
	}

	if(sImage < 0 || sImage >= TMTB_MAXIMAGES)
		return TMTB_INVALIDIMAGEINDEX;

	//	Make sure the button is in the bar
	if(!IsButton(sId))
		return TMTB_NOERROR;


	m_Toolbar.ChangeBitmap((sId + TMTB_COMMANDOFFSET), sImage);
	
	return TMTB_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::SetButtonLabel()
//
// 	Description:	This method allows the caller to set the text label 
//					associated with a button
//
// 	Returns:		TMTB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::SetButtonLabel(short sId, LPCTSTR lpLabel) 
{
	if(sId < 0 || sId >= TMTB_MAXBUTTONS)
		return TMTB_INVALIDBUTTONID;

	if(lpLabel)
		m_aLabels[sId] = lpLabel;
	else
		m_aLabels[sId].Empty();

	return 0;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::SetButtonMap()
//
// 	Description:	This method allows the caller to set which buttons appear
//					in the toolbar and at which position they appear.
//
// 	Returns:		TMTB_NOERROR if successful
//
//	Notes:			The map provided by the caller should be an array of short
//					integers. The maximum size of the array is TMTB_MAXBUTTONS
//					As soon a value of -1 is reached, processing of the array
//					is stopped.
//
//==============================================================================
short CTMToolCtrl::SetButtonMap(short FAR* pMap) 
{
	short	aMap[TMTB_MAXBUTTONS];
	int		i;

	//	Don't bother if not in user mode
	if(!AmbientUserMode())
		return TMTB_NOERROR;

	ASSERT(pMap);
	ZeroMemory(aMap, sizeof(aMap));

	//	Verify each of the button identifiers
	for(i = 0; i < TMTB_MAXBUTTONS; i++)
	{
		//	Is this the end of the list?
		if(pMap[i]  < 0)
			break;

		//	Is the button identifier valid?
		if(pMap[i] >= TMTB_MAXBUTTONS)
		{
			CString Id;
			Id.Format("%d", pMap[i]);
			m_Errors.Handle(0, IDS_TMTB_INVALIDBUTTONID, Id);
			return TMTB_INVALIDBUTTONID;
		}
		else
		{
			aMap[i] = pMap[i];
		}
	}		

	//	Terminate the map
	if(i < TMTB_MAXBUTTONS)
		aMap[i] = -1;

	//	Transfer to our local map
	memcpy(m_aMap, aMap, sizeof(m_aMap));

	//	Rebuild the toolbar if it's already been initialized
	if(IsWindow(m_Toolbar.m_hWnd))
	{
		//	Create the toolbar control
		if(!CreateToolbar())
		{
			m_Errors.Handle(0, IDS_TMTB_CREATEBARFAILED);
			return TMTB_CREATEBARFAILED;
		}

		//	Add the buttons
		AddButtons();
	
		//	Reposition the bar to the appropriate location in the client area
		ResetFrame();
	}

	return TMTB_NOERROR;
}	

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::SetColorButton()
//
// 	Description:	This method is called to set the check state of the 
//					current color button
//
// 	Returns:		TMTB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::SetColorButton(short sId) 
{
	//	Is this toolbar available?
	if(!IsWindow(m_Toolbar.m_hWnd))
	{
		m_Errors.Handle(0, IDS_TMTB_NOTINITIALIZED);
		return TMTB_NOTINITIALIZED;
	}

	//	Set the check states of the color buttons
	CheckButton(TMTB_BLUE, (sId == TMTB_BLUE));
	CheckButton(TMTB_GREEN, (sId == TMTB_GREEN));
	CheckButton(TMTB_RED, (sId == TMTB_RED));
	CheckButton(TMTB_DARKBLUE, (sId == TMTB_DARKBLUE));
	CheckButton(TMTB_DARKGREEN, (sId == TMTB_DARKGREEN));
	CheckButton(TMTB_DARKRED, (sId == TMTB_DARKRED));
	CheckButton(TMTB_LIGHTBLUE, (sId == TMTB_LIGHTBLUE));
	CheckButton(TMTB_LIGHTGREEN, (sId == TMTB_LIGHTGREEN));
	CheckButton(TMTB_LIGHTRED, (sId == TMTB_LIGHTRED));
	CheckButton(TMTB_YELLOW, (sId == TMTB_YELLOW));
	CheckButton(TMTB_BLACK, (sId == TMTB_BLACK));
	CheckButton(TMTB_WHITE, (sId == TMTB_WHITE));

	return TMTB_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::SetLinkButton()
//
// 	Description:	This method is called to set the check state and image
//					of the TMTB_LINK button
//
// 	Returns:		TMTB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::SetLinkButton(BOOL bDisabled) 
{
	//	Is this toolbar available?
	if(!IsWindow(m_Toolbar.m_hWnd))
	{
		m_Errors.Handle(0, IDS_TMTB_NOTINITIALIZED);
		return TMTB_NOTINITIALIZED;
	}
	
	//	Is the disable links button available?
	if(IsButton(TMTB_DISABLELINKS))
	{
		//	Do we want to disable the links?
		if(bDisabled)
		{
			CheckButton(TMTB_DISABLELINKS, TRUE);
			SetButtonImage(TMTB_DISABLELINKS, GetImageIndex(TMTB_ENABLELINKS));
		}
		else
		{
			CheckButton(TMTB_DISABLELINKS, FALSE);
			SetButtonImage(TMTB_DISABLELINKS, GetImageIndex(TMTB_DISABLELINKS));
		}

	}
	//	Is the enable links button available?
	if(IsButton(TMTB_ENABLELINKS))
	{
		//	Do we want to disable the links?
		if(bDisabled)
		{
			CheckButton(TMTB_ENABLELINKS, FALSE);
			SetButtonImage(TMTB_ENABLELINKS, GetImageIndex(TMTB_ENABLELINKS));
		}
		else
		{
			CheckButton(TMTB_ENABLELINKS, TRUE);
			SetButtonImage(TMTB_ENABLELINKS, GetImageIndex(TMTB_DISABLELINKS));
		}

	}
	return TMTB_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::SetPlayButton()
//
// 	Description:	This method is called to set the check state and image
//					of the TMTB_PLAY button
//
// 	Returns:		TMTB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::SetPlayButton(BOOL bPlaying) 
{
	//	Is this toolbar available?
	if(!IsWindow(m_Toolbar.m_hWnd))
	{
		m_Errors.Handle(0, IDS_TMTB_NOTINITIALIZED);
		return TMTB_NOTINITIALIZED;
	}

	//	Is the play designation button available?
	if(IsButton(TMTB_PLAYDESIGNATION))
	{
		//	Are we playing?
		if(bPlaying)
		{
			CheckButton(TMTB_PLAYDESIGNATION, TRUE);
			SetButtonImage(TMTB_PLAYDESIGNATION, GetImageIndex(TMTB_PAUSEDESIGNATION));
		}
		else
		{
			CheckButton(TMTB_PLAYDESIGNATION, FALSE);
			SetButtonImage(TMTB_PLAYDESIGNATION, GetImageIndex(TMTB_PLAYDESIGNATION));
		}

	}
	//	Is the pause designation button available?
	if(IsButton(TMTB_PAUSEDESIGNATION))
	{
		//	Are we playing?
		if(bPlaying)
		{
			CheckButton(TMTB_PAUSEDESIGNATION, FALSE);
			SetButtonImage(TMTB_PAUSEDESIGNATION, GetImageIndex(TMTB_PAUSEDESIGNATION));
		}
		else
		{
			CheckButton(TMTB_PAUSEDESIGNATION, TRUE);
			SetButtonImage(TMTB_PAUSEDESIGNATION, GetImageIndex(TMTB_PLAYDESIGNATION));
		}

	}
	//	Is the play movie button available?
	if(IsButton(TMTB_PLAYMOVIE))
	{
		//	Are we playing?
		if(bPlaying)
		{
			CheckButton(TMTB_PLAYMOVIE, TRUE);
			SetButtonImage(TMTB_PLAYMOVIE, GetImageIndex(TMTB_PAUSEMOVIE));
		}
		else
		{
			CheckButton(TMTB_PLAYMOVIE, FALSE);
			SetButtonImage(TMTB_PLAYMOVIE, GetImageIndex(TMTB_PLAYMOVIE));
		}

	}
	//	Is the pause movie button available?
	if(IsButton(TMTB_PAUSEMOVIE))
	{
		//	Are we playing?
		if(bPlaying)
		{
			CheckButton(TMTB_PAUSEMOVIE, FALSE);
			SetButtonImage(TMTB_PAUSEMOVIE, GetImageIndex(TMTB_PAUSEMOVIE));
		}
		else
		{
			CheckButton(TMTB_PAUSEMOVIE, TRUE);
			SetButtonImage(TMTB_PAUSEMOVIE, GetImageIndex(TMTB_PLAYMOVIE));
		}

	}
	return TMTB_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::SetSplitButton()
//
// 	Description:	This method is called to set the check state and image
//					of the TMTB_SPLIT button
//
// 	Returns:		TMTB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::SetSplitButton(BOOL bSplit, BOOL bHorizontal) 
{
	//	Is this toolbar available?
	if(!IsWindow(m_Toolbar.m_hWnd))
	{
		m_Errors.Handle(0, IDS_TMTB_NOTINITIALIZED);
		return TMTB_NOTINITIALIZED;
	}

	//	Is the split vertical button available?
	if(IsButton(TMTB_SPLITVERTICAL))
	{
		if(bSplit && !bHorizontal)
		{
			CheckButton(TMTB_SPLITVERTICAL, TRUE);
		}
		else
		{
			CheckButton(TMTB_SPLITVERTICAL, FALSE);
		}

	}
	//	Is the split horizontal button available?
	if(IsButton(TMTB_SPLITHORIZONTAL))
	{
		if(bSplit && bHorizontal)
		{
			CheckButton(TMTB_SPLITHORIZONTAL, TRUE);
		}
		else
		{
			CheckButton(TMTB_SPLITHORIZONTAL, FALSE);
		}

	}
	return TMTB_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::SetShapeButton()
//
// 	Description:	This method is called to set the check state of the 
//					current drawing pen/shape button
//
// 	Returns:		TMTB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::SetShapeButton(short sId) 
{
	//	Is this toolbar available?
	if(!IsWindow(m_Toolbar.m_hWnd))
	{
		m_Errors.Handle(0, IDS_TMTB_NOTINITIALIZED);
		return TMTB_NOTINITIALIZED;
	}

	//	Set the check states of the buttons
	CheckButton(TMTB_FREEHAND, (sId == TMTB_FREEHAND));
	CheckButton(TMTB_LINE, (sId == TMTB_LINE));
	CheckButton(TMTB_ARROW, (sId == TMTB_ARROW));
	CheckButton(TMTB_ELLIPSE, (sId == TMTB_ELLIPSE));
	CheckButton(TMTB_RECTANGLE, (sId == TMTB_RECTANGLE));
	CheckButton(TMTB_FILLEDELLIPSE, (sId == TMTB_FILLEDELLIPSE));
	CheckButton(TMTB_FILLEDRECTANGLE, (sId == TMTB_FILLEDRECTANGLE));
	CheckButton(TMTB_POLYLINE, (sId == TMTB_POLYLINE));
	CheckButton(TMTB_POLYGON, (sId == TMTB_POLYGON));
	CheckButton(TMTB_ANNTEXT, (sId == TMTB_ANNTEXT));

	return TMTB_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::SetToolButton()
//
// 	Description:	This method is called to set the check state of the 
//					current annotation tool button
//
// 	Returns:		TMTB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::SetToolButton(short sId) 
{
	//	Is this toolbar available?
	if(!IsWindow(m_Toolbar.m_hWnd))
	{
		m_Errors.Handle(0, IDS_TMTB_NOTINITIALIZED);
		return TMTB_NOTINITIALIZED;
	}

	//	Set the check states of the color buttons
	CheckButton(TMTB_CALLOUT, (sId == TMTB_CALLOUT));
	CheckButton(TMTB_PAN, (sId == TMTB_PAN));
	CheckButton(TMTB_DRAWTOOL, (sId == TMTB_DRAWTOOL));
	CheckButton(TMTB_HIGHLIGHT, (sId == TMTB_HIGHLIGHT));
	CheckButton(TMTB_REDACT, (sId == TMTB_REDACT));
	CheckButton(TMTB_ZOOM, (sId == TMTB_ZOOM));
	CheckButton(TMTB_ZOOMRESTRICTED, (sId == TMTB_ZOOMRESTRICTED));
	CheckButton(TMTB_SELECT, (sId == TMTB_SELECT));
	CheckButton(TMTB_GESTUREPAN, (sId == TMTB_GESTUREPAN));
	CheckButton(TMTB_BINDERLIST, (sId == TMTB_BINDERLIST));

	return TMTB_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::ShowRects()
//
// 	Description:	This function is used for debugging to view the 
//					rectangles used to manage the toolbar
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::ShowRects(LPSTR lpTitle) 
{
	CString Msg;
	CString Tmp;

	Msg.Empty();

	//	Frame rectangle
	Tmp.Format("Frame Rectangle:\nLeft %d\nTop = %d\nRight = %d\nBottom = %d\n\n",
				m_rcFrame.left, m_rcFrame.top, m_rcFrame.right, m_rcFrame.bottom);
	Msg += Tmp;

	//	Bar rectangle
	Tmp.Format("Bar Rectangle:\nLeft %d\nTop = %d\nRight = %d\nBottom = %d\n\n",
				m_rcBar.left, m_rcBar.top, m_rcBar.right, m_rcBar.bottom);
	Msg += Tmp;

	//	Buttons rectangle
	Tmp.Format("Buttons Rectangle:\nLeft %d\nTop = %d\nRight = %d\nBottom = %d\n\n",
				m_rcButtons.left, m_rcButtons.top, m_rcButtons.right, m_rcButtons.bottom);
	Msg += Tmp;

	MessageBox(Msg, (lpTitle) ? lpTitle : "CTMToolCtrl::ShowRects()",
			   MB_ICONINFORMATION | MB_OK);

}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::WriteIniFile()
//
// 	Description:	This function is called to write the toolbar configuration
//					information to the ini file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::WriteIniFile() 
{
	CTMIni Ini;
	//	Open the file
	Ini.Open(m_strIniFile, m_strIniSection);

	//	Delete the section to get rid of the existing button map
	Ini.DeleteSection(m_strIniSection);

	//	Write the control properties
	Ini.WriteLong(TMTB_INI_STYLE_LINE, m_sStyle);
	Ini.WriteLong(TMTB_INI_SIZE_LINE, m_sButtonSize);
	Ini.WriteLong(TMTB_INI_ORIENTATION_LINE, m_sOrientation);
	Ini.WriteLong(TMTB_INI_ROWS_LINE, m_sButtonRows);
	Ini.WriteBool(TMTB_INI_STRETCH_LINE, m_bStretch);
	Ini.WriteBool(TMTB_INI_TIPS_LINE, m_bToolTips);

	//	Write the button map
	for(int i = 0; i < TMTB_MAXBUTTONS; i++)
	{
		if(m_aMap[i] < 0)
			break;
		else
			Ini.WriteLong(i, m_aMap[i]);
	}
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::SetZoomButton()
//
// 	Description:	This method is called to set the zoom button image and
//					zoom mode check state
//
// 	Returns:		TMTB_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::SetZoomButton(BOOL bZoom, BOOL bRestricted) 
{
	//	Is this toolbar available?
	if(!IsWindow(m_Toolbar.m_hWnd))
	{
		m_Errors.Handle(0, IDS_TMTB_NOTINITIALIZED);
		return TMTB_NOTINITIALIZED;
	}

	//	Is the zoom button available?
	if(IsButton(TMTB_ZOOM))
	{
		//	Are we in unrestricted zoom mode?
		if(bZoom && !bRestricted)
		{
			CheckButton(TMTB_ZOOM, TRUE);
		}
		else
		{
			CheckButton(TMTB_ZOOM, FALSE);
		}

	}
	//	Is the restricted zoom button available?
	if(IsButton(TMTB_ZOOMRESTRICTED))
	{
		//	Are we in restricted zoom mode?
		if(bZoom && bRestricted)
		{
			CheckButton(TMTB_ZOOMRESTRICTED, TRUE);
		}
		else
		{
			CheckButton(TMTB_ZOOMRESTRICTED, FALSE);
		}

	}
	return TMTB_NOERROR;
}


//==============================================================================
//
// 	Function Name:	CTMToolCtrl::SetCtrlSize()
//
// 	Description:	Called to set the control size to match the bar size
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMToolCtrl::SetCtrlSize() 
{
	CWnd* pwndDesktop = NULL;
	char tmp[512];
	RECT ;
	
	//	Get a CDC object so we can call HIMETRICtoDP() and DPtoHIMETRIC()
	pwndDesktop = CWnd::FromHandle(::GetDesktopWindow());


	CClientDC dc(pwndDesktop);
	//	Set the size to that of the bar
	CSize size((m_rcBar.right - m_rcBar.left), 
			   (m_rcBar.bottom - m_rcBar.top));
	dc.DPtoHIMETRIC(&size);
	
	SetControlSize(size.cx, size.cy);

	SCROLLINFO si;
	
	si.cbSize = sizeof(si);
	si.fMask = SIF_PAGE | SIF_RANGE ;
	si.nMin = 0;
	//si.nMax = m_rcButtons.right - m_rcButtons.left;
	si.nMax = m_rcButtons.right - m_rcBar.right;
//	si.nPage  = si.nMax -(m_rcButtons.right - m_rcBar.right);

	
	//si.nPage = m_ScreenResolution.right - si.nMax;
	//si.nMax += si.nPage; 
	if(m_ScreenResolution.right - si.nMax > 0)
		si.nPage = m_ScreenResolution.right - si.nMax;
	else 
		si.nPage =  si.nMax - m_ScreenResolution.right;
	if (si.nMax > 0)
		si.nMax += si.nPage;

	
	SetScrollInfo(SB_HORZ, &si,FALSE);
//	SetScrollRange(SB_HORZ,si.nMin,si.nMax+si.nPage,TRUE);
	SetScrollRange(SB_HORZ,si.nMin,si.nMax,FALSE);


}


BOOL CTMToolCtrl::PreCreateWindow(CREATESTRUCT& cs)
{
	cs.style |= WS_HSCROLL;
	return COleControl::PreCreateWindow(cs);
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetButtonActualWidth()
//
// 	Description:	This function will return button width
//					
//
// 	Returns:		short
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::GetButtonActualWidth()
{	
	return GetButtonWidth();
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetBarXPosition()
//
// 	Description:	This function will return bar X Position
//					
//
// 	Returns:		short
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::GetBarXPosition()
{	
	return this->m_rcBar.left;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetButtonXPosition()
//
// 	Description:	This function will return button X Position
//					
//
// 	Returns:		short
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::GetButtonXPosition(short sId)
{
	int iCommandId = TMTB_COMMANDOFFSET + sId;

	RECT rect;
	if(m_Toolbar.GetRect(iCommandId, &rect) == TRUE)
	{
		short xPosition = rect.left;
		return xPosition;
	}

	return 0;
}

//==============================================================================
//
// 	Function Name:	CTMToolCtrl::GetNumberOfButtonsToAdd()
//
// 	Description:	This function will calculate the number of buttons can be shown 
//					according to screen size and button size
//
// 	Returns:		short
//
//	Notes:			None
//
//==============================================================================
short CTMToolCtrl::GetNumberOfButtonsToAdd()
{		
	 int screenWidth = GetSystemMetrics(SM_CXSCREEN);
	 int buttonWidth = GetButtonWidth() + 7;

	 int numberOfButtonsToAdd = screenWidth/buttonWidth;	
	 return numberOfButtonsToAdd;
}