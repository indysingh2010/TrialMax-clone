using System;
using System.Collections;
using System.Xml;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared.Trialmax;

namespace FTI.Shared.Xml
{
	/// <summary>
	/// Objects capable of being written to an Xml log file using the CXmlFile object should support this interface
	/// </summary>
	public interface IXmlObject
	{
		/// <summary>
		/// The function is used to create an Xml node that represents the object
		/// </summary>
		/// <returns>A valid Xml node</returns>
		XmlNode ToXmlNode(XmlDocument xmlDocument);
		
		XmlNode ToXmlNode(XmlDocument xmlDocument, string strName);
	}
	
	/// <summary>This is the base class for TrialMax XML classes</summary>
	public class CXmlBase : IXmlObject
	{
		#region Constants
		
		// This is the default time allotted for a line of transcript text
		//	We use this to assign a value to the stop position when we don't have
		//	a valid value
		protected const double XMLBASE_DEFAULT_TRANSCRIPT_TIME = 1.0;
		
		protected const int ERROR_ADD_ATTRIBUTE_EX	= 0;
		protected const int ERROR_ADD_ELEMENT_EX	= 1;
		protected const int LAST_XML_BASE_ERROR = 1;
		
		#endregion Constants
		
		#region Protected Members
		
		/// <summary>Error builder object used to construct formatted error messages</summary>
		protected CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();		
		
		/// <summary>Local member bound to EventSource property</summary>
		protected CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member bound to UserData property</summary>
		protected object m_userData = null;
		
		/// <summary>Flag to indicate if the node has been modified</summary>
		protected bool m_bModified = false;
		
		#endregion Protected Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CXmlBase()
		{
			//	Populate the error builder
			SetErrorStrings();
		}
		
		/// <summary>Copy constructor</summary>
		/// <param name="xmlSource">Source node to be copied</param>
		public CXmlBase(CXmlBase xmlSource) : base()
		{
			Debug.Assert(xmlSource != null);
			
			if(xmlSource != null)
				Copy(xmlSource);
		}
		
		/// <summary>Called to copy the properties of the source node</summary>
		virtual public void Copy(CXmlBase xmlSource)
		{
			UserData = xmlSource.UserData;
			Modified = xmlSource.Modified;
		}
		
		/// <summary>This method creates an xml node using the object's properties</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		/// <param name="strName">The name assigned to the node</param>
		///	<returns>An Xml node that represents the object</returns>
		public virtual XmlNode ToXmlNode(XmlDocument xmlDocument)
		{
			return ToXmlNode(xmlDocument, null);	
		}
		
		/// <summary>This method creates an xml node using the object's properties</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		/// <param name="strName">The name assigned to the node</param>
		///	<returns>An Xml node that represents the object</returns>
		public virtual XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		{
			return null;
		}
		
		/// <summary>This method will add an attribute to the specified node</summary>
		/// <param name="xmlElement">Element node associated with the attribute</param>
		/// <param name="strName">Name of the attribute</param>
		/// <param name="objValue">Value of the attribute</param>
		/// <returns>true if successful</returns>
		virtual public bool AddAttribute(XmlElement xmlElement, string strName, object objValue)
		{
			if(objValue.GetType() == typeof(bool))
				return AddAttribute(xmlElement, strName, BoolToXml((bool)objValue));
			else
				return AddAttribute(xmlElement, strName, objValue.ToString());

		}// virtual public bool AddAttribute(XmlElement xmlElement, string strName, object objValue)
					
		/// <summary>This method will add an attribute to the specified node</summary>
		/// <param name="xmlElement">Element node associated with the attribute</param>
		/// <param name="strName">Name of the attribute</param>
		/// <param name="objValue">Value of the attribute</param>
		/// <returns>true if successful</returns>
		virtual public bool AddAttribute(XmlElement xmlElement, string strName, string strValue)
		{
			try
			{
				if(strValue != null)
				{
					xmlElement.SetAttribute(strName, strValue);
					return true;
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddAttribute", m_tmaxErrorBuilder.Message(ERROR_ADD_ATTRIBUTE_EX, strName, strValue != null ? strValue : "null"), Ex);
			}
			
			return false;

		}// virtual public bool AddAttribute(XmlElement xmlElement, string strName, string strValue)
					
		/// <summary>This method will add a child element to the specified node</summary>
		/// <param name="xmlDocument">The Xml document that owns the node</param>
		/// <param name="xmlParent">The node to which the new child is added</param>
		/// <param name="strName">The element name</param>
		/// <param name="objValue">The element value</param>
		/// <returns>The new element node if successful</returns>
		virtual public XmlElement AddElement(XmlDocument xmlDocument, XmlNode xmlParent, string strName, object objValue)
		{
			return AddElement(xmlDocument, xmlParent, null, strName, null, objValue);
		}
	
		/// <summary>This method will add a child element to the specified node</summary>
		/// <param name="xmlDocument">The Xml document that owns the node</param>
		/// <param name="xmlParent">The node to which the new child is added</param>
		/// <param name="strName">The element name</param>
		///	<param name="strNamespace">The namespace associated with the element</param>
		/// <param name="objValue">The element value</param>
		/// <returns>The new element node if successful</returns>
		virtual public XmlElement AddElement(XmlDocument xmlDocument, XmlNode xmlParent, string strName, string strNamespace, object objValue)
		{
			return AddElement(xmlDocument, xmlParent, null, strName, strNamespace, objValue);
		}
	
		/// <summary>This method will add a child element to the specified node</summary>
		/// <param name="xmlDocument">The Xml document that owns the node</param>
		/// <param name="xmlParent">The node to which the new child is added</param>
		/// <param name="strPrefix">The node's prefix</param>
		/// <param name="strName">The element name</param>
		///	<param name="strNamespace">The namespace associated with the element</param>
		/// <param name="objValue">The element value</param>
		/// <returns>The new element node if successful</returns>
		virtual public XmlElement AddElement(XmlDocument xmlDocument, XmlNode xmlParent, string strPrefix, string strName, string strNamespace, object objValue)
		{
			XmlElement xmlChild = null;
		
			try
			{
				//	If you supply a prefix you must also have a namespace
				if((strNamespace != null) && (strNamespace.Length > 0))
				{
					if((strPrefix != null) && (strPrefix.Length > 0))
						xmlChild = xmlDocument.CreateElement(strPrefix, strName, strNamespace);
					else
						xmlChild = xmlDocument.CreateElement(strName, strNamespace);
				}
				else
				{
					xmlChild = xmlDocument.CreateElement(strName);
				}
					
				if(xmlChild != null)
				{
					if(objValue != null)
						xmlChild.InnerText = objValue.ToString();
					xmlParent.AppendChild(xmlChild);
					
					return xmlChild;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddElement", m_tmaxErrorBuilder.Message(ERROR_ADD_ELEMENT_EX, strName, objValue != null ? objValue.ToString() : "null"), Ex);
			}
			
			return null;

		}// virtual public XmlElement AddElement(XmlDocument xmlDocument, XmlNode xmlParent, string strPrefix, string strName, string strNamespace, object objValue)
	
		/// <summary>This method converts the attribute to a boolean value</summary>
		/// <param name="strAttribute">The XML attribute to be converted</param>
		/// <returns>The eqivalent boolean value</returns>
		virtual public bool XmlToBool(string strAttribute)
		{
			return CXmlBase.AsBool(strAttribute);
		}
		
		/// <summary>This method retrieves the default display text for the node</summary>
		/// <returns>The default display text string</returns>
		virtual public string GetDisplayString()
		{
			return ToString();
		}
		
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="xmlBase">Object to be compared</param>
		/// <param name="iMode">Mode identifier defined by derived class</param>
		/// <returns>-1 if this less than xmlBase, 0 if equal, 1 if xmlBase greater than this</returns>
		virtual public int Compare(CXmlBase xmlBase, int iMode)
		{
			return -1;
		}
		
		/// <summary>This method converts the boolean value to an XML attribute</summary>
		/// <param name="bValue">The boolean value to be converted</param>
		/// <returns>The eqivalent XML attribute</returns>
		virtual public string BoolToXml(bool bValue)
		{
			return CXmlBase.AsXmlAttribute(bValue);			
		}
		
		/// <summary>This method converts the double value to an XML attribute</summary>
		/// <param name="dValue">The double value to be converted</param>
		/// <returns>The eqivalent XML attribute</returns>
		virtual public string DoubleToXml(double dValue)
		{
			return CXmlBase.AsXmlAttribute(dValue);			
		}
		
		/// <summary>This method converts the boolean value to an XML attribute</summary>
		/// <param name="bValue">The boolean value to be converted</param>
		/// <returns>The eqivalent XML attribute</returns>
		static public string AsXmlAttribute(bool bValue)
		{
			if(bValue == true)
				return "yes";
			else
				return "no";
			
		}// static public string AsXmlAttribute(bool bValue)
		
		/// <summary>This method converts the double value to an XML attribute</summary>
		/// <param name="dValue">The double value to be converted</param>
		/// <returns>The eqivalent XML attribute</returns>
		static public string AsXmlAttribute(double dValue)
		{
			return dValue.ToString("0.###");
		}
		
		/// <summary>This method converts the attribute to a boolean value</summary>
		/// <param name="strAttribute">The XML attribute to be converted</param>
		/// <returns>The eqivalent boolean value</returns>
		static public bool AsBool(string strXmlAttribute)
		{
			if((strXmlAttribute != null) && (strXmlAttribute.Length > 0))
			{
				return (String.Compare(strXmlAttribute, "yes", true) == 0);
			}
			
			return false;
			
		}// static public bool AsBool(string strXmlAttribute)

		/// <summary>This method converts the specified string to an appropriate XPath query string</summary>
		/// <param name="strSource">The desired source string</param>
		/// <returns>The encoded XPath string</returns>
		static public string GetXPathString(string strSource)
		{
			string	strXPath = "";
			string	strSubString = "";
			string	strSearch = strSource;
			char[]	aQuoteChars = new char[] { '\'', '"' };
			int		iToken = -1;

			//	Search for the first occurance of one of the quote characters
			if((iToken = strSearch.IndexOfAny(aQuoteChars)) < 0)
			{
				//	Wrap the source with bounding single quotes
				strXPath = String.Format("'{0}'", strSource);
			}
			else // Must be at lease one quote character
			{
				strXPath = "concat(";
				
				while(iToken >= 0)
				{
					//	Extract the portion up to the quote character
					strSubString = strSearch.Substring(0, iToken);
					strXPath += "'" + strSubString + "', ";
					
					//	Is this a single quote character?
					if(strSearch.Substring(iToken, 1) == "'")
					{
						strXPath += "\"'\", "; // Wrap with double quotes
					}
					else
					{
						strXPath += "'\"', "; // Wrap double quotes with single quotes
					}
					
					//	Locate the next token in the remaining portion
					strSearch = strSearch.Substring(iToken + 1, strSearch.Length - iToken - 1);
					iToken = strSearch.IndexOfAny(aQuoteChars);

				}// while(iToken >= 0)
				
				strXPath += "'" + strSearch + "')";
			}
			
			return strXPath;

		}// static public string GetXPathString(string strSource)

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method will advance the XML text reader to the next node</summary>
		/// <param name="xmlReader">The XML text reader used to open the file</param>
		/// <param name="bSkip">True to skip all children in the current node</param>
		/// <returns>True if successful</returns>
		protected virtual bool AdvanceReader(XmlTextReader xmlReader, bool bSkip)
		{
			Debug.Assert(xmlReader != null);
			if(xmlReader == null) return false;
			
			//	Is the reader already at the end of the file
			if(xmlReader.EOF == true) return false;
			
			//	Should we skip the children in the current node?
			if((bSkip == true) && (xmlReader.NodeType == XmlNodeType.Element))
			{
				//	Skip over children and line up on next node
				xmlReader.Skip();
				return (xmlReader.EOF == false);
			}
			else
			{
				return xmlReader.Read();
			}
			
		}// protected virtual bool AdvanceReader(XmlTextReader xmlReader, bool bSkip)
		
		/// <summary>This method will populate the local error builder's format string collection</summary>
		protected virtual void SetErrorStrings()
		{
			ArrayList aStrings = null;
			
			if(m_tmaxErrorBuilder != null)
				aStrings = m_tmaxErrorBuilder.FormatStrings;
		
			if(aStrings == null) return;
				
			//	The format strings must be added in the order in which they are defined
			aStrings.Add("An exception was raised while attempting to add a new attribute: name = %1 value = %2");
			aStrings.Add("An exception was raised while attempting to add a new element: name = %1 value = %2");

		}
		
		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public CTmaxEventSource EventSource
		{
			get
			{
				return m_tmaxEventSource;
			}
			
		}// EventSource property
		
		/// <summary>Property that allows for user assigned data object</summary>
		public object UserData
		{
			get
			{
				return m_userData;
			}
			set
			{
				m_userData = value;
			}
			
		}// UserData property
		
		/// <summary>Flag to indicate if the node has been modified</summary>
		public bool Modified
		{
			get
			{
				return m_bModified;
			}
			set
			{
				m_bModified = value;
			}
			
		}// Modified property
		
		/// <summary>Property used to display the node</summary>
		public string DisplayString
		{
			get{ return GetDisplayString();	}
			
		}// DisplayString property
		
		#endregion Properties
	
	}// public class CXmlBase
	
}// namespace FTI.Shared.Xml
