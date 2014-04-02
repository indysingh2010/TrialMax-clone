// Machine generated IDispatch wrapper class(es) created by Microsoft Visual C++

// NOTE: Do not modify the contents of this file.  If this class is regenerated by
//  Microsoft Visual C++, your modifications will be overwritten.


#include "stdafx.h"
#include "tmview.h"

/////////////////////////////////////////////////////////////////////////////
// CTm_view

IMPLEMENT_DYNCREATE(CTm_view, CWnd)

/////////////////////////////////////////////////////////////////////////////
// CTm_view properties

OLE_COLOR CTm_view::GetBackColor()
{
	OLE_COLOR result;
	GetProperty(DISPID_BACKCOLOR, VT_I4, (void*)&result);
	return result;
}

void CTm_view::SetBackColor(OLE_COLOR propVal)
{
	SetProperty(DISPID_BACKCOLOR, VT_I4, propVal);
}

short CTm_view::GetBorderStyle()
{
	short result;
	GetProperty(DISPID_BORDERSTYLE, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetBorderStyle(short propVal)
{
	SetProperty(DISPID_BORDERSTYLE, VT_I2, propVal);
}

BOOL CTm_view::GetEnabled()
{
	BOOL result;
	GetProperty(DISPID_ENABLED, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetEnabled(BOOL propVal)
{
	SetProperty(DISPID_ENABLED, VT_BOOL, propVal);
}

short CTm_view::GetRedactColor()
{
	short result;
	GetProperty(0x1, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetRedactColor(short propVal)
{
	SetProperty(0x1, VT_I2, propVal);
}

short CTm_view::GetHighlightColor()
{
	short result;
	GetProperty(0x2, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetHighlightColor(short propVal)
{
	SetProperty(0x2, VT_I2, propVal);
}

short CTm_view::GetAnnColor()
{
	short result;
	GetProperty(0x3, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetAnnColor(short propVal)
{
	SetProperty(0x3, VT_I2, propVal);
}

short CTm_view::GetAnnTool()
{
	short result;
	GetProperty(0x4, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetAnnTool(short propVal)
{
	SetProperty(0x4, VT_I2, propVal);
}

short CTm_view::GetAnnThickness()
{
	short result;
	GetProperty(0x5, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetAnnThickness(short propVal)
{
	SetProperty(0x5, VT_I2, propVal);
}

BOOL CTm_view::GetScaleImage()
{
	BOOL result;
	GetProperty(0x6, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetScaleImage(BOOL propVal)
{
	SetProperty(0x6, VT_BOOL, propVal);
}

short CTm_view::GetRotation()
{
	short result;
	GetProperty(0x7, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetRotation(short propVal)
{
	SetProperty(0x7, VT_I2, propVal);
}

short CTm_view::GetMaxZoom()
{
	short result;
	GetProperty(0x8, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetMaxZoom(short propVal)
{
	SetProperty(0x8, VT_I2, propVal);
}

BOOL CTm_view::GetFitToImage()
{
	BOOL result;
	GetProperty(0x9, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetFitToImage(BOOL propVal)
{
	SetProperty(0x9, VT_BOOL, propVal);
}

short CTm_view::GetAction()
{
	short result;
	GetProperty(0xa, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetAction(short propVal)
{
	SetProperty(0xa, VT_I2, propVal);
}

BOOL CTm_view::GetEnableErrors()
{
	BOOL result;
	GetProperty(0xb, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetEnableErrors(BOOL propVal)
{
	SetProperty(0xb, VT_BOOL, propVal);
}

CString CTm_view::GetBackgroundFile()
{
	CString result;
	GetProperty(0xc, VT_BSTR, (void*)&result);
	return result;
}

void CTm_view::SetBackgroundFile(LPCTSTR propVal)
{
	SetProperty(0xc, VT_BSTR, propVal);
}

BOOL CTm_view::GetAutoAnimate()
{
	BOOL result;
	GetProperty(0xd, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetAutoAnimate(BOOL propVal)
{
	SetProperty(0xd, VT_BOOL, propVal);
}

BOOL CTm_view::GetLoopAnimation()
{
	BOOL result;
	GetProperty(0xe, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetLoopAnimation(BOOL propVal)
{
	SetProperty(0xe, VT_BOOL, propVal);
}

short CTm_view::GetVerBuild()
{
	short result;
	GetProperty(0x35, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetVerBuild(short propVal)
{
	SetProperty(0x35, VT_I2, propVal);
}

BOOL CTm_view::GetHideScrollBars()
{
	BOOL result;
	GetProperty(0xf, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetHideScrollBars(BOOL propVal)
{
	SetProperty(0xf, VT_BOOL, propVal);
}

short CTm_view::GetPanPercent()
{
	short result;
	GetProperty(0x10, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetPanPercent(short propVal)
{
	SetProperty(0x10, VT_I2, propVal);
}

short CTm_view::GetZoomOnLoad()
{
	short result;
	GetProperty(0x11, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetZoomOnLoad(short propVal)
{
	SetProperty(0x11, VT_I2, propVal);
}

short CTm_view::GetCalloutColor()
{
	short result;
	GetProperty(0x12, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetCalloutColor(short propVal)
{
	SetProperty(0x12, VT_I2, propVal);
}

short CTm_view::GetCalloutFrameThickness()
{
	short result;
	GetProperty(0x13, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetCalloutFrameThickness(short propVal)
{
	SetProperty(0x13, VT_I2, propVal);
}

short CTm_view::GetCalloutFrameColor()
{
	short result;
	GetProperty(0x14, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetCalloutFrameColor(short propVal)
{
	SetProperty(0x14, VT_I2, propVal);
}

BOOL CTm_view::GetRightClickPan()
{
	BOOL result;
	GetProperty(0x15, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetRightClickPan(BOOL propVal)
{
	SetProperty(0x15, VT_BOOL, propVal);
}

short CTm_view::GetSplitFrameThickness()
{
	short result;
	GetProperty(0x16, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetSplitFrameThickness(short propVal)
{
	SetProperty(0x16, VT_I2, propVal);
}

short CTm_view::GetSplitFrameColor()
{
	short result;
	GetProperty(0x17, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetSplitFrameColor(short propVal)
{
	SetProperty(0x17, VT_I2, propVal);
}

BOOL CTm_view::GetSplitScreen()
{
	BOOL result;
	GetProperty(0x18, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetSplitScreen(BOOL propVal)
{
	SetProperty(0x18, VT_BOOL, propVal);
}

short CTm_view::GetActivePane()
{
	short result;
	GetProperty(0x19, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetActivePane(short propVal)
{
	SetProperty(0x19, VT_I2, propVal);
}

BOOL CTm_view::GetSyncPanes()
{
	BOOL result;
	GetProperty(0x1a, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetSyncPanes(BOOL propVal)
{
	SetProperty(0x1a, VT_BOOL, propVal);
}

CString CTm_view::GetLeftFile()
{
	CString result;
	GetProperty(0x1b, VT_BSTR, (void*)&result);
	return result;
}

void CTm_view::SetLeftFile(LPCTSTR propVal)
{
	SetProperty(0x1b, VT_BSTR, propVal);
}

CString CTm_view::GetRightFile()
{
	CString result;
	GetProperty(0x1c, VT_BSTR, (void*)&result);
	return result;
}

void CTm_view::SetRightFile(LPCTSTR propVal)
{
	SetProperty(0x1c, VT_BSTR, propVal);
}

BOOL CTm_view::GetSyncCalloutAnn()
{
	BOOL result;
	GetProperty(0x1d, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetSyncCalloutAnn(BOOL propVal)
{
	SetProperty(0x1d, VT_BOOL, propVal);
}

BOOL CTm_view::GetPenSelectorVisible()
{
	BOOL result;
	GetProperty(0x1e, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetPenSelectorVisible(BOOL propVal)
{
	SetProperty(0x1e, VT_BOOL, propVal);
}

short CTm_view::GetPenSelectorColor()
{
	short result;
	GetProperty(0x1f, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetPenSelectorColor(short propVal)
{
	SetProperty(0x1f, VT_I2, propVal);
}

short CTm_view::GetPenSelectorSize()
{
	short result;
	GetProperty(0x20, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetPenSelectorSize(short propVal)
{
	SetProperty(0x20, VT_I2, propVal);
}

BOOL CTm_view::GetKeepAspect()
{
	BOOL result;
	GetProperty(0x21, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetKeepAspect(BOOL propVal)
{
	SetProperty(0x21, VT_BOOL, propVal);
}

short CTm_view::GetBitonalScaling()
{
	short result;
	GetProperty(0x22, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetBitonalScaling(short propVal)
{
	SetProperty(0x22, VT_I2, propVal);
}

BOOL CTm_view::GetZoomToRect()
{
	BOOL result;
	GetProperty(0x23, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetZoomToRect(BOOL propVal)
{
	SetProperty(0x23, VT_BOOL, propVal);
}

CString CTm_view::GetVerTextLong()
{
	CString result;
	GetProperty(0x36, VT_BSTR, (void*)&result);
	return result;
}

void CTm_view::SetVerTextLong(LPCTSTR propVal)
{
	SetProperty(0x36, VT_BSTR, propVal);
}

CString CTm_view::GetAnnFontName()
{
	CString result;
	GetProperty(0x24, VT_BSTR, (void*)&result);
	return result;
}

void CTm_view::SetAnnFontName(LPCTSTR propVal)
{
	SetProperty(0x24, VT_BSTR, propVal);
}

short CTm_view::GetAnnFontSize()
{
	short result;
	GetProperty(0x25, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetAnnFontSize(short propVal)
{
	SetProperty(0x25, VT_I2, propVal);
}

BOOL CTm_view::GetAnnFontBold()
{
	BOOL result;
	GetProperty(0x26, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetAnnFontBold(BOOL propVal)
{
	SetProperty(0x26, VT_BOOL, propVal);
}

BOOL CTm_view::GetAnnFontStrikeThrough()
{
	BOOL result;
	GetProperty(0x27, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetAnnFontStrikeThrough(BOOL propVal)
{
	SetProperty(0x27, VT_BOOL, propVal);
}

BOOL CTm_view::GetAnnFontUnderline()
{
	BOOL result;
	GetProperty(0x28, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetAnnFontUnderline(BOOL propVal)
{
	SetProperty(0x28, VT_BOOL, propVal);
}

unsigned long CTm_view::GetDeskewBackColor()
{
	unsigned long result;
	GetProperty(0x29, VT_I4, (void*)&result);
	return result;
}

void CTm_view::SetDeskewBackColor(unsigned long propVal)
{
	SetProperty(0x29, VT_I4, propVal);
}

BOOL CTm_view::GetAnnotateCallouts()
{
	BOOL result;
	GetProperty(0x2a, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetAnnotateCallouts(BOOL propVal)
{
	SetProperty(0x2a, VT_BOOL, propVal);
}

short CTm_view::GetPrintOrientation()
{
	short result;
	GetProperty(0x2b, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetPrintOrientation(short propVal)
{
	SetProperty(0x2b, VT_I2, propVal);
}

unsigned long CTm_view::GetPrintBorderColor()
{
	unsigned long result;
	GetProperty(0x2c, VT_I4, (void*)&result);
	return result;
}

void CTm_view::SetPrintBorderColor(unsigned long propVal)
{
	SetProperty(0x2c, VT_I4, propVal);
}

float CTm_view::GetPrintLeftMargin()
{
	float result;
	GetProperty(0x2d, VT_R4, (void*)&result);
	return result;
}

void CTm_view::SetPrintLeftMargin(float propVal)
{
	SetProperty(0x2d, VT_R4, propVal);
}

float CTm_view::GetPrintRightMargin()
{
	float result;
	GetProperty(0x2e, VT_R4, (void*)&result);
	return result;
}

void CTm_view::SetPrintRightMargin(float propVal)
{
	SetProperty(0x2e, VT_R4, propVal);
}

float CTm_view::GetPrintTopMargin()
{
	float result;
	GetProperty(0x2f, VT_R4, (void*)&result);
	return result;
}

void CTm_view::SetPrintTopMargin(float propVal)
{
	SetProperty(0x2f, VT_R4, propVal);
}

float CTm_view::GetPrintBottomMargin()
{
	float result;
	GetProperty(0x30, VT_R4, (void*)&result);
	return result;
}

void CTm_view::SetPrintBottomMargin(float propVal)
{
	SetProperty(0x30, VT_R4, propVal);
}

float CTm_view::GetPrintBorderThickness()
{
	float result;
	GetProperty(0x31, VT_R4, (void*)&result);
	return result;
}

void CTm_view::SetPrintBorderThickness(float propVal)
{
	SetProperty(0x31, VT_R4, propVal);
}

BOOL CTm_view::GetPrintCallouts()
{
	BOOL result;
	GetProperty(0x32, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetPrintCallouts(BOOL propVal)
{
	SetProperty(0x32, VT_BOOL, propVal);
}

BOOL CTm_view::GetPrintBorder()
{
	BOOL result;
	GetProperty(0x33, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetPrintBorder(BOOL propVal)
{
	SetProperty(0x33, VT_BOOL, propVal);
}

BOOL CTm_view::GetPrintCalloutBorders()
{
	BOOL result;
	GetProperty(0x34, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetPrintCalloutBorders(BOOL propVal)
{
	SetProperty(0x34, VT_BOOL, propVal);
}

short CTm_view::GetAnnColorDepth()
{
	short result;
	GetProperty(0xfffffe02, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetAnnColorDepth(short propVal)
{
	SetProperty(0xfffffe02, VT_I2, propVal);
}

BOOL CTm_view::GetSplitHorizontal()
{
	BOOL result;
	GetProperty(0xfffffe01, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetSplitHorizontal(BOOL propVal)
{
	SetProperty(0xfffffe01, VT_BOOL, propVal);
}

short CTm_view::GetQFactor()
{
	short result;
	GetProperty(0x100, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetQFactor(short propVal)
{
	SetProperty(0x100, VT_I2, propVal);
}

BOOL CTm_view::GetResizeCallouts()
{
	BOOL result;
	GetProperty(0x101, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetResizeCallouts(BOOL propVal)
{
	SetProperty(0x101, VT_BOOL, propVal);
}

unsigned long CTm_view::GetCalloutHandleColor()
{
	unsigned long result;
	GetProperty(0x102, VT_I4, (void*)&result);
	return result;
}

void CTm_view::SetCalloutHandleColor(unsigned long propVal)
{
	SetProperty(0x102, VT_I4, propVal);
}

BOOL CTm_view::GetShadeOnCallout()
{
	BOOL result;
	GetProperty(0x103, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetShadeOnCallout(BOOL propVal)
{
	SetProperty(0x103, VT_BOOL, propVal);
}

short CTm_view::GetCalloutShadeGrayscale()
{
	short result;
	GetProperty(0x104, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetCalloutShadeGrayscale(short propVal)
{
	SetProperty(0x104, VT_I2, propVal);
}

BOOL CTm_view::GetPanCallouts()
{
	BOOL result;
	GetProperty(0x105, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetPanCallouts(BOOL propVal)
{
	SetProperty(0x105, VT_BOOL, propVal);
}

BOOL CTm_view::GetZoomCallouts()
{
	BOOL result;
	GetProperty(0x106, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetZoomCallouts(BOOL propVal)
{
	SetProperty(0x106, VT_BOOL, propVal);
}

BOOL CTm_view::GetLoadAsynch()
{
	BOOL result;
	GetProperty(0x107, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetLoadAsynch(BOOL propVal)
{
	SetProperty(0x107, VT_BOOL, propVal);
}

BOOL CTm_view::GetEnableAxErrors()
{
	BOOL result;
	GetProperty(0x108, VT_BOOL, (void*)&result);
	return result;
}

void CTm_view::SetEnableAxErrors(BOOL propVal)
{
	SetProperty(0x108, VT_BOOL, propVal);
}

short CTm_view::GetVerMajor()
{
	short result;
	GetProperty(0x109, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetVerMajor(short propVal)
{
	SetProperty(0x109, VT_I2, propVal);
}

short CTm_view::GetVerMinor()
{
	short result;
	GetProperty(0x10a, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetVerMinor(short propVal)
{
	SetProperty(0x10a, VT_I2, propVal);
}

short CTm_view::GetVerQEF()
{
	short result;
	GetProperty(0x10b, VT_I2, (void*)&result);
	return result;
}

void CTm_view::SetVerQEF(short propVal)
{
	SetProperty(0x10b, VT_I2, propVal);
}

CString CTm_view::GetVerTextShort()
{
	CString result;
	GetProperty(0x10c, VT_BSTR, (void*)&result);
	return result;
}

void CTm_view::SetVerTextShort(LPCTSTR propVal)
{
	SetProperty(0x10c, VT_BSTR, propVal);
}

CString CTm_view::GetVerBuildDate()
{
	CString result;
	GetProperty(0x10d, VT_BSTR, (void*)&result);
	return result;
}

void CTm_view::SetVerBuildDate(LPCTSTR propVal)
{
	SetProperty(0x10d, VT_BSTR, propVal);
}

/////////////////////////////////////////////////////////////////////////////
// CTm_view operations

void CTm_view::Redraw()
{
	InvokeHelper(0x37, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

short CTm_view::SetAnnotationProperties(long lFlags)
{
	short result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x38, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lFlags);
	return result;
}

void CTm_view::SetPrinter()
{
	InvokeHelper(0x39, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

short CTm_view::LoadFile(LPCTSTR lpszFilename, short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_BSTR VTS_I2;
	InvokeHelper(0x3a, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lpszFilename, sPane);
	return result;
}

short CTm_view::Print(BOOL bFullImage, short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_BOOL VTS_I2;
	InvokeHelper(0x3b, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		bFullImage, sPane);
	return result;
}

short CTm_view::DestroyCallouts(short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x3c, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane);
	return result;
}

short CTm_view::Erase(short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x3d, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane);
	return result;
}

float CTm_view::GetAspectRatio(short sPane)
{
	float result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x3e, DISPATCH_METHOD, VT_R4, (void*)&result, parms,
		sPane);
	return result;
}

short CTm_view::GetCurrentPage(short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x3f, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane);
	return result;
}

float CTm_view::GetImageHeight(short sPane)
{
	float result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x40, DISPATCH_METHOD, VT_R4, (void*)&result, parms,
		sPane);
	return result;
}

float CTm_view::GetImageWidth(short sPane)
{
	float result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x41, DISPATCH_METHOD, VT_R4, (void*)&result, parms,
		sPane);
	return result;
}

short CTm_view::GetPageCount(short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x42, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane);
	return result;
}

short CTm_view::GetPanStates(short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x43, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane);
	return result;
}

float CTm_view::GetSrcRatio(short sPane)
{
	float result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x44, DISPATCH_METHOD, VT_R4, (void*)&result, parms,
		sPane);
	return result;
}

float CTm_view::GetZoomFactor(short sPane)
{
	float result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x45, DISPATCH_METHOD, VT_R4, (void*)&result, parms,
		sPane);
	return result;
}

short CTm_view::GetZoomState(short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x46, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane);
	return result;
}

BOOL CTm_view::IsAnimation(short sPane)
{
	BOOL result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x47, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		sPane);
	return result;
}

BOOL CTm_view::IsLoaded(short sPane)
{
	BOOL result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x48, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		sPane);
	return result;
}

BOOL CTm_view::IsPlaying(short sPane)
{
	BOOL result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x49, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		sPane);
	return result;
}

short CTm_view::NextPage(short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x4a, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane);
	return result;
}

short CTm_view::PlayAnimation(BOOL bPlay, BOOL bContinuous, short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_BOOL VTS_BOOL VTS_I2;
	InvokeHelper(0x4b, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		bPlay, bContinuous, sPane);
	return result;
}

short CTm_view::PrevPage(short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x4c, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane);
	return result;
}

short CTm_view::Realize(short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x4d, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane);
	return result;
}

BOOL CTm_view::Pan(short sDirection, short sPane)
{
	BOOL result;
	static BYTE parms[] =
		VTS_I2 VTS_I2;
	InvokeHelper(0x4e, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		sDirection, sPane);
	return result;
}

short CTm_view::Render(long hDc, float fLeft, float fTop, float fWidth, float fHeight, short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I4 VTS_R4 VTS_R4 VTS_R4 VTS_R4 VTS_I2;
	InvokeHelper(0x4f, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		hDc, fLeft, fTop, fWidth, fHeight, sPane);
	return result;
}

void CTm_view::ResetZoom(short sPane)
{
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x50, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 sPane);
}

void CTm_view::ResizeSourceToImage(short sPane)
{
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x51, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 sPane);
}

void CTm_view::ResizeSourceToView(short sPane)
{
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x52, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 sPane);
}

void CTm_view::Rotate(BOOL bRedraw, short sPane)
{
	static BYTE parms[] =
		VTS_BOOL VTS_I2;
	InvokeHelper(0x53, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 bRedraw, sPane);
}

void CTm_view::RotateCcw(BOOL bRedraw, short sPane)
{
	static BYTE parms[] =
		VTS_BOOL VTS_I2;
	InvokeHelper(0x54, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 bRedraw, sPane);
}

void CTm_view::RotateCw(BOOL bRedraw, short sPane)
{
	static BYTE parms[] =
		VTS_BOOL VTS_I2;
	InvokeHelper(0x55, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 bRedraw, sPane);
}

short CTm_view::Save(LPCTSTR lpszFilename, short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_BSTR VTS_I2;
	InvokeHelper(0x56, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lpszFilename, sPane);
	return result;
}

short CTm_view::SaveZap(LPCTSTR lpszFilename, short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_BSTR VTS_I2;
	InvokeHelper(0x57, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lpszFilename, sPane);
	return result;
}

short CTm_view::ShowCallouts(BOOL bShow, short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_BOOL VTS_I2;
	InvokeHelper(0x58, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		bShow, sPane);
	return result;
}

void CTm_view::ZoomFullHeight(short sPane)
{
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x59, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 sPane);
}

void CTm_view::ZoomFullWidth(short sPane)
{
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x5a, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 sPane);
}

short CTm_view::FirstPage(short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x5b, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane);
	return result;
}

short CTm_view::LastPage(short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x5c, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane);
	return result;
}

short CTm_view::GetColor()
{
	short result;
	InvokeHelper(0x5d, DISPATCH_METHOD, VT_I2, (void*)&result, NULL);
	return result;
}

void CTm_view::SetColor(short sColor)
{
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x5e, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 sColor);
}

void CTm_view::SetData(short sPane, long lData)
{
	static BYTE parms[] =
		VTS_I2 VTS_I4;
	InvokeHelper(0x5f, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 sPane, lData);
}

long CTm_view::GetData(short sPane)
{
	long result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x60, DISPATCH_METHOD, VT_I4, (void*)&result, parms,
		sPane);
	return result;
}

short CTm_view::GetSelectCount(short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x61, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane);
	return result;
}

short CTm_view::DeleteSelections(short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x62, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane);
	return result;
}

short CTm_view::DeleteLastAnn(short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x63, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane);
	return result;
}

short CTm_view::LoadZap(LPCTSTR lpszFilename, BOOL bUseView, BOOL bScaleView, BOOL bCallouts, short sPane, LPCTSTR lpszSourceFile)
{
	short result;
	static BYTE parms[] =
		VTS_BSTR VTS_BOOL VTS_BOOL VTS_BOOL VTS_I2 VTS_BSTR;
	InvokeHelper(0x64, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lpszFilename, bUseView, bScaleView, bCallouts, sPane, lpszSourceFile);
	return result;
}

short CTm_view::Copy(short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x65, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane);
	return result;
}

short CTm_view::Paste(short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x66, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane);
	return result;
}

long CTm_view::GetRGBColor(short sColor)
{
	long result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x67, DISPATCH_METHOD, VT_I4, (void*)&result, parms,
		sColor);
	return result;
}

short CTm_view::ViewImageProperties(short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x68, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane);
	return result;
}

BOOL CTm_view::SetPrinterByName(LPCTSTR lpName)
{
	BOOL result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x69, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		lpName);
	return result;
}

CString CTm_view::GetDefaultPrinter()
{
	CString result;
	InvokeHelper(0x6a, DISPATCH_METHOD, VT_BSTR, (void*)&result, NULL);
	return result;
}

BOOL CTm_view::CanPrint()
{
	BOOL result;
	InvokeHelper(0x6b, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

CString CTm_view::GetCurrentPrinter()
{
	CString result;
	InvokeHelper(0x6c, DISPATCH_METHOD, VT_BSTR, (void*)&result, NULL);
	return result;
}

short CTm_view::GetImageProperties(long lpProperties, short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I4 VTS_I2;
	InvokeHelper(0x6d, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lpProperties, sPane);
	return result;
}

short CTm_view::Deskew(short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x6e, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane);
	return result;
}

CString CTm_view::GetRegisteredPath()
{
	CString result;
	InvokeHelper(0x6f, DISPATCH_METHOD, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CTm_view::GetClassIdString()
{
	CString result;
	InvokeHelper(0x70, DISPATCH_METHOD, VT_BSTR, (void*)&result, NULL);
	return result;
}

short CTm_view::Despeckle(short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x71, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane);
	return result;
}

short CTm_view::GetLeadToolError(short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x72, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane);
	return result;
}

short CTm_view::DotRemove(short sPane, long lMinWidth, long lMinHeight, long lMaxWidth, long lMaxHeight)
{
	short result;
	static BYTE parms[] =
		VTS_I2 VTS_I4 VTS_I4 VTS_I4 VTS_I4;
	InvokeHelper(0x73, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane, lMinWidth, lMinHeight, lMaxWidth, lMaxHeight);
	return result;
}

short CTm_view::HolePunchRemove(short sPane, long lMinWidth, long lMinHeight, long lMaxWidth, long lMaxHeight, long lLocation)
{
	short result;
	static BYTE parms[] =
		VTS_I2 VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4;
	InvokeHelper(0x74, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane, lMinWidth, lMinHeight, lMaxWidth, lMaxHeight, lLocation);
	return result;
}

short CTm_view::Smooth(short sPane, long lLength, short sFavorLong)
{
	short result;
	static BYTE parms[] =
		VTS_I2 VTS_I4 VTS_I2;
	InvokeHelper(0x75, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane, lLength, sFavorLong);
	return result;
}

short CTm_view::BorderRemove(short sPane, long lBorderPercent, long lWhiteNoise, long lVariance, long lLocation)
{
	short result;
	static BYTE parms[] =
		VTS_I2 VTS_I4 VTS_I4 VTS_I4 VTS_I4;
	InvokeHelper(0x76, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane, lBorderPercent, lWhiteNoise, lVariance, lLocation);
	return result;
}

short CTm_view::Cleanup(short sPane, LPCTSTR lpszSaveAs)
{
	short result;
	static BYTE parms[] =
		VTS_I2 VTS_BSTR;
	InvokeHelper(0x77, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane, lpszSaveAs);
	return result;
}

short CTm_view::SetupPrintPage()
{
	short result;
	InvokeHelper(0x78, DISPATCH_METHOD, VT_I2, (void*)&result, NULL);
	return result;
}

short CTm_view::PrintEx(long hDc, short bFullImage, short bAutoRotate, short sLeft, short sTop, short sWidth, short sHeight, short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I4 VTS_I2 VTS_I2 VTS_I2 VTS_I2 VTS_I2 VTS_I2 VTS_I2;
	InvokeHelper(0x79, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		hDc, bFullImage, bAutoRotate, sLeft, sTop, sWidth, sHeight, sPane);
	return result;
}

short CTm_view::RescaleZapCallouts()
{
	short result;
	InvokeHelper(0x7a, DISPATCH_METHOD, VT_I2, (void*)&result, NULL);
	return result;
}

short CTm_view::ShowDiagnostics(BOOL bShow)
{
	short result;
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x7b, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		bShow);
	return result;
}

long CTm_view::DrawSourceRectangle(short sLeft, short sTop, short sRight, short sBottom, unsigned long lColor, short sTransparency, BOOL bLocked, short sPane)
{
	long result;
	static BYTE parms[] =
		VTS_I2 VTS_I2 VTS_I2 VTS_I2 VTS_I4 VTS_I2 VTS_BOOL VTS_I2;
	InvokeHelper(0x7c, DISPATCH_METHOD, VT_I4, (void*)&result, parms,
		sLeft, sTop, sRight, sBottom, lColor, sTransparency, bLocked, sPane);
	return result;
}

short CTm_view::DeleteAnn(long lAnnotation, short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I4 VTS_I2;
	InvokeHelper(0x7d, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lAnnotation, sPane);
	return result;
}

long CTm_view::DrawText(LPCTSTR lpszText, short sLeft, short sTop, short sRight, short sBottom, unsigned long lColor, LPCTSTR lpszFont, short sSize, BOOL bLocked, short sPane)
{
	long result;
	static BYTE parms[] =
		VTS_BSTR VTS_I2 VTS_I2 VTS_I2 VTS_I2 VTS_I4 VTS_BSTR VTS_I2 VTS_BOOL VTS_I2;
	InvokeHelper(0x7e, DISPATCH_METHOD, VT_I4, (void*)&result, parms,
		lpszText, sLeft, sTop, sRight, sBottom, lColor, lpszFont, sSize, bLocked, sPane);
	return result;
}

long CTm_view::DrawSourceText(LPCTSTR lpszText, short sLeft, short sTop, short sRight, short sBottom, unsigned long lColor, LPCTSTR lpszFont, short sSize, BOOL bLocked, short sPane)
{
	long result;
	static BYTE parms[] =
		VTS_BSTR VTS_I2 VTS_I2 VTS_I2 VTS_I2 VTS_I4 VTS_BSTR VTS_I2 VTS_BOOL VTS_I2;
	InvokeHelper(0x7f, DISPATCH_METHOD, VT_I4, (void*)&result, parms,
		lpszText, sLeft, sTop, sRight, sBottom, lColor, lpszFont, sSize, bLocked, sPane);
	return result;
}

short CTm_view::DrawTmaxRedaction(long pRedaction, short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I4 VTS_I2;
	InvokeHelper(0x80, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		pRedaction, sPane);
	return result;
}

short CTm_view::DrawTmaxRedactions(long paRedactions, short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I4 VTS_I2;
	InvokeHelper(0x81, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		paRedactions, sPane);
	return result;
}

short CTm_view::DeleteTmaxRedaction(long pRedaction, short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I4 VTS_I2;
	InvokeHelper(0x82, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		pRedaction, sPane);
	return result;
}

short CTm_view::DeleteTmaxRedactions(long paRedactions, short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I4 VTS_I2;
	InvokeHelper(0x83, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		paRedactions, sPane);
	return result;
}

long CTm_view::DrawRectangle(short sLeft, short sTop, short sRight, short sBottom, unsigned long lColor, short sTransparency, BOOL bLocked, short sPane)
{
	long result;
	static BYTE parms[] =
		VTS_I2 VTS_I2 VTS_I2 VTS_I2 VTS_I4 VTS_I2 VTS_BOOL VTS_I2;
	InvokeHelper(0x84, DISPATCH_METHOD, VT_I4, (void*)&result, parms,
		sLeft, sTop, sRight, sBottom, lColor, sTransparency, bLocked, sPane);
	return result;
}

short CTm_view::LockAnn(long lAnnotation, short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I4 VTS_I2;
	InvokeHelper(0x85, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lAnnotation, sPane);
	return result;
}

short CTm_view::UnlockAnn(long lAnnotation, short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I4 VTS_I2;
	InvokeHelper(0x86, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lAnnotation, sPane);
	return result;
}

unsigned long CTm_view::GetOleColor(short sTmviewColor)
{
	unsigned long result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x87, DISPATCH_METHOD, VT_I4, (void*)&result, parms,
		sTmviewColor);
	return result;
}

short CTm_view::GetCalloutCount(short sPane)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x88, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane);
	return result;
}

BOOL CTm_view::SetPrinterProperties(long hWnd)
{
	BOOL result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x89, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		hWnd);
	return result;
}

short CTm_view::SetProperties(LPCTSTR lpszFilename, LPCTSTR lpszSection)
{
	short result;
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR;
	InvokeHelper(0x8a, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lpszFilename, lpszSection);
	return result;
}

short CTm_view::SaveProperties(LPCTSTR lpszFilename, LPCTSTR lpszSection)
{
	short result;
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR;
	InvokeHelper(0x8b, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lpszFilename, lpszSection);
	return result;
}

short CTm_view::HolePunchRemove2(short sPane, long lMinHolePunches, long lMaxHolePunches, long lLocation)
{
	short result;
	static BYTE parms[] =
		VTS_I2 VTS_I4 VTS_I4 VTS_I4;
	InvokeHelper(0x8c, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sPane, lMinHolePunches, lMaxHolePunches, lLocation);
	return result;
}

short CTm_view::SavePages(LPCTSTR lpszFilename, LPCTSTR lpszFolder, LPCTSTR lpszPrefix)
{
	short result;
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR VTS_BSTR;
	InvokeHelper(0x8d, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lpszFilename, lpszFolder, lpszPrefix);
	return result;
}

short CTm_view::ShowPrinterCaps()
{
	short result;
	InvokeHelper(0x8e, DISPATCH_METHOD, VT_I2, (void*)&result, NULL);
	return result;
}

void CTm_view::EnableDIBPrinting(short bEnable)
{
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x8f, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 bEnable);
}

short CTm_view::SwapPanes()
{
	short result;
	InvokeHelper(0x90, DISPATCH_METHOD, VT_I2, (void*)&result, NULL);
	return result;
}

short CTm_view::SaveSplitZap(LPCTSTR lpszTLFilename, LPCTSTR lpszBRFilename)
{
	short result;
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR;
	InvokeHelper(0x91, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lpszTLFilename, lpszBRFilename);
	return result;
}

void CTm_view::AboutBox()
{
	InvokeHelper(0xfffffdd8, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void CTm_view::DoGesturePan(long lX, long lY)
{
	static BYTE parms[] = VTS_I4 VTS_I4 ;
	InvokeHelper(0x92, DISPATCH_METHOD, VT_EMPTY, NULL, parms, lX, lY);
}

void CTm_view::DoGestureZoom(float zoomFactor)
{
	static BYTE parms[] = VTS_R4 ;
	InvokeHelper(0x93, DISPATCH_METHOD, VT_EMPTY, NULL, parms, zoomFactor);
}

void CTm_view::SetZoomedNextPage(bool bZoomed)
{
	static BYTE parms[] = VTS_BOOL;
	InvokeHelper(0x94, DISPATCH_METHOD, VT_EMPTY, NULL, parms, bZoomed);
}
