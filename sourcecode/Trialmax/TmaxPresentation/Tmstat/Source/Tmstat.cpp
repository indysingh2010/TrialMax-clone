//==============================================================================
//
// File Name:	tmstat.cpp
//
// Description:	This file contains member functions of the CTMStatCtrl class.
//
// See Also:	tmstat.h
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
#include <tmstatap.h>
#include <tmstat.h>
#include <tmstatpg.h>
#include <tmstdefs.h>
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
extern CTMStatApp NEAR theApp;

/* Replace 2 */
const IID BASED_CODE IID_DTMStat6 =
		{ 0xb6554c6c, 0xd285, 0x42ed, { 0x84, 0x33, 0xb0, 0x98, 0xec, 0x38, 0x8, 0x4b } };
/* Replace 3 */
const IID BASED_CODE IID_DTMStat6Events =
		{ 0xcd88da40, 0x2eab, 0x44e3, { 0xbb, 0x80, 0x2d, 0x4d, 0x22, 0x3a, 0x1, 0xde } };

// Control type information
static const DWORD BASED_CODE _dwTMStatOleMisc =
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
BEGIN_MESSAGE_MAP(CTMStatCtrl, COleControl)
	//{{AFX_MSG_MAP(CTMStatCtrl)
	ON_WM_CREATE()
	//}}AFX_MSG_MAP
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
END_MESSAGE_MAP()

// Dispatch map
BEGIN_DISPATCH_MAP(CTMStatCtrl, COleControl)
	//{{AFX_DISPATCH_MAP(CTMStatCtrl)
	DISP_PROPERTY_NOTIFY(CTMStatCtrl, "AutoInit", m_bAutoInit, OnAutoInitChanged, VT_BOOL)
	DISP_PROPERTY_EX(CTMStatCtrl, "VerBuild", GetVerBuild, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMStatCtrl, "VerMajor", GetVerMajor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMStatCtrl, "VerMinor", GetVerMinor, SetNotSupported, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMStatCtrl, "EnableErrors", m_bEnableErrors, OnEnableErrorsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMStatCtrl, "AutosizeFont", m_bAutosizeFont, OnAutosizeFontChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMStatCtrl, "StatusText", m_strStatusText, OnStatusTextChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMStatCtrl, "LeftMargin", m_sLeftMargin, OnLeftMarginChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMStatCtrl, "RightMargin", m_sRightMargin, OnRightMarginChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMStatCtrl, "TopMargin", m_sTopMargin, OnTopMarginChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMStatCtrl, "BottomMargin", m_sBottomMargin, OnBottomMarginChanged, VT_I2)
	DISP_PROPERTY_EX(CTMStatCtrl, "VerTextLong", GetVerTextLong, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMStatCtrl, "PlaylistTime", m_dPlaylistTime, OnPlaylistTimeChanged, VT_R8)
	DISP_PROPERTY_NOTIFY(CTMStatCtrl, "ElapsedPlaylist", m_dElapsedPlaylist, OnElapsedPlaylistChanged, VT_R8)
	DISP_PROPERTY_NOTIFY(CTMStatCtrl, "DesignationTime", m_dDesignationTime, OnDesignationTimeChanged, VT_R8)
	DISP_PROPERTY_NOTIFY(CTMStatCtrl, "ElapsedDesignation", m_dElapsedDesignation, OnElapsedDesignationChanged, VT_R8)
	DISP_PROPERTY_NOTIFY(CTMStatCtrl, "Mode", m_sMode, OnModeChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMStatCtrl, "DesignationCount", m_lDesignationCount, OnDesignationCountChanged, VT_I4)
	DISP_PROPERTY_NOTIFY(CTMStatCtrl, "DesignationIndex", m_lDesignationIndex, OnDesignationIndexChanged, VT_I4)
	DISP_PROPERTY_NOTIFY(CTMStatCtrl, "TextLine", m_lTextLine, OnTextLineChanged, VT_I4)
	DISP_PROPERTY_NOTIFY(CTMStatCtrl, "TextPage", m_lTextPage, OnTextPageChanged, VT_I4)
	DISP_FUNCTION(CTMStatCtrl, "Initialize", Initialize, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMStatCtrl, "SetPlaylistInfo", SetPlaylistInfo, VT_I2, VTS_I4)
	DISP_FUNCTION(CTMStatCtrl, "GetClassIdString", GetClassIdString, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMStatCtrl, "GetRegisteredPath", GetRegisteredPath, VT_BSTR, VTS_NONE)
	DISP_STOCKPROP_BACKCOLOR()
	DISP_STOCKPROP_FONT()
	DISP_STOCKPROP_FORECOLOR()
	DISP_STOCKPROP_APPEARANCE()
	//}}AFX_DISPATCH_MAP
	
	//	Added rev 6.1.0
	DISP_PROPERTY_EX_ID(CTMStatCtrl, "VerTextShort", DISPID_VERTEXTSHORT, GetVerTextShort, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_EX_ID(CTMStatCtrl, "VerQEF", DISPID_VERQEF, GetVerQEF, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX_ID(CTMStatCtrl, "VerBuildDate", DISPID_VERBUILDDATE, GetVerBuildDate, SetNotSupported, VT_BSTR)

	DISP_FUNCTION_ID(CTMStatCtrl, "AboutBox", DISPID_ABOUTBOX, AboutBox, VT_EMPTY, VTS_NONE)

	DISP_FUNCTION_ID(CTMStatCtrl, "GetStatusBarWidth", dispidGetStatusBarWidth, GetStatusBarWidth, VT_I4, VTS_NONE)
	DISP_FUNCTION_ID(CTMStatCtrl, "SetStatusBarcode", dispidSetStatusBarcode, SetStatusBarcode, VT_EMPTY, VTS_PBSTR)
END_DISPATCH_MAP()

// Event map
BEGIN_EVENT_MAP(CTMStatCtrl, COleControl)
	//{{AFX_EVENT_MAP(CTMStatCtrl)
	EVENT_STOCK_CLICK()
	EVENT_STOCK_DBLCLICK()
	//}}AFX_EVENT_MAP
END_EVENT_MAP()

// Property pages
BEGIN_PROPPAGEIDS(CTMStatCtrl, 2)
	//PROPPAGEID(CTMStatProperties::guid)
	PROPPAGEID(CLSID_CColorPropPage)
	PROPPAGEID(CLSID_CFontPropPage)
END_PROPPAGEIDS(CTMStatCtrl)

/* Replace 4 */
IMPLEMENT_OLECREATE_EX(CTMStatCtrl, "TMSTAT6.TMStatCtrl.1",
	0xc69f0d1, 0x9bb0, 0x4db0, 0xa6, 0, 0xd9, 0x86, 0x21, 0xe8, 0xd8, 0xb3)

// Type library ID and version
IMPLEMENT_OLETYPELIB(CTMStatCtrl, _tlid, _wVerMajor, _wVerMinor)
IMPLEMENT_OLECTLTYPE(CTMStatCtrl, IDS_TMSTAT, _dwTMStatOleMisc)

IMPLEMENT_DYNCREATE(CTMStatCtrl, COleControl)

// Interface map for IObjectSafety
BEGIN_INTERFACE_MAP(CTMStatCtrl, COleControl )
	INTERFACE_PART(CTMStatCtrl, IID_IObjectSafety, ObjSafety)
END_INTERFACE_MAP()

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::CTMStatCtrlFactory::UpdateRegistry
//
// 	Description:	Adds or removes system registry entries for this control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTMStatCtrl::CTMStatCtrlFactory::UpdateRegistry(BOOL bRegister)
{
	BOOL	bReturn;
	HRESULT	hResult;

	if(bRegister)
	{
		bReturn = AfxOleRegisterControlClass(AfxGetInstanceHandle(),
											 m_clsid,
											 m_lpszProgID,
											 IDS_TMSTAT,
											 IDB_TMSTAT,
											 afxRegApartmentThreading,
											 _dwTMStatOleMisc,
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
// 	Function Name:	CTMStatCtrl::XObjSafety::AddRef()
//
// 	Description:	This function is called to attach to the nested
//					IObjectSafety dispatch interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMStatCtrl::XObjSafety::AddRef()
{
    METHOD_PROLOGUE(CTMStatCtrl, ObjSafety)
    return pThis->ExternalAddRef();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::XObjSafety::GetInterfaceSafetyOptions()
//
// 	Description:	This function is called to get this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMStatCtrl::XObjSafety::GetInterfaceSafetyOptions( 
		/* [in]  */ REFIID riid,
        /* [out] */ DWORD __RPC_FAR *pdwSupportedOptions,
        /* [out] */ DWORD __RPC_FAR *pdwEnabledOptions)
{
	HRESULT			hReturn;
	IUnknown FAR*	pInterface;

	METHOD_PROLOGUE(CTMStatCtrl, ObjSafety)

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
// 	Function Name:	CTMStatCtrl::XObjSafety::QueryInterface()
//
// 	Description:	This function is called to query the IObjectSafety interface
//					for the requested method/property.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT FAR EXPORT CTMStatCtrl::XObjSafety::QueryInterface(REFIID iid, 
														  void FAR* FAR* ppvObj)
{
    METHOD_PROLOGUE(CTMStatCtrl, ObjSafety)
    return (HRESULT)pThis->ExternalQueryInterface(&iid, ppvObj);
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::XObjSafety::Release()
//
// 	Description:	This function is called to detach from the IObjectSafety
//					dispatch interface.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMStatCtrl::XObjSafety::Release()
{
    METHOD_PROLOGUE(CTMStatCtrl, ObjSafety)
    return pThis->ExternalRelease();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::XObjSafety::SetInterfaceSafetyOptions()
//
// 	Description:	This function is called to set this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMStatCtrl::XObjSafety::SetInterfaceSafetyOptions( 
        /* [in] */ REFIID riid,
        /* [in] */ DWORD dwOptionSetMask,
        /* [in] */ DWORD dwEnabledOptions)
{
	IUnknown FAR* pInterface;

    METHOD_PROLOGUE(CTMStatCtrl, ObjSafety)
	
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
// 	Function Name:	CTMStatCtrl::AboutBox()
//
// 	Description:	This method will display the control's about box
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::AboutBox()
{
	CDialog dlgAbout(IDD_ABOUTBOX_TMSTAT, this);
	dlgAbout.DoModal();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::CheckVersion()
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
BOOL CTMStatCtrl::CheckVersion(DWORD dwVersion)
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
	              "TMStat ActiveX control. You should upgrade tm_stat6.ocx "
				  "as soon as possible\n\n%s", 
				  (wMinor > _wVerMinor) ? "a newer" : "an older", strVersion);

	MessageBeep(MB_ICONEXCLAMATION);
	MessageBox(strMsg, "TrialMax Error", MB_ICONEXCLAMATION | MB_OK);

	//	The versions do not match
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::ConvertDesignationTimes()
//
// 	Description:	This function is called to convert the Designation time values
//					to their string equivalents
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::ConvertDesignationTimes() 
{
	long	lTotal = (long)(m_dDesignationTime + 0.5);
	long	lElapsed = (long)(m_dElapsedDesignation + 0.5);
	long	lRemaining = lTotal - lElapsed;
   
	//	Get the string equivalents
	TimeToStr(lTotal, m_strDesignationTime);
	TimeToStr(lElapsed, m_strElapsedDesignation);
	TimeToStr(lRemaining, m_strRemainingDesignation);
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::ConvertPlaylistTimes()
//
// 	Description:	This function is called to convert the playlist time values
//					to their string equivalents
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::ConvertPlaylistTimes() 
{
	long	lTotal = (long)(m_dPlaylistTime + 0.5);
	long	lElapsed = (long)(m_dElapsedPlaylist + 0.5);
	long	lRemaining = lTotal - lElapsed;
   
	//	Get the string equivalents
	TimeToStr(lTotal, m_strPlaylistTime);
	TimeToStr(lElapsed, m_strElapsedPlaylist);
	TimeToStr(lRemaining, m_strRemainingPlaylist);
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::CTMStatCtrl()
//
// 	Description:	This is the constructor for CTMStatCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMStatCtrl::CTMStatCtrl()
{
	InitializeIIDs(&IID_DTMStat6, &IID_DTMStat6Events);

	//	Initialize the local data
	m_pFont = 0;
	m_bShowPageLine = TRUE;
	m_bShowPlaylist = TRUE;
	m_bShowLink = FALSE;
	m_strPlaylistId = "";
	m_strLinkId = "";
	m_strText = "";
	m_strPlaylistTime = "";
	m_strElapsedPlaylist = "";
	m_strRemainingPlaylist = "";
	m_strDesignationTime = "";
	m_strElapsedDesignation = "";
	m_strRemainingDesignation = "";
	ZeroMemory(&m_rcText, sizeof(m_rcText));
	ZeroMemory(&m_rcClient, sizeof(m_rcClient));

	//	Get the registry information
	GetRegistration();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::~CTMStatCtrl()
//
// 	Description:	This is the destructor for CTMStatCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMStatCtrl::~CTMStatCtrl()
{
	if(m_pFont)
		delete m_pFont;
}		

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::DoPropertyExchange()
//
// 	Description:	This function manages the exchange of persistant 
//					properties
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::DoPropExchange(CPropExchange* pPX)
{
	BOOL bAutoInit = FALSE;
	BOOL bEnableErrors = FALSE;
	BOOL bAutosize = FALSE;
	BOOL bStatusText = FALSE;
	BOOL bTopMargin = FALSE;
	BOOL bLeftMargin = FALSE;
	BOOL bBottomMargin = FALSE;
	BOOL bRightMargin = FALSE;
	BOOL bMode = FALSE;
	BOOL bPlaylistTime = FALSE;
	BOOL bDesignationTime = FALSE;
	BOOL bDesignationCount = FALSE;
	BOOL bDesignationIndex = FALSE;
	BOOL bElapsedPlaylist = FALSE;
	BOOL bElapsedDesignation = FALSE;
	BOOL bTextPage = FALSE;
	BOOL bTextLine = FALSE;

	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
	COleControl::DoPropExchange(pPX);

	//CheckVersion(pPX->GetVersion());

	//	Load the control's persistent properties
	try
	{
		bAutoInit = PX_Bool(pPX, _T("AutoInit"), m_bAutoInit, TMSTAT_AUTOINIT);
		bEnableErrors = PX_Bool(pPX, _T("EnableErrors"), m_bEnableErrors, TRUE);
		bAutosize = PX_Bool(pPX, _T("AutosizeFont"), m_bAutosizeFont, TMSTAT_AUTOSIZEFONT);
		bStatusText = PX_String(pPX, _T("StatusText"), m_strStatusText, "");
		bTopMargin = PX_Short(pPX, _T("TopMargin"), m_sTopMargin, TMSTAT_TOPMARGIN);
		bLeftMargin = PX_Short(pPX, _T("LeftMargin"), m_sLeftMargin, TMSTAT_LEFTMARGIN);
		bBottomMargin = PX_Short(pPX, _T("BottomMargin"), m_sBottomMargin, TMSTAT_BOTTOMMARGIN);
		bRightMargin = PX_Short(pPX, _T("RightMargin"), m_sRightMargin, TMSTAT_RIGHTMARGIN);
		bMode = PX_Short(pPX, _T("Mode"), m_sMode, TMSTAT_MODE);
		bPlaylistTime = PX_Double(pPX, _T("PlaylistTime"), m_dPlaylistTime, TMSTAT_PLAYLISTTIME);
		bDesignationTime = PX_Double(pPX, _T("DesignationTime"), m_dDesignationTime, TMSTAT_DESIGNATIONTIME);
		bDesignationCount = PX_Long(pPX, _T("DesignationCount"), m_lDesignationCount, TMSTAT_DESIGNATIONCOUNT);
		bDesignationIndex = PX_Long(pPX, _T("DesignationIndex"), m_lDesignationIndex, TMSTAT_DESIGNATIONINDEX);
		bElapsedPlaylist = PX_Double(pPX, _T("ElapsedPlaylist"), m_dElapsedPlaylist, TMSTAT_ELAPSEDPLAYLIST);
		bElapsedDesignation = PX_Double(pPX, _T("ElapsedDesignation"), m_dElapsedDesignation, TMSTAT_ELAPSEDDESIGNATION);
		bTextPage = PX_Long(pPX, _T("TextPage"), m_lTextPage, TMSTAT_TEXTPAGE);
		bTextLine = PX_Long(pPX, _T("TextLine"), m_lTextLine, TMSTAT_TEXTLINE);
	}
	catch(...)
	{
		if(!bAutoInit) m_bAutoInit = TMSTAT_AUTOINIT;
		if(!bEnableErrors) m_bEnableErrors = TRUE;
		if(!bAutosize) m_bAutosizeFont = TMSTAT_AUTOSIZEFONT;
		if(!bStatusText) m_strStatusText = "";
		if(!bTopMargin) m_sTopMargin = TMSTAT_TOPMARGIN;
		if(!bLeftMargin) m_sLeftMargin = TMSTAT_LEFTMARGIN;
		if(!bBottomMargin) m_sBottomMargin = TMSTAT_BOTTOMMARGIN;
		if(!bRightMargin) m_sRightMargin = TMSTAT_RIGHTMARGIN;
		if(!bMode) m_sMode = TMSTAT_MODE;
		if(!bPlaylistTime) m_dPlaylistTime = TMSTAT_PLAYLISTTIME;
		if(!bDesignationTime) m_dDesignationTime = TMSTAT_DESIGNATIONTIME;
		if(!bDesignationCount) m_lDesignationCount = TMSTAT_DESIGNATIONCOUNT;
		if(!bDesignationIndex) m_lDesignationIndex = TMSTAT_DESIGNATIONINDEX;
		if(!bElapsedPlaylist) m_dElapsedPlaylist = TMSTAT_ELAPSEDPLAYLIST;
		if(!bElapsedDesignation) m_dElapsedDesignation = TMSTAT_ELAPSEDDESIGNATION;
		if(!bTextPage) m_lTextPage = TMSTAT_TEXTPAGE;
		if(!bTextLine) m_lTextLine = TMSTAT_TEXTLINE;
	}
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::DrawText()
//
// 	Description:	This function will draw the caller's text into the requested
//					device context
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::DrawText(CDC* pdc, LPCSTR lpText)
{
	CBrush		brBackground;
	CFont*		pOldFont;
	COLORREF	crOldColor;
	int			iOldMode;

	//	Create a brush using the background color
	brBackground.CreateSolidBrush(TranslateColor(GetBackColor()));
		
	//	Set up the dc
	if(m_pFont)
		pOldFont = pdc->SelectObject(m_pFont);
	else
		pOldFont = SelectStockFont(pdc);
	iOldMode = pdc->SetBkMode(TRANSPARENT);
	crOldColor = pdc->SetTextColor(TranslateColor(GetForeColor()));
	
	// m_strBarcode contains either the current page barcode or the barcode that the user is typing
	
	// buffer consists of 2 portions. 
	//		1st portion displays the barcode (m_strBarcode).
	//		2nd portion displays the extra information of the media running i.e. in case of videos, runtime etc (lpText).

	char buffer[2048];
	sprintf(buffer, "%s %s", m_strBarcode, lpText);

	//	Paint the background and draw the text
	pdc->FillRect(&m_rcClient, &brBackground);
	pdc->DrawText(buffer, &m_rcText, DT_LEFT | DT_BOTTOM | DT_SINGLELINE); 
	
	//	Restore the dc
	if(pOldFont) pdc->SelectObject(pOldFont);
	pdc->SetBkMode(iOldMode);
	pdc->SetTextColor(crOldColor);
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::FormatText()
//
// 	Description:	This function will format the text for display
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::FormatText()
{
	//	What is the current mode?
	switch(m_sMode)
	{
		case TMSTAT_PLAYLISTMODE:
		
			//	Are we displaying the playlist times?
			if(m_bShowPlaylist)
			{
				ConvertPlaylistTimes();
				ConvertDesignationTimes();

				//	Are we displaying the page/line numbers?
				if(m_bShowPageLine)
				{
					m_strText.Format("Playlist T-%s E-%s R-%s    Desg %d of %d P/L %d-%d R-%s",
									 m_strPlaylistTime,
									 m_strElapsedPlaylist,
									 m_strRemainingPlaylist,
									 m_lDesignationIndex,
									 m_lDesignationCount,
									 m_lTextPage,
									 m_lTextLine,
									 m_strRemainingDesignation);
				}
				else
				{
					m_strText.Format("Playlist T-%s E-%s R-%s    Desg %d of %d R-%s",
									 m_strPlaylistTime,
									 m_strElapsedPlaylist,
									 m_strRemainingPlaylist,
									 m_lDesignationIndex,
									 m_lDesignationCount,
									 m_strRemainingDesignation);
				}

				//	Are we displaying a link?
				if((m_strLinkId.GetLength() > 0) && (m_bShowLink == TRUE))
				{
					m_strText += "     Link: ";
					m_strText += m_strLinkId;
				}

			}
			else
			{
				ConvertDesignationTimes();

				//	Are we displaying the page/line numbers?
				if(m_bShowPageLine)
				{
					m_strText.Format("%d of %d  DT: %s DE: %s DR: %s  PGLN: %d:%d",
									 m_lDesignationIndex,
									 m_lDesignationCount,
									 m_strDesignationTime,
									 m_strElapsedDesignation,
									 m_strRemainingDesignation,
									 m_lTextPage,
									 m_lTextLine);
				}
				else
				{
					m_strText.Format("%d of %d  DT: %s DE: %s DR: %s",
									 m_lDesignationIndex,
									 m_lDesignationCount,
									 m_strDesignationTime,
									 m_strElapsedDesignation,
									 m_strRemainingDesignation);
				}
			}

			break;

		case TMSTAT_TEXTMODE:

			m_strText = m_strStatusText;
			break;
	}
}	

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::GetClassIdString()
//
// 	Description:	This method is called to get the class GUID as a null
//					terminated string.
//
// 	Returns:		The string equivalent of the CLASS GUID
//
//	Notes:			None
//
//==============================================================================
BSTR CTMStatCtrl::GetClassIdString() 
{
	CString strClsId = m_tmVersion.GetClsId();
	return strClsId.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::GetRegisteredPath()
//
// 	Description:	This method is called to get the ocx path stored in the
//					system registry.
//
// 	Returns:		The path to the ocx stored in the registry
//
//	Notes:			None
//
//==============================================================================
BSTR CTMStatCtrl::GetRegisteredPath() 
{
	CString strRegistered = m_tmVersion.GetFileSpec();
	return strRegistered.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::GetRegistration()
//
// 	Description:	This function is called to get the control's registration
//					information.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::GetRegistration() 
{
	CLSID	clsid;

	//	Get the GUID 
	GetClassID(&clsid);

	//	Initialize the version information
	m_tmVersion.InitFromClsId("TMStat", "Status Bar Control", clsid);


}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::GetVerBuild()
//
// 	Description:	This method is called to get the value of the VerBuild
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMStatCtrl::GetVerBuild() 
{
	return m_tmVersion.GetBuild();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::GetVerBuildDate()
//
// 	Description:	This method is called to get the value of the VerBuildDate
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMStatCtrl::GetVerBuildDate() 
{
	CString strBuildDate = m_tmVersion.GetBuildDate();
	return strBuildDate.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::GetVerMajor()
//
// 	Description:	This method is called to get the value of the VerMajor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMStatCtrl::GetVerMajor() 
{
	return m_tmVersion.GetMajor();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::GetVerMinor()
//
// 	Description:	This method is called to get the value of the VerMinor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMStatCtrl::GetVerMinor() 
{
	return m_tmVersion.GetMinor();
}


//==============================================================================
//
// 	Function Name:	CTMStatCtrl::GetVerQEF()
//
// 	Description:	This method is called to get the value of the VerQEF
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMStatCtrl::GetVerQEF() 
{
	return m_tmVersion.GetUpdate();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::GetVerTextLong()
//
// 	Description:	This method is called to get the value of the VerTextLong
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMStatCtrl::GetVerTextLong() 
{
	CString strVer = m_tmVersion.GetTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::GetVerTextShort()
//
// 	Description:	This method is called to get the value of the VerTextShort
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMStatCtrl::GetVerTextShort() 
{
	CString strVer = m_tmVersion.GetShortTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::Initialize()
//
// 	Description:	This function will construct and initialize the depobar
//
// 	Returns:		TMTX_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMStatCtrl::Initialize()
{
	//	Don't bother if not in user mode
	if(!AmbientUserMode())
		return TMSTAT_NOERROR;

	//	Set up the correct text rectangle
	Recalc(FALSE);

	return TMSTAT_NOERROR;
}	

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::OnAutoInitChanged()
//
// 	Description:	This function is called when the AutoInit property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::OnAutoInitChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::OnAutosizeFontChanged()
//
// 	Description:	This function is called when the window is resized
//
// 	Returns:		Nonzero if ok to resize control
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::OnAutosizeFontChanged() 
{
	CDC*		pdc;
	LOGFONT		lfFont;
	HFONT		hStock;
	CFont*		pStock;

	SetModifiedFlag();

	//	Don't bother doing anything if in design mode
	if(!AmbientUserMode() || !IsWindow(m_hWnd))
		return;

	//	Delete the existing font
	if(m_pFont)
	{
		delete m_pFont;
		m_pFont = 0;
	}

	//	Stop here if not autosizing the font?
	if(!m_bAutosizeFont)
		return;

	//	Get the device context for this window
	if((pdc = GetDC()) == 0)
		return;

	//	Get a handle to the stock font object
	if((hStock = InternalGetFont().GetFontHandle()) == 0)
	{
		ReleaseDC(pdc);
		return;
	}
	
	//	Get a CFont object we can use to get the stock font properties
	if((pStock = CFont::FromHandle(hStock)) == 0)
	{
		ReleaseDC(pdc);
		return;
	}

	//	Get the LOGFONT structure for the stock font
	if(!pStock->GetLogFont(&lfFont))
	{
		ReleaseDC(pdc);
		return;
	}

	//	Allocate a new font
	m_pFont = new CFont();
	ASSERT(m_pFont);

	//	Adjust the height to suit the window
	lfFont.lfHeight = m_rcText.bottom - m_rcText.top;

	//	Create the font
	if(!m_pFont->CreateFontIndirect(&lfFont))
	{
		delete m_pFont;
		m_pFont = 0;
	}

	ReleaseDC(pdc);
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::OnBottomMarginChanged()
//
// 	Description:	This function is called when the BottomMargin property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::OnBottomMarginChanged() 
{
	if(AmbientUserMode())
		Recalc(TRUE);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::OnCreate()
//
// 	Description:	This fuction handles all WM_CREATE messages
//
// 	Returns:		0 if successful
//
//	Notes:			None
//
//==============================================================================
int CTMStatCtrl::OnCreate(LPCREATESTRUCT lpCreateStruct) 
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
// 	Function Name:	CTMStatCtrl::OnDesignationCountChanged()
//
// 	Description:	This function is called when the DesignationCount property
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::OnDesignationCountChanged() 
{
	if(AmbientUserMode())
		InvalidateControl();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::OnDesignationIndexChanged()
//
// 	Description:	This function is called when the DesignationIndex property
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::OnDesignationIndexChanged() 
{
	if(AmbientUserMode())
		InvalidateControl();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::OnDesignationTimeChanged()
//
// 	Description:	This function is called when the DesignationTime property
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::OnDesignationTimeChanged() 
{
	if(AmbientUserMode())
		InvalidateControl();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::OnDraw()
//
// 	Description:	This function is called in response to WM_PAINT messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::OnDraw(CDC* pdc, const CRect& rcBounds,const CRect& rcInvalid)
{
	CDC			dcScratch;
	HBITMAP		hBitmap = 0;
	HBITMAP		hOldBitmap = 0;

	//	Are we in user mode?
	if(AmbientUserMode())
	{
		//	Get the text for display
		FormatText();

		//	Make the dcScratch dc compatible with the dc for this window
		if(dcScratch.CreateCompatibleDC(pdc) != 0)
		{
			//	Create a compatible bitmap and select it into the scratch dc
			hBitmap = CreateCompatibleBitmap(pdc->GetSafeHdc(), 
											 m_rcClient.right, 
											 m_rcClient.bottom);
			hOldBitmap = (HBITMAP)dcScratch.SelectObject(hBitmap);

			//	Draw the text in the scratch dc
			DrawText(&dcScratch, m_strText);

			//	Blt the dcScratch bitmap
			pdc->BitBlt(0, 0, m_rcClient.right, m_rcClient.bottom, &dcScratch, 
				        0, 0, SRCCOPY);

			//	Cleanup
			dcScratch.SelectObject(hOldBitmap);
			DeleteObject(hBitmap);
		}
		else
		{
			//	Draw the text into the window dc
			DrawText(pdc, m_strText);
		}
	}
	else
	{
		//	Format the design mode text
		m_strText.Format("FTI Status Bar Control (rev. %d.%d)",
					     _wVerMajor, _wVerMinor);

		//	Draw the design mode text
		DrawText(pdc, m_strText);
	}
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::OnElapsedDesignationChanged()
//
// 	Description:	This function is called when the ElapsedDesignation property
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::OnElapsedDesignationChanged() 
{
	if(AmbientUserMode())
		RedrawWindow();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::OnElapsedPlaylistChanged()
//
// 	Description:	This function is called when the ElapsedPlaylist property
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::OnElapsedPlaylistChanged() 
{
	if(AmbientUserMode())
		InvalidateControl();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::OnEnableErrorsChanged()
//
// 	Description:	This function is called when the EnableErrors property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::OnEnableErrorsChanged() 
{
	m_Errors.Enable(m_bEnableErrors);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::OnLeftMarginChanged()
//
// 	Description:	This function is called when the LeftMargin property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::OnLeftMarginChanged() 
{
	if(AmbientUserMode())
		Recalc(TRUE);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::OnModeChanged()
//
// 	Description:	This function is called when the Mode property changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::OnModeChanged() 
{
	if(AmbientUserMode())
		InvalidateControl();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::OnPlaylistTimeChanged()
//
// 	Description:	This function is called when the PlaylistTime property
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::OnPlaylistTimeChanged() 
{
	if(AmbientUserMode())
		InvalidateControl();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::OnRightMarginChanged()
//
// 	Description:	This function is called when the RightMargin property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::OnRightMarginChanged() 
{
	if(AmbientUserMode())
		Recalc(TRUE);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::OnSetExtent()
//
// 	Description:	This function is called when the window is resized
//
// 	Returns:		Nonzero if ok to resize control
//
//	Notes:			None
//
//==============================================================================
BOOL CTMStatCtrl::OnSetExtent(LPSIZEL lpSizeL) 
{
	CSize Size;

	//	Get a CDC object so we can call HIMETRICtoDP()
	CWnd* pWindow = CWnd::FromHandle(::GetDesktopWindow());
	CClientDC dc(pWindow);

	//	Convert to pixels
	Size.cx = lpSizeL->cx;
	Size.cy = lpSizeL->cy;
	dc.HIMETRICtoDP(&Size);

	//	Save the client area rectangle
	m_rcClient.left   = 0;
	m_rcClient.right  = Size.cx;
	m_rcClient.top    = 0;
	m_rcClient.bottom = Size.cy;

	//	Now calculate the text rectangle
	Recalc(FALSE);

	return COleControl::OnSetExtent(lpSizeL);
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::OnStatusTextChanged()
//
// 	Description:	This function is called when the StatusText property changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::OnStatusTextChanged() 
{
	if(AmbientUserMode())
		InvalidateControl();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::OnTextLineChanged()
//
// 	Description:	This function is called when the TextLine property
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::OnTextLineChanged() 
{
	if(AmbientUserMode())
		InvalidateControl();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::OnTextPageChanged()
//
// 	Description:	This function is called when the TextPage property
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::OnTextPageChanged() 
{
	if(AmbientUserMode())
		InvalidateControl();
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::OnTopMarginChanged()
//
// 	Description:	This function is called when the TopMargin property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::OnTopMarginChanged() 
{
	if(AmbientUserMode())
		Recalc(TRUE);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::Recalc()
//
// 	Description:	This function will recalculate the text rectangle using the
//					current available client area
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::Recalc(BOOL bRedraw) 
{
	//	Start with the full client area
	memcpy(&m_rcText, &m_rcClient, sizeof(m_rcText));
	
	//	Allow for 3d borders
	if(GetAppearance())
		InflateRect(&m_rcText, -2, -2);

	//	Adjust for margins
	m_rcText.left += m_sLeftMargin;
	m_rcText.right -= m_sRightMargin;
	m_rcText.top += m_sTopMargin;
	m_rcText.bottom -= m_sBottomMargin;

	//	Has the window been created yet?
	if(IsWindow(m_hWnd))
	{
		//	Should we resize the font?
		if(m_bAutosizeFont)
			OnAutosizeFontChanged();

		//	Should we redraw the window?
		if(bRedraw)
			RedrawWindow();
	}
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::SetPlaylistInfo()
//
// 	Description:	This method is called to set the playlist mode information
//					using the structure defined in tmstdefs.h
//
// 	Returns:		TMSTAT_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMStatCtrl::SetPlaylistInfo(long lInfo) 
{
	SPlaylistStatus* pStatus = (SPlaylistStatus*)lInfo;

	//	Did the caller provide new information?
	if(pStatus)
	{
		//	Assign the new values
		m_dPlaylistTime = pStatus->dPlaylistTime;
		m_dElapsedPlaylist = pStatus->dElapsedPlaylist;
		m_dDesignationTime = pStatus->dDesignationTime;
		m_dElapsedDesignation = pStatus->dElapsedDesignation;
		m_lDesignationCount = pStatus->lDesignationCount;
		m_lDesignationIndex = pStatus->lDesignationOrder;
		m_lTextPage = pStatus->lTextPage;
		m_lTextLine = pStatus->lTextLine;
		m_bShowPageLine = pStatus->bShowPageLine;
		m_bShowPlaylist = pStatus->bShowPlaylist;
		m_bShowLink = pStatus->bShowLink;
		m_strPlaylistId = pStatus->szMediaId;
		m_strLinkId = pStatus->szLinkId;
	}
	else
	{
		//	Assign the defaults
		m_dPlaylistTime = 0.0;
		m_dElapsedPlaylist = 0.0;
		m_dDesignationTime = 0.0;
		m_dElapsedDesignation = 0.0;
		m_lDesignationCount = 0;
		m_lDesignationIndex = 0;
		m_lTextPage = 0;
		m_lTextLine = 0;
		m_bShowPageLine = TRUE;
		m_bShowPlaylist = TRUE;
		m_bShowLink = FALSE;
		m_strPlaylistId = "";
		m_strLinkId = "";
	}

	//	Redraw the window
	RedrawWindow();

	return TMSTAT_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::TimeToStr()
//
// 	Description:	This function is called to convert the seconds to a string
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMStatCtrl::TimeToStr(long lTime, CString& strTime) 
{
	ldiv_t	Div;
	long	lHours;
	long	lMinutes;
	long	lSeconds;
   
	//	Get the number of hours
	Div = ldiv(lTime, 3600);
	lHours = Div.quot;

	//	Get the number of minutes and seconds
	Div = ldiv(Div.rem, 60);
	lMinutes = Div.quot;
	lSeconds = Div.rem;

	//	Format the string
	strTime.Format("%.01d:%.02d:%.02d", lHours, lMinutes, lSeconds);
}

//==============================================================================
//
// 	Function Name:	CTMStatCtrl::GetStatusBarWidth()
//
// 	Description:	This function is called to calculate the length of the 
//					status bar
//
// 	Returns:		Length of the status bar
//
//	Notes:			None
//
//==============================================================================
LONG CTMStatCtrl::GetStatusBarWidth(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your dispatch handler code here
	
	CBrush		brBackground;
	CFont*		pOldFont;
	COLORREF	crOldColor;
	int			iOldMode;
	CDC*		pdc = CWnd::GetDC();
	//	Create a brush using the background color
	brBackground.CreateSolidBrush(TranslateColor(GetBackColor()));
		
	//	Set up the dc
	if(m_pFont)
		pOldFont = pdc->SelectObject(m_pFont);
	else
		pOldFont = SelectStockFont(pdc);
	iOldMode = pdc->SetBkMode(TRANSPARENT);
	crOldColor = pdc->SetTextColor(TranslateColor(GetForeColor()));

	long size;
	if (m_bShowPlaylist) // Check if current media running is video i.e. Will have more details then just barcode
		size = m_rcClient.right; // Size of the full status bar displaying the Barcode (Not cropped)
	else
		size = ((pdc->GetTextExtent(m_strBarcode)).cx) + ((pdc->GetTextExtent("M")).cx); // Size of the cropped status bar displaying the Barcode
	
	//	Restore the dc
	if(pOldFont) pdc->SelectObject(pOldFont);
	pdc->SetBkMode(iOldMode);
	pdc->SetTextColor(crOldColor);
	
	return size;
}

//==============================================================================
//
//  Function Name:	CTMStatCtrl::SetStatusBarcode()
//
//  Description:	This function is called to set the Barcode value
//
//  Returns:		None
//
//  Notes:			None
//
//==============================================================================
void CTMStatCtrl::SetStatusBarcode(BSTR *barcode)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	m_strBarcode = *barcode;
	Invalidate();
}
