//==============================================================================
//
// File Name:	tmtext.cpp
//
// Description:	This file contains member functions of the CTMTextCtrl class.
//
// See Also:	tmtext.h
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
#include <tmtextap.h>
#include <tmtext.h>
#include <tmtextpg.h>
#include <tmtxdefs.h>
#include <font.h>
#include <mmsystem.h>
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
extern CTMTextApp NEAR theApp;

/* Replace 2 */
const IID BASED_CODE IID_DTMText6 =
		{ 0xe931c59c, 0x513e, 0x45c6, { 0xa4, 0xb8, 0xfa, 0x2, 0x27, 0xbb, 0xc7, 0x91 } };
/* Replace 3 */
const IID BASED_CODE IID_DTMText6Events =
		{ 0xf8edb3af, 0x6e80, 0x4c5c, { 0x9f, 0x11, 0x2c, 0xcf, 0xe0, 0xd9, 0x84, 0xd9 } };

// Control type information
static const DWORD BASED_CODE _dwTMTextOleMisc =
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
BEGIN_MESSAGE_MAP(CTMTextCtrl, COleControl)
	//{{AFX_MSG_MAP(CTMTextCtrl)
	ON_WM_SIZE()
	ON_WM_CREATE()
	ON_WM_DESTROY()
	//}}AFX_MSG_MAP
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
END_MESSAGE_MAP()

// Dispatch map
BEGIN_DISPATCH_MAP(CTMTextCtrl, COleControl)
	//{{AFX_DISPATCH_MAP(CTMTextCtrl)
	DISP_PROPERTY_NOTIFY(CTMTextCtrl, "AutoInit", m_bAutoInit, OnAutoInitChanged, VT_BOOL)
	DISP_PROPERTY_EX(CTMTextCtrl, "VerBuild", GetVerBuild, SetNotSupported, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMTextCtrl, "EnableErrors", m_bEnableErrors, OnEnableErrorsChanged, VT_BOOL)
	DISP_PROPERTY_EX(CTMTextCtrl, "VerMajor", GetVerMajor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMTextCtrl, "VerMinor", GetVerMinor, SetNotSupported, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMTextCtrl, "HighlightColor", m_ocHighlight, OnHighlightColorChanged, VT_COLOR)
	DISP_PROPERTY_NOTIFY(CTMTextCtrl, "HighlightTextColor", m_ocHighlightText, OnHighlightTextColorChanged, VT_COLOR)
	DISP_PROPERTY_NOTIFY(CTMTextCtrl, "HighlightLines", m_sHighlight, OnHighlightLinesChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMTextCtrl, "DisplayLines", m_sDisplay, OnDisplayLinesChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMTextCtrl, "Combine", m_bCombine, OnCombineDesignationsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMTextCtrl, "ShowPageLine", m_bShowPgLn, OnShowPageLineChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMTextCtrl, "UseAvgCharWidth", m_bUseAvgCharWidth, OnUseAvgCharWidthChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMTextCtrl, "ResizeOnChange", m_bResizeOnChange, OnResizeOnChangeChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMTextCtrl, "MaxCharsPerLine", m_sMaxCharsPerLine, OnMaxCharsPerLineChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMTextCtrl, "TopMargin", m_sTopMargin, OnTopMarginChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMTextCtrl, "BottomMargin", m_sBottomMargin, OnBottomMarginChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMTextCtrl, "LeftMargin", m_sLeftMargin, OnLeftMarginChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMTextCtrl, "RightMargin", m_sRightMargin, OnRightMarginChanged, VT_I2)
	DISP_PROPERTY_EX(CTMTextCtrl, "VerTextLong", GetVerTextLong, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMTextCtrl, "SmoothScroll", m_bSmoothScroll, OnSmoothScrollChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMTextCtrl, "ScrollTime", m_sScrollTime, OnScrollTimeChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMTextCtrl, "ScrollSteps", m_sScrollSteps, OnScrollStepsChanged, VT_I2)
	DISP_FUNCTION(CTMTextCtrl, "Initialize", Initialize, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMTextCtrl, "GetMinHeight", GetMinHeight, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMTextCtrl, "GetCurrentLine", GetCurrentLine, VT_I4, VTS_NONE)
	DISP_FUNCTION(CTMTextCtrl, "ResizeFont", ResizeFont, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMTextCtrl, "IsFirstLine", IsFirstLine, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMTextCtrl, "IsLastLine", IsLastLine, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMTextCtrl, "Next", Next, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMTextCtrl, "Previous", Previous, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMTextCtrl, "SetLineObject", SetLineObject, VT_I2, VTS_I4 VTS_I2)
	DISP_FUNCTION(CTMTextCtrl, "SetMaxWidth", SetMaxWidth, VT_I2, VTS_I2 VTS_I2)
	DISP_FUNCTION(CTMTextCtrl, "SetPlaylist", SetPlaylist, VT_I2, VTS_I4 VTS_I2)
	DISP_FUNCTION(CTMTextCtrl, "GetCharHeight", GetCharHeight, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMTextCtrl, "GetCharWidth", GetCharWidth, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMTextCtrl, "GetLogFont", GetLogFont, VT_I2, VTS_I4)
	DISP_FUNCTION(CTMTextCtrl, "IsReady", IsReady, VT_BOOL, VTS_NONE)
	DISP_FUNCTION(CTMTextCtrl, "SetLine", SetLine, VT_I2, VTS_I4 VTS_I4 VTS_I4 VTS_I2)
	DISP_FUNCTION(CTMTextCtrl, "GetRegisteredPath", GetRegisteredPath, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMTextCtrl, "GetClassIdString", GetClassIdString, VT_BSTR, VTS_NONE)
	DISP_STOCKPROP_BACKCOLOR()
	DISP_STOCKPROP_FONT()
	DISP_STOCKPROP_FORECOLOR()
	//}}AFX_DISPATCH_MAP
	DISP_FUNCTION_ID(CTMTextCtrl, "AboutBox", DISPID_ABOUTBOX, AboutBox, VT_EMPTY, VTS_NONE)
	
	//	Added rev 5.1
	DISP_PROPERTY_NOTIFY_ID(CTMTextCtrl, "UseLineColor", DISPID_USELINECOLOR, m_bUseLineColor, OnUseLineColorChanged, VT_BOOL)

	//	Added rev 6.1.0
	DISP_PROPERTY_EX_ID(CTMTextCtrl, "VerTextShort", DISPID_VERTEXTSHORT, GetVerTextShort, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_EX_ID(CTMTextCtrl, "VerQEF", DISPID_VERQEF, GetVerQEF, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX_ID(CTMTextCtrl, "VerBuildDate", DISPID_VERBUILDDATE, GetVerBuildDate, SetNotSupported, VT_BSTR)

	//	Added rev 6.3.4
	DISP_PROPERTY_NOTIFY_ID(CTMTextCtrl, "ShowText", DISPID_SHOWTEXT, m_bShowText, OnShowTextChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY_ID(CTMTextCtrl, "BulletStyle", DISPID_BULLETSTYLE, m_sBulletStyle, OnBulletStyleChanged, VT_I2)
	DISP_PROPERTY_NOTIFY_ID(CTMTextCtrl, "BulletMargin", DISPID_BULLETMARGIN, m_sBulletMargin, OnBulletMarginChanged, VT_I2)

END_DISPATCH_MAP()

// Event map
BEGIN_EVENT_MAP(CTMTextCtrl, COleControl)
	//{{AFX_EVENT_MAP(CTMTextCtrl)
	EVENT_CUSTOM("HeightChange", FireHeightChange, VTS_I2)
	//}}AFX_EVENT_MAP
END_EVENT_MAP()

// Property pages
BEGIN_PROPPAGEIDS(CTMTextCtrl, 2)
	//PROPPAGEID(CTMTextProperties::guid)
	PROPPAGEID(CLSID_CColorPropPage)
	PROPPAGEID(CLSID_CFontPropPage)
END_PROPPAGEIDS(CTMTextCtrl)

/* Replace 4 */
IMPLEMENT_OLECREATE_EX(CTMTextCtrl, "TMTEXT6.TMTextCtrl.1",
	0xaa52288d, 0x2a50, 0x494f, 0x98, 0xfe, 0xff, 0xf0, 0xd9, 0xfb, 0xde, 0x56)

// Type library ID and version
IMPLEMENT_OLETYPELIB(CTMTextCtrl, _tlid, _wVerMajor, _wVerMinor)
IMPLEMENT_OLECTLTYPE(CTMTextCtrl, IDS_TMTEXT, _dwTMTextOleMisc)

IMPLEMENT_DYNCREATE(CTMTextCtrl, COleControl)

// Interface map for IObjectSafety
BEGIN_INTERFACE_MAP(CTMTextCtrl, COleControl )
	INTERFACE_PART(CTMTextCtrl, IID_IObjectSafety, ObjSafety)
END_INTERFACE_MAP()

//==============================================================================
//
// 	Function Name:	ProcessTimer()
//
// 	Description:	This is the callback function for timer events
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CALLBACK ProcessTimer(UINT uId, UINT uMsg, DWORD dwUser, DWORD, DWORD)
{
	CTMTextCtrl* pTMText = (CTMTextCtrl*)dwUser;

	//	Forward the event to the main window
	if(pTMText)
		pTMText->OnScrollTimer();
}
 
//==============================================================================
//
// 	Function Name:	CTMTextCtrl::CTMTextCtrlFactory::UpdateRegistry
//
// 	Description:	Adds or removes system registry entries for this control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTMTextCtrl::CTMTextCtrlFactory::UpdateRegistry(BOOL bRegister)
{
	BOOL	bReturn;
	HRESULT	hResult;

	if(bRegister)
	{
		bReturn = AfxOleRegisterControlClass(AfxGetInstanceHandle(),
											 m_clsid,
											 m_lpszProgID,
											 IDS_TMTEXT,
											 IDB_TMTEXT,
											 afxRegApartmentThreading,
											 _dwTMTextOleMisc,
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
// 	Function Name:	CTMTextCtrl::XObjSafety::AddRef()
//
// 	Description:	This function is called to attach to the nested
//					IObjectSafety dispatch interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMTextCtrl::XObjSafety::AddRef()
{
    METHOD_PROLOGUE(CTMTextCtrl, ObjSafety)
    return pThis->ExternalAddRef();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::XObjSafety::GetInterfaceSafetyOptions()
//
// 	Description:	This function is called to get this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMTextCtrl::XObjSafety::GetInterfaceSafetyOptions( 
		/* [in]  */ REFIID riid,
        /* [out] */ DWORD __RPC_FAR *pdwSupportedOptions,
        /* [out] */ DWORD __RPC_FAR *pdwEnabledOptions)
{
	HRESULT			hReturn;
	IUnknown FAR*	pInterface;

	METHOD_PROLOGUE(CTMTextCtrl, ObjSafety)

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
// 	Function Name:	CTMTextCtrl::XObjSafety::QueryInterface()
//
// 	Description:	This function is called to query the IObjectSafety interface
//					for the requested method/property.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT FAR EXPORT CTMTextCtrl::XObjSafety::QueryInterface(REFIID iid, 
														  void FAR* FAR* ppvObj)
{
    METHOD_PROLOGUE(CTMTextCtrl, ObjSafety)
    return (HRESULT)pThis->ExternalQueryInterface(&iid, ppvObj);
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::XObjSafety::Release()
//
// 	Description:	This function is called to detach from the IObjectSafety
//					dispatch interface.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMTextCtrl::XObjSafety::Release()
{
    METHOD_PROLOGUE(CTMTextCtrl, ObjSafety)
    return pThis->ExternalRelease();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::XObjSafety::SetInterfaceSafetyOptions()
//
// 	Description:	This function is called to set this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMTextCtrl::XObjSafety::SetInterfaceSafetyOptions( 
        /* [in] */ REFIID riid,
        /* [in] */ DWORD dwOptionSetMask,
        /* [in] */ DWORD dwEnabledOptions)
{
	IUnknown FAR* pInterface;

    METHOD_PROLOGUE(CTMTextCtrl, ObjSafety)
	
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
// 	Function Name:	CTMTextCtrl::AboutBox()
//
// 	Description:	This method will display the control's about box
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::AboutBox()
{
	CDialog dlgAbout(IDD_ABOUTBOX_TMTEXT, this);
	dlgAbout.DoModal();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::CalcScrollPeriod()
//
// 	Description:	This function will calculate the timer period for smooth
//					scrolling
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::CalcScrollPeriod()
{
    ASSERT(m_sScrollSteps != 0);
	if(m_sScrollSteps == 0)
	{
		m_uPeriod = MINIMUM_SCROLLPERIOD;
	}
	else
	{
		m_uPeriod = (m_sScrollTime / m_sScrollSteps);
		if(m_uPeriod < MINIMUM_SCROLLPERIOD)
			m_uPeriod = MINIMUM_SCROLLPERIOD;
	}

}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::CheckVersion()
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
BOOL CTMTextCtrl::CheckVersion(DWORD dwVersion)
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
	              "TMText ActiveX control. You should upgrade tm_text6.ocx "
				  "as soon as possible\n\n%s", 
				  (wMinor > _wVerMinor) ? "a newer" : "an older", strVersion);

	MessageBeep(MB_ICONEXCLAMATION);
	MessageBox(strMsg, "TrialMax Error", MB_ICONEXCLAMATION | MB_OK);

	//	The versions do not match
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::CTMTextCtrl()
//
// 	Description:	This is the constructor for CTMTextCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMTextCtrl::CTMTextCtrl()
{
	InitializeIIDs(&IID_DTMText6, &IID_DTMText6Events);

	//	Initialize the local data
	m_pPlaylist = 0;
	m_pLine = 0;
	m_pNext = 0;
	m_pPending = 0;
	m_pFont = 0;
	m_iCharHeight = 0;
	m_iMaxWidth = 0;
	m_iCharWidth = 0;
	m_uPeriod = 0;
	m_uTimer = 0;
	m_sStepCount = 0;
	m_fStepSize = 0;
	m_bScrolling = FALSE;
	m_bProcessingTimer = FALSE;
	ZeroMemory(&m_rcTextBounds, sizeof(m_rcTextBounds));
	ZeroMemory(&m_rcBulletBounds, sizeof(m_rcBulletBounds));
	ZeroMemory(&m_lfFont, sizeof(m_lfFont));

	//	Get the registry information
	GetRegistration();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::~CTMTextCtrl()
//
// 	Description:	This is the destructor for CTMTextCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMTextCtrl::~CTMTextCtrl()
{
	//	Make sure the timer is shut down
	KillScrollTimer();

	//	Flush the text lists
	m_Available.Flush(FALSE);
	m_Lines.Flush(FALSE);

	//	Delete the font if we have one
	if(m_pFont)
		delete m_pFont;
}		

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::DoPropertyExchange()
//
// 	Description:	This function manages the exchange of persistant 
//					properties
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::DoPropExchange(CPropExchange* pPX)
{
	BOOL bAutoInit = FALSE;
	BOOL bEnableErrors = FALSE;
	BOOL bCombine = FALSE;
	BOOL bShowPageLine = FALSE;
	BOOL bDisplayLines = FALSE;
	BOOL bHighlightLines = FALSE;
	BOOL bHighlightColor = FALSE;
	BOOL bHighlightTextColor = FALSE;
	BOOL bTopMargin = FALSE;
	BOOL bLeftMargin = FALSE;
	BOOL bBottomMargin = FALSE;
	BOOL bRightMargin = FALSE;
	BOOL bMaxCharsPerLine = FALSE;
	BOOL bUseAvgCharWidth = FALSE;
	BOOL bResizeOnChange = FALSE;
	BOOL bSmoothScroll = FALSE;
	BOOL bScrollSteps = FALSE;
	BOOL bScrollTime = FALSE;
	BOOL bUseLineColor = FALSE;
	BOOL bShowText = FALSE;
	BOOL bBulletStyle = FALSE;
	BOOL bBulletMargin = FALSE;

	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
	COleControl::DoPropExchange(pPX);

	//CheckVersion(pPX->GetVersion());

	//	Load the control's persistent properties
	try
	{
		bAutoInit = PX_Bool(pPX, _T("AutoInit"), m_bAutoInit, TMTEXT_AUTOINIT);
		bEnableErrors = PX_Bool(pPX, _T("EnableErrors"), m_bEnableErrors, TRUE);
		bCombine = PX_Bool(pPX, _T("Combine"), m_bCombine, TMTEXT_COMBINEDESIGNATIONS);
		bShowPageLine = PX_Bool(pPX, _T("ShowPageLine"), m_bShowPgLn, TMTEXT_SHOWPAGELINE);
		bDisplayLines = PX_Short(pPX, _T("DisplayLines"), m_sDisplay, TMTEXT_DISPLAYLINES);
		bHighlightLines = PX_Short(pPX, _T("HighlightLines"), m_sHighlight, TMTEXT_HIGHLINES);
		bHighlightColor = PX_Color(pPX, _T("HighlightColor"), m_ocHighlight, TMTEXT_HIGHCOLOR);
		bHighlightTextColor = PX_Color(pPX, _T("HighlightTextColor"), m_ocHighlightText, TMTEXT_HIGHTEXTCOLOR);
		bTopMargin = PX_Short(pPX, _T("TopMargin"), m_sTopMargin, TMTEXT_TOPMARGIN);
		bLeftMargin = PX_Short(pPX, _T("LeftMargin"), m_sLeftMargin, TMTEXT_LEFTMARGIN);
		bBottomMargin = PX_Short(pPX, _T("BottomMargin"), m_sBottomMargin, TMTEXT_BOTTOMMARGIN);
		bRightMargin = PX_Short(pPX, _T("RightMargin"), m_sRightMargin, TMTEXT_RIGHTMARGIN);
		bMaxCharsPerLine = PX_Short(pPX, _T("MaxCharsPerLine"), m_sMaxCharsPerLine, TMTEXT_MAXCHARSPERLINE);
		bUseAvgCharWidth = PX_Bool(pPX, _T("UseAvgCharWidth"), m_bUseAvgCharWidth, TMTEXT_USEAVGCHARWIDTH);
		bResizeOnChange = PX_Bool(pPX, _T("ResizeOnChange"), m_bResizeOnChange, TMTEXT_RESIZEONCHANGE);
		bSmoothScroll = PX_Bool(pPX, _T("SmoothScroll"), m_bSmoothScroll, TMTEXT_SMOOTHSCROLL);
		bScrollSteps = PX_Short(pPX, _T("ScrollSteps"), m_sScrollSteps, TMTEXT_SCROLLSTEPS);
		bScrollTime = PX_Short(pPX, _T("ScrollTime"), m_sScrollTime, TMTEXT_SCROLLTIME);
		bUseLineColor = PX_Bool(pPX, _T("UseLineColor"), m_bUseLineColor, TMTEXT_USELINECOLOR);
		bShowText = PX_Bool(pPX, _T("ShowText"), m_bShowText, TMTEXT_SHOWTEXT);
		bBulletStyle = PX_Short(pPX, _T("BulletStyle"), m_sBulletStyle, TMTEXT_BULLETSTYLE);
		bBulletMargin = PX_Short(pPX, _T("BulletMargin"), m_sBulletMargin, TMTEXT_BULLETMARGIN);
	}
	catch(...)
	{
		if(!bAutoInit) m_bAutoInit = TMTEXT_AUTOINIT;
		if(!bEnableErrors) m_bEnableErrors = TRUE;
		if(!bCombine) m_bCombine = TMTEXT_COMBINEDESIGNATIONS;
		if(!bShowPageLine) m_bShowPgLn = TMTEXT_SHOWPAGELINE;
		if(!bDisplayLines) m_sDisplay = TMTEXT_DISPLAYLINES;
		if(!bHighlightLines) m_sHighlight = TMTEXT_HIGHLINES;
		if(!bHighlightColor) m_ocHighlight = TMTEXT_HIGHCOLOR;
		if(!bHighlightTextColor) m_ocHighlightText = TMTEXT_HIGHTEXTCOLOR;
		if(!bTopMargin) m_sTopMargin = TMTEXT_TOPMARGIN;
		if(!bLeftMargin) m_sLeftMargin = TMTEXT_LEFTMARGIN;
		if(!bBottomMargin) m_sBottomMargin = TMTEXT_BOTTOMMARGIN;
		if(!bRightMargin) m_sRightMargin = TMTEXT_RIGHTMARGIN;
		if(!bMaxCharsPerLine) m_sMaxCharsPerLine = TMTEXT_MAXCHARSPERLINE;
		if(!bUseAvgCharWidth) m_bUseAvgCharWidth = TMTEXT_USEAVGCHARWIDTH;
		if(!bResizeOnChange) m_bResizeOnChange = TMTEXT_RESIZEONCHANGE;
		if(!bSmoothScroll) m_bSmoothScroll = TMTEXT_SMOOTHSCROLL;
		if(!bScrollSteps) m_sScrollSteps = TMTEXT_SCROLLSTEPS;
		if(!bScrollTime) m_sScrollTime = TMTEXT_SCROLLTIME;
		if(!bUseLineColor) m_bUseLineColor = TMTEXT_USELINECOLOR;
		if(!bShowText) m_bShowText = TMTEXT_SHOWTEXT;
		if(!bBulletStyle) m_sBulletStyle = TMTEXT_BULLETSTYLE;
		if(!bBulletMargin) m_sBulletMargin = TMTEXT_BULLETMARGIN;
	}	

/*
	if(pPX->IsLoading())
	{
		//	Set default values for properties added after initial release
		//	of the major version
		//
		//	NOTE:	The drop through design of the switch statement is intentional
		switch(LOWORD(pPX->GetVersion()))
		{
			case 0:

		}
	}
*/

}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::DrawBullet()
//
// 	Description:	This function will draw the bullet symbol in the specified
//					location 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::DrawBullet(CDC* pdc, CTextLine* pLine, int iTop, int iBottom, BOOL bHighlighted) 
{
	CBrush		brBullet;
	CBrush		brBackground;
	CPen		penBullet;
	RECT		rcBullet;
	COLORREF	crBackColor;
	COLORREF	crForeColor;
	CBrush*		pOldBrush;
	CPen*		pOldPen;
	POINT		aDiamond[4];
	int			iCx;
	int			iCy;

	ASSERT(pdc);

	//	Ignore blank lines allocated by this control
	if((m_sBulletStyle != TMTEXT_BULLET_NONE) && (pLine->m_lDesignationId != TMTEXT_ALLOCID))
	{
		rcBullet.left = m_rcBulletBounds.left;
		rcBullet.right = m_rcBulletBounds.right;
		rcBullet.top = iTop;
		rcBullet.bottom = iBottom;

		//	If the user has requested the colors assigned in the database then we do not
		//	want to highlight the bullet
		if((bHighlighted == TRUE) && (m_bUseLineColor == TRUE))
			bHighlighted = FALSE;

		//	Get the colors
		crForeColor = GetForeColorEx(pLine, bHighlighted);
		crBackColor = GetBackColorEx(pLine, bHighlighted);

		brBullet.CreateSolidBrush(crForeColor);
		brBackground.CreateSolidBrush(crBackColor);
		penBullet.CreatePen(PS_SOLID | PS_INSIDEFRAME, 1, crForeColor);

		//	Fill the background
		pdc->FillRect(&rcBullet, &brBackground);

		//	Reduce the rectangle for drawing the actual bullet image
		rcBullet.left   += m_sBulletMargin;
		rcBullet.right  -= m_sBulletMargin;
		rcBullet.top    += m_sBulletMargin;
		rcBullet.bottom -= m_sBulletMargin;

		pOldBrush = pdc->SelectObject(&brBullet);
		pOldPen = pdc->SelectObject(&penBullet);

		//	Draw the image
		switch(m_sBulletStyle)
		{
			case TMTEXT_BULLET_CIRCLE:

				pdc->Ellipse(&rcBullet);
				break;

			case TMTEXT_BULLET_DIAMOND:

				//	Compute the center of the bullet rectangle
				iCx = rcBullet.left + ((rcBullet.right - rcBullet.left) / 2);
				iCy = rcBullet.top + ((rcBullet.bottom - rcBullet.top) / 2);

				//	Set the points used to draw the diamond
				aDiamond[0].x = rcBullet.left;
				aDiamond[0].y = iCy;
				
				aDiamond[1].x = iCx;
				aDiamond[1].y = rcBullet.top;
				
				aDiamond[2].x = rcBullet.right;
				aDiamond[2].y = iCy;
				
				aDiamond[3].x = iCx;
				aDiamond[3].y = rcBullet.bottom;

				pdc->Polygon(aDiamond, 4);
				break;

			case TMTEXT_BULLET_SQUARE:
			default:

				pdc->FillRect(&rcBullet, &brBullet);
				break;
		}

		//	Cleanup
		if(pOldBrush) pdc->SelectObject(pOldBrush);
		if(pOldPen) pdc->SelectObject(pOldPen);
	}

}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::DrawLine()
//
// 	Description:	This function will draw the specified line into the dc
//					at the requested location
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::DrawLine(CDC* pdc, CTextLine* pLine, int iTop, int iBottom, BOOL bHighlighted) 
{
	RECT		rcLine;
	CBrush		brBackground;
	int			iOldMode;
	int			iOldBkColor;
	int			iOldTextColor;
	CString		strPageLine;
	UINT		uFlags = (DT_LEFT | DT_SINGLELINE | DT_BOTTOM | DT_NOPREFIX);
	COLORREF	crForeColor;
	COLORREF	crBackColor;

	ASSERT(pdc);
	ASSERT(pLine);

	//	Get the colors
	crForeColor = GetForeColorEx(pLine, bHighlighted);
	crBackColor = GetBackColorEx(pLine, bHighlighted);

	//	Set the top and bottom coordinates
	rcLine.top    = iTop;
	rcLine.bottom = iBottom;
	rcLine.left   = m_rcTextBounds.left;
	rcLine.right  = m_rcTextBounds.right;

	//	Fill the rectangle that contains this line
	brBackground.CreateSolidBrush(crBackColor);
	pdc->FillRect(&rcLine, &brBackground);

	//	Don't bother doing anything else if scrolling text is disabled
	//	for this line
	if(pLine->m_bEnableScroll == TRUE)
	{
		//	Initialize the dc
		iOldBkColor = pdc->SetBkColor(crBackColor);
		iOldTextColor = pdc->SetTextColor(crForeColor);
		iOldMode = pdc->SetBkMode(TRANSPARENT);

		//	Do we need to display the page and line numbers?
		if(m_bShowPgLn)
		{
			//	Draw the page:line numbers
			rcLine.right = rcLine.left + (m_iCharWidth * PAGELINE_CHARACTERS);
				
			//	Is this a blank line allocated by this control?
			if(pLine->m_lDesignationId == TMTEXT_ALLOCID)
				strPageLine.Empty();
			else
				strPageLine.Format("%ld:%02d ", pLine->m_lPageNum, pLine->m_lLineNum);
			pdc->DrawText(strPageLine, &rcLine, 
						  DT_RIGHT | DT_SINGLELINE | DT_BOTTOM);

			//	Now draw the transcript text if the option is turned on 
			if(m_bShowText == TRUE)
			{
				rcLine.left  = rcLine.right;
				rcLine.right = m_rcTextBounds.right;
				pdc->DrawText(pLine->m_strText, &rcLine, uFlags);
			}
										   
		}
		else if(m_bShowText == TRUE)
		{
			rcLine.right = m_rcTextBounds.right;
			pdc->DrawText(pLine->m_strText, &rcLine, uFlags);
		}

		//	Restore the dc
		pdc->SetBkColor(iOldBkColor);
		pdc->SetTextColor(iOldTextColor);
		pdc->SetBkMode(iOldMode);

	}// if(pLine->m_bEnableScroll == TRUE)

}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::DrawLines()
//
// 	Description:	This function will draw the lines of text into the dc 
//					provided by the caller
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::DrawLines(CDC* pdc) 
{
	CBrush		brBackground;
	CBrush		brHighlight;
	RECT		rcClient;
	RECT		rcHighlight;
	CTextLine*	pLine;
	CFont*		pOldFont;
	int			iAbove;
	int			iTop;
	int			iBottom;
	BOOL		bHighlighted = FALSE;

	//	Lock the critical section so that the playlist can't be reset while
	//	we are drawing
	m_CriticalSection.Lock();

	//	Fill the full client area with the background color
	brBackground.CreateSolidBrush(TranslateColor(GetBackColor()));
	GetClientRect(&rcClient);
	pdc->FillRect(&rcClient, &brBackground);

	//	Don't bother doing anything else if the list is empty
	if(m_Lines.IsEmpty())
	{
		m_CriticalSection.Unlock();
		return;
	}

	//	Make sure we have the correct layout
	RecalcLayout();

	//	Are we highlighting lines?
	if(m_sHighlight > 0)
	{
		//	How many lines appear above the highlighted text?
		//
		//	NOTE:	We round up so that the number of blank lines (if present)
		//			matches up with our highlight
		if(m_sHighlight > m_sDisplay)
			m_sHighlight = 1;
		iAbove = (int)((((float)m_sDisplay - (float)m_sHighlight) / 2.0f) + 0.5);

		//	Calculate the coordinates of the highlight rectangle
		rcHighlight.left = m_rcTextBounds.left;
		rcHighlight.right = m_rcTextBounds.right;
		rcHighlight.top = m_sTopMargin + (iAbove * m_iCharHeight);
		rcHighlight.bottom = rcHighlight.top + (m_sHighlight * m_iCharHeight);

		//	Draw the highlighted region
		//
		//	NOTE:	We do this because there may initially be highlighted area without
		//			any text
		brHighlight.CreateSolidBrush(GetBackColorEx(m_Lines.First(), TRUE));
		pdc->FillRect(&rcHighlight, &brHighlight);
	}

	//	Initialize the dc
	if(m_pFont)
		pOldFont = pdc->SelectObject(m_pFont);
	else
		pOldFont = SelectStockFont(pdc);

	//	Draw the display text
	iTop = m_sTopMargin;
	iBottom = iTop + m_iCharHeight;
	pLine = m_Lines.First();
	for(int i = 0; i < m_sDisplay && pLine; i++)
	{
		//	Do we need to switch the text color?
		if(m_sHighlight > 0)
		{
			if(i == iAbove)
				bHighlighted = TRUE;
			else if(i == (iAbove + m_sHighlight))
				bHighlighted = FALSE;
		}

		//	Draw the bullet for this line
		DrawBullet(pdc, pLine, iTop, iBottom, bHighlighted);

		//	Draw this line of text
		DrawLine(pdc, pLine, iTop, iBottom, bHighlighted);

		//	Move down one line
		iTop = iBottom;
		iBottom = iTop + m_iCharHeight;

		//	Get the next line
		pLine = m_Lines.Next();
	}

	//	Unlock the critical section now that we're done with the lines
	m_CriticalSection.Unlock();

	//	Restore the dc
	if(pOldFont)
		pdc->SelectObject(pOldFont);
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::DrawLines()
//
// 	Description:	This is an alternate form of the function. It allows the
//					caller to specify a verticle offset when drawing the lines
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::DrawLines(CDC* pdc, float fOffset) 
{
	CBrush		brBackground;
	CBrush		brHighlight;
	RECT		rcClient;
	RECT		rcHighlight;
	CTextLine*	pLine;
	CFont*		pOldFont;
	int			iAbove;
	int			iTop;
	int			iBottom;

	//	Lock the critical section so that the playlist can't be reset while
	//	we are drawing
	m_CriticalSection.Lock();

	//	Fill the full client area with the background color
	brBackground.CreateSolidBrush(TranslateColor(GetBackColor()));
	GetClientRect(&rcClient);
	pdc->FillRect(&rcClient, &brBackground);

	//	Don't bother doing anything else if the list is empty
	if(m_Lines.IsEmpty())
	{
		m_CriticalSection.Unlock();
		return;
	}

	//	Make sure we have the correct layout
	RecalcLayout();

	//	Are we highlighting lines?
	if(m_sHighlight > 0)
	{
		//	How many lines appear above the highlighted text?
		//
		//	NOTE:	We round up so that the number of blank lines (if present)
		//			matches up with our highlight
		if(m_sHighlight > m_sDisplay)
			m_sHighlight = 1;
		iAbove = (int)((((float)m_sDisplay - (float)m_sHighlight) / 2.0f) + 0.5);

		//	Calculate the coordinates of the highlight rectangle
		rcHighlight.left = m_rcTextBounds.left;
		rcHighlight.right = m_rcTextBounds.right;
		rcHighlight.top = m_sTopMargin + (iAbove * m_iCharHeight);
		rcHighlight.bottom = rcHighlight.top + (m_sHighlight * m_iCharHeight);

		//	Draw the highlighted region
		//
		//	NOTE:	We do this because there may initially be highlighted area without
		//			any text
		brHighlight.CreateSolidBrush(GetBackColorEx(m_Lines.First(), TRUE));
		pdc->FillRect(&rcHighlight, &brHighlight);
	}

	//	Initialize the dc
	if(m_pFont)
		pOldFont = pdc->SelectObject(m_pFont);
	else
		pOldFont = SelectStockFont(pdc);

	//	Draw the display text
	iTop = m_sTopMargin - (int)fOffset;
	iBottom = iTop + m_iCharHeight;
	pLine = m_Lines.First();
	for(int i = 0; i < (m_sDisplay + 1) && pLine; i++)
	{
		//	Is the top of the line above the highlight?
		if((m_sHighlight > 0) && (iTop < rcHighlight.top))
		{
			//	Draw the bullet for this line
			DrawBullet(pdc, pLine, iTop, iBottom, FALSE);

			//	Draw the full rectangle in the non-highlight color
			DrawLine(pdc, pLine, iTop, iBottom, FALSE);

			//	Is the line partially in the highlight area?
			if(iBottom > rcHighlight.top)
			{
				DrawLine(pdc, pLine, rcHighlight.top, iBottom, TRUE);
			DrawBullet(pdc, pLine, iTop, iBottom, TRUE);
			}

		}

		//	Is it within the highlight area?
		else if((m_sHighlight > 0) && (iTop < rcHighlight.bottom))
		{
			//	Draw the bullet for this line
			DrawBullet(pdc, pLine, iTop, iBottom, TRUE);

			//	Draw the full line in the highlight color
			DrawLine(pdc, pLine, iTop, iBottom, TRUE);

			//	Is the line partially outside the highlight
			if(iBottom > rcHighlight.bottom)
			{
				DrawLine(pdc, pLine, rcHighlight.bottom, iBottom, FALSE);
			//	Draw the bullet for this line
			DrawBullet(pdc, pLine, iTop, iBottom, FALSE);

			}
		}

		//	Is must be totally below the highlight or no lines being highlighted
		else
		{
			DrawBullet(pdc, pLine, iTop, iBottom, FALSE);
			DrawLine(pdc, pLine, iTop, iBottom, FALSE);
		}

		//	Move down one line
		iTop = iBottom;
		iBottom = iTop + m_iCharHeight;

		//	Get the next line
		pLine = m_Lines.Next();
	}

	//	Unlock the critical section now that we're done with the lines
	m_CriticalSection.Unlock();

	//	Restore the dc
	if(pOldFont)
		pdc->SelectObject(pOldFont);
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::GetBackColor()
//
// 	Description:	This method is called to get the background color required
//					to draw the line
//
// 	Returns:		The appropriate background color
//
//	Notes:			None
//
//==============================================================================
COLORREF CTMTextCtrl::GetBackColorEx(CTextLine* pLine, BOOL bHighlighted)
{
	//	Use the line color if it is assigned a valid color
	if((m_bUseLineColor == TRUE) && ((pLine->m_crColor & 0xFF000000) == 0))
	{
		//	Use the configured background color if we are not highlighting
		if(bHighlighted == FALSE)
			return TranslateColor(COleControl::GetBackColor());
		else
			return pLine->m_crColor;
	}
	else
	{
		if(bHighlighted == TRUE)
			return TranslateColor(m_ocHighlight);
		else
			return TranslateColor(COleControl::GetBackColor());
	}

}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::GetCharHeight()
//
// 	Description:	This method is called to get the character cell height
//
// 	Returns:		The current height
//
//	Notes:			None
//
//==============================================================================
short CTMTextCtrl::GetCharHeight() 
{
	return (short)m_iCharHeight;
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::GetCharWidth()
//
// 	Description:	This method is called to get the character cell width
//
// 	Returns:		The current width
//
//	Notes:			The value is related to the UseAvgCharWidth property
//
//==============================================================================
short CTMTextCtrl::GetCharWidth() 
{
	return (short)m_iCharWidth;
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::GetClassIdString()
//
// 	Description:	This method is called to get the class GUID as a null
//					terminated string.
//
// 	Returns:		The string equivalent of the CLASS GUID
//
//	Notes:			None
//
//==============================================================================
BSTR CTMTextCtrl::GetClassIdString() 
{
	CString strClsId = m_tmVersion.GetClsId();
	return strClsId.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::GetCurrentLine()
//
// 	Description:	This method is called to get the current line
//
// 	Returns:		The pointer to the current line object
//
//	Notes:			None
//
//==============================================================================
long CTMTextCtrl::GetCurrentLine() 
{
	return (long)m_pLine;
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::GetDesignation()
//
// 	Description:	This function will get the designation with the id specified
//					by the caller
//
// 	Returns:		A pointer to the requested designation if found
//
//	Notes:			None
//
//==============================================================================
CDesignation* CTMTextCtrl::GetDesignation(long lId)
{
	CDesignation*	pDesignation;
	POSITION		Pos;

	//	Do we have a valid playlist?
	if(!m_pPlaylist)
		return 0;
	
	//	Check each of the designations
	Pos = m_pPlaylist->m_Designations.GetHeadPosition();
	while(Pos)
	{
		pDesignation = (CDesignation*)m_pPlaylist->m_Designations.GetNext(Pos);
		
		//	Is this the one we are looking for?
		if(pDesignation && pDesignation->m_lTertiaryId == lId)
			return pDesignation;
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::GetForeColor()
//
// 	Description:	This method is called to get the foreground color required
//					to draw the line
//
// 	Returns:		The appropriate foreground color
//
//	Notes:			None
//
//==============================================================================
COLORREF CTMTextCtrl::GetForeColorEx(CTextLine* pLine, BOOL bHighlighted)
{
	//	Use the line color if it is assigned a valid color
	if((m_bUseLineColor == TRUE) && ((pLine->m_crColor & 0xFF000000) == 0))
	{
		//	Use the configured background color if we are highlighting
		if(bHighlighted == TRUE)
			return TranslateColor(COleControl::GetBackColor());
		else
			return pLine->m_crColor;
	}
	else
	{
		if(bHighlighted == TRUE)
			return TranslateColor(m_ocHighlightText);
		else
			return TranslateColor(COleControl::GetForeColor());
	}

}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::GetLogFont()
//
// 	Description:	This method is called to get the logical font descriptor for
//					the current text font
//
// 	Returns:		TMTEXT_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMTextCtrl::GetLogFont(long lLogFont) 
{
	LOGFONT*	pLogFont = (LOGFONT*)lLogFont;
	HFONT		hStock;
	CFont*		pStock;

	//	Do we have a valid text font?
	if(m_pFont)
	{
		memcpy(pLogFont, &m_lfFont, sizeof(m_lfFont));
	}
	else
	{
		//	Get a handle to the stock font object
		if((hStock = InternalGetFont().GetFontHandle()) == 0)
		{
			m_Errors.Handle(0, IDS_TMTEXT_INVALIDLOGFONT);
			return TMTEXT_INVALIDLOGFONT;
		}
	
		//	Get a CFont object we can use to get the stock font information
		if((pStock = CFont::FromHandle(hStock)) == 0)
		{
			m_Errors.Handle(0, IDS_TMTEXT_INVALIDLOGFONT);
			return TMTEXT_INVALIDLOGFONT;
		}

		//	Get the LOGFONT structure for the stock font
		if(!pStock->GetLogFont(pLogFont))
		{
			m_Errors.Handle(0, IDS_TMTEXT_INVALIDLOGFONT);
			return TMTEXT_INVALIDLOGFONT;
		}
	}

	return TMTEXT_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::GetMinHeight()
//
// 	Description:	This method allows the caller to retrieve the height 
//					required to display all lines of text
//
// 	Returns:		The required height in pixels
//
//	Notes:			None
//
//==============================================================================
short CTMTextCtrl::GetMinHeight() 
{
	return (m_sDisplay * m_iCharHeight + m_sTopMargin + m_sBottomMargin);
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::GetRegisteredPath()
//
// 	Description:	This method is called to get the ocx path stored in the
//					system registry.
//
// 	Returns:		The path to the ocx stored in the registry
//
//	Notes:			None
//
//==============================================================================
BSTR CTMTextCtrl::GetRegisteredPath() 
{
	CString strRegistered = m_tmVersion.GetFileSpec();
	return strRegistered.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::GetRegistration()
//
// 	Description:	This function is called to get the control's registration
//					information.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::GetRegistration() 
{
	CLSID	clsid;

	//	Get the GUID 
	GetClassID(&clsid);

	//	Initialize the version information
	m_tmVersion.InitFromClsId("TMText", "Scrolling Text Control", clsid);

}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::GetVerBuild()
//
// 	Description:	This method is called to get the value of the VerBuild
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMTextCtrl::GetVerBuild() 
{
	return m_tmVersion.GetBuild();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::GetVerBuildDate()
//
// 	Description:	This method is called to get the value of the VerBuildDate
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMTextCtrl::GetVerBuildDate() 
{
	CString strBuildDate = m_tmVersion.GetBuildDate();
	return strBuildDate.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::GetVerMajor()
//
// 	Description:	This method is called to get the value of the VerMajor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMTextCtrl::GetVerMajor() 
{
	return m_tmVersion.GetMajor();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::GetVerMinor()
//
// 	Description:	This method is called to get the value of the VerMinor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMTextCtrl::GetVerMinor() 
{
	return m_tmVersion.GetMinor();
}


//==============================================================================
//
// 	Function Name:	CTMTextCtrl::GetVerQEF()
//
// 	Description:	This method is called to get the value of the VerQEF
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMTextCtrl::GetVerQEF() 
{
	return m_tmVersion.GetUpdate();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::GetVerTextLong()
//
// 	Description:	This method is called to get the value of the VerTextLong
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMTextCtrl::GetVerTextLong() 
{
	CString strVer = m_tmVersion.GetTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::GetVerTextShort()
//
// 	Description:	This method is called to get the value of the VerTextShort
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMTextCtrl::GetVerTextShort() 
{
	CString strVer = m_tmVersion.GetShortTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::Initialize()
//
// 	Description:	This function will construct and initialize the depobar
//
// 	Returns:		TMTX_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMTextCtrl::Initialize()
{
	RECT rcClient;

	//	Don't bother if not in user mode
	if(!AmbientUserMode())
		return TMTEXT_NOERROR;

	//	Set the maximum width to use the full client area
	GetClientRect(&rcClient);
	SetMaxWidth((short)(rcClient.right - rcClient.left), FALSE);

	//	Make sure the font is appropriately sized
	ResizeFont(TRUE);

	//	Calculate the smooth scrolling timer period
	CalcScrollPeriod();

    //	Set the resolution of the scroll timer
	timeBeginPeriod(MINIMUM_SCROLLPERIOD);

//	this->SetFocus();
//	::ShowCursor(true);
	return TMTEXT_NOERROR;
}	

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::IsFirstLine()
//
// 	Description:	This method is called to determine if the current line is
//					the first line of available text
//
// 	Returns:		TRUE if it is the first line
//
//	Notes:			None
//
//==============================================================================
short CTMTextCtrl::IsFirstLine() 
{
	if(m_pLine)
		return (m_pLine == m_Available.First());
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::IsLastLine()
//
// 	Description:	This method is called to determine if the current line is
//					the last line of available text
//
// 	Returns:		TRUE if it is the first line
//
//	Notes:			None
//
//==============================================================================
short CTMTextCtrl::IsLastLine() 
{
	if(m_pLine)
		return (m_pLine == m_Available.Last());
	else
		return FALSE;
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::IsReady()
//
// 	Description:	This method allows the caller to check if the control is
//					ready to display text
//
// 	Returns:		TRUE if ready
//
//	Notes:			None
//
//==============================================================================
BOOL CTMTextCtrl::IsReady() 
{
	return (m_pPlaylist != 0);
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::KillScrollTimer()
//
// 	Description:	This function will reset the timer used for smooth scrolling
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::KillScrollTimer()
{
	//	Do we have an active timer?
    if(!m_uTimer)
		return;

	//	Shut down the timer
    timeKillEvent(m_uTimer);
	m_uTimer = 0;
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::LoadAvailable()
//
// 	Description:	This function is called to load the list of available lines
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::LoadAvailable() 
{
	POSITION		Pos;
	CDesignations*	pDesignations;
	CDesignation*	pDesignation;

	//	Flush the current list
	m_Available.Flush(FALSE);

	//	Do we have a valid playlist?
	if(m_pPlaylist == 0)
		return;

	//	Get the list of designations
	pDesignations = &(m_pPlaylist->m_Designations);

	//	Are we combining all designations?
	if(m_bCombine)
	{
		//	Add all designation transcripts to the list
		Pos = pDesignations->GetHeadPosition();
		while(Pos)
		{
			if((pDesignation = (CDesignation*)pDesignations->GetNext(Pos)) != 0)
				m_Available.Add(&(pDesignation->m_Pages));
		}

	}
	else
	{
		//	If we don't have a current line we don't know what designation to
		//	use
		if(m_pLine == 0)
			return;

		//	Add the transcript text associated with this designation
		if((pDesignation = GetDesignation(m_pLine->m_lDesignationId)) != 0)
		{
			m_Available.Add(&(pDesignation->m_Pages));
		}
		else
		{
			m_pLine = 0;
			m_pNext = 0;
			m_pPending = 0;
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::LoadLines()
//
// 	Description:	This function will load the list of lines to be displayed
//					by the control
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::LoadLines()
{
	CTextLine*	pLine;
	int			iBefore;
	int			iAfter;
	int			i;
	long		lSort;

	//	Flush the current list of display text
	m_Lines.Flush(TRUE, TMTEXT_ALLOCID);
	m_pNext = 0;
	m_pPending = 0;

	//	Do we have a valid line selection?
	if(!m_pLine)
		return;

	//	This line MUST be in the list of available text. 
	if(m_Available.Find(m_pLine) == 0)
		return;

	//	Calculate the number of lines we will need before and after the current
	//	line. We always add one additional line to allow for smooth scrolling
	//	when needed
	iBefore = m_sDisplay / 2;
	iAfter  = m_sDisplay - iBefore;

	//	Add the lines that appear before the current line
	m_Available.SetPos(m_pLine);
	lSort = m_pLine->m_lDesignationOrder;
	for(i = 0; i < iBefore; i++)
	{
		//	Do we have a line available?
		if((pLine = m_Available.Prev()) == 0)
		{
			//	No line is available so we have to allocate a blank line
			pLine = new CTextLine();
			pLine->m_lDesignationId = TMTEXT_ALLOCID;

			//	Keep the color of the active line
			pLine->m_crColor = m_pLine->m_crColor;

			//	This makes sure we maintain the appropriate sort order
			pLine->m_lDesignationOrder = lSort - 1;
		}

		//	Add to our list of display lines
		ASSERT(pLine);

		m_Lines.Add(pLine);

		//	Keep track of the designation id so we can maintain the appropriate
		//	sort order if we have to add our own lines
		lSort = pLine->m_lDesignationOrder;
	}

	//	Add the current line to the list
	m_Lines.Add(m_pLine);

	//	Add the lines that appear after the current line
	m_Available.SetPos(m_pLine);
	lSort = m_pLine->m_lDesignationOrder;
	for(i = 0; i < iAfter; i++)
	{
		//	Do we have a line available?
		if((pLine = m_Available.Next()) == 0)
		{
			//	No line is available so we have to allocate a blank line
			pLine = new CTextLine();
			pLine->m_lDesignationId = TMTEXT_ALLOCID;

			//	Keep the color of the active line
			pLine->m_crColor = m_pLine->m_crColor;

			//	This makes sure we maintain the appropriate sort order
			pLine->m_lDesignationOrder = lSort + 1;
		}

		//	Add to our list of display lines
		ASSERT(pLine);
		m_Lines.Add(pLine);

		//	Keep track of the designation id so we can maintain the appropriate
		//	sort order if we have to add our own lines
		lSort = pLine->m_lDesignationOrder;
	}

	//	Save a reference to the next line
	m_Available.SetPos(m_pLine);
	m_pNext = m_Available.Next();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::Next()
//
// 	Description:	This function will advance to the next line of available
//					text
//
// 	Returns:		TMTEXT_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMTextCtrl::Next(short bRedraw) 
{
	//	We must have a valid playlist and line
	if(!m_pPlaylist)
	{
		m_Errors.Handle(0, IDS_TMTEXT_NOPLAYLIST);
		return TMTEXT_NOPLAYLIST;
	}

	//	We must have a valid line
	if(!m_pLine)
		return TMTEXT_NOERROR;

	//	Do we have another line?
	if(m_pNext)
	{
		//	Are we smooth scrolling?
		if(m_bSmoothScroll)
		{
			ScrollNext();
		}
		else
		{
			m_pLine = m_pNext;
			LoadLines();

			if(bRedraw)
				RedrawWindow();
		}
	}

	return TMTEXT_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnAutoInitChanged()
//
// 	Description:	This function is called when the AutoInit property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnAutoInitChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnBottomMarginChanged()
//
// 	Description:	This function is called when the BottomMargin property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnBottomMarginChanged() 
{
	//	Should we resize the font?
	if(AmbientUserMode() && m_bResizeOnChange)
		ResizeFont(TRUE);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnCombineDesignationsChanged()
//
// 	Description:	This function is called when the Combine  
//					property changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnCombineDesignationsChanged() 
{
	if(AmbientUserMode())
	{
		//	Reset the list of available text
		LoadAvailable();

		//	Reset the display text if we have a current line
		if(m_pLine)
			LoadLines();

		//	Redraw the control
		RedrawWindow();
	}

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnCreate()
//
// 	Description:	This fuction handles all WM_CREATE messages
//
// 	Returns:		0 if successful
//
//	Notes:			None
//
//==============================================================================
int CTMTextCtrl::OnCreate(LPCREATESTRUCT lpCreateStruct) 
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
// 	Function Name:	CTMTextCtrl::OnDestroy()
//
// 	Description:	This function is called when the window is destroyed
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnDestroy() 
{
	//	Make sure we kill the scroll timer
	KillScrollTimer();
    timeEndPeriod(MINIMUM_SCROLLPERIOD);      

	//	Do the base class processing
	COleControl::OnDestroy();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnBulletMarginChanged()
//
// 	Description:	This function is called when the BulletMargin property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnBulletMarginChanged() 
{
	//	Redraw the control if in user mode
	if(AmbientUserMode())
		RedrawWindow();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnBulletStyleChanged()
//
// 	Description:	This function is called when the BulletStyle property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnBulletStyleChanged() 
{
	//	Redraw the control if in user mode
	if(AmbientUserMode())
		RedrawWindow();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnDisplayLinesChanged()
//
// 	Description:	This function is called when the DisplayLines property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnDisplayLinesChanged() 
{
	if(AmbientUserMode())
	{
		FireHeightChange(GetMinHeight());

		//	Update the current line
		if(m_pLine)
			LoadLines();

		RedrawWindow();
	}

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnDraw()
//
// 	Description:	This function is called in response to WM_PAINT messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnDraw(CDC* pdc, const CRect& rcBounds,const CRect& rcInvalid)
{
	CDC			dcScratch;
	HBITMAP		hBitmap = 0;
	HBITMAP		hOldBitmap = 0;

	//	Are we in user mode?
	if(AmbientUserMode())
	{
		//	Make the dcScratch dc compatible with the dc for this window
		if(dcScratch.CreateCompatibleDC(pdc) != 0)
		{
			//	Create a compatible bitmap and select it into the scratch dc
			hBitmap = CreateCompatibleBitmap(pdc->GetSafeHdc(), 
											 rcBounds.right, 
											 rcBounds.bottom);
			hOldBitmap = (HBITMAP)dcScratch.SelectObject(hBitmap);

			//	Draw the text in the scratch dc
			if(m_bScrolling)
				DrawLines(&dcScratch, ((float)m_sStepCount * m_fStepSize));
			else
				DrawLines(&dcScratch);

			//	Blt the dcScratch bitmap
			pdc->BitBlt(0, 0, rcBounds.right, rcBounds.bottom, &dcScratch, 
				        0, 0, SRCCOPY);

			//	Cleanup
			dcScratch.SelectObject(hOldBitmap);
			DeleteObject(hBitmap);
		}
		else
		{
			//	Draw the text into the window dc
			DrawLines(pdc);
		}
	}
	else
	{
		CRect	ControlRect = rcBounds;
		CString strText;
		CBrush	brBackground;

		//	Create a brush using the background color
		brBackground.CreateSolidBrush(TranslateColor(GetBackColor()));
		
		strText.Format("FTI Depo Text Control (rev. %d.%d)",
					   _wVerMajor, _wVerMinor);

		//	Paint the background
		pdc->FillRect(ControlRect, &brBackground);
		pdc->Draw3dRect(ControlRect, RGB(0x00,0x00,0x00), 
									 RGB(0xFF,0xFF,0xFF));

		pdc->SetBkMode(TRANSPARENT);
		pdc->SetTextColor(TranslateColor(GetForeColor()));
		pdc->DrawText(strText, ControlRect, 
					  DT_CENTER | DT_VCENTER | DT_SINGLELINE); 
	}
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnEnableErrorsChanged()
//
// 	Description:	This function is called when the EnableErrors property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnEnableErrorsChanged() 
{
	m_Errors.Enable(m_bEnableErrors);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnFontChanged()
//
// 	Description:	This function handles notifications fired when the Font
//					property changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnFontChanged() 
{
	//	Resize the font if the window is valid
	if(AmbientUserMode() && IsWindow(m_hWnd))
	{
		if(m_bResizeOnChange)
			ResizeFont(TRUE);
	}
	else
	{
		COleControl::OnFontChanged();
	}
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnHighlightColorChanged()
//
// 	Description:	This function is called when the HighlightColor property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnHighlightColorChanged() 
{
	//	Redraw the control if in user mode
	if(AmbientUserMode())
		RedrawWindow();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnHighlightLinesChanged()
//
// 	Description:	This function is called when the HighlightLines property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnHighlightLinesChanged() 
{
	if(AmbientUserMode())
		RedrawWindow();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnHighlightTextColorChanged()
//
// 	Description:	This function is called when the HighlightTextColor property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnHighlightTextColorChanged() 
{
	//	Redraw the control if in user mode
	if(AmbientUserMode())
		RedrawWindow();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnLeftMarginChanged()
//
// 	Description:	This function is called when the LeftMargin property changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnLeftMarginChanged() 
{
	//	Should we resize the font?
	if(AmbientUserMode() && m_bResizeOnChange)
		ResizeFont(TRUE);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnMaxCharsPerLineChanged()
//
// 	Description:	This function is called when the MaxCharsPerLine property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnMaxCharsPerLineChanged() 
{
	//	Don't permit zero characters
	if(m_sMaxCharsPerLine <= 0)
		m_sMaxCharsPerLine = TMTEXT_MAXCHARSPERLINE;

	//	Should we resize the font?
	if(AmbientUserMode() && m_bResizeOnChange)
		ResizeFont(TRUE);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnResizeOnChangeChanged()
//
// 	Description:	This function is called when the ResizeOnChange property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnResizeOnChangeChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnRightMarginChanged()
//
// 	Description:	This function is called when the RightMargin property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnRightMarginChanged() 
{
	//	Should we resize the font?
	if(AmbientUserMode() && m_bResizeOnChange)
		ResizeFont(TRUE);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnScrollStepsChanged()
//
// 	Description:	This function is called when the ScrollSteps property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnScrollStepsChanged() 
{
	if(AmbientUserMode())
		CalcScrollPeriod();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnScrollTimeChanged()
//
// 	Description:	This function is called when the ScrollTime property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnScrollTimeChanged() 
{
	if(AmbientUserMode())
		CalcScrollPeriod();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnScrollTimer()
//
// 	Description:	This function is handles all multimedia timer events
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnScrollTimer() 
{
	ASSERT(m_pNext);
	if(!m_pNext) return;
	
	//	Are we already processing a timer event?
	if(m_bProcessingTimer)
		return;
	else
		m_bProcessingTimer = TRUE;

	//	Increment the step counter
	if(m_pPending)
		m_sStepCount = m_sScrollSteps;
	else
		m_sStepCount++;
	
	//	Is this the last step?
	if(m_sStepCount >= m_sScrollSteps)
	{
		//	Stop the timer
		KillScrollTimer();
		m_sStepCount = 0;
		m_bScrolling = FALSE;

		//	Make the next line the current line
		m_pLine = m_pNext;
		LoadLines();

		//	Wait as long as possible to reenable the timer processing
		m_bProcessingTimer = FALSE;
		
		//	Execute the pending line change if there is one
		if(m_pPending)
			SetLineObject((long)m_pPending, TRUE);
		else
			RedrawWindow();
	}
	else
	{
		//	Redraw the text
		RedrawWindow();

		//	We are done processing the timer
		m_bProcessingTimer = FALSE;
	}

}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnShowPageLineChanged()
//
// 	Description:	This function is called when the ShowPageLine property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnShowPageLineChanged() 
{
	//	Should we resize the font?
	if(AmbientUserMode() && m_bResizeOnChange)
		ResizeFont(TRUE);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnShowTextChanged()
//
// 	Description:	This function is called when the ShowText property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnShowTextChanged() 
{
	//	Redraw the control if in user mode
	if(AmbientUserMode())
		RedrawWindow();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnSize()
//
// 	Description:	This function will resize the rectangles used to calculate
//					the text positions when the window is resized
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnSize(UINT nType, int cx, int cy) 
{
	//	Do the base class processing
	COleControl::OnSize(nType, cx, cy);
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnSmoothScrollChanged()
//
// 	Description:	This function is called when the SmoothScroll property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnSmoothScrollChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnTopMarginChanged()
//
// 	Description:	This function is called when the TopMargin property changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnTopMarginChanged() 
{
	//	Should we resize the font?
	if(AmbientUserMode() && m_bResizeOnChange)
		ResizeFont(TRUE);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnUseAvgCharWidthChanged()
//
// 	Description:	This function is called when the UseAvgCharWidth property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnUseAvgCharWidthChanged() 
{
	//	Should we resize the font?
	if(AmbientUserMode() && m_bResizeOnChange)
		ResizeFont(TRUE);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::OnUseLineColorChanged()
//
// 	Description:	This function is called when the UseLineColor property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::OnUseLineColorChanged() 
{
	//	Redraw the control if in user mode
	if(AmbientUserMode())
		RedrawWindow();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::Previous()
//
// 	Description:	This function will advance to the previous line of available
//					text
//
// 	Returns:		TMTEXT_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMTextCtrl::Previous(short bRedraw) 
{
	CTextLine* pLine;

	//	We must have a valid playlist and line
	if(!m_pPlaylist)
	{
		m_Errors.Handle(0, IDS_TMTEXT_NOPLAYLIST);
		return TMTEXT_NOPLAYLIST;
	}

	//	We must have a valid line
	if(!m_pLine)
		return TMTEXT_NOERROR;

	//	Get the next line
	m_Available.SetPos(m_pLine);
	if((pLine = m_Available.Prev()) != 0)
	{
		m_pLine = pLine;
		LoadLines();
	}

	//	Do we need to redraw the control
	if(bRedraw)
		RedrawWindow();

	return TMTEXT_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::RecalcLayout()
//
// 	Description:	Called to calculate the position of the text and other
//					features within the client rectangle
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::RecalcLayout() 
{
	RECT rcClient;

	GetClientRect(&rcClient);	

	//	Are bullets turned off?
	if(m_sBulletStyle == TMTEXT_BULLET_NONE)
	{
		memset(&m_rcBulletBounds, 0, sizeof(m_rcBulletBounds));

		m_rcTextBounds.left = m_sLeftMargin;
		m_rcTextBounds.right = m_iMaxWidth - m_sRightMargin;
		m_rcTextBounds.top = rcClient.top + m_sTopMargin;
		m_rcTextBounds.bottom = rcClient.bottom - m_sBottomMargin;
	}
	else
	{
		//	Use the character height as the bullet column width so that
		//	we wind up with square bullet rectangles for each line
		m_rcBulletBounds.left   = m_sLeftMargin;
		m_rcBulletBounds.right  = m_rcBulletBounds.left + m_iCharHeight;
		m_rcBulletBounds.top    = rcClient.top + m_sTopMargin;
		m_rcBulletBounds.bottom = rcClient.bottom - m_sBottomMargin;

		m_rcTextBounds.left   = m_rcBulletBounds.right + m_sLeftMargin;
		m_rcTextBounds.right  = m_iMaxWidth - m_sRightMargin;
		m_rcTextBounds.top    = rcClient.top + m_sTopMargin;
		m_rcTextBounds.bottom = rcClient.bottom - m_sBottomMargin;
	}

}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::ResizeFont()
//
// 	Description:	This function will size the text font to ensure that the
//					number of characters defined by MaxChars will be visible
//
// 	Returns:		TMTEXT_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMTextCtrl::ResizeFont(short bRedraw) 
{
	TEXTMETRIC	Metric;
	HFONT		hStock;
	CFont*		pStock;
	CFont*		pOldFont;
	CFont*		pTest = 0;
	CFont*		pPassed = 0;
	CDC*		pdc;
	int			iMaxWidth;
	float		fPixelsPerChar;
	BOOL		bChanged = FALSE;

	//	Don't bother unless we have a valid window
	if(!IsWindow(m_hWnd))
		return TMTEXT_NOERROR;

	//	Get the device context for this window
	if((pdc = GetDC()) == 0)
	{
		m_Errors.Handle(0, IDS_TMTEXT_CREATEFONTFAILED);
		return TMTEXT_CREATEFONTFAILED;
	}

	//	Calculate the maximum character width we can use
	ASSERT(m_sMaxCharsPerLine > 0);
	iMaxWidth = m_iMaxWidth - m_sLeftMargin - m_sRightMargin;
	if(m_bShowPgLn)
		fPixelsPerChar = (float)iMaxWidth / (float)(m_sMaxCharsPerLine + PAGELINE_CHARACTERS);
	else
		fPixelsPerChar = (float)iMaxWidth / (float)m_sMaxCharsPerLine;
		
	//	Get a handle to the stock font object
	if((hStock = InternalGetFont().GetFontHandle()) == 0)
	{
		m_Errors.Handle(0, IDS_TMTEXT_CREATEFONTFAILED);
		ReleaseDC(pdc);
		return TMTEXT_CREATEFONTFAILED;
	}
	
	//	Get a CFont object we can use to get the stock font properties
	if((pStock = CFont::FromHandle(hStock)) == 0)
	{
		m_Errors.Handle(0, IDS_TMTEXT_CREATEFONTFAILED);
		ReleaseDC(pdc);
		return TMTEXT_CREATEFONTFAILED;
	}

	//	Get the LOGFONT structure for the stock font
	if(!pStock->GetLogFont(&m_lfFont))
	{
		m_Errors.Handle(0, IDS_TMTEXT_CREATEFONTFAILED);
		ReleaseDC(pdc);
		return TMTEXT_CREATEFONTFAILED;
	}

	//	Select the current stock font into the device context so that we can
	//	get the current font and restore it before we finish
	pOldFont = SelectStockFont(pdc);

	//	Keep increasing the height until we find the maximum height that will
	//	allow us to show all characters
	for(int i = 5; ; i++)
	{
		//	Save the last font to have passed the test
		if(pPassed)
			delete pPassed;
		pPassed = pTest;
		
		//	Allocate a new font
		pTest = new CFont();
		ASSERT(pTest);

		//	Save the height of this font
		m_iCharHeight = m_lfFont.lfHeight;

		//	Set the font size
		m_lfFont.lfHeight = -i;

		if(!pTest->CreateFontIndirect(&m_lfFont))
			break;

		//	Select the test font into the dc and get the metrics
		pdc->SelectObject(pTest);
		pdc->GetTextMetrics(&Metric);
			
		//	Has the font gotten too large?
		if(m_bUseAvgCharWidth && (Metric.tmAveCharWidth > fPixelsPerChar))
			break;
		else if(!m_bUseAvgCharWidth && (Metric.tmMaxCharWidth > fPixelsPerChar))
			break;
	}
		
	//	Restore the dc BEFORE we delete the test font
	if(pOldFont)
		pdc->SelectObject(pOldFont);

	//	Did we find a font of the right size?
	if(pPassed)
	{
		//	Delete the existing font
		if(m_pFont)
			delete m_pFont;
		
		//	Save the new font
		m_pFont = pPassed;

		//	Delete the test font
		if(pTest)
			delete pTest;

		bChanged = TRUE;
	}
	
	//	If we don't have a small enough font then assign the closest we can get
	else if(pTest)
	{
		//	Delete the existing font
		if(m_pFont)
			delete m_pFont;

		m_pFont = pTest;

		bChanged = TRUE;
	}

	//	Get the values for the new font
	if(m_pFont)
	{
		//	Select the new font
		pdc->SelectObject(m_pFont);

		//	Get the metrics
		pdc->GetTextMetrics(&Metric);
		
		//	Update the log font information
		m_pFont->GetLogFont(&m_lfFont);

		//	Save the height of this font
		m_iCharHeight = Metric.tmHeight;

		//	Update the character width
		m_iCharWidth = (m_bUseAvgCharWidth) ? Metric.tmAveCharWidth :
											  Metric.tmMaxCharWidth;

		//	Notify the container that the height has changed
		FireHeightChange(GetMinHeight());
	}

	//	Clean up
	if(pOldFont) pdc->SelectObject(pOldFont);
	ReleaseDC(pdc);

	//	Do we need to redraw the control
	if(bRedraw)
		RedrawWindow();

	return TMTEXT_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::ScrollNext()
//
// 	Description:	This function will scroll to the next line of text
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMTextCtrl::ScrollNext()
{
	//	Reset the members used to smooth scroll
	m_bProcessingTimer = FALSE; //	Reset here because of SetPlaylist() 
	m_sStepCount = 0;
	if(m_sScrollSteps > 0)
		m_fStepSize  = (float)m_iCharHeight / (float)m_sScrollSteps;
	else
		m_fStepSize = 1;

	//	Start the scroll timer
	if(SetScrollTimer())
	{
		m_bScrolling = TRUE;
	}
	else
	{
		m_bScrolling = FALSE;

		//	Jump to the next line since we can't scroll
		if(m_pNext)
		{
			m_pLine = m_pNext;
			LoadLines();
			RedrawWindow();
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::SetLine()
//
// 	Description:	This method allows the caller to set the current line based
//					on the designation, page, and line number provided by the
//					caller
//
// 	Returns:		TMTEXT_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMTextCtrl::SetLine(long lDesignation, long lPageNum, 
						   long lLineNum, short bRedraw) 
{
	CString			strError;
	CDesignation*	pDesignation;
	CTextPage*		pPage;
	CTextLine*		pLine;
	POSITION		Pos;

	//	We must have a valid playlist
	if(!m_pPlaylist)
	{
		m_Errors.Handle(0, IDS_TMTEXT_NOPLAYLIST);
		return TMTEXT_NOPLAYLIST;
	}

	//	Get the designation requested by the caller
	if((pDesignation = GetDesignation(lDesignation)) == 0)
	{
		strError.Format("\nDesignation = %ld\nPage = %ld\nLine = %ld",
						lDesignation, lPageNum, lLineNum);
		m_Errors.Handle(0, IDS_TMTEXT_LINENOTFOUND, strError);
		return TMTEXT_LINENOTFOUND;
	}

	//	Get the page requested by the caller
	Pos = pDesignation->m_Pages.GetHeadPosition();
	while(Pos)
	{
		pPage = (CTextPage*)pDesignation->m_Pages.GetNext(Pos);
		if(pPage && (pPage->m_lPageNum == lPageNum))
			break;
	}

	//	Did we find the requested page?
	if(!pPage || pPage->m_lPageNum != lPageNum)
	{
		strError.Format("\nDesignation = %ld\nPage = %ld\nLine = %ld",
						lDesignation, lPageNum, lLineNum);
		m_Errors.Handle(0, IDS_TMTEXT_LINENOTFOUND, strError);
		return TMTEXT_LINENOTFOUND;
	}

	//	Get the line requested by the caller
	Pos = pPage->m_Lines.GetHeadPosition();
	while(Pos)
	{
		pLine = (CTextLine*)pPage->m_Lines.GetNext(Pos);
		if(pLine && (pLine->m_lLineNum == lLineNum))
			return SetLineObject((long)pLine, bRedraw);
	}

	//	If we made it this far we must not be able to find the line
	strError.Format("\nDesignation = %ld\nPage = %ld\nLine = %ld",
					lDesignation, lPageNum, lLineNum);
	m_Errors.Handle(0, IDS_TMTEXT_LINENOTFOUND, strError);
	return TMTEXT_LINENOTFOUND;
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::SetLineObject()
//
// 	Description:	This method allows the caller to set the current line to the
//					line object specified by the caller
//
// 	Returns:		TMTEXT_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMTextCtrl::SetLineObject(long lLine, short bRedraw) 
{
	CTextLine* pLine = (CTextLine*)lLine;

	//	We must have a valid playlist
	if(!m_pPlaylist)
	{
		m_Errors.Handle(0, IDS_TMTEXT_NOPLAYLIST);
		return TMTEXT_NOPLAYLIST;
	}

	//	Are we in the middle of scrolling?
	if(m_bScrolling)
	{
		//	Mark the new line as pending so that the scrolling code can handle
		//	the line change
		m_pPending = pLine;
		return TMTEXT_NOERROR;
	}

	//	Should we smooth scroll?
	if(pLine && (pLine == m_pNext) && m_bSmoothScroll && IsWindowVisible())
	{
		ScrollNext();
		return TMTEXT_NOERROR;
	}

	//	Do we need to reload the available text?
	if(!m_bCombine && pLine)
	{
		//	Has the designation changed?
		if(!m_pLine || m_pLine->m_lDesignationOrder != pLine->m_lDesignationOrder)
		{
			//	Reload the available text
			m_pLine = pLine;
			LoadAvailable();
		}
	}

	//	Set the new selection
	m_pLine = pLine;
	LoadLines();
	
	//	Does the caller want us to redraw the control?
	if(bRedraw)
		RedrawWindow();

	return TMTEXT_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::SetMaxWidth()
//
// 	Description:	This method allows the caller to set the maximum width to
//					be used to display the text
//
// 	Returns:		TMTEXT_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMTextCtrl::SetMaxWidth(short sWidth, short bRedraw) 
{
	//	Save the new width
	m_iMaxWidth = sWidth;

	//	Resize the font 
	return ResizeFont(bRedraw);
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::SetScrollTimer()
//
// 	Description:	This function will set up the timer used to implement 
//					smooth scrolling
//
// 	Returns:		TRUE if successful
//
//	Notes:			This control uses the high speed multimedia timer
//					to get better resolution.
//
//==============================================================================
BOOL CTMTextCtrl::SetScrollTimer()
{
	//	Make sure the current timer is reset
	KillScrollTimer();

	//	Set up the timer event
	m_uTimer = timeSetEvent(m_uPeriod, 0, ProcessTimer, (DWORD)this, 
							TIME_PERIODIC);

	//	Were we able to set the timer?
    if(!m_uTimer)
	{
		m_Errors.Handle(0, IDS_TMTEXT_SETTIMERFAILED);
		return FALSE;
	}
	else
	{
		return TRUE;
	}
}

//==============================================================================
//
// 	Function Name:	CTMTextCtrl::SetPlaylist()
//
// 	Description:	This method allows the caller to set the active playlist
//
// 	Returns:		TMTEXT_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMTextCtrl::SetPlaylist(long lPlaylist, short bRedraw) 
{
	//	Lock the critical section so that no drawing takes place until we've
	//	reset the playlist
	m_CriticalSection.Lock();

	//	Kill any active scrolling operation
	KillScrollTimer();
	m_sStepCount = 0;
	m_bScrolling = FALSE;
	m_bProcessingTimer = TRUE;	//	This inhibits any buffered events

	//	Save the new playlist
	m_pPlaylist = (CPlaylist*)lPlaylist;

	//	Reset the available text
	LoadAvailable();

	//	Flush the existing display text
	m_pLine    = 0;
	m_pNext    = 0;
	m_pPending = 0;
	LoadLines();

	//	Ok to draw now
	m_CriticalSection.Unlock();

	//	Redraw the control if requested
	if(bRedraw)
		RedrawWindow();
	
	return TMTEXT_NOERROR;
}

