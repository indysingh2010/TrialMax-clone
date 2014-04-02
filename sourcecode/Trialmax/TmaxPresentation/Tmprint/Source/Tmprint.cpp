//==============================================================================
//
// File Name:	tmprint.cpp
//
// Description:	This file contains member functions of the CTMPrintCtrl class.
//
// See Also:	tmlpen.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	12-13-99	1.00		Original Release
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmprntap.h>
#include <tmprint.h>
#include <tmprntpg.h>
#include <tmprdefs.h>
#include <template.h>
#include <winspool.h>
#include <regcats.h>
#include <dispid.h>
#include <filever.h>
#include <toolbox.h>
#include <fprintercaps.h>

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
extern CTMPrintApp NEAR theApp;

/* Replace 2 */
const IID BASED_CODE IID_DTMPrint6 =
		{ 0x2eabdfe, 0x378f, 0x4f1d, { 0x9c, 0xc9, 0xe8, 0x11, 0x5, 0x20, 0x11, 0xa3 } };
/* Replace 3 */
const IID BASED_CODE IID_DTMPrint6Events =
		{ 0x644c7420, 0xe402, 0x4a84, { 0x8d, 0x38, 0xed, 0xad, 0x79, 0x7b, 0x33, 0xea } };

// Control type information
static const DWORD BASED_CODE _dwTMPrintOleMisc =
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
BEGIN_MESSAGE_MAP(CTMPrintCtrl, COleControl)
	//{{AFX_MSG_MAP(CTMPrintCtrl)
	ON_WM_CREATE()
	ON_WM_SETFOCUS()
	//}}AFX_MSG_MAP
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
	ON_MESSAGE(WM_ERROR_EVENT, OnWMErrorEvent)
END_MESSAGE_MAP()

// Dispatch map
BEGIN_DISPATCH_MAP(CTMPrintCtrl, COleControl)
	//{{AFX_DISPATCH_MAP(CTMPrintCtrl)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "AutoInit", m_bAutoInit, OnAutoInitChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "EnableErrors", m_bEnableErrors, OnEnableErrorsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "IniFile", m_strIniFile, OnIniFileChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "IniSection", m_strIniSection, OnIniSectionChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "EnablePowerPoint", m_bEnablePowerPoint, OnEnablePowerPointChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "Collate", m_bCollate, OnCollateChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "Copies", m_sCopies, OnCopiesChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "IncludePathInFileName", m_bIncludePathInFileName, OnIncludePathInFileNameChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "IncludePageTotal", m_bIncludePageTotal, OnIncludePageTotalChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "PrintImage", m_bPrintImage, OnPrintImageChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "PrintBarcodeGraphic", m_bPrintBarcodeGraphic, OnPrintBarcodeGraphicChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "PrintBarcodeText", m_bPrintBarcodeText, OnPrintBarcodeTextChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "PrintName", m_bPrintName, OnPrintNameChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "PrintFileName", m_bPrintFileName, OnPrintFileNameChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "PrintDeponent", m_bPrintDeponent, OnPrintDeponentChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "PrintPageNumber", m_bPrintPageNumber, OnPrintPageNumberChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "PrintCellBorder", m_bPrintCellBorder, OnPrintCellBorderChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "Printer", m_strPrinter, OnPrinterChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "TemplateName", m_strTemplateName, OnTemplateNameChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "ForceNewPage", m_bForceNewPage, OnForceNewPageChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "UseSlideIDs", m_bUseSlideIDs, OnUseSlideIDsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "BarcodeCharacter", m_strBarcodeCharacter, OnBarcodeCharacterChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "ShowOptions", m_bShowOptions, OnShowOptionsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "BarcodeFont", m_strBarcodeFont, OnBarcodeFontChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "ShowStatus", m_bShowStatus, OnShowStatusChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "LeftMargin", m_fLeftMargin, OnLeftMarginChanged, VT_R4)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "TopMargin", m_fTopMargin, OnTopMarginChanged, VT_R4)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "PrintCalloutBorders", m_bPrintCalloutBorders, OnPrintCalloutBordersChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "PrintBorderColor", m_lPrintBorderColor, OnPrintBorderColorChanged, VT_COLOR)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "PrintBorderThickness", m_fPrintBorderThickness, OnPrintBorderThicknessChanged, VT_R4)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "PrintCallouts", m_bPrintCallouts, OnPrintCalloutsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "AutoRotate", m_bAutoRotate, OnAutoRotateChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMPrintCtrl, "PrintForeignBarcode", m_bPrintForeignBarcode, OnPrintForeignBarcodeChanged, VT_BOOL)
	DISP_PROPERTY_EX(CTMPrintCtrl, "VerBuild", GetVerBuild, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMPrintCtrl, "VerMajor", GetVerMajor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMPrintCtrl, "VerMinor", GetVerMinor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMPrintCtrl, "VerTextLong", GetVerTextLong, SetNotSupported, VT_BSTR)
	DISP_FUNCTION(CTMPrintCtrl, "Initialize", Initialize, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMPrintCtrl, "Add", Add, VT_I2, VTS_BSTR)
	DISP_FUNCTION(CTMPrintCtrl, "GetQueueCount", GetQueueCount, VT_I4, VTS_NONE)
	DISP_FUNCTION(CTMPrintCtrl, "Clear", Clear, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMPrintCtrl, "RefreshTemplates", RefreshTemplates, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMPrintCtrl, "Print", Print, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMPrintCtrl, "EnumerateTemplates", EnumerateTemplates, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMPrintCtrl, "EnumeratePrinters", EnumeratePrinters, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMPrintCtrl, "GetDefaultPrinter", GetDefaultPrinter, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMPrintCtrl, "SelectPrinter", SelectPrinter, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMPrintCtrl, "SetPrintTemplates", SetPrintTemplates, VT_I2, VTS_I4)
	DISP_FUNCTION(CTMPrintCtrl, "SetPrintTemplate", SetPrintTemplate, VT_I2, VTS_I4)
	DISP_FUNCTION(CTMPrintCtrl, "IsReady", IsReady, VT_BOOL, VTS_NONE)
	DISP_FUNCTION(CTMPrintCtrl, "GetRegisteredPath", GetRegisteredPath, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMPrintCtrl, "GetClassIdString", GetClassIdString, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMPrintCtrl, "GetPrintTemplates", GetPrintTemplates, VT_I4, VTS_NONE)
	DISP_FUNCTION(CTMPrintCtrl, "GetPrintTemplate", GetPrintTemplate, VT_I4, VTS_NONE)
	DISP_FUNCTION(CTMPrintCtrl, "GetRowsPerPage", GetRowsPerPage, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMPrintCtrl, "GetColumnsPerPage", GetColumnsPerPage, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMPrintCtrl, "Abort", Abort, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMPrintCtrl, "GetFieldEnabledMask", GetFieldEnabledMask, VT_I4, VTS_BSTR)
	DISP_FUNCTION(CTMPrintCtrl, "GetFieldDefaultMask", GetFieldDefaultMask, VT_I4, VTS_BSTR)
	DISP_FUNCTION(CTMPrintCtrl, "SetPrinterProperties", SetPrinterProperties, VT_BOOL, VTS_HANDLE)
	DISP_FUNCTION(CTMPrintCtrl, "ShowPrinterCaps", ShowPrinterCaps, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMPrintCtrl, "EnableDIBPrinting", EnableDIBPrinting, VT_EMPTY, VTS_I2)
	DISP_FUNCTION(CTMPrintCtrl, "EnumerateTextFields", EnumerateTextFields, VT_I2, VTS_BSTR)
	DISP_FUNCTION(CTMPrintCtrl, "SetTextFieldEnabled", SetTextFieldEnabled, VT_I2, VTS_I4 VTS_BSTR VTS_I2)
	DISP_STOCKPROP_BACKCOLOR()
	//}}AFX_DISPATCH_MAP

	//	Added rev 6.0
	DISP_PROPERTY_NOTIFY_ID(CTMPrintCtrl, "EnableAxErrors", DISPID_ENABLEAXERRORS, m_bEnableAxErrors, OnEnableAxErrorsChanged, VT_BOOL)

	//	Added rev 6.1.0
	DISP_PROPERTY_EX_ID(CTMPrintCtrl, "VerTextShort", DISPID_VERTEXTSHORT, GetVerTextShort, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_EX_ID(CTMPrintCtrl, "VerQEF", DISPID_VERQEF, GetVerQEF, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX_ID(CTMPrintCtrl, "VerBuildDate", DISPID_VERBUILDDATE, GetVerBuildDate, SetNotSupported, VT_BSTR)

	//	Added rev 6.1.6
	DISP_PROPERTY_NOTIFY_ID(CTMPrintCtrl, "JobName", DISPID_JOBNAME, m_strJobName, OnJobNameChanged, VT_BSTR)

	//	Added rev 6.2.3
	DISP_PROPERTY_NOTIFY_ID(CTMPrintCtrl, "PrintSourceBarcode", DISPID_PRINTSOURCEBARCODE, m_bPrintSourceBarcode, OnPrintSourceBarcodeChanged, VT_BOOL)

	//	Added rev 6.3.2
	DISP_PROPERTY_NOTIFY_ID(CTMPrintCtrl, "InsertSlipSheet", DISPID_INSERTSLIPSHEET, m_bInsertSlipSheet, OnInsertSlipSheetChanged, VT_BOOL)

	//	Added rev 6.3.3
	DISP_PROPERTY_NOTIFY_ID(CTMPrintCtrl, "CalloutFrameColor", DISPID_CALLOUTFRAMECOLOR, m_sCalloutFrameColor, OnCalloutFrameColorChanged, VT_I2)

END_DISPATCH_MAP()

// Event map
BEGIN_EVENT_MAP(CTMPrintCtrl, COleControl)
	//{{AFX_EVENT_MAP(CTMPrintCtrl)
	EVENT_CUSTOM("FirstTemplate", FireFirstTemplate, VTS_BSTR)
	EVENT_CUSTOM("NextTemplate", FireNextTemplate, VTS_BSTR)
	EVENT_CUSTOM("FirstPrinter", FireFirstPrinter, VTS_BSTR)
	EVENT_CUSTOM("NextPrinter", FireNextPrinter, VTS_BSTR)
	EVENT_CUSTOM("EndJob", FireEndJob, VTS_I2)
	EVENT_CUSTOM("PrintPage", FirePrintPage, VTS_I4)
	EVENT_CUSTOM("StartJob", FireStartJob, VTS_BSTR  VTS_I4  VTS_I4  VTS_I4)
	EVENT_CUSTOM("PrintImage", FirePrintImage, VTS_I4  VTS_BSTR)
	EVENT_CUSTOM("AxError", FireAxError, VTS_BSTR)
	EVENT_CUSTOM("AxDiagnostic", FireAxDiagnostic, VTS_BSTR  VTS_BSTR)
	EVENT_CUSTOM("FirstTextField", FireFirstTextField, VTS_I4  VTS_BSTR  VTS_BSTR  VTS_I2  VTS_I2)
	EVENT_CUSTOM("NextTextField", FireNextTextField, VTS_I4  VTS_BSTR  VTS_BSTR  VTS_I2  VTS_I2)
	//}}AFX_EVENT_MAP
END_EVENT_MAP()

// Property pages
BEGIN_PROPPAGEIDS(CTMPrintCtrl, 2)
	PROPPAGEID(CTMPrintProperties::guid)
	PROPPAGEID(CLSID_CColorPropPage)
END_PROPPAGEIDS(CTMPrintCtrl)

/* Replace 4 */
IMPLEMENT_OLECREATE_EX(CTMPrintCtrl, "TMPRINT6.TMPrintCtrl.1",
	0x2b6165a5, 0xc1fc, 0x463e, 0x9b, 0x56, 0x20, 0x14, 0x3b, 0xf4, 0xf6, 0x27)

// Type library ID and version
IMPLEMENT_OLETYPELIB(CTMPrintCtrl, _tlid, _wVerMajor, _wVerMinor)
IMPLEMENT_OLECTLTYPE(CTMPrintCtrl, IDS_TMPRINT, _dwTMPrintOleMisc)

IMPLEMENT_DYNCREATE(CTMPrintCtrl, COleControl)

// Interface map for IObjectSafety
BEGIN_INTERFACE_MAP(CTMPrintCtrl, COleControl )
	INTERFACE_PART(CTMPrintCtrl, IID_IObjectSafety, ObjSafety)
END_INTERFACE_MAP()

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::CTMPrintCtrlFactory::UpdateRegistry
//
// 	Description:	Adds or removes system registry entries for this control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPrintCtrl::CTMPrintCtrlFactory::UpdateRegistry(BOOL bRegister)
{
	BOOL	bReturn;
	HRESULT	hResult;

	if(bRegister)
	{
		bReturn = AfxOleRegisterControlClass(AfxGetInstanceHandle(),
											 m_clsid,
											 m_lpszProgID,
											 IDS_TMPRINT,
											 IDB_TMPRINT,
											 afxRegApartmentThreading,
											 _dwTMPrintOleMisc,
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
// 	Function Name:	CTMPrintCtrl::XObjSafety::AddRef()
//
// 	Description:	This function is called to attach to the nested
//					IObjectSafety dispatch interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMPrintCtrl::XObjSafety::AddRef()
{
    METHOD_PROLOGUE(CTMPrintCtrl, ObjSafety)
    return pThis->ExternalAddRef();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::XObjSafety::GetInterfaceSafetyOptions()
//
// 	Description:	This function is called to get this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMPrintCtrl::XObjSafety::GetInterfaceSafetyOptions( 
		/* [in]  */ REFIID riid,
        /* [out] */ DWORD __RPC_FAR *pdwSupportedOptions,
        /* [out] */ DWORD __RPC_FAR *pdwEnabledOptions)
{
	HRESULT			hReturn;
	IUnknown FAR*	pInterface;

	METHOD_PROLOGUE(CTMPrintCtrl, ObjSafety)

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
// 	Function Name:	CTMPrintCtrl::XObjSafety::QueryInterface()
//
// 	Description:	This function is called to query the IObjectSafety interface
//					for the requested method/property.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT FAR EXPORT CTMPrintCtrl::XObjSafety::QueryInterface(REFIID iid, 
														  void FAR* FAR* ppvObj)
{
    METHOD_PROLOGUE(CTMPrintCtrl, ObjSafety)
    return (HRESULT)pThis->ExternalQueryInterface(&iid, ppvObj);
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::XObjSafety::Release()
//
// 	Description:	This function is called to detach from the IObjectSafety
//					dispatch interface.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMPrintCtrl::XObjSafety::Release()
{
    METHOD_PROLOGUE(CTMPrintCtrl, ObjSafety)
    return pThis->ExternalRelease();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::XObjSafety::SetInterfaceSafetyOptions()
//
// 	Description:	This function is called to set this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMPrintCtrl::XObjSafety::SetInterfaceSafetyOptions( 
        /* [in] */ REFIID riid,
        /* [in] */ DWORD dwOptionSetMask,
        /* [in] */ DWORD dwEnabledOptions)
{
	IUnknown FAR* pInterface;

    METHOD_PROLOGUE(CTMPrintCtrl, ObjSafety)
	
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
// 	Function Name:	CTMPrintCtrl::Abort()
//
// 	Description:	This method is called to abort the print job.
//
// 	Returns:		TMPRINT_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPrintCtrl::Abort() 
{
	//	Has the control been intialized?
	if(m_pOptions != 0)
	{
		m_pOptions->Abort();
		return TMPRINT_NOERROR;
	}
	else
	{
		m_Errors.Handle(0, IDS_TMPRINT_NOTINITIALIZED);
		return TMPRINT_NOTINITIALIZED;
	}
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::Add()
//
// 	Description:	This method is called to add an entry to the print queue
//					using the formatted string specification.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
short CTMPrintCtrl::Add(LPCTSTR lpszString) 
{
	//	Has the control been intialized?
	if(m_pOptions == 0)
	{
		m_Errors.Handle(0, IDS_TMPRINT_NOTINITIALIZED);
		return TMPRINT_NOTINITIALIZED;
	}
	else
	{
		CString strString = lpszString;
		return m_pOptions->Add(strString);
	}
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::CalculateSize()
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
void CTMPrintCtrl::CalculateSize()
{
	CDialog SizeDialog;

	//	This is the size of the control window in dialog units. These values
	//	were calculated by trial and error during development.
	int iDlgWidth  = 396;
	int iDlgHeight = 244;

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
// 	Function Name:	CTMPrintCtrl::CheckVersion()
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
BOOL CTMPrintCtrl::CheckVersion(DWORD dwVersion)
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
	              "TMPrint ActiveX control. You should upgrade tm_print6.ocx "
				  "as soon as possible\n\n%s", 
				  (wMinor > _wVerMinor) ? "a newer" : "an older", strVersion);

	MessageBeep(MB_ICONEXCLAMATION);
	MessageBox(strMsg, "TrialMax Error", MB_ICONEXCLAMATION | MB_OK);

	//	The versions do not match
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::Clear()
//
// 	Description:	This method allows the caller to flush the current print
//					queue.
//
// 	Returns:		TMPRINT_NOERROR  if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPrintCtrl::Clear() 
{
	if(AmbientUserMode() && m_pOptions)
		m_pOptions->Flush();

	return TMPRINT_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::CTMPrintCtrl()
//
// 	Description:	This is the constructor for CTMPrintCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMPrintCtrl::CTMPrintCtrl()
{
	InitializeIIDs(&IID_DTMPrint6, &IID_DTMPrint6Events);

	//	Initialize the local data
	m_pOptions = 0;
	m_iHeight = 0;
	m_iWidth = 0;
	m_bDIBPrintingEnabled = TRUE;

	//	Calculate the size of the options dialog
	CalculateSize();

	//	Get the registry information
	GetRegistration();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::~CTMPrintCtrl()
//
// 	Description:	This is the destructor for CTMPrintCtrl objects
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMPrintCtrl::~CTMPrintCtrl()
{
	if(m_pOptions)
	{
		delete m_pOptions;
	}
}		

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::DoPropertyExchange()
//
// 	Description:	This function manages the exchange of persistant 
//					properties
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::DoPropExchange(CPropExchange* pPX)
{
	BOOL bAutoInit = FALSE;
	BOOL bEnableErrors = FALSE;
	BOOL bIniFile = FALSE;
	BOOL bIniSection = FALSE;
	BOOL bEnablePowerPoint = FALSE;
	BOOL bCollate = FALSE;
	BOOL bCopies = FALSE;
	BOOL bIncludePathInFileName = FALSE;
	BOOL bIncludePageTotal = FALSE;
	BOOL bPrintImage = FALSE;
	BOOL bPrintBarcodeGraphic = FALSE;
	BOOL bPrintBarcodeText = FALSE;
	BOOL bPrintName = FALSE;
	BOOL bPrintDeponent = FALSE;
	BOOL bPrintPageNumber = FALSE;
	BOOL bPrintCellBorder = FALSE;
	BOOL bPrintFileName = FALSE;
	BOOL bPrinter = FALSE;
	BOOL bTemplateName = FALSE;
	BOOL bForceNewPage = FALSE;
	BOOL bUseSlideIds = FALSE;
	BOOL bBarcodeCharacter = FALSE;
	BOOL bShowOptions = FALSE;
	BOOL bBarcodeFont = FALSE;
	BOOL bShowStatus = FALSE;
	BOOL bLeftMargin = FALSE;
	BOOL bTopMargin = FALSE;
	BOOL bPrintCallouts = FALSE;
	BOOL bPrintCalloutBorders = FALSE;
	BOOL bPrintBorderColor = FALSE;
	BOOL bPrintBorderThickness = FALSE;
	BOOL bEnableAxErrors = FALSE;
	BOOL bAutoRotate = FALSE;
	BOOL bPrintForeignBarcode = FALSE;
	BOOL bJobName = FALSE;
	BOOL bPrintSourceBarcode = FALSE;
	BOOL bInsertSlipSheet = FALSE;
	BOOL bCalloutFrameColor = FALSE;

	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));

	COleControl::DoPropExchange(pPX);
	//CheckVersion(pPX->GetVersion());

	try
	{
		//	Load the control's persistent properties
		bAutoInit = PX_Bool(pPX, _T("AutoInit"), m_bAutoInit, TMPRINT_AUTOINIT);
		bEnableErrors = PX_Bool(pPX, _T("EnableErrors"), m_bEnableErrors, TRUE);
		bIniFile = PX_String(pPX, _T("IniFile"), m_strIniFile, TMPRINT_INIFILE);
		bIniSection = PX_String(pPX, _T("IniSection"), m_strIniSection, TMPRINT_INISECTION);
		bEnablePowerPoint = PX_Bool(pPX, _T("EnablePowerPoint"), m_bEnablePowerPoint, TMPRINT_ENABLEPOWERPOINT);
		bCollate = PX_Bool(pPX, _T("Collate"), m_bCollate, TMPRINT_COLLATE);
		bCopies = PX_Short(pPX, _T("Copies"), m_sCopies, TMPRINT_COPIES);
		bIncludePathInFileName = PX_Bool(pPX, _T("IncludePathInFileName"), m_bIncludePathInFileName, TMPRINT_INCLUDEPATHINFILENAME);
		bIncludePageTotal = PX_Bool(pPX, _T("IncludePageTotal"), m_bIncludePageTotal, TMPRINT_INCLUDEPAGETOTAL);
		bPrintImage = PX_Bool(pPX, _T("PrintImage"), m_bPrintImage, TMPRINT_PRINTIMAGE);
		bPrintBarcodeGraphic = PX_Bool(pPX, _T("PrintBarcodeGraphic"), m_bPrintBarcodeGraphic, TMPRINT_PRINTBARCODEGRAPHIC);
		bPrintBarcodeText = PX_Bool(pPX, _T("PrintBarcodeText"), m_bPrintBarcodeText, TMPRINT_PRINTBARCODETEXT);
		bPrintName = PX_Bool(pPX, _T("PrintName"), m_bPrintName, TMPRINT_PRINTNAME);
		bPrintFileName = PX_Bool(pPX, _T("PrintFileName"), m_bPrintFileName, TMPRINT_PRINTFILENAME);
		bPrintDeponent = PX_Bool(pPX, _T("PrintDeponent"), m_bPrintDeponent, TMPRINT_PRINTDEPONENT);
		bPrintPageNumber = PX_Bool(pPX, _T("PrintPageNumber"), m_bPrintPageNumber, TMPRINT_PRINTPAGENUMBER);
		bPrintCellBorder = PX_Bool(pPX, _T("PrintCellBorder"), m_bPrintCellBorder, TMPRINT_PRINTCELLBORDER);
		bPrinter = PX_String(pPX, _T("Printer"), m_strPrinter, TMPRINT_PRINTER);
		bTemplateName = PX_String(pPX, _T("TemplateName"), m_strTemplateName, TMPRINT_TEMPLATENAME);
		bForceNewPage = PX_Bool(pPX, _T("ForceNewPage"), m_bForceNewPage, TMPRINT_FORCENEWPAGE);
		bUseSlideIds = PX_Bool(pPX, _T("UseSlideIDs"), m_bUseSlideIDs, TMPRINT_USESLIDEIDS);
		bBarcodeCharacter = PX_String(pPX, _T("BarcodeCharacter"), m_strBarcodeCharacter, TMPRINT_BARCODECHARACTER);
		bShowOptions = PX_Bool(pPX, _T("ShowOptions"), m_bShowOptions, TMPRINT_SHOWOPTIONS);
		bBarcodeFont = PX_String(pPX, _T("BarcodeFont"), m_strBarcodeFont, TMPRINT_BARCODEFONT);
		bShowStatus = PX_Bool(pPX, _T("ShowStatus"), m_bShowStatus, TMPRINT_SHOWSTATUS);
		bLeftMargin = PX_Float(pPX, _T("LeftMargin"), m_fLeftMargin, TMPRINT_LEFTMARGIN);
		bTopMargin = PX_Float(pPX, _T("TopMargin"), m_fTopMargin, TMPRINT_TOPMARGIN);
		bPrintCallouts = PX_Bool(pPX, _T("PrintCallouts"), m_bPrintCallouts, TMPRINT_PRINTCALLOUTS);
		bPrintCalloutBorders = PX_Bool(pPX, _T("PrintCalloutBorders"), m_bPrintCalloutBorders, TMPRINT_PRINTCALLOUTBORDERS);
		bPrintBorderColor = PX_Color(pPX, _T("PrintBorderColor"), m_lPrintBorderColor, TMPRINT_PRINTBORDERCOLOR);
		bPrintBorderThickness = PX_Float(pPX, _T("PrintBorderThickness"), m_fPrintBorderThickness, TMPRINT_PRINTBORDERTHICKNESS);
		bEnableAxErrors = PX_Bool(pPX, _T("EnableAxErrors"), m_bEnableAxErrors, TMPRINT_ENABLEAXERRORS);
		bAutoRotate = PX_Bool(pPX, _T("AutoRotate"), m_bAutoRotate, TMPRINT_AUTOROTATE);
		bPrintForeignBarcode = PX_Bool(pPX, _T("PrintForeignBarcode"), m_bPrintForeignBarcode, TMPRINT_PRINTFOREIGNBARCODE);
		bJobName = PX_String(pPX, _T("JobName"), m_strJobName, TMPRINT_JOBNAME);
		bPrintSourceBarcode = PX_Bool(pPX, _T("PrintSourceBarcode"), m_bPrintSourceBarcode, TMPRINT_PRINTSOURCEBARCODE);
		bInsertSlipSheet = PX_Bool(pPX, _T("InsertSlipSheet"), m_bInsertSlipSheet, TMPRINT_INSERTSLIPSHEET);
		bCalloutFrameColor = PX_Short(pPX, _T("CalloutFrameColor"), m_sCalloutFrameColor, TMPRINT_CALLOUTFRAMECOLOR);
	}
	catch(...)
	{
		if(!bAutoInit) m_bAutoInit = TMPRINT_AUTOINIT;
		if(!bEnableErrors) m_bEnableErrors = TRUE;
		if(!bIniFile) m_strIniFile = TMPRINT_INIFILE;
		if(!bIniSection) m_strIniSection = TMPRINT_INISECTION;
		if(!bEnablePowerPoint) m_bEnablePowerPoint = TMPRINT_ENABLEPOWERPOINT;
		if(!bCollate) m_bCollate = TMPRINT_COLLATE;
		if(!bCopies) m_sCopies = TMPRINT_COPIES;
		if(!bIncludePathInFileName) m_bIncludePathInFileName = TMPRINT_INCLUDEPATHINFILENAME;
		if(!bIncludePageTotal) m_bIncludePageTotal = TMPRINT_INCLUDEPAGETOTAL;
		if(!bPrintImage) m_bPrintImage = TMPRINT_PRINTIMAGE;
		if(!bPrintBarcodeGraphic) m_bPrintBarcodeGraphic = TMPRINT_PRINTBARCODEGRAPHIC;
		if(!bPrintBarcodeText) m_bPrintBarcodeText = TMPRINT_PRINTBARCODETEXT;
		if(!bPrintName) m_bPrintName = TMPRINT_PRINTNAME;
		if(!bPrintFileName) m_bPrintFileName = TMPRINT_PRINTFILENAME;
		if(!bPrintDeponent) m_bPrintDeponent = TMPRINT_PRINTDEPONENT;
		if(!bPrintPageNumber) m_bPrintPageNumber = TMPRINT_PRINTPAGENUMBER;
		if(!bPrintCellBorder) m_bPrintCellBorder = TMPRINT_PRINTCELLBORDER;
		if(!bPrinter) m_strPrinter = TMPRINT_PRINTER;
		if(!bTemplateName) m_strTemplateName = TMPRINT_TEMPLATENAME;
		if(!bForceNewPage) m_bForceNewPage = TMPRINT_FORCENEWPAGE;
		if(!bUseSlideIds) m_bUseSlideIDs = TMPRINT_USESLIDEIDS;
		if(!bBarcodeCharacter) m_strBarcodeCharacter = TMPRINT_BARCODECHARACTER;
		if(!bShowOptions) m_bShowOptions = TMPRINT_SHOWOPTIONS;
		if(!bBarcodeFont) m_strBarcodeFont = TMPRINT_BARCODEFONT;
		if(!bShowStatus) m_bShowStatus = TMPRINT_SHOWSTATUS;
		if(!bLeftMargin) m_fLeftMargin = TMPRINT_LEFTMARGIN;
		if(!bTopMargin) m_fTopMargin = TMPRINT_TOPMARGIN;
		if(!bPrintCallouts) m_bPrintCallouts = TMPRINT_PRINTCALLOUTS;
		if(!bPrintCalloutBorders) m_bPrintCalloutBorders = TMPRINT_PRINTCALLOUTBORDERS;
		if(!bPrintBorderColor) m_lPrintBorderColor = TMPRINT_PRINTBORDERCOLOR;
		if(!bPrintBorderThickness) m_fPrintBorderThickness = TMPRINT_PRINTBORDERTHICKNESS;
		if(!bEnableAxErrors) m_bEnableAxErrors = TMPRINT_ENABLEAXERRORS;
		if(!bAutoRotate) m_bAutoRotate = TMPRINT_AUTOROTATE;
		if(!bPrintForeignBarcode) m_bPrintForeignBarcode = TMPRINT_PRINTFOREIGNBARCODE;
		if(!bJobName) m_strJobName = TMPRINT_JOBNAME;
		if(!bPrintSourceBarcode) m_bPrintSourceBarcode = TMPRINT_PRINTSOURCEBARCODE;
		if(!bInsertSlipSheet) m_bInsertSlipSheet = TMPRINT_INSERTSLIPSHEET;
		if(!bCalloutFrameColor) m_sCalloutFrameColor = TMPRINT_CALLOUTFRAMECOLOR;
	}

	//	Set default values for new properties
	if(pPX->IsLoading())
	{
		switch(LOWORD(pPX->GetVersion()))
		{
			case 1:

				m_strJobName = "";
				
				//	Drop through
				//		.
				//		.
				//		.

			case 0:

				break;

		}
	}

}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::EnableDIBPrinting()
//
// 	Description:	This function is called to enable/disable DIB printing
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::EnableDIBPrinting(short bEnable) 
{
	//	Update the local class member
	m_bDIBPrintingEnabled = bEnable;

	//	Notify the options form
	if(m_pOptions != NULL)
		m_pOptions->EnableDIBPrinting(bEnable);

}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::EnumeratePrinters()
//
// 	Description:	This method is called to force the control to enumerate the
//					list of printers available on this machine
//
// 	Returns:		TMPRINT_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPrintCtrl::EnumeratePrinters() 
{
	CTMPrinter	Printer;
	CObList*	pPrinters = NULL;
	POSITION	Pos = NULL;
	CString*	pPrinter = NULL;
	BOOL		bFirstPrinter = TRUE;

	if((pPrinters = Printer.EnumPrinters()) != NULL)
	{
		Pos = pPrinters->GetHeadPosition();
		while(Pos != NULL)
		{
			if((pPrinter = (CString*)pPrinters->GetNext(Pos)) != NULL)
			{
				if(bFirstPrinter == TRUE)
				{
					FireFirstPrinter(*pPrinter);
					bFirstPrinter = FALSE;
				}
				else
				{
					FireNextPrinter(*pPrinter);
				}

				delete pPrinter;
				pPrinter = NULL;
			
			}// if((pPrinter = (CString*)pPrinters->GetNext(Pos)) != NULL)

		}// while(Pos != NULL)	
		
		delete pPrinters;		
	
	}// if((pPrinters = Printer.EnumPrinters()) != NULL)

	return TMPRINT_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::EnumerateTemplates()
//
// 	Description:	This method instructs the control to enumerate the list of
//					templates
//
// 	Returns:		TMPRINT_NOERROR  if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPrintCtrl::EnumerateTemplates() 
{
	CTemplates* pTemplates;
	CTemplate*	pTemplate;

	//	Has the control been intialized?
	if(m_pOptions == 0)
	{
		m_Errors.Handle(0, IDS_TMPRINT_NOTINITIALIZED);
		return TMPRINT_NOTINITIALIZED;
	}
	else 
	{
		if((pTemplates = m_pOptions->GetTemplates()) != 0)
		{
			pTemplate = pTemplates->GetFirstTemplate();
			if(pTemplate)
				FireFirstTemplate(pTemplate->m_strDescription);

			while(pTemplate != 0)
			{
				pTemplate = pTemplates->GetNextTemplate();
				if(pTemplate)
					FireNextTemplate(pTemplate->m_strDescription);
			}
		}
		return TMPRINT_NOERROR;
	}
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::EnumerateTextFields()
//
// 	Description:	This method instructs the control to enumerate the list of
//					text fields in the specified template
//
// 	Returns:		TMPRINT_NOERROR  if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPrintCtrl::EnumerateTextFields(LPCTSTR lpszTemplate) 
{
	CTemplate*		pTemplate = NULL;
	CTemplateField*	pField = NULL;

	//	Has the control been intialized?
	if(m_pOptions == NULL)
	{
		m_Errors.Handle(0, IDS_TMPRINT_NOTINITIALIZED);
		return TMPRINT_NOTINITIALIZED;
	}
	else 
	{
		//	Get the requested template
		if((pTemplate = m_pOptions->GetTemplate(lpszTemplate)) != NULL)
		{
			pField = pTemplate->m_aTextFields.First();
			if(pField)
				FireFirstTextField(pField->m_iId, pField->m_strName, pField->m_strText, pField->m_bPrint, pField->m_bDefault);

			while(pField != 0)
			{
				pField = pTemplate->m_aTextFields.Next();
				if(pField)
					FireNextTextField(pField->m_iId, pField->m_strName, pField->m_strText, pField->m_bPrint, pField->m_bDefault);
			}

			return TMPRINT_NOERROR;
		}
		else
		{
			m_Errors.Handle(0, IDS_TMPRINT_TEMPLATENOTFOUND, lpszTemplate);
			return TMPRINT_TEMPLATENOTFOUND;
		}

	}
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::GetClassIdString()
//
// 	Description:	This method is called to get the class GUID as a null
//					terminated string.
//
// 	Returns:		The string equivalent of the CLASS GUID
//
//	Notes:			None
//
//==============================================================================
BSTR CTMPrintCtrl::GetClassIdString() 
{
	CString strClsId = m_tmVersion.GetClsId();
	return strClsId.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::GetColumnsPerPage()
//
// 	Description:	This method is called to get the number of columns per page
//					defined by the current template.
//
// 	Returns:		The number of columns in each page
//
//	Notes:			None
//
//==============================================================================
short CTMPrintCtrl::GetColumnsPerPage() 
{
	//	Has the control been intialized?
	if(m_pOptions != 0)
	{
		return m_pOptions->GetColumnsPerPage();
	}
	else
	{
		return 0;
	}
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::GetDefaultPrinter()
//
// 	Description:	This method is called to get the name of the default printer
//					device.
//
// 	Returns:		The default printer name
//
//	Notes:			None
//
//==============================================================================
BSTR CTMPrintCtrl::GetDefaultPrinter() 
{
	CString strPrinter;

	//	Has the control been intialized?
	if(m_pOptions == 0)
	{
		strPrinter.Empty();
	}
	else
	{	
		//	Get the name of the default printer
		m_pOptions->GetDefaultPrinter(strPrinter);
	}

	return strPrinter.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::GetFieldDefaultMask()
//
// 	Description:	This function is called to get the mask that represents
//					the fields in the specified template that are turned on by
//					default
//
// 	Returns:		The bitmask that represents fields that are defaulted to ON
//
//	Notes:			None
//
//==============================================================================
long CTMPrintCtrl::GetFieldDefaultMask(LPCTSTR lpszTemplate) 
{
	//	Has the control been intialized?
	if(m_pOptions != 0)
	{
		return m_pOptions->GetDefaultMask(lpszTemplate); 
	}
	else
	{
		return 0;
	}
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::GetFieldEnabledMask()
//
// 	Description:	This function is called to get the mask that represents
//					the fields that have been enabled for the specified template
//
// 	Returns:		The bitmask that represents fields that are enabled for the
//					specified template
//
//	Notes:			None
//
//==============================================================================
long CTMPrintCtrl::GetFieldEnabledMask(LPCTSTR lpszTemplate) 
{
	//	Has the control been intialized?
	if(m_pOptions != 0)
	{
		return m_pOptions->GetEnabledMask(lpszTemplate); 
	}
	else
	{
		return 0;
	}
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::GetPrintTemplate()
//
// 	Description:	This method is called to set the get the current template
//					selected for the print job
//
// 	Returns:		A pointer to the active template
//
//	Notes:			None
//
//==============================================================================
long CTMPrintCtrl::GetPrintTemplate() 
{
	//	Has the control been intialized?
	if(m_pOptions != 0)
	{
		return ((long)m_pOptions->GetTemplate());
	}
	else
	{
		return 0;
	}
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::GetPrintTemplates()
//
// 	Description:	This method is called to set the get the list of templates
//					available for all print jobs.
//
// 	Returns:		A pointer to the list of templates
//
//	Notes:			None
//
//==============================================================================
long CTMPrintCtrl::GetPrintTemplates() 
{
	//	Has the control been intialized?
	if(m_pOptions != 0)
	{
		return ((long)m_pOptions->GetTemplates());
	}
	else
	{
		return 0;
	}
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::GetQueueCount()
//
// 	Description:	This function is called to retrieve the number of entries
//					in the queue.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
long CTMPrintCtrl::GetQueueCount() 
{
	if(m_pOptions)
		return m_pOptions->GetQueueCount();
	else
		return 0;
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::GetRegisteredPath()
//
// 	Description:	This method is called to get the ocx path stored in the
//					system registry.
//
// 	Returns:		The path to the ocx stored in the registry
//
//	Notes:			None
//
//==============================================================================
BSTR CTMPrintCtrl::GetRegisteredPath() 
{
	CString strRegistered = m_tmVersion.GetFileSpec();
	return strRegistered.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::GetRegistration()
//
// 	Description:	This function is called to get the control's registration
//					information.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::GetRegistration() 
{
	CLSID	clsid;

	//	Get the GUID 
	GetClassID(&clsid);

	//	Initialize the version information
	m_tmVersion.InitFromClsId("TMPrint", "Template Printing Control", clsid);

}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::GetRowsPerPage()
//
// 	Description:	This method is called to get the number of rows per page
//					defined by the current template.
//
// 	Returns:		The number of rows in each page
//
//	Notes:			None
//
//==============================================================================
short CTMPrintCtrl::GetRowsPerPage() 
{
	//	Has the control been intialized?
	if(m_pOptions != 0)
	{
		return m_pOptions->GetRowsPerPage();
	}
	else
	{
		return 0;
	}
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::GetVerBuild()
//
// 	Description:	This method is called to get the value of the VerBuild
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMPrintCtrl::GetVerBuild() 
{
	return m_tmVersion.GetBuild();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::GetVerBuildDate()
//
// 	Description:	This method is called to get the value of the VerBuildDate
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMPrintCtrl::GetVerBuildDate() 
{
	CString strBuildDate = m_tmVersion.GetBuildDate();
	return strBuildDate.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::GetVerMajor()
//
// 	Description:	This method is called to get the value of the VerMajor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMPrintCtrl::GetVerMajor() 
{
	return m_tmVersion.GetMajor();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::GetVerMinor()
//
// 	Description:	This method is called to get the value of the VerMinor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMPrintCtrl::GetVerMinor() 
{
	return m_tmVersion.GetMinor();
}


//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::GetVerQEF()
//
// 	Description:	This method is called to get the value of the VerQEF
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMPrintCtrl::GetVerQEF() 
{
	return m_tmVersion.GetUpdate();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::GetVerTextLong()
//
// 	Description:	This method is called to get the value of the VerTextLong
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMPrintCtrl::GetVerTextLong() 
{
	CString strVer = m_tmVersion.GetTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::GetVerTextShort()
//
// 	Description:	This method is called to get the value of the VerTextShort
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMPrintCtrl::GetVerTextShort() 
{
	CString strVer = m_tmVersion.GetShortTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::Initialize()
//
// 	Description:	This function will construct and initialize the lpenbar
//
// 	Returns:		TMDP_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPrintCtrl::Initialize()
{
	//	Don't bother if not in user mode
	if(!AmbientUserMode())
		return TMPRINT_NOERROR;

	//	Is the control already initialized?
	if((m_pOptions != 0) && IsWindow(m_pOptions->m_hWnd))
		return TMPRINT_NOERROR;

	//	Allocate the overlay window
	if(m_pOptions)
		delete m_pOptions;
	m_pOptions = new COptions(this, &m_Errors);
	ASSERT(m_pOptions);

	//	Set these options BEFORE creating the dialog box
	m_pOptions->EnablePowerPoint(m_bEnablePowerPoint);
	m_pOptions->EnableDIBPrinting(m_bDIBPrintingEnabled);
	m_pOptions->SetCalloutFrameColor(m_sCalloutFrameColor);

	//	Create the window
	if(!m_pOptions->Create())
	{
		m_Errors.Handle(0, IDS_TMPRINT_CREATEOPTIONSFAILED);
		if(m_pOptions)
		{
			delete m_pOptions;
			m_pOptions = 0;
		}
	}
	else
	{
		//	Set the printer
		m_pOptions->SetPrinter(m_strPrinter);

		//	Make sure the options are set from the ini file if a filename
		//	and section have been provided
		if((m_strIniFile.GetLength() > 0) && (m_strIniSection.GetLength() > 0))
		{
			m_pOptions->SetFromIni(m_strIniFile, m_strIniSection);

			//	Set the control properties to match
			SetProperties(m_pOptions->GetTemplate());
			m_bShowStatus = m_pOptions->m_bShowStatus;
			m_bForceNewPage = m_pOptions->m_bForceNewPage;
			m_bIncludePathInFileName = m_pOptions->m_bIncludePath;
			m_bIncludePageTotal = m_pOptions->m_bIncludeTotal;
			m_bCollate = m_pOptions->m_bCollate;
			m_sCopies = m_pOptions->m_sCopies;
			m_bInsertSlipSheet = m_pOptions->m_bInsertSlipSheet;

			//	Should we make the options dialog visible?
			if(m_bShowOptions)
				m_pOptions->ShowWindow(SW_SHOW);
		}
		else
		{

			//	Should we make the options dialog visible?
			if(m_bShowOptions)
				m_pOptions->ShowWindow(SW_SHOW);

			//	Initialize the options dialog using the control properties
			OnTemplateNameChanged();
			OnShowStatusChanged();
			OnForceNewPageChanged();
			OnIncludePathInFileNameChanged();
			OnIncludePageTotalChanged();
			OnCollateChanged();
			OnCopiesChanged();
			OnInsertSlipSheetChanged();
		}
	}

	return TMPRINT_NOERROR;
}	

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::IsReady()
//
// 	Description:	This function is called to determine if the control is ready
//					to accept print jobs
//
// 	Returns:		TRUE if ready
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPrintCtrl::IsReady()
{
	//	Has the control been initialized?
	if(m_pOptions != 0)
	{
		//	Has a specific printer been defined?
		if(m_strPrinter.GetLength() == 0)
		{
			//	Use the default printer
			m_pOptions->GetDefaultPrinter(m_strPrinter);
		}

		if(m_strPrinter.GetLength() > 0)
		{
			//	Do we have a valid template?
			if(m_pOptions->GetTemplate() != 0)
				return TRUE;
		}
	}

	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnAutoInitChanged()
//
// 	Description:	This function is called when the AutoInit property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnAutoInitChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnAutoRotateChanged()
//
// 	Description:	This function is called when the AutoRotate property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnAutoRotateChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnBarcodeCharacterChanged()
//
// 	Description:	This function is called when the BarcodeCharacter property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnBarcodeCharacterChanged() 
{
	if(AmbientUserMode() && m_pOptions)
	{
		if(!m_strBarcodeCharacter.IsEmpty())
			m_pOptions->SetBarcodeCharacter(m_strBarcodeCharacter.GetAt(0));
		else
			m_pOptions->SetBarcodeCharacter(0);
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnBarcodeFontChanged()
//
// 	Description:	This function is called when the BarcodeFont property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnBarcodeFontChanged() 
{
	if(AmbientUserMode() && m_pOptions)
		m_pOptions->SetBarcodeFont(m_strBarcodeFont);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnPrintBorderColorChanged()
//
// 	Description:	This function is called when the PrintBorderColor property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnPrintBorderColorChanged() 
{
	if(AmbientUserMode() && m_pOptions)
		m_pOptions->SetPrintBorderColor(TranslateColor(m_lPrintBorderColor));

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnPrintBorderThicknessChanged()
//
// 	Description:	This function is called when the PrintBorderThickness property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnPrintBorderThicknessChanged() 
{
	if(AmbientUserMode() && m_pOptions)
		m_pOptions->SetPrintBorderThickness(m_fPrintBorderThickness);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnCalloutFrameColorChanged()
//
// 	Description:	This function is called when the CalloutFrameColor property
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnCalloutFrameColorChanged() 
{
	if(AmbientUserMode() && m_pOptions)
		m_pOptions->SetCalloutFrameColor(m_sCalloutFrameColor);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnCollateChanged()
//
// 	Description:	This function is called when the Collate property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnCollateChanged() 
{
	if(AmbientUserMode() && m_pOptions)
		m_pOptions->SetCollate(m_bCollate);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnCopiesChanged()
//
// 	Description:	This function is called when the Copies property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnCopiesChanged() 
{
	if(AmbientUserMode() && m_pOptions)
		m_pOptions->SetCopies(m_sCopies);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnCreate()
//
// 	Description:	This fuction handles all WM_CREATE messages
//
// 	Returns:		0 if successful
//
//	Notes:			None
//
//==============================================================================
int CTMPrintCtrl::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
	//	Do the base class processing first
	if(COleControl::OnCreate(lpCreateStruct) == -1)
		return -1;

	//	Initialize the error handler
	m_Errors.Enable(m_bEnableErrors);
	m_Errors.SetParent(m_hWnd);
	m_Errors.SetTitle("TMPrint Error");
	m_Errors.SetMessageId(m_bEnableAxErrors == TRUE ? WM_ERROR_EVENT : 0);

	//	Initialize the control
	if(m_bAutoInit)
		Initialize();
	
	return 0;
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnDraw()
//
// 	Description:	This function is called in response to WM_PAINT messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnDraw(CDC* pdc, const CRect& rcBounds,const CRect& rcInvalid)
{
	CBrush brBackground;

	//	Are we in user mode?
	if(AmbientUserMode())
	{
		//	Create a brush using the background color
		brBackground.CreateSolidBrush(TranslateColor(GetBackColor()));

		//	Is the options dialog visible?
		if(m_pOptions && IsWindow(m_pOptions->m_hWnd) && m_bShowOptions)
		{
			pdc->FillRect(rcBounds, &brBackground);
			m_pOptions->RedrawWindow();
		}
		else
		{
			//	Fill the client area
			pdc->FillRect(rcBounds, &brBackground);
		}
	}
	else
	{
		CString	strText;
		CRect ControlRect = rcBounds;

		strText.Format("FTI Print Control (rev. %d.%d)", _wVerMajor, _wVerMinor);

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
// 	Function Name:	CTMPrintCtrl::OnEnableAxErrorsChanged()
//
// 	Description:	This function is called when the EnableAxErrors property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnEnableAxErrorsChanged() 
{
	SetModifiedFlag();

	if(AmbientUserMode())
		m_Errors.SetMessageId(m_bEnableAxErrors == TRUE ? WM_ERROR_EVENT : 0);
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnEnableErrorsChanged()
//
// 	Description:	This function is called when the EnableErrors property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnEnableErrorsChanged() 
{
	m_Errors.Enable(m_bEnableErrors);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnEnablePowerPointChanged()
//
// 	Description:	This function is called when the EnablePowerPoint property
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnEnablePowerPointChanged() 
{
	if(AmbientUserMode() && m_pOptions)
		m_pOptions->EnablePowerPoint(m_bEnablePowerPoint);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnIncludePathInFileNameChanged()
//
// 	Description:	This function is called when the IncludePathInFileName
//					property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnIncludePathInFileNameChanged() 
{
	if(AmbientUserMode() && m_pOptions)
		m_pOptions->SetIncludePath(m_bIncludePathInFileName);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnEndJob()
//
// 	Description:	This function handles notifications from the options dialog
//					when it ends a print job.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnEndJob(BOOL bAborted) 
{
	//	Fire the event
	FireEndJob(bAborted ? 1 : 0);
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnForceNewPageChanged()
//
// 	Description:	This function is called when the ForceNewPage property
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnForceNewPageChanged() 
{
	if(AmbientUserMode() && m_pOptions)
		m_pOptions->SetForceNewPage(m_bForceNewPage);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnIncludePageTotalChanged()
//
// 	Description:	This function is called when the IncludePageTotal property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnIncludePageTotalChanged() 
{
	if(AmbientUserMode() && m_pOptions)
		m_pOptions->SetIncludeTotal(m_bIncludePageTotal);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnIniFileChanged()
//
// 	Description:	This function is called when the IniFile property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnIniFileChanged() 
{
	if(AmbientUserMode() && (m_pOptions != 0))
		m_pOptions->SetFromIni(m_strIniFile, m_strIniSection);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnIniSectionChanged()
//
// 	Description:	This function is called when the IniSection property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnIniSectionChanged() 
{
	if(AmbientUserMode() && (m_pOptions != 0))
		m_pOptions->SetFromIni(m_strIniFile, m_strIniSection);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnInsertSlipSheetChanged()
//
// 	Description:	This function is called when the InsertSlipSheet property
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnInsertSlipSheetChanged() 
{
	if(AmbientUserMode() && m_pOptions)
		m_pOptions->SetInsertSlipSheet(m_bInsertSlipSheet);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnJobNameChanged()
//
// 	Description:	This function is called when the JobName property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnJobNameChanged() 
{
	if(AmbientUserMode() && m_pOptions)
	{
		m_pOptions->SetJobName(m_strJobName);
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnLeftMarginChanged()
//
// 	Description:	This function is called when the LeftMargin property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnLeftMarginChanged() 
{
	if(AmbientUserMode() && m_pOptions)
		m_pOptions->SetLeftMargin(m_fLeftMargin);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnPrintBarcodeGraphicChanged()
//
// 	Description:	This function is called when the PrintBarcodeGraphic
//					property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnPrintBarcodeGraphicChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnPrintBarcodeTextChanged()
//
// 	Description:	This function is called when the PrintBarcodeText
//					property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnPrintBarcodeTextChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnPrintCalloutBordersChanged()
//
// 	Description:	This function is called when the PrintCalloutBorders property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnPrintCalloutBordersChanged() 
{
	if(AmbientUserMode() && m_pOptions)
		m_pOptions->SetPrintCalloutBorders(m_bPrintCalloutBorders);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnPrintCalloutsChanged()
//
// 	Description:	This function is called when the PrintCallouts property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnPrintCalloutsChanged() 
{
	if(AmbientUserMode() && m_pOptions)
		m_pOptions->SetPrintCallouts(m_bPrintCallouts);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnPrintCellBorderChanged()
//
// 	Description:	This function is called when the PrintCellBorder property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnPrintCellBorderChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnPrintDeponentChanged()
//
// 	Description:	This function is called when the PrintDeponent property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnPrintDeponentChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnPrinterChanged()
//
// 	Description:	This function is called when the Printer property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnPrinterChanged() 
{
	if(AmbientUserMode() && (m_pOptions != 0))
	{
		if(!m_pOptions->SetPrinter(m_strPrinter))
		{
			m_Errors.Handle(0, IDS_TMPRINT_PRINTERNOTFOUND, m_strPrinter);
			m_strPrinter = m_pOptions->m_strPrinter;
		}
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnPrintFileNameChanged()
//
// 	Description:	This function is called when the PrintFileName property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnPrintFileNameChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnPrintForeignBarcodeChanged()
//
// 	Description:	This function is called when the PrintForeignBarcode 
//					property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnPrintForeignBarcodeChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnPrintImage()
//
// 	Description:	This function handles notifications from the options dialog
//					when it starts a new image.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnPrintImage(long lImage, LPCSTR lpFilename) 
{
	//	Fire the event
	FirePrintImage(lImage, lpFilename);
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnPrintImageChanged()
//
// 	Description:	This function is called when the PrintImage property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnPrintImageChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnPrintNameChanged()
//
// 	Description:	This function is called when the PrintName property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnPrintNameChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnPrintPage()
//
// 	Description:	This function handles notifications from the options dialog
//					when it starts a new page.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnPrintPage(long lPage) 
{
	//	Fire the event
	FirePrintPage(lPage);
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnPrintPageNumberChanged()
//
// 	Description:	This function is called when the PrintPageNumber property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnPrintPageNumberChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnPrintSourceBarcodeChanged()
//
// 	Description:	This function is called when the PrintSourceBarcode 
//					property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnPrintSourceBarcodeChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnSetExtent()
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
BOOL CTMPrintCtrl::OnSetExtent(LPSIZEL lpSizeL) 
{
	CWnd* pWindow;

	//	If we are not showing the options then don't do anything
	if(!m_bShowOptions)
		return COleControl::OnSetExtent(lpSizeL);

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
// 	Function Name:	CTMPrintCtrl::OnSetFocus()
//
// 	Description:	This function is called when the control gains the keyboard
//					focus.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnSetFocus(CWnd* pOldWnd) 
{
	COleControl::OnSetFocus(pOldWnd);
	
	//	Set the focus to the options dialog if it's visible
	if(m_bShowOptions && m_pOptions)
		m_pOptions->SetFocus();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnShowOptionsChanged()
//
// 	Description:	This function is called when the ShowOptions property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnShowOptionsChanged() 
{
	if(m_pOptions)
	{
		//	Set the window visibility
		m_pOptions->ShowWindow(m_bShowOptions ? SW_SHOW : SW_HIDE);
	}

	//	Force correct sizing if the control is visible
	if(m_bShowOptions)
		SetControlSize(1,1);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnShowStatusChanged()
//
// 	Description:	This function is called when the ShowStatus property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnShowStatusChanged() 
{
	if(AmbientUserMode() && m_pOptions)
		m_pOptions->SetShowStatus(m_bShowStatus);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnStartJob()
//
// 	Description:	This function handles notifications from the options dialog
//					when it starts a new print job.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnStartJob(LPCSTR lpPrinter, long lPages, long lImages,
							  CTemplate* pTemplate) 
{
	//	Fire the event
	FireStartJob(lpPrinter, lPages, lImages, (long)pTemplate);
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnTemplateNameChanged()
//
// 	Description:	This function is called when the TemplateName property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnTemplateNameChanged() 
{
	if(m_pOptions)
	{
		m_pOptions->SetTemplate(m_strTemplateName);

		//	Update the control properties
		SetProperties(m_pOptions->GetTemplate());
	}
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnTopMarginChanged()
//
// 	Description:	This function is called when the TopMargin property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnTopMarginChanged() 
{
	if(AmbientUserMode() && m_pOptions)
		m_pOptions->SetTopMargin(m_fTopMargin);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnUseSlideIDsChanged()
//
// 	Description:	This function is called when the UseSlideIDs property
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::OnUseSlideIDsChanged() 
{
	if(AmbientUserMode() && m_pOptions)
		m_pOptions->SetUseSlideId(m_bUseSlideIDs);

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::OnWMErrorEvent()
//
// 	Description:	This function handles all WM_ERROR_EVENT messages
//
// 	Returns:		Zero if the message is handled
//
//	Notes:			None
//
//==============================================================================
LONG CTMPrintCtrl::OnWMErrorEvent(WPARAM wParam, LPARAM lParam)
{
	if((m_bEnableAxErrors == TRUE) && (lstrlen(m_Errors.GetMessage()) > 0))
	{
		FireAxError(m_Errors.GetMessage());
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::PreCreateWindow()
//
// 	Description:	This function is called by the framework before creating the
//					window
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPrintCtrl::PreCreateWindow(CREATESTRUCT& cs) 
{
	return COleControl::PreCreateWindow(cs);
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::PreTranslateMessage()
//
// 	Description:	This function is called trap all messages sent to the
//					control window.
//
// 	Returns:		TRUE if the message is handled
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPrintCtrl::PreTranslateMessage(MSG* pMsg) 
{
	//	Do not propagate the escape and return keys to the options dialog
	if(pMsg->wParam == VK_ESCAPE || pMsg->wParam == VK_RETURN)
		return COleControl::PreTranslateMessage(pMsg);

	//	Let the options dialog process the message if it's visible
	if(m_bShowOptions && m_pOptions)
	{
		if(m_pOptions->PreTranslateMessage(pMsg))
			return TRUE;
		else
			return COleControl::PreTranslateMessage(pMsg);
	}
	else
	{
		return COleControl::PreTranslateMessage(pMsg);
	}
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::Print()
//
// 	Description:	This method is called to print the current contents of the
//					queue
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
short CTMPrintCtrl::Print() 
{
	CTemplate* pTemplate;

	//	Has the control been intialized?
	if(m_pOptions == 0)
	{
		m_Errors.Handle(0, IDS_TMPRINT_NOTINITIALIZED);
		return TMPRINT_NOTINITIALIZED;
	}

	//	Set the current template using the TemplateName property
	if((pTemplate = m_pOptions->SetTemplate(m_strTemplateName)) == 0)
	{
		m_Errors.Handle(0, IDS_TMPRINT_TEMPLATENOTFOUND, m_strTemplateName);
		return TMPRINT_TEMPLATENOTFOUND;
	}

	//	Set the current printer using the Printer property
	if(!m_strPrinter.IsEmpty())
	{
		if(!m_pOptions->SetPrinter(m_strPrinter))
		{
			m_Errors.Handle(0, IDS_TMPRINT_PRINTERNOTFOUND, m_strPrinter);
			return TMPRINT_PRINTERNOTFOUND;
		}
	}

	//	Set the template's runtime options
	pTemplate->m_bPrintImage = m_bPrintImage;
	pTemplate->m_bPrintBorder = m_bPrintCellBorder;
	pTemplate->m_bPrintFullPath = m_bIncludePathInFileName;
	pTemplate->m_bPageAsSeries = m_bIncludePageTotal;
	pTemplate->m_bAutoRotate = m_bAutoRotate;

	pTemplate->SetPrintEnabled(TEMPLATE_NAME, m_bPrintName);
	pTemplate->SetPrintEnabled(TEMPLATE_BARCODE, m_bPrintBarcodeText);
	pTemplate->SetPrintEnabled(TEMPLATE_GRAPHIC, m_bPrintBarcodeGraphic);
	pTemplate->SetPrintEnabled(TEMPLATE_FILENAME, m_bPrintFileName);
	pTemplate->SetPrintEnabled(TEMPLATE_PAGENUM, m_bPrintPageNumber);
	pTemplate->SetPrintEnabled(TEMPLATE_DEPONENT, m_bPrintDeponent);
	pTemplate->SetPrintEnabled(TEMPLATE_FOREIGN_BARCODE, m_bPrintForeignBarcode);
	pTemplate->SetPrintEnabled(TEMPLATE_SOURCE_BARCODE, m_bPrintSourceBarcode);

	//	Set the job options
	m_pOptions->SetForceNewPage(m_bForceNewPage);
	m_pOptions->SetInsertSlipSheet(m_bInsertSlipSheet);
	m_pOptions->SetUseSlideId(m_bUseSlideIDs);
	m_pOptions->SetBarcodeFont(m_strBarcodeFont);
	m_pOptions->SetJobName(m_strJobName);
	m_pOptions->SetCopies(m_sCopies);
	m_pOptions->SetCollate(m_bCollate);
	m_pOptions->SetShowStatus(m_bShowStatus);
	m_pOptions->SetPrintCallouts(m_bPrintCallouts);
	m_pOptions->SetPrintCalloutBorders(m_bPrintCalloutBorders);
	m_pOptions->SetPrintBorderColor(TranslateColor(m_lPrintBorderColor));
	m_pOptions->SetPrintBorderThickness(m_fPrintBorderThickness);
	m_pOptions->SetLeftMargin(m_fLeftMargin);
	m_pOptions->SetTopMargin(m_fTopMargin);

	if(!m_strBarcodeCharacter.IsEmpty())
		m_pOptions->SetBarcodeCharacter(m_strBarcodeCharacter.GetAt(0));
	else
		m_pOptions->SetBarcodeCharacter(0);

	return m_pOptions->Print();
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::RefreshTemplates()
//
// 	Description:	This method is called to refresh the current template list
//					from the ini file.
//
// 	Returns:		TMPRINT_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPrintCtrl::RefreshTemplates() 
{
	//	Has the control been intialized?
	if(m_pOptions == 0)
	{
		m_Errors.Handle(0, IDS_TMPRINT_NOTINITIALIZED);
		return TMPRINT_NOTINITIALIZED;
	}
	else
	{
		return m_pOptions->RefreshTemplates();
	}
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::SelectPrinter()
//
// 	Description:	This method is called to select the printer to be used for
//					susequent print jobs
//
// 	Returns:		TMPRINT_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPrintCtrl::SelectPrinter() 
{
	//	Has the control been intialized?
	if(m_pOptions == 0)
	{
		m_Errors.Handle(0, IDS_TMPRINT_NOTINITIALIZED);
		return TMPRINT_NOTINITIALIZED;
	}
	else
	{
		//	Initialize the options
		m_pOptions->SetPrinter(m_strPrinter);
		m_pOptions->SetCopies(m_sCopies);
		m_pOptions->SetCollate(m_bCollate);

		if(m_pOptions->SelectPrinter())
		{
			//	Save the new property values
			m_strPrinter = m_pOptions->m_strPrinter;
			m_sCopies = m_pOptions->m_sCopies;
			m_bCollate = m_pOptions->m_bCollate;
		}

		return TMPRINT_NOERROR;
	}
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::SetPrinterProperties()
//
// 	Description:	This method is called to open the property sheet for the
//					current printer
//
// 	Returns:		TMPRINT_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMPrintCtrl::SetPrinterProperties(OLE_HANDLE hWnd) 
{
	CString strPrinter;

	//	Has the control been intialized?
	if(m_pOptions == 0)
	{
		m_Errors.Handle(0, IDS_TMPRINT_NOTINITIALIZED);
		return TMPRINT_NOTINITIALIZED;
	}
	else
	{
		//	Make sure the options dialog has the current settings
		m_pOptions->SetPrinter(m_strPrinter);
		m_pOptions->SetCopies(m_sCopies);
		m_pOptions->SetCollate(m_bCollate);
		m_pOptions->SetTemplate(m_strTemplateName);	//	Template controls the orientation
				
		if(m_pOptions->SetPrinterProperties((HWND)hWnd))
		{
			//	The user may have changed these values via the properties form
			m_strPrinter = m_pOptions->m_strPrinter;
			m_sCopies = m_pOptions->m_sCopies;
			m_bCollate = m_pOptions->m_bCollate;

			return TRUE;
		}
		else
		{
			return FALSE;
		}

	}
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::SetPrintTemplate()
//
// 	Description:	This method is called to set the template for the print job
//
// 	Returns:		TMPRINT_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPrintCtrl::SetPrintTemplate(long lTemplate) 
{
	//	Has the control been intialized?
	if(m_pOptions == 0)
	{
		m_Errors.Handle(0, IDS_TMPRINT_NOTINITIALIZED);
		return TMPRINT_NOTINITIALIZED;
	}
	else
	{
		if(m_pOptions->SetTemplate((CTemplate*)lTemplate))
		{
			//	Update the control properties
			SetProperties(m_pOptions->GetTemplate());
		}
		return TMPRINT_NOERROR;
	}
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::SetPrintTemplates()
//
// 	Description:	This method is called to set the list of templates to be
//					used for print jobs
//
// 	Returns:		TMPRINT_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPrintCtrl::SetPrintTemplates(long lTemplates) 
{
	//	Has the control been intialized?
	if(m_pOptions == 0)
	{
		m_Errors.Handle(0, IDS_TMPRINT_NOTINITIALIZED);
		return TMPRINT_NOTINITIALIZED;
	}
	else
	{
		m_pOptions->SetTemplates((CTemplates*)lTemplates);
		
		
		return TMPRINT_NOERROR;
	}
}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::SetProperties()
//
// 	Description:	This function is called to set the control properties using
//					the specified template
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMPrintCtrl::SetProperties(CTemplate* pTemplate) 
{	
	if(pTemplate != 0)
	{
		m_bAutoRotate = (pTemplate->m_bAutoRotateEnable && pTemplate->m_bAutoRotate);
		m_bPrintCellBorder = pTemplate->m_bDefaultBorder;
		m_bPrintImage = pTemplate->m_bDefaultImage;
		m_strTemplateName = pTemplate->m_strDescription;
		m_bPrintBarcodeGraphic = pTemplate->GetDefault(TEMPLATE_GRAPHIC);
		m_bPrintBarcodeText = pTemplate->GetDefault(TEMPLATE_BARCODE);
		m_bPrintName = pTemplate->GetDefault(TEMPLATE_NAME);
		m_bPrintFileName = pTemplate->GetDefault(TEMPLATE_FILENAME);
		m_bPrintDeponent = pTemplate->GetDefault(TEMPLATE_DEPONENT);
		m_bPrintPageNumber = pTemplate->GetDefault(TEMPLATE_PAGENUM);
		m_bPrintForeignBarcode = pTemplate->GetDefault(TEMPLATE_FOREIGN_BARCODE);
		m_bPrintSourceBarcode = pTemplate->GetDefault(TEMPLATE_SOURCE_BARCODE);
	}
	else
	{
		m_bAutoRotate = TMPRINT_AUTOROTATE;
		m_bIncludePathInFileName = TMPRINT_INCLUDEPATHINFILENAME;
		m_bIncludePageTotal = TMPRINT_INCLUDEPAGETOTAL;
		m_bPrintImage = TMPRINT_PRINTIMAGE;
		m_bPrintBarcodeGraphic = TMPRINT_PRINTBARCODEGRAPHIC;
		m_bPrintBarcodeText = TMPRINT_PRINTBARCODETEXT;
		m_bPrintName = TMPRINT_PRINTNAME;
		m_bPrintFileName = TMPRINT_PRINTFILENAME;
		m_bPrintDeponent = TMPRINT_PRINTDEPONENT;
		m_bPrintPageNumber = TMPRINT_PRINTPAGENUMBER;
		m_bPrintCellBorder = TMPRINT_PRINTCELLBORDER;
		m_bPrintForeignBarcode = TMPRINT_PRINTFOREIGNBARCODE;
		m_bPrintSourceBarcode = TMPRINT_PRINTSOURCEBARCODE;
		m_strTemplateName = TMPRINT_TEMPLATENAME;
	}

}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::SetTextFieldEnabled()
//
// 	Description:	This method enables/disables the specified text field
//					for the print job
//
// 	Returns:		TMPRINT_NOERROR  if successful
//
//	Notes:			None
//
//==============================================================================
short CTMPrintCtrl::SetTextFieldEnabled(long lId, LPCTSTR lpszName, short bEnabled) 
{
	CTemplate*		pTemplate = NULL;
	CTemplateField*	pField = NULL;

	//	Has the control been intialized?
	if(m_pOptions == NULL)
	{
		m_Errors.Handle(0, IDS_TMPRINT_NOTINITIALIZED);
		return TMPRINT_NOTINITIALIZED;
	}
	
	//	Get the specified text field from the active template
	if((pTemplate = m_pOptions->GetTemplate()) != NULL)
		pField = pTemplate->m_aTextFields.Find(lId);

	//	Enable / disable the field
	if(pField != NULL)
	{
		pField->m_bPrint = (bEnabled != 0);
		return TMPRINT_NOERROR;
	}
	else
	{
		m_Errors.Handle(0, IDS_TMPRINT_TEXTFIELDNOTFOUND, lpszName);
		return TMPRINT_TEXTFIELD_NOT_FOUND;
	}

}

//==============================================================================
//
// 	Function Name:	CTMPrintCtrl::ZoomFullWidth()
//
// 	Description:	This external method allows the caller to display the dialog
//					that shows the capabilities of all system printers
//
// 	Returns:		TMPRINT_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMPrintCtrl::ShowPrinterCaps() 
{
	//	Let the container know we are about to open the dialog box
	PreModalDialog();

	CFPrinterCaps printerCaps(this);
	printerCaps.DoModal();

	//	Let the container know we are done
	PostModalDialog();


	return TMPRINT_NOERROR;
}

