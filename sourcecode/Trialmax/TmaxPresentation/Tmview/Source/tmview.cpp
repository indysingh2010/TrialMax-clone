//==============================================================================
//
// File Name:	tmview.cpp
//
// Description:	This file contains member functions of the CTMViewCtrl class.
//
// See Also:	tmview.h
//
// Copyright	FTI Consulting 1997-1999
//
//==============================================================================
//	Date		Revision    Description
//	07-11-97	1.00		Original Release
//	01-09-98	1.10		Added methods for retrieving image width, height
//							and aspect ratio
//	01-09-98	1.10		Added Render() method
//	03-21-98	1.20		Added properties and methods required to pan
//							the image
//	03-22-98	1.20		Added methods and properties for zooming image
//							to full window width or height
//	04-04-98	1.20		Added MousePan and Callout Color properties
//	04-30-98	1.30		Added split screen capabilities
//	01-03-99	3.00		Added ZoomToRect property
//	02-14-2009	6.3.4		Added SaveSplitZap method
//	01-29-2014	7.0.27		Added interface methods DoGesturePan() and
//							DoGestureZoom()
//  03-25-2014	7.0.31		Added interface methods SetZoomedNextPage()
//==============================================================================

//------------------------------------------------------------------------------
//	INCLUDES
//------------------------------------------------------------------------------
#include <stdafx.h>
#include <tmviewap.h>
#include <tmview.h>
#include <tmviewpg.h>
#include <annprops.h>
#include <ltann.h>
#include <callout.h>
#include <regcats.h>
#include <pageset.h>
#include <dispid.h>
#include <diagnose.h>
#include <redact.h>
#include <filever.h>
#include <toolbox.h>
#include <fprintercaps.h>
#include "..\Include\tmview.h"

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

CTMViewCtrl* _pControl;

extern CTMViewApp NEAR	theApp;

/* Replace 2 */
const IID BASED_CODE IID_DTm_view6 =
		{ 0xb6ee6d22, 0x30f7, 0x434b, { 0x96, 0xd2, 0xdf, 0xe4, 0xa9, 0x9e, 0x24, 0x6a } };

/* Replace 3 */
const IID BASED_CODE IID_DTm_view6Events =
		{ 0xa46ec24d, 0x5ee5, 0x45eb, { 0xaa, 0xea, 0x8d, 0xe5, 0xa1, 0xea, 0x8d, 0x22 } };

//	Control type information
static const DWORD BASED_CODE _dwTm_view6OleMisc =
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

BEGIN_MESSAGE_MAP(CTMViewCtrl, COleControl)
	//{{AFX_MSG_MAP(CTMViewCtrl)
	ON_WM_CREATE()
	ON_WM_SIZE()
	ON_WM_PALETTECHANGED()
	ON_WM_QUERYNEWPALETTE()
	ON_WM_LBUTTONDOWN()
	ON_WM_RBUTTONDOWN()
	ON_WM_PARENTNOTIFY()
	ON_WM_SETCURSOR()
	ON_WM_LBUTTONUP()
	ON_WM_RBUTTONUP()
	ON_WM_MOUSEMOVE()
	ON_WM_TIMER()
	//}}AFX_MSG_MAP
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
	ON_MESSAGE(WM_ERROR_EVENT, OnWMErrorEvent)
END_MESSAGE_MAP()

BEGIN_DISPATCH_MAP(CTMViewCtrl, COleControl)
	//{{AFX_DISPATCH_MAP(CTMViewCtrl)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "RedactColor", m_sRedactColor, OnRedactColorChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "HighlightColor", m_sHighlightColor, OnHighlightColorChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "AnnColor", m_sAnnColor, OnAnnColorChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "AnnTool", m_sAnnTool, OnAnnToolChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "AnnThickness", m_sAnnThickness, OnAnnThicknessChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "ScaleImage", m_bScaleImage, OnScaleImageChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "Rotation", m_sRotation, OnRotationChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "MaxZoom", m_sMaxZoom, OnMaxZoomChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "FitToImage", m_bFitToImage, OnFitToImageChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "Action", m_sAction, OnActionChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "EnableErrors", m_bEnableErrors, OnEnableErrorsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "BackgroundFile", m_strBackgroundFile, OnBackgroundFileChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "AutoAnimate", m_bAutoAnimate, OnAutoAnimateChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "LoopAnimation", m_bLoopAnimation, OnLoopAnimationChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "HideScrollBars", m_bHideScrollBars, OnHideScrollBarsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "PanPercent", m_sPanPercent, OnPanPercentChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "ZoomOnLoad", m_sZoomOnLoad, OnZoomOnLoadChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "CalloutColor", m_sCalloutColor, OnCalloutColorChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "CalloutFrameThickness", m_sCalloutFrameThickness, OnCalloutFrameThicknessChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "CalloutFrameColor", m_sCalloutFrameColor, OnCalloutFrameColorChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "RightClickPan", m_bRightClickPan, OnRightClickPanChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "SplitFrameThickness", m_sSplitFrameThickness, OnSplitFrameThicknessChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "SplitFrameColor", m_sSplitFrameColor, OnSplitFrameColorChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "SplitScreen", m_bSplitScreen, OnSplitScreenChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "ActivePane", m_sActivePane, OnActivePaneChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "SyncPanes", m_bSyncPanes, OnSyncPanesChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "LeftFile", m_strLeftFile, OnLeftFileChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "RightFile", m_strRightFile, OnRightFileChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "SyncCalloutAnn", m_bSyncCalloutAnn, OnSyncCalloutAnnChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "PenSelectorVisible", m_bPenSelectorVisible, OnPenSelectorVisibleChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "PenSelectorColor", m_sPenSelectorColor, OnPenSelectorColorChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "PenSelectorSize", m_sPenSelectorSize, OnPenSelectorSizeChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "KeepAspect", m_bKeepAspect, OnKeepAspectChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "BitonalScaling", m_sBitonal, OnBitonalScalingChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "ZoomToRect", m_bZoomToRect, OnZoomToRectChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "AnnFontName", m_strAnnFontName, OnAnnFontNameChanged, VT_BSTR)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "AnnFontSize", m_sAnnFontSize, OnAnnFontSizeChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "AnnFontBold", m_bAnnFontBold, OnAnnFontBoldChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "AnnFontStrikeThrough", m_bAnnFontStrikeThrough, OnAnnFontStrikeThroughChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "AnnFontUnderline", m_bAnnFontUnderline, OnAnnFontUnderlineChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "DeskewBackColor", m_lDeskewBackColor, OnDeskewBackColorChanged, VT_COLOR)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "AnnotateCallouts", m_bAnnotateCallouts, OnAnnotateCalloutsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "PrintOrientation", m_sPrintOrientation, OnPrintOrientationChanged, VT_I2)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "PrintBorderColor", m_lPrintBorderColor, OnPrintBorderColorChanged, VT_COLOR)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "PrintLeftMargin", m_fPrintLeftMargin, OnPrintLeftMarginChanged, VT_R4)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "PrintRightMargin", m_fPrintRightMargin, OnPrintRightMarginChanged, VT_R4)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "PrintTopMargin", m_fPrintTopMargin, OnPrintTopMarginChanged, VT_R4)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "PrintBottomMargin", m_fPrintBottomMargin, OnPrintBottomMarginChanged, VT_R4)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "PrintBorderThickness", m_fPrintBorderThickness, OnPrintBorderThicknessChanged, VT_R4)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "PrintCallouts", m_bPrintCallouts, OnPrintCalloutsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "PrintBorder", m_bPrintBorder, OnPrintBorderChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY(CTMViewCtrl, "PrintCalloutBorders", m_bPrintCalloutBorders, OnPrintCalloutBordersChanged, VT_BOOL)
	DISP_PROPERTY_EX(CTMViewCtrl, "VerBuild", GetVerBuild, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX(CTMViewCtrl, "VerTextLong", GetVerTextLong, SetNotSupported, VT_BSTR)
	DISP_FUNCTION(CTMViewCtrl, "Redraw", Redraw, VT_EMPTY, VTS_NONE)
	DISP_FUNCTION(CTMViewCtrl, "SetAnnotationProperties", SetAnnotationProperties, VT_I2, VTS_I4)
	DISP_FUNCTION(CTMViewCtrl, "SetPrinter", SetPrinter, VT_EMPTY, VTS_NONE)
	DISP_FUNCTION(CTMViewCtrl, "LoadFile", LoadFile, VT_I2, VTS_BSTR VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "Print", Print, VT_I2, VTS_BOOL VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "DestroyCallouts", DestroyCallouts, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "Erase", Erase, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "GetAspectRatio", GetAspectRatio, VT_R4, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "GetCurrentPage", GetCurrentPage, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "GetImageHeight", GetImageHeight, VT_R4, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "GetImageWidth", GetImageWidth, VT_R4, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "GetPageCount", GetPageCount, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "GetPanStates", GetPanStates, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "GetSrcRatio", GetSrcRatio, VT_R4, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "GetZoomFactor", GetZoomFactor, VT_R4, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "GetZoomState", GetZoomState, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "IsAnimation", IsAnimation, VT_BOOL, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "IsLoaded", IsLoaded, VT_BOOL, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "IsPlaying", IsPlaying, VT_BOOL, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "NextPage", NextPage, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "PlayAnimation", PlayAnimation, VT_I2, VTS_BOOL VTS_BOOL VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "PrevPage", PrevPage, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "Realize", Realize, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "Pan", Pan, VT_BOOL, VTS_I2 VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "Render", Render, VT_I2, VTS_HANDLE VTS_R4 VTS_R4 VTS_R4 VTS_R4 VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "ResetZoom", ResetZoom, VT_EMPTY, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "ResizeSourceToImage", ResizeSourceToImage, VT_EMPTY, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "ResizeSourceToView", ResizeSourceToView, VT_EMPTY, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "Rotate", Rotate, VT_EMPTY, VTS_BOOL VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "RotateCcw", RotateCcw, VT_EMPTY, VTS_BOOL VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "RotateCw", RotateCw, VT_EMPTY, VTS_BOOL VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "Save", Save, VT_I2, VTS_BSTR VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "SaveZap", SaveZap, VT_I2, VTS_BSTR VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "ShowCallouts", ShowCallouts, VT_I2, VTS_BOOL VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "ZoomFullHeight", ZoomFullHeight, VT_EMPTY, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "ZoomFullWidth", ZoomFullWidth, VT_EMPTY, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "FirstPage", FirstPage, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "LastPage", LastPage, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "GetColor", GetColor, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMViewCtrl, "SetColor", SetColor, VT_EMPTY, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "SetData", SetData, VT_EMPTY, VTS_I2 VTS_I4)
	DISP_FUNCTION(CTMViewCtrl, "GetData", GetData, VT_I4, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "GetSelectCount", GetSelectCount, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "DeleteSelections", DeleteSelections, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "DeleteLastAnn", DeleteLastAnn, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "LoadZap", LoadZap, VT_I2, VTS_BSTR VTS_BOOL VTS_BOOL VTS_BOOL VTS_I2 VTS_BSTR)
	DISP_FUNCTION(CTMViewCtrl, "Copy", Copy, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "Paste", Paste, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "GetRGBColor", GetRGBColor, VT_I4, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "ViewImageProperties", ViewImageProperties, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "SetPrinterByName", SetPrinterByName, VT_BOOL, VTS_BSTR)
	DISP_FUNCTION(CTMViewCtrl, "GetDefaultPrinter", GetDefaultPrinter, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMViewCtrl, "CanPrint", CanPrint, VT_BOOL, VTS_NONE)
	DISP_FUNCTION(CTMViewCtrl, "GetCurrentPrinter", GetCurrentPrinter, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMViewCtrl, "GetImageProperties", GetImageProperties, VT_I2, VTS_I4 VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "Deskew", Deskew, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "GetRegisteredPath", GetRegisteredPath, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMViewCtrl, "GetClassIdString", GetClassIdString, VT_BSTR, VTS_NONE)
	DISP_FUNCTION(CTMViewCtrl, "Despeckle", Despeckle, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "GetLeadToolError", GetLeadToolError, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "DotRemove", DotRemove, VT_I2, VTS_I2 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION(CTMViewCtrl, "HolePunchRemove", HolePunchRemove, VT_I2, VTS_I2 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION(CTMViewCtrl, "Smooth", Smooth, VT_I2, VTS_I2 VTS_I4 VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "BorderRemove", BorderRemove, VT_I2, VTS_I2 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION(CTMViewCtrl, "Cleanup", Cleanup, VT_I2, VTS_I2 VTS_BSTR)
	DISP_FUNCTION(CTMViewCtrl, "SetupPrintPage", SetupPrintPage, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMViewCtrl, "PrintEx", PrintEx, VT_I2, VTS_HANDLE VTS_I2 VTS_I2 VTS_I2 VTS_I2 VTS_I2 VTS_I2 VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "RescaleZapCallouts", RescaleZapCallouts, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMViewCtrl, "ShowDiagnostics", ShowDiagnostics, VT_I2, VTS_BOOL)
	DISP_FUNCTION(CTMViewCtrl, "DrawSourceRectangle", DrawSourceRectangle, VT_I4, VTS_I2 VTS_I2 VTS_I2 VTS_I2 VTS_COLOR VTS_I2 VTS_BOOL VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "DeleteAnn", DeleteAnn, VT_I2, VTS_I4 VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "DrawText", DrawText, VT_I4, VTS_BSTR VTS_I2 VTS_I2 VTS_I2 VTS_I2 VTS_COLOR VTS_BSTR VTS_I2 VTS_BOOL VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "DrawSourceText", DrawSourceText, VT_I4, VTS_BSTR VTS_I2 VTS_I2 VTS_I2 VTS_I2 VTS_COLOR VTS_BSTR VTS_I2 VTS_BOOL VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "DrawTmaxRedaction", DrawTmaxRedaction, VT_I2, VTS_I4 VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "DrawTmaxRedactions", DrawTmaxRedactions, VT_I2, VTS_I4 VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "DeleteTmaxRedaction", DeleteTmaxRedaction, VT_I2, VTS_I4 VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "DeleteTmaxRedactions", DeleteTmaxRedactions, VT_I2, VTS_I4 VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "DrawRectangle", DrawRectangle, VT_I4, VTS_I2 VTS_I2 VTS_I2 VTS_I2 VTS_COLOR VTS_I2 VTS_BOOL VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "LockAnn", LockAnn, VT_I2, VTS_I4 VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "UnlockAnn", UnlockAnn, VT_I2, VTS_I4 VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "GetOleColor", GetOleColor, VT_COLOR, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "GetCalloutCount", GetCalloutCount, VT_I2, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "SetPrinterProperties", SetPrinterProperties, VT_BOOL, VTS_HANDLE)
	DISP_FUNCTION(CTMViewCtrl, "SetProperties", SetProperties, VT_I2, VTS_BSTR VTS_BSTR)
	DISP_FUNCTION(CTMViewCtrl, "SaveProperties", SaveProperties, VT_I2, VTS_BSTR VTS_BSTR)
	DISP_FUNCTION(CTMViewCtrl, "HolePunchRemove2", HolePunchRemove2, VT_I2, VTS_I2 VTS_I4 VTS_I4 VTS_I4)
	DISP_FUNCTION(CTMViewCtrl, "SavePages", SavePages, VT_I2, VTS_BSTR VTS_BSTR VTS_BSTR)
	DISP_FUNCTION(CTMViewCtrl, "ShowPrinterCaps", ShowPrinterCaps, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMViewCtrl, "EnableDIBPrinting", EnableDIBPrinting, VT_EMPTY, VTS_I2)
	DISP_FUNCTION(CTMViewCtrl, "SwapPanes", SwapPanes, VT_I2, VTS_NONE)
	DISP_FUNCTION(CTMViewCtrl, "SaveSplitZap", SaveSplitZap, VT_I2, VTS_BSTR VTS_BSTR)
	DISP_STOCKPROP_BACKCOLOR()
	DISP_STOCKPROP_BORDERSTYLE()
	DISP_STOCKPROP_ENABLED()
	//}}AFX_DISPATCH_MAP
	DISP_FUNCTION_ID(CTMViewCtrl, "AboutBox", DISPID_ABOUTBOX, AboutBox, VT_EMPTY, VTS_NONE)

	//	Added rev 5.1
	DISP_PROPERTY_NOTIFY_ID(CTMViewCtrl, "AnnColorDepth", DISPID_ANNCOLORDEPTH, m_sAnnColorDepth, OnAnnColorDepthChanged, VT_I2)

	//	Added rev 5.2
	DISP_PROPERTY_NOTIFY_ID(CTMViewCtrl, "SplitHorizontal", DISPID_SPLITHORIZONTAL, m_bSplitHorizontal, OnSplitHorizontalChanged, VT_BOOL)

	//	Added rev 5.3
	DISP_PROPERTY_NOTIFY_ID(CTMViewCtrl, "QFactor", DISPID_QFACTOR, m_sQFactor, OnQFactorChanged, VT_I2)

	//	Added rev 5.4
	DISP_PROPERTY_NOTIFY_ID(CTMViewCtrl, "ResizeCallouts", DISPID_RESIZECALLOUTS, m_bResizeCallouts, OnResizeCalloutsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY_ID(CTMViewCtrl, "CalloutHandleColor", DISPID_CALLOUTHANDLECOLOR, m_sCalloutHandleColor, OnCalloutHandleColorChanged, VT_I2)

	//	Added rev 5.7
	DISP_PROPERTY_NOTIFY_ID(CTMViewCtrl, "ShadeOnCallout", DISPID_SHADEONCALLOUT, m_bShadeOnCallout, OnShadeOnCalloutChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY_ID(CTMViewCtrl, "CalloutShadeGrayscale", DISPID_CALLOUTSHADEGRAYSCALE, m_sCalloutShadeGrayscale, OnCalloutShadeGrayscaleChanged, VT_I2)

	//	Added rev 5.8
	DISP_PROPERTY_NOTIFY_ID(CTMViewCtrl, "PanCallouts", DISPID_PANCALLOUTS, m_bPanCallouts, OnPanCalloutsChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY_ID(CTMViewCtrl, "ZoomCallouts", DISPID_ZOOMCALLOUTS, m_bZoomCallouts, OnZoomCalloutsChanged, VT_BOOL)

	//	Added rev 6.0
	DISP_PROPERTY_NOTIFY_ID(CTMViewCtrl, "LoadAsync", DISPID_LOADASYNC, m_bLoadAsync, OnLoadAsyncChanged, VT_BOOL)
	DISP_PROPERTY_NOTIFY_ID(CTMViewCtrl, "EnableAxErrors", DISPID_ENABLEAXERRORS, m_bEnableAxErrors, OnEnableAxErrorsChanged, VT_BOOL)

	//	Added rev 6.1.0
	DISP_PROPERTY_EX_ID(CTMViewCtrl, "VerTextShort", DISPID_VERTEXTSHORT, GetVerTextShort, SetNotSupported, VT_BSTR)
	DISP_PROPERTY_EX_ID(CTMViewCtrl, "VerMajor", DISPID_VERMAJOR, GetVerMajor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX_ID(CTMViewCtrl, "VerMinor", DISPID_VERMINOR, GetVerMinor, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX_ID(CTMViewCtrl, "VerQEF", DISPID_VERQEF, GetVerQEF, SetNotSupported, VT_I2)
	DISP_PROPERTY_EX_ID(CTMViewCtrl, "VerBuildDate", DISPID_VERBUILDDATE, GetVerBuildDate, SetNotSupported, VT_BSTR)

	DISP_FUNCTION_ID(CTMViewCtrl, "DoGesturePan", dispidDoGesturePan, DoGesturePan, VT_BOOL, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_BOOL)
	DISP_FUNCTION_ID(CTMViewCtrl, "DoGestureZoom", dispidDoGestureZoom, DoGestureZoom, VT_EMPTY, VTS_R4)
	DISP_FUNCTION_ID(CTMViewCtrl, "SetZoomedNextPage", dispidZoomedNextPage, SetZoomedNextPage, VT_EMPTY, VTS_BOOL)
	END_DISPATCH_MAP()

BEGIN_EVENT_MAP(CTMViewCtrl, COleControl)
	//{{AFX_EVENT_MAP(CTMViewCtrl)
	EVENT_CUSTOM("MouseClick", FireMouseClick, VTS_I2  VTS_I2)
	EVENT_CUSTOM("MouseDblClick", FireMouseDblClick, VTS_I2  VTS_I2)
	EVENT_CUSTOM("CreateCallout", FireCreateCallout, VTS_HANDLE)
	EVENT_CUSTOM("DestroyCallout", FireDestroyCallout, VTS_HANDLE)
	EVENT_CUSTOM("SelectPane", FireSelectPane, VTS_I2)
	EVENT_CUSTOM("OpenTextBox", FireOpenTextBox, VTS_I2)
	EVENT_CUSTOM("CloseTextBox", FireCloseTextBox, VTS_I2)
	EVENT_CUSTOM("SelectCallout", FireSelectCallout, VTS_HANDLE  VTS_I2)
	EVENT_CUSTOM("StartTextEdit", FireStartTextEdit, VTS_I2)
	EVENT_CUSTOM("StopTextEdit", FireStopTextEdit, VTS_I2)
	EVENT_CUSTOM("AnnotationDeleted", FireAnnotationDeleted, VTS_I4  VTS_I2)
	EVENT_CUSTOM("AnnotationModified", FireAnnotationModified, VTS_I4  VTS_I2)
	EVENT_CUSTOM("AnnotationDrawn", FireAnnotationDrawn, VTS_I4  VTS_I2)
	EVENT_CUSTOM("CalloutResized", FireCalloutResized, VTS_HANDLE  VTS_I2)
	EVENT_CUSTOM("CalloutMoved", FireCalloutMoved, VTS_HANDLE  VTS_I2)
	EVENT_CUSTOM("AxError", FireAxError, VTS_BSTR)
	EVENT_CUSTOM("AxDiagnostic", FireAxDiagnostic, VTS_BSTR  VTS_BSTR)
	EVENT_CUSTOM("SavedPage", FireSavedPage, VTS_BSTR  VTS_BSTR  VTS_I2  VTS_I2)
	EVENT_STOCK_MOUSEDOWN()
	EVENT_STOCK_MOUSEUP()
	EVENT_CUSTOM_ID("MouseMove", DISPID_MOUSEMOVE, FireMouseMove, VTS_I2  VTS_I2  VTS_XPOS_PIXELS  VTS_YPOS_PIXELS)
	//}}AFX_EVENT_MAP
END_EVENT_MAP()

BEGIN_EVENTSINK_MAP(CTMViewCtrl, COleControl)
    //{{AFX_EVENTSINK_MAP(CTMViewCtrl)
	ON_EVENT(CTMViewCtrl, IDC_PANEA, 14 /* Animate */, OnAAnimate, VTS_BOOL)
	ON_EVENT(CTMViewCtrl, IDC_PANEA, 12 /* AnnChange */, OnAAnnChange, VTS_I4 VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEA, 9 /* AnnClicked */, OnAAnnClicked, VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEA, 7 /* AnnCreate */, OnAAnnCreate, VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEA, 8 /* AnnDestroy */, OnAAnnDestroy, VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEA, 10 /* AnnDrawn */, OnAAnnDrawn, VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEA, 31 /* AnnMenu */, OnAAnnMenu, VTS_DISPATCH)
	ON_EVENT(CTMViewCtrl, IDC_PANEA, 28 /* AnnMouseDown */, OnAAnnMouseDown, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEA, 29 /* AnnMouseMove */, OnAAnnMouseMove, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEA, 30 /* AnnMouseUp */, OnAAnnMouseUp, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEA, 16 /* AnnSelect */, OnAAnnSelect, VTS_VARIANT VTS_I2)
	ON_EVENT(CTMViewCtrl, IDC_PANEA, 38 /* AnnUserMenu */, OnAAnnUserMenu, VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEA, DISPID_CLICK/* Click */, OnAMouseClick, VTS_NONE)
	ON_EVENT(CTMViewCtrl, IDC_PANEA, DISPID_DBLCLICK /* DblClick */, OnAMouseDblClick, VTS_NONE)
	ON_EVENT(CTMViewCtrl, IDC_PANEA, DISPID_KEYDOWN /* KeyDown */, OnAKeyDown, VTS_PI2 VTS_I2)
	ON_EVENT(CTMViewCtrl, IDC_PANEA, DISPID_KEYPRESS /* KeyPress */, OnAKeyPress, VTS_PI2)
	ON_EVENT(CTMViewCtrl, IDC_PANEA, DISPID_KEYUP /* KeyUp */, OnAKeyUp, VTS_PI2 VTS_I2)
	ON_EVENT(CTMViewCtrl, IDC_PANEA, DISPID_MOUSEDOWN /* MouseDown */, OnAMouseDown, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEA, DISPID_MOUSEMOVE /* MouseMove */, OnAMouseMove, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEA, DISPID_MOUSEUP /* MouseUp */, OnAMouseUp, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEA, 15/* RubberBand */, OnARubberBand, VTS_NONE)
	ON_EVENT(CTMViewCtrl, IDC_PANEB, 14 /* Animate */, OnBAnimate, VTS_BOOL)
	ON_EVENT(CTMViewCtrl, IDC_PANEB, 12 /* AnnChange */, OnBAnnChange, VTS_I4 VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEB, 9 /* AnnClicked */, OnBAnnClicked, VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEB, 7 /* AnnCreate */, OnBAnnCreate, VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEB, 8 /* AnnDestroy */, OnBAnnDestroy, VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEB, 10 /* AnnDrawn */, OnBAnnDrawn, VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEB, 31 /* AnnMenu */, OnBAnnMenu, VTS_DISPATCH)
	ON_EVENT(CTMViewCtrl, IDC_PANEB, 28 /* AnnMouseDown */, OnBAnnMouseDown, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEB, 29 /* AnnMouseMove */, OnBAnnMouseMove, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEB, 30 /* AnnMouseUp */, OnBAnnMouseUp, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEB, 16 /* AnnSelect */, OnBAnnSelect, VTS_VARIANT VTS_I2)
	ON_EVENT(CTMViewCtrl, IDC_PANEB, 38 /* AnnUserMenu */, OnBAnnUserMenu, VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEB, DISPID_CLICK/* Click */, OnBMouseClick, VTS_NONE)
	ON_EVENT(CTMViewCtrl, IDC_PANEB, DISPID_DBLCLICK /* DblClick */, OnBMouseDblClick, VTS_NONE)
	ON_EVENT(CTMViewCtrl, IDC_PANEB, DISPID_KEYDOWN /* KeyDown */, OnBKeyDown, VTS_PI2 VTS_I2)
	ON_EVENT(CTMViewCtrl, IDC_PANEB, DISPID_KEYPRESS /* KeyPress */, OnBKeyPress, VTS_PI2)
	ON_EVENT(CTMViewCtrl, IDC_PANEB, DISPID_KEYUP /* KeyUp */, OnBKeyUp, VTS_PI2 VTS_I2)
	ON_EVENT(CTMViewCtrl, IDC_PANEB, DISPID_MOUSEDOWN /* MouseDown */, OnBMouseDown, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEB, DISPID_MOUSEMOVE /* MouseMove */, OnBMouseMove, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEB, DISPID_MOUSEUP /* MouseUp */, OnBMouseUp, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
	ON_EVENT(CTMViewCtrl, IDC_PANEB, 15/* RubberBand */, OnBRubberBand, VTS_NONE)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()


/*--------------------------------------------------------------------------------------------------------------------------
//  ADDITIONAL LEAD TOOLS v16.5 EVENTS
//--------------------------------------------------------------------------------------------------------------------------
ON_EVENT(CTMViewCtrl, IDC_PANEA, 47, OnAnimationFrame, VTS_I2)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 25, OnAnnEnumerate, VTS_I4)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 20, OnAnnHyperlink, VTS_I4 VTS_I2 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 21, OnAnnHyperlinkMenu, VTS_VARIANT VTS_I2)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 18, OnAnnLocked, VTS_I4)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 26, OnAnnToolChecked, VTS_I2)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 27, OnAnnToolDestroy, VTS_NONE)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 19, OnAnnUnlocked, VTS_I4)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 46, OnBitmapDataPathClosed, VTS_NONE)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 34, OnBorderRemoveEvent, VTS_I4 VTS_I4 VTS_R4 VTS_R4 VTS_R4 VTS_R4)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 5, OnChange, VTS_NONE)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 40, OnCustomCompressedData, VTS_I4)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 41, OnCustomUnCompressedData, VTS_VARIANT VTS_I4 VTS_R4 VTS_R4 VTS_I2)	
ON_EVENT(CTMViewCtrl, IDC_PANEA, 36, OnDotRemoveEvent, VTS_I4 VTS_R4 VTS_R4 VTS_R4 VTS_R4 VTS_I4 VTS_I4)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 13, OnFilePage, VTS_NONE)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 45, OnFilePageLoaded, VTS_NONE)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 37, OnHolePunchRemoveEvent, VTS_I4 VTS_R4 VTS_R4 VTS_R4 VTS_R4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 35, OnInvertedTextEvent, VTS_I4 VTS_R4 VTS_R4 VTS_R4 VTS_R4 VTS_I4 VTS_I4)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 33, OnLineRemoveEvent, VTS_I4 VTS_I4 VTS_I4 VTS_I4)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 11, OnLoadInfo, VTS_NONE)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 39, OnMagGlass, VTS_I4 VTS_I4 VTS_VARIANT)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 44, OnMagGlassCursor, VTS_NONE)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 48, OnMouseWheel, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 50, OnOLECompleteDrag, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 51, OnOLEDragOver, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 24, OnOLEDropFile, VTS_BSTR)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 52, OnOLEGiveFeedback, VTS_PBOOL)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 49, OnOLEStartDrag, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 1, OnPaint, VTS_NONE)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 17, OnPaintNotification, VTS_I2 VTS_I2)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 23, OnPanWin, VTS_I4 VTS_I2)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 4, OnProgressStatus, VTS_I2)
ON_EVENT(CTMViewCtrl, IDC_PANEA, DISPID_READYSTATECHANGE, OnReadyStateChange, VTS_I4)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 2, OnResize, VTS_NONE)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 6, OnRgnChange, VTS_NONE)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 43, OnSaveBuffer, VTS_I4)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 3, OnScroll, VTS_NONE)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 32, OnSmoothEvent, VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 42, OnTransformMarker, VTS_I2 VTS_I4 VTS_VARIANT VTS_I2)
ON_EVENT(CTMViewCtrl, IDC_PANEA, 22, OnZoomInDone, VTS_I4 VTS_I4)
---------------------------------------------------------------------------------------------------------------*/

BEGIN_PROPPAGEIDS(CTMViewCtrl, 2)
	PROPPAGEID(CTMViewProperties::guid)
	PROPPAGEID(CLSID_CColorPropPage)
END_PROPPAGEIDS(CTMViewCtrl)

// Initialize class factory and guid
/* Replace 4 */
IMPLEMENT_OLECREATE_EX(CTMViewCtrl, "TMVIEW6.TmviewCtrl.1",
	0x5a3a9fc9, 0xd747, 0x4b92, 0x91, 0x6, 0xa3, 0x2c, 0x7e, 0x6e, 0x84, 0xa3)

// Type library ID and version
IMPLEMENT_OLETYPELIB(CTMViewCtrl, _tlid, _wVerMajor, _wVerMinor)

IMPLEMENT_OLECTLTYPE(CTMViewCtrl, IDS_TM_VIEW6, _dwTm_view6OleMisc)

IMPLEMENT_DYNCREATE(CTMViewCtrl, COleControl)

// Interface map for IObjectSafety
BEGIN_INTERFACE_MAP(CTMViewCtrl, COleControl )
	INTERFACE_PART(CTMViewCtrl, IID_IObjectSafety, ObjSafety)
END_INTERFACE_MAP()

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::CTMViewCtrlFactory::UpdateRegistry
//
// 	Description:	Adds or removes system registry entries for this control.
//
// 	Returns:		TRUE if successful.
//
//	Notes:			None
//
//==============================================================================
BOOL CTMViewCtrl::CTMViewCtrlFactory::UpdateRegistry(BOOL bRegister)
{
	BOOL	bReturn;
	HRESULT	hResult;

	if(bRegister)
	{
		bReturn = AfxOleRegisterControlClass(AfxGetInstanceHandle(),
											 m_clsid,
											 m_lpszProgID,
											 IDS_TM_VIEW6,
											 IDB_TM_VIEW6,
											 afxRegApartmentThreading,
											 _dwTm_view6OleMisc,
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
// 	Function Name:	CTMViewCtrl::XObjSafety::AddRef()
//
// 	Description:	This function is called to attach to the nested
//					IObjectSafety dispatch interface.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMViewCtrl::XObjSafety::AddRef()
{
    METHOD_PROLOGUE(CTMViewCtrl, ObjSafety)
    return pThis->ExternalAddRef();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::XObjSafety::GetInterfaceSafetyOptions()
//
// 	Description:	This function is called to get this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMViewCtrl::XObjSafety::GetInterfaceSafetyOptions( 
		/* [in]  */ REFIID riid,
        /* [out] */ DWORD __RPC_FAR *pdwSupportedOptions,
        /* [out] */ DWORD __RPC_FAR *pdwEnabledOptions)
{
	HRESULT			hReturn;
	IUnknown FAR*	pInterface;

	METHOD_PROLOGUE(CTMViewCtrl, ObjSafety)

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
// 	Function Name:	CTMViewCtrl::XObjSafety::QueryInterface()
//
// 	Description:	This function is called to query the IObjectSafety interface
//					for the requested method/property.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT FAR EXPORT CTMViewCtrl::XObjSafety::QueryInterface(REFIID iid, 
														  void FAR* FAR* ppvObj)
{
    METHOD_PROLOGUE(CTMViewCtrl, ObjSafety)
    return (HRESULT)pThis->ExternalQueryInterface(&iid, ppvObj);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::XObjSafety::Release()
//
// 	Description:	This function is called to detach from the IObjectSafety
//					dispatch interface.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
ULONG FAR EXPORT CTMViewCtrl::XObjSafety::Release()
{
    METHOD_PROLOGUE(CTMViewCtrl, ObjSafety)
    return pThis->ExternalRelease();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::XObjSafety::SetInterfaceSafetyOptions()
//
// 	Description:	This function is called to set this control's safety 
//					options.
//
// 	Returns:		S_OK if successful
//
//	Notes:			None
//
//==============================================================================
HRESULT STDMETHODCALLTYPE CTMViewCtrl::XObjSafety::SetInterfaceSafetyOptions( 
        /* [in] */ REFIID riid,
        /* [in] */ DWORD dwOptionSetMask,
        /* [in] */ DWORD dwEnabledOptions)
{
	IUnknown FAR* pInterface;

    METHOD_PROLOGUE(CTMViewCtrl, ObjSafety)
	
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
// 	Function Name:	CTMViewCtrl::AboutBox()
//
// 	Description:	This function will display the control's about box.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::AboutBox()
{
	CDialog dlgAbout(IDD_ABOUTBOX_TMVIEW, this);
	dlgAbout.DoModal();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::BorderRemove()
//
// 	Description:	This external method allows the caller to remove the 
//					boarders from 1-bit images.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::BorderRemove(short sPane, long lBorderPercent, 
								long lWhiteNoise, long lVariance, 
								long lLocation) 
{
	return GetPane(sPane)->BorderRemove(lBorderPercent, lWhiteNoise,
										lVariance, lLocation);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::CanPrint()
//
// 	Description:	This external method allows the caller to check to see if
//					the control is capable of printing.
//
// 	Returns:		TRUE if capable of printing
//
//	Notes:			None
//
//==============================================================================
BOOL CTMViewCtrl::CanPrint() 
{
	//	Just assume there is a default printer
	//
	//	NOTE:	We've left this in place for backward compatability but we no longer
	//			attach to the printer until the user actually attempts a print job
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::CheckVersion()
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
BOOL CTMViewCtrl::CheckVersion(DWORD dwVersion)
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
	              "TMView ActiveX control. You should upgrade tm_view6.ocx "
				  "as soon as possible\n\n%s", 
				  (wMinor > _wVerMinor) ? "a newer" : "an older", strVersion);

	MessageBeep(MB_ICONEXCLAMATION);
	MessageBox(strMsg, "TrialMax Error", MB_ICONEXCLAMATION | MB_OK);

	//	The versions do not match
	return FALSE;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::Cleanup()
//
// 	Description:	This external method displays a dialog box that allows the
//					user to clean up a scanned image.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::Cleanup(short sPane, LPCTSTR lpszSaveAs) 
{
	return GetPane(sPane)->Cleanup(lpszSaveAs);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::Copy()
//
// 	Description:	This external method allows the caller to copy the contents
//					of the specified pane to the clipboard as a device
//					independent bitmap
//
// 	Returns:		TMV_NOERROR if successful. 
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::Copy(short sPane) 
{
	return GetPane(sPane)->Copy();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::CTMViewCtrl()
//
// 	Description:	This is the control constructor.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMViewCtrl::CTMViewCtrl()
{
	//	Initialize the dispatch interfaces first
	InitializeIIDs(&IID_DTm_view6, &IID_DTm_view6Events);

	//	Set the global reference
	_pControl = this;

	//	Initialize the local members
	m_pCallout   = 0;
	m_pLeft      = &m_PaneA;
	m_pRight     = &m_PaneB;
	m_pActive	 = m_pLeft;
	m_iWidth     = 0;
	m_iHeight    = 0;
	m_iTextOpen  = 0;
	m_bRedraw	 = TRUE;
	m_bParentNotify = FALSE;
	m_bSetCursor    = FALSE;
	m_bPenTop = TRUE;
	m_bSplitHorizontal = FALSE;
	m_bEditTextAnn = FALSE;
	m_bLoadingZap = FALSE;
	memset(&m_rcLPane, 0, sizeof(RECT));
	memset(&m_rcRPane, 0, sizeof(RECT));
	memset(&m_rcLFrame, 0, sizeof(RECT));
	memset(&m_rcRFrame, 0, sizeof(RECT));
	memset(&m_rcMax, 0, sizeof(RECT));

	//	Get the registry information
	GetRegistration();


	HCURSOR plusCur = LoadCursor(AfxGetInstanceHandle(), MAKEINTRESOURCE(IDC_PLUS));
	L_AnnSetAutoCursor(NULL, ANNAUTOCURSOR_TOOL_RECT_CURSOR, plusCur);
	L_AnnSetAutoCursor(NULL, ANNAUTOCURSOR_TOOL_ELLIPSE_CURSOR, plusCur);
	L_AnnSetAutoCursor(NULL, ANNAUTOCURSOR_TOOL_LINE_CURSOR, plusCur);
	L_AnnSetAutoCursor(NULL, ANNAUTOCURSOR_TOOL_POLYLINE_CURSOR, plusCur);
	L_AnnSetAutoCursor(NULL, ANNAUTOCURSOR_TOOL_POLYGON_CURSOR, plusCur);
	L_AnnSetAutoCursor(NULL, ANNAUTOCURSOR_TOOL_TEXT_CURSOR, plusCur);
	L_AnnSetAutoCursor(NULL, ANNAUTOCURSOR_TOOL_FREEHAND_CURSOR, plusCur);
	L_AnnSetAutoCursor(NULL, ANNAUTOCURSOR_TOOL_POINTER_CURSOR, plusCur);
	L_AnnSetAutoCursor(NULL, ANNAUTOCURSOR_TOOL_HILITE_CURSOR, plusCur);
	L_AnnSetAutoCursor(NULL, ANNAUTOCURSOR_TOOL_REDACT_CURSOR, plusCur);

}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::~CTMViewCtrl()
//
// 	Description:	This is the control destructor.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
CTMViewCtrl::~CTMViewCtrl()
{
	_pControl = 0;

	//	Flush all callouts
	m_Callouts.Flush(TRUE);
	m_pCallout = 0;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::DeleteAnn()
//
// 	Description:	This method allows the caller to delete the specified
//					annotation.
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			lAnnotation should be a handle returned by one of this
//					control's drawing methods.
//
//==============================================================================
short CTMViewCtrl::DeleteAnn(long lAnnotation, short sPane) 
{
	return GetPane(sPane)->DeleteAnn((HANNOBJECT)lAnnotation, FALSE);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::DeleteLastAnn()
//
// 	Description:	This method allows the caller to delete the last annotation
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::DeleteLastAnn(short sPane) 
{
	return GetPane(sPane)->DeleteLastAnn();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::DeleteSelections()
//
// 	Description:	This method allows the caller to delete the current 
//					selections
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::DeleteSelections(short sPane) 
{
	return GetPane(sPane)->DeleteSelections();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::DeleteTmaxRedaction()
//
// 	Description:	This external method allows the caller to delete the
//					annotations associated with the specified redaction object.
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::DeleteTmaxRedaction(long pRedaction, short sPane) 
{
	CRedaction* ptmaxRedaction = (CRedaction*)pRedaction;

	if(ptmaxRedaction != 0)
	{
		if(ptmaxRedaction->m_lAnnRedaction != 0)
		{
			GetPane(sPane)->DeleteAnn((HANNOBJECT)ptmaxRedaction->m_lAnnRedaction, TRUE);
			ptmaxRedaction->m_lAnnRedaction = 0;
		}

		if(ptmaxRedaction->m_lAnnLabel != 0)
		{
			GetPane(sPane)->DeleteAnn((HANNOBJECT)ptmaxRedaction->m_lAnnLabel, TRUE);
			ptmaxRedaction->m_lAnnLabel = 0;
		}

	}

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::DeleteTmaxRedactions()
//
// 	Description:	This function delete the redactions in the specified list
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::DeleteTmaxRedactions(long paRedactions, short sPane) 
{
	CRedactions* ptmaxRedactions = (CRedactions*)paRedactions;
	CRedaction*	 ptmaxRedaction;
	POSITION	 Pos;

	Pos = ptmaxRedactions->GetHeadPosition();
	while(Pos != NULL)
	{
		if((ptmaxRedaction = (CRedaction*)ptmaxRedactions->GetNext(Pos)) != 0)
		{
			DeleteTmaxRedaction((long)ptmaxRedaction, sPane);
		}

	}

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::Deskew()
//
// 	Description:	This external method allows the caller to deskew the 
//					image in the specified pane.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::Deskew(short sPane) 
{
	return GetPane(sPane)->Deskew();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::Despeckle()
//
// 	Description:	This external method allows the caller to despeckle the 
//					image in the specified pane.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::Despeckle(short sPane) 
{
	return GetPane(sPane)->Despeckle();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::DestroyCallouts()
//
// 	Description:	This external method allows the caller to destroy all of
//					the callouts
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::DestroyCallouts(short sPane) 
{
	return GetPane(sPane)->DestroyCallouts();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::DoPropExchange()
//
// 	Description:	This function manages the exchange of the control properties
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::DoPropExchange(CPropExchange* pPX)
{
	BOOL bBackFile = FALSE;
	BOOL bLeftFile = FALSE;
	BOOL bRightFile = FALSE;
	BOOL bAnnTool = FALSE;
	BOOL bAnnThick = FALSE;
	BOOL bAnnColor = FALSE;
	BOOL bRedactColor = FALSE;
	BOOL bHighColor = FALSE;
	BOOL bRotation = FALSE;
	BOOL bMaxZoom = FALSE;
	BOOL bAction = FALSE;
	BOOL bPanPercent = FALSE;
	BOOL bZoomOnLoad = FALSE;
	BOOL bScaleImage = FALSE;
	BOOL bFitToImage = FALSE;
	BOOL bEnableErrors = FALSE;
	BOOL bLoopAnimation = FALSE;
	BOOL bAutoAnimate = FALSE;
	BOOL bHideScrollBars = FALSE;
	BOOL bRightClickPan = FALSE;
	BOOL bSplitScreen = FALSE;
	BOOL bSyncPanes = FALSE;
	BOOL bCalloutColor = FALSE;
	BOOL bCalloutFrameThickness = FALSE;
	BOOL bCalloutFrameColor = FALSE;
	BOOL bSplitFrameThickness = FALSE;
	BOOL bSplitFrameColor = FALSE;
	BOOL bActivePane = FALSE;
	BOOL bSyncCalloutAnn = FALSE;
	BOOL bPenSelectorVisible = FALSE;
	BOOL bPenSelectorColor = FALSE;
	BOOL bPenSelectorSize = FALSE;
	BOOL bKeepAspect = FALSE;
	BOOL bBitonalScaling = FALSE;
	BOOL bZoomToRect = FALSE;
	BOOL bAnnFontName = FALSE;
	BOOL bAnnFontSize = FALSE;
	BOOL bAnnFontUnderline = FALSE;
	BOOL bAnnFontStrikeThrough = FALSE;
	BOOL bAnnFontBold = FALSE;
	BOOL bDeskewBackColor = FALSE;
	BOOL bAnnotateCallouts = FALSE;
	BOOL bPrintOrientation = FALSE;
	BOOL bPrintBorderColor = FALSE;
	BOOL bPrintBorderThickness = FALSE;
	BOOL bPrintLeftMargin = FALSE;
	BOOL bPrintRightMargin = FALSE;
	BOOL bPrintTopMargin = FALSE;
	BOOL bPrintBottomMargin = FALSE;
	BOOL bPrintCallouts = FALSE;
	BOOL bPrintBorder = FALSE;
	BOOL bPrintCalloutBorders = FALSE;
	BOOL bAnnColorDepth = FALSE;
	BOOL bSplitHorizontal = FALSE;
	BOOL bQFactor = FALSE;
	BOOL bResizeCallouts = FALSE;
	BOOL bCalloutHandleColor = FALSE;
	BOOL bShadeOnCallout = FALSE;
	BOOL bCalloutShadeGrayscale = FALSE;
	BOOL bPanCallouts = FALSE;
	BOOL bZoomCallouts = FALSE;
	BOOL bLoadAsync = FALSE;
	BOOL bEnableAxErrors = FALSE;

	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
	COleControl::DoPropExchange(pPX);

	//	Check the control revision
	//CheckVersion(pPX->GetVersion());

	try
	{
		//	Load the control's persistent properties
		bBackFile = PX_String(pPX, _T("BackgroundFile"), m_strBackgroundFile, "Not available");
		bLeftFile = PX_String(pPX, _T("LeftFile"), m_strLeftFile, "");
		bRightFile = PX_String(pPX, _T("RightFile"), m_strRightFile, "");
		bAnnTool = PX_Short(pPX, _T("AnnTool"), m_sAnnTool, DEFAULT_ANNTOOL);
		bAnnThick = PX_Short(pPX, _T("AnnThickness"), m_sAnnThickness, DEFAULT_ANNTHICKNESS);
		bAnnColor = PX_Short(pPX, _T("AnnColor"), m_sAnnColor, DEFAULT_ANNCOLOR);
		bRedactColor = PX_Short(pPX, _T("RedactColor"), m_sRedactColor, DEFAULT_REDACTCOLOR);
		bHighColor = PX_Short(pPX, _T("HighlightColor"), m_sHighlightColor, DEFAULT_HIGHLIGHTCOLOR);
		bRotation = PX_Short(pPX, _T("Rotation"), m_sRotation, DEFAULT_ROTATION);
		bMaxZoom = PX_Short(pPX, _T("MaxZoom"), m_sMaxZoom, DEFAULT_MAXZOOM);
		bAction = PX_Short(pPX, _T("Action"), m_sAction, DEFAULT_ACTION);
		bPanPercent = PX_Short(pPX, _T("PanPercent"), m_sPanPercent, DEFAULT_PANPERCENT);
		bZoomOnLoad = PX_Short(pPX, _T("ZoomOnLoad"), m_sZoomOnLoad, ZOOMED_NONE);
		bScaleImage = PX_Bool(pPX, _T("ScaleImage"), m_bScaleImage, DEFAULT_SCALEIMAGE);
		bFitToImage = PX_Bool(pPX, _T("FitToImage"), m_bFitToImage, DEFAULT_FITTOIMAGE);
		bEnableErrors = PX_Bool(pPX, _T("EnableErrors"), m_bEnableErrors, TRUE);
		bLoopAnimation = PX_Bool(pPX, _T("LoopAnimation"), m_bLoopAnimation, FALSE);
		bAutoAnimate = PX_Bool(pPX, _T("AutoAnimate"), m_bAutoAnimate, FALSE);
		bHideScrollBars = PX_Bool(pPX, _T("HideSrollBars"), m_bHideScrollBars, DEFAULT_HIDESCROLLBARS);
		bRightClickPan = PX_Bool(pPX, _T("RightClickPan"), m_bRightClickPan, DEFAULT_RIGHTCLICKPAN);
		bSplitScreen = PX_Bool(pPX, _T("SplitScreen"), m_bSplitScreen, DEFAULT_SPLITSCREEN);
		bSyncPanes = PX_Bool(pPX, _T("SyncPanes"), m_bSyncPanes, DEFAULT_SYNCPANES);
		bCalloutColor = PX_Short(pPX, _T("CalloutColor"), m_sCalloutColor, DEFAULT_CALLOUTCOLOR);
		bCalloutFrameThickness = PX_Short(pPX, _T("CalloutFrameThickness"), m_sCalloutFrameThickness, DEFAULT_CALLFRAMETHICKNESS);
		bCalloutFrameColor = PX_Short(pPX, _T("CalloutFrameColor"), m_sCalloutFrameColor, DEFAULT_CALLFRAMECOLOR);
		bSplitFrameThickness = PX_Short(pPX, _T("SplitFrameThickness"), m_sSplitFrameThickness, DEFAULT_SPLITFRAMETHICKNESS);
		bSplitFrameColor = PX_Short(pPX, _T("SplitFrameColor"), m_sSplitFrameColor, DEFAULT_SPLITFRAMECOLOR);
		bActivePane = PX_Short(pPX, _T("ActivePane"), m_sActivePane, DEFAULT_ACTIVEPANE);
		bSyncCalloutAnn = PX_Bool(pPX, _T("SyncCalloutAnn"), m_bSyncCalloutAnn, DEFAULT_SYNCCALLOUTANN);
		bPenSelectorVisible = PX_Bool(pPX, _T("PenSelectorVisible"), m_bPenSelectorVisible, DEFAULT_PENSELECTORVISIBLE);
		bPenSelectorColor = PX_Short(pPX, _T("PenSelectorColor"), m_sPenSelectorColor, DEFAULT_PENSELECTORCOLOR);
		bPenSelectorSize = PX_Short(pPX, _T("PenSelectorSize"), m_sPenSelectorSize, DEFAULT_PENSELECTORSIZE);
		bKeepAspect = PX_Bool(pPX, _T("KeepAspect"), m_bKeepAspect, DEFAULT_KEEPASPECT);
		bBitonalScaling = PX_Short(pPX, _T("BitonalScaling"), m_sBitonal, DEFAULT_BITONALSCALING);
		bZoomToRect = PX_Bool(pPX, _T("ZoomToRect"), m_bZoomToRect, DEFAULT_ZOOMTORECT);
		bAnnFontName = PX_String(pPX, _T("AnnFontName"), m_strAnnFontName, DEFAULT_ANNFONTNAME);
		bAnnFontSize = PX_Short(pPX, _T("AnnFontSize"), m_sAnnFontSize, DEFAULT_ANNFONTSIZE);
		bAnnFontUnderline = PX_Bool(pPX, _T("AnnFontUnderline"), m_bAnnFontUnderline, DEFAULT_ANNFONTUNDERLINE);
		bAnnFontBold = PX_Bool(pPX, _T("AnnFontBold"), m_bAnnFontBold, DEFAULT_ANNFONTBOLD);
		bAnnFontStrikeThrough = PX_Bool(pPX, _T("AnnFontStrikeThrough"), m_bAnnFontStrikeThrough, DEFAULT_ANNFONTSTRIKETHROUGH);
		bDeskewBackColor = PX_Color(pPX, _T("DeskewBackColor"), m_lDeskewBackColor, ((OLE_COLOR)DEFAULT_DESKEWBACKCOLOR));
		bAnnotateCallouts = PX_Bool(pPX, _T("AnnotateCallouts"), m_bAnnotateCallouts, DEFAULT_ANNOTATECALLOUTS);
		bPrintOrientation = PX_Short(pPX, _T("PrintOrientation"), m_sPrintOrientation, DEFAULT_PRINTORIENTATION);
		bPrintBorderColor = PX_Color(pPX, _T("PrintBorderColor"), m_lPrintBorderColor, DEFAULT_PRINTBORDERCOLOR);
		bPrintBorderThickness = PX_Float(pPX, _T("PrintBorderThickness"), m_fPrintBorderThickness, DEFAULT_PRINTBORDERTHICKNESS);
		bPrintLeftMargin = PX_Float(pPX, _T("PrintLeftMargin"), m_fPrintLeftMargin, DEFAULT_PRINTLEFTMARGIN);
		bPrintRightMargin = PX_Float(pPX, _T("PrintRightMargin"), m_fPrintRightMargin, DEFAULT_PRINTRIGHTMARGIN);
		bPrintTopMargin = PX_Float(pPX, _T("PrintTopMargin"), m_fPrintTopMargin, DEFAULT_PRINTTOPMARGIN);
		bPrintBottomMargin = PX_Float(pPX, _T("PrintBottomMargin"), m_fPrintBottomMargin, DEFAULT_PRINTBOTTOMMARGIN);
		bPrintCallouts = PX_Bool(pPX, _T("PrintCallouts"), m_bPrintCallouts, DEFAULT_PRINTCALLOUTS);
		bPrintBorder = PX_Bool(pPX, _T("PrintBorder"), m_bPrintBorder, DEFAULT_PRINTBORDER);
		bPrintCalloutBorders = PX_Bool(pPX, _T("PrintCalloutBorders"), m_bPrintCalloutBorders, DEFAULT_PRINTCALLOUTBORDERS);
		bAnnColorDepth = PX_Short(pPX, _T("AnnColorDepth"), m_sAnnColorDepth, DEFAULT_ANNCOLORDEPTH);
		bSplitHorizontal = PX_Bool(pPX, _T("SplitHorizontal"), m_bSplitHorizontal, DEFAULT_SPLITHORIZONTAL);
		bQFactor = PX_Short(pPX, _T("QFactor"), m_sQFactor, DEFAULT_QFACTOR);
		bResizeCallouts = PX_Bool(pPX, _T("ResizeCallouts"), m_bResizeCallouts, DEFAULT_RESIZECALLOUTS);
		bCalloutHandleColor = PX_Short(pPX, _T("CalloutHandleColor"), m_sCalloutHandleColor, DEFAULT_CALLHANDLECOLOR);
		bShadeOnCallout = PX_Bool(pPX, _T("ShadeOnCallout"), m_bShadeOnCallout, DEFAULT_SHADEONCALLOUT);
		bCalloutShadeGrayscale = PX_Short(pPX, _T("CalloutShadeGrayscale"), m_sCalloutShadeGrayscale, DEFAULT_CALLOUTSHADEGRAYSCALE);
		bPanCallouts = PX_Bool(pPX, _T("PanCallouts"), m_bPanCallouts, DEFAULT_PANCALLOUTS);
		bZoomCallouts = PX_Bool(pPX, _T("ZoomCallouts"), m_bZoomCallouts, DEFAULT_ZOOMCALLOUTS);
		bLoadAsync = PX_Bool(pPX, _T("LoadAsync"), m_bLoadAsync, DEFAULT_LOADASYNC);
		bEnableAxErrors = PX_Bool(pPX, _T("EnableAxErrors"), m_bEnableAxErrors, DEFAULT_ENABLEAXERRORS);
	}
	catch(...)
	{
		if(!bBackFile) m_strBackgroundFile.Empty();
		if(!bLeftFile) m_strLeftFile.Empty();
		if(!bRightFile) m_strRightFile.Empty();
		if(!bAnnTool) m_sAnnTool = DEFAULT_ANNTOOL;
		if(!bAnnThick) m_sAnnThickness = DEFAULT_ANNTHICKNESS;
		if(!bAnnColor) m_sAnnColor = DEFAULT_ANNCOLOR;
		if(!bRedactColor) m_sRedactColor = DEFAULT_REDACTCOLOR;
		if(!bHighColor) m_sHighlightColor = DEFAULT_HIGHLIGHTCOLOR;
		if(!bRotation) m_sRotation = DEFAULT_ROTATION;
		if(!bMaxZoom) m_sMaxZoom = DEFAULT_MAXZOOM;
		if(!bAction) m_sAction, DEFAULT_ACTION;
		if(!bPanPercent) m_sPanPercent = DEFAULT_PANPERCENT;
		if(!bZoomOnLoad) m_sZoomOnLoad = ZOOMED_NONE;
		if(!bScaleImage) m_bScaleImage = DEFAULT_SCALEIMAGE;
		if(!bFitToImage) m_bFitToImage = DEFAULT_FITTOIMAGE;
		if(!bEnableErrors) m_bEnableErrors = TRUE;
		if(!bLoopAnimation) m_bLoopAnimation = FALSE;
		if(!bAutoAnimate) m_bAutoAnimate = FALSE;
		if(!bHideScrollBars) m_bHideScrollBars = DEFAULT_HIDESCROLLBARS;
		if(!bRightClickPan) m_bRightClickPan = DEFAULT_RIGHTCLICKPAN;
		if(!bSplitScreen) m_bSplitScreen = DEFAULT_SPLITSCREEN;
		if(!bSyncPanes) m_bSyncPanes = DEFAULT_SYNCPANES;
		if(!bCalloutColor) m_sCalloutColor = DEFAULT_CALLOUTCOLOR;
		if(!bCalloutFrameThickness) m_sCalloutFrameThickness = DEFAULT_CALLFRAMETHICKNESS;
		if(!bCalloutFrameColor) m_sCalloutFrameColor = DEFAULT_CALLFRAMECOLOR;
		if(!bSplitFrameThickness) m_sSplitFrameThickness = DEFAULT_SPLITFRAMETHICKNESS;
		if(!bSplitFrameColor) m_sSplitFrameColor = DEFAULT_SPLITFRAMECOLOR;
		if(!bActivePane) m_sActivePane = DEFAULT_ACTIVEPANE;
		if(!bSyncCalloutAnn) m_bSyncCalloutAnn = DEFAULT_SYNCCALLOUTANN;
		if(!bPenSelectorVisible) m_bPenSelectorVisible = DEFAULT_PENSELECTORVISIBLE;
		if(!bPenSelectorColor) m_sPenSelectorColor = DEFAULT_PENSELECTORCOLOR;
		if(!bPenSelectorSize) m_sPenSelectorSize = DEFAULT_PENSELECTORSIZE;
		if(!bKeepAspect) m_bKeepAspect = DEFAULT_KEEPASPECT;
		if(!bBitonalScaling) m_sBitonal = DEFAULT_BITONALSCALING;
		if(!bZoomToRect) m_bZoomToRect = DEFAULT_ZOOMTORECT;
		if(!bAnnFontName) m_strAnnFontName = DEFAULT_ANNFONTNAME;
		if(!bAnnFontSize) m_sAnnFontSize = DEFAULT_ANNFONTSIZE;
		if(!bAnnFontUnderline) m_bAnnFontUnderline = DEFAULT_ANNFONTUNDERLINE;
		if(!bAnnFontBold) m_bAnnFontBold = DEFAULT_ANNFONTBOLD;
		if(!bAnnFontStrikeThrough) m_bAnnFontStrikeThrough = DEFAULT_ANNFONTSTRIKETHROUGH;
		if(!bDeskewBackColor) m_lDeskewBackColor = ((OLE_COLOR)DEFAULT_DESKEWBACKCOLOR);
		if(!bAnnotateCallouts) m_bAnnotateCallouts = DEFAULT_ANNOTATECALLOUTS;
		if(!bPrintOrientation) m_sPrintOrientation = DEFAULT_PRINTORIENTATION;
		if(!bPrintBorderColor) m_lPrintBorderColor = DEFAULT_PRINTBORDERCOLOR;
		if(!bPrintBorderThickness) m_fPrintBorderThickness = DEFAULT_PRINTBORDERTHICKNESS;
		if(!bPrintLeftMargin) m_fPrintLeftMargin = DEFAULT_PRINTLEFTMARGIN;
		if(!bPrintRightMargin) m_fPrintRightMargin = DEFAULT_PRINTRIGHTMARGIN;
		if(!bPrintTopMargin) m_fPrintTopMargin = DEFAULT_PRINTTOPMARGIN;
		if(!bPrintBottomMargin) m_fPrintBottomMargin = DEFAULT_PRINTBOTTOMMARGIN;
		if(!bPrintCallouts) m_bPrintCallouts = DEFAULT_PRINTCALLOUTS;
		if(!bPrintBorder) m_bPrintBorder = DEFAULT_PRINTBORDER;
		if(!bPrintCalloutBorders) m_bPrintCalloutBorders = DEFAULT_PRINTCALLOUTBORDERS;
		if(!bAnnColorDepth) m_sAnnColorDepth = DEFAULT_ANNCOLORDEPTH;
		if(!bSplitHorizontal) m_bSplitHorizontal = DEFAULT_SPLITHORIZONTAL;
		if(!bQFactor) m_sQFactor = DEFAULT_QFACTOR;
		if(!bResizeCallouts) m_bResizeCallouts = DEFAULT_RESIZECALLOUTS;
		if(!bCalloutHandleColor) m_sCalloutHandleColor = DEFAULT_CALLHANDLECOLOR;
		if(!bShadeOnCallout) m_bShadeOnCallout = DEFAULT_SHADEONCALLOUT;
		if(!bCalloutShadeGrayscale) m_sCalloutShadeGrayscale = DEFAULT_CALLOUTSHADEGRAYSCALE;
		if(!bPanCallouts) m_bPanCallouts = DEFAULT_PANCALLOUTS;
		if(!bZoomCallouts) m_bZoomCallouts = DEFAULT_ZOOMCALLOUTS;
		if(!bLoadAsync) m_bLoadAsync = DEFAULT_LOADASYNC;
		if(!bEnableAxErrors) m_bEnableAxErrors = DEFAULT_ENABLEAXERRORS;
	}

	if(pPX->IsLoading())
	{
		//	Set default values for properties added after initial release
		//	of the major version
		//
		//	NOTE:	The drop through design of the switch statement is intentional
		//switch(LOWORD(pPX->GetVersion()))
		//{
		//}
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::DotRemove()
//
// 	Description:	This external method allows the caller to remove dots 
//					from 1-bit images.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::DotRemove(short sPane, long lMinWidth, long lMinHeight,
							 long lMaxWidth, long lMaxHeight) 
{
	return GetPane(sPane)->DotRemove(lMinWidth, lMinHeight, lMaxWidth, lMaxHeight);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::DrawPenSelector()
//
// 	Description:	This function draws the highlight that permits selection of
//					empty panes with a light pen
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::DrawPenSelector()
{
	CDC*	pDc;
	RECT	Rect;
	CBrush	brSelector;

	if(!m_bPenSelectorVisible)
		return;

	//	Get the dc for this window
	if((pDc = GetDC()) == 0)
		return;

	//	Create the brush we need to draw the selector
	brSelector.CreateSolidBrush(m_PaneA.GetColorRef(m_sPenSelectorColor));

	//	Is the left pane empty?
	if(!GetPane(TMV_LEFTPANE)->IsLoaded())
	{
		//	Get the coordinates of the selector rectangle
		if(m_bPenTop)
		{
			Rect.left = m_rcLPane.left + 2;
			Rect.right = Rect.left + m_sPenSelectorSize;
			Rect.top = m_rcLPane.top + 2;
			Rect.bottom = Rect.top + m_sPenSelectorSize;
		}
		else
		{
			Rect.left = m_rcLPane.left + 2;
			Rect.right = Rect.left + m_sPenSelectorSize;
			Rect.bottom = m_rcLPane.bottom - 2;
			Rect.top = Rect.bottom - m_sPenSelectorSize;
		}

		if(Rect.right > m_rcLPane.right)
			Rect.right = m_rcLPane.right;
		if(Rect.top < m_rcLPane.top)
			Rect.top = m_rcLPane.top;

		pDc->FillRect(&Rect, &brSelector);
	}

	//	Is the right pane empty?
	if(!GetPane(TMV_RIGHTPANE)->IsLoaded())
	{
		//	Get the coordinates of the selector rectangle
		if(m_bPenTop)
		{
			Rect.right = (m_rcRPane.left + m_rcRPane.right) - 2;
			Rect.left = Rect.right - m_sPenSelectorSize;
			Rect.top = m_rcRPane.top + 2;
			Rect.bottom = Rect.top + m_sPenSelectorSize;
		}
		else
		{
			Rect.right = (m_rcRPane.left + m_rcRPane.right) - 2;
			Rect.left = Rect.right - m_sPenSelectorSize;
			Rect.bottom = (m_rcRPane.top + m_rcRPane.bottom) - 2;
			Rect.top = Rect.bottom - m_sPenSelectorSize;
		}

		pDc->FillRect(&Rect, &brSelector);
	}

	ReleaseDC(pDc);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::DrawRectangle()
//
// 	Description:	This function draws a rectangular annotation using the 
//					specified coordinates and properties. The coordinates are
//					expected to be client window coordinates.
//
// 	Returns:		A handle used to identify the annotation if successful
//					Zero on failure
//
//	Notes:			None
//
//==============================================================================
long CTMViewCtrl::DrawRectangle(short sLeft, short sTop, short sRight, short sBottom, OLE_COLOR lColor, short sTransparency, BOOL bLocked, short sPane) 
{
	RECT	rcRect;
	long	lAnn;

	//	Initialize a rectangle using the caller's coordinates
	rcRect.left = sLeft;
	rcRect.top = sTop;
	rcRect.right = sRight;
	rcRect.bottom = sBottom;

	lAnn = (long)GetPane(sPane)->DrawRectangle(rcRect, TranslateColor(lColor), sTransparency);

	if((lAnn != 0) && (bLocked == TRUE))
		GetPane(sPane)->LockAnn((HANNOBJECT)lAnn);

	return lAnn;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::DrawSourceRectangle()
//
// 	Description:	This function draws a rectangular annotation using the 
//					specified coordinates and properties. The coordinates are
//					expected to be source image coordinates.
//
// 	Returns:		A handle used to identify the annotation if successful
//					Zero on failure
//
//	Notes:			None
//
//==============================================================================
long CTMViewCtrl::DrawSourceRectangle(short sLeft, short sTop, short sRight, short sBottom, OLE_COLOR lColor, short sTransparency, BOOL bLocked, short sPane) 
{
	RECT rcRect;
	long lAnn;

	//	Initialize a rectangle using the caller's coordinates
	rcRect.left = sLeft;
	rcRect.top = sTop;
	rcRect.right = sRight;
	rcRect.bottom = sBottom;

	//	Draw the rectangle
	lAnn = (long)GetPane(sPane)->DrawSourceRectangle(rcRect, TranslateColor(lColor), sTransparency);

	if((lAnn != 0) && (bLocked == TRUE))
		GetPane(sPane)->LockAnn((HANNOBJECT)lAnn);

	return lAnn;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::DrawSourceText()
//
// 	Description:	This function draws a text annotation using the 
//					specified coordinates and properties. The coordinates are
//					expected to be source image coordinates.
//
// 	Returns:		A handle used to identify the annotation if successful
//					Zero on failure
//
//	Notes:			None
//
//==============================================================================
long CTMViewCtrl::DrawSourceText(LPCTSTR lpszText, short sLeft, short sTop, short sRight, short sBottom, OLE_COLOR lColor, LPCTSTR lpszFont, short sSize, BOOL bLocked, short sPane) 
{
	RECT rcRect;
	long lAnn;

	//	Initialize a rectangle using the caller's coordinates
	rcRect.left = sLeft;
	rcRect.top = sTop;
	rcRect.right = sRight;
	rcRect.bottom = sBottom;

	lAnn = (long)GetPane(sPane)->DrawSourceText(lpszText, rcRect, TranslateColor(lColor), lpszFont, sSize);

	if((lAnn != 0) && (bLocked == TRUE))
		GetPane(sPane)->LockAnn((HANNOBJECT)lAnn);

	return lAnn;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::DrawSplitFrame()
//
// 	Description:	This function draws the highlight frame for the active view
//					in split screen mode
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::DrawSplitFrame(RECT* pRect)
{
	CPen	FramePen;
	CDC*	pDc;
	CPen*	pOldPen;
	POINT	Points[5];

	//	Are we in user mode?
	if(!AmbientUserMode())
		return;

	//	Are we using a frame indicator
	if(m_sSplitFrameThickness <= 0)
		return;

	//	Get the dc for this window
	if((pDc = GetDC()) == 0)
		return;

	//	Create the pens used to draw the frames
	FramePen.CreatePen(PS_INSIDEFRAME, m_sSplitFrameThickness, 
					   m_PaneA.GetColorRef(m_sSplitFrameColor));

	//	Set up the array of points
	Points[0].x = pRect->left;
	Points[0].y = pRect->top;
	Points[1].x = pRect->right;
	Points[1].y = pRect->top;
	Points[2].x = pRect->right;
	Points[2].y = pRect->bottom;
	Points[3].x = pRect->left;
	Points[3].y = pRect->bottom;
	Points[4].x = pRect->left;
	Points[4].y = pRect->top;

	//	Select the pen into the dc
	pOldPen = pDc->SelectObject(&FramePen);
		
	//	Draw the highlight frame
	pDc->Polyline(Points, 5);

	//	Select the original pen
	pDc->SelectObject(pOldPen);

	ReleaseDC(pDc);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::DrawText()
//
// 	Description:	This function draws a text annotation using the 
//					specified coordinates and properties. The coordinates are
//					expected to be client window coordinates.
//
// 	Returns:		A handle used to identify the annotation if successful
//					Zero on failure
//
//	Notes:			None
//
//==============================================================================
long CTMViewCtrl::DrawText(LPCTSTR lpszText, short sLeft, short sTop, short sRight, short sBottom, OLE_COLOR lColor, LPCTSTR lpszFont, short sSize, BOOL bLocked, short sPane) 
{
	RECT rcRect;
	long lAnn;

	//	Initialize a rectangle using the caller's coordinates
	rcRect.left = sLeft;
	rcRect.top = sTop;
	rcRect.right = sRight;
	rcRect.bottom = sBottom;

	lAnn = (long)GetPane(sPane)->DrawText(lpszText, rcRect, TranslateColor(lColor), lpszFont, sSize);

	if((lAnn != 0) && (bLocked == TRUE))
		GetPane(sPane)->LockAnn((HANNOBJECT)lAnn);

	return lAnn;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::DrawTmaxRedaction()
//
// 	Description:	This function draws the annotations required to create the
//					TrialMax redaction.
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::DrawTmaxRedaction(long pRedaction, short sPane) 
{
	CRedaction* ptmaxRedaction = (CRedaction*)pRedaction;

	//	Draw the redaction
	ptmaxRedaction->m_lAnnRedaction = (long)GetPane(sPane)->DrawSourceRectangle(ptmaxRedaction->m_rcBounds,
																			 ptmaxRedaction->m_crRedaction,
																			 ptmaxRedaction->m_bOpaque ? TMV_OPAQUE : TMV_TRANSLUCENT);
	//	Should we add a label?
	if((ptmaxRedaction->m_lAnnRedaction != 0) && (ptmaxRedaction->m_strLabel.GetLength() > 0))
	{
		ptmaxRedaction->m_lAnnLabel = (long)GetPane(sPane)->DrawSourceText(ptmaxRedaction->m_strLabel,
																		ptmaxRedaction->m_rcBounds,
																		ptmaxRedaction->m_crLabel,
																		ptmaxRedaction->m_strFontName,
																		ptmaxRedaction->m_sFontSize);
	}

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::DrawTmaxRedactions()
//
// 	Description:	This function draw redactions for each object in the list.
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::DrawTmaxRedactions(long paRedactions, short sPane) 
{
	CRedactions* ptmaxRedactions = (CRedactions*)paRedactions;
	CRedaction*	 ptmaxRedaction;
	POSITION	 Pos;

	Pos = ptmaxRedactions->GetHeadPosition();
	while(Pos != NULL)
	{
		if((ptmaxRedaction = (CRedaction*)ptmaxRedactions->GetNext(Pos)) != 0)
		{
			DrawTmaxRedaction((long)ptmaxRedaction, sPane);
		}

	}

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::EnableDIBPrinting()
//
// 	Description:	This external method is called by the application to enable
//					or disable DIB printing
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::EnableDIBPrinting(short bEnable) 
{
	m_PaneA.EnableDIBPrinting(bEnable != 0);
	m_PaneB.EnableDIBPrinting(bEnable != 0);
	m_Scratch.EnableDIBPrinting(bEnable != 0);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::Erase()
//
// 	Description:	This external method allows the caller to erase all 
//					annotations on the current image.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::Erase(short sPane) 
{
	GetPane(sPane)->Erase(FALSE);
	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::EraseSplitFrame()
//
// 	Description:	This function draws the erase the split frame using the
//					rectangle provided by the caller
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::EraseSplitFrame(RECT* pRect)
{
	CPen	FramePen;
	CDC*	pDc;
	CPen*	pOldPen;
	POINT	Points[5];

	//	Are we in user mode?
	if(!AmbientUserMode())
		return;

	//	Get the dc for this window
	if((pDc = GetDC()) == 0)
		return;

	//	Create the pens used to draw the frames
	FramePen.CreatePen(PS_INSIDEFRAME, m_sSplitFrameThickness, 
					   TranslateColor(GetBackColor()));

	//	Set up the array of points
	Points[0].x = pRect->left;
	Points[0].y = pRect->top;
	Points[1].x = pRect->right;
	Points[1].y = pRect->top;
	Points[2].x = pRect->right;
	Points[2].y = pRect->bottom;
	Points[3].x = pRect->left;
	Points[3].y = pRect->bottom;
	Points[4].x = pRect->left;
	Points[4].y = pRect->top;

	//	Select the pen into the dc
	pOldPen = pDc->SelectObject(&FramePen);
		
	//	Draw the highlight frame
	pDc->Polyline(Points, 5);

	//	Select the original pen
	pDc->SelectObject(pOldPen);

	ReleaseDC(pDc);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::FillZapHeader()
//
// 	Description:	This function will populate the zap file header specified
//					by the caller with the default values
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::FillZapHeader(SZapHeader* pZapHeader) 
{
	ASSERT(pZapHeader != NULL);

	//	Initialize the header
	ZeroMemory(pZapHeader, sizeof(SZapHeader));
	pZapHeader->lVersion = ZAP_NEW_VERSION;
	pZapHeader->sScreenWidth  = GetSystemMetrics(SM_CXSCREEN);
	pZapHeader->sScreenHeight = GetSystemMetrics(SM_CYSCREEN);
	pZapHeader->bSplitScreen = FALSE;
	pZapHeader->dwCtrlVersion = m_tmVersion.GetPackedVer();
	pZapHeader->wFooterSize = sizeof(SZapFooter);
	pZapHeader->wFlags = ZAP_HF_WND_COORDINATES; //	Indicates valid pane coordinates in SZapPane
	GetWindowRect(&(pZapHeader->rcWindow));

	//m_PaneA.MsgBox(pZapHeader, "Fill");
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::FirstPage()
//
// 	Description:	This external method allows the caller to go to the first
//					page in the file
//
// 	Returns:		TMV_NOERROR if successful. Otherwise, one of the TMV error
//					levels defined in tmvdefs.h
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::FirstPage(short sPane) 
{
	return GetPane(sPane)->FirstPage();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetAspectRatio()
//
// 	Description:	This method is called to retrieve the aspect ratio of the
//					current image.
//
// 	Returns:		The aspect ratio. 1 if not loaded.
//
//	Notes:			None
//
//==============================================================================
float CTMViewCtrl::GetAspectRatio(short sPane) 
{
	return GetPane(sPane)->GetAspectRatio();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetCalloutCount()
//
// 	Description:	This method is called to retrieve the numer of callouts
//					owned by the specified pane
//
// 	Returns:		The number of callouts
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::GetCalloutCount(short sPane) 
{
	return ((short)GetPane(sPane)->GetCalloutCount());
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetClassIdString()
//
// 	Description:	This method is called to get the class GUID as a null
//					terminated string.
//
// 	Returns:		The string equivalent of the CLASS GUID
//
//	Notes:			None
//
//==============================================================================
BSTR CTMViewCtrl::GetClassIdString() 
{
	CString strClsId = m_tmVersion.GetClsId();
	return strClsId.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetColor()
//
// 	Description:	This external method allows the caller to retrieve the 
//					color associated with the active tool of the active pane
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::GetColor() 
{
	return GetPane()->GetColor();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetControlFlags()
//
// 	Description:	The container calls this function to determine how to use
//					and display the control. This function modifies the default
//					behavior of an Active X control.
//
// 	Returns:		The control flags
//
//	Notes:			None
//
//==============================================================================
DWORD CTMViewCtrl::GetControlFlags()
{
	DWORD dwFlags = COleControl::GetControlFlags();

	// The control's output is not being clipped.
	// The control guarantees that it will not paint outside its
	// client rectangle.
	dwFlags &= ~clipPaintDC;

	// The control will not be redrawn when making the transition
	// between the active and inactivate state.
	dwFlags |= noFlickerActivate;
	return dwFlags;
	
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetCurrentPage()
//
// 	Description:	This external method allows the caller to request the 
//					page number being displayed.
//
// 	Returns:		The page number.
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::GetCurrentPage(short sPane) 
{
	return GetPane(sPane)->GetCurrentPage();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetCurrentPrinter()
//
// 	Description:	This external method allows the caller to get the name of
//					the attached printer.
//
// 	Returns:		The name of the current printer if it exists.
//
//	Notes:			None
//
//==============================================================================
BSTR CTMViewCtrl::GetCurrentPrinter() 
{
	CString strPrinter;
	strPrinter = m_Printer.GetName();
	return strPrinter.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetData()
//
// 	Description:	This external method allows the caller to get the value
//					associated with a pane that it set with a call to
//					SetData()
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
long CTMViewCtrl::GetData(short sPane) 
{
	return GetPane(sPane)->m_lUserData;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetDefaultPrinter()
//
// 	Description:	This external method allows the caller to get the name of
//					the default printer.
//
// 	Returns:		The name of the default printer if it exists.
//
//	Notes:			None
//
//==============================================================================
BSTR CTMViewCtrl::GetDefaultPrinter() 
{
	CString strDefault;
	m_Printer.GetDefault(strDefault);
	return strDefault.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetImageHeight()
//
// 	Description:	This method is called to retrieve the height in pixels of
//					the current image.
//
// 	Returns:		The image height. 0 if not loaded.
//
//	Notes:			None
//
//==============================================================================
float CTMViewCtrl::GetImageHeight(short sPane) 
{
	return GetPane(sPane)->GetImageHeight();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetImageProperties()
//
// 	Description:	This method is called to retrieve the set of properties 
//					associated with the image loaded in the specified pane.
//
// 	Returns:		The set of properties
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::GetImageProperties(long lpProperties, short sPane)
{
	return GetPane(sPane)->GetImageProperties((STMVImageProperties*)lpProperties);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetImageWidth()
//
// 	Description:	This method is called to retrieve the width in pixels of
//					the current image.
//
// 	Returns:		The image width. 0 if not loaded.
//
//	Notes:			None
//
//==============================================================================
float CTMViewCtrl::GetImageWidth(short sPane) 
{
	return GetPane(sPane)->GetImageWidth();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetControlKeys()
//
// 	Description:	This function will check the keyboard to get the state of
//					the control keys
//
// 	Returns:		The appropriate key state identifier
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::GetControlKeys()
{
	short sKeyState = 0;

	//	Is the shift key pressed?
	if(GetKeyState(VK_SHIFT) & 0x8000)
	{
		//	Is the control key also pressed?
		if(GetKeyState(VK_CONTROL))
		{
			//	Is the Alt key also pressed?
			if(GetKeyState(VK_MENU))
				sKeyState = CTRLALTSHIFT;
			else
				sKeyState = CTRLSHIFT;
		}
		else if(GetKeyState(VK_MENU))
		{
			sKeyState = ALTSHIFT;
		}
		else
		{
			sKeyState = SHIFT;
		}
	}
	else if(GetKeyState(VK_CONTROL))
	{
		//	Is the Alt key pressed?
		if(GetKeyState(VK_MENU))
			sKeyState = CTRLALT;
		else
			sKeyState = CTRL;
	}
	else if(GetKeyState(VK_MENU))
	{
		sKeyState = ALT;
	}

	return sKeyState;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetLeadToolError()
//
// 	Description:	This external method allows the caller to retrieve the last
//					LeadTools error code.
//
// 	Returns:		The LeadTools error identifier
//
//	Notes:			This function is normally called after recieving a
//					TMV_LEADERROR return value.
//
//==============================================================================
short CTMViewCtrl::GetLeadToolError(short sPane) 
{
	return GetPane(sPane)->GetLeadError();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetOleColor()
//
// 	Description:	This method will translate a Tmview color identifier to its
//					equivalent OLE_COLOR identifier
//
// 	Returns:		The associated OLE_COLOR identifier
//
//	Notes:			None
//
//==============================================================================
OLE_COLOR CTMViewCtrl::GetOleColor(short sTmviewColor) 
{
	return (OLE_COLOR)GetPane()->GetColorRef(sTmviewColor);
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetOrientation()
//
// 	Description:	This function will determine the best orientation for the
//					print job.
//
// 	Returns:		TRUE for portrait orientation
//
//	Notes:			None
//
//==============================================================================
BOOL CTMViewCtrl::GetOrientation(BOOL bLeft, BOOL bRight) 
{
	//	Has the user defined the orientation?
	if(m_sPrintOrientation != PRINT_ORIENTATION_AUTO)
		return (m_sPrintOrientation == PRINT_ORIENTATION_PORTRAIT);

	//	Always use landscape if printing both images
	if(bLeft && bRight)
		return FALSE;

	//	Are we printing the image in the left pane?
	if(bLeft)
	{
		//	Use landscape if printing callouts
		if(m_bPrintCallouts && GetPane(TMV_LEFTPANE)->GetCalloutCount() > 0)
			return FALSE;
		else
			return (GetPane(TMV_LEFTPANE)->GetSrcAspect() >= 1) ? TRUE : FALSE;
	}
	else
	{
		//	Use landscape if printing callouts
		if(m_bPrintCallouts && GetPane(TMV_RIGHTPANE)->GetCalloutCount() > 0)
			return FALSE;
		else
			return (GetPane(TMV_RIGHTPANE)->GetSrcAspect() >= 1) ? TRUE : FALSE;
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetPageCount()
//
// 	Description:	This external method allows the caller to retrieve the 
//					number of pages in the current image file.
//
// 	Returns:		The number of pages
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::GetPageCount(short sPane) 
{
	return GetPane(sPane)->GetPageCount();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetPane()
//
// 	Description:	This function is called to get the active pane
//
// 	Returns:		A pointer to the pane specified by the caller or the
//					active pane
//
//	Notes:			None
//
//==============================================================================
CTMLead* CTMViewCtrl::GetPane(short sPane) 
{
	//	Are we looking for the left pane?
	if(sPane == TMV_LEFTPANE)
	{
		return (m_pLeft) ? m_pLeft : &m_PaneA;
	}
	else if(sPane == TMV_RIGHTPANE)
	{
		return (m_pRight) ? m_pRight : &m_PaneB;
	}
	else
	{
		//	Return the active pane
		return (m_pActive) ? m_pActive : &m_PaneA;
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetPaneId()
//
// 	Description:	This function is called to get the id of the specified pane
//
// 	Returns:		The pane identifier
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::GetPaneId(CTMLead* pPane) 
{
	//	Are we looking for the left pane?
	if(pPane == m_pLeft)
	{
		return TMV_LEFTPANE;
	}
	else if(pPane == m_pRight)
	{
		return TMV_RIGHTPANE;
	}
	else
	{
		//	Return the active pane
		return m_sActivePane;
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetPanStates()
//
// 	Description:	This method is called to determine if the image can be 
//					panned in any direction
//
// 	Returns:		A packed short to indicate the directional states
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::GetPanStates(short sPane) 
{
	return GetPane(sPane)->GetPanStates();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetRegisteredPath()
//
// 	Description:	This method is called to get the ocx path stored in the
//					system registry.
//
// 	Returns:		The path to the ocx stored in the registry
//
//	Notes:			None
//
//==============================================================================
BSTR CTMViewCtrl::GetRegisteredPath() 
{
	CString strRegistered = m_tmVersion.GetFileSpec();
	return strRegistered.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetRegistration()
//
// 	Description:	This function is called to get the control's registration
//					information.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::GetRegistration() 
{
	CLSID clsid;

	//	Get the GUID 
	GetClassID(&clsid);

	//	Initialize the version information
	m_tmVersion.InitFromClsId("TMView", "Image Viewer", clsid);

}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetRGBColor()
//
// 	Description:	This external method allows the caller to convert a TMView
//					color identifier to its RGB equivalent.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
long CTMViewCtrl::GetRGBColor(short sColor) 
{
	return (long)m_PaneA.GetColorRef(sColor);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetScratchPane()
//
// 	Description:	This function is called to get the pane used for background
//					image processing.
//
// 	Returns:		A pointer to the scratch pane if available.
//
//	Notes:			None
//
//==============================================================================
CTMLead* CTMViewCtrl::GetScratchPane() 
{
	if(IsWindow(m_Scratch.m_hWnd))
		return &m_Scratch;
	else
		return 0;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetSelectCount()
//
// 	Description:	This method allows the caller to determine how many 
//					annotations are selected in the current pane
//
// 	Returns:		The number of selections
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::GetSelectCount(short sPane) 
{
	//	Check to see if control is loaded first otherwise LeadTools will throw
	//	an exception
	if(!GetPane(sPane)->IsLoaded())
		return 0;
	else	
		return GetPane(sPane)->AnnGetSelectCount();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetSrcRatio()
//
// 	Description:	This external method allows the caller to retrieve the 
//					current aspect ratio of the source rectangle
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
float CTMViewCtrl::GetSrcRatio(short sPane) 
{
	return GetPane(sPane)->GetSrcAspect();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetVerBuild()
//
// 	Description:	This method is called to get the value of the VerBuild
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::GetVerBuild() 
{
	return m_tmVersion.GetBuild();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetVerBuildDate()
//
// 	Description:	This method is called to get the value of the VerBuildDate
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMViewCtrl::GetVerBuildDate() 
{
	CString strBuildDate = m_tmVersion.GetBuildDate();
	return strBuildDate.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetVerMajor()
//
// 	Description:	This method is called to get the value of the VerMajor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::GetVerMajor() 
{
	return m_tmVersion.GetMajor();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetVerMinor()
//
// 	Description:	This method is called to get the value of the VerMinor
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::GetVerMinor() 
{
	return m_tmVersion.GetMinor();
}


//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetVerQEF()
//
// 	Description:	This method is called to get the value of the VerQEF
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::GetVerQEF() 
{
	return m_tmVersion.GetUpdate();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetVerTextLong()
//
// 	Description:	This method is called to get the value of the VerTextLong
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMViewCtrl::GetVerTextLong() 
{
	CString strVer = m_tmVersion.GetTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetVerTextShort()
//
// 	Description:	This method is called to get the value of the VerTextShort
//					property
//
// 	Returns:		The current value of the property
//
//	Notes:			None
//
//==============================================================================
BSTR CTMViewCtrl::GetVerTextShort() 
{
	CString strVer = m_tmVersion.GetShortTextVer();
	return strVer.AllocSysString();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::GetZoomFactor()
//
// 	Description:	This function will get the current zoom factor for the image
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
float CTMViewCtrl::GetZoomFactor(short sPane) 
{
	return GetPane(sPane)->GetZoomFactor();
}

//==============================================================================
//
// 	Function Name:	CTMLead::GetZoomState()
//
// 	Description:	This function is called to retrieve the zoom state 
//					current image.
//
// 	Returns:		The current zoom state
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::GetZoomState(short sPane) 
{
	return GetPane(sPane)->GetZoomState();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::HolePunchRemove()
//
// 	Description:	This external method allows the caller to remove hole  
//					punches from 1-bit images.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::HolePunchRemove(short sPane, long lMinWidth, long lMinHeight,
							       long lMaxWidth, long lMaxHeight, long lLocation) 
{
	return GetPane(sPane)->HolePunchRemove(lMinWidth, lMinHeight, lMaxWidth, 
										   lMaxHeight, lLocation);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::HolePunchRemove2()
//
// 	Description:	This external method allows the caller to remove hole  
//					punches from 1-bit images.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::HolePunchRemove2(short sPane, long lMinHoles, long lMaxHoles,
							        long lLocation) 
{
	return GetPane(sPane)->HolePunchRemove2(lMinHoles, lMaxHoles, lLocation);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::IsAnimation()
//
// 	Description:	This function allows the caller to determine if the current
//					image is an animation.
//
// 	Returns:		TRUE if it is an animation.
//
//	Notes:			None
//
//==============================================================================
BOOL CTMViewCtrl::IsAnimation(short sPane) 
{
	return GetPane(sPane)->IsAnimation();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::IsLoaded()
//
// 	Description:	This function allows the caller to determine if a file is
//					currently loaded.
//
// 	Returns:		TRUE if loaded.
//
//	Notes:			None
//
//==============================================================================
BOOL CTMViewCtrl::IsLoaded(short sPane) 
{
	return GetPane(sPane)->IsLoaded();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::IsOldZap()
//
// 	Description:	This function maintains support for zap files created prior
//					to revision 2.1
//
// 	Returns:		TRUE if the file is an old zap file
//
//	Notes:			None
//
//==============================================================================
BOOL CTMViewCtrl::IsOldZap(LPCTSTR lpszFilename) 
{
	long lKey;

	//	Open the file as a standard ini file
	if(!m_Zap.Open(lpszFilename, TMVIEW_SECTION))
		return FALSE;
	
	//	Read the key we use to check the validity of the file
	lKey = m_Zap.ReadLong(KEYCHECK_LINE, 0);

	//	Is the file valid?
	if(lKey != (LONG)ZAP_ORIGINAL_VALIDATE_KEY)
		return FALSE;

	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::IsPlaying()
//
// 	Description:	This function allows the caller to determine if an animation
//					is playing.
//
// 	Returns:		TRUE if it is playing an animation.
//
//	Notes:			None
//
//==============================================================================
BOOL CTMViewCtrl::IsPlaying(short sPane) 
{
	return GetPane(sPane)->IsPlaying();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::LastPage()
//
// 	Description:	This external method allows the caller to go directly to the
//					last page of the file
//
// 	Returns:		TMV_NOERROR if successful. Otherwise, one of the TMV error
//					levels defined in tmvdefs.h
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::LastPage(short sPane) 
{
	return GetPane(sPane)->LastPage();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::LoadFile()
//
// 	Description:	This function is called when the Filename property changes.
//					If we are in user mode, the change is passed to the dialog
//					before it's saved.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			The caller can perform the same action by setting the
//					filename property but no error level is returned unless
//					this method is used.
//
//==============================================================================
short CTMViewCtrl::LoadFile(LPCTSTR lpszFilename, short sPane) 
{
	CTMLead*	pPane = 0;
	CString		strFilename;

	//	Are we in user mode?
	if(AmbientUserMode())
	{
		//	Is the user requesting the active pane?
		if(sPane != TMV_LEFTPANE && sPane != TMV_RIGHTPANE)
			sPane = m_sActivePane;

		strFilename = (lpszFilename != 0) ? lpszFilename : "";
		
		//	Load the file into the requested pane
		if(sPane == TMV_RIGHTPANE)
		{
			m_strRightFile = strFilename;
			pPane = GetPane(TMV_RIGHTPANE);

			return GetPane(TMV_RIGHTPANE)->SetFilename(m_strRightFile);
		}
		else
		{
			m_strLeftFile = strFilename;
			pPane = GetPane(TMV_LEFTPANE);
		}

		if(m_bLoadAsync)
		{
			pPane->m_AsyncParams.strFilename = strFilename;
			pPane->m_AsyncParams.bZap = FALSE;

			//	Have we already set a timer
			if(pPane->m_AsyncParams.uTimer == 0)
			{
				pPane->m_AsyncParams.uTimer = SetTimer(pPane->m_AsyncParams.iTimerId, 250, NULL);
			}

			return TMV_NOERROR;
		}
		else
		{
			return pPane->SetFilename(m_strLeftFile);
		}

	}
	
	return 0;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::LoadOldZap()
//
// 	Description:	This function maintains support for zap files created prior
//					to revision 2.1
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::LoadOldZap(LPCTSTR lpszFilename, BOOL bUseView, 
						      BOOL bScaleView, BOOL bCallouts, short sPane) 
{
	long lKey;

	//	Open the zap file as a standard ini file
	if(!m_Zap.Open(lpszFilename, TMVIEW_SECTION))
	{
		m_Errors.Handle(0, IDS_TMV_ZAPNOTFOUND, lpszFilename);
		return TMV_ZAPNOTFOUND;
	}
	
	//	Read the key we use to check the validity of the file
	lKey = m_Zap.ReadLong(KEYCHECK_LINE, 0);

	//	Is the file valid?
	if(lKey != (LONG)ZAP_ORIGINAL_VALIDATE_KEY)
	{
		m_Errors.Handle(0, IDS_TMV_INVALIDZAP, lpszFilename);
		return TMV_INVALIDZAP;
	}

	//	Load the zap
	return GetPane(sPane)->LoadOldZap(lpszFilename, LEFTPANE_SECTION, bUseView,
							          bScaleView, bCallouts);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::LoadZap()
//
// 	Description:	This external method allows the caller to load the 
//					annotations in the specified file.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::LoadZap(LPCTSTR lpszFilename, BOOL bUseView, 
						   BOOL bScaleView, BOOL bCallouts, short sPane,
						   LPCTSTR lpszSourceFile) 
{
	SZapHeader	Header;
	char		szErrorMsg[256];
	short		sReturn = TMV_NOERROR;

	//	Make sure the file exists
	if(!m_PaneA.FindFile(lpszFilename))
	{
		m_Errors.Handle(0, IDS_TMV_ZAPNOTFOUND, lpszFilename);
		return TMV_ZAPNOTFOUND;
	}
	
	try
	{
		//	Open the zap file
		CFile Zap(lpszFilename, CFile::modeRead);

		//	Read the zap header
		if(Zap.Read((void*)&Header, sizeof(Header)) != sizeof(Header))
		{
			Zap.Close();

			//	Check to see if this is an old zap format
			if(IsOldZap(lpszFilename))
				return LoadOldZap(lpszFilename, bUseView, bScaleView, 
								  bCallouts, sPane);
								   
			m_Errors.Handle(0, IDS_TMV_INVALIDZAP, lpszFilename);
			return TMV_INVALIDZAP;
		}

		//	Is this one of the original zap files?
		if(Header.lVersion != ZAP_NEW_VERSION)
		{
			Zap.Close();

			//	Check to see if this is an old zap format
			if(IsOldZap(lpszFilename))
				return LoadOldZap(lpszFilename, bUseView, bScaleView, 
								  bCallouts, sPane);
								   
			m_Errors.Handle(0, IDS_TMV_INVALIDZAP, lpszFilename);
			return TMV_INVALIDZAP;
		}

		//	Now load the zap into the active pane
		m_bLoadingZap = TRUE;
		if(!GetPane(sPane)->LoadZap(&Zap, &Header, 
											 bUseView, bScaleView,
											 bCallouts, lpszSourceFile))
		{
			Zap.Close();
			m_Errors.Handle(0, IDS_TMV_INVALIDZAP, lpszFilename);
			sReturn = TMV_INVALIDZAP;
		}
		else
		{
			Zap.Close();
			sReturn = TMV_NOERROR;
		}

	}
	//	Catch all file exceptions
	catch(CFileException* pFileException)
	{
		//	Get the error message and pass it on to the handler
		pFileException->GetErrorMessage(szErrorMsg, sizeof(szErrorMsg));
		m_Errors.Handle(0, szErrorMsg);
		pFileException->Delete();

		sReturn = TMV_LOADZAPFAILED;
	}
	//	Catch all other exceptions
	catch(CException* pException)
	{
		//	Get the error message and pass it on to the handler
		pException->GetErrorMessage(szErrorMsg, sizeof(szErrorMsg));
		m_Errors.Handle(0, szErrorMsg);
		pException->Delete();

		sReturn = TMV_LOADZAPFAILED;
	}

	m_bLoadingZap = FALSE;
	return sReturn;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::LockAnn()
//
// 	Description:	This function will lock the specified annotation
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::LockAnn(long lAnnotation, short sPane) 
{
	return GetPane(sPane)->LockAnn((HANNOBJECT)lAnnotation);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::NextPage()
//
// 	Description:	This external method allows the caller to advance to the 
//					next page in the file.
//
// 	Returns:		TMV_NOERROR if successful. Otherwise, one of the TMV error
//					levels defined in tmvdefs.h
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::NextPage(short sPane) 
{
	return GetPane(sPane)->NextPage();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAAnimate()
//
// 	Description:	This function handles the event notification sent from the
//					PaneA control. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAAnimate(BOOL bEnable) 
{
	//	Set the active pane
	SetPane(&m_PaneA);

	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("Animate: Enable = %s", bEnable ? "TRUE" : "FALSE");
		m_Diagnostics.Report("A", Msg);
	#endif

	m_PaneA.OnAnimate(bEnable);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAAnnChange()
//
// 	Description:	This function handles the event notification sent from the
//					PaneA control. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAAnnChange(long hObject, long uType) 
{
	//	Set the active pane
	SetPane(&m_PaneA);

	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("AnnChange: hObject = %ld", hObject);
		m_Diagnostics.Report("A", Msg);
	#endif

	m_PaneA.OnAnnChange(hObject, uType);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAAnnClicked()
//
// 	Description:	This function handles the event notification sent from the
//					PaneA control. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAAnnClicked(long hObject) 
{
	//	Set the active pane
	SetPane(&m_PaneA);

	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("AnnClicked: hObject = %ld", hObject);
		m_Diagnostics.Report("A", Msg);
	#endif
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAAnnCreate()
//
// 	Description:	This function handles the event notification sent from the
//					PaneA control. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAAnnCreate(long hObject) 
{
	//	Set the active pane
	SetPane(&m_PaneA);

	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("AnnCreate: hObject = %ld", hObject);
		m_Diagnostics.Report("A", Msg);
	#endif

	m_PaneA.OnAnnCreate(hObject);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAAnnDestroy()
//
// 	Description:	This function handles the event notification sent from the
//					PaneA control when an annotation is destroyed. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAAnnDestroy(long hObject) 
{
	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("AnnDestroy: hObject = %ld", hObject);
		m_Diagnostics.Report("A", Msg);
	#endif

	m_PaneA.OnAnnDestroy(hObject);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAAnnDrawn()
//
// 	Description:	This function handles the event notification sent from the
//					PaneA control. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAAnnDrawn(long hObject) 
{
	//	Set the active pane
	SetPane(&m_PaneA);

	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("AnnDrawn: hObject = %ld", hObject);
		m_Diagnostics.Report("A", Msg);
	#endif

	m_PaneA.OnAnnDrawn(hObject);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAAnnMenu()
//
// 	Description:	This function handles the AnnMenu event from PaneA
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAAnnMenu(LPDISPATCH AnnMenu) 
{
	#if defined _EVENT_DIAGNOSTICS
		m_Diagnostics.Report("A", "AnnMenu");
	#endif
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAAnnMouseDown()
//
// 	Description:	This function will set up rubberbanding if the current 
//					action is to ZOOM.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAAnnMouseDown(short Button, short Shift, long X, long Y) 
{
	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("AnnMouseDown: Button = %d Shift = %d X = %ld Y = %ld", Button, Shift, X, Y);
		m_Diagnostics.Report("A", Msg);
	#endif

	//	Set the active pane
	SetPane(&m_PaneA);

	//	Let the TMLead handle the event
	if(IsWindow(m_PaneA.m_hWnd))
		m_PaneA.OnAnnMouseDown(Button, Shift, X, Y);

	//	Notify the container
	POINT Screen;
	Screen.x = X;
	Screen.y = Y;
	m_PaneA.ClientToScreen(&Screen);
	FireMouseDown(Button, Shift, Screen.x, Screen.y);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAAnnMouseMove()
//
// 	Description:	This function will handle mouse move events fired by the
//					PaneA control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAAnnMouseMove(short Button, short Shift, long X, long Y) 
{
	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("AnnMouseMove: Button = %d Shift = %d X = %ld Y = %ld", Button, Shift, X, Y);
		m_Diagnostics.Report("A", Msg);
	#endif

	//	Notify the container
	POINT Screen;
	Screen.x = X;
	Screen.y = Y;
	m_PaneA.ClientToScreen(&Screen);
	FireMouseMove(Button, Shift, Screen.x, Screen.y);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAAnnMouseUp()
//
// 	Description:	This function will handle mouse up events fired by the
//					PaneA object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAAnnMouseUp(short Button, short Shift, long X, long Y) 
{
	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("AnnMouseUp: Button = %d Shift = %d X = %ld Y = %ld", Button, Shift, X, Y);
		m_Diagnostics.Report("A", Msg);
	#endif

	//	Let the TMLead handle the event
	if(IsWindow(m_PaneA.m_hWnd))
		m_PaneA.OnAnnMouseUp(Button, Shift, X, Y);


	//	Notify the container
	POINT Screen;
	Screen.x = X;
	Screen.y = Y;
	m_PaneA.ClientToScreen(&Screen);
	FireMouseUp(Button, Shift, Screen.x, Screen.y);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAAnnSelect()
//
// 	Description:	This function handles the event notification sent from the
//					PaneA control when the user selects a new annotation 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAAnnSelect(const VARIANT FAR& aObjects, short uCount) 
{
	//	Set the active pane
	SetPane(&m_PaneA);

	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("AnnSelect: uCount = %d", uCount);
		m_Diagnostics.Report("A", Msg);
	#endif

	m_PaneA.OnAnnSelect();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAAnnUserMenu()
//
// 	Description:	This function handles the AnnUserMenu from PaneA
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAAnnUserMenu(long nID) 
{
	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("AnnUserMenu: nID = %ld", nID);
		m_Diagnostics.Report("A", Msg);
	#endif
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnActionChanged()
//
// 	Description:	This function is called when the operator action property
//					is changed.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnActionChanged() 
{
	SetModifiedFlag();
	
	//	Clear the text box counter
	m_iTextOpen = 0;

	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetAction(m_sAction);
		m_PaneB.SetAction(m_sAction);
	}
	else
	{
		GetPane()->SetAction(m_sAction);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnActivePaneChanged()
//
// 	Description:	This function is called when the ActivePane property changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnActivePaneChanged() 
{
	if(AmbientUserMode())
	{
		//	Which pane is active
		m_pActive = GetPane(m_sActivePane);

		//	Are we in split screen mode?
		if(m_bSplitScreen)
		{
			//	Erase the inactive highlight
			EraseSplitFrame((m_sActivePane == TMV_LEFTPANE) ? &m_rcRFrame : 
															  &m_rcLFrame);

			//	Draw the active highlight
			DrawSplitFrame((m_sActivePane == TMV_LEFTPANE) ? &m_rcLFrame : 
															 &m_rcRFrame);
		}

		//	Notify the container
		FireSelectPane(m_sActivePane);
	}
	
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAKeyDown()
//
// 	Description:	This function will trap the KeyDown event on PaneA
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAKeyDown(short* KeyCode, short Shift) 
{
	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("KeyDown: KeyCode = %d Shift = %d", *KeyCode, Shift);
		//m_Diagnostics.Report("A", Msg);
	#endif
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAKeyPress()
//
// 	Description:	This function will trap the KeyPress event on PaneA
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAKeyPress(short* KeyAscii) 
{
	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("KeyPress: KeyAscii = %d", *KeyAscii);
		//m_Diagnostics.Report("A", Msg);
	#endif
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAKeyUp()
//
// 	Description:	This function will trap the KeyUp event on PaneA
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAKeyUp(short* KeyCode, short Shift) 
{
	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("KeyUp: KeyCode = %d Shift = %d", *KeyCode, Shift);
		m_Diagnostics.Report("A", Msg);
	#endif

	//	Let the TMLead handle the event
	if(IsWindow(m_PaneA.m_hWnd))
		m_PaneA.OnKeyUp(KeyCode, Shift);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAMouseClick()
//
// 	Description:	This function will respond to a click on the PaneA control
//					by firing a MouseClick event.
//
// 	Returns:		None
//
//	Notes:			This event is not fired if the current action is ZOOM and 
//					it's the left mouse button.
//
//==============================================================================
void CTMViewCtrl::OnAMouseClick() 
{
	//	Set the active pane
	SetPane(&m_PaneA);

	#if defined _EVENT_DIAGNOSTICS
		m_Diagnostics.Report("A", "MouseClick");
	#endif

	if(!IsWindow(m_PaneA.m_hWnd))
		return;

	//	Don't fire an event if the TMLead is zooming the image
	if(m_PaneA.m_sAction == ZOOM && m_sButton == LEFT_MOUSEBUTTON)
		return;

	//	Is this one of the unwanted LeadTools events?
	//
	//	See notes in OnOpenTextBox()
	if(m_iTextOpen > 0)
		m_iTextOpen--;
	else
		FireMouseClick(m_sButton, m_sKey);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAMouseDblClick()
//
// 	Description:	This function will respond to a double click on the PaneA
//					control by firing a MouseDblClick event.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAMouseDblClick() 
{
	if(!IsWindow(m_PaneA.m_hWnd))
		return;

	//	Set the active pane
	SetPane(&m_PaneA);

	#if defined _EVENT_DIAGNOSTICS
		m_Diagnostics.Report("A", "MouseDblClick");
	#endif

	//	Let the TMLead handle the event
	m_PaneA.OnMouseDblClick();

	//	Fire an event
	FireMouseDblClick(m_sButton, m_sKey);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAMouseDown()
//
// 	Description:	This function will set up rubberbanding if the current 
//					action is to ZOOM.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAMouseDown(short Button, short Shift, long X, long Y) 
{
	//	Save the key identifier and button identifier
	m_sButton = Button;
	m_sKey    = Shift;

	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("MouseDown: Button = %d Shift = %d X = %ld Y = %ld", Button, Shift, X, Y);
		m_Diagnostics.Report("A", Msg);
	#endif

	//	Set the active pane
	SetPane(&m_PaneA);

	//	Let the TMLead handle the event
	if(IsWindow(m_PaneA.m_hWnd))
		m_PaneA.OnMouseDown(Button, Shift, X, Y);

	//	Notify the container
	POINT point;
	point.x = X;
	point.y = Y;
	m_PaneA.ClientToScreen(&point);
	ScreenToClient(&point);
	FireMouseDown(Button, Shift, point.x, point.y);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAMouseMove()
//
// 	Description:	This function will handle mouse move events fired by the
//					PaneA control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAMouseMove(short Button, short Shift, long X, long Y) 
{
	//	Save the key identifier and button identifier
	m_sButton = Button;
	m_sKey    = Shift;

	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("MouseMove: Button = %d Shift = %d X = %ld Y = %ld", Button, Shift, X, Y);
		m_Diagnostics.Report("A", Msg);
	#endif

	//	Let the TMLead handle the event
	if(IsWindow(m_PaneA.m_hWnd))
		m_PaneA.OnMouseMove(Button, Shift, X, Y);

	//	Don't fire an event if the TMLead is zooming the image
	if(m_PaneA.GetAction() != ZOOM)
	{
		POINT point;
		point.x = X;
		point.y = Y;
		m_PaneA.ClientToScreen(&point);
		ScreenToClient(&point);
		FireMouseMove(Button, Shift, point.x, point.y);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAMouseUp()
//
// 	Description:	This function will handle mouse up events fired by the
//					PaneA object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAMouseUp(short Button, short Shift, long X, long Y) 
{
	//	Set the active pane
	SetPane(&m_PaneA);

	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("MouseUp: Button = %d Shift = %d X = %ld Y = %ld", Button, Shift, X, Y);
		m_Diagnostics.Report("A", Msg);
	#endif

	//	Let the TMLead handle the event
	if(IsWindow(m_PaneA.m_hWnd))
		m_PaneA.OnMouseUp(Button, Shift, X, Y);

	//	Notify the container
	POINT point;
	point.x = X;
	point.y = Y;
	m_PaneA.ClientToScreen(&point);
	ScreenToClient(&point);
	FireMouseUp(Button, Shift, point.x, point.y);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAnnColorChanged()
//
// 	Description:	This function is called when the annotation color property 
//					is changed. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAnnColorChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetAnnColor(m_sAnnColor);
		m_PaneB.SetAnnColor(m_sAnnColor);
	}
	else
	{
		GetPane()->SetAnnColor(m_sAnnColor);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAnnColorDepthChanged()
//
// 	Description:	This function is called when the annotation color depth 
//					property is changed. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAnnColorDepthChanged() 
{
	SetModifiedFlag();

	if(!AmbientUserMode())
	{
		return;
	}
	else
	{
		//	Set the property in the scratch pane
		m_Scratch.SetAnnColorDepth(m_sAnnColorDepth);

		if(m_bSyncPanes)
		{
			m_PaneA.SetAnnColorDepth(m_sAnnColorDepth);
			m_PaneB.SetAnnColorDepth(m_sAnnColorDepth);
		}
		else
		{
			GetPane()->SetAnnColorDepth(m_sAnnColorDepth);
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAnnFontBoldChanged()
//
// 	Description:	This function is called when the AnnFontBold property 
//					is changed. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAnnFontBoldChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetAnnFontBold(m_bAnnFontBold);
		m_PaneB.SetAnnFontBold(m_bAnnFontBold);
	}
	else
	{
		GetPane()->SetAnnFontBold(m_bAnnFontBold);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAnnFontChanged()
//
// 	Description:	This function is called by one of the TMLead controls when
//					the user changes the annotation text properties via the
//					text annotation dialog box.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAnnFontChanged(CTMLead* pTMLead) 
{
	CTMLead* pSync;

	//	Update the control properties
	m_strAnnFontName = pTMLead->m_strAnnFontName;
	m_sAnnFontSize = pTMLead->m_sAnnFontSize;
	m_bAnnFontBold = pTMLead->m_bAnnFontBold;
	m_bAnnFontStrikeThrough = pTMLead->m_bAnnFontStrikeThrough;
	m_bAnnFontUnderline = pTMLead->m_bAnnFontUnderline;

	//	Update the other pane if we are syncing panes
	if(m_bSyncPanes)
	{
		pSync = (pTMLead == &m_PaneA) ? &m_PaneB : &m_PaneA;

		pSync->SetAnnFontName(pTMLead->m_strAnnFontName);
		pSync->SetAnnFontSize(pTMLead->m_sAnnFontSize);
		pSync->SetAnnFontBold(pTMLead->m_bAnnFontBold);
		pSync->SetAnnFontStrikeThrough(pTMLead->m_bAnnFontStrikeThrough);
		pSync->SetAnnFontUnderline(pTMLead->m_bAnnFontUnderline);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAnnFontNameChanged()
//
// 	Description:	This function is called when the AnnFontName property 
//					is changed. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAnnFontNameChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetAnnFontName(m_strAnnFontName);
		m_PaneB.SetAnnFontName(m_strAnnFontName);
	}
	else
	{
		GetPane()->SetAnnFontName(m_strAnnFontName);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAnnFontSizeChanged()
//
// 	Description:	This function is called when the AnnFontSize property 
//					is changed. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAnnFontSizeChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetAnnFontSize(m_sAnnFontSize);
		m_PaneB.SetAnnFontSize(m_sAnnFontSize);
	}
	else
	{
		GetPane()->SetAnnFontSize(m_sAnnFontSize);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAnnFontStrikeThroughChanged()
//
// 	Description:	This function is called when the AnnFontStrikeThrough property 
//					is changed. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAnnFontStrikeThroughChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetAnnFontStrikeThrough(m_bAnnFontStrikeThrough);
		m_PaneB.SetAnnFontStrikeThrough(m_bAnnFontStrikeThrough);
	}
	else
	{
		GetPane()->SetAnnFontStrikeThrough(m_bAnnFontStrikeThrough);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAnnFontUnderlineChanged()
//
// 	Description:	This function is called when the AnnFontUnderline property 
//					is changed. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAnnFontUnderlineChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetAnnFontUnderline(m_bAnnFontUnderline);
		m_PaneB.SetAnnFontUnderline(m_bAnnFontUnderline);
	}
	else
	{
		GetPane()->SetAnnFontUnderline(m_bAnnFontUnderline);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAnnotateCalloutsChanged()
//
// 	Description:	This function is called when the AnnotateCallouts
//					property is changed. The change is passed to the TMLead 
//					object
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAnnotateCalloutsChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetAnnotateCallouts(m_bAnnotateCallouts);
		m_PaneB.SetAnnotateCallouts(m_bAnnotateCallouts);
	}
	else
	{
		GetPane()->SetAnnotateCallouts(m_bAnnotateCallouts);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAnnotationDeleted()
//
// 	Description:	This function is called by the CTMLead object when the user
//					deletes an annotation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAnnotationDeleted(CTMLead* pPane, long lAnnotation) 
{
	//	Fire an event
	FireAnnotationDeleted(lAnnotation, GetPaneId(pPane));

	#if defined _EVENT_DIAGNOSTICS
		m_Diagnostics.Report("F", "FireAnnotationDeleted");
	#endif

}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAnnotationDrawn()
//
// 	Description:	This function is called by the CTMLead object when the user
//					draws an annotation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAnnotationDrawn(CTMLead* pPane, long lAnnotation) 
{
	//	Fire an event
	FireAnnotationDrawn(lAnnotation, GetPaneId(pPane));

	#if defined _EVENT_DIAGNOSTICS
		m_Diagnostics.Report("F", "FireAnnotationDrawn");
	#endif

}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAnnotationModified()
//
// 	Description:	This function is called by the CTMLead object when the user
//					modifies an annotation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAnnotationModified(CTMLead* pPane, long lAnnotation) 
{
	//	Fire an event
	FireAnnotationModified(lAnnotation, GetPaneId(pPane));

	#if defined _EVENT_DIAGNOSTICS
		m_Diagnostics.Report("F", "FireAnnotationModified");
	#endif

}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAnnThicknessChanged()
//
// 	Description:	This function is called when the annotation thickness
//					property is changed. The change is passed to the TMLead 
//					object
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAnnThicknessChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetAnnThickness(m_sAnnThickness);
		m_PaneB.SetAnnThickness(m_sAnnThickness);
	}
	else
	{
		GetPane()->SetAnnThickness(m_sAnnThickness);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAnnToolChanged()
//
// 	Description:	This function is called when the annotation tool property is
//					changed. The change is passed to the TMLead object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAnnToolChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetAnnTool(m_sAnnTool);
		m_PaneB.SetAnnTool(m_sAnnTool);
	}
	else
	{
		GetPane()->SetAnnTool(m_sAnnTool);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnARubberBand()
//
// 	Description:	This function is called when the user completes a rubber
//					banding operation. It will zoom the image to the selection
//					rectangle.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnARubberBand() 
{
	//	Set the active pane
	SetPane(&m_PaneA);

	#if defined _EVENT_DIAGNOSTICS
		m_Diagnostics.Report("A", "RubberBand");
	#endif

	m_PaneA.OnRubberBand();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnAutoAnimateChanged()
//
// 	Description:	This function informs the TMLead object when the 
//					AutoAnimate property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnAutoAnimateChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetAutoAnimate(m_bAutoAnimate);
		m_PaneB.SetAutoAnimate(m_bAutoAnimate);
	}
	else
	{
		GetPane()->SetAutoAnimate(m_bAutoAnimate);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBackColorChanged()
//
// 	Description:	This function is called when the background color property
//					is changed. It will set the background color of the lead
//					control to match.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBackColorChanged() 
{
	COleControl::OnBackColorChanged();
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetBackColor(GetBackColor(),TranslateColor(GetBackColor()));
		m_PaneB.SetBackColor(GetBackColor(),TranslateColor(GetBackColor()));
	}
	else
	{
		GetPane()->SetBackColor(GetBackColor(),TranslateColor(GetBackColor()));
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBackgroundFileChanged()
//
// 	Description:	This function is called when the name of the background
//					file is changed.
//
// 	Returns:		None
//
//	Notes:			This property is not yet implemented.
//
//==============================================================================
void CTMViewCtrl::OnBackgroundFileChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBAnimate()
//
// 	Description:	This function handles the event notification sent from the
//					PaneB control. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBAnimate(BOOL bEnable) 
{
	//	Set the active pane
	SetPane(&m_PaneB);

	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("Animate: Enable = %s", bEnable ? "TRUE" : "FALSE");
		m_Diagnostics.Report("B", Msg);
	#endif

	m_PaneB.OnAnimate(bEnable);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBAnnChange()
//
// 	Description:	This function handles the event notification sent from the
//					PaneB control. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBAnnChange(long hObject, long uType) 
{
	//	Set the active pane
	SetPane(&m_PaneB);

	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("AnnChange: hObject = %ld", hObject);
		m_Diagnostics.Report("B", Msg);
	#endif

	m_PaneB.OnAnnChange(hObject, uType);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBAnnClicked()
//
// 	Description:	This function handles the event notification sent from the
//					PaneB control. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBAnnClicked(long hObject) 
{
	//	Set the active pane
	SetPane(&m_PaneB);

	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("AnnClicked: hObject = %ld", hObject);
		m_Diagnostics.Report("B", Msg);
	#endif
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBAnnCreate()
//
// 	Description:	This function handles the event notification sent from the
//					PaneB control. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBAnnCreate(long hObject) 
{
	//	Set the active pane
	SetPane(&m_PaneB);

	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("AnnCreate: hObject = %ld", hObject);
		m_Diagnostics.Report("B", Msg);
	#endif

	m_PaneB.OnAnnCreate(hObject);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBAnnDestroy()
//
// 	Description:	This function handles the event notification sent from the
//					PaneB control when an annotation is destroyed. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBAnnDestroy(long hObject) 
{
	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("AnnDestroy: hObject = %ld", hObject);
		m_Diagnostics.Report("B", Msg);
	#endif

	m_PaneB.OnAnnDestroy(hObject);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBAnnDraw()
//
// 	Description:	This function handles the event notification sent from the
//					PaneB control. 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBAnnDrawn(long hObject) 
{
	//	Set the active pane
	SetPane(&m_PaneB);

	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("AnnDrawn: hObject = %ld", hObject);
		m_Diagnostics.Report("B", Msg);
	#endif

	m_PaneB.OnAnnDrawn(hObject);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBAnnMenu()
//
// 	Description:	This function handles the AnnMenu event from PaneB
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBAnnMenu(LPDISPATCH AnnMenu) 
{
	#if defined _EVENT_DIAGNOSTICS
		m_Diagnostics.Report("B", "AnnMenu");
	#endif
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBAnnMouseDown()
//
// 	Description:	This function will set up rubberbanding if the current 
//					action is to ZOOM.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBAnnMouseDown(short Button, short Shift, long X, long Y) 
{
	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("AnnMouseDown: Button = %d Shift = %d X = %ld Y = %ld", Button, Shift, X, Y);
		m_Diagnostics.Report("B", Msg);
	#endif

	//	Set the active pane
	SetPane(&m_PaneB);

	//	Let the TMLead handle the event
	if(IsWindow(m_PaneB.m_hWnd))
		m_PaneB.OnAnnMouseDown(Button, Shift, X, Y);

	//	Notify the container
	POINT Screen;
	Screen.x = X;
	Screen.y = Y;
	m_PaneB.ClientToScreen(&Screen);
	FireMouseDown(Button, Shift, Screen.x, Screen.y);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBAnnMouseMove()
//
// 	Description:	This function will handle mouse move events fired by the
//					PaneA control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBAnnMouseMove(short Button, short Shift, long X, long Y) 
{
	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("AnnMouseMove: Button = %d Shift = %d X = %ld Y = %ld", Button, Shift, X, Y);
		m_Diagnostics.Report("B", Msg);
	#endif

	//	Notify the container
	POINT Screen;
	Screen.x = X;
	Screen.y = Y;
	m_PaneB.ClientToScreen(&Screen);
	FireMouseMove(Button, Shift, Screen.x, Screen.y);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBAnnMouseUp()
//
// 	Description:	This function will handle mouse up events fired by the
//					PaneA object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBAnnMouseUp(short Button, short Shift, long X, long Y) 
{
	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("AnnMouseUp: Button = %d Shift = %d X = %ld Y = %ld", Button, Shift, X, Y);
		m_Diagnostics.Report("B", Msg);
	#endif

	//	Let the TMLead handle the event
	if(IsWindow(m_PaneA.m_hWnd))
		m_PaneA.OnAnnMouseUp(Button, Shift, X, Y);

	//	Notify the container
	POINT Screen;
	Screen.x = X;
	Screen.y = Y;
	m_PaneB.ClientToScreen(&Screen);
	FireMouseUp(Button, Shift, Screen.x, Screen.y);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBAnnSelect()
//
// 	Description:	This function handles the event notification sent from the
//					PaneB control when the user selects a new annotation 
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBAnnSelect(const VARIANT FAR& aObjects, short uCount) 
{
	//	Set the active pane
	SetPane(&m_PaneB);

	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("AnnSelect: uCount = %d", uCount);
		m_Diagnostics.Report("B", Msg);
	#endif

	m_PaneB.OnAnnSelect();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBAnnUserMenu()
//
// 	Description:	This function handles the AnnUserMenu from PaneB
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBAnnUserMenu(long nID) 
{
	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("AnnUserMenu: nID = %ld", nID);
		m_Diagnostics.Report("B", Msg);
	#endif
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBitonalScalingChanged()
//
// 	Description:	This function is called when the bitonal scaling property is
//					changed. The change is passed to the TMLead object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBitonalScalingChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetBitonal(m_sBitonal);
		m_PaneB.SetBitonal(m_sBitonal);
	}
	else
	{
		GetPane()->SetBitonal(m_sBitonal);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBKeyDown()
//
// 	Description:	This function will trap the KeyDown event on PaneB
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBKeyDown(short* KeyCode, short Shift) 
{
	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("KeyDown: KeyCode = %d Shift = %d", *KeyCode, Shift);
		//m_Diagnostics.Report("B", Msg);
	#endif
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBKeyPress()
//
// 	Description:	This function will trap the KeyPress event on PaneB
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBKeyPress(short* KeyAscii) 
{
	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("KeyPress: KeyAscii = %d", *KeyAscii);
		//m_Diagnostics.Report("B", Msg);
	#endif
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBKeyUp()
//
// 	Description:	This function will trap the KeyUp event on PaneB
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBKeyUp(short* KeyCode, short Shift) 
{
	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("KeyUp: KeyCode = %d Shift = %d", *KeyCode, Shift);
		//m_Diagnostics.Report("B", Msg);
	#endif

	//	Let the TMLead handle the event
	if(IsWindow(m_PaneB.m_hWnd))
		m_PaneB.OnKeyUp(KeyCode, Shift);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBMouseClick()
//
// 	Description:	This function will respond to a click on the PaneB control
//					by firing a MouseClick event.
//
// 	Returns:		None
//
//	Notes:			This event is not fired if the current action is ZOOM and 
//					it's the left mouse button.
//
//==============================================================================
void CTMViewCtrl::OnBMouseClick() 
{
	//	Set the active pane
	SetPane(&m_PaneB);

	#if defined _EVENT_DIAGNOSTICS
		m_Diagnostics.Report("B", "MouseClick");
	#endif

	if(!IsWindow(m_PaneB.m_hWnd))
		return;

	//	Don't fire an event if the TMLead is zooming the image
	if(m_PaneB.m_sAction == ZOOM && m_sButton == LEFT_MOUSEBUTTON)
		return;

	//	Is this one of the unwanted LeadTools events?
	//
	//	See notes in OnOpenTextBox()
	if(m_iTextOpen > 0)
		m_iTextOpen--;
	else
		FireMouseClick(m_sButton, m_sKey);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBMouseDblClick()
//
// 	Description:	This function will respond to a double click on the PaneB
//					control by firing a MouseDblClick event.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBMouseDblClick() 
{
	if(!IsWindow(m_PaneB.m_hWnd))
		return;

	//	Set the active pane
	SetPane(&m_PaneB);

	#if defined _EVENT_DIAGNOSTICS
		m_Diagnostics.Report("B", "MouseDblClick");
	#endif

	//	Let the TMLead handle the event
	m_PaneB.OnMouseDblClick();

	//	Fire an event
	FireMouseDblClick(m_sButton, m_sKey);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBMouseDown()
//
// 	Description:	This function will set up rubberbanding if the current 
//					action is to ZOOM.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBMouseDown(short Button, short Shift, long X, long Y) 
{
	//	Save the key identifier and button identifier
	m_sButton = Button;
	m_sKey    = Shift;

	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("MouseDown: Button = %d Shift = %d X = %ld Y = %ld", Button, Shift, X, Y);
		m_Diagnostics.Report("B", Msg);
	#endif

	//	Set the active pane
	SetPane(&m_PaneB);

	//	Let the TMLead handle the event
	if(IsWindow(m_PaneB.m_hWnd))
		m_PaneB.OnMouseDown(Button, Shift, X, Y);

	//	Notify the container
	POINT point;
	point.x = X;
	point.y = Y;
	m_PaneB.ClientToScreen(&point);
	ScreenToClient(&point);
	FireMouseDown(Button, Shift, point.x, point.y);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBMouseMove()
//
// 	Description:	This function will handle mouse move events fired by the
//					PaneB control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBMouseMove(short Button, short Shift, long X, long Y) 
{
	//	Save the key identifier and button identifier
	m_sButton = Button;
	m_sKey    = Shift;

	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("MouseMove: Button = %d Shift = %d X = %ld Y = %ld", Button, Shift, X, Y);
		m_Diagnostics.Report("B", Msg);
	#endif

	//	Let the TMLead handle the event
	if(IsWindow(m_PaneB.m_hWnd))
		m_PaneB.OnMouseMove(Button, Shift, X, Y);

	//	Don't fire an event if the TMLead is zooming the image
	if(m_PaneB.GetAction() != ZOOM)
	{
		POINT point;
		point.x = X;
		point.y = Y;
		m_PaneB.ClientToScreen(&point);
		ScreenToClient(&point);
		FireMouseMove(Button, Shift, point.x, point.y);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBMouseUp()
//
// 	Description:	This function will handle mouse up events fired by the
//					PaneB object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBMouseUp(short Button, short Shift, long X, long Y) 
{
	//	Set the active pane
	SetPane(&m_PaneB);

	#if defined _EVENT_DIAGNOSTICS
		CString Msg;
		Msg.Format("MouseUp: Button = %d Shift = %d X = %ld Y = %ld", Button, Shift, X, Y);
		m_Diagnostics.Report("B", Msg);
	#endif

	//	Let the TMLead handle the event
	if(IsWindow(m_PaneB.m_hWnd))
		m_PaneB.OnMouseUp(Button, Shift, X, Y);

	//	Notify the container
	POINT point;
	point.x = X;
	point.y = Y;
	m_PaneB.ClientToScreen(&point);
	ScreenToClient(&point);
	FireMouseUp(Button, Shift, point.x, point.y);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnBRubberBand()
//
// 	Description:	This function is called when the user completes a rubber
//					banding operation. It will zoom the image to the selection
//					rectangle.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnBRubberBand() 
{
	//	Set the active pane
	SetPane(&m_PaneB);

	#if defined _EVENT_DIAGNOSTICS
		m_Diagnostics.Report("B", "RubberBand");
	#endif

	m_PaneB.OnRubberBand();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnCalloutActivated()
//
// 	Description:	This function is called by the CTMLead object when one of
//					it's callouts is activated.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnCalloutActivated(CTMLead* pSource, CCallout* pCallout) 
{
	//	Notify the host control
	if((pSource != 0) && IsWindow(pSource->m_hWnd))
	{
		//	Make sure the correct pane is active
		SetPane(pSource);

		if((pCallout != 0) && IsWindow(pCallout->m_hWnd))
		{
			if(GetPane(TMV_RIGHTPANE) == pSource)
				FireSelectCallout((OLE_HANDLE)pCallout->m_hWnd, TMV_RIGHTPANE);
			else
				FireSelectCallout((OLE_HANDLE)pCallout->m_hWnd, TMV_LEFTPANE);
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnCalloutColorChanged()
//
// 	Description:	This function is called when the Color color property is
//					changed. The change is passed to the TMLead object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnCalloutColorChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetCalloutColor(m_sCalloutColor);
		m_PaneB.SetCalloutColor(m_sCalloutColor);
	}
	else
	{
		GetPane()->SetCalloutColor(m_sCalloutColor);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnCalloutCreated()
//
// 	Description:	This function is called by the CTMLead object when a new
//					callout is created. It will fire a CreateCallout event.
//
// 	Returns:		None
//
//	Notes:			This event is fired AFTER the callout is created
//
//==============================================================================
void CTMViewCtrl::OnCalloutCreated(CTMLead* pSource, CCallout* pCallout) 
{
	//	Fire an event
	if(pCallout && IsWindow(pCallout->m_hWnd))
		FireCreateCallout((OLE_HANDLE)pCallout->m_hWnd);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnCalloutDestroyed()
//
// 	Description:	This function is called by the CTMLead object when a 
//					callout is destroyed. It will fire a DestroyCallout event.
//
// 	Returns:		None
//
//	Notes:			This event is fired BEFORE the callout is destroyed
//
//==============================================================================
void CTMViewCtrl::OnCalloutDestroyed(CTMLead* pSource, CCallout* pCallout) 
{
	//	Fire an event
	if(pCallout && IsWindow(pCallout->m_hWnd))
		FireDestroyCallout((OLE_HANDLE)pCallout->m_hWnd);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnCalloutFrameColorChanged()
//
// 	Description:	This function is called when the callout frame color
//					property is changed. The change is passed to the TMLead 
//					object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnCalloutFrameColorChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetCallFrameColor(m_sCalloutFrameColor);
		m_PaneB.SetCallFrameColor(m_sCalloutFrameColor);
	}
	else
	{
		GetPane()->SetCallFrameColor(m_sCalloutFrameColor);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnCalloutFrameThicknessChanged()
//
// 	Description:	This function is called when the callout frame thickness
//					property is changed. The change is passed to the TMLead 
//					object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnCalloutFrameThicknessChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetCallFrameThickness(m_sCalloutFrameThickness);
		m_PaneB.SetCallFrameThickness(m_sCalloutFrameThickness);
	}
	else
	{
		GetPane()->SetCallFrameThickness(m_sCalloutFrameThickness);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnCalloutHandleColorChanged()
//
// 	Description:	This function is called when the callout frame color
//					property is changed. The change is passed to the TMLead 
//					object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnCalloutHandleColorChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetCallHandleColor(m_sCalloutHandleColor);
		m_PaneB.SetCallHandleColor(m_sCalloutHandleColor);
	}
	else
	{
		GetPane()->SetCallHandleColor(m_sCalloutHandleColor);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnCalloutMoved()
//
// 	Description:	This function is called by the CTMLead object when the user
//					moves a callout.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnCalloutMoved(CTMLead* pPane, CCallout* pCallout) 
{
	//	Fire an event
	if(pCallout && IsWindow(pCallout->m_hWnd))
	{
		FireCalloutMoved((OLE_HANDLE)pCallout->m_hWnd, GetPaneId(pPane));

		#if defined _EVENT_DIAGNOSTICS
			m_Diagnostics.Report("F", "FireCalloutMoved");
		#endif

	}

}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnCalloutResized()
//
// 	Description:	This function is called by the CTMLead object when the user
//					resizes a callout.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnCalloutResized(CTMLead* pPane, CCallout* pCallout) 
{
	//	Fire an event
	if(pCallout && IsWindow(pCallout->m_hWnd))
	{
		FireCalloutResized((OLE_HANDLE)pCallout->m_hWnd, GetPaneId(pPane));

		#if defined _EVENT_DIAGNOSTICS
			m_Diagnostics.Report("F", "FireCalloutResized");
		#endif

	}

}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnCalloutShadeGrayscaleChanged()
//
// 	Description:	This function is called when the CutOutBackgroundColor
//					property is changed. The change is passed to the TMLead 
//					object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnCalloutShadeGrayscaleChanged() 
{
	COLORREF crGrayscale;
	
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else
	{
		//	Make sure the new value is within range
		if(m_sCalloutShadeGrayscale < 0)
			m_sCalloutShadeGrayscale = 0;
		else if(m_sCalloutShadeGrayscale > 255)
			m_sCalloutShadeGrayscale = 255;

		crGrayscale = RGB(m_sCalloutShadeGrayscale, 
						  m_sCalloutShadeGrayscale, 
						  m_sCalloutShadeGrayscale);

		if(m_bSyncPanes)
		{
			m_PaneA.SetCalloutShadeColor(crGrayscale);
			m_PaneB.SetCalloutShadeColor(crGrayscale);
		}
		else
		{
			GetPane()->SetCalloutShadeColor(crGrayscale);
		}
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnClickCallout()
//
// 	Description:	This function will handles notifications from a pane when
//					the user clicks in one of its callouts.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnClickCallout(CTMLead* pTMLead, CCallout* pCallout, 
								 short sButton, short sKey) 
{
	//	Fire an event
	FireMouseClick(sButton, sKey);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnCloseTextBox()
//
// 	Description:	This function is called by one of the TMLead controls when
//					the user closes the dialog box to enter the text for an
//					annotation.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnCloseTextBox(CTMLead* pTMLead) 
{
	//	Fire the appropriate event
	FireCloseTextBox(m_sActivePane);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnCreate()
//
// 	Description:	This function will create and size the dialog box containing
//					the lead control after the control window is created.
//
// 	Returns:		0 if successful
//
//	Notes:			None
//
//==============================================================================
int CTMViewCtrl::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
	//	Perform base class construction first
	if(COleControl::OnCreate(lpCreateStruct) == -1)
		return -1;
	
	//	Initialize the error handler
	m_Errors.Enable(m_bEnableErrors);
	m_Errors.SetParent(m_hWnd);
	m_Errors.SetTitle(TMVERRORS_TITLE);
	m_Errors.SetMessageId(m_bEnableAxErrors == TRUE ? WM_ERROR_EVENT : 0);
	
	//	Create the left and right panes
	if(!m_PaneA.Create(this, IDC_PANEA))
		return -1;
	if(!m_PaneB.Create(this, IDC_PANEB))
		return -1;
	
	//	Create the scratch pane
	if(!m_Scratch.Create(this, IDC_BACKUP))
	{
		m_Errors.Handle(0, IDS_TMV_NOBACKUP);
	}

	//	Set the pointer to the control window. We have to do this as a separate
	//	operation because not all parents of CTMLead will be TMView controls
	m_PaneA.SetControl(this, &m_Errors);
	m_PaneB.SetControl(this, &m_Errors);
	m_Scratch.SetControl(this, &m_Errors);

	//	Make visibility for each pane
	m_PaneA.ShowWindow(SW_SHOW);
	m_PaneB.ShowWindow(SW_HIDE);

	//	Initialize the scratch pane
	m_Scratch.ShowWindow(SW_HIDE);
	m_Scratch.MoveWindow(0,0,1,1);
	m_Scratch.SetAction(NONE);

	//	Set these properties
	m_PaneA.SetEnabled(GetEnabled());
	m_PaneA.SetBackColor(GetBackColor(), TranslateColor(GetBackColor()));
	m_PaneB.SetEnabled(GetEnabled());
	m_PaneB.SetBackColor(GetBackColor(), TranslateColor(GetBackColor()));
	m_Scratch.SetBackColor(GetBackColor(), TranslateColor(GetBackColor()));

	//	Initialize the asynchronous file loading parameters
	m_PaneA.m_AsyncParams.bCallouts = FALSE;
	m_PaneA.m_AsyncParams.bScaleView = FALSE;
	m_PaneA.m_AsyncParams.bUseView = FALSE;
	m_PaneA.m_AsyncParams.iTimerId = ASYNC_TIMER_PANEA;
	m_PaneA.m_AsyncParams.uTimer = 0;
	m_PaneA.m_AsyncParams.strFilename.Empty();
	m_PaneA.m_AsyncParams.strSourceFilename.Empty();

	m_PaneB.m_AsyncParams.bCallouts = FALSE;
	m_PaneB.m_AsyncParams.bScaleView = FALSE;
	m_PaneB.m_AsyncParams.bUseView = FALSE;
	m_PaneB.m_AsyncParams.iTimerId = ASYNC_TIMER_PANEB;
	m_PaneB.m_AsyncParams.uTimer = 0;
	m_PaneB.m_AsyncParams.strFilename.Empty();
	m_PaneB.m_AsyncParams.strSourceFilename.Empty();


	//	Only set these properties if we are in user mode
	if(AmbientUserMode())
	{
		//	Set the top/left pane properties
		SetPaneProps(&m_PaneA);

		//	Set the bottom/right pane properties
		SetPaneProps(&m_PaneB);

		//	Set the properties for the scratch pane
		SetPaneProps(&m_Scratch);
	}

	//	Set up the split screen mode
	OnSplitScreenChanged();
	
	//	Load the files if any are specified
	if(!m_strLeftFile.IsEmpty())
		m_PaneA.SetFilename(m_strLeftFile);
	if(!m_strRightFile.IsEmpty())
		m_PaneB.SetFilename(m_strRightFile);
	
//ShowDiagnostics(TRUE);
	return 0;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnShadeOnCalloutChanged()
//
// 	Description:	This function is called when the CutOutCalloutHighlights
//					property is changed. The change is passed to the TMLead 
//					object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnShadeOnCalloutChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetShadeOnCallout(m_bShadeOnCallout);
		m_PaneB.SetShadeOnCallout(m_bShadeOnCallout);
	}
	else
	{
		GetPane()->SetShadeOnCallout(m_bShadeOnCallout);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnDeskewBackColorChanged()
//
// 	Description:	This function is called when the DeskewBackColor property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnDeskewBackColorChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetDeskewBackColor(TranslateColor(m_lDeskewBackColor));
		m_PaneB.SetDeskewBackColor(TranslateColor(m_lDeskewBackColor));
	}
	else
	{
		GetPane()->SetDeskewBackColor(TranslateColor(m_lDeskewBackColor));
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnDraw()
//
// 	Description:	This function draws the control using the background color
//					property.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnDraw(CDC* pdc, const CRect& rcBounds,const CRect& rcInvalid)
{
	CBrush brBackground;

	//	Don't bother if we are inhibiting redraw operations
	if(!m_bRedraw)
		return;

	//	Are we in user mode?
	if(AmbientUserMode())
	{
		//	Use the container's background color in user mode if we are
		//	fitting the window to the image. Otherwise use the background color
		if(m_bFitToImage)
			brBackground.CreateSolidBrush(TranslateColor(AmbientBackColor()));
		else
			brBackground.CreateSolidBrush(TranslateColor(GetBackColor()));

		pdc->FillRect(rcBounds, &brBackground);
		
		//	Draw the highlight if we are in split screen mode
		if(m_bSplitScreen)
		{
			DrawSplitFrame(m_sActivePane == TMV_LEFTPANE ? &m_rcLFrame : &m_rcRFrame);
			DrawPenSelector();
		}

		//	Redraw the left pane
		if(m_pLeft && m_pLeft->IsLoaded() && IsWindow(m_pLeft->m_hWnd))
			m_pLeft->ForceRepaint();

		//	Redraw the right pane
		if(m_bSplitScreen && m_pRight && m_pRight->IsLoaded() && IsWindow(m_pRight->m_hWnd))
			m_pRight->ForceRepaint();
	}
	else
	{
		CRect ControlRect = rcBounds;
		CString strText;

		strText.Format("FTI Graphic Viewer Control (rev. %d.%d)",
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
// 	Function Name:	CTMViewCtrl::OnEnableAxErrorsChanged()
//
// 	Description:	This function is called when the EnableAxErrors property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnEnableAxErrorsChanged() 
{
	SetModifiedFlag();

	if(AmbientUserMode())
		m_Errors.SetMessageId(m_bEnableAxErrors == TRUE ? WM_ERROR_EVENT : 0);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnEnabledChanged()
//
// 	Description:	This function is called when the enable property is
//					changed. It will set the style of the lead control to match.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnEnabledChanged() 
{
	COleControl::OnEnabledChanged();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetEnabled(GetEnabled());
		m_PaneB.SetEnabled(GetEnabled());
	}
	else
	{
		GetPane()->SetEnabled(GetEnabled());
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnEnableErrorsChanged()
//
// 	Description:	This function is called when the enable errors property is
//					changed. The change is passed to the TMLead object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnEnableErrorsChanged() 
{
	SetModifiedFlag();
	
	if(AmbientUserMode())
	{
		m_Errors.Enable(m_bEnableErrors);
	}

}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnEndEditTextAnn()
//
// 	Description:	This function is called by one of the TMLead controls when
//					the user starts an in-place edit of a text annotation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnEndEditTextAnn(CTMLead* pTMLead) 
{
	#if defined _EVENT_DIAGNOSTICS
		m_Diagnostics.Report("CTMViewCtrl", "OnEndEditTextAnn");
	#endif

	//	Clear the flag used by PreProcessMessage()
	m_bEditTextAnn = FALSE;

	//	Notify the container
	FireStopTextEdit(m_sActivePane);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnFitToImageChanged()
//
// 	Description:	This function is called when the fit to image property is
//					changed. The change is passed to the TMLead object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnFitToImageChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetFitToImage(m_bFitToImage);
		m_PaneB.SetFitToImage(m_bFitToImage);
	}
	else
	{
		GetPane()->SetFitToImage(m_bFitToImage);
	}
	RedrawWindow();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnHideScrollBarsChanged()
//
// 	Description:	This function is called when the pan image property is
//					changed. The change is passed to the TMLead object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnHideScrollBarsChanged() 
{	
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetHideScrollBars(m_bHideScrollBars);
		m_PaneB.SetHideScrollBars(m_bHideScrollBars);
	}
	else
	{
		GetPane()->SetHideScrollBars(m_bHideScrollBars);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnHighlightColorChanged()
//
// 	Description:	This function is called when the highlight color property is
//					changed. The change is passed to the TMLead object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnHighlightColorChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetHighlightColor(m_sHighlightColor);
		m_PaneB.SetHighlightColor(m_sHighlightColor);
	}
	else
	{
		GetPane()->SetHighlightColor(m_sHighlightColor);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnKeepAspectChanged()
//
// 	Description:	This function is called when the KeepAspect property changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnKeepAspectChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetKeepAspect(m_bKeepAspect);
		m_PaneB.SetKeepAspect(m_bKeepAspect);
	}
	else
	{
		GetPane()->SetKeepAspect(m_bKeepAspect);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnLButtonDown()
//
// 	Description:	This function traps WM_LBUTTONDOWN messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnLButtonDown(UINT nFlags, CPoint point) 
{
	short sPane;

	//	Are we in split screen mode?
	if(m_bSplitScreen)
	{
		//	What pane is the user in?
		if(m_bSplitHorizontal)
		{
			if(point.y <= m_rcLFrame.bottom)
				sPane = TMV_LEFTPANE;
			else
				sPane = TMV_RIGHTPANE;
		}
		else
		{
			if(point.x <= m_rcLFrame.right)
				sPane = TMV_LEFTPANE;
			else
				sPane = TMV_RIGHTPANE;
		}

		//	Do we need to switch panes?
		if(sPane != m_sActivePane)
		{
			m_sActivePane = sPane;
			OnActivePaneChanged();

		}
	}

	COleControl::OnLButtonDown(nFlags, point);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnLButtonUp()
//
// 	Description:	This function traps WM_LBUTTONUP messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnLButtonUp(UINT nFlags, CPoint point) 
{
	//	Fire an event
	FireMouseClick(LEFT_MOUSEBUTTON, GetControlKeys());

	COleControl::OnLButtonUp(nFlags, point);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnLeftFileChanged()
//
// 	Description:	This function is called when the LeftFile property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnLeftFileChanged() 
{
	LoadFile(m_strLeftFile, TMV_LEFTPANE);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnLoadAsyncChanged()
//
// 	Description:	This function is called when the LoadAsync property changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnLoadAsyncChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnLoopAnimationChanged()
//
// 	Description:	This function informs the TMLead object when the 
//					LoopAnimation property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnLoopAnimationChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetLoopAnimate(m_bLoopAnimation);
		m_PaneB.SetLoopAnimate(m_bLoopAnimation);
	}
	else
	{
		GetPane()->SetLoopAnimate(m_bLoopAnimation);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnMaxZoomChanged()
//
// 	Description:	This function is called when the maximum zoom property is
//					changed. The change is passed to the TMLead object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnMaxZoomChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetMaxZoom(m_sMaxZoom);
		m_PaneB.SetMaxZoom(m_sMaxZoom);
	}
	else
	{
		GetPane()->SetMaxZoom(m_sMaxZoom);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnMouseMove()
//
// 	Description:	This function traps WM_MOUSEMOVE messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnMouseMove(UINT nFlags, CPoint point) 
{
	//	Notify the container
	FireMouseMove(m_sButton, GetControlKeys(), point.x, point.y);

	COleControl::OnMouseMove(nFlags, point);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnOpenTextBox()
//
// 	Description:	This function is called by one of the TMLead controls when
//					the user opens the dialog box to enter the text for an
//					annotation.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnOpenTextBox(CTMLead* pTMLead) 
{
	//	Initialize the counter we use to track mouse click events when the text
	//	box is open. In order to trick the LeadTools control into updating the
	//	display properly we have to send some messages that result in two 
	//	unwanted MouseClick() events from LeadTools. We use this counter to keep
	//	from firing unwanted events to the container.
	m_iTextOpen = 2;

	//	Fire the appropriate event
	FireOpenTextBox(m_sActivePane);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnPaletteChanged()
//
// 	Description:	This function forwards the WM_PALETTECHANGED message 
//					to the lead control dialog.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnPaletteChanged(CWnd* pFocusWnd) 
{
	COleControl::OnPaletteChanged(pFocusWnd);
	m_PaneA.SendMessage(WM_PALETTECHANGED, (WPARAM)pFocusWnd->m_hWnd);
	m_PaneB.SendMessage(WM_PALETTECHANGED, (WPARAM)pFocusWnd->m_hWnd);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnPanCalloutsChanged()
//
// 	Description:	This function is called when the PanCallouts property changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnPanCalloutsChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetPanCallouts(m_bPanCallouts);
		m_PaneB.SetPanCallouts(m_bPanCallouts);
	}
	else
	{
		GetPane()->SetPanCallouts(m_bPanCallouts);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnPanPercentChanged()
//
// 	Description:	This function is called when the pan percent property is
//					changed. The change is passed to the TMLead object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnPanPercentChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetPanPercent(m_sPanPercent);
		m_PaneB.SetPanPercent(m_sPanPercent);
	}
	else
	{
		GetPane()->SetPanPercent(m_sPanPercent);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnParentNotify()
//
// 	Description:	This function handles WM_PARENTNOTIFY messages sent from
//					one of the lead tool controls
//
// 	Returns:		None
//
//	Notes:			This function works with OnSetCursor() to work around a bug
//					in the LeadTools control. See OnSetCursor() for more info.
//
//==============================================================================
void CTMViewCtrl::OnParentNotify(UINT message, LPARAM lParam) 
{
	short	sPane;
	POINT	Pt;

	//	Perform the base class processing first
	COleControl::OnParentNotify(message, lParam);

	//	What notification is being sent?
	switch(message)
	{
		case WM_LBUTTONDOWN:
		case WM_RBUTTONDOWN:

			//	Unpack the coordinates
			Pt.x = LOWORD(lParam);
			Pt.y = HIWORD(lParam);
			
			//	Make sure the correct pane is activated if we are in split screen
			if(m_bSplitScreen)
			{
				//	What pane is the user in?
				if(m_bSplitHorizontal)
				{
					if(Pt.y <= m_rcLFrame.bottom)
						sPane = TMV_LEFTPANE;
					else
						sPane = TMV_RIGHTPANE;
				}
				else
				{
					if(Pt.x <= m_rcLFrame.right)
						sPane = TMV_LEFTPANE;
					else
						sPane = TMV_RIGHTPANE;
				}

				//	Do we need to switch panes?
				if(sPane != m_sActivePane)
				{
					m_sActivePane = sPane;
					OnActivePaneChanged();
				}
			}

			//	Set the flag to indicate the selection is changing
			if(GetPane()->m_sAction == SELECT && GetPane()->IsLoaded())
			{
				m_bParentNotify = TRUE;
				m_bSetCursor    = FALSE;
			}

			break;
		
		default:
			
			break;
	}
	
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnPenSelectorVisibleColor()
//
// 	Description:	This function is called when the PenSelectorColor property
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnPenSelectorColorChanged() 
{
	if(AmbientUserMode() && m_bSplitScreen && m_bPenSelectorVisible)
		RedrawWindow();

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnPenSelectorSizeChanged()
//
// 	Description:	This function is called when the PenSelectorSize property
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnPenSelectorSizeChanged() 
{
	if(AmbientUserMode() && m_bSplitScreen && m_bPenSelectorVisible)
		RedrawWindow();

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnPenSelectorVisibleChanged()
//
// 	Description:	This function is called when the PenSelectorVisible property
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnPenSelectorVisibleChanged() 
{
	if(AmbientUserMode() && m_bSplitScreen)
		RedrawWindow();

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnPrintBorderChanged()
//
// 	Description:	The framework calls this function when the PrintBorder
//					property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnPrintBorderChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetPrintBorder(m_bPrintBorder);
		m_PaneB.SetPrintBorder(m_bPrintBorder);
	}
	else
	{
		GetPane()->SetPrintBorder(m_bPrintBorder);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnPrintBorderColorChanged()
//
// 	Description:	The framework calls this function when the PrintBorderColor
//					property changes.
//
// 	Returns:		None
//
//	Notes:			No action is required.
//
//==============================================================================
void CTMViewCtrl::OnPrintBorderColorChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetPrintBorderColor(TranslateColor(m_lPrintBorderColor));
		m_PaneB.SetPrintBorderColor(TranslateColor(m_lPrintBorderColor));
	}
	else
	{
		GetPane()->SetPrintBorderColor(TranslateColor(m_lPrintBorderColor));
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnPrintBorderThicknessChanged()
//
// 	Description:	The framework calls this function when the PrintBorderThickness
//					property changes.
//
// 	Returns:		None
//
//	Notes:			No action is required.
//
//==============================================================================
void CTMViewCtrl::OnPrintBorderThicknessChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetPrintBorderThickness(m_fPrintBorderThickness);
		m_PaneB.SetPrintBorderThickness(m_fPrintBorderThickness);
	}
	else
	{
		GetPane()->SetPrintBorderThickness(m_fPrintBorderThickness);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnPrintBottomMarginChanged()
//
// 	Description:	The framework calls this function when the PrintBottomMargin
//					property changes.
//
// 	Returns:		None
//
//	Notes:			No action is required.
//
//==============================================================================
void CTMViewCtrl::OnPrintBottomMarginChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnPrintCalloutsChanged()
//
// 	Description:	This function is called when the PrintCallouts property
//					is changed
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnPrintCalloutsChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnPrintBorderChanged()
//
// 	Description:	The framework calls this function when the PrintCalloutBorders
//					property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnPrintCalloutBordersChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetPrintCalloutBorders(m_bPrintCalloutBorders);
		m_PaneB.SetPrintCalloutBorders(m_bPrintCalloutBorders);
	}
	else
	{
		GetPane()->SetPrintCalloutBorders(m_bPrintCalloutBorders);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnPrintLeftMarginChanged()
//
// 	Description:	The framework calls this function when the PrintLeftMargin
//					property changes.
//
// 	Returns:		None
//
//	Notes:			No action is required.
//
//==============================================================================
void CTMViewCtrl::OnPrintLeftMarginChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnPrintOrientationChanged()
//
// 	Description:	This function is called when the PrintOrientation property
//					is changed
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnPrintOrientationChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnPrintRightMarginChanged()
//
// 	Description:	The framework calls this function when the PrintRightMargin
//					property changes.
//
// 	Returns:		None
//
//	Notes:			No action is required.
//
//==============================================================================
void CTMViewCtrl::OnPrintRightMarginChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnPrintTopMarginChanged()
//
// 	Description:	The framework calls this function when the PrintTopMargin
//					property changes.
//
// 	Returns:		None
//
//	Notes:			No action is required.
//
//==============================================================================
void CTMViewCtrl::OnPrintTopMarginChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnQFactorChanged()
//
// 	Description:	This function is called when the QFactor property is 
//					changed. The change is passed to the TMLead pane(s).
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnQFactorChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetQFactor(m_sQFactor);
		m_PaneB.SetQFactor(m_sQFactor);
	}
	else
	{
		GetPane()->SetQFactor(m_sQFactor);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnQueryNewPalette()
//
// 	Description:	This function forwards the WM_QUERYNEWPALETTE message 
//					to the lead control dialog.
//
// 	Returns:		FALSE if not successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMViewCtrl::OnQueryNewPalette() 
{
	m_PaneA.SendMessage(WM_QUERYNEWPALETTE);
	m_PaneB.SendMessage(WM_QUERYNEWPALETTE);
	return COleControl::OnQueryNewPalette();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnRButtonDown()
//
// 	Description:	This function traps WM_RBUTTONDOWN messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnRButtonDown(UINT nFlags, CPoint point) 
{
	short sPane;

	//	Are we in split screen mode?
	if(m_bSplitScreen)
	{
		//	What pane is the user in?
		if(m_bSplitHorizontal)
		{
			if(point.y <= m_rcLFrame.bottom)
				sPane = TMV_LEFTPANE;
			else
				sPane = TMV_RIGHTPANE;
		}
		else
		{
			if(point.x <= m_rcLFrame.right)
				sPane = TMV_LEFTPANE;
			else
				sPane = TMV_RIGHTPANE;
		}

		//	Do we need to switch panes?
		if(sPane != m_sActivePane)
		{
			m_sActivePane = sPane;
			OnActivePaneChanged();
		}
	}

	COleControl::OnRButtonDown(nFlags, point);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnRButtonUp()
//
// 	Description:	This function traps WM_RBUTTONUP messages
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnRButtonUp(UINT nFlags, CPoint point) 
{
	//	Fire an event
	FireMouseClick(RIGHT_MOUSEBUTTON, GetControlKeys());

	COleControl::OnRButtonUp(nFlags, point);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnRedactColorChanged()
//
// 	Description:	This function is called when the redact color property is
//					changed. The change is passed to the TMLead object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnRedactColorChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetRedactColor(m_sRedactColor);
		m_PaneB.SetRedactColor(m_sRedactColor);
	}
	else
	{
		GetPane()->SetRedactColor(m_sRedactColor);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnResetState()
//
// 	Description:	This function is called by the framework when the control
//					properties need to be reset.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnResetState()
{
	//	Reset properties to defaults found in DoPropExchange
	COleControl::OnResetState(); 
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnResizeCalloutsChanged()
//
// 	Description:	This function is called when the ResizeCallouts property changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnResizeCalloutsChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetResizeCallouts(m_bResizeCallouts);
		m_PaneB.SetResizeCallouts(m_bResizeCallouts);
	}
	else
	{
		GetPane()->SetResizeCallouts(m_bResizeCallouts);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnRightClickPanChanged()
//
// 	Description:	This function is called when the right click pan property is
//					changed. The change is passed to the TMLead object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnRightClickPanChanged() 
{
	m_PaneA.SetRightClickPan(m_bRightClickPan);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnRightFileChanged()
//
// 	Description:	This function is called when the RightFile property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnRightFileChanged() 
{
	LoadFile(m_strRightFile, TMV_RIGHTPANE);
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnRotationChanged()
//
// 	Description:	This function is called when the rotation property is
//					changed. The change is passed to the TMLead object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnRotationChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetRotation(m_sRotation);
		m_PaneB.SetRotation(m_sRotation);
	}
	else
	{
		GetPane()->SetRotation(m_sRotation);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnSavedPage()
//
// 	Description:	This function handles notifications from a pane when
//					it saves a page in a multipage file
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnSavedPage(LPCSTR lpszSourceFile, LPCSTR lpszPageFile, short sPage, short sTotal) 
{
	//	Fire an event
	FireSavedPage(lpszSourceFile, lpszPageFile, sPage, sTotal);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnScaleImageChanged()
//
// 	Description:	This function is called when the scale image property is
//					changed. The change is passed to the TMLead object.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnScaleImageChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetScaleImage(m_bScaleImage);
		m_PaneB.SetScaleImage(m_bScaleImage);
	}
	else
	{
		GetPane()->SetScaleImage(m_bScaleImage);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnSetCursor()
//
// 	Description:	This function handles WM_SETCURSOR messages sent from
//					one of the lead tool controls
//
// 	Returns:		None
//
//	Notes:			This function works with OnParentNotify() to work around a
//					bug in the LeadTools control. If the user selects an 
//					annotation in SELECT mode the control will fire an AnnSelect
//					event. However, if the user clears all the selections or
//					selects annototations by rubberbanding, the control does
//					not fire the AnnSelect event.
//
//					To make sure we keep the selections synchronized, we trap
//					the WM_PARENTNOTIFY and WM_SETCURSOR messages. When the user
//					clicks in the window, a WM_PARENTNOTIFY message will be 
//					sent followed by a WM_SETCURSOR message. Then, when the user
//					completes the rubber banding, another WM_SETCURSOR message
//					is sent. We wait for that second WM_SETCURSOR to update
//					the selections.
//
//==============================================================================
BOOL CTMViewCtrl::OnSetCursor(CWnd* pWnd, UINT nHitTest, UINT message) 
{
	//	Do we need to update the annotation selections?
	if(m_bParentNotify)
	{
		m_bParentNotify = FALSE;
		m_bSetCursor    = TRUE;
	}
	else if(m_bSetCursor)
	{
		
		m_bParentNotify = FALSE;
		m_bSetCursor    = FALSE;
		
		//	Update the selections in the active pane
		GetPane()->OnAnnSelect();
	}

	//	Perform the base class processing
	return COleControl::OnSetCursor(pWnd, nHitTest, message);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnSize()
//
// 	Description:	This function will resize the dialog box that contains the
//					lead tool control whenever the control is resized. The 
//					dialog box will resize the lead control in response.
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnSize(UINT nType, int cx, int cy) 
{
	//	Recalculate the rectangles
	
	RecalcLayout();

	//	Set the new extents for the viewers
	if(IsWindow(m_PaneA.m_hWnd) && IsWindow(m_PaneB.m_hWnd))
	{
		//	Block attempts to redraw while we resize everything
		m_bRedraw = FALSE;

		//	Size the left pane
		if(m_pLeft)
		{
			//	Are we operating in split screen mode?
			if(m_bSplitScreen)
				m_pLeft->SetMaxRect(&m_rcLPane, TRUE, TRUE);
			else
				m_pLeft->SetMaxRect(&m_rcMax, FALSE, TRUE);
		}

		//	Size the right pane if it's visible
		if(m_pRight && m_bSplitScreen)
			m_pRight->SetMaxRect(&m_rcRPane, TRUE, TRUE);
		
		//	It's ok to draw now
		m_bRedraw = TRUE;

		RedrawWindow();
	}
	else
		COleControl::OnSize(nType, cx, cy);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnSplitPaneColorChanged()
//
// 	Description:	This function is called when the SplitPaneColor property
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnSplitFrameColorChanged() 
{
	if(AmbientUserMode() && m_bSplitScreen)
		RedrawWindow();

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnSplitFrameThicknessChanged()
//
// 	Description:	This function is called when the SplitFrameThickness 
//					property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnSplitFrameThicknessChanged() 
{
	//	If we are in split screen mode update the frame
	if(AmbientUserMode())
	{
		//	Recalculate the rectangles
		RecalcLayout();

		//	Redraw if we are in split screen mode
		if(m_bSplitScreen)
			RedrawWindow();
	}

	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnSplitHorizontalChanged()
//
// 	Description:	This function is called when the SplitHorizontal property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnSplitHorizontalChanged() 
{
	SetModifiedFlag();

	if(AmbientUserMode())
	{
		//	Recalculate the rectangles
		RecalcLayout();

		//	Redraw if we are in split screen mode
		if(m_bSplitScreen)
		{
			//	Prevent attempts to redraw 
			m_bRedraw = FALSE;

			//	Make sure we have the correct panes
			m_pLeft = GetPane(TMV_LEFTPANE);
			m_pRight = GetPane(TMV_RIGHTPANE);

			//	Size the each pane
			m_pLeft->SetMaxRect(&m_rcLPane, TRUE, TRUE);
			m_pRight->UnloadImage();
			m_pRight->SetMaxRect(&m_rcRPane, TRUE, TRUE);

			//	Redraw the windows
			m_bRedraw = TRUE;
			RedrawWindow();
		}

	}

}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnSplitScreenChanged()
//
// 	Description:	This function is called when the SplitScreen property 
//					changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnSplitScreenChanged() 
{
	RECT rcInactive;

	SetModifiedFlag();

	//	Don't bother if we're not in user mode?
	if(!AmbientUserMode())
		return;

	//	Prevent attempts to redraw 
	m_bRedraw = FALSE;

	//	Make the active pane the left pane
	m_pLeft = GetPane();
		
	//	Make the inactive pane the right pane
	m_pRight = (m_pLeft == &m_PaneA) ? &m_PaneB : &m_PaneA;

	ASSERT(m_pLeft);
	ASSERT(m_pRight);

	if(m_bSplitScreen)
	{
		//	Size each pane
		m_pLeft->SetMaxRect(&m_rcLPane, TRUE, TRUE);
		m_pRight->UnloadImage();
		m_pRight->SetMaxRect(&m_rcRPane, TRUE, TRUE);

		//	Both panes should be visible if they are loaded
		if(m_pLeft->IsLoaded())
		{
			m_pLeft->ShowWindow(SW_SHOW);
			m_pLeft->ShowCallouts(TRUE);
		}
		if(m_pRight->IsLoaded())
		{
			m_pRight->ShowCallouts(TRUE);
			m_pRight->ShowWindow(SW_SHOW);
		}
	}
	else
	{
		//	Size the active pane
		m_pLeft->SetMaxRect(&m_rcMax, FALSE, TRUE);

		//	The inactive pane should not be visible
		m_pRight->ShowCallouts(FALSE);
		m_pRight->ShowWindow(SW_HIDE);

		//	Make sure the inactive is shrunk to zero
		memset(&rcInactive, 0, sizeof(rcInactive));
		m_pRight->SetMaxRect(&rcInactive, FALSE, TRUE);
	}

	//	Make sure the pane is properly selected
	m_sActivePane = TMV_LEFTPANE;
	m_pActive = m_pLeft;
	FireSelectPane(m_sActivePane);

	//	Redraw the windows
	m_bRedraw = TRUE;
	RedrawWindow();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnStartEditTextAnn()
//
// 	Description:	This function is called by one of the TMLead controls when
//					the user starts an in-place edit of a text annotation
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnStartEditTextAnn(CTMLead* pTMLead) 
{
	#if defined _EVENT_DIAGNOSTICS
		m_Diagnostics.Report("CTMViewCtrl", "OnStartEditTextAnn");
	#endif

	//	Set the flag used by PreProcessMessage()
	m_bEditTextAnn = TRUE;

	//	Notify the container
	FireStartTextEdit(m_sActivePane);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnSyncCalloutAnnChanged()
//
// 	Description:	This function is called when the SyncCalloutAnn property is
//					changed. The change is passed to the TMLead object(s).
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnSyncCalloutAnnChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetSyncCalloutAnn(m_bSyncCalloutAnn);
		m_PaneB.SetSyncCalloutAnn(m_bSyncCalloutAnn);
	}
	else
	{
		GetPane()->SetSyncCalloutAnn(m_bSyncCalloutAnn);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnSyncPanesChanged()
//
// 	Description:	This function is called when the SyncPanes property changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnSyncPanesChanged() 
{
	SetModifiedFlag();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnTimer()
//
// 	Description:	This function handles all WM_TIMER events
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnTimer(UINT nIDEvent) 
{
	CTMLead* pPane;

	if(nIDEvent == (UINT)m_PaneA.m_AsyncParams.iTimerId)
	{
		pPane = &m_PaneA;	
	}
	else if(nIDEvent == (UINT)m_PaneB.m_AsyncParams.iTimerId)
	{
		pPane = &m_PaneB;
	}
	
	KillTimer(nIDEvent);

	if(pPane != NULL)
	{
		pPane->m_AsyncParams.uTimer = 0;	//	Set up next load

		if(pPane->m_AsyncParams.bZap == TRUE)
		{
		}
		else
		{
			pPane->SetFilename(pPane->m_AsyncParams.strFilename);
			pPane->m_AsyncParams.strFilename.Empty();
		}

	}

	COleControl::OnTimer(nIDEvent);
}

//==============================================================================
//
// 	Function Name:	CTMView::OnWMErrorEvent()
//
// 	Description:	This function handles all WM_ERROR_EVENT messages
//
// 	Returns:		Zero if the message is handled
//
//	Notes:			None
//
//==============================================================================
LONG CTMViewCtrl::OnWMErrorEvent(WPARAM wParam, LPARAM lParam)
{
	if((m_bEnableAxErrors == TRUE) && (lstrlen(m_Errors.GetMessage()) > 0))
	{
		FireAxError(m_Errors.GetMessage());
	}
	return 0;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnZoomCalloutsChanged()
//
// 	Description:	This function is called when the ZoomCallouts property changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnZoomCalloutsChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetZoomCallouts(m_bZoomCallouts);
		m_PaneB.SetZoomCallouts(m_bZoomCallouts);
	}
	else
	{
		GetPane()->SetZoomCallouts(m_bZoomCallouts);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnZoomOnLoadChanged()
//
// 	Description:	This function is called when the zoom on load property 
//					changes.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnZoomOnLoadChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetZoomOnLoad(m_sZoomOnLoad);
		m_PaneB.SetZoomOnLoad(m_sZoomOnLoad);
	}
	else
	{
		GetPane()->SetZoomOnLoad(m_sZoomOnLoad);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::OnZoomToRectChanged()
//
// 	Description:	This function is called when the ZoomToRect property changes
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::OnZoomToRectChanged() 
{
	SetModifiedFlag();
	
	if(!AmbientUserMode())
	{
		return;
	}
	else if(m_bSyncPanes)
	{
		m_PaneA.SetZoomToRect(m_bZoomToRect);
		m_PaneB.SetZoomToRect(m_bZoomToRect);
	}
	else
	{
		GetPane()->SetZoomToRect(m_bZoomToRect);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::Pan()
//
// 	Description:	This method is called to pan the image
//
// 	Returns:		TRUE if successful
//
//	Notes:			None
//
//==============================================================================
BOOL CTMViewCtrl::Pan(short sDirection, short sPane) 
{
	return GetPane(sPane)->Pan(sDirection);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::Paste()
//
// 	Description:	This external method allows the caller to paste the contents
//					of the clipboard to the specified pane
//
// 	Returns:		TMV_NOERROR if successful. 
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::Paste(short sPane) 
{
	return GetPane(sPane)->Paste();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::PlayAnimation()
//
// 	Description:	This function allows the caller to play or stop an
//					animation.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::PlayAnimation(BOOL bPlay, BOOL bContinuous, short sPane) 
{
	return GetPane(sPane)->PlayAnimation(bPlay, bContinuous);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::PreCreateWindow()
//
// 	Description:	This function modifies the window style just before it's
//					created.
//
// 	Returns:		TRUE if successful.
//
//	Notes:			None
//
//==============================================================================
BOOL CTMViewCtrl::PreCreateWindow(CREATESTRUCT& cs) 
{
	return COleControl::PreCreateWindow(cs);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::PreTranslateMessage()
//
// 	Description:	This function will look at messages before they are
//					processed.
//
// 	Returns:		TRUE if handled.
//
//	Notes:			None
//
//==============================================================================
BOOL CTMViewCtrl::PreTranslateMessage(MSG* pMsg) 
{
	switch(pMsg->message)
	{
		case WM_KEYDOWN:
		case WM_KEYUP:

			//	Are we editing a text annotation?
			if(m_bEditTextAnn)
			{
				//	Is this the escape key?
				if(pMsg->wParam == VK_ESCAPE)
				{
					//	NOTE:	We do this because of a problem with Lead Tools
					//			We can't reliably predict the end of an edit
					//			session when the user presses Escape because
					//			Lead Tools does not fire an AnnChange event
					GetPane()->OnEndEditTextAnn(TRUE);
				}

			}
			break;
	}

	//	If we made it this far we want to do the default processing
	return COleControl::PreTranslateMessage(pMsg);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::PrevPage()
//
// 	Description:	This external method allows the caller to go back to the 
//					previous page in the file.
//
// 	Returns:		TMV_NOERROR if successful. Otherwise, one of the TMV error
//					levels defined in tmvdefs.h
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::PrevPage(short sPane) 
{
	return GetPane(sPane)->PrevPage();
}

//==============================================================================
//
// 	Function Name:	CTMLead::Print()
//
// 	Description:	This function will print the current image to the default
//					printer.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::Print(BOOL bFullImage, short sPane) 
{
	CDC*			pdc;
	RECT			rcLeft;
	RECT			rcRight;
	RECT			rcMax;	
	int				iWidth;
	int				iHeight;
	int				iCx;
	long			lCallouts = 0;
	BOOL			bPortrait;
	BOOL			bLeft;
	BOOL			bRight;
	CString			strMsg;

	//	Make sure the printer is attached
	if(!m_Printer.Attach())
	{
		m_Errors.Handle(0, m_Printer.GetErrorMsg());
		return TMV_NODEFAULTPRINTER;
	}

	//	What pane do we want to print?
	if(m_bSplitScreen)
	{
		//	What pane has been requested by the caller?
		if(sPane == TMV_LEFTPANE)
		{
			bLeft = GetPane(TMV_LEFTPANE)->IsLoaded();
			bRight = FALSE;
		}
		else if(sPane == TMV_RIGHTPANE)
		{
			bLeft = FALSE;
			bRight = GetPane(TMV_RIGHTPANE)->IsLoaded();
		}
		else
		{
			bLeft = GetPane(TMV_LEFTPANE)->IsLoaded();
			bRight = GetPane(TMV_RIGHTPANE)->IsLoaded();
		}
	}
	else
	{
		//	We only print the active pane if not in split screen mode
		bLeft = GetPane()->IsLoaded();
		bRight = FALSE;
	}

	//	Is there an image to print?
	if(!bLeft && !bRight)
	{
		m_Errors.Handle(0, IDS_TMV_NOIMAGE);
		return TMV_NOIMAGE;
	}

	//	Determine the total number of callouts available for printing
	if(bLeft)
		lCallouts += GetPane(TMV_LEFTPANE)->GetCalloutCount();
	if(bRight)
		lCallouts += GetPane(TMV_RIGHTPANE)->GetCalloutCount();

	//	How do we want to orient the page?
	bPortrait = GetOrientation(bLeft, bRight);
	if(bPortrait)
		m_Printer.SetOrientation(TMPRINTER_ORIENTATION_PORTRAIT);
	else
		m_Printer.SetOrientation(TMPRINTER_ORIENTATION_LANDSCAPE);

	//	Start the print job
	if((pdc = m_Printer.Start("TrialMax TMView Job")) == 0)
	{
		strMsg = m_Printer.GetErrorMsg();
		m_Errors.Handle(0, IDS_TMV_STARTJOBFAILED, strMsg);

		return TMV_STARTJOBFAILED;
	}

	//	Start the new page
	m_Printer.StartPage();

	//	Get the maximum print area
	memcpy(&rcMax, m_Printer.GetMaxRect(), sizeof(rcMax));

	//	Are we going to be printing callouts?
	if(m_bPrintCallouts && lCallouts > 0)
	{
		//	Adjust the maximum print rectangle so that it has the same
		//	aspect ratio as the screen
		GetPane()->SetToScreenRatio(&rcMax);
	}

	//	Are we printing both images?
	if(bLeft && bRight)
	{
		//	Get the page extents in pixels
		iWidth  = rcMax.right  - rcMax.left;
		iHeight = rcMax.bottom - rcMax.top;

		//	Get the center line of the page
		iCx = rcMax.left + (iWidth / 2);

		//	Initialize the left hand print rectangle
		rcLeft.left   = rcMax.left;
		rcLeft.top	  = rcMax.top;
		rcLeft.right  = iCx - ((int)((float)m_Printer.GetXDpi() * m_Printer.GetRightMargin()));
		rcLeft.bottom = rcMax.bottom;
			
		//	Initialize the left hand print rectangle
		rcRight.left   = iCx + ((int)((float)m_Printer.GetXDpi() * m_Printer.GetLeftMargin()));
		rcRight.top	   = rcMax.top;
		rcRight.right  = rcMax.right;
		rcRight.bottom = rcMax.bottom;

		GetPane(TMV_LEFTPANE)->Print(pdc, &rcLeft, &rcMax, bFullImage);
		GetPane(TMV_RIGHTPANE)->Print(pdc, &rcRight, &rcMax, bFullImage);
	}
	else if(bLeft)
	{
		GetPane(TMV_LEFTPANE)->Print(pdc, &rcMax, &rcMax, bFullImage);
	}
	else
	{
		GetPane(TMV_RIGHTPANE)->Print(pdc, &rcMax, &rcMax, bFullImage);
	}

	//	Are we supposed to print the callouts?
	if(m_bPrintCallouts)
	{
		if(bLeft && GetPane(TMV_LEFTPANE)->GetCalloutCount() > 0)
			GetPane(TMV_LEFTPANE)->PrintCallouts(pdc, &rcMax, FALSE);
		if(bRight && GetPane(TMV_RIGHTPANE)->GetCalloutCount() > 0)
			GetPane(TMV_RIGHTPANE)->PrintCallouts(pdc, &rcMax, FALSE);
	}

	//	End the page
	m_Printer.EndPage();

	//	End the job
	m_Printer.End();

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMLead::PrintEx()
//
// 	Description:	This function will print the current image into the 
//					specified device context using the specified coordinates as
//					a bounding rectangle.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::PrintEx(OLE_HANDLE hDC, short bFullImage, short bAutoRotate, 
						   short sLeft, short sTop, short sWidth, short sHeight, 
						   short sPane) 
{
	RECT		rcBounds;
	RECT		rcTLPane;
	RECT		rcBRPane;
	CTMLead*	pTLPane = NULL;
	CTMLead*	pBRPane = NULL;
	CDC*		pdc = CDC::FromHandle((HDC)hDC);
	BOOL		bRotate = FALSE;
	BOOL		bCallouts = FALSE;
	BOOL		bSplitScreen = FALSE;
	double		dImageRatio = 0.0;
	double		dMaxWidth = 0.0;
	double		dMaxHeight = 0.0;
	double		dCx = 0.0;
	double		dCy = 0.0;

	ASSERT(sWidth > 0);
	ASSERT(sHeight > 0);
	ASSERT(pdc);

	//	Has the caller specified a particular pane?
	if((sPane == TMV_LEFTPANE) || (sPane == TMV_RIGHTPANE))
	{
		//	Use the top/left pane as the active print pane
		pTLPane = GetPane(sPane);
	}
	else
	{
		//	Are we in split screen mode?
		if(m_bSplitScreen == TRUE)
		{
			//	Are both panes loaded?
			if((GetPane(TMV_LEFTPANE)->IsLoaded() == TRUE) && 
			   (GetPane(TMV_RIGHTPANE)->IsLoaded() == TRUE))
				bSplitScreen = TRUE;
		}

		//	Print the active pane if not printing split screen
		if(bSplitScreen == TRUE)
		{
			pTLPane = GetPane(TMV_LEFTPANE);
			pBRPane = GetPane(TMV_RIGHTPANE);
		}
		else
		{
			pTLPane = GetPane(TMV_ACTIVEPANE);
		}
		
	}// if((sPane == TMV_LEFTPANE) || (sPane == TMV_RIGHTPANE))

	ASSERT(pTLPane != NULL); // TL pane should always be assigned

	//	Make sure the main pane is loaded
	//
	//	NOTE:	If split screen we've already verified both panes are loaded
	if(!pTLPane->IsLoaded())
	{
		m_Errors.Handle(0, IDS_TMV_NOIMAGE);
		return TMV_NOIMAGE;
	}

	//	Are we going to be printing callouts?
	if(m_bPrintCallouts)
	{
		if(pTLPane->GetCalloutCount() > 0)
			bCallouts = TRUE;
		else if((pBRPane != NULL) && (pBRPane->GetCalloutCount() > 0))
			bCallouts = TRUE;
	}

	//	Set up the rectangle using the caller's coordinates	
	rcBounds.left   = sLeft;
	rcBounds.top    = sTop;
	rcBounds.right  = rcBounds.left + sWidth;
	rcBounds.bottom = rcBounds.top  + sHeight;

	//	Should we check to see if the image should be rotated?
	if(bAutoRotate != 0)
	{
		//	What is the desired aspect ratio?
		//
		//	NOTE:	We always scale to screen for split screen
		if(bCallouts || bSplitScreen)
		{
			dImageRatio = pTLPane->GetScreenRatio();
		}
		else if(bFullImage)
		{
			dImageRatio = pTLPane->GetSrcWidth() / pTLPane->GetSrcHeight();
		}
		else
		{
			//	REPLACE THIS CODE WITH THE COMMENTED OUT CODE ONCE WE HAVE THE FLAG
			//	THAT LETS THE USER DETERMINE IF TREATMENTS SHOULD BE ROTATED AS WELL AS
			//	BASE IMAGES
			dImageRatio = pTLPane->GetDstWidth() / pTLPane->GetDstHeight();
			
			/*
			//	Use the visible portion of the source image
			RECT rcVisible;
			pPane->GetSrcVisible(&rcVisible);
			if((rcVisible.bottom - rcVisible.top) > 0)
				dImageRatio = ((float)(rcVisible.right - rcVisible.left)) / ((float)(rcVisible.bottom - rcVisible.top));
			*/
		}

		//	Is the callers rectangle Landscape oriented?
		if(sWidth > sHeight)
			bRotate = (dImageRatio < 1.0);
		else
			bRotate = (dImageRatio > 1.0);
	}

	//	Do we need to resize the rectangle to fit the screen ratio?
	if(bCallouts || bSplitScreen)
	{
		if(bRotate == TRUE)
			pTLPane->SetToRatio(&rcBounds, 1 / pTLPane->GetScreenRatio());
		else
			pTLPane->SetToScreenRatio(&rcBounds);
	}

	//	Are we split screen?
	if(bSplitScreen)
	{
		//	Get the extents of the bounding rectangle
		dMaxWidth = (double)(rcBounds.right - rcBounds.left);
		dMaxHeight = (double)(rcBounds.bottom - rcBounds.top);
		dCx = (double)(rcBounds.left) + (dMaxWidth / 2.0);
		dCy = (double)(rcBounds.top) + (dMaxHeight / 2.0);

		//	Set the rectangles for the individual images
		memcpy(&rcTLPane, &rcBounds, sizeof(rcTLPane));
		memcpy(&rcBRPane, &rcBounds, sizeof(rcBRPane));

		//	For now we assume there is no split screen frame
		//	
		//	If there is a frame it throws off the callouts positions
		ASSERT(m_sSplitFrameThickness == 0);

		if(m_bSplitHorizontal)
		{
			if(bRotate) // Counter clockwise rotation
			{
				rcTLPane.right = (int)dCx;
				rcBRPane.left = rcTLPane.right;
			}
			else
			{
				rcTLPane.bottom = (int)dCy;
				rcBRPane.top = rcTLPane.bottom;
			}
		}
		else
		{
			if(bRotate) // Counter clockwise rotation
			{
				rcTLPane.top = (int)dCy;
				rcBRPane.bottom = rcTLPane.top;
			}
			else
			{
				rcTLPane.right = (int)dCx;
				rcBRPane.left = rcTLPane.right;
			}
		}

		pTLPane->Print(pdc, &rcTLPane, &rcBounds, bFullImage, bRotate ? -90 : 0);
		pBRPane->Print(pdc, &rcBRPane, &rcBounds, bFullImage, bRotate ? -90 : 0);

		if(bCallouts)
		{
			if(pTLPane->GetCalloutCount() > 0)
				pTLPane->PrintCallouts(pdc, &rcBounds, bRotate ? -90 : 0);
			if(pBRPane->GetCalloutCount() > 0)
				pBRPane->PrintCallouts(pdc, &rcBounds, bRotate ? -90 : 0);
		}
	}
	else
	{
		//	Print the image and callouts using the full rectangle
		pTLPane->Print(pdc, &rcBounds, &rcBounds, bFullImage, bRotate ? -90 : 0);
		if(bCallouts)
			pTLPane->PrintCallouts(pdc, &rcBounds, bRotate ? -90 : 0);
	}

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::Realize()
//
// 	Description:	This external method allows the caller to realize the 
//					current annotations to the current image.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::Realize(short sPane) 
{
	return GetPane(sPane)->Realize(TRUE);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::RecalcLayout()
//
// 	Description:	This function will calculate the rectangles required to
//					draw the split screen views
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::RecalcLayout()
{
	//	Get the rectangle for the full client rectangle
 	GetClientRect(&m_rcMax);

	//	Calculate the full extents
	m_iWidth  = m_rcMax.right - m_rcMax.left;
	m_iHeight = m_rcMax.bottom - m_rcMax.top;

	//	Are we using horizontal split screening?
	if(m_bSplitHorizontal)
	{
		//	Calculate the left/top frame rectangle
		m_rcLFrame.left   = 0;
		m_rcLFrame.top    = 0;
		m_rcLFrame.right  = m_iWidth;
		m_rcLFrame.bottom = m_iHeight / 2;
		
		//	Calculate the right/bottom frame rectangle
		m_rcRFrame.left   = 0;
		m_rcRFrame.top    = m_rcLFrame.bottom;
		m_rcRFrame.right  = m_iWidth;
		m_rcRFrame.bottom = m_iHeight;
	}
	else
	{
		//	Calculate the left/top frame rectangle
		m_rcLFrame.left   = 0;
		m_rcLFrame.top    = 0;
		m_rcLFrame.right  = m_iWidth / 2;
		m_rcLFrame.bottom = m_iHeight;
		
		//	Calculate the right/bottom frame rectangle
		m_rcRFrame.left   = m_rcLFrame.right;
		m_rcRFrame.top    = 0;
		m_rcRFrame.right  = m_iWidth;
		m_rcRFrame.bottom = m_iHeight;
	}

	//	Calculate the left pane rectangle.
	//
	//	Note: The bottom/right members are used for width/height
	m_rcLPane.left   = m_rcLFrame.left + m_sSplitFrameThickness;
	m_rcLPane.top    = m_rcLFrame.top + m_sSplitFrameThickness;
	m_rcLPane.right  = (m_rcLFrame.right - m_rcLFrame.left) - (2 * m_sSplitFrameThickness);
	m_rcLPane.bottom = (m_rcLFrame.bottom - m_rcLFrame.top) - (2 * m_sSplitFrameThickness);

	//	Calculate the right pane rectangle
	//
	//	Note: The bottom/right members are used for width/height
	m_rcRPane.left   = m_rcRFrame.left + m_sSplitFrameThickness;
	m_rcRPane.top    = m_rcRFrame.top + m_sSplitFrameThickness;
	m_rcRPane.right  = (m_rcRFrame.right - m_rcRFrame.left) - (2 * m_sSplitFrameThickness);
	m_rcRPane.bottom = (m_rcRFrame.bottom - m_rcRFrame.top) - (2 * m_sSplitFrameThickness);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::Redraw()
//
// 	Description:	This is an external method that allows the caller to redraw
//					the control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::Redraw() 
{
	if(m_bSplitScreen)
	{
		m_PaneA.Redraw();
		m_PaneB.Redraw();
	}
	else
	{
		GetPane()->Redraw();
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::Render()
//
// 	Description:	This function will render the image into the dc provided
//					by the caller at the position provided by the caller.
//
// 	Returns:		0 if successful
//					TMV_NOIMAGE if not loaded
//					LeadTools error level otherwise
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::Render(OLE_HANDLE hDc, float fLeft, float fTop, 
						  float fWidth, float fHeight, short sPane) 
{
	return GetPane(sPane)->Render(hDc, fLeft, fTop, fWidth, fHeight);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::RescaleZapCallouts()
//
// 	Description:	This is an external method that allows the caller to force
//					rescaling of the callouts in a zap file.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::RescaleZapCallouts() 
{
	if(m_bSplitScreen)
	{
		m_PaneA.RescaleZapCallouts();
		m_PaneB.RescaleZapCallouts();
	}
	else
	{
		GetPane()->RescaleZapCallouts();
	}

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::ResetZoom()
//
// 	Description:	This function will reset the zoom factor and redisplay the
//					image with a factor of 1.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::ResetZoom(short sPane) 
{
	GetPane(sPane)->ResetZoom();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::ResizeSourceToImage()
//
// 	Description:	This external method allows the caller to set the current
//					source rectangle to include the full image
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::ResizeSourceToImage(short sPane) 
{
	GetPane(sPane)->ResizeSourceToImage();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::ResizeSourceToView()
//
// 	Description:	This external method allows the caller to resize the source
//					to match the current view port.
//
// 	Returns:		None
//
//	Notes:			This method is typically used to resize the source before
//					printing. The image is not redrawn thus giving the caller
//					a chance to restore the source rectangle after printing
//
//==============================================================================
void CTMViewCtrl::ResizeSourceToView(short sPane) 
{
	GetPane(sPane)->ResizeSourceToView();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::Rotate()
//
// 	Description:	This external method allows the caller to rotate the image
//					by the amount determined by the rotation property.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::Rotate(BOOL bRedraw, short sPane) 
{
	GetPane(sPane)->Rotate(bRedraw);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::RotateCcw()
//
// 	Description:	This external method allows the caller to rotate the image
//					counter-clockwise 90 degrees.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::RotateCcw(BOOL bRedraw, short sPane) 
{
	GetPane(sPane)->RotateCcw(bRedraw);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::RotateCw()
//
// 	Description:	This external method allows the caller to rotate the image
//					clockwise 90 degrees.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::RotateCw(BOOL bRedraw, short sPane) 
{
	GetPane(sPane)->RotateCw(bRedraw);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::Save()
//
// 	Description:	This external method allows the caller to save the current
//					image using the specified filename.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::Save(LPCTSTR lpszFilename, short sPane) 
{
	return GetPane(sPane)->Save(lpszFilename);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::SavePages()
//
// 	Description:	This external method is called to save the pages of the
//					specified file to the requested folder
//
// 	Returns:		The total number of saved pages
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::SavePages(LPCTSTR lpszFilename, LPCTSTR lpszFolder, LPCTSTR lpszPrefix)  
{
	return GetPane(TMV_ACTIVEPANE)->SavePages(lpszFilename, lpszFolder, lpszPrefix);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::SaveProperties()
//
// 	Description:	This method is called to save the control properties to the
//					specified initialization file.
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::SaveProperties(LPCTSTR lpszFilename, LPCTSTR lpszSection) 
{
	CTMIni				Ini;
	SGraphicsOptions	GO;

	//	Open the ini file
	Ini.Open(lpszFilename, lpszSection);
	
	//	Read the values from the INI file to initialize the graphics structure
	Ini.ReadGraphicsOptions(&GO);

	//	Update the values
	GO.sAnnColor = GetPane()->m_sAnnColor;
	GO.sHighlightColor = GetPane()->m_sHighlightColor;
	GO.sCalloutColor = GetPane()->m_sCalloutColor;
	GO.sCalloutFrameColor = GetPane()->m_sCallFrameColor;
	GO.sCalloutHandleColor = GetPane()->m_sCallHandleColor;
	GO.sCalloutShadeGrayscale = (short)(GetPane()->m_crCalloutShadeBackground & 0xFF);
	GO.sRedactColor = GetPane()->m_sRedactColor;
	GO.sAnnThickness = GetPane()->m_sAnnThickness;
	GO.sCalloutFrameThickness = GetPane()->m_sCallFrameThick;
	GO.sMaxZoom = (short)GetPane()->m_fMaxZoom;
	GO.sAnnTool = GetPane()->m_sAnnTool;
	GO.bShadeOnCallout = GetPane()->m_bShadeOnCallout;
	GO.bResizableCallouts = GetPane()->GetResizeCallouts();
	GO.bPanCallouts = GetPane()->GetPanCallouts();
	GO.bZoomCallouts = GetPane()->GetZoomCallouts();
	GO.sBitonalScaling = GetPane()->m_sBitonal;
	GO.strAnnFontName = GetPane()->m_strAnnFontName;
	GO.sAnnFontSize = GetPane()->m_sAnnFontSize;
	GO.bAnnFontBold = GetPane()->m_bAnnFontBold;
	GO.bAnnFontStrikeThrough = GetPane()->m_bAnnFontStrikeThrough;
	GO.bAnnFontUnderline = GetPane()->m_bAnnFontUnderline;
	
	//	Update the INI file
	Ini.WriteGraphicsOptions(&GO);

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::SaveSplitZap()
//
// 	Description:	This external method allows the caller to save the current
//					annotations in each pane to the specified zap files.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::SaveSplitZap(LPCTSTR lpszTLFilename, LPCTSTR lpszBRFilename) 
{
	CString strError = "";
	CString	strMessage = "";
	short	sError = TMV_NOERROR;

	while(1)
	{
		//	Must be in split screen mode
		if(m_bSplitScreen == FALSE)
		{
			strError = "Viewer must be in split screen mode to perform this operation";
			sError = TMV_NOTSPLITSCREEN;
			break;
		}

		//	Both panes must be loaded
		if((GetPane(TMV_LEFTPANE)->IsLoaded() == FALSE) || 
		   (GetPane(TMV_RIGHTPANE)->IsLoaded() == FALSE))
		{
			strError = "Both panes must be loaded to perform this operation.";
			sError = TMV_NOTSPLITSCREEN;
			break;
		}

		break;
	
	}// while(1)

	//	Did an error occur?
	if(sError != TMV_NOERROR)
	{
		strMessage = "Unable to save split screen treatment:\n\n";
		strMessage += strError;
		m_Errors.Handle(0, strMessage);
	}
MessageBox(strMessage, "SAVE SPLIT");
	return sError;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::SaveZap()
//
// 	Description:	This external method allows the caller to save the current
//					annotations to the specified zap file.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::SaveZap(LPCTSTR lpszFilename, short sPane) 
{
	SZapHeader	Header;
	SZapFooter	Footer;

	//	Set default values for header and footer
	FillZapHeader(&Header);
	memset(&Footer, 0, sizeof(Footer));

	//	Make sure to set single-pane zap properties
	Header.bSplitScreen = FALSE;

	//	Save the file
	return SaveZapEx(lpszFilename, &Header, &Footer, sPane);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::SaveZapEx()
//
// 	Description:	This method will create a zap file for the specified pane
//					using the header provided by the caller.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::SaveZapEx(LPCTSTR lpszFilename, SZapHeader* pZapHeader, SZapFooter* pZapFooter, short sPane) 
{
	char szErrorMsg[256];

	try
	{
		//	Create and open the capture file
		CFile Zap(lpszFilename, CFile::modeCreate | CFile::modeWrite);

		//	Write the header
		Zap.Write(pZapHeader, sizeof(SZapHeader));

		//	Write the active pane
		if(GetPane(sPane)->SaveZap(&Zap))
		{
			//	Write the footer
			//
			//	NOTE:	We originally planned to put split-screen treatment
			//			information into the zap file but changed our mind
			//			during development. Adding a footer would have allowed us
			//			to add the information and maintain backward compatability.
			//			The code has been left in place for future use if desired
			Zap.Write(pZapFooter, sizeof(SZapFooter));

			Zap.Close();
			return TMV_NOERROR;
		}
		else
		{
			Zap.Close();
			return TMV_SAVEZAPFAILED;
		}
	}
	//	Catch all file exceptions
	catch(CFileException* pFileException)
	{
		//	Get the error message and pass it on to the handler
		pFileException->GetErrorMessage(szErrorMsg, sizeof(szErrorMsg));
		m_Errors.Handle(0, szErrorMsg);
		pFileException->Delete();

		return TMV_SAVEZAPFAILED;
	}
	//	Catch all other exceptions
	catch(CException* pException)
	{
		//	Get the error message and pass it on to the handler
		pException->GetErrorMessage(szErrorMsg, sizeof(szErrorMsg));
		m_Errors.Handle(0, szErrorMsg);
		pException->Delete();

		return TMV_SAVEZAPFAILED;
	}

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::SetAnnotationProperties()
//
// 	Description:	This method is called to pop up a dialog box that allows the
//					user to set the control properties at runtime. 
//
// 	Returns:		IDOK if successful.
//
//	Notes:			We process this method here rather than pass it on to the
//					LeadDlg object so that we have access to the control
//					properties.
//
//==============================================================================
short CTMViewCtrl::SetAnnotationProperties(long lFlags) 
{
	CAnnotationProperties Dialog(lFlags, this);

	//	Initialize the dialog box members using the active pane
	Dialog.m_nDrawColor = GetPane()->m_sAnnColor;
	Dialog.m_nHighlightColor = GetPane()->m_sHighlightColor;
	Dialog.m_nCalloutColor = GetPane()->m_sCalloutColor;
	Dialog.m_nCalloutFrameColor = GetPane()->m_sCallFrameColor;
	Dialog.m_nCalloutHandleColor = GetPane()->m_sCallHandleColor;
	Dialog.m_sCalloutShadeGrayscale = (short)(GetPane()->m_crCalloutShadeBackground & 0xFF);
	Dialog.m_nRedactColor = GetPane()->m_sRedactColor;
	Dialog.m_nBackgroundColor = GetPane()->GetColorId(TranslateColor(GetBackColor()));
	Dialog.m_nThickness = GetPane()->m_sAnnThickness;
	Dialog.m_nFrameThickness = GetPane()->m_sCallFrameThick;
	Dialog.m_nZoomFactor = (short)GetPane()->m_fMaxZoom;
	Dialog.m_nTool = GetPane()->m_sAnnTool;
	Dialog.m_bShadeOnCallout = GetPane()->m_bShadeOnCallout;
	Dialog.m_bResizeableCallouts = GetPane()->GetResizeCallouts();
	Dialog.m_bPanCallouts = GetPane()->GetPanCallouts();
	Dialog.m_bZoomCallouts = GetPane()->GetZoomCallouts();
	Dialog.m_bFitToImage = GetPane()->m_bFitToImage;
	Dialog.m_bScaleImage = GetPane()->m_bScaleImage;
	Dialog.m_bKeepAspect = GetPane()->m_bKeepAspect;
	Dialog.m_nRotation = GetPane()->m_sRotation;
	Dialog.m_bHideScrollBars = GetPane()->m_bHideScrollBars;
	Dialog.m_bRightClickPan = GetPane()->m_bRightClickPan;
	Dialog.m_nPanPercent = (short)(GetPane()->m_fPanPercent * 100);
	Dialog.m_sZoomOnLoad = GetPane()->m_sZoomOnLoad;
	Dialog.m_bSyncCalloutAnn = GetPane()->m_bSyncCalloutAnn;
	Dialog.m_iBitonal = GetPane()->m_sBitonal;
	Dialog.m_bZoomToRect = GetPane()->m_bZoomToRect;
	Dialog.m_strFontName = GetPane()->m_strAnnFontName;
	Dialog.m_sFontSize = GetPane()->m_sAnnFontSize;
	Dialog.m_bFontBold = GetPane()->m_bAnnFontBold;
	Dialog.m_bFontStrikeThrough = GetPane()->m_bAnnFontStrikeThrough;
	Dialog.m_bFontUnderline = GetPane()->m_bAnnFontUnderline;

	//	Get the control level properties
	Dialog.m_nSplitFrameColor = m_sSplitFrameColor;
	Dialog.m_nSplitThickness = m_sSplitFrameThickness;
	Dialog.m_bSplitScreen = m_bSplitScreen;
	Dialog.m_bSyncPanes = m_bSyncPanes; 
	Dialog.m_bSelectorVisible = m_bPenSelectorVisible;
	Dialog.m_nSelectorColor = m_sPenSelectorColor;
	Dialog.m_nSelectorSize = m_sPenSelectorSize;
	
	//	Clear the text box counter
	m_iTextOpen = 0;

	//	Open the dialog box
	if(Dialog.DoModal() != IDOK)
		return IDCANCEL;

	//	Save the new values
	m_sAnnColor = Dialog.m_nDrawColor;
	m_sHighlightColor = Dialog.m_nHighlightColor;
	m_sCalloutColor = Dialog.m_nCalloutColor;
	m_sCalloutFrameColor = Dialog.m_nCalloutFrameColor;
	m_sCalloutHandleColor = Dialog.m_nCalloutHandleColor;
	m_sCalloutShadeGrayscale = Dialog.m_sCalloutShadeGrayscale;
	m_sRedactColor = Dialog.m_nRedactColor;
	m_sAnnThickness = Dialog.m_nThickness;
	m_sCalloutFrameThickness = Dialog.m_nFrameThickness;
	m_sMaxZoom = Dialog.m_nZoomFactor;
	m_sAnnTool = Dialog.m_nTool;
	m_sZoomOnLoad = Dialog.m_sZoomOnLoad;
	m_bShadeOnCallout = Dialog.m_bShadeOnCallout;
	m_bResizeCallouts = Dialog.m_bResizeableCallouts;
	m_bPanCallouts = Dialog.m_bPanCallouts;
	m_bZoomCallouts = Dialog.m_bZoomCallouts;
	m_bFitToImage = Dialog.m_bFitToImage;
	m_bScaleImage = Dialog.m_bScaleImage;
	m_bKeepAspect = Dialog.m_bKeepAspect;
	m_bRightClickPan = Dialog.m_bRightClickPan;
	m_bHideScrollBars = Dialog.m_bHideScrollBars;
	m_sPanPercent = Dialog.m_nPanPercent;
	m_sRotation = Dialog.m_nRotation;
	m_bSyncCalloutAnn = Dialog.m_bSyncCalloutAnn;
	m_sBitonal = Dialog.m_iBitonal;
	m_bZoomToRect = Dialog.m_bZoomToRect;
	m_strAnnFontName = Dialog.m_strFontName;
	m_sAnnFontSize = Dialog.m_sFontSize;
	m_bAnnFontBold = Dialog.m_bFontBold;
	m_bAnnFontStrikeThrough = Dialog.m_bFontStrikeThrough;
	m_bAnnFontUnderline = Dialog.m_bFontUnderline;

	//	Make sure the grayscale value is within range
	if(m_sCalloutShadeGrayscale < 0)
		m_sCalloutShadeGrayscale = 0;
	else if(m_sCalloutShadeGrayscale > 255)
		m_sCalloutShadeGrayscale = 255;

	//	Set the pane properties
	if(m_bSyncPanes)
	{
		SetPaneProps(&m_PaneA);
		SetPaneProps(&m_PaneB);
	}
	else
	{
		SetPaneProps(GetPane());
	}

	//	Set the control level properties
	m_sSplitFrameColor = Dialog.m_nSplitFrameColor;
	m_sSplitFrameThickness = Dialog.m_nSplitThickness;
	m_bSplitScreen = Dialog.m_bSplitScreen;
	m_bSyncPanes = Dialog.m_bSyncPanes; 
	m_bPenSelectorVisible = Dialog.m_bSelectorVisible;
	m_sPenSelectorColor = Dialog.m_nSelectorColor;
	m_sPenSelectorSize = Dialog.m_nSelectorSize;
	SetBackColor((OLE_COLOR)m_PaneA.GetColorRef(Dialog.m_nBackgroundColor));

	//	Redraw the control
	OnSplitScreenChanged();

	return IDOK;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::SetColor()
//
// 	Description:	This external method allows the caller to set the 
//					color associated with the active tool. If the panes are
//					synchronized the color for each pane is set. Otherwise 
//					only the color for the active pane is changed.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::SetColor(short sColor) 
{
	if(m_bSyncPanes)
	{
		m_PaneA.SetColor(sColor);
		m_PaneB.SetColor(sColor);
	}
	else
	{
		GetPane()->SetColor(sColor);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::SetData()
//
// 	Description:	This external method allows the caller to set a value to
//					be associated with the specified frame. The data is not 
//					used by this control but can be retrieved with a call to
//					GetData()
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::SetData(short sPane, long lData) 
{
	GetPane(sPane)->m_lUserData = lData;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::SetPane()
//
// 	Description:	This function will set the active pane to that provided by
//					the caller.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::SetPane(CTMLead* pPane) 
{
	ASSERT(pPane);

	//	Is this already the active pane?
	if((m_pActive == pPane) || (m_bLoadingZap == TRUE))
		return;

	//	Update the pane identifier
	m_sActivePane = (pPane == m_pRight) ? TMV_RIGHTPANE : TMV_LEFTPANE;
	OnActivePaneChanged();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::SetPanePrintRect()
//
// 	Description:	This function will set the coordinates of the print 
//					rectangle for the pane specified by the caller
//
// 	Returns:		None
//
//	Notes:			This method assumes the specified bounding rectangle
//					has already been scaled to the screen ratio
//
//==============================================================================
void CTMViewCtrl::SetPanePrintRect(RECT* prcBounds, RECT* prcPrint, 
								   CTMLead& rwndPane, short sRotation) 
{
	RECT	rcPaneWnd;
	double	dBoundsCx;
	double	dBoundsCy;
	double	dBoundsWidth;
	double	dBoundsHeight;
	double	dScaleFactor;
	double	dPaneWndCx;
	double	dPaneWndCy;
	double	dPaneWndWidth;
	double	dPaneWndHeight;
	double	dScreenCx;
	double	dScreenCy;
	double	dScreenWidth;
	double	dScreenHeight;
	double	dPrintWidth;
	double	dPrintHeight;
	double	dPrintLeft;
	double	dPrintTop;

	ASSERT(prcBounds);
	ASSERT(prcPrint);

	//	Get the current position of the pane window on the screen
	rwndPane.GetPaneScreenRect(&rcPaneWnd, TRUE);

rwndPane.ShowRectangle(&rcPaneWnd);
rwndPane.ShowRectangle(prcBounds);

	//	Calculate the size and center point of the pane window
	dPaneWndWidth  = (double)(rcPaneWnd.right - rcPaneWnd.left);
	dPaneWndHeight = (double)(rcPaneWnd.bottom - rcPaneWnd.top);
	dPaneWndCx     = (double)rcPaneWnd.left + (dPaneWndWidth / 2.0);
	dPaneWndCy     = (double)rcPaneWnd.top  + (dPaneWndHeight / 2.0);

	//	Calculate the size and center point of the callout window
	dScreenWidth  = (double)rwndPane.GetScreenWidth();
	dScreenHeight = (double)rwndPane.GetScreenHeight();
	dScreenCx     = (dScreenWidth / 2.0);
	dScreenCy     = (dScreenHeight / 2.0);

	//	Calculate the size and center point of the bounding rectangle
	dBoundsWidth  = (double)(prcBounds->right - prcBounds->left);
	dBoundsHeight = (double)(prcBounds->bottom - prcBounds->top);
	dBoundsCx	  = ((double)(prcBounds->left)) + (dBoundsWidth / 2.0);
	dBoundsCy	  = ((double)(prcBounds->top)) + (dBoundsHeight / 2.0);

	//	Compute the scale factor to convert from pixels to print units
	//
	//	NOTE:	We assume the bounding rectangle has been scaled to match
	//			the screen ratio
	if(sRotation != 0)
		dScaleFactor = dBoundsHeight / dScreenWidth;
	else
		dScaleFactor = dBoundsWidth / dScreenWidth;

	//	Calculate the print size and offset allowing for rotation
	if(sRotation < 0)
	{
		//dPrintCx = dScreenYOffset;
		//dPrintCy = dScreenXOffset * -1.0;
		//dPrintWidth   = dCallWndHeight;
		//dPrintHeight  = dCallWndWidth;
	}
	else if(sRotation > 0)
	{
		//dPrintCx = dScreenYOffset * -1.0;
		//dPrintCy = dScreenXOffset;
		//dPrintWidth   = dCallWndHeight;
		//dPrintHeight  = dCallWndWidth;
	}
	else
	{
		dPrintLeft    = (double)(prcBounds->left) + (((double)(rcPaneWnd.left)) * dScaleFactor);
		dPrintTop     = (double)(prcBounds->top) + (((double)(rcPaneWnd.top)) * dScaleFactor);
		dPrintWidth   = dPaneWndWidth * dScaleFactor;
		dPrintHeight  = dPaneWndHeight * dScaleFactor;
	}

	//	Convert from screen coordinates to print rectangle coordinates
/*
	dPrintCx	 *= dScaleFactor;
	dPrintCy	 *= dScaleFactor;
	dPrintWidth  *= dScaleFactor;
	dPrintHeight *= dScaleFactor;
*/
	//	Calculate the coordinates of the print rectangle
	prcPrint->left   = ROUND(dPrintLeft);
	prcPrint->top    = ROUND(dPrintTop);
	prcPrint->right  = prcPrint->left + ROUND(dPrintWidth);
	prcPrint->bottom = prcPrint->top + ROUND(dPrintHeight);
rwndPane.ShowRectangle(prcPrint);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::SetPaneProps()
//
// 	Description:	This function will set the properties for the pane provided
//					by the caller
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::SetPaneProps(CTMLead* pPane) 
{
	ASSERT(pPane);
	
	pPane->SetScaleImage(m_bScaleImage);
	pPane->SetKeepAspect(m_bKeepAspect);
	pPane->SetFitToImage(m_bFitToImage);
	pPane->SetAnnTool(m_sAnnTool);
	pPane->SetAnnThickness(m_sAnnThickness);
	pPane->SetAnnColor(m_sAnnColor);
	pPane->SetAnnColorDepth(m_sAnnColorDepth);
	pPane->SetRedactColor(m_sRedactColor);
	pPane->SetHighlightColor(m_sHighlightColor);
	pPane->SetCalloutColor(m_sCalloutColor);
	pPane->SetCallFrameThickness(m_sCalloutFrameThickness);
	pPane->SetCallFrameColor(m_sCalloutFrameColor);
	pPane->SetCallHandleColor(m_sCalloutHandleColor);
	pPane->SetCalloutShadeColor(RGB(m_sCalloutShadeGrayscale, m_sCalloutShadeGrayscale, m_sCalloutShadeGrayscale));
	pPane->SetShadeOnCallout(m_bShadeOnCallout);
	pPane->SetResizeCallouts(m_bResizeCallouts);
	pPane->SetPanCallouts(m_bPanCallouts);
	pPane->SetZoomCallouts(m_bZoomCallouts);
	pPane->SetMaxZoom(m_sMaxZoom);
	pPane->SetRotation(m_sRotation);
	pPane->SetAction(m_sAction);
	pPane->SetAutoAnimate(m_bAutoAnimate);
	pPane->SetLoopAnimate(m_bLoopAnimation);
	pPane->SetHideScrollBars(m_bHideScrollBars);
	pPane->SetPanPercent(m_sPanPercent);
	pPane->SetRightClickPan(m_bRightClickPan);
	pPane->SetZoomOnLoad(m_sZoomOnLoad);
	pPane->SetSyncCalloutAnn(m_bSyncCalloutAnn);
	pPane->SetBitonal(m_sBitonal);
	pPane->SetZoomToRect(m_bZoomToRect);
	pPane->SetAnnFontName(m_strAnnFontName);
	pPane->SetAnnFontSize(m_sAnnFontSize);
	pPane->SetAnnFontBold(m_bAnnFontBold);
	pPane->SetAnnFontStrikeThrough(m_bAnnFontStrikeThrough);
	pPane->SetAnnFontUnderline(m_bAnnFontUnderline);
	pPane->SetAnnotateCallouts(m_bAnnotateCallouts);
	pPane->SetDeskewBackColor(TranslateColor(m_lDeskewBackColor));
	pPane->SetPrintBorderColor(TranslateColor(m_lPrintBorderColor));
	pPane->SetPrintBorderThickness(m_fPrintBorderThickness);
	pPane->SetPrintBorder(m_bPrintBorder);
	pPane->SetPrintCalloutBorders(m_bPrintCalloutBorders);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::SetPrinter()
//
// 	Description:	This function will open a print setup dialog that allows
//					the user to select the printer to be used for print jobs
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::SetPrinter()
{
	m_Printer.Select(this);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::SetPrinterByName()
//
// 	Description:	This external method allows the caller to set the name of
//					the printer used by this control.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTMViewCtrl::SetPrinterByName(LPCTSTR lpName) 
{
	m_Printer.SetName(lpName);
	return TRUE;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::SetPrinterProperties()
//
// 	Description:	This external method allows the caller to invoke the
//					properties page for the active printer
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
BOOL CTMViewCtrl::SetPrinterProperties(OLE_HANDLE hWnd) 
{
	int iReturn = 0;

	iReturn = m_Printer.SetProperties((hWnd != 0) ? (HWND)hWnd : m_hWnd);

	if(iReturn < 0)
	{
		m_Errors.Handle(0, m_Printer.GetErrorMsg());
	}

	return (iReturn == IDOK);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::SetProperties()
//
// 	Description:	This method is called to set the control properties using
//					values contained in the specified initialization file.
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::SetProperties(LPCTSTR lpszFilename, LPCTSTR lpszSection) 
{
	CTMIni				Ini;
	SGraphicsOptions	GO;

	//	Open the ini file
	if(!Ini.Open(lpszFilename, lpszSection))
	{
		m_Errors.Handle(0, IDS_TMV_OPENINIFAILED, lpszFilename);
		return TMV_OPENINIFAILED;
	}
	
	//	Read the values from the INI file
	Ini.ReadGraphicsOptions(&GO);
	
	//	Save the new values
	m_sAnnColor = GO.sAnnColor;
	m_sHighlightColor = GO.sHighlightColor;
	m_sCalloutColor = GO.sCalloutColor;
	m_sCalloutFrameColor = GO.sCalloutFrameColor;
	m_sCalloutHandleColor = GO.sCalloutHandleColor;
	m_sCalloutShadeGrayscale = GO.sCalloutShadeGrayscale;
	m_sRedactColor = GO.sRedactColor;
	m_sAnnThickness = GO.sAnnThickness;
	m_sCalloutFrameThickness = GO.sCalloutFrameThickness;
	m_sMaxZoom = GO.sMaxZoom;
	m_sAnnTool = GO.sAnnTool;
	m_bShadeOnCallout = GO.bShadeOnCallout;
	m_bResizeCallouts = GO.bResizableCallouts;
	m_bPanCallouts = GO.bPanCallouts;
	m_bZoomCallouts = GO.bZoomCallouts;
	m_sBitonal = GO.sBitonalScaling;
	m_strAnnFontName = GO.strAnnFontName;
	m_sAnnFontSize = GO.sAnnFontSize;
	m_bAnnFontBold = GO.bAnnFontBold;
	m_bAnnFontStrikeThrough = GO.bAnnFontStrikeThrough;
	m_bAnnFontUnderline = GO.bAnnFontUnderline;

	SetPaneProps(&m_PaneA);
	SetPaneProps(&m_PaneB);

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::SetupPrintPage()
//
// 	Description:	This external method invokes a dialog box that allows the
//					user to define the properties for the printed page.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::SetupPrintPage() 
{
	CPageSetup Setup(this);

	//	Initialize the dialog box
	Setup.m_fLeftMargin = m_fPrintLeftMargin;
	Setup.m_fRightMargin = m_fPrintRightMargin;
	Setup.m_fTopMargin = m_fPrintTopMargin;
	Setup.m_fBottomMargin = m_fPrintBottomMargin;
	Setup.m_fBorderThickness = m_fPrintBorderThickness;
	Setup.m_crBorderColor = TranslateColor(m_lPrintBorderColor);
	Setup.m_iOrientation = m_sPrintOrientation;
	Setup.m_bPrintCallouts = m_bPrintCallouts;
	Setup.m_bPrintBorder = m_bPrintBorder;
	Setup.m_bPrintCalloutBorders = m_bPrintCalloutBorders;

	//	Let the container know we are about to open the dialog box
	PreModalDialog();

	//	Open the dialog box
	if(Setup.DoModal() == IDOK)
	{
		m_fPrintLeftMargin = Setup.m_fLeftMargin;
		m_fPrintRightMargin = Setup.m_fRightMargin;
		m_fPrintTopMargin = Setup.m_fTopMargin;
		m_fPrintBottomMargin = Setup.m_fBottomMargin;
		m_fPrintBorderThickness = Setup.m_fBorderThickness;
		m_lPrintBorderColor = (OLE_COLOR)Setup.m_crBorderColor;
		m_sPrintOrientation = Setup.m_iOrientation;
		m_bPrintCallouts = Setup.m_bPrintCallouts;
		m_bPrintBorder = Setup.m_bPrintBorder;
		m_bPrintCalloutBorders = Setup.m_bPrintCalloutBorders;

		//	Update the panes
		OnPrintBorderThicknessChanged();
		OnPrintBorderColorChanged();
		OnPrintBorderChanged();
		OnPrintCalloutBordersChanged();
	}

	//	Let the container know we are about to close the dialog box
	PostModalDialog();

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::ShowCallouts()
//
// 	Description:	This external method allows the caller to show or hide
//					the callouts
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::ShowCallouts(BOOL bShow, short sPane) 
{
	return GetPane(sPane)->ShowCallouts(bShow);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::ShowDiagnostics()
//
// 	Description:	This function will show / hide the diagnostics window
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::ShowDiagnostics(BOOL bShow) 
{
	//	Create the diagnostics window
	#if defined _DEBUG
		
		if(!IsWindow(m_Diagnostics.m_hWnd))
			m_Diagnostics.Create(this);

		m_Diagnostics.ShowWindow(bShow ? SW_SHOW : SW_HIDE);
	#endif

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::ZoomFullWidth()
//
// 	Description:	This external method allows the caller to display the dialog
//					that shows the capabilities of all system printers
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::ShowPrinterCaps() 
{
	//	Let the container know we are about to open the dialog box
	PreModalDialog();

	CFPrinterCaps printerCaps(this);
	printerCaps.DoModal();

	//	Let the container know we are done
	PostModalDialog();


	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::ShowRectangles()
//
// 	Description:	This function is provided as a debugging aid. It will 
//					display the dimensions of the display rectangles.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::ShowRectangles(LPCSTR lpTitle) 
{
	CString	Msg;
	CString Tmp;

	//	Bounding window rectangle
	Msg.Format("MaxLeft %d\nMaxTop %d\nMaxRight %d\nMaxBottom %d\n\n",
			   m_rcMax.left, m_rcMax.top, m_rcMax.right, m_rcMax.bottom);

	//	Left Frame rectangle
	Tmp.Format("LFrameLeft %d\nLFrameTop %d\nLFrameRight %d\nLFrameBottom %d\n\n",
			   m_rcLFrame.left, m_rcLFrame.top, m_rcLFrame.right, m_rcLFrame.bottom);
	Msg += Tmp;

	//	Right Frame rectangle
	Tmp.Format("RFrameLeft %d\nRFrameTop %d\nRFrameRight %d\nRFrameBottom %d\n\n",
			   m_rcRFrame.left, m_rcRFrame.top, m_rcRFrame.right, m_rcRFrame.bottom);
	Msg += Tmp;

	//	Left Pane rectangle
	Tmp.Format("LPaneLeft %d\nLPaneTop %d\nLPaneRight %d\nLPaneBottom %d\n\n",
			   m_rcLPane.left, m_rcLPane.top, m_rcLPane.right, m_rcLPane.bottom);
	Msg += Tmp;

	//	Right Pane rectangle
	Tmp.Format("RPaneLeft %d\nRPaneTop %d\nRPaneRight %d\nRPaneBottom %d\n\n",
			   m_rcRPane.left, m_rcRPane.top, m_rcRPane.right, m_rcRPane.bottom);
	Msg += Tmp;

	MessageBox(Msg, lpTitle, MB_ICONINFORMATION | MB_OK);

}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::Smooth()
//
// 	Description:	This external method allows the caller to smooth bumps and
//					nicks in 1-bit images.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::Smooth(short sPane, long lLength, short sFavorLong) 
{
	return GetPane(sPane)->Smooth(lLength, sFavorLong);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::SwapPanes()
//
// 	Description:	This method is called to swap the pane positions
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::SwapPanes() 
{
	RECT rcInactive;

	//	Prevent attempts to redraw 
	m_bRedraw = FALSE;

	//	Swap the left and right pane assignments
	m_pLeft = m_pRight;
	m_pRight = (m_pLeft == &m_PaneA) ? &m_PaneB : &m_PaneA;

	ASSERT(m_pLeft);
	ASSERT(m_pRight);

	if(m_bSplitScreen)
	{
		//	Size each pane
		m_pLeft->SetMaxRect(&m_rcLPane, TRUE, TRUE);
		m_pRight->SetMaxRect(&m_rcRPane, TRUE, TRUE);

		//	Both panes should be visible if they are loaded
		if(m_pLeft->IsLoaded())
		{
			m_pLeft->ShowWindow(SW_SHOW);
			m_pLeft->ShowCallouts(TRUE);
		}
		if(m_pRight->IsLoaded())
		{
			m_pRight->ShowCallouts(TRUE);
			m_pRight->ShowWindow(SW_SHOW);
		}
	}
	else
	{
		//	Size the active pane
		m_pLeft->SetMaxRect(&m_rcMax, FALSE, TRUE);

		if(m_pLeft->IsLoaded())
		{
			m_pLeft->ShowWindow(SW_SHOW);
			m_pLeft->ShowCallouts(TRUE);
		}

		//	The inactive pane should not be visible
		m_pRight->ShowCallouts(FALSE);
		m_pRight->ShowWindow(SW_HIDE);

		//	Make sure the inactive is shrunk to zero
		memset(&rcInactive, 0, sizeof(rcInactive));
		m_pRight->SetMaxRect(&rcInactive, FALSE, TRUE);

	}

	//	Make sure the pane is properly selected
	if(m_sActivePane == TMV_LEFTPANE)
		m_pActive = m_pLeft;
	else
		m_pActive = m_pRight;
	FireSelectPane(m_sActivePane);

	//	Redraw the windows
	m_bRedraw = TRUE;
	RedrawWindow();

	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::UnlockAnn()
//
// 	Description:	This function will unlock the specified annotation
//
// 	Returns:		TMV_NOERROR if successful
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::UnlockAnn(long lAnnotation, short sPane) 
{
	return GetPane(sPane)->UnlockAnn((HANNOBJECT)lAnnotation);
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::ViewImageProperties()
//
// 	Description:	This method is called to display a dialog box that shows
//					the properties of the image in the specified pane.
//
// 	Returns:		TMV_NOERROR if successful.
//
//	Notes:			None
//
//==============================================================================
short CTMViewCtrl::ViewImageProperties(short sPane) 
{
	short sReturn;

	//	Are we in user mode?
	if(AmbientUserMode())
	{
		//	Is the user requesting the active pane?
		if(sPane != TMV_LEFTPANE && sPane != TMV_RIGHTPANE)
			sPane = m_sActivePane;

		//	Notify the container we are about to open a modal dialog
		PreModalDialog();

		//	Load the file into the requested pane
		if(sPane == TMV_RIGHTPANE)
		{
			sReturn = GetPane(TMV_RIGHTPANE)->ViewImageProperties();
		}
		else
		{
			sReturn = GetPane(TMV_LEFTPANE)->ViewImageProperties();
		}

		//	Notify the container
		PostModalDialog();

		return sReturn;
	}
	
	return TMV_NOERROR;
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::ZoomFullHeight()
//
// 	Description:	This external method allows the caller to scale the current
//					image to the full available height.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::ZoomFullHeight(short sPane) 
{
	GetPane(sPane)->ZoomFullHeight();
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::ZoomFullWidth()
//
// 	Description:	This external method allows the caller to scale the current
//					image to the full available width.
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::ZoomFullWidth(short sPane) 
{
	GetPane(sPane)->ZoomFullWidth();
}


//==============================================================================
//
// 	Function Name:	CTMViewCtrl::DoGesturePan()
//
// 	Description:	This external method is used to Pan object on pan gesture
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
bool CTMViewCtrl::DoGesturePan(LONG lCurrentX, LONG lCurrentY, LONG lLastX, LONG lLastY, bool* bSmooth)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	GetPane(TMV_ACTIVEPANE)->GesturePan(lCurrentX - lLastX, lCurrentY - lLastY);

	short sPanStates = GetPane(TMV_ACTIVEPANE)->GetPanStates();

	if(lLastY < lCurrentY) {

		return (sPanStates & ENABLE_PANUP);
	} else {
		return (sPanStates & ENABLE_PANDOWN);
	}
}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::DoGestureZoom()
//
// 	Description:	This external method is used to zoom in / zoom out 
//					object on pinch pinch gestures
//
// 	Returns:		None
//
//	Notes:			zoomfactor > 1 = zoom in 
//					zoomfactor < 1 = zoom out
//
//==============================================================================
void CTMViewCtrl::DoGestureZoom(FLOAT zoomFactor)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// if there is no zoom factor
	if (zoomFactor <= 0 || zoomFactor == 1)
		return;

	GetPane(TMV_ACTIVEPANE)->GestureZoom(zoomFactor);

}

//==============================================================================
//
// 	Function Name:	CTMViewCtrl::ZoomedNextPage
//
// 	Description:	This external method will set the bit for zoomed 
//                  next page
//
// 	Returns:		None
//
//	Notes:			None
//
//==============================================================================
void CTMViewCtrl::SetZoomedNextPage(BOOL bZoomed)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	GetPane(TMV_ACTIVEPANE)->m_bZoomedSwipe = bZoomed;

}
