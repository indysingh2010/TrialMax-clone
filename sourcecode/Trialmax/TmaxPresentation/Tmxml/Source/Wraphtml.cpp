// Machine generated IDispatch wrapper class(es) created with ClassWizard

#include "Stdafx.h"
#include "wraphtml.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif



/////////////////////////////////////////////////////////////////////////////
// CIHTMLDocument2 properties

/////////////////////////////////////////////////////////////////////////////
// CIHTMLDocument2 operations

LPDISPATCH CIHTMLDocument2::GetAll()
{
	LPDISPATCH result;
	InvokeHelper(0x3eb, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIHTMLDocument2::GetBody()
{
	LPDISPATCH result;
	InvokeHelper(0x3ec, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIHTMLDocument2::GetActiveElement()
{
	LPDISPATCH result;
	InvokeHelper(0x3ed, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIHTMLDocument2::GetImages()
{
	LPDISPATCH result;
	InvokeHelper(0x3f3, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIHTMLDocument2::GetApplets()
{
	LPDISPATCH result;
	InvokeHelper(0x3f0, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIHTMLDocument2::GetLinks()
{
	LPDISPATCH result;
	InvokeHelper(0x3f1, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIHTMLDocument2::GetForms()
{
	LPDISPATCH result;
	InvokeHelper(0x3f2, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIHTMLDocument2::GetAnchors()
{
	LPDISPATCH result;
	InvokeHelper(0x3ef, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetTitle(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x3f4, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIHTMLDocument2::GetTitle()
{
	CString result;
	InvokeHelper(0x3f4, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIHTMLDocument2::GetScripts()
{
	LPDISPATCH result;
	InvokeHelper(0x3f5, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIHTMLDocument2::GetSelection()
{
	LPDISPATCH result;
	InvokeHelper(0x3f9, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

CString CIHTMLDocument2::GetReadyState()
{
	CString result;
	InvokeHelper(0x3fa, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIHTMLDocument2::GetFrames()
{
	LPDISPATCH result;
	InvokeHelper(0x3fb, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIHTMLDocument2::GetEmbeds()
{
	LPDISPATCH result;
	InvokeHelper(0x3f7, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIHTMLDocument2::GetPlugins()
{
	LPDISPATCH result;
	InvokeHelper(0x3fd, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetAlinkColor(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x3fe, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetAlinkColor()
{
	VARIANT result;
	InvokeHelper(0x3fe, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetBgColor(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(DISPID_BACKCOLOR, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetBgColor()
{
	VARIANT result;
	InvokeHelper(DISPID_BACKCOLOR, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetFgColor(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x8001138a, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetFgColor()
{
	VARIANT result;
	InvokeHelper(0x8001138a, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetLinkColor(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x400, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetLinkColor()
{
	VARIANT result;
	InvokeHelper(0x400, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetVlinkColor(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x3ff, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetVlinkColor()
{
	VARIANT result;
	InvokeHelper(0x3ff, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

CString CIHTMLDocument2::GetReferrer()
{
	CString result;
	InvokeHelper(0x403, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIHTMLDocument2::GetLocation()
{
	LPDISPATCH result;
	InvokeHelper(0x402, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

CString CIHTMLDocument2::GetLastModified()
{
	CString result;
	InvokeHelper(0x404, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetUrl(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x401, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIHTMLDocument2::GetUrl()
{
	CString result;
	InvokeHelper(0x401, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetDomain(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x405, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIHTMLDocument2::GetDomain()
{
	CString result;
	InvokeHelper(0x405, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetCookie(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x406, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIHTMLDocument2::GetCookie()
{
	CString result;
	InvokeHelper(0x406, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetDefaultCharset(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x409, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIHTMLDocument2::GetDefaultCharset()
{
	CString result;
	InvokeHelper(0x409, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIHTMLDocument2::GetMimeType()
{
	CString result;
	InvokeHelper(0x411, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIHTMLDocument2::GetFileSize()
{
	CString result;
	InvokeHelper(0x412, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIHTMLDocument2::GetFileCreatedDate()
{
	CString result;
	InvokeHelper(0x413, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIHTMLDocument2::GetFileModifiedDate()
{
	CString result;
	InvokeHelper(0x414, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIHTMLDocument2::GetFileUpdatedDate()
{
	CString result;
	InvokeHelper(0x415, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIHTMLDocument2::GetSecurity()
{
	CString result;
	InvokeHelper(0x416, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIHTMLDocument2::GetProtocol()
{
	CString result;
	InvokeHelper(0x417, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIHTMLDocument2::GetNameProp()
{
	CString result;
	InvokeHelper(0x418, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIHTMLDocument2::open(LPCTSTR url, const VARIANT& name, const VARIANT& features, const VARIANT& replace)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR VTS_VARIANT VTS_VARIANT VTS_VARIANT;
	InvokeHelper(0x420, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		url, &name, &features, &replace);
	return result;
}

void CIHTMLDocument2::close()
{
	InvokeHelper(0x421, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void CIHTMLDocument2::clear()
{
	InvokeHelper(0x422, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

BOOL CIHTMLDocument2::queryCommandSupported(LPCTSTR cmdID)
{
	BOOL result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x423, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		cmdID);
	return result;
}

BOOL CIHTMLDocument2::queryCommandEnabled(LPCTSTR cmdID)
{
	BOOL result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x424, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		cmdID);
	return result;
}

BOOL CIHTMLDocument2::queryCommandState(LPCTSTR cmdID)
{
	BOOL result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x425, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		cmdID);
	return result;
}

BOOL CIHTMLDocument2::queryCommandIndeterm(LPCTSTR cmdID)
{
	BOOL result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x426, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		cmdID);
	return result;
}

CString CIHTMLDocument2::queryCommandText(LPCTSTR cmdID)
{
	CString result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x427, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		cmdID);
	return result;
}

VARIANT CIHTMLDocument2::queryCommandValue(LPCTSTR cmdID)
{
	VARIANT result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x428, DISPATCH_METHOD, VT_VARIANT, (void*)&result, parms,
		cmdID);
	return result;
}

BOOL CIHTMLDocument2::execCommand(LPCTSTR cmdID, BOOL showUI, const VARIANT& value)
{
	BOOL result;
	static BYTE parms[] =
		VTS_BSTR VTS_BOOL VTS_VARIANT;
	InvokeHelper(0x429, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		cmdID, showUI, &value);
	return result;
}

BOOL CIHTMLDocument2::execCommandShowHelp(LPCTSTR cmdID)
{
	BOOL result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x42a, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		cmdID);
	return result;
}

LPDISPATCH CIHTMLDocument2::createElement(LPCTSTR eTag)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x42b, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		eTag);
	return result;
}

void CIHTMLDocument2::SetOnhelp(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x8001177d, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetOnhelp()
{
	VARIANT result;
	InvokeHelper(0x8001177d, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetOnclick(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x80011778, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetOnclick()
{
	VARIANT result;
	InvokeHelper(0x80011778, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetOndblclick(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x80011779, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetOndblclick()
{
	VARIANT result;
	InvokeHelper(0x80011779, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetOnkeyup(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x80011776, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetOnkeyup()
{
	VARIANT result;
	InvokeHelper(0x80011776, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetOnkeydown(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x80011775, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetOnkeydown()
{
	VARIANT result;
	InvokeHelper(0x80011775, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetOnkeypress(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x80011777, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetOnkeypress()
{
	VARIANT result;
	InvokeHelper(0x80011777, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetOnmouseup(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x80011773, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetOnmouseup()
{
	VARIANT result;
	InvokeHelper(0x80011773, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetOnmousedown(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x80011772, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetOnmousedown()
{
	VARIANT result;
	InvokeHelper(0x80011772, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetOnmousemove(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x80011774, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetOnmousemove()
{
	VARIANT result;
	InvokeHelper(0x80011774, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetOnmouseout(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x80011771, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetOnmouseout()
{
	VARIANT result;
	InvokeHelper(0x80011771, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetOnmouseover(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x80011770, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetOnmouseover()
{
	VARIANT result;
	InvokeHelper(0x80011770, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetOnreadystatechange(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x80011789, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetOnreadystatechange()
{
	VARIANT result;
	InvokeHelper(0x80011789, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetOnafterupdate(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x80011786, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetOnafterupdate()
{
	VARIANT result;
	InvokeHelper(0x80011786, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetOnrowexit(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x80011782, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetOnrowexit()
{
	VARIANT result;
	InvokeHelper(0x80011782, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetOnrowenter(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x80011783, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetOnrowenter()
{
	VARIANT result;
	InvokeHelper(0x80011783, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetOndragstart(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x80011793, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetOndragstart()
{
	VARIANT result;
	InvokeHelper(0x80011793, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetOnselectstart(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x80011795, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetOnselectstart()
{
	VARIANT result;
	InvokeHelper(0x80011795, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIHTMLDocument2::elementFromPoint(long x, long y)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0x42c, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		x, y);
	return result;
}

LPDISPATCH CIHTMLDocument2::GetParentWindow()
{
	LPDISPATCH result;
	InvokeHelper(0x40a, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIHTMLDocument2::GetStyleSheets()
{
	LPDISPATCH result;
	InvokeHelper(0x42d, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetOnbeforeupdate(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x80011785, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetOnbeforeupdate()
{
	VARIANT result;
	InvokeHelper(0x80011785, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIHTMLDocument2::SetOnerrorupdate(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x80011796, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIHTMLDocument2::GetOnerrorupdate()
{
	VARIANT result;
	InvokeHelper(0x80011796, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

CString CIHTMLDocument2::toString()
{
	CString result;
	InvokeHelper(0x42e, DISPATCH_METHOD, VT_BSTR, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIHTMLDocument2::createStyleSheet(LPCTSTR bstrHref, long lIndex)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR VTS_I4;
	InvokeHelper(0x42f, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		bstrHref, lIndex);
	return result;
}


