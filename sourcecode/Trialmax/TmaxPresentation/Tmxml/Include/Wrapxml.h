// Machine generated IDispatch wrapper class(es) created with ClassWizard
/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMDocument wrapper class

class CIXMLDOMDocument : public COleDispatchDriver
{
public:
	CIXMLDOMDocument() {}		// Calls COleDispatchDriver default constructor
	CIXMLDOMDocument(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDOMDocument(const CIXMLDOMDocument& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	CString GetNodeName();
	VARIANT GetNodeValue();
	void SetNodeValue(const VARIANT& newValue);
	long GetNodeType();
	LPDISPATCH GetParentNode();
	LPDISPATCH GetChildNodes();
	LPDISPATCH GetFirstChild();
	LPDISPATCH GetLastChild();
	LPDISPATCH GetPreviousSibling();
	LPDISPATCH GetNextSibling();
	LPDISPATCH GetAttributes();
	LPDISPATCH insertBefore(LPDISPATCH newChild, const VARIANT& refChild);
	LPDISPATCH replaceChild(LPDISPATCH newChild, LPDISPATCH oldChild);
	LPDISPATCH removeChild(LPDISPATCH childNode);
	LPDISPATCH appendChild(LPDISPATCH newChild);
	BOOL hasChildNodes();
	LPDISPATCH GetOwnerDocument();
	LPDISPATCH cloneNode(BOOL deep);
	CString GetNodeTypeString();
	CString GetText();
	void SetText(LPCTSTR lpszNewValue);
	BOOL GetSpecified();
	LPDISPATCH GetDefinition();
	VARIANT GetNodeTypedValue();
	void SetNodeTypedValue(const VARIANT& newValue);
	VARIANT GetDataType();
	void SetDataType(LPCTSTR lpszNewValue);
	CString GetXml();
	CString transformNode(LPDISPATCH stylesheet);
	LPDISPATCH selectNodes(LPCTSTR queryString);
	LPDISPATCH selectSingleNode(LPCTSTR queryString);
	BOOL GetParsed();
	CString GetNamespaceURI();
	CString GetPrefix();
	CString GetBaseName();
	void transformNodeToObject(LPDISPATCH stylesheet, const VARIANT& outputObject);
	LPDISPATCH GetDoctype();
	LPDISPATCH GetImplementation();
	LPDISPATCH GetDocumentElement();
	void SetRefDocumentElement(LPDISPATCH newValue);
	LPDISPATCH createElement(LPCTSTR tagName);
	LPDISPATCH createDocumentFragment();
	LPDISPATCH createTextNode(LPCTSTR data);
	LPDISPATCH createComment(LPCTSTR data);
	LPDISPATCH createCDATASection(LPCTSTR data);
	LPDISPATCH createProcessingInstruction(LPCTSTR target, LPCTSTR data);
	LPDISPATCH createAttribute(LPCTSTR name);
	LPDISPATCH createEntityReference(LPCTSTR name);
	LPDISPATCH getElementsByTagName(LPCTSTR tagName);
	LPDISPATCH createNode(const VARIANT& type, LPCTSTR name, LPCTSTR namespaceURI);
	LPDISPATCH nodeFromID(LPCTSTR idString);
	BOOL load(const VARIANT& xmlSource);
	long GetReadyState();
	LPDISPATCH GetParseError();
	CString GetUrl();
	BOOL GetAsync();
	void SetAsync(BOOL bNewValue);
	void abort();
	BOOL loadXML(LPCTSTR bstrXML);
	void save(const VARIANT& desination);
	BOOL GetValidateOnParse();
	void SetValidateOnParse(BOOL bNewValue);
	BOOL GetResolveExternals();
	void SetResolveExternals(BOOL bNewValue);
	BOOL GetPreserveWhiteSpace();
	void SetPreserveWhiteSpace(BOOL bNewValue);
	void SetOnreadystatechange(const VARIANT& newValue);
	void SetOndataavailable(const VARIANT& newValue);
	void SetOntransformnode(const VARIANT& newValue);
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMNode wrapper class

class CIXMLDOMNode : public COleDispatchDriver
{
public:
	CIXMLDOMNode() {}		// Calls COleDispatchDriver default constructor
	CIXMLDOMNode(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDOMNode(const CIXMLDOMNode& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	CString GetNodeName();
	VARIANT GetNodeValue();
	void SetNodeValue(const VARIANT& newValue);
	long GetNodeType();
	LPDISPATCH GetParentNode();
	LPDISPATCH GetChildNodes();
	LPDISPATCH GetFirstChild();
	LPDISPATCH GetLastChild();
	LPDISPATCH GetPreviousSibling();
	LPDISPATCH GetNextSibling();
	LPDISPATCH GetAttributes();
	LPDISPATCH insertBefore(LPDISPATCH newChild, const VARIANT& refChild);
	LPDISPATCH replaceChild(LPDISPATCH newChild, LPDISPATCH oldChild);
	LPDISPATCH removeChild(LPDISPATCH childNode);
	LPDISPATCH appendChild(LPDISPATCH newChild);
	BOOL hasChildNodes();
	LPDISPATCH GetOwnerDocument();
	LPDISPATCH cloneNode(BOOL deep);
	CString GetNodeTypeString();
	CString GetText();
	void SetText(LPCTSTR lpszNewValue);
	BOOL GetSpecified();
	LPDISPATCH GetDefinition();
	VARIANT GetNodeTypedValue();
	void SetNodeTypedValue(const VARIANT& newValue);
	VARIANT GetDataType();
	void SetDataType(LPCTSTR lpszNewValue);
	CString GetXml();
	CString transformNode(LPDISPATCH stylesheet);
	LPDISPATCH selectNodes(LPCTSTR queryString);
	LPDISPATCH selectSingleNode(LPCTSTR queryString);
	BOOL GetParsed();
	CString GetNamespaceURI();
	CString GetPrefix();
	CString GetBaseName();
	void transformNodeToObject(LPDISPATCH stylesheet, const VARIANT& outputObject);
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMNodeList wrapper class

class CIXMLDOMNodeList : public COleDispatchDriver
{
public:
	CIXMLDOMNodeList() {}		// Calls COleDispatchDriver default constructor
	CIXMLDOMNodeList(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDOMNodeList(const CIXMLDOMNodeList& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	LPDISPATCH GetItem(long index);
	long GetLength();
	LPDISPATCH nextNode();
	void reset();
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMNamedNodeMap wrapper class

class CIXMLDOMNamedNodeMap : public COleDispatchDriver
{
public:
	CIXMLDOMNamedNodeMap() {}		// Calls COleDispatchDriver default constructor
	CIXMLDOMNamedNodeMap(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDOMNamedNodeMap(const CIXMLDOMNamedNodeMap& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	LPDISPATCH getNamedItem(LPCTSTR name);
	LPDISPATCH setNamedItem(LPDISPATCH newItem);
	LPDISPATCH removeNamedItem(LPCTSTR name);
	LPDISPATCH GetItem(long index);
	long GetLength();
	LPDISPATCH getQualifiedItem(LPCTSTR baseName, LPCTSTR namespaceURI);
	LPDISPATCH removeQualifiedItem(LPCTSTR baseName, LPCTSTR namespaceURI);
	LPDISPATCH nextNode();
	void reset();
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLHttpRequest wrapper class

class CIXMLHttpRequest : public COleDispatchDriver
{
public:
	CIXMLHttpRequest() {}		// Calls COleDispatchDriver default constructor
	CIXMLHttpRequest(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLHttpRequest(const CIXMLHttpRequest& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	void open(LPCTSTR bstrMethod, LPCTSTR bstrUrl, const VARIANT& varAsync, const VARIANT& bstrUser, const VARIANT& bstrPassword);
	void setRequestHeader(LPCTSTR bstrHeader, LPCTSTR bstrValue);
	CString getResponseHeader(LPCTSTR bstrHeader);
	CString getAllResponseHeaders();
	void send(const VARIANT& varBody);
	void abort();
	long GetStatus();
	CString GetStatusText();
	LPDISPATCH GetResponseXML();
	CString GetResponseText();
	VARIANT GetResponseBody();
	VARIANT GetResponseStream();
	long GetReadyState();
	void SetOnreadystatechange(LPDISPATCH newValue);
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMParseError wrapper class

class CIXMLDOMParseError : public COleDispatchDriver
{
public:
	CIXMLDOMParseError() {}		// Calls COleDispatchDriver default constructor
	CIXMLDOMParseError(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDOMParseError(const CIXMLDOMParseError& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	long GetErrorCode();
	CString GetUrl();
	CString GetReason();
	CString GetSrcText();
	long GetLine();
	long GetLinepos();
	long GetFilepos();
};
