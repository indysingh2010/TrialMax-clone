// Machine generated IDispatch wrapper class(es) created by Microsoft Visual C++

// NOTE: Do not modify the contents of this file.  If this class is regenerated by
//  Microsoft Visual C++, your modifications will be overwritten.


#include "stdafx.h"
#include "tmtext.h"

// Dispatch interfaces referenced by this interface
#include "font.h"

/////////////////////////////////////////////////////////////////////////////
// CTMText

IMPLEMENT_DYNCREATE(CTMText, CWnd)

/////////////////////////////////////////////////////////////////////////////
// CTMText properties

BOOL CTMText::GetAutoInit()
{
	BOOL result;
	GetProperty(0x1, VT_BOOL, (void*)&result);
	return result;
}

void CTMText::SetAutoInit(BOOL propVal)
{
	SetProperty(0x1, VT_BOOL, propVal);
}

short CTMText::GetVerBuild()
{
	short result;
	GetProperty(0x2, VT_I2, (void*)&result);
	return result;
}

void CTMText::SetVerBuild(short propVal)
{
	SetProperty(0x2, VT_I2, propVal);
}

OLE_COLOR CTMText::GetBackColor()
{
	OLE_COLOR result;
	GetProperty(DISPID_BACKCOLOR, VT_I4, (void*)&result);
	return result;
}

void CTMText::SetBackColor(OLE_COLOR propVal)
{
	SetProperty(DISPID_BACKCOLOR, VT_I4, propVal);
}

BOOL CTMText::GetEnableErrors()
{
	BOOL result;
	GetProperty(0x3, VT_BOOL, (void*)&result);
	return result;
}

void CTMText::SetEnableErrors(BOOL propVal)
{
	SetProperty(0x3, VT_BOOL, propVal);
}

COleFont CTMText::GetFont()
{
	LPDISPATCH pDispatch;
	GetProperty(DISPID_FONT, VT_DISPATCH, (void*)&pDispatch);
	return COleFont(pDispatch);
}

void CTMText::SetFont(LPDISPATCH propVal)
{
	SetProperty(DISPID_FONT, VT_DISPATCH, propVal);
}

OLE_COLOR CTMText::GetForeColor()
{
	OLE_COLOR result;
	GetProperty(DISPID_FORECOLOR, VT_I4, (void*)&result);
	return result;
}

void CTMText::SetForeColor(OLE_COLOR propVal)
{
	SetProperty(DISPID_FORECOLOR, VT_I4, propVal);
}

short CTMText::GetVerMajor()
{
	short result;
	GetProperty(0x4, VT_I2, (void*)&result);
	return result;
}

void CTMText::SetVerMajor(short propVal)
{
	SetProperty(0x4, VT_I2, propVal);
}

short CTMText::GetVerMinor()
{
	short result;
	GetProperty(0x5, VT_I2, (void*)&result);
	return result;
}

void CTMText::SetVerMinor(short propVal)
{
	SetProperty(0x5, VT_I2, propVal);
}

unsigned long CTMText::GetHighlightColor()
{
	unsigned long result;
	GetProperty(0x6, VT_I4, (void*)&result);
	return result;
}

void CTMText::SetHighlightColor(unsigned long propVal)
{
	SetProperty(0x6, VT_I4, propVal);
}

unsigned long CTMText::GetHighlightTextColor()
{
	unsigned long result;
	GetProperty(0x7, VT_I4, (void*)&result);
	return result;
}

void CTMText::SetHighlightTextColor(unsigned long propVal)
{
	SetProperty(0x7, VT_I4, propVal);
}

short CTMText::GetHighlightLines()
{
	short result;
	GetProperty(0x8, VT_I2, (void*)&result);
	return result;
}

void CTMText::SetHighlightLines(short propVal)
{
	SetProperty(0x8, VT_I2, propVal);
}

short CTMText::GetDisplayLines()
{
	short result;
	GetProperty(0x9, VT_I2, (void*)&result);
	return result;
}

void CTMText::SetDisplayLines(short propVal)
{
	SetProperty(0x9, VT_I2, propVal);
}

BOOL CTMText::GetCombineDesignations()
{
	BOOL result;
	GetProperty(0xa, VT_BOOL, (void*)&result);
	return result;
}

void CTMText::SetCombineDesignations(BOOL propVal)
{
	SetProperty(0xa, VT_BOOL, propVal);
}

BOOL CTMText::GetShowPageLine()
{
	BOOL result;
	GetProperty(0xb, VT_BOOL, (void*)&result);
	return result;
}

void CTMText::SetShowPageLine(BOOL propVal)
{
	SetProperty(0xb, VT_BOOL, propVal);
}

BOOL CTMText::GetUseAvgCharWidth()
{
	BOOL result;
	GetProperty(0xc, VT_BOOL, (void*)&result);
	return result;
}

void CTMText::SetUseAvgCharWidth(BOOL propVal)
{
	SetProperty(0xc, VT_BOOL, propVal);
}

BOOL CTMText::GetResizeOnChange()
{
	BOOL result;
	GetProperty(0xd, VT_BOOL, (void*)&result);
	return result;
}

void CTMText::SetResizeOnChange(BOOL propVal)
{
	SetProperty(0xd, VT_BOOL, propVal);
}

short CTMText::GetMaxCharsPerLine()
{
	short result;
	GetProperty(0xe, VT_I2, (void*)&result);
	return result;
}

void CTMText::SetMaxCharsPerLine(short propVal)
{
	SetProperty(0xe, VT_I2, propVal);
}

short CTMText::GetTopMargin()
{
	short result;
	GetProperty(0xf, VT_I2, (void*)&result);
	return result;
}

void CTMText::SetTopMargin(short propVal)
{
	SetProperty(0xf, VT_I2, propVal);
}

short CTMText::GetBottomMargin()
{
	short result;
	GetProperty(0x10, VT_I2, (void*)&result);
	return result;
}

void CTMText::SetBottomMargin(short propVal)
{
	SetProperty(0x10, VT_I2, propVal);
}

short CTMText::GetLeftMargin()
{
	short result;
	GetProperty(0x11, VT_I2, (void*)&result);
	return result;
}

void CTMText::SetLeftMargin(short propVal)
{
	SetProperty(0x11, VT_I2, propVal);
}

short CTMText::GetRightMargin()
{
	short result;
	GetProperty(0x12, VT_I2, (void*)&result);
	return result;
}

void CTMText::SetRightMargin(short propVal)
{
	SetProperty(0x12, VT_I2, propVal);
}

CString CTMText::GetVerTextLong()
{
	CString result;
	GetProperty(0x13, VT_BSTR, (void*)&result);
	return result;
}

void CTMText::SetVerTextLong(LPCTSTR propVal)
{
	SetProperty(0x13, VT_BSTR, propVal);
}

BOOL CTMText::GetSmoothScroll()
{
	BOOL result;
	GetProperty(0x14, VT_BOOL, (void*)&result);
	return result;
}

void CTMText::SetSmoothScroll(BOOL propVal)
{
	SetProperty(0x14, VT_BOOL, propVal);
}

short CTMText::GetScrollTime()
{
	short result;
	GetProperty(0x15, VT_I2, (void*)&result);
	return result;
}

void CTMText::SetScrollTime(short propVal)
{
	SetProperty(0x15, VT_I2, propVal);
}

short CTMText::GetScrollSteps()
{
	short result;
	GetProperty(0x16, VT_I2, (void*)&result);
	return result;
}

void CTMText::SetScrollSteps(short propVal)
{
	SetProperty(0x16, VT_I2, propVal);
}

BOOL CTMText::GetUseLineColor()
{
	BOOL result;
	GetProperty(0x100, VT_BOOL, (void*)&result);
	return result;
}

void CTMText::SetUseLineColor(BOOL propVal)
{
	SetProperty(0x100, VT_BOOL, propVal);
}

short CTMText::GetVerQEF()
{
	short result;
	GetProperty(0x101, VT_I2, (void*)&result);
	return result;
}

void CTMText::SetVerQEF(short propVal)
{
	SetProperty(0x101, VT_I2, propVal);
}

CString CTMText::GetVerTextShort()
{
	CString result;
	GetProperty(0x102, VT_BSTR, (void*)&result);
	return result;
}

void CTMText::SetVerTextShort(LPCTSTR propVal)
{
	SetProperty(0x102, VT_BSTR, propVal);
}

CString CTMText::GetVerBuildDate()
{
	CString result;
	GetProperty(0x103, VT_BSTR, (void*)&result);
	return result;
}

void CTMText::SetVerBuildDate(LPCTSTR propVal)
{
	SetProperty(0x103, VT_BSTR, propVal);
}

BOOL CTMText::GetShowText()
{
	BOOL result;
	GetProperty(0x104, VT_BOOL, (void*)&result);
	return result;
}

void CTMText::SetShowText(BOOL propVal)
{
	SetProperty(0x104, VT_BOOL, propVal);
}

short CTMText::GetBulletStyle()
{
	short result;
	GetProperty(0x105, VT_I2, (void*)&result);
	return result;
}

void CTMText::SetBulletStyle(short propVal)
{
	SetProperty(0x105, VT_I2, propVal);
}

short CTMText::GetBulletMargin()
{
	short result;
	GetProperty(0x106, VT_I2, (void*)&result);
	return result;
}

void CTMText::SetBulletMargin(short propVal)
{
	SetProperty(0x106, VT_I2, propVal);
}

/////////////////////////////////////////////////////////////////////////////
// CTMText operations

short CTMText::Initialize()
{
	short result;
	InvokeHelper(0x17, DISPATCH_METHOD, VT_I2, (void*)&result, NULL);
	return result;
}

short CTMText::GetMinHeight()
{
	short result;
	InvokeHelper(0x18, DISPATCH_METHOD, VT_I2, (void*)&result, NULL);
	return result;
}

long CTMText::GetCurrentLine()
{
	long result;
	InvokeHelper(0x19, DISPATCH_METHOD, VT_I4, (void*)&result, NULL);
	return result;
}

short CTMText::ResizeFont(short bRedraw)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x1a, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		bRedraw);
	return result;
}

short CTMText::IsFirstLine()
{
	short result;
	InvokeHelper(0x1b, DISPATCH_METHOD, VT_I2, (void*)&result, NULL);
	return result;
}

short CTMText::IsLastLine()
{
	short result;
	InvokeHelper(0x1c, DISPATCH_METHOD, VT_I2, (void*)&result, NULL);
	return result;
}

short CTMText::Next(short bRedraw)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x1d, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		bRedraw);
	return result;
}

short CTMText::Previous(short bRedraw)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x1e, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		bRedraw);
	return result;
}

short CTMText::SetLineObject(long lLine, short bRedraw)
{
	short result;
	static BYTE parms[] =
		VTS_I4 VTS_I2;
	InvokeHelper(0x1f, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lLine, bRedraw);
	return result;
}

short CTMText::SetMaxWidth(short sWidth, short bRedraw)
{
	short result;
	static BYTE parms[] =
		VTS_I2 VTS_I2;
	InvokeHelper(0x20, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		sWidth, bRedraw);
	return result;
}

short CTMText::SetPlaylist(long lPlaylist, short bRedraw)
{
	short result;
	static BYTE parms[] =
		VTS_I4 VTS_I2;
	InvokeHelper(0x21, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lPlaylist, bRedraw);
	return result;
}

short CTMText::GetCharHeight()
{
	short result;
	InvokeHelper(0x22, DISPATCH_METHOD, VT_I2, (void*)&result, NULL);
	return result;
}

short CTMText::GetCharWidth()
{
	short result;
	InvokeHelper(0x23, DISPATCH_METHOD, VT_I2, (void*)&result, NULL);
	return result;
}

short CTMText::GetLogFont(long lLogFont)
{
	short result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x24, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lLogFont);
	return result;
}

BOOL CTMText::IsReady()
{
	BOOL result;
	InvokeHelper(0x25, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

short CTMText::SetLine(long lDesignation, long lPageNum, long lLineNum, short bRedraw)
{
	short result;
	static BYTE parms[] =
		VTS_I4 VTS_I4 VTS_I4 VTS_I2;
	InvokeHelper(0x26, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lDesignation, lPageNum, lLineNum, bRedraw);
	return result;
}

CString CTMText::GetRegisteredPath()
{
	CString result;
	InvokeHelper(0x27, DISPATCH_METHOD, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CTMText::GetClassIdString()
{
	CString result;
	InvokeHelper(0x28, DISPATCH_METHOD, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CTMText::AboutBox()
{
	InvokeHelper(0xfffffdd8, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}
