// Machine generated IDispatch wrapper class(es) created with ClassWizard
/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMImplementation wrapper class

class CIXMLDOMImplementation : public COleDispatchDriver
{
public:
	CIXMLDOMImplementation() {}		// Calls COleDispatchDriver default constructor
	CIXMLDOMImplementation(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDOMImplementation(const CIXMLDOMImplementation& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	BOOL hasFeature(LPCTSTR feature, LPCTSTR version);
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
	void save(const VARIANT& destination);
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
// CIXMLDOMDocumentType wrapper class

class CIXMLDOMDocumentType : public COleDispatchDriver
{
public:
	CIXMLDOMDocumentType() {}		// Calls COleDispatchDriver default constructor
	CIXMLDOMDocumentType(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDOMDocumentType(const CIXMLDOMDocumentType& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

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
	CString GetName();
	LPDISPATCH GetEntities();
	LPDISPATCH GetNotations();
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMElement wrapper class

class CIXMLDOMElement : public COleDispatchDriver
{
public:
	CIXMLDOMElement() {}		// Calls COleDispatchDriver default constructor
	CIXMLDOMElement(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDOMElement(const CIXMLDOMElement& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

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
	CString GetTagName();
	VARIANT getAttribute(LPCTSTR name);
	void setAttribute(LPCTSTR name, const VARIANT& value);
	void removeAttribute(LPCTSTR name);
	LPDISPATCH getAttributeNode(LPCTSTR name);
	LPDISPATCH setAttributeNode(LPDISPATCH DOMAttribute);
	LPDISPATCH removeAttributeNode(LPDISPATCH DOMAttribute);
	LPDISPATCH getElementsByTagName(LPCTSTR tagName);
	void normalize();
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMAttribute wrapper class

class CIXMLDOMAttribute : public COleDispatchDriver
{
public:
	CIXMLDOMAttribute() {}		// Calls COleDispatchDriver default constructor
	CIXMLDOMAttribute(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDOMAttribute(const CIXMLDOMAttribute& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

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
	CString GetName();
	VARIANT GetValue();
	void SetValue(const VARIANT& newValue);
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMDocumentFragment wrapper class

class CIXMLDOMDocumentFragment : public COleDispatchDriver
{
public:
	CIXMLDOMDocumentFragment() {}		// Calls COleDispatchDriver default constructor
	CIXMLDOMDocumentFragment(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDOMDocumentFragment(const CIXMLDOMDocumentFragment& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

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
// CIXMLDOMText wrapper class

class CIXMLDOMText : public COleDispatchDriver
{
public:
	CIXMLDOMText() {}		// Calls COleDispatchDriver default constructor
	CIXMLDOMText(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDOMText(const CIXMLDOMText& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

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
	CString GetData();
	void SetData(LPCTSTR lpszNewValue);
	long GetLength();
	CString substringData(long offset, long count);
	void appendData(LPCTSTR data);
	void insertData(long offset, LPCTSTR data);
	void deleteData(long offset, long count);
	void replaceData(long offset, long count, LPCTSTR data);
	LPDISPATCH splitText(long offset);
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMCharacterData wrapper class

class CIXMLDOMCharacterData : public COleDispatchDriver
{
public:
	CIXMLDOMCharacterData() {}		// Calls COleDispatchDriver default constructor
	CIXMLDOMCharacterData(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDOMCharacterData(const CIXMLDOMCharacterData& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

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
	CString GetData();
	void SetData(LPCTSTR lpszNewValue);
	long GetLength();
	CString substringData(long offset, long count);
	void appendData(LPCTSTR data);
	void insertData(long offset, LPCTSTR data);
	void deleteData(long offset, long count);
	void replaceData(long offset, long count, LPCTSTR data);
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMComment wrapper class

class CIXMLDOMComment : public COleDispatchDriver
{
public:
	CIXMLDOMComment() {}		// Calls COleDispatchDriver default constructor
	CIXMLDOMComment(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDOMComment(const CIXMLDOMComment& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

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
	CString GetData();
	void SetData(LPCTSTR lpszNewValue);
	long GetLength();
	CString substringData(long offset, long count);
	void appendData(LPCTSTR data);
	void insertData(long offset, LPCTSTR data);
	void deleteData(long offset, long count);
	void replaceData(long offset, long count, LPCTSTR data);
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMCDATASection wrapper class

class CIXMLDOMCDATASection : public COleDispatchDriver
{
public:
	CIXMLDOMCDATASection() {}		// Calls COleDispatchDriver default constructor
	CIXMLDOMCDATASection(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDOMCDATASection(const CIXMLDOMCDATASection& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

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
	CString GetData();
	void SetData(LPCTSTR lpszNewValue);
	long GetLength();
	CString substringData(long offset, long count);
	void appendData(LPCTSTR data);
	void insertData(long offset, LPCTSTR data);
	void deleteData(long offset, long count);
	void replaceData(long offset, long count, LPCTSTR data);
	LPDISPATCH splitText(long offset);
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMProcessingInstruction wrapper class

class CIXMLDOMProcessingInstruction : public COleDispatchDriver
{
public:
	CIXMLDOMProcessingInstruction() {}		// Calls COleDispatchDriver default constructor
	CIXMLDOMProcessingInstruction(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDOMProcessingInstruction(const CIXMLDOMProcessingInstruction& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

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
	CString GetTarget();
	CString GetData();
	void SetData(LPCTSTR lpszNewValue);
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMEntityReference wrapper class

class CIXMLDOMEntityReference : public COleDispatchDriver
{
public:
	CIXMLDOMEntityReference() {}		// Calls COleDispatchDriver default constructor
	CIXMLDOMEntityReference(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDOMEntityReference(const CIXMLDOMEntityReference& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

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
/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMSchemaCollection wrapper class

class CIXMLDOMSchemaCollection : public COleDispatchDriver
{
public:
	CIXMLDOMSchemaCollection() {}		// Calls COleDispatchDriver default constructor
	CIXMLDOMSchemaCollection(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDOMSchemaCollection(const CIXMLDOMSchemaCollection& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	void add(LPCTSTR namespaceURI, const VARIANT& var);
	LPDISPATCH get(LPCTSTR namespaceURI);
	void remove(LPCTSTR namespaceURI);
	long GetLength();
	CString GetNamespaceURI(long index);
	void addCollection(LPDISPATCH otherCollection);
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMDocument2 wrapper class

class CIXMLDOMDocument2 : public COleDispatchDriver
{
public:
	CIXMLDOMDocument2() {}		// Calls COleDispatchDriver default constructor
	CIXMLDOMDocument2(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDOMDocument2(const CIXMLDOMDocument2& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

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
	void save(const VARIANT& destination);
	BOOL GetValidateOnParse();
	void SetValidateOnParse(BOOL bNewValue);
	BOOL GetResolveExternals();
	void SetResolveExternals(BOOL bNewValue);
	BOOL GetPreserveWhiteSpace();
	void SetPreserveWhiteSpace(BOOL bNewValue);
	void SetOnreadystatechange(const VARIANT& newValue);
	void SetOndataavailable(const VARIANT& newValue);
	void SetOntransformnode(const VARIANT& newValue);
	LPDISPATCH GetNamespaces();
	VARIANT GetSchemas();
	void SetRefSchemas(const VARIANT& newValue);
	LPDISPATCH validate();
	void setProperty(LPCTSTR name, const VARIANT& value);
	VARIANT getProperty(LPCTSTR name);
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMNotation wrapper class

class CIXMLDOMNotation : public COleDispatchDriver
{
public:
	CIXMLDOMNotation() {}		// Calls COleDispatchDriver default constructor
	CIXMLDOMNotation(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDOMNotation(const CIXMLDOMNotation& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

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
	VARIANT GetPublicId();
	VARIANT GetSystemId();
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMEntity wrapper class

class CIXMLDOMEntity : public COleDispatchDriver
{
public:
	CIXMLDOMEntity() {}		// Calls COleDispatchDriver default constructor
	CIXMLDOMEntity(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDOMEntity(const CIXMLDOMEntity& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

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
	VARIANT GetPublicId();
	VARIANT GetSystemId();
	CString GetNotationName();
};
/////////////////////////////////////////////////////////////////////////////
// CIXTLRuntime wrapper class

class CIXTLRuntime : public COleDispatchDriver
{
public:
	CIXTLRuntime() {}		// Calls COleDispatchDriver default constructor
	CIXTLRuntime(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXTLRuntime(const CIXTLRuntime& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

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
	long uniqueID(LPDISPATCH pNode);
	long depth(LPDISPATCH pNode);
	long childNumber(LPDISPATCH pNode);
	long ancestorChildNumber(LPCTSTR bstrNodeName, LPDISPATCH pNode);
	long absoluteChildNumber(LPDISPATCH pNode);
	CString formatIndex(long lIndex, LPCTSTR bstrFormat);
	CString formatNumber(double dblNumber, LPCTSTR bstrFormat);
	CString formatDate(const VARIANT& varDate, LPCTSTR bstrFormat, const VARIANT& varDestLocale);
	CString formatTime(const VARIANT& varTime, LPCTSTR bstrFormat, const VARIANT& varDestLocale);
};
/////////////////////////////////////////////////////////////////////////////
// CIXSLTemplate wrapper class

class CIXSLTemplate : public COleDispatchDriver
{
public:
	CIXSLTemplate() {}		// Calls COleDispatchDriver default constructor
	CIXSLTemplate(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXSLTemplate(const CIXSLTemplate& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	void SetRefStylesheet(LPDISPATCH newValue);
	LPDISPATCH GetStylesheet();
	LPDISPATCH createProcessor();
};
/////////////////////////////////////////////////////////////////////////////
// CIXSLProcessor wrapper class

class CIXSLProcessor : public COleDispatchDriver
{
public:
	CIXSLProcessor() {}		// Calls COleDispatchDriver default constructor
	CIXSLProcessor(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXSLProcessor(const CIXSLProcessor& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	void SetInput(const VARIANT& newValue);
	VARIANT GetInput();
	LPDISPATCH GetOwnerTemplate();
	void setStartMode(LPCTSTR mode, LPCTSTR namespaceURI);
	CString GetStartMode();
	CString GetStartModeURI();
	void SetOutput(const VARIANT& newValue);
	VARIANT GetOutput();
	BOOL transform();
	void reset();
	long GetReadyState();
	void addParameter(LPCTSTR baseName, const VARIANT& parameter, LPCTSTR namespaceURI);
	void addObject(LPDISPATCH obj, LPCTSTR namespaceURI);
	LPDISPATCH GetStylesheet();
};
/////////////////////////////////////////////////////////////////////////////
// CIVBSAXXMLReader wrapper class

class CIVBSAXXMLReader : public COleDispatchDriver
{
public:
	CIVBSAXXMLReader() {}		// Calls COleDispatchDriver default constructor
	CIVBSAXXMLReader(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIVBSAXXMLReader(const CIVBSAXXMLReader& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	BOOL getFeature(LPCTSTR strName);
	void putFeature(LPCTSTR strName, BOOL fValue);
	VARIANT getProperty(LPCTSTR strName);
	void putProperty(LPCTSTR strName, const VARIANT& varValue);
	LPDISPATCH GetEntityResolver();
	void SetRefEntityResolver(LPDISPATCH newValue);
	LPDISPATCH GetContentHandler();
	void SetRefContentHandler(LPDISPATCH newValue);
	LPDISPATCH GetDtdHandler();
	void SetRefDtdHandler(LPDISPATCH newValue);
	LPDISPATCH GetErrorHandler();
	void SetRefErrorHandler(LPDISPATCH newValue);
	CString GetBaseURL();
	void SetBaseURL(LPCTSTR lpszNewValue);
	CString GetSecureBaseURL();
	void SetSecureBaseURL(LPCTSTR lpszNewValue);
	void parse(const VARIANT& varInput);
	void parseURL(LPCTSTR strURL);
};
/////////////////////////////////////////////////////////////////////////////
// CIVBSAXEntityResolver wrapper class

class CIVBSAXEntityResolver : public COleDispatchDriver
{
public:
	CIVBSAXEntityResolver() {}		// Calls COleDispatchDriver default constructor
	CIVBSAXEntityResolver(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIVBSAXEntityResolver(const CIVBSAXEntityResolver& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	VARIANT resolveEntity(BSTR* strPublicId, BSTR* strSystemId);
};
/////////////////////////////////////////////////////////////////////////////
// CIVBSAXContentHandler wrapper class

class CIVBSAXContentHandler : public COleDispatchDriver
{
public:
	CIVBSAXContentHandler() {}		// Calls COleDispatchDriver default constructor
	CIVBSAXContentHandler(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIVBSAXContentHandler(const CIVBSAXContentHandler& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	void SetRefDocumentLocator(LPDISPATCH newValue);
	void startDocument();
	void endDocument();
	void startPrefixMapping(BSTR* strPrefix, BSTR* strURI);
	void endPrefixMapping(BSTR* strPrefix);
	void startElement(BSTR* strNamespaceURI, BSTR* strLocalName, BSTR* strQName, LPDISPATCH oAttributes);
	void endElement(BSTR* strNamespaceURI, BSTR* strLocalName, BSTR* strQName);
	void characters(BSTR* strChars);
	void ignorableWhitespace(BSTR* strChars);
	void processingInstruction(BSTR* strTarget, BSTR* strData);
	void skippedEntity(BSTR* strName);
};
/////////////////////////////////////////////////////////////////////////////
// CIVBSAXLocator wrapper class

class CIVBSAXLocator : public COleDispatchDriver
{
public:
	CIVBSAXLocator() {}		// Calls COleDispatchDriver default constructor
	CIVBSAXLocator(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIVBSAXLocator(const CIVBSAXLocator& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	long GetColumnNumber();
	long GetLineNumber();
	CString GetPublicId();
	CString GetSystemId();
};
/////////////////////////////////////////////////////////////////////////////
// CIVBSAXAttributes wrapper class

class CIVBSAXAttributes : public COleDispatchDriver
{
public:
	CIVBSAXAttributes() {}		// Calls COleDispatchDriver default constructor
	CIVBSAXAttributes(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIVBSAXAttributes(const CIVBSAXAttributes& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	long GetLength();
	CString getURI(long nIndex);
	CString getLocalName(long nIndex);
	CString getQName(long nIndex);
	long getIndexFromName(LPCTSTR strURI, LPCTSTR strLocalName);
	long getIndexFromQName(LPCTSTR strQName);
	CString getType(long nIndex);
	CString getTypeFromName(LPCTSTR strURI, LPCTSTR strLocalName);
	CString getTypeFromQName(LPCTSTR strQName);
	CString getValue(long nIndex);
	CString getValueFromName(LPCTSTR strURI, LPCTSTR strLocalName);
	CString getValueFromQName(LPCTSTR strQName);
};
/////////////////////////////////////////////////////////////////////////////
// CIVBSAXDTDHandler wrapper class

class CIVBSAXDTDHandler : public COleDispatchDriver
{
public:
	CIVBSAXDTDHandler() {}		// Calls COleDispatchDriver default constructor
	CIVBSAXDTDHandler(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIVBSAXDTDHandler(const CIVBSAXDTDHandler& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	void notationDecl(BSTR* strName, BSTR* strPublicId, BSTR* strSystemId);
	void unparsedEntityDecl(BSTR* strName, BSTR* strPublicId, BSTR* strSystemId, BSTR* strNotationName);
};
/////////////////////////////////////////////////////////////////////////////
// CIVBSAXErrorHandler wrapper class

class CIVBSAXErrorHandler : public COleDispatchDriver
{
public:
	CIVBSAXErrorHandler() {}		// Calls COleDispatchDriver default constructor
	CIVBSAXErrorHandler(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIVBSAXErrorHandler(const CIVBSAXErrorHandler& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	void error(LPDISPATCH oLocator, BSTR* strErrorMessage, long nErrorCode);
	void fatalError(LPDISPATCH oLocator, BSTR* strErrorMessage, long nErrorCode);
	void ignorableWarning(LPDISPATCH oLocator, BSTR* strErrorMessage, long nErrorCode);
};
/////////////////////////////////////////////////////////////////////////////
// CIVBSAXXMLFilter wrapper class

class CIVBSAXXMLFilter : public COleDispatchDriver
{
public:
	CIVBSAXXMLFilter() {}		// Calls COleDispatchDriver default constructor
	CIVBSAXXMLFilter(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIVBSAXXMLFilter(const CIVBSAXXMLFilter& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	LPDISPATCH GetParent();
	void SetRefParent(LPDISPATCH newValue);
};
/////////////////////////////////////////////////////////////////////////////
// CIVBSAXLexicalHandler wrapper class

class CIVBSAXLexicalHandler : public COleDispatchDriver
{
public:
	CIVBSAXLexicalHandler() {}		// Calls COleDispatchDriver default constructor
	CIVBSAXLexicalHandler(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIVBSAXLexicalHandler(const CIVBSAXLexicalHandler& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	void startDTD(BSTR* strName, BSTR* strPublicId, BSTR* strSystemId);
	void endDTD();
	void startEntity(BSTR* strName);
	void endEntity(BSTR* strName);
	void startCDATA();
	void endCDATA();
	void comment(BSTR* strChars);
};
/////////////////////////////////////////////////////////////////////////////
// CIVBSAXDeclHandler wrapper class

class CIVBSAXDeclHandler : public COleDispatchDriver
{
public:
	CIVBSAXDeclHandler() {}		// Calls COleDispatchDriver default constructor
	CIVBSAXDeclHandler(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIVBSAXDeclHandler(const CIVBSAXDeclHandler& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	void elementDecl(BSTR* strName, BSTR* strModel);
	void attributeDecl(BSTR* strElementName, BSTR* strAttributeName, BSTR* strType, BSTR* strValueDefault, BSTR* strValue);
	void internalEntityDecl(BSTR* strName, BSTR* strValue);
	void externalEntityDecl(BSTR* strName, BSTR* strPublicId, BSTR* strSystemId);
};
/////////////////////////////////////////////////////////////////////////////
// CIMXWriter wrapper class

class CIMXWriter : public COleDispatchDriver
{
public:
	CIMXWriter() {}		// Calls COleDispatchDriver default constructor
	CIMXWriter(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIMXWriter(const CIMXWriter& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	void SetOutput(const VARIANT& newValue);
	VARIANT GetOutput();
	void SetEncoding(LPCTSTR lpszNewValue);
	CString GetEncoding();
	void SetByteOrderMark(BOOL bNewValue);
	BOOL GetByteOrderMark();
	void SetIndent(BOOL bNewValue);
	BOOL GetIndent();
	void SetStandalone(BOOL bNewValue);
	BOOL GetStandalone();
	void SetOmitXMLDeclaration(BOOL bNewValue);
	BOOL GetOmitXMLDeclaration();
	void SetVersion(LPCTSTR lpszNewValue);
	CString GetVersion();
	void SetDisableOutputEscaping(BOOL bNewValue);
	BOOL GetDisableOutputEscaping();
	void flush();
};
/////////////////////////////////////////////////////////////////////////////
// CIMXAttributes wrapper class

class CIMXAttributes : public COleDispatchDriver
{
public:
	CIMXAttributes() {}		// Calls COleDispatchDriver default constructor
	CIMXAttributes(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIMXAttributes(const CIMXAttributes& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	void addAttribute(LPCTSTR strURI, LPCTSTR strLocalName, LPCTSTR strQName, LPCTSTR strType, LPCTSTR strValue);
	void addAttributeFromIndex(const VARIANT& varAtts, long nIndex);
	void clear();
	void removeAttribute(long nIndex);
	void setAttribute(long nIndex, LPCTSTR strURI, LPCTSTR strLocalName, LPCTSTR strQName, LPCTSTR strType, LPCTSTR strValue);
	void setAttributes(const VARIANT& varAtts);
	void setLocalName(long nIndex, LPCTSTR strLocalName);
	void setQName(long nIndex, LPCTSTR strQName);
	void setType(long nIndex, LPCTSTR strType);
	void setURI(long nIndex, LPCTSTR strURI);
	void setValue(long nIndex, LPCTSTR strValue);
};
/////////////////////////////////////////////////////////////////////////////
// CIMXReaderControl wrapper class

class CIMXReaderControl : public COleDispatchDriver
{
public:
	CIMXReaderControl() {}		// Calls COleDispatchDriver default constructor
	CIMXReaderControl(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIMXReaderControl(const CIMXReaderControl& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	void abort();
	void resume();
	void suspend();
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLElementCollection wrapper class

class CIXMLElementCollection : public COleDispatchDriver
{
public:
	CIXMLElementCollection() {}		// Calls COleDispatchDriver default constructor
	CIXMLElementCollection(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLElementCollection(const CIXMLElementCollection& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	long GetLength();
	LPDISPATCH item(const VARIANT& var1, const VARIANT& var2);
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLDocument wrapper class

class CIXMLDocument : public COleDispatchDriver
{
public:
	CIXMLDocument() {}		// Calls COleDispatchDriver default constructor
	CIXMLDocument(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDocument(const CIXMLDocument& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	LPDISPATCH GetRoot();
	CString GetUrl();
	void SetUrl(LPCTSTR lpszNewValue);
	long GetReadyState();
	CString GetCharset();
	void SetCharset(LPCTSTR lpszNewValue);
	CString GetVersion();
	CString GetDoctype();
	LPDISPATCH createElement(const VARIANT& vType, const VARIANT& var1);
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLElement wrapper class

class CIXMLElement : public COleDispatchDriver
{
public:
	CIXMLElement() {}		// Calls COleDispatchDriver default constructor
	CIXMLElement(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLElement(const CIXMLElement& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	CString GetTagName();
	void SetTagName(LPCTSTR lpszNewValue);
	LPDISPATCH GetParent();
	void setAttribute(LPCTSTR strPropertyName, const VARIANT& PropertyValue);
	VARIANT getAttribute(LPCTSTR strPropertyName);
	void removeAttribute(LPCTSTR strPropertyName);
	LPDISPATCH GetChildren();
	long GetType();
	CString GetText();
	void SetText(LPCTSTR lpszNewValue);
	void addChild(LPDISPATCH pChildElem, long lIndex, long lReserved);
	void removeChild(LPDISPATCH pChildElem);
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLElement2 wrapper class

class CIXMLElement2 : public COleDispatchDriver
{
public:
	CIXMLElement2() {}		// Calls COleDispatchDriver default constructor
	CIXMLElement2(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLElement2(const CIXMLElement2& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	CString GetTagName();
	void SetTagName(LPCTSTR lpszNewValue);
	LPDISPATCH GetParent();
	void setAttribute(LPCTSTR strPropertyName, const VARIANT& PropertyValue);
	VARIANT getAttribute(LPCTSTR strPropertyName);
	void removeAttribute(LPCTSTR strPropertyName);
	LPDISPATCH GetChildren();
	long GetType();
	CString GetText();
	void SetText(LPCTSTR lpszNewValue);
	void addChild(LPDISPATCH pChildElem, long lIndex, long lReserved);
	void removeChild(LPDISPATCH pChildElem);
	LPDISPATCH GetAttributes();
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLAttribute wrapper class

class CIXMLAttribute : public COleDispatchDriver
{
public:
	CIXMLAttribute() {}		// Calls COleDispatchDriver default constructor
	CIXMLAttribute(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLAttribute(const CIXMLAttribute& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	CString GetName();
	CString GetValue();
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLDOMSelection wrapper class

class CIXMLDOMSelection : public COleDispatchDriver
{
public:
	CIXMLDOMSelection() {}		// Calls COleDispatchDriver default constructor
	CIXMLDOMSelection(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLDOMSelection(const CIXMLDOMSelection& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	LPDISPATCH GetItem(long index);
	long GetLength();
	LPDISPATCH nextNode();
	void reset();
	CString GetExpr();
	void SetExpr(LPCTSTR lpszNewValue);
	LPDISPATCH GetContext();
	void SetRefContext(LPDISPATCH newValue);
	LPDISPATCH peekNode();
	LPDISPATCH matches(LPDISPATCH pNode);
	LPDISPATCH removeNext();
	void removeAll();
	LPDISPATCH clone();
	VARIANT getProperty(LPCTSTR name);
	void setProperty(LPCTSTR name, const VARIANT& value);
};
/////////////////////////////////////////////////////////////////////////////
// CXMLDOMDocumentEvents wrapper class

class CXMLDOMDocumentEvents : public COleDispatchDriver
{
public:
	CXMLDOMDocumentEvents() {}		// Calls COleDispatchDriver default constructor
	CXMLDOMDocumentEvents(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CXMLDOMDocumentEvents(const CXMLDOMDocumentEvents& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	// method 'ondataavailable' not emitted because of invalid return type or parameter type
	// method 'onreadystatechange' not emitted because of invalid return type or parameter type
};
/////////////////////////////////////////////////////////////////////////////
// CIDSOControl wrapper class

class CIDSOControl : public COleDispatchDriver
{
public:
	CIDSOControl() {}		// Calls COleDispatchDriver default constructor
	CIDSOControl(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIDSOControl(const CIDSOControl& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	LPDISPATCH GetXMLDocument();
	void SetXMLDocument(LPDISPATCH newValue);
	long GetJavaDSOCompatible();
	void SetJavaDSOCompatible(long nNewValue);
	long GetReadyState();
};
/////////////////////////////////////////////////////////////////////////////
// CIXMLHTTPRequest wrapper class

class CIXMLHTTPRequest : public COleDispatchDriver
{
public:
	CIXMLHTTPRequest() {}		// Calls COleDispatchDriver default constructor
	CIXMLHTTPRequest(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIXMLHTTPRequest(const CIXMLHTTPRequest& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

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
// CIServerXMLHTTPRequest wrapper class

class CIServerXMLHTTPRequest : public COleDispatchDriver
{
public:
	CIServerXMLHTTPRequest() {}		// Calls COleDispatchDriver default constructor
	CIServerXMLHTTPRequest(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	CIServerXMLHTTPRequest(const CIServerXMLHTTPRequest& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

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
	void setTimeouts(long resolveTimeout, long connectTimeout, long sendTimeout, long receiveTimeout);
	BOOL waitForResponse(const VARIANT& timeoutInSeconds);
	VARIANT getOption(long option);
	void setOption(long option, const VARIANT& value);
};
