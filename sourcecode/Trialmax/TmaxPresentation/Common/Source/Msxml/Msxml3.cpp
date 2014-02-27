// Machine generated IDispatch wrapper class(es) created with ClassWizard

#include "stdafx.h"
#include "msxml3.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif



/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMImplementation properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMImplementation operations

BOOL CIXMLDOMImplementation::hasFeature(LPCTSTR feature, LPCTSTR version)
{
	BOOL result;
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR;
	InvokeHelper(0x91, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		feature, version);
	return result;
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMNode properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMNode operations

CString CIXMLDOMNode::GetNodeName()
{
	CString result;
	InvokeHelper(0x2, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMNode::GetNodeValue()
{
	VARIANT result;
	InvokeHelper(0x3, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMNode::SetNodeValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x3, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

long CIXMLDOMNode::GetNodeType()
{
	long result;
	InvokeHelper(0x4, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNode::GetParentNode()
{
	LPDISPATCH result;
	InvokeHelper(0x6, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNode::GetChildNodes()
{
	LPDISPATCH result;
	InvokeHelper(0x7, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNode::GetFirstChild()
{
	LPDISPATCH result;
	InvokeHelper(0x8, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNode::GetLastChild()
{
	LPDISPATCH result;
	InvokeHelper(0x9, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNode::GetPreviousSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNode::GetNextSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xb, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNode::GetAttributes()
{
	LPDISPATCH result;
	InvokeHelper(0xc, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNode::insertBefore(LPDISPATCH newChild, const VARIANT& refChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0xd, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, &refChild);
	return result;
}

LPDISPATCH CIXMLDOMNode::replaceChild(LPDISPATCH newChild, LPDISPATCH oldChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_DISPATCH;
	InvokeHelper(0xe, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, oldChild);
	return result;
}

LPDISPATCH CIXMLDOMNode::removeChild(LPDISPATCH childNode)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xf, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		childNode);
	return result;
}

LPDISPATCH CIXMLDOMNode::appendChild(LPDISPATCH newChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x10, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild);
	return result;
}

BOOL CIXMLDOMNode::hasChildNodes()
{
	BOOL result;
	InvokeHelper(0x11, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNode::GetOwnerDocument()
{
	LPDISPATCH result;
	InvokeHelper(0x12, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNode::cloneNode(BOOL deep)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x13, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		deep);
	return result;
}

CString CIXMLDOMNode::GetNodeTypeString()
{
	CString result;
	InvokeHelper(0x15, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMNode::GetText()
{
	CString result;
	InvokeHelper(0x18, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMNode::SetText(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x18, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

BOOL CIXMLDOMNode::GetSpecified()
{
	BOOL result;
	InvokeHelper(0x16, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNode::GetDefinition()
{
	LPDISPATCH result;
	InvokeHelper(0x17, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMNode::GetNodeTypedValue()
{
	VARIANT result;
	InvokeHelper(0x19, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMNode::SetNodeTypedValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x19, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIXMLDOMNode::GetDataType()
{
	VARIANT result;
	InvokeHelper(0x1a, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMNode::SetDataType(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1a, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIXMLDOMNode::GetXml()
{
	CString result;
	InvokeHelper(0x1b, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMNode::transformNode(LPDISPATCH stylesheet)
{
	CString result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x1c, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		stylesheet);
	return result;
}

LPDISPATCH CIXMLDOMNode::selectNodes(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1d, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

LPDISPATCH CIXMLDOMNode::selectSingleNode(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1e, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

BOOL CIXMLDOMNode::GetParsed()
{
	BOOL result;
	InvokeHelper(0x1f, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMNode::GetNamespaceURI()
{
	CString result;
	InvokeHelper(0x20, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMNode::GetPrefix()
{
	CString result;
	InvokeHelper(0x21, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMNode::GetBaseName()
{
	CString result;
	InvokeHelper(0x22, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMNode::transformNodeToObject(LPDISPATCH stylesheet, const VARIANT& outputObject)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0x23, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 stylesheet, &outputObject);
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMNodeList properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMNodeList operations

LPDISPATCH CIXMLDOMNodeList::GetItem(long index)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x0, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, parms,
		index);
	return result;
}

long CIXMLDOMNodeList::GetLength()
{
	long result;
	InvokeHelper(0x4a, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNodeList::nextNode()
{
	LPDISPATCH result;
	InvokeHelper(0x4c, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void CIXMLDOMNodeList::reset()
{
	InvokeHelper(0x4d, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMNamedNodeMap properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMNamedNodeMap operations

LPDISPATCH CIXMLDOMNamedNodeMap::getNamedItem(LPCTSTR name)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x53, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		name);
	return result;
}

LPDISPATCH CIXMLDOMNamedNodeMap::setNamedItem(LPDISPATCH newItem)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x54, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newItem);
	return result;
}

LPDISPATCH CIXMLDOMNamedNodeMap::removeNamedItem(LPCTSTR name)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x55, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		name);
	return result;
}

LPDISPATCH CIXMLDOMNamedNodeMap::GetItem(long index)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x0, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, parms,
		index);
	return result;
}

long CIXMLDOMNamedNodeMap::GetLength()
{
	long result;
	InvokeHelper(0x4a, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNamedNodeMap::getQualifiedItem(LPCTSTR baseName, LPCTSTR namespaceURI)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR;
	InvokeHelper(0x57, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		baseName, namespaceURI);
	return result;
}

LPDISPATCH CIXMLDOMNamedNodeMap::removeQualifiedItem(LPCTSTR baseName, LPCTSTR namespaceURI)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR;
	InvokeHelper(0x58, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		baseName, namespaceURI);
	return result;
}

LPDISPATCH CIXMLDOMNamedNodeMap::nextNode()
{
	LPDISPATCH result;
	InvokeHelper(0x59, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void CIXMLDOMNamedNodeMap::reset()
{
	InvokeHelper(0x5a, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMDocument properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMDocument operations

CString CIXMLDOMDocument::GetNodeName()
{
	CString result;
	InvokeHelper(0x2, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMDocument::GetNodeValue()
{
	VARIANT result;
	InvokeHelper(0x3, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument::SetNodeValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x3, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

long CIXMLDOMDocument::GetNodeType()
{
	long result;
	InvokeHelper(0x4, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument::GetParentNode()
{
	LPDISPATCH result;
	InvokeHelper(0x6, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument::GetChildNodes()
{
	LPDISPATCH result;
	InvokeHelper(0x7, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument::GetFirstChild()
{
	LPDISPATCH result;
	InvokeHelper(0x8, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument::GetLastChild()
{
	LPDISPATCH result;
	InvokeHelper(0x9, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument::GetPreviousSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument::GetNextSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xb, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument::GetAttributes()
{
	LPDISPATCH result;
	InvokeHelper(0xc, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument::insertBefore(LPDISPATCH newChild, const VARIANT& refChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0xd, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, &refChild);
	return result;
}

LPDISPATCH CIXMLDOMDocument::replaceChild(LPDISPATCH newChild, LPDISPATCH oldChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_DISPATCH;
	InvokeHelper(0xe, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, oldChild);
	return result;
}

LPDISPATCH CIXMLDOMDocument::removeChild(LPDISPATCH childNode)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xf, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		childNode);
	return result;
}

LPDISPATCH CIXMLDOMDocument::appendChild(LPDISPATCH newChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x10, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild);
	return result;
}

BOOL CIXMLDOMDocument::hasChildNodes()
{
	BOOL result;
	InvokeHelper(0x11, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument::GetOwnerDocument()
{
	LPDISPATCH result;
	InvokeHelper(0x12, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument::cloneNode(BOOL deep)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x13, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		deep);
	return result;
}

CString CIXMLDOMDocument::GetNodeTypeString()
{
	CString result;
	InvokeHelper(0x15, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocument::GetText()
{
	CString result;
	InvokeHelper(0x18, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument::SetText(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x18, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

BOOL CIXMLDOMDocument::GetSpecified()
{
	BOOL result;
	InvokeHelper(0x16, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument::GetDefinition()
{
	LPDISPATCH result;
	InvokeHelper(0x17, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMDocument::GetNodeTypedValue()
{
	VARIANT result;
	InvokeHelper(0x19, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument::SetNodeTypedValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x19, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIXMLDOMDocument::GetDataType()
{
	VARIANT result;
	InvokeHelper(0x1a, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument::SetDataType(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1a, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIXMLDOMDocument::GetXml()
{
	CString result;
	InvokeHelper(0x1b, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocument::transformNode(LPDISPATCH stylesheet)
{
	CString result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x1c, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		stylesheet);
	return result;
}

LPDISPATCH CIXMLDOMDocument::selectNodes(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1d, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

LPDISPATCH CIXMLDOMDocument::selectSingleNode(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1e, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

BOOL CIXMLDOMDocument::GetParsed()
{
	BOOL result;
	InvokeHelper(0x1f, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocument::GetNamespaceURI()
{
	CString result;
	InvokeHelper(0x20, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocument::GetPrefix()
{
	CString result;
	InvokeHelper(0x21, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocument::GetBaseName()
{
	CString result;
	InvokeHelper(0x22, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument::transformNodeToObject(LPDISPATCH stylesheet, const VARIANT& outputObject)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0x23, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 stylesheet, &outputObject);
}

LPDISPATCH CIXMLDOMDocument::GetDoctype()
{
	LPDISPATCH result;
	InvokeHelper(0x26, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument::GetImplementation()
{
	LPDISPATCH result;
	InvokeHelper(0x27, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument::GetDocumentElement()
{
	LPDISPATCH result;
	InvokeHelper(0x28, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument::SetRefDocumentElement(LPDISPATCH newValue)
{
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x28, DISPATCH_PROPERTYPUTREF, VT_EMPTY, NULL, parms,
		 newValue);
}

LPDISPATCH CIXMLDOMDocument::createElement(LPCTSTR tagName)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x29, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		tagName);
	return result;
}

LPDISPATCH CIXMLDOMDocument::createDocumentFragment()
{
	LPDISPATCH result;
	InvokeHelper(0x2a, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument::createTextNode(LPCTSTR data)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x2b, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		data);
	return result;
}

LPDISPATCH CIXMLDOMDocument::createComment(LPCTSTR data)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x2c, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		data);
	return result;
}

LPDISPATCH CIXMLDOMDocument::createCDATASection(LPCTSTR data)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x2d, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		data);
	return result;
}

LPDISPATCH CIXMLDOMDocument::createProcessingInstruction(LPCTSTR target, LPCTSTR data)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR;
	InvokeHelper(0x2e, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		target, data);
	return result;
}

LPDISPATCH CIXMLDOMDocument::createAttribute(LPCTSTR name)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x2f, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		name);
	return result;
}

LPDISPATCH CIXMLDOMDocument::createEntityReference(LPCTSTR name)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x31, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		name);
	return result;
}

LPDISPATCH CIXMLDOMDocument::getElementsByTagName(LPCTSTR tagName)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x32, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		tagName);
	return result;
}

LPDISPATCH CIXMLDOMDocument::createNode(const VARIANT& type, LPCTSTR name, LPCTSTR namespaceURI)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_VARIANT VTS_BSTR VTS_BSTR;
	InvokeHelper(0x36, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		&type, name, namespaceURI);
	return result;
}

LPDISPATCH CIXMLDOMDocument::nodeFromID(LPCTSTR idString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x38, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		idString);
	return result;
}

BOOL CIXMLDOMDocument::load(const VARIANT& xmlSource)
{
	BOOL result;
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x3a, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		&xmlSource);
	return result;
}

long CIXMLDOMDocument::GetReadyState()
{
	long result;
	InvokeHelper(DISPID_READYSTATE, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument::GetParseError()
{
	LPDISPATCH result;
	InvokeHelper(0x3b, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocument::GetUrl()
{
	CString result;
	InvokeHelper(0x3c, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

BOOL CIXMLDOMDocument::GetAsync()
{
	BOOL result;
	InvokeHelper(0x3d, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument::SetAsync(BOOL bNewValue)
{
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x3d, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 bNewValue);
}

void CIXMLDOMDocument::abort()
{
	InvokeHelper(0x3e, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

BOOL CIXMLDOMDocument::loadXML(LPCTSTR bstrXML)
{
	BOOL result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x3f, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		bstrXML);
	return result;
}

void CIXMLDOMDocument::save(const VARIANT& destination)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x40, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 &destination);
}

BOOL CIXMLDOMDocument::GetValidateOnParse()
{
	BOOL result;
	InvokeHelper(0x41, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument::SetValidateOnParse(BOOL bNewValue)
{
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x41, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 bNewValue);
}

BOOL CIXMLDOMDocument::GetResolveExternals()
{
	BOOL result;
	InvokeHelper(0x42, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument::SetResolveExternals(BOOL bNewValue)
{
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x42, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 bNewValue);
}

BOOL CIXMLDOMDocument::GetPreserveWhiteSpace()
{
	BOOL result;
	InvokeHelper(0x43, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument::SetPreserveWhiteSpace(BOOL bNewValue)
{
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x43, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 bNewValue);
}

void CIXMLDOMDocument::SetOnreadystatechange(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x44, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

void CIXMLDOMDocument::SetOndataavailable(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x45, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

void CIXMLDOMDocument::SetOntransformnode(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x46, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMDocumentType properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMDocumentType operations

CString CIXMLDOMDocumentType::GetNodeName()
{
	CString result;
	InvokeHelper(0x2, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMDocumentType::GetNodeValue()
{
	VARIANT result;
	InvokeHelper(0x3, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocumentType::SetNodeValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x3, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

long CIXMLDOMDocumentType::GetNodeType()
{
	long result;
	InvokeHelper(0x4, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentType::GetParentNode()
{
	LPDISPATCH result;
	InvokeHelper(0x6, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentType::GetChildNodes()
{
	LPDISPATCH result;
	InvokeHelper(0x7, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentType::GetFirstChild()
{
	LPDISPATCH result;
	InvokeHelper(0x8, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentType::GetLastChild()
{
	LPDISPATCH result;
	InvokeHelper(0x9, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentType::GetPreviousSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentType::GetNextSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xb, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentType::GetAttributes()
{
	LPDISPATCH result;
	InvokeHelper(0xc, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentType::insertBefore(LPDISPATCH newChild, const VARIANT& refChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0xd, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, &refChild);
	return result;
}

LPDISPATCH CIXMLDOMDocumentType::replaceChild(LPDISPATCH newChild, LPDISPATCH oldChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_DISPATCH;
	InvokeHelper(0xe, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, oldChild);
	return result;
}

LPDISPATCH CIXMLDOMDocumentType::removeChild(LPDISPATCH childNode)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xf, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		childNode);
	return result;
}

LPDISPATCH CIXMLDOMDocumentType::appendChild(LPDISPATCH newChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x10, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild);
	return result;
}

BOOL CIXMLDOMDocumentType::hasChildNodes()
{
	BOOL result;
	InvokeHelper(0x11, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentType::GetOwnerDocument()
{
	LPDISPATCH result;
	InvokeHelper(0x12, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentType::cloneNode(BOOL deep)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x13, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		deep);
	return result;
}

CString CIXMLDOMDocumentType::GetNodeTypeString()
{
	CString result;
	InvokeHelper(0x15, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocumentType::GetText()
{
	CString result;
	InvokeHelper(0x18, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocumentType::SetText(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x18, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

BOOL CIXMLDOMDocumentType::GetSpecified()
{
	BOOL result;
	InvokeHelper(0x16, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentType::GetDefinition()
{
	LPDISPATCH result;
	InvokeHelper(0x17, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMDocumentType::GetNodeTypedValue()
{
	VARIANT result;
	InvokeHelper(0x19, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocumentType::SetNodeTypedValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x19, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIXMLDOMDocumentType::GetDataType()
{
	VARIANT result;
	InvokeHelper(0x1a, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocumentType::SetDataType(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1a, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIXMLDOMDocumentType::GetXml()
{
	CString result;
	InvokeHelper(0x1b, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocumentType::transformNode(LPDISPATCH stylesheet)
{
	CString result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x1c, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		stylesheet);
	return result;
}

LPDISPATCH CIXMLDOMDocumentType::selectNodes(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1d, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

LPDISPATCH CIXMLDOMDocumentType::selectSingleNode(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1e, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

BOOL CIXMLDOMDocumentType::GetParsed()
{
	BOOL result;
	InvokeHelper(0x1f, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocumentType::GetNamespaceURI()
{
	CString result;
	InvokeHelper(0x20, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocumentType::GetPrefix()
{
	CString result;
	InvokeHelper(0x21, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocumentType::GetBaseName()
{
	CString result;
	InvokeHelper(0x22, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocumentType::transformNodeToObject(LPDISPATCH stylesheet, const VARIANT& outputObject)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0x23, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 stylesheet, &outputObject);
}

CString CIXMLDOMDocumentType::GetName()
{
	CString result;
	InvokeHelper(0x83, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentType::GetEntities()
{
	LPDISPATCH result;
	InvokeHelper(0x84, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentType::GetNotations()
{
	LPDISPATCH result;
	InvokeHelper(0x85, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMElement properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMElement operations

CString CIXMLDOMElement::GetNodeName()
{
	CString result;
	InvokeHelper(0x2, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMElement::GetNodeValue()
{
	VARIANT result;
	InvokeHelper(0x3, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMElement::SetNodeValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x3, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

long CIXMLDOMElement::GetNodeType()
{
	long result;
	InvokeHelper(0x4, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMElement::GetParentNode()
{
	LPDISPATCH result;
	InvokeHelper(0x6, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMElement::GetChildNodes()
{
	LPDISPATCH result;
	InvokeHelper(0x7, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMElement::GetFirstChild()
{
	LPDISPATCH result;
	InvokeHelper(0x8, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMElement::GetLastChild()
{
	LPDISPATCH result;
	InvokeHelper(0x9, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMElement::GetPreviousSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMElement::GetNextSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xb, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMElement::GetAttributes()
{
	LPDISPATCH result;
	InvokeHelper(0xc, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMElement::insertBefore(LPDISPATCH newChild, const VARIANT& refChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0xd, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, &refChild);
	return result;
}

LPDISPATCH CIXMLDOMElement::replaceChild(LPDISPATCH newChild, LPDISPATCH oldChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_DISPATCH;
	InvokeHelper(0xe, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, oldChild);
	return result;
}

LPDISPATCH CIXMLDOMElement::removeChild(LPDISPATCH childNode)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xf, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		childNode);
	return result;
}

LPDISPATCH CIXMLDOMElement::appendChild(LPDISPATCH newChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x10, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild);
	return result;
}

BOOL CIXMLDOMElement::hasChildNodes()
{
	BOOL result;
	InvokeHelper(0x11, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMElement::GetOwnerDocument()
{
	LPDISPATCH result;
	InvokeHelper(0x12, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMElement::cloneNode(BOOL deep)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x13, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		deep);
	return result;
}

CString CIXMLDOMElement::GetNodeTypeString()
{
	CString result;
	InvokeHelper(0x15, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMElement::GetText()
{
	CString result;
	InvokeHelper(0x18, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMElement::SetText(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x18, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

BOOL CIXMLDOMElement::GetSpecified()
{
	BOOL result;
	InvokeHelper(0x16, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMElement::GetDefinition()
{
	LPDISPATCH result;
	InvokeHelper(0x17, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMElement::GetNodeTypedValue()
{
	VARIANT result;
	InvokeHelper(0x19, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMElement::SetNodeTypedValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x19, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIXMLDOMElement::GetDataType()
{
	VARIANT result;
	InvokeHelper(0x1a, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMElement::SetDataType(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1a, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIXMLDOMElement::GetXml()
{
	CString result;
	InvokeHelper(0x1b, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMElement::transformNode(LPDISPATCH stylesheet)
{
	CString result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x1c, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		stylesheet);
	return result;
}

LPDISPATCH CIXMLDOMElement::selectNodes(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1d, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

LPDISPATCH CIXMLDOMElement::selectSingleNode(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1e, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

BOOL CIXMLDOMElement::GetParsed()
{
	BOOL result;
	InvokeHelper(0x1f, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMElement::GetNamespaceURI()
{
	CString result;
	InvokeHelper(0x20, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMElement::GetPrefix()
{
	CString result;
	InvokeHelper(0x21, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMElement::GetBaseName()
{
	CString result;
	InvokeHelper(0x22, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMElement::transformNodeToObject(LPDISPATCH stylesheet, const VARIANT& outputObject)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0x23, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 stylesheet, &outputObject);
}

CString CIXMLDOMElement::GetTagName()
{
	CString result;
	InvokeHelper(0x61, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMElement::getAttribute(LPCTSTR name)
{
	VARIANT result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x63, DISPATCH_METHOD, VT_VARIANT, (void*)&result, parms,
		name);
	return result;
}

void CIXMLDOMElement::setAttribute(LPCTSTR name, const VARIANT& value)
{
	static BYTE parms[] =
		VTS_BSTR VTS_VARIANT;
	InvokeHelper(0x64, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 name, &value);
}

void CIXMLDOMElement::removeAttribute(LPCTSTR name)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x65, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 name);
}

LPDISPATCH CIXMLDOMElement::getAttributeNode(LPCTSTR name)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x66, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		name);
	return result;
}

LPDISPATCH CIXMLDOMElement::setAttributeNode(LPDISPATCH DOMAttribute)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x67, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		DOMAttribute);
	return result;
}

LPDISPATCH CIXMLDOMElement::removeAttributeNode(LPDISPATCH DOMAttribute)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x68, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		DOMAttribute);
	return result;
}

LPDISPATCH CIXMLDOMElement::getElementsByTagName(LPCTSTR tagName)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x69, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		tagName);
	return result;
}

void CIXMLDOMElement::normalize()
{
	InvokeHelper(0x6a, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMAttribute properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMAttribute operations

CString CIXMLDOMAttribute::GetNodeName()
{
	CString result;
	InvokeHelper(0x2, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMAttribute::GetNodeValue()
{
	VARIANT result;
	InvokeHelper(0x3, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMAttribute::SetNodeValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x3, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

long CIXMLDOMAttribute::GetNodeType()
{
	long result;
	InvokeHelper(0x4, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMAttribute::GetParentNode()
{
	LPDISPATCH result;
	InvokeHelper(0x6, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMAttribute::GetChildNodes()
{
	LPDISPATCH result;
	InvokeHelper(0x7, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMAttribute::GetFirstChild()
{
	LPDISPATCH result;
	InvokeHelper(0x8, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMAttribute::GetLastChild()
{
	LPDISPATCH result;
	InvokeHelper(0x9, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMAttribute::GetPreviousSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMAttribute::GetNextSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xb, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMAttribute::GetAttributes()
{
	LPDISPATCH result;
	InvokeHelper(0xc, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMAttribute::insertBefore(LPDISPATCH newChild, const VARIANT& refChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0xd, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, &refChild);
	return result;
}

LPDISPATCH CIXMLDOMAttribute::replaceChild(LPDISPATCH newChild, LPDISPATCH oldChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_DISPATCH;
	InvokeHelper(0xe, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, oldChild);
	return result;
}

LPDISPATCH CIXMLDOMAttribute::removeChild(LPDISPATCH childNode)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xf, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		childNode);
	return result;
}

LPDISPATCH CIXMLDOMAttribute::appendChild(LPDISPATCH newChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x10, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild);
	return result;
}

BOOL CIXMLDOMAttribute::hasChildNodes()
{
	BOOL result;
	InvokeHelper(0x11, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMAttribute::GetOwnerDocument()
{
	LPDISPATCH result;
	InvokeHelper(0x12, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMAttribute::cloneNode(BOOL deep)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x13, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		deep);
	return result;
}

CString CIXMLDOMAttribute::GetNodeTypeString()
{
	CString result;
	InvokeHelper(0x15, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMAttribute::GetText()
{
	CString result;
	InvokeHelper(0x18, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMAttribute::SetText(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x18, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

BOOL CIXMLDOMAttribute::GetSpecified()
{
	BOOL result;
	InvokeHelper(0x16, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMAttribute::GetDefinition()
{
	LPDISPATCH result;
	InvokeHelper(0x17, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMAttribute::GetNodeTypedValue()
{
	VARIANT result;
	InvokeHelper(0x19, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMAttribute::SetNodeTypedValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x19, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIXMLDOMAttribute::GetDataType()
{
	VARIANT result;
	InvokeHelper(0x1a, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMAttribute::SetDataType(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1a, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIXMLDOMAttribute::GetXml()
{
	CString result;
	InvokeHelper(0x1b, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMAttribute::transformNode(LPDISPATCH stylesheet)
{
	CString result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x1c, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		stylesheet);
	return result;
}

LPDISPATCH CIXMLDOMAttribute::selectNodes(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1d, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

LPDISPATCH CIXMLDOMAttribute::selectSingleNode(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1e, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

BOOL CIXMLDOMAttribute::GetParsed()
{
	BOOL result;
	InvokeHelper(0x1f, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMAttribute::GetNamespaceURI()
{
	CString result;
	InvokeHelper(0x20, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMAttribute::GetPrefix()
{
	CString result;
	InvokeHelper(0x21, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMAttribute::GetBaseName()
{
	CString result;
	InvokeHelper(0x22, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMAttribute::transformNodeToObject(LPDISPATCH stylesheet, const VARIANT& outputObject)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0x23, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 stylesheet, &outputObject);
}

CString CIXMLDOMAttribute::GetName()
{
	CString result;
	InvokeHelper(0x76, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMAttribute::GetValue()
{
	VARIANT result;
	InvokeHelper(0x78, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMAttribute::SetValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x78, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMDocumentFragment properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMDocumentFragment operations

CString CIXMLDOMDocumentFragment::GetNodeName()
{
	CString result;
	InvokeHelper(0x2, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMDocumentFragment::GetNodeValue()
{
	VARIANT result;
	InvokeHelper(0x3, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocumentFragment::SetNodeValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x3, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

long CIXMLDOMDocumentFragment::GetNodeType()
{
	long result;
	InvokeHelper(0x4, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentFragment::GetParentNode()
{
	LPDISPATCH result;
	InvokeHelper(0x6, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentFragment::GetChildNodes()
{
	LPDISPATCH result;
	InvokeHelper(0x7, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentFragment::GetFirstChild()
{
	LPDISPATCH result;
	InvokeHelper(0x8, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentFragment::GetLastChild()
{
	LPDISPATCH result;
	InvokeHelper(0x9, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentFragment::GetPreviousSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentFragment::GetNextSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xb, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentFragment::GetAttributes()
{
	LPDISPATCH result;
	InvokeHelper(0xc, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentFragment::insertBefore(LPDISPATCH newChild, const VARIANT& refChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0xd, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, &refChild);
	return result;
}

LPDISPATCH CIXMLDOMDocumentFragment::replaceChild(LPDISPATCH newChild, LPDISPATCH oldChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_DISPATCH;
	InvokeHelper(0xe, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, oldChild);
	return result;
}

LPDISPATCH CIXMLDOMDocumentFragment::removeChild(LPDISPATCH childNode)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xf, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		childNode);
	return result;
}

LPDISPATCH CIXMLDOMDocumentFragment::appendChild(LPDISPATCH newChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x10, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild);
	return result;
}

BOOL CIXMLDOMDocumentFragment::hasChildNodes()
{
	BOOL result;
	InvokeHelper(0x11, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentFragment::GetOwnerDocument()
{
	LPDISPATCH result;
	InvokeHelper(0x12, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentFragment::cloneNode(BOOL deep)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x13, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		deep);
	return result;
}

CString CIXMLDOMDocumentFragment::GetNodeTypeString()
{
	CString result;
	InvokeHelper(0x15, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocumentFragment::GetText()
{
	CString result;
	InvokeHelper(0x18, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocumentFragment::SetText(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x18, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

BOOL CIXMLDOMDocumentFragment::GetSpecified()
{
	BOOL result;
	InvokeHelper(0x16, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocumentFragment::GetDefinition()
{
	LPDISPATCH result;
	InvokeHelper(0x17, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMDocumentFragment::GetNodeTypedValue()
{
	VARIANT result;
	InvokeHelper(0x19, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocumentFragment::SetNodeTypedValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x19, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIXMLDOMDocumentFragment::GetDataType()
{
	VARIANT result;
	InvokeHelper(0x1a, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocumentFragment::SetDataType(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1a, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIXMLDOMDocumentFragment::GetXml()
{
	CString result;
	InvokeHelper(0x1b, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocumentFragment::transformNode(LPDISPATCH stylesheet)
{
	CString result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x1c, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		stylesheet);
	return result;
}

LPDISPATCH CIXMLDOMDocumentFragment::selectNodes(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1d, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

LPDISPATCH CIXMLDOMDocumentFragment::selectSingleNode(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1e, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

BOOL CIXMLDOMDocumentFragment::GetParsed()
{
	BOOL result;
	InvokeHelper(0x1f, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocumentFragment::GetNamespaceURI()
{
	CString result;
	InvokeHelper(0x20, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocumentFragment::GetPrefix()
{
	CString result;
	InvokeHelper(0x21, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocumentFragment::GetBaseName()
{
	CString result;
	InvokeHelper(0x22, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocumentFragment::transformNodeToObject(LPDISPATCH stylesheet, const VARIANT& outputObject)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0x23, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 stylesheet, &outputObject);
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMText properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMText operations

CString CIXMLDOMText::GetNodeName()
{
	CString result;
	InvokeHelper(0x2, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMText::GetNodeValue()
{
	VARIANT result;
	InvokeHelper(0x3, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMText::SetNodeValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x3, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

long CIXMLDOMText::GetNodeType()
{
	long result;
	InvokeHelper(0x4, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMText::GetParentNode()
{
	LPDISPATCH result;
	InvokeHelper(0x6, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMText::GetChildNodes()
{
	LPDISPATCH result;
	InvokeHelper(0x7, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMText::GetFirstChild()
{
	LPDISPATCH result;
	InvokeHelper(0x8, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMText::GetLastChild()
{
	LPDISPATCH result;
	InvokeHelper(0x9, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMText::GetPreviousSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMText::GetNextSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xb, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMText::GetAttributes()
{
	LPDISPATCH result;
	InvokeHelper(0xc, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMText::insertBefore(LPDISPATCH newChild, const VARIANT& refChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0xd, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, &refChild);
	return result;
}

LPDISPATCH CIXMLDOMText::replaceChild(LPDISPATCH newChild, LPDISPATCH oldChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_DISPATCH;
	InvokeHelper(0xe, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, oldChild);
	return result;
}

LPDISPATCH CIXMLDOMText::removeChild(LPDISPATCH childNode)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xf, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		childNode);
	return result;
}

LPDISPATCH CIXMLDOMText::appendChild(LPDISPATCH newChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x10, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild);
	return result;
}

BOOL CIXMLDOMText::hasChildNodes()
{
	BOOL result;
	InvokeHelper(0x11, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMText::GetOwnerDocument()
{
	LPDISPATCH result;
	InvokeHelper(0x12, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMText::cloneNode(BOOL deep)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x13, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		deep);
	return result;
}

CString CIXMLDOMText::GetNodeTypeString()
{
	CString result;
	InvokeHelper(0x15, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMText::GetText()
{
	CString result;
	InvokeHelper(0x18, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMText::SetText(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x18, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

BOOL CIXMLDOMText::GetSpecified()
{
	BOOL result;
	InvokeHelper(0x16, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMText::GetDefinition()
{
	LPDISPATCH result;
	InvokeHelper(0x17, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMText::GetNodeTypedValue()
{
	VARIANT result;
	InvokeHelper(0x19, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMText::SetNodeTypedValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x19, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIXMLDOMText::GetDataType()
{
	VARIANT result;
	InvokeHelper(0x1a, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMText::SetDataType(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1a, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIXMLDOMText::GetXml()
{
	CString result;
	InvokeHelper(0x1b, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMText::transformNode(LPDISPATCH stylesheet)
{
	CString result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x1c, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		stylesheet);
	return result;
}

LPDISPATCH CIXMLDOMText::selectNodes(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1d, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

LPDISPATCH CIXMLDOMText::selectSingleNode(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1e, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

BOOL CIXMLDOMText::GetParsed()
{
	BOOL result;
	InvokeHelper(0x1f, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMText::GetNamespaceURI()
{
	CString result;
	InvokeHelper(0x20, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMText::GetPrefix()
{
	CString result;
	InvokeHelper(0x21, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMText::GetBaseName()
{
	CString result;
	InvokeHelper(0x22, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMText::transformNodeToObject(LPDISPATCH stylesheet, const VARIANT& outputObject)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0x23, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 stylesheet, &outputObject);
}

CString CIXMLDOMText::GetData()
{
	CString result;
	InvokeHelper(0x6d, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMText::SetData(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x6d, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

long CIXMLDOMText::GetLength()
{
	long result;
	InvokeHelper(0x6e, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMText::substringData(long offset, long count)
{
	CString result;
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0x6f, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		offset, count);
	return result;
}

void CIXMLDOMText::appendData(LPCTSTR data)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x70, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 data);
}

void CIXMLDOMText::insertData(long offset, LPCTSTR data)
{
	static BYTE parms[] =
		VTS_I4 VTS_BSTR;
	InvokeHelper(0x71, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 offset, data);
}

void CIXMLDOMText::deleteData(long offset, long count)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0x72, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 offset, count);
}

void CIXMLDOMText::replaceData(long offset, long count, LPCTSTR data)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4 VTS_BSTR;
	InvokeHelper(0x73, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 offset, count, data);
}

LPDISPATCH CIXMLDOMText::splitText(long offset)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x7b, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		offset);
	return result;
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMCharacterData properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMCharacterData operations

CString CIXMLDOMCharacterData::GetNodeName()
{
	CString result;
	InvokeHelper(0x2, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMCharacterData::GetNodeValue()
{
	VARIANT result;
	InvokeHelper(0x3, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMCharacterData::SetNodeValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x3, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

long CIXMLDOMCharacterData::GetNodeType()
{
	long result;
	InvokeHelper(0x4, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCharacterData::GetParentNode()
{
	LPDISPATCH result;
	InvokeHelper(0x6, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCharacterData::GetChildNodes()
{
	LPDISPATCH result;
	InvokeHelper(0x7, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCharacterData::GetFirstChild()
{
	LPDISPATCH result;
	InvokeHelper(0x8, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCharacterData::GetLastChild()
{
	LPDISPATCH result;
	InvokeHelper(0x9, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCharacterData::GetPreviousSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCharacterData::GetNextSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xb, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCharacterData::GetAttributes()
{
	LPDISPATCH result;
	InvokeHelper(0xc, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCharacterData::insertBefore(LPDISPATCH newChild, const VARIANT& refChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0xd, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, &refChild);
	return result;
}

LPDISPATCH CIXMLDOMCharacterData::replaceChild(LPDISPATCH newChild, LPDISPATCH oldChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_DISPATCH;
	InvokeHelper(0xe, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, oldChild);
	return result;
}

LPDISPATCH CIXMLDOMCharacterData::removeChild(LPDISPATCH childNode)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xf, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		childNode);
	return result;
}

LPDISPATCH CIXMLDOMCharacterData::appendChild(LPDISPATCH newChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x10, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild);
	return result;
}

BOOL CIXMLDOMCharacterData::hasChildNodes()
{
	BOOL result;
	InvokeHelper(0x11, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCharacterData::GetOwnerDocument()
{
	LPDISPATCH result;
	InvokeHelper(0x12, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCharacterData::cloneNode(BOOL deep)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x13, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		deep);
	return result;
}

CString CIXMLDOMCharacterData::GetNodeTypeString()
{
	CString result;
	InvokeHelper(0x15, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMCharacterData::GetText()
{
	CString result;
	InvokeHelper(0x18, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMCharacterData::SetText(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x18, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

BOOL CIXMLDOMCharacterData::GetSpecified()
{
	BOOL result;
	InvokeHelper(0x16, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCharacterData::GetDefinition()
{
	LPDISPATCH result;
	InvokeHelper(0x17, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMCharacterData::GetNodeTypedValue()
{
	VARIANT result;
	InvokeHelper(0x19, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMCharacterData::SetNodeTypedValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x19, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIXMLDOMCharacterData::GetDataType()
{
	VARIANT result;
	InvokeHelper(0x1a, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMCharacterData::SetDataType(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1a, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIXMLDOMCharacterData::GetXml()
{
	CString result;
	InvokeHelper(0x1b, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMCharacterData::transformNode(LPDISPATCH stylesheet)
{
	CString result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x1c, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		stylesheet);
	return result;
}

LPDISPATCH CIXMLDOMCharacterData::selectNodes(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1d, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

LPDISPATCH CIXMLDOMCharacterData::selectSingleNode(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1e, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

BOOL CIXMLDOMCharacterData::GetParsed()
{
	BOOL result;
	InvokeHelper(0x1f, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMCharacterData::GetNamespaceURI()
{
	CString result;
	InvokeHelper(0x20, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMCharacterData::GetPrefix()
{
	CString result;
	InvokeHelper(0x21, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMCharacterData::GetBaseName()
{
	CString result;
	InvokeHelper(0x22, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMCharacterData::transformNodeToObject(LPDISPATCH stylesheet, const VARIANT& outputObject)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0x23, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 stylesheet, &outputObject);
}

CString CIXMLDOMCharacterData::GetData()
{
	CString result;
	InvokeHelper(0x6d, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMCharacterData::SetData(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x6d, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

long CIXMLDOMCharacterData::GetLength()
{
	long result;
	InvokeHelper(0x6e, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMCharacterData::substringData(long offset, long count)
{
	CString result;
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0x6f, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		offset, count);
	return result;
}

void CIXMLDOMCharacterData::appendData(LPCTSTR data)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x70, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 data);
}

void CIXMLDOMCharacterData::insertData(long offset, LPCTSTR data)
{
	static BYTE parms[] =
		VTS_I4 VTS_BSTR;
	InvokeHelper(0x71, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 offset, data);
}

void CIXMLDOMCharacterData::deleteData(long offset, long count)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0x72, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 offset, count);
}

void CIXMLDOMCharacterData::replaceData(long offset, long count, LPCTSTR data)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4 VTS_BSTR;
	InvokeHelper(0x73, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 offset, count, data);
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMComment properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMComment operations

CString CIXMLDOMComment::GetNodeName()
{
	CString result;
	InvokeHelper(0x2, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMComment::GetNodeValue()
{
	VARIANT result;
	InvokeHelper(0x3, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMComment::SetNodeValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x3, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

long CIXMLDOMComment::GetNodeType()
{
	long result;
	InvokeHelper(0x4, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMComment::GetParentNode()
{
	LPDISPATCH result;
	InvokeHelper(0x6, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMComment::GetChildNodes()
{
	LPDISPATCH result;
	InvokeHelper(0x7, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMComment::GetFirstChild()
{
	LPDISPATCH result;
	InvokeHelper(0x8, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMComment::GetLastChild()
{
	LPDISPATCH result;
	InvokeHelper(0x9, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMComment::GetPreviousSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMComment::GetNextSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xb, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMComment::GetAttributes()
{
	LPDISPATCH result;
	InvokeHelper(0xc, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMComment::insertBefore(LPDISPATCH newChild, const VARIANT& refChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0xd, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, &refChild);
	return result;
}

LPDISPATCH CIXMLDOMComment::replaceChild(LPDISPATCH newChild, LPDISPATCH oldChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_DISPATCH;
	InvokeHelper(0xe, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, oldChild);
	return result;
}

LPDISPATCH CIXMLDOMComment::removeChild(LPDISPATCH childNode)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xf, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		childNode);
	return result;
}

LPDISPATCH CIXMLDOMComment::appendChild(LPDISPATCH newChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x10, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild);
	return result;
}

BOOL CIXMLDOMComment::hasChildNodes()
{
	BOOL result;
	InvokeHelper(0x11, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMComment::GetOwnerDocument()
{
	LPDISPATCH result;
	InvokeHelper(0x12, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMComment::cloneNode(BOOL deep)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x13, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		deep);
	return result;
}

CString CIXMLDOMComment::GetNodeTypeString()
{
	CString result;
	InvokeHelper(0x15, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMComment::GetText()
{
	CString result;
	InvokeHelper(0x18, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMComment::SetText(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x18, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

BOOL CIXMLDOMComment::GetSpecified()
{
	BOOL result;
	InvokeHelper(0x16, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMComment::GetDefinition()
{
	LPDISPATCH result;
	InvokeHelper(0x17, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMComment::GetNodeTypedValue()
{
	VARIANT result;
	InvokeHelper(0x19, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMComment::SetNodeTypedValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x19, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIXMLDOMComment::GetDataType()
{
	VARIANT result;
	InvokeHelper(0x1a, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMComment::SetDataType(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1a, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIXMLDOMComment::GetXml()
{
	CString result;
	InvokeHelper(0x1b, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMComment::transformNode(LPDISPATCH stylesheet)
{
	CString result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x1c, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		stylesheet);
	return result;
}

LPDISPATCH CIXMLDOMComment::selectNodes(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1d, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

LPDISPATCH CIXMLDOMComment::selectSingleNode(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1e, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

BOOL CIXMLDOMComment::GetParsed()
{
	BOOL result;
	InvokeHelper(0x1f, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMComment::GetNamespaceURI()
{
	CString result;
	InvokeHelper(0x20, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMComment::GetPrefix()
{
	CString result;
	InvokeHelper(0x21, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMComment::GetBaseName()
{
	CString result;
	InvokeHelper(0x22, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMComment::transformNodeToObject(LPDISPATCH stylesheet, const VARIANT& outputObject)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0x23, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 stylesheet, &outputObject);
}

CString CIXMLDOMComment::GetData()
{
	CString result;
	InvokeHelper(0x6d, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMComment::SetData(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x6d, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

long CIXMLDOMComment::GetLength()
{
	long result;
	InvokeHelper(0x6e, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMComment::substringData(long offset, long count)
{
	CString result;
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0x6f, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		offset, count);
	return result;
}

void CIXMLDOMComment::appendData(LPCTSTR data)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x70, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 data);
}

void CIXMLDOMComment::insertData(long offset, LPCTSTR data)
{
	static BYTE parms[] =
		VTS_I4 VTS_BSTR;
	InvokeHelper(0x71, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 offset, data);
}

void CIXMLDOMComment::deleteData(long offset, long count)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0x72, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 offset, count);
}

void CIXMLDOMComment::replaceData(long offset, long count, LPCTSTR data)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4 VTS_BSTR;
	InvokeHelper(0x73, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 offset, count, data);
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMCDATASection properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMCDATASection operations

CString CIXMLDOMCDATASection::GetNodeName()
{
	CString result;
	InvokeHelper(0x2, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMCDATASection::GetNodeValue()
{
	VARIANT result;
	InvokeHelper(0x3, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMCDATASection::SetNodeValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x3, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

long CIXMLDOMCDATASection::GetNodeType()
{
	long result;
	InvokeHelper(0x4, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCDATASection::GetParentNode()
{
	LPDISPATCH result;
	InvokeHelper(0x6, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCDATASection::GetChildNodes()
{
	LPDISPATCH result;
	InvokeHelper(0x7, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCDATASection::GetFirstChild()
{
	LPDISPATCH result;
	InvokeHelper(0x8, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCDATASection::GetLastChild()
{
	LPDISPATCH result;
	InvokeHelper(0x9, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCDATASection::GetPreviousSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCDATASection::GetNextSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xb, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCDATASection::GetAttributes()
{
	LPDISPATCH result;
	InvokeHelper(0xc, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCDATASection::insertBefore(LPDISPATCH newChild, const VARIANT& refChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0xd, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, &refChild);
	return result;
}

LPDISPATCH CIXMLDOMCDATASection::replaceChild(LPDISPATCH newChild, LPDISPATCH oldChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_DISPATCH;
	InvokeHelper(0xe, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, oldChild);
	return result;
}

LPDISPATCH CIXMLDOMCDATASection::removeChild(LPDISPATCH childNode)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xf, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		childNode);
	return result;
}

LPDISPATCH CIXMLDOMCDATASection::appendChild(LPDISPATCH newChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x10, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild);
	return result;
}

BOOL CIXMLDOMCDATASection::hasChildNodes()
{
	BOOL result;
	InvokeHelper(0x11, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCDATASection::GetOwnerDocument()
{
	LPDISPATCH result;
	InvokeHelper(0x12, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCDATASection::cloneNode(BOOL deep)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x13, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		deep);
	return result;
}

CString CIXMLDOMCDATASection::GetNodeTypeString()
{
	CString result;
	InvokeHelper(0x15, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMCDATASection::GetText()
{
	CString result;
	InvokeHelper(0x18, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMCDATASection::SetText(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x18, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

BOOL CIXMLDOMCDATASection::GetSpecified()
{
	BOOL result;
	InvokeHelper(0x16, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMCDATASection::GetDefinition()
{
	LPDISPATCH result;
	InvokeHelper(0x17, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMCDATASection::GetNodeTypedValue()
{
	VARIANT result;
	InvokeHelper(0x19, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMCDATASection::SetNodeTypedValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x19, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIXMLDOMCDATASection::GetDataType()
{
	VARIANT result;
	InvokeHelper(0x1a, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMCDATASection::SetDataType(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1a, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIXMLDOMCDATASection::GetXml()
{
	CString result;
	InvokeHelper(0x1b, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMCDATASection::transformNode(LPDISPATCH stylesheet)
{
	CString result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x1c, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		stylesheet);
	return result;
}

LPDISPATCH CIXMLDOMCDATASection::selectNodes(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1d, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

LPDISPATCH CIXMLDOMCDATASection::selectSingleNode(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1e, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

BOOL CIXMLDOMCDATASection::GetParsed()
{
	BOOL result;
	InvokeHelper(0x1f, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMCDATASection::GetNamespaceURI()
{
	CString result;
	InvokeHelper(0x20, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMCDATASection::GetPrefix()
{
	CString result;
	InvokeHelper(0x21, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMCDATASection::GetBaseName()
{
	CString result;
	InvokeHelper(0x22, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMCDATASection::transformNodeToObject(LPDISPATCH stylesheet, const VARIANT& outputObject)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0x23, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 stylesheet, &outputObject);
}

CString CIXMLDOMCDATASection::GetData()
{
	CString result;
	InvokeHelper(0x6d, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMCDATASection::SetData(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x6d, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

long CIXMLDOMCDATASection::GetLength()
{
	long result;
	InvokeHelper(0x6e, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMCDATASection::substringData(long offset, long count)
{
	CString result;
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0x6f, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		offset, count);
	return result;
}

void CIXMLDOMCDATASection::appendData(LPCTSTR data)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x70, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 data);
}

void CIXMLDOMCDATASection::insertData(long offset, LPCTSTR data)
{
	static BYTE parms[] =
		VTS_I4 VTS_BSTR;
	InvokeHelper(0x71, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 offset, data);
}

void CIXMLDOMCDATASection::deleteData(long offset, long count)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0x72, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 offset, count);
}

void CIXMLDOMCDATASection::replaceData(long offset, long count, LPCTSTR data)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4 VTS_BSTR;
	InvokeHelper(0x73, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 offset, count, data);
}

LPDISPATCH CIXMLDOMCDATASection::splitText(long offset)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x7b, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		offset);
	return result;
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMProcessingInstruction properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMProcessingInstruction operations

CString CIXMLDOMProcessingInstruction::GetNodeName()
{
	CString result;
	InvokeHelper(0x2, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMProcessingInstruction::GetNodeValue()
{
	VARIANT result;
	InvokeHelper(0x3, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMProcessingInstruction::SetNodeValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x3, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

long CIXMLDOMProcessingInstruction::GetNodeType()
{
	long result;
	InvokeHelper(0x4, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMProcessingInstruction::GetParentNode()
{
	LPDISPATCH result;
	InvokeHelper(0x6, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMProcessingInstruction::GetChildNodes()
{
	LPDISPATCH result;
	InvokeHelper(0x7, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMProcessingInstruction::GetFirstChild()
{
	LPDISPATCH result;
	InvokeHelper(0x8, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMProcessingInstruction::GetLastChild()
{
	LPDISPATCH result;
	InvokeHelper(0x9, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMProcessingInstruction::GetPreviousSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMProcessingInstruction::GetNextSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xb, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMProcessingInstruction::GetAttributes()
{
	LPDISPATCH result;
	InvokeHelper(0xc, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMProcessingInstruction::insertBefore(LPDISPATCH newChild, const VARIANT& refChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0xd, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, &refChild);
	return result;
}

LPDISPATCH CIXMLDOMProcessingInstruction::replaceChild(LPDISPATCH newChild, LPDISPATCH oldChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_DISPATCH;
	InvokeHelper(0xe, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, oldChild);
	return result;
}

LPDISPATCH CIXMLDOMProcessingInstruction::removeChild(LPDISPATCH childNode)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xf, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		childNode);
	return result;
}

LPDISPATCH CIXMLDOMProcessingInstruction::appendChild(LPDISPATCH newChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x10, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild);
	return result;
}

BOOL CIXMLDOMProcessingInstruction::hasChildNodes()
{
	BOOL result;
	InvokeHelper(0x11, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMProcessingInstruction::GetOwnerDocument()
{
	LPDISPATCH result;
	InvokeHelper(0x12, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMProcessingInstruction::cloneNode(BOOL deep)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x13, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		deep);
	return result;
}

CString CIXMLDOMProcessingInstruction::GetNodeTypeString()
{
	CString result;
	InvokeHelper(0x15, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMProcessingInstruction::GetText()
{
	CString result;
	InvokeHelper(0x18, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMProcessingInstruction::SetText(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x18, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

BOOL CIXMLDOMProcessingInstruction::GetSpecified()
{
	BOOL result;
	InvokeHelper(0x16, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMProcessingInstruction::GetDefinition()
{
	LPDISPATCH result;
	InvokeHelper(0x17, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMProcessingInstruction::GetNodeTypedValue()
{
	VARIANT result;
	InvokeHelper(0x19, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMProcessingInstruction::SetNodeTypedValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x19, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIXMLDOMProcessingInstruction::GetDataType()
{
	VARIANT result;
	InvokeHelper(0x1a, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMProcessingInstruction::SetDataType(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1a, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIXMLDOMProcessingInstruction::GetXml()
{
	CString result;
	InvokeHelper(0x1b, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMProcessingInstruction::transformNode(LPDISPATCH stylesheet)
{
	CString result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x1c, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		stylesheet);
	return result;
}

LPDISPATCH CIXMLDOMProcessingInstruction::selectNodes(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1d, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

LPDISPATCH CIXMLDOMProcessingInstruction::selectSingleNode(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1e, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

BOOL CIXMLDOMProcessingInstruction::GetParsed()
{
	BOOL result;
	InvokeHelper(0x1f, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMProcessingInstruction::GetNamespaceURI()
{
	CString result;
	InvokeHelper(0x20, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMProcessingInstruction::GetPrefix()
{
	CString result;
	InvokeHelper(0x21, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMProcessingInstruction::GetBaseName()
{
	CString result;
	InvokeHelper(0x22, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMProcessingInstruction::transformNodeToObject(LPDISPATCH stylesheet, const VARIANT& outputObject)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0x23, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 stylesheet, &outputObject);
}

CString CIXMLDOMProcessingInstruction::GetTarget()
{
	CString result;
	InvokeHelper(0x7f, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMProcessingInstruction::GetData()
{
	CString result;
	InvokeHelper(0x80, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMProcessingInstruction::SetData(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x80, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMEntityReference properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMEntityReference operations

CString CIXMLDOMEntityReference::GetNodeName()
{
	CString result;
	InvokeHelper(0x2, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMEntityReference::GetNodeValue()
{
	VARIANT result;
	InvokeHelper(0x3, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMEntityReference::SetNodeValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x3, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

long CIXMLDOMEntityReference::GetNodeType()
{
	long result;
	InvokeHelper(0x4, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntityReference::GetParentNode()
{
	LPDISPATCH result;
	InvokeHelper(0x6, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntityReference::GetChildNodes()
{
	LPDISPATCH result;
	InvokeHelper(0x7, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntityReference::GetFirstChild()
{
	LPDISPATCH result;
	InvokeHelper(0x8, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntityReference::GetLastChild()
{
	LPDISPATCH result;
	InvokeHelper(0x9, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntityReference::GetPreviousSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntityReference::GetNextSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xb, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntityReference::GetAttributes()
{
	LPDISPATCH result;
	InvokeHelper(0xc, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntityReference::insertBefore(LPDISPATCH newChild, const VARIANT& refChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0xd, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, &refChild);
	return result;
}

LPDISPATCH CIXMLDOMEntityReference::replaceChild(LPDISPATCH newChild, LPDISPATCH oldChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_DISPATCH;
	InvokeHelper(0xe, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, oldChild);
	return result;
}

LPDISPATCH CIXMLDOMEntityReference::removeChild(LPDISPATCH childNode)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xf, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		childNode);
	return result;
}

LPDISPATCH CIXMLDOMEntityReference::appendChild(LPDISPATCH newChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x10, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild);
	return result;
}

BOOL CIXMLDOMEntityReference::hasChildNodes()
{
	BOOL result;
	InvokeHelper(0x11, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntityReference::GetOwnerDocument()
{
	LPDISPATCH result;
	InvokeHelper(0x12, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntityReference::cloneNode(BOOL deep)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x13, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		deep);
	return result;
}

CString CIXMLDOMEntityReference::GetNodeTypeString()
{
	CString result;
	InvokeHelper(0x15, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMEntityReference::GetText()
{
	CString result;
	InvokeHelper(0x18, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMEntityReference::SetText(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x18, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

BOOL CIXMLDOMEntityReference::GetSpecified()
{
	BOOL result;
	InvokeHelper(0x16, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntityReference::GetDefinition()
{
	LPDISPATCH result;
	InvokeHelper(0x17, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMEntityReference::GetNodeTypedValue()
{
	VARIANT result;
	InvokeHelper(0x19, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMEntityReference::SetNodeTypedValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x19, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIXMLDOMEntityReference::GetDataType()
{
	VARIANT result;
	InvokeHelper(0x1a, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMEntityReference::SetDataType(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1a, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIXMLDOMEntityReference::GetXml()
{
	CString result;
	InvokeHelper(0x1b, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMEntityReference::transformNode(LPDISPATCH stylesheet)
{
	CString result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x1c, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		stylesheet);
	return result;
}

LPDISPATCH CIXMLDOMEntityReference::selectNodes(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1d, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

LPDISPATCH CIXMLDOMEntityReference::selectSingleNode(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1e, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

BOOL CIXMLDOMEntityReference::GetParsed()
{
	BOOL result;
	InvokeHelper(0x1f, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMEntityReference::GetNamespaceURI()
{
	CString result;
	InvokeHelper(0x20, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMEntityReference::GetPrefix()
{
	CString result;
	InvokeHelper(0x21, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMEntityReference::GetBaseName()
{
	CString result;
	InvokeHelper(0x22, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMEntityReference::transformNodeToObject(LPDISPATCH stylesheet, const VARIANT& outputObject)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0x23, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 stylesheet, &outputObject);
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMParseError properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMParseError operations

long CIXMLDOMParseError::GetErrorCode()
{
	long result;
	InvokeHelper(0x0, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMParseError::GetUrl()
{
	CString result;
	InvokeHelper(0xb3, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMParseError::GetReason()
{
	CString result;
	InvokeHelper(0xb4, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMParseError::GetSrcText()
{
	CString result;
	InvokeHelper(0xb5, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

long CIXMLDOMParseError::GetLine()
{
	long result;
	InvokeHelper(0xb6, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

long CIXMLDOMParseError::GetLinepos()
{
	long result;
	InvokeHelper(0xb7, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

long CIXMLDOMParseError::GetFilepos()
{
	long result;
	InvokeHelper(0xb8, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMSchemaCollection properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMSchemaCollection operations

void CIXMLDOMSchemaCollection::add(LPCTSTR namespaceURI, const VARIANT& var)
{
	static BYTE parms[] =
		VTS_BSTR VTS_VARIANT;
	InvokeHelper(0x3, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 namespaceURI, &var);
}

LPDISPATCH CIXMLDOMSchemaCollection::get(LPCTSTR namespaceURI)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x4, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		namespaceURI);
	return result;
}

void CIXMLDOMSchemaCollection::remove(LPCTSTR namespaceURI)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x5, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 namespaceURI);
}

long CIXMLDOMSchemaCollection::GetLength()
{
	long result;
	InvokeHelper(0x6, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMSchemaCollection::GetNamespaceURI(long index)
{
	CString result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x0, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, parms,
		index);
	return result;
}

void CIXMLDOMSchemaCollection::addCollection(LPDISPATCH otherCollection)
{
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x8, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 otherCollection);
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMDocument2 properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMDocument2 operations

CString CIXMLDOMDocument2::GetNodeName()
{
	CString result;
	InvokeHelper(0x2, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMDocument2::GetNodeValue()
{
	VARIANT result;
	InvokeHelper(0x3, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument2::SetNodeValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x3, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

long CIXMLDOMDocument2::GetNodeType()
{
	long result;
	InvokeHelper(0x4, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::GetParentNode()
{
	LPDISPATCH result;
	InvokeHelper(0x6, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::GetChildNodes()
{
	LPDISPATCH result;
	InvokeHelper(0x7, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::GetFirstChild()
{
	LPDISPATCH result;
	InvokeHelper(0x8, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::GetLastChild()
{
	LPDISPATCH result;
	InvokeHelper(0x9, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::GetPreviousSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::GetNextSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xb, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::GetAttributes()
{
	LPDISPATCH result;
	InvokeHelper(0xc, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::insertBefore(LPDISPATCH newChild, const VARIANT& refChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0xd, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, &refChild);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::replaceChild(LPDISPATCH newChild, LPDISPATCH oldChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_DISPATCH;
	InvokeHelper(0xe, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, oldChild);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::removeChild(LPDISPATCH childNode)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xf, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		childNode);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::appendChild(LPDISPATCH newChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x10, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild);
	return result;
}

BOOL CIXMLDOMDocument2::hasChildNodes()
{
	BOOL result;
	InvokeHelper(0x11, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::GetOwnerDocument()
{
	LPDISPATCH result;
	InvokeHelper(0x12, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::cloneNode(BOOL deep)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x13, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		deep);
	return result;
}

CString CIXMLDOMDocument2::GetNodeTypeString()
{
	CString result;
	InvokeHelper(0x15, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocument2::GetText()
{
	CString result;
	InvokeHelper(0x18, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument2::SetText(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x18, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

BOOL CIXMLDOMDocument2::GetSpecified()
{
	BOOL result;
	InvokeHelper(0x16, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::GetDefinition()
{
	LPDISPATCH result;
	InvokeHelper(0x17, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMDocument2::GetNodeTypedValue()
{
	VARIANT result;
	InvokeHelper(0x19, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument2::SetNodeTypedValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x19, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIXMLDOMDocument2::GetDataType()
{
	VARIANT result;
	InvokeHelper(0x1a, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument2::SetDataType(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1a, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIXMLDOMDocument2::GetXml()
{
	CString result;
	InvokeHelper(0x1b, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocument2::transformNode(LPDISPATCH stylesheet)
{
	CString result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x1c, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		stylesheet);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::selectNodes(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1d, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::selectSingleNode(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1e, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

BOOL CIXMLDOMDocument2::GetParsed()
{
	BOOL result;
	InvokeHelper(0x1f, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocument2::GetNamespaceURI()
{
	CString result;
	InvokeHelper(0x20, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocument2::GetPrefix()
{
	CString result;
	InvokeHelper(0x21, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocument2::GetBaseName()
{
	CString result;
	InvokeHelper(0x22, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument2::transformNodeToObject(LPDISPATCH stylesheet, const VARIANT& outputObject)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0x23, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 stylesheet, &outputObject);
}

LPDISPATCH CIXMLDOMDocument2::GetDoctype()
{
	LPDISPATCH result;
	InvokeHelper(0x26, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::GetImplementation()
{
	LPDISPATCH result;
	InvokeHelper(0x27, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::GetDocumentElement()
{
	LPDISPATCH result;
	InvokeHelper(0x28, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument2::SetRefDocumentElement(LPDISPATCH newValue)
{
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x28, DISPATCH_PROPERTYPUTREF, VT_EMPTY, NULL, parms,
		 newValue);
}

LPDISPATCH CIXMLDOMDocument2::createElement(LPCTSTR tagName)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x29, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		tagName);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::createDocumentFragment()
{
	LPDISPATCH result;
	InvokeHelper(0x2a, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::createTextNode(LPCTSTR data)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x2b, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		data);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::createComment(LPCTSTR data)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x2c, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		data);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::createCDATASection(LPCTSTR data)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x2d, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		data);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::createProcessingInstruction(LPCTSTR target, LPCTSTR data)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR;
	InvokeHelper(0x2e, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		target, data);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::createAttribute(LPCTSTR name)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x2f, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		name);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::createEntityReference(LPCTSTR name)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x31, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		name);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::getElementsByTagName(LPCTSTR tagName)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x32, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		tagName);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::createNode(const VARIANT& type, LPCTSTR name, LPCTSTR namespaceURI)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_VARIANT VTS_BSTR VTS_BSTR;
	InvokeHelper(0x36, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		&type, name, namespaceURI);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::nodeFromID(LPCTSTR idString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x38, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		idString);
	return result;
}

BOOL CIXMLDOMDocument2::load(const VARIANT& xmlSource)
{
	BOOL result;
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x3a, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		&xmlSource);
	return result;
}

long CIXMLDOMDocument2::GetReadyState()
{
	long result;
	InvokeHelper(DISPID_READYSTATE, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMDocument2::GetParseError()
{
	LPDISPATCH result;
	InvokeHelper(0x3b, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMDocument2::GetUrl()
{
	CString result;
	InvokeHelper(0x3c, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

BOOL CIXMLDOMDocument2::GetAsync()
{
	BOOL result;
	InvokeHelper(0x3d, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument2::SetAsync(BOOL bNewValue)
{
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x3d, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 bNewValue);
}

void CIXMLDOMDocument2::abort()
{
	InvokeHelper(0x3e, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

BOOL CIXMLDOMDocument2::loadXML(LPCTSTR bstrXML)
{
	BOOL result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x3f, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		bstrXML);
	return result;
}

void CIXMLDOMDocument2::save(const VARIANT& destination)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x40, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 &destination);
}

BOOL CIXMLDOMDocument2::GetValidateOnParse()
{
	BOOL result;
	InvokeHelper(0x41, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument2::SetValidateOnParse(BOOL bNewValue)
{
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x41, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 bNewValue);
}

BOOL CIXMLDOMDocument2::GetResolveExternals()
{
	BOOL result;
	InvokeHelper(0x42, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument2::SetResolveExternals(BOOL bNewValue)
{
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x42, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 bNewValue);
}

BOOL CIXMLDOMDocument2::GetPreserveWhiteSpace()
{
	BOOL result;
	InvokeHelper(0x43, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument2::SetPreserveWhiteSpace(BOOL bNewValue)
{
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x43, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 bNewValue);
}

void CIXMLDOMDocument2::SetOnreadystatechange(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x44, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

void CIXMLDOMDocument2::SetOndataavailable(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x45, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

void CIXMLDOMDocument2::SetOntransformnode(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x46, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

LPDISPATCH CIXMLDOMDocument2::GetNamespaces()
{
	LPDISPATCH result;
	InvokeHelper(0xc9, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMDocument2::GetSchemas()
{
	VARIANT result;
	InvokeHelper(0xca, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument2::SetRefSchemas(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0xca, DISPATCH_PROPERTYPUTREF, VT_EMPTY, NULL, parms,
		 &newValue);
}

LPDISPATCH CIXMLDOMDocument2::validate()
{
	LPDISPATCH result;
	InvokeHelper(0xcb, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void CIXMLDOMDocument2::setProperty(LPCTSTR name, const VARIANT& value)
{
	static BYTE parms[] =
		VTS_BSTR VTS_VARIANT;
	InvokeHelper(0xcc, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 name, &value);
}

VARIANT CIXMLDOMDocument2::getProperty(LPCTSTR name)
{
	VARIANT result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0xcd, DISPATCH_METHOD, VT_VARIANT, (void*)&result, parms,
		name);
	return result;
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMNotation properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMNotation operations

CString CIXMLDOMNotation::GetNodeName()
{
	CString result;
	InvokeHelper(0x2, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMNotation::GetNodeValue()
{
	VARIANT result;
	InvokeHelper(0x3, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMNotation::SetNodeValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x3, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

long CIXMLDOMNotation::GetNodeType()
{
	long result;
	InvokeHelper(0x4, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNotation::GetParentNode()
{
	LPDISPATCH result;
	InvokeHelper(0x6, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNotation::GetChildNodes()
{
	LPDISPATCH result;
	InvokeHelper(0x7, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNotation::GetFirstChild()
{
	LPDISPATCH result;
	InvokeHelper(0x8, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNotation::GetLastChild()
{
	LPDISPATCH result;
	InvokeHelper(0x9, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNotation::GetPreviousSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNotation::GetNextSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xb, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNotation::GetAttributes()
{
	LPDISPATCH result;
	InvokeHelper(0xc, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNotation::insertBefore(LPDISPATCH newChild, const VARIANT& refChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0xd, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, &refChild);
	return result;
}

LPDISPATCH CIXMLDOMNotation::replaceChild(LPDISPATCH newChild, LPDISPATCH oldChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_DISPATCH;
	InvokeHelper(0xe, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, oldChild);
	return result;
}

LPDISPATCH CIXMLDOMNotation::removeChild(LPDISPATCH childNode)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xf, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		childNode);
	return result;
}

LPDISPATCH CIXMLDOMNotation::appendChild(LPDISPATCH newChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x10, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild);
	return result;
}

BOOL CIXMLDOMNotation::hasChildNodes()
{
	BOOL result;
	InvokeHelper(0x11, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNotation::GetOwnerDocument()
{
	LPDISPATCH result;
	InvokeHelper(0x12, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNotation::cloneNode(BOOL deep)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x13, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		deep);
	return result;
}

CString CIXMLDOMNotation::GetNodeTypeString()
{
	CString result;
	InvokeHelper(0x15, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMNotation::GetText()
{
	CString result;
	InvokeHelper(0x18, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMNotation::SetText(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x18, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

BOOL CIXMLDOMNotation::GetSpecified()
{
	BOOL result;
	InvokeHelper(0x16, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMNotation::GetDefinition()
{
	LPDISPATCH result;
	InvokeHelper(0x17, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMNotation::GetNodeTypedValue()
{
	VARIANT result;
	InvokeHelper(0x19, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMNotation::SetNodeTypedValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x19, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIXMLDOMNotation::GetDataType()
{
	VARIANT result;
	InvokeHelper(0x1a, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMNotation::SetDataType(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1a, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIXMLDOMNotation::GetXml()
{
	CString result;
	InvokeHelper(0x1b, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMNotation::transformNode(LPDISPATCH stylesheet)
{
	CString result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x1c, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		stylesheet);
	return result;
}

LPDISPATCH CIXMLDOMNotation::selectNodes(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1d, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

LPDISPATCH CIXMLDOMNotation::selectSingleNode(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1e, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

BOOL CIXMLDOMNotation::GetParsed()
{
	BOOL result;
	InvokeHelper(0x1f, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMNotation::GetNamespaceURI()
{
	CString result;
	InvokeHelper(0x20, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMNotation::GetPrefix()
{
	CString result;
	InvokeHelper(0x21, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMNotation::GetBaseName()
{
	CString result;
	InvokeHelper(0x22, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMNotation::transformNodeToObject(LPDISPATCH stylesheet, const VARIANT& outputObject)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0x23, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 stylesheet, &outputObject);
}

VARIANT CIXMLDOMNotation::GetPublicId()
{
	VARIANT result;
	InvokeHelper(0x88, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMNotation::GetSystemId()
{
	VARIANT result;
	InvokeHelper(0x89, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMEntity properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMEntity operations

CString CIXMLDOMEntity::GetNodeName()
{
	CString result;
	InvokeHelper(0x2, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMEntity::GetNodeValue()
{
	VARIANT result;
	InvokeHelper(0x3, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMEntity::SetNodeValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x3, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

long CIXMLDOMEntity::GetNodeType()
{
	long result;
	InvokeHelper(0x4, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntity::GetParentNode()
{
	LPDISPATCH result;
	InvokeHelper(0x6, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntity::GetChildNodes()
{
	LPDISPATCH result;
	InvokeHelper(0x7, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntity::GetFirstChild()
{
	LPDISPATCH result;
	InvokeHelper(0x8, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntity::GetLastChild()
{
	LPDISPATCH result;
	InvokeHelper(0x9, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntity::GetPreviousSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntity::GetNextSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xb, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntity::GetAttributes()
{
	LPDISPATCH result;
	InvokeHelper(0xc, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntity::insertBefore(LPDISPATCH newChild, const VARIANT& refChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0xd, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, &refChild);
	return result;
}

LPDISPATCH CIXMLDOMEntity::replaceChild(LPDISPATCH newChild, LPDISPATCH oldChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_DISPATCH;
	InvokeHelper(0xe, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, oldChild);
	return result;
}

LPDISPATCH CIXMLDOMEntity::removeChild(LPDISPATCH childNode)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xf, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		childNode);
	return result;
}

LPDISPATCH CIXMLDOMEntity::appendChild(LPDISPATCH newChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x10, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild);
	return result;
}

BOOL CIXMLDOMEntity::hasChildNodes()
{
	BOOL result;
	InvokeHelper(0x11, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntity::GetOwnerDocument()
{
	LPDISPATCH result;
	InvokeHelper(0x12, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntity::cloneNode(BOOL deep)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x13, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		deep);
	return result;
}

CString CIXMLDOMEntity::GetNodeTypeString()
{
	CString result;
	InvokeHelper(0x15, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMEntity::GetText()
{
	CString result;
	InvokeHelper(0x18, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMEntity::SetText(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x18, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

BOOL CIXMLDOMEntity::GetSpecified()
{
	BOOL result;
	InvokeHelper(0x16, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMEntity::GetDefinition()
{
	LPDISPATCH result;
	InvokeHelper(0x17, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMEntity::GetNodeTypedValue()
{
	VARIANT result;
	InvokeHelper(0x19, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMEntity::SetNodeTypedValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x19, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIXMLDOMEntity::GetDataType()
{
	VARIANT result;
	InvokeHelper(0x1a, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXMLDOMEntity::SetDataType(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1a, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIXMLDOMEntity::GetXml()
{
	CString result;
	InvokeHelper(0x1b, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMEntity::transformNode(LPDISPATCH stylesheet)
{
	CString result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x1c, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		stylesheet);
	return result;
}

LPDISPATCH CIXMLDOMEntity::selectNodes(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1d, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

LPDISPATCH CIXMLDOMEntity::selectSingleNode(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1e, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

BOOL CIXMLDOMEntity::GetParsed()
{
	BOOL result;
	InvokeHelper(0x1f, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMEntity::GetNamespaceURI()
{
	CString result;
	InvokeHelper(0x20, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMEntity::GetPrefix()
{
	CString result;
	InvokeHelper(0x21, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMEntity::GetBaseName()
{
	CString result;
	InvokeHelper(0x22, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMEntity::transformNodeToObject(LPDISPATCH stylesheet, const VARIANT& outputObject)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0x23, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 stylesheet, &outputObject);
}

VARIANT CIXMLDOMEntity::GetPublicId()
{
	VARIANT result;
	InvokeHelper(0x8c, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMEntity::GetSystemId()
{
	VARIANT result;
	InvokeHelper(0x8d, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

CString CIXMLDOMEntity::GetNotationName()
{
	CString result;
	InvokeHelper(0x8e, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}


/////////////////////////////////////////////////////////////////////////////
// CIXTLRuntime properties

/////////////////////////////////////////////////////////////////////////////
// CIXTLRuntime operations

CString CIXTLRuntime::GetNodeName()
{
	CString result;
	InvokeHelper(0x2, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

VARIANT CIXTLRuntime::GetNodeValue()
{
	VARIANT result;
	InvokeHelper(0x3, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXTLRuntime::SetNodeValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x3, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

long CIXTLRuntime::GetNodeType()
{
	long result;
	InvokeHelper(0x4, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXTLRuntime::GetParentNode()
{
	LPDISPATCH result;
	InvokeHelper(0x6, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXTLRuntime::GetChildNodes()
{
	LPDISPATCH result;
	InvokeHelper(0x7, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXTLRuntime::GetFirstChild()
{
	LPDISPATCH result;
	InvokeHelper(0x8, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXTLRuntime::GetLastChild()
{
	LPDISPATCH result;
	InvokeHelper(0x9, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXTLRuntime::GetPreviousSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXTLRuntime::GetNextSibling()
{
	LPDISPATCH result;
	InvokeHelper(0xb, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXTLRuntime::GetAttributes()
{
	LPDISPATCH result;
	InvokeHelper(0xc, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXTLRuntime::insertBefore(LPDISPATCH newChild, const VARIANT& refChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0xd, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, &refChild);
	return result;
}

LPDISPATCH CIXTLRuntime::replaceChild(LPDISPATCH newChild, LPDISPATCH oldChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_DISPATCH;
	InvokeHelper(0xe, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild, oldChild);
	return result;
}

LPDISPATCH CIXTLRuntime::removeChild(LPDISPATCH childNode)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xf, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		childNode);
	return result;
}

LPDISPATCH CIXTLRuntime::appendChild(LPDISPATCH newChild)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x10, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		newChild);
	return result;
}

BOOL CIXTLRuntime::hasChildNodes()
{
	BOOL result;
	InvokeHelper(0x11, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXTLRuntime::GetOwnerDocument()
{
	LPDISPATCH result;
	InvokeHelper(0x12, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXTLRuntime::cloneNode(BOOL deep)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x13, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		deep);
	return result;
}

CString CIXTLRuntime::GetNodeTypeString()
{
	CString result;
	InvokeHelper(0x15, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXTLRuntime::GetText()
{
	CString result;
	InvokeHelper(0x18, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXTLRuntime::SetText(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x18, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

BOOL CIXTLRuntime::GetSpecified()
{
	BOOL result;
	InvokeHelper(0x16, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXTLRuntime::GetDefinition()
{
	LPDISPATCH result;
	InvokeHelper(0x17, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

VARIANT CIXTLRuntime::GetNodeTypedValue()
{
	VARIANT result;
	InvokeHelper(0x19, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXTLRuntime::SetNodeTypedValue(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x19, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIXTLRuntime::GetDataType()
{
	VARIANT result;
	InvokeHelper(0x1a, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIXTLRuntime::SetDataType(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1a, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIXTLRuntime::GetXml()
{
	CString result;
	InvokeHelper(0x1b, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXTLRuntime::transformNode(LPDISPATCH stylesheet)
{
	CString result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x1c, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		stylesheet);
	return result;
}

LPDISPATCH CIXTLRuntime::selectNodes(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1d, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

LPDISPATCH CIXTLRuntime::selectSingleNode(LPCTSTR queryString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1e, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		queryString);
	return result;
}

BOOL CIXTLRuntime::GetParsed()
{
	BOOL result;
	InvokeHelper(0x1f, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

CString CIXTLRuntime::GetNamespaceURI()
{
	CString result;
	InvokeHelper(0x20, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXTLRuntime::GetPrefix()
{
	CString result;
	InvokeHelper(0x21, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXTLRuntime::GetBaseName()
{
	CString result;
	InvokeHelper(0x22, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXTLRuntime::transformNodeToObject(LPDISPATCH stylesheet, const VARIANT& outputObject)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_VARIANT;
	InvokeHelper(0x23, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 stylesheet, &outputObject);
}

long CIXTLRuntime::uniqueID(LPDISPATCH pNode)
{
	long result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xbb, DISPATCH_METHOD, VT_I4, (void*)&result, parms,
		pNode);
	return result;
}

long CIXTLRuntime::depth(LPDISPATCH pNode)
{
	long result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xbc, DISPATCH_METHOD, VT_I4, (void*)&result, parms,
		pNode);
	return result;
}

long CIXTLRuntime::childNumber(LPDISPATCH pNode)
{
	long result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xbd, DISPATCH_METHOD, VT_I4, (void*)&result, parms,
		pNode);
	return result;
}

long CIXTLRuntime::ancestorChildNumber(LPCTSTR bstrNodeName, LPDISPATCH pNode)
{
	long result;
	static BYTE parms[] =
		VTS_BSTR VTS_DISPATCH;
	InvokeHelper(0xbe, DISPATCH_METHOD, VT_I4, (void*)&result, parms,
		bstrNodeName, pNode);
	return result;
}

long CIXTLRuntime::absoluteChildNumber(LPDISPATCH pNode)
{
	long result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xbf, DISPATCH_METHOD, VT_I4, (void*)&result, parms,
		pNode);
	return result;
}

CString CIXTLRuntime::formatIndex(long lIndex, LPCTSTR bstrFormat)
{
	CString result;
	static BYTE parms[] =
		VTS_I4 VTS_BSTR;
	InvokeHelper(0xc0, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		lIndex, bstrFormat);
	return result;
}

CString CIXTLRuntime::formatNumber(double dblNumber, LPCTSTR bstrFormat)
{
	CString result;
	static BYTE parms[] =
		VTS_R8 VTS_BSTR;
	InvokeHelper(0xc1, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		dblNumber, bstrFormat);
	return result;
}

CString CIXTLRuntime::formatDate(const VARIANT& varDate, LPCTSTR bstrFormat, const VARIANT& varDestLocale)
{
	CString result;
	static BYTE parms[] =
		VTS_VARIANT VTS_BSTR VTS_VARIANT;
	InvokeHelper(0xc2, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		&varDate, bstrFormat, &varDestLocale);
	return result;
}

CString CIXTLRuntime::formatTime(const VARIANT& varTime, LPCTSTR bstrFormat, const VARIANT& varDestLocale)
{
	CString result;
	static BYTE parms[] =
		VTS_VARIANT VTS_BSTR VTS_VARIANT;
	InvokeHelper(0xc3, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		&varTime, bstrFormat, &varDestLocale);
	return result;
}


/////////////////////////////////////////////////////////////////////////////
// CIXSLTemplate properties

/////////////////////////////////////////////////////////////////////////////
// CIXSLTemplate operations

void CIXSLTemplate::SetRefStylesheet(LPDISPATCH newValue)
{
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x2, DISPATCH_PROPERTYPUTREF, VT_EMPTY, NULL, parms,
		 newValue);
}

LPDISPATCH CIXSLTemplate::GetStylesheet()
{
	LPDISPATCH result;
	InvokeHelper(0x2, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXSLTemplate::createProcessor()
{
	LPDISPATCH result;
	InvokeHelper(0x3, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, NULL);
	return result;
}


/////////////////////////////////////////////////////////////////////////////
// CIXSLProcessor properties

/////////////////////////////////////////////////////////////////////////////
// CIXSLProcessor operations

void CIXSLProcessor::SetInput(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x2, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIXSLProcessor::GetInput()
{
	VARIANT result;
	InvokeHelper(0x2, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXSLProcessor::GetOwnerTemplate()
{
	LPDISPATCH result;
	InvokeHelper(0x3, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void CIXSLProcessor::setStartMode(LPCTSTR mode, LPCTSTR namespaceURI)
{
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR;
	InvokeHelper(0x4, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 mode, namespaceURI);
}

CString CIXSLProcessor::GetStartMode()
{
	CString result;
	InvokeHelper(0x5, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXSLProcessor::GetStartModeURI()
{
	CString result;
	InvokeHelper(0x6, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXSLProcessor::SetOutput(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x7, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIXSLProcessor::GetOutput()
{
	VARIANT result;
	InvokeHelper(0x7, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

BOOL CIXSLProcessor::transform()
{
	BOOL result;
	InvokeHelper(0x8, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

void CIXSLProcessor::reset()
{
	InvokeHelper(0x9, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

long CIXSLProcessor::GetReadyState()
{
	long result;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

void CIXSLProcessor::addParameter(LPCTSTR baseName, const VARIANT& parameter, LPCTSTR namespaceURI)
{
	static BYTE parms[] =
		VTS_BSTR VTS_VARIANT VTS_BSTR;
	InvokeHelper(0xb, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 baseName, &parameter, namespaceURI);
}

void CIXSLProcessor::addObject(LPDISPATCH obj, LPCTSTR namespaceURI)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_BSTR;
	InvokeHelper(0xc, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 obj, namespaceURI);
}

LPDISPATCH CIXSLProcessor::GetStylesheet()
{
	LPDISPATCH result;
	InvokeHelper(0xd, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}


/////////////////////////////////////////////////////////////////////////////
// CIVBSAXXMLReader properties

/////////////////////////////////////////////////////////////////////////////
// CIVBSAXXMLReader operations

BOOL CIVBSAXXMLReader::getFeature(LPCTSTR strName)
{
	BOOL result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x502, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		strName);
	return result;
}

void CIVBSAXXMLReader::putFeature(LPCTSTR strName, BOOL fValue)
{
	static BYTE parms[] =
		VTS_BSTR VTS_BOOL;
	InvokeHelper(0x503, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strName, fValue);
}

VARIANT CIVBSAXXMLReader::getProperty(LPCTSTR strName)
{
	VARIANT result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x504, DISPATCH_METHOD, VT_VARIANT, (void*)&result, parms,
		strName);
	return result;
}

void CIVBSAXXMLReader::putProperty(LPCTSTR strName, const VARIANT& varValue)
{
	static BYTE parms[] =
		VTS_BSTR VTS_VARIANT;
	InvokeHelper(0x505, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strName, &varValue);
}

LPDISPATCH CIVBSAXXMLReader::GetEntityResolver()
{
	LPDISPATCH result;
	InvokeHelper(0x506, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void CIVBSAXXMLReader::SetRefEntityResolver(LPDISPATCH newValue)
{
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x506, DISPATCH_PROPERTYPUTREF, VT_EMPTY, NULL, parms,
		 newValue);
}

LPDISPATCH CIVBSAXXMLReader::GetContentHandler()
{
	LPDISPATCH result;
	InvokeHelper(0x507, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void CIVBSAXXMLReader::SetRefContentHandler(LPDISPATCH newValue)
{
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x507, DISPATCH_PROPERTYPUTREF, VT_EMPTY, NULL, parms,
		 newValue);
}

LPDISPATCH CIVBSAXXMLReader::GetDtdHandler()
{
	LPDISPATCH result;
	InvokeHelper(0x508, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void CIVBSAXXMLReader::SetRefDtdHandler(LPDISPATCH newValue)
{
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x508, DISPATCH_PROPERTYPUTREF, VT_EMPTY, NULL, parms,
		 newValue);
}

LPDISPATCH CIVBSAXXMLReader::GetErrorHandler()
{
	LPDISPATCH result;
	InvokeHelper(0x509, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void CIVBSAXXMLReader::SetRefErrorHandler(LPDISPATCH newValue)
{
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x509, DISPATCH_PROPERTYPUTREF, VT_EMPTY, NULL, parms,
		 newValue);
}

CString CIVBSAXXMLReader::GetBaseURL()
{
	CString result;
	InvokeHelper(0x50a, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIVBSAXXMLReader::SetBaseURL(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x50a, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIVBSAXXMLReader::GetSecureBaseURL()
{
	CString result;
	InvokeHelper(0x50b, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIVBSAXXMLReader::SetSecureBaseURL(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x50b, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

void CIVBSAXXMLReader::parse(const VARIANT& varInput)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x50c, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 &varInput);
}

void CIVBSAXXMLReader::parseURL(LPCTSTR strURL)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x50d, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strURL);
}


/////////////////////////////////////////////////////////////////////////////
// CIVBSAXEntityResolver properties

/////////////////////////////////////////////////////////////////////////////
// CIVBSAXEntityResolver operations

VARIANT CIVBSAXEntityResolver::resolveEntity(BSTR* strPublicId, BSTR* strSystemId)
{
	VARIANT result;
	static BYTE parms[] =
		VTS_PBSTR VTS_PBSTR;
	InvokeHelper(0x527, DISPATCH_METHOD, VT_VARIANT, (void*)&result, parms,
		strPublicId, strSystemId);
	return result;
}


/////////////////////////////////////////////////////////////////////////////
// CIVBSAXContentHandler properties

/////////////////////////////////////////////////////////////////////////////
// CIVBSAXContentHandler operations

void CIVBSAXContentHandler::SetRefDocumentLocator(LPDISPATCH newValue)
{
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x52a, DISPATCH_PROPERTYPUTREF, VT_EMPTY, NULL, parms,
		 newValue);
}

void CIVBSAXContentHandler::startDocument()
{
	InvokeHelper(0x52b, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void CIVBSAXContentHandler::endDocument()
{
	InvokeHelper(0x52c, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void CIVBSAXContentHandler::startPrefixMapping(BSTR* strPrefix, BSTR* strURI)
{
	static BYTE parms[] =
		VTS_PBSTR VTS_PBSTR;
	InvokeHelper(0x52d, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strPrefix, strURI);
}

void CIVBSAXContentHandler::endPrefixMapping(BSTR* strPrefix)
{
	static BYTE parms[] =
		VTS_PBSTR;
	InvokeHelper(0x52e, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strPrefix);
}

void CIVBSAXContentHandler::startElement(BSTR* strNamespaceURI, BSTR* strLocalName, BSTR* strQName, LPDISPATCH oAttributes)
{
	static BYTE parms[] =
		VTS_PBSTR VTS_PBSTR VTS_PBSTR VTS_DISPATCH;
	InvokeHelper(0x52f, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strNamespaceURI, strLocalName, strQName, oAttributes);
}

void CIVBSAXContentHandler::endElement(BSTR* strNamespaceURI, BSTR* strLocalName, BSTR* strQName)
{
	static BYTE parms[] =
		VTS_PBSTR VTS_PBSTR VTS_PBSTR;
	InvokeHelper(0x530, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strNamespaceURI, strLocalName, strQName);
}

void CIVBSAXContentHandler::characters(BSTR* strChars)
{
	static BYTE parms[] =
		VTS_PBSTR;
	InvokeHelper(0x531, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strChars);
}

void CIVBSAXContentHandler::ignorableWhitespace(BSTR* strChars)
{
	static BYTE parms[] =
		VTS_PBSTR;
	InvokeHelper(0x532, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strChars);
}

void CIVBSAXContentHandler::processingInstruction(BSTR* strTarget, BSTR* strData)
{
	static BYTE parms[] =
		VTS_PBSTR VTS_PBSTR;
	InvokeHelper(0x533, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strTarget, strData);
}

void CIVBSAXContentHandler::skippedEntity(BSTR* strName)
{
	static BYTE parms[] =
		VTS_PBSTR;
	InvokeHelper(0x534, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strName);
}


/////////////////////////////////////////////////////////////////////////////
// CIVBSAXLocator properties

/////////////////////////////////////////////////////////////////////////////
// CIVBSAXLocator operations

long CIVBSAXLocator::GetColumnNumber()
{
	long result;
	InvokeHelper(0x521, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

long CIVBSAXLocator::GetLineNumber()
{
	long result;
	InvokeHelper(0x522, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

CString CIVBSAXLocator::GetPublicId()
{
	CString result;
	InvokeHelper(0x523, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIVBSAXLocator::GetSystemId()
{
	CString result;
	InvokeHelper(0x524, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}


/////////////////////////////////////////////////////////////////////////////
// CIVBSAXAttributes properties

/////////////////////////////////////////////////////////////////////////////
// CIVBSAXAttributes operations

long CIVBSAXAttributes::GetLength()
{
	long result;
	InvokeHelper(0x540, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

CString CIVBSAXAttributes::getURI(long nIndex)
{
	CString result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x541, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		nIndex);
	return result;
}

CString CIVBSAXAttributes::getLocalName(long nIndex)
{
	CString result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x542, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		nIndex);
	return result;
}

CString CIVBSAXAttributes::getQName(long nIndex)
{
	CString result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x543, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		nIndex);
	return result;
}

long CIVBSAXAttributes::getIndexFromName(LPCTSTR strURI, LPCTSTR strLocalName)
{
	long result;
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR;
	InvokeHelper(0x544, DISPATCH_METHOD, VT_I4, (void*)&result, parms,
		strURI, strLocalName);
	return result;
}

long CIVBSAXAttributes::getIndexFromQName(LPCTSTR strQName)
{
	long result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x545, DISPATCH_METHOD, VT_I4, (void*)&result, parms,
		strQName);
	return result;
}

CString CIVBSAXAttributes::getType(long nIndex)
{
	CString result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x546, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		nIndex);
	return result;
}

CString CIVBSAXAttributes::getTypeFromName(LPCTSTR strURI, LPCTSTR strLocalName)
{
	CString result;
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR;
	InvokeHelper(0x547, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		strURI, strLocalName);
	return result;
}

CString CIVBSAXAttributes::getTypeFromQName(LPCTSTR strQName)
{
	CString result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x548, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		strQName);
	return result;
}

CString CIVBSAXAttributes::getValue(long nIndex)
{
	CString result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x549, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		nIndex);
	return result;
}

CString CIVBSAXAttributes::getValueFromName(LPCTSTR strURI, LPCTSTR strLocalName)
{
	CString result;
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR;
	InvokeHelper(0x54a, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		strURI, strLocalName);
	return result;
}

CString CIVBSAXAttributes::getValueFromQName(LPCTSTR strQName)
{
	CString result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x54b, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		strQName);
	return result;
}


/////////////////////////////////////////////////////////////////////////////
// CIVBSAXDTDHandler properties

/////////////////////////////////////////////////////////////////////////////
// CIVBSAXDTDHandler operations

void CIVBSAXDTDHandler::notationDecl(BSTR* strName, BSTR* strPublicId, BSTR* strSystemId)
{
	static BYTE parms[] =
		VTS_PBSTR VTS_PBSTR VTS_PBSTR;
	InvokeHelper(0x537, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strName, strPublicId, strSystemId);
}

void CIVBSAXDTDHandler::unparsedEntityDecl(BSTR* strName, BSTR* strPublicId, BSTR* strSystemId, BSTR* strNotationName)
{
	static BYTE parms[] =
		VTS_PBSTR VTS_PBSTR VTS_PBSTR VTS_PBSTR;
	InvokeHelper(0x538, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strName, strPublicId, strSystemId, strNotationName);
}


/////////////////////////////////////////////////////////////////////////////
// CIVBSAXErrorHandler properties

/////////////////////////////////////////////////////////////////////////////
// CIVBSAXErrorHandler operations

void CIVBSAXErrorHandler::error(LPDISPATCH oLocator, BSTR* strErrorMessage, long nErrorCode)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_PBSTR VTS_I4;
	InvokeHelper(0x53b, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 oLocator, strErrorMessage, nErrorCode);
}

void CIVBSAXErrorHandler::fatalError(LPDISPATCH oLocator, BSTR* strErrorMessage, long nErrorCode)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_PBSTR VTS_I4;
	InvokeHelper(0x53c, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 oLocator, strErrorMessage, nErrorCode);
}

void CIVBSAXErrorHandler::ignorableWarning(LPDISPATCH oLocator, BSTR* strErrorMessage, long nErrorCode)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_PBSTR VTS_I4;
	InvokeHelper(0x53d, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 oLocator, strErrorMessage, nErrorCode);
}


/////////////////////////////////////////////////////////////////////////////
// CIVBSAXXMLFilter properties

/////////////////////////////////////////////////////////////////////////////
// CIVBSAXXMLFilter operations

LPDISPATCH CIVBSAXXMLFilter::GetParent()
{
	LPDISPATCH result;
	InvokeHelper(0x51d, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void CIVBSAXXMLFilter::SetRefParent(LPDISPATCH newValue)
{
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x51d, DISPATCH_PROPERTYPUTREF, VT_EMPTY, NULL, parms,
		 newValue);
}


/////////////////////////////////////////////////////////////////////////////
// CIVBSAXLexicalHandler properties

/////////////////////////////////////////////////////////////////////////////
// CIVBSAXLexicalHandler operations

void CIVBSAXLexicalHandler::startDTD(BSTR* strName, BSTR* strPublicId, BSTR* strSystemId)
{
	static BYTE parms[] =
		VTS_PBSTR VTS_PBSTR VTS_PBSTR;
	InvokeHelper(0x54e, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strName, strPublicId, strSystemId);
}

void CIVBSAXLexicalHandler::endDTD()
{
	InvokeHelper(0x54f, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void CIVBSAXLexicalHandler::startEntity(BSTR* strName)
{
	static BYTE parms[] =
		VTS_PBSTR;
	InvokeHelper(0x550, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strName);
}

void CIVBSAXLexicalHandler::endEntity(BSTR* strName)
{
	static BYTE parms[] =
		VTS_PBSTR;
	InvokeHelper(0x551, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strName);
}

void CIVBSAXLexicalHandler::startCDATA()
{
	InvokeHelper(0x552, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void CIVBSAXLexicalHandler::endCDATA()
{
	InvokeHelper(0x553, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void CIVBSAXLexicalHandler::comment(BSTR* strChars)
{
	static BYTE parms[] =
		VTS_PBSTR;
	InvokeHelper(0x554, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strChars);
}


/////////////////////////////////////////////////////////////////////////////
// CIVBSAXDeclHandler properties

/////////////////////////////////////////////////////////////////////////////
// CIVBSAXDeclHandler operations

void CIVBSAXDeclHandler::elementDecl(BSTR* strName, BSTR* strModel)
{
	static BYTE parms[] =
		VTS_PBSTR VTS_PBSTR;
	InvokeHelper(0x557, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strName, strModel);
}

void CIVBSAXDeclHandler::attributeDecl(BSTR* strElementName, BSTR* strAttributeName, BSTR* strType, BSTR* strValueDefault, BSTR* strValue)
{
	static BYTE parms[] =
		VTS_PBSTR VTS_PBSTR VTS_PBSTR VTS_PBSTR VTS_PBSTR;
	InvokeHelper(0x558, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strElementName, strAttributeName, strType, strValueDefault, strValue);
}

void CIVBSAXDeclHandler::internalEntityDecl(BSTR* strName, BSTR* strValue)
{
	static BYTE parms[] =
		VTS_PBSTR VTS_PBSTR;
	InvokeHelper(0x559, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strName, strValue);
}

void CIVBSAXDeclHandler::externalEntityDecl(BSTR* strName, BSTR* strPublicId, BSTR* strSystemId)
{
	static BYTE parms[] =
		VTS_PBSTR VTS_PBSTR VTS_PBSTR;
	InvokeHelper(0x55a, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strName, strPublicId, strSystemId);
}


/////////////////////////////////////////////////////////////////////////////
// CIMXWriter properties

/////////////////////////////////////////////////////////////////////////////
// CIMXWriter operations

void CIMXWriter::SetOutput(const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x569, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 &newValue);
}

VARIANT CIMXWriter::GetOutput()
{
	VARIANT result;
	InvokeHelper(0x569, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

void CIMXWriter::SetEncoding(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x56b, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIMXWriter::GetEncoding()
{
	CString result;
	InvokeHelper(0x56b, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIMXWriter::SetByteOrderMark(BOOL bNewValue)
{
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x56c, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 bNewValue);
}

BOOL CIMXWriter::GetByteOrderMark()
{
	BOOL result;
	InvokeHelper(0x56c, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

void CIMXWriter::SetIndent(BOOL bNewValue)
{
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x56d, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 bNewValue);
}

BOOL CIMXWriter::GetIndent()
{
	BOOL result;
	InvokeHelper(0x56d, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

void CIMXWriter::SetStandalone(BOOL bNewValue)
{
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x56e, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 bNewValue);
}

BOOL CIMXWriter::GetStandalone()
{
	BOOL result;
	InvokeHelper(0x56e, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

void CIMXWriter::SetOmitXMLDeclaration(BOOL bNewValue)
{
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x56f, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 bNewValue);
}

BOOL CIMXWriter::GetOmitXMLDeclaration()
{
	BOOL result;
	InvokeHelper(0x56f, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

void CIMXWriter::SetVersion(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x570, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIMXWriter::GetVersion()
{
	CString result;
	InvokeHelper(0x570, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIMXWriter::SetDisableOutputEscaping(BOOL bNewValue)
{
	static BYTE parms[] =
		VTS_BOOL;
	InvokeHelper(0x571, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 bNewValue);
}

BOOL CIMXWriter::GetDisableOutputEscaping()
{
	BOOL result;
	InvokeHelper(0x571, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

void CIMXWriter::flush()
{
	InvokeHelper(0x572, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}


/////////////////////////////////////////////////////////////////////////////
// CIMXAttributes properties

/////////////////////////////////////////////////////////////////////////////
// CIMXAttributes operations

void CIMXAttributes::addAttribute(LPCTSTR strURI, LPCTSTR strLocalName, LPCTSTR strQName, LPCTSTR strType, LPCTSTR strValue)
{
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR VTS_BSTR VTS_BSTR VTS_BSTR;
	InvokeHelper(0x55d, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strURI, strLocalName, strQName, strType, strValue);
}

void CIMXAttributes::addAttributeFromIndex(const VARIANT& varAtts, long nIndex)
{
	static BYTE parms[] =
		VTS_VARIANT VTS_I4;
	InvokeHelper(0x567, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 &varAtts, nIndex);
}

void CIMXAttributes::clear()
{
	InvokeHelper(0x55e, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void CIMXAttributes::removeAttribute(long nIndex)
{
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x55f, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 nIndex);
}

void CIMXAttributes::setAttribute(long nIndex, LPCTSTR strURI, LPCTSTR strLocalName, LPCTSTR strQName, LPCTSTR strType, LPCTSTR strValue)
{
	static BYTE parms[] =
		VTS_I4 VTS_BSTR VTS_BSTR VTS_BSTR VTS_BSTR VTS_BSTR;
	InvokeHelper(0x560, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 nIndex, strURI, strLocalName, strQName, strType, strValue);
}

void CIMXAttributes::setAttributes(const VARIANT& varAtts)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x561, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 &varAtts);
}

void CIMXAttributes::setLocalName(long nIndex, LPCTSTR strLocalName)
{
	static BYTE parms[] =
		VTS_I4 VTS_BSTR;
	InvokeHelper(0x562, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 nIndex, strLocalName);
}

void CIMXAttributes::setQName(long nIndex, LPCTSTR strQName)
{
	static BYTE parms[] =
		VTS_I4 VTS_BSTR;
	InvokeHelper(0x563, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 nIndex, strQName);
}

void CIMXAttributes::setType(long nIndex, LPCTSTR strType)
{
	static BYTE parms[] =
		VTS_I4 VTS_BSTR;
	InvokeHelper(0x564, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 nIndex, strType);
}

void CIMXAttributes::setURI(long nIndex, LPCTSTR strURI)
{
	static BYTE parms[] =
		VTS_I4 VTS_BSTR;
	InvokeHelper(0x565, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 nIndex, strURI);
}

void CIMXAttributes::setValue(long nIndex, LPCTSTR strValue)
{
	static BYTE parms[] =
		VTS_I4 VTS_BSTR;
	InvokeHelper(0x566, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 nIndex, strValue);
}


/////////////////////////////////////////////////////////////////////////////
// CIMXReaderControl properties

/////////////////////////////////////////////////////////////////////////////
// CIMXReaderControl operations

void CIMXReaderControl::abort()
{
	InvokeHelper(0x576, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void CIMXReaderControl::resume()
{
	InvokeHelper(0x577, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void CIMXReaderControl::suspend()
{
	InvokeHelper(0x578, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLElementCollection properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLElementCollection operations

long CIXMLElementCollection::GetLength()
{
	long result;
	InvokeHelper(0x10001, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLElementCollection::item(const VARIANT& var1, const VARIANT& var2)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_VARIANT VTS_VARIANT;
	InvokeHelper(0x10003, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		&var1, &var2);
	return result;
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLDocument properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDocument operations

LPDISPATCH CIXMLDocument::GetRoot()
{
	LPDISPATCH result;
	InvokeHelper(0x10065, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

CString CIXMLDocument::GetUrl()
{
	CString result;
	InvokeHelper(0x10069, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDocument::SetUrl(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x10069, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

long CIXMLDocument::GetReadyState()
{
	long result;
	InvokeHelper(0x1006b, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

CString CIXMLDocument::GetCharset()
{
	CString result;
	InvokeHelper(0x1006d, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDocument::SetCharset(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1006d, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString CIXMLDocument::GetVersion()
{
	CString result;
	InvokeHelper(0x1006e, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLDocument::GetDoctype()
{
	CString result;
	InvokeHelper(0x1006f, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDocument::createElement(const VARIANT& vType, const VARIANT& var1)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_VARIANT VTS_VARIANT;
	InvokeHelper(0x1006c, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		&vType, &var1);
	return result;
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLElement properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLElement operations

CString CIXMLElement::GetTagName()
{
	CString result;
	InvokeHelper(0x100c9, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLElement::SetTagName(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x100c9, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

LPDISPATCH CIXMLElement::GetParent()
{
	LPDISPATCH result;
	InvokeHelper(0x100ca, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void CIXMLElement::setAttribute(LPCTSTR strPropertyName, const VARIANT& PropertyValue)
{
	static BYTE parms[] =
		VTS_BSTR VTS_VARIANT;
	InvokeHelper(0x100cb, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strPropertyName, &PropertyValue);
}

VARIANT CIXMLElement::getAttribute(LPCTSTR strPropertyName)
{
	VARIANT result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x100cc, DISPATCH_METHOD, VT_VARIANT, (void*)&result, parms,
		strPropertyName);
	return result;
}

void CIXMLElement::removeAttribute(LPCTSTR strPropertyName)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x100cd, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strPropertyName);
}

LPDISPATCH CIXMLElement::GetChildren()
{
	LPDISPATCH result;
	InvokeHelper(0x100ce, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

long CIXMLElement::GetType()
{
	long result;
	InvokeHelper(0x100cf, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

CString CIXMLElement::GetText()
{
	CString result;
	InvokeHelper(0x100d0, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLElement::SetText(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x100d0, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

void CIXMLElement::addChild(LPDISPATCH pChildElem, long lIndex, long lReserved)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_I4 VTS_I4;
	InvokeHelper(0x100d1, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 pChildElem, lIndex, lReserved);
}

void CIXMLElement::removeChild(LPDISPATCH pChildElem)
{
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x100d2, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 pChildElem);
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLElement2 properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLElement2 operations

CString CIXMLElement2::GetTagName()
{
	CString result;
	InvokeHelper(0x100c9, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLElement2::SetTagName(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x100c9, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

LPDISPATCH CIXMLElement2::GetParent()
{
	LPDISPATCH result;
	InvokeHelper(0x100ca, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void CIXMLElement2::setAttribute(LPCTSTR strPropertyName, const VARIANT& PropertyValue)
{
	static BYTE parms[] =
		VTS_BSTR VTS_VARIANT;
	InvokeHelper(0x100cb, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strPropertyName, &PropertyValue);
}

VARIANT CIXMLElement2::getAttribute(LPCTSTR strPropertyName)
{
	VARIANT result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x100cc, DISPATCH_METHOD, VT_VARIANT, (void*)&result, parms,
		strPropertyName);
	return result;
}

void CIXMLElement2::removeAttribute(LPCTSTR strPropertyName)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x100cd, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 strPropertyName);
}

LPDISPATCH CIXMLElement2::GetChildren()
{
	LPDISPATCH result;
	InvokeHelper(0x100ce, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

long CIXMLElement2::GetType()
{
	long result;
	InvokeHelper(0x100cf, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

CString CIXMLElement2::GetText()
{
	CString result;
	InvokeHelper(0x100d0, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLElement2::SetText(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x100d0, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

void CIXMLElement2::addChild(LPDISPATCH pChildElem, long lIndex, long lReserved)
{
	static BYTE parms[] =
		VTS_DISPATCH VTS_I4 VTS_I4;
	InvokeHelper(0x100d1, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 pChildElem, lIndex, lReserved);
}

void CIXMLElement2::removeChild(LPDISPATCH pChildElem)
{
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x100d2, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 pChildElem);
}

LPDISPATCH CIXMLElement2::GetAttributes()
{
	LPDISPATCH result;
	InvokeHelper(0x100d3, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLAttribute properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLAttribute operations

CString CIXMLAttribute::GetName()
{
	CString result;
	InvokeHelper(0x10191, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

CString CIXMLAttribute::GetValue()
{
	CString result;
	InvokeHelper(0x10192, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMSelection properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMSelection operations

LPDISPATCH CIXMLDOMSelection::GetItem(long index)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x0, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, parms,
		index);
	return result;
}

long CIXMLDOMSelection::GetLength()
{
	long result;
	InvokeHelper(0x4a, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMSelection::nextNode()
{
	LPDISPATCH result;
	InvokeHelper(0x4c, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void CIXMLDOMSelection::reset()
{
	InvokeHelper(0x4d, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

CString CIXMLDOMSelection::GetExpr()
{
	CString result;
	InvokeHelper(0x51, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLDOMSelection::SetExpr(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x51, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

LPDISPATCH CIXMLDOMSelection::GetContext()
{
	LPDISPATCH result;
	InvokeHelper(0x52, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void CIXMLDOMSelection::SetRefContext(LPDISPATCH newValue)
{
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x52, DISPATCH_PROPERTYPUTREF, VT_EMPTY, NULL, parms,
		 newValue);
}

LPDISPATCH CIXMLDOMSelection::peekNode()
{
	LPDISPATCH result;
	InvokeHelper(0x53, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLDOMSelection::matches(LPDISPATCH pNode)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x54, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		pNode);
	return result;
}

LPDISPATCH CIXMLDOMSelection::removeNext()
{
	LPDISPATCH result;
	InvokeHelper(0x55, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void CIXMLDOMSelection::removeAll()
{
	InvokeHelper(0x56, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

LPDISPATCH CIXMLDOMSelection::clone()
{
	LPDISPATCH result;
	InvokeHelper(0x57, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLDOMSelection::getProperty(LPCTSTR name)
{
	VARIANT result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x58, DISPATCH_METHOD, VT_VARIANT, (void*)&result, parms,
		name);
	return result;
}

void CIXMLDOMSelection::setProperty(LPCTSTR name, const VARIANT& value)
{
	static BYTE parms[] =
		VTS_BSTR VTS_VARIANT;
	InvokeHelper(0x59, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 name, &value);
}


/////////////////////////////////////////////////////////////////////////////
// XMLDOMDocumentEvents properties

/////////////////////////////////////////////////////////////////////////////
// XMLDOMDocumentEvents operations


/////////////////////////////////////////////////////////////////////////////
// CIDSOControl properties

/////////////////////////////////////////////////////////////////////////////
// CIDSOControl operations

LPDISPATCH CIDSOControl::GetXMLDocument()
{
	LPDISPATCH result;
	InvokeHelper(0x10001, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void CIDSOControl::SetXMLDocument(LPDISPATCH newValue)
{
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x10001, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 newValue);
}

long CIDSOControl::GetJavaDSOCompatible()
{
	long result;
	InvokeHelper(0x10002, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

void CIDSOControl::SetJavaDSOCompatible(long nNewValue)
{
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x10002, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 nNewValue);
}

long CIDSOControl::GetReadyState()
{
	long result;
	InvokeHelper(DISPID_READYSTATE, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}


/////////////////////////////////////////////////////////////////////////////
// CIXMLHTTPRequest properties

/////////////////////////////////////////////////////////////////////////////
// CIXMLHTTPRequest operations

void CIXMLHTTPRequest::open(LPCTSTR bstrMethod, LPCTSTR bstrUrl, const VARIANT& varAsync, const VARIANT& bstrUser, const VARIANT& bstrPassword)
{
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR VTS_VARIANT VTS_VARIANT VTS_VARIANT;
	InvokeHelper(0x1, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 bstrMethod, bstrUrl, &varAsync, &bstrUser, &bstrPassword);
}

void CIXMLHTTPRequest::setRequestHeader(LPCTSTR bstrHeader, LPCTSTR bstrValue)
{
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR;
	InvokeHelper(0x2, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 bstrHeader, bstrValue);
}

CString CIXMLHTTPRequest::getResponseHeader(LPCTSTR bstrHeader)
{
	CString result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x3, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		bstrHeader);
	return result;
}

CString CIXMLHTTPRequest::getAllResponseHeaders()
{
	CString result;
	InvokeHelper(0x4, DISPATCH_METHOD, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIXMLHTTPRequest::send(const VARIANT& varBody)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x5, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 &varBody);
}

void CIXMLHTTPRequest::abort()
{
	InvokeHelper(0x6, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

long CIXMLHTTPRequest::GetStatus()
{
	long result;
	InvokeHelper(0x7, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

CString CIXMLHTTPRequest::GetStatusText()
{
	CString result;
	InvokeHelper(0x8, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIXMLHTTPRequest::GetResponseXML()
{
	LPDISPATCH result;
	InvokeHelper(0x9, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

CString CIXMLHTTPRequest::GetResponseText()
{
	CString result;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLHTTPRequest::GetResponseBody()
{
	VARIANT result;
	InvokeHelper(0xb, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

VARIANT CIXMLHTTPRequest::GetResponseStream()
{
	VARIANT result;
	InvokeHelper(0xc, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

long CIXMLHTTPRequest::GetReadyState()
{
	long result;
	InvokeHelper(0xd, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

void CIXMLHTTPRequest::SetOnreadystatechange(LPDISPATCH newValue)
{
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xe, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 newValue);
}


/////////////////////////////////////////////////////////////////////////////
// CIServerXMLHTTPRequest properties

/////////////////////////////////////////////////////////////////////////////
// CIServerXMLHTTPRequest operations

void CIServerXMLHTTPRequest::open(LPCTSTR bstrMethod, LPCTSTR bstrUrl, const VARIANT& varAsync, const VARIANT& bstrUser, const VARIANT& bstrPassword)
{
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR VTS_VARIANT VTS_VARIANT VTS_VARIANT;
	InvokeHelper(0x1, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 bstrMethod, bstrUrl, &varAsync, &bstrUser, &bstrPassword);
}

void CIServerXMLHTTPRequest::setRequestHeader(LPCTSTR bstrHeader, LPCTSTR bstrValue)
{
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR;
	InvokeHelper(0x2, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 bstrHeader, bstrValue);
}

CString CIServerXMLHTTPRequest::getResponseHeader(LPCTSTR bstrHeader)
{
	CString result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x3, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		bstrHeader);
	return result;
}

CString CIServerXMLHTTPRequest::getAllResponseHeaders()
{
	CString result;
	InvokeHelper(0x4, DISPATCH_METHOD, VT_BSTR, (void*)&result, NULL);
	return result;
}

void CIServerXMLHTTPRequest::send(const VARIANT& varBody)
{
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x5, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 &varBody);
}

void CIServerXMLHTTPRequest::abort()
{
	InvokeHelper(0x6, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

long CIServerXMLHTTPRequest::GetStatus()
{
	long result;
	InvokeHelper(0x7, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

CString CIServerXMLHTTPRequest::GetStatusText()
{
	CString result;
	InvokeHelper(0x8, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

LPDISPATCH CIServerXMLHTTPRequest::GetResponseXML()
{
	LPDISPATCH result;
	InvokeHelper(0x9, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

CString CIServerXMLHTTPRequest::GetResponseText()
{
	CString result;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}

VARIANT CIServerXMLHTTPRequest::GetResponseBody()
{
	VARIANT result;
	InvokeHelper(0xb, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

VARIANT CIServerXMLHTTPRequest::GetResponseStream()
{
	VARIANT result;
	InvokeHelper(0xc, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

long CIServerXMLHTTPRequest::GetReadyState()
{
	long result;
	InvokeHelper(0xd, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

void CIServerXMLHTTPRequest::SetOnreadystatechange(LPDISPATCH newValue)
{
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xe, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 newValue);
}

void CIServerXMLHTTPRequest::setTimeouts(long resolveTimeout, long connectTimeout, long sendTimeout, long receiveTimeout)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4 VTS_I4 VTS_I4;
	InvokeHelper(0xf, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 resolveTimeout, connectTimeout, sendTimeout, receiveTimeout);
}

BOOL CIServerXMLHTTPRequest::waitForResponse(const VARIANT& timeoutInSeconds)
{
	BOOL result;
	static BYTE parms[] =
		VTS_VARIANT;
	InvokeHelper(0x10, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		&timeoutInSeconds);
	return result;
}

VARIANT CIServerXMLHTTPRequest::getOption(long option)
{
	VARIANT result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x11, DISPATCH_METHOD, VT_VARIANT, (void*)&result, parms,
		option);
	return result;
}

void CIServerXMLHTTPRequest::setOption(long option, const VARIANT& value)
{
	static BYTE parms[] =
		VTS_I4 VTS_VARIANT;
	InvokeHelper(0x12, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 option, &value);
}
