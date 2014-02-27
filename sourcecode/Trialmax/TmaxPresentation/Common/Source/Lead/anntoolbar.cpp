// Machine generated IDispatch wrapper class(es) created by Microsoft Visual C++

// NOTE: Do not modify the contents of this file.  If this class is regenerated by
//  Microsoft Visual C++, your modifications will be overwritten.


#include "stdafx.h"
#include "anntoolbar.h"


/////////////////////////////////////////////////////////////////////////////
// CAnnToolbar properties

BOOL CAnnToolbar::GetVisible()
{
	BOOL result;
	GetProperty(0x1, VT_BOOL, (void*)&result);
	return result;
}

void CAnnToolbar::SetVisible(BOOL propVal)
{
	SetProperty(0x1, VT_BOOL, propVal);
}

short CAnnToolbar::GetTool()
{
	short result;
	GetProperty(0x2, VT_I2, (void*)&result);
	return result;
}

void CAnnToolbar::SetTool(short propVal)
{
	SetProperty(0x2, VT_I2, propVal);
}

BOOL CAnnToolbar::GetAutoSetTool()
{
	BOOL result;
	GetProperty(0x3, VT_BOOL, (void*)&result);
	return result;
}

void CAnnToolbar::SetAutoSetTool(BOOL propVal)
{
	SetProperty(0x3, VT_BOOL, propVal);
}

short CAnnToolbar::GetButtonCount()
{
	short result;
	GetProperty(0x4, VT_I2, (void*)&result);
	return result;
}

void CAnnToolbar::SetButtonCount(short propVal)
{
	SetProperty(0x4, VT_I2, propVal);
}

short CAnnToolbar::GetButtonColumns()
{
	short result;
	GetProperty(0x5, VT_I2, (void*)&result);
	return result;
}

void CAnnToolbar::SetButtonColumns(short propVal)
{
	SetProperty(0x5, VT_I2, propVal);
}

short CAnnToolbar::GetButtonRows()
{
	short result;
	GetProperty(0x6, VT_I2, (void*)&result);
	return result;
}

void CAnnToolbar::SetButtonRows(short propVal)
{
	SetProperty(0x6, VT_I2, propVal);
}

long CAnnToolbar::GetHWnd()
{
	long result;
	GetProperty(0x7, VT_I4, (void*)&result);
	return result;
}

void CAnnToolbar::SetHWnd(long propVal)
{
	SetProperty(0x7, VT_I4, propVal);
}

/////////////////////////////////////////////////////////////////////////////
// CAnnToolbar operations

short CAnnToolbar::Create(float x, float y, short iAlign)
{
	short result;
	static BYTE parms[] =
		VTS_R4 VTS_R4 VTS_I2;
	InvokeHelper(0x8, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		x, y, iAlign);
	return result;
}

BOOL CAnnToolbar::GetButtonVisible(short nButton)
{
	BOOL result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x9, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, parms,
		nButton);
	return result;
}

void CAnnToolbar::SetButtonVisible(short nButton, BOOL bNewValue)
{
	static BYTE parms[] =
		VTS_I2 VTS_BOOL;
	InvokeHelper(0x9, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 nButton, bNewValue);
}

short CAnnToolbar::GetButtonTool(short iButtonIndex)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_I2, (void*)&result, parms,
		iButtonIndex);
	return result;
}

void CAnnToolbar::SetButtonTool(short iButtonIndex, short nNewValue)
{
	static BYTE parms[] =
		VTS_I2 VTS_I2;
	InvokeHelper(0xa, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 iButtonIndex, nNewValue);
}

short CAnnToolbar::GetButtonToolCount(short iButtonIndex)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0xb, DISPATCH_PROPERTYGET, VT_I2, (void*)&result, parms,
		iButtonIndex);
	return result;
}

void CAnnToolbar::SetButtonToolCount(short iButtonIndex, short nNewValue)
{
	static BYTE parms[] =
		VTS_I2 VTS_I2;
	InvokeHelper(0xb, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 iButtonIndex, nNewValue);
}

short CAnnToolbar::GetButtonMenuTool(short iButtonIndex, short iToolIndex)
{
	short result;
	static BYTE parms[] =
		VTS_I2 VTS_I2;
	InvokeHelper(0xc, DISPATCH_PROPERTYGET, VT_I2, (void*)&result, parms,
		iButtonIndex, iToolIndex);
	return result;
}

void CAnnToolbar::SetButtonMenuTool(short iButtonIndex, short iToolIndex, short nNewValue)
{
	static BYTE parms[] =
		VTS_I2 VTS_I2 VTS_I2;
	InvokeHelper(0xc, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 iButtonIndex, iToolIndex, nNewValue);
}

short CAnnToolbar::GetButtonMenuTextID(short iButtonIndex, short iToolIndex)
{
	short result;
	static BYTE parms[] =
		VTS_I2 VTS_I2;
	InvokeHelper(0xd, DISPATCH_PROPERTYGET, VT_I2, (void*)&result, parms,
		iButtonIndex, iToolIndex);
	return result;
}

void CAnnToolbar::SetButtonMenuTextID(short iButtonIndex, short iToolIndex, short nNewValue)
{
	static BYTE parms[] =
		VTS_I2 VTS_I2 VTS_I2;
	InvokeHelper(0xd, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 iButtonIndex, iToolIndex, nNewValue);
}

CString CAnnToolbar::GetButtonMenuText(short iButtonIndex, short iToolIndex)
{
	CString result;
	static BYTE parms[] =
		VTS_I2 VTS_I2;
	InvokeHelper(0xe, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, parms,
		iButtonIndex, iToolIndex);
	return result;
}

void CAnnToolbar::SetButtonMenuText(short iButtonIndex, short iToolIndex, LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_I2 VTS_I2 VTS_BSTR;
	InvokeHelper(0xe, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 iButtonIndex, iToolIndex, lpszNewValue);
}

long CAnnToolbar::GetButtonBitmapDown(short iButtonIndex)
{
	long result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0xf, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, parms,
		iButtonIndex);
	return result;
}

void CAnnToolbar::SetButtonBitmapDown(short iButtonIndex, long nNewValue)
{
	static BYTE parms[] =
		VTS_I2 VTS_I4;
	InvokeHelper(0xf, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 iButtonIndex, nNewValue);
}

long CAnnToolbar::GetButtonBitmapUp(short iButtonIndex)
{
	long result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x10, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, parms,
		iButtonIndex);
	return result;
}

void CAnnToolbar::SetButtonBitmapUp(short iButtonIndex, long nNewValue)
{
	static BYTE parms[] =
		VTS_I2 VTS_I4;
	InvokeHelper(0x10, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 iButtonIndex, nNewValue);
}

short CAnnToolbar::GetButtonToolTipTextID(short iButtonIndex)
{
	short result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x11, DISPATCH_PROPERTYGET, VT_I2, (void*)&result, parms,
		iButtonIndex);
	return result;
}

void CAnnToolbar::SetButtonToolTipTextID(short iButtonIndex, short nNewValue)
{
	static BYTE parms[] =
		VTS_I2 VTS_I2;
	InvokeHelper(0x11, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 iButtonIndex, nNewValue);
}

CString CAnnToolbar::GetButtonToolTipText(short iButtonIndex)
{
	CString result;
	static BYTE parms[] =
		VTS_I2;
	InvokeHelper(0x12, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, parms,
		iButtonIndex);
	return result;
}

void CAnnToolbar::SetButtonToolTipText(short iButtonIndex, LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_I2 VTS_BSTR;
	InvokeHelper(0x12, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 iButtonIndex, lpszNewValue);
}
