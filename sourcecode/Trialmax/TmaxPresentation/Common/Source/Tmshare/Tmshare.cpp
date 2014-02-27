// Machine generated IDispatch wrapper class(es) created by Microsoft Visual C++

// NOTE: Do not modify the contents of this file.  If this class is regenerated by
//  Microsoft Visual C++, your modifications will be overwritten.


#include "stdafx.h"
#include "tmshare.h"

/////////////////////////////////////////////////////////////////////////////
// CTMShare

IMPLEMENT_DYNCREATE(CTMShare, CWnd)

/////////////////////////////////////////////////////////////////////////////
// CTMShare properties

short CTMShare::GetVerBuild()
{
	short result;
	GetProperty(0x1, VT_I2, (void*)&result);
	return result;
}

void CTMShare::SetVerBuild(short propVal)
{
	SetProperty(0x1, VT_I2, propVal);
}

BOOL CTMShare::GetEnableErrors()
{
	BOOL result;
	GetProperty(0x2, VT_BOOL, (void*)&result);
	return result;
}

void CTMShare::SetEnableErrors(BOOL propVal)
{
	SetProperty(0x2, VT_BOOL, propVal);
}

short CTMShare::GetVerMajor()
{
	short result;
	GetProperty(0x3, VT_I2, (void*)&result);
	return result;
}

void CTMShare::SetVerMajor(short propVal)
{
	SetProperty(0x3, VT_I2, propVal);
}

short CTMShare::GetVerMinor()
{
	short result;
	GetProperty(0x4, VT_I2, (void*)&result);
	return result;
}

void CTMShare::SetVerMinor(short propVal)
{
	SetProperty(0x4, VT_I2, propVal);
}

CString CTMShare::GetVerTextLong()
{
	CString result;
	GetProperty(0x5, VT_BSTR, (void*)&result);
	return result;
}

void CTMShare::SetVerTextLong(LPCTSTR propVal)
{
	SetProperty(0x5, VT_BSTR, propVal);
}

short CTMShare::GetOwner()
{
	short result;
	GetProperty(0x6, VT_I2, (void*)&result);
	return result;
}

void CTMShare::SetOwner(short propVal)
{
	SetProperty(0x6, VT_I2, propVal);
}

long CTMShare::GetPeekPeriod()
{
	long result;
	GetProperty(0x7, VT_I4, (void*)&result);
	return result;
}

void CTMShare::SetPeekPeriod(long propVal)
{
	SetProperty(0x7, VT_I4, propVal);
}

long CTMShare::GetCommand()
{
	long result;
	GetProperty(0x8, VT_I4, (void*)&result);
	return result;
}

void CTMShare::SetCommand(long propVal)
{
	SetProperty(0x8, VT_I4, propVal);
}

long CTMShare::GetError()
{
	long result;
	GetProperty(0x9, VT_I4, (void*)&result);
	return result;
}

void CTMShare::SetError(long propVal)
{
	SetProperty(0x9, VT_I4, propVal);
}

CString CTMShare::GetMediaId()
{
	CString result;
	GetProperty(0xa, VT_BSTR, (void*)&result);
	return result;
}

void CTMShare::SetMediaId(LPCTSTR propVal)
{
	SetProperty(0xa, VT_BSTR, propVal);
}

BOOL CTMShare::GetEnableAxErrors()
{
	BOOL result;
	GetProperty(0xb, VT_BOOL, (void*)&result);
	return result;
}

void CTMShare::SetEnableAxErrors(BOOL propVal)
{
	SetProperty(0xb, VT_BOOL, propVal);
}

CString CTMShare::GetBarcode()
{
	CString result;
	GetProperty(0xc, VT_BSTR, (void*)&result);
	return result;
}

void CTMShare::SetBarcode(LPCTSTR propVal)
{
	SetProperty(0xc, VT_BSTR, propVal);
}

CString CTMShare::GetAppFolder()
{
	CString result;
	GetProperty(0xd, VT_BSTR, (void*)&result);
	return result;
}

void CTMShare::SetAppFolder(LPCTSTR propVal)
{
	SetProperty(0xd, VT_BSTR, propVal);
}

long CTMShare::GetPrimaryId()
{
	long result;
	GetProperty(0xe, VT_I4, (void*)&result);
	return result;
}

void CTMShare::SetPrimaryId(long propVal)
{
	SetProperty(0xe, VT_I4, propVal);
}

long CTMShare::GetSecondaryId()
{
	long result;
	GetProperty(0xf, VT_I4, (void*)&result);
	return result;
}

void CTMShare::SetSecondaryId(long propVal)
{
	SetProperty(0xf, VT_I4, propVal);
}

long CTMShare::GetTertiaryId()
{
	long result;
	GetProperty(0x10, VT_I4, (void*)&result);
	return result;
}

void CTMShare::SetTertiaryId(long propVal)
{
	SetProperty(0x10, VT_I4, propVal);
}

long CTMShare::GetQuaternaryId()
{
	long result;
	GetProperty(0x11, VT_I4, (void*)&result);
	return result;
}

void CTMShare::SetQuaternaryId(long propVal)
{
	SetProperty(0x11, VT_I4, propVal);
}

CString CTMShare::GetSourceFileName()
{
	CString result;
	GetProperty(0x12, VT_BSTR, (void*)&result);
	return result;
}

void CTMShare::SetSourceFileName(LPCTSTR propVal)
{
	SetProperty(0x12, VT_BSTR, propVal);
}

CString CTMShare::GetSourceFilePath()
{
	CString result;
	GetProperty(0x13, VT_BSTR, (void*)&result);
	return result;
}

void CTMShare::SetSourceFilePath(LPCTSTR propVal)
{
	SetProperty(0x13, VT_BSTR, propVal);
}

CString CTMShare::GetCaseFolder()
{
	CString result;
	GetProperty(0x14, VT_BSTR, (void*)&result);
	return result;
}

void CTMShare::SetCaseFolder(LPCTSTR propVal)
{
	SetProperty(0x14, VT_BSTR, propVal);
}

long CTMShare::GetBarcodeId()
{
	long result;
	GetProperty(0x15, VT_I4, (void*)&result);
	return result;
}

void CTMShare::SetBarcodeId(long propVal)
{
	SetProperty(0x15, VT_I4, propVal);
}

long CTMShare::GetDisplayOrder()
{
	long result;
	GetProperty(0x16, VT_I4, (void*)&result);
	return result;
}

void CTMShare::SetDisplayOrder(long propVal)
{
	SetProperty(0x16, VT_I4, propVal);
}

short CTMShare::GetVerQEF()
{
	short result;
	GetProperty(0x100, VT_I2, (void*)&result);
	return result;
}

void CTMShare::SetVerQEF(short propVal)
{
	SetProperty(0x100, VT_I2, propVal);
}

CString CTMShare::GetVerTextShort()
{
	CString result;
	GetProperty(0x101, VT_BSTR, (void*)&result);
	return result;
}

void CTMShare::SetVerTextShort(LPCTSTR propVal)
{
	SetProperty(0x101, VT_BSTR, propVal);
}

CString CTMShare::GetVerBuildDate()
{
	CString result;
	GetProperty(0x102, VT_BSTR, (void*)&result);
	return result;
}

void CTMShare::SetVerBuildDate(LPCTSTR propVal)
{
	SetProperty(0x102, VT_BSTR, propVal);
}

long CTMShare::GetPageNumber()
{
	long result;
	GetProperty(0x103, VT_I4, (void*)&result);
	return result;
}

void CTMShare::SetPageNumber(long propVal)
{
	SetProperty(0x103, VT_I4, propVal);
}

short CTMShare::GetLineNumber()
{
	short result;
	GetProperty(0x104, VT_I2, (void*)&result);
	return result;
}

void CTMShare::SetLineNumber(short propVal)
{
	SetProperty(0x104, VT_I2, propVal);
}

/////////////////////////////////////////////////////////////////////////////
// CTMShare operations

CString CTMShare::GetRegisteredPath()
{
	CString result;
	InvokeHelper(0x17, DISPATCH_METHOD, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CTMShare::GetClassIdString()
{
	CString result;
	InvokeHelper(0x18, DISPATCH_METHOD, VT_BSTR, (void*)&result, NULL);
	return result;
}

BOOL CTMShare::GetInitialized()
{
	BOOL result;
	InvokeHelper(0x19, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

short CTMShare::GetResponse()
{
	short result;
	InvokeHelper(0x1a, DISPATCH_METHOD, VT_I2, (void*)&result, NULL);
	return result;
}

short CTMShare::GetRequest()
{
	short result;
	InvokeHelper(0x1b, DISPATCH_METHOD, VT_I2, (void*)&result, NULL);
	return result;
}

short CTMShare::SetResponse()
{
	short result;
	InvokeHelper(0x1c, DISPATCH_METHOD, VT_I2, (void*)&result, NULL);
	return result;
}

void CTMShare::Terminate()
{
	InvokeHelper(0x1d, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

BOOL CTMShare::IsRunning()
{
	BOOL result;
	InvokeHelper(0x1e, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

CString CTMShare::GetSisterFileSpec()
{
	CString result;
	InvokeHelper(0x1f, DISPATCH_METHOD, VT_BSTR, (void*)&result, NULL);
	return result;
}

short CTMShare::Initialize()
{
	short result;
	InvokeHelper(0x20, DISPATCH_METHOD, VT_I2, (void*)&result, NULL);
	return result;
}

short CTMShare::SetRequest(long lWaitResponse)
{
	short result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x21, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lWaitResponse);
	return result;
}

short CTMShare::Execute()
{
	short result;
	InvokeHelper(0x22, DISPATCH_METHOD, VT_I2, (void*)&result, NULL);
	return result;
}

void CTMShare::AboutBox()
{
	InvokeHelper(0xfffffdd8, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}
