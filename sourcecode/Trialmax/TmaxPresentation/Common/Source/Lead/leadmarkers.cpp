// Machine generated IDispatch wrapper class(es) created by Microsoft Visual C++

// NOTE: Do not modify the contents of this file.  If this class is regenerated by
//  Microsoft Visual C++, your modifications will be overwritten.


#include "stdafx.h"
#include "leadmarkers.h"

// Dispatch interfaces referenced by this interface
#include "LEADMarker.h"


/////////////////////////////////////////////////////////////////////////////
// CLEADMarkers properties

long CLEADMarkers::GetCount()
{
	long result;
	GetProperty(0x1, VT_I4, (void*)&result);
	return result;
}

void CLEADMarkers::SetCount(long propVal)
{
	SetProperty(0x1, VT_I4, propVal);
}

/////////////////////////////////////////////////////////////////////////////
// CLEADMarkers operations

short CLEADMarkers::Delete(long lType, long lCount)
{
	short result;
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0x2, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lType, lCount);
	return result;
}

short CLEADMarkers::Insert(long lIndex, LPDISPATCH pMarker)
{
	short result;
	static BYTE parms[] =
		VTS_I4 VTS_DISPATCH;
	InvokeHelper(0x3, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lIndex, pMarker);
	return result;
}

short CLEADMarkers::Insert2(long lIndex, long lType, long lSize, const VARIANT& vData)
{
	short result;
	static BYTE parms[] =
		VTS_I4 VTS_I4 VTS_I4 VTS_VARIANT;
	InvokeHelper(0x5, DISPATCH_METHOD, VT_I2, (void*)&result, parms,
		lIndex, lType, lSize, &vData);
	return result;
}

LPDISPATCH CLEADMarkers::Clone()
{
	LPDISPATCH result;
	InvokeHelper(0x4, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

CLEADMarker CLEADMarkers::GetMarker(long lIndex)
{
	LPDISPATCH pDispatch;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x6, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&pDispatch, parms,
		lIndex);
	return CLEADMarker(pDispatch);
}

void CLEADMarkers::SetRefMarker(long lIndex, LPDISPATCH newValue)
{
	static BYTE parms[] =
		VTS_I4 VTS_DISPATCH;
	InvokeHelper(0x6, DISPATCH_PROPERTYPUTREF, VT_EMPTY, NULL, parms,
		 lIndex, newValue);
}
