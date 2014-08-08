// Machine generated IDispatch wrapper class(es) created by Microsoft Visual C++

// NOTE: Do not modify the contents of this file.  If this class is regenerated by
//  Microsoft Visual C++, your modifications will be overwritten.


#include "stdafx.h"
#include "tmstat.h"

// Dispatch interfaces referenced by this interface
#include "Font.h"

/////////////////////////////////////////////////////////////////////////////
// CTMStat

IMPLEMENT_DYNCREATE(CTMStat, CWnd)

/////////////////////////////////////////////////////////////////////////////
// CTMStat properties

BOOL CTMStat::GetAutoInit()
{
	BOOL result;
	GetProperty(0x1, VT_BOOL, (void*)&result);
	return result;
}

void CTMStat::SetAutoInit(BOOL propVal)
{
	SetProperty(0x1, VT_BOOL, propVal);
}

short CTMStat::GetVerBuild()
{
	short result;
	GetProperty(0x2, VT_I2, (void*)&result);
	return result;
}

void CTMStat::SetVerBuild(short propVal)
{
	SetProperty(0x2, VT_I2, propVal);
}

short CTMStat::GetVerMajor()
{
	short result;
	GetProperty(0x3, VT_I2, (void*)&result);
	return result;
}

void CTMStat::SetVerMajor(short propVal)
{
	SetProperty(0x3, VT_I2, propVal);
}

short CTMStat::GetVerMinor()
{
	short result;
	GetProperty(0x4, VT_I2, (void*)&result);
	return result;
}

void CTMStat::SetVerMinor(short propVal)
{
	SetProperty(0x4, VT_I2, propVal);
}

BOOL CTMStat::GetEnableErrors()
{
	BOOL result;
	GetProperty(0x5, VT_BOOL, (void*)&result);
	return result;
}

void CTMStat::SetEnableErrors(BOOL propVal)
{
	SetProperty(0x5, VT_BOOL, propVal);
}

OLE_COLOR CTMStat::GetBackColor()
{
	OLE_COLOR result;
	GetProperty(DISPID_BACKCOLOR, VT_I4, (void*)&result);
	return result;
}

void CTMStat::SetBackColor(OLE_COLOR propVal)
{
	SetProperty(DISPID_BACKCOLOR, VT_I4, propVal);
}

COleFont CTMStat::GetFont()
{
	LPDISPATCH pDispatch;
	GetProperty(DISPID_FONT, VT_DISPATCH, (void*)&pDispatch);
	return COleFont(pDispatch);
}

void CTMStat::SetFont(LPDISPATCH propVal)
{
	SetProperty(DISPID_FONT, VT_DISPATCH, propVal);
}

OLE_COLOR CTMStat::GetForeColor()
{
	OLE_COLOR result;
	GetProperty(DISPID_FORECOLOR, VT_I4, (void*)&result);
	return result;
}

void CTMStat::SetForeColor(OLE_COLOR propVal)
{
	SetProperty(DISPID_FORECOLOR, VT_I4, propVal);
}

short CTMStat::GetAppearance()
{
	short result;
	GetProperty(DISPID_APPEARANCE, VT_I2, (void*)&result);
	return result;
}

void CTMStat::SetAppearance(short propVal)
{
	SetProperty(DISPID_APPEARANCE, VT_I2, propVal);
}

BOOL CTMStat::GetAutosizeFont()
{
	BOOL result;
	GetProperty(0x6, VT_BOOL, (void*)&result);
	return result;
}

void CTMStat::SetAutosizeFont(BOOL propVal)
{
	SetProperty(0x6, VT_BOOL, propVal);
}

CString CTMStat::GetStatusText()
{
	CString result;
	GetProperty(0x7, VT_BSTR, (void*)&result);
	return result;
}

void CTMStat::SetStatusText(LPCTSTR propVal)
{
	SetProperty(0x7, VT_BSTR, propVal);
}

short CTMStat::GetLeftMargin()
{
	short result;
	GetProperty(0x8, VT_I2, (void*)&result);
	return result;
}

void CTMStat::SetLeftMargin(short propVal)
{
	SetProperty(0x8, VT_I2, propVal);
}

short CTMStat::GetRightMargin()
{
	short result;
	GetProperty(0x9, VT_I2, (void*)&result);
	return result;
}

void CTMStat::SetRightMargin(short propVal)
{
	SetProperty(0x9, VT_I2, propVal);
}

short CTMStat::GetTopMargin()
{
	short result;
	GetProperty(0xa, VT_I2, (void*)&result);
	return result;
}

void CTMStat::SetTopMargin(short propVal)
{
	SetProperty(0xa, VT_I2, propVal);
}

short CTMStat::GetBottomMargin()
{
	short result;
	GetProperty(0xb, VT_I2, (void*)&result);
	return result;
}

void CTMStat::SetBottomMargin(short propVal)
{
	SetProperty(0xb, VT_I2, propVal);
}

CString CTMStat::GetVerTextLong()
{
	CString result;
	GetProperty(0xc, VT_BSTR, (void*)&result);
	return result;
}

void CTMStat::SetVerTextLong(LPCTSTR propVal)
{
	SetProperty(0xc, VT_BSTR, propVal);
}

double CTMStat::GetPlaylistTime()
{
	double result;
	GetProperty(0xd, VT_R8, (void*)&result);
	return result;
}

void CTMStat::SetPlaylistTime(double propVal)
{
	SetProperty(0xd, VT_R8, propVal);
}

double CTMStat::GetElapsedPlaylist()
{
	double result;
	GetProperty(0xe, VT_R8, (void*)&result);
	return result;
}

void CTMStat::SetElapsedPlaylist(double propVal)
{
	SetProperty(0xe, VT_R8, propVal);
}

double CTMStat::GetDesignationTime()
{
	double result;
	GetProperty(0xf, VT_R8, (void*)&result);
	return result;
}

void CTMStat::SetDesignationTime(double propVal)
{
	SetProperty(0xf, VT_R8, propVal);
}

double CTMStat::GetElapsedDesignation()
{
	double result;
	GetProperty(0x10, VT_R8, (void*)&result);
	return result;
}

void CTMStat::SetElapsedDesignation(double propVal)
{
	SetProperty(0x10, VT_R8, propVal);
}

short CTMStat::GetMode()
{
	short result;
	GetProperty(0x11, VT_I2, (void*)&result);
	return result;
}

void CTMStat::SetMode(short propVal)
{
	SetProperty(0x11, VT_I2, propVal);
}

long CTMStat::GetDesignationCount()
{
	long result;
	GetProperty(0x12, VT_I4, (void*)&result);
	return result;
}

void CTMStat::SetDesignationCount(long propVal)
{
	SetProperty(0x12, VT_I4, propVal);
}

long CTMStat::GetDesignationIndex()
{
	long result;
	GetProperty(0x13, VT_I4, (void*)&result);
	return result;
}

void CTMStat::SetDesignationIndex(long propVal)
{
	SetProperty(0x13, VT_I4, propVal);
}

long CTMStat::GetTextLine()
{
	long result;
	GetProperty(0x14, VT_I4, (void*)&result);
	return result;
}

void CTMStat::SetTextLine(long propVal)
{
	SetProperty(0x14, VT_I4, propVal);
}

long CTMStat::GetTextPage()
{
	long result;
	GetProperty(0x15, VT_I4, (void*)&result);
	return result;
}

void CTMStat::SetTextPage(long propVal)
{
	SetProperty(0x15, VT_I4, propVal);
}

short CTMStat::GetVerQEF()
{
	short result;
	GetProperty(0x100, VT_I2, (void*)&result);
	return result;
}

void CTMStat::SetVerQEF(short propVal)
{
	SetProperty(0x100, VT_I2, propVal);
}

CString CTMStat::GetVerTextShort()
{
	CString result;
	GetProperty(0x101, VT_BSTR, (void*)&result);
	return result;
}

void CTMStat::SetVerTextShort(LPCTSTR propVal)
{
	SetProperty(0x101, VT_BSTR, propVal);
}

CString CTMStat::GetVerBuildDate()
{
	CString result;
	GetProperty(0x102, VT_BSTR, (void*)&result);
	return result;
}

void CTMStat::SetVerBuildDate(LPCTSTR propVal)
{
	SetProperty(0x102, VT_BSTR, propVal);
}

/////////////////////////////////////////////////////////////////////////////
// CTMStat operations

short CTMStat::Initialize()
{
	short result;
	InvokeHelper(0x16, DISPATCH_METHOD, VT_I2, (void*)&result, NULL);
	return result;
}

short CTMStat::SetPlaylistInfo(long lInfo)
{
	short result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x17, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lInfo);
	return result;
}

CString CTMStat::GetClassIdString()
{
	CString result;
	InvokeHelper(0x18, DISPATCH_METHOD, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CTMStat::GetRegisteredPath()
{
	CString result;
	InvokeHelper(0x19, DISPATCH_METHOD, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CTMStat::AboutBox()
{
	InvokeHelper(0xfffffdd8, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

LONG CTMStat::GetStatusBarWidth(void)
{
	LONG result;
	InvokeHelper(0x1a, DISPATCH_METHOD, VT_I4, (void*)&result, NULL);
	return result;
}